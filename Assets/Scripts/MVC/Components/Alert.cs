using System.Collections.Generic;
using UI;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Components
{
    /// <summary>
    ///  全局弹出框
    /// </summary>
    public class Alert : Panel_Base
    {
        /// <summary>
        /// 列表
        /// </summary>
        private static List<(string, string, UnityAction<object>, object, UnityAction<object>, bool)> list = new List<(string, string, UnityAction<object>, object, UnityAction<object>, bool)>();
        /// <summary>
        /// 当前列表
        /// </summary>
        private static (string, string, UnityAction<object>, object, UnityAction<object>, bool) current;
        /// <summary>
        /// 是否显示
        /// </summary>
        private static bool isShow = true;

        private Text base_info, info_content;

        private Button btn_success, btn_close;

        /// <summary>
        ///  静态属性
        /// </summary>
        private static Alert instance;

        protected override void Awake()
        {
            base.Awake();

            instance = this;

            transform.SetAsLastSibling();

            base_info = Find<Text>("alertbg/base_info/Text");

            info_content = Find<Text>("alertbg/info_value/ScrollView/Viewport/Text");

            btn_success = Find<Button>("alertbg/btn_list/btn_success");

            btn_success.onClick.AddListener(TryCallback);

            btn_close = Find<Button>("alertbg/btn_list/btn_close");

            btn_close.onClick.AddListener(NoTryCallback);
            // 自动隐藏
            Hide();
        }
        public override void Hide()
        {
            if (list.Count > 0)
            {
                list.Remove(current);
                isShow = true;
                if (list.Count > 0)
                {
                    current = list[0];
                    instance.ShowIt(current.Item1, current.Item2, current.Item3, current.Item4, current.Item5, current.Item6);
                }
                else base.Hide();
            }
            else
            base.Hide();
        }
        /// <summary>
        ///  回调函数
        /// </summary>
        private UnityAction<object> callback;

        /// <summary>
        ///  回调函数
        /// </summary>
        private UnityAction<object> Nocallback;
        /// <summary>
        ///  参数
        /// </summary>
        private object data;
        /// <summary>
        ///  调用回调函数
        /// </summary>
        private void TryCallback()
        {
            // 调用回调函数
            if (callback != null) callback(data);
            // 关闭
            Hide();
        }
        private void NoTryCallback()
        {
            // 调用回调函数
            if (Nocallback != null) Nocallback(data);
            // 关闭
            Hide();
        }
        /// <summary>
        ///  显示弹出框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="callback">回调函数</param>
        /// <param name="data">参数</param>
        /// <param name="showCloseButton">是否显示关闭按钮</param>
        public void ShowIt(string title, string content,
            UnityAction<object> callback = null, object data = null, UnityAction<object> Nocallback = null,
            bool showCloseButton = true)
        {
            isShow = false;
            this.base_info.text = title;

            this.info_content.text = content;

            this.callback = callback;

            this.Nocallback = Nocallback;

            this.data = data;

            btn_close.gameObject.SetActive(!showCloseButton);

            Show();
        }

        /// <summary>
        ///  显示弹出框
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="content">内容</param>
        /// <param name="callback">回调函数</param>
        /// <param name="data">参数</param>
        /// <param name="showCloseButton">是否显示关闭按钮</param>
        public static void Show(string title, string content,
            UnityAction<object> callback = null, object data = null, UnityAction<object> Nocallback = null,
            bool showCloseButton = true)
        {
            (string, string, UnityAction<object>, object, UnityAction<object>, bool) item= (title, content, callback, data, Nocallback, showCloseButton);
            list.Add(item);
            if (isShow)
            {
                current = item;
                instance.ShowIt(title, content, callback, data, Nocallback, showCloseButton);
            } 
        }
    }
}
