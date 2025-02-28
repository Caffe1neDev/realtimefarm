using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    public Transform[] cropUIElements; // Inspector에서 직접 설정 (ResultPanel의 자식들)
    public TextMeshProUGUI totalText; // 총합 텍스트 UI
    [SerializeField] private AudioSource clickAudio;
    
    private void Update()
    {
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

            // 🍅 "Image" 오브젝트 찾기
            Transform imageTransform = cropUI.Find("Image");
            if (imageTransform == null)
            {
                Debug.LogError($"❌ {cropUI.name}의 'Image' 오브젝트를 찾을 수 없습니다.");
                continue;
            }

            // 📜 "Image" 내부에서 "Text (TMP)" 찾기
            Transform textTransform = imageTransform.Find("Text (TMP)");
            if (textTransform == null)
            {
                Debug.LogError($"❌ {cropUI.name}의 'Image' 내부에 'Text (TMP)'가 존재하지 않습니다.");
                continue;
            }

            // 🎯 TextMeshProUGUI 컴포넌트 가져오기
            TextMeshProUGUI textComponent = textTransform.GetComponent<TextMeshProUGUI>();
            if (textComponent == null)
            {
                Debug.LogError($"❌ {cropUI.name}의 'Text (TMP)'에서 TextMeshProUGUI 컴포넌트를 찾을 수 없습니다.");
                continue;
            }

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
                else
                {
                    Debug.LogError($"❌ Plant ID {entry.plantId}에 해당하는 식물이 없습니다.");
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

    public void DeleteDB()
    {
        GameObject dbObject = GameObject.Find("DB");
        if (dbObject != null)
        {
            Destroy(dbObject);
            Debug.Log("✅ 'DB' 오브젝트 삭제 완료");
        }
        else
        {
            Debug.LogWarning("⚠️ 'DB' 오브젝트를 찾을 수 없습니다.");
        }
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("StartScene");
        QuantityManager quantityManager = QuantityManager.Instance;
        quantityManager.Suicide();
        PlantDatabase plantDatabase = FindObjectOfType<PlantDatabase>();
        plantDatabase.Suicide();
        DeleteDB();
    }

    public void PlayClickAudio()
    {
        clickAudio.Play();
    }

}
