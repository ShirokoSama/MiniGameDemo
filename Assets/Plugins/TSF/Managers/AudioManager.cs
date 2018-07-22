//————————————————————————————————————————————
//  AudioManager.cs
//
//  Created by Chiyu Ren on 2016-09-17 23:17
//————————————————————————————————————————————

using DG.Tweening;
using System.Collections.Generic;
using TooSimpleFramework.Common;
using TooSimpleFramework.Utils;
using UnityEngine;
using UnityEngine.Events;


namespace TooSimpleFramework.Components.Managers
{
    /// <summary>
    /// 音频管理器，用于播放背景音乐和音效
    /// </summary>
    public class AudioManager : BaseManager<AudioManager>
    {
        #region Properties
        public float BGMVolume
        {
            get
            {
                return this.m_fBGMVolume;
            }
            set
            {
                this.m_fBGMVolume = value;
                if (this.m_CurBGM != null)
                {
                    this.m_CurBGM.volume = this.m_fBGMVolume;
                }
            }
        }
        public float SNDVolume
        {
            get
            {
                return this.m_fSNDVolume;
            }
            set
            {
                this.m_fSNDVolume = value;
                this.m_AudioObjectPool.Traversal((item) =>
                {
                    item.volume = this.m_fSNDVolume;
                });
            }
        }
        #endregion


        #region Private Members
        private Transform m_AudioRoot;
        private List<AudioSource> m_AudioObjectPool;
        private AudioSource m_CurBGM = null;
        private Sequence m_Sequence = null;
        private float m_fBGMVolume;
        private float m_fSNDVolume;
        #endregion


        #region Constants
        private const string TIMER_KEY_AUDMGR = "Timer_AudioManager";
        private const int TIMER_TICK = 5; // 每5秒检测一次AB的引用计数
        #endregion


        #region Public Methods
        /// <summary>
        /// 播放指定BGM，调用后会终止当前BGM的播放，返回挂载了AudioSource的对象
        /// </summary>
        public GameObject PlayBGM(AudioClip pMusic)
        {
            if (pMusic == null)
            {
                return null;
            }

            this.m_CurBGM.Stop();

            this.m_CurBGM.clip = pMusic;
            this.m_CurBGM.playOnAwake = false;
            this.m_CurBGM.volume = this.m_fBGMVolume;
            this.m_CurBGM.loop = true;
            this.m_CurBGM.Play();

            return this.m_CurBGM.gameObject;
        }

        /// <summary>
        /// 渐入播放指定BGM，调用后当前BGM会渐出
        /// </summary>
        public void PlayBGMWithFade(AudioClip pMusic, UnityAction<GameObject, object> pPlayedCallback, object pParam, float pFadeDuration = 1.0f)
        {
            if (pMusic == null)
            {
                if (pPlayedCallback != null)
                {
                    pPlayedCallback.Invoke(null, pParam);
                }
                return;
            }

            if (this.m_Sequence != null)
            {
                this.m_Sequence.Kill();
            }

            this.m_Sequence = DOTween.Sequence();

            if (this.m_CurBGM.clip != null)
            {
                this.m_Sequence.Append(DOTween.To((v) => { this.m_CurBGM.volume = v; }, this.m_CurBGM.volume, 0, pFadeDuration));
            }

            this.m_Sequence.AppendCallback(() =>
            {
                this.m_CurBGM.Stop();

                if (pPlayedCallback != null)
                {
                    pPlayedCallback.Invoke(this.m_CurBGM.gameObject, pParam);
                }

                this.m_CurBGM.clip = pMusic;
                this.m_CurBGM.playOnAwake = false;
                this.m_CurBGM.volume = this.m_fBGMVolume;
                this.m_CurBGM.loop = true;
                this.m_CurBGM.Play();
            });

            this.m_Sequence.Append(DOTween.To((v) => { this.m_CurBGM.volume = v; }, this.m_CurBGM.volume, this.m_fBGMVolume, pFadeDuration));
            this.m_Sequence.AppendCallback(() =>
            {
                this.m_Sequence = null;
            });
        }

        /// <summary>
        /// 停止当前播放的BGM
        /// </summary>
        public void StopBGM(bool pFade, float pFadeDuration = 1.0f)
        {
            if (pFade)
            {
                DOTween.To((v) => { this.m_CurBGM.volume = v; }, this.m_CurBGM.volume, 0, pFadeDuration).OnComplete(this.m_CurBGM.Stop);
            }
            else
            {
                this.m_CurBGM.Stop();
            }
        }

        /// <summary>
        /// 播放指定音效，返回挂载了AudioSource的对象
        /// </summary>
        public GameObject PlaySND(AudioClip pClip)
        {
            if (pClip == null)
            {
                return null;
            }

            AudioSource ret = null;
            this.m_AudioObjectPool.Traversal_Break((item) => // 先从缓存池中找到没有播放的对象
            {
                if (!item.isPlaying)
                {
                    ret = item;
                    return true;
                }
                return false;
            });

            if (ret == null) // 缓存池已满时，创建新对象
            {
                ret = this._CreateObject("SNDObject");
                this.m_AudioObjectPool.Add(ret);
            }

            ret.clip = pClip;
            ret.volume = this.m_fSNDVolume;
            ret.Play();
            return ret.gameObject;
        }

        /// <summary>
        /// 停止播放所有音效
        /// </summary>
        public void StopAllSND()
        {
            this.m_AudioObjectPool.Traversal((item) =>
            {
                if (item.isPlaying)
                {
                    item.Stop();
                }
            });
        }

        /// <summary>
        /// 释放音频资源，在下一帧生效
        /// </summary>
        public void UnloadUnusedAssetBundles()
        {
            TimerManager.Instance.GetTimer(TIMER_KEY_AUDMGR).Full();
        }
        #endregion


        #region Protected Methods
        protected override void OnInitialize()
        {
            this.m_AudioRoot = new GameObject("AudioManager").transform;
            this.m_AudioRoot.SetParent(Framework.Instance.transform);
            this.m_AudioObjectPool = new List<AudioSource>();
            this.m_fBGMVolume = 1;
            this.m_fSNDVolume = 1;

            this.m_CurBGM = this._CreateObject("BGMObject");
            
            for (int i = 0; i < 3; i++)
            {
                var obj = this._CreateObject("SNDObject");
                this.m_AudioObjectPool.Add(obj);
            }

            TimerManager.Instance.AddTimer(TIMER_KEY_AUDMGR, TIMER_TICK, this._TimerUpdate);
        }

        protected override void OnDispose()
        {
            TimerManager.Instance.RemoveTimer(TIMER_KEY_AUDMGR);

            this.m_AudioRoot = null;
            this.m_CurBGM = null;
            this.m_AudioObjectPool.Clear();
            this.m_AudioObjectPool = null;
        }
        #endregion


        #region Private Methods
        private void _TimerUpdate()
        {
            for (int i = this.m_AudioObjectPool.Count - 1; i > 2; i--) // 前三个为保留项，不处理
            {
                var item = this.m_AudioObjectPool[i];
                if (!item.isPlaying)
                {
                    this.m_AudioObjectPool.RemoveAt(i);
                    Object.Destroy(item.gameObject);
                }
            }
        }

        private AudioSource _CreateObject(string pName)
        {
            var obj = new GameObject(pName);
            obj.transform.SetParent(this.m_AudioRoot);

            var ret = obj.AddComponent<AudioSource>();
            ret.playOnAwake = false;

            return ret;
        }
        #endregion
    }
}