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

'元もとのコードなので一旦除外
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.Estimate.Quotation.BizLogic.SC3070205BusinessLogic.#GetUcarAssessmentInfo(System.String,System.String,System.Decimal,System.Int64)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="strCD", Scope:="member", Target:="Toyota.eCRB.Estimate.Quotation.BizLogic.SC3070205BusinessLogic.#GetUcarAssessmentInfo(System.String,System.String,System.Decimal,System.Int64)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="dlrCD", Scope:="member", Target:="Toyota.eCRB.Estimate.Quotation.BizLogic.SC3070205BusinessLogic.#GetUcarAssessmentInfo(System.String,System.String,System.Decimal,System.Int64)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.Estimate.Quotation.BizLogic.SC3070205BusinessLogic.#GetMemoMax()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="strcd", Scope:="member", Target:="Toyota.eCRB.Estimate.Quotation.BizLogic.SC3070205BusinessLogic.#GetEstimateId(System.String,System.String,System.Decimal)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="dlrcd", Scope:="member", Target:="Toyota.eCRB.Estimate.Quotation.BizLogic.SC3070205BusinessLogic.#GetEstimateId(System.String,System.String,System.Decimal)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId:="dlrCD", Scope:="member", Target:="Toyota.eCRB.Estimate.Quotation.BizLogic.SC3070205BusinessLogic.#GetCustNametitle(System.String,System.String,System.String)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.iCROP.BizLogic.SC3070205.SC3070205BusinessLogic.#GetUcarAssessmentInfo(System.String,System.String,System.Decimal,System.Int64)")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.iCROP.BizLogic.SC3070205.SC3070205BusinessLogic.#GetMemoMax()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Scope:="member", Target:="Toyota.eCRB.iCROP.BizLogic.SC3070205.SC3070205BusinessLogic.#GetEstimateVehicleTaxRatio()")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e", Scope:="namespace", Target:="Toyota.eCRB.iCROP.BizLogic.SC3070205")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CROP", Scope:="namespace", Target:="Toyota.eCRB.iCROP.BizLogic.SC3070205")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB", Scope:="namespace", Target:="Toyota.eCRB.iCROP.BizLogic.SC3070205")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="i", Scope:="namespace", Target:="Toyota.eCRB.iCROP.BizLogic.SC3070205")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope:="namespace", Target:="Toyota.eCRB.iCROP.BizLogic.SC3070205")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:スコープを失う前にオブジェクトを破棄", Scope:="member", Target:="Toyota.eCRB.iCROP.BizLogic.SC3070205.SC3070205BusinessLogic.#GetUcarAssessmentInfo(System.String,System.String,System.Int64,System.Int64)")> 

<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB", Scope:="namespace", Target:="Toyota.eCRB.Estimate.Quotation.BizLogic")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e", Scope:="namespace", Target:="Toyota.eCRB.Estimate.Quotation.BizLogic")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CRB")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="CROP")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="e")> 
<Assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId:="i")> 

