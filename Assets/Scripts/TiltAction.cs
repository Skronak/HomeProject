using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltAction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<RectTransform>().localScale = Vector3.zero;

        LeanTween.scale(gameObject, new Vector3(2,2,0), 1f);
        LeanTween.moveLocal(gameObject, new Vector3(96,120,0),1).setLoopPingPong(1);
        LeanTween.scale(gameObject, new Vector3(1,1,0),1f).setDelay(1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
