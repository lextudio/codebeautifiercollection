// -----------------------------------------------------------------------------------
// Use it as you please, but keep this header.
// Author : Marcus Deecke, 2006
// Web    : www.yaowi.com
// Email  : code@yaowi.com
// -----------------------------------------------------------------------------------
using System;
using System.IO;
using System.Text;

using System.ComponentModel;
using System.Reflection;
using System.Xml;
using System.Runtime.Remoting;
using System.Collections;

namespace Yaowi.Common.Serialization
{
  /// <summary>
  /// Deserializes complex objects serialized with the XmlSerializer.
  /// </summary>
  public class XmlDeserializer : IDisposable
  {
    private bool ignorecreationerrors;
    private Hashtable typedictionary = new Hashtable(); // Parsed Types
    private Hashtable assemblycache = new Hashtable();  // Found Assemblies
    private Hashtable assemblyregister = new Hashtable(); // Predefined Assemblies

    #region XmlDeserializer Properties

    /// <summary>
    /// Gets whether the current root node provides a type dictionary.
    /// </summary>
    protected bool HasTypeDictionary
    {
      get
      {
        if (typedictionary != null && typedictionary.Count > 0)
		{  return true; }

        return false;
      }
    }

    /// <summary>
    /// Gets or sets whether creation errors shall be ignored.
    /// <br/>Creation errors can occur if e.g. a type has no parameterless constructor
    /// and an instance cannot be instantiated from String.
    /// </summary>
    public bool IgnoreCreationErrors
    {
      get { return ignorecreationerrors; }
      set { ignorecreationerrors = value; }
    }

    #endregion XmlDeserializer Properties

    #region Deserialize

    /// <summary>
    /// Deserialzes an object from a xml file.
    /// </summary>
    /// <param name="filename"></param>
    /// <returns></returns>
    public object Deserialize(string fileName)
    {
      XmlDocument doc = new XmlDocument();
      doc.Load(fileName);
      return Deserialize(doc);
    }

    /// <summary>
    /// Deserialzes an object from XmlDocument.
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    public object Deserialize(XmlDocument document)
    {
      XmlNode node = document.SelectSingleNode(XmlSerializationTag.OBJECT);
      return Deserialize(node);
    }


    /// <summary>
    /// Deserializes an Object from the specified XmlNode. 
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public object Deserialize(XmlNode node)
    {
      // Clear previous collections
      Reset();
      AddAssemblyRegisterToCache();

      XmlNode rootnode = node;
      if (!rootnode.Name.Equals(XmlSerializationTag.OBJECT))
      {
        rootnode = node.SelectSingleNode(XmlSerializationTag.OBJECT);

        if (rootnode == null)
        {
          throw new ArgumentException("Invalid node. The specified node or its direct children do not contain a " + XmlSerializationTag.OBJECT + " tag.", "XmlNode node");
        }
      }

      // Load TypeDictionary
      this.typedictionary = ParseTypeDictionary(rootnode);

      // Get the Object
      object obj = GetObject(rootnode);

      if (obj != null)
      {
        //Console.WriteLine(obj.ToString());
        GetProperties(obj, rootnode);
      }
	  else
	  {	Console.WriteLine("Object is null");    }

      return obj;
    }

    /// <summary>
    /// Parses the TypeDictionary (if given).
    /// </summary>
    /// <param name="parentNode"></param>
    /// <returns></returns>
    /// <remarks>
    /// The TypeDictionary is Hashtable in which TypeInfo items are stored.
    /// </remarks>
    protected Hashtable ParseTypeDictionary(XmlNode parentNode)
    {
      Hashtable dict = new Hashtable();

      XmlNode dictNode = parentNode.SelectSingleNode(XmlSerializationTag.TYPE_DICTIONARY);
      if (dictNode == null)
	  {  return dict;   }

      object obj = GetObject(dictNode);

      if (obj != null && typeof(Hashtable).IsAssignableFrom(obj.GetType()))
      {
        dict = (Hashtable)obj;
        GetProperties(dict, dictNode);
      }

      return dict;
    }

    #endregion Deserialize

    #region Properties & values

