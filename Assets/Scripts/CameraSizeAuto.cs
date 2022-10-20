using UnityEngine;

public class CameraSizeAuto : MonoBehaviour
{
    [SerializeField] public Camera cam;

    public void Resize(float width)
    {
        // var width = cam.orthographicSize * Screen.width / Screen.height;
        cam.orthographicSize = width * Screen.height / Screen.width;
    }
}