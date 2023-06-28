using System;
using System.Collections.Generic;
using System.Linq;
using Temp.Views;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Temp.Mono
{
    public class PowerUpsHandler : MonoBehaviour
    {
        #region Serialize Fields

        [SerializeField] private PooledPowerUp[] powerUps;

        #endregion

        #region Weights

        private float[] _powerUpsWeights;
        private float _allWeight;

        #endregion

        #region Dictionaries

        private Dictionary<PowerUpType, PowerUpView> _typesToViews;
        private Dictionary<PowerUpType, Queue<PowerUpView>> _powerUps;

        #endregion
        
        
        public void Init()
        {
            _powerUps = new Dictionary<PowerUpType, Queue<PowerUpView>>(powerUps.Length);
            _typesToViews = new Dictionary<PowerUpType, PowerUpView>(powerUps.Length);
            foreach (var pooledPowerUp in powerUps)
            {
                var q = new Queue<PowerUpView>(pooledPowerUp.initialNumber);
                for (var i = 0; i < pooledPowerUp.initialNumber; i++)
                {
                    var item = Instantiate(pooledPowerUp.view, transform);
                    item.gameObject.SetActive(false);
                    q.Enqueue(item);
                }
                
                _powerUps.Add(pooledPowerUp.view.type, q);
                _typesToViews.Add(pooledPowerUp.view.type, pooledPowerUp.view);
            }
            
            // Counting Weights
            _allWeight = powerUps.Sum(powerUp => powerUp.view.weight);
            _powerUpsWeights = new float[powerUps.Length + 1];
            _powerUpsWeights[0] = 0;
            for (var i = 1; i < powerUps.Length + 1; i++)
            {
                _powerUpsWeights[i] = _powerUpsWeights[i - 1] + powerUps[i - 1].view.weight;
            }
        }


        public PowerUpType GetRandomPowerUpType()
        {
            var index = GetRandomPowerUpIndex();

            for (var i = 0; i < powerUps.Length; i++)
            {
                if (i == index) return powerUps[i].view.type;
            }
            
            return PowerUpType.Coin;
        }
        
        public PowerUpView GetPowerUp(PowerUpType type)
        {
            var res = !_powerUps[type].TryDequeue(out var result)
                ? Instantiate(_typesToViews[type])
                : result;
            res.gameObject.SetActive(true);
            return res;
        }

        public void ReturnPowerUp(PowerUpView powerUpView)
        {
            powerUpView.gameObject.SetActive(false);
            powerUpView.transform.parent = transform;
            _powerUps[powerUpView.type].Enqueue(powerUpView);
        }
        
        private int GetRandomPowerUpIndex()
        {
            var weight = Random.Range(float.Epsilon, _allWeight - float.Epsilon);

            for (var i = 1; i < _powerUpsWeights.Length; i++)
            {
                if (weight > _powerUpsWeights[i - 1] && weight <= _powerUpsWeights[i])
                {
                    return i - 1;
                }
            }

            throw new Exception("Incorrect weight array or weight generation");
        }
    }

    [Serializable]
    public class PooledPowerUp
    {
        public PowerUpView view;

        public int initialNumber;
    }
}