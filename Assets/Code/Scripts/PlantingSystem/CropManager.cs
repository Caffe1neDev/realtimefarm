using UnityEngine;

// Generate fields for each crop tile and manage planting & harvesting
public class CropManager : MonoBehaviour
{  
    [SerializeField] private MouseSelect mouseSelect;
    [SerializeField] private Color overgrownCropColor;
    [SerializeField] public AudioSource PlantAudio;
    [SerializeField] public AudioSource HarvestAudio;


    private Field[] fields;    
    private Field selectedField;

    [Header("Test Crop to plant")]
    public PlantData plantData;

    private PlantData selectedPlant;

    // Start is called before the first frame update
    void Start()
    {
        fields = FindObjectsOfType<Field>();
        selectedField = null;
        foreach (Field field in fields)
        {
            field.onFieldSelection += OnFieldSelection;
        }        
    }

    // Update is called once per frame
    public void UpdateTime(float deltaTime)
    {
        foreach (Field field in fields)
        {
            field.UpdateTimer(deltaTime);
        }
    }

    public void OnFieldSelection(Field field)
    {
        selectedField = field;

        if(field != null)
        {
            mouseSelect.SetCursurPosition(field.transform.position);
        }
        else
        {
            mouseSelect.SetCursurPosition(new Vector3(-1000, -1000, 0));
        }
    }

    public void Plant()               
    {
        if(selectedPlant == null || selectedField == null)
        {
            return;
        }

        selectedField.Plant(selectedPlant);
        PlayAudio(PlantAudio);

    }
    public void Harvest()
    {
        if(selectedField == null)
        {
            return;
        }

        if (selectedField.Harvest()) // 성공적으로 수확했을 때만
        {
            PlayAudio(HarvestAudio);
        }
    }

    public void SetSelectedPlant(PlantData plant)
    {
        selectedPlant = plant;
    }

    public void PlayAudio(AudioSource audio){
        audio.Play();
    }
}
