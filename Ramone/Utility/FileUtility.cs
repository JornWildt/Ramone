using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ramone.Utility
{
  public static class FileUtility
  {
    public static String GetFileName(String path)
    {
      // Code copied from .NET source - but without the CheckInvalidPathChars(path) element in it.

      if (path != null)
      {
        int length = path.Length;
        for (int i = length; --i >= 0;)
        {
          char ch = path[i];
          if (ch == Path.DirectorySeparatorChar || ch == Path.AltDirectorySeparatorChar || ch == Path.VolumeSeparatorChar)
            return path.Substring(i + 1, length - i - 1);
        }
      }

      return path;
    }
  }
}
