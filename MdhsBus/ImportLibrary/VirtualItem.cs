using System;
using System.Collections.Generic;
using System.Text;

namespace ImportLibrary
{
    /// <summary>
    /// 自行定義的按紐
    /// </summary>
    public class VirtualItem
    {
        private string _Description;

        /// <summary>
        /// 取得或設定與Item關聯的文字描述。
        /// </summary>
        public virtual string Description
        {
            get { return _Description; }
            set { _Description = value; if (DescriptionChanged != null)DescriptionChanged(this, new EventArgs()); }
        }

        private string _Text;

        /// <summary>
        /// 取得或設定與Item關聯的文字標籤。
        /// </summary>
        public virtual string Text
        {
            get { return _Text; }
            set { _Text = value; if (TextChanged != null)TextChanged(this, new EventArgs()); }
        }
        /// <summary>
        /// 發生於Description屬性變更時。
        /// </summary>
        public event EventHandler DescriptionChanged;
        /// <summary>
        /// 發生於Text屬性變更時。
        /// </summary>
        public event EventHandler TextChanged;
        private bool _Enabled = true;

        public event EventHandler EnabledChanged;

        /// <summary>
        /// 取得或設定Item是否可回應使用者互動。
        /// </summary>
        public bool Enabled
        {
            get { return _Enabled; }
            set { _Enabled = value; if (EnabledChanged != null)EnabledChanged(this, new EventArgs()); }
        }

        private bool _Visible = true;
        /// <summary>
        /// 發生於Visible屬性變更時。
        /// </summary>
        public event EventHandler VisibleChanged;

        /// <summary>
        /// 取得或設定值，指出是否顯示Item。
        /// </summary>
        public bool Visible
        {
            get { return _Visible; }
            set { _Visible = value; if (VisibleChanged != null)VisibleChanged(this, new EventArgs()); }
        }
    }
}
