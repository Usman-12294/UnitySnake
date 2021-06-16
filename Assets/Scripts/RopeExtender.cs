using Obi;
using UnityEngine;

public class RopeExtender : MonoBehaviour {
    private ObiRope rope;
    private ObiRopeCursor ropeCursor;
    public float length;
    public float mass;
    public Vector3 com;

    private void Start() {
        rope = GetComponent<ObiRope>();
        ropeCursor = GetComponent<ObiRopeCursor>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            IncreaseLength();
        }
    }

    public void IncreaseLength() {
        ropeCursor.ChangeLength(rope.restLength + 0.2f);
        length = rope.CalculateLength();
        mass = rope.GetMass(out com);
    }

    public void DecreaseLength() {
        ropeCursor.ChangeLength(rope.restLength - 0.2f);
        length = rope.CalculateLength();
        mass = rope.GetMass(out com);
    }
}