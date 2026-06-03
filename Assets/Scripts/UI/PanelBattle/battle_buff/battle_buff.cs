using Common;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battle_buff : MonoBehaviour
{
    private Transform m_btn;

    private buff_item btn_item_prefab;

    private Dictionary<string, buff_item> buff_item_dic = new Dictionary<string, buff_item>();

    private void Awake()
    {
        m_btn= GetComponent<Transform>();
        btn_item_prefab = Tool_UI.Find_Prefabs<buff_item>("buff_item");
        Init();
        StartCoroutine(Game_buffTime(600f));

    } 
    private void Init()
    {
        List<(string, string, int)> buffs = SumSave.crt_user_unit.GetBuff;
        for (int i = 0; i < buffs.Count; i++)
        {
            int spanSeconds = Battle_Tool.SettlementTransport(buffs[i].Item2, 3);
            int time = buffs[i].Item3 - spanSeconds;//剩余时间
            if (buffs[i].Item1 == common_Buff.月卡.ToString())
            {
                if (buffs[i].Item3 >= 99999)
                {
                    if (!buff_item_dic.ContainsKey(buffs[i].Item1))
                    {
                        buff_item item = Instantiate(btn_item_prefab, m_btn);
                        item.Init(buffs[i].Item1, "永久");
                        buff_item_dic.Add(buffs[i].Item1, item);
                    }
                    else
                    {
                        buff_item_dic[buffs[i].Item1].gameObject.SetActive(true);
                        buff_item_dic[buffs[i].Item1].Init(buffs[i].Item1, "永久");
                    }
                }
                else
                if (time > 0)
                {
                    if (!buff_item_dic.ContainsKey(buffs[i].Item1))
                    {
                        buff_item item = Instantiate(btn_item_prefab, m_btn);
                        item.Init(buffs[i].Item1, time + "h");
                        buff_item_dic.Add(buffs[i].Item1, item);
                    }
                    else
                    {
                        buff_item_dic[buffs[i].Item1].gameObject.SetActive(true);
                        buff_item_dic[buffs[i].Item1].Init(buffs[i].Item1, time + "h");
                    }
                }
                else
                {
                    if (buff_item_dic.ContainsKey(buffs[i].Item1))
                    { 
                       buff_item_dic[buffs[i].Item1].gameObject.SetActive(false);
                    }
                }
            } 
            else
            {
                if (time > 0)
                {
                    if (!buff_item_dic.ContainsKey(buffs[i].Item1))
                    {
                        buff_item item = Instantiate(btn_item_prefab, m_btn);
                        item.Init(buffs[i].Item1, time + "h");
                        buff_item_dic.Add(buffs[i].Item1, item);
                    }
                    else
                    {
                        buff_item_dic[buffs[i].Item1].gameObject.SetActive(true);
                        buff_item_dic[buffs[i].Item1].Init(buffs[i].Item1, time + "h");
                    }

                }
                else
                {
                    if (buff_item_dic.ContainsKey(buffs[i].Item1))
                    { 
                       buff_item_dic[buffs[i].Item1].gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    private IEnumerator Game_buffTime(float time)
    {
        yield return new WaitForSeconds(time);
        Init();
        StartCoroutine(Game_buffTime(time));
    }
}
