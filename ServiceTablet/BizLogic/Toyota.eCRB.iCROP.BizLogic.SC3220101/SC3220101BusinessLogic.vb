'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3220101BusinessLogic.vb
'─────────────────────────────────────
'機能： SMメインメニュービジネスロジック
'補足： 
'作成： 2012/05/28 日比野 
'更新： 2012/09/11 TMEJ 日比野 チップに車両登録Noが表示されない不具合対応
'更新： 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応
'更新： 2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応
'更新： 2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 
'更新： 2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新： 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
'更新： 2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新： 2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新： 
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core

Imports Toyota.eCRB.iCROP.DataAccess.SC3220101
Imports Toyota.eCRB.iCROP.DataAccess.SC3220101.SC3220101DataSet

'2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801002
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801002

'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801003
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003

'2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.GetUserList.Api.DataAccess

'2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.BizLogic.IC3800703
'Imports Toyota.eCRB.DMSLinkage.CustomerInfo.DataAccess.IC3800703

'2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.DataAccess
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.BizLogic
Imports Toyota.eCRB.SMBLinkage.GetServiceLT.Api.DataAccess.IC3810701DataSet

'2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

''2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 START
'Imports Toyota.eCRB.DMSLinkage.AddRepair.BizLogic.IC3800804
'Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800804
'Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800804.IC3800804DataSet
''2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応

Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

'2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


