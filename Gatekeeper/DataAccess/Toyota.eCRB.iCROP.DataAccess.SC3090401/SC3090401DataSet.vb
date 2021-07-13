'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3090401DataSet.vb
'──────────────────────────────────
'機能： 予約一覧
'補足： 
'作成： 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
'更新： 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 $01
'──────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Text
Imports Oracle.DataAccess.Client

Namespace SC3090401DataSetTableAdapters

    Public Class SC3090401TableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"
        ''' <summary>
        ''' 機能ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ApplicationId As String = "SC3090401"

        ''' <summary>
        ''' 来店済み取得フラグ(0:取得しない)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitFlagOff As String = "0"

        ''' <summary>
        ''' 来店済み取得フラグ(1:取得する)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitFlagOn As String = "1"

        ''' <summary>
        ''' サービスステータス(00:未入庫)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SvcStatusNotCarin As String = "00"

        ''' <summary>
        ''' サービスステータス(01:未来店)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SvcStatusNoShow As String = "01"

        ''' <summary>
        ''' キャンセルフラグ(0:有効)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CancelFlagOff As String = "0"

        ''' <summary>
        ''' 振当てステータス（0:未振当て）
        ''' </summary>
        Private Const NonAssign As String = "0"

        ''' <summary>
        ''' 振当てステータス（1:受付待ち）
        ''' </summary>
        Private Const AssignWait As String = "1"

        ''' <summary>
        ''' 振当てステータス（2:振当済み）
        ''' </summary>
        Private Const AssignFinish As String = "2"

        ''' <summary>
        ''' 振当てステータス（3:BP/保険）
        ''' </summary>
        Private Const AssignBpInsurance As String = "3"

        ''' <summary>
        ''' 振当てステータス（4:退店）
        ''' </summary>
        Private Const DealerOut As String = "4"

        ''' <summary>
        ''' 振当てステータス（9:HOLD中）
        ''' </summary>
        Private Const Holding As String = "9"

        ''' <summary>
        ''' ソート条件区分（0:予約日時）
        ''' </summary>
        Private Const SortTypeRezDate As String = "0"

        ''' <summary>
        ''' ソート条件区分（1:車両登録番号）
        ''' </summary>
        Private Const SortTypeRegNum As String = "1"

        ''' <summary>
        ''' 対応フラグ（0:未対応）
        ''' </summary>
        Private Const DealFlgOff As String = "0"

        ''' <summary>
        ''' 対応フラグ（1:来店通知済み）
        ''' </summary>
        Private Const DealFlgOn As String = "1"

        ''' <summary>
        ''' 削除フラグ（0:削除以外）
        ''' </summary>
        Private Const DelFlgOff As String = "0"

        ''' <summary>
        ''' DB日付省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MinDate As String = "1900/01/01 00:00:00"
#End Region

#Region "予約件数取得"
        ''' <summary>
        ''' 予約件数取得
        ''' </summary>
        ''' <param name="inDealerCode">ログインユーザーの販売店コード</param>
        ''' <param name="inBranchCode">ログインユーザーの店舗コード</param>
        ''' <param name="inVisitFlag">来店済み取得フラグ</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns>予約件数</returns>
        ''' <remarks></remarks>
        Public Function GetReservationCount(ByVal inDealerCode As String, _
                                            ByVal inBranchCode As String, _
                                            ByVal inVisitFlag As String, _
                                            ByVal inNowDate As Date) As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inVisitFlag = {4}" & _
            ", inNowDate = {5}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inDealerCode, inBranchCode, inVisitFlag, inNowDate))

            'データ格納用
            Dim dt As SC3090401DataSet.SC3090401CountDataDataTable

            Dim sql As New StringBuilder
            ' SQL文組立て
            With sql
                .AppendLine("SELECT /* SC3090401_001 */ ")
                .AppendLine("       COUNT(1) AS COUNT")
                .AppendLine("  FROM ")
                .AppendLine("       TB_T_SERVICEIN SVC ")
                .AppendLine("     , TB_T_JOB_DTL JD ")
                .AppendLine("     , TB_M_CUSTOMER CST ")
                .AppendLine("     , TB_M_VEHICLE VCL ")
                .AppendLine("     , TB_M_VEHICLE_DLR VCLDLR ")
                .AppendLine("     , ( ")
                .AppendLine("         SELECT ")
                .AppendLine("                SVC1.SVCIN_ID ")
                .AppendLine("              , MIN(SU1.SCHE_START_DATETIME) AS START_DATETIME ")
                .AppendLine("           FROM ")
                .AppendLine("                TB_T_SERVICEIN SVC1 ")
                .AppendLine("              , TB_T_JOB_DTL JD1 ")
                .AppendLine("              , TB_T_STALL_USE SU1 ")
                .AppendLine("          WHERE SVC1.SVCIN_ID = JD1.SVCIN_ID ")
                .AppendLine("            AND JD1.JOB_DTL_ID = SU1.JOB_DTL_ID ")
                .AppendLine("            AND SVC1.DLR_CD = :DLR_CD ")
                .AppendLine("            AND SVC1.BRN_CD = :BRN_CD ")
                .AppendLine("            AND JD1.DLR_CD = :DLR_CD ")
                .AppendLine("            AND JD1.BRN_CD = :BRN_CD ")
                .AppendLine("            AND SU1.DLR_CD = :DLR_CD ")
                .AppendLine("            AND SU1.BRN_CD = :BRN_CD ")
                .AppendLine("            AND SVC1.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01) ")
                .AppendLine("            AND JD1.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("            AND SU1.SCHE_START_DATETIME ")
                .AppendLine("                BETWEEN TRUNC(:NOW_DATETIME) ")
                .AppendLine("                AND TRUNC(:NOW_DATETIME) + 86399/86400 ")
                .AppendLine("          GROUP BY SVC1.SVCIN_ID ")
                .AppendLine("       ) REZ ")
                .AppendLine("     , ( ")
                .AppendLine("         SELECT ")
                .AppendLine("                SVM1.FREZID ")
                .AppendLine("              , MAX(SVM1.UPDATEDATE) AS UPDATEDATE ")
                .AppendLine("           FROM ")
                .AppendLine("                TBL_SERVICE_VISIT_MANAGEMENT SVM1 ")
                .AppendLine("          WHERE SVM1.DLRCD = :DLR_CD ")
                .AppendLine("            AND SVM1.STRCD = :BRN_CD ")
                .AppendLine("            AND SVM1.ASSIGNSTATUS IN (:ASSIGNSTATUS_0, :ASSIGNSTATUS_1) ")
                .AppendLine("          GROUP BY SVM1.FREZID ")
                .AppendLine("       ) SVM ")
                .AppendLine(" WHERE SVC.SVCIN_ID = REZ.SVCIN_ID ")
                .AppendLine("   AND SVC.SVCIN_ID = JD.SVCIN_ID ")
                .AppendLine("   AND SVC.CST_ID = CST.CST_ID ")
                .AppendLine("   AND SVC.VCL_ID = VCL.VCL_ID ")
                .AppendLine("   AND SVC.VCL_ID = VCLDLR.VCL_ID ")
                .AppendLine("   AND SVC.SVCIN_ID = SVM.FREZID (+) ")
                .AppendLine("   AND SVC.DLR_CD = :DLR_CD ")
                .AppendLine("   AND SVC.BRN_CD = :BRN_CD ")
                .AppendLine("   AND VCLDLR.DLR_CD = :DLR_CD ")
                .AppendLine("   AND SVC.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01) ")
                .AppendLine("   AND DECODE(SVC.SCHE_SVCIN_DATETIME, :MINDATE, REZ.START_DATETIME, SVC.SCHE_SVCIN_DATETIME) ")
                .AppendLine("       BETWEEN TRUNC(:NOW_DATETIME) ")
                .AppendLine("       AND TRUNC(:NOW_DATETIME) + 86399/86400 ")
                ' 来店済み取得フラグが「0:取得しない」の場合
                If inVisitFlag = VisitFlagOff Then

                    .AppendLine("   AND SVM.UPDATEDATE IS NULL ")
                End If
                .AppendLine("   AND JD.JOB_DTL_ID = ( ")
                .AppendLine("       SELECT ")
                .AppendLine("              MIN(JD2.JOB_DTL_ID)  ")
                .AppendLine("         FROM ")
                .AppendLine("              TB_T_JOB_DTL JD2 ")
                .AppendLine("        WHERE JD2.DLR_CD = :DLR_CD ")
                .AppendLine("          AND JD2.BRN_CD = :BRN_CD ")
                .AppendLine("          AND JD2.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("          AND JD2.SVCIN_ID = SVC.SVCIN_ID ")
                .AppendLine(") ")
                .AppendLine("   AND NOT EXISTS ( ")
                .AppendLine("       SELECT ")
                .AppendLine("              1 ")
                .AppendLine("         FROM ")
                .AppendLine("              TB_T_RO_INFO RO ")
                .AppendLine("            , TBL_SERVICE_VISIT_MANAGEMENT SVM2 ")
                .AppendLine("        WHERE RO.VISIT_ID = SVM2.VISITSEQ ")
                .AppendLine("          AND RO.DLR_CD = :DLR_CD ")
                .AppendLine("          AND RO.BRN_CD = :BRN_CD ")
                .AppendLine("          AND SVM2.DLRCD = :DLR_CD ")
                .AppendLine("          AND SVM2.STRCD = :BRN_CD ")
                .AppendLine("          AND SVM2.ASSIGNSTATUS <> :ASSIGNSTATUS_4 ")
                .AppendLine("          AND RO.SVCIN_ID = SVC.SVCIN_ID ")
                .AppendLine(") ")
                .AppendLine("   AND NOT EXISTS( ")
                .AppendLine("       SELECT  ")
                .AppendLine("              1 ")
                .AppendLine("         FROM ")
                .AppendLine("              TBL_SERVICE_VISIT_MANAGEMENT SVM3 ")
                .AppendLine("        WHERE ")
                .AppendLine("              SVM3.DLRCD = :DLR_CD ")
                .AppendLine("          AND SVM3.STRCD = :BRN_CD ")
                .AppendLine("          AND SVM3.FREZID = SVC.SVCIN_ID ")
                .AppendLine("          AND SVM3.ASSIGNSTATUS IN (:ASSIGNSTATUS_2, :ASSIGNSTATUS_3, :ASSIGNSTATUS_9) ")
                .AppendLine(") ")
            End With

            Using query As New DBSelectQuery(Of SC3090401DataSet.SC3090401CountDataDataTable)("SC3240401_001")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, SvcStatusNotCarin)
                query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, SvcStatusNoShow)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelFlagOff)
                query.AddParameterWithTypeValue("NOW_DATETIME", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_0", OracleDbType.NVarchar2, NonAssign)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_1", OracleDbType.NVarchar2, AssignWait)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_2", OracleDbType.NVarchar2, AssignFinish)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_3", OracleDbType.NVarchar2, AssignBpInsurance)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_4", OracleDbType.NVarchar2, DealerOut)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_9", OracleDbType.NVarchar2, Holding)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.CurrentCulture))

                ' SQL実行
                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows(0)(0)))

            Return CInt(dt.Rows(0)(0))

        End Function
