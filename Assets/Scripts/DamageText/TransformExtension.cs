using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Transform����չ��
/// </summary>
public static class TransformExtension
{
    /// <summary>
    /// ��û���������
    /// </summary>
    /// <typeparam name="T">�������</typeparam>
    /// <returns></returns>
    public static T GetOrAddComponent<T>(this Transform transform) where T : UnityEngine.Component
    {
        T component = transform.GetComponent<T>();
        if (component == null)
        {
            component = transform.gameObject.AddComponent<T>();
        }
        return component;
    }



}
