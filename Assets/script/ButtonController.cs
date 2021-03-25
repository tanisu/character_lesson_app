using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnClickButton);
    }

    

    private void OnClickButton()
    {
        AudioManager.I.ClickSoft();       
    }
}
