using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "Plant", menuName = "PlantData")]
public class PlantData : ScriptableObject
{
    public int plantId;
    public string plantName;
    public Sprite plantImage;

    [Range(1, 8)]
    public int maxGrowthLevel;

    [Tooltip("Sprites of the plant for each level, 0 - maxGrowthLevel")]
    public List<Sprite> spriteForLevel;

    [Range(1.0f, 60.0f)]
    public float growthTimePerLevel;

    void OnValidate()
    {
        // Sprite Count가 기대치와 다르면 개수 조정
        int expectedSpriteCount = maxGrowthLevel + 1;

        while(spriteForLevel.Count < expectedSpriteCount)
        {
            spriteForLevel.Add(null);
        }

        if(spriteForLevel.Count > expectedSpriteCount)
        {
            spriteForLevel.RemoveRange(expectedSpriteCount, spriteForLevel.Count - expectedSpriteCount);
        }
    }
}
