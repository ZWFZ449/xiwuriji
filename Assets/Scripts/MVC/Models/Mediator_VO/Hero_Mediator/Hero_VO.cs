using CodeStage.AntiCheat.ObscuredTypes;
using Common;
using MVC;
using UnityEngine;
/// <summary>
/// 角色数据
/// </summary>
public class Hero_VO : Base_VO
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public string hero_name;
    /// <summary>
    /// 角色职业
    /// </summary>
    public string hero_type;
    /// <summary>
    /// 角色编号
    /// </summary>
    public string hero_index="1";
    /// <summary>
    /// 已有英雄列表
    /// </summary>
    public string hero_list1;
    /// <summary>
    /// 角色等级
    /// </summary>
    public string hero_lv;
    /// <summary>
    /// 角色等级
    /// </summary>
    public ObscuredInt hero_Lv;
    /// <summary>
    /// 角色经验
    /// </summary>
    public string hero_exp;
    /// <summary>
    /// 角色经验
    /// </summary>
    public long hero_Exp;
    /// <summary>
    /// 角色VIP 等级 经验
    /// </summary>
    public string hero_vip_lv_exp1;

    /// <summary>
    /// 角色上阵选择
    /// </summary>
    public string hero_pos;
    /// <summary>
    /// 角色存储
    /// </summary>
    public string hero_value;
    /// <summary>
    /// 获取英雄资源
    /// </summary>
    public string hero_material;
    /// <summary>
    /// 英雄资源 转生 强化
    /// </summary>
    public ObscuredInt[] hero_material_list;
    /// <summary>
    /// 天命台
    /// </summary>
    public ObscuredInt[] tianming_Platform;


    /// <summary>
    /// 当前职业和天命台写入
    /// </summary>
    private string Get_hero_pos_vase()
    {
        if (tianming_Platform == null)
        {
            InitTianming_Platform();
        }

        string str = "";
        str = hero_pos + "|" + ArrayHelper.Data_Encryption(tianming_Platform);
        return str;
    }


    /// <summary>
    /// 初始化天命台
    /// </summary>
   public void InitDestinyTower(string str)
    {
        tianming_Platform = new ObscuredInt[5];
        string[] tianming = str.Split(' ');
        for (ObscuredInt i = 0; i < tianming.Length; i++)
        {
            tianming_Platform[i] = int.Parse(tianming[i]);
        }

    }
    /// <summary>
    /// 写入皮肤数据，替换前调用
    /// </summary>
    public void Merge_hero_value()
    {
        string[] str = hero_value.Split(',');
        bool isHave = true ;
        for (ObscuredInt i = 0; i < str.Length; i++)
        {
            string[] str1 = str[i].Split('|');
            if(str1[0]== hero_pos)
            {
                str[i] = str1[0] + "|" + ArrayHelper.Data_Encryption(tianming_Platform);
                isHave = false;
                break;
            }

        }
        hero_value = string.Join(",", str);

        if(isHave)
        {
            AddSkin(SumSave.old_crt_hero.hero_pos);
        }    
    }


    /// <summary>
    /// 新增皮肤
    /// </summary>
    /// <param name="str"></param>
    public void AddSkin(string str)
    {
        if(str== "")
        {
            return;
        }
        SumSave.old_crt_hero.InitTianming_Platform();
        hero_value += (hero_value == "" ? "" : ",") + str + "|" + ArrayHelper.Data_Encryption(tianming_Platform);
        MysqlData();
    }


    /// <summary>
    /// 刷新天命台属性并写入
    /// </summary>
    public void RefreshTianming()
    {
        tianming_Platform=Uptianming_Platform();
        MysqlData();
    }


    /// <summary>
    /// 刷新天命台属性
    /// </summary>
    public  ObscuredInt[]  Uptianming_Platform()
    {
        ObscuredInt[] tianming = new ObscuredInt[5];
        for (ObscuredInt i = 0; i < tianming.Length; i++)
        {
            ObscuredInt index = Random.Range(0, 5);
            tianming[i] = index;
        }
        return tianming;
    }
    /// <summary>
    /// 初始化天命
    /// </summary>
    public void InitTianming_Platform()
    {

        tianming_Platform = new ObscuredInt[5];
        for (ObscuredInt i = 0; i < tianming_Platform.Length; i++)
        {
            tianming_Platform[i] = i;
        }
    }


    public override string[] Set_Instace_String()
    {
        return new string[]
        {
            GetStr(0),
            GetStr(SumSave.crt_user.uid),
            GetStr(hero_name),
            GetStr(hero_lv),
            GetStr(hero_exp),
            GetStr(Get_hero_pos_vase()),
            GetStr(hero_value),
            GetStr(ArrayHelper.Data_Encryption(hero_material_list)),
        };
    }

    public override string[] Get_Update_Character()
    {
        return new string[]
        {
            "hero_name",
            "hero_lv",
            "hero_exp",
            "hero_pos",
            "hero_value",
            "hero_material",
        };
    }

    public override void MysqlData()
    {
        base.MysqlData();
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_hero,
                    SumSave.old_crt_hero.Set_Uptade_String(), SumSave.old_crt_hero.Get_Update_Character());
    }

    public override string[] Set_Uptade_String()
    {
        return new string[]
       {
            GetStr(hero_name),
            GetStr(hero_Lv),
            GetStr(hero_Exp),
            GetStr(Get_hero_pos_vase()),
            GetStr(hero_value),
            GetStr(ArrayHelper.Data_Encryption(hero_material_list)),

       };
    }

}
