using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Camera _camera;
    private Rigidbody _rigidBody;

    // Player Move Speed
    [Header("Move")]
    [SerializeField]
    private float _moveSpeed;
    private float _h;
    private float _v;

    // Mouse
    [Header("Rotate")]
    [SerializeField]
    private float _mouseSpeed;
    private float _xRotation;
    private float _yRotation;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;      
    }

    // Update is called once per frame
    void Update()
    {
        _Move();
        _Rotate();
    }

    private void _Move()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");

        Vector3 moveVec = transform.forward * _v + transform.right * _h;
        transform.position += moveVec.normalized * _moveSpeed * Time.deltaTime;
    }

    private void _Rotate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * _mouseSpeed * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * _mouseSpeed * Time.deltaTime;

        _xRotation -= mouseY;
        _yRotation += mouseX;

        _xRotation = Mathf.Clamp(_xRotation, -90.0f, 90.0f);

        _camera.transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        transform.rotation = Quaternion.Euler(0, _yRotation, 0);
    }
}
