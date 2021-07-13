'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100103DataSet.vb
'──────────────────────────────────
'機能： スタンバイスタッフ並び順変更
'補足： 
'作成： 2012/08/17 TMEJ m.okamura
'更新： 2013/02/27 TMEJ t.shimamura 新車タブレット受付画面管理指標変更対応 $01
'更新： 2013/05/24 TMEJ t.shimamura 【A.STEP2】次世代e-CRB新車タブレット　新DB適応に向けた機能開発 $02
'──────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace SC3100103DataSetTableAdapters

    ''' <summary>
    ''' SC3100103
    ''' スタンバイスタッフ並び順変更 データアクセスクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3100103TableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' 来店実績ステータス（商談中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusNegotiate As String = "07"

        ''' <summary>
        ''' 来店実績ステータス（商談終了）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusNegotiateEnd As String = "08"

        ' $01 start 納車作業ステータス対応
        ''' <summary>
        ''' 来店実績ステータス（商談終了）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusDeliverlyEnd As String = "12"
        ' $01 end   納車作業ステータス対応

        ''' <summary>
        ''' スタッフステータス（スタンバイ）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StaffStatusStandby As String = "1"

        ''' <summary>
        ''' 操作権限コード（セールススタッフ）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OperationCodeSalesStaff As Long = 8

        ''' <summary>
        ''' 削除フラグ（未削除）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DeleteFlagNotDelete As String = "0"

#End Region

#Region "スタッフ情報の取得"

        ''' <summary>
        ''' スタッフ情報の取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="startTime">取得開始日時</param>
        ''' <param name="endTime">取得終了日時</param>
        ''' <returns>商談テーブル使用有無データテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetTargetStaff(ByVal dealerCode As String, ByVal storeCode As String, _
                                       ByVal startTime As Date, ByVal endTime As Date) _
                                       As SC3100103DataSet.SC3100103TargetStaffDataTable

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of SC3100103DataSet.SC3100103TargetStaffDataTable)("SC3100103_001")

                'SQL組み立て
                Dim sql As New StringBuilder

                With sql
                    .Append("SELECT /* SC3100103_001 */")
                    .Append("          US.ACCOUNT")
                    .Append("        , US.USERNAME")
                    .Append("        , US.ORG_IMGFILE")
                    .Append("        , US.PRESENCECATEGORYDATE")
                    .Append("        , VS.RESULTCOUNT")
                    .Append("        , VS.MAXSALESEND")
                    .Append("     FROM TBL_USERS US")
                    .Append("        , TBL_STANDBYSTAFF_SORT SS")
                    .Append("        , (")
                    .Append("        SELECT VSS.ACCOUNT")
                    ' $02 start
                    .Append("             , COUNT(DISTINCT RPAD(VSS.CUSTID, 20) || CUSTSEGMENT) AS RESULTCOUNT")
                    ' $02 end
                    .Append("             , MAX(VSS.SALESEND) AS MAXSALESEND")
                    .Append("          FROM TBL_VISIT_SALES VSS")
                    .Append("         WHERE VSS.DLRCD = :DLRCD")
                    .Append("           AND VSS.STRCD = :STRCD")
                    .Append("           AND VSS.SALESSTART BETWEEN :STARTTIME")
                    .Append("                              AND :ENDTIME")
                    .Append("           AND VSS.VISITSTATUS IN (:VISITSTATUS_SALES_END, :VISITSTATUS_DELIVERLY_END)")
                    .Append("         GROUP BY ACCOUNT")
                    .Append("          ) VS")
                    .Append("    WHERE US.ACCOUNT = SS.ACCOUNT(+)")
                    .Append("      AND US.DLRCD = SS.DLRCD(+)")
                    .Append("      AND US.STRCD = SS.STRCD(+)")
                    .Append("      AND US.PRESENCECATEGORYDATE = SS.PRESENCECATEGORYDATE(+)")
                    .Append("      AND US.ACCOUNT = VS.ACCOUNT(+)")
                    .Append("      AND US.DLRCD = :DLRCD")
                    .Append("      AND US.STRCD = :STRCD")
                    .Append("      AND US.PRESENCECATEGORY = :PRESENCECATEGORY")
                    .Append("      AND US.DELFLG = :DELFLG")
                    .Append("      AND US.OPERATIONCODE = :OPERATIONCODE")
                    .Append("    ORDER BY SS.SORTNO ASC")
                    .Append("           , US.PRESENCECATEGORYDATE ASC")

                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                'SQLパラメータ設定
                ' $01 start 納車作業ステータス対応
                query.AddParameterWithTypeValue("VISITSTATUS_SALES_END", OracleDbType.Char, VisitStatusNegotiateEnd)
                query.AddParameterWithTypeValue("VISITSTATUS_DELIVERLY_END", OracleDbType.Char, VisitStatusDeliverlyEnd)
                ' $01 end   納車作業ステータス対応
                query.AddParameterWithTypeValue("PRESENCECATEGORY", OracleDbType.Char, StaffStatusStandby)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlagNotDelete)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Long, OperationCodeSalesStaff)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, startTime)
                query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, endTime)

                'SQL実行（結果表を返却）
                Return query.GetData()

            End Using

        End Function

#End Region

#Region "スタッフ並び順の削除"

        ''' <summary>
        ''' スタッフ並び順の削除
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <returns>処理結果(True：成功/False：失敗)</returns>
        ''' <remarks></remarks>
        Public Function DeleteStaffSort(ByVal dealerCode As String, ByVal storeCode As String) _
            As Boolean

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3100103_002")

                'SQL組み立て
                Dim sql As New StringBuilder

                With sql

                    .Append(" DELETE /* SC3100103_002 */")
                    .Append("   FROM TBL_STANDBYSTAFF_SORT")
                    .Append("  WHERE DLRCD = :DLRCD")
                    .Append("    AND STRCD = :STRCD")

                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)

                'SQL実行（結果を返却）
                If query.Execute() >= 0 Then
                    Return True
                Else
                    Return False
                End If

            End Using

        End Function

