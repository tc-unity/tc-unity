using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonHpBarLookAt : MonoBehaviour {

    public Camera m_Camera;
	// Use this for initialization
	void Start () {
        m_Camera = GameObject.Find("Camera").GetComponent<Camera>();
        transform.LookAt(m_Camera.transform);
	}
	
	// Update is called once per frame
	void Update () {
        transform.LookAt(m_Camera.transform);
    }
  
}
