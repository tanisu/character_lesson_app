using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

public class CountController : MonoBehaviour
{
    private string[][] charcters = new string[][]{ 
        new string[]{ "‚ ", "‚¢", "‚¤", "‚¦", "‚¨" },
        new string[]{ "‚©", "‚«", "‚­", "‚¯", "‚±" },
        new string[]{ "‚³", "‚µ", "‚·", "‚¹", "‚»" },
        new string[]{ "‚½", "‚¿", "‚Â", "‚Ä", "‚Æ" },
        new string[]{ "‚È", "‚É", "‚Ê", "‚Ë", "‚Ì" },
        new string[]{ "‚Í", "‚Ð", "‚Ó", "‚Ö", "‚Ù" },
        new string[]{ "‚Ü", "‚Ý", "‚Þ", "‚ß", "‚à" },
        new string[]{ "‚â",  "‚ä", "‚æ" },
        new string[]{ "‚ç", "‚è", "‚é", "‚ê", "‚ë" },
        new string[]{ "‚í",  "‚ð",  "‚ñ" },
    };
    //private int charactersCount = -1;
    [SerializeField]
    private Button buttonInstance;
    [SerializeField]
    private GameObject spacer;
    [SerializeField]
    private GameObject columnPanel;
    [SerializeField]
    private GameObject SceneManager;

    private int buttonCount;

    private List<GameObject> columnPanalList;
    void Start()
    {
        columnPanalList = new List<GameObject>();
        
        for(int column = 0;column < charcters.Length; column++)
        {
            GameObject panel = Instantiate<GameObject>(columnPanel,this.transform);
            columnPanalList.Add(panel);
            for (int index = 0;index < charcters[column].Length; index++)
            {
                if(charcters[column].Length == 3 && index > 0)
                {
                    Instantiate<GameObject>(spacer, columnPanalList[column].transform);
                }
                Button button = Instantiate<Button>(buttonInstance, columnPanalList[column].transform);
                buttonCount++;               
                Text buttonText = button.transform.GetChild(0).GetComponent<Text>();
                buttonText.text = charcters[column][index];
                int targetStage = buttonCount -1;
                button.onClick.AddListener(
                    () => {
                        //SceneManager.GetComponent<Scene>().PlayClickSound();
                        SceneManager.GetComponent<Scene>().OnClickCharacter(targetStage); 
                    }
                );
            }
        }
    }
}
