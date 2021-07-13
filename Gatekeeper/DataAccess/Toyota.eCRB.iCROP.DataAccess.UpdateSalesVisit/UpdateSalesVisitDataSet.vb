'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'UpdateSalesVisitDataSet.vb
'──────────────────────────────────
'機能： セールス来店実績更新
'補足： 
'作成： 2011/12/12 KN k.nagasawa
'更新： 2012/08/23 TMEJ m.okamura 新車受付機能改善 $01
'更新： 2013/01/23 TMEJ m.asano 新車タブレットショールーム管理機能開発 $02
'更新： 2013/02/25 TMEJ t.shimamura 新車タブレット受付画面管理指標の変更対応 $03
'──────────────────────────────────

Option Strict On
Option Explicit On

Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace UpdateSalesVisitDataSetTableAdapters

    ''' <summary>
    ''' セールス来店実績更新テーブルアダプター
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UpdateSalesVisitTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

#Region "デバッグ用"

#Region "メソッド名"

        ''' <summary>
        ''' メソッド名（商談開始時の来店実績情報の更新）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MethodNameUpdateVisitSalesStart As String = "UpdateSalesVisitTableAdapter.UpdateVisitSalesStart"

        ''' <summary>
        ''' メソッド名（商談開始時の来店実績の取得）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MethodNameGetVisitSalesStart As String = "UpdateSalesVisitTableAdapter.GetVisitSalesStart"

        ''' <summary>
        ''' メソッド名（来店実績シーケンス取得）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MethodNameGetVisitSalesSeqNextValue As String = "UpdateSalesVisitTableAdapter.GetVisitSalesSeqNextValue"

        ''' <summary>
        ''' メソッド名（来店実績の登録）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MethodNameInsertVisitSales As String = "UpdateSalesVisitTableAdapter.InsertVisitSales"

        ''' <summary>
        ''' メソッド名（Follow-up Box指定の来店実績情報の取得）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MethodNameGetVisitSalesFollowUp As String = "UpdateSalesVisitTableAdapter.GetVisitSalesFollowUp"

        ''' <summary>
        ''' メソッド名（商談終了時の来店実績情報の更新）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MethodNameUpdateVisitSalesEnd As String = "UpdateSalesVisitTableAdapter.UpdateVisitSalesEnd"

        ''' <summary>
        ''' メソッド名（顧客登録時の来店実績情報の更新）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MethodNameUpdateVisitSalesCustomer As String = "UpdateSalesVisitTableAdapter.UpdateVisitSalesCustomer"

        ''' <summary>
        ''' メソッド名（ログイン時の来店実績情報の更新）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MethodNameUpdateVisitLogin As String = "UpdateSalesVisitTableAdapter.UpdateVisitLogin"

#End Region

#Region "配列の名前"

        ''' <summary>
        ''' 配列の名前（引数）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ArrayNameParameters As String = "Param"

        ''' <summary>
        ''' 配列の名前（戻り値）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ArrayNameReturnValues As String = "Ret"

#End Region

#End Region

#Region "来店実績ステータス"

        ''' <summary>
        ''' 来店実績ステータス（01:フリー）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusFree As String = "01"

        ''' <summary>
        ''' 来店実績ステータス（03:調整中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusAdjust As String = "03"

        ''' <summary>
        ''' 来店実績ステータス（04:確定(ブロードキャスト)）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusDecisionBroadcast As String = "04"

        ''' <summary>
        ''' 来店実績ステータス（05:確定）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusDecision As String = "05"

        ''' <summary>
        ''' 来店実績ステータス（06:待ち）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusWait As String = "06"

        ''' <summary>
        ''' 来店実績ステータス（07:商談中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusSalesStart As String = "07"

        ''' <summary>
        ''' 来店実績ステータス（08:商談終了）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusSalesEnd As String = "08"

        ' $01 start 複数顧客に対する商談平行対応
        ''' <summary>
        ''' 来店実績ステータス（09:商談中断）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusSalesStop As String = "09"
        ' $01 end   複数顧客に対する商談平行対応

        ' $03 納車作業ステータス追加 start
        ''' <summary>
        ''' 来店実績ステータス（11:納車作業開始）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusDeliverlyStart As String = "11"

        ''' <summary>
        ''' 来店実績ステータス（12:納車作業終了）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusDeliverlyEnd As String = "12"
        ' $03 納車作業ステータス追加 end

        ''' <summary>
        ''' 来店実績ステータス（99:来店キャンセル）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusVisitCancel As String = "99"

#End Region

        ''' <summary>
        ''' 1行目
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RnumFirst As Integer = 1

        ''' <summary>
        ''' 1番目
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FirstOrder As Integer = 0

        ''' <summary>
        ''' 2番目
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SecondOrder As Integer = 1

        ''' <summary>
        ''' 3番目
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ThirdOrder As Integer = 2

#End Region

