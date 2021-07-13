'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240301BusinessLogic.vb
'─────────────────────────────────────
'機能：次世代SMBのサブチップ共通のビジネスロジック
'補足： 
'作成：2013/01/17 TMEJ 丁 タブレット版SMB機能開発(工程管理)
'更新：2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
'更新：2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
'更新：2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応
'更新：2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 
'更新：2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) 
'更新：2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発
'更新：2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力
'更新：2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化
'更新：2015/07/17 TMEJ 河原 タブレットSMB性能改善 通知WebService化
'更新：2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
'更新：2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新：2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない
'更新：2018/12/21 NSK 坂本 TR-SVT-TMT-20180421-001 サービスタブレットのレスポンスが全画面で遅延している_工程管理でPS01を呼び出さないようにしたい
'更新：2019/07/19 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Globalization
Imports Toyota.eCRB.SMB.SubChipBox.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.DataAccess.IC3802503DataSet
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Text
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess
'2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet
'2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END
'2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 START
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
'2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 START

'2015/07/17 TMEJ 河原 タブレットSMB性能改善 通知WebService化 START

Imports System.Threading
Imports System.IO
Imports System.Xml
Imports System.Net
Imports System.Web
Imports System.Text.RegularExpressions
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

'2015/07/17 TMEJ 河原 タブレットSMB性能改善 通知WebService化 END


Public Class SC3240301BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

    '2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 START
    Private LogServiceCommonBiz As New ServiceCommonClassBusinessLogic(True)
    '2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 END

#Region "定数"
    '操作結果
    Private Const RET_OK As Long = 0                     '正常終了
    Private Const ADDWORK_CONFIRMWAIT As String = "2"                    '追加作業承認待ち
    '受付区分
    Private Const ACCEPTANCE_TYPE_RECEPTION As String = "0"              '受付区分　0:予約客
    Private Const ACCEPTANCE_TYPE_WALKIN As String = "1"              '受付区分　1:walk-in

    ''' <summary>
    ''' 自画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMID As String = "SC3240301"

    ''' <summary>
    ''' DB数値型の既定値（0）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultNumberValue As Long = 0
    'キャンセルフラグ
    ''' <summary>
    ''' キャンセルフラグ 　0:有効　
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOT_CANCEL = "0"
    ''' <summary>
    ''' キャンセルフラグ   1:キャンセル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CANCEL = "1"

    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
    '仮置きフラグ
    ''' <summary>
    ''' 仮置きフラグ 　0:仮置きでない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOT_TEMP As String = "0"
    ''' 仮置きフラグ 　1:仮置き
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TEMP As String = "1"
    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

    ''' <summary>
    ''' サブエリアID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RECEPTION As String = "100"                 '受付
    Private Const C_ADDITIONALWORK As String = "200"            '追加作業
    Private Const C_COMPLETIONINSPECTION As String = "300"      '完成検査
    Private Const C_CARWASH As String = "400"                   '洗車
    Private Const C_DELIVERDCAR As String = "500"               '納車待ち
    Private Const C_NOSHOW As String = "600"                    'NoShow
    Private Const C_STOP As String = "700"                      '中断

    ''' <summary>
    ''' プッシュ送信用引数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PUSH_FuntionNM As String = "CallPushEvent()"  'プッシュ送信で呼び出されるJSメソッド名

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 来店管理メインメニューリフレッシュ関数名
    ''' </summary>
    Private Const PUSH_FuntionSVR As String = "RefreshWindow()"

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' ROステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_TC_ISSUING As String = "15"                 'TC起票中
    Private Const C_WAITING_FOR_FM_APPROVLA As String = "20"            'FM承認待ち
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    Private Const C_CREATING_PARTS_ROUGH_QUOTATION As String = "25"      '25：Creating Parts rough quotation
    Private Const C_CREATING_PARTS_QUOTATION As String = "30"            '30：Creating Parts quotation
    Private Const C_WAITING_FOR_RO_CONFIRMATION As String = "35"         '35：Waiting for R/O Confirmation
    Private Const C_WAITING_FOR_CUSTOMER_APPROVAL As String = "40"       '40：Waiting for Customer Approval
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' 追加作業ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_AW_ADDINGWORK As String = "1"                 'TC起票中
    Private Const C_AW_WAIT_COMMITTED As String = "2"            'FM承認待ち

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    Private Const C_AW_NOMARK As String = "0"                    '追加作業マークが表示されない

#Region "部品"
    ''' <summary>
    ''' 部品準備ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Parts_Issued_Completely As String = "8"                 '部品準備完了

    ''' <summary>
    ''' 部品準備フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PartsFlg_NoIssue As String = "0"                 '部品準備未完了
    Private Const PartsFlg_Completely As String = "1"                 '部品準備完了


#End Region
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

#Region "Publicメッソド"

    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
#Region "指定サービス入庫IDに紐づく最大のストール利用IDの取得"

    ''' <summary>
    ''' 指定サービス入庫IDに紐づく最大のストール利用IDを取得する
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <returns>サービス入庫IDに紐づく最大のストール利用ID</returns>
    ''' <remarks></remarks>
    Public Function GetMaxStallUseIdGroupByServiceId(ByVal inServiceInId As Decimal, _
                                                     ByVal inDealerCode As String, _
                                                     ByVal inBranchCode As String) As Decimal

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} Start inServiceInId={2}, inDealerCode={3}, inBranchCode={4}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , inServiceInId _
                                , inDealerCode _
                                , inBranchCode))

        'ストール利用IDに0を設定する
        Dim maxStallUseId As Decimal = 0

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            '最大ストールIDを取得
            maxStallUseId = _
                clsTabletSMBCommonClass.GetMaxStallUseIdGroupByServiceId(inServiceInId, _
                                                                         inDealerCode, _
                                                                         inBranchCode)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} Start maxStallUseId={2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , maxStallUseId))

        Return maxStallUseId

    End Function

#End Region

#Region "納車遅れとなる配置を行った場合、通知の処理"

    ''' <summary>
    ''' 納車遅れとなる配置を行った場合、通知の処理
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inNow">現在日時</param>
    ''' <param name="inUserInfo">ログイン情報</param>
    ''' <remarks></remarks>
    Public Sub SendNoticeWhenSetChipToBeLated(ByVal inStallUseId As Decimal, _
                                              ByVal inNow As Date, _
                                              ByVal inUserInfo As StaffContext)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} Start inStallUseId={2}, inNow={3}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , inStallUseId _
                                , inNow))

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            'チップエンティティを取得
            Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = clsTabletSMBCommonClass.GetChipEntity(inStallUseId)

            If 0 < dtChipEntity.Count Then
                '取得できた場合(取得行数>0)

                '通知を出す処理
                clsTabletSMBCommonClass.SendNoticeWhenSetChipToBeLated(inUserInfo, _
                                                                       inNow, _
                                                                       dtChipEntity(0))

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} End" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region
    '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

#Region "納車待ちチップ情報の取得処理"
    ''' <summary>
    ''' 納車待ちチップ情報の取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDeliverdChipData(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim nowDate As Date = DateTimeFunc.Now(dlrCode)
        '遅れ見込みディクショナリー(サービス入庫ID,遅れ見込み日時)
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        'Dim deliDelayData As Dictionary(Of Decimal, Date)
        Dim deliDelayData As Dictionary(Of Decimal, TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow)
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        '納車待ちチップ情報の取得
        Dim dt As SC3240301DataSet.SC3240301SubChipInfoDataTable
        Dim commonDataAdapter As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
        Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
            dt = adapter.GetDeliverdChipList(dlrCode, brnCode)
        End Using

        Try
            If dt.Count > 0 Then
                Dim svcInIdList As New List(Of Decimal)
                For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt
                    'ROステータスを取得する
                    Dim inRoNum As String = dr.RO_NUM

                    '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
                    Dim dtRoStatus As TabletSMBCommonClassDataSet.TabletSmbCommonClassROStatusDataTable = _
                        commonDataAdapter.GetROStatusInfo(inRoNum, _
                                                          dlrCode, _
                                                          brnCode)
                    '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

                    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                    'If Not IsNothing(dtRoStatus) AndAlso _
                    '         dtRoStatus.Count > 0 AndAlso _
                    '         Not dtRoStatus(0).IsRO_STATUSNull Then



                    ''ROステータスが存在する場合、更に「15」(TC起票中)、「20」(FM承認待ち)で絞込み
                    'Dim strSelect As String = String.Format(CultureInfo.InvariantCulture, "RO_NUM='{0}' AND RO_STATUS='{1}' OR RO_STATUS='{2}'", _
                    '                                        inRoNum _
                    '                                        , C_TC_ISSUING _
                    '                                        , C_WAITING_FOR_FM_APPROVLA)

                    'Dim drsRoStatus As TabletSMBCommonClassDataSet.TabletSmbCommonClassROStatusRow() _
                    '    = CType(dtRoStatus.Select(strSelect), TabletSMBCommonClassDataSet.TabletSmbCommonClassROStatusRow())

                    'For Each drRostatus As TabletSMBCommonClassDataSet.TabletSmbCommonClassROStatusRow In drsRoStatus
                    '    If C_TC_ISSUING.Equals(drRostatus.RO_STATUS) Then
                    '        dr.ADD_WORKSTATUS = C_AW_ADDINGWORK
                    '        Exit For
                    '    ElseIf C_WAITING_FOR_FM_APPROVLA.Equals(drRostatus.RO_STATUS) Then
                    '        dr.ADD_WORKSTATUS = C_AW_WAIT_COMMITTED
                    '        Exit For
                    '    End If
                    'Next

                    'End If
                    '追加作業マークに0(表示しない)を初期化
                    dr.ADD_WORKSTATUS = C_AW_NOMARK

                    '全ROレコードをループする
                    For Each drRostatus As TabletSMBCommonClassDataSet.TabletSmbCommonClassROStatusRow In dtRoStatus

                        If C_TC_ISSUING.Equals(drRostatus.RO_STATUS) Then

                            'TC 承認待ちの場合、白いプラスマークが表示される
                            dr.ADD_WORKSTATUS = C_AW_ADDINGWORK
                            Exit For

                        ElseIf C_WAITING_FOR_FM_APPROVLA.Equals(drRostatus.RO_STATUS) _
                            Or C_CREATING_PARTS_ROUGH_QUOTATION.Equals(drRostatus.RO_STATUS) _
                            Or C_CREATING_PARTS_QUOTATION.Equals(drRostatus.RO_STATUS) _
                            Or C_WAITING_FOR_RO_CONFIRMATION.Equals(drRostatus.RO_STATUS) _
                            Or C_WAITING_FOR_CUSTOMER_APPROVAL.Equals(drRostatus.RO_STATUS) Then

                            'FM承認待ちなど場合、黄色プラスマークが表示される
                            dr.ADD_WORKSTATUS = C_AW_WAIT_COMMITTED
                            Exit For
                        End If
                    Next
                    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

                    '遅れ見込みを計算するために、遅れではないチップのサービス入庫IDを記録する
                    '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'If NarrowDownForDelayCompute(dr.SCHE_DELI_DATETIME, nowDate) _
                    '    AndAlso Not svcInIdList.Contains(dr.SVCIN_ID) Then
                    '    svcInIdList.Add(dr.SVCIN_ID)
                    'End If
                    If Not svcInIdList.Contains(dr.SVCIN_ID) AndAlso dr.SCHE_DELI_DATETIME <> DefaultDateTimeValue() Then
                        svcInIdList.Add(dr.SVCIN_ID)
                    End If
                    '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                Next
                '遅れ見込み情報を取得する
                deliDelayData = Me.GetDeliDelayDateTime(svcInIdList)
                If deliDelayData.Count > 0 Then
                    '取得できた場合、LOOPで遅れ見込み日時をチップ情報に設定する
                    For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt
                        If deliDelayData.ContainsKey(dr.SVCIN_ID) Then
                            '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                            'dr.PLAN_DELAYDATE = deliDelayData(dr.SVCIN_ID)
                            Dim delayDr As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow
                            delayDr = deliDelayData(dr.SVCIN_ID)
                            dr.PLAN_DELAYDATE = delayDr.DELI_DELAY_DATETIME
                            dr.REMAINING_INSPECTION_TYPE = delayDr.REMAINING_INSPECTION_TYPE
                            '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                        End If
                    Next
                End If

                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                '部品準備フラグ取得
                dt = Me.GetPartsFlg(dlrCode, brnCode, dt, C_DELIVERDCAR)
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

            End If

            Return dt
        Finally
            commonDataAdapter = Nothing
        End Try
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Function
#End Region

#Region "納車待ちボタンの情報取得処理"
    ''' <summary>
    ''' 納車待ちボタンの情報取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/03/09 TMEJ 明瀬 既存バグ修正
    ''' </history>
    Public Function GetDeliverdButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim nowDate As Date = DateTimeFunc.Now(dlrCode)
        Dim drCountInfo As SC3240301DataSet.SC3240301ChipCountRow
        Using dtCountInfo As New SC3240301DataSet.SC3240301ChipCountDataTable
            drCountInfo = CType(dtCountInfo.NewRow, SC3240301DataSet.SC3240301ChipCountRow)
            '納車待ちCOUNT情報を取得
            Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable
            Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
                '納車待ちCOUNT情報を取得
                dt = adapter.GetDeliverdButtonInfo(dlrCode, brnCode, nowDate)
            End Using
            Dim objSum As Object
            Dim svcInIdList As New List(Of Decimal)
            objSum = dt.Compute("SUM(LATEFLG)", "")
            If dt.Count > 0 Then
                '既に遅れチップが存在するかどうかの判断
                If CType(objSum, Long) = 0 Then
                    '遅れチップは一個もないの場合、エリアの全チップに対して遅れ見込みを計算する
                    drCountInfo.LATEFLG = 0
                    For Each dr As SC3240301DataSet.SC3240301ChipCountRow In dt
                        'サービス入庫単位で遅れ見込み計算するため、サービス入庫IDを保持する
                        If Not svcInIdList.Contains(dr.SVCIN_ID) Then
                            svcInIdList.Add(dr.SVCIN_ID)
                        End If
                    Next
                    Using combiz As New TabletSMBCommonClassBusinessLogic
                        '遅れ見込み計算する
                        Dim dtDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable = _
                            combiz.GetDeliveryDelayDateList(svcInIdList, dlrCode, brnCode, nowDate)
                        'ループで遅れ見込み日時を判断する
                        For Each drDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow In dtDeliDelay

                            '2015/03/09 TMEJ 明瀬 既存バグ修正 START
                            'If drDeliDelay.IsDELI_DELAY_DATETIMENull Then
                            '    Continue For
                            'ElseIf DateDiff("s", nowDate, drDeliDelay.DELI_DELAY_DATETIME) < 0 Then
                            '    '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                            '    drCountInfo.LATEFLG = 1
                            '    Exit For
                            'End If

                            If drDeliDelay.IsDELI_DELAY_DATETIMENull Then

                                Continue For

                            ElseIf drDeliDelay.DELI_DELAY_DATETIME <= nowDate Then
                                '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                                drCountInfo.LATEFLG = 1
                                Exit For
                            End If
                            '2015/03/09 TMEJ 明瀬 既存バグ修正 END

                        Next
                    End Using
                Else
                    '遅れチップがある場合、遅れフラグを立てる
                    drCountInfo.LATEFLG = 1
                End If
            Else
                drCountInfo.LATEFLG = 0
            End If
            'エリアCOUNT情報を補足する
            drCountInfo.AREAID = C_DELIVERDCAR
            drCountInfo.COUNT = dt.Count
            dtCountInfo.AddSC3240301ChipCountRow(drCountInfo)
            '返却用データテーブル
            Dim retrunDt As SC3240301DataSet.SC3240301ChipCountDataTable = _
                CType(dtCountInfo.Copy(), SC3240301DataSet.SC3240301ChipCountDataTable)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return retrunDt
        End Using
    End Function
#End Region

