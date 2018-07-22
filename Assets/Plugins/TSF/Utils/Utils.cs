//————————————————————————————————————————————
//  Utils.cs
//  For project: TooSimple Framework
//
//  Created by Chiyu Ren on 2016-06-17 18:41
//————————————————————————————————————————————

using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using DG.Tweening;


namespace TooSimpleFramework.Utils
{
    /// <summary>
    /// 各种工具方法、扩展
    /// </summary>
    public static class Utils
    {
        #region Transform Extensions
        /// <summary>
        /// 强制获取指定Transform上的组件。若组件不存在，则添加
        /// </summary>
        public static T ForceGetComponent<T>(this Transform pTransform) where T : Component
        {
            if (pTransform == null)
            {
                return null;
            }

            T ret;

            ret = pTransform.GetComponent<T>();
            if (ret == null)
            {
                ret = pTransform.gameObject.AddComponent<T>();
            }

            return ret;
        }

        /// <summary>
        /// 尝试获取指定名称的子级Transform，若实例为null则返回null
        /// </summary>
        public static Transform TryFindChild(this Transform pTransform, string pName)
        {
            return pTransform == null
                ? null
                : pTransform.Find(pName);
        }

        /// <summary>
        /// 尝试获取指定子级上的指定组件，若实例为null或子级不存在则返回null
        /// </summary>
        public static T TryFindComponent<T>(this Transform pTransform, string pName) where T : Component
        {
            T ret = null;
            var trans = pTransform.TryFindChild(pName);
            if (trans != null)
            {
                ret = trans.GetComponent<T>();
            }
            return ret;
        }

        /// <summary>
        /// 设置指定Transform的localScale为1、localPosition为0、localRotation为0
        /// </summary>
        public static void ToDefaultSize(this Transform pTransform)
        {
            if (pTransform == null)
            {
                return;
            }
            pTransform.localScale = Vector3.one;
            pTransform.localPosition = Vector3.zero;
            pTransform.localRotation = Quaternion.identity;
        }

        /// <summary>
        /// 设置指定Transform的LocalScale（x、y、z)
        /// </summary>
        public static void SetLocalScale(this Transform pTransform, float pScale)
        {
            if (pTransform == null)
            {
                return;
            }
            pTransform.localScale = new Vector3(pScale, pScale, pScale);
        }

        /// <summary>
        /// 设置指定Transform的LocalScale的X
        /// </summary>
        public static void SetLocalScaleX(this Transform pTransform, float pScale)
        {
            if (pTransform == null)
            {
                return;
            }
            var s = pTransform.localScale;
            s.x = pScale;
            pTransform.localScale = s;
        }

        /// <summary>
        /// 设置指定Transform的LocalScale的Y
        /// </summary>
        public static void SetLocalScaleY(this Transform pTransform, float pScale)
        {
            if (pTransform == null)
            {
                return;
            }
            var s = pTransform.localScale;
            s.y = pScale;
            pTransform.localScale = s;
        }

        /// <summary>
        /// 设置指定Transform的LocalScale的Z
        /// </summary>
        public static void SetLocalScaleZ(this Transform pTransform, float pScale)
        {
            if (pTransform == null)
            {
                return;
            }
            var s = pTransform.localScale;
            s.z = pScale;
            pTransform.localScale = s;
        }

        /// <summary>
        /// 设置指定Transform的LocalScale的X和Y
        /// </summary>
        public static void SetLocalScaleXY(this Transform pTransform, float pScaleX, float pScaleY)
        {
            if (pTransform == null)
            {
                return;
            }
            var s = pTransform.localScale;
            s.x = pScaleX;
            s.y = pScaleY;
            pTransform.localScale = s;
        }

        /// <summary>
        /// 设定指定Transform的X坐标
        /// </summary>
        public static void SetPositionX(this Transform pTransform, float pPosX)
        {
            if (pTransform == null)
            {
                return;
            }
            var p = pTransform.position;
            p.x = pPosX;
            pTransform.position = p;
        }

