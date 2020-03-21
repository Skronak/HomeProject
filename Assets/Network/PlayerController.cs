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
            socket.Emit("handRequest");
		}


	}
}