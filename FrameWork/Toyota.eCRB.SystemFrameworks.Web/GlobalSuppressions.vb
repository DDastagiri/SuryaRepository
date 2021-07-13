'
'
' このファイルは、このプロジェクトに適用される SuppressMessage 
'属性を保持するために、コード分析によって使用されます。
' プロジェクト レベルの抑制には、ターゲットがないものと、特定のターゲット
'が指定され、名前空間、型、メンバーなどをスコープとするものがあります。
'
' このファイルに抑制を追加するには、[エラー一覧] でメッセージを
'右クリックし、[メッセージの非表示] をポイントして、
'[プロジェクト抑制ファイル内] をクリックします。
' このファイルに手動で抑制を追加する必要はありません。

'1262
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="0#", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.AuthenticationManager.#Auth(System.String&,System.String,System.String)")> 
'1263
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.AuthenticationManager.#Auth(System.String&,System.String,System.String)")> 
'1280-1281
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#_LoadHooks()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#_SetConnectionStringsItem(Toyota.eCRB.SystemFrameworks.Core.SystemConfiguration,Toyota.eCRB.SystemFrameworks.Core.SystemConfigurationType,System.String)")> 
'1277-'1279
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BasePage.#CommonMaster")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#BaseHttpApplication_Error(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.AuthenticationManager.#Auth(System.String&,System.String,System.String)")> 
'1265-1275
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.PopOverForm.#OnPreRender(System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.PopOver.#OnPreRender(System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.NumericBox.#OnPreRender(System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomTextBox.#OnPreRender(System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomRepeater.#OnPreRender(System.EventArgs)")> 
'1264
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.StaffContext.#_presenceUpdateDate")> 
'0025
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.WebResource.#GetUrl(System.String)")> 
'0097
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterHeaderButton.#OnClientClick")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView.#RaiseCallbackEvent(System.String)")> 
'0056
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage.#_GetCurrentSessionInfo()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage.#_GetConfigValue(Toyota.eCRB.SystemFrameworks.Configuration.ClassSection)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#_SetConnectionStringsItem(Toyota.eCRB.SystemFrameworks.Core.SystemConfiguration,Toyota.eCRB.SystemFrameworks.Core.SystemConfigurationType,System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#_RedirectErrorPage(System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#_LoadHooks()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#_CheckApplicationIsReady()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.AuthenticationManager.#Auth(System.String&,System.String,System.String)")> 
'0022
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.AuthenticationManager.#Auth(System.String&,System.String,System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.PopOverForm.#RenderContents(System.Web.UI.HtmlTextWriter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="1", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.PopOverForm.#LoadPostData(System.String,System.Collections.Specialized.NameValueCollection)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.PopOverForm.#AddAttributesToRender(System.Web.UI.HtmlTextWriter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.PopOver.#RenderContents(System.Web.UI.HtmlTextWriter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.PopOver.#AddAttributesToRender(System.Web.UI.HtmlTextWriter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="1", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.NumericBox.#LoadPostData(System.String,System.Collections.Specialized.NameValueCollection)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.NumericBox.#AddAttributesToRender(System.Web.UI.HtmlTextWriter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.MultiItemSelector.#Render(System.Web.UI.HtmlTextWriter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="1", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.MultiItemSelector.#LoadPostData(System.String,System.Collections.Specialized.NameValueCollection)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.MultiItemSelector.#AddAttributesToRender(System.Web.UI.HtmlTextWriter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.JavaScriptUtility.#RegisterStartupScript(System.Web.UI.Page,System.String,System.String,System.Boolean)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.ItemSelector.#Render(System.Web.UI.HtmlTextWriter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="1", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.ItemSelector.#LoadPostData(System.String,System.Collections.Specialized.NameValueCollection)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.ItemSelector.#AddAttributesToRender(System.Web.UI.HtmlTextWriter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.HistorySiteMapProvider.#GetParentNode(System.Web.SiteMapNode)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.HistorySiteMapProvider.#GetChildNodes(System.Web.SiteMapNode)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="1", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.DateTimeSelector.#LoadPostData(System.String,System.Collections.Specialized.NameValueCollection)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.DateTimeSelector.#AddAttributesToRender(System.Web.UI.HtmlTextWriter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomTextBox.#AddAttributesToRender(System.Web.UI.HtmlTextWriter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomRepeater.#AddAttributesToRender(System.Web.UI.HtmlTextWriter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomButton.#AddAttributesToRender(System.Web.UI.HtmlTextWriter)")> 
'0025
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomButton.#IconUrl")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CheckMark.#OnIconUrl")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CheckMark.#OffIconUrl")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CheckButton.#OnIconUrl")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1056:UriPropertiesShouldNotBeStrings", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CheckButton.#OffIconUrl")> 
'0073
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.WebResource.#GetUrl(System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterContextMenu.#menuItem_Click(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#Application_Start(System.Object,System.EventArgs)")> 
'0019
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#_LoadHooks()")> 
'0019
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.JavaScriptUtility.#RegisterStartupScript(System.Web.UI.Page,System.String,System.String,System.Boolean)")> 
'0023
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView.#Columns")> 
'0027
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="ID", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterContextMenuItem.#ID")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="ID", Scope:="type", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterContextMenuBuiltinMenuID")> 

