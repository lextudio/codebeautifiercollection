JEDI Code Format Integration Expert 1.0 for Delphi 2005

Submitted by Lex Mark

========================
===  Features        ===
========================
a simple JEDI Code Format integration (requires JEDI Code Format jcf.exe from http://jedicodeformat.sourceforge.net/)

========================
===  Install/Unistall===
========================
1.After building the project in Delphi 2005 (maybe you have to change the reference to Borland.Studio.ToolsAPI.dll), run the batch file "install for D2005.bat". This will cause a new item called JcfExpert to be added to your IDE menu the next time you load.

2.run the batch file "uninstall for D2005.bat" to uninstall.

========================
===  Usage           ===
========================
1.Select "JCF Options". on the JEDI Code Format page, browse for the folder which contain the executable "jcf.exe". Click OK.

2.Click "JEDI Code Format" to format current file, the original file is saved in the same folder with the extension ".bak".
 
3.The default setting uses the JCF commandline

-clarify -backup -f -y 

you can change it to whatever you like, according to the Help file.

========================
===  Compatiblity    ===
========================
Tested under Delphi 2005 Update 1 and Windows XP Professional SP2. But I guess that after some slight modifications, the expert should be able to integrate in Delphi 8 for .NET (I didn't try it because I didn't have D8). 

If you're searching for experts to integrate in Delphi 5/6/7. You can find them on JEDI Code Format website (http://jedicodeformat.sourceforge.net/).

========================
===  Notice          ===
========================
This expert (JEDI Code Format Integration Expert 1.0 for Delphi 2005) is free and open-source, and covered by the Mozilla Public License (MPL). I chose it because JEDI Code Format is covered by it, too. you can view the License here: http://www.mozilla.org/MPL/MPL-1.1.html

If you improve this expert in any way (bug fix, new feature, better algorithm, whatever), I would appreciate it if you send those improvements back to me for possible inclusion in a future version. Thanks.

Lex Mark(e_libra@tom.com)

========================
===  TODO in v.1.1   ===
========================

1.Add an "Undo" menuitem.
2.Add an "Format Current Project" menuitem.
3.Add an "Format All Open Files" menuitem.
4.Add an "Format All Files in Current Folder" menuitem.
5.Rewrite in Delphi language.
