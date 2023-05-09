using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    #region Contants

    public const float Size = 1f;

    #endregion
    
    
    #region Colors

    public static Color AvailableColor = Color.white;
    public static Color HighlightedColor = Color.green;
    public static Color UnAvailableColor = Color.gray;

    #endregion


    #region Properties

    public Vector3 Position => gameObject.transform.position;

    public Vector3 RelevantPosition => _position;

    public Vector3 ParentPosition => _parent.transform.position;
    
    public bool Available { get; set; } = true;

    public bool ShouldBeCleared { get; set; } = false;

    public bool OrderedForPlacement { get; set; } = false;

    public bool AnchorBlockPlacement { get; set; } = false;

    #endregion


    #region Private Values

    private SpriteRenderer _renderer;
    private Transform _parent;
    private Vector3 _position;

    #endregion
    

    

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _parent = transform.parent;
        _position = _parent.transform.position + gameObject.transform.position;
    }

    public void ChangeColor(bool t)
    {
        _renderer.color = t ? Color.green: Color.white;
    }

    public void SetBlock()
    {
        _renderer.color = Color.gray;
    }

    public void SetColor(Color color)
    {
        _renderer.color = color;
    }

    public void Clear()
    {
        ShouldBeCleared = false;
        Available = true;
        AnchorBlockPlacement = false;
        OrderedForPlacement = false;
        SetColor(AvailableColor);
    }
}
