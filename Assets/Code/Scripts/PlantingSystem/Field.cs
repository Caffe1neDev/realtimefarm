using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public enum FieldStatus
{
    Empty,
    Growing,
    Mature,
    Overgrown
};

public class Field
{
    private PlantData plant;
    private FieldStatus status;
    private float growthTimer;
    private int growthLevel;
    public Vector3Int tilePos; // used to notify crop level change of current Tile

    public delegate void CropGrowthEventHandler(Vector3Int tilePos, TileBase tile, bool isOvergrown);
    public event CropGrowthEventHandler onCropLevelChangeEvent = delegate { };

    // public Field(TileBase matchingTile)
    // {
    //     tilePos = Vector3Int.FloorToInt(matchingTile.GetComponent<Transform>().position);
    //     plant = null;
    // }

    public void UpdateTimer(Season currentSeason, float deltaTime) // TODO : add seasonal effect
    {
        growthTimer += deltaTime;

        if(status == FieldStatus.Growing) // Growth
        {
            if(growthTimer >= plant.growthTimePerLevel)
            {
                growthTimer -= plant.growthTimePerLevel;

                if(growthLevel < plant.maxGrowthLevel 
                    && (((int)plant.bestSeason & (1 << (int)currentSeason)) != 0))
                {
                    ++growthLevel;
                    onCropLevelChangeEvent.Invoke(tilePos, plant.tilesForLevel[growthLevel], false);
                }

                if(growthLevel == plant.maxGrowthLevel)
                {
                    status = FieldStatus.Mature;
                }
            }
        }
        else if(status == FieldStatus.Mature) // 과성장 확인
        {        
            if(growthTimer >= plant.growthTimePerLevel * 2) // temp: 한단계 성장시간*2 방치시 과성장으로 판단
            {
                status = FieldStatus.Overgrown;
                onCropLevelChangeEvent.Invoke(tilePos, plant.tilesForLevel[plant.maxGrowthLevel], true);
            }
        }
    }

    public void Plant(PlantData toPlant)
    {
        if(status != FieldStatus.Empty)
        {
            return;
        }

        status = FieldStatus.Growing;
        plant = toPlant;

        growthTimer = 0.0f;
        growthLevel = 0;

        onCropLevelChangeEvent.Invoke(tilePos, plant.tilesForLevel[growthLevel], false);
    }

    public void Harvest()
    {
        // TODO : add harvest logic
        plant = null;
        status = FieldStatus.Empty;
        
        onCropLevelChangeEvent.Invoke(tilePos, null, false);
    }
}
