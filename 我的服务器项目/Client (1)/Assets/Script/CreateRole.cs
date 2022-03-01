using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateRole : MonoBehaviour {

    private InputField InputField_RoleName;
    private Text TipsText;
    private Client clientScript;
    private GameObject playerPreView;
    private Slider Color_R;
    private Slider Color_G;
    private Slider Color_B;
    public void Start()
    {
        InputField_RoleName = transform.Find("InputField_RoleName").GetComponent<InputField>();
        TipsText = transform.Find("TipsText").GetComponent<Text>();
        clientScript = GameObject.Find("Client").GetComponent<Client>();
        playerPreView = GameObject.Find("playerPreView");
        Color_R = transform.Find("Color_R").GetComponent<Slider>();
        Color_G = transform.Find("Color_G").GetComponent<Slider>();
        Color_B = transform.Find("Color_B").GetComponent<Slider>();
    }

    /// <summary>
    /// 确定按钮点击回调
    /// </summary>
    public void OnComfirmClicked()
    {
        //检查输入的值是否正确
        if (string.IsNullOrEmpty(InputField_RoleName.text))
        {
            TipsText.text = "角色名不能为空";
            return;
        }

        //发送消息
        string message = ((int)MessageType.CreateRole).ToString() + "_"
            + InputField_RoleName.text + "_" +
            Color_R.value + "_" + Color_G.value + "_" + Color_B.value;
        clientScript.Send(message);
    }

    //颜色滑动条的事件回调
    public void OnColor_RValueChange(float value)
    {
        Color color = playerPreView.GetComponent<MeshRenderer>().material.color;
        playerPreView.GetComponent<MeshRenderer>().material.color = new Color(Color_R.value, color.g, color.b);
    }
    public void OnColor_GValueChange(float value)
    {
        Color color = playerPreView.GetComponent<MeshRenderer>().material.color;
        playerPreView.GetComponent<MeshRenderer>().material.color = new Color(color.r, Color_G.value, color.b);
    }
    public void OnColor_BValueChange(float value)
    {
        Color color = playerPreView.GetComponent<MeshRenderer>().material.color;
        playerPreView.GetComponent<MeshRenderer>().material.color = new Color(color.r, color.g, Color_B.value);
    }
}
