'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3140103DataSet.vb
'─────────────────────────────────────
'機能： メインメニュー(SA) データアクセス
'補足： 
'作成： 2012/01/16 KN 小林
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Globalization

Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003.IC3801003DataSet
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003.IC3801003TableAdapter

Namespace SC3140103DataSetTableAdapters
    Public Class SC3140103DataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        Public Const MinReserveId As Long = -1           ' 最小予約ID

        ''' <summary>
        ''' Log開始用文言
        ''' </summary>
        ''' <remarks></remarks>
        Private Const LOG_START As String = "Start"

        ''' <summary>
        ''' Log終了文言
        ''' </summary>
        ''' <remarks></remarks>
        Private Const LOG_END As String = "End"

        ''' <summary>
        ''' エラー:DBタイムアウト
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_RET_DBTIMEOUT As Long = 901

#End Region

#Region " サービス来店実績取得"

        '''-------------------------------------------------------
        ''' <summary>
        ''' サービス来店実績取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="account">アカウント</param>
        ''' <param name="orderNoList">整備受注№DataTable</param>
        ''' <param name="nowDate">現在日付</param>
        ''' <returns>サービス来店実績データセット</returns>
        ''' <remarks></remarks>
        '''-------------------------------------------------------
        Public Function GetVisitManagement(ByVal dealerCode As String,
                                           ByVal branchCode As String,
                                           ByVal account As String,
                                           ByVal orderNoList As IC3801003DataSet.IC3801003NoDeliveryRODataTable,
                                           ByVal nowDate As Date) As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, account = {5}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_START _
                                    , dealerCode _
                                    , branchCode _
                                    , account))

            Dim dt As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103ServiceVisitManagementDataTable)("SC3140103_001")
                    Dim sql As New StringBuilder
                    Dim sqlOrderNo As New StringBuilder

                    ' R/O番号用取得文字列
                    If Not orderNoList Is Nothing Then
                        If orderNoList.Count > 0 Then
                            sqlOrderNo.Append(" OR ORDERNO IN ( ")
                            Dim count As Long = 1
                            Dim orderName As String
                            For Each row As IC3801003DataSet.IC3801003NoDeliveryRORow In orderNoList.Rows
                                ' SQL作成
                                orderName = String.Format(CultureInfo.CurrentCulture, "ORDERNO{0}", count)
                                If count > 1 Then
                                    sqlOrderNo.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", orderName))
                                Else
                                    sqlOrderNo.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", orderName))
                                End If

                                ' パラメータ作成
                                query.AddParameterWithTypeValue(orderName, OracleDbType.Char, row.ORDERNO)
                                count += 1
                            Next
                            sqlOrderNo.Append(" ) ")
                        End If
                    End If

                    'SQL文作成
                    With sql
                        .Append("SELECT /* SC3140103_001 */ ")
                        .Append("       VISITSEQ ")
                        .Append("     , DLRCD ")
                        .Append("     , STRCD ")
                        .Append("     , NVL(VISITTIMESTAMP, :MINDATE) AS VISITTIMESTAMP ")
                        .Append("     , VCLREGNO ")
                        .Append("     , CUSTSEGMENT ")
                        .Append("     , DMSID ")
                        .Append("     , VIN ")
                        .Append("     , MODELCODE ")
                        .Append("     , NAME ")
                        .Append("     , TELNO ")
                        .Append("     , MOBILE ")
                        .Append("     , SACODE ")
                        .Append("     , NVL(ASSIGNTIMESTAMP, :MINDATE) AS ASSIGNTIMESTAMP ")
                        .Append("     , NVL(REZID, :MINREZID) AS REZID ")
                        .Append("     , PARKINGCODE ")
                        .Append("     , ORDERNO ")
                        .Append("     , NVL(FREZID, :MINREZID) AS FREZID ")
                        .Append("  FROM TBL_SERVICE_VISIT_MANAGEMENT ")
                        .Append(" WHERE DLRCD  = :DLRCD ")
                        .Append("   AND STRCD  = :STRCD ")
                        .Append("   AND SACODE = :SACODE ")
                        .Append("   AND ( ")
                        .Append("       (NVL(ORDERNO, ' ') = ' ' ")
                        .Append("   AND ")
                        .Append("       TO_CHAR(:TODAY, 'YYYYMMDD') <= TO_CHAR(VISITTIMESTAMP, 'YYYYMMDD')) ")
                        .Append(sqlOrderNo.ToString())
                        .Append("   ) ")
                    End With

                    query.CommandText = sql.ToString()
                    'バインド変数
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                    query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, account)
                    query.AddParameterWithTypeValue("TODAY", OracleDbType.Date, nowDate)
                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, DateTime.MinValue)
                    query.AddParameterWithTypeValue("MINREZID", OracleDbType.Int64, MinReserveId)

                    '検索結果返却
                    dt = query.GetData()
                End Using
            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , C_RET_DBTIMEOUT _
                                        , ex.Message))
                Throw ex
            End Try

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dt.Rows.Count))

            Return dt
        End Function

        '''-------------------------------------------------------
        ''' <summary>
        ''' サービス来店実績取得（チップ詳細用）
        ''' </summary>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <returns>サービス来店実績データセット</returns>
        ''' <remarks></remarks>
        '''-------------------------------------------------------
        Public Function GetVisitManagement(ByVal visitSeq As Long) As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} IN:viditSeq = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_START _
                                    , visitSeq))

            Dim dt As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103ServiceVisitManagementDataTable)("SC3140103_002")
                    Dim sql As New StringBuilder

                    'SQL文作成
                    With sql
                        .Append("SELECT /* SC3140103_002 */ ")
                        .Append("       VISITSEQ ")
                        .Append("     , DLRCD ")
                        .Append("     , STRCD ")
                        .Append("     , NVL(VISITTIMESTAMP, :MINDATE) AS VISITTIMESTAMP ")
                        .Append("     , VCLREGNO ")
                        .Append("     , CUSTSEGMENT ")
                        .Append("     , DMSID ")
                        .Append("     , VIN ")
                        .Append("     , MODELCODE ")
                        .Append("     , NAME ")
                        .Append("     , TELNO ")
                        .Append("     , MOBILE ")
                        .Append("     , SACODE ")
                        .Append("     , NVL(ASSIGNTIMESTAMP, :MINDATE) AS ASSIGNTIMESTAMP ")
                        .Append("     , NVL(REZID, :MINREZID) AS REZID ")
                        .Append("     , PARKINGCODE ")
                        .Append("     , ORDERNO ")
                        .Append("     , NVL(FREZID, :MINREZID) AS FREZID ")
                        .Append("  FROM TBL_SERVICE_VISIT_MANAGEMENT ")
                        .Append(" WHERE VISITSEQ = :VISITSEQ ")
                    End With

                    query.CommandText = sql.ToString()
                    'バインド変数
                    query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, visitSeq)
                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, DateTime.MinValue)
                    query.AddParameterWithTypeValue("MINREZID", OracleDbType.Int64, MinReserveId)

                    '検索結果返却
                    dt = query.GetData()
                End Using
            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , C_RET_DBTIMEOUT _
                                        , ex.Message))
                Throw ex
            End Try

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dt.Rows.Count))

            Return dt
        End Function

