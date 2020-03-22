﻿using System.Collections.Generic;
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
    public GameObject playerCardSpawn;
    public GameObject playerHandSpawn;
    public GameObject[] rolesImage;
    public Dictionary<string, GameObject> playersMap;
    public GameObject playersContainer;


    void Start()
    {
        playersMap = new Dictionary<string, GameObject>();
    }

    public void startGame()
    {
    }

    public void GenerateDeck(int turn)
    {
    }

    public void topCard()
    {
    }


    public void GeneratePlayerHand(List<string> hand)
    {
        Vector3 startHandPosition = playerHandSpawn.transform.position;
        Vector3 startPreviewPosition = playerCardSpawn.transform.position;

        for (int i = 0; i < hand.Count; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, new Vector3(startHandPosition.x, startHandPosition.y, 2), Quaternion.identity);
            newCard.GetComponent<Selectable>().enabled = false;

            if (hand[i].Equals("bomb"))
            {
                newCard.GetComponent<Card>().setCardType(CardTypeEnum.Bomb);
            }
            else if (hand[i].Equals("wire"))
            {
                newCard.GetComponent<Card>().setCardType(CardTypeEnum.Wire);
            }
            else
            {
                newCard.GetComponent<Card>().setCardType(CardTypeEnum.Empty);
            }
            startHandPosition.x -= 2;

            // simule cartes que les autres joueurs voient: TODO a melanger
            GameObject cardPreview = Instantiate(cardPrefab, new Vector3(startPreviewPosition.x, startPreviewPosition.y, 3), Quaternion.identity);
            cardPreview.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            cardPreview.GetComponent<Card>().isHidden = true;
            cardPreview.GetComponent<Selectable>().enabled = false;
            startPreviewPosition.x += 2;
        }
    }

    public void GenerateOtherPlayersHand(int nbCard)
    {
        foreach (KeyValuePair<string, GameObject> player in playersMap)
        {
            Vector3 lastPosition = player.Value.transform.GetChild(0).gameObject.transform.position; // accede a CardSpawn du prefab...

            for (int i = 0; i < nbCard; i++)
            {
                GameObject newCard = Instantiate(cardPrefab, new Vector3(lastPosition.x, lastPosition.y, 2), Quaternion.identity);
                newCard.GetComponent<Card>().isHidden = true;
                lastPosition.x = lastPosition.x + 2; // magic number
                newCard.GetComponent<Card>().playerId = player.Key;
            }
        }
    }

    public void AddPlayer(string id, string pseudo)
    {

        if (!playersMap.ContainsKey(id)) {			
            GameObject location = emptyPlayerSlot[0];
            GameObject newPlayer = Instantiate(playerPrefab, new Vector3(location.transform.position.x, location.transform.position.y, 2), Quaternion.identity);
            Player player = newPlayer.GetComponent<Player>();
            player.nameTextMesh.text = pseudo.Replace("\"", string.Empty);

            emptyPlayerSlot.RemoveAt(0);
            newPlayer.name = id;
            newPlayer.transform.parent = playersContainer.transform;
            playersMap.Add(id, newPlayer);
        }
    }

    public void RemovePlayer(string id)
    {
        GameObject go = playersMap[id];
        Vector3 disconnectedPosition = playersMap[id].transform.position;
        Destroy(go);
        playersMap.Remove(id);

        GameObject emptySeat = Instantiate(emptySeatPrefab, disconnectedPosition, Quaternion.identity);
        emptyPlayerSlot.Add(emptySeat);
    }

    public void showRole(string role) {
        if (role.Equals("\"moriarty\"")) {
            (rolesImage[0]).active = false;
        }
        else {
            (rolesImage[1]).active = false;
        }
    }
    
    // Update is called once per frame
    void Update()
    {

    }
}
