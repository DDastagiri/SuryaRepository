'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3230101.aspx.vb
'─────────────────────────────────────
'機能： メインメニュー(FM)画面 データセット
'補足： 
'作成： 2014/02/XX NEC 桜井
'更新： 
'更新： 
'─────────────────────────────────────

Option Explicit On
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.Foreman.MainMenu.DataAccess
Imports System.Globalization


Partial Class SC3230101DataSet

#Region "定数"

    ''' <summary>
    ''' RO連番
    ''' </summary>
    ''' <remarks>RO情報のRO連番</remarks>
    Private Structure RO_SEQ
        ''' <summary>
        ''' 通常作業(親R/O)
        ''' </summary>
        ''' <remarks>通常作業：0</remarks>
        Public Const NormalWork As Integer = 0

    End Structure

    ''' <summary>
    ''' ROステータス
    ''' </summary>
    ''' <remarks>RO情報のROステータス</remarks>
    Private Structure RO_Status
        ''' <summary>
        ''' FM承認待ち
        ''' </summary>
        ''' <remarks>FM承認待ち："20"</remarks>
        Public Const FM_Appr As String = "20"

        ''' <summary>
        ''' R/Oキャンセル
        ''' </summary>
        ''' <remarks>R/Oキャンセル："99"</remarks>
        Public Const RO_Cancel As String = "99"

        ''' <summary>
        ''' 納車済み
        ''' </summary>
        ''' <remarks>納車済み："90"</remarks>
        Public Const Delivery As String = "90"

    End Structure

    ''' <summary>
    ''' 完成検査ステータス
    ''' </summary>
    ''' <remarks>作業内容の完成検査ステータス</remarks>
    Private Structure Ins_Status
        ''' <summary>
        ''' 完成検査承認待ち
        ''' </summary>
        ''' <remarks>完成検査承認待ち："1"</remarks>
        Public Const InsRltAppr As String = "1"

        ''' <summary>
        '''  完成検査未完了
        ''' </summary>
        ''' <remarks>完成検査未完了："0"</remarks>
        Public Const insRltNotComp As String = "0"

    End Structure

    ''' <summary>
    ''' 検査必要フラグ
    ''' </summary>
    ''' <remarks>完成検査で承認要否を表すフラグ</remarks>
    Private Structure Ins_Need_FLG

        ''' <summary>
        ''' 検査必要フラグオン
        ''' </summary>
        ''' <remarks>検査必要：必要</remarks>
        Public Const insNeedOn As String = "1"

    End Structure

    ''' <summary>
    ''' キャンセルフラグ
    ''' </summary>
    ''' <remarks>作業内容のキャンセルフラグ</remarks>
    Private Structure Cancel_FLG
        ''' <summary>
        ''' デフォルト値
        ''' </summary>
        ''' <remarks>デフォルト値：""</remarks>
        Public Const DefaultValue As String = ""

        ''' <summary>
        ''' 有効
        ''' </summary>
        ''' <remarks>有効："0"</remarks>
        Public Const Valid As String = "0"

        ''' <summary>
        ''' キャンセル
        ''' </summary>
        ''' <remarks>キャンセル："1"</remarks>
        Public Const Cancel As String = "1"

    End Structure

    ''' <summary>
    ''' ストール利用ステータス
    ''' </summary>
    ''' <remarks>ストール利用のストール利用ステータス</remarks>
    Private Structure Stall_Status
        ''' <summary>着工指示待ち</summary>
        ''' <remarks>着工指示待ち："00"</remarks>
        Public Const WaitIndicate As String = "00"

        ''' <summary>作業開始待ち</summary>
        ''' <remarks>作業開始待ち："01"</remarks>
        Public Const WaitJobStart As String = "01"

        ''' <summary>作業中</summary>
        ''' <remarks>作業中："02"</remarks>
        Public Const Working As String = "02"

        ''' <summary>完了</summary>
        ''' <remarks>完了："03"</remarks>
        Public Const Complete As String = "03"

        ''' <summary>作業指示の一部の作業が中断</summary>
        ''' <remarks>作業指示の一部の作業が中断："04"</remarks>
        Public Const StopPart As String = "04"

        ''' <summary>中断</summary>
        ''' <remarks>中断："05"</remarks>
        Public Const Interrupt As String = "05"

        ''' <summary>日跨ぎ終了</summary>
        ''' <remarks>日跨ぎ終了："06"</remarks>
        Public Const HimatagiEnd As String = "06"

        ''' <summary>未来店客</summary>
        ''' <remarks>未来店客："07"</remarks>
        Public Const Noshow As String = "07"

    End Structure

    ''' <summary>
    ''' サービスステータス
    ''' </summary>
    ''' <remarks>サービス入庫のサービスステータス</remarks>
    Private Structure Svc_Status
        ''' <summary>キャンセル</summary>
        ''' <remarks>キャンセル："02"</remarks>
        Public Const Cancel As String = "02"

        ''' <summary>納車済み</summary>
        ''' <remarks>納車済み："13"</remarks>
        Public Const Delivery As String = "13"

    End Structure

#End Region

