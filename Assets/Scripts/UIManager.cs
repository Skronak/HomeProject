using UnityEngine;
using SocketIO;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject nextTurnButton;
    public GameObject endTurnBanner;
	private SocketIOComponent socket;
    private GameObject currentGameObject;
    public TextMeshProUGUI turnCounterTextMesh;
    public RectTransform newTurnImage;
    public TextMeshProUGUI diffusingWireText;
    public TextMeshProUGUI secureWireText;

	void Start()
	{
        socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent> ();
    }

    public void showNextTurnButton() {
        nextTurnButton.transform.localScale = Vector3.zero;
        nextTurnButton.SetActive(true);
        LeanTween.scale(nextTurnButton, new Vector3(2, 2, 0), 1f);
    }

    public  void hideNextTurnButton() {
        currentGameObject = nextTurnButton;
        LeanTween.scale(currentGameObject, new Vector3(0, 0, 0), 1f).setOnComplete(DisableGameObject);
//        nextTurnButton.SetActive(false);
    }

    public void showEndTurnBanner() {
        endTurnBanner.GetComponent<RectTransform>().localScale = Vector3.zero;
        endTurnBanner.SetActive(true);
        LeanTween.scale(endTurnBanner, Vector3.one * 2, 1f);
    }

    public void hideEndTurnBanner() {
        currentGameObject = endTurnBanner;
        LeanTween.scale(currentGameObject, new Vector3(0, 0, 0), 1f).setOnComplete(DisableGameObject);
    }

    public void DisableGameObject() {
        currentGameObject.gameObject.SetActive(false);
    }

    public void showNewTurnBanner(string turnNumber)
    {
        newTurnImage.localScale = Vector3.zero;
        newTurnImage.gameObject.SetActive(true);
        turnCounterTextMesh.SetText(turnNumber);
        TimeBomb.IsInputEnabled = false;
        LeanTween.scale(newTurnImage, Vector3.one * 2, 1f);
        LeanTween.delayedCall(2, PrepareNewTurn);
    }

    public void PrepareNewTurn()
    {
        newTurnImage.gameObject.SetActive(false);
        TimeBomb.IsInputEnabled = true;
    }

    public void updateWireCounter(string secureCounter, string maxSecureCounter, string difusingCounter, string maxDifusingCounter) {
        secureWireText.SetText("Secure wire: "+secureCounter+"/"+maxSecureCounter);
        secureWireText.SetText("Defusing wire: "+difusingCounter+"/"+maxDifusingCounter);
    }

}
