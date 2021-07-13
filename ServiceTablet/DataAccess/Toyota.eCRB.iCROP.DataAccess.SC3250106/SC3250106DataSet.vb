'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250106DataSet.vb
'─────────────────────────────────────
'機能： 部品説明/残量グラフDataSet.vb
'補足： 
'作成： 2014/08/XX NEC 村瀬
'更新： 2014/08/xx 
'更新： 2019/12/10 NCN 吉川（FS）次世代サービス業務における車両型式別点検の検証
'─────────────────────────────────────

Option Explicit On

Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core

Partial Class SC3250106DataSet
'2019/12/18　TKM要件:型式対応　START　
#Region "定数"
    ''' <summary>
    ''' VCL_KATASHIKIの初期値(半角スペース) 
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_KATASHIKI_SPACE As String = " "

#End Region
'2019/12/18　TKM要件:型式対応　END　
    '2019/08/02　TKM要件:型式対応　START　↓↓↓
    ''' <summary>
    ''' グラフマスタ設定情報取得
    ''' </summary>
    ''' <param name="vclVin">VIN</param>
    ''' <param name="inspecItemcd">点検項目コード</param>
    ''' <param name="dealerCd">販売店コード</param>
    ''' <param name="branchCd">店舗コード</param>
    ''' <param name="useFlgKatashiki">型式使用フラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUpsellChartSetting(
                            ByVal vclVin As String, _
                            ByVal inspecItemCd As String, _
                            ByVal dealerCd As String, _
                            ByVal branchCd As String, _
                            ByVal useFlgKatashiki As Boolean
                                            ) As SC3250106DataSet.UpsellChartSettingDataTable

        Dim sqlNo As String = "SC3250106_001"
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Dim sql As New StringBuilder
        'SQL文作成
        With sql
            '--VIN = 'TEST_6T153SK1009012609'
            '--INSPEC_ITEM_CD = '7052'
            '            .Append(" SELECT DISTINCT /* ").Append(sqlNo).Append(" */")
            .Append(" SELECT /* ").Append(sqlNo).Append(" */")
            .Append(" MUC.PARTS_GROUP_CD")
            .Append(",MUC.GRAPH_PARTS_MAX_VAL")
            .Append(",MUC.GRAPH_RECOMMEND_REPLACE_VAL")
            .Append(",MUC.GRAPH_GRADUATION")
            .Append(",MUC.GRAPH_DISP_UNIT")
            .Append(" ")
            .Append("FROM ")
            .Append(" TB_M_VEHICLE MV")
            .Append(",TB_M_INSPECTION_COMB MIC")
            .Append(",TB_M_FINAL_INSPECTION_DETAIL MFID")
            .Append(",TB_M_UPSELL_CHARTSETTING MUC")
            .Append(",TB_T_SERVICEIN SV")
            .Append(" ")
            .Append("WHERE")
            '2019/12/02 NCN吉川 TKM要件：型式対応 Start
            .Append(" MIC.MODEL_CD = MV.MODEL_CD ")
            .Append(" AND MIC.MODEL_CD = MUC.MODEL_CD")
            '型式使用時
            If useFlgKatashiki = True Then
                .Append(" AND MIC.VCL_KATASHIKI = MV.VCL_KATASHIKI ")
                .Append(" AND MIC.VCL_KATASHIKI = MUC.VCL_KATASHIKI ")
            Else '型式を半角スペースとして条件
                .Append(" AND MIC.VCL_KATASHIKI = '" & DEFAULT_KATASHIKI_SPACE & "'")
                .Append(" AND MUC.VCL_KATASHIKI = '" & DEFAULT_KATASHIKI_SPACE & "'")
            End If
            '2019/12/02 NCN吉川 TKM要件：型式対応 End
            '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 Start
            '.Append(" AND MIC.GRADE_CD = MUC.GRADE_CD")
            '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 End
            .Append(" AND MIC.INSPEC_ITEM_CD = MFID.INSPEC_ITEM_CD")
            .Append(" AND MFID.PARTS_GROUP_CD = MUC.PARTS_GROUP_CD")
            '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 Start
            '.Append(" AND MIC.GRADE_CD = ' '")
            '2020/02/14 TKM要件：型式対応 GRADE_CD 廃止 End
            .Append(" AND MIC.DLR_CD = :DLR_CD")
            .Append(" AND MIC.BRN_CD = :BRN_CD")
            .Append(" AND MFID.INSPEC_ITEM_CD = :INSPEC_ITEM_CD")
            .Append(" AND MV.VCL_VIN = :VCL_VIN")
            .Append(" AND ROWNUM = 1") 'DISTINCT見直し 12/05 edit
        End With

        Using query As New DBSelectQuery(Of SC3250106DataSet.UpsellChartSettingDataTable)(sqlNo)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, inspecItemCd)
            query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, vclVin)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCd)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCd)

            sql = Nothing

            Using dt As SC3250106DataSet.UpsellChartSettingDataTable = query.GetData
                Return dt
            End Using
        End Using

    End Function

    '2019/08/02　TKM要件:型式対応　END　↑↑↑

    ''' <summary>
    ''' グラフ表示データ取得
    ''' </summary>
    ''' <param name="vclVin">VIN</param>
    ''' <param name="partsGroupCd">部品グループコード</param>
    ''' <param name="graphDspOrder">グラフ表示順</param>
    ''' <param name="dealerCd">販売店コード</param>
    ''' <param name="branchCd">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetUpsellChartData(
                            ByVal vclVin As String, _
                            ByVal partsGroupCd As String, _
                            ByVal graphDspOrder As Integer, _
                            ByVal dealerCd As String, _
                            ByVal branchCd As String
                                            ) As SC3250106DataSet.TempUpsellChartDataDataTable

        Dim sqlNo As String = "SC3250106_002"
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Dim sql As New StringBuilder
        'SQL文作成
        With sql
            '--VIN = 'TEST_6T153SK1009012609'
            '--INSPEC_ITEM_CD = '7052'

            'Before値の取得
            .Append("SELECT /* ").Append(sqlNo).Append(" */")
            .Append(" DISTINCT")
            .Append(" 0 AS SORT_KEY")

            '作業内容.検査必要フラグ='1'(必要)だったら「完成検査承認日時」、'0'(不要)だったら行更新日時を取得
            .Append(",CASE WHEN JD.INSPECTION_NEED_FLG = '1' THEN TFIH.INSPECTION_APPROVAL_DATETIME")
            .Append(" ELSE TFIH.ROW_UPDATE_DATETIME END INSPECTION_APPROVAL_DATETIME")

            '入庫履歴の走行距離(Null時は0をセット)
            .Append(",NVL(VM.REG_MILE, 0) AS REG_MILE")

            .Append(",TFID.RSLT_VAL_BEFORE AS RSLT_VAL")
            .Append(",MFID.SUB_INSPEC_ITEM_NAME")
            .Append(" ")
            .Append("FROM")
            .Append(" TB_M_VEHICLE MV")
            .Append(",TB_M_FINAL_INSPECTION_DETAIL MFID")
            .Append(",TB_T_FINAL_INSPECTION_HEAD TFIH")
            .Append(",TB_T_FINAL_INSPECTION_DETAIL TFID")
            .Append(",TB_T_JOB_DTL JD")
            .Append(",TB_T_VEHICLE_SVCIN_HIS VSH")
            .Append(",TB_T_VEHICLE_MILEAGE VM")
            .Append(" ")
            .Append("WHERE ")
            .Append(" MV.VCL_ID = VSH.VCL_ID")
            .Append(" AND VSH.VCL_MILE_ID = VM.VCL_MILE_ID(+) ")
            .Append(" AND TFIH.JOB_DTL_ID = TFID.JOB_DTL_ID")
            .Append(" AND TFIH.JOB_DTL_ID = JD.JOB_DTL_ID")
            .Append(" AND TFID.INSPEC_ITEM_CD = MFID.INSPEC_ITEM_CD")
            .Append(" AND INSTR(VSH.SVCIN_NUM, TFIH.RO_NUM, 1,1) > 0") '入庫管理番号にRO番号が含まれているか？
            .Append(" AND TFIH.DLR_CD = :DLR_CD")
            .Append(" AND TFIH.BRN_CD = :BRN_CD")
            .Append(" AND TFID.RSLT_VAL_BEFORE >= 0") '初期値が「-1」なので「0」も入力値とみなし条件に含む
            .Append(" AND MFID.PARTS_GROUP_CD = :PARTS_GROUP_CD")
            .Append(" AND MFID.GRAPH_DSPORDER = :GRAPH_DSPORDER")
            .Append(" AND MV.VCL_VIN = :VCL_VIN")

            .Append(" ")
            .Append("UNION ALL")
            .Append(" ")

            'After値の取得
            .Append("SELECT")
            .Append(" DISTINCT")
            .Append(" 1 AS SORT_KEY")

            '作業内容.検査必要フラグ='1'(必要)だったら「完成検査承認日時」、'0'(不要)だったら行更新日時を取得
            .Append(",CASE WHEN JD.INSPECTION_NEED_FLG = '1' THEN TFIH.INSPECTION_APPROVAL_DATETIME")
            .Append(" ELSE TFIH.ROW_UPDATE_DATETIME END INSPECTION_APPROVAL_DATETIME")

            '入庫履歴の走行距離(Null時は0をセット)
            .Append(",NVL(VM.REG_MILE, 0) AS REG_MILE")

            .Append(",TFID.RSLT_VAL_AFTER AS RSLT_VAL")
            .Append(",MFID.SUB_INSPEC_ITEM_NAME")
            .Append(" ")
            .Append("FROM")
            .Append(" TB_M_VEHICLE MV")
            .Append(",TB_M_FINAL_INSPECTION_DETAIL MFID")
            .Append(",TB_T_FINAL_INSPECTION_HEAD TFIH")
            .Append(",TB_T_FINAL_INSPECTION_DETAIL TFID")
            .Append(",TB_T_JOB_DTL JD")
            .Append(",TB_T_VEHICLE_SVCIN_HIS VSH")
            .Append(",TB_T_VEHICLE_MILEAGE VM")
            .Append(" ")
            .Append("WHERE ")
            .Append(" MV.VCL_ID = VSH.VCL_ID")
            .Append(" AND VSH.VCL_MILE_ID = VM.VCL_MILE_ID(+) ")
            .Append(" AND TFIH.JOB_DTL_ID = TFID.JOB_DTL_ID")
            .Append(" AND TFIH.JOB_DTL_ID = JD.JOB_DTL_ID")
            .Append(" AND TFID.INSPEC_ITEM_CD = MFID.INSPEC_ITEM_CD")
            .Append(" AND INSTR(VSH.SVCIN_NUM, TFIH.RO_NUM, 1,1) > 0") '入庫管理番号にRO番号が含まれているか？
            .Append(" AND TFIH.DLR_CD = :DLR_CD")
            .Append(" AND TFIH.BRN_CD = :BRN_CD")
            .Append(" AND TFID.RSLT_VAL_BEFORE <> TFID.RSLT_VAL_AFTER") 'Before値=After値は抽出しない
            .Append(" AND TFID.RSLT_VAL_AFTER >= 0") '初期値が「-1」なので「0」も入力値とみなし条件に含む
            .Append(" AND MFID.PARTS_GROUP_CD = :PARTS_GROUP_CD")
            .Append(" AND MFID.GRAPH_DSPORDER = :GRAPH_DSPORDER")
            .Append(" AND MV.VCL_VIN = :VCL_VIN")

            '2017/02/07　ライフサイクル対応追加　START　↓↓↓

            .Append(" ")
            .Append("UNION ALL")
            .Append(" ")

            .Append("SELECT")
            .Append(" DISTINCT")
            .Append(" 0 AS SORT_KEY")
            .Append(",CASE WHEN JD.INSPECTION_NEED_FLG = '1' THEN TFIH.INSPECTION_APPROVAL_DATETIME")
            .Append(" ELSE TFIH.ROW_UPDATE_DATETIME END INSPECTION_APPROVAL_DATETIME")
            .Append(",NVL(VM.REG_MILE, 0) AS REG_MILE")
            .Append(",TFID.RSLT_VAL_BEFORE AS RSLT_VAL")
            .Append(",MFID.SUB_INSPEC_ITEM_NAME")
            .Append(" ")
            .Append("FROM")
            .Append(" TB_M_VEHICLE MV")
            .Append(",TB_M_FINAL_INSPECTION_DETAIL MFID")
            .Append(",TB_H_FINAL_INSPECTION_HEAD TFIH")
            .Append(",TB_H_FINAL_INSPECTION_DETAIL TFID")
            .Append(",TB_H_JOB_DTL JD")
            .Append(",TB_T_VEHICLE_SVCIN_HIS VSH")
            .Append(",TB_T_VEHICLE_MILEAGE VM")
            .Append(" ")
            .Append("WHERE ")
            .Append(" MV.VCL_ID = VSH.VCL_ID")
            .Append(" AND VSH.VCL_MILE_ID = VM.VCL_MILE_ID(+) ")
            .Append(" AND TFIH.JOB_DTL_ID = TFID.JOB_DTL_ID")
            .Append(" AND TFIH.JOB_DTL_ID = JD.JOB_DTL_ID")
            .Append(" AND TFID.INSPEC_ITEM_CD = MFID.INSPEC_ITEM_CD")
            .Append(" AND INSTR(VSH.SVCIN_NUM, TFIH.RO_NUM, 1,1) > 0") '入庫管理番号にRO番号が含まれているか？
            .Append(" AND TFIH.DLR_CD = :DLR_CD")
            .Append(" AND TFIH.BRN_CD = :BRN_CD")
            .Append(" AND TFID.RSLT_VAL_BEFORE >= 0") '初期値が「-1」なので「0」も入力値とみなし条件に含む
            .Append(" AND MFID.PARTS_GROUP_CD = :PARTS_GROUP_CD")
            .Append(" AND MFID.GRAPH_DSPORDER = :GRAPH_DSPORDER")
            .Append(" AND MV.VCL_VIN = :VCL_VIN")

            .Append(" ")
            .Append("UNION ALL")
            .Append(" ")

            .Append("SELECT")
            .Append(" DISTINCT")
            .Append(" 1 AS SORT_KEY")
            .Append(",CASE WHEN JD.INSPECTION_NEED_FLG = '1' THEN TFIH.INSPECTION_APPROVAL_DATETIME")
            .Append(" ELSE TFIH.ROW_UPDATE_DATETIME END INSPECTION_APPROVAL_DATETIME")
            .Append(",NVL(VM.REG_MILE, 0) AS REG_MILE")
            .Append(",TFID.RSLT_VAL_AFTER AS RSLT_VAL")
            .Append(",MFID.SUB_INSPEC_ITEM_NAME")
            .Append(" ")
            .Append("FROM")
            .Append(" TB_M_VEHICLE MV")
            .Append(",TB_M_FINAL_INSPECTION_DETAIL MFID")
            .Append(",TB_H_FINAL_INSPECTION_HEAD TFIH")
            .Append(",TB_H_FINAL_INSPECTION_DETAIL TFID")
            .Append(",TB_H_JOB_DTL JD")
            .Append(",TB_T_VEHICLE_SVCIN_HIS VSH")
            .Append(",TB_T_VEHICLE_MILEAGE VM")
            .Append(" ")
            .Append("WHERE ")
            .Append(" MV.VCL_ID = VSH.VCL_ID")
            .Append(" AND VSH.VCL_MILE_ID = VM.VCL_MILE_ID(+) ")
            .Append(" AND TFIH.JOB_DTL_ID = TFID.JOB_DTL_ID")
            .Append(" AND TFIH.JOB_DTL_ID = JD.JOB_DTL_ID")
            .Append(" AND TFID.INSPEC_ITEM_CD = MFID.INSPEC_ITEM_CD")
            .Append(" AND INSTR(VSH.SVCIN_NUM, TFIH.RO_NUM, 1,1) > 0") '入庫管理番号にRO番号が含まれているか？
            .Append(" AND TFIH.DLR_CD = :DLR_CD")
            .Append(" AND TFIH.BRN_CD = :BRN_CD")
            .Append(" AND TFID.RSLT_VAL_BEFORE <> TFID.RSLT_VAL_AFTER") 'Before値=After値は抽出しない
            .Append(" AND TFID.RSLT_VAL_AFTER >= 0") '初期値が「-1」なので「0」も入力値とみなし条件に含む
            .Append(" AND MFID.PARTS_GROUP_CD = :PARTS_GROUP_CD")
            .Append(" AND MFID.GRAPH_DSPORDER = :GRAPH_DSPORDER")
            .Append(" AND MV.VCL_VIN = :VCL_VIN")

            '2017/02/07　ライフサイクル対応追加　END　↑↑↑

            .Append(" ")
            .Append("ORDER BY")
            .Append(" INSPECTION_APPROVAL_DATETIME")
            .Append(",SORT_KEY")


            ''Before値の取得
            '.Append("SELECT /* ").Append(sqlNo).Append(" */")
            '.Append(" DISTINCT")
            '.Append(" 0 AS SORT_KEY")

            ''作業内容.検査必要フラグ='1'(必要)だったら「完成検査承認日時」、'0'(不要)だったら行更新日時を取得
            '.Append(",CASE WHEN JD.INSPECTION_NEED_FLG = '1' THEN TFIH.INSPECTION_APPROVAL_DATETIME")
            '.Append(" ELSE TFIH.ROW_UPDATE_DATETIME END INSPECTION_APPROVAL_DATETIME")
            ''.Append(",TFIH.INSPECTION_APPROVAL_DATETIME")

            ''入庫履歴の走行距離(Null時は0をセット)
            '.Append(",NVL(VM.REG_MILE, 0) AS REG_MILE")
            ''.Append(",SV.SVCIN_MILE")

            '.Append(",TFID.RSLT_VAL_BEFORE AS RSLT_VAL")
            '.Append(",MFID.SUB_INSPEC_ITEM_NAME")
            '.Append(" ")
            '.Append("FROM")
            '.Append(" TB_M_VEHICLE MV")
            '.Append(",TB_M_FINAL_INSPECTION_DETAIL MFID")
            '.Append(",TB_T_FINAL_INSPECTION_HEAD TFIH")
            '.Append(",TB_T_FINAL_INSPECTION_DETAIL TFID")
            '.Append(",TB_T_SERVICEIN SV")
            '.Append(",TB_T_JOB_DTL JD")
            '.Append(",TB_T_RO_INFO RO")
            '.Append(",TB_T_VEHICLE_SVCIN_HIS VSH")
            '.Append(",TB_T_VEHICLE_MILEAGE VM")
            '.Append(" ")
            '.Append("WHERE ")
            '.Append(" MV.VCL_ID = SV.VCL_ID")
            '.Append(" AND SV.DLR_CD = TFIH.DLR_CD")
            '.Append(" AND SV.BRN_CD = TFIH.BRN_CD")
            '.Append(" AND SV.SVCIN_ID = JD.SVCIN_ID")
            '.Append(" AND SV.DLR_CD = RO.DLR_CD")
            '.Append(" AND SV.BRN_CD = RO.BRN_CD")
            '.Append(" AND SV.SVCIN_ID = RO.SVCIN_ID")
            '.Append(" AND SV.DLR_CD = VSH.DLR_CD(+)")
            '.Append(" AND SV.CST_ID = VSH.CST_ID(+)")
            '.Append(" AND SV.VCL_ID = VSH.VCL_ID(+)")
            ''.Append(" AND SV.RSLT_DELI_DATETIME = VSH.SVCIN_DELI_DATE(+)") '結合できないため条件から削除
            '.Append(" AND VSH.VCL_MILE_ID = VM.VCL_MILE_ID(+) ")
            '.Append(" AND JD.JOB_DTL_ID = TFIH.JOB_DTL_ID")
            '.Append(" AND TFIH.JOB_DTL_ID = TFID.JOB_DTL_ID")
            '.Append(" AND MFID.INSPEC_ITEM_CD = TFID.INSPEC_ITEM_CD")
            '.Append(" AND MFID.PARTS_GROUP_CD = :PARTS_GROUP_CD")
            '.Append(" AND MV.VCL_VIN = :VCL_VIN")
            '.Append(" AND SV.DLR_CD = :DLR_CD")
            '.Append(" AND SV.BRN_CD = :BRN_CD")
            '.Append(" AND TFID.RSLT_VAL_BEFORE >= 0") '初期値が「-1」なので「0」も入力値とみなし条件に含む
            '.Append(" AND MFID.GRAPH_DSPORDER = :GRAPH_DSPORDER")
            '.Append(" AND INSTR(VSH.SVCIN_NUM, RO.RO_NUM, 1,1) > 0") '入庫管理番号にRO番号が含まれているか？

            '.Append(" ")
            '.Append("UNION ALL")
            '.Append(" ")

            ''After値の取得
            '.Append("SELECT")
            '.Append(" DISTINCT")
            '.Append(" 1 AS SORT_KEY")

            ''作業内容.検査必要フラグ='1'(必要)だったら「完成検査承認日時」、'0'(不要)だったら行更新日時を取得
            '.Append(",CASE WHEN JD.INSPECTION_NEED_FLG = '1' THEN TFIH.INSPECTION_APPROVAL_DATETIME")
            '.Append(" ELSE TFIH.ROW_UPDATE_DATETIME END INSPECTION_APPROVAL_DATETIME")
            ''.Append(",TFIH.INSPECTION_APPROVAL_DATETIME")

            ''入庫履歴の走行距離(Null時は0をセット)
            '.Append(",NVL(VM.REG_MILE, 0) AS REG_MILE")
            ''.Append(",SV.SVCIN_MILE")

            '.Append(",TFID.RSLT_VAL_AFTER AS RSLT_VAL")
            '.Append(",MFID.SUB_INSPEC_ITEM_NAME")
            '.Append(" ")
            '.Append("FROM")
            '.Append(" TB_M_VEHICLE MV")
            '.Append(",TB_M_FINAL_INSPECTION_DETAIL MFID")
            '.Append(",TB_T_FINAL_INSPECTION_HEAD TFIH")
            '.Append(",TB_T_FINAL_INSPECTION_DETAIL TFID")
            '.Append(",TB_T_SERVICEIN SV")
            '.Append(",TB_T_JOB_DTL JD")
            '.Append(",TB_T_RO_INFO RO")
            '.Append(",TB_T_VEHICLE_SVCIN_HIS VSH")
            '.Append(",TB_T_VEHICLE_MILEAGE VM")
            '.Append(" ")
            '.Append("WHERE ")
            '.Append(" MV.VCL_ID = SV.VCL_ID")
            '.Append(" AND SV.DLR_CD = TFIH.DLR_CD")
            '.Append(" AND SV.BRN_CD = TFIH.BRN_CD")
            '.Append(" AND SV.SVCIN_ID = JD.SVCIN_ID")
            '.Append(" AND SV.DLR_CD = RO.DLR_CD")
            '.Append(" AND SV.BRN_CD = RO.BRN_CD")
            '.Append(" AND SV.SVCIN_ID = RO.SVCIN_ID")
            '.Append(" AND SV.DLR_CD = VSH.DLR_CD(+)")
            '.Append(" AND SV.CST_ID = VSH.CST_ID(+)")
            '.Append(" AND SV.VCL_ID = VSH.VCL_ID(+)")
            ''.Append(" AND SV.RSLT_DELI_DATETIME = VSH.SVCIN_DELI_DATE(+)") '結合できないため条件から削除
            '.Append(" AND VSH.VCL_MILE_ID = VM.VCL_MILE_ID(+) ")
            '.Append(" AND JD.JOB_DTL_ID = TFIH.JOB_DTL_ID")
            '.Append(" AND TFIH.JOB_DTL_ID = TFID.JOB_DTL_ID")
            '.Append(" AND MFID.INSPEC_ITEM_CD = TFID.INSPEC_ITEM_CD")
            '.Append(" AND TFID.RSLT_VAL_BEFORE <> TFID.RSLT_VAL_AFTER") 'Before値=After値は抽出しない
            '.Append(" AND MFID.PARTS_GROUP_CD = :PARTS_GROUP_CD")
            '.Append(" AND MV.VCL_VIN = :VCL_VIN")
            '.Append(" AND SV.DLR_CD = :DLR_CD")
            '.Append(" AND SV.BRN_CD = :BRN_CD")
            '.Append(" AND TFID.RSLT_VAL_AFTER >= 0") '初期値が「-1」なので「0」も入力値とみなし条件に含む
            '.Append(" AND MFID.GRAPH_DSPORDER = :GRAPH_DSPORDER")
            '.Append(" AND INSTR(VSH.SVCIN_NUM, RO.RO_NUM, 1,1) > 0") '入庫管理番号にRO番号が含まれているか？
            '.Append(" ")
            '.Append("ORDER BY")
            '.Append(" INSPECTION_APPROVAL_DATETIME")
            '.Append(",SORT_KEY")

            '※Before値=After値のデータはAfter値に-1をセットする
            '.Append("SELECT /* ").Append(sqlNo).Append(" */")
            '.Append(" TFIH.INSPECTION_APPROVAL_DATETIME")
            '.Append(",SV.SVCIN_MILE")
            '.Append(",TFID.RSLT_VAL_BEFORE")
            '.Append(",CASE WHEN TFID.RSLT_VAL_BEFORE = TFID.RSLT_VAL_AFTER THEN -1")
            '.Append(" ELSE TFID.RSLT_VAL_AFTER END RSLT_VAL_AFTER")
            ''.Append(",TFID.RSLT_VAL_AFTER")
            '.Append(",MFID.INSPEC_ITEM_CD")
            '.Append(" ")
            '.Append("FROM")
            '.Append(" TB_M_VEHICLE MV")
            '.Append(",TB_M_FINAL_INSPECTION_DETAIL MFID")
            '.Append(",TB_T_FINAL_INSPECTION_HEAD TFIH")
            '.Append(",TB_T_FINAL_INSPECTION_DETAIL TFID")
            '.Append(",TB_T_SERVICEIN SV")
            '.Append(",TB_T_JOB_DTL JD")
            '.Append(" ")
            '.Append("WHERE ")
            '.Append(" MV.VCL_ID = SV.VCL_ID")
            '.Append(" AND SV.SVCIN_ID = JD.SVCIN_ID")
            '.Append(" AND SV.DLR_CD = TFIH.DLR_CD")
            '.Append(" AND SV.BRN_CD = TFIH.BRN_CD")
            '.Append(" AND JD.JOB_DTL_ID = TFIH.JOB_DTL_ID")
            '.Append(" AND TFIH.JOB_DTL_ID = TFID.JOB_DTL_ID")
            '.Append(" AND MFID.INSPEC_ITEM_CD = TFID.INSPEC_ITEM_CD")
            '.Append(" AND MFID.PARTS_GROUP_CD = :PARTS_GROUP_CD")
            '.Append(" AND MV.VCL_VIN = :VCL_VIN")
            '.Append(" AND SV.DLR_CD = :DLR_CD")
            '.Append(" AND SV.BRN_CD = :BRN_CD")
            ''.Append(" AND TFID.INSPEC_RSLT_CD = :INSPEC_RSLT_CD")
            '.Append(" AND TFID.RSLT_VAL_BEFORE >= 0") '初期値が「-1」なので「0」も入力値とみなし条件に含む
            '.Append(" AND MFID.GRAPH_DSPORDER = :GRAPH_DSPORDER")
            '.Append(" ")
            '.Append("ORDER BY")
            '.Append(" MFID.GRAPH_DSPORDER")
            '.Append(",TFIH.INSPECTION_APPROVAL_DATETIME")
        End With

        Using query As New DBSelectQuery(Of SC3250106DataSet.TempUpsellChartDataDataTable)(sqlNo)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("PARTS_GROUP_CD", OracleDbType.NVarchar2, partsGroupCd)
            query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, vclVin)
            query.AddParameterWithTypeValue("GRAPH_DSPORDER", OracleDbType.Int32, graphDspOrder)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCd)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCd)

            sql = Nothing

            Using dt As SC3250106DataSet.TempUpsellChartDataDataTable = query.GetData
                Return dt
            End Using
        End Using

    End Function