Public Class SC3220101BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ''' <summary>
    ''' SAのオンラインステータス:オンライン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SA_ONLINESTATE_ONLINE As String = "1"
    ''' <summary>
    ''' SAのオンラインステータス:退席中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SA_ONLINESTATE_AWAY As String = "2"
    ''' <summary>
    ''' SAのオンラインステータス:オフライン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SA_ONLINESTATE_OFFLINE As String = "3"
    ''' <summary>
    ''' チップの工程ステータス:受付中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_PROGRESSSTATE_RECEPTION As Integer = 1
    ''' <summary>
    ''' チップの工程ステータス:追加作業
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_PROGRESSSTATE_ADDITION_WORKING As Integer = 2
    ''' <summary>
    ''' チップの工程ステータス:洗車・納車準備
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_PROGRESSSTATE_PREPARATION_DELIVERY As Integer = 3
    ''' <summary>
    ''' チップの工程ステータス:納車作業
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_PROGRESSSTATE_DELIVERY As Integer = 4
    ''' <summary>
    ''' チップの工程ステータス:作業中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_PROGRESSSTATE_WORKING As Integer = 5
    ''' <summary>
    ''' チップの工程ステータス:来店(受付待ち)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_PROGRESSSTATE_RECEPTION_WAIT As Integer = 6
    ''' <summary>
    ''' チップの追加作業の有無:なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_ADD_APPROVAL_EXISTENCE_FALSE As String = "0"
    ''' <summary>
    ''' チップの追加作業の有無:あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_ADD_APPROVAL_EXISTENCE_TRUE As String = "1"
    ''' <summary>
    ''' チップのR/O有無:なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_RO_EXISTENCE_FALSE As String = "0"
    ''' <summary>
    ''' チップのR/Oの有無:あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_RO_EXISTENCE_TRUE As String = "1"
    ''' <summary>
    ''' チップの洗車状況:洗車なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_WASHFLG_FALSE As String = "0"
    ''' <summary>
    ''' チップの洗車状況:洗車あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_WASHFLG_TRUE As String = "1"
    ''' <summary>
    ''' チップの洗車状況:洗車なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_WASHING_NONE As String = "0"

    ''' <summary>
    ''' チップの洗車状況:洗車未完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_WASHING_IMPERFECT As String = "1"

    ''' <summary>
    ''' チップの洗車有無:洗車完了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CHIP_WASHING_FINISH As String = "2"


    ' ストール実績：実績ステータス
    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
    'Private Const C_STALLPROSESS_NONE As String = "00"          ' 未入庫
    'Private Const C_STALLPROSESS_CAR_IN As String = "10"        ' 入庫
    'Private Const C_STALLPROSESS_WORKING As String = "20"       ' 作業中
    'Private Const C_STALLPROSESS_ITEM_NONE As String = "30"     ' 部品欠品
    'Private Const C_STALLPROSESS_CUST_WAIT As String = "31"     ' お客様連絡待ち
    'Private Const C_STALLPROSESS_STALL_WAIT As String = "38"    ' ストール待機
    'Private Const C_STALLPROSESS_ETC As String = "39"           ' その他
    'Private Const C_STALLPROSESS_WASH_WAIT As String = "40"     ' 洗車待ち
    'Private Const C_STALLPROSESS_WASH_DOING As String = "41"    ' 洗車中
    'Private Const C_STALLPROSESS_INSP_WAIT As String = "42"     ' 検査待ち
    'Private Const C_STALLPROSESS_INSP_DOING As String = "43"    ' 検査中
    'Private Const C_STALLPROSESS_INSP_NG As String = "44"       ' 検査不合格
    'Private Const C_STALLPROSESS_CARRY_WAIT As String = "50"    ' 預かり中
    'Private Const C_STALLPROSESS_DELI_WAIT As String = "60"     ' 納車待ち
    'Private Const C_STALLPROSESS_PFINISH As String = "97"       ' 関連チップの前工程作業終了
    'Private Const C_STALLPROSESS_MFINISH As String = "98"       ' MidFinish
    'Private Const C_STALLPROSESS_COMPLETE As String = "99"      ' 完了
    Private Const C_STALLPROSESS_CARRY_WAIT As String = "11"    ' 預かり中
    Private Const C_STALLPROSESS_DELI_WAIT As String = "12"     ' 納車待ち
    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

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
    ''' 実績あり
    ''' </summary>
    Private Const RESULT_TYPE_TRUE As String = "1"
    ''' <summary>
    ''' 実績なし
    ''' </summary>
    Private Const RESULT_TYPE_FALSE As String = "0"

    Private Const MinReserveId As Long = -1           ' 最小予約ID

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' ROステータス（"80"：納車準備）
    ''' </summary>
    Private Const StatusDeliveryWait As String = "80"

    ''' <summary>
    ''' ROステータス（"85"：納車作業）
    ''' </summary>
    Private Const StatusDeliveryWork As String = "85"

    ''' <summary>
    ''' 作業終了ステータス（"0"：作業中）
    ''' </summary>
    Private Const WorkEndType0 As String = "0"

    ''' <summary>
    ''' 作業終了ステータス（"1"：作業終了）
    ''' </summary>
    Private Const WorkEndType1 As String = "1"

    ''' <summary>
    ''' 洗車終了ステータステータス（"0"：洗車中）
    ''' </summary>
    Private Const CarWashEndType0 As String = "0"

    ''' <summary>
    ''' 洗車終了ステータス（"1"：洗車終了）
    ''' </summary>
    Private Const CarWashEndType1 As String = "1"

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

#End Region

#Region "メンバ変数"


#End Region

#Region "チップ情報取得"

    ' 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 START
    ''' <summary>
    ''' 来店チップ情報取得
    ''' </summary>
    ''' <param name="dtStanderdLt">標準時間情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    ''' </History>
    Public Function GetVisitChip(ByVal dtStanderdLT As StandardLTListDataTable) _
                                 As SC3220101DataSet.SC3220101ChipInfoDataTable

        'Public Function GetVisitChip() As SC3220101DataSet.SC3220101ChipInfoDataTable                                        
        ' 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 END

        Logger.Info("GetVisitChip Start")

        'ログイン情報取得
        Dim userContext As StaffContext = StaffContext.Current

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        '現在日時
        Dim presentTime As Date = DateTimeFunc.Now(userContext.DlrCD)

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        Dim dtVisitChipInfo As SC3220101ChipInfoDataTable = Nothing
        Dim dtReserveChipInfo As SC3220101ChipInfoDataTable = Nothing

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START
        Dim dtAddApprovalChipInfo As SC3220101AddApprovalChipInfoDataTable = Nothing

        'Dim dtAdd As IC3801002DataSet.ConfirmAddListDataTable = Nothing
        'Dim dtRo As IC3801003DataSet.IC3801003NoDeliveryRODataTable = Nothing


        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        Try

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

            ''店舗別未納者R/O情報を取得
            'dtRo = Me.GetIFNoDeliveryROList(userContext)

            ''店舗別追加承認待ち情報を取得
            'dtAdd = Me.GetIFAddApprovalList(userContext)

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

            'サービス来店チップ情報取得
            'dtVisitChipInfo = Me.GetVisitChip(userContext.DlrCD, userContext.BrnCD, dtRo)
            dtVisitChipInfo = Me.GetVisitChip(userContext.DlrCD, userContext.BrnCD, presentTime)

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

            '追加作業情報取得
            dtAddApprovalChipInfo = Me.GetAddApprovalChipInfo(dtVisitChipInfo, userContext.DlrCD, userContext.BrnCD)

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            'ストール予約チップ情報取得
            dtReserveChipInfo = Me.GetReserveChip(userContext.DlrCD, _
                                                  userContext.BrnCD, _
                                                  dtVisitChipInfo)


        Catch ex As OracleExceptionEx When ex.Number = 1013
            'ORACLEのタイムアウトのみ処理

            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , ex.Message))

            Throw ex

        End Try

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        ' 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 START
        'Dim result As SC3220101ChipInfoDataTable = Me.CreateChipData(dtStanderdLT, _
        '                                                             dtVisitChipInfo, _
        '                                                             dtReserveChipInfo, _
        '                                                             dtAdd, _
        '                                                             dtRo)
        'チップ情報格納
        Dim result As SC3220101ChipInfoDataTable = Me.CreateChipData(dtStanderdLT, _
                                                                     dtVisitChipInfo, _
                                                                     dtReserveChipInfo, _
                                                                     dtAddApprovalChipInfo, _
                                                                     presentTime)

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        Logger.Info("GetVisitChip END")

        Return result

        ' 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 END

    End Function

    '2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 START

    ''' <summary>
    ''' チップ情報格納
    ''' </summary>
    ''' <param name="dtStanderdLT">標準時間情報</param>
    ''' <param name="dtVisitChipInfo">来店情報</param>
    ''' <param name="dtReserveChipInfo">予約情報</param>
    ''' <param name="dtAddApprovalChipInfo">追加作業情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <returns>チップ情報</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応
    ''' 2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    ''' </History>
    Private Function CreateChipData(ByVal dtStanderdLT As StandardLTListDataTable, _
                                    ByVal dtVisitChipInfo As SC3220101ChipInfoDataTable, _
                                    ByVal dtReserveChipInfo As SC3220101ChipInfoDataTable, _
                                    ByVal dtAddApprovalChipInfo As SC3220101AddApprovalChipInfoDataTable, _
                                    ByVal inPresentTime As Date) _
                                    As SC3220101ChipInfoDataTable

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

        'Private Function CreateChipData(ByVal dtStanderdLT As StandardLTListDataTable, _
        '                            ByVal dtVisitChipInfo As SC3220101ChipInfoDataTable, _
        '                            ByVal dtReserveChipInfo As SC3220101ChipInfoDataTable, _
        '                            ByVal dtAdd As IC3801002DataSet.ConfirmAddListDataTable, _
        '                            ByVal dtRo As IC3801003DataSet.IC3801003NoDeliveryRODataTable) As SC3220101ChipInfoDataTable


        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END



        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} START" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim userContext As StaffContext = StaffContext.Current
        Dim smbCommonBiz As SMBCommonClassBusinessLogic = Nothing

        Try

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

            ''2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 START
            'Dim ic3800804Biz As New IC3800804BusinessLogic
            ''2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 END

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            smbCommonBiz = New SMBCommonClassBusinessLogic
            smbCommonBiz.InitCommon(userContext.DlrCD, _
                                    userContext.BrnCD, _
                                    inPresentTime)

            For Each row As SC3220101ChipInfoRow In dtVisitChipInfo.Rows

                Dim rowReserveChipInfoList As SC3220101ChipInfoRow() = Nothing

                '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 START
                '予約の有無
                Dim reserveExistence As String = "0"
                '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 END

                '予約チェック
                If Not row.IsPREZIDNull Then
                    '予約有り

                    Dim rezId As Decimal = row.PREZID

                    '予約情報を予約IDで検索
                    rowReserveChipInfoList = (From col In dtReserveChipInfo _
                                              Where col.PREZID = rezId _
                                              Select col).ToArray

                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} PREZID={2}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , CType(rezId, String)))

                End If

                '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                Dim visitSeq As Long = row.VISITSEQ            '来店実績連番
                Dim orderNo As String = row.ORDERNO            '整備受注No
                Dim orderStatus As String = String.Empty       'R/Oステータス
                '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                Dim minOrderStatus As String = String.Empty
                '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                Dim printTime As DateTime = Nothing            '清算書印刷時刻
                Dim checkFinTime As DateTime = Nothing         '完成検査完了時刻
                Dim chackOrderStatus As String = String.Empty  '作業終了ステータス(表示区分判定に使用する最小のROステータスが80未満の場合は0に設定して作業終了していないとする)
                Dim carWashEndType As String = String.Empty    '洗車終了フラグ
                Dim serviceStatus As String = String.Empty     'サービスステータス

                '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                '予約情報の検索結果確認
                If rowReserveChipInfoList IsNot Nothing AndAlso rowReserveChipInfoList.Count > 0 Then
                    '検索結果有り

                    'ROWに変換
                    Dim rowReserveChipInfo As SC3220101ChipInfoRow = rowReserveChipInfoList(0)

                    '納車予定日時のチェック
                    If Not rowReserveChipInfo.IsREZ_DELI_TIMENull Then
                        '納車予定日時が存在する場合

                        '納車予定日時
                        row.REZ_DELI_TIME = rowReserveChipInfo.REZ_DELI_TIME

                    End If

                    row.FIRST_STARTTIME = Me.SetDateTimeData(rowReserveChipInfo.Item("FIRST_STARTTIME"), Nothing)       '作業開始日時(初回)
                    row.LAST_ENDTIME = Me.SetDateTimeData(rowReserveChipInfo.Item("LAST_ENDTIME"), Date.MinValue)       '使用終了日時(最後)
                    row.WORK_TIME = Me.SetNumericData(rowReserveChipInfo.Item("WORK_TIME"), 0)                          '作業時間(未実施合計)
                    row.UNVALID_REZ_COUNT = Me.SetNumericData(rowReserveChipInfo.Item("UNVALID_REZ_COUNT"), 0)          '有効以外件数

                    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                    printTime = Me.SetDateTimeData(rowReserveChipInfo.Item("INVOICE_PRINT_DATETIME"), Nothing)          '清算書印刷日時      

                    '完成検査完了日時チェック
                    If rowReserveChipInfo.IsMIN_INSPECTION_DATENull Then
                        'NULLの場合
                        '完成検査が終了していないため

                        '日付最小値を設定
                        checkFinTime = Nothing

                    Else
                        '上記以外の場合
                        '完成検査終了しているまたは親子全て終了していない場合

                        '最終完成検査完了日時を設定
                        checkFinTime = rowReserveChipInfo.MAX_INSPECTION_DATE

                    End If

                    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                    '実績ステータス
                    If RESULT_TYPE_TRUE.Equals(rowReserveChipInfo.RESULT_TYPE) Then
                        '実績あり
                        row.RESULT_STATUS = Me.SetStringData(rowReserveChipInfo.Item("RESULT_STATUS"), String.Empty)
                    Else
                        '実績なし
                        row.RESULT_STATUS = String.Empty
                    End If

                    row.WASHFLG = Me.SetStringData(rowReserveChipInfo.Item("WASHFLG"), "")                              '洗車フラグ
                    row.WASH_START = Me.SetDateTimeData(rowReserveChipInfo.Item("WASH_START"), Date.MinValue)           '洗車開始日時
                    row.WASH_END = Me.SetDateTimeData(rowReserveChipInfo.Item("WASH_END"), Date.MinValue)               '洗車終了日時

                    '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 START
                    reserveExistence = "1"  '予約の有無:あり
                    '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 END


                    'ROステータスが85でも洗車が終わっていなければ納車準備へ
                    '洗車有りかつ洗車完了実績が入ってない場合
                    If CHIP_WASHFLG_TRUE.Equals(rowReserveChipInfo.WASHFLG) _
                        AndAlso rowReserveChipInfo.IsWASH_ENDNull Then

                        '洗車が終了していない

                        carWashEndType = CarWashEndType0

                    Else
                        '上記以外

                        '洗車が終わっていないまたは洗車無し

                        carWashEndType = CarWashEndType1

                    End If

                    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                    'ステータスチェック
                    If Not (rowReserveChipInfo.IsRESULT_STATUSNull) Then
                        '存在する場合
                        '値を設定
                        serviceStatus = rowReserveChipInfo.RESULT_STATUS

                    End If

                    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

                    '2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    row.REMAINING_INSPECTION_TYPE = _
                        Me.SetStringData(rowReserveChipInfo.Item("REMAINING_INSPECTION_TYPE"), "")                      '残完成検査区分
                    '2017/09/12 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                Else
                    '検索結果無し

                    row.LAST_ENDTIME = Date.MinValue    '使用終了日時(最後)
                    row.WORK_TIME = 0                   '作業時間(未実施合計)
                    row.UNVALID_REZ_COUNT = 0           '有効以外件数
                    row.WASH_START = Date.MinValue      '洗車開始日時
                    row.WASH_END = Date.MinValue        '洗車終了日時
                    '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 START
                    reserveExistence = "0"  '予約の有無:なし
                    '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 END
                End If


                '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

                'Dim orderNo As String = row.ORDERNO         '整備受注No
                'Dim orderStatus As String = String.Empty    'R/Oステータス
                'Dim printTime As DateTime = Nothing         '清算書印刷時刻
                'Dim checkFinTime As DateTime = Nothing      '完成検査完了時刻

                'Dim dtAddRepairStatus As IC3800804AddRepairStatusDataTableDataTable = Nothing

                'If Not String.IsNullOrEmpty(orderNo) Then
                '    'R/O情報に整備受注Noが一致するデータが存在する場合、追加する

                '    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                '    'Dim rowOrderList As IC3801003DataSet.IC3801003NoDeliveryRORow() = (From col In dtRo _
                '    '                                                                   Where col.ORDERNO = orderNo _
                '    '                                                                   Select col).ToArray
                '    Dim rowOrderList As IC3801003DataSet.IC3801003NoDeliveryRORow() = (From col In dtRo _
                '                                                                       Where col.ORDERNO.Trim = orderNo _
                '                                                                       Select col).ToArray
                '    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                '    If 0 < rowOrderList.Count Then
                '        Dim rowOrder As IC3801003DataSet.IC3801003NoDeliveryRORow = rowOrderList(0)

                '        row.RO_EXISTENCE = CHIP_RO_EXISTENCE_TRUE       'R/O有無:あり

                '        If rowOrder.IsDELIVERYHOPEDATENull Then  '納車予定日時
                '            row.REZ_DELI_TIME = Date.MaxValue
                '        Else
                '            row.REZ_DELI_TIME = Me.StringParseDate(rowOrder.DELIVERYHOPEDATE)
                '        End If

                '        orderStatus = Me.SetStringData(rowOrder.Item("ORDERSTATUS"), "")
                '        printTime = Me.SetDateTimeData(rowOrder.Item("COLSINGPRINTTIME"), Nothing)
                '        checkFinTime = Me.SetDateTimeData(rowOrder.Item("EXAMINETIME"), Nothing)
                '    Else

                '        row.RO_EXISTENCE = CHIP_RO_EXISTENCE_FALSE      'R/O有無:なし
                '        row.REZ_DELI_TIME = Date.MaxValue               '納車予定日時

                '    End If

                '    '追加承認待ち情報に整備受注Noが一致するデータが存在する場合、追加する
                '    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
                '    'Dim rowAddList As IC3801002DataSet.ConfirmAddListRow() = (From col In dtAdd _
                '    '                                                          Where col.ORDERNO = orderNo _
                '    '                                                          Select col).ToArray
                '    Dim rowAddList As IC3801002DataSet.ConfirmAddListRow() = (From col In dtAdd _
                '                                                              Where col.ORDERNO.Trim = orderNo _
                '                                                              Select col).ToArray
                '    '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

                '    If 0 < rowAddList.Count Then
                '        Dim rowAdd As IC3801002DataSet.ConfirmAddListRow = rowAddList(0)

                '        row.ADD_APPROVAL_EXISTENCE = CHIP_ADD_APPROVAL_EXISTENCE_TRUE               '追加承認待ち有無:あり

                '        If Not rowAdd.IsSACONFIRMRELYDATENull Then
                '            row.SACONFIRMRELYDATE = Me.StringParseDate(rowAdd.SACONFIRMRELYDATE)    'SA承認待ち日時
                '        End If
                '    Else
                '        row.ADD_APPROVAL_EXISTENCE = CHIP_ADD_APPROVAL_EXISTENCE_FALSE      '追加承認待ち有無：なし
                '    End If

                '    '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 START

                '    '追加作業ステータスの取得
                '    dtAddRepairStatus = _
                '        DirectCast(ic3800804Biz.GetAddRepairStatusList(userContext.DlrCD, orderNo),  _
                '                   IC3800804AddRepairStatusDataTableDataTable)
                '    '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 END

                'Else
                '    row.RO_EXISTENCE = CHIP_RO_EXISTENCE_FALSE                          'R/O有無:なし
                '    row.REZ_DELI_TIME = Date.MaxValue                                   '納車予定日時
                '    row.ADD_APPROVAL_EXISTENCE = CHIP_ADD_APPROVAL_EXISTENCE_FALSE      '追加承認待ち有無：なし
                'End If


                'RO情報の有無
                If Not row.IsVISIT_IDNull Then
                    'RO情報有り

                    'R/O有無:あり
                    row.RO_EXISTENCE = CHIP_RO_EXISTENCE_TRUE

                    '納車予定日時チェック
                    If row.IsREZ_DELI_TIMENull Then
                        '無し

                        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                        '最大日付設定
                        'row.REZ_DELI_TIME = Date.MaxValue

                        '最大日付設定
                        row.REZ_DELI_TIME = Date.MinValue
                        '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                    End If

                    'ROステータス(最大)
                    orderStatus = row.MAX_RO_STATUS

                    '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'ROステータス（最小）
                    minOrderStatus = row.MIN_RO_STATUS
                    '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                    '追加承認待ち情報に来店実績連番が一致するデータが存在する場合、追加する
                    Dim rowAddList As SC3220101AddApprovalChipInfoRow() = (From col In dtAddApprovalChipInfo _
                                                                           Where col.VISIT_ID = visitSeq _
                                                                           Select col).ToArray

                    '追加作業チェック
                    If 0 < rowAddList.Count Then
                        '追加作業有り

                        'ROWに変換
                        Dim rowAdd As SC3220101AddApprovalChipInfoRow = rowAddList(0)

                        row.ADD_APPROVAL_EXISTENCE = CHIP_ADD_APPROVAL_EXISTENCE_TRUE       '追加承認待ち有無:あり

                        'FM承認日時
                        If Not rowAdd.IsRO_CHECK_DATETIMENull Then
                            '値有り

                            'FM承認日時設定
                            row.SACONFIRMRELYDATE = rowAdd.RO_CHECK_DATETIME                'SA承認待ち日時

                        Else
                            '値無し

                            '最小値設定
                            row.SACONFIRMRELYDATE = Date.MinValue

                        End If
                    Else
                        '追加作業無し

                        row.ADD_APPROVAL_EXISTENCE = CHIP_ADD_APPROVAL_EXISTENCE_FALSE      '追加承認待ち有無：なし

                    End If

                    '最小のROステータスチェック
                    '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'If StatusDeliveryWait.Equals(row.MIN_RO_STATUS) _
                    '    OrElse StatusDeliveryWork.Equals(row.MIN_RO_STATUS) Then
                    '    '最小のROステータスが"80"：納車準備・"85"：納車作業の場合

                    '    '起票中の追加作業の存在チェック
                    '    If CHIP_ADD_APPROVAL_EXISTENCE_TRUE.Equals(row.ADD_APPROVAL_EXISTENCE) Then
                    '        '起票中の追加作業追加作業有り

                    '        '作業中を設定
                    '        chackOrderStatus = WorkEndType0

                    '    Else
                    '        '起票中の追加作業追加作業無し

                    '        '作業終了を設定
                    '        chackOrderStatus = WorkEndType1

                    '    End If

                    'Else
                    '    '上記以外の場合

                    '    '作業中を設定
                    '    chackOrderStatus = WorkEndType0

                    'End If
                    '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                    '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 END

                Else
                    'RO情報無し

                    row.RO_EXISTENCE = CHIP_RO_EXISTENCE_FALSE                          'R/O有無:なし
                    '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'row.REZ_DELI_TIME = Date.MaxValue                                   '納車予定日時
                    row.REZ_DELI_TIME = Date.MinValue                                   '納車予定日時
                    '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                    row.ADD_APPROVAL_EXISTENCE = CHIP_ADD_APPROVAL_EXISTENCE_FALSE      '追加承認待ち有無：なし

                End If

                    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END


                    '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 START
                    '表示区分判定
                    'row.DISPLAY_STATUS = smbCommonBiz.GetChipArea(row.RESULT_STATUS, row.RO_EXISTENCE, orderStatus)

                    '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 START
                    'row.DISPLAY_STATUS = smbCommonBiz.GetChipArea(row.RESULT_STATUS, _
                    '                                              row.RO_EXISTENCE, _
                    '                                              orderStatus, _
                    '                                              reserveExistence)

                    '2012/09/27 TMEJ 日比野 キャンセルしたチップが表示される不具合対応 END

                    '表示区分判定
                    'row.DISPLAY_STATUS = smbCommonBiz.GetChipArea(row.RO_EXISTENCE, _
                    '                                              orderStatus, _
                    '                                              reserveExistence, _
                    '                                              chackOrderStatus, _
                    '                                              carWashEndType, _
                    '                                              serviceStatus)
                '2012/11/03 TMEJ 河原 【A.STEP2】次世代サービス ROステータス切り離し対応 END

                '表示区分判定
                '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                row.DISPLAY_STATUS = smbCommonBiz.GetChipArea(row.RO_EXISTENCE, _
                                                              orderStatus, _
                                                              reserveExistence, _
                                                              carWashEndType, _
                                                              serviceStatus, _
                                                              minOrderStatus)

                    '2017/09/28 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                    If Not row.DISPLAY_STATUS = SMBCommonClassBusinessLogic.DisplayType.Err Then
                        '納車見込遅れ時間
                        row.DELAY_DELI_TIME = Me.GetDelayDeliveryTime(smbCommonBiz, _
                                                                      row, _
                                                                      checkFinTime, _
                                                                      printTime, _
                                                                      dtStanderdLT, _
                                                                      userContext)
                    End If

            Next
        Finally
            If smbCommonBiz IsNot Nothing Then
                smbCommonBiz.Dispose()
                smbCommonBiz = Nothing
            End If
        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dtVisitChipInfo
    End Function

    ''' <summary>
    ''' 納車見込遅れ時間を取得
    ''' </summary>
    ''' <param name="smbCommonBiz">共通関数</param>
    ''' <param name="row">チップ情報</param>
    ''' <param name="checkFinTime">完成検査完了時間</param>
    ''' <param name="printTime">清算書印刷時間</param>
    ''' <param name="dtStanderdLT">標準時間情報</param>
    ''' <param name="userContext">ユーザ情報</param>
    ''' <returns>納車見込遅れ時間</returns>
    ''' <remarks></remarks>
    Private Function GetDelayDeliveryTime(ByVal smbCommonBiz As SMBCommonClassBusinessLogic, _
                                          ByVal row As SC3220101ChipInfoRow, _
                                          ByVal checkFinTime As Date, _
                                          ByVal printTime As Date, _
                                          ByVal dtStanderdLT As StandardLTListDataTable, _
                                          ByVal userContext As StaffContext) As Date
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} START" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim delayDeliveryTime As New Date

        '受付中でない AND 有効以外件数=0
        If (Not CHIP_PROGRESSSTATE_RECEPTION.Equals(row.DISPLAY_STATUS)) _
            And row.UNVALID_REZ_COUNT = 0 Then

            Try
                '納車見込遅れ日時
                '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                'delayDeliveryTime = smbCommonBiz.GetDeliveryDelayDate _
                '                            (CType(row.DISPLAY_STATUS, SMBCommonClassBusinessLogic.DisplayType), _
                '                            row.REZ_DELI_TIME, _
                '                            row.LAST_ENDTIME, _
                '                            checkFinTime, _
                '                            row.WASH_START, _
                '                            row.WASH_END, _
                '                            printTime, _
                '                            row.WORK_TIME, _
                '                            row.WASHFLG, _
                '                            DateTimeFunc.Now(userContext.DlrCD))
                delayDeliveryTime = smbCommonBiz.GetDeliveryDelayDate _
                                            (CType(row.DISPLAY_STATUS, SMBCommonClassBusinessLogic.DisplayType), _
                                            row.REZ_DELI_TIME, _
                                            row.LAST_ENDTIME, _
                                            checkFinTime, _
                                            row.WASH_START, _
                                            row.WASH_END, _
                                            printTime, _
                                            row.WORK_TIME, _
                                            row.WASHFLG, _
                                            DateTimeFunc.Now(userContext.DlrCD), _
                                            row.REMAINING_INSPECTION_TYPE)
                '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

            Catch ex As ArgumentException
                delayDeliveryTime = Date.MaxValue
            End Try
            '2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 START
        ElseIf CHIP_PROGRESSSTATE_RECEPTION.Equals(row.DISPLAY_STATUS) Then
            '受付中の場合
            Dim addMinutes As Long = 0

            If dtStanderdLT IsNot Nothing AndAlso dtStanderdLT.Rows.Count > 0 Then
                Dim rowStanderdLt As StandardLTListRow = DirectCast(dtStanderdLT.Rows(0), StandardLTListRow)
                addMinutes = rowStanderdLt.RECEPT_STANDARD_LT
            End If

            delayDeliveryTime = row.ASSIGNTIMESTAMP.AddMinutes(addMinutes)
            '2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 END
        Else
            delayDeliveryTime = Date.MaxValue     '納車見込遅れ日時
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return delayDeliveryTime

    End Function
    '2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 END

