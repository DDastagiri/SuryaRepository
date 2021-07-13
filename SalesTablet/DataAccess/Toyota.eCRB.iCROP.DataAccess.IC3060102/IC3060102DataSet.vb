
'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3060102DataSet.vb
'─────────────────────────────────────
'機能： 査定依頼取得インタフェースデータアクセス
'補足： 
'作成： 
'更新： 2013/05/27 TMEJ m.asano 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 $01
'─────────────────────────────────────
Imports System.Text
Imports System.Reflection.MethodBase
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core


Namespace IC3060102DataSetTableAdapters

    ''' <summary>
    ''' IC3060102（査定依頼取得インタフェース）
    ''' 査定依頼取得データアクセスクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class IC3060102DataTableTableAdapter
        Inherits Global.System.ComponentModel.Component


#Region "コンストラクタ"
        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub
#End Region

#Region "001.未対応査定依頼件数取得"

        ''' <summary>
        ''' 001.未対応査定依頼件数取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="clientId">端末ID</param>
        ''' <param name="sendDateFrom">送信日時From</param>
        ''' <param name="sendDateTo">送信日時To</param>
        ''' <returns>未対応査定依頼件数DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetAssessmentReqCountDataTable( _
            ByVal dealerCode As String, _
            ByVal storeCode As String, _
            ByVal clientId As String, _
            ByVal sendDateFrom As Date, _
            ByVal sendDateTo As Date) _
            As IC3060102DataSet.IC3060102AssessmentReqCountDataTable
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("dealerCode", dealerCode, False), True)
            Logger.Info(getLogParam("storeCode", storeCode, True), True)
            Logger.Info(getLogParam("clientId", clientId, True), True)
            Logger.Info(getLogParam("sendDateFrom", CStr(sendDateFrom), True), True)
            Logger.Info(getLogParam("sendDateTo", CStr(sendDateTo), True), True)

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3060102_001 */ ")
                .Append("       COUNT(1) AS ASSESSMENTREQCOUNT ")
                .Append("  FROM TBL_NOTICEREQUEST T1 ")
                .Append("     , TBL_NOTICEINFO T2 ")
                .Append("     , TBL_UCARASSESSMENT T3 ")
                .Append(" WHERE T1.NOTICEREQID = T2.NOTICEREQID ")
                .Append("   AND T1.REQCLASSID = T3.ASSESSMENTNO ")
                .Append("   AND (T1.LASTNOTICEID = T2.NOTICEID ")
                .Append("    OR T1.LASTNOTICEID = 0) ")
                .Append("   AND T1.NOTICEREQCTG = '01' ")
                .Append("   AND T1.DLRCD = :DLRCD ")
                .Append("   AND T1.STRCD = :STRCD ")
                .Append("   AND T1.STATUS = '1' ")
                .Append("   AND T2.TOCLIENTID = :CLIENTID ")
                .Append("   AND T2.SENDDATE BETWEEN :SENDDATEFROM AND :SENDDATETO ")
                .Append("   AND T2.STATUS = '1' ")
            End With

            Using query As New DBSelectQuery( _
                Of IC3060102DataSet.IC3060102AssessmentReqCountDataTable)("IC3060102_001")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("CLIENTID", OracleDbType.Varchar2, clientId)
                query.AddParameterWithTypeValue("SENDDATEFROM", OracleDbType.Date, sendDateFrom)
                query.AddParameterWithTypeValue("SENDDATETO", OracleDbType.Date, sendDateTo)

                '終了ログ出力
                Dim dt As IC3060102DataSet.IC3060102AssessmentReqCountDataTable = query.GetData()
                Dim count As Integer = dt.Count
                Logger.Info(getReturnParam("DataTable Count:" & CStr(count)), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)

                Return dt

            End Using

        End Function

#End Region

