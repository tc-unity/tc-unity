using UnityEngine;
using System.Collections;

public class MouseCursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.lockState = CursorLockMode.Locked;
	}

    void OnGUI()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
			
    }

	// Update is called once per frame
	void Update () {
	
	}
}
