// -----------------------------------------------------------------------------------
// Use it as you please, but keep this header.
// Author : Marcus Deecke, 2006
// Web    : www.yaowi.com
// Email  : code@yaowi.com
// -----------------------------------------------------------------------------------
using System;
using System.IO;

using System.ComponentModel;
using System.Reflection;
using System.Xml;
using System.Collections;

namespace Yaowi.Common.Serialization
{
  /// <summary>
  /// Serializes arbitrary objects to XML.
  /// </summary>
  public class XmlSerializer : IDisposable
  {
    #region Members

    // All serialized objects are registered here
    private ArrayList objlist = new ArrayList();
    private bool ignoreserializableattribute;

    // If a type dictionary is used, store the Types here
    private Hashtable typedictionary = new Hashtable();
    // Usage of a type dictionary is optional, but it's advised
    private bool usetypedictionary = true;
	/// <summary>
	/// Type key prefix.
	/// </summary>
    public const string TYPE_KEY_PREFIX = "TK";

    #endregion Members

    #region Properties

    /// <summary>
    /// Gets or sets whether a type dictionary is used to store Type information.
    /// </summary>
    [Description("Gets or sets whether a type dictionary is used to store Type information.")]
    public bool UseTypeDictionary
    {
      get { return usetypedictionary; }
      set { usetypedictionary = value; }
    }

    /// <summary>
    /// Gets or sets whether the ISerializable Attribute is ignored.
    /// </summary>
    /// <remarks>
    /// Set this property only to true if you know about side effects.
    /// </remarks>
    [Description("Gets or sets whether the ISerializable Attribute is ignored.")]
    public bool IgnoreSerializableAttribute
    {
      get { return ignoreserializableattribute; }
      set { ignoreserializableattribute = value; }
    }

    #endregion Properties

    #region Serialize

    /// <summary>
    /// Serializes an Object to a file.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="filename"></param>
    /// <returns></returns>
    public void Serialize(object obj, string fileName)
    {
      XmlDocument doc = Serialize(obj);
      doc.Save(fileName);
    }

    /// <summary>
    /// Serializes an Object to a new XmlDocument.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public XmlDocument Serialize(object obj)
    {
      XmlDocument doc = new XmlDocument();
      XmlDeclaration xd = doc.CreateXmlDeclaration("1.0", "utf-8", null);
      doc.AppendChild(xd);

      Serialize(obj, null, doc);

      return doc;
    }

    /// <summary>
    /// Serializes an Object and appends it to root (DocumentElement) of the specified XmlDocument.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <param name="doc"></param>
    public void Serialize(object obj, String name, XmlDocument doc)
    {
      Reset();

      XmlElement root = doc.CreateElement(XmlSerializationTag.OBJECT);

      SetObjectInfoAttributes(name, obj.GetType(), root);

      if (doc.DocumentElement == null)
	  {  doc.AppendChild(root);     }
      else
	  {  doc.DocumentElement.AppendChild(root);    }

      SerializeProperties(obj, root);
      WriteTypeDictionary(root);
    }

    /// <summary>
    /// Serializes an Object and appends it to the specified XmlNode.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="name"></param>
    /// <param name="parent"></param>
    public void Serialize(object obj, String name, XmlNode parent)
    {
      Reset();

      XmlDocument doc = parent.OwnerDocument;

      XmlElement root = doc.CreateElement(XmlSerializationTag.OBJECT);
      parent.AppendChild(root);

      SetObjectInfoAttributes(name, obj.GetType(), root);
      SerializeProperties(obj, root);
      WriteTypeDictionary(root);
    }

    #endregion Serialize

    #region ObjectInfo

    /// <summary>
    /// Returns an ObjectInfo filled with the values of Name, Type, and Assembly.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    private ObjectInfo GetObjectInfo(string name, Type type)
    {
      ObjectInfo oi = new ObjectInfo();

      oi.Name = name;
      oi.Type = type.FullName;
      oi.Assembly = type.Assembly.FullName;

      return oi;
    }

