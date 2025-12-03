using UnityEngine;

public class DisappearOnApproach : MonoBehaviour
{
    // The main VR camera (usually the head-mounted display)
    // Drag your main camera/XR rig head object here in the Inspector.
    public Transform vrCamera;

    // The maximum distance before the object disappears.
    // Adjust this value in the Inspector to fine-tune the effect.
    public float maxDistanceBeforeDisappear = 0.5f;

    private Renderer objectRenderer;
    private Collider objectCollider;

    void Start()
    {
        // Get the Renderer and Collider components of this object.
        // We will disable these to make the object "disappear" and prevent interaction.
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();

        // IMPORTANT: Check if the camera reference is set.
        if (vrCamera == null)
        {
            Debug.LogError("VR Camera Transform is not set on " + gameObject.name + "! Please assign it in the Inspector.");
            // Try to find the main camera if it's null (might not work reliably in all VR setups)
            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                vrCamera = mainCam.transform;
            }
        }
    }

    void Update()
    {
        if (vrCamera == null)
        {
            return; // Can't proceed without a camera reference.
        }

        // Calculate the distance between this object and the VR camera.
        float distance = Vector3.Distance(transform.position, vrCamera.position);

        // Check if the distance is less than the disappearance threshold.
        if (distance <= maxDistanceBeforeDisappear)
        {
            // If close enough, make the object disappear (disable Renderer and Collider).
            SetObjectVisibility(false);
        }
        else
        {
            // If far enough, make sure the object is visible.
            SetObjectVisibility(true);
        }
    }

    /// <summary>
    /// Helper method to control the visibility and collision of the object.
    /// </summary>
    /// <param name="isVisible">True to show the object, False to hide it.</param>
    void SetObjectVisibility(bool isVisible)
    {
        if (objectRenderer != null)
        {
            objectRenderer.enabled = isVisible;
        }

        if (objectCollider != null)
        {
            // Disabling the collider prevents the player from hitting the invisible object.
            objectCollider.enabled = isVisible;
        }

        // You could also disable the entire GameObject:
        // gameObject.SetActive(isVisible);
    }
}