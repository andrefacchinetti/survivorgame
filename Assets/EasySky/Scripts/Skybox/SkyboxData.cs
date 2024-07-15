//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasySky.Skybox
{
    [CreateAssetMenu(fileName = "SkyboxData", menuName = "Assets/SkyboxData")]
    public class SkyboxData : ScriptableObject
    {
        #region Public Variables
        public List<SkyData> data;
        public int selectedData;
        #endregion

        #region Private Variables
        [SerializeField] private VisualTreeAsset _skyboxLabel;
        #endregion

        #region Properties
        public VisualTreeAsset SkyboxLabel { get => _skyboxLabel; }
        #endregion
    }

    [Serializable]
    public struct SkyData
    {
        public string guid;
        public AnimationCurve animationCurve;
        public Cubemap cubemap;

        public SkyData(string guid, AnimationCurve animationCurve, Cubemap cubemap)
        {
            this.guid = guid;
            this.animationCurve = animationCurve;
            this.cubemap = cubemap;
        }
    }
}