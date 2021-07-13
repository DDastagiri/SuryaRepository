'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070206BusinessLogic.vb
'─────────────────────────────────────
'機能： 価格相談回答
'補足： 
'作成： 2013/12/09 TCS 外崎  Aカード情報相互連携開発
'─────────────────────────────────────
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Public Class SC3070206BusinessLogic
    Inherits BaseBusinessComponent


    ''' <summary>
    ''' 依頼種別（価格相談）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_PRICE As String = "02"

    ''' <summary>
    ''' ステータス（受付）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_RECEIVE As String = "4"

    ''' <summary>
    ''' I/Fパラメータ　カテゴリータイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_PUSHCATEGORY As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　表示位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_POSITION As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　表示時間
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_TIME As Long = 3

    ''' <summary>
    ''' I/Fパラメータ　表示タイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_DISPLAY_TYPE As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　色
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_COLOR As String = "1"

    ''' <summary>
    ''' I/Fパラメータ　表示時間数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_DISPLAY_FUNCTION As String = "icropScript.ui.setNotice()"

    ''' <summary>
    ''' I/Fパラメータ　アクション時関数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NOTICE_IF_ACTFUNCTION As String = "icropScript.ui.openNoticeDialog()"


    ''' <summary>
    ''' 初期表示データ取得
    ''' </summary>
    ''' <param name="noticeReqId">通知依頼ID (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>画面の初期表示データを取得する。</remarks>
    Public Function GetInitialData(ByVal noticeReqId As Long) As SC3070206DataSet.SC3070206EstDiscountApprovalDataTable
        Return SC3070206TableAdapter.GetAnswer(noticeReqId)
    End Function

    ''' <summary>
    ''' マネージャー回答送信前チェック
    ''' </summary>
    ''' <remarks>マネージャー回答送信前チェックを実施する。</remarks>
    Public Function GetManagerAnswerCheck(ByVal noticeReqId As Long) As SC3070206DataSet.SC3070206NoticeRequestInfoDataTable
        Return SC3070206TableAdapter.GetNoticeRequestInfo(noticeReqId)
    End Function


    ''' <summary>
    ''' 契約状況取得
    ''' </summary>
    ''' <remarks>契約状況を取得する。</remarks>
    Public Function GetContract(ByVal estimateId As Long) As SC3070206DataSet.SC3070206ContractDataTable
        Return SC3070206TableAdapter.GetContractFlg(estimateId)
    End Function

    ''' <summary>
    ''' マネージャー回答登録
    ''' </summary>
    ''' <remarks>マネージャー回答を登録する。</remarks>
    <EnableCommit()> _
    Public Function UpdateManagerAnswer(ByVal estimateId As Long, _
                                        ByVal seqNo As Long, _
                                        ByVal managerAccount As String, _
                                        ByVal approvedPrice As Nullable(Of Double), _
                                        ByVal managerMemo As String, _
                                        ByVal noticeReqId As Long, _
                                        ByVal updateAccount As String, _
                                        ByVal updateid As String) As Boolean
        SC3070206TableAdapter.LockEstimateInfo(estimateId)

        'マネージャー回答登録
        Dim chkFlg As Boolean = SC3070206TableAdapter.RegistAnswer(estimateId, _
                                                   seqNo, _
                                                   managerAccount, _
                                                   approvedPrice, _
                                                   managerMemo, _
                                                   updateAccount, _
                                                   updateid)

        If chkFlg Then
            'マネージャー回答登録・見積金額更新
            chkFlg = SC3070206TableAdapter.RegistDiscountPrice(estimateId, _
                                                               approvedPrice, _
                                                               updateAccount, _
                                                               updateid)
        End If

        '更新失敗の場合、ロールバックし処理を終了する
        If chkFlg = False Then
            Me.Rollback = True
            Return False
        End If

        'マネージャー回答通知登録
        Dim staffInfo As StaffContext = StaffContext.Current
        RegistNotification(staffInfo.Account, staffInfo.UserName, estimateId, noticeReqId, staffInfo.Account, NOTICE_IF_RECEIVE)

        Return True
    End Function

    ''' <summary>
    ''' 通知登録IF呼び出し
    ''' </summary>
    ''' <returns>通知ID</returns>
    ''' <remarks>通知登録IFを呼び出す。</remarks>
    Private Function RegistNotification(ByVal staffCode As String, _
                                   ByVal staffName As String, _
                                   ByVal estimateId As Long, _
                                   ByVal noticeReqId As Long, _
                                   ByVal salesStaffCode As String, _
                                   ByVal status As String) As Long
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("RegistNotification Start")

        Dim noticeData As XmlNoticeData = Nothing
        Dim account As XmlAccount = Nothing
        Dim requestNotice As XmlRequestNotice = Nothing
        Dim pushInfo As XmlPushInfo = Nothing

        Try
            Dim StaffInfo As StaffContext = StaffContext.Current
            Dim noticeRequestTable As SC3070206DataSet.SC3070206NoticeRequestInfoDataTable = SC3070206TableAdapter.GetNoticeRequestInfo(noticeReqId)
            Dim noticeRequest As SC3070206DataSet.SC3070206NoticeRequestInfoRow = noticeRequestTable(0)

            noticeData = New XmlNoticeData
            'headにデータを格納
            noticeData.TransmissionDate = DateTimeFunc.Now(StaffInfo.DlrCD)

            '回答先情報をセット（スタッフ）
            account = New XmlAccount
            account.ToAccount = noticeRequest.FROMACCOUNT
            If (Not noticeRequest.IsFROMACCOUNTNAMENull()) Then
                account.ToAccountName = noticeRequest.FROMACCOUNTNAME
            End If

            '回答者情報（マネージャー情報）をセット
            requestNotice = New XmlRequestNotice
            requestNotice.DealerCode = StaffInfo.DlrCD
            requestNotice.StoreCode = StaffInfo.BrnCD
            requestNotice.RequestClass = NOTICE_IF_PRICE
            requestNotice.Status = status
            requestNotice.RequestId = noticeReqId
            requestNotice.RequestClassId = estimateId
            requestNotice.FromAccount = StaffInfo.Account
            requestNotice.FromAccountName = StaffInfo.UserName
            If (Not noticeRequest.IsCRCUSTIDNull()) Then
                requestNotice.CustomId = noticeRequest.CRCUSTID
            End If
            If (Not noticeRequest.IsCUSTOMNAMENull()) Then
                requestNotice.CustomName = noticeRequest.CUSTOMNAME
            End If
            If (Not noticeRequest.IsCUSTOMERCLASSNull()) Then
                requestNotice.CustomerClass = noticeRequest.CUSTOMERCLASS
            End If
            If (Not noticeRequest.IsCSTKINDNull()) Then
                requestNotice.CustomerKind = noticeRequest.CSTKIND
            End If
            requestNotice.SalesStaffCode = salesStaffCode
            If (Not noticeRequest.IsVCLIDNull()) Then
                requestNotice.VehicleSequenceNumber = noticeRequest.VCLID
            End If
            If (Not noticeRequest.IsFLLWUPBOXSTRCDNull()) Then
                requestNotice.FollowUpBoxStoreCode = noticeRequest.FLLWUPBOXSTRCD
            End If
            If (Not noticeRequest.IsFLLWUPBOXNull()) Then
                requestNotice.FollowUpBoxNumber = noticeRequest.FLLWUPBOX
            End If

            '通知方法をセット
            pushInfo = New XmlPushInfo
            pushInfo.PushCategory = NOTICE_IF_PUSHCATEGORY          'カテゴリータイプ
            pushInfo.PositionType = NOTICE_IF_POSITION              '表示位置
            pushInfo.Time = NOTICE_IF_TIME                          '表示時間
            pushInfo.DisplayType = NOTICE_IF_DISPLAY_TYPE           '表示タイプ
            pushInfo.DisplayContents = String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(984), requestNotice.FromAccountName, requestNotice.CustomName)
            pushInfo.Color = NOTICE_IF_COLOR                        '色
            pushInfo.DisplayFunction = NOTICE_IF_DISPLAY_FUNCTION   '表示時関数
            pushInfo.ActionFunction = NOTICE_IF_ACTFUNCTION         'アクション時関数


            '格納したデータを親クラスに格納
            noticeData.AccountList.Add(account)
            noticeData.RequestNotice = requestNotice
            noticeData.PushInfo = pushInfo

            'ロジックを呼ぶ
            Using noticeRequestIF As New IC3040801BusinessLogic
                'i-CROPへ送信
                Dim response As XmlCommon = noticeRequestIF.NoticeDisplay(noticeData, ConstCode.NoticeDisposal.Peculiar)
                'Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0} NoticeID:{1}", GetCurrentMethod.Name, response.NoticeRequestId), True)
                Return response.NoticeRequestId
            End Using
        Finally
            If pushInfo IsNot Nothing Then
                pushInfo.Dispose()
            End If

            If requestNotice IsNot Nothing Then
                requestNotice.Dispose()
            End If

            If account IsNot Nothing Then
                account.Dispose()
            End If

            If noticeData IsNot Nothing Then
                noticeData.Dispose()
            End If
        End Try
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("RegistNotification End")
    End Function


End Class
