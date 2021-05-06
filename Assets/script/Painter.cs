using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// ‚¨ŠG•`‚«
/// </summary>
public class Painter : MonoBehaviour
{
    Texture2D texture;
    Vector3 beforeMousePos;

    //Color bgColor = Color.black;
    Color bgColor = new Color(0f,0f,0f,0f);

    Color[] buffer;
    Vector2 _prevPos;

    void Start()
    {
        Image img = GetComponent<Image>();
        RectTransform rt = GetComponent<RectTransform>();
        int width = (int)rt.rect.width;
        int height = (int)rt.rect.height;
        texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        Color[] pixels = texture.GetPixels();
        buffer = new Color[pixels.Length];
        pixels.CopyTo(buffer, 0);
        img.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                buffer.SetValue(bgColor, x + width * y);
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();
    }

    public void paintCanvas(List<List<Vector3>> positions)
    {
        foreach(List<Vector3> pos in positions)
        {

            foreach(var (p,index) in pos.Select((item,index) => (item,index)))
            {
                if (_prevPos == Vector2.zero)
                {
                    Debug.Log("Method zero");
                    _prevPos = p;
                }
                //else
                //{
                //    _prevPos = pos[index - 1];
                //}
                Vector2 endPos = p;
                //Debug.Log($"Method _prevPos:{_prevPos} endPos:{endPos}");
                float lineLength = Vector2.Distance(_prevPos, endPos);
                Debug.Log($"Method lineLength:{lineLength}");
                int lerpCountAdjustNum = 5;
                int lerpCount = Mathf.CeilToInt(lineLength / lerpCountAdjustNum);
                for(int i = 1;i <= lerpCount; i++)
                {
                    float lerpWeight = (float)i / lerpCount;
                    Vector3 lerpPos = Vector3.Lerp(_prevPos, Input.mousePosition, lerpWeight);
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(lerpPos);
                    Vector3 localPos = transform.InverseTransformPoint(worldPos.x, worldPos.y, -1.0f);
                    RectTransform r = GetComponent<RectTransform>();
                    int xpos = (int)localPos.x;
                    int ypos = (int)localPos.y;
                    if (xpos < 0)
                    {
                        xpos = xpos + (int)r.rect.width / 2;
                    }
                    else
                    {
                        xpos += (int)r.rect.width / 2;
                    }
                    if (ypos > 0)
                    {
                        ypos = ypos + (int)r.rect.height / 2;
                    }
                    else
                    {
                        ypos += (int)r.rect.height / 2;
                    }

                    DrawBuffer(new Vector2(xpos, ypos));
                    texture.SetPixels(buffer);
                    texture.Apply();
                }
                _prevPos = p;
            }
            _prevPos = Vector2.zero;
        }
       

    }

  /*  void Update()
    { 
        if (Input.GetMouseButton(0))
        {
           
            if (_prevPos == Vector2.zero)
            {
                Debug.Log("Update zero");
                _prevPos = Input.mousePosition;
            }
            Vector2 endPos = Input.mousePosition;
            //Debug.Log($"Update _prevPos:{_prevPos} endPos:{endPos}");
            float lineLength = Vector2.Distance(_prevPos, endPos);
            Debug.Log($"Update lineLength:{lineLength}");
            int lerpCountAdjustNum = 5;
            int lerpCount = Mathf.CeilToInt(lineLength / lerpCountAdjustNum);
            for (int i = 1; i <= lerpCount; i++)
            {
                float lerpWeight = (float)i / lerpCount;
                Vector3 lerpPos = Vector3.Lerp(_prevPos, Input.mousePosition, lerpWeight);
                Vector3 pos = Camera.main.ScreenToWorldPoint(lerpPos);
                Vector3 localPos = transform.InverseTransformPoint(pos.x, pos.y, -1.0f);
                RectTransform r = GetComponent<RectTransform>();
                int xpos = (int)localPos.x;
                int ypos = (int)localPos.y;
                if (xpos < 0)
                {
                    xpos = xpos + (int)r.rect.width / 2;
                }
                else
                {
                    xpos += (int)r.rect.width / 2;
                }
                if (ypos > 0)
                {
                    ypos = ypos + (int)r.rect.height / 2;
                }
                else
                {
                    ypos += (int)r.rect.height / 2;
                }

                DrawBuffer(new Vector2(xpos, ypos));
                texture.SetPixels(buffer);
                texture.Apply();
            }
            _prevPos = Input.mousePosition;
        }
        else
        {
            _prevPos = Vector2.zero;
        }
            

    }
  */

    public void DrawBuffer(Vector2 p)
    {
        for(int x = 0;x < texture.width; x++)
        {
            for(int y = 0;y < texture.height; y++)
            {
                if((p - new Vector2(x,y)).magnitude < 1.5)
                {
                    buffer.SetValue(Color.red, x + texture.width * y);
                }
            }
        }
    }



}