using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DraggableGameObject : MonoBehaviour
{
    #region Actions

    public Action<Vector3> StartedDragging;
    public Action<Vector3> Dragging;
    public Action<Vector3> StoppedDragging;

    #endregion
    
    private Vector3 _mousePositionOffset;

    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown()
    {
        _mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
        StartedDragging?.Invoke(transform.position);
    }

    private void OnMouseDrag()
    {
        Dragging?.Invoke(transform.position);
        transform.position = GetMouseWorldPosition() + _mousePositionOffset;
    }

    private void OnMouseUp()
    {
        StoppedDragging?.Invoke(transform.position);
        Debug.Log("Stopped dragging");
    }
}
