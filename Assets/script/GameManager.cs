using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

    //自身を入れるstatci変数
    public static GameManager instance;
    
    //はみ出た回数
    private int overCount;
    
    //文字全体の数
    private int characterCount;
    //現在のステージ数
    private int currentStage;
    //現在のステージオブジェクト（文字）
    private GameObject currentCharacterObject;
    //現在のステージオブジェクトにアタッチされたクラス
    private BaseCharacterManager currentCharacterManager;
    //スタートマーカー用レンダラー変数
    private SpriteRenderer startRenderer;
    //ゴールマーカー用レンダラー変数
    private SpriteRenderer goalRenderer;
    //クレヨン用レンダラー変数
    private SpriteRenderer crayonRenderer;

    //画数
    private int strokeMax;
    //現在の画数
    private int strokeCurrent;
    //スタート地点の座標
    private List<float> startPosition;
    //ゴール地点の座標
    private List<float> endPosition;

    ////
    //private List<List<float>> markerPositions;
    ////
    //private List<GameObject> startViewMarkers;

    //スタートマーカー生成用変数
    private GameObject startViewMarker;
    //エンドマーカー生成用変数
    private GameObject goalViewMarker;

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
    //ステージ全体のゲームオブジェクトを保存しておくリスト
    [SerializeField]
    private List<GameObject> allCharacters;


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
    private Button nextButton;

    //ゲームスタートフラグ　他スクリプトでも参照するのでpublic
    public bool startFlag;
    public bool onBoardFlag;
    public bool enterGoal;
    public bool onPanelFlag;
    public GameObject stroke;


    private void Awake()
    {
        instance = this;
        characterCount = allCharacters.Count;
        Input.multiTouchEnabled = false;
    }

    
    void Start()
    {
        overCount = 0;        
        if (Scene.selectStage > -1)
        {
            currentStage = Scene.selectStage;
        }
        else
        {
            currentStage = 0;
        }


        currentCharacterObject = Instantiate(allCharacters[currentStage].gameObject, CharactersWrapper.transform);
        currentCharacterManager = currentCharacterObject.GetComponent<BaseCharacterManager>();
        
        //最初の画数初期化
        strokeCurrent = 0;
        //最大画数画数
        strokeMax = currentCharacterManager.startEnd_x_y.Count;
        //最初のスタート位置（x,y）
        startPosition = this._getPositions(currentCharacterManager.startEnd_x_y[strokeCurrent].List,"start");
        //最初のゴール位置(x,y)
        endPosition = this._getPositions(currentCharacterManager.startEnd_x_y[strokeCurrent].List, "end");


        //markerPositions = new List<List<float>>();
        //startViewMarkers = new List<GameObject>();
        ////Debug.Log(currentCharacterManager.startEnd_x_y.Count);
        ////foreach(ValueList l in currentCharacterManager.startEnd_x_y)
        ////{
        ////    markerPositions.Add(l.List);
        ////    Debug.Log(markerPositions[0][1]);
        ////}
        //for(int i = 0;i < currentCharacterManager.startEnd_x_y.Count; i++)
        //{
        //    for(int j = 0;j < currentCharacterManager.startEnd_x_y[i].List.Count; j++)
        //    {

        //        //Debug.Log(currentCharacterManager.startEnd_x_y[i].List.Count);
        //       markerPositions.Add(currentCharacterManager.startEnd_x_y[i].List);
        //       //Debug.Log(markerPositions[j][0]);
        //    }

        //    //startViewMarkers[i] = Instantiate(startMarker, new Vector3(markerPositions[i][0], markerPositions[i][1]));
        //}

        //次のボタン用処理
        //nextButtonWrapper.SetActive(true);


        this._viewNextButton();
        

        //文字のコライダーのトリガーをtrueにする
        currentCharacterObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        startFlag = false;
        onBoardFlag = false;
        enterGoal = false;
        onPanelFlag = false;
        this._viewStartAndGoal();
        scoreboard.text = "";
    }

   

    private void _viewStartAndGoal()
    {
        //スタートとゴールのインスタンス生成
        startViewMarker = Instantiate(startMarker, currentCharacterObject.transform);
        goalViewMarker = Instantiate(goalMarker, currentCharacterObject.transform);
        //スタートとゴールの位置設定
        startViewMarker.transform.localPosition = new Vector3(startPosition[0], startPosition[1]);
        goalViewMarker.transform.localPosition = new Vector3(endPosition[0], endPosition[1]);
        //スタート・ゴール・クレヨンのスプライトを取得
        startRenderer = startViewMarker.GetComponent<SpriteRenderer>();
        goalRenderer = goalViewMarker.GetComponent<SpriteRenderer>();
        crayonRenderer = startViewMarker.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        //点滅処理
        DotweenFade(goalRenderer, 0.2f, 0f); 
    }

    private void _viewNextButton()
    {
        if(currentStage + 1 <characterCount)
        {
            
        
        GameObject nextCharacter = Instantiate(allCharacters[currentStage + 1]);
        string nextCharacterName = nextCharacter.GetComponent<BaseCharacterManager>().displayName;
        Destroy(nextCharacter);
        //nextCharacter.SetActive(false);
        nextButton.transform.GetComponentInChildren<Text>().text = "つぎは\n「" + nextCharacterName + "」";
        nextButton.onClick.AddListener(() => SceneManager.GetComponent<Scene>().OnClickCharacter(currentStage + 1));
            //nextButton.onClick.AddListener(() =>
            //{
            //    Debug.Log("next");
            //    Destroy(currentCharacterObject);
            //    Destroy(nextCharacter);
            //    this._restart(0);
            //    //Destroy(currentCharacterObject);
            //    //Scene.selectStage = currentStage + 1;
            //    //this.Start();
            //});
        }
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

    public void StartAct()
    {
        Debug.Log("start");
        message.text = "スタート";
        startFlag = true;
        onBoardFlag = true;

        //点滅処理
        DotweenFade(startRenderer, 0.2f, 0.0f);
        DotweenFade(crayonRenderer, 0f,0.0f);
        DotweenFade(goalRenderer, 1.0f, 0.0f);
        
    }

    public void NextStroke()
    {
        if (strokeCurrent < (strokeMax - 1))
        {
            startFlag = false;
            enterGoal = false;
            //マーカーを破壊

            Destroy(startViewMarker);
            Destroy(goalViewMarker);

            strokeCurrent++;
            //最初のスタート位置（x,y）
            startPosition = this._getPositions(currentCharacterManager.startEnd_x_y[strokeCurrent].List, "start");
            //最初のゴール位置(x,y)
            endPosition = this._getPositions(currentCharacterManager.startEnd_x_y[strokeCurrent].List, "end");
            this._viewStartAndGoal();
        }
        else
        {
            this.GoalAct();
        }
    }

    public void GoalAct()
    {
        
        message.text = "ゴール！";
        Destroy(startViewMarker);
        Destroy(goalViewMarker);

        startFlag = false;
        
        stroke.GetComponent<StrokeManager>().StopStroke();
        DotweenFade(currentCharacterObject,0f,0.0f);
        string scoreMessage = $"はみでたのは {overCount} かいだよ\n";
        
        if(overCount == 0)
        {
            scoreMessage += "「すごーい！てんさい！」";
        }else if(overCount < 4)
        {
            scoreMessage += "「あとすこしでかんぺきだね！」";
        }else if(overCount < 7)
        {
            scoreMessage += "ゆっくりやればだいじょうぶ！";
        }else if(overCount < 21)
        {
            scoreMessage += "なんどもれんしゅうしてみよう！";
        }else
        {
            scoreMessage += "ママやパパとやってみよう！";
        }
        scoreboard.text = scoreMessage;
    }






    public void OverCharacter()
    {
        overCount++;
        scoreboard.text = "がんばれがんばれ";
        
    }

    public void EnterCharacter()
    {
        scoreboard.text = "そのちょうし！";
    }

    public void OverhangBoard()
    {
        onBoardFlag = false;
        scoreboard.text = "わくのなかにかいてね";
        DotweenFade(startRenderer, 1.0f, 0.0f);
        DotweenFade(crayonRenderer, 1.0f, 0.0f);
        //DotweenFade(goalRenderer, 0.0f, 0.0f);
    }

    public void QuitStroke()
    {
        scoreboard.text = "ゴールめざしてがんばれ！";
        DotweenFade(startRenderer, 1.0f, 0.0f);
        DotweenFade(crayonRenderer, 1.0f, 0.0f);
        //DotweenFade(goalRenderer, 0.0f, 0.0f);
    }
    public void EnterGoal()
    {
        enterGoal = true;
        //DotweenFade(goalRenderer, 0.5f, 0.2f, 3);

    }

    public void DotweenFade(SpriteRenderer sprite,float endValue,float duration)
    {
        sprite.material.DOFade(endValue: endValue, duration: duration);
    }

    public void DotweenFade(SpriteRenderer sprite, float endValue, float duration,int loop)
    {
        sprite.material.DOFade(endValue: endValue, duration: duration).SetLoops(loop, LoopType.Yoyo).OnComplete(()=> {
            DotweenFade(sprite, 1.0f, 0.5f);
        });
    }

    public void DotweenFade(GameObject gObj, float endValue, float duration)
    {
        gObj.GetComponent<SpriteRenderer>().material.DOFade(endValue: endValue, duration: duration).OnComplete(() =>
            {
                Destroy(gObj);
            }
        );
    }

    //不要メソッドになるかも
    void _restart(int stage = 0)
    {
        overCount = 0;
        currentStage = 0;
        currentCharacterObject = Instantiate(allCharacters[currentStage].gameObject, CharactersWrapper.transform);
        currentCharacterManager = currentCharacterObject.GetComponent<BaseCharacterManager>();
        //最初の画数初期化
        strokeCurrent = 0;
        //最大画数画数
        strokeMax = currentCharacterManager.startEnd_x_y.Count;
        //最初のスタート位置（x,y）
        startPosition = this._getPositions(currentCharacterManager.startEnd_x_y[strokeCurrent].List, "start");
        //最初のゴール位置(x,y)
        endPosition = this._getPositions(currentCharacterManager.startEnd_x_y[strokeCurrent].List, "end");

        //次のボタン用処理
        //nextButtonWrapper.SetActive(true);


        this._viewNextButton();


        //文字のコライダーのトリガーをtrueにする
        currentCharacterObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        startFlag = false;
        onBoardFlag = false;
        enterGoal = false;
        onPanelFlag = false;
        this._viewStartAndGoal();
        scoreboard.text = "";
    }




}
