using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "Plant", menuName = "PlantData")]
public class PlantData : ScriptableObject
{
    public string plantName;
    public Season bestSeason;
    [Range(0, 20)]
    public int maxGrowthLevel;
    [Tooltip("Sprites of the plant for each level, 0 - maxGrowthLevel")]
    public List<TileBase> tilesForLevel;
    [Range(0.0f, 600.0f)]
    public float growthTimePerLevel;

    void OnValidate()
    {
        // Sprite Count가 기대치와 다르면 개수 조정
        int expectedSpriteCount = maxGrowthLevel + 1;

        while(tilesForLevel.Count < expectedSpriteCount)
        {
            tilesForLevel.Add(null);
        }

        if(tilesForLevel.Count > expectedSpriteCount)
        {
            tilesForLevel.RemoveRange(expectedSpriteCount, tilesForLevel.Count - expectedSpriteCount);
        }
    }
}