    /// <summary>
    /// Reads the properties of the specified node and sets them an the parent object.
    /// </summary>
    /// <param name="parent"></param>
    /// <param name="node"></param>
    /// <remarks>
    /// This is the central method which is called recursivly!
    /// </remarks>
    protected void GetProperties(object parent, XmlNode node)
    {
      if (parent == null)
      {  return;  }

      // Get the properties
      XmlNodeList nl = node.SelectNodes(XmlSerializationTag.PROPERTIES + "/" + XmlSerializationTag.PROPERTY);

      // Properties found?
      if (nl == null || nl.Count == 0)
      {
        // No properties found... perhaps a collection?
        if (TypeInfo.IsCollection(parent.GetType()))
        {
          SetCollectionValues((ICollection)parent, node);
        }
        else
        {
          // Nothing to do here
          return;
        }
      }

      // Loop the properties found
      foreach (XmlNode prop in nl)
      {
        // Collect the nodes type information about the property to deserialize
        ObjectInfo oi = GetObjectInfo(prop);

        // Enough info?
        if (oi.IsSufficient && !String.IsNullOrEmpty(oi.Name))
        {
          object obj = null;

          // Create an instance, but note: arrays always need the size for instantiation
          if (TypeInfo.IsArray(oi.Type))
          {
            int c = GetArrayLength(prop);
            obj = CreateArrayInstance(oi, c);
          }
          else
          {
            obj = CreateInstance(oi);
          }

          // Setting the instance (or null) as the property's value
          PropertyInfo pi = parent.GetType().GetProperty(oi.Name);
          if (pi != null)
          {
            pi.SetValue(parent, obj, null);
          }

          // Process the property's properties (recursive call of this method)
          if (obj != null)
          {
            GetProperties(obj, prop);
          }
        }
      }

      return;
    }

    /// <summary>
    /// Sets the entries on an ICollection implementation.
    /// </summary>
    /// <param name="coll"></param>
    /// <param name="parentNode"></param>
    protected void SetCollectionValues(ICollection coll, XmlNode parentNode)
    {
      if (typeof(IDictionary).IsAssignableFrom(coll.GetType()))
      {
        // IDictionary
        SetDictionaryValues((IDictionary)coll, parentNode);
        return;
      }
      else if (typeof(IList).IsAssignableFrom(coll.GetType()))
      {
        // IList
        SetListValues((IList)coll, parentNode);
        return;
      }
    }

    /// <summary>
    /// Sets the entries on an IList implementation.
    /// </summary>
    /// <param name="list"></param>
    /// <param name="parentNode"></param>
    protected void SetListValues(IList list, XmlNode parentNode)
    {
      // Get the item nodes
      XmlNodeList nlitems = parentNode.SelectNodes(XmlSerializationTag.ITEMS + "/" + XmlSerializationTag.ITEM);

      // Loop them
      for (int i = 0; i < nlitems.Count; i++)
      {
        XmlNode nodeitem = nlitems[i];

        // Create an instance
        object obj = GetObject(nodeitem);

        // Process the properties
        GetProperties(obj, nodeitem);

        if (list.IsFixedSize)
		{  list[i] = obj;  }
        else
		{  list.Add(obj); }
      }
    }

    /// <summary>
    /// Sets the entries of an IDictionary implementation.
    /// </summary>
    /// <param name="dictionary"></param>
    /// <param name="parentNode"></param>
    protected void SetDictionaryValues(IDictionary dictionary, XmlNode parentNode)
    {
      // Get the item nodes
      XmlNodeList nlitems = parentNode.SelectNodes(XmlSerializationTag.ITEMS + "/" + XmlSerializationTag.ITEM);

      // Loop them
      for (int i = 0; i < nlitems.Count; i++)
      {
        XmlNode nodeitem = nlitems[i];

        // Retrieve the single property
        string path = XmlSerializationTag.PROPERTIES + "/" + XmlSerializationTag.PROPERTY + "[@" + XmlSerializationTag.NAME + "='" + XmlSerializationTag.NAME_ATT_KEY + "']";
        XmlNode nodekey = nodeitem.SelectSingleNode(path);

        path = XmlSerializationTag.PROPERTIES + "/" + XmlSerializationTag.PROPERTY + "[@" + XmlSerializationTag.NAME + "='" + XmlSerializationTag.NAME_ATT_VALUE + "']";
        XmlNode nodeval = nodeitem.SelectSingleNode(path);

        // Create an instance of the key
        object objkey = GetObject(nodekey);
        object objval = null;

        // Try to get the value
        if (nodeval != null)
        {
          objval = GetObject(nodeval);
        }

        // Set the entry if the key is not null
        if (objkey != null)
        {
          // Set the entry's value if its is not null and process its properties
          if (objval != null)
		   { GetProperties(objval, nodeval);  }

          dictionary.Add(objkey, objval);
        }
      }
    }

