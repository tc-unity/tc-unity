// this script controls the HP and Instantiates an HP Particle

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPScript : MonoBehaviour {

    public MonsterPro mon_Pro;
    //HpBar
    public Image hpBar;
    //the current HP of the character/gameobject
 //   public float MaxHp = 100;
	//public float HP; 

	//the HP Particle
	public GameObject HPParticle;

	//Default Forces
	public Vector3 DefaultForce = new Vector3(0f,1f,0f);
	public float DefaultForceScatter = 0.5f;
    public void Start()
    {
        EntrustCtrl.Instance.AddVessel(EventType_Game.ET_MonHp, PanelUpdate);
        //hpBar=
        PanelUpdate();
    }

    void PanelUpdate()
    {
        if (hpBar)
        {
            hpBar.fillAmount = mon_Pro.mHp / mon_Pro.mMaxHp;
        }
        
    }
    //Change the HP without an effect
    public void ChangeHP(float Delta)
	{

        mon_Pro.mHp = mon_Pro.mHp + Delta;
        // 触发掉血事件
        EntrustCtrl.Instance.TriggerEvent(EventType_Game.ET_MonHp);

    }

	//Change the HP and Instantiates an HP Particle with a Custom Force and Color
	public void ChangeHP(float Delta,Vector3 Position, Vector3 Force, float ForceScatter, Color ThisColor)
	{
        mon_Pro.mHp = mon_Pro.mHp + Delta;
        //触发掉血事件
        EntrustCtrl.Instance.TriggerEvent(EventType_Game.ET_MonHp);

        GameObject NewHPP = Instantiate(HPParticle,Position,gameObject.transform.rotation) as GameObject;
		NewHPP.GetComponent<AlwaysFace>().Target = GameObject.Find("Camera").gameObject;

		TextMesh TM  = NewHPP.transform.Find("HPLabel").GetComponent<TextMesh>();

		if (Delta > 0)
		{
			TM.text = "+" + Delta.ToString();
		}
		else
		{
			TM.text = Delta.ToString();
		}

		TM.color =  ThisColor;
        //hpBar.fillAmount = HP / MaxHp;
        NewHPP.GetComponent<Rigidbody>().AddForce( new Vector3(Force.x + Random.Range(-ForceScatter,ForceScatter),Force.y + Random.Range(-ForceScatter,ForceScatter),Force.z + Random.Range(-ForceScatter,ForceScatter)));
	}

	//Change the HP and Instantiates an HP Particle with a Custom Force
	public void ChangeHP(float Delta,Vector3 Position, Vector3 Force, float ForceScatter)
	{
        mon_Pro.mHp = mon_Pro.mHp + Delta;

        //触发掉血事件
        EntrustCtrl.Instance.TriggerEvent(EventType_Game.ET_MonHp);

        GameObject NewHPP = Instantiate(HPParticle,Position,gameObject.transform.rotation) as GameObject;
		NewHPP.GetComponent<AlwaysFace>().Target = GameObject.Find("Main Camera").gameObject;
		
		TextMesh TM  = NewHPP.transform.Find("HPLabel").GetComponent<TextMesh>();

		if (Delta > 0f)
		{
			TM.text = "+" + Delta.ToString();
			TM.color =  new Color(0f,1f,0f,1f);
		}
		else
		{
			TM.text = Delta.ToString();
			TM.color =  new Color(1f,0f,0f,1f);
		}
        //hpBar.fillAmount = HP / MaxHp;
        NewHPP.GetComponent<Rigidbody>().AddForce( new Vector3(Force.x + Random.Range(-ForceScatter,ForceScatter),Force.y + Random.Range(-ForceScatter,ForceScatter),Force.z + Random.Range(-ForceScatter,ForceScatter)));
	}

	//Change the HP and Instantiates an HP Particle with a Custom Color
	public void ChangeHP(float Delta,Vector3 Position, Color ThisColor)
	{
        mon_Pro.mHp = mon_Pro.mHp + Delta;

        //触发掉血事件
        EntrustCtrl.Instance.TriggerEvent(EventType_Game.ET_MonHp);

        GameObject NewHPP = Instantiate(HPParticle,Position,gameObject.transform.rotation) as GameObject;
		NewHPP.GetComponent<AlwaysFace>().Target = GameObject.Find("Main Camera").gameObject;
		
		TextMesh TM  = NewHPP.transform.Find("HPLabel").GetComponent<TextMesh>();

		if (Delta > 0)
		{
			TM.text = "+" + Delta.ToString();
		}
		else
		{
			TM.text = Delta.ToString();
		}

		TM.color =  ThisColor;
       // hpBar.fillAmount = HP / MaxHp;
        NewHPP.GetComponent<Rigidbody>().AddForce(new Vector3(DefaultForce.x + Random.Range(-DefaultForceScatter,DefaultForceScatter),DefaultForce.y + Random.Range(-DefaultForceScatter,DefaultForceScatter),DefaultForce.z + Random.Range(-DefaultForceScatter,DefaultForceScatter)));
	}

	//Change the HP and Instantiates an HP Particle with default force and color
	public void ChangeHP(float Delta,Vector3 Position)
	{
        mon_Pro.mHp = mon_Pro.mHp + Delta;

        //触发掉血事件
        EntrustCtrl.Instance.TriggerEvent(EventType_Game.ET_MonHp);

        GameObject NewHPP = Instantiate(HPParticle,Position,gameObject.transform.rotation) as GameObject;
		NewHPP.GetComponent<AlwaysFace>().Target = GameObject.Find("Camera").gameObject;
		
		TextMesh TM  = NewHPP.transform.Find("HPLabel").GetComponent<TextMesh>();

		if (Delta > 0f)
		{
			TM.text = "+" + Delta.ToString();
			TM.color =  new Color(0f,1f,0f,1f);
		}
		else
		{
			TM.text = Delta.ToString();
			TM.color =  new Color(1f,0f,0f,1f);
		}
       // hpBar.fillAmount = HP / MaxHp;

        NewHPP.GetComponent<Rigidbody>().AddForce( new Vector3(DefaultForce.x + Random.Range(-DefaultForceScatter,DefaultForceScatter),DefaultForce.y + Random.Range(-DefaultForceScatter,DefaultForceScatter),DefaultForce.z + Random.Range(-DefaultForceScatter,DefaultForceScatter)));
	}

	//Change the HP and Instantiates an HP Particle with Custom Text
	public void ChangeHP(float Delta,Vector3 Position, string text)
	{
        mon_Pro.mHp = mon_Pro.mHp + Delta;

        //触发掉血事件
        EntrustCtrl.Instance.TriggerEvent(EventType_Game.ET_MonHp);

        GameObject NewHPP = Instantiate(HPParticle,Position,gameObject.transform.rotation) as GameObject;
		NewHPP.GetComponent<AlwaysFace>().Target = GameObject.Find("Main Camera").gameObject;
		
		TextMesh TM  = NewHPP.transform.Find("HPLabel").GetComponent<TextMesh>();
		TM.text = text;
		
		if (Delta > 0f)
		{
			TM.color =  new Color(0f,1f,0f,1f);
		}
		else
		{
			TM.color =  new Color(1f,0f,0f,1f);
		}
        //hpBar.fillAmount = HP / MaxHp;

        NewHPP.GetComponent<Rigidbody>().AddForce( new Vector3(DefaultForce.x + Random.Range(-DefaultForceScatter,DefaultForceScatter),DefaultForce.y + Random.Range(-DefaultForceScatter,DefaultForceScatter),DefaultForce.z + Random.Range(-DefaultForceScatter,DefaultForceScatter)));
	}

	//Change the HP and Instantiates an HP Particle with Custom Text and Force,
	public void ChangeHP(float Delta,Vector3 Position, Vector3 Force, float ForceScatter, string text)
	{
        mon_Pro.mHp = mon_Pro.mHp + Delta;

        //触发掉血事件
        EntrustCtrl.Instance.TriggerEvent(EventType_Game.ET_MonHp);

        GameObject NewHPP = Instantiate(HPParticle,Position,gameObject.transform.rotation) as GameObject;
		NewHPP.GetComponent<AlwaysFace>().Target = GameObject.Find("Main Camera").gameObject;
		
		TextMesh TM  = NewHPP.transform.Find("HPLabel").GetComponent<TextMesh>();
		TM.text = text;
		
		if (Delta > 0f)
		{
			TM.color =  new Color(0f,1f,0f,1f);
		}
		else
		{
			TM.color =  new Color(1f,0f,0f,1f);
		}

       // hpBar.fillAmount = HP / MaxHp;
        NewHPP.GetComponent<Rigidbody>().AddForce( new Vector3(Force.x + Random.Range(-ForceScatter,ForceScatter),Force.y + Random.Range(-ForceScatter,ForceScatter),Force.z + Random.Range(-ForceScatter,ForceScatter)));
	}

	//Change the HP and Instantiates an HP Particle with Custom Text, Force and Color
	public void ChangeHP(float Delta,Vector3 Position, Vector3 Force, float ForceScatter, Color ThisColor, string text)
	{
        mon_Pro.mHp = mon_Pro.mHp + Delta;

        //触发掉血事件
        EntrustCtrl.Instance.TriggerEvent(EventType_Game.ET_MonHp);

        GameObject NewHPP = Instantiate(HPParticle,Position,gameObject.transform.rotation) as GameObject;
		NewHPP.GetComponent<AlwaysFace>().Target = GameObject.Find("Main Camera").gameObject;
		
		TextMesh TM  = NewHPP.transform.Find("HPLabel").GetComponent<TextMesh>();
		TM.text = text;
		TM.color =  ThisColor;
       // hpBar.fillAmount = HP / MaxHp;
        NewHPP.GetComponent<Rigidbody>().AddForce( new Vector3(Force.x + Random.Range(-ForceScatter,ForceScatter),Force.y + Random.Range(-ForceScatter,ForceScatter),Force.z + Random.Range(-ForceScatter,ForceScatter)));
	}

	//Change the HP and Instantiates an HP Particle with Custom Text and Color
	public void ChangeHP(float Delta,Vector3 Position, Color ThisColor, string text)
	{
        mon_Pro.mHp = mon_Pro.mHp + Delta;

        //触发掉血事件
        EntrustCtrl.Instance.TriggerEvent(EventType_Game.ET_MonHp);

        GameObject NewHPP = Instantiate(HPParticle,Position,gameObject.transform.rotation) as GameObject;
		NewHPP.GetComponent<AlwaysFace>().Target = GameObject.Find("Main Camera").gameObject;
		
		TextMesh TM  = NewHPP.transform.Find("HPLabel").GetComponent<TextMesh>();
		TM.text = text;
		TM.color =  ThisColor;
      //  hpBar.fillAmount = HP / MaxHp;
        NewHPP.GetComponent<Rigidbody>().AddForce( new Vector3(DefaultForce.x + Random.Range(-DefaultForceScatter,DefaultForceScatter),DefaultForce.y + Random.Range(-DefaultForceScatter,DefaultForceScatter),DefaultForce.z + Random.Range(-DefaultForceScatter,DefaultForceScatter)));
	}
	
}
