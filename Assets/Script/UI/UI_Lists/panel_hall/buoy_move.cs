using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace MVC
{
    public class buoy_move : Base_Mono
    {
        private Rigidbody rb;
        /// <summary>
        /// 自身速度
        /// </summary>
        private float AttackSpeed = 300;

        private float X_min = 100, X_max = 800;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            rb.velocity = transform.right * AttackSpeed;
        }
        /// <summary>
        /// 移动
        /// </summary>
        /// <param name="direction"></param>
        public void Move(float x_min, float x_max)
        {
            X_min = x_min;

            X_max = x_max;

            AttackSpeed = Random.Range(200, 500);

            rb.velocity = transform.right * AttackSpeed;

            transform.position = new Vector2(X_min, transform.position.y);
        }

        public void Gather()
        {
            AttackSpeed = 0;

            rb.velocity = transform.right * AttackSpeed;

        }
        private void Update()
        {
            Movement(transform);
        }

        private void Movement(Transform screenPoint)
        {
            if (screenPoint.position.x > X_max)
            {
                screenPoint.position = new Vector2(X_min, screenPoint.position.y);
            }
        }
    }

}