    #endregion Properties & values

    #region Creating instances and types

    /// <summary>
    /// Creates an instance by the contents of the given XmlNode.
    /// </summary>
    /// <param name="node"></param>
    protected object GetObject(XmlNode node)
    {
      ObjectInfo oi = GetObjectInfo(node);

      if (TypeInfo.IsArray(oi.Type))
      {
        int c = GetArrayLength(node);
        return CreateArrayInstance(oi, c);
      }

      return CreateInstance(oi);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="assembly"></param>
    /// <returns></returns>
    protected Assembly GetAssembly(String assembly)
    {
      Assembly a = null;

      // Shortnamed, version independent assembly name
      int x = assembly.IndexOf(",");
      string ass = null;
      if (x > 0)
	  {  ass = assembly.Substring(0, x);    }

	  // Cached already?
      if (ass != null && assemblycache.ContainsKey(ass))
      {
        //Console.WriteLine("Cached Assembly found: " + ass);
        return (Assembly)assemblycache[ass];
      }

      // Cached already?
      if (assemblycache.ContainsKey(assembly))
      {
        //Console.WriteLine("Cached Assembly found: " + assembly);
        return (Assembly)assemblycache[assembly];
      }

      // TODO: Clean the upcoming code. Caching is handled above.

      // Try to get the Type in any way
      try
      {
        String path = Path.GetDirectoryName(assembly);
        if (!String.IsNullOrEmpty(path))
        {
          // Assembly cached already?
          if (assemblycache.ContainsKey(assembly))
          {
            // Cached
            a = (Assembly)assemblycache[assembly];
          }
          else
          {
            // Not cached, yet
            a = Assembly.LoadFrom(assembly);
            assemblycache.Add(assembly, a);
          }
        }
        else
        {
          try
          {
            // Try to load the assembly version independent
            if (ass!=null)
            {
              // Assembly cached already?
              if (assemblycache.ContainsKey(ass))
              {
                // Cached
                a = (Assembly)assemblycache[ass];
              }
              else
              {
                // Not cached, yet
                a = Assembly.Load(ass);
                assemblycache.Add(ass, a);
              }
            }
            else
            {
              // Assembly cached already?
              if (assemblycache.ContainsKey(assembly))
              {
                // Cached
                a = (Assembly)assemblycache[assembly];
              }
              else
              {
                // Not cached, yet
                a = Assembly.Load(assembly);
                assemblycache.Add(assembly, a);
              }
            }
          }
          catch
          {
            // Loading the assembly version independent failed: load it with the given version.

            if (assemblycache.ContainsKey(assembly))
            {
              // Cached
              a = (Assembly)assemblycache[assembly];
            }
            else
            {
              // Not cached, yet
              a = Assembly.Load(assembly);
              assemblycache.Add(assembly, a);
            }
          }
        }
      }
      catch { /* ok, we did not get the Assembly */ }

      return a;
    }

    /// <summary>
    /// Creates a type from the specified assembly and type names. In case of failure null will be returned.
    /// </summary>
    /// <param name="assembly"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    protected Type CreateType(String assembly, string type)
    {
      Type t = null;

      try
      {
        Assembly a = GetAssembly(assembly);

        if (a != null)
        {
          t = a.GetType(type);
        }
      }
      catch { /* ok, we did not get the Type */ }

      return t;
    }

    /// <summary>
    /// Creates an instance of an Array by the specified ObjectInfo.
    /// </summary>
    /// <param name="info"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    private Array CreateArrayInstance(ObjectInfo info, int length)
    {
      // The Type name of an array ends with "[]"
      // Exclude this to get the real Type
      string typename = info.Type.Substring(0, info.Type.Length - 2);

      Type t = CreateType(info.Assembly, typename);

      Array arr = Array.CreateInstance(t, length);

      return arr;
    }

