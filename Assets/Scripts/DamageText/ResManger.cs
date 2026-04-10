using Common;
using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ResManger ：
/// </summary>
public class ResManger : MonoSingleton<ResManger>
{
    /// <summary>
    /// 加载预制体的实例
    /// </summary>
    /// <param name="path">路径</param>
    /// <param name="action"></param>
    /// <param name="isAsync">默认false 为异步</param>
    /// <param name="parent"></param>
    public static void LoadPrefabInstance(string path, Action<GameObject> action = null, Transform parent = null, bool isAsync = false)
    {
        GameObject Prefab;
        GameObject PrefabInstace = null;
        if (!isAsync)
        {
            Prefab = Resources.Load<GameObject>(path);
            if (Prefab == null)
                PrefabInstace = Instantiate(Prefab);
            else
                PrefabInstace = Instantiate(Prefab, parent);

            if (action != null && PrefabInstace != null)
                action(PrefabInstace);
        }
        else
        {
            Instance.StartCoroutine(Instance.IELoadResoucresAsync(path, action, parent));
        }
    }

    /// <summary>
    /// 加载Sprite给某个图片
    /// </summary>
    /// <param name="path"></param>
    /// <param name="gameObject">需要给谁设置Sprite</param>
    /// <param name="isAsync"></param>
    public static void LoadSpriteToImage(string path, Transform gameObject, bool isAsync = false, Transform parent = null)
    {
        if (isAsync)
        {
            Instance.StartCoroutine(Instance.IELoadSpriteToImage(path, gameObject, parent));
        }
        else
        {
            if (parent == null)
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
            else
            {
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
                gameObject.SetParent(parent);
            }
        }
    }

    public IEnumerator IELoadSpriteToImage(string path, Transform gameObject, Transform parent = null)
    {
        //是否传入了父节点
        bool isParent = false;
        if (parent != null)
            isParent = true;
        ResourceRequest request = Resources.LoadAsync<Sprite>(path);
        yield return request;

        //父节点不存在
        if (isParent && parent == null)
        {
            //Debug.Log("父节点不存在删除资源");
            Resources.UnloadAsset(request.asset as Sprite);
        }
        else
        {
            if (request.asset is Sprite)
            {
                gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);

                if (parent != null)
                    gameObject.SetParent(parent);
            }
        }
    }

    /// <summary>
    /// 异步加载资源并且实例化
    /// </summary>
    /// <param name="path">资源的路径</param>
    /// <param name="callBack">得到加载出来的资源GameObject的处理函数</param>
    /// <returns></returns>
    public IEnumerator IELoadResoucresAsync(string path, Action<GameObject> action, Transform parent = null)
    {
        //是否传入了父节点
        bool isParent = false;

        if (parent != null)
            isParent = true;
        ResourceRequest request = Resources.LoadAsync<GameObject>(path);
        yield return request;
        //父节点不存在
        if (isParent && parent == null)
        {
            //Debug.Log("父节点不存在删除资源");
            //Resources.UnloadUnusedAssets();

            Resources.UnloadAsset(request.asset);

        }
        else
        {
            if (request.asset is GameObject)
            {
                GameObject gameObject = null;

                if (parent == null)
                    gameObject = Instantiate(request.asset as GameObject);
                else
                    gameObject = Instantiate(request.asset as GameObject, parent);


                action(gameObject);
            }
        }
    }

    /// <summary>
    /// 给指定物体添加组件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    public static void AddComponent<T>(GameObject gameObject) where T : UnityEngine.Component
    {
        if (gameObject.GetComponent<T>() == null)
            gameObject.AddComponent<T>();
    }

    /// <summary>
    /// 设置物体显示/隐藏
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="isView">true--显示</param>
    public void SetGameObjView(GameObject gameObject, bool isView = false)
    {
        gameObject.SetActive(isView);
    }

    /// <summary>
    /// 从JSON文件读取资源
    /// </summary>
    /// <returns></returns>
    public static string ReadInfoJSON<T>(string path, out T JsonClass)
    {
        TextAsset textAsset = Resources.Load<TextAsset>(path);
        JsonClass = JsonUtility.FromJson<T>(textAsset.text);
        return textAsset.text;
    }

    /// <summary>
    /// 从文件夹中加载的所有文件
    /// </summary>
    /// <param name="path">文件夹路径</param>
    /// <returns></returns>
    public static Sprite[] LoadSpriteFromFolder(string path)
    {
        if (!Directory.Exists(path))
            return null;
        string[] imagePaths = Directory.GetFiles(path, "*.png"); // 获取指定文件夹下所有的png图片路径
        int lenth = imagePaths.Length;
        Sprite[] sprites = new Sprite[imagePaths.Length];
        for (int i = 0; i < lenth; i++)
        {
            string fileName = Path.GetFileNameWithoutExtension(imagePaths[i]);
            string name = imagePaths[i].Substring(17, imagePaths[i].Length - 22 - fileName.Length);
            string realPath = name + "/" + fileName;
            sprites[i] = Resources.Load<Sprite>(realPath);
        }
        return sprites;
    }
}
