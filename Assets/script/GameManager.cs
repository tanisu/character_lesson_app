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
    //private int characterCount;

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

 

    //スタートマーカー生成用変数
    private GameObject startViewMarker;
    //エンドマーカー生成用変数
    private GameObject goalViewMarker;

    [SerializeField]
    TextAsset stageFile;

    //書き順スタートゲームオブジェクト
    [SerializeField]
    private GameObject startMarker;
    //書き順ゴールゲームオブジェクト
    [SerializeField]
    private GameObject goalMarker;

    
    //全文字を保存してる空の親ゲームオブジェクト
    [SerializeField]
    private GameObject CharactersWrapper;
    //クリア後に出てくるパネル
    [SerializeField]
    private GameObject clearPanel;

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
    private List<Button> nextButtons;
    [SerializeField]
    private GameObject borad;
    [SerializeField]
    private GameObject Stroke;
    [SerializeField]
    private GameObject TestSprite;
    [SerializeField]
    private GameObject CompImage;
    [SerializeField]
    private List<Transform> ColorButtons;
    


    private int canvasH = 600;
    private Color bgColor;
    private List<string> characters;

    //ゲームスタートフラグ　他スクリプトでも参照するのでpublic
    public bool startFlag;
    public bool onBoardFlag;
    public bool enterGoal;
    public bool gameClear;
    public bool onPanelFlag;
    
    public string currentName;
    public Texture2D saveTexture;
    public Sprite testSprite;

    private void Awake()
    {
        
        instance = this;
        characters = new List<string>();
        LoadCharacterData();
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
        
        GameObject tempObj = (GameObject)Resources.Load("Characters/"+currentStage);
        currentCharacterObject = Instantiate(tempObj, CharactersWrapper.transform);
        currentCharacterManager = currentCharacterObject.GetComponent<BaseCharacterManager>();
        currentName = currentCharacterManager.name.Replace("(Clone)","");
         
        //最初の画数初期化
        strokeCurrent = 0;
        //最大画数画数
        strokeMax = currentCharacterManager.startEnd_x_y.Count;
        //最初のスタート位置（x,y）
        startPosition = this._getPositions(currentCharacterManager.startEnd_x_y[strokeCurrent].List,"start");
        //最初のゴール位置(x,y)
        endPosition = this._getPositions(currentCharacterManager.startEnd_x_y[strokeCurrent].List, "end");


        this._viewNextButton();


        //文字のコライダーのトリガーをtrueにする
        currentCharacterObject.GetComponent<PolygonCollider2D>().isTrigger = true;
        startFlag = false;
        onBoardFlag = false;
        enterGoal = false;
        gameClear = false;
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
        string nextCharacterName = "";
        if (currentStage + 1 < characters.Count)
        {
            nextCharacterName = characters[currentStage + 1];
 
        }else
        {
            nextCharacterName = characters[0];
        }
        foreach (Button nextButton in nextButtons)
        {
            nextButton.transform.GetComponentInChildren<Text>().text = "つぎは\n「" + nextCharacterName + "」";
            nextButton.onClick.AddListener(() => SceneManager.GetComponent<Scene>().OnClickCharacter(currentStage + 1));
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
        message.text = "スタート";
        startFlag = true;
        onBoardFlag = true;
        AudioManager.I.StartStroke();
        //点滅処理
        DotweenFade(startRenderer, 0.2f, 0.0f);
        DotweenFade(crayonRenderer, 0f,0.0f);
        DotweenFade(goalRenderer, 1.0f, 0.0f);
        
    }

    public void NextStroke()
    {
        if (strokeCurrent < (strokeMax - 1))
        {
            AudioManager.I.EnterGoal();
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
            AudioManager.I.ClearStage();
            this.GoalAct();
        }
    }



    public void GoalAct()
    {

        //clearPanel.SetActive(true);
        message.gameObject.SetActive(false);
        Destroy(startViewMarker);
        Destroy(goalViewMarker);

        startFlag = false;
        gameClear = true;
        Stroke.GetComponent<StrokeManager>().StopStroke();
        currentCharacterObject.SetActive(false);


        
        string scoreMessage = $"はみでたのは <size=30><color=#bb0000>{overCount}</color></size> かいだよ\n";
        
        if(overCount == 0)
        {
            scoreMessage += "すごーい！てんさい！";
        }else if(overCount < 4)
        {
            scoreMessage += "あとすこしだね！";
        }else if(overCount < 7)
        {
            scoreMessage += "ゆっくりかけばだいじょうぶ！";
        }else if(overCount < 21)
        {
            scoreMessage += "たくさんれんしゅうしよう！";
        }else
        {
            scoreMessage += "ママやパパとかいてみよう！";
        }
        scoreboard.text = scoreMessage;


        //stroke.SetActive(false);
        borad.GetComponent<SpriteRenderer>().enabled = false;

        //文字を保存する処理を追記する
        SaveTempTexture();
    }

    //セーブ用のスプライトを生成するメソッド
    private void SaveTempTexture()
    {
        float scH = Screen.height;
        float mag = scH / canvasH;
        float strokeSize = Stroke.GetComponent<RectTransform>().rect.height * mag;
        
        bgColor = Camera.main.backgroundColor;
        
        StartCoroutine(CaptureCorutine((int)strokeSize, (int)strokeSize));
    }
    //セーブ用のスプライトを生成するメソッド＿コルーチン
    private IEnumerator CaptureCorutine(int width, int height)
    {
        yield return new WaitForEndOfFrame();
        Texture2D bufferTexture = ScreenCapture.CaptureScreenshotAsTexture();

        int x = bufferTexture.width / 2;
        int y = (bufferTexture.height - height) / 2;

        Color[] colors = bufferTexture.GetPixels(x, y, width, height);
        saveTexture = new Texture2D(width, height, TextureFormat.ARGB32, false);


        
        for (int i = 0; i < colors.Length; i++)
        {
            
            if (colors[i] == bgColor)
            {
                
                colors[i].a = 0.0f;
            }
        
        }
        
        saveTexture.SetPixels(colors);
        saveTexture.Apply();
        Sprite sp = Sprite.Create(saveTexture, new Rect(0, 0, saveTexture.width, saveTexture.height), Vector2.zero);
        
        CompImage.GetComponent<Image>().sprite = sp;
        
        Stroke.SetActive(false);
        clearPanel.SetActive(true);
        clearPanel.GetComponent<CanvasGroup>().DOFade(endValue: 1f, duration: 0.2f).SetEase(Ease.OutQuad).SetLink(clearPanel.gameObject).OnComplete(()=> {
            scoreboard.transform.DOLocalMoveY(endValue:75.6f,duration:1f).SetLink(scoreboard.gameObject);
            scoreboard.GetComponent<CanvasGroup>().DOFade(endValue: 1f, duration: 0.5f).SetLink(scoreboard.gameObject);
        });
    }

    public int GetCurrentStage()
    {
        return currentStage;
    }


    public void OverCharacter()
    {
        overCount++;
        message.text = "がんばれがんばれ";
        
    }

    public void EnterCharacter()
    {
        message.text = "そのちょうし！";
    }

    public void OverhangBoard()
    {
        enterGoal = false;
        onBoardFlag = false;
        message.text = "わくのなかにかいてね";
        DotweenFade(startRenderer, 1.0f, 0.0f);
        DotweenFade(crayonRenderer, 1.0f, 0.0f);
        
    }
    
    public void QuitStroke()
    {
        message.text = "ゴールめざしてがんばれ！";
        DotweenFade(startRenderer, 1.0f, 0.0f);
        DotweenFade(crayonRenderer, 1.0f, 0.0f);
    }
    
    public void EnterGoal()
    {
        enterGoal = true;
    }

    public void DotweenFade(SpriteRenderer sprite,float endValue,float duration)
    {
        sprite.material.DOFade(endValue: endValue, duration: duration).SetLink(sprite.gameObject);
    }

    public void DotweenFade(SpriteRenderer sprite, float endValue, float duration,int loop)
    {
        sprite.material.DOFade(endValue: endValue, duration: duration).SetLink(sprite.gameObject).SetLoops(loop, LoopType.Yoyo).OnComplete(()=> {
            DotweenFade(sprite, 1.0f, 0.5f);
        });
    }

    public void DotweenFade(GameObject gObj, float endValue, float duration)
    {
        gObj.GetComponent<SpriteRenderer>().material.DOFade(endValue: endValue, duration: duration).SetLink(gObj).OnComplete(() =>
            {
                Destroy(gObj);
            }
        );
    }

  
    public void ChangeColor(Color c)
    {
        Stroke.GetComponent<StrokeManager>().lineColor = c;
    }

    public void ChangeWidth(float f)
    {
        Stroke.GetComponent<StrokeManager>().lineWidth = f;
        foreach(Transform colorButton in ColorButtons)
        {
            switch (f)
            {
                case 0.12f:
                    
                    colorButton.localScale = new Vector2(0.8f, 0.8f);
                    break;
                case 0.25f:
                    colorButton.localScale = new Vector2(1f, 1f);
                    break;
                case 0.5f:
                    colorButton.localScale = new Vector2(1.2f, 1.2f);
                    break;
            }
        }
    }
    void LoadCharacterData()
    {
        string[] lines = stageFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        
        foreach (string line in lines)
        {
            
            string[] values = line.Split(new[] { ',' });
            foreach (string v in values)
            {
                characters.Add(v);
            }
        }
    }

}
