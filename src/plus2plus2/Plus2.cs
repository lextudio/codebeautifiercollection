namespace Lextm.CodeBeautifierCollection.Collections {

    using System;
	using System.IO;
	using System.Xml;
using System.Reflection;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Text;
	///<summary>
	/// Plus record.
	/// </summary>
	[Serializable]
	public class Plus2 {

		/// <summary>
		/// Constructor.
		/// </summary>
		public Plus2( ) { }

		private string name = "noname";
		private Feature2[] features;
		private string moduleName;
        /// <summary>Module name.</summary>
        /// <remarks>File name without extension.</remarks>
        public string ModuleName {
            get {
				return moduleName;
			}
			set {
                moduleName = value;
            }
        }
		/// <summary>
		/// Name.
		/// </summary>
		public string Name {
			get {
				return name;
			}
			set {
				name = value;
            }
		}
		/// <summary>
		/// Feature list.
		/// </summary>
		public Feature2[] Features {
			get {
				return features;
			}
			set {
                features = value;
            }
		}

		private const string PatternPlus2 = "*.plus2";
		/// <summary>
		/// Gets .plus2 files.
		/// </summary>
		/// <returns></returns>
//		public static string[] GetPlus2Files() {
//			return Lextm.IO.File.GetFiles(Lextm.Reflection.Assembly.Folder,
//								PatternPlus2);
//		}

		private const string MaskDll = "{0}.dll";
		private const string MaskExe = "{0}.exe";

		/// <summary>Loads assembly for plus.</summary>
		public static Assembly LoadAssembly(string moduleName ) {
			string dllName = String.Format(MaskDll, moduleName); // full path, .dll
			string exeName = String.Format(MaskExe, moduleName); // full path, .exe

			if (File.Exists(dllName)) // dll name is first.
			{
				return Assembly.LoadFrom(dllName);
			} else if (File.Exists(exeName)) {
				return Assembly.LoadFrom(exeName);
			}
			else {
				return null;
			}
		}
#region Load and Save
		/// <summary>
		/// Loads a Plus2 object from file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <returns></returns>
//		public static Plus2 Load(string fileName ) {
//			Debug.Indent();
//			Debug.WriteLine("--> Enter PlusArrayList.LoadPlus2");
//
//			Plus2 result = null;
//
//			if (BeWise.Common.Utils.Utils.FileIsValid(fileName)) {
//				XmlSerializer _Serializer = new XmlSerializer(typeof(Plus2));
//				FileStream _FileStream = null;
//				XmlReader _Reader = null;
//				try {
//					try {
//						_FileStream = File.OpenRead(fileName);
//					}
//					catch (DirectoryNotFoundException) {
//						// no such directory
//						Debug.WriteLine("no such directory");
//						//OTAUtils.AddMessage("No such directory");
//					}
//					catch (FileNotFoundException) {
//						// cannot find the file
//						Debug.WriteLine("no file");
//						//OTAUtils.AddMessage("Cannot find the file");
//					}
//					catch (ArgumentException) {
//						Debug.WriteLine("invalid file name.");
//					}
//					finally {
//
//						_Reader = new XmlTextReader(_FileStream);
//
//						if (_FileStream != null) {
//							result = (Plus2) _Serializer.Deserialize(_Reader);
//						}
//						else {
//							Debug.WriteLine("null file stream");
//						}
//					}
//				}
//				catch (XmlException) {
//					//OTAUtils.AddMessage("Cannot read the plus2 file.");
//				} catch (InvalidOperationException) {
//					Console.WriteLine("deserialization fails");
//				}
//				finally {
//					if (_Reader != null) {
//						_Reader.Close();
//					}
//					else {
//						Debug.WriteLine("null reader");
//					}
//				}
//			}
//			else {
//				Debug.WriteLine("invalid plus2 file");
//			}
//			Debug.WriteLine("<-- Leave PlusArrayList.LoadPlus2");
//			Debug.Unindent();
//			return result;
//		}
		/// <summary>
		/// Saves a Plus2 object to file.
		/// </summary>
		/// <param name="fileName">File name</param>
		/// <param name="plus2">Object</param>
		public static void Save( string fileName, Plus2 plus2 ) {
            Debug.Indent();
			Debug.WriteLine("--> Enter PlusArrayList.SavePlus2");

			XmlSerializer _Serializer = new XmlSerializer(typeof(Plus2));
            Stream _FileStream = null;
            XmlTextWriter _Writer = null;
            
            try {
				_FileStream = new FileStream(fileName, FileMode.Create);

            }
            catch (DirectoryNotFoundException) {
                Debug.WriteLine("no such directory");
                // no such directory
                //OTAUtils.AddMessage("No such directory");
            }
			catch (ArgumentException) {
            	Debug.WriteLine("invalid file name");
			}
            finally {
                _Writer = new XmlTextWriter(_FileStream, new UTF8Encoding());
                _Writer.Formatting = Formatting.Indented;
                
                if (_FileStream != null) {
					// Serialize using the XmlTextWriter.
					Console.WriteLine(plus2);
					_Serializer.Serialize(_Writer, plus2);
                }
                else {
                    Debug.WriteLine("null file stream");
                }
                _Writer.Close();
            }
            Debug.WriteLine("<-- Leave PlusArrayList.SavePlus2");
            Debug.Unindent();
        }
#endregion
	}
}
