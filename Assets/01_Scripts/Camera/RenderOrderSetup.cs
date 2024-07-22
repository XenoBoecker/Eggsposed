using UnityEngine;
using UnityEngine.VFX;

public class RenderOrderSetup : MonoBehaviour
{
    public Camera mainCamera;
    public Camera overlayCamera;
    public VisualEffect visualEffect;
    public Canvas canvas;

    void Start()
    {
        // Set the main camera to render everything except the canvas and Visual Effect layers
        mainCamera.cullingMask = ~(1 << LayerMask.NameToLayer("Canvas") | 1 << LayerMask.NameToLayer("VisualEffect"));

        // Set the overlay camera to render only the canvas and Visual Effect layers
        overlayCamera.cullingMask = (1 << LayerMask.NameToLayer("Canvas") | 1 << LayerMask.NameToLayer("VisualEffect"));

        // Set the overlay camera to render after the main camera
        overlayCamera.depth = mainCamera.depth + 1;

        // Ensure the Visual Effect is rendered in front of the canvas
        visualEffect.GetComponent<Renderer>().sortingOrder = 1;

        // Ensure the canvas is rendered in the overlay camera
        canvas.renderMode = RenderMode.WorldSpace;
    }
}
