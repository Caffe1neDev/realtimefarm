using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GlobalTimeManager : MonoBehaviour
{
    [Header("UI Displays")]
    public Image progressPointer;
    public Image progressBar;

    private ImageRotator pointerRotator;

    [Header("Global Time Setting")]
    public int gameLengthInDays;
    public int dayLengthInSeconds;

    private float elapsedTime;
    private float elaspedTimeInDay;
    private int currentDay;
    private float progressPointerMovePerSeconds;

    public CropManager cropManager;

    private bool isGamePaused;

    // Start is called before the first frame update
    void Start()
    {
        elapsedTime = 0.0f;
        elaspedTimeInDay = 0.0f;
        currentDay = 1;

        InitializeProgressPointerInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGamePaused) 
            return;

        elapsedTime += Time.deltaTime;
        elaspedTimeInDay += Time.deltaTime;

        if(elaspedTimeInDay >= dayLengthInSeconds)
        {
            elaspedTimeInDay -= dayLengthInSeconds;
            UpdateDay();
        }

        cropManager.UpdateTime(Time.deltaTime);

        progressPointer.rectTransform.Translate(new Vector3(progressPointerMovePerSeconds * Time.deltaTime, 0, 0), Space.World);
        pointerRotator.Rotate(Time.deltaTime);

        // For Testing
        if(Input.GetKeyDown(KeyCode.Y))
            SceneManager.LoadScene("EndScene");
    }

    void UpdateDay()
    {
        if(++currentDay > gameLengthInDays)
        {
            // game over
            SceneManager.LoadScene("EndScene");
        }
        else
        {
            //dayText.text = currentDay + "일차";
        }
    }

    void InitializeProgressPointerInfo()
    {
        int totalGameLength = dayLengthInSeconds * gameLengthInDays;
        progressPointerMovePerSeconds = progressBar.rectTransform.rect.width / totalGameLength;
        float scaler = 1.0f;
        Transform parent = progressPointer.transform.parent;
        while(parent != null)
        {
            scaler *= parent.localScale.x;
            parent = parent.parent;
        }
        progressPointerMovePerSeconds *= scaler;

        pointerRotator = progressPointer.GetComponent<ImageRotator>();
    }

    public void OnPause()
    {
        isGamePaused = true;
    }

    public void OnResume()
    {
        isGamePaused = false;
    }
}
