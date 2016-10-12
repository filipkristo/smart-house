using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Template10.Mvvm;

namespace SmartHouse.UWPClient.ViewModels
{
    public class BaseViewModel : ViewModelBase
    {
        private Dictionary<Object, Object> propertyValues = new Dictionary<Object, Object>();

        public string Status { get { return Get<string>(); } set { Set(value); } }

        protected virtual Boolean Set(object value, [CallerMemberName] string name = "")
        {
            if (propertyValues.ContainsKey(name))
            {
                var oldValue = propertyValues[name];
                if (oldValue == null || !oldValue.Equals(value))
                {
                    propertyValues[name] = value;

                    RaisePropertyChanged(name);
                    return true;
                }
            }
            else
            {
                propertyValues.Add(name, value);

                RaisePropertyChanged(name);
                return true;
            }

            return false;
        }

        protected virtual T Get<T>([CallerMemberName] string name = "")
        {
            if (propertyValues.ContainsKey(name))
                return (T)propertyValues[name];
            else
                return default(T);
        }
    }
}