    /// <summary>
    /// Sets the property attributes of a Property to an XmlNode.
    /// </summary>
    /// <param name="propertyName"></param>
    /// <param name="type"></param>
    /// <param name="node"></param>
    private void SetObjectInfoAttributes(String propertyName, Type type, XmlNode node)
    {
      ObjectInfo objinfo = new ObjectInfo();

      GetObjectInfo(propertyName, type);

      if (type != null)
      {
        objinfo = GetObjectInfo(propertyName, type);
      }

      // Use of a TypeDictionary?
      if (usetypedictionary)
      {
        // TypeDictionary
        String typekey = GetTypeKey(type);

        XmlAttribute att = node.OwnerDocument.CreateAttribute(XmlSerializationTag.NAME);
        att.Value = objinfo.Name;
        node.Attributes.Append(att);

        att = node.OwnerDocument.CreateAttribute(XmlSerializationTag.TYPE);
        att.Value = typekey;
        node.Attributes.Append(att);

        // The assembly will be set, also, but it's always empty.
        att = node.OwnerDocument.CreateAttribute(XmlSerializationTag.ASSEMBLY);
        att.Value = "";
        node.Attributes.Append(att);
      }
      else
      {
        // No TypeDictionary
        XmlAttribute att = node.OwnerDocument.CreateAttribute(XmlSerializationTag.NAME);
        att.Value = objinfo.Name;
        node.Attributes.Append(att);

        att = node.OwnerDocument.CreateAttribute(XmlSerializationTag.TYPE);
        att.Value = objinfo.Type;
        node.Attributes.Append(att);

        att = node.OwnerDocument.CreateAttribute(XmlSerializationTag.ASSEMBLY);
        att.Value = objinfo.Assembly;
        node.Attributes.Append(att);
      }
    }

    #endregion ObjectInfo

    #region Properties

    /// <summary>
    /// Serializes the properties an Object and appends them to the specified XmlNode.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="parent"></param>
    protected void SerializeProperties(object obj, XmlNode parent)
    {
      if (TypeInfo.IsCollection(obj.GetType()))
      {
        SetCollectionItems(obj, (ICollection)obj, parent);
      }
      else
      {
        XmlElement node = parent.OwnerDocument.CreateElement(XmlSerializationTag.PROPERTIES);

        PropertyInfo[] piarr = obj.GetType().GetProperties();
        for (int i = 0; i < piarr.Length; i++)
        {
          PropertyInfo pi = piarr[i];
          SetProperty(obj, pi, node);
        }

        parent.AppendChild(node);
      }
    }

    /// <summary>
    /// Sets a property.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="pi"></param>
    /// <param name="parent"></param>
    protected void SetProperty(object obj, PropertyInfo pi, XmlNode parent)
    {
      objlist.Add(obj);

      object val = pi.GetValue(obj, null);
      // If the value there's nothing to do
      if (val == null)
	  {	return;  }

      // If the the value already exists in the list of processed objects/properties
      // ignore it o avoid circular calls.
      if(objlist.Contains(val))
	  {  return;     }

      SetProperty(obj, val, pi, parent);
    }

