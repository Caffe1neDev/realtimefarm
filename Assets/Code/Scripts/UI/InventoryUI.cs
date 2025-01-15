using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject InventoryPanel;
    bool ActiveInventory = false;

    private void Start() {
        InventoryPanel.SetActive(ActiveInventory);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.I)){
            ActiveInventory = !ActiveInventory;
            InventoryPanel.SetActive(ActiveInventory);
        }
    }
}
