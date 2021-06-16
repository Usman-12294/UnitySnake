using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ReplaceObjects : MonoBehaviour
{
   public GameObject NewObject;
   public GameObject[] OldObjects;
   
   

   [ContextMenu("Add Objects")]
   void AddObjects()
   {
      OldObjects = GameObject.FindGameObjectsWithTag("Fruit");
   }
   
   
   [ContextMenu("Replace Objects")]
   void ReplaceObject()
   {
      for (int i = 0; i < OldObjects.Length; i++)
      {
         Transform Parent = OldObjects[i].transform.parent;
         GameObject TempNewObject = Instantiate(NewObject, OldObjects[i].transform.position, OldObjects[i].transform.rotation, Parent);
         TempNewObject.GetComponent<Collectable>().CollectableNumber = Random.Range(0, 5);
         TempNewObject.GetComponent<Collectable>().ChangeTransform.endValueV3 = TempNewObject.transform.position + new Vector3(0,0.15f,0);
         DestroyImmediate(OldObjects[i]);
      }
   }
}
