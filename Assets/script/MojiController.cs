using UnityEngine;


public class MojiController : MonoBehaviour
{

    public void onPointerExit()
    {
        if (Input.GetMouseButton(0) && GameManager.instance.startFlag)
        {
            GameManager.instance.OverCharacter();
        }
    }



    public void onPointerEnter()
    {
        if(Input.GetMouseButton(0) && GameManager.instance.startFlag)
        {
            GameManager.instance.EnterCharacter();
        }
    }

}
