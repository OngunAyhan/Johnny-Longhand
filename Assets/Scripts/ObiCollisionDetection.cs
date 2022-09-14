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

        // just iterate over all contacts in the current frame:
        if (armScript.DidWin && pointCounter < pointAmount)
        {
            foreach (Oni.Contact contact in e.contacts)
            {
                // if this one is an actual collision:
                if (contact.distance < 0.01)
                {

                    //armScript.BackCount++;
                    armScript.BackwardsTransformList[pointCounter].localPosition = (Vector3)contact.pointB;
                    armScript.BackwardsSetPointsList.Add(armScript.BackwardsTransformList[pointCounter]);
                    pointCounter++;


                    /*ObiColliderBase col = world.colliderHandles[contact.bodyB].owner;
                    if (col != null)
                    {
                        
                    }
                    */
                }
            }
        }
        
    }

}
