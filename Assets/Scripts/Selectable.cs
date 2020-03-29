using UnityEngine;
using SocketIO;

public class Selectable : MonoBehaviour
{
    public bool isSelected;
    public Card card;
   	public SocketIOComponent socket;

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
        socket.Emit("revealCard", JSONObject.CreateStringObject(card.cardId.ToString()));
    }

    void OnMouseEnter() {
        AddCardHoverEffect();
        socket.Emit("cardHover", JSONObject.CreateStringObject(card.cardId.ToString()));
    }

    void OnMouseExit () {
        RemoveCardHoverEffect();
    }

    public void AddCardHoverEffect() {
        GetComponent<SpriteRenderer>().color = Color.yellow;
    }

    public void RemoveCardHoverEffect() {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
