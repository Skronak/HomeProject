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
		if (Input.GetKeyDown(KeyCode.A)) 
		{
			message.AddField("message","Hello! I am " + socket.sid);
			socket.Emit("hello", message);
			message.Clear ();

            socket.Emit("handRequest");
		}

		if (Input.GetKeyDown(KeyCode.Z)) 
		{
			socket.Emit("startGame");
			message.Clear ();
		}

		if (Input.GetKeyDown(KeyCode.E)) {
//			message.AddField("target", );
			socket.Emit("revealCard");
		}
	}
}