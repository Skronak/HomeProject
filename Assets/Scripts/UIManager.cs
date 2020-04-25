using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Transform uiRoot;
    private Network network;
    private GameObject currentGameObject;
    private TextMeshProUGUI turnCounterTextMesh;
    public RectTransform newTurnPopUpPrefab;
    public TextMeshProUGUI diffusingWireText;
    public TextMeshProUGUI secureWireText;
    public string maxDefusingWire;
    public string maxSecureWire;
    public GameObject goodInstructionPopUpPrefab;
    public GameObject badInstructionPopUpPrefab;
    public GameObject connectionCheckIcon;
    public GameObject endTurnPopUpPrefab;
    public GameObject nextTurnButtonPrefab;
    public GameObject startGameButtonPrefab;
    private GameObject endTurnPopUp;
    private RectTransform newTurnPopUp;
    private GameObject nextTurnButton;
    private GameObject startGameButton;

    void Start()
    {
        network = GameObject.Find("SocketIO").GetComponent<Network>();
    }

    public void showNextTurnButton()
    {
        nextTurnButton = Instantiate(nextTurnButtonPrefab, new Vector3(Screen.width * 0.5f, 210, 0), Quaternion.identity);
        nextTurnButton.transform.localScale = Vector3.zero;
        nextTurnButton.transform.SetParent(uiRoot);
        nextTurnButton.gameObject.GetComponent<Button>().onClick.AddListener(network.triggerNextTurn);
        LeanTween.scale(nextTurnButton, new Vector3(2, 2, 0), 1f);
    }

    public void hideNextTurnButton()
    {
        if (nextTurnButton != null)
        {
            scaleDownAndDestroy(nextTurnButton);
        }
    }

    public void showEndTurnBanner()
    {
        endTurnPopUp = Instantiate(endTurnPopUpPrefab, new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0), Quaternion.identity);
        endTurnPopUp.GetComponent<RectTransform>().localScale = Vector3.zero;
        endTurnPopUp.transform.SetParent(uiRoot);
        LeanTween.scale(endTurnPopUp, Vector3.one * 2, 1f);
    }

    public void hideEndTurnBanner()
    {
        if (endTurnPopUp != null)
        {
            scaleDownAndDestroy(endTurnPopUp);
        }
    }

    public void showGoodInstruction()
    {
        GameObject popUp = Instantiate(goodInstructionPopUpPrefab, new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0), Quaternion.identity);
        popUp.transform.SetParent(uiRoot);
    }

    public void showBadInstruction()
    {
        GameObject popUp = Instantiate(badInstructionPopUpPrefab, new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0), Quaternion.identity);
        popUp.transform.SetParent(uiRoot);
    }

    public void showStartGameButton() {
        startGameButton = Instantiate(startGameButtonPrefab, new Vector3(Screen.width * 0.5f,Screen.height * 0.5f,0), Quaternion.identity);
        currentGameObject = startGameButton;
        startGameButton.transform.SetParent(uiRoot);
        startGameButton.gameObject.GetComponent<Button>().onClick.AddListener(network.triggerStartGame);
        startGameButton.gameObject.GetComponent<Button>().onClick.AddListener(DestroyGameObject);
    }

    public void hideStartGameButton() {
        if (startGameButton != null) {
            scaleDownAndDestroy(startGameButton);
        }
    }

    public void showNewTurnBanner(string turnNumber)
    {
        newTurnPopUp = Instantiate(newTurnPopUpPrefab, new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0), Quaternion.identity);
        newTurnPopUp.SetParent(uiRoot);
        newTurnPopUp.localScale = Vector3.zero;
        turnCounterTextMesh = newTurnPopUp.transform.GetComponentInChildren<TextMeshProUGUI>();//GetChild(0).gameObject.transform.GetChild(0).GetComponent<;
        turnCounterTextMesh.SetText(turnNumber);
        TimeBomb.IsInputEnabled = false;
        LeanTween.scale(newTurnPopUp, Vector3.one * 2, 1f);
        LeanTween.delayedCall(2, PrepareNewTurn);
    }

    public void PrepareNewTurn()
    {
        currentGameObject = newTurnPopUp.gameObject;
        scaleDownAndDestroy(newTurnPopUp.gameObject);
        TimeBomb.IsInputEnabled = true;
    }

    public void updateWireCounter(string secureCounter, string difusingCounter)
    {
        secureWireText.SetText("Secure wire: " + secureCounter + "/" + maxSecureWire);
        diffusingWireText.SetText("Defusing wire: " + difusingCounter + "/" + maxDefusingWire);
    }

    private void scaleDownAndDestroy(GameObject gameObject) {
        currentGameObject = gameObject;
        LeanTween.scale(currentGameObject, new Vector3(0, 0, 0), 1f).setOnComplete(DestroyGameObject);
    }

    private void DisableGameObject()
    {
        currentGameObject.gameObject.SetActive(false);
    }

    private void DestroyGameObject()
    {
        Destroy(currentGameObject);
    }

}