#Region "洗車チップ情報の取得処理"
    ''' <summary>
    ''' 洗車チップ情報の取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCarWashChipData(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim nowDate As Date = DateTimeFunc.Now(dlrCode)
        '遅れ見込みディクショナリー(サービス入庫ID,遅れ見込み日時)
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        'Dim deliDelayData As Dictionary(Of Decimal, Date)
        Dim deliDelayData As Dictionary(Of Decimal, TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow)
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        Dim commonDataAdapter As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
        '洗車チップ情報の取得
        Dim dt As SC3240301DataSet.SC3240301SubChipInfoDataTable
        Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
            dt = adapter.GetCarWashChipList(dlrCode, brnCode)
        End Using
        Try
            Dim svcInIdList As New List(Of Decimal)
            If dt.Count > 0 Then
                For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt
                    '追加作業ステータスを取得する
                    Dim inRoNum As String = dr.RO_NUM
                    '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
                    'Dim dtRoStatus As TabletSMBCommonClassDataSet.TabletSmbCommonClassROStatusDataTable = commonDataAdapter.GetROStatusInfo(inRoNum)

                    Dim dtRoStatus As TabletSMBCommonClassDataSet.TabletSmbCommonClassROStatusDataTable = _
                        commonDataAdapter.GetROStatusInfo(inRoNum, _
                                                          dlrCode, _
                                                          brnCode)
                    '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

                    'If Not IsNothing(dtRoStatus) AndAlso _
                    '         dtRoStatus.Count > 0 AndAlso _
                    '         Not dtRoStatus(0).IsRO_STATUSNull Then
                    '追加作業が存在する場合

                    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                    'Dim strSelect As String = String.Format(CultureInfo.InvariantCulture, "RO_NUM='{0}' AND RO_STATUS='{1}' OR RO_STATUS='{2}'", _
                    '                                        inRoNum _
                    '                                        , C_TC_ISSUING _
                    '                                        , C_WAITING_FOR_FM_APPROVLA)

                    'Dim drsRoStatus As TabletSMBCommonClassDataSet.TabletSmbCommonClassROStatusRow() _
                    '    = CType(dtRoStatus.Select(strSelect), TabletSMBCommonClassDataSet.TabletSmbCommonClassROStatusRow())

                    'For Each drRostatus As TabletSMBCommonClassDataSet.TabletSmbCommonClassROStatusRow In drsRoStatus
                    '    If C_TC_ISSUING.Equals(drRostatus.RO_STATUS) Then
                    '        dr.ADD_WORKSTATUS = C_AW_ADDINGWORK
                    '        Exit For
                    '    ElseIf C_WAITING_FOR_FM_APPROVLA.Equals(drRostatus.RO_STATUS) Then
                    '        dr.ADD_WORKSTATUS = C_AW_WAIT_COMMITTED
                    '        Exit For
                    '    End If
                    'Next
                    'End If

                    '追加作業マークに0(表示しない)を初期化
                    dr.ADD_WORKSTATUS = C_AW_NOMARK

                    '全ROレコードをループする
                    For Each drRostatus As TabletSMBCommonClassDataSet.TabletSmbCommonClassROStatusRow In dtRoStatus

                        If C_TC_ISSUING.Equals(drRostatus.RO_STATUS) Then

                            'TC 承認待ちの場合、白いプラスマークが表示される
                            dr.ADD_WORKSTATUS = C_AW_ADDINGWORK
                            Exit For

                        ElseIf C_WAITING_FOR_FM_APPROVLA.Equals(drRostatus.RO_STATUS) _
                            Or C_CREATING_PARTS_ROUGH_QUOTATION.Equals(drRostatus.RO_STATUS) _
                            Or C_CREATING_PARTS_QUOTATION.Equals(drRostatus.RO_STATUS) _
                            Or C_WAITING_FOR_RO_CONFIRMATION.Equals(drRostatus.RO_STATUS) _
                            Or C_WAITING_FOR_CUSTOMER_APPROVAL.Equals(drRostatus.RO_STATUS) Then

                            'FM承認待ちなど場合、黄色プラスマークが表示される
                            dr.ADD_WORKSTATUS = C_AW_WAIT_COMMITTED
                            Exit For

                        End If

                    Next
                    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END



                    '遅れ見込みを計算するために、遅れではないチップのサービス入庫IDを記録する
                    '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'If NarrowDownForDelayCompute(dr.SCHE_DELI_DATETIME, nowDate) _
                    '    AndAlso Not svcInIdList.Contains(dr.SVCIN_ID) Then
                    '    svcInIdList.Add(dr.SVCIN_ID)
                    'End If
                    If Not svcInIdList.Contains(dr.SVCIN_ID) AndAlso dr.SCHE_DELI_DATETIME <> DefaultDateTimeValue() Then
                        svcInIdList.Add(dr.SVCIN_ID)
                    End If
                    '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                Next
                '遅れ見込み情報を取得する
                deliDelayData = Me.GetDeliDelayDateTime(svcInIdList)
                If deliDelayData.Count > 0 Then
                    '取得できた場合、ループで遅れ見込み日時をチップ情報に設定する
                    For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt
                        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                        'dr.PLAN_DELAYDATE = deliDelayData(dr.SVCIN_ID)
                        If deliDelayData.ContainsKey(dr.SVCIN_ID) Then
                            Dim delayDr As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow
                            delayDr = deliDelayData(dr.SVCIN_ID)
                            dr.PLAN_DELAYDATE = delayDr.DELI_DELAY_DATETIME
                            dr.REMAINING_INSPECTION_TYPE = delayDr.REMAINING_INSPECTION_TYPE
                        End If
                        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                    Next
                End If

                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                '部品準備フラグ取得
                dt = Me.GetPartsFlg(dlrCode, brnCode, dt, C_CARWASH)
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

            Return dt
        Finally
            commonDataAdapter = Nothing
        End Try
    End Function
#End Region

#Region "洗車ボタンの情報取得処理"
    ''' <summary>
    ''' 洗車ボタンの情報取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/03/09 TMEJ 明瀬 既存バグ修正
    ''' </history>
    Public Function GetCarWashButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim nowDate As Date = DateTimeFunc.Now(dlrCode)

        Using dtCountInfo As New SC3240301DataSet.SC3240301ChipCountDataTable
            Dim drCountInfo As SC3240301DataSet.SC3240301ChipCountRow = CType(dtCountInfo.NewRow, SC3240301DataSet.SC3240301ChipCountRow)
            '洗車COUNT情報を取得
            Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable
            Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
                dt = adapter.GetCarWashButtonInfo(dlrCode, brnCode, nowDate)
            End Using
            Dim objSum As Object
            Dim svcInIdList As New List(Of Decimal)
            'COUNT情報の遅れ状態を統計する
            objSum = dt.Compute("SUM(LATEFLG)", "")
            If dt.Count > 0 Then
                '既に遅れチップが存在するかどうかの判断
                If CType(objSum, Long) = 0 Then
                    '遅れチップは一個もないの場合、エリアの全チップに対して遅れ見込みを計算する
                    For Each dr As SC3240301DataSet.SC3240301ChipCountRow In dt
                        'サービス入庫単位で遅れ見込み計算するため、サービス入庫IDを保持する
                        If Not svcInIdList.Contains(dr.SVCIN_ID) Then
                            svcInIdList.Add(dr.SVCIN_ID)
                        End If
                    Next
                    Using combiz As New TabletSMBCommonClassBusinessLogic
                        '遅れ見込み計算する
                        Dim dtDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable = _
                            combiz.GetDeliveryDelayDateList(svcInIdList, dlrCode, brnCode, nowDate)
                        'ループで遅れ見込み日時を判断する
                        For Each drDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow In dtDeliDelay

                            '2015/03/09 TMEJ 明瀬 既存バグ修正 START
                            'If drDeliDelay.IsDELI_DELAY_DATETIMENull Then
                            '    Continue For
                            'ElseIf DateDiff("s", nowDate, drDeliDelay.DELI_DELAY_DATETIME) < 0 Then
                            '    '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                            '    drCountInfo.LATEFLG = 1
                            '    Exit For
                            'End If

                            If drDeliDelay.IsDELI_DELAY_DATETIMENull Then

                                Continue For

                            ElseIf drDeliDelay.DELI_DELAY_DATETIME <= nowDate Then
                                '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                                drCountInfo.LATEFLG = 1
                                Exit For
                            End If
                            '2015/03/09 TMEJ 明瀬 既存バグ修正 END

                        Next
                    End Using
                Else
                    '遅れチップがある場合、遅れフラグを立てる
                    drCountInfo.LATEFLG = 1
                End If
            Else
                drCountInfo.LATEFLG = 0
            End If
            'エリアCOUNT情報を補足する
            drCountInfo.AREAID = C_CARWASH
            drCountInfo.COUNT = dt.Count
            dtCountInfo.AddSC3240301ChipCountRow(drCountInfo)
            '返却用データテーブル
            Dim retrunDt As SC3240301DataSet.SC3240301ChipCountDataTable = _
                CType(dtCountInfo.Copy(), SC3240301DataSet.SC3240301ChipCountDataTable)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return retrunDt
        End Using
    End Function
#End Region

#Region "完成検査チップ情報の取得処理"
    ''' <summary>
    ''' 完成検査チップ情報の取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCompletedInspectionChipData(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim nowDate As Date = DateTimeFunc.Now(dlrCode)
        '遅れ見込みディクショナリー(サービス入庫ID,遅れ見込み日時)
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        'Dim deliDelayData As Dictionary(Of Decimal, Date)
        Dim deliDelayData As Dictionary(Of Decimal, TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow)
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        '完成検査チップ情報の取得
        Dim dt As SC3240301DataSet.SC3240301SubChipInfoDataTable
        Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
            dt = adapter.GetCompletedInspectionChipList(dlrCode, brnCode)
        End Using

        If dt.Count > 0 Then
            Dim svcInIdList As New List(Of Decimal)
            For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt
                '遅れ見込みを計算するために、遅れではないチップのサービス入庫IDを記録する
                '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                'If NarrowDownForDelayCompute(dr.SCHE_DELI_DATETIME, nowDate) _
                '    AndAlso Not svcInIdList.Contains(dr.SVCIN_ID) Then
                '    svcInIdList.Add(dr.SVCIN_ID)
                'End If
                If Not svcInIdList.Contains(dr.SVCIN_ID) AndAlso dr.SCHE_DELI_DATETIME <> DefaultDateTimeValue() Then
                    svcInIdList.Add(dr.SVCIN_ID)
                End If
                '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            Next

            '遅れ見込み情報を取得する
            deliDelayData = Me.GetDeliDelayDateTime(svcInIdList)
            If deliDelayData.Count > 0 Then
                '取得できた場合、ループで遅れ見込み日時をチップ情報に設定する
                For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt
                    If deliDelayData.ContainsKey(dr.SVCIN_ID) Then
                        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                        'dr.PLAN_DELAYDATE = deliDelayData(dr.SVCIN_ID)
                        Dim delayDr As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow
                        delayDr = deliDelayData(dr.SVCIN_ID)
                        dr.PLAN_DELAYDATE = delayDr.DELI_DELAY_DATETIME
                        dr.REMAINING_INSPECTION_TYPE = delayDr.REMAINING_INSPECTION_TYPE
                        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                    End If
                Next
            End If

            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            '部品準備フラグ取得
            dt = Me.GetPartsFlg(dlrCode, brnCode, dt, C_COMPLETIONINSPECTION)
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        Return dt
    End Function

#End Region

#Region "完成検査ボタンの情報取得処理"
    ''' <summary>
    ''' 完成検査ボタンの情報取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/03/09 TMEJ 明瀬 既存バグ修正
    ''' </history>
    Public Function GetCompletedInspectionButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim nowDate As Date = DateTimeFunc.Now(dlrCode)

        Using dtCountInfo As New SC3240301DataSet.SC3240301ChipCountDataTable
            Dim drCountInfo As SC3240301DataSet.SC3240301ChipCountRow = CType(dtCountInfo.NewRow, SC3240301DataSet.SC3240301ChipCountRow)
            '完成検査COUNT情報を取得
            Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable
            Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
                dt = adapter.GetCompletedInspectionButtonInfo(dlrCode, brnCode, nowDate)
            End Using

            Dim objSum As Object
            Dim svcInIdList As New List(Of Decimal)
            objSum = dt.Compute("SUM(LATEFLG)", "")
            If dt.Count > 0 Then
                '既に遅れチップが存在するかどうかの判断
                If CType(objSum, Long) = 0 Then
                    '遅れチップは一個もないの場合、エリアの全チップに対して遅れ見込みを計算する
                    For Each dr As SC3240301DataSet.SC3240301ChipCountRow In dt
                        'サービス入庫単位で遅れ見込み計算するため、サービス入庫IDを保持する
                        If Not svcInIdList.Contains(dr.SVCIN_ID) Then
                            svcInIdList.Add(dr.SVCIN_ID)
                        End If
                    Next
                    Using combiz As New TabletSMBCommonClassBusinessLogic
                        '遅れ見込み計算する
                        Dim dtDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable = _
                            combiz.GetDeliveryDelayDateList(svcInIdList, dlrCode, brnCode, nowDate)

                        'ループで遅れ見込み日時を判断する
                        For Each drDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow In dtDeliDelay

                            '2015/03/09 TMEJ 明瀬 既存バグ修正 START
                            'If drDeliDelay.IsDELI_DELAY_DATETIMENull Then
                            '    Continue For
                            'ElseIf DateDiff("s", nowDate, drDeliDelay.DELI_DELAY_DATETIME) < 0 Then
                            '    '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                            '    drCountInfo.LATEFLG = 1
                            '    Exit For
                            'End If

                            If drDeliDelay.IsDELI_DELAY_DATETIMENull Then

                                Continue For

                            ElseIf drDeliDelay.DELI_DELAY_DATETIME <= nowDate Then
                                '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                                drCountInfo.LATEFLG = 1
                                Exit For
                            End If
                            '2015/03/09 TMEJ 明瀬 既存バグ修正 END

                        Next
                    End Using
                Else
                    '遅れチップがある場合、遅れフラグを立てる
                    drCountInfo.LATEFLG = 1
                End If
            Else
                drCountInfo.LATEFLG = 0
            End If
            'エリアCOUNT情報を補足する
            drCountInfo.AREAID = C_COMPLETIONINSPECTION
            drCountInfo.COUNT = dt.Count
            dtCountInfo.AddSC3240301ChipCountRow(drCountInfo)
            '返却用データテーブル
            Dim retrunDt As SC3240301DataSet.SC3240301ChipCountDataTable = _
                CType(dtCountInfo.Copy(), SC3240301DataSet.SC3240301ChipCountDataTable)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return retrunDt
        End Using
    End Function
#End Region

#Region "追加作業チップ情報の取得処理"
    ''' <summary>
    ''' 追加作業チップ情報の取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ''' Public Function GetAddWorkChipData(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable
    Public Function GetAddWorkChipData(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim roNumlist As New List(Of String)
        '遅れ見込みディクショナリー(サービス入庫ID,遅れ見込み日時)

        'Using dtsort As New SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable
        '    '追加作業ステータスが「2:追加作業承認待ち」の追加作業情報を取得
        '    Dim dtAddWorkStatusInfo As IC3800809DataSet.IC3800809AddRepairInfoDataTable = Me.GetAddRepairInfoList(dlrCode)
        '    If dtAddWorkStatusInfo.Count > 0 Then
        '        '取得した追加作業情報のROナンバーを記録する
        '        For Each dr As IC3800809DataSet.IC3800809AddRepairInfoRow In dtAddWorkStatusInfo
        '            roNumlist.Add(dr.orderNO)
        '        Next
        '        Dim dt As SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable
        '        Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
        '            '追加作業チップ情報を取得する
        '            dt = adapter.GetAddWorkChipList(roNumlist, dlrCode, brnCode)
        '        End Using

        '        If dt.Count > 0 Then
        '            Dim svcInIdList As New List(Of Long)
        '            '追加作業チップ情報を補足する
        '            For Each dr As SC3240301DataSet.SC3240301SubReceptionChipInfoRow In dt
        '                Dim roNum As String = dr.RO_NUM
        '                'IF情報と受付チップ情報のレコードを特定する
        '                Dim drAddWorkStatus As IC3800809DataSet.IC3800809AddRepairInfoRow() = _
        '                        (From col In dtAddWorkStatusInfo Where col.orderNO = roNum Select col).ToArray
        '                If drAddWorkStatus.Count > DefaultNumberValue Then
        '                    dr.RO_JOB_SEQ = drAddWorkStatus(0).workSeq
        '                    dr.SRVADDSEQ = drAddWorkStatus(0).addSeq
        '                    If Not drAddWorkStatus(0).IsdeliveryHopeDateNull Then
        '                        dr.SCHE_DELI_DATETIME = drAddWorkStatus(0).deliveryHopeDate
        '                    Else
        '                        dr.SCHE_DELI_DATETIME = DefaultDateTimeValue()
        '                    End If
        '                End If

        '                '遅れ見込みを計算するために、遅れじゃないチップのサービス入庫IDを記録する
        '                If NarrowDownForDelayCompute(dr.SCHE_DELI_DATETIME, DateTimeFunc.Now(dlrCode)) _
        '                    AndAlso Not svcInIdList.Contains(dr.SVCIN_ID) Then
        '                    svcInIdList.Add(dr.SVCIN_ID)
        '                End If
        '            Next

        '            '遅れ見込み情報を取得する
        '            deliDelayData = Me.GetDeliDelayDateTime(svcInIdList)
        '            If deliDelayData.Count > DefaultNumberValue Then
        '                '取得できた場合、ループで遅れ見込み日時をチップ情報に設定する
        '                For Each dr As SC3240301DataSet.SC3240301SubReceptionChipInfoRow In dt
        '                    If deliDelayData.ContainsKey(dr.SVCIN_ID) Then
        '                        dr.PLAN_DELAYDATE = deliDelayData(dr.SVCIN_ID)
        '                    End If
        '                Next
        '            End If

        '            '追加作業チップ情報を納車予定日時でソートする
        '            Dim dv As DataView = dt.DefaultView
        '            dv.Sort = "SCHE_DELI_DATETIME ASC"
        '            dtsort.Merge(dv.ToTable())
        '        End If
        '    End If
        '    Dim retrunDt As SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable = CType(dtsort.Copy(), SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable)
        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        '    Return retrunDt
        'End Using
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        'Dim deliDelayData As Dictionary(Of Decimal, Date)
        Dim deliDelayData As Dictionary(Of Decimal, TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow)
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
            '追加作業チップ情報を取得する
            Dim dt As SC3240301DataSet.SC3240301SubChipInfoDataTable = adapter.GetAddWorkChipList(dlrCode, brnCode)
            If dt.Count > 0 Then
                Dim svcInIdList As New List(Of Decimal)
                For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt
                    '遅れ見込みを計算するために、遅れではないチップのサービス入庫IDを記録する
                    '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'If NarrowDownForDelayCompute(dr.SCHE_DELI_DATETIME, nowDate) _
                    '    AndAlso Not svcInIdList.Contains(dr.SVCIN_ID) Then
                    '    svcInIdList.Add(dr.SVCIN_ID)
                    'End If
                    If Not svcInIdList.Contains(dr.SVCIN_ID) AndAlso dr.SCHE_DELI_DATETIME <> DefaultDateTimeValue() Then
                        svcInIdList.Add(dr.SVCIN_ID)
                    End If
                    '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                Next

                '遅れ見込み情報を取得する
                deliDelayData = Me.GetDeliDelayDateTime(svcInIdList)
                If deliDelayData.Count > DefaultNumberValue Then
                    '取得できた場合、ループで遅れ見込み日時をチップ情報に設定する
                    For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt
                        If deliDelayData.ContainsKey(dr.SVCIN_ID) Then
                            '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                            'dr.PLAN_DELAYDATE = deliDelayData(dr.SVCIN_ID)
                            Dim delayDr As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow
                            delayDr = deliDelayData(dr.SVCIN_ID)
                            dr.PLAN_DELAYDATE = delayDr.DELI_DELAY_DATETIME
                            dr.REMAINING_INSPECTION_TYPE = delayDr.REMAINING_INSPECTION_TYPE
                            '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                        End If
                    Next
                End If

            End If
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using
    End Function
#End Region

#Region "追加作業ボタンの情報取得処理"
    ''' <summary>
    ''' 追加作業ボタンの情報取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/03/09 TMEJ 明瀬 既存バグ修正
    ''' </history>
    Public Function GetAddWorkButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
        'Dim nowDate As Date = DateTimeFunc.Now(dlrCode)
        'Dim addWorkCount As Long
        'Dim delayFlg As Boolean = False
        'Using dtCountInfo As New SC3240301DataSet.SC3240301ChipCountDataTable
        '    Dim drCountInfo As SC3240301DataSet.SC3240301ChipCountRow = _
        '        CType(dtCountInfo.NewRow, SC3240301DataSet.SC3240301ChipCountRow)
        '    '追加作業ステータスが「2:追加作業承認待ち」の追加作業情報を取得
        '    Dim dtAddInfo As IC3800809DataSet.IC3800809AddRepairInfoDataTable = Me.GetAddRepairInfoList(dlrCode)
        '    Dim roNumlist As New List(Of String)
        '    If dtAddInfo.Count > DefaultNumberValue Then
        '        For Each dr As IC3800809DataSet.IC3800809AddRepairInfoRow In dtAddInfo
        '            '取得した追加作業情報のROナンバーを記録する
        '            roNumlist.Add(dr.orderNO)
        '            '遅れているチップが存在するかどうかを判断する
        '            If DateDiff("s", dr.deliveryHopeDate, nowDate) > DefaultNumberValue Then
        '                '遅れているチップがあった場合
        '                delayFlg = True
        '            End If
        '        Next

        '        '追加作業エリアのCOUNT情報を取得する
        '        Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable
        '        Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
        '            'dt = adapter.GetReceptionAddWorkButtonInfo(dlrCode, brnCode, roNumlist, nowDate)
        '            dt = adapter.GetAddWorkButtonInfo(dlrCode, brnCode, nowDate)
        '        End Using

        '        'COUNTを記録する
        '        addWorkCount = dt.Count
        '        Dim svcInIdList As New List(Of Long)
        '        If dt.Count > DefaultNumberValue Then
        '            If Not delayFlg Then
        '                '遅れていない場合遅れ見込み計算する
        '                For Each dr As SC3240301DataSet.SC3240301ChipCountRow In dt
        '                    '遅れ見込み計算必要のチップのサービス入庫IDを記録する
        '                    If Not svcInIdList.Contains(dr.SVCIN_ID) Then
        '                        svcInIdList.Add(dr.SVCIN_ID)
        '                    End If
        '                Next

        '                '遅れ見込み計算する
        '                Dim dtDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable
        '                Using combiz As New TabletSMBCommonClassBusinessLogic
        '                    dtDeliDelay = combiz.GetDeliveryDelayDateList(svcInIdList, dlrCode, brnCode, nowDate)
        '                End Using

        '                'ループで遅れ見込み日時を判断する
        '                For Each drDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow In dtDeliDelay
        '                    If drDeliDelay.IsDELI_DELAY_DATETIMENull Then
        '                        Continue For
        '                    ElseIf DateDiff("s", nowDate, drDeliDelay.DELI_DELAY_DATETIME) < DefaultNumberValue Then
        '                        '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
        '                        drCountInfo.LATEFLG = 1
        '                        Exit For
        '                    End If
        '                Next
        '            Else
        '                '遅れチップがある場合、遅れフラグを立てる
        '                drCountInfo.LATEFLG = 1
        '            End If
        '        Else
        '            drCountInfo.LATEFLG = DefaultNumberValue
        '        End If
        '        'エリアCOUNT情報を補足する
        '        drCountInfo.AREAID = C_ADDITIONALWORK
        '        drCountInfo.COUNT = addWorkCount
        '    Else
        '        drCountInfo.AREAID = C_ADDITIONALWORK
        '        drCountInfo.COUNT = dtAddInfo.Count
        '        drCountInfo.LATEFLG = DefaultNumberValue
        '    End If
        '    dtCountInfo.AddSC3240301ChipCountRow(drCountInfo)
        '    '返却用データテーブル
        '    Dim retrunDt As SC3240301DataSet.SC3240301ChipCountDataTable = _
        '        CType(dtCountInfo.Copy(), SC3240301DataSet.SC3240301ChipCountDataTable)
        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        '    Return retrunDt
        Dim nowDate As Date = DateTimeFunc.Now(dlrCode)
        Dim RoNumRoSeqList As New List(Of String)
        Using dtCountInfo As New SC3240301DataSet.SC3240301ChipCountDataTable
            Dim drCountInfo As SC3240301DataSet.SC3240301ChipCountRow = CType(dtCountInfo.NewRow, SC3240301DataSet.SC3240301ChipCountRow)
            '追加作業ボタンの情報を取得
            Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable
            Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
                dt = adapter.GetAddWorkButtonInfo(dlrCode, brnCode, nowDate)
            End Using

            Dim objSum As Object
            Dim svcInIdList As New List(Of Decimal)
            Dim lateFlg As Long
            objSum = dt.Compute("SUM(LATEFLG)", "")
            If dt.Count > 0 Then
                '既に遅れチップが存在するかどうかの判断
                If CType(objSum, Long) = 0 Then
                    '遅れチップは一個もないの場合、エリアの全チップに対して遅れ見込みを計算する
                    For Each dr As SC3240301DataSet.SC3240301ChipCountRow In dt
                        'サービス入庫単位で遅れ見込み計算するため、サービス入庫IDを保持する
                        If Not svcInIdList.Contains(dr.SVCIN_ID) Then
                            svcInIdList.Add(dr.SVCIN_ID)
                        End If

                        'カウントを計算するため、RO番号とRO枝番を記録する
                        If Not RoNumRoSeqList.Contains(dr.RO_NUM & dr.RO_JOB_SEQ) Then
                            RoNumRoSeqList.Add(dr.RO_NUM & dr.RO_JOB_SEQ)
                        End If
                    Next
                    Using combiz As New TabletSMBCommonClassBusinessLogic
                        '遅れ見込み計算する
                        Dim dtDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable = _
                            combiz.GetDeliveryDelayDateList(svcInIdList, dlrCode, brnCode, nowDate)

                        'ループで遅れ見込み日時を判断する
                        For Each drDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow In dtDeliDelay

                            '2015/03/09 TMEJ 明瀬 既存バグ修正 START
                            'If drDeliDelay.IsDELI_DELAY_DATETIMENull Then
                            '    Continue For
                            'ElseIf DateDiff("s", nowDate, drDeliDelay.DELI_DELAY_DATETIME) < 0 Then
                            '    '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                            '    lateFlg = 1
                            '    Exit For
                            'End If

                            If drDeliDelay.IsDELI_DELAY_DATETIMENull Then

                                Continue For

                            ElseIf drDeliDelay.DELI_DELAY_DATETIME <= nowDate Then
                                '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                                lateFlg = 1
                                Exit For
                            End If
                            '2015/03/09 TMEJ 明瀬 既存バグ修正 END

                        Next
                    End Using
                Else
                    '遅れチップがある場合、遅れフラグを立てる
                    lateFlg = 1

                    For Each dr As SC3240301DataSet.SC3240301ChipCountRow In dt
                        'カウントを計算するため、RO番号とRO枝番を記録する
                        If Not RoNumRoSeqList.Contains(dr.RO_NUM & dr.RO_JOB_SEQ) Then
                            RoNumRoSeqList.Add(dr.RO_NUM & dr.RO_JOB_SEQ)
                        End If
                    Next
                End If
            End If
            'エリアCOUNT情報を補足する
            drCountInfo.AREAID = C_ADDITIONALWORK
            drCountInfo.COUNT = RoNumRoSeqList.Count
            drCountInfo.LATEFLG = lateFlg
            dtCountInfo.AddSC3240301ChipCountRow(drCountInfo)
            '返却用データテーブル
            Dim retrunDt As SC3240301DataSet.SC3240301ChipCountDataTable = _
                CType(dtCountInfo.Copy(), SC3240301DataSet.SC3240301ChipCountDataTable)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return retrunDt
        End Using
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
    End Function
#End Region

#Region "受付チップ情報の取得処理"
    ''' <summary>
    ''' 受付チップ情報の取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    '''  Public Function GetReceptionChipData(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable
    Public Function GetReceptionChipData(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable
        ' 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        'Using retrunDt As New SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable
        '    '承認待ちのRO情報を取得する
        '    Dim dtRo As IC3801013DataSet.IC3801013ROReserveInfoDataTable = Me.GetWaittConfirOrderInfo(dlrCode)
        '    '受付チップ情報の取得
        '    If dtRo.Count > DefaultNumberValue Then
        '        Dim roNumlist As New List(Of String)
        '        For Each dr As IC3801013DataSet.IC3801013ROReserveInfoRow In dtRo
        '            roNumlist.Add(dr.orderNO)
        '        Next


        '        Dim dt As SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable
        '        Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
        '            dt = adapter.GetReceptionChipList(roNumlist, dlrCode, brnCode)
        '        End Using
        '        Dim dtRecetion As SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable = Me.MergeRecetionInfo(dt, dtRo)
        '        retrunDt.Merge(dtRecetion)
        '    End If
        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        '    Return retrunDt
        'End Using
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '遅れ見込みディクショナリー(サービス入庫ID,遅れ見込み日時)
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        'Dim deliDelayData As Dictionary(Of Decimal, Date)
        Dim deliDelayData As Dictionary(Of Decimal, TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow)
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter

            LogServiceCommonBiz.OutputLog(33, "●■● 1.8.1 SC3240301_007 START")

            '受付チップ情報の取得
            Dim retrunDt As SC3240301DataSet.SC3240301SubChipInfoDataTable = adapter.GetReceptionChipList(dlrCode, brnCode)

            LogServiceCommonBiz.OutputLog(33, "●■● 1.8.1 SC3240301_007 END")

            If retrunDt.Count > 0 Then
                Dim svcInIdList As New List(Of Decimal)

                '2015/07/17 TMEJ 河原 タブレットSMB性能改善 通知WebService化 START

                'ループ内で現在日時を取得している処理を削除し
                'ループ外で現在日時を取得して以降の処理で使用する

                '現在日時取得
                Dim presentTime As Date = DateTimeFunc.Now(dlrCode)

                '2015/07/17 TMEJ 河原 タブレットSMB性能改善 通知WebService化 END

                For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In retrunDt
                    '遅れ見込みを計算するために、遅れではないチップのサービス入庫IDを記録する

                    '2015/07/17 TMEJ 河原 タブレットSMB性能改善 通知WebService化 START

                    'ループ内で現在日時を取得している処理を削除

                    'If NarrowDownForDelayCompute(dr.SCHE_DELI_DATETIME, DateTimeFunc.Now(dlrCode)) _
                    'AndAlso Not svcInIdList.Contains(dr.SVCIN_ID) Then

                    '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'If NarrowDownForDelayCompute(dr.SCHE_DELI_DATETIME, presentTime) _
                    'AndAlso Not svcInIdList.Contains(dr.SVCIN_ID) Then
                    If Not svcInIdList.Contains(dr.SVCIN_ID) AndAlso dr.SCHE_DELI_DATETIME <> DefaultDateTimeValue() Then
                        '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                        '2015/07/17 TMEJ 河原 タブレットSMB性能改善 通知WebService化 END

                        svcInIdList.Add(dr.SVCIN_ID)
                    End If
                Next

                LogServiceCommonBiz.OutputLog(34, "●■● 1.8.2 受付チップの遅れ見込情報取得 START")

                '遅れ見込み情報を取得する
                deliDelayData = Me.GetDeliDelayDateTime(svcInIdList)

                LogServiceCommonBiz.OutputLog(34, "●■● 1.8.2 受付チップの遅れ見込情報取得 END")

                If deliDelayData.Count > 0 Then
                    '取得できた場合、ループで遅れ見込み日時をチップ情報に設定する
                    For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In retrunDt
                        If deliDelayData.ContainsKey(dr.SVCIN_ID) Then
                            '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                            Dim delayDr As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow
                            delayDr = deliDelayData(dr.SVCIN_ID)
                            dr.PLAN_DELAYDATE = delayDr.DELI_DELAY_DATETIME
                            dr.REMAINING_INSPECTION_TYPE = delayDr.REMAINING_INSPECTION_TYPE
                            '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                        End If
                    Next
                End If

                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                '部品準備フラグ取得
                retrunDt = Me.GetPartsFlg(dlrCode, brnCode, retrunDt, C_RECEPTION)
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

            End If
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

            Return retrunDt
        End Using
    End Function
#End Region

#Region "受付ボタンの情報取得処理"
    ''' <summary>
    ''' 受付ボタンの情報取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/03/09 TMEJ 明瀬 既存バグ修正
    ''' </history>
    Public Function GetReceptionButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim nowDate As Date = DateTimeFunc.Now(dlrCode)
        'Dim delayFlg As Boolean = False
        'Using dtCountInfo As New SC3240301DataSet.SC3240301ChipCountDataTable
        '    Dim drCountInfo As SC3240301DataSet.SC3240301ChipCountRow = CType(dtCountInfo.NewRow, SC3240301DataSet.SC3240301ChipCountRow)
        '    '承認待ちのRO情報を取得する
        '    Dim dtRo As IC3801013DataSet.IC3801013ROReserveInfoDataTable = Me.GetWaittConfirOrderInfo(dlrCode)
        '    Dim roNumlist As New List(Of String)
        '    Dim maxCustConfiDate As Date = DefaultDateTimeValue()
        '    If dtRo.Count > DefaultNumberValue Then
        '        For Each dr As IC3801013DataSet.IC3801013ROReserveInfoRow In dtRo
        '            If dr.IscustomerConfirmDateNull Then
        '                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod.Name & " customerConfirmDate IS NULL")
        '                Continue For
        '            End If
        '            '承認待ちROのRO番号を記録する
        '            roNumlist.Add(dr.orderNO)
        '            '受付ボタンを点滅するため、最新のお客承認日時を保持する
        '            If DateDiff("s", maxCustConfiDate, dr.customerConfirmDate) > DefaultNumberValue Then
        '                maxCustConfiDate = dr.customerConfirmDate
        '            End If
        '            '受付エリアに遅れチップがあるかどうかを判断する
        '            If DateDiff("s", dr.deliveryHopeDate, nowDate) > DefaultNumberValue Then
        '                delayFlg = True
        '                '最新のお客承認日時を取得するためループは続ける
        '            End If
        '        Next

        '        Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable
        '        Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
        '            '受付エリアのCOUNT情報を取得する
        '            'dt = adapter.GetReceptionAddWorkButtonInfo(dlrCode, brnCode, roNumlist, nowDate)
        '            dt = adapter.GetReceptionAddWorkButtonInfo(dlrCode, brnCode, nowDate)
        '        End Using
        '        Dim svcInIdList As New List(Of Long)
        '        If dt.Count > DefaultNumberValue Then
        '            If Not delayFlg Then
        '                '受付エリアに遅れているチップ一つもない場合
        '                For Each dr As SC3240301DataSet.SC3240301ChipCountRow In dt
        '                    'サービス入庫単位で遅れ見込み計算するため、サービス入庫IDを保持する
        '                    If Not svcInIdList.Contains(dr.SVCIN_ID) Then
        '                        svcInIdList.Add(dr.SVCIN_ID)
        '                    End If
        '                Next

        '                Using combiz As New TabletSMBCommonClassBusinessLogic
        '                    '遅れ見込み計算する
        '                    Dim dtDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable = _
        '                        combiz.GetDeliveryDelayDateList(svcInIdList, dlrCode, brnCode, nowDate)

        '                    'ループで遅れ見込み日時を判断する
        '                    For Each drDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow In dtDeliDelay
        '                        If drDeliDelay.IsDELI_DELAY_DATETIMENull Then
        '                            Continue For
        '                        ElseIf DateDiff("s", nowDate, drDeliDelay.DELI_DELAY_DATETIME) < DefaultNumberValue Then
        '                            '遅れ見込みになっているチップ一つがあったら、遅れフラグを立てる、ループを離脱する
        '                            drCountInfo.LATEFLG = 1
        '                            Exit For
        '                        End If
        '                    Next

        '                End Using
        '            Else
        '                '受付エリアに既に遅れているチップがある場合
        '                drCountInfo.LATEFLG = 1
        '            End If
        '        Else
        '            drCountInfo.LATEFLG = DefaultNumberValue
        '        End If
        '        '受付エリアCOUNT情報を補足する
        '        drCountInfo.AREAID = C_RECEPTION
        '        drCountInfo.COUNT = Me.GetRecetionCount(dt, dtRo)
        '        drCountInfo.CUST_CONFIRMDATE = maxCustConfiDate
        '    Else
        '        drCountInfo.AREAID = C_RECEPTION
        '        drCountInfo.COUNT = dtRo.Count
        '        drCountInfo.LATEFLG = DefaultNumberValue
        '        drCountInfo.CUST_CONFIRMDATE = maxCustConfiDate
        '    End If

        '    dtCountInfo.AddSC3240301ChipCountRow(drCountInfo)

        '    '返却用データテーブル
        '    Dim retrunDt As SC3240301DataSet.SC3240301ChipCountDataTable = _
        '        CType(dtCountInfo.Copy(), SC3240301DataSet.SC3240301ChipCountDataTable)
        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        '    Return retrunDt
        'End Using

        '■■■■■基盤からDBアクセス 現在時間取得 2-2 1-1-0-0-0-0 START■■■■■
        LogServiceCommonBiz.OutputLog(36, "●■● 2.1.1 基盤からDBアクセス 現在時間取得 START")


        Dim nowDate As Date = DateTimeFunc.Now(dlrCode)

        LogServiceCommonBiz.OutputLog(36, "●■● 2.1.1 基盤からDBアクセス 現在時間取得 END")
        '■■■■■基盤からDBアクセス 現在時間取得 2-2 1-1-0-0-0-0 END■■■■■

        Dim maxCustConfiDate As Date = DefaultDateTimeValue()
        Using dtCountInfo As New SC3240301DataSet.SC3240301ChipCountDataTable
            Dim drCountInfo As SC3240301DataSet.SC3240301ChipCountRow = CType(dtCountInfo.NewRow, SC3240301DataSet.SC3240301ChipCountRow)
            '受付COUNT情報を取得
            Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable
            Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter

                '■■■■■SQL_SC3240301_013 受付エリア情報を取得 2-3 1-2-0-0-0-0 START■■■■■
                LogServiceCommonBiz.OutputLog(37, "●■● 2.1.2 受付エリア情報を取得 START")

                dt = adapter.GetReceptionButtonInfo(dlrCode, brnCode, nowDate)

                LogServiceCommonBiz.OutputLog(37, "●■● 2.1.2 受付エリア情報を取得[取得件数:" & dt.Count & "] END")
                '■■■■■SQL_SC3240301_013 受付エリア情報を取得(★件数表示) 2-3 1-2-0-0-0-0 END■■■■■
            End Using

            Dim objSum As Object
            Dim svcInIdList As New List(Of Decimal)
            Dim RoNumRoSeqList As New List(Of String)
            Dim lateFlg As Long
            '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
            Dim countTemp As Long
            '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
            objSum = dt.Compute("SUM(LATEFLG)", "")
            If dt.Count > 0 Then
                '既に遅れチップが存在するかどうかの判断
                If CType(objSum, Long) = 0 Then
                    '遅れチップは一個もないの場合、エリアの全チップに対して遅れ見込みを計算する
                    For Each dr As SC3240301DataSet.SC3240301ChipCountRow In dt
                        '受付ボタンを点滅するため、最新のお客承認日時を保持する
                        If DateDiff("s", maxCustConfiDate, dr.CUST_CONFIRMDATE) > DefaultNumberValue Then
                            maxCustConfiDate = dr.CUST_CONFIRMDATE
                        End If
                        'サービス入庫単位で遅れ見込み計算するため、サービス入庫IDを保持する
                        If Not svcInIdList.Contains(dr.SVCIN_ID) Then
                            svcInIdList.Add(dr.SVCIN_ID)
                        End If

                        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                        'カウントを計算するため、RO番号とRO枝番を記録する
                        'If Not RoNumRoSeqList.Contains(dr.RO_NUM & dr.RO_JOB_SEQ) Then
                        '    RoNumRoSeqList.Add(dr.RO_NUM & dr.RO_JOB_SEQ)
                        'End If
                        'カウントを計算するため、RO番号とRO枝番を記録する
                        If dr.TEMP_FLG = NOT_TEMP Then
                            If Not RoNumRoSeqList.Contains(dr.RO_NUM & dr.RO_JOB_SEQ) Then
                                RoNumRoSeqList.Add(dr.RO_NUM & dr.RO_JOB_SEQ)
                            End If
                        ElseIf dr.TEMP_FLG = TEMP Then
                            '仮置きチップの件数をカウントして保持()
                            countTemp += 1
                        End If
                        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                    Next
                    Using combiz As New TabletSMBCommonClassBusinessLogic

                        '■■■■■受付エリア遅れ見込み計算 2-4 1-3-0-0-0-0 START■■■■■
                        LogServiceCommonBiz.OutputLog(38, "●■● 2.1.3 受付エリア遅れ見込み計算 START")


                        '遅れ見込み計算する
                        Dim dtDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable = _
                            combiz.GetDeliveryDelayDateList(svcInIdList, dlrCode, brnCode, nowDate)

                        LogServiceCommonBiz.OutputLog(38, "●■● 2.1.3 受付エリア遅れ見込み計算 END")
                        '■■■■■受付エリア遅れ見込み計算 2-4 1-3-0-0-0-0 END■■■■■

                        'ループで遅れ見込み日時を判断する
                        For Each drDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow In dtDeliDelay

                            '2015/03/09 TMEJ 明瀬 既存バグ修正 START
                            'If drDeliDelay.IsDELI_DELAY_DATETIMENull Then
                            '    Continue For
                            'ElseIf DateDiff("s", nowDate, drDeliDelay.DELI_DELAY_DATETIME) < 0 Then
                            '    '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                            '    lateFlg = 1
                            '    Exit For
                            'End If

                            If drDeliDelay.IsDELI_DELAY_DATETIMENull Then

                                Continue For

                            ElseIf drDeliDelay.DELI_DELAY_DATETIME <= nowDate Then
                                '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                                lateFlg = 1
                                Exit For
                            End If
                            '2015/03/09 TMEJ 明瀬 既存バグ修正 END

                        Next
                    End Using
                Else
                    '遅れチップがある場合、遅れフラグを立てる
                    lateFlg = 1
                    For Each dr As SC3240301DataSet.SC3240301ChipCountRow In dt
                        '受付ボタンを点滅するため、最新のお客承認日時を保持する
                        If DateDiff("s", maxCustConfiDate, dr.CUST_CONFIRMDATE) > DefaultNumberValue Then
                            maxCustConfiDate = dr.CUST_CONFIRMDATE
                        End If

                        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                        'カウントを計算するため、RO番号とRO枝番を記録する
                        'If Not RoNumRoSeqList.Contains(dr.RO_NUM & dr.RO_JOB_SEQ) Then
                        '    RoNumRoSeqList.Add(dr.RO_NUM & dr.RO_JOB_SEQ)
                        'End If
                        ''カウントを計算するため、RO番号とRO枝番を記録する
                        If dr.TEMP_FLG = NOT_TEMP Then
                            If Not RoNumRoSeqList.Contains(dr.RO_NUM & dr.RO_JOB_SEQ) Then
                                RoNumRoSeqList.Add(dr.RO_NUM & dr.RO_JOB_SEQ)
                            End If
                        ElseIf dr.TEMP_FLG = TEMP Then
                            '仮置きチップの件数をカウントして保持()
                            countTemp += 1
                        End If
                        '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                    Next
                End If
            End If
            'エリアCOUNT情報を補足する
            drCountInfo.AREAID = C_RECEPTION
            drCountInfo.COUNT = RoNumRoSeqList.Count
            '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
            drCountInfo.COUNT += countTemp
            '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
            drCountInfo.CUST_CONFIRMDATE = maxCustConfiDate
            drCountInfo.LATEFLG = lateFlg
            dtCountInfo.AddSC3240301ChipCountRow(drCountInfo)
            '返却用データテーブル
            Dim retrunDt As SC3240301DataSet.SC3240301ChipCountDataTable = _
                CType(dtCountInfo.Copy(), SC3240301DataSet.SC3240301ChipCountDataTable)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return retrunDt
        End Using
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
    End Function
#End Region

#Region "NoShowチップ情報の取得処理"
    ''' <summary>
    ''' NoShowチップ情報の取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNoShowChipData(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim nowDate As Date = DateTimeFunc.Now(dlrCode)
        '遅れ見込みディクショナリー(サービス入庫ID,遅れ見込み日時)
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        'Dim deliDelayData As Dictionary(Of Decimal, Date)
        Dim deliDelayData As Dictionary(Of Decimal, TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow)
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
        'NoShowチップ情報の取得
        Dim dt As SC3240301DataSet.SC3240301SubChipInfoDataTable
        Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
            dt = adapter.GetNoShowChipList(dlrCode, brnCode)
        End Using

        If dt.Count > DefaultNumberValue Then
            Dim svcInIdList As New List(Of Decimal)
            For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt
                '遅れ見込みを計算するために、遅れではないチップのサービス入庫IDを保持する
                '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                'If NarrowDownForDelayCompute(dr.SCHE_DELI_DATETIME, nowDate) _
                '    AndAlso Not svcInIdList.Contains(dr.SVCIN_ID) Then
                '    svcInIdList.Add(dr.SVCIN_ID)
                'End If
                If Not svcInIdList.Contains(dr.SVCIN_ID) AndAlso dr.SCHE_DELI_DATETIME <> DefaultDateTimeValue() Then
                    svcInIdList.Add(dr.SVCIN_ID)
                End If
                '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            Next

            '遅れ見込み情報を取得する
            deliDelayData = Me.GetDeliDelayDateTime(svcInIdList)
            If deliDelayData.Count > DefaultNumberValue Then
                '取得できた場合、ループで遅れ見込み日時をチップ情報に設定する
                For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt
                    If deliDelayData.ContainsKey(dr.SVCIN_ID) Then
                        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                        Dim delayDr As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow
                        delayDr = deliDelayData(dr.SVCIN_ID)
                        dr.PLAN_DELAYDATE = delayDr.DELI_DELAY_DATETIME
                        dr.REMAINING_INSPECTION_TYPE = delayDr.REMAINING_INSPECTION_TYPE
                        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                    End If
                Next
            End If

        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return dt
    End Function
#End Region

#Region "NoShowボタンの情報取得処理"
    ''' <summary>
    ''' NoShowボタンの情報取得処理
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/03/09 TMEJ 明瀬 既存バグ修正
    ''' </history>
    Public Function GetNoShowButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim nowDate As Date = DateTimeFunc.Now(dlrCode)

        Using dtCountInfo As New SC3240301DataSet.SC3240301ChipCountDataTable
            Dim drCountInfo As SC3240301DataSet.SC3240301ChipCountRow = CType(dtCountInfo.NewRow, SC3240301DataSet.SC3240301ChipCountRow)
            'NoShowエリアのCOUNT情報を取得する
            Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable
            Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter

                '■■■■■SQL_SC3240301_009 サブチップエリアのNoShowボタンの情報取得 2-16 2-1-0-0-0-0 START■■■■■
                LogServiceCommonBiz.OutputLog(50, "●■● 2.2.1 SC3240301_009 START")

                dt = adapter.GetNoShowButtonInfo(dlrCode, brnCode, nowDate)

                LogServiceCommonBiz.OutputLog(50, "●■● 2.2.1 SC3240301_009[取得件数:" & dt.Count & "] END")
                '■■■■■SQL_SC3240301_009 サブチップエリアのNoShowボタンの情報取得件数(★件数表示) 2-16 2-1-0-0-0-0 END■■■■■

            End Using

            Dim objSum As Object
            Dim svcInIdList As New List(Of Decimal)
            '既に遅れチップが存在するかどうかの判断
            objSum = dt.Compute("SUM(LATEFLG)", "")
            If dt.Count > DefaultNumberValue Then
                If CType(objSum, Long) = DefaultNumberValue Then
                    '遅れチップは一個もないの場合、エリアの全チップに対して遅れ見込みを計算する
                    For Each dr As SC3240301DataSet.SC3240301ChipCountRow In dt
                        'サービス入庫単位で遅れ見込み計算するため、サービス入庫IDを保持する
                        If Not svcInIdList.Contains(dr.SVCIN_ID) Then
                            svcInIdList.Add(dr.SVCIN_ID)
                        End If
                    Next

                    Using combiz As New TabletSMBCommonClassBusinessLogic

                        '■■■■■NoShowエリア遅れ見込み計算 2-17 2-2-0-0-0-0 START■■■■■
                        LogServiceCommonBiz.OutputLog(51, "●■● 2.2.2 NoShowエリア遅れ見込み計算 START")

                        '遅れ見込み計算する
                        Dim dtDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable = _
                            combiz.GetDeliveryDelayDateList(svcInIdList, dlrCode, brnCode, nowDate)

                        LogServiceCommonBiz.OutputLog(51, "●■● 2.2.2 NoShowエリア遅れ見込み計算 END")
                        '■■■■■NoShowエリア遅れ見込み計算 2-17 2-2-0-0-0-0 END■■■■■

                        'ループで遅れ見込み日時を判断する
                        For Each drDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow In dtDeliDelay

                            '2015/03/09 TMEJ 明瀬 既存バグ修正 START
                            'If drDeliDelay.IsDELI_DELAY_DATETIMENull Then
                            '    Continue For
                            'ElseIf DateDiff("s", nowDate, drDeliDelay.DELI_DELAY_DATETIME) < DefaultNumberValue Then
                            '    '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                            '    drCountInfo.LATEFLG = 1
                            '    Exit For
                            'End If

                            If drDeliDelay.IsDELI_DELAY_DATETIMENull Then

                                Continue For

                            ElseIf drDeliDelay.DELI_DELAY_DATETIME <= nowDate Then
                                '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                                drCountInfo.LATEFLG = 1
                                Exit For
                            End If
                            '2015/03/09 TMEJ 明瀬 既存バグ修正 END

                        Next
                    End Using
                Else
                    '遅れチップがある場合、遅れフラグを立てる
                    drCountInfo.LATEFLG = 1
                End If
            Else
                drCountInfo.LATEFLG = DefaultNumberValue
            End If
            'エリアCOUNT情報を補足する
            drCountInfo.AREAID = C_NOSHOW
            drCountInfo.COUNT = dt.Count
            dtCountInfo.AddSC3240301ChipCountRow(drCountInfo)
            '返却用データテーブル
            Dim retrunDt As SC3240301DataSet.SC3240301ChipCountDataTable = _
                CType(dtCountInfo.Copy(), SC3240301DataSet.SC3240301ChipCountDataTable)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return retrunDt
        End Using
    End Function
#End Region

#Region "中断チップ情報の取得処理"
    ''' <summary>
    ''' 中断チップ情報の取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStopChipData(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301SubChipInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim nowDate As Date = DateTimeFunc.Now(dlrCode)
        '遅れ見込みディクショナリー(サービス入庫ID,遅れ見込み日時)
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        'Dim deliDelayData As Dictionary(Of Decimal, Date)
        Dim deliDelayData As Dictionary(Of Decimal, TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow)
        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        '中断チップ情報の取得
        Dim dt As SC3240301DataSet.SC3240301SubChipInfoDataTable
        Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
            dt = adapter.GetStopChipList(dlrCode, brnCode)
        End Using

        If dt.Count > DefaultNumberValue Then
            Dim svcInIdList As New List(Of Decimal)
            For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt
                '遅れ見込みを計算するために、遅れではないチップのサービス入庫IDを保持する
                '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                'If NarrowDownForDelayCompute(dr.SCHE_DELI_DATETIME, nowDate) _
                '    AndAlso Not svcInIdList.Contains(dr.SVCIN_ID) Then
                '    svcInIdList.Add(dr.SVCIN_ID)
                'End If
                If Not svcInIdList.Contains(dr.SVCIN_ID) AndAlso dr.SCHE_DELI_DATETIME <> DefaultDateTimeValue() Then
                    svcInIdList.Add(dr.SVCIN_ID)
                End If
                '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            Next
            '遅れ見込み日時を計算する
            deliDelayData = Me.GetDeliDelayDateTime(svcInIdList)
            If deliDelayData.Count > DefaultNumberValue Then
                '取得できた場合、ループで遅れ見込み日時をチップ情報に設定する
                For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt
                    If deliDelayData.ContainsKey(dr.SVCIN_ID) Then
                        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                        Dim delayDr As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow
                        delayDr = deliDelayData(dr.SVCIN_ID)
                        dr.PLAN_DELAYDATE = delayDr.DELI_DELAY_DATETIME
                        dr.REMAINING_INSPECTION_TYPE = delayDr.REMAINING_INSPECTION_TYPE
                        '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                    End If
                Next
            End If

            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            '部品準備フラグ取得
            dt = Me.GetPartsFlg(dlrCode, brnCode, dt, C_STOP)
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

            '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
            Dim allSvcInIdList As New List(Of Decimal)
            For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt
                'サービス入庫単位での未完了チップ件数を取得するため、全サービス入庫IDのリストを作成
                If Not allSvcInIdList.Contains(dr.SVCIN_ID) Then
                    allSvcInIdList.Add(dr.SVCIN_ID)
                End If
            Next

            Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

                '指定サービス入庫ID単位で未完了チップ件数の取得
                Dim dtChipCount As TabletSmbCommonClassNotFinishedChipCountDataTable = _
                    clsTabletSMBCommonClass.GetNotFinishedChipCount(dlrCode, _
                                                                    brnCode, _
                                                                    allSvcInIdList)

                If dtChipCount IsNot Nothing Then

                    '取得する成功した場合、返却テーブルにループで未完了チップ件数を設定する
                    For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dt

                        Dim svcInId As Decimal = dr.SVCIN_ID

                        '取得したチップ件数情報を絞込
                        Dim jobCountList As List(Of TabletSmbCommonClassNotFinishedChipCountRow) = _
                            (From p In dtChipCount _
                             Where p.SVCIN_ID = svcInId _
                             Select p).ToList()

                        If jobCountList.Count = 0 Then
                            '未完了チップ件数に"-1"を設定する

                            dr.NOT_FINISHED_COUNT = -1

                        Else
                            '（未完了チップ件数テーブル）に含まれている場合

                            dr.NOT_FINISHED_COUNT = jobCountList(0).COUNT

                        End If
                    Next

                End If

            End Using
            '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        Return dt
    End Function
#End Region

#Region "中断ボタンの情報取得処理"
    ''' <summary>
    ''' 中断ボタンの情報取得処理
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/03/09 TMEJ 明瀬 既存バグ修正
    ''' </history>
    Public Function GetStopButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))
        Dim nowDate As Date = DateTimeFunc.Now(dlrCode)
        Using dtCountInfo As New SC3240301DataSet.SC3240301ChipCountDataTable
            Dim drCountInfo As SC3240301DataSet.SC3240301ChipCountRow = CType(dtCountInfo.NewRow, SC3240301DataSet.SC3240301ChipCountRow)
            '中断エリアCOUNT情報を取得する
            Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable
            Using adapter As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
                dt = adapter.GetStopButtonInfo(dlrCode, brnCode, nowDate)
            End Using

            Dim objSum As Object
            Dim svcInIdList As New List(Of Decimal)
            '既に遅れチップが存在するかどうかの判断
            objSum = dt.Compute("SUM(LATEFLG)", "")
            If dt.Count > DefaultNumberValue Then
                If CType(objSum, Long) = DefaultNumberValue Then
                    For Each dr As SC3240301DataSet.SC3240301ChipCountRow In dt
                        'サービス入庫単位で遅れ見込み計算するため、サービス入庫IDを保持する
                        If Not svcInIdList.Contains(dr.SVCIN_ID) Then
                            svcInIdList.Add(dr.SVCIN_ID)
                        End If
                    Next

                    Using combiz As New TabletSMBCommonClassBusinessLogic
                        '遅れ見込み計算する
                        Dim dtDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable = _
                            combiz.GetDeliveryDelayDateList(svcInIdList, dlrCode, brnCode, nowDate)
                        'ループで遅れ見込み日時を判断する
                        For Each drDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow In dtDeliDelay

                            '2015/03/09 TMEJ 明瀬 既存バグ修正 START
                            'If drDeliDelay.IsDELI_DELAY_DATETIMENull Then
                            '    Continue For
                            'ElseIf DateDiff("s", nowDate, drDeliDelay.DELI_DELAY_DATETIME) < DefaultNumberValue Then
                            '    '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                            '    drCountInfo.LATEFLG = 1
                            '    Exit For
                            'End If

                            If drDeliDelay.IsDELI_DELAY_DATETIMENull Then

                                Continue For

                            ElseIf drDeliDelay.DELI_DELAY_DATETIME <= nowDate Then
                                '遅れ見込みのチップがあれば遅れフラグを立てる、ループを離脱する
                                drCountInfo.LATEFLG = 1
                                Exit For
                            End If
                            '2015/03/09 TMEJ 明瀬 既存バグ修正 END
                        Next
                    End Using

                Else
                    '遅れチップがある場合、遅れフラグを立てる
                    drCountInfo.LATEFLG = 1
                End If
            Else
                drCountInfo.LATEFLG = DefaultNumberValue
            End If
            'エリアCOUNT情報を補足する
            drCountInfo.AREAID = C_STOP
            drCountInfo.COUNT = dt.Count
            dtCountInfo.AddSC3240301ChipCountRow(drCountInfo)
            '返却用データテーブル
            Dim retrunDt As SC3240301DataSet.SC3240301ChipCountDataTable = _
                CType(dtCountInfo.Copy(), SC3240301DataSet.SC3240301ChipCountDataTable)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return retrunDt
        End Using
    End Function
