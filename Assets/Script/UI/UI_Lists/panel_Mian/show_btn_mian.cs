
using UI;
using UnityEngine.UI;

namespace MVC
{
    /// <summary>
    /// 控制开启显示面板
    /// </summary>
    public class show_btn_mian : Panel_Base
    {

        private new void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Show_Panel);
        }
        /// <summary>
        ///  面板列表
        /// </summary>
        public Main_List select_panel;
        /// <summary>
        ///  显示面板
        /// </summary>
        private void Show_Panel()
        {
            transform.parent.parent.parent.parent.SendMessage("OnClickMap", select_panel.ToString());
            Game_Omphalos.i.Show_Screensaver();
        }
    }

}
