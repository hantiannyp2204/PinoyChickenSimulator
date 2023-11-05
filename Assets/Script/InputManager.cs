using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public Camera mainCam;

    private Vector3 lastPosiiton;

    public LayerMask placableArea;

    public event Action OnClicked, OnExit;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnClicked?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnExit?.Invoke();
        }
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();
    public Vector3 getSelectedMapPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mainCam.nearClipPlane; // make sure ONLY objects that are rendered can be selected
        Ray ray = mainCam.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if(Physics.Raycast(ray,out hit, 100, placableArea))
        {
            lastPosiiton = hit.point;
        }
        return lastPosiiton;


    }
}
