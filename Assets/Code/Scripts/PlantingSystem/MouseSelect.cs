using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class MouseSelect : MonoBehaviour
{

    [Header("�۹� �ɴ� Ÿ�� �� �־�� ��")]
    public Tilemap tilemap; //�۹��� Ÿ��
    [Header("ui�� �۹� ���õ� Ÿ��")]
    public TileBase changeTile;
    TileBase selectedTile;
/*    [Header("0�� �ɴ¿� Ŀ��, 1�� ���� Ŀ��")]
    public Sprite[] cursorSprites;*/

    SpriteRenderer cursor;
    bool isCropField;
    bool isPlanting;
    Vector2 mousePosition;

    // Start is called before the first frame update
    void Start()
    {
        cursor = GetComponent<SpriteRenderer>();
        isPlanting = true; //������ ������ �ɱ���;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CropField"))
        {
            print("������");
            isCropField = true;
            //�۹� �ɱ� �ʵ��� ��
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("CropField"))
        {
            print("������");
            isCropField = false;
        }
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return; //UI ����X �϶��� �۵��ϵ���

        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mousePosition = new Vector2(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y));
        transform.position = mousePosition;

        //�̹� ��ġ�� Ÿ���� �ִ°� üũ
        selectedTile = tilemap.GetTile(new Vector3Int((int)mousePosition.x, (int)mousePosition.y, 0));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isPlanting = !isPlanting;

            if (isPlanting)
            {
                print("�ɴ���");
            }
            else
            {
                print("�����");
            }
            //Ŀ������
        }

        if (isPlanting) ActivatePlantingMode();
        else ActivateHarvestMode();

    }
    void ActivatePlantingMode()
    {
        if (!isCropField || selectedTile != null)
        {
            //��Ȯ ������ ���� X �̰� �̹��� ��Ȱ��ȭ ��Ű�� �͵� ������ ���� ��
            cursor.color = new Color(1f, 0.7f, 0.7f, 0.5f);
        }
        else
        {
            cursor.color = new Color(1f, 1f, 1f, 1f);
            //��Ŭ����
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
        //��Ȯ
        tilemap.SetTile(new Vector3Int((int)mousePosition.x, (int)mousePosition.y, 0), myTile);
    }

}
