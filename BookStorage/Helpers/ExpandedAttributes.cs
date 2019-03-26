using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStorage.Helpers
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExpandedAttributes: Attribute
    {
        public string HeaderName { get; set; }
    }
}
