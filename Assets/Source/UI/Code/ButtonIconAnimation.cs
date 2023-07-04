using Source.Code.Common.Animations;
using UnityEngine;

namespace Source.UI.Code
{
    public class ButtonIconAnimation : MonoBehaviour
    {
        [SerializeField] private Transform rotatingObject;
        [SerializeField] private float timeInSeconds;
        [SerializeField] private Vector3Int magnitude;
    
        public void OnButtonClick()
        {
            StartCoroutine(RotationCoroutines.Rotate(
                rotatingObject, 
                Quaternion.Euler(magnitude.x, magnitude.y, magnitude.z),
                timeInSeconds
            ));
        }
    }
}