#End Region

#Region "予約情報取得"
        ''' <summary>
        ''' 予約情報取得
        ''' </summary>
        ''' <param name="inDealerCode">ログインユーザーの販売店コード</param>
        ''' <param name="inBranchCode">ログインユーザーの店舗コード</param>
        ''' <param name="inVisitFlag">来店済み取得フラグ</param>
        ''' <param name="inSortType">ソート条件区分</param>
        ''' <param name="inBeginIndex">取得する予約情報の開始行番号</param>
        ''' <param name="inEndIndex">取得する予約情報の終了行番号</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns>予約情報</returns>
        ''' <remarks></remarks>
        Public Function GetReservationInfo(ByVal inDealerCode As String, _
                                           ByVal inBranchCode As String, _
                                           ByVal inVisitFlag As String, _
                                           ByVal inSortType As String, _
                                           ByVal inBeginIndex As Integer, _
                                           ByVal inEndIndex As Integer, _
                                           ByVal inNowDate As Date) As SC3090401DataSet.SC3090401ReserveDataDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inVisitFlag = {4}" & _
            ", inSortType = {5}, inBeginIndex = {6}, inEndIndex = {7}, inNowDate = {8}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inDealerCode, inBranchCode, inVisitFlag, inSortType _
            , inBeginIndex, inEndIndex, inNowDate))

            'データ格納用
            Dim dt As SC3090401DataSet.SC3090401ReserveDataDataTable

            Dim sql As New StringBuilder
            ' SQL文組立て
            With sql
                .AppendLine("SELECT /* SC3090401_002 */ ")
                .AppendLine("       TGT.SVCIN_ID ")
                .AppendLine("     , TGT.REZ_DATETIME ")
                .AppendLine("     , TGT.MODEL_NAME ")
                '$01 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
                '.AppendLine("     , TGT.MERC_NAME ")
                .AppendLine("     , TGT.SVC_CLASS_NAME")
                '$01 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END
                .AppendLine("     , TGT.REG_NUM ")
                .AppendLine("     , TGT.CST_NAME ")
                .AppendLine("     , TGT.NAMETITLE_NAME ")
                .AppendLine("     , TGT.POSITION_TYPE ")
                .AppendLine("     , TGT.UPDATEDATE ")
                .AppendLine("  FROM ( ")
                .AppendLine("         SELECT ")
                .AppendLine("                SVC.SVCIN_ID ")
                .AppendLine("              , DECODE(SVC.SCHE_SVCIN_DATETIME, :MINDATE ")
                .AppendLine("                     , REZ.START_DATETIME, SVC.SCHE_SVCIN_DATETIME) AS REZ_DATETIME ")
                .AppendLine("              , NVL(MDL.MODEL_NAME, VCL.NEWCST_MODEL_NAME) AS MODEL_NAME ")
                '$01 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
                '.AppendLine("              , NVL(TRIM(CONCAT(MERC.UPPER_DISP, MERC.LOWER_DISP)) ")
                '.AppendLine("                  , NVL(TRIM(SC.SVC_CLASS_NAME), SC.SVC_CLASS_NAME_ENG)) AS MERC_NAME  ")
                .AppendLine("              , NVL(TRIM(SC.SVC_CLASS_NAME), SC.SVC_CLASS_NAME_ENG) AS SVC_CLASS_NAME  ")
                '$01 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END
                .AppendLine("              , VCLDLR.REG_NUM ")
                .AppendLine("              , CST.CST_NAME ")
                .AppendLine("              , NT.NAMETITLE_NAME ")
                .AppendLine("              , NT.POSITION_TYPE ")
                .AppendLine("              , SVM.UPDATEDATE ")
                ' 引数．ソート条件区分が0:予約日時の場合
                If inSortType = SortTypeRezDate Then
                    .AppendLine("              , ROW_NUMBER() OVER (ORDER BY DECODE(SVC.SCHE_SVCIN_DATETIME, :MINDATE ")
                    .AppendLine("                                                 , REZ.START_DATETIME, SVC.SCHE_SVCIN_DATETIME) ")
                    .AppendLine("                                          , VCLDLR.REG_NUM ASC) AS ROW_COUNT ")
                    ' 引数．ソート条件区分が1:車両登録番号の場合
                ElseIf inSortType = SortTypeRegNum Then
                    .AppendLine("              , ROW_NUMBER() OVER (ORDER BY VCLDLR.REG_NUM ")
                    .AppendLine("                                          , DECODE(SVC.SCHE_SVCIN_DATETIME, :MINDATE ")
                    .AppendLine("                                                 , REZ.START_DATETIME, SVC.SCHE_SVCIN_DATETIME) ASC) AS ROW_COUNT ")
                End If
                .AppendLine("           FROM ")
                .AppendLine("                TB_T_SERVICEIN SVC ")
                .AppendLine("              , TB_T_JOB_DTL JD ")
                .AppendLine("              , TB_M_CUSTOMER CST ")
                .AppendLine("              , TB_M_VEHICLE VCL ")
                .AppendLine("              , TB_M_VEHICLE_DLR VCLDLR ")
                .AppendLine("              , TB_M_SERVICE_CLASS SC ")
                '$01 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
                '.AppendLine("              , TB_M_MERCHANDISE MERC ")
                '$01 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END
                .AppendLine("              , TB_M_NAMETITLE NT ")
                .AppendLine("              , TB_M_MODEL MDL ")
                .AppendLine("              , ( ")
                .AppendLine("                  SELECT ")
                .AppendLine("                         SVC1.SVCIN_ID ")
                .AppendLine("                       , MIN(SU1.SCHE_START_DATETIME) AS START_DATETIME ")
                .AppendLine("                    FROM ")
                .AppendLine("                         TB_T_SERVICEIN SVC1 ")
                .AppendLine("                       , TB_T_JOB_DTL JD1 ")
                .AppendLine("                       , TB_T_STALL_USE SU1 ")
                .AppendLine("                   WHERE SVC1.SVCIN_ID = JD1.SVCIN_ID ")
                .AppendLine("                     AND JD1.JOB_DTL_ID = SU1.JOB_DTL_ID ")
                .AppendLine("                     AND SVC1.DLR_CD = :DLR_CD ")
                .AppendLine("                     AND SVC1.BRN_CD = :BRN_CD ")
                .AppendLine("                     AND JD1.DLR_CD = :DLR_CD ")
                .AppendLine("                     AND JD1.BRN_CD = :BRN_CD ")
                .AppendLine("                     AND SU1.DLR_CD = :DLR_CD ")
                .AppendLine("                     AND SU1.BRN_CD = :BRN_CD ")
                .AppendLine("                     AND SVC1.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01) ")
                .AppendLine("                     AND JD1.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("                     AND SU1.SCHE_START_DATETIME ")
                .AppendLine("                         BETWEEN TRUNC(:NOW_DATETIME) ")
                .AppendLine("                             AND TRUNC(:NOW_DATETIME) + 86399/86400 ")
                .AppendLine("                   GROUP BY SVC1.SVCIN_ID ")
                .AppendLine("              ) REZ ")
                .AppendLine("              , ( ")
                .AppendLine("                  SELECT ")
                .AppendLine("                         SVM1.FREZID ")
                .AppendLine("                       , MAX(SVM1.UPDATEDATE) AS UPDATEDATE ")
                .AppendLine("                    FROM ")
                .AppendLine("                         TBL_SERVICE_VISIT_MANAGEMENT SVM1 ")
                .AppendLine("                   WHERE SVM1.DLRCD = :DLR_CD ")
                .AppendLine("                     AND SVM1.STRCD = :BRN_CD ")
                .AppendLine("                     AND SVM1.ASSIGNSTATUS IN (:ASSIGNSTATUS_0, :ASSIGNSTATUS_1) ")
                .AppendLine("                   GROUP BY SVM1.FREZID ")
                .AppendLine("              ) SVM ")
                .AppendLine("          WHERE SVC.SVCIN_ID = REZ.SVCIN_ID ")
                .AppendLine("            AND SVC.SVCIN_ID = JD.SVCIN_ID ")
                .AppendLine("            AND SVC.CST_ID = CST.CST_ID ")
                .AppendLine("            AND SVC.VCL_ID = VCL.VCL_ID ")
                .AppendLine("            AND SVC.VCL_ID = VCLDLR.VCL_ID ")
                .AppendLine("            AND SVC.SVCIN_ID = SVM.FREZID (+) ")
                .AppendLine("            AND JD.SVC_CLASS_ID = SC.SVC_CLASS_ID (+) ")
                '$01 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
                '.AppendLine("            AND JD.MERC_ID = MERC.MERC_ID (+) ")
                '$01 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END
                .AppendLine("            AND CST.NAMETITLE_CD = NT.NAMETITLE_CD (+) ")
                .AppendLine("            AND VCL.MODEL_CD = MDL.MODEL_CD (+) ")
                .AppendLine("            AND SVC.DLR_CD = :DLR_CD ")
                .AppendLine("            AND SVC.BRN_CD = :BRN_CD ")
                .AppendLine("            AND VCLDLR.DLR_CD = :DLR_CD ")
                .AppendLine("            AND SVC.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01) ")
                .AppendLine("            AND DECODE(SVC.SCHE_SVCIN_DATETIME, :MINDATE, REZ.START_DATETIME, SVC.SCHE_SVCIN_DATETIME) ")
                .AppendLine("                BETWEEN TRUNC(:NOW_DATETIME) ")
                .AppendLine("                    AND TRUNC(:NOW_DATETIME) + 86399/86400 ")
                ' 来店済み取得フラグが「0:取得しない」の場合
                If inVisitFlag = VisitFlagOff Then

                    .AppendLine("   AND SVM.UPDATEDATE IS NULL ")
                End If
                .AppendLine("            AND JD.JOB_DTL_ID = ( ")
                .AppendLine("                SELECT ")
                .AppendLine("                       MIN(JD2.JOB_DTL_ID)  ")
                .AppendLine("                  FROM ")
                .AppendLine("                       TB_T_JOB_DTL JD2 ")
                .AppendLine("                 WHERE JD2.DLR_CD = :DLR_CD ")
                .AppendLine("                   AND JD2.BRN_CD = :BRN_CD ")
                .AppendLine("                   AND JD2.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("                   AND JD2.SVCIN_ID = SVC.SVCIN_ID ")
                .AppendLine("         ) ")
                .AppendLine("            AND NOT EXISTS ( ")
                .AppendLine("                SELECT ")
                .AppendLine("                       1 ")
                .AppendLine("                  FROM ")
                .AppendLine("                       TB_T_RO_INFO RO ")
                .AppendLine("                     , TBL_SERVICE_VISIT_MANAGEMENT SVM2  ")
                .AppendLine("                 WHERE RO.VISIT_ID = SVM2.VISITSEQ ")
                .AppendLine("                   AND RO.DLR_CD = :DLR_CD ")
                .AppendLine("                   AND RO.BRN_CD = :BRN_CD ")
                .AppendLine("                   AND SVM2.DLRCD = :DLR_CD ")
                .AppendLine("                   AND SVM2.STRCD = :BRN_CD ")
                .AppendLine("                   AND SVM2.ASSIGNSTATUS <> :ASSIGNSTATUS_4 ")
                .AppendLine("                   AND RO.SVCIN_ID = SVC.SVCIN_ID ")
                .AppendLine("          ) ")
                .AppendLine("            AND NOT EXISTS( ")
                .AppendLine("                SELECT  ")
                .AppendLine("                       1 ")
                .AppendLine("                  FROM ")
                .AppendLine("                       TBL_SERVICE_VISIT_MANAGEMENT SVM3 ")
                .AppendLine("                 WHERE ")
                .AppendLine("                       SVM3.DLRCD = :DLR_CD ")
                .AppendLine("                   AND SVM3.STRCD = :BRN_CD ")
                .AppendLine("                   AND SVM3.FREZID = SVC.SVCIN_ID ")
                .AppendLine("                   AND SVM3.ASSIGNSTATUS IN (:ASSIGNSTATUS_2, :ASSIGNSTATUS_3, :ASSIGNSTATUS_9) ")
                .AppendLine("          ) ")
                .AppendLine(") TGT ")
                .AppendLine("WHERE TGT.ROW_COUNT BETWEEN :BEGIN_INDEX AND :END_INDEX ")
                .AppendLine("ORDER BY TGT.ROW_COUNT ")
            End With

            Using query As New DBSelectQuery(Of SC3090401DataSet.SC3090401ReserveDataDataTable)("SC3240401_002")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, SvcStatusNotCarin)
                query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, SvcStatusNoShow)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelFlagOff)
                query.AddParameterWithTypeValue("NOW_DATETIME", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_0", OracleDbType.NVarchar2, NonAssign)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_1", OracleDbType.NVarchar2, AssignWait)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_2", OracleDbType.NVarchar2, AssignFinish)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_3", OracleDbType.NVarchar2, AssignBpInsurance)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_4", OracleDbType.NVarchar2, DealerOut)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_9", OracleDbType.NVarchar2, Holding)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("BEGIN_INDEX", OracleDbType.Long, inBeginIndex)
                query.AddParameterWithTypeValue("END_INDEX", OracleDbType.Long, inEndIndex)

                ' SQL実行
                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return dt

        End Function
