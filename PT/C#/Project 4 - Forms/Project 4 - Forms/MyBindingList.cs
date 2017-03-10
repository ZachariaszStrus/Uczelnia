using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Project_4___Forms
{
    public class MyBindingList<T> : BindingList<T>
    {
        private bool _isSorted;
        ListSortDirection _currentDirection;

        public MyBindingList(IList<T> list) : base(list) { }

        protected override bool SupportsSortingCore
        {
            get
            {
                return true;
            }
        }

        protected override void ApplySortCore(PropertyDescriptor property, ListSortDirection direction)
        {
            if (_isSorted && _currentDirection == direction)
            {
                direction = direction == ListSortDirection.Ascending ? 
                    ListSortDirection.Descending : ListSortDirection.Ascending;
                                               
            }
            if (property.PropertyType.GetInterface("IComparable") != null)
            {
                List<Car> items = this.Items as List<Car>;

                if (items != null)
                {
                    var pc = new PropertyComparer<Car>(property, direction);
                    items.Sort(pc);
                    _currentDirection = direction;
                    _isSorted = true;
                }
                else
                {
                    _isSorted = false;
                }

                this.OnListChanged(
                  new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        protected override bool IsSortedCore
        {
            get { return _isSorted; }
        }

        protected override bool SupportsSearchingCore
        {
            get
            {
                return true;
            }
        }

        protected override int FindCore(PropertyDescriptor property, object key)
        {
            if (property == null) return -1;

            List<T> items = this.Items as List<T>;
            foreach (T item in items)
            {
                string value = property.GetValue(item).ToString();
                if (key.ToString() == value) return IndexOf(item);
            }
            return -1;
        }

        public int Find(PropertyDescriptor property, object key)
        {
            return FindCore(property, key);
        }
    }
}
