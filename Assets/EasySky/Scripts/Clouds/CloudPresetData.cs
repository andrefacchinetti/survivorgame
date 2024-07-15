//
// Copyright(c) 2023 EasySky ASSIST Software
// https://assist-software.net
//
// For information about the licensing and copyright of this software please
// contact ASSIST Software at 3d_support@assist.ro
//

using UnityEngine;

namespace EasySky.Clouds
{
    public class CloudPresetData<T> : ScriptableObject where T : class
    {
        public virtual void CopyData(T data)
        {

        }
    }
}