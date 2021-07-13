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

'0294 TMEJ shimomura
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope:="type", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSet+SC3240501SvcClassCDDataTable")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope:="type", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSet+SC3240501SvcClassListDataTable")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope:="type", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSet+SC3240501NameTitleListDataTable")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope:="type", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSet+SC3240501MercListDataTable")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope:="type", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSet+SC3240501CustomerListDataTable")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope:="type", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSet+SC3240501CustomerCountDataTable")> 

'0293 TMEJ shimomura
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope:="type", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSet")> 

'0011 TMEJ shimomura
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="i")> 

'0009 TMEJ shimomura
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CROP")> 

'0006 TMEJ shimomura
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e", Scope:="namespace", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e", Scope:="namespace", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSetTableAdapters")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e")> 

'0005 TMEJ shimomura
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB", Scope:="namespace", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB", Scope:="namespace", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSetTableAdapters")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB")> 
'0004 TMEJ shimomura<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSetTableAdapters")> 

'1556-1562 TMEJ shimomura
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="SMB", Scope:="namespace", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="SMB", Scope:="namespace", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSetTableAdapters")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSetTableAdapters.SC3240501DataTableAdapter.#ConvertTimeToDateString(System.String,System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSetTableAdapters.SC3240501DataTableAdapter.#GetCustomerList(System.String,System.String,System.String,System.String,System.String,System.String,System.Int64,System.Int64)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSetTableAdapters.SC3240501DataTableAdapter.#GetCustomerCount(System.String,System.String,System.String,System.String,System.String,System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSetTableAdapters.SC3240501DataTableAdapter.#GetSvcNameTitle()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope:="member", Target:="Toyota.eCRB.SMB.ReservationManagement.DataAccess.SC3240501DataSetTableAdapters.SC3240501DataTableAdapter.#OutputErrLog(System.String,System.Exception,System.String,System.Object[])")> 

