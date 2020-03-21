using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIElementMovement : MonoBehaviour
{
    private Vector3 pointA;
    private Vector3 pointB;
    public float targetX;
    public bool repeat;
    public float speed = 1;
    private float t;    

    void Start()
    {
        pointA = transform.position;
        pointB = transform.position;
    }

    public void triggerMoveAnimation()
    {
        pointB.x += targetX;
    }

    private void Update()
    {
        t += Time.deltaTime * speed;
        // Moves the object to target position
        transform.position = Vector3.Lerp(pointA, pointB, t);
        // Flip the points once it has reached the target
        if (t >= 1 && repeat)
        {
            var b = pointB;
            var a = pointA;
            pointA = b;
            pointB = a;
            t = 0;
        }
    }
    // What Linear interpolation actually looks like in terms of code
    private Vector3 CustomLerp(Vector3 a, Vector3 b, float t)
    {
        return a * (1 - t) + b * t;
    }
}