#Region "002.対応中査定依頼情報取得"

        ''' <summary>
        ''' 002.対応中査定依頼情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="clientId">端末ID</param>
        ''' <param name="sendDateFrom">送信日時From</param>
        ''' <param name="sendDateTo">送信日時To</param>
        ''' <returns>対応中査定依頼情報DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetInProgressAssessmentReqInfoDataTable( _
            ByVal dealerCode As String, _
            ByVal storeCode As String, _
            ByVal clientId As String, _
            ByVal sendDateFrom As Date, _
            ByVal sendDateTo As Date) _
            As IC3060102DataSet.IC3060102InProgressAssessmentReqInfoDataTable
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("dealerCode", dealerCode, False), True)
            Logger.Info(getLogParam("storeCode", storeCode, True), True)
            Logger.Info(getLogParam("clientId", clientId, True), True)
            Logger.Info(getLogParam("sendDateFrom", CStr(sendDateFrom), True), True)
            Logger.Info(getLogParam("sendDateTo", CStr(sendDateTo), True), True)

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3060102_002 */ ")
                .Append("       T1.NOTICEREQID ")
                .Append("  FROM TBL_NOTICEREQUEST T1 ")
                .Append("     , TBL_NOTICEINFO T2 ")
                .Append("     , TBL_UCARASSESSMENT T3 ")
                .Append(" WHERE T1.NOTICEREQID = T2.NOTICEREQID ")
                .Append("   AND T1.LASTNOTICEID = T2.NOTICEID ")
                .Append("   AND T1.REQCLASSID = T3.ASSESSMENTNO ")
                .Append("   AND T1.NOTICEREQCTG = '01' ")
                .Append("   AND T1.DLRCD = :DLRCD ")
                .Append("   AND T1.STRCD = :STRCD ")
                .Append("   AND T1.STATUS = '3' ")
                .Append("   AND T2.FROMCLIENTID = :CLIENTID ")
                .Append("   AND T2.SENDDATE BETWEEN :SENDDATEFROM AND :SENDDATETO ")
                .Append("   AND T2.STATUS = '3' ")
            End With

            Using query As New DBSelectQuery( _
                Of IC3060102DataSet.IC3060102InProgressAssessmentReqInfoDataTable) _
                ("IC3060102_002")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("CLIENTID", OracleDbType.Varchar2, clientId)
                query.AddParameterWithTypeValue("SENDDATEFROM", OracleDbType.Date, sendDateFrom)
                query.AddParameterWithTypeValue("SENDDATETO", OracleDbType.Date, sendDateTo)

                '終了ログ出力
                Dim dt As IC3060102DataSet.IC3060102InProgressAssessmentReqInfoDataTable = query.GetData()
                Dim count As Integer = dt.Count
                Logger.Info(getReturnParam("DataTable Count:" & CStr(count)), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)

                Return dt

            End Using

        End Function

#End Region

#Region "003.査定依頼一覧情報取得"

        ''' <summary>
        ''' 003.査定依頼一覧情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="clientId">端末ID</param>
        ''' <param name="sendDateFrom">送信日時From</param>
        ''' <param name="sendDateTo">送信日時To</param>
        ''' <param name="status">ステータス</param>
        ''' <returns>査定依頼一覧情報DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetAssessmentReqListInfoDataTable( _
            ByVal dealerCode As String, _
            ByVal storeCode As String, _
            ByVal clientId As String, _
            ByVal sendDateFrom As Date, _
            ByVal sendDateTo As Date, _
            ByVal status As String) _
            As IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("dealerCode", dealerCode, False), True)
            Logger.Info(getLogParam("storeCode", storeCode, True), True)
            Logger.Info(getLogParam("clientId", clientId, True), True)
            Logger.Info(getLogParam("sendDateFrom", CStr(sendDateFrom), True), True)
            Logger.Info(getLogParam("sendDateTo", CStr(sendDateTo), True), True)
            Logger.Info(getLogParam("status", CStr(status), True), True)

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3060102_003 */ ")
                .Append("       T1.NOTICEREQID ")
                .Append("     , T1.REQCLASSID ")
                .Append("     , T1.CRCUSTID ")
                .Append("     , T1.CUSTOMERCLASS ")
                .Append("     , T1.CSTKIND ")
                .Append("     , T1.STATUS ")
                .Append("     , T1.CUSTOMNAME ")
                .Append("     , T2.FROMACCOUNT ")
                .Append("     , T2.FROMCLIENTID ")
                .Append("     , T2.FROMACCOUNTNAME ")
                .Append("     , T2.SENDDATE ")
                .Append("     , T3.RETENTION ")
                .Append("     , T3.ORGCSTVCL_VIN AS VIN ")
                .Append("     , T3.NEWCSTVCL_SEQNO ")
                .Append("     , T3.MAKERNAME ")
                .Append("     , T3.VEHICLENAME AS SERIESNM")
                .Append("     , T3.REGISTRATIONNO AS VCLREGNO ")
                .Append("     , T4.SALESTABLENO ")
                .Append("  FROM TBL_NOTICEREQUEST T1 ")
                .Append("     , TBL_NOTICEINFO T2 ")
                .Append("     , TBL_UCARASSESSMENT T3 ")
                .Append("     , ( ")
                .Append("    SELECT DLRCD ")
                .Append("         , STRCD ")
                .Append("         , VCLREGNO ")
                .Append("         , CUSTID ")
                .Append("         , SALESTABLENO ")
                .Append("         , ROW_NUMBER() ")
                .Append("               OVER(PARTITION BY DLRCD ")
                .Append("                               , STRCD ")
                .Append("                               , CUSTID ")
                .Append("               ORDER BY VISITSEQ DESC) AS ROWNO ")
                .Append("      FROM TBL_VISIT_SALES ")
                .Append("     WHERE DLRCD = :DLRCD ")
                .Append("       AND STRCD = :STRCD ")
                .Append("       AND VISITSTATUS = '07' ")
                .Append("       ) T4 ")
                .Append(" WHERE T1.NOTICEREQID = T2.NOTICEREQID ")
                .Append("   AND T1.REQCLASSID = T3.ASSESSMENTNO ")
                .Append("   AND T1.DLRCD = T4.DLRCD(+) ")
                .Append("   AND T1.STRCD = T4.STRCD(+) ")
                .Append("   AND T1.CRCUSTID = T4.CUSTID(+) ")
                .Append("   AND (T1.LASTNOTICEID = T2.NOTICEID ")
                .Append("    OR T1.LASTNOTICEID = 0) ")
                .Append("   AND T1.NOTICEREQCTG = '01' ")
                .Append("   AND T1.DLRCD = :DLRCD ")
                .Append("   AND T1.STRCD = :STRCD ")
                .Append("   AND T1.STATUS = :STATUS ")
                .Append("   AND T2.TOCLIENTID = :CLIENTID ")
                .Append("   AND T2.SENDDATE BETWEEN :SENDDATEFROM AND :SENDDATETO ")
                .Append("   AND T2.STATUS = :STATUS ")
                .Append("   AND T4.ROWNO(+) = 1 ")
                .Append(" ORDER BY T2.SENDDATE ")
                .Append("        , T1.NOTICEREQID ")
            End With

            Using query As New DBSelectQuery( _
                Of IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable) _
                ("IC3060102_003")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("CLIENTID", OracleDbType.Varchar2, clientId)
                query.AddParameterWithTypeValue("SENDDATEFROM", OracleDbType.Date, sendDateFrom)
                query.AddParameterWithTypeValue("SENDDATETO", OracleDbType.Date, sendDateTo)
                query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, status)

                '終了ログ出力
                Dim dt As IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable = query.GetData()
                Dim count As Integer = dt.Count
                Logger.Info(getReturnParam("DataTable Count:" & CStr(count)), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return dt

            End Using

        End Function

#End Region

#Region "004.査定依頼状態確認"

        ''' <summary>
        ''' 004.査定依頼状態確認
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="noticeReqId">通知依頼ID</param>
        ''' <param name="sendDateFrom">送信日時From</param>
        ''' <param name="sendDateTo">送信日時To</param>
        ''' <returns>査定依頼状態情報DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetAssessmentReqStateInfoDataTable( _
            ByVal dealerCode As String, _
            ByVal storeCode As String, _
            ByVal noticeReqId As Long, _
            ByVal sendDateFrom As Date, _
            ByVal sendDateTo As Date) _
            As IC3060102DataSet.IC3060102AssessmentReqStateInfoDataTable
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("dealerCode", dealerCode, False), True)
            Logger.Info(getLogParam("storeCode", storeCode, True), True)
            Logger.Info(getLogParam("noticeReqId", CStr(noticeReqId), True), True)
            Logger.Info(getLogParam("sendDateFrom", CStr(sendDateFrom), True), True)
            Logger.Info(getLogParam("sendDateTo", CStr(sendDateTo), True), True)

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3060102_004 */ ")
                .Append("       T1.NOTICEREQCTG ")
                .Append("     , T1.CRCUSTID ")
                .Append("     , T1.CUSTOMERCLASS ")
                .Append("     , T1.CSTKIND ")
                .Append("     , T1.LASTNOTICEID ")
                .Append("     , T1.STATUS ")
                .Append("     , T2.FROMACCOUNT ")
                .Append("     , T2.FROMCLIENTID ")
                .Append("     , T2.FROMACCOUNTNAME ")
                .Append("     , T2.TOACCOUNT ")
                .Append("     , T2.TOCLIENTID ")
                .Append("     , T2.TOACCOUNTNAME ")
                .Append("  FROM TBL_NOTICEREQUEST T1 ")
                .Append("     , TBL_NOTICEINFO T2 ")
                .Append(" WHERE T1.NOTICEREQID = T2.NOTICEREQID(+) ")
                .Append("   AND T1.LASTNOTICEID = T2.NOTICEID(+) ")
                .Append("   AND T1.NOTICEREQID = :NOTICEREQID ")
                .Append("   AND T1.NOTICEREQCTG = '01' ")
                .Append("   AND T1.DLRCD = :DLRCD ")
                .Append("   AND T1.STRCD = :STRCD ")
                .Append("   AND T2.SENDDATE(+) BETWEEN :SENDDATEFROM AND :SENDDATETO ")
            End With

            Using query As New DBSelectQuery(
                Of IC3060102DataSet.IC3060102AssessmentReqStateInfoDataTable) _
                ("IC3060102_004")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Long, noticeReqId)
                query.AddParameterWithTypeValue("SENDDATEFROM", OracleDbType.Date, sendDateFrom)
                query.AddParameterWithTypeValue("SENDDATETO", OracleDbType.Date, sendDateTo)

                '終了ログ出力
                Dim dt As IC3060102DataSet.IC3060102AssessmentReqStateInfoDataTable = query.GetData()
                Dim count As Integer = dt.Count
                Logger.Info(getReturnParam("DataTable Count:" & CStr(count)), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)

                Return dt

            End Using

        End Function

