using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (GameManager.instance.startFlag)
        {
            if (Input.GetMouseButton(0))
            {
                GameManager.instance.OverhangBoard();

            }
        }


    }
}
