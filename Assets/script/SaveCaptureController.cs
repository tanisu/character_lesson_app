using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using DG.Tweening;


public class SaveCaptureController : MonoBehaviour
{
    //[SerializeField]
    //private GameObject Stroke;
    [SerializeField]
    private GameObject ConfirmPanel;
    [SerializeField]
    private GameObject SuccessPanel;
    [SerializeField]
    private GameObject ErrorPanel;

    private int currentStage;
    private string directoryPath;
    private string folderName;
    private List<string> files;

    private const int FILE_LIMIT_NUM = 3;

    void Start()
    {

        currentStage = GameManager.instance.GetCurrentStage();
        folderName = "img";
        directoryPath = Application.persistentDataPath + "/" + folderName + "/" + currentStage + "/";
        //directoryPath = Application.persistentDataPath + "/" + folderName + "/";
        
        
        GetComponent<Button>().onClick.AddListener(()=>
        {
            string path = GetSavePath();
            files = GetAllFiles();
            if (files.Count >= FILE_LIMIT_NUM)
            {
                ConfirmPanel.SetActive(true);
                
                ConfirmPanel.transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => {
                    File.Delete(files[0]);
                    if (SavePng(path))
                    {
                        StartCoroutine("ViewSuccess");
                    }
                    
                    ConfirmPanel.SetActive(false);
                } );
                ConfirmPanel.transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => {
                    ConfirmPanel.SetActive(false);
                });

            }
            else
            {
                if (SavePng(path))
                {
                    StartCoroutine("ViewSuccess");
                }
            }
        });
    }

    private IEnumerator ViewSuccess()
    {
        SuccessPanel.SetActive(true);
        yield return new WaitForSeconds(1.2f);
        SuccessPanel.SetActive(false);
    }

    private bool SavePng(string path)
    {
        try
        {
            byte[] bytes = GameManager.instance.saveTexture.EncodeToPNG();
            File.WriteAllBytes(path, bytes);
            return true;
        }
        catch(Exception e)
        {
            Debug.Log(e.Message);
            return false;
        }
        
    }

    private List<string> GetAllFiles()
    {
        List<string> imageFilePathList = Directory.GetFiles(directoryPath, "*.png", SearchOption.AllDirectories)
            .Where(filePath => Path.GetFileName(filePath) != ".DS_Store")
            .OrderBy(filePath => File.GetLastWriteTime(filePath).Date)
            .ThenBy(filePath => File.GetLastWriteTime(filePath).TimeOfDay)
            .ToList();

        return imageFilePathList;
    }

    
  
    private string GetSavePath()
    {


        DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        long t = (long)(DateTime.Now - UnixEpoch).TotalSeconds;

        if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                return directoryPath + t + ".png";
            }
            return directoryPath + t + ".png";
       
        
    }



    
    
}
