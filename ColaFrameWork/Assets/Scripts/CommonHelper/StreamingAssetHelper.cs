//----------------------------------------------
//            ColaFramework
// Copyright © 2018-2049 ColaFramework 马三小伙儿
//----------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using ColaFramework.Foundation;

namespace ColaFramework
{
    /// <summary>
    /// StreamingAsset助手类
    /// </summary>
    public static class StreamingAssetHelper
    {
        /// <summary>
        /// 读取写资源路径
        /// </summary>
        private static String assetPath;

        private static string sourceDir;
        private static string destDir;
        private static Action<bool> callback;

        public static String AssetPathDir
        {
            get { return assetPath; }
        }

        /// <summary>
        /// 设置可读写路径
        /// </summary>
        /// <param name="debugDir"></param>
        public static void SetAssetPathDir(String debugDir)
        {
            assetPath = debugDir;
        }

        /// <summary>
        /// 拼接StreamingAsset的子路径
        /// </summary>
        /// <param name="subPath"></param>
        /// <returns></returns>
        public static String CombinePath(String subPath)
        {
            return Path.Combine(Application.streamingAssetsPath, subPath);
        }

        /// <summary>
        /// 读取文件(从可读写资源路径或者StreamingAsset中)
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static String ReadFileText(String filePath)
        {
            String debugPath = assetPath != null ? Path.Combine(assetPath, filePath) : null;
            //先尝试从debupath中读取文件，如果没有再去StreamingAsset中读取
            if (debugPath != null && File.Exists(debugPath))
            {
                return File.ReadAllText(debugPath);
            }
            else
            {
                string fullPath = CombinePath(filePath);
#if UNITY_ANDROID && !UNITY_EDITOR
			WWW www = new WWW(fullPath); 
			while(!www.isDone){}
			return www.text;
#else
                return File.ReadAllText(fullPath);
#endif
            }
        }

#if UNITY_ANDROID && (!UNITY_EDITOR)
    /// <summary>
    /// 在Android平台上，从(apk包中的) Assets目录复制文件至指定目录
    /// 注意：只能在子线程中调用
    /// </summary>
    /// <param name="sourceDir">源目录，是以 assets 为根目录的相对路径</param>
    /// <param name="destDir">目标目录</param>
    /// <param name="title">进度对话框标题</param>
    /// <returns></returns>
    public static void CopyAssetDirectoryInThread(String sourceDir, String destDir,Action<bool> callback)
    {
        StreamingAssetHelper.sourceDir = sourceDir;
        StreamingAssetHelper.destDir = assetPath != null ? Path.Combine(assetPath, destDir) : null;
        StreamingAssetHelper.callback = callback;
        if (StreamingAssetHelper.destDir != null)
        {
            ColaLoom.RunAsync(CopyAssetDirectory);
        }
    }

    /// <summary>
    /// 在Android平台上，从(apk包中的) Assets目录复制文件至指定目录
    /// 注意：只能在主线程中调用
    /// </summary>
    /// <param name="sourceDir">源目录，是以 assets 为根目录的相对路径</param>
    /// <param name="destDir">目标目录</param>
    /// <param name="title">进度对话框标题</param>
    /// <returns></returns>
    public static void CopyAssetDirectory()
    {
        ColaLoom.QueueOnMainThread(() =>
        {
            Debug.Log("CopyAssetDirectory, sourceDir = " + sourceDir + ", destDir = " + destDir);

            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActitivy = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            Debug.Log("CopyAssetDirectory before execute");

            bool ret = currentActitivy.Call<bool>("copyAssets", sourceDir, destDir);
            callback(ret);
            Debug.Log("CopyAssetDirectory finished");
        });
    }
#endif
    }
}