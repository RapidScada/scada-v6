using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Scheme.Model
{
    /// <summary>
    /// 元素对齐
    /// </summary>
    public enum CompAlignEnum
    {
        /// <summary>
        /// 左
        /// </summary>
        Left = 0,

        /// <summary>
        /// 右
        /// </summary>
        Right = 1,

        /// <summary>
        /// 上
        /// </summary>
        Top = 2,

        /// <summary>
        /// 下
        /// </summary>
        Bottom = 3,

        /// <summary>
        /// 水平
        /// </summary>
        Verticle = 4,

        /// <summary>
        /// 垂直
        /// </summary>
        Center = 5,

        /// <summary>
        /// 左右均匀
        /// </summary>
        PicSide = 6,

        /// <summary>
        /// 上下均匀
        /// </summary>
        PicCenter = 7
    }
}
