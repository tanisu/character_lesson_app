using UnityEngine;
using DG.Tweening;

public class StartController : MonoBehaviour
{

    public void OnClickStart()
    {
        if (!GameManager.instance.startFlag)
        {
            GameManager.instance.StartAct();
        }
    }
}
