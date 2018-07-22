//————————————————————————————————————————————
//  Framework.cs
//  For project: TooSimple Framework
//
//  Created by Chiyu Ren on 2017-06-01 11:34
//————————————————————————————————————————————

using DG.Tweening;

using TooSimpleFramework.Common;
using TooSimpleFramework.Components;
using TooSimpleFramework.Components.Managers;


namespace TooSimpleFramework
{
    /// <summary>
    /// 框架初始化接口
    /// </summary>
    public class Framework : SingletonBehaviour<Framework>
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        public void Initialize()
        {
            base.gameObject.name = "TooSimple Framework";

            TimerManager.Instance.Initialize();
            AudioManager.Instance.Initialize();

            DOTween.Init();
        }


        void Update()
        {
            TimerManager.Instance.Update();
        }


        void OnDestroy()
        {
            TimerManager.Instance.Dispose();
            AudioManager.Instance.Dispose();
            DOTween.Clear(true);
        }
    }
}