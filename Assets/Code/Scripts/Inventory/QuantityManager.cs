using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class QuantityEntry
{
    public int plantId;  // 식물 ID
    public QuantityData quantity;  // 수확 개수 데이터
}

[Serializable]
public class QuantityData
{
    public int underripe = 0;  // 덜익음 개수
    public int overripe = 0;   // 너무익음 개수
    public int best = 0;       // 최상 개수
}

public class QuantityManager : MonoBehaviour
{
    public static QuantityManager Instance;

    [SerializeField]
    private List<QuantityEntry> plantQuantities = new List<QuantityEntry>();  // Inspector에서 확인 가능

    private Dictionary<int, QuantityData> plantQuantitiesDict = new Dictionary<int, QuantityData>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            InitializeDictionary();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDictionary()
    {
        plantQuantitiesDict.Clear();
        foreach (var entry in plantQuantities)
        {
            plantQuantitiesDict[entry.plantId] = entry.quantity;
        }
    }

    public void InitializeQuantities(List<Plant> plants)
    {
        foreach (var plant in plants)
        {
            if (!plantQuantitiesDict.ContainsKey(plant.id))
            {
                QuantityEntry newEntry = new QuantityEntry { plantId = plant.id, quantity = new QuantityData() };
                plantQuantities.Add(newEntry);
                plantQuantitiesDict[plant.id] = newEntry.quantity;
            }
        }
    }

    public void UpdateQuantity(int plantId, string grade, int amount)
    {
        if (!plantQuantitiesDict.ContainsKey(plantId))
        {
            Debug.LogError($"❌ 식물 ID {plantId}가 존재하지 않습니다!");
            return;
        }

        switch (grade)
        {
            case "underripe":
                plantQuantitiesDict[plantId].underripe += amount;
                break;
            case "overripe":
                plantQuantitiesDict[plantId].overripe += amount;
                break;
            case "best":
                plantQuantitiesDict[plantId].best += amount;
                break;
            default:
                Debug.LogError($"❌ 잘못된 등급: {grade}");
                break;
        }

        // 리스트 데이터 동기화
        foreach (var entry in plantQuantities)
        {
            if (entry.plantId == plantId)
            {
                entry.quantity = plantQuantitiesDict[plantId];
                break;
            }
        }

        Debug.Log($"📊 {plantId}번 식물({grade}) 개수 업데이트: {plantQuantitiesDict[plantId].underripe} / {plantQuantitiesDict[plantId].overripe} / {plantQuantitiesDict[plantId].best}");
    }

    public QuantityData GetQuantity(int plantId)
    {
        if (plantQuantitiesDict.ContainsKey(plantId))
        {
            return plantQuantitiesDict[plantId];
        }

        Debug.LogError($"❌ 수확 데이터가 존재하지 않음 (ID: {plantId})");
        return new QuantityData();
    }
}