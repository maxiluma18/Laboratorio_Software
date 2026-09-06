using UnityEngine;
using UnityEngine.InputSystem; 

[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour
{
    [Header("Configuración de Zoom")]
    public float zoomSpeed = 0.05f;
    public float minZoom = 20f;
    public float maxZoom = 80f;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Verificamos que haya un mouse detectado por el sistema
        if (Mouse.current == null) return;

        // Leemos el eje Y de la rueda del mouse
        float scrollData = Mouse.current.scroll.y.ReadValue();

        if (scrollData != 0f)
        {
            if (cam.orthographic)
            {
                // Zoom para cámara 2D
                cam.orthographicSize -= scrollData * zoomSpeed;
                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
            }
            else
            {
                // Zoom para cámara 3D
                cam.fieldOfView -= scrollData * zoomSpeed;
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, minZoom, maxZoom);
            }
        }
    }
}