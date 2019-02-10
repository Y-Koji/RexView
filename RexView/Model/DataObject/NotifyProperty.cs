using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace RexView.Model.DataObject
{
    [DataContract]
    public class NotifyProperty : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private IDictionary<string, object> ValuePool { get; set; } = null;

        protected T GetValue<T>(T initialValue = default(T), [CallerMemberName] string propName = "")
        {
            if (null == ValuePool)
            {
                ValuePool = new Dictionary<string, object>();
            }

            if (ValuePool.ContainsKey(propName))
            {
                return (T)ValuePool[propName];
            }
            else
            {
                ValuePool[propName] = initialValue;

                return (T)ValuePool[propName];
            }
        }

        protected void SetValue<T>(T value, Action foward = null, [CallerMemberName] string  propName = "")
        {
            if (null == ValuePool)
            {
                ValuePool = new Dictionary<string, object>();
            }

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
