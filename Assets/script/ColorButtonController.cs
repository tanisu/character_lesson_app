using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ColorButtonController : MonoBehaviour
{
    
    public Color c;

    

    public void ChangeColor()
    {
        GameManager.instance.ChangeColor(c);
    }

}
