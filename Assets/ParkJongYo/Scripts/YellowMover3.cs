using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowMover3 : MonoBehaviour
{
    [SerializeField] float moveDistance = 4f; // 이동 거리
    [SerializeField] float moveSpeed = 20f; // 이동 속도

    private bool isMoving = false; // 이동 중인지 여부
    private Transform playerTransform; // 플레이어 오브젝트의 transform
    [SerializeField] private CubeMove cubeMove; // CubeMove 스크립트 참조
    [SerializeField] private CubeChecker cubeChecker;

    // Raycast가 검사하는 레이어
    [SerializeField] private LayerMask _yellowMask;

    // 인스펙터에서 입력되는 거리를 저장 함
    private float _moveDistance;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform; // 플레이어 오브젝트의 transform을 가져옴
        }
        _moveDistance = moveDistance;
    }

    void Update()
    {
        // 플레이어 오브젝트가 존재할 때만 이동 처리
        if (playerTransform != null && !isMoving)
        {
            if (Input.GetMouseButtonDown(0))
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

                // Raycast로 이동 거리를 변경함
                moveDistance = StartRay(moveDirection);

                moveDirection *= moveDistance; // 이동 거리 설정

                StartCoroutine(SmoothMove(moveDirection)); // 이동
            }
        }
    }

    IEnumerator SmoothMove(Vector3 direction)
    {
        isMoving = true; // 이동 시작
        Vector3 startPosition = playerTransform.position; // 플레이어 오브젝트의 시작 위치
        Vector3 endPosition = startPosition + direction; // 끝 위치 계산
        float elapsedTime = 0;

        while (elapsedTime < moveDistance / moveSpeed)
        {
            playerTransform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / (moveDistance / moveSpeed)); // 플레이어 오브젝트 위치 업데이트
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerTransform.position = endPosition; // 마지막 위치 설정
        isMoving = false; // 이동 완료

        // 추가
        cubeMove.IsRolling = false;
        cubeChecker.transform.position = cubeMove.transform.position + Vector3.up * 0.5f;
        cubeMove.FallCheck();
    }
    private float StartRay(Vector3 _dir)
    {
        // 노랑색 스템프의 이동 방향으로 Raycast
        if (Physics.Raycast(transform.position, _dir, out RaycastHit hit, _moveDistance + 1, _yellowMask))
        {
            Debug.Log(hit.transform);

            // 검사되는 물체가 "CrackedRock" 태그를 가진 경우
            if (hit.transform.CompareTag("CrackedRock"))
            {
                StartCoroutine(DestroyCrackedRock(hit.transform.gameObject)); // CrackedRock 파괴 코루틴 시작
                return Mathf.Floor(Vector3.Distance(transform.position, hit.point)); // 충돌 위치 바로 앞에서 멈춤
            }

            // 큐브와 블록이 맞닿아 있을 때 거리를 1로 정확히 맞춤
            return Mathf.Floor(Vector3.Distance(transform.position, hit.point));
        }

        // 검사되는 물체가 없다면 최대 거리로 리턴
        return _moveDistance;
    }

    private IEnumerator DestroyCrackedRock(GameObject crackedRock)
    {
        yield return new WaitForSeconds(0.1f); // 0.1초 대기
        Destroy(crackedRock); // CrackedRock 파괴
    }
}
