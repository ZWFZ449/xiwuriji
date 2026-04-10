using UnityEngine;

namespace Common
{
    /*
        如何使用脚本单例类：
        1. 定义管理类，继承单例类，传递管理类类型。
        2. 重写Init方法进行初始化。
        **********************************
        3. 何时何地都可以通过以下代码访问管理类成员
            管理类.Instance.成员 
        */
    /// <summary>
    /// 脚本单例类
    /// </summary>
    public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    //instance = this as T;
                    instance = FindObjectOfType<T>();
                    //如果在场景中没有找到该对象
                    if (instance == null)
                    {
                        //创建脚本对象，立即执行Awake，为instance赋值。
                        new GameObject("Singleton of " + typeof(T)).AddComponent<T>();
                    }
                    else
                    {
                        instance.Initialize();
                    }
                }
                return instance;
            }
        }
        public static T I
        {
            get { return Instance; }
        }
        protected virtual void Initialize() { }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this as T;

                Initialize();
            }
        }
    }
}