#End Region

#Region "来店(受付待ち)エリアのチップ情報取得"
    '2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 START

    ''' <summary>
    ''' 来店(受付待ち)エリアのチップ情報取得
    ''' </summary>
    ''' <returns>来店エリアのチップ情報</returns>
    ''' <remarks></remarks>
    Public Function GetVisitAreaChip(ByVal userContext As StaffContext) As SC3220101VisitAreaInfoDataTable
        '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        'Public Function GetVisitAreaChip(ByVal staffInfo As StaffContext) As SC3220101VisitAreaInfoDataTable
        '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} START" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログイン情報取得
        '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 START
        'Dim userContext As StaffContext = StaffContext.Current
        '2013/06/13 TMEJ 小澤 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発 END

        Dim dt As SC3220101VisitAreaInfoDataTable = Nothing
        Using adapter As New SC3220101DataSetTableAdapters.SC3220101DataTableAdapter
            dt = adapter.GetVisitAreaChip(userContext.DlrCD, userContext.BrnCD, DateTimeFunc.Now(userContext.DlrCD))
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dt
    End Function

    '2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 END
#End Region

#Region "工程表示アイコン情報作成"

    ' 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 START
    ''' <summary>
    ''' リスト形式に変換
    ''' </summary>
    ''' <param name="saTable">SA情報</param>
    ''' <param name="chipTable">チップ情報</param>
    ''' <param name="dtReseptionWait">来店エリアのチップ情報</param>
    ''' <param name="dtStanderdLT">標準LT情報</param>
    ''' <returns>チップ情報(リスト形式)</returns>
    ''' <remarks></remarks>
    Public Function CreateVisitChip(ByVal saTable As IC3810601DataSet.AcknowledgeStaffListDataTable, _
                                    ByVal chipTable As SC3220101ChipInfoDataTable, _
                                    ByVal dtReseptionWait As SC3220101VisitAreaInfoDataTable, _
                                    ByVal dtStanderdLT As StandardLTListDataTable) As List(Of SAItem)
        'Public Function CreateVisitChip(ByVal saTable As IC3810601DataSet.AcknowledgeStaffListDataTable, _
        '                                ByVal chipTable As SC3220101ChipInfoDataTable) As List(Of SAItem)                                
        ' 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} START" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim SAList As List(Of SAItem) = New List(Of SAItem)
        Dim saItem As SAItem

        For Each saRow As IC3810601DataSet.AcknowledgeStaffListRow In _
                    saTable.Select("", "PRESENCECATEGORY ASC, USERNAME ASC")
            saItem = New SAItem

            saItem.Id = saRow.ACCOUNT                           'アカウント
            saItem.Name = Me.SetStringData(saRow.USERNAME, "")  'SA名

            '退席状況
            Select Case saRow.PRESENCECATEGORY
                Case PresenceCategory.Standby
                    saItem.Stats = "1"
                Case PresenceCategory.Suspend
                    saItem.Stats = "2"
                Case PresenceCategory.Offline
                    saItem.Stats = "3"
                Case Else
                    saItem.Stats = "3"
            End Select

            '2012/09/19 TMEJ 日比野 受付待ち工程の追加対応 START
            '来店(受付待ち)フラグ 0:来店以外
            saItem.Visit = "0"
            '2012/09/19 TMEJ 日比野 受付待ち工程の追加対応 END

            '受付中
            saItem.ChipList.AddRange(Me.GetChipList(saItem.Id, _
                                                    CHIP_PROGRESSSTATE_RECEPTION, _
                                                    "ASSIGNTIMESTAMP ASC", _
                                                    chipTable))
            '作業中/追加作業
            saItem.ChipList.AddRange(Me.GetWorkingChipList(saItem.Id, _
                                                           CHIP_PROGRESSSTATE_WORKING, _
                                                           "LAST_ENDTIME ASC, ASSIGNTIMESTAMP ASC", _
                                                           chipTable))
            '納車準備
            saItem.ChipList.AddRange(Me.GetChipList(saItem.Id, _
                                                    CHIP_PROGRESSSTATE_PREPARATION_DELIVERY, _
                                                    "REZ_DELI_TIME ASC, ASSIGNTIMESTAMP ASC", _
                                                    chipTable))
            '納車作業
            saItem.ChipList.AddRange(Me.GetChipList(saItem.Id, _
                                                    CHIP_PROGRESSSTATE_DELIVERY, _
                                                    "REZ_DELI_TIME ASC, ASSIGNTIMESTAMP ASC", _
                                                    chipTable))

            SAList.Add(saItem)
        Next

        ' 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 START
        '来店エリア情報の追加
        If dtReseptionWait IsNot Nothing AndAlso 0 < dtReseptionWait.Rows.Count Then
            SAList.Add(Me.GetVisitChipData(dtReseptionWait, dtStanderdLT))
        End If
        ' 2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return SAList
    End Function

    ''' <summary>
    ''' チップリストを取得する
    ''' </summary>
    ''' <param name="serchSAcode">検索条件(SAコード)</param>
    ''' <param name="displayStatus">検索条件(工程コード)</param>
    ''' <param name="orderStr">並び替え条件</param>
    ''' <param name="ChipTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetChipList(ByVal serchSAcode As String, _
                                 ByVal displayStatus As Integer, _
                                 ByVal orderStr As String, _
                                 ByVal ChipTable As SC3220101ChipInfoDataTable) As List(Of ChipItem)

        Dim chipList As List(Of ChipItem) = New List(Of ChipItem)

        For Each row As SC3220101ChipInfoRow In ChipTable.Select( _
                                                    "SACODE= '" + serchSAcode + "' AND " + _
                                                    "DISPLAY_STATUS ='" + _
                                                    CType(displayStatus, String) + "'", _
                                                    orderStr)

            chipList.Add(Me.GetChipItem(row, row.DISPLAY_STATUS))
        Next

        Return chipList
    End Function

    ''' <summary>
    ''' 作業中のチップリストを取得する
    ''' </summary>
    ''' <param name="serchSAcode">検索条件(SAコード)</param>
    ''' <param name="displayStatus">検索条件(工程コード)</param>
    ''' <param name="orderStr">並び替え条件</param>
    ''' <param name="ChipTable">チップ情報</param>
    ''' <returns>作業中のチップリスト</returns>
    ''' <remarks></remarks>
    Private Function GetWorkingChipList(ByVal serchSAcode As String, _
                                        ByVal displayStatus As Integer, _
                                        ByVal orderStr As String, _
                                        ByVal ChipTable As SC3220101ChipInfoDataTable) As List(Of ChipItem)

        Dim chipList As List(Of ChipItem) = New List(Of ChipItem)

        Using AddWorkChipTable As New SC3220101ChipInfoDataTable
            For Each row As SC3220101ChipInfoRow In ChipTable.Select( _
                                                    "SACODE= '" + serchSAcode + "' AND " + _
                                                    "DISPLAY_STATUS ='" + _
                                                    CType(displayStatus, String) + "'", _
                                                    orderStr)

                chipList.Add(Me.GetChipItem(row, row.DISPLAY_STATUS))

                '追加作業がある場合
                If row.ADD_APPROVAL_EXISTENCE.Equals(CHIP_ADD_APPROVAL_EXISTENCE_TRUE) Then
                    AddWorkChipTable.ImportRow(row)
                End If
            Next

            If 0 < AddWorkChipTable.Count Then
                '追加作業をチップリストに追加（追加作業承認開始日時でソート）
                For Each row As SC3220101ChipInfoRow In AddWorkChipTable.Select("", "SACONFIRMRELYDATE ASC")

                    chipList.Add(Me.GetChipItem(row, CHIP_PROGRESSSTATE_ADDITION_WORKING))
                Next
            End If
        End Using

        Return chipList
    End Function

    '2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 START
    ''' <summary>
    ''' 来店エリアのチップ情報を取得する
    ''' </summary>
    ''' <param name="dtReseptionWait">来店(受付待ち)情報</param>
    ''' <param name="dtStanderdLT">標準LT情報</param>
    ''' <returns>来店エリアのチップ情報</returns>
    ''' <remarks></remarks>
    Private Function GetVisitChipData(ByVal dtReseptionWait As SC3220101VisitAreaInfoDataTable, _
                                      ByVal dtStanderdLT As StandardLTListDataTable) As SAItem

        Dim visitData As New SAItem

        Dim addMinutes As Long = 0

        If dtStanderdLT IsNot Nothing AndAlso dtStanderdLT.Rows.Count > 0 Then
            Dim rowStanderdLt As StandardLTListRow = DirectCast(dtStanderdLT.Rows(0), StandardLTListRow)
            addMinutes = rowStanderdLt.RECEPT_GUIDE_STANDARD_LT
        End If

        visitData.Visit = "1"

        Dim dtChipInfo As New SC3220101ChipInfoDataTable
        Dim rowChipInfo As SC3220101ChipInfoRow = Nothing

        Try
            For Each row As SC3220101VisitAreaInfoRow In dtReseptionWait.Rows
                rowChipInfo = dtChipInfo.NewSC3220101ChipInfoRow

                rowChipInfo.VCLREGNO = row.VCLREGNO
                rowChipInfo.VISITSEQ = row.VISITSEQ
                '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                'rowChipInfo.REZ_DELI_TIME = Date.MaxValue
                rowChipInfo.REZ_DELI_TIME = Date.MinValue
                '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                rowChipInfo.DELAY_DELI_TIME = row.VISITTIMESTAMP.AddMinutes(addMinutes)

                '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                rowChipInfo.IMP_VCL_FLG = row.IMP_VCL_FLG
                rowChipInfo.SML_AMC_FLG = row.SML_AMC_FLG
                rowChipInfo.EW_FLG = row.EW_FLG
                rowChipInfo.TLM_MBR_FLG = row.TLM_MBR_FLG
                '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

                visitData.ChipList.Add(Me.GetChipItem(rowChipInfo, CHIP_PROGRESSSTATE_RECEPTION_WAIT))
            Next
        Finally
            If dtChipInfo IsNot Nothing Then
                dtChipInfo.Dispose()
            End If
        End Try


        Return visitData
    End Function
    '2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 END

    ''' <summary>
    ''' チップ情報を設定
    ''' </summary>
    ''' <param name="row">チップ情報</param>
    ''' <param name="dispStatus">表示区分</param>
    ''' <returns>チップ情報(リスト項目)</returns>
    ''' <remarks></remarks>
    Private Function GetChipItem(ByVal row As SC3220101ChipInfoRow, _
                                 ByVal dispStatus As Integer) As ChipItem

        Dim ChipItem As New ChipItem

        ChipItem.Id = row.VISITSEQ                      '来店実績連番
        '2012/09/11 TMEJ 日比野 チップに車両登録Noが表示されない不具合対応 START
        'ChipItem.VehiclesRegNo = Right(row.VCLREGNO, 5)
        ChipItem.VehiclesRegNo = Right(row.VCLREGNO.Trim, 5) '車両登録No
        '2012/09/11 TMEJ 日比野 チップに車両登録Noが表示されない不具合対応 END
        ChipItem.Stats = dispStatus                     '表示区分
        '納車予定時刻（yyyy/MM/dd HH:mm形式）
        '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        'ChipItem.DeliTime = Me.FormatTimeStringToDate(row.REZ_DELI_TIME, Date.MaxValue)
        ChipItem.DeliTime = Me.FormatTimeStringToDate(row.REZ_DELI_TIME, Date.MinValue)
        '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        '納車遅れ見込時刻（yyyy/MM/dd HH:mm形式）
        ChipItem.DelayTime = Me.FormatTimeStringToDate(row.DELAY_DELI_TIME, Date.MaxValue)

        '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        ChipItem.ImpVclFlg = row.IMP_VCL_FLG
        ChipItem.SmlAmcFlg = row.SML_AMC_FLG
        ChipItem.EwFlg = row.EW_FLG
        ChipItem.TlmMbrFlg = row.TLM_MBR_FLG
        '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START

        '表示区分(洗車・納車準備)の場合
        If CHIP_PROGRESSSTATE_PREPARATION_DELIVERY.Equals(dispStatus) Then

            '洗車ありの場合
            If (Not row.IsWASHFLGNull) AndAlso CHIP_WASHFLG_TRUE.Equals(row.WASHFLG) Then
                '50:預かり中 or 60:納車待ちの場合
                If (C_STALLPROSESS_CARRY_WAIT.Equals(row.RESULT_STATUS) Or _
                        C_STALLPROSESS_DELI_WAIT.Equals(row.RESULT_STATUS)) Then

                    ChipItem.Wash = CHIP_WASHING_FINISH     '洗車状況:完了
                Else
                    ChipItem.Wash = CHIP_WASHING_IMPERFECT  '洗車状況:未完了
                End If
            Else
                ChipItem.Wash = CHIP_WASHING_NONE           '洗車状況:なし
            End If
        Else
            ChipItem.Wash = CHIP_WASHING_NONE
        End If

        Return ChipItem
    End Function

