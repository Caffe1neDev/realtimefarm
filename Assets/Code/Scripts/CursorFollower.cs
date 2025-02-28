using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollower : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector3 cameraSize;
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        cameraSize = new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, 0);

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        rectTransform.localPosition = (Input.mousePosition - cameraSize/2);
    }
}
