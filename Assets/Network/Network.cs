using System.Collections.Generic;
using UnityEngine;
using SocketIO;

public class Network : MonoBehaviour
{
    SocketIOComponent socket;
    Dictionary<string, GameObject> serverObjects;

    private TimeBomb timeBomb;
    private string pseudoForServer;

    [Header("Network Client")]
    [SerializeField]
    private Transform networkContainer;

    public static string ClientID { get; private set; }

    void Start()
    {
        serverObjects = new Dictionary<string, GameObject>();
        socket = GetComponent<SocketIOComponent>();

        // This line will set up the listener function
        socket.On("connectionEstabilished", onConnectionEstabilished);
        socket.On("foreignMessage", onForeignMessage);
        socket.On("playerConnection", onPlayerConnection);
        socket.On("playerDisconnection", onPlayerDisconnection);
        socket.On("newHand", onNewHandReceived);
        socket.On("roleAssignement", onRoleAssignement);
        socket.On("userList", onPlayerList);
        socket.On("startGame", onStartGame);

        timeBomb = GameObject.Find("Game").GetComponent<TimeBomb>();
    }

    // This is the listener function definition
    void onConnectionEstabilished(SocketIOEvent evt)
    {
        Debug.Log("You are connected: " + evt.data.GetField("id"));
        //ClientID = evt.data["id"].ToString();
    }

    void onPlayerConnection(SocketIOEvent evt)
    {
        string id = evt.data["id"].ToString();
        string pseudo = evt.data["username"].ToString(); // TODO: a mapper dans un objet
        Debug.Log("Player is connected: " + id);

        if (!serverObjects.ContainsKey(id)) {			
            GameObject go = timeBomb.AddPlayer(pseudo);
            go.transform.SetParent(networkContainer);
            serverObjects.Add(id, go);
        }
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
        GameObject go = serverObjects[id];

        timeBomb.RemovePlayer(go.transform.position);

        Destroy(go);
        serverObjects.Remove(id);
    }

    void onForeignMessage(SocketIOEvent evt)
    {
        Debug.Log(evt.data.GetField("message"));
    }

    void onNewHandReceived(SocketIOEvent evt)
    {
        JSONObject data = evt.data.GetField("hand");
        Debug.Log("new hand " + data);

        string[] dataArray = getJsonArray<string>(data.ToString());
        List<string> hand = new List<string>(dataArray);

        timeBomb.GeneratePlayerHand(hand);
        timeBomb.GenerateOtherPlayersHand(serverObjects, hand.Count);
    }

    void onStartGame(SocketIOEvent evt)
    {
        Debug.Log("Game start: " + evt.data.GetField("id"));
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