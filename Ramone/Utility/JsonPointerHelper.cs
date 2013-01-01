using System;
using System.Linq.Expressions;
using System.Globalization;


namespace Ramone.Utility
{
  public class JsonPointerHelper<TObject>
  {
    private string Separator;


    public JsonPointerHelper(string separator = "/")
    {
      Separator = separator;
    }


    public string GetPath<TProperty>(Expression<Func<TObject, TProperty>> expr)
    {
      return Separator + GetPath(expr.Body, true);
    }


    private string GetPath(Expression expr, bool firstTime)
    {
      if (expr.NodeType == ExpressionType.MemberAccess)
      {
        MemberExpression m = expr as MemberExpression;
        string left = GetPath(m.Expression, false);
        if (left != null)
          return left + Separator + m.Member.Name;
        else
          return m.Member.Name;
      }
      else if (expr.NodeType == ExpressionType.Call)
      {
        MethodCallExpression m = (MethodCallExpression)expr;
        string left = GetPath(m.Object, false);
        if (left != null)
          return left + Separator + GetIndexerInvocation(m.Arguments[0]);
        else
          return GetIndexerInvocation(m.Arguments[0]);
      }
      else if (expr.NodeType == ExpressionType.ArrayIndex)
      {
        BinaryExpression b = (BinaryExpression)expr;
        string left = GetPath(b.Left, false);
        if (left != null)
          return left + Separator + b.Right.ToString();
        else
          return b.Right.ToString();
      }
      else if (expr.NodeType == ExpressionType.Parameter)
      {
        // Fits "x => x" (the whole document which is "" as JSON pointer)
        return firstTime ? "" : null;
      }
      else
        return null;
    }


    private static string GetIndexerInvocation(Expression expression)
    {
      Expression converted = Expression.Convert(expression, typeof(object));
      ParameterExpression fakeParameter = Expression.Parameter(typeof(object), null);
      Expression<Func<object, object>> lambda = Expression.Lambda<Func<object, object>>(converted, fakeParameter);
      Func<object, object> func;

      func = lambda.Compile();

      return Convert.ToString(func(null), CultureInfo.InvariantCulture);
    }
  }
}
