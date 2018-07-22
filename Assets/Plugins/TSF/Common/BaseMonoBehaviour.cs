//————————————————————————————————————————————
//  BaseMonoBehaviour.cs
//  For project: TooSimple Framework
//
//  Created by Chiyu Ren on 2016-10-06 13:00
//————————————————————————————————————————————
using UnityEngine;
using System.Collections;


namespace TooSimpleFramework.Common
{
    /// <summary>
    /// Mono脚本基类
    /// </summary>
    public class BaseMonoBehaviour : MonoBehaviour
    {
        #region Protected Methods
        /// <summary>
        /// 获取指定名称的子级Transform
        /// </summary>
        protected Transform FindChild(string pName)
        {
            return base.transform == null
                ? null
                : base.transform.Find(pName);
        }

        /// <summary>
        /// 获取指定组件
        /// </summary>
        protected T FindComponent<T>() where T : Component
        {
            return base.gameObject.GetComponent<T>();
        }

        /// <summary>
        /// 获取指定子级上的指定组件
        /// </summary>
        protected T FindComponent<T>(string pName) where T : Component
        {
            T ret = null;

            var trans = this.FindChild(pName);
            if (trans != null)
            {
                ret = trans.GetComponent<T>();
            }

            return ret;
        }
        #endregion
    }
}