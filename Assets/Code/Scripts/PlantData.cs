using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Flags]
public enum SeasonFlags
{
    Spring = 1 << 0,
    Summer = 1 << 1,
    Fall = 1 << 2,
    Invalid = 1 << 3
};

[CreateAssetMenu(fileName = "Plant", menuName = "PlantData")]
public class PlantData : ScriptableObject
{
    public string plantName;

    public SeasonFlags bestSeason;

    [Range(0, 20)]
    public int maxGrowthLevel;

    [Tooltip("Sprites of the plant for each level, 0 - maxGrowthLevel")]
    public List<TileBase> tilesForLevel;
    public List<Sprite> spriteForLevel;

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