#End Region

#Region " ストール予約取得"

        '''-------------------------------------------------------
        ''' <summary>
        ''' ストール予約取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveIdList">初回予約IDリスト</param>
        ''' <returns>ストール予約データセット</returns>
        ''' <remarks></remarks>
        '''-------------------------------------------------------
        Public Function GetStallReserveInformation(ByVal dealerCode As String, ByVal branchCode As String, ByVal reserveIdList As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable) As SC3140103DataSet.SC3140103StallRezinfoDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_START _
                                    , dealerCode _
                                    , branchCode))

            Dim dt As SC3140103DataSet.SC3140103StallRezinfoDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103StallRezinfoDataTable)("SC3140103_003")
                    Dim sql As New StringBuilder
                    Dim sqlReserveId As New StringBuilder
                    Dim sqlPreReserveId As New StringBuilder

                    ' 初回予約ID用取得文字列
                    If Not reserveIdList Is Nothing Then
                        If reserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID").Count > 0 Then
                            sqlReserveId.Append(" AND (T1.REZID IN ( ")
                            sqlPreReserveId.Append(" OR T1.PREZID IN ( ")
                            Dim count As Long = 1
                            Dim reserveIdName As String
                            Dim preReserveIdName As String
                            For Each row As SC3140103DataSet.SC3140103ServiceVisitManagementRow In reserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID")
                                ' SQL作成
                                reserveIdName = String.Format(CultureInfo.CurrentCulture, "REZID{0}", count)
                                preReserveIdName = String.Format(CultureInfo.CurrentCulture, "PREZID{0}", count)
                                If count > 1 Then
                                    sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", reserveIdName))
                                    sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", preReserveIdName))
                                Else
                                    sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", reserveIdName))
                                    sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", preReserveIdName))
                                End If

                                ' パラメータ作成
                                query.AddParameterWithTypeValue(reserveIdName, OracleDbType.Int64, row.FREZID)
                                query.AddParameterWithTypeValue(preReserveIdName, OracleDbType.Int64, row.FREZID)
                                count += 1
                            Next
                            sqlReserveId.Append(" ) ")
                            sqlPreReserveId.Append(" )) ")
                        End If
                    End If
                    If String.IsNullOrEmpty(sqlReserveId.ToString()) Then
                        sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND T1.REZID = {0} ", MinReserveId))
                    End If

                    'SQL文作成
                    With sql
                        .Append("SELECT /* SC3140103_003 */ ")
                        .Append("       T1.DLRCD ")                                                 ' 販売店コード
                        .Append("     , T1.STRCD ")                                                 ' 店舗コード
                        .Append("     , T1.REZID")                                                  ' 予約ID
                        .Append("     , NVL(T1.PREZID, T1.REZID) AS PREZID ")                       ' 管理予約ID
                        .Append("     , NVL(T1.STARTTIME, :MINDATE) AS STARTTIME ")                 ' 使用開始日時
                        .Append("     , NVL(T1.ENDTIME, :MINDATE) AS ENDTIME ")                     ' 使用終了日時 (作業終了予定時刻)
                        .Append("     , T1.CUSTCD ")                                                ' 顧客コード
                        .Append("     , T1.CUSTOMERNAME ")                                          ' 氏名
                        .Append("     , T1.TELNO ")                                                 ' 電話番号
                        .Append("     , T1.MOBILE ")                                                ' 携帯番号
                        .Append("     , T1.VEHICLENAME ")                                           ' 車名
                        .Append("     , T1.VCLREGNO ")                                              ' 登録ナンバー
                        .Append("     , T1.VIN ")                                                   ' VIN
                        .Append("     , T1.MERCHANDISECD ")                                         ' 商品コード
                        .Append("     , T3.MERCHANDISENAME ")                                       ' 商品名 (代表入庫項目)
                        .Append("     , T1.MODELCODE ")                                             ' モデル
                        .Append("     , NVL(T1.MILEAGE, -1) AS MILEAGE ")                           ' 走行距離
                        .Append("     , NVL(T1.WASHFLG, '0') AS WASHFLG ")                          ' 洗車有無
                        .Append("     , T1.WALKIN ")                                                ' 来店フラグ
                        .Append("     , T1.REZ_DELI_DATE ")                                         ' 予約_納車_希望日時時刻 (納車予定日時)
                        .Append("     , NVL(T1.ACTUAL_STIME, :MINDATE) AS ACTUAL_STIME ")           ' 作業開始日時
                        .Append("     , NVL(T1.ACTUAL_ETIME, :MINDATE) AS ACTUAL_ETIME ")           ' 作業終了日時
                        .Append("  FROM TBL_STALLREZINFO T1 ")
                        .Append("     , TBL_MERCHANDISEMST T3 ")
                        .Append(" WHERE T1.DLRCD = T3.DLRCD  (+) ")
                        .Append("   AND T1.MERCHANDISECD = T3.MERCHANDISECD (+) ")
                        .Append("   AND T1.DLRCD = :DLRCD ")
                        .Append("   AND T1.STRCD = :STRCD ")
                        .Append(sqlReserveId.ToString())
                        .Append(sqlPreReserveId.ToString())
                        .Append("   AND NOT EXISTS ( SELECT 1 ")
                        .Append("                      FROM TBL_STALLREZINFO T2 ")
                        .Append("                     WHERE T2.DLRCD = T1.DLRCD ")
                        .Append("                       AND T2.STRCD = T1.STRCD ")
                        .Append("                       AND T2.REZID = T1.REZID ")
                        .Append("                       AND ( (T2.STOPFLG = :STOPFLG0 ")
                        .Append("                       AND T2.CANCELFLG = :CANCELFLG1) ")
                        .Append("                        OR T2.REZCHILDNO IN ( :CHILDNOLEAVE, :CHILDNODELIVERY ) ) ) ")

                    End With

                    query.CommandText = sql.ToString()
                    'バインド変数
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                    query.AddParameterWithTypeValue("STOPFLG0", OracleDbType.Char, "0")
                    query.AddParameterWithTypeValue("CANCELFLG1", OracleDbType.Char, "1")
                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, DateTime.MinValue)
                    query.AddParameterWithTypeValue("CHILDNOLEAVE", OracleDbType.Int64, 0)         ' 子予約連番-0:引取
                    query.AddParameterWithTypeValue("CHILDNODELIVERY", OracleDbType.Int64, 999)    ' 子予約連番-999:納車

                    '検索結果返却
                    dt = query.GetData()
                End Using
            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , C_RET_DBTIMEOUT _
                                        , ex.Message))
                Throw ex
            End Try

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dt.Rows.Count))

            Return dt
        End Function

