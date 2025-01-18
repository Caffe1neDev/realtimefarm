using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum Season 
{
    Spring, Summer, Fall, Invalid
}
public class GlobalTimeManager : MonoBehaviour
{
    [Header("UI Displays")]
    public SpriteRenderer seasonDisplay;
    public TextMesh dayDisplay;
    public Timer globalTimer;
    public Sprite[] seasonSprites = new Sprite[4];

    [Header("Global Time Setting")]
    public Season startSeason;
    public int seasonLengthinDays;
    public int dayLengthInSeconds;

    private float elapsedTime;
    private float elaspedTimeInDay;
    private Season currentSeason;
    private int currentDay;

    public CropManager cropManager;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(globalTimer);

        elapsedTime = 0.0f;
        elaspedTimeInDay = 0.0f;
        currentSeason = startSeason;
        currentDay = 1;
        UpdateSeasonDisplay(currentSeason);
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
    }

    void UpdateSeason()
    {
        currentDay = 1;
        ++currentSeason;

        if(currentSeason != Season.Invalid)
        {
            UpdateSeasonDisplay(currentSeason);
        }
        else
        {
            // game over
        }
    }

    void UpdateDayDisplay()
    {
        dayDisplay.text = "Day " + currentDay.ToString("D2");
    }

    void UpdateSeasonDisplay(Season newSeason)
    {
        seasonDisplay.sprite = seasonSprites[(int)newSeason];
    }
}