#End Region

#Region "サービス来店管理情報"

    ''' <summary>
    ''' サービス来店管理情報取得
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    ''' </History>
    Public Function GetVisitManager(ByVal visitSeq As Long) As SC3220101ServiceVisitManagerInfoDataTable
        Logger.Info("GetVisitManager Start Param:visitSeq=" & CType(visitSeq, String))

        'ログイン情報取得
        Dim userContext As StaffContext = StaffContext.Current

        Dim dt As New SC3220101ServiceVisitManagerInfoDataTable
        Dim adapter As New SC3220101DataSetTableAdapters.SC3220101DataTableAdapter

        Try
            'サービス来店管理情報の取得
            dt = adapter.GetVisitManagement(userContext.DlrCD, userContext.BrnCD, visitSeq)


            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

            'If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            '    Dim row As SC3220101ServiceVisitManagerInfoRow = _
            '        CType(dt.Rows(0), SC3220101ServiceVisitManagerInfoRow)

            '    If (Not String.IsNullOrEmpty(row.VIN)) OrElse _
            '        (Not String.IsNullOrEmpty(row.VCLREGNO)) Then
            '        '顧客情報を取得
            '        Dim dtCustomer As IC3800703DataSet.IC3800703SrvCustomerDataTable = _
            '                    Me.GetIFCustomerInfo(row.VCLREGNO, row.VIN, userContext.DlrCD)

            '        If dtCustomer IsNot Nothing AndAlso 0 < dtCustomer.Rows.Count Then
            '            Dim rowCustomer As IC3800703DataSet.IC3800703SrvCustomerFRow
            '            rowCustomer = CType(dtCustomer.Rows(0), IC3800703DataSet.IC3800703SrvCustomerFRow)

            '            row.NAME = rowCustomer.BUYERNAME            '顧客名
            '            row.VCLREGNO = rowCustomer.REGISTERNO       '車両登録No
            '            row.VIN = rowCustomer.VINNO                 'VIN
            '            row.MODELCODE = rowCustomer.MODEL           'モデルコード
            '            row.TELNO = rowCustomer.BUYERTEL1           '電話番号
            '            row.MOBILE = rowCustomer.BUYERTEL2          '携帯番号

            '        End If
            '    End If
            'End If

            'サービス来店管理情報の取得チェック
            If dt IsNot Nothing AndAlso 0 < dt.Rows.Count Then
                '取得成功している場合

                '基幹コードへ変換処理
                Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow = Me.ChangeDmsCode(userContext)

                '基幹販売店コード設定
                dt(0).DMSDLRCD = rowDmsCodeMap.CODE1

                '基幹店舗コード設定
                dt(0).DMSSTRCD = rowDmsCodeMap.CODE2

                '基幹アカウントコード設定
                dt(0).DMSACCOUNT = rowDmsCodeMap.ACCOUNT

            End If

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        Catch ex As OracleExceptionEx When ex.Number = 1013
            Logger.Error("GetVisitManager END Exception:" + ex.Message)
            Throw
        Finally
            ' リソースを解放
            If adapter IsNot Nothing Then
                adapter.Dispose()
                adapter = Nothing
            End If

        End Try

        Logger.Info("GetVisitManager END")
        Return dt

    End Function

#End Region

#Region "外部IF処理"

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' 店舗別未納者R/O情報を取得
    ' ''' </summary>
    ' ''' <param name="staffInfo">スタッフ情報</param>
    ' ''' <returns>SA別未納者R/O一覧データセット</returns>
    ' ''' <remarks></remarks>
    'Private Function GetIFNoDeliveryROList(ByVal staffInfo As StaffContext) _
    '                            As IC3801003DataSet.IC3801003NoDeliveryRODataTable

    '    '開始ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} {2}" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                            , LOG_START))

    '    Dim bis As New IC3801003BusinessLogic
    '    Dim dt As IC3801003DataSet.IC3801003NoDeliveryRODataTable

    '    'IF用ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                              , "CALL IF:IC3801003BusinessLogic.GetNoDeliveryROList" + _
    '                                " IN:dlrcd={0}, SACODE={1}, isRezFlag={2}" _
    '                              , staffInfo.DlrCD _
    '                              , String.Empty _
    '                              , "0"))

    '    dt = bis.GetNoDeliveryROList(staffInfo.DlrCD, String.Empty, "0")

    '    ' IF戻り値をログ出力
    '    'Me.OutPutIFLog(dt, "IC3801003BusinessLogic.GetNoDeliveryROList")

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                              , "CALL IF:IC3801003BusinessLogic.GetNoDeliveryROList OUT:Count = {0}" _
    '                              , dt.Rows.Count))

    '    '終了ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} {2}" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                            , LOG_END))

    '    '処理結果返却
    '    Return dt
    'End Function

    ' ''' <summary>
    ' ''' 店舗別追加承認待ち情報を取得
    ' ''' </summary>
    ' ''' <param name="staffInfo">スタッフ情報</param>
    ' ''' <returns>SA別未納者R/O一覧データセット</returns>
    ' ''' <remarks></remarks>
    'Private Function GetIFAddApprovalList(ByVal staffInfo As StaffContext) As IC3801002DataSet.ConfirmAddListDataTable

    '    '開始ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} {2}" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                            , LOG_START))

    '    Dim biz As New IC3801002BusinessLogic
    '    Dim dt As IC3801002DataSet.ConfirmAddListDataTable

    '    'IF用ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                              , "CALL IF:IC3801002BusinessLogic.GetConfirmAddList " + _
    '                                "IN:dlrcd={0}, STRCD={1}, SACODE={2}" _
    '                              , staffInfo.DlrCD _
    '                              , staffInfo.BrnCD _
    '                              , String.Empty))

    '    dt = biz.GetConfirmAddList(staffInfo.DlrCD, staffInfo.BrnCD, String.Empty)

    '    ' IF戻り値をログ出力
    '    'Me.OutPutIFLog(dt, "IC3801002BusinessLogic.GetConfirmAddList")

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                              , "CALL IF:IC3801002BusinessLogic.GetConfirmAddList " + _
    '                                "OUT:Count = {0}" _
    '                              , dt.Rows.Count))

    '    '終了ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} {2}" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                            , LOG_END))

    '    '処理結果返却
    '    Return dt
    'End Function

    ' ''' <summary>
    ' ''' 顧客情報を取得
    ' ''' </summary>
    ' ''' <param name="vclregNo">来店実績連番</param>
    ' ''' <param name="vin">VIN</param>
    ' ''' <param name="dlrCode">販売店コード</param>
    ' ''' <returns>顧客情報</returns>
    ' ''' <remarks></remarks>
    'Private Function GetIFCustomerInfo(ByVal vclregNo As String, _
    '                                   ByVal vin As String, _
    '                                   ByVal dlrCode As String) As IC3800703DataSet.IC3800703SrvCustomerDataTable

    '    '開始ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} {2}" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                            , LOG_START))

    '    Dim IC3800703Bis As New IC3800703BusinessLogic
    '    Dim dt As IC3800703DataSet.IC3800703SrvCustomerDataTable

    '    'IF用ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                              , "CALL IF:IC3800703BusinessLogic.GetCustomerInfo " + _
    '                                "IN:vclregNo={0}, vin={1}, dlrcd={2}" _
    '                              , vclregNo _
    '                              , vin _
    '                              , dlrCode))

    '    dt = IC3800703Bis.GetCustomerInfo(vclregNo, vin, dlrCode)

    '    ' IF戻り値をログ出力
    '    'Me.OutPutIFLog(dt, "IC3800703BusinessLogic.GetCustomerInfo")

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                              , "CALL IF:IC3800703BusinessLogic.GetCustomerInfo OUT:Count = {0}" _
    '                              , dt.Rows.Count))

    '    '終了ログ
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} {2}" _
    '                            , Me.GetType.ToString _
    '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                            , LOG_END))

    '    '処理結果返却
    '    Return dt
    'End Function

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

    '2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 START

    ''' <summary>
    ''' サービス標準LT取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <returns>標準LT</returns>
    ''' <remarks></remarks>
    Public Function GetStandardLTList(ByVal inDealerCode As String, _
                                      ByVal inStoreCode As String) _
                                      As IC3810701DataSet.StandardLTListDataTable
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:inDealerCode = {3}, inStoreCode = {4}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , inDealerCode, inStoreCode))

        Dim bl As New IC3810701BusinessLogic
        Dim dt As IC3810701DataSet.StandardLTListDataTable = Nothing

        Try
            dt = bl.GetStandardLTList(inDealerCode, inStoreCode)
        Finally
            If bl IsNot Nothing Then
                bl.Dispose()
                bl = Nothing
            End If
        End Try

        ' IF戻り値をログ出力
        Me.OutPutIFLog(dt, "IC3810701BusinessLogic.GetStandardLTList")

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        Return dt
    End Function

    ''' <summary>
    ''' SA情報の取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inStoreCode">店舗コード</param>
    ''' <returns>SA情報</returns>
    ''' <remarks></remarks>
    Public Function GetAcknowledgeStaffList(ByVal inDealerCode As String, _
                                            ByVal inStoreCode As String) _
                                        As IC3810601DataSet.AcknowledgeStaffListDataTable
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                 , "{0}.{1} {2} IN:inDealerCode = {3}, inStoreCode = {4}" _
                                 , Me.GetType.ToString _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , LOG_START _
                                 , inDealerCode, inStoreCode))

        Dim bl As IC3810601BusinessLogic = Nothing
        Dim dt As IC3810601DataSet.AcknowledgeStaffListDataTable = Nothing

        Dim operationCodeList As New List(Of Long)
        operationCodeList.Add(9)

        Try
            bl = New IC3810601BusinessLogic
            dt = bl.GetAcknowledgeStaffList(inDealerCode, inStoreCode, operationCodeList)
        Finally
            If bl IsNot Nothing Then
                bl.Dispose()
                bl = Nothing
            End If
        End Try

        ' IF戻り値をログ出力
        Me.OutPutIFLog(dt, "IC3810601BusinessLogic.GetAcknowledgeStaffList")

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))
        Return dt

    End Function
    '2012/09/19 TMEJ 日比野 【SERVICE_2】受付待ち工程の追加対応 END

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' 基幹コードへ変換処理
    ''' 販売店コード・店舗コード・アカウントをそれぞれ
    ''' 基幹販売店コード・基幹店舗コード・基幹アカウントに変換
    ''' </summary>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <remarks>基幹コード情報ROW</remarks>
    ''' <history>
    ''' </history>
    Public Function ChangeDmsCode(ByVal inStaffInfo As StaffContext) _
                                  As ServiceCommonClassDataSet.DmsCodeMapRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4} ACCOUNT = {5} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inStaffInfo.DlrCD, inStaffInfo.BrnCD, inStaffInfo.Account))

        'SMBCommonClassBusinessLogicのインスタンス
        Using smbCommon As New ServiceCommonClassBusinessLogic


            '基幹コードへ変換処理
            Dim dtDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
                smbCommon.GetIcropToDmsCode(inStaffInfo.DlrCD, _
                                            ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                            inStaffInfo.DlrCD, _
                                            inStaffInfo.BrnCD, _
                                            String.Empty, _
                                            inStaffInfo.Account)

            '基幹コード情報Row
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow

            '基幹コードへ変換処理結果チェック
            If dtDmsCodeMap IsNot Nothing AndAlso 0 < dtDmsCodeMap.Rows.Count Then
                '基幹コードへ変換処理成功

                'Rowに変換
                rowDmsCodeMap = CType(dtDmsCodeMap.Rows(0), ServiceCommonClassDataSet.DmsCodeMapRow)

                '基幹アカウントチェック
                If rowDmsCodeMap.IsACCOUNTNull Then
                    '値無し

                    '空文字を設定する
                    '基幹アカウント
                    rowDmsCodeMap.ACCOUNT = String.Empty

                End If

                '基幹販売店コードチェック
                If rowDmsCodeMap.IsCODE1Null Then
                    '値無し

                    '空文字を設定する
                    '基幹販売店コード
                    rowDmsCodeMap.CODE1 = String.Empty

                End If

                '基幹店舗コードチェック
                If rowDmsCodeMap.IsCODE2Null Then
                    '値無し

                    '空文字を設定する
                    '基幹店舗コード
                    rowDmsCodeMap.CODE2 = String.Empty

                End If

            Else
                '基幹コードへ変換処理成功失敗

                '新しいRowを作成
                rowDmsCodeMap = CType(dtDmsCodeMap.NewDmsCodeMapRow, ServiceCommonClassDataSet.DmsCodeMapRow)

                '空文字を設定する
                '基幹アカウント
                rowDmsCodeMap.ACCOUNT = String.Empty
                '基幹販売店コード
                rowDmsCodeMap.CODE1 = String.Empty
                '基幹店舗コード
                rowDmsCodeMap.CODE2 = String.Empty

            End If


            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2} dtDmsCodeMap:COUNT = {3}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtDmsCodeMap.Count))

            '結果返却
            Return rowDmsCodeMap

        End Using

    End Function

    '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

