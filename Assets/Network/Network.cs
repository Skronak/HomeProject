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
		socket.On("userList", onPlayerList);

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
        players.Add(evt.data["id"].ToString(), null); // doublons
    }

	void onPlayerList(SocketIOEvent evt) {
		Debug.Log("Players in session: " + evt.data.GetField("userList").ToString());
		string data = evt.data.ToString();
		UserList playerList = new UserList();
		playerList = JsonUtility.FromJson<UserList>(data);
		Player[] dataArray = getJsonArray<Player>(data);
//		JsonUtility.FromJsonOverwrite (data, playerList);

		//timeBomb.players = playerList;
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
		JSONObject data = evt.data.GetField("hand");	
		Debug.Log(data);

		string[] dataArray = getJsonArray<string>(data.ToString());
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

	[System.Serializable]
	public class Player {
		public string id { get; set; }
		public string playerName { get; set; }
	}

	[System.Serializable]
	public class UserList {
    	public List <Player> userList;
	}
}