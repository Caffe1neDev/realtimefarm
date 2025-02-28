using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

enum CursorState
{
    Default,
    PreChecking, // 마우스가 움직이지 않으면서, Check Indicator가 나타나기 전
    Checking, // Check Indicator가 표시되는 중
    AFK,
    InventoryOpened,
    NotInGame
}

public class CursorManager : MonoBehaviour
{
    public GameObject AFKIndicatorObject;
    public GameObject AFKCheckerObject;
    public float AFKReadyTimer = 1.0f;
    public float AFKSetTimer = 1.0f;

    /*
        AFK Checkers
    */
    private Vector3 prevMousePosition;
    private float AFKTimer;
    private float indicatorDisplayTimer;

    private RectTransform rectTransform;
    private Image cursorImage;
    private CursorState cursorState;

    private CursorState prevState;

    public GlobalTimeManager globalTimeManager;

    void Awake()
    {
        rectTransform = this.GetComponent<RectTransform>();
        cursorImage = GetComponent<Image>();

        cursorState = CursorState.NotInGame;

        Cursor.visible = false;

        AFKIndicatorObject.SetActive(false);
        AFKCheckerObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(globalTimeManager != null)
        {
            globalTimeManager.OnPause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.position = Input.mousePosition;

        if(cursorState == CursorState.NotInGame || cursorState == CursorState.InventoryOpened)
        {
            return;
        }

        if(prevMousePosition != Input.mousePosition)
        {
            LeaveAFK();
        }
        else if(cursorState == CursorState.Default)
        {
            EnterCheckAFK();
        }
        else if(cursorState == CursorState.PreChecking || cursorState == CursorState.Checking)
        {
            CheckAFK();
        }
    }

    void EnterCheckAFK()
    {
        cursorState = CursorState.PreChecking;

        AFKTimer = AFKSetTimer + AFKReadyTimer;
        indicatorDisplayTimer = AFKReadyTimer;
    }

    void CheckAFK()
    {
        AFKTimer -= Time.deltaTime;
        indicatorDisplayTimer -= Time.deltaTime;

        if(cursorState == CursorState.PreChecking)
        {
            if(indicatorDisplayTimer <= 0.0)
            {
                cursorState = CursorState.Checking;
                AFKCheckerObject.SetActive(true);
                AFKCheckerObject.GetComponent<Image>().material.SetFloat("_FillAmount", 0.0001f);
            }
        }
        else
        {
            if(AFKTimer <= 0.0)
            {
                EnterAFK();
            }
            else
            {
                AFKCheckerObject.GetComponent<Image>().material.SetFloat("_FillAmount", AFKTimer / AFKSetTimer);
            }
        }
    }

    void EnterAFK()
    {
        cursorState = CursorState.AFK;
        AFKCheckerObject.SetActive(false);
        AFKIndicatorObject.SetActive(true);
        
        globalTimeManager.OnResume();
    }

    void LeaveAFK()
    {
        cursorState = CursorState.Default;

        prevMousePosition = Input.mousePosition;

        AFKCheckerObject.SetActive(false);
        AFKIndicatorObject.SetActive(false);
        
        globalTimeManager.OnPause();
    }

    public void OnGameStart()
    {
        cursorState = CursorState.Default;
    }

    public void OnToggleInventory()
    {
        if(cursorState != CursorState.InventoryOpened)
        {
            if(cursorState == CursorState.AFK)
            {
                globalTimeManager.OnPause();
            }

            prevState = cursorState;
            cursorState = CursorState.InventoryOpened;

            AFKCheckerObject.SetActive(false);
            AFKIndicatorObject.SetActive(false);
        }
        else
        {
            if(prevState == CursorState.AFK)
            {
                globalTimeManager.OnResume();
                AFKIndicatorObject.SetActive(true);
            }
            else if(prevState == CursorState.Checking)
            {
                AFKCheckerObject.SetActive(true);
            }
            cursorState = prevState;
        }
    }
}
