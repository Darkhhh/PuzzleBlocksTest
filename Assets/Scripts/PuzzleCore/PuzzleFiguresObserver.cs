using UnityEngine;

public class PuzzleFiguresObserver : MonoBehaviour
{
    private Vector3 _offset;

    [SerializeField] private Camera sceneCamera;
    [SerializeField] private LayerMask movableLayers;

    private PuzzleFigure[] _activeFigures;
    private PuzzleFigure _capturedFigure;
    private bool _dragging;
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var mousePosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            
            var hit = Physics2D.Raycast(mousePosition, Vector2.zero, float.PositiveInfinity, 
                movableLayers);
            
            if (hit)
            {
                _dragging = true;
                CaptureFigure(hit.transform);
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            _capturedFigure = null;
            _dragging = false;
        }

        if (_dragging) MoveFigure();
    }

    private void CaptureFigure(Transform hit)
    {
        foreach (var activeFigure in _activeFigures)
        {
            if (activeFigure.transform != hit) continue;
            _capturedFigure = activeFigure;
            _offset = _capturedFigure.transform.position - sceneCamera.ScreenToWorldPoint(Input.mousePosition);
            break;
        }
                
        // _puzzleGrid.SetActivePuzzleFigure(_capturedFigure);
    }

    private void MoveFigure()
    {
        _capturedFigure.transform.position = sceneCamera.ScreenToWorldPoint(Input.mousePosition) + _offset;
    }
}
