using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class TimeBomb : MonoBehaviour
{

    public List<GameObject> emptyPlayerSlot;
    public GameObject [] revealedCardSpawn;
    public GameObject playerAvatarPrefab;
    public GameObject playerHandCardPrefab;
    public GameObject cardPrefab;
    public GameObject emptySeatPrefab;
    public GameObject playerPreviewHandSpawnGood;
    public GameObject playerPreviewHandSpawnBad;
    private GameObject playerPreviewHandSpawn;
    public GameObject[] playerHandCardsSpawn;
    private List<GameObject> currentPlayerHandCards; // TODO useless?
    private Dictionary<string, GameObject> currentCardsInGame;
    public GameObject[] rolesImage;
    public Dictionary<string, GameObject> playerMap;
    public GameObject playersContainer;
    public GameObject consoleGameobject;
    public GameObject socketIOGO;
    private CustomConsole console;
    private SocketIOComponent socketIoComponent;
    private GameObject currentHoverObject;
    private List<GameObject> currentCardRevealed;
    public GameObject token;

    void Start()
    {
        console = consoleGameobject.GetComponent<CustomConsole>();
        socketIoComponent = socketIOGO.GetComponent<SocketIOComponent>();
        currentPlayerHandCards = new List<GameObject>();
        currentCardRevealed = new List<GameObject>();
        currentCardsInGame = new Dictionary<string, GameObject>();
        playerMap = new Dictionary<string, GameObject>();
    }

    public void startGame()
    {
        console.sendMessageToConsole("system", "Game start");

        currentPlayerHandCards = new List<GameObject>();
        currentCardsInGame = new Dictionary<string, GameObject>();
        playerMap = new Dictionary<string, GameObject>();
    }

    public void GeneratePlayerHand(PlayerHand playerHand) {
        initNewTurn();

        Vector3 lastPosition = playerPreviewHandSpawn.transform.position;         
        for (int i = 0; i < playerHand.hand.Length; i++)
        {
            PlayerCard playerCard = playerHand.hand[i];

            Transform spawnToReplace = playerHandCardsSpawn[i].transform;
            GameObject playerCardGO = Instantiate(playerHandCardPrefab, new Vector3(spawnToReplace.position.x, spawnToReplace.position.y, 2), spawnToReplace.rotation);
            playerCardGO.GetComponent<Card>().setCardType(playerCard.value);
            playerCardGO.transform.parent = spawnToReplace;
            currentPlayerHandCards.Add(playerCardGO);

            GameObject previewPlayerCardGO = Instantiate(playerHandCardPrefab, new Vector3(lastPosition.x, lastPosition.y, 2), Quaternion.identity);
            previewPlayerCardGO.GetComponent<Card>().isHidden = true;
            lastPosition.x = lastPosition.x + 2; // magic number
            previewPlayerCardGO.GetComponent<Card>().playerId = playerCard.player;
            previewPlayerCardGO.GetComponent<Card>().cardId = playerCard.id;
            previewPlayerCardGO.AddComponent<Selectable>().socket = socketIoComponent;
            previewPlayerCardGO.GetComponent<Selectable>().isPlayerCard = true;
            currentCardsInGame.Add(playerCard.id, previewPlayerCardGO);
        }
    }

    public void GenerateOtherPlayersHand(OtherPlayerHands otherPlayerHands)
    {
        for (int i = 0; i < otherPlayerHands.otherPlayerHand.Length; i++)
        {   
            Shuffle(otherPlayerHands.otherPlayerHand);            
            OtherPlayerHand otherPlayerHand = otherPlayerHands.otherPlayerHand[i];
           
            GameObject playerGO = playerMap[otherPlayerHand.playerId];

            Vector3 lastPosition = playerGO.transform.GetChild(0).gameObject.transform.position; // accede a CardSpawn du prefab...
            foreach (int cardId in otherPlayerHand.cardId) {
                GameObject newCard = Instantiate(playerHandCardPrefab, new Vector3(lastPosition.x, lastPosition.y, 2), Quaternion.identity);
                newCard.GetComponent<Card>().isHidden = true;
                lastPosition.x = lastPosition.x + 2; // magic number
                newCard.GetComponent<Card>().playerId = otherPlayerHand.playerId;
                newCard.GetComponent<Card>().cardId = cardId.ToString();
                newCard.AddComponent<Selectable>().socket = socketIoComponent;
                newCard.transform.parent = playerPreviewHandSpawn.transform;
                currentCardsInGame.Add(cardId.ToString(), newCard); // TODO moche de cast tostring
            }
        }
    }

    public void AddPlayer(string id, string pseudo)
    {

        if (!playerMap.ContainsKey(id)) {			
            GameObject location = emptyPlayerSlot[0];
            GameObject newPlayer = Instantiate(playerAvatarPrefab, new Vector3(location.transform.position.x, location.transform.position.y, 2), Quaternion.identity);
            Player player = newPlayer.GetComponent<Player>();
            player.nameTextMesh.text = pseudo;
            player.transform.parent = location.transform;

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
        if (role.Equals("1")) {
            (rolesImage[0]).SetActive(false);
            (rolesImage[1]).SetActive(true);
            Camera.main.backgroundColor = Color.red;
            playerPreviewHandSpawn = playerPreviewHandSpawnBad;
        }
        else {
            (rolesImage[0]).SetActive(true);
            (rolesImage[1]).SetActive(false);
            Camera.main.backgroundColor = Color.blue;
            playerPreviewHandSpawn = playerPreviewHandSpawnGood;
        }
    }
    
    public void cleanBoard() {
        for (int i = 0; i < currentPlayerHandCards.Count; ++i) {
                Destroy(currentPlayerHandCards[i].gameObject);
        }
        currentPlayerHandCards.Clear();

        foreach(KeyValuePair<string, GameObject> cardGameObject in currentCardsInGame) {
            Destroy(cardGameObject.Value);
        }
        currentCardsInGame.Clear();

        for (int i = 0; i < currentCardRevealed.Count; ++i) {
                Destroy(currentCardRevealed[i].gameObject);
        }
        currentCardRevealed.Clear();
    }

    public void HoverCard(string cardId) {
        if (currentHoverObject!= null) {
            currentHoverObject.GetComponent<Selectable>().RemoveCardHoverEffect();
        }
        currentHoverObject = currentCardsInGame[cardId];
        currentCardsInGame[cardId].GetComponent<Selectable>().AddCardHoverEffect();
    }

    public void RevealCard(PlayerCard playerCard) {
        currentHoverObject = currentCardsInGame[playerCard.id];
        GameObject cardRevealedGO = Instantiate(cardPrefab, new Vector3(currentHoverObject.transform.position.x, currentHoverObject.transform.position.y, 2), Quaternion.identity);
        cardRevealedGO.transform.localScale = currentHoverObject.transform.localScale;
        cardRevealedGO.GetComponent<Card>().setCardType(playerCard.value);
        currentHoverObject.AddComponent<Dissolve>();
        currentHoverObject.GetComponent<ScaleOnHover>().enabled = false;
        cardRevealedGO.transform.parent = revealedCardSpawn[currentCardRevealed.Count].transform;
        currentCardRevealed.Add(cardRevealedGO);

        LeanTween.moveLocal(cardRevealedGO, revealedCardSpawn[currentCardRevealed.Count].transform.position, 0.5f).setDelay(2f);
    }
  
    public void showToken(bool isSelf, string playerId) {
        if(isSelf) {
            token.SetActive(true);
            token.transform.position = Vector3.zero;
            token.transform.localScale = Vector3.zero;

            LeanTween.scale(gameObject, new Vector3(2,2,0), 1f);
            LeanTween.moveLocal(gameObject, new Vector3(-62,-81,0),1).setDelay(1);
            LeanTween.scale(gameObject, new Vector3(1,1,0),1f).setDelay(1f);
        } else {
            token.SetActive(false);
        }
    }

    public void initNewTurn() {
        cleanBoard();
        token.SetActive(false);
    }

    private void Shuffle<T>(T[] list)
    {
        System.Random random = new System.Random();
        int n = list.Length;
        while (n > 1)
        {
            int k = random.Next(n);
            n--;
            T temp = list[k];
            list[k] = list[n];
            list[n] = temp;
        }
    }
}
