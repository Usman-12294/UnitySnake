using System;
using System.Collections;
using Obi;
using UnityEngine;

public class SnakeCutter : MonoBehaviour {
    public ObiSolver solver;
    public ObiRope rope;


    private void OnEnable() {
//        solver.OnCollision += Solver_OnCollision;
    }

    private void Start() {
        rope = GameManager._Instance.rope;
        solver = GameManager._Instance.solver;
        //solver.OnCollision += Solver_OnCollision;
    }

    private void OnDisable() {
        //solver.OnCollision -= Solver_OnCollision;
    }

    void Solver_OnCollision(ObiSolver sender, Obi.ObiSolver.ObiCollisionEventArgs e) {
        foreach (Oni.Contact contact in e.contacts) {
            ObiColliderBase collider = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;
            //Debug.Log(collider.gameObject.name);
            if (collider.gameObject.CompareTag("Cutter") && contact.distance < 0.01f) {
                int particleIndex = solver.simplices[contact.bodyA];
                //Debug.Log(rope.solverIndices[particleIndex]);
                //Debug.Log(rope.solver.actors[0].activeParticleCount);
                float mu = rope.solverIndices[particleIndex] / (float) rope.solver.actors[0].activeParticleCount;
                float a = mu;
                rope.Tear(rope.GetElementAt(mu, out a));
                //GameManager._Instance.SnakePlayer.GetComponent<HeadControllerV2>().enabled = false;
                //GameManager._Instance.RestartLevel();
                Debug.Log("SnakeCutter "+contact.distance);
                //ObiColliderWorld.GetInstance().UpdateColliders();
                StartCoroutine(GameOver(1.0f));
                //rope.RebuildConstraintsFromElements();
                break;
//                return;
            }
        }
    }

    IEnumerator GameOver(float timer) {
        yield return new WaitForSeconds(timer);
        GameManager._Instance.RestartLevel();
    }
}