        /// <summary>
        /// 设定指定Transform的Y坐标
        /// </summary>
        public static void SetPositionY(this Transform pTransform, float pPosY)
        {
            if (pTransform == null)
            {
                return;
            }
            var p = pTransform.position;
            p.y = pPosY;
            pTransform.position = p;
        }

        /// <summary>
        /// 设定指定Transform的Z坐标
        /// </summary>
        public static void SetPositionZ(this Transform pTransform, float pPosZ)
        {
            if (pTransform == null)
            {
                return;
            }
            var p = pTransform.position;
            p.z = pPosZ;
            pTransform.position = p;
        }

        /// <summary>
        /// 设定指定Transform的Local X坐标
        /// </summary>
        public static void SetLocalPositionX(this Transform pTransform, float pPosX)
        {
            if (pTransform == null)
            {
                return;
            }
            var p = pTransform.localPosition;
            p.x = pPosX;
            pTransform.localPosition = p;
        }

        /// <summary>
        /// 设定指定Transform的Local Y坐标
        /// </summary>
        public static void SetLocalPositionY(this Transform pTransform, float pPosY)
        {
            if (pTransform == null)
            {
                return;
            }
            var p = pTransform.localPosition;
            p.y = pPosY;
            pTransform.localPosition = p;
        }

        /// <summary>
        /// 设定指定Transform的Local Z坐标
        /// </summary>
        public static void SetLocalPositionZ(this Transform pTransform, float pPosZ)
        {
            if (pTransform == null)
            {
                return;
            }
            var p = pTransform.localPosition;
            p.z = pPosZ;
            pTransform.localPosition = p;
        }

        /// <summary>
        /// 设定指定Transform的Local X和Y坐标
        /// </summary>
        public static void SetLocalPositionXY(this Transform pTransform, float pPosX, float pPosY)
        {
            if (pTransform == null)
            {
                return;
            }
            var p = pTransform.localPosition;
            p.x = pPosX;
            p.y = pPosY;
            pTransform.localPosition = p;
        }

        /// <summary>
        /// 设定指定Transform的Rotation
        /// </summary>
        public static void SetRotation(this Transform pTransform, Vector3 pRotate)
        {
            if (pTransform == null)
            {
                return;
            }
            pTransform.rotation = Quaternion.Euler(pRotate);
        }

        /// <summary>
        /// 设置指定Transform的Rotation的X
        /// </summary>
        public static void SetRotationX(this Transform pTransform, float pRotate)
        {
            if (pTransform == null)
            {
                return;
            }
            var r = pTransform.rotation.eulerAngles;
            r.x = pRotate;
            SetRotation(pTransform, r);
        }

        /// <summary>
        /// 设置指定Transform的Rotation的Y
        /// </summary>
        public static void SetRotationY(this Transform pTransform, float pRotate)
        {
            if (pTransform == null)
            {
                return;
            }
            var r = pTransform.rotation.eulerAngles;
            r.y = pRotate;
            SetRotation(pTransform, r);
        }

        /// <summary>
        /// 设置指定Transform的Rotation的Z
        /// </summary>
        public static void SetRotationZ(this Transform pTransform, float pRotate)
        {
            if (pTransform == null)
            {
                return;
            }
            var r = pTransform.rotation.eulerAngles;
            r.z = pRotate;
            SetRotation(pTransform, r);
        }

        /// <summary>
        /// 设定指定Transform的LocalRotation
        /// </summary>
        public static void SetLocalRotation(this Transform pTransform, Vector3 pRotate)
        {
            if (pTransform == null)
            {
                return;
            }
            pTransform.localRotation = Quaternion.Euler(pRotate);
        }

        /// <summary>
        /// 设置指定Transform的LocalRotation的X
        /// </summary>
        public static void SetLocalRotationX(this Transform pTransform, float pRotate)
        {
            if (pTransform == null)
            {
                return;
            }
            var r = pTransform.localRotation.eulerAngles;
            r.x = pRotate;
            SetLocalRotation(pTransform, r);
        }

        /// <summary>
        /// 设置指定Transform的LocalRotation的Y
        /// </summary>
        public static void SetLocalRotationY(this Transform pTransform, float pRotate)
        {
            if (pTransform == null)
            {
                return;
            }
            var r = pTransform.localRotation.eulerAngles;
            r.y = pRotate;
            SetLocalRotation(pTransform, r);
        }