#Region "完成検査承認待ちデータ取得"
    'TR-SVT-TMT-20160909-001(レスポンス対応)↓
    'Public Function GetInsRltApprData(dlrCD As String, brnCD As String) As SC3230101InsRltApprDataTable
    '    'TMT2販社 BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 
    '    Using query As New DBSelectQuery(Of SC3230101InsRltApprDataTable)("SC3230101_001")

    '        Dim strSql As New StringBuilder
    '        With strSql
    '            .Append("SELECT /* SC3230101_001 */ ")
    '            .Append("        MAX_STALL.RO_NUM ")
    '            .Append("      , MAX_STALL.RO_SEQ ")
    '            .Append("      , MAX_STALL.DLR_CD ")
    '            .Append("      , MAX_STALL.BRN_CD ")
    '            .Append("      , MAX_STALL.RO_CREATE_DATETIME ")
    '            .Append("      , MAX_STALL.VISITSEQ ")
    '            .Append("      , MAX_STALL.REZID ")
    '            .Append("      , MAX_STALL.VIN ")
    '            .Append("      , MAX_STALL.SCHE_DELI_DATETIME ")
    '            .Append("      , MAX_STALL.CARWASH_NEED_FLG ")
    '            .Append("      , MAX_STALL.REG_NUM ")
    '            .Append("      , MAX_STALL.RSLT_START_DATETIME AS CAR_WASH_START ")
    '            .Append("      , MAX_STALL.RSLT_END_DATETIME AS CAR_WASH_END ")
    '            .Append("      , MAX_STALL.INSPECTION_REQ_STF_CD ")
    '            .Append("      , MAX_STALL.JOB_DTL_ID ")
    '            .Append("      , MAX_STALL.MAX_SCHE_END_DATETIME ")
    '            .Append("      , SU.RSLT_END_DATETIME ")
    '            .Append("      , ST.STALL_NAME_SHORT ")
    '            .Append("      , SUM(NVL(SU_TIME.SCHE_WORKTIME, 0)) AS SUM_SCHE_WORKTIME ")
    '            .Append("  FROM TB_T_STALL_USE SU ")
    '            .Append("     , TB_M_STALL ST ")
    '            .Append("     , TB_T_STALL_USE SU_TIME ")
    '            .Append("     , (")
    '            .Append("    SELECT ")
    '            .Append("            RI.RO_NUM ")
    '            '.Append("          , RI.RO_SEQ ")
    '            .Append("          , MIN(RI.RO_SEQ) AS RO_SEQ ")
    '            .Append("          , RI.DLR_CD ")
    '            .Append("          , RI.BRN_CD ")
    '            '.Append("          , RI.RO_CREATE_DATETIME ")
    '            .Append("          , MIN(RI.RO_CREATE_DATETIME) AS RO_CREATE_DATETIME ")
    '            .Append("          , SV.VISITSEQ ")
    '            .Append("          , SV.REZID ")
    '            .Append("          , SV.VIN ")
    '            .Append("          , SI.SCHE_DELI_DATETIME ")
    '            .Append("          , SI.CARWASH_NEED_FLG ")
    '            .Append("          , VD.REG_NUM ")
    '            .Append("          , CW.RSLT_START_DATETIME ")
    '            .Append("          , CW.RSLT_END_DATETIME ")
    '            '.Append("          , JD.INSPECTION_REQ_STF_CD ")
    '            .Append("          , IH.INSPECTION_REQ_STF_CD ")
    '            .Append("          , JD.JOB_DTL_ID ")
    '            .Append("          , MAX(SU_MAX.STALL_USE_ID) AS MAX_ID ")
    '            .Append("          , MAX(SU_MAX.SCHE_END_DATETIME) AS MAX_SCHE_END_DATETIME ")
    '            .Append("      FROM TB_T_RO_INFO RI ")
    '            .Append("         , TBL_SERVICE_VISIT_MANAGEMENT SV ")
    '            .Append("         , TB_T_SERVICEIN SI ")
    '            .Append("         , TB_M_VEHICLE_DLR VD ")
    '            .Append("         , TB_T_CARWASH_RESULT CW ")
    '            .Append("         , TB_T_JOB_INSTRUCT JI ")
    '            .Append("         , TB_T_JOB_DTL JD ")
    '            .Append("         , TB_T_STALL_USE SU_MAX ")
    '            .Append("         , TB_T_FINAL_INSPECTION_HEAD IH ")
    '            .Append("     WHERE RI.VISIT_ID = SV.VISITSEQ ")
    '            .Append("       AND RI.SVCIN_ID = SI.SVCIN_ID ")
    '            .Append("       AND SI.DLR_CD = VD.DLR_CD ")
    '            .Append("       AND SI.VCL_ID = VD.VCL_ID ")
    '            .Append("       AND SI.SVCIN_ID = CW.SVCIN_ID (+) ")
    '            .Append("       AND RI.RO_NUM = JI.RO_NUM ")
    '            .Append("       AND RI.RO_SEQ = JI.RO_SEQ ")
    '            .Append("       AND JI.JOB_DTL_ID = JD.JOB_DTL_ID ")
    '            'TMT2販社 BTS135 完成検査承認時にR/Oステータスが更新されない -横展開修正- START
    '            .Append("       AND JD.DLR_CD=RI.DLR_CD ")
    '            .Append("       AND JD.BRN_CD=RI.BRN_CD ")
    '            'TMT2販社 BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 start
    '            .Append("       AND RI.DLR_CD = :DLR_CD ")
    '            .Append("       AND RI.BRN_CD = :BRN_CD ")
    '            'TMT2販社 BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 end
    '            'TMT2販社 BTS135 完成検査承認時にR/Oステータスが更新されない -横展開修正- END
    '            .Append("       AND JD.JOB_DTL_ID = SU_MAX.JOB_DTL_ID ")
    '            .Append("       AND JD.JOB_DTL_ID = IH.JOB_DTL_ID ")
    '            '.Append("       AND RI.RO_SEQ = :RO_SEQ ")
    '            .Append("       AND RI.RO_STATUS < :RO_STATUS ")
    '            .Append("       AND JD.INSPECTION_STATUS = :INS_STATUS ")
    '            '.Append("       AND JD.CANCEL_FLG IN (:CANCEL_FLG1, :CANCEL_FLG2) ")
    '            .Append("     GROUP BY RI.RO_NUM ")
    '            '.Append("            , RI.RO_SEQ ")
    '            .Append("            , RI.DLR_CD ")
    '            .Append("            , RI.BRN_CD ")
    '            '.Append("            , RI.RO_CREATE_DATETIME ")
    '            .Append("            , SV.VISITSEQ ")
    '            .Append("            , SV.REZID ")
    '            .Append("            , SV.VIN ")
    '            .Append("            , SI.SCHE_DELI_DATETIME ")
    '            .Append("            , SI.CARWASH_NEED_FLG ")
    '            .Append("            , VD.REG_NUM ")
    '            .Append("            , CW.RSLT_START_DATETIME ")
    '            .Append("            , CW.RSLT_END_DATETIME ")
    '            '.Append("            , JD.INSPECTION_REQ_STF_CD ")
    '            .Append("            , IH.INSPECTION_REQ_STF_CD ")
    '            .Append("            , JD.JOB_DTL_ID) MAX_STALL ")
    '            .Append(" WHERE SU.STALL_USE_ID = MAX_STALL.MAX_ID ")
    '            .Append("   AND SU.STALL_ID = ST.STALL_ID (+) ")
    '            .Append("   AND SU.STALL_USE_STATUS IN (:STALL_USE_STATUS00, ") 'ストール利用ステータス：着工指示待ち
    '            .Append(":STALL_USE_STATUS01, ")                                'ストール利用ステータス：作業開始待ち
    '            .Append(":STALL_USE_STATUS02, ")                                'ストール利用ステータス：作業中
    '            .Append(":STALL_USE_STATUS03, ")                                'ストール利用ステータス：完成
    '            .Append(":STALL_USE_STATUS04, ")                                'ストール利用ステータス：作業指示の一部の作業が中断
    '            .Append(":STALL_USE_STATUS06, ")                                'ストール利用ステータス：日跨ぎ終了
    '            .Append(":STALL_USE_STATUS07) ")                                'ストール利用ステータス：未来店客
    '            .Append("   AND MAX_STALL.JOB_DTL_ID = SU_TIME.JOB_DTL_ID (+) ")
    '            .Append("   AND SU_TIME.STALL_USE_STATUS (+) IN ")
    '            .Append("(:STALL_USE_STATUS00, :STALL_USE_STATUS01) ")   '[ストール利用ステータス：着工指示待ち or 作業開始待ち]の場合の予定作業時間の合計を残作業時間として取得
    '            .Append(" GROUP BY MAX_STALL.RO_NUM ")
    '            .Append("        , MAX_STALL.RO_SEQ ")
    '            .Append("        , MAX_STALL.DLR_CD ")
    '            .Append("        , MAX_STALL.BRN_CD ")
    '            .Append("        , MAX_STALL.RO_CREATE_DATETIME ")
    '            .Append("        , MAX_STALL.VISITSEQ ")
    '            .Append("        , MAX_STALL.REZID ")
    '            .Append("        , MAX_STALL.VIN ")
    '            .Append("        , MAX_STALL.SCHE_DELI_DATETIME ")
    '            .Append("        , MAX_STALL.CARWASH_NEED_FLG ")
    '            .Append("        , MAX_STALL.REG_NUM ")
    '            .Append("        , MAX_STALL.RSLT_START_DATETIME ")
    '            .Append("        , MAX_STALL.RSLT_END_DATETIME ")
    '            .Append("        , MAX_STALL.INSPECTION_REQ_STF_CD ")
    '            .Append("        , MAX_STALL.JOB_DTL_ID ")
    '            .Append("        , MAX_STALL.MAX_SCHE_END_DATETIME ")
    '            .Append("        , SU.RSLT_END_DATETIME ")
    '            .Append("        , ST.STALL_NAME_SHORT ")

    '        End With

    '        query.CommandText = strSql.ToString()

    '        'query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Int64, RO_SEQ.NormalWork)                    '通常作業
    '        query.AddParameterWithTypeValue("RO_STATUS", OracleDbType.Char, RO_Status.RO_Cancel)                'R/Oキャンセル
    '        query.AddParameterWithTypeValue("INS_STATUS", OracleDbType.Char, Ins_Status.InsRltAppr)             '完成検査承認待ち
    '        'query.AddParameterWithTypeValue("CANCEL_FLG1", OracleDbType.Char, Cancel_FLG.Valid)                 'キャンセルフラグ：有効(キャンセルではない)
    '        'query.AddParameterWithTypeValue("CANCEL_FLG2", OracleDbType.Char, Cancel_FLG.DefaultValue)          'キャンセルフラグ：デフォルト値(キャンセルではないと判断)
    '        query.AddParameterWithTypeValue("STALL_USE_STATUS00", OracleDbType.Char, Stall_Status.WaitIndicate) '着工指示待ち
    '        query.AddParameterWithTypeValue("STALL_USE_STATUS01", OracleDbType.Char, Stall_Status.WaitJobStart) '作業開始待ち
    '        query.AddParameterWithTypeValue("STALL_USE_STATUS02", OracleDbType.Char, Stall_Status.Working)      '作業中
    '        query.AddParameterWithTypeValue("STALL_USE_STATUS03", OracleDbType.Char, Stall_Status.Complete)     '完成
    '        query.AddParameterWithTypeValue("STALL_USE_STATUS04", OracleDbType.Char, Stall_Status.StopPart)     '作業指示の一部の作業が中断
    '        query.AddParameterWithTypeValue("STALL_USE_STATUS06", OracleDbType.Char, Stall_Status.HimatagiEnd)  '日跨ぎ終了
    '        query.AddParameterWithTypeValue("STALL_USE_STATUS07", OracleDbType.Char, Stall_Status.Noshow)       '未来店客
    '        ' 2015/03/31 BTS256 他販売店・店舗が表示されないよう修正 start
    '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                            '販売店コード
    '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                            '店舗コード
    '        ' 2015/03/31 BTS256 他販売店・店舗が表示されないよう修正 end

    '        Return query.GetData()

    '    End Using
    'End Function

    ''' <summary>
    ''' 完成検査承認待ち基本データ取得
    ''' </summary>
    ''' <returns>完成検査承認待ち基本データ</returns>
    ''' <remarks></remarks>
    Public Function GetInsRltApprDataBase(dlrCD As String, brnCD As String) As SC3230101InsRltApprBaseDataTable
        Using query As New DBSelectQuery(Of SC3230101InsRltApprBaseDataTable)("SC3230101_003")

            Dim strSql As New StringBuilder
            With strSql

                .Append("SELECT ")
                .Append("        RI.RO_NUM ")
                .Append("      , MIN(RI.RO_SEQ) AS RO_SEQ ")
                .Append("      , RI.DLR_CD ")
                .Append("      , RI.BRN_CD ")
                .Append("      , MIN(RI.RO_CREATE_DATETIME) AS RO_CREATE_DATETIME ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする start
                '.Append("      , SV.VISITSEQ ")
                '.Append("      , SV.REZID ")
                '.Append("      , SV.VIN ")
                .Append("      , RI.VISIT_ID AS VISITSEQ ")
                .Append("      , NVL(SV.REZID , -2) AS REZID ")
                .Append("      , MV.VCL_VIN AS VIN")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする end
                .Append("      , SI.SCHE_DELI_DATETIME ")
                .Append("      , SI.CARWASH_NEED_FLG ")
                .Append("      , VD.REG_NUM ")
                .Append("      , CW.RSLT_START_DATETIME ")
                .Append("      , CW.RSLT_END_DATETIME ")
                .Append("      , IH.INSPECTION_REQ_STF_CD ")
                .Append("      , JD.JOB_DTL_ID ")
                .Append("      , MAX(SU_MAX.STALL_USE_ID) AS MAX_ID ")
                .Append("      , MAX(SU_MAX.SCHE_END_DATETIME) AS MAX_SCHE_END_DATETIME ")
                .Append("      , SI.SVCIN_ID ")
                .Append("      , VD.IMP_VCL_FLG ")
                .Append("  FROM ")
                .Append("       TB_T_RO_INFO RI ")
                .Append("     , TBL_SERVICE_VISIT_MANAGEMENT SV ")
                .Append("     , TB_T_SERVICEIN SI ")
                .Append("     , TB_M_VEHICLE_DLR VD ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする start
                .Append("     , TB_M_VEHICLE MV ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする end
                .Append("     , TB_T_CARWASH_RESULT CW ")
                .Append("     , TB_T_JOB_INSTRUCT JI ")
                .Append("     , TB_T_JOB_DTL JD ")
                .Append("     , TB_T_STALL_USE SU_MAX ")
                .Append("     , TB_T_FINAL_INSPECTION_HEAD IH ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする start
                '.Append(" WHERE RI.VISIT_ID = SV.VISITSEQ ")
                .Append(" WHERE RI.VISIT_ID = SV.VISITSEQ (+) ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする end
                .Append("   AND RI.SVCIN_ID = SI.SVCIN_ID ")
                .Append("   AND SI.DLR_CD = VD.DLR_CD ")
                .Append("   AND SI.VCL_ID = VD.VCL_ID ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする start
                .Append("   AND VD.VCL_ID = MV.VCL_ID ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする end
                .Append("   AND SI.SVCIN_ID = CW.SVCIN_ID (+) ")
                .Append("   AND RI.RO_NUM = JI.RO_NUM ")
                .Append("   AND RI.RO_SEQ = JI.RO_SEQ ")
                .Append("   AND JI.JOB_DTL_ID = JD.JOB_DTL_ID ")
                .Append("   AND JD.DLR_CD=RI.DLR_CD ")
                .Append("   AND JD.BRN_CD=RI.BRN_CD ")
                .Append("   AND RI.DLR_CD = :DLR_CD")
                .Append("   AND RI.BRN_CD = :BRN_CD")
                .Append("   AND JD.JOB_DTL_ID = SU_MAX.JOB_DTL_ID ")
                .Append("   AND JD.JOB_DTL_ID = IH.JOB_DTL_ID ")
                .Append("   AND RI.RO_STATUS < :RO_DELIVERY")
                .Append("   AND JD.INSPECTION_STATUS = :INS_STATUS")
                .Append("   AND JD.CANCEL_FLG <> :CANCEL_FLG_CANCEL ")
                .Append("   AND SI.SVC_STATUS NOT IN (:SVC_CANCEL , :SVC_DELIVERY)")
                .Append(" GROUP BY RI.RO_NUM ")
                .Append("        , RI.DLR_CD ")
                .Append("        , RI.BRN_CD ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする start
                '.Append("        , SV.VISITSEQ ")
                .Append("        , RI.VISIT_ID ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする end
                .Append("        , SV.REZID ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする start
                '.Append("        , SV.VIN ")
                .Append("        , MV.VCL_VIN ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする end
                .Append("        , SI.SCHE_DELI_DATETIME ")
                .Append("        , SI.CARWASH_NEED_FLG ")
                .Append("        , VD.REG_NUM ")
                .Append("        , CW.RSLT_START_DATETIME ")
                .Append("        , CW.RSLT_END_DATETIME ")
                .Append("        , IH.INSPECTION_REQ_STF_CD ")
                .Append("        , JD.JOB_DTL_ID ")
                .Append("        , SI.SVCIN_ID ")
                .Append("        , VD.IMP_VCL_FLG ")

            End With

            query.CommandText = strSql.ToString()

            '各パラメータのセット
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                            '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                            '店舗コード
            query.AddParameterWithTypeValue("RO_DELIVERY", OracleDbType.NVarchar2, RO_Status.Delivery)            'ROステータス：納車済み
            query.AddParameterWithTypeValue("CANCEL_FLG_CANCEL", OracleDbType.NVarchar2, Cancel_FLG.Cancel)     'キャンセルフラグ：キャンセル
            query.AddParameterWithTypeValue("SVC_CANCEL", OracleDbType.NVarchar2, Svc_Status.Cancel)            'サービスステータス：キャンセル
            query.AddParameterWithTypeValue("SVC_DELIVERY", OracleDbType.NVarchar2, Svc_Status.Delivery)        'サービスステータス：納車済み
            query.AddParameterWithTypeValue("INS_STATUS", OracleDbType.NVarchar2, Ins_Status.InsRltAppr)        '完成検査承認待ち

            Return query.GetData()


        End Using
    End Function
    'TR-SVT-TMT-20160909-001(レスポンス対応)↑

    'TR-SVT-TMT-20160909-001(レスポンス対応)↓
    ''' <summary>
    ''' 完成検査承認待ちデータ取得(ストール情報)
    ''' </summary>
    ''' <returns>完成検査承認待ちデータ(ストール情報)</returns>
    ''' <remarks></remarks>
    Public Function GetInsRltApprDataStall(ByVal ParamStallUseId As String) As SC3230101InsRltApprStallDataTable

        Using query As New DBSelectQuery(Of SC3230101InsRltApprStallDataTable)("SC3230101_004")

            Dim strSql As New StringBuilder
            With strSql

                .Append("SELECT ")
                .Append("        SU.STALL_USE_ID ")
                .Append("      , SU.JOB_DTL_ID")
                .Append("      , SU.RSLT_END_DATETIME ")
                .Append("      , ST.STALL_NAME_SHORT ")
                .Append("  FROM ")
                .Append("      TB_T_STALL_USE SU ")
                .Append("     ,TB_M_STALL ST ")
                .Append(" WHERE ")
                .Append("       SU.STALL_USE_ID IN (" & ParamStallUseId & ")  ")
                .Append("   AND SU.STALL_ID = ST.STALL_ID (+) ")
                .Append("   AND SU.STALL_USE_STATUS IN (:STALL_USE_STATUS00,  ")
                .Append("   :STALL_USE_STATUS01,  ")
                .Append("   :STALL_USE_STATUS02,  ")
                .Append("   :STALL_USE_STATUS03,  ")
                .Append("   :STALL_USE_STATUS04,  ")
                .Append("   :STALL_USE_STATUS06,  ")
                .Append("   :STALL_USE_STATUS07) ")

            End With

            query.CommandText = strSql.ToString()

            '各パラメータのセット
            query.AddParameterWithTypeValue("STALL_USE_STATUS00", OracleDbType.NVarchar2, Stall_Status.WaitIndicate) '着工指示待ち
            query.AddParameterWithTypeValue("STALL_USE_STATUS01", OracleDbType.NVarchar2, Stall_Status.WaitJobStart) '作業開始待ち
            query.AddParameterWithTypeValue("STALL_USE_STATUS02", OracleDbType.NVarchar2, Stall_Status.Working)      '作業中
            query.AddParameterWithTypeValue("STALL_USE_STATUS03", OracleDbType.NVarchar2, Stall_Status.Complete)     '完成
            query.AddParameterWithTypeValue("STALL_USE_STATUS04", OracleDbType.NVarchar2, Stall_Status.StopPart)     '作業指示の一部の作業が中断
            query.AddParameterWithTypeValue("STALL_USE_STATUS06", OracleDbType.NVarchar2, Stall_Status.HimatagiEnd)  '日跨ぎ終了
            query.AddParameterWithTypeValue("STALL_USE_STATUS07", OracleDbType.NVarchar2, Stall_Status.Noshow)       '未来店客

            Return query.GetData()
        End Using
    End Function
    'TR-SVT-TMT-20160909-001(レスポンス対応)↑

    'TR-SVT-TMT-20160909-001(レスポンス対応)↓
    ''' <summary>
    ''' 完成検査承認待ちデータ取得(予定作業時間)
    ''' </summary>
    ''' <returns>完成検査承認待ちデータ(予定作業時間)</returns>
    ''' <remarks></remarks>
    Public Function GetInsRltApprDataWorktime(ByVal ParamJobDtlId As String) As SC3230101InsRltApprWorktimeDataTable

        Using query As New DBSelectQuery(Of SC3230101InsRltApprWorktimeDataTable)("SC3230101_005")

            Dim strSql As New StringBuilder
            With strSql

                .Append("SELECT ")
                .Append("       SU_TIME.STALL_USE_ID")
                .Append("      ,SU_TIME.JOB_DTL_ID")
                .Append("      ,SU_TIME.SCHE_WORKTIME")
                .Append("  FROM ")
                .Append("       TB_T_STALL_USE SU_TIME ")
                .Append(" WHERE ")
                .Append("SU_TIME.JOB_DTL_ID IN (" & ParamJobDtlId & ")")
                .Append("   AND SU_TIME.STALL_USE_STATUS (+) IN ")
                .Append("(:STALL_USE_STATUS00, :STALL_USE_STATUS01) ")

            End With

            query.CommandText = strSql.ToString()

            '各パラメータのセット
            query.AddParameterWithTypeValue("STALL_USE_STATUS00", OracleDbType.NVarchar2, Stall_Status.WaitIndicate) '着工指示待ち
            query.AddParameterWithTypeValue("STALL_USE_STATUS01", OracleDbType.NVarchar2, Stall_Status.WaitJobStart) '作業開始待ち

            Return query.GetData()
        End Using
    End Function
    'TR-SVT-TMT-20160909-001(レスポンス対応)↑

#End Region

#Region "追加作業承認待ちデータ取得"
    ''' <summary>
    ''' 追加作業承認待ちデータ取得
    ''' </summary>
    ''' <returns>追加作業承認待ちデータ</returns>
    ''' <remarks></remarks>
    Public Function GetAddJobApprData(dlrCD As String, brnCD As String) As SC3230101AddJobApprDataTable
        'TMT2販社 BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 
        Using query As New DBSelectQuery(Of SC3230101AddJobApprDataTable)("SC3230101_002")

            Dim strSql As New StringBuilder
            With strSql
                .Append("SELECT /* SC3230101_002 */ ")
                .Append("        RI.RO_NUM ")
                .Append("      , RI.RO_SEQ ")
                .Append("      , RI.DLR_CD ")
                .Append("      , RI.BRN_CD ")
                .Append("      , RI.RO_CHECK_STF_CD ")
                .Append("      , RI.RO_CREATE_DATETIME ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする start
                '.Append("      , SV.VISITSEQ ")
                '.Append("      , SV.VIN ")
                .Append("      , RI.VISIT_ID AS VISITSEQ ")
                .Append("      , MV.VCL_VIN AS VIN")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする end
                .Append("      , SI.SCHE_DELI_DATETIME ")
                .Append("      , SI.CARWASH_NEED_FLG ")
                .Append("      , VD.REG_NUM ")
                .Append("      , CW.RSLT_START_DATETIME AS CAR_WASH_START ")
                .Append("      , CW.RSLT_END_DATETIME AS CAR_WASH_END ")
                .Append("      , MIN(JD.DMS_JOB_DTL_ID) AS DMS_JOB_DTL_ID ")
                .Append("      , MAX(NVL(SU_GRP.SCHE_END_DATETIME, TO_DATE('1900-1-1', 'YYYY-MM-DD'))) AS MAX_SCHE_END_DATETIME ")
                .Append("      , SUM(NVL(SU_GRP.SCHE_WORKTIME, 0)) AS SUM_SCHE_WORKTIME ")
                .Append("      , SI.SVCIN_ID ")
                .Append("      , VD.IMP_VCL_FLG ")
                .Append("  FROM TB_T_RO_INFO RI ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする start
                '.Append("     , TBL_SERVICE_VISIT_MANAGEMENT SV ")
                .Append("     , TB_M_VEHICLE MV ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする end
                .Append("     , TB_T_SERVICEIN SI ")
                .Append("     , TB_M_VEHICLE_DLR VD ")
                .Append("     , TB_T_CARWASH_RESULT CW ")
                .Append("     , TB_T_JOB_DTL JD ")
                .Append("     , TB_T_STALL_USE SU_GRP ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする start
                '.Append(" WHERE RI.VISIT_ID = SV.VISITSEQ ")
                '.Append("   AND RI.SVCIN_ID = SI.SVCIN_ID ")
                .Append(" WHERE RI.SVCIN_ID = SI.SVCIN_ID ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする end
                'TMT2販社 BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 start
                .Append("   AND RI.DLR_CD = :DLR_CD ")
                .Append("   AND RI.BRN_CD = :BRN_CD ")
                'TMT2販社 BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 end
                .Append("   AND SI.DLR_CD = VD.DLR_CD ")
                .Append("   AND SI.VCL_ID = VD.VCL_ID ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする start
                .Append("   AND VD.VCL_ID = MV.VCL_ID ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする end
                .Append("   AND SI.SVCIN_ID = CW.SVCIN_ID (+) ")
                .Append("   AND SI.SVCIN_ID = JD.SVCIN_ID (+) ")
                .Append("   AND JD.JOB_DTL_ID = SU_GRP.JOB_DTL_ID (+) ")
                .Append("   AND RI.RO_SEQ > :RO_SEQ ")
                .Append("   AND RI.RO_STATUS = :RO_STATUS ")
                .Append("   AND SU_GRP.STALL_USE_STATUS (+) IN ")
                .Append("(:STALL_USE_STATUS00, :STALL_USE_STATUS01) ")   '[ストール利用ステータス：着工指示待ち or 作業開始待ち]の場合の予定作業時間の合計を残作業時間として取得
                .Append(" GROUP BY RI.RO_NUM ")
                .Append("        , RI.RO_SEQ ")
                .Append("        , RI.DLR_CD ")
                .Append("        , RI.BRN_CD ")
                .Append("        , RI.RO_CHECK_STF_CD ")
                .Append("        , RI.RO_CREATE_DATETIME ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする start
                '.Append("        , SV.VISITSEQ ")
                '.Append("        , SV.VIN ")
                'TR-SVT-TMT-20160512-001 サービス来店者管理が存在しない場合にもFMメインを有効とする end
                .Append("        , RI.VISIT_ID ")
                .Append("        , MV.VCL_VIN ")
                .Append("        , SI.SCHE_DELI_DATETIME ")
                .Append("        , SI.CARWASH_NEED_FLG ")
                .Append("        , VD.REG_NUM ")
                .Append("        , CW.RSLT_START_DATETIME ")
                .Append("        , CW.RSLT_END_DATETIME ")
                .Append("        , SI.SVCIN_ID ")
                .Append("        , VD.IMP_VCL_FLG ")
            End With

            query.CommandText = strSql.ToString()

            query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Int64, RO_SEQ.NormalWork)        '追加作業(通常作業より大なりで絞り込む)
            query.AddParameterWithTypeValue("RO_STATUS", OracleDbType.NVarchar2, RO_Status.FM_Appr)      'FM承認待ち
            query.AddParameterWithTypeValue("STALL_USE_STATUS00", OracleDbType.NVarchar2, Stall_Status.WaitIndicate) '着工指示待ち
            query.AddParameterWithTypeValue("STALL_USE_STATUS01", OracleDbType.NVarchar2, Stall_Status.WaitJobStart) '作業開始待ち
            'TMT2販社 BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 start
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)                        '販売店コード
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCD)                        '店舗コード
            'TMT2販社 BTS256 他販売店・店舗が表示されないよう修正 2015/03/31 end

            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 残完成検査区分取得
    ''' </summary>
    ''' <param name="inSvcinIdList">サービス入庫IDリスト</param>
    ''' <returns>残完成検査区分  "0"(残完成検査未完了) "1"(残完成検査承認待ち) "2"(なし)</returns>
    ''' <remarks></remarks>
    Function GetRemainInspectionStatus(ByVal inSvcinIdList As List(Of Decimal)) As SC3230101GetRemainInspectionStatusDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim InspectData As New SC3230101GetRemainInspectionStatusDataTable

        'サービス入庫IDがない場合、空白テーブルを戻す
        If IsNothing(inSvcinIdList) OrElse inSvcinIdList.Count = 0 Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END ReturnCount:[{2}]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , InspectData.Count.ToString))

            Return InspectData
        End If

        '★★★パラメーター確認用ログ START
        'If inSvcinIdList.Count > 0 Then
        '    Dim sbSvcin As New StringBuilder
        '    Dim logI As Integer = 1
        '    For Each logSvcinId As String In inSvcinIdList
        '        sbSvcin.Append(logSvcinId)
        '        If Not inSvcinIdList.Count() = logI Then
        '            sbSvcin.Append(",")
        '        End If
        '        logI = logI + 1
        '    Next
        '    Logger.Info(String.Format("■■■ GetRemainInspectionStatus PARAM inSvcinIdList : [{0}]", sbSvcin.ToString()))
        'End If
        '★★★パラメーター確認用ログ END

        Using query As New DBSelectQuery(Of SC3230101GetRemainInspectionStatusDataTable)("SC3230101_006")

            Dim strSql As New StringBuilder
            With strSql
                .AppendLine(" SELECT /* SC3230101_006 */ ")
                .AppendLine("   SVCIN_ID, ")
                .AppendLine("   COUNT(1) AS ROW_COUNT, ")
                .AppendLine("   MIN(INSPECTION_STATUS) AS REMAIN_INSPECTION_STATUS ")
                .AppendLine(" FROM ")
                .AppendLine("   TB_T_JOB_DTL ")
                .AppendLine(" WHERE ")
                .AppendLine("     SVCIN_ID IN ( ")

                'サービス入庫をカンマ区切りでパラメータ化
                Dim i As Integer = 1
                For Each svcinId As String In inSvcinIdList
                    .Append(" :SVCIN_ID_IN" & CStr(i))
                    query.AddParameterWithTypeValue("SVCIN_ID_IN" & CStr(i), OracleDbType.Decimal, svcinId)
                    If Not inSvcinIdList.Count() = i Then
                        .Append(",")
                    End If
                    i = i + 1
                Next

                .AppendLine("   ")
                .AppendLine("                 ) ")
                .AppendLine(" AND INSPECTION_NEED_FLG = :INSPECTION_NEED_FLG_ON ")
                .AppendLine(" AND INSPECTION_STATUS IN (:INSPECTION_STATUS_NOT_FINISH,:INSPECTION_STATUS_WAIT_APPROVE)")
                .AppendLine(" AND CANCEL_FLG <> :CANCEL_FLG_CANCEL ")
                .AppendLine(" GROUP BY ")
                .AppendLine("   SVCIN_ID ")

            End With

            query.CommandText = strSql.ToString()

            query.AddParameterWithTypeValue("INSPECTION_NEED_FLG_ON", OracleDbType.NVarchar2, Ins_Need_FLG.insNeedOn)
            query.AddParameterWithTypeValue("INSPECTION_STATUS_NOT_FINISH", OracleDbType.NVarchar2, Ins_Status.insRltNotComp)
            query.AddParameterWithTypeValue("INSPECTION_STATUS_WAIT_APPROVE", OracleDbType.NVarchar2, Ins_Status.InsRltAppr)
            query.AddParameterWithTypeValue("CANCEL_FLG_CANCEL", OracleDbType.NVarchar2, Cancel_FLG.Cancel)

            InspectData = query.GetData()

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END ReturnCount:[{2}]" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , InspectData.Count.ToString))

        'Return rtnKbn
        Return InspectData
    End Function

    ''' <summary>
    ''' 作業終了予定情報取得
    ''' </summary>
    ''' <param name="inDlrCd">販売店コード</param>
    ''' <param name="inBrnCd">店舗コード</param>
    ''' <param name="inSvcinIdList">サービス入庫IDリスト</param>
    ''' <returns>作業終了予定時刻  実績終了日時、見込終了日時、予定終了日時の順に入力値がある値を設定</returns>
    ''' <remarks></remarks>
    Function GetScheEndInfo(ByVal inDlrCd As String, ByVal inBrnCd As String, ByVal inSvcinIdList As List(Of Decimal)) As SC3230101MaxEndDateInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START PARAMS inDlrCd : [{2}] , inBrnCd : [{3}]" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , inDlrCd _
                                , inBrnCd))

        Dim InspectData As New SC3230101MaxEndDateInfoDataTable

        'サービス入庫IDがない場合、空白テーブルを戻す
        If IsNothing(inSvcinIdList) OrElse inSvcinIdList.Count = 0 Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END ReturnCount:[{2}]" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , InspectData.Count.ToString))

            Return InspectData
        End If

        '★★★パラメーター確認用ログ START
        'If inSvcinIdList.Count > 0 Then
        '    Dim sbSvcin As New StringBuilder
        '    Dim logI As Integer = 1
        '    For Each logSvcinId As String In inSvcinIdList
        '        sbSvcin.Append(logSvcinId)
        '        If Not inSvcinIdList.Count() = logI Then
        '            sbSvcin.Append(",")
        '        End If
        '        logI = logI + 1
        '    Next
        '    Logger.Info(String.Format("■■■ GetScheEndInfo PARAM inSvcinIdList : [{0}]", sbSvcin.ToString()))
        'End If
        '★★★パラメーター確認用ログ END

        Using query As New DBSelectQuery(Of SC3230101MaxEndDateInfoDataTable)("SC3230101_007")

            Dim strSql As New StringBuilder
            With strSql
                .AppendLine("   SELECT /* SC3230101_007 */ ")
                .AppendLine("          T1.SVCIN_ID ")
                .AppendLine("        , MAX( ")
                .AppendLine("           CASE ")
                .AppendLine("            WHEN T3.RSLT_END_DATETIME <> TO_DATE('19000101000000','YYYYMMDDHH24MISS') THEN T3.RSLT_END_DATETIME ")
                .AppendLine("            WHEN T3.PRMS_END_DATETIME <> TO_DATE('19000101000000','YYYYMMDDHH24MISS') THEN T3.PRMS_END_DATETIME ")
                .AppendLine("            ELSE T3.SCHE_END_DATETIME ")
                .AppendLine("           END ")
                .AppendLine("              ) AS MAX_END_DATETIME ")
                .AppendLine("     FROM ")
                .AppendLine("          TB_T_SERVICEIN T1 ")
                .AppendLine("        , TB_T_JOB_DTL T2 ")
                .AppendLine("        , TB_T_STALL_USE T3 ")
                .AppendLine("    WHERE ")
                .AppendLine("          T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("      AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("      AND T2.CANCEL_FLG = N'0' ")
                .AppendLine("      AND T3.STALL_USE_ID IN ( ")
                .AppendLine("                                 SELECT MAX(S3.STALL_USE_ID) ")
                .AppendLine("                                   FROM ")
                .AppendLine("                                        TB_T_SERVICEIN S1 ")
                .AppendLine("                                      , TB_T_JOB_DTL S2 ")
                .AppendLine("                                      , TB_T_STALL_USE S3 ")
                .AppendLine("                                  WHERE ")
                .AppendLine("                                        S1.SVCIN_ID = S2.SVCIN_ID ")
                .AppendLine("                                    AND S2.JOB_DTL_ID = S3.JOB_DTL_ID ")
                .AppendLine("                                    AND S3.DLR_CD = :DLR_CD ")
                .AppendLine("                                    AND S3.BRN_CD = :BRN_CD ")
                .AppendLine("                                    AND S1.RSLT_DELI_DATETIME = TO_DATE('19000101000000','YYYYMMDDHH24MISS') ")
                .AppendLine("                                    AND S1.SVCIN_ID IN ( ")

                'サービス入庫をカンマ区切りでパラメータ化
                Dim i As Integer = 1
                For Each svcinId As String In inSvcinIdList
                    .Append(" :SVCIN_ID_IN" & CStr(i))
                    query.AddParameterWithTypeValue("SVCIN_ID_IN" & CStr(i), OracleDbType.Decimal, svcinId)
                    If Not inSvcinIdList.Count() = i Then
                        .Append(",")
                    End If
                    i = i + 1
                Next

                .AppendLine("   ")
                .AppendLine("                                                       ) ")
                .AppendLine("                               GROUP BY S2.JOB_DTL_ID ") 'チップ単位で最大のストールIDを抽出
                .AppendLine("                             ) ")
                .AppendLine(" GROUP BY T1.SVCIN_ID ") 'チップ単位で抽出したストールの終了時間最大を出したものを、入庫単位で集約
            End With

            query.CommandText = strSql.ToString()

            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDlrCd)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBrnCd)

            InspectData = query.GetData()


        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END ReturnCount:[{2}]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , InspectData.Count.ToString))

        Return InspectData
    End Function
#End Region

End Class

