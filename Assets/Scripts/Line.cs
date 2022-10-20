using System;
using UnityEngine;

public class Line : MonoBehaviour
{
    [SerializeField] private Material colorNormal;
    [SerializeField] private Material colorActive;
    [SerializeField] private MeshRenderer meshRenderer;
    [field: NonSerialized] public NodeVisual Node1 { get; set; }
    [field: NonSerialized] public NodeVisual Node2 { get; set; }

    private void Start()
    {
        SetCorrect(false);
    }

    public void SetEndPoints(Vector3 a, Vector3 b)
    {
        var midPoint = (a + b) / 2f;
        transform.position = midPoint;
        transform.rotation = Quaternion.LookRotation(a - b);
        var scale = transform.localScale;
        scale.z = (a - b).magnitude;
    }

    public void SetCorrect(bool correct)
    {
        meshRenderer.sharedMaterial = correct ? colorActive : colorNormal;
    }
}