#End Region

#Region "チップの移動、リサイズ処理"

    ''' <summary>
    ''' NoShow、中断チップを移動、リサイズ
    ''' </summary>
    ''' <param name="stallUseId">チップの予約ID</param>
    ''' <param name="stallId">変更後のストールのSTALLID</param>
    ''' <param name="ScheStartDateTime">変更後の表示開始日時</param>
    ''' <param name="ScheWorkTime">仕事時間</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <param name="updatedt">更新日時</param>
    ''' <param name="staffCd">スタッフコード</param>
    ''' <param name="rowLockVersion">ROWロックバージョン</param>
    ''' <param name="systemid">プログラムID</param>
    ''' <returns></returns>
    <EnableCommit()>
    Public Function SubChipMoveResize(ByVal stallUseId As Decimal, _
                                        ByVal stallId As Decimal, _
                                        ByVal scheStartDateTime As Date, _
                                        ByVal scheWorkTime As Long, _
                                        ByVal restFlg As String, _
                                        ByVal stallStartTime As Date, _
                                        ByVal stallEndTime As Date, _
                                        ByVal updatedt As Date, _
                                        ByVal staffcd As String, _
                                        ByVal rowLockVersion As Long, _
                                        ByVal systemid As String) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))
        'ストールロックフラグ
        Dim isStallLock As Boolean = False
        Dim result As Long


        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try
                'ストールロック
                result = clsTabletSMBCommonClass.LockStall(stallId, scheStartDateTime, staffcd, updatedt, systemid)
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    Me.Rollback = True
                    Return TabletSMBCommonClassBusinessLogic.ActionResult.LockStallError
                End If
                isStallLock = True

                Dim objStaffContext As StaffContext = StaffContext.Current

                'チップ配置DB処理
                result = clsTabletSMBCommonClass.MoveAndResize(stallUseId, stallId, scheStartDateTime, _
                                     scheWorkTime, restFlg, stallStartTime, stallEndTime, updatedt, objStaffContext, systemid, updatedt, rowLockVersion)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                '    Me.Rollback = True
                '    Return result
                'End If
                'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success

                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success _
                And result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '共通関数から返却された処理結果コードが下記の場合
                    '    0(成功)以外、かつ
                    '-9000(DMS除外エラーの警告)以外

                    'ロールバック実施
                    Me.Rollback = True

                End If

                Return result

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            Finally
                If isStallLock Then
                    'ストールロック解除
                    clsTabletSMBCommonClass.LockStallReset(stallId, scheStartDateTime, staffcd, updatedt, systemid)
                End If
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.", MethodBase.GetCurrentMethod.Name))
            End Try
        End Using

    End Function

#Region "受付チップを移動、リサイズ、通知・Push"

    '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    ''' <summary>
    ''' 受付チップを移動、リサイズ
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="stallId">変更後のストールのSTALLID</param>
    ''' <param name="ScheStartDateTime">変更後の表示開始日時</param>
    ''' <param name="ScheWorkTime">仕事時間</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <param name="updatedt">更新日時</param>
    ''' <param name="staffcd">スタフコード</param>
    ''' <param name="rowLockVersion">ROWロックバージョン</param>
    ''' <param name="systemId">プログラムID</param>
    ''' <param name="scheDeliDatetime">予定納車日時</param>
    ''' <param name="maintecd">整備コード</param>
    ''' <param name="workSeq">作業連番</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="pickDeliType">納車区分</param>
    ''' <param name="scheSvcinDateTime">予定入庫日時</param>
    ''' <param name="inspectionNeedFlg">検査必要フラグ</param>
    ''' <returns></returns>
    ''' <history>
    ''' 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
    ''' <history>
    ''' 2015/07/17 TMEJ 河原 タブレットSMB性能改善 通知WebService化
    ''' </history>
    ''' 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    Public Function ReceptionChipMoveResize(ByVal svcinId As Decimal, _
                                            ByVal jobDtlId As Decimal, _
                                            ByVal stallUseId As Decimal, _
                                            ByVal stallId As Decimal, _
                                            ByVal scheStartDateTime As Date, _
                                            ByVal scheWorkTime As Long, _
                                            ByVal restFlg As String, _
                                            ByVal stallStartTime As Date, _
                                            ByVal stallEndTime As Date, _
                                            ByVal updatedt As Date, _
                                            ByVal staffcd As String, _
                                            ByVal rowLockVersion As Long, _
                                            ByVal systemId As String, _
                                            ByVal scheDeliDatetime As Date, _
                                            ByVal maintecd As String, _
                                            ByVal workSeq As Long, _
                                            ByVal roNum As String, _
                                            ByVal pickDeliType As String, _
                                            ByVal scheSvcinDateTime As Date, _
                                            ByVal inspectionNeedFlg As String, _
                                            ByVal tempFlg As String) As Long
        'Public Function ReceptionChipMoveResize(ByVal svcinId As Decimal, _
        '                                        ByVal jobDtlId As Decimal, _
        '                                        ByVal stallUseId As Decimal, _
        '                                        ByVal stallId As Decimal, _
        '                                        ByVal scheStartDateTime As Date, _
        '                                        ByVal scheWorkTime As Long, _
        '                                        ByVal restFlg As String, _
        '                                        ByVal stallStartTime As Date, _
        '                                        ByVal stallEndTime As Date, _
        '                                        ByVal updatedt As Date, _
        '                                        ByVal staffcd As String, _
        '                                        ByVal rowLockVersion As Long, _
        '                                        ByVal systemId As String, _
        '                                        ByVal scheDeliDatetime As Date, _
        '                                        ByVal maintecd As String, _
        '                                        ByVal workSeq As Long, _
        '                                        ByVal roNum As String, _
        '                                        ByVal pickDeliType As String, _
        '                                        ByVal scheSvcinDateTime As Date, _
        '                                        ByVal inspectionNeedFlg As String) As Long
        'Public Function ReceptionChipMoveResize(ByVal svcinId As Long, _
        '                                ByVal jobDtlId As Long, _
        '                                ByVal stallUseId As Long, _
        '                                ByVal stallId As Long, _
        '                                ByVal scheStartDateTime As Date, _
        '                                ByVal scheWorkTime As Long, _
        '                                ByVal restFlg As String, _
        '                                ByVal stallStartTime As Date, _
        '                                ByVal stallEndTime As Date, _
        '                                ByVal updatedt As Date, _
        '                                ByVal staffcd As String, _
        '                                ByVal dlrcd As String, _
        '                                ByVal brncd As String, _
        '                                ByVal rowLockVersion As Long, _
        '                                ByVal systemId As String, _
        '                                ByVal scheDeliDatetime As Date, _
        '                                ByVal maintecd As String, _
        '                                ByVal partsFlg As String, _
        '                                ByVal workSeq As Long, _
        '                                ByVal roNum As String, _
        '                                ByVal pickDeliType As String, _
        '                                ByVal scheSvcinDateTime As Date) As Long
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))
        'ストールロックフラグ
        Dim isStallLock As Boolean = False
        Dim result As Long

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START
        Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow
        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try
                'ストールロック
                result = clsTabletSMBCommonClass.LockStall(stallId, scheStartDateTime, staffcd, updatedt, systemId)
                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                    Return TabletSMBCommonClassBusinessLogic.ActionResult.LockStallError
                End If
                isStallLock = True
                Dim objStaffContext As StaffContext = StaffContext.Current

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START
                'Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow
                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'チップの着工指示FLG TRUE:着工指示済み　FALSE:着工指示なし
                Dim chipInstructFlg As Boolean

                'stallUseId=0の時は追加作業新規
                If stallUseId = DefaultNumberValue Then
                    '着工指示なし
                    chipInstructFlg = False
                Else
                    'チップの着工指示状態取得する
                    chipInstructFlg = Me.GetChipInstructFlg(jobDtlId)
                End If
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

                If stallUseId <> DefaultNumberValue Then

                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                    If tempFlg = TEMP Then

                        '更新ロジック
                        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                        'result = clsTabletSMBCommonClass.ReceptionChipMoveUpdate(svcinId, _
                        '                               stallUseId, _
                        '                               stallId, _
                        '                               scheStartDateTime, _
                        '                               scheWorkTime, _
                        '                               restFlg, _
                        '                               stallStartTime, _
                        '                               stallEndTime, _
                        '                               updatedt, _
                        '                               objStaffContext, _
                        '                               systemId, _
                        '                               scheDeliDatetime, _
                        '                               partsFlg, _
                        '                               workSeq, _
                        '                               rowLockVersion, _
                        '                               staffcd, _
                        '                               roNum)

                        '仮置きチップをストールに配置処理
                        drWebServiceResult = clsTabletSMBCommonClass.ToReceptionChipMove(stallUseId, _
                                                                                             stallId, _
                                                                                             jobDtlId, _
                                                                                             scheStartDateTime, _
                                                                                             scheWorkTime, _
                                                                                             restFlg, _
                                                                                             stallStartTime, _
                                                                                             stallEndTime, _
                                                                                             updatedt, _
                                                                                             objStaffContext, _
                                                                                             systemId, _
                                                                                             scheDeliDatetime, _
                                                                                             rowLockVersion, _
                                                                                             staffcd)
                        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                    Else
                        drWebServiceResult = clsTabletSMBCommonClass.ReceptionChipMoveUpdate(stallUseId, _
                                                                     stallId, _
                                                                     scheStartDateTime, _
                                                                     scheWorkTime, _
                                                                     restFlg, _
                                                                     stallStartTime, _
                                                                     stallEndTime, _
                                                                     updatedt, _
                                                                     objStaffContext, _
                                                                     systemId, _
                                                                     scheDeliDatetime, _
                                                                     workSeq, _
                                                                     rowLockVersion, _
                                                                     staffcd)

                    End If
                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

                Else
                    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                    'INSERTロジック
                    'result = clsTabletSMBCommonClass.ReceptionChipMoveInsert(jobDtlId, _
                    '                                                        stallId, _
                    '                                                        scheStartDateTime, _
                    '                                                        scheWorkTime, _
                    '                                                        restFlg, _
                    '                                                        stallStartTime, _
                    '                                                        stallEndTime, _
                    '                                                        updatedt, _
                    '                                                        objStaffContext, _
                    '                                                        systemId, _
                    '                                                        scheDeliDatetime, _
                    '                                                        maintecd, _
                    '                                                        partsFlg, _
                    '                                                        workSeq, _
                    '                                                        pickDeliType, _
                    '                                                        scheSvcinDateTime, _
                    '                                                        rowLockVersion, _
                    '                                                        roNum)
                    drWebServiceResult = clsTabletSMBCommonClass.ReceptionChipMoveInsert(svcinId, _
                                                                                         jobDtlId, _
                                                                                         stallId, _
                                                                                         scheStartDateTime, _
                                                                                         scheWorkTime, _
                                                                                         restFlg, _
                                                                                         stallStartTime, _
                                                                                         stallEndTime, _
                                                                                         updatedt, _
                                                                                         objStaffContext, _
                                                                                         systemId, _
                                                                                         scheDeliDatetime, _
                                                                                         maintecd, _
                                                                                         workSeq, _
                                                                                         pickDeliType, _
                                                                                         scheSvcinDateTime, _
                                                                                         rowLockVersion, _
                                                                                         roNum, _
                                                                                         inspectionNeedFlg)
                    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If drWebServiceResult.RESULTCODE <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
                '    Return drWebServiceResult.RESULTCODE
                '    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                '    'Else
                '    '    '日付が今日の場合TCにPUSH送信
                '    '    If stallStartTime.Date = DateTimeFunc.Now(objStaffContext.DlrCD).Date Then
                '    '        'TCにPUSH送信する(着工指示済)
                '    '        Dim stallList As New List(Of Decimal)
                '    '        stallList.Add(stallId)
                '    '        Dim operationCodeList As New List(Of Decimal)
                '    '        operationCodeList.Add(Operation.TEC)
                '    '        clsTabletSMBCommonClass.SendPushGetReady(dlrcd, brncd, operationCodeList, PUSH_FuntionNM, stallList)
                '    '    End If
                'Else

                If drWebServiceResult.RESULTCODE <> TabletSMBCommonClassBusinessLogic.ActionResult.Success _
                And drWebServiceResult.RESULTCODE <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '共通関数から返却された処理結果コードが下記の場合
                    '    0(成功)以外、かつ
                    '-9000(DMS除外エラーの警告)以外

                    Return drWebServiceResult.RESULTCODE

                Else

                    '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                    ''着工指示済の場合、通知を送らない
                    'If Not chipInstructFlg Then
                    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
                    '着工指示未指示または仮置きの場合、通知を行う
                    'If Not chipInstructFlg Or tempFlg = TEMP Then
                    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
                    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END 


                    '2015/07/17 TMEJ 河原 タブレットSMB性能改善 通知WebService化 START

                    '※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※
                    '※
                    '※ レスポンス問題発生により通知・Push処理のマルチスレッド化を実施
                    '※ 以下の処理をWebService側で行い、WebServiceの呼出しをマルチスレッド化することによりレスポンス改善を図る
                    '※
                    '※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※※

                    ''通知APIを呼ぶ
                    'Using clsTabletSMBCommonClassDataAdpter As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter

                    '    LogServiceCommonBiz.OutputLog(0, "●■● 1.1 TABLETSMBCOMMONCLASS_055 START")

                    '    '情報取得
                    '    Dim dtNoticeInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassNoticeInfoDataTable = clsTabletSMBCommonClassDataAdpter.GetNoticeInfo(svcinId, _
                    '                                                                                   objStaffContext.DlrCD, _
                    '                                                                                   objStaffContext.BrnCD, _
                    '                                                                                   drWebServiceResult.JOB_DTL_ID)

                    '    LogServiceCommonBiz.OutputLog(0, "●■● 1.1 TABLETSMBCOMMONCLASS_055 END")


                    '    '秒を切り捨てる
                    '    Dim truncSecondDispStartDateTime As Date = Me.GetDateTimeFloorSecond(scheStartDateTime)

                    '    Dim serviceWorkEndDateTime As Date

                    '    LogServiceCommonBiz.OutputLog(1, "●■● 1.2 チップ開始時間の計算 START")

                    '    '普通のチップの場合、開始時間を計算する(休憩チップと)
                    '    truncSecondDispStartDateTime = clsTabletSMBCommonClass.GetServiceStartDateTime(stallId, scheStartDateTime, stallStartTime, stallEndTime, restFlg)

                    '    LogServiceCommonBiz.OutputLog(1, "●■● 1.2 チップ開始時間の計算 END")

                    '    LogServiceCommonBiz.OutputLog(3, "●■● 1.3 チップ終了時間の計算開始 START")

                    '    '普通の予約チップの場合、予約終了時間が変わる
                    '    serviceWorkEndDateTime = clsTabletSMBCommonClass.GetServiceEndDateTime(stallId, truncSecondDispStartDateTime, _
                    '                                            scheWorkTime, stallStartTime, stallEndTime, restFlg)

                    '    LogServiceCommonBiz.OutputLog(3, "●■● 1.3 チップ終了時間の計算開始 END")

                    '    If dtNoticeInfo.Count > 0 Then
                    '        Dim drNoticeInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassNoticeInfoRow = dtNoticeInfo(0)

                    '        LogServiceCommonBiz.OutputLog(8, "●■● 1.4 着工指示通知処理 START")

                    '        '通知処理
                    '        clsTabletSMBCommonClass.JobInstructNotice(drNoticeInfo, _
                    '                             objStaffContext, _
                    '                             truncSecondDispStartDateTime, _
                    '                             serviceWorkEndDateTime, _
                    '                             stallId)

                    '        LogServiceCommonBiz.OutputLog(8, "●■● 1.4 着工指示通知処理 END")

                    '        LogServiceCommonBiz.OutputLog(18, "●■● 1.5 着工指示Push処理① START")

                    '        '通知対象にPush
                    '        clsTabletSMBCommonClass.NoticeAccountPush(objStaffContext, stallId)

                    '        LogServiceCommonBiz.OutputLog(18, "●■● 1.5 着工指示Push処理① END")

                    '    End If

                    '    LogServiceCommonBiz.OutputLog(23, "●■● 1.6 着工指示Push処理② START")

                    '    'Push
                    '    clsTabletSMBCommonClass.JobInstructPush(objStaffContext, stallId)

                    '    LogServiceCommonBiz.OutputLog(23, "●■● 1.6 着工指示Push処理② END")

                    'End Using

                    LogServiceCommonBiz.OutputLog(0, "●■● 1.0 通知・PushWebService呼出し START")

                    '★★★
                    '受付エリアチップ移動、リサイズの通知・PushWebService呼出し処理
                    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
                    'Me.CallNoticeWebService(svcinId, jobDtlId, stallUseId, stallId, objStaffContext)
                    Dim beforeInstructFlg As Boolean
                    '変更前チップが着工指示されていないまたは仮置きの場合
                    If Not chipInstructFlg Or TEMP.Equals(tempFlg) Then

                        '着工指示されていない
                        beforeInstructFlg = False
                    Else

                        '着工指示されている
                        beforeInstructFlg = True
                    End If
                    Me.CallNoticeWebService(svcinId, jobDtlId, stallUseId, stallId, objStaffContext, beforeInstructFlg)
                    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END
                    '★★★

                    LogServiceCommonBiz.OutputLog(0, "●■● 1.0 通知・PushWebService呼出し END")

                    '2015/07/17 TMEJ 河原 タブレットSMB性能改善 通知WebService化 END
                    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
                    'End If
                    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END
                    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                End If

            Finally
                If isStallLock Then

                    LogServiceCommonBiz.OutputLog(30, "●■● 1.7 ストールロック解除処理 START")

                    'ストールロック解除
                    clsTabletSMBCommonClass.LockStallReset(stallId, scheStartDateTime, staffcd, updatedt, systemId)

                    LogServiceCommonBiz.OutputLog(30, "●■● 1.7 ストールロック解除処理 END")

                End If
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.", MethodBase.GetCurrentMethod.Name))
            End Try
        End Using

        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START
        'Return TabletSMBCommonClassBusinessLogic.ActionResult.Success
        Return drWebServiceResult.RESULTCODE
        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    '2015/07/17 TMEJ 河原 タブレットSMB性能改善 通知WebService化 START

