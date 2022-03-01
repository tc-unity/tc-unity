using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class RockerContro : MonoBehaviour, IDragHandler, IEndDragHandler
{

   // public static RockerContro _rockerContro;
    private Vector2 _begin;
    private float range = 50;

    void Awake()
    {
        _begin = transform.position;
    }

    public void OnDrag(PointerEventData other)
    {
        transform.position = other.position;
        float myrange = Vector3.Distance(transform.position, _begin);
        if (myrange > range)
        {
            transform.position = _begin + (other.position - _begin).normalized * range;
        }
        else
        {
            transform.position = other.position;
        }
    }

    public void OnEndDrag(PointerEventData other)
    {
        transform.position = _begin;
    }

    public float X
    {
        get { return ((Vector2)transform.position - _begin).normalized.x; }
    }
    public float Y
    {
        get { return ((Vector2)transform.position - _begin).normalized.y; }
    }
}
