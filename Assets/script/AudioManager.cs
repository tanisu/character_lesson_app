using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
    AudioClip buttonClip;
    AudioClip goalCilp;
    AudioClip clickSoft;
    AudioClip clearStage;
    AudioClip startStroke;
    AudioClip create;
    AudioClip delete;
    private AudioSource audioSource;

    public static AudioManager I { get; private set; }


    public static AudioManager GetI()
    {
        if (I == null)
        {
            GameObject obj = new GameObject("AudioManager");
            I = obj.AddComponent<AudioManager>();
            I.gameObject.AddComponent<AudioSource>();
            DontDestroyOnLoad(obj);
        }
        return I;
        
    }


    private void Awake()
    {

        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        buttonClip = Resources.Load<AudioClip>("Sound/click");
        goalCilp = Resources.Load<AudioClip>("Sound/goal");
        clickSoft = Resources.Load<AudioClip>("Sound/clickSoft");
        clearStage = Resources.Load<AudioClip>("Sound/clear");
        startStroke = Resources.Load<AudioClip>("Sound/start");
        create = Resources.Load<AudioClip>("Sound/create");
        delete = Resources.Load<AudioClip>("Sound/delete");


    }


    private void OnDestroy()
    {
        if(I == this)
        {
            I = null;
        }
    }

    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        
        
    }

    public void ButtonClick()
    {
        audioSource.PlayOneShot(buttonClip);
    }
    public void StartStroke()
    {
        audioSource.PlayOneShot(startStroke);
    }

    public void EnterGoal()
    {
        audioSource.PlayOneShot(goalCilp);
    }
    
    public void ClickSoft()
    {
        audioSource.PlayOneShot(clickSoft);
    }

    public void ClearStage()
    {
        audioSource.PlayOneShot(clearStage);
    }

    public void CreateImage()
    {
        audioSource.PlayOneShot(create);
    }

    public void DeleteImage()
    {
        audioSource.PlayOneShot(delete);
    }
}