#Region "メソッド"
#Region "商談または納車作業開始時の来店実績情報の更新"
        ''' <summary>
        ''' 商談または納車作業開始時の来店実績情報の更新
        ''' </summary>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <param name="account">対応担当スタッフコード</param>
        ''' <param name="followUpBoxDealerCode">Follow-up Box販売店コード</param>
        ''' <param name="followUpBoxStoreCode">Follow-up Box店舗コード</param>
        ''' <param name="followUpBoxSeqNo">Follow-up Box内連番</param>
        ''' <param name="salesStart">商談開始日時</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <param name="updateId">更新機能ID</param>
        ''' <param name="statusClass">ステータス区分</param>
        ''' <returns>処理結果（True：成功　/　False：失敗）</returns>
        ''' <exception cref="OracleExceptionEx">データベースの操作中に例外が発生した場合</exception>
        ''' <remarks></remarks>
        Public Function UpdateVisitSalesStart( _
                ByVal visitSeq As Long, ByVal account As String, _
                ByVal followUpBoxDealerCode As String, ByVal followUpBoxStoreCode As String, _
                ByVal followUpBoxSeqNo As Decimal, ByVal salesStart As Date, _
                ByVal updateAccount As String, ByVal updateId As String, _
                ByVal statusClass As String) As Boolean

            OutputStartLog(MethodNameUpdateVisitSalesStart, visitSeq, account, _
                    followUpBoxDealerCode, followUpBoxStoreCode, followUpBoxSeqNo, salesStart, _
                    updateAccount, updateId)

            ' 更新対象レコード件数
            Dim record As Integer = 0

            Using query As New DBUpdateQuery("UPDATESALESVISIT_002")
                Dim sql As New StringBuilder

                ' SQL文作成
                With sql
                    .Append(" UPDATE /* UPDATESALESVISIT_002 */ ")
                    .Append("        TBL_VISIT_SALES")
                    .Append("    SET VISITSTATUS = :VISITSTATUS")
                    .Append("      , ACCOUNT = :ACCOUNT")
                    .Append("      , FLLWUPBOX_DLRCD = :FLLWUPBOX_DLRCD")
                    .Append("      , FLLWUPBOX_STRCD = :FLLWUPBOX_STRCD")
                    .Append("      , FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO")
                    .Append("      , SALESSTART = :SALESSTART")
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEID = :UPDATEID")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("      , FIRST_SALESSTART = NVL(FIRST_SALESSTART, :SALESSTART)")
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                End With

                ' SQL文を設定
                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数を設定
                ' $03 納車作業ステータス更新追加 start 
                If statusClass = "1" Then
                    query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, _
                            VisitStatusSalesStart)
                Else
                    query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, _
                            VisitStatusDeliverlyStart)
                End If
                ' $03 納車作業ステータス更新追加 end

                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)
                query.AddParameterWithTypeValue("FLLWUPBOX_DLRCD", OracleDbType.Char, _
                        followUpBoxDealerCode)
                query.AddParameterWithTypeValue("FLLWUPBOX_STRCD", OracleDbType.Char, _
                        followUpBoxStoreCode)

                ' Follow-up Box内連番が設定されている場合
                If 0L <> followUpBoxSeqNo Then
                    Logger.Info("UpdateSalesVisitTableAdapter.UpdateVisitSalesStart_001")
                    query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, _
                            followUpBoxSeqNo)

                    ' Follow-up Box内連番が設定されていない場合
                Else
                    Logger.Info("UpdateSalesVisitTableAdapter.UpdateVisitSalesStart_002")
                    query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, _
                            Nothing)
                End If

                query.AddParameterWithTypeValue("SALESSTART", OracleDbType.Date, salesStart)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, _
                        updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, visitSeq)

                ' SQLの実行
                record = query.Execute()
            End Using

            ' 処理結果
            Dim isSuccess As Boolean = False

            ' 実行結果が0件超過の場合
            If 0 < record Then
                ' 処理結果に成功を設定
                isSuccess = True
            End If

            OutputEndLog(MethodNameUpdateVisitSalesStart, isSuccess)

            ' 戻り値に処理結果を設定
            Return isSuccess

        End Function
#End Region
#Region "商談または納車作業開始時の来店実績の取得"
        ''' <summary>
        ''' 商談または納車作業開始時の来店実績の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="customerSegment">顧客区分</param>
        ''' <param name="customerId">顧客コード</param>
        ''' <param name="visitTimestampStart">来店日時開始</param>
        ''' <param name="visitTimestampEnd">来店日時終了</param>
        ''' <returns>来店実績データセット</returns>
        ''' <exception cref="OracleExceptionEx">データベースの操作中に例外が発生した場合</exception>
        ''' <remarks></remarks>
        Public Function GetVisitSalesStart( _
                ByVal dealerCode As String, ByVal storeCode As String, _
                ByVal customerSegment As String, ByVal customerId As String, _
                ByVal visitTimestampStart As Date, ByVal visitTimestampEnd As Date) _
                As UpdateSalesVisitDataSet.UpdateSalesVisitDataTable

            OutputStartLog(MethodNameGetVisitSalesStart, dealerCode, storeCode, customerSegment, _
                    customerId, visitTimestampStart, visitTimestampEnd)

            ' 来店実績データセット
            Dim dt As UpdateSalesVisitDataSet.UpdateSalesVisitDataTable = Nothing

            Using query As New DBSelectQuery( _
                    Of UpdateSalesVisitDataSet.UpdateSalesVisitDataTable)("UPDATESALESVISIT_003")
                Dim sql As New StringBuilder

                ' SQL文作成
                With sql
                    .Append(" SELECT /* UPDATESALESVISIT_003 */")
                    .Append("        T1.VISITSEQ")
                    .Append("      , T1.VISITSTATUS")
                    .Append("   FROM (")
                    .Append("     SELECT T2.VISITSEQ")
                    .Append("          , T2.VISITSTATUS")
                    .Append("          , ROW_NUMBER() OVER (ORDER BY (")
                    .Append("           CASE T2.VISITSTATUS")
                    .Append("           WHEN :VISITSTATUS_SALES_START")
                    .Append("           THEN :FIRST_ORDER")
                    ' $03 start 納車作業ステータス対応
                    .Append("           WHEN :VISITSTATUS_DELIVERLY_START")
                    .Append("           THEN :FIRST_ORDER")
                    ' $03 end   納車作業ステータス対応
                    .Append("           WHEN :VISITSTATUS_SALES_END")
                    .Append("           THEN :THIRD_ORDER")
                    ' $03 start 納車作業ステータス対応
                    .Append("           WHEN :VISITSTATUS_DELIVERLY_END")
                    .Append("           THEN :THIRD_ORDER")
                    ' $03 end   納車作業ステータス対応
                    .Append("           WHEN :VISITSTATUS_VISIT_CANCEL")
                    .Append("           THEN :THIRD_ORDER")
                    .Append("           ELSE :SECOND_ORDER")
                    .Append("            END)")
                    .Append("              , (")
                    .Append("            CASE")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("            WHEN T2.STOPTIME       IS NOT NULL THEN T2.STOPTIME")
                    .Append("            WHEN T2.VISITTIMESTAMP IS NOT NULL THEN T2.VISITTIMESTAMP")
                    .Append("            ELSE T2.SALESSTART")
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append("             END) DESC) AS RNUM")
                    .Append("       FROM TBL_VISIT_SALES T2")
                    .Append("      WHERE T2.DLRCD = :DLRCD")
                    .Append("        AND T2.STRCD = :STRCD")
                    .Append("        AND (")
                    .Append("            CASE")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("            WHEN T2.STOPTIME       IS NOT NULL THEN T2.STOPTIME")
                    .Append("            WHEN T2.VISITTIMESTAMP IS NOT NULL THEN T2.VISITTIMESTAMP")
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append("            ELSE T2.SALESSTART")
                    .Append("             END)")
                    .Append("    BETWEEN :VISITTIMESTAMP_START")
                    .Append("        AND :VISITTIMESTAMP_END")
                    .Append("        AND T2.CUSTSEGMENT = :CUSTSEGMENT")
                    .Append("        AND T2.CUSTID = :CUSTID")
                    .Append("      ) T1")
                    .Append("  WHERE RNUM = :RNUM_FIRST")
                    .Append("    AND T1.VISITSTATUS IN (:VISITSTATUS_FREE, :VISITSTATUS_ADJUST")
                    .Append("          , :VISITSTATUS_DECISION_BROADCAST, :VISITSTATUS_DECISION")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("          , :VISITSTATUS_WAIT, :VISITSTATUS_SALES_START")
                    ' $03 start 納車作業ステータス対応
                    .Append("          , :VISITSTATUS_STOP, :VISITSTATUS_DELIVERLY_START)")
                    ' $03 end 納車作業ステータス対応
                    ' $01 end   複数顧客に対する商談平行対応
                End With

                ' SQL文を設定
                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数を設定
                query.AddParameterWithTypeValue("VISITSTATUS_SALES_START", OracleDbType.Char, _
                        VisitStatusSalesStart)
                query.AddParameterWithTypeValue("VISITSTATUS_SALES_END", OracleDbType.Char, _
                        VisitStatusSalesEnd)
                query.AddParameterWithTypeValue("VISITSTATUS_VISIT_CANCEL", OracleDbType.Char, _
                        VisitStatusVisitCancel)
                query.AddParameterWithTypeValue("FIRST_ORDER", OracleDbType.Decimal, FirstOrder)
                query.AddParameterWithTypeValue("SECOND_ORDER", OracleDbType.Decimal, SecondOrder)
                query.AddParameterWithTypeValue("THIRD_ORDER", OracleDbType.Decimal, ThirdOrder)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("VISITTIMESTAMP_START", OracleDbType.Date, _
                        visitTimestampStart)
                query.AddParameterWithTypeValue("VISITTIMESTAMP_END", OracleDbType.Date, _
                        visitTimestampEnd)
                query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Decimal, _
                        customerSegment)
                query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, customerId)
                query.AddParameterWithTypeValue("RNUM_FIRST", OracleDbType.Decimal, RnumFirst)
                query.AddParameterWithTypeValue("VISITSTATUS_FREE", OracleDbType.Char, _
                        VisitStatusFree)
                query.AddParameterWithTypeValue("VISITSTATUS_ADJUST", OracleDbType.Char, _
                        VisitStatusAdjust)
                query.AddParameterWithTypeValue("VISITSTATUS_DECISION_BROADCAST", _
                        OracleDbType.Char, VisitStatusDecisionBroadcast)
                query.AddParameterWithTypeValue("VISITSTATUS_DECISION", OracleDbType.Char, _
                        VisitStatusDecision)
                query.AddParameterWithTypeValue("VISITSTATUS_WAIT", OracleDbType.Char, _
                        VisitStatusWait)
                ' $01 start 複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("VISITSTATUS_STOP", OracleDbType.Char, _
                        VisitStatusSalesStop)
                ' $01 end   複数顧客に対する商談平行対応
                ' $03 start 納車作業ステータス対応
                query.AddParameterWithTypeValue("VISITSTATUS_DELIVERLY_START", OracleDbType.Char, _
                                                VisitStatusDeliverlyStart)
                query.AddParameterWithTypeValue("VISITSTATUS_DELIVERLY_END", OracleDbType.Char, _
                                VisitStatusDeliverlyEnd)
                ' $03 end   納車作業ステータス対応

                ' SQLの実行
                dt = query.GetData()
            End Using

            OutputEndLog(MethodNameGetVisitSalesStart, dt)

            ' 戻り値に来店実績データセットを設定
            Return dt

        End Function
#End Region
#Region "来店実績シーケンスの取得"
        ''' <summary>
        ''' 来店実績シーケンス取得
        ''' </summary>
        ''' <returns>来店実績シーケンスの次番号</returns>
        ''' <exception cref="OracleExceptionEx">データベースの操作中に例外が発生した場合</exception>
        ''' <remarks></remarks>
        Public Function GetVisitSalesSeqNextValue() As Long

            OutputStartLog(MethodNameGetVisitSalesSeqNextValue)

            ' 来店実績シーケンスの次番号
            Dim visitSeqNextValue As Long = 0L

            Using query As New DBSelectQuery( _
                    Of UpdateSalesVisitDataSet.UpdateSalesVisitDataTable)("UPDATESALESVISIT_004")
                Dim sql As New StringBuilder

                ' SQL文作成
                With sql
                    .Append(" SELECT /* UPDATESALESVISIT_004 */")
                    .Append("        SEQ_VISIT_SALES_VISITSEQ.NEXTVAL AS VISITSEQ")
                    .Append("   FROM DUAL")
                End With

                ' SQL文を設定
                query.CommandText = sql.ToString()
                sql = Nothing

                ' SQLを実行
                Using dt As UpdateSalesVisitDataSet.UpdateSalesVisitDataTable = query.GetData()
                    ' レコードが取得できた場合
                    If 0 < dt.Count Then
                        ' 来店実績シーケンスの次番号を取得
                        visitSeqNextValue = dt.Item(0).VISITSEQ
                    End If
                End Using
            End Using

            OutputEndLog(MethodNameGetVisitSalesSeqNextValue, visitSeqNextValue)

            ' 戻り値に来店実績シーケンスの次番号を設定
            Return visitSeqNextValue

        End Function
#End Region
#Region "来店実績の登録"
        ''' <summary>
        ''' 来店実績の登録
        ''' </summary>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="customerSegment">顧客区分</param>
        ''' <param name="customerId">顧客コード</param>
        ''' <param name="staffCode">顧客担当スタッフコード</param>
        ''' <param name="account">対応担当スタッフコード</param>
        ''' <param name="followUpBoxDealerCode">Follow-up Box販売店コード</param>
        ''' <param name="followUpBoxStoreCode">Follow-up Box店舗コード</param>
        ''' <param name="followUpBoxSeqNo">Follow-up Box内連番</param>
        ''' <param name="salesStart">商談開始日時</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <param name="updateId">更新機能ID</param>
        ''' <param name="statusClass">ステータス区分</param>
        ''' <returns>処理結果（True：成功　/　False：失敗）</returns>
        ''' <exception cref="OracleExceptionEx">データベースの操作中に例外が発生した場合</exception>
        ''' <remarks></remarks>
        Public Function InsertVisitSales( _
                ByVal visitSeq As Long, ByVal dealerCode As String, ByVal storeCode As String, _
                ByVal customerSegment As String, ByVal customerId As String, _
                ByVal staffCode As String, ByVal account As String, _
                ByVal followUpBoxDealerCode As String, ByVal followUpBoxStoreCode As String, _
                ByVal followUpBoxSeqNo As Decimal, ByVal salesStart As Date, _
                ByVal updateAccount As String, ByVal updateId As String, _
                ByVal statusClass As String) As Boolean

            OutputStartLog(MethodNameInsertVisitSales, visitSeq, dealerCode, storeCode, _
                    customerSegment, customerId, staffCode, account, followUpBoxDealerCode, _
                    followUpBoxStoreCode, followUpBoxSeqNo, salesStart, updateAccount, updateId)

            ' 更新対象レコード件数
            Dim record As Integer = 0

            Using query As New DBUpdateQuery("UPDATESALESVISIT_005")
                Dim sql As New StringBuilder

                ' SQL文作成
                With sql
                    .Append(" INSERT /* UPDATESALESVISIT_005 */")
                    .Append("   INTO TBL_VISIT_SALES (")
                    .Append("        VISITSEQ")
                    .Append("      , DLRCD")
                    .Append("      , STRCD")
                    .Append("      , CUSTSEGMENT")
                    .Append("      , CUSTID")
                    .Append("      , STAFFCD")
                    .Append("      , VISITSTATUS")
                    .Append("      , ACCOUNT")
                    .Append("      , FLLWUPBOX_DLRCD")
                    .Append("      , FLLWUPBOX_STRCD")
                    .Append("      , FLLWUPBOX_SEQNO")
                    .Append("      , SALESSTART")
                    .Append("      , CREATEDATE")
                    .Append("      , UPDATEDATE")
                    .Append("      , CREATEACCOUNT")
                    .Append("      , UPDATEACCOUNT")
                    .Append("      , CREATEID")
                    .Append("      , UPDATEID")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("      , FIRST_SALESSTART")
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append(" )")
                    .Append(" VALUES (")
                    .Append("        :VISITSEQ")
                    .Append("      , :DLRCD")
                    .Append("      , :STRCD")
                    .Append("      , :CUSTSEGMENT")
                    .Append("      , :CUSTID")
                    .Append("      , :STAFFCD")
                    .Append("      , :VISITSTATUS")
                    .Append("      , :ACCOUNT")
                    .Append("      , :FLLWUPBOX_DLRCD")
                    .Append("      , :FLLWUPBOX_STRCD")
                    .Append("      , :FLLWUPBOX_SEQNO")
                    .Append("      , :SALESSTART")
                    .Append("      , SYSDATE")
                    .Append("      , SYSDATE")
                    .Append("      , :CREATEACCOUNT")
                    .Append("      , :UPDATEACCOUNT")
                    .Append("      , :CREATEID")
                    .Append("      , :UPDATEID")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("      , :FIRST_SALESSTART")
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append(" )")
                End With

                ' SQL文を設定
                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数を設定
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, visitSeq)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Decimal, _
                        customerSegment)
                query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, customerId)
                query.AddParameterWithTypeValue("STAFFCD", OracleDbType.Varchar2, staffCode)
                ' $03 start 納車作業ステータス対応
                If statusClass = "1" Then
                    query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, _
                            VisitStatusSalesStart)
                Else
                    query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, _
                            VisitStatusDeliverlyStart)
                End If
                ' $03 end   納車作業ステータス対応
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)
                query.AddParameterWithTypeValue("FLLWUPBOX_DLRCD", OracleDbType.Char, _
                        followUpBoxDealerCode)
                query.AddParameterWithTypeValue("FLLWUPBOX_STRCD", OracleDbType.Char, _
                        followUpBoxStoreCode)

                ' Follow-up Box内連番が設定されている場合
                If 0L <> followUpBoxSeqNo Then
                    Logger.Info("UpdateSalesVisitTableAdapter.InsertVisitSales_001")
                    query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, _
                            followUpBoxSeqNo)

                    ' Follow-up Box内連番が設定されていない場合
                Else
                    Logger.Info("UpdateSalesVisitTableAdapter.InsertVisitSales_002")
                    query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, _
                            Nothing)
                End If

                query.AddParameterWithTypeValue("SALESSTART", OracleDbType.Date, salesStart)
                ' $01 start 複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("FIRST_SALESSTART", OracleDbType.Date, salesStart)
                ' $01 end   複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, _
                        updateAccount)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, _
                        updateAccount)
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, updateId)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)

                ' SQLの実行
                record = query.Execute()
            End Using

            ' 処理結果
            Dim isSuccess As Boolean = False

            ' 実行結果が0件超過の場合
            If 0 < record Then
                ' 処理結果に成功を設定
                isSuccess = True
            End If

            OutputEndLog(MethodNameInsertVisitSales, isSuccess)

            ' 戻り値に処理結果を設定
            Return isSuccess

        End Function
#End Region
#Region "顧客指定の来店実績情報の取得"
        ''' <summary>
        ''' 顧客指定の来店実績情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="customerSegment">顧客区分</param>
        ''' <param name="customerId">顧客コード</param>
        ''' <returns>来店実績データセット</returns>
        ''' <exception cref="OracleExceptionEx">データベースの操作中に例外が発生した場合</exception>
        ''' <remarks></remarks>
        Public Function GetVisitSalesFollowUp( _
                ByVal dealerCode As String, ByVal storeCode As String, _
                ByVal customerSegment As String, ByVal customerId As String) _
                As UpdateSalesVisitDataSet.UpdateSalesVisitDataTable

            OutputStartLog(MethodNameGetVisitSalesFollowUp, dealerCode, storeCode, _
                    customerSegment, customerId)

            ' 来店実績データセット
            Dim dt As UpdateSalesVisitDataSet.UpdateSalesVisitDataTable = Nothing

            Using query As New DBSelectQuery( _
                    Of UpdateSalesVisitDataSet.UpdateSalesVisitDataTable)("UPDATESALESVISIT_006")
                Dim sql As New StringBuilder

                ' SQL文作成
                With sql
                    .Append(" SELECT /* UPDATESALESVISIT_006 */")
                    .Append("        VISITSEQ")
                    .Append("      , ACCOUNT")
                    .Append("   FROM TBL_VISIT_SALES")
                    .Append("  WHERE DLRCD = :DLRCD")
                    .Append("    AND STRCD = :STRCD")
                    .Append("    AND CUSTSEGMENT = :CUSTSEGMENT")
                    .Append("    AND CUSTID = :CUSTID")
                    ' $03 start 納車作業中ステータス対応
                    .Append("    AND VISITSTATUS IN (:VISITSTATUS_SALES_START, :VISITSTATUS_DELIVERLY_START)")
                    ' $03 end 納車作業中ステータス対応

                End With

                ' SQL文を設定
                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数を設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Decimal, _
                        customerSegment)
                query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, customerId)

                query.AddParameterWithTypeValue("VISITSTATUS_SALES_START", OracleDbType.Char, _
                        VisitStatusSalesStart)
                ' $03 start 納車作業中ステータス対応
                query.AddParameterWithTypeValue("VISITSTATUS_DELIVERLY_START", OracleDbType.Char, _
                        VisitStatusDeliverlyStart)
                ' $03 end 納車作業中ステータス対応
                ' SQLの実行
                dt = query.GetData()
            End Using

            OutputEndLog(MethodNameGetVisitSalesFollowUp, dt)

            ' 戻り値に来店実績データセットを設定
            Return dt

        End Function
#End Region
#Region "商談または納車作業終了時の来店実績情報の更新"
        ''' <summary>
        ''' 商談または納車作業終了時の来店実績情報の更新
        ''' </summary>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <param name="salesEnd">商談終了日時</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <param name="updateId">更新機能ID</param>
        ''' <param name="statusClass">ステータス区分</param>
        ''' <returns>処理結果（True：成功　/　False：失敗）</returns>
        ''' <exception cref="OracleExceptionEx">データベースの操作中に例外が発生した場合</exception>
        ''' <remarks></remarks>
        Public Function UpdateVisitSalesEnd( _
                ByVal visitSeq As Long, ByVal salesEnd As Date, ByVal updateAccount As String, _
                ByVal updateId As String, ByVal statusClass As String) As Boolean

            OutputStartLog(MethodNameUpdateVisitSalesEnd, visitSeq, salesEnd, updateAccount, _
                    updateId)

            ' 更新対象レコード件数
            Dim record As Integer = 0

            Using query As New DBUpdateQuery("UPDATESALESVISIT_007")
                Dim sql As New StringBuilder

                ' SQL文作成
                With sql
                    .Append(" UPDATE /* UPDATESALESVISIT_007 */")
                    .Append("        TBL_VISIT_SALES")
                    .Append("    SET VISITSTATUS = :VISITSTATUS")
                    .Append("      , SALESEND = :SALESEND")
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEID = :UPDATEID")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                End With

                ' SQL文を設定
                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数を設定
                '$03 start 納車作業終了ステータス対応
                If statusClass = "1" Then
                    query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, _
                            VisitStatusSalesEnd)
                Else
                    query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, _
                            VisitStatusDeliverlyEnd)
                End If
                query.AddParameterWithTypeValue("SALESEND", OracleDbType.Date, salesEnd)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, _
                        updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, visitSeq)

                ' SQLの実行
                record = query.Execute()
            End Using

            ' 処理結果
            Dim isSuccess As Boolean = False

            ' 実行結果が0件超過の場合
            If 0 < record Then
                ' 処理結果に成功を設定
                isSuccess = True
            End If

            OutputEndLog(MethodNameUpdateVisitSalesEnd, isSuccess)

            ' 戻り値に処理結果を設定
            Return isSuccess

        End Function
