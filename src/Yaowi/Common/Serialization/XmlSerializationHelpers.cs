// -----------------------------------------------------------------------------------
// Use it as you please, but keep this header.
// Author : Marcus Deecke, 2006
// Web    : www.yaowi.com
// Email  : code@yaowi.com
// -----------------------------------------------------------------------------------
using System;

using System.Text;
using System.Runtime.CompilerServices;
using System.Collections;

namespace Yaowi.Common.Serialization
{
  /// <summary>
  /// This class supports the Yaowi Framework infrastructure and is not intended to be used directly from your code. 
  /// <P/>These constants are used to parse the XmlNodes.
  /// </summary>
  internal class XmlSerializationTag
  {
    public const string OBJECT = "object";
    public const string NAME = "name";
    public const string TYPE = "type";
    public const string ASSEMBLY = "assembly";
    public const string PROPERTIES = "properties";
    public const string PROPERTY = "property";
    public const string ITEMS = "items";
    public const string ITEM = "item";
    public const string INDEX = "index";
    public const string NAME_ATT_KEY = "Key";
    public const string NAME_ATT_VALUE = "Value";
    public const string TYPE_DICTIONARY = "typedictionary";
    private XmlSerializationTag() { }
  }

  /// <summary>
  /// This struct supports the Yaowi Framework infrastructure and is not intended to be used directly from your code. 
  /// <P/>This struct records relevant object information.
  /// </summary>
  /// <remarks>
  /// Strings in a struct? Strings are reference types and structs should not contain types like this. TODO!
  /// </remarks>
  internal struct ObjectInfo
  {
    // Members
	public string Name;
    public string Type;
    public string Assembly;
    public string Value;

    /// <summary>
    /// ToString()
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      string n = Name;
      if (String.IsNullOrEmpty(n))
	  {  n = "<Name not set>";  }

      string t = Type;
      if (String.IsNullOrEmpty(t))
      {  t = "<Type not set>";     }

      string a = Type;
      if (String.IsNullOrEmpty(a))
	  {  a = "<Assembly not set>";  }

      return n + "; " + t + "; " + a;
    }

    /// <summary>
    /// Determines whether the values are sufficient to create an instance.
    /// </summary>
    /// <returns></returns>
    public bool IsSufficient
    {
      get
      {
        // Type and Assembly should be enough
        if (String.IsNullOrEmpty(Type) || String.IsNullOrEmpty(Assembly))
		{  return false; }

        return true;
      }
    }
  }

  /// <summary>
  /// Helper class storing a Types creation information as well as providing various static methods 
  /// returning information about types.
  /// </summary>
  [Serializable]
  public class TypeInfo
  {
    #region Members & Properties

    private string typename;
    private string assemblyname;

    /// <summary>
    /// Gets or sets the Types name.
    /// </summary>
    public string TypeName
    {
      get { return typename; }
      set { typename = value; }
    }

    /// <summary>
    /// Gets or sets the Assemblys name.
    /// </summary>
    public string AssemblyName
    {
      get { return assemblyname; }
      set { assemblyname = value; }
    }

    #endregion Members & Properties

    #region Constructors
	/// <summary>
	/// Constructor.
	/// </summary>
    public TypeInfo()
    {
    }
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="obj"></param>
    public TypeInfo(object obj)
    {
      if (obj == null)
	  {  return;  }

	  TypeName = obj.GetType().FullName;
      AssemblyName = obj.GetType().Assembly.FullName;
    }
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="type"></param>
    public TypeInfo(Type type)
    {
      if (type == null)
	  {  return;    }

      TypeName = type.FullName;
      AssemblyName = type.Assembly.FullName;
    }

    #endregion Constructors

    #region static Helpers

    /// <summary>
    /// Determines whether a Type is a Collection type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static bool IsCollection(Type type)
    {
      if (typeof(ICollection).IsAssignableFrom(type))
      {
        return true;
      }
      return false;
    }

    /// <summary>
    /// Determines whether a Type is a Dictionary type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static bool IsDictionary(Type type)
    {
      if (typeof(IDictionary).IsAssignableFrom(type))
      {
        return true;
      }
      return false;
    }

    /// <summary>
    /// Determines whether a Type is a List type.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static bool IsList(Type type)
    {
      if (typeof(IList).IsAssignableFrom(type))
      {
        return true;
      }
      return false;
    }

    /// <summary>
    /// Determines whether the typename describes an array.
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.Synchronized)]
    public static bool IsArray(String type)
    {
      // The simple way
      if (type!=null && type.EndsWith("[]"))
      {  return true;   }

      return false;
    }

    #endregion static Helpers
  }
}
