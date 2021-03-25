using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scene : MonoBehaviour
{
    public static int selectStage = -1;
    public static int stageCount = -1;

    private void Awake()
    {
        AudioManager.GetI();
    }


    public void OnClickScene(string scene)
    {
        
        SceneManager.LoadScene(scene);
        AudioManager.I.ClickSoft();
    }
    public void OnClickCharacter(int index)
    {
        selectStage = index;
        
        SceneManager.LoadScene("PlayScene");
        AudioManager.I.ClickSoft();
    }

    


}
