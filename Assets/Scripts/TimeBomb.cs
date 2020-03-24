using System.Collections.Generic;
using UnityEngine;

public class TimeBomb : MonoBehaviour
{

    public List<GameObject> emptyPlayerSlot;
    public List<Card> discoveredCards = new List<Card>();
    public GameObject playerAvatarPrefab;
    public GameObject playerHandCardPrefab;
    public GameObject emptySeatPrefab;
    public GameObject playerCardSpawn;
    public GameObject playerHandSpawn;
    public GameObject[] playerHandCardsSpawn;
    private List<GameObject> currentPlayerHandCards;
    public GameObject[] rolesImage;
    public Dictionary<string, GameObject> playerMap;
    public GameObject playersContainer;
    public GameObject consoleGameobject;
    private CustomConsole console;
    
    void Start()
    {
        currentPlayerHandCards = new List<GameObject>();
        playerMap = new Dictionary<string, GameObject>();
        console = consoleGameobject.GetComponent<CustomConsole>();
    }

    public void startGame()
    {
        console.sendMessageToConsole("system", "Game start");
    }

    public void GenerateDeck(int turn)
    {
    }

    public void topCard()
    {
    }


    public void GeneratePlayerHand(PlayerCards playerCards) {
        cleanHand();
        for (int i = 0; i < 5; i++)
        {
            PlayerCard playerCard = playerCards.hand[i];
            Transform spawnToReplace = playerHandCardsSpawn[i].transform;
            GameObject playerCardGO = Instantiate(playerHandCardPrefab, new Vector3(spawnToReplace.position.x, spawnToReplace.position.y, 2), spawnToReplace.rotation);
            playerCardGO.GetComponent<Card>().setCardType(playerCard.value);
            playerCardGO.transform.parent = spawnToReplace;
            currentPlayerHandCards.Add(playerCardGO);
        }
    }

    public void GeneratePlayerHand2(List<string> hand)
    {
        Vector3 startHandPosition = playerHandSpawn.transform.position;
        Vector3 startPreviewPosition = playerCardSpawn.transform.position;

            // simule cartes que les autres joueurs voient: TODO a melanger
            GameObject cardPreview = Instantiate(playerHandCardPrefab, new Vector3(startPreviewPosition.x, startPreviewPosition.y, 3), Quaternion.identity);
            cardPreview.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            cardPreview.GetComponent<Card>().isHidden = true;
            cardPreview.GetComponent<Selectable>().enabled = false;
            startPreviewPosition.x += 2;
    }

    public void GenerateOtherPlayersHand(int nbCard)
    {
        foreach (KeyValuePair<string, GameObject> player in playerMap)
        {
            Vector3 lastPosition = player.Value.transform.GetChild(0).gameObject.transform.position; // accede a CardSpawn du prefab...

            for (int i = 0; i < nbCard; i++)
            {
                GameObject newCard = Instantiate(playerHandCardPrefab, new Vector3(lastPosition.x, lastPosition.y, 2), Quaternion.identity);
                newCard.GetComponent<Card>().isHidden = true;
                lastPosition.x = lastPosition.x + 2; // magic number
                newCard.GetComponent<Card>().playerId = player.Key;
            }
        }
    }

    public void AddPlayer(string id, string pseudo)
    {

        if (!playerMap.ContainsKey(id)) {			
            GameObject location = emptyPlayerSlot[0];
            GameObject newPlayer = Instantiate(playerAvatarPrefab, new Vector3(location.transform.position.x, location.transform.position.y, 2), Quaternion.identity);
            Player player = newPlayer.GetComponent<Player>();
            player.nameTextMesh.text = pseudo.Replace("\"", string.Empty);

            emptyPlayerSlot.RemoveAt(0);
            newPlayer.name = id;
            newPlayer.transform.parent = playersContainer.transform;
            playerMap.Add(id, newPlayer);
        }
    }

    public void RemovePlayer(string id)
    {
        GameObject go = playerMap[id];
        Vector3 disconnectedPosition = playerMap[id].transform.position;
        Destroy(go);
        playerMap.Remove(id);

        GameObject emptySeat = Instantiate(emptySeatPrefab, disconnectedPosition, Quaternion.identity);
        emptyPlayerSlot.Add(emptySeat);
    }

    public void showRole(string role) {
        if (role.Equals("\"moriarty\"")) {
            (rolesImage[0]).SetActive(false);
            (rolesImage[1]).SetActive(true);
            Camera.main.backgroundColor = Color.red;
        }
        else {
            (rolesImage[0]).SetActive(true);
            (rolesImage[1]).SetActive(false);
            Camera.main.backgroundColor = Color.blue;
        }
    }
    
    public void cleanHand() {
        for (int i = 0; i < currentPlayerHandCards.Count; ++i) {
                Destroy(currentPlayerHandCards[i].gameObject);
             }
            currentPlayerHandCards.Clear();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
