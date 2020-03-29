using UnityEngine;

public class Card : MonoBehaviour
{

public Sprite cardWire;
public Sprite cardEmpty;
public Sprite cardBomb;
public Sprite cardBack;
public string playerId;
public string cardId;
public bool isHidden;

private int cardType;
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

    public void setCardType(int cardType) {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        switch (cardType)
        {
          case 2:
              this.spriteRenderer.sprite = cardBomb;
              this.cardFront = cardBomb;
              break;
         case 1:
              this.spriteRenderer.sprite = cardEmpty;
              this.cardFront = cardEmpty;
              break;
          case 0:
              this.spriteRenderer.sprite = cardWire;
              this.cardFront = cardWire;
              break;
         default:
              break;
        }
    }

}