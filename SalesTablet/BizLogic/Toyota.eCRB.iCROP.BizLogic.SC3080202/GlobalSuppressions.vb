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

'0386～0387
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope:="type", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#GetActivityList(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080202DataSet+SC3080202GetActivityListFromDataTable)")> 
'0349～350,0380～0385
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#UpdateSelectedSeries(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080202DataSet+SC3080202UpdateSelectedSeriesFromDataTable,System.Int32&)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#UpdateSelectedCompe(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080202DataSet+SC3080202UpdateSelectedCompeFromDataTable)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#UpdateSalesMemo(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080202DataSet+SC3080202UpdateSalesMemoFromDataTable)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#UpdateSalesCondition(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080202DataSet+SC3080202UpdateSalesConditionFromDataTable)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#GetSalesCondition(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080202DataSet+SC3080202GetSalesConditionFromDataTable)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#GetActivityList(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080202DataSet+SC3080202GetActivityListFromDataTable)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#GetActivityDetail(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080202DataSet+SC3080202GetActivityDetailFromDataTable)")> 
'0347
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId:="1#", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#UpdateSelectedSeries(Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080202DataSet+SC3080202UpdateSelectedSeriesFromDataTable,System.Int32&)")> 
'0004
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic")>  
'0043
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#GetProcessIcons()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#GetSelectedCompeModelMaster()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#GetSelectedCompeMakerMaster()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#GetFllwupboxSeqno()")> 
'0005
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="Cd", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#ActionCdQuotation")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e", Scope:="namespace", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB", Scope:="namespace", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="Cd", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#ActionCdTestdrive")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="Cd", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#ActionCdEvaluation")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="Cd", Scope:="member", Target:="Toyota.eCRB.CustomerInfo.Details.BizLogic.SC3080202BusinessLogic.#ActionCdCatalog")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CROP")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="i")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB")> 
