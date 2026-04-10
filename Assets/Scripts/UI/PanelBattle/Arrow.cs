using UnityEngine;
using System.Collections;
using MVC;
using Common;

public class Arrow : MonoBehaviour
{

    float speed = 1000;
    [HideInInspector]
    public GameObject target; //瞄准的目标  
    private string skill_name;
    Rigidbody2D rb2d;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    public void Receive(GameObject monster,string skill)
    {
        target = monster;
        skill_name= skill;
        number = 0;
    }
    /// <summary>
    /// 确定物品存在
    /// </summary>
    bool exist = true;

    int number = 0;

    private void OnDestroy()
    {

    }

    void Update()
    {
        if (target != null)
        {
            if (Vector2.Distance(transform.position, target.transform.position) > 50)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.transform.position, 600 * Time.deltaTime);
            }
            else
            {
                OnDestroy();

                ObjectPoolManager.instance.PushObjectToPool(skill_name, this.gameObject);
            }
        }

    }
}