#End Region

#Region "未入庫予約存在チェック"
        ''' <summary>
        ''' 未入庫予約存在チェック
        ''' </summary>
        ''' <param name="inServiceinId">選択中の予約情報のサービス入庫ID</param>
        ''' <returns>データ件数</returns>
        ''' <remarks></remarks>
        Public Function IsNotCarInStatus(ByVal inServiceinId As Decimal) As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START IN:inServiceinId = {2}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inServiceinId))

            'データ格納用
            Dim dt As SC3090401DataSet.SC3090401CountDataDataTable

            Dim sql As New StringBuilder
            ' SQL文組立て
            With sql
                .AppendLine("SELECT /* SC3090401_003 */ ")
                .AppendLine("       COUNT(1) AS COUNT")
                .AppendLine("  FROM ")
                .AppendLine("       TB_T_SERVICEIN T1 ")
                .AppendLine(" WHERE T1.SVCIN_ID = :SVCIN_ID ")
                .AppendLine("   AND T1.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01) ")
            End With

            Using query As New DBSelectQuery(Of SC3090401DataSet.SC3090401CountDataDataTable)("SC3240401_003")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceinId)
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, SvcStatusNotCarin)
                query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, SvcStatusNoShow)

                ' SQL実行
                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END OUT:COUNT = {2}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , dt.Rows(0)(0)))

            Return CInt(dt.Rows(0)(0))

        End Function
