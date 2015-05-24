Code Beautifier Collection Expert v2.0 for Delphi 2005

lextm (CSDN.net ID) 提供 

========================
===  功能            ===
========================
为 Delphi 2005 IDE 建立的一些代码美化专家的集合集成专家。(需要下载 JEDI Code Format 项目的命令行工具 jcf.exe， 网址 http://jedicodeformat.sourceforge.net/，AStyle 的命令行工具 astyle.exe，网址  http://sourceforge.net/projects/astyle )

由于 Borland 宣称下一版本的 Delphi 会集成 C++ personality，加入了针对 C/C++ 代码的 AStyle。 

========================
===  安装/卸载       ===
========================
1.在 Delphi 2005 的 C# personality 中编译后 (你也许需要重新设置对Borland.Studio.ToolsAPI.dll的引用), 运行批处理程序 "install for D2005.bat"。下次 IDE 启动时菜单栏会多出一项 Beautifiers。

2.运行批处理程序 "uninstall for D2005.bat" 完成卸载。

========================
===  使用            ===
========================
1.选择菜单项 "CBC Options"。在 JEDI Code Format 页上，浏览到放有 "jcf.exe" 的文件夹路径，单击 OK。
AStyle 的设置类似。

2.单击 "Beautifiers" 会格式化当前文件, 原文件的后缀变为 ".bak" 或者 ".orig".
 
3.本专家调用 jcf.exe 的默认设置为命令行参数

-clarify -backup -f -y

你可以依据帮助文件的说明自行设置合适的参数。

========================
===  兼容性          ===
========================
在Delphi 2005 Update 3 及 Windows XP Professional SP2环境下测试成功。 

如果你在寻找和 Delphi 5/6/7集成的方式，你可以在JEDI Code Format的站点上找到，网址是(http://jedicodeformat.sourceforge.net/).对于 C#Builder  可用 SharpBuilderTools 集成的格式化专家。C++Builder 的用户可以试试  SourceFormat （我没有试过）。

========================
===  协议注意        ===
========================
本专家(全名Code Beautifier Collection Expert for Delphi 2005)是免费开发源代码的。本专家是在随Delphi 2005光盘一同发布的CSharp demo SharpBuilderTools v0.9.1的基础上修改得到的，但是原来的代码没有明显的协议说明。

注意，作者声明：目前的发布版本不提供任何明确或隐含的担保，此类担保包括但不局限于是否适合商业使用或是否适合某类特定目的的用途等。作者不对使用本专家或源于本专家的其他产品而可能导致的问题与损失承担任何责任。

如果你对本专家进行了任何形式的改进 (除错，加入新特性，更好的实现方法……任何形式), 麻烦将你的改进通告我，以便在今后的版本中加入。谢谢。

lextm(cylextm-guard@yahoo.com.cn)

========================
===  开发历史        ===
========================
2005.3.16 JEDI Code Format Integration Expert v1.0.(0) for Delphi 2005
1.添加了最简单的专家集成功能。
2.公开发布的第一个稳定版本。

2005.3.17 v1.0.1
1.升级到与 Delphi 2005 Update 2 兼容。

2005.6.18 Code Beautifier Collection Expert for Delphi 2005 v2.0
1.更名为 Code Beautifier Collection Expert for Delphi 2005，因为加入了 AStyle 的接口。

========================
===  TODO in v.1.1   ===
========================
（也许计划会有改变——计划跟不上变化^-^）
1.加入 "Undo" /撤销.
2.加入 "Format Current Project"/格式化当前工程.
3.加入 "Format All Open Files"/格式化当前打开文件。
4.加入 "Format All Files in Current Folder"/格式化当前文件夹中所有文件。
