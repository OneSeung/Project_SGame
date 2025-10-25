using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,
    Walk,
    Run
}

public class PlayerController : MonoBehaviour
{
    private Camera _camera;
    private Rigidbody _rigidBody;

    // Player Move Speed
    [Header("Move")]
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _currentSpeed;
    private float _h, _v;

    // Mouse
    [Header("Rotate")]
    [SerializeField] private float _mouseSpeed = 500.0f;
    private float _xRotation;
    private float _yRotation;

    // Stamina & State
    [Header("Stamina & State")]
    [SerializeField] private int _hp;
    [SerializeField] private float _stamina;
    private float _staminaMax;
    private float _staminaMinus;
    private float _staminaPlus;
    private float _staminaPlusDelay;
    private float _lastRunTime = -999.0f;
    [SerializeField] private PlayerState _state = PlayerState.Idle;

    void Start()
    {
        _camera = Camera.main;
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _hp = PlayerSettings.Instance.Hp;
        _walkSpeed = PlayerSettings.Instance.WalkSpeed;
        _runSpeed = PlayerSettings.Instance.RunSpeed;

        _staminaMax = PlayerSettings.Instance.StaminaMax;
        _staminaMinus = PlayerSettings.Instance.StaminaMinus;
        _staminaPlus = PlayerSettings.Instance.StaminaPlus;
        _staminaPlusDelay = PlayerSettings.Instance.StaminaPlusDelay;

        _stamina = _staminaMax;
    }

    void Update()
    {
        _Move();
        _Rotate();
        _HandleStamina();
    }

    private void _Move()
    {
        _h = Input.GetAxisRaw("Horizontal");
        _v = Input.GetAxisRaw("Vertical");

        bool isMoving = (_h != 0 || _v != 0);
        bool isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving && _stamina > 0;

        _currentSpeed = isRunning ? _runSpeed : _walkSpeed;

        Vector3 moveVec = transform.forward * _v + transform.right * _h;
        transform.position += moveVec.normalized * _currentSpeed * Time.deltaTime;

        if (!isMoving)
            _state = PlayerState.Idle;
        else if (isRunning)
            _state = PlayerState.Run;
        else
            _state = PlayerState.Walk;
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

    private void _HandleStamina()
    {
        switch (_state)
        {
            case PlayerState.Run:
                _stamina -= _staminaMinus * Time.deltaTime;
                _stamina = Mathf.Max(_stamina, 0f);
                _lastRunTime = Time.time;
                break;

            case PlayerState.Idle:
            case PlayerState.Walk:
                if (Time.time - _lastRunTime >= _staminaPlusDelay)
                {
                    _stamina += _staminaPlus * Time.deltaTime;
                    _stamina = Mathf.Min(_stamina, _staminaMax);
                }
                break;
        }

        if (_stamina <= 0f && _state == PlayerState.Run)
            _state = PlayerState.Walk;
    }
}