#Region "WebService用定数"

    ''' <summary>
    ''' 本クラスの名前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MyClassName As String = "SC3240301BusinessLogic"

    ''' <summary>
    ''' 基幹連携URL（部品ステータス情報）
    ''' </summary>
    Private Const DlrSysLinkUrlNoticePushJobInstruc = "NOTICE_PUSH_JOB_INSTRUCT_URL"

    ' ''' <summary>
    ' ''' WebService(IC3040803.asmx)メソッド名
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const WebServiceMethodName As String = "NoticePushJobInstruct"

    ''' <summary>
    ''' WebService(IC3040803.asmx/NoticePushJobInstruct)引数名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceArgumentName As String = "xsData="

    ''' <summary>
    ''' エンコード(UTF-8)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EncodeUtf8 As Integer = 65001

    ''' <summary>
    ''' 送信方法(POST)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Post As String = "POST"

    ''' <summary>
    ''' ContentType
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ContentTypeString As String = "application/x-www-form-urlencoded"

    ''' <summary>
    ''' 日付フォーマット(yyyy/MM/dd HH:mm:ss)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const yyyyMMddHHmmssDateFormat As String = "yyyy/MM/dd HH:mm:ss"


    ''' <summary>
    ''' タグ名：NoticePushJobInstruct
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoticePushJobInstructTagName As String = "NoticePushJobInstruct"


    ''' <summary>
    ''' タグ名：Head
    ''' </summary>
    ''' <remarks></remarks>
    Private Const HeadTagName As String = "Head"

    ''' <summary>
    ''' タグ名：TransmissionDate
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TransmissionDateTagName As String = "TransmissionDate"


    ''' <summary>
    ''' タグ名：Contents
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ContentsTagName As String = "Contents"

    ''' <summary>
    ''' タグ名：DLR_CD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DlrCdTagName As String = "DLR_CD"

    ''' <summary>
    ''' タグ名：BRN_CD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BrnCdTagName As String = "BRN_CD"

    ''' <summary>
    ''' タグ名：SVCIN_ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SvcInIdTagName As String = "SVCIN_ID"

    ''' <summary>
    ''' タグ名：JOB_DTL_ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const JobDtlIdTagName As String = "JOB_DTL_ID"

    ''' <summary>
    ''' タグ名：STALL_USE_ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseIdTagName As String = "STALL_USE_ID"

    ''' <summary>
    ''' タグ名：STALL_ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallIdTagName As String = "STALL_ID"

    ''' <summary>
    ''' タグ名：STAFF_ACCOUNT
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffAccountTagName As String = "STAFF_ACCOUNT"

    ''' <summary>
    ''' タグ名：STAFF_NAME
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffNameTagName As String = "STAFF_NAME"

    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
    ''' <summary>
    ''' タグ名：BEFORE_WORKORDERFLG
    ''' </summary>
    ''' <remarks></remarks>
    Private Const BeforeWorkOrderFlgTagName As String = "BEFORE_WORKORDERFLG"
    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END

    ''' <summary>
    ''' 受信XML用タグ名：Response
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NodeResponse As String = "Response"

    ''' <summary>
    ''' 受信XML用タグ名：ResultId
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TagResultId As String = "ResultId"

