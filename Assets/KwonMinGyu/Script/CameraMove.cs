using UnityEngine;
public enum CameraPos
{
    Up, Down, Right, Left
}
public class CameraMove : MonoBehaviour
{
    [SerializeField] private Transform _cubeTransform;  // Cube ������Ʈ
    [SerializeField] private Vector3 _offset = new Vector3(0, 5, -8);  // ī�޶� �⺻ ��ġ
    [SerializeField] private float _mouseSensitivity = 200f;  // ���콺 �ΰ���
    [SerializeField] private float _rotationUpLimit = 50f;  // ī�޶� ȸ���� �� �ִ� �ִ� ���� (���� ����)
    [SerializeField] private float _rotationDownLimit = -20f;  // ī�޶� ȸ���� �� �ִ� �ִ� ���� (���� ����)

    private float _pitch = 0f;  // ���Ʒ� ���� (X�� ȸ��)
    private float _yaw = 0f;  // �¿� ���� (Y�� ȸ��)

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // �� �����Ӹ��� ȣ��Ǵ� ī�޶� ������Ʈ
    void LateUpdate()
    {
        // ���콺 �Է� �ޱ�
        float mouseX = Input.GetAxis("Mouse X") * _mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _mouseSensitivity * Time.deltaTime;

        // ���콺 �Է��� �������� ī�޶��� ���� ����
        _yaw += mouseX;
        _pitch -= mouseY;

        // ���Ʒ� ���� ���� (ť�긦 �Ѿ�� �ʵ���)
        _pitch = Mathf.Clamp(_pitch, _rotationDownLimit, _rotationUpLimit);

        // ī�޶��� ȸ�� ���� (ť�긦 �߽����� ȸ��) 
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        // ī�޶� ť�긦 �߽����� ȸ���ϵ��� ��ġ ���
        transform.position = _cubeTransform.position + rotation * _offset;  // Quaternion * position = postion�� ���� ����
        // ī�޶� �׻� ť�긦 �ٶ󺸵��� ����
        transform.LookAt(_cubeTransform.position);
    }

    // ī�޶� ť�긦 �������� ��� ���⿡ �ִ��� ���ϴ� �Լ�
    public CameraPos CameraPosition()
    {
        CameraPos _cameraPos = new CameraPos();
        // ť�긦 ���������� �� ī�޶��� x��ǥ
        float tempX = transform.position.x - _cubeTransform.position.x;
        // ī�޶��� ù y��ǥ�� �������� �̵��� y��ǥ
        float tempY = transform.position.y - (_offset.y + _cubeTransform.position.y);

        // ī�޶��� ��ǥ���� �뷫 +-_offset.z - 2 �϶� ���� ��ȯ
        // y���� ���� �� x�� ���� ���ϱ� ������ tempY�� ����
        if (tempX + tempY > -_offset.z - 2)
            _cameraPos = CameraPos.Right;
        else if (tempX - tempY < _offset.z + 2)
            _cameraPos = CameraPos.Left;
        // ī�޶��� ��ġ�� �� ���� �ƴϰ� z��ǥ���� ������ ī�޶�� ť���� �� ���⿡ ����
        else if (transform.position.z - _cubeTransform.position.z > 0)
            _cameraPos = CameraPos.Up;
        else
            _cameraPos = CameraPos.Down;
        //Debug.Log(_cameraPos);
        return _cameraPos;
    }
}
