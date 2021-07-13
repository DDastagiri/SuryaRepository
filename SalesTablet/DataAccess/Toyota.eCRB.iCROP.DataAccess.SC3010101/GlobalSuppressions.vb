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

'0004 TCS Watanabe
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSetTableAdapters")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="Toyota.eCRB.Common.Login.DataAccess")> 

'0011 TCS Watanabe
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="i")> 

'0010 TCS Watanabe
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e", Scope:="namespace", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSetTableAdapters")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e", Scope:="namespace", Target:="Toyota.eCRB.Common.Login.DataAccess")> 

'0008 TCS Watanabe
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB", Scope:="namespace", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSetTableAdapters")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB", Scope:="namespace", Target:="Toyota.eCRB.Common.Login.DataAccess")> 

'0009 TCS Watanabe
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CROP")> 

'SC3010101DataSet.Designer.vb 内の自動生成
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet.#.ctor()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1003:UseGenericEventHandlerInstances", Scope:="type", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableRowChangeEventHandler")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope:="type", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableRowChangeEventHandler")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible", Scope:="type", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableRowChangeEvent")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableRow.#Macaddress")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1065:DoNotRaiseExceptionsInUnexpectedLocations", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableRow.#Dlrcd")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableDataTable.#OnRowDeleting(System.Data.DataRowChangeEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableDataTable.#OnRowDeleted(System.Data.DataRowChangeEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableDataTable.#OnRowChanging(System.Data.DataRowChangeEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableDataTable.#OnRowChanged(System.Data.DataRowChangeEventArgs)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableDataTable.#GetTypedTableSchema(System.Xml.Schema.XmlSchemaSet)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableDataTable.#GetTypedTableSchema(System.Xml.Schema.XmlSchemaSet)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="Macaddress", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableDataTable.#AddSC3010101MacDataTableRow(System.String,System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="Dlrcd", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableDataTable.#AddSC3010101MacDataTableRow(System.String,System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableDataTable.#.ctor()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope:="type", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet+SC3010101MacDataTableDataTable")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet.#ReadXmlSerializable(System.Xml.XmlReader)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet.#GetTypedDataSetSchema(System.Xml.Schema.XmlSchemaSet)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet.#GetTypedDataSetSchema(System.Xml.Schema.XmlSchemaSet)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2236:CallBaseClassMethodsOnISerializableTypes", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet.#.ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2240:ImplementISerializableCorrectly", Scope:="type", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:パブリック メソッドの引数の検証", MessageId:="0", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet.#.ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet.#.ctor(System.Runtime.Serialization.SerializationInfo,System.Runtime.Serialization.StreamingContext)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.Common.Login.DataAccess.SC3010101DataSet.#GetSchemaSerializable()")> 
