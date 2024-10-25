using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowMover3 : MonoBehaviour
{
    [SerializeField] float moveDistance = 4f; // �̵� �Ÿ�
    [SerializeField] float moveSpeed = 20f; // �̵� �ӵ�
    [SerializeField] float xRotationThreshold = 0.1f; // X ȸ�� ��� ����
    [SerializeField] float zRotationThreshold = 0.1f; // Z ȸ�� ��� ����
    [SerializeField] float targetYPosition = 0.5f; // Y ��ġ ��� ��

    private bool isMoving = false; // �̵� ������ ����
    private Transform playerTransform; // �θ� ������Ʈ�� transform

    void Start()
    {
        playerTransform = transform.parent; // �θ� ������Ʈ�� transform�� ������
    }

    void Update()
    {
        // �̵� ������ �ʰ�, ������ ������ ���� ���콺 ���� Ű �Է��� Ȯ��
        if (!isMoving &&
            Mathf.Abs(playerTransform.eulerAngles.x) < xRotationThreshold &&
            Mathf.Abs(playerTransform.eulerAngles.z) < zRotationThreshold &&
            Mathf.Abs(playerTransform.position.y - targetYPosition) < 0.01f &&
            Input.GetMouseButtonDown(0))
        {
            // ī�޶� �ٶ󺸴� �������� �̵� ���� ���
            Vector3 moveDirection = Camera.main.transform.forward; // ī�޶� ����
            moveDirection.y = 0; // Y �� �̵� ����

            // X �Ǵ� Z �������θ� �̵��ϵ��� ����
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.z))
            {
                moveDirection.z = 0; // Z ���� ����
            }
            else
            {
                moveDirection.x = 0; // X ���� ����
            }

            moveDirection.Normalize(); // ���� ����ȭ
            moveDirection *= moveDistance; // �̵� �Ÿ� ����
            StartCoroutine(SmoothMove(moveDirection)); // �̵�
        }
    }

    IEnumerator SmoothMove(Vector3 direction)
    {
        isMoving = true; // �̵� ����
        Vector3 startPosition = playerTransform.position; // �θ� ������Ʈ�� ���� ��ġ
        Vector3 endPosition = startPosition + direction; // �� ��ġ ���
        float elapsedTime = 0;

        while (elapsedTime < moveDistance / moveSpeed)
        {
            playerTransform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / (moveDistance / moveSpeed)); // �θ� ������Ʈ ��ġ ������Ʈ
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerTransform.position = endPosition; // ������ ��ġ ����
        isMoving = false; // �̵� �Ϸ�
    }
}
