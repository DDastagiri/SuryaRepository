'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250105DataSet.vb
'─────────────────────────────────────
'機能： 部品説明画面（部品交換情報）DataSet.vb
'補足： 
'作成： 2014/08/XX NEC 上野
'更新： 2014/08/xx 
'─────────────────────────────────────

Option Explicit On

Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core

Partial Class SC3250105DataSet

    '点検結果
    Public Enum Enum_InspecRsltCd
        NoAction = 0
        NoProblem = 1
        NeedInspection = 2
        NeedReplace = 3
        NeedFixing = 4
        NeedCleaning = 5
        NeedSwapping = 6
    End Enum


    ' ''' <summary>
    ' ''' 前回部品交換履歴データ取得(RO番号あり)
    ' ''' </summary>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="branchCode">店舗コード</param>
    ' ''' <param name="vclVin">VIN</param>
    ' ''' <param name="inspecItemcd">点検項目コード</param>
    ' ''' <param name="roNum">RO番号(任意)</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Shared Function GetPreviosReplacement(
    '                    ByVal dealerCode As String,
    '                    ByVal branchCode As String, _
    '                    ByVal roNum As String, _
    '                    ByVal vclVin As String, _
    '                    ByVal inspecItemCd As String
    '                                        ) As SC3250105DataSet.PreviosReplacementDataTable

    '    Dim sqlNo As String = "SC3250105_001"
    '    Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

    '    Dim sql As New StringBuilder
    '    'SQL文作成
    '    With sql
    '        '--RO No.='GSJ1400648'がセッションで渡されてきたことを想定
    '        '--VIN='TEST_6T153SK1009012609'
    '        '--INSPEC_ITEM_CD = '7052'
    '        '※RO番号が引き渡されてきた場合、そのRO番号以外のデータで最新のものを取得する仕様
    '        .Append("SELECT ")
    '        .Append(" /* SC3250105_001 */ ")
    '        .Append(" SUB.* ")
    '        .Append(" ")
    '        .Append("FROM ")
    '        .Append(" ( ")
    '        .Append("SELECT ")
    '        .Append("  SV.SVCIN_ID ")
    '        .Append(", SV.SVCIN_MILE ")
    '        .Append(", SV.RSLT_DELI_DATETIME ")
    '        .Append(", RO_SUB.RO_NUM ")
    '        .Append(", RO_SUB.RO_STATUS ")
    '        .Append(", FIH.RO_NUM AS FIH_RO_NUM_SAMPLE ")
    '        .Append(", JD.JOB_DTL_ID AS JD_JOB_DTL_ID_SAMPLE ")
    '        .Append(" ")
    '        .Append("FROM ")
    '        .Append(" TB_M_VEHICLE MV ")
    '        .Append(",TB_T_SERVICEIN SV ")
    '        .Append(",TB_T_JOB_DTL JD ")
    '        .Append(",TB_T_FINAL_INSPECTION_HEAD FIH ")
    '        .Append(",TB_T_FINAL_INSPECTION_DETAIL FID ")
    '        .Append(",( ")
    '        .Append("SELECT SVCIN_ID, RO_NUM, RO_STATUS FROM TB_T_RO_INFO WHERE RO_NUM <> :RO_NUM AND RO_SEQ = 0 ")
    '        .Append(") RO_SUB ")
    '        .Append(" ")
    '        .Append("WHERE ")
    '        .Append(" MV.VCL_ID = SV.VCL_ID ")
    '        .Append("AND SV.SVCIN_ID = RO_SUB.SVCIN_ID ")
    '        .Append("AND SV.SVCIN_ID = JD.SVCIN_ID ")
    '        .Append("AND JD.JOB_DTL_ID = FIH.JOB_DTL_ID ")
    '        .Append("AND FIH.JOB_DTL_ID = FID.JOB_DTL_ID ")
    '        .Append("AND FIH.RO_NUM = RO_SUB.RO_NUM ")
    '        .Append("AND MV.VCL_VIN = :VCL_VIN ")
    '        .Append("AND FIH.DLR_CD = :DLR_CD ")
    '        .Append("AND FIH.BRN_CD = :BRN_CD ")
    '        .Append("AND FIH.RO_NUM <> :RO_NUM ")
    '        .Append("AND FID.INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
    '        .Append(" ")
    '        .Append("ORDER BY ")
    '        .Append(" SV.RSLT_DELI_DATETIME DESC ")
    '        .Append(") SUB ")
    '        .Append(" ")
    '        .Append("WHERE ")
    '        .Append(" ROWNUM=1 ")
    '    End With

    '    Using query As New DBSelectQuery(Of SC3250105DataSet.PreviosReplacementDataTable)(sqlNo)
    '        query.CommandText = sql.ToString()
    '        query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.Char, vclVin)
    '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
    '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)
    '        query.AddParameterWithTypeValue("RO_NUM", OracleDbType.Char, roNum)
    '        query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.Char, inspecItemCd)

    '        sql = Nothing

    '        Using dt As SC3250105DataSet.PreviosReplacementDataTable = query.GetData
    '            Return dt
    '        End Using
    '    End Using

    'End Function

    ''' <summary>
    ''' 001:前回部品交換時の日時を取得する
    ''' </summary>
    ''' <param name="vclVin">VIN</param>
    ''' <param name="inspecItemcd">点検項目コード</param>
    ''' <param name="roNum">RO番号(任意)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPreviosReplacementDatetime(ByVal vclVin As String, _
                                                  ByVal inspecItemCd As String, _
                                                  Optional ByVal roNum As String = "" _
                                                  ) As SC3250105DataSet.PreviosReplacementDateTimeDataTable

        Dim sqlNo As String = "SC3250105_001"
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                         "{0} Start [vclVin:{1}][inspecItemCd:{2}][roNum:{3}]",
                         System.Reflection.MethodBase.GetCurrentMethod.Name,
                         vclVin, inspecItemCd, roNum))

        Dim sql As New StringBuilder
        'SQL文作成
        With sql
            .Append("SELECT /* SC3250105_001 */ ")
            .Append("	INSPEC_ITEM_CD, ")
            .Append("	JOB_DTL_ID, ")
            .Append("	DLR_CD, ")
            .Append("	BRN_CD, ")
            .Append("	RO_NUM, ")
            .Append("	REPLACED_DATETIME ")
            .Append("FROM ")
            .Append("( ")

            .Append("SELECT ")
            .Append("	INSPEC_ITEM_CD, ")
            .Append("	JOB_DTL_ID, ")
            .Append("	DLR_CD, ")
            .Append("	BRN_CD, ")
            .Append("	RO_NUM, ")
            .Append("	REPLACED_DATETIME ")
            .Append("FROM ")
            .Append("	(SELECT ")
            .Append("		 T5.INSPEC_ITEM_CD ")
            .Append("		,T2.JOB_DTL_ID ")
            .Append("		,T4.DLR_CD ")
            .Append("		,T4.BRN_CD ")
            .Append("		,T4.RO_NUM ")
            .Append("		,CASE  ")
            .Append("			WHEN T2.INSPECTION_NEED_FLG = '1' THEN T4.INSPECTION_APPROVAL_DATETIME ")
            .Append("			ELSE T4.ROW_UPDATE_DATETIME ")
            .Append("		 END AS REPLACED_DATETIME ")
            .Append("	FROM ")
            .Append("		TB_M_VEHICLE M1 ")
            .Append("		,TB_T_SERVICEIN T1 ")
            .Append("		,TB_T_JOB_DTL T2 ")
            .Append("		,TB_T_JOB_INSTRUCT T3 ")
            .Append("		,TB_T_FINAL_INSPECTION_HEAD T4 ")
            .Append("		,TB_T_FINAL_INSPECTION_DETAIL T5 ")
            .Append("	WHERE ")
            .Append("		M1.VCL_VIN = :VCL_VIN ")
            .Append("		AND M1.VCL_ID = T1.VCL_ID ")
            .Append("		AND T1.SVCIN_ID = T2.SVCIN_ID ")
            .Append("		AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
            .Append("		AND T3.JOB_DTL_ID = T4.JOB_DTL_ID ")
            .Append("		AND T3.JOB_DTL_ID = T5.JOB_DTL_ID ")
            .Append("		AND T3.JOB_INSTRUCT_ID = T5.JOB_INSTRUCT_ID ")
            .Append("		AND T3.JOB_INSTRUCT_SEQ = T5.JOB_INSTRUCT_SEQ ")
            .Append("		AND T5.INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
            '2014/09/03　条件追加（OPERATION_RSLT_ALREADY_REPLACEが「0」以外　又は　INSPEC_RSLT_CDが「3」（Need Replace））
            '.Append("		AND T5.OPERATION_RSLT_ALREADY_REPLACE <> 0 ")
            '.Append("		AND (T5.OPERATION_RSLT_ALREADY_REPLACE <> 0 OR T5.INSPEC_RSLT_CD = 3) ")
            .Append("		AND (T5.OPERATION_RSLT_ALREADY_REPLACE <> 0 OR T5.INSPEC_RSLT_CD = ").Append(Enum_InspecRsltCd.NeedReplace).Append(") ")
            If roNum <> "" Then
                .Append("		AND T4.RO_NUM <> :RO_NUM ")
            End If
            .Append("UNION ALL ")
            .Append("	SELECT ")
            .Append("		 T5.INSPEC_ITEM_CD ")
            .Append("		,T2.JOB_DTL_ID ")
            .Append("		,T4.DLR_CD ")
            .Append("		,T4.BRN_CD ")
            .Append("		,T4.RO_NUM ")
            .Append("		,CASE  ")
            .Append("			WHEN T2.INSPECTION_NEED_FLG = '1' THEN T4.INSPECTION_APPROVAL_DATETIME ")
            .Append("			ELSE T4.ROW_UPDATE_DATETIME ")
            .Append("		 END AS REPLACED_DATETIME ")
            .Append("	FROM ")
            .Append("		TB_M_VEHICLE M1 ")
            .Append("		,TB_H_SERVICEIN T1 ")
            .Append("		,TB_H_JOB_DTL T2 ")
            .Append("		,TB_H_JOB_INSTRUCT T3 ")
            .Append("		,TB_H_FINAL_INSPECTION_HEAD T4 ")
            .Append("		,TB_H_FINAL_INSPECTION_DETAIL T5 ")
            .Append("	WHERE ")
            .Append("		M1.VCL_VIN = :VCL_VIN ")
            .Append("		AND M1.VCL_ID = T1.VCL_ID ")
            .Append("		AND T1.SVCIN_ID = T2.SVCIN_ID ")
            .Append("		AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
            .Append("		AND T3.JOB_DTL_ID = T4.JOB_DTL_ID ")
            .Append("		AND T3.JOB_DTL_ID = T5.JOB_DTL_ID ")
            .Append("		AND T3.JOB_INSTRUCT_ID = T5.JOB_INSTRUCT_ID ")
            .Append("		AND T3.JOB_INSTRUCT_SEQ = T5.JOB_INSTRUCT_SEQ ")
            .Append("		AND T5.INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
            .Append("		AND (T5.OPERATION_RSLT_ALREADY_REPLACE <> 0 OR T5.INSPEC_RSLT_CD = ").Append(Enum_InspecRsltCd.NeedReplace).Append(") ")
            If roNum <> "" Then
                .Append("		AND T4.RO_NUM <> :RO_NUM ")
            End If
            .Append(" ) TBL1 ")

            .Append("ORDER BY ")
            .Append("	REPLACED_DATETIME DESC")

            .Append(" ) ")
            .Append("WHERE ")
            .Append("	ROWNUM = 1 ")
        End With

        Using query As New DBSelectQuery(Of SC3250105DataSet.PreviosReplacementDateTimeDataTable)(sqlNo)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.Char, vclVin)
            If roNum <> "" Then
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.Char, roNum)
            End If
            query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.Char, inspecItemCd)

            sql = Nothing

            Using dt As SC3250105DataSet.PreviosReplacementDateTimeDataTable = query.GetData
                Return dt
            End Using
        End Using

    End Function

    ''' <summary>
    ''' 002:前回部品交換時の走行距離を取得する
    ''' </summary>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strSVCIN_NUM">入庫管理番号</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPreviosReplacementMileage(ByVal strDLR_CD As String, _
                                                 ByVal strSVCIN_NUM As String _
                                                 ) As SC3250105DataSet.PreviosReplacementMileageDataTable

        Dim sqlNo As String = "SC3250105_002"
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Dim sql As New StringBuilder
        'SQL文作成
        With sql
            .Append("SELECT /* SC3250105_002 */ ")
            .Append("	NVL(T2.REG_MILE, 0) AS REG_MILE ")
            .Append("FROM ")
            .Append("	 TB_T_VEHICLE_SVCIN_HIS T1 ")
            .Append("	,TB_T_VEHICLE_MILEAGE T2 ")
            .Append("WHERE ")
            .Append("	T1.DLR_CD = :DLR_CD ")
            .Append("	AND T1.SVCIN_NUM = :SVCIN_NUM ")
            .Append("	AND T1.VCL_MILE_ID = T2.VCL_MILE_ID(+) ")

        End With

        Using query As New DBSelectQuery(Of SC3250105DataSet.PreviosReplacementMileageDataTable)(sqlNo)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, strDLR_CD)
            query.AddParameterWithTypeValue("SVCIN_NUM", OracleDbType.Char, strSVCIN_NUM)

            sql = Nothing

            Using dt As SC3250105DataSet.PreviosReplacementMileageDataTable = query.GetData
                Return dt
            End Using
        End Using

    End Function

    ''' <summary>
    ''' 003:納車日と車両区分を取得する
    ''' </summary>
    ''' <param name="strVCL_VIN">VINコード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVehicleDeliveryDate(ByVal strVCL_VIN As String) As SC3250105DataSet.VehicleDeliveryDataTable

        Dim sqlNo As String = "SC3250105_003"
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Dim sql As New StringBuilder
        'SQL文作成
        With sql
            .Append("SELECT /* SC3250105_003 */ ")
            .Append("   DELI_DATE, VCL_TYPE ")
            .Append("FROM ( ")
            .Append("SELECT /* SC3250105_003 */ ")
            .Append("	M2.DELI_DATE ")
            .Append("	,M2.VCL_TYPE ")
            .Append("FROM ")
            .Append("	 TB_M_VEHICLE M1 ")
            .Append("	,(SELECT VCL_ID, DLR_CD FROM TB_T_SERVICEIN UNION ALL SELECT VCL_ID, DLR_CD FROM TB_H_SERVICEIN) T1 ")
            .Append("	,TB_M_VEHICLE_DLR M2 ")
            .Append("WHERE ")
            .Append("	M1.VCL_VIN = :VCL_VIN ")
            .Append("	AND M1.VCL_ID = T1.VCL_ID ")
            .Append("	AND T1.VCL_ID = M2.VCL_ID ")
            .Append("	AND T1.DLR_CD = M2.DLR_CD ")
            .Append(") ")
            .Append("WHERE ")
            .Append("	ROWNUM = 1 ")
        End With

        Using query As New DBSelectQuery(Of SC3250105DataSet.VehicleDeliveryDataTable)(sqlNo)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.Char, strVCL_VIN)

            sql = Nothing

            Using dt As SC3250105DataSet.VehicleDeliveryDataTable = query.GetData
                Return dt
            End Using
        End Using

    End Function

    ''' <summary>
    ''' 004:販売店システム設定から設定値を取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="allDealerCode">全店舗を示す販売店コード</param>
    ''' <param name="allBranchCode">全店舗を示す店舗コード</param>
    ''' <param name="settingName">販売店システム設定名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDlrSystemSettingValue(ByVal dealerCode As String, _
                                             ByVal branchCode As String, _
                                             ByVal allDealerCode As String, _
                                             ByVal allBranchCode As String, _
                                             ByVal settingName As String) As SC3250105DataSet.SystemSettingDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dealerCode, _
                                  branchCode, _
                                  allDealerCode, _
                                  allBranchCode, _
                                  settingName))

        Dim sql As New StringBuilder
        With sql
            .Append("   SELECT /* SC3250105_004 */ ")
            .Append(" 		   SETTING_VAL ")
            .Append("     FROM ")
            .Append(" 		   TB_M_SYSTEM_SETTING_DLR ")
            .Append("    WHERE ")
            .Append(" 		   DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
            .Append(" 	   AND BRN_CD IN (:BRN_CD, :ALL_BRN_CD) ")
            .Append("      AND SETTING_NAME = :SETTING_NAME ")
            .Append(" ORDER BY ")
            .Append("          DLR_CD ASC, BRN_CD ASC ")
        End With

        Dim dt As SC3250105DataSet.SystemSettingDataTable = Nothing

        Using query As New DBSelectQuery(Of SC3250105DataSet.SystemSettingDataTable)("SC3250105_004")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
            query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, allDealerCode)
            query.AddParameterWithTypeValue("ALL_BRN_CD", OracleDbType.NVarchar2, allBranchCode)
            query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)

            dt = query.GetData()
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} QUERY:COUNT = {2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dt.Count))

        Return dt

    End Function

    ''' <summary>
    ''' 005:i-CROP→DMSの値に変換された値を基幹コードマップテーブルから取得する
    ''' </summary>
    ''' <param name="allDealerCD">全販売店を意味するワイルドカード販売店コード</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="dmsCodeType">基幹コード区分</param>
    ''' <param name="icropCD1">iCROPコード1</param>
    ''' <param name="icropCD2">iCROPコード2</param>
    ''' <param name="icropCD3">iCROPコード3</param>
    ''' <returns>DmsCodeMapDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetIcropToDmsCode(ByVal allDealerCD As String, _
                                      ByVal dealerCD As String, _
                                      ByVal dmsCodeType As Integer, _
                                      ByVal icropCD1 As String, _
                                      ByVal icropCD2 As String, _
                                      ByVal icropCD3 As String) As SC3250105DataSet.DmsCodeMapDataTable

        Dim sql As New StringBuilder
        With sql
            .Append("   SELECT /* SC3250105_005 */ ")
            .Append(" 		     DMS_CD_1 CODE1 ")                '基幹コード1
            .Append(" 		   , DMS_CD_2 CODE2 ")                '基幹コード2
            .Append(" 		   , DMS_CD_3 CODE3 ")                '基幹コード3
            .Append("     FROM ")
            .Append(" 		     TB_M_DMS_CODE_MAP ")             '基幹コードマップ
            .Append("    WHERE ")
            .Append(" 		     DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
            .Append(" 	   AND   DMS_CD_TYPE = :DMS_CD_TYPE ")
            .Append(" 	   AND   ICROP_CD_1 = :ICROP_CD_1 ")

            If Not String.IsNullOrEmpty(icropCD2) Then
                .AppendLine(" 	   AND   ICROP_CD_2 = :ICROP_CD_2 ")
            End If

            If Not String.IsNullOrEmpty(icropCD3) Then
                .AppendLine(" 	   AND   ICROP_CD_3 = :ICROP_CD_3 ")
            End If

            .AppendLine(" ORDER BY DLR_CD ASC ")
        End With

        Dim dt As SC3250105DataSet.DmsCodeMapDataTable = Nothing

        Using query As New DBSelectQuery(Of SC3250105DataSet.DmsCodeMapDataTable)("SC3250105_005")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCD)
            query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, allDealerCD)
            query.AddParameterWithTypeValue("DMS_CD_TYPE", OracleDbType.NVarchar2, dmsCodeType)
            query.AddParameterWithTypeValue("ICROP_CD_1", OracleDbType.NVarchar2, icropCD1)

            If Not String.IsNullOrEmpty(icropCD2) Then
                query.AddParameterWithTypeValue("ICROP_CD_2", OracleDbType.NVarchar2, icropCD2)
            End If

            If Not String.IsNullOrEmpty(icropCD3) Then
                query.AddParameterWithTypeValue("ICROP_CD_3", OracleDbType.NVarchar2, icropCD3)
            End If

            dt = query.GetData()
        End Using

        Return dt

    End Function


End Class