'0018
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage.#Logout")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="type", Target:="Toyota.eCRB.SystemFrameworks.Web.ILoginHook")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterContextMenuBuiltinMenuID.#LogoutItem")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.ILoginHook.#HookAfterLogin()")> 

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage2.#LogoutButton_Click(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.HeaderButton.#Logout")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage2.#Logout")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage1.#LogoutButton_Click(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage1.#Logout")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="TCV", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterFooterButtonClickEventArgs.#TCVFunction")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="SMB", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.FooterMenuCategory.#SMB")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CheckMark.#OnPreRender(System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.StaffContext.#IsCreated")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.WebWordUtility+WordResourceManager.#LoadWord()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView.#DataKeyFieldNames")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.HistorySiteMapProvider.#Clear()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView+CallBackResultData.#TotalCount")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView+CallBackResultData.#RequestPageIndex")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView+CallBackResultData.#PageIndexTo")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView+CallBackResultData.#PageIndexFrom")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#Session_Start(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#Session_End(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#CheckCustomGridViewCallBack()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#Application_Start(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#Application_Error(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#Application_End(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#Application_BeginRequest(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#Application_AuthenticateRequest(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#_SetSystemConfiguration()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="type", Target:="Toyota.eCRB.SystemFrameworks.Web.LoginResult")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.LoginResult.#LoginTimeError")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Login", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.LoginResult.#LoginError")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId:="Logout", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage.#LogoutButton_Click(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="TCV", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.FooterMenuCategory.#TCV")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="GHD", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.LoginResult.#GHDExistError")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="GHD", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.LoginResult.#GHDEditComplete")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="DLRCD", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.LoginResult.#NotExistDLRCDError")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:ローカライズされるパラメーターとしてリテラルを渡さない", MessageId:="System.Web.UI.WebControls.TableCell.set_Text(System.String)", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView.#CreateHeaderRow()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BasePage.#DeclareCommonMasterFooter(Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage,Toyota.eCRB.SystemFrameworks.Web.FooterMenuCategory&)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView.#RaiseCallbackEvent(System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BaseHttpApplication.#BaseHttpApplication_Error(System.Object,System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.BasePage.#GetFormState()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.CommonMasterPage.#_AddFooterButton(System.Int32)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView.#CreateDataTemplate()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView.#CreateHeaderRow()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView.#CreatePagingRow(System.String,System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView.#CreateTable()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView.#CreateWrapPnael()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView.#RenderContents(System.Web.UI.HtmlTextWriter)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.ItemSelector.#OnPreRender(System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.MultiItemSelector.#OnPreRender(System.EventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Scope:="member", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridViewSelectedRowEventArgs.#DataKeys")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e", Scope:="namespace", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e", Scope:="namespace", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.Design")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e", Scope:="namespace", Target:="Toyota.eCRB.SystemFrameworks.Web")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB", Scope:="namespace", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls.Design")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB", Scope:="namespace", Target:="Toyota.eCRB.SystemFrameworks.Web.Controls")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB", Scope:="namespace", Target:="Toyota.eCRB.SystemFrameworks.Web")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB")> 

