using System;
using System.Collections.Generic;
using UnityEngine;

public class BindersController : MonoBehaviour
{
    // To reuse the created binders
    private static Queue<Binder> disabledBinders = new Queue<Binder>();

    private static List<Binder> binders = new List<Binder>();

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
                newBinder = CreateBinder();

                rectangle.OnMoved += newBinder.UpdateLine;
                newBinder.Point1 = rectangle.transform;
            }
        }
        else if (Input.GetMouseButtonUp(1) && newBinder != null)
        {
            if (CheckClickOnRectangle(out Rectangle rectangle))
            {
                if (DeleteBinder(rectangle))
                    return;

                rectangle.OnMoved += newBinder.UpdateLine;
                newBinder.Point2 = rectangle.transform;

                newBinder.gameObject.SetActive(true);
                newBinder.UpdateLine();
            }
            else if (newBinder != null)
            {
                DeleteBinder(newBinder);
                newBinder = null;
            }
        }
    }

    private bool DeleteBinder(Rectangle rectangle)
    {
        foreach (var binder in binders)
        {
            if (binder.Point1 == rectangle.transform && binder.Point2 == newBinder.Point1
             || binder.Point1 == newBinder.Point1 && binder.Point2 == rectangle.transform)
            {
                DeleteBinder(binder);

                DeleteBinder(newBinder);
                newBinder = null;
                return true;
            }
        }
        return false;
    }

    private bool CheckClickOnRectangle(out Rectangle rectangle)
    {
        var hit = Physics2D.Raycast(cam.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.transform != null && hit.transform.TryGetComponent(out rectangle))
            return true;

        rectangle = null;
        return false;
    }

    private Binder CreateBinder()
    {
        if (disabledBinders.Count > 0)
            return disabledBinders.Dequeue();

        var binder = Instantiate(binderPrefab, transform).GetComponent<Binder>();
        binders.Add(binder);
        binder.name = $"Binder{Binder.BindersCount}";
        return binder;
    }

    public static void DeleteBinder(Binder binder)
    {
        if (disabledBinders.Contains(binder))
            return;

        binder.Point1 = null;
        binder.Point2 = null;
        binder.gameObject.SetActive(false);
        disabledBinders.Enqueue(binder);
    }
}
