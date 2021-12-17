using System.Collections;
using System.Collections.Immutable;
using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace MarkdrawBrowser.Shared;

internal record ObjectInformation(object Object, string ObjectType, bool IsBasic)
{
  public ObjectInformation(object obj) : this(obj, obj.GetType().Name, IsObjectBasic(obj)) {}

  private static bool IsTypeOfType(Type type)
  {
    return typeof(Type).IsAssignableFrom(type);
  }

  private static bool IsObjectBasic(object obj)
  {
    return IsTypeOfType(obj.GetType()) || obj is int or string or bool or null or char or long or double or IList;
  }
}

public partial class Inspector : ComponentBase
{
  private List<(string, ObjectInformation)> _properties = new();
  private List<ObjectInformation>? _contents = null;
  private Stack<object> _history = new();

  private object _baseObject = "";
  [Parameter]
  public object BaseObject
  {
    get => _baseObject;
    set {
      _baseObject = value;
      _history = new Stack<object>();
      Object = value;
      _history.Pop();
    }
  }

  private object _object = "";
  private object Object {
    get => _object;
    set {
      _history.Push(_object);
      _object = value;
      _properties = value.GetType()
        .GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public
          | BindingFlags.NonPublic).Select(property =>
            (property.Name, new ObjectInformation(property.GetValue(value) ?? "Couldn't dereference."))
          ).ToList();
      _contents = value switch {
        IEnumerable<object> enumerable => enumerable.Select(obj => new ObjectInformation(obj)).ToList(),
        _ => null
      };
    }
  }

  private void Reset()
  {
    Object = BaseObject;
    _history = new Stack<object>();
  }

  private void Back()
  {
    Object = _history.Pop();
    _history.Pop();
  }
}
