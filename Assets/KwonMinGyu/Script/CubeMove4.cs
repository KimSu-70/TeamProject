using System;
using System.Collections;
using UnityEngine;
public enum CubePos
{
    Up, Down, Right, Left
}
public class CubeMove4 : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _SlopeSpeed;
    [SerializeField] private CameraMove4 _cameraMove;
    [SerializeField] private CubeChecker4 _cubeChecker;
    [SerializeField] private BoxCollider _cubeUpCheck;
    [SerializeField] private Rigidbody _rigidbody;

    CameraPos _cameraPos;
    CubePos _cubePos;

    Vector3[] _moveDir = new Vector3[4];
    Bounds bound;

    bool IsRolling;

    public bool slopeForward;
    public CubePos slopeDir;

    void Start()
    {
        // bound.size������ �ݶ��̴��� ������� size�� ���� �� ����
        // �Ʒ� ������ RotateAround�� ȸ�� �������� ��
        bound = GetComponent<BoxCollider>().bounds;
        _moveDir[0] = new Vector3(0, -bound.size.y / 2, bound.size.z / 2);
        _moveDir[1] = new Vector3(0, -bound.size.y / 2, -bound.size.z / 2);
        _moveDir[2] = new Vector3(bound.size.x / 2, -bound.size.y / 2, 0);
        _moveDir[3] = new Vector3(-bound.size.x / 2, -bound.size.y / 2, 0);

        _rigidbody = GetComponent<Rigidbody>();

        if (!_cubeChecker.IsGround())
            CubeFall();
    }

    void Update()
    {
        _cameraPos = _cameraMove.CameraPosition();
        float _HMove = Input.GetAxisRaw("Horizontal");
        float _VMove = Input.GetAxisRaw("Vertical");

        if (_HMove == 0 && _VMove == 0) return;


        // Move Forward
        if (_VMove == 1)
        {
            // CameraPosition()�� ī�޶� ������ ���� ȸ�� ������ ����
            switch (_cameraPos)
            {
                case CameraPos.Up:
                    _cubePos = CubePos.Down;
                    break;
                case CameraPos.Down:
                    _cubePos = CubePos.Up;
                    break;
                case CameraPos.Right:
                    _cubePos = CubePos.Left;
                    break;
                case CameraPos.Left:
                    _cubePos = CubePos.Right;
                    break;
            }
        }

        // Move Backwards
        else if (_VMove == -1)
        {
            switch (_cameraPos)
            {
                case CameraPos.Up:
                    _cubePos = CubePos.Up;
                    break;
                case CameraPos.Down:
                    _cubePos = CubePos.Down;
                    break;
                case CameraPos.Right:
                    _cubePos = CubePos.Right;
                    break;
                case CameraPos.Left:
                    _cubePos = CubePos.Left;
                    break;
            }
        }

        // Move Right
        else if (_HMove == 1)
        {
            switch (_cameraPos)
            {
                case CameraPos.Up:
                    _cubePos = CubePos.Left;
                    break;
                case CameraPos.Down:
                    _cubePos = CubePos.Right;
                    break;
                case CameraPos.Right:
                    _cubePos = CubePos.Up;
                    break;
                case CameraPos.Left:
                    _cubePos = CubePos.Down;
                    break;
            }
        }

        // Move Left
        else if (_HMove == -1)
        {
            switch (_cameraPos)
            {
                case CameraPos.Up:
                    _cubePos = CubePos.Right;
                    break;
                case CameraPos.Down:
                    _cubePos = CubePos.Left;
                    break;
                case CameraPos.Right:
                    _cubePos = CubePos.Down;
                    break;
                case CameraPos.Left:
                    _cubePos = CubePos.Up;
                    break;
            }
        }
        if (!IsRolling)
        {
            // ���� �տ��� ���� ������ �̵��� ���� �̵� �ڷ�ƾ ����
            if (slopeForward && slopeDir == _cubePos)
            {
                StartCoroutine(SlopeRoll(_cubePos));
                return;
            }
            StartCoroutine(Roll(_cubePos));
        }
    }

    IEnumerator Roll(CubePos cubePos)
    {
        Vector3 positionToRotation = _moveDir[(int)cubePos];

        IsRolling = true;
        _cubeUpCheck.enabled = false; // �̵� �� �ٸ� �ݶ��̴��� �������� �ʵ��� _cubeUpCheck ��Ȱ��ȭ

        float angle = 0;
        Vector3 point = transform.position + positionToRotation;
        Vector3 axis = Vector3.Cross(Vector3.up, positionToRotation).normalized;

        while (angle < 90f)
        {
            float angleSpeed = Time.deltaTime * _rotationSpeed;
            transform.RotateAround(point, axis, angleSpeed); // (������, ����, ȸ����)���� ȸ��
            angle += angleSpeed;
            yield return null;
        }

        transform.RotateAround(point, axis, 90 - angle); // ȸ���� 90������ ���ų� ���� �� �� 90���� ����

        IsRolling = false;

        _cubeUpCheck.transform.position = transform.position + Vector3.up; // _cubeUpCheck ť�� ���� �̵�
        _cubeUpCheck.enabled = true;

        if (!_cubeChecker.IsGround())
            CubeFall();
    }

    IEnumerator SlopeRoll(CubePos cubePos)
    {
        Vector3 positionToRotation = _moveDir[(int)cubePos];

        IsRolling = true;
        _cubeUpCheck.enabled = false; // �̵� �� �ٸ� �ݶ��̴��� �������� �ʵ��� _cubeUpCheck ��Ȱ��ȭ

        float angle = 0;
        Vector3 point = transform.position + positionToRotation;
        Vector3 axis = Vector3.Cross(Vector3.up, positionToRotation).normalized;

        while (angle < 134f)
        {
            float angleSpeed = Time.deltaTime * _rotationSpeed;
            transform.RotateAround(point, axis, angleSpeed);
            angle += angleSpeed;
            yield return null;
        }

        transform.RotateAround(point, axis, 134f - angle);

        // ���� ���� ��ǥ ����
        Vector3 _slopePoint = new();
        switch (_cubePos)
        {
            case CubePos.Up:
                _slopePoint = new Vector3(0, -3.3f, 3.3f);
                break;
            case CubePos.Down:
                _slopePoint = new Vector3(0, -3.3f, -3.3f);
                break;
            case CubePos.Right:
                _slopePoint = new Vector3(3.3f, -3.3f, 0);
                break;
            case CubePos.Left:
                _slopePoint = new Vector3(-3.3f, -3.3f, 0);
                break;
        }
        Vector3 TPos = transform.position + _slopePoint;
        while (true)
        {
            transform.position = Vector3.MoveTowards(transform.position, TPos, _SlopeSpeed * Time.deltaTime);
            if (TPos.sqrMagnitude - transform.position.sqrMagnitude < 0.01f)
                break;
            yield return null;
        }
        angle = 134f;
        point = transform.position + (-Vector3.up * 0.5f * MathF.Sqrt(2));
        while (angle < 180f)
        {
            float angleSpeed = Time.deltaTime * _rotationSpeed;
            transform.RotateAround(point, axis, angleSpeed);
            angle += angleSpeed;
            yield return null;
        }
        transform.RotateAround(point, axis, 180f - angle);
        IsRolling = false;

        _cubeUpCheck.transform.position = transform.position + Vector3.up;
        _cubeUpCheck.enabled = true;

        transform.position = new Vector3((float)Math.Round(transform.position.x), (float)Math.Round(transform.position.y), (float)Math.Round(transform.position.z));
    }

    private void CubeFall()
    {
        IsRolling = true; // �������� �� ȸ�� ����
        _rigidbody.isKinematic = false; // ������ Ȱ��ȭ
        transform.GetComponent<BoxCollider>().size = new Vector3(0.9f, 0.9f, 0.9f); // �ݶ��̴� ��ҷ� ���� ����
    }

    // CubeFall()���� ������ Ȱ��ȭ �� ���� OnCollisionEnter�� ����� �� ����
    private void OnCollisionEnter(Collision collision)
    {
        IsRolling = false;
        _rigidbody.isKinematic = true;

        transform.GetComponent<BoxCollider>().size = Vector3.one; // �ݶ��̴� ����

        _cubeUpCheck.transform.position = transform.position + Vector3.up; // _cubeUpCheck ť�� ���� �̵�

        // ���� ���� ��ǥ�� ����
        transform.position = new Vector3((float)Math.Round(transform.position.x), (float)Math.Round(transform.position.y), (float)Math.Round(transform.position.z));
    }
}
