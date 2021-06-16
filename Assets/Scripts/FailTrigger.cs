using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            GameManager._Instance.RestartLevel();
        }
    }
}
