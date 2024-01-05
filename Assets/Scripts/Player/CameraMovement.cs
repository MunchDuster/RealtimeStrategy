using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float maxHeight;

    [SerializeField] private Transform camera;

    private float _angleX, _angleY, _height;
    private void Update()
    {
        bool rotating = Input.GetKey(KeyCode.Space);
        if (rotating)
        {
            _angleY += Input.GetAxisRaw("Horizontal") * rotationSpeed * Time.deltaTime;
            _angleX -= Input.GetAxisRaw("Vertical") * rotationSpeed * Time.deltaTime;

            _angleX = Mathf.Clamp(-90, _angleX, 90);
        }

        Quaternion rotation = Quaternion.Euler(0, _angleY, 0);

        if(!rotating)
            transform.position += rotation * new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * moveSpeed * Time.deltaTime;
        
        transform.localRotation = rotation;

        camera.localRotation = Quaternion.Euler(_angleX, 0, 0);

        _height -= Input.mouseScrollDelta.y;
        _height = Mathf.Clamp(_height, 0, maxHeight);
        camera.localPosition = Vector3.up * _height;
    }
}
