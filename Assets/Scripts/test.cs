using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void StartAnimation()
    {
        var seq = LeanTween.sequence();

		// Rotate 360
        seq.append(LeanTween.scale(gameObject, new Vector3(3,3,3),0.5f).setDelay(1));
        seq.append(LeanTween.scale(gameObject, new Vector3(0,0,0),0.5f).setOnComplete(SelfDestruct));
    }

    void Reduce() {
        LeanTween.scale(gameObject, new Vector3(0,0,0),0.5f).setOnComplete(SelfDestruct);
    }

    void rotate() {
        
    }
    void SelfDestruct()
    {
        Destroy(gameObject);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            StartAnimation();
        }
    }
}
