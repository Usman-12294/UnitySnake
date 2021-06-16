using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    [SerializeField] private GameObject CheckFlag;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Head"))
        {
            CheckFlag.GetComponent<DOTweenAnimation>().DORestart();
            GameManager._Instance.SaveCheckpoint(this.transform.position);
            this.GetComponent<BoxCollider>().enabled = false;
        }
    }
}