#End Region

#Region "005.自社客情報取得"

        ''' <summary>
        ''' 005.自社客情報取得
        ''' </summary>
        ''' <param name="originalId">自社客連番一覧</param>
        ''' <param name="vin">VIN</param>
        ''' <returns>自社客情報DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetOrgCustomerInfoDataTable( _
            ByVal originalId As String, _
            ByVal vin As String) _
            As IC3060102DataSet.IC3060102CustomerInfoDataTable
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("originalId", originalId, False), True)
            Logger.Info(getLogParam("vin", vin, True), True)

            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3060102_005 */ ")
                .Append("        CUST.CST_PHONE AS TELNO ")
                .Append("      , CUST.CST_MOBILE AS MOBILE ")
                .Append("      , VCL_DLR.REG_NUM AS VCLREGNO ")
                .Append("      , MODEL.MODEL_CD AS SERIESCD ")
                .Append("      , MODEL.MODEL_NAME AS SERIESNM ")
                .Append("      , MAKER.MAKER_CD AS MAKERCD ")
                .Append("      , MAKER.MAKER_NAME AS MAKERNAME ")
                .Append("   FROM TB_M_CUSTOMER CUST ")
                .Append("      , TB_M_CUSTOMER_DLR CUST_DLR ")
                .Append("      , TB_M_CUSTOMER_VCL CUST_VCL ")
                .Append("      , TB_M_VEHICLE VCL ")
                .Append("      , TB_M_VEHICLE_DLR VCL_DLR ")
                .Append("      , TB_M_MODEL MODEL ")
                .Append("      , TB_M_MAKER MAKER  ")
                .Append("  WHERE CUST.CST_ID = CUST_DLR.CST_ID ")
                .Append("    AND CUST_DLR.DLR_CD = CUST_VCL.DLR_CD ")
                .Append("    AND CUST_DLR.CST_ID = CUST_VCL.CST_ID ")
                .Append("    AND CUST_VCL.DLR_CD = VCL_DLR.DLR_CD(+) ")
                .Append("    AND CUST_VCL.VCL_ID = VCL_DLR.VCL_ID(+) ")
                .Append("    AND VCL_DLR.VCL_ID = VCL.VCL_ID(+) ")
                .Append("    AND VCL.MODEL_CD = MODEL.MODEL_CD(+) ")
                .Append("    AND MODEL.MAKER_CD = MAKER.MAKER_CD(+) ")
                .Append("    AND CUST.CST_ID = :CST_ID ")
                .Append("    AND VCL.VCL_VIN(+) = :VCL_VIN ")
                .Append("    AND CUST_DLR.CST_TYPE = '1' ")
                .Append("    AND CUST_VCL.CST_VCL_TYPE = '1' ")
            End With
            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            Using query As New DBSelectQuery( _
                Of IC3060102DataSet.IC3060102CustomerInfoDataTable) _
                ("IC3060102_005")

                query.CommandText = sql.ToString()

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, originalId)
                query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.Varchar2, vin)
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                '終了ログ出力
                Dim dt As IC3060102DataSet.IC3060102CustomerInfoDataTable = query.GetData()
                Dim count As Integer = dt.Count
                Logger.Info(getReturnParam("DataTable Count:" & CStr(count)), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)

                Return dt

            End Using

        End Function