#End Region
#Region "顧客登録時の来店実績情報の更新"
        ''' <summary>
        ''' 顧客登録時の来店実績情報の更新
        ''' </summary>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <param name="customerSegment">顧客区分</param>
        ''' <param name="customerId">顧客コード</param>
        ''' <param name="staffCode">顧客担当スタッフコード</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <param name="updateId">更新機能ID</param>
        ''' <returns>処理結果（True：成功　/　False：失敗）</returns>
        ''' <exception cref="OracleExceptionEx">データベースの操作中に例外が発生した場合</exception>
        ''' <remarks></remarks>
        Public Function UpdateVisitSalesCustomer( _
                ByVal visitSeq As Long, ByVal customerSegment As String, _
                ByVal customerId As String, ByVal staffCode As String, _
                ByVal updateAccount As String, ByVal updateId As String) As Boolean

            OutputStartLog(MethodNameUpdateVisitSalesCustomer, visitSeq, customerSegment, _
                    customerId, staffCode, updateAccount, updateId)

            ' 更新対象レコード件数
            Dim record As Integer = 0

            Using query As New DBUpdateQuery("UPDATESALESVISIT_008")
                Dim sql As New StringBuilder

                ' SQL文作成
                With sql
                    .Append(" UPDATE /* UPDATESALESVISIT_008 */ ")
                    .Append("        TBL_VISIT_SALES")
                    .Append("    SET CUSTSEGMENT = :CUSTSEGMENT")
                    .Append("      , CUSTID = :CUSTID")
                    .Append("      , STAFFCD = :STAFFCD")
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEID = :UPDATEID")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                    .Append("    AND CUSTSEGMENT IS NULL")
                    .Append("    AND CUSTID IS NULL")
                End With

                ' SQL文を設定
                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数を設定
                query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Decimal, _
                        customerSegment)
                query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, customerId)
                query.AddParameterWithTypeValue("STAFFCD", OracleDbType.Varchar2, staffCode)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, _
                        updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, visitSeq)

                ' SQLの実行
                record = query.Execute()
            End Using

            ' 処理結果
            Dim isSuccess As Boolean = False

            ' 実行結果が0件超過の場合
            If 0 < record Then
                ' 処理結果に成功を設定
                isSuccess = True
            End If

            OutputEndLog(MethodNameUpdateVisitSalesCustomer, isSuccess)

            ' 戻り値に処理結果を設定
            Return isSuccess

        End Function
