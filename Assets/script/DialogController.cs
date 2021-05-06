using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public void SetUp(string text, string leftButtonText, string rightButtonText, UnityEngine.Events.UnityAction leftButtonAction, UnityEngine.Events.UnityAction rightButtonAction)
    {
        // メインのテキストにtextを入れる
        transform.GetChild(0).GetComponent<Text>().text = text;

        // 左ボタンのテキストにleftButtonTextを入れる
        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = leftButtonText;

        // 右ボタンのテキストにrightButtonTextを入れる
        transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = rightButtonText;

        // 左ボタンのOnClickにメソッドを入れる
        transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(retrunTrue);

        // 右ボタンのOnClickにメソッドを入れる
        transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(rightButtonAction);
    }

    private void Start()
    {
        transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(retrunTrue);
    }

    private void retrunTrue()
    {
        Debug.Log("true");
        //return true;
    }
}