#End Region

#Region "通知・PushWebService化処理"

    ''' <summary>
    ''' 受付エリアチップ移動、リサイズの通知・PushWebService呼出し
    ''' </summary>
    ''' <param name="inSvcinId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inBeforeInstructFlg">着工指示フラグ</param>
    ''' <history>
    ''' 2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない
    ''' </history>
    Public Sub CallNoticeWebService(ByVal inSvcinId As Decimal, _
                                    ByVal inJobDtlId As Decimal, _
                                    ByVal inStallUseId As Decimal, _
                                    ByVal inStallId As Decimal, _
                                    ByVal inStaffInfo As StaffContext, _
                                    ByVal inBeforeInstructFlg As Boolean)


        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0} {1}.START IN: SVCIN_ID：{2} JOB_DTL_ID：{3} STALL_USE_ID：{4} STALL_ID：{5}", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inSvcinId, inJobDtlId, inStallUseId, inStallId))

        Try

            'NoticePushJobInstructWebServiceのURL取得
            'SystemEnvSettingのインスタンス
            Dim systemEnvBiz As New SystemEnvSetting

            'SystemEnvSettingの取得値
            Dim webServiceUrl As String = String.Empty

            'SystemEnvSettingの取得処理
            Dim drSystemEnvSetting As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = _
                systemEnvBiz.GetSystemEnvSetting(DlrSysLinkUrlNoticePushJobInstruc)

            'WebServiceのURL取得確認
            If IsNothing(drSystemEnvSetting) Then
                '取得できなかった場合

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0} {1}.ERROR WebServiceURL:EMPTY", _
                                       MyClassName, _
                                       MethodBase.GetCurrentMethod.Name))
                '処理終了
                Exit Try

            Else
                '取得できた場合

                'WebServiceのURL
                webServiceUrl = drSystemEnvSetting.PARAMVALUE

            End If

            '現在日時取得
            Dim presentTime As Date = DateTimeFunc.Now(inStaffInfo.DlrCD)

            '送信XMLの作成
            '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
            'Dim sendXml As XmlDocument = Me.CreateSendXml(inSvcinId, _
            '                                              inJobDtlId, _
            '                                              inStallUseId, _
            '                                              inStallId, _
            '                                              inStaffInfo, _
            '                                              presentTime)
            Dim sendXml As XmlDocument = Me.CreateSendXml(inSvcinId, _
                                                          inJobDtlId, _
                                                          inStallUseId, _
                                                          inStallId, _
                                                          inStaffInfo, _
                                                          presentTime, _
                                                          inBeforeInstructFlg)
            '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END

            '送信XMLを引数に設定
            Dim sendXmlString As String = String.Concat(WebServiceArgumentName, _
                                                        sendXml.InnerXml)

            'マルチスレッド用メソッドの引数作成
            '(マルチスレッドの引数はObjectのみで以下の方法で実装)
            Dim paramObj(1) As Object

            '送信XMLを第一引数に設定
            paramObj(0) = sendXmlString
            'WebServiceのURLを第二引数に設定
            paramObj(1) = webServiceUrl

            '関数をマルチスレッドに設定
            Dim thread As Threading.Thread = New Threading.Thread(New ParameterizedThreadStart(AddressOf MultiThreadCallWebServiceSite))

            'マルチスレッド開始
            '通知・PushWebService呼出し(マルチスレッド用)
            thread.Start(paramObj)

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0} {1}.END SENDDATE：{2} SENDXML：{3}", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  presentTime, sendXmlString))

        Catch ex As Exception

            '通知処理の為、全てのエラーをキャッチしログに出力し正常終了させる
            'エラーログの出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                         , "{0}.{1} OUT:ERROR = {2}" _
                         , Me.GetType.ToString _
                         , MethodBase.GetCurrentMethod.Name _
                         , ex.Message))

        End Try

    End Sub

    ''' <summary>
    ''' 送信用XML作成
    ''' </summary>
    ''' <param name="inSvcinId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <param name="inBeforeInstructFlg">着工指示フラグ</param> 
    ''' <returns>XMLString</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない
    ''' </history>
    Private Function CreateSendXml(ByVal inSvcinId As Decimal, _
                                   ByVal inJobDtlId As Decimal, _
                                   ByVal inStallUseId As Decimal, _
                                   ByVal inStallId As Decimal, _
                                   ByVal inStaffInfo As StaffContext, _
                                   ByVal inPresentTime As Date, _
                                   ByVal inBeforeInstructFlg As Boolean) As XmlDocument

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                 "{0} {1}.START", _
                                 MyClassName, _
                                 MethodBase.GetCurrentMethod.Name))

        '65001がUTF-8
        Dim xmlEncode As Encoding = Encoding.GetEncoding(EncodeUtf8)

        'XMLドキュメント作成
        Dim xmlDocument As New XmlDocument

        'ヘッダ部作成(<?xml version="1.0" encoding="utf-8"?>の部分)
        Dim xmlDeclaration As XmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", xmlEncode.BodyName, Nothing)

        '■■■■ルートタグ■■■■

        'ルートタグ(NoticePushJobInstructタグ)の作成
        Dim xmlRoot As XmlElement = xmlDocument.CreateElement(NoticePushJobInstructTagName)


        '■■■■Headタグ■■■■

        'Headタグを作成
        Dim headTag As XmlElement = xmlDocument.CreateElement(HeadTagName)

        'Headタグの子要素を作成
        '送信日時タグの作成
        Dim TransmissionDateTag As XmlElement = xmlDocument.CreateElement(TransmissionDateTagName)

        '子要素に値を設定
        '送信日時をセット
        TransmissionDateTag.AppendChild(xmlDocument.CreateTextNode(inPresentTime.ToString(yyyyMMddHHmmssDateFormat, CultureInfo.CurrentCulture)))

        'Headタグ内に追加
        '送信日時タグを追加
        headTag.AppendChild(TransmissionDateTag)


        '■■■■Contentsタグ■■■■

        'Contentsタグを作成
        Dim contentsTag As XmlElement = xmlDocument.CreateElement(ContentsTagName)

        'headタグの子要素を作成
        '販売店コードタグの作成
        Dim DlrCdTag As XmlElement = xmlDocument.CreateElement(DlrCdTagName)
        '店舗コードタグの作成
        Dim BrnCdTag As XmlElement = xmlDocument.CreateElement(BrnCdTagName)
        'サービス入庫IDタグの作成
        Dim SvcInIdTag As XmlElement = xmlDocument.CreateElement(SvcInIdTagName)
        '作業内容IDタグの作成
        Dim JobDtlIdTag As XmlElement = xmlDocument.CreateElement(JobDtlIdTagName)
        'ストール利用IDタグの作成
        Dim StallUseIdTag As XmlElement = xmlDocument.CreateElement(StallUseIdTagName)
        'ストールIDタグの作成
        Dim StallIdTag As XmlElement = xmlDocument.CreateElement(StallIdTagName)
        'スタッフアカウントタグの作成
        Dim StaffAccountTag As XmlElement = xmlDocument.CreateElement(StaffAccountTagName)
        'スタッフ名タグの作成
        Dim StaffNameTag As XmlElement = xmlDocument.CreateElement(StaffNameTagName)
        '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
        '着工指示フラグタグの作成
        Dim BeforeWorkOrderFlgTag As XmlElement = xmlDocument.CreateElement(BeforeWorkOrderFlgTagName)
        '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END

        '子要素に値を設定
        '販売店コードをセット
        DlrCdTag.AppendChild(xmlDocument.CreateTextNode(inStaffInfo.DlrCD))
        '店舗コードをセット
        BrnCdTag.AppendChild(xmlDocument.CreateTextNode(inStaffInfo.BrnCD))
        'サービス入庫IDをセット
        SvcInIdTag.AppendChild(xmlDocument.CreateTextNode(CType(inSvcinId, String)))
        '作業内容IDをセット
        JobDtlIdTag.AppendChild(xmlDocument.CreateTextNode(CType(inJobDtlId, String)))
        'ストール利用IDをセット
        StallUseIdTag.AppendChild(xmlDocument.CreateTextNode(CType(inStallUseId, String)))
        'ストールIDをセット
        StallIdTag.AppendChild(xmlDocument.CreateTextNode(CType(inStallId, String)))
        'スタッフアカウントをセット
        StaffAccountTag.AppendChild(xmlDocument.CreateCDataSection(inStaffInfo.Account))
        'スタッフ名をセット
        StaffNameTag.AppendChild(xmlDocument.CreateCDataSection(inStaffInfo.UserName))
        '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
        '着工指示フラグをセット
        If inBeforeInstructFlg Then
            BeforeWorkOrderFlgTag.AppendChild(xmlDocument.CreateTextNode("1"))
        Else
            BeforeWorkOrderFlgTag.AppendChild(xmlDocument.CreateTextNode("0"))
        End If
        '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END

        'Contentsタグ内に追加
        '販売店コードタグを追加
        contentsTag.AppendChild(DlrCdTag)
        '店舗コードタグを追加
        contentsTag.AppendChild(BrnCdTag)
        'サービス入庫IDタグを追加
        contentsTag.AppendChild(SvcInIdTag)
        '作業内容IDタグを追加
        contentsTag.AppendChild(JobDtlIdTag)
        'ストール利用IDタグを追加
        contentsTag.AppendChild(StallUseIdTag)
        'ストールIDタグを追加
        contentsTag.AppendChild(StallIdTag)
        'スタッフアカウントタグを追加
        contentsTag.AppendChild(StaffAccountTag)
        'スタッフ名タグを追加
        contentsTag.AppendChild(StaffNameTag)
        '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
        '着工指示フラグを追加
        contentsTag.AppendChild(BeforeWorkOrderFlgTag)
        '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END
        '■■■■XML全体を作成■■■■

        'ルートタグ内に追加
        'Headタグを追加
        xmlRoot.AppendChild(headTag)
        'Contentsタグを追加
        xmlRoot.AppendChild(contentsTag)


        '■■■■最終整形■■■■

        '送信用XMLを構築
        xmlDocument.AppendChild(xmlDeclaration)
        xmlDocument.AppendChild(xmlRoot)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0} {1}.END OUT:STRUCTXML = " & vbCrLf & "{2}", _
                                  MyClassName, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  xmlDocument.InnerXml))

        Return xmlDocument


    End Function

    ''' <summary>
    ''' 通知・PushWebService呼出し(マルチスレッド用)
    ''' WebServiceのサイトを呼出
    ''' WebServiceを送信し結果を受信する
    ''' </summary>
    ''' <param name="param">配列(0：SendXML,1：URL)</param>
    ''' <returns>結果XML</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function MultiThreadCallWebServiceSite(ByVal param As Object) As String

        '以降の処理はマルチスレッドであり
        'ログの出力が不可能


        '返却文字列(応答XML文字列をHTMLデコード)
        Dim retString As String = String.Empty

        Try

            Dim obj() As Object = DirectCast(param, Object())

            '文字コードを指定する
            Dim enc As System.Text.Encoding = _
                System.Text.Encoding.GetEncoding(EncodeUtf8)

            'バイト型配列に変換
            Dim postDataBytes As Byte() = _
                System.Text.Encoding.UTF8.GetBytes(DirectCast(obj(0), String))

            'WebRequestの作成
            Dim req As WebRequest = WebRequest.Create(DirectCast(obj(1), String))

            With req

                req.Method = Post                           'メソッドにPOSTを指定
                req.ContentType = ContentTypeString         'ContentType指定(固定)
                req.ContentLength = postDataBytes.Length    'POST送信するデータの長さを指定
                'req.Timeout = デフォルト                   '送信タイムアウト値設定

            End With

            'データをPOST送信するためのStreamを取得
            Using reqStream As Stream = req.GetRequestStream()

                '送信するデータを書き込む
                reqStream.Write(postDataBytes, 0, postDataBytes.Length)

            End Using

            '応答XML文字列
            Dim responseString As String = String.Empty

            'サーバーからの応答を受信するためのWebResponseを取得
            Dim resultResponse As WebResponse = req.GetResponse()

            '応答データを受信するためのStreamを取得
            Dim resultStream As Stream = resultResponse.GetResponseStream()

            '受信して表示
            Using resultReader As New StreamReader(resultStream, enc)

                '応答XML文字列を取得
                responseString = resultReader.ReadToEnd()

            End Using

            '応答XML文字列をHTMLデコードする
            retString = HttpUtility.HtmlDecode(responseString)


            'XML名前空間用の正規表現設定
            Dim regex As Regex = New Regex(" xmln.*=""[^""]*"".")

            'XML名前空間を除去
            Dim editResultString As String = regex.Replace(retString, Space(0))

            '結果コード(0：正常、0以外：失敗)
            Dim retCode As Long = 0

            '受信XMLを解析
            retCode = Me.GetXMLData(editResultString)


        Catch ex As Exception

            'マルチスレッド対応の為、全てのExceptionキャッチする
            'ログが出力できない為、処理無し

        End Try

        '返却文字列を返却
        Return retString

    End Function

    ''' <summary>
    ''' WebServiceの戻りXMLを解析し値を取得
    ''' </summary>
    ''' <param name="resultString">受信XML文字列</param>
    ''' <returns>結果コード(0：正常、0以外：失敗)</returns>
    ''' <remarks></remarks>
    Private Function GetXMLData(ByVal resultString As String) As Long

        '結果コード(0：正常、0以外：失敗)
        Dim retCode As Long = -1

        Try

            'XmlDocument
            Dim resultXmlDocument As New XmlDocument

            '返却された文字列をXML化
            resultXmlDocument.LoadXml(resultString)

            'XmlElementを取得
            Dim resultXmlElement As XmlElement = resultXmlDocument.DocumentElement

            'XmlElementの確認
            If resultXmlElement Is Nothing Then
                '取得失敗

                Return retCode

            End If

            '子ノードリストの取得
            Dim resultXmlNodeList As XmlNodeList = resultXmlElement.GetElementsByTagName(NodeResponse)

            '子ノードリストの確認
            If resultXmlNodeList Is Nothing OrElse resultXmlNodeList.Count = 0 Then
                '取得失敗

                Return retCode

            End If


            '子ノードの取得
            Dim resultXmlNode As XmlNode = resultXmlNodeList.Item(0)

            '解析したXMLから設定されている値の取得
            retCode = GetXmlNodeValue(resultXmlNode)

        Catch ex As XmlException

            'ログが出力できない為、処理無し

        End Try

        Return retCode

    End Function

    ''' <summary>
    ''' 戻りXMLから設定されている値を取得
    ''' </summary>
    ''' <param name="resultXmlNode">受信XMLノード</param>
    ''' <returns>結果コード(0：正常、0以外：失敗)</returns>
    ''' <remarks></remarks>
    Private Function GetXmlNodeValue(ByVal resultXmlNode As XmlNode) As Long

        '結果コード(0：正常、0以外：失敗)
        Dim retCode As Long = -1

        'ResultCodeタグの値取得
        retCode = GetTagValue(resultXmlNode, TagResultId)

        'WEBServiceの処理結果確認
        If retCode <> 0 Then
            '処理結果が失敗

            Return retCode

        End If

        Return retCode

    End Function

    ''' <summary>
    ''' Tagから値を取得
    ''' </summary>
    ''' <param name="resultXmlNode">受信XMLノード</param>
    ''' <param name="tagName">Tag名</param>
    ''' <returns>結果XML</returns>
    ''' <remarks></remarks>
    Private Function GetTagValue(ByVal resultXmlNode As XmlNode, _
                                 ByVal tagName As String) As Long

        '処理結果
        Dim resultValue As Long = -1

        'タグの取得
        Dim selectNodeList As XmlNodeList = resultXmlNode.SelectNodes(tagName)

        'タグの確認
        If selectNodeList Is Nothing OrElse selectNodeList.Count = 0 Then
            '取得失敗

            'コードに-1を設定
            Return resultValue

        End If

        '値の取得
        Dim tagValue As String = selectNodeList.Item(0).InnerText.Trim

        '取得した値をLongに変換
        If Not Long.TryParse(tagValue, resultValue) Then
            'Longに変換できなかった場合

            'コードに-1を設定
            Return -1

        End If

        Return resultValue

    End Function

#End Region

    '2015/07/17 TMEJ 河原 タブレットSMB性能改善 通知WebService化 END

#End Region



    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' NoShowチップ再配置プッシュ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub NoShowChipMovePush()
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        Dim userContext As StaffContext = StaffContext.Current
        'Push
        Dim pushUsersList As New List(Of String)
        Dim operationCodeList As New List(Of Decimal)
        Dim exceptStaffCodeList As New List(Of String)
        'SVR権限を追加
        operationCodeList.Add(Operation.SVR)
        Using bizTabletSmbComm As New TabletSMBCommonClassBusinessLogic
            'SVR権限のユーザーを取得
            pushUsersList = bizTabletSmbComm.GetSendStaffCode(userContext.DlrCD, userContext.BrnCD, operationCodeList, exceptStaffCodeList)
            'ユーザーリストに対してPUSHする
            bizTabletSmbComm.SendPushByStaffCodeList(pushUsersList, PUSH_FuntionSVR)
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.", MethodBase.GetCurrentMethod.Name))
    End Sub
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

#Region "店舗設定の取得"
    ''' <summary>
    ''' 店舗稼動時間情報の取得
    ''' </summary>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <returns>店舗稼動時間情報テーブル</returns>
    Public Function GetBranchOperatingHours(ByVal objStaffContext As StaffContext) As TabletSMBCommonClassDataSet.TabletSmbCommonClassBranchOperatingHoursDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim dt As TabletSMBCommonClassDataSet.TabletSmbCommonClassBranchOperatingHoursDataTable

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            dt = clsTabletSMBCommonClass.GetBranchOperatingHours(objStaffContext.DlrCD, objStaffContext.BrnCD)
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dt
    End Function

