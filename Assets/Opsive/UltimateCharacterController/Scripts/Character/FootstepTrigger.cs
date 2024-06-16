/// ---------------------------------------------
/// Ultimate Character Controller
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateCharacterController.Character
{
    using Opsive.UltimateCharacterController.Utility;
    using UnityEngine;

    /// <summary>
    /// Notifies the CharacterFootEffects component when the foot has collided with the ground.
    /// </summary>
    public class FootstepTrigger : MonoBehaviour
    {
        [SerializeField] PhysicsMaterial iceMaterial;
        [SerializeField] UltimateCharacterLocomotion locomotion;
        [SerializeField] bool isPeDireito;
        [SerializeField] float slidingForce = 10f; // A força a ser aplicada para simular o deslize
        private bool onIce = false; // Flag para verificar se o jogador está no gelo

        [Tooltip("Should the footprint texture be flipped?")]
        [SerializeField] protected bool m_FlipFootprint;

        public bool FlipFootprint { get { return m_FlipFootprint; } set { m_FlipFootprint = value; } }

        private Transform m_Transform;
        private CharacterFootEffects m_FootEffects;
        private CharacterLayerManager m_CharacterLayerManager;
        

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        private void Awake()
        {
            m_Transform = transform;
            m_FootEffects = GetComponentInParent<CharacterFootEffects>();
            m_CharacterLayerManager = GetComponentInParent<CharacterLayerManager>();
        }

        private bool CompareMaterialNames(string name1, string name2)
        {
            // Remove "(Instance)" do final dos nomes se existir
            name1 = name1.Replace(" (Instance)", "");
            name2 = name2.Replace(" (Instance)", "");
            return name1 == name2;
        }

        /// <summary>
        /// The trigger has collided with another object.
        /// </summary>
        /// <param name="other">The Collider that the trigger collided with.</param>
        private void OnTriggerEnter(Collider other)
        {
            // Notify the CharacterFootEffects component if the layer is valid.
            if (MathUtility.InLayerMask(other.gameObject.layer, m_CharacterLayerManager.IgnoreInvisibleCharacterWaterLayers)) {
                m_FootEffects.TriggerFootStep(m_Transform, m_FlipFootprint);
            }

            if (isPeDireito && other.material != null && CompareMaterialNames(other.material.name, iceMaterial.name))
            {
                Debug.Log("Entrou em contato com o material de gelo!");
                onIce = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (isPeDireito && other.material != null && CompareMaterialNames(other.material.name, iceMaterial.name))
            {
                Debug.Log("Saiu do contato com o material de gelo!");
                onIce = false;
            }
        }

        void FixedUpdate()
        {
            if (!isPeDireito) return;
            if (onIce)
            {
                // Aplica uma força constante para frente
                Debug.Log(" Aplica uma força constante para frente!");
                locomotion.AddForce(locomotion.transform.forward * slidingForce);
            }
        }
    }
}