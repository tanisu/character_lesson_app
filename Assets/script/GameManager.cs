using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{

    //自身を入れるstatci変数
    public static GameManager instance;
    
    //特典
    private int score;
    //ステージ全体のゲームオブジェクトを保存しておくリスト
    private List<GameObject> allCharacters;
    //文字全体の数
    private int characterCount;
    //現在のステージ数
    private int currentStage;
    //現在のステージオブジェクト（文字）
    private GameObject currentCharacterObject;
    //現在のステージオブジェクトにアタッチされたクラス
    private BaseCharacterManager currentCharacterManager;
    

    //画数
    private int strokeMax;
    //現在の画数
    private int strokeCurrent;
    //スタート地点の座標
    private List<float> startPosition;
    //ゴール地点の座標
    private List<float> endPosition;


    //書き順スタートゲームオブジェクト
    [SerializeField]
    private GameObject startMarker;
    //書き順ゴールゲームオブジェクト
    [SerializeField]
    private GameObject goalMarker;

    //完成した文字を保存するためのもの（今後使う予定）
    private List<GameObject> completeCharacters;
    //全文字を保存してる空の親ゲームオブジェクト
    [SerializeField]
    private GameObject CharactersWrapper;


    //シーンマネージャー
    [SerializeField]
    private GameObject SceneManager;
    //メッセージテキスト
    [SerializeField]
    private Text message;
    //はみ出したときに応援するようテキスト
    [SerializeField]
    private Text scoreboard;
    //次の文字ボタン用の親ゲームオブジェクト
    [SerializeField]
    private GameObject nextButtonWrapper;

    //ゲームスタートフラグ　MojiControllerでも参照するのでpublic
    public bool startFlag;
    public GameObject stroke;


    private void Awake()
    {
        instance = this;
        allCharacters = CharactersWrapper.GetComponentsInChildren<Transform>(true).Select(
            (Transform t) => {
                
                if(t.tag == "moji")
                {
                    return t.gameObject;
                }
                return null;
            }).ToList();
        allCharacters.RemoveAll(t => t == null);
        characterCount = allCharacters.Count;
    }

    
    void Start()
    {
        score = 100;
        

        
        if (Scene.selectStage > -1)
        {
            currentStage = Scene.selectStage;
        }
        else
        {
            currentStage = 0;
        }
        currentCharacterObject = allCharacters[currentStage].gameObject;
        currentCharacterManager = currentCharacterObject.GetComponent<BaseCharacterManager>();
        //Debug.Log(currentCharacterObject.GetComponent<BaseCharacterManager>().startEnd_x_y.Count);
        
        //最初の画数初期化
        strokeCurrent = 0;
        //最大画数画数
        strokeMax = currentCharacterManager.startEnd_x_y.Count;
        //最初のスタート位置（x,y）
        startPosition = this._getPositions(currentCharacterManager.startEnd_x_y[strokeCurrent].List,"start");
        //最初のゴール位置(x,y)
        endPosition = this._getPositions(currentCharacterManager.startEnd_x_y[strokeCurrent].List, "end");
        


        //次のボタン用処理
        if (currentStage < (characterCount - 1))
        {
            nextButtonWrapper.SetActive(true);
            string nextCharacter = allCharacters[currentStage + 1].GetComponent<BaseCharacterManager>().displayName;
            nextButtonWrapper.GetComponentInChildren<Button>().onClick.AddListener(()=> SceneManager.GetComponent<Scene>().OnClickCharacter(currentStage + 1));
            

            nextButtonWrapper.transform.GetComponentInChildren<Text>().text = "つぎは「"+nextCharacter+"」だよ";
        }


        currentCharacterObject.SetActive(true);
        this._viewStartAndGoal();
        scoreboard.text = "";
        startFlag = false;
        
    }

    private List<float> _getPositions(List<float> posList ,string posType)
    {
        List<float> pos;
        if(posType == "start")
        {
            pos = posList.Take(2).ToList();
        }else
        {
            pos = posList.Skip(2).Take(2).ToList();
        }
        return pos;
    }

    public void startAct()
    {
        message.text = "スタート";
        startFlag = true;
    }

    public void goalAct()
    {
        
        message.text = "ゴール！";
        startMarker.SetActive(false);
        goalMarker.SetActive(false);
        startFlag = false;
        stroke.GetComponent<StrokeManager>().stopStroke();

        currentCharacterObject.SetActive(false);
        if (score >= 80)
        {
            scoreboard.text = "よくできました";
        }
        else if (score < 79 && score >= 60)
        {
            scoreboard.text = "もうすこし！";
        }
        else
        {
            scoreboard.text = "がんばろうね！";
        }
    }


    private void _viewStartAndGoal()
    {

        startMarker = Instantiate(startMarker,currentCharacterObject.transform);
        goalMarker = Instantiate(goalMarker,currentCharacterObject.transform);

        startMarker.transform.localPosition = new Vector3(startPosition[0], startPosition[1]);
        goalMarker.transform.localPosition = new Vector3(endPosition[0], endPosition[1]);


        startMarker.SetActive(true);
        goalMarker.SetActive(true);
   
    }

    public void nextStroke()
    {
        if(strokeCurrent <(strokeMax-1))
        {
            startFlag = false;

            /*
            Destroy(startMarker);
            Destroy(goalMarker);
            */
            
            startMarker.SetActive(false);
            goalMarker.SetActive(false);
            
            strokeCurrent++;
            //最初のスタート位置（x,y）
            startPosition = this._getPositions(currentCharacterManager.startEnd_x_y[strokeCurrent].List, "start");
            //最初のゴール位置(x,y)
            endPosition = this._getPositions(currentCharacterManager.startEnd_x_y[strokeCurrent].List, "end");
            this._viewStartAndGoal();
        }
        else
        {
            this.goalAct();
        }

    }

    public void overCharacter()
    {
        score -= 1;
        scoreboard.text = "がんばれがんばれ";
        //Debug.Log(score);
    }

    public void enterCharacter()
    {
        scoreboard.text = "そのちょうし！";
    }







}
