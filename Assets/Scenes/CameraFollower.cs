using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public GameObject target;

    public float LookSpeed = 5;
    public float FollowSpeed = 5;

    Vector3 offset;

    void Start()
    {
        offset = target.transform.position - transform.position;
    }

    void FixedUpdate()
    {
        
        var newRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, LookSpeed * Time.deltaTime);

        
        Vector3 newPosition = target.transform.position - target.transform.forward * offset.z - target.transform.up * offset.y;
        transform.position = Vector3.Slerp(transform.position, newPosition, Time.deltaTime * FollowSpeed);
    }
}
