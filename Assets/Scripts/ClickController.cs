using System;
using UnityEngine;

public class ClickController : MonoBehaviour
{
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (_camera is null) return;
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                hit.collider.GetComponent<IClickTarget>().Click();
            }
        }
    }
}

public interface IClickTarget
{
    void Click();
}