using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private TextMesh displayText;
    private float currentTime;
    
    
    // Start is called before the first frame update
    void Start()
    {
        displayText = GetComponent<TextMesh>();

        currentTime = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        UpdateDisplayTime();
    }

    void UpdateDisplayTime()
    {
        if(displayText == null) return;

        int seconds = (int)currentTime;
        int minutes = seconds / 60;
        seconds %= 60;
        
        displayText.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }
}
