using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // Assign your cursor texture in Inspector
    public Vector2 hotSpot = Vector2.zero; // Adjust if needed

    void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }
}
