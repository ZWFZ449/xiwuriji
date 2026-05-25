
using CodeStage.AntiCheat.ObscuredTypes;
using Common;


namespace MVC
{
    /// <summary>
    ///  物品数据结构
    /// </summary>
    public class Bag_Base_VO : Base_VO
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly ObscuredInt hp, mp, ac, ac2, mac, mac2, dc, dc2, sc, sc2, mc, mc2; 
        /// <summary>
        /// 物品名称
        /// </summary>
        public string Name;
        /// <summary>
        /// 物品类型
        /// </summary>
        public string StdMode;
        /// <summary>
        /// 物品类型
        /// </summary>
        public ObscuredInt Shape;
        /// <summary>
        /// 需求等级
        /// </summary>
        public ObscuredInt need_lv;
        /// <summary>
        /// 物品等级
        /// </summary>
        public ObscuredInt equip_lv;
        /// <summary>
        /// 需求职业 -1通用 0战士 1法师 2道士
        /// </summary>
        public int job;
        /// <summary>
        /// 售价
        /// </summary>
        public ObscuredInt price;        
        /// <summary>
        /// 属性 防御
        /// </summary>
        public ObscuredInt defmin;
        /// <summary>
        /// 属性 防御
        /// </summary>
        public ObscuredInt defmax;
        public ObscuredInt macdefmin;
        public ObscuredInt macdefmax;
        public ObscuredInt damgemin;
        public ObscuredInt damagemax;
        public ObscuredInt magicmin;
        public ObscuredInt magicmax;
        public string dec;
        /// <summary>
        /// 套装
        /// </summary>
        public ObscuredInt suit;
        /// <summary>
        /// 套装名称
        /// </summary>
        public string suit_name;
        /// <summary>
        public string suit_dec;
        /// <summary>
        /// 判断值 1 名称 2 强化等级 3品质 4附加值 5套装6锁定
        /// </summary>
        private string User_value;
        //ObscuredInt hp, mp, ac, ac2, mac, mac2, dc, dc2, sc, sc2, mc, mc2;
        public Bag_Base_VO(ObscuredInt hp,ObscuredInt mp,ObscuredInt ac,ObscuredInt ac2,ObscuredInt mac,ObscuredInt mac2,ObscuredInt dc,ObscuredInt dc2,ObscuredInt sc,ObscuredInt sc2,ObscuredInt mc,ObscuredInt mc2)
        { 
            this.hp = hp;
            this.mp = mp;
            this.ac = ac;
            this.ac2 = ac2;
            this.mac = mac;
            this.mac2 = mac2;
            this.dc = dc;
            this.dc2 = dc2;
            this.sc = sc;
            this.sc2 = sc2;
            this.mc = mc;
            this.mc2 = mc2;
        
        }

        public Bag_Base_VO()
        {

        }

        public Bag_Base_VO(Bag_Base_VO item)
        {
            Name= item.Name;
            StdMode = item.StdMode;
            Shape = item.Shape;
            need_lv = item.need_lv;
            equip_lv = item.equip_lv;
            price = item.price;
            hp = item.hp;
            mp = item.mp;
            defmin = item.defmin;
            defmax = item.defmax;
            macdefmin = item.macdefmin;
            macdefmax = item.macdefmax;
            damgemin = item.damgemin;
            damagemax = item.damagemax;
            magicmin = item.magicmin;
            magicmax = item.magicmax;
            dec = item.dec;
            suit = item.suit;
            suit_name = item.suit_name;
            suit_dec = item.suit_dec;

        }

        public string user_value
        {
            get { return User_value; }

            set { User_value = value; }
        }
    }
}