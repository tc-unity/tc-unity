using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatPlan : MonoBehaviour {

    //获取聊天信息输入框
    private InputField InputField_Account;
    private Client clientScript;

    public Text chatText;
    public Transform chatTextPos;
    public ScrollRect scrollRect;

    private int count = 0;
    // Use this for initialization
    void Start () {
        InputField_Account = transform.GetChild(9).GetChild(0).GetComponent<InputField>();
        clientScript = GameObject.Find("Client").GetComponent<Client>();
        //scrollRect
        //chatText= transform.GetChild(9).GetChild(0).GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {


    }
    public void OnClick()
    {
        //消息类型+聊天内容
        string message = ((int)MessageType.TextInfo).ToString() + "_" + InputField_Account.text;
        clientScript.Send(message);
        InputField_Account.text = "";
        count++;


    }
    public void ChatContent(string msg)
    {
        if (count>=15)
        {
            Destroy(chatTextPos.transform.GetChild(0).gameObject);
        }
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();

        string[] tmpArray = msg.Split('_');
        Text obj = Instantiate(chatText, chatTextPos);
        obj = obj.GetComponent<Text>();
        obj.color = Color.green;
        obj.text = tmpArray[1] +":"+ "[" + tmpArray[2] + "]";

    }

}
