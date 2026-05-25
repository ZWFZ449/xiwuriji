#region copyright
// ------------------------------------------------------
// Copyright (C) Dmitry Yuhanov [https://codestage.net]
// ------------------------------------------------------
#endregion

using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.ObscuredTypes.EditorCode;
using CodeStage.AntiCheat.Utils;
using UnityEditor;
using UnityEngine;

namespace CodeStage.AntiCheat.EditorCode.PropertyDrawers
{
	[CustomPropertyDrawer(typeof(ObscuredDouble))]
	internal class ObscuredDoubleDrawer : ObscuredTypeDrawer<SerializedObscuredDouble, double>
	{
		private protected override void DrawProperty(Rect position, SerializedProperty sp, GUIContent label)
		{
#if UNITY_2022_1_OR_NEWER
			plain = EditorGUI.DelayedDoubleField(position, label, plain);
#else
			plain = EditorGUI.DoubleField(position, label, plain);
#endif
		}
		
		private protected override void ApplyChanges()
		{
			serialized.Hidden = ObscuredDouble.Encrypt(plain, serialized.Key);
			serialized.Hash = HashUtils.CalculateHash(plain);
		}
	}
}