using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageAnimiton : MonoBehaviour
{
    private Text textComponent;
    private Vector3 doMoveVec3;
    private Vector3 initialPosition;
    private bool isStart = false;
    private void Awake()
    {
       
        textComponent = GetComponent<Text>();
    }
    private void Update()
    {
        if (isStart)
        {
            
            // ��ȡ������ĵ�ǰλ��
            Vector3 parentOffset = transform.parent.position - initialPosition;
            // �����ƶ�Ŀ��λ�ã����Ǹ������λ��
            Vector3 finalTargetPosition = doMoveVec3 + parentOffset;
            transform.DOMove(transform.position + doMoveVec3, 1.0f).SetUpdate(true);
        }
    }

    public void Init(float _offset)
    {
        doMoveVec3 = new Vector3(20, 20 * _offset);
        
        initialPosition = transform.position;
        DOTween.Kill(transform);
        textComponent.DOFade(0, 1.0f).OnComplete(() =>
        {
            DamageTextManager.Instance.ReturnDamageTextToPool(gameObject);
            isStart = false;
        });
        isStart = true;
    }
}