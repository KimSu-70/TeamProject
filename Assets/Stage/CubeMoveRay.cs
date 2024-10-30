using UnityEngine;

public class CubeMoveRay : MonoBehaviour
{
    // 큐브 제어를 위한 스크립트
    [SerializeField] private CubeMove _cubeMove;

    // 해당 객체가 관리할 방향
    [SerializeField] private CubePos _blockingDir;

    // 바닥이 가지는 레이를 입력
    [SerializeField] private LayerMask _layerMask;

    // 큐브기준 오프셋
    [SerializeField] private Vector3 _offSet;

    // 트리거 상태 일 때 트루
    bool _wallTrigger;

    private void OnTriggerStay(Collider other)
    {
        // 맵이 가지는 레이어(Default)가 아니라면 리턴
        if (other.gameObject.layer != 0) return;

        // 트리거 되면 해당 방향을 막음
        _cubeMove.BlockingDir[(int)_blockingDir] = _blockingDir;

        // 트리거 진입 상태
        _wallTrigger = true;
    }
    private void OnTriggerExit(Collider other)
    {
        // 맵이 가지는 레이어(Default)가 아니라면 리턴
        if (other.gameObject.layer != 0) return;

        // 벽면에서 빠져 나왔음으로 BlockingDir을 None으로 변경
        _cubeMove.BlockingDir[(int)_blockingDir] = CubePos.None;

        // 트리거 벗어남
        _wallTrigger = false;
    }

    private void Update()
    {
        // 큐브 위치 + 오프셋 위치로 이동
        transform.position = _cubeMove.transform.position + _offSet;

        // 트리거 중 이라면 리턴
        if (_wallTrigger) return;

        // 바닥이 감지 안돼면 해당 방향을 블로킹
        _cubeMove.BlockingDir[(int)_blockingDir] = Physics.Raycast(transform.position, Vector3.down, out RaycastHit _hit, 5f, _layerMask) ? CubePos.None : _blockingDir;
    }
}