//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace EasySky.Utils
{
    [InitializeOnLoad]
    public class EasySkyRelativePath
    {
        private const int kPathDiscardCharacterCount = 37;

        static EasySkyRelativePath()
        {
            EditorApplication.projectChanged += UpdatePath;
        }

        /// <summary>
        /// Updates the path of the EasySky folder every time the project has been changed
        /// </summary>
        public static void UpdatePath()
        {
            string[] assets = AssetDatabase.FindAssets("EasySkyRelativePath t:Script");
            if (assets.Length == 1)
            {
                string str = AssetDatabase.GUIDToAssetPath(assets[0]);
                str = str.Remove(str.Length - kPathDiscardCharacterCount);
                if (EasySkyWeatherManager.Instance)
                {
                    EasySkyWeatherManager.Instance.RelativePath = str;
                }
            }
            else
            {
                Debug.LogError("Could not determine EasySky folder path");
            }
        }
    }
}
#endif