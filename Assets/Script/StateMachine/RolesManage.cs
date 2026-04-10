using Common;
using MVC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace StateMachine
{
    public class RolesManage : MonoBehaviour
    {
        [HideInInspector] public Animator anim;
     
        [HideInInspector] public Rigidbody2D rb;

        [HideInInspector] public BattleAttack BattleAttack = new BattleAttack();//战斗管理器
        [HideInInspector] public BattleHealth TatgetObg = new BattleHealth();//目标物体
        [HideInInspector] public Vector2 TargetPosition;//目标位置
        [HideInInspector] public Vector2 BackstabPosition;//背刺位置
        [HideInInspector] public float animSpeed ;//动画速度
        [HideInInspector] public AnimatorStateInfo animStateInfo;//动画状态
        [HideInInspector] public AttackStateMachine attack;//战斗逻辑

        /// <summary>
        /// 获取技能
        /// </summary>
        protected base_skill_vo baseskill;
       

        [Header("造成伤害")]
        public float attackDamage = 10f;
        [Header("攻击距离")]
        protected  float AttackDistance = 500f;
        [Header("背刺的距离")]
        public float BehindDistance = 100f;
        [Header("攻击速度")]
        public float AttackSpeed = 5f;
        [Header("移动速度")]
        public float MoveSpeed = 1f;
        [Header("技能释放概率")]
        public float skillRelease = 50f;
        [Header("是否面向左")]
        protected bool facingLeft=true;

        /// <summary>
        /// 宠物名称,当前宠物
        /// </summary>
        private string pet_name,pet_type;
        /// <summary>
        /// 是否为宠物
        /// </summary>
        private bool is_pet = false;
        /// <summary>
        /// 宠物Image显示
        /// </summary>
        private Transform pet_transform;
        /// <summary>
        /// 宠物精灵
        /// </summary>
        private Image pet_Image;

        #region 角色外观
        /// <summary>
        /// 皮肤预制体
        /// </summary>
        private GameObject skin_prefabs;

        /// <summary>
        /// 怪物头像
        /// </summary>
        private Sprite mon_profilePicture;

        /// <summary>
        /// 角色外观位置
        /// </summary>
        private Transform panel_role_health;
        /// <summary>
        /// 怪物类型
        /// </summary>
        private enum_monster_type monster_type;
      

        #endregion


        

        protected virtual void Awake()
        {
            #region 角色外观初始化
         
            //else if(GetComponent<Monster>() != null)
            //{
            //    monster_type = enum_monster_type.磷火兵;
            //    ////skin_prefabs = Resources.Load<GameObject>("Prefabs/monsters/mon_" + monster_type.ToString());
            //    mon_profilePicture= Resources.Load<Sprite>("Prefabs/monsters/mon_" + monster_type.ToString());

            //    panel_role_health = transform.Find("Appearance/profilePicture");
            //    panel_role_health.GetComponent<Image>().sprite = mon_profilePicture;

            //    //Instantiate(skin_prefabs, panel_role_health);
            //}

            #endregion


            rb = GetComponent<Rigidbody2D>();
            //anim = GetComponentInChildren<Animator>();//获取动画组件
            //animSpeed = anim.speed;
            attack = GetComponent<AttackStateMachine>();
            TargetPosition = new Vector2(transform.position.x, transform.position.y);
            BackstabPosition = new Vector2(transform.position.x, transform.position.y);
           
        }


        #region 显示天命光环
        /// <summary>
        /// 天命台
        /// </summary>
        private int[] tianming_Platform;
        /// <summary>
        /// 天命台位置
        /// </summary>
        private Transform show_tianming_Platform;
        /// <summary>
        /// 天命台父物体大小,当前天命大小
        /// </summary>
        private Vector2 pos_tianming_size, tianming_size;
        /// <summary>
        /// 缩放比例
        /// </summary>
        private float scaling = 1;
        /// <summary>
        /// 每个天命的数量
        /// </summary>
        private Dictionary<int, int> tianming_num;

        /// <summary>
        /// 显示天命光环
        /// </summary>
        private void Show_Info_life()
        {

            tianming_Platform = (int[])SumSave.old_crt_hero.tianming_Platform.Clone();

            for (int i = show_tianming_Platform.childCount - 1; i >= 0; i--)//清空区域内按钮
            {
                Destroy(show_tianming_Platform.GetChild(i).gameObject);
            }
            pos_tianming_size = show_tianming_Platform.GetComponent<RectTransform>().rect.size;

            tianming_num = new Dictionary<int, int>();



            for (int i = 0; i < SumSave.old_crt_hero.tianming_Platform.Length; i++)
            {
                if (tianming_num.ContainsKey(SumSave.old_crt_hero.tianming_Platform[i]))
                {
                    tianming_num[SumSave.old_crt_hero.tianming_Platform[i]]++;
                }
                else
                {
                    tianming_num.Add(SumSave.old_crt_hero.tianming_Platform[i], 1);
                }
            }


            for (int i = 0; i < SumSave.old_crt_hero.tianming_Platform.Length; i++)
            {
                GameObject game = Resources.Load<GameObject>("Prefabs/halo/halo_" + (SumSave.old_crt_hero.tianming_Platform[i] + 1));
                GameObject tianming = Instantiate(game, show_tianming_Platform);

                tianming.transform.Rotate(new Vector3(0, 0, 15 * i));


                tianming_size = new Vector2(pos_tianming_size.x * scaling, pos_tianming_size.y * scaling);
                tianming.GetComponent<RectTransform>().sizeDelta = tianming_size;

                Color currentColor = tianming.GetComponentInChildren<Image>().color;
                currentColor.a = tianming_num[SumSave.old_crt_hero.tianming_Platform[i]] * 0.2f;
                tianming.GetComponentInChildren<Image>().color = currentColor;
            }
        }

        #endregion


        protected virtual void Start()
        {
           

        }
        private string animName;

        protected virtual void Update()
        {
            if (TatgetObg == null) return;
            TargetPosition = new Vector2(TatgetObg.transform.position.x, transform.position.y);
            BackstabPosition = new Vector2(TatgetObg.transform.position.x - BehindDistance, transform.position.y);

            if (GetComponent<Player>() != null)
            {
                //int hero_index = int.Parse(SumSave.crt_hero.hero_index);
                //skin_state = (enum_skin_state)hero_index;
                if(is_pet==false)
                {
                    if (animName != SumSave.old_crt_hero.hero_pos)
                    {

                        skin_prefabs = Resources.Load<GameObject>("Prefabs/Skins/skin_" + SumSave.old_crt_hero.hero_pos);

                        animName = SumSave.old_crt_hero.hero_pos;

                        panel_role_health = transform.Find("Appearance");

                        for (int i = panel_role_health.childCount - 1; i >= 1; i--)
                        {
                            Destroy(panel_role_health.GetChild(i).gameObject);
                        }

                        Instantiate(skin_prefabs, panel_role_health);
                        show_tianming_Platform = transform.Find("Appearance/tianming_Platform");

                    }

                    if (tianming_Platform == null || !tianming_Platform.SequenceEqual(SumSave.old_crt_hero.tianming_Platform))
                    {
                        Show_Info_life();
                    }
                }else
                {
                    if(pet_name!=pet_type)
                    {
                        pet_type = pet_name;
                        pet_transform= transform.Find("petAppearance");
                        pet_transform.gameObject.SetActive(true);
                        pet_Image= pet_transform.Find("Image").GetComponent<Image>();
                        Sprite pet_Sprite = Resources.Load<Sprite>("icon/" + pet_name);
                        pet_Image.sprite = pet_Sprite;

                    }
                }
                
            }

        }
        /// <summary>
        /// 宠物Image初始化
        /// </summary>
        /// <param name="name"></param>
        /// <param name="_is_pet"></param>
        public void skil_pet(string name ,bool _is_pet=true)
        {
            pet_name= name;
            is_pet = _is_pet;
        }



        public virtual void Init(BattleAttack battle, BattleHealth _tatgetObg)//初始化参数
        {

            TatgetObg = _tatgetObg;
            BattleAttack = battle;
        }

        public virtual void Animator_State(Arrow_Type arrowType)
        {

        }


        /// <summary>
        /// 延迟关闭动画
        /// </summary>
        /// <param name="animName"></param>
        /// <param name="delay"></param>
        /// <returns></returns>
        public IEnumerator CloseAnimAfterDelay(string animName, float delay)
        {
            yield return new WaitForSeconds(delay);

            CloseAnim(animName); // 传递参数
        }
        /// <summary>
        /// 关闭动画 
        /// </summary>
        /// <param name="animName"></param>
        public void CloseAnim(string animName)
        {
            anim.SetBool(animName, false);
        }

        /// <summary>
        /// 是否背刺0不背刺1背刺
        /// </summary>
        public int isBackstab= 0;
        public void Backstab(int _isBackstab=0)
        {
            isBackstab = _isBackstab;
        }
        /// <summary>
        /// 背刺距离
        /// </summary>
        public float _BehindDistance = 600f;
        /// <summary>
        /// 是否到达背刺地点
        /// </summary>
        [HideInInspector]
        public bool isBackstabpos=false;
        public bool isAttackDistance()//攻击距离
        {
            if(TatgetObg == null) return true;
            if (isBackstab!=1)
            {
                float distance = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(TatgetObg.transform.position.x, 0));
                if (AttackDistance > distance)
                {
                    RbZero();
                    return true;
                }
                else
                    return false;
            }else
            {
                float distance = Vector2.Distance(new Vector2(transform.position.x, 0), new Vector2(TatgetObg.transform.position.x, 0));
                if (transform.position.x < TatgetObg.transform.position.x-_BehindDistance || isBackstabpos)
                {
                    RbZero();
                    isBackstabpos = true;
                    return true;
                }
                return false;
            }
                
        }

        public void TargetMove(Vector2 targetMove)//平移 
        {
            Vector2 direction = Direction(targetMove);
            //Debug.Log(direction + "方向" + TatgetObg.transform.position + "目标位置" + transform.position + "自己位置");
            // 施加追踪力（物理驱动）
            rb.AddForce(direction * MoveSpeed, ForceMode2D.Impulse);
            FlipControl(Direction(TatgetObg.transform.position));

        }

        public void FlipControl(Vector2 direction)//翻转控制
        {
            if (direction.x > 0 && facingLeft)
            {
                Flip();
            }
            else if (direction.x < 0 && !facingLeft)
            {
                Flip();
            }
        }

        public void Flip()//翻转
        {
            facingLeft = !facingLeft;
            //transform.Rotate(0, 180, 0);
        }



        public Vector2 Direction(Vector2 targetPosition)//判断方向
        {
            Vector2 direction = targetPosition - rb.position;
            direction.Normalize();
            return direction;
        }



        public void RbZero()//停止移动
        {
            rb.velocity = Vector2.zero;
        }


    }

}