#End Region

#Region "スタッフ並び順の登録"

        ''' <summary>
        ''' スタッフ並び順の登録
        ''' </summary>
        ''' <param name="staffSortRow">スタッフ並び順データロウ</param>
        ''' <param name="createAccount">作成アカウント</param>
        ''' <param name="createId">作成機能ID</param>
        ''' <remarks></remarks>
        Public Function InsertStaffSort(ByVal staffSortRow As SC3100103DataSet.SC3100103StandByStaffSortRow, _
                                        ByVal createAccount As String, ByVal createId As String) _
                                        As Boolean

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3100103_003")

                'SQL組み立て
                Dim sql As New StringBuilder

                With sql
                    .Append(" INSERT /* SC3100103_003 */")
                    .Append("   INTO TBL_STANDBYSTAFF_SORT (")
                    .Append("        DLRCD")
                    .Append("      , STRCD")
                    .Append("      , ACCOUNT")
                    .Append("      , PRESENCECATEGORYDATE")
                    .Append("      , SORTNO")
                    .Append("      , CREATEDATE")
                    .Append("      , UPDATEDATE")
                    .Append("      , CREATEACCOUNT")
                    .Append("      , UPDATEACCOUNT")
                    .Append("      , CREATEID")
                    .Append("      , UPDATEID")
                    .Append(" )")
                    .Append(" VALUES (")
                    .Append("        :DLRCD")
                    .Append("      , :STRCD")
                    .Append("      , :ACCOUNT")
                    .Append("      , :PRESENCECATEGORYDATE")
                    .Append("      , :SORTNO")
                    .Append("      , SYSDATE")
                    .Append("      , SYSDATE")
                    .Append("      , :CREATEACCOUNT")
                    .Append("      , :UPDATEACCOUNT")
                    .Append("      , :CREATEID")
                    .Append("      , :UPDATEID")
                    .Append(" )")
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, staffSortRow.DLRCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, staffSortRow.STRCD)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, staffSortRow.ACCOUNT)
                query.AddParameterWithTypeValue("PRESENCECATEGORYDATE", OracleDbType.Date, staffSortRow.PRESENCECATEGORYDATE)
                query.AddParameterWithTypeValue("SORTNO", OracleDbType.Int16, staffSortRow.SORTNO)
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, createAccount)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, createAccount)
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, createId)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, createId)

                'SQL実行（結果を返却）
                If query.Execute() > 0 Then
                    Return True
                Else
                    Return False
                End If

            End Using

        End Function

#End Region

    End Class

End Namespace

Partial Class SC3100103DataSet
End Class
