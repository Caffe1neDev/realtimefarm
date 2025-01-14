using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class MouseSelect : MonoBehaviour
{

    [Header("작물 심는 타일 맵 넣어야 함")]
    public Tilemap tilemap; //작물용 타일
    [Header("ui로 작물 선택된 타일")]
    public TileBase changeTile;
    TileBase selectedTile;
/*    [Header("0은 심는용 커서, 1은 재배용 커서")]
    public Sprite[] cursorSprites;*/

    SpriteRenderer cursor;
    bool isCropField;
    bool isPlanting;
    Vector2 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        cursor = GetComponent<SpriteRenderer>();
        isPlanting = true; //시작은 무조건 심기모드;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CropField"))
        {
            print("범위안");
            isCropField = true;
            //작물 심기 필드일 떄
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("CropField"))
        {
            print("범위밖");
            isCropField = false;
        }
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; //UI 선택X 일때만 작동하도록

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition = new Vector2(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y));
        transform.position = mousePosition;

        //이미 설치한 타일이 있는가 체크
        selectedTile = tilemap.GetTile(new Vector3Int((int)mousePosition.x, (int)mousePosition.y, 0));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPlanting = !isPlanting;

            if (isPlanting)
            {
                print("심는중");
            }
            else
            {
                print("재배중");
            }
            //커서변경
        }

        if (isPlanting) ActivatePlantingMode();
        else ActivateHarvestMode();

    }
    void ActivatePlantingMode()
    {
        if (!isCropField || selectedTile != null)
        {
            //수확 가능한 공간 X 이거 이미지 비활성화 시키는 것도 나쁘지 않을 듯
            cursor.color = new Color(1f, 0.7f, 0.7f, 0.5f);
        }
        else
        {
            cursor.color = new Color(1f, 1f, 1f, 1f);
            //우클릭시
            if (Input.GetMouseButtonUp(0))
            {
                OnClickMouse(changeTile);
            }
        }
    }

    void ActivateHarvestMode()
    {
        if (Input.GetMouseButtonUp(0))
        {
            OnClickMouse(null);
        }
    }

    void OnClickMouse(TileBase myTile)
    {
        //수확
        tilemap.SetTile(new Vector3Int((int)mousePosition.x, (int)mousePosition.y, 0), myTile);
    }

}
