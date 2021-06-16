using Obi;
using UnityEngine;

public class HeadController : MonoBehaviour {
    private Rigidbody rb;
    public float amountUp = 10;
    public float amountZ = 10;
    private float amountZTemp;
    public float hoverHeight = 5;
    public float hoverForce = 200;
    private float hoverForceTemp;
    public ObiRope rope;
    public Vector3 com;
    public float d, y, z, AutoMovement;
    private Vector3 lookDir;
    public FloatingJoystick joystick;
    public float amplitude = 1;
    public float headAmplitude = 1;
    public float frequency = 1;
    public float headFrequency = 1;
    public float headRotSpeed = 5.0f;
    public Transform head;
    public float forwardMag;
    public SnakeCamera snakeCamera;
    public float forwardSign;
    public Vector3 comRb;

    private void Start() {
        AutoMovement = 0;
        rb = GetComponent<Rigidbody>();
        amountZTemp = amountZ;
        hoverForceTemp = hoverForce;
        //amountZ = 0;
        //hoverForce = 0;
    }

    private void Update() {
        //y = Input.GetAxis("Vertical");
        //z = Input.GetAxis("Horizontal");

        y = joystick.Vertical + Input.GetAxis("Vertical");
        z = joystick.Horizontal + Input.GetAxis("Horizontal") + AutoMovement;

        if (z > 0) {
            y += 0.6f;
        }
        else if (z < 0) {
            y += 0.6f;
        }

        if (y > -0.2f && y < 0) {
            y = 0f;
        }

        if (y > 0) {
            forwardSign = Mathf.Sign(z);
        }

        amountZ = 0;
        hoverForce = 0;
        amountZ = Mathf.Lerp(amountZ, amountZTemp, Mathf.Abs(z));
        hoverForce = Mathf.Lerp(hoverForce, hoverForceTemp, Mathf.Abs(y));
        //rb.centerOfMass = comRb;
    }

    private void FixedUpdate() {
        float m = rope.GetMass(out com);
        hoverHeight = GameManager._Instance.snakeCurrentLength + 1;
        Vector3 zForce = z * amountZ * m * Vector3.forward;
        Vector3 appliedHoverForce = Vector3.zero;
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out var hit, hoverHeight)) {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            appliedHoverForce = proportionalHeight * hoverForce * y * Vector3.up;
            rb.AddForce(appliedHoverForce, ForceMode.Force);
        }

        forwardMag = zForce.magnitude;
        Vector3 headBob = Mathf.Abs(z) *
                          new Vector3(Mathf.Sin(Time.time * headAmplitude) * headFrequency, 0, 0);

        rb.AddForce(zForce);

        //rb.AddRelativeForce(headBob, ForceMode.Force);
        //rb.AddForce(headBob);
        if (z > 0 || z < 0) {
            rope.solver.externalForces[1] = new Vector3(Mathf.Sin(Time.time * amplitude) * frequency, 0, 0);
            rope.solver.externalForces[2] = new Vector3(Mathf.Sin(Time.time * amplitude) * frequency, 0, 0);
            rope.solver.externalForces[3] = new Vector3(Mathf.Sin(Time.time * amplitude) * frequency, 0, 0);
//            rope.solver.externalForces[rope.activeParticleCount - 1] =
//                new Vector3(Mathf.Sin(Time.time * amplitude) * frequency, 0, 0);
        }


        lookDir = zForce + appliedHoverForce;
        Debug.DrawRay(transform.position, lookDir, Color.red);
        //transform.LookAt(transform.position + lookDir.normalized, Vector3.up);


        d = Vector3.Distance(com, transform.position);
        rope.RebuildConstraintsFromElements();
        Vector3 headLookDir = transform.position + transform.forward - head.position;
        Debug.DrawRay(head.position, headLookDir * 5, Color.blue);
        Quaternion lookRotation =
            Quaternion.LookRotation(headLookDir);
        head.rotation = Quaternion.Slerp(head.rotation, lookRotation, headRotSpeed * Time.fixedDeltaTime);


//        if (snakeCamera.manualUpdate) {
//            Vector3 pos = rope.GetParticlePosition(rope.activeParticleCount - 1);
//            snakeCamera.followPos = pos;
//            snakeCamera.ManualUpdateV2();
//        }
    }

    private void LateUpdate() {
//        Quaternion lookRotation =
//            Quaternion.LookRotation((transform.position + lookDir.normalized) - transform.position);
//        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, headRotSpeed * Time.deltaTime);


//        Quaternion lookRotation =
//            Quaternion.LookRotation((head.position + lookDir.normalized) - head.position);
//        head.rotation = Quaternion.Slerp(head.rotation, lookRotation, headRotSpeed * Time.deltaTime);

//        Quaternion lookRotation =
//            Quaternion.LookRotation((transform.position+transform.forward) - head.position);
//        head.rotation = Quaternion.Slerp(head.rotation, lookRotation, headRotSpeed * Time.deltaTime);
    }
}