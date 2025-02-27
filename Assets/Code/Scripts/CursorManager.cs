using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorManager : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Texture2D AFKCursorTexture;
    public GameObject AFKCursorObject;
    public float AFKReadyTimer = 1.0f;
    public float AFKSetTimer = 1.0f;
    private float AFKTimer;
    private float indicatorDisplayTimer;

    private Vector3 prevMousePosition;
    private bool isAFK;
    private bool isTestingAFK;
    private bool isWaitingIndicator;

    private RectTransform rectTransform;
    private Image AFKIndicator;

    public GlobalTimeManager globalTimeManager;

    void Awake()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        isAFK = false;
        isTestingAFK = false;
        AFKTimer = AFKSetTimer;

        rectTransform = this.GetComponent<RectTransform>();
        AFKIndicator = this.GetComponent<Image>();
    }

    // Start is called before the first frame update
    void Start()
    {
        globalTimeManager.OnPause();
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.position = Input.mousePosition;

        if(prevMousePosition != Input.mousePosition)
        {
            prevMousePosition = Input.mousePosition;

            isAFK = false;
            isTestingAFK = false;
            //Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
            Cursor.visible = true;
            AFKIndicator.enabled = false;
            AFKCursorObject.SetActive(false);
            
            globalTimeManager.OnPause();
        }
        else if(!isTestingAFK)
        {
            AFKTimer = AFKSetTimer + AFKReadyTimer;
            indicatorDisplayTimer = AFKReadyTimer;
            isTestingAFK = true;
            isWaitingIndicator = true;
        }
        else if(!isAFK)
        {  
            AFKTimer -= Time.deltaTime;
            indicatorDisplayTimer -= Time.deltaTime;

            if(isWaitingIndicator && indicatorDisplayTimer <= 0.0) // test if indicator should be displayed
            {
                isWaitingIndicator = false;
                AFKIndicator.enabled = true;
            }
            else if(!isWaitingIndicator && AFKTimer > 0.0) // Update indicator indicator
            {
                AFKIndicator.material.SetFloat("_FillAmount", AFKTimer / AFKSetTimer);
            }
            else if(AFKTimer <= 0.0)
            {
                isAFK = true;
                //Cursor.SetCursor(AFKCursorTexture, Vector2.zero, CursorMode.Auto);
                AFKIndicator.enabled = false;
                AFKCursorObject.SetActive(true);
                Cursor.visible = false;
                
                globalTimeManager.OnResume();
            }
        }
    }
}
