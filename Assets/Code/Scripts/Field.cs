using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Assertions;

public class Field : MonoBehaviour
{
    private PlantData plant;
    private float growthTimer;
    private int growthLevel;
    private SpriteRenderer plantSpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        plant = null;
        
        Debug.Assert(plantSpriteRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>());
    }

    public void UpdateTimer(float deltaTime) // TODO : add seasonal effect
    {
        if(plant == null) 
        {
            return;
        }

        growthTimer += deltaTime;

        if(growthTimer >= plant.growthTimePerLevel)
        {
            growthTimer -= plant.growthTimePerLevel;

            if(growthLevel < plant.maxGrowthLevel) // TODO : implement case of overgrowth
            {
                plantSpriteRenderer.sprite = plant.spritesForLevel[++growthLevel];
            }
        }
    }

    public void Plant(PlantData toPlant)
    {
        if(plant != null)
        {
            return;
        }

        Debug.Log("Planted " + toPlant.plantName + " on " + gameObject.name);
        plant = toPlant;

        growthTimer = 0.0f;
        growthLevel = 0;
        plantSpriteRenderer.sprite = plant.spritesForLevel[0];
    }

    public void Harvest()
    {
        Debug.Log("Harvested " + plant.plantName + " on " + gameObject.name);
    }

    [Header("Test")]
    public PlantData testPlant;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("Test Plant function called");
            // 테스트용 임시 식물심기 기능
            Plant(testPlant);
        }
    }
}
