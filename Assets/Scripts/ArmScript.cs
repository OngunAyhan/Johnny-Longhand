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

    private float rotationX;
    private float rotationY;

    private bool isStartedReverse;

    public GameController gameController;

    public bool DidWin = false;

    public GameObject HandledLootObject;

    private float displacementTime;

    private float lastVelocity;

    public List<Transform> BackwardsTransformList;

    public List<Transform> BackwardsSetPointsList;

    public Transform BasePoint;

    public int BackCount;

    public GameObject HandVisual;

    void Start()
    {
        //rope = GetComponent<ObiRope>();
        //cursor = GetComponent<ObiRopeCursor>();
    }

    void Update()
    {

        
        if (gameController.IsGameStarted)
        {
            if (Input.GetMouseButton(0))
            {
                
                if (!isStartedReverse)
                {

                    float tension = rope.CalculateLength() / rope.restLength - 1;

                    if (tension > 0.01f)
                    {
                        cursor.ChangeLength(rope.restLength + forwardspeed * Time.deltaTime);
                    }
                    else
                    {
                        if (rope.restLength > minLength)
                        {
                            cursor.ChangeLength(rope.restLength - forwardspeed * Time.deltaTime);
                        }
                    }
                }

            }
        }

        
    }

    void FixedUpdate()
    {
        
        LimitMovement();
        if (gameController.IsGameStarted)
        {
            

            if (Input.GetMouseButton(0))
            {

                //SetPositionForBackwards();

                rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
                rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

                Vector3 m_EulerAngleVelocity = new Vector3(-rotationY, rotationX, 0f);

                Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * rotationSpeed * Time.fixedDeltaTime);
                rigidbody.rotation = (rigidbody.rotation * deltaRotation);

                rigidbody.MovePosition(rigidbody.position + transform.forward * forwardspeed * Time.fixedDeltaTime);



            }
            else
            {
                rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, Quaternion.Euler(rigidbody.transform.eulerAngles.x, rigidbody.transform.eulerAngles.y, 0f), 8f * Time.deltaTime);
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }


            if (rope.restLength >= maxLength)
            {
                /*gameController.EndLevelTPV();
                Invoke("ReverseArm", 2f);
                gameController.IsGameStarted = false;

                GetComponent<Collider>().enabled = false;

                DidWin = false;*/
            }
        }
        else
        {
            

            if (isStartedReverse)
            {

                if (BackCount >= BackwardsSetPointsList.Count)
                {
                    gameController.ArmObjects.SetActive(false);
                    gameController.ThieveLeanObject.SetActive(false);
                    gameController.ThieveObject.SetActive(true);
                    gameController.ThieveObject.transform.rotation = Quaternion.Euler(new Vector3(0f,130f,0f));
                    //gameController.ThieveHandledObject.SetActive(true);
                    isStartedReverse = false;
                    gameController.CameraSendToCharPos(DidWin);
                    
                }
                else
                {

                    transform.LookAt(BackwardsSetPointsList[BackCount].position);
                    HandVisual.transform.localPosition = new Vector3(0f,0f,-1.32f);
                    HandVisual.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                    Vector3 directionalVector  = (BackwardsSetPointsList[BackCount].position - transform.position).normalized * forwardspeed * 5f;
                    rigidbody.velocity = directionalVector;
                    
                    float distance = Vector3.Distance(rigidbody.position, BackwardsSetPointsList[BackCount].position);
                    if (distance <= 0.5f)
                    {
                        BackCount++;

                    }
                    
                    float tension = rope.CalculateLength() / rope.restLength - 1;

                    if (tension <= 0.05f)
                    {
                        if (rope.restLength > minLength)
                        {
                            cursor.ChangeLength(rope.restLength - forwardspeed * 5f * Time.deltaTime);
                        }
                    }

                    
                }
                
                
            }
        }
    }


    private void LimitMovement()
    {

        rigidbody.position = new Vector3(
        Mathf.Clamp(rigidbody.position.x, -4f, 3.5f),
        Mathf.Clamp(rigidbody.position.y, 0f, 1f),
        Mathf.Clamp(rigidbody.position.z, -5f, 5f));
    }

    private void LimitRotation()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        
        if (other.gameObject.CompareTag("Loot"))
        {
            gameController.EndLevelTPV();
            Invoke("ReverseArm", 2f);
            gameController.IsGameStarted = false;

            GetComponent<Collider>().enabled = false;


            HandledLootObject.SetActive(true);
            other.gameObject.SetActive(false);
            DidWin = true;
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

        if (other.gameObject.CompareTag("Obstacle"))
        {
            gameController.IsGameStarted = false;
            GetComponent<Collider>().enabled = false;
            rigidbody.isKinematic = true;
            DidWin = false;
            other.gameObject.SetActive(false);
        }
        
    }



    private void ReverseArm()
    {
        //rigidbody.isKinematic = false;
        BackwardsSetPointsList.Add(BasePoint);
        isStartedReverse = true;
    }

}

