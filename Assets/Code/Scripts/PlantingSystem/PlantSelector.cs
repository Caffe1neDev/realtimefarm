using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlantSelector : MonoBehaviour
{
    [SerializeField] private List<Image> images = new List<Image>();
    [SerializeField] private List<PlantData> plantDatas = new List<PlantData>();
    
    private PlantData selectedPlant;

    [SerializeField] private CropManager cropManager;
    [SerializeField] private Image selectionUI;
    void Start()
    {
        for(int i = 0; i < 8; ++i)
        {
            images[i].sprite = plantDatas[i].plantImage;
        }
    }

    public void OnClickSlot(int slotNumber)
    {
        if(selectedPlant != null && slotNumber == selectedPlant.plantId-1) // 같은거 두번->선택취소
        {
            selectedPlant = null;
            cropManager.SetSelectedPlant(selectedPlant);
            selectionUI.transform.position = new Vector3(-1000, -1000, 0); // 화면 밖으로
            return;
        }

        selectedPlant = plantDatas[slotNumber];
        cropManager.SetSelectedPlant(selectedPlant);
        selectionUI.transform.position = images[slotNumber].transform.position;
    }
}
