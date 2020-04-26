using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class TimeBomb : MonoBehaviour
{
    public List<GameObject> emptyPlayerSlot;
    public GameObject[] revealedCardSpawn;
    public GameObject playerAvatarPrefab;
    public GameObject playerHandCardPrefab;
    public GameObject cardPrefab;
    public GameObject handPrefab;
    public GameObject emptySeatPrefab;
    public GameObject playerPreviewHand;
    public GameObject[] playerHandCardsSpawn;
    private List<GameObject> currentPlayerHandCards; // TODO useless?
    private List<GameObject> currentCardRevealedThisTurn;
    private Dictionary<string, GameObject> currentCardsInGame;
    public GameObject[] rolesImage;
    public Dictionary<string, GameObject> playerMap;
    public GameObject playersContainer;
    public GameObject socketIOGO;
    private SocketIOComponent socketIoComponent;
    private GameObject currentHoverObject;
    private List<GameObject> currentCardRevealed;
    public GameObject token;
    private GameObject currentPlayerWithToken;
    public static bool IsInputEnabled = true;
    private UIManager uiManager;
    private Network network;

    void Start()
    {
        socketIoComponent = socketIOGO.GetComponent<SocketIOComponent>();
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        network = GameObject.Find("SocketIO").GetComponent<Network>();

        currentPlayerHandCards = new List<GameObject>();
        currentCardRevealed = new List<GameObject>();
        currentCardRevealedThisTurn = new List<GameObject>();
        currentCardsInGame = new Dictionary<string, GameObject>();
        playerMap = new Dictionary<string, GameObject>();
    }

    public void startGame()
    {
//        currentPlayerHandCards = new List<GameObject>();
//        currentCardsInGame = new Dictionary<string, GameObject>();
//        playerMap = new Dictionary<string, GameObject>();
    }

    public void GeneratePlayerHand(PlayerHand playerHand)
    {
        IsInputEnabled = false;
        Vector3 lastPosition = playerPreviewHand.transform.position;
        for (int i = 0; i < playerHand.hand.Length; i++)
        {
            PlayerCard playerCard = playerHand.hand[i];

            Transform spawnToReplace = playerHandCardsSpawn[i].transform;
            GameObject playerCardGO = Instantiate(handPrefab, new Vector3(spawnToReplace.position.x, spawnToReplace.position.y, 2), spawnToReplace.rotation);
            playerCardGO.transform.localScale = spawnToReplace.localScale;
            playerCardGO.GetComponent<CardUi>().setCardType(playerCard.value);
            playerCardGO.transform.SetParent(spawnToReplace);
            currentPlayerHandCards.Add(playerCardGO);
            uiManager.showFlipCardButton();

            GameObject previewPlayerCardGO = Instantiate(playerHandCardPrefab, new Vector3(lastPosition.x, lastPosition.y, 2), Quaternion.identity);
            previewPlayerCardGO.GetComponent<Card>().isHidden = true;
            lastPosition.x = lastPosition.x + 2; // magic number
            previewPlayerCardGO.GetComponent<Card>().playerId = playerCard.player;
            previewPlayerCardGO.GetComponent<Card>().cardId = playerCard.id;
            previewPlayerCardGO.AddComponent<Selectable>().socket = socketIoComponent;
            previewPlayerCardGO.GetComponent<Selectable>().isPlayerCard = true;
            previewPlayerCardGO.transform.SetParent(playerPreviewHand.transform);
            previewPlayerCardGO.SetActive(false);
            currentCardsInGame.Add(playerCard.id, previewPlayerCardGO);
        }
    }

    public void GenerateOtherPlayersHand(OtherPlayerHands otherPlayerHands)
    {
        for (int i = 0; i < otherPlayerHands.otherPlayerHand.Length; i++)
        {
            OtherPlayerHand otherPlayerHand = otherPlayerHands.otherPlayerHand[i];
            Shuffle(otherPlayerHand.cardId);

            GameObject playerGO = playerMap[otherPlayerHand.playerId];

            Vector3 lastPosition = playerGO.transform.GetChild(0).gameObject.transform.position; // accede a CardSpawn du prefab...
            foreach (int cardId in otherPlayerHand.cardId)
            {
                GameObject newCard = Instantiate(playerHandCardPrefab, new Vector3(lastPosition.x, lastPosition.y, 2), Quaternion.identity);
                newCard.GetComponent<Card>().isHidden = true;
                lastPosition.x = lastPosition.x + 2; // magic number
                newCard.GetComponent<Card>().playerId = otherPlayerHand.playerId;
                newCard.GetComponent<Card>().cardId = cardId.ToString();
                newCard.AddComponent<Selectable>().socket = socketIoComponent;
                newCard.transform.parent = playerGO.transform.GetChild(0).transform;
                newCard.SetActive(false);

                currentCardsInGame.Add(cardId.ToString(), newCard); // TODO moche de cast tostring
            }
        }
    }

    public void AddPlayer(string id, string pseudo)
    {
        if (!playerMap.ContainsKey(id))
        {
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

    public void showRole(string role)
    {
        if (role.Equals("1"))
        {
            uiManager.showBadInstruction();
            (rolesImage[0]).SetActive(false);
            (rolesImage[1]).SetActive(true);
            Camera.main.backgroundColor = Color.red;
        }
        else
        {
            uiManager.showGoodInstruction();
            (rolesImage[0]).SetActive(true);
            (rolesImage[1]).SetActive(false);
            Camera.main.backgroundColor = Color.blue;
        }
    }

    // TODO a passer dans UI entierement
    public void flipHand() {
        uiManager.hideFlipButton();
        for (int i = 0; i < currentPlayerHandCards.Count; ++i) {
            uiManager.scaleDownAndDestroy(currentPlayerHandCards[i].gameObject);
        }
        
        foreach(Transform playerCard in playerPreviewHand.transform)
        {
            playerCard.gameObject.SetActive(true);
        }
        network.triggerFlipHand();
    }

    public void flipHandForPlayer(string playerId) {
        Transform otherPlayerCards = playerMap[playerId].transform.GetChild(0);
        foreach(Transform playerCard in otherPlayerCards)
        {
            playerCard.gameObject.SetActive(true);
        }
    }

    public void startTurn() {
        IsInputEnabled = true;
    }

    public void cleanBoard()
    {
        for (int i = 0; i < currentPlayerHandCards.Count; ++i)
        {
            Destroy(currentPlayerHandCards[i].gameObject);
        }
        currentPlayerHandCards.Clear();

        foreach (KeyValuePair<string, GameObject> cardGameObject in currentCardsInGame)
        {
            Destroy(cardGameObject.Value);
        }
        currentCardsInGame.Clear();

        for (int i = 0; i < currentCardRevealedThisTurn.Count; ++i)
        {
            currentCardRevealed.Add(currentCardRevealedThisTurn[i].gameObject);
            currentCardRevealedThisTurn[i].SetActive(false);
            //Destroy(currentCardRevealed[i].gameObject);
        }
        currentCardRevealedThisTurn.Clear();
    }

    public void HoverCard(string cardId)
    {
        if (currentHoverObject != null)
        {
            currentHoverObject.GetComponent<Selectable>().RemoveCardHoverEffect();
        }
        currentHoverObject = currentCardsInGame[cardId];
        currentCardsInGame[cardId].GetComponent<Selectable>().AddCardHoverEffect();
    }

    public void RevealCard(PlayerCard playerCard)
    {
        currentHoverObject = currentCardsInGame[playerCard.id];
        GameObject cardRevealedGO = Instantiate(cardPrefab, new Vector3(currentHoverObject.transform.position.x, currentHoverObject.transform.position.y, 2), Quaternion.identity);
        cardRevealedGO.transform.localScale = currentHoverObject.transform.localScale;
        cardRevealedGO.GetComponent<Card>().setCardType(playerCard.value);
        currentHoverObject.AddComponent<Dissolve>();
        currentHoverObject.GetComponent<ScaleOnHover>().enabled = false;
        cardRevealedGO.transform.parent = revealedCardSpawn[currentCardRevealedThisTurn.Count].transform;
        currentCardRevealedThisTurn.Add(cardRevealedGO);

        LeanTween.moveLocal(cardRevealedGO, revealedCardSpawn[currentCardRevealedThisTurn.Count].transform.position, 0.5f).setDelay(2f);
    }

    public void showToken(bool isSelf, string playerId)
    {
        if (isSelf)
        {
            token.gameObject.SetActive(true);

            if (currentPlayerWithToken != null)
            {
                currentPlayerWithToken.GetComponent<Player>().token.SetActive(false);
            }
        }
        else
        {
            token.gameObject.SetActive(false);
            playerMap[playerId].GetComponent<Player>().token.gameObject.SetActive(true);
            currentPlayerWithToken = playerMap[playerId];
        }
    }

    public void initNewTurn()
    {
        cleanBoard();
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
