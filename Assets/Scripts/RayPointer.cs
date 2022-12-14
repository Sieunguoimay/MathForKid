using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class RayPointer : Singleton<RayPointer>
{
    private readonly List<IRaycastTarget> _listeners = new();
    private Camera _camera;
    
    public bool Interactable { get; set; }

    private void Start()
    {
        SetCamera(Camera.main);
        Interactable = true;
    }

    public void SetCamera(Camera cam)
    {
        _camera = cam;
    }

    public void Reset()
    {
        _listeners.Clear();
        Interactable = true;
    }

    public void Register(IRaycastTarget target)
    {
        _listeners.Add(target);
    }

    public void Unregister(IRaycastTarget target)
    {
        _listeners.Remove(target);
    }

    public void Update()
    {
        if (!Interactable) return;
        if (Input.GetMouseButtonUp(0))
        {
            ProcessMouse(Input.mousePosition);
        }
    }

    private void ProcessMouse(Vector3 position)
    {
        var ray = _camera.ScreenPointToRay(position);

        var minDistance = float.MaxValue;
        IRaycastTarget selectedTarget = null;

        for (var i = 0; i < _listeners.Count; i++)
        {
            var l = _listeners[i];
            if (!l.Bounds.IntersectRay(ray, out var distance)) continue;
            if (minDistance > distance)
            {
                minDistance = distance;
                selectedTarget = l;
            }
        }

        selectedTarget?.OnHit(ray, minDistance);
    }

    public interface IRaycastTarget
    {
        Bounds Bounds { get; }
        void OnHit(Ray ray, float distance);
    }

}