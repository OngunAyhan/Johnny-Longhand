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

    public HandCoughtEffect handCoughtEffect;

    public Transform movementPivot;

    float tension;

    void Start()
    {
        //rope = GetComponent<ObiRope>();
        //cursor = GetComponent<ObiRopeCursor>();
    }

    void Update()
    {
        tension = rope.CalculateLength() / rope.restLength - 1;
    }

    void FixedUpdate()
    {
        
        
        if (gameController.IsGameStarted)
        {

            var lookDirection = movementPivot.position - rigidbody.position;
            
            Quaternion lookOrientation = Quaternion.LookRotation(lookDirection);

            if (lookOrientation != Quaternion.Euler( Vector3.zero))
            {
                rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, lookOrientation, rotationSpeed * Time.fixedDeltaTime);
            }

            float distance = Vector3.Distance(movementPivot.position, rigidbody.position);
            if (distance > 0.1f)
            {
                Vector3 pos = movementPivot.position - rigidbody.position;

                rigidbody.MovePosition(rigidbody.position + pos * forwardspeed * Time.fixedDeltaTime);
            }
            else
            {
                rigidbody.rotation = Quaternion.Lerp(rigidbody.rotation, Quaternion.Euler(rigidbody.transform.eulerAngles.x, rigidbody.transform.eulerAngles.y, 0f), 8f * Time.fixedDeltaTime);
                rigidbody.velocity = Vector3.zero;
                rigidbody.angularVelocity = Vector3.zero;
            }

            

            if (tension > 0.02f)
            {
                cursor.ChangeLength(rope.restLength + forwardspeed * tension * Time.fixedDeltaTime);
            }
            if (tension < 0.01f)
            {
                if (rope.restLength > minLength)
                {
                    cursor.ChangeLength(rope.restLength - forwardspeed * Time.fixedDeltaTime);
                }
            }


            if (rope.restLength >= maxLength)
            {
                FailedToRetrieveLoot();
            }
        }
        else
        {
            

            if (isStartedReverse)
            {

                if (BackCount >= BackwardsSetPointsList.Count)
                {
                    

                    isStartedReverse = false;
                    gameController.CameraSendToCharPos(DidWin);
                    
                }
                else
                {

                    transform.LookAt(BackwardsSetPointsList[BackCount].position);
                    HandVisual.transform.localPosition = new Vector3(0f,0f,-1.32f);
                    HandVisual.transform.localRotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                    Vector3 directionalVector  = (BackwardsSetPointsList[BackCount].position - transform.position).normalized;
                    rigidbody.velocity = directionalVector * Time.fixedDeltaTime * 400f;
                    
                    float distance = Vector3.Distance(rigidbody.position, BackwardsSetPointsList[BackCount].position);
                    if (distance <= 0.5f)
                    {
                        BackCount++;

                    }
                    

                    if (tension <= 0.05f)
                    {
                        if (rope.restLength > minLength)
                        {
                            cursor.ChangeLength(rope.restLength - Time.fixedDeltaTime * 4f);
                        }
                    }

                    
                }
                
                
            }
        }
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
            //el yapýþacak
            handCoughtEffect.enabled = true;
        }
        
    }

    public void FailedWithHandBlowEffect()
    {

    }


    public void FailedToRetrieveLoot()
    {
        gameController.IsGameStarted = false;

        GetComponent<Collider>().enabled = false;

        DidWin = false;
        FailedFunction();
    }

    public void FailedToRetrieveLoot(float delayTime)
    {
        gameController.IsGameStarted = false;

        GetComponent<Collider>().enabled = false;

        DidWin = false;
        Invoke("FailedFunction",delayTime);
    }

    private void FailedFunction()
    {
        
        gameController.CameraSendToCharPos(DidWin);
    }

    private void ReverseArm()
    {
        //rigidbody.isKinematic = false;
        BackwardsSetPointsList.Add(BasePoint);
        isStartedReverse = true;
    }

}

