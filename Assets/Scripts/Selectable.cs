using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool isSelected;
    public Card card;

    // Start is called before the first frame update
    void Start()
    {
        card = GetComponent<Card>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown() {
        GetComponent<Card>().isHidden=!card.isHidden;
;
    }

    void OnMouseOver() {
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    void OnMouseExit () {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
