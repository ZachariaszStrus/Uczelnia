using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Project_4___Forms
{
    class PropertyComparer<T> : IComparer<T>
    {
        private PropertyDescriptor sortProperty;
        private ListSortDirection sortDirection;
        
        public PropertyComparer(PropertyDescriptor property, ListSortDirection direction)
        {
            sortProperty = property;
            sortDirection = direction;
        }

        public int Compare(T x, T y)
        {
            var v1 = x.GetType().GetProperty(sortProperty.Name).GetValue(x, null) as IComparable;
            var v2 = y.GetType().GetProperty(sortProperty.Name).GetValue(y, null) as IComparable;

            if (sortDirection == ListSortDirection.Ascending)
            {
                return v1.CompareTo(v2);
            }
            else
            {
                return v2.CompareTo(v1);
            }
        }
    }
}
