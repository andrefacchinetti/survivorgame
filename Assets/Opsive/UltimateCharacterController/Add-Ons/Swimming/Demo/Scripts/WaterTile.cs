using UnityEngine;

/// <summary>
/// Script created by Unity within their Standard Assets package.
/// </summary>
namespace Opsive.UltimateCharacterController.AddOns.Swimming.Demo
{
    using Camera = UnityEngine.Camera;

    [ExecuteInEditMode]
    public class WaterTile : MonoBehaviour
    {
        public WaterBase waterBase;

        public void Start()
        {
            AcquireComponents();
        }

        private void AcquireComponents()
        {
            if (!waterBase) {
                if (transform.parent)
                    waterBase = transform.parent.GetComponent<WaterBase>();
                else
                    waterBase = transform.GetComponent<WaterBase>();
            }
        }

#if UNITY_EDITOR
        public void Update()
        {
            AcquireComponents();
        }
#endif

        public void OnWillRenderObject()
        {
            if (waterBase)
                waterBase.WaterTileBeingRendered(transform, Camera.current);
        }
    }
}