using System.Collections.Generic;
using UnityEngine;

public class RectanglesController : MonoBehaviour
{
    [SerializeField]
    private GameObject rectanglePrefab;

    // To reuse the created squares
    private static Stack<GameObject> disabledRectangles = new Stack<GameObject>();

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CreateRectangle(cam.ScreenToWorldPoint(Input.mousePosition), rectanglePrefab);
        }
    }

    public static void DeleteRectangle(GameObject rectangle)
    {
        rectangle.SetActive(false);
        disabledRectangles.Push(rectangle);
    }

    private bool CreateRectangle(Vector3 position, GameObject rectanglePrefab)
    {
        GameObject rectangle;

        //Check if there is enough space
        if (Physics2D.BoxCast(position, rectanglePrefab.transform.lossyScale, rectanglePrefab.transform.rotation.z, Vector2.zero))
            return false;

        position.z = 0f;
        if (disabledRectangles.Count > 0)
        {
            rectangle = disabledRectangles.Pop();
            rectangle.transform.position = position;
            rectangle.SetActive(true);
        }
        else
        {
            rectangle = Instantiate(rectanglePrefab, position, rectanglePrefab.transform.rotation);
            rectangle.transform.parent = gameObject.transform;
            rectangle.name = $"Rectangle{Rectangle.RectaglesCount}";
        }
        //Random color
        rectangle.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
        return true;
    }
}