#End Region

#Region " ストール実績取得"

        '''-------------------------------------------------------
        ''' <summary>
        ''' ストール実績取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveIdList">初回予約IDリスト</param>
        ''' <returns>ストール実績データセット</returns>
        ''' <remarks></remarks>
        '''-------------------------------------------------------
        Public Function GetStallProcess(ByVal dealerCode As String, ByVal branchCode As String, ByVal reserveIdList As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable) As SC3140103DataSet.SC3140103StallProcessDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_START _
                                    , dealerCode _
                                    , branchCode))

            Dim dt As SC3140103DataSet.SC3140103StallProcessDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103StallProcessDataTable)("SC3140103_004")
                    Dim sql As New StringBuilder
                    Dim sqlReserveId As New StringBuilder
                    Dim sqlPreReserveId As New StringBuilder

                    ' 初回予約ID用取得文字列
                    If Not reserveIdList Is Nothing Then
                        If reserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID").Count > 0 Then
                            sqlReserveId.Append(" AND (T1.REZID IN ( ")
                            sqlPreReserveId.Append(" OR T1.PREZID IN ( ")
                            Dim count As Long = 1
                            Dim reserveIdName As String
                            Dim preReserveIdName As String
                            For Each row As SC3140103DataSet.SC3140103ServiceVisitManagementRow In reserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID")
                                ' SQL作成
                                reserveIdName = String.Format(CultureInfo.CurrentCulture, "REZID{0}", count)
                                preReserveIdName = String.Format(CultureInfo.CurrentCulture, "PREZID{0}", count)
                                If count > 1 Then
                                    sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", reserveIdName))
                                    sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", preReserveIdName))
                                Else
                                    sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", reserveIdName))
                                    sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", preReserveIdName))
                                End If

                                ' パラメータ作成
                                query.AddParameterWithTypeValue(reserveIdName, OracleDbType.Int64, row.FREZID)
                                query.AddParameterWithTypeValue(preReserveIdName, OracleDbType.Int64, row.FREZID)
                                count += 1
                            Next
                            sqlReserveId.Append(" ) ")
                            sqlPreReserveId.Append(" )) ")
                        End If
                    End If
                    If String.IsNullOrEmpty(sqlReserveId.ToString()) Then
                        sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND T1.REZID = {0} ", MinReserveId))
                    End If

                    'SQL文作成
                    With sql
                        .Append("SELECT /* SC3140103_004 */ ")
                        .Append("       T1.DLRCD ")                             ' 販売店コード
                        .Append("     , T1.STRCD ")                             ' 店舗コード
                        .Append("     , T1.REZID")                              ' 予約ID
                        .Append("     , NVL(T1.PREZID, T1.REZID) AS PREZID ")   ' 管理予約ID
                        .Append("     , T2.DSEQNO")                             ' 日跨ぎシーケンス番号
                        .Append("     , T2.SEQNO")                              ' シーケンス番号
                        .Append("     , NVL(T1.WASHFLG, '0') AS WASHFLG")       ' 洗車有無
                        .Append("     , T2.RESULT_STATUS ")                     ' 実績_ステータス
                        .Append("     , T2.REZ_END_TIME ")                      ' 予定_ストール終了日時時刻 (納車予定日時)
                        .Append("     , T2.RESULT_WASH_START ")                 ' 洗車開始
                        .Append("     , T2.RESULT_WASH_END ")                   ' 洗車終了
                        .Append("     , T3.STAFFCD AS STAFFCD ")                ' 担当テクニシャンCD
                        .Append("     , T5.USERNAME AS STAFFNAME ")             ' 担当テクニシャン名
                        .Append("  FROM TBL_STALLREZINFO T1 ")
                        .Append("     , TBL_STALLPROCESS T2 ")
                        .Append("     , TBL_TSTAFFSTALL T3 ")
                        .Append("     , TBL_SSTAFF T4 ")
                        .Append("     , TBL_USERS T5 ")
                        .Append(" WHERE T1.DLRCD = T2.DLRCD ")
                        .Append("   AND T1.STRCD = T2.STRCD ")
                        .Append("   AND T1.REZID = T2.REZID ")
                        .Append("   AND T2.DLRCD = T3.DLRCD (+) ")
                        .Append("   AND T2.STRCD = T3.STRCD (+) ")
                        .Append("   AND T2.REZID = T3.REZID (+) ")
                        .Append("   AND T3.DLRCD = T4.DLRCD (+) ")
                        .Append("   AND T3.STRCD = T4.STRCD (+) ")
                        .Append("   AND T3.STAFFCD = T4.STAFFCD (+) ")
                        .Append("   AND T4.DLRCD = T5.DLRCD (+) ")
                        .Append("   AND T4.STRCD = T5.STRCD (+) ")
                        .Append("   AND T4.ACCOUNT = T5.ACCOUNT (+) ")
                        .Append("   AND T1.DLRCD = :DLRCD ")
                        .Append("   AND T1.STRCD = :STRCD ")
                        .Append("   AND NVL(T1.REZCHILDNO, 1) <> :CHILDNOLEAVE ")
                        .Append("   AND NVL(T1.REZCHILDNO, 1) <> :CHILDNODELIVERY ")
                        .Append(sqlReserveId.ToString())
                        .Append(sqlPreReserveId.ToString())
                    End With

                    query.CommandText = sql.ToString()
                    'バインド変数
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                    query.AddParameterWithTypeValue("CHILDNOLEAVE", OracleDbType.Int64, 0)         ' 子予約連番-0:引取
                    query.AddParameterWithTypeValue("CHILDNODELIVERY", OracleDbType.Int64, 999)    ' 子予約連番-999:納車

                    '検索結果返却
                    dt = query.GetData()
                End Using
            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , C_RET_DBTIMEOUT _
                                        , ex.Message))
                Throw ex
            End Try

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dt.Rows.Count))

            Return dt
        End Function

        '''-------------------------------------------------------
        ''' <summary>
        ''' ストール実績取得(チップ詳細用)
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveIdList">初回予約IDリスト</param>
        ''' <returns>ストール実績データセット</returns>
        ''' <remarks></remarks>
        '''-------------------------------------------------------
        Public Function GetStallProcessDetail(ByVal dealerCode As String, ByVal branchCode As String, ByVal reserveIdList As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable) As SC3140103DataSet.SC3140103StallProcessDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_START _
                                    , dealerCode _
                                    , branchCode))

            Dim dt As SC3140103DataSet.SC3140103StallProcessDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103StallProcessDataTable)("SC3140103_005")
                    Dim sql As New StringBuilder
                    Dim sqlReserveId As New StringBuilder
                    Dim sqlPreReserveId As New StringBuilder

                    ' 初回予約ID用取得文字列
                    If Not reserveIdList Is Nothing Then
                        If reserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID").Count > 0 Then
                            sqlReserveId.Append(" AND (T1.REZID IN ( ")
                            sqlPreReserveId.Append(" OR T1.PREZID IN ( ")
                            Dim count As Long = 1
                            Dim reserveIdName As String
                            Dim preReserveIdName As String
                            For Each row As SC3140103DataSet.SC3140103ServiceVisitManagementRow In reserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID")
                                ' SQL作成
                                reserveIdName = String.Format(CultureInfo.CurrentCulture, "REZID{0}", count)
                                preReserveIdName = String.Format(CultureInfo.CurrentCulture, "PREZID{0}", count)
                                If count > 1 Then
                                    sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", reserveIdName))
                                    sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", preReserveIdName))
                                Else
                                    sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", reserveIdName))
                                    sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", preReserveIdName))
                                End If

                                ' パラメータ作成
                                query.AddParameterWithTypeValue(reserveIdName, OracleDbType.Int64, row.FREZID)
                                query.AddParameterWithTypeValue(preReserveIdName, OracleDbType.Int64, row.FREZID)
                                count += 1
                            Next
                            sqlReserveId.Append(" ) ")
                            sqlPreReserveId.Append(" )) ")
                        End If
                    End If
                    If String.IsNullOrEmpty(sqlReserveId.ToString()) Then
                        sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND T1.REZID = {0} ", MinReserveId))
                    End If

                    'SQL文作成
                    With sql
                        .Append("SELECT /* SC3140103_005 */ ")
                        .Append("       T1.DLRCD ")                             ' 販売店コード
                        .Append("     , T1.STRCD ")                             ' 店舗コード
                        .Append("     , T1.REZID ")                             ' 予約ID
                        .Append("     , NVL(T1.PREZID, T1.REZID) AS PREZID ")   ' 管理予約ID
                        .Append("     , T2.DSEQNO ")                            ' 日跨ぎシーケンス番号
                        .Append("     , T2.SEQNO ")                             ' シーケンス番号
                        .Append("     , NVL(T1.WASHFLG, '0') AS WASHFLG ")      ' 洗車有無
                        .Append("     , T2.RESULT_STATUS ")                     ' 実績_ステータス
                        .Append("     , T2.REZ_END_TIME ")                      ' 予定_ストール終了日時時刻 (納車予定日時)
                        .Append("     , T2.RESULT_WASH_START ")                 ' 洗車開始
                        .Append("     , T2.RESULT_WASH_END ")                   ' 洗車終了
                        .Append("     , NULL AS STAFFCD ")                      ' 担当テクニシャン
                        .Append("     , NULL AS STAFFNAME ")                    ' 担当テクニシャン名
                        .Append("  FROM TBL_STALLREZINFO T1 ")
                        .Append("     , TBL_STALLPROCESS T2 ")
                        .Append(" WHERE T1.DLRCD = T2.DLRCD ")
                        .Append("   AND T1.STRCD = T2.STRCD ")
                        .Append("   AND T1.REZID = T2.REZID ")
                        .Append("   AND T1.DLRCD  = :DLRCD ")
                        .Append("   AND T1.STRCD  = :STRCD ")
                        .Append(sqlReserveId.ToString())
                        .Append(sqlPreReserveId.ToString())
                    End With

                    query.CommandText = sql.ToString()
                    'バインド変数
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)

                    '検索結果返却
                    dt = query.GetData()
                End Using
            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , C_RET_DBTIMEOUT _
                                        , ex.Message))
                Throw ex
            End Try

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dt.Rows.Count))

            Return dt
        End Function