#Region "販売店システム設定データ取得"
    ''' <summary>
    ''' SC3250106_003:販売店システム設定から設定値を取得する
    ''' </summary>
    ''' <param name="dealerCd">販売店コード</param>
    ''' <param name="branchCd">店舗コード</param>
    ''' <param name="allDealerCd">全店舗を示す販売店コード</param>
    ''' <param name="allBranchCd">全店舗を示す店舗コード</param>
    ''' <param name="settingName">販売店システム設定名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetDlrSystemSettingValue(ByVal dealerCd As String, _
                                             ByVal branchCd As String, _
                                             ByVal allDealerCd As String, _
                                             ByVal allBranchCd As String, _
                                             ByVal settingName As String) As SC3250106DataSet.DlrSystemSettingValueDataTable

        Dim sqlNo As String = "SC3250106_003"
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                   "{0} P1:{1} P2:{2} P3:{3} P4:{4} P5:{5} ", _
                                   methodName, _
                                   dealerCd, _
                                   branchCd, _
                                   allDealerCd, _
                                   allBranchCd, _
                                   settingName))

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* ").Append(sqlNo).Append(" */")
            .Append(" SETTING_VAL")
            .Append(" ")
            .Append("FROM")
            .Append(" TB_M_SYSTEM_SETTING_DLR")
            .Append(" ")
            .Append("WHERE")
            .Append(" DLR_CD IN (:DLR_CD, :ALL_DLR_CD)")
            .Append(" AND BRN_CD IN (:BRN_CD, :ALL_BRN_CD)")
            .Append(" AND SETTING_NAME = :SETTING_NAME")
            .Append(" ")
            .Append("ORDER BY ")
            .Append(" DLR_CD ASC, BRN_CD ASC")
        End With

        Using query As New DBSelectQuery(Of SC3250106DataSet.DlrSystemSettingValueDataTable)(sqlNo)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCd)
            query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.Char, allDealerCd)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCd)
            query.AddParameterWithTypeValue("ALL_BRN_CD", OracleDbType.Char, allBranchCd)
            query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.Char, settingName)

            sql = Nothing

            Using dt As SC3250106DataSet.DlrSystemSettingValueDataTable = query.GetData
                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                          "{0} QUERY:COUNT = {1}", _
                                          methodName, _
                                          dt.Count))
                Return dt
            End Using
        End Using
    End Function
