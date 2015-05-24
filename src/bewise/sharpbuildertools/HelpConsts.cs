using System;

namespace BeWise.SharpBuilderTools {
	/// <summary>
	/// Not to used.
	/// </summary>
	public class HelpHeader {
		/// <summary>
		/// Not used.
		/// </summary>
        public const string Introduction = "Introduction";
        /// <summary>
        /// Not used.
        /// </summary>
        public const string Helpers = "Helpers";
        /// <summary>
        /// Not used.
        /// </summary>
        public const string Options = "Options";
        /// <summary>
        /// Not used.
        /// </summary>
        public const string Tools = "Tools";
        /// <summary>
        /// Not used.
        /// </summary>
        public const string Wizards = "Wizards";
    }

    /// <summary>
    /// Not to used.
    /// </summary>
    public class HelpTopic {
		/// <summary>
		/// Not to used.
		/// </summary>
        public const string Introduction =
"<span style=\"font-family:Helvetica,Arial; font-size:10pt; color:#000000\"> <br>" +
"<b>Version v2.0.0.1 Updated October 2003</b><br>" +
"<br>" +
"Welcome to SharpBuilderTools, a C#Builder IDE enhancements package. <br>" +
"<br>" +
"SharpBuilderTools is a set of tools built to increase the productivity of C#Builder" +
"programmers by adding several features to the IDE. SharpBuilderTools is developed" +
"as Open Source software and encourages user contributions to the project. <br>" +
"<br>" +
"SharpBuilderTools would not exist without the help and advice of some people." +
"David Hervieux is the lead developer of SharpBuilderTools. Valentino Kyriakides" +
"is contributing to SharpBuilderTools as a co-worker for the project. The project" +
"is Open Source in order to speed development and further enhance the quality of" +
"the experts. For a list of credits, see the author section in the Readme.txt." +
"<br>" +
"<br>" +
"Please take a moment to read the Readme.txt file included with SharpBuilderTools," +
"which will often contain information not included in this help file including" +
"the latest changes, known bugs, and plans for the future. <br>" +
"<br>" +
"The latest version of SharpBuilderTools is always available at: <span style=\"font-family:Helvetica,Arial; font-size:10pt; color:#0000FF\"><u>" +
"http://sourceforge.net/projects/sbtools/</u></span><span style=\"font-family:Helvetica,Arial; font-size:10pt; color:#000000\">." +
"You can send bug reports, suggestions and feedback to David Hervieux via e-mail:" +
"<a href=\"mailto:dhervieux@videotron.ca\">dhervieux@videotron.ca</a>. <br>" +
"</span></span>";
	}

	/// <summary>
	/// Not to used.
	/// </summary>
    public class HelpAction {

