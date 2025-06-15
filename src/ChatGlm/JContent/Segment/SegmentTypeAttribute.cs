using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGlm.JContent.Segment;

[AttributeUsage(AttributeTargets.Class)]
public class SegmentTypeAttribute(string type) : Attribute
{
    public string Type { get; } = type;
}
