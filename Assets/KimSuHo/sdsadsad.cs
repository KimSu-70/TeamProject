using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sdsadsad : MonoBehaviour
{
    private float rotationAngle = 90f; // ȸ���� ����
    private float rotationSpeed = 360f; // �ʴ� ȸ�� �ӵ�

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // A Ű�� ������ ��
        {
            StartCoroutine(Rotate(-rotationAngle)); // �������� 90�� ȸ��
        }
        else if (Input.GetKeyDown(KeyCode.Q)) // D Ű�� ������ ��
        {
            StartCoroutine(Rotate(rotationAngle)); // ���������� 90�� ȸ��
        }
    }

    private IEnumerator Rotate(float angle)
    {
        float targetAngle = transform.eulerAngles.y + angle; // ��ǥ ����
        float currentAngle = transform.eulerAngles.y; // ���� ����
        float rotSpeed = rotationSpeed * Time.deltaTime; // ȸ�� �ӵ�

        //Mathf.Abs(���밪 ��ȯ) �����϶� ���, ��� �϶� ���
        while (Mathf.Abs(targetAngle - currentAngle) > rotSpeed)
        {
            // Mathf.MoveTowardsAngle(���� ����, ��ǥ ����, ȸ���ӵ�)
            currentAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotSpeed);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, currentAngle, transform.eulerAngles.z);
            yield return null; // ���� �����ӱ��� ���
        }

        // ��ǥ ������ ������ �� ���� ������ ����
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, targetAngle, transform.eulerAngles.z);
    }
}
