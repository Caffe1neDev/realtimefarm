using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class MouseSelect : MonoBehaviour
{
    [Header("땅 크기")]
    public int x;
    public int y;

    public CropManager cropManager;

    private bool isInventoryOpened;
    // Start is called before the first frame update
    void Start()
    {
        if(cropManager == null)
        {
            cropManager = FindObjectOfType<CropManager>();
        }
        
        isInventoryOpened = false;
    }

    private void Update()
    {
        if(isInventoryOpened) return;
        
        if (Input.GetMouseButtonUp(0))
        {
            ActivatePlantingMode();
        }
        else if(Input.GetMouseButtonUp(1))
        {
            ActivateHarvestMode();
        }
    }
    void ActivatePlantingMode()
    {
        cropManager.Plant();
    }

    void ActivateHarvestMode()
    {
        cropManager.Harvest();
    }

    public void SetCursurPosition(Vector3 position)
    {
        transform.position = position;
    }

    public void ToggleInventory()   
    {
        isInventoryOpened = !isInventoryOpened;
    }
}
