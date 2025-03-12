using UnityEngine;

public class ImageHoverScale : MonoBehaviour
{
    public float scaleFactor = 1.2f; // How much to enlarge the object
    private Vector3 originalScale;

    void Awake()
    {
        originalScale = transform.localScale;
    }

    void OnEnable()
    {
        transform.localScale = originalScale; // Reset scale when enabled
    }

    void OnMouseEnter()
    {
        transform.localScale = originalScale * scaleFactor;
    }

    void OnMouseExit()
    {
        transform.localScale = originalScale;
    }
}
