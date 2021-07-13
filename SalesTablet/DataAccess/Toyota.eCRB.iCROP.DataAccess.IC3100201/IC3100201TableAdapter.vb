'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3100201TableAdapter.vb
'──────────────────────────────────
'機能： セールス来店実績更新
'補足： 
'作成： 2011/12/12 KN k.nagasawa
'更新： 2012/08/27 TMEJ m.okamura 新車受付機能改善 $01
'──────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' 未対応来店客件数取得用I/FのTableAdapterクラス
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class IC3100201TableAdapter

#Region "定数"
    ' 来店実績ステータス：フリー(ブロードキャスト)
    Private Const VISITSTATUS_FREE_BROADCAST As String = "02"
    ' 来店実績ステータス：調整中
    Private Const VISITSTATUS_ADJUST As String = "03"
    ' 来店実績ステータス：確定(ブロードキャスト)
    Private Const VISITSTATUS_DECISION_BROADCAST As String = "04"
    ' 来店実績ステータス：確定
    Private Const VISITSTATUS_DECISION As String = "05"
    ' 来店実績ステータス：待ち
    Private Const VISITSTATUS_WAIT As String = "06"
    ' $01 start 複数顧客に対する商談平行対応
    ' 来店実績ステータス：待ち
    Private Const VisitStatusNegotiateStop As String = "09"
    ' $01 end   複数顧客に対する商談平行対応

    ' スタッフと紐付け済み
    Private Const DISPCLASS_STAFF As String = "01"
    ' ブロードキャスト中
    Private Const DISPCLASS_BROADCAST As String = "02"

    ' 削除フラグ：削除以外
    Private Const NOTICE_DELFLG As String = "0"
    ' 削除フラグ：削除以外
    Private Const US_DELFLG As String = "0"
    ' 削除フラグ：削除以外
    Private Const UA_DELFLG As String = "0"
