Code Beautifier Collection Expert v2.0 for Delphi 2005

Submitted by Lex Mark

========================
===  Features        ===
========================
a code beautifier collection for Delphi/C/C++/C#/XML source code (requires JEDI Code Format jcf.exe from http://jedicodeformat.sourceforge.net/,
AStyle astyle.exe from http://sourceforge.net/projects/astyle).

Since Borland claims that in next version of Delphi, C++ personality would be included, I add AStyle interface for C/C++ files.

========================
===  Install/Unistall===
========================
1.After building the project in Delphi 2005 (you have to use the proper reference to Borland.Studio.ToolsAPI.dll), run the batch file "install for D2005.bat". This will cause a new item called Beautifier to be added to your IDE menu the next time you load (CSharp personality is necessary). 

2.run the batch file "uninstall for D2005.bat" to uninstall.

========================
===  Usage           ===
========================
1.Select "CBC Options". on the JEDI Code Format page, browse for the folder which contain the executable "jcf.exe". Click OK.

2.Click "Beautifiers" to format current file, the original file is saved in the same folder with the extension ".bak" or ".orig".
 
3.The default setting uses the JCF commandline

-clarify -backup -f -config="./JCF2Settings.cfg" -y

you can change it to whatever you like, according to the JCF's Help file.

========================
===  Compatiblity    ===
========================
Tested under Delphi 2005 Update 3 on Windows XP Professional SP2. 

If you're searching for experts to integrate in Delphi 5/6/7. You can find them on JEDI Code Format website (http://jedicodeformat.sourceforge.net/), SharpBuilderTools ships with a C# beautifier for C#Builder, and C++Builder users may try SourceFormat (I hadn't tried).

========================
===  License Notice  ===
========================
This expert (Code Beautifier Collection Expert v2.0 for Delphi 2005) is free and open-source. 

The software is distributed and provided to you "AS IS" without warranty of any kind, either expressed or implied, including, but without limitation, warranties that the software is free of defects, merchantable, or fit for a particular purpose. The entire risk as to the quality and performance of the software is with you. Should the software prove defective in any respect, you (and not ME), assume the cost of any necessary servicing, repair or correction.

This disclaimer of warranty constitutes an essential part of this license.

If you improve this expert in any way (bug fix, new feature, better algorithm, whatever), I would appreciate it if you send those improvements back to me for possible inclusion in a future version. Thanks.

lextm (cylextm-guard@yahoo.com.cn)

========================
===  History         ===
========================
2005.6.18 Code Beautifier Collection Expert v2.0 for Delphi 2005
+ 1. rename to Code Beautifier Collection Expert, since AStyle interface is added.

2005.3.17 v1.0.1
* 1.Update to Delphi 2005 Update 2 compatible.

2005.3.16 JEDI Code Format Integration Expert v1.0.(0) for Delphi 2005
1.Add simplest feature for Jcf.exe.
2.Initial stable version to public.

========================
===  TODO in v.2.1   ===
========================
(Possible to change a little bit)
1.Add an "Undo" menuitem.
2.Add an "Format Current Project" menuitem.
3.Add an "Format All Open Files" menuitem.
4.Add an "Format All Files in Current Folder" menuitem.

If you have any suggestion, please email me.

