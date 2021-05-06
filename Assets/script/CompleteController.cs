using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompleteController : MonoBehaviour
{
    [SerializeField]
    Slider sizeSlider;
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    GameObject image;
    [SerializeField]
    GameObject text;

    private List<GameObject> imagePrefabs;

    void Start()
    {
        imagePrefabs = new List<GameObject>();
        string dirPath = Application.persistentDataPath + "/img/";
        string[] files = Directory.GetFiles(dirPath, "*.png");
        if(files.Length > 0)
        {
            foreach (string file in files)
            {
                byte[] bytes = File.ReadAllBytes(file);
                GameObject imagePrefab = Instantiate(image);
                Texture2D loadTexture = new Texture2D(2, 2);
                loadTexture.LoadImage(bytes);
                imagePrefab.GetComponent<Image>().sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), Vector2.zero);
                imagePrefab.transform.SetParent(canvas.transform, false);
                imagePrefab.transform.localScale = new Vector3(sizeSlider.value, sizeSlider.value, 1);
                imagePrefabs.Add(imagePrefab);
            }
        }
        else
        {
            text.SetActive(true);
        }
        



    }

    public void OnChangedSize()
    {
        float characterSize = sizeSlider.value;
        
        foreach(GameObject imagePrefab in imagePrefabs)
        {
            imagePrefab.transform.localScale = new Vector3(characterSize, characterSize, 1);
        }
        
    }

}
