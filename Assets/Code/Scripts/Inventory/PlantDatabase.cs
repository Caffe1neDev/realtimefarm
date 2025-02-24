using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDatabase : MonoBehaviour
{
    public static PlantDatabase Instance;
    public List<Plant> plants;

    private void Awake() {
        if(Instance == null)
        {
            Instance = this;
            LoadPlantData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadPlantData(){
        TextAsset jsonFile = Resources.Load<TextAsset>("plant_data");
       
        if (jsonFile != null)
        {
            PlantList plantList = JsonUtility.FromJson<PlantList>("{\"plants\":" + jsonFile.text + "}");
            plants = plantList.plants;
        }

        else
        {
            Debug.LogError("jsonfile not found");
        }
    }

    // 테스트용
    private void Start() {
        foreach(var plant in PlantDatabase.Instance.plants)
         {
        Debug.Log($"이름: {plant.name}, 최상 등급 가격: {plant.price.best}");
        }
    }
}
