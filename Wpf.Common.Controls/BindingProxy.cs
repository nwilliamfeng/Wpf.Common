/**************************************************************************
*   Copyright (c) QiCheng Tech Corporation.  All rights reserved.
*   http://www.iqichengtech.com 
*   
*   =================================
*   CLR版本  ：4.0.30319.42000
*   命名空间 ：QiCheng.QCTrader.Controls
*   文件名称 ：BindingProxy.cs
*   =================================
*   创 建 者 ：wei.feng
*   创建日期 ：2019/10/14 16:39:44 
*   功能描述 ：
*   使用说明 ：
*   =================================
*   修 改 者 ：
*   修改日期 ：
*   修改内容 ：
*   =================================
*  
***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QiCheng.QCTrader.Controls
{
    /// <summary>
    /// <see cref="https://stackoverflow.com/questions/22073740/binding-visibility-for-datagridcolumn-in-wpf"/>
    /// </summary>
    public class BindingProxy : Freezable
    {
        #region Overrides of Freezable

        protected override Freezable CreateInstanceCore()
        {
            return new BindingProxy();
        }

        #endregion

        public object Data
        {
            get { return (object)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(object),
                                         typeof(BindingProxy));
    }
}
