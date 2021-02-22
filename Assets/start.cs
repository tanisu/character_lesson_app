using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class start : MonoBehaviour
{
    GameObject aleart;
    Text message;


    private void Start()
    {
        aleart = GameObject.FindWithTag("aleart");
        message = GameObject.FindWithTag("Message").GetComponent<Text>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.aleart.GetComponent<Renderer>().material.color = Color.red;
        message.text = "スタート！";
        Debug.Log("enter");
    }
}
