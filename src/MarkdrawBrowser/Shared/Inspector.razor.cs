﻿using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Components;

namespace MarkdrawBrowser.Shared;

internal record ObjectInformation(object? Object, string ObjectType, bool IsBasic)
{
  public ObjectInformation(object? obj) : this(obj, GetFriendlyTypeName(obj?.GetType()), IsObjectBasic(obj)) {}

  public ObjectInformation(object? obj, Type? type) : this(obj, GetFriendlyTypeName(type), IsObjectBasic(obj)) {}

  private static bool IsTypeOfType(Type type)
  {
    return typeof(Type).IsAssignableFrom(type);
  }

  private static bool IsObjectBasic(object? obj)
  {
    return obj is null or int or string or bool or char or long or double or CouldNotEvaluateResponse
      || IsTypeOfType(obj.GetType());
  }

  private static string GetFriendlyTypeName(Type? type)
  {
    if (type is null)
    {
      return "null";
    }

    var name = type.Name;

    if (type.IsGenericParameter || !type.IsGenericType)
    {
      return type.Name;
    }

    var index = name.IndexOf("`", StringComparison.Ordinal);

    if (index == -1)
    {
      return type.Name;
    }

    var builder = new StringBuilder();
    builder.Append(name[..index]);
    builder.Append('<');
    builder.AppendJoin(',', type.GetGenericArguments().Select(GetFriendlyTypeName));
    builder.Append('>');
    return builder.ToString();
  }
}

internal class CouldNotEvaluateResponse
{
  public override string ToString()
  {
    return "[COULD NOT EVALUATE]";
  }
}

public partial class Inspector : ComponentBase
{
  private object _baseObject = "";
  private List<ObjectInformation>? _contents = null;
  private Stack<object> _history = new();

  private object _object = "";
  private List<(string, ObjectInformation)> _properties = new();
  [Parameter]
  public object BaseObject
  {
    get => _baseObject;
    set
    {
      _baseObject = value;
      _history = new Stack<object>();
      Object = value;
      _history.Pop();
    }
  }
  private object Object
  {
    get => _object;
    set
    {
      _history.Push(_object);
      _object = value;
      _properties = value.GetType()
        .GetProperties(BindingFlags.Instance | BindingFlags.FlattenHierarchy | BindingFlags.Public
          | BindingFlags.NonPublic).Select(property => {
          try
          {
            return (property.Name, new ObjectInformation(property.GetValue(value), property.PropertyType));
          }
          catch
          {
            return (property.Name, new ObjectInformation(new CouldNotEvaluateResponse(), property.PropertyType));
          }
        }).ToList();
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

  private static string LinkString(object? obj)
  {
    return obj switch {
      string s => @$"""{s}""",
      null => "null",
      _ => obj.ToString() switch {
        "" => @"""""",
        null => "null",
        var x => x
      }
    };
  }
}
