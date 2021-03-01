using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public Transform mTarget;
    public float maxFollowSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, mTarget.position);
        if (distance < maxFollowSpeed)
            transform.position = Vector3.Lerp(transform.position, mTarget.position, distance * Time.fixedDeltaTime);
        else
            transform.position = Vector3.Lerp(transform.position, mTarget.position, maxFollowSpeed * Time.fixedDeltaTime);
    }
}
