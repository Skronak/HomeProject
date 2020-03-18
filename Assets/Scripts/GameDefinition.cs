using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDefinition : MonoBehaviour
{
    public int players;
    public int bomb;
    public int wire;
    public int empty;
    public int badGuys;
    public int goodGuys;

    public GameDefinition(int players, int wire, int empty, int bomb, int badGuys, int goodGuys){
        this.players = players;
        this.wire = wire;
        this.empty = empty;
        this.bomb = bomb;
        this.badGuys = badGuys;
        this.goodGuys = goodGuys;
    }
}
