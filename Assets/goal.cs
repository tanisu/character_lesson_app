using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class goal : MonoBehaviour
{
    GameObject aleart;
    Text message;
    // Start is called before the first frame update

    private void Start()
    {
        aleart = GameObject.FindWithTag("aleart");
        message = GameObject.FindWithTag("Message").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        this.aleart.GetComponent<Renderer>().material.color = Color.red;
        message.text = "ÉSÅ[ÉãÅI";
        Debug.Log("enter");
    }
}
