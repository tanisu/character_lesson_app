using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
{
    public void OnPointerEnter()
    {
        GameManager.instance.onPanelFlag = true;
    }

    public void OnPointerExit()
    {
        GameManager.instance.onPanelFlag = false;
    }
}