    /// <summary>
    /// Sets a property.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    /// <param name="pi"></param>
    /// <param name="parent"></param>
    /// <remarks>
    /// This is the central method which is called recursivly!
    /// </remarks>
    protected void SetProperty(object obj, object value, PropertyInfo pi, XmlNode parent)
    {
      object val = value;

      // Get the Type
	  Type pt;// = pi.PropertyType;
      pt = val.GetType();

      // Empty values are ignored (no need to restore null references or empty Strings)
      if (val == null || val.Equals(""))
	  {  return;   }

      // Check whether this property can be serialized and deserialized
      if ((pt.IsSerializable || IgnoreSerializableAttribute) && (pi.CanWrite) && ((pt.IsPublic) || (pt.IsEnum)))
      {
        XmlElement prop = parent.OwnerDocument.CreateElement(XmlSerializationTag.PROPERTY);

        SetObjectInfoAttributes(pi.Name, pt, prop);

        // Collections ask for a specific handling
        if (TypeInfo.IsCollection(pt))
        {
          SetCollectionItems(obj, (ICollection)value, prop);
        }
        else
        {
          bool complexclass = false;  // Holds whether the propertys type is an complex type (the properties of objects have to be iterated, either)

          // Get all properties
          PropertyInfo[] piarr2 = pt.GetProperties();
          XmlElement proplist = null;

          // Loop all properties
          for (int j = 0; j < piarr2.Length; j++)
          {
            PropertyInfo pi2 = piarr2[j];
            // Check whether this property can be serialized and deserialized
            if ((pi2.PropertyType.IsSerializable || IgnoreSerializableAttribute) && (pi2.CanWrite) && ((pi2.PropertyType.IsPublic) || (pi2.PropertyType.IsEnum)))
            {
              // Seems to be a complex type
              complexclass = true;

              // Add a properties parent node
              if (proplist == null)
              {
                proplist = parent.OwnerDocument.CreateElement(XmlSerializationTag.PROPERTIES);
                prop.AppendChild(proplist);
              }

              // Set the property (recursive call of this method!)
              SetProperty(val, pi2, proplist);
            }
          }

          // Ok, that was not a complex class
          if (!complexclass)
          {
            // If possible, convert this property to a string
            TypeConverter tc = TypeDescriptor.GetConverter(pt);
            if (tc.CanConvertTo(typeof(string)))
            {
              val = (string)tc.ConvertTo(pi.GetValue(obj, null), typeof(string));
              prop.InnerText = val.ToString();
            }
            else
            {
              // Converting to string was not possible, just set the value by ToString()
              prop.InnerText = pi.GetValue(obj, null).ToString();
            }
          }
        }
        // Append the property node to the paren XmlNode
        parent.AppendChild(prop);
      }
    }

    #endregion Properties

    #region SetCollectionItems

    /// <summary>
    /// Sets the items on a collection.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="value"></param>
    /// <param name="parent"></param>
    /// <remarks>
    /// This method could be simplified since it's mainly the same code you can find in SetProperty()
    /// </remarks>
    protected void SetCollectionItems(object obj, ICollection value, XmlNode parent)
    {
      // Validating the parameters
      if (obj == null || value == null || parent == null)
	  {  return;   }

	  ICollection coll = value;

      XmlElement collnode = parent.OwnerDocument.CreateElement(XmlSerializationTag.ITEMS);
      parent.AppendChild(collnode);

      int cnt = 0;

      // What kind of Collection?
      if (TypeInfo.IsDictionary(coll.GetType()))
      {
        // IDictionary
        IDictionary dict = (IDictionary)coll;
        IDictionaryEnumerator de = dict.GetEnumerator();
        while (de.MoveNext())
        {
          object key = de.Key;
		  //object val = de.Value;

          XmlElement itemnode = parent.OwnerDocument.CreateElement(XmlSerializationTag.ITEM);
          collnode.AppendChild(itemnode);

          object curr = de.Current;

          XmlElement propsnode = parent.OwnerDocument.CreateElement(XmlSerializationTag.PROPERTIES);
          itemnode.AppendChild(propsnode);

          PropertyInfo[] piarr = curr.GetType().GetProperties();
          for (int i = 0; i < piarr.Length; i++)
          {
            PropertyInfo pik = piarr[i];
            SetProperty(curr, pik, propsnode);
          }
        }
      }
      else
      {
        // Everything else
        IEnumerator ie = coll.GetEnumerator();
        while (ie.MoveNext())
        {
          object obj2 = ie.Current;

          XmlElement itemnode = parent.OwnerDocument.CreateElement(XmlSerializationTag.ITEM);

          if (obj2 != null)
          {
            SetObjectInfoAttributes(null, obj2.GetType(), itemnode);
          }
          else
          {
            SetObjectInfoAttributes(null, null, itemnode);
          }

          itemnode.Attributes[XmlSerializationTag.NAME].Value = "" + cnt;

          cnt++;

          collnode.AppendChild(itemnode);

          if (obj2 == null)
		  {  continue; }

		  Type pt = obj2.GetType();

          if (TypeInfo.IsCollection(pt))
          {
            SetCollectionItems(obj, (ICollection)obj2, itemnode);
          }
          else
          {
            bool complexclass = false;

            XmlElement propsnode = null;

            PropertyInfo[] piarr2 = pt.GetProperties();
            for (int j = 0; j < piarr2.Length; j++)
            {
              PropertyInfo pi2 = piarr2[j];
              if ((pi2.PropertyType.IsSerializable || IgnoreSerializableAttribute) && (pi2.CanWrite) && ((pi2.PropertyType.IsPublic) || (pi2.PropertyType.IsEnum)))
              {

                if (propsnode == null)
                {
                  propsnode = parent.OwnerDocument.CreateElement(XmlSerializationTag.PROPERTIES);
                  itemnode.AppendChild(propsnode);
                }

                complexclass = true;
                SetProperty(obj2, pi2, propsnode);
              }
            }

            if (!complexclass)
            {
              TypeConverter tc = TypeDescriptor.GetConverter(pt);
              if (tc.CanConvertTo(typeof(string)))
              {
                object val = (string)tc.ConvertTo(obj2, typeof(string));
                itemnode.InnerText = val.ToString();
              }
              else
              {
                itemnode.InnerText = value.ToString();
              }
            } // complexclass?
          } // IsCollection?
        } // Loop collection
      } // IsDictionary?
    }

