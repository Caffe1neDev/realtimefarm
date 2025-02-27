using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public enum Season 
{
    Spring, Summer, Fall, Invalid
}

public class GlobalTimeManager : MonoBehaviour
{
    [Header("UI Displays")]
    public Image seasonPointer;
    public Image seasonBar;
    public TMP_Text dayDisplay;
    public Sprite[] seasonSprites = new Sprite[4];

    [Header("Global Time Setting")]
    public Season startSeason;
    public int seasonLengthinDays;
    public int dayLengthInSeconds;

    private float elapsedTime;
    private float elaspedTimeInDay;
    private Season currentSeason;
    private int currentDay;
    private float seasonPointerMovePerSeconds;

    public CropManager cropManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(dayDisplay);

        elapsedTime = 0.0f;
        elaspedTimeInDay = 0.0f;
        currentSeason = startSeason;
        currentDay = 1;

        InitializeSeasonPointerInfo();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        elaspedTimeInDay += Time.deltaTime;

        if(elaspedTimeInDay >= dayLengthInSeconds)
        {
            elaspedTimeInDay -= dayLengthInSeconds;
            currentDay++;

            if(currentDay > seasonLengthinDays)
            {
                UpdateSeason();
            }

            UpdateDayDisplay();
        }

        cropManager.UpdateTime(currentSeason, Time.deltaTime);

        seasonPointer.rectTransform.Translate(new Vector3(seasonPointerMovePerSeconds * Time.deltaTime, 0, 0), Space.World);
        if(Input.GetKeyDown(KeyCode.Y))
            SceneManager.LoadScene("EndScene");
    }

    void UpdateSeason()
    {
        currentDay = 1;
        ++currentSeason;

        if(currentSeason != Season.Invalid)
        {
            //UpdateSeasonDisplay(currentSeason);
        }
        else
        {
            // game over
            SceneManager.LoadScene("EndScene");
        }
    }

    void UpdateDayDisplay()
    {
        dayDisplay.text = SeasonToString(currentSeason) + " " + currentDay + "일";
    }

    void InitializeSeasonPointerInfo()
    {
        int totalGameLength = dayLengthInSeconds * seasonLengthinDays * 3;
        seasonPointerMovePerSeconds = seasonBar.rectTransform.rect.width / totalGameLength;
        float scaler = 1.0f;
        Transform parent = seasonPointer.transform.parent;
        while(parent != null)
        {
            scaler *= parent.localScale.x;
            parent = parent.parent;
        }
        seasonPointerMovePerSeconds *= scaler;
    }

    string SeasonToString(Season season)
    {
        switch(season)
        {
            case Season.Spring:
                return "봄";
            case Season.Summer:
                return "여름";
            case Season.Fall:
                return "가을";
            case Season.Invalid:
                return "Invalid";
            default:
                return "Invalid";
        }
    }
}
