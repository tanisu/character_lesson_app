using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MojiController : MonoBehaviour
{


    //private GameObject gameManager;
    

    private void Start()
    {
        
        //gameManager = GameObject.Find("GameManager");
    }


    public void onStartAction()
    {
        if (!GameManager.instance.startFlag)
        {
            GameManager.instance.startAct();
        }
        

    }

    public void onGoalAction()
    {
        if (Input.GetMouseButton(0) && GameManager.instance.startFlag)
        {
            GameManager.instance.goalAct();
            //GameManager.instance.completeCharacter();
            
        }
    }

    public void onNextAction()
    {
        if (Input.GetMouseButton(0) && GameManager.instance.startFlag)
        {
            GameManager.instance.nextStroke();
            
        }
        
    }



    public void onPointerExit()
    {
        if (Input.GetMouseButton(0) && GameManager.instance.startFlag)
        {
            GameManager.instance.overCharacter();
        }
    }

    public void onPointerEnter()
    {
        if(Input.GetMouseButton(0) && GameManager.instance.startFlag)
        {
            GameManager.instance.enterCharacter();
        }
    }

}