#End Region

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' 未対応来店客数の取得処理
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="account">スタッフコード</param>
    ''' <param name="nowDate">現在日付</param>
    ''' <returns>未対応来店客件数が格納されたデータテーブル</returns>
    ''' <remarks></remarks>
    Public Shared Function GetNotDealCount(ByVal dlrcd As String, _
                                           ByVal strcd As String, _
                                           ByVal account As String, _
                                           ByVal nowDate As Date) As IC3100201DataSetDataSet.IC3100201NotDealCountDataTable
        ' メソッド名を取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        ' 開始ログの出力
        Dim startLog As New StringBuilder()
        startLog.Append(methodName & "_Start Param[")
        startLog.Append("dlrcd=" & dlrcd & ", ")
        startLog.Append("strcd=" & strcd & ", ")
        startLog.Append("account=" & account & ", ")
        startLog.Append("nowDate=" & nowDate)
        Logger.Info(startLog.ToString())

        ' 戻り値
        Dim dtRet As IC3100201DataSetDataSet.IC3100201NotDealCountDataTable = Nothing

        Using query As New DBSelectQuery(Of IC3100201DataSetDataSet.IC3100201NotDealCountDataTable)("IC3010201_001")

            Dim sql As New StringBuilder

            ' 取得開始日時と取得終了日時の作成
            Dim dtStart As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 0, 0, 0)
            Dim dtEnd As New Date(nowDate.Year, nowDate.Month, nowDate.Day, 23, 59, 59)

            ' SQL文作成
            With sql
                .Append(" SELECT /* IC3100201_001 */")
                .Append("        SUM(T1.COUNT_NOT_DEAL_VISIT) AS COUNT_NOT_DEAL_VISIT")
                .Append("   FROM (")
                .Append("     SELECT COUNT(1) AS COUNT_NOT_DEAL_VISIT")
                .Append("          , :DISPCLASS_BROADCAST AS DISPCLASS")
                .Append("       FROM TBL_VISIT_SALES VS1")
                .Append("          , TBL_VISITDEAL_NOTICE NOTICE1")
                .Append("          , TBL_USERS US1")
                .Append("          , TBL_USERS UA1")
                .Append("      WHERE VS1.VISITSEQ = NOTICE1.VISITSEQ")
                .Append("        AND VS1.STAFFCD = US1.ACCOUNT(+)")
                .Append("        AND VS1.ACCOUNT = UA1.ACCOUNT(+)")
                .Append("        AND VS1.DLRCD = :DLRCD")
                .Append("        AND VS1.STRCD = :STRCD")
                .Append("        AND VS1.VISITTIMESTAMP >= :VISITTIMESTAMP_START")
                .Append("        AND VS1.VISITTIMESTAMP <= :VISITTIMESTAMP_END")
                .Append("        AND VS1.VISITSTATUS = :VISITSTATUS_FREE_BROADCAST")
                .Append("        AND NOTICE1.ACCOUNT = :ACCOUNT")
                .Append("        AND NOTICE1.DELFLG = :NOTICE_DELFLG")
                .Append("        AND US1.DELFLG(+) = :US_DELFLG")
                .Append("        AND UA1.DELFLG(+) = :UA_DELFLG")
                .Append("      UNION ALL")
                .Append("     SELECT COUNT(1) AS COUNT_NOT_DEAL_VISIT")
                .Append("          , :DISPCLASS_STAFF AS DISPCLASS")
                .Append("       FROM TBL_VISIT_SALES VS2")
                .Append("          , TBL_USERS US2")
                .Append("          , TBL_USERS UA2")
                .Append("      WHERE VS2.STAFFCD = US2.ACCOUNT(+)")
                .Append("        AND VS2.ACCOUNT = UA2.ACCOUNT(+)")
                .Append("        AND VS2.DLRCD = :DLRCD")
                .Append("        AND VS2.STRCD = :STRCD")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("        AND NVL(VS2.STOPTIME, VS2.VISITTIMESTAMP) BETWEEN :VISITTIMESTAMP_START")
                .Append("                                                      AND :VISITTIMESTAMP_END")
                ' $01 end   複数顧客に対する商談平行対応
                .Append("        AND VS2.ACCOUNT = :ACCOUNT")
                .Append("        AND VS2.VISITSTATUS IN (:VISITSTATUS_ADJUST")
                .Append("                              , :VISITSTATUS_DECISION_BROADCAST")
                .Append("                              , :VISITSTATUS_DECISION")
                ' $01 start 複数顧客に対する商談平行対応
                .Append("                              , :VISITSTATUS_WAIT")
                .Append("                              , :VISITSTATUS_SALES_STOP)")
                ' $01 end   複数顧客に対する商談平行対応
                .Append("        AND US2.DELFLG(+) = :US_DELFLG")
                .Append("        AND UA2.DELFLG(+) = :UA_DELFLG")
                .Append("      ) T1")
            End With
            query.CommandText = sql.ToString()

            ' バインド変数設定
            With query
                ' 変動値
                .AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                .AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
                .AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                .AddParameterWithTypeValue("VISITTIMESTAMP_START", OracleDbType.Date, dtStart)
                .AddParameterWithTypeValue("VISITTIMESTAMP_END", OracleDbType.Date, dtEnd)
                ' 固定値
                .AddParameterWithTypeValue("VISITSTATUS_FREE_BROADCAST", OracleDbType.Char, VISITSTATUS_FREE_BROADCAST)
                .AddParameterWithTypeValue("NOTICE_DELFLG", OracleDbType.Char, NOTICE_DELFLG)
                .AddParameterWithTypeValue("US_DELFLG", OracleDbType.Char, US_DELFLG)
                .AddParameterWithTypeValue("UA_DELFLG", OracleDbType.Char, UA_DELFLG)
                .AddParameterWithTypeValue("VISITSTATUS_ADJUST", OracleDbType.Char, VISITSTATUS_ADJUST)
                .AddParameterWithTypeValue("VISITSTATUS_DECISION_BROADCAST", OracleDbType.Char, VISITSTATUS_DECISION_BROADCAST)
                .AddParameterWithTypeValue("VISITSTATUS_DECISION", OracleDbType.Char, VISITSTATUS_DECISION)
                .AddParameterWithTypeValue("VISITSTATUS_WAIT", OracleDbType.Char, VISITSTATUS_WAIT)
                ' $01 start 複数顧客に対する商談平行対応
                .AddParameterWithTypeValue("VISITSTATUS_SALES_STOP", OracleDbType.Char, VisitStatusNegotiateStop)
                ' $01 end   複数顧客に対する商談平行対応
                .AddParameterWithTypeValue("DISPCLASS_STAFF", OracleDbType.Char, DISPCLASS_STAFF)
                .AddParameterWithTypeValue("DISPCLASS_BROADCAST", OracleDbType.Char, DISPCLASS_BROADCAST)
            End With

            dtRet = query.GetData()

            ' 終了ログの出力
            Logger.Info(methodName & "_End RowCount=" & dtRet.Rows.Count)

        End Using

        ' 検索結果返却
        Return dtRet

    End Function


End Class