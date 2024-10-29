using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class YellowMover2 : BaseState2
{
    [SerializeField] float moveDistance = 4f; // 이동 거리
    [SerializeField] float moveSpeed = 20f; // 이동 속도
    [SerializeField] float xRotationThreshold = 0.1f; // X 회전 허용 범위
    [SerializeField] float zRotationThreshold = 0.1f; // Z 회전 허용 범위

    private bool isMoving = false; // 이동 중인지 여부
    [SerializeField] private Transform playerTransform; // 플레이어 오브젝트의 transform
    [SerializeField] private Transform checkUpPoint;    // 윗면 확인 오브젝트의 transform
    [SerializeField] Coroutine moveCoroutine;

    public YellowMover2(UseStampAbility2 parent) : base(parent)
    {
        playerTransform = GameObject.FindWithTag("Player").transform; // 플레이어 오브젝트의 transform을 가져옴
        checkUpPoint = GameObject.FindWithTag("Finish").transform;
    }

    public override void Enter()
    {
        Debug.Log("Yellow Enter!");  
    }

    public override void Update()
    {
        Debug.Log($"line 1 :{Mathf.Abs(playerTransform.eulerAngles.x) < xRotationThreshold}");
        Debug.Log($"line 2 :{Mathf.Abs(playerTransform.eulerAngles.z) < zRotationThreshold}");
        // 플레이어 오브젝트가 존재할 때만 이동 처리
        if (playerTransform != null && !isMoving &&
            Mathf.Abs(playerTransform.eulerAngles.x) < xRotationThreshold &&
            Mathf.Abs(playerTransform.eulerAngles.z) < zRotationThreshold &&
            Input.GetMouseButtonDown(0))
        {
            // 카메라가 바라보는 방향으로 이동 벡터 계산
            Vector3 moveDirection = Camera.main.transform.forward; // 카메라 방향
            moveDirection.y = 0; // Y 축 이동 방지

            // X 또는 Z 방향으로만 이동하도록 제한
            if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.z))
            {
                moveDirection.z = 0; // Z 방향 제거
            }
            else
            {
                moveDirection.x = 0; // X 방향 제거
            }

            moveDirection.Normalize(); // 방향 정규화
            moveDirection *= moveDistance; // 이동 거리 설정
            parent.StartCoroutine(SmoothMove(moveDirection)); // 이동
        }
    }

    public override void Exit()
    {
        isMoving = false;
    }

    IEnumerator SmoothMove(Vector3 direction)
    {
        isMoving = true; // 이동 시작
        Vector3 startPosition = playerTransform.position; // 플레이어 오브젝트의 시작 위치
        Vector3 endPosition = startPosition + direction; // 끝 위치 계산
        float elapsedTime = 0;

        Vector3 startCheckUpPointPosition = checkUpPoint.position; // 윗면 확인 오브젝트의 시작 위치
        Vector3 endCheckUpPointPosition = startCheckUpPointPosition + direction; // 끝 위치 계산

        while (elapsedTime < moveDistance / moveSpeed)
        {
            playerTransform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / (moveDistance / moveSpeed)); // 플레이어 오브젝트 위치 업데이트
            checkUpPoint.position = Vector3.Lerp(startCheckUpPointPosition, endCheckUpPointPosition, elapsedTime / (moveDistance / moveSpeed)); // 플레이어 오브젝트 위치 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerTransform.position = endPosition; // 마지막 위치 설정
        checkUpPoint.position = endCheckUpPointPosition;
        isMoving = false; // 이동 완료
    }
}
