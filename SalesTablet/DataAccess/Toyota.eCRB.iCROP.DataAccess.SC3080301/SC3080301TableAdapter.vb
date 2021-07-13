'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080301TableAdapter.vb
'─────────────────────────────────────
'機能： 査定依頼
'補足： 
'作成： 2012/01/05 TCS 鈴木(恭)
'更新： 2013/03/25 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/06/30 TCS 吉村 【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
'2013/06/30 TCS 吉村 2013/10対応版 既存流用 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
'2013/06/30 TCS 吉村 2013/10対応版 既存流用 END

Public Class SC3080301TableAdapter
    'Inherits Global.System.ComponentModel.Component

#Region "定数"
    ''' <summary>
    ''' 削除フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DeleteFlg As String = "0"

    ''' <summary>
    ''' 来店実績ステータス(07:商談中)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisiteStatus As String = "07"

    ''' <summary>
    ''' 依頼種別(01:査定)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoticeReqCtg As String = "01"

    ''' <summary>
    ''' 固定値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IntZero As Integer = 0

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AssessmentDisplayId As String = "SC3080301"

    ''' <summary>
    ''' 通知依頼情報の最終ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoticeStatusRequest As String = "1"
    Private Const NoticeStatusReceive As String = "3"
    Private Const NoticeStatusEnd As String = "4"

