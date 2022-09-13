using UnityEngine;
using Obi;

public class ObiCollisionDetection : MonoBehaviour
{
    ObiSolver solver;

    public GameObject Cube;

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
        foreach (Oni.Contact contact in e.contacts)
        {
            // if this one is an actual collision:
            if (contact.distance < 0.01)
            {
                

                /*ObiColliderBase col = world.colliderHandles[contact.bodyB].owner;
                  if (col != null)
                      {
                           Cube.transform.position = col.
                      }*/

                int particleIndex = solver.simplices[contact.bodyA];

                // do something with the particle, for instance get its position:
                Cube.transform.position = solver.positions.GetVector3(particleIndex);

                
            }
        }
    }

}