#End Region

    ''' <summary>
    ''' サービス入庫IDにより、関連チップ情報を取得
    ''' </summary>
    ''' <param name="svcInIdList">サービス入庫IDリスト</param>
    ''' <returns></returns>
    Public Function GetAllRelationChipInfo(ByVal svcInIdList As List(Of Decimal)) As TabletSMBCommonClassDataSet.TabletSmbCommonClassRelationChipInfoDataDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using ta As New TabletSMBCommonClassBusinessLogic
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ta.GetAllRelationChipInfo(svcInIdList)
        End Using
    End Function

    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
    ' ''' <summary>
    ' ''' 指定日時から変更されたチップの情報を取得します
    ' ''' </summary>
    ' ''' <param name="dtNow">今の時間</param>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <param name="svcinIdList">サービス入庫IDリスト</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetStallChipBySvcinId(ByVal dtNow As Date, _
    '                                        ByVal objStaffContext As StaffContext, _
    '                                        ByVal svcinIdList As List(Of Decimal)) As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = Nothing
    '    Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
    '        dtChipInfo = clsTabletSMBCommonClass.GetStallChipBySvcinId(objStaffContext.DlrCD, objStaffContext.BrnCD, dtNow, svcinIdList)
    '    End Using

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    Return dtChipInfo
    'End Function

    ''' <summary>
    ''' 各操作後、ストール上更新されたチップの情報を取得
    ''' </summary>
    ''' <param name="inDlrCode">販売店コード</param>
    ''' <param name="inBrnCode">店舗コード</param>
    ''' <param name="inShowDate">画面に表示されてる日時</param>
    ''' <param name="inLastRefreshTime">最新の更新日時</param>
    ''' <returns>最新のチップ情報</returns>
    ''' <remarks></remarks>
    Public Function GetStallChipAfterOperation(ByVal inDlrCode As String, _
                                               ByVal inBrnCode As String, _
                                               ByVal inShowDate As Date, _
                                               ByVal inLastRefreshTime As Date) As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture _
                                , "{0}.Start. inDlrCode={1}, inBrnCode={2}, inShowDate={3}, inLastRefreshTime={4}" _
                                , Reflection.MethodBase.GetCurrentMethod.Name _
                                , inDlrCode _
                                , inBrnCode _
                                , inShowDate _
                                , inLastRefreshTime))


        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            Dim dtChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = _
                clsTabletSMBCommonClass.GetStallChipAfterOperation(inDlrCode, _
                                                                   inBrnCode, _
                                                                   inShowDate, _
                                                                   inLastRefreshTime)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.End. Chip count is = {1}", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dtChipInfo.Count))
            Return dtChipInfo

        End Using


    End Function

    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

#End Region

#Region "Privateメソッド"
    ''' <summary>
    ''' 紐付いているRO情報を作成
    ''' </summary>
    ''' <param name="dtJobInstruct">紐付いているRO情報</param>
    ''' <returns>Dictionary(R/O番号,枝番リスト)</returns>
    ''' <remarks></remarks>
    Private Function CreateJobInstructInfo(ByVal dtJobInstruct As SC3240301DataSet.SC3240301JobInstructInfoDataTable) _
                                                                                    As Dictionary(Of Decimal, List(Of Long))
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        Dim rtJobInstruct As New Dictionary(Of Decimal, List(Of Long))
        Dim roSeqList As New List(Of Long)

        'ループで全チップ分の紐付いているRO情報を作成(dtJobInstructは作業内容IDでソート済)
        For Each drJobInstruct As SC3240301DataSet.SC3240301JobInstructInfoRow In dtJobInstruct
            '同じR/O番号の場合、枝番をリスト追加
            If rtJobInstruct.ContainsKey(drJobInstruct.JOB_DTL_ID) Then
                roSeqList.Add(drJobInstruct.RO_JOB_SEQ)
                rtJobInstruct.Item(drJobInstruct.JOB_DTL_ID) = roSeqList
                Continue For
            End If
            roSeqList = New List(Of Long)
            roSeqList.Add(drJobInstruct.RO_JOB_SEQ)
            rtJobInstruct.Add(drJobInstruct.JOB_DTL_ID, roSeqList)
        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.", MethodBase.GetCurrentMethod.Name))
        Return rtJobInstruct
    End Function

    ''' <summary>
    ''' ディフォルト値(DB日付型の既定値（1900-1-1 00:00:00）)を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DefaultDateTimeValue() As Date
        Return Date.Parse("1900/01/01 00:00:00", CultureInfo.InvariantCulture)
    End Function

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 秒を切り捨てた日時を取得する.
    ''' </summary>
    ''' <param name="target">対象日時</param>
    ''' <returns>秒を切り捨てた日時</returns>
    Private Function GetDateTimeFloorSecond(ByVal target As DateTime) As DateTime
        Return New DateTime(target.Year, target.Month, target.Day, target.Hour, target.Minute, 0)
    End Function

    ''' <summary>
    ''' チップが着工指示済かどうかを取得する
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetChipInstructFlg(ByVal inJobDtlId As Decimal) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))
        '返却用変数
        Dim JobInstructFlg As Boolean = False
        Using tabletSmbCommonDataAdapter As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
            'チップの着工指示状態を取得する
            Dim dt As TabletSMBCommonClassDataSet.TabletSmbCommonClassJobInstructDataTable = _
                tabletSmbCommonDataAdapter.GetJobInstructIdAndSeqByJobDtlId(inJobDtlId)
            If dt.Count > 0 Then
                '紐付く作業があった場合
                JobInstructFlg = True
            End If
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.", MethodBase.GetCurrentMethod.Name))
            Return JobInstructFlg
        End Using
    End Function