        /// <summary>
        /// 设置指定Transform的LocalRotation的Z
        /// </summary>
        public static void SetLocalRotationZ(this Transform pTransform, float pRotate)
        {
            if (pTransform == null)
            {
                return;
            }
            var r = pTransform.localRotation.eulerAngles;
            r.z = pRotate;
            SetLocalRotation(pTransform, r);
        }
        #endregion


        #region RectTransform Extensions
        /// <summary>
        /// 设置指定RectTransform的大小
        /// </summary>
        public static void SetSize(this RectTransform pRT, float pWidth, float pHeight)
        {
            if (pRT == null) 
            {
                return;
            }
            pRT.sizeDelta = new Vector2(pWidth, pHeight);
        }

        /// <summary>
        /// 设置指定RectTransform的宽度
        /// </summary>
        public static void SetWidth(this RectTransform pRT, float pWidth) 
        {
            if (pRT == null)
            {
                return;
            }
            var s = pRT.sizeDelta;
            s.x = pWidth;
            pRT.sizeDelta = s;
        }

        /// <summary>
        /// 设置指定RectTransform的高度
        /// </summary>
        public static void SetHeight(this RectTransform pRT, float pHeight)
        {
            if (pRT == null)
            {
                return;
            }
            var s = pRT.sizeDelta;
            s.y = pHeight;
            pRT.sizeDelta = s;
        }

        /// <summary>
        /// 设置指定RectTransform的AnchoredPosition
        /// </summary>
        public static void SetAnchoredPosition(this RectTransform pRT, float pX, float pY)
        {
            if (pRT == null)
            {
                return;
            }
            pRT.anchoredPosition = new Vector2(pX, pY);
        }

        /// <summary>
        /// 设置指定RectTransform的AnchoredPosition的X
        /// </summary>
        public static void SetAnchoredPositionX(this RectTransform pRT, float pValue)
        {
            if (pRT == null)
            {
                return;
            }
            var pos = pRT.anchoredPosition;
            pos.x = pValue;
            pRT.anchoredPosition = pos;
        }

        /// <summary>
        /// 设置指定RectTransform的AnchoredPosition的Y
        /// </summary>
        public static void SetAnchoredPositionY(this RectTransform pRT, float pValue)
        {
            if (pRT == null)
            {
                return;
            }
            var pos = pRT.anchoredPosition;
            pos.y = pValue;
            pRT.anchoredPosition = pos;
        }

        /// <summary>
        /// 设置指定RectTransform的AnchoreMin
        /// </summary>
        public static void SetAnchoreMin(this RectTransform pRT, float pX, float pY)
        {
            if (pRT == null)
            {
                return;
            }
            pRT.anchorMin = new Vector2(pX, pY);
        }

        /// <summary>
        /// 设置指定RectTransform的AnchoreMin的X
        /// </summary>
        public static void SetAnchoreMinX(this RectTransform pRT, float pValue)
        {
            if (pRT == null)
            {
                return;
            }
            var pos = pRT.anchorMin;
            pos.x = pValue;
            pRT.anchorMin = pos;
        }

        /// <summary>
        /// 设置指定RectTransform的AnchoreMin的Y
        /// </summary>
        public static void SetAnchoreMinY(this RectTransform pRT, float pValue)
        {
            if (pRT == null)
            {
                return;
            }
            var pos = pRT.anchorMin;
            pos.y = pValue;
            pRT.anchorMin = pos;
        }

        /// <summary>
        /// 设置指定RectTransform的Pivot
        /// </summary>
        public static void SetPivot(this RectTransform pRT, float pX, float pY)
        {
            if (pRT == null)
            {
                return;
            }
            pRT.pivot = new Vector2(pX, pY);
        }

        /// <summary>
        /// 设置指定RectTransform的AnchoreMax
        /// </summary>
        public static void SetAnchoreMax(this RectTransform pRT, float pX, float pY)
        {
            if (pRT == null)
            {
                return;
            }
            pRT.anchorMax = new Vector2(pX, pY);
        }