#End Region
#Region "ログイン時の来店実績情報の更新"
        ''' <summary>
        ''' ログイン時の来店実績情報の更新
        ''' </summary>
        ''' <param name="account">対応担当スタッフコード</param>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <param name="updateId">更新機能ID</param>
        ''' <returns>更新件数</returns>
        ''' <exception cref="OracleExceptionEx">データベースの操作中に例外が発生した場合</exception>
        ''' <remarks></remarks>
        Public Function UpdateVisitLogin( _
                ByVal account As String, ByVal dealerCode As String, ByVal storeCode As String, _
                ByVal updateAccount As String, ByVal updateId As String) As Integer

            OutputStartLog(MethodNameUpdateVisitLogin, account, dealerCode, storeCode, _
                    updateAccount, updateId)

            ' 更新対象レコード件数
            Dim record As Integer = 0

            Using query As New DBUpdateQuery("UPDATESALESVISIT_009")
                Dim sql As New StringBuilder

                ' SQL文作成
                With sql
                    .Append(" UPDATE /* UPDATESALESVISIT_009 */")
                    .Append("        TBL_VISIT_SALES")
                    '$03 start 納車作業中ステータス対応
                    .Append("    SET VISITSTATUS =  ")
                    .Append("   CASE ")
                    .Append("   WHEN VISITSTATUS = :VISITSTATUS_SALES_START ")
                    .Append("   THEN :VISITSTATUS_SALES_END ")
                    .Append("   WHEN VISITSTATUS = :VISITSTATUS_DELIVERLY_START ")
                    .Append("   THEN :VISITSTATUS_DELIVERLY_END ")
                    .Append("    END")
                    '$03 end 納車作業中ステータス対応
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEID = :UPDATEID")
                    .Append("  WHERE DLRCD = :DLRCD")
                    .Append("    AND STRCD = :STRCD")
                    .Append("    AND VISITSTATUS IN (:VISITSTATUS_SALES_START, :VISITSTATUS_DELIVERLY_START)")
                    .Append("    AND ACCOUNT = :ACCOUNT")
                End With

                ' SQL文を設定
                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数を設定
                query.AddParameterWithTypeValue("VISITSTATUS_SALES_END", OracleDbType.Char, _
                        VisitStatusSalesEnd)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, _
                        updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("VISITSTATUS_SALES_START", OracleDbType.Char, _
                        VisitStatusSalesStart)

                '$03 start 納車作業中ステータス対応
                query.AddParameterWithTypeValue("VISITSTATUS_DELIVERLY_START", OracleDbType.Char, _
                        VisitStatusDeliverlyStart)
                query.AddParameterWithTypeValue("VISITSTATUS_DELIVERLY_END", OracleDbType.Char, _
                        VisitStatusDeliverlyEnd)
                '$03 start 納車作業中ステータス対応
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)

                ' SQLの実行
                record = query.Execute()
            End Using

            OutputEndLog(MethodNameUpdateVisitLogin, record)

            ' 戻り値に更新対象レコード件数を設定
            Return record

        End Function
#End Region
#Region "商談中断時の来店実績情報の作成"
        ' $01 start 複数顧客に対する商談平行対応
        ''' <summary>
        ''' 商談中断時の来店実績情報の作成
        ''' </summary>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <param name="stopTime">商談中断日時</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <param name="updateId">更新機能ID</param>
        ''' <returns>処理結果（True：成功　/　False：失敗）</returns>
        ''' <exception cref="OracleExceptionEx">データベースの操作中に例外が発生した場合</exception>
        ''' <remarks></remarks>
        Public Function CopyVisitSalesStop( _
                ByVal visitSeq As Long, ByVal stopTime As Date, _
                ByVal updateAccount As String, ByVal updateId As String) As Boolean

            OutputStartLog(MethodNameInsertVisitSales, visitSeq, stopTime, updateAccount, updateId)

            ' 更新対象レコード件数
            Dim record As Integer = 0
            ' 新規来店実績連番
            Dim newVisitSeq As Long = Me.GetVisitSalesSeqNextValue()

            Using query As New DBUpdateQuery("UPDATESALESVISIT_010")
                Dim sql As New StringBuilder

                ' SQL文作成
                With sql
                    .Append(" INSERT /* UPDATESALESVISIT_010 */")
                    .Append("   INTO TBL_VISIT_SALES (")
                    .Append("        VISITSEQ")
                    .Append("      , DLRCD")
                    .Append("      , STRCD")
                    .Append("      , VISITTIMESTAMP")
                    .Append("      , VCLREGNO")
                    .Append("      , CUSTSEGMENT")
                    .Append("      , CUSTID")
                    .Append("      , STAFFCD")
                    .Append("      , VISITPERSONNUM")
                    .Append("      , VISITMEANS")
                    .Append("      , VISITSTATUS")
                    .Append("      , BROUDCASTFLG")
                    .Append("      , TENTATIVENAME")
                    .Append("      , ACCOUNT")
                    .Append("      , SALESTABLENO")
                    .Append("      , FLLWUPBOX_DLRCD")
                    .Append("      , FLLWUPBOX_STRCD")
                    .Append("      , FLLWUPBOX_SEQNO")
                    .Append("      , CREATEDATE")
                    .Append("      , UPDATEDATE")
                    .Append("      , CREATEACCOUNT")
                    .Append("      , UPDATEACCOUNT")
                    .Append("      , CREATEID")
                    .Append("      , UPDATEID")
                    .Append("      , STOPTIME")
                    .Append("      , FIRST_SALESSTART")
                    ' $02 start 新車タブレットショールーム管理機能開発
                    .Append("      , UNNECESSARYCOUNT")
                    .Append("      , UNNECESSARYDATE")
                    .Append("      , SC_ASSIGNDATE")
                    ' $02 end   新車タブレットショールーム管理機能開発
                    .Append(" )")
                    .Append(" SELECT")
                    .Append("        :NEWVISITSEQ")
                    .Append("      , DLRCD")
                    .Append("      , STRCD")
                    .Append("      , VISITTIMESTAMP")
                    .Append("      , VCLREGNO")
                    .Append("      , CUSTSEGMENT")
                    .Append("      , CUSTID")
                    .Append("      , STAFFCD")
                    .Append("      , VISITPERSONNUM")
                    .Append("      , VISITMEANS")
                    .Append("      , :VISITSTATUS")
                    .Append("      , BROUDCASTFLG")
                    .Append("      , TENTATIVENAME")
                    .Append("      , ACCOUNT")
                    .Append("      , SALESTABLENO")
                    .Append("      , FLLWUPBOX_DLRCD")
                    .Append("      , FLLWUPBOX_STRCD")
                    .Append("      , FLLWUPBOX_SEQNO")
                    .Append("      , SYSDATE")
                    .Append("      , SYSDATE")
                    .Append("      , :CREATEACCOUNT")
                    .Append("      , :UPDATEACCOUNT")
                    .Append("      , :CREATEID")
                    .Append("      , :UPDATEID")
                    .Append("      , :STOPTIME")
                    .Append("      , FIRST_SALESSTART")
                    ' $02 start 新車タブレットショールーム管理機能開発
                    .Append("      , UNNECESSARYCOUNT")
                    .Append("      , UNNECESSARYDATE")
                    .Append("      , SC_ASSIGNDATE")
                    ' $02 end   新車タブレットショールーム管理機能開発
                    .Append("   FROM TBL_VISIT_SALES")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                End With

                ' SQL文を設定
                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数を設定
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, visitSeq)
                query.AddParameterWithTypeValue("NEWVISITSEQ", OracleDbType.Decimal, newVisitSeq)
                query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, _
                        VisitStatusSalesStop)
                query.AddParameterWithTypeValue("STOPTIME", OracleDbType.Date, stopTime)
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, _
                        updateAccount)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, _
                        updateAccount)
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, updateId)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)

                ' SQLの実行
                record = query.Execute()
            End Using

            ' 処理結果
            Dim isSuccess As Boolean = False

            ' 実行結果が0件超過の場合
            If 0 < record Then
                ' 処理結果に成功を設定
                isSuccess = True
            End If

            OutputEndLog(MethodNameInsertVisitSales, isSuccess)

            ' 戻り値に処理結果を設定
            Return isSuccess

        End Function
        ' $01 end   複数顧客に対する商談平行対応
#End Region
#Region "ログ出力"

        ''' <summary>
        ''' 開始ログを出力する
        ''' </summary>
        ''' <param name="name">メソッド名</param>
        ''' <param name="parameters">メソッドの引数</param>
        ''' <remarks></remarks>
        Private Sub OutputStartLog(ByVal name As String, ByVal ParamArray parameters As Object())

            Dim sb As New StringBuilder(name)
            sb.Append("_Start")
            AppendArray(sb, ArrayNameParameters, parameters)
            Logger.Info(sb.ToString())
            sb = Nothing

        End Sub

        ''' <summary>
        ''' 配列の内容を StringBuilder の末尾に追加する
        ''' </summary>
        ''' <param name="sb">StringBuilder</param>
        ''' <param name="arrayName">配列の名前</param>
        ''' <param name="array">配列</param>
        ''' <remarks></remarks>
        Private Sub AppendArray( _
                ByVal sb As StringBuilder, ByVal arrayName As String, ByVal array As Object())

            With sb
                Dim lastIndex As Integer = array.Length - 1

                ' すべての要素
                For i As Integer = 0 To lastIndex
                    ' 最初の要素
                    If 0 = i Then
                        .Append(" ")
                        .Append(arrayName)
                        .Append("[")

                        ' 最初の要素でない場合
                    Else
                        .Append(", ")
                    End If

                    .Append(array(i))

                    ' データテーブルの場合
                    If TypeOf array(i) Is DataTable Then
                        .Append("[Count = ")
                        .Append(DirectCast(array(i), DataTable).Rows.Count)
                        .Append("]")
                    End If

                    ' 最後の要素の場合
                    If i = lastIndex Then
                        .Append("]")
                    End If
                Next i
            End With

        End Sub

        ''' <summary>
        ''' 終了ログを出力する
        ''' </summary>
        ''' <param name="name">メソッド名</param>
        ''' <param name="returnValues">メソッドの戻り値</param>
        ''' <remarks></remarks>
        Private Sub OutputEndLog(ByVal name As String, ByVal ParamArray returnValues As Object())

            Dim sb As New StringBuilder(name)
            sb.Append("_End")
            AppendArray(sb, ArrayNameReturnValues, returnValues)
            Logger.Info(sb.ToString())
            sb = Nothing

        End Sub

#End Region

#End Region

    End Class

End Namespace