    	private HelpAction( ) { }    /*
		// *************************************************************************
        //                         AddFileTreeMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string AddFileTreeMenuHelpText {
            get {
                return "Add File Tree...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string AddFileTreeMenuHelpTitle {
            get {
                return "Add File Tree...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string AddFileTreeMenuHelpTopic {
            get {
                return "SBTProjectMenu";
            }
        }

        // *************************************************************************
        //                         AntMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string AntMenuHelpText {
            get {
                return "Ant / NAnt";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string AntMenuHelpTitle {
            get {
                return "Ant / NAnt";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string AntMenuHelpTopic {
            get {
                return "SBTToolMenu";
            }
        }

        // *************************************************************************
        //                         AppendToClipBoardMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string AppendToClipBoardMenuHelpText {
            get {
                return "this is a desc";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string AppendToClipBoardMenuHelpTitle {
            get {
                return "Append selection to clipboard";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string AppendToClipBoardMenuHelpTopic {
            get {
                return "Clipboard";
            }
        }

        // *************************************************************************
        //                         BackMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string BackMenuHelpText {
            get {
                return "Back";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string BackMenuHelpTitle {
            get {
                return "Back";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string BackMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         CloseOpenTabsExceptCurrentMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string CloseOpenTabsExceptCurrentMenuHelpText {
            get {
                return "Close All Tabs (Except current file)";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CloseOpenTabsExceptCurrentMenuHelpTitle {
            get {
                return "Close All Tabs (Except current file)";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CloseOpenTabsExceptCurrentMenuHelpTopic {
            get {
                return "SBTViewMenu";
            }
        }

        // *************************************************************************
        //                         CloseOpenTabsMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string CloseOpenTabsMenuHelpText {
            get {
                return "Close All Tabs";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CloseOpenTabsMenuHelpTitle {
            get {
                return "Close All Tabs";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CloseOpenTabsMenuHelpTopic {
            get {
                return "SBTViewMenu";
            }
        }

        // *************************************************************************
        //                         CodeBeautifierMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string CodeBeautifierMenuHelpText {
            get {
                return "Code Beautifier";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CodeBeautifierMenuHelpTitle {
            get {
                return "Code Beautifier";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CodeBeautifierMenuHelpTopic {
            get {
                return "SBTToolMenu";
            }
        }

        // *************************************************************************
        //                         CodeCompleteMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string CodeCompleteMenuHelpText {
            get {
                return "Show Code Complete";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CodeCompleteMenuHelpTitle {
            get {
                return "Show Code Complete";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CodeCompleteMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         CodeCompleteParamsMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string CodeCompleteParamsMenuHelpText {
            get {
                return "Show Code Complete Params";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CodeCompleteParamsMenuHelpTitle {
            get {
                return "Show Code Complete Params";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CodeCompleteParamsMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         CodeTemplateMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string CodeTemplateMenuHelpText {
            get {
                return "Insert Code Template";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CodeTemplateMenuHelpTitle {
            get {
                return "Insert Code Template";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CodeTemplateMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         CommentMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string CommentMenuHelpText {
            get {
                return "Comment";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CommentMenuHelpTitle {
            get {
                return "Comment";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CommentMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         CompareFileMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string CompareFileMenuHelpText {
            get {
                return "Compare file ...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CompareFileMenuHelpTitle {
            get {
                return "Compare file ...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CompareFileMenuHelpTopic {
            get {
                return "WinMergeMenu";
            }
        }

        // *************************************************************************
        //                         CopyFileMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string CopyFileMenuHelpText {
            get {
                return "Copy File...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CopyFileMenuHelpTitle {
            get {
                return "Copy File...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CopyFileMenuHelpTopic {
            get {
                return "SBTProjectMenu";
            }
        }

        // *************************************************************************
        //                         CopyFileNameToClipBoardMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string CopyFileNameToClipBoardMenuHelpText {
            get {
                return "Send file name to clipboard";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CopyFileNameToClipBoardMenuHelpTitle {
            get {
                return "Send file name to clipboard";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CopyFileNameToClipBoardMenuHelpTopic {
            get {
                return "SBTClipboardMenu";
            }
        }

        // *************************************************************************
        //                         CopyFilePathToClipBoardMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string CopyFilePathToClipBoardMenuHelpText {
            get {
                return "Send file path to clipboard";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CopyFilePathToClipBoardMenuHelpTitle {
            get {
                return "Send file path to clipboard";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CopyFilePathToClipBoardMenuHelpTopic {
            get {
                return "SBTClipboardMenu";
            }
        }

        // *************************************************************************
        //                         CreateRegionMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string CreateRegionMenuHelpText {
            get {
                return "Create Region...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CreateRegionMenuHelpTitle {
            get {
                return "Create Region...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CreateRegionMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         CreateToDoMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string CreateToDoMenuHelpText {
            get {
                return "Create To Do...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CreateToDoMenuHelpTitle {
            get {
                return "Create To Do...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string CreateToDoMenuHelpTopic {
            get {
                return "SBTCodeWizardMenu";
            }
        }

        // *************************************************************************
        //                         DeleteCurrentModuleMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string DeleteCurrentModuleMenuHelpText {
            get {
                return "Delete Current Module...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string DeleteCurrentModuleMenuHelpTitle {
            get {
                return "Delete Current Module...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string DeleteCurrentModuleMenuHelpTopic {
            get {
                return "SBTProjectMenu";
            }
        }

        // *************************************************************************
        //                         DeleteProjectTargetMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string DeleteProjectTargetMenuHelpText {
            get {
                return "Delete Project Target...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string DeleteProjectTargetMenuHelpTitle {
            get {
                return "Delete Project Target...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string DeleteProjectTargetMenuHelpTopic {
            get {
                return "SBTProjectMenu";
            }
        }

        // *************************************************************************
        //                         ElideNearestBlockMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ElideNearestBlockMenuHelpText {
            get {
                return "Fold Nearest Block                       Ctrl+Shift+K+E";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ElideNearestBlockMenuHelpTitle {
            get {
                return "Fold Nearest Block                       Ctrl+Shift+K+E";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ElideNearestBlockMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         ExpertManagerMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ExpertManagerMenuHelpText {
            get {
                return "Expert Manager...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ExpertManagerMenuHelpTitle {
            get {
                return "Expert Manager...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ExpertManagerMenuHelpTopic {
            get {
                return "SBTOptionsMenu";
            }
        }

        // *************************************************************************
        //                         ExpressoMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ExpressoMenuHelpText {
            get {
                return "Expresso";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ExpressoMenuHelpTitle {
            get {
                return "Expresso";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ExpressoMenuHelpTopic {
            get {
                return "SBTToolMenu";
            }
        }

        // *************************************************************************
        //                         FavoritesMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string FavoritesMenuHelpText {
            get {
                return "Favorites...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string FavoritesMenuHelpTitle {
            get {
                return "Favorites...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string FavoritesMenuHelpTopic {
            get {
                return "SBTViewMenu";
            }
        }

        // *************************************************************************
        //                         ForwardMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ForwardMenuHelpText {
            get {
                return "Forward";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ForwardMenuHelpTitle {
            get {
                return "Forward";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ForwardMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         FxCopAnalyseMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string FxCopAnalyseMenuHelpText {
            get {
                return "Analyse";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string FxCopAnalyseMenuHelpTitle {
            get {
                return "Analyse";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string FxCopAnalyseMenuHelpTopic {
            get {
                return "FxCopMenu";
            }
        }

        // *************************************************************************
        //                         FxCopMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string FxCopMenuHelpText {
            get {
                return "FxCop";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string FxCopMenuHelpTitle {
            get {
                return "FxCop";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string FxCopMenuHelpTopic {
            get {
                return "SBTToolMenu";
            }
        }

        // *************************************************************************
        //                         GenerateDefaultDocMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string GenerateDefaultDocMenuHelpText {
            get {
                return "Generate Default Project Doc";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GenerateDefaultDocMenuHelpTitle {
            get {
                return "Generate Default Project Doc";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GenerateDefaultDocMenuHelpTopic {
            get {
                return "NDocMenu";
            }
        }

        // *************************************************************************
        //                         GenerateDocMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string GenerateDocMenuHelpText {
            get {
                return "Generate Doc ...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GenerateDocMenuHelpTitle {
            get {
                return "Generate Doc ...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GenerateDocMenuHelpTopic {
            get {
                return "NDocMenu";
            }
        }

        // *************************************************************************
        //                         GetFilePropertiesMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string GetFilePropertiesMenuHelpText {
            get {
                return "File Properties...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GetFilePropertiesMenuHelpTitle {
            get {
                return "File Properties...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GetFilePropertiesMenuHelpTopic {
            get {
                return "SBTProjectMenu";
            }
        }

        // *************************************************************************
        //                         GotoInitializeComponentMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoInitializeComponentMenuHelpText {
            get {
                return "Go to InitializeComponent";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoInitializeComponentMenuHelpTitle {
            get {
                return "Go to InitializeComponent";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoInitializeComponentMenuHelpTopic {
            get {
                return "SBTViewMenu";
            }
        }

        // *************************************************************************
        //                         GoToNextErrorMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string GoToNextErrorMenuHelpText {
            get {
                return "Goto to Next Error";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GoToNextErrorMenuHelpTitle {
            get {
                return "Goto to Next Error";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GoToNextErrorMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         GotoNextMethodMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoNextMethodMenuHelpText {
            get {
                return "Go to Next Method";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoNextMethodMenuHelpTitle {
            get {
                return "Go to Next Method";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoNextMethodMenuHelpTopic {
            get {
                return "SBTViewMenu";
            }
        }

        // *************************************************************************
        //                         GotoNextOccurenceMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoNextOccurenceMenuHelpText {
            get {
                return "Go to Next Occurence";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoNextOccurenceMenuHelpTitle {
            get {
                return "Go to Next Occurence";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoNextOccurenceMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         GoToPreviousErrorMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string GoToPreviousErrorMenuHelpText {
            get {
                return "Goto to Previous Error";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GoToPreviousErrorMenuHelpTitle {
            get {
                return "Goto to Previous Error";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GoToPreviousErrorMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         GotoPreviousMethodMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoPreviousMethodMenuHelpText {
            get {
                return "Go to Previous Method";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoPreviousMethodMenuHelpTitle {
            get {
                return "Go to Previous Method";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoPreviousMethodMenuHelpTopic {
            get {
                return "SBTViewMenu";
            }
        }

        // *************************************************************************
        //                         GotoPriorOccurenceMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoPriorOccurenceMenuHelpText {
            get {
                return "Go to Prior Occurence";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoPriorOccurenceMenuHelpTitle {
            get {
                return "Go to Prior Occurence";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string GotoPriorOccurenceMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         HyperBackwardMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string HyperBackwardMenuHelpText {
            get {
                return "Hyper Backward";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string HyperBackwardMenuHelpTitle {
            get {
                return "Hyper Backward";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string HyperBackwardMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         HyperForwardMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string HyperForwardMenuHelpText {
            get {
                return "Hyper Forward";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string HyperForwardMenuHelpTitle {
            get {
                return "Hyper Forward";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string HyperForwardMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         IndentMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string IndentMenuHelpText {
            get {
                return "Indent";
            }
        }

         /// <summary>
        /// Not used.
        /// </summary>
       public static string IndentMenuHelpTitle {
            get {
                return "Indent";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string IndentMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         LastAntMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string LastAntMenuHelpText {
            get {
                return "Run Last NAnt Target...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string LastAntMenuHelpTitle {
            get {
                return "Run Last NAnt Target...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string LastAntMenuHelpTopic {
            get {
                return "AntMenu";
            }
        }

        // *************************************************************************
        //                         LowCaseMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string LowCaseMenuHelpText {
            get {
                return "Low Case                                              Ctrl+K+O";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string LowCaseMenuHelpTitle {
            get {
                return "Low Case                                              Ctrl+K+O";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string LowCaseMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         MultipleAntMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string MultipleAntMenuHelpText {
            get {
                return "Run Multiple NAnt Target...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string MultipleAntMenuHelpTitle {
            get {
                return "Run Multiple NAnt Target...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string MultipleAntMenuHelpTopic {
            get {
                return "AntMenu";
            }
        }

        // *************************************************************************
        //                         NDocMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string NDocMenuHelpText {
            get {
                return "NDoc";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NDocMenuHelpTitle {
            get {
                return "NDoc";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NDocMenuHelpTopic {
            get {
                return "SBTToolMenu";
            }
        }

        // *************************************************************************
        //                         NUnitConsoleMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitConsoleMenuHelpText {
            get {
                return "Run NUnit (Console) on Project Target";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitConsoleMenuHelpTitle {
            get {
                return "Run NUnit (Console) on Project Target";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitConsoleMenuHelpTopic {
            get {
                return "NUnitMenu";
            }
        }

        // *************************************************************************
        //                         NUnitConsoleProjectGroupAssemblyMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitConsoleProjectGroupAssemblyMenuHelpText {
            get {
                return "Run NUnit (Console) on Project Group Assemblies";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitConsoleProjectGroupAssemblyMenuHelpTitle {
            get {
                return "Run NUnit (Console) on Project Group Assemblies";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitConsoleProjectGroupAssemblyMenuHelpTopic {
            get {
                return "NUnitMenu";
            }
        }

        // *************************************************************************
        //                         NUnitGuiMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitGuiMenuHelpText {
            get {
                return "Start NUnit (Gui)...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitGuiMenuHelpTitle {
            get {
                return "Start NUnit (Gui)...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitGuiMenuHelpTopic {
            get {
                return "NUnitMenu";
            }
        }

        // *************************************************************************
        //                         NUnitGuiWithAssemblyMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitGuiWithAssemblyMenuHelpText {
            get {
                return "Run NUnit (Gui) on Project Target...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitGuiWithAssemblyMenuHelpTitle {
            get {
                return "Run NUnit (Gui) on Project Target...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitGuiWithAssemblyMenuHelpTopic {
            get {
                return "NUnitMenu";
            }
        }

        // *************************************************************************
        //                         NUnitMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitMenuHelpText {
            get {
                return "NUnit";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitMenuHelpTitle {
            get {
                return "NUnit";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitMenuHelpTopic {
            get {
                return "SBTToolMenu";
            }
        }

        // *************************************************************************
        //                         NUnitWizardMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitWizardMenuHelpText {
            get {
                return "New NUnit Test Fixture...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitWizardMenuHelpTitle {
            get {
                return "New NUnit Test Fixture...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string NUnitWizardMenuHelpTopic {
            get {
                return "NUnitMenu";
            }
        }

        // *************************************************************************
        //                         OpenExpressoGuiMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenExpressoGuiMenuHelpText {
            get {
                return "Open Expresso";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenExpressoGuiMenuHelpTitle {
            get {
                return "Open Expresso";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenExpressoGuiMenuHelpTopic {
            get {
                return "ExpressoMenu";
            }
        }

        // *************************************************************************
        //                         OpenExpressoProjectMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenExpressoProjectMenuHelpText {
            get {
                return "Open Expresso Project";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenExpressoProjectMenuHelpTitle {
            get {
                return "Open Expresso Project";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenExpressoProjectMenuHelpTopic {
            get {
                return "ExpressoMenu";
            }
        }

        // *************************************************************************
        //                         OpenFolderMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenFolderMenuHelpText {
            get {
                return "Open Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenFolderMenuHelpTitle {
            get {
                return "Open Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenFolderMenuHelpTopic {
            get {
                return "SBTProjectMenu";
            }
        }

        // *************************************************************************
        //                         OpenFxCopGuiMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenFxCopGuiMenuHelpText {
            get {
                return "Open FxCop";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenFxCopGuiMenuHelpTitle {
            get {
                return "Open FxCop";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenFxCopGuiMenuHelpTopic {
            get {
                return "FxCopMenu";
            }
        }

        // *************************************************************************
        //                         OpenFxCopProjectMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenFxCopProjectMenuHelpText {
            get {
                return "Open FxCop Project";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenFxCopProjectMenuHelpTitle {
            get {
                return "Open FxCop Project";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenFxCopProjectMenuHelpTopic {
            get {
                return "FxCopMenu";
            }
        }

        // *************************************************************************
        //                         OpenModuleFolderMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenModuleFolderMenuHelpText {
            get {
                return "Open Module Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenModuleFolderMenuHelpTitle {
            get {
                return "Open Module Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenModuleFolderMenuHelpTopic {
            get {
                return "OpenFolderMenu";
            }
        }

        // *************************************************************************
        //                         OpenNDocGuiMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenNDocGuiMenuHelpText {
            get {
                return "Open NDoc";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenNDocGuiMenuHelpTitle {
            get {
                return "Open NDoc";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenNDocGuiMenuHelpTopic {
            get {
                return "NDocMenu";
            }
        }

        // *************************************************************************
        //                         OpenNDocProjectMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenNDocProjectMenuHelpText {
            get {
                return "Open NDoc Project";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenNDocProjectMenuHelpTitle {
            get {
                return "Open NDoc Project";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenNDocProjectMenuHelpTopic {
            get {
                return "NDocMenu";
            }
        }

        // *************************************************************************
        //                         OpenNUnitProjectMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenNUnitProjectMenuHelpText {
            get {
                return "Open NUnit Project (Gui)...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenNUnitProjectMenuHelpTitle {
            get {
                return "Open NUnit Project (Gui)...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenNUnitProjectMenuHelpTopic {
            get {
                return "NUnitMenu";
            }
        }

        // *************************************************************************
        //                         OpenProjectFolderMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenProjectFolderMenuHelpText {
            get {
                return "Open Project Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenProjectFolderMenuHelpTitle {
            get {
                return "Open Project Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenProjectFolderMenuHelpTopic {
            get {
                return "OpenFolderMenu";
            }
        }

        // *************************************************************************
        //                         OpenProjectGroupFolderMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenProjectGroupFolderMenuHelpText {
            get {
                return "Open Project Group Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenProjectGroupFolderMenuHelpTitle {
            get {
                return "Open Project Group Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenProjectGroupFolderMenuHelpTopic {
            get {
                return "OpenFolderMenu";
            }
        }

        // *************************************************************************
        //                         OpenProjectTargetFolderMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenProjectTargetFolderMenuHelpText {
            get {
                return "Open Project Target Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenProjectTargetFolderMenuHelpTitle {
            get {
                return "Open Project Target Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenProjectTargetFolderMenuHelpTopic {
            get {
                return "OpenFolderMenu";
            }
        }

        // *************************************************************************
        //                         OpenShellDefaultMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellDefaultMenuHelpText {
            get {
                return "Default";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellDefaultMenuHelpTitle {
            get {
                return "Default";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellDefaultMenuHelpTopic {
            get {
                return "ShellMenu";
            }
        }

        // *************************************************************************
        //                         OpenShellModuleMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellModuleMenuHelpText {
            get {
                return "Module Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellModuleMenuHelpTitle {
            get {
                return "Module Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellModuleMenuHelpTopic {
            get {
                return "ShellMenu";
            }
        }

        // *************************************************************************
        //                         OpenShellProjectGroupMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellProjectGroupMenuHelpText {
            get {
                return "Project Group Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellProjectGroupMenuHelpTitle {
            get {
                return "Project Group Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellProjectGroupMenuHelpTopic {
            get {
                return "ShellMenu";
            }
        }

        // *************************************************************************
        //                         OpenShellProjectMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellProjectMenuHelpText {
            get {
                return "Project Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellProjectMenuHelpTitle {
            get {
                return "Project Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellProjectMenuHelpTopic {
            get {
                return "ShellMenu";
            }
        }

        // *************************************************************************
        //                         OpenShellProjectTargetMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellProjectTargetMenuHelpText {
            get {
                return "Project Target Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellProjectTargetMenuHelpTitle {
            get {
                return "Project Target Folder";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenShellProjectTargetMenuHelpTopic {
            get {
                return "ShellMenu";
            }
        }

        // *************************************************************************
        //                         OpenWinMergeMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenWinMergeMenuHelpText {
            get {
                return "Open WinMerge ...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenWinMergeMenuHelpTitle {
            get {
                return "Open WinMerge ...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string OpenWinMergeMenuHelpTopic {
            get {
                return "WinMergeMenu";
            }
        }

        // *************************************************************************
        //                         PasteCommentedMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string PasteCommentedMenuHelpText {
            get {
                return "Paste Commented";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string PasteCommentedMenuHelpTitle {
            get {
                return "Paste Commented";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string PasteCommentedMenuHelpTopic {
            get {
                return "SBTClipboardMenu";
            }
        }

        // *************************************************************************
        //                         PasteKeepSelectedMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string PasteKeepSelectedMenuHelpText {
            get {
                return "Paste Keep Selected";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string PasteKeepSelectedMenuHelpTitle {
            get {
                return "Paste Keep Selected";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string PasteKeepSelectedMenuHelpTopic {
            get {
                return "SBTClipboardMenu";
            }
        }

        // *************************************************************************
        //                         ProjectGroupOptionsMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ProjectGroupOptionsMenuHelpText {
            get {
                return "Project Group Options...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ProjectGroupOptionsMenuHelpTitle {
            get {
                return "Project Group Options...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ProjectGroupOptionsMenuHelpTopic {
            get {
                return "SBTOptionsMenu";
            }
        }

        // *************************************************************************
        //                         SaveFilesMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string SaveFilesMenuHelpText {
            get {
                return "Save Files...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SaveFilesMenuHelpTitle {
            get {
                return "Save Files...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SaveFilesMenuHelpTopic {
            get {
                return "SBTProjectMenu";
            }
        }

        // *************************************************************************
        //                         SharpBuilderToolsOptionsMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string SharpBuilderToolsOptionsMenuHelpText {
            get {
                return "Sharp Builder Tools Options...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SharpBuilderToolsOptionsMenuHelpTitle {
            get {
                return "Sharp Builder Tools Options...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SharpBuilderToolsOptionsMenuHelpTopic {
            get {
                return "SBTOptionsMenu";
            }
        }

        // *************************************************************************
        //                         SharpBuilderToolsOptionsReloadMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string SharpBuilderToolsOptionsReloadMenuHelpText {
            get {
                return "Reload Options";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SharpBuilderToolsOptionsReloadMenuHelpTitle {
            get {
                return "Reload Options";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SharpBuilderToolsOptionsReloadMenuHelpTopic {
            get {
                return "SBTOptionsMenu";
            }
        }

        // *************************************************************************
        //                         SharpBuilderToolsShortcutReloadMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string SharpBuilderToolsShortcutReloadMenuHelpText {
            get {
                return "Refresh Shortcut";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SharpBuilderToolsShortcutReloadMenuHelpTitle {
            get {
                return "Refresh Shortcut";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SharpBuilderToolsShortcutReloadMenuHelpTopic {
            get {
                return "SBTOptionsMenu";
            }
        }

        // *************************************************************************
        //                         SharpHelperViewMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string SharpHelperViewMenuHelpText {
            get {
                return "Clipboard Viewer...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SharpHelperViewMenuHelpTitle {
            get {
                return "Clipboard Viewer...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SharpHelperViewMenuHelpTopic {
            get {
                return "SBTClipboardMenu";
            }
        }

        // *************************************************************************
        //                         ShellMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ShellMenuHelpText {
            get {
                return "Shell";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ShellMenuHelpTitle {
            get {
                return "Shell";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ShellMenuHelpTopic {
            get {
                return "SBTProjectMenu";
            }
        }

        // *************************************************************************
        //                         SingleAntMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string SingleAntMenuHelpText {
            get {
                return "Run NAnt Target...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SingleAntMenuHelpTitle {
            get {
                return "Run NAnt Target...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SingleAntMenuHelpTopic {
            get {
                return "AntMenu";
            }
        }

        // *************************************************************************
        //                         SortMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string SortMenuHelpText {
            get {
                return "Sort...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SortMenuHelpTitle {
            get {
                return "Sort...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SortMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         SortToolBoxMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string SortToolBoxMenuHelpText {
            get {
                return "Sort ToolBox Items";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SortToolBoxMenuHelpTitle {
            get {
                return "Sort ToolBox Items";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SortToolBoxMenuHelpTopic {
            get {
                return "SBTViewMenu";
            }
        }

        // *************************************************************************
        //                         SpaceToTabMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string SpaceToTabMenuHelpText {
            get {
                return "Convert Space to Tab";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SpaceToTabMenuHelpTitle {
            get {
                return "Convert Space to Tab";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SpaceToTabMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         SpellCheckMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string SpellCheckMenuHelpText {
            get {
                return "Spell Check...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SpellCheckMenuHelpTitle {
            get {
                return "Spell Check...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string SpellCheckMenuHelpTopic {
            get {
                return "SBTToolMenu";
            }
        }

        // *************************************************************************
        //                         StartProjectTargetMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string StartProjectTargetMenuHelpText {
            get {
                return "Start Project Target";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string StartProjectTargetMenuHelpTitle {
            get {
                return "Start Project Target";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string StartProjectTargetMenuHelpTopic {
            get {
                return "SBTProjectMenu";
            }
        }

        // *************************************************************************
        //                         TabToSpaceMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string TabToSpaceMenuHelpText {
            get {
                return "Convert Tab to Space";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string TabToSpaceMenuHelpTitle {
            get {
                return "Convert Tab to Space";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string TabToSpaceMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         ToDoMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ToDoMenuHelpText {
            get {
                return "To Do...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ToDoMenuHelpTitle {
            get {
                return "To Do...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ToDoMenuHelpTopic {
            get {
                return "SBTViewMenu";
            }
        }

        // *************************************************************************
        //                         ToggleCaseMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ToggleCaseMenuHelpText {
            get {
                return "Toggle Case";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ToggleCaseMenuHelpTitle {
            get {
                return "Toggle Case";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ToggleCaseMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         ToggleCommentMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ToggleCommentMenuHelpText {
            get {
                return "Toggle Comment";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ToggleCommentMenuHelpTitle {
            get {
                return "Toggle Comment";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ToggleCommentMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         UnCommentMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string UnCommentMenuHelpText {
            get {
                return "UnComment";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string UnCommentMenuHelpTitle {
            get {
                return "UnComment";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string UnCommentMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         UnElideAllBlocksMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string UnElideAllBlocksMenuHelpText {
            get {
                return "Unfold All Blocks                           Ctrl+Shift+K+A";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string UnElideAllBlocksMenuHelpTitle {
            get {
                return "Unfold All Blocks                           Ctrl+Shift+K+A";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string UnElideAllBlocksMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         UnElideNearestBlockMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string UnElideNearestBlockMenuHelpText {
            get {
                return "Unfold Nearest Block                   Ctrl+Shift+K+U";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string UnElideNearestBlockMenuHelpTitle {
            get {
                return "Unfold Nearest Block                   Ctrl+Shift+K+U";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string UnElideNearestBlockMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         UnindentMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string UnindentMenuHelpText {
            get {
                return "Unindent";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string UnindentMenuHelpTitle {
            get {
                return "Unindent";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string UnindentMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         UpCaseCaseMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string UpCaseCaseMenuHelpText {
            get {
                return "Up Case                                                Ctrl+K+N";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string UpCaseCaseMenuHelpTitle {
            get {
                return "Up Case                                                Ctrl+K+N";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string UpCaseCaseMenuHelpTopic {
            get {
                return "SBTEditMenu";
            }
        }

        // *************************************************************************
        //                         ViewAllMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAllMenuHelpText {
            get {
                return "View All Code...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAllMenuHelpTitle {
            get {
                return "View All Code...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAllMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewAntFileMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAntFileMenuHelpText {
            get {
                return "View Ant file...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAntFileMenuHelpTitle {
            get {
                return "View Ant file...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAntFileMenuHelpTopic {
            get {
                return "AntMenu";
            }
        }

        // *************************************************************************
        //                         ViewAntPropertiesMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAntPropertiesMenuHelpText {
            get {
                return "View Ant Properties...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAntPropertiesMenuHelpTitle {
            get {
                return "View Ant Properties...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAntPropertiesMenuHelpTopic {
            get {
                return "AntMenu";
            }
        }

        // *************************************************************************
        //                         ViewAntStructureMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAntStructureMenuHelpText {
            get {
                return "View Last Ant Structure...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAntStructureMenuHelpTitle {
            get {
                return "View Last Ant Structure...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAntStructureMenuHelpTopic {
            get {
                return "AntMenu";
            }
        }

        // *************************************************************************
        //                         ViewAntTargetsMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAntTargetsMenuHelpText {
            get {
                return "View Ant Targets...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAntTargetsMenuHelpTitle {
            get {
                return "View Ant Targets...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAntTargetsMenuHelpTopic {
            get {
                return "AntMenu";
            }
        }

        // *************************************************************************
        //                         ViewAssembliesMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAssembliesMenuHelpText {
            get {
                return "View Assemblies...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAssembliesMenuHelpTitle {
            get {
                return "View Assemblies...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewAssembliesMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewBookmarksMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewBookmarksMenuHelpText {
            get {
                return "View Bookmarks...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewBookmarksMenuHelpTitle {
            get {
                return "View Bookmarks...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewBookmarksMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewClassesMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewClassesMenuHelpText {
            get {
                return "View Classes...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewClassesMenuHelpTitle {
            get {
                return "View Classes...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewClassesMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewComponentListMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewComponentListMenuHelpText {
            get {
                return "View Component List...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewComponentListMenuHelpTitle {
            get {
                return "View Component List...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewComponentListMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewComponentTreeMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewComponentTreeMenuHelpText {
            get {
                return "View Component Tree...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewComponentTreeMenuHelpTitle {
            get {
                return "View Component Tree...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewComponentTreeMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewEventsMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewEventsMenuHelpText {
            get {
                return "View Events...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewEventsMenuHelpTitle {
            get {
                return "View Events...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewEventsMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewFieldsPropertiesMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewFieldsPropertiesMenuHelpText {
            get {
                return "View Fields / Properties...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewFieldsPropertiesMenuHelpTitle {
            get {
                return "View Fields / Properties...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewFieldsPropertiesMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewFilesMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewFilesMenuHelpText {
            get {
                return "View Files...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewFilesMenuHelpTitle {
            get {
                return "View Files...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewFilesMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewFormsMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewFormsMenuHelpText {
            get {
                return "View Forms...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewFormsMenuHelpTitle {
            get {
                return "View Forms...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewFormsMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewMethodsMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewMethodsMenuHelpText {
            get {
                return "View Methods...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewMethodsMenuHelpTitle {
            get {
                return "View Methods...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewMethodsMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewModulesMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewModulesMenuHelpText {
            get {
                return "View All Files...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewModulesMenuHelpTitle {
            get {
                return "View All Files...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewModulesMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewNavigateComponentMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewNavigateComponentMenuHelpText {
            get {
                return "View Components...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewNavigateComponentMenuHelpTitle {
            get {
                return "View Components...";
            }
        }

		/// <summary>
        /// Not used.
        /// </summary>
        public static string ViewNavigateComponentMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewOpenTabMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewOpenTabMenuHelpText {
            get {
                return "View Open Tabs...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewOpenTabMenuHelpTitle {
            get {
                return "View Open Tabs...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewOpenTabMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewSourceInfoMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewSourceInfoMenuHelpText {
            get {
                return "View Source Info...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewSourceInfoMenuHelpTitle {
            get {
                return "View Source Info...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewSourceInfoMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewSourceMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewSourceMenuHelpText {
            get {
                return "View Source Files...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewSourceMenuHelpTitle {
            get {
                return "View Source Files...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewSourceMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         ViewTypesMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewTypesMenuHelpText {
            get {
                return "View Types...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewTypesMenuHelpTitle {
            get {
                return "View Types...";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string ViewTypesMenuHelpTopic {
            get {
                return "SBTNavigateMenu";
            }
        }

        // *************************************************************************
        //                         WinMergeMenu
        // *************************************************************************
        /// <summary>
        /// Not used.
        /// </summary>
        public static string WinMergeMenuHelpText {
            get {
                return "WinMerge";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string WinMergeMenuHelpTitle {
            get {
                return "WinMerge";
            }
        }

        /// <summary>
        /// Not used.
        /// </summary>
        public static string WinMergeMenuHelpTopic {
            get {
                return "SBTToolMenu";
            }
		} */
	}

}
