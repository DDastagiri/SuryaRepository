'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3140103DataSet.vb
'─────────────────────────────────────
'機能： メインメニュー(SA) データアクセス
'補足： 
'作成： 2012/01/16 KN 小林
'更新： 2012/05/15 KN 森下 【SERVICE_1】残課題管理表 No.10 代表整備項目の表示対応
'更新： 2012/06/18 KN 西岡 【SERVICE_2】事前準備対応
'更新： 2012/07/28 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加)
'更新： 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更)
'更新： 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発
'更新： 2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
'更新： 2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」
'更新： 2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成
'更新： 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新： 2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発
'更新： 2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発
'更新： 2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新； 2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する
'更新； 2015/12/17 TM 浅野 12月号口配信に向けた緊急対応 通知処理時の顧客名称の取得元をサービス来店実績から顧客へ変更する
'更新： 2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新： 2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新： 2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
'更新： 2018/10/26 NSK 坂本 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 No.67調査（SA振り当て解除）
'更新： 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
'更新：                      [TKM]PUAT-4100 SAメインのチップ詳細に追加作業承認ボタンが表示されない を修正
'更新： 2019/05/20 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション
'更新：                      [TKM]PUAT-4153 SAメインの追加JOBのアイコン追加回数が減る を修正
'更新：
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Globalization

Namespace SC3140103DataSetTableAdapters
    Public Class SC3140103DataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        Public Const MinReserveId As Decimal = -1           ' 最小予約ID

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

        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ''' <summary>
        ''' DB日付省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MinDate As String = "1900/01/01 00:00:00"

        ''' <summary>
        ''' ハイフン
        ''' </summary>
        ''' <remarks></remarks>
        Private Const Hyphen As String = "-"

        ''' <summary>
        ''' 基本型式(ALL)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BaseTypeAll As String = "X"

        ''' <summary>
        ''' サービスステータス(キャンセル)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusCancel As String = "02"

        ''' <summary>
        ''' キャンセルフラグ(有効)
        ''' </summary>
        Private Const CancelFlagEffective As String = "0"

        ''' <summary>
        ''' 使用中フラグ(使用中)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const InUse As String = "1"

        ''' <summary>
        ''' 振当てステータス（未振当て）
        ''' </summary>
        Private Const NonAssign As String = "0"

        ''' <summary>
        ''' 振当てステータス（受付待ち）
        ''' </summary>
        Private Const AssignWait As String = "1"

        ''' <summary>
        ''' 振当てステータス（SA振当済）
        ''' </summary>
        Private Const AssignFinish As String = "2"

        ''' <summary>
        ''' 仮置フラグ(仮置き)
        ''' </summary>
        Private Const TenpFlag As String = "1"

        ''' <summary>
        ''' 予約ステータス(本予約)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RezStatus As String = "1"

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

        ''' <summary>
        ''' ストール利用ステータス(未来店客)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallStatusUncome As String = "07"

        ''' <summary>
        ''' 削除フラグ(削除)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const Delete As String = "1"

        ''' <summary>
        ''' オーナーチェンジフラグ(0：未設定)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const NoOwnerChange As String = "0"

        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        ''' <summary>
        ''' 振当てステータス（"0"：未振当て）
        ''' </summary>
        Private Const NoReserve As String = "0"

        ''' <summary>
        ''' 振当てステータス（"1"：受付待ち）
        ''' </summary>
        Private Const GetReserve As String = "1"

        ''' <summary>
        ''' 振当てステータス（"4"：退店）
        ''' </summary>
        Private Const DealerOut As String = "4"

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
        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        ''' <summary>
        ''' 表示アイコンフラグ("2"：ON 表示)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOn2 As String = "2"
        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

        ''' <summary>
        ''' 受付区分("0"：予約客)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AcceptanceTypeRez As String = "0"

        ''' <summary>
        ''' 洗車必要フラグ("0"：洗車不要)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const NonWash As String = "0"

        ''' <summary>
        ''' ROステータス（"10"：SA起票中）
        ''' </summary>
        Private Const StatusSAIssuance As String = "10"

        ''' <summary>
        ''' ROステータス（"35"：SA承認待ち）
        ''' </summary>
        Private Const StatusConfirmationWait As String = "35"

        ''' <summary>
        ''' ROステータス（"40"：顧客承認待ち）
        ''' </summary>
        Private Const StatusCustomerWait As String = "40"

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

        ''' <summary>
        ''' ユーザー削除フラグ("0"：有効)
        ''' </summary>
        Private Const DeleteFlag As String = "0"

        ''' <summary>
        ''' 表示エリア("3"：納車準備)
        ''' </summary>
        Public Const DisplayDivPreparation As String = "3"

        ''' <summary>
        ''' 表示エリア("4"：納車作業)
        ''' </summary>
        Public Const DisplayDivDelivery As String = "4"

        ''' <summary>
        ''' アプリケーションID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ApplicationId As String = "SC3140103"

        ''' <summary>
        ''' 呼出ステータス（"0"：未呼出）
        ''' </summary>
        Private Const NonCall As String = "0"

        ''' <summary>
        ''' 呼出ステータス（"1"：呼出中）
        ''' </summary>
        Private Const Calling As String = "1"

        ''' <summary>
        ''' 呼出ステータス（"2"：呼出完了）
        ''' </summary>
        Private Const CallEnd As String = "2"

        ''' <summary>
        ''' 案内待ちキュー状態("1"：非案内待ち)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const QueueStatusNotWait As String = "1"

        ''' <summary>
        ''' 工程管理エリア
        ''' </summary>
        ''' <remarks></remarks>
        Private Enum ChipArea
            ''' <summary>未選択</summary>
            None = 0
            ''' <summary>受付</summary>
            Reception
            ''' <summary>追加承認</summary>
            Approval
            ''' <summary>納車準備</summary>
            Preparation
            ''' <summary>納車作業</summary>
            Delivery
            ''' <summary>作業中</summary>
            Work
            ''' <summary>事前準備</summary>
            AdvancePreparations
            ''' <summary>振当待ち</summary>
            Assignment

        End Enum

        ''' <summary>
        ''' 顧客種別("2"：未取引客)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CustsegmentNewCustomer As String = "2"

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        ''' <summary>
        ''' 顧客車両区分("4"：保険)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const Insurance As String = "4"

        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

#End Region

        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        '#Region " サービス来店実績取得"

        '        '''-------------------------------------------------------
        '        ''' <summary>
        '        ''' サービス来店実績取得
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="account">アカウント</param>
        '        ''' <param name="orderNoList">整備受注№DataTable</param>
        '        ''' <param name="nowDate">現在日付</param>
        '        ''' <returns>サービス来店実績データセット</returns>
        '        ''' <remarks></remarks>
        '        ''' <history>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </history>
        '        '''-------------------------------------------------------
        '        Public Function GetVisitManagement(ByVal dealerCode As String,
        '                                           ByVal branchCode As String,
        '                                           ByVal account As String,
        '                                           ByVal orderNoList As IC3801003DataSet.IC3801003NoDeliveryRODataTable,
        '                                           ByVal nowDate As Date) As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, account = {5}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_START _
        '                                    , dealerCode _
        '                                    , branchCode _
        '                                    , account))

        '            Dim dt As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable

        '            Try
        '                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103ServiceVisitManagementDataTable)("SC3140103_001")
        '                    Dim sql As New StringBuilder
        '                    Dim sqlOrderNo As New StringBuilder

        '                    ' R/O番号用取得文字列
        '                    If Not orderNoList Is Nothing Then
        '                        If orderNoList.Count > 0 Then
        '                            sqlOrderNo.Append(" OR ORDERNO IN ( ")
        '                            Dim count As Long = 1
        '                            Dim orderName As String
        '                            For Each row As IC3801003DataSet.IC3801003NoDeliveryRORow In orderNoList.Rows
        '                                ' SQL作成
        '                                orderName = String.Format(CultureInfo.CurrentCulture, "ORDERNO{0}", count)
        '                                If count > 1 Then
        '                                    sqlOrderNo.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", orderName))
        '                                Else
        '                                    sqlOrderNo.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", orderName))
        '                                End If

        '                                ' パラメータ作成

        '                                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                                'query.AddParameterWithTypeValue(orderName, OracleDbType.Char, row.ORDERNO)
        '                                query.AddParameterWithTypeValue(orderName, OracleDbType.NVarchar2, Trim(row.ORDERNO))

        '                                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                                count += 1
        '                            Next
        '                            sqlOrderNo.Append(" ) ")
        '                        End If
        '                    End If

        '                    'SQL文作成
        '                    With sql
        '                        .Append("SELECT /* SC3140103_001 */ ")
        '                        .Append("       VISITSEQ ")
        '                        .Append("     , DLRCD ")
        '                        .Append("     , STRCD ")
        '                        .Append("     , NVL(VISITTIMESTAMP, :MINDATE) AS VISITTIMESTAMP ")
        '                        .Append("     , VCLREGNO ")
        '                        .Append("     , CUSTSEGMENT ")
        '                        .Append("     , DMSID ")
        '                        .Append("     , VIN ")
        '                        .Append("     , MODELCODE ")
        '                        .Append("     , NAME ")
        '                        .Append("     , TELNO ")
        '                        .Append("     , MOBILE ")
        '                        .Append("     , SACODE ")
        '                        .Append("     , NVL(ASSIGNTIMESTAMP, :MINDATE) AS ASSIGNTIMESTAMP ")
        '                        .Append("     , NVL(REZID, :MINREZID) AS REZID ")
        '                        .Append("     , PARKINGCODE ")
        '                        .Append("     , ORDERNO ")
        '                        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 START
        '                        .Append("     , CALLSTATUS ")
        '                        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成 END
        '                        .Append("     , NVL(FREZID, :MINREZID) AS FREZID ")
        '                        .Append("  FROM TBL_SERVICE_VISIT_MANAGEMENT ")
        '                        .Append(" WHERE DLRCD  = :DLRCD ")
        '                        .Append("   AND STRCD  = :STRCD ")
        '                        .Append("   AND SACODE = :SACODE ")
        '                        .Append("   AND ( ")
        '                        .Append("       (NVL(ORDERNO, ' ') = ' ' ")
        '                        .Append("   AND ")
        '                        .Append("       TO_CHAR(:TODAY, 'YYYYMMDD') <= TO_CHAR(VISITTIMESTAMP, 'YYYYMMDD')) ")
        '                        .Append(sqlOrderNo.ToString())
        '                        .Append("   ) ")
        '                    End With

        '                    query.CommandText = sql.ToString()
        '                    'バインド変数

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '                    'query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, account)

        '                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
        '                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
        '                    query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, account)

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


        '                    query.AddParameterWithTypeValue("TODAY", OracleDbType.Date, nowDate)
        '                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, DateTime.MinValue)
        '                    query.AddParameterWithTypeValue("MINREZID", OracleDbType.Int64, MinReserveId)

        '                    '検索結果返却
        '                    dt = query.GetData()
        '                End Using
        '            Catch ex As OracleExceptionEx When ex.Number = 1013
        '                'ORACLEのタイムアウトのみ処理
        '                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
        '                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
        '                                        , Me.GetType.ToString _
        '                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                        , C_RET_DBTIMEOUT _
        '                                        , ex.Message))
        '                Throw ex
        '            End Try

        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_END _
        '                                    , dt.Rows.Count))

        '            Return dt
        '        End Function

        '        '''-------------------------------------------------------
        '        ''' <summary>
        '        ''' サービス来店実績取得（チップ詳細用）
        '        ''' </summary>
        '        ''' <param name="visitSeq">来店実績連番</param>
        '        ''' <returns>サービス来店実績データセット</returns>
        '        ''' <remarks></remarks>
        '        '''-------------------------------------------------------
        '        Public Function GetVisitManagement(ByVal visitSeq As Long) _
        '            As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} IN:viditSeq = {3}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_START _
        '                                    , visitSeq))

        '            Dim dt As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable

        '            Try
        '                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103ServiceVisitManagementDataTable)("SC3140103_002")
        '                    Dim sql As New StringBuilder

        '                    'SQL文作成
        '                    With sql
        '                        .Append("SELECT /* SC3140103_002 */ ")
        '                        .Append("       VISITSEQ ")
        '                        .Append("     , DLRCD ")
        '                        .Append("     , STRCD ")
        '                        .Append("     , NVL(VISITTIMESTAMP, :MINDATE) AS VISITTIMESTAMP ")
        '                        .Append("     , VCLREGNO ")
        '                        .Append("     , CUSTSEGMENT ")
        '                        .Append("     , DMSID ")
        '                        .Append("     , VIN ")
        '                        .Append("     , MODELCODE ")
        '                        .Append("     , NAME ")
        '                        .Append("     , TELNO ")
        '                        .Append("     , MOBILE ")
        '                        .Append("     , SACODE ")
        '                        .Append("     , NVL(ASSIGNTIMESTAMP, :MINDATE) AS ASSIGNTIMESTAMP ")
        '                        .Append("     , NVL(REZID, :MINREZID) AS REZID ")
        '                        .Append("     , PARKINGCODE ")
        '                        .Append("     , ORDERNO ")
        '                        .Append("     , NVL(FREZID, :MINREZID) AS FREZID ")
        '                        .Append("  FROM TBL_SERVICE_VISIT_MANAGEMENT ")
        '                        .Append(" WHERE VISITSEQ = :VISITSEQ ")
        '                    End With

        '                    query.CommandText = sql.ToString()
        '                    'バインド変数
        '                    query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, visitSeq)
        '                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, DateTime.MinValue)
        '                    query.AddParameterWithTypeValue("MINREZID", OracleDbType.Int64, MinReserveId)

        '                    '検索結果返却
        '                    dt = query.GetData()
        '                End Using
        '            Catch ex As OracleExceptionEx When ex.Number = 1013
        '                'ORACLEのタイムアウトのみ処理
        '                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
        '                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
        '                                        , Me.GetType.ToString _
        '                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                        , C_RET_DBTIMEOUT _
        '                                        , ex.Message))
        '                Throw ex
        '            End Try

        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_END _
        '                                    , dt.Rows.Count))

        '            Return dt
        '        End Function

        '        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 START

        '        '''-------------------------------------------------------
        '        ''' <summary>
        '        ''' サービス来店管理情報取得（受付待ちエリア用）
        '        ''' </summary>
        '        ''' <param name="inDealerCode">販売店コード</param>
        '        ''' <param name="inBranchCode">店舗コード</param>
        '        ''' <param name="inPresentTime">現在時間</param>
        '        ''' <returns>受付待ち情報データセット</returns>
        '        ''' <remarks></remarks>
        '        '''-------------------------------------------------------
        '        Public Function GetAssignmentInfo(ByVal inDealerCode As String _
        '                                        , ByVal inBranchCode As String _
        '                                        , ByVal inPresentTime As Date) _
        '                                          As SC3140103DataSet.SC3140103AssignmentInfoDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} IN:PRESENTTIME = {3}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_START _
        '                                    , inPresentTime))

        '            Dim dt As SC3140103DataSet.SC3140103AssignmentInfoDataTable

        '            Try
        '                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AssignmentInfoDataTable)("SC3140103_026")
        '                    Dim sql As New StringBuilder

        '                    'SQL文作成
        '                    With sql

        '                        .AppendLine("  SELECT T1.VISITSEQ ")
        '                        .AppendLine("        ,T1.VISITTIMESTAMP ")
        '                        .AppendLine("        ,TRIM(T1.VCLREGNO) AS VCLREGNO ")
        '                        .AppendLine("        ,TRIM(T1.NAME) AS NAME ")
        '                        .AppendLine("        ,TRIM(T1.ORDERNO) AS ORDERNO ")
        '                        .AppendLine("        ,T1.FREZID AS REZID ")
        '                        .AppendLine("        ,CASE ")
        '                        .AppendLine("              WHEN 0 < T1.FREZID  ")
        '                        .AppendLine("              THEN :REZ_MARK_1 ")
        '                        .AppendLine("              ELSE :REZ_MARK_0 ")
        '                        .AppendLine("          END AS REZ_MARK ")
        '                        .AppendLine("        ,TRIM(T1.PARKINGCODE) AS PARKINGCODE ")
        '                        .AppendLine("        ,T1.UPDATEDATE ")
        '                        .AppendLine("        ,T2.SCHE_START_DATETIME ")
        '                        .AppendLine("        ,NVL(CONCAT(TRIM(T4.UPPER_DISP), TRIM(T4.LOWER_DISP)), NVL(T5.SVC_CLASS_NAME, T5.SVC_CLASS_NAME_ENG)) AS MERCHANDISENAME ")
        '                        .AppendLine("    FROM TBL_SERVICE_VISIT_MANAGEMENT T1 ")
        '                        .AppendLine("        ,(SELECT S2.SVCIN_ID ")
        '                        .AppendLine("                ,MIN(S3.JOB_DTL_ID) AS JOB_DTL_ID ")
        '                        .AppendLine("                ,MIN(S4.SCHE_START_DATETIME) AS SCHE_START_DATETIME ")
        '                        .AppendLine("            FROM TBL_SERVICE_VISIT_MANAGEMENT S1 ")
        '                        .AppendLine("                ,TB_T_SERVICEIN S2 ")
        '                        .AppendLine("                ,TB_T_JOB_DTL S3 ")
        '                        .AppendLine("                ,TB_T_STALL_USE S4 ")
        '                        .AppendLine("           WHERE S1.FREZID = S2.SVCIN_ID ")
        '                        .AppendLine("             AND S2.SVCIN_ID = S3.SVCIN_ID ")
        '                        .AppendLine("             AND S3.JOB_DTL_ID = S4.JOB_DTL_ID ")
        '                        .AppendLine("             AND S1.DLRCD = :DLRCD ")
        '                        .AppendLine("             AND S1.STRCD = :STRCD ")
        '                        .AppendLine("             AND S1.VISITTIMESTAMP ")
        '                        .AppendLine("         BETWEEN TRUNC(:VISITTIMESTAMP) ")
        '                        .AppendLine("             AND TRUNC(:VISITTIMESTAMP) + 86399/86400 ")
        '                        .AppendLine("             AND S1.ASSIGNSTATUS IN (:ASSIGNSTATUS_RECEPTION, :ASSIGNSTATUS_WAIT) ")
        '                        .AppendLine("             AND S2.DLR_CD = :DLRCD ")
        '                        .AppendLine("             AND S2.BRN_CD = :STRCD ")
        '                        .AppendLine("             AND S2.SVC_STATUS <> :STATUS_CANCEL ")
        '                        .AppendLine("             AND S3.DLR_CD = :DLRCD ")
        '                        .AppendLine("             AND S3.BRN_CD = :STRCD ")
        '                        .AppendLine("             AND S3.CANCEL_FLG = :CANCELFLG ")
        '                        .AppendLine("             AND S4.DLR_CD = :DLRCD ")
        '                        .AppendLine("             AND S4.BRN_CD = :STRCD ")
        '                        .AppendLine("        GROUP BY S2.SVCIN_ID) T2 ")
        '                        .AppendLine("        ,TB_T_JOB_DTL T3 ")
        '                        .AppendLine("        ,TB_M_MERCHANDISE T4 ")
        '                        .AppendLine("        ,TB_M_SERVICE_CLASS T5 ")
        '                        .AppendLine("        ,TB_M_VEHICLE T6 ")
        '                        .AppendLine("   WHERE T1.FREZID = T2.SVCIN_ID(+) ")
        '                        .AppendLine("     AND T2.JOB_DTL_ID = T3.JOB_DTL_ID(+) ")
        '                        .AppendLine("     AND T3.MERC_ID = T4.MERC_ID(+) ")
        '                        .AppendLine("     AND T3.SVC_CLASS_ID = T5.SVC_CLASS_ID(+) ")
        '                        .AppendLine("     AND T1.VCL_ID = T6.VCL_ID(+) ")
        '                        .AppendLine("     AND T1.DLRCD = :DLRCD ")
        '                        .AppendLine("     AND T1.STRCD = :STRCD ")
        '                        .AppendLine("     AND T1.VISITTIMESTAMP ")
        '                        .AppendLine(" BETWEEN TRUNC(:VISITTIMESTAMP) ")
        '                        .AppendLine("     AND TRUNC(:VISITTIMESTAMP) + 86399/86400 ")
        '                        .AppendLine("     AND T1.ASSIGNSTATUS IN (:ASSIGNSTATUS_RECEPTION, :ASSIGNSTATUS_WAIT) ")
        '                        .AppendLine("     AND T3.DLR_CD(+) = :DLRCD ")
        '                        .AppendLine("     AND T3.BRN_CD(+) = :STRCD ")
        '                        .AppendLine("ORDER BY T1.VISITTIMESTAMP ASC ")
        '                        .AppendLine("        ,T2.SCHE_START_DATETIME ASC ")

        '                    End With

        '                    'SQL設定
        '                    query.CommandText = sql.ToString()

        '                    'バインド変数
        '                    '販売店コード
        '                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
        '                    '店舗コード
        '                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)
        '                    '予約アイコンフラグ(予約有り)
        '                    query.AddParameterWithTypeValue("REZ_MARK_1", OracleDbType.NVarchar2, GetReserve)
        '                    '予約アイコンフラグ(予約無し)
        '                    query.AddParameterWithTypeValue("REZ_MARK_0", OracleDbType.NVarchar2, NoReserve)
        '                    '来店日時
        '                    query.AddParameterWithTypeValue("VISITTIMESTAMP", OracleDbType.Date, inPresentTime)
        '                    '振当てステータス(0：受付待ち)
        '                    query.AddParameterWithTypeValue("ASSIGNSTATUS_RECEPTION", OracleDbType.NVarchar2, NonAssign)
        '                    '振当てステータス(1：振当待ち)
        '                    query.AddParameterWithTypeValue("ASSIGNSTATUS_WAIT", OracleDbType.NVarchar2, AssignWait)
        '                    'サービスステータス
        '                    query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
        '                    'キャンセルフラグ
        '                    query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.NVarchar2, CancelFlagEffective)

        '                    'SQL実行
        '                    dt = query.GetData()

        '                End Using

        '            Catch ex As OracleExceptionEx When ex.Number = 1013

        '                'ORACLEのタイムアウトのみ処理
        '                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
        '                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
        '                                        , Me.GetType.ToString _
        '                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                        , C_RET_DBTIMEOUT _
        '                                        , ex.Message))

        '                Throw ex
        '            End Try

        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_END _
        '                                    , dt.Rows.Count))

        '            Return dt

        '        End Function

        '        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

        '#End Region

        '#Region " ストール予約取得"

        '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '        ''更新： 2012/05/15 KN 森下 【SERVICE_1】残課題管理表 No.10 代表整備項目の表示対応 START
        '        ' ''' <summary>
        '        ' ''' TBL_DLRENVSETTINGから「*」を取得
        '        ' ''' </summary>
        '        ' ''' <param name="dealerCD">販売店コード</param>
        '        ' ''' <value></value>
        '        ' ''' <returns></returns>
        '        ' ''' <remarks></remarks>
        '        ' ''' <history>
        '        ' ''' </history>
        '        'Private ReadOnly Property BaseTypeAll(ByVal dealerCD As String) As String
        '        '    Get
        '        '        Const BASETYPE_ALL As String = "BASETYPE_ALL"
        '        '        Static value As String
        '        '        If String.IsNullOrEmpty(value) = True Then
        '        '            Dim row As DlrEnvSettingDataSet.DLRENVSETTINGRow = (New DealerEnvSetting).GetEnvSetting(dealerCD, BASETYPE_ALL)
        '        '            value = If(row IsNot Nothing, row.PARAMVALUE, "*")
        '        '        End If
        '        '        Return value
        '        '    End Get
        '        'End Property
        '        ''更新： 2012/05/15 KN 森下 【SERVICE_1】残課題管理表 No.10 代表整備項目の表示対応 END

        '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '        '''-------------------------------------------------------
        '        ''' <summary>
        '        ''' ストール予約取得
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="reserveIdList">初回予約IDリスト</param>
        '        ''' <returns>ストール予約データセット</returns>
        '        ''' <remarks></remarks>
        '        ''' <history>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </history>
        '        '''-------------------------------------------------------
        '        Public Function GetStallReserveInformation(ByVal dealerCode As String, _
        '                                                   ByVal branchCode As String, _
        '                                                   ByVal reserveIdList As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable) _
        '                                                   As SC3140103DataSet.SC3140103StallRezinfoDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_START _
        '                                    , dealerCode _
        '                                    , branchCode))

        '            Dim dt As SC3140103DataSet.SC3140103StallRezinfoDataTable

        '            Try
        '                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103StallRezinfoDataTable)("SC3140103_003")
        '                    Dim sql As New StringBuilder
        '                    Dim sqlReserveId As New StringBuilder
        '                    Dim sqlPreReserveId As New StringBuilder

        '                    ' 初回予約ID用取得文字列
        '                    If Not reserveIdList Is Nothing Then

        '                        If reserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID").Count > 0 Then

        '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                            'sqlReserveId.Append(" AND (T1.REZID IN ( ")
        '                            sqlReserveId.Append(" AND T1.SVCIN_ID IN ( ")

        '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                            sqlPreReserveId.Append(" OR T1.PREZID IN ( ")

        '                            Dim count As Long = 1
        '                            Dim reserveIdName As String
        '                            Dim preReserveIdName As String

        '                            For Each row As SC3140103DataSet.SC3140103ServiceVisitManagementRow In reserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID")

        '                                ' SQL作成
        '                                reserveIdName = String.Format(CultureInfo.CurrentCulture, "REZID{0}", count)
        '                                preReserveIdName = String.Format(CultureInfo.CurrentCulture, "PREZID{0}", count)

        '                                If count > 1 Then

        '                                    sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", reserveIdName))
        '                                    sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", preReserveIdName))
        '                                Else

        '                                    sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", reserveIdName))
        '                                    sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", preReserveIdName))
        '                                End If

        '                                ' パラメータ作成
        '                                query.AddParameterWithTypeValue(reserveIdName, OracleDbType.Int64, row.FREZID)

        '                                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                                'query.AddParameterWithTypeValue(preReserveIdName, OracleDbType.Int64, row.FREZID)

        '                                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                                count += 1
        '                            Next

        '                            sqlReserveId.Append(" ) ")
        '                            sqlPreReserveId.Append(" )) ")

        '                        End If
        '                    End If

        '                    If String.IsNullOrEmpty(sqlReserveId.ToString()) Then

        '                        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                        'sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND T1.REZID = {0} ", MinReserveId))
        '                        sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND T1.SVCIN_ID = {0} ", MinReserveId))

        '                        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


        '                    End If


        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                    ''SQL文作成
        '                    'With sql
        '                    '    .Append("SELECT /* SC3140103_003 */ ")
        '                    '    .Append("       T1.DLRCD ")                                                 ' 販売店コード
        '                    '    .Append("     , T1.STRCD ")                                                 ' 店舗コード
        '                    '    .Append("     , T1.REZID")                                                  ' 予約ID
        '                    '    .Append("     , NVL(T1.PREZID, T1.REZID) AS PREZID ")                       ' 管理予約ID
        '                    '    .Append("     , NVL(T1.STARTTIME, :MINDATE) AS STARTTIME ")                 ' 使用開始日時
        '                    '    .Append("     , NVL(T1.ENDTIME, :MINDATE) AS ENDTIME ")                     ' 使用終了日時 (作業終了予定時刻)
        '                    '    .Append("     , T1.CUSTCD ")                                                ' 顧客コード
        '                    '    .Append("     , T1.CUSTOMERNAME ")                                          ' 氏名
        '                    '    .Append("     , T1.TELNO ")                                                 ' 電話番号
        '                    '    .Append("     , T1.MOBILE ")                                                ' 携帯番号
        '                    '    .Append("     , T1.VEHICLENAME ")                                           ' 車名
        '                    '    .Append("     , T1.VCLREGNO ")                                              ' 登録ナンバー
        '                    '    .Append("     , T1.VIN ")                                                   ' VIN
        '                    '    '更新： 2012/05/15 KN 森下 【SERVICE_1】残課題管理表 No.10 代表整備項目の表示対応 START
        '                    '    '項目名は影響範囲最小に抑えるため元のまま
        '                    '    '.Append("     , T1.MERCHANDISECD ")                                        ' 商品コード
        '                    '    .Append("     , T1.MNTNCD AS MERCHANDISECD ")                               ' 整備コード
        '                    '    '.Append("     , T3.MERCHANDISENAME ")                                      ' 商品名 (代表入庫項目)
        '                    '    .Append("      ,(CASE")
        '                    '    .Append("            WHEN T3.MAINTECD IS NOT NULL THEN T3.MAINTENM ")       ' 整備名称 (代表入庫項目)
        '                    '    .Append("            ELSE T4.MAINTENM ")
        '                    '    .Append("        END) AS MERCHANDISENAME ")
        '                    '    '更新： 2012/05/15 KN 森下 【SERVICE_1】残課題管理表 No.10 代表整備項目の表示対応 END
        '                    '    .Append("     , T1.MODELCODE ")                                             ' モデル
        '                    '    .Append("     , NVL(T1.MILEAGE, -1) AS MILEAGE ")                           ' 走行距離
        '                    '    .Append("     , NVL(T1.WASHFLG, '0') AS WASHFLG ")                          ' 洗車有無
        '                    '    .Append("     , T1.WALKIN ")                                                ' 来店フラグ
        '                    '    .Append("     , T1.REZ_DELI_DATE ")                                         ' 予約_納車_希望日時時刻 (納車予定日時)
        '                    '    .Append("     , NVL(T1.ACTUAL_STIME, :MINDATE) AS ACTUAL_STIME ")           ' 作業開始日時
        '                    '    .Append("     , NVL(T1.ACTUAL_ETIME, :MINDATE) AS ACTUAL_ETIME ")           ' 作業終了日時
        '                    '    .Append("  FROM TBL_STALLREZINFO T1 ")
        '                    '    '更新： 2012/05/15 KN 森下 【SERVICE_1】残課題管理表 No.10 代表整備項目の表示対応 START
        '                    '    '.Append("     , TBL_MERCHANDISEMST T3 ")                        
        '                    '    '.Append(" WHERE T1.DLRCD = T3.DLRCD  (+) ")
        '                    '    '.Append("   AND T1.MERCHANDISECD = T3.MERCHANDISECD (+) ")
        '                    '    .Append("     , TBLORG_MAINTEMASTER T3 ")
        '                    '    .Append("     , TBLORG_MAINTEMASTER T4 ")
        '                    '    .Append(" WHERE T1.DLRCD = T3.DLRCD  (+) ")
        '                    '    .Append("   AND T1.MNTNCD = T3.MAINTECD (+) ")
        '                    '    .Append("   AND :BASETYPEALL = T3.BASETYPE (+) ")
        '                    '    .Append("   AND T1.DLRCD = T4.DLRCD (+) ")
        '                    '    .Append("   AND T1.MNTNCD = T4.MAINTECD (+) ")
        '                    '    .Append("   AND T1.MODELCODE = T4.BASETYPE (+) ")
        '                    '    '更新： 2012/05/15 KN 森下 【SERVICE_1】残課題管理表 No.10 代表整備項目の表示対応 END
        '                    '    .Append("   AND T1.DLRCD = :DLRCD ")
        '                    '    .Append("   AND T1.STRCD = :STRCD ")
        '                    '    .Append(sqlReserveId.ToString())
        '                    '    .Append(sqlPreReserveId.ToString())
        '                    '    .Append("   AND NOT EXISTS ( SELECT 1 ")
        '                    '    .Append("                      FROM TBL_STALLREZINFO T2 ")
        '                    '    .Append("                     WHERE T2.DLRCD = T1.DLRCD ")
        '                    '    .Append("                       AND T2.STRCD = T1.STRCD ")
        '                    '    .Append("                       AND T2.REZID = T1.REZID ")
        '                    '    .Append("                       AND ( (T2.STOPFLG = :STOPFLG0 ")
        '                    '    .Append("                       AND T2.CANCELFLG = :CANCELFLG1) ")
        '                    '    .Append("                        OR T2.REZCHILDNO IN ( :CHILDNOLEAVE, :CHILDNODELIVERY ) ) ) ")

        '                    'End With

        '                    'query.CommandText = sql.ToString()
        '                    ''バインド変数
        '                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '                    'query.AddParameterWithTypeValue("STOPFLG0", OracleDbType.Char, "0")
        '                    'query.AddParameterWithTypeValue("CANCELFLG1", OracleDbType.Char, "1")
        '                    'query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, DateTime.MinValue)
        '                    'query.AddParameterWithTypeValue("CHILDNOLEAVE", OracleDbType.Int64, 0)         ' 子予約連番-0:引取
        '                    'query.AddParameterWithTypeValue("CHILDNODELIVERY", OracleDbType.Int64, 999)    ' 子予約連番-999:納車
        '                    ''更新： 2012/05/15 KN 森下 【SERVICE_1】残課題管理表 No.10 代表整備項目の表示対応 START
        '                    'query.AddParameterWithTypeValue("BASETYPEALL", OracleDbType.NVarchar2, Me.BaseTypeAll(dealerCode))    ' BASETYPE「*」
        '                    ''更新： 2012/05/15 KN 森下 【SERVICE_1】残課題管理表 No.10 代表整備項目の表示対応 END

        '                    'SQL文作成
        '                    With sql

        '                        .AppendLine(" SELECT  /* SC3140103_003 */ ")
        '                        .AppendLine("         TRIM(S1.DLR_CD) AS DLRCD ")
        '                        .AppendLine("        ,TRIM(S1.BRN_CD) AS STRCD ")
        '                        .AppendLine("        ,S1.SVCIN_ID AS REZID ")
        '                        .AppendLine("        ,S1.SVCIN_ID AS PREZID ")
        '                        .AppendLine("        ,DECODE(S1.SVCIN_MILE, 0, -1, S1.SVCIN_MILE) AS MILEAGE ")
        '                        .AppendLine("        ,DECODE(TRIM(S1.ACCEPTANCE_TYPE), NULL, '0', S1.ACCEPTANCE_TYPE) AS WALKIN ")
        '                        .AppendLine("        ,DECODE(TRIM(S1.CARWASH_NEED_FLG), NULL, '0', S1.CARWASH_NEED_FLG) AS WASHFLG ")
        '                        .AppendLine("        ,DECODE(S1.SCHE_DELI_DATETIME, '190001010000', NULL, S1.SCHE_DELI_DATETIME) AS REZ_DELI_DATE ")
        '                        .AppendLine("        ,TRIM(S1.MAINTE_CD) AS MERCHANDISECD ")
        '                        .AppendLine("        ,DECODE(S1.SCHE_START_DATETIME, :MINDATE, :MIN, S1.SCHE_START_DATETIME) AS STARTTIME ")
        '                        .AppendLine("        ,DECODE(S1.SCHE_END_DATETIME, :MINDATE, :MIN, S1.SCHE_END_DATETIME) AS ENDTIME ")
        '                        .AppendLine("        ,DECODE(S1.RSLT_START_DATETIME, :MINDATE, :MIN, S1.RSLT_START_DATETIME) AS ACTUAL_STIME ")
        '                        .AppendLine("        ,DECODE(S1.RSLT_END_DATETIME, :MINDATE, :MIN, S1.RSLT_END_DATETIME) AS ACTUAL_ETIME ")
        '                        .AppendLine("        ,TRIM(S1.DMS_CST_CD) AS CUSTCD ")
        '                        .AppendLine("        ,TRIM(S1.CST_NAME) AS CUSTOMERNAME ")
        '                        .AppendLine("        ,TRIM(S1.CST_PHONE) AS TELNO ")
        '                        .AppendLine("        ,TRIM(S1.CST_MOBILE) AS MOBILE ")
        '                        .AppendLine("        ,TRIM(S1.REG_NUM) AS VCLREGNO ")
        '                        .AppendLine("        ,TRIM(S1.VCL_VIN) AS VIN ")
        '                        .AppendLine("        ,TRIM(S1.VCL_KATASHIKI) AS MODELCODE ")
        '                        .AppendLine("        ,TRIM(S1.MODEL_NAME) AS VEHICLENAME ")
        '                        .AppendLine("        ,DECODE(TRIM(T8.MAINTECD), NULL, T9.MAINTENM, T8.MAINTENM) AS MERCHANDISENAME ")
        '                        .AppendLine("   FROM          ")
        '                        .AppendLine("         (SELECT ")
        '                        .AppendLine("                  T1.DLR_CD ")
        '                        .AppendLine("                 ,T1.BRN_CD ")
        '                        .AppendLine("                 ,T1.SVCIN_ID ")
        '                        .AppendLine("                 ,T1.SVCIN_MILE ")
        '                        .AppendLine("                 ,T1.ACCEPTANCE_TYPE ")
        '                        .AppendLine("                 ,T1.CARWASH_NEED_FLG ")
        '                        .AppendLine("                 ,TO_CHAR(T1.SCHE_DELI_DATETIME, 'YYYYMMDDHH24MI') AS SCHE_DELI_DATETIME ")
        '                        .AppendLine("                 ,T2.MAINTE_CD ")
        '                        .AppendLine("                 ,T3.SCHE_START_DATETIME ")
        '                        .AppendLine("                 ,T3.SCHE_END_DATETIME ")
        '                        .AppendLine("                 ,T3.RSLT_START_DATETIME ")
        '                        .AppendLine("                 ,T3.RSLT_END_DATETIME ")
        '                        .AppendLine("                 ,T4.DMS_CST_CD ")
        '                        .AppendLine("                 ,T4.CST_NAME ")
        '                        .AppendLine("                 ,T4.CST_PHONE ")
        '                        .AppendLine("                 ,T4.CST_MOBILE ")
        '                        .AppendLine("                 ,T5.REG_NUM ")
        '                        .AppendLine("                 ,T6.VCL_VIN ")
        '                        .AppendLine("                 ,T6.VCL_KATASHIKI ")
        '                        .AppendLine("                 ,T7.MODEL_NAME ")
        '                        .AppendLine("            FROM  TB_T_SERVICEIN T1 ")
        '                        .AppendLine("                 ,TB_T_JOB_DTL T2 ")
        '                        .AppendLine("                 ,TB_T_STALL_USE T3 ")
        '                        .AppendLine("                 ,TB_M_CUSTOMER T4 ")
        '                        .AppendLine("                 ,TB_M_VEHICLE_DLR T5 ")
        '                        .AppendLine("                 ,TB_M_VEHICLE T6 ")
        '                        .AppendLine("                 ,TB_M_MODEL T7 ")
        '                        .AppendLine("           WHERE  T1.SVCIN_ID = T2.SVCIN_ID ")
        '                        .AppendLine("             AND  T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
        '                        .AppendLine("             AND  T1.CST_ID = T4.CST_ID (+) ")
        '                        .AppendLine("             AND  T1.DLR_CD = T5.DLR_CD (+) ")
        '                        .AppendLine("             AND  T1.VCL_ID = T5.VCL_ID (+) ")
        '                        .AppendLine("             AND  T1.VCL_ID = T6.VCL_ID (+) ")
        '                        .AppendLine("             AND  T6.MODEL_CD = T7.MODEL_CD (+) ")
        '                        .AppendLine("             AND  T1.DLR_CD = :DLR_CD ")
        '                        .AppendLine("             AND  T1.BRN_CD = :BRN_CD ")
        '                        .AppendLine(sqlReserveId.ToString())
        '                        .AppendLine("             AND  T2.DLR_CD = :DLR_CD ")
        '                        .AppendLine("             AND  T2.BRN_CD = :BRN_CD ")
        '                        .AppendLine("             AND  T3.DLR_CD = :DLR_CD ")
        '                        .AppendLine("             AND  T3.BRN_CD = :BRN_CD ")
        '                        .AppendLine("             AND  NOT EXISTS (SELECT 1 ")
        '                        .AppendLine("                                FROM TB_T_SERVICEIN D1 ")
        '                        .AppendLine("                               WHERE D1.SVCIN_ID = T1.SVCIN_ID ")
        '                        .AppendLine("                                 AND D1.SVC_STATUS = :STATUS_CANCEL) ")
        '                        .AppendLine("             AND  T2.CANCEL_FLG = :CANCEL_FLG ")
        '                        .AppendLine("             AND  T7.INUSE_FLG(+) = :INUSE_FLG ")
        '                        .AppendLine("          ) S1 ")
        '                        .AppendLine("         ,TBLORG_MAINTEMASTER T8 ")
        '                        .AppendLine("         ,TBLORG_MAINTEMASTER T9 ")
        '                        .AppendLine("  WHERE  S1.DLR_CD = T8.DLRCD (+) ")
        '                        .AppendLine("    AND  S1.MAINTE_CD = T8.MAINTECD (+) ")
        '                        .AppendLine("    AND  :BASETYPEALL = T8.BASETYPE (+)    ")
        '                        .AppendLine("    AND  S1.DLR_CD = T9.DLRCD (+) ")
        '                        .AppendLine("    AND  S1.MAINTE_CD = T9.MAINTECD (+) ")
        '                        .AppendLine("    AND  SUBSTR(S1.VCL_KATASHIKI, 0, INSTR(S1.VCL_KATASHIKI, :HYPHEN) -1) = T9.BASETYPE (+) ")

        '                    End With

        '                    'パラメータの設定

        '                    '日付省略値
        '                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
        '                    query.AddParameterWithTypeValue("MIN", OracleDbType.Date, Date.MinValue)
        '                    '販売店コード
        '                    query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
        '                    '店舗コード
        '                    query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
        '                    'サービスステータス
        '                    query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
        '                    'キャンセルフラグ
        '                    query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)
        '                    '使用中フラグ
        '                    query.AddParameterWithTypeValue("INUSE_FLG", OracleDbType.NVarchar2, InUse)
        '                    'BASETYPE「*」
        '                    query.AddParameterWithTypeValue("BASETYPEALL", OracleDbType.NVarchar2, BaseTypeAll)
        '                    '基本型式検索用(ハイフン)
        '                    query.AddParameterWithTypeValue("HYPHEN", OracleDbType.NVarchar2, Hyphen)

        '                    'SQL格納
        '                    query.CommandText = sql.ToString()

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                    '検索結果返却
        '                    dt = query.GetData()

        '                End Using

        '            Catch ex As OracleExceptionEx When ex.Number = 1013

        '                'ORACLEのタイムアウトのみ処理
        '                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
        '                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
        '                                        , Me.GetType.ToString _
        '                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                        , C_RET_DBTIMEOUT _
        '                                        , ex.Message))
        '                Throw ex
        '            End Try

        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_END _
        '                                    , dt.Rows.Count))

        '            Return dt
        '        End Function


        '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '        ''' <summary>
        '        ''' ストール予約の最新情報取得
        '        ''' </summary>
        '        ''' <param name="inRezID">予約ID</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        ''' <history>
        '        ''' </history>
        '        Public Function GetDBNewestStallRezInfo(ByVal inRezId As Long) _
        '                                                As SC3140103DataSet.SC3140103NewestStallRezInfoDataTable

        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                        , "{0}.{1} REZID:{2}" _
        '                        , Me.GetType.ToString _
        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                        , inRezId))

        '            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103NewestStallRezInfoDataTable)("SC3140103_024")

        '                Dim sql As New StringBuilder      ' SQL文格納

        '                With sql

        '                    .AppendLine(" SELECT /* SC3140103_024 */ ")
        '                    .AppendLine("        ROW_LOCK_VERSION AS ROW_LOCK_VERSION ")
        '                    .AppendLine("   FROM ")
        '                    .AppendLine("        TB_T_SERVICEIN T1 ")
        '                    .AppendLine("  WHERE ")
        '                    .AppendLine("        T1.SVCIN_ID = :REZID ")

        '                End With

        '                'SQL格納
        '                query.CommandText = sql.ToString()


        '                'バインド変数
        '                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, inRezId)


        '                '実行
        '                Dim dt As SC3140103DataSet.SC3140103NewestStallRezInfoDataTable = query.GetData()

        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                            , "{0}.{1} END COUNT = {2}" _
        '                            , Me.GetType.ToString _
        '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                            , dt.Count))

        '                Return dt

        '            End Using
        '        End Function


        '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


        '#End Region

        '#Region " ストール実績取得"

        '        '''-------------------------------------------------------
        '        ''' <summary>
        '        ''' ストール実績取得
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="reserveIdList">初回予約IDリスト</param>
        '        ''' <returns>ストール実績データセット</returns>
        '        ''' <remarks></remarks>
        '        ''' <history>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </history>
        '        '''-------------------------------------------------------
        '        Public Function GetStallProcess(ByVal dealerCode As String, _
        '                                        ByVal branchCode As String, _
        '                                        ByVal reserveIdList As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable) _
        '                                        As SC3140103DataSet.SC3140103StallProcessDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_START _
        '                                    , dealerCode _
        '                                    , branchCode))

        '            Dim dt As SC3140103DataSet.SC3140103StallProcessDataTable

        '            Try
        '                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103StallProcessDataTable)("SC3140103_004")

        '                    Dim sql As New StringBuilder
        '                    Dim sqlReserveId As New StringBuilder
        '                    Dim sqlPreReserveId As New StringBuilder

        '                    ' 初回予約ID用取得文字列
        '                    If Not reserveIdList Is Nothing Then

        '                        If reserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID").Count > 0 Then

        '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                            'sqlReserveId.Append(" AND (T1.REZID IN ( ")
        '                            sqlReserveId.Append(" AND T1.SVCIN_ID IN ( ")

        '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                            sqlPreReserveId.Append(" OR R.PREZID IN ( ")

        '                            Dim count As Long = 1
        '                            Dim reserveIdName As String
        '                            Dim preReserveIdName As String

        '                            For Each row As SC3140103DataSet.SC3140103ServiceVisitManagementRow In reserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID")

        '                                ' SQL作成
        '                                reserveIdName = String.Format(CultureInfo.CurrentCulture, "REZID{0}", count)
        '                                preReserveIdName = String.Format(CultureInfo.CurrentCulture, "PREZID{0}", count)

        '                                If count > 1 Then

        '                                    sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", reserveIdName))
        '                                    sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", preReserveIdName))
        '                                Else

        '                                    sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", reserveIdName))
        '                                    sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", preReserveIdName))
        '                                End If

        '                                ' パラメータ作成
        '                                query.AddParameterWithTypeValue(reserveIdName, OracleDbType.Int64, row.FREZID)

        '                                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                                'query.AddParameterWithTypeValue(preReserveIdName, OracleDbType.Int64, row.FREZID)

        '                                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                                count += 1
        '                            Next

        '                            sqlReserveId.Append(" ) ")
        '                            sqlPreReserveId.Append(" )) ")

        '                        End If
        '                    End If

        '                    If String.IsNullOrEmpty(sqlReserveId.ToString()) Then


        '                        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                        'sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND R.REZID = {0} ", MinReserveId))
        '                        sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND T1.SVCIN_ID = {0} ", MinReserveId))

        '                        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


        '                    End If

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                    ''SQL文作成
        '                    'With sql
        '                    '    .AppendLine(" SELECT /* SC3140103_004 */ ")
        '                    '    .AppendLine("        MAX(DLRCD)                            AS DLRCD ")
        '                    '    .AppendLine("      , MAX(STRCD)                            AS STRCD ")
        '                    '    .AppendLine("      , PREZID                                AS PREZID ")
        '                    '    .AppendLine("      , MAX(WASHFLG)                          AS WASHFLG ")
        '                    '    .AppendLine("      , MAX(DECODE(NUM,1,RESULT_STATUS,NULL)) AS RESULT_STATUS ")
        '                    '    .AppendLine("      , MAX(REZ_END_TIME)                     AS REZ_END_TIME ")
        '                    '    .AppendLine("      , MAX(RESULT_WASH_START)                AS RESULT_WASH_START ")
        '                    '    .AppendLine("      , MAX(RESULT_WASH_END)                  AS RESULT_WASH_END ")
        '                    '    .AppendLine("      , MAX(DECODE(NUM,1,STAFFCD,NULL))       AS STAFFCD ")
        '                    '    .AppendLine("      , MAX(DECODE(NUM,1,USERNAME,NULL))      AS STAFFNAME ")
        '                    '    .AppendLine("      , SUM(DECODE(NVL(STOPFLG,0),0,0,1))     AS UNVALID_REZ_COUNT ")
        '                    '    .AppendLine("      , MIN(ACTUAL_STIME)                     AS FIRST_STARTTIME ")
        '                    '    .AppendLine("      , MAX(NVL2(RESULT_END_TIME ")
        '                    '    .AppendLine("               , TO_DATE(RESULT_END_TIME,'YYYYMMDDHH24MI') ")
        '                    '    .AppendLine("               , ENDTIME))                    AS LAST_ENDTIME ")
        '                    '    .AppendLine("      , SUM(DECODE(RESULT_STATUS,'0' ,REZ_WORK_TIME, ")
        '                    '    .AppendLine("            DECODE(RESULT_STATUS,'00',REZ_WORK_TIME, ")
        '                    '    .AppendLine("            DECODE(RESULT_STATUS,'10',REZ_WORK_TIME, ")
        '                    '    .AppendLine("            DECODE(RESULT_STATUS,NULL,REZ_WORK_TIME,0))))) AS WORK_TIME ")
        '                    '    .AppendLine("      , DECODE(MIN(NUM),0,'0','1')                         AS RESULT_TYPE ")
        '                    '    .AppendLine("   FROM ")
        '                    '    .AppendLine("     (SELECT ")
        '                    '    .AppendLine("             R.DLRCD ")
        '                    '    .AppendLine("           , R.STRCD ")
        '                    '    .AppendLine("           , R.REZID ")
        '                    '    .AppendLine("           , P.SEQNO ")
        '                    '    .AppendLine("           , P.DSEQNO ")
        '                    '    .AppendLine("           , R.WASHFLG ")
        '                    '    .AppendLine("           , P.RESULT_STATUS ")
        '                    '    .AppendLine("           , P.REZ_END_TIME ")
        '                    '    .AppendLine("           , P.RESULT_WASH_START ")
        '                    '    .AppendLine("           , P.RESULT_WASH_END ")
        '                    '    .AppendLine("           , NVL(R.PREZID,R.REZID) AS PREZID ")
        '                    '    .AppendLine("           , T3.STAFFCD ")
        '                    '    .AppendLine("           , T5.USERNAME ")
        '                    '    .AppendLine("           , R.REZ_WORK_TIME ")
        '                    '    .AppendLine("           , R.STOPFLG ")
        '                    '    .AppendLine("           , R.ENDTIME ")
        '                    '    .AppendLine("           , P.RESULT_END_TIME ")
        '                    '    .AppendLine("           , R.ACTUAL_STIME ")
        '                    '    .AppendLine("           , ROW_NUMBER() OVER ( ")
        '                    '    .AppendLine("                 PARTITION BY NVL(R.PREZID, R.REZID) ")
        '                    '    .AppendLine("                 ORDER BY ENDTIME DESC ")
        '                    '    .AppendLine("                        , STARTTIME DESC ")
        '                    '    .AppendLine("                        , USERNAME ASC) NUM ")
        '                    '    .AppendLine("        FROM TBL_STALLPROCESS P ")
        '                    '    .AppendLine("           , TBL_STALLREZINFO R ")
        '                    '    .AppendLine("           , TBL_TSTAFFSTALL T3 ")
        '                    '    .AppendLine("           , TBL_SSTAFF T4 ")
        '                    '    .AppendLine("           , TBL_USERS T5 ")
        '                    '    .AppendLine("       WHERE R.DLRCD = P.DLRCD ")
        '                    '    .AppendLine("         AND R.STRCD = P.STRCD ")
        '                    '    .AppendLine("         AND R.REZID = P.REZID ")
        '                    '    .AppendLine("         AND P.DLRCD = T3.DLRCD (+) ")
        '                    '    .AppendLine("         AND P.STRCD = T3.STRCD (+) ")
        '                    '    .AppendLine("         AND P.REZID = T3.REZID (+) ")
        '                    '    .AppendLine("         AND P.SEQNO = T3.SEQNO (+) ")
        '                    '    .AppendLine("         AND P.DSEQNO = T3.DSEQNO (+) ")
        '                    '    .AppendLine("         AND T3.DLRCD = T4.DLRCD (+) ")
        '                    '    .AppendLine("         AND T3.STRCD = T4.STRCD (+) ")
        '                    '    .AppendLine("         AND T3.STAFFCD = T4.STAFFCD (+) ")
        '                    '    .AppendLine("         AND T4.DLRCD = T5.DLRCD (+) ")
        '                    '    .AppendLine("         AND T4.STRCD = T5.STRCD (+) ")
        '                    '    .AppendLine("         AND T4.ACCOUNT = T5.ACCOUNT (+) ")
        '                    '    .AppendLine("         AND R.DLRCD = :DLRCD ")
        '                    '    .AppendLine("         AND R.STRCD = :STRCD ")
        '                    '    .Append(sqlReserveId.ToString())
        '                    '    .Append(sqlPreReserveId.ToString())
        '                    '    .AppendLine("         AND DECODE(R.STOPFLG,'0',DECODE(R.CANCELFLG,'1',1,0),0) + ")
        '                    '    .AppendLine("             DECODE(R.REZCHILDNO,0,1,0) + ")
        '                    '    .AppendLine("             DECODE(R.REZCHILDNO,999,1,0) = 0 ")
        '                    '    .AppendLine("         AND P.SEQNO = ( ")
        '                    '    .AppendLine("             SELECT MAX(SEQNO) ")
        '                    '    .AppendLine("               FROM TBL_STALLPROCESS ")
        '                    '    .AppendLine("              WHERE P.DLRCD = DLRCD ")
        '                    '    .AppendLine("                AND P.STRCD = STRCD ")
        '                    '    .AppendLine("                AND P.REZID = REZID ")
        '                    '    .AppendLine("                AND P.DSEQNO = DSEQNO) ")
        '                    '    .AppendLine("         AND P.DSEQNO = ( ")
        '                    '    .AppendLine("             SELECT MAX(DSEQNO) ")
        '                    '    .AppendLine("               FROM TBL_STALLPROCESS ")
        '                    '    .AppendLine("              WHERE P.DLRCD = DLRCD ")
        '                    '    .AppendLine("                AND P.STRCD = STRCD ")
        '                    '    .AppendLine("                AND P.REZID = REZID) ")
        '                    '    .AppendLine("       UNION ALL ")
        '                    '    .AppendLine("      SELECT  ")
        '                    '    .AppendLine("             R.DLRCD ")
        '                    '    .AppendLine("           , R.STRCD ")
        '                    '    .AppendLine("           , R.REZID ")
        '                    '    .AppendLine("           , NULL ")
        '                    '    .AppendLine("           , NULL ")
        '                    '    .AppendLine("           , R.WASHFLG ")
        '                    '    .AppendLine("           , NULL ")
        '                    '    .AppendLine("           , NULL ")
        '                    '    .AppendLine("           , NULL ")
        '                    '    .AppendLine("           , NULL ")
        '                    '    .AppendLine("           , NVL(R.PREZID,R.REZID) AS PREZID ")
        '                    '    .AppendLine("           , NULL ")
        '                    '    .AppendLine("           , NULL ")
        '                    '    .AppendLine("           , R.REZ_WORK_TIME ")
        '                    '    .AppendLine("           , R.STOPFLG ")
        '                    '    .AppendLine("           , R.ENDTIME ")
        '                    '    .AppendLine("           , NULL ")
        '                    '    .AppendLine("           , R.ACTUAL_STIME ")
        '                    '    .AppendLine("           , 0 AS NUM ")
        '                    '    .AppendLine("        FROM TBL_STALLREZINFO R ")
        '                    '    .AppendLine("       WHERE R.DLRCD = :DLRCD ")
        '                    '    .AppendLine("         AND R.STRCD = :STRCD ")
        '                    '    .AppendLine(sqlReserveId.ToString())
        '                    '    .AppendLine(sqlPreReserveId.ToString())
        '                    '    .AppendLine("         AND DECODE(R.STOPFLG,'0',DECODE(R.CANCELFLG,'1',1,0),0) + ")
        '                    '    .AppendLine("             DECODE(R.REZCHILDNO,0,1,0) + ")
        '                    '    .AppendLine("             DECODE(R.REZCHILDNO,999,1,0) = 0 ")
        '                    '    .AppendLine("         AND NOT EXISTS ( ")
        '                    '    .AppendLine("                 SELECT '1' ")
        '                    '    .AppendLine("                   FROM TBL_STALLPROCESS ")
        '                    '    .AppendLine("                  WHERE R.DLRCD = DLRCD ")
        '                    '    .AppendLine("                    AND R.STRCD = STRCD ")
        '                    '    .AppendLine("                    AND R.REZID = REZID)  ")
        '                    '    .AppendLine("     ) M ")
        '                    '    .AppendLine(" GROUP BY M.PREZID ")
        '                    'End With

        '                    'query.CommandText = sql.ToString()
        '                    ''バインド変数
        '                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '                    ''query.AddParameterWithTypeValue("CHILDNOLEAVE", OracleDbType.Int64, 0)         ' 子予約連番-0:引取
        '                    ''query.AddParameterWithTypeValue("CHILDNODELIVERY", OracleDbType.Int64, 999)    ' 子予約連番-999:納車


        '                    'SQL文作成
        '                    With sql

        '                        .AppendLine("   SELECT  /* SC3140103_004 */ ")
        '                        .AppendLine("           MAX(TRIM(P1.DLR_CD)) AS DLRCD ")
        '                        .AppendLine("          ,MAX(TRIM(P1.BRN_CD)) AS STRCD ")
        '                        .AppendLine("          ,MAX(P1.SVCIN_ID) AS PREZID ")
        '                        .AppendLine("          ,MAX(DECODE(P1.CARWASH_NEED_FLG, ' ', '0', P1.CARWASH_NEED_FLG)) AS WASHFLG ")
        '                        .AppendLine("          ,MAX(DECODE(P1.NUM, 1, P1.SVC_STATUS, '0')) AS RESULT_STATUS ")
        '                        .AppendLine("          ,DECODE(MAX(P1.RESULT_WASH_START), :MINDATE, NULL, TO_CHAR(MAX(P1.RESULT_WASH_START), 'YYYYMMDDHH24MI')) AS RESULT_WASH_START ")
        '                        .AppendLine("          ,DECODE(MAX(P1.RESULT_WASH_END), :MINDATE, NULL, TO_CHAR(MAX(P1.RESULT_WASH_END), 'YYYYMMDDHH24MI')) AS RESULT_WASH_END ")
        '                        .AppendLine("          ,SUM(DECODE(P1.STALL_USE_STATUS, '05', 1, 0)) AS UNVALID_REZ_COUNT ")
        '                        .AppendLine("          ,DECODE(MAX(P1.SCHE_END_DATETIME), :MINDATE, NULL, TO_CHAR(MAX(P1.SCHE_END_DATETIME), 'YYYYMMDDHH24MI')) AS REZ_END_TIME ")
        '                        .AppendLine("          ,MIN(DECODE(P1.RSLT_START_DATETIME, :MINDATE, TO_DATE(NULL), P1.RSLT_START_DATETIME)) AS FIRST_STARTTIME ")
        '                        .AppendLine("          ,MAX(CASE ")
        '                        .AppendLine("               WHEN P1.RSLT_END_DATETIME <> :MINDATE THEN P1.RSLT_END_DATETIME ")
        '                        .AppendLine("               WHEN P1.PRMS_END_DATETIME <> :MINDATE THEN P1.PRMS_END_DATETIME ")
        '                        .AppendLine("               ELSE P1.SCHE_END_DATETIME END) AS LAST_ENDTIME ")
        '                        .AppendLine("          ,SUM(DECODE(P1.SVC_STATUS,'00',P1.SCHE_WORKTIME, ")
        '                        .AppendLine("               DECODE(P1.SVC_STATUS,'10',P1.SCHE_WORKTIME,0))) AS WORK_TIME ")
        '                        .AppendLine("          ,MAX(DECODE(P1.NUM, 1, P1.STF_CD, NULL)) AS STAFFCD ")
        '                        .AppendLine("          ,MAX(DECODE(P1.NUM, 1, P1.USERNAME, NULL)) AS STAFFNAME ")
        '                        .AppendLine("          ,DECODE(MIN(P1.NUM), 0, '0', '1') AS RESULT_TYPE  ")
        '                        .AppendLine("     FROM          ")
        '                        .AppendLine("           (SELECT ")
        '                        .AppendLine("                    T1.DLR_CD ")
        '                        .AppendLine("                   ,T1.BRN_CD ")
        '                        .AppendLine("                   ,T1.SVCIN_ID ")
        '                        .AppendLine("                   ,T1.CARWASH_NEED_FLG ")
        '                        .AppendLine("                   ,T1.SVC_STATUS ")
        '                        .AppendLine("                   ,T2.RSLT_START_DATETIME AS RESULT_WASH_START ")
        '                        .AppendLine("                   ,T2.RSLT_END_DATETIME AS RESULT_WASH_END ")
        '                        .AppendLine("                   ,T4.STALL_USE_STATUS ")
        '                        .AppendLine("                   ,T4.SCHE_END_DATETIME ")
        '                        .AppendLine("                   ,T4.RSLT_START_DATETIME ")
        '                        .AppendLine("                   ,T4.PRMS_END_DATETIME ")
        '                        .AppendLine("                   ,T4.RSLT_END_DATETIME ")
        '                        .AppendLine("                   ,T4.SCHE_WORKTIME ")
        '                        .AppendLine("                   ,T5.STF_CD ")
        '                        .AppendLine("                   ,T6.USERNAME ")
        '                        .AppendLine("                   ,ROW_NUMBER() OVER (PARTITION BY T1.SVCIN_ID ")
        '                        .AppendLine("                                           ORDER BY T4.SCHE_END_DATETIME DESC ")
        '                        .AppendLine("                                                   ,T4.SCHE_START_DATETIME DESC ")
        '                        .AppendLine("                                                   ,T6.USERNAME ASC ")
        '                        .AppendLine("                                       ) AS NUM ")
        '                        .AppendLine("              FROM  TB_T_SERVICEIN T1 ")
        '                        .AppendLine("                   ,TB_T_CARWASH_RESULT T2 ")
        '                        .AppendLine("                   ,TB_T_JOB_DTL T3 ")
        '                        .AppendLine("                   ,TB_T_STALL_USE T4 ")
        '                        .AppendLine("                   ,TB_T_STAFF_JOB T5 ")
        '                        .AppendLine("                   ,TBL_USERS T6 ")
        '                        .AppendLine("             WHERE  T1.SVCIN_ID = T2.SVCIN_ID (+)  ")
        '                        .AppendLine("               AND  T1.SVCIN_ID = T3.SVCIN_ID ")
        '                        .AppendLine("               AND  T3.JOB_DTL_ID = T4.JOB_DTL_ID ")
        '                        .AppendLine("               AND  T4.JOB_ID = T5.JOB_ID (+) ")
        '                        .AppendLine("               AND  T5.STF_CD = T6.ACCOUNT (+) ")
        '                        .AppendLine(sqlReserveId.ToString())
        '                        .AppendLine("               AND  NOT EXISTS (SELECT 1 ")
        '                        .AppendLine("                                  FROM TB_T_SERVICEIN D1 ")
        '                        .AppendLine("                                 WHERE D1.SVCIN_ID = T1.SVCIN_ID ")
        '                        .AppendLine("                                   AND D1.SVC_STATUS = :STATUS_CANCEL) ")
        '                        .AppendLine("               AND  T3.CANCEL_FLG = :CANCEL_FLG ")
        '                        .AppendLine("               AND  T4.DLR_CD = :DLR_CD ")
        '                        .AppendLine("               AND  T4.BRN_CD = :BRN_CD ")
        '                        .AppendLine("               AND  T6.DLRCD(+) = :DLRCD ")
        '                        .AppendLine("               AND  T6.STRCD(+) = :STRCD ")
        '                        .AppendLine("            ) P1 ")
        '                        .AppendLine(" GROUP BY  P1.SVCIN_ID ")

        '                    End With

        '                    'パラメータ設定

        '                    '日付省略値
        '                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
        '                    'サービスステータス
        '                    query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
        '                    'キャンセルフラグ
        '                    query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)
        '                    '販売店コード
        '                    query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
        '                    '店舗コード
        '                    query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
        '                    '販売店コード(CHAR)
        '                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                    '店舗コード(CHAR)
        '                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)

        '                    'SQL格納
        '                    query.CommandText = sql.ToString()

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                    '検索結果返却
        '                    dt = query.GetData()

        '                End Using

        '            Catch ex As OracleExceptionEx When ex.Number = 1013

        '                'ORACLEのタイムアウトのみ処理
        '                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
        '                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
        '                                        , Me.GetType.ToString _
        '                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                        , C_RET_DBTIMEOUT _
        '                                        , ex.Message))
        '                Throw ex
        '            End Try

        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_END _
        '                                    , dt.Rows.Count))

        '            Return dt
        '        End Function

        '        '''-------------------------------------------------------
        '        ''' <summary>
        '        ''' ストール実績取得(チップ詳細用)
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="reserveIdList">初回予約IDリスト</param>
        '        ''' <returns>ストール実績データセット</returns>
        '        ''' <remarks></remarks>
        '        ''' <Hisgtory>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </Hisgtory>
        '        '''-------------------------------------------------------
        '        Public Function GetStallProcessDetail(ByVal dealerCode As String, _
        '                                              ByVal branchCode As String, _
        '                                              ByVal reserveIdList As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable) _
        '                                              As SC3140103DataSet.SC3140103StallProcessDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_START _
        '                                    , dealerCode _
        '                                    , branchCode))

        '            Dim dt As SC3140103DataSet.SC3140103StallProcessDataTable

        '            Try
        '                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103StallProcessDataTable)("SC3140103_005")

        '                    Dim sql As New StringBuilder
        '                    Dim sqlReserveId As New StringBuilder
        '                    Dim sqlPreReserveId As New StringBuilder

        '                    ' 初回予約ID用取得文字列
        '                    If Not reserveIdList Is Nothing Then

        '                        If reserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID").Count > 0 Then

        '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                            'sqlReserveId.Append(" AND (T1.REZID IN ( ")
        '                            sqlReserveId.Append(" AND T1.SVCIN_ID IN ( ")

        '                            '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                            sqlPreReserveId.Append(" OR T1.PREZID IN ( ")

        '                            Dim count As Long = 1
        '                            Dim reserveIdName As String
        '                            Dim preReserveIdName As String

        '                            For Each row As SC3140103DataSet.SC3140103ServiceVisitManagementRow In reserveIdList.Select(String.Format(CultureInfo.CurrentCulture, "FREZID > {0}", MinReserveId), "FREZID")

        '                                ' SQL作成
        '                                reserveIdName = String.Format(CultureInfo.CurrentCulture, "REZID{0}", count)
        '                                preReserveIdName = String.Format(CultureInfo.CurrentCulture, "PREZID{0}", count)

        '                                If count > 1 Then

        '                                    sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", reserveIdName))
        '                                    sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", preReserveIdName))
        '                                Else

        '                                    sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", reserveIdName))
        '                                    sqlPreReserveId.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", preReserveIdName))
        '                                End If

        '                                ' パラメータ作成
        '                                query.AddParameterWithTypeValue(reserveIdName, OracleDbType.Int64, row.FREZID)


        '                                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                                'query.AddParameterWithTypeValue(preReserveIdName, OracleDbType.Int64, row.FREZID)

        '                                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                                count += 1

        '                            Next

        '                            sqlReserveId.Append(" ) ")
        '                            sqlPreReserveId.Append(" )) ")

        '                        End If
        '                    End If

        '                    If String.IsNullOrEmpty(sqlReserveId.ToString()) Then

        '                        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                        'sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND T1.REZID = {0} ", MinReserveId))
        '                        sqlReserveId.Append(String.Format(CultureInfo.CurrentCulture, " AND T1.SVCIN_ID = {0} ", MinReserveId))

        '                        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                    End If

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                    ''SQL文作成
        '                    'With sql
        '                    '    .Append("SELECT /* SC3140103_005 */ ")
        '                    '    .Append("       T1.DLRCD ")                             ' 販売店コード
        '                    '    .Append("     , T1.STRCD ")                             ' 店舗コード
        '                    '    .Append("     , T1.REZID ")                             ' 予約ID
        '                    '    .Append("     , NVL(T1.PREZID, T1.REZID) AS PREZID ")   ' 管理予約ID
        '                    '    .Append("     , T2.DSEQNO ")                            ' 日跨ぎシーケンス番号
        '                    '    .Append("     , T2.SEQNO ")                             ' シーケンス番号
        '                    '    .Append("     , NVL(T1.WASHFLG, '0') AS WASHFLG ")      ' 洗車有無
        '                    '    .Append("     , T2.RESULT_STATUS ")                     ' 実績_ステータス
        '                    '    .Append("     , T2.REZ_END_TIME ")                      ' 予定_ストール終了日時時刻 (納車予定日時)
        '                    '    .Append("     , T2.RESULT_WASH_START ")                 ' 洗車開始
        '                    '    .Append("     , T2.RESULT_WASH_END ")                   ' 洗車終了
        '                    '    .Append("     , NULL AS STAFFCD ")                      ' 担当テクニシャン
        '                    '    .Append("     , NULL AS STAFFNAME ")                    ' 担当テクニシャン名
        '                    '    .Append("  FROM TBL_STALLREZINFO T1 ")
        '                    '    .Append("     , TBL_STALLPROCESS T2 ")
        '                    '    .Append(" WHERE T1.DLRCD = T2.DLRCD ")
        '                    '    .Append("   AND T1.STRCD = T2.STRCD ")
        '                    '    .Append("   AND T1.REZID = T2.REZID ")
        '                    '    .Append("   AND T1.DLRCD  = :DLRCD ")
        '                    '    .Append("   AND T1.STRCD  = :STRCD ")
        '                    '    .Append(sqlReserveId.ToString())
        '                    '    .Append(sqlPreReserveId.ToString())
        '                    'End With

        '                    'query.CommandText = sql.ToString()
        '                    ''バインド変数
        '                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)


        '                    'SQL文作成
        '                    With sql

        '                        .AppendLine(" SELECT  /* SC3140103_005 */ ")
        '                        .AppendLine("         TRIM(T1.DLR_CD) AS DLRCD ")
        '                        .AppendLine("        ,TRIM(T1.BRN_CD) AS STRCD ")
        '                        .AppendLine("        ,T1.SVCIN_ID AS REZID ")
        '                        .AppendLine("        ,T1.SVCIN_ID AS PREZID ")
        '                        .AppendLine("        ,0 AS DSEQNO ")
        '                        .AppendLine("        ,0 AS SEQNO ")
        '                        .AppendLine("        ,DECODE(T1.CARWASH_NEED_FLG, ' ', '0', T1.CARWASH_NEED_FLG) AS WASHFLG ")
        '                        .AppendLine("        ,DECODE(T1.SVC_STATUS, ' ', '0', T1.SVC_STATUS) AS RESULT_STATUS ")
        '                        .AppendLine("        ,DECODE(T2.RSLT_START_DATETIME, :MINDATE, NULL, TO_CHAR(T2.RSLT_START_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_WASH_START ")
        '                        .AppendLine("        ,DECODE(T2.RSLT_END_DATETIME, :MINDATE, NULL, TO_CHAR(T2.RSLT_END_DATETIME, 'YYYYMMDDHH24MI')) AS RESULT_WASH_END ")
        '                        .AppendLine("        ,DECODE(T4.SCHE_END_DATETIME, :MINDATE, NULL, TO_CHAR(T4.SCHE_END_DATETIME, 'YYYYMMDDHH24MI')) AS SCHE_END_DATETIME ")
        '                        .AppendLine("   FROM  TB_T_SERVICEIN T1 ")
        '                        .AppendLine("        ,TB_T_CARWASH_RESULT T2 ")
        '                        .AppendLine("        ,TB_T_JOB_DTL T3 ")
        '                        .AppendLine("        ,TB_T_STALL_USE T4 ")
        '                        .AppendLine("  WHERE  T1.SVCIN_ID = T2.SVCIN_ID (+)  ")
        '                        .AppendLine("    AND  T1.SVCIN_ID = T3.SVCIN_ID ")
        '                        .AppendLine("    AND  T3.JOB_DTL_ID = T4.JOB_DTL_ID ")
        '                        .AppendLine(sqlReserveId.ToString())
        '                        .AppendLine("    AND  NOT EXISTS (SELECT 1 ")
        '                        .AppendLine("                       FROM TB_T_SERVICEIN D1 ")
        '                        .AppendLine("                      WHERE D1.SVCIN_ID = T1.SVCIN_ID ")
        '                        .AppendLine("                        AND D1.SVC_STATUS = :STATUS_CANCEL) ")
        '                        .AppendLine("    AND  T3.CANCEL_FLG = :CANCEL_FLG ")
        '                        .AppendLine("    AND  T4.DLR_CD = :DLR_CD ")
        '                        .AppendLine("    AND  T4.BRN_CD = :BRN_CD ")

        '                    End With

        '                    'パラメータ設定

        '                    '日付省略値
        '                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
        '                    'サービスステータス
        '                    query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
        '                    'キャンセルフラグ
        '                    query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)
        '                    '販売店コード
        '                    query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
        '                    '店舗コード
        '                    query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)


        '                    'SQL格納
        '                    query.CommandText = sql.ToString()

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                    '検索結果返却
        '                    dt = query.GetData()

        '                End Using

        '            Catch ex As OracleExceptionEx When ex.Number = 1013

        '                'ORACLEのタイムアウトのみ処理
        '                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
        '                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
        '                                        , Me.GetType.ToString _
        '                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                        , C_RET_DBTIMEOUT _
        '                                        , ex.Message))
        '                Throw ex
        '            End Try

        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_END _
        '                                    , dt.Rows.Count))

        '            Return dt
        '        End Function

        '#End Region

        '#Region " ストール設定情報取得(標準時間専用)"
        '        ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更)
        '        ' '''-------------------------------------------------------
        '        ' ''' <summary>
        '        ' ''' ストール設定情報取得
        '        ' ''' </summary>
        '        ' ''' <param name="dealerCode">販売店コード</param>
        '        ' ''' <param name="branchCode">店舗コード</param>
        '        ' ''' <returns>ストール設定データセット</returns>
        '        ' ''' <remarks></remarks>
        '        ' '''-------------------------------------------------------
        '        'Public Function GetStallControl(ByVal dealerCode As String, ByVal branchCode As String) As SC3140103DataSet.SC3140103StallCtl2DataTable

        '        '    '開始ログ
        '        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '        '                            , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}" _
        '        '                            , Me.GetType.ToString _
        '        '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '        '                            , LOG_START _
        '        '                            , dealerCode _
        '        '                            , branchCode))

        '        '    Dim dt As SC3140103DataSet.SC3140103StallCtl2DataTable

        '        '    Try
        '        '        Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103StallCtl2DataTable)("SC3140103_006")
        '        '            Dim sql As New StringBuilder

        '        '            'SQL文作成
        '        '            With sql
        '        '                .Append("SELECT /* SC3140103_006 */ ")
        '        '                .Append("       NVL(RECEPT_NORES_WARNING_LT, 0) AS RECEPT_NORES_WARNING_LT")
        '        '                .Append("     , NVL(RECEPT_NORES_ABNORMAL_LT, 0) AS RECEPT_NORES_ABNORMAL_LT ")
        '        '                .Append("     , NVL(RECEPT_RES_WARNING_LT, 0) AS RECEPT_RES_WARNING_LT ")
        '        '                .Append("     , NVL(RECEPT_RES_ABNORMAL_LT, 0) AS RECEPT_RES_ABNORMAL_LT ")
        '        '                .Append("     , NVL(ADDWORK_NORES_WARNING_LT, 0) AS ADDWORK_NORES_WARNING_LT ")
        '        '                .Append("     , NVL(ADDWORK_NORES_ABNORMAL_LT, 0) AS ADDWORK_NORES_ABNORMAL_LT ")
        '        '                .Append("     , NVL(ADDWORK_RES_WARNING_LT, 0) AS ADDWORK_RES_WARNING_LT ")
        '        '                .Append("     , NVL(ADDWORK_RES_ABNORMAL_LT, 0) AS ADDWORK_RES_ABNORMAL_LT ")
        '        '                .Append("     , NVL(DELIVERYPRE_ABNORMAL_LT, 0) AS DELIVERYPRE_ABNORMAL_LT ")
        '        '                .Append("     , NVL(DELIVERYWR_ABNORMAL_LT, 0) AS DELIVERYWR_ABNORMAL_LT ")
        '        '                .Append("  FROM TBL_SERVICEINI ")
        '        '                .Append(" WHERE DLRCD = :DLRCD ")
        '        '                .Append("   AND STRCD = :STRCD ")
        '        '            End With

        '        '            query.CommandText = sql.ToString()
        '        '            'バインド変数
        '        '            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '        '            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)

        '        '            '検索結果返却
        '        '            dt = query.GetData()
        '        '        End Using
        '        '    Catch ex As OracleExceptionEx When ex.Number = 1013
        '        '        'ORACLEのタイムアウトのみ処理
        '        '        Logger.Warn(String.Format(CultureInfo.CurrentCulture _
        '        '                                , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
        '        '                                , Me.GetType.ToString _
        '        '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '        '                                , C_RET_DBTIMEOUT _
        '        '                                , ex.Message))
        '        '        Throw ex
        '        '    End Try

        '        '    '終了ログ
        '        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '        '                            , "{0}.{1} {2} OUT:COUNT = {3}" _
        '        '                            , Me.GetType.ToString _
        '        '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '        '                            , LOG_END _
        '        '                            , dt.Rows.Count))

        '        '    Return dt
        '        'End Function
        '        ' 2012/08/03 TMEJ 日比野【SERVICE_2】STEP2対応(初期表示処理の変更)
        '#End Region

        '#Region "事前準備チップ情報取得"

        '        ''' <summary>
        '        ''' 事前準備チップ予約情報取得
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="reserveId">予約ID</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function GetAdvancePreparationsReserveInfoData(ByVal dealerCode As String, _
        '                                                              ByVal branchCode As String, _
        '                                                              ByVal reserveId As Long) _
        '                                                              As SC3140103DataSet.SC3140103AdvancePreparationsReserveInfoDataTable
        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                       , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, reserveId = {5}" _
        '                                       , Me.GetType.ToString _
        '                                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                       , LOG_START _
        '                                       , dealerCode _
        '                                       , branchCode _
        '                                       , reserveId))

        '            Dim dt As SC3140103DataSet.SC3140103AdvancePreparationsReserveInfoDataTable

        '            Try
        '                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AdvancePreparationsReserveInfoDataTable)("SC3140103_009")

        '                    Dim sql As New StringBuilder


        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                    ''SQL文作成
        '                    'With sql
        '                    '    .Append(" SELECT /* SC3140103_009 */ ")
        '                    '    .Append("        CUSTOMERFLAG ")
        '                    '    .Append("      , CUSTOMERNAME ")
        '                    '    .Append("      , VCLREGNO ")
        '                    '    .Append("      , VIN ")
        '                    '    .Append("      , MODELCODE ")
        '                    '    .Append("      , TELNO ")
        '                    '    .Append("      , MOBILE ")
        '                    '    .Append("      , WASHFLG ")
        '                    '    .Append("      , ACCOUNT_PLAN ")
        '                    '    .Append("      , ORDERNO ")
        '                    '    ' 2012/06/26 西岡 事前準備対応 START
        '                    '    .Append("      , REZ_PICK_DATE AS STOCKTIME")
        '                    '    .Append("      , STARTTIME AS WORKTIME")
        '                    '    ' 2012/06/26 西岡 事前準備対応 END
        '                    '    .Append("   FROM TBL_STALLREZINFO ")
        '                    '    .Append("  WHERE DLRCD = :DLRCD ")
        '                    '    .Append("    AND STRCD = :STRCD ")
        '                    '    .Append("    AND (PREZID = :REZID OR REZID = :REZID) ")
        '                    'End With

        '                    'query.CommandText = sql.ToString()


        '                    ''バインド変数
        '                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '                    'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

        '                    'SQL文作成
        '                    With sql

        '                        .AppendLine(" SELECT  /* SC3140103_009 */ ")
        '                        .AppendLine("         DECODE(T1.CARWASH_NEED_FLG, ' ', '0', T1.CARWASH_NEED_FLG) AS WASHFLG ")
        '                        .AppendLine("        ,TRIM(T1.PIC_SA_STF_CD) AS ACCOUNT_PLAN ")
        '                        .AppendLine("        ,TRIM(T1.RO_NUM) AS ORDERNO ")
        '                        .AppendLine("        ,DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE, NULL, TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MI')) AS STOCKTIME ")
        '                        .AppendLine("        ,DECODE(T3.SCHE_START_DATETIME, :MINDATE, TO_DATE(NULL), T3.SCHE_START_DATETIME) AS WORKTIME ")
        '                        .AppendLine("        ,TRIM(T4.CST_NAME) AS CUSTOMERNAME ")
        '                        .AppendLine("        ,TRIM(T4.CST_PHONE) AS TELNO ")
        '                        .AppendLine("        ,TRIM(T4.CST_MOBILE) AS MOBILE ")
        '                        .AppendLine("        ,DECODE(T8.CST_ID, NULL, NVL(TRIM(T5.CST_TYPE), '2'), '1') AS CUSTOMERFLAG ")
        '                        .AppendLine("        ,NVL(TRIM(T6.VCL_VIN), TRIM(T8.VCL_VIN)) AS VIN ")
        '                        .AppendLine("        ,TRIM(T6.VCL_KATASHIKI) AS MODELCODE ")
        '                        .AppendLine("        ,TRIM(T7.REG_NUM) AS VCLREGNO ")
        '                        .AppendLine("   FROM  TB_T_SERVICEIN T1 ")
        '                        .AppendLine("        ,TB_T_JOB_DTL T2 ")
        '                        .AppendLine("        ,TB_T_STALL_USE T3 ")
        '                        .AppendLine("        ,TB_M_CUSTOMER T4 ")
        '                        .AppendLine("        ,TB_M_CUSTOMER_DLR T5 ")
        '                        .AppendLine("        ,TB_M_VEHICLE T6 ")
        '                        .AppendLine("        ,TB_M_VEHICLE_DLR T7 ")
        '                        .AppendLine("        ,TBL_SERVICEIN_APPEND T8 ")
        '                        .AppendLine("  WHERE  T1.SVCIN_ID = T2.SVCIN_ID ")
        '                        .AppendLine("    AND  T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
        '                        .AppendLine("    AND  T1.CST_ID = T4.CST_ID (+) ")
        '                        .AppendLine("    AND  T1.DLR_CD = T5.DLR_CD (+) ")
        '                        .AppendLine("    AND  T1.CST_ID = T5.CST_ID (+) ")
        '                        .AppendLine("    AND  T1.VCL_ID = T6.VCL_ID (+) ")
        '                        .AppendLine("    AND  T1.DLR_CD = T7.DLR_CD (+) ")
        '                        .AppendLine("    AND  T1.VCL_ID = T7.VCL_ID (+) ")
        '                        .AppendLine("    AND  T1.CST_ID = T8.CST_ID (+) ")
        '                        .AppendLine("    AND  T1.VCL_ID = T8.VCL_ID (+) ")
        '                        .AppendLine("    AND  T1.DLR_CD = :DLRCD ")
        '                        .AppendLine("    AND  T1.BRN_CD = :STRCD ")
        '                        .AppendLine("    AND  T1.SVCIN_ID = :REZID ")
        '                        .AppendLine("    AND  NOT EXISTS (SELECT 1 ")
        '                        .AppendLine("                       FROM TB_T_SERVICEIN D1 ")
        '                        .AppendLine("                      WHERE D1.SVCIN_ID = T1.SVCIN_ID ")
        '                        .AppendLine("                        AND D1.SVC_STATUS = :STATUS_CANCEL) ")
        '                        .AppendLine("    AND  T2.CANCEL_FLG = :CANCEL_FLG ")
        '                        .AppendLine("    AND  T3.DLR_CD = :DLRCD ")
        '                        .AppendLine("    AND  T3.BRN_CD = :STRCD ")

        '                    End With

        '                    query.CommandText = sql.ToString()

        '                    'パラメータ設定

        '                    '日付省略値
        '                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
        '                    '販売店コード
        '                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
        '                    '店舗コード
        '                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
        '                    'サービス入庫ID
        '                    query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
        '                    'サービスステータス
        '                    query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
        '                    'キャンセルフラグ
        '                    query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                    '検索結果返却
        '                    dt = query.GetData()

        '                End Using

        '            Catch ex As OracleExceptionEx When ex.Number = 1013

        '                'ORACLEのタイムアウトのみ処理
        '                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
        '                                           , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
        '                                           , Me.GetType.ToString _
        '                                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                           , C_RET_DBTIMEOUT _
        '                                           , ex.Message))
        '                Throw ex

        '            End Try

        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                       , "{0}.{1} {2} OUT:COUNT = {3}" _
        '                                       , Me.GetType.ToString _
        '                                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                       , LOG_END _
        '                                       , dt.Rows.Count))

        '            Return dt

        '        End Function

        '        ''' <summary>
        '        ''' 事前準備チップサービス来店管理情報取得
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="reserveId">予約ID</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function GetAdvancePreparationsServiceVisitManagementData(ByVal dealerCode As String, _
        '                                                                         ByVal branchCode As String, _
        '                                                                         ByVal reserveId As Long) As SC3140103DataSet.SC3140103AdvancePreparationsServiceVisitManagementDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                       , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, reserveId = {5}" _
        '                                       , Me.GetType.ToString _
        '                                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                       , LOG_START _
        '                                       , dealerCode _
        '                                       , branchCode _
        '                                       , reserveId))

        '            Dim dt As SC3140103DataSet.SC3140103AdvancePreparationsServiceVisitManagementDataTable

        '            Try
        '                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AdvancePreparationsServiceVisitManagementDataTable)("SC3140103_010")
        '                    Dim sql As New StringBuilder

        '                    'SQL文作成
        '                    With sql
        '                        .Append(" SELECT /* SC3140103_010 */ ")
        '                        .Append("        VISITSEQ ")
        '                        .Append("      , CUSTSEGMENT ")
        '                        .Append("      , ASSIGNSTATUS ")
        '                        .Append("      , SACODE ")
        '                        .Append("      , ORDERNO ")
        '                        .Append("   FROM TBL_SERVICE_VISIT_MANAGEMENT ")
        '                        .Append("  WHERE DLRCD = :DLRCD ")
        '                        .Append("    AND STRCD = :STRCD ")
        '                        .Append("    AND FREZID = :REZID ")
        '                    End With

        '                    query.CommandText = sql.ToString()
        '                    'バインド変数

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
        '                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                    query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

        '                    '検索結果返却
        '                    dt = query.GetData()

        '                End Using

        '            Catch ex As OracleExceptionEx When ex.Number = 1013
        '                'ORACLEのタイムアウトのみ処理
        '                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
        '                                           , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
        '                                           , Me.GetType.ToString _
        '                                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                           , C_RET_DBTIMEOUT _
        '                                           , ex.Message))
        '                Throw ex

        '            End Try

        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                       , "{0}.{1} {2} OUT:COUNT = {3}" _
        '                                       , Me.GetType.ToString _
        '                                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                       , LOG_END _
        '                                       , dt.Rows.Count))

        '            Return dt


        '        End Function

        '        ''' <summary>
        '        ''' 事前準備チップ情報取得
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="nowDay">本日年月日（YYYYMMDD）</param>
        '        ''' <param name="nextBusinessDay">翌営業年月日（YYYYMMDD）</param>
        '        ''' <param name="saCode">SAコード</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function GetAdvancePreparationsChipData(ByVal dealerCode As String, _
        '                                                       ByVal branchCode As String, _
        '                                                       ByVal nowDay As String, _
        '                                                       ByVal nextBusinessDay As String, _
        '                                                       ByVal saCode As String) _
        '                                                       As SC3140103DataSet.SC3140103AdvancePreparationsDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, nowDay = {5}, nextBusinessDay = {6}, saCode = {7}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , LOG_START _
        '               , dealerCode _
        '               , branchCode _
        '               , nowDay _
        '               , nextBusinessDay _
        '               , saCode))

        '            Dim dt As SC3140103DataSet.SC3140103AdvancePreparationsDataTable

        '            Try
        '                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AdvancePreparationsDataTable)("SC3140103_011")

        '                    Dim sql As New StringBuilder

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                    'With sql
        '                    '    .AppendLine(" SELECT /* SC3140103_011 */ ")
        '                    '    .AppendLine("        R4.DLRCD ")
        '                    '    .AppendLine("      , R4.STRCD ")
        '                    '    .AppendLine("      , R4.REZID ")
        '                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T1.VCLREGNO, NULL))          AS VCLREGNO ")
        '                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T1.VIN, NULL))               AS VIN ")
        '                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T1.CUSTOMERNAME, NULL))      AS CUSTOMERNAME ")
        '                    '    .AppendLine("      , R4.REZ_PICK_DATE ")
        '                    '    .AppendLine("      , R4.REZ_DELI_DATE ")
        '                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T4.MERCHANDISENAME, NULL))   AS MERCHANDISENAME ")
        '                    '    .AppendLine("      , NVL(MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T3.VISITSEQ, NULL)), -1) AS VISITSEQ ")
        '                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T3.ASSIGNSTATUS, NULL))      AS ASSIGNSTATUS ")
        '                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T1.ORDERNO, NULL))           AS ORDERNO ")
        '                    '    .AppendLine("      , DECODE(SUBSTR(R4.REZ_PICK_DATE, 1, 8), :TODAY, 1, 0)                AS TODAYFLG ")
        '                    '    .AppendLine("   FROM TBL_STALLREZINFO T1, ")
        '                    '    .AppendLine("        TBL_SERVICE_VISIT_MANAGEMENT T3, ")
        '                    '    .AppendLine("        TBL_MERCHANDISEMST T4, ")
        '                    '    .AppendLine("        (SELECT R2.DLRCD ")
        '                    '    .AppendLine("              , R2.STRCD ")
        '                    '    .AppendLine("              , NVL(R2.PREZID,R2.REZID) AS REZID ")
        '                    '    .AppendLine("              , NVL(MIN(R2.REZ_PICK_DATE), TO_CHAR(MIN(R2.STARTTIME), 'YYYYMMDDHH24MI')) AS REZ_PICK_DATE ")
        '                    '    .AppendLine("              , NVL(MAX(R2.REZ_DELI_DATE), TO_CHAR(MAX(R2.ENDTIME), 'YYYYMMDDHH24MI'))   AS REZ_DELI_DATE ")
        '                    '    .AppendLine("              , MIN(R2.STARTTIME) AS STARTTIME ")
        '                    '    .AppendLine("           FROM TBL_STALLREZINFO R2 ")
        '                    '    .AppendLine("          WHERE R2.DLRCD = :DLRCD ")
        '                    '    .AppendLine("            AND R2.STRCD = :STRCD ")
        '                    '    .AppendLine("            AND R2.STARTTIME >= TO_DATE(:NOWFROMTIME,'YYYYMMDDHH24MI') ")
        '                    '    .AppendLine("            AND R2.ACCOUNT_PLAN = :SACODE ")
        '                    '    .AppendLine("            AND R2.STATUS = 1 ")
        '                    '    .AppendLine("            AND DECODE(R2.STOPFLG,'0',DECODE(R2.CANCELFLG,'1',1,0),0) + ")
        '                    '    .AppendLine("                DECODE(R2.REZCHILDNO,0,1,0) + ")
        '                    '    .AppendLine("                DECODE(R2.REZCHILDNO,999,1,0) = 0 ")
        '                    '    .AppendLine("            AND NOT EXISTS( ")
        '                    '    .AppendLine("                SELECT 1 ")
        '                    '    .AppendLine("                  FROM TBL_STALLREZINFO R3 ")
        '                    '    .AppendLine("                 WHERE R2.DLRCD = R3.DLRCD ")
        '                    '    .AppendLine("                   AND R2.STRCD = R3.STRCD ")
        '                    '    .AppendLine("                   AND (R2.PREZID = R3.PREZID OR R2.REZID = R3.REZID) ")
        '                    '    .AppendLine("                   AND DECODE(R3.STOPFLG,'0',DECODE(R3.CANCELFLG,'1',1,0),0) + ")
        '                    '    .AppendLine("                       DECODE(R3.REZCHILDNO,0,1,0) + ")
        '                    '    .AppendLine("                       DECODE(R3.REZCHILDNO,999,1,0) = 0 ")
        '                    '    .AppendLine("                   AND (R3.STOPFLG = '2' OR R3.STOPFLG = '6' ")
        '                    '    .AppendLine("                    OR NVL(R3.REZ_PICK_DATE,TO_CHAR(R3.STARTTIME,'YYYYMMDDHH24MI')) < :NOWFROMTIME)) ")
        '                    '    .AppendLine("          GROUP BY R2.DLRCD, ")
        '                    '    .AppendLine("                   R2.STRCD, ")
        '                    '    .AppendLine("                   NVL(R2.PREZID,R2.REZID) ")
        '                    '    .AppendLine("         HAVING NVL(MIN(R2.REZ_PICK_DATE),TO_CHAR(MIN(R2.STARTTIME),'YYYYMMDDHH24MI')) BETWEEN :NOWFROMTIME  AND :NOWTOTIME  ")
        '                    '    .AppendLine("             OR NVL(MIN(R2.REZ_PICK_DATE),TO_CHAR(MIN(R2.STARTTIME),'YYYYMMDDHH24MI')) BETWEEN :NEXTFROMTIME AND :NEXTTOTIME) R4 ")
        '                    '    .AppendLine("  WHERE T1.DLRCD = R4.DLRCD ")
        '                    '    .AppendLine("    AND T1.STRCD = R4.STRCD ")
        '                    '    .AppendLine("    AND (R4.REZID = T1.REZID OR R4.REZID = T1.PREZID) ")
        '                    '    .AppendLine("    AND R4.DLRCD = T3.DLRCD(+) ")
        '                    '    .AppendLine("    AND R4.STRCD = T3.STRCD(+) ")
        '                    '    .AppendLine("    AND R4.REZID = T3.FREZID(+) ")
        '                    '    .AppendLine("    AND T1.DLRCD = T4.DLRCD(+) ")
        '                    '    .AppendLine("    AND T1.MERCHANDISECD = T4.MERCHANDISECD(+) ")
        '                    '    .AppendLine("    AND DECODE(T1.STOPFLG,'0',DECODE(T1.CANCELFLG,'1',1,0),0) + ")
        '                    '    .AppendLine("        DECODE(T1.REZCHILDNO,0,1,0) + ")
        '                    '    .AppendLine("        DECODE(T1.REZCHILDNO,999,1,0) = 0 ")
        '                    '    .AppendLine("    AND (T3.ASSIGNSTATUS IS NULL OR T3.ASSIGNSTATUS = '0' OR T3.ASSIGNSTATUS = '1') ")
        '                    '    .AppendLine("  GROUP BY R4.DLRCD, ")
        '                    '    .AppendLine("           R4.STRCD, ")
        '                    '    .AppendLine("           R4.REZID, ")
        '                    '    .AppendLine("           R4.REZ_PICK_DATE, ")
        '                    '    .AppendLine("           R4.REZ_DELI_DATE ")
        '                    'End With

        '                    'query.CommandText = sql.ToString()

        '                    ''バインド変数
        '                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '                    'query.AddParameterWithTypeValue("TODAY", OracleDbType.Char, nowDay)
        '                    'query.AddParameterWithTypeValue("NOWFROMTIME", OracleDbType.Char, nowDay + "0000")
        '                    'query.AddParameterWithTypeValue("NOWTOTIME", OracleDbType.Char, nowDay + "2359")
        '                    'query.AddParameterWithTypeValue("NEXTFROMTIME", OracleDbType.Char, nextBusinessDay + "0000")
        '                    'query.AddParameterWithTypeValue("NEXTTOTIME", OracleDbType.Char, nextBusinessDay + "2359")
        '                    'query.AddParameterWithTypeValue("SACODE", OracleDbType.Char, saCode)


        '                    With sql

        '                        .AppendLine(" SELECT  /* SC3140103_011 */ ")
        '                        .AppendLine("         TRIM(S1.DLR_CD) AS DLRCD ")
        '                        .AppendLine("        ,TRIM(S1.BRN_CD) AS STRCD ")
        '                        .AppendLine("        ,S1.SVCIN_ID AS REZID ")
        '                        .AppendLine("        ,S1.RO_NUM AS ORDERNO ")
        '                        .AppendLine("        ,NVL(S1.SCHE_SVCIN_DATETIME, S1.SCHE_START_DATETIME) AS REZ_PICK_DATE ")
        '                        .AppendLine("        ,NVL(S1.SCHE_DELI_DATETIME, S1.SCHE_END_DATETIME) AS REZ_DELI_DATE ")
        '                        .AppendLine("        ,S1.CST_NAME AS CUSTOMERNAME ")
        '                        .AppendLine("        ,NVL(S1.VCL_VIN, TRIM(T10.VCL_VIN)) AS VIN ")
        '                        .AppendLine("        ,S1.REG_NUM AS VCLREGNO ")
        '                        .AppendLine("        ,NVL(TRIM(T8.MERC_NAME), TRIM(T8.MERC_NAME_ENG)) AS MERCHANDISENAME ")
        '                        .AppendLine("        ,NVL(T9.VISITSEQ, -1) AS VISITSEQ ")
        '                        .AppendLine("        ,T9.ASSIGNSTATUS AS ASSIGNSTATUS ")
        '                        .AppendLine("        ,DECODE(SUBSTR(S1.SCHE_SVCIN_DATETIME, 1, 8), :TODAY, 1, 0) AS TODAYFLG ")
        '                        .AppendLine("   FROM          ")
        '                        .AppendLine("         (SELECT ")
        '                        .AppendLine("                  MAX(T1.DLR_CD) AS DLR_CD ")
        '                        .AppendLine("                 ,MAX(T1.BRN_CD) AS BRN_CD ")
        '                        .AppendLine("                 ,T1.SVCIN_ID ")
        '                        .AppendLine("                 ,MAX(TRIM(T1.RO_NUM)) AS RO_NUM ")
        '                        .AppendLine("                 ,MAX(T1.CST_ID) AS CST_ID ")
        '                        .AppendLine("                 ,MAX(T1.VCL_ID) AS VCL_ID ")
        '                        .AppendLine("                 ,MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE , TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MI'), TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_SVCIN_DATETIME ")
        '                        .AppendLine("                 ,MAX(DECODE(T1.SCHE_DELI_DATETIME, :MINDATE , TO_CHAR(T3.SCHE_END_DATETIME, 'YYYYMMDDHH24MI'), TO_CHAR(T1.SCHE_DELI_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_DELI_DATETIME ")
        '                        .AppendLine("                 ,MIN(T2.JOB_DTL_ID) AS JOB_DTL_ID ")
        '                        .AppendLine("                 ,MIN(DECODE(T3.SCHE_START_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_START_DATETIME ")
        '                        .AppendLine("                 ,MIN(DECODE(T3.SCHE_END_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_END_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_END_DATETIME ")
        '                        .AppendLine("                 ,MAX(TRIM(T4.CST_NAME)) AS CST_NAME ")
        '                        .AppendLine("                 ,MAX(TRIM(T5.VCL_VIN)) AS VCL_VIN ")
        '                        .AppendLine("                 ,MAX(TRIM(T5.VCL_KATASHIKI)) AS VCL_KATASHIKI ")
        '                        .AppendLine("                 ,MAX(TRIM(T6.REG_NUM)) AS REG_NUM ")
        '                        .AppendLine("            FROM  TB_T_SERVICEIN T1 ")
        '                        .AppendLine("                 ,TB_T_JOB_DTL T2 ")
        '                        .AppendLine("                 ,TB_T_STALL_USE T3 ")
        '                        .AppendLine("                 ,TB_M_CUSTOMER T4 ")
        '                        .AppendLine("                 ,TB_M_VEHICLE T5 ")
        '                        .AppendLine("                 ,TB_M_VEHICLE_DLR T6 ")
        '                        .AppendLine("           WHERE  T1.SVCIN_ID = T2.SVCIN_ID ")
        '                        .AppendLine("             AND  T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
        '                        .AppendLine("             AND  T1.CST_ID = T4.CST_ID (+) ")
        '                        .AppendLine("             AND  T1.VCL_ID = T5.VCL_ID (+) ")
        '                        .AppendLine("             AND  T1.DLR_CD = T6.DLR_CD (+) ")
        '                        .AppendLine("             AND  T1.VCL_ID = T6.VCL_ID (+) ")
        '                        .AppendLine("             AND  T1.DLR_CD = :DLRCD ")
        '                        .AppendLine("             AND  T1.BRN_CD = :STRCD ")
        '                        .AppendLine("             AND  T1.RESV_STATUS = :RESV_STATUS ")
        '                        .AppendLine("             AND  T1.PIC_SA_STF_CD = :SACODE ")
        '                        .AppendLine("             AND  NOT EXISTS (SELECT 1 ")
        '                        .AppendLine("                                FROM TB_T_SERVICEIN D1 ")
        '                        .AppendLine("                               WHERE D1.SVCIN_ID = T1.SVCIN_ID ")
        '                        .AppendLine("                                 AND D1.SVC_STATUS = :STATUS_CANCEL) ")
        '                        .AppendLine("             AND  T2.CANCEL_FLG = :CANCEL_FLG ")
        '                        .AppendLine("             AND  T3.SCHE_START_DATETIME >= TO_DATE(:NOWFROMTIME, 'YYYYMMDDHH24MISS') ")
        '                        .AppendLine("             AND  NOT((T3.TEMP_FLG = :TEMP_FLG AND T3.STALL_USE_STATUS IN (:STATUS_00, :STATUS_01)) OR T3.STALL_USE_STATUS = :STATUS_07)  ")
        '                        .AppendLine("        GROUP BY  T1.SVCIN_ID ")
        '                        .AppendLine("          HAVING  NVL(MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE , NULL, TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MISS'))) ")
        '                        .AppendLine("                    , MIN(DECODE(T3.SCHE_START_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MISS')))) BETWEEN :NOWFROMTIME  AND :NOWTOTIME ")
        '                        .AppendLine("              OR  NVL(MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE , NULL, TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MISS'))) ")
        '                        .AppendLine("                    , MIN(DECODE(T3.SCHE_START_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MISS')))) BETWEEN :NEXTFROMTIME AND :NEXTTOTIME ")
        '                        .AppendLine("          ) S1 ")
        '                        .AppendLine("         ,TB_T_JOB_DTL T7 ")
        '                        .AppendLine("         ,TB_M_MERCHANDISE T8 ")
        '                        .AppendLine("         ,TBL_SERVICE_VISIT_MANAGEMENT T9 ")
        '                        .AppendLine("         ,TBL_SERVICEIN_APPEND T10 ")
        '                        .AppendLine("  WHERE  S1.SVCIN_ID = T9.FREZID (+) ")
        '                        .AppendLine("    AND  S1.JOB_DTL_ID = T7.JOB_DTL_ID ")
        '                        .AppendLine("    AND  T7.MERC_ID = T8.MERC_ID(+)  ")
        '                        .AppendLine("    AND  S1.CST_ID = T10.CST_ID(+) ")
        '                        .AppendLine("    AND  S1.VCL_ID = T10.VCL_ID(+)  ")
        '                        .AppendLine("    AND  T9.DLRCD(+) = :DLRCD ")
        '                        .AppendLine("    AND  T9.STRCD(+) = :STRCD ")
        '                        .AppendLine("    AND  (T9.ASSIGNSTATUS IS NULL OR T9.ASSIGNSTATUS IN (:ASSIGNSTATUS_0, :ASSIGNSTATUS_1)) ")

        '                    End With


        '                    'パラメータ設定

        '                    '当日フラグ
        '                    query.AddParameterWithTypeValue("TODAY", OracleDbType.NVarchar2, nowDay)
        '                    '日付省略値
        '                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
        '                    '販売店コード
        '                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
        '                    '店舗コード
        '                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
        '                    '予約ステータス
        '                    query.AddParameterWithTypeValue("RESV_STATUS", OracleDbType.NVarchar2, RezStatus)
        '                    '担当SAスタッフコード
        '                    query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, saCode)
        '                    'サービスステータス
        '                    query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
        '                    'キャンセルフラグ
        '                    query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)

        '                    '仮置フラグ
        '                    query.AddParameterWithTypeValue("TEMP_FLG", OracleDbType.NVarchar2, TenpFlag)
        '                    'ストール利用ステータス
        '                    query.AddParameterWithTypeValue("STATUS_00", OracleDbType.NVarchar2, StallStatusInstruct)
        '                    query.AddParameterWithTypeValue("STATUS_01", OracleDbType.NVarchar2, StallStatusWaitWork)
        '                    query.AddParameterWithTypeValue("STATUS_07", OracleDbType.NVarchar2, StallStatusUncome)
        '                    '振当ステータス
        '                    query.AddParameterWithTypeValue("ASSIGNSTATUS_0", OracleDbType.NVarchar2, NonAssign)
        '                    query.AddParameterWithTypeValue("ASSIGNSTATUS_1", OracleDbType.NVarchar2, AssignWait)

        '                    '条件(当日)
        '                    '予定入庫日時・予定開始日時
        '                    query.AddParameterWithTypeValue("NOWFROMTIME", OracleDbType.NVarchar2, nowDay + "000000")
        '                    query.AddParameterWithTypeValue("NOWTOTIME", OracleDbType.NVarchar2, nowDay + "235959")
        '                    query.AddParameterWithTypeValue("NEXTFROMTIME", OracleDbType.NVarchar2, nextBusinessDay + "000000")
        '                    query.AddParameterWithTypeValue("NEXTTOTIME", OracleDbType.NVarchar2, nextBusinessDay + "235959")


        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                    'SQL格納
        '                    query.CommandText = sql.ToString()

        '                    '検索結果返却
        '                    dt = query.GetData()

        '                End Using

        '            Catch ex As OracleExceptionEx When ex.Number = 1013
        '                'ORACLEのタイムアウトのみ処理
        '                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
        '                   , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
        '                   , Me.GetType.ToString _
        '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                   , C_RET_DBTIMEOUT _
        '                   , ex.Message))
        '                Throw ex
        '            End Try

        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} {2} OUT:COUNT = {3}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , LOG_END _
        '               , dt.Rows.Count))

        '            Return dt
        '        End Function

        '        '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）SATRT
        '        ''' <summary>
        '        ''' 事前準備チップ情報取得(担当SAが未当て)
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="nowDay">本日年月日（YYYYMMDD）</param>
        '        ''' <param name="nextBusinessDay">翌営業年月日（YYYYMMDD）</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function GetAdvancePreparationsChipDataNoSA(ByVal dealerCode As String, _
        '                                                           ByVal branchCode As String, _
        '                                                           ByVal nowDay As String, _
        '                                                           ByVal nextBusinessDay As String) _
        '                                                           As SC3140103DataSet.SC3140103AdvancePreparationsDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, nowDay = {5}, nextBusinessDay = {6}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , LOG_START _
        '               , dealerCode _
        '               , branchCode _
        '               , nowDay _
        '               , nextBusinessDay))

        '            Dim dt As SC3140103DataSet.SC3140103AdvancePreparationsDataTable

        '            Try
        '                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AdvancePreparationsDataTable)("SC3140103_020")

        '                    Dim sql As New StringBuilder

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                    'With sql
        '                    '    .AppendLine(" SELECT /* SC3140103_020 */ ")
        '                    '    .AppendLine("        R4.DLRCD ")
        '                    '    .AppendLine("      , R4.STRCD ")
        '                    '    .AppendLine("      , R4.REZID ")
        '                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T1.VCLREGNO, NULL))          AS VCLREGNO ")
        '                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T1.VIN, NULL))               AS VIN ")
        '                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T1.CUSTOMERNAME, NULL))      AS CUSTOMERNAME ")
        '                    '    .AppendLine("      , R4.REZ_PICK_DATE ")
        '                    '    .AppendLine("      , R4.REZ_DELI_DATE ")
        '                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T4.MERCHANDISENAME, NULL))   AS MERCHANDISENAME ")
        '                    '    .AppendLine("      , NVL(MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T3.VISITSEQ, NULL)), -1) AS VISITSEQ ")
        '                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T3.ASSIGNSTATUS, NULL))      AS ASSIGNSTATUS ")
        '                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T1.ORDERNO, NULL))           AS ORDERNO ")
        '                    '    .AppendLine("      , DECODE(SUBSTR(R4.REZ_PICK_DATE, 1, 8), :TODAY, 1, 0)                AS TODAYFLG ")
        '                    '    .AppendLine("   FROM TBL_STALLREZINFO T1, ")
        '                    '    .AppendLine("        TBL_SERVICE_VISIT_MANAGEMENT T3, ")
        '                    '    .AppendLine("        TBL_MERCHANDISEMST T4, ")
        '                    '    .AppendLine("        (SELECT R2.DLRCD ")
        '                    '    .AppendLine("              , R2.STRCD ")
        '                    '    .AppendLine("              , NVL(R2.PREZID,R2.REZID) AS REZID ")
        '                    '    .AppendLine("              , NVL(MIN(R2.REZ_PICK_DATE), TO_CHAR(MIN(R2.STARTTIME), 'YYYYMMDDHH24MI')) AS REZ_PICK_DATE ")
        '                    '    .AppendLine("              , NVL(MAX(R2.REZ_DELI_DATE), TO_CHAR(MAX(R2.ENDTIME), 'YYYYMMDDHH24MI'))   AS REZ_DELI_DATE ")
        '                    '    .AppendLine("              , MIN(R2.STARTTIME) AS STARTTIME ")
        '                    '    .AppendLine("           FROM TBL_STALLREZINFO R2, ")
        '                    '    .AppendLine("                TBL_USERS T5")
        '                    '    .AppendLine("          WHERE R2.DLRCD = :DLRCD ")
        '                    '    .AppendLine("            AND R2.STRCD = :STRCD ")
        '                    '    .AppendLine("            AND R2.STARTTIME >= TO_DATE(:NOWFROMTIME,'YYYYMMDDHH24MI') ")
        '                    '    .AppendLine("            AND (TRIM(R2.ACCOUNT_PLAN) IS NULL ")
        '                    '    .AppendLine("            OR  T5.DELFLG = 1 ) ")
        '                    '    .AppendLine("            AND R2.STATUS = 1 ")
        '                    '    .AppendLine("            AND R2.ACCOUNT_PLAN = T5.ACCOUNT(+) ")
        '                    '    .AppendLine("            AND DECODE(R2.STOPFLG,'0',DECODE(R2.CANCELFLG,'1',1,0),0) + ")
        '                    '    .AppendLine("                DECODE(R2.REZCHILDNO,0,1,0) + ")
        '                    '    .AppendLine("                DECODE(R2.REZCHILDNO,999,1,0) = 0 ")
        '                    '    .AppendLine("            AND NOT EXISTS( ")
        '                    '    .AppendLine("                SELECT 1 ")
        '                    '    .AppendLine("                  FROM TBL_STALLREZINFO R3 ")
        '                    '    .AppendLine("                 WHERE R2.DLRCD = R3.DLRCD ")
        '                    '    .AppendLine("                   AND R2.STRCD = R3.STRCD ")
        '                    '    .AppendLine("                   AND (R2.PREZID = R3.PREZID OR R2.REZID = R3.REZID) ")
        '                    '    .AppendLine("                   AND DECODE(R3.STOPFLG,'0',DECODE(R3.CANCELFLG,'1',1,0),0) + ")
        '                    '    .AppendLine("                       DECODE(R3.REZCHILDNO,0,1,0) + ")
        '                    '    .AppendLine("                       DECODE(R3.REZCHILDNO,999,1,0) = 0 ")
        '                    '    .AppendLine("                   AND (R3.STOPFLG = '2' OR R3.STOPFLG = '6' ")
        '                    '    .AppendLine("                    OR NVL(R3.REZ_PICK_DATE,TO_CHAR(R3.STARTTIME,'YYYYMMDDHH24MI')) < :NOWFROMTIME)) ")
        '                    '    .AppendLine("          GROUP BY R2.DLRCD, ")
        '                    '    .AppendLine("                   R2.STRCD, ")
        '                    '    .AppendLine("                   NVL(R2.PREZID,R2.REZID) ")
        '                    '    .AppendLine("         HAVING NVL(MIN(R2.REZ_PICK_DATE),TO_CHAR(MIN(R2.STARTTIME),'YYYYMMDDHH24MI')) BETWEEN :NOWFROMTIME  AND :NOWTOTIME  ")
        '                    '    .AppendLine("             OR NVL(MIN(R2.REZ_PICK_DATE),TO_CHAR(MIN(R2.STARTTIME),'YYYYMMDDHH24MI')) BETWEEN :NEXTFROMTIME AND :NEXTTOTIME) R4 ")
        '                    '    .AppendLine("  WHERE T1.DLRCD = R4.DLRCD ")
        '                    '    .AppendLine("    AND T1.STRCD = R4.STRCD ")
        '                    '    .AppendLine("    AND (R4.REZID = T1.REZID OR R4.REZID = T1.PREZID) ")
        '                    '    .AppendLine("    AND R4.DLRCD = T3.DLRCD(+) ")
        '                    '    .AppendLine("    AND R4.STRCD = T3.STRCD(+) ")
        '                    '    .AppendLine("    AND R4.REZID = T3.FREZID(+) ")
        '                    '    .AppendLine("    AND T1.DLRCD = T4.DLRCD(+) ")
        '                    '    .AppendLine("    AND T1.MERCHANDISECD = T4.MERCHANDISECD(+) ")
        '                    '    .AppendLine("    AND DECODE(T1.STOPFLG,'0',DECODE(T1.CANCELFLG,'1',1,0),0) + ")
        '                    '    .AppendLine("        DECODE(T1.REZCHILDNO,0,1,0) + ")
        '                    '    .AppendLine("        DECODE(T1.REZCHILDNO,999,1,0) = 0 ")
        '                    '    .AppendLine("    AND (T3.ASSIGNSTATUS IS NULL OR T3.ASSIGNSTATUS = '0' OR T3.ASSIGNSTATUS = '1') ")
        '                    '    .AppendLine("  GROUP BY R4.DLRCD, ")
        '                    '    .AppendLine("           R4.STRCD, ")
        '                    '    .AppendLine("           R4.REZID, ")
        '                    '    .AppendLine("           R4.REZ_PICK_DATE, ")
        '                    '    .AppendLine("           R4.REZ_DELI_DATE ")
        '                    'End With

        '                    'query.CommandText = sql.ToString()

        '                    ''バインド変数
        '                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '                    'query.AddParameterWithTypeValue("TODAY", OracleDbType.Char, nowDay)
        '                    'query.AddParameterWithTypeValue("NOWFROMTIME", OracleDbType.Char, nowDay + "0000")
        '                    'query.AddParameterWithTypeValue("NOWTOTIME", OracleDbType.Char, nowDay + "2359")
        '                    'query.AddParameterWithTypeValue("NEXTFROMTIME", OracleDbType.Char, nextBusinessDay + "0000")
        '                    'query.AddParameterWithTypeValue("NEXTTOTIME", OracleDbType.Char, nextBusinessDay + "2359")


        '                    With sql

        '                        .AppendLine(" SELECT  /* SC3140103_020 */ ")
        '                        .AppendLine("         TRIM(S1.DLR_CD) AS DLRCD ")
        '                        .AppendLine("        ,TRIM(S1.BRN_CD) AS STRCD ")
        '                        .AppendLine("        ,S1.SVCIN_ID AS REZID ")
        '                        .AppendLine("        ,S1.RO_NUM AS ORDERNO ")
        '                        .AppendLine("        ,NVL(S1.SCHE_SVCIN_DATETIME, S1.SCHE_START_DATETIME) AS REZ_PICK_DATE ")
        '                        .AppendLine("        ,NVL(S1.SCHE_DELI_DATETIME, S1.SCHE_END_DATETIME) AS REZ_DELI_DATE ")
        '                        .AppendLine("        ,S1.CST_NAME AS CUSTOMERNAME ")
        '                        .AppendLine("        ,NVL(S1.VCL_VIN, TRIM(T11.VCL_VIN)) AS VIN ")
        '                        .AppendLine("        ,S1.REG_NUM AS VCLREGNO ")
        '                        .AppendLine("        ,NVL(TRIM(T9.MERC_NAME), TRIM(T9.MERC_NAME_ENG)) AS MERCHANDISENAME ")
        '                        .AppendLine("        ,NVL(T10.VISITSEQ, -1) AS VISITSEQ ")
        '                        .AppendLine("        ,T10.ASSIGNSTATUS AS ASSIGNSTATUS ")
        '                        .AppendLine("        ,DECODE(SUBSTR(S1.SCHE_SVCIN_DATETIME, 1, 8), :TODAY, 1, 0) AS TODAYFLG ")
        '                        .AppendLine("   FROM          ")
        '                        .AppendLine("         (SELECT ")
        '                        .AppendLine("                  MAX(T1.DLR_CD) AS DLR_CD ")
        '                        .AppendLine("                 ,MAX(T1.BRN_CD) AS BRN_CD ")
        '                        .AppendLine("                 ,T1.SVCIN_ID ")
        '                        .AppendLine("                 ,MAX(TRIM(T1.RO_NUM)) AS RO_NUM ")
        '                        .AppendLine("                 ,MAX(T1.CST_ID) AS CST_ID ")
        '                        .AppendLine("                 ,MAX(T1.VCL_ID) AS VCL_ID ")
        '                        .AppendLine("                 ,MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE , TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MI'), TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_SVCIN_DATETIME ")
        '                        .AppendLine("                 ,MAX(DECODE(T1.SCHE_DELI_DATETIME, :MINDATE , TO_CHAR(T3.SCHE_END_DATETIME, 'YYYYMMDDHH24MI'), TO_CHAR(T1.SCHE_DELI_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_DELI_DATETIME ")
        '                        .AppendLine("                 ,MIN(T2.JOB_DTL_ID) AS JOB_DTL_ID ")
        '                        .AppendLine("                 ,MIN(DECODE(T3.SCHE_START_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_START_DATETIME ")
        '                        .AppendLine("                 ,MIN(DECODE(T3.SCHE_END_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_END_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_END_DATETIME ")
        '                        .AppendLine("                 ,MAX(TRIM(T4.CST_NAME)) AS CST_NAME ")
        '                        .AppendLine("                 ,MAX(TRIM(T5.VCL_VIN)) AS VCL_VIN ")
        '                        .AppendLine("                 ,MAX(TRIM(T5.VCL_KATASHIKI)) AS VCL_KATASHIKI ")
        '                        .AppendLine("                 ,MAX(TRIM(T6.REG_NUM)) AS REG_NUM ")
        '                        .AppendLine("            FROM  TB_T_SERVICEIN T1 ")
        '                        .AppendLine("                 ,TB_T_JOB_DTL T2 ")
        '                        .AppendLine("                 ,TB_T_STALL_USE T3 ")
        '                        .AppendLine("                 ,TB_M_CUSTOMER T4 ")
        '                        .AppendLine("                 ,TB_M_VEHICLE T5 ")
        '                        .AppendLine("                 ,TB_M_VEHICLE_DLR T6 ")
        '                        .AppendLine("                 ,TBL_USERS T7 ")
        '                        .AppendLine("           WHERE  T1.SVCIN_ID = T2.SVCIN_ID ")
        '                        .AppendLine("             AND  T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
        '                        .AppendLine("             AND  T1.CST_ID = T4.CST_ID (+) ")
        '                        .AppendLine("             AND  T1.VCL_ID = T5.VCL_ID (+) ")
        '                        .AppendLine("             AND  T1.DLR_CD = T6.DLR_CD (+) ")
        '                        .AppendLine("             AND  T1.VCL_ID = T6.VCL_ID (+) ")
        '                        .AppendLine("             AND  T1.PIC_SA_STF_CD = T7.ACCOUNT (+) ")
        '                        .AppendLine("             AND  T1.DLR_CD = :DLRCD ")
        '                        .AppendLine("             AND  T1.BRN_CD = :STRCD ")
        '                        .AppendLine("             AND  T1.RESV_STATUS = :RESV_STATUS ")
        '                        .AppendLine("             AND  NOT EXISTS (SELECT 1 ")
        '                        .AppendLine("                                FROM TB_T_SERVICEIN D1 ")
        '                        .AppendLine("                               WHERE D1.SVCIN_ID = T1.SVCIN_ID ")
        '                        .AppendLine("                                 AND D1.SVC_STATUS = :STATUS_CANCEL) ")
        '                        .AppendLine("             AND  T2.CANCEL_FLG = :CANCEL_FLG ")
        '                        .AppendLine("             AND  T3.DLR_CD = :DLRCD ")
        '                        .AppendLine("             AND  T3.BRN_CD = :STRCD ")
        '                        .AppendLine("             AND  T3.SCHE_START_DATETIME >= TO_DATE(:NOWFROMTIME, 'YYYYMMDDHH24MISS') ")
        '                        .AppendLine("             AND  NOT((T3.TEMP_FLG = :TEMP_FLG AND T3.STALL_USE_STATUS IN (:STATUS_00, :STATUS_01)) OR T3.STALL_USE_STATUS = :STATUS_07)  ")
        '                        .AppendLine("             AND  (TRIM(T1.PIC_SA_STF_CD) IS NULL OR T7.DELFLG = :DELFLG ) ")
        '                        .AppendLine("        GROUP BY  T1.SVCIN_ID ")
        '                        .AppendLine("          HAVING  NVL(MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE , NULL, TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MISS'))) ")
        '                        .AppendLine("                    , MIN(DECODE(T3.SCHE_START_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MISS')))) BETWEEN :NOWFROMTIME  AND :NOWTOTIME ")
        '                        .AppendLine("              OR  NVL(MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE , NULL, TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MISS'))) ")
        '                        .AppendLine("                    , MIN(DECODE(T3.SCHE_START_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MISS')))) BETWEEN :NEXTFROMTIME AND :NEXTTOTIME ")
        '                        .AppendLine("          ) S1 ")
        '                        .AppendLine("         ,TB_T_JOB_DTL T8 ")
        '                        .AppendLine("         ,TB_M_MERCHANDISE T9 ")
        '                        .AppendLine("         ,TBL_SERVICE_VISIT_MANAGEMENT T10 ")
        '                        .AppendLine("         ,TBL_SERVICEIN_APPEND T11 ")
        '                        .AppendLine("  WHERE  S1.SVCIN_ID = T10.FREZID (+) ")
        '                        .AppendLine("    AND  S1.JOB_DTL_ID = T8.JOB_DTL_ID ")
        '                        .AppendLine("    AND  T8.MERC_ID = T9.MERC_ID(+)  ")
        '                        .AppendLine("    AND  S1.CST_ID = T11.CST_ID(+) ")
        '                        .AppendLine("    AND  S1.VCL_ID = T11.VCL_ID(+)  ")
        '                        .AppendLine("    AND  T10.DLRCD(+) = :DLRCD ")
        '                        .AppendLine("    AND  T10.STRCD(+) = :STRCD ")
        '                        .AppendLine("    AND  (T10.ASSIGNSTATUS IS NULL OR T10.ASSIGNSTATUS IN (:ASSIGNSTATUS_0, :ASSIGNSTATUS_1)) ")

        '                    End With


        '                    'パラメータ設定

        '                    '当日フラグ
        '                    query.AddParameterWithTypeValue("TODAY", OracleDbType.NVarchar2, nowDay)
        '                    '日付省略値
        '                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
        '                    '販売店コード
        '                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
        '                    '店舗コード
        '                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
        '                    '予約ステータス
        '                    query.AddParameterWithTypeValue("RESV_STATUS", OracleDbType.NVarchar2, RezStatus)
        '                    'サービスステータス
        '                    query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
        '                    'キャンセルフラグ
        '                    query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)

        '                    '仮置フラグ
        '                    query.AddParameterWithTypeValue("TEMP_FLG", OracleDbType.NVarchar2, TenpFlag)
        '                    'ストール利用ステータス
        '                    query.AddParameterWithTypeValue("STATUS_00", OracleDbType.NVarchar2, StallStatusInstruct)
        '                    query.AddParameterWithTypeValue("STATUS_01", OracleDbType.NVarchar2, StallStatusWaitWork)
        '                    query.AddParameterWithTypeValue("STATUS_07", OracleDbType.NVarchar2, StallStatusUncome)
        '                    '削除フラグ
        '                    query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, Delete)
        '                    '振当ステータス
        '                    query.AddParameterWithTypeValue("ASSIGNSTATUS_0", OracleDbType.NVarchar2, NonAssign)
        '                    query.AddParameterWithTypeValue("ASSIGNSTATUS_1", OracleDbType.NVarchar2, AssignWait)

        '                    '条件(当日)
        '                    '予定入庫日時・予定開始日時
        '                    query.AddParameterWithTypeValue("NOWFROMTIME", OracleDbType.NVarchar2, nowDay + "000000")
        '                    query.AddParameterWithTypeValue("NOWTOTIME", OracleDbType.NVarchar2, nowDay + "235959")
        '                    query.AddParameterWithTypeValue("NEXTFROMTIME", OracleDbType.NVarchar2, nextBusinessDay + "000000")
        '                    query.AddParameterWithTypeValue("NEXTTOTIME", OracleDbType.NVarchar2, nextBusinessDay + "235959")


        '                    'SQL格納
        '                    query.CommandText = sql.ToString()

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                    '検索結果返却
        '                    dt = query.GetData()

        '                End Using

        '            Catch ex As OracleExceptionEx When ex.Number = 1013

        '                'ORACLEのタイムアウトのみ処理
        '                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
        '                   , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
        '                   , Me.GetType.ToString _
        '                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                   , C_RET_DBTIMEOUT _
        '                   , ex.Message))

        '                Throw ex
        '            End Try

        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} {2} OUT:COUNT = {3}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , LOG_END _
        '               , dt.Rows.Count))

        '            Return dt

        '        End Function

        '        '2012/11/21 TMEJ 丁   【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）END

        '#End Region

        '        ' 2012/07/28 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) START
        '#Region "顧客写真情報取得"

        '        ''' <summary>
        '        ''' 顧客写真情報取得
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="customerId">顧客コード</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function GetCustomerPhotoData(ByVal dealerCode As String, _
        '                                             ByVal branchCode As String, _
        '                                             ByVal customerId As String) _
        '                                             As SC3140103DataSet.SC3140103VisitPhotoInfoDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                       , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, CustomerId = {5}" _
        '                                       , Me.GetType.ToString _
        '                                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                       , LOG_START _
        '                                       , dealerCode _
        '                                       , branchCode _
        '                                       , customerId))

        '            Dim dt As SC3140103DataSet.SC3140103VisitPhotoInfoDataTable

        '            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103VisitPhotoInfoDataTable)("SC3140103_012")

        '                Dim sql As New StringBuilder

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'With sql
        '                '    .AppendLine(" SELECT /* SC3140103_012 */ ")
        '                '    .AppendLine("        T1.IMAGEFILE_S ")
        '                '    .AppendLine("      , T1.ORIGINALID ")
        '                '    .AppendLine("   FROM TBLORG_CUSTOMER_APPEND T1 ")
        '                '    .AppendLine("      , TBLORG_CUSTOMER T2 ")
        '                '    .AppendLine("  WHERE T2.ORIGINALID = T1.ORIGINALID ")
        '                '    .AppendLine("    AND T2.DLRCD = :DLRCD ")
        '                '    .AppendLine("    AND T2.STRCD = :STRCD ")
        '                '    .AppendLine("    AND T2.CUSTCD = :CUSTCD ")
        '                '    .AppendLine("    AND T2.DELFLG = '0' ")
        '                '    .AppendLine("    AND ROWNUM = 1 ")
        '                'End With

        '                'query.CommandText = sql.ToString()

        '                ''バインド変数
        '                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '                'query.AddParameterWithTypeValue("CUSTCD", OracleDbType.NVarchar2, customerId)


        '                With sql

        '                    .AppendLine(" SELECT  /* SC3140103_012 */ ")
        '                    .AppendLine("         T1.CST_ID AS ORIGINALID ")
        '                    .AppendLine("        ,TRIM(T2.IMG_FILE_SMALL) AS IMAGEFILE_S ")
        '                    .AppendLine("   FROM  TB_M_CUSTOMER T1 ")
        '                    .AppendLine("        ,TB_M_CUSTOMER_DLR T2 ")
        '                    .AppendLine("  WHERE  T1.CST_ID = T2.CST_ID ")
        '                    .AppendLine("    AND  T1.DMS_CST_CD = :DMS_CST_CD ")
        '                    .AppendLine("    AND  T2.DLR_CD = :DLRCD ")

        '                End With


        '                'パラメータ設定

        '                '販売店コード
        '                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
        '                '基幹顧客ID
        '                query.AddParameterWithTypeValue("DMS_CST_CD", OracleDbType.NVarchar2, customerId)

        '                'SQL格納
        '                query.CommandText = sql.ToString()

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                '検索結果返却
        '                dt = query.GetData()

        '            End Using

        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} {2} OUT:COUNT = {3}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , LOG_END _
        '               , dt.Rows.Count))

        '            Return dt

        '        End Function

        '#End Region

        '#Region "付替え確認用サービス来店管理情報取得"

        '        ''' <summary>
        '        ''' 付替え確認用サービス来店管理情報取得
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="visitNumber">来店実績連番</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function GetServiseVisitManagementForChangeDate(ByVal dealerCode As String, _
        '                                                               ByVal branchCode As String, _
        '                                                               ByVal visitNumber As Long) As SC3140103DataSet.SC3140103ChangesServiceVisitManagementDataTable

        '            Dim dt As SC3140103DataSet.SC3140103ChangesServiceVisitManagementDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                   , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, visitNumber = {5}" _
        '                                   , Me.GetType.ToString _
        '                                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                   , LOG_START _
        '                                   , dealerCode _
        '                                   , branchCode _
        '                                   , visitNumber))

        '            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103ChangesServiceVisitManagementDataTable)("SC3140103_013")
        '                Dim sql As New StringBuilder

        '                With sql
        '                    .AppendLine(" SELECT /* SC3140103_013 */ ")
        '                    .AppendLine("        NVL(FREZID, -1) AS FREZID ")
        '                    .AppendLine("      , CUSTSEGMENT ")
        '                    .AppendLine("	   , ASSIGNSTATUS ")
        '                    .AppendLine("      , SACODE ")
        '                    .AppendLine("      , ORDERNO ")
        '                    .AppendLine("      , ASSIGNTIMESTAMP ")
        '                    .AppendLine("   FROM TBL_SERVICE_VISIT_MANAGEMENT ")
        '                    .AppendLine("  WHERE DLRCD = :DLRCD ")
        '                    .AppendLine("    AND STRCD = :STRCD ")
        '                    .AppendLine("    AND VISITSEQ = :VISITSEQ ")
        '                End With

        '                query.CommandText = sql.ToString()

        '                'バインド変数

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
        '                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, visitNumber)

        '                '検索結果返却
        '                dt = query.GetData()
        '            End Using

        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} {2} OUT:COUNT = {3}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , LOG_END _
        '               , dt.Rows.Count))

        '            Return dt

        '        End Function
        '#End Region

        '#Region "付替え確認用ストール予約情報取得"

        '        ''' <summary>
        '        ''' 付替え確認用ストール予約情報取得
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="reserveId">予約ID</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function GetStallReserveInfoForChangeData(ByVal dealerCode As String, _
        '                                                         ByVal branchCode As String, _
        '                                                         ByVal reserveId As Long) _
        '                                                         As SC3140103DataSet.SC3140103ChangesStallReserveDataTable

        '            Dim dt As SC3140103DataSet.SC3140103ChangesStallReserveDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                       , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, reserveId = {5}" _
        '                                       , Me.GetType.ToString _
        '                                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                       , LOG_START _
        '                                       , dealerCode _
        '                                       , branchCode _
        '                                       , reserveId))

        '            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103ChangesStallReserveDataTable)("SC3140103_014")

        '                Dim sql As New StringBuilder

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'With sql
        '                '    .AppendLine(" SELECT /* SC3140103_014 */ ")
        '                '    .AppendLine("        T1.ACCOUNT_PLAN ")
        '                '    .AppendLine("      , T1.ORDERNO ")
        '                '    .AppendLine("      , NVL(T1.REZ_PICK_DATE, TO_CHAR(T1.STARTTIME, 'YYYYMMDDHH24MI')) AS STOCKTIME ")
        '                '    .AppendLine("      , T1.STARTTIME ")
        '                '    .AppendLine("   FROM TBL_STALLREZINFO T1 ")
        '                '    .AppendLine("  WHERE T1.DLRCD = :DLRCD ")
        '                '    .AppendLine("    AND T1.STRCD = :STRCD ")
        '                '    .AppendLine("    AND(T1.PREZID = :REZID ")
        '                '    .AppendLine("     OR T1.REZID = :REZID) ")
        '                '    .AppendLine("    AND NOT EXISTS ( SELECT 1 ")
        '                '    .AppendLine("                       FROM TBL_STALLREZINFO T2 ")
        '                '    .AppendLine("	                   WHERE T2.DLRCD = T1.DLRCD ")
        '                '    .AppendLine("                        AND T2.STRCD = T1.STRCD ")
        '                '    .AppendLine("                        AND T2.REZID = T1.REZID ")
        '                '    .AppendLine("                        AND((T2.STOPFLG = :STOPFLG0 ")
        '                '    .AppendLine("                        AND T2.CANCELFLG = :CANCELFLG1) ")
        '                '    .AppendLine("                         OR T2.REZCHILDNO IN ( :CHILDNOLEAVE, :CHILDNODELIVERY ) ) ) ")
        '                'End With

        '                'query.CommandText = sql.ToString()

        '                ''バインド変数
        '                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '                'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
        '                'query.AddParameterWithTypeValue("STOPFLG0", OracleDbType.Char, "0")
        '                'query.AddParameterWithTypeValue("CANCELFLG1", OracleDbType.Char, "1")
        '                'query.AddParameterWithTypeValue("CHILDNOLEAVE", OracleDbType.Int64, 0)         ' 子予約連番-0:引取
        '                'query.AddParameterWithTypeValue("CHILDNODELIVERY", OracleDbType.Int64, 999)    ' 子予約連番-999:納車


        '                With sql

        '                    .AppendLine("  SELECT  /* SC3140103_014 */ ")
        '                    .AppendLine("          TRIM(T1.PIC_SA_STF_CD) AS ACCOUNT_PLAN ")
        '                    .AppendLine("         ,TRIM(T1.RO_NUM) AS ORDERNO ")
        '                    .AppendLine("         ,TO_CHAR(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE, T3.SCHE_START_DATETIME, T1.SCHE_SVCIN_DATETIME), 'YYYYMMDDHH24MI') AS STOCKTIME ")
        '                    .AppendLine("         ,DECODE(T3.SCHE_START_DATETIME, :MINDATE, TO_DATE(NULL), T3.SCHE_START_DATETIME) AS STARTTIME ")
        '                    .AppendLine("    FROM  TB_T_SERVICEIN T1 ")
        '                    .AppendLine("         ,TB_T_JOB_DTL T2 ")
        '                    .AppendLine("         ,TB_T_STALL_USE T3 ")
        '                    .AppendLine("   WHERE  T1.SVCIN_ID = T2.SVCIN_ID ")
        '                    .AppendLine("     AND  T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
        '                    .AppendLine("     AND  T1.DLR_CD = :DLRCD ")
        '                    .AppendLine("     AND  T1.BRN_CD = :STRCD ")
        '                    .AppendLine("     AND  T1.SVCIN_ID = :REZID ")
        '                    .AppendLine("     AND  NOT EXISTS (SELECT 1 ")
        '                    .AppendLine("                        FROM TB_T_SERVICEIN D1 ")
        '                    .AppendLine("                       WHERE D1.SVCIN_ID = T1.SVCIN_ID ")
        '                    .AppendLine("                         AND D1.SVC_STATUS = :STATUS_CANCEL) ")
        '                    .AppendLine("     AND  T2.CANCEL_FLG = :CANCEL_FLG ")

        '                End With


        '                'パラメータ設定

        '                '日付省略値
        '                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
        '                '販売店コード
        '                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
        '                '店舗コード
        '                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
        '                'サービス入庫ID
        '                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)
        '                'サービスステータス
        '                query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
        '                'キャンセルフラグ
        '                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)


        '                'SQL格納
        '                query.CommandText = sql.ToString()

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                '検索結果返却
        '                dt = query.GetData()
        '            End Using


        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} {2} OUT:COUNT = {3}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , LOG_END _
        '               , dt.Rows.Count))

        '            Return dt

        '        End Function

        '#End Region

        '#Region "付替え先予約検索"

        '        ''' <summary>
        '        ''' 付替え先予約検索
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="resisterNumber">車両登録No</param>
        '        ''' <param name="vinNumber">VIN</param>
        '        ''' <param name="stockDate">入庫予定日</param> 
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function SearchStallReserveInfoChangeData(ByVal dealerCode As String, _
        '                                                         ByVal branchCode As String, _
        '                                                         ByVal resisterNumber As String, _
        '                                                         ByVal vinNumber As String, _
        '                                                         ByVal stockDate As String) _
        '                                                         As SC3140103DataSet.SC3140103SearchChangesReserveDataTable

        '            Dim dt As SC3140103DataSet.SC3140103SearchChangesReserveDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                   , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, resistrationNumber = {5}, vinNumber = {6}, stockDate = {7}" _
        '                                   , Me.GetType.ToString _
        '                                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                   , LOG_START _
        '                                   , dealerCode _
        '                                   , branchCode _
        '                                   , resisterNumber _
        '                                   , vinNumber _
        '                                   , stockDate))

        '            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103SearchChangesReserveDataTable)("SC3140103_015")

        '                Dim sql As New StringBuilder

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'With sql
        '                '    .AppendLine(" SELECT /* SC3140103_015 */ ")
        '                '    .AppendLine("        NVL(T1.PREZID, T1.REZID) AS RESERVEID ")
        '                '    .AppendLine("      , MAX(T1.ORDERNO)         AS ORDERNO ")
        '                '    .AppendLine("	   , MAX(T1.ACCOUNT_PLAN)    AS ACCOUNT_PLAN ")
        '                '    .AppendLine("      , MIN(NVL(TO_DATE(T1.REZ_PICK_DATE, 'YYYYMMDDHH24MI'), T1.STARTTIME)) AS REZ_PICK_DATE ")
        '                '    .AppendLine("   FROM TBL_STALLREZINFO T1 ")
        '                '    .AppendLine("  WHERE T1.DLRCD = :DLRCD ")
        '                '    .AppendLine("    AND T1.STRCD = :STRCD ")
        '                '    .AppendLine("    AND T1.STARTTIME >= TO_DATE(:TODAY||'0000','YYYYMMDDHH24MI') ")
        '                '    .AppendLine("    AND(T1.VCLREGNO = :VCLREGNO OR T1.VIN = :VIN) ")
        '                '    .AppendLine("    AND(T1.VCLREGNO = :VCLREGNO OR NVL(TRIM(T1.VCLREGNO),' ') = ' ') ")
        '                '    .AppendLine("    AND(T1.VIN = :VIN OR NVL(TRIM(T1.VIN),' ') = ' ') ")
        '                '    .AppendLine("    AND DECODE(T1.STOPFLG,'0',DECODE(T1.CANCELFLG,'1',1,0),0) + ")
        '                '    .AppendLine("        DECODE(T1.REZCHILDNO,0,1,0) + ")
        '                '    .AppendLine("        DECODE(T1.REZCHILDNO,999,1,0) = 0 ")
        '                '    .AppendLine("    AND NOT EXISTS( ")
        '                '    .AppendLine("         SELECT 1 ")
        '                '    .AppendLine("           FROM TBL_STALLREZINFO T2 ")
        '                '    .AppendLine("          WHERE T1.DLRCD = T2.DLRCD ")
        '                '    .AppendLine("            AND T1.STRCD = T2.STRCD ")
        '                '    .AppendLine("            AND(T1.PREZID = T2.PREZID OR T1.REZID = T2.REZID) ")
        '                '    .AppendLine("            AND DECODE(T2.STOPFLG,'0',DECODE(T2.CANCELFLG,'1',1,0),0) + ")
        '                '    .AppendLine("                DECODE(T2.REZCHILDNO,0,1,0) + ")
        '                '    .AppendLine("                DECODE(T2.REZCHILDNO,999,1,0) = 0 ")
        '                '    .AppendLine("            AND NVL(T2.REZ_PICK_DATE,TO_CHAR(T2.STARTTIME,'YYYYMMDDHH24MI')) < :TODAY || '0000') ")
        '                '    .AppendLine("  GROUP BY T1.DLRCD ")
        '                '    .AppendLine("         , T1.STRCD ")
        '                '    .AppendLine("         , NVL(T1.PREZID,T1.REZID) ")
        '                '    .AppendLine(" HAVING MIN(NVL(T1.REZ_PICK_DATE,TO_CHAR(T1.STARTTIME,'YYYYMMDDHH24MI'))) ")
        '                '    .AppendLine("        BETWEEN :TODAY||'0000' AND :TODAY || '2359' ")
        '                'End With

        '                'query.CommandText = sql.ToString()

        '                ''バインド変数
        '                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '                'query.AddParameterWithTypeValue("TODAY", OracleDbType.Char, stockDate)
        '                'query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, resisterNumber)
        '                'query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, vinNumber)


        '                With sql

        '                    .AppendLine("   SELECT  /* SC3140103_015 */ ")
        '                    .AppendLine("           T2.SVCIN_ID AS RESERVEID ")
        '                    .AppendLine("          ,MAX(NVL(T2.CST_ID, 0)) AS CST_ID ")
        '                    .AppendLine("          ,MAX(NVL(T2.VCL_ID, 0)) AS VCL_ID ")
        '                    .AppendLine("          ,MAX(TRIM(T2.RO_NUM)) AS ORDERNO ")
        '                    .AppendLine("          ,MAX(TRIM(T2.PIC_SA_STF_CD)) AS ACCOUNT_PLAN ")
        '                    .AppendLine("          ,TO_CHAR(MIN(DECODE(T2.SCHE_SVCIN_DATETIME, :MINDATE, T4.SCHE_START_DATETIME, T2.SCHE_SVCIN_DATETIME)), 'YYYYMMDDHH24MI') AS REZ_PICK_DATE ")
        '                    .AppendLine("     FROM          ")
        '                    .AppendLine("           (SELECT ")
        '                    .AppendLine("                    V1.VCL_ID ")
        '                    .AppendLine("              FROM  TB_M_VEHICLE V1 ")
        '                    .AppendLine("                   ,TB_M_VEHICLE_DLR V2 ")
        '                    .AppendLine("             WHERE  V1.VCL_ID = V2.VCL_ID ")
        '                    .AppendLine("               AND  (V1.VCL_VIN = :VIN OR V2.REG_NUM = :VCLREGNO) ")
        '                    .AppendLine("               AND  (V1.VCL_VIN = :VIN OR V1.VCL_VIN = N' ')  ")
        '                    .AppendLine("               AND  (V2.REG_NUM = :VCLREGNO OR V2.REG_NUM = N' ') ")
        '                    .AppendLine("               AND  V2.DLR_CD = :DLRCD ")
        '                    .AppendLine("            ) T1 ")
        '                    .AppendLine("           ,TB_T_SERVICEIN T2 ")
        '                    .AppendLine("           ,TB_T_JOB_DTL T3 ")
        '                    .AppendLine("           ,TB_T_STALL_USE T4 ")
        '                    .AppendLine("    WHERE  T1.VCL_ID = T2.VCL_ID ")
        '                    .AppendLine("      AND  T2.SVCIN_ID = T3.SVCIN_ID ")
        '                    .AppendLine("      AND  T3.JOB_DTL_ID = T4.JOB_DTL_ID ")
        '                    .AppendLine("      AND  T2.DLR_CD = :DLRCD ")
        '                    .AppendLine("      AND  T2.BRN_CD = :STRCD ")
        '                    .AppendLine("      AND  NOT EXISTS (SELECT 1 ")
        '                    .AppendLine("                         FROM TB_T_SERVICEIN D1 ")
        '                    .AppendLine("                        WHERE D1.SVCIN_ID = T2.SVCIN_ID ")
        '                    .AppendLine("                          AND D1.SVC_STATUS = :STATUS_CANCEL) ")
        '                    .AppendLine("      AND  T3.CANCEL_FLG = :CANCEL_FLG ")
        '                    .AppendLine("      AND  T4.SCHE_START_DATETIME >= TO_DATE(:TODAY || '000000', 'YYYYMMDDHH24MISS') ")
        '                    .AppendLine(" GROUP BY  T2.SVCIN_ID ")
        '                    .AppendLine("   HAVING  MIN(DECODE(T2.SCHE_SVCIN_DATETIME, :MINDATE, T4.SCHE_START_DATETIME, T2.SCHE_SVCIN_DATETIME)) BETWEEN  TO_DATE(:TODAY || '000000', 'YYYYMMDDHH24MISS') AND  TO_DATE(:TODAY || '235959', 'YYYYMMDDHH24MISS') ")

        '                End With


        '                'パラメータ設定

        '                '日付省略値
        '                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
        '                '車両登録番号
        '                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, resisterNumber)
        '                'VIN
        '                query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vinNumber)
        '                '販売店コード
        '                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
        '                '店舗コード
        '                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
        '                'サービスステータス
        '                query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
        '                'キャンセルフラグ
        '                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)

        '                '条件
        '                '日付
        '                query.AddParameterWithTypeValue("TODAY", OracleDbType.NVarchar2, stockDate)



        '                'SQL格納
        '                query.CommandText = sql.ToString()

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                '検索結果返却
        '                dt = query.GetData()

        '            End Using


        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} {2} OUT:COUNT = {3}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , LOG_END _
        '               , dt.Rows.Count))

        '            Return dt

        '        End Function
        '#End Region

        '#Region "付替え先来店検索"

        '        ''' <summary>
        '        ''' 付替え先来店検索
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="branchCode">店舗コード</param>
        '        ''' <param name="resisterNumber">車両登録No</param>
        '        ''' <param name="vinNumber">VIN</param>
        '        ''' <param name="visitDate">来店日</param> 
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function SearchServiceVisitManagementChangeData(ByVal dealerCode As String, _
        '                                                               ByVal branchCode As String, _
        '                                                               ByVal resisterNumber As String, _
        '                                                               ByVal vinNumber As String, _
        '                                                               ByVal visitDate As String) _
        '                                                               As SC3140103DataSet.SC3140103SearchChangesVisitDataTable

        '            Dim dt As SC3140103DataSet.SC3140103SearchChangesVisitDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, resistrationNumber = {5}, vinNumber = {6}, visitDate = {7}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , LOG_START _
        '               , dealerCode _
        '               , branchCode _
        '               , resisterNumber _
        '               , vinNumber _
        '               , visitDate))

        '            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103SearchChangesVisitDataTable)("SC3140103_016")
        '                Dim sql As New StringBuilder

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'With sql
        '                '    .AppendLine(" SELECT /* SC3140103_016 */ ")
        '                '    .AppendLine("        T1.VISITSEQ ")
        '                '    .AppendLine("      , NVL(T1.FREZID, -1) AS FREZID ")
        '                '    .AppendLine("      , T1.CUSTSEGMENT ")
        '                '    .AppendLine("	   , T1.ASSIGNSTATUS ")
        '                '    .AppendLine("      , T1.SACODE ")
        '                '    .AppendLine("      , T1.ORDERNO ")
        '                '    .AppendLine("      , T1.VISITTIMESTAMP ")
        '                '    .AppendLine("      , T2.ACCOUNT_PLAN ")
        '                '    .AppendLine("   FROM TBL_SERVICE_VISIT_MANAGEMENT T1 ")
        '                '    .AppendLine("      , TBL_STALLREZINFO T2 ")
        '                '    .AppendLine("  WHERE T1.DLRCD = T2.DLRCD(+) ")
        '                '    .AppendLine("    AND T1.STRCD = T2.STRCD(+) ")
        '                '    .AppendLine("    AND T1.FREZID = T2.REZID(+) ")
        '                '    .AppendLine("    AND T1.DLRCD = :DLRCD ")
        '                '    .AppendLine("    AND T1.STRCD = :STRCD ")
        '                '    .AppendLine("    AND T1.VISITTIMESTAMP BETWEEN TO_DATE(:TODAY||'0000','YYYYMMDDHH24MI') ")
        '                '    .AppendLine("                              AND TO_DATE(:TODAY||'2359','YYYYMMDDHH24MI') ")
        '                '    .AppendLine("    AND(T1.VCLREGNO = :VCLREGNO OR T1.VIN = :VIN) ")
        '                '    .AppendLine("    AND(T1.VCLREGNO = :VCLREGNO OR NVL(TRIM(T1.VCLREGNO),' ') = ' ') ")
        '                '    .AppendLine("    AND(T1.VIN = :VIN OR NVL(TRIM(T1.VIN),' ') = ' ') ")
        '                'End With

        '                'query.CommandText = sql.ToString()

        '                ''バインド変数
        '                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
        '                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
        '                'query.AddParameterWithTypeValue("TODAY", OracleDbType.Char, visitDate)
        '                'query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, resisterNumber)
        '                'query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, vinNumber)


        '                With sql

        '                    .AppendLine(" SELECT /* SC3140103_016 */  ")
        '                    .AppendLine("        T1.VISITSEQ  ")
        '                    .AppendLine("      , NVL(T1.FREZID, -1) AS FREZID  ")
        '                    .AppendLine("      , T1.CUSTSEGMENT  ")
        '                    .AppendLine("      , NVL(T1.CUSTID, 0) AS CST_ID ")
        '                    .AppendLine("      , NVL(T1.VCL_ID, 0) AS VCL_ID ")
        '                    .AppendLine(" 	   , T1.ASSIGNSTATUS  ")
        '                    .AppendLine("      , T1.SACODE  ")
        '                    .AppendLine("      , T1.ORDERNO  ")
        '                    .AppendLine("      , T1.VISITTIMESTAMP  ")
        '                    .AppendLine("      , T2.PIC_SA_STF_CD AS ACCOUNT_PLAN ")
        '                    .AppendLine("   FROM TBL_SERVICE_VISIT_MANAGEMENT T1  ")
        '                    .AppendLine("      , TB_T_SERVICEIN T2  ")
        '                    .AppendLine("  WHERE T1.DLRCD = T2.DLR_CD (+)  ")
        '                    .AppendLine("    AND T1.STRCD = T2.BRN_CD (+)  ")
        '                    .AppendLine("    AND T1.FREZID = T2.SVCIN_ID (+)  ")
        '                    .AppendLine("    AND T1.DLRCD = :DLRCD  ")
        '                    .AppendLine("    AND T1.STRCD = :STRCD  ")
        '                    .AppendLine("    AND T1.VISITTIMESTAMP BETWEEN TO_DATE(:TODAY||'0000','YYYYMMDDHH24MI')  ")
        '                    .AppendLine("                              AND TO_DATE(:TODAY||'2359','YYYYMMDDHH24MI')  ")
        '                    .AppendLine("    AND(T1.VCLREGNO = :VCLREGNO OR T1.VIN = :VIN)  ")
        '                    .AppendLine("    AND(T1.VCLREGNO = :VCLREGNO OR NVL(TRIM(T1.VCLREGNO),' ') = ' ')  ")
        '                    .AppendLine("    AND(T1.VIN = :VIN OR NVL(TRIM(T1.VIN),' ') = ' ')  ")

        '                End With


        '                'パラメータ設定

        '                '販売店コード
        '                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
        '                '店舗コード
        '                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
        '                '条件
        '                '日付
        '                query.AddParameterWithTypeValue("TODAY", OracleDbType.NVarchar2, visitDate)
        '                '車両登録番号
        '                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, resisterNumber)
        '                'VIN
        '                query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vinNumber)

        '                'SQL格納
        '                query.CommandText = sql.ToString()

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                '検索結果返却
        '                dt = query.GetData()

        '            End Using

        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} {2} OUT:COUNT = {3}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , LOG_END _
        '               , dt.Rows.Count))

        '            Return dt

        '        End Function
        '#End Region

        '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '#Region "付替え先車両情報検索"

        '        ''' <summary>
        '        ''' 付替え先車両情報検索
        '        ''' </summary>
        '        ''' <param name="dealerCode">販売店コード</param>
        '        ''' <param name="resisterNumber">車両登録番号</param>
        '        ''' <param name="vinNumber">VIN</param>
        '        ''' <returns>付替え先情報</returns>
        '        ''' <remarks></remarks>
        '        Public Function GetDBAfterVehicleInfo(ByVal dealerCode As String, _
        '                                              ByVal resisterNumber As String, _
        '                                              ByVal vinNumber As String) As SC3140103DataSet.SC3140103AfterVehicleInfoDataTable

        '            Dim dt As SC3140103DataSet.SC3140103AfterVehicleInfoDataTable

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} {2} IN:dealerCode = {3}, resistrationNumber = {4}, vinNumber = {5}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , LOG_START _
        '               , dealerCode _
        '               , resisterNumber _
        '               , vinNumber))

        '            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AfterVehicleInfoDataTable)("SC3140103_025")
        '                Dim sql As New StringBuilder

        '                With sql

        '                    .AppendLine("  SELECT /* SC3140103_025 */  ")
        '                    .AppendLine("          T1.VCL_ID ")
        '                    .AppendLine("         ,T4.CST_ID ")
        '                    .AppendLine("    FROM  TB_M_VEHICLE T1 ")
        '                    .AppendLine("         ,TB_M_VEHICLE_DLR T2 ")
        '                    .AppendLine("         ,TB_M_CUSTOMER_VCL T3 ")
        '                    .AppendLine("         ,TB_M_CUSTOMER_DLR T4 ")
        '                    .AppendLine("   WHERE  T1.VCL_ID = T2.VCL_ID ")
        '                    .AppendLine("     AND  T1.VCL_ID = T3.VCL_ID ")
        '                    .AppendLine("     AND  T3.CST_ID = T4.CST_ID ")
        '                    .AppendLine("     AND  (T1.VCL_VIN = :VIN OR T2.REG_NUM = :VCLREGNO) ")
        '                    .AppendLine("     AND  (T1.VCL_VIN = :VIN OR T1.VCL_VIN = N' ')  ")
        '                    .AppendLine("     AND  (T2.REG_NUM = :VCLREGNO OR T2.REG_NUM = N' ') ")
        '                    .AppendLine("     AND  T2.DLR_CD = :DLRCD ")
        '                    .AppendLine("     AND  T3.DLR_CD = :DLRCD ")
        '                    .AppendLine("     AND  T3.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")
        '                    .AppendLine("     AND  T4.DLR_CD = :DLRCD ")
        '                    .AppendLine("ORDER BY  T2.DMS_TAKEIN_DATETIME DESC ")
        '                    .AppendLine("         ,T4.CST_TYPE ASC ")
        '                    .AppendLine("         ,T2.REG_NUM DESC ")
        '                    .AppendLine("         ,T1.VCL_VIN DESC ")
        '                    .AppendLine("         ,T1.VCL_ID DESC ")

        '                End With


        '                'パラメータ設定

        '                '販売店コード
        '                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)

        '                '車両登録番号
        '                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, resisterNumber)

        '                'VIN
        '                query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vinNumber)

        '                'オーナーチェンジフラグ(0：未設定)
        '                query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, NoOwnerChange)

        '                'SQL格納
        '                query.CommandText = sql.ToString()

        '                '検索結果返却
        '                dt = query.GetData()


        '            End Using


        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} {2} OUT:COUNT = {3}" _
        '               , Me.GetType.ToString _
        '               , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '               , LOG_END _
        '               , dt.Rows.Count))

        '            Return dt

        '        End Function

        '#End Region

        '        '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '#Region "サービス来店顧客更新"
        '        ''' <summary>
        '        ''' サービス来店顧客更新
        '        ''' </summary>
        '        ''' <param name="visitNumber">来店実績連番</param>
        '        ''' <param name="customerType">顧客区分</param>
        '        ''' <param name="customerCode">顧客コード</param>
        '        ''' <param name="basicCustomerId">基幹顧客ID</param>
        '        ''' <param name="customerName">氏名</param>
        '        ''' <param name="phone">電話番号</param>
        '        ''' <param name="mobile">携帯番号</param>
        '        ''' <param name="vipMark">VIPマーク</param>
        '        ''' <param name="resisterNumber">車両登録No.</param>
        '        ''' <param name="vin">VIN</param>
        '        ''' <param name="modelCode">モデルコード</param>
        '        ''' <param name="reserveId">予約ID</param>
        '        ''' <param name="orderNumber">整備受注No.</param>
        '        ''' <param name="defaultSACode">SAコード</param>
        '        ''' <param name="assignSACode">振当SA</param>
        '        ''' <param name="updateDate">更新日</param>
        '        ''' <param name="updateAccount">更新アカウント</param>
        '        ''' <param name="updateId">更新機能ID</param>
        '        ''' <param name="afterVehicleId">車両ID</param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function SetVisitCustomer(ByVal visitNumber As Long, _
        '                                         ByVal customerType As String, _
        '                                         ByVal customerCode As String, _
        '                                         ByVal basicCustomerId As String, _
        '                                         ByVal customerName As String, _
        '                                         ByVal phone As String, _
        '                                         ByVal mobile As String, _
        '                                         ByVal vipMark As String, _
        '                                         ByVal resisterNumber As String, _
        '                                         ByVal vin As String, _
        '                                         ByVal modelCode As String, _
        '                                         ByVal reserveId As Long, _
        '                                         ByVal orderNumber As String, _
        '                                         ByVal defaultSACode As String, _
        '                                         ByVal assignSACode As String, _
        '                                         ByVal updateDate As Date, _
        '                                         ByVal updateAccount As String, _
        '                                         ByVal updateId As String, _
        '                                         ByVal afterVehicleId As Long) As Long

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} IN:visitNumber = {3}, customerType = {4}" +
        '                                      ", customerCode = {5}, dmsId = {6}, customerName = {7}, " + _
        '                                      "phone = {8}, mobile = {9}, vipMark = {10}, registerNumber = {11}," +
        '                                      " vin = {12}, modelCode = {13}, reserveId = {14}, " + _
        '                                      "orderNmber = {15}, defaultSaCode = {16}, assignSaCode = {17}," +
        '                                      " updateDate = {18}, updateAccount = {19}, updateId = {20} , afterVehicleId = {21} " _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_START _
        '                                    , visitNumber _
        '                                    , customerType _
        '                                    , customerCode _
        '                                    , basicCustomerId _
        '                                    , customerName _
        '                                    , phone _
        '                                    , mobile _
        '                                    , vipMark _
        '                                    , resisterNumber _
        '                                    , vin _
        '                                    , modelCode _
        '                                    , reserveId _
        '                                    , orderNumber _
        '                                    , defaultSACode _
        '                                    , assignSACode _
        '                                    , updateDate _
        '                                    , updateAccount _
        '                                    , updateId _
        '                                    , afterVehicleId))

        '            Dim count As Long = 0

        '            Using query As New DBUpdateQuery("SC3140103_017")
        '                Dim sql As New StringBuilder

        '                With sql
        '                    .AppendLine(" UPDATE /* SC3140103_017 */ ")
        '                    .AppendLine("        TBL_SERVICE_VISIT_MANAGEMENT ")
        '                    .AppendLine("    SET VCLREGNO = :VCLREGNO ")
        '                    .AppendLine("      , CUSTSEGMENT = :CUSTSEGMENT ")
        '                    .AppendLine("      , CUSTID = :CUSTID ")
        '                    .AppendLine("      , DMSID = :DMSID ")

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        '                    '車両IDを更新
        '                    .AppendLine("      , VCL_ID = :VCL_ID ")

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                    .AppendLine("      , VIN = :VIN ")
        '                    .AppendLine("      , MODELCODE = :MODELCODE ")
        '                    .AppendLine("      , NAME = :NAME ")
        '                    .AppendLine("      , TELNO = :TELNO ")
        '                    .AppendLine("      , MOBILE = :MOBILE ")
        '                    .AppendLine("      , REZID = :REZID ")
        '                    .AppendLine("      , ORDERNO = :ORDERNO ")
        '                    .AppendLine("      , DEFAULTSACODE = :DEFAULTSACODE ")
        '                    .AppendLine("      , SACODE = :SACODE ")
        '                    .AppendLine("      , FREZID = :REZID ")
        '                    .AppendLine("      , UPDATEDATE = :UPDATEDATE ")
        '                    .AppendLine("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
        '                    .AppendLine("      , UPDATEID = :UPDATEID ")
        '                    .AppendLine("  WHERE VISITSEQ = :VISITSEQ ")
        '                End With

        '                query.CommandText = sql.ToString()

        '                'バインド変数
        '                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, resisterNumber)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, customerType)
        '                'query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, customerCode)
        '                query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.NVarchar2, customerType)

        '                'CSTID
        '                Dim longCustomerCode As Long = 0

        '                '引数の顧客IDがLongに変換できるかチェック
        '                If Not Long.TryParse(customerCode, longCustomerCode) Then
        '                    '変換できない場合
        '                    'Nullを設定
        '                    query.AddParameterWithTypeValue("CUSTID", OracleDbType.Int64, DBNull.Value)
        '                Else
        '                    '変換できた場合
        '                    '変換値を設定
        '                    query.AddParameterWithTypeValue("CUSTID", OracleDbType.Int64, longCustomerCode)
        '                End If



        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, basicCustomerId)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                '車両ID確認
        '                If afterVehicleId <= 0 Then
        '                    '車両IDが存在しない

        '                    query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Int64, DBNull.Value)
        '                Else
        '                    '車両IDが存在する

        '                    query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Int64, afterVehicleId)
        '                End If

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, vin)
        '                query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vin)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, modelCode)
        '                query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, customerName)
        '                query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, phone)
        '                query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, mobile)
        '                query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, reserveId)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, orderNumber)
        '                'query.AddParameterWithTypeValue("DEFAULTSACODE", OracleDbType.Varchar2, defaultSACode)
        '                'query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, assignSACode)
        '                query.AddParameterWithTypeValue("ORDERNO", OracleDbType.NVarchar2, orderNumber)
        '                query.AddParameterWithTypeValue("DEFAULTSACODE", OracleDbType.NVarchar2, defaultSACode)
        '                query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, assignSACode)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updateDate)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)
        '                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)
        '                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateAccount)
        '                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, updateId)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, visitNumber)

        '                '検索結果返却
        '                count = query.Execute()
        '            End Using


        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                       , "{0}.{1} {2} OUT:COUNT = {3}" _
        '                       , Me.GetType.ToString _
        '                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                       , LOG_END _
        '                       , count))

        '            Return count

        '        End Function
        '#End Region

        '#Region "サービス来店顧客クリア"
        '        ''' <summary>
        '        ''' サービス来店顧客クリア
        '        ''' </summary>
        '        ''' <param name="visitNumber">来店実績連番</param>
        '        ''' <param name="updateDate">更新日</param>
        '        ''' <param name="updateAccount">更新アカウント</param>
        '        ''' <param name="updateId">更新機能ID</param>
        '        ''' <returns>実行結果</returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function VisitCustomerClear(ByVal visitNumber As Long, _
        '                                           ByVal updateDate As Date, _
        '                                           ByVal updateAccount As String, _
        '                                           ByVal updateId As String) As Long

        '            '開始ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} IN:visitNumber = {3}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_START _
        '                                    , visitNumber))

        '            Dim count As Long = 0

        '            Using query As New DBUpdateQuery("SC3140103_018")
        '                Dim sql As New StringBuilder

        '                With sql
        '                    .AppendLine(" UPDATE /* SC3140103_018 */ ")
        '                    .AppendLine("        TBL_SERVICE_VISIT_MANAGEMENT ")
        '                    .AppendLine("    SET VCLREGNO = NULL ")

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                    '.AppendLine("      , CUSTSEGMENT = '2' ")
        '                    .AppendLine("      , CUSTSEGMENT = N'2' ")

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                    .AppendLine("      , CUSTID = NULL ")
        '                    .AppendLine("      , DMSID = NULL ")

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        '                    '車両IDを更新(NULL)
        '                    .AppendLine("      , VCL_ID = NULL ")

        '                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                    .AppendLine("      , VIN = NULL ")
        '                    .AppendLine("      , MODELCODE = NULL ")
        '                    .AppendLine("      , NAME = NULL ")
        '                    .AppendLine("      , TELNO = NULL ")
        '                    .AppendLine("      , MOBILE = NULL ")
        '                    .AppendLine("      , REZID = NULL ")
        '                    .AppendLine("      , ORDERNO = NULL ")
        '                    .AppendLine("      , DEFAULTSACODE = NULL ")
        '                    .AppendLine("      , FREZID = NULL ")
        '                    .AppendLine("      , UPDATEDATE = :UPDATEDATE ")
        '                    .AppendLine("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
        '                    .AppendLine("      , UPDATEID = :UPDATEID ")
        '                    .AppendLine("  WHERE VISITSEQ = :VISITSEQ ")
        '                End With

        '                query.CommandText = sql.ToString()

        '                'バインド変数
        '                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updateDate)


        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateAccount)
        '                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)
        '                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateAccount)
        '                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, updateId)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, visitNumber)

        '                '検索結果返却
        '                count = query.Execute()
        '            End Using


        '            '終了ログ
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                                    , "{0}.{1} {2} OUT:COUNT = {3}" _
        '                                    , Me.GetType.ToString _
        '                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                                    , LOG_END _
        '                                    , count))

        '            Return count

        '        End Function
        '#End Region
        '        ' 2012/07/28 TMEJ 西岡  【SERVICE_2】STEP2対応(顧客検索機能追加) END

        '        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 START
        '        ''' <summary>
        '        ''' 退店処理
        '        ''' </summary>
        '        ''' <param name="inVisitSequence">来店実績連番</param>
        '        ''' <param name="inNowDate">現在日時</param>
        '        ''' <param name="inAccount">アカウント</param>
        '        ''' <param name="inApplicationId">アプリケーションID</param>
        '        ''' <returns>更新件数取得</returns>
        '        ''' <remarks></remarks>
        '        ''' <history>
        '        ''' 2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </history>
        '        Public Function UpdateVisitSequence(ByVal inVisitSequence As Long, _
        '                                            ByVal inNowDate As Date, _
        '                                            ByVal inAccount As String, _
        '                                            ByVal inApplicationId As String) As Integer
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                      , "{0}.{1} START inVisitSequence:{2} inNowDate:{3} inAccount:{4} inApplicationId:{5}" _
        '                      , Me.GetType.ToString _
        '                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                      , inVisitSequence.ToString(CultureInfo.CurrentCulture) _
        '                      , inNowDate.ToString(CultureInfo.CurrentCulture) _
        '                      , inAccount, inApplicationId))

        '            Using query As New DBUpdateQuery("SC3140103_019")
        '                Dim sql As New StringBuilder
        '                With sql
        '                    .AppendLine("UPDATE /* SC3140103_019 */")
        '                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
        '                    .AppendLine("   SET ASSIGNSTATUS = :ASSIGNSTATUS")      ' 割振りステータス
        '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」START
        '                    '.AppendLine("     , HOLDSTAFF = :HOLDSTAFF")
        '                    .AppendLine("     , HOLDSTAFF = NULL")                  ' HOLD案内係
        '                    '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」END
        '                    .AppendLine("     , SACODE = NULL")                     ' 振当てＳＡ
        '                    .AppendLine("     , ASSIGNTIMESTAMP = NULL")            ' ＳＡ振当て日時
        '                    .AppendLine("     , UPDATEDATE = :UPDATEDATE")          ' 更新日
        '                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")    ' 更新アカウント
        '                    .AppendLine("     , UPDATEID = :UPDATEID")              ' 更新機能ID
        '                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ")              ' 来店実績連番
        '                End With
        '                query.CommandText = sql.ToString()
        '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」START
        '                'query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.Char, "4")              ' 割振りステータス(退店:4)
        '                'query.AddParameterWithTypeValue("HOLDSTAFF", OracleDbType.Varchar2, inAccount)       ' HOLD案内係(自分)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.Char, "1")             
        '                query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, "1")         ' 割振りステータス(受付待:1)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                '2012/12/06 TMEJ 小澤  問連対応「GTMC121204093」END
        '                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inNowDate)          ' 更新日


        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inAccount)  
        '                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, inApplicationId) 
        '                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)  ' 更新アカウント
        '                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, inApplicationId) ' 更新機能ID

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSequence)     ' 来店実績連番

        '                Dim updateCount As Integer = query.Execute()

        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                          , "{0}.{1} END RETURN:{2}" _
        '                          , Me.GetType.ToString _
        '                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                          , updateCount.ToString(CultureInfo.CurrentCulture)))
        '                Return updateCount
        '            End Using
        '        End Function
        '        ' 2012/11/02 TMEJ 小澤 【SERVICE_2】サービス業務管理機能開発 END
        '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　START
        '#Region "お客様呼び出し処理"
        '        ''' <summary>
        '        ''' お客様呼び出し処理
        '        ''' </summary>
        '        ''' <param name="inVisitSequence">来店実績連番</param>
        '        ''' <param name="inupdateDate">更新日時</param>
        '        ''' <param name="inNowdate">現在日時</param>
        '        ''' <param name="inAccount">アカウント</param>
        '        ''' <param name="inApplicationId">アプリケーションID</param>
        '        ''' <returns>更新件数取得</returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function UpdateVisitStausCall(ByVal inVisitSequence As Long, _
        '                                            ByVal inUpdateDate As Date, _
        '                                            ByVal inNowdate As Date, _
        '                                            ByVal inAccount As String, _
        '                                            ByVal inApplicationId As String) As Integer
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                      , "{0}.{1} START inVisitSequence:{2} inUpdateDate:{3} inNowDate:{4} inAccount:{5} inApplicationId:{6}" _
        '                      , Me.GetType.ToString _
        '                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                      , inVisitSequence.ToString(CultureInfo.CurrentCulture) _
        '                      , inUpdateDate _
        '                      , inNowdate.ToString(CultureInfo.CurrentCulture) _
        '                      , inAccount, inApplicationId))

        '            Using query As New DBUpdateQuery("SC3140103_021")
        '                Dim sql As New StringBuilder
        '                With sql
        '                    .AppendLine("UPDATE /* SC3140103_021 */ ")
        '                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT ")
        '                    .AppendLine("   SET CALLSTATUS = :CALLING ")      ' 呼出ステータス
        '                    .AppendLine("     , CALLSTARTDATE = :NOWDATE ")    '呼出開始日時
        '                    .AppendLine("     , UPDATEDATE = :NOWDATE ")          ' 更新日
        '                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT ")    ' 更新アカウント
        '                    .AppendLine("     , UPDATEID = :UPDATEID ")              ' 更新機能ID
        '                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ ")              ' 来店実績連番
        '                    .AppendLine("     AND UPDATEDATE = :UPDATEDATE ")
        '                    .AppendLine("     AND CALLSTATUS = :NOTCALL ")
        '                    .AppendLine("     AND SACODE = :UPDATEACCOUNT ")
        '                End With
        '                query.CommandText = sql.ToString()


        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("CALLING", OracleDbType.Char, "1")
        '                'query.AddParameterWithTypeValue("NOTCALL", OracleDbType.Char, "0")
        '                query.AddParameterWithTypeValue("CALLING", OracleDbType.NVarchar2, "1")               ' 呼出ステータス(呼び出し中:1)
        '                query.AddParameterWithTypeValue("NOTCALL", OracleDbType.NVarchar2, "0")               ' 呼出ステータス(未呼出:0)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpdateDate)        ' 更新日


        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inAccount)
        '                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, inApplicationId)
        '                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)   ' 更新アカウント
        '                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, inApplicationId)  ' 更新機能ID

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSequence)     ' 来店実績連番
        '                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowdate)             ' 現在日時

        '                Dim updateCount As Integer = query.Execute()

        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                          , "{0}.{1} END RETURN:{2}" _
        '                          , Me.GetType.ToString _
        '                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                          , updateCount.ToString(CultureInfo.CurrentCulture)))
        '                Return updateCount
        '            End Using
        '        End Function
        '#End Region

        '#Region "お客様呼び出しキャンセル処理"
        '        ''' <summary>
        '        ''' お客様呼び出しキャンセル処理
        '        ''' </summary>
        '        ''' <param name="inVisitSequence">来店実績連番</param>
        '        ''' <param name="inupdateDate">更新日時</param>
        '        ''' <param name="inNowdate">現在日時</param>
        '        ''' <param name="inAccount">アカウント</param>
        '        ''' <param name="inApplicationId">アプリケーションID</param>
        '        ''' <returns>更新件数取得</returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function UpdateVisitStausCallCancel(ByVal inVisitSequence As Long, _
        '                                            ByVal inUpdateDate As Date, _
        '                                            ByVal inNowdate As Date, _
        '                                            ByVal inAccount As String, _
        '                                            ByVal inApplicationId As String) As Integer
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                      , "{0}.{1} START inVisitSequence:{2} inUpdateDate:{3} inNowDate:{4} inAccount:{5} inApplicationId:{6}" _
        '                      , Me.GetType.ToString _
        '                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                      , inVisitSequence.ToString(CultureInfo.CurrentCulture) _
        '                      , inUpdateDate _
        '                      , inNowdate.ToString(CultureInfo.CurrentCulture) _
        '                      , inAccount, inApplicationId))

        '            Using query As New DBUpdateQuery("SC3140103_022")
        '                Dim sql As New StringBuilder
        '                With sql
        '                    .AppendLine("UPDATE /* SC3140103_022 */ ")
        '                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT ")
        '                    .AppendLine("   SET CALLSTATUS = :NOTCALL ")             ' 呼出ステータス
        '                    .AppendLine("     , CALLSTARTDATE = '' ")                ' 呼出開始日時
        '                    .AppendLine("     , UPDATEDATE = :NOWDATE ")             ' 更新日
        '                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT ")    ' 更新アカウント
        '                    .AppendLine("     , UPDATEID = :UPDATEID ")              ' 更新機能ID
        '                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ ")              ' 来店実績連番
        '                    .AppendLine("     AND UPDATEDATE = :UPDATEDATE ")
        '                    .AppendLine("     AND CALLSTATUS = :CALLING ")
        '                    .AppendLine("     AND SACODE = :UPDATEACCOUNT ")
        '                End With
        '                query.CommandText = sql.ToString()


        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("CALLING", OracleDbType.Char, "1")             
        '                'query.AddParameterWithTypeValue("NOTCALL", OracleDbType.Char, "0")             
        '                query.AddParameterWithTypeValue("CALLING", OracleDbType.NVarchar2, "1")              ' 呼出ステータス(呼び出し中:1)
        '                query.AddParameterWithTypeValue("NOTCALL", OracleDbType.NVarchar2, "0")              ' 呼出ステータス(未呼出:0)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpdateDate)       ' 更新日

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inAccount)  
        '                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, inApplicationId) 
        '                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)  ' 更新アカウント
        '                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, inApplicationId) ' 更新機能ID

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSequence)     ' 来店実績連番
        '                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowdate)             ' 現在日時

        '                Dim updateCount As Integer = query.Execute()

        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                          , "{0}.{1} END RETURN:{2}" _
        '                          , Me.GetType.ToString _
        '                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                          , updateCount.ToString(CultureInfo.CurrentCulture)))
        '                Return updateCount
        '            End Using
        '        End Function
        '#End Region

        '#Region "呼び出し場所更新"
        '        ''' <summary>
        '        ''' 呼び出し場所更新
        '        ''' </summary>
        '        ''' <param name="inVisitSequence">来店実績連番</param>
        '        ''' <param name="inCallPlace">呼出場所</param>
        '        ''' <param name="inupdateDate">更新日時</param>
        '        ''' <param name="inNowdate">現在日時</param>
        '        ''' <param name="inAccount">アカウント</param>
        '        ''' <param name="inApplicationId">アプリケーションID</param>
        '        ''' <returns>更新件数取得</returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function UpdateCallPlace(ByVal inVisitSequence As Long, _
        '                                            ByVal inCallPlace As String, _
        '                                            ByVal inUpdateDate As Date, _
        '                                            ByVal inNowdate As Date, _
        '                                            ByVal inAccount As String, _
        '                                            ByVal inApplicationId As String) As Integer
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                      , "{0}.{1} START inVisitSequence:{2} inCallPlace:{3} inUpdateDate:{4} inNowDate:{5} inAccount:{6} inApplicationId:{7}" _
        '                      , Me.GetType.ToString _
        '                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                      , inVisitSequence.ToString(CultureInfo.CurrentCulture) _
        '                      , inCallPlace, inUpdateDate _
        '                      , inNowdate.ToString(CultureInfo.CurrentCulture) _
        '                      , inAccount, inApplicationId))

        '            Using query As New DBUpdateQuery("SC3140103_022")
        '                Dim sql As New StringBuilder
        '                With sql
        '                    .AppendLine("UPDATE /* SC3140103_022 */ ")
        '                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT ")
        '                    .AppendLine("   SET CALLPLACE = :CALLPLACE ")            ' 呼出ステータス
        '                    .AppendLine("     , UPDATEDATE = :NOWDATE ")             ' 更新日
        '                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT ")    ' 更新アカウント
        '                    .AppendLine("     , UPDATEID = :UPDATEID ")              ' 更新機能ID
        '                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ ")              ' 来店実績連番
        '                    .AppendLine("     AND UPDATEDATE = :UPDATEDATE ")
        '                    .AppendLine("     AND CALLSTATUS = :NOTCALL ")
        '                    .AppendLine("     AND SACODE = :UPDATEACCOUNT ")
        '                End With
        '                query.CommandText = sql.ToString()

        '                query.AddParameterWithTypeValue("CALLPLACE", OracleDbType.NVarchar2, inCallPlace)    ' 呼出場所

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("NOTCALL", OracleDbType.Char, "0")            
        '                query.AddParameterWithTypeValue("NOTCALL", OracleDbType.NVarchar2, "0")              ' 呼出ステータス(未呼出:0)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpdateDate)       ' 更新日


        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inAccount)  
        '                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, inApplicationId) 
        '                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)  ' 更新アカウント
        '                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, inApplicationId) ' 更新機能ID

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSequence)     ' 来店実績連番
        '                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowdate)             ' 現在日時

        '                Dim updateCount As Integer = query.Execute()

        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                          , "{0}.{1} END RETURN:{2}" _
        '                          , Me.GetType.ToString _
        '                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                          , updateCount.ToString(CultureInfo.CurrentCulture)))
        '                Return updateCount
        '            End Using
        '        End Function
        '#End Region

        '#Region "呼び出し完了"
        '        ''' <summary>
        '        ''' 呼び出し完了更新
        '        ''' </summary>
        '        ''' <param name="inVisitSequence">来店実績連番</param>
        '        ''' <param name="inupdateDate">更新日時</param>
        '        ''' <param name="inNowdate">現在日時</param>
        '        ''' <param name="inAccount">アカウント</param>
        '        ''' <param name="inApplicationId">アプリケーションID</param>
        '        ''' <returns>更新件数取得</returns>
        '        ''' <remarks></remarks>
        '        ''' <History>
        '        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        '        ''' </History>
        '        Public Function UpdateCallCompleted(ByVal inVisitSequence As Long, _
        '                                            ByVal inUpdateDate As Date, _
        '                                            ByVal inNowdate As Date, _
        '                                            ByVal inAccount As String, _
        '                                            ByVal inApplicationId As String) As Integer
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                      , "{0}.{1} START inVisitSequence:{2} inUpdateDate:{3} inNowDate:{4} inAccount:{5} inApplicationId:{6}" _
        '                      , Me.GetType.ToString _
        '                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                      , inVisitSequence.ToString(CultureInfo.CurrentCulture) _
        '                      , inUpdateDate _
        '                      , inNowdate.ToString(CultureInfo.CurrentCulture) _
        '                      , inAccount, inApplicationId))

        '            Using query As New DBUpdateQuery("SC3140103_023")
        '                Dim sql As New StringBuilder
        '                With sql
        '                    .AppendLine("UPDATE /* SC3140103_023 */ ")
        '                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT ")
        '                    .AppendLine("   SET CALLSTATUS = :CALLED ")              ' 呼出ステータス
        '                    .AppendLine("     , CALLENDDATE = :NOWDATE ")            ' 呼出完了日時
        '                    .AppendLine("     , UPDATEDATE = :NOWDATE ")             ' 更新日
        '                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT ")    ' 更新アカウント
        '                    .AppendLine("     , UPDATEID = :UPDATEID ")              ' 更新機能ID
        '                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ ")              ' 来店実績連番
        '                    .AppendLine("     AND SACODE = :UPDATEACCOUNT ")
        '                    .AppendLine("     AND UPDATEDATE = :UPDATEDATE ")
        '                End With
        '                query.CommandText = sql.ToString()

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("CALLED", OracleDbType.Char, "2")             
        '                query.AddParameterWithTypeValue("CALLED", OracleDbType.NVarchar2, "2")                 ' 呼出ステータス(呼出完了:2)

        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpdateDate)         ' 更新日


        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inAccount)
        '                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, inApplicationId)
        '                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)     ' 更新アカウント
        '                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, inApplicationId)    ' 更新機能ID


        '                '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSequence)       ' 来店実績連番
        '                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowdate)               ' 現在日時

        '                Dim updateCount As Integer = query.Execute()

        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                          , "{0}.{1} END RETURN:{2}" _
        '                          , Me.GetType.ToString _
        '                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                          , updateCount.ToString(CultureInfo.CurrentCulture)))
        '                Return updateCount
        '            End Using
        '        End Function
        '#End Region
        '        '2013/03/12 TMEJ 丁 【A.STEP1】来店受付管理オペレーション確立に向けた評価アプリ作成　END


        '2013/12/10 TMEJ 河原 TMEJ次世代サービス 工程管理機能開発 END

#Region "各工程エリアチップ情報取得"

        ''' <summary>
        ''' 振当待ちエリアチップ情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inPresentTime">現在時間</param>
        ''' <returns>振当待ちエリアチップ情報取得データセット</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発
        ''' 2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ''' </history>
        Public Function GetAssignmentChipInfo(ByVal inDealerCode As String _
                                            , ByVal inBranchCode As String _
                                            , ByVal inPresentTime As Date) _
                                              As SC3140103DataSet.SC3140103VisitManagementInfoDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN: PRESENTTIME = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , inPresentTime))

            Dim dt As SC3140103DataSet.SC3140103VisitManagementInfoDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103VisitManagementInfoDataTable)("SC3140103_026")
                    Dim sql As New StringBuilder

                    'SQL文作成
                    With sql

                        .AppendLine("  SELECT /* SC3140103_026 */ ")
                        .AppendLine("         T1.VISITSEQ ")
                        .AppendLine("        ,T1.VISITTIMESTAMP ")
                        '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                        '.AppendLine("        ,TRIM(T1.VCLREGNO) AS VCLREGNO ")
                        .AppendLine("        ,NVL(TRIM(T6.REG_NUM), TRIM(T1.VCLREGNO)) AS VCLREGNO ")
                        '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END

                        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                        '.AppendLine("        ,TRIM(T1.NAME) AS NAME ")
                        .AppendLine("        ,NVL(TRIM(T1.VISITNAME), TRIM(T1.NAME)) AS NAME ")
                        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                        .AppendLine("        ,TRIM(T1.ORDERNO) AS ORDERNO ")
                        .AppendLine("        ,T1.FREZID AS REZID ")
                        .AppendLine("        ,CASE ")
                        .AppendLine("              WHEN T2.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_0 THEN :ICON_FLAG_ON ")
                        .AppendLine("              ELSE :ICON_FLAG_OFF ")
                        .AppendLine("          END AS REZ_MARK ")
                        .AppendLine("        ,TRIM(T1.PARKINGCODE) AS PARKINGCODE ")
                        .AppendLine("        ,T1.UPDATEDATE ")
                        .AppendLine("        ,T2.SCHE_START_DATETIME ")
                        .AppendLine("        ,NVL(CONCAT(TRIM(T4.UPPER_DISP), TRIM(T4.LOWER_DISP)), NVL(T5.SVC_CLASS_NAME, T5.SVC_CLASS_NAME_ENG)) AS MERCHANDISENAME ")
                        .AppendLine("        ,CASE  ")

                        '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 START

                        '.AppendLine("              WHEN T6.VIP_FLG = :ICON_FLAG_ON THEN T6.VIP_FLG ")

                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                        '.AppendLine("              WHEN T6.IMP_VCL_FLG = :ICON_FLAG_ON THEN T6.IMP_VCL_FLG ")
                        .AppendLine("              WHEN T6.IMP_VCL_FLG = :ICON_FLAG_ON THEN T6.IMP_VCL_FLG ")
                        .AppendLine("              WHEN T6.IMP_VCL_FLG = :ICON_FLAG_ON_2 THEN T6.IMP_VCL_FLG ")
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

                        '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 END

                        .AppendLine("              ELSE :ICON_FLAG_OFF  ")
                        .AppendLine("          END AS JDP_MARK ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                        .AppendLine("        ,T7.SPECIAL_CAMPAIGN_TGT_FLG AS SSC_MARK ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                        .AppendLine("        ,NVL(TRIM(T8.SML_AMC_FLG), :ICON_FLAG_OFF) AS SML_AMC_FLG")
                        .AppendLine("        ,NVL(TRIM(T8.EW_FLG), :ICON_FLAG_OFF) AS EW_FLG")
                        .AppendLine("        ,CASE ")
                        .AppendLine("               WHEN T9.VCL_VIN IS NOT NULL THEN :ICON_FLAG_ON ")
                        .AppendLine("               ELSE :ICON_FLAG_OFF ")
                        .AppendLine("         END AS TLM_MBR_FLG ")
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                        .AppendLine("    FROM TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                        .AppendLine("        ,(SELECT S2.SVCIN_ID ")
                        .AppendLine("                ,MAX(TRIM(S2.ACCEPTANCE_TYPE)) AS ACCEPTANCE_TYPE ")
                        .AppendLine("                ,MIN(S3.JOB_DTL_ID) AS JOB_DTL_ID ")
                        .AppendLine("                ,MIN(S4.SCHE_START_DATETIME) AS SCHE_START_DATETIME ")
                        .AppendLine("            FROM TBL_SERVICE_VISIT_MANAGEMENT S1 ")
                        .AppendLine("                ,TB_T_SERVICEIN S2 ")
                        .AppendLine("                ,TB_T_JOB_DTL S3 ")
                        .AppendLine("                ,TB_T_STALL_USE S4 ")
                        .AppendLine("           WHERE S1.FREZID = S2.SVCIN_ID ")
                        .AppendLine("             AND S2.SVCIN_ID = S3.SVCIN_ID ")
                        .AppendLine("             AND S3.JOB_DTL_ID = S4.JOB_DTL_ID ")
                        .AppendLine("             AND S1.DLRCD = :DLRCD ")
                        .AppendLine("             AND S1.STRCD = :STRCD ")
                        .AppendLine("             AND S1.VISITTIMESTAMP ")
                        .AppendLine("         BETWEEN TRUNC(:VISITTIMESTAMP) ")
                        .AppendLine("             AND TRUNC(:VISITTIMESTAMP) + 86399/86400 ")
                        .AppendLine("             AND S1.ASSIGNSTATUS IN (:ASSIGNSTATUS_RECEPTION, :ASSIGNSTATUS_WAIT) ")
                        .AppendLine("             AND S2.DLR_CD = :DLRCD ")
                        .AppendLine("             AND S2.BRN_CD = :STRCD ")
                        .AppendLine("             AND S2.SVC_STATUS <> :STATUS_CANCEL ")
                        .AppendLine("             AND S3.DLR_CD = :DLRCD ")
                        .AppendLine("             AND S3.BRN_CD = :STRCD ")
                        .AppendLine("             AND S3.CANCEL_FLG = :CANCELFLG ")
                        .AppendLine("             AND S4.DLR_CD = :DLRCD ")
                        .AppendLine("             AND S4.BRN_CD = :STRCD ")
                        .AppendLine("        GROUP BY S2.SVCIN_ID) T2 ")
                        .AppendLine("        ,TB_T_JOB_DTL T3 ")
                        .AppendLine("        ,TB_M_MERCHANDISE T4 ")
                        .AppendLine("        ,TB_M_SERVICE_CLASS T5 ")
                        .AppendLine("        ,TB_M_VEHICLE_DLR T6 ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                        .AppendLine("        ,TB_M_VEHICLE T7 ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                        .AppendLine("        ,TB_LM_VEHICLE T8 ")
                        .AppendLine("        ,TB_LM_TLM_MEMBER T9 ")
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                        .AppendLine("   WHERE T1.FREZID = T2.SVCIN_ID(+) ")
                        .AppendLine("     AND T2.JOB_DTL_ID = T3.JOB_DTL_ID(+) ")
                        .AppendLine("     AND T3.MERC_ID = T4.MERC_ID(+) ")
                        .AppendLine("     AND T3.SVC_CLASS_ID = T5.SVC_CLASS_ID(+) ")
                        .AppendLine("     AND T1.VCL_ID = T6.VCL_ID(+) ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                        .AppendLine("     AND T1.VCL_ID = T7.VCL_ID(+) ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                        .AppendLine("     AND T7.VCL_ID = T8.VCL_ID(+) ")
                        .AppendLine("     AND T7.VCL_VIN = T9.VCL_VIN(+) ")
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                        .AppendLine("     AND T1.DLRCD = :DLRCD ")
                        .AppendLine("     AND T1.STRCD = :STRCD ")
                        .AppendLine("     AND T1.VISITTIMESTAMP ")
                        .AppendLine(" BETWEEN TRUNC(:VISITTIMESTAMP) ")
                        .AppendLine("     AND TRUNC(:VISITTIMESTAMP) + 86399/86400 ")
                        .AppendLine("     AND T1.ASSIGNSTATUS IN (:ASSIGNSTATUS_RECEPTION, :ASSIGNSTATUS_WAIT) ")
                        .AppendLine("     AND T3.DLR_CD(+) = :DLRCD ")
                        .AppendLine("     AND T3.BRN_CD(+) = :STRCD ")
                        .AppendLine("     AND T6.DLR_CD(+) = :DLRCD ")
                        .AppendLine("ORDER BY T1.VISITTIMESTAMP ASC ")
                        .AppendLine("        ,T2.SCHE_START_DATETIME ASC ")

                    End With

                    'SQL設定
                    query.CommandText = sql.ToString()

                    'バインド変数
                    '販売店コード
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                    '店舗コード
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)
                    '受付区分("0"：予約客)
                    query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_0", OracleDbType.NVarchar2, AcceptanceTypeRez)
                    '表示アイコンフラグ("0"：非表示)
                    query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                    '表示アイコンフラグ("1"：表示)
                    query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)
                    '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                    '表示アイコンフラグ2("2"：表示)
                    query.AddParameterWithTypeValue("ICON_FLAG_ON_2", OracleDbType.NVarchar2, IconFlagOn2)
                    '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                    '来店日時
                    query.AddParameterWithTypeValue("VISITTIMESTAMP", OracleDbType.Date, inPresentTime)
                    '振当てステータス(0：受付待ち)
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_RECEPTION", OracleDbType.NVarchar2, NonAssign)
                    '振当てステータス(1：振当待ち)
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_WAIT", OracleDbType.NVarchar2, AssignWait)
                    'サービスステータス
                    query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
                    'キャンセルフラグ
                    query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.NVarchar2, CancelFlagEffective)

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

        ''' <summary>
        ''' 受付エリアチップ情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inAccount">ログインSAアカウント</param>
        ''' <param name="inPresentTime">現在日付</param>
        ''' <returns>受付エリアチップ情報取得データセット</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発
        ''' 2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ''' </history>
        Public Function GetReceptionChipInfo(ByVal inDealerCode As String _
                                           , ByVal inBranchCode As String _
                                           , ByVal inAccount As String _
                                           , ByVal inPresentTime As Date) As SC3140103DataSet.SC3140103VisitManagementInfoDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN: ACCOUNT = {3} PRESENTTIME = {4}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , inPresentTime, inAccount))

            Dim dt As SC3140103DataSet.SC3140103VisitManagementInfoDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103VisitManagementInfoDataTable)("SC3140103_027")
                    Dim sql As New StringBuilder

                    'SQL文作成
                    With sql

                        .AppendLine("  SELECT /* SC3140103_027 */ ")
                        .AppendLine("         T1.VISITSEQ ")
                        .AppendLine("        ,T1.VISITTIMESTAMP ")
                        '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                        '.AppendLine("        ,TRIM(T1.VCLREGNO) AS VCLREGNO ")
                        .AppendLine("        ,NVL(TRIM(T6.REG_NUM), TRIM(T1.VCLREGNO)) AS VCLREGNO ")
                        '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END

                        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                        '.AppendLine("        ,TRIM(T1.NAME) AS NAME ")
                        .AppendLine("        ,NVL(TRIM(T1.VISITNAME), TRIM(T1.NAME)) AS NAME ")
                        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                        .AppendLine("        ,TRIM(T1.ORDERNO) AS ORDERNO ")
                        .AppendLine("        ,T1.FREZID AS REZID ")
                        .AppendLine("        ,TRIM(T1.PARKINGCODE) AS PARKINGCODE ")
                        .AppendLine("        ,T1.UPDATEDATE ")
                        .AppendLine("        ,T1.CALLSTATUS ")
                        .AppendLine("        ,CASE ")
                        .AppendLine("              WHEN T2.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_0 THEN :ICON_FLAG_ON ")
                        .AppendLine("              ELSE :ICON_FLAG_OFF ")
                        .AppendLine("          END AS REZ_MARK ")
                        .AppendLine("        ,T2.SCHE_START_DATETIME ")
                        .AppendLine("        ,NVL(CONCAT(TRIM(T4.UPPER_DISP), TRIM(T4.LOWER_DISP)), NVL(T5.SVC_CLASS_NAME, T5.SVC_CLASS_NAME_ENG)) AS MERCHANDISENAME ")
                        .AppendLine("        ,CASE ")

                        '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 START

                        '.AppendLine("              WHEN T6.VIP_FLG = :ICON_FLAG_ON THEN T6.VIP_FLG ")

                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                        '.AppendLine("              WHEN T6.IMP_VCL_FLG = :ICON_FLAG_ON THEN T6.IMP_VCL_FLG ")
                        .AppendLine("              WHEN T6.IMP_VCL_FLG = :ICON_FLAG_ON THEN T6.IMP_VCL_FLG ")
                        .AppendLine("              WHEN T6.IMP_VCL_FLG = :ICON_FLAG_ON_2 THEN T6.IMP_VCL_FLG ")
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

                        '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 END

                        .AppendLine("              ELSE :ICON_FLAG_OFF  ")
                        .AppendLine("          END AS JDP_MARK ")
                        .AppendLine("        ,T7.VISIT_ID AS RO_INFO_VISIT_ID ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                        .AppendLine("        ,T8.SPECIAL_CAMPAIGN_TGT_FLG AS SSC_MARK ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                        .AppendLine("        ,NVL(TRIM(T9.SML_AMC_FLG), :ICON_FLAG_OFF) AS SML_AMC_FLG ")
                        .AppendLine("        ,NVL(TRIM(T9.EW_FLG), :ICON_FLAG_OFF) AS EW_FLG ")
                        .AppendLine("        ,CASE ")
                        .AppendLine("               WHEN T10.VCL_VIN IS NOT NULL THEN :ICON_FLAG_ON")
                        .AppendLine("               ELSE :ICON_FLAG_OFF")
                        .AppendLine("         END AS TLM_MBR_FLG")
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                        .AppendLine("    FROM TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                        .AppendLine("        ,(SELECT S2.SVCIN_ID ")
                        .AppendLine("                ,MAX(TRIM(S2.ACCEPTANCE_TYPE)) AS ACCEPTANCE_TYPE ")
                        .AppendLine("                ,MIN(S3.JOB_DTL_ID) AS JOB_DTL_ID ")
                        .AppendLine("                ,MIN(S4.SCHE_START_DATETIME) AS SCHE_START_DATETIME ")
                        .AppendLine("            FROM TBL_SERVICE_VISIT_MANAGEMENT S1 ")
                        .AppendLine("                ,TB_T_SERVICEIN S2 ")
                        .AppendLine("                ,TB_T_JOB_DTL S3 ")
                        .AppendLine("                ,TB_T_STALL_USE S4 ")
                        .AppendLine("           WHERE S1.FREZID = S2.SVCIN_ID ")
                        .AppendLine("             AND S2.SVCIN_ID = S3.SVCIN_ID ")
                        .AppendLine("             AND S3.JOB_DTL_ID = S4.JOB_DTL_ID ")
                        .AppendLine("             AND S1.DLRCD = :DLRCD ")
                        .AppendLine("             AND S1.STRCD = :STRCD ")
                        .AppendLine("             AND S1.VISITTIMESTAMP ")
                        .AppendLine("         BETWEEN TRUNC(:VISITTIMESTAMP) ")
                        .AppendLine("             AND TRUNC(:VISITTIMESTAMP) + 86399/86400 ")
                        .AppendLine("             AND S1.SACODE = :SACODE ")
                        .AppendLine("             AND S1.ASSIGNSTATUS IN (:ASSIGNSTATUS_FIN) ")
                        .AppendLine("             AND S1.ORDERNO IS NULL ")
                        .AppendLine("             AND S2.DLR_CD = :DLRCD ")
                        .AppendLine("             AND S2.BRN_CD = :STRCD ")
                        .AppendLine("             AND S2.SVC_STATUS <> :STATUS_CANCEL ")
                        .AppendLine("             AND S3.DLR_CD = :DLRCD ")
                        .AppendLine("             AND S3.BRN_CD = :STRCD ")
                        .AppendLine("             AND S3.CANCEL_FLG = :CANCELFLG ")
                        .AppendLine("             AND S4.DLR_CD = :DLRCD ")
                        .AppendLine("             AND S4.BRN_CD = :STRCD ")
                        .AppendLine("        GROUP BY S2.SVCIN_ID) T2 ")
                        .AppendLine("        ,TB_T_JOB_DTL T3 ")
                        .AppendLine("        ,TB_M_MERCHANDISE T4 ")
                        .AppendLine("        ,TB_M_SERVICE_CLASS T5 ")
                        .AppendLine("        ,TB_M_VEHICLE_DLR T6 ")
                        .AppendLine("        ,TB_T_RO_INFO T7 ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                        .AppendLine("        ,TB_M_VEHICLE T8 ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                        .AppendLine("        ,TB_LM_VEHICLE T9 ")
                        .AppendLine("        ,TB_LM_TLM_MEMBER T10 ")
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                        .AppendLine("   WHERE T1.FREZID = T2.SVCIN_ID(+) ")
                        .AppendLine("     AND T2.JOB_DTL_ID = T3.JOB_DTL_ID(+) ")
                        .AppendLine("     AND T3.MERC_ID = T4.MERC_ID(+) ")
                        .AppendLine("     AND T3.SVC_CLASS_ID = T5.SVC_CLASS_ID(+) ")
                        .AppendLine("     AND T1.VCL_ID = T6.VCL_ID(+) ")
                        .AppendLine("     AND T1.VISITSEQ = T7.VISIT_ID(+) ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                        .AppendLine("     AND T1.VCL_ID = T8.VCL_ID(+) ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                        .AppendLine("     AND T8.VCL_ID = T9.VCL_ID(+) ")
                        .AppendLine("     AND T8.VCL_VIN = T10.VCL_VIN(+) ")
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                        .AppendLine("     AND T1.DLRCD = :DLRCD ")
                        .AppendLine("     AND T1.STRCD = :STRCD ")
                        .AppendLine("     AND T1.VISITTIMESTAMP ")
                        .AppendLine(" BETWEEN TRUNC(:VISITTIMESTAMP) ")
                        .AppendLine("     AND TRUNC(:VISITTIMESTAMP) + 86399/86400 ")
                        .AppendLine("     AND T1.SACODE = :SACODE ")
                        .AppendLine("     AND T1.ASSIGNSTATUS IN (:ASSIGNSTATUS_FIN) ")
                        .AppendLine("     AND T1.ORDERNO IS NULL ")
                        .AppendLine("     AND T3.DLR_CD(+) = :DLRCD ")
                        .AppendLine("     AND T3.BRN_CD(+) = :STRCD ")
                        .AppendLine("     AND T6.DLR_CD(+) = :DLRCD ")
                        .AppendLine("     AND T7.DLR_CD(+) = :DLRCD ")
                        .AppendLine("     AND T7.BRN_CD(+) = :STRCD ")
                        .AppendLine("     AND T7.RO_STATUS(+) <> :STATUS_99 ")
                        .AppendLine("ORDER BY T1.VISITTIMESTAMP ASC ")
                        .AppendLine("        ,T2.SCHE_START_DATETIME ASC ")

                    End With

                    'SQL設定
                    query.CommandText = sql.ToString()

                    'バインド変数
                    '販売店コード
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                    '店舗コード
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)
                    '受付区分("0"：予約客)
                    query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_0", OracleDbType.NVarchar2, AcceptanceTypeRez)
                    '表示アイコンフラグ("0"：非表示)
                    query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                    '表示アイコンフラグ("1"：表示)
                    query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)
                    '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                    '表示アイコンフラグ2("2"：表示)
                    query.AddParameterWithTypeValue("ICON_FLAG_ON_2", OracleDbType.NVarchar2, IconFlagOn2)
                    '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                    '担当SAアカウント
                    query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, inAccount)
                    '来店日時
                    query.AddParameterWithTypeValue("VISITTIMESTAMP", OracleDbType.Date, inPresentTime)
                    '振当てステータス(2：SA振当済)
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_FIN", OracleDbType.NVarchar2, AssignFinish)
                    'サービスステータス
                    query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
                    'キャンセルフラグ
                    query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.NVarchar2, CancelFlagEffective)
                    'ROステータス（"99"：キャンセル）
                    query.AddParameterWithTypeValue("STATUS_99", OracleDbType.NVarchar2, StatusROCancel)

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

        ''' <summary>
        ''' 作業中・納車準備・納車作業エリアチップ情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inAccount">ログインSAアカウント</param>
        ''' <returns>作業中・納車準備・納車作業エリアチップ情報取得データセット</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発
        ''' 2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ''' 2019/05/20 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション
        '''                      [TKM]PUAT-4153 SAメインの追加JOBのアイコン追加回数が減る を修正
        ''' </history>
        Public Function GetMainChipInfo(ByVal inDealerCode As String _
                                      , ByVal inBranchCode As String _
                                      , ByVal inAccount As String) As SC3140103DataSet.SC3140103MainChipInfoDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN: ACCOUNT = {3} " _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , inAccount))

            Dim dt As SC3140103DataSet.SC3140103MainChipInfoDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103MainChipInfoDataTable)("SC3140103_028")
                    Dim sql As New StringBuilder

                    'SQL文作成
                    With sql

                        .AppendLine("  SELECT   /* SC3140103_028 */ ")
                        .AppendLine("           T1.VISIT_ID AS VISIT_ID")
                        .AppendLine("          ,TRIM(T1.RO_NUM) AS RO_NUM ")
                        .AppendLine("          ,MAX(NVL(T10.RO_SEQ, -1)) AS RO_SEQ ")
                        .AppendLine("          ,SUBSTR(TO_CHAR(MAX(T10.RO_SEQ)), -1, 1) AS MAX_RO_SEQ ") '1の位のみ取得する
                        .AppendLine("          ,MIN(NVL(TRIM(T10.RO_STATUS), TRIM(T1.RO_STATUS))) AS MIN_RO_STATUS ")
                        .AppendLine("          ,MAX(TRIM(T1.RO_STATUS)) AS RO_STATUS ")
                        .AppendLine("          ,MAX(T2.VISITTIMESTAMP) AS VISITTIMESTAMP ")
                        '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                        '.AppendLine("          ,MAX(TRIM(T2.VCLREGNO)) AS VCLREGNO ")
                        .AppendLine("          ,MAX(NVL(TRIM(T4.REG_NUM), TRIM(T2.VCLREGNO))) AS VCLREGNO ")
                        '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END


                        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                        '.AppendLine("          ,MAX(TRIM(T2.NAME)) AS NAME ")
                        .AppendLine("        ,MAX(NVL(TRIM(T2.VISITNAME), TRIM(T2.NAME))) AS NAME ")
                        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                        .AppendLine("          ,MAX(T2.ASSIGNTIMESTAMP) AS ASSIGNTIMESTAMP ")
                        .AppendLine("          ,MAX(T3.SVCIN_ID) AS SVCIN_ID ")
                        .AppendLine("          ,MAX(CASE  ")
                        .AppendLine("                    WHEN T3.ACCEPTANCE_TYPE = :RESERVE THEN :ICON_FLAG_ON ")
                        .AppendLine("                    ELSE :ICON_FLAG_OFF  ")
                        .AppendLine("                END) AS REZ_MARK ")
                        .AppendLine("          ,MAX(NVL(TRIM(T3.CARWASH_NEED_FLG), :NONWASH)) AS CARWASH_NEED_FLG ")
                        .AppendLine("          ,MAX(TRIM(T3.SVC_STATUS)) AS SVC_STATUS ")
                        .AppendLine("          ,MAX(CASE  ")
                        .AppendLine("                    WHEN T3.SCHE_DELI_DATETIME = :MINDATE THEN NULL ")
                        .AppendLine("                    ELSE T3.SCHE_DELI_DATETIME  ")
                        .AppendLine("                END) AS SCHE_DELI_DATETIME ")
                        .AppendLine("          ,MAX(CASE  ")
                        .AppendLine("                    WHEN T3.INVOICE_PREP_COMPL_DATETIME = :MINDATE THEN :MINVALUE ")
                        .AppendLine("                    ELSE T3.INVOICE_PREP_COMPL_DATETIME  ")
                        .AppendLine("                END) AS INVOICE_PRINT_DATETIME ")
                        .AppendLine("          ,MAX(CASE  ")

                        '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 START

                        '.AppendLine("                    WHEN T4.VIP_FLG = :ICON_FLAG_ON THEN T4.VIP_FLG ")

                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                        '.AppendLine("                    WHEN T4.IMP_VCL_FLG = :ICON_FLAG_ON THEN T4.IMP_VCL_FLG ")
                        .AppendLine("                    WHEN T4.IMP_VCL_FLG = :ICON_FLAG_ON THEN T4.IMP_VCL_FLG ")
                        .AppendLine("                    WHEN T4.IMP_VCL_FLG = :ICON_FLAG_ON_2 THEN T4.IMP_VCL_FLG ")
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

                        '2014/05/08 TMEJ 小澤 IT9669_サービスタブレットDMS連携作業追加機能開発 END

                        .AppendLine("                    ELSE :ICON_FLAG_OFF  ")
                        .AppendLine("                END) AS JDP_MARK ")
                        .AppendLine("          ,MAX(CASE  ")
                        .AppendLine("                    WHEN T5.RSLT_START_DATETIME = :MINDATE THEN :MINVALUE ")
                        .AppendLine("                    WHEN T5.RSLT_START_DATETIME IS NULL THEN :MINVALUE ")
                        .AppendLine("                    ELSE T5.RSLT_START_DATETIME  ")
                        .AppendLine("                END) AS WASH_RSLT_START_DATETIME ")
                        .AppendLine("          ,MAX(CASE  ")
                        .AppendLine("                    WHEN T5.RSLT_END_DATETIME = :MINDATE THEN :MINVALUE ")
                        .AppendLine("                    WHEN T5.RSLT_END_DATETIME IS NULL THEN :MINVALUE ")
                        .AppendLine("                    ELSE T5.RSLT_END_DATETIME  ")
                        .AppendLine("                END) AS WASH_RSLT_END_DATETIME ")
                        .AppendLine("          ,MAX(CASE  ")
                        .AppendLine("                    WHEN T7.MAX_INSPECTION_DATETIME = :MINDATE THEN :MINVALUE ")
                        .AppendLine("                    ELSE T7.MAX_INSPECTION_DATETIME  ")
                        .AppendLine("                END) AS MAX_INSPECTION_DATE ")
                        .AppendLine("          ,MIN(CASE  ")
                        .AppendLine("                    WHEN T7.MIN_INSPECTION_DATETIME = :MINDATE THEN :MINVALUE ")
                        .AppendLine("                    ELSE T7.MIN_INSPECTION_DATETIME  ")
                        .AppendLine("                END) AS MIN_INSPECTION_DATE ")
                        .AppendLine("          ,MAX(CASE  ")
                        .AppendLine("                    WHEN T7.SCHE_START_DATETIME = :MINDATE THEN :MINVALUE ")
                        .AppendLine("                    ELSE T7.SCHE_START_DATETIME ")
                        .AppendLine("                END) AS MAX_SCHE_START_DATETIME ")
                        .AppendLine("          ,MAX(CASE  ")
                        .AppendLine("                    WHEN T7.SCHE_END_DATETIME = :MINDATE THEN :MINVALUE ")
                        .AppendLine("                    ELSE T7.SCHE_END_DATETIME  ")
                        .AppendLine("                END) AS MAX_SCHE_END_DATETIME ")
                        .AppendLine("          ,MAX(NVL(CONCAT(TRIM(T8.UPPER_DISP), TRIM(T8.LOWER_DISP)),NVL(T9.SVC_CLASS_NAME,T9.SVC_CLASS_NAME_ENG))) AS MERCHANDISENAME ")
                        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                        .AppendLine("          ,MAX(T7.REMAINING_INSPECTION_TYPE) AS REMAINING_INSPECTION_TYPE")
                        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                        .AppendLine("          ,MAX(CASE  ")
                        .AppendLine("                    WHEN T11.SPECIAL_CAMPAIGN_TGT_FLG = :ICON_FLAG_ON THEN T11.SPECIAL_CAMPAIGN_TGT_FLG ")
                        .AppendLine("                    ELSE :ICON_FLAG_OFF  ")
                        .AppendLine("                END) AS SSC_MARK ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                        .AppendLine("        ,MAX(NVL(TRIM(T12.SML_AMC_FLG), :ICON_FLAG_OFF)) AS SML_AMC_FLG ")
                        .AppendLine("        ,MAX(NVL(TRIM(T12.EW_FLG), :ICON_FLAG_OFF)) AS EW_FLG ")
                        .AppendLine("        ,MAX(CASE ")
                        .AppendLine("               WHEN T13.VCL_VIN IS NOT NULL THEN :ICON_FLAG_ON")
                        .AppendLine("               ELSE :ICON_FLAG_OFF")
                        .AppendLine("         END) AS TLM_MBR_FLG")
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                        .AppendLine("      FROM TB_T_RO_INFO T1 ")
                        .AppendLine("          ,TBL_SERVICE_VISIT_MANAGEMENT T2 ")
                        .AppendLine("          ,TB_T_SERVICEIN T3 ")
                        .AppendLine("          ,TB_M_VEHICLE_DLR T4 ")
                        .AppendLine("          ,TB_T_CARWASH_RESULT T5 ")
                        .AppendLine("          ,TB_T_JOB_DTL T6 ")
                        .AppendLine("          ,(  SELECT R3.SVCIN_ID ")
                        .AppendLine("                    ,MIN(R4.JOB_DTL_ID) AS JOB_DTL_ID  ")
                        .AppendLine("                    ,MAX(R4.INSPECTION_APPROVAL_DATETIME) AS MAX_INSPECTION_DATETIME ")
                        .AppendLine("                    ,MIN(R4.INSPECTION_APPROVAL_DATETIME) AS MIN_INSPECTION_DATETIME ")
                        .AppendLine("                    ,MAX(R5.SCHE_START_DATETIME) AS SCHE_START_DATETIME ")
                        .AppendLine("                    ,MAX(CASE  ")
                        .AppendLine("                              WHEN R5.RSLT_END_DATETIME <> :MINDATE THEN R5.RSLT_END_DATETIME ")
                        .AppendLine("                              WHEN R5.PRMS_END_DATETIME <> :MINDATE THEN PRMS_END_DATETIME ")
                        .AppendLine("                              ELSE R5.SCHE_END_DATETIME ")
                        .AppendLine("                          END) AS SCHE_END_DATETIME ")
                        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                        .AppendLine("                    ,MIN(DECODE(R4.INSPECTION_NEED_FLG, 1, R4.INSPECTION_STATUS, 2)) AS REMAINING_INSPECTION_TYPE ")
                        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                        .AppendLine("                FROM TB_T_RO_INFO R1 ")
                        .AppendLine("                    ,TBL_SERVICE_VISIT_MANAGEMENT R2 ")
                        .AppendLine("                    ,TB_T_SERVICEIN R3 ")
                        .AppendLine("                    ,TB_T_JOB_DTL R4 ")
                        .AppendLine("                    ,TB_T_STALL_USE R5 ")
                        .AppendLine("               WHERE R1.VISIT_ID = R2.VISITSEQ ")
                        .AppendLine("                 AND R2.FREZID = R3.SVCIN_ID  ")
                        .AppendLine("                 AND R3.SVCIN_ID = R4.SVCIN_ID ")
                        .AppendLine("                 AND R4.JOB_DTL_ID = R5.JOB_DTL_ID ")
                        .AppendLine("                 AND R1.RO_STATUS IN (:STATUS_50,:STATUS_60,:STATUS_80,:STATUS_85) ")
                        .AppendLine("                 AND R1.DLR_CD = :DLRCD ")
                        .AppendLine("                 AND R1.BRN_CD = :STRCD ")
                        '.AppendLine("                 AND R1.RO_CREATE_STF = :RO_CREATE_STF ")
                        .AppendLine("                 AND R2.DLRCD = :DLRCD ")
                        .AppendLine("                 AND R2.STRCD = :STRCD ")
                        .AppendLine("                 AND R2.SACODE = :SACODE ")
                        .AppendLine("                 AND R3.DLR_CD = :DLRCD ")
                        .AppendLine("                 AND R3.BRN_CD = :STRCD ")
                        .AppendLine("                 AND R3.SVC_STATUS <> :STATUS_CANCEL ")
                        .AppendLine("                 AND R4.DLR_CD = :DLRCD ")
                        .AppendLine("                 AND R4.BRN_CD = :STRCD ")
                        .AppendLine("                 AND R4.CANCEL_FLG = :CANCELFLG ")
                        .AppendLine("                 AND R5.DLR_CD = :DLRCD ")
                        .AppendLine("                 AND R5.BRN_CD = :STRCD ")
                        .AppendLine("            GROUP BY R3.SVCIN_ID ")
                        .AppendLine("           ) T7 ")
                        .AppendLine("          ,TB_M_MERCHANDISE T8 ")
                        .AppendLine("          ,TB_M_SERVICE_CLASS T9 ")
                        .AppendLine("          ,TB_T_RO_INFO T10 ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                        .AppendLine("          ,TB_M_VEHICLE T11 ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                        .AppendLine("        ,TB_LM_VEHICLE T12")
                        .AppendLine("        ,TB_LM_TLM_MEMBER T13")
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                        .AppendLine("     WHERE T1.VISIT_ID = T2.VISITSEQ ")
                        .AppendLine("       AND T2.FREZID = T3.SVCIN_ID  ")
                        .AppendLine("       AND T2.VCL_ID = T4.VCL_ID (+) ")
                        .AppendLine("       AND T3.SVCIN_ID = T5.SVCIN_ID (+) ")
                        .AppendLine("       AND T3.SVCIN_ID = T6.SVCIN_ID  ")
                        .AppendLine("       AND T6.JOB_DTL_ID = T7.JOB_DTL_ID ")
                        .AppendLine("       AND T6.MERC_ID = T8.MERC_ID (+) ")
                        .AppendLine("       AND T6.SVC_CLASS_ID = T9.SVC_CLASS_ID (+) ")
                        .AppendLine("       AND T2.VISITSEQ = T10.VISIT_ID ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
                        .AppendLine("       AND T2.VCL_ID = T11.VCL_ID (+) ")
                        '2018/02/22 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                        .AppendLine("       AND T11.VCL_ID = T12.VCL_ID(+) ")
                        .AppendLine("       AND T11.VCL_VIN = T13.VCL_VIN(+) ")
                        '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                        .AppendLine("       AND T1.RO_STATUS IN (:STATUS_50,:STATUS_60,:STATUS_80,:STATUS_85) ")
                        .AppendLine("       AND T1.DLR_CD = :DLRCD ")
                        .AppendLine("       AND T1.BRN_CD = :STRCD ")
                        '.AppendLine("       AND T1.RO_CREATE_STF = :RO_CREATE_STF ")
                        .AppendLine("       AND T2.DLRCD = :DLRCD ")
                        .AppendLine("       AND T2.STRCD = :STRCD ")
                        .AppendLine("       AND T2.SACODE = :SACODE ")
                        .AppendLine("       AND T3.DLR_CD = :DLRCD ")
                        .AppendLine("       AND T3.BRN_CD = :STRCD ")
                        .AppendLine("       AND T4.DLR_CD (+) = :DLRCD ")
                        .AppendLine("  	    AND T6.DLR_CD = :DLRCD ")
                        .AppendLine("       AND T6.BRN_CD = :STRCD ")
                        .AppendLine("  	    AND T10.DLR_CD = :DLRCD ")
                        .AppendLine("       AND T10.BRN_CD = :STRCD ")

                        ' 2019/05/20 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション
                        '                      [TKM]PUAT-4153 SAメインの追加JOBのアイコン追加回数が減る を修正 START
                        '.AppendLine("       AND T10.RO_STATUS <> :STATUS_99 ")
                        ' 2019/05/20 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション
                        '                      [TKM]PUAT-4153 SAメインの追加JOBのアイコン追加回数が減る を修正 END

                        .AppendLine("  GROUP BY T1.VISIT_ID ")
                        .AppendLine("          ,T1.RO_NUM ")

                    End With

                    'SQL設定
                    query.CommandText = sql.ToString()

                    'バインド変数

                    '日付省略値
                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
                    '日付最小値
                    query.AddParameterWithTypeValue("MINVALUE", OracleDbType.Date, Date.MinValue)
                    '予約区分("0"：予約客)
                    query.AddParameterWithTypeValue("RESERVE", OracleDbType.NVarchar2, AcceptanceTypeRez)
                    '表示アイコンフラグ("0"：非表示)
                    query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                    '表示アイコンフラグ("1"：表示)
                    query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)
                    '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                    '表示アイコンフラグ2("2"：表示)
                    query.AddParameterWithTypeValue("ICON_FLAG_ON_2", OracleDbType.NVarchar2, IconFlagOn2)
                    '2018/06/20 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                    '洗車必要フラグ("0"：洗車不要)
                    query.AddParameterWithTypeValue("NONWASH", OracleDbType.NVarchar2, NonWash)

                    'ROステータス("50"：着工指示待ち)
                    query.AddParameterWithTypeValue("STATUS_50", OracleDbType.NVarchar2, StatusInstructionsWait)
                    'ROステータス("60"：作業中)
                    query.AddParameterWithTypeValue("STATUS_60", OracleDbType.NVarchar2, StatusWork)
                    'ROステータス("80"：納車準備待ち)
                    query.AddParameterWithTypeValue("STATUS_80", OracleDbType.NVarchar2, StatusDeliveryWait)
                    'ROステータス("85"：納車作業中)
                    query.AddParameterWithTypeValue("STATUS_85", OracleDbType.NVarchar2, StatusDeliveryWork)

                    ' 2019/05/20 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション
                    '                      [TKM]PUAT-4153 SAメインの追加JOBのアイコン追加回数が減る を修正 START
                    ''ROステータス（"99"：キャンセル）
                    'query.AddParameterWithTypeValue("STATUS_99", OracleDbType.NVarchar2, StatusROCancel)
                    ' 2019/05/20 NSK 鈴木 18PRJ03142_(FS)業務検証:セールス受注前活動活動結果登録オペレーション
                    '                      [TKM]PUAT-4153 SAメインの追加JOBのアイコン追加回数が減る を修正 END

                    'サービスステータス
                    query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
                    'キャンセルフラグ
                    query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.NVarchar2, CancelFlagEffective)
                    '販売店コード
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                    '店舗コード
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)
                    '担当SAアカウント
                    query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, inAccount)


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

        ''' <summary>
        ''' 追加作業エリアチップ情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inDTMainChipInfo">作業中・納車準備・納車作業エリアチップ情報</param>
        ''' <returns>追加作業エリアチップ情報取得データセット</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
        '''                      [TKM]PUAT-4100 SAメインのチップ詳細に追加作業承認ボタンが表示されない を修正
        ''' </history>
        Public Function GetAddApprovalChipInfo(ByVal inDealerCode As String _
                                             , ByVal inBranchCode As String _
                                             , ByVal inDTMainChipInfo As SC3140103DataSet.SC3140103MainChipInfoDataTable) _
                                               As SC3140103DataSet.SC3140103AddApprovalChipInfoDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN: COUNT = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , inDTMainChipInfo.Count))

            Dim dt As SC3140103DataSet.SC3140103AddApprovalChipInfoDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AddApprovalChipInfoDataTable)("SC3140103_029")
                    Dim sql As New StringBuilder

                    '来店実績連番　条件用文字列
                    Dim sqlVisitSeq As New StringBuilder

                    '来店実績連番　条件IN
                    sqlVisitSeq.AppendLine(" AND T1.VISIT_ID IN ( ")

                    '来店実績連番の数
                    Dim count As Long = 1

                    'パラメータ用文字列
                    Dim visitSeqPramName As String

                    '工程エリアチップ分ループ
                    For Each row As SC3140103DataSet.SC3140103MainChipInfoRow In inDTMainChipInfo

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
                        query.AddParameterWithTypeValue(visitSeqPramName, OracleDbType.Int64, row.VISIT_ID)

                        count += 1
                    Next

                    sqlVisitSeq.AppendLine(" ) ")


                    'SQL文作成
                    With sql

                        .AppendLine("    SELECT /* SC3140103_029 */ ")
                        .AppendLine("           T1.VISIT_ID AS VISIT_ID ")
                        .AppendLine("          ,T1.RO_SEQ ")
                        .AppendLine("          ,MAX(CASE  ")
                        .AppendLine("                    WHEN T1.RO_CHECK_DATETIME = :MINDATE THEN :MINVALUE ")
                        .AppendLine("                    ELSE T1.RO_CHECK_DATETIME ")
                        .AppendLine("               END) AS RO_CHECK_DATETIME ")
                        .AppendLine("          ,MAX(T2.USERNAME) AS ISSUANCE_TC_NAME ")
                        .AppendLine("     FROM TB_T_RO_INFO T1 ")
                        .AppendLine("         ,TBL_USERS T2 ")
                        .AppendLine("    WHERE T1.RO_CREATE_STF_CD = T2.ACCOUNT(+) ")
                        .AppendLine(sqlVisitSeq.ToString)
                        .AppendLine("      AND T1.DLR_CD = :DLR_CD ")
                        .AppendLine("      AND T1.BRN_CD = :BRN_CD ")
                        '.AppendLine("      AND T1.RO_STATUS IN (:STATUS_10, :STATUS_35) ")
                        .AppendLine("      AND T1.RO_STATUS = :STATUS_35 ")
                        .AppendLine("      AND T2.DLRCD(+) = :DLRCD ")
                        .AppendLine("      AND T2.STRCD(+) = :STRCD ")
                        .AppendLine("      AND T2.OPERATIONCODE(+) = :OPERATIONCODE_TC ")
                        .AppendLine(" GROUP BY T1.VISIT_ID ")
                        .AppendLine("         ,T1.RO_SEQ ")
                        ' 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
                        '                      [TKM]PUAT-4100 SAメインのチップ詳細に追加作業承認ボタンが表示されない を修正 START
                        .AppendLine(" ORDER BY T1.VISIT_ID ")
                        .AppendLine("         ,T1.RO_SEQ ")
                        ' 2019/05/10 NSK 鈴木 18PRJ00XXX_(FS)納車時オペレーションCS向上にむけた評価（サービス）
                        '                      [TKM]PUAT-4100 SAメインのチップ詳細に追加作業承認ボタンが表示されない を修正 END

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

                    ''ROステータス("10"：SA起票中)
                    'query.AddParameterWithTypeValue("STATUS_10", OracleDbType.NVarchar2, StatusSAIssuance)
                    'ROステータス("35"：SA承認待ち)
                    query.AddParameterWithTypeValue("STATUS_35", OracleDbType.NVarchar2, StatusConfirmationWait)

                    '販売店コード(CHAR)
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                    '店舗コード(CHAR)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inBranchCode)
                    '権限コード("14"：TC)
                    query.AddParameterWithTypeValue("OPERATIONCODE_TC", OracleDbType.Int64, Operation.TEC)

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

        ''' <summary>
        ''' 最終作業テクニシャン名情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inDTMainChipInfo">作業中・納車準備・納車作業エリアチップ情報</param>
        ''' <returns>最終作業テクニシャン名情報データセット</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetLastTechnicianInfo(ByVal inDealerCode As String _
                                            , ByVal inBranchCode As String _
                                            , ByVal inDTMainChipInfo As SC3140103DataSet.SC3140103MainChipInfoDataTable) _
                                              As SC3140103DataSet.SC3140103LastTechnicianInfoDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN: DTMAINCHIPINFO = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , inDTMainChipInfo.Count))

            Dim dt As SC3140103DataSet.SC3140103LastTechnicianInfoDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103LastTechnicianInfoDataTable)("SC3140103_030")
                    Dim sql As New StringBuilder

                    'サービス入庫ID　条件用文字列
                    Dim sqlServiceInId As New StringBuilder

                    'サービス入庫IDの数
                    Dim count As Long = 1

                    'パラメータ用文字列
                    Dim serviceInIdPramName As String

                    '工程エリア(納車準備・納車)チップ分ループ
                    For Each row As SC3140103DataSet.SC3140103MainChipInfoRow _
                        In inDTMainChipInfo.Select(String.Format(CultureInfo.CurrentCulture, "DISP_AREA IN ({0}, {1})", DisplayDivPreparation, DisplayDivDelivery))

                        ' SQL作成
                        serviceInIdPramName = String.Format(CultureInfo.CurrentCulture, "SVCIN_ID{0}", count)

                        '1行目か判定
                        If 1 < count Then
                            '2行目以降

                            'カンマ設定
                            sqlServiceInId.AppendLine(String.Format(CultureInfo.CurrentCulture, ", :{0} ", serviceInIdPramName))
                        Else
                            '1行目

                            'サービス入庫ID　条件IN
                            sqlServiceInId.AppendLine(" AND T1.SVCIN_ID IN ( ")

                            'カンマ無し
                            sqlServiceInId.AppendLine(String.Format(CultureInfo.CurrentCulture, "  :{0} ", serviceInIdPramName))
                        End If

                        ' パラメータ作成
                        query.AddParameterWithTypeValue(serviceInIdPramName, OracleDbType.Decimal, row.SVCIN_ID)

                        count += 1
                    Next


                    If 1 < count Then

                        sqlServiceInId.AppendLine(" ) ")

                    Else

                        Return New SC3140103DataSet.SC3140103LastTechnicianInfoDataTable

                    End If


                    'SQL文作成
                    With sql

                        .AppendLine("  SELECT /* SC3140103_030 */ ")
                        .AppendLine("         T6.SVCIN_ID ")
                        .AppendLine("        ,T6.LAST_TC_NAME ")
                        .AppendLine("    FROM      ")
                        .AppendLine("         (SELECT  ")
                        .AppendLine("                T1.SVCIN_ID ")
                        .AppendLine("               ,T2.JOB_DTL_ID ")
                        .AppendLine("               ,T3.STALL_USE_ID ")
                        .AppendLine("               ,TRIM(T5.USERNAME) AS LAST_TC_NAME ")
                        .AppendLine("           FROM TB_T_SERVICEIN T1  ")
                        .AppendLine("               ,TB_T_JOB_DTL T2  ")
                        .AppendLine("               ,TB_T_STALL_USE T3  ")
                        .AppendLine("               ,TB_T_STAFF_JOB T4 ")
                        .AppendLine("               ,TBL_USERS T5 ")
                        .AppendLine("          WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                        .AppendLine("            AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                        .AppendLine("            AND T3.JOB_ID = T4.JOB_ID (+) ")
                        .AppendLine("            AND T4.STF_CD = T5.ACCOUNT (+) ")
                        .AppendLine(sqlServiceInId.ToString)
                        .AppendLine("            AND T1.DLR_CD = :DLRCD ")
                        .AppendLine("            AND T1.BRN_CD = :STRCD ")
                        .AppendLine("            AND T2.DLR_CD = :DLRCD ")
                        .AppendLine("            AND T2.BRN_CD = :STRCD ")
                        .AppendLine("            AND T2.CANCEL_FLG = :CANCELFLG ")
                        .AppendLine("            AND T3.DLR_CD = :DLRCD ")
                        .AppendLine("            AND T3.BRN_CD = :STRCD ")
                        .AppendLine("            AND T5.DLRCD (+) = :CHAR_DLRCD ")
                        .AppendLine("            AND T5.STRCD (+) = :CHAR_STRCD ")
                        .AppendLine("            AND T5.OPERATIONCODE(+) = :OPERATIONCODE_TC ")
                        .AppendLine("            AND T5.DELFLG (+) = :DELFLG ")
                        .AppendLine("       ORDER BY T3.RSLT_END_DATETIME DESC ")
                        .AppendLine("               ,T5.USERNAME ASC ")
                        .AppendLine("         ) T6 ")
                        .AppendLine("   WHERE ROWNUM =1 ")

                    End With

                    'SQL設定
                    query.CommandText = sql.ToString()

                    'バインド変数

                    '販売店コード
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                    '店舗コード
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)
                    'キャンセルフラグ
                    query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.NVarchar2, CancelFlagEffective)
                    '販売店コード(CHAR) 
                    query.AddParameterWithTypeValue("CHAR_DLRCD", OracleDbType.Char, inDealerCode)
                    '店舗コード(CHAR)
                    query.AddParameterWithTypeValue("CHAR_STRCD", OracleDbType.Char, inBranchCode)
                    '権限ID("14"：TC)
                    query.AddParameterWithTypeValue("OPERATIONCODE_TC", OracleDbType.Int64, Operation.TEC)
                    '削除フラグ("0"：有効)
                    query.AddParameterWithTypeValue("DELFLG", OracleDbType.NVarchar2, DeleteFlag)

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

        ''' <summary>
        ''' 残作業時間情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inDTMainChipInfo">作業中・納車準備・納車作業エリアチップ情報</param>
        ''' <returns> 残作業時間情報取得データセット</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetRemainingTimeInfo(ByVal inDealerCode As String _
                                           , ByVal inBranchCode As String _
                                           , ByVal inDTMainChipInfo As SC3140103DataSet.SC3140103MainChipInfoDataTable) _
                                             As SC3140103DataSet.SC3140103RemainingTimeInfoDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN: DTMAINCHIPINFO = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , inDTMainChipInfo.Count))

            Dim dt As SC3140103DataSet.SC3140103RemainingTimeInfoDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103RemainingTimeInfoDataTable)("SC3140103_031")
                    Dim sql As New StringBuilder

                    'サービス入庫ID　条件用文字列
                    Dim sqlServiceInId As New StringBuilder

                    'サービス入庫ID　条件IN
                    sqlServiceInId.AppendLine(" AND T1.SVCIN_ID IN ( ")

                    'サービス入庫IDの数
                    Dim count As Long = 1

                    'パラメータ用文字列
                    Dim serviceInIdPramName As String

                    'チップ分ループ
                    For Each row In inDTMainChipInfo

                        ' SQL作成
                        serviceInIdPramName = String.Format(CultureInfo.CurrentCulture, "SVCIN_ID{0}", count)

                        '1行目か判定
                        If 1 < count Then
                            '2行目以降

                            'カンマ設定
                            sqlServiceInId.AppendLine(String.Format(CultureInfo.CurrentCulture, ", :{0} ", serviceInIdPramName))
                        Else
                            '1行目

                            'カンマ無し
                            sqlServiceInId.AppendLine(String.Format(CultureInfo.CurrentCulture, "  :{0} ", serviceInIdPramName))
                        End If

                        ' パラメータ作成
                        query.AddParameterWithTypeValue(serviceInIdPramName, OracleDbType.Decimal, row.SVCIN_ID)

                        count += 1
                    Next

                    sqlServiceInId.AppendLine(" ) ")


                    'SQL文作成
                    With sql

                        .AppendLine("  SELECT /* SC3140103_031 */ ")
                        .AppendLine("         T4.SVCIN_ID ")
                        .AppendLine("        ,T4.REMAININGTIME ")
                        .AppendLine("    FROM      ")
                        .AppendLine("         (SELECT  ")
                        .AppendLine("                T1.SVCIN_ID ")
                        .AppendLine("               ,SUM(T3.SCHE_WORKTIME) AS REMAININGTIME ")
                        .AppendLine("           FROM TB_T_SERVICEIN T1  ")
                        .AppendLine("               ,TB_T_JOB_DTL T2  ")
                        .AppendLine("               ,TB_T_STALL_USE T3  ")
                        .AppendLine("          WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                        .AppendLine("            AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                        .AppendLine(sqlServiceInId.ToString)
                        .AppendLine("            AND T1.DLR_CD = :DLRCD ")
                        .AppendLine("            AND T1.BRN_CD = :STRCD ")
                        .AppendLine("            AND T2.DLR_CD = :DLRCD ")
                        .AppendLine("            AND T2.BRN_CD = :STRCD ")
                        .AppendLine("            AND T2.CANCEL_FLG = :CANCELFLG ")
                        .AppendLine("            AND T3.DLR_CD = :DLRCD ")
                        .AppendLine("            AND T3.BRN_CD = :STRCD ")
                        .AppendLine("            AND T3.STALL_USE_STATUS IN (:USE_STATUS_00, :USE_STATUS_01) ")
                        .AppendLine("       GROUP BY T1.SVCIN_ID ")
                        .AppendLine("         ) T4 ")
                        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START

                        'レコード数の制限から、最初のチップの残作業時間しか取得できないため削除
                        '.AppendLine("   WHERE ROWNUM =1 ")

                        '2017/09/06 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                    End With

                    'SQL設定
                    query.CommandText = sql.ToString()

                    'バインド変数

                    '販売店コード
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                    '店舗コード
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)
                    'キャンセルフラグ
                    query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.NVarchar2, CancelFlagEffective)
                    'ストール利用ステータス("00"：着工指示待ち)
                    query.AddParameterWithTypeValue("USE_STATUS_00", OracleDbType.NVarchar2, StallStatusInstruct)
                    'ストール利用ステータス("01"：作業開始待ち)
                    query.AddParameterWithTypeValue("USE_STATUS_01", OracleDbType.NVarchar2, StallStatusWaitWork)

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

#End Region

#Region "事前準備チップ情報取得"

        ''' <summary>
        ''' 事前準備チップ情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="nowDay">本日年月日（YYYYMMDD）</param>
        ''' <param name="nextBusinessDay">翌営業年月日（YYYYMMDD）</param>
        ''' <param name="saCode">SAコード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </History>
        Public Function GetAdvancePreparationsChipData(ByVal dealerCode As String, _
                                                       ByVal branchCode As String, _
                                                       ByVal nowDay As String, _
                                                       ByVal nextBusinessDay As String, _
                                                       ByVal saCode As String) _
                                                       As SC3140103DataSet.SC3140103AdvancePreparationsDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, nowDay = {5}, nextBusinessDay = {6}, saCode = {7}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , dealerCode _
                      , branchCode _
                      , nowDay _
                      , nextBusinessDay _
                      , saCode))

            Dim dt As SC3140103DataSet.SC3140103AdvancePreparationsDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AdvancePreparationsDataTable)("SC3140103_011")

                    Dim sql As New StringBuilder

                    With sql

                        .AppendLine(" SELECT  /* SC3140103_011 */ ")
                        .AppendLine("         TRIM(S1.DLR_CD) AS DLRCD ")
                        .AppendLine("        ,TRIM(S1.BRN_CD) AS STRCD ")
                        .AppendLine("        ,S1.SVCIN_ID AS REZID ")
                        .AppendLine("        ,S1.RO_NUM AS ORDERNO ")
                        .AppendLine("        ,NVL(S1.SCHE_SVCIN_DATETIME, S1.SCHE_START_DATETIME) AS REZ_PICK_DATE ")
                        .AppendLine("        ,NVL(S1.SCHE_DELI_DATETIME, S1.SCHE_END_DATETIME) AS REZ_DELI_DATE ")
                        .AppendLine("        ,S1.CST_NAME AS CUSTOMERNAME ")
                        .AppendLine("        ,NVL(S1.VCL_VIN, TRIM(T10.VCL_VIN)) AS VIN ")
                        .AppendLine("        ,S1.REG_NUM AS VCLREGNO ")
                        .AppendLine("        ,NVL(TRIM(T8.MERC_NAME), TRIM(T8.MERC_NAME_ENG)) AS MERCHANDISENAME ")
                        .AppendLine("        ,NVL(T9.VISITSEQ, -1) AS VISITSEQ ")
                        .AppendLine("        ,T9.ASSIGNSTATUS AS ASSIGNSTATUS ")
                        .AppendLine("        ,DECODE(SUBSTR(S1.SCHE_SVCIN_DATETIME, 1, 8), :TODAY, 1, 0) AS TODAYFLG ")
                        .AppendLine("   FROM          ")
                        .AppendLine("         (SELECT ")
                        .AppendLine("                  MAX(T1.DLR_CD) AS DLR_CD ")
                        .AppendLine("                 ,MAX(T1.BRN_CD) AS BRN_CD ")
                        .AppendLine("                 ,T1.SVCIN_ID ")
                        .AppendLine("                 ,MAX(TRIM(T1.RO_NUM)) AS RO_NUM ")
                        .AppendLine("                 ,MAX(T1.CST_ID) AS CST_ID ")
                        .AppendLine("                 ,MAX(T1.VCL_ID) AS VCL_ID ")
                        .AppendLine("                 ,MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE , TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MI'), TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_SVCIN_DATETIME ")
                        .AppendLine("                 ,MAX(DECODE(T1.SCHE_DELI_DATETIME, :MINDATE , TO_CHAR(T3.SCHE_END_DATETIME, 'YYYYMMDDHH24MI'), TO_CHAR(T1.SCHE_DELI_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_DELI_DATETIME ")
                        .AppendLine("                 ,MIN(T2.JOB_DTL_ID) AS JOB_DTL_ID ")
                        .AppendLine("                 ,MIN(DECODE(T3.SCHE_START_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_START_DATETIME ")
                        .AppendLine("                 ,MIN(DECODE(T3.SCHE_END_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_END_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_END_DATETIME ")
                        .AppendLine("                 ,MAX(TRIM(T4.CST_NAME)) AS CST_NAME ")
                        .AppendLine("                 ,MAX(TRIM(T5.VCL_VIN)) AS VCL_VIN ")
                        .AppendLine("                 ,MAX(TRIM(T5.VCL_KATASHIKI)) AS VCL_KATASHIKI ")
                        .AppendLine("                 ,MAX(TRIM(T6.REG_NUM)) AS REG_NUM ")
                        .AppendLine("            FROM  TB_T_SERVICEIN T1 ")
                        .AppendLine("                 ,TB_T_JOB_DTL T2 ")
                        .AppendLine("                 ,TB_T_STALL_USE T3 ")
                        .AppendLine("                 ,TB_M_CUSTOMER T4 ")
                        .AppendLine("                 ,TB_M_VEHICLE T5 ")
                        .AppendLine("                 ,TB_M_VEHICLE_DLR T6 ")
                        .AppendLine("           WHERE  T1.SVCIN_ID = T2.SVCIN_ID ")
                        .AppendLine("             AND  T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                        .AppendLine("             AND  T1.CST_ID = T4.CST_ID (+) ")
                        .AppendLine("             AND  T1.VCL_ID = T5.VCL_ID (+) ")
                        .AppendLine("             AND  T1.DLR_CD = T6.DLR_CD (+) ")
                        .AppendLine("             AND  T1.VCL_ID = T6.VCL_ID (+) ")
                        .AppendLine("             AND  T1.DLR_CD = :DLRCD ")
                        .AppendLine("             AND  T1.BRN_CD = :STRCD ")
                        .AppendLine("             AND  T1.RESV_STATUS = :RESV_STATUS ")
                        .AppendLine("             AND  T1.PIC_SA_STF_CD = :SACODE ")
                        .AppendLine("             AND  NOT EXISTS (SELECT 1 ")
                        .AppendLine("                                FROM TB_T_SERVICEIN D1 ")
                        .AppendLine("                               WHERE D1.SVCIN_ID = T1.SVCIN_ID ")
                        .AppendLine("                                 AND D1.SVC_STATUS = :STATUS_CANCEL) ")
                        .AppendLine("             AND  T2.CANCEL_FLG = :CANCEL_FLG ")
                        .AppendLine("             AND  T3.SCHE_START_DATETIME >= TO_DATE(:NOWFROMTIME, 'YYYYMMDDHH24MISS') ")
                        .AppendLine("             AND  NOT((T3.TEMP_FLG = :TEMP_FLG AND T3.STALL_USE_STATUS IN (:STATUS_00, :STATUS_01)) OR T3.STALL_USE_STATUS = :STATUS_07)  ")
                        .AppendLine("        GROUP BY  T1.SVCIN_ID ")
                        .AppendLine("          HAVING  NVL(MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE , NULL, TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MISS'))) ")
                        .AppendLine("                    , MIN(DECODE(T3.SCHE_START_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MISS')))) BETWEEN :NOWFROMTIME  AND :NOWTOTIME ")
                        .AppendLine("              OR  NVL(MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE , NULL, TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MISS'))) ")
                        .AppendLine("                    , MIN(DECODE(T3.SCHE_START_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MISS')))) BETWEEN :NEXTFROMTIME AND :NEXTTOTIME ")
                        .AppendLine("          ) S1 ")
                        .AppendLine("         ,TB_T_JOB_DTL T7 ")
                        .AppendLine("         ,TB_M_MERCHANDISE T8 ")
                        .AppendLine("         ,TBL_SERVICE_VISIT_MANAGEMENT T9 ")
                        .AppendLine("         ,TBL_SERVICEIN_APPEND T10 ")
                        .AppendLine("  WHERE  S1.SVCIN_ID = T9.FREZID (+) ")
                        .AppendLine("    AND  S1.JOB_DTL_ID = T7.JOB_DTL_ID ")
                        .AppendLine("    AND  T7.MERC_ID = T8.MERC_ID(+)  ")
                        .AppendLine("    AND  S1.CST_ID = T10.CST_ID(+) ")
                        .AppendLine("    AND  S1.VCL_ID = T10.VCL_ID(+)  ")
                        .AppendLine("    AND  T9.DLRCD(+) = :DLRCD ")
                        .AppendLine("    AND  T9.STRCD(+) = :STRCD ")
                        .AppendLine("    AND  (T9.ASSIGNSTATUS IS NULL OR T9.ASSIGNSTATUS IN (:ASSIGNSTATUS_0, :ASSIGNSTATUS_1)) ")

                    End With


                    'パラメータ設定

                    '当日フラグ
                    query.AddParameterWithTypeValue("TODAY", OracleDbType.NVarchar2, nowDay)
                    '日付省略値
                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
                    '販売店コード
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                    '店舗コード
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                    '予約ステータス
                    query.AddParameterWithTypeValue("RESV_STATUS", OracleDbType.NVarchar2, RezStatus)
                    '担当SAスタッフコード
                    query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, saCode)
                    'サービスステータス
                    query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
                    'キャンセルフラグ
                    query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)

                    '仮置フラグ
                    query.AddParameterWithTypeValue("TEMP_FLG", OracleDbType.NVarchar2, TenpFlag)
                    'ストール利用ステータス
                    query.AddParameterWithTypeValue("STATUS_00", OracleDbType.NVarchar2, StallStatusInstruct)
                    query.AddParameterWithTypeValue("STATUS_01", OracleDbType.NVarchar2, StallStatusWaitWork)
                    query.AddParameterWithTypeValue("STATUS_07", OracleDbType.NVarchar2, StallStatusUncome)
                    '振当ステータス
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_0", OracleDbType.NVarchar2, NonAssign)
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_1", OracleDbType.NVarchar2, AssignWait)

                    '条件(当日)
                    '予定入庫日時・予定開始日時
                    query.AddParameterWithTypeValue("NOWFROMTIME", OracleDbType.NVarchar2, nowDay + "000000")
                    query.AddParameterWithTypeValue("NOWTOTIME", OracleDbType.NVarchar2, nowDay + "235959")
                    query.AddParameterWithTypeValue("NEXTFROMTIME", OracleDbType.NVarchar2, nextBusinessDay + "000000")
                    query.AddParameterWithTypeValue("NEXTTOTIME", OracleDbType.NVarchar2, nextBusinessDay + "235959")

                    'SQL格納
                    query.CommandText = sql.ToString()

                    '検索結果返却
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

        ''' <summary>
        ''' 事前準備チップ予約情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </History>
        Public Function GetAdvancePreparationsReserveInfoData(ByVal dealerCode As String, _
                                                              ByVal branchCode As String, _
                                                              ByVal reserveId As Decimal) _
                                                              As SC3140103DataSet.SC3140103AdvancePreparationsReserveInfoDataTable
            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, reserveId = {5}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , dealerCode _
                      , branchCode _
                      , reserveId))

            Dim dt As SC3140103DataSet.SC3140103AdvancePreparationsReserveInfoDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AdvancePreparationsReserveInfoDataTable)("SC3140103_009")

                    Dim sql As New StringBuilder

                    'SQL文作成
                    With sql

                        .AppendLine(" SELECT  /* SC3140103_009 */ ")
                        .AppendLine("         DECODE(T1.CARWASH_NEED_FLG, ' ', '0', T1.CARWASH_NEED_FLG) AS WASHFLG ")
                        .AppendLine("        ,TRIM(T1.PIC_SA_STF_CD) AS ACCOUNT_PLAN ")
                        .AppendLine("        ,TRIM(T1.RO_NUM) AS ORDERNO ")
                        .AppendLine("        ,DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE, NULL, TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MI')) AS STOCKTIME ")
                        .AppendLine("        ,DECODE(T3.SCHE_START_DATETIME, :MINDATE, TO_DATE(NULL), T3.SCHE_START_DATETIME) AS WORKTIME ")
                        .AppendLine("        ,TRIM(T4.CST_NAME) AS CUSTOMERNAME ")
                        .AppendLine("        ,TRIM(T4.CST_PHONE) AS TELNO ")
                        .AppendLine("        ,TRIM(T4.CST_MOBILE) AS MOBILE ")
                        .AppendLine("        ,DECODE(T8.CST_ID, NULL, NVL(TRIM(T5.CST_TYPE), '2'), '1') AS CUSTOMERFLAG ")
                        .AppendLine("        ,NVL(TRIM(T6.VCL_VIN), TRIM(T8.VCL_VIN)) AS VIN ")
                        .AppendLine("        ,TRIM(T6.VCL_KATASHIKI) AS MODELCODE ")
                        .AppendLine("        ,TRIM(T7.REG_NUM) AS VCLREGNO ")
                        .AppendLine("   FROM  TB_T_SERVICEIN T1 ")
                        .AppendLine("        ,TB_T_JOB_DTL T2 ")
                        .AppendLine("        ,TB_T_STALL_USE T3 ")
                        .AppendLine("        ,TB_M_CUSTOMER T4 ")
                        .AppendLine("        ,TB_M_CUSTOMER_DLR T5 ")
                        .AppendLine("        ,TB_M_VEHICLE T6 ")
                        .AppendLine("        ,TB_M_VEHICLE_DLR T7 ")
                        .AppendLine("        ,TBL_SERVICEIN_APPEND T8 ")
                        .AppendLine("  WHERE  T1.SVCIN_ID = T2.SVCIN_ID ")
                        .AppendLine("    AND  T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                        .AppendLine("    AND  T1.CST_ID = T4.CST_ID (+) ")
                        .AppendLine("    AND  T1.DLR_CD = T5.DLR_CD (+) ")
                        .AppendLine("    AND  T1.CST_ID = T5.CST_ID (+) ")
                        .AppendLine("    AND  T1.VCL_ID = T6.VCL_ID (+) ")
                        .AppendLine("    AND  T1.DLR_CD = T7.DLR_CD (+) ")
                        .AppendLine("    AND  T1.VCL_ID = T7.VCL_ID (+) ")
                        .AppendLine("    AND  T1.CST_ID = T8.CST_ID (+) ")
                        .AppendLine("    AND  T1.VCL_ID = T8.VCL_ID (+) ")
                        .AppendLine("    AND  T1.DLR_CD = :DLRCD ")
                        .AppendLine("    AND  T1.BRN_CD = :STRCD ")
                        .AppendLine("    AND  T1.SVCIN_ID = :REZID ")
                        .AppendLine("    AND  NOT EXISTS (SELECT 1 ")
                        .AppendLine("                       FROM TB_T_SERVICEIN D1 ")
                        .AppendLine("                      WHERE D1.SVCIN_ID = T1.SVCIN_ID ")
                        .AppendLine("                        AND D1.SVC_STATUS = :STATUS_CANCEL) ")
                        .AppendLine("    AND  T2.CANCEL_FLG = :CANCEL_FLG ")
                        .AppendLine("    AND  T3.DLR_CD = :DLRCD ")
                        .AppendLine("    AND  T3.BRN_CD = :STRCD ")

                    End With

                    query.CommandText = sql.ToString()

                    'パラメータ設定

                    '日付省略値
                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
                    '販売店コード
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                    '店舗コード
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                    'サービス入庫ID
                    query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, reserveId)
                    'サービスステータス
                    query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
                    'キャンセルフラグ
                    query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)

                    '検索結果返却
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

        ''' <summary>
        ''' 事前準備チップサービス来店管理情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </History>
        Public Function GetAdvancePreparationsServiceVisitManagementData(ByVal dealerCode As String, _
                                                                         ByVal branchCode As String, _
                                                                         ByVal reserveId As Decimal) As SC3140103DataSet.SC3140103AdvancePreparationsServiceVisitManagementDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, reserveId = {5}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , dealerCode _
                      , branchCode _
                      , reserveId))

            Dim dt As SC3140103DataSet.SC3140103AdvancePreparationsServiceVisitManagementDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AdvancePreparationsServiceVisitManagementDataTable)("SC3140103_010")
                    Dim sql As New StringBuilder

                    'SQL文作成
                    With sql
                        .Append(" SELECT /* SC3140103_010 */ ")
                        .Append("        VISITSEQ ")
                        .Append("      , CUSTSEGMENT ")
                        .Append("      , ASSIGNSTATUS ")
                        .Append("      , SACODE ")
                        .Append("      , ORDERNO ")
                        .Append("   FROM TBL_SERVICE_VISIT_MANAGEMENT ")
                        .Append("  WHERE DLRCD = :DLRCD ")
                        .Append("    AND STRCD = :STRCD ")
                        .Append("    AND FREZID = :REZID ")
                    End With

                    query.CommandText = sql.ToString()
                    'バインド変数

                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                    query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, reserveId)

                    '検索結果返却
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

        ''' <summary>
        ''' 事前準備チップ情報取得(担当SAが未当て)
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="nowDay">本日年月日（YYYYMMDD）</param>
        ''' <param name="nextBusinessDay">翌営業年月日（YYYYMMDD）</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' </History>
        Public Function GetAdvancePreparationsChipDataNoSA(ByVal dealerCode As String, _
                                                           ByVal branchCode As String, _
                                                           ByVal nowDay As String, _
                                                           ByVal nextBusinessDay As String) _
                                                           As SC3140103DataSet.SC3140103AdvancePreparationsDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, nowDay = {5}, nextBusinessDay = {6}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , dealerCode _
                      , branchCode _
                      , nowDay _
                      , nextBusinessDay))

            Dim dt As SC3140103DataSet.SC3140103AdvancePreparationsDataTable

            Try
                Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AdvancePreparationsDataTable)("SC3140103_020")

                    Dim sql As New StringBuilder

                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                    'With sql
                    '    .AppendLine(" SELECT /* SC3140103_020 */ ")
                    '    .AppendLine("        R4.DLRCD ")
                    '    .AppendLine("      , R4.STRCD ")
                    '    .AppendLine("      , R4.REZID ")
                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T1.VCLREGNO, NULL))          AS VCLREGNO ")
                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T1.VIN, NULL))               AS VIN ")
                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T1.CUSTOMERNAME, NULL))      AS CUSTOMERNAME ")
                    '    .AppendLine("      , R4.REZ_PICK_DATE ")
                    '    .AppendLine("      , R4.REZ_DELI_DATE ")
                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T4.MERCHANDISENAME, NULL))   AS MERCHANDISENAME ")
                    '    .AppendLine("      , NVL(MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T3.VISITSEQ, NULL)), -1) AS VISITSEQ ")
                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T3.ASSIGNSTATUS, NULL))      AS ASSIGNSTATUS ")
                    '    .AppendLine("      , MAX(DECODE(R4.STARTTIME, T1.STARTTIME, T1.ORDERNO, NULL))           AS ORDERNO ")
                    '    .AppendLine("      , DECODE(SUBSTR(R4.REZ_PICK_DATE, 1, 8), :TODAY, 1, 0)                AS TODAYFLG ")
                    '    .AppendLine("   FROM TBL_STALLREZINFO T1, ")
                    '    .AppendLine("        TBL_SERVICE_VISIT_MANAGEMENT T3, ")
                    '    .AppendLine("        TBL_MERCHANDISEMST T4, ")
                    '    .AppendLine("        (SELECT R2.DLRCD ")
                    '    .AppendLine("              , R2.STRCD ")
                    '    .AppendLine("              , NVL(R2.PREZID,R2.REZID) AS REZID ")
                    '    .AppendLine("              , NVL(MIN(R2.REZ_PICK_DATE), TO_CHAR(MIN(R2.STARTTIME), 'YYYYMMDDHH24MI')) AS REZ_PICK_DATE ")
                    '    .AppendLine("              , NVL(MAX(R2.REZ_DELI_DATE), TO_CHAR(MAX(R2.ENDTIME), 'YYYYMMDDHH24MI'))   AS REZ_DELI_DATE ")
                    '    .AppendLine("              , MIN(R2.STARTTIME) AS STARTTIME ")
                    '    .AppendLine("           FROM TBL_STALLREZINFO R2, ")
                    '    .AppendLine("                TBL_USERS T5")
                    '    .AppendLine("          WHERE R2.DLRCD = :DLRCD ")
                    '    .AppendLine("            AND R2.STRCD = :STRCD ")
                    '    .AppendLine("            AND R2.STARTTIME >= TO_DATE(:NOWFROMTIME,'YYYYMMDDHH24MI') ")
                    '    .AppendLine("            AND (TRIM(R2.ACCOUNT_PLAN) IS NULL ")
                    '    .AppendLine("            OR  T5.DELFLG = 1 ) ")
                    '    .AppendLine("            AND R2.STATUS = 1 ")
                    '    .AppendLine("            AND R2.ACCOUNT_PLAN = T5.ACCOUNT(+) ")
                    '    .AppendLine("            AND DECODE(R2.STOPFLG,'0',DECODE(R2.CANCELFLG,'1',1,0),0) + ")
                    '    .AppendLine("                DECODE(R2.REZCHILDNO,0,1,0) + ")
                    '    .AppendLine("                DECODE(R2.REZCHILDNO,999,1,0) = 0 ")
                    '    .AppendLine("            AND NOT EXISTS( ")
                    '    .AppendLine("                SELECT 1 ")
                    '    .AppendLine("                  FROM TBL_STALLREZINFO R3 ")
                    '    .AppendLine("                 WHERE R2.DLRCD = R3.DLRCD ")
                    '    .AppendLine("                   AND R2.STRCD = R3.STRCD ")
                    '    .AppendLine("                   AND (R2.PREZID = R3.PREZID OR R2.REZID = R3.REZID) ")
                    '    .AppendLine("                   AND DECODE(R3.STOPFLG,'0',DECODE(R3.CANCELFLG,'1',1,0),0) + ")
                    '    .AppendLine("                       DECODE(R3.REZCHILDNO,0,1,0) + ")
                    '    .AppendLine("                       DECODE(R3.REZCHILDNO,999,1,0) = 0 ")
                    '    .AppendLine("                   AND (R3.STOPFLG = '2' OR R3.STOPFLG = '6' ")
                    '    .AppendLine("                    OR NVL(R3.REZ_PICK_DATE,TO_CHAR(R3.STARTTIME,'YYYYMMDDHH24MI')) < :NOWFROMTIME)) ")
                    '    .AppendLine("          GROUP BY R2.DLRCD, ")
                    '    .AppendLine("                   R2.STRCD, ")
                    '    .AppendLine("                   NVL(R2.PREZID,R2.REZID) ")
                    '    .AppendLine("         HAVING NVL(MIN(R2.REZ_PICK_DATE),TO_CHAR(MIN(R2.STARTTIME),'YYYYMMDDHH24MI')) BETWEEN :NOWFROMTIME  AND :NOWTOTIME  ")
                    '    .AppendLine("             OR NVL(MIN(R2.REZ_PICK_DATE),TO_CHAR(MIN(R2.STARTTIME),'YYYYMMDDHH24MI')) BETWEEN :NEXTFROMTIME AND :NEXTTOTIME) R4 ")
                    '    .AppendLine("  WHERE T1.DLRCD = R4.DLRCD ")
                    '    .AppendLine("    AND T1.STRCD = R4.STRCD ")
                    '    .AppendLine("    AND (R4.REZID = T1.REZID OR R4.REZID = T1.PREZID) ")
                    '    .AppendLine("    AND R4.DLRCD = T3.DLRCD(+) ")
                    '    .AppendLine("    AND R4.STRCD = T3.STRCD(+) ")
                    '    .AppendLine("    AND R4.REZID = T3.FREZID(+) ")
                    '    .AppendLine("    AND T1.DLRCD = T4.DLRCD(+) ")
                    '    .AppendLine("    AND T1.MERCHANDISECD = T4.MERCHANDISECD(+) ")
                    '    .AppendLine("    AND DECODE(T1.STOPFLG,'0',DECODE(T1.CANCELFLG,'1',1,0),0) + ")
                    '    .AppendLine("        DECODE(T1.REZCHILDNO,0,1,0) + ")
                    '    .AppendLine("        DECODE(T1.REZCHILDNO,999,1,0) = 0 ")
                    '    .AppendLine("    AND (T3.ASSIGNSTATUS IS NULL OR T3.ASSIGNSTATUS = '0' OR T3.ASSIGNSTATUS = '1') ")
                    '    .AppendLine("  GROUP BY R4.DLRCD, ")
                    '    .AppendLine("           R4.STRCD, ")
                    '    .AppendLine("           R4.REZID, ")
                    '    .AppendLine("           R4.REZ_PICK_DATE, ")
                    '    .AppendLine("           R4.REZ_DELI_DATE ")
                    'End With

                    'query.CommandText = sql.ToString()

                    ''バインド変数
                    'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                    'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                    'query.AddParameterWithTypeValue("TODAY", OracleDbType.Char, nowDay)
                    'query.AddParameterWithTypeValue("NOWFROMTIME", OracleDbType.Char, nowDay + "0000")
                    'query.AddParameterWithTypeValue("NOWTOTIME", OracleDbType.Char, nowDay + "2359")
                    'query.AddParameterWithTypeValue("NEXTFROMTIME", OracleDbType.Char, nextBusinessDay + "0000")
                    'query.AddParameterWithTypeValue("NEXTTOTIME", OracleDbType.Char, nextBusinessDay + "2359")


                    With sql

                        .AppendLine(" SELECT  /* SC3140103_020 */ ")
                        .AppendLine("         TRIM(S1.DLR_CD) AS DLRCD ")
                        .AppendLine("        ,TRIM(S1.BRN_CD) AS STRCD ")
                        .AppendLine("        ,S1.SVCIN_ID AS REZID ")
                        .AppendLine("        ,S1.RO_NUM AS ORDERNO ")
                        .AppendLine("        ,NVL(S1.SCHE_SVCIN_DATETIME, S1.SCHE_START_DATETIME) AS REZ_PICK_DATE ")
                        .AppendLine("        ,NVL(S1.SCHE_DELI_DATETIME, S1.SCHE_END_DATETIME) AS REZ_DELI_DATE ")
                        .AppendLine("        ,S1.CST_NAME AS CUSTOMERNAME ")
                        .AppendLine("        ,NVL(S1.VCL_VIN, TRIM(T11.VCL_VIN)) AS VIN ")
                        .AppendLine("        ,S1.REG_NUM AS VCLREGNO ")
                        .AppendLine("        ,NVL(TRIM(T9.MERC_NAME), TRIM(T9.MERC_NAME_ENG)) AS MERCHANDISENAME ")
                        .AppendLine("        ,NVL(T10.VISITSEQ, -1) AS VISITSEQ ")
                        .AppendLine("        ,T10.ASSIGNSTATUS AS ASSIGNSTATUS ")
                        .AppendLine("        ,DECODE(SUBSTR(S1.SCHE_SVCIN_DATETIME, 1, 8), :TODAY, 1, 0) AS TODAYFLG ")
                        .AppendLine("   FROM          ")
                        .AppendLine("         (SELECT ")
                        .AppendLine("                  MAX(T1.DLR_CD) AS DLR_CD ")
                        .AppendLine("                 ,MAX(T1.BRN_CD) AS BRN_CD ")
                        .AppendLine("                 ,T1.SVCIN_ID ")
                        .AppendLine("                 ,MAX(TRIM(T1.RO_NUM)) AS RO_NUM ")
                        .AppendLine("                 ,MAX(T1.CST_ID) AS CST_ID ")
                        .AppendLine("                 ,MAX(T1.VCL_ID) AS VCL_ID ")
                        .AppendLine("                 ,MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE , TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MI'), TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_SVCIN_DATETIME ")
                        .AppendLine("                 ,MAX(DECODE(T1.SCHE_DELI_DATETIME, :MINDATE , TO_CHAR(T3.SCHE_END_DATETIME, 'YYYYMMDDHH24MI'), TO_CHAR(T1.SCHE_DELI_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_DELI_DATETIME ")
                        .AppendLine("                 ,MIN(T2.JOB_DTL_ID) AS JOB_DTL_ID ")
                        .AppendLine("                 ,MIN(DECODE(T3.SCHE_START_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_START_DATETIME ")
                        .AppendLine("                 ,MIN(DECODE(T3.SCHE_END_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_END_DATETIME, 'YYYYMMDDHH24MI'))) AS SCHE_END_DATETIME ")
                        .AppendLine("                 ,MAX(TRIM(T4.CST_NAME)) AS CST_NAME ")
                        .AppendLine("                 ,MAX(TRIM(T5.VCL_VIN)) AS VCL_VIN ")
                        .AppendLine("                 ,MAX(TRIM(T5.VCL_KATASHIKI)) AS VCL_KATASHIKI ")
                        .AppendLine("                 ,MAX(TRIM(T6.REG_NUM)) AS REG_NUM ")
                        .AppendLine("            FROM  TB_T_SERVICEIN T1 ")
                        .AppendLine("                 ,TB_T_JOB_DTL T2 ")
                        .AppendLine("                 ,TB_T_STALL_USE T3 ")
                        .AppendLine("                 ,TB_M_CUSTOMER T4 ")
                        .AppendLine("                 ,TB_M_VEHICLE T5 ")
                        .AppendLine("                 ,TB_M_VEHICLE_DLR T6 ")
                        .AppendLine("                 ,TBL_USERS T7 ")
                        .AppendLine("           WHERE  T1.SVCIN_ID = T2.SVCIN_ID ")
                        .AppendLine("             AND  T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                        .AppendLine("             AND  T1.CST_ID = T4.CST_ID (+) ")
                        .AppendLine("             AND  T1.VCL_ID = T5.VCL_ID (+) ")
                        .AppendLine("             AND  T1.DLR_CD = T6.DLR_CD (+) ")
                        .AppendLine("             AND  T1.VCL_ID = T6.VCL_ID (+) ")
                        .AppendLine("             AND  T1.PIC_SA_STF_CD = T7.ACCOUNT (+) ")
                        .AppendLine("             AND  T1.DLR_CD = :DLRCD ")
                        .AppendLine("             AND  T1.BRN_CD = :STRCD ")
                        .AppendLine("             AND  T1.RESV_STATUS = :RESV_STATUS ")
                        .AppendLine("             AND  NOT EXISTS (SELECT 1 ")
                        .AppendLine("                                FROM TB_T_SERVICEIN D1 ")
                        .AppendLine("                               WHERE D1.SVCIN_ID = T1.SVCIN_ID ")
                        .AppendLine("                                 AND D1.SVC_STATUS = :STATUS_CANCEL) ")
                        .AppendLine("             AND  T2.CANCEL_FLG = :CANCEL_FLG ")
                        .AppendLine("             AND  T3.DLR_CD = :DLRCD ")
                        .AppendLine("             AND  T3.BRN_CD = :STRCD ")
                        .AppendLine("             AND  T3.SCHE_START_DATETIME >= TO_DATE(:NOWFROMTIME, 'YYYYMMDDHH24MISS') ")
                        .AppendLine("             AND  NOT((T3.TEMP_FLG = :TEMP_FLG AND T3.STALL_USE_STATUS IN (:STATUS_00, :STATUS_01)) OR T3.STALL_USE_STATUS = :STATUS_07)  ")
                        .AppendLine("             AND  (TRIM(T1.PIC_SA_STF_CD) IS NULL OR T7.DELFLG = :DELFLG ) ")
                        .AppendLine("        GROUP BY  T1.SVCIN_ID ")
                        .AppendLine("          HAVING  NVL(MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE , NULL, TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MISS'))) ")
                        .AppendLine("                    , MIN(DECODE(T3.SCHE_START_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MISS')))) BETWEEN :NOWFROMTIME  AND :NOWTOTIME ")
                        .AppendLine("              OR  NVL(MIN(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE , NULL, TO_CHAR(T1.SCHE_SVCIN_DATETIME, 'YYYYMMDDHH24MISS'))) ")
                        .AppendLine("                    , MIN(DECODE(T3.SCHE_START_DATETIME, :MINDATE , NULL, TO_CHAR(T3.SCHE_START_DATETIME, 'YYYYMMDDHH24MISS')))) BETWEEN :NEXTFROMTIME AND :NEXTTOTIME ")
                        .AppendLine("          ) S1 ")
                        .AppendLine("         ,TB_T_JOB_DTL T8 ")
                        .AppendLine("         ,TB_M_MERCHANDISE T9 ")
                        .AppendLine("         ,TBL_SERVICE_VISIT_MANAGEMENT T10 ")
                        .AppendLine("         ,TBL_SERVICEIN_APPEND T11 ")
                        .AppendLine("  WHERE  S1.SVCIN_ID = T10.FREZID (+) ")
                        .AppendLine("    AND  S1.JOB_DTL_ID = T8.JOB_DTL_ID ")
                        .AppendLine("    AND  T8.MERC_ID = T9.MERC_ID(+)  ")
                        .AppendLine("    AND  S1.CST_ID = T11.CST_ID(+) ")
                        .AppendLine("    AND  S1.VCL_ID = T11.VCL_ID(+)  ")
                        .AppendLine("    AND  T10.DLRCD(+) = :DLRCD ")
                        .AppendLine("    AND  T10.STRCD(+) = :STRCD ")
                        .AppendLine("    AND  (T10.ASSIGNSTATUS IS NULL OR T10.ASSIGNSTATUS IN (:ASSIGNSTATUS_0, :ASSIGNSTATUS_1)) ")

                    End With


                    'パラメータ設定

                    '当日フラグ
                    query.AddParameterWithTypeValue("TODAY", OracleDbType.NVarchar2, nowDay)
                    '日付省略値
                    query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
                    '販売店コード
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                    '店舗コード
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                    '予約ステータス
                    query.AddParameterWithTypeValue("RESV_STATUS", OracleDbType.NVarchar2, RezStatus)
                    'サービスステータス
                    query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
                    'キャンセルフラグ
                    query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)

                    '仮置フラグ
                    query.AddParameterWithTypeValue("TEMP_FLG", OracleDbType.NVarchar2, TenpFlag)
                    'ストール利用ステータス
                    query.AddParameterWithTypeValue("STATUS_00", OracleDbType.NVarchar2, StallStatusInstruct)
                    query.AddParameterWithTypeValue("STATUS_01", OracleDbType.NVarchar2, StallStatusWaitWork)
                    query.AddParameterWithTypeValue("STATUS_07", OracleDbType.NVarchar2, StallStatusUncome)
                    '削除フラグ
                    query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, Delete)
                    '振当ステータス
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_0", OracleDbType.NVarchar2, NonAssign)
                    query.AddParameterWithTypeValue("ASSIGNSTATUS_1", OracleDbType.NVarchar2, AssignWait)

                    '条件(当日)
                    '予定入庫日時・予定開始日時
                    query.AddParameterWithTypeValue("NOWFROMTIME", OracleDbType.NVarchar2, nowDay + "000000")
                    query.AddParameterWithTypeValue("NOWTOTIME", OracleDbType.NVarchar2, nowDay + "235959")
                    query.AddParameterWithTypeValue("NEXTFROMTIME", OracleDbType.NVarchar2, nextBusinessDay + "000000")
                    query.AddParameterWithTypeValue("NEXTTOTIME", OracleDbType.NVarchar2, nextBusinessDay + "235959")


                    'SQL格納
                    query.CommandText = sql.ToString()

                    '2013/06/03 TMEJ 河原 IT9530_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                    '検索結果返却
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

#End Region

#Region "SA振当処理"

        ''' <summary>
        ''' SA振当用来店管理情報取得
        ''' </summary>
        ''' <param name="inVisitSeq">来店実績連番</param>
        ''' <param name="inUpDateTime">更新日時</param>
        ''' <returns>来店管理情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetSAAssignInfo(ByVal inVisitSeq As Long, _
                                        ByVal inUpDateTime As Date) _
                                        As SC3140103DataSet.SC3140103SAAssignInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} VISITSEQ:{2} UPDATEDATE:{3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inVisitSeq, inUpDateTime))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103SAAssignInfoDataTable)("SC3140103_032")

                Dim sql As New StringBuilder      ' SQL文格納

                With sql

                    .AppendLine("SELECT /* SC3140103_032 */")
                    .AppendLine("       VISITSEQ")
                    .AppendLine("     , DEFAULTSACODE")
                    .AppendLine("     , NVL(REZID, -1) AS REZID")
                    .AppendLine("     , ORDERNO")
                    .AppendLine("  FROM TBL_SERVICE_VISIT_MANAGEMENT")
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ")
                    .AppendLine("   AND UPDATEDATE = :UPDATEDATE")

                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSeq)
                '更新日時(チップ表示時の値)
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpDateTime)

                '実行
                Dim dt As SC3140103DataSet.SC3140103SAAssignInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END COUNT = {2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , dt.Count))

                Return dt

            End Using
        End Function

        ''' <summary>
        ''' 最新予約情報取得
        ''' </summary>
        ''' <param name="inRezID">予約ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetNewReservationInfo(ByVal inRezId As Decimal) _
                                              As SC3140103DataSet.SC3140103NewReservationInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} REZID:{2}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inRezId))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103NewReservationInfoDataTable)("SC3140103_033")

                Dim sql As New StringBuilder      ' SQL文格納

                With sql

                    .AppendLine(" SELECT /* SC3140103_033 */ ")
                    .AppendLine("        TRIM(T1.PIC_SA_STF_CD) AS ACCOUNT_PLAN ")
                    .AppendLine("       ,TRIM(T1.RO_NUM) AS ORDERNO ")
                    .AppendLine("       ,ROW_LOCK_VERSION AS ROW_LOCK_VERSION ")
                    .AppendLine("   FROM ")
                    .AppendLine("        TB_T_SERVICEIN T1 ")
                    .AppendLine("  WHERE ")
                    .AppendLine("        T1.SVCIN_ID = :REZID ")
                    .AppendLine("    AND T1.SVC_STATUS <> :SVC_STATUS ")

                End With

                'SQL設定
                query.CommandText = sql.ToString()


                'バインド変数
                '予約ID
                query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, inRezId)
                'サービスステータス("02"：キャンセル)
                query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, StatusCancel)


                '実行
                Dim dt As SC3140103DataSet.SC3140103NewReservationInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END COUNT = {2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , dt.Count))

                Return dt

            End Using
        End Function

        ''' <summary>
        ''' SA振当登録更新処理
        ''' </summary>
        ''' <param name="inRowVisitInfo">来店管理情報</param>
        ''' <param name="inAccount">ログインアカウント</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function RegisterSAAssign(ByVal inRowVisitInfo As SC3140103DataSet.SC3140103SAAssignInfoRow _
                                       , ByVal inAccount As String) _
                                         As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} START" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'UPDATE件数返却
            Dim updateCount As Integer = 0

            Using query As New DBUpdateQuery("SC3140103_034")
                Dim sql As New StringBuilder

                With sql

                    .AppendLine("UPDATE /* SC3140103_034 */")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                    .AppendLine("   SET DEFAULTSACODE = :DEFAULTSACODE")        ' 受付担当予定者
                    .AppendLine("     , SACODE = :SACODE")                      ' 振当SA
                    .AppendLine("     , ASSIGNTIMESTAMP = :ASSIGNTIMESTAMP")    ' 振当時間
                    .AppendLine("     , ASSIGNSTATUS = :ASSIGNSTATUS")          ' 振当ステータス
                    .AppendLine("     , QUEUESTATUS = :QUEUESTATUS")            ' 案内待ちキュー状態
                    .AppendLine("     , HOLDSTAFF = NULL")                      ' ホールドスタッフ
                    .AppendLine("     , ORDERNO = :ORDERNO")                    ' 整備受注No
                    .AppendLine("     , UPDATEDATE = :PRESENTTIME")             ' 更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")        ' 更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID")                  ' 更新機能ID
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ")                  ' 来店実績連番

                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数
                '受付担当予定者
                query.AddParameterWithTypeValue("DEFAULTSACODE", OracleDbType.NVarchar2, inRowVisitInfo.DEFAULTSACODE)
                '振当SA
                query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, inRowVisitInfo.SACODE)
                '振当時間(現在日時)
                query.AddParameterWithTypeValue("ASSIGNTIMESTAMP", OracleDbType.Date, inRowVisitInfo.PRESENTTIME)
                '振当ステータス("2"：SA振当済)
                query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, AssignFinish)
                '案内待ちキュー状態("1"：非案内待ち)
                query.AddParameterWithTypeValue("QUEUESTATUS", OracleDbType.NVarchar2, QueueStatusNotWait)
                '整備受注No
                query.AddParameterWithTypeValue("ORDERNO", OracleDbType.NVarchar2, inRowVisitInfo.ORDERNO)
                '更新日(現在日時)
                query.AddParameterWithTypeValue("PRESENTTIME", OracleDbType.Date, inRowVisitInfo.PRESENTTIME)
                '更新アカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)
                '更新機能ID  
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationId)
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inRowVisitInfo.VISITSEQ)

                '処理結果
                updateCount = query.Execute()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END UPDATECOUNT = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , updateCount))

            Return updateCount

        End Function

#End Region

#Region "呼出し完了更新処理"

        ''' <summary>
        ''' 呼び出し完了更新処理
        ''' </summary>
        ''' <param name="inVisitSeq">来店実績連番</param>
        ''' <param name="inUpDateTime">更新日時</param>
        ''' <param name="inAccount">アカウント</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <returns>更新件数取得</returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' </History>
        Public Function UpdateCallCompleted(ByVal inVisitSeq As Long _
                                          , ByVal inUpDateTime As Date _
                                          , ByVal inAccount As String _
                                          , ByVal inPresentTime As Date) As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} VISITSEQ:{2} UPDATEDATE:{3} ACCOUNT:{4} PRESENTTIME:{5}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inVisitSeq, inUpDateTime, inAccount, inPresentTime))


            Using query As New DBUpdateQuery("SC3140103_023")
                Dim sql As New StringBuilder

                With sql

                    .AppendLine("UPDATE /* SC3140103_023 */ ")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT ")
                    .AppendLine("   SET CALLSTATUS = :CALLED ")              ' 呼出ステータス
                    .AppendLine("     , CALLENDDATE = :NOWDATE ")            ' 呼出完了日時
                    .AppendLine("     , UPDATEDATE = :NOWDATE ")             ' 更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT ")    ' 更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID ")              ' 更新機能ID
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ ")              ' 来店実績連番
                    .AppendLine("     AND SACODE = :UPDATEACCOUNT ")
                    .AppendLine("     AND UPDATEDATE = :UPDATEDATE ")

                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数
                '呼出ステータス("2"：呼出完了)
                query.AddParameterWithTypeValue("CALLED", OracleDbType.NVarchar2, CallEnd)
                '現在日時
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inPresentTime)
                '更新日
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpDateTime)
                '更新アカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)
                '更新機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationId)
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSeq)

                '処理結果
                Dim updateCount As Integer = query.Execute()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END RETURN:{2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , updateCount))

                Return updateCount

            End Using

        End Function

#End Region

#Region "画面遷移用来店情報取得"

        ''' <summary>
        ''' 画面遷移用来店情報取得
        ''' </summary>
        ''' <param name="inDetailArea">チップ詳細表示エリア</param>
        ''' <param name="inVisitSeq">来店実績連番</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inRowLockVersion">サービス入庫テーブルの行ロックバージョン(排他用)</param>
        ''' <param name="inPreviewFlg">参照フラグ(RO参照・顧客参照へ遷移する際は排他制御が必要ないため)True：参照モード</param>
        ''' <returns>画面遷移用来店情報データセット</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
        ''' </history>
        Public Function GetNextScreenVisitInfo(ByVal inDetailArea As Long _
                                             , ByVal inVisitSeq As Long _
                                             , ByVal inDealerCode As String _
                                             , ByVal inBranchCode As String _
                                             , ByVal inRowLockVersion As Long _
                                             , ByVal inPreviewFlg As Boolean) _
                                               As SC3140103DataSet.SC3140103NextScreenVisitInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} VISITSEQ:{2}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inVisitSeq))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103NextScreenVisitInfoDataTable)("SC3140103_035")

                Dim sql As New StringBuilder      ' SQL文格納

                With sql

                    .AppendLine("	SELECT  /* SC3140103_035 */ ")
                    .AppendLine("           T1.VISITSEQ ")
                    .AppendLine("          ,MAX(T1.DLRCD) AS DLRCD ")
                    .AppendLine("          ,MAX(T1.STRCD) AS STRCD ")
                    .AppendLine("          ,MAX(TRIM(T1.VCLREGNO)) AS VCLREGNO ")
                    .AppendLine("          ,MAX(TRIM(T1.DMSID)) AS DMSID ")
                    .AppendLine("          ,MAX(TRIM(T1.VIN)) AS VIN ")
                    .AppendLine("          ,MAX(CASE ")
                    .AppendLine("                    WHEN 0 < T1.FREZID THEN T1.FREZID ")
                    .AppendLine("                    ELSE 0 ")
                    .AppendLine("                END ) AS REZID ")
                    .AppendLine("          ,MAX(TRIM(T2.RO_NUM)) AS RO_NUM ")
                    .AppendLine("          ,MIN(T2.RO_SEQ) AS RO_SEQ ")
                    .AppendLine("          ,MIN(TRIM(T2.RO_STATUS)) AS RO_STATUS ")
                    .AppendLine("          ,MAX(TRIM(T5.DMS_JOB_DTL_ID)) AS DMS_JOB_DTL_ID ")

                    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START

                    .AppendLine("          ,MAX(T1.VISITNAME) AS VISITNAME ")
                    .AppendLine("          ,MAX(T1.VISITTELNO) AS VISITTELNO ")

                    '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                    .AppendLine("	 FROM   TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                    .AppendLine("          ,TB_T_RO_INFO T2 ")
                    .AppendLine("          ,TB_T_SERVICEIN T3 ")
                    .AppendLine("          ,(SELECT MAX(S3.SVCIN_ID) AS SVCIN_ID ")
                    .AppendLine("                  ,MIN(S3.JOB_DTL_ID) AS JOB_DTL_ID ")
                    .AppendLine("              FROM TBL_SERVICE_VISIT_MANAGEMENT S1 ")
                    .AppendLine("                  ,TB_T_SERVICEIN S2 ")
                    .AppendLine("                  ,TB_T_JOB_DTL S3 ")
                    .AppendLine("             WHERE S1.FREZID = S2.SVCIN_ID ")
                    .AppendLine("               AND S2.SVCIN_ID = S3.SVCIN_ID ")
                    .AppendLine("               AND S1.VISITSEQ = :VISITSEQ ")
                    .AppendLine("               AND S1.DLRCD = :DLRCD ")
                    .AppendLine("               AND S1.STRCD = :STRCD ")
                    .AppendLine("               AND S2.DLR_CD = :DLRCD ")
                    .AppendLine("               AND S2.BRN_CD = :STRCD ")
                    .AppendLine("               AND S2.SVC_STATUS <> :STATUS_CANCEL ")
                    .AppendLine("               AND S3.DLR_CD = :DLRCD ")
                    .AppendLine("               AND S3.BRN_CD = :STRCD ")
                    .AppendLine("               AND S3.CANCEL_FLG = :CANCELFLG ")
                    .AppendLine("          GROUP BY S1.VISITSEQ ")
                    .AppendLine("           ) T4 ")
                    .AppendLine("          ,TB_T_JOB_DTL T5 ")
                    .AppendLine("    WHERE  T1.VISITSEQ = T2.VISIT_ID(+) ")
                    .AppendLine("      AND  T1.FREZID = T3.SVCIN_ID(+) ")
                    .AppendLine("      AND  T3.SVCIN_ID = T4.SVCIN_ID(+) ")
                    .AppendLine("      AND  T4.JOB_DTL_ID = T5.JOB_DTL_ID(+) ")
                    .AppendLine("      AND  T1.VISITSEQ = :VISITSEQ ")
                    .AppendLine("      AND  T1.DLRCD = :DLRCD ")
                    .AppendLine("      AND  T1.STRCD = :STRCD ")
                    .AppendLine("      AND  T2.DLR_CD(+) = :DLRCD ")
                    .AppendLine("      AND  T2.BRN_CD(+) = :STRCD ")
                    .AppendLine("      AND  T2.RO_STATUS(+) <> :STATUS_99 ")
                    .AppendLine("      AND  T3.DLR_CD(+) = :DLRCD ")
                    .AppendLine("      AND  T3.BRN_CD(+) = :STRCD ")
                    .AppendLine("      AND  T3.SVC_STATUS(+) <> :STATUS_CANCEL ")

                    'モード確認　かつ　表示エリア　かつ　行ロックバージョンのチェック
                    If Not inPreviewFlg _
                        AndAlso inDetailArea = ChipArea.Preparation _
                        AndAlso 0 <= inRowLockVersion Then
                        '編集モードかつ納車準備エリアかつ行ロックバージョンが0以上の場合

                        '排他用条件の追加
                        .AppendLine("      AND  T3.ROW_LOCK_VERSION(+) = :ROW_LOCK_VERSION ")

                        '行ロックバージョン(排他用)
                        query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Int64, inRowLockVersion)

                    End If

                    .AppendLine("      AND  T5.DLR_CD(+) = :DLRCD ")
                    .AppendLine("      AND  T5.BRN_CD(+) = :STRCD ")
                    .AppendLine("      AND  T5.CANCEL_FLG(+) = :CANCELFLG ")
                    .AppendLine(" GROUP BY  T1.VISITSEQ ")

                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数

                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSeq)
                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                '店舗コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)
                'ROステータス（"99"：キャンセル）
                query.AddParameterWithTypeValue("STATUS_99", OracleDbType.NVarchar2, StatusROCancel)
                'サービスステータス
                query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
                'キャンセルフラグ
                query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.NVarchar2, CancelFlagEffective)

                '実行
                Dim dt As SC3140103DataSet.SC3140103NextScreenVisitInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END COUNT = {2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , dt.Count))

                Return dt

            End Using
        End Function


        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        ''' <summary>
        ''' 顧客詳細遷移用情報取得
        ''' </summary>
        ''' <param name="inVisitSeq">来店実績連番</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' </History>
        Public Function GetInfoToPassCustomerDetail(ByVal inVisitSeq As Long, _
                                                    ByVal inDealerCode As String, _
                                                    ByVal inBranchCode As String) _
                                                    As SC3140103DataSet.SC3140103InfoToPassCustomerDetailDataTable

            Dim dt As SC3140103DataSet.SC3140103InfoToPassCustomerDetailDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:VISITSEQ = {3}, DLRCD = {4}, STRCD = {5}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , inVisitSeq _
                      , inDealerCode _
                      , inBranchCode))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103InfoToPassCustomerDetailDataTable)("SC3140103_038")

                Dim sql As New StringBuilder

                With sql

                    .AppendLine("  SELECT /* SC3140103_038 */ ")
                    .AppendLine("         NVL(TRIM(T3.DMS_CST_CD), T1.DMSID) AS DMS_CST_CD ")
                    .AppendLine("    FROM ")
                    .AppendLine("         TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                    .AppendLine("       , TB_T_SERVICEIN T2 ")
                    .AppendLine("       , TB_M_CUSTOMER T3 ")
                    .AppendLine("   WHERE ")
                    .AppendLine("         T1.FREZID = T2.SVCIN_ID(+) ")
                    .AppendLine("     AND T2.CST_ID = T3.CST_ID(+) ")
                    .AppendLine("     AND T1.VISITSEQ = :VISITSEQ ")
                    .AppendLine("     AND T1.DLRCD = :DLRCD ")
                    .AppendLine("     AND T1.STRCD = :STRCD ")

                End With

                'パラメータ設定

                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, inVisitSeq)
                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                '店舗コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)

                'SQL格納
                query.CommandText = sql.ToString()

                '検索結果返却
                dt = query.GetData()

            End Using


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} OUT:COUNT = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_END _
                      , dt.Rows.Count))

            Return dt

        End Function

        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

#End Region

#Region "退店登録処理"

        ''' <summary>
        ''' 退店登録処理
        ''' </summary>
        ''' <param name="invisitSeq">来店実績連番</param>
        ''' <param name="inupDateTime">更新日時</param>
        ''' <param name="inAccount">更新アカウント</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function RegisterChipDelete(ByVal inVisitSeq As Long, _
                                           ByVal inUpDateTime As Date, _
                                           ByVal inAccount As String, _
                                           ByVal inPresentTime As Date) _
                                           As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} VISITSEQ:{2} UPDATETIME:{3} ACCOUNT:{4} PRESENTTIME:{5}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inVisitSeq, inUpDateTime, inAccount, inPresentTime))

            'UPDATE件数返却
            Dim updateCount As Integer = 0

            Using query As New DBUpdateQuery("SC3140103_036")

                'SQL文格納
                Dim sql As New StringBuilder

                With sql

                    .AppendLine("UPDATE /* SC3140103_036 */")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT ")
                    .AppendLine("   SET ASSIGNSTATUS = :ASSIGNSTATUS ")      '振当ステータス
                    .AppendLine("     , UPDATEDATE = :PRESENTTIME ")         '更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT ")    '更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID ")              '更新機能ID
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ ")              '来店実績連番
                    .AppendLine("   AND UPDATEDATE = :UPDATEDATE ")          '更新日時

                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数

                '振当ステータス("4"：退店)
                query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, DealerOut)
                '更新日
                query.AddParameterWithTypeValue("PRESENTTIME", OracleDbType.Date, inPresentTime)
                '更新アカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)
                '更新機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationId)
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSeq)
                '更新日時(排他用)
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpDateTime)

                '実行
                updateCount = query.Execute()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END UPDATECOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , updateCount))

            Return updateCount

        End Function

#End Region

#Region "振当解除登録処理"

        ''' <summary>
        ''' 振当解除登録処理
        ''' </summary>
        ''' <param name="invisitSeq">来店実績連番</param>
        ''' <param name="inupDateTime">更新日時</param>
        ''' <param name="inAccount">更新アカウント</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function RegisterAssignmentUndo(ByVal inVisitSeq As Long, _
                                               ByVal inUpDateTime As Date, _
                                               ByVal inAccount As String, _
                                               ByVal inPresentTime As Date) As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} VISITSEQ:{2} UPDATETIME:{3} ACCOUNT:{4} PRESENTTIME:{5}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inVisitSeq, inUpDateTime, inAccount, inPresentTime))

            Using query As New DBUpdateQuery("SC3140103_019")

                'SQL文格納
                Dim sql As New StringBuilder

                With sql

                    .AppendLine("UPDATE /* SC3140103_019 */")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                    .AppendLine("   SET ASSIGNSTATUS = :ASSIGNSTATUS")      '割振りステータス
                    .AppendLine("     , HOLDSTAFF = NULL")                  'HOLD案内係
                    .AppendLine("     , SACODE = NULL")                     '振当てSA
                    .AppendLine("     , ASSIGNTIMESTAMP = NULL")            'SA振当て日時
                    .AppendLine("     , CALLSTATUS = :NOTCALL")             '呼出ステータス
                    .AppendLine("     , CALLSTARTDATE = NULL ")             '呼出開始日時
                    .AppendLine("     , UPDATEDATE = :UPDATEDATE")          '更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")    '更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID")              '更新機能ID
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ")              '来店実績連番
                    '2018/10/26 NSK 坂本 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 No.67調査（SA振り当て解除）START
                    .AppendLine("   AND UPDATEDATE = :UPDATETIME ")         '更新日時
                    '2018/10/26 NSK 坂本 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 No.67調査（SA振り当て解除）END

                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数

                '振当ステータス("1"：受付待)
                query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, GetReserve)
                '呼出ステータス("0"：未呼出)
                query.AddParameterWithTypeValue("NOTCALL", OracleDbType.NVarchar2, NonCall)
                '更新日
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inPresentTime)
                '更新アカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)
                '更新機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationId)
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSeq)
                '更新日時(排他用)
                '2018/10/26 NSK 坂本 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 No.67調査（SA振り当て解除）START
                query.AddParameterWithTypeValue("UPDATETIME", OracleDbType.Date, inUpDateTime)
                '2018/10/26 NSK 坂本 17PRJ03047-06_TKM Next Gen e-CRB Project Test（Connectivity, SIT & UAT）Block D-1 No.67調査（SA振り当て解除）END

                '実行
                Dim updateCount As Integer = query.Execute()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END RETURN:{2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , updateCount))

                Return updateCount

            End Using

        End Function

        ''' <summary>
        ''' サービス来店管理情報取得
        ''' </summary>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <returns>サービス来店実績データセット</returns>
        ''' <remarks></remarks>
        Public Function GetVisitManagement(ByVal visitSeq As Long) _
                                           As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:VISITSEQ = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , visitSeq))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103ServiceVisitManagementDataTable)("SC3140103_002")
                'SQL文格納
                Dim sql As New StringBuilder

                'SQL文作成
                With sql

                    .Append("SELECT /* SC3140103_002 */ ")
                    .Append("       VISITSEQ ")
                    .Append("     , DLRCD ")
                    .Append("     , STRCD ")
                    .Append("     , NVL(VISITTIMESTAMP, :MINDATE) AS VISITTIMESTAMP ")
                    .Append("     , VCLREGNO ")
                    .Append("     , CUSTSEGMENT ")
                    .Append("     , DMSID ")
                    .Append("     , VIN ")
                    .Append("     , MODELCODE ")
                    .Append("     , NAME ")
                    .Append("     , TELNO ")
                    .Append("     , MOBILE ")
                    .Append("     , SACODE ")
                    .Append("     , NVL(ASSIGNTIMESTAMP, :MINDATE) AS ASSIGNTIMESTAMP ")
                    .Append("     , NVL(REZID, :MINREZID) AS REZID ")
                    .Append("     , PARKINGCODE ")
                    .Append("     , ORDERNO ")
                    .Append("     , NVL(FREZID, :MINREZID) AS FREZID ")
                    .Append("  FROM TBL_SERVICE_VISIT_MANAGEMENT ")
                    .Append(" WHERE VISITSEQ = :VISITSEQ ")

                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, visitSeq)
                '日付最小値
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, DateTime.MinValue)
                '予約最小値
                query.AddParameterWithTypeValue("MINREZID", OracleDbType.Decimal, MinReserveId)

                '検索結果返却
                Dim dt As SC3140103DataSet.SC3140103ServiceVisitManagementDataTable = query.GetData()

                '終了ログ
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} {2} OUT:COUNT = {3}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , LOG_END _
                          , dt.Rows.Count))

                Return dt

            End Using

        End Function

#End Region

#Region "呼出し登録処理"

        ''' <summary>
        ''' 呼出し登録処理
        ''' </summary>
        ''' <param name="inVisitSequence">来店実績連番</param>
        ''' <param name="inupdateDate">更新日時</param>
        ''' <param name="inNowdate">現在日時</param>
        ''' <param name="inAccount">アカウント</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' </History>
        Public Function UpdateVisitStausCall(ByVal inVisitSequence As Long, _
                                             ByVal inUpdateDate As Date, _
                                             ByVal inNowdate As Date, _
                                             ByVal inAccount As String) As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} START inVisitSequence:{2} inUpdateDate:{3} inNowDate:{4} inAccount:{5} " _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inVisitSequence _
                      , inUpdateDate _
                      , inNowdate _
                      , inAccount))

            Using query As New DBUpdateQuery("SC3140103_021")

                'SQL文格納
                Dim sql As New StringBuilder

                With sql

                    .AppendLine("UPDATE /* SC3140103_021 */ ")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT ")
                    .AppendLine("   SET CALLSTATUS = :CALLING ")             '呼出ステータス
                    .AppendLine("     , CALLSTARTDATE = :NOWDATE ")          '呼出開始日時
                    .AppendLine("     , UPDATEDATE = :NOWDATE ")             '更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT ")    '更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID ")              '更新機能ID
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ ")              '来店実績連番
                    .AppendLine("   AND UPDATEDATE = :UPDATEDATE ")
                    .AppendLine("   AND CALLSTATUS = :NOTCALL ")
                    .AppendLine("   AND SACODE = :UPDATEACCOUNT ")

                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数

                '呼出ステータス("1"：呼出中)
                query.AddParameterWithTypeValue("CALLING", OracleDbType.NVarchar2, Calling)
                '現在日時
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowdate)
                '更新アカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)
                '更新機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationId)
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSequence)
                '呼出ステータス("0"：未呼出)
                query.AddParameterWithTypeValue("NOTCALL", OracleDbType.NVarchar2, NonCall)
                '更新日(排他用)
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpdateDate)

                '実行
                Dim updateCount As Integer = query.Execute()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END RETURN:{2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , updateCount.ToString(CultureInfo.CurrentCulture)))

                Return updateCount

            End Using

        End Function

#End Region

#Region "呼出しキャンセル登録処理"

        ''' <summary>
        ''' お客様呼び出しキャンセル処理
        ''' </summary>
        ''' <param name="inVisitSequence">来店実績連番</param>
        ''' <param name="inupdateDate">更新日時</param>
        ''' <param name="inNowdate">現在日時</param>
        ''' <param name="inAccount">アカウント</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' </History>
        Public Function UpdateVisitStausCallCancel(ByVal inVisitSequence As Long, _
                                                   ByVal inUpdateDate As Date, _
                                                   ByVal inNowdate As Date, _
                                                   ByVal inAccount As String) As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} START inVisitSequence:{2} inUpdateDate:{3} inNowDate:{4} inAccount:{5} " _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inVisitSequence _
                      , inUpdateDate _
                      , inNowdate _
                      , inAccount))

            Using query As New DBUpdateQuery("SC3140103_022")

                'SQL文格納
                Dim sql As New StringBuilder

                With sql

                    .AppendLine("UPDATE /* SC3140103_022 */ ")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT ")
                    .AppendLine("   SET CALLSTATUS = :NOTCALL ")             '呼出ステータス
                    .AppendLine("     , CALLSTARTDATE = NULL ")              '呼出開始日時
                    .AppendLine("     , UPDATEDATE = :NOWDATE ")             '更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT ")    '更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID ")              '更新機能ID
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ ")              '来店実績連番
                    .AppendLine("   AND UPDATEDATE = :UPDATEDATE ")
                    .AppendLine("   AND CALLSTATUS = :CALLING ")
                    .AppendLine("   AND SACODE = :UPDATEACCOUNT ")

                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数

                '呼出ステータス("0"：未呼出)
                query.AddParameterWithTypeValue("NOTCALL", OracleDbType.NVarchar2, NonCall)
                '現在日時
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowdate)
                '更新アカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)
                '更新機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationId)
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSequence)
                '呼出ステータス("1"：呼出中)
                query.AddParameterWithTypeValue("CALLING", OracleDbType.NVarchar2, Calling)
                '更新日(排他用)
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpdateDate)

                '実行
                Dim updateCount As Integer = query.Execute()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END RETURN:{2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , updateCount.ToString(CultureInfo.CurrentCulture)))

                Return updateCount

            End Using

        End Function

#End Region

#Region "呼び出し場所更新"

        ''' <summary>
        ''' 呼び出し場所更新
        ''' </summary>
        ''' <param name="inVisitSequence">来店実績連番</param>
        ''' <param name="inCallPlace">呼出場所</param>
        ''' <param name="inupdateDate">更新日時</param>
        ''' <param name="inNowdate">現在日時</param>
        ''' <param name="inAccount">アカウント</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' </History>
        Public Function UpdateCallPlace(ByVal inVisitSequence As Long, _
                                        ByVal inCallPlace As String, _
                                        ByVal inUpdateDate As Date, _
                                        ByVal inNowdate As Date, _
                                        ByVal inAccount As String) As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} START inVisitSequence:{2} inCallPlace:{3} inUpdateDate:{4} inNowDate:{5} inAccount:{6}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inVisitSequence, inCallPlace, inUpdateDate, inNowdate, inAccount))

            Using query As New DBUpdateQuery("SC3140103_022")

                'SQL文格納
                Dim sql As New StringBuilder

                With sql

                    .AppendLine("UPDATE /* SC3140103_022 */ ")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT ")
                    .AppendLine("   SET CALLPLACE = :CALLPLACE ")            '呼出ステータス
                    .AppendLine("     , UPDATEDATE = :NOWDATE ")             '更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT ")    '更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID ")              '更新機能ID
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ ")              '来店実績連
                    .AppendLine("   AND UPDATEDATE = :UPDATEDATE ")
                    .AppendLine("   AND CALLSTATUS = :NOTCALL ")
                    .AppendLine("   AND SACODE = :UPDATEACCOUNT ")

                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数

                '呼出場所
                query.AddParameterWithTypeValue("CALLPLACE", OracleDbType.NVarchar2, inCallPlace)
                '現在日時
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowdate)
                '更新アカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)
                '更新機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationId)
                '更新日(排他用)
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpdateDate)
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSequence)
                '呼出ステータス("0"：未呼出)
                query.AddParameterWithTypeValue("NOTCALL", OracleDbType.NVarchar2, NonCall)

                '実行
                Dim updateCount As Integer = query.Execute()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END RETURN:{2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , updateCount.ToString(CultureInfo.CurrentCulture)))

                Return updateCount

            End Using

        End Function

#End Region

#Region "顧客写真情報取得"

        ''' <summary>
        ''' 顧客写真情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="customerId">顧客コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCustomerPhotoData(ByVal dealerCode As String, _
                                             ByVal branchCode As String, _
                                             ByVal customerId As String) _
                                             As SC3140103DataSet.SC3140103VisitPhotoInfoDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, CustomerId = {5}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , dealerCode _
                      , branchCode _
                      , customerId))

            Dim dt As SC3140103DataSet.SC3140103VisitPhotoInfoDataTable

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103VisitPhotoInfoDataTable)("SC3140103_012")

                Dim sql As New StringBuilder

                With sql

                    .AppendLine(" SELECT  /* SC3140103_012 */ ")
                    .AppendLine("         T1.CST_ID AS ORIGINALID ")
                    .AppendLine("        ,TRIM(T2.IMG_FILE_SMALL) AS IMAGEFILE_S ")
                    .AppendLine("   FROM  TB_M_CUSTOMER T1 ")
                    .AppendLine("        ,TB_M_CUSTOMER_DLR T2 ")
                    .AppendLine("  WHERE  T1.CST_ID = T2.CST_ID ")
                    .AppendLine("    AND  T1.DMS_CST_CD = :DMS_CST_CD ")
                    .AppendLine("    AND  T2.DLR_CD = :DLRCD ")

                End With

                'SQL格納
                query.CommandText = sql.ToString()

                'パラメータ設定

                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                '基幹顧客ID
                query.AddParameterWithTypeValue("DMS_CST_CD", OracleDbType.NVarchar2, customerId)


                '検索結果返却
                dt = query.GetData()

            End Using

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} OUT:COUNT = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_END _
                      , dt.Rows.Count))

            Return dt

        End Function

#End Region

#Region "顧客付替え前確認処理"

        ''' <summary>
        ''' 付替え確認用(付替え元)サービス来店管理情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="visitNumber">来店実績連番</param>
        '''  <param name="inUpdateDate">更新日時(排他用)</param>
        ''' <returns>付替え確認用サービス来店管理情報取得情報</returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' </History>
        Public Function GetServiseVisitManagementForChangeDate(ByVal dealerCode As String, _
                                                               ByVal branchCode As String, _
                                                               ByVal visitNumber As Long, _
                                                               ByVal inUpdateDate As Date) As SC3140103DataSet.SC3140103ChangesServiceVisitManagementDataTable

            Dim dt As SC3140103DataSet.SC3140103ChangesServiceVisitManagementDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, visitNumber = {5}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , dealerCode _
                      , branchCode _
                      , visitNumber))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103ChangesServiceVisitManagementDataTable)("SC3140103_013")
                Dim sql As New StringBuilder

                With sql

                    .AppendLine(" SELECT /* SC3140103_013 */ ")
                    .AppendLine("        NVL(FREZID, -1) AS FREZID ")
                    .AppendLine("      , CUSTSEGMENT ")
                    .AppendLine("	   , ASSIGNSTATUS ")
                    .AppendLine("      , SACODE ")
                    .AppendLine("      , ORDERNO ")
                    .AppendLine("      , ASSIGNTIMESTAMP ")
                    .AppendLine("   FROM TBL_SERVICE_VISIT_MANAGEMENT ")
                    .AppendLine("  WHERE DLRCD = :DLRCD ")
                    .AppendLine("    AND STRCD = :STRCD ")
                    .AppendLine("    AND VISITSEQ = :VISITSEQ ")
                    .AppendLine("    AND UPDATEDATE = :UPDATEDATE ")

                End With

                'SQL格納
                query.CommandText = sql.ToString()

                'バインド変数

                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                '店舗コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, visitNumber)
                '更新日時(排他用)
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpdateDate)

                '検索結果返却
                dt = query.GetData()

            End Using

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} OUT:COUNT = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_END _
                      , dt.Rows.Count))

            Return dt

        End Function

        ''' <summary>
        ''' 付替え先予約取得処理
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="resisterNumber">車両登録No</param>
        ''' <param name="vinNumber">VIN</param>
        ''' <param name="stockDate">入庫予定日</param> 
        ''' <param name="inDmsId">基幹顧客コード</param> 
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ''' </History>
        Public Function SearchStallReserveInfoChangeData(ByVal dealerCode As String, _
                                                         ByVal branchCode As String, _
                                                         ByVal resisterNumber As String, _
                                                         ByVal vinNumber As String, _
                                                         ByVal stockDate As String, _
                                                         ByVal inDmsId As String) _
                                                         As SC3140103DataSet.SC3140103SearchChangesReserveDataTable

            Dim dt As SC3140103DataSet.SC3140103SearchChangesReserveDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, resistrationNumber = {5}, vinNumber = {6}, stockDate = {7}, inDmsId = {8}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , dealerCode _
                      , branchCode _
                      , resisterNumber _
                      , vinNumber _
                      , stockDate _
                      , inDmsId))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103SearchChangesReserveDataTable)("SC3140103_015")

                Dim sql As New StringBuilder

                With sql

                    .AppendLine("   SELECT  /* SC3140103_015 */ ")
                    .AppendLine("           T2.SVCIN_ID AS RESERVEID ")
                    .AppendLine("          ,MAX(NVL(T2.CST_ID, 0)) AS CST_ID ")
                    .AppendLine("          ,MAX(NVL(T2.VCL_ID, 0)) AS VCL_ID ")
                    .AppendLine("          ,MAX(TRIM(T2.RO_NUM)) AS ORDERNO ")
                    .AppendLine("          ,MAX(TRIM(T2.PIC_SA_STF_CD)) AS ACCOUNT_PLAN ")
                    .AppendLine("          ,TO_CHAR(MIN(DECODE(T2.SCHE_SVCIN_DATETIME, :MINDATE, T4.SCHE_START_DATETIME, T2.SCHE_SVCIN_DATETIME)), 'YYYYMMDDHH24MI') AS REZ_PICK_DATE ")
                    .AppendLine("     FROM          ")
                    .AppendLine("           (SELECT ")
                    .AppendLine("                    V1.VCL_ID ")
                    .AppendLine("                   ,C2.CST_ID ")
                    .AppendLine("              FROM  TB_M_VEHICLE V1 ")
                    .AppendLine("                   ,TB_M_VEHICLE_DLR V2 ")
                    .AppendLine("                   ,TB_M_CUSTOMER_VCL C1 ")
                    .AppendLine("                   ,TB_M_CUSTOMER C2 ")
                    .AppendLine("             WHERE  V1.VCL_ID = V2.VCL_ID ")
                    .AppendLine("               AND  V2.VCL_ID = C1.VCL_ID ")
                    .AppendLine("               AND  C1.CST_ID = C2.CST_ID ")
                    .AppendLine("               AND  (V1.VCL_VIN = :VIN OR V2.REG_NUM = :VCLREGNO) ")
                    .AppendLine("               AND  (V1.VCL_VIN = :VIN OR V1.VCL_VIN = N' ')  ")
                    .AppendLine("               AND  (V2.REG_NUM = :VCLREGNO OR V2.REG_NUM = N' ') ")
                    .AppendLine("               AND  V2.DLR_CD = :DLRCD ")
                    .AppendLine("               AND  C1.DLR_CD = :DLRCD ")
                    .AppendLine("               AND  C1.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")

                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                    .AppendLine("               AND  C1.CST_VCL_TYPE <> :CST_VCL_TYPE_4 ")
                    '.AppendLine("               AND  (C2.DMS_CST_CD = :DMS_CST_CD OR C2.DMS_CST_CD = N' ') ")


                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                    .AppendLine("            ) T1 ")
                    .AppendLine("           ,TB_T_SERVICEIN T2 ")
                    .AppendLine("           ,TB_T_JOB_DTL T3 ")
                    .AppendLine("           ,TB_T_STALL_USE T4 ")
                    .AppendLine("    WHERE  T1.VCL_ID = T2.VCL_ID ")
                    .AppendLine("      AND  T1.CST_ID = T2.CST_ID ")
                    .AppendLine("      AND  T2.SVCIN_ID = T3.SVCIN_ID ")
                    .AppendLine("      AND  T3.JOB_DTL_ID = T4.JOB_DTL_ID ")
                    .AppendLine("      AND  T2.DLR_CD = :DLRCD ")
                    .AppendLine("      AND  T2.BRN_CD = :STRCD ")
                    .AppendLine("      AND  NOT EXISTS (SELECT 1 ")
                    .AppendLine("                         FROM TB_T_SERVICEIN D1 ")
                    .AppendLine("                        WHERE D1.SVCIN_ID = T2.SVCIN_ID ")
                    .AppendLine("                          AND D1.SVC_STATUS = :STATUS_CANCEL) ")
                    .AppendLine("      AND  T3.CANCEL_FLG = :CANCEL_FLG ")
                    .AppendLine("      AND  T4.SCHE_START_DATETIME >= TO_DATE(:TODAY || '000000', 'YYYYMMDDHH24MISS') ")
                    .AppendLine(" GROUP BY  T2.SVCIN_ID ")
                    .AppendLine("   HAVING  MIN(DECODE(T2.SCHE_SVCIN_DATETIME, :MINDATE, T4.SCHE_START_DATETIME, T2.SCHE_SVCIN_DATETIME)) BETWEEN  TO_DATE(:TODAY || '000000', 'YYYYMMDDHH24MISS') AND  TO_DATE(:TODAY || '235959', 'YYYYMMDDHH24MISS') ")

                End With


                'パラメータ設定

                '日付省略値("1900/01/01 00:00:00")
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))

                '車両登録番号
                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, resisterNumber)

                'VIN
                query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vinNumber)

                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)

                '店舗コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)

                'サービスステータス("02"：キャンセル)
                query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)

                'キャンセルフラグ("0"：キャンセル)
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)

                'オーナーチェンジフラグ(0：未設定)
                query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, NoOwnerChange)

                '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                query.AddParameterWithTypeValue("CST_VCL_TYPE_4", OracleDbType.NVarchar2, Insurance)

                '基幹顧客ID
                'query.AddParameterWithTypeValue("DMS_CST_CD", OracleDbType.NVarchar2, inDmsId)

                '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                '条件
                '日付
                query.AddParameterWithTypeValue("TODAY", OracleDbType.NVarchar2, stockDate)

                'SQL格納
                query.CommandText = sql.ToString()

                '検索結果返却
                dt = query.GetData()

            End Using


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} OUT:COUNT = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_END _
                      , dt.Rows.Count))

            Return dt

        End Function

        ''' <summary>
        ''' 付替え先サービス来店管理情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="resisterNumber">車両登録No</param>
        ''' <param name="vinNumber">VIN</param>
        ''' <param name="visitDate">来店日</param> 
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' </History>
        Public Function SearchServiceVisitManagementChangeData(ByVal dealerCode As String, _
                                                               ByVal branchCode As String, _
                                                               ByVal resisterNumber As String, _
                                                               ByVal vinNumber As String, _
                                                               ByVal visitDate As String) _
                                                               As SC3140103DataSet.SC3140103SearchChangesVisitDataTable

            Dim dt As SC3140103DataSet.SC3140103SearchChangesVisitDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, resistrationNumber = {5}, vinNumber = {6}, visitDate = {7}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , dealerCode _
                      , branchCode _
                      , resisterNumber _
                      , vinNumber _
                      , visitDate))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103SearchChangesVisitDataTable)("SC3140103_016")

                Dim sql As New StringBuilder

                With sql

                    .AppendLine(" SELECT /* SC3140103_016 */  ")
                    .AppendLine("        T1.VISITSEQ  ")
                    .AppendLine("      , NVL(T1.FREZID, -1) AS FREZID  ")
                    .AppendLine("      , T1.CUSTSEGMENT  ")
                    .AppendLine("      , NVL(T1.CUSTID, 0) AS CST_ID ")
                    .AppendLine("      , NVL(T1.VCL_ID, 0) AS VCL_ID ")
                    .AppendLine(" 	   , T1.ASSIGNSTATUS  ")
                    .AppendLine("      , T1.SACODE  ")
                    .AppendLine("      , T1.ORDERNO  ")
                    .AppendLine("      , T1.VISITTIMESTAMP  ")
                    .AppendLine("      , T2.PIC_SA_STF_CD AS ACCOUNT_PLAN ")
                    .AppendLine("   FROM TBL_SERVICE_VISIT_MANAGEMENT T1  ")
                    .AppendLine("      , TB_T_SERVICEIN T2  ")
                    .AppendLine("  WHERE T1.DLRCD = T2.DLR_CD (+)  ")
                    .AppendLine("    AND T1.STRCD = T2.BRN_CD (+)  ")
                    .AppendLine("    AND T1.FREZID = T2.SVCIN_ID (+)  ")
                    .AppendLine("    AND T1.DLRCD = :DLRCD  ")
                    .AppendLine("    AND T1.STRCD = :STRCD  ")
                    .AppendLine("    AND T1.VISITTIMESTAMP BETWEEN TO_DATE(:TODAY||'0000','YYYYMMDDHH24MI')  ")
                    .AppendLine("                              AND TO_DATE(:TODAY||'2359','YYYYMMDDHH24MI')  ")
                    .AppendLine("    AND(T1.VCLREGNO = :VCLREGNO OR T1.VIN = :VIN)  ")
                    .AppendLine("    AND(T1.VCLREGNO = :VCLREGNO OR NVL(TRIM(T1.VCLREGNO),' ') = ' ')  ")
                    .AppendLine("    AND(T1.VIN = :VIN OR NVL(TRIM(T1.VIN),' ') = ' ')  ")
                    .AppendLine("    AND T1.ASSIGNSTATUS <> :ASSIGNSTATUS_4 ")

                End With


                'パラメータ設定

                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                '店舗コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                '条件
                '日付
                query.AddParameterWithTypeValue("TODAY", OracleDbType.NVarchar2, visitDate)
                '車両登録番号
                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, resisterNumber)
                'VIN
                query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vinNumber)
                '振当てステータス「4：退店」
                query.AddParameterWithTypeValue("ASSIGNSTATUS_4", OracleDbType.NVarchar2, DealerOut)

                'SQL格納
                query.CommandText = sql.ToString()

                '検索結果返却
                dt = query.GetData()

            End Using

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} OUT:COUNT = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_END _
                      , dt.Rows.Count))

            Return dt

        End Function

        ''' <summary>
        ''' 付替え先車両情報検索
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="resisterNumber">車両登録番号</param>
        ''' <param name="vinNumber">VIN</param>
        ''' <param name="inDmsId">基幹顧客コード</param>
        ''' <returns>付替え先情報</returns>
        ''' <remarks></remarks>
        Public Function GetDBAfterVehicleInfo(ByVal dealerCode As String, _
                                              ByVal resisterNumber As String, _
                                              ByVal vinNumber As String, _
                                              ByVal inDmsId As String) As SC3140103DataSet.SC3140103AfterVehicleInfoDataTable

            Dim dt As SC3140103DataSet.SC3140103AfterVehicleInfoDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:dealerCode = {3}, resistrationNumber = {4}, vinNumber = {5}, vinNumber = {6}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , dealerCode _
                      , resisterNumber _
                      , vinNumber _
                      , inDmsId))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AfterVehicleInfoDataTable)("SC3140103_025")
                Dim sql As New StringBuilder

                With sql

                    .AppendLine("  SELECT /* SC3140103_025 */  ")
                    .AppendLine("          T1.VCL_ID ")
                    .AppendLine("         ,T4.CST_ID ")
                    .AppendLine("    FROM  TB_M_VEHICLE T1 ")
                    .AppendLine("         ,TB_M_VEHICLE_DLR T2 ")
                    .AppendLine("         ,TB_M_CUSTOMER_VCL T3 ")
                    .AppendLine("         ,TB_M_CUSTOMER_DLR T4 ")
                    .AppendLine("         ,TB_M_CUSTOMER T5 ")
                    .AppendLine("   WHERE  T1.VCL_ID = T2.VCL_ID ")
                    .AppendLine("     AND  T1.VCL_ID = T3.VCL_ID ")
                    .AppendLine("     AND  T3.CST_ID = T4.CST_ID ")
                    .AppendLine("     AND  T4.CST_ID = T5.CST_ID  ")
                    .AppendLine("     AND  (T1.VCL_VIN = :VIN OR T2.REG_NUM = :VCLREGNO) ")
                    .AppendLine("     AND  (T1.VCL_VIN = :VIN OR T1.VCL_VIN = N' ')  ")
                    .AppendLine("     AND  (T2.REG_NUM = :VCLREGNO OR T2.REG_NUM = N' ') ")
                    .AppendLine("     AND  T2.DLR_CD = :DLRCD ")
                    .AppendLine("     AND  T3.DLR_CD = :DLRCD ")
                    .AppendLine("     AND  T3.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")
                    .AppendLine("     AND  T4.DLR_CD = :DLRCD ")
                    .AppendLine("     AND  (T5.DMS_CST_CD = :DMS_CST_CD OR T5.DMS_CST_CD = N' ')  ")
                    .AppendLine("ORDER BY  T2.DMS_TAKEIN_DATETIME DESC ")
                    .AppendLine("         ,T4.CST_TYPE ASC ")

                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 STRAT
                    .AppendLine("         ,T3.CST_VCL_TYPE ASC ")
                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                    .AppendLine("         ,T2.REG_NUM DESC ")
                    .AppendLine("         ,T1.VCL_VIN DESC ")
                    .AppendLine("         ,T1.VCL_ID DESC ")

                End With


                'パラメータ設定

                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)

                '車両登録番号
                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, resisterNumber)

                'VIN
                query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vinNumber)

                'オーナーチェンジフラグ("0"：未設定)
                query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, NoOwnerChange)

                '基幹顧客ID
                query.AddParameterWithTypeValue("DMS_CST_CD", OracleDbType.NVarchar2, inDmsId)

                'SQL格納
                query.CommandText = sql.ToString()

                '検索結果返却
                dt = query.GetData()


            End Using


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} OUT:COUNT = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_END _
                      , dt.Rows.Count))

            Return dt

        End Function

#End Region

#Region "顧客付替え登録処理"

        ''' <summary>
        ''' 付替え元予約情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' </History>
        Public Function GetStallReserveInfoForChangeData(ByVal dealerCode As String, _
                                                         ByVal branchCode As String, _
                                                         ByVal reserveId As Decimal) _
                                                         As SC3140103DataSet.SC3140103ChangesStallReserveDataTable

            Dim dt As SC3140103DataSet.SC3140103ChangesStallReserveDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:dealerCode = {3}, branchCode = {4}, reserveId = {5}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , dealerCode _
                      , branchCode _
                      , reserveId))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103ChangesStallReserveDataTable)("SC3140103_014")

                Dim sql As New StringBuilder

                With sql

                    .AppendLine("  SELECT  /* SC3140103_014 */ ")
                    .AppendLine("          TRIM(T1.PIC_SA_STF_CD) AS ACCOUNT_PLAN ")
                    .AppendLine("         ,TRIM(T1.RO_NUM) AS ORDERNO ")
                    .AppendLine("         ,TO_CHAR(DECODE(T1.SCHE_SVCIN_DATETIME, :MINDATE, T3.SCHE_START_DATETIME, T1.SCHE_SVCIN_DATETIME), 'YYYYMMDDHH24MI') AS STOCKTIME ")
                    .AppendLine("         ,DECODE(T3.SCHE_START_DATETIME, :MINDATE, TO_DATE(NULL), T3.SCHE_START_DATETIME) AS STARTTIME ")
                    .AppendLine("    FROM  TB_T_SERVICEIN T1 ")
                    .AppendLine("         ,TB_T_JOB_DTL T2 ")
                    .AppendLine("         ,TB_T_STALL_USE T3 ")
                    .AppendLine("   WHERE  T1.SVCIN_ID = T2.SVCIN_ID ")
                    .AppendLine("     AND  T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                    .AppendLine("     AND  T1.DLR_CD = :DLRCD ")
                    .AppendLine("     AND  T1.BRN_CD = :STRCD ")
                    .AppendLine("     AND  T1.SVCIN_ID = :REZID ")
                    .AppendLine("     AND  NOT EXISTS (SELECT 1 ")
                    .AppendLine("                        FROM TB_T_SERVICEIN D1 ")
                    .AppendLine("                       WHERE D1.SVCIN_ID = T1.SVCIN_ID ")
                    .AppendLine("                         AND D1.SVC_STATUS = :STATUS_CANCEL) ")
                    .AppendLine("     AND  T2.CANCEL_FLG = :CANCEL_FLG ")

                End With


                'パラメータ設定

                '日付省略値(1900/01/01 00:00:00)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                '店舗コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                'サービス入庫ID
                query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, reserveId)
                'サービスステータス("02"：キャンセル)
                query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
                'キャンセルフラグ("0"：キャンセル)
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)

                'SQL格納
                query.CommandText = sql.ToString()

                '検索結果返却
                dt = query.GetData()

            End Using


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} OUT:COUNT = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_END _
                      , dt.Rows.Count))

            Return dt

        End Function

        ''' <summary>
        ''' 顧客付替えサービス来店管理テーブル顧客情報更新処理
        ''' </summary>
        ''' <param name="visitNumber">来店実績連番</param>
        ''' <param name="customerType">顧客区分</param>
        ''' <param name="customerCode">顧客コード</param>
        ''' <param name="basicCustomerId">基幹顧客ID</param>
        ''' <param name="customerName">氏名</param>
        ''' <param name="phone">電話番号</param>
        ''' <param name="mobile">携帯番号</param>
        ''' <param name="vipMark">VIPマーク</param>
        ''' <param name="resisterNumber">車両登録No.</param>
        ''' <param name="vin">VIN</param>
        ''' <param name="modelCode">モデルコード</param>
        ''' <param name="reserveId">予約ID</param>
        ''' <param name="orderNumber">整備受注No.</param>
        ''' <param name="defaultSACode">SAコード</param>
        ''' <param name="assignSACode">振当SA</param>
        ''' <param name="updateDate">更新日</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <param name="beforeAssignStatus">付替え元振当ステータス</param>
        ''' <param name="beforeAssignDate">振当日時</param>
        ''' <param name="afterVehicleId">車両ID</param>
        ''' <param name="inAssignFlg">振当て登録フラグ(True：振当て処理を行う　False:振当て処理を行わない)</param>
        ''' <param name="inVisitName">来店者氏名(予約の顧客氏名)</param>
        ''' <param name="inVisitTel">来店者電話番号(予約の電話番号または携帯番号)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ''' </History>
        Public Function SetVisitCustomer(ByVal visitNumber As Long, _
                                         ByVal customerType As String, _
                                         ByVal customerCode As String, _
                                         ByVal basicCustomerId As String, _
                                         ByVal customerName As String, _
                                         ByVal phone As String, _
                                         ByVal mobile As String, _
                                         ByVal vipMark As String, _
                                         ByVal resisterNumber As String, _
                                         ByVal vin As String, _
                                         ByVal modelCode As String, _
                                         ByVal reserveId As Decimal, _
                                         ByVal orderNumber As String, _
                                         ByVal defaultSACode As String, _
                                         ByVal assignSACode As String, _
                                         ByVal updateDate As Date, _
                                         ByVal updateAccount As String, _
                                         ByVal beforeAssignStatus As String, _
                                         ByVal beforeAssignDate As Date, _
                                         ByVal afterVehicleId As Decimal, _
                                         ByVal inAssignFlg As Boolean, _
                                         ByVal inVisitName As String, _
                                         ByVal inVisitTel As String) As Long

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:visitNumber = {3}, customerType = {4}" +
                        ", customerCode = {5}, dmsId = {6}, customerName = {7}, " + _
                        "phone = {8}, mobile = {9}, vipMark = {10}, registerNumber = {11}," +
                        " vin = {12}, modelCode = {13}, reserveId = {14}, " + _
                        "orderNmber = {15}, defaultSaCode = {16}, assignSaCode = {17}," +
                        " updateDate = {18}, updateAccount = {19}, beforeAssignStatus = {20} , beforeAssignDate = {21} afterVehicleId = {22}  inAssignFlg = {23} " +
                        " updateDate = {24}, updateAccount = {25}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , visitNumber _
                      , customerType _
                      , customerCode _
                      , basicCustomerId _
                      , customerName _
                      , phone _
                      , mobile _
                      , vipMark _
                      , resisterNumber _
                      , vin _
                      , modelCode _
                      , reserveId _
                      , orderNumber _
                      , defaultSACode _
                      , assignSACode _
                      , updateDate _
                      , updateAccount _
                      , beforeAssignStatus _
                      , beforeAssignDate _
                      , afterVehicleId _
                      , inAssignFlg _
                      , inVisitName _
                      , inVisitTel))

            '更新件数
            Dim count As Long = 0

            Using query As New DBUpdateQuery("SC3140103_017")

                Dim sql As New StringBuilder

                With sql

                    .AppendLine(" UPDATE /* SC3140103_017 */ ")
                    .AppendLine("        TBL_SERVICE_VISIT_MANAGEMENT ")
                    .AppendLine("    SET VCLREGNO = :VCLREGNO ")
                    .AppendLine("      , CUSTSEGMENT = :CUSTSEGMENT ")
                    .AppendLine("      , CUSTID = :CUSTID ")
                    .AppendLine("      , DMSID = :DMSID ")
                    .AppendLine("      , VCL_ID = :VCL_ID ")
                    .AppendLine("      , VIN = :VIN ")
                    .AppendLine("      , MODELCODE = :MODELCODE ")
                    .AppendLine("      , NAME = :NAME ")
                    .AppendLine("      , TELNO = :TELNO ")
                    .AppendLine("      , MOBILE = :MOBILE ")
                    .AppendLine("      , REZID = :REZID ")
                    '.AppendLine("      , ORDERNO = :ORDERNO ")
                    .AppendLine("      , DEFAULTSACODE = :DEFAULTSACODE ")


                    '振当日時
                    '更新する来店管理情報が既に振当てされているか確認
                    '振当てされている場合は更新しない
                    '振当てされいない場合は振当て処理をして現在日時を振当て日時とする
                    If inAssignFlg _
                        AndAlso Not AssignFinish.Equals(beforeAssignStatus) Then
                        '振当てされていない場合

                        '振当SA
                        .AppendLine("      , SACODE = :SACODE ")
                        '振当ステータス
                        .AppendLine("      , ASSIGNSTATUS = :ASSIGNSTATUS ")
                        '振当日時
                        .AppendLine("      , ASSIGNTIMESTAMP = :ASSIGNTIMESTAMP ")


                        '振当SA
                        query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, assignSACode)
                        '振当ステータス("2"：SA振当済)
                        query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, AssignFinish)
                        '振当日時(現在日時)
                        query.AddParameterWithTypeValue("ASSIGNTIMESTAMP", OracleDbType.Date, updateDate)

                    End If


                    .AppendLine("      , FREZID = :REZID ")
                    .AppendLine("      , UPDATEDATE = :UPDATEDATE ")
                    .AppendLine("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                    .AppendLine("      , UPDATEID = :UPDATEID ")

                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                    '来店者氏名
                    .AppendLine("      , VISITNAME = :VISITNAME ")
                    '来店者電話番号
                    .AppendLine("      , VISITTELNO = :VISITTELNO ")

                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                    .AppendLine("  WHERE VISITSEQ = :VISITSEQ ")

                End With

                'SQL格納
                query.CommandText = sql.ToString()

                'バインド変数

                '車両登録番号
                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, resisterNumber)
                '顧客種別("1"：自社客)
                query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.NVarchar2, customerType)

                'CSTID
                Dim longCustomerCode As Decimal = 0

                '引数の顧客IDがLongに変換できるかチェック
                If Not Decimal.TryParse(customerCode, longCustomerCode) Then
                    '変換できない場合

                    'Nullを設定
                    query.AddParameterWithTypeValue("CUSTID", OracleDbType.Decimal, DBNull.Value)
                Else
                    '変換できた場合

                    '変換値を設定
                    query.AddParameterWithTypeValue("CUSTID", OracleDbType.Decimal, longCustomerCode)
                End If

                '基幹顧客ID
                query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, basicCustomerId)

                '車両ID確認
                If afterVehicleId <= 0 Then
                    '車両IDが存在しない

                    query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, DBNull.Value)
                Else
                    '車両IDが存在する

                    '変換値を設定
                    query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, afterVehicleId)
                End If

                'VIN
                query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, vin)
                'モデルコード
                query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, modelCode)
                '顧客名
                query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, customerName)
                '電話番号
                query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, phone)
                '携帯番号
                query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, mobile)
                '予約ID
                query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, reserveId)
                ''整備受注番号
                'query.AddParameterWithTypeValue("ORDERNO", OracleDbType.NVarchar2, orderNumber)
                'デフォルトSAコード
                query.AddParameterWithTypeValue("DEFAULTSACODE", OracleDbType.NVarchar2, defaultSACode)

                '更新日時
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updateDate)
                '更新アカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateAccount)
                '更新機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationId)

                '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                '来店者氏名
                query.AddParameterWithTypeValue("VISITNAME", OracleDbType.NVarchar2, inVisitName)
                '来店者電話番号
                query.AddParameterWithTypeValue("VISITTELNO", OracleDbType.NVarchar2, inVisitTel)

                '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END



                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, visitNumber)

                '検索結果返却
                count = query.Execute()

            End Using


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} OUT:COUNT = {3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , count))

            Return count

        End Function

        ''' <summary>
        ''' 顧客付替え予約の行ロックバージョン取得処理
        ''' </summary>
        ''' <param name="inRezID">予約ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetDBNewestStallRezInfo(ByVal inRezId As Decimal) _
                                                As SC3140103DataSet.SC3140103NewestStallRezInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} REZID:{2}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inRezId))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103NewestStallRezInfoDataTable)("SC3140103_024")

                Dim sql As New StringBuilder      ' SQL文格納

                With sql

                    .AppendLine(" SELECT /* SC3140103_024 */ ")
                    .AppendLine("        ROW_LOCK_VERSION AS ROW_LOCK_VERSION ")
                    .AppendLine("   FROM ")
                    .AppendLine("        TB_T_SERVICEIN T1 ")
                    .AppendLine("  WHERE ")
                    .AppendLine("        T1.SVCIN_ID = :REZID ")

                End With

                'SQL格納
                query.CommandText = sql.ToString()

                'バインド変数

                '予約ID
                query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, inRezId)

                '実行
                Dim dt As SC3140103DataSet.SC3140103NewestStallRezInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END COUNT = {2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , dt.Count))

                Return dt

            End Using
        End Function

        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        ''' <summary>
        ''' 付替え先来店情報取得
        ''' </summary>
        ''' <param name="inVisitSeq">来店実績連番</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' </History>
        Public Function GetAfterServiceVisitManagementInfo(ByVal inVisitSeq As Long, _
                                                           ByVal inDealerCode As String, _
                                                           ByVal inBranchCode As String) _
                                                           As SC3140103DataSet.SC3140103AfterServiceVisitManagementInfoDataTable

            Dim dt As SC3140103DataSet.SC3140103AfterServiceVisitManagementInfoDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:VISITSEQ = {3}, DLRCD = {4}, BRNCD = {5}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , inVisitSeq _
                      , inDealerCode _
                      , inBranchCode))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AfterServiceVisitManagementInfoDataTable)("SC3140103_039")

                Dim sql As New StringBuilder

                With sql

                    .AppendLine(" SELECT /* SC3140103_039 */ ")
                    .AppendLine("        VISITNAME ")
                    .AppendLine("	   , VISITTELNO ")
                    .AppendLine("   FROM TBL_SERVICE_VISIT_MANAGEMENT ")
                    .AppendLine("  WHERE DLRCD = :DLRCD ")
                    .AppendLine("    AND STRCD = :STRCD ")
                    .AppendLine("    AND VISITSEQ = :VISITSEQ ")

                End With

                'SQL格納
                query.CommandText = sql.ToString()

                'バインド変数

                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                '店舗コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSeq)

                'SQL格納
                query.CommandText = sql.ToString()

                '検索結果返却
                dt = query.GetData()

            End Using


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} OUT:COUNT = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_END _
                      , dt.Rows.Count))

            Return dt

        End Function

        ''' <summary>
        ''' 付替え先予約顧客情報取得
        ''' </summary>
        ''' <param name="inReserveId">予約ID</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' </History>
        Public Function GetAfterlReserveCustomerInfo(ByVal inReserveId As Decimal, _
                                                     ByVal inDealerCode As String, _
                                                     ByVal inBranchCode As String) _
                                                     As SC3140103DataSet.SC3140103AfterlReserveCustomerInfoDataTable

            Dim dt As SC3140103DataSet.SC3140103AfterlReserveCustomerInfoDataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:SVCIN_ID = {3}, DLR_CD = {4}, BRN_CD = {5}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , inReserveId _
                      , inDealerCode _
                      , inBranchCode))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103AfterlReserveCustomerInfoDataTable)("SC3140103_040")

                Dim sql As New StringBuilder

                With sql

                    .AppendLine("  SELECT  /* SC3140103_040 */ ")
                    .AppendLine("          TRIM(T2.CST_NAME) AS CST_NAME ")
                    .AppendLine("         ,NVL(TRIM(T2.CST_PHONE), TRIM(T2.CST_MOBILE)) AS CST_PHONE ")
                    .AppendLine("    FROM  TB_T_SERVICEIN T1 ")
                    .AppendLine("         ,TB_M_CUSTOMER T2 ")
                    .AppendLine("   WHERE  T1.CST_ID = T2.CST_ID ")
                    .AppendLine("     AND  T1.DLR_CD = :DLRCD ")
                    .AppendLine("     AND  T1.BRN_CD = :STRCD ")
                    .AppendLine("     AND  T1.SVCIN_ID = :REZID ")

                End With


                'パラメータ設定

                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                '店舗コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)
                'サービス入庫ID
                query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, inReserveId)

                'SQL格納
                query.CommandText = sql.ToString()

                '検索結果返却
                dt = query.GetData()

            End Using


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} OUT:COUNT = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_END _
                      , dt.Rows.Count))

            Return dt

        End Function

        '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

#End Region

#Region "顧客付替え・顧客解除用サービス来店管理テーブル顧客情報クリア処理"

        ''' <summary>
        ''' 顧客付替え・顧客解除用サービス来店管理テーブル顧客情報クリア処理
        ''' </summary>
        ''' <param name="visitNumber">来店実績連番</param>
        ''' <param name="updateDate">更新日</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <param name="inAssignStatus">振当ステータス</param>
        ''' <param name="inAssignDate">振当て日時</param>
        ''' <param name="inAssignFlg">振当て登録フラグ(True：振当て処理を行う　False:振当て処理を行わない)</param>
        ''' <returns>実行結果</returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ''' </History>
        Public Function VisitCustomerClear(ByVal visitNumber As Long, _
                                           ByVal updateDate As Date, _
                                           ByVal updateAccount As String, _
                                           ByVal inAssignStatus As String, _
                                           ByVal inAssignDate As Date, _
                                           ByVal inAssignFlg As Boolean) As Long

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} IN:VISITNUMBER = {3} UPDATEDATE = {4} UPDATEACCOUNT = {5} ASSIGNSTATUS = {6} ASSIGNTIMESTAMP = {7} ASSIGNFLAG = {8}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_START _
                      , visitNumber, updateDate, updateAccount, inAssignStatus, inAssignDate, inAssignFlg))

            Dim count As Long = 0

            Using query As New DBUpdateQuery("SC3140103_018")
                Dim sql As New StringBuilder

                With sql
                    .AppendLine(" UPDATE /* SC3140103_018 */ ")
                    .AppendLine("        TBL_SERVICE_VISIT_MANAGEMENT ")
                    .AppendLine("    SET VCLREGNO = NULL ")
                    .AppendLine("      , CUSTSEGMENT = :CUSTSEGMENT ")
                    .AppendLine("      , CUSTID = NULL ")
                    .AppendLine("      , DMSID = NULL ")
                    .AppendLine("      , VCL_ID = NULL ")
                    .AppendLine("      , VIN = NULL ")
                    .AppendLine("      , MODELCODE = NULL ")
                    .AppendLine("      , NAME = NULL ")
                    .AppendLine("      , TELNO = NULL ")
                    .AppendLine("      , MOBILE = NULL ")
                    .AppendLine("      , REZID = NULL ")
                    .AppendLine("      , ORDERNO = NULL ")
                    .AppendLine("      , DEFAULTSACODE = NULL ")
                    .AppendLine("      , FREZID = NULL ")

                    '振当て処理フラグチェック
                    If inAssignFlg Then
                        '振当て処理を行う

                        '振当日時
                        '更新する来店管理情報が既に振当てされているか確認
                        '振当てされている場合は更新しない
                        '振当てされいない場合は振当て処理をして現在日時を振当て日時とする
                        If Not AssignFinish.Equals(inAssignStatus) Then
                            '振当てされていない場合

                            .AppendLine("      , SACODE = :SACODE ")
                            .AppendLine("      , ASSIGNTIMESTAMP = :ASSIGNTIMESTAMP ")
                            .AppendLine("      , ASSIGNSTATUS = :ASSIGNSTATUS ")

                            '振当SA
                            query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, updateAccount)
                            '振当日時(現在日時)
                            query.AddParameterWithTypeValue("ASSIGNTIMESTAMP", OracleDbType.Date, updateDate)
                            '振当ステータス("2"：SA振当済)
                            query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, AssignFinish)

                        End If

                    End If

                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                    .AppendLine("      , VISITNAME = NULL ")
                    .AppendLine("      , VISITTELNO = NULL ")

                    '2015/09/01 TMEJ 井上(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                    .AppendLine("      , UPDATEDATE = :UPDATEDATE ")
                    .AppendLine("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                    .AppendLine("      , UPDATEID = :UPDATEID ")
                    .AppendLine("  WHERE VISITSEQ = :VISITSEQ ")
                End With

                query.CommandText = sql.ToString()

                'バインド変数

                '顧客種別("2"：未取引客)
                query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.NVarchar2, CustsegmentNewCustomer)
                '更新日時
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, updateDate)
                '更新者アカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateAccount)
                '更新機能ID
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationId)
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, visitNumber)

                '検索結果返却
                count = query.Execute()

            End Using


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2} OUT:COUNT = {3}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_END _
                      , count))

            Return count

        End Function

#End Region

#Region "通知送信用情報取得"

        ''' <summary>
        ''' 通知送信用情報取得
        ''' </summary>
        ''' <param name="inVisitSeq">来店実績連番</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns>通知送信用情報データセット</returns>
        ''' <remarks></remarks>
        Public Function GetNoticeProcessingInfo(ByVal inVisitSeq As Long _
                                              , ByVal inDealerCode As String _
                                              , ByVal inBranchCode As String) _
                                                As SC3140103DataSet.SC3140103NoticeProcessingInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} VISITSEQ:{2}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inVisitSeq))

            Using query As New DBSelectQuery(Of SC3140103DataSet.SC3140103NoticeProcessingInfoDataTable)("SC3140103_037")

                'SQL文格納
                Dim sql As New StringBuilder

                With sql

                    .AppendLine("		SELECT /* SC3140103_037 */ ")
                    .AppendLine("		       T1.VISITSEQ ")
                    .AppendLine("		      ,TRIM(T1.VCLREGNO) AS VCLREGNO ")
                    .AppendLine("		      ,TRIM(T1.CUSTSEGMENT) AS CUSTSEGMENT ")
                    .AppendLine("		      ,TRIM(T1.DMSID) AS DMSID ")
                    .AppendLine("		      ,TRIM(T1.VIN) AS VIN ")

                    '2015/12/17 TM 浅野 12月号口配信に向けた緊急対応 START
                    .AppendLine("		      ,TRIM(T8.CST_NAME) AS NAME ")
                    '.AppendLine("		      ,TRIM(T1.NAME) AS NAME ")
                    '2015/12/17 TM 浅野 12月号口配信に向けた緊急対応 END

                    .AppendLine("		      ,NVL(T1.FREZID, -1) AS REZID ")
                    .AppendLine("		      ,CASE ")
                    .AppendLine("		            WHEN T5.SCHE_START_DATETIME = :MINDATE THEN :MINVALUE ")
                    .AppendLine("		            ELSE T5.SCHE_START_DATETIME ")
                    .AppendLine("		             END AS SCHE_START_DATETIME ")
                    .AppendLine("		      ,CASE ")
                    .AppendLine("		            WHEN T5.SCHE_END_DATETIME = :MINDATE THEN :MINVALUE ")
                    .AppendLine("		            ELSE T5.SCHE_END_DATETIME ")
                    .AppendLine("		             END AS SCHE_END_DATETIME ")
                    .AppendLine("		      ,NVL(CONCAT(TRIM(T6.UPPER_DISP), TRIM(T6.LOWER_DISP)), NVL(T7.SVC_CLASS_NAME, T7.SVC_CLASS_NAME_ENG)) AS MERCHANDISENAME ")
                    .AppendLine("		      ,TRIM(T9.NAMETITLE_NAME) AS NAMETITLE_NAME ")
                    .AppendLine("		      ,TRIM(T9.POSITION_TYPE) AS POSITION_TYPE ")
                    .AppendLine("		 FROM  TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                    .AppendLine("		      ,TB_T_SERVICEIN T2 ")
                    .AppendLine("		      ,(SELECT MAX(S3.SVCIN_ID) AS SVCIN_ID ")
                    .AppendLine("		              ,MIN(S3.JOB_DTL_ID) AS JOB_DTL_ID ")
                    .AppendLine("		              ,MAX(S4.STALL_USE_ID) AS STALL_USE_ID ")
                    .AppendLine("		          FROM TBL_SERVICE_VISIT_MANAGEMENT S1 ")
                    .AppendLine("		              ,TB_T_SERVICEIN S2 ")
                    .AppendLine("		              ,TB_T_JOB_DTL S3 ")
                    .AppendLine("		              ,TB_T_STALL_USE S4 ")
                    .AppendLine("		         WHERE S1.FREZID = S2.SVCIN_ID ")
                    .AppendLine("		           AND S2.SVCIN_ID = S3.SVCIN_ID ")
                    .AppendLine("		           AND S3.JOB_DTL_ID = S4.JOB_DTL_ID ")
                    .AppendLine("		           AND S1.VISITSEQ = :VISITSEQ ")
                    .AppendLine("		           AND S1.DLRCD = :DLRCD ")
                    .AppendLine("		           AND S1.STRCD = :STRCD ")
                    .AppendLine("		           AND S2.DLR_CD = :DLRCD ")
                    .AppendLine("		           AND S2.BRN_CD = :STRCD ")
                    .AppendLine("		           AND S2.SVC_STATUS <> :STATUS_CANCEL ")
                    .AppendLine("		           AND S3.DLR_CD = :DLRCD ")
                    .AppendLine("		           AND S3.BRN_CD = :STRCD ")
                    .AppendLine("		           AND S3.CANCEL_FLG = :CANCELFLG ")
                    .AppendLine("		           AND S4.DLR_CD = :DLRCD ")
                    .AppendLine("		           AND S4.BRN_CD = :STRCD ")
                    .AppendLine("		      GROUP BY S1.VISITSEQ ")
                    .AppendLine("		       ) T3 ")
                    .AppendLine("		      ,TB_T_JOB_DTL T4 ")
                    .AppendLine("		      ,TB_T_STALL_USE T5 ")
                    .AppendLine("		      ,TB_M_MERCHANDISE T6 ")
                    .AppendLine("		      ,TB_M_SERVICE_CLASS T7 ")
                    .AppendLine("		      ,TB_M_CUSTOMER T8 ")
                    .AppendLine("		      ,TB_M_NAMETITLE T9 ")
                    .AppendLine("		WHERE  T1.FREZID = T2.SVCIN_ID(+) ")
                    .AppendLine("		  AND  T2.SVCIN_ID = T3.SVCIN_ID(+) ")
                    .AppendLine("		  AND  T3.JOB_DTL_ID = T4.JOB_DTL_ID(+) ")
                    .AppendLine("		  AND  T4.JOB_DTL_ID = T5.JOB_DTL_ID(+) ")
                    .AppendLine("		  AND  T4.MERC_ID = T6.MERC_ID(+) ")
                    .AppendLine("		  AND  T4.SVC_CLASS_ID = T7.SVC_CLASS_ID(+) ")
                    .AppendLine("		  AND  T1.CUSTID = T8.CST_ID(+) ")
                    .AppendLine("		  AND  T8.NAMETITLE_CD = T9.NAMETITLE_CD(+) ")
                    .AppendLine("		  AND  T1.VISITSEQ = :VISITSEQ ")
                    .AppendLine("		  AND  T1.DLRCD = :DLRCD ")
                    .AppendLine("		  AND  T1.STRCD = :STRCD ")
                    .AppendLine("		  AND  T2.DLR_CD(+) = :DLRCD ")
                    .AppendLine("		  AND  T2.BRN_CD(+) = :STRCD ")
                    .AppendLine("		  AND  T2.SVC_STATUS(+) <> :STATUS_CANCEL ")
                    .AppendLine("		  AND  T4.DLR_CD(+) = :DLRCD ")
                    .AppendLine("		  AND  T4.BRN_CD(+) = :STRCD ")
                    .AppendLine("		  AND  T4.CANCEL_FLG(+) = :CANCELFLG ")
                    .AppendLine("		  AND  T5.DLR_CD(+) = :DLRCD ")
                    .AppendLine("		  AND  T5.BRN_CD(+) = :STRCD ")
                    .AppendLine("		  AND  T9.INUSE_FLG(+) = :INUSE_FLG ")

                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数

                '日付省略値
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
                '日付最小値
                query.AddParameterWithTypeValue("MINVALUE", OracleDbType.Date, Date.MinValue)
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSeq)
                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                '店舗コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)
                'サービスステータス
                query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel)
                'キャンセルフラグ
                query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.NVarchar2, CancelFlagEffective)
                '使用中フラグ("1"：使用中)
                query.AddParameterWithTypeValue("INUSE_FLG", OracleDbType.NVarchar2, InUse)

                '実行
                Dim dt As SC3140103DataSet.SC3140103NoticeProcessingInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END COUNT = {2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , dt.Count))

                Return dt

            End Using

        End Function

#End Region

    End Class

End Namespace

Partial Class SC3140103DataSet
End Class
