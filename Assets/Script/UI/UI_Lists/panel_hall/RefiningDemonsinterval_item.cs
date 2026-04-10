using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MVC
{
    public class RefiningDemonsinterval_item : Base_Mono
    {

        public int index = 0;

        public void show_color(int pos, int lv)
        {
            index = pos;

            switch (lv)
            {
                case 0: GetComponent<Image>().color = Color.gray; break;

                case 1: GetComponent<Image>().color = Color.green; break;//3

                case 2: GetComponent<Image>().color = Color.yellow; break;//2

                case 3: GetComponent<Image>().color = Color.red; break;//1

                default:
                    break;
            }
        }
    }


}
