using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class GameManager : MonoBehaviour
{

    // static
    public static GameManager instance;

    public int strokeOrder;
    public int score;
    private GameObject startStroke;
    private GameObject goalStroke;
    private GameObject parentObject;
    private GameObject moji;
    //private GameObject complateChar;
    private List<GameObject> completeCharacters;
    private Canvas canvas;
    private Text message;
    private Text scoreboard;
    private string startName;
    private string goalName;
    private string[] characters;
    private int stage;
    public bool startFlag;
    public GameObject character;

    private void Awake()
    {
        instance = this;
        characters = new string[] { "a", "i","u" };
        stage = 0;
        canvas = GameObject.FindWithTag("characterCanvas").GetComponent<Canvas>();
    }

    // Start is called before the first frame update
    void Start()
    {
        score = 100;
        strokeOrder = 1;
        parentObject = canvas.transform.Find(characters[stage]).gameObject;
        
        parentObject.SetActive(true);

        this.viewStartAndGoal();
        startFlag = false;
        
        
        message = GameObject.FindWithTag("Message").GetComponent<Text>();
        moji = GameObject.FindWithTag("moji");
        scoreboard = GameObject.Find("score").GetComponent<Text>();
    }

    public void startAct()
    {
        message.text = "スタート！";
        startFlag = true;
    }

    public void goalAct()
    {
        
        message.text = "ゴール！";
        startStroke.SetActive(false);
        goalStroke.SetActive(false);
        moji.GetComponent<SpriteRenderer>().enabled = false;
        startFlag = false;


        if (score >= 80)
        {
            scoreboard.text = "よくできました";
        }
        else if (score < 79 && score >= 60)
        {
            scoreboard.text = "あとすこし！";
        }
        else
        {
            scoreboard.text = "がんばろう！";
        }
        if(stage + 1 < characters.Length)
        {

            /*
            GameObject copied = Object.Instantiate(character) as GameObject;
            Destroy(copied.GetComponent<NewBehaviourScript>());
            Vector3 scale = copied.transform.localScale;
            scale = new Vector3(scale.x / 2, scale.y / 2, scale.z);
            copied.transform.localScale = scale;
            */
            parentObject.SetActive(false);


            foreach (Transform n in character.transform)
            {   
                Destroy(n.gameObject);
            }
            
            
            stage++;
            character.GetComponent<NewBehaviourScript>().Restart();
            this.Start();
        }
    }

    private void viewStartAndGoal()
    {
        startName = "start_" + strokeOrder;
        goalName = "goal_" + strokeOrder;
        

        startStroke = parentObject.transform.Find(startName).gameObject;
        goalStroke = parentObject.transform.Find(goalName).gameObject;
        startStroke.SetActive(true);
        goalStroke.SetActive(true);
    }

    public void nextStroke()
    {
        startFlag = false;
        startStroke.SetActive(false);
        goalStroke.SetActive(false);
        strokeOrder++;
        this.viewStartAndGoal();
    }

    public void overCharacter()
    {
        score -= 1;
        //Debug.Log(score);
    }







}
