using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHealthState : Base_Mono
{
    /// <summary>
    /// 当前生命值和魔法值外显
    /// </summary>
    private CircularHealthBar circularHealthBar;
    /// <summary>
    /// 最大生命值和魔法值
    /// </summary>
    private long maxHP, maxMP;

    private string base_name;
    /// <summary>
    /// 当前生命值和魔法值
    /// </summary>
    private long currentHP, currentMP;
    /// <summary>
    /// 初始化
    /// </summary>
    private bool is_Dead = false;
    /// <summary>
    /// 被攻击次数
    /// </summary>
    public int attack_number = 0;

    private long CurrentHP { set{ currentHP = value; Show_Info(); } get { return currentHP; } }
    private long CurrentMP { set { currentMP = value; Show_Info(); } get { return currentMP; } }
    /// <summary>
    /// 显示信息
    /// </summary>
    private void Show_Info()
    {
        switch (GetComponent<BaseBattleAttack>().Data.type)
        {
            case Battle_Game_Type.player:
                transform.parent.parent.parent.parent.SendMessage("Show_Slider", this);
                break;
            case Battle_Game_Type.call:
                break;
            case Battle_Game_Type.monster:
                break;
            case Battle_Game_Type.Boss:
                transform.parent.parent.parent.parent.SendMessage("Real_Time_BossSlider", CurrentHP);
                break;
            case Battle_Game_Type.Activity_Monster:
                break;
        }

    }

    public string GetBaseName()
    {
        return base_name + " " + CurrentHP;
    }
    public void Clear()
    {
        //for (int i = transform.childCount - 1; i >= 2; i--)//清空区域内按钮
        //{
        //    if (transform.GetChild(i).GetComponent<Skill_Hit>() != null)
        //    {
        //        //Debug.Log("删除技能");
        //        transform.GetChild(i).GetComponent<Skill_Hit>().On_Destroy();
        //    }
        //} 
        PushObjectToPool(GetComponent<BaseBattleAttack>().Data.crt_name);
    }
    /// <summary>
    /// 初始化生命值和魔法值
    /// </summary>
    /// <param name="_maxHP"></param>
    /// <param name="_maxMP"></param>
    public void Init(long _maxHP, int _maxMP,string _base_name)
    {
        is_Dead = true;
        maxHP = _maxHP + 1;
        maxMP = _maxMP + 1;
        CurrentHP = maxHP;
        CurrentMP = maxMP;
        base_name = _base_name;
        circularHealthBar.Init(_maxHP + 1);
    }
    private void Awake()
    {
        circularHealthBar = Find<CircularHealthBar>("HealthBar_Background/HealthBar_Fill");
    }
    /// <summary>
    /// 文字偏移量
    /// </summary>
    float offset = 1;
    private void Hurt(float dec, DamageEnum type)
    {
        string _dec = dec.ToString("F0");
        offset -= 0.1f;
        if (offset < -2) offset = 1;
        DamageTextManager.Instance.ShowDamageText(type, _dec, this.transform, offset);
    }
    public void TakeDamage(int damage,DamageEnum type = DamageEnum.普通伤害)
    {
        if (CurrentHP <= 0) return;
        CurrentHP -= damage;
        Hurt(damage, type);
        circularHealthBar.ChangeHealth(damage);
        if (CurrentHP <= 0)
        {
            is_Dead = false;
            //StartCoroutine(WaitAndDestory(base_name));
            transform.parent.parent.parent.parent.SendMessage("clearSumhealth", this);
            Clear();
            //PushObjectToPool(base_name);
        }
    }
    /// <summary>
    /// 监控自身血量变化百分比
    /// </summary>
    /// <returns></returns>
    public (int, int) Proportion()
    {
        (int, int) proportion = (100, 100);
        proportion.Item1 = (int)(CurrentHP * 100 / maxHP);
        proportion.Item2 = (int)(CurrentMP * 100 / maxMP);
        return proportion;
    }
    public virtual IEnumerator WaitAndDestory(string healthname)
    {
        if (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(0.2f); 
            PushObjectToPool(healthname);
        }
    }
    /// <summary>
    /// 回收
    /// </summary>
    /// <param name="healthname"></param>
    private void PushObjectToPool(string healthname)
    {
        ObjectPoolManager.instance.PushObjectToPool(healthname, this.gameObject);

    }
    /// <summary>
    /// 判定死亡
    /// </summary>
    public bool isDead { get { return CurrentHP <= 0 && is_Dead; } }

    public int Get_MP { get { return (int)CurrentMP; } } 

    public int Get_hp { get { return (int)CurrentHP; } }
    /// <summary>
    /// 消耗魔法值
    /// </summary>
    public int Set_Mp { set { CurrentMP -= value; } }
    /// <summary>
    /// 使用药品或者治疗
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="mp"></param>
    public void Use_Medicine(int hp, int mp)
    {
        CurrentHP = CurrentHP + hp > maxHP ? maxHP : CurrentHP + hp;
        CurrentMP = CurrentMP + mp > maxMP ? maxMP : CurrentMP + mp;
    }



}
