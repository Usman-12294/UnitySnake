using System;
using System.Collections;
using Obi;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeadControllerV2 : MonoBehaviour {
    private Rigidbody rb;
    public float zVelocity;
    public float upVelocity;
    public float hoverHeight = 5;
    public ObiRope rope;
    public Vector3 com;
    public float d, y, z, AutoMovement;
    private Vector3 lookDir;
    public FloatingJoystick joystick;
    public float amplitude = 1;
    public float frequency = 1;
    public float headRotSpeed = 5.0f;
    public Transform head;
    public GameObject bumpSphere;
    private ObiPathSmoother pathSmoother;
    [Range(0, 1.0f)] public float section = 0;
    public float sectionScale = 0;
    public float bumpTimer = 0.75f;
    public float inputFlag = 1;
    public float snakeLength;
    public Animator animator;

    private void OnEnable() {
        rope.solver.OnCollision += OnCol;
    }

    private void OnDisable() {
        rope.solver.OnCollision -= OnCol;
    }

    private void Start() {
        AutoMovement = 0;
        rb = GetComponent<Rigidbody>();
        pathSmoother = rope.GetComponent<ObiPathSmoother>();
    }


    private void Update() {
        y = (joystick.Vertical + Input.GetAxis("Vertical")) * inputFlag;
        z = (joystick.Horizontal + Input.GetAxis("Horizontal") + AutoMovement) * inputFlag;

        if (z > 0 || z < 0) {
            y += 0.15f;
            y = Mathf.Min(y, 1.0f);
        }

        //ObiPathFrame frame = pathSmoother.GetSectionAt(section);
        //Vector3 p = pathSmoother.transform.TransformPoint(frame.position);
        //debugSphere.transform.position = p;
        //rope.GetElementAt(section, out float mu);
        // for (int i = 0; i < rope.elements.Count; i++) {
        //     rope.solver.principalRadii[i] = Vector4.one * sectionScale;
        // }
        //rope.solver.principalRadii[(int) (rope.elements.Count * section)] = Vector4.one * sectionScale;
        //rope.solver.principalRadii[(int) (rope.elements.Count * section)] = Vector4.one;
        if (Input.GetKeyDown(KeyCode.Q)) {
            StartCoroutine(EmulateBodyBump());
            animator.SetTrigger("eat");
        }

        snakeLength = GameManager._Instance.snakeCurrentLength;
    }


    private void FixedUpdate() {
        Vector3 forwardSpeed = z * zVelocity * Vector3.forward;
        Debug.DrawRay(transform.position, forwardSpeed * 10, Color.green);
        rb.velocity += forwardSpeed;
        hoverHeight = GameManager._Instance.snakeCurrentLength * 0.75f;
        //hoverHeight = GameManager._Instance.snakeCurrentLength;
        Vector3 appliedHoverForce = Vector3.zero;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out var hit, hoverHeight)) {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            appliedHoverForce = proportionalHeight * upVelocity * y * Vector3.up;
            rb.velocity += appliedHoverForce;
        }

        if (z > 0 || z < 0) {
            rope.solver.externalForces[1] = new Vector3(Mathf.Sin(Time.time * amplitude) * frequency, 0, 0);
            rope.solver.externalForces[2] = new Vector3(Mathf.Sin(Time.time * amplitude) * frequency, 0, 0);
            rope.solver.externalForces[3] = new Vector3(Mathf.Sin(Time.time * amplitude) * frequency, 0, 0);
        }

        Vector3 lookSum = (appliedHoverForce + forwardSpeed);
        Debug.DrawRay(transform.position, appliedHoverForce * 10, Color.cyan);
        Debug.DrawRay(transform.position, lookSum * 10, Color.yellow);
        Debug.DrawRay(transform.position, transform.forward * 10, Color.blue);
        rope.RebuildConstraintsFromElements();
        //head.transform.rotation = transform.rotation * Quaternion.Inverse(transform.rotation);
        //head.transform.rotation = Quaternion.Inverse(transform.rotation) * transform.rotation;
        //head.transform.LookAt(transform.position + transform.forward);
        Vector3 headLookDir = transform.position + transform.forward - head.position;
        Quaternion lookRotation =
            Quaternion.LookRotation(headLookDir, Vector3.up).normalized;
        head.rotation = Quaternion.Slerp(head.rotation, lookRotation, headRotSpeed * Time.fixedDeltaTime);
    }

    private void OnCol(ObiSolver solver, ObiSolver.ObiCollisionEventArgs e) {
        if (e == null) return;
        foreach (Oni.Contact contact in e.contacts) {
            ObiColliderBase collider = ObiColliderWorld.GetInstance().colliderHandles[contact.bodyB].owner;
            if (!collider.CompareTag("Head") && !collider.CompareTag("Untagged")) {
                //Debug.Log(collider.gameObject.tag);
            }

            if (collider.CompareTag("Fruit")) {
                //Handheld.Vibrate();
                Debug.Log("Collectible");
                collider.GetComponent<Collectable>().Collect();
                animator.SetTrigger("eat");
                break;
            }


            if (collider.gameObject.CompareTag("Cutter") && contact.distance < 0.01f) {
                Handheld.Vibrate();
                int particleIndex = solver.simplices[contact.bodyA];
                //Debug.Log(rope.solverIndices[particleIndex]);
                //Debug.Log(rope.solver.actors[0].activeParticleCount);
                float mu = rope.solverIndices[particleIndex] / (float) rope.solver.actors[0].activeParticleCount;
                float a = mu;
                rope.Tear(rope.GetElementAt(mu, out a));
                rope.RebuildConstraintsFromElements();
                inputFlag = 0;
                //Debug.Log("SnakeCutter " + contact.distance);
                StartCoroutine(GameManager._Instance.RestartLevelAfterTime(2.0f));
                break;
            }
        }
    }

    public IEnumerator EmulateBodyBump() {
        GameObject bumpGo = Instantiate(bumpSphere);
        float elapsedTimer = 0;
        while (elapsedTimer < bumpTimer) {
            float percentage = 1 - (elapsedTimer / bumpTimer);
            float rad = rope.solver.principalRadii[(int) (rope.elements.Count * percentage)].x;
            ObiPathFrame frame = pathSmoother.GetSectionAt(percentage);
            Vector3 p = pathSmoother.transform.TransformPoint(frame.position);
            bumpGo.transform.localScale = Vector3.one * rad * 2.5f;
            bumpGo.transform.position = p;
            elapsedTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(bumpGo);
        rope.GetComponent<RopeExtender>().IncreaseLength();
    }
}