#End Region







    ''' <summary>
    ''' 032:i-CROP→DMSの値に変換された値を基幹コードマップテーブルから取得する
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
                                      ByVal icropCD3 As String) As SC3250106DataSet.DmsCodeMapDataTable

        Dim sqlNo As String = "SC3250106_004"

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* ").Append(sqlNo).Append(" */")
            .Append(" DMS_CD_1 CODE1")                '基幹コード1
            .Append(",DMS_CD_2 CODE2")                '基幹コード2
            .Append(",DMS_CD_3 CODE3")                '基幹コード3
            .Append(" ")
            .Append("FROM")
            .Append(" TB_M_DMS_CODE_MA ")             '基幹コードマップ
            .Append(" ")
            .Append("WHERE")
            .Append(" DLR_CD IN (:DLR_CD, :ALL_DLR_CD)")
            .Append("AND DMS_CD_TYPE = :DMS_CD_TYPE")
            .Append("AND ICROP_CD_1 = :ICROP_CD_1")

            If Not String.IsNullOrEmpty(icropCD2) Then
                .AppendLine("AND ICROP_CD_2 = :ICROP_CD_2")
            End If

            If Not String.IsNullOrEmpty(icropCD3) Then
                .AppendLine("AND ICROP_CD_3 = :ICROP_CD_3")
            End If

            .Append(" ")
            .AppendLine("ORDER BY DLR_CD ASC")
        End With

        Dim dt As SC3250106DataSet.DmsCodeMapDataTable = Nothing

        Using query As New DBSelectQuery(Of SC3250106DataSet.DmsCodeMapDataTable)(sqlNo)
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

    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    ''' <summary>
    ''' 型式使用フラグの取得
    ''' </summary>
    ''' <param name="strRoNum">R/O番号</param>
    ''' <param name="strDlrCd">販売店コード</param>
    ''' <param name="strBrnCd">店舗コード</param>
    ''' <returns>登録状態 DataTable TRANSACTION_EXIST : 1 or 0 , HISTORY_EXIST : 1 or 0 </returns>
    ''' <remarks>点検組み合わせマスタ、整備属性マスタ、車両マスタと紐づく型式値を取得する</remarks>
    Public Function GetDlrCdExistMst(ByVal strRoNum As String, _
                                     ByVal strDlrCd As String, _
                                     ByVal strBrnCd As String) As DataTable

        Dim dt As DataTable

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using query As New DBSelectQuery(Of DataTable)("SC3250106_005")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT")
                .Append("    /* SC3250106_005 */")
                .Append("    CASE")
                .Append("        WHEN IC.VCL_KATASHIKI <> MV.VCL_KATASHIKI")
                .Append("    OR  NVL(IC.VCL_KATASHIKI, ' ') = ' '")
                .Append("    OR  NVL(MV.VCL_KATASHIKI, ' ') = ' ' THEN '0'")
                .Append("        ELSE '1'")
                .Append("    END KATASHIKI_EXIST")
                .Append(" FROM")
                .Append("    TB_M_VEHICLE MV")
                .Append("    LEFT OUTER JOIN")
                .Append("        (")
                .Append("            SELECT")
                .Append("                IC.MODEL_CD,")
                .Append("                IC.DLR_CD,")
                .Append("                IC.BRN_CD,")
                .Append("                IC.VCL_KATASHIKI")
                .Append("            FROM")
                .Append("                TB_M_INSPECTION_COMB IC")
                .Append("            WHERE")
                .Append("                IC.DLR_CD IN(:DLR_CD, 'XXXXX')")
                .Append("            AND IC.BRN_CD IN(:BRN_CD, 'XXX')")
                .Append("            ORDER BY")
                .Append("                IC.VCL_KATASHIKI DESC")
                .Append("        ) IC")
                .Append("    ON  IC.MODEL_CD = MV.MODEL_CD")
                .Append("    AND IC.VCL_KATASHIKI = MV.VCL_KATASHIKI")
                .Append("    LEFT OUTER JOIN")
                .Append("        (")
                .Append("            SELECT")
                .Append("                TSI.VCL_ID,")
                .Append("                TSI.RO_NUM")
                .Append("            FROM")
                .Append("                TB_T_SERVICEIN TSI")
                .Append("            WHERE")
                .Append("                TSI.RO_NUM = :RO_NUM")
                .Append("            AND TSI.DLR_CD = :DLR_CD")
                .Append("            AND TSI.BRN_CD = :BRN_CD")
                .Append("    UNION ")
                .Append("            SELECT")
                .Append("                HSI.VCL_ID,")
                .Append("                HSI.RO_NUM")
                .Append("            FROM")
                .Append("                TB_H_SERVICEIN HSI")
                .Append("            WHERE")
                .Append("                HSI.RO_NUM = :RO_NUM")
                .Append("            AND HSI.DLR_CD = :DLR_CD")
                .Append("            AND HSI.BRN_CD = :BRN_CD")
                .Append("            AND ROWNUM = 1")
                .Append("        ) SI")
                .Append("    ON  SI.RO_NUM = :RO_NUM")
                .Append(" WHERE")
                .Append("    ROWNUM = 1")
                .Append("    AND MV.VCL_ID = SI.VCL_ID")
            End With
            'クエリ設定
            query.CommandText = sql.ToString()
            'パラメータ設定
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, strRoNum)     'R/O番号
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDlrCd)     '販売店
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBrnCd)     '店舗
            '結果取得
            dt = query.GetData()
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dt
    End Function
    '2019/07/05　TKM要件:型式対応　END    ↑↑↑

End Class
