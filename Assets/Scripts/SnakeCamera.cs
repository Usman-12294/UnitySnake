using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeCamera : MonoBehaviour {
    public Transform followTransform;
    public float followSpeed = 2;
    private Vector3 initialOffset;
    public bool manualUpdate = false;
    public Vector3 followPos;

    private void Start() {
        initialOffset = transform.position - followTransform.position;
        //StartCoroutine(LateFixedUpdate());
    }

    void UpdateCam() {
        Vector3 newPos = initialOffset + new Vector3(0, followTransform.position.y, followTransform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.fixedDeltaTime * followSpeed);
    }

    IEnumerator LateFixedUpdate() {
        while (true) {
            UpdateCam();
            yield return new WaitForFixedUpdate();
        }
    }

    void FixedUpdate() {
        if (!manualUpdate) {
            UpdateCam();
        }
    }

    public void ManualUpdate() {
        UpdateCam();
    }

    public void ManualUpdateV2() {
        Vector3 newPos = initialOffset + new Vector3(0, followPos.y, followPos.z);
        transform.position = Vector3.Lerp(transform.position, newPos, Time.fixedDeltaTime * followSpeed);
    }
}