#End Region

#Region "サービス来店者管理存在チェック"
        ''' <summary>
        ''' サービス来店者管理存在チェック
        ''' </summary>
        ''' <param name="inDealerCode">ログインユーザーの販売店コード</param>
        ''' <param name="inBranchCode">ログインユーザーの店舗コード</param>
        ''' <param name="inServiceinId">選択中の予約情報のサービス入庫ID</param>
        ''' <returns>データ件数</returns>
        ''' <remarks></remarks>
        Public Function GetVisitManagementCount(ByVal inDealerCode As String, _
                                                ByVal inBranchCode As String, _
                                                ByVal inServiceinId As Decimal) As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inServiceinId = {4}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inDealerCode, inBranchCode, inServiceinId))

            'データ格納用
            Dim dt As SC3090401DataSet.SC3090401CountDataDataTable

            Dim sql As New StringBuilder
            ' SQL文組立て
            With sql
                .AppendLine("SELECT /* SC3090401_004 */ ")
                .AppendLine("       COUNT(1) AS COUNT")
                .AppendLine("  FROM ")
                .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                .AppendLine(" WHERE T1.DLRCD = :DLR_CD ")
                .AppendLine("   AND T1.STRCD = :BRN_CD ")
                .AppendLine("   AND T1.FREZID = :SVCIN_ID ")
                .AppendLine("   AND T1.ASSIGNSTATUS <> :ASSIGNSTATUS_4 ")
            End With

            Using query As New DBSelectQuery(Of SC3090401DataSet.SC3090401CountDataDataTable)("SC3240401_004")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceinId)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_4", OracleDbType.NVarchar2, DealerOut)

                ' SQL実行
                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END OUT:COUNT = {2}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , dt.Rows(0)(0)))

            Return CInt(dt.Rows(0)(0))

        End Function
