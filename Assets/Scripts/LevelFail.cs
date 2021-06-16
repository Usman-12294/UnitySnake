using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelFail : MonoBehaviour
{
    public float WaitTime;
    private void OnEnable()
    {
        StartCoroutine(YouLose());
    }
    
    IEnumerator YouLose()
    {
        float TempFillAmount = 1 / WaitTime;
        while (WaitTime > 0.1)
        {
            this.GetComponent<Image>().fillAmount += TempFillAmount;
            WaitTime-=0.1f;
            yield return new WaitForSecondsRealtime(TempFillAmount);
        }
    }
}
