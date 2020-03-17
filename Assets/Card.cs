using UnityEngine;

public class Card : MonoBehaviour
{

public Sprite frontImage;
public Sprite backImage;
public CardTypeEnum cardType;

    public Card(CardTypeEnum type) {
        this.cardType = type;
    }

}
