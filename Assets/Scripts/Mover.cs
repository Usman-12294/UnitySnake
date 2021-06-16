using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    public float speed = 2.0f;
    private Rigidbody rb;
    public float amplitude;
    public float frequency;

    private void Start() {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        transform.Translate((new Vector3(0, 0, x) * speed) * Time.deltaTime);
        transform.Translate((new Vector3(0, y, 0) * speed) * Time.deltaTime);
        //rb.AddRelativeForce((new Vector3(0, 0, x) * speed));
        //rb.AddRelativeForce(transform.forward * speed, ForceMode.Impulse);
        //rb.MovePosition(new Vector3(0, 0, x) * speed * Time.deltaTime);
        //rb.MovePosition(new Vector3(0, y, 0) * speed * Time.deltaTime);

        transform.position = new Vector3(Mathf.Cos(Time.time * amplitude) * frequency, 0, 0);
    }
}