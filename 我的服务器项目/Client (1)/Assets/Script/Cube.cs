using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour {

    public Transform tager;
	// Use this for initialization
	void Start () {
        if (gameObject.name=="Cube")
        {
            tager = GameObject.Find("Cube1").GetComponent<Transform>();
        }
        else
        {
            tager = GameObject.Find("Cube").GetComponent<Transform>();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void OnCollisionEnter(Collision colliderInfo)
    {
        if (colliderInfo.collider.name=="Plane")
        {
            tager.transform.position = Vector3.Lerp(tager.position,new Vector3(tager.position.x,10, tager.position.z),5);
        }
    }
}
