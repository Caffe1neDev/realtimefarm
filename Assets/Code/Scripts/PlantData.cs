using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(fileName = "Plant", menuName = "PlantData")]
public class PlantData : ScriptableObject
{
    public string plantName;
    public Season bestSeason;
    [Range(0, 20)]
    public int maxGrowthLevel;
    [Tooltip("Sprites of the plant for each level, 0 - maxGrowthLevel")]
    public List<Sprite> spritesForLevel;
    [Range(0.0f, 600.0f)]
    public float growthTimePerLevel;

    void OnValidate()
    {
        if(maxGrowthLevel != 0)
        {
            Debug.Assert(spritesForLevel.Capacity == maxGrowthLevel + 1, 
                plantName + "'s max growth level and count of sprites are mismatched! Expected sprites count: " + (maxGrowthLevel + 1));
        }
    }
}
