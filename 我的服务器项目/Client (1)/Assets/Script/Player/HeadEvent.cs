using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadEvent : MonoBehaviour 
{

    public Text Lv;
    public Image HpPic;
    public Image MpPic;
    public Text t_hp;
    public Text t_mp;
    public Text m_name;
    public Text Kill;
    public Text Surplus;
    public Text Score;
    public Text Level;
    // 用于初始化 只执行一次
    void Start () 
	{
        PanelUpdate();

        EntrustCtrl.Instance.AddVessel(EventType_Game.ET_AttributeUpdate, PanelUpdate);
        EntrustCtrl.Instance.AddVessel(EventType_Game.ET_LvUp, PanelUpdate);
	}
	

	public void PanelUpdate () 
	{
        
        var modle = PlayerCtrl.Instance;

        Lv.text = "Lv:" + modle.mLv;
        m_name.text = modle.mName;
        t_hp.text = modle.mHp + "/" + modle.mMaxHp;
        t_mp.text = modle.mMp + "/" + modle.mMaxMp;
        HpPic.fillAmount = (float)modle.mHp / (float)modle.mMaxHp;
        MpPic.fillAmount = (float)modle.mMp / (float)modle.mMaxMp;

        Kill.text= modle.mKill.ToString();
        Surplus.text= modle.mSurplus.ToString();
        Score.text = modle.mScore.ToString();
        Level.text = modle.mLevel.ToString();
        Debug.Log(modle.mName);

    }
}
