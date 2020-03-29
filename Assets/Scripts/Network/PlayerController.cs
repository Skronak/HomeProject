using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class PlayerController : MonoBehaviour 
{
	SocketIOComponent socket;
	JSONObject message;

	void Start()
	{
		socket = GameObject.Find("SocketIO").GetComponent<SocketIOComponent> ();
		message = new JSONObject();
	}
	
	void Update () 
	{
		// demande au serveur de logger les utilisateurs dans la partie
		if (Input.GetKeyDown(KeyCode.A)) 
		{
			socket.Emit("logUser");
		}
		if (Input.GetKeyDown(KeyCode.Space)){
			socket.Emit("register", JSONObject.CreateStringObject("debug"));
		}

		// declenche manuellement le début d'une partie 
		if (Input.GetKeyDown(KeyCode.Z)) 
		{
			socket.Emit("startGame");
		}

		if (Input.GetKeyDown(KeyCode.E)) {
//			message.AddField("target", );
			socket.Emit("revealCard");
		}

		if (Input.GetKeyDown(KeyCode.R)) 
		{
//			string jsonString = "{\"otherPlayerCard\":[{\"playerId\": \"GJ-nqwV4Y3VNx648AAAA\",\"cardId\":[1,2]},{\"playerId\": \"GJ-nqwV4Y3VNx648AAAA\",\"cardId\":[1,2]}]}";
//			string jsonString = "{\"otherPlayerCard\":[{\"playerId\":\"JxlUxtXl20bsnR42AAAC\",\"cardId\":[1,2]},{\"playerId\":\"JxlUxtXl20bsnR42AAAC\",\"cardId\":[10,2,4,7,5]},{\"playerId\":\"JxlUxtXl20bsnR42AAAC\",\"cardId\":[14,11,13,8,12]}]}";
			string jsonString = "{\"otherPlayerHand\":[{\"playerId\":\"F2cz5kl13BksPJ40AAAA\",\"cardId\":[11,13,0,12,3]},{\"playerId\":\"yEChzy01pZO48QkdAAAB\",\"cardId\":[14,6,9,4,5]}]}";
			OtherPlayerHands otherPlayerHands = JsonUtility.FromJson<OtherPlayerHands>(jsonString);
			Debug.Log(otherPlayerHands);
		}
	}
}