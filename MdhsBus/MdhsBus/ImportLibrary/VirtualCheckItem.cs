using System;
using System.Collections.Generic;
using System.Text;

namespace ImportLibrary
{
    public abstract class VirtualCheckItem : VirtualItem
    {
        private bool _Checked;

        /// <summary>
        /// 發生於Checked屬性變更時。
        /// </summary>
        public event EventHandler CheckedChanged;
        /// <summary>
        /// 取得或設定值，指出是否已選取 CheckBox 。
        /// </summary>
        public virtual bool Checked
        {
            get { return _Checked; }
            set { _Checked = value; if (CheckedChanged != null)CheckedChanged(this, new EventArgs()); }
        }
    }
}
