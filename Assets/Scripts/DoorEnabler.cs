using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class DoorEnabler : MonoBehaviour
{
    public GameObject SwitchDoor;
    public GameObject Button;
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Head") || other.CompareTag("Weight"))
        {
            Button.transform.localPosition = new Vector3(Button.transform.localPosition.x, -1.50f, Button.transform.localPosition.z);
            other.GetComponent<MeshRenderer>().material.color = Color.green;
            if (other.CompareTag("Weight")) {
                //StartCoroutine(FixBox(other.gameObject, 0.3f));
                //other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                if (other.GetComponent<CubeType>().staticType)
                {
                    other.GetComponent<Rigidbody>().isKinematic = true;
                    gameObject.GetComponent<CapsuleCollider>().enabled = false;
                }
                //gameObject.SetActive(false);
                
            }
                
            SwitchDoor.GetComponent<MeshRenderer>().material.color = Color.green;
            SwitchDoor.GetComponent<DOTweenAnimation>().DORestartById("0");
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Head") || other.CompareTag("Weight"))
        {
            Button.transform.localPosition = new Vector3(Button.transform.localPosition.x, 0, Button.transform.localPosition.z);
            other.GetComponent<MeshRenderer>().material.color = Color.red;
//            other.GetComponent<Rigidbody>().isKinematic = false;
            SwitchDoor.GetComponent<MeshRenderer>().material.color = Color.red;
            SwitchDoor.GetComponent<DOTweenAnimation>().DORestartById("1");
        }
    }


    IEnumerator FixBox(GameObject other,float timer)
    {
        yield return new WaitForSeconds(timer);
        other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
    }

}
