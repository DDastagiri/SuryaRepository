'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3810203DataSet.vb
'─────────────────────────────────────
'機能： 来店情報登録
'補足： 
'作成： 2012/09/19 TMEJ 小澤
'更新： 2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新： 2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発
'更新： 2014/02/24 TMEJ 小澤 店舗展開のユーザテスト不具合 緊急対応
'更新： 2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発
'更新： 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新： 2018/12/13 NSK 坂本　ウェルカムボードが同じお客様を2件表示する
'更新：
'─────────────────────────────────────

Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace IC3810203DataSetTableAdapters
    Public Class IC3810203DataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' 振当ステータス（SA振当済）
        ''' </summary>
        ''' <remarks></remarks>
        Public Const AssignFinished As String = "2"

        ''' <summary>
        ''' 振当ステータス（退店）
        ''' </summary>
        ''' <remarks></remarks>
        Public Const AssignOutStore As String = "4"

        ''' <summary>
        ''' 実績_ステータス（完了）
        ''' </summary>
        ''' <remarks></remarks>
        Public Const ResultStatusFinished As String = "99"

        ''' <summary>
        ''' 案内待ちキュー状態(非案内待ち)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const QueueStatusNotWait As String = "1"

        ''' <summary>
        ''' 画面ID:顧客情報編集画面
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SYSTEM_EDIT_CUSTOMER_INFO As String = "SC3080209"

        ''' <summary>
        ''' 画面ID:車両情報編集画面
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SYSTEM_EDIT_VECHICLE_INFO As String = "SC3080211"

        '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        ''' <summary>
        ''' 顧客種別
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CustomerTypeNew As String = "2"

        ''' <summary>
        ''' サービスステータス（02：キャンセル）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiceStatusCancel As String = "02"

        ''' <summary>
        ''' サービスステータス（13：納車済み）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiceStatusFinishDelivery As String = "13"

        '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 START

        ''' <summary>
        ''' 本日来店フラグ（0：本日来店ではない）
        ''' </summary>
        ''' <remarks></remarks>
        Public Const VisitTimeStampTypeNone As String = "0"
        ''' <summary>
        ''' 本日来店フラグ（1：本日来店）
        ''' </summary>
        ''' <remarks></remarks>
        Public Const VisitTimeStampTypeToday As String = "1"

        ''' <summary>
        ''' オーナーチェンジフラグ（0：未設定）
        ''' </summary>
        ''' <remarks></remarks>
        Public Const OwnerChangeTypeNone As String = "0"

        '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 END

        '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 START
        ''' <summary>
        ''' CALLSTATUS
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CALLSTATUS_2 As String = "2"
        '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 END
#End Region

        ''' <summary>
        ''' サービス来店者管理テーブルの存在チェック
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <returns>サービス来店者キー情報</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' 2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        Public Function GetVisitKey(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow) As IC3810203DataSet.IC3810203VisitKeyDataTable
            Try
                ''引数をログに出力
                Dim args As New List(Of String)
                ' DataRow内の項目を列挙
                Me.AddLogData(args, rowIN)
                ''開始ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

                Using query As New DBSelectQuery(Of IC3810203DataSet.IC3810203VisitKeyDataTable)("IC3810203_001")
                    ''SQLの設定
                    Dim sql As New StringBuilder
                    sql.AppendLine("SELECT /* IC3810203_001 */")
                    sql.AppendLine("       SACODE")
                    sql.AppendLine("     , ASSIGNSTATUS")
                    sql.AppendLine("  FROM TBL_SERVICE_VISIT_MANAGEMENT")
                    sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
                    sql.AppendLine("   AND DLRCD = :DLRCD")
                    sql.AppendLine("   AND STRCD = :STRCD")
                    query.CommandText = sql.ToString()
                    ''パラメータの設定
                    query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, rowIN.DLRCD)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, rowIN.STRCD)
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                    ''SQLの実行
                    Using dt As IC3810203DataSet.IC3810203VisitKeyDataTable = query.GetData()
                        ''終了ログの出力
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))
                        Return dt
                    End Using
                End Using
            Finally

            End Try
        End Function

        ''' <summary>
        ''' サービス来店者管理テーブルのユニークキー情報取得
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <returns>サービス来店者ユニーキー情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' 2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発
        ''' </history>
        Public Function GetVisitUniqueKey(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow, _
                                          ByVal inNowDate As Date) As IC3810203DataSet.IC3810203VisitUniqueKeyDataTable
            '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 START
            'Public Function GetVisitUniqueKey(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow) As IC3810203DataSet.IC3810203VisitUniqueKeyDataTable
            '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 END
            Try
                ''引数をログに出力
                Dim args As New List(Of String)
                ' DataRow内の項目を列挙
                Me.AddLogData(args, rowIN)
                ''開始ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

                Using query As New DBSelectQuery(Of IC3810203DataSet.IC3810203VisitUniqueKeyDataTable)("IC3810203_002")
                    ''SQLの設定
                    Dim sql As New StringBuilder

                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'sql.AppendLine("SELECT /* IC3810203_002 */")
                    'sql.AppendLine("       T1.VISITSEQ")
                    'sql.AppendLine("     , T1.SACODE")
                    'sql.AppendLine("     , T1.ASSIGNSTATUS")
                    'sql.AppendLine("  FROM TBL_SERVICE_VISIT_MANAGEMENT T1")
                    'sql.AppendLine("  LEFT JOIN TBL_STALLPROCESS T2 ")
                    'sql.AppendLine("    ON T2.DLRCD = T1.DLRCD")
                    'sql.AppendLine("   AND T2.STRCD = T1.STRCD")
                    'sql.AppendLine("   AND T2.REZID = T1.REZID")
                    'sql.AppendLine("   AND T2.RESULT_STATUS <> :RESULT_STATUS")
                    'sql.AppendLine(" WHERE T1.DLRCD = :DLRCD")
                    'sql.AppendLine("   AND T1.STRCD = :STRCD")
                    'sql.AppendLine("   AND T1.ASSIGNSTATUS <> :ASSIGNSTATUS")
                    'sql.AppendLine("   AND RTRIM(T1.DMSID) = :DMSID")
                    'sql.AppendLine("   AND (RTRIM(T1.VCLREGNO) = :VCLREGNO")
                    'sql.AppendLine("       OR RTRIM(T1.VIN) = :VIN")
                    'sql.AppendLine("       ) ")

                    ' ''顧客情報編集画面以外および車両情報編集画面以外から呼ばれた場合、
                    ' ''整備受注No.が発行前のデータを対象とする
                    'If (rowIN.SYSTEM.TrimEnd <> SYSTEM_EDIT_CUSTOMER_INFO) _
                    '    AndAlso (rowIN.SYSTEM.TrimEnd <> SYSTEM_EDIT_VECHICLE_INFO) Then
                    '    sql.AppendLine("   AND TRIM(T1.ORDERNO) IS NULL")
                    'End If

                    'sql.AppendLine("   AND (T2.SEQNO IS NULL ")
                    'sql.AppendLine("       OR (T2.DSEQNO = (SELECT MAX(T3.DSEQNO) ")
                    'sql.AppendLine("                          FROM TBL_STALLPROCESS T3 ")
                    'sql.AppendLine("                         WHERE T3.DLRCD = T2.DLRCD ")
                    'sql.AppendLine("                           AND T3.STRCD = T2.STRCD ")
                    'sql.AppendLine("                           AND T3.REZID = T2.REZID ")
                    'sql.AppendLine("                         GROUP BY T3.DLRCD, T3.STRCD, T3.REZID) ")
                    'sql.AppendLine("          AND T2.SEQNO = (SELECT MAX(T4.SEQNO) ")
                    'sql.AppendLine("                            FROM TBL_STALLPROCESS T4 ")
                    'sql.AppendLine("                           WHERE T4.DLRCD = T2.DLRCD ")
                    'sql.AppendLine("                             AND T4.STRCD = T2.STRCD ")
                    'sql.AppendLine("                             AND T4.REZID = T2.REZID ")
                    'sql.AppendLine("                             AND T4.DSEQNO = T2.DSEQNO) ")
                    'sql.AppendLine("          ) ")
                    'sql.AppendLine("       ) ")
                    'sql.AppendLine(" ORDER BY T1.CREATEDATE")
                    With sql
                        .AppendLine("SELECT /* IC3810203_002 */ ")
                        .AppendLine("       T1.VISITSEQ ")
                        .AppendLine("      ,T1.SACODE ")
                        .AppendLine("      ,T1.ASSIGNSTATUS ")
                        .AppendLine("  FROM ")
                        .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                        .AppendLine("      ,TB_T_SERVICEIN T2 ")
                        .AppendLine(" WHERE ")
                        .AppendLine("       T1.DLRCD = T2.DLR_CD(+) ")
                        .AppendLine("   AND T1.STRCD = T2.BRN_CD(+) ")
                        .AppendLine("   AND T1.REZID = T2.SVCIN_ID(+) ")
                        .AppendLine("   AND T1.DLRCD = :DLRCD ")
                        .AppendLine("   AND T1.STRCD = :STRCD ")
                        .AppendLine("   AND T1.ASSIGNSTATUS <> :ASSIGNSTATUS_4 ")
                        .AppendLine("   AND RTRIM(T1.DMSID) = :DMSID ")
                        .AppendLine("   AND (RTRIM(T1.VCLREGNO) = :VCLREGNO ")
                        .AppendLine("        OR RTRIM(T1.VIN) = :VIN) ")
                        '顧客情報編集画面以外および車両情報編集画面以外から呼ばれた場合、整備受注No.が発行前のデータを対象とする
                        If (rowIN.SYSTEM.TrimEnd <> SYSTEM_EDIT_CUSTOMER_INFO) _
                            AndAlso (rowIN.SYSTEM.TrimEnd <> SYSTEM_EDIT_VECHICLE_INFO) Then
                            .AppendLine("   AND TRIM(T1.ORDERNO) IS NULL ")
                        End If
                        .AppendLine("   AND (T2.SVCIN_ID IS NULL ")
                        .AppendLine("        OR (NOT EXISTS (SELECT 1 ")
                        .AppendLine("                          FROM TB_T_SERVICEIN T3 ")
                        .AppendLine("                         WHERE T3.SVCIN_ID = T2.SVCIN_ID ")
                        .AppendLine("                           AND T3.SVC_STATUS = :SVC_STATUS_13))) ")
                        '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 START
                        .AppendLine("   AND T1.VISITTIMESTAMP >= TRUNC(:NOWDATE) ")
                        '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 END
                        .AppendLine(" ORDER BY T1.CREATEDATE ")
                    End With
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                    query.CommandText = sql.ToString()

                    ''パラメータの設定
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                    'query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.Char, AssignOutStore)
                    'query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, rowIN.DMSID.TrimEnd)
                    'query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, rowIN.VCLREGNO.TrimEnd)
                    'query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowIN.VIN.TrimEnd)
                    'query.AddParameterWithTypeValue("RESULT_STATUS", OracleDbType.Char, ResultStatusFinished)
                    '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 START
                    query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                    '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 END
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, rowIN.DLRCD)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, rowIN.STRCD)
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_4", OracleDbType.NVarchar2, AssignOutStore)
                    query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, rowIN.DMSID)
                    query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, rowIN.VCLREGNO)
                    query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, rowIN.VIN)
                    query.AddParameterWithTypeValue("SVC_STATUS_13", OracleDbType.NVarchar2, ServiceStatusFinishDelivery)
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                    ''SQLの実行
                    Using dt As IC3810203DataSet.IC3810203VisitUniqueKeyDataTable = query.GetData()
                        ''終了ログの出力
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))
                        Return dt
                    End Using
                End Using
            Finally

            End Try
        End Function

        '2018/12/13 NSK 坂本　ウェルカムボードが同じお客様を2件表示する START
        ''' <summary>
        ''' サービス来店者管理テーブルのユニークキー情報取得（サービス入庫）
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <returns>サービス来店者ユニーキー情報</returns>
        ''' <remarks></remarks>
        Public Function GetVisitUniqueKeyByServiceId(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow, _
                                          ByVal inNowDate As Date) As IC3810203DataSet.IC3810203VisitUniqueKeyDataTable
            Try
                ''引数をログに出力
                Dim args As New List(Of String)
                ' DataRow内の項目を列挙
                Me.AddLogData(args, rowIN)
                ''開始ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

                Using query As New DBSelectQuery(Of IC3810203DataSet.IC3810203VisitUniqueKeyDataTable)("IC3810203_010")
                    ''SQLの設定
                    Dim sql As New StringBuilder
                    With sql
                        .AppendLine("SELECT /* IC3810203_010 */ ")
                        .AppendLine("       T1.VISITSEQ ")
                        .AppendLine("      ,T1.SACODE ")
                        .AppendLine("      ,T1.ASSIGNSTATUS ")
                        .AppendLine("  FROM ")
                        .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                        .AppendLine(" WHERE ")
                        .AppendLine("       T1.DLRCD = :DLRCD ")
                        .AppendLine("   AND T1.STRCD = :STRCD ")
                        .AppendLine("   AND T1.ASSIGNSTATUS <> :ASSIGNSTATUS_4 ")
                        .AppendLine("   AND TRIM(T1.DMSID) IS NULL ")
                        .AppendLine("   AND TRIM(T1.ORDERNO) IS NULL ")
                        .AppendLine("   AND T1.FREZID = :SVCIN_ID")
                        .AppendLine("   AND T1.VISITTIMESTAMP >= TRUNC(:NOWDATE) ")
                        .AppendLine(" ORDER BY T1.CREATEDATE ")
                    End With

                    query.CommandText = sql.ToString()

                    ''パラメータの設定
                    query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, rowIN.DLRCD)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, rowIN.STRCD)
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_4", OracleDbType.NVarchar2, AssignOutStore)
                    query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.NVarchar2, rowIN.SVCIN_ID)


                    ''SQLの実行
                    Using dt As IC3810203DataSet.IC3810203VisitUniqueKeyDataTable = query.GetData()
                        ''終了ログの出力
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))
                        Return dt
                    End Using
                End Using
            Finally

            End Try
        End Function

        ''' <summary>
        ''' サービス来店者管理テーブルのユニークキー情報取得（車両）
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <returns>サービス来店者ユニーキー情報</returns>
        ''' <remarks></remarks>

        Public Function GetVisitUniqueKeyByVehcle(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow, _
                                          ByVal inNowDate As Date) As IC3810203DataSet.IC3810203VisitUniqueKeyDataTable
            Try
                ''引数をログに出力
                Dim args As New List(Of String)
                ' DataRow内の項目を列挙
                Me.AddLogData(args, rowIN)
                ''開始ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

                Using query As New DBSelectQuery(Of IC3810203DataSet.IC3810203VisitUniqueKeyDataTable)("IC3810203_011")
                    ''SQLの設定
                    Dim sql As New StringBuilder
                    With sql
                        .AppendLine("SELECT /* IC3810203_011 */ ")
                        .AppendLine("       T1.VISITSEQ ")
                        .AppendLine("      ,T1.SACODE ")
                        .AppendLine("      ,T1.ASSIGNSTATUS ")
                        .AppendLine("  FROM ")
                        .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                        .AppendLine(" WHERE ")
                        .AppendLine("       T1.DLRCD = :DLRCD ")
                        .AppendLine("   AND T1.STRCD = :STRCD ")
                        .AppendLine("   AND T1.ASSIGNSTATUS <> :ASSIGNSTATUS_4 ")
                        If Not (String.IsNullOrWhiteSpace(rowIN.VCLREGNO)) And Not(String.IsNullOrWhiteSpace(rowIN.VIN)) Then
                            .AppendLine("   AND (RTRIM(T1.VCLREGNO) = :VCLREGNO ")
                            .AppendLine("        OR RTRIM(T1.VIN) = :VIN) ")
                        Else If Not (String.IsNullOrWhiteSpace(rowIN.VCLREGNO)) Then
                            .AppendLine("   AND RTRIM(T1.VCLREGNO) = :VCLREGNO ")
                        Else If Not (String.IsNullOrWhiteSpace(rowIN.VIN)) Then
                            .AppendLine("   AND RTRIM(T1.VIN) = :VIN ")
                        End If
                        .AppendLine("   AND TRIM(T1.DMSID) IS NULL ")
                        .AppendLine("   AND TRIM(T1.ORDERNO) IS NULL ")
                        .AppendLine("   AND T1.VISITTIMESTAMP >= TRUNC(:NOWDATE) ")
                        .AppendLine(" ORDER BY T1.CREATEDATE ")
                    End With

                    query.CommandText = sql.ToString()

                    ''パラメータの設定
                    query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, rowIN.DLRCD)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, rowIN.STRCD)
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_4", OracleDbType.NVarchar2, AssignOutStore)
                    If Not (String.IsNullOrWhiteSpace(rowIN.VCLREGNO)) Then
                        query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, rowIN.VCLREGNO)
                    End If
                    If Not (String.IsNullOrWhiteSpace(rowIN.VIN)) Then
                        query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, rowIN.VIN)
                    End If

                    ''SQLの実行
                    Using dt As IC3810203DataSet.IC3810203VisitUniqueKeyDataTable = query.GetData()
                        ''終了ログの出力
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))
                        Return dt
                    End Using
                End Using
            Finally

            End Try
        End Function
        '2018/12/13 NSK 坂本　ウェルカムボードが同じお客様を2件表示する END

        ''' <summary>
        ''' 顧客登録結果反映(新規追加)
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <returns>来店実績連番</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' 2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ''' </history>
        Public Function InsertVisitCustomer(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow) As Long
            ''引数をログに出力
            Dim args As New List(Of String)
            ' DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            Dim visitseq As Long = 0
            Using query As New DBSelectQuery(Of DataTable)("IC3810203_101")
                ''SQLの設定
                Dim sqlNextVal As New StringBuilder
                sqlNextVal.AppendLine("SELECT /* IC3810203_101 */")
                sqlNextVal.AppendLine("       SEQ_SERVICE_VISIT_MANAGEMENT.NEXTVAL AS VISITSEQ")
                sqlNextVal.AppendLine("  FROM DUAL")
                query.CommandText = sqlNextVal.ToString()
                Using dt As DataTable = query.GetData()
                    visitseq = CType(dt.Rows(0)("VISITSEQ"), Long)
                End Using
            End Using
            ''SQLの設定
            Dim sqlInsert As New StringBuilder
            sqlInsert.AppendLine("INSERT /* IC3810203_102 */")
            sqlInsert.AppendLine("  INTO TBL_SERVICE_VISIT_MANAGEMENT (")
            sqlInsert.AppendLine("       VISITSEQ")
            sqlInsert.AppendLine("     , DLRCD")
            sqlInsert.AppendLine("     , STRCD")
            sqlInsert.AppendLine("     , VISITTIMESTAMP")
            sqlInsert.AppendLine("     , VCLREGNO")
            sqlInsert.AppendLine("     , CUSTSEGMENT")
            sqlInsert.AppendLine("     , CUSTID")
            sqlInsert.AppendLine("     , DMSID")
            '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            sqlInsert.AppendLine("     , VCL_ID")
            '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
            sqlInsert.AppendLine("     , VIN")
            sqlInsert.AppendLine("     , MODELCODE")
            sqlInsert.AppendLine("     , NAME")
            sqlInsert.AppendLine("     , TELNO")
            sqlInsert.AppendLine("     , MOBILE")
            sqlInsert.AppendLine("     , SACODE")
            sqlInsert.AppendLine("     , ASSIGNTIMESTAMP")
            If Not (rowIN.IsSVCIN_IDNull) AndAlso rowIN.SVCIN_ID > 0 Then
                sqlInsert.AppendLine("     , REZID")
                sqlInsert.AppendLine("     , FREZID")
            End If
            If Not rowIN.IsORDERNONull Then
                sqlInsert.AppendLine("     , ORDERNO")
            End If
            sqlInsert.AppendLine("     , ASSIGNSTATUS")
            sqlInsert.AppendLine("     , REGISTKIND")
            sqlInsert.AppendLine("     , CREATEDATE")
            sqlInsert.AppendLine("     , UPDATEDATE")
            sqlInsert.AppendLine("     , CREATEACCOUNT")
            sqlInsert.AppendLine("     , UPDATEACCOUNT")
            sqlInsert.AppendLine("     , CREATEID")
            sqlInsert.AppendLine("     , UPDATEID")
            '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 START
            sqlInsert.AppendLine("     , CALLSTATUS")
            '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 END
            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
            sqlInsert.AppendLine("     , VISITNAME")
            sqlInsert.AppendLine("     , VISITTELNO")
            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
            sqlInsert.AppendLine(") ")
            sqlInsert.AppendLine("VALUES (")
            sqlInsert.AppendLine("       :VISITSEQ")
            sqlInsert.AppendLine("     , :DLRCD")
            sqlInsert.AppendLine("     , :STRCD")
            sqlInsert.AppendLine("     , :VISITTIMESTAMP")
            sqlInsert.AppendLine("     , :VCLREGNO")
            sqlInsert.AppendLine("     , :CUSTSEGMENT")
            sqlInsert.AppendLine("     , :CUSTID")
            sqlInsert.AppendLine("     , :DMSID")
            '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
            sqlInsert.AppendLine("     , :VCL_ID")
            '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
            sqlInsert.AppendLine("     , :VIN")
            sqlInsert.AppendLine("     , :MODELCODE")
            sqlInsert.AppendLine("     , :NAME")
            sqlInsert.AppendLine("     , :TELNO")
            sqlInsert.AppendLine("     , :MOBILE")
            sqlInsert.AppendLine("     , :SACODE")
            sqlInsert.AppendLine("     , :ASSIGNTIMESTAMP")
            If Not (rowIN.IsSVCIN_IDNull) AndAlso rowIN.SVCIN_ID > 0 Then
                sqlInsert.AppendLine("     , :REZID")
                sqlInsert.AppendLine("     , :REZID")
            End If
            If Not rowIN.IsORDERNONull Then
                sqlInsert.AppendLine("     , :ORDERNO")
            End If
            sqlInsert.AppendLine("     , :ASSIGNSTATUS")
            sqlInsert.AppendLine("     , :REGISTKIND")
            sqlInsert.AppendLine("     , SYSDATE")
            sqlInsert.AppendLine("     , SYSDATE")
            sqlInsert.AppendLine("     , :CREATEACCOUNT")
            sqlInsert.AppendLine("     , :UPDATEACCOUNT")
            sqlInsert.AppendLine("     , :CREATEID")
            sqlInsert.AppendLine("     , :UPDATEID")
            '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 START
            sqlInsert.AppendLine("     , :CALLSTATUS")
            '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 END
            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
            sqlInsert.AppendLine("     , :VISITNAME")
            sqlInsert.AppendLine("     , :VISITTELNO")
            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
            sqlInsert.AppendLine(")")
            Using query As New DBUpdateQuery("IC3810203_102")
                query.CommandText = sqlInsert.ToString()

                ''システム日時の取得
                Dim sysDate As Date = Toyota.eCRB.SystemFrameworks.Core.DateTimeFunc.Now(rowIN.DLRCD)

                ''パラメータの設定
                ''来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, visitseq)
                ''販売店コード
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, rowIN.DLRCD)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''店舗コード
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, rowIN.STRCD)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''来店日時
                query.AddParameterWithTypeValue("VISITTIMESTAMP", OracleDbType.Date, sysDate)
                ''車両登録No
                If (rowIN.IsVCLREGNONull = True) Then
                    query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, rowIN.VCLREGNO)
                End If
                ''顧客区分 1:自社客
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, "1")
                query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.NVarchar2, "1")
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                '顧客コード
                If (rowIN.IsCUSTOMERCODENull = True) Then
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, DBNull.Value)
                    query.AddParameterWithTypeValue("CUSTID", OracleDbType.Long, DBNull.Value)
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                Else
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, rowIN.CUSTOMERCODE)
                    query.AddParameterWithTypeValue("CUSTID", OracleDbType.Long, CType(rowIN.CUSTOMERCODE, Long))
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                End If
                ''基幹顧客ID
                If (rowIN.IsDMSIDNull = True) Then
                    query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, rowIN.DMSID)
                End If
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                ''車両ID
                If (rowIN.IsVCL_IDNull = True) Then
                    query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Long, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Long, rowIN.VCL_ID)
                End If
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''VIN
                If (rowIN.IsVINNull = True) Then
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, DBNull.Value)
                    query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, DBNull.Value)
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                Else
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowIN.VIN)
                    query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, rowIN.VIN)
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                End If
                ''モデルコード
                If (rowIN.IsMODELCODENull = True) Then
                    query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, rowIN.MODELCODE)
                End If
                ''モデルコード
                If (rowIN.IsCUSTOMERNAMENull = True) Then
                    query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, rowIN.CUSTOMERNAME)
                End If
                ''電話番号
                If (rowIN.IsTELNONull = True) Then
                    query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, rowIN.TELNO)
                End If
                ''携帯番号
                If (rowIN.IsMOBILENull = True) Then
                    query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, rowIN.MOBILE)
                End If
                ''振当SA
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, rowIN.SACODE)
                query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, rowIN.SACODE)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''SA割当日時
                query.AddParameterWithTypeValue("ASSIGNTIMESTAMP", OracleDbType.Date, sysDate)
                ''予約ID
                If Not (rowIN.IsSVCIN_IDNull) AndAlso rowIN.SVCIN_ID > 0 Then
                    '更新： 2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発　START
                    'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, rowIN.SVCIN_ID)
                    query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, rowIN.SVCIN_ID)
                    '更新： 2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発　END
                End If
                '整備受注No
                If Not rowIN.IsORDERNONull Then
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)
                    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.NVarchar2, rowIN.ORDERNO)
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                End If
                '振当ステータス
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.Char, AssignFinished)
                query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, AssignFinished)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''登録区分 1:SAが登録
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("REGISTKIND", OracleDbType.Char, "1")
                query.AddParameterWithTypeValue("REGISTKIND", OracleDbType.NVarchar2, "1")
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''作成日
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
                query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.NVarchar2, rowIN.ACCOUNT)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''更新日
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, rowIN.ACCOUNT)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''作成機能ID
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, rowIN.SYSTEM)
                query.AddParameterWithTypeValue("CREATEID", OracleDbType.NVarchar2, rowIN.SYSTEM)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''更新機能ID
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, rowIN.SYSTEM)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, rowIN.SYSTEM)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                ''SQLの実行
                '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 START
                query.AddParameterWithTypeValue("CALLSTATUS", OracleDbType.NVarchar2, CALLSTATUS_2)
                '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 END
                '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                '来店者氏名
                If (rowIN.IsVISITNAMENull = True) Then
                    query.AddParameterWithTypeValue("VISITNAME", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("VISITNAME", OracleDbType.NVarchar2, rowIN.VISITNAME)
                End If
                '来店者電話番号
                If (rowIN.IsVISITTELNONull = True) Then
                    query.AddParameterWithTypeValue("VISITTELNO", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("VISITTELNO", OracleDbType.NVarchar2, rowIN.VISITTELNO)
                End If
                '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                query.Execute()
            End Using
            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT:VISITSEQ = {2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , visitseq))
            Return visitseq
        End Function

        ''' <summary>
        ''' 顧客登録結果反映(修正更新)
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' 2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ''' </history>
        Public Function UpdateVisitCustomer(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow) As Long
            ''引数をログに出力
            Dim args As New List(Of String)
            ' DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            Using query As New DBUpdateQuery("IC3810203_103")
                ''SQLの設定
                Dim sql As New StringBuilder
                sql.AppendLine("UPDATE /* IC3810203_103 */")
                sql.AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                sql.AppendLine("   SET VCLREGNO = :VCLREGNO")
                sql.AppendLine("     , CUSTSEGMENT = :CUSTSEGMENT")
                sql.AppendLine("     , VIN = :VIN")
                sql.AppendLine("     , DMSID = :DMSID")
                sql.AppendLine("     , MODELCODE = :MODELCODE")
                sql.AppendLine("     , TELNO = :TELNO")
                sql.AppendLine("     , MOBILE = :MOBILE")
                sql.AppendLine("     , NAME = :NAME")
                If Not (rowIN.IsSVCIN_IDNull) AndAlso rowIN.SVCIN_ID > 0 Then
                    sql.AppendLine("     , REZID = :REZID")
                    sql.AppendLine("     , FREZID = :REZID")
                End If
                If Not rowIN.IsORDERNONull AndAlso Not String.IsNullOrEmpty(rowIN.ORDERNO) Then
                    sql.AppendLine("     , ORDERNO = :ORDERNO")
                End If
                sql.AppendLine("     , UPDATEDATE = SYSDATE")
                sql.AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")
                sql.AppendLine("     , UPDATEID = :UPDATEID")
                '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                sql.AppendLine("     , VISITNAME = :VISITNAME")
                sql.AppendLine("     , VISITTELNO = :VISITTELNO")
                '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
                sql.AppendLine("   AND DLRCD = :DLRCD")
                sql.AppendLine("   AND STRCD = :STRCD")
                query.CommandText = sql.ToString()

                ''パラメータの設定
                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, rowIN.VCLREGNO)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, "1")
                query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.NVarchar2, "1")
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                'VIN
                If (rowIN.IsVINNull = True) Then
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, DBNull.Value)
                    query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, DBNull.Value)
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                Else
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowIN.VIN)
                    query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, rowIN.VIN)
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                End If
                '基幹顧客ID
                If (rowIN.IsDMSIDNull = True) Then
                    query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, rowIN.DMSID)
                End If
                'モデルコード
                If (rowIN.IsMODELCODENull = True) Then
                    query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, rowIN.MODELCODE)
                End If
                '電話番号
                If (rowIN.IsTELNONull = True) Then
                    query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, rowIN.TELNO)
                End If
                '携帯電話番号
                If (rowIN.IsMOBILENull = True) Then
                    query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, rowIN.MOBILE)
                End If
                '顧客コード
                If (rowIN.IsCUSTOMERNAMENull = True) Then
                    query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, rowIN.CUSTOMERNAME)
                End If
                ''予約ID
                If Not (rowIN.IsSVCIN_IDNull) AndAlso rowIN.SVCIN_ID > 0 Then
                    '更新： 2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発　START
                    'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, rowIN.SVCIN_ID)
                    query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, rowIN.SVCIN_ID)
                    '更新： 2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発　END
                End If
                '整備受注No
                If Not rowIN.IsORDERNONull AndAlso Not String.IsNullOrEmpty(rowIN.ORDERNO) Then
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)
                    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.NVarchar2, rowIN.ORDERNO)
                    '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                End If
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, rowIN.SYSTEM)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, rowIN.ACCOUNT)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, rowIN.SYSTEM)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                '来店者氏名
                If (rowIN.IsVISITNAMENull = True) Then
                    query.AddParameterWithTypeValue("VISITNAME", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("VISITNAME", OracleDbType.NVarchar2, rowIN.VISITNAME)
                End If
                '来店者電話番号
                If (rowIN.IsVISITTELNONull = True) Then
                    query.AddParameterWithTypeValue("VISITTELNO", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("VISITTELNO", OracleDbType.NVarchar2, rowIN.VISITTELNO)
                End If
                '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, rowIN.DLRCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, rowIN.STRCD)
                '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                ''SQLの実行
                Dim ret As Integer = query.Execute()
                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , ret))
                Return ret
            End Using
        End Function

        '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        ' ''' <summary>
        ' ''' ストール予約情報の顧客取得(予約IDあり)
        ' ''' </summary>
        ' ''' <param name="rowIN">顧客登録結果反引数</param>
        ' ''' <param name="nowDate">現在日時</param>
        ' ''' <returns>ストール予約情報</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' </history>
        'Public Function GetStallReseveInfo(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow, _
        '                                   ByVal nowDate As DateTime) As IC3810203DataSet.IC3810203StallReserveInfoDataTable
        '    Try
        '        ''引数をログに出力
        '        Dim args As New List(Of String)
        '        ' DataRow内の項目を列挙
        '        Me.AddLogData(args, rowIN)
        '        ''開始ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} IN:{2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , String.Join(", ", args.ToArray())))

        '        Using query As New DBSelectQuery(Of IC3810203DataSet.IC3810203StallReserveInfoDataTable)("IC3810203_003")
        '            ''SQLの設定
        '            Dim sql As New StringBuilder
        '            sql.Append("SELECT /* IC3810203_003 */ ")
        '            sql.Append("       T1.REZID, ")
        '            sql.Append("       T1.ORDERNO, ")
        '            sql.Append("       T1.STARTTIME, ")
        '            sql.Append("       T1.ACCOUNT_PLAN, ")
        '            sql.Append("       T6.ORIGINALID ")
        '            sql.Append("  FROM TBL_STALLREZINFO T1, ")
        '            sql.Append("       ( ")
        '            sql.Append("       SELECT ")
        '            sql.Append("              T2.DLRCD, ")
        '            sql.Append("              T2.STRCD, ")
        '            sql.Append("              T2.INSDID, ")
        '            sql.Append("              T2.VCLREGNO, ")
        '            sql.Append("              T2.VIN ")
        '            sql.Append("         FROM TBL_STALLREZINFO T2 ")
        '            sql.Append("        WHERE T2.DLRCD = :DLRCD ")
        '            sql.Append("          AND T2.STRCD = :STRCD ")
        '            sql.Append("          AND T2.REZID = :REZID ")
        '            sql.Append("       ) T3, ")
        '            sql.Append("       TBLORG_CUSTOMER T6 ")
        '            sql.Append(" WHERE T1.DLRCD = T3.DLRCD ")
        '            sql.Append("   AND T1.STRCD = T3.STRCD ")
        '            sql.Append("   AND T1.INSDID = T3.INSDID ")
        '            sql.Append("   AND (T1.VCLREGNO = T3.VCLREGNO OR T1.VIN = T3.VIN) ")
        '            sql.Append("   AND T1.DLRCD = T6.DLRCD(+) ")
        '            sql.Append("   AND T1.STRCD = T6.STRCD(+) ")
        '            sql.Append("   AND T1.CUSTCD = T6.CUSTCD(+) ")
        '            sql.Append("   AND T1.STARTTIME >= TRUNC(:STARTTIME) ")
        '            sql.Append("   AND T1.DELIVERY_FLG = '0' ")
        '            sql.Append("   AND NOT EXISTS ( ")
        '            sql.Append("       SELECT 1 ")
        '            sql.Append("         FROM TBL_STALLREZINFO T4 ")
        '            sql.Append("        WHERE T4.DLRCD = T1.DLRCD ")
        '            sql.Append("          AND T4.STRCD = T1.STRCD ")
        '            sql.Append("          AND T4.REZID = T1.REZID ")
        '            sql.Append("          AND T4.STOPFLG = '0' ")
        '            sql.Append("          AND T4.CANCELFLG = '1' ")
        '            sql.Append("       ) ")

        '            query.CommandText = sql.ToString()

        '            ''パラメータの設定
        '            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
        '            query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, rowIN.REZID)
        '            query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, nowDate)

        '            ''SQLの実行
        '            Using dt As IC3810203DataSet.IC3810203StallReserveInfoDataTable = query.GetData()
        '                ''終了ログの出力
        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                    , Me.GetType.ToString _
        '                    , MethodBase.GetCurrentMethod.Name _
        '                    , dt.Rows.Count))
        '                Return dt
        '            End Using
        '        End Using
        '    Finally

        '    End Try
        'End Function

        ' ''' <summary>
        ' ''' ストール予約情報の顧客取得(予約IDなし)
        ' ''' </summary>
        ' ''' <param name="rowIN">顧客登録結果反引数</param>
        ' ''' <param name="nowDate">現在日時</param>
        ' ''' <returns>ストール予約情報</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' </history>
        'Public Function GetStallReseveInfoNotReserveId(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow, _
        '                                               ByVal nowDate As DateTime) As IC3810203DataSet.IC3810203StallReserveInfoDataTable
        '    Try
        '        ''引数をログに出力
        '        Dim args As New List(Of String)
        '        ' DataRow内の項目を列挙
        '        Me.AddLogData(args, rowIN)
        '        ''開始ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} IN:{2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , String.Join(", ", args.ToArray())))

        '        Using query As New DBSelectQuery(Of IC3810203DataSet.IC3810203StallReserveInfoDataTable)("IC3810203_004")
        '            ''SQLの設定
        '            Dim sql As New StringBuilder
        '            sql.Append("SELECT /* IC3810203_004 */ ")
        '            sql.Append("       T1.REZID, ")
        '            sql.Append("       T1.ORDERNO, ")
        '            sql.Append("       T1.STARTTIME, ")
        '            sql.Append("       T1.ACCOUNT_PLAN, ")
        '            sql.Append("       T6.ORIGINALID ")
        '            sql.Append("  FROM TBL_STALLREZINFO T1, ")
        '            sql.Append("       TBLORG_CUSTOMER T6 ")
        '            sql.Append(" WHERE T1.DLRCD = T6.DLRCD(+) ")
        '            sql.Append("   AND T1.STRCD = T6.STRCD(+) ")
        '            sql.Append("   AND T1.DLRCD = :DLRCD ")
        '            sql.Append("   AND T1.STRCD = :STRCD ")
        '            sql.Append("   AND T1.CUSTCD = :CUSTCD ")
        '            sql.Append("   AND (T1.VCLREGNO = :VCLREGNO OR T1.VIN = :VIN) ")
        '            sql.Append("   AND T1.CUSTCD = T6.CUSTCD(+) ")
        '            sql.Append("   AND T1.STARTTIME >= TRUNC(:STARTTIME) ")
        '            sql.Append("   AND T1.DELIVERY_FLG = '0' ")
        '            sql.Append("   AND NOT EXISTS ( ")
        '            sql.Append("       SELECT 1 ")
        '            sql.Append("         FROM TBL_STALLREZINFO T4 ")
        '            sql.Append("        WHERE T4.DLRCD = T1.DLRCD ")
        '            sql.Append("          AND T4.STRCD = T1.STRCD ")
        '            sql.Append("          AND T4.REZID = T1.REZID ")
        '            sql.Append("          AND T4.STOPFLG = '0' ")
        '            sql.Append("          AND T4.CANCELFLG = '1' ")
        '            sql.Append("       ) ")

        '            query.CommandText = sql.ToString()

        '            ''パラメータの設定
        '            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
        '            query.AddParameterWithTypeValue("CUSTCD", OracleDbType.Char, rowIN.DMSID)
        '            query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.Char, rowIN.VCLREGNO)
        '            query.AddParameterWithTypeValue("VIN", OracleDbType.Char, rowIN.VIN)
        '            query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, nowDate)

        '            ''SQLの実行
        '            Using dt As IC3810203DataSet.IC3810203StallReserveInfoDataTable = query.GetData()
        '                ''終了ログの出力
        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                    , Me.GetType.ToString _
        '                    , MethodBase.GetCurrentMethod.Name _
        '                    , dt.Rows.Count))
        '                Return dt
        '            End Using
        '        End Using
        '    Finally

        '    End Try
        'End Function

        ' ''' <summary>
        ' ''' ストール予約更新
        ' ''' </summary>
        ' ''' <param name="rowIN">更新情報</param>
        ' ''' <param name="drStallReserveInfo">ストール予約情報</param>
        ' ''' <param name="nowDate">現在日時</param>
        ' ''' <returns>更新数</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' </history>
        'Public Function UpdateDBStallOrder(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow, _
        '                                   ByVal drStallReserveInfo As IC3810203DataSet.IC3810203StallReserveInfoRow, _
        '                                   ByVal nowDate As Date) As Integer
        '    ''引数をログに出力
        '    Dim args As New List(Of String)
        '    ' DataRow内の項目を列挙
        '    Me.AddLogData(args, rowIN)
        '    ''開始ログの出力
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '        , "{0}.{1} IN:{2}" _
        '        , Me.GetType.ToString _
        '        , MethodBase.GetCurrentMethod.Name _
        '        , String.Join(", ", args.ToArray())))

        '    'DBSelectQueryインスタンス生成
        '    Using query As New DBUpdateQuery("IC3810203_104")
        '        'SQL組み立て
        '        Dim sql As New StringBuilder
        '        With sql
        '            .Append("UPDATE /* IC3810203_104 */ ")
        '            .Append("       TBL_STALLREZINFO T1 ")
        '            .Append("   SET T1.CUSTCD = :CUSTCD ")                          '顧客コード
        '            .Append("     , T1.CUSTOMERNAME = NVL(:CUSTOMERNAME, ' ') ")    '氏名
        '            .Append("     , T1.TELNO = NVL(:TELNO, ' ') ")                  '電話番号
        '            .Append("     , T1.MOBILE = NVL(:MOBILE, ' ') ")                '携帯番号
        '            .Append("     , T1.EMAIL1 = :EMAIL1 ")                          'E-MAILアドレス1
        '            .Append("     , T1.VEHICLENAME = NVL(:VEHICLENAME, ' ') ")      '車名
        '            .Append("     , T1.VCLREGNO = NVL(:VCLREGNO, ' ') ")            '登録ナンバー
        '            .Append("     , T1.VIN = NVL(:VIN, ' ') ")                      'VIN
        '            .Append("     , T1.CUSTOMERFLAG = '0' ")                        '識別フラグ
        '            .Append("     , T1.MODELCODE = :MODELCODE ")                    'モデルコード
        '            .Append("     , T1.UPDATE_COUNT = T1.UPDATE_COUNT + 1 ")        '更新カウント
        '            .Append("     , T1.UPDATEDATE = :NOWDATE ")                     '更新日
        '            .Append("     , T1.UPDATEACCOUNT = :UPDATEACCOUNT ")            '更新ユーザーアカウント
        '            .Append("     , T1.CUSTOMERCLASS = '1' ")                       '顧客分類
        '            If Not drStallReserveInfo.IsORIGINALIDNull Then
        '                .Append("     , T1.INSDID = NVL(:INSDID, ' ') ")            '内部管理ID
        '                .Append("     , T1.CRCUSTID = NVL(:CRCUSTID, ' ') ")        '活動先顧客コード
        '            End If
        '            .Append(" WHERE ")
        '            .Append("       T1.DLRCD = :DLRCD ")
        '            .Append("   AND T1.STRCD = :STRCD ")
        '            .Append("   AND T1.REZID = :REZID ")
        '        End With
        '        query.CommandText = sql.ToString()
        '        'SQLパラメータ設定
        '        '顧客コード
        '        query.AddParameterWithTypeValue("CUSTCD", OracleDbType.Char, rowIN.DMSID)
        '        '氏名
        '        query.AddParameterWithTypeValue("CUSTOMERNAME", OracleDbType.Char, rowIN.CUSTOMERNAME)
        '        '電話番号
        '        query.AddParameterWithTypeValue("TELNO", OracleDbType.Char, rowIN.TELNO)
        '        '携帯番号
        '        query.AddParameterWithTypeValue("MOBILE", OracleDbType.Char, rowIN.MOBILE)
        '        'E-MAILアドレス1
        '        query.AddParameterWithTypeValue("EMAIL1", OracleDbType.Char, rowIN.EMAIL1)
        '        '車名
        '        query.AddParameterWithTypeValue("VEHICLENAME", OracleDbType.Char, rowIN.VEHICLENAME)
        '        '登録ナンバー
        '        query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.Char, rowIN.VCLREGNO)
        '        'VIN
        '        query.AddParameterWithTypeValue("VIN", OracleDbType.Char, rowIN.VIN)
        '        'モデルコード
        '        query.AddParameterWithTypeValue("MODELCODE", OracleDbType.Char, rowIN.MODELCODE)
        '        '更新日
        '        query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, nowDate)
        '        '更新ユーザーアカウント
        '        query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Char, rowIN.ACCOUNT)

        '        If Not drStallReserveInfo.IsORIGINALIDNull Then
        '            '内部管理ID
        '            query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, drStallReserveInfo.ORIGINALID)
        '            '活動先顧客コード
        '            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, drStallReserveInfo.ORIGINALID)
        '        End If

        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
        '        query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, drStallReserveInfo.REZID)

        '        ''SQLの実行
        '        Dim ret As Integer = query.Execute()
        '        ''終了ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ret))
        '        Return ret
        '    End Using
        'End Function

        ' ''' <summary>
        ' ''' ストール予約情報取得
        ' ''' </summary>
        ' ''' <param name="rowIN">顧客登録結果反引数</param>
        ' ''' <returns>ストール予約情報</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' </history>
        'Public Function GetStallReseveIdInfo(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow) As IC3810203DataSet.IC3810203StallReserveInfoDataTable
        '    Try
        '        ''引数をログに出力
        '        Dim args As New List(Of String)
        '        ' DataRow内の項目を列挙
        '        Me.AddLogData(args, rowIN)
        '        ''開始ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} IN:{2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , String.Join(", ", args.ToArray())))

        '        Using query As New DBSelectQuery(Of IC3810203DataSet.IC3810203StallReserveInfoDataTable)("IC3810203_005")
        '            ''SQLの設定
        '            Dim sql As New StringBuilder
        '            sql.Append("SELECT /* IC3810203_005 */ ")
        '            sql.Append("       T1.REZID ")
        '            sql.Append("     , T1.ORDERNO ")
        '            sql.Append("  FROM TBL_STALLREZINFO T1 ")
        '            sql.Append(" WHERE T1.DLRCD = :DLRCD ")
        '            sql.Append("   AND T1.STRCD = :STRCD ")
        '            sql.Append("   AND T1.REZID = :REZID ")
        '            sql.Append("   AND NOT EXISTS ( ")
        '            sql.Append("       SELECT 1 ")
        '            sql.Append("         FROM TBL_STALLREZINFO T2 ")
        '            sql.Append("        WHERE T2.DLRCD = T1.DLRCD ")
        '            sql.Append("          AND T2.STRCD = T1.STRCD ")
        '            sql.Append("          AND T2.REZID = T1.REZID ")
        '            sql.Append("          AND T2.STOPFLG = '0' ")
        '            sql.Append("          AND T2.CANCELFLG = '1' ")
        '            sql.Append("       ) ")

        '            query.CommandText = sql.ToString()

        '            ''パラメータの設定
        '            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
        '            query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, rowIN.REZID)

        '            ''SQLの実行
        '            Using dt As IC3810203DataSet.IC3810203StallReserveInfoDataTable = query.GetData()
        '                ''終了ログの出力
        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                    , Me.GetType.ToString _
        '                    , MethodBase.GetCurrentMethod.Name _
        '                    , dt.Rows.Count))
        '                Return dt
        '            End Using
        '        End Using
        '    Finally

        '    End Try
        'End Function

        ''' <summary>
        ''' サービス入庫追加情報取得
        ''' </summary>
        ''' <param name="inCustomerId">顧客ID</param>
        ''' <param name="inVehicleId">車両ID</param>
        ''' <returns>サービス入庫追加情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        Public Function GetServiceInAppendData(ByVal inCustomerId As Decimal, _
                                               ByVal inVehicleId As Decimal) As IC3810203DataSet.IC3810203ServiceInAppendDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Using query As New DBSelectQuery(Of IC3810203DataSet.IC3810203ServiceInAppendDataTable)("IC3810203_007")
                'SQLの設定
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("SELECT /* IC3810203_007 */ ")
                    .AppendLine("       DMS_CST_CD AS DMSID ")
                    .AppendLine("      ,VCL_VIN AS VIN ")
                    .AppendLine("  FROM ")
                    .AppendLine("       TBL_SERVICEIN_APPEND ")
                    .AppendLine(" WHERE ")
                    .AppendLine("       CST_ID = :CST_ID ")
                    .AppendLine("   AND VCL_ID = :VCL_ID ")
                End With
                query.CommandText = sql.ToString()

                'パラメータの設定
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Long, inCustomerId)
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Long, inVehicleId)

                'SQLの実行
                Using dt As IC3810203DataSet.IC3810203ServiceInAppendDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using
        End Function

        ''' <summary>
        ''' サービス入庫追加情報更新
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        Public Function UpdateServiceInAppend(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow, _
                                              ByVal inNowDate As Date) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Using query As New DBUpdateQuery("IC3810203_105")
                ''SQLの設定
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("UPDATE /* IC3810203_105 */ ")
                    .AppendLine("       TBL_SERVICEIN_APPEND ")
                    .AppendLine("   SET DMS_CST_CD = :DMS_CST_CD ")
                    .AppendLine("      ,VCL_VIN = :VCL_VIN ")
                    .AppendLine("      ,ROW_UPDATE_DATETIME = :NOWDATE ")
                    .AppendLine("      ,ROW_UPDATE_ACCOUNT = :ACCOUNT ")
                    .AppendLine("      ,ROW_UPDATE_FUNCTION = :SYSTEM ")
                    .AppendLine("      ,ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .AppendLine(" WHERE CST_ID = :CST_ID ")
                    .AppendLine("   AND VCL_ID = :VCL_ID ")
                End With
                query.CommandText = sql.ToString()

                'パラメータの設定
                query.AddParameterWithTypeValue("DMS_CST_CD", OracleDbType.NVarchar2, rowIN.DMSID)
                query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, rowIN.VIN)
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, rowIN.ACCOUNT)
                query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, rowIN.SYSTEM)
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Long, CType(rowIN.CUSTOMERCODE, Long))
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Long, rowIN.VCL_ID)

                'SQLの実行
                Dim ret As Integer = query.Execute()

                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , ret))
                Return ret
            End Using
        End Function

        ''' <summary>
        ''' サービス入庫追加情報更新
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' </history>
        Public Function InsertServiceInAppend(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow, _
                                              ByVal inNowDate As Date) As Long
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Using query As New DBUpdateQuery("IC3810203_106")
                'SQLの設定
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("INSERT /* IC3810203_106 */ ")
                    .AppendLine("   INTO TBL_SERVICEIN_APPEND( ")
                    .AppendLine("        CST_ID ")
                    .AppendLine("       ,VCL_ID ")
                    .AppendLine("       ,DMS_CST_CD ")
                    .AppendLine("       ,VCL_VIN ")
                    .AppendLine("       ,ROW_CREATE_DATETIME ")
                    .AppendLine("       ,ROW_CREATE_ACCOUNT ")
                    .AppendLine("       ,ROW_CREATE_FUNCTION ")
                    .AppendLine("       ,ROW_UPDATE_DATETIME ")
                    .AppendLine("       ,ROW_UPDATE_ACCOUNT ")
                    .AppendLine("       ,ROW_UPDATE_FUNCTION ")
                    .AppendLine("       ,ROW_LOCK_VERSION ")
                    .AppendLine(" ) VALUES ( ")
                    .AppendLine("        :CST_ID ")
                    .AppendLine("       ,:VCL_ID ")
                    .AppendLine("       ,:DMS_CST_CD ")
                    .AppendLine("       ,:VCL_VIN ")
                    .AppendLine("       ,:NOWDATE ")
                    .AppendLine("       ,:ACCOUNT ")
                    .AppendLine("       ,:SYSTEM ")
                    .AppendLine("       ,:NOWDATE ")
                    .AppendLine("       ,:ACCOUNT ")
                    .AppendLine("       ,:SYSTEM ")
                    .AppendLine("       ,0 ")
                    .AppendLine(" ) ")
                End With
                query.CommandText = sql.ToString()

                'パラメータの設定
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Long, CType(rowIN.CUSTOMERCODE, Long))
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Long, rowIN.VCL_ID)
                query.AddParameterWithTypeValue("DMS_CST_CD", OracleDbType.NVarchar2, rowIN.DMSID)
                query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, rowIN.VIN)
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, rowIN.ACCOUNT)
                query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, rowIN.SYSTEM)

                'SQLの実行
                Dim ret As Integer = query.Execute()

                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , ret))
                Return ret
            End Using
        End Function

        ''' <summary>
        ''' サービス入庫情報取得
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <returns>ストール予約情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ''' </history>
        Public Function GetServiceInData(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow) As IC3810203DataSet.IC3810203ServiceInInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Using query As New DBSelectQuery(Of IC3810203DataSet.IC3810203ServiceInInfoDataTable)("IC3810203_008")
                'SQLの設定
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("SELECT /* IC3810203_008 */ ")
                    .AppendLine("       T1.SVCIN_ID ")
                    .AppendLine("      ,TRIM(T1.RO_NUM) AS RO_NUM ")
                    .AppendLine("      ,T1.ROW_LOCK_VERSION ")
                    .AppendLine("      ,T1.CST_ID ")
                    .AppendLine("      ,T1.VCL_ID ")
                    .AppendLine("      ,T2.CST_TYPE ")
                    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                    .AppendLine("      ,TRIM(T3.CST_NAME) AS CST_NAME ")
                    .AppendLine("      ,NVL( TRIM(T3.CST_PHONE), TRIM(T3.CST_MOBILE)) AS CST_TELNO ")
                    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                    .AppendLine("  FROM  ")
                    .AppendLine("       TB_T_SERVICEIN T1 ")
                    .AppendLine("      ,TB_M_CUSTOMER_DLR T2 ")
                    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                    .AppendLine("      ,TB_M_CUSTOMER T3")
                    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                    .AppendLine(" WHERE  ")
                    .AppendLine("       T1.SVCIN_ID = :SVCIN_ID ")
                    .AppendLine("   AND T1.DLR_CD = T2.DLR_CD ")
                    .AppendLine("   AND T1.CST_ID = T2.CST_ID ")
                    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                    .AppendLine("   AND T2.CST_ID = T3.CST_ID ")
                    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                    .AppendLine("   AND NOT EXISTS (SELECT 1 ")
                    .AppendLine("                     FROM TB_T_SERVICEIN T2 ")
                    .AppendLine("                    WHERE T2.SVCIN_ID = T1.SVCIN_ID ")
                    .AppendLine("                      AND T2.SVC_STATUS = :SVC_STATUS_02) ")
                End With

                query.CommandText = sql.ToString()

                'パラメータの設定
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, rowIN.SVCIN_ID)
                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)

                ''SQLの実行
                Using dt As IC3810203DataSet.IC3810203ServiceInInfoDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using
        End Function

        '2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 START

        ''' <summary>
        ''' 顧客情報取得
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <returns>顧客情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/07/08 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ''' 2014/02/24 TMEJ 小澤 店舗展開のユーザテスト不具合 緊急対応
        ''' </history>
        Public Function GetCustomerInfo(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow) As IC3810203DataSet.IC3810203CustomerInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Using query As New DBSelectQuery(Of IC3810203DataSet.IC3810203CustomerInfoDataTable)("IC3810203_009")
                'SQLの設定
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("SELECT /* IC3810203_009 */ ")
                    .AppendLine("       T1.CST_ID ")
                    .AppendLine("      ,T1.VCL_ID ")
                    .AppendLine("  FROM ")
                    .AppendLine("       TB_M_CUSTOMER_VCL T1 ")
                    .AppendLine("      ,TB_M_VEHICLE T2 ")
                    .AppendLine("      ,TB_M_VEHICLE_DLR T3 ")
                    .AppendLine("      ,TB_M_CUSTOMER_DLR T4 ")
                    .AppendLine("      ,TB_M_CUSTOMER T5 ")
                    .AppendLine(" WHERE ")
                    .AppendLine("       T1.VCL_ID = T2.VCL_ID ")
                    .AppendLine("   AND T1.DLR_CD = T3.DLR_CD ")
                    .AppendLine("   AND T2.VCL_ID = T3.VCL_ID ")
                    .AppendLine("   AND T1.DLR_CD = T4.DLR_CD ")
                    .AppendLine("   AND T1.CST_ID = T4.CST_ID ")
                    .AppendLine("   AND T1.CST_ID = T5.CST_ID ")
                    .AppendLine("   AND T1.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND T1.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")

                    '2014/02/24 TMEJ 小澤 店舗展開のユーザテスト不具合 緊急対応 START
                    '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 START
                    '.AppendLine("   AND (T5.DMS_CST_CD = :DMS_CST_CD OR T5.DMS_CST_CD = :SPACE_1) ")
                    .AppendLine("   AND (T5.DMS_CST_CD = :DMS_CST_CD OR T5.DMS_CST_CD = N' ') ")
                    '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 END
                    '2014/02/24 TMEJ 小澤 店舗展開のユーザテスト不具合 緊急対応 END

                    .AppendLine("   AND ((T2.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) AND T3.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH)) OR ")
                    '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 START
                    '.AppendLine("        (T2.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) AND T3.REG_NUM_SEARCH = :SPACE_1) OR ")
                    '.AppendLine("        (T2.VCL_VIN_SEARCH = :SPACE_1 AND T3.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH))) ")
                    .AppendLine("        (T2.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) AND T3.REG_NUM_SEARCH = N' ') OR ")
                    .AppendLine("        (T2.VCL_VIN_SEARCH = N' ' AND T3.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH))) ")
                    '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 END

                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 STRAT
                    '.AppendLine(" ORDER BY T5.DMS_TAKEIN_DATETIME DESC ")
                    .AppendLine(" ORDER BY T3.DMS_TAKEIN_DATETIME DESC ")
                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                    .AppendLine("         ,T4.CST_TYPE ASC ")

                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 STRAT
                    .AppendLine("         ,T1.CST_VCL_TYPE ASC ")
                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                    .AppendLine("         ,T3.REG_NUM DESC ")
                    .AppendLine("         ,T2.VCL_VIN DESC ")
                    .AppendLine("         ,T2.VCL_ID DESC ")
                End With

                query.CommandText = sql.ToString()

                'パラメータの設定
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, rowIN.DLRCD)
                query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, OwnerChangeTypeNone)
                query.AddParameterWithTypeValue("VCL_VIN_SEARCH", OracleDbType.NVarchar2, rowIN.VIN)
                query.AddParameterWithTypeValue("REG_NUM_SEARCH", OracleDbType.NVarchar2, rowIN.VCLREGNO)
                '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 START
                'query.AddParameterWithTypeValue("SPACE_1", OracleDbType.NVarchar2, Space(1))
                '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 END

                '2014/02/24 TMEJ 小澤 店舗展開のユーザテスト不具合 緊急対応 START
                query.AddParameterWithTypeValue("DMS_CST_CD", OracleDbType.NVarchar2, rowIN.DMSID)
                '2014/02/24 TMEJ 小澤 店舗展開のユーザテスト不具合 緊急対応 END

                ''SQLの実行
                Using dt As IC3810203DataSet.IC3810203CustomerInfoDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using
        End Function

        ''' <summary>
        ''' SA振当て処理
        ''' </summary>
        ''' <param name="rowIN">顧客登録結果反引数</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' 2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発
        ''' </history>
        Public Function UpdateAssginInfo(ByVal rowIN As IC3810203DataSet.IC3810203InCustomerSaveRow, _
                                         ByVal inNowDate As Date) As Long
            ''引数をログに出力
            Dim args As New List(Of String)
            ' DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            Using query As New DBUpdateQuery("IC3810203_107")
                ''SQLの設定
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("UPDATE /* IC3810203_107 */")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                    .AppendLine("   SET SACODE = :SACODE")
                    .AppendLine("      ,ASSIGNTIMESTAMP = :ASSIGNTIMESTAMP")
                    .AppendLine("      ,ASSIGNSTATUS = :ASSIGNSTATUS")
                    .AppendLine("      ,QUEUESTATUS = :QUEUESTATUS")
                    .AppendLine("      ,HOLDSTAFF = NULL")
                    .AppendLine("      ,UPDATEDATE = :PRESENTTIME")
                    .AppendLine("      ,UPDATEACCOUNT = :UPDATEACCOUNT")
                    .AppendLine("      ,UPDATEID = :UPDATEID")
                    '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 START
                    .AppendLine("      ,CALLSTATUS = :CALLSTATUS")
                    '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 END
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ")
                End With

                query.CommandText = sql.ToString()

                ''パラメータの設定
                query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, rowIN.SACODE)
                query.AddParameterWithTypeValue("ASSIGNTIMESTAMP", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, AssignFinished)
                query.AddParameterWithTypeValue("QUEUESTATUS", OracleDbType.NVarchar2, QueueStatusNotWait)
                query.AddParameterWithTypeValue("PRESENTTIME", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, rowIN.ACCOUNT)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, rowIN.SYSTEM)
                '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 START
                query.AddParameterWithTypeValue("CALLSTATUS", OracleDbType.NVarchar2, CALLSTATUS_2)
                '2014/02/24 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発 END
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, rowIN.VISITSEQ)

                ''SQLの実行
                Dim ret As Integer = query.Execute()
                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , ret))
                Return ret
            End Using
        End Function

        '2013/08/08 TMEJ 小澤 次世代e-CRB SAサービス受付機能開発 END

        ''' <summary>
        ''' DataRow内の項目を列挙(ログ出力用)
        ''' </summary>
        ''' <param name="args">ログ項目のコレクション</param>
        ''' <param name="row">対象となるDataRow</param>
        ''' <remarks></remarks>
        Private Sub AddLogData(ByVal args As List(Of String), ByVal row As DataRow)
            For Each column As DataColumn In row.Table.Columns
                If row.IsNull(column.ColumnName) = True Then
                    args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, row(column.ColumnName)))
                End If
            Next
        End Sub

    End Class

End Namespace

Partial Class IC3810203DataSet
End Class
