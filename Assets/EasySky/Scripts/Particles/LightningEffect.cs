//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace EasySky.Particles
{
    public class LightningEffect : MonoBehaviour
    {
        #region Constants
        private const float MIN_LIGHTNING_HEIGHT = 9f;
        private const float MAX_LIGHTNING_HEIGHT = 20f;
        #endregion

        #region Private Variables
        [SerializeField] private List<Transform> _points;
        [SerializeField] private VisualEffect _visualEffect;
        #endregion

        #region Properties
        public float EffectTime { get; set; }
        #endregion

        #region Public Methods
        public void Play()
        {
            RandomizePoints();
            RandomizePosition();
            _visualEffect.Play();
        }
        #endregion

        #region Private Methods
        private void RandomizePosition()
        {
            var camera = Camera.main.transform;

            var random = Random.insideUnitCircle * 100;
            var height = 0f;

            transform.position = new Vector3(camera.position.x + random.x, height, camera.position.z + random.y);

            if (Physics.Raycast(transform.position + new Vector3(0,1000,0), transform.TransformDirection(Vector3.down), out var hit, Mathf.Infinity))
            {
                if (hit.point != null)
                {
                    height = hit.point.y;
                }
            }

            transform.position = new Vector3(camera.position.x + random.x, height, camera.position.z + random.y);
        }

        private void RandomizePoints()
        {
            foreach (var point in _points)
            {
                var random = Random.insideUnitCircle;

                var pos = random * MIN_LIGHTNING_HEIGHT;

                point.localPosition = new Vector3(pos.x, point.localPosition.y, pos.y);
            }

            LightningScaler();
        }

        private void LightningScaler()
        {
            var randomHeight = Random.Range(MIN_LIGHTNING_HEIGHT, MAX_LIGHTNING_HEIGHT);
            for (int i = 0; i < _points.Count; i++)
            {
                _points[i].localPosition = new Vector3(_points[i].localPosition.x, randomHeight * i, _points[i].localPosition.z);
            }
        }
        #endregion
    }
}