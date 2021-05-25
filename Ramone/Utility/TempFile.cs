using System;
using System.IO;
using Ramone.Utility.Validation;


namespace Ramone.Utility
{
  public class TempFile : IDisposable
  {
    string _path;


    public string Path
    {
      get
      {
        if (_path == null) 
          throw new ObjectDisposedException(GetType().Name);
        return _path;
      }
    }


    public TempFile() 
      : this(System.IO.Path.GetTempFileName()) 
    { 
    }

    
    public TempFile(string path)
    {
      Condition.Requires(path, "path").IsNotNullOrEmpty();
      _path = path;
    }


    ~TempFile() 
    { 
      Dispose(false); 
    }
    
    
    public void Dispose() 
    { 
      Dispose(true); 
    }
    
    
    private void Dispose(bool disposing)
    {
      if (disposing)
      {
        GC.SuppressFinalize(this);
      }
      if (_path != null)
      {
        try { File.Delete(_path); }
        catch { } // best effort
        _path = null;
      }
    }
  }
}
