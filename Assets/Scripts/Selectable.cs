using UnityEngine;
using SocketIO;

public class Selectable : MonoBehaviour
{
    public bool isSelected;
    public Card card;
    public SocketIOComponent socket;
    public bool isPlayerCard;

    // Start is called before the first frame update
    void Start()
    {
        card = GetComponent<Card>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnMouseDown()
    {
        if (!isPlayerCard)
        {
            socket.Emit("revealCard", JSONObject.CreateStringObject(card.cardId.ToString()));
        }
    }

    void OnMouseEnter()
    {
        if (!isPlayerCard)
        {
            AddCardHoverEffect();
            socket.Emit("cardHover", JSONObject.CreateStringObject(card.cardId.ToString()));
        }
    }

    void OnMouseExit()
    {
        RemoveCardHoverEffect();
    }

    public void AddCardHoverEffect()
    {
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    public void RemoveCardHoverEffect()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
