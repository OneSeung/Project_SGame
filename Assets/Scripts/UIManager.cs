using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameObject _tablet;

    private void Awake()
    {
        _tablet = transform.Find("Tablet").gameObject;
        _tablet.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _tablet.SetActive(!_tablet.activeSelf);
        }
    }
}
