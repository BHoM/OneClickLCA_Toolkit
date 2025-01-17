using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.OneClickLCA.Enums
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CategoryLevelAttribute : Attribute, IImmutable, IObject
    {
        public virtual int Level { get; private set; } = 0;


        public CategoryLevelAttribute(int level)
        {
            Level = level;
        }
    }
}