#End Region

#Region "顧客車両情報取得"
        ''' <summary>
        ''' 顧客車両情報取得
        ''' </summary>
        ''' <param name="inServiceinId">選択中の予約情報のサービス入庫ID</param>
        ''' <returns>選択中の予約の顧客車両情報</returns>
        ''' <remarks></remarks>
        Public Function GetCustomerVehicleInfo(ByVal inServiceinId As Decimal) _
            As SC3090401DataSet.SC3090401CustomerVehicleDataDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START IN:inServiceinId = {2}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inServiceinId))

            'データ格納用
            Dim dt As SC3090401DataSet.SC3090401CustomerVehicleDataDataTable

            Dim sql As New StringBuilder
            ' SQL文組立て
            With sql
                .AppendLine("SELECT /* SC3090401_005 */ ")
                .AppendLine("       TRIM(T2.REG_NUM) AS REG_NUM ")
                .AppendLine("     , T2.REG_NUM_SEARCH ")
                .AppendLine("     , T3.VCL_ID ")
                .AppendLine("     , TRIM(T3.VCL_VIN) AS VCL_VIN")
                .AppendLine("     , TRIM(T4.CST_TYPE) AS CST_TYPE ")
                .AppendLine("     , T5.CST_ID ")
                .AppendLine("     , TRIM(T5.DMS_CST_CD) AS DMS_CST_CD")
                .AppendLine("     , TRIM(T5.NAMETITLE_NAME) AS NAMETITLE_NAME ")
                .AppendLine("     , TRIM(T5.CST_NAME) AS CST_NAME ")
                .AppendLine("     , TRIM(T5.CST_GENDER) AS CST_GENDER ")
                .AppendLine("     , TRIM(T6.SLS_PIC_STF_CD) AS SLS_PIC_STF_CD ")
                .AppendLine("     , TRIM(T6.SVC_PIC_STF_CD) AS SVC_PIC_STF_CD ")
                .AppendLine("  FROM ")
                .AppendLine("       TB_T_SERVICEIN T1 ")
                .AppendLine("     , TB_M_VEHICLE_DLR T2 ")
                .AppendLine("     , TB_M_VEHICLE T3 ")
                .AppendLine("     , TB_M_CUSTOMER_DLR T4 ")
                .AppendLine("     , TB_M_CUSTOMER T5 ")
                .AppendLine("     , TB_M_CUSTOMER_VCL T6 ")
                .AppendLine(" WHERE T1.DLR_CD = T2.DLR_CD ")
                .AppendLine("   AND T1.VCL_ID = T2.VCL_ID ")
                .AppendLine("   AND T1.VCL_ID = T3.VCL_ID ")
                .AppendLine("   AND T1.DLR_CD = T4.DLR_CD ")
                .AppendLine("   AND T1.CST_ID = T4.CST_ID ")
                .AppendLine("   AND T1.CST_ID = T5.CST_ID ")
                .AppendLine("   AND T1.DLR_CD = T6.DLR_CD ")
                .AppendLine("   AND T1.VCL_ID = T6.VCL_ID ")
                .AppendLine("   AND T1.CST_ID = T6.CST_ID ")
                .AppendLine("   AND T1.CST_VCL_TYPE = T6.CST_VCL_TYPE ")
                .AppendLine("   AND T1.SVCIN_ID = :SVCIN_ID ")
            End With

            Using query As New DBSelectQuery(Of SC3090401DataSet.SC3090401CustomerVehicleDataDataTable)("SC3240401_005")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceinId)

                ' SQL実行
                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END OUT:COUNT = {2}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return dt

        End Function
