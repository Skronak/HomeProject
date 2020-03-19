using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Sprite sprite;
    public string id;
    public string pseudo;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
