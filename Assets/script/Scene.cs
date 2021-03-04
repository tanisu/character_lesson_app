using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scene : MonoBehaviour
{
    public static int selectStage = -1;
    public static int stageCount = -1;
    public AudioClip buttonSE;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
        
    }
    public void OnClickScene(string scene)
    {
        
        SceneManager.LoadScene(scene);
    }
    public void OnClickCharacter(int index)
    {
        selectStage = index;
        SceneManager.LoadScene("PlayScene");
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(buttonSE);
    }


}
