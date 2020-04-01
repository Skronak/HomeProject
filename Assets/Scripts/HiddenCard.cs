using UnityEngine;

public class HiddenCard : MonoBehaviour
{   
    Material material;
    bool isDissolving = false;
    public float fadeDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        material = GetComponent<SpriteRenderer>().material;
        isDissolving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDissolving) {
            fadeDuration -=(Time.deltaTime/1.5f);
            if(fadeDuration <= 0f) {
                fadeDuration = 0f;
                isDissolving = false;
                Destroy(gameObject);
            }
            material.SetFloat("_Fade", fadeDuration);
        }
    }
}
