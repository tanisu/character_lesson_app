using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.EnterGoal();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameManager.instance.enterGoal = false;
    }
}
