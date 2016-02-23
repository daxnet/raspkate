using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Raspkate.Controllers.Routing
{
    internal sealed class Route : IList<RouteItem>
    {
        private readonly List<RouteItem> items = new List<RouteItem>();

        internal Route() { }


        public int IndexOf(RouteItem item)
        {
            return items.IndexOf(item);
        }

        public void Insert(int index, RouteItem item)
        {
            items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            items.RemoveAt(index);
        }

        public RouteItem this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                items[index] = value;
            }
        }

        public void Add(RouteItem item)
        {
            items.Add(item);
        }

        public void Clear()
        {
            items.Clear();
        }

        public bool Contains(RouteItem item)
        {
            return items.Contains(item);
        }

        public void CopyTo(RouteItem[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(RouteItem item)
        {
            return items.Remove(item);
        }

        public IEnumerator<RouteItem> GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public bool TryGetValue(string input, out RouteValueCollection values)
        {
            values = new RouteValueCollection();
            var splittedInput = input.Split('/');
            if (this.Count == 0)
                return false;
            if (this.Count != splittedInput.Length)
                return false;
            int idx = 0;
            foreach(var splittedInputItem in splittedInput)
            {
                var literalRouteItem = this[idx] as LiteralRouteItem;
                if (literalRouteItem != null)
                {
                    if (literalRouteItem.Name == splittedInputItem)
                    {
                        idx++;
                        continue;
                    }
                    else
                        return false;
                }
                var parameterRouteItem = this[idx] as ParameterRouteItem;
                if (parameterRouteItem != null)
                {
                    var parameterValue = parameterRouteItem.GetValue(splittedInputItem);
                    if (parameterValue != null)
                    {
                        values.Add(parameterRouteItem.Name, parameterValue);
                        idx++;
                        continue;
                    }
                    else
                        return false;
                }
                idx++;
            }
            return true;
        }
    }
}