#Region "遅れ見込み計算"
    ''' <summary>
    ''' 遅れ見込み計算
    ''' </summary>
    ''' <param name="inSvcInIdList">サービス入庫IDリスト</param>
    ''' <returns>遅れ見込みディクショナリー(サービス入庫ID,遅れ見込情報)</returns>
    ''' <remarks></remarks>
    ''' <history>2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一</history>
    Private Function GetDeliDelayDateTime(ByVal inSvcInIdList As List(Of Decimal)) As Dictionary(Of Decimal, TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        Dim userContext As StaffContext = StaffContext.Current
        Dim dlrCode As String = userContext.DlrCD
        Dim brnCode As String = userContext.BrnCD
        Dim nowDate As Date = DateTimeFunc.Now(dlrCode)
        Using combiz As New TabletSMBCommonClassBusinessLogic
            '遅れ見込み情報を取得する
            Dim dtDeliDelay As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateDataTable = _
                combiz.GetDeliveryDelayDateList(inSvcInIdList, dlrCode, brnCode, nowDate)
            '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
            'Dim deliDelayData As New Dictionary(Of Decimal, Date)
            Dim deliDelayData As New Dictionary(Of Decimal, TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow)
            '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            '取得した情報をもとに遅れ見込みディクショナリー(サービス入庫ID,遅れ見込み日時)を作成する
            For Each dr As TabletSMBCommonClassDataSet.TabletSmbCommonClassDeliDelayDateRow In dtDeliDelay
                If Not deliDelayData.ContainsKey(dr.SVCIN_ID) AndAlso Not dr.IsDELI_DELAY_DATETIMENull Then
                    '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    'deliDelayData.Add(dr.SVCIN_ID, dr.DELI_DELAY_DATETIME)
                    'サービス入庫IDに紐づくデータ行をディクショナリの値にする
                    deliDelayData.Add(dr.SVCIN_ID, dr)
                    '2017/10/19 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                End If
            Next
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return deliDelayData
        End Using
    End Function
    '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
    ''' <summary>
    ''' 遅れ見込み計算量を少ない為、データを１回で絞り込む
    ''' </summary>
    ''' <param name="scheDeliDatetime">予定納車日時</param>
    ''' <param name="nowDate">今の時刻</param>
    ''' <returns>TRUE:要るデータ、FALSE:要らないデータ</returns>
    ''' <remarks></remarks>
    'Private Function NarrowDownForDelayCompute(ByVal scheDeliDatetime As Date, _
    '                                           ByVal nowDate As Date) As Boolean
    '    '予定納車時間があるかつ今の時間より大きいの場合、遅れ見込み計算が要る
    '    If scheDeliDatetime <> DefaultDateTimeValue() _
    '        AndAlso DateDiff("n", scheDeliDatetime, nowDate) <= DefaultNumberValue Then
    '        Return True
    '    Else
    '        Return False
    '    End If
    'End Function
    '2017/10/20 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
#End Region

    ' ''' <summary>
    ' ''' ディフォルト値(DB日付型の既定値（1900-1-1 00:00:00）)を判断する
    ' ''' </summary>
    ' ''' <param name="para"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Private Function IsDefaultValue(ByVal para As Date) As Boolean
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. para={1}" _
    '        , MethodBase.GetCurrentMethod.Name, para))

    '    If para = Date.Parse("1900/01/01 00:00:00", CultureInfo.InvariantCulture) Then
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return True", MethodBase.GetCurrentMethod.Name))
    '        Return True
    '    Else
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return False", MethodBase.GetCurrentMethod.Name))
    '        Return False
    '    End If
    'End Function

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
#Region "部品準備フラグ取得"
    ''' <summary>
    ''' 部品準備フラグ取得(メイン)
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="dtSubChip">サブボックスチップデータテーブル</param>
    ''' <param name="subChipAreaId">サブボックスID</param>
    ''' <returns>渡されたテーブルに部品準備フラグを設定したデータテーブル</returns>
    ''' <remarks>追加作業エリア、NoShowエリア取得しない</remarks>
    Private Function GetPartsFlg(ByVal dlrCode As String, _
                                 ByVal brnCode As String, _
                                 ByVal dtSubChip As SC3240301DataSet.SC3240301SubChipInfoDataTable, _
                                 ByVal subChipAreaId As String) _
                                    As SC3240301DataSet.SC3240301SubChipInfoDataTable
        '2018/12/21 NSK 坂本 TR-SVT-TMT-20180421-001 サービスタブレットのレスポンスが全画面で遅延している_工程管理でPS01を呼び出さないようにしたい START
        'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        'Using dtRONumInfo As New IC3802503RONumInfoDataTable
        '    Select Case subChipAreaId
        '        Case C_RECEPTION

        '            '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
        '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} ⑫SC3240301_受付エリアからのチップ配置処理 [部品ステータス情報取得] START" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '            '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

        '            '受付エリアの場合
        '            dtSubChip = Me.GetPartsFlgRecetion(dlrCode, _
        '                                               brnCode, _
        '                                               dtSubChip)

        '            '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
        '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} ⑫SC3240301_受付エリアからのチップ配置処理 [部品ステータス情報取得] END" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '            '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

        '        Case C_STOP, C_COMPLETIONINSPECTION
        '            '完成検査、中断エリアの場合
        '            dtSubChip = Me.GetPartsFlgCompleteStop(dlrCode, _
        '                                               brnCode, _
        '                                               dtSubChip)
        '        Case C_CARWASH, C_DELIVERDCAR
        '            '洗車待ち、納車待ちエリアの場合
        '            dtSubChip = Me.GetPartsFlgDeliWash(dlrCode, _
        '                                            brnCode, _
        '                                            dtSubChip)
        '    End Select
        'End Using

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.", MethodBase.GetCurrentMethod.Name)
        '2018/12/21 NSK 坂本 TR-SVT-TMT-20180421-001 サービスタブレットのレスポンスが全画面で遅延している_工程管理でPS01を呼び出さないようにしたい END

        Return dtSubChip
    End Function

    ''' <summary>
    ''' 部品準備フラグ取得(受付エリア)
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="dtSubChip">サブボックスチップデータテーブル</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPartsFlgRecetion(ByVal dlrCode As String, _
                                 ByVal brnCode As String, _
                                 ByVal dtSubChip As SC3240301DataSet.SC3240301SubChipInfoDataTable) As SC3240301DataSet.SC3240301SubChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        Using dtRONumInfo As New IC3802503RONumInfoDataTable
            '全チップ分のR/O情報を作成
            For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dtSubChip
                If String.IsNullOrWhiteSpace(dr.RO_NUM) Then
                    Continue For
                End If
                Dim drRONumInfo As IC3802503RONumInfoRow = dtRONumInfo.NewIC3802503RONumInfoRow
                drRONumInfo.R_O = dr.RO_NUM
                drRONumInfo.R_O_SEQNO = dr.RO_JOB_SEQ.ToString(CultureInfo.InvariantCulture)

                dtRONumInfo.AddIC3802503RONumInfoRow(drRONumInfo)
            Next
            If dtRONumInfo.Count > 0 Then
                '部品ステータス取得するAPIをインスタンス化
                Using ic3802503bl As New IC3802503BusinessLogic
                    '部品ステータスを取得する(全チップ分)
                    Dim dtPartsStatus As IC3802503PartsStatusDataTable = ic3802503bl.GetPartsStatusList(dlrCode, _
                                                                                                        brnCode, _
                                                                                                        dtRONumInfo)


                    If Not IsNothing(dtPartsStatus) AndAlso dtPartsStatus.Count > 0 Then
                        '取得する成功した場合、返却テーブルにループで部品準備フラグを設定する

                        ' 全部品ステータス情報をログ出力
                        Me.OutPutIFLog(dtPartsStatus, "IC3802503PartsStatusDataTable")

                        'GetPartsStatusList操作がエラー発生したかチェック
                        If dtPartsStatus(0).ResultCode <> IC3802503BusinessLogic.Result.Success Then
                            Dim errCode As Long = TabletSMBCommonClassBusinessLogic.ActionResult.IC3802503ResultOtherError
                            Select Case dtPartsStatus(0).ResultCode
                                Case IC3802503BusinessLogic.Result.TimeOutError
                                    errCode = TabletSMBCommonClassBusinessLogic.ActionResult.IC3802503ResultTimeOutError
                                Case IC3802503BusinessLogic.Result.DmsError
                                    errCode = TabletSMBCommonClassBusinessLogic.ActionResult.IC3802503ResultDmsError
                                Case IC3802503BusinessLogic.Result.OtherError
                                    errCode = TabletSMBCommonClassBusinessLogic.ActionResult.IC3802503ResultOtherError
                            End Select

                            Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                                   "{0}.Error, get PartsStatus error.", _
                                   MethodBase.GetCurrentMethod.Name))

                            dtSubChip(0).PARTS_FLG = errCode.ToString(CultureInfo.InvariantCulture)
                            Return dtSubChip
                        End If

                        For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dtSubChip
                            '取得した部品ステータス情報を絞込
                            Dim drPartsStatus As IC3802503PartsStatusRow() = CType(dtPartsStatus.Select( _
                                String.Format(CultureInfo.InvariantCulture, "R_O='{0}' AND R_O_SEQNO={1} AND PARTS_ISSUE_STATUS='{2}'", _
                                              dr.RO_NUM, _
                                              dr.RO_JOB_SEQ, _
                                              Parts_Issued_Completely)),  _
                                IC3802503PartsStatusRow())

                            '未完了の場合
                            If drPartsStatus.Count = 0 Then
                                dr.PARTS_FLG = PartsFlg_NoIssue
                            Else
                                '準備完了の場合
                                dr.PARTS_FLG = PartsFlg_Completely
                            End If
                        Next
                    End If
                End Using
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.", MethodBase.GetCurrentMethod.Name))
        Return dtSubChip
    End Function

    ''' <summary>
    ''' 部品準備フラグ取得(中断エリア、完成検査エリア)
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="dtSubChip">サブボックスチップデータテーブル</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPartsFlgCompleteStop(ByVal dlrCode As String, _
                                 ByVal brnCode As String, _
                                 ByVal dtSubChip As SC3240301DataSet.SC3240301SubChipInfoDataTable) As SC3240301DataSet.SC3240301SubChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        Using dtRONumInfo As New IC3802503RONumInfoDataTable
            '全チップ分のR/O情報を作成
            For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dtSubChip
                If String.IsNullOrWhiteSpace(dr.RO_NUM) Then
                    Continue For
                End If
                Dim drRONumInfo As IC3802503RONumInfoRow = dtRONumInfo.NewIC3802503RONumInfoRow
                drRONumInfo.R_O = dr.RO_NUM

                dtRONumInfo.AddIC3802503RONumInfoRow(drRONumInfo)
            Next
            If dtRONumInfo.Count > 0 Then
                Dim dtPartsStatus As IC3802503PartsStatusDataTable
                '部品ステータス取得するAPIをインスタンス化
                Using ic3802503bl As New IC3802503BusinessLogic
                    '部品ステータスを取得する(全チップ分)
                    dtPartsStatus = ic3802503bl.GetPartsStatusList(dlrCode, _
                                                                   brnCode, _
                                                                   dtRONumInfo)
                End Using

                If Not IsNothing(dtPartsStatus) AndAlso dtPartsStatus.Count > 0 Then
                    ' 全部品ステータス情報をログ出力
                    Me.OutPutIFLog(dtPartsStatus, "IC3802503PartsStatusDataTable")

                    'GetPartsStatusList操作がエラー発生したかチェック
                    If dtPartsStatus(0).ResultCode <> IC3802503BusinessLogic.Result.Success Then
                        Dim errCode As Long = TabletSMBCommonClassBusinessLogic.ActionResult.IC3802503ResultOtherError
                        Select Case dtPartsStatus(0).ResultCode
                            Case IC3802503BusinessLogic.Result.TimeOutError
                                errCode = TabletSMBCommonClassBusinessLogic.ActionResult.IC3802503ResultTimeOutError
                            Case IC3802503BusinessLogic.Result.DmsError
                                errCode = TabletSMBCommonClassBusinessLogic.ActionResult.IC3802503ResultDmsError
                            Case IC3802503BusinessLogic.Result.OtherError
                                errCode = TabletSMBCommonClassBusinessLogic.ActionResult.IC3802503ResultOtherError
                        End Select

                        Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                               "{0}.Error, get PartsStatus error.", _
                               MethodBase.GetCurrentMethod.Name))

                        dtSubChip(0).PARTS_FLG = errCode.ToString(CultureInfo.InvariantCulture)
                        Return dtSubChip
                    End If

                    '全チップ分のRO紐付き情報
                    Dim jobInstruct As New Dictionary(Of Decimal, List(Of Long))
                    Dim dtJobInstruct As SC3240301DataSet.SC3240301JobInstructInfoDataTable
                    Using SC3240301da As New SC3240301DataSetTableAdapters.SC3240301StallInfoDataTableAdapter
                        dtJobInstruct = SC3240301da.GetJobInstructInfo(dtSubChip)
                        jobInstruct = Me.CreateJobInstructInfo(dtJobInstruct)
                    End Using

                    '取得する成功した場合、返却テーブルにループで部品準備フラグを設定する
                    For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dtSubChip
                        If Not jobInstruct.ContainsKey(dr.JOB_DTL_ID) Then
                            dr.PARTS_FLG = PartsFlg_NoIssue
                            Continue For
                        End If
                        '取得した部品ステータス情報を絞込
                        Dim drPartsStatus As IC3802503PartsStatusRow() = CType(dtPartsStatus.Select( _
                            String.Format(CultureInfo.InvariantCulture, "R_O='{0}' AND R_O_SEQNO IN({1})", _
                                          dr.RO_NUM, _
                                          String.Join(", ", jobInstruct.Item(dr.JOB_DTL_ID).ToArray()))),  _
                            IC3802503PartsStatusRow())

                        '部品なしか、部品ステータスが「8」以外の場合
                        If drPartsStatus.Count = 0 Then
                            dr.PARTS_FLG = PartsFlg_NoIssue
                        Else
                            dr.PARTS_FLG = PartsFlg_Completely
                            For Each drChipPartsStatus As IC3802503PartsStatusRow In drPartsStatus
                                If Not Parts_Issued_Completely.Equals(drChipPartsStatus.PARTS_ISSUE_STATUS) Then
                                    '部品準備一つでもできてない場合
                                    dr.PARTS_FLG = PartsFlg_NoIssue
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                End If
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.", MethodBase.GetCurrentMethod.Name))
        Return dtSubChip
    End Function

    ''' <summary>
    ''' 部品準備フラグ取得(洗車待ち、納車待ちエリア)
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="dtSubChip">サブボックスチップデータテーブル</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPartsFlgDeliWash(ByVal dlrCode As String, _
                                 ByVal brnCode As String, _
                                 ByVal dtSubChip As SC3240301DataSet.SC3240301SubChipInfoDataTable) As SC3240301DataSet.SC3240301SubChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        Using dtRONumInfo As New IC3802503RONumInfoDataTable
            '全チップ分のR/O情報を作成
            For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dtSubChip
                If String.IsNullOrWhiteSpace(dr.RO_NUM) Then
                    Continue For
                End If

                Dim drRONumInfo As IC3802503RONumInfoRow = dtRONumInfo.NewIC3802503RONumInfoRow
                drRONumInfo.R_O = dr.RO_NUM

                dtRONumInfo.AddIC3802503RONumInfoRow(drRONumInfo)
            Next

            If dtRONumInfo.Count > 0 Then
                Dim dtPartsStatus As IC3802503PartsStatusDataTable
                '部品ステータス取得するAPIをインスタンス化
                Using ic3802503bl As New IC3802503BusinessLogic
                    '部品ステータスを取得する(全チップ分)
                    dtPartsStatus = ic3802503bl.GetPartsStatusList(dlrCode, _
                                                                   brnCode, _
                                                                   dtRONumInfo)

                End Using


                If Not IsNothing(dtPartsStatus) AndAlso dtPartsStatus.Count > 0 Then

                    ' 全部品ステータス情報をログ出力
                    Me.OutPutIFLog(dtPartsStatus, "IC3802503PartsStatusDataTable")

                    'GetPartsStatusList操作がエラー発生したかチェック
                    If dtPartsStatus(0).ResultCode <> IC3802503BusinessLogic.Result.Success Then
                        Dim errCode As Long = TabletSMBCommonClassBusinessLogic.ActionResult.IC3802503ResultOtherError
                        Select Case dtPartsStatus(0).ResultCode
                            Case IC3802503BusinessLogic.Result.TimeOutError
                                errCode = TabletSMBCommonClassBusinessLogic.ActionResult.IC3802503ResultTimeOutError
                            Case IC3802503BusinessLogic.Result.DmsError
                                errCode = TabletSMBCommonClassBusinessLogic.ActionResult.IC3802503ResultDmsError
                            Case IC3802503BusinessLogic.Result.OtherError
                                errCode = TabletSMBCommonClassBusinessLogic.ActionResult.IC3802503ResultOtherError
                        End Select

                        Logger.Warn(String.Format(CultureInfo.InvariantCulture, _
                               "{0}.Error, get PartsStatus error.", _
                               MethodBase.GetCurrentMethod.Name))
                        dtSubChip(0).PARTS_FLG = errCode.ToString(CultureInfo.InvariantCulture)
                        Return dtSubChip
                    End If

                    '取得する成功した場合、返却テーブルにループで部品準備フラグを設定する
                    For Each dr As SC3240301DataSet.SC3240301SubChipInfoRow In dtSubChip
                        '取得した部品ステータス情報を絞込
                        Dim drPartsStatus As IC3802503PartsStatusRow() = CType(dtPartsStatus.Select( _
                            String.Format(CultureInfo.InvariantCulture, "R_O='{0}'", _
                                          dr.RO_NUM)),  _
                            IC3802503PartsStatusRow())

                        '部品なしか、部品ステータスが「8」以外の場合
                        If drPartsStatus.Count = 0 Then
                            dr.PARTS_FLG = PartsFlg_NoIssue
                        Else
                            dr.PARTS_FLG = PartsFlg_Completely
                            For Each drChipPartsStatus As IC3802503PartsStatusRow In drPartsStatus
                                If Not Parts_Issued_Completely.Equals(drChipPartsStatus.PARTS_ISSUE_STATUS) Then
                                    '部品準備一つでもできてない場合
                                    dr.PARTS_FLG = PartsFlg_NoIssue
                                    Exit For
                                End If
                            Next
                        End If
                    Next
                End If
            End If
        End Using
        ' 全部品ステータス情報をログ出力
        Me.OutPutIFLog(dtSubChip, "SC3240301SubChipInfoDataTable")
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E.", MethodBase.GetCurrentMethod.Name))
        Return dtSubChip
    End Function
#End Region
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

#Region "ログ出力用"
    ' ''' <summary>
    ' ''' DataRow内の項目を列挙(ログ出力用)
    ' ''' </summary>
    ' ''' <param name="args">ログ項目のコレクション</param>
    ' ''' <param name="row">対象となるDataRow</param>
    ' ''' <remarks></remarks>
    'Private Sub AddLogData(ByVal args As List(Of String), ByVal row As DataRow)
    '    If row Is Nothing Then
    '        Return
    '    End If
    '    For Each column As DataColumn In row.Table.Columns
    '        If row.IsNull(column.ColumnName) Then
    '            args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
    '        Else
    '            args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, row(column.ColumnName)))
    '        End If
    '    Next
    'End Sub

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

        Logger.Info(MY_PROGRAMID & ".ascx " & ifName + " Result START " + " OutPutCount: " + (dt.Rows.Count).ToString(CultureInfo.InvariantCulture))

        Dim log As New Text.StringBuilder

        For j = 0 To dt.Rows.Count - 1

            log = New Text.StringBuilder()
            Dim dr As DataRow = dt.Rows(j)

            log.Append("RowNum: " + (j + 1).ToString(CultureInfo.InvariantCulture) + " -- ")

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

        Logger.Info(MY_PROGRAMID & ".ascx " & ifName + " Result END ")

    End Sub
