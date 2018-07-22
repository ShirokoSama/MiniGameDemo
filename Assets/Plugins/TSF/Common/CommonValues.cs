//————————————————————————————————————————————
//  Constants.cs
//  For project: TooSimple Framework
//
//  Created by Chiyu Ren on 2016-06-17 18:42
//————————————————————————————————————————————
using System.Text;


namespace TooSimpleFramework.Common
{
    /// <summary>
    /// 框架需要的常量
    /// </summary>
    public sealed class CommonValues
    {    
        /// <summary>
        /// 获取或设置当前平台的AB后缀名
        /// </summary>
        public static string AssetBundle_Extension { get; set; }


        /// <summary>
        /// 获取或设置当前平台的AB存放文件夹名
        /// </summary>
        public static string AssetBundle_Folder { get; set; }


        /// <summary>
        /// 获取或设置应用数据的路径（可读写）
        /// </summary>
        public static string ApplicationDataPath { get; set; }


        /// <summary>
        /// 获取或设置文本编码格式
        /// </summary>
        public static Encoding TextEncoding { get; set; }


        /// <summary>
        /// 获取或设置是否使用AssetBundle
        /// </summary>
        public static bool UseAssetBundle { get; set; }
    }
}