// Lutify - Unity Asset
// Copyright (c) 2015 - Thomas Hourdel
// http://www.thomashourdel.com

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Lutify))]
public class LutifyEditor : Editor
{
	SerializedProperty p_LookupTexture;
	SerializedProperty p_Split;
	SerializedProperty p_ForceCompatibility;
	SerializedProperty p_LutFiltering;
	SerializedProperty p_Blend;

	void OnEnable()
	{
		p_LookupTexture = serializedObject.FindProperty("LookupTexture");
		p_Split = serializedObject.FindProperty("Split");
		p_ForceCompatibility = serializedObject.FindProperty("ForceCompatibility");
		p_LutFiltering = serializedObject.FindProperty("LutFiltering");
		p_Blend = serializedObject.FindProperty("Blend");
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		Texture2D lut = (Texture2D)p_LookupTexture.objectReferenceValue;

		EditorGUILayout.BeginHorizontal();
		{
			EditorGUILayout.PrefixLabel("Lookup Texture");

			EditorGUILayout.BeginHorizontal();
			{
				lut = (Texture2D)EditorGUILayout.ObjectField(lut, typeof(Texture2D), false);
				if (GUILayout.Button("N", EditorStyles.miniButton)) lut = null;
			}
			EditorGUILayout.EndHorizontal();
		}
		EditorGUILayout.EndHorizontal();

		p_LookupTexture.objectReferenceValue = lut;

		EditorGUILayout.PropertyField(p_Split);
		EditorGUILayout.PropertyField(p_ForceCompatibility);
		EditorGUILayout.PropertyField(p_LutFiltering);
		EditorGUILayout.PropertyField(p_Blend);

		if (LutifyBrowser.inst == null)
		{
			if (GUILayout.Button("Open LUT Gallery"))
				LutifyBrowser.Init(target as Lutify);
		}
		else
		{
			if (GUILayout.Button("Close LUT Gallery"))
				LutifyBrowser.inst.Close();
		}

		serializedObject.ApplyModifiedProperties();
	}
}
