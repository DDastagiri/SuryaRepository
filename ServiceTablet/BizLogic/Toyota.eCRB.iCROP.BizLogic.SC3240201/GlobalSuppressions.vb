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

'1625   TMEJ shimomura
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SMB.ChipDetail.BizLogic.SC3240201BusinessLogic.#SendPushAndNoticeDisplay(Toyota.eCRB.SMB.ChipDetail.DataAccess.CallBackArgumentClass,System.DateTime,Toyota.eCRB.SystemFrameworks.Web.StaffContext)")> 

'1560-1562   TMEJ shimomura
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SMB.ChipDetail.BizLogic.SC3240201BusinessLogic.#UpdateDataUsingWebService(Toyota.eCRB.SMB.ChipDetail.DataAccess.CallBackArgumentClass,System.DateTime,System.DateTime,System.DateTime)")> 

'1483   TMEJ iwaki
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SMB.ChipDetail.BizLogic.SC3240201BusinessLogic.#UpdateData(Toyota.eCRB.SMB.ChipDetail.DataAccess.CallBackArgumentClass,System.DateTime,System.DateTime,System.DateTime,System.DateTime,System.Int64)")> 

'1482   TMEJ iwaki
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Scope:="type", Target:="Toyota.eCRB.SMB.ChipDetail.BizLogic.SC3240201BusinessLogic")> 

'1481   TMEJ iwaki
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="SMB", Scope:="namespace", Target:="Toyota.eCRB.SMB.ChipDetail.BizLogic")> 

'0016   TMEJ iwaki
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope:="type", Target:="Toyota.eCRB.SMB.ChipDetail.BizLogic.SC3240201BusinessLogic+ChipDetailDateTimeClass")> 

'0011   TMEJ iwaki
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="i")> 

'0009   TMEJ iwaki
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CROP")> 

'0005,0006   TMEJ iwaki
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e", Scope:="namespace", Target:="Toyota.eCRB.SMB.ChipDetail.BizLogic")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB", Scope:="namespace", Target:="Toyota.eCRB.SMB.ChipDetail.BizLogic")> 

'0004   TMEJ iwaki<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="Toyota.eCRB.SMB.ChipDetail.BizLogic")> 

