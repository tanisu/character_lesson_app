using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// ‚¨ŠG•`‚«
/// </summary>
public class Painter : MonoBehaviour
{
    Texture2D texture;
    Vector3 beforeMousePos;

    Color bgColor = Color.white;
    Color lineColor = Color.red;

    void Start()
    {
        var img = GetComponent<Image>();
        var rt = GetComponent<RectTransform>();
        var width = (int)rt.rect.width;
        var height = (int)rt.rect.height;
        texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        Color32[] texColors = Enumerable.Repeat<Color32>(bgColor, width * height).ToArray();
        texture.SetPixels32(texColors);
        texture.Apply();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            beforeMousePos = GetPosition();
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 v = GetPosition();
            LineTo(beforeMousePos, v, lineColor);
            beforeMousePos = v;
            texture.Apply();
        }
    }


    public Vector3 GetPosition()
    {
        Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1.0f);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3 localPos = transform.InverseTransformPoint(worldPosition.x, worldPosition.y, -1.0f);
        var rect1 = GetComponent<RectTransform>();
        int xpos = (int)(localPos.x);
        int ypos = (int)(localPos.y);

        if (xpos < 0) xpos = xpos + (int)rect1.rect.width / 2;
        else xpos += (int)rect1.rect.width / 2;

        if (ypos > 0) ypos = ypos + (int)rect1.rect.height / 2;
        else ypos += (int)rect1.rect.height / 2;

        return new Vector3(xpos, ypos, 0);

    }


    public void LineTo(Vector3 start, Vector3 end, Color color)
    {

 
        float x = start.x, y = start.y;

        int paintW = 3, paintH = 3;
        int paintColorNum = paintW * paintH;
        Color[] wcolor = new Color[paintColorNum];
        for(int i = 0;i < wcolor.Length; i++)
        {
            wcolor[i] = Color.red;
        }


        if (Mathf.Abs(start.x - end.x) > Mathf.Abs(start.y - end.y))
        {
            float dy = Math.Abs(end.x - start.x) < float.Epsilon ? 0 : (end.y - start.y) / (end.x - start.x);
            float dx = start.x < end.x ? 1 : -1;
            while (x > 0 && x < texture.width && y > 0 && y < texture.height)
            {
                try
                {
                    texture.SetPixels((int)x, (int)y, paintW, paintH, wcolor);
                    x += dx;
                    y += dx * dy;
                    if (start.x < end.x && x > end.x ||
                        start.x > end.x && x < end.x)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    break;
                }
            }
        }
        else if (Mathf.Abs(start.x - end.x) < Mathf.Abs(start.y - end.y))
        {
            
            float dx = Math.Abs(start.y - end.y) < float.Epsilon ? 0 : (end.x - start.x) / (end.y - start.y);
            float dy = start.y < end.y ? 1 : -1;
            while (x > 0 && x < texture.width && y > 0 && y < texture.height)
            {
                try
                {
                    texture.SetPixels((int)x, (int)y, paintW, paintH, wcolor);
                    x += dx * dy;
                    y += dy;
                    if (start.y < end.y && y > end.y ||
                        start.y > end.y && y < end.y)
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    break;
                }
            }
        }
    }
}