using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ObjectReflection
{
    public static class ObjectReflection
    {

        public static List<ChangedMember> GetChanges<T>(T oldObject, T newObject, params Expression<Func<T, object>>[] predicators)
        {
            List<ChangedMember> changes = new List<ChangedMember>();

            Type type = typeof(T);

            foreach (Expression<Func<T, object>> predicator in predicators)
            {
                string fieldName, oldValue, newValue;
                if (predicator.Body is MemberExpression)
                {
                    MemberExpression e = predicator.Body as MemberExpression;

                    _extractMemberChanges(oldObject, newObject, e, out fieldName, out oldValue, out newValue);

                    if (oldValue != newValue)
                    {
                        changes.Add(new ChangedMember
                        {
                            Field = fieldName,
                            OldValue = oldValue,
                            NewValue = newValue,
                        });
                    }
                }
                else if (predicator.Body is UnaryExpression)
                {
                    MemberExpression e = (predicator.Body as UnaryExpression).Operand as MemberExpression;

                    _extractMemberChanges(oldObject, newObject, e, out fieldName, out oldValue, out newValue);

                    if (oldValue != newValue)
                    {
                        changes.Add(new ChangedMember
                        {
                            Field = fieldName,
                            OldValue = oldValue,
                            NewValue = newValue,
                        });
                    }
                }
            }

            return changes;

        }

        internal static void _extractMemberChanges<T>(T oldObject, T newObject, MemberExpression e,
            out string fieldName, out string oldValue, out string newValue)
        {
            object _old, _new;
            _extractMemberChanges(oldObject, newObject, e, out fieldName, out _old, out _new);
            if (!string.IsNullOrEmpty(fieldName)) { fieldName += "."; }
            fieldName += e.Member.Name;
            oldValue = _old?.GetType().GetProperty(e.Member.Name).GetValue(_old)?.ToString() ?? string.Empty;
            newValue = _new?.GetType().GetProperty(e.Member.Name).GetValue(_new)?.ToString() ?? string.Empty;
        }

        internal static void _extractMemberChanges<T>(T oldObject, T newObject, MemberExpression e, out string fieldName, out object _old, out object _new)
        {
            if (e.Expression is ParameterExpression)
            {
                fieldName = string.Empty;
                _old = oldObject;
                _new = newObject;
            }
            else if (e.Expression is MemberExpression)
            {
                _extractMemberChanges(oldObject, newObject, e.Expression as MemberExpression, out fieldName, out _old, out _new);
                MemberExpression exp = e.Expression as MemberExpression;
                if (!string.IsNullOrEmpty(fieldName)) { fieldName += "."; }
                fieldName += exp.Member.Name;
                if (_old != null)
                {
                    _old = _old.GetType().GetProperty(exp.Member.Name).GetValue(_old);
                }
                if (_new != null)
                {
                    _new = _new.GetType().GetProperty(exp.Member.Name).GetValue(_new);
                }
            }
            else
            {
                fieldName = string.Empty;
                _old = null;
                _new = null;
            }
        }

        private static void _extractCurrentMembers<T>(T oldObject, T newObject, MemberExpression e, out string fieldName, out string oldValue, out string newValue)
        {
            fieldName = e.Member.Name;
            oldValue = typeof(T).GetProperty(fieldName).GetValue(oldObject)?.ToString() ?? string.Empty;
            newValue = typeof(T).GetProperty(fieldName).GetValue(newObject)?.ToString() ?? string.Empty;
        }
    }

    public class ChangedMember
    {
        public string Field { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
