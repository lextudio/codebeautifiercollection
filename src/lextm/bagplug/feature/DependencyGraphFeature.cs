using Microsoft.Office.Core;
using Microsoft.Win32;
using Extensibility;
using System.Runtime.InteropServices;
using EnvDTE;
using System.IO;

public class ClassGraphDialog : System.Windows.Forms.Form
{

	EnvDTE.DTE applicationObject;

		//to .dot-file for graphviz
	StreamWriter depfile;
		//list of all classes in current project - 10000 should be enough
	string[] classnames = new string[100001];
		//number of classes
	int nclasses;
	bool dependency;

	// set the application object to refer to
	public void setApplicationObject(EnvDTE.DTE appobj)
	{
		applicationObject = appobj;
	}


	#region " Windows Form Designer generated code "

	public ClassGraphDialog() : base()
	{

		//This call is required by the Windows Form Designer.
		InitializeComponent();

		//Add any initialization after the InitializeComponent() call

	}

	//Form overrides dispose to clean up the component list.
	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if ((components != null))
			{
				components.Dispose();
			}
		}
		base.Dispose(disposing);
	}

	//Required by the Windows Form Designer
	private System.ComponentModel.IContainer components;

	//NOTE: The following procedure is required by the Windows Form Designer
	//It can be modified using the Windows Form Designer.  
	//Do not modify it using the code editor.
	internal System.Windows.Forms.Button Inheritance;
	internal System.Windows.Forms.Button Cancel;
	internal System.Windows.Forms.CheckBox LowEnergy;
	internal System.Windows.Forms.Button Depend;
	[System.Diagnostics.DebuggerStepThrough()]
	private void InitializeComponent()
	{
		this.Depend = new System.Windows.Forms.Button();
		this.Inheritance = new System.Windows.Forms.Button();
		this.Cancel = new System.Windows.Forms.Button();
		this.LowEnergy = new System.Windows.Forms.CheckBox();
		this.SuspendLayout();
		//
		//Depend
		//
		this.Depend.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.Depend.Location = new System.Drawing.Point(8, 48);
		this.Depend.Name = "Depend";
		this.Depend.Size = new System.Drawing.Size(80, 23);
		this.Depend.TabIndex = 0;
		this.Depend.Text = "Dependency";
		//
		//Inheritance
		//
		this.Inheritance.DialogResult = System.Windows.Forms.DialogResult.OK;
		this.Inheritance.Location = new System.Drawing.Point(96, 48);
		this.Inheritance.Name = "Inheritance";
		this.Inheritance.Size = new System.Drawing.Size(80, 23);
		this.Inheritance.TabIndex = 1;
		this.Inheritance.Text = "Inheritance";
		//
		//Cancel
		//
		this.Cancel.CausesValidation = false;
		this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
		this.Cancel.Location = new System.Drawing.Point(184, 48);
		this.Cancel.Name = "Cancel";
		this.Cancel.Size = new System.Drawing.Size(80, 23);
		this.Cancel.TabIndex = 2;
		this.Cancel.Text = "Cancel";
		//
		//LowEnergy
		//
		this.LowEnergy.Location = new System.Drawing.Point(8, 16);
		this.LowEnergy.Name = "LowEnergy";
		this.LowEnergy.Size = new System.Drawing.Size(248, 24);
		this.LowEnergy.TabIndex = 3;
		this.LowEnergy.Text = "balanced or \"Low Energy\" graph";
		//
		//ClassGraphDialog
		//
		this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
		this.ClientSize = new System.Drawing.Size(272, 86);
		this.Controls.Add(this.LowEnergy);
		this.Controls.Add(this.Cancel);
		this.Controls.Add(this.Inheritance);
		this.Controls.Add(this.Depend);
		this.Name = "ClassGraphDialog";
		this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
		this.Text = "ClassGraphDialog";
		this.ResumeLayout(false);

	}

	#endregion

	private void Dependency_Click(object sender, System.EventArgs e)
	{
		dependency = true;
		ScanTree();
	}

	private void Inheritance_Click(object sender, System.EventArgs e)
	{
		dependency = false;
		ScanTree();
	}

	public void ScanTree()
	{
		//scans each project in the current solution
		//for each project, recursively scan all classes and structs for
		//base classes/structs or inner classes/structs
		int i;
		int j;
		int k;
		Project project;
		CodeElement cl;
		string dir;

		StreamWriter webfile;
		//to open the picture inside visual studio
		try {
			Directory.CreateDirectory(Path.GetDirectoryName(applicationObject.Solution.FullName) + "\\Doc");
		}
		catch {
		}
		if (dependency)
		{
			webfile = File.CreateText(Path.GetDirectoryName(applicationObject.Solution.FullName) + "\\Doc\\depgraph.html");
			webfile.WriteLine("<html><head><title>ClassGraph</title></head><body bgcolor=\"#DDDDDD\"><br><br><center>");
		}
		else
		{
			webfile = File.CreateText(Path.GetDirectoryName(applicationObject.Solution.FullName) + "\\Doc\\inhgraph.html");
			webfile.WriteLine("<html><head><title>ClassGraph</title></head><body bgcolor=\"#DDDDDD\"><br><br><center>");
		}

		foreach ( project in applicationObject.Solution) {
			try {
				dir = Path.GetDirectoryName(project.FullName) + "\\";
				if ((File.Exists(dir + "dep.txt")))
				{
					File.Delete(dir + "dep.txt");
				}
				depfile = File.CreateText(dir + "dep.txt");
				depfile.WriteLine("digraph G {");
				//the first line for the .dot file
				depfile.WriteLine("overlap = false");
				depfile.WriteLine("spline = true");
				nclasses = 1;
				BuildObjectList(project.CodeModel.CodeElements, 0);
				foreach ( cl in project.CodeModel.CodeElements) {
					if ((cl.Kind == vsCMElement.vsCMElementClass))
					{
						//cl is a class
						ScanBases(cl, 0);
						//scan all base classes/structs
						if ((dependency))
						{
							ScanDependencies(cl);
							//also scan all dependencies of the class
						}
					}
				}
				foreach ( cl in project.CodeModel.CodeElements) {
					if ((cl.Kind == vsCMElement.vsCMElementStruct))
					{
						//cl is a struct
						ScanBases(cl, 0);
						//scan all base classes/structs
						if ((dependency))
						{
							ScanDependencies(cl);
							//also scan al dependencies of the struct
						}
					}
				}
				foreach ( cl in project.CodeModel.CodeElements) {
					if ((cl.Kind == vsCMElement.vsCMElementNamespace))
					{
						//cl is a namespace
						for (i = 0; i <= cl.Collection.Count; i++) {
							CodeElement codelm;
							codelm = cl.Collection.Item(i);
							if ((codelm.Kind == vsCMElement.vsCMElementClass))
							{
								//cl is a class
								ScanBases(codelm, 0);
								//scan all base classes/structs
								if ((dependency))
								{
									ScanDependencies(codelm);
									//also scan all dependencies of the class
								}
							}
							if ((codelm.Kind == vsCMElement.vsCMElementStruct))
							{
								//cl is a struct
								ScanBases(codelm, 0);
								//scan all base classes/structs
								if ((dependency))
								{
									ScanDependencies(codelm);
									//also scan al dependencies of the struct
								}
							}
						}
					}
				}
				depfile.WriteLine("}");
				depfile.Flush();
				depfile.Close();

				string dotdir;
				//since the add-in is a dll loaded by the visual studio itself, there's no way (I know of) to
				//get the directory the dll is located. that directory contains also the graphviz files which
				//are needed for generating the graphs.
				//only way left to get that directory is via the registry and the setup-tool...
				dotdir = Registry.LocalMachine.OpenSubKey(applicationObject.RegistryRoot + "\\AddIns\\Kings.Tools", false).GetValue("SatelliteDLLPath", " ");

				if (dependency)
				{
					if (LowEnergy.Checked)
					{
						applicationObject.ExecuteCommand("Tools.Shell", "/output " + dotdir + "\\neato.exe -Tpng \"" + dir + "dep.txt\" -o \"" + Path.GetDirectoryName(applicationObject.Solution.FullName) + "\\Doc\\" + project.Name + "depgraph.png\"");
					}
					else
					{
						applicationObject.ExecuteCommand("Tools.Shell", "/output " + dotdir + "\\dot.exe -Tpng \"" + dir + "dep.txt\" -o \"" + Path.GetDirectoryName(applicationObject.Solution.FullName) + "\\Doc\\" + project.Name + "depgraph.png\"");
					}
					webfile.WriteLine("<h2>" + project.Name + "</h2>");
					webfile.WriteLine("<img src=\"" + project.Name + "depgraph.png" + "\" border=\"0\"><br>");
				}
				else
				{
					if (LowEnergy.Checked)
					{
						applicationObject.ExecuteCommand("Tools.Shell", "/output " + dotdir + "\\neato.exe -Tpng \"" + dir + "dep.txt\" -o \"" + Path.GetDirectoryName(applicationObject.Solution.FullName) + "\\Doc\\" + project.Name + "inhgraph.png\"");
					}
					else
					{
						applicationObject.ExecuteCommand("Tools.Shell", "/output " + dotdir + "\\dot.exe -Tpng \"" + dir + "dep.txt\" -o \"" + Path.GetDirectoryName(applicationObject.Solution.FullName) + "\\Doc\\" + project.Name + "inhgraph.png\"");
					}
					webfile.WriteLine("<h2>" + project.Name + "</h2>");
					webfile.WriteLine("<img src=\"" + project.Name + "inhgraph.png" + "\" border=\"0\"><br>");
				}
				File.Delete(dir + "dep.txt");
			}
			catch (Exception e) {
				Debug.WriteLine(e.Message);
				Debug.WriteLine(e.StackTrace);
			}
		}
		webfile.WriteLine("</center></body></html>");
		webfile.Close();
		if (dependency)
		{
			applicationObject.ItemOperations.Navigate(Path.GetDirectoryName(applicationObject.Solution.FullName) + "\\Doc\\depgraph.html", vsNavigateOptions.vsNavigateOptionsNewWindow);
		}
		else
		{
			applicationObject.ItemOperations.Navigate(Path.GetDirectoryName(applicationObject.Solution.FullName) + "\\Doc\\inhgraph.html", vsNavigateOptions.vsNavigateOptionsNewWindow);
		}
	}

	public void ScanBases(CodeElement c, int level)
	{
		try {
			if (c.Name.Length != 0)
			{
				int i;
				CodeElement child;
				foreach ( child in c.Children) {
					if ((child.Kind == vsCMElement.vsCMElementClass) & child.Name.Length != 0)
					{
						//inner class
						if (level == 0)
						{
							depfile.WriteLine("subgraph cluster" + c.Name + "{");
							depfile.WriteLine("style=filled;");
							depfile.WriteLine("color=lightgrey;");
							depfile.Write(c.Name + " ");
						}
						depfile.Write("-> " + child.Name);
						ScanBases(child, level + 1);
						if (level == 0)
						{
							//root object
							depfile.WriteLine(";");
							depfile.WriteLine("}");
						}
					}
					if ((child.Kind == vsCMElement.vsCMElementStruct) & child.Name.Length != 0)
					{
						//inner struct
						if (level == 0)
						{
							depfile.WriteLine("subgraph cluster" + c.Name + "{");
							depfile.WriteLine("style=filled;");
							depfile.WriteLine("color=lightgrey;");
							depfile.Write(c.Name + " ");
						}
						depfile.Write("-> " + child.Name);
						ScanBases(child, level + 1);
						if (level == 0)
						{
							//root object
							depfile.WriteLine(";");
							depfile.WriteLine("}");
						}
					}
				}
				if ((c.Kind == vsCMElement.vsCMElementClass))
				{
					if (level == 0)
					{
						depfile.Write(c.Name + " ");
					}
					for (i = 1; i <= c.Bases.Count; i++) {
						try {
							if ((IsProjectClass(c.Bases.Item(i).Name)))
							{
								depfile.Write("-> " + c.Bases.Item(i).Name);
								ScanBases(c.Bases.Item(i), 0);
							}
						}
						catch (Exception e) {
							Debug.WriteLine(e.Message);
							Debug.WriteLine(e.StackTrace);
						}
						if ((i < c.Bases.Count))
						{
							depfile.WriteLine(";");
							depfile.Write(c.Name + " ");
						}
					}
					if (level == 0)
					{
						depfile.WriteLine(";");
					}
				}
				//If (c.Kind = vsCMElement.vsCMElementNamespace) Then
				//    If level = 0 Then
				//        depfile.Write(c.Name + " ")
				//    End If
				//    For i = 1 To c.Collection.Count
				//        Try
				//            If (IsProjectClass(c.Collection.Item(i).Name)) Then
				//                depfile.Write("-> " + c.Collection.Item(i).Name)
				//                If level = 0 Then ScanBases(c.Collection.Item(i), 0)
				//            End If
				//        Catch e As Exception
				//            Debug.WriteLine(e.Message)
				//            Debug.WriteLine(e.StackTrace)
				//        End Try
				//        If (i < c.Collection.Count) Then
				//            depfile.WriteLine(";")
				//            depfile.Write(c.Name + " ")
				//        End If
				//    Next
				//    If level = 0 Then
				//        depfile.WriteLine(";")
				//    End If
				//End If
			}
		}
		catch (Exception e) {
			Debug.WriteLine(e.Message);
			Debug.WriteLine(e.StackTrace);
		}
	}

	public void ScanDependencies(CodeElement c)
	{
		try {
			if (c.Name.Length != 0)
			{
				int i;
				CodeClass cc;
				CodeStruct cs;
				CodeElement ce;
				CodeVariable cv;

				if ((c.Kind == vsCMElement.vsCMElementClass))
				{
					cc = c;
					foreach ( ce in cc.Children) {
						try {
							if ((ce.Kind == vsCMElement.vsCMElementVariable) & ce.Name.Length != 0)
							{
								cv = ce;
								if ((IsProjectClass(c.Name)))
								{
									if ((IsProjectClass(cv.Type.AsFullName)) & cv.Name.Length != 0)
									{
										depfile.WriteLine(cv.Type.AsFullName + " -> " + c.Name + " [style=dashed,color=darkgreen,label=" + cv.Name + "];");
									}

									if ((cv.Type.TypeKind == vsCMTypeRef.vsCMTypeRefArray) & cv.Name.Length != 0)
									{
										if ((IsProjectClass(cv.Type.ElementType.AsFullName)))
										{
											depfile.WriteLine(cv.Type.ElementType.AsFullName + " -> " + c.Name + " [style=dashed,color=red,label=\"" + cv.Name + "[" + "]\"" + "];");
										}
									}
									if ((cv.Type.TypeKind == vsCMTypeRef.vsCMTypeRefPointer) & cv.Name.Length != 0)
									{
										string s;
										s = cv.Type.AsFullName;

										s = s.Remove(s.Length - 2, 2);
										if ((IsProjectClass(s)))
										{
											depfile.WriteLine(s + " -> " + c.Name + " [style=dashed,color=blue,label=" + cv.Name + "];");
										}
									}

								}
							}
						}
						catch (Exception e) {
							Debug.WriteLine(e.Message);
							Debug.WriteLine(e.StackTrace);

						}
					}
				}
			}
		}
		catch (Exception e) {
			Debug.WriteLine(e.Message);
			Debug.WriteLine(e.StackTrace);
		}
	}

	//check if object is part of the project or 'external' i.e. system related
	public bool IsProjectClass(string s)
	{
		string name;
		s = s.Trim();
		if ((s.Length > 1))
		{
			foreach ( name in classnames) {
				if ((s == name))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void BuildObjectList(CodeElements ce, int level)
	{
		CodeElement cl;
		foreach ( cl in ce) {
			try {
				if ((cl.Kind == vsCMElement.vsCMElementClass) & cl.Name.Length != 0)
				{
					classnames(nclasses) = cl.Name;
					if ((level > 0))
					{
						depfile.WriteLine(cl.Name + " [shape=box,style=filled,color=lightsalmon];");
					}
					else
					{
						depfile.WriteLine(cl.Name + " [shape=box,style=filled,color=lightpink];");
					}
					nclasses = nclasses + 1;
					BuildObjectList(cl.Children, level + 1);
				}
				if ((cl.Kind == vsCMElement.vsCMElementNamespace) & cl.Name.Length != 0)
				{
					if (!IsProjectClass(cl.Name))
					{
						classnames(nclasses) = cl.Name;
						if ((level > 0))
						{
							depfile.WriteLine(cl.Name + " [shape=box,style=filled,color=lightsalmon];");
						}
						else
						{
							depfile.WriteLine(cl.Name + " [shape=box,style=filled,color=lightpink];");
						}
						nclasses = nclasses + 1;
						//If level = 0 Then BuildObjectList(cl.Collection, level + 1)
					}
				}
				if ((cl.Kind == vsCMElement.vsCMElementStruct) & cl.Name.Length != 0)
				{
					classnames(nclasses) = cl.Name;
					if ((level > 0))
					{
						depfile.WriteLine(cl.Name + " [shape=box,style=filled,color=lightskyblue];");
					}
					else
					{
						depfile.WriteLine(cl.Name + " [shape=box,style=filled,color=lightslateblue];");
					}
					nclasses = nclasses + 1;
					BuildObjectList(cl.Children, level + 1);
				}
			}
			catch (Exception e) {
				Debug.WriteLine(e.Message);
				Debug.WriteLine(e.StackTrace);

			}
		}
	}
}
