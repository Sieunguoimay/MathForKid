using UnityEngine;
using UnityEngine.Events;

public abstract class ABoundsClicker : MonoBehaviour, RayPointer.IRaycastTarget
{
    [SerializeField] private UnityEvent click;
    [SerializeField] private bool selfSetup = true;
    [SerializeField] private AudioClip button;

    public UnityEvent Clicked => click;
    public abstract Bounds Bounds { get; }

    private void OnEnable()
    {
        InnerSetup();
    }

    private void OnDisable()
    {
        if (selfSetup)
        {
            SetInteractable(false);
        }
    }

    protected virtual void InnerSetup()
    {
        if (selfSetup)
        {
            SetInteractable(true);
        }
    }

    public void SetInteractable(bool interactable)
    {
        if (interactable)
        {
            RayPointer.Instance.Register(this);
        }
        else
        {
            RayPointer.Instance?.Unregister(this);
        }
    }

    private void OnDrawGizmos()
    {
        // Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawWireCube(transform.position, Bounds.size);
    }

    public void OnHit(Ray ray, float distance)
    {
        click?.Invoke();
        AudioPlayer.Instance.Play(button);
    }
}

public class BoundsClicker : ABoundsClicker
{
    [SerializeField] private Vector3 size;
    public override Bounds Bounds => new(transform.position, size);
}