#End Region

#Region "006.未取引客情報取得"

        ''' <summary>
        ''' 006.未取引客情報取得
        ''' </summary>
        ''' <param name="cstId">未取引客ユーザID</param>
        ''' <param name="seqNo">シーケンス番号</param>
        ''' <returns>未取引客情報DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetNewCustomerInfoDataTable( _
            ByVal cstId As String, _
            ByVal seqNo As Decimal) _
            As IC3060102DataSet.IC3060102CustomerInfoDataTable
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("cstId", cstId, False), True)
            Logger.Info(getLogParam("seqNo", CStr(seqNo), True), True)

            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3060102_006 */ ")
                .Append("        CUST.CST_PHONE AS TELNO ")
                .Append("      , CUST.CST_MOBILE AS MOBILE ")
                .Append("      , VCL.VCL_VIN AS VIN ")
                .Append("      , VCL_DLR.REG_NUM AS VCLREGNO ")
                .Append("      , VCL.MODEL_CD AS SERIESCD ")
                .Append("      , VCL.NEWCST_MODEL_NAME AS SERIESNM ")
                .Append("      , VCL.NEWCST_MAKER_NAME AS MAKERNAME ")
                .Append("   FROM TB_M_CUSTOMER CUST ")
                .Append("      , TB_M_CUSTOMER_DLR CUST_DLR ")
                .Append("      , TB_M_CUSTOMER_VCL CUST_VCL ")
                .Append("      , TB_M_VEHICLE VCL ")
                .Append("      , TB_M_VEHICLE_DLR VCL_DLR ")
                .Append("  WHERE CUST.CST_ID = CUST_DLR.CST_ID ")
                .Append("    AND CUST_DLR.DLR_CD = CUST_VCL.DLR_CD ")
                .Append("    AND CUST_DLR.CST_ID = CUST_VCL.CST_ID ")
                .Append("    AND CUST_VCL.DLR_CD = VCL_DLR.DLR_CD(+) ")
                .Append("    AND CUST_VCL.VCL_ID = VCL_DLR.VCL_ID(+) ")
                .Append("    AND VCL_DLR.VCL_ID = VCL.VCL_ID(+) ")
                .Append("    AND CUST.CST_ID = :CST_ID ")
                .Append("    AND VCL.VCL_ID(+) = :VCL_ID ")
                .Append("    AND CUST_DLR.CST_TYPE = '2' ")
            End With
            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            Using query As New DBSelectQuery( _
                Of IC3060102DataSet.IC3060102CustomerInfoDataTable) _
                ("IC3060102_006")

                query.CommandText = sql.ToString()

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstId)
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, seqNo)
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                '終了ログ出力
                Dim dt As IC3060102DataSet.IC3060102CustomerInfoDataTable = query.GetData()
                Dim count As Integer = dt.Count
                Logger.Info(getReturnParam("DataTable Count:" & CStr(count)), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)

                Return dt

            End Using

        End Function

