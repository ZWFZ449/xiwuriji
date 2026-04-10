using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MVC;
using UI;
using UnityEngine.UI;
namespace Components
{
    public class Alert_Screensaver : Panel_Base
    {
        /// <summary>
        ///  静态属性
        /// </summary>
        private static Alert_Screensaver instance;
        private Button close;
        protected override void Awake()
        {
            base.Awake();
            instance = this;
            this.gameObject.SetActive(false);
        }
        public override void Initialize()
        {
            base.Initialize();
            close = Find<Button>("close");
            close.onClick.AddListener(() => { Hide(); });
        }

        public override void Hide()
        {
            Game_Omphalos.i.Show_Screensaver();
            base.Hide();
        }
        public static void show_Screensaver()
        {
            instance.Show();
        }
     }

}
