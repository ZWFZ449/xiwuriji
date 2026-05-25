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
	[CustomPropertyDrawer(typeof(ObscuredSByte))]
	internal class ObscuredSByteDrawer : ObscuredTypeDrawer<SerializedObscuredSByte, sbyte>
	{
		private protected override void DrawProperty(Rect position, SerializedProperty sp, GUIContent label)
		{
			plain = (sbyte)EditorGUI.IntField(position, label, plain);
		}

		private protected override void ApplyChanges()
		{
			serialized.Hidden = ObscuredSByte.Encrypt(plain, serialized.Key);
			serialized.Hash = HashUtils.CalculateHash(plain);
		}
	}
}


