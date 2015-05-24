/*
 * Created by SharpDevelop.
 * User: lextm
 * Date: 2007/11/3
 * Time: 16:27
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace Lextm.ExpertLibrary
{
	public interface IExpert
	{
		void CheckInstall();
		string FileName
		{
			get;
		}
		void Install();
	}
}
