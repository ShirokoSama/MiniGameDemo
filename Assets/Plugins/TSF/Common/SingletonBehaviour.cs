//————————————————————————————————————————————
//  SingletonBehavior.cs
//  For project: TooSimple Framework
//
//  Created by Chiyu Ren on 2016-06-11 21:51
//————————————————————————————————————————————

using UnityEngine;
using System.Collections;

 
namespace TooSimpleFramework.Common 
{
    /// <summary>
    /// 单例Mono脚本基类
    /// </summary>
    public abstract class SingletonBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T m_Ins = null;
        private static object m_LockObj = new object();


        public static T Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    if (m_Ins == null)
                    {
                        var t = typeof(T);
                        m_Ins = (T)Object.FindObjectOfType(t);

                        if (m_Ins == null)
                        {
                            var obj = new GameObject();
                            m_Ins = obj.AddComponent<T>();
                            m_Ins.name = string.Format("{0} (Singleton)", t.Name);
                            Object.DontDestroyOnLoad(m_Ins);
                        }
                    }
                    return m_Ins;
                }
            }
        }
    }
}
