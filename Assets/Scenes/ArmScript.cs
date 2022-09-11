using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Obi;

public class ArmScript : MonoBehaviour
{
    public float rotationSpeed;
    public float forwardspeed;

    public Vector3 direction;

    public Rigidbody rigidbody;

    public ObiRopeCursor cursor;
    public ObiRope rope;
    public float minLength = 0.1f;
    public float maxLength;

    public GameObject Cam1, Cam2;

    private float rotationX;
    private float rotationY;

    private bool isStartedReverse;
    void Start()
    {
        //rope = GetComponent<ObiRope>();
        //cursor = GetComponent<ObiRopeCursor>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {

            


            //direction = (new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f));

            //transform.Rotate(direction * Time.deltaTime * rotationSpeed);

            //transform.Translate(Vector3.forward * Time.deltaTime * forwardspeed);


            //rigidbody.isKinematic = true;
            //rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
            //rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

            //rigidbody.transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime, rotationY);
            //rigidbody.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, rotationX);

            

            //rigidbody.MovePosition(rigidbody.position + transform.forward * forwardspeed * Time.deltaTime);


            //cursor.ChangeLength(rope.restLength + forwardspeed * Time.deltaTime);

            if (!isStartedReverse)
            {

                float tension = rope.CalculateLength() / rope.restLength - 1;

                if (tension > 0.01f)
                {
                    cursor.ChangeLength(rope.restLength + forwardspeed * Time.deltaTime);
                }
            }
            
        }
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
            rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

            Vector3 m_EulerAngleVelocity = new Vector3(-rotationY, rotationX, 0f);

            Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * rotationSpeed * Time.fixedDeltaTime);
            rigidbody.rotation = (rigidbody.rotation * deltaRotation);

            rigidbody.MovePosition(rigidbody.position + transform.forward * forwardspeed * Time.fixedDeltaTime);

            //rigidbody.AddForce(transform.forward * forwardspeed * Time.fixedDeltaTime,ForceMode.VelocityChange);
            
        }
        else
        {
            rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, Quaternion.Euler(rigidbody.transform.eulerAngles.x, rigidbody.transform.eulerAngles.y,0f), 8 * Time.deltaTime);
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        if (Input.GetMouseButton(1))
        {
            isStartedReverse = true;
            Cam1.SetActive(false);
            Cam2.SetActive(true);
            rigidbody.isKinematic = false;
            GetComponent<Collider>().enabled = false;
            if (rope.restLength > minLength)
                cursor.ChangeLength(rope.restLength - forwardspeed * Time.deltaTime);
        }

        


        if (rope.restLength >= maxLength)
        {
            Debug.Log("Sýýýç");
        }
    }
    

    
}

