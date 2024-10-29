using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

/// <summary>
/// 콜라이더 
/// </summary>
public class CheckCollider2 : MonoBehaviour
{
    [SerializeField] StampType2 type;               // 큐브에 달려있는 스탬프 확인
    [SerializeField] BoxCollider boxCollider;       // 콜라이더

    [SerializeField] UseStampAbility2 useStampAbility;  // 스탬프 능력을 관리하는 스크립트


    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update()
    {
        // 임시로 번호키를 누르면 변경
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            type.ChangeType = StampType2.StampType.None;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            type.ChangeType = StampType2.StampType.Red;

        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            type.ChangeType = StampType2.StampType.Blue;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            type.ChangeType = StampType2.StampType.Yellow;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        StampType2 type = other.GetComponent<StampType2>();

        // type 컴포넌트가 있다 -> cube에 붙어 있는 Stemp
        if (type is not null)
        {
            this.type = type;
            useStampAbility.ChangeAbility(type.GetStampType);
            Debug.Log($"enter object : {other.gameObject.name}");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        StampType2 type = other.GetComponent<StampType2>();

        if (this.type.Equals(type))
        {
            this.type = null;
            useStampAbility.ChangeAbility(StampType2.StampType.None);
        }
    }
}