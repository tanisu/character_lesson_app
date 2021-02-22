using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paint : MonoBehaviour
{
    [SerializeField]
    private RawImage m_image = null;

    private Texture2D m_texture = null;

    // Start is called before the first frame update
    void Start()
    {
        Rect rect = m_image.gameObject.GetComponent<RectTransform>().rect;
        m_texture = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGBA32, false);
        m_image.texture = m_texture;

    }
    void OnDestroy()
    {
        if(m_texture != null)
        {
            m_texture = null;

        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Input.mousePosition;
            m_texture.SetPixel((int)pos.x, (int)pos.y, Color.red);
            m_texture.Apply();
        }
    }
}
