using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using System.Collections;

public class ParallaxController : MonoBehaviour {

	public Transform[] backgrounds;			// Array (list) of all the back- and foregrounds to be parallaxed

	// Is called before Start(). Great for references.
	void Awake () {
	}

	// Use this for initialization
	void Start () {
		LeanTween.moveLocal(backgrounds[0].gameObject, new Vector3(400f,-100f,0f), 45).setDelay(1f).setLoopPingPong();
		LeanTween.moveLocal(backgrounds[1].gameObject, new Vector3(-300f,-100f,0f), 60f).setDelay(5f).setLoopPingPong();
    }
}

