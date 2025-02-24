using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantDatabase : MonoBehaviour
{
    public static PlantDatabase Instance;
    public List<Plant> plants = new List<Plant>();  // 기본값으로 빈 리스트 할당

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            LoadPlantData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void LoadPlantData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("plant_data");

        if (jsonFile != null)
        {
            Debug.Log("JSON 로드 성공!");
            Debug.Log("로드된 JSON: " + jsonFile.text);  // JSON 내용 확인용 로그 추가

            plants = JsonUtility.FromJson<PlantList>("{\"plants\":" + jsonFile.text + "}").plants;

            Debug.Log($"로드된 식물 개수: {plants.Count}");
        }
        else
        {
            Debug.LogError("JSON 파일을 찾을 수 없습니다. plant_data.json이 Resources 폴더에 있는지 확인하세요.");
        }
    }
}