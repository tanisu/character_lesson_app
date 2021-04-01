using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveCaptureController : MonoBehaviour
{
    [SerializeField]
    GameObject character;

    private Vector2 vector2;
    void Start()
    {
        vector2 = RectTransformUtility.WorldToScreenPoint(Camera.main,character.transform.position);
        //Debug.Log(vector2.x);
        GetComponent<Button>().onClick.AddListener(()=>
        {
            StartCoroutine(CaptureCoroutine((int)vector2.x / 2,(int)vector2.y));
        });
    }

    
    protected IEnumerator CaptureCoroutine(int w,int h)
    {
        yield return new WaitForEndOfFrame();
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
        Debug.Log(texture.width);
        int x = (texture.width - w);
        int y = (texture.height - h)/2;
        

        Color[] colors = texture.GetPixels(x, y, w, h);
        Debug.Log(colors.Length);
        
        Texture2D saveTexture = new Texture2D(w, h, TextureFormat.ARGB32, false);
        saveTexture.SetPixels(colors);
        File.WriteAllBytes("ss.png", saveTexture.EncodeToPNG());
        

    }
}
