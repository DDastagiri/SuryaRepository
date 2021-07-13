'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3220101DataSet.vb
'─────────────────────────────────────
'機能： SAステータスマネジメントメインメニュー データセット
'補足： 
'作成： 2012/05/28 日比野
'更新： 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応
'更新： 2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新： 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
'更新： 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新； 2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する
'更新： 2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新： 2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新：
'─────────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web

Imports Toyota.eCRB.iCROP.DataAccess.SC3220101.SC3220101DataSet

'2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003.IC3801003DataSet

'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003

'2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END


Namespace SC3220101DataSetTableAdapters
    Public Class SC3220101DataTableAdapter
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

        '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START

        ''' <summary>
        ''' サービスステータス（00：未入庫）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiceStatusNoneCarIn As String = "00"

        ''' <summary>
        ''' サービスステータス（01：未来店客）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiceStatusNoneVisit As String = "01"

        ''' <summary>
        ''' サービスステータス（02：キャンセル）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiceStatusCancel As String = "02"

        ''' <summary>
        ''' キャンセルフラグ（0：有効）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CancelTypeEffective As String = "0"

        ''' <summary>
        ''' ストール利用テータス（05：中断）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallUseStatusStop As String = "05"

        ''' <summary>
        ''' 日付最小値文字列
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DateMinValue As String = "1900/01/01 00:00:00"

        ''' <summary>
        ''' 振当てステータス（未振当て）
        ''' </summary>
        Private Const NonAssign As String = "0"

        ''' <summary>
        ''' 振当てステータス（受付待ち）
        ''' </summary>
        Private Const AssignWait As String = "1"

        ''' <summary>
        ''' 振当てステータス（振当済み）
        ''' </summary>
        Private Const AssignFinish As String = "2"

        ''' <summary>
        ''' 振当てステータス（HOLD中）
        ''' </summary>
        Private Const AssignHolding As String = "9"

        '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        ''' <summary>
        ''' DB日付省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MinDate As String = "1900/01/01 00:00:00"

        ''' <summary>
        ''' ROステータス（"10"：SA起票中）
        ''' </summary>
        Private Const StatusSAIssuance As String = "10"

        ''' <summary>
        ''' ROステータス（"35"：SA承認待ち）
        ''' </summary>
        Private Const StatusConfirmationWait As String = "35"

        ''' <summary>
        ''' ROステータス（"50"：着工指示待ち）
        ''' </summary>
        Private Const StatusInstructionsWait As String = "50"

        ''' <summary>
        ''' ROステータス（"60"：作業中）
        ''' </summary>
        Private Const StatusWork As String = "60"

        ''' <summary>
        ''' ROステータス（"80"：納車準備）
        ''' </summary>
        Private Const StatusDeliveryWait As String = "80"

        ''' <summary>
        ''' ROステータス（"85"：納車作業）
        ''' </summary>
        Private Const StatusDeliveryWork As String = "85"

        ''' <summary>
        ''' ROステータス（"99"：キャンセル）
        ''' </summary>
        Private Const StatusROCancel As String = "99"

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

        '2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START

        ''' <summary>
        ''' ストール利用ステータス(着工指示)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallStatusInstruct As String = "00"

        ''' <summary>
        ''' ストール利用ステータス(作業開始待ち)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallStatusWaitWork As String = "01"

        '2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        ''' <summary>
        ''' アイコンのフラグ(0：対象外)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOff As String = "0"
        ''' <summary>
        ''' アイコンのフラグ(1：対象内)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOn As String = "1"
        '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
#End Region

#Region "チップ情報取得"

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        ' ''' <summary>
        ' ''' サービス来店チップ情報取得
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <param name="orderNoList">整備受注No.(DataTable)</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' ''' 2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
        ' ''' </history>
        'Public Function GetVisitChip(ByVal dealerCode As String,
        '                             ByVal branchCode As String,
        '                             ByVal orderNoList As IC3801003DataSet.IC3801003NoDeliveryRODataTable) As SC3220101DataSet.SC3220101ChipInfoDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                            , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, orderNoList.Count = {5}" _
        '                            , Me.GetType.ToString _
        '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                            , LOG_START _
        '                            , dealerCode _
        '                            , branchCode _
        '                            , orderNoList.Rows.Count))

        '    Dim dt As SC3220101DataSet.SC3220101ChipInfoDataTable


        '    Using query As New DBSelectQuery(Of SC3220101DataSet.SC3220101ChipInfoDataTable)("SC3220101_001")

        '        Dim sql As New StringBuilder
        '        Dim sqlOrderNo As New StringBuilder
        '        Dim userContext As StaffContext = StaffContext.Current

        '        ' R/O番号用取得文字列
        '        If Not orderNoList Is Nothing Then
        '            If orderNoList.Count > 0 Then
        '                Dim count As Long = 1
        '                Dim orderName As String
        '                sqlOrderNo.Append("V.ORDERNO IN ( ")
        '                For Each row As IC3801003NoDeliveryRORow In orderNoList.Rows

        '                    If row.IsORDERNONull Then
        '                        Continue For
        '                    End If

        '                    'SQL作成
        '                    orderName = String.Format(CultureInfo.CurrentCulture, "ORDERNO{0}", count)
        '                    If count > 1 Then
        '                        sqlOrderNo.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", orderName))
        '                    Else
        '                        sqlOrderNo.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", orderName))
        '                    End If

        '                    'パラメータ作成
        '                    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        '                    'query.AddParameterWithTypeValue(orderName, OracleDbType.Char, row.ORDERNO)
        '                    query.AddParameterWithTypeValue(orderName, OracleDbType.NVarchar2, Trim(row.ORDERNO))
        '                    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
        '                    count += 1
        '                    If count > 900 Then
        '                        Exit For
        '                    End If
        '                Next
        '                sqlOrderNo.Append(" ) OR ")
        '            End If
        '        End If

        '        'SQL文作成
        '        With sql
        '            .AppendLine(" SELECT /* SC3220101_001 */ ")
        '            .AppendLine("        V.VISITSEQ        AS VISITSEQ ")
        '            .AppendLine("      , V.DLRCD           AS DLRCD ")
        '            .AppendLine("      , V.STRCD           AS STRCD ")
        '            .AppendLine("      , V.VCLREGNO        AS VCLREGNO ")
        '            .AppendLine("      , V.ORDERNO         AS ORDERNO ")
        '            .AppendLine("      , V.FREZID          AS PREZID ")
        '            .AppendLine("      , V.ASSIGNTIMESTAMP AS ASSIGNTIMESTAMP ")
        '            .AppendLine("      , V.CUSTSEGMENT     AS CUSTSEGMENT ")
        '            .AppendLine("      , V.SACODE          AS SACODE ")
        '            .AppendLine("   FROM TBL_SERVICE_VISIT_MANAGEMENT V ")
        '            .AppendLine("  WHERE V.DLRCD = :DLRCD ")
        '            .AppendLine("    AND V.STRCD = :STRCD ")
        '            '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        '            '.AppendLine("    AND V.ASSIGNSTATUS = '2' ")
        '            .AppendLine("    AND V.ASSIGNSTATUS = :ASSIGNSTATUS_2 ")
        '            '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
        '            .AppendLine("    AND( ").Append(sqlOrderNo.ToString)
        '            .AppendLine("        (NVL(V.ORDERNO,' ') = ' ' AND VISITTIMESTAMP > TRUNC(:NOWDATE)) ")
        '            .AppendLine("       ) ")
        '        End With

        '        Try
        '            query.CommandText = sql.ToString()

        '            'バインド変数
        '            '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        '            'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '            'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
        '            query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
        '            query.AddParameterWithTypeValue("ASSIGNSTATUS_2", OracleDbType.NVarchar2, AssignFinish)
        '            '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
        '            query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, DateTimeFunc.Now(userContext.DlrCD))


        '            '検索結果返却
        '            dt = query.GetData()
        '        Catch ex As OracleExceptionEx When ex.Number = 1013
        '            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , C_RET_DBTIMEOUT _
        '                                    , ex.Message))
        '            Throw ex
        '        End Try
        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                            , "{0}.{1} {2} OUT:COUNT = {3}" _
        '                            , Me.GetType.ToString _
        '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                            , LOG_END _
        '                            , dt.Rows.Count))
        '    Return dt

        'End Function

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        ''' <summary>
        ''' サービス来店チップ情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetVisitChip(ByVal dealerCode As String,
                                     ByVal branchCode As String, _
                                     ByVal inPresentTime As Date) _
                                     As SC3220101DataSet.SC3220101ChipInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_START _
                                    , dealerCode _
                                    , branchCode))

            Dim dt As SC3220101DataSet.SC3220101ChipInfoDataTable


            Using query As New DBSelectQuery(Of SC3220101DataSet.SC3220101ChipInfoDataTable)("SC3220101_001")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("SELECT /* SC3220101_001 */ ")
                    .AppendLine("       T1.VISITSEQ ")
                    .AppendLine("     , T1.DLRCD ")
                    .AppendLine("     , T1.STRCD ")
                    .AppendLine("     , T1.VCLREGNO ")
                    .AppendLine("     , T1.ORDERNO ")
                    .AppendLine("     , T1.PREZID ")
                    .AppendLine("     , T1.ASSIGNTIMESTAMP ")
                    .AppendLine("     , T1.CUSTSEGMENT ")
                    .AppendLine("     , T1.SACODE ")
                    .AppendLine("     , T1.VISIT_ID ")
                    .AppendLine("     , T1.MAX_RO_STATUS ")
                    .AppendLine("     , T1.MIN_RO_STATUS ")
                    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                    .AppendLine("     , T1.IMP_VCL_FLG ")
                    .AppendLine("     , T1.SML_AMC_FLG ")
                    .AppendLine("     , T1.EW_FLG ")
                    .AppendLine("     , T1.TLM_MBR_FLG ")
                    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

                    .AppendLine("  FROM (SELECT V.VISITSEQ                                      AS VISITSEQ ")
                    .AppendLine("             , MAX(V.DLRCD)                                    AS DLRCD ")
                    .AppendLine("             , MAX(V.STRCD)                                    AS STRCD ")
                    '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                    '.AppendLine("             , MAX(V.VCLREGNO)                                 AS VCLREGNO ")
                    .AppendLine("             , NVL(MAX(TRIM(VD.REG_NUM)), MAX(V.VCLREGNO))           AS VCLREGNO ")
                    '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                    .AppendLine("             , MAX(V.ORDERNO)                                  AS ORDERNO ")
                    .AppendLine("             , MAX(V.FREZID)                                   AS PREZID ")
                    .AppendLine("             , MAX(V.ASSIGNTIMESTAMP)                          AS ASSIGNTIMESTAMP ")
                    .AppendLine("             , MAX(V.CUSTSEGMENT)                              AS CUSTSEGMENT ")
                    .AppendLine("             , MAX(V.SACODE)                                   AS SACODE ")
                    .AppendLine("             , MAX(R.VISIT_ID)                                 AS VISIT_ID ")
                    .AppendLine("             , MAX(TRIM(R.RO_STATUS))                          AS MAX_RO_STATUS ")
                    .AppendLine("             , MIN(NVL(TRIM(R2.RO_STATUS), TRIM(R.RO_STATUS))) AS MIN_RO_STATUS ")
                    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                    .AppendLine("             , MAX(NVL(TRIM(VD.IMP_VCL_FLG), :ICON_FLAG_OFF))  AS IMP_VCL_FLG ")
                    .AppendLine("             , MAX(NVL(TRIM(LV.SML_AMC_FLG), :ICON_FLAG_OFF))  AS SML_AMC_FLG ")
                    .AppendLine("             , MAX(NVL(TRIM(LV.EW_FLG), :ICON_FLAG_OFF))       AS EW_FLG ")
                    .AppendLine("             , MAX(CASE ")
                    .AppendLine("                    WHEN TLM.VCL_VIN IS NOT NULL THEN :ICON_FLAG_ON ")
                    .AppendLine("                    ELSE :ICON_FLAG_OFF ")
                    .AppendLine("              END) AS TLM_MBR_FLG ")
                    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                    .AppendLine("          FROM TBL_SERVICE_VISIT_MANAGEMENT V ")
                    .AppendLine("             , TB_T_RO_INFO R ")
                    .AppendLine("             , TB_T_RO_INFO R2 ")
                    '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                    .AppendLine("             , TB_M_VEHICLE_DLR VD ")
                    '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                    .AppendLine("             , TB_M_VEHICLE VCL ")
                    .AppendLine("             , TB_LM_VEHICLE LV ")
                    .AppendLine("             , TB_LM_TLM_MEMBER TLM ")
                    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                    .AppendLine("         WHERE V.VISITSEQ = R.VISIT_ID(+) ")
                    .AppendLine("           AND V.VISITSEQ = R2.VISIT_ID(+) ")
                    '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                    .AppendLine("           AND V.VCL_ID = VD.VCL_ID(+) ")
                    '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                    .AppendLine("           AND V.VCL_ID = VCL.VCL_ID(+) ")
                    .AppendLine("           AND VCL.VCL_ID = LV.VCL_ID(+) ")
                    .AppendLine("           AND VCL.VCL_VIN = TLM.VCL_VIN(+) ")
                    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                    .AppendLine("           AND V.DLRCD = :DLRCD ")
                    .AppendLine("           AND V.STRCD = :STRCD ")
                    .AppendLine("           AND V.ASSIGNSTATUS = :ASSIGNSTATUS_2 ")
                    .AppendLine("           AND (    ( R.RO_STATUS IN (:STATUS_50, :STATUS_60, :STATUS_80, :STATUS_85) ) ")
                    .AppendLine("                 OR ( V.ORDERNO IS NULL AND V.VISITTIMESTAMP > TRUNC(:NOWDATE) ) ) ")
                    .AppendLine("           AND R.DLR_CD(+) = :DLRCD ")
                    .AppendLine("           AND R.BRN_CD(+) = :STRCD ")
                    .AppendLine("           AND R.RO_STATUS(+) <> :STATUS_99 ")
                    .AppendLine("           AND R2.DLR_CD(+) = :DLRCD ")
                    .AppendLine("           AND R2.BRN_CD(+) = :STRCD ")
                    .AppendLine("           AND R2.RO_STATUS(+) <> :STATUS_99 ")
                    '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                    .AppendLine("           AND VD.DLR_CD(+) = :DLRCD ")
                    '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                    .AppendLine("         GROUP BY V.VISITSEQ) T1 ")
                    .AppendLine(" WHERE ROWNUM <= 900 ")
                End With

                Try

                    'SQL設定
                    query.CommandText = sql.ToString()

                    'バインド変数

                    '販売店コード
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                    '店舗コード
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                    '振当てステータス(2：SA振当済)
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_2", OracleDbType.NVarchar2, AssignFinish)
                    'ROステータス("50"：着工指示待ち)
                    query.AddParameterWithTypeValue("STATUS_50", OracleDbType.NVarchar2, StatusInstructionsWait)
                    'ROステータス("60"：作業中)
                    query.AddParameterWithTypeValue("STATUS_60", OracleDbType.NVarchar2, StatusWork)
                    'ROステータス("80"：納車準備待ち)
                    query.AddParameterWithTypeValue("STATUS_80", OracleDbType.NVarchar2, StatusDeliveryWait)
                    'ROステータス("85"：納車作業中)
                    query.AddParameterWithTypeValue("STATUS_85", OracleDbType.NVarchar2, StatusDeliveryWork)
                    'ROステータス（"99"：キャンセル）
                    query.AddParameterWithTypeValue("STATUS_99", OracleDbType.NVarchar2, StatusROCancel)
                    '現在日時
                    query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inPresentTime)

                    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                    query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                    query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)
                    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

                    '検索結果返却
                    dt = query.GetData()

                Catch ex As OracleExceptionEx When ex.Number = 1013

                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                            , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
                                            , Me.GetType.ToString _
                                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                            , C_RET_DBTIMEOUT _
                                            , ex.Message))

                    Throw ex

                End Try

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dt.Rows.Count))

            Return dt

        End Function

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

        ''' <summary>
        ''' 予約チップ情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="visitChipNoList">サービス来店チップ情報</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetReserveChip(ByVal dealerCode As String,
                                       ByVal branchCode As String,
                                       ByVal visitChipNoList As SC3220101ChipInfoDataTable) As SC3220101ChipInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_START _
                                    , dealerCode _
                                    , branchCode))

            Dim dt As SC3220101ChipInfoDataTable = Nothing
            Dim sqlRezId As New StringBuilder
            Dim count As Long = 0
            Dim reserveIdList As List(Of Decimal) = Nothing

            ' 予約ID用取得文字列
            If Not visitChipNoList Is Nothing Then
                If visitChipNoList.Count > 0 Then
                    reserveIdList = New List(Of Decimal)

                    Dim rezIdName As String
                    For Each row As SC3220101ChipInfoRow In visitChipNoList.Rows

                        If row.IsPREZIDNull Then
                            Continue For
                        End If

                        count += 1
                        ' SQL作成
                        rezIdName = String.Format(CultureInfo.CurrentCulture, "REZID{0}", count)
                        If count > 1 Then
                            sqlRezId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", rezIdName))
                        Else
                            sqlRezId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", rezIdName))
                        End If

                        ' パラメータ作成
                        reserveIdList.Add(row.PREZID)
                    Next
                End If
            End If

            Dim sql As New StringBuilder
            With sql
                '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                '.AppendLine(" SELECT /* SC3220101_002 */ ")
                '.AppendLine("        PREZID ")
                '.AppendLine("      , MIN(ACTUAL_STIME)                                  AS FIRST_STARTTIME ")
                '.AppendLine("      , MAX(NVL2(RESULT_END_TIME ")
                '.AppendLine("               , TO_DATE(RESULT_END_TIME,'YYYYMMDDHH24MI') ")
                '.AppendLine("               , ENDTIME))                                 AS LAST_ENDTIME  ")
                '.AppendLine("      , SUM(DECODE(RESULT_STATUS,'0' ,REZ_WORK_TIME ")
                '.AppendLine("          , DECODE(RESULT_STATUS,'00',REZ_WORK_TIME ")
                '.AppendLine("          , DECODE(RESULT_STATUS,'10',REZ_WORK_TIME ")
                '.AppendLine("          , DECODE(RESULT_STATUS,NULL,REZ_WORK_TIME,0))))) AS WORK_TIME ")
                '.AppendLine("      , SUM(DECODE(NVL(STOPFLG,0),0,0,1))                  AS UNVALID_REZ_COUNT ")
                '.AppendLine("      , MAX(DECODE(NUM,1,RESULT_STATUS,NULL))              AS RESULT_STATUS ")
                '.AppendLine("      , MAX(WASHFLG)                                       AS WASHFLG ")
                '.AppendLine("      , TO_DATE(MAX(RESULT_WASH_START),'YYYYMMDDHH24MI')   AS WASH_START ")
                '.AppendLine("      , TO_DATE(MAX(RESULT_WASH_END),'YYYYMMDDHH24MI')     AS WASH_END ")
                '.AppendLine("      , MAX(INSTRUCT)                                      AS INSTRUCT")
                '.AppendLine("      , DECODE(MIN(NUM),0,'0','1')                         AS RESULT_TYPE")
                '.AppendLine("   FROM ")
                '.AppendLine("     (SELECT NVL(R.PREZID,R.REZID) AS PREZID ")
                '.AppendLine("           , R.STARTTIME ")
                '.AppendLine("           , R.ENDTIME ")
                '.AppendLine("           , R.ACTUAL_STIME ")
                '.AppendLine("           , R.ACTUAL_ETIME ")
                '.AppendLine("           , R.REZ_WORK_TIME ")
                '.AppendLine("           , R.WASHFLG ")
                '.AppendLine("           , R.STOPFLG ")
                '.AppendLine("           , R.CANCELFLG ")
                '.AppendLine("           , R.REZCHILDNO ")
                '.AppendLine("           , R.INSTRUCT ")
                '.AppendLine("           , P.RESULT_STATUS ")
                '.AppendLine("           , P.RESULT_WASH_START ")
                '.AppendLine("           , P.RESULT_WASH_END ")
                '.AppendLine("           , P.RESULT_END_TIME ")
                '.AppendLine("           , ROW_NUMBER() OVER ( ")
                '.AppendLine("                 PARTITION BY NVL(R.PREZID,R.REZID) ")
                '.AppendLine("                 ORDER BY ENDTIME DESC, ")
                '.AppendLine("                          STARTTIME DESC) NUM ")
                '.AppendLine("        FROM TBL_STALLPROCESS P ")
                '.AppendLine("           , TBL_STALLREZINFO R ")
                '.AppendLine("       WHERE R.DLRCD = P.DLRCD ")
                '.AppendLine("         AND R.STRCD = P.STRCD ")
                '.AppendLine("         AND R.REZID = P.REZID ")
                '.AppendLine("         AND R.DLRCD = :DLRCD ")
                '.AppendLine("         AND R.STRCD = :STRCD ")
                '.AppendLine("         AND (R.REZID IN (").Append(sqlRezId).Append(") ")
                '.AppendLine("          OR R.PREZID IN (").Append(sqlRezId).Append(")) ")
                '.AppendLine("         AND DECODE(R.STOPFLG,'0',DECODE(R.CANCELFLG,'1',1,0),0) + ")
                '.AppendLine("             DECODE(R.REZCHILDNO,0,1,0) + ")
                '.AppendLine("             DECODE(R.REZCHILDNO,999,1,0) = 0 ")
                '.AppendLine("         AND P.SEQNO = ( ")
                '.AppendLine("             SELECT MAX(SEQNO) ")
                '.AppendLine("               FROM TBL_STALLPROCESS ")
                '.AppendLine("              WHERE P.DLRCD = DLRCD ")
                '.AppendLine("                AND P.STRCD = STRCD ")
                '.AppendLine("                AND P.REZID = REZID ")
                '.AppendLine("                AND P.DSEQNO = DSEQNO) ")
                '.AppendLine("         AND P.DSEQNO = ( ")
                '.AppendLine("             SELECT MAX(DSEQNO) ")
                '.AppendLine("               FROM TBL_STALLPROCESS ")
                '.AppendLine("              WHERE P.DLRCD = DLRCD ")
                '.AppendLine("                AND P.STRCD = STRCD ")
                '.AppendLine("                AND P.REZID = REZID) ")
                '.AppendLine("       UNION ALL ")
                '.AppendLine("      SELECT NVL(R.PREZID,R.REZID) AS PREZID ")
                '.AppendLine("           , R.STARTTIME ")
                '.AppendLine("           , R.ENDTIME ")
                '.AppendLine("           , R.ACTUAL_STIME ")
                '.AppendLine("           , R.ACTUAL_ETIME ")
                '.AppendLine("           , R.REZ_WORK_TIME ")
                '.AppendLine("           , R.WASHFLG ")
                '.AppendLine("           , R.STOPFLG ")
                '.AppendLine("           , R.CANCELFLG ")
                '.AppendLine("           , R.REZCHILDNO ")
                '.AppendLine("           , R.INSTRUCT ")
                '.AppendLine("           , NULL ")
                '.AppendLine("           , NULL ")
                '.AppendLine("           , NULL ")
                '.AppendLine("           , NULL ")
                '.AppendLine("           , 0 ")
                '.AppendLine("        FROM TBL_STALLREZINFO R ")
                '.AppendLine("       WHERE R.DLRCD = :DLRCD ")
                '.AppendLine("         AND R.STRCD = :STRCD ")
                '.AppendLine("         AND (R.REZID IN (").Append(sqlRezId).Append(") ")
                '.AppendLine("          OR R.PREZID IN (").Append(sqlRezId).Append(")) ")
                '.AppendLine("         AND DECODE(R.STOPFLG,'0',DECODE(R.CANCELFLG,'1',1,0),0) + ")
                '.AppendLine("             DECODE(R.REZCHILDNO,0,1,0) + ")
                '.AppendLine("             DECODE(R.REZCHILDNO,999,1,0) = 0 ")
                '.AppendLine("         AND NOT EXISTS ( ")
                '.AppendLine("                 SELECT '1' ")
                '.AppendLine("                   FROM TBL_STALLPROCESS ")
                '.AppendLine("                  WHERE R.DLRCD = DLRCD ")
                '.AppendLine("                    AND R.STRCD = STRCD ")
                '.AppendLine("                    AND R.REZID = REZID)  ")
                '.AppendLine("     ) M ")
                '.AppendLine(" GROUP BY M.PREZID ")

                .AppendLine("SELECT /* SC3220101_002 */ ")
                .AppendLine("       T1.SVCIN_ID AS PREZID ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

                .AppendLine("      ,MAX(CASE ")
                .AppendLine("                WHEN T1.SCHE_DELI_DATETIME = :MINDATE THEN NULL ")
                .AppendLine("                ELSE T1.SCHE_DELI_DATETIME ")
                .AppendLine("            END ) AS REZ_DELI_TIME ")
                .AppendLine("      ,MAX(CASE ")
                .AppendLine("                WHEN T1.INVOICE_PREP_COMPL_DATETIME = :MINDATE THEN NULL ")
                .AppendLine("                ELSE T1.INVOICE_PREP_COMPL_DATETIME ")
                .AppendLine("            END ) AS INVOICE_PRINT_DATETIME ")
                .AppendLine("      ,MAX(CASE  ")
                .AppendLine("                WHEN T1.INSPECTION_APPROVAL_DATETIME = :MINDATE THEN NULL ")
                .AppendLine("                ELSE T1.INSPECTION_APPROVAL_DATETIME  ")
                .AppendLine("            END) AS MAX_INSPECTION_DATE ")
                .AppendLine("      ,MIN(CASE  ")
                .AppendLine("                WHEN T1.INSPECTION_APPROVAL_DATETIME = :MINDATE THEN NULL ")
                .AppendLine("                ELSE T1.INSPECTION_APPROVAL_DATETIME  ")
                .AppendLine("            END) AS MIN_INSPECTION_DATE ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                .AppendLine("      ,MIN(T1.RSLT_START_DATETIME) AS FIRST_STARTTIME ")
                .AppendLine("      ,MAX(CASE ")
                .AppendLine("           WHEN T1.RSLT_END_DATETIME <> :MINDATE THEN T1.RSLT_END_DATETIME ")
                .AppendLine("           WHEN T1.PRMS_END_DATETIME <> :MINDATE THEN T1.PRMS_END_DATETIME ")
                .AppendLine("           ELSE T1.SCHE_END_DATETIME END) AS LAST_ENDTIME ")
                '2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START

                '正しく残作業時間が取得できないため削除
                '.AppendLine("      ,SUM(DECODE(T1.SVC_STATUS, :SVC_STATUS_00, T1.SCHE_WORKTIME, ")
                '.AppendLine("           DECODE(T1.SVC_STATUS, :SVC_STATUS_01, T1.SCHE_WORKTIME, 0))) AS WORK_TIME ")
                .AppendLine("      ,SUM(DECODE(T1.STALL_USE_STATUS, :USE_STATUS_00, T1.SCHE_WORKTIME, ")
                .AppendLine("           DECODE(T1.STALL_USE_STATUS, :USE_STATUS_01, T1.SCHE_WORKTIME, 0))) AS WORK_TIME ")

                '2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                .AppendLine("      ,SUM(DECODE(T1.STALL_USE_STATUS, :STALL_USE_STATUS_05, 1, 0)) AS UNVALID_REZ_COUNT ")
                .AppendLine("      ,MAX(T1.SVC_STATUS) AS RESULT_STATUS ")
                .AppendLine("      ,MAX(NVL2(TRIM(T1.CARWASH_NEED_FLG), T1.CARWASH_NEED_FLG, 0)) AS WASHFLG ")
                .AppendLine("      ,MAX(T1.RSLT_START_DATETIME_WASH) AS WASH_START ")
                .AppendLine("      ,MAX(CASE  ")
                .AppendLine("                WHEN T1.RSLT_END_DATETIME_WASH = :MINDATE THEN NULL ")
                .AppendLine("                ELSE T1.RSLT_END_DATETIME_WASH  ")
                .AppendLine("            END) AS WASH_END ")
                .AppendLine("      ,MAX(T1.STALL_USE_STATUS) AS INSTRUCT ")
                .AppendLine("      ,MAX(DECODE(T1.RSLT_START_DATETIME, :MINDATE, 0, 1)) AS RESULT_TYPE ")
                '2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                .AppendLine("      ,MIN(DECODE(T1.INSPECTION_NEED_FLG, 1, T1.INSPECTION_STATUS, 2)) AS REMAINING_INSPECTION_TYPE ")
                '2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                .AppendLine(" FROM ")
                .AppendLine("     (SELECT M1.SVCIN_ID ")
                .AppendLine("            ,M1.CARWASH_NEED_FLG ")
                .AppendLine("            ,M1.SVC_STATUS ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

                .AppendLine("            ,M1.SCHE_DELI_DATETIME ")
                .AppendLine("            ,M1.INVOICE_PREP_COMPL_DATETIME ")
                .AppendLine("            ,M2.INSPECTION_APPROVAL_DATETIME ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                '2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                .AppendLine("            ,M2.INSPECTION_NEED_FLG ")
                .AppendLine("            ,M2.INSPECTION_STATUS ")
                '2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                .AppendLine("            ,M3.SCHE_END_DATETIME ")
                .AppendLine("            ,M3.SCHE_WORKTIME ")
                .AppendLine("            ,M3.RSLT_START_DATETIME ")
                .AppendLine("            ,M3.PRMS_END_DATETIME ")
                .AppendLine("            ,M3.RSLT_END_DATETIME ")
                .AppendLine("            ,M3.STALL_USE_STATUS ")
                .AppendLine("            ,M4.RSLT_START_DATETIME AS RSLT_START_DATETIME_WASH ")
                .AppendLine("            ,M4.RSLT_END_DATETIME AS RSLT_END_DATETIME_WASH ")
                .AppendLine("            ,ROW_NUMBER() OVER ( ")
                .AppendLine("             PARTITION BY M1.SVCIN_ID ")
                .AppendLine("             ORDER BY M3.SCHE_END_DATETIME DESC ")
                .AppendLine("            ,M3.SCHE_START_DATETIME DESC) NUM ")
                .AppendLine("        FROM TB_T_SERVICEIN M1 ")
                .AppendLine("            ,TB_T_JOB_DTL M2 ")
                .AppendLine("            ,TB_T_STALL_USE M3 ")
                .AppendLine("            ,TB_T_CARWASH_RESULT M4 ")
                .AppendLine("       WHERE M1.SVCIN_ID = M2.SVCIN_ID ")
                .AppendLine("         AND M2.JOB_DTL_ID = M3.JOB_DTL_ID ")
                .AppendLine("         AND M1.SVCIN_ID = M4.SVCIN_ID(+) ")
                .AppendLine("         AND M3.DLR_CD = :DLR_CD ")
                .AppendLine("         AND M3.BRN_CD = :BRN_CD ")
                .AppendLine("         AND M1.SVCIN_ID IN ( ")
                .AppendLine(sqlRezId.ToString)
                .AppendLine("         ) ")
                .AppendLine("         AND M1.SVC_STATUS <> :SVC_STATUS_02 ")
                .AppendLine("         AND M2.CANCEL_FLG = :CANCEL_FLG_0) T1 ")
                .AppendLine(" GROUP BY T1.SVCIN_ID ")
                '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
            End With

            Using query As New DBSelectQuery(Of SC3220101ChipInfoDataTable)("SC3220101_002")
                Try
                    query.CommandText = sql.ToString()

                    'バインド変数
                    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))

                    '2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, ServiceStatusNoneCarIn)
                    'query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, ServiceStatusNoneVisit)
                    query.AddParameterWithTypeValue("USE_STATUS_00", OracleDbType.NVarchar2, StallStatusInstruct)
                    query.AddParameterWithTypeValue("USE_STATUS_01", OracleDbType.NVarchar2, StallStatusWaitWork)
                    '2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                    query.AddParameterWithTypeValue("STALL_USE_STATUS_05", OracleDbType.NVarchar2, StallUseStatusStop)
                    query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                    query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                    query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)
                    query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                    Dim resetveIdName As String = Nothing
                    Dim i As Integer = 1
                    For Each reserveId As Decimal In reserveIdList
                        resetveIdName = String.Format(CultureInfo.CurrentCulture, "REZID{0}", i)
                        '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                        'query.AddParameterWithTypeValue(resetveIdName, OracleDbType.Int32, reserveId)
                        query.AddParameterWithTypeValue(resetveIdName, OracleDbType.Decimal, reserveId)
                        '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                        i = i + 1
                    Next

                    '検索結果返却
                    dt = query.GetData()
                Catch ex As OracleExceptionEx When ex.Number = 1013
                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                            , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
                                            , Me.GetType.ToString _
                                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                            , C_RET_DBTIMEOUT _
                                            , ex.Message))
                    Throw
                End Try
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dt.Rows.Count))
            Return dt
        End Function

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        ''' <summary>
        ''' 追加作業チップ情報取得
        ''' </summary>
        ''' <param name="inDTChipInfo">作業中・納車準備・納車作業エリアチップ情報</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns>追加作業エリアチップ情報取得データセット</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetAddApprovalChipInfo(ByVal inDTChipInfo As SC3220101DataSet.SC3220101ChipInfoDataTable _
                                             , ByVal inDealerCode As String _
                                             , ByVal inBranchCode As String) _
                                               As SC3220101DataSet.SC3220101AddApprovalChipInfoDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN: COUNT = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , inDTChipInfo.Count))

            Dim dt As SC3220101AddApprovalChipInfoDataTable

            Try
                Using query As New DBSelectQuery(Of SC3220101AddApprovalChipInfoDataTable)("SC3220101_005")
                    Dim sql As New StringBuilder

                    '来店実績連番　条件用文字列
                    Dim sqlVisitSeq As New StringBuilder

                    '来店実績連番　条件IN
                    sqlVisitSeq.AppendLine(" T1.VISIT_ID IN ( ")

                    '来店実績連番の数
                    Dim count As Long = 1

                    'パラメータ用文字列
                    Dim visitSeqPramName As String

                    '工程エリアチップ分ループ
                    For Each row As SC3220101ChipInfoRow In inDTChipInfo

                        ' SQL作成
                        visitSeqPramName = String.Format(CultureInfo.CurrentCulture, "VISIT_ID{0}", count)

                        '1行目か判定
                        If 1 < count Then
                            '2行目以降

                            'カンマ設定
                            sqlVisitSeq.AppendLine(String.Format(CultureInfo.CurrentCulture, ", :{0} ", visitSeqPramName))
                        Else
                            '1行目

                            'カンマ無し
                            sqlVisitSeq.AppendLine(String.Format(CultureInfo.CurrentCulture, "  :{0} ", visitSeqPramName))
                        End If

                        ' パラメータ作成
                        query.AddParameterWithTypeValue(visitSeqPramName, OracleDbType.Int64, row.VISITSEQ)

                        count += 1
                    Next

                    sqlVisitSeq.AppendLine(" ) ")


                    'SQL文作成
                    With sql

                        .AppendLine("    SELECT /* SC3220101_005 */ ")
                        .AppendLine("           T1.VISIT_ID AS VISIT_ID ")
                        .AppendLine("          ,T1.RO_SEQ ")
                        .AppendLine("          ,MAX(CASE  ")
                        .AppendLine("                    WHEN T1.RO_CHECK_DATETIME = :MINDATE THEN :MINVALUE ")
                        .AppendLine("                    ELSE T1.RO_CHECK_DATETIME ")
                        .AppendLine("               END) AS RO_CHECK_DATETIME ")
                        .AppendLine("     FROM TB_T_RO_INFO T1 ")
                        .AppendLine("    WHERE ")
                        .AppendLine(sqlVisitSeq.ToString)
                        .AppendLine("      AND T1.DLR_CD = :DLR_CD ")
                        .AppendLine("      AND T1.BRN_CD = :BRN_CD ")
                        .AppendLine("      AND T1.RO_STATUS = :STATUS_35 ")
                        .AppendLine(" GROUP BY T1.VISIT_ID ")
                        .AppendLine("         ,T1.RO_SEQ ")

                    End With

                    'SQL設定
                    query.CommandText = sql.ToString()

                    'バインド変数

                    '日付省略値
                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
                    '日付最小値
                    query.AddParameterWithTypeValue("MINVALUE", OracleDbType.Date, Date.MinValue)
                    '販売店コード
                    query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                    '店舗コード
                    query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                    'ROステータス("35"：SA承認待ち)
                    query.AddParameterWithTypeValue("STATUS_35", OracleDbType.NVarchar2, StatusConfirmationWait)

                    'SQL実行
                    dt = query.GetData()

                End Using

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理

                Logger.Error(String.Format(CultureInfo.CurrentCulture _
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

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

#End Region

#Region "サービス来店管理情報"

        ''' <summary>
        ''' サービス来店管理情報を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="strCode">店舗コード</param>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
        ''' </history>
        Public Function GetVisitManagement(ByVal dealerCode As String, _
                                           ByVal strCode As String, _
                                           ByVal visitSeq As Long) As SC3220101DataSet.SC3220101ServiceVisitManagerInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} IN:dealerCode = {3}, strCode = {4}, visitSeq = {5}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_START _
                                    , dealerCode _
                                    , strCode _
                                    , visitSeq))

            Dim dt As SC3220101DataSet.SC3220101ServiceVisitManagerInfoDataTable

            Dim sql As New StringBuilder

            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

            'SQL文作成
            'With sql
            '.AppendLine(" SELECT /* SC3220101_003 */ ")
            '.AppendLine("        VISITSEQ ")                     '来店実績連番
            '.AppendLine("      , DLRCD ")                        '販売店コード
            '.AppendLine("      , STRCD ")                        '店舗コード
            '.AppendLine("      , VCLREGNO ")                     '整備受注No.
            '.AppendLine("      , ORDERNO ")                      '車両登録No
            '.AppendLine("      , VIN ")                          'VIN
            '.AppendLine("      , CUSTSEGMENT ")                  '顧客区分


            '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

            '.AppendLine("      , DMSID ")                        '基幹顧客コード

            '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END


            '.AppendLine("   FROM TBL_SERVICE_VISIT_MANAGEMENT ")


            '.AppendLine("  WHERE DLRCD = :DLRCD ")
            '.AppendLine("    AND STRCD = :STRCD ")
            '.AppendLine("    AND VISITSEQ = :VISITSEQ ")
            'End With

            With sql
                .AppendLine(" SELECT /* SC3220101_003 */ ")
                .AppendLine("        T1.VISITSEQ ")                                                      '来店実績連番
                .AppendLine("      , T1.DLRCD ")                                                         '販売店コード
                .AppendLine("      , T1.STRCD ")                                                         '店舗コード
                .AppendLine("      , T1.VCLREGNO ")                                                      '整備受注No.
                .AppendLine("      , T1.ORDERNO ")                                                       '車両登録No
                .AppendLine("      , T1.VIN ")                                                           'VIN
                .AppendLine("      , T1.CUSTSEGMENT ")                                                   '顧客区分
                .AppendLine("      , T1.DMSID ")                                                         '基幹顧客コード
                .AppendLine("      , NVL( TRIM(T3.DMS_CST_CD) , TRIM(T1.DMSID) ) AS DMSID_CSTDTLUSE ")   '基幹顧客コード(顧客詳細用)
                .AppendLine("   FROM TBL_SERVICE_VISIT_MANAGEMENT T1 ")                                  '「サービス来店管理」テーブル
                .AppendLine("      , TB_T_SERVICEIN T2 ")                                                '「サービス入庫」テーブル
                .AppendLine("      , TB_M_CUSTOMER T3 ")                                                 '「顧客」テーブル
                .AppendLine("  WHERE T1.FREZID = T2.SVCIN_ID(+) ")
                .AppendLine("    AND T2.CST_ID = T3.CST_ID(+) ")
                .AppendLine("    AND T1.DLRCD = :DLRCD ")
                .AppendLine("    AND T1.STRCD = :STRCD ")
                .AppendLine("    AND T1.VISITSEQ = :VISITSEQ ")
            End With

            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

            Using query As New DBSelectQuery(Of SC3220101DataSet.SC3220101ServiceVisitManagerInfoDataTable)("SC3220101_003")
                Try
                    query.CommandText = sql.ToString()

                    'バインド変数
                    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strCode)
                    'query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int32, visitSeq)
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, strCode)
                    query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSeq)
                    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                    '検索結果返却
                    dt = query.GetData()
                Catch ex As OracleExceptionEx When ex.Number = 1013
                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                            , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
                                            , Me.GetType.ToString _
                                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                            , C_RET_DBTIMEOUT _
                                            , ex.Message))
                    Throw
                End Try
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dt.Rows.Count))
            Return dt

        End Function

#End Region

#Region "来店(受付待ち)情報取得"
        '2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 START

        ''' <summary>
        ''' 来店(受付待ち)情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="nowDate">現在時刻</param>
        ''' <returns>受付待ち情報</returns>
        ''' <remarks></remarks>
        Public Function GetVisitAreaChip(ByVal dealerCode As String, _
                                         ByVal branchCode As String, _
                                         ByVal nowDate As Date) As SC3220101VisitAreaInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_START _
                                    , dealerCode _
                                    , branchCode))

            Dim dt As SC3220101VisitAreaInfoDataTable = Nothing
            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine(" SELECT /* SC3220101_004 */ ")
                .AppendLine("        V.VISITSEQ                  AS VISITSEQ ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                '.AppendLine("      , V.VCLREGNO        AS VCLREGNO ")
                .AppendLine("      , NVL(TRIM(VD.REG_NUM), V.VCLREGNO) AS VCLREGNO ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                .AppendLine("      , V.VISITTIMESTAMP            AS VISITTIMESTAMP ")
                '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("      , NVL(TRIM(VD.IMP_VCL_FLG), :ICON_FLAG_OFF)  AS IMP_VCL_FLG ")
                .AppendLine("      , NVL(TRIM(LV.SML_AMC_FLG), :ICON_FLAG_OFF)  AS SML_AMC_FLG ")
                .AppendLine("      , NVL(TRIM(LV.EW_FLG), :ICON_FLAG_OFF)       AS EW_FLG ")
                .AppendLine("      , CASE ")
                .AppendLine("               WHEN TLM.VCL_VIN IS NOT NULL THEN :ICON_FLAG_ON ")
                .AppendLine("               ELSE :ICON_FLAG_OFF ")
                .AppendLine("        END AS TLM_MBR_FLG ")
                '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("   FROM TBL_SERVICE_VISIT_MANAGEMENT V ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                .AppendLine("       ,TB_M_VEHICLE_DLR VD ")
                '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("       , TB_M_VEHICLE VCL ")
                .AppendLine("       , TB_LM_VEHICLE LV ")
                .AppendLine("       , TB_LM_TLM_MEMBER TLM ")
                '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                '.AppendLine("  WHERE V.DLRCD = :DLRCD ")
                .AppendLine("  WHERE V.VCL_ID = VD.VCL_ID(+) ")
                '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("    AND V.VCL_ID = VCL.VCL_ID(+) ")
                .AppendLine("    AND VCL.VCL_ID = LV.VCL_ID(+) ")
                .AppendLine("    AND VCL.VCL_VIN = TLM.VCL_VIN(+) ")
                '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("    AND V.DLRCD = :DLRCD ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                .AppendLine("    AND V.STRCD = :STRCD ")
                .AppendLine("    AND VISITTIMESTAMP >= TRUNC(:NOWDATE) ")
                '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                '.AppendLine("    AND V.ASSIGNSTATUS IN ('0', '1', '9') ")
                .AppendLine("    AND V.ASSIGNSTATUS IN (:ASSIGNSTATUS_0, :ASSIGNSTATUS_1, :ASSIGNSTATUS_9) ")
                '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                .AppendLine("    AND VD.DLR_CD(+) = :DLRCD ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                .AppendLine("  ORDER BY VISITTIMESTAMP ASC ")
            End With

            Using query As New DBSelectQuery(Of SC3220101VisitAreaInfoDataTable)("SC3220101_004")

                Try
                    query.CommandText = sql.ToString()

                    'バインド変数
                    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_0", OracleDbType.NVarchar2, NonAssign)
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_1", OracleDbType.NVarchar2, AssignWait)
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_9", OracleDbType.NVarchar2, AssignHolding)
                    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
                    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                    query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                    query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)
                    '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                    query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, nowDate)

                    '検索結果返却
                    dt = query.GetData()
                Catch ex As OracleExceptionEx When ex.Number = 1013
                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                            , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
                                            , Me.GetType.ToString _
                                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                            , C_RET_DBTIMEOUT _
                                            , ex.Message))
                    Throw
                End Try

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_END _
                                    , dt.Rows.Count))
            Return dt

        End Function
        '2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 END

#End Region

    End Class

End Namespace

Partial Class SC3220101DataSet
End Class
