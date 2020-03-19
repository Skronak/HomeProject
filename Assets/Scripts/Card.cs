using UnityEngine;

public class Card : MonoBehaviour
{

public Sprite cardWire;
public Sprite cardEmpty;
public Sprite cardBomb;
public Sprite cardBack;
public string playerId;
public bool isHidden;

private CardTypeEnum cardType;
private Sprite cardFront;
private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (isHidden) {
            spriteRenderer.sprite = cardBack;
        } else {
            spriteRenderer.sprite = cardFront;
        }
    }

    public void setCardType(CardTypeEnum cardType) {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        switch (cardType)
        {
          case CardTypeEnum.Bomb:
              this.spriteRenderer.sprite = cardBomb;
              this.cardFront = cardBomb;
              break;
         case CardTypeEnum.Empty:
              this.spriteRenderer.sprite = cardEmpty;
              this.cardFront = cardEmpty;
              break;
          case CardTypeEnum.Wire:
              this.spriteRenderer.sprite = cardWire;
              this.cardFront = cardWire;
              break;
         default:
              break;
        }
    }

}