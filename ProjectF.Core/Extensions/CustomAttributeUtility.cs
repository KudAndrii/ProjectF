using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json.Serialization;

namespace ProjectF.Core.Extensions;

public static class CustomAttributeUtility
{
    /// <summary>
    /// Method gets json name of the property provided by <paramref name="func"/>.
    /// </summary>
    /// <seealso cref="GetAttributeValue{TAttribute, TModel}"/>
    public static string? GetJsonName<TModel>(Expression<Func<TModel, object?>> func) =>
        GetAttributeValue<JsonPropertyNameAttribute, TModel>(attr => attr.Name, func);

    /// <summary>
    /// Method defines attribute property by <paramref name="attrFunc"/>
    /// from which result-value should be taken.<br/>
    /// The value from specific attribute property suppose to be taken
    /// from the property of the <typeparamref name="TModel"/> by given <paramref name="modelFunc"/>.
    /// </summary>
    public static string? GetAttributeValue<TAttribute, TModel>(
        Expression<Func<TAttribute, object?>> attrFunc,
        Expression<Func<TModel, object?>> modelFunc)
        where TAttribute : Attribute
    {
        // Resolve the model property info
        if (GetMemberExpression(modelFunc.Body) is not { Member: PropertyInfo } modelMember)
        {
            throw new ArgumentException("The argument must point to a member.", nameof(modelFunc));
        }

        if (modelMember.Member is not PropertyInfo modelPropertyInfo)
        {
            throw new ArgumentException("The argument must point to a property.", nameof(modelFunc));
        }

        // Get the attribute applied to the property
        var attribute = modelPropertyInfo
            .GetCustomAttributes(typeof(TAttribute), inherit: true)
            .OfType<TAttribute>()
            .FirstOrDefault();

        if (attribute is null)
        {
            throw new InvalidOperationException(
                $"The attribute {typeof(TAttribute).Name} is not applied to the model property.");
        }

        // Resolve the attribute property
        if (attrFunc.Body is not MemberExpression attrMember)
        {
            throw new ArgumentException("The attrFunc must point to a member.", nameof(attrFunc));
        }

        if (attrMember.Member is not PropertyInfo attrPropertyInfo)
        {
            throw new ArgumentException("The attrFunc must point to a property.", nameof(attrFunc));
        }

        // Get the value from the attribute property
        var value = attrPropertyInfo.GetValue(attribute);

        return value?.ToString();
    }
    
    

    private static MemberExpression? GetMemberExpression(Expression expression)
    {
        return expression switch
        {
            MemberExpression memberExpression => memberExpression,
            UnaryExpression { Operand: MemberExpression operand } => operand,
            _ => null
        };
    }
}