#End Region

#Region "サービス来店者管理ロック取得"
        ''' <summary>
        ''' サービス来店者管理ロック取得
        ''' </summary>
        ''' <param name="inDealerCode">ログインユーザーの販売店コード</param>
        ''' <param name="inBranchCode">ログインユーザーの店舗コード</param>
        ''' <param name="inServiceinId">選択中の予約情報のサービス入庫ID</param>
        ''' <returns>ロック対象情報</returns>
        ''' <remarks></remarks>
        Public Function GetLockVisitManagement(ByVal inDealerCode As String, _
                                               ByVal inBranchCode As String, _
                                               ByVal inServiceinId As Decimal) _
                                           As SC3090401DataSet.SC3090401LockTargetDataDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inServiceinId = {4}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inDealerCode, inBranchCode, inServiceinId))

            'データ格納用
            Dim dt As SC3090401DataSet.SC3090401LockTargetDataDataTable

            Dim sql As New StringBuilder
            ' SQL文組立て
            With sql
                .AppendLine("SELECT /* SC3090401_006 */ ")
                .AppendLine("       T1.VISITSEQ ")
                .AppendLine("     , T1.UPDATEDATE ")
                .AppendLine("  FROM ")
                .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                .AppendLine(" WHERE T1.DLRCD = :DLRCD ")
                .AppendLine("   AND T1.STRCD = :STRCD ")
                .AppendLine("   AND T1.FREZID = :SVCIN_ID ")
                .AppendLine("   AND T1.ASSIGNSTATUS IN (:ASSIGNSTATUS_0, :ASSIGNSTATUS_1) ")
                .AppendLine("   FOR UPDATE WAIT 1 ")
            End With

            Using query As New DBSelectQuery(Of SC3090401DataSet.SC3090401LockTargetDataDataTable)("SC3240401_006")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceinId)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_0", OracleDbType.NVarchar2, NonAssign)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_1", OracleDbType.NVarchar2, AssignWait)

                ' SQL実行
                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END OUT:COUNT = {2}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return dt

        End Function
