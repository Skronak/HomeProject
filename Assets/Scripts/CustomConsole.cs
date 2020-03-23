using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomConsole : MonoBehaviour
{
    public GameObject chatMessage;
    public Transform chatContentTransform;
    public List<Message> messageList;
    public int maxMessages;

    void Start()
    {
        messageList = new List<Message>();
    }

    public void sendMessageToConsole(string transmitter, string text)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message(transmitter, text);

        GameObject newConsoleText = Instantiate(chatMessage, chatContentTransform);
        newConsoleText.GetComponent<Text>().text = transmitter+": "+text; 
        messageList.Add(newMessage);
    }
}
