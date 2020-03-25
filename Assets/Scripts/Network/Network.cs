﻿using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour
{
    SocketIOComponent socket;

    private TimeBomb timeBomb;
    private string pseudoForServer;

    public static string ClientID { get; private set; }

    void Start()
    {
        socket = GetComponent<SocketIOComponent>();

        // This line will set up the listener function
        socket.On("connectionEstabilished", onConnectionEstabilished);
        socket.On("foreignMessage", onForeignMessage);
        socket.On("playerConnection", onPlayerConnection);
        socket.On("playerDisconnection", onPlayerDisconnection);
        socket.On("sendCard", onNewHandDistributed);
        socket.On("otherCard", onOtherCardDistributed);
        socket.On("roleAssigment", onRoleAssignement);
        socket.On("userList", onPlayerList);
        socket.On("startGame", onStartGame);

        timeBomb = GameObject.Find("Game").GetComponent<TimeBomb>();
    }

    // This is the listener function definition
    void onConnectionEstabilished(SocketIOEvent evt)
    {
        Debug.Log("You are connected: " + evt.data.GetField("id"));
    }

    void onPlayerConnection(SocketIOEvent evt)
    {
        string id = evt.data["id"].ToString();
        string pseudo = evt.data["username"].ToString();
        Debug.Log("Player is connected: " + id);

        timeBomb.AddPlayer(id, pseudo);
    }

    void onPlayerList(SocketIOEvent evt)
    {
        Debug.Log("Players in session: " + evt.data.GetField("userList").ToString());
        string data = evt.data.ToString();
        UserList playerList = new UserList();
        playerList = JsonUtility.FromJson<UserList>(data);
        Player[] dataArray = getJsonArray<Player>(data);
    }

    void onPlayerDisconnection(SocketIOEvent evt)
    {
        string id = evt.data["id"].ToString();
        Debug.Log("Player is disconnected: " + id);
        
        timeBomb.RemovePlayer(id);
    }

    void onForeignMessage(SocketIOEvent evt)
    {
        Debug.Log(evt.data.GetField("message"));
    }

    void onNewHandDistributed(SocketIOEvent evt)
    {
        string jsonString = evt.data.ToString();
        PlayerHand playerHand = JsonUtility.FromJson<PlayerHand>(jsonString);

        timeBomb.GeneratePlayerHand(playerHand);
    }

    void onOtherCardDistributed(SocketIOEvent evt)
    {
        string jsonString = evt.data.ToString();
        Card playerCards = JsonUtility.FromJson<Card>(jsonString);

//        timeBomb.GenerateOtherPlayersHand(null);
    }

    void onStartGame(SocketIOEvent evt)
    {
        timeBomb.startGame();
    }

    void onRoleAssignement(SocketIOEvent evt)
    {
        string role = evt.data.GetField("role").ToString();
        Debug.Log("Role assigned: " + role);

        timeBomb.showRole(role);
    }

    public static T[] getJsonArray<T>(string json)
    {
        string newJson = "{ \"array\": " + json + "}";
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(newJson);
        return wrapper.array;
    }

    public void joinGame() {
        if (pseudoForServer == null) {
            pseudoForServer = "noname";
        }

        socket.Emit("register", JSONObject.CreateStringObject(pseudoForServer));
    }

    public void setPseudoForServer(string pseudo) {
        pseudoForServer = pseudo;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] array;
    }

    [System.Serializable]
    public class Player
    {
        public string id { get; set; }
        public string playerName { get; set; }
    }

    [System.Serializable]
    public class UserList
    {
        public List<Player> userList;
    }
}