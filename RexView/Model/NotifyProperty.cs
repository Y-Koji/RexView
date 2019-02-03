using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RexView.Model
{
    public class NotifyProperty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IDictionary<string, object> ValuePool { get; } = new Dictionary<string, object>();

        protected T GetValue<T>([CallerMemberName] string propName = "")
        {
            if (ValuePool.ContainsKey(propName))
            {
                return (T)ValuePool[propName];
            }
            else
            {
                if (typeof(T) == typeof(string))
                {
                    ValuePool[propName] = string.Empty;
                }
                else if (typeof(T) == typeof(int))
                {
                    ValuePool[propName] = 0;
                }
                else
                {
                    ValuePool[propName] = default(T);
                }

                return (T)ValuePool[propName];
            }
        }

        protected void SetValue<T>(T value, Action foward = null, [CallerMemberName] string  propName = "")
        {
            ValuePool[propName] = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));

            foward?.Invoke();
        }

        public void SetHandler(string propName, Action handler)
        {
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == propName)
                {
                    handler?.Invoke();
                }
            };
        }
    }
}
