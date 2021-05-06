using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;


public class ScrollPanelController : MonoBehaviour
{

    [SerializeField]
    GameObject  C_Panel;
    [SerializeField]
    TextAsset stageFile;
    [SerializeField]
    GameObject EmptyButton;
    [SerializeField]
    Text NotYetText;
    [SerializeField]
    GameObject SceneManager;


    private List<string> characters;
    void Start()
    {
        characters = new List<string>();
        LoadCharacterData();
        InitPanel();

    }

    void InitPanel()
    {

        foreach ((string c,int i) in characters.Select((c, i) => (c, i)))
        {
            GameObject p = Instantiate(C_Panel);
            p.transform.SetParent(transform, false);
            p.transform.GetChild(0).GetComponent<Button>().transform.GetChild(0).GetComponent<Text>().text = c;

            List<string> files = new List<string>();
            string dirPath = Application.persistentDataPath + "/img/" + i;
            if (Directory.Exists(dirPath))
            {
                files = Directory.GetFiles(dirPath, "*.png").ToList();
            }
            
                
            if(files.Count > 0)
            {
                foreach(string file in files)
                {
                    GameObject emptyButton = Instantiate(EmptyButton);
                    emptyButton.GetComponent<EmptyButtonController>().myPath = file;


                    byte[] bytes = File.ReadAllBytes(file);
                    emptyButton.GetComponent<EmptyButtonController>().bytes = bytes;

                    Texture2D loadTexture = new Texture2D(2, 2);
                    loadTexture.LoadImage(bytes);
                    emptyButton.transform.GetChild(0).GetComponent<Image>().sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), Vector2.zero);
                    emptyButton.transform.SetParent(p.transform, false);
                }
            }
            else
            {
                Text notYettext = Instantiate(NotYetText);
                notYettext.transform.SetParent(p.transform, false);
            }

            p.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(
                () => {
                    SceneManager.GetComponent<Scene>().OnClickCharacter(i);
                }
            );

        }
    }

    void LoadCharacterData()
    {
        string[] lines = stageFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        
        foreach(string line in lines)
        {
            string[] values = line.Split(new[] { ',' });
            foreach(string v in values)
            {
                characters.Add(v);
            }
        }
    }
}
