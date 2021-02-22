using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MojiTrigger : MonoBehaviour
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
        message.text = "ÇÕÇ›Ç≈ÇΩÇÊ";
        Debug.Log("enter");
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        //this.aleart.GetComponent<Renderer>().material.color = Color.green;
        //message.text = "ÇªÇÃÇøÇÂÇ§ÇµÅI";
        //Debug.Log("stay");
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        this.aleart.GetComponent<Renderer>().material.color = Color.red;
        message.text = "ÇÕÇ›Ç≈ÇΩÇÊ";
        Debug.Log("exit");
    }
}
