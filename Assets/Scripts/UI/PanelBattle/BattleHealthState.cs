using MVC;
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

    public void Clear()
    {
        StopAllCoroutines();
        PushObjectToPool(GetComponent<BaseBattleAttack>().Data.crt_name);
    }
    /// <summary>
    /// 初始化生命值和魔法值
    /// </summary>
    /// <param name="_maxHP"></param>
    /// <param name="_maxMP"></param>
    public void Init(long _maxHP, int _maxMP,string _base_name)
    {
        maxHP = _maxHP;
        maxMP = _maxMP; 
        currentHP = maxHP;
        currentMP = maxMP;
        base_name = _base_name;
        circularHealthBar.Init(maxHP);
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
        if (currentHP <= 0) return;
        currentHP -= damage;
        switch (GetComponent<BaseBattleAttack>().Data.type)
        {
            case Battle_Game_Type.player:
                break;
            case Battle_Game_Type.call:
                break;
            case Battle_Game_Type.monster:
                break;
            case Battle_Game_Type.Boss:
                transform.parent.parent.parent.parent.SendMessage("Real_Time_BossSlider", currentHP);
                break;
            case Battle_Game_Type.Activity_Monster:
                break;
        }
        Hurt(damage, type);
        circularHealthBar.ChangeHealth(currentHP);
        if (currentHP <= 0)
        {
            StartCoroutine(WaitAndDestory(base_name));

        }
    }
    /// <summary>
    /// 监控自身血量变化百分比
    /// </summary>
    /// <returns></returns>
    public (int, int) Proportion()
    {
        (int, int) proportion = (100, 100);
        proportion.Item1 = (int)(currentHP * 100 / maxHP);
        proportion.Item2 = (int)(currentMP * 100 / maxMP);
        return proportion;
    }
    public virtual IEnumerator WaitAndDestory(string healthname)
    {
        if (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(0.48f);
            PushObjectToPool(healthname);
            transform.parent.parent.parent.parent.SendMessage("clearSumhealth", this);
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
    public bool isDead { get { return currentHP <= 0; } }

    public int Get_MP { get { return (int)currentMP; } } 
    /// <summary>
    /// 消耗魔法值
    /// </summary>
    public int Set_Mp { set { currentMP -= value; } }
    /// <summary>
    /// 使用药品或者治疗
    /// </summary>
    /// <param name="hp"></param>
    /// <param name="mp"></param>
    public void Use_Medicine(int hp, int mp)
    {
        currentHP = currentHP + hp > maxHP ? maxHP : currentHP + hp;
        currentMP = currentMP + mp > maxMP ? maxMP : currentMP + mp;
    }

}
