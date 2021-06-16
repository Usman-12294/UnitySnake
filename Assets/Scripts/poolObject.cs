using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class poolObject : MonoBehaviour {

    public List<GameObject> childCubes = new List<GameObject>();
    public List<Vector3> childrenPositions = new List<Vector3>();

    public float explosionForce,explosionRadius;
	
    // Use this for initialization
    void Start () {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            childCubes.Add(this.transform.GetChild(i).gameObject);
            childrenPositions.Add(this.transform.GetChild(i).localPosition);
        }
        this.gameObject.SetActive(false);
    }
	
    // Update is called once per frame
    void Update () {
		
    }

    [SerializeField] private float forceAmount = 5;
    public void explode()
    {
        foreach (var t in childCubes)
        {
            Rigidbody rb = t.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = false;
                //rb.AddForce(new Vector3(0, 1 ,1 ) * forceAmount,ForceMode.VelocityChange);
                //Debug.LogError("Wait");
                rb.AddExplosionForce(explosionForce, transform.forward,explosionRadius, 50);
            }
        }

        StartCoroutine(poolMe(2f));
    }

    IEnumerator poolMe(float wait)
    {
        yield return new WaitForSeconds(wait);
		
        for (int i = 0; i < childCubes.Count; i++)
        {
            Rigidbody rb = childCubes[i].GetComponent<Rigidbody>();
            if (rb)
            {
                rb.isKinematic = true;
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
            //Debug.LogError("Check: " + childCubes[i].name + " " + childrenPositions[i].name);
            childCubes[i].transform.localPosition = childrenPositions[i];
            childCubes[i].transform.localEulerAngles = Vector3.zero;
        }
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.Euler(0,0,0);
        this.gameObject.SetActive(false);
    }
}