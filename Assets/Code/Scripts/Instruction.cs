using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Instruction : MonoBehaviour
{
    [SerializeField] private GlobalTimeManager timeManager;
    [SerializeField] private List<GameObject> instructionPages = new List<GameObject>();
    private int currentPage;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(timeManager != null, "Time Manager is not set in Instruction");

        foreach (var page in instructionPages)
        {
            page.SetActive(false);
        }

        currentPage = 0;
        instructionPages[currentPage].SetActive(true);
    }

    public void OnNextPage()
    {
        if (currentPage < instructionPages.Count - 1)
        {
            instructionPages[currentPage].SetActive(false);
            currentPage++;
            instructionPages[currentPage].SetActive(true);
        }
    }

    public void OnPreviousPage()
    {
        if (currentPage > 0)
        {
            instructionPages[currentPage].SetActive(false);
            currentPage--;
            instructionPages[currentPage].SetActive(true);
        }
    }

    public void OnGameStart()
    {
        timeManager.gameObject.SetActive(true);

        instructionPages[currentPage].SetActive(false);
        gameObject.SetActive(false);
    }
}
