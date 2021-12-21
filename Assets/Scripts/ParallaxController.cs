using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxController : MonoBehaviour {

private GameObject cloudFirst;
private GameObject cloudSecond;

void Awake()
{
	foreach (Transform child in transform) {
		if (child.name == "cloud1") {
			cloudFirst = child.gameObject;
		}
		if (child.name == "cloud2") {
			cloudSecond = child.gameObject;
		}
	}
}
	void Start () {
		LeanTween.moveLocal(cloudFirst, new Vector3(400f,-100f,0f), 45).setDelay(1f).setLoopPingPong();
		LeanTween.moveLocal(cloudSecond, new Vector3(400f,-100f,0f), 45).setDelay(1f).setLoopPingPong();
	}
}