#End Region

#Region "DataSet取得"

    ''' <summary>
    ''' サービス来店チップ情報取得.
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <returns>来店チップ情報</returns>
    ''' <remarks></remarks>
    ''' <History>
    ''' 2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発
    ''' </History>
    Private Function GetVisitChip(ByVal dealerCode As String,
                                  ByVal branchCode As String,
                                  ByVal inPresentTime As Date) _
                                  As SC3220101DataSet.SC3220101ChipInfoDataTable

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START


        'Private Function GetVisitChip(ByVal dealerCode As String,
        '                      ByVal branchCode As String,
        '                      ByVal orderNoList As IC3801003DataSet.IC3801003NoDeliveryRODataTable) _
        '                  As SC3220101DataSet.SC3220101ChipInfoDataTable

        '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        Dim adapter As New SC3220101DataSetTableAdapters.SC3220101DataTableAdapter

        Try

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

            'Dim dtVisitChipInfo As SC3220101ChipInfoDataTable = _
            '    adapter.GetVisitChip(dealerCode, branchCode, orderNoList)

            'サービス来店チップ情報取得
            Dim dtVisitChipInfo As SC3220101ChipInfoDataTable = _
                adapter.GetVisitChip(dealerCode, branchCode, inPresentTime)

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

            '結果返却
            Return dtVisitChipInfo


        Catch ex As OracleExceptionEx When ex.Number = 1013
            'DBタイムアウト

            Throw ex

        Finally

            If adapter IsNot Nothing Then

                adapter.Dispose()
                adapter = Nothing

            End If

        End Try

    End Function

    ''' <summary>
    ''' ストール予約チップ情報取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="visitChipNoList">サービス来店チップ情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetReserveChip(ByVal dealerCode As String,
                                   ByVal branchCode As String,
                                   ByVal visitChipNoList As SC3220101ChipInfoDataTable) As SC3220101ChipInfoDataTable

        Dim adapter As New SC3220101DataSetTableAdapters.SC3220101DataTableAdapter

        Try
            Dim dtReserveChipInfo As SC3220101ChipInfoDataTable = Nothing

            If visitChipNoList IsNot Nothing AndAlso visitChipNoList.Rows.Count > 0 Then
                dtReserveChipInfo = adapter.GetReserveChip(dealerCode, branchCode, visitChipNoList)
            End If

            Return dtReserveChipInfo
        Catch ex As OracleExceptionEx When ex.Number = 1013
            Throw ex
        Finally
            If adapter IsNot Nothing Then
                adapter.Dispose()
                adapter = Nothing
            End If
        End Try

    End Function

    ''' <summary>
    ''' 追加作業情報取得
    ''' </summary>
    ''' <param name="visitChipNoList">サービス来店チップ情報</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks>追加作業情報</remarks>
    Private Function GetAddApprovalChipInfo(ByVal visitChipNoList As SC3220101ChipInfoDataTable _
                                          , ByVal inDealerCode As String _
                                          , ByVal inBranchCode As String) _
                                            As SC3220101DataSet.SC3220101AddApprovalChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} {2} IN:COUNT = {3}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , LOG_START _
                                    , visitChipNoList.Count))

        'SC3220101DataTableAdapterのインスタンス
        Using adapter As New SC3220101DataSetTableAdapters.SC3220101DataTableAdapter

            Dim dtAddApprovalChipInfo As SC3220101AddApprovalChipInfoDataTable = Nothing

            'サービス来店チップ情報チェック
            If visitChipNoList IsNot Nothing AndAlso 0 < visitChipNoList.Rows.Count Then
                'サービス来店チップ情報が存在する場合

                '追加作業チップ情報取得
                dtAddApprovalChipInfo = adapter.GetAddApprovalChipInfo(visitChipNoList, inDealerCode, inBranchCode)

            End If

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} {2}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , LOG_END))

            '結果返却
            Return dtAddApprovalChipInfo

        End Using

    End Function

