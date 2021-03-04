using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StrokeManager : MonoBehaviour
{
    /// <summary>
    /// ���R���|�[�l���g�̃��X�g
    /// </summary>
    private List<LineRenderer> lineRenderers;

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

    private int strokeCount;
  

    void Start()
    {
        lineRenderers = new List<LineRenderer>();
        strokeCount = 0;
    }

    void Update()
    {
        //�Q�[���X�^�[�g��
        if (GameManager.instance.startFlag) {
            //�X�^�[�g�}�[�J�[�N���b�N��
            if (Input.GetMouseButtonDown(0))
            {
                this.AddLineObject();
            }
            //�X�g���[�N��
            if (Input.GetMouseButton(0))
            {
                //�{�[�h���ł���Ε`��
                if (GameManager.instance.onBoardFlag)
                {
                    this.AddPositionDataToLineRendererList();
                }
            }
            //�{�[�h�O�ŃN���b�N����߂����O�̃X�g���[�N�������A�{�[�h�t���O��true�ɂ���
            if (Input.GetMouseButtonUp(0) && !GameManager.instance.onBoardFlag && !GameManager.instance.onPanelFlag)
            {
                this.DestoryStroke();
                GameManager.instance.onBoardFlag = true;
                GameManager.instance.startFlag = false;
            }

            //�S�[�����Ă��Ȃ��̂ɃN���b�N����߂����O�̃X�g���[�N������
            if (Input.GetMouseButtonUp(0) && !GameManager.instance.enterGoal && GameManager.instance.onBoardFlag)
            {
                this.DestoryStroke();
                GameManager.instance.startFlag = false;
            }
            //�S�[�����ă}�E�X���グ����S�[���G���^�[�t���O��false�ɂ���
            //else if (Input.GetMouseButtonUp(0) && GameManager.instance.enterGoal)
            //{
            //    GameManager.instance.enterGoal = false;
            //}
        }







    }

    /// <summary>
    /// ���I�u�W�F�N�g�̒ǉ����\�b�h
    /// </summary>
    private void AddLineObject()
    {
        
        //��̃Q�[���I�u�W�F�N�g�쐬
        GameObject lineObject = new GameObject("stroke");
        lineObject.tag = "stroke";
        //��̃Q�[���I�u�W�F�N�g��LineRenderer�R���|�[�l���g�ǉ�
        lineObject.AddComponent<LineRenderer>();
            
        lineObject.AddComponent<CircleCollider2D>();
        lineObject.GetComponent<CircleCollider2D>().radius = 0.2f;
        lineObject.GetComponent<CircleCollider2D>().isTrigger = true;
        lineObject.AddComponent<Rigidbody2D>();
        lineObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        lineObject.GetComponent<Rigidbody2D>().simulated = true;
        lineObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        //LineRenderer���X�g�ɏ�L�Q�[���I�u�W�F�N�g�ǉ�
        lineRenderers.Add(lineObject.GetComponent<LineRenderer>());
        lineObject.transform.parent = transform;
            
        this.initLastRenderers();
        strokeCount++;
        //Debug.Log(strokeCount);
        
    }

    public void Restart()
    {
        this.AddLineObject();
    }

    public void StopStroke()
    {
        Destroy(this);
    }
 
    public void DestoryStroke()
    {
        Destroy(lineRenderers[lineRenderers.Count - 1].gameObject);
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
        try
        {
            //�}�E�X���W���擾���A���[���h���W�ɕϊ�
            Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane + 1.0f);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);



            //���Ɛ����Ȃ��_�̐����X�V
            lineRenderers.Last().positionCount += 1;



            //���̃R���|�[�l���g���X�g���X�V
            lineRenderers.Last().SetPosition(lineRenderers.Last().positionCount - 1, worldPosition);


            Vector3 localPos = transform.InverseTransformPoint(worldPosition.x, worldPosition.y, -1.0f);
            //Debug.Log($"before:{localPos}");
            lineRenderers.Last().transform.localPosition = localPos;

            //Debug.Log($"local:{lineRenderers.Last().transform.localPosition}");
        }catch(MissingReferenceException e)
        {
            Debug.LogError(e.Message);
        }



    }

    




}
