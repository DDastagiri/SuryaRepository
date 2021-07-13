'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3802801TableAdapter.vb
'──────────────────────────────────
'機能： 見込み客情報送信I/F
'補足： 
'作成： yyyy/MM/dd KN  x.xxxxxx
'更新: 2018/07/05[SA01_LC_001] NSK Niiya Next Gen e-CRB Project Application development Block B-2 SI No.1,2,4,5,6 $01
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'更新: 2020/01/16[SA01_LC_002] NSK Natsume TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) $02
'──────────────────────────────────

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.iCROP.DataAccess.IC3802801.IC3802801DataSet

' For return value																									
Public Enum ReturnCode
    Success = 0
    XmlIncorrect = -1
    MandatoryItemError = 2000
    ItemTypeError = 3000
    ItemSizeError = 4000
    ValueError = 5000
    DataBaseError = 9000
    SystemError = 9999
End Enum

Public Class IC3802801TableAdapter

    'Method ID (ReturnCode(Sequence))														
    Public Enum DataBaseErrorCode
        GetSalesSqlError = 1
        GetSalesTempSqlError = 2
        GetEstimateInfoSqlError = 3
        GetFollowUpRequestSqlError = 4
        GetRequestActionSqlError = 5
        GetLastRequestActionIdSqlError = 6
        GetFirstRequestActionSqlError = 7
        GetStatusRequestActionSqlError = 8
        GetFollowUpAttractSqlError = 9
        GetAttractActionSqlError = 10
        GetLastAttractActionIdSqlError = 11
        GetFirstAttractActionSqlError = 12
        GetStatusAttractActionSqlError = 13
        GetFllwUpBoxSalesSqlError = 14
        GetActionMemoSqlError = 15
        GetSalesActionSqlError = 17
        GetSelectedSeriesSqlError = 18
        GetActionResultSqlError = 19
        GetCompetitorSeriesSqlError = 20
        GetMakerModelSqlError = 21
        GetActionSqlError = 22
        GetSalesConditionSqlError = 23
        GetDlrCstVclSqlError = 24
        GetVehicleSqlError = 25
        GetCustomerSqlError = 26
        GetDlrCustomerMemoSqlError = 27
        GetContactTimeslotSqlError = 29
        GetFamilyInfomationSqlError = 30
        GetHobbySqlError = 31
        GetSystemSettingSqlError = 32
        GetSystemSettingEnvSqlError = 33
        GetDmsCd1SqlError = 34
        GetIcropCd1SqlError = 35
        GetDmsCd2SqlError = 36
        GetIcropCd2SqlError = 37
        GetDmsCd3SqlError = 38
        GetIcropCd3SqlError = 39
        GetActionSeqSqlError = 40
        GetLastActionSeqSqlError = 41
        InsertActionSeqDataAdapterSqlError = 42
        MoveActionSeqSqlError = 43
        DeleteActionSeqSqlError = 44
        'ISSUE-0023_20130219_by_chatchai_Start
        GetReqSource1SqlError = 45
        GetReqSource2SqlError = 46
        GetStateInfoSqlError = 47
        GetDistrictInfoSqlError = 48
        GetCityInfoSqlError = 49
        GetLocationInfoSqlError = 50
        'ISSUE-0023_20130219_by_chatchai_End
        GetEstimateVclInfoSqlError = 51
        '$01 start
        GetCustomerLocalSqlError = 52
        GetVehicleDlrLocalSqlError = 53
        GetSalesLocalSqlError = 54
        '$01 end
    End Enum

    '$01 start
    Private Const OrganizationInputTypeMaster As String = "1"
    Private Const OrganizationInputTypeManual As String = "2"
    '$01 end

    ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
    'Public Function GetSystemSetting(ByVal DlrCd As String, ByVal BranchCd As String, ByVal SettingName As String) As IC3802801DataSet.IC3802801SystemSettingDataTable

    '    'SQL execution query instance creation																								
    '    Dim query As New DBSelectQuery(Of IC3802801SystemSettingDataTable)("IC3802801")

    '    Try
    '        'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
    '        Dim sql As New StringBuilder
    '        With sql
    '            .Append("SELECT ")
    '            .Append("DLR_CD ")
    '            .Append(",BRN_CD ")
    '            .Append(",SETTING_NAME ")
    '            .Append(",SETTING_VAL ")
    '            .Append("FROM ")
    '            .Append("TB_M_SYSTEM_SETTING_DLR  ")
    '            .Append("WHERE ")
    '            .Append("DLR_CD			=	:bindDlrCd	 ")
    '            .Append("AND BRN_CD		=	:bindBranchCd ")
    '            .Append("AND SETTING_NAME	=	:bindSettingName ")
    '        End With

    '        'Bind setting of condition																								
    '        query.AddParameterWithTypeValue("bindDlrCd", OracleDbType.NVarchar2, DlrCd)
    '        query.AddParameterWithTypeValue("bindBranchCd", OracleDbType.NVarchar2, BranchCd)
    '        query.AddParameterWithTypeValue("bindSettingName", OracleDbType.NVarchar2, SettingName)

    '        'Return SQL execution & execution result																								
    '        query.CommandText() = sql.ToString()
    '        Return query.GetData()

    '    Catch ex As SystemException
    '        Logger.Error(ex.Message, ex)
    '        Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetSystemSettingSqlError)

    '    End Try

    'End Function

    'Public Function GetSystemSettingEnv(CountryCode As String, ByVal ParamName As String) As IC3802801DataSet.IC3802801SystemSettingEnvDataTable

    '    'SQL execution query instance creation																								
    '    Dim query As New DBSelectQuery(Of IC3802801SystemSettingEnvDataTable)("IC3802801")

    '    Try
    '        'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
    '        Dim sql As New StringBuilder
    '        With sql
    '            .Append("SELECT ")
    '            .Append("ID ")
    '            .Append(",CNTCD ")
    '            .Append(",PARAMNAME ")
    '            .Append(",PARAMVALUE ")
    '            .Append("FROM ")
    '            .Append("TBL_SYSTEMENVSETTING ")
    '            .Append("WHERE ")
    '            .Append("CNTCD	=	:bindCntCd ")
    '            .Append(" AND PARAMNAME	=	:bindParamName ")
    '        End With

    '        'Bind setting of condition field
    '        query.AddParameterWithTypeValue("bindCntCd", OracleDbType.NVarchar2, CountryCode)
    '        query.AddParameterWithTypeValue("bindParamName", OracleDbType.NVarchar2, ParamName)

    '        'Return SQL execution & execution result																								
    '        query.CommandText() = sql.ToString()
    '        Return query.GetData()

    '    Catch ex As SystemException
    '        Logger.Error(ex.Message, ex)
    '        Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetSystemSettingEnvSqlError)

    '    End Try

    'End Function
    ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

    Public Function GetSales(ByVal SalesId As String) As IC3802801DataSet.IC3802801SalesDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801SalesDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("SALES_ID ")
                .Append(",DLR_CD ")
                .Append(",BRN_CD ")
                .Append(",CST_ID ")
                .Append(",SALES_PROSPECT_CD ")
                .Append(",REQ_ID ")
                .Append(",ATT_ID ")
                .Append(",ORIGIN_SALES_ID ")
                .Append(",SALES_TARGET_DATE ")
                .Append(",SALES_COMPLETE_FLG ")
                .Append(",DIRECT_SALES_FLG ")
                .Append(",GIVEUP_COMP_VCL_SEQ ")
                .Append(",GIVEUP_REASON ")
                .Append(",ROW_CREATE_DATETIME ")
                .Append(",ROW_CREATE_ACCOUNT ")
                .Append(",ACARD_NUM ")
                .Append(",BRAND_RECOGNITION_ID ")
                .Append(",SALES_START_DATE ")
                .Append(",DEMAND_STRUCTURE ")
                .Append("FROM ")
                .Append("TB_T_SALES ")
                .Append("WHERE ")
                .Append("SALES_ID 			=	:bindSalesId ")
            End With

            'Bind setting of condition																							
            query.AddParameterWithTypeValue("bindSalesId", OracleDbType.Decimal, SalesId)

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetSalesSqlError)

        End Try
    End Function

    Public Function GetSalesTemp(ByVal SalesId As String) As IC3802801DataSet.IC3802801SalesTempDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801SalesTempDataTable)("IC3802801")
        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("SALES_ID ")
                .Append(",SOURCE_1_CD ")
                .Append(",BRAND_RECOGNITION_ID ")
                .Append(",ACARD_NUM ")
                .Append(",ROW_CREATE_DATETIME ")
                .Append(",ROW_CREATE_ACCOUNT ")
                .Append(",ROW_UPDATE_DATETIME ")
                .Append(",ROW_UPDATE_ACCOUNT ")
                .Append("FROM ")
                .Append("TB_T_SALES_TEMP ")
                .Append("WHERE ")
                .Append("SALES_ID		=	:bindSalesId ")
            End With

            'Bind setting of condition																							
            query.AddParameterWithTypeValue("bindSalesId", OracleDbType.Decimal, SalesId)

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetSalesTempSqlError)

        End Try
    End Function

    'ISSUE08_20130217_by_takeda_Start
    'Public Function GetEstimateInfo(ByVal EstimateId As String) As IC3802801DataSet.IC3802801EstimateInfoDataTable
    Public Function GetEstimateInfo(ByVal DlrCd As String, ByVal StrCd As String, ByVal SalesId As String, ByVal DelFlg As String) As IC3802801DataSet.IC3802801EstimateInfoDataTable
        'ISSUE08_20130217_by_takeda_End

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801EstimateInfoDataTable)("IC3802801")
        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("ESTIMATEID  ")
                .Append(",DLRCD ")
                .Append(",STRCD ")
                .Append(",FLLWUPBOX_SEQNO ")
                .Append(",CNT_STRCD ")
                .Append(",CNT_STAFF ")
                .Append(",CSTKIND ")
                .Append(",CUSTOMERCLASS ")
                .Append(",CRCUSTID ")
                .Append(",CUSTID ")
                .Append(",CONTRACT_APPROVAL_REQUESTDATE ")
                .Append("FROM ")
                .Append("TBL_ESTIMATEINFO ")
                .Append("WHERE ")
                'ISSUE08_20130217_by_takeda_Start
                '.Append("ESTIMATEID	=	:bindEstimateId ")
                .Append("DLRCD	=	:bindDlrCd ")
                .Append("AND STRCD	=	:bindStrCd ")
                .Append("AND FLLWUPBOX_SEQNO	=	:bindSalesId ")
                .Append("AND DELFLG	=	:bindDelFlg ")
                'ISSUE08_20130217_by_takeda_End
            End With

            'ISSUE08_20130217_by_takeda_Start
            ''Bind setting of condition																							
            query.AddParameterWithTypeValue("bindDlrCd", OracleDbType.NVarchar2, DlrCd.ToString().PadRight(5, " "))
            query.AddParameterWithTypeValue("bindStrCd", OracleDbType.NVarchar2, StrCd.ToString().PadRight(3, " "))
            query.AddParameterWithTypeValue("bindSalesId", OracleDbType.Decimal, SalesId)
            query.AddParameterWithTypeValue("bindDelFlg", OracleDbType.Char, DelFlg)
            'ISSUE08_20130217_by_takeda_End

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetEstimateInfoSqlError)

        End Try
    End Function

    Public Function GetFollowUpRequest(ByVal ReqId As Long) As IC3802801DataSet.IC3802801FollowUpRequestDataTable

        'SQL execution query instance creation																						
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801FollowUpRequestDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT	")
                .Append("RQ.REQ_ID ")
                .Append(",RQ.SOURCE_1_CD ")
                .Append(",RQ.SOURCE_2_CD ")
                .Append(",RQ.LAST_ACT_ID ")
                .Append("FROM ")
                .Append("TB_T_REQUEST RQ ")
                .Append("WHERE ")
                .Append("RQ.REQ_ID=:bindReqId ")
            End With

            'Bind setting of condition																						
            query.AddParameterWithTypeValue("bindReqId", OracleDbType.Decimal, ReqId)

            'Return SQL execution & execution result																						
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetFollowUpRequestSqlError)

        End Try

    End Function

    Public Function GetRequestAction(ByVal ReqId As Long) As IC3802801DataSet.IC3802801ActionDataTable

        'SQL execution query instance creation																						
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ActionDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("AT.ACT_ID ")
                .Append(",AT.REQ_ID ")
                .Append(",AT.ATT_ID ")
                .Append(",AT.ACT_COUNT ")
                .Append(",AT.SCHE_DATEORTIME ")
                .Append(",AT.SCHE_DLR_CD ")
                .Append(",AT.SCHE_BRN_CD ")
                .Append(",AT.SCHE_STF_CD ")
                .Append(",AT.SCHE_CONTACT_MTD ")
                .Append(",AT.RSLT_FLG ")
                .Append(",AT.RSLT_DATETIME ")
                .Append(",AT.RSLT_DLR_CD ")
                .Append(",AT.RSLT_BRN_CD ")
                .Append(",AT.RSLT_STF_CD ")
                .Append(",AT.RSLT_CONTACT_MTD ")
                .Append(",AT.ACT_STATUS ")
                .Append(",AT.RSLT_ID ")
                .Append(",AT.RSLT_SALES_PROSPECT_CD ")
                .Append(",AT.ROW_CREATE_DATETIME ")
                .Append(",AT.ROW_CREATE_ACCOUNT ")
                .Append(",AT.ROW_UPDATE_DATETIME ")
                .Append(",AT.ROW_UPDATE_ACCOUNT ")
                .Append("FROM ")
                .Append("TB_T_ACTIVITY AT  ")
                .Append("WHERE ")
                .Append("AT.REQ_ID		=	:bindReqId	 ")
                .Append("ORDER BY AT.ACT_COUNT Asc ")
            End With

            'Bind setting of condition																						
            query.AddParameterWithTypeValue("bindReqId", OracleDbType.Decimal, ReqId)

            'Return SQL execution & execution result																						
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetRequestActionSqlError)


        End Try

    End Function

    Public Function GetFirstRequestAction(ByVal ReqId As Long, ByVal RsltSalesProspectCd As String) As IC3802801DataSet.IC3802801ActionDataTable

        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ActionDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT	")
                .Append("AT.RSLT_DATETIME	")
                .Append("FROM	")
                .Append("TB_T_ACTIVITY AT	")
                .Append("WHERE	")
                .Append("AT.REQ_ID					=	:bindReqId	")
                .Append("AND AT.RSLT_SALES_PROSPECT_CD	<>	:bindRsltSalesProspectCd	")
                .Append("ORDER BY  AT.ACT_COUNT Asc ")
            End With

            'Bind setting of condition																								
            query.AddParameterWithTypeValue("bindReqId", OracleDbType.Decimal, ReqId)
            query.AddParameterWithTypeValue("bindRsltSalesProspectCd", OracleDbType.NVarchar2, RsltSalesProspectCd)

            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetFirstRequestActionSqlError)

        End Try

    End Function

    Public Function GetStatusRequestAction(ByVal ReqId As Long, ByVal RsltSalesProspectCd As String, ByVal RsltFlg As String) As IC3802801DataSet.IC3802801ActionDataTable

        'SQL execution query instance creation																									
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ActionDataTable)("IC3802801")

        Dim ListRsltSalesProspectCd As String() = RsltSalesProspectCd.Split(New Char() {","c})
        RsltSalesProspectCd = ""
        For Each SalesProspectCd In ListRsltSalesProspectCd
            If (RsltSalesProspectCd = "") Then
                RsltSalesProspectCd = RsltSalesProspectCd + "'" + SalesProspectCd + "'"
            Else
                RsltSalesProspectCd = RsltSalesProspectCd + ",'" + SalesProspectCd + "'"
            End If
        Next

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT	")
                .Append("AT.RSLT_SALES_PROSPECT_CD ")
                .Append(",AT.RSLT_DATETIME	")
                .Append("FROM	")
                .Append("TB_T_ACTIVITY AT 	")
                .Append(",(	SELECT	")
                .Append("REQ_ID	")
                .Append(",RSLT_SALES_PROSPECT_CD	")
                .Append(",MAX(ACT_COUNT) AS ACT_COUNT	")
                .Append("FROM	")
                .Append("TB_T_ACTIVITY	")
                .Append("WHERE	")
                .Append("REQ_ID	=	:bindReqId	AND ")
                .Append("RSLT_SALES_PROSPECT_CD	IN( " + RsltSalesProspectCd + " )	")
                .Append("AND RSLT_FLG = :bindRsltFlg ")
                .Append("GROUP BY 	")
                .Append("REQ_ID	")
                .Append(",RSLT_SALES_PROSPECT_CD	")
                .Append(") AT_STATUS ")
                .Append("WHERE	")
                .Append("AT.REQ_ID	=	:bindReqId AND ")
                .Append("AT.REQ_ID = AT_STATUS.REQ_ID AND ")
                .Append("AT.RSLT_SALES_PROSPECT_CD = AT_STATUS.RSLT_SALES_PROSPECT_CD AND ")
                .Append("AT.ACT_COUNT = AT_STATUS.ACT_COUNT	")
            End With

            'Bind setting of condition																									
            query.AddParameterWithTypeValue("bindReqId", OracleDbType.Decimal, ReqId)
            query.AddParameterWithTypeValue("bindRsltFlg", OracleDbType.NVarchar2, RsltFlg)

            'Return SQL execution & execution result																									
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetStatusRequestActionSqlError)

        End Try

    End Function

    Public Function GetFollowUpAttract(ByVal AttId As Long) As IC3802801DataSet.IC3802801FollowUpAttractDataTable

        'SQL execution query instance creation																						
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801FollowUpAttractDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT	")
                .Append("AT.ATT_ID ")
                .Append(",AT.SOURCE_1_CD ")
                .Append(",AT.SOURCE_2_CD ")
                .Append(",AT.LAST_ACT_ID ")
                .Append(",AT.VCL_ID ")
                .Append("FROM ")
                .Append("TB_T_ATTRACT AT ")
                .Append("WHERE ")
                .Append("AT.ATT_ID =	:bindAttId ")
            End With

            'Bind setting of condition																						
            query.AddParameterWithTypeValue("bindAttId", OracleDbType.Decimal, AttId)

            'Return SQL execution & execution result																						
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetFollowUpAttractSqlError)

        End Try
    End Function

    Public Function GetAttractAction(ByVal AttId As Long) As IC3802801DataSet.IC3802801ActionDataTable

        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ActionDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("AT.ACT_ID ")
                .Append(",AT.REQ_ID	 ")
                .Append(",AT.ATT_ID ")
                .Append(",AT.ACT_COUNT ")
                .Append(",AT.SCHE_DATEORTIME ")
                .Append(",AT.SCHE_DLR_CD ")
                .Append(",AT.SCHE_BRN_CD ")
                .Append(",AT.SCHE_STF_CD ")
                .Append(",AT.SCHE_CONTACT_MTD ")
                .Append(",AT.RSLT_FLG ")
                .Append(",AT.RSLT_DATETIME ")
                .Append(",AT.RSLT_DLR_CD ")
                .Append(",AT.RSLT_BRN_CD ")
                .Append(",AT.RSLT_STF_CD ")
                .Append(",AT.RSLT_CONTACT_MTD ")
                .Append(",AT.ACT_STATUS ")
                .Append(",AT.RSLT_ID ")
                .Append(",AT.RSLT_SALES_PROSPECT_CD ")
                .Append(",AT.ROW_CREATE_DATETIME ")
                .Append(",AT.ROW_CREATE_ACCOUNT ")
                .Append(",AT.ROW_UPDATE_DATETIME ")
                .Append(",AT.ROW_UPDATE_ACCOUNT ")
                .Append("FROM ")
                .Append("TB_T_ACTIVITY AT ")
                .Append("WHERE ")
                .Append("AT.ATT_ID		=	:bindAttId ")
                .Append("ORDER BY ")
                .Append("AT.ACT_COUNT Asc ")
            End With

            'Bind setting of condition																								
            query.AddParameterWithTypeValue("bindAttId", OracleDbType.Decimal, AttId)

            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetAttractActionSqlError)

        End Try

    End Function

    Public Function GetFirstAttractAction(ByVal AttId As Long, ByVal RsltSalesProspectCd As String) As IC3802801DataSet.IC3802801ActionDataTable

        'SQL execution query instance creation																									
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ActionDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("AT.RSLT_DATETIME ")
                .Append("FROM ")
                .Append("TB_T_ACTIVITY AT ")
                .Append("WHERE ")
                .Append("AT.ATT_ID	=	:bindAttId ")
                .Append("AND AT.RSLT_SALES_PROSPECT_CD <>	:bindRsltSalesProspectCd ")
                .Append("ORDER BY ")
                .Append("AT.ACT_COUNT Asc ")
            End With

            'Bind setting of condition																									
            query.AddParameterWithTypeValue("bindAttId", OracleDbType.Decimal, AttId)
            query.AddParameterWithTypeValue("bindRsltSalesProspectCd", OracleDbType.NVarchar2, RsltSalesProspectCd)

            'Return SQL execution & execution result																									
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetFirstAttractActionSqlError)

        End Try

    End Function

    Public Function GetStatusAttractAction(ByVal AttId As Long, ByVal RsltSalesProspectCd As String, ByVal RsltFlg As String) As IC3802801DataSet.IC3802801ActionDataTable

        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ActionDataTable)("IC3802801")
        Dim ListRsltSalesProspectCd As String() = RsltSalesProspectCd.Split(New Char() {","c})
        RsltSalesProspectCd = ""
        For Each SalesProspectCd In ListRsltSalesProspectCd
            If (RsltSalesProspectCd = "") Then
                RsltSalesProspectCd = RsltSalesProspectCd + "'" + SalesProspectCd + "'"
            Else
                RsltSalesProspectCd = RsltSalesProspectCd + ",'" + SalesProspectCd + "'"
            End If
        Next

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("AT.RSLT_SALES_PROSPECT_CD ")
                .Append(",AT.RSLT_DATETIME  ")
                .Append("FROM ")
                .Append("TB_T_ACTIVITY AT ")
                .Append(",(	SELECT ")
                .Append("ATT_ID ")
                .Append(", RSLT_SALES_PROSPECT_CD ")
                .Append(", MAX(ACT_COUNT) AS ACT_COUNT ")
                .Append("FROM ")
                .Append("TB_T_ACTIVITY ")
                .Append("WHERE ")
                .Append("ATT_ID	 =	:bindAttId AND ")
                .Append("RSLT_SALES_PROSPECT_CD		IN( " + RsltSalesProspectCd + " )	 ")
                .Append("AND RSLT_FLG = :bindRsltFlg ")
                .Append("GROUP BY ")
                .Append("ATT_ID ")
                .Append(",RSLT_SALES_PROSPECT_CD ")
                .Append(") AT_STATUS ")
                .Append("WHERE ")
                .Append("AT.ATT_ID	=	:bindAttId and ")
                .Append("AT.ATT_ID = AT_STATUS.ATT_ID and ")
                .Append("AT.RSLT_SALES_PROSPECT_CD = AT_STATUS.RSLT_SALES_PROSPECT_CD and ")
                .Append("AT.ACT_COUNT = AT_STATUS.ACT_COUNT ")
            End With

            'Bind setting of condition																								
            query.AddParameterWithTypeValue("bindAttId", OracleDbType.Decimal, AttId)
            query.AddParameterWithTypeValue("bindRsltFlg", OracleDbType.NVarchar2, RsltFlg)

            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetStatusAttractActionSqlError)


        End Try

    End Function

    Public Function GetFllwUpBoxSales(ByVal DlrCd As String, ByVal StrCd As String, ByVal SalesId As Long, ByVal ActId As Long) As IC3802801DataSet.IC3802801GetFllwUpBoxSalesDataTable
        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801GetFllwUpBoxSalesDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("FS.DlrCd ")
                .Append(",FS.STRCD ")
                .Append(",FS.FLLWUPBOX_SEQNO ")
                .Append(",FS.CUSTSEGMENT ")
                .Append(",FS.CUSTOMERCLASS ")
                .Append(",FS.CRCUSTID ")
                .Append(",FS.STARTTIME ")
                .Append(",FS.ENDTIME ")
                .Append(",FS.REGISTFLG ")
                .Append(",SALES_SEQNO ")
                'ISSUE-0016,ISSUE-0017_by_chatchai_Start
                .Append("FROM ")
                .Append("TBL_FLLWUPBOX_SALES FS ")
                .Append(",TB_T_ACTIVITY AT ")
                .Append("WHERE ")
                '20140320 Fujita Upd Start
                '.Append("To_CHAR(FS.STARTTIME,'dd-mm-yyyy')  = To_CHAR(AT.RSLT_DATETIME,'dd-mm-yyyy') ")
                .Append("FS.STARTTIME  = AT.RSLT_DATETIME ")
                '20140320 Fujita Upd End
                .Append("AND FS.DLRCD				=	:bindDlrCd ")
                .Append("AND FS.STRCD				=	:bindStrCd ")
                .Append("AND FS.FLLWUPBOX_SEQNO		=	:bindFllwUpBoxSeqNo ")
                .Append("AND AT.ACT_ID				=	:bindSalesSeqNo ")
                'ISSUE-0016,ISSUE-0017_by_chatchai_End
            End With

            'Bind setting of condition																								
            query.AddParameterWithTypeValue("bindDlrCd", OracleDbType.NVarchar2, DlrCd.PadRight(5, " "))
            query.AddParameterWithTypeValue("bindStrCd", OracleDbType.NVarchar2, StrCd.PadRight(3, " "))
            query.AddParameterWithTypeValue("bindFllwUpBoxSeqNo", OracleDbType.Decimal, SalesId)
            query.AddParameterWithTypeValue("bindSalesSeqNo", OracleDbType.Decimal, ActId)

            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetFllwUpBoxSalesSqlError)

        End Try

    End Function

    'ISSUE-0023_20130219_by_chatchai_Start
    'Public Function GetReqSource(ByVal Source1stCd As Long, ByVal Source2ndCd As Long) As IC3802801DataSet.IC3802801ReqSourceDataTable

    '    'SQL execution query instance creation																					
    '    Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ReqSourceDataTable)("IC3802801")
    '    Try

    '        'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
    '        Dim sql As New StringBuilder
    '        With sql
    '            .Append("SELECT	")
    '            .Append("S1.SOURCE_1_CD ")
    '            .Append(",S2.SOURCE_2_CD ")
    '            .Append(",S1.SOURCE_1_NAME ")
    '            .Append(",S2.REQ_SECOND_CAT_NAME ")
    '            .Append("FROM ")
    '            .Append("TB_M_SOURCE_1 S1, ")
    '            .Append("TB_M_SOURCE_2 S2 ")
    '            .Append("WHERE ")
    '            .Append("S1.SOURCE_1_CD = S2.SOURCE_1_CD ")
    '            .Append("AND S1.SOURCE_1_CD	=:bindSrc1Cd ")
    '            .Append("AND S2.SOURCE_2_CD	=:bindSrc2Cd")
    '        End With

    '        'Bind setting of condition																					
    '        query.AddParameterWithTypeValue("bindSrc1Cd", OracleDbType.Decimal, Source1stCd)   'Parameter request source (1st) code)									
    '        query.AddParameterWithTypeValue("bindSrc2Cd", OracleDbType.Decimal, Source2ndCd)   'Parameter request source (2nd) code)									

    '        'Return SQL execution & execution result																					
    '        query.CommandText() = sql.ToString()
    '        Return query.GetData()

    '    Catch ex As SystemException
    '        Logger.Error(ex.Message, ex)
    '        Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetReqSourceSqlError)

    '    End Try
    'End Function
    'ISSUE-0023_20130219_by_chatchai_End

    'ISSUE-0023_20130219_by_chatchai_Start
    Public Function GetReqSource1(ByVal Source1stCd As Long) As IC3802801DataSet.IC3802801ReqSource1DataTable

        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ReqSource1DataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("S1.SOURCE_1_CD ")
                .Append(",S1.SOURCE_1_NAME ")
                .Append("FROM ")
                .Append("TB_M_SOURCE_1 S1 ")
                .Append("WHERE ")
                .Append("S1.SOURCE_1_CD	=:bindSrc1Cd ")
            End With

            'Bind setting of condition																								
            query.AddParameterWithTypeValue("bindSrc1Cd", OracleDbType.Decimal, Source1stCd)

            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetReqSource1SqlError)
        End Try
    End Function
    'ISSUE-0023_20130219_by_chatchai_End

    'ISSUE-0023_20130219_by_chatchai_Start
    Public Function GetReqSource2(ByVal Source1stCd As Long, ByVal Source2ndCd As Long) As IC3802801DataSet.IC3802801ReqSource2DataTable

        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ReqSource2DataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("S2.SOURCE_2_CD ")
                .Append(",S2.REQ_SECOND_CAT_NAME ")
                .Append("FROM ")
                .Append("TB_M_SOURCE_2 S2 ")
                .Append("WHERE ")
                .Append("S2.SOURCE_1_CD	=	:bindSrc1Cd ")
                .Append("And S2.SOURCE_2_CD	=	:bindSrc2Cd	")
            End With

            'Bind setting of condition																								
            query.AddParameterWithTypeValue("bindSrc1Cd", OracleDbType.Decimal, Source1stCd)
            query.AddParameterWithTypeValue("bindSrc2Cd", OracleDbType.Decimal, Source2ndCd)

            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetReqSource2SqlError)
        End Try

    End Function
    'ISSUE-0023_20130219_by_chatchai_End


    Public Function GetSalesAction(ByVal SalesId As Long, ByVal ActId As Long) As IC3802801DataSet.IC3802801SalesActionDataTable

        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801SalesActionDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("SALES_ACT_ID ")
                .Append(",SALES_ID ")
                .Append(",ACT_ID ")
                .Append(",RSLT_SALES_CAT ")
                .Append(",ROW_CREATE_ACCOUNT ")
                .Append("FROM ")
                .Append("TB_T_SALES_ACT ")
                .Append("WHERE ")
                .Append("SALES_ID =	:bindSalesId ")
                '$FSBT90,91,92_20130221_by_chatchai_Start
                .Append("AND ACT_ID =	:bindActId ")
                '$FSBT90,91,92_20130221_by_chatchai_End
                .Append("ORDER BY ACT_ID Asc")

            End With
            'Bind setting of condition																								
            query.AddParameterWithTypeValue("bindSalesId", OracleDbType.Decimal, SalesId)
            query.AddParameterWithTypeValue("bindActId", OracleDbType.Decimal, ActId)


            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetSalesActionSqlError)

        End Try

    End Function

    Public Function GetSelectedSeries(ByVal SalesId As String) As IC3802801DataSet.IC3802801SelectedSeriesDataTable

        'SQL execution query instance creation																					
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801SelectedSeriesDataTable)("IC3802801")
        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT	")
                .Append("SALES_ID ")
                .Append(",PREF_VCL_SEQ ")
                .Append(",MODEL_CD ")
                .Append(",GRADE_CD ")
                .Append(",SUFFIX_CD ")
                .Append(",BODYCLR_CD ")
                .Append(",INTERIORCLR_CD ")
                .Append(",PREF_AMOUNT ")
                .Append(",EST_AMOUNT ")
                .Append(",SALESBKG_ACT_ID ")
                .Append(",SALES_PROSPECT_CD ")
                .Append("FROM ")
                .Append("TB_T_PREFER_VCL ")
                .Append("WHERE ")
                .Append("SALES_ID =	:bindSalesId	")
            End With

            'Bind setting of condition																					
            query.AddParameterWithTypeValue("bindSalesId", OracleDbType.Decimal, SalesId)

            'Return SQL execution & execution result																					
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetSelectedSeriesSqlError)

        End Try
    End Function

    Public Function GetActionResult(ByVal ActRsltId As Long) As IC3802801DataSet.IC3802801ActionResultDataTable

        'SQL execution query instance creation																									
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ActionResultDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT	")
                .Append("AR.ACT_RSLT_ID	")
                .Append(",AR.DLR_CD	")
                .Append(",AR.BRN_CD	")
                .Append(",AR.RSLT_CAT_NAME	")
                .Append(",AR.RSLT_DESCRIPTION	")
                .Append(",AR.ROW_CREATE_DATETIME	")
                .Append(",AR.ROW_CREATE_ACCOUNT	")
                .Append("FROM	")
                .Append("TB_M_ACTIVITY_RESULT AR 	")
                .Append("WHERE	")
                .Append("AR.	ACT_RSLT_ID						=	:bindActRsltId		")
            End With

            'Bind setting of condition																									
            query.AddParameterWithTypeValue("bindActRsltId", OracleDbType.Decimal, ActRsltId)

            'Return SQL execution & execution result																									
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetActionResultSqlError)

        End Try

    End Function

    Public Function GetCompetitorSeries(ByVal SalesId As String) As IC3802801DataSet.IC3802801CompetitorSeriesDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801CompetitorSeriesDataTable)("IC3802801")
        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT	")
                .Append("CV.SALES_ID")
                .Append(",CV.COMP_VCL_SEQ	")
                .Append(",CV.MODEL_CD	")
                .Append("FROM ")
                .Append("TB_T_COMPETITOR_VCL CV ")
                .Append("WHERE	")
                .Append("CV.SALES_ID	=	:bindSalesId")
            End With

            'Bind setting of condition																							
            query.AddParameterWithTypeValue("bindSalesId", OracleDbType.Decimal, SalesId)               'Parameter sales ID											

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetCompetitorSeriesSqlError)

        End Try
    End Function

    Public Function GetMakerModel(ByVal ModelCd As String) As IC3802801DataSet.IC3802801MakerModelDataTable

        'SQL execution query instance creation																					
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801MakerModelDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT	")
                .Append("MO.MODEL_CD ")
                .Append(",MO.MAKER_CD ")
                .Append(",MO.MODEL_NAME ")
                .Append(",MA.MAKER_NAME ")
                .Append(" FROM	")
                .Append("TB_M_MODEL	MO,	")
                .Append("TB_M_MAKER	MA	")
                .Append("WHERE	")
                .Append("MO.MAKER_CD = MA.MAKER_CD ")
                .Append("AND MO.MODEL_CD	= :bindModelId ")
            End With

            'Bind setting of condition																					
            query.AddParameterWithTypeValue("bindModelId", OracleDbType.NVarchar2, ModelCd)

            'Return SQL execution & execution result																					
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetMakerModelSqlError)

        End Try
    End Function

    Public Function GetAction(ByVal ActId As String) As IC3802801DataSet.IC3802801ActionDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ActionDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT	")
                .Append("AT.ACT_ID ")
                .Append(",AT.REQ_ID ")
                .Append(",AT.ATT_ID ")
                .Append(",AT.ACT_COUNT ")
                .Append(",AT.SCHE_DATEORTIME ")
                .Append(",AT.SCHE_DLR_CD ")
                .Append(",AT.SCHE_BRN_CD ")
                .Append(",AT.SCHE_STF_CD ")
                .Append(",AT.SCHE_CONTACT_MTD ")
                .Append(",AT.RSLT_FLG ")
                .Append(",AT.RSLT_DATETIME ")
                .Append(",AT.RSLT_DLR_CD ")
                .Append(",AT.RSLT_BRN_CD ")
                .Append(",AT.RSLT_STF_CD ")
                .Append(",AT.RSLT_CONTACT_MTD ")
                .Append(",AT.ACT_STATUS ")
                .Append(",AT.RSLT_ID ")
                .Append(",AT.RSLT_SALES_PROSPECT_CD ")
                .Append(",AT.ROW_CREATE_DATETIME ")
                .Append(",AT.ROW_CREATE_ACCOUNT ")
                .Append(",AT.ROW_UPDATE_DATETIME ")
                .Append(",AT.ROW_UPDATE_ACCOUNT ")
                .Append("FROM	")
                .Append("TB_T_ACTIVITY	AT	")
                .Append("WHERE	")
                .Append("AT.ACT_ID		=	:bindActId	")
            End With

            'Bind setting of condition																							
            query.AddParameterWithTypeValue("bindActId", OracleDbType.Decimal, ActId)

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetActionSqlError)

        End Try
    End Function

    Public Function GetSalesCondition(ByVal DlrCd As String,
        ByVal StrId As String,
        ByVal SalesId As Long,
        ByVal CstId As String) As IC3802801DataSet.IC3802801SalesConditionDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801SalesConditionDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT	")
                .Append("FS.DLRCD ")
                .Append(",FS.STRCD ")
                .Append(",FS.FLLWUPBOX_SEQNO ")
                .Append(",FS.SALESCONDITIONNO ")
                .Append(",FS.ITEMNO	")
                .Append(",FS.OTHERSALESCONDITION ")
                .Append("FROM	")
                .Append("TBL_FLLWUPBOX_SALESCONDITION FS ")
                .Append("WHERE ")
                .Append("FS.DLRCD			=	:bindDlrCd	")
                .Append("AND FS.STRCD		=	:bindStrCd	")
                .Append("AND FS.FLLWUPBOX_SEQNO		=	:bindSalesId ")
                .Append("AND FS.CRCUSTID	=	:bindCstId	")
                .Append("ORDER BY 	")
                .Append("FS.SALESCONDITIONNO Asc 	")
                'takeda_update_start_20140606
                .Append(",FS.ITEMNO Asc 	")
                'takeda_update_end_20140606
            End With

            'Bind setting of condition																							
            query.AddParameterWithTypeValue("bindDlrCd", OracleDbType.NVarchar2, DlrCd.ToString().PadRight(5, " "))
            query.AddParameterWithTypeValue("bindStrCd", OracleDbType.NVarchar2, StrId.ToString().PadRight(3, " "))
            query.AddParameterWithTypeValue("bindSalesId", OracleDbType.Decimal, SalesId)
            query.AddParameterWithTypeValue("bindCstId", OracleDbType.NVarchar2, CstId.ToString().PadRight(20, " "))

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetSalesConditionSqlError)

        End Try
    End Function

    Public Function GetDlrCstVcl(ByVal DlrCd As String,
        ByVal CstId As Long,
        ByVal CstVclKbn As String,
        ByVal OwnerChgFlg As String) As IC3802801DataSet.IC3802801DlrCstVclDataTable

        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801DlrCstVclDataTable)("IC3802801")
        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT	")
                .Append("DLR_CD	")
                .Append(",CST_ID ")
                .Append(",VCL_ID ")
                .Append(",CST_VCL_TYPE	")
                .Append(",ACT_CAT_TYPE ")
                .Append(",SLS_PIC_BRN_CD ")
                .Append(",SLS_PIC_STF_CD ")
                .Append(",SVC_PIC_BRN_CD ")
                .Append(",SVC_PIC_STF_CD ")
                .Append("FROM ")
                .Append("TB_M_CUSTOMER_VCL ")
                .Append("WHERE ")
                .Append("DLR_CD				=:bindDlrCd ")
                .Append("AND CST_ID			=:bindCstId	")
                .Append("AND CST_VCL_TYPE	=:bindCstVclKbn	")
                '20140312 Fujita Upd Start
                '.Append("AND OWNER_CHG_FLG	=:bindOwnerChgFlg ")
                .Append("AND OWNER_CHG_FLG	<>:bindOwnerChgFlg ")
                '20140312 Fujita Upd End
            End With

            'Bind setting of condition																								
            query.AddParameterWithTypeValue("bindDlrCd", OracleDbType.NVarchar2, DlrCd)
            query.AddParameterWithTypeValue("bindCstId", OracleDbType.Decimal, CstId)
            query.AddParameterWithTypeValue("bindCstVclKbn", OracleDbType.NVarchar2, CstVclKbn)
            query.AddParameterWithTypeValue("bindOwnerChgFlg", OracleDbType.NVarchar2, OwnerChgFlg)

            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetDlrCstVclSqlError)

        End Try
    End Function

    Public Function GetVehicle(ByVal DlrCd As String,
        ByVal VclId As Long) As IC3802801DataSet.IC3802801VehicleDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801VehicleDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("VC.VCL_ID ")
                .Append(",VC.VCL_VIN ")
                .Append(",VC.MODEL_CD ")
                .Append(",VC.NEWCST_MODEL_NAME ")
                .Append(",VD.REG_DATE ")
                .Append(",VD.REG_NUM ")
                .Append(",VD.DELI_DATE ")
                .Append("FROM	")
                .Append("TB_M_VEHICLE	VC,	")
                .Append("TB_M_VEHICLE_DLR	VD	")
                .Append("WHERE	")
                .Append("VC.VCL_ID = VD.VCL_ID	")
                .Append("AND VC.VCL_ID	=	:bindVclId	")
                .Append("AND VD.DLR_CD	=	:bindDlrCd	")
            End With

            'Bind setting of condition																							
            query.AddParameterWithTypeValue("bindVclId", OracleDbType.Decimal, VclId)
            query.AddParameterWithTypeValue("bindDlrCd", OracleDbType.NVarchar2, DlrCd)

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetVehicleSqlError)

        End Try
    End Function

    Public Function GetActionMemo(ByVal RelationActType As String, ByVal RelationActId As Long) As IC3802801DataSet.IC3802801ActionMemoDataTable

        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ActionMemoDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("AM.ACT_MEMO_ID ")
                .Append(",AM.DLR_CD ")
                .Append(",AM.CST_ID	 ")
                .Append(",AM.VCL_ID ")
                .Append(",AM.CST_MEMO ")
                .Append(",AM.ROW_CREATE_DATETIME ")
                .Append(",AM.ROW_CREATE_ACCOUNT ")
                .Append(",AM.RELATION_ACT_ID ")
                .Append(",AM.RELATION_ACT_TYPE ")
                .Append(",AM.CREATE_DATETIME ")
                .Append(",AM.CREATE_STF_CD ")
                .Append("FROM ")
                .Append("TB_T_ACTIVITY_MEMO AM ")
                .Append("WHERE  ")
                .Append("AM.RELATION_ACT_TYPE	=	:bindRelationActType ")
                .Append("AND AM.RELATION_ACT_ID	=	:bindRelationActId	 ")
                .Append("ORDER BY AM.ACT_MEMO_ID Asc ")
            End With

            'Bind setting of condition																								
            query.AddParameterWithTypeValue("bindRelationActType", OracleDbType.NVarchar2, RelationActType)
            query.AddParameterWithTypeValue("bindRelationActId", OracleDbType.Decimal, RelationActId)

            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetActionMemoSqlError)

        End Try

    End Function

    Public Function GetDlrCustomerMemo(ByVal DlrCd As String, ByVal CstId As Long) As IC3802801DataSet.IC3802801CustomerMemoDataTable

        'SQL execution query instance creation																									
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801CustomerMemoDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT	")
                .Append("DLR_CD ")
                .Append(",CST_ID ")
                .Append(",CST_MEMO_SEQ ")
                .Append(",CST_MEMO ")
                .Append("FROM ")
                .Append("TB_T_CUSTOMER_MEMO	")
                .Append("WHERE	")
                .Append("DLR_CD	=	:bindDlrCd	")
                .Append("AND CST_ID =	:bindCstId ")
                .Append("ORDER BY ")
                .Append("CST_MEMO_SEQ DESC 	")
            End With

            'Bind setting of condition																									
            query.AddParameterWithTypeValue("bindDlrCd", OracleDbType.NVarchar2, DlrCd)
            query.AddParameterWithTypeValue("bindCstId", OracleDbType.Decimal, CstId)

            'Return SQL execution & execution result																									
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetDlrCustomerMemoSqlError)

        End Try

    End Function

    'ISSUE-0025_20130219_by_chatchai_Start
    Public Function GetStateInfo(ByVal StateCd As String) As IC3802801DataSet.IC3802801StateInfoDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801StateInfoDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("STATE_CD ")
                .Append(",STATE_NAME ")
                .Append("FROM ")
                .Append("TB_M_STATE ")
                .Append("WHERE ")
                .Append("STATE_CD	=	:bindStateCd ")
            End With

            'Bind setting of condition																							
            query.AddParameterWithTypeValue("bindStateCd", OracleDbType.NVarchar2, StateCd)
            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetStateInfoSqlError)
        End Try

    End Function
    'ISSUE-0025_20130219_by_chatchai_End

    'ISSUE-0025_20130219_by_chatchai_Start
    Public Function GetDistrictInfo(ByVal StateCd As String, ByVal DistrictCd As String) As IC3802801DataSet.IC3802801DistrictInfoDataTable
        'SQL execution query instance creation																									
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801DistrictInfoDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("STATE_CD ")
                .Append(",DISTRICT_CD ")
                .Append(",DISTRICT_NAME ")
                .Append("FROM ")
                .Append("TB_M_DISTRICT ")
                .Append("WHERE ")
                .Append("STATE_CD	     =	:bindStateCd ")
                .Append("AND DISTRICT_CD =	:bindDistrictCd ")
            End With

            'Bind setting of condition																									
            query.AddParameterWithTypeValue("bindStateCd", OracleDbType.NVarchar2, StateCd)
            query.AddParameterWithTypeValue("bindDistrictCd", OracleDbType.NVarchar2, DistrictCd)

            'Return SQL execution & execution result																									
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetDistrictInfoSqlError)
        End Try

    End Function
    'ISSUE-0025_20130219_by_chatchai_End

    'ISSUE-0025_20130219_by_chatchai_Start
    Public Function GetCityInfo(ByVal StateCd As String, ByVal DistrictCd As String, ByVal CityCd As String) As IC3802801DataSet.IC3802801CityInfoDataTable
        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801CityInfoDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("STATE_CD ")
                .Append(",DISTRICT_CD ")
                .Append(",CITY_CD ")
                .Append(",CITY_NAME ")
                .Append("FROM ")
                .Append("TB_M_CITY ")
                .Append("WHERE ")
                .Append("STATE_CD	=	:bindStateCd ")
                .Append("AND DISTRICT_CD =	:bindDistrictCd ")
                .Append("AND CITY_CD =	:bindCityCd ")
            End With

            'Bind setting of condition																							
            query.AddParameterWithTypeValue("bindStateCd", OracleDbType.NVarchar2, StateCd)
            query.AddParameterWithTypeValue("bindDistrictCd", OracleDbType.NVarchar2, DistrictCd)
            query.AddParameterWithTypeValue("bindCityCd", OracleDbType.NVarchar2, CityCd)

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetCityInfoSqlError)
        End Try

    End Function
    'ISSUE-0025_20130219_by_chatchai_End

    'ISSUE-0025_20130219_by_chatchai_Start
    Public Function GetLocationInfo(ByVal StateCd As String, ByVal DistrictCd As String, ByVal CityCd As String, ByVal LocationCd As String) As IC3802801DataSet.IC3802801LocationInfoDataTable
        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801LocationInfoDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("STATE_CD ")
                .Append(",DISTRICT_CD ")
                .Append(",CITY_CD ")
                .Append(",LOCATION_CD ")
                .Append(",LOCATION_NAME ")
                .Append("FROM ")
                .Append("TB_M_LOCATION ")
                .Append("WHERE ")
                .Append("STATE_CD =	:bindStateCd ")
                .Append("AND DISTRICT_CD	=	:bindDistrictCd ")
                .Append("AND CITY_CD        =	:bindCityCd ")
                .Append("AND LOCATION_CD	=	:bindLocationCd ")
            End With

            'Bind setting of condition																								
            query.AddParameterWithTypeValue("bindStateCd", OracleDbType.NVarchar2, StateCd)
            query.AddParameterWithTypeValue("bindDistrictCd", OracleDbType.NVarchar2, DistrictCd)
            query.AddParameterWithTypeValue("bindCityCd", OracleDbType.NVarchar2, CityCd)
            query.AddParameterWithTypeValue("bindLocationCd", OracleDbType.NVarchar2, LocationCd)

            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetLocationInfoSqlError)
        End Try

    End Function
    'ISSUE-0025_20130219_by_chatchai_End


    Public Function GetDmsCd1(ByVal DmsCdType As String, ByVal IcropCd1 As String) As IC3802801DataSet.IC3802801DmsCodeMapDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801DmsCodeMapDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("DMS_CD_1 ")
                .Append(",DMS_CD_2 ")
                .Append(",DMS_CD_3 ")
                .Append(",ICROP_CD_1 ")
                .Append(",ICROP_CD_2 ")
                .Append(",ICROP_CD_3 ")
                .Append("FROM ")
                .Append("TB_M_DMS_CODE_MAP ")
                .Append("WHERE ")
                .Append("DMS_CD_TYPE		=	:bindDmsCdType ")
                .Append("AND ICROP_CD_1		=	:bindIcropCd1 ")
            End With

            'Bind setting of condition																							
            query.AddParameterWithTypeValue("bindDmsCdType", OracleDbType.NVarchar2, DmsCdType)
            query.AddParameterWithTypeValue("bindIcropCd1", OracleDbType.NVarchar2, IcropCd1)

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetDmsCd1SqlError)

        End Try

    End Function

    Public Function GetIcropCd1(ByVal DmsCdType As String, ByVal DmsCd1 As String) As IC3802801DataSet.IC3802801DmsCodeMapDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801DmsCodeMapDataTable)("IC3802801")
        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("DMS_CD_1 ")
                .Append(",DMS_CD_2 ")
                .Append(",DMS_CD_3 ")
                .Append(",ICROP_CD_1 ")
                .Append(",ICROP_CD_2 ")
                .Append(",ICROP_CD_3 ")
                .Append("FROM ")
                .Append("TB_M_DMS_CODE_MAP ")
                .Append("WHERE ")
                .Append("DMS_CD_TYPE	=	:bindDmsCdType ")
                .Append("AND DMS_CD_1	=	:bindDmsCd1 ")
            End With

            'Bind setting of condition																							
            query.AddParameterWithTypeValue("bindDmsCdType", OracleDbType.NVarchar2, DmsCdType)
            query.AddParameterWithTypeValue("bindDmsCd1", OracleDbType.NVarchar2, DmsCd1)

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetIcropCd1SqlError)

        End Try
    End Function

    Public Function GetDmsCd2(ByVal DmsCdType As String, ByVal IcropCd1 As String, ByVal IcropCd2 As String) As IC3802801DataSet.IC3802801DmsCodeMapDataTable

        'SQL execution query instance creation																						
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801DmsCodeMapDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("DMS_CD_1 ")
                .Append(",DMS_CD_2 ")
                .Append(",DMS_CD_3 ")
                .Append(",ICROP_CD_1 ")
                .Append(",ICROP_CD_2 ")
                .Append(",ICROP_CD_3 ")
                .Append("FROM ")
                .Append("TB_M_DMS_CODE_MAP ")
                .Append("WHERE ")
                .Append("DMS_CD_TYPE	=	:bindDmsCdType ")
                .Append("AND ICROP_CD_1	=	:bindIcropCd1 ")
                .Append("AND ICROP_CD_2	=	:bindIcropCd2 ")
            End With

            'Bind setting of condition																						
            query.AddParameterWithTypeValue("bindDmsCdType", OracleDbType.NVarchar2, DmsCdType)
            query.AddParameterWithTypeValue("bindIcropCd1", OracleDbType.NVarchar2, IcropCd1)
            query.AddParameterWithTypeValue("bindIcropCd2", OracleDbType.NVarchar2, IcropCd2)

            'Return SQL execution & execution result																						
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetDmsCd2SqlError)

        End Try
    End Function

    Public Function GetIcropCd2(ByVal DmsCdType As String, ByVal DmsCd1 As String, ByVal DmsCd2 As String) As IC3802801DataSet.IC3802801DmsCodeMapDataTable

        'SQL execution query instance creation																					
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801DmsCodeMapDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("DMS_CD_1 ")
                .Append(",DMS_CD_2 ")
                .Append(",DMS_CD_3 ")
                .Append(",ICROP_CD_1 ")
                .Append(",ICROP_CD_2 ")
                .Append(",ICROP_CD_3 ")
                .Append("FROM ")
                .Append("TB_M_DMS_CODE_MAP ")
                .Append("WHERE ")
                .Append("DMS_CD_TYPE	=	:bindDmsCdType ")
                .Append("AND DMS_CD_1	=	:bindDmsCd1 ")
                .Append("AND DMS_CD_2	=	:bindDmsCd2 ")
            End With

            'Bind setting of condition																					
            query.AddParameterWithTypeValue("bindDmsCdType", OracleDbType.NVarchar2, DmsCdType)
            query.AddParameterWithTypeValue("bindDmsCd1", OracleDbType.NVarchar2, DmsCd1)
            query.AddParameterWithTypeValue("bindDmsCd2", OracleDbType.NVarchar2, DmsCd2)

            'Return SQL execution & execution result																					
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetIcropCd2SqlError)

        End Try
    End Function

    Public Function GetDmsCd3(ByVal DmsCdType As String, ByVal IcropCd1 As String, ByVal IcropCd3 As String) As IC3802801DataSet.IC3802801DmsCodeMapDataTable

        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801DmsCodeMapDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("DMS_CD_1 ")
                .Append(",DMS_CD_2 ")
                .Append(",DMS_CD_3 ")
                .Append(",ICROP_CD_1 ")
                .Append(",ICROP_CD_2 ")
                .Append(",ICROP_CD_3 ")
                .Append("FROM  ")
                .Append("TB_M_DMS_CODE_MAP ")
                .Append("WHERE ")
                .Append("DMS_CD_TYPE			=	:bindDmsCdType ")
                .Append("AND 	ICROP_CD_1		=	:bindIcropCd1 ")
                .Append("AND 	ICROP_CD_3		=	:bindIcropCd3 ")
            End With

            'Bind setting of condition																								
            query.AddParameterWithTypeValue("bindDmsCdType", OracleDbType.NVarchar2, DmsCdType)
            query.AddParameterWithTypeValue("bindIcropCd1", OracleDbType.NVarchar2, IcropCd1)
            query.AddParameterWithTypeValue("bindIcropCd3", OracleDbType.NVarchar2, IcropCd3)

            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetDmsCd3SqlError)

        End Try

    End Function

    Public Function GetIcropCd3(ByVal DmsCdType As String, ByVal DmsCd1 As String, ByVal DmsCd3 As String) As IC3802801DataSet.IC3802801DmsCodeMapDataTable

        'SQL execution query instance creation																									
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801DmsCodeMapDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT	")
                .Append("DMS_CD_1 ")
                .Append(",DMS_CD_2 ")
                .Append(",DMS_CD_3 ")
                .Append(",ICROP_CD_1 ")
                .Append(",ICROP_CD_2 ")
                .Append(",ICROP_CD_3 ")
                .Append("FROM ")
                .Append("TB_M_DMS_CODE_MAP ")
                .Append("WHERE ")
                .Append("DMS_CD_TYPE    =	:bindDmsCdType ")
                .Append("AND DMS_CD_1   =	:bindDmsCd1 ")
                .Append("AND DMS_CD_3	=	:bindDmsCd3	")
            End With

            'Bind setting of condition																									
            query.AddParameterWithTypeValue("bindDmsCdType", OracleDbType.NVarchar2, DmsCdType)
            query.AddParameterWithTypeValue("bindDmsCd1", OracleDbType.NVarchar2, DmsCd1)
            query.AddParameterWithTypeValue("bindDmsCd3", OracleDbType.NVarchar2, DmsCd3)

            'Return SQL execution & execution result																									
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetIcropCd3SqlError)


        End Try

    End Function

    Public Function GetActionSeq(ByVal SalesId As Long,
        ByVal RelationActType As String,
        ByVal RelationActId As Long) As IC3802801DataSet.IC3802801ActionSeqDataTable

        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ActionSeqDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("SALES_ID ")
                .Append(",RELATION_ACT_SEQ ")
                .Append(",RELATION_ACT_TYPE ")
                .Append(",RELATION_ACT_ID ")
                .Append("FROM ")
                .Append("TB_T_ACTIVITY_SEQ_MANAGER ")
                .Append("WHERE ")
                .Append("SALES_ID		        =	:bindSalesId ")
                .Append("AND RELATION_ACT_TYPE	=	:bindRelationActType ")
                .Append("AND RELATION_ACT_ID	=	:bindRelationActId ")
                .Append("ORDER BY ")
                .Append("RELATION_ACT_SEQ ASC ")
            End With

            'Bind setting of condition																								
            query.AddParameterWithTypeValue("bindSalesId", OracleDbType.Decimal, SalesId)
            query.AddParameterWithTypeValue("bindRelationActType", OracleDbType.NVarchar2, RelationActType)
            query.AddParameterWithTypeValue("bindRelationActId", OracleDbType.Decimal, RelationActId)

            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetActionSeqSqlError)

        End Try

    End Function

    Public Function GetLastActionSeq(ByVal SalesId As Long) As IC3802801DataSet.IC3802801ActionSeqDataTable

        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ActionSeqDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("NVL(MAX(RELATION_ACT_SEQ),0) + 1 AS RELATION_ACT_SEQ ")
                .Append("FROM ")
                .Append("TB_T_ACTIVITY_SEQ_MANAGER ")
                .Append("WHERE ")
                .Append("SALES_ID	=	:bindSalesId ")
            End With

            'Bind setting of condition																								
            query.AddParameterWithTypeValue("bindSalesId", OracleDbType.Decimal, SalesId)

            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetLastActionSeqSqlError)

        End Try

    End Function

    Public Function InsertActionSeqDataAdapter(ByVal dataRow As IC3802801DataSet.IC3802801ActionSeqRow) As Long

        'SQL execution query instance creation																									
        Using query As New DBUpdateQuery("IC3802801_001")

            Try

                'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)																									
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT INTO TB_T_ACTIVITY_SEQ_MANAGER(SALES_ID, RELATION_ACT_SEQ, RELATION_ACT_TYPE, RELATION_ACT_ID, ROW_CREATE_DATETIME, ROW_CREATE_ACCOUNT, ROW_CREATE_FUNCTION, ROW_UPDATE_DATETIME, ROW_UPDATE_ACCOUNT, ROW_UPDATE_FUNCTION, ROW_LOCK_VERSION) ")
                    .Append("VALUES (:SALES_ID,:RELATION_ACT_SEQ,:RELATION_ACT_TYPE,:RELATION_ACT_ID,:ROW_CREATE_DATETIME ,:ROW_CREATE_ACCOUNT ,:ROW_CREATE_FUNCTION ,:ROW_UPDATE_DATETIME,:ROW_UPDATE_ACCOUNT,:ROW_UPDATE_FUNCTION ,:ROW_LOCK_VERSION) ")
                End With

                query.CommandText() = sql.ToString()

                'VALUE bind setting																									
                'takeda_update_start_20150109
                'query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Int32, dataRow.SALES_ID)
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, dataRow.SALES_ID)
                'takeda_update_end_20150109
                query.AddParameterWithTypeValue("RELATION_ACT_SEQ", OracleDbType.Int32, dataRow.RELATION_ACT_SEQ)
                query.AddParameterWithTypeValue("RELATION_ACT_TYPE", OracleDbType.Char, dataRow.RELATION_ACT_TYPE)
                'takeda_update_start_20150109
                'query.AddParameterWithTypeValue("RELATION_ACT_ID", OracleDbType.Int32, dataRow.RELATION_ACT_ID)
                query.AddParameterWithTypeValue("RELATION_ACT_ID", OracleDbType.Decimal, dataRow.RELATION_ACT_ID)
                'takeda_update_end_20150109
                query.AddParameterWithTypeValue("ROW_CREATE_DATETIME", OracleDbType.Date, Date.Now) ' dataRow.ROW_CREATE_DATETIME
                query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, dataRow.ROW_CREATE_ACCOUNT)
                query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, dataRow.ROW_CREATE_FUNCTION)
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, Date.Now) 'dataRow.ROW_UPDATE_DATETIME
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, dataRow.ROW_UPDATE_ACCOUNT)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, dataRow.ROW_UPDATE_FUNCTION)
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Int32, dataRow.ROW_LOCK_VERSION)

                Dim count As Long
                'Return SQL execution & execution result																									
                count = query.Execute()
                Return count

            Catch ex As SystemException
                Logger.Error(ex.Message, ex)
                Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.InsertActionSeqDataAdapterSqlError)

            End Try

        End Using

    End Function

    Public Function GetCustomer(ByVal DlrCd As String, ByVal CstId As Long) As IC3802801DataSet.IC3802801CustomerDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801CustomerDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("CS.CST_ID ")
                .Append(",CS.DMS_CST_CD ")
                .Append(",CS.DMS_CST_CD_DISP ")
                .Append(",CS.NEWCST_CD ")
                .Append(",CS.FLEET_FLG ")
                .Append(",CS.FLEET_PIC_NAME ")
                .Append(",CS.FLEET_PIC_DEPT ")
                .Append(",CS.FLEET_PIC_POSITION ")
                .Append(",CS.CST_SOCIALNUM ")
                .Append(",CS.NAMETITLE_CD ")
                .Append(",CS.NAMETITLE_NAME ")
                .Append(",CS.FIRST_NAME ")
                .Append(",CS.MIDDLE_NAME ")
                .Append(",CS.LAST_NAME ")
                .Append(",CS.NICK_NAME ")
                .Append(",CS.CST_GENDER ")
                .Append(",CS.CST_DOMICILE ")
                .Append(",CS.CST_COUNTRY ")
                .Append(",CS.CST_ZIPCD ")
                .Append(",CS.CST_ADDRESS ")
                .Append(",CS.CST_ADDRESS_1 ")
                .Append(",CS.CST_ADDRESS_2 ")
                .Append(",CS.CST_ADDRESS_3 ")
                .Append(",CS.CST_ADDRESS_STATE ")
                .Append(",CS.CST_ADDRESS_DISTRICT ")
                .Append(",CS.CST_ADDRESS_CITY ")
                .Append(",CS.CST_ADDRESS_LOCATION ")
                .Append(",CS.CST_PHONE ")
                .Append(",CS.CST_MOBILE ")
                .Append(",CS.CST_FAX ")
                .Append(",CS.CST_COMPANY_NAME ")
                .Append(",CS.CST_BIZ_PHONE ")
                .Append(",CS.CST_EMAIL_1	 ")
                .Append(",CS.CST_EMAIL_2	 ")
                .Append(",CS.CST_BIRTH_DATE ")
                .Append(",CS.CST_INCOME ")
                .Append(",CS.CST_OCCUPATION_ID ")
                .Append(",CS.CST_OCCUPATION ")
                .Append(",CS.DEFAULT_LANG ")
                .Append(",CS.PRIVATE_FLEET_ITEM_CD ")
                .Append(",CS.DMS_NEWCST_CD ")
                .Append(",CS.DMS_NEWCST_CD_DISP ")
                .Append(",CD.CST_TYPE ")
                .Append(",CS.ROW_CREATE_DATETIME ")
                .Append(",CS.ROW_UPDATE_DATETIME ")
                .Append("FROM  ")
                .Append("TB_M_CUSTOMER CS,	 ")
                .Append("TB_M_CUSTOMER_DLR CD ")
                .Append("WHERE  ")
                .Append("CS.CST_ID = CD.CST_ID  ")
                .Append("AND CS.CST_ID	=	:bindCstId	 ")
                .Append("AND CD.DLR_CD	=	:bindDlrCd	 ")

            End With

            'Bind setting of condition																							
            query.AddParameterWithTypeValue("bindCstId", OracleDbType.Decimal, CstId)
            query.AddParameterWithTypeValue("bindDlrCd", OracleDbType.NVarchar2, DlrCd)

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetCustomerSqlError)
        End Try

    End Function

    'ISSUE-0025_20130219_by_chatchai_Start
    'Public Function GetCustomerAddress(ByVal StateCd As String,
    '    ByVal DistrictCd As String,
    '    ByVal CityCd As String,
    '    ByVal LocationCd As String) As IC3802801DataSet.IC3802801CustomerAddressDataTable

    '    'SQL execution query instance creation																							
    '    Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801CustomerAddressDataTable)("IC3802801")

    '    Try


    '        'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
    '        Dim sql As New StringBuilder
    '        With sql
    '            .Append("SELECT	")
    '            .Append("ST.STATE_NAME	")
    '            .Append(",DI.DISTRICT_NAME	")
    '            .Append(",CI.CITY_NAME	")
    '            .Append(",LO.LOCATION_NAME	")
    '            .Append("FROM	")
    '            .Append("TB_M_STATE			ST	")
    '            .Append(",TB_M_DISTRICT		DI	")
    '            .Append(",TB_M_CITY			CI	")
    '            .Append(",TB_M_LOCATION		LO	")
    '            .Append("WHERE	")
    '            .Append("LO.STATE_CD = ST.STATE_CD	")
    '            .Append("AND 	LO.DISTRICT_CD		=	DI.DISTRICT_CD	")
    '            .Append("AND 	LO.CITY_CD			=	CI.CITY_CD	")
    '            .Append("AND 	LO.STATE_CD		=	:bindStateCd	")
    '            .Append("AND 	LO.DISTRICT_CD		=	:bindDistrictCd	")
    '            .Append("AND 	LO.CITY_CD			=	:bindCityCd	")
    '            .Append("AND 	LO.LOCATION_CD	=	:bindLocationCd	")
    '        End With

    '        'Bind setting of condition																							
    '        query.AddParameterWithTypeValue("bindStateCd", OracleDbType.NVarchar2, StateCd)                   'Parameter state code)										
    '        query.AddParameterWithTypeValue("bindDistrictCd", OracleDbType.NVarchar2, DistrictCd)                     'Parameter district code)										
    '        query.AddParameterWithTypeValue("bindCityCd", OracleDbType.NVarchar2, CityCd)                     'Parameter city code)										
    '        query.AddParameterWithTypeValue("bindLocationCd", OracleDbType.NVarchar2, LocationCd)                     'Parameter location code)										

    '        'Return SQL execution & execution result																							
    '        query.CommandText() = sql.ToString()
    '        Return query.GetData()
    '    Catch ex As SystemException
    '        Logger.Error(ex.Message, ex)
    '        Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetCustomerAddressSqlError)
    '    End Try

    'End Function
    'ISSUE-0025_20130219_by_chatchai_End

    Public Function GetFamilyInfomation(ByVal CrCustId As String) As IC3802801DataSet.IC3802801FamilyInfomationDataTable

        'SQL execution query instance creation																						
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801FamilyInfomationDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("FA.CSTKIND ")
                .Append(",FA.CUSTOMERCLASS ")
                .Append(",FA.CRCUSTID ")
                .Append(",FA.FAMILYNO ")
                .Append(",FA.FAMILYRELATIONSHIPNO ")
                .Append(",FA.OTHERFAMILYRELATIONSHIP ")
                .Append(",FA.BIRTHDAY ")
                .Append(",FR.FAMILYRELATIONSHIP ")
                .Append("FROM ")
                .Append("TBL_CSTFAMILY				FA, ")
                .Append("TBL_FAMILYRELATIONSHIPMST	FR ")
                .Append("WHERE ")
                .Append("FA.FAMILYRELATIONSHIPNO = FR.FAMILYRELATIONSHIPNO ")
                .Append("AND FA.CRCUSTID	=	:bindCrCustId ")
                .Append("ORDER BY ")
                .Append("FA.FAMILYNO Asc ")
            End With

            'Bind setting of condition																						
            query.AddParameterWithTypeValue("bindCrCustId", OracleDbType.Varchar2, CrCustId.ToString().PadRight(20, " "))                    'Parameter activity target customer code									

            'Return SQL execution & execution result																						
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetFamilyInfomationSqlError)
        End Try

    End Function

    Public Function GetContactTimeslot(ByVal CstId As Long, ByVal TimeslotClass As String) As IC3802801DataSet.IC3802801ContactTimeslotDataTable

        'SQL execution query instance creation																								
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ContactTimeslotDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("SUM(CONTACT_TIMESLOT) AS CONTACT_TIMESLOT ")
                .Append("FROM ")
                .Append("TB_M_CST_CONTACT_TIMESLOT LO ")
                .Append("WHERE ")
                .Append("LO.CST_ID	= :bindCstId ")
                .Append("AND LO.TIMESLOT_CLASS	=	:bindTimeslotClass ")
            End With


            'Return SQL execution & execution result																								
            query.AddParameterWithTypeValue("bindCstId", OracleDbType.Decimal, CstId)
            query.AddParameterWithTypeValue("bindTimeslotClass", OracleDbType.NVarchar2, TimeslotClass)

            'Return SQL execution & execution result																								
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetContactTimeslotSqlError)

        End Try

    End Function

    Public Function GetHobby(ByVal CrCustId As String) As IC3802801DataSet.IC3802801HobbyDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801HobbyDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("CH.CSTKIND ")
                .Append(",CH.CUSTOMERCLASS ")
                .Append(",CH.CRCUSTID ")
                .Append(",CH.HOBBYNO ")
                .Append(",CH.OTHERHOBBY ")
                .Append(",HM.HOBBY ")
                .Append(" FROM ")
                .Append("TBL_CSTHOBBY				CH, ")
                .Append("TBL_HOBBYMST				HM ")
                .Append("WHERE ")
                .Append("CH.HOBBYNO = HM.HOBBYNO ")
                .Append("AND 	CH.	CRCUSTID	=	:bindCrCustId ")
            End With

            'Bind setting of condition																							
            'ISSUE08_20130217_by_takeda_Start
            'query.AddParameterWithTypeValue("bindCrCustId", OracleDbType.Decimal, CrCustId)
            query.AddParameterWithTypeValue("bindCrCustId", OracleDbType.Varchar2, CrCustId.ToString().PadRight(20, " "))
            'ISSUE08_20130217_by_takeda_End

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetHobbySqlError)
        End Try

    End Function

    Public Function GetLastRequestActionId(ByVal ReqId As Long) As IC3802801DataSet.IC3802801ActionDataTable

        'SQL execution query instance creation																									
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ActionDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("AT.ACT_ID ")
                .Append("FROM ")
                .Append("TB_T_ACTIVITY AT ")
                .Append(",(	SELECT REQ_ID	")
                .Append(",	MAX(ACT_COUNT) AS ACT_COUNT	")
                .Append("FROM ")
                .Append("TB_T_ACTIVITY ")
                .Append("WHERE REQ_ID	=	:bindReqId ")
                'ISSUE-IT2-1_by_takeda_start
                .Append(" AND RSLT_FLG	=	'1' ")
                'ISSUE-IT2-1_by_takeda_end
                .Append("GROUP BY REQ_ID ")
                .Append(") AT_STATUS ")
                .Append("WHERE ")
                .Append("AT_STATUS.REQ_ID							=	:bindReqId	")
                .Append(" AND AT.REQ_ID = AT_STATUS.REQ_ID ")
                .Append(" AND AT.ACT_COUNT = AT_STATUS.ACT_COUNT ")
            End With


            'Bind setting of condition																									
            query.AddParameterWithTypeValue("bindReqId", OracleDbType.Decimal, ReqId)

            'Return SQL execution & execution result																									
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetLastRequestActionIdSqlError)
        End Try

    End Function

    Public Function GetLastAttractActionId(ByVal AttId As Long) As IC3802801DataSet.IC3802801ActionDataTable

        'SQL execution query instance creation																									
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801ActionDataTable)("IC3802801")

        Try

            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("AT.ACT_ID ")
                .Append("FROM ")
                .Append("TB_T_ACTIVITY AT ")
                .Append(",(	SELECT ")
                .Append("ATT_ID ")
                .Append(",MAX(ACT_COUNT) AS ACT_COUNT ")
                .Append("FROM ")
                .Append("TB_T_ACTIVITY ")
                .Append("WHERE ")
                .Append("ATT_ID		=	:bindAttId	 ")
                'ISSUE-IT2-1_by_takeda_start
                .Append(" AND RSLT_FLG	=	'1' ")
                'ISSUE-IT2-1_by_takeda_end
                .Append("GROUP BY ATT_ID ")
                .Append(") AT_STATUS	 ")
                .Append("WHERE ")
                .Append("AT_STATUS.ATT_ID	=	:bindAttId AND ")
                .Append("AT.ATT_ID = AT_STATUS.ATT_ID AND ")
                .Append("AT.ACT_COUNT = AT_STATUS.ACT_COUNT ")
            End With

            'Bind setting of condition																									
            query.AddParameterWithTypeValue("bindAttId", OracleDbType.Decimal, AttId)

            'Return SQL execution & execution result																									
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetLastAttractActionIdSqlError)
        End Try

    End Function

    Public Function GetEstimateVclInfo(ByVal EstimateId As Long) As IC3802801DataSet.IC3802801EstimateVclInfoDataTable

        'SQL execution query instance creation																									
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801EstimateVclInfoDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("ESTIMATEID ")
                .Append(",SERIESCD ")
                .Append(",MODELCD ")
                .Append(",SUFFIXCD ")
                .Append(",EXTCOLORCD ")
                .Append(",INTCOLORCD ")
                .Append("FROM ")
                .Append("TBL_EST_VCLINFO ")
                .Append("WHERE ")
                .Append("ESTIMATEID = :bindEstimateId ")
            End With

            'Bind setting of condition																									
            query.AddParameterWithTypeValue("bindEstimateId", OracleDbType.Decimal, EstimateId)

            'Return SQL execution & execution result																									
            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetEstimateVclInfoSqlError)
        End Try

    End Function



    Public Function MoveActionSeq(ByVal SalesId As Long)

        'SQL execution query instance creation																									
        Using query As New DBUpdateQuery("IC3802801_002")

            Try
                'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT INTO TB_H_ACTIVITY_SEQ_MANAGER (SALES_ID,RELATION_ACT_SEQ,RELATION_ACT_TYPE,RELATION_ACT_ID,ROW_CREATE_DATETIME,ROW_CREATE_ACCOUNT,ROW_CREATE_FUNCTION, ROW_UPDATE_DATETIME,ROW_UPDATE_ACCOUNT,ROW_UPDATE_FUNCTION,ROW_LOCK_VERSION) ")
                    .Append("Select SALES_ID,RELATION_ACT_SEQ,RELATION_ACT_TYPE,RELATION_ACT_ID,ROW_CREATE_DATETIME,ROW_CREATE_ACCOUNT,ROW_CREATE_FUNCTION, ROW_UPDATE_DATETIME,ROW_UPDATE_ACCOUNT,ROW_UPDATE_FUNCTION,ROW_LOCK_VERSION FROM TB_T_ACTIVITY_SEQ_MANAGER ")
                    .Append("WHERE SALES_ID	=	:bindSalesId")
                End With


                'Bind setting of condition																									
                query.AddParameterWithTypeValue("bindSalesId", OracleDbType.Decimal, SalesId)
                query.CommandText() = sql.ToString()

                'SQL execution																									
                Return query.Execute()

            Catch ex As SystemException
                Logger.Error(ex.Message, ex)
                Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.MoveActionSeqSqlError)
            End Try

        End Using

    End Function

    Public Function DeleteActionSeq(ByVal SalesId As Long)

        'SQL execution query instance creation																									
        Using query As New DBUpdateQuery("IC3802801_003")

            Try
                'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)
                Dim sql As New StringBuilder
                With sql
                    .Append("DELETE ")
                    .Append("FROM TB_T_ACTIVITY_SEQ_MANAGER ")
                    .Append("WHERE SALES_ID=:bindSalesId ")
                End With


                'Bind setting of condition																									
                query.AddParameterWithTypeValue("bindSalesId", OracleDbType.Decimal, SalesId)
                query.CommandText() = sql.ToString()

                'Sql(execution)
                Return query.Execute()

            Catch ex As SystemException
                Logger.Error(ex.Message, ex)
                Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.DeleteActionSeqSqlError)
            End Try

        End Using

    End Function

    Public Function CheckAttractData(ByVal AttId As Long) As IC3802801DataSet.SumAttractDataTable

        'SQL execution query instance creation																									
        Dim query As New DBSelectQuery(Of IC3802801DataSet.SumAttractDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("SUM(CNT) as CNT ")
                .Append("FROM ")
                .Append("(	SELECT ")
                .Append("COUNT(*) AS CNT ")
                .Append("FROM ")
                .Append("TB_T_ATTRACT_DM ")
                .Append("WHERE ")
                .Append("ATT_ID						=	:bindAttId	")
                .Append("AND CONSTRAINT_STATUS	    =	1 ")
                .Append("AND RSLT_FLG				<>	1 ")
                .Append("UNION ALL ")
                .Append("SELECT ")
                .Append("COUNT(*) AS CNT ")
                .Append("FROM ")
                .Append("TB_T_ATTRACT_RMM ")
                .Append("WHERE ")
                .Append("ATT_ID						=	:bindAttId ")
                .Append("AND CONSTRAINT_STATUS		=	1 ")
                .Append("AND RSLT_FLG				<>	1 ")
                .Append(" )")
            End With

            'Bind setting of condition																									
            query.AddParameterWithTypeValue("bindAttId", OracleDbType.Decimal, AttId)

            query.CommandText() = sql.ToString()
            Return query.GetData()

        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.DeleteActionSeqSqlError)
        End Try

    End Function

    '==========takeda_update_start_20140422_性能改善調査==========
    Public Function InsertActionSeqDataAdapter2(ByVal SalesId As Decimal, ByVal actSeq As Long) As Long

        'SQL execution query instance creation																									
        Using query As New DBUpdateQuery("IC3802801_901")

            Try

                'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)																									
                Dim sql As New StringBuilder
                With sql
                    .Append("INSERT INTO TB_T_ACTIVITY_SEQ_MANAGER(SALES_ID, RELATION_ACT_SEQ, RELATION_ACT_TYPE, RELATION_ACT_ID, ROW_CREATE_DATETIME, ROW_CREATE_ACCOUNT, ROW_CREATE_FUNCTION, ROW_UPDATE_DATETIME, ROW_UPDATE_ACCOUNT, ROW_UPDATE_FUNCTION, ROW_LOCK_VERSION) ")
                    .Append("VALUES (:SALES_ID,:RELATION_ACT_SEQ,:RELATION_ACT_TYPE,:RELATION_ACT_ID,:ROW_CREATE_DATETIME ,:ROW_CREATE_ACCOUNT ,:ROW_CREATE_FUNCTION ,:ROW_UPDATE_DATETIME,:ROW_UPDATE_ACCOUNT,:ROW_UPDATE_FUNCTION ,:ROW_LOCK_VERSION) ")
                End With

                query.CommandText() = sql.ToString()

                Dim dtDateTime As DateTime
                Dim strDateTime As String
                Dim dmDateTime As Decimal
                '現在時刻を求める
                dtDateTime = DateTime.Now
                '区切り文字なし、24H形式に編集し、文字列項目に格納(ミリ秒は3桁表記)
                strDateTime = dtDateTime.ToString("yyyyMMddHHmmss") & dtDateTime.Millisecond.ToString("000")
                dmDateTime = Decimal.Parse(strDateTime)

                'VALUE bind setting																									
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, SalesId)
                query.AddParameterWithTypeValue("RELATION_ACT_SEQ", OracleDbType.Int32, actSeq)
                query.AddParameterWithTypeValue("RELATION_ACT_TYPE", OracleDbType.Char, "0")
                query.AddParameterWithTypeValue("RELATION_ACT_ID", OracleDbType.Decimal, dmDateTime)
                query.AddParameterWithTypeValue("ROW_CREATE_DATETIME", OracleDbType.Date, Date.Now) ' dataRow.ROW_CREATE_DATETIME
                query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, " ")
                query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, " ")
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, Date.Now) 'dataRow.ROW_UPDATE_DATETIME
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, " ")
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, " ")
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Int32, 0)

                Dim count As Long
                'Return SQL execution & execution result																									
                count = query.Execute()
                Return count

            Catch ex As SystemException
                Logger.Error(ex.Message, ex)
                Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.InsertActionSeqDataAdapterSqlError)

            End Try

        End Using

    End Function
    '==========takeda_update_end_20140422_性能改善調査============

    '$01 start
    Public Function GetCustomerLocal(ByVal CstId As Long) As IC3802801DataSet.IC3802801CustomerLocalDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801CustomerLocalDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("CS.CST_SUBCAT2_CD, ")
                .Append("CASE ")
                .Append("CS.CST_ORGNZ_INPUT_TYPE ")
                .Append("WHEN :bindMaster THEN ")
                .Append("CO.CST_ORGNZ_NAME  ")
                .Append("WHEN :bindManual THEN ")
                .Append("CS.CST_ORGNZ_NAME ")
                .Append("END AS CST_ORGNZ_NAME ")
                .Append("FROM ")
                .Append("TB_LM_CUSTOMER CS ")
                .Append("LEFT JOIN ")
                .Append("TB_LM_CUSTOMER_ORGANIZATION CO ")
                .Append("ON CS.CST_ORGNZ_CD = CO.CST_ORGNZ_CD ")
                .Append("WHERE ")
                .Append("CS.CST_ID = :bindCstId ")
            End With

            'Bind setting of condition
            query.AddParameterWithTypeValue("bindMaster", OracleDbType.NVarchar2, OrganizationInputTypeMaster)
            query.AddParameterWithTypeValue("bindManual", OracleDbType.NVarchar2, OrganizationInputTypeManual)
            query.AddParameterWithTypeValue("bindCstId", OracleDbType.Decimal, CstId)

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetCustomerLocalSqlError)

        End Try
    End Function
    '$01 end

    '$01 start
    Public Function GetVehicleDlrLocal(ByVal DlrCd As String,
        ByVal VclId As Long) As IC3802801DataSet.IC3802801VehicleDlrLocalDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801VehicleDlrLocalDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("VD.VCL_MILE ")
                .Append(",VD.MODEL_YEAR ")
                .Append("FROM ")
                .Append("TB_LM_VEHICLE_DLR VD ")
                .Append("WHERE ")
                .Append("VD.VCL_ID = :bindVclId ")
                .Append("AND VD.DLR_CD = :bindDlrCd ")
            End With

            'Bind setting of condition																							
            query.AddParameterWithTypeValue("bindVclId", OracleDbType.Decimal, VclId)
            query.AddParameterWithTypeValue("bindDlrCd", OracleDbType.NVarchar2, DlrCd)

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetVehicleDlrLocalSqlError)

        End Try
    End Function
    '$01 end

    '$01 start
    Public Function GetSalesLocal(ByVal SalesId As Long) As IC3802801DataSet.IC3802801SalesLocalDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801SalesLocalDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("SA.DEMAND_STRUCTURE_CD ")
                .Append(",DS.TRADEINCAR_ENABLED_FLG ")
                .Append(",MA.MAKER_NAME ")
                .Append(",MO.MODEL_NAME ")
                .Append(",SA.TRADEINCAR_MILE ")
                .Append(",SA.TRADEINCAR_MODEL_YEAR ")
                .Append(",SA.ROW_CREATE_DATETIME ")
                .Append("FROM ")
                .Append("TB_LT_SALES SA ")
                .Append("LEFT JOIN ")
                .Append("TB_M_MAKER MA ")
                .Append("ON SA.TRADEINCAR_MAKER_CD = MA.MAKER_CD ")
                .Append("LEFT JOIN ")
                .Append("TB_M_MODEL MO ")
                .Append("ON SA.TRADEINCAR_MAKER_CD = MO.MAKER_CD ")
                .Append("AND SA.TRADEINCAR_MODEL_CD = MO.MODEL_CD ")
                .Append("LEFT JOIN ")
                .Append("TB_LM_DEMAND_STRUCTURE DS ")
                .Append("ON SA.DEMAND_STRUCTURE_CD = DS.DEMAND_STRUCTURE_CD ")
                .Append("WHERE ")
                .Append("SA.SALES_ID = :bindSalesId ")

            End With

            'Bind setting of condition													
            query.AddParameterWithTypeValue("bindSalesId", OracleDbType.NVarchar2, SalesId)

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetSalesLocalSqlError)

        End Try
    End Function
    '$01 end

    '$02 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
    Public Function GetSalesLocalSource2CD(ByVal SalesId As Long) As IC3802801DataSet.IC3802801SalesLocalDataTable

        'SQL execution query instance creation																							
        Dim query As New DBSelectQuery(Of IC3802801DataSet.IC3802801SalesLocalDataTable)("IC3802801")

        Try
            'Store the following SELECT statement in a variable (Store in SQL variable of StringBuffer type)	
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("SALES_ID ,")
                .Append("SOURCE_2_CD ")
                .Append("FROM ")
                .Append("TB_LT_SALES ")
                .Append("WHERE ")
                .Append("SALES_ID = :bindSalesId ")

            End With

            'Bind setting of condition													
            query.AddParameterWithTypeValue("bindSalesId", OracleDbType.NVarchar2, SalesId)

            'Return SQL execution & execution result																							
            query.CommandText() = sql.ToString()
            Return query.GetData()
        Catch ex As SystemException
            Logger.Error(ex.Message, ex)
            Throw New ApplicationException(ReturnCode.DataBaseError + DataBaseErrorCode.GetSalesLocalSqlError)

        End Try
    End Function
    '$02 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end

End Class