        /// <summary>
        /// 设置指定RectTransform的AnchoreMax的X
        /// </summary>
        public static void SetAnchoreMaxX(this RectTransform pRT, float pValue)
        {
            if (pRT == null)
            {
                return;
            }
            var pos = pRT.anchorMax;
            pos.x = pValue;
            pRT.anchorMax = pos;
        }

        /// <summary>
        /// 设置指定RectTransform的AnchoreMax的Y
        /// </summary>
        public static void SetAnchoreMaxY(this RectTransform pRT, float pValue)
        {
            if (pRT == null)
            {
                return;
            }
            var pos = pRT.anchorMax;
            pos.y = pValue;
            pRT.anchorMax = pos;
        }

        /// <summary>
        /// 设置指定RectTransform的Pivot的X
        /// </summary>
        public static void SetPivotX(this RectTransform pRT, float pValue)
        {
            if (pRT == null)
            {
                return;
            }
            var pos = pRT.pivot;
            pos.x = pValue;
            pRT.pivot = pos;
        }

        /// <summary>
        /// 设置指定RectTransform的Pivot的Y
        /// </summary>
        public static void SetPivotY(this RectTransform pRT, float pValue)
        {
            if (pRT == null)
            {
                return;
            }
            var pos = pRT.pivot;
            pos.y = pValue;
            pRT.pivot = pos;
        }
        #endregion


        #region GameObject Extensions
        /// <summary>
        /// 强制获取指定GameObject上的组件。若组件不存在，则添加
        /// </summary>
        public static T ForceGetComponent<T>(this GameObject pGameObject) where T : Component
        {
            if (pGameObject == null)
            {
                return null;
            }

            T ret;

            ret = pGameObject.GetComponent<T>();
            if (ret == null)
            {
                ret = pGameObject.AddComponent<T>();
            }

            return ret;
        }

        /// <summary>
        /// 获取指定GameObject的拷贝，拷贝出的对象和源GameObject的大小和坐标相同，处于同一父节点下
        /// </summary>
        public static GameObject Copy(this GameObject pGameObject)
        {
            if (pGameObject == null)
            {
                return null;
            }
            return Copy(pGameObject, pGameObject.transform.parent);
        }

        /// <summary>
        /// 获取指定GameObject的拷贝，拷贝出的对象和源GameObject的大小和坐标相同，处于指定父节点下
        /// </summary>
        public static GameObject Copy(this GameObject pGameObject, Transform pParent)
        {
            if (pGameObject == null)
            {
                return null;
            }
            var ret = Object.Instantiate(pGameObject);
            var ot = pGameObject.transform;
            var t = ret.transform;
            t.SetParent(pParent == null ? ot.parent : pParent);
            t.localPosition = ot.localPosition;
            t.localScale = ot.localScale;
            t.localRotation = ot.localRotation;
            return ret;
        }
        #endregion


        #region BigEndian Extensions
        public static short ToBigEndian(this short v)
        {
            return IPAddress.HostToNetworkOrder(v);
        }
        public static int ToBigEndian(this int v)
        {
            return IPAddress.HostToNetworkOrder(v);
        }
        public static long ToBigEndian(this long v)
        {
            return IPAddress.HostToNetworkOrder(v);
        }
        public static short FromBigEndian(this short v)
        {
            return IPAddress.NetworkToHostOrder(v);
        }
        public static int FromBigEndian(this int v)
        {
            return IPAddress.NetworkToHostOrder(v);
        }
        public static long FromBigEndian(this long v)
        {
            return IPAddress.NetworkToHostOrder(v);
        }
        #endregion


        #region Other Extensions
        /// <summary>
        /// 设置组件是否可见
        /// </summary>
        public static void SetActive(this Component pComponent, bool pActive)
        {
            if (pComponent != null)
            {
                pComponent.gameObject.SetActive(pActive);
            }
        }

        /// <summary>
        /// 获取组件是否可见
        /// </summary>
        public static bool GetActiveSelf(this Component pComponent)
        {
            return pComponent == null ? false : pComponent.gameObject.activeSelf;
        }

