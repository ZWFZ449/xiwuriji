using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;


namespace Components
{
    /// <summary>
    ///  全局消息弹出框
    /// </summary>
    public class Alert_Icon : Panel_Base
    {
        /// <summary>
        ///  静态属性
        /// </summary>
        private static Alert_Icon instance;
        /// <summary>
        ///  容器
        /// </summary>
        public Transform container;

        public material_item TextPrefabs;
        protected override void Awake()
        {
            base.Awake();

            instance = this;

            transform.SetAsLastSibling();
            // 自动隐藏
            Hide();
        }

        /// <summary>
        ///  显示弹出框
        /// </summary>
        /// <param name="content">内容</param>
        public void ShowIt(string content)
        {
            Show();
            StartCoroutine(close());
        }
       
        /// <summary>
        /// 延迟隐藏
        /// </summary>
        /// <returns></returns>
        IEnumerator close()
        {
            yield return new WaitForSeconds(3f);

            Hide();
        }
        /// <summary>
        ///  显示弹出框
        /// </summary>
        /// <param name="dec">标题</param>

        public static void Show(List<string> dec)
        {
            instance.Info(dec);
        }
        public static void Show(List<(string,int)> dec)
        {
            instance.Info(dec);
        }
        public static void Show(Dictionary<string,int> dec)
        {
            instance.Info(dec);
        }
        public void Info(List<string> dec)
        {
            Show();
            for (int i = container.childCount - 1; i >= 0; i--)//清空区域内按钮
            {
                Destroy(container.GetChild(i).gameObject);
            }
            for (int i = 0; i < dec.Count; i++)
            {
                Instantiate(TextPrefabs, container).Init((dec[i], 0));
            }
        }
        public void Info(List<(string,int)> dec)
        {
            Show();
            for (int i = container.childCount - 1; i >= 0; i--)//清空区域内按钮
            {
                Destroy(container.GetChild(i).gameObject);
            }
            for (int i = 0; i < dec.Count; i++)
            {
                Instantiate(TextPrefabs, container).Init((dec[i].Item1, dec[i].Item2));
            }
        }

        public void Info(Dictionary<string, int> dec)
        {
            Show();
            for (int i = container.childCount - 1; i >= 0; i--)//清空区域内按钮
            {
                Destroy(container.GetChild(i).gameObject);
            }
            foreach (var item in dec.Keys)
            {
                Instantiate(TextPrefabs, container).Init((item, dec[item]));

            }
            
        }
    }
}
