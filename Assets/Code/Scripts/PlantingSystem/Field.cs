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

public class Field : MonoBehaviour
{
    private PlantData plant;
    private FieldStatus status;
    private float growthTimer;
    private int growthLevel;

    private float growthPeriodRandomizer = 0.05f; // 성장에 필요한 시간 랜덤화 범위
    public Vector3Int tilePos; // used to notify crop level change of current Tile

    public delegate void CropGrowthEventHandler(Vector3Int tilePos, TileBase tile, bool isOvergrown);
    public event CropGrowthEventHandler onCropLevelChangeEvent = delegate { };

    public delegate void OnFieldSelection(Field field);
    public event OnFieldSelection onFieldSelection = delegate { };

    private SpriteRenderer plantSpriteRenderer;

    void Awake()
    {
        plantSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void UpdateTimer(Season currentSeason, float deltaTime) // TODO : add seasonal effect
    {
        if(status == FieldStatus.Empty)
        {
            return;
        }

        growthTimer -= deltaTime;

        if(status == FieldStatus.Growing) // 일반 성장 확인
        {
            if(growthTimer < 0.0f && (((int)plant.bestSeason & (1 << (int)currentSeason)) != 0))
            {
                // ++growthLevel;
                // plantSpriteRenderer.sprite = plant.spriteForLevel[growthLevel];
                //onCropLevelChangeEvent.Invoke(tilePos, plant.tilesForLevel[growthLevel], false);
                OnPlantGrowth();
                
                if(growthLevel == plant.maxGrowthLevel)
                {
                    status = FieldStatus.Mature;
                    growthTimer += plant.growthTimePerLevel; // temp : 한단계 성장시간*2 방치시 과성장으로 판단
                }
            }
        }
        else if(status == FieldStatus.Mature) // 과성장 확인
        {        
            if(growthTimer < 0.0f)
            {
                status = FieldStatus.Overgrown;
                onCropLevelChangeEvent.Invoke(tilePos, plant.tilesForLevel[plant.maxGrowthLevel], true);
            }
        }
    }

    private void OnPlantGrowth()
    {
        ++growthLevel;
        plantSpriteRenderer.sprite = plant.spriteForLevel[growthLevel];

        // 성장에 따른 sprite 위치 조정
        plantSpriteRenderer.transform.position = new Vector3(plantSpriteRenderer.transform.position.x, 
        plantSpriteRenderer.transform.position.y + (plant.spriteForLevel[growthLevel].bounds.size.y - plant.spriteForLevel[growthLevel - 1].bounds.size.y) / 2.0f,
        plantSpriteRenderer.transform.position.z);
        
        growthTimer = GetRandomizedGrowthTime();
    }

    public void Plant(PlantData toPlant)
    {
        if(status != FieldStatus.Empty)
        {
            return;
        }

        status = FieldStatus.Growing;
        plant = toPlant;

        growthLevel = 0;
        OnPlantGrowth();

        //onCropLevelChangeEvent.Invoke(tilePos, plant.tilesForLevel[growthLevel], false);
    }

    public void Harvest()
    {
        if(status == FieldStatus.Empty)
        {
            return;
        }

        // TODO : add harvest logic
        switch(status)
        {
            case FieldStatus.Growing:
                // TODO : add immature harvest logic
                break;
            case FieldStatus.Mature:
                // TODO : add harvest logic
                break;
            case FieldStatus.Overgrown:
                // TODO : add overgrown harvest logic
                break;
            default:
                return;
        }

        plant = null;
        status = FieldStatus.Empty;
        plantSpriteRenderer.sprite = null;
        
        //onCropLevelChangeEvent.Invoke(tilePos, null, false);
    }

    private float GetRandomizedGrowthTime()
    {
        return plant.growthTimePerLevel * (1.0f + Random.Range(-growthPeriodRandomizer, growthPeriodRandomizer));
    }

    void OnMouseEnter()
    {
        // TODO : add mouse enter logic
        Debug.Log("Mouse Enter");

        onFieldSelection.Invoke(this);
    }

    void OnMouseExit()
    {
        // TODO : add mouse exit logic
        Debug.Log("Mouse Exit");
        onFieldSelection.Invoke(this);
    }

    public Vector3 GetPlantSpritePosition()
    {
        return plantSpriteRenderer.transform.position;
    }
}
