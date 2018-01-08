using System;

namespace Kontur.ImageTransformer.Filters
{
    public class ThresholdParam : IImageFilterParam
    {
        private int _value;

        public object Value
        {
            get => _value;
            set
            {
                var intVal = value is int i ? i : (int?) null;
                if (intVal == null) throw new NotSupportedException();

                _value = intVal.Value;
            }
        }
    }
}