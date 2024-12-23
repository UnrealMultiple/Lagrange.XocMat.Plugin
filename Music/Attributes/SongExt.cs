using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Music.Attributes;

[AttributeUsage(AttributeTargets.Field)]
public class SongExt(string ext) : Attribute
{
    public string Ext { get; } = ext;
}
