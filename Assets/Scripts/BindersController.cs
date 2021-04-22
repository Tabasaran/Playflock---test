using System;
using System.Collections.Generic;
using UnityEngine;

public class BindersController : MonoBehaviour
{
    // To reuse the created binders
    private static Queue<GameObject> disabledBinders = new Queue<GameObject>();

    [SerializeField]
    private GameObject binderPrefab;
    private Binder newBinder;

    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (CheckClickOnRectangle(out Rectangle rectangle))
            {
                newBinder = CreateBinder().GetComponent<Binder>();

                rectangle.OnMoved += newBinder.UpdateLine;
                newBinder.Point1 = rectangle.transform;
            }
        }
        else if (Input.GetMouseButtonUp(1) && newBinder != null)
        {
            if (CheckClickOnRectangle(out Rectangle rectangle))
            {
                rectangle.OnMoved += newBinder.UpdateLine;
                newBinder.Point2 = rectangle.transform;

                newBinder.gameObject.SetActive(true);
                newBinder.UpdateLine();
            }
            else if (newBinder != null)
            {
                DeleteBinder(newBinder.gameObject);
                newBinder = null;
            }
        }
    }

    private bool CheckClickOnRectangle(out Rectangle rectangle)
    {
        var hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.transform != null && hit.transform.TryGetComponent(out rectangle))
            return true;

        rectangle = null;
        return false;
    }

    private GameObject CreateBinder()
    {
        if (disabledBinders.Count > 0)
            return disabledBinders.Dequeue();

        var binder = Instantiate(binderPrefab, transform);
        binder.name = $"Binder{Binder.BindersCount}";
        return binder;
    }

    public static void DeleteBinder(GameObject binder)
    {
        if (disabledBinders.Contains(binder))
            return;

        binder.SetActive(false);
        disabledBinders.Enqueue(binder);
    }
}
