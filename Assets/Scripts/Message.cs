using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Message : MonoBehaviour
{
    public string id;
    public string text;

    public Message(string id, string text) {
        this.id = id;
        this.text = text;
    }

    public string formatMessage() {
        return this.id + ": " + this.text;
    }
}
