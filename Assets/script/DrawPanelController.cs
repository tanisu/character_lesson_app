using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPanelController : MonoBehaviour
{
    public static DrawPanelController i;
    private int currentCount;
    private int maxCount = 20;
    

    private void Awake()
    {
        i = this;
        
    }
    private void Start()
    {
        currentCount = 0;
    }
    public bool CheckMax()
    {
        if(currentCount >= maxCount)
        {
            return false;
        }
        return true;
    }

    public void AddCount()
    {
        currentCount++;
    }
    public void DelCount()
    {
        currentCount--;
    }
}
