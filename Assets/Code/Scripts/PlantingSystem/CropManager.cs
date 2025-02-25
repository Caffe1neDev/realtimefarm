using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Tilemaps;

// Generate fields for each crop tile and manage planting & harvesting
public class CropManager : MonoBehaviour
{  
    [SerializeField] private MouseSelect mouseSelect;
    [SerializeField] private Color overgrownCropColor;

    private Field[] fields;    
    private Field selectedField;

    [Header("Test Crop to plant")]
    public PlantData plantData;



    // Start is called before the first frame update
    void Start()
    {
        fields = FindObjectsOfType<Field>();
        selectedField = null;
        foreach (Field field in fields)
        {
            //field.onCropLevelChangeEvent += OnCropLevelChange;
            field.onFieldSelection += OnFieldSelection;
        }        
    }

    // Update is called once per frame
    public void UpdateTime(Season season, float deltaTime)
    {
        foreach (Field field in fields)
        {
            field.UpdateTimer(season, deltaTime);
        }
        
    }

    public void OnCropLevelChange(Vector3Int tilePos, TileBase newTile, bool isOvergrown)
    {
        // if(isOvergrown)
        // {
        //     Debug.Log("Overgrown");
        //     cropTilemap.SetTileFlags(tilePos, TileFlags.None);
        //     cropTilemap.SetColor(tilePos, overgrownCropColor);
        // }
        // else
        // {
        //     cropTilemap.SetTile(tilePos, newTile);
        // }
    }

    public void OnFieldSelection(Field field)
    {
        selectedField = field;

        if(field != null)
        {
            mouseSelect.SetCursurPosition(field.transform.position);
        }
        else
        {
            mouseSelect.SetCursurPosition(Vector3.zero);
        }
    }

    public void Plant(Vector3 mousePosition)               
    {
        if(selectedField == null)
        {
            return;
        }

        selectedField.Plant(plantData);

        // Vector3Int cellPos = cropTilemap.WorldToCell(mousePosition) - cropTilemap.cellBounds.min;
        // fields[cellPos.y][cellPos.x].Plant(plantData);
        Debug.Log("Plant on " + name);
    }
    public void Harvest(Vector3 mousePosition)
    {
        if(selectedField == null)
        {
            return;
        }

        selectedField.Harvest();

        // Vector3Int cellPos = cropTilemap.WorldToCell(mousePosition) - cropTilemap.cellBounds.min;
        // fields[cellPos.y][cellPos.x].Harvest();
        Debug.Log("Harvest on " + name);
    }
}
