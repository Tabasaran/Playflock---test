using System;
using UnityEngine;

public class Rectangle : MonoBehaviour
{
    private static int rectaglesCount = 0;
    public static int RectaglesCount { get => rectaglesCount++; }

    [SerializeField]
    public float maxWaitSecondTapTime = 1f;
    private float waitSecondTapTime = 0f;
    
    public event Action OnMoved;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void OnMouseDrag()
    {
        MoveRectangle();
    }

    private void MoveRectangle()
    {
        var position = cam.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0f;

        gameObject.SetActive(false);// Disable for correct casting
        if (Physics2D.BoxCast(position, transform.lossyScale, transform.rotation.z, Vector2.zero))
        {
            gameObject.SetActive(true);
            return;
        }
        gameObject.SetActive(true);

        transform.position = position;
        OnMoved?.Invoke();
    }

    private void OnMouseUp()
    {
        if (CheckDoubleTap())
        {
            RectanglesController.DeleteRectangle(gameObject);
            OnMoved?.Invoke();
            OnMoved = null;// Remove binders
        }
    }

    private bool CheckDoubleTap()
    {
        var isDoubleTap = Time.time <= waitSecondTapTime;
        waitSecondTapTime = Time.time + maxWaitSecondTapTime;
        return isDoubleTap; 
    }
}
