using System;
using System.IO;
using SharpCompress.Archives;
using SharpCompress.Common;

namespace DetectTShock;
public class SecureDllExtractor
{
    public static List<(string FileName, byte[] FileData)> ExtractDllFiles(byte[] buffer)
    {
        var dllFiles = new List<(string FileName, byte[] FileData)>();
        using (var stream = new MemoryStream(buffer))
        using (var archive = ArchiveFactory.Open(stream))
        {
            foreach (var entry in archive.Entries)
            {
                var fileName = Path.GetFileName(entry.Key);
                if (!entry.IsDirectory && fileName !=null && (fileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase) || fileName.EndsWith(".pdb", StringComparison.OrdinalIgnoreCase)))
                {
                    using (var entryStream = entry.OpenEntryStream())
                    using (var memoryStream = new MemoryStream())
                    {
                        entryStream.CopyTo(memoryStream);
                        dllFiles.Add((fileName, memoryStream.ToArray()));
                    }
                }
            }
        }

        return dllFiles;
    }
}
