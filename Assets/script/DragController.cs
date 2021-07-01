using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour,IDragHandler,IDropHandler
{
    bool isGomibako = false;

 

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(eventData.position);
        targetPos.z = 0;
        transform.position = targetPos;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (isGomibako)
        {
            DrawPanelController.i.DelCount();
            AudioManager.I.DeleteImage();
            Destroy(gameObject.GetComponent<Image>().sprite.texture);
            Destroy(gameObject);
        }
        
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Gomibako") )
        {
            isGomibako = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Gomibako"))
        {
            isGomibako = false;
        }
    }

}
