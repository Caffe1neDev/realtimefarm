using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject InventoryPanel;   // 인벤토리 패널
    public GameObject DetailPanel;      // 디테일 패널
    //public GameObject PlantInfoPanel;
    public GameObject[] PlantSlots;     // 9개의 고정된 슬롯 배열
    public GameObject[] PlantInfoSlots = new GameObject[4];

    public TMP_Text DetailName;         // 상세 정보 - 식물 이름
    public Image DetailImage;           // 상세 정보 - 식물 이미지
    public TMP_Text DescriptionBox;     // 상세 정보 - 설명

    // 등급별 개수 UI 요소
    public TMP_Text UnripeQuantity;   
    public TMP_Text OverripeQuantity;
    public TMP_Text BestQuantity;

    private bool ActiveInventory = false;

    private void Start()
    {
        InventoryPanel.SetActive(ActiveInventory);
        //PlantInfoPanel.SetActive(false);
        DetailPanel.SetActive(false);
        LoadPlants();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ActiveInventory = !ActiveInventory;
            InventoryPanel.SetActive(ActiveInventory);
        }
    }

    void LoadPlants()
    {
        if (PlantDatabase.Instance == null)
        {
            Debug.LogError("❌ PlantDatabase.Instance가 null입니다! PlantDatabase 스크립트를 확인하세요.");
            return;
        }

        if (PlantDatabase.Instance.plants == null || PlantDatabase.Instance.plants.Count == 0)
        {
            Debug.LogError("❌ Plant 데이터가 없습니다! JSON이 올바르게 로드되었는지 확인하세요.");
            return;
        }

        Debug.Log($"✅ {PlantDatabase.Instance.plants.Count}개의 식물을 인벤토리에 로드합니다!");

        for (int i = 0; i < PlantSlots.Length; i++)
        {
            if (i < PlantDatabase.Instance.plants.Count)
            {
                Plant plant = PlantDatabase.Instance.plants[i];
                Debug.Log($"🌱 슬롯 {i}에 {plant.name} 추가");

                //TMP_Text plantName = PlantSlots[i].transform.Find("PlantName").GetComponent<TMP_Text>();
                //Image plantImage = PlantSlots[i].transform.Find("PlantImage").GetComponent<Image>();

                DetailName.text = plant.name;
                //plantImage.sprite = Resources.Load<Sprite>("Images/" + plant.image);

                // 클릭 이벤트 추가
                int index = i;
                PlantSlots[i].GetComponent<Button>().onClick.AddListener(() => ShowPlantDetails(PlantDatabase.Instance.plants[index]));
            }
            else
            {
                Debug.Log($"🔲 슬롯 {i} 숨김 (데이터 부족)");
                PlantSlots[i].SetActive(false);
            }
        }
    }

    void ShowPlantDetails(Plant plant)
    {
        DetailPanel.SetActive(true);
        ShowPlantInfo(plant);

        // 스프라이트 로드
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/" + plant.sprite_name);
        DetailImage.sprite = sprites[sprites.Length - 1];  // 마지막 스프라이트 적용

        // 개수 가져오기
        QuantityData quantity = QuantityManager.Instance.GetQuantity(plant.id);
        Debug.Log($"📊 {plant.id}번 식물 개수 업데이트: {quantity.underripe} / {quantity.overripe} / {quantity.best}");

        // UI 강제 업데이트
        UnripeQuantity.text = $"x{quantity.underripe}";
        OverripeQuantity.text = $"x{quantity.overripe}";
        BestQuantity.text = $"x{quantity.best}";
        // 나머지 정보 업데이트
        DetailName.text = plant.name;
        DescriptionBox.text = plant.description;
        // 강제로 UI를 갱신하는 코드 추가
        Canvas.ForceUpdateCanvases();
    }


    void ShowPlantInfo(Plant plant)
    {
        
        Sprite[] sprites = Resources.LoadAll<Sprite>("Images/" + plant.sprite_name);
        for(int i = 0; i<4; i++)
        {
            Image slotImage = PlantInfoSlots[i].transform.Find("Image").GetComponent<Image>();
            slotImage.sprite = sprites[i];
        }
    }
}

