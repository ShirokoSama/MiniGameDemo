//————————————————————————————————————————————
//  Singleton.cs
//  For project: TooSimple Framework
//
//  Created by Chiyu Ren on 2016-06-11 21:51
//————————————————————————————————————————————
using UnityEngine;
using System;


namespace TooSimpleFramework.Common
{
    /// <summary>
    /// 单例类基类
    /// </summary>
    public abstract class Singleton<T> where T : class, new()
    {
        private static T m_Ins = null;
        private static object m_LockObj = new object();


        ~Singleton()
        {
            m_Ins = null;
            m_LockObj = null;
        }


        public static T Instance
        {
            get
            {
                lock (m_LockObj)
                {
                    if (m_Ins == null)
                    {
                        m_Ins = new T();
                    }
                    return m_Ins;
                }
            }
        }
    }
}