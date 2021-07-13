' このファイルは、このプロジェクトに適用される SuppressMessage 
'属性を保持するために、コード分析によって使用されます。
' プロジェクト レベルの抑制には、ターゲットがないものと、特定のターゲット
'が指定され、名前空間、型、メンバーなどをスコープとするものがあります。
'
' このファイルに抑制を追加するには、[エラー一覧] でメッセージを
'右クリックし、[メッセージの非表示] をポイントして、
'[プロジェクト抑制ファイル内] をクリックします。
' このファイルに手動で抑制を追加する必要はありません。

'0588 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope:="type", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic")> 
'0238 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#UpdateActivityData(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable)")> 
'0241 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#UpdateActivityData(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable)")> 
'0240 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1809:AvoidExcessiveLocals", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#InsertActivityData(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable)")> 
'0237 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#InsertActivityData(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable)")> 
'0234 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#UpdateActivityData(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable)")> 
'0233 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#InsertActivityData(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable)")> 
'0232 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#UpdateActivityData(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable)")> 
'0231 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#IsInputeCheck(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable,System.Int32&)")> 
'0229 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#InsertActivityData(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable)")> 
'0321 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#SetToDo(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable,System.String)")> 
'0005 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="get", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#getFllwupDoneCategory(System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="get", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#getFllwupBoxType(System.String,System.Nullable`1<System.Int64>,System.String,System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="i")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="Fllw", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetFllwSeries(System.String,System.Int64)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="Fllw", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetFllwModel(System.String,System.Int64)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="Fllw", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetFllwColor(System.String,System.Int64)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e", Scope:="namespace", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CROP")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB", Scope:="namespace", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB")> 
'0318 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#IsInputeCheck(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable,System.Int32&)")> 
'0014 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetNoCompetitionMakermaster()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetUsers()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetNextActContact()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetFollowContact()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetDateFormat()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetCompetitorMaster()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetCompetitionMakermaster()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetAlertSel()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetAlertNonSel()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetActContact()")> 
'0004 TCS Kawara
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic")> 

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="Dt", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#IsInputeCheck(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable,Toyota.eCRB.CommonUtility.DataAccess.ActivityInfoDataSet+GetNewCustomerDataTable,System.String,System.Int32&)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="3#", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#IsInputeCheck(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable,Toyota.eCRB.CommonUtility.DataAccess.ActivityInfoDataSet+GetNewCustomerDataTable,System.String,System.Int32&)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1505:AvoidUnmaintainableCode", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#IsInputeCheck(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable,Toyota.eCRB.CommonUtility.DataAccess.ActivityInfoDataSet+GetNewCustomerDataTable,System.String,System.Int32&)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#IsInputeCheck(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080203DataSet+SC3080203RegistDataDataTable,Toyota.eCRB.CommonUtility.DataAccess.ActivityInfoDataSet+GetNewCustomerDataTable,System.String,System.Int32&)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080203BusinessLogic.#GetStaffStatus()")> 