using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData itemData;

    private Camera _cam;
    private MeshRenderer _renderer;

    void Start()
    {
        _cam = Camera.main;
        _renderer = GetComponent<MeshRenderer>();
        _renderer.material = itemData.Material;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + _cam.transform.forward);
    }
}
