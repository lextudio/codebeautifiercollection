using System;
using System.IO;
using Lextm.CodeBeautifierCollection.Collections;
using System.Collections;

namespace Plus2Plus2
{
	/// <summary>
	/// Summary description for Class.
	/// </summary>
	class Class
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			//
			// TODO: Add code to start application here
			//
			string plusFileName = null;

			if (args.Length == 1)
			{
				if (args[0].StartsWith("-f:"))
				{
					plusFileName = args[0].Substring(3);
					//Console.WriteLine("temp is " + tmp);
                }
			}
			if (plusFileName == null) {
				Console.WriteLine("plus file is null");
				Console.Read();
				return;
            }

			Console.WriteLine("plus file is " + plusFileName);

			string plus2FileName = Path.ChangeExtension(plusFileName, ".plus2");

			if (!File.Exists(plusFileName)) 
			{
                Console.WriteLine("file is not there: " + plusFileName);
                Console.Read();
				return;
			}

            Console.WriteLine("plus to plus2: " + plus2FileName);

			Plus2 plus = CreatePlus2Object(plusFileName);

			Console.WriteLine("plus2 object is created");

			Plus2.Save(plus2FileName, plus);

            Console.WriteLine("Done");

        	Console.Read();
		}

		private static Plus2 CreatePlus2Object(string fileName) {
			Plus2 result = new Plus2();

			result.Name = fileName.Split('.')[1];
			Console.WriteLine("plus2 name is " + result.Name);

			result.ModuleName = Path.ChangeExtension(fileName, null);
			Console.WriteLine("plus2 module is " + result.ModuleName);

			ArrayList features = new ArrayList();

			foreach (string line in GetLines(fileName)) {
				//creates feature objects.
				string[] properties = line.Split(';');
				if (properties.Length == 2) { // valid plus I lines
					Feature2 feature = new Feature2();
					feature.Name = properties[0];
					feature.Description = "nohint";

					ArrayList records = new ArrayList();
                    //for BDS 1-4 versions
					for (int i = 1; i < 5; i++) {
						EnabledRecord record = new EnabledRecord();
						record.Version = i;
						record.Enabled = Convert.ToBoolean(properties[1]);

						records.Add(record);
                    }
					feature.EnabledRecords = (EnabledRecord[]) records.ToArray(typeof(EnabledRecord));

					features.Add(feature);
				}
			}
			result.Features = (Feature2[]) features.ToArray(typeof(Feature2));

			return result;
		}

		private static System.Collections.ArrayList GetLines( string fileName ) {
			ArrayList result = new ArrayList();
			using (StreamReader sr = new StreamReader(fileName)) {
				String line;
				// Read and display lines from the file until the end of 
				// the file is reached.
				while ((line = sr.ReadLine()) != null) 
				{
					result.Add(line);
				}
			}
	
			return result;
		}		
	}
}
