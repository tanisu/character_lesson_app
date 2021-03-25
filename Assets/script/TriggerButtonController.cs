using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerButtonController : MonoBehaviour
{
    private bool toggle;
    private string toggleType;
    private List<int> dakutenPanelList = new List<int> { 1, 2, 3, 5 };
    private List<int> handakutenPanelList = new List<int> { 5 };
    private List<int> komojiPanelList = new List<int> { 3,7};
    private List<int> targetPanelList;
    void Start()
    {
        toggleType = transform.tag;
        toggle = false;
        GetComponent<Button>().onClick.AddListener(()=>
        {
            toggle = !toggle;
            if (toggle)
            {
                switch (toggleType)
                {
                    case "Dakuten":
                        targetPanelList = dakutenPanelList;
                        break;
                    case "Handakuten":
                        targetPanelList = handakutenPanelList;
                        break;
                    case "Komoji":
                        targetPanelList = komojiPanelList;
                        break;
                }
            }
            Debug.Log(targetPanelList.Count);
        });
    }

    
    
}
