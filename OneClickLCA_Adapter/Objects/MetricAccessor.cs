using System;
using System.Collections.Generic;
using System.Text;

namespace BH.Adapter.OneClickLCA.Objects
{
    public class MetricAccessor
    {
        public virtual Type Type { get; set; }

        public virtual string Name { get; set; } = "";

        public virtual double Factor { get; set; } = 1;
    }
}
