using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class DoorLock : MonoBehaviour {
    public int requiredFruits = 10;
    public Text remainingFruit;
    private bool locked = true;
    public GameObject tickObj;

    private void Start() {
        InvokeRepeating(nameof(UpdateRemainingFruitText), 1.0f, 0.05f);
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Head")) {
            if (GameManager._Instance.CollectiblesCounter >= requiredFruits) {
                if (locked)
                {
                    this.GetComponent<BoxCollider>().enabled = false;
                    GameManager._Instance.ExplodeDoor(this.transform.position);
//                    GetComponent<DOTweenAnimation>().DORestart();
                    locked = false;
                    this.gameObject.SetActive(false);
                }
            }
            else {
                Debug.Log("Locked");
                int remainingFruits = requiredFruits - GameManager._Instance.CollectiblesCounter;
            }
        }
    }

    void UpdateRemainingFruitText() {
        int remainingFruits = requiredFruits - GameManager._Instance.CollectiblesCounter;
        if (remainingFruits > 0) {
            remainingFruit.text = "<color=red>" + remainingFruits + "</color>";
        }
        else {
            //remainingFruit.text = "Door is unlocked, hit the door to open it";
            remainingFruit.gameObject.SetActive(false);
            tickObj.SetActive(true);
        }
    }
}