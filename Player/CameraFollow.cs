using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // initialize game object target variable for Transform method/function
    public Transform target;
    // variable that sets how smooth/fast the camera glides to the target
    public float smoothTime = 0.3f;
    // variable that offsets the camera to emphasize the target moving
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
