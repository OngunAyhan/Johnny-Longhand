using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPivot : MonoBehaviour
{
    public float rotationSpeed;
    public float forwardSpeed;
    private float forwardSpeedHolder;
    private float rotationX;
    private float rotationY;

    public GameController gameController;



    void Start()
    {
        forwardSpeedHolder = forwardSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.IsGameStarted)
        {
            return;
        }

        LimitMovement();

        if (Input.GetMouseButton(0))
        {
            rotationX = Input.GetAxis("Mouse X");
            rotationY = Input.GetAxis("Mouse Y");


            Vector3 m_EulerAngleVelocity = new Vector3(-rotationY, rotationX, 0f);

            Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * rotationSpeed * Time.deltaTime);


            transform.rotation = transform.rotation * deltaRotation;

            transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);
        }
        else
        {
            transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, 0f);
        }
        
        //rigidbody.MovePosition(rigidbody.position + transform.forward * forwardspeed * Time.fixedDeltaTime);

    }

    void FixedUpdate()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd, out hit, 0.005f))
        {
            if (!hit.collider.CompareTag("Loot") && !hit.collider.CompareTag("Player"))
            {
                
                forwardSpeed = 0f;
            }
        }
        else
        {
            
            forwardSpeed = forwardSpeedHolder;
        }
    }


    private void LimitMovement()
    {

        transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, -4f, 3.5f),
        Mathf.Clamp(transform.position.y, 0f, 1f),
        Mathf.Clamp(transform.position.z, -2f, 5f));
    }
}
