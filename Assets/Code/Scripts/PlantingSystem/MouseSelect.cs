using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class MouseSelect : MonoBehaviour
{

    [Header("�۹� �ɴ� Ÿ�� �� �־�� ��")]
    public Tilemap tilemap; //타일맵 타일
    [Header("ui�� �۹� ���õ� Ÿ��")]
    public TileBase changeTile;
    TileBase selectedTile;
/*     [Header("0은 심기용 커서, 1은 수확용 커서")]
    public Sprite[] cursorSprites;*/

    SpriteRenderer cursor;
    bool isCropField;
    bool isPlanting;
    Vector2 mousePosition;

    public CropManager cropManager;

    // Start is called before the first frame update
    void Start()
    {
        cursor = GetComponent<SpriteRenderer>();
        isPlanting = true; //처음엔 심기모드로 시작;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CropField"))
        {
            print("작물밭");
            isCropField = true;
            //교체할 타일이 작물밭인지 확인
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("CropField"))
        {
            print("작물밭 아님");
            isCropField = false;
        }
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; //UI ����X �϶��� �۵��ϵ���

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition = new Vector2(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y));
        transform.position = mousePosition;

        // 이미지 위치에 타일이 있는지 확인
        selectedTile = tilemap.GetTile(new Vector3Int((int)mousePosition.x, (int)mousePosition.y, 0));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPlanting = !isPlanting;

            if (isPlanting)
            {
                print("심기 모드");
            }
            else
            {
                print("수확 모드");
            }
            // 커서 색상 변경
        }

        if (isPlanting) ActivatePlantingMode();
        else ActivateHarvestMode();

    }
    void ActivatePlantingMode()
    {
        if (!isCropField) //  || selectedTile != null)
        {
            //정확한 타일이 아님. 이것은 이미지 비활성화 시키는 코드
            cursor.color = new Color(1f, 0.7f, 0.7f, 0.5f);
        }
        else
        {
            cursor.color = new Color(1f, 1f, 1f, 1f);
            //클릭 이벤트
            if (Input.GetMouseButtonUp(0))
            {
                //OnClickMouse(changeTile);
                cropManager.Plant(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            }
        }
    }

    void ActivateHarvestMode()
    {
        if (!isCropField) // || selectedTile != null)
        {
            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            //OnClickMouse(null);
            cropManager.Harvest(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }

    void OnClickMouse(TileBase myTile)
    {
        //��Ȯ
        tilemap.SetTile(new Vector3Int((int)mousePosition.x, (int)mousePosition.y, 0), myTile);
    }

}