#End Region

#Region " ストール設定情報取得(標準時間専用)"

        '''-------------------------------------------------------
        ''' <summary>
        ''' ストール設定情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns>ストール設定データセット</returns>
        ''' <remarks></remarks>
        '''-------------------------------------------------------
        Public Function GetStallControl(ByVal dealerCode As String, ByVal branchCode As String) As SC3140103DataSet.SC3140103StallCtl2DataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_START _
                                    , dealerCode _
                                    , branchCode))

            Dim dt As SC3140103DataSet.SC3140103StallCtl2DataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103StallCtl2DataTable)("SC3140103_006")
                    Dim sql As New StringBuilder

                'SQL文作成
                    With sql
                        .Append("SELECT /* SC3140103_006 */ ")
                        .Append("       NVL(RECEPT_NORES_WARNING_LT, 0) AS RECEPT_NORES_WARNING_LT")
                        .Append("     , NVL(RECEPT_NORES_ABNORMAL_LT, 0) AS RECEPT_NORES_ABNORMAL_LT ")
                        .Append("     , NVL(RECEPT_RES_WARNING_LT, 0) AS RECEPT_RES_WARNING_LT ")
                        .Append("     , NVL(RECEPT_RES_ABNORMAL_LT, 0) AS RECEPT_RES_ABNORMAL_LT ")
                        .Append("     , NVL(ADDWORK_NORES_WARNING_LT, 0) AS ADDWORK_NORES_WARNING_LT ")
                        .Append("     , NVL(ADDWORK_NORES_ABNORMAL_LT, 0) AS ADDWORK_NORES_ABNORMAL_LT ")
                        .Append("     , NVL(ADDWORK_RES_WARNING_LT, 0) AS ADDWORK_RES_WARNING_LT ")
                        .Append("     , NVL(ADDWORK_RES_ABNORMAL_LT, 0) AS ADDWORK_RES_ABNORMAL_LT ")
                        .Append("     , NVL(DELIVERYPRE_ABNORMAL_LT, 0) AS DELIVERYPRE_ABNORMAL_LT ")
                        .Append("     , NVL(DELIVERYWR_ABNORMAL_LT, 0) AS DELIVERYWR_ABNORMAL_LT ")
                        .Append("  FROM TBL_SERVICEINI ")
                        .Append(" WHERE DLRCD = :DLRCD ")
                        .Append("   AND STRCD = :STRCD ")
                    End With

                    query.CommandText = sql.ToString()
                    'バインド変数
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)

                    '検索結果返却
                    dt = query.GetData()
                End Using
            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , C_RET_DBTIMEOUT _
                                        , ex.Message))
                Throw ex
            End Try

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dt.Rows.Count))

            Return dt
        End Function

#End Region

    End Class

End Namespace


Partial Class SC3140103DataSet
End Class