#End Region

#Region "来店車両実績更新"
        ''' <summary>
        ''' 来店車両実績更新
        ''' </summary>
        ''' <param name="inDealerCode">ログインユーザーの販売店コード</param>
        ''' <param name="inBranchCode">ログインユーザーの店舗コード</param>
        ''' <param name="inUpdateAccount">更新アカウント</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inRegNumSearch">来店させる車両登録番号(検索用)</param>
        ''' <remarks></remarks>
        Public Sub UpdateVisitVehicle(ByVal inDealerCode As String, _
                                      ByVal inBranchCode As String, _
                                      ByVal inUpdateAccount As String, _
                                      ByVal inNowDate As Date, _
                                      ByVal inRegNumSearch As String)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inUpdateAccount = {4}" & _
                      ", inNowDate = {5}, inRegNumSearch = {6}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inDealerCode, inBranchCode, inUpdateAccount, inNowDate, inRegNumSearch))

            Dim sql As New StringBuilder
            ' SQL文組立て
            With sql
                .AppendLine("UPDATE /* SC3090401_007 */ ")
                .AppendLine("       TBL_VISIT_VEHICLE T1 ")
                .AppendLine("   SET ")
                .AppendLine("       T1.DEALFLG = :DEALFLG_1 ")
                .AppendLine("     , T1.UPDATEDATE = SYSDATE ")
                .AppendLine("     , T1.UPDATEACCOUNT = :UPDATEACCOUNT ")
                .AppendLine("     , T1.UPDATEID = :UPDATEID ")
                .AppendLine(" WHERE ")
                .AppendLine("       T1.DLRCD = :DLRCD ")
                .AppendLine("   AND T1.STRCD = :STRCD ")
                .AppendLine("   AND T1.VISITTIMESTAMP ")
                .AppendLine("       BETWEEN TRUNC(:NOWDATE) ")
                .AppendLine("       AND TRUNC(:NOWDATE) + 86399/86400 ")
                .AppendLine("   AND T1.DEALFLG = :DEALFLG_0 ")
                .AppendLine("   AND T1.DELFLG = :DELFLG_0 ")
                .AppendLine("   AND UPPER(T1.VCLREGNO) = :REG_NUM_SEARCH ")
            End With

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3090401_007")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("DEALFLG_0", OracleDbType.Char, DealFlgOff)
                query.AddParameterWithTypeValue("DEALFLG_1", OracleDbType.Char, DealFlgOn)
                query.AddParameterWithTypeValue("DELFLG_0", OracleDbType.Char, DelFlgOff)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inUpdateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, ApplicationId)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inBranchCode)
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("REG_NUM_SEARCH", OracleDbType.NVarchar2, inRegNumSearch)

                'SQL実行
                query.Execute()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} END" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name))

        End Sub

#End Region

    End Class
End Namespace

Partial Class SC3090401DataSet
End Class
