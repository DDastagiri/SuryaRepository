'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'ISC3100104BusinessLogic.vb
'──────────────────────────────────
'機能： お客様チップ作成
'補足： 
'作成： 2013/09/04 TMEJ m.asano
'──────────────────────────────────

Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSet
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSetTableAdapters
''' <summary>
''' SC3100104
''' お客様チップ作成作成を行う画面のビジネスロジック用インターフェース
''' コミット行うメソッドを定義します。
''' </summary>
''' <remarks></remarks>
Public Interface ISC3100104BusinessLogic

    ''' <summary>
    ''' お客様チップ作成
    ''' </summary>
    ''' <param name="insertRow">セールス来店実績データロウ</param>
    ''' <param name="isComplaint">苦情有無フラグ</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Function CreateCustomerChip(ByVal insertRow As VisitReceptionVisitSalesRow, ByVal isComplaint As Boolean) As Integer

End Interface