#End Region

    ''' <summary>
    ''' 端末ID取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <returns>SC3080301UcarTerminalDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetUcarTerminal(ByVal dlrcd As String, _
                                  ByVal strcd As String) As SC3080301DataSet.SC3080301UcarTerminalDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[dlrcd:{0}][strcd:{1}]", dlrcd, strcd))

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080301_001 */ ")
            .Append("    TERMINALID ")                '端末ID
            .Append("FROM ")
            .Append("    TBL_UCARTERMINAL ")
            .Append("WHERE ")
            .Append("    DLRCD = :DLRCD ")            '販売店コード
            .Append("AND STRCD = :STRCD ")            '店舗コード
            .Append("AND DELFLG = :DELFLG ")
        End With

        Using query As New DBSelectQuery(Of SC3080301DataSet.SC3080301UcarTerminalDataTable)("SC3080301_001")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                       '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)                       '店舗コード
            query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlg)                  '削除フラグ

            'SQL実行
            Dim rtnDt As SC3080301DataSet.SC3080301UcarTerminalDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return rtnDt

        End Using

    End Function

    ' 2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 来店客情報取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <returns>SC3080301VisitSalesDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetVisitSales(ByVal fllwupboxseqno As Decimal) As SC3080301DataSet.SC3080301VisitSalesDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetVisitSales_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080301_002 */ ")
            .Append("    SALESTABLENO ")                            '商談テーブルNo
            .Append("FROM ")
            .Append("    TBL_VISIT_SALES ")
            .Append("WHERE ")
            .Append("    FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")  'Follow-up Box内連番
            .Append("AND VISITSTATUS = :VISITSTATUS ")
        End With

        Using query As New DBSelectQuery(Of SC3080301DataSet.SC3080301VisitSalesDataTable)("SC3080301_002")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, VisiteStatus)

            'SQL実行
            Dim rtnDt As SC3080301DataSet.SC3080301VisitSalesDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetVisitSales_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 吉村 2013/10対応版　既存流用 END
            Return rtnDt

        End Using

    End Function

    ' 2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 査定情報取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <param name="crcustid">活動先顧客コード</param>
    ''' <returns>SC3080301UcarAssessmentDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetAssessmentInfo(ByVal fllwupboxseqno As Decimal, _
                                      ByVal crcustid As String) As SC3080301DataSet.SC3080301UcarAssessmentDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAssessmentInfo_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080301_003 */ ")
            .Append("    A.ASSESSMENTNO ")                        '査定No
            .Append("   ,A.CSTKIND ")                             '顧客種別
            .Append("   ,A.CUSTOMERCLASS ")                       '顧客分類 
            .Append("   ,A.CRCUSTID ")                            '活動先顧客コード 
            .Append("   ,A.RETENTION ")                           '保有フラグ 
            .Append("   ,A.ORGCSTVCL_VIN ")                       'VIN
            .Append("   ,A.NEWCSTVCL_SEQNO     ")                 '車両シーケンス
            .Append("   ,A.NOTICEREQID ")                         '通知依頼ID
            .Append("   ,A.APPRISAL_PRICE ")                      '査定価格
            .Append("   ,A.INSPECTIONDATE ")                      '査定日時
            .Append("   ,A.UPDATEDATE ")                          '送信時間
            .Append("   ,B.STATUS ")                              'ステータス
            .Append("FROM ")
            .Append("    TBL_UCARASSESSMENT A ")
            .Append("   ,TBL_NOTICEREQUEST B ")
            .Append("WHERE ")
            .Append("    A.NOTICEREQID = B.NOTICEREQID ")         '通知依頼ID
            .Append("AND A.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")  'Follow-up Box内連番
            .Append("AND A.CRCUSTID = :CRCUSTID ")                '活動先顧客コード
            .Append("AND B.NOTICEREQCTG = :NOTICEREQCTG ")        '通知依頼種別
            .Append("AND B.STATUS in (  ")
            .Append("                :REQUEST  ")                 'ステータス（依頼）
            .Append("               ,:RECEIVE  ")                 'ステータス（受付）
            .Append("               ,:END  ")                     'ステータス（査定完了）
            .Append("                )  ")
        End With

        Using query As New DBSelectQuery(Of SC3080301DataSet.SC3080301UcarAssessmentDataTable)("SC3080301_003")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustid)                 '活動先顧客コード
            query.AddParameterWithTypeValue("NOTICEREQCTG", OracleDbType.Char, NoticeReqCtg)         '通知依頼種別
            query.AddParameterWithTypeValue("REQUEST", OracleDbType.Char, NoticeStatusRequest)       '査定のステータス（依頼）
            query.AddParameterWithTypeValue("RECEIVE", OracleDbType.Char, NoticeStatusReceive)       '査定のステータス（受付）
            query.AddParameterWithTypeValue("END", OracleDbType.Char, NoticeStatusEnd)               '査定のステータス（査定完了）

            'SQL実行
            Dim rtnDt As SC3080301DataSet.SC3080301UcarAssessmentDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAssessmentInfo_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 吉村 2013/10対応版　既存流用 END

            Return rtnDt

        End Using

    End Function

    ' 2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 自社客車両情報取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="originalid">自社客連番</param>
    ''' <returns>SC3080301OrgVehicleDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetOrgVehicle(ByVal dlrcd As String, _
                                  ByVal originalid As String) As SC3080301DataSet.SC3080301VehicleDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgVehicle_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT ")
            .Append("  /* SC3080301_201 */ ")
            .Append("  TO_CHAR(CST_ID) AS ORIGINALID , ")
            .Append("  VCL_VIN AS VIN , ")
            .Append("  ROWNUM AS SEQNO , ")
            .Append("  REG_NUM AS VCLREGNO , ")
            .Append("  MODEL_NAME AS SERIESNAME , ")
            .Append("  MAKER_NAME AS MAKERNAME ")
            .Append("FROM ")
            .Append("   (SELECT ")
            .Append("       T1.CST_ID , ")
            .Append("       T2.VCL_VIN , ")
            .Append("       T3.REG_NUM , ")
            .Append("       T4.MODEL_NAME , ")
            .Append("       T5.MAKER_NAME , ")
            .Append("       CASE WHEN T3.DELI_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
            .Append("              NULL  ")
            .Append("       ELSE T3.DELI_DATE END AS DELI_DATE ")
            .Append("    FROM ")
            .Append("      TB_M_CUSTOMER_VCL T1 , ")
            .Append("      TB_M_VEHICLE T2 , ")
            .Append("      TB_M_VEHICLE_DLR T3 , ")
            .Append("      TB_M_MODEL T4 , ")
            .Append("      TB_M_MAKER T5 ")
            .Append("    WHERE ")
            .Append("          T1.VCL_ID = T2.VCL_ID ")
            .Append("      AND T1.DLR_CD = T3.DLR_CD ")
            .Append("      AND T1.VCL_ID = T3.VCL_ID ")
            .Append("      AND T2.MODEL_CD = T4.MODEL_CD(+) ")
            .Append("      AND T4.MAKER_CD = T5.MAKER_CD(+) ")
            .Append("      AND T1.DLR_CD = :DLRCD ")
            .Append("      AND T1.CST_ID = :ORIGINALID ")
            .Append("      AND T1.CST_VCL_TYPE = '1' ")
            .Append("      AND Trim(T2.VCL_VIN) IS NOT NULL ")
            .Append("    ORDER BY ")
            .Append("      DELI_DATE , ")
            .Append("      T3.REG_NUM ")
            .Append(" ) ")
        End With

        Using query As New DBSelectQuery(Of SC3080301DataSet.SC3080301VehicleDataTable)("SC3080301_201")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)                  '販売店コード
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)        '自社客連番

            'SQL実行
            Dim rtnDt As SC3080301DataSet.SC3080301VehicleDataTable = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOrgVehicle_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 吉村 2013/10対応版　既存流用 END

            Return rtnDt

        End Using

    End Function

    ' ''' <summary>
    ' ''' 副顧客車両情報取得
    ' ''' </summary>
    ' ''' <param name="dlrcd">販売店コード</param>
    ' ''' <param name="originalid">副顧客連番</param>
    ' ''' <returns>SC3080301OrgVehicleDataTable</returns>
    ' ''' <remarks></remarks>
    'Public Function GetSubVehicle(ByVal dlrcd As String, _
    '                              ByVal originalid As String) As SC3080301DataSet.SC3080301VehicleDataTable

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & _
    '                              "_Start[dlrcd:{0}][originalid:{1}]", dlrcd, originalid))

    '    Dim sql As New StringBuilder
    '    With sql
    '        .Append("SELECT /* SC3080301_010 */ ")
    '        .Append("    A.SUBCUSTID ")                '自社客連番
    '        .Append("   ,B.VIN ")                       'VIN
    '        .Append("   ,ROWNUM AS SEQNO ")             'SEQNO
    '        .Append("   ,B.VCLREGNO ")                  '車両登録No.
    '        .Append("   ,B.SERIESNM AS SERIESNAME ")    'モデル
    '        .Append("   ,C.MAKERNAME ")                 'メーカー
    '        .Append("FROM ")
    '        .Append("    TBLORG_SUBCUSTOMER A ")
    '        .Append("   ,TBLORG_VCLINFO B ")
    '        .Append("   ,TBLORG_MAKERMASTER C ")
    '        .Append("   ,TBLORG_SERIESMASTER D ")
    '        .Append("WHERE ")
    '        .Append("    A.DLRCD = B.DLRCD(+) ")
    '        .Append("AND A.ORIGINALID = B.ORIGINALID(+) ")
    '        .Append("AND B.DLRCD = D.DLRCD(+) ")
    '        .Append("AND B.SERIESCD = D.SERIESCD(+) ")
    '        .Append("AND D.DLRCD = C.DLRCD(+) ")
    '        .Append("AND D.MAKERCD = C.MAKERCD(+) ")
    '        .Append("AND A.DLRCD = :DLRCD ")            '販売店コード
    '        .Append("AND A.SUBCUSTID = :SUBCUSTID ")   '副顧客連番
    '        .Append("AND B.DELFLG = :DELFLG ")          '削除フラグ
    '        .Append("ORDER BY B.VCLDELIDATE ")
    '        .Append("       , B.VCLREGNO ")
    '    End With

    '    Using query As New DBSelectQuery(Of SC3080301DataSet.SC3080301VehicleDataTable)("SC3080301_010")

    '        query.CommandText = sql.ToString()
    '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)                 '販売店コード
    '        query.AddParameterWithTypeValue("SUBCUSTID", OracleDbType.Char, originalid)        '自社客連番
    '        query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlg)            '削除フラグ

    '        'SQL実行
    '        Dim rtnDt As SC3080301DataSet.SC3080301VehicleDataTable = query.GetData()

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

    '        Return rtnDt

    '    End Using

    'End Function

    ' 2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客車両情報取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstid">未取引客ユーザID</param>
    ''' <returns>SC3080301NewVehicleDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetNewVehicle(ByVal dlrcd As String, _
                                  ByVal cstid As String) As SC3080301DataSet.SC3080301VehicleDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewVehicle_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT ")
            .Append("  /* SC3080301_202 */ ")
            .Append("  TO_CHAR(CST_ID) AS ORIGINALID , ")
            .Append("  VCL_VIN AS VIN , ")
            .Append("  VCL_ID AS SEQNO , ")
            .Append("  REG_NUM AS VCLREGNO , ")
            .Append("  NEWCST_MODEL_NAME AS SERIESNAME , ")
            .Append("  NEWCST_MAKER_NAME AS MAKERNAME ")
            .Append("FROM ")
            .Append("   (SELECT ")
            .Append("       T1.CST_ID , ")
            .Append("       T2.VCL_VIN , ")
            .Append("       T2.VCL_ID , ")
            .Append("       T3.REG_NUM , ")
            .Append("       T2.NEWCST_MODEL_NAME , ")
            .Append("       T2.NEWCST_MAKER_NAME , ")
            .Append("       CASE WHEN T3.DELI_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
            .Append("              NULL  ")
            .Append("       ELSE T3.DELI_DATE END AS DELI_DATE ")
            .Append("    FROM ")
            .Append("      TB_M_CUSTOMER_VCL T1 , ")
            .Append("      TB_M_VEHICLE T2 , ")
            .Append("      TB_M_VEHICLE_DLR T3 ")
            .Append("    WHERE ")
            .Append("          T1.VCL_ID = T2.VCL_ID ")
            .Append("      AND T1.DLR_CD = T3.DLR_CD ")
            .Append("      AND T1.VCL_ID = T3.VCL_ID ")
            .Append("      AND T1.DLR_CD = :DLRCD ")
            .Append("      AND T1.CST_ID = :CSTID ")
            .Append("      AND T1.CST_VCL_TYPE = '1' ")
            .Append("    ORDER BY ")
            .Append("      DELI_DATE , ")
            .Append("      T3.REG_NUM ")
            .Append(" ) ")
        End With

        Using query As New DBSelectQuery(Of SC3080301DataSet.SC3080301VehicleDataTable)("SC3080301_202")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)

            'SQL実行
            Dim rtnDt As SC3080301DataSet.SC3080301VehicleDataTable = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewVehicle_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 吉村 2013/10対応版　既存流用 END

            Return rtnDt

        End Using

    End Function

    ' 2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 査定情報登録
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="fllwupboxseqno">Follow-up 活動連番</param>
    ''' <param name="crcustid">顧客コード</param>
    ''' <param name="customerclass">顧客分類</param>
    ''' <param name="cstkind">顧客種別</param>
    ''' <param name="retention">保有</param>
    ''' <param name="orgcstvclvin">VIN</param>
    ''' <param name="newcstvclseqno">車両シーケンス</param>
    ''' <param name="assessmentno">査定ID</param>
    ''' <param name="account">更新アカウント</param>
    ''' <param name="id">機能ID</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Function InsertUcarAssessment(ByVal dlrcd As String, _
                                            ByVal strcd As String, _
                                            ByVal fllwupboxseqno As Decimal, _
                                            ByVal crcustid As String, _
                                            ByVal customerclass As String, _
                                            ByVal cstkind As String, _
                                            ByVal retention As String, _
                                            ByVal orgcstvclvin As String, _
                                            ByVal newcstvclseqno As Long, _
                                            ByVal assessmentno As Long, _
                                            ByVal account As String, _
                                            ByVal id As String) As Integer

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertUcarAssessment_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder
        With sql
            .Append("INSERT ")
            .Append("INTO ")
            .Append("    TBL_UCARASSESSMENT /* SC3080301_006 */ ")
            .Append("( ")
            .Append("    ASSESSMENTNO ")            '査定No
            .Append("   ,CSTKIND ")                 '顧客種別
            .Append("   ,CUSTOMERCLASS ")           '顧客分類
            .Append("   ,CRCUSTID ")                '活動先顧客コード
            .Append("   ,RETENTION ")               '保有
            .Append("   ,ORGCSTVCL_VIN ")           '自社客車両VIN
            .Append("   ,NEWCSTVCL_SEQNO ")         '未取引客車両シーケンス番号
            .Append("   ,FLLWUPBOX_DLRCD ")         'Follow-up Box販売店コード
            .Append("   ,FLLWUPBOX_STRCD ")         'Follow-up Box店舗コード
            .Append("   ,FLLWUPBOX_SEQNO ")         'Follow-up Box内連番
            .Append("   ,MAKERNAME ")               'メーカー名
            .Append("   ,VEHICLENAME ")             '車名
            .Append("   ,REGISTRATIONNO ")          '登録番号
            .Append("   ,INSPECTIONDATE ")          '検査日
            .Append("   ,APPRISAL_PRICE ")          '提示価格
            .Append("   ,NOTICEREQID ")             '通知依頼ID
            .Append("   ,CREATEDATE ")              '作成日
            .Append("   ,UPDATEDATE ")              '更新日
            .Append("   ,CREATEACCOUNT ")           '作成アカウント
            .Append("   ,UPDATEACCOUNT ")           '更新アカウント
            .Append("   ,CREATEID ")                '作成機能ID
            .Append("   ,UPDATEID ")                '更新機能ID
            .Append(") ")
            .Append("VALUES ")
            .Append("( ")
            .Append("    :ASSESSMENTNO ")
            .Append("   ,:CSTKIND ")
            .Append("   ,:CUSTOMERCLASS ")
            .Append("   ,:CRCUSTID ")
            .Append("   ,:RETENTION ")
            .Append("   ,:ORGCSTVCL_VIN ")
            .Append("   ,:NEWCSTVCL_SEQNO ")
            .Append("   ,:FLLWUPBOX_DLRCD ")
            .Append("   ,:FLLWUPBOX_STRCD ")
            .Append("   ,:FLLWUPBOX_SEQNO ")
            .Append("   ,NULL ")
            .Append("   ,NULL ")
            .Append("   ,NULL ")
            .Append("   ,NULL ")
            .Append("   ,NULL ")
            .Append("   ,:NOTICEREQID ")
            .Append("   ,SYSDATE ")
            .Append("   ,SYSDATE ")
            .Append("   ,:CREATEACCOUNT ")
            .Append("   ,:UPDATEACCOUNT ")
            .Append("   ,:CREATEID ")
            .Append("   ,:UPDATEID ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080301_006")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Int64, assessmentno)            '査定ID
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, crcustid)                     '顧客コード
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, customerclass)           '顧客分類
            query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, cstkind)                       '顧客種別
            query.AddParameterWithTypeValue("RETENTION", OracleDbType.Char, retention)                   '保有
            query.AddParameterWithTypeValue("ORGCSTVCL_VIN", OracleDbType.Varchar2, orgcstvclvin)        'VIN
            If newcstvclseqno = 0 Then
                query.AddParameterWithTypeValue("NEWCSTVCL_SEQNO", OracleDbType.Int64, DBNull.Value)     '車両シーケンス(Null)
            Else
                query.AddParameterWithTypeValue("NEWCSTVCL_SEQNO", OracleDbType.Int64, newcstvclseqno)   '車両シーケンス
            End If
            query.AddParameterWithTypeValue("FLLWUPBOX_DLRCD", OracleDbType.Char, dlrcd)                 '販売店コード
            query.AddParameterWithTypeValue("FLLWUPBOX_STRCD", OracleDbType.Char, strcd)                 '店舗コード
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, fllwupboxseqno)       'Follow-up 活動連番
            query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, IntZero)                  '査定依頼ID(0:固定値)
            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, account)             '作成アカウント
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, account)             '更新アカウント
            query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, id)                       '作成機能ID
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, id)                       '更新機能ID

            'SQL実行
            Dim rtnInt As Integer = query.Execute()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnInt.ToString(CultureInfo.CurrentCulture)))
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertUcarAssessment_End")
            'ログ出力 End *****************************************************************************
            ' 2013/06/30 TCS 吉村 2013/10対応版　既存流用 END

            Return rtnInt

        End Using

    End Function

    ''' <summary>
    ''' 査定情報更新
    ''' </summary>
    ''' <param name="noticereqid">ステータス</param>
    ''' <param name="assessmentno">査定ID</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Function UpdateUcarAssessment(ByVal noticereqid As Long, _
                                ByVal assessmentno As Long) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                          "_Start[noticereqid:{0}][assessmentno:{1}]", noticereqid, assessmentno))

        Dim sql As New StringBuilder
        With sql
            .Append("UPDATE /* SC3080301_007 */ ")
            .Append("    TBL_UCARASSESSMENT ")
            .Append("SET ")
            .Append("    NOTICEREQID = :NOTICEREQID ")            '通知依頼ID
            .Append("WHERE ")
            .Append("    ASSESSMENTNO = :ASSESSMENTNO ")          '査定ID
        End With

        Using query As New DBUpdateQuery("SC3080301_007")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, noticereqid)                    'ステータス
            query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Int64, assessmentno)       '査定ID

            'SQL実行
            Dim rtnInt As Integer = query.Execute()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnInt.ToString(CultureInfo.CurrentCulture)))

            Return rtnInt

        End Using

    End Function

    ''' <summary>
    ''' 査定Noシーケンス采番
    ''' </summary>
    ''' <returns>シーケンスNo</returns>
    ''' <remarks></remarks>
    Public Function GetUcarAssessmentseq() As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & "_Start"))

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080301_008 */ ")
            .Append("    SEQ_UCARASSESSMENT_NO.NEXTVAL AS SEQ ")
            .Append("FROM ")
            .Append("    DUAL")
        End With

        Using query As New DBSelectQuery(Of SC3080301DataSet.SC3080301SeqDataTable)("SC3080301_008")
            query.CommandText = sql.ToString()

            Dim seqTbl As SC3080301DataSet.SC3080301SeqDataTable

            seqTbl = query.GetData()

            'SQL実行
            Dim rtnLng As Long = seqTbl.Item(0).Seq

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnLng.ToString(CultureInfo.CurrentCulture)))

            Return rtnLng

        End Using

    End Function

    ''' <summary>
    ''' 再査定用端末ID取得
    ''' </summary>
    ''' <returns>端末ID</returns>
    ''' <remarks></remarks>
    Public Function GetNoticeFromClient(ByVal noticereqid As Long) As SC3080301DataSet.SC3080301NoticeInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                  "_Start[noticereqid:{0}]", noticereqid))

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080301_009 */ ")
            .Append("    FROMCLIENTID ")
            .Append("FROM ")
            .Append("    TBL_NOTICEINFO ")
            .Append("WHERE ")
            .Append("    NOTICEREQID = :NOTICEREQID ")
            .Append("AND STATUS = :STATUS ")
            .Append("ORDER BY NOTICEID DESC ")
        End With

        Using query As New DBSelectQuery(Of SC3080301DataSet.SC3080301NoticeInfoDataTable)("SC3080301_009")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, noticereqid)           '査定依頼ID
            query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, NoticeStatusReceive)         'ステータス

            'SQL実行
            Dim rtnDt As SC3080301DataSet.SC3080301NoticeInfoDataTable = query.GetData()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", rtnDt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return rtnDt

        End Using

    End Function

    '2013/03/25 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD START
    ''' <summary>
    ''' 査定情報更新（同車種下取り査定情報の再利用時）
    ''' </summary>
    ''' <param name="assessmentno">査定ID</param>
    ''' <returns>更新件数</returns>
    ''' <remarks></remarks>
    Public Function UpdateUcarAssessmentInfo(ByVal assessmentno As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, AssessmentDisplayId & System.Reflection.MethodBase.GetCurrentMethod.Name & _
                                  "_Start[assessmentno:{0}]", assessmentno))

        Dim sql As New StringBuilder
        With sql
            .Append("UPDATE /* SC3080301_010 */ ")
            .Append("    TBL_UCARASSESSMENT ")
            .Append("SET ")
            .Append("    ASM_ACT_FLG = '1' ")               '査定実績フラグ
            .Append("WHERE ")
            .Append("    ASSESSMENTNO = :ASSESSMENTNO ")    '査定ID
        End With

        Using query As New DBUpdateQuery("SC3080301_010")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Int64, assessmentno)       '査定ID

            'SQL実行
            Dim rtnInt As Long = query.Execute()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[GetCount:{0}]", _
                                      rtnInt.ToString(CultureInfo.CurrentCulture)))

            Return rtnInt

        End Using

    End Function
    '2013/03/25 TCS 橋本 【A.STEP2】新車タブレット受付画面の管理指標変更対応 ADD END

    '2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 下取り査定情報のロック
    ''' </summary>
    ''' <param name="assessmentno">査定ID</param>
    ''' <remarks></remarks>
    Public Shared Sub GetAssesmetLock(ByVal assessmentno As Long)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAssesmetLock_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of DataTable)("SC3080301_203")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

            Dim sql As New StringBuilder

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080301_203 */ ")
                .Append("1 ")
                .Append("FROM ")
                .Append("  TBL_UCARASSESSMENT ")
                .Append("WHERE ")
                .Append("  ASSESSMENTNO = :ASSESSMENTNO ")
                .Append(sqlForUpdate)
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ASSESSMENTNO", OracleDbType.Int64, assessmentno)       '査定ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAssesmetLock_End")
            'ログ出力 End *****************************************************************************
            query.GetData()

        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, System.Reflection.MethodBase.GetCurrentMethod.Name & "_End[Success:{0}]", 1))

    End Sub
    '2013/06/30 TCS 吉村 2013/10対応版　既存流用 END

End Class


