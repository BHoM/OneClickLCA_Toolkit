using BH.oM.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace BH.oM.OneClickLCA.Enums
{
    [AttributeUsage(AttributeTargets.Field)]
    public class CategoryCodeAttribute : Attribute, IImmutable, IObject
    {
        public virtual string Code { get; private set; } = "";


        public CategoryCodeAttribute(string code)
        {
            Code = code;
        }
    }
}