#End Region

#End Region

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    '#Region "商品IDを取得する"
    '    ''' <summary>
    '    ''' 商品IDを取得する
    '    ''' </summary>
    '    ''' <param name="dlrCode">販売店コード</param>
    '    ''' <param name="mntnCode">整備コード</param>
    '    ''' <param name="vin">VIN</param>
    '    ''' <returns>商品ID</returns>
    '    ''' <remarks></remarks>
    '    Private Function GetMercId(ByVal dlrCode As String, ByVal mntnCode As String, ByVal vin As String) As Decimal
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

    '        Using commBiz As New TabletSMBCommonClassBusinessLogic
    '            '商品IDを取得する
    '            Dim mercId As Decimal = commBiz.GetMercId(dlrCode, mntnCode, vin)
    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    '            Return mercId
    '        End Using
    '    End Function
    '#End Region

    '#Region "承認待ちのRO情報を取得する商品IDを取得する"
    '    ''' <summary>
    '    ''' 承認待ちのRO情報を取得する商品IDを取得する
    '    ''' </summary>
    '    ''' <param name="dlrCode">販売店コード</param>
    '    ''' <returns>承認待ちのRO情報</returns>
    '    ''' <remarks></remarks>
    '    Private Function GetWaittConfirOrderInfo(ByVal dlrCode As String) As IC3801013DataSet.IC3801013ROReserveInfoDataTable
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))
    '        Dim biz As New IC3801013BusinessLogic
    '        Try
    '            '承認待ちのRO情報を取得する
    '            Dim dtRo As IC3801013DataSet.IC3801013ROReserveInfoDataTable = biz.GetROReserveInfoList(dlrCode)
    '            'APIから取得したデータテーブルをログに出力
    '            Me.OutPutIFLog(dtRo, "IC3801013BusinessLogic.GetROReserveInfoList")
    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    '            Return dtRo
    '        Finally
    '            biz = Nothing
    '        End Try
    '    End Function
    '#End Region

    '#Region "承認待ちの追加作業情報取得"
    '    ''' <summary>
    '    ''' 承認待ちの追加作業情報取得
    '    ''' </summary>
    '    ''' <param name="dlrCode">販売店コード</param>
    '    ''' <returns>承認待ちの追加作業情報</returns>
    '    ''' <remarks></remarks>
    '    Private Function GetAddRepairInfoList(ByVal dlrCode As String) As IC3800809DataSet.IC3800809AddRepairInfoDataTable
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))
    '        Dim biz As New IC3800809BusinessLogic
    '        Try
    '            '追加作業ステータスリストに「2:追加作業承認待ち」を追加する
    '            Dim addStatus(0) As String
    '            addStatus.SetValue(ADDWORK_CONFIRMWAIT, 0)
    '            '承認待ちの追加作業情報を取得する
    '            Dim dtRo As IC3800809DataSet.IC3800809AddRepairInfoDataTable = biz.GetAddRepairInfoList(dlrCode, addStatus)
    '            'APIから取得したデータテーブルをログに出力
    '            Me.OutPutIFLog(dtRo, "IC3800809BusinessLogic.GetAddRepairInfoList")
    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    '            Return dtRo
    '        Finally
    '            biz = Nothing
    '        End Try
    '    End Function
    '#End Region

    '#Region "商品情報を取得する"
    '    ''' <summary>
    '    ''' 商品情報を取得する
    '    ''' </summary>
    '    ''' <param name="mercId">商品Id</param>
    '    ''' <returns>商品情報</returns>
    '    ''' <remarks></remarks>
    '    Private Function GetMercInfo(ByVal mercId As Decimal) As TabletSMBCommonClassDataSet.TabletSmbCommonClassMercinfoDataTable
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

    '        Using commBiz As New TabletSMBCommonClassBusinessLogic
    '            '商品情報を取得する
    '            Dim dtMercInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassMercinfoDataTable = commBiz.GetSvcClassInfo(mercId)
    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    '            Return dtMercInfo
    '        End Using
    '    End Function
    '#End Region
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END


    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START

    '#Region "受付チップ情報整理"
    '    ''' <summary>
    '    ''' 受付チップ情報整理
    '    ''' </summary>
    '    ''' <param name="dtSvc">サービス入庫側情報</param>
    '    ''' <param name="dtRo">RO情報</param>
    '    ''' <returns>店舗稼動時間情報テーブル</returns>
    '    Private Function MergeRecetionInfo(ByVal dtSvc As SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable, _
    '                                      ByVal dtRo As IC3801013DataSet.IC3801013ROReserveInfoDataTable) As SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Using dt As New SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable
    '            Dim drRetrun As SC3240301DataSet.SC3240301SubReceptionChipInfoRow = _
    '                CType(dt.NewRow(), SC3240301DataSet.SC3240301SubReceptionChipInfoRow)
    '            Dim svcInIdList As New List(Of Long)
    '            'IFで取得した情報を受付チップ情報に設定する
    '            Dim defaultTime As Date = DefaultDateTimeValue()
    '            '受付チップ情報をループして、情報を補足する
    '            Dim roNum As String = ""
    '            For Each drRo As IC3801013DataSet.IC3801013ROReserveInfoRow In dtRo
    '                roNum = drRo.orderNO
    '                'IF情報と受付チップ情報をあてる
    '                Dim drSvc As SC3240301DataSet.SC3240301SubReceptionChipInfoRow() = _
    '                    (From col In dtSvc Where col.RO_NUM = roNum Select col).ToArray
    '                '情報収集
    '                Dim mntnCd As String = ""
    '                Dim vin As String = ""
    '                Dim mercId As Long
    '                If drSvc.Count = DefaultNumberValue Then
    '                    Continue For
    '                Else
    '                    '情報作成サービス入庫情報詰め込む
    '                    drRetrun = drSvc(0)

    '                    If drRo.workSeq > DefaultNumberValue Then
    '                        '追加作業の場合
    '                        drRetrun.STALL_USE_ID = DefaultNumberValue
    '                        drRetrun.RSLT_START_DATETIME = defaultTime
    '                        drRetrun.RSLT_END_DATETIME = defaultTime
    '                    End If
    '                    '作業連番
    '                    drRetrun.RO_JOB_SEQ = drRo.workSeq
    '                    '枝番
    '                    drRetrun.SRVADDSEQ = drRo.addSeq
    '                    '代表整備コード
    '                    If Not drRo.IsorderTypicalSrvTypeCodeNull Then
    '                        drRetrun.MNTNCD = drRo.orderTypicalSrvTypeCode
    '                        mntnCd = drRo.orderTypicalSrvTypeCode
    '                    End If
    '                    '部品準備完了フラグ
    '                    If Not drRo.IspartsRepareFlgNull Then
    '                        drRetrun.PARTS_FLG = drRo.partsRepareFlg
    '                    End If
    '                    'VIN
    '                    If Not drRo.IsvinNONull Then
    '                        vin = drRo.vinNO
    '                    End If
    '                    '納車予定日時
    '                    drRetrun.SCHE_DELI_DATETIME = drRo.deliveryHopeDate
    '                    'お客様承認日時
    '                    If Not drRo.IscustomerConfirmDateNull Then
    '                        drRetrun.CUST_CONFIRMDATE = drRo.customerConfirmDate
    '                    Else
    '                        drRetrun.CUST_CONFIRMDATE = DefaultDateTimeValue()
    '                    End If

    '                    '商品IDを取得する
    '                    If Not String.IsNullOrWhiteSpace(mntnCd) Then
    '                        mercId = Me.GetMercId(drSvc(0).DLR_CD, mntnCd, vin)
    '                    End If

    '                    '商品情報を取得する
    '                    If mercId <> DefaultNumberValue Then
    '                        Dim dtMercInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassMercinfoDataTable = Me.GetMercInfo(mercId)
    '                        If dtMercInfo.Count > DefaultNumberValue Then
    '                            drRetrun.MERC_ID = mercId
    '                            drRetrun.SVC_CLASS_ID = dtMercInfo(0).SVC_CLASS_ID
    '                            drRetrun.UPPER_DISP = dtMercInfo(0).UPPER_DISP
    '                            drRetrun.LOWER_DISP = dtMercInfo(0).LOWER_DISP
    '                            drRetrun.SVC_CLASS_NAME = dtMercInfo(0).SVC_CLASS_NAME
    '                            drRetrun.SVC_CLASS_NAME_ENG = dtMercInfo(0).SVC_CLASS_NAME_ENG
    '                        End If
    '                    End If
    '                End If
    '                '遅れ見込みを計算するために、遅れではないチップのサービス入庫IDを記録する
    '                If NarrowDownForDelayCompute(drRetrun.SCHE_DELI_DATETIME, DateTimeFunc.Now(drSvc(0).DLR_CD)) _
    '                    AndAlso Not svcInIdList.Contains(drRetrun.SVCIN_ID) Then
    '                    svcInIdList.Add(drRetrun.SVCIN_ID)
    '                End If
    '                dt.ImportRow(drRetrun)
    '            Next

    '            '遅れ見込み情報を取得する
    '            '遅れ見込みディクショナリー(サービス入庫ID,遅れ見込み日時)
    '            Dim deliDelayData As Dictionary(Of Long, Date) = Me.GetDeliDelayDateTime(svcInIdList)
    '            '取得できた場合、ループで遅れ見込み日時をチップ情報に設定する
    '            For Each dr As SC3240301DataSet.SC3240301SubReceptionChipInfoRow In dt
    '                If deliDelayData.ContainsKey(dr.SVCIN_ID) Then
    '                    dr.PLAN_DELAYDATE = deliDelayData(dr.SVCIN_ID)
    '                End If
    '            Next


    '            Dim dv As DataView = dt.DefaultView
    '            'お客承認日時でソートする
    '            dv.Sort = "CUST_CONFIRMDATE ASC"
    '            Using dtRetrun As New SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable
    '                dtRetrun.Merge(dv.ToTable())
    '                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '                Return dtRetrun
    '            End Using
    '        End Using
    '    End Function

    '#End Region

    '#Region "受付チップ数計算"
    '    ''' <summary>
    '    ''' 受付チップ数計算
    '    ''' </summary>
    '    ''' <param name="dtSvc">サービス入庫側情報</param>
    '    ''' <param name="dtRo">RO情報</param>
    '    ''' <returns>店舗稼動時間情報テーブル</returns>
    '    Private Function GetRecetionCount(ByVal dtSvc As SC3240301DataSet.SC3240301ChipCountDataTable, _
    '                                      ByVal dtRo As IC3801013DataSet.IC3801013ROReserveInfoDataTable) As Long
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Dim count As Long
    '        For Each drRo As IC3801013DataSet.IC3801013ROReserveInfoRow In dtRo
    '            Dim roNum As String = drRo.orderNO
    '            'IF情報と受付チップ情報をあてる
    '            Dim drSvc As SC3240301DataSet.SC3240301ChipCountRow() = _
    '                (From col In dtSvc Where col.RO_NUM = roNum Select col).ToArray
    '            If drSvc.Count > DefaultNumberValue Then
    '                count = count + 1
    '            End If
    '        Next
    '        Return count
    '    End Function

    '#End Region

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
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

#Region "中断終了処理"
    '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
    ''' <summary>
    ''' 中断終了(次工程へ移動)処理
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inDTNow">現在日時</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <returns>実行結果 0：正常 / 0以外：エラー</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function FinishStopChip(ByVal inStallUseId As Decimal, _
                                   ByVal inDTNow As Date, _
                                   ByVal inRowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} Start inStallUseId={2}, inDTNow={3}, inRowLockVersion={4} ", _
                                  Me.GetType.ToString, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inStallUseId, _
                                  inDTNow, _
                                  inRowLockVersion))

        Dim result As Long

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic
            Try
                '中断したチップの完了
                result = clsTabletSMBCommonClass.FinishStopChip(inStallUseId, _
                                                                inDTNow, _
                                                                inRowLockVersion, _
                                                                MY_PROGRAMID)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                ''エラーコードを戻す
                'If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then

                '    Me.Rollback = True

                'End If

                If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success _
                And result <> TabletSMBCommonClassBusinessLogic.ActionResult.WarningOmitDmsError Then
                    '共通関数から返却された処理結果コードが下記の場合
                    '    0(成功)以外、かつ
                    '-9000(DMS除外エラーの警告)以外

                    'ロールバック実施
                    Me.Rollback = True

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'DBTimeout
                Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                                           "{0}.{1} End [DBTimeOutError]", _
                                           Me.GetType.ToString, _
                                           MethodBase.GetCurrentMethod.Name))

                Me.Rollback = True
                Return TabletSMBCommonClassBusinessLogic.ActionResult.DBTimeOutError

            End Try
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} End ReturnValue={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  result))
        Return result

    End Function

    ''' <summary>
    ''' 中断終了(次工程へ移動)操作で通知を出す
    ''' </summary>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <remarks></remarks>
    Public Sub SendNoticeByStopFinish(ByVal inStaffInfo As StaffContext, _
                                      ByVal inStallUseId As Decimal)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} Start inStallUseId={2} ", _
                                  Me.GetType.ToString, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inStallUseId))

        Using clsTabletSMBCommonClass As New TabletSMBCommonClassBusinessLogic

            'チップエンティティを取得
            Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = clsTabletSMBCommonClass.GetChipEntity(inStallUseId)

            If 0 < dtChipEntity.Count Then
                '取得できた場合(0 < 取得行数)

                '通知を出す処理
                clsTabletSMBCommonClass.SendNoticeByStopFinish(inStaffInfo, _
                                                               dtChipEntity(0).SVCIN_ID, _
                                                               dtChipEntity(0).STALL_ID, _
                                                               MY_PROGRAMID)
            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} End ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub
    '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END
#End Region

End Class

