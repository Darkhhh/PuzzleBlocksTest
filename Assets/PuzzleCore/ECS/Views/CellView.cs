using System;
using DG.Tweening;
using UnityEngine;

namespace PuzzleCore.ECS.Views
{
    public class CellView : MonoBehaviour
    {
        public enum CellState : byte
        {
            Default, Suggested, Highlighted, Occupied, Destroyable, Targeted
        }
        
        
        private SpriteRenderer _renderer;
        private GameObject _puzzleBlock = null;
        private GameObject _target = null;
        private CellState _currentState = CellState.Default;
        private CellState _lastState = CellState.Default;
        #region Constants

        public const float Size = 1f;

        #endregion
    
        #region Colors

        private static readonly Color AvailableColor = Color.white;
        private static readonly Color HighlightedColor = Color.green;
        private static readonly Color UnAvailableColor = Color.gray;
        private static readonly Color SuggestionColor = Color.cyan;

        #endregion
    
        public Vector3 ParentPosition => _parent.transform.position;
        public bool Suggested { get; private set; }
        private Transform _parent;

        public void Init()
        {
            _renderer = GetComponent<SpriteRenderer>();
            _parent = transform.parent;
        }

        public void InjectPuzzleBlock(GameObject puzzleBlock)
        {
            var thisTransform = transform;
            
            puzzleBlock.transform.position = thisTransform.position;
            puzzleBlock.transform.parent = thisTransform;
            puzzleBlock.SetActive(false);
            
            _puzzleBlock = puzzleBlock;
        }

        public void InjectTarget(GameObject target)
        {
            var thisTransform = transform;
            
            target.transform.position = thisTransform.position;
            target.transform.parent = thisTransform;
            target.SetActive(false);
            _target = target;
        }
        
        #region Deprecated

        public void SetAvailable(bool available)
        {
            // if (available) _puzzleBlock.SetActive(false);
            // _puzzleBlock.transform.position -= new Vector3(0, 0.1f, 0);
            _target.SetActive(false);
            SetColor(AvailableColor);
        }

        public void SetDestroyable(bool available)
        {
            if (available) _puzzleBlock.SetActive(true);
            _puzzleBlock.transform.position += new Vector3(0, 0.1f, 0);
        }

        public void SetHighlighted()
        {
            SetColor(HighlightedColor);
        }

        public void SetSuggestion()
        {
            Suggested = true;
            SetColor(SuggestionColor);
        }

        public void SetTarget()
        {
            _target.SetActive(true);
        }

        public void SetSimple()
        {
            _puzzleBlock.SetActive(false);
            Suggested = false;
            SetColor(AvailableColor);
        }

        public void SetUnAvailable()
        {
            _puzzleBlock.SetActive(true);
            Suggested = false;
            SetColor(UnAvailableColor);
        }
    
        private void SetColor(Color color)
        {
            _renderer.color = color;
        }

        #endregion


        public void ChangeState(CellState newState)
        {
            switch (_currentState)
            {
                case CellState.Default:
                {
                    _lastState = CellState.Default;
                    _currentState = newState;
                    SetState(newState);
                }
                    break;
                case CellState.Destroyable:
                {
                    if (newState != CellState.Default) return;
                    _lastState = CellState.Destroyable;
                    _currentState = newState;
                    SetState(newState);
                }
                    break;
                case CellState.Suggested:
                {
                    if (newState is not (CellState.Default or CellState.Destroyable or CellState.Occupied)) return;
                    _lastState = CellState.Suggested;
                    _currentState = newState;
                    SetState(newState);
                }
                    break;
                case CellState.Highlighted:
                {
                    if (newState is not (CellState.Default or CellState.Destroyable or CellState.Occupied)) return;
                    _lastState = CellState.Highlighted;
                    _currentState = newState;
                    SetState(newState);
                }
                    break;
                case CellState.Occupied:
                {
                    if (newState is not (CellState.Default or CellState.Destroyable or CellState.Targeted)) break;
                    _lastState = CellState.Occupied;
                    _currentState = newState;
                    SetState(newState);
                }
                    break;
                case CellState.Targeted:
                {
                    if (newState is not (CellState.Default or CellState.Occupied)) return;
                    _lastState = CellState.Targeted;
                    _currentState = newState;
                    SetState(newState);
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetState(CellState state)
        {
            switch (state)
            {
                case CellState.Default:
                {
                    _puzzleBlock.SetActive(false);
                    _target.SetActive(false);
                    SetColor(AvailableColor);
                }
                    break;
                case CellState.Suggested:
                {
                    _puzzleBlock.SetActive(false);
                    _target.SetActive(false);
                    SetColor(SuggestionColor);
                }
                    break;
                case CellState.Highlighted:
                {
                    SetColor(HighlightedColor);
                }
                    break;
                case CellState.Occupied:
                {
                    _puzzleBlock.SetActive(true);
                    _puzzleBlock.transform.DOScale(new Vector3(1f, 1f, 1f), 0.1f);
                    _target.SetActive(false);
                    //SetColor(AvailableColor);
                }
                    break;
                case CellState.Destroyable:
                {
                    _puzzleBlock.transform.DOScale(new Vector3(.85f, .85f, .85f), 0.1f);
                }
                    break;
                case CellState.Targeted:
                {
                    _target.SetActive(true);
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }
    }
}
