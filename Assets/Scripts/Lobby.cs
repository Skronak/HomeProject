using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    public enum LobbyState{
        Default,
        JoinedRoom
    }
    public LobbyState State = LobbyState.Default;
    public bool Debugging = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
