
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[InitializeOnLoad]
public class HierarchyEditor : MonoBehaviour
{

#if UNITY_EDITOR
    private static bool s_IsEditor = true;
#else
        private static bool s_IsEditor = false;
#endif
    private static bool s_ShowExportInfo = false;
    private static Dictionary<string, bool> s_ExportItemNameMap = new Dictionary<string, bool>();
    private static Dictionary<string, bool> s_ExportHierarchyItemNameMap = new Dictionary<string, bool>();
    private static GameObject[] s_SelectGameObjects;
    private static Dictionary<GameObject, Rect> s_GameObjectMapRect = new Dictionary<GameObject, Rect>();

    //private static bool s_Dirty = false;

    static HierarchyEditor()
    {
        // UnityEditor.SceneManagement.PrefabStage.prefabStageClosing += OnPrefabStageClosing;
        //UnityEditor.SceneManagement.PrefabStage.prefabStageOpened += OnPrefabStageOpened;
        //UnityEditor.SceneManagement.PrefabStage.prefabSaving += OnPrefabSaving;
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        Selection.selectionChanged += OnSelectionChanged;
    }

    private static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        Object obj = EditorUtility.InstanceIDToObject(instanceID);
        s_ShowExportInfo = UnityEditor.SceneManagement.EditorSceneManager.IsPreviewSceneObject(obj)
            && s_IsEditor && !obj.name.EndsWith("(Environment)");
        ItemOnGUI(selectionRect, obj);
    }

    private static void ItemOnGUI(Rect selectionRect, Object obj)
    {
        if (obj is GameObject gameObject)
        {
            if (!s_GameObjectMapRect.ContainsKey(gameObject))
            {
                s_GameObjectMapRect.Add(gameObject, selectionRect);
            }

            ShowOrHideGameObject(gameObject, selectionRect);
            // CheckExportItem(gameObject, selectionRect);
            CalculateChildNode(gameObject, selectionRect);
        }
    }

    private static void ShowOrHideGameObject(GameObject gameObject, Rect selectionRect)
    {
        Rect rect = new Rect(selectionRect);
        rect.x += rect.width - 10;
        rect.width = 20f;

        bool isActive = GUI.Toggle(rect, gameObject.activeSelf, "");

        if (isActive == gameObject.activeSelf)
            return;

        if (s_SelectGameObjects != null && s_SelectGameObjects.Length > 1)
        {
            Undo.RecordObjects(s_SelectGameObjects, "setActive");
            for (int i = 0; i < s_SelectGameObjects.Length; i++)
            {
                if (s_SelectGameObjects[i] != gameObject)
                {
                    s_SelectGameObjects[i].SetActive(isActive);
                }
            }
        }
        Undo.RecordObject(gameObject, "setActive");
        gameObject.SetActive(isActive);
    }

    // static void CheckExportItem(GameObject obj, Rect selectionRect)
    // {
    //     if (!s_ShowExportInfo)
    //     {
    //         return;
    //     }
    //     Rect rect = new Rect(selectionRect);
    //     rect.x += rect.width - 30;
    //     rect.width = 20f;

    //     if (obj.TryGetComponent<ExportHierarchy>(out var exportHierarchy))
    //     {
    //         if (GUI.Button(rect, "E"))
    //         {
    //             EditorHelper.ExportNested(obj);
    //         }
    //         return;
    //     }
    //     if (GUI.Button(rect, "+"))
    //     {
    //         Undo.AddComponent<ExportHierarchy>(obj);
    //     }
    // }

    static void CalculateChildNode(GameObject gameObject, Rect selectionRect)
    {

        Transform trans = gameObject.transform;

        Rect r = new Rect(selectionRect);
        r.x -= s_ShowExportInfo ? 30 : 10;
        int childCount = 0;
        Transform[] childs = trans.GetComponentsInChildren<Transform>(true);

        childCount = childs.Length - 1;
        TextAnchor ta = GUI.skin.label.alignment;
        GUI.skin.label.alignment = TextAnchor.MiddleRight;
        GUI.Label(r, childCount == 0 ? "" : childCount.ToString());
        GUI.skin.label.alignment = ta;
    }

    static void OnSelectionChanged()
    {
        s_SelectGameObjects = Selection.gameObjects;
    }

    // private static void OnPrefabStageClosing(UnityEditor.SceneManagement.PrefabStage prefabStage)
    // {
    //     string prefabPath = prefabStage.assetPath;
    //     var root = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
    //     if (root == null) return;
    //     var rootUIHierarchy = root.GetComponent<ExportHierarchy>();
    //     if (rootUIHierarchy == null) return;
    //     var uiHierarchyList = root.GetComponentsInChildren<ExportHierarchy>(true);
    //     var uiExportItemList = root.GetComponentsInChildren<ExportItem>(true);
    //     int uiExportItemCount = uiExportItemList.Length + uiHierarchyList.Length - 1;
    //     int uiHierarchyExportCount = 0;
    //     var stringBuilder = Onemt.Core.Util.StringBuilderPool.Get();
    //     bool isRepetition = false;
    //     s_ExportItemNameMap.Clear();
    //     foreach (var uiHierarchy in uiHierarchyList)
    //     {
    //         s_ExportHierarchyItemNameMap.Clear();
    //         var widgets = uiHierarchy.widgets;
    //         foreach (var widget in widgets)
    //         {
    //             uiHierarchyExportCount++;
    //             string name = widget.name;
    //             if (s_ExportHierarchyItemNameMap.ContainsKey(name))
    //             {
    //                 if (!isRepetition)
    //                 {
    //                     isRepetition = true;
    //                     stringBuilder.AppendLine("挂了ExportItem，但是名字重复了");
    //                     stringBuilder.AppendLine();
    //                 }
    //                 stringBuilder.AppendLine(name);
    //             }
    //             else
    //             {
    //                 s_ExportHierarchyItemNameMap.Add(name, true);
    //             }
    //             if (!s_ExportItemNameMap.ContainsKey(name))
    //             {
    //                 s_ExportItemNameMap.Add(name, true);
    //             }
    //         }
    //         // if (uiHierarchy.gameObject.GetComponent<ExportItem>() != null)
    //         // {
    //         //     stringBuilder.AppendLine("挂了UIHierarchy会被直接导出，无需再挂UIExportItem，该节点为：" + uiHierarchy.name);
    //         // }
    //     }

    //     if (isRepetition)
    //     {
    //         if (EditorUtility.DisplayDialog("Warning", stringBuilder.ToString(), "检查"))
    //         {
    //             // AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath));
    //         }
    //         s_ExportItemNameMap.Clear();
    //         Onemt.Core.Util.StringBuilderPool.Release(stringBuilder);
    //         return;
    //     }

    //     if (uiHierarchyExportCount != uiExportItemCount)
    //     {
    //         stringBuilder.AppendLine("挂了ExportItem的总数和ExportHierarchy导出数不相等");
    //         stringBuilder.AppendLine();
    //         if (uiExportItemCount > uiHierarchyExportCount)
    //         {
    //             foreach (var uiExportItem in uiExportItemList)
    //             {
    //                 string name = uiExportItem.name;
    //                 if (!s_ExportItemNameMap.ContainsKey(name))
    //                 {
    //                     stringBuilder.AppendLine(name);
    //                 }
    //             }
    //             stringBuilder.AppendLine();
    //             stringBuilder.Append("以上ExportItem未导出");
    //         }

    //         if (uiHierarchyExportCount > uiExportItemCount)
    //         {
    //             stringBuilder.AppendLine();
    //             stringBuilder.Append("ExportHierarchy导出数 > 挂了ExportItem的总数");
    //         }
    //         if (EditorUtility.DisplayDialog("Warning", stringBuilder.ToString(), "导出", "忽略"))
    //         {
    //             foreach (var uiHierarchy in uiHierarchyList)
    //             {
    //                 EditorHelper.ExportNested(uiHierarchy.gameObject);
    //             }
    //         }
    //         s_ExportItemNameMap.Clear();
    //         Onemt.Core.Util.StringBuilderPool.Release(stringBuilder);
    //     }
    //     //if (_isChanged)
    //     //{
    //     //    if (!UIPrefabHelper.IsArPrefab(prefabPath) && UIPrefabHelper.IsHaveArPrefab(prefabPath))
    //     //    {
    //     //        if (EditorUtility.DisplayDialog("Warning", "有对应的阿语预制体，是否打开？", "打开", "下次"))
    //     //        {
    //     //            AssetDatabase.OpenAsset(AssetDatabase.LoadAssetAtPath<GameObject>(UIPrefabHelper.GetArPrefabPath(prefabPath)));
    //     //        }
    //     //    }
    //     //}
    // }

    // private static void OnPrefabStageOpened(UnityEditor.SceneManagement.PrefabStage prefabStage)
    // {
    //     //_isChanged = false;
    //     //int fontSizeOffset = Config.Instance != null ? Config.Instance.FontSizeOffset : 0;
    //     ////艺术字不改变字体大小
    //     //if (Application.isPlaying && fontSizeOffset != 0)
    //     //{
    //     //    var textList = prefabStage.prefabContentsRoot.transform.GetComponentsInChildren<SoarDText>(true);
    //     //    if (textList.Length > 0)
    //     //    {
    //     //        if (EditorUtility.DisplayDialog("Warning",
    //     //            $"当前游戏语言不为中文，运行时SoarDText会默认{fontSizeOffset}，切勿现在编辑预制体！（只涉及Font=Arial或者没有字体的SoarDText）",
    //     //            $"继续编辑并把所有文本字号+{-fontSizeOffset}", "退出运行模式"))
    //     //        {
    //     //            foreach (var text in textList)
    //     //            {
    //     //                var font = text.font;
    //     //                bool isArial = font == null || font.name == "Arial";
    //     //                if (isArial)
    //     //                {
    //     //                    text.fontSize -= fontSizeOffset;
    //     //                }
    //     //            }

    //     //        }
    //     //        else
    //     //        {
    //     //            EditorApplication.isPlaying = false;
    //     //            UnityEditor.SceneManagement.EditorSceneManager.ClosePreviewScene(prefabStage.scene);
    //     //        }
    //     //    }
    //     //}
    // }

    // //private static void OnPrefabSaving(GameObject obj)
    // //{
    // //    s_Dirty = true;
    // //}
}
