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
    [SerializeField] private Inventory _inventory;

    // State
    [Header("State")]
    [SerializeField] private PlayerState _state = PlayerState.Idle;

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

    // Stamina
    [Header("Stamina")]
    [SerializeField] private int _hp;
    [SerializeField] private float _stamina;
    private float _staminaMax;
    private float _staminaMinus;
    private float _staminaPlus;
    private float _staminaPlusDelay;
    private float _lastRunTime = -999.0f;

    // Mentality
    [Header("State")]
    [SerializeField] private float _mentality;
    private float _mentalityRange;
    private int _mentalityDebuff;

    [Header("Inventory")]
    [SerializeField] private List<ItemData> _itemDatas = new List<ItemData>();

    [Header("Others")]
    [SerializeField] private float _pickUpDistance = 1.0f;
    [SerializeField] private ItemObject _nearbyItem;


    void Start()
    {
        _camera = Camera.main;
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _ApplySettings();
    }

    void Update()
    {
        _Move();
        _Rotate();
        _HandleStamina();

        // About Items
        _CheckNearbyItem();
        if (_nearbyItem != null && Input.GetKeyDown(KeyCode.F))
        {
            _itemDatas.Add(_nearbyItem.itemData);
            _inventory.PickUpItem(_nearbyItem);
            _nearbyItem = null;
        }

        _HandleItemUseInput();
    }

    private void _ApplySettings()
    {
        var settings = PlayerSettings.Instance;
        _hp = settings.Hp;
        _walkSpeed = settings.WalkSpeed;
        _runSpeed = settings.RunSpeed;
        _staminaMax = settings.StaminaMax;
        _staminaMinus = settings.StaminaMinus;
        _staminaPlus = settings.StaminaPlus;
        _staminaPlusDelay = settings.StaminaPlusDelay;
        _stamina = _staminaMax;
        _mentality = settings.Mentality;
        _mentalityRange = settings.MentalityRange;
        _mentalityDebuff = settings.MentalityDebuff;
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

    private void _CheckNearbyItem()
    {
        ItemObject[] items = FindObjectsOfType<ItemObject>();
        float closestDist = _pickUpDistance;
        _nearbyItem = null;

        foreach (var item in items)
        {
            float dist = Vector3.Distance(transform.position, item.transform.position);
            if (dist <= closestDist)
            {
                closestDist = dist;
                _nearbyItem = item;
                Debug.Log($"[System] Near By Item : {item.name}");
            }
        }
    }

    private void _HandleItemUseInput()
    {
        for (int i = 0; i < Mathf.Min(9, _itemDatas.Count); i++)
        {
            KeyCode key = KeyCode.Alpha1 + i; // Alpha1, Alpha2, ...
            if (Input.GetKeyDown(key))
            {
                _inventory.UseItem(_itemDatas, i);
                _itemDatas.RemoveAt(i);
                break;
            }
        }
    }

    public int Hp => _hp;
    public float Stamina => _stamina;
    public float Mentality => _mentality;
    public List<ItemData> ItemDatas => _itemDatas;
}

