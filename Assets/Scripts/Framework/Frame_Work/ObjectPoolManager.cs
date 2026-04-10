using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

/// <summary>
/// 对象池管理
/// </summary>
/// ！！！此脚本写好后基本上不需要改动脚本，使用此脚本时需要把此脚本挂在场景中，否则无法使用此脚本
public class ObjectPoolManager : MonoBehaviour

{   //单例
    public static ObjectPoolManager instance;

    //Dictionary字典
    //创建字典，键为string类型，值为List类型的，字典的名字为pool
    private Dictionary<string, List<GameObject>> pool;


    private void Awake()
    {
        instance = this;
        //实例化字典
        pool = new Dictionary<string, List<GameObject>>();
    }
    /// <summary>
    /// 取对象的方法
    /// </summary>
    /// <param name="objName">预设体的名字，也是池子里的名字，也是键值对的名字</param>
    /// <param name="pos">生成对象的坐标</param>
    /// <param name="qua">生成对象的角度</param>
    /// <returns>得到的对象</returns> GameObject为返回值 返回的是实例化的对象
    public GameObject GetObjectFormPool(string objName, GameObject game, Vector3 pos, Quaternion qua, Transform crt)
    {
        //被实例化的对象
        GameObject go;
        //判断是否存在对应的池子（通过字典的键值对判断是否包含objname的键）
        //并判断池子里是否包含对象，有对象才能取出来（通过判断List里的元素个数，大于0说明至少有一个）
        if (pool.ContainsKey(objName) && pool[objName].Count > 0)
        {
            //从链表中取出地一个元素
            go = pool[objName][0];

            //并将地0个元素从链表中移除
            pool[objName].RemoveAt(0);

            //激活取出的对象                          
            go.SetActive(true);
            go.transform.position = pos;
            go.transform.rotation = qua;
            go.transform.parent = crt;
        }
        else
        {
            //如果池子中没有该元素，就从Resources文件夹中实例化出来赋值给go
            go = Instantiate(game, pos, qua, crt);
        }
        //把生成的预设体返回给go
        return go;
    }

    //把实例化出来的物体都存到池子中
    public void PushObjectToPool(string path, GameObject go)
    {
        //通过Instantiate生成的对象名字均为预设体名字加上（clone），所以需要
        //切割才能得到真正预设体的名字
        //string prefabName = go.name.Split('(')[0];
        //if (path == "血光之灾")
        //{
        //    UnityEngine.Debug.LogError("查找路径");
        //}
        //通过预设体的名字来判断是否已经有对应的池子，如果有直接将go放到池子中
        if (pool.ContainsKey(path))
        {
            //把对象go添加到池子中
            pool[path].Add(go);
        }

        //没有对应的池子，那就在pool中创建一组键值对，并将go放到新的池子中
        else
        {
            //实例化一个池子并把go放到池子中
            pool[path] = new List<GameObject>() { go };

        }

        //把物体放到池子中后要把物体身上的速度设为0
        //go.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //把预设体放入池子中后要把生成出来的物体设为隐藏状态
        go.SetActive(false);
    }

    //清空某个类别的游戏对象
    public void Clear(string key)
    {
        //清空时候不是清空键值，而是清空值所对应的对象
        for (int i = pool[key].Count - 1; i >= 0; i--)
        {
            Destroy(pool[key][i]);
        }
        //foreach (var item in cache[key])
        //{
        //    Destroy(item);
        //}
        pool.Remove(key);
    }

    public void RefreshPostion(string key)
    {
        Vector3 vector3 = new Vector3(-1000, -1000);
        for (int i = pool[key].Count - 1; i >= 0; i--)
        {
            pool[key][i].transform.position = vector3;
        }
    }

    public void ClearAll()
    {
        //将池中所有的键给到清空类别进行清空
        //会异常，无效的操作    cache.Remove(key)删除一个键，这时候cache[key].Count=0
        //foreach (var key in cache.Keys)
        //{
        //    Clear(key);
        //}
        //将字典的键用集合存起来 遍历集合
        foreach (var key in new List<string>(pool.Keys))
        {
            Clear(key);
        }
    }


}