#End Region

#Region "007.副顧客情報取得"

        ''' <summary>
        ''' 007.副顧客情報取得
        ''' </summary>
        ''' <param name="subCustId">副顧客コード</param>
        ''' <param name="vin">VIN</param>
        ''' <returns>副顧客情報DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetSubCustomerInfoDataTable( _
            ByVal subCustId As String, _
            ByVal vin As String) _
            As IC3060102DataSet.IC3060102CustomerInfoDataTable
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("subCustId", subCustId, False), True)
            Logger.Info(getLogParam("vin", vin, True), True)

            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3060102_007 */ ")
                .Append("        CUST.CST_PHONE AS TELNO ")
                .Append("      , CUST.CST_MOBILE AS MOBILE ")
                .Append("      , VCL_DLR.REG_NUM AS VCLREGNO ")
                .Append("      , MODEL.MODEL_CD AS SERIESCD ")
                .Append("      , MODEL.MODEL_NAME AS SERIESNM ")
                .Append("      , MAKER.MAKER_CD AS MAKERCD ")
                .Append("      , MAKER.MAKER_NAME AS MAKERNAME ")
                .Append("   FROM TB_M_CUSTOMER CUST ")
                .Append("      , TB_M_CUSTOMER_DLR CUST_DLR ")
                .Append("      , TB_M_CUSTOMER_VCL CUST_VCL ")
                .Append("      , TB_M_VEHICLE VCL ")
                .Append("      , TB_M_VEHICLE_DLR VCL_DLR ")
                .Append("      , TB_M_MODEL MODEL ")
                .Append("      , TB_M_MAKER MAKER  ")
                .Append("  WHERE CUST.CST_ID = CUST_DLR.CST_ID ")
                .Append("    AND CUST_DLR.DLR_CD = CUST_VCL.DLR_CD ")
                .Append("    AND CUST_DLR.CST_ID = CUST_VCL.CST_ID ")
                .Append("    AND CUST_VCL.DLR_CD = VCL_DLR.DLR_CD(+) ")
                .Append("    AND CUST_VCL.VCL_ID = VCL_DLR.VCL_ID(+) ")
                .Append("    AND VCL_DLR.VCL_ID  = VCL.VCL_ID(+) ")
                .Append("    AND VCL.MODEL_CD = MODEL.MODEL_CD(+) ")
                .Append("    AND MODEL.MAKER_CD = MAKER.MAKER_CD(+) ")
                .Append("    AND CUST.CST_ID = :SUBCUSTID ")
                .Append("    AND VCL.VCL_VIN(+) = :VIN ")
                .Append("    AND CUST_DLR.CST_TYPE = '1' ")
                .Append("    AND CUST_VCL.CST_VCL_TYPE <> '1' ")
            End With
            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            Using query As New DBSelectQuery( _
                Of IC3060102DataSet.IC3060102CustomerInfoDataTable) _
                ("IC3060102_007")

                query.CommandText = sql.ToString()

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                query.AddParameterWithTypeValue("SUBCUSTID", OracleDbType.Decimal, subCustId)
                query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, vin)
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                '終了ログ出力
                Dim dt As IC3060102DataSet.IC3060102CustomerInfoDataTable = query.GetData()
                Dim count As Integer = dt.Count
                Logger.Info(getReturnParam("DataTable Count:" & CStr(count)), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)

                Return dt

            End Using

        End Function

