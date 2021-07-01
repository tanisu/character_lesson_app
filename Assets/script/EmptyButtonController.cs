using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EmptyButtonController : MonoBehaviour
{
    [SerializeField]
    Image DrawImage;

    public string myPath;
    
    public byte[] bytes;
    
    private void Start()
    {
        //書いたひらがなをボードに並べる
        GetComponent<Button>().onClick.AddListener(SetImage);
    }

    private void SetImage()
    {
        //ボードに並べられるかどうかチェック
        if (DrawPanelController.i.CheckMax())
        {
            DrawPanelController.i.AddCount();
            AudioManager.I.CreateImage();
            Texture2D loadTexture = new Texture2D(2, 2);
            loadTexture.LoadImage(bytes);
            Image drawImage = Instantiate(DrawImage);
            drawImage.sprite = Sprite.Create(loadTexture, new Rect(0, 0, loadTexture.width, loadTexture.height), Vector2.zero);
            drawImage.transform.SetParent(DrawPanelController.i.transform, false);
        }

    }
}
