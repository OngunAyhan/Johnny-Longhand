using UnityEngine;
using Obi;

public class ObiCollisionDetection : MonoBehaviour
{
    ObiSolver solver;

    public GameController gameController;

    public ArmScript armScript;

    public int pointAmount;
    public int pointCounter;

    

    void Awake()
    {
        solver = GetComponent<ObiSolver>();
    }

    void OnEnable()
    {
        solver.OnCollision += Solver_OnCollision;
    }

    void OnDisable()
    {
        solver.OnCollision -= Solver_OnCollision;
    }

    void Solver_OnCollision(object sender, Obi.ObiSolver.ObiCollisionEventArgs e)
    {
        var world = ObiColliderWorld.GetInstance();

        // Oyun sonu geri dönme
        if (armScript.DidWin && pointCounter < pointAmount)
        {
            foreach (Oni.Contact contact in e.contacts)
            {
                
                if (contact.distance < 0.01)
                {

                    //armScript.BackCount++;
                    armScript.BackwardsTransformList[pointCounter].localPosition = (Vector3)contact.pointB;
                    armScript.BackwardsSetPointsList.Add(armScript.BackwardsTransformList[pointCounter]);
                    pointCounter++;
                    
                }
                
            }
        }

        if(gameController.IsGameStarted)//Oyun içi collusion
        {
            foreach (Oni.Contact contact in e.contacts)
            {

                if (contact.distance < 0.05)
                {
                    ObiColliderBase col = world.colliderHandles[contact.bodyB].owner;
                    if (col.CompareTag("Breakable"))
                    {
                        col.GetComponent<BreakableScript>().IsHitting = true;
                    }

                }
                else
                {
                    ObiColliderBase col = world.colliderHandles[contact.bodyB].owner;
                    if (col.CompareTag("Breakable"))
                    {
                        col.GetComponent<BreakableScript>().IsHitting = false;
                    }
                }
            }
        }
        
    }

}
