//————————————————————————————————————————————
//  BaseState.cs
//  For project: AssetBundleTest
//
//  Created by Chiyu Ren on 2016-08-09 22:10
//————————————————————————————————————————————

using UnityEngine;
using System.Collections;


namespace TooSimpleFramework.Common
{    
    /// <summary>
    /// 状态基类，所有的状态都需要从此类继承
    /// </summary>
    public abstract class BaseState
    {
        #region Proterties
        public int State { get; protected set; }
        public object ParamObject { protected get; set; }
        #endregion


        public BaseState(int pState) 
        {
            this.State = pState;
        }


        #region Public Methods
        /// <summary>
        /// 进入状态
        /// </summary>
        public void Enter() 
        {
            this.OnEnter();
        }
        /// <summary>
        /// 每桢更新状态
        /// </summary>
        public void Update() 
        {
            this.OnUpdate();
        }
        /// <summary>
        /// 退出状态
        /// </summary>
        public void Exit() 
        {
            this.OnExit();
        }
        #endregion


        #region Protected Methods
        /// <summary>
        /// 进入状态时调用
        /// </summary>
        protected abstract void OnEnter();
        /// <summary>
        /// 每桢调用
        /// </summary>
        protected virtual void OnUpdate() { }
        /// <summary>
        /// 退出状态时调用
        /// </summary>
        protected abstract void OnExit();
        #endregion
    }
}