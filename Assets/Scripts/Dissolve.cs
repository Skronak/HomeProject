using UnityEngine;

public class Dissolve : MonoBehaviour
{
    Material material;
    bool isDissolving = false;
    float fade = 1f;

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
            fade -=(Time.deltaTime/1.5f);
            if(fade <= 0f) {
                fade = 0f;
                isDissolving = false;
                Destroy(gameObject);
            }
            material.SetFloat("_Fade", fade);
        }
    }
}
