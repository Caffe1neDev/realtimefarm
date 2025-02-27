using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageRotator : MonoBehaviour
{
    public float rotationSpeed = 1.0f;
    public float maxRotationDegree;

    private bool isRotatingCCW = false;
    private RectTransform rectTransform;


    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isRotatingCCW)
        {
            //rectTransform.Rotate(Vector3.forward, );
            rectTransform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
            if(rectTransform.rotation.eulerAngles.z < 180.0f && rectTransform.rotation.eulerAngles.z > maxRotationDegree)
            {
                isRotatingCCW = false;
            }
        }
        else
        {
            rectTransform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
            if(rectTransform.rotation.eulerAngles.z > 180.0f && rectTransform.rotation.eulerAngles.z < (360.0f - maxRotationDegree))
            {
                isRotatingCCW = true;
            }
        }
    }
}
