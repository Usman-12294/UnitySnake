using System;
using System.Collections;
using System.Collections.Generic;
using Obi;
using UnityEngine;

public class AntiGravitySnake : MonoBehaviour {
    private HeadController head;
    private ObiRope rope;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Head")) {
            HeadController headController = other.GetComponent<HeadController>();
            rope = headController.rope;
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Head")) {
            rope.solver.parameters.gravity = Vector3.up * 9.8f;
            rope.solver.PushSolverParameters();
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Head")) {
            rope.solver.parameters.gravity = Vector3.up * -9.8f;
            rope.solver.PushSolverParameters();
        }
    }
}