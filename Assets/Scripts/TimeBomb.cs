using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBomb : MonoBehaviour
{

    public static string[] cards = new string[] { "1", "2", "3" };

    public List<Card> deck = new List<Card>();
    public List<Player> players = new List<Player>();
    public List<Card> discoveredCards = new List<Card>();
    public GameObject cardPrefab;

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
            cardPrefab.GetComponent<Card>().setCardType(hand[i]);
            startPosition = newCard.transform.position.x;
        }

    }

    public void GenerateCharacters()
    {
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
