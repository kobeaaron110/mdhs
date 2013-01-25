using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace ImportLibrary
{
    public class FieldsCollection : Collection<string>
    {
        private bool _StopEvent = false;
        /// <summary>
        /// 發生於項目改變時。
        /// </summary>
        public event EventHandler ItemsChanged;
        /// <summary>
        /// 大量新增
        /// </summary>
        /// <param name="collection">新增項目集合</param>
        public new void AddRange(params string[] collection)
        {
            AddRange((IEnumerable<string>)collection);
        }
        /// <summary>
        /// 大量新增
        /// </summary>
        /// <param name="collection">新增項目集合</param>
        public new void AddRange(IEnumerable<string> collection)
        {
            bool hasInsert = false;
            _StopEvent = true;
            foreach (string s in collection)
            {
                this.Add(s);
                hasInsert = true;
            }
            _StopEvent = false;
            if (hasInsert && ItemsChanged != null)
                ItemsChanged(this, new EventArgs());
        }
        protected override void ClearItems()
        {
            base.ClearItems();
            if (!_StopEvent && ItemsChanged != null)
                ItemsChanged(this, new EventArgs());
        }
        protected override void InsertItem(int index, string item)
        {
            base.InsertItem(index, item);
            if (!_StopEvent && ItemsChanged != null)
                ItemsChanged(this, new EventArgs());
        }
        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);
            if (!_StopEvent && ItemsChanged != null)
                ItemsChanged(this, new EventArgs());
        }
        protected override void SetItem(int index, string item)
        {
            base.SetItem(index, item);
            if (!_StopEvent && ItemsChanged != null)
                ItemsChanged(this, new EventArgs());
        }
    }
}
