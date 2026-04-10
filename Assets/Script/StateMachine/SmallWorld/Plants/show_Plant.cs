using Common;
using Components;
using MVC;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class show_Plant : Base_Mono
{

    private Transform pos_btn,pos_panlt;

    private List<string> btn_list= new List<string>() { "扩容", "播种","收获"};//"浇水",
    /// <summary>
    /// 植物按钮
    /// </summary>
    private btn_item btn_item_Prefabs;
    /// <summary>
    /// 选择植物下拉列表
    /// </summary>
    private Dropdown plantDropdown;
    /// <summary>
    /// 植物预制体
    /// </summary>
    private seed_item panltItemPrefab;
    /// <summary>
    /// 当前选择的植物
    /// </summary>
    private string currentPlant;
    /// <summary>
    /// 种植的植物列表
    /// </summary>
    private List<seed_item> panltList = new List<seed_item>();
    /// <summary>
    /// 浇水减少时间
    /// </summary>
    private int reduceTime = 10;
    /// <summary>
    /// 守护兽显示
    /// </summary>
    private Image pet_guardImage;
    /// <summary>
    /// 守护兽名字显示
    /// </summary>
    private Text pet_guardtext;
    private int[] need_list = new int[] { 500, 5000, 10000, 20000, 30000, 40000, 50000, 60000, 200000, 200000, 200000, 200000, 200000, 200000, 200000, 200000, 200000, 200000 };
    /// <summary>
    /// 透明Sprite 
    /// </summary>
    private Sprite transparent;
    private void Awake()
    {
        pos_btn=Find<Transform>("btn_list");
        btn_item_Prefabs = Battle_Tool.Find_Prefabs<btn_item>("btn_item"); 
        for (int i = 0; i < btn_list.Count; i++)
        {
            btn_item item= Instantiate(btn_item_Prefabs, pos_btn);
            item.Show(i, btn_list[i]);
            item.GetComponent<Button>().onClick.AddListener(()=> { OnClick(item); });
        }
        plantDropdown= Find<Dropdown>("plant_Dropdown");
        plantDropdown.options.Add(new Dropdown.OptionData() { text = "请选择种子" });
        for (int i = 0; i < SumSave.db_plants.Count; i++)
        {
            plantDropdown.options.Add(new Dropdown.OptionData() { text = SumSave.db_plants[i].plantName });
        }
        plantDropdown.onValueChanged.AddListener(OnSelect);
        panltItemPrefab = Battle_Tool.Find_Prefabs<seed_item>("seeditem"); //Resources.Load<seed_item>("Prefabs/panel_smallWorld/seeditem"); //Assets / Resources / Prefabs / panel_smallWorld / seeditem.prefab
        pos_panlt = Find<Transform>("Scroll View/Viewport/Content");

        pet_guardImage = Find<Image>("pet_guard/display/icon");
        pet_guardtext = Find<Text>("pet_guard/info/name");
        transparent= pet_guardImage.sprite;

    }
    /// <summary>
    /// 选择植物
    /// </summary>
    /// <param name="arg0"></param>
    private void OnSelect(int index)
    {
        currentPlant = null;
        if (index != 0)
        {
            currentPlant = plantDropdown.options[index].text;
            Alert_Dec.Show("当前选择种植 " + currentPlant);
        }
    }

    public override void Show()
    {
        base.Show();
        
        Base_Show();
    }

    private void Base_Show()
    {
        pet_Init();
        List<(string, DateTime)> Set = SumSave.crt_plant.Set();
        StopAllCoroutines();
        ClearObject(pos_panlt);
        panltList.Clear();//清空列表
        for (int i = 0; i < Set.Count; i++)
        {
            seed_item item = Instantiate(panltItemPrefab, pos_panlt);
            item.index = i;
            item.GetComponent<Button>().onClick.AddListener(delegate { PlantInfo(item); });
            user_plant_vo vo = ArrayHelper.Find(SumSave.db_plants, e => e.plantName == Set[i].Item1);
            if (vo != null) item.Init(vo,Set[i].Item2); 
            panltList.Add(item);
        }
        StartCoroutine(ShowPlant()); 
    }
    /// <summary>
    /// 刷新状态
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowPlant()
    {
        for (int i = 0; i < panltList.Count; i++)
        {
            panltList[i].Fixed_Update(1);
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(ShowPlant());

    }
    /// <summary>
    /// 点击植物信息 单个播种
    /// </summary>
    /// <param name="item"></param>
    private void PlantInfo(seed_item item)
    {
        if (item.state())
        {
            if (currentPlant == null) { Alert_Dec.Show("请选择种子");return; }
            NeedConsumablesold(currentPlant, 1);
            if (RefreshConsumables_old())
            {
                List<(string, DateTime)> Set = SumSave.crt_plant.Set();
                Set[item.index] = (currentPlant, SumSave.nowtime);
                user_plant_vo vo = ArrayHelper.Find(SumSave.db_plants, e => e.plantName == currentPlant);
                if (vo != null) item.Init(vo, Set[item.index].Item2);

                SumSave.crt_plant.Up_user_plants(Set);//更新角色数据
                Wirte(Set);//写入数据库

                Alert_Dec.Show("播种成功");
                SeedingTask();
            }
            else Alert_Dec.Show("种子不足");
        }
        else Alert_Dec.Show("当前土地种植" + item.db_plant.plantName);
    }
    /// <summary>
    /// 守护兽初始化
    /// </summary>
    public void pet_Init()
    {
        List<db_pet_vo_old> crt_pet_list = SumSave.crt_pet_Old.Set();
        if(crt_pet_list.Count==0)
        {
            pet_guardImage.sprite = transparent;
            pet_guardtext.text = "无守护兽";
            return;
        }
        for (int i = 0; i < crt_pet_list.Count; i++)
        {
            if (crt_pet_list[i].pet_state == "1")
            {
                pet_guardImage.sprite= UI.UI_Manager.I.GetEquipSprite("UI/pet/", crt_pet_list[i].petName);
                pet_guardtext.text = crt_pet_list[i].petName+"lv:"+ crt_pet_list[i].level;
            }
        }
    }
    /// <summary>
    /// 点击植物按钮
    /// </summary>
    /// <param name="item"></param>
    private void OnClick(btn_item item)
    {
        switch (btn_list[item.index])
        {
            case "扩容":
                Expansion();
                break;
            case "播种":
                Seeding();
                break;
            case "浇水":
                Watering();
                break;
            case "收获":
                Harvest();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 扩容
    /// </summary>
    private void Expansion()
    {
        if ((SumSave.crt_world.World_Lv / 5 + 3) > panltList.Count)
        {
            int num = panltList.Count - 1;
            if (panltList.Count <= 1)
            {
                num = 0;
            }
            Alert.Show("扩容", "扩容需要" + currency_unit.灵气 + need_list[num],
                ConfigExpansion);
           
        }
        else
        {
            Alert_Dec.Show("可扩容数量为0");
        }

    }
    /// <summary>
    /// 拓地
    /// </summary>
    /// <param name="arg0"></param>
    private void ConfigExpansion(object arg0)
    {
        int num = panltList.Count - 1;
        if (panltList.Count <= 1)
        {
            num = 0;
        }
        NeedConsumablesold(currency_unit.灵气, need_list[num]);
        if (RefreshConsumables_old())
        {
            List<(string, DateTime)> Set = SumSave.crt_plant.Set();
            Set.Add(("0", SumSave.nowtime));
            Wirte(Set);
            Base_Show();
            transform.parent.parent.GetComponent<panel_smallWorld>().Base_Show();
        }
        else Alert_Dec.Show("灵气不足");
    }

    /// <summary>
    /// 浇水
    /// </summary>
    private void Watering()
    {
        int number = 0;
        for (int i = 0; i < panltList.Count; i++)
        {
            if (!panltList[i].state()&& !panltList[i].isMatured())//判断是否可以浇水且植物不为空&& Set[i].Item1 != "0"
            {
                number++;
            }
        }
        if (number > 0)
        {
            Alert.Show("浇水", "浇水需要" + currency_unit.灵气 + " " + (200 * number) + "\n是否需要？",
    ConfigWatering, number);
        }else
        {
            Alert_Dec.Show("没有需要浇水的植物");
        }
        
    }
    /// <summary>
    /// 浇水
    /// </summary>
    /// <param name="arg0"></param>
    private void ConfigWatering(object arg0)
    {
        int number = (int)arg0;
        NeedConsumablesold(currency_unit.灵气, number * 200);
        if (RefreshConsumables_old())
        {
            List<(string, DateTime)> Set = SumSave.crt_plant.Set();
            for (int i = 0; i < panltList.Count; i++)
            {
                if (!panltList[i].state())//判断是否可以浇水且植物不为空&& Set[i].Item1 != "0"
                {
                    Set[i] = (Set[i].Item1, Set[i].Item2.AddSeconds(-reduceTime));//更新成熟时间
                    Alert_Dec.Show("已浇水，" + Set[i].Item1 + "生长时间减少" + reduceTime + "秒");
                    panltList[i].updata_time(Set[i].Item2);
                }
            }
            SumSave.crt_plant.Up_user_plants(Set);//更新角色数据
            Wirte(Set);//写入数据库
        }
        else
        {
            Alert_Dec.Show("灵气不足");
        }
    }

    /// <summary>
    /// 写入
    /// </summary>
    private void Wirte(List<(string, DateTime)> Set)
    {
        SumSave.crt_plant.Set_data(Set);
        Game_Omphalos.i.GetQueue(Mysql_Type.UpdateInto, Mysql_Table_Name.mo_user_plant,
            SumSave.crt_plant.Set_Uptade_String(), SumSave.crt_plant.Get_Update_Character());

    }

    /// <summary>
    /// 收获
    /// </summary>
    private void Harvest()
    {
        List<(string, DateTime)> Set = SumSave.crt_plant.Set();

        for (int i = 0; i < panltList.Count; i++)
        {
            if (panltList[i].isMatured())
            {
                Set[i] = ("0", SumSave.nowtime > DateTime.Now ? SumSave.nowtime : DateTime.Now);
                Wirte(Set);//写入数据库
                Alert_Dec.Show("已收获");
                int number = panltList[i].db_plant.DoubleTheAcquisition();
                int random = Random.Range(1, 100);
                int maxnumber = number + Random.Range(1, 100);
                Battle_Tool.Obtain_Resources(Obtain_Int.Add(1, panltList[i].db_plant.HarvestMaterials, new int[] { number + random, random }), maxnumber);
                //Battle_Tool.Obtain_Resources(panltList[i].db_plant.HarvestMaterials, number);
                panltList[i].Clear();
            }
        }
    }


    /// <summary>
    /// 播种
    /// </summary>
    private void Seeding()
    {
        if (currentPlant == null) { Alert_Dec.Show("请选择播种物品"); return; }
        Alert_Dec.Show("请点击空白土地播种");
        return;
        List<(string, DateTime)> Set = SumSave.crt_plant.Set();//获取种子名称以及需要成熟的时间
        List<int> numbers = new List<int>();//空土地的索引
        for (int i = 0; i < Set.Count; i++)
        {
            if (Set[i].Item1 == "0") numbers.Add(i);
        }
        if (numbers.Count == 0)
        {
            Alert_Dec.Show("土地已满");
            return;
        }
        int number = numbers.Count;//空土地的数量

        NeedConsumablesold(currentPlant, number);
        while (!RefreshConsumables_old())
        {
            if (number > 0)
            {
                number--;
                NeedConsumablesold(currentPlant, number);
            }
            else
            {
                Alert_Dec.Show("背包中没有该种子");
                return;
            }
        }
        //判断是否可以种植
        for (int i = 0; i < number; i++)
        {
            Set[numbers[i]] = (currentPlant.ToString(), SumSave.nowtime);
        }
        Wirte(Set);
        SeedingTask();
        Base_Show();

    }
    /// <summary>
    /// 播种任务
    /// </summary>
    private static void SeedingTask()
    {
        SumSave.crt_pass.progress(1);
        tool_Categoryt.Base_Task(1090);
    }

}
