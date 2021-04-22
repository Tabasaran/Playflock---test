using UnityEngine;

public class Binder : MonoBehaviour
{
    private static int bindersCount = 0;
    public static int BindersCount { get => bindersCount++; }

    private LineRenderer line;

    public Transform Point1 { get; set; }
    public Transform Point2 { get; set; }

    private void Start()
    {
        line = GetComponent<LineRenderer>();
    }

    public void UpdateLine()
    {
        if (Point1 == null || Point2 == null 
            || Point1.gameObject.activeSelf == false 
            || Point2.gameObject.activeSelf == false)
        {
            BindersController.DeleteBinder(gameObject);
            return;
        }

        line.SetPositions(new Vector3[] { Point1.position, Point2.position });
    }
}
