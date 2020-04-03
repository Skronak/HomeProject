using UnityEngine;

// Card spawn in player hand
public class ScaleOnHover : MonoBehaviour
{
    private Vector3 onOverPosition;
    private Vector3 originalPosition;
    private float moveYBy = 0.5f;
    private float scaleBy = 1.2f;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
        onOverPosition = new Vector3(originalPosition.x, originalPosition.y + moveYBy, -1);
    }

    void OnMouseEnter()
    {
        if (TimeBomb.IsInputEnabled)
        {
            transform.position = onOverPosition;
            transform.localScale = new Vector3(scaleBy, scaleBy, scaleBy);
        }
    }

    void OnMouseExit()
    {
        if (TimeBomb.IsInputEnabled)
        {
            transform.position = originalPosition;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
}