    /// <summary>
    /// Creates an instance by the specified ObjectInfo.
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    private object CreateInstance(ObjectInfo info)
    {
      try
      {
        // Enough information to create an instance?
        if (!info.IsSufficient)
        {  return null;   }

		object obj = null;

        // Get the Type
        Type type = CreateType(info.Assembly, info.Type);

        if (type == null)
        {
          throw new Exception("Assembly or Type not found.");
        }

        // Ok, we've got the Type, now try to create an instance
        // Problem: only parameterless constructors or constructors with one parameter
        // which can be converted from String are supported.
        // Failure Example:
        // string s = new string();
        // string s = new string("");
        // This cannot be compiled, but the follwing works;
        // string s = new string("".ToCharArray());
        // The TypeConverter provides a way to instantite objects by non-parameterless 
        // constructors if they can be converted fro String
        try
        {
          TypeConverter tc = TypeDescriptor.GetConverter(type);
          if (tc.CanConvertFrom(typeof(string)))
          {
            obj = tc.ConvertFrom(info.Value);
            return obj;
          }
        }
        catch { ; }

        obj = Activator.CreateInstance(type);

        if (obj == null)
		{  throw new Exception("Instance could not be created."); }

		return obj;
      }
      catch (Exception e)
      {
        string msg = "Creation of an instance failed. Type: " + info.Type + " Assembly: " + info.Assembly + " Cause: " + e.Message;
        if (IgnoreCreationErrors)
        {
          Console.WriteLine(msg);
          return null;
        }
        else
		{  throw new Exception(msg, e); }
	  }
    }

    #endregion Creating instances and types

    #region Misc

    /// <summary>
    /// Gets an ObjectInfo instance by the attributes of the specified XmlNode.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private ObjectInfo GetObjectInfo(XmlNode node)
    {
      ObjectInfo oi = new ObjectInfo();

      // If a TypeDictionary is given try to find the Types properties.
      if (HasTypeDictionary)
      {
        String typekey = GetAttributeValue(node, XmlSerializationTag.TYPE);

        if (this.typedictionary.ContainsKey(typekey))
        {
          TypeInfo ti = (TypeInfo)this.typedictionary[typekey];

          oi.Type = ti.TypeName;
          oi.Assembly = ti.AssemblyName;
        }
      }

      // If a TypeDictionary is given, did we find the necessary information to create an instance?
      // If not, try to get information by the Node itself
      if(!oi.IsSufficient)
      {
        oi.Type = GetAttributeValue(node, XmlSerializationTag.TYPE);
        oi.Assembly = GetAttributeValue(node, XmlSerializationTag.ASSEMBLY);
      }

      // Name and Value
      oi.Name = GetAttributeValue(node, XmlSerializationTag.NAME);
      oi.Value = node.InnerText;

      return oi;
    }

    /// <summary>
    /// Returns the length of the array of a arry-XmlNode.
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    protected int GetArrayLength(XmlNode parent)
    {
      XmlNodeList nl = parent.SelectNodes(XmlSerializationTag.ITEMS + "/" + XmlSerializationTag.ITEM);
      int c = 0;

      if (nl != null)
      {  c = nl.Count;   }

      return c;
    }

    /// <summary>
    /// Returns the value or the attribute with the specified name from the given node if it is not null or empty.
    /// </summary>
    /// <param name="node"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    protected string GetAttributeValue(XmlNode node, string name)
    {
      if (node == null || String.IsNullOrEmpty(name))
	  {  return null;  }

      String val = null;
      XmlAttribute att = node.Attributes[name];

      if (att != null)
      {
        val = att.Value;
		if (val.Equals(""))
		{  val = null; }
      }
      return val;
    }

    /// <summary>
    /// Registers an Assembly.
    /// </summary>
    /// <param name="assembly"></param>
    /// <remarks>
    /// Register Assemblies which are not known at compile time, e.g. PlugIns or whatever.
    /// </remarks>
    public void RegisterAssembly(Assembly assembly)
    {
      string ass = assembly.FullName;

      int x = ass.IndexOf(",");
      if (x > 0)
	  {  ass = ass.Substring(0, x);   }

	  assemblyregister[ass] = assembly;
    }

    /// <summary>
    /// Adds the assembly register items to the assembly cache.
    /// </summary>
    protected void AddAssemblyRegisterToCache()
    {
	  if (assemblyregister == null)
	  {	return;  }

      IDictionaryEnumerator de = assemblyregister.GetEnumerator();
      while (de.MoveNext())
      {
        assemblycache[de.Key] = de.Value;
      }
    }

    /// <summary>
    /// Clears all collections.
    /// </summary>
    public void Reset()
    {
      if (this.typedictionary != null)
	  {  this.typedictionary.Clear();  }

      if (this.assemblycache != null)
	  {  this.assemblycache.Clear();   }
	}

    /// <summary>
    /// Dispose, release references.
    /// </summary>
    public void Dispose()
    {
      Reset();

      if (assemblyregister != null)
	  {  assemblyregister.Clear(); }
      GC.SuppressFinalize(this);
    }

    #endregion Misc
  }
}
