using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

public class NewBehaviourScript : MonoBehaviour
{
    /// <summary>
    /// ���R���|�[�l���g�̃��X�g
    /// </summary>
    private List<LineRenderer> lineRenderers;

    /// <summary>
    /// �`��̈�R���|�[�l���g
    /// </summary>
    public GameObject paintArea;

    /// <summary>
    /// ���̃}�e���A��
    /// </summary>
    public Material lineMaterial;

    /// <summary>
    /// ���̐F
    /// </summary>
    public Color lineColor;

    /// <summary>
    /// ���̑���
    /// </summary>
    [Range(0, 10)] public float lineWidth;


  

    void Start()
    {
        lineRenderers = new List<LineRenderer>();
        //paintArea.GetComponent<Painter>().Test();
        lineColor = Color.cyan;

    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            this.AddLineObject();
        }

        if (Input.GetMouseButton(0))
        {
            this.AddPositionDataToLineRendererList();
        }
        //if (Input.GetMouseButtonUp(0))
        //{
            
        //    Debug.Log(lineRenderers.Last().positionCount);
        //}
    }

    /// <summary>
    /// ���I�u�W�F�N�g�̒ǉ����\�b�h
    /// </summary>
    private void AddLineObject()
    {
        //��̃Q�[���I�u�W�F�N�g�쐬
        GameObject lineObject = new GameObject("stroke");
        //��̃Q�[���I�u�W�F�N�g��LineRenderer�R���|�[�l���g�ǉ�
        lineObject.AddComponent<LineRenderer>();
        lineObject.AddComponent<CircleCollider2D>();
        lineObject.AddComponent<Rigidbody2D>();
        lineObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        lineObject.GetComponent<Rigidbody2D>().simulated = true;

        //LineRenderer���X�g�ɏ�L�Q�[���I�u�W�F�N�g�ǉ�
        lineRenderers.Add(lineObject.GetComponent<LineRenderer>());
        lineObject.transform.parent = transform;
        this.initLastRenderers();
    }

    public void Restart()
    {
        this.AddLineObject();
    }

    /// <summary>
    /// ���R���|�[�l���g������
    /// </summary>

    private void initLastRenderers()
    {
        //lineRenderers.Last().GetComponent<RectTransform>().SetAsLastSibling();
        //�����Ȃ��_���O�ɏ�����
        lineRenderers.Last().positionCount = 0;
        //�}�e���A����������
        lineRenderers.Last().material = this.lineMaterial;
        //�F������
        lineRenderers.Last().material.color = this.lineColor;
        //���̑���������
        lineRenderers.Last().startWidth = this.lineWidth;
        lineRenderers.Last().endWidth = this.lineWidth;
    }


    /// <summary>
    /// ���R���|�[�l���g�Ɉʒu���o�^���\�b�h
    /// </summary>
    private void AddPositionDataToLineRendererList()
    {
        //�}�E�X���W���擾���A���[���h���W�ɕϊ�
        Vector3 screenPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1.0f);
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(screenPosition);

        

        //���Ɛ����Ȃ��_�̐����X�V
        lineRenderers.Last().positionCount += 1;

        /*
        Debug.Log($"LineRenderer:mousePosition > {mousePosition}");
        Debug.Log($"LineRenderer:screenPosition > {screenPosition}");
        */
        //paintArea.GetComponent<Painter>().LineTo(mousePosition, mousePosition, Color.green);

        //���̃R���|�[�l���g���X�g���X�V
        lineRenderers.Last().SetPosition(lineRenderers.Last().positionCount - 1, mousePosition);
        
    }

    

}
