using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIO;

namespace Project.Networking
{
    public class NetworkClient : SocketIOComponent
    {

        Dictionary<string, GameObject> serverObjects;
        TimeBomb timeBomb;

        [Header("Network Client")]
        [SerializeField]
        private Transform networkContainer;

        public override void Start()
        {
            if (autoConnect) { Connect(); }
            serverObjects = new Dictionary<string, GameObject>();

            // This line will set up the listener function
            On("connectionEstabilished", onConnectionEstabilished);
            On("foreignMessage", onForeignMessage);
            On("playerConnection", onPlayerConnection);
            On("playerDisconnection", onPlayerDisconnection);
            On("newHand", onNewHandReceived);
            On("roleAssignement", onRoleAssignement);
            On("userList", onPlayerList);
            On("startGame", onStartGame);

            timeBomb = GameObject.Find("Game").GetComponent<TimeBomb>();

        }

        public override void Update()
        {
            base.Update();
        }

        // This is the listener function definition
        void onConnectionEstabilished(SocketIOEvent evt)
        {
            Debug.Log("You are connected: " + evt.data.GetField("id"));
        }

        void onPlayerConnection(SocketIOEvent evt)
        {
            string id = evt.data["id"].ToString();
            Debug.Log("Player is connected: " + id);

            GameObject go = new GameObject(id);
            go.transform.SetParent(networkContainer);

            serverObjects.Add(id, go);
//            timeBomb.addPlayer(id);
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
            Debug.Log(data);

            string[] dataArray = getJsonArray<string>(data.ToString());
            List<string> hand = new List<string>(dataArray);

            timeBomb.getNewHand(hand);
        }

        void onStartGame(SocketIOEvent evt)
        {
            Debug.Log("Game start: " + evt.data.GetField("id"));
            timeBomb.startGame();
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
}
