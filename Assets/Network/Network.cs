using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour 
{
	SocketIOComponent socket;
    Dictionary<string, GameObject> players;
	TimeBomb timeBomb;
//    List<string> players = new List<string>();

	void Start()
	{
        players = new Dictionary<string, GameObject>();

		socket = GetComponent<SocketIOComponent> ();

		// This line will set up the listener function
		socket.On ("connectionEstabilished", onConnectionEstabilished);
        socket.On("foreignMessage", onForeignMessage);
        socket.On("playerConnection", onPlayerConnection);
        socket.On("playerDisconnection", onPlayerDisconnection);
        socket.On("newHand", onNewHandReceived);
		socket.On("roleAssignement", onRoleAssignement);

		timeBomb = GameObject.Find("Game").GetComponent<TimeBomb>();
	}

	// This is the listener function definition
	void onConnectionEstabilished(SocketIOEvent evt)
	{
		Debug.Log ("You are connected: " + evt.data.GetField("id"));
	}

	void onPlayerConnection(SocketIOEvent evt)
	{
		Debug.Log ("Player is connected: " + evt.data.GetField("id"));
        players.Add(evt.data["id"].ToString(), null);
    }

	void onPlayerDisconnection(SocketIOEvent evt)
	{
		Debug.Log ("Player is disconnected: " + evt.data.GetField("id"));
	}

    void onForeignMessage(SocketIOEvent evt)
	{
		Debug.Log(evt.data.GetField("message"));
	}

    void onNewHandReceived(SocketIOEvent evt)
    {
		Debug.Log("Hand received");
		JSONObject data = evt.data.GetField("hand");	
		string[] dataArray = JsonHelper.getJsonArray<string>(data.ToString());
		List<string> hand = new List<string>(dataArray); 

		timeBomb.getNewHand(hand);
    }

   	void onStartGame(SocketIOEvent evt)
	{
		Debug.Log ("Game start: " + evt.data.GetField("id"));
	}

	void onRoleAssignement(SocketIOEvent evt)
	{
		Debug.Log("You are in team: " + evt.data.GetField("role"));
	}

	 public class JsonHelper
 {
     public static T[] getJsonArray<T>(string json)
     {
         string newJson = "{ \"array\": " + json + "}";
         Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
         return wrapper.array;
     }
 
     [System.Serializable]
     private class Wrapper<T>
     {
         public T[] array;
     }
 }

}