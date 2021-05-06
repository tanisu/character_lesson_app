using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    public void SetUp(string text, string leftButtonText, string rightButtonText, UnityEngine.Events.UnityAction leftButtonAction, UnityEngine.Events.UnityAction rightButtonAction)
    {
        // ���C���̃e�L�X�g��text������
        transform.GetChild(0).GetComponent<Text>().text = text;

        // ���{�^���̃e�L�X�g��leftButtonText������
        transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<Text>().text = leftButtonText;

        // �E�{�^���̃e�L�X�g��rightButtonText������
        transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = rightButtonText;

        // ���{�^����OnClick�Ƀ��\�b�h������
        transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(retrunTrue);

        // �E�{�^����OnClick�Ƀ��\�b�h������
        transform.GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(rightButtonAction);
    }

    private void Start()
    {
        transform.GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(retrunTrue);
    }

    private void retrunTrue()
    {
        Debug.Log("true");
        //return true;
    }
}
