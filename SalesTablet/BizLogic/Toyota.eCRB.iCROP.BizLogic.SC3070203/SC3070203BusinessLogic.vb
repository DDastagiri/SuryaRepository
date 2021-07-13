'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3070203BusinessLogic.vb
'─────────────────────────────────────
'機能： 価格相談
'補足： 
'更新： 2013/06/30 TCS 葛西　  2013/10対応版　既存流用
'更新： 2013/11/28 TCS 森      Aカード情報相互連携開発
'─────────────────────────────────────

''' <summary>
''' SC3070203()
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class SC3070203BusinessLogic

#Region "定数"
    Private Enum RequestTypeEnum
        Request
        Cancel
    End Enum

#Region "通知依頼IF用の定数"
    '依頼種別（価格相談）
    Private Const NOTICE_IF_PRICE As String = "02"
    'ステータス（依頼）
    Private Const NOTICE_IF_REQUEST As String = "1"
    'ステータス（キャンセル）
    Private Const NOTICE_IF_CANCEL As String = "2"

    '(カテゴリータイプ)  : Popup
    Private Const NOTICE_IF_PUSHCATEGORY_POPUP As String = "1"
    '(表示位置) : header
    Private Const NOTICE_IF_POSITION_HEADER As String = "1"
    '(表示時間) 
    Private Const NOTICE_IF_TIME As Long = 3
    '(表示タイプ) : Text()
    Private Const NOTICE_IF_DISPLAY_TYPE_TEXT As String = "1"
    '(色) : 薄い黄色
    Private Const NOTICE_IF_COLOR As String = "1"
    '(表示時関数)
    Private Const NOTICE_IF_DISPLAY_FUNCTION As String = "icropScript.ui.openNoticeList()"
    '(アクション時関数)
    Private Const NOTICE_IF_ACTION_FUNCTION As String = "icropScript.ui.openNoticeList()"
#End Region

#End Region

#Region "メンバ変数"
    Private StaffInfo As StaffContext
    '見積作成画面からの引継ぎ情報
    Private TakingOverInfo As SC3070203TakingOverInfoRow
#End Region

#Region "コンストラクタ"
    Sub New(ByVal takingOverInfo As SC3070203TakingOverInfoDataTable)
        Me.StaffInfo = StaffContext.Current
        Me.TakingOverInfo = takingOverInfo(0)
    End Sub
#End Region

