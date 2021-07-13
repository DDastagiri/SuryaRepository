'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250103DataSet.vb
'─────────────────────────────────────
'機能： 部品説明画面 データセット
'補足： 
'作成： 2014/08/XX NEC 上野
'更新： 
'更新： 
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Text
Imports Toyota.eCRB.iCROP.DataAccess.SC3250103
Imports System.Globalization
Imports Oracle.DataAccess.Client

Partial Class SC3250103DataSet

    ''' <summary>
    ''' 001:コンテンツエリア表示設定に必要な情報取得
    ''' </summary>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function GetInspecItemInfo(ByVal strINSPEC_ITEM_CD As String
                                                    ) As SC3250103DataSet.InspecItemInfoDataTable
        Dim No As String = "SC3250103_001"
        Dim strMethodName As String = "GetInspecItemInfo"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Try
            Dim dt As New SC3250103DataSet.InspecItemInfoDataTable
            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append("SELECT /* SC3250103_001 */ ")
                .Append("	M1.INSPEC_ITEM_CD ")
                .Append("	,NVL(M1.INSPEC_ITEM_NAME, ' ') AS INSPEC_ITEM_NAME ")
                .Append("	,NVL(M1.SUB_INSPEC_ITEM_NAME, ' ') AS SUB_INSPEC_ITEM_NAME ")
                .Append("	,NVL(M1.PARTS_AREA_URL, ' ') AS PARTS_AREA_URL ")
                .Append("	,NVL(M1.NEW_PARTS_FILE_NAME, ' ') AS NEW_PARTS_FILE_NAME ")
                .Append("	,NVL((SELECT INSPEC_ITEM_CD ")
                .Append("		FROM TB_M_FINAL_INSPECTION_DETAIL ")
                .Append("		WHERE PARTS_GROUP_CD=M1.PARTS_GROUP_CD AND PRIMARY_INSPEC_ITEM_FLG='1' AND ROWNUM=1), ' ') AS PRIMARY_INSPEC_ITEM_CD ")
                .Append("FROM ")
                .Append("	(SELECT ")
                .Append("		INSPEC_ITEM_CD ")
                .Append("		,INSPEC_ITEM_NAME ")
                .Append("		,SUB_INSPEC_ITEM_NAME ")
                .Append("		,PARTS_AREA_URL ")
                .Append("		,NEW_PARTS_FILE_NAME ")
                .Append("		,PARTS_GROUP_CD ")
                .Append("	FROM ")
                .Append("		TB_M_FINAL_INSPECTION_DETAIL ")
                .Append("	WHERE ")
                .Append("		INSPEC_ITEM_CD = :INSPEC_ITEM_CD )M1 ")
            End With

            Using query As New DBSelectQuery(Of SC3250103DataSet.InspecItemInfoDataTable)(No)

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, strINSPEC_ITEM_CD)

                dt = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
                'ログ出力 End *****************************************************************************

                Return dt
            End Using

        Catch ex As Exception
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
            'ログ出力 End *****************************************************************************
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 002:画面URL情報取得
    ''' </summary>
    ''' <param name="inDisplayNumber">表示番号</param>
    ''' <returns>URL情報</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function TB_M_DISP_RELATION_Select(ByVal inDisplayNumber As Long) As DisplayRelationDataTable

        Dim No As String = "SC3250103_002"
        Dim strMethodName As String = "TB_M_DISP_RELATION_Select"

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Try
            'データ格納用
            Dim dt As DisplayRelationDataTable
            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append("SELECT /* SC3250103_002 */ ")
                .Append("   DMS_DISP_ID ")
                .Append("  ,DMS_DISP_URL ")
                .Append("FROM ")
                .Append("  TB_M_DISP_RELATION ")
                .Append("WHERE ")
                .Append("  DMS_DISP_ID = :DMS_DISP_ID ")
            End With

            Using query As New DBSelectQuery(Of DisplayRelationDataTable)(No)

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DMS_DISP_ID", OracleDbType.Long, inDisplayNumber)

                dt = query.GetData()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
                'ログ出力 End *****************************************************************************
                Return dt

            End Using

        Catch ex As Exception
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
            'ログ出力 End *****************************************************************************
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' 003:i-CROP→DMSの値に変換された値を基幹コードマップテーブルから取得する
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
                                      ByVal icropCD3 As String) As SC3250103DataSet.DmsCodeMapDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  allDealerCD, _
                                  dealerCD, _
                                  dmsCodeType, _
                                  icropCD1, _
                                  icropCD2, _
                                  icropCD3))

        Dim sql As New StringBuilder
        With sql
            .Append("   SELECT /* SC3250103_003 */ ")
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

        Dim dt As SC3250103DataSet.DmsCodeMapDataTable = Nothing

        Using query As New DBSelectQuery(Of SC3250103DataSet.DmsCodeMapDataTable)("SC3250103_003")
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

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} QUERY:COUNT = {2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dt.Count))

        Return dt

    End Function

    ''' <summary>
    ''' グラフ作成用グループコード存在チェック用
    ''' </summary>
    ''' <param name="inspecItemcd">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ChkPartsGroupCd(
                            ByVal inspecItemCd As String
                                            ) As SC3250103DataSet.PartsGroupCdDataTable

        Dim sqlNo As String = "SC3250103_004"

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} P1:{2} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inspecItemCd))

        Dim sql As New StringBuilder
        'SQL文作成
        With sql
            .Append("SELECT /* ").Append(sqlNo).Append(" */")
            .Append(" NVL(MFID.PARTS_GROUP_CD, 0) AS PARTS_GROUP_CD")
            .Append(" ")
            .Append("FROM ")
            .Append(" TB_M_FINAL_INSPECTION_DETAIL MFID")
            .Append(" ")
            .Append("WHERE")
            .Append(" MFID.INSPEC_ITEM_CD = :INSPEC_ITEM_CD")
        End With

        Using query As New DBSelectQuery(Of SC3250103DataSet.PartsGroupCdDataTable)(sqlNo)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, inspecItemCd)

            sql = Nothing

            Using dt As SC3250103DataSet.PartsGroupCdDataTable = query.GetData
                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                          "{0}.{1} QUERY:COUNT = {2}", _
                                          Me.GetType.ToString, _
                                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          dt.Count))
                Return dt
            End Using
        End Using

    End Function

    ''' <summary>
    ''' グラフ表示データカウント取得
    ''' </summary>
    ''' <param name="vclVin">VIN</param>
    ''' <param name="partsGroupCd">部品グループコード</param>
    ''' <param name="dealerCd">販売店コード</param>
    ''' <param name="branchCd">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUpsellChartDataCount(
                            ByVal vclVin As String, _
                            ByVal partsGroupCd As String, _
                            ByVal dealerCd As String, _
                            ByVal branchCd As String
                                            ) As Integer

        Dim sqlNo As String = "SC3250103_005"
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Dim sql As New StringBuilder
        'SQL文作成
        With sql
            '※SC3250106DataSet「GetUpsellChartData」メソッド（グラフ表示データ取得）のSQLより
            '　グラフ表示順をなくして、取得数をカウント
            '2014/09/04　SQL文変更　グラフ表示するためのデータがあるかどうかをチェックするためのSQLであり、
            'データが1件以上あるかどうか調べれればいいため、不要な条件やテーブル参照を削除

            .Append("SELECT ( ")

            .Append("SELECT /* ").Append(sqlNo).Append(" */ ")
            .Append("	COUNT(1) AS COUNT ")
            .Append("FROM ")
            .Append("	 TB_M_VEHICLE MV ")
            .Append("	,TB_M_FINAL_INSPECTION_DETAIL MFID ")
            .Append("	,TB_T_FINAL_INSPECTION_HEAD TFIH ")
            .Append("	,TB_T_FINAL_INSPECTION_DETAIL TFID ")
            .Append("	,TB_T_VEHICLE_SVCIN_HIS VSH ")
            .Append("WHERE ")
            .Append("	 MV.VCL_ID = VSH.VCL_ID ")
            .Append("	 AND TFIH.JOB_DTL_ID = TFID.JOB_DTL_ID ")
            .Append("	 AND TFID.INSPEC_ITEM_CD = MFID.INSPEC_ITEM_CD ")
            .Append("	 AND INSTR(VSH.SVCIN_NUM, TFIH.RO_NUM, 1,1) > 0 ")
            .Append("	 AND TFIH.DLR_CD = :DLR_CD ")
            .Append("	 AND TFIH.BRN_CD = :BRN_CD ")
            .Append("	 AND (TFID.RSLT_VAL_BEFORE >= 0 OR TFID.RSLT_VAL_AFTER >= 0) ")
            .Append("	 AND MFID.PARTS_GROUP_CD = :PARTS_GROUP_CD ")
            .Append("	 AND MV.VCL_VIN = :VCL_VIN ")

            '.Append("SELECT /* ").Append(sqlNo).Append(" */ ")
            '.Append("	COUNT(1) AS COUNT ")
            '.Append("FROM ")
            '.Append("	(SELECT DISTINCT ")
            '.Append("		0 AS SORT_KEY ")
            '.Append("		,CASE WHEN JD.INSPECTION_NEED_FLG = '1' THEN TFIH.INSPECTION_APPROVAL_DATETIME ")
            '.Append("		 ELSE TFIH.ROW_UPDATE_DATETIME END INSPECTION_APPROVAL_DATETIME ")
            '.Append("		,NVL(VM.REG_MILE, 0) AS REG_MILE ")
            '.Append("		,TFID.RSLT_VAL_BEFORE AS RSLT_VAL ")
            '.Append("		,MFID.SUB_INSPEC_ITEM_NAME ")
            '.Append("	FROM ")
            '.Append("		 TB_M_VEHICLE MV ")
            '.Append("		,TB_M_FINAL_INSPECTION_DETAIL MFID ")
            '.Append("		,TB_T_FINAL_INSPECTION_HEAD TFIH ")
            '.Append("		,TB_T_FINAL_INSPECTION_DETAIL TFID ")
            '.Append("		,TB_T_JOB_DTL JD ")
            '.Append("		,TB_T_VEHICLE_SVCIN_HIS VSH ")
            '.Append("		,TB_T_VEHICLE_MILEAGE VM ")
            '.Append("	WHERE ")
            '.Append("		 MV.VCL_ID = VSH.VCL_ID ")
            '.Append("		 AND VSH.VCL_MILE_ID = VM.VCL_MILE_ID(+) ")
            '.Append("		 AND TFIH.JOB_DTL_ID = TFID.JOB_DTL_ID ")
            '.Append("		 AND TFIH.JOB_DTL_ID = JD.JOB_DTL_ID ")
            '.Append("		 AND TFID.INSPEC_ITEM_CD = MFID.INSPEC_ITEM_CD ")
            '.Append("		 AND INSTR(VSH.SVCIN_NUM, TFIH.RO_NUM, 1,1) > 0 ")
            '.Append("		 AND TFIH.DLR_CD = :DLR_CD ")
            '.Append("		 AND TFIH.BRN_CD = :BRN_CD ")
            '.Append("		 AND TFID.RSLT_VAL_BEFORE >= 0 ")
            '.Append("		 AND MFID.PARTS_GROUP_CD = :PARTS_GROUP_CD ")
            '.Append("		 AND MV.VCL_VIN = :VCL_VIN ")
            '.Append("	UNION ALL ")
            '.Append("	SELECT DISTINCT ")
            '.Append("		 1 AS SORT_KEY ")
            '.Append("		,CASE WHEN JD.INSPECTION_NEED_FLG = '1' THEN TFIH.INSPECTION_APPROVAL_DATETIME ")
            '.Append("		 ELSE TFIH.ROW_UPDATE_DATETIME END INSPECTION_APPROVAL_DATETIME ")
            '.Append("		,NVL(VM.REG_MILE, 0) AS REG_MILE ")
            '.Append("		,TFID.RSLT_VAL_AFTER AS RSLT_VAL ")
            '.Append("		,MFID.SUB_INSPEC_ITEM_NAME")
            '.Append("	FROM ")
            '.Append("		 TB_M_VEHICLE MV ")
            '.Append("		,TB_M_FINAL_INSPECTION_DETAIL MFID ")
            '.Append("		,TB_T_FINAL_INSPECTION_HEAD TFIH ")
            '.Append("		,TB_T_FINAL_INSPECTION_DETAIL TFID ")
            '.Append("		,TB_T_JOB_DTL JD ")
            '.Append("		,TB_T_VEHICLE_SVCIN_HIS VSH ")
            '.Append("		,TB_T_VEHICLE_MILEAGE VM ")
            '.Append("	WHERE ")
            '.Append("		 MV.VCL_ID = VSH.VCL_ID ")
            '.Append("		 AND VSH.VCL_MILE_ID = VM.VCL_MILE_ID(+) ")
            '.Append("		 AND TFIH.JOB_DTL_ID = TFID.JOB_DTL_ID ")
            '.Append("		 AND TFIH.JOB_DTL_ID = JD.JOB_DTL_ID ")
            '.Append("		 AND TFID.INSPEC_ITEM_CD = MFID.INSPEC_ITEM_CD ")
            '.Append("		 AND INSTR(VSH.SVCIN_NUM, TFIH.RO_NUM, 1,1) > 0 ")
            '.Append("		 AND TFIH.DLR_CD = :DLR_CD ")
            '.Append("		 AND TFIH.BRN_CD = :BRN_CD ")
            '.Append("		 AND TFID.RSLT_VAL_BEFORE <> TFID.RSLT_VAL_AFTER ")
            '.Append("		 AND TFID.RSLT_VAL_AFTER >= 0 ")
            '.Append("		 AND MFID.PARTS_GROUP_CD = :PARTS_GROUP_CD ")
            '.Append("		 AND MV.VCL_VIN = :VCL_VIN) ")

            .Append(" ) ")
            .Append(" + ")
            .Append(" ( ")

            .Append("SELECT  ")
            .Append("    COUNT(1) AS COUNT ")
            .Append("FROM ")
            .Append("     TB_M_VEHICLE MV ")
            .Append("    ,TB_M_FINAL_INSPECTION_DETAIL MFID ")
            .Append("    ,TB_H_FINAL_INSPECTION_HEAD HFIH ")
            .Append("    ,TB_H_FINAL_INSPECTION_DETAIL HFID ")
            .Append("    ,TB_T_VEHICLE_SVCIN_HIS VSH ")
            .Append("WHERE ")
            .Append("     MV.VCL_ID = VSH.VCL_ID ")
            .Append("     AND HFIH.JOB_DTL_ID = HFID.JOB_DTL_ID ")
            .Append("     AND HFID.INSPEC_ITEM_CD = MFID.INSPEC_ITEM_CD ")
            .Append("     AND INSTR(VSH.SVCIN_NUM, HFIH.RO_NUM, 1,1) > 0 ")
            .Append("     AND HFIH.DLR_CD = :DLR_CD ")
            .Append("     AND HFIH.BRN_CD = :BRN_CD ")
            .Append("     AND (HFID.RSLT_VAL_BEFORE >= 0 OR HFID.RSLT_VAL_AFTER >= 0) ")
            .Append("     AND MFID.PARTS_GROUP_CD = :PARTS_GROUP_CD ")
            .Append("     AND MV.VCL_VIN = :VCL_VIN ")

            .Append(" ) AS COUNT ")
            .Append(" FROM DUAL ")

        End With

        Using query As New DBSelectQuery(Of SC3250103DataSet.SC3250103CntDataTable)(sqlNo)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("PARTS_GROUP_CD", OracleDbType.NVarchar2, partsGroupCd)
            query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, vclVin)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCd)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCd)

            sql = Nothing

            Using dt As SC3250103DataSet.SC3250103CntDataTable = query.GetData
                Return dt(0).COUNT
            End Using
        End Using

    End Function

End Class
