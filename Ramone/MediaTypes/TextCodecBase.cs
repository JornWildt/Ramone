using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Ramone.MediaTypes
{
  /// <summary>
  /// Base for a type-checked codec which will only serialize objects of type TEntity
  /// </summary>
  /// <typeparam name="TEntity">The type of the entity.</typeparam>
  public abstract class TextCodecBase<TEntity>
    : IMediaTypeWriter, IMediaTypeReader where TEntity : class
  {
    #region Ramone.IMediaTypeReader Members

    public object ReadFrom(Stream s, Type t)
    {
      using (var reader = new StreamReader(s))
      {
        return ReadFrom(reader);
      }
    }

    #endregion


    #region IMediaTypeWriter

    public void WriteTo(Stream s, Type t, object data)
    {
      using (var writer = new StreamWriter(s))
      {
        WriteTo(data as TEntity, writer);
      }
    }

    #endregion

    protected abstract TEntity ReadFrom(TextReader reader);

    protected abstract void WriteTo(TEntity item, TextWriter writer);
  }
}
