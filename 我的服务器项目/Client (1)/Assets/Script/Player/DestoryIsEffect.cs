using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryIsEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //transform.position = Vector3.zero;
        Destroy(gameObject, 2);
	}

}
