using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WidthController : MonoBehaviour
{
    public float w;


    public void ChangeWidth()
    {
        GameManager.instance.ChangeWidth(w);
    }

}
