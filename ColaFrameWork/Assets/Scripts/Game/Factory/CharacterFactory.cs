//----------------------------------------------
//            ColaFramework
// Copyright © 2018-2049 ColaFramework 马三小伙儿
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ColaFramework
{
    /// <summary>
    /// 角色的工厂，负责角色的构建，提供获取角色配置的各种接口
    /// </summary>
    public class CharacterFactory
    {
        /// <summary>
        /// 获取Avatar的ui预览配置信息
        /// </summary>
        /// <param name="index"></param>
        /// <param name="rotation"></param>
        /// <param name="offset"></param>
        /// <param name="scale"></param>
        public static void GetUIAvatarPreviewSetting(int index, out Vector3 rotation, out Vector3 offset, out float scale)
        {
            rotation = Vector3.zero;
            offset = Vector3.zero;
            scale = 0f;
        }
    }
}