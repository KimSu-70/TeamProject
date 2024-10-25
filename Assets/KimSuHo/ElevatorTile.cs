using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTile : MonoBehaviour
{
    public Transform targetPosition; // ���������Ͱ� �̵��� ��ǥ ��ġ
    public Transform pos;            // ���� ���������� ��ġ
    public float speed = 2f; // �̵� �ӵ�
    private bool playerOnTile = false; // �÷��̾ Ÿ�� ���� �ִ��� ����

    void Update()
    {
        if (playerOnTile)
        {
            // ���������͸� ��ǥ ��ġ�� �̵�
            transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, pos.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾�� �浹���� ��
        {
            playerOnTile = true; // �÷��̾ Ÿ�� ���� �ִٰ� ����
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // �÷��̾ Ÿ�Ͽ��� ������ ��
        {
            playerOnTile = false; // �÷��̾ Ÿ�� ���� ���ٰ� ����
        }
    }
}
