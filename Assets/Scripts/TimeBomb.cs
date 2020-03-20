using System.Collections.Generic;
using UnityEngine;

public class TimeBomb : MonoBehaviour
{

    public static string[] cards = new string[] { "1", "2", "3" };

    public List<Card> deck = new List<Card>();
    public List<GameObject> emptyPlayerSlot;
    public List<Card> discoveredCards = new List<Card>();
    public GameObject playerPrefab;
    public GameObject cardPrefab;
    public GameObject emptySeatPrefab;

    public void startGame() 
    {
    }

    public void GenerateDeck(int turn)
    {
    }

    public void topCard()
    {
    }


    public void getNewHand(List<string> hand)
    {   
        float startPosition = transform.position.x - 3;
        for(int i=0; i<hand.Count; i++) {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(startPosition + 3, transform.position.y, 2), Quaternion.identity);
            if(hand[i].Equals("bomb")){
                newCard.GetComponent<Card>().setCardType(CardTypeEnum.Bomb);
            } else if (hand[i].Equals("wire")) {
                newCard.GetComponent<Card>().setCardType(CardTypeEnum.Wire);
            } else {
                newCard.GetComponent<Card>().setCardType(CardTypeEnum.Empty);
            }
            startPosition = newCard.transform.position.x;
        }

//        addAnotherPlayerHand(emptyPlayerSlotlot[0], hand.Count);

    }

    public void addAnotherPlayerHand(string playerId, int nbCard) {
        float startPosition = transform.position.x - 3;
        for(int i=0; i < nbCard; i++) {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(startPosition + 3, transform.position.y+5, 2), Quaternion.identity);
            newCard.GetComponent<Card>().playerId = playerId;
            startPosition = newCard.transform.position.x;
        }
    }

    public GameObject AddPlayer(string id)
    {
        GameObject location = emptyPlayerSlot[0];
        GameObject gameObject = Instantiate(playerPrefab,  new Vector3(location.transform.position.x, location.transform.position.y, 2),Quaternion.identity);
        gameObject.GetComponent<Player>().textMesh.text = id.ToString(); 

        emptyPlayerSlot.RemoveAt(0);

        return gameObject;
    }

    public void RemovePlayer(Vector3 disconnectedPosition)
    {
        GameObject emptySeat = Instantiate(emptySeatPrefab, disconnectedPosition, Quaternion.identity);
        emptyPlayerSlot.Add(emptySeat);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
