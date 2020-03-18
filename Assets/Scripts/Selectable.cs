using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool isSelected;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected) {

        }
    }

    void OnMouseDown() {
        GetComponent<Card>().isHidden=false;
    }

    void OnMouseOver() {
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    void OnMouseExit () {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
