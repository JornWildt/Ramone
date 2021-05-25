#region Copyright (c) 2009 S. van Deursen
/* The CuttingEdge.Conditions library enables developers to validate pre- and postconditions in a fluent 
 * manner.
 * 
 * To contact me, please visit my blog at http://www.cuttingedge.it/blogs/steven/ 
 *
 * Copyright (c) 2009 S. van Deursen
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
 * associated documentation files (the "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all copies or substantial
 * portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT
 * LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO
 * EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
 * IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE
 * USE OR OTHER DEALINGS IN THE SOFTWARE.
*/
#endregion

namespace Ramone.Utility.Validation
{
  /// <summary>
  /// Copy from https://github.com/dotnetjunkie/cuttingedge.conditions/
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public abstract class ConditionValidator<T>
  {
    public readonly T Value;

    private readonly string argumentName;

    /// <summary>Initializes a new instance of the <see cref="ConditionValidator{T}"/> class.</summary>
    /// <param name="argumentName">The name of the argument to be validated</param>
    /// <param name="value">The value of the argument to be validated</param>
    protected ConditionValidator(string argumentName, T value)
    {
      // This constructor is internal. It is not useful for a user to inherit from this class.
      // When this ctor is made protected, so should be the BuildException method.
      this.Value = value;
      this.argumentName = argumentName;
    }


    /// <summary>Gets the name of the argument.</summary>
    public string ArgumentName
    {
      get { return this.argumentName; }
    }


    /// <summary>Throws an exception.</summary>
    /// <param name="condition">Describes the condition that doesn't hold, e.g., "Value should not be 
    /// null".</param>
    public void ThrowException(string condition)
    {
      this.ThrowExceptionCore(condition, null, ConstraintViolationType.Default);
    }

    protected abstract void ThrowExceptionCore(string condition, string additionalMessage, ConstraintViolationType type);

  }
}