#Region "Public"
    ''' <summary>
    ''' セールスマネージャー一覧取得
    ''' </summary>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>セールスマネージャの一覧を取得する</remarks>
    Public Function SelectSalesManagerList() As SC3070203SalesManagerDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "Start {0}", GetCurrentMethod.Name), True)

        Using ta As New SC3070203DataTableTableAdapter(Me.StaffInfo.DlrCD, Me.StaffInfo.BrnCD, Me.StaffInfo.Account, Me.TakingOverInfo)
            Dim dt As SC3070203SalesManagerDataTable = ta.SelectSalesManagerList(Me.StaffInfo.TeamLeader)
            If IsNothing(dt) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0}", GetCurrentMethod.Name), True)

            Else
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0} DataCount:{1}", GetCurrentMethod.Name, dt.Count), True)
            End If

            Return dt
        End Using
    End Function

    ''' <summary>
    ''' 値引き理由一覧取得
    ''' </summary>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>値引き理由の一覧を取得する。</remarks>
    Public Function SelectPriceConsultationResonList() As SC3070203PriceConsultationReasonDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "Start {0}", GetCurrentMethod.Name), True)

        Using ta As New SC3070203DataTableTableAdapter(Me.StaffInfo.DlrCD, Me.StaffInfo.BrnCD, Me.StaffInfo.Account, Me.TakingOverInfo)
            Dim dt As SC3070203DataSet.SC3070203PriceConsultationReasonDataTable = ta.SelectPriceConsultationResonList
            If IsNothing(dt) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0}", GetCurrentMethod.Name), True)
            Else
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0} DataCount:{1}", GetCurrentMethod.Name, dt.Count), True)
            End If

            Return dt
        End Using
    End Function

    ''' <summary>
    ''' 価格相談中情報取得
    ''' </summary>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>価格相談中の情報を取得する。</remarks>
    Public Function SelectUnderPriceConsultationInfo() As SC3070203PriceConsultationInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "Start {0}", GetCurrentMethod.Name), True)

        Using ta As New SC3070203DataTableTableAdapter(Me.StaffInfo.DlrCD, Me.StaffInfo.BrnCD, Me.StaffInfo.Account, Me.TakingOverInfo)
            Dim dt As SC3070203DataSet.SC3070203PriceConsultationInfoDataTable = ta.SelectUnderPriceConsultationInfo()
            If IsNothing(dt) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0}", GetCurrentMethod.Name), True)
            Else
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0} DataCount:{1}", GetCurrentMethod.Name, dt.Count), True)
            End If

            Return dt
        End Using
    End Function

    ''' <summary>
    ''' 価格相談状況取得
    ''' </summary>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>見積に対し、価格相談中か否かの状況を取得する。</remarks>
    Public Function IsUnderPriceConsultation() As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "Start {0}", GetCurrentMethod.Name), True)

        Using ta As New SC3070203DataTableTableAdapter(Me.StaffInfo.DlrCD, Me.StaffInfo.BrnCD, Me.StaffInfo.Account, Me.TakingOverInfo)
            Dim dt As SC3070203PriceConsultationCountDataTable = ta.SelectUnderPriceConsultationCount()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0} DataCount:{1}", GetCurrentMethod.Name, dt(0).COUNT), True)

            If dt(0).COUNT > 0 Then
                Return True
            Else
                Return False
            End If

        End Using
    End Function

    ''' <summary>
    ''' 価格相談最新履歴取得
    ''' </summary>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>見積に対し、価格相談の最新履歴を取得する。</remarks>
    Public Function SelectPriceConsultationNewestHistory() As SC3070203PriceConsultationInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "Start {0}", GetCurrentMethod.Name), True)

        Using ta As New SC3070203DataTableTableAdapter(Me.StaffInfo.DlrCD, Me.StaffInfo.BrnCD, Me.StaffInfo.Account, Me.TakingOverInfo)
            Dim dt As SC3070203PriceConsultationInfoDataTable = ta.SelectPriceConsultationNewestHistory()

            If IsNothing(dt) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0}", GetCurrentMethod.Name), True)
            Else
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0} DataCount:{1}", GetCurrentMethod.Name, dt.Count), True)
            End If

            Return dt

        End Using
    End Function


    ''' <summary>
    ''' 見積価格相談登録処理
    ''' </summary>
    ''' <returns>登録件数</returns>
    ''' <remarks>画面で入力された情報をDBに新規登録する。</remarks>
    <EnableCommit()> _
    Public Function InsertPriceConsultationInfo() As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "Start {0}", GetCurrentMethod.Name), True)

        Using ta As New SC3070203DataTableTableAdapter(Me.StaffInfo.DlrCD, Me.StaffInfo.BrnCD, Me.StaffInfo.Account, Me.TakingOverInfo)
            Dim seqno As Long = ta.InsertPriceConsultationInfo()
            Me.TakingOverInfo.Seqno = seqno
            Dim noticeid As Long = NoticeRequest(RequestTypeEnum.Request)
            Me.TakingOverInfo.NoticeRequestid = noticeid
            UpdateNoticeid()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0}    seqno:{1} noticereqid:{2}", GetCurrentMethod.Name, seqno, noticeid), True)
            Return seqno
        End Using
    End Function


    ''' <summary>
    ''' 見積価格相談キャンセル処理
    ''' </summary>
    ''' <returns>登録件数</returns>
    ''' <remarks>画面に表示されている価格相談依頼をキャンセルする。</remarks>
    <EnableCommit()> _
    Public Function CancelPriceConsultationInfo() As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "Start {0}", GetCurrentMethod.Name), True)

        Return NoticeRequest(RequestTypeEnum.Cancel)

    End Function

    ' 2013/11/28 TCS 森      Aカード情報相互連携開発 START
    ''' <summary>
    ''' 見積値引き額情報取得
    ''' </summary>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>価格相談済の見積値引き額情報を取得する。</remarks>
    Public Function SelectDiscountPriceInfo() As Integer
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "Start {0}", GetCurrentMethod.Name), True)

        Using ta As New SC3070203DataTableTableAdapter(Me.StaffInfo.DlrCD, Me.StaffInfo.BrnCD, Me.StaffInfo.Account, Me.TakingOverInfo)
            Dim dt As Integer = ta.GetEstDiscountPrice()
            If IsNothing(dt) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0}", GetCurrentMethod.Name), True)
            Else
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0} DataCount:{1}", GetCurrentMethod.Name, dt), True)
            End If

            Return dt
        End Using
    End Function



    ' 2013/11/28 TCS 森      Aカード情報相互連携開発 END
#End Region

