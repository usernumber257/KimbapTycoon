using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

public class Holder : MonoBehaviour
{
    [SerializeField] Transform holdPlace;
    public GameObject holdingObj;
    public UnityAction<GameObject> OnHold;

    [SerializeField] SpriteRenderer sprite;

    public bool alreadyHold;

    int mySortingOrder = 0;
    int originSortingOrder = 15;

    //[SerializeField] bool changeHoldingObjOrder = true;

    private void Awake()
    {
        mySortingOrder = sprite.sortingOrder + 1;
    }

    public void Hold(GameObject go)
    {
        //go.transform.parent = holdPlace;

        //z ���� ���� �� ��� �ִ� ������Ʈ�� ���� ���õ� �� �ְ�
        //go.transform.localPosition = new Vector3(0f, 0f, -0.1f);

        if (go == null)
            return;

        Holder targetHolder = go.GetComponent<Holder>();

        if (targetHolder == null)
        {
            if (go.GetComponent<CompleteFood>() == null)//food stacker �� holder �� ���� �׳� ��� �������� �ǳ���
                return;

            go.transform.parent = holdPlace;
            holdingObj = go;
            holdingObj.transform.localPosition = Vector3.zero;
            holdingObj.GetComponent<Collider2D>().enabled = false;

            OnHold?.Invoke(go);
        }
        else
        {
            if (targetHolder.holdingObj == null)
                return;

            holdingObj = targetHolder.Give();
            holdingObj.transform.parent = holdPlace;
            OnHold?.Invoke(targetHolder.holdingObj);
        }

        //if (changeHoldingObjOrder)
        {
            //����� ���̺� ���� �� ���̾� ������ ����
            go.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = mySortingOrder;
        }
    }    

    public GameObject Give()
    {
        GameObject temp = holdingObj;

        //���ĵ� ���̾� �ʱ�ȭ
        temp.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = originSortingOrder;

        holdingObj = null;
        
        return temp;
    }

    private void Update()
    {
        if (!alreadyHold && holdingObj != null)
            alreadyHold = true;
        else if (alreadyHold && holdingObj == null)
            alreadyHold = false;
    }
}
