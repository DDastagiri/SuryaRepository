'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3220201DataSet.vb
'─────────────────────────────────────
'機能： 全体管理 データセット
'補足： 
'作成： 2013/02/28 TMEJ小澤	初版作成
'更新： 2013/05/01 TMEJ小澤	ITxxxx_TSL自主研緊急対応（サービス）2回目
'更新： 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新： 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
'更新； 2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する
'更新： 2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新： 2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新： 2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新：
'─────────────────────────────────────


Imports System.Text
Imports Oracle.DataAccess.Client
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Imports Toyota.eCRB.DMSLinkage.CompleteCheck.DataAccess.SC3220201DataSet

'2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003.IC3801003TableAdapter
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003.IC3801003DataSet

'2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

Namespace SC3220201DataSetTableAdapters
    Public Class SC3220201DataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' 最小予約ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MinReserveId As Long = -1

        ''' <summary>
        ''' SQL置換用文字列(REZID)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ReserveReplaceWord As String = "#RESERVE#"
        ''' <summary>
        ''' SQL置換用文字列(PREZID)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PreReserveReplaceWord As String = "#PRERESERVE#"


        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ''' <summary>
        ''' 基本型式検索WORD(ハイフン)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const WordHyphen As String = "-"

        ''' <summary>
        ''' 基本型式(ALL)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BaseTypeAll As String = "X"

        ''' <summary>
        ''' キャンセルフラグ(有効)
        ''' </summary>
        Private Const CancelFlagEffective As String = "0"

        ''' <summary>
        ''' 受付区分("0"：予約客)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AcceptanceTypeRez As String = "0"

        ''' <summary>
        ''' サービスステータス(未入庫)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusNoIn As String = "00"

        ''' <summary>
        ''' サービスステータス(キャンセル)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusCancel As String = "02"

        ''' <summary>
        ''' サービスステータス(検査中)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusInspection As String = "10"

        ''' <summary>
        ''' ストール利用ステータス(中断)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallStatusWait As String = "05"

        '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START

        ''' <summary>
        ''' ストール利用ステータス(着工指示待ち)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallStatusWorkOrderWait As String = "00"


        ''' <summary>
        ''' ストール利用ステータス(作業開始待ち)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallStatusStartWait As String = "01"

        '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        ''' <summary>
        ''' 洗車必要フラグ()
        ''' </summary>
        Private Const CarWash As String = "0"

        ''' <summary>
        ''' 振当てステータス（未振当て）
        ''' </summary>
        Private Const NonAssign As String = "0"

        ''' <summary>
        ''' 振当てステータス（受付待ち）
        ''' </summary>
        Private Const AssignWait As String = "1"

        ''' <summary>
        ''' 振当てステータス（HOLD）
        ''' </summary>
        Private Const AssignHold As String = "9"

        ''' <summary>
        ''' DB日付省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MinDate As String = "1900/01/01 00:00:00"

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        ''' <summary>
        ''' 表示アイコンフラグ("0"：OFF 非表示)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOff As String = "0"

        ''' <summary>
        ''' 表示アイコンフラグ("1"：ON 表示)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOn As String = "1"

        ''' <summary>
        ''' 振当てステータス（振当済み）
        ''' </summary>
        Private Const AssignFinish As String = "2"

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
        '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        ''' <summary>
        ''' 表示アイコンフラグ("2"：ON 表示)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOn2 As String = "2"
        '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

#End Region

#Region "メイン"

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        ' ''' <summary>
        ' ''' サービス来店実績取得
        ' ''' </summary>
        ' ''' <param name="inDealerCode">販売店コード</param>
        ' ''' <param name="inBranchCode">店舗コード</param>
        ' ''' <param name="inNowDate">現在日付</param>
        ' ''' <returns>サービス来店実績データセット</returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ' ''' </history>
        'Public Function GetVisitManagement(ByVal inDealerCode As String,
        '                                   ByVal inBranchCode As String,
        '                                   ByVal inNowDate As Date, _
        '                                   ByVal orderNoList As IC3801003DataSet.IC3801003NoDeliveryRODataTable) _
        '                                   As SC3220201ServiceVisitManagementDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inNowDate = {4}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inDealerCode _
        '                , inBranchCode _
        '                , inNowDate.ToString(CultureInfo.CurrentCulture)))

        '    Dim dt As SC3220201ServiceVisitManagementDataTable

        '    Using query As New DBSelectQuery(Of SC3220201ServiceVisitManagementDataTable)("SC3220201_001")
        '        Dim sql As New StringBuilder

        '        Dim sqlOrderNo As New StringBuilder

        '        ' R/O番号用取得文字列
        '        If Not orderNoList Is Nothing _
        '            AndAlso 0 < orderNoList.Count Then

        '            sqlOrderNo.Append(" OR ORDERNO IN ( ")
        '            Dim count As Integer = 1
        '            Dim orderName As String

        '            For Each row As IC3801003DataSet.IC3801003NoDeliveryRORow In orderNoList.Rows
        '                ' SQL作成
        '                orderName = String.Format(CultureInfo.CurrentCulture, "ORDERNO{0}", count)
        '                If 1 < count Then
        '                    sqlOrderNo.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", orderName))
        '                Else
        '                    sqlOrderNo.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", orderName))
        '                End If

        '                ' パラメータ作成


        '                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue(orderName, OracleDbType.Char, row.ORDERNO)
        '                query.AddParameterWithTypeValue(orderName, OracleDbType.NVarchar2, row.ORDERNO.Trim)

        '                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


        '                count += 1
        '            Next
        '            sqlOrderNo.Append(" ) ")
        '        End If


        '        'SQL文作成
        '        With sql
        '            .Append("SELECT /* SC3220201_001 */ ")
        '            .Append("       VISITSEQ ")
        '            .Append("     , DLRCD ")
        '            .Append("     , STRCD ")
        '            .Append("     , NVL(VISITTIMESTAMP, :MINDATE) AS VISITTIMESTAMP ")
        '            .Append("     , VCLREGNO ")
        '            .Append("     , CUSTSEGMENT ")
        '            .Append("     , DMSID ")
        '            .Append("     , VIN ")
        '            .Append("     , MODELCODE ")
        '            .Append("     , NAME ")
        '            .Append("     , TELNO ")
        '            .Append("     , MOBILE ")
        '            .Append("     , SACODE ")
        '            .Append("     , ASSIGNSTATUS ")
        '            .Append("     , NVL(ASSIGNTIMESTAMP, :MINDATE) AS ASSIGNTIMESTAMP ")
        '            .Append("     , NVL(REZID, :MINREZID) AS REZID ")
        '            .Append("     , PARKINGCODE ")
        '            .Append("     , ORDERNO ")
        '            .Append("     , NVL(FREZID, :MINREZID) AS FREZID ")
        '            .Append("  FROM TBL_SERVICE_VISIT_MANAGEMENT ")
        '            .Append(" WHERE DLRCD  = :DLRCD ")
        '            .Append("   AND STRCD  = :STRCD ")

        '            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '            '.Append("   AND ASSIGNSTATUS = '2' ")
        '            .Append("   AND ASSIGNSTATUS = N'2' ")

        '            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '            .Append("   AND (TO_CHAR(:TODAY, 'YYYYMMDD') = TO_CHAR(VISITTIMESTAMP, 'YYYYMMDD') ")
        '            .Append(sqlOrderNo.ToString())
        '            .Append("   ) ")
        '        End With

        '        query.CommandText = sql.ToString()
        '        'バインド変数

        '        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '        'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
        '        'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inBranchCode)
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)

        '        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '        query.AddParameterWithTypeValue("TODAY", OracleDbType.Date, inNowDate)
        '        query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, DateTime.MinValue)
        '        query.AddParameterWithTypeValue("MINREZID", OracleDbType.Int64, MinReserveId)

        '        '検索結果返却
        '        dt = query.GetData()
        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} END OUT:COUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
        '    Return dt
        'End Function

        ' ''' <summary>
        ' ''' ストール予約取得
        ' ''' </summary>
        ' ''' <param name="inDealerCode">販売店コード</param>
        ' ''' <param name="inBranchCode">店舗コード</param>
        ' ''' <param name="inReserveIdList">初回予約IDリスト</param>
        ' ''' <returns>ストール予約データセット</returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ' ''' </history>
        'Public Function GetStallReserveInformation(ByVal inDealerCode As String, _
        '                                           ByVal inBranchCode As String, _
        '                                           ByVal inReserveIdList As SC3220201ServiceVisitManagementDataTable) As SC3220201StallRezinfoDataTable
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inDealerCode _
        '                , inBranchCode))

        '    Dim dt As SC3220201StallRezinfoDataTable
        '    Dim sql As New StringBuilder

        '    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '    'SQL文作成
        '    'With sql
        '    '    .Append("SELECT /* SC3220201_002 */ ")
        '    '    .Append("       T1.DLRCD ")                                                 ' 販売店コード
        '    '    .Append("     , T1.STRCD ")                                                 ' 店舗コード
        '    '    .Append("     , T1.REZID")                                                  ' 予約ID
        '    '    .Append("     , NVL(T1.PREZID, T1.REZID) AS PREZID ")                       ' 管理予約ID
        '    '    .Append("     , NVL(T1.STARTTIME, :MINDATE) AS STARTTIME ")                 ' 使用開始日時
        '    '    .Append("     , NVL(T1.ENDTIME, :MINDATE) AS ENDTIME ")                     ' 使用終了日時 (作業終了予定時刻)
        '    '    .Append("     , T1.CUSTCD ")                                                ' 顧客コード
        '    '    .Append("     , T1.CUSTOMERNAME ")                                          ' 氏名
        '    '    .Append("     , T1.TELNO ")                                                 ' 電話番号
        '    '    .Append("     , T1.MOBILE ")                                                ' 携帯番号
        '    '    .Append("     , T1.VEHICLENAME ")                                           ' 車名
        '    '    .Append("     , T1.VCLREGNO ")                                              ' 登録ナンバー
        '    '    .Append("     , T1.VIN ")                                                   ' VIN
        '    '    .Append("     , T1.MNTNCD AS MERCHANDISECD ")                               ' 整備コード
        '    '    .Append("      ,(CASE")
        '    '    .Append("            WHEN T3.MAINTECD IS NOT NULL THEN T3.MAINTENM ")       ' 整備名称 (代表入庫項目)
        '    '    .Append("            ELSE T4.MAINTENM ")
        '    '    .Append("        END) AS MERCHANDISENAME ")
        '    '    .Append("     , T1.MODELCODE ")                                             ' モデル
        '    '    .Append("     , NVL(T1.MILEAGE, -1) AS MILEAGE ")                           ' 走行距離
        '    '    .Append("     , NVL(T1.WASHFLG, '0') AS WASHFLG ")                          ' 洗車有無
        '    '    .Append("     , T1.WALKIN ")                                                ' 来店フラグ
        '    '    .Append("     , T1.REZ_DELI_DATE ")                                         ' 予約_納車_希望日時時刻 (納車予定日時)
        '    '    .Append("     , NVL(T1.ACTUAL_STIME, :MINDATE) AS ACTUAL_STIME ")           ' 作業開始日時
        '    '    .Append("     , NVL(T1.ACTUAL_ETIME, :MINDATE) AS ACTUAL_ETIME ")           ' 作業終了日時
        '    '    .Append("  FROM TBL_STALLREZINFO T1 ")
        '    '    .Append("     , TBLORG_MAINTEMASTER T3 ")
        '    '    .Append("     , TBLORG_MAINTEMASTER T4 ")
        '    '    .Append(" WHERE T1.DLRCD = T3.DLRCD  (+) ")
        '    '    .Append("   AND T1.MNTNCD = T3.MAINTECD (+) ")
        '    '    .Append("   AND :BASETYPEALL = T3.BASETYPE (+) ")
        '    '    .Append("   AND T1.DLRCD = T4.DLRCD (+) ")
        '    '    .Append("   AND T1.MNTNCD = T4.MAINTECD (+) ")
        '    '    .Append("   AND T1.MODELCODE = T4.BASETYPE (+) ")
        '    '    .Append("   AND T1.DLRCD = :DLRCD ")
        '    '    .Append("   AND T1.STRCD = :STRCD ")
        '    '    .Append(ReserveReplaceWord)
        '    '    .Append(PreReserveReplaceWord)
        '    '    .Append("   AND NOT EXISTS ( SELECT 1 ")
        '    '    .Append("                      FROM TBL_STALLREZINFO T2 ")
        '    '    .Append("                     WHERE T2.DLRCD = T1.DLRCD ")
        '    '    .Append("                       AND T2.STRCD = T1.STRCD ")
        '    '    .Append("                       AND T2.REZID = T1.REZID ")
        '    '    .Append("                       AND ( (T2.STOPFLG = :STOPFLG0 ")
        '    '    .Append("                       AND T2.CANCELFLG = :CANCELFLG1) ")
        '    '    .Append("                        OR T2.REZCHILDNO IN ( :CHILDNOLEAVE, :CHILDNODELIVERY ) ) ) ")

        '    'End With

        'With sql
        '    .AppendLine(" SELECT /* SC3220201_002 */ ")
        '    .AppendLine("        M1.DLR_CD AS DLRCD ")
        '    .AppendLine("       ,M1.BRN_CD AS STRCD ")
        '    .AppendLine("       ,M1.SVCIN_ID AS REZID ")
        '    .AppendLine("       ,M1.SVCIN_ID AS PREZID ")
        '    .AppendLine("       ,TO_CHAR(M1.CST_ID) AS CUSTCD ")
        '    .AppendLine("       ,NVL(TRIM(M1.CARWASH_NEED_FLG), :CARWASH_NEED_FLG_0) AS WASHFLG ")
        '    .AppendLine("       ,DECODE(M1.SCHE_DELI_DATETIME, :MINDATE, NULL, TO_CHAR(M1.SCHE_DELI_DATETIME, 'YYYYMMDDHH24MI')) AS REZ_DELI_DATE ")
        '    .AppendLine("       ,TRIM(M1.ACCEPTANCE_TYPE) AS WALKIN ")
        '    .AppendLine("       ,M1.SVCIN_MILE AS MILEAGE ")
        '    .AppendLine("       ,TO_CHAR(M1.MERC_ID) AS MERCHANDISECD ")
        '    .AppendLine("       ,DECODE(M1.SCHE_START_DATETIME, :MINDATE, :MIN, M1.SCHE_START_DATETIME) AS STARTTIME ")
        '    .AppendLine("       ,DECODE(M1.SCHE_END_DATETIME, :MINDATE, :MIN, M1.SCHE_END_DATETIME) AS ENDTIME ")
        '    .AppendLine("       ,DECODE(M1.RSLT_START_DATETIME, :MINDATE, :MIN, M1.RSLT_START_DATETIME) AS ACTUAL_STIME ")
        '    .AppendLine("       ,DECODE(M1.RSLT_END_DATETIME, :MINDATE, :MIN, M1.RSLT_END_DATETIME) AS ACTUAL_ETIME ")
        '    .AppendLine("       ,CASE WHEN M2.MAINTECD IS NOT NULL THEN ")
        '    .AppendLine("                  M2.MAINTENM ELSE ")
        '    .AppendLine("                  M3.MAINTENM END AS MERCHANDISENAME ")
        '    .AppendLine("       ,TRIM(M1.VCL_VIN) AS VIN ")
        '    .AppendLine("       ,TRIM(M1.MODEL_CD) AS MODELCODE ")
        '    .AppendLine("       ,TRIM(M1.REG_NUM) AS VCLREGNO ")
        '    .AppendLine("       ,TRIM(M1.MODEL_NAME) AS VEHICLENAME ")
        '    .AppendLine("       ,TRIM(M1.CST_NAME) AS CUSTOMERNAME ")
        '    .AppendLine("       ,TRIM(M1.CST_PHONE) AS TELNO ")
        '    .AppendLine("       ,TRIM(M1.CST_MOBILE) AS MOBILE ")
        '    .AppendLine("   FROM ")
        '    .AppendLine("       (SELECT T1.DLR_CD ")
        '    .AppendLine("              ,T1.BRN_CD ")
        '    .AppendLine("              ,T1.SVCIN_ID ")
        '    .AppendLine("              ,T1.CST_ID ")
        '    .AppendLine("              ,T1.CARWASH_NEED_FLG ")
        '    .AppendLine("              ,T1.SCHE_DELI_DATETIME ")
        '    .AppendLine("              ,T1.ACCEPTANCE_TYPE ")
        '    .AppendLine("              ,T1.SVCIN_MILE ")
        '    .AppendLine("              ,T2.MERC_ID ")
        '    .AppendLine("              ,T2.MAINTE_CD ")
        '    .AppendLine("              ,T3.SCHE_START_DATETIME ")
        '    .AppendLine("              ,T3.SCHE_END_DATETIME ")
        '    .AppendLine("              ,T3.RSLT_START_DATETIME ")
        '    .AppendLine("              ,T3.RSLT_END_DATETIME ")
        '    .AppendLine("              ,T4.VCL_VIN ")
        '    .AppendLine("              ,T4.MODEL_CD ")
        '    .AppendLine("              ,T4.VCL_KATASHIKI ")
        '    .AppendLine("              ,T5.REG_NUM ")
        '    .AppendLine("              ,T6.MODEL_NAME ")
        '    .AppendLine("              ,T7.CST_NAME ")
        '    .AppendLine("              ,T7.CST_PHONE ")
        '    .AppendLine("              ,T7.CST_MOBILE ")
        '    .AppendLine("          FROM TB_T_SERVICEIN T1 ")
        '    .AppendLine("              ,TB_T_JOB_DTL T2 ")
        '    .AppendLine("              ,TB_T_STALL_USE T3 ")
        '    .AppendLine("              ,TB_M_VEHICLE T4 ")
        '    .AppendLine("              ,TB_M_VEHICLE_DLR T5 ")
        '    .AppendLine("              ,TB_M_MODEL T6 ")
        '    .AppendLine("              ,TB_M_CUSTOMER T7 ")
        '    .AppendLine("         WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
        '    .AppendLine("           AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
        '    .AppendLine("           AND T1.VCL_ID = T4.VCL_ID ")
        '    .AppendLine("           AND T1.DLR_CD = T5.DLR_CD ")
        '    .AppendLine("           AND T1.VCL_ID = T5.VCL_ID ")
        '    .AppendLine("           AND T4.MODEL_CD = T6.MODEL_CD(+) ")
        '    .AppendLine("           AND T1.CST_ID = T7.CST_ID ")
        '    .AppendLine(ReserveReplaceWord)
        '    .AppendLine("           AND T1.SVC_STATUS <> :SVC_STATUS_02 ")
        '    .AppendLine("           AND T2.DLR_CD = :DLR_CD ")
        '    .AppendLine("           AND T2.BRN_CD = :BRN_CD ")
        '    .AppendLine("           AND T2.CANCEL_FLG = :CANCEL_FLG_0) M1 ")
        '    .AppendLine("       ,TBLORG_MAINTEMASTER M2 ")
        '    .AppendLine("       ,TBLORG_MAINTEMASTER M3 ")
        '    .AppendLine("  WHERE ")
        '    .AppendLine("        M1.DLR_CD = M2.DLRCD(+) ")
        '    .AppendLine("    AND M1.MAINTE_CD = M2.MAINTECD(+) ")
        '    .AppendLine("    AND :MAINTE_KATASHIKI = M2.BASETYPE(+) ")
        '    .AppendLine("    AND M1.DLR_CD = M3.DLRCD(+) ")
        '    .AppendLine("    AND M1.MAINTE_CD = M3.MAINTECD(+) ")
        '    .AppendLine("    AND SUBSTR(M1.VCL_KATASHIKI, 0, INSTR(M1.VCL_KATASHIKI, :KATASHIKI) - 1) = M3.BASETYPE(+) ")
        '    End With

        '    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '    Using query As New DBSelectQuery(Of SC3220201StallRezinfoDataTable)("SC3220201_002")
        '        Dim sqlReserveId As New StringBuilder
        '        Dim sqlPreReserveId As New StringBuilder

        '        ' 初回予約ID用取得文字列
        '        If Not inReserveIdList Is Nothing Then
        '            If inReserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID").Count > 0 Then

        '                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'sqlReserveId.Append(" AND (T1.REZID IN ( ")
        '                sqlReserveId.Append(" AND T1.SVCIN_ID IN ( ")

        '                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                sqlPreReserveId.Append(" OR T1.PREZID IN ( ")

        '                Dim count As Long = 1
        '                Dim reserveIdName As String
        '                Dim preReserveIdName As String
        '                For Each row As SC3220201ServiceVisitManagementRow In inReserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID")
        '                    ' SQL作成
        '                    reserveIdName = String.Format(CultureInfo.CurrentCulture, "REZID{0}", count)
        '                    preReserveIdName = String.Format(CultureInfo.CurrentCulture, "PREZID{0}", count)
        '                    If count > 1 Then
        '                        sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", reserveIdName))
        '                        sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", preReserveIdName))
        '                    Else
        '                        sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", reserveIdName))
        '                        sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", preReserveIdName))
        '                    End If

        '                    ' パラメータ作成
        '                    query.AddParameterWithTypeValue(reserveIdName, OracleDbType.Int64, row.FREZID)


        '                    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                    'query.AddParameterWithTypeValue(preReserveIdName, OracleDbType.Int64, row.FREZID)

        '                    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


        '                    count += 1
        '                Next
        '                sqlReserveId.Append(" ) ")
        '                sqlPreReserveId.Append(" )) ")
        '            End If
        '        End If

        '        If String.IsNullOrEmpty(sqlReserveId.ToString()) Then

        '            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '            'sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND T1.REZID = {0} ", MinReserveId))
        '            sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND T1.SVCIN_ID = {0} ", MinReserveId))

        '            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
        '        End If
        '        'SQL置換
        '        sql.Replace(ReserveReplaceWord, sqlReserveId.ToString)


        '        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '        'sql.Replace(PreReserveReplaceWord, sqlPreReserveId.ToString)

        '        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


        '        query.CommandText = sql.ToString()


        '        'バインド変数
        '        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START 

        '        'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
        '        'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inBranchCode)
        '        'query.AddParameterWithTypeValue("STOPFLG0", OracleDbType.Char, "0")
        '        'query.AddParameterWithTypeValue("CANCELFLG1", OracleDbType.Char, "1")
        '        'query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, DateTime.MinValue)
        '        'query.AddParameterWithTypeValue("CHILDNOLEAVE", OracleDbType.Int64, 0)         ' 子予約連番-0:引取
        '        'query.AddParameterWithTypeValue("CHILDNODELIVERY", OracleDbType.Int64, 999)    ' 子予約連番-999:納車
        '        'query.AddParameterWithTypeValue("BASETYPEALL", OracleDbType.NVarchar2, Me.BaseTypeAll(inDealerCode))    ' BASETYPE「*」

        '        query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
        '        query.AddParameterWithTypeValue("MIN", OracleDbType.Date, Date.MinValue)
        '        query.AddParameterWithTypeValue("CARWASH_NEED_FLG_0", OracleDbType.NVarchar2, CarWash)
        '        query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, StatusCancel)
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
        '        query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelFlagEffective)
        '        query.AddParameterWithTypeValue("MAINTE_KATASHIKI", OracleDbType.NVarchar2, BaseTypeAll)    ' BASETYPE「X」
        '        query.AddParameterWithTypeValue("KATASHIKI", OracleDbType.NVarchar2, WordHyphen)

        '        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '        '検索結果返却
        '        dt = query.GetData()
        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} END OUT:COUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
        '    Return dt
        'End Function

        ' ''' <summary>
        ' ''' ストール実績取得
        ' ''' </summary>
        ' ''' <param name="inDealerCode">販売店コード</param>
        ' ''' <param name="inBranchCode">初回予約IDリスト</param>
        ' ''' <param name="inReserveIdList">初回予約IDリスト</param>
        ' ''' <returns>ストール実績データセット</returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ' ''' </history>
        'Public Function GetStallProcess(ByVal inDealerCode As String, _
        '                                ByVal inBranchCode As String, _
        '                                ByVal inReserveIdList As SC3220201ServiceVisitManagementDataTable) As SC3220201StallProcessDataTable
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inDealerCode _
        '                , inBranchCode))

        '    Dim dt As SC3220201StallProcessDataTable

        '    Dim sql As New StringBuilder
        '    'SQL文作成

        '    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '    'With sql
        '    '    .Append(" SELECT /* SC3220201_003 */ ")
        '    '    .Append("        MAX(DLRCD)                            AS DLRCD ")
        '    '    .Append("      , MAX(STRCD)                            AS STRCD ")
        '    '    .Append("      , PREZID                                AS PREZID ")
        '    '    .Append("      , MAX(WASHFLG)                          AS WASHFLG ")
        '    '    .Append("      , MAX(DECODE(NUM,1,RESULT_STATUS,NULL)) AS RESULT_STATUS ")
        '    '    .Append("      , MAX(REZ_END_TIME)                     AS REZ_END_TIME ")
        '    '    .Append("      , MAX(RESULT_WASH_START)                AS RESULT_WASH_START ")
        '    '    .Append("      , MAX(RESULT_WASH_END)                  AS RESULT_WASH_END ")
        '    '    .Append("      , MAX(DECODE(NUM,1,STAFFCD,NULL))       AS STAFFCD ")
        '    '    .Append("      , MAX(DECODE(NUM,1,USERNAME,NULL))      AS STAFFNAME ")
        '    '    .Append("      , SUM(DECODE(NVL(STOPFLG,0),0,0,1))     AS UNVALID_REZ_COUNT ")
        '    '    .Append("      , MIN(ACTUAL_STIME)                     AS FIRST_STARTTIME ")
        '    '    .Append("      , MAX(NVL2(RESULT_END_TIME ")
        '    '    .Append("               , TO_DATE(RESULT_END_TIME,'YYYYMMDDHH24MI') ")
        '    '    .Append("               , ENDTIME))                    AS LAST_ENDTIME ")
        '    '    .Append("      , SUM(DECODE(RESULT_STATUS,'0' ,REZ_WORK_TIME, ")
        '    '    .Append("            DECODE(RESULT_STATUS,'00',REZ_WORK_TIME, ")
        '    '    .Append("            DECODE(RESULT_STATUS,'10',REZ_WORK_TIME, ")
        '    '    .Append("            DECODE(RESULT_STATUS,NULL,REZ_WORK_TIME,0))))) AS WORK_TIME ")
        '    '    .Append("      , DECODE(MIN(NUM),0,'0','1')                         AS RESULT_TYPE ")
        '    '    .Append("   FROM ")
        '    '    .Append("     (SELECT ")
        '    '    .Append("             R.DLRCD ")
        '    '    .Append("           , R.STRCD ")
        '    '    .Append("           , R.REZID ")
        '    '    .Append("           , P.SEQNO ")
        '    '    .Append("           , P.DSEQNO ")
        '    '    .Append("           , R.WASHFLG ")
        '    '    .Append("           , P.RESULT_STATUS ")
        '    '    .Append("           , P.REZ_END_TIME ")
        '    '    .Append("           , P.RESULT_WASH_START ")
        '    '    .Append("           , P.RESULT_WASH_END ")
        '    '    .Append("           , NVL(R.PREZID,R.REZID) AS PREZID ")
        '    '    .Append("           , T3.STAFFCD ")
        '    '    .Append("           , T5.USERNAME ")
        '    '    .Append("           , R.REZ_WORK_TIME ")
        '    '    .Append("           , R.STOPFLG ")
        '    '    .Append("           , R.ENDTIME ")
        '    '    .Append("           , P.RESULT_END_TIME ")
        '    '    .Append("           , R.ACTUAL_STIME ")
        '    '    .Append("           , ROW_NUMBER() OVER ( ")
        '    '    .Append("                 PARTITION BY NVL(R.PREZID, R.REZID) ")
        '    '    .Append("                 ORDER BY ENDTIME DESC ")
        '    '    .Append("                        , STARTTIME DESC ")
        '    '    .Append("                        , USERNAME ASC) NUM ")
        '    '    .Append("        FROM TBL_STALLPROCESS P ")
        '    '    .Append("           , TBL_STALLREZINFO R ")
        '    '    .Append("           , TBL_TSTAFFSTALL T3 ")
        '    '    .Append("           , TBL_SSTAFF T4 ")
        '    '    .Append("           , TBL_USERS T5 ")
        '    '    .Append("       WHERE R.DLRCD = P.DLRCD ")
        '    '    .Append("         AND R.STRCD = P.STRCD ")
        '    '    .Append("         AND R.REZID = P.REZID ")
        '    '    .Append("         AND P.DLRCD = T3.DLRCD (+) ")
        '    '    .Append("         AND P.STRCD = T3.STRCD (+) ")
        '    '    .Append("         AND P.REZID = T3.REZID (+) ")
        '    '    .Append("         AND P.SEQNO = T3.SEQNO (+) ")
        '    '    .Append("         AND P.DSEQNO = T3.DSEQNO (+) ")
        '    '    .Append("         AND T3.DLRCD = T4.DLRCD (+) ")
        '    '    .Append("         AND T3.STRCD = T4.STRCD (+) ")
        '    '    .Append("         AND T3.STAFFCD = T4.STAFFCD (+) ")
        '    '    .Append("         AND T4.DLRCD = T5.DLRCD (+) ")
        '    '    .Append("         AND T4.STRCD = T5.STRCD (+) ")
        '    '    .Append("         AND T4.ACCOUNT = T5.ACCOUNT (+) ")
        '    '    .Append("         AND R.DLRCD = :DLRCD ")
        '    '    .Append("         AND R.STRCD = :STRCD ")
        '    '    .Append(ReserveReplaceWord)
        '    '    .Append(PreReserveReplaceWord)
        '    '    .Append("         AND DECODE(R.STOPFLG,'0',DECODE(R.CANCELFLG,'1',1,0),0) + ")
        '    '    .Append("             DECODE(R.REZCHILDNO,0,1,0) + ")
        '    '    .Append("             DECODE(R.REZCHILDNO,999,1,0) = 0 ")
        '    '    .Append("         AND P.SEQNO = ( ")
        '    '    .Append("             SELECT MAX(SEQNO) ")
        '    '    .Append("               FROM TBL_STALLPROCESS ")
        '    '    .Append("              WHERE P.DLRCD = DLRCD ")
        '    '    .Append("                AND P.STRCD = STRCD ")
        '    '    .Append("                AND P.REZID = REZID ")
        '    '    .Append("                AND P.DSEQNO = DSEQNO) ")
        '    '    .Append("         AND P.DSEQNO = ( ")
        '    '    .Append("             SELECT MAX(DSEQNO) ")
        '    '    .Append("               FROM TBL_STALLPROCESS ")
        '    '    .Append("              WHERE P.DLRCD = DLRCD ")
        '    '    .Append("                AND P.STRCD = STRCD ")
        '    '    .Append("                AND P.REZID = REZID) ")
        '    '    .Append("       UNION ALL ")
        '    '    .Append("      SELECT  ")
        '    '    .Append("             R.DLRCD ")
        '    '    .Append("           , R.STRCD ")
        '    '    .Append("           , R.REZID ")
        '    '    .Append("           , NULL ")
        '    '    .Append("           , NULL ")
        '    '    .Append("           , R.WASHFLG ")
        '    '    .Append("           , NULL ")
        '    '    .Append("           , NULL ")
        '    '    .Append("           , NULL ")
        '    '    .Append("           , NULL ")
        '    '    .Append("           , NVL(R.PREZID,R.REZID) AS PREZID ")
        '    '    .Append("           , NULL ")
        '    '    .Append("           , NULL ")
        '    '    .Append("           , R.REZ_WORK_TIME ")
        '    '    .Append("           , R.STOPFLG ")
        '    '    .Append("           , R.ENDTIME ")
        '    '    .Append("           , NULL ")
        '    '    .Append("           , R.ACTUAL_STIME ")
        '    '    .Append("           , 0 AS NUM ")
        '    '    .Append("        FROM TBL_STALLREZINFO R ")
        '    '    .Append("       WHERE R.DLRCD = :DLRCD ")
        '    '    .Append("         AND R.STRCD = :STRCD ")
        '    '    .Append(ReserveReplaceWord)
        '    '    .Append(PreReserveReplaceWord)
        '    '    .Append("         AND DECODE(R.STOPFLG,'0',DECODE(R.CANCELFLG,'1',1,0),0) + ")
        '    '    .Append("             DECODE(R.REZCHILDNO,0,1,0) + ")
        '    '    .Append("             DECODE(R.REZCHILDNO,999,1,0) = 0 ")
        '    '    .Append("         AND NOT EXISTS ( ")
        '    '    .Append("                 SELECT '1' ")
        '    '    .Append("                   FROM TBL_STALLPROCESS ")
        '    '    .Append("                  WHERE R.DLRCD = DLRCD ")
        '    '    .Append("                    AND R.STRCD = STRCD ")
        '    '    .Append("                    AND R.REZID = REZID)  ")
        '    '    .Append("     ) M ")
        '    '    .Append(" GROUP BY M.PREZID ")
        '    'End With


        '    With sql
        '        .AppendLine("  SELECT /* SC3220201_003 */ ")
        '        .AppendLine("         T1.SVCIN_ID AS PREZID ")
        '        .AppendLine("        ,MAX(T1.DLR_CD) AS DLRCD ")
        '        .AppendLine("        ,MAX(T1.BRN_CD) AS STRCD ")
        '        .AppendLine("        ,MAX(NVL(TRIM(T1.CARWASH_NEED_FLG), :CARWASH_NEED_FLG_0)) AS WASHFLG ")
        '        .AppendLine("        ,MAX(TRIM(T1.SVC_STATUS)) AS RESULT_STATUS ")
        '        .AppendLine("        ,DECODE(MAX(T1.SCHE_END_DATETIME), :MINDATE, NULL, TO_CHAR(MAX(T1.SCHE_END_DATETIME), 'YYYYMMDDHH24MI')) AS REZ_END_TIME ")
        '        .AppendLine("        ,MAX(DECODE(T1.NUM, 1, TRIM(T1.STF_CD), NULL)) AS STAFFCD ")
        '        .AppendLine("        ,MAX(DECODE(T1.NUM, 1, TRIM(T1.STF_NAME), NULL)) STAFFNAME ")
        '        .AppendLine("        ,SUM(DECODE(T1.STALL_USE_STATUS, :STALL_USE_STATUS_05, 1, 0)) AS UNVALID_REZ_COUNT ")
        '        .AppendLine("        ,MIN(DECODE(T1.RSLT_START_DATETIME, :MINDATE, TO_DATE(NULL), T1.RSLT_START_DATETIME)) AS FIRST_STARTTIME ")
        '        .AppendLine("        ,MAX(DECODE(T1.RSLT_END_DATETIME, :MINDATE, T1.SCHE_END_DATETIME, T1.RSLT_END_DATETIME)) AS LAST_ENDTIME ")
        '        .AppendLine("        ,SUM(DECODE(T1.SVC_STATUS, :SVC_STATUS_00, T1.SCHE_WORKTIME, ")
        '        .AppendLine("             DECODE(T1.SVC_STATUS, :SVC_STATUS_10, T1.SCHE_WORKTIME, 0))) AS WORK_TIME ")
        '        .AppendLine("        ,MAX(DECODE(T1.RSLT_START_DATETIME_WASH, :MINDATE, TO_DATE(NULL), T1.RSLT_START_DATETIME_WASH)) AS RESULT_WASH_START ")
        '        .AppendLine("        ,MAX(DECODE(T1.RSLT_END_DATETIME_WASH, :MINDATE, TO_DATE(NULL), T1.RSLT_END_DATETIME_WASH)) AS RESULT_WASH_END ")
        '        .AppendLine("        ,MAX(DECODE(T1.RSLT_START_DATETIME, :MINDATE, 0, 1)) AS RESULT_TYPE ")
        '        .AppendLine("    FROM ")
        '        .AppendLine("        (SELECT M1.DLR_CD ")
        '        .AppendLine("               ,M1.BRN_CD ")
        '        .AppendLine("               ,M1.SVCIN_ID ")
        '        .AppendLine("               ,M1.CARWASH_NEED_FLG ")
        '        .AppendLine("               ,M1.SVC_STATUS ")
        '        .AppendLine("               ,M3.SCHE_END_DATETIME ")
        '        .AppendLine("               ,M3.JOB_ID ")
        '        .AppendLine("               ,M3.SCHE_WORKTIME ")
        '        .AppendLine("               ,M3.RSLT_START_DATETIME ")
        '        .AppendLine("               ,M3.RSLT_END_DATETIME ")
        '        .AppendLine("               ,M3.STALL_USE_STATUS ")
        '        .AppendLine("               ,M5.STF_CD ")
        '        .AppendLine("               ,M6.STF_NAME ")
        '        .AppendLine("               ,M4.RSLT_START_DATETIME AS RSLT_START_DATETIME_WASH ")
        '        .AppendLine("               ,M4.RSLT_END_DATETIME AS RSLT_END_DATETIME_WASH ")
        '        .AppendLine("               ,ROW_NUMBER() OVER ( ")
        '        .AppendLine("                             PARTITION BY M1.SVCIN_ID ")
        '        .AppendLine("                                 ORDER BY M3.SCHE_END_DATETIME DESC ")
        '        .AppendLine("                                         ,M3.SCHE_START_DATETIME DESC ")
        '        .AppendLine("                                         ,M6.STF_NAME ASC) NUM ")
        '        .AppendLine("          FROM TB_T_SERVICEIN M1 ")
        '        .AppendLine("              ,TB_T_JOB_DTL M2 ")
        '        .AppendLine("              ,TB_T_STALL_USE M3 ")
        '        .AppendLine("              ,TB_T_CARWASH_RESULT M4 ")
        '        .AppendLine("              ,TB_T_STAFF_JOB M5 ")
        '        .AppendLine("              ,TB_M_STAFF M6 ")
        '        .AppendLine("         WHERE M1.SVCIN_ID = M2.SVCIN_ID ")
        '        .AppendLine("           AND M2.JOB_DTL_ID = M3.JOB_DTL_ID ")
        '        .AppendLine("           AND M1.SVCIN_ID = M4.SVCIN_ID(+) ")
        '        .AppendLine("           AND M3.JOB_ID = M5.JOB_ID(+) ")
        '        .AppendLine("           AND M5.STF_CD = M6.STF_CD(+) ")
        '        .AppendLine(ReserveReplaceWord)
        '        .AppendLine("           AND M1.SVC_STATUS <> :SVC_STATUS_02 ")
        '        .AppendLine("           AND M2.CANCEL_FLG = :CANCEL_FLG_0 ")
        '        .AppendLine("           AND M2.DLR_CD = :DLR_CD ")
        '        .AppendLine("           AND M2.BRN_CD = :BRN_CD) T1 ")
        '        .AppendLine("   GROUP BY T1.SVCIN_ID ")
        '    End With


        '    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '    Using query As New DBSelectQuery(Of SC3220201StallProcessDataTable)("SC3220201_003")
        '        Dim sqlReserveId As New StringBuilder
        '        Dim sqlPreReserveId As New StringBuilder

        '        ' 初回予約ID用取得文字列
        '        If Not inReserveIdList Is Nothing Then
        '            If inReserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID").Count > 0 Then

        '                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'sqlReserveId.Append(" AND (R.REZID IN ( ")
        '                sqlReserveId.Append(" AND M1.SVCIN_ID IN ( ")

        '                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                sqlPreReserveId.Append(" OR R.PREZID IN ( ")
        '                Dim count As Long = 1
        '                Dim reserveIdName As String
        '                Dim preReserveIdName As String
        '                For Each row As SC3220201ServiceVisitManagementRow In inReserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID")
        '                    ' SQL作成
        '                    reserveIdName = String.Format(CultureInfo.CurrentCulture, "REZID{0}", count)
        '                    preReserveIdName = String.Format(CultureInfo.CurrentCulture, "PREZID{0}", count)
        '                    If count > 1 Then
        '                        sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", reserveIdName))
        '                        sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", preReserveIdName))
        '                    Else
        '                        sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", reserveIdName))
        '                        sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", preReserveIdName))
        '                    End If

        '                    ' パラメータ作成
        '                    query.AddParameterWithTypeValue(reserveIdName, OracleDbType.Int64, row.FREZID)


        '                    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                    'query.AddParameterWithTypeValue(preReserveIdName, OracleDbType.Int64, row.FREZID)

        '                    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                    count += 1
        '                Next
        '                sqlReserveId.Append(" ) ")
        '                sqlPreReserveId.Append(" )) ")
        '            End If
        '        End If

        '        If String.IsNullOrEmpty(sqlReserveId.ToString()) Then

        '            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '            'sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND R.REZID = {0} ", MinReserveId))
        '            sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND M1.SVCIN_ID = {0} ", MinReserveId))

        '            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '        End If

        '        'SQL置換
        '        sql.Replace(ReserveReplaceWord, sqlReserveId.ToString)


        '        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '        'sql.Replace(PreReserveReplaceWord, sqlPreReserveId.ToString)

        '        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END



        '        query.CommandText = sql.ToString()

        '        'バインド変数

        '        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '        'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
        '        'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inBranchCode)

        '        query.AddParameterWithTypeValue("CARWASH_NEED_FLG_0", OracleDbType.NVarchar2, CarWash)
        '        query.AddParameterWithTypeValue("STALL_USE_STATUS_05", OracleDbType.NVarchar2, StallStatusWait)
        '        query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, StatusNoIn)
        '        query.AddParameterWithTypeValue("SVC_STATUS_10", OracleDbType.NVarchar2, StatusInspection)
        '        query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
        '        query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, StatusCancel)
        '        query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelFlagEffective)
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)

        '        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '        '検索結果返却
        '        dt = query.GetData()
        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} END OUT:COUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
        '    Return dt
        'End Function

        ''' <summary>
        ''' サービス来店実績取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inNowDate">現在日付</param>
        ''' <returns>サービス来店実績データセット</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発
        ''' </history>
        Public Function GetVisitManagement(ByVal inDealerCode As String,
                                           ByVal inBranchCode As String,
                                           ByVal inNowDate As Date) _
                                           As SC3220201ServiceVisitManagementDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inNowDate = {4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode _
                        , inBranchCode _
                        , inNowDate.ToString(CultureInfo.CurrentCulture)))

            Dim dt As SC3220201ServiceVisitManagementDataTable

            Using query As New DBSelectQuery(Of SC3220201ServiceVisitManagementDataTable)("SC3220201_001")
                Dim sql As New StringBuilder


                'SQL文作成
                With sql

                    .AppendLine("   SELECT /* SC3220201_001 */ ")
                    .AppendLine("          V.VISITSEQ              AS VISITSEQ ")
                    .AppendLine("        , MAX(V.DLRCD)            AS DLRCD ")
                    .AppendLine("        , MAX(V.STRCD)            AS STRCD ")
                    .AppendLine("        , MAX(V.VISITTIMESTAMP)   AS VISITTIMESTAMP ")
                    '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                    '.AppendLine("        , MAX(V.VCLREGNO)         AS VCLREGNO ")
                    .AppendLine("        , NVL(MAX(TRIM(VD.REG_NUM)), MAX(V.VCLREGNO))  AS VCLREGNO ")
                    '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                    .AppendLine("        , MAX(V.NAME)             AS NAME  ")
                    .AppendLine("        , MAX(V.ORDERNO)          AS ORDERNO ")
                    .AppendLine("        , MAX(NVL(V.FREZID, -1))           AS FREZID ")
                    .AppendLine("        , MAX(V.ASSIGNSTATUS)     AS ASSIGNSTATUS ")
                    .AppendLine("        , MAX(V.ASSIGNTIMESTAMP)  AS ASSIGNTIMESTAMP ")
                    .AppendLine("        , MAX(V.CUSTSEGMENT)      AS CUSTSEGMENT ")
                    .AppendLine("        , MAX(CASE ")
                    .AppendLine("                   WHEN 0 < V.FREZID ")
                    .AppendLine("                   THEN :ICON_FLAG_ON ")
                    .AppendLine("                   ELSE :ICON_FLAG_OFF ")
                    .AppendLine("               END )              AS REZ_MARK  ")
                    .AppendLine("        , MAX(R.VISIT_ID)        AS VISIT_ID ")
                    .AppendLine("        , MAX(NVL(R2.RO_SEQ, -1)) AS RO_SEQ ")
                    .AppendLine("        , SUBSTR((TO_CHAR(MAX(R2.RO_SEQ))), -1, 1) AS MAX_RO_SEQ ")
                    .AppendLine("        , MIN(NVL(TRIM(R2.RO_STATUS), TRIM(R.RO_STATUS))) AS MIN_RO_STATUS ")
                    .AppendLine("        , MAX(R.RO_STATUS)        AS MAX_RO_STATUS ")
                    .AppendLine("        , MAX(CASE ")

                    '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 START

                    '.AppendLine("                   WHEN VD.VIP_FLG = :ICON_FLAG_ON THEN VD.VIP_FLG ")

                    .AppendLine("                   WHEN VD.IMP_VCL_FLG = :ICON_FLAG_ON THEN VD.IMP_VCL_FLG ")

                    '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 END

                    '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                    .AppendLine("                  WHEN VD.IMP_VCL_FLG = :ICON_FLAG_ON_2 THEN VD.IMP_VCL_FLG ")
                    '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                    .AppendLine("                   ELSE :ICON_FLAG_OFF  ")
                    .AppendLine("               END )              AS JDP_MARK ")
                    '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                    .AppendLine("        , MAX(CASE ")
                    .AppendLine("                   WHEN VCL.SPECIAL_CAMPAIGN_TGT_FLG = :ICON_FLAG_ON THEN VCL.SPECIAL_CAMPAIGN_TGT_FLG ")
                    .AppendLine("                   ELSE :ICON_FLAG_OFF  ")
                    .AppendLine("               END )              AS SSC_MARK ")
                    '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                    '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                    .AppendLine("        , MAX(NVL(TRIM(LV.SML_AMC_FLG), :ICON_FLAG_OFF)) AS SML_AMC_FLG ")
                    .AppendLine("        , MAX(NVL(TRIM(LV.EW_FLG), :ICON_FLAG_OFF)) AS EW_FLG ")
                    .AppendLine("        , MAX(CASE ")
                    .AppendLine("                   WHEN TLM.VCL_VIN IS NOT NULL THEN :ICON_FLAG_ON ")
                    .AppendLine("                   ELSE :ICON_FLAG_OFF ")
                    .AppendLine("              END )                AS TLM_MBR_FLG ")
                    '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                    .AppendLine("     FROM TBL_SERVICE_VISIT_MANAGEMENT V ")
                    .AppendLine("        , TB_T_RO_INFO R ")
                    .AppendLine("        , TB_T_RO_INFO R2 ")
                    .AppendLine("        , TB_M_VEHICLE_DLR VD ")
                    '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                    .AppendLine("        , TB_M_VEHICLE VCL ")
                    '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                    '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                    .AppendLine("        , TB_LM_VEHICLE LV ")
                    .AppendLine("        , TB_LM_TLM_MEMBER TLM ")
                    '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                    .AppendLine("    WHERE V.VISITSEQ = R.VISIT_ID(+) ")
                    .AppendLine("      AND V.VISITSEQ = R2.VISIT_ID(+) ")
                    .AppendLine("      AND V.VCL_ID = VD.VCL_ID(+) ")
                    '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                    .AppendLine("      AND V.VCL_ID = VCL.VCL_ID(+) ")
                    '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                    '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                    .AppendLine("      AND VCL.VCL_ID = LV.VCL_ID(+) ")
                    .AppendLine("      AND VCL.VCL_VIN = TLM.VCL_VIN(+) ")
                    '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                    .AppendLine("      AND V.DLRCD = :DLRCD ")
                    .AppendLine("      AND V.STRCD = :STRCD ")
                    .AppendLine("      AND V.ASSIGNSTATUS = :ASSIGNSTATUS_2 ")
                    .AppendLine("      AND (    ( R.RO_STATUS IN (:STATUS_50, :STATUS_60, :STATUS_80, :STATUS_85) ) ")
                    .AppendLine("            OR ( V.ORDERNO IS NULL AND V.VISITTIMESTAMP > TRUNC(:NOWDATE) ) ) ")
                    .AppendLine("      AND R.DLR_CD(+) = :DLRCD ")
                    .AppendLine("      AND R.BRN_CD(+) = :STRCD ")
                    .AppendLine("      AND R.RO_STATUS(+) <> :STATUS_99 ")
                    .AppendLine("      AND R2.DLR_CD(+) = :DLRCD ")
                    .AppendLine("      AND R2.BRN_CD(+) = :STRCD ")
                    .AppendLine("      AND R2.RO_STATUS(+) <> :STATUS_99 ")
                    .AppendLine("      AND VD.DLR_CD(+) = :DLRCD ")
                    .AppendLine(" GROUP BY V.VISITSEQ ")

                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数

                '表示アイコンフラグ("0"：非表示)
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                '表示アイコンフラグ("1"：表示)
                query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                '表示アイコンフラグ("2"：L/B表示)
                query.AddParameterWithTypeValue("ICON_FLAG_ON_2", OracleDbType.NVarchar2, IconFlagOn2)
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                '店舗コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)
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
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)

                '検索結果返却
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return dt

        End Function

        ''' <summary>
        ''' 予約情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inReserveIdList">初回予約IDリスト</param>
        ''' <returns>ストール予約データセット</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
        ''' </history>
        Public Function GetStallReserveInformation(ByVal inDealerCode As String, _
                                                   ByVal inBranchCode As String, _
                                                   ByVal inReserveIdList As SC3220201ServiceVisitManagementDataTable) As SC3220201StallRezinfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode _
                        , inBranchCode))

            Dim dt As SC3220201StallRezinfoDataTable
            Dim sql As New StringBuilder

            With sql

                .AppendLine("  SELECT /* SC3220201_002 */ ")
                .AppendLine("       T1.SVCIN_ID AS PREZID ")
                .AppendLine("      ,MAX(CASE ")
                .AppendLine("                WHEN T1.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_0 THEN :ICON_FLAG_ON ")
                .AppendLine("                ELSE :ICON_FLAG_OFF ")
                .AppendLine("            END ) AS REZ_MARK ")
                .AppendLine("      ,MAX(CASE ")
                .AppendLine("                WHEN T1.SCHE_DELI_DATETIME = :MINDATE THEN :MIN ")
                .AppendLine("                WHEN T1.SCHE_DELI_DATETIME IS NULL THEN :MIN ")
                .AppendLine("                ELSE T1.SCHE_DELI_DATETIME ")
                .AppendLine("            END ) AS REZ_DELI_DATE ")
                .AppendLine("      ,MAX(NVL(TRIM(T1.CARWASH_NEED_FLG), :CARWASH_NEED_FLG_0)) AS WASHFLG ")
                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                '.AppendLine("      ,MAX(CASE  ")
                '.AppendLine("                WHEN T2.SCHE_END_DATETIME = :MINDATE THEN :MIN ")
                '.AppendLine("                WHEN T2.SCHE_END_DATETIME IS NULL THEN :MIN ")
                '.AppendLine("                ELSE T2.SCHE_END_DATETIME ")
                '.AppendLine("            END) AS ENDTIME ")
                .AppendLine("      ,MAX(CASE  ")
                .AppendLine("                WHEN T2.LAST_END_DATETIME = :MINDATE THEN :MIN ")
                .AppendLine("                WHEN T2.LAST_END_DATETIME IS NULL THEN :MIN ")
                .AppendLine("                ELSE T2.LAST_END_DATETIME ")
                .AppendLine("            END) AS ENDTIME ")
                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                .AppendLine("      ,MAX(CASE  ")
                .AppendLine("                WHEN T2.RSLT_START_DATETIME = :MINDATE THEN :MIN ")
                .AppendLine("                WHEN T2.RSLT_START_DATETIME IS NULL THEN :MIN ")
                .AppendLine("                ELSE T2.RSLT_START_DATETIME ")
                .AppendLine("            END) AS ACTUAL_STIME ")
                .AppendLine("      ,MAX(CASE  ")
                .AppendLine("                WHEN T3.RSLT_START_DATETIME = :MINDATE THEN :MIN ")
                .AppendLine("                WHEN T3.RSLT_START_DATETIME IS NULL THEN :MIN ")
                .AppendLine("                ELSE T3.RSLT_START_DATETIME ")
                .AppendLine("            END) AS WASH_START ")
                .AppendLine("      ,MAX(CASE  ")
                .AppendLine("                WHEN T3.RSLT_END_DATETIME = :MINDATE THEN :MIN ")
                .AppendLine("                WHEN T3.RSLT_END_DATETIME IS NULL THEN :MIN ")
                .AppendLine("                ELSE T3.RSLT_END_DATETIME ")
                .AppendLine("            END) AS WASH_END ")
                .AppendLine("      ,MAX(NVL(CONCAT(TRIM(T5.UPPER_DISP), TRIM(T5.LOWER_DISP)), NVL(T6.SVC_CLASS_NAME, T6.SVC_CLASS_NAME_ENG))) AS MERCHANDISENAME ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START
                .AppendLine("      ,MAX(SVC_STATUS) AS RESULT_STATUS ")
                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                .AppendLine("      ,MAX(T2.REMAINING_WORK_TIME) AS WORK_TIME ")
                .AppendLine("      ,MAX(T2.INSPECTION_STATUS) AS REMAINING_INSPECTION_TYPE ")
                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                .AppendLine(" FROM ")
                .AppendLine("      TB_T_SERVICEIN T1 ")
                .AppendLine("     ,(SELECT M1.SVCIN_ID ")
                .AppendLine("             ,MIN(M2.JOB_DTL_ID) AS JOB_DTL_ID ")
                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                .AppendLine("             ,MIN(DECODE(M2.INSPECTION_NEED_FLG, 1, M2.INSPECTION_STATUS, 2)) AS INSPECTION_STATUS ")

                '.AppendLine("             ,MAX(M3.SCHE_END_DATETIME) AS SCHE_END_DATETIME ")
                .AppendLine("             ,MAX(CASE ")
                .AppendLine("                  WHEN M3.RSLT_END_DATETIME <> :MINDATE THEN M3.RSLT_END_DATETIME ")
                .AppendLine("                  WHEN M3.PRMS_END_DATETIME <> :MINDATE THEN M3.PRMS_END_DATETIME ")
                .AppendLine("                  ELSE M3.SCHE_END_DATETIME END) AS LAST_END_DATETIME ")
                .AppendLine("             ,SUM(DECODE(M3.STALL_USE_STATUS, :STALL_USE_00, M3.SCHE_WORKTIME, ")
                .AppendLine("                  DECODE(M3.STALL_USE_STATUS, :STALL_USE_01, M3.SCHE_WORKTIME, 0))) AS REMAINING_WORK_TIME ")
                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                .AppendLine("             ,MAX(M3.RSLT_START_DATETIME) AS RSLT_START_DATETIME ")
                .AppendLine("         FROM TB_T_SERVICEIN M1 ")
                .AppendLine("             ,TB_T_JOB_DTL M2 ")
                .AppendLine("             ,TB_T_STALL_USE M3 ")
                .AppendLine("        WHERE M1.SVCIN_ID = M2.SVCIN_ID ")
                .AppendLine("          AND M2.JOB_DTL_ID = M3.JOB_DTL_ID ")
                .AppendLine("          AND M1.SVCIN_ID IN ( ")
                .AppendLine(ReserveReplaceWord)
                .AppendLine("                              ) ")
                .AppendLine("          AND M1.DLR_CD = :DLR_CD ")
                .AppendLine("          AND M1.BRN_CD = :BRN_CD ")
                .AppendLine("          AND M1.SVC_STATUS <> :SVC_STATUS_02 ")
                .AppendLine("          AND M2.DLR_CD = :DLR_CD ")
                .AppendLine("          AND M2.BRN_CD = :BRN_CD ")
                .AppendLine("          AND M2.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("          AND M3.DLR_CD = :DLR_CD ")
                .AppendLine("          AND M3.BRN_CD = :BRN_CD ")
                .AppendLine("     GROUP BY M1.SVCIN_ID ")
                .AppendLine("      ) T2 ")
                .AppendLine("     ,TB_T_CARWASH_RESULT T3 ")
                .AppendLine("     ,TB_T_JOB_DTL T4 ")
                .AppendLine("     ,TB_M_MERCHANDISE T5 ")
                .AppendLine("     ,TB_M_SERVICE_CLASS T6 ")
                .AppendLine("WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("  AND T1.SVCIN_ID = T3.SVCIN_ID(+) ")
                .AppendLine("  AND T2.JOB_DTL_ID = T4.JOB_DTL_ID ")
                .AppendLine("  AND T4.MERC_ID = T5.MERC_ID(+) ")
                .AppendLine("  AND T4.SVC_CLASS_ID = T6.SVC_CLASS_ID(+) ")
                .AppendLine("  AND T1.SVCIN_ID IN ( ")
                .AppendLine(ReserveReplaceWord)
                .AppendLine("                      ) ")
                .AppendLine("  AND T1.DLR_CD = :DLR_CD ")
                .AppendLine("  AND T1.BRN_CD = :BRN_CD ")
                .AppendLine("  AND T1.SVC_STATUS <> :SVC_STATUS_02 ")
                .AppendLine("  AND T4.DLR_CD = :DLR_CD ")
                .AppendLine("  AND T4.BRN_CD = :BRN_CD ")
                .AppendLine("  AND T4.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine(" GROUP BY T1.SVCIN_ID ")

            End With

            Using query As New DBSelectQuery(Of SC3220201StallRezinfoDataTable)("SC3220201_002")
                Dim sqlReserveId As New StringBuilder
                ' 初回予約ID用取得文字列
                If Not inReserveIdList Is Nothing Then

                    If inReserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID").Count > 0 Then

                        'sqlReserveId.Append(" AND T1.SVCIN_ID IN ( ")

                        Dim count As Long = 1

                        Dim reserveIdName As String

                        For Each row As SC3220201ServiceVisitManagementRow In inReserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID")

                            ' SQL作成
                            reserveIdName = String.Format(CultureInfo.CurrentCulture, "REZID{0}", count)

                            If count > 1 Then

                                sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", reserveIdName))
                            Else

                                sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", reserveIdName))
                            End If

                            ' パラメータ作成
                            query.AddParameterWithTypeValue(reserveIdName, OracleDbType.Decimal, row.FREZID)

                            count += 1


                        Next

                        'sqlReserveId.Append(" ) ")

                    End If

                End If

                If String.IsNullOrEmpty(sqlReserveId.ToString()) Then

                    'sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND T1.SVCIN_ID = {0} ", MinReserveId))
                    sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " {0} ", MinReserveId))

                End If

                'SQL置換
                sql.Replace(ReserveReplaceWord, sqlReserveId.ToString)

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数

                '受付区分("0"：予約客)
                query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_0", OracleDbType.NVarchar2, AcceptanceTypeRez)
                '表示アイコンフラグ("0"：非表示)
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                '表示アイコンフラグ("1"：表示)
                query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)
                'DB最小値
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
                '日付最小値
                query.AddParameterWithTypeValue("MIN", OracleDbType.Date, Date.MinValue)
                '洗車必要フラグ("0"：不要)
                query.AddParameterWithTypeValue("CARWASH_NEED_FLG_0", OracleDbType.NVarchar2, CarWash)
                'サービスステータス("02"：キャンセル)
                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, StatusCancel)
                '販売店コード
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                '店舗コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                'キャンセルフラグ("0"：有効)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelFlagEffective)

                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                'ストール利用ステータス(00:着工指示待ち)
                query.AddParameterWithTypeValue("STALL_USE_00", OracleDbType.NVarchar2, StallStatusWorkOrderWait)

                'ストール利用ステータス(01:作業開始待ち)
                query.AddParameterWithTypeValue("STALL_USE_01", OracleDbType.NVarchar2, StallStatusStartWait)
                '2017/09/14 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START

                '検索結果返却
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            Return dt

        End Function

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

        ''' <summary>
        ''' 予約エリア情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns>受付待ち情報</returns>
        ''' <history>
        ''' 2013/05/01 TMEJ小澤	ITxxxx_TSL自主研緊急対応（サービス）2回目
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
        ''' 2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発
        ''' </history>
        ''' <remarks></remarks>
        Public Function GetReserveAreaInformation(ByVal inDealerCode As String, _
                                                  ByVal inBranchCode As String, _
                                                  ByVal inNowDate As Date) As SC3220201RezAreainfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inNowDate = {4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode _
                        , inBranchCode _
                        , inNowDate.ToString(CultureInfo.CurrentCulture)))

            Dim dt As SC3220201RezAreainfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成

            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            'With sql
            '    sql.Append("SELECT /* SC3220201_004 */ ")
            '    sql.Append("       T1.DLRCD ")
            '    sql.Append("      ,T1.STRCD ")
            '    sql.Append("      ,T1.REZID ")
            '    sql.Append("      ,NVL(T1.PREZID, T1.REZID) AS PREZID ")
            '    sql.Append("      ,NVL(T1.STARTTIME, :MINDATE) AS STARTTIME ")
            '    sql.Append("      ,NVL(T1.ENDTIME, :MINDATE) AS ENDTIME ")
            '    sql.Append("      ,T1.CUSTCD ")
            '    sql.Append("      ,T1.CUSTOMERNAME ")
            '    sql.Append("      ,T1.TELNO ")
            '    sql.Append("      ,T1.MOBILE ")
            '    sql.Append("      ,T1.VEHICLENAME ")
            '    sql.Append("      ,T1.VCLREGNO ")
            '    sql.Append("      ,T1.VIN ")
            '    sql.Append("      ,T1.MNTNCD AS MERCHANDISECD ")
            '    sql.Append("      ,(CASE WHEN T3.MAINTECD IS NOT NULL THEN T3.MAINTENM ELSE T4.MAINTENM END) AS MERCHANDISENAME ")
            '    sql.Append("      ,T1.MODELCODE ")
            '    sql.Append("      ,NVL(T1.MILEAGE, - 1) AS MILEAGE ")
            '    sql.Append("      ,NVL(T1.WASHFLG, '0') AS WASHFLG ")
            '    sql.Append("      ,T1.WALKIN ")
            '    sql.Append("      ,T1.REZ_DELI_DATE ")
            '    sql.Append("      ,NVL(T1.ACTUAL_STIME, :MINDATE) AS ACTUAL_STIME ")
            '    sql.Append("      ,NVL(T1.ACTUAL_ETIME, :MINDATE) AS ACTUAL_ETIME ")
            '    sql.Append("      ,NVL(T2.VISITSEQ, 0) AS VISITSEQ ")
            '    sql.Append("      ,T2.PARKINGCODE ")
            '    sql.Append("      ,NVL(T2.VISITTIMESTAMP, :MINDATE) AS VISITTIMESTAMP ")
            '    sql.Append("      ,NVL2(T1.REZ_PICK_DATE, TO_DATE(T1.REZ_PICK_DATE, 'YYYY/MM/DD HH24:MI'), :MINDATE) AS REZ_PICK_DATE ")
            '    sql.Append("      ,T1.STARTTIME ")
            '    sql.Append("      ,T2.ASSIGNSTATUS ")
            '    sql.Append("      ,T1.ORDERNO ")
            '    sql.Append("      ,T2.CUSTSEGMENT ")
            '    sql.Append("      ,T2.DMSID ")
            '    sql.Append("  FROM TBL_STALLREZINFO T1 ")
            '    sql.Append("      ,(SELECT M2.VISITSEQ ")
            '    sql.Append("              ,M2.DLRCD ")
            '    sql.Append("              ,M2.STRCD ")
            '    sql.Append("              ,NVL(M2.FREZID, M2.REZID) AS REZID ")
            '    sql.Append("              ,M2.PARKINGCODE ")
            '    sql.Append("              ,M2.VISITTIMESTAMP ")
            '    sql.Append("              ,M2.ASSIGNSTATUS ")
            '    sql.Append("              ,M2.ORDERNO ")
            '    sql.Append("              ,M2.CUSTSEGMENT ")
            '    sql.Append("              ,M2.DMSID ")
            '    sql.Append("          FROM TBL_SERVICE_VISIT_MANAGEMENT M2 ")
            '    sql.Append("         WHERE M2.DLRCD = :DLRCD ")
            '    sql.Append("           AND M2.STRCD = :STRCD ")
            '    sql.Append("           AND TRUNC(M2.VISITTIMESTAMP) = TRUNC(:NOWDATE)) T2 ")
            '    sql.Append("      ,TBLORG_MAINTEMASTER T3 ")
            '    sql.Append("      ,TBLORG_MAINTEMASTER T4 ")
            '    sql.Append("      ,(SELECT M5.DLRCD ")
            '    sql.Append("              ,M5.STRCD ")
            '    sql.Append("              ,M5.REZID ")
            '    sql.Append("              ,MIN(M5.STARTTIME) ")
            '    sql.Append("          FROM (SELECT M3.DLRCD ")
            '    sql.Append("                      ,M3.STRCD ")
            '    sql.Append("                      ,NVL(M3.PREZID, M3.REZID) AS REZID ")
            '    sql.Append("                      ,M3.STARTTIME ")
            '    sql.Append("                  FROM TBL_STALLREZINFO M3 ")
            '    sql.Append("                 WHERE M3.DLRCD = :DLRCD ")
            '    sql.Append("                   AND M3.STRCD = :STRCD ")
            '    sql.Append("                   AND NOT EXISTS (SELECT 1 ")
            '    sql.Append("                                     FROM TBL_STALLREZINFO M4 ")
            '    sql.Append("                                    WHERE M4.DLRCD = M3.DLRCD ")
            '    sql.Append("                                      AND M4.STRCD = M3.STRCD ")
            '    sql.Append("                                      AND M4.REZID = M3.REZID ")
            '    sql.Append("                                      AND ((M4.STOPFLG = :STOPFLG0 ")
            '    sql.Append("                                          AND M4.CANCELFLG = :CANCELFLG1) ")
            '    sql.Append("                                          OR M4.REZCHILDNO IN (:CHILDNOLEAVE, :CHILDNODELIVERY)))) M5 ")
            '    sql.Append("      GROUP BY M5.DLRCD, M5.STRCD, M5.REZID) T5 ")
            '    sql.Append(" WHERE T1.DLRCD = T3.DLRCD (+) ")
            '    sql.Append("   AND T1.MNTNCD = T3.MAINTECD (+) ")
            '    sql.Append("   AND :BASETYPEALL = T3.BASETYPE (+) ")
            '    sql.Append("   AND T1.DLRCD = T4.DLRCD (+) ")
            '    sql.Append("   AND T1.MNTNCD = T4.MAINTECD (+) ")
            '    sql.Append("   AND T1.MODELCODE = T4.BASETYPE (+) ")
            '    sql.Append("   AND T1.DLRCD = T2.DLRCD(+) ")
            '    sql.Append("   AND T1.STRCD = T2.STRCD(+) ")
            '    sql.Append("   AND T1.REZID = T2.REZID(+) ")
            '    sql.Append("   AND T1.DLRCD = T5.DLRCD ")
            '    sql.Append("   AND T1.STRCD = T5.STRCD ")
            '    sql.Append("   AND T1.REZID = T5.REZID ")
            '    sql.Append("   AND T1.DLRCD = :DLRCD ")
            '    sql.Append("   AND T1.STRCD = :STRCD ")
            '    sql.Append("   AND T2.VISITTIMESTAMP IS NULL ")
            '    '2013/05/01 TMEJ小澤	ITxxxx_TSL自主研緊急対応（サービス）2回目 START
            '    sql.Append("   AND T1.STRDATE IS NULL ")
            '    '2013/05/01 TMEJ小澤	ITxxxx_TSL自主研緊急対応（サービス）2回目 END
            '    sql.Append("   AND TRUNC(NVL2(T1.REZ_PICK_DATE, TO_DATE(T1.REZ_PICK_DATE, 'YYYY/MM/DD HH24:MI'), T1.STARTTIME)) = TRUNC(:NOWDATE) ")
            '    sql.Append("   AND NOT EXISTS (SELECT 1 ")
            '    sql.Append("                     FROM TBL_STALLREZINFO M1 ")
            '    sql.Append("                    WHERE M1.DLRCD = T1.DLRCD ")
            '    sql.Append("                      AND M1.STRCD = T1.STRCD ")
            '    sql.Append("                      AND M1.REZID = T1.REZID ")
            '    sql.Append("                      AND ((M1.STOPFLG = :STOPFLG0 ")
            '    sql.Append("                          AND M1.CANCELFLG = :CANCELFLG1) ")
            '    sql.Append("                          OR M1.REZCHILDNO IN (:CHILDNOLEAVE, :CHILDNODELIVERY)))  ")
            '    sql.Append(" UNION ALL ")
            '    sql.Append("    SELECT ")
            '    sql.Append("            M3.DLRCD ")
            '    sql.Append("           ,M3.STRCD ")
            '    sql.Append("           ,M3.REZID ")
            '    sql.Append("           ,NVL(M3.FREZID, M3.REZID) AS PREZID ")
            '    sql.Append("           ,:MINDATE AS STARTTIME ")
            '    sql.Append("           ,:MINDATE AS ENDTIME ")
            '    sql.Append("           ,M3.CUSTID ")
            '    sql.Append("           ,M3.NAME AS CUSTOMERNAME ")
            '    sql.Append("           ,M3.TELNO ")
            '    sql.Append("           ,M3.MOBILE ")
            '    sql.Append("           ,NULL AS VEHICLENAME ")
            '    sql.Append("           ,M3.VCLREGNO ")
            '    sql.Append("           ,M3.VIN ")
            '    sql.Append("           ,NULL AS MERCHANDISECD ")
            '    sql.Append("           ,NULL AS MERCHANDISENAME ")
            '    sql.Append("           ,M3.MODELCODE ")
            '    sql.Append("           ,-1 AS MILEAGE ")
            '    sql.Append("           ,'0' AS WASHFLG ")
            '    sql.Append("           ,S1.WALKIN AS WALKIN ")
            '    sql.Append("           ,NULL AS REZ_DELI_DATE ")
            '    sql.Append("           ,:MINDATE AS ACTUAL_STIME ")
            '    sql.Append("           ,:MINDATE AS ACTUAL_ETIME ")
            '    sql.Append("           ,M3.VISITSEQ AS VISITSEQ ")
            '    sql.Append("           ,M3.PARKINGCODE ")
            '    sql.Append("           ,NVL(M3.VISITTIMESTAMP, :MINDATE) AS VISITTIMESTAMP ")
            '    sql.Append("           ,:MINDATE AS REZ_PICK_DATE ")
            '    sql.Append("           ,:MINDATE AS STARTTIME ")
            '    sql.Append("           ,M3.ASSIGNSTATUS ")
            '    sql.Append("           ,M3.ORDERNO ")
            '    sql.Append("           ,M3.CUSTSEGMENT ")
            '    sql.Append("           ,M3.DMSID ")                
            '    sql.Append("      FROM TBL_SERVICE_VISIT_MANAGEMENT M3 ")
            '    sql.Append("          ,TBL_STALLREZINFO S1 ")
            '    sql.Append("     WHERE M3.DLRCD = S1.DLRCD(+) ")
            '    sql.Append("       AND M3.STRCD = S1.STRCD(+) ")
            '    sql.Append("       AND M3.FREZID = S1.REZID(+) ")
            '    sql.Append("       AND M3.DLRCD = :DLRCD ")
            '    sql.Append("       AND M3.STRCD = :STRCD ")
            '    '2013/05/01 TMEJ小澤	ITxxxx_TSL自主研緊急対応（サービス）2回目 START
            '    sql.Append("       AND S1.STRDATE IS NULL ")
            '    '2013/05/01 TMEJ小澤	ITxxxx_TSL自主研緊急対応（サービス）2回目 END
            '    sql.Append("       AND TRUNC(M3.VISITTIMESTAMP) = TRUNC(:NOWDATE) ")
            '    sql.Append("       AND M3.ASSIGNSTATUS IN ('0', '1', '9') ")

            'End With

            With sql

                .AppendLine(" SELECT /* SC3220201_004 */ ")
                .AppendLine("        A1.DLR_CD AS DLRCD ")
                .AppendLine("       ,A1.BRN_CD AS STRCD ")
                .AppendLine("       ,A1.SVCIN_ID AS REZID ")
                .AppendLine("       ,A1.SVCIN_ID AS PREZID ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                '.AppendLine("       ,DECODE(A1.SCHE_START_DATETIME, :MINDATE, :MIN, A1.SCHE_START_DATETIME) AS STARTTIME ")
                '.AppendLine("       ,DECODE(A1.SCHE_END_DATETIME, :MINDATE, :MIN, A1.SCHE_END_DATETIME) AS ENDTIME ")

                .AppendLine("       ,CASE ")
                .AppendLine("             WHEN A1.SCHE_START_DATETIME = :MINDATE THEN :MIN ")
                .AppendLine("             WHEN A1.SCHE_START_DATETIME IS NULL THEN :MIN ")
                .AppendLine("             ELSE A1.SCHE_START_DATETIME ")
                .AppendLine("         END AS STARTTIME ")
                .AppendLine("       ,CASE ")
                .AppendLine("             WHEN A1.SCHE_END_DATETIME = :MINDATE THEN :MIN ")
                .AppendLine("             WHEN A1.SCHE_END_DATETIME IS NULL THEN :MIN ")
                .AppendLine("             ELSE A1.SCHE_END_DATETIME ")
                .AppendLine("         END AS ENDTIME ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                .AppendLine("       ,TO_CHAR(A1.CST_ID) AS CUSTID ")
                .AppendLine("       ,TRIM(A1.CST_NAME) AS CUSTOMERNAME ")
                .AppendLine("       ,TRIM(A1.CST_PHONE) AS TELNO ")
                .AppendLine("       ,TRIM(A1.CST_MOBILE) AS MOBILE ")
                .AppendLine("       ,TRIM(A1.MODEL_NAME) AS VEHICLENAME ")
                .AppendLine("       ,TRIM(A1.REG_NUM) AS VCLREGNO ")
                .AppendLine("       ,TRIM(A1.VCL_VIN) AS VIN ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                '.AppendLine("       ,TRIM(A1.MAINTE_CD) AS MERCHANDISECD ")
                '.AppendLine("       ,(CASE WHEN A4.MAINTECD IS NOT NULL THEN ")
                '.AppendLine("                   A4.MAINTENM ELSE ")
                '.AppendLine("                   A5.MAINTENM END) AS MERCHANDISENAME ")

                .AppendLine("      ,NVL(CONCAT(TRIM(A4.UPPER_DISP), TRIM(A4.LOWER_DISP)), NVL(A5.SVC_CLASS_NAME, A5.SVC_CLASS_NAME_ENG)) AS MERCHANDISENAME ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                .AppendLine("       ,TRIM(A1.MODEL_CD) AS MODELCODE ")
                .AppendLine("       ,A1.SVCIN_MILE AS MILEAGE ")
                .AppendLine("       ,NVL(TRIM(A1.CARWASH_NEED_FLG), :CARWASH_NEED_FLG_0) AS WASHFLG ")
                .AppendLine("       ,TRIM(A1.ACCEPTANCE_TYPE) AS WALKIN ")
                .AppendLine("       ,DECODE(A1.SCHE_SVCIN_DATETIME, :MINDATE, :MIN, A1.SCHE_SVCIN_DATETIME) AS REZ_PICK_DATE ")
                .AppendLine("       ,DECODE(A1.RSLT_START_DATETIME, :MINDATE, :MIN, A1.RSLT_START_DATETIME) AS ACTUAL_STIME ")
                .AppendLine("       ,DECODE(A1.RSLT_END_DATETIME, :MINDATE, :MIN, A1.RSLT_END_DATETIME) AS ACTUAL_ETIME ")
                .AppendLine("       ,NVL(A2.VISITSEQ, 0) AS VISITSEQ ")
                .AppendLine("       ,NVL(A2.VISITTIMESTAMP, :MIN) AS VISITTIMESTAMP ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                '.AppendLine("       ,DECODE(A1.SCHE_DELI_DATETIME, :MINDATE, NULL, TO_CHAR(A1.SCHE_DELI_DATETIME, 'YYYYMMDDHH24MI')) AS REZ_DELI_DATE ")
                .AppendLine("       ,CASE ")
                .AppendLine("             WHEN A1.SCHE_DELI_DATETIME = :MINDATE THEN :MIN ")
                .AppendLine("             WHEN A1.SCHE_DELI_DATETIME IS NULL THEN :MIN ")
                .AppendLine("             ELSE A1.SCHE_DELI_DATETIME ")
                .AppendLine("         END AS REZ_DELI_DATE ")

                .AppendLine("       ,CASE ")

                '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 START

                '.AppendLine("             WHEN A1.VIP_FLG = :ICON_FLAG_ON THEN A1.VIP_FLG ")

                .AppendLine("             WHEN A1.IMP_VCL_FLG = :ICON_FLAG_ON THEN A1.IMP_VCL_FLG ")

                '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 END

                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("             WHEN A1.IMP_VCL_FLG = :ICON_FLAG_ON_2 THEN A1.IMP_VCL_FLG ")
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("             ELSE :ICON_FLAG_OFF  ")
                .AppendLine("         END AS JDP_MARK ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                .AppendLine("       ,A2.ASSIGNSTATUS ")
                .AppendLine("       ,TRIM(A1.RO_NUM) AS ORDERNO ")
                .AppendLine("       ,A2.CUSTSEGMENT ")
                .AppendLine("       ,A2.DMSID ")
                '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                .AppendLine("       ,CASE ")
                .AppendLine("             WHEN A1.SSC_MARK = :ICON_FLAG_ON THEN A1.SSC_MARK ")
                .AppendLine("             ELSE :ICON_FLAG_OFF  ")
                .AppendLine("         END AS SSC_MARK ")
                '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("       ,A1.SML_AMC_FLG ")
                .AppendLine("       ,A1.EW_FLG ")
                .AppendLine("       ,A1.TLM_MBR_FLG")
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("   FROM ")
                .AppendLine("       (SELECT T1.DLR_CD ")
                .AppendLine("              ,T1.BRN_CD ")
                .AppendLine("              ,T1.SVCIN_ID ")
                .AppendLine("              ,T1.CST_ID ")
                .AppendLine("              ,T1.CARWASH_NEED_FLG ")
                .AppendLine("              ,T1.SCHE_DELI_DATETIME ")
                .AppendLine("              ,T1.ACCEPTANCE_TYPE ")
                .AppendLine("              ,T1.RSLT_SVCIN_DATETIME ")
                .AppendLine("              ,T1.RO_NUM ")
                .AppendLine("              ,T1.SCHE_SVCIN_DATETIME ")
                .AppendLine("              ,T1.SVCIN_MILE ")
                .AppendLine("              ,T2.JOB_DTL_ID ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                .AppendLine("              ,T2.MERC_ID ")
                .AppendLine("              ,T2.SVC_CLASS_ID ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                .AppendLine("              ,T2.MAINTE_CD ")
                .AppendLine("              ,T3.STALL_USE_ID ")
                .AppendLine("              ,T3.SCHE_START_DATETIME ")
                .AppendLine("              ,T3.SCHE_END_DATETIME ")
                .AppendLine("              ,T3.RSLT_START_DATETIME ")
                .AppendLine("              ,T3.RSLT_END_DATETIME ")
                .AppendLine("              ,T4.CST_NAME ")
                .AppendLine("              ,T4.CST_PHONE ")
                .AppendLine("              ,T4.CST_MOBILE ")
                .AppendLine("              ,T5.VCL_VIN ")
                .AppendLine("              ,T5.MODEL_CD ")
                .AppendLine("              ,T5.VCL_KATASHIKI ")
                .AppendLine("              ,T6.REG_NUM ")

                '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 START

                '.AppendLine("              ,T6.VIP_FLG ")
                .AppendLine("              ,T6.IMP_VCL_FLG ")

                '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 END

                .AppendLine("              ,T7.MODEL_NAME ")
                '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                .AppendLine("              ,T5.SPECIAL_CAMPAIGN_TGT_FLG AS SSC_MARK ")
                '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("              , NVL(TRIM(T8.SML_AMC_FLG), :ICON_FLAG_OFF) AS SML_AMC_FLG ")
                .AppendLine("              , NVL(TRIM(T8.EW_FLG), :ICON_FLAG_OFF) AS EW_FLG ")
                .AppendLine("              , CASE ")
                .AppendLine("                       WHEN T9.VCL_VIN IS NOT NULL THEN :ICON_FLAG_ON ")
                .AppendLine("                       ELSE :ICON_FLAG_OFF ")
                .AppendLine("                END AS TLM_MBR_FLG ")
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("          FROM TB_T_SERVICEIN T1 ")
                .AppendLine("              ,TB_T_JOB_DTL T2 ")
                .AppendLine("              ,TB_T_STALL_USE T3 ")
                .AppendLine("              ,TB_M_CUSTOMER T4 ")
                .AppendLine("              ,TB_M_VEHICLE T5 ")
                .AppendLine("              ,TB_M_VEHICLE_DLR T6 ")
                .AppendLine("              ,TB_M_MODEL T7 ")
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("              ,TB_LM_VEHICLE T8 ")
                .AppendLine("              ,TB_LM_TLM_MEMBER T9")
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("        WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("          AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("          AND T1.CST_ID = T4.CST_ID ")
                .AppendLine("          AND T1.VCL_ID = T5.VCL_ID ")
                .AppendLine("          AND T1.DLR_CD = T6.DLR_CD ")
                .AppendLine("          AND T1.VCL_ID = T6.VCL_ID ")
                .AppendLine("          AND T5.MODEL_CD = T7.MODEL_CD(+) ")
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("          AND T5.VCL_ID = T8.VCL_ID(+) ")
                .AppendLine("          AND T5.VCL_VIN = T9.VCL_VIN(+) ")
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("          AND T1.DLR_CD = :DLR_CD ")
                .AppendLine("          AND T1.BRN_CD = :BRN_CD ")
                .AppendLine("          AND T2.CANCEL_FLG = :CANCEL_FLG_0) A1 ")
                .AppendLine("       ,(SELECT Y1.VISITSEQ ")
                .AppendLine("               ,Y1.VISITTIMESTAMP ")
                .AppendLine("               ,Y1.ASSIGNSTATUS ")
                .AppendLine("               ,Y1.CUSTSEGMENT ")
                .AppendLine("               ,Y1.DMSID ")
                .AppendLine("               ,Y1.REZID ")
                .AppendLine("           FROM TBL_SERVICE_VISIT_MANAGEMENT  Y1 ")
                .AppendLine("          WHERE Y1.DLRCD = :DLR_CD ")
                .AppendLine("            AND Y1.STRCD = :BRN_CD ")
                .AppendLine("            AND Y1.VISITTIMESTAMP >= TRUNC(:NOWDATE)) A2 ")
                .AppendLine("       ,(SELECT U1.SVCIN_ID ")
                .AppendLine("               ,MIN(U2.JOB_DTL_ID) AS JOB_DTL_ID_MIN ")
                .AppendLine("               ,MIN(U3.STALL_USE_ID) AS STALL_USE_ID_MIN ")
                .AppendLine("               ,MIN(U3.SCHE_START_DATETIME) AS SCHE_START_DATETIME_MIN ")
                .AppendLine("           FROM TB_T_SERVICEIN U1 ")
                .AppendLine("               ,TB_T_JOB_DTL U2 ")
                .AppendLine("               ,TB_T_STALL_USE U3 ")
                .AppendLine("          WHERE U1.SVCIN_ID = U2.SVCIN_ID ")
                .AppendLine("            AND U2.JOB_DTL_ID = U3.JOB_DTL_ID ")
                .AppendLine("            AND U1.DLR_CD = :DLR_CD ")
                .AppendLine("            AND U1.BRN_CD = :BRN_CD ")
                .AppendLine("            AND U2.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("          GROUP BY U1.SVCIN_ID) A3 ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                '.AppendLine("       ,TBLORG_MAINTEMASTER A4 ")
                '.AppendLine("       ,TBLORG_MAINTEMASTER A5 ")

                .AppendLine("       ,TB_M_MERCHANDISE A4 ")
                .AppendLine("       ,TB_M_SERVICE_CLASS A5 ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                .AppendLine("  WHERE ")
                .AppendLine("        A1.SVCIN_ID = A2.REZID(+) ")
                .AppendLine("    AND A1.SVCIN_ID = A3.SVCIN_ID ")
                .AppendLine("    AND A1.JOB_DTL_ID = A3.JOB_DTL_ID_MIN ")
                .AppendLine("    AND A1.STALL_USE_ID = A3.STALL_USE_ID_MIN ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                '.AppendLine("    AND A1.DLR_CD = A4.DLRCD(+) ")
                '.AppendLine("    AND A1.MAINTE_CD = A4.MAINTECD(+) ")
                '.AppendLine("    AND :MAINTE_KATASHIKI = A4.BASETYPE(+) ")
                '.AppendLine("    AND A1.DLR_CD = A5.DLRCD(+) ")
                '.AppendLine("    AND A1.MAINTE_CD = A5.MAINTECD(+) ")
                '.AppendLine("    AND SUBSTR(A1.VCL_KATASHIKI, 0, INSTR(A1.VCL_KATASHIKI, :KATASHIKI) - 1) = A5.BASETYPE(+) ")

                .AppendLine("    AND A1.MERC_ID = A4.MERC_ID(+) ")
                .AppendLine("    AND A1.SVC_CLASS_ID = A5.SVC_CLASS_ID(+) ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                .AppendLine("    AND A1.DLR_CD = :DLR_CD ")
                .AppendLine("    AND A1.BRN_CD = :BRN_CD ")
                .AppendLine("    AND NOT EXISTS (SELECT 1 ")
                .AppendLine("                      FROM TB_T_SERVICEIN F1 ")
                .AppendLine("                     WHERE F1.SVCIN_ID = A1.SVCIN_ID ")
                .AppendLine("                       AND F1.SVC_STATUS = :SVC_STATUS_02) ")
                .AppendLine("    AND A2.VISITSEQ IS NULL ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START
                '.AppendLine("    AND A1.RSLT_SVCIN_DATETIME = :MINDATE ")
                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                .AppendLine("    AND TRUNC(DECODE(A1.SCHE_SVCIN_DATETIME, :MINDATE, A3.SCHE_START_DATETIME_MIN, A1.SCHE_SVCIN_DATETIME)) = TRUNC(:NOWDATE) ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

                '.AppendLine("    AND A4.DLRCD(+) = :DLR_CD ")
                '.AppendLine("    AND A5.DLRCD(+) = :DLR_CD ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

                .AppendLine("  UNION ALL ")
                .AppendLine(" SELECT ")
                .AppendLine("        S1.DLRCD AS DLRCD ")
                .AppendLine("       ,S1.STRCD AS STRCD ")
                .AppendLine("       ,NVL(S1.REZID, -1) AS REZID ")
                .AppendLine("       ,NVL(NVL(S1.FREZID, S1.REZID), -1) AS PREZID ")
                .AppendLine("       ,:MIN AS STARTTIME ")
                .AppendLine("       ,:MIN AS ENDTIME ")
                .AppendLine("       ,TO_CHAR(S1.CUSTID) AS CUSTID ")
                .AppendLine("       ,S1.NAME AS CUSTOMERNAME ")
                .AppendLine("       ,S1.TELNO ")
                .AppendLine("       ,S1.MOBILE ")
                .AppendLine("       ,NULL AS VEHICLENAME ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                '.AppendLine("       ,S1.VCLREGNO ")
                .AppendLine("       ,NVL(TRIM(S7.REG_NUM), S1.VCLREGNO) ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END
                .AppendLine("       ,S1.VIN AS VIN ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                '.AppendLine("       ,TRIM(S2.MAINTE_CD) AS MERCHANDISECD ")
                '.AppendLine("       ,(CASE WHEN S5.MAINTECD IS NOT NULL THEN ")
                '.AppendLine("                   S5.MAINTENM ELSE ")
                '.AppendLine("                   S6.MAINTENM END) AS MERCHANDISENAME ")

                .AppendLine("      ,NVL(CONCAT(TRIM(S5.UPPER_DISP), TRIM(S5.LOWER_DISP)), NVL(S6.SVC_CLASS_NAME, S6.SVC_CLASS_NAME_ENG)) AS MERCHANDISENAME ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                .AppendLine("       ,S1.MODELCODE ")
                .AppendLine("       ,-1 AS MILEAGE ")
                .AppendLine("       ,:CARWASH_NEED_FLG_0 AS WASHFLG ")
                .AppendLine("       ,S2.ACCEPTANCE_TYPE AS WALKIN ")
                .AppendLine("       ,:MIN AS REZ_PICK_DATE ")
                .AppendLine("       ,:MIN AS ACTUAL_STIME ")
                .AppendLine("       ,:MIN AS ACTUAL_ETIME ")
                .AppendLine("       ,S1.VISITSEQ ")
                .AppendLine("       ,NVL(S1.VISITTIMESTAMP, :MIN) AS VISITTIMESTAMP ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                '.AppendLine("       ,NULL AS REZ_DELI_DATE ")
                .AppendLine("       ,:MIN AS REZ_DELI_DATE ")

                .AppendLine("       ,CASE ")

                '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 START

                '.AppendLine("             WHEN S7.VIP_FLG = :ICON_FLAG_ON THEN S7.VIP_FLG ")
                .AppendLine("             WHEN S7.IMP_VCL_FLG = :ICON_FLAG_ON THEN S7.IMP_VCL_FLG ")

                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("             WHEN S7.IMP_VCL_FLG = :ICON_FLAG_ON_2 THEN S7.IMP_VCL_FLG ")
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 END

                .AppendLine("             ELSE :ICON_FLAG_OFF  ")
                .AppendLine("         END AS JDP_MARK ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                .AppendLine("       ,S1.ASSIGNSTATUS ")
                .AppendLine("       ,S1.ORDERNO AS ORDERNO ")
                .AppendLine("       ,S1.CUSTSEGMENT ")
                .AppendLine("       ,S1.DMSID ")
                '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                .AppendLine("       ,CASE ")
                .AppendLine("             WHEN S8.SPECIAL_CAMPAIGN_TGT_FLG = :ICON_FLAG_ON THEN S8.SPECIAL_CAMPAIGN_TGT_FLG ")
                .AppendLine("             ELSE :ICON_FLAG_OFF  ")
                .AppendLine("         END AS SSC_MARK ")
                '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("              , NVL(TRIM(S9.SML_AMC_FLG), :ICON_FLAG_OFF) AS SML_AMC_FLG ")
                .AppendLine("              , NVL(TRIM(S9.EW_FLG), :ICON_FLAG_OFF) AS EW_FLG ")
                .AppendLine("              , CASE ")
                .AppendLine("                       WHEN S10.VCL_VIN IS NOT NULL THEN :ICON_FLAG_ON ")
                .AppendLine("                       ELSE :ICON_FLAG_OFF ")
                .AppendLine("                END AS TLM_MBR_FLG ")
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("   FROM ")
                .AppendLine("        TBL_SERVICE_VISIT_MANAGEMENT S1 ")
                .AppendLine("       ,(SELECT TB1.SVCIN_ID ")
                .AppendLine("               ,TB1.ACCEPTANCE_TYPE ")
                .AppendLine("               ,TB1.DLR_CD ")
                .AppendLine("               ,TB1.RSLT_SVCIN_DATETIME ")
                .AppendLine("               ,TB2.VCL_KATASHIKI ")
                .AppendLine("               ,TB3.JOB_DTL_ID ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                .AppendLine("               ,TB3.MERC_ID ")
                .AppendLine("               ,TB3.SVC_CLASS_ID ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                .AppendLine("               ,TB3.MAINTE_CD ")
                .AppendLine("               ,ROW_NUMBER() OVER ( ")
                .AppendLine("                     PARTITION BY TB1.SVCIN_ID ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                '.AppendLine("                                 ,TB3.JOB_DTL_ID ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                .AppendLine("                         ORDER BY TB1.SVCIN_ID ASC ")
                .AppendLine("                                 ,TB3.JOB_DTL_ID ASC) AS NUM ")
                .AppendLine("          FROM TB_T_SERVICEIN TB1 ")
                .AppendLine("              ,TB_M_VEHICLE TB2 ")
                .AppendLine("              ,TB_T_JOB_DTL TB3 ")
                .AppendLine("         WHERE TB1.VCL_ID = TB2.VCL_ID(+) ")
                .AppendLine("           AND TB1.SVCIN_ID = TB3.SVCIN_ID ")
                .AppendLine("           AND TB1.DLR_CD = :DLR_CD ")
                .AppendLine("           AND TB1.BRN_CD = :BRN_CD ")
                .AppendLine("           AND TB3.DLR_CD = :DLR_CD ")
                .AppendLine("           AND TB3.BRN_CD = :BRN_CD ")
                .AppendLine("           AND TB3.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("        ) S2 ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                '.AppendLine("       ,TBLORG_MAINTEMASTER S5 ")
                '.AppendLine("       ,TBLORG_MAINTEMASTER S6 ")

                .AppendLine("       ,TB_M_MERCHANDISE S5 ")
                .AppendLine("       ,TB_M_SERVICE_CLASS S6 ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT
                .AppendLine("       ,TB_M_VEHICLE_DLR S7 ")
                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                .AppendLine("       ,TB_M_VEHICLE S8 ")
                '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("       ,TB_LM_VEHICLE S9")
                .AppendLine("       ,TB_LM_TLM_MEMBER S10")
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("  WHERE ")
                .AppendLine("        S1.REZID = S2.SVCIN_ID(+) ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                '.AppendLine("    AND S2.DLR_CD = S5.DLRCD(+) ")
                '.AppendLine("    AND S2.MAINTE_CD = S5.MAINTECD(+) ")
                '.AppendLine("    AND :MAINTE_KATASHIKI = S5.BASETYPE(+) ")
                '.AppendLine("    AND S2.DLR_CD = S6.DLRCD(+) ")
                '.AppendLine("    AND S2.MAINTE_CD = S6.MAINTECD(+) ")
                '.AppendLine("    AND SUBSTR(S2.VCL_KATASHIKI, 0, INSTR(S2.VCL_KATASHIKI, :KATASHIKI) - 1) = S6.BASETYPE(+) ")

                .AppendLine("    AND S2.MERC_ID = S5.MERC_ID(+) ")
                .AppendLine("    AND S2.SVC_CLASS_ID = S6.SVC_CLASS_ID(+) ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                .AppendLine("    AND S1.DLRCD = S7.DLR_CD(+) ")
                .AppendLine("    AND S1.VCL_ID = S7.VCL_ID(+) ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                .AppendLine("    AND S1.VCL_ID = S8.VCL_ID(+) ")
                '2018/02/26 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                .AppendLine("          AND S8.VCL_ID = S9.VCL_ID(+) ")
                .AppendLine("          AND S8.VCL_VIN = S10.VCL_VIN(+) ")
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
                .AppendLine("    AND S1.DLRCD = :DLR_CD ")
                .AppendLine("    AND S1.STRCD = :BRN_CD ")
                .AppendLine("    AND S1.VISITTIMESTAMP >= TRUNC(:NOWDATE) ")
                .AppendLine("    AND S1.ASSIGNSTATUS IN (:ASSIGNSTATUS_RECEPTION, :ASSIGNSTATUS_WAIT, :ASSIGNSTATUS_HOLD) ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START
                '.AppendLine("    AND S2.RSLT_SVCIN_DATETIME(+) = :MINDATE ")
                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                .AppendLine("    AND S2.NUM(+) = 1 ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

                '.AppendLine("    AND S5.DLRCD(+) = :DLR_CD ")
                '.AppendLine("    AND S6.DLRCD(+) = :DLR_CD ")

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

            End With


            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            Using query As New DBSelectQuery(Of SC3220201RezAreainfoDataTable)("SC3220201_004")
                query.CommandText = sql.ToString()
                'バインド変数

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inBranchCode)
                'query.AddParameterWithTypeValue("STOPFLG0", OracleDbType.Char, "0")
                'query.AddParameterWithTypeValue("CANCELFLG1", OracleDbType.Char, "1")
                'query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, DateTime.MinValue)
                'query.AddParameterWithTypeValue("CHILDNOLEAVE", OracleDbType.Int64, 0)
                'query.AddParameterWithTypeValue("CHILDNODELIVERY", OracleDbType.Int64, 999)
                'query.AddParameterWithTypeValue("BASETYPEALL", OracleDbType.NVarchar2, Me.BaseTypeAll(inDealerCode))
                'query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)

                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelFlagEffective)

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                'query.AddParameterWithTypeValue("MAINTE_KATASHIKI", OracleDbType.NVarchar2, BaseTypeAll)    ' BASETYPE「X」
                'query.AddParameterWithTypeValue("KATASHIKI", OracleDbType.NVarchar2, WordHyphen)

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, StatusCancel)
                query.AddParameterWithTypeValue("CARWASH_NEED_FLG_0", OracleDbType.NVarchar2, CarWash)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("MIN", OracleDbType.Date, Date.MinValue)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_RECEPTION", OracleDbType.NVarchar2, NonAssign)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_WAIT", OracleDbType.NVarchar2, AssignWait)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_HOLD", OracleDbType.NVarchar2, AssignHold)

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 STRAT

                '表示アイコンフラグ("0"：非表示)
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                '表示アイコンフラグ("1"：表示)
                query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                '表示アイコンフラグ("2"：表示)
                query.AddParameterWithTypeValue("ICON_FLAG_ON_2", OracleDbType.NVarchar2, IconFlagOn2)
                '2018/06/29 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

                '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END


                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                '検索結果返却
                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
            Return dt
        End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ' ''' <summary>
        ' ''' TBL_DLRENVSETTINGから「*」を取得
        ' ''' </summary>
        ' ''' <param name="dealerCD">販売店コード</param>
        ' ''' <value></value>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' ''' </history>
        'Private ReadOnly Property BaseTypeAll(ByVal dealerCD As String) As String
        '    Get
        '        Const BASETYPE_ALL As String = "BASETYPE_ALL"
        '        Static value As String
        '        If String.IsNullOrEmpty(value) = True Then
        '            Dim row As DlrEnvSettingDataSet.DLRENVSETTINGRow = (New DealerEnvSetting).GetEnvSetting(dealerCD, BASETYPE_ALL)
        '            value = If(row IsNot Nothing, row.PARAMVALUE, "*")
        '        End If
        '        Return value
        '    End Get
        'End Property

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        ''' <summary>
        ''' 追加作業チップ情報取得
        ''' </summary>
        ''' <param name="inReserveIdList">初回予約IDリスト</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns>追加作業エリアチップ情報取得データセット</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetAddApprovalChipInfo(ByVal inReserveIdList As SC3220201ServiceVisitManagementDataTable _
                                             , ByVal inDealerCode As String _
                                             , ByVal inBranchCode As String) _
                                               As SC3220201AddApprovalChipInfoDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} START IN: COUNT = {2}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inReserveIdList.Count))

            Dim dt As SC3220201AddApprovalChipInfoDataTable

            Using query As New DBSelectQuery(Of SC3220201AddApprovalChipInfoDataTable)("SC3220201_005")
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
                For Each row As SC3220201ServiceVisitManagementRow In inReserveIdList

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

                    .AppendLine("    SELECT /* SC3220201_005 */ ")
                    .AppendLine("           T1.VISIT_ID AS VISIT_ID ")
                    '.AppendLine("          ,T1.RO_JOB_SEQ ")
                    '.AppendLine("          ,MAX(CASE  ")
                    '.AppendLine("                    WHEN T1.RO_CHECK_DATETIME = :MINDATE THEN :MINVALUE ")
                    '.AppendLine("                    ELSE T1.RO_CHECK_DATETIME ")
                    '.AppendLine("               END) AS RO_CHECK_DATETIME ")
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
                '販売店コード
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                '店舗コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                'ROステータス("35"：SA承認待ち)
                query.AddParameterWithTypeValue("STATUS_35", OracleDbType.NVarchar2, StatusConfirmationWait)

                'SQL実行
                dt = query.GetData()

            End Using

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} END OUT:COUNT = {2}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , dt.Rows.Count))

            Return dt

        End Function

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END


#End Region

    End Class

End Namespace

Partial Class SC3220201DataSet
End Class