#End Region

#Region "008.自社客お客様名取得"

        ''' <summary>
        ''' 008.自社客お客様名取得
        ''' </summary>
        ''' <param name="originalId">自社客連番</param>
        ''' <returns>お客様名DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetOrgCustomerNameDataTable( _
            ByVal originalId As String) _
            As IC3060102DataSet.IC3060102CustomerNameDataTable
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("originalId", originalId, False), True)

            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3060102_008 */ ")
                .Append("     A.CST_NAME  AS NAME ")
                .Append(" FROM ")
                .Append("     TB_M_CUSTOMER A ")
                .Append(" WHERE ")
                .Append("     A.CST_ID = :ORIGINALID ")
            End With
            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            Using query As New DBSelectQuery( _
                Of IC3060102DataSet.IC3060102CustomerNameDataTable) _
                ("IC3060102_008")

                query.CommandText = sql.ToString()

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalId)
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                '終了ログ出力
                Dim dt As IC3060102DataSet.IC3060102CustomerNameDataTable = query.GetData()
                Dim count As Integer = dt.Count
                Logger.Info(getReturnParam("DataTable Count:" & CStr(count)), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)

                Return dt

            End Using

        End Function

#End Region

#Region "009.未取引客お客様名取得"

        ''' <summary>
        ''' 009.未取引客お客様名取得
        ''' </summary>
        ''' <param name="cstId">未取引客ユーザID</param>
        ''' <returns>お客様名DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetNewCustomerNameDataTable( _
            ByVal cstId As String) _
            As IC3060102DataSet.IC3060102CustomerNameDataTable
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("cstId", cstId, False), True)

            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3060102_009 */ ")
                .Append("     A.CST_NAME  AS NAME ")
                .Append(" FROM ")
                .Append("     TB_M_CUSTOMER A ")
                .Append(" WHERE ")
                .Append("     A.CST_ID = :CSTID ")
            End With
            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            Using query As New DBSelectQuery( _
                Of IC3060102DataSet.IC3060102CustomerNameDataTable) _
                ("IC3060102_009")

                query.CommandText = sql.ToString()

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstId)
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                '終了ログ出力
                Dim dt As IC3060102DataSet.IC3060102CustomerNameDataTable = query.GetData()
                Dim count As Integer = dt.Count
                Logger.Info(getReturnParam("DataTable Count:" & CStr(count)), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)

                Return dt

            End Using

        End Function

#End Region

