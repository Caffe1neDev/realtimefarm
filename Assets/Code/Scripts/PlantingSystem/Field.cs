using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;


public class Field
{
    private PlantData plant;
    private float growthTimer;
    private int growthLevel;
    public Vector3Int tilePos; // used to notify crop level change of current Tile

    public delegate void CropEventHandler(Vector3Int tilePos, TileBase tile);
    public event CropEventHandler onCropLevelChangeEvent = delegate { };

    // public Field(TileBase matchingTile)
    // {
    //     tilePos = Vector3Int.FloorToInt(matchingTile.GetComponent<Transform>().position);
    //     plant = null;
    // }

    public void UpdateTimer(Season currentSeason, float deltaTime) // TODO : add seasonal effect
    {
        if(plant == null) 
        {
            return;
        }

        growthTimer += deltaTime;

        if(growthTimer >= plant.growthTimePerLevel)
        {
            growthTimer -= plant.growthTimePerLevel;

            if(growthLevel < plant.maxGrowthLevel && (((int)plant.bestSeason & (1 << (int)currentSeason)) != 0)) // TODO : implement case of overgrowth
            {
                ++growthLevel;
                onCropLevelChangeEvent.Invoke(tilePos, plant.tilesForLevel[growthLevel]);
            }
        }
    }

    public void Plant(PlantData toPlant)
    {
        if(plant != null)
        {
            return;
        }

        plant = toPlant;

        growthTimer = 0.0f;
        growthLevel = 0;

        onCropLevelChangeEvent.Invoke(tilePos, plant.tilesForLevel[growthLevel]);
    }

    public void Harvest()
    {
        // TODO : add harvest logic
        plant = null;

        onCropLevelChangeEvent.Invoke(tilePos, null);
    }
}
