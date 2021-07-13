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

'0916 KN Nakamura
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope:="member", Target:="Toyota.eCRB.Visit.Api.BizLogic.VisitUtility.#SendPushPrivate(System.String,System.String)")> 

'0915 KN akiyama
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.Visit.Api.BizLogic.VisitUtility.#GetOnlineUsers(Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.UsersDataSet+USERSDataTable)")> 

'0916 KN akiyama
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope:="member", Target:="Toyota.eCRB.Visit.Api.BizLogic.VisitUtility.#SendPush(System.String)")> 

'0005 KN akiyama
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="i")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CROP")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e", Scope:="namespace", Target:="Toyota.eCRB.Visit.Api.BizLogic")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB", Scope:="namespace", Target:="Toyota.eCRB.Visit.Api.BizLogic")> 

'0004 KN akiyama
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="Toyota.eCRB.Visit.Api.BizLogic")> 