#Region "010.副顧客お客様名取得"

        ''' <summary>
        ''' 010.副顧客お客様名取得
        ''' </summary>
        ''' <param name="subCustId">副顧客コード</param>
        ''' <returns>副顧客情報DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetSubCustomerNameDataTable( _
            ByVal subCustId As String) _
            As IC3060102DataSet.IC3060102CustomerNameDataTable
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("subCustId", subCustId, False), True)

            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* IC3060102_010 */ ")
                .Append("     A.CST_NAME  AS NAME ")
                .Append(" FROM ")
                .Append("     TB_M_CUSTOMER A ")
                .Append(" WHERE ")
                .Append("     A.CST_ID = :SUBCUSTID ")
            End With
            '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

            Using query As New DBSelectQuery( _
                Of IC3060102DataSet.IC3060102CustomerNameDataTable) _
                ("IC3060102_010")

                query.CommandText = sql.ToString()

                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                query.AddParameterWithTypeValue("SUBCUSTID", OracleDbType.Decimal, subCustId)
                '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                '終了ログ出力
                Dim dt As IC3060102DataSet.IC3060102CustomerNameDataTable = query.GetData()
                Dim count As Integer = dt.Count
                Logger.Info(getReturnParam("DataTable Count:" & CStr(count)), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)

                Return dt

            End Using

        End Function

#End Region

#Region "011.対応中査定依頼一覧情報取得"

        ''' <summary>
        ''' 011.対応中査定依頼一覧情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="clientId">端末ID</param>
        ''' <param name="sendDateFrom">送信日時From</param>
        ''' <param name="sendDateTo">送信日時To</param>
        ''' <param name="status">ステータス</param>
        ''' <returns>査定依頼一覧情報DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetInProgressAssessmentReqListInfoDataTable( _
            ByVal dealerCode As String, _
            ByVal storeCode As String, _
            ByVal clientId As String, _
            ByVal sendDateFrom As Date, _
            ByVal sendDateTo As Date, _
            ByVal status As String) _
            As IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("dealerCode", dealerCode, False), True)
            Logger.Info(getLogParam("storeCode", storeCode, True), True)
            Logger.Info(getLogParam("clientId", clientId, True), True)
            Logger.Info(getLogParam("sendDateFrom", CStr(sendDateFrom), True), True)
            Logger.Info(getLogParam("sendDateTo", CStr(sendDateTo), True), True)
            Logger.Info(getLogParam("status", CStr(status), True), True)

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3060102_011 */ ")
                .Append("       T1.NOTICEREQID ")
                .Append("     , T1.REQCLASSID ")
                .Append("     , T1.CRCUSTID ")
                .Append("     , T1.CUSTOMERCLASS ")
                .Append("     , T1.CSTKIND ")
                .Append("     , T1.STATUS ")
                .Append("     , T1.CUSTOMNAME ")
                .Append("     , T2.TOACCOUNT AS FROMACCOUNT ")
                .Append("     , T2.TOCLIENTID AS FROMCLIENTID ")
                .Append("     , T2.TOACCOUNTNAME AS FROMACCOUNTNAME ")
                .Append("     , T2.SENDDATE ")
                .Append("     , T3.RETENTION ")
                .Append("     , T3.ORGCSTVCL_VIN AS VIN ")
                .Append("     , T3.NEWCSTVCL_SEQNO ")
                .Append("     , T3.MAKERNAME ")
                .Append("     , T3.VEHICLENAME AS SERIESNM")
                .Append("     , T3.REGISTRATIONNO AS VCLREGNO ")
                .Append("     , T4.SALESTABLENO ")
                .Append("  FROM TBL_NOTICEREQUEST T1 ")
                .Append("     , TBL_NOTICEINFO T2 ")
                .Append("     , TBL_UCARASSESSMENT T3 ")
                .Append("     , ( ")
                .Append("    SELECT DLRCD ")
                .Append("         , STRCD ")
                .Append("         , VCLREGNO ")
                .Append("         , CUSTID ")
                .Append("         , SALESTABLENO ")
                .Append("         , ROW_NUMBER() ")
                .Append("               OVER(PARTITION BY DLRCD ")
                .Append("                               , STRCD ")
                .Append("                               , CUSTID ")
                .Append("               ORDER BY VISITSEQ DESC) AS ROWNO ")
                .Append("      FROM TBL_VISIT_SALES ")
                .Append("     WHERE DLRCD = :DLRCD ")
                .Append("       AND STRCD = :STRCD ")
                .Append("       AND VISITSTATUS = '07' ")
                .Append("       ) T4 ")
                .Append(" WHERE T1.NOTICEREQID = T2.NOTICEREQID ")
                .Append("   AND T1.LASTNOTICEID = T2.NOTICEID ")
                .Append("   AND T1.REQCLASSID = T3.ASSESSMENTNO ")
                .Append("   AND T1.DLRCD = T4.DLRCD(+) ")
                .Append("   AND T1.STRCD = T4.STRCD(+) ")
                .Append("   AND T1.CRCUSTID = T4.CUSTID(+) ")
                .Append("   AND T1.NOTICEREQCTG = '01' ")
                .Append("   AND T1.DLRCD = :DLRCD ")
                .Append("   AND T1.STRCD = :STRCD ")
                .Append("   AND T1.STATUS = :STATUS ")
                .Append("   AND T2.FROMCLIENTID = :CLIENTID ")
                .Append("   AND T2.SENDDATE BETWEEN :SENDDATEFROM AND :SENDDATETO ")
                .Append("   AND T2.STATUS = :STATUS ")
                .Append("   AND T4.ROWNO(+) = 1 ")
                .Append(" ORDER BY T2.SENDDATE ")
                .Append("        , T1.NOTICEREQID ")
            End With

            Using query As New DBSelectQuery( _
                Of IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable) _
                ("IC3060102_011")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("CLIENTID", OracleDbType.Varchar2, clientId)
                query.AddParameterWithTypeValue("SENDDATEFROM", OracleDbType.Date, sendDateFrom)
                query.AddParameterWithTypeValue("SENDDATETO", OracleDbType.Date, sendDateTo)
                query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, status)

                '終了ログ出力
                Dim dt As IC3060102DataSet.IC3060102AssessmentReqListInfoDataTable = query.GetData()
                Dim count As Integer = dt.Count
                Logger.Info(getReturnParam("DataTable Count:" & CStr(count)), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)
                Return dt

            End Using

        End Function

#End Region

#Region "012.受付者アカウント名取得"

        ''' <summary>
        ''' 012.受付者アカウント名取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="noticeReqId">通知依頼ID</param>
        ''' <returns>アカウント名DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetReceptionistAccountNameDataTable( _
            ByVal dealerCode As String, _
            ByVal storeCode As String, _
            ByVal noticeReqId As Long) _
            As IC3060102DataSet.IC3060102NoticeFromAccountNameDataTable
            '開始ログ出力
            Logger.Info(getLogMethod(GetCurrentMethod.Name, True), True)
            Logger.Info(getLogParam("dealerCode", dealerCode, False), True)
            Logger.Info(getLogParam("storeCode", storeCode, True), True)
            Logger.Info(getLogParam("noticeReqId", CStr(noticeReqId), True), True)

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3060102_012 */ ")
                .Append("       T3.FROMACCOUNTNAME ")
                .Append("  FROM ( ")
                .Append("    SELECT T2.FROMACCOUNTNAME ")
                .Append("         , ROW_NUMBER() ")
                .Append("               OVER(PARTITION BY T1.NOTICEREQID ")
                .Append("               ORDER BY T2.NOTICEID) AS ROWNO ")
                .Append("      FROM TBL_NOTICEREQUEST T1 ")
                .Append("         , TBL_NOTICEINFO T2 ")
                .Append("     WHERE T1.NOTICEREQID = T2.NOTICEREQID ")
                .Append("       AND T1.NOTICEREQID = :NOTICEREQID ")
                .Append("       AND T1.NOTICEREQCTG = '01' ")
                .Append("       AND T1.DLRCD = :DLRCD ")
                .Append("       AND T1.STRCD = :STRCD ")
                .Append("       AND T2.STATUS = '4' ")
                .Append("       ) T3 ")
                .Append(" WHERE T3.ROWNO = 1 ")
            End With

            Using query As New DBSelectQuery(
                Of IC3060102DataSet.IC3060102NoticeFromAccountNameDataTable) _
                ("IC3060102_012")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Long, noticeReqId)

                '終了ログ出力
                Dim dt As IC3060102DataSet.IC3060102NoticeFromAccountNameDataTable = query.GetData()
                Dim count As Integer = dt.Count
                Logger.Info(getReturnParam("DataTable Count:" & CStr(count)), True)
                Logger.Info(getLogMethod(GetCurrentMethod.Name, False), True)

                Return dt

            End Using

        End Function

