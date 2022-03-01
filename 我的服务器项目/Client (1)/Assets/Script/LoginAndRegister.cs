using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginAndRegister : MonoBehaviour {
    private Transform Panel_Login;
    private Transform Panel_Register;

    private InputField InputField_Account_Login;
    private InputField InputField_Password_Login;

    private InputField InputField_Account_Register;
    private InputField InputField_Password_Register;
    private InputField InputField_Password2_Register;
    private Text TipsText;
    private Client clientScript;

    public void Start()
    {
        Panel_Login = transform.Find("Panel_Login");
        Panel_Register = transform.Find("Panel_Register");

        InputField_Account_Login = Panel_Login.Find("InputField_Account").GetComponent<InputField>();
        InputField_Password_Login = Panel_Login.Find("InputField_Password").GetComponent<InputField>();

        InputField_Account_Register = Panel_Register.Find("InputField_Account").GetComponent<InputField>();
        InputField_Password_Register = Panel_Register.Find("InputField_Password").GetComponent<InputField>();
        InputField_Password2_Register = Panel_Register.Find("InputField_Password2").GetComponent<InputField>();

        TipsText = transform.Find("TipsText").GetComponent<Text>();

        clientScript = GameObject.Find("Client").GetComponent<Client>();
    }

    /// <summary>
    /// 注册按钮回调
    /// </summary>

    public void OnRegisterBtnClicked()
    {
        Panel_Login.gameObject.SetActive(false);
        Panel_Register.gameObject.SetActive(true);
    }

    /// <summary>
    /// 返回按钮回调
    /// </summary>
    public void OnBackBtnClicked()
    {
        Panel_Login.gameObject.SetActive(true);
        Panel_Register.gameObject.SetActive(false);
    }

    /// <summary>
    /// 注册确定按钮回调
    /// </summary>
    public void OnComfirmBtnClicked()
    {
        //检查输入的值是否正确
        if (string.IsNullOrEmpty(InputField_Account_Register.text))
        {
            TipsText.text = "账号不能为空";
            return;
        }
        else if (string.IsNullOrEmpty(InputField_Password_Register.text))
        {
            TipsText.text = "密码不能为空";
            return;
        }
        else if (string.IsNullOrEmpty(InputField_Password2_Register.text))
        {
            TipsText.text = "确认密码不能为空";
            return;
        }
        else if (InputField_Password_Register.text != InputField_Password2_Register.text)
        {
            TipsText.text = "两次输入的密码不一致！";
            return;
        }

        //发送消息
        string message = ((int)MessageType.Register).ToString() + "_"
            + InputField_Account_Register.text + "_" + InputField_Password_Register.text;
        clientScript.Send(message);
    }

    /// <summary>
    /// 登录按钮点击回调
    /// </summary>
    public void OnLoginBtnClicked()
    {
        //检查输入的值是否正确
        if (string.IsNullOrEmpty(InputField_Account_Login.text))
        {
            TipsText.text = "账号不能为空";
            return;
        }
        else if (string.IsNullOrEmpty(InputField_Password_Login.text))
        {
            TipsText.text = "密码不能为空";
            return;
        }

        //发送消息
        string message = ((int)MessageType.Login).ToString() + "_"
            + InputField_Account_Login.text + "_" + InputField_Password_Login.text;
        clientScript.Send(message);
    }

    void ClearTextValue()
    {
        InputField_Account_Login.text = "";
        InputField_Password_Login.text = "";

        InputField_Account_Register.text = "";
        InputField_Password_Register.text = "";
        InputField_Password2_Register.text = "";

        TipsText.text = "";
    }

    IEnumerator ClearInputStr()
    {
        yield return new WaitForSeconds(1.5f);
        //清除输入文本内容
        ClearTextValue();
    }
    IEnumerator BackToLogin()
    {
        yield return new WaitForSeconds(1.5f);
        Panel_Login.gameObject.SetActive(true);
        Panel_Register.gameObject.SetActive(false);
    }

    public void ReceiveRegisterMessage(string msg)
    {
        string[] tmpArray = msg.Split('_');
        if (tmpArray[1] == "0")
        {
            TipsText.text = "账号已存在，注册失败！";
            StartCoroutine(ClearInputStr());
        }
        else if (tmpArray[1] == "1")
        {
            TipsText.text = "注册成功！";
            StartCoroutine(BackToLogin());
        }
    }

    public void ReceiveLoginMessage(string msg)
    {
        string[] tmpArray = msg.Split('_');
        if (tmpArray[1] == "0")
        {
            TipsText.text = "账号或者密码错误，登录失败！";
            StartCoroutine(ClearInputStr());
        }
        else if (tmpArray[1] == "1")
        {
            TipsText.text = "登录成功！";
            string message = ((int)MessageType.ChangeScene).ToString() ;
            if (tmpArray[2] == "1")//有角色
            {
                SceneManager.LoadScene("main");
               message += "_" + 2;
            }
            else if (tmpArray[2] == "0")//无角色
            {
                SceneManager.LoadScene("CreateRole");
                message += "_" + 1;
            } 
            
            clientScript.Send(message);
        }
    }
}
