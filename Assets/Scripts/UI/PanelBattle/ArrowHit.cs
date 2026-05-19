using GifImporter;
using MVC;
using System.Collections.Generic;
using UnityEngine;

public class ArrowHit : MonoBehaviour
{
    GameObject ArrowPrefabs;

    /// <summary>
    /// 存储技能预制体
    /// </summary>
    private Dictionary<db_skill_vo, GameObject> dic = new Dictionary<db_skill_vo, GameObject>();

    public void OnArrow(BaseBattleAttack baseBattle, db_skill_vo skill, BattleHealthState monster)
    {
        if (!dic.ContainsKey(skill)) 
            dic.Add(skill, Resources.Load<GameObject>("UI/skill_prefabs/skill_" + skill.id));
        ArrowPrefabs= dic[skill];
        GameObject go = ObjectPoolManager.instance.GetObjectFormPool(skill.show_name, ArrowPrefabs, new Vector3(transform.position.x, transform.position.y), Quaternion.identity, transform);

        Skill_Hit projectile = go.GetComponent<Skill_Hit>();
        projectile.SetTargetPosition(baseBattle, skill, monster);
    }

}
