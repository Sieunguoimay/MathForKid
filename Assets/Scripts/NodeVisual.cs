using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class NodeVisual : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private TextMeshPro text;
    [SerializeField] private Material materialSelectable;
    [SerializeField] private Material materialPositive;
    [SerializeField] private Material materialNegative;
    [SerializeField] private Material materialInactive;

    private bool _isSelectable;

    [field: System.NonSerialized] public Node Node { get; private set; }

    public void Setup(Node node)
    {
        Node = node;

        //Display text
        text.text = $"{Node.Number}";
    }

    public void SetSelectable(bool selectable)
    {
        _isSelectable = selectable;
        ShowSelectable(_isSelectable);
    }

    private void Awake()
    {
        Reset();
    }

    public void Click()
    {
        PlayBounceAnim();

        if (_isSelectable && !Node.Selector.Selected)
        {
            Node.Selector?.Select(Node);
            ShowResult(Node?.Condition.IsCorrect() ?? false);
        }
    }

    public void ShowResult(bool result)
    {
        meshRenderer.sharedMaterial = result ? materialPositive : materialNegative;
    }


    public void ShowSelectable(bool selectable)
    {
        meshRenderer.sharedMaterial = selectable ? materialSelectable : materialInactive;
    }

    public void Reset()
    {
        meshRenderer.sharedMaterial = materialInactive;
        _isSelectable = false;
    }

    public void PlayBounceAnim()
    {
        meshRenderer.transform.DOPunchScale(Vector3.one * 0.1f, .5f);
    }
}