using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Tilemaps;

// Generate fields for each crop tile and manage planting & harvesting
public class CropManager : MonoBehaviour
{  
    [SerializeField] private Tilemap cropTilemap;
    [SerializeField] private Color overgrownCropColor;

    private Field[][] fields;

    [Header("Test Crop to plant")]
    public PlantData plantData;

    

    // Start is called before the first frame update
    void Start()
    {
        fields = new Field[cropTilemap.cellBounds.size.y][];

        for(int i = 0; i < cropTilemap.cellBounds.size.y; i++)
        {
            fields[i] = new Field[cropTilemap.cellBounds.size.x];

            for(int j = 0; j < cropTilemap.cellBounds.size.x; j++)
            {
                fields[i][j] = new Field();

                Vector3 worldPos = cropTilemap.CellToWorld(new Vector3Int(cropTilemap.cellBounds.x + j * (int)cropTilemap.cellSize.x, 
                                            cropTilemap.cellBounds.y + i * (int)cropTilemap.cellSize.y, 0)) - cropTilemap.cellSize / 2;
                
                fields[i][j].tilePos = new Vector3Int(Mathf.RoundToInt(worldPos.x), Mathf.RoundToInt(worldPos.y), 0);
            }
        }

        Debug.Log($"Tilemap size x : {cropTilemap.cellBounds.size.x} y : {cropTilemap.cellBounds.size.y}, \n" +
              $"Cellbounds x : {cropTilemap.cellBounds.x} y : {cropTilemap.cellBounds.y}, \n" +
              $"Created {cropTilemap.cellBounds.size.y * cropTilemap.cellBounds.size.x} fields in total");

        foreach (Field[] row in fields)
        {
            foreach (Field field in row)
            {
                field.onCropLevelChangeEvent += OnCropLevelChange;
            }
        }
    }

    // Update is called once per frame
    public void UpdateTime(Season season, float deltaTime)
    {
        foreach (Field[] row in fields)
        {
            foreach (Field field in row)
            {
                field.UpdateTimer(season, deltaTime);
            }
        }
    }

    public void OnCropLevelChange(Vector3Int tilePos, TileBase newTile, bool isOvergrown)
    {
        if(isOvergrown)
        {
            Debug.Log("Overgrown");
            cropTilemap.SetTileFlags(tilePos, TileFlags.None);
            cropTilemap.SetColor(tilePos, overgrownCropColor);
        }
        else
        {
            cropTilemap.SetTile(tilePos, newTile);
        }
    }

    public void Plant(Vector3 mousePosition)               
    {

        Vector3Int cellPos = cropTilemap.WorldToCell(mousePosition) - cropTilemap.cellBounds.min;
        fields[cellPos.y][cellPos.x].Plant(plantData);
        Debug.Log("Plant on " + cellPos);
    }
    public void Harvest(Vector3 mousePosition)
    {
        Vector3Int cellPos = cropTilemap.WorldToCell(mousePosition) - cropTilemap.cellBounds.min;
        fields[cellPos.y][cellPos.x].Harvest();
        Debug.Log("Harvest on " + cellPos);
    }
}
