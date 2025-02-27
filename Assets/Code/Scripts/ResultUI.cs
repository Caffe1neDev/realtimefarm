using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ResultUI : MonoBehaviour
{
    public Transform[] cropUIElements; // Inspector에서 직접 설정 (ResultPanel의 자식들)
    public TextMeshProUGUI totalText; // 총합 텍스트 UI
    public GameObject resultUI;
    private void Start()
    {
        resultUI.SetActive(false);
        
    }

    private void Update() {
        UpdateHarvestUI();
    }
    private void UpdateHarvestUI()
    {
        QuantityManager quantityManager = QuantityManager.Instance;
        PlantDatabase plantDatabase = FindObjectOfType<PlantDatabase>();

        if (quantityManager == null || plantDatabase == null)
        {
            Debug.LogError("❌ QuantityManager 또는 PlantDatabase를 찾을 수 없습니다.");
            return;
        }

        List<QuantityEntry> plantQuantities = quantityManager.GetAllQuantities();
        int totalEarnings = 0;

        for (int i = 0; i < cropUIElements.Length; i++)
        {
            Transform cropUI = cropUIElements[i];
            if (cropUI == null) continue;

            // 🍅 해당 UI에 있는 Text (TMP) 가져오기
            TextMeshProUGUI textComponent = cropUI.Find("Text (TMP)").GetComponent<TextMeshProUGUI>();
            if (textComponent == null) continue;

            // 🌱 QuantityManager에서 해당 작물 데이터 찾기
            if (i < plantQuantities.Count)
            {
                QuantityEntry entry = plantQuantities[i];
                Plant plant = plantDatabase.GetPlantById(entry.plantId);

                if (plant != null)
                {
                    int earnings = (entry.quantity.underripe * plant.price.underripe) +
                                   (entry.quantity.overripe * plant.price.overripe) +
                                   (entry.quantity.best * plant.price.best);

                    totalEarnings += earnings;
                    textComponent.text = $"{earnings}원"; // UI에 업데이트
                }
            }
            else
            {
                textComponent.text = "0원"; // 데이터가 없으면 0 처리
            }
        }

        // 💰 총합 표시
        if (totalText != null)
        {

            totalText.text = $"총합: {totalEarnings}원";
        }
    }

    public void LoadMainMenu(){
        SceneManager.LoadScene("StartScene");
    }
}
