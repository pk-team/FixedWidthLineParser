using System.Linq.Expressions;
using System.Reflection;

public class FixedWidthLineParser<T> where T : class, new() {
    private readonly Dictionary<string, (int StartIndex, int Length)> _fieldMap = [];
    private readonly int _totalLength;

    public FixedWidthLineParser() {
        _totalLength = InitializeFieldMap();
    }

    private int InitializeFieldMap() {
        var members = typeof(T).GetMembers(BindingFlags.Public | BindingFlags.Instance);

        var startIndex = 0;
        var totalLength = 0;

        foreach (var member in members) {
            if (member is PropertyInfo propertyInfo && propertyInfo.PropertyType == typeof(int)) {
                var length = (int)(propertyInfo.GetValue(new T()) ?? 0);
                _fieldMap[member.Name] = (startIndex, length);
                startIndex += length;
                totalLength += length;
            }
            else if (member is FieldInfo fieldInfo && fieldInfo.FieldType == typeof(int)) {
                var length = (int)(fieldInfo.GetValue(new T()) ?? 0);
                _fieldMap[member.Name] = (startIndex, length);
                startIndex += length;
                totalLength += length;
            }
        }
        return totalLength;
    }

    public string Parse(string line, Expression<Func<T, object>> fieldSelector) {
        if (line.Length < _totalLength) {
            throw new ArgumentException($"The provided line length is less than the expected total length of {_totalLength} characters.");
        }
        
        var memberName = GetMemberName(fieldSelector);
        if (_fieldMap.TryGetValue(memberName, out var info)) {
            return line.Substring(info.StartIndex, info.Length);
        }

        throw new ArgumentException($"Field or property '{memberName}' not found in type '{typeof(T).Name}'");
    }

    private static string GetMemberName(Expression<Func<T, object>> fieldSelector) {
        var memberExpression = fieldSelector.Body as MemberExpression;
        if (memberExpression == null) {
            var unaryExpression = fieldSelector.Body as UnaryExpression;
            memberExpression = unaryExpression?.Operand as MemberExpression;
        }

        return memberExpression?.Member?.Name ?? throw new ArgumentException("Invalid field selector expression", nameof(fieldSelector));
    }
}