#End Region


#Region "ログデータ加工処理"

        ''' <summary>
        ''' ログデータ（メソッド）
        ''' </summary>
        ''' <param name="methodName">メソッド名</param>
        ''' <param name="startEndFlag">True：「method start」を表示、False：「method end」を表示</param>
        ''' <returns>加工した文字列</returns>
        ''' <remarks></remarks>
        Private Function getLogMethod(ByVal methodName As String,
                                      ByVal startEndFlag As Boolean) As String
            Dim sb As New StringBuilder
            With sb
                .Append("[")
                .Append(methodName)
                .Append("]")
                If startEndFlag Then
                    .Append(" method start")
                Else
                    .Append(" method end")
                End If
            End With
            Return sb.ToString
        End Function

        ''' <summary>
        ''' ログデータ（引数）
        ''' </summary>
        ''' <param name="paramName">引数名</param>
        ''' <param name="paramData">引数値</param>
        ''' <param name="kanmaFlag">True：引数名の前に「,」を表示、False：特になし</param>
        ''' <returns>加工した文字列</returns>
        ''' <remarks></remarks>
        Private Function getLogParam(ByVal paramName As String,
                                     ByVal paramData As String,
                                     ByVal kanmaFlag As Boolean) As String
            Dim sb As New StringBuilder
            With sb
                If kanmaFlag Then
                    .Append(",")
                End If
                .Append(paramName)
                .Append("=")
                .Append(paramData)
            End With
            Return sb.ToString
        End Function

        ''' <summary>
        ''' ログデータ（戻り値）
        ''' </summary>
        ''' <param name="paramData">引数値</param>
        ''' <returns>加工した文字列</returns>
        ''' <remarks></remarks>
        Private Function getReturnParam(ByVal paramData As String) As String
            Dim sb As New StringBuilder
            With sb
                .Append("Return=")
                .Append(paramData)
            End With
            Return sb.ToString
        End Function
#End Region

    End Class

End Namespace
