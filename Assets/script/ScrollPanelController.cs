using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Profiling;
using System.Linq;
using System.IO;
using DG.Tweening;

public class ScrollPanelController : MonoBehaviour
{

    [SerializeField]
    GameObject C_Panel;

    [SerializeField]
    GameObject[] Dummy_C_Panel;
    [SerializeField]
    TextAsset stageFile;
    [SerializeField]
    GameObject EmptyButton;
    
    [SerializeField]
    Text NotYetText;
    [SerializeField]
    GameObject SceneManager;
    [SerializeField]
    GameObject LoadingPanel;
    [SerializeField]
    Image FillProgress;
    [SerializeField]
    CanvasGroup ScrollView;
    [SerializeField]
    CanvasGroup LineUpPanel;

    private int childCount;

    private List<string> characters;
    //List<Texture2D> loadTextures;
    string[][] filePaths;
    Texture2D loadTexture;
    int charactersLength;
    CanvasGroup cg;

    void Start()
    {
        characters = new List<string>();
        filePaths = new string[75][];
        cg = LoadingPanel.GetComponent<CanvasGroup>();
        
        //loadTextures = new List<Texture2D>();
        childCount = Dummy_C_Panel[0].transform.childCount;
        charactersLength =  LoadCharacterData();
        
        InitPanel();
        StartCoroutine("LoadImages");
        
    }

    private IEnumerator LoadImages()
    {
        for(int i = 0;i < charactersLength; i++)
        {
            /*èëÇ¢ÇΩâÊëúÇ™Ç†ÇÈèÍçá*/
            if(filePaths[i].Length != 0)
            {
                int loopCount = 0;
                foreach((string file, int index) in filePaths[i].Select((file, index) => (file, index)))
                {
                    Transform dt = Dummy_C_Panel[i].transform;
                    EmptyButtonController emptyButtonController = dt.GetChild(index + 1).GetComponent<EmptyButtonController>();
                    emptyButtonController.myPath = file;
                    byte[] bytes = File.ReadAllBytes(file);
                    emptyButtonController.bytes = bytes;
                    loadTexture = new Texture2D(1, 1);
                    loadTexture.LoadImage(bytes);
                    Image textureImage = dt.GetChild(index + 1).transform.GetChild(0).GetComponent<Image>();
                    textureImage.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), Vector2.zero);

                    Color tc = textureImage.color;
                    textureImage.color = new Color(tc.r, tc.g, tc.b, 1f);

                    loopCount++;

                }
                /*èëÇ¢ÇΩâÊëúÇ™3Ç¬ñ¢ñûÇÃèÍçá*/
                if (loopCount < 3)
                {
                    int emptyCount = 3 - loopCount;

                    for (int empty = 0; empty < emptyCount; empty++)
                    {
                        Dummy_C_Panel[i].transform.GetChild(childCount - emptyCount + empty - 1).gameObject.SetActive(false);
                    }
                }
            }
            /*èëÇ¢ÇΩâÊëúÇ™ì‡èÍçá*/
            else
            {
                for (int empty = 1; empty < childCount; empty++)
                {
                    if (empty < 4)
                    {
                        Dummy_C_Panel[i].transform.GetChild(empty).gameObject.SetActive(false);
                    }
                    else
                    {
                        Dummy_C_Panel[i].transform.GetChild(empty).gameObject.SetActive(true);
                    }

                }
            }
            float progress = i / (charactersLength - 1.0f);
            FillProgress.DOFillAmount(progress, 0.1f).SetEase(Ease.InSine).SetLink(FillProgress.gameObject);
            if (i == charactersLength -1)
            {

                cg.DOFade(0.0f,1.1f).SetLink(LoadingPanel.gameObject).OnComplete(()=> {
                    LoadingPanel.SetActive(false);
                });
                LineUpPanel.DOFade(1f, 1.1f).SetEase(Ease.InSine).SetLink(LineUpPanel.gameObject);
                ScrollView.DOFade(1f, 1.1f).SetLink(ScrollView.gameObject);
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void InitPanel()
    {


         foreach ((string c, int i) in characters.Select((c, i) => (c, i)))
          {
            
            Dummy_C_Panel[i].transform.GetChild(0).GetComponent<Button>().transform.GetChild(0).GetComponent<Text>().text = c;
            string dirPath = Application.persistentDataPath + "/img/" + i;
            
            if (Directory.Exists(dirPath))
            {
                filePaths[i] = Directory.GetFiles(dirPath, "*.png");
            }
            else
            {
                filePaths[i] = new string[0];
            }
            //èëÇ≠ÉVÅ[ÉìÇ…ëJà⁄
            Dummy_C_Panel[i].transform.GetChild(0).GetComponent<Button>().onClick.AddListener(
                () =>
                {
                    DrawPanelController.i.AddCount();
                    SceneManager.GetComponent<Scene>().OnClickCharacter(i);
                }
            );

            
        }
    }

    int LoadCharacterData()
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
        return characters.Count;
    }

    private void OnDestroy()
    {
        Resources.UnloadUnusedAssets();
    }

}
