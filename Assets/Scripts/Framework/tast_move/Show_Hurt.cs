using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MVC
{
    /// <summary>
    /// 显示信息
    /// </summary>
    public class Show_Hurt : Base_Mono
    {

        /// <summary>
        /// 显示内容
        /// </summary>
        Text Text;
        /// <summary>
        /// 滚动速度
        /// </summary>
        private float speed = 2f;
        /// <summary>
        /// 计时器
        /// </summary>
        private float timer = 0f;
        /// <summary>
        /// 销毁时间
        /// </summary>
        private float time = 1.5f;

        float fontSize = 30f;

        private void Awake()
        {
            Text = GetComponent<Text>();
        }


        public void Show(string news, int type)
        {

            switch (type)
            {
                case 1: news = Show_Color.Red(news); break;

                case 2: news = Show_Color.Green(news); break;

                default:
                    break;
            }

            Text.text = news;

            Text.GetComponent<Text>().fontSize = (int)fontSize;

            timer = 0;

        }
        private void Update()
        {
            Scroll();
        }
        private float[] color = { 1, 0, 0 };

        private float HP = 1, MaxHP = 1; 
        /// <summary>
        /// 冒泡效果
        /// </summary>
        private void Scroll()
        {
            //字体滚动
            Text.transform.Translate(Vector3.up * speed * Time.deltaTime);

            Text.transform.position = transform.position + Vector3.up * Mathf.Lerp(4f, 2f, HP / MaxHP);

            timer += Time.deltaTime;
            //字体缩小
            Text.GetComponent<Text>().fontSize--;
            //字体渐变透明
            Text.GetComponent<Text>().color = new Color(color[0], color[1], color[2], 1 - timer);

            if (timer >= 1) ObjectPoolManager.instance.PushObjectToPool("Show_Hurt", this.gameObject);

        }
    }

}
