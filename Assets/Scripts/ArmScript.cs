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

    public int BackCount;

    private float BackTimeCounter;

    public float BackTime;

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
                SetPositionForBackwards();

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

        
    }

    void FixedUpdate()
    {
        
        LimitMovement();
        if (gameController.IsGameStarted)
        {
            

            if (Input.GetMouseButton(0))
            {

                

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
                gameController.EndLevelTPV();
                Invoke("ReverseArm", 2f);
                gameController.IsGameStarted = false;

                GetComponent<Collider>().enabled = false;

                DidWin = false;
            }
        }
        else
        {
            

            if (isStartedReverse)
            {

                if (BackCount < 0)
                {
                    gameController.ArmObjects.SetActive(false);
                    //gameController.ThieveHandledObject.SetActive(true);
                    isStartedReverse = false;
                    gameController.CameraSendToCharPos(DidWin);
                    
                }
                else
                {
                    //transform.position = Vector3.MoveTowards(transform.position, BackwardsTransformList[BackCount].position, 5f * forwardspeed * Time.deltaTime);

                    Vector3 directionalVector  = (BackwardsTransformList[BackCount].position - transform.position).normalized * forwardspeed * 5f;
                    //transform.LookAt(transform.position - BackwardsTransformList[BackCount].position);
                    rigidbody.velocity = directionalVector;
                    
                    float distance = Vector3.Distance(rigidbody.position, BackwardsTransformList[BackCount].position);
                    if (distance <= 0.5f)
                    {
                        BackCount--;

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

    private float ClaculateDisplacement()
    {
        displacementTime += Time.fixedDeltaTime;

        var acceleration = (rigidbody.velocity.magnitude - lastVelocity) / displacementTime;
        lastVelocity = rigidbody.velocity.magnitude;

        float disp = (rigidbody.velocity.magnitude * displacementTime) + 0.5f * acceleration * Mathf.Sqrt(displacementTime);


        return disp;
    }

    private void SetPositionForBackwards()
    {
        BackTimeCounter += Time.deltaTime;
        if (BackTimeCounter >= BackTime)
        {
            BackTimeCounter = 0;
            BackCount++;
            BackwardsTransformList[BackCount].position = transform.position;
        }
        
    }


    private void LimitMovement()
    {

        rigidbody.position = new Vector3(
        Mathf.Clamp(rigidbody.position.x, -5f, 5f),
        Mathf.Clamp(rigidbody.position.y, -1f, 1f),
        Mathf.Clamp(rigidbody.position.z, -5f, 5f));
    }

    private void LimitRotation()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {

        gameController.EndLevelTPV();
        Invoke("ReverseArm", 2f);
        gameController.IsGameStarted = false;

        GetComponent<Collider>().enabled = false;

        if (other.gameObject.CompareTag("Loot"))
        {
            HandledLootObject.SetActive(true);
            other.gameObject.SetActive(false);
            DidWin = true;
        }
        else
        {
            DidWin = false;
        }

    }



    private void ReverseArm()
    {
        //rigidbody.isKinematic = false;
        isStartedReverse = true;
    }

}

