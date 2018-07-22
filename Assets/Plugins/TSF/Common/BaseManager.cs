//————————————————————————————————————————————
//  BaseManager.cs
//  For project: TooSimple Framework
//
//  Created by Chiyu Ren on 2016-06-14 17:27
//————————————————————————————————————————————

using UnityEngine;
using System.Collections;


namespace TooSimpleFramework.Common
{
    /// <summary>
    /// 管理器基类
    /// </summary>
    public abstract class BaseManager<T> : Singleton<T> where T : class, new()
    {
        public void Initialize()
        {
            this.OnInitialize(); 
        }

        
        public void Update() 
        {
            this.OnUpdate();
        }
        

        public void Dispose() 
        {
            this.OnDispose();
        }


        protected abstract void OnInitialize();

        protected virtual void OnUpdate() { }

        protected abstract void OnDispose();
    }
}