    #endregion SetCollectionItems

    #region Misc

    /// <summary>
    /// Builds the Hashtable that will be written to XML as the type dictionary,
    /// </summary>
    /// <returns></returns>
    /// <remarks>
    /// While serialization the key of the type dictionary is the Type so it's easy to determine
    /// whether a Type is registered already. For deserialization the order is reverse: find a Type
    /// for a given key. 
    /// This methods creates a reversed Hashtable with the Types information stored in TypeInfo instances.
    /// </remarks>
    protected Hashtable BuildSerializeableTypeDictionary()
    {
      Hashtable ht = new Hashtable();

      IDictionaryEnumerator de = typedictionary.GetEnumerator();
      while (de.MoveNext())
      {
        Type type = (Type)de.Entry.Key;
        string key = (String)de.Value;

        TypeInfo ti = new TypeInfo(type);
        ht.Add(key, ti);
      }
      return ht;
    }

    /// <summary>
    /// Gets the key of a Type from the type dictionary.
    /// <p/>If the Type is not registered, yet, it will be registered here.
    /// </summary>
    /// <param name="obj"></param>
    /// <returns>If the given Object is null, null, otherwise the Key of the Objects Type.</returns>
    protected string GetTypeKey(object obj)
    {
      if (obj == null)
	  {  return null; }

      return GetTypeKey(obj.GetType());
    }

    /// <summary>
    /// Gets the key of a Type from the type dictionary.
    /// <p/>If the Type is not registered, yet, it will be registered here.
    /// </summary>
    /// <param name="type"></param>
    /// <returns>If the given Type is null, null, otherwise the Key of the Type.</returns>
    protected string GetTypeKey(Type type)
    {
      if (type == null)
      {  return null;   }

      if (!typedictionary.ContainsKey(type))
      {
        typedictionary.Add(type, TYPE_KEY_PREFIX + typedictionary.Count);
      }

      return (String)typedictionary[type];
    }

    /// <summary>
    /// Writes the TypeDictionary to XML.
    /// </summary>
    /// <param name="parentNode"></param>
    protected void WriteTypeDictionary(XmlNode parentNode)
    {
      if (UseTypeDictionary)
      {
        XmlElement dictelem = parentNode.OwnerDocument.CreateElement(XmlSerializationTag.TYPE_DICTIONARY);
        parentNode.AppendChild(dictelem);
        Hashtable dict = BuildSerializeableTypeDictionary();

        // Temporary set UseTypeDictionary to false, otherwise TypeKeys instead of the
        // Type information will  be written
        UseTypeDictionary = false;

        SetObjectInfoAttributes(null, dict.GetType(), dictelem);
        SerializeProperties(dict, dictelem);

        // Reset UseTypeDictionary
        UseTypeDictionary = true;
      }
    }

    /// <summary>
    /// Clears the Collections.
    /// </summary>
    public void Reset()
    {
      if (this.objlist != null)
	  {  this.objlist.Clear(); }

      if (this.typedictionary != null)
      {  this.typedictionary.Clear();  }
    }

    /// <summary>
    /// Dispose, release references.
    /// </summary>
    public void Dispose()
    {
      Reset();
      GC.SuppressFinalize(this);
    }

    #endregion Misc
  }
}
