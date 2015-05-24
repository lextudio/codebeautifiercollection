JEDI Code Format Integration Expert 1.0 for Delphi 2005

lextm (CSDN.net ID)提供 

========================
===  功能            ===
========================
JEDI Code Format命令行工具的Delphi 2005 IDE集成专家。(需要下载JEDI Code Format项目的命令行工具jcf.exe， 网址http://jedicodeformat.sourceforge.net/)

========================
===  安装/卸载       ===
========================
1.在Delphi 2005中编译后 (你也许需要修改对Borland.Studio.ToolsAPI.dll的引用), 运行批处理程序 "install for D2005.bat"。下次IDE启动时菜单栏会多出一项JcfExpert。

2.运行批处理程序 "uninstall for D2005.bat" 完成卸载。

========================
===  使用            ===
========================
1.选择菜单项 "JCF Options"。在JEDI Code Format页上，浏览到放有"jcf.exe"的文件夹路径，单击OK。

2.单击 "JEDI Code Format" 会格式化当前文件, 原文件的后缀变为".bak".
 
3.本专家调用jcf.exe的默认设置为命令行参数

-clarify -backup -f -y

你可以依据帮助文件的说明自行设置合适的参数。

========================
===  兼容性          ===
========================
在Delphi 2005 Update 1 及 Windows XP Professional SP2环境下测试成功。但是经过一定的修改，应该可以与Delphi 8 for .NET 集成(我没有试过，因为我没有D8). 

如果你在寻找和 Delphi 5/6/7集成的方式，你可以在JEDI Code Format的站点上找到，网址是(http://jedicodeformat.sourceforge.net/).

========================
===  注意            ===
========================
本专家(全名JEDI Code Format Integration Expert 1.0 for Delphi 2005)是免费开发源代码的，基于 Mozilla Public License (MPL)协议。我选择该协议，是因为JEDI Code Format是在该协议下开发的。协议原文请见: http://www.mozilla.org/MPL/MPL-1.1.html

如果你对本专家进行了任何形式的改进 (除错，加入新特性，更好的实现方法……任何形式), 麻烦将你的改进通告我，以便在今后的版本中加入。谢谢。

lextm(e_libra@tom.com)

========================
===  TODO in v.1.1   ===
========================

1.加入 "Undo" /撤销.
2.加入 "Format Current Project"/格式化当前工程.
3.加入 "Format All Open Files"/格式化当前打开文件。
4.加入 "Format All Files in Current Folder"/格式化当前文件夹中所有文件。
5.用Delphi语言重写.