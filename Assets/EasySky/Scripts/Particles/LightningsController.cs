//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System.Collections.Generic;
using UnityEngine;

namespace EasySky.Particles
{
    /// <summary>
    /// Controls the lightning appearance
    /// </summary>
    public class LightningsController : MonoBehaviour
    {
        #region Private Variables
        [SerializeField] private LightningEffect _lightningPrefab;

        private List<LightningEffect> _lightningEffects = new List<LightningEffect>();
        private bool _shouldUpdate;
        private bool _wasSpawned;
        private float _interval;
        private bool _isLightningEnabled;
        private float _frequency;
        #endregion

        #region Unity Methods
        private void Update()
        {
            if (!_isLightningEnabled)
            {
                return;
            }

            if (_shouldUpdate)
            {
                _shouldUpdate = false;
                foreach (var element in _lightningEffects)
                {
                    if (element.EffectTime > 0)
                    {
                        element.EffectTime -= Time.deltaTime;
                        _shouldUpdate = true;
                    }
                }
            }

            if (_interval <= 0)
            {
                _interval = Random.Range(0, Mathf.Abs(_frequency - 1) * 250);
                _wasSpawned = false;
                foreach (var element in _lightningEffects)
                {
                    if (element.EffectTime <= 0)
                    {
                        element.Play();
                        element.EffectTime = 5;
                        _wasSpawned = true;
                        _shouldUpdate = true;
                        break;
                    }
                }

                if (!_wasSpawned)
                {
                    var lightning = Instantiate(_lightningPrefab, transform);
                    _lightningEffects.Add(lightning);
                    lightning.Play();
                    lightning.EffectTime = 5;
                    _shouldUpdate = true;
                }
            }

            _interval -= Time.deltaTime;
        }
        #endregion

        #region Public Methods
        public void ApplyData(bool isEnabled, float frequency)
        {
            _isLightningEnabled = isEnabled;
            _frequency = frequency;
            _interval = Random.Range(0, Mathf.Abs(_frequency - 1) * 250);
        }
        #endregion
    }
}