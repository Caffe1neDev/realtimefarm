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
        // Sprite Count가 기대치와 다르면 개수 조정
        int expectedSpriteCount = maxGrowthLevel + 1;

        while(spritesForLevel.Count < expectedSpriteCount)
        {
            spritesForLevel.Add(null);
        }

        if(spritesForLevel.Count > expectedSpriteCount)
        {
            spritesForLevel.RemoveRange(expectedSpriteCount, spritesForLevel.Count - expectedSpriteCount);
        }
    }
}
