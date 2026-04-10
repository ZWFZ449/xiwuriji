
using MVC;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    ///  显示和隐藏功能的面板,closeButton
    /// </summary>
    public class Panel_Base : Base_Mono
    {
        protected Button closeButton;
        protected string panelName;
        protected virtual void Awake()
        {
            closeButton = Find<Button>("close_button");
            if (closeButton != null) closeButton.onClick.AddListener(Hide);
            Initialize();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public virtual void Initialize()
        {

        }
        /// <summary>
        ///  隐藏
        /// </summary>
        public virtual void Hide()
        {
            this.gameObject.SetActive(false);
        }
        /// <summary>
        /// 清空区域内的对象
        /// </summary>
        /// <param name="pos_btn"></param>
        //public virtual void ClearObject(Transform pos_btn)
        //{
        //    for (int i = pos_btn.childCount - 1; i >= 0; i--)//清空区域内按钮
        //    {
        //        Destroy(pos_btn.GetChild(i).gameObject);
        //    }
        }
}