#End Region

#Region "ログ出力(IF戻り値用)"

    ''' <summary>
    ''' ログ出力(IF戻り値用)
    ''' </summary>
    ''' <param name="dt">戻り値(DataTable)</param>
    ''' <param name="ifName">使用IF名</param>
    ''' <remarks></remarks>
    Private Sub OutPutIFLog(ByVal dt As DataTable, ByVal ifName As String)

        If dt Is Nothing Then
            Return
        End If

        Logger.Info(ifName + " Result START " + " OutPutCount: " + CType(dt.Rows.Count, String))

        Dim log As New Text.StringBuilder

        For j = 0 To dt.Rows.Count - 1

            log = New Text.StringBuilder()
            Dim dr As DataRow = dt.Rows(j)

            log.Append("RowNum: " + CType(j + 1, String) + " -- ")

            For i = 0 To dt.Columns.Count - 1
                log.Append(dt.Columns(i).Caption)
                If IsDBNull(dr(i)) Then
                    log.Append(" IS NULL")
                Else
                    log.Append(" = ")
                    log.Append(dr(i).ToString)
                End If

                If i <= dt.Columns.Count - 2 Then
                    log.Append(", ")
                End If
            Next

            Logger.Info(log.ToString)
        Next

        Logger.Info(ifName + " Result END ")

    End Sub