        /// <summary>
        /// 将字符串转换为颜色，字符串格式为FF0000（不带Alpha）或FF0000FF（带Alpha）
        /// </summary>
        public static Color ToColor(this string pStr)
        {
            var color = new Color();

            color.r = System.Convert.ToInt32(pStr.Substring(0, 2), 16) / 255.0f;
            color.g = System.Convert.ToInt32(pStr.Substring(2, 2), 16) / 255.0f;
            color.b = System.Convert.ToInt32(pStr.Substring(4, 2), 16) / 255.0f;

            if (pStr.Length == 8)
            {
                color.a = System.Convert.ToInt32(pStr.Substring(6, 2), 16) / 255.0f;
            }
            else
            {
                color.a = 1f;
            }

            return color;
        }

        /// <summary>
        /// 设置Alpha渐变动画
        /// </summary>
        public static Tweener DOFade(this Graphic target, float endValue, float duration)
        {
            return DOTween.To(
                () => target.color.a,
                (v) => { var c = target.color; c.a = v; target.color = c; },
                endValue,
                duration);
        }

        /// <summary>
        /// 设置Alpha渐变动画
        /// </summary>
        public static Tweener DOFade(this CanvasGroup target, float endValue, float duration)
        {
            return DOTween.To(
                () => target.alpha,
                (v) => { target.alpha = v; },
                endValue,
                duration);
        }

        /// <summary>
        /// 获取字符串的MD5（大写）
        /// </summary>
        public static string GetMD5(this string pSrc)
        {
            return Encoding.Unicode.GetBytes(pSrc).GetMD5();
        }

