using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestSummaryManager : MonoBehaviour
{
    private void Start() {
        CalculateTotalEarning();
    }

    private void CalculateTotalEarning()
    {
        QuantityManager quantityManager = QuantityManager.Instance;
        PlantDatabase plantDatabase = FindObjectOfType<PlantDatabase>();

        if(quantityManager == null || plantDatabase == null)
        {
            Debug.LogError("DB가 존재하지 않음");
            return;
        }

        List<QuantityEntry> plantQuantities = quantityManager.GetAllQuantities();
        int totalEarnings = 0;

        Debug.Log("수확결과: ");
        
         foreach (var entry in plantQuantities)
        {
            Plant plant = plantDatabase.GetPlantById(entry.plantId);
            if (plant != null)
            {
                int earnings = (entry.quantity.underripe * plant.price.underripe) +
                               (entry.quantity.overripe * plant.price.overripe) +
                               (entry.quantity.best * plant.price.best);

                totalEarnings += earnings;
                Debug.Log($"{plant.name}: {earnings}원 (U:{entry.quantity.underripe}, O:{entry.quantity.overripe}, B:{entry.quantity.best})");
            }
            else
            {
                Debug.LogError($"Plant ID {entry.plantId}에 해당하는 작물을 찾을 수 없습니다.");
            }
        }
        Debug.Log($"💰 총 수확 수익: {totalEarnings}원");
    }
}
