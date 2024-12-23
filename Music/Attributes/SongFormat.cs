using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class SongFormat(string format) : Attribute
{
    public string Format { get; } = format;
}
