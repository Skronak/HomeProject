using UnityEngine;

public class Card : MonoBehaviour
{

public Sprite cardWire;
public Sprite cardEmpty;
public Sprite cardBomb;
public Sprite cardBack;
public bool isHidden;
private CardTypeEnum cardType;

private Sprite cardFront;
private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        switch (cardType)
        {
          case CardTypeEnum.Bomb:
              spriteRenderer.sprite = cardBomb;
              cardFront = cardBomb;
              break;
         case CardTypeEnum.Empty:
              spriteRenderer.sprite = cardEmpty;
              cardFront = cardEmpty;
              break;
          case CardTypeEnum.Wire:
              spriteRenderer.sprite = cardWire;
              cardFront = cardWire;
              break;
         default:
              break;
        }
    }

    void setCardType(CardTypeEnum cardType) {

    }
    void Update()
    {
        if (isHidden) {
            spriteRenderer.sprite = cardBack;
        } else {
            spriteRenderer.sprite = cardFront;
        }
    }

}
