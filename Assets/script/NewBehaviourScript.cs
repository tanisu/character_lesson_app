using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

public class NewBehaviourScript : MonoBehaviour
{
    /// <summary>
    /// 線コンポーネントのリスト
    /// </summary>
    private List<LineRenderer> lineRenderers;

    /// <summary>
    /// 描画領域コンポーネント
    /// </summary>
    public GameObject paintArea;

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


  

    void Start()
    {
        lineRenderers = new List<LineRenderer>();
        //paintArea.GetComponent<Painter>().Test();
        lineColor = Color.cyan;

    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            this.AddLineObject();
        }

        if (Input.GetMouseButton(0))
        {
            this.AddPositionDataToLineRendererList();
        }
        //if (Input.GetMouseButtonUp(0))
        //{
            
        //    Debug.Log(lineRenderers.Last().positionCount);
        //}
    }

    /// <summary>
    /// 線オブジェクトの追加メソッド
    /// </summary>
    private void AddLineObject()
    {
        //空のゲームオブジェクト作成
        GameObject lineObject = new GameObject("stroke");
        //空のゲームオブジェクトにLineRendererコンポーネント追加
        lineObject.AddComponent<LineRenderer>();
        lineObject.AddComponent<CircleCollider2D>();
        lineObject.AddComponent<Rigidbody2D>();
        lineObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        lineObject.GetComponent<Rigidbody2D>().simulated = true;

        //LineRendererリストに上記ゲームオブジェクト追加
        lineRenderers.Add(lineObject.GetComponent<LineRenderer>());
        lineObject.transform.parent = transform;
        this.initLastRenderers();
    }

    public void Restart()
    {
        this.AddLineObject();
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
        //マウス座標を取得し、ワールド座標に変換
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1.0f);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(screenPosition);

        

        //線と線をつなぐ点の数を更新
        lineRenderers.Last().positionCount += 1;

        /*
        Debug.Log($"LineRenderer:mousePosition > {mousePosition}");
        Debug.Log($"LineRenderer:screenPosition > {screenPosition}");
        */
        //paintArea.GetComponent<Painter>().LineTo(mousePosition, mousePosition, Color.green);

        //線のコンポーネントリストを更新
        lineRenderers.Last().SetPosition(lineRenderers.Last().positionCount - 1, mousePosition);
        
    }

    

}
