using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour,IDragHandler
{
    public void OnDrag(PointerEventData eventData)
    {

        Vector3 targetPos = Camera.main.ScreenToWorldPoint(eventData.position);
        targetPos.z = 0;
        transform.position = targetPos;
        
    }


}
