
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
        public readonly int hp, mp, ac, ac2, mac, mac2, dc, dc2, sc, sc2, mc, mc2; 
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
        public int Shape;
        /// <summary>
        /// 需求等级
        /// </summary>
        public int need_lv;
        /// <summary>
        /// 物品等级
        /// </summary>
        public int equip_lv;
        /// <summary>
        /// 需求职业 -1通用 0战士 1法师 2道士
        /// </summary>
        public int job;
        /// <summary>
        /// 售价
        /// </summary>
        public int price;        
        /// <summary>
        /// 属性 防御
        /// </summary>
        public int defmin;
        /// <summary>
        /// 属性 防御
        /// </summary>
        public int defmax;
        public int macdefmin;
        public int macdefmax;
        public int damgemin;
        public int damagemax;
        public int magicmin;
        public int magicmax;
        public string dec;
        /// <summary>
        /// 套装
        /// </summary>
        public int suit;
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
        //int hp, mp, ac, ac2, mac, mac2, dc, dc2, sc, sc2, mc, mc2;
        public Bag_Base_VO(int hp,int mp,int ac,int ac2,int mac,int mac2,int dc,int dc2,int sc,int sc2,int mc,int mc2)
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