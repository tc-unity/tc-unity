using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BornPoint : MonoBehaviour {
    
    private Client clientScript;
    void Awake()
    {
        clientScript = GameObject.Find("Client").GetComponent<Client>();

        string message = ((int)MessageType.ChangeScene).ToString() + "_ "+2;
        clientScript.Send(message);

        message = ((int)MessageType.CreateInfo).ToString();
        clientScript.Send(message);
    }
}
