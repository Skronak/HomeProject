using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUi : MonoBehaviour
{
    public Sprite cardWire;
    public Sprite cardEmpty;
    public Sprite cardBomb;
    public Sprite cardBack;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    public void setCardType(int cardType)
    {
        if (image == null)
            image = GetComponent<Image>();
            
        switch (cardType)
        {
            case 2:
                image.sprite = cardBomb;
                break;
            case 1:
                image.sprite = cardEmpty;
                break;
            case 0:
                image.sprite = cardWire;
                break;
            default:
                break;
        }
    }
}