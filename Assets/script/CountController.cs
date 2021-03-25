using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


using System.Linq;

public class CountController : MonoBehaviour
{

    private string[][] characters;
    
    
    public GameObject buttonInstance;
    public GameObject columnPanel;
    [SerializeField]
    private GameObject spacer;
    [SerializeField]
    private GameObject SceneManager;
    [SerializeField]
    TextAsset stageFile;
    [SerializeField]
    ToggleGroup toggleGroup;
    [SerializeField]
    Toggle dakutenToggle;
    [SerializeField]
    Toggle handakutenToggle;
    [SerializeField]
    Toggle komojiToggle;

    public List<GameObject> dakutenButtons;
    public List<GameObject> hanDakutenButtons;
    public List<GameObject> komojiButtons;


    private int buttonCount;

    private List<GameObject> columnPanalList;
    void Start()
    {
        
        LoadCharacterData();
        columnPanalList = new List<GameObject>();
        dakutenButtons = new List<GameObject>();
        hanDakutenButtons = new List<GameObject>();
        komojiButtons = new List<GameObject>();
        
        dakutenToggle.onValueChanged.AddListener(ChangeToggleEvent);
        handakutenToggle.onValueChanged.AddListener(ChangeToggleEvent);
        komojiToggle.onValueChanged.AddListener(ChangeToggleEvent);



        for (int column = 0;column < characters.Length; column++)
        {
            GameObject panel = Instantiate(columnPanel,transform);
            panel.name = column.ToString();
            columnPanalList.Add(panel);
            for (int index = 0;index < characters[column].Length; index++)
            {
                if(characters[column].Length == 3 && index > 0)
                {
                    Instantiate(spacer, columnPanalList[column].transform);
                }               
                GameObject button = Instantiate(buttonInstance,columnPanalList[column].transform);
                if(column > 9)
                {
                    if (column < 14)
                    {
                        int newColumnNumber = column % 9;
                        if (column == 13)
                        {
                            newColumnNumber++;
                        }
                        button.transform.SetParent(columnPanalList[newColumnNumber].transform, false);
                        dakutenButtons.Add(button);
                    }
                    else if (column == 14)
                    {
                        button.transform.SetParent(columnPanalList[5].transform, false);
                        hanDakutenButtons.Add(button);
                    }
                    else if (column == 15)
                    {
                        int newColumnNumber = 3;
                        if (index == 0)
                        {
                            newColumnNumber = 3;
                            button.transform.SetParent(columnPanalList[newColumnNumber].transform);
                            button.transform.SetSiblingIndex(3);
                            komojiButtons.Add(button);
                        }
                        else
                        {
                            newColumnNumber = 7;
                            button.transform.SetParent(columnPanalList[newColumnNumber].transform);
                            GameObject spacerObj =  Instantiate(spacer, columnPanalList[newColumnNumber].transform);
                            komojiButtons.Add(button);
                            komojiButtons.Add(spacerObj);
                        }
                        
                        
                    }
                    
                    

                    button.SetActive(false);
                }
 
                

                buttonCount++;
                Text buttonText = button.transform.GetChild(0).GetComponent<Text>();
                buttonText.text = characters[column][index];
                button.name = buttonCount.ToString();
                int targetStage = buttonCount -1;
                button.GetComponent<Button>().onClick.AddListener(
                    () => {
                        SceneManager.GetComponent<Scene>().OnClickCharacter(targetStage); 
                    }
                );
            }
        }//endfor
        
        foreach (GameObject columnPanel in columnPanalList)
        {

            if (columnPanel.transform.childCount == 0)
            {
                
                Destroy(columnPanel);

            }
        }
        columnPanalList.RemoveRange(10, 6);
        
        
    }
    void ChangeToggleEvent(bool isActive)
    {
        if (toggleGroup.AnyTogglesOn())
        {
            Toggle t = toggleGroup.ActiveToggles().FirstOrDefault();
            string toggleType = t.tag;
            switch (toggleType)
            {
                case "Dakuten":
                    DefalutDisplayOff();
                    HideAndDisplay(dakutenButtons, new List<GameObject>[] { hanDakutenButtons,komojiButtons },new int[] { 1, 2, 3, 5 } );
                    break;
                case "Handakuten":
                    DefalutDisplay();
                    DefalutDisplayOff();
                    HideAndDisplay(hanDakutenButtons, new List<GameObject>[] { dakutenButtons,komojiButtons }, new int[] { 5 });

                    break;
                case "Komoji":
                    DefalutDisplay();
                    DefalutDisplayOff();
                   KomojiHideAndDisplay(komojiButtons, new List<GameObject>[] { hanDakutenButtons, dakutenButtons },new int[] { 3,7});
                    break;
                default:
                    break;
            }
        }
        else
        {
            DefalutDisplay();
        }
        
       // Debug.Log(isActive);
    }
    void DefalutDisplayOff()
    {
        for (int i = 0; i < columnPanalList.Count; i++)
        {
            for (int j = 0; j < columnPanalList[i].transform.childCount; j++)
            {
                if (!columnPanalList[i].transform.GetChild(j).transform.CompareTag("Spacer"))
                {
                    columnPanalList[i].transform.GetChild(j).GetComponent<Button>().interactable = false;
                }
                
            }
        }
    }
    void DefalutDisplay()
    {
        for(int i = 0;i < columnPanalList.Count; i++)
        {
            for(int j = 0;j < columnPanalList[i].transform.childCount; j++)
            {
                columnPanalList[i].transform.GetChild(j).gameObject.SetActive(true);
                if (!columnPanalList[i].transform.GetChild(j).transform.CompareTag("Spacer"))
                {
                    columnPanalList[i].transform.GetChild(j).GetComponent<Button>().interactable = true;
                }
            }
        }
        foreach(GameObject dakutenButton in dakutenButtons)
        {
            dakutenButton.SetActive(false);
        }
        foreach(GameObject handakutenButton in hanDakutenButtons)
        {
            handakutenButton.SetActive(false);
        }
        foreach(GameObject komojiButton in komojiButtons)
        {
            komojiButton.SetActive(false);
        }
    }

    void KomojiHideAndDisplay(List<GameObject> viewButtons, List<GameObject>[] hideButtonGroup, int[] defalutPanels)
    {
        for (int i = 0; i < hideButtonGroup.Length; i++)
        {
            foreach (GameObject hideButton in hideButtonGroup[i])
            {
                hideButton.SetActive(false);
            }
        }

        for (int panelNumber = 0; panelNumber < defalutPanels.Length; panelNumber++)
        {
            int idx = defalutPanels[panelNumber];
            for (int i = 0; i < columnPanalList[idx].transform.childCount; i++)
            {   
                if(panelNumber == 0 && i != 2)
                {
                    continue;
                }
                columnPanalList[idx].transform.GetChild(i).gameObject.SetActive(false);
            }
            

        }

        foreach (GameObject viewButton in viewButtons)
        {
            if (!viewButton.transform.CompareTag("Spacer"))
            {
                viewButton.GetComponent<Button>().interactable = true;
            }
            
            viewButton.SetActive(true);
        }
    }


    void HideAndDisplay(List<GameObject> viewButtons,List<GameObject>[] hideButtonGroup,int[] defalutPanels)
    {
        for(int i = 0; i < hideButtonGroup.Length;i++)
        {
            foreach(GameObject hideButton in hideButtonGroup[i])
            {
                hideButton.SetActive(false);
            }
        }

        for(int panelNumber = 0;panelNumber < defalutPanels.Length; panelNumber++)
        {
            int idx = defalutPanels[panelNumber];
            for (int i = 0; i < columnPanalList[idx].transform.childCount; i++)
            {
                columnPanalList[idx].transform.GetChild(i).gameObject.SetActive(false);
            }
        }

        foreach (GameObject viewButton in viewButtons)
        {
            viewButton.GetComponent<Button>().interactable = true;
            viewButton.SetActive(true);
        }

    }

    void LoadCharacterData()
    {
        string[] lines = stageFile.text.Split(new[] { '\n','\r'},System.StringSplitOptions.RemoveEmptyEntries);
        characters = new string[lines.Length][];
        for (int i = 0;i < lines.Length; i++)
        {
            string[] values = lines[i].Split(new[] { ',' });
            characters[i] = values;
        }
    }
}
