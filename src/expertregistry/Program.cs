/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2007/11/3
 * Time: 16:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Lextm.ExpertLibrary;

namespace expertregistry
{
	class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				return;
			}
			else
			{
				foreach (string arg in args)
				{
					if (arg == "/u")
					{
						ExpertRegistry.UnregisterExpert(args);
						return;
					}
				}
				ExpertRegistry.RegisterExpert(args);
			}
		}
	}
}