
using UI;
using UnityEngine.UI;

namespace MVC
{
    /// <summary>
    /// 控制开启显示面板
    /// </summary>
    public class Show_Panel_Button : Panel_Base
    {

        private new void Awake()
        {
            GetComponent<Button>().onClick.AddListener(Show_Panel);
        }
        /// <summary>
        ///  面板列表
        /// </summary>
        public Panel_List select_panel;
        /// <summary>
        ///  显示面板
        /// </summary>
        private void Show_Panel()
        {
            UI_Manager.I.TogglePanel(select_panel, true);
            Game_Omphalos.i.Show_Screensaver();
        }
    }

}
