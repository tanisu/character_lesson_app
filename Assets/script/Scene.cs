using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scene : MonoBehaviour
{
    public static string selectStage;

    public void OnClickScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void OnClickCharacter(string charcter)
    {
        selectStage = charcter;
        SceneManager.LoadScene("PlayScene");
    }

}
