using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHurt : MonoBehaviour {

    public enum EnumChangeHPType { Normal = 0, CustomColor = 1, CustomForce = 2, CustomColorandForce = 3, CustomText = 4 };
    public EnumChangeHPType ChangeHPType = EnumChangeHPType.Normal;
    public Color CustomColor;
    public Vector3 CustomForce;
    public float CustomForceScatter;

    public void OnTriggerEnter(Collider collider)
    {
        Debug.Log(collider.tag );
        if (collider.tag=="Monster"|| collider.tag=="BossMon")
        {
         
            if (ChangeHPType == EnumChangeHPType.Normal)
            {
                collider.gameObject.GetComponent<HPScript>().ChangeHP(-80, collider.transform.position);
            }
            else if (ChangeHPType == EnumChangeHPType.CustomColor)
            {
                collider.gameObject.GetComponent<HPScript>().ChangeHP(-80, collider.transform.position, CustomColor);
            }
            else if (ChangeHPType == EnumChangeHPType.CustomForce)
            {
                collider.gameObject.GetComponent<HPScript>().ChangeHP(-80, collider.transform.position, CustomForce, CustomForceScatter);
            }
            else if (ChangeHPType == EnumChangeHPType.CustomColorandForce)
            {
                collider.gameObject.GetComponent<HPScript>().ChangeHP(-80, collider.transform.position, CustomForce, CustomForceScatter, CustomColor);
            }
            else if (ChangeHPType == EnumChangeHPType.CustomText)
            {
                collider.gameObject.GetComponent<HPScript>().ChangeHP(-80, collider.transform.position, "Custom Text");
            }

        }
    }

}
