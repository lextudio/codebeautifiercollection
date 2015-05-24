/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2007/11/4
 * Time: 16:11
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using Lextm.ExpertLibrary;

namespace expertmaintainer
{
	class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length != 0)
			{
				return;
			}
			ExpertRegistry.CheckExperts();
		}
	}
}