using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandCard : MonoBehaviour
{
    private Vector3 onOverPosition;
    private Vector3 originalPosition;
    private bool isSelected;
    private float moveYBy = 0.5f;
    private float scaleBy = 1.2f;
 
    // Start is called before the first frame update
    void Start()
    {   
        originalPosition = transform.position;
        onOverPosition = new Vector3(originalPosition.x, originalPosition.y + moveYBy, -1);
    }

    void OnMouseEnter() {
        if (!isSelected) {
            transform.position = onOverPosition;
            transform.localScale = new Vector3(scaleBy,scaleBy,scaleBy);
            isSelected = true;
        }
    }

    void OnMouseExit() {
        transform.position = originalPosition;       
        transform.localScale = new Vector3(1f,1f,1f);
        isSelected = false;
    }

    // Update is called once per frame
    void Update()
    {
    }
}