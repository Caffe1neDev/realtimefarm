using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Season 
{
    Spring, Summer, Fall, Invalid
}
public class GlobalTimeManager : MonoBehaviour
{
    public Field[] fields;

    [Header("Global Time Setting")]
    public Timer globalTimer;
    public Season startSeason;
    [Tooltip("The length of each season in seconds")]
    public float seasonLength;

    private float elapsedTime;
    private Season currentSeason;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Assert(globalTimer);

        elapsedTime = 0.0f;
        currentSeason = startSeason;

        // 현재 씬 전체 field 저장
        fields = FindObjectsOfType<Field>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;

        foreach(Field field in fields)
        {
            field.UpdateTimer(Time.deltaTime);
        }
    }
}
