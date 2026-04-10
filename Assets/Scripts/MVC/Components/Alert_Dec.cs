using System.Collections;
using UI;
using UnityEngine;


namespace Components
{
    /// <summary>
    ///  全局消息弹出框
    /// </summary>
    public class Alert_Dec : Panel_Base
    {
        /// <summary>
        ///  静态属性
        /// </summary>
        private static Alert_Dec instance;

        protected override void Awake()
        {
            base.Awake();

            instance = this;

            transform.SetAsLastSibling(); 
 
            // 自动隐藏
            //Hide();
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
        ///  容器
        /// </summary>
        public Transform container;

        public GameObject TextPrefabs;
        /// <summary>
        /// 延迟隐藏
        /// </summary>
        /// <returns></returns>
        IEnumerator close()
        {
            yield return new WaitForSeconds(1.2f);
            Hide();
        } 
        /// <summary>
        ///  显示弹出框
        /// </summary>
        /// <param name="dec">标题</param>

        public static void Show(string dec)
        {
            instance.Info(dec);
        }

        public void Info(string dec)
        {
            GameObject go = ObjectPoolManager.instance.GetObjectFormPool(

               "info_base", TextPrefabs, new Vector3(container.position.x, container.transform.position.y),

               Quaternion.identity, container);
            if(go!=null)
            go.GetComponent<base_info_item>().show_info(dec);
        }
    }
}
