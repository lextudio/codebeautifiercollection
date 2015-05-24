// This is code dom provider class. 
// Copyright (C) 2007  Lex Y. Li
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System;
using System.CodeDom;
using Borland.Studio.ToolsAPI;
using Lextm.Diagnostics;
using System.Collections.Generic;

namespace Lextm.OpenTools.CodeDom
{
    /// <summary>
    /// Code DOM provider.
    /// </summary>
    public sealed class CodeDomProvider
    {
        private CodeDomProvider()
        {
        }
        /// <summary>
        /// Loads method info into a list.
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="module">Module</param>
        public static void LoadMethodInfoInto(IList<CodeDomInfo> list, IOTAServiceProvider module)
        {
            if (module == null)
            {
                return;
            }
            IOTACodeDomProvider _CodeDomProvider = (IOTACodeDomProvider)module.GetService(typeof(IOTACodeDomProvider));
            if (_CodeDomProvider == null)
            {
                LoggingService.Debug("null code dom provider");
                return;
            }
            IOTACodeDomFile _CodeDomFile = _CodeDomProvider.CodeDomFile;
            if (_CodeDomFile == null)
            {
                LoggingService.Debug("null code dom file");
                return;
            }
            CodeObject _CodeObject = _CodeDomFile.GetDom();
            if (_CodeObject == null)
            {
                LoggingService.Debug("null code object");
                return;
            }
            // unit object.
            CodeCompileUnit _CodeCompileUnit = (CodeCompileUnit)_CodeObject;
            for (int i = 0; i < _CodeCompileUnit.Namespaces.Count; i++)
            {
                // namespace object.
                CodeNamespace _CodeNamespace = _CodeCompileUnit.Namespaces[i];
                for (int j = 0; j < _CodeNamespace.Types.Count; j++)
                {
                    CodeTypeDeclaration _CodeTypeDeclaration = _CodeNamespace.Types[j];
                    if (_CodeTypeDeclaration.IsClass || _CodeTypeDeclaration.IsInterface)
                    {
                        // class, interface.
                        string _ClassName = _CodeTypeDeclaration.Name;
                        for (int k = 0; k < _CodeTypeDeclaration.Members.Count; k++)
                        {
                            CodeTypeMember _CodeTypeMember = _CodeTypeDeclaration.Members[k];
                            CodeMemberMethod method = _CodeTypeMember as CodeMemberMethod;
                            if (method != null)
                            {
                                LoadMethodInto(list, method, _ClassName);
                            }
                            CodeTypeDeclaration type = _CodeTypeMember as CodeTypeDeclaration;
                            if (type != null)
                            {
                                LoadTypeMemberInto(list, type);
                            }
                            CodeMemberProperty _CodeMemberProperty = _CodeTypeMember as CodeMemberProperty;
                            if (_CodeMemberProperty == null)
                            {
                                LoggingService.Warn("something else is here. such as fields");
                                return;
                            }
                            if (_CodeMemberProperty.HasGet)
                            {
                                if (_CodeMemberProperty.GetStatements.Count > 0)
                                {
                                    LoadPropertyInto(list,
                                                       _CodeMemberProperty,
                                                         _CodeMemberProperty.GetStatements[0],
                                                         true,
                                                         _ClassName);
                                }
                                else
                                {
                                    LoadPropertyInto(list, _CodeMemberProperty,
                                                         null,
                                                         true,
                                                         _ClassName);

                                }
                            }
                            if (_CodeMemberProperty.HasSet)
                            {
                                if (_CodeMemberProperty.SetStatements.Count > 0)
                                {
                                    LoadPropertyInto(list, _CodeMemberProperty,
                                                         _CodeMemberProperty.SetStatements[0],
                                                         false,
                                                         _ClassName);
                                }
                                else
                                {
                                    LoadPropertyInto(list, _CodeMemberProperty,
                                                         null,
                                                         false,
                                                         _ClassName);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <remarks>This one is for properties.</remarks>
        private static void LoadPropertyInto(IList<CodeDomInfo> arrayList,
                                               CodeTypeMember codeTypeMember,
                                                 CodeStatement codeStatement,
                                                 bool isGet,
                                                 string className)
        {
            MethodInfo _MethodInfo = MethodInfo.CreateProperty(className, isGet, codeTypeMember, codeStatement);
            arrayList.Add(_MethodInfo);
        }
        /// <remarks>This method is for type.</remarks>
        private static void LoadTypeMemberInto(IList<CodeDomInfo> list,
                                               CodeTypeDeclaration codeTypeDeclaration)
        {
            if (codeTypeDeclaration.IsClass || codeTypeDeclaration.IsInterface)
            {
                for (int i = 0; i < codeTypeDeclaration.Members.Count; i++)
                {
                    CodeTypeMember _CodeTypeMember = codeTypeDeclaration.Members[i];

                    if (_CodeTypeMember is CodeMemberMethod)
                    {
                        // methods
                        LoadMethodInto(list, _CodeTypeMember, codeTypeDeclaration.Name);
                    }
                    else if (codeTypeDeclaration.Members[i] is CodeTypeDeclaration)
                    {
                        // nested type
                        CodeTypeDeclaration _CodeTypeDeclaration = (CodeTypeDeclaration)codeTypeDeclaration.Members[i];

                        if (_CodeTypeDeclaration.IsClass || _CodeTypeDeclaration.IsInterface)
                        {
                            LoadTypeMemberInto(list, _CodeTypeDeclaration);
                        }
                    }
                }
            }
        }
        /// <remarks>This one is for methods.</remarks>
        private static void LoadMethodInto(IList<CodeDomInfo> arrayList,
                                                     CodeTypeMember codeTypeMember,
                                                     string className)
        {
            MethodInfo _MethodInfo = MethodInfo.CreateMethod(className, codeTypeMember);
            arrayList.Add(_MethodInfo);
        }

        /// <summary>
        /// Loads FieldInfo into a list.
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="module">Module</param>
        public static void LoadFieldInfoInto(IList<CodeDomInfo> list, IOTAServiceProvider module)
        {
            if (module == null)
            {
                return;
            }
            IOTACodeDomProvider _CodeDomProvider = (IOTACodeDomProvider)module.GetService(typeof(IOTACodeDomProvider));
            if (_CodeDomProvider == null)
            {
                LoggingService.Debug("null code dom provider");
                return;
            }
            IOTACodeDomFile _CodeDomFile = _CodeDomProvider.CodeDomFile;
            if (_CodeDomFile == null)
            {
                LoggingService.Debug("null code dom file");
                return;
            }
            CodeObject _CodeObject = _CodeDomFile.GetDom();
            if (_CodeObject == null)
            {
                LoggingService.Debug("null code object");
                return;
            }
            CodeCompileUnit _CodeCompileUnit = (CodeCompileUnit)_CodeObject;
            for (int i = 0; i < _CodeCompileUnit.Namespaces.Count; i++)
            {
                CodeNamespace _CodeNamespace = _CodeCompileUnit.Namespaces[i];
                for (int j = 0; j < _CodeNamespace.Types.Count; j++)
                {
                    CodeTypeDeclaration _CodeTypeDeclaration = _CodeNamespace.Types[j];
                    if (_CodeTypeDeclaration.IsClass || _CodeTypeDeclaration.IsInterface)
                    {
                        string _ClassName = _CodeTypeDeclaration.Name;
                        for (int k = 0; k < _CodeTypeDeclaration.Members.Count; k++)
                        {
                            CodeTypeMember _CodeTypeMember = _CodeTypeDeclaration.Members[k];
                            if (_CodeTypeMember is CodeMemberProperty ||
                                _CodeTypeMember is CodeMemberField)
                            {
                                LoadFieldInfoInto(list, _CodeTypeMember, _ClassName);
                            }
                            else if (_CodeTypeMember is CodeTypeDeclaration)
                            {
                                LoadFieldInfoInto(list, _CodeTypeMember);
                            }
                        }
                    }
                }
            }
        }

        private static void LoadFieldInfoInto(IList<CodeDomInfo> list, CodeTypeMember codeTypeMember)
        {
            CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)codeTypeMember;
            if (codeTypeDeclaration.IsClass || codeTypeDeclaration.IsInterface)
            {
                for (int i = 0; i < codeTypeDeclaration.Members.Count; i++)
                {
                    CodeTypeMember _CodeTypeMember = codeTypeDeclaration.Members[i];
                    if (_CodeTypeMember is CodeMemberProperty ||
                        _CodeTypeMember is CodeMemberField)
                    {
                        LoadFieldInfoInto(list, _CodeTypeMember, codeTypeDeclaration.Name);
                    }
                    else if (codeTypeDeclaration.Members[i] is CodeTypeDeclaration)
                    {
                        CodeTypeDeclaration _CodeTypeDeclaration = (CodeTypeDeclaration)codeTypeDeclaration.Members[i];
                        if (_CodeTypeDeclaration.IsClass || _CodeTypeDeclaration.IsInterface)
                        {
                            LoadFieldInfoInto(list, _CodeTypeDeclaration);
                        }
                    }
                }
            }
        }

        private static void LoadFieldInfoInto(IList<CodeDomInfo> list, CodeTypeMember codeTypeMember, string className)
        {
            if (codeTypeMember is CodeMemberProperty ||
                codeTypeMember is CodeMemberField)
            {
                if (codeTypeMember.LinePragma != null)
                {
                    FieldInfo _FieldInfo = new FieldInfo(codeTypeMember, className);
                    list.Add(_FieldInfo);
                }
            }
        }
        /// <summary>
        /// Loads EventInfo into a list.
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="module">Module</param>
        public static void LoadEventInfoInto(IList<CodeDomInfo> list, IOTAServiceProvider module)
        {
            if (module == null)
            {
                return;
            }
            IOTACodeDomProvider _CodeDomProvider = (IOTACodeDomProvider)module.GetService(typeof(IOTACodeDomProvider));
            if (_CodeDomProvider == null)
            {
                LoggingService.Debug("null code dom provider");
                return;
            }
            IOTACodeDomFile _CodeDomFile = _CodeDomProvider.CodeDomFile;
            if (_CodeDomFile == null)
            {
                LoggingService.Debug("null code dom file");
                return;
            }
            CodeObject _CodeObject = _CodeDomFile.GetDom();
            if (_CodeObject == null)
            {
                LoggingService.Debug("null code object");
                return;
            }
            CodeCompileUnit _CodeCompileUnit = (CodeCompileUnit)_CodeObject;
            for (int i = 0; i < _CodeCompileUnit.Namespaces.Count; i++)
            {
                CodeNamespace _CodeNamespace = _CodeCompileUnit.Namespaces[i];
                for (int j = 0; j < _CodeNamespace.Types.Count; j++)
                {
                    CodeTypeDeclaration _CodeTypeDeclaration = _CodeNamespace.Types[j];
                    if (_CodeTypeDeclaration.IsClass || _CodeTypeDeclaration.IsInterface)
                    {
                        string _ClassName = _CodeTypeDeclaration.Name;
                        for (int k = 0; k < _CodeTypeDeclaration.Members.Count; k++)
                        {
                            CodeTypeMember _CodeTypeMember = _CodeTypeDeclaration.Members[k];
                            if (_CodeTypeMember is CodeMemberEvent)
                            {
                                LoadEventInfoInto(list, _CodeTypeMember, _ClassName);
                            }
                            else if (_CodeTypeMember is CodeTypeDeclaration)
                            {
                                LoadEventInfoInto(list, _CodeTypeMember);
                            }
                        }
                    }
                }
            }
        }        

        private static void LoadEventInfoInto(IList<CodeDomInfo> list, CodeTypeMember member)
        {
            CodeTypeDeclaration codeTypeDeclaration = (CodeTypeDeclaration)member;
            if (codeTypeDeclaration.IsClass || codeTypeDeclaration.IsInterface)
            {
                for (int i = 0; i < codeTypeDeclaration.Members.Count; i++)
                {
                    CodeTypeMember _CodeTypeMember = codeTypeDeclaration.Members[i];

                    if (_CodeTypeMember is CodeMemberEvent)
                    {
                        LoadEventInfoInto(list, _CodeTypeMember, codeTypeDeclaration.Name);
                    }
                    else if (codeTypeDeclaration.Members[i] is CodeTypeDeclaration)
                    {
                        CodeTypeDeclaration _CodeTypeDeclaration = (CodeTypeDeclaration)codeTypeDeclaration.Members[i];

                        if (_CodeTypeDeclaration.IsClass || _CodeTypeDeclaration.IsInterface)
                        {
                            LoadEventInfoInto(list, _CodeTypeDeclaration);
                        }
                    }
                }
            }
        }

        private static void LoadEventInfoInto(IList<CodeDomInfo> list, CodeTypeMember codeTypeMember, string className)
        {
            CodeMemberEvent _event = codeTypeMember as CodeMemberEvent;
            if (_event != null)
            {
                if (_event.LinePragma != null)
                {
                    EventInfo _EventInfo = new EventInfo(_event, className);
                    list.Add(_EventInfo);
                }
            }
        }
        /// <summary>
        /// Loads TypeInfo into a list.
        /// </summary>
        /// <param name="list">List</param>
        /// <param name="module">Module</param>
        public static void LoadTypeInfoInto(IList<CodeDomInfo> list, IOTAServiceProvider module)
        {
            if (module == null)
            {
                return;
            }
            IOTACodeDomProvider _CodeDomProvider = (IOTACodeDomProvider)module.GetService(typeof(IOTACodeDomProvider));
            if (_CodeDomProvider == null)
            {
                LoggingService.Debug("null code dom provider");
                return;
            }
            IOTACodeDomFile _CodeDomFile = _CodeDomProvider.CodeDomFile;
            if (_CodeDomFile == null)
            {
                LoggingService.Debug("null code dom file");
                return;
            }
            CodeObject _CodeObject = _CodeDomFile.GetDom();
            if (_CodeObject == null)
            {
                LoggingService.Debug("null code object");
                return;
            }
            CodeCompileUnit _CodeCompileUnit = (CodeCompileUnit)_CodeObject;
            for (int i = 0; i < _CodeCompileUnit.Namespaces.Count; i++)
            {
                CodeNamespace _CodeNamespace = _CodeCompileUnit.Namespaces[i];
                for (int j = 0; j < _CodeNamespace.Types.Count; j++)
                {
                    CodeTypeDeclaration _CodeTypeDeclaration = _CodeNamespace.Types[j];
                    LoadTypeInfoInto(list, _CodeTypeDeclaration);
                }
            }
        }

        private static void LoadTypeInfoInto(IList<CodeDomInfo> arrayList, CodeTypeDeclaration codeTypeDeclaration)
        {
            TypeInfo _TypeInfo = new TypeInfo(codeTypeDeclaration);
            arrayList.Add(_TypeInfo);
            if (codeTypeDeclaration.IsClass)
            {
                for (int i = 0; i < codeTypeDeclaration.Members.Count; i++)
                {
                    if (codeTypeDeclaration.Members[i] is CodeTypeDeclaration)
                    {
                        CodeTypeDeclaration _CodeTypeDeclaration = (CodeTypeDeclaration)codeTypeDeclaration.Members[i];
                        LoadTypeInfoInto(arrayList, _CodeTypeDeclaration);
                    }
                }
            }
        }
    }
}
