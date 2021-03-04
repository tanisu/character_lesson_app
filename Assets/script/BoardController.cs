using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D collision)
    {
        
        if (Input.GetMouseButton(0) )
        {
            GameManager.instance.OverhangBoard();
            
        }
        else if (Input.GetMouseButtonUp(0) && GameManager.instance.onBoardFlag)
        {
            GameManager.instance.QuitStroke();
        }

    }
}