#End Region

#Region "その他"

    ''' <summary>
    ''' Date型を文字列に変換(yyyy/MM/dd HH:mm)
    ''' </summary>
    ''' <param name="_date">変換元</param>
    ''' <param name="strNull">変換できない場合に返す値</param>
    ''' <returns>変換値</returns>
    ''' <remarks></remarks>
    Private Function FormatTimeStringToDate(ByVal _date As Date, ByVal strNull As Date) As String

        Dim str As String
        Try

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

            'str = DateTimeFunc.FormatDate(2, _date)

            '日付チェック(yyyy/MM/dd HH:mm)
            If _date <> Date.MinValue Then
                '日付(最小値)以外の場合

                '引数でフォーマットする
                str = String.Format(CultureInfo.CurrentCulture, "{0:yyyy/MM/dd HH:mm}", _date)

            Else
                '日付(最小値)の場合

                'デフォルト値でフォーマットする(yyyy/MM/dd HH:mm)
                str = String.Format(CultureInfo.CurrentCulture, "{0:yyyy/MM/dd HH:mm}", strNull)

            End If

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        Catch ex As FormatException

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 START

            'str = DateTimeFunc.FormatDate(2, strNull)
            '(yyyy/MM/dd HH:mm)
            str = String.Format(CultureInfo.CurrentCulture, "{0:yyyy/MM/dd HH:mm}", strNull)

            '2013/12/10 TMEJ 河原 次世代サービス 工程管理機能開発 END

        End Try

        Return str
    End Function

    ''' <summary>
    ''' 文字列をDate型に変換
    ''' </summary>
    ''' <param name="str">変換元文字列</param>
    ''' <returns>変換後日付</returns>
    ''' <remarks></remarks>
    Private Function StringParseDate(ByVal str As String) As DateTime
        Dim rtnDate As DateTime

        If String.IsNullOrEmpty(str) Then
            rtnDate = Nothing
        Else
            Try
                rtnDate = DateTime.Parse(str, CultureInfo.InvariantCulture())
            Catch ex As FormatException
                rtnDate = Nothing
            End Try
        End If

        Return rtnDate
    End Function

    ''' <summary>
    ''' DBNullのデータをデフォルト値で返す
    ''' </summary>
    ''' <param name="src"></param>
    ''' <param name="defult">デフォルト値</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetStringData(ByVal src As Object, ByVal defult As String) As String

        Dim returnValue As String

        If IsDBNull(src) = True Then
            returnValue = defult
        Else
            returnValue = DirectCast(src, String)
        End If

        Return returnValue

    End Function

    ''' <summary>
    ''' DBNullのデータをデフォルト値で返す
    ''' </summary>
    ''' <param name="src"></param>
    ''' <param name="defult">デフォルト値</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetNumericData(ByVal src As Object, ByVal defult As Integer) As Long

        Dim returnValue As Long

        If IsDBNull(src) = True Then
            returnValue = defult
        Else
            returnValue = DirectCast(src, Long)
        End If

        Return returnValue

    End Function

    ''' <summary>
    ''' DBNullのデータをデフォルト値で返す
    ''' </summary>
    ''' <param name="src"></param>
    ''' <param name="defult"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetDateTimeData(ByVal src As Object, ByVal defult As DateTime) As DateTime

        Dim returnValue As Date

        If IsDBNull(src) = True Then
            returnValue = defult
        Else
            returnValue = DirectCast(src, DateTime)
        End If

        Return returnValue
    End Function

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class