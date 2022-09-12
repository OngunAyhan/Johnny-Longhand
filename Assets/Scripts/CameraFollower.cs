using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    public GameObject target;

    public float LookSpeed = 5;
    public float FollowSpeed = 5;

    Vector3 offset;

    public GameController gameController;

    public bool CanGo;

    public Vector3 GoPos;

    public Vector3 GoRot;

    private float GoSpeed;

    public float FPVSpeed;

    public float TPVSpeed;

    void Start()
    {
        //offset = target.transform.position - transform.position;
    }

    void FixedUpdate()
    {
        if (!gameController.IsGameStarted)
        {
            return;
        }
        var newRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, LookSpeed * Time.deltaTime);

        
        Vector3 newPosition = target.transform.position - target.transform.forward * offset.z - target.transform.up * offset.y;
        transform.position = Vector3.Slerp(transform.position, newPosition, Time.deltaTime * FollowSpeed);
    }


    private void LateUpdate()
    {
        if (CanGo)
        {
            transform.position = Vector3.Lerp(transform.position, GoPos, GoSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(GoRot), GoSpeed * Time.deltaTime);

            
        }
        
    }

    public void SetCameraOffset()
    {
        offset = target.transform.position - transform.position;
    }

    public void GoToPosSetter(Vector3 pos, Vector3 rot, int WhereTo)
    {
        GoPos = pos;
        GoRot = rot;



        if (WhereTo == 0)//FPV
        {
            GoSpeed = FPVSpeed;
            
        }
        else//TPV
        {
            GoSpeed = TPVSpeed;
            
        }

        CanGo = true;
    }
}