        /// <summary>
        /// 获取字节数据的MD5（大写）
        /// </summary>
        public static string GetMD5(this byte[] pSrc)
        {
            var csp = new MD5CryptoServiceProvider();
            pSrc = csp.ComputeHash(pSrc);
            var sb = new StringBuilder();
            for (int i = 0, count = pSrc.Length; i < count; i++)
            {
                sb.Append(pSrc[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 设置指定Graphic的透明度（0~1）
        /// </summary>
        public static void SetAlpha(this Graphic pGraphic, float pValue)
        {
            if (pGraphic == null)
            {
                return;
            }

            var c = pGraphic.color;
            c.a = Mathf.Clamp01(pValue);
            pGraphic.color = c;
        }


        //private static readonly Material m_GrayMaterial = new Material(Shader.Find("UI/Transparent Colored Gray"));
        ///// <summary>
        ///// 设置图像是否为黑白
        ///// </summary>
        //public static void SetGray(this Image pImg, bool pIsGray)
        //{
        //    if (pImg == null)
        //    {
        //        return;
        //    }

        //    pImg.material = pIsGray ? m_GrayMaterial : null;
        //}

        ///// <summary>
        ///// 设置按钮图像是否为黑白
        ///// </summary>
        //public static void SetGray(this Button pBtn, bool pIsGray, bool pSetInteractable = true)
        //{
        //    if (pBtn == null)
        //    {
        //        return;
        //    }

        //    if (pSetInteractable)
        //    {
        //        pBtn.interactable = !pIsGray;
        //    }

        //    pBtn.GetComponent<Image>().SetGray(pIsGray);
        //}


        //private static readonly Material m_HighLightMaterial = new Material(Shader.Find("UI/UIColoredHightLight"));
        ///// <summary>
        ///// 设置图像是否为高亮
        ///// </summary>
        //public static void SetHighLight(this Image pImg, bool pHighLight)
        //{
        //    if (pImg == null)
        //    {
        //        return;
        //    }

        //    pImg.material = pHighLight ? m_HighLightMaterial : null;
        //}

        /// <summary>
        /// 获取指定Component的拷贝，拷贝出的对象和源Component的大小和坐标相同，处于同一父节点下
        /// </summary>
        public static T Copy<T>(this T pComponent) where T : Component
        {
            if (pComponent == null)
            {
                return null;
            }
            return Copy(pComponent, pComponent.transform.parent);
        }

        /// <summary>
        /// 获取指定pComponent的拷贝，拷贝出的对象和源pComponent的大小和坐标相同，处于指定父节点下
        /// </summary>
        public static T Copy<T>(this T pComponent, Transform pParent) where T : Component
        {
            if (pComponent == null)
            {
                return null;
            }
            return pComponent.gameObject.Copy(pParent).GetComponent<T>();
        }

        /// <summary>
        /// 将指定字符串转换为int，若字符串为空或格式不符，则返回0
        /// </summary>
        public static int ToInt(this string pInput)
        {
            if (string.IsNullOrEmpty(pInput))
            {
                return 0;
            }
            else
            {
                var ret = 0;
                int.TryParse(pInput, out ret);
                return ret;
            }
        }

        /// <summary>
        /// 将指定字符串转换为float，若字符串为空或格式不符，则返回0
        /// </summary>
        public static float ToFloat(this string pInput)
        {
            if (string.IsNullOrEmpty(pInput))
            {
                return 0;
            }
            else
            {
                var ret = 0.0f;
                float.TryParse(pInput, out ret);
                return ret;
            }
        }

        /// <summary>
        /// 将指定字符串分割并转化为int数组
        /// </summary>
        public static int[] ToIntArray(this string pInput, char pSeparator = ';')
        {
            int[] ret = null;
            if (!string.IsNullOrEmpty(pInput))
            {
                var arr = pInput.Split(pSeparator);
                ret = new int[arr.Length];
                for (int i = 0, count = arr.Length; i < count; i++)
                {
                    ret[i] = arr[i].ToInt();
                }
            }
            return ret;
        }

        /// <summary>
        /// 将指定字符串分割并转化为float数组
        /// </summary>
        public static float[] ToFloatArray(this string pInput, char pSeparator = ';')
        {
            float[] ret = null;
            if (!string.IsNullOrEmpty(pInput))
            {
                var arr = pInput.Split(pSeparator);
                ret = new float[arr.Length];
                for (int i = 0, count = arr.Length; i < count; i++)
                {
                    ret[i] = arr[i].ToFloat();
                }
            }
            return ret;
        }

        /// <summary>
        /// 获取列表的第一项
        /// </summary>
        public static T GetFirst<T>(this IList<T> pList)
        {
            if (pList == null || pList.Count == 0)
            {
                return default(T);
            }
            return pList[0];
        }

        /// <summary>
        /// 设置列表的第一项
        /// </summary>
        public static void SetFirst<T>(this IList<T> pList, T pValue)
        {
            if (pList == null || pList.Count == 0)
            {
                return;
            }
            pList[0] = pValue;
        }

        /// <summary>
        /// 获取列表的最后一项
        /// </summary>
        public static T GetLast<T>(this IList<T> pList)
        {
            if (pList == null || pList.Count == 0)
            {
                return default(T);
            }
            return pList[pList.Count - 1];
        }

        /// <summary>
        /// 设置列表的最后一项
        /// </summary>
        public static void SetLast<T>(this IList<T> pList, T pValue)
        {
            if (pList == null || pList.Count == 0)
            {
                return;
            }
            pList[pList.Count - 1] = pValue;
        }

        /// <summary>
        /// 获取列表的随机索引
        /// </summary>
        public static int GetRandomIndex<T>(this IList<T> pList)
        {
            if (pList == null || pList.Count == 0)
            {
                return -1;
            }
            if (pList.Count == 1)
            {
                return 0;
            }
            return Random.Range(0, pList.Count);
        }

        /// <summary>
        /// 获取列表中的随机项
        /// </summary>
        public static T GetRandomItem<T>(this IList<T> pList)
        {
            if (pList == null || pList.Count == 0)
            {
                return default(T);
            }
            if (pList.Count == 1)
            {
                return pList[0];
            }
            return pList[pList.GetRandomIndex()];
        }

        /// <summary>
        /// 交换列表中的两个索引的元素
        /// </summary>
        public static void Swap<T>(this IList<T> pList, int pIndex1, int pIndex2)
        {
            if (pList == null || pList.Count == 0
                || pIndex1 < 0 || pIndex1 >= pList.Count
                || pIndex2 < 0 || pIndex2 >= pList.Count)
            {
                return;
            }

            var item = pList[pIndex1];
            pList[pIndex1] = pList[pIndex2];
            pList[pIndex2] = item;
        }

        /// <summary>
        /// 顺序遍历列表。当回调返回true时跳出循环
        /// </summary>
        public static void Traversal_Break<T>(this IList<T> pList, System.Func<T, bool> pCallback)
        {
            if (pList == null)
            {
                return;
            }
            Traversal_Break(pList, pCallback, 0, pList.Count - 1);
        }

        /// <summary>
        /// 倒序遍历列表。当回调返回true时跳出循环
        /// </summary>
        public static void Traversal_Reverse_Break<T>(this IList<T> pList, System.Func<T, bool> pCallback)
        {
            if (pList == null)
            {
                return;
            }
            Traversal_Break(pList, pCallback, pList.Count - 1, 0);
        }

        /// <summary>
        /// 根据指定区间[指定索引, 结束索引]依次访问列表项。当回调返回true时跳出循环
        /// </summary>
        public static void Traversal_Break<T>(this IList<T> pList, System.Func<T, bool> pCallback, int pStartIndex, int pEndIndex)
        {
            if (pList == null || pList.Count == 0 || pCallback == null)
            {
                return;
            }

            var offset = pStartIndex < pEndIndex ? 1 : -1;
            pEndIndex += offset;
            while (pStartIndex != pEndIndex)
            {
                if (pCallback.Invoke(pList[pStartIndex]))
                {
                    break;
                }
                pStartIndex += offset;
            }
        }

        /// <summary>
        /// 顺序遍历列表
        /// </summary>
        public static void Traversal<T>(this IList<T> pList, System.Action<T> pCallback)
        {
            if (pList == null)
            {
                return;
            }
            Traversal(pList, pCallback, 0, pList.Count - 1);
        }

        /// <summary>
        /// 倒序遍历列表
        /// </summary>
        public static void Traversal_Reverse<T>(this IList<T> pList, System.Action<T> pCallback)
        {
            if (pList == null)
            {
                return;
            }
            Traversal(pList, pCallback, pList.Count - 1, 0);
        }

        /// <summary>
        /// 根据指定区间[指定索引, 结束索引]依次访问列表项
        /// </summary>
        public static void Traversal<T>(this IList<T> pList, System.Action<T> pCallback, int pStartIndex, int pEndIndex)
        {
            if (pList == null || pList.Count == 0 || pCallback == null)
            {
                return;
            }

            var offset = pStartIndex < pEndIndex ? 1 : -1;
            pEndIndex += offset;
            while (pStartIndex != pEndIndex)
            {
                pCallback.Invoke(pList[pStartIndex]);
                pStartIndex += offset;
            }
        }
        #endregion


        /// <summary>
        /// 获取百分比值
        /// </summary>
        public static float GetPercent(int pValue)
        {
            return pValue * 0.01f;
        }


        /// <summary>
        /// 获取万分比值
        /// </summary>
        public static float GetPertenthousand(int pValue)
        {
            return pValue * 0.0001f;
        }


        /// <summary>
        /// 获取百分比概率
        /// </summary>
        public static bool GetIsPercentRate(int pRate)
        {
            if (pRate <= 0)
            {
                return false;
            }
            else if (pRate >= 100)
            {
                return true;
            }
            return Random.Range(1, 100) <= pRate;
        }


        /// <summary>
        /// 获取万分比概率
        /// </summary>
        public static bool GetIsPercenthousandRate(int pRate)
        {
            if (pRate <= 0)
            {
                return false;
            }
            else if (pRate >= 10000)
            {
                return true;
            }
            return Random.Range(1, 10000) <= pRate;
        }


        /// <summary>
        /// 秒转换为毫秒
        /// </summary>
        public static int Second2MilliSecond(float pSecond)
        {
            return Mathf.CeilToInt(pSecond * 1000);
        }


        /// <summary>
        /// 毫秒转换为秒
        /// </summary>
        public static float MilliSecond2Second(int pMilliSecond)
        {
            return pMilliSecond * 0.001f;
        }


        /// <summary>
        /// 按位计算是否包含
        /// </summary>
        public static bool BitContains(this int pSrcValue, int pValue)
        {
            return (pSrcValue & pValue) == pValue;
        }
    }
}