using System.Collections.Generic;
using UnityEngine;
using Common;
using MVC;
using System;

namespace UI
{
    /// <summary>
    ///  UI管理器:获取面板,切换面板显示或隐藏,切换多个对象的显示或隐藏
    /// </summary>
    public class UI_Manager : MonoSingleton <UI_Manager>
    {
        /// <summary>
        ///  初始化
        /// </summary>
        protected override void Initialize()
        {
            mainCanvas = GameObject.FindGameObjectWithTag("MainCanvas").transform;

            panels = new Dictionary<string, GameObject>();

            objects = new Dictionary<string, GameObject[]>();
            // 遍历枚举类Tags
            Array tagArr = Enum.GetValues(typeof(Tags));
            foreach (var t in tagArr)
            {
                string tag = t.ToString();
                objects[tag] = GameObject.FindGameObjectsWithTag(tag); // 自动添加对象
            }

            //objects[ "MainObjects" ] = GameObject.FindGameObjectsWithTag( "MainObjects" );
            //objects[ "UserObjects" ] = GameObject.FindGameObjectsWithTag( "UserObjects" );
        }

        private Transform mainCanvas;
        /// <summary>
        ///  存储所有面板
        /// </summary>
        private Dictionary<string, GameObject> panels;

        /// <summary>
        ///  获取面板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetPanel<T>(string name = null)
        {
            string key = name == null ? typeof(T).Name : name;
            // 先判断缓存里面有没有
            if (!panels.ContainsKey(key)) // 如果没有
                panels[key] = mainCanvas.Find(key).gameObject;  // 找出对象,缓存一下
            return panels[key].GetComponent<T>();
        }
        /// <summary>
        /// 切换面板隐藏或隐藏
        /// </summary>
        /// <param name="name">面板名称(继承BasePanel)</param>
        /// <param name="active">显示或隐藏</param>
        public void TogglePanel(string name, bool active)
        {
            Panel_Base p = GetPanel<Panel_Base>(name);

            if (active) p.Show();

            else p.Hide();
        }
        /// <summary>
        /// 切换面板隐藏或隐藏
        /// </summary>
        /// <param name="p"></param>
        /// <param name="active"></param>
        public void TogglePanel(Panel_List p, bool active)   
        {
            TogglePanel(p.ToString(), active);
        }
       
        /// <summary>
        ///  存储指定tag对象
        /// </summary>
        private Dictionary<string, GameObject[]> objects;
        /// <summary>
        ///  切换指定tag对象的显示和隐藏
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="active">显示或隐藏</param>
        public void ToggleObjects(string tag, bool active)
        {

            foreach (GameObject go in objects[tag])
                go.SetActive(active);
        }
        /// <summary>
        ///  切换指定tag对象的显示和隐藏
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="active">显示或隐藏</param>
        public void ToggleObjects(Tags tag, bool active)
        {
            ToggleObjects(tag.ToString(), active);
        }
        /// <summary>
        ///  获取装备Sprite
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sprite GetEquipSprite(string Path, object name)
        {
            return GetAtlasSprite(Path, name.ToString());
        }
        /// <summary>
        /// 获取技能Sprite
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Sprite GetEquipSprite(string name)
        {
            return GetAtlasSprite("icon/", name);
        }
       
        // 加载图集
        /// <summary>
        ///  图集的缓存
        /// </summary>
        private Dictionary<string, UnityEngine.Object[]> atlasDic =
            new Dictionary<string, UnityEngine.Object[]>();
        /// <summary>
        ///  从图集获取Sprite
        /// </summary>
        /// <param name="atlasPath">图集路径</param>
        /// <param name="name">Sprite的名称</param>
        /// <returns></returns>
        public Sprite GetAtlasSprite(string atlasPath, string name)
        {
            UnityEngine.Object[] atlas;
            if (atlasDic.ContainsKey(atlasPath)) // 如果缓存里面有
                atlas = atlasDic[atlasPath];
            else // 缓存没有
            {
                atlas = Resources.LoadAll(atlasPath); // 去加载
                atlasDic[atlasPath] = atlas; // 放到缓存中
            }
            return GetSpriteFromAtlas(atlas, name);
        }
        /// <summary>
        ///  Sprite缓存
        /// </summary>
        private Dictionary<string, Sprite> sprites =
            new Dictionary<string, Sprite>();
        /// <summary>
        ///  从图集中找到指定Sprite
        /// </summary>
        /// <param name="atlas">图集对象</param>
        /// <param name="name">Sprite名称</param>
        /// <returns></returns>
        public Sprite GetSpriteFromAtlas(UnityEngine.Object[] atlas, string name)
        {

            // 从缓存中查找
            //if (sprites.ContainsKey(name))
            //    return sprites[name];
            // 从atlas查找
            for (int i = 0; i < atlas.Length; i++)
            {
                if (atlas[i].GetType() == typeof(UnityEngine.Sprite)
                    && atlas[i].name == name)
                {
                    Sprite sp = (Sprite)atlas[i];
                    sprites[name] = sp;
                    return sp;
                }
            }
            // 没有找到
            Debug.LogError("图片名: " + name + "在图集中找不到!");

            return null;
        }
    }
}
