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
	[CustomPropertyDrawer(typeof(ObscuredUInt))]
	internal class ObscuredUIntDrawer : ObscuredTypeDrawer<SerializedObscuredUInt, uint>
	{
		private protected override void DrawProperty(Rect position, SerializedProperty sp, GUIContent label)
		{
			plain = (uint)EditorGUI.LongField(position, label, plain);
		}

		private protected override void ApplyChanges()
		{
			serialized.Hidden = ObscuredUInt.Encrypt(plain, serialized.Key);
			serialized.Hash = HashUtils.CalculateHash(plain);
		}
	}
}