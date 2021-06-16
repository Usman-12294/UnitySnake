using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Obi;
using UnityEngine;

public class Collectable : MonoBehaviour {
    public int CollectableNumber;
    public DOTweenAnimation ChangeTransform;
    public DOTweenAnimation ChangeRotation;


    private void OnEnable() {
        if (transform.childCount > 0)
            transform.GetChild(CollectableNumber).gameObject.SetActive(true);

//        Transform tempPosition = this.transform;
//        tempPosition.position += new Vector3(0,0.15f,0);

//        ChangeTransform.endValueV3 = this.transform.localPosition + new Vector3(0,0.15f,0);
//        if(CollectableNumber > 2)
//            Debug.Log(ChangeTransform.endValueTransform.position);
//
//        ChangeTransform.enabled = true;
//        ChangeTransform.DORestart();
//        ChangeRotation.DORestart();
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Head")) {
            Collect();
        }
    }

    public void Collect() {
        Handheld.Vibrate();
        GameManager._Instance.UpdateCounter();
        GameManager._Instance.gameCollectibles[CollectableNumber].count++;
        GameManager._Instance.ShowSumUp(this.transform.position);
        GameManager._Instance.IncreaseLength();
        this.gameObject.SetActive(false);
    }
}