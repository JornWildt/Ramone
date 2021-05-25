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

using System;
using System.Collections.Generic;
using System.Text;

namespace Ramone.Utility.Validation
{
  public static partial class ValidatorExtensions
  {
    /// <summary>
    /// Checks whether the given value is not null and not an <see cref="String.Empty"/> string.
    /// An exception is thrown otherwise.
    /// </summary>
    /// <param name="validator">The <see cref="ConditionValidator{T}"/> that holds the value that has to be checked.</param>
    /// <returns>The specified <paramref name="validator"/> instance.</returns>
    /// <exception cref="ArgumentException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is <see cref="String.Empty"/>, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Requires{T}(T,string)">Requires</see> extension method.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is null or empty, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Requires{T}(T,string)">Requires</see> extension method.</exception>
    /// <exception cref="PostconditionException">Thrown when the <see cref="ConditionValidator{T}.Value">Value</see> of the specified <paramref name="validator"/> is null or empty, while the specified <paramref name="validator"/> is created using the <see cref="Condition.Ensures{T}(T,string)">Ensures</see> extension method.</exception>
    public static ConditionValidator<string> IsNotNullOrEmpty(this ConditionValidator<string> validator)
    {
      bool valueIsInvalid = String.IsNullOrEmpty(validator.Value);

      if (valueIsInvalid)
      {
        Throw.ValueShouldNotBeNullOrAnEmptyString(validator, null);
      }

      return validator;
    }
  }
}