#Region "Private"
    ''' <summary>
    ''' 通知登録IF呼び出し
    ''' </summary>
    ''' <returns>通知ID</returns>
    ''' <remarks>通知登録IFを呼び出す。</remarks>
    Private Function NoticeRequest(ByVal requestType As RequestTypeEnum) As Long
        Dim noticeData As XmlNoticeData = Nothing
        Dim account As XmlAccount = Nothing
        Dim requestNotice As XmlRequestNotice = Nothing
        Dim pushInfo As XmlPushInfo = Nothing
        Try
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "Start {0}", GetCurrentMethod.Name), True)

            noticeData = New XmlNoticeData
            'headにデータを格納
            noticeData.TransmissionDate = DateTimeFunc.Now(Me.StaffInfo.DlrCD)

            '相談先情報をセット（セールスマネージャー）
            account = New XmlAccount
            account.ToAccount = Me.TakingOverInfo.ManagerAccount
            account.ToAccountName = Me.TakingOverInfo.ManagerName

            '相談者情報（スタッフ情報）をセット
            requestNotice = New XmlRequestNotice
            requestNotice.DealerCode = Me.StaffInfo.DlrCD
            requestNotice.StoreCode = Me.StaffInfo.BrnCD
            requestNotice.RequestClass = NOTICE_IF_PRICE

            If requestType = RequestTypeEnum.Request Then
                requestNotice.Status = NOTICE_IF_REQUEST
            Else
                requestNotice.Status = NOTICE_IF_CANCEL
                requestNotice.RequestId = Me.TakingOverInfo.NoticeRequestid
            End If

            requestNotice.RequestClassId = Me.TakingOverInfo.ESTIMATEID
            requestNotice.FromAccount = Me.StaffInfo.Account
            requestNotice.FromAccountName = Me.StaffInfo.UserName
            requestNotice.CustomId = Me.TakingOverInfo.Customerid
            requestNotice.CustomName = Me.TakingOverInfo.CustomerName
            requestNotice.CustomerClass = Me.TakingOverInfo.CUSTOMERCLASS
            requestNotice.CustomerKind = Me.TakingOverInfo.CustomerKind
            requestNotice.SalesStaffCode = Me.TakingOverInfo.SALESSTAFFCODE
            requestNotice.VehicleSequenceNumber = Me.TakingOverInfo.VEHICLESEQUENCENUMBER
            requestNotice.FollowUpBoxStoreCode = Me.TakingOverInfo.FOLLOWUPBOXSTORECODE
            If Me.TakingOverInfo.IsFOLLOWUPBOXNUMBERNull = False Then
                requestNotice.FollowUpBoxNumber = Me.TakingOverInfo.FOLLOWUPBOXNUMBER
            End If



            '通知方法をセット
            pushInfo = New XmlPushInfo
            'pushInfoにデータを格納


            pushInfo.PushCategory = NOTICE_IF_PUSHCATEGORY_POPUP
            pushInfo.PositionType = NOTICE_IF_POSITION_HEADER
            pushInfo.Time = NOTICE_IF_TIME
            pushInfo.DisplayType = NOTICE_IF_DISPLAY_TYPE_TEXT
            pushInfo.Color = NOTICE_IF_COLOR
            pushInfo.DisplayFunction = NOTICE_IF_DISPLAY_FUNCTION
            pushInfo.ActionFunction = NOTICE_IF_ACTION_FUNCTION

            If requestType = RequestTypeEnum.Request Then
                pushInfo.DisplayContents = WebWordUtility.GetWord("SC3070203", 903)
            Else
                pushInfo.DisplayContents = WebWordUtility.GetWord("SC3070203", 904)
            End If

            '格納したデータを親クラスに格納
            noticeData.AccountList.Add(account)
            noticeData.RequestNotice = requestNotice
            noticeData.PushInfo = pushInfo

            'ロジックを呼ぶ
            Using noticeRequestIF As New IC3040801BusinessLogic

                'i-CROPへ送信
                Dim response As XmlCommon = noticeRequestIF.NoticeDisplay(noticeData, ConstCode.NoticeDisposal.Peculiar)
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0} NoticeID:{1}", GetCurrentMethod.Name, response.NoticeRequestId), True)
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
    End Function


    ''' <summary>
    ''' 通知依頼ID更新
    ''' </summary>
    ''' <returns>更新件数</returns>
    ''' <remarks>見積価格相談の通知依頼IDを更新する。</remarks>
    Private Function UpdateNoticeid() As Integer
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "Start {0}", GetCurrentMethod.Name), True)

        'ロジックを呼ぶ
        Using ta As New SC3070203DataTableTableAdapter(Me.StaffInfo.DlrCD, Me.StaffInfo.BrnCD, Me.StaffInfo.Account, Me.TakingOverInfo)

            '2013/06/30 TCS 葛西 2013/10対応版　既存流用 START
            '通知依頼ID更新ロック取得
            ta.GetEstimateinfoLock()

            '通知依頼ID更新
            Dim updateCount As Integer

            updateCount = ta.UpdateNoticeid()

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0} Return:{1}", GetCurrentMethod.Name, updateCount), True)

            Return updateCount
            '2013/06/30 TCS 葛西 2013/10対応版　既存流用 END

        End Using

    End Function
#End Region
End Class
