//����������������������������������������������������������������������������������������
//  TimerManager.cs
//  For project: TooSimple Framework
//
//  Created by Chiyu Ren on 2016-06-11 21:38
//����������������������������������������������������������������������������������������
using UnityEngine;
using System.Collections.Generic;

using TooSimpleFramework.Common;


namespace TooSimpleFramework.Components.Managers
{
    /// <summary>
    /// ��ʱ�������������ڴ���ÿ���������ִ��һ�β����ļ�ʱ��
    /// </summary>
    public class TimerManager : BaseManager<TimerManager>
    {
        #region Private Members
        private Dictionary<string, Timer> m_TimerMap;
        private List<string> m_InvokeList;
        private List<string> m_RemoveList;
        #endregion


        #region Public Methods
        /// <summary>
        /// ���һ����ʱ��
        /// </summary>
        /// <param name="pKey">��ʱ����ʶ��</param>
        /// <param name="pTick">ÿ�����������һ�λص�</param>
        /// <param name="pCallback">�ص�����</param>
        /// <param name="pInvokeNow">�Ƿ��������ûص�</param>
        public Timer AddTimer(string pKey, float pTick, TimerCallback pCallback, bool pInvokeNow = false)
        {
            Timer ret = null;

            if (pTick > 0 && pCallback != null)
            {
                if (this.m_TimerMap.TryGetValue(pKey, out ret))
                {
                    ret.Tick = pTick;
                    ret.Callback = pCallback;
                    if (pInvokeNow) 
                    {
                        ret.Full();
                    }
                    ret.ShouldRemove = false;
                }
                else
                {
                    ret = new Timer(pTick, pCallback, pInvokeNow);
                    this.m_TimerMap.Add(pKey, ret);
                }
            }

            return ret;
        }

        /// <summary>
        /// ��ȡָ����ʶ���ļ�ʱ��
        /// </summary>
        public Timer GetTimer(string pKey)
        {
            Timer ret = null;
            if (this.m_TimerMap.ContainsKey(pKey))
            {
                ret = this.m_TimerMap[pKey];
            }
            return ret;
        }

        /// <summary>
        /// �Ƴ�ָ����ʶ���ļ�ʱ��
        /// </summary>
        /// <param name="pKey">��ʱ����ʶ��</param>
        /// <param name="pDoInvoke">�Ƿ����Ƴ�ǰ���ûص�</param>
        public void RemoveTimer(string pKey, bool pDoInvoke = false)
        {
            if (this.m_TimerMap == null)
            {
                return;
            }

            Timer timer = null;
            if (this.m_TimerMap.TryGetValue(pKey, out timer))
            {
                timer.ShouldRemove = true;
                timer.InvokeWhenRemove = pDoInvoke;
            }
        }

        /// <summary>
        /// ������м�ʱ��
        /// </summary>
        /// <param name="pDoInvoke">�Ƿ����Ƴ�ǰ���ûص�</param>
        public void Clear(bool pDoInvoke = false)
        {
            foreach (var pair in this.m_TimerMap)
            {
                pair.Value.ShouldRemove = true;
                pair.Value.InvokeWhenRemove = pDoInvoke;
            }
        }
        #endregion


        #region Protected Methods
        protected override void OnInitialize()
        {
            this.m_TimerMap = new Dictionary<string, Timer>();
            this.m_InvokeList = new List<string>();
            this.m_RemoveList = new List<string>();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (this.m_TimerMap.Count == 0)
            {
                return;
            }

            var deltaTime = Time.deltaTime;
            foreach (var pair in this.m_TimerMap)
            {
                if (pair.Value.ShouldRemove)
                {
                    this.m_RemoveList.Add(pair.Key);
                }
                else
                {
                    pair.Value.Update(deltaTime);
                } 

                if (pair.Value.ShouldInvoke)
                {
                    this.m_InvokeList.Add(pair.Key);
                }
            }

            if (this.m_InvokeList.Count > 0) 
            {
                for (int i = 0, count = this.m_InvokeList.Count; i < count; i++)
                {
                    var key = this.m_InvokeList[i];
                    this.m_TimerMap[key].Invoke();
                }
                this.m_InvokeList.Clear();
            }

            if (this.m_RemoveList.Count > 0)
            {
                for (int i = 0, count = this.m_RemoveList.Count; i < count; i++)
                {
                    var key = this.m_RemoveList[i];
                    this.m_TimerMap[key].Dispose();
                    this.m_TimerMap.Remove(key);
                }
                this.m_RemoveList.Clear();
            }
        }

        protected override void OnDispose()
        {
            foreach (var pair in this.m_TimerMap)
            {
                pair.Value.Dispose();
            }

            this.m_TimerMap = null;

            this.m_RemoveList.Clear();
            this.m_RemoveList = null;
        }
        #endregion
    }


    /// <summary>
    /// ��ʱ���࣬��TimerManager���ʹ��
    /// </summary>
    public class Timer
    {
        public bool ShouldInvoke { get { return this.m_fTimeCount > this.Tick; } }
        public bool ShouldRemove { get; set; }
        public bool InvokeWhenRemove { get; set; }

        public TimerCallback Callback { get; set; }
        public float Tick { get; set; }

        private float m_fTimeCount;
        private bool m_bPause;


        public Timer(float pTick, TimerCallback pCallback, bool pInvokeNow)
        {
            this.Tick = pTick;
            this.Callback = pCallback;
            this.m_fTimeCount = pInvokeNow ? pTick : 0;
            this.m_bPause = false;
            this.ShouldRemove = false;
            this.InvokeWhenRemove = false;
        }


        public void Update(float pDeltaTime)
        {
            if (this.m_bPause)
            {
                return;
            }

            this.m_fTimeCount += pDeltaTime;
        }


        public void Pause()
        {
            this.m_bPause = true;
        }


        public void Resume()
        {
            this.m_bPause = false;
        }


        public void Reset() 
        {
            this.m_fTimeCount = 0;
        }


        /// <summary>
        /// ʹ��ʱ���ﵽ��ֵ
        /// </summary>
        public void Full()
        {
            this.m_fTimeCount = this.Tick;
        }


        public void Invoke()
        {
            if (this.Callback != null)
            {
                this.Callback.Invoke();
            }
            this.m_fTimeCount = 0;
        }


        public void Dispose()
        {
            if (this.InvokeWhenRemove && this.Callback != null)
            {
                this.Callback.Invoke();
            }
            this.Callback = null;
        }
    }


    public delegate void TimerCallback();
}