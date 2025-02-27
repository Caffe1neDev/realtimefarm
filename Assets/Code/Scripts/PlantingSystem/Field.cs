using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public enum FieldStatus
{
    Empty,
    Growing,
    Mature,
    Overgrown
};

public class Field : MonoBehaviour
{
    [SerializeField] private Color overgrownCropColor;
    private PlantData plant;
    private FieldStatus status;
    private float growthTimer;
    private int growthLevel;

    private float growthPeriodRandomizer = 0.05f; // 성장에 필요한 시간 랜덤화 범위

    public delegate void OnFieldSelection(Field field);
    public event OnFieldSelection onFieldSelection = delegate { };

    private SpriteRenderer plantSpriteRenderer;

    void Awake()
    {
        plantSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void UpdateTimer(float deltaTime) 
    {
        if(status == FieldStatus.Empty)
        {
            return;
        }

        growthTimer -= deltaTime;

        if(status == FieldStatus.Growing) // 일반 성장 확인
        {
            if(growthTimer < 0.0f)
            {
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
                plantSpriteRenderer.color = overgrownCropColor;
            }
        }
    }

    private void OnPlantGrowth()
    {
        ++growthLevel;
        plantSpriteRenderer.sprite = plant.spriteForLevel[growthLevel];

        if(growthLevel == 1)
        {
            plantSpriteRenderer.transform.position = this.transform.position;
        }
        else
        {
            // 성장에 따른 sprite 위치 조정
            plantSpriteRenderer.transform.position = new Vector3(plantSpriteRenderer.transform.position.x, 
            plantSpriteRenderer.transform.position.y + (plant.spriteForLevel[growthLevel].bounds.size.y - plant.spriteForLevel[growthLevel - 1].bounds.size.y) / 2.0f,
            plantSpriteRenderer.transform.position.z);
        }
        
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
        plantSpriteRenderer.color = Color.white;

        growthLevel = 0;
        OnPlantGrowth();
    }

    public void Harvest()
    {
        if(status == FieldStatus.Empty)
        {
            return;
        }
    
        // harvest logic
        switch(status)
        {
            case FieldStatus.Growing:
                QuantityManager.Instance.UpdateQuantity(plant.plantId, "underripe", 1);
                break;
            case FieldStatus.Mature:
                QuantityManager.Instance.UpdateQuantity(plant.plantId, "best", 1);
                break;
            case FieldStatus.Overgrown:
                QuantityManager.Instance.UpdateQuantity(plant.plantId, "overripe", 1);
                break;
            default:
                return;
        }

        plant = null;
        status = FieldStatus.Empty;
        plantSpriteRenderer.sprite = null;
        
        //ShowPlantDetails(PlantDatabase.Instance.plants.Find(p => p.id == plantId)); // UI 즉시 업데이트
    }

    private float GetRandomizedGrowthTime()
    {
        return plant.growthTimePerLevel * (1.0f + Random.Range(-growthPeriodRandomizer, growthPeriodRandomizer));
    }

    void OnMouseEnter()
    {
        onFieldSelection.Invoke(this);
    }

    void OnMouseExit()
    {
        onFieldSelection.Invoke(null);
    }
}
