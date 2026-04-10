
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 文字弹出
/// </summary>
public class base_info_item : MonoBehaviour
{

    private float fontSize = 60f;

    private float time = 0f;

    private bool state = false;

    private Color color;

    private void Start()
    {
        color = GetComponent<Text>().color;
    }

    public void show_info(string info)//显示信息
    {
        transform.SetAsLastSibling();
        GetComponent<Text>().fontSize = (int)fontSize;
        time = 0;state = true; 
        GetComponent<Text>().text = info;
    }

    private void Update()
    {
        if (state)
        {
            if (time < 3)
            {
                time += Time.deltaTime;
                if (time>2)
                GetComponent<Text>().fontSize--;
                //字体渐变透明
                GetComponent<Text>().color = new Color(color.r, color.g, color.b, 1 - Time.deltaTime);
            }
            else close();
        } 
    }

    private void close()
    {

        state = false;time = 0;

        ObjectPoolManager.instance.PushObjectToPool("info_base", this.gameObject);

    }
}