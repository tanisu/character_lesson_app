using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StrokeManager : MonoBehaviour
{
    /// <summary>
    /// 線コンポーネントのリスト
    /// </summary>
    private List<LineRenderer> lineRenderers;

    /// <summary>
    /// 線のマテリアル
    /// </summary>
    public Material lineMaterial;

    /// <summary>
    /// 線の色
    /// </summary>
    public Color lineColor;

    /// <summary>
    /// 線の太さ
    /// </summary>
    [Range(0, 10)] public float lineWidth;

    private int strokeCount;
  

    void Start()
    {
        lineRenderers = new List<LineRenderer>();
        strokeCount = 0;
    }

    void Update()
    {
        //ゲームスタート時
        if (GameManager.instance.startFlag) {
            //スタートマーカークリック時
            if (Input.GetMouseButtonDown(0))
            {
                this.AddLineObject();
            }
            //ストローク中
            if (Input.GetMouseButton(0))
            {
                //ボード内であれば描画
                if (GameManager.instance.onBoardFlag)
                {
                    this.AddPositionDataToLineRendererList();
                }
            }
            //ボード外でクリックをやめたら一つ前のストロークを消し、ボードフラグをtrueにする
            if (Input.GetMouseButtonUp(0) && !GameManager.instance.onBoardFlag && !GameManager.instance.onPanelFlag)
            {
                this.DestoryStroke();
                GameManager.instance.onBoardFlag = true;
                GameManager.instance.startFlag = false;
            }

            //ゴールしていないのにクリックをやめたら一つ前のストロークを消す
            if (Input.GetMouseButtonUp(0) && !GameManager.instance.enterGoal && GameManager.instance.onBoardFlag)
            {
                this.DestoryStroke();
                GameManager.instance.startFlag = false;
            }
            //ゴールしてマウスを上げたらゴールエンターフラグをfalseにする
            //else if (Input.GetMouseButtonUp(0) && GameManager.instance.enterGoal)
            //{
            //    GameManager.instance.enterGoal = false;
            //}
        }







    }

    /// <summary>
    /// 線オブジェクトの追加メソッド
    /// </summary>
    private void AddLineObject()
    {
        
        //空のゲームオブジェクト作成
        GameObject lineObject = new GameObject("stroke");
        lineObject.tag = "stroke";
        //空のゲームオブジェクトにLineRendererコンポーネント追加
        lineObject.AddComponent<LineRenderer>();
            
        lineObject.AddComponent<CircleCollider2D>();
        lineObject.GetComponent<CircleCollider2D>().radius = 0.2f;
        lineObject.GetComponent<CircleCollider2D>().isTrigger = true;
        lineObject.AddComponent<Rigidbody2D>();
        lineObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        lineObject.GetComponent<Rigidbody2D>().simulated = true;
        lineObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        //LineRendererリストに上記ゲームオブジェクト追加
        lineRenderers.Add(lineObject.GetComponent<LineRenderer>());
        lineObject.transform.parent = transform;
            
        this.initLastRenderers();
        strokeCount++;
        //Debug.Log(strokeCount);
        
    }

    public void Restart()
    {
        this.AddLineObject();
    }

    public void StopStroke()
    {
        Destroy(this);
    }
 
    public void DestoryStroke()
    {
        Destroy(lineRenderers[lineRenderers.Count - 1].gameObject);
    }



    /// <summary>
    /// 線コンポーネント初期化
    /// </summary>

    private void initLastRenderers()
    {
        //lineRenderers.Last().GetComponent<RectTransform>().SetAsLastSibling();
        //線をつなぐ点を０に初期化
        lineRenderers.Last().positionCount = 0;
        //マテリアルを初期化
        lineRenderers.Last().material = this.lineMaterial;
        //色初期化
        lineRenderers.Last().material.color = this.lineColor;
        //線の太さ初期化
        lineRenderers.Last().startWidth = this.lineWidth;
        lineRenderers.Last().endWidth = this.lineWidth;
    }


    /// <summary>
    /// 線コンポーネントに位置情報登録メソッド
    /// </summary>
    private void AddPositionDataToLineRendererList()
    {
        try
        {
            //マウス座標を取得し、ワールド座標に変換
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1.0f);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);



            //線と線をつなぐ点の数を更新
            lineRenderers.Last().positionCount += 1;



            //線のコンポーネントリストを更新
            lineRenderers.Last().SetPosition(lineRenderers.Last().positionCount - 1, worldPosition);


            Vector3 localPos = transform.InverseTransformPoint(worldPosition.x, worldPosition.y, -1.0f);
            //Debug.Log($"before:{localPos}");
            lineRenderers.Last().transform.localPosition = localPos;

            //Debug.Log($"local:{lineRenderers.Last().transform.localPosition}");
        }catch(MissingReferenceException e)
        {
            Debug.LogError(e.Message);
        }



    }

    




}
