'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3090401.aspx.vb
'──────────────────────────────────
'機能： 予約一覧
'補足： 
'作成： 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001 iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
'更新： 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 $01
'更新：
'──────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.GateKeeper.AppointmentList.BizLogic
Imports Toyota.eCRB.GateKeeper.AppointmentList.DataAccess
Imports System.Globalization
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports System.Web.Script.Serialization

Partial Class Pages_SC3090401
    Inherits BasePage

#Region "定数"

#Region "共通"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApplicationId As String = "SC3090401"

    ''' <summary>
    ''' ゲートキーパーメイン画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const GatekeeperMainPageId As String = "SC3090301"

    ''' <summary>
    ''' 来店済み取得フラグ(0:取得しない)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllDisplayFlagOff As String = "0"

    ''' <summary>
    ''' ソート条件区分（0:予約日時）
    ''' </summary>
    Private Const SortTypeRezDate As String = "0"

    ''' <summary>
    ''' 敬称が名称の後
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeBehindCustName As String = "1"
#End Region

#Region "文言ID"

    ''' <summary>
    ''' 文言ID：正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdResultSuccess As Integer = 0

    ''' <summary>
    ''' 文言ID：画面タイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdDispTitle As Integer = 1

    ''' <summary>
    ''' 文言ID：更新中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdUpdating As Integer = 4

    ''' <summary>
    ''' 文言ID：次のN件
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdNext As Integer = 6

    ''' <summary>
    ''' 文言ID：前のN件
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdBefore As Integer = 7

    ''' <summary>
    ''' 文言ID：予約が見つかりません
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdReserveNotFound As Integer = 8

    ''' <summary>
    ''' 文言ID：未来店の予約が見つかりません
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdNoShowReserveNotFound As Integer = 9

    ''' <summary>
    ''' 文言ID：登録しますか
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdRegister As Integer = 10

    ''' <summary>
    ''' 文言ID：来店を取り消しますか
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdCancelVisit As Integer = 11

    ''' <summary>
    ''' 文言ID：データベースタイムアウトエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdDbTimeout As Integer = 901
#End Region

#Region "DB関連"

    ''' <summary>
    ''' システム管理マスタ.パラメータ名:標準読込件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StandardReadCountParam As String = "SC3090401_DEFAULT_READ_COUNT"

    ''' <summary>
    ''' システム管理マスタ.パラメータ名:最大表示件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MaxDisplayCountParam As String = "SC3090401_MAX_DISPLAY_COUNT"

#End Region

#End Region

#Region "イベント"
    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load_Start")

        ' PostBack時、初期表示処理は行わない。
        If IsPostBack = True Then

            Return
        End If

        ' ログイン情報管理(販売店コード,店舗コードの取得)
        Dim staffInfo As StaffContext = StaffContext.Current

        Dim dealerCode As String = staffInfo.DlrCD
        Dim branchCode As String = staffInfo.BrnCD

        Dim updDate As String = DateTimeFunc.Now(staffInfo.DlrCD).ToString

        'サーバの時間
        Me.ServerTimeHidden.Value = updDate

        'MM/ddとHH:mmのデータフォーマットを取得する
        Me.hidDateFormatMMdd.Value = DateTimeFunc.GetDateFormat(11)
        Me.hidDateFormatHHmm.Value = DateTimeFunc.GetDateFormat(14)

        Dim sysEnvSet As New SystemEnvSetting

        ' 標準読込件数
        Dim loadCount As String = sysEnvSet.GetSystemEnvSetting(StandardReadCountParam).PARAMVALUE

        ' 最大表示件数
        Dim maxDispCount As String = sysEnvSet.GetSystemEnvSetting(MaxDisplayCountParam).PARAMVALUE

        ' 読込開始行、読込終了行を設定
        Me.AppointmentListBeginIndex.Value = "1"

        ' 読込終了行は標準読込件数と最大表示件数の小さい値で初期化
        If CInt(loadCount) < CInt(maxDispCount) Then

            Me.AppointmentListEndIndex.Value = loadCount
        Else

            Me.AppointmentListEndIndex.Value = maxDispCount
            loadCount = maxDispCount
        End If

        ' 標準読込件数、最大表示件数を設定
        Me.StandardReadCountNumber.Value = loadCount
        Me.MaxDisplayCountNumber.Value = maxDispCount

        ' 文言の設定
        SetWord()

        ' 来店済み表示切替ボタンの状態を非表示に設定
        Me.AllDisplayFlag.Value = AllDisplayFlagOff

        ' ソート順切替ボタンの状態を予約日時順に設定
        Me.SortType.Value = SortTypeRezDate

        Logger.Info("Page_Load_End")
    End Sub

    ''' <summary>
    ''' 文言の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord()

        Logger.Info("SetWord_Start")

        ' 来店登録の確認ダイアログ
        Me.RegistConfirmMessageText.Value = WebWordUtility.GetWord(MessageIdRegister)

        ' 来店取消の確認ダイアログ
        Me.CancelConfirmMessageText.Value = WebWordUtility.GetWord(MessageIdCancelVisit)

        ' 検索結果0件の文言
        Me.NoSearchNoShowWord.Text = WebWordUtility.GetWord(ApplicationId, MessageIdNoShowReserveNotFound)
        Me.NoSearchWord.Text = WebWordUtility.GetWord(ApplicationId, MessageIdReserveNotFound)

        ' ページングの文言を設定
        Me.BackPageWord.Text = WebWordUtility.GetWord(ApplicationId, WordIdBefore).Replace("{0}", Me.StandardReadCountNumber.Value)
        Me.BackPageLoadWord.Text = WebWordUtility.GetWord(ApplicationId, WordIdUpdating)
        Me.NextPageWord.Text = WebWordUtility.GetWord(ApplicationId, WordIdNext).Replace("{0}", Me.StandardReadCountNumber.Value)
        Me.NextPageLoadWord.Text = WebWordUtility.GetWord(ApplicationId, WordIdUpdating)

        Logger.Info("SetWord_End")
    End Sub

    ''' <summary>
    ''' 初回読込ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub InitButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles InitButton.Click

        Logger.Info("InitButton_Click_Start")

        ' 予約一覧表示
        SetAppointmentList(Me.AllDisplayFlag.Value, _
                           Me.SortType.Value, _
                           1, _
                           CInt(Me.StandardReadCountNumber.Value))


        Logger.Info("InitButton_Click_End")

    End Sub


    ''' <summary>
    ''' 戻るボタンクリックイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub BackButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BackButton.Click

        Logger.Info("BackButton_Click_Start")

        ' ゲートキーパーメイン画面(SC3090301)に遷移する
        Me.RedirectNextScreen(GatekeeperMainPageId)

        Logger.Info("BackButton_Click_End")

    End Sub

    ''' <summary>
    ''' ソート順切替ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub SortButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SortButton.Click

        Logger.Info("SortButton_Click_Start")

        ' 予約一覧表示
        SetAppointmentList(Me.AllDisplayFlag.Value, _
                           Me.SortType.Value, _
                           1, _
                           CInt(Me.StandardReadCountNumber.Value))


        Logger.Info("SortButton_Click_End")

    End Sub

    ''' <summary>
    ''' 来店済表示切替ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub AllDisplayButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AllDisplayButton.Click

        Logger.Info("AllDisplayButton_Click_Start")

        ' 予約一覧表示
        SetAppointmentList(Me.AllDisplayFlag.Value, _
                           Me.SortType.Value, _
                           1, _
                           CInt(Me.StandardReadCountNumber.Value))


        Logger.Info("AllDisplayButton_Click_End")

    End Sub

    ''' <summary>
    ''' 次のN件タップ時イベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub NextPageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles NextPageButton.Click

        Logger.Info("NextPageButton_Click_Start")

        ' 標準取得件数を取得
        Dim loadCount As Integer = CInt(Me.StandardReadCountNumber.Value)

        ' 最大表示件数の取得
        Dim maxDisplayCount As Integer = CInt(Me.MaxDisplayCountNumber.Value)

        ' 直前の終了行番号の取得
        Dim startIndex As Integer = CInt(Me.AppointmentListBeginIndex.Value)
        Dim endIndex As Integer = CInt(Me.AppointmentListEndIndex.Value)

        ' 検索に使用する開始行番号、終了行番号
        Dim searcdStartIndex As Integer
        Dim searcdEndIndex As Integer

        ' 終了行の設定
        searcdEndIndex = endIndex + loadCount

        ' 開始行の設定
        Dim setStartMax As Long = searcdEndIndex - startIndex + 1
        If setStartMax <= maxDisplayCount Then

            searcdStartIndex = startIndex
        Else
            searcdStartIndex = searcdEndIndex - maxDisplayCount + 1

            If searcdStartIndex <= 0 Then

                searcdStartIndex = 1
            End If
        End If

        ' 予約一覧表示
        SetAppointmentList(Me.AllDisplayFlag.Value, _
                           Me.SortType.Value, _
                           searcdStartIndex, _
                           searcdEndIndex)

        Logger.Info("NextPageButton_Click_End")

    End Sub

    ''' <summary>
    ''' 前のN件タップ時イベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub BackPageButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BackPageButton.Click

        Logger.Info("BackPageButton_Click_Start")

        ' 標準取得件数を取得
        Dim loadCount As Integer = CInt(Me.StandardReadCountNumber.Value)

        ' 最大表示件数の取得
        Dim maxDisplayCount As Integer = CInt(Me.MaxDisplayCountNumber.Value)

        ' 開始行番号、終了行番号の取得
        Dim startIndex As Integer = CInt(Me.AppointmentListBeginIndex.Value)
        Dim endIndex As Integer = CInt(Me.AppointmentListEndIndex.Value)

        ' 検索に使用する開始行番号、終了行番号
        Dim searcdStartIndex As Integer
        Dim searcdEndIndex As Integer

        ' 開始行の設定
        Dim setStartMin As Integer = startIndex - loadCount
        If setStartMin <= 0 Then

            searcdStartIndex = 1
        Else
            searcdStartIndex = setStartMin
        End If

        ' 終了行の設定
        Dim setEndMin As Integer = endIndex - searcdStartIndex + 1
        If setEndMin < maxDisplayCount Then

            searcdEndIndex = endIndex
        Else

            searcdEndIndex = searcdStartIndex + maxDisplayCount - 1
        End If

        ' 予約一覧表示
        SetAppointmentList(Me.AllDisplayFlag.Value, _
                           Me.SortType.Value, _
                           searcdStartIndex, _
                           searcdEndIndex)

        Logger.Info("BackPageButton_Click_End")

    End Sub

    ''' <summary>
    ''' 来店登録イベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub VisitEventButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles VisitEventButton.Click

        Logger.Info("VisitEventButton_Click_Start")

        ' ログイン情報管理(販売店コード,店舗コードの取得)
        Dim staffInfo As StaffContext = StaffContext.Current

        Dim dealerCode As String = staffInfo.DlrCD
        Dim branchCode As String = staffInfo.BrnCD

        ' 現在日時を取得
        Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)

        ' 選択した予約のサービス入庫IDを取得
        Dim selectSvcinId As Decimal = CType(Me.HiddenSelectServiceinId.Value, Decimal)

        Dim biz As SC3090401BusinessLogic = New SC3090401BusinessLogic

        ' サービス来店登録
        Dim messageId As Integer = biz.RegistServiceVisit(dealerCode, branchCode, selectSvcinId, nowDate)

        ' 更新失敗の場合
        If messageId <> MessageIdResultSuccess Then

            ' エラーメッセージを出力
            Me.ShowMessageBox(messageId)
        End If

        ' 予約一覧表示
        SetAppointmentList(Me.AllDisplayFlag.Value, _
                           Me.SortType.Value, _
                           1, _
                           CInt(Me.StandardReadCountNumber.Value))

        Logger.Info("VisitEventButton_Click_End")

    End Sub

    ''' <summary>
    ''' 来店取消イベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub VisitCancelButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles VisitCancelButton.Click

        Logger.Info("VisitCancelButton_Click_Start")

        ' ログイン情報管理(販売店コード,店舗コードの取得)
        Dim staffInfo As StaffContext = StaffContext.Current

        Dim dealerCode As String = staffInfo.DlrCD
        Dim branchCode As String = staffInfo.BrnCD

        ' 現在日時を取得
        Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)

        ' 選択した予約のサービス入庫IDを取得
        Dim selectSvcinId As Decimal = CType(Me.HiddenSelectServiceinId.Value, Decimal)

        ' 選択した予約の更新日を取得
        Dim selectUpdateDate As Date = Date.Parse(Me.HiddenSelectUpdateDate.Value, CultureInfo.CurrentCulture)

        Dim biz As SC3090401BusinessLogic = New SC3090401BusinessLogic

        ' サービス来店取消
        Dim messageId As Integer = biz.CancelServiceVisit(dealerCode, branchCode, selectSvcinId, selectUpdateDate)

        ' 更新失敗の場合
        If messageId <> MessageIdResultSuccess Then

            ' エラーメッセージを出力
            Me.ShowMessageBox(messageId)
        End If

        ' 予約一覧表示
        SetAppointmentList(Me.AllDisplayFlag.Value, _
                           Me.SortType.Value, _
                           1, _
                           CInt(Me.StandardReadCountNumber.Value))

        Logger.Info("VisitCancelButton_Click_End")

    End Sub

    ''' <summary>
    ''' プルダウンリフレッシュボタンクリックイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub PullDownRefreshButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PullDownRefreshButton.Click

        Logger.Info("PullDownRefreshButton_Click_Start")

        ' 予約一覧表示
        SetAppointmentList(Me.AllDisplayFlag.Value, _
                           Me.SortType.Value, _
                           1, _
                           CInt(Me.StandardReadCountNumber.Value))

        Logger.Info("PullDownRefreshButton_Click_End")

    End Sub

#End Region

#Region "Privateメソッド"
    ''' <summary>
    ''' 予約一覧表示処理
    ''' </summary>
    ''' <param name="inAllDisplayFlag">来店済み取得フラグ</param>
    ''' <param name="inSortType">ソート条件区分</param>
    ''' <param name="inBeginIndex">取得する予約情報の開始行番号</param>
    ''' <param name="inEndIndex">取得する予約情報の終了行番号</param>
    ''' <remarks></remarks>
    Private Sub SetAppointmentList(ByVal inAllDisplayFlag As String, _
                                   ByVal inSortType As String, _
                                   ByVal inBeginIndex As Integer, _
                                   ByVal inEndIndex As Integer)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inAllDisplayFlag = {2}, inSortType = {3}, inBeginIndex = {4}, inEndIndex = {5}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inAllDisplayFlag, inSortType, inBeginIndex, inEndIndex))

        ' ログイン情報管理(販売店コード,店舗コードの取得)
        Dim staffInfo As StaffContext = StaffContext.Current

        Dim dealerCode As String = staffInfo.DlrCD
        Dim branchCode As String = staffInfo.BrnCD

        ' 現在日時を取得
        Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)

        Dim biz As SC3090401BusinessLogic = New SC3090401BusinessLogic

        Try
            ' 予約件数取得
            Dim reserveCount As Integer = biz.GetReservationCount(dealerCode, _
                                                                  branchCode, _
                                                                  inAllDisplayFlag, _
                                                                  nowDate)

            ' 予約が存在する場合
            If 0 < reserveCount Then

                ' 予約情報を取得する
                Dim dtReserveInfo As SC3090401DataSet.SC3090401ReserveDataDataTable = _
                    biz.GetReservationInfo(dealerCode, _
                                           branchCode, _
                                           inAllDisplayFlag, _
                                           inSortType, _
                                           inBeginIndex, _
                                           inEndIndex, _
                                           nowDate)

                ' 予約情報をバインドする
                Me.VisitServiceInfoRepeater.DataSource = dtReserveInfo
                Me.VisitServiceInfoRepeater.DataBind()

                Dim dateFormatHHmm As String = DateTimeFunc.GetDateFormat(14)

                Dim i As Integer
                For i = 0 To Me.VisitServiceInfoRepeater.Items.Count - 1

                    ' 画面定義取得
                    Dim chipReserveArea As Control = Me.VisitServiceInfoRepeater.Items(i)

                    ' 1行取得
                    Dim drCustomerInfo As SC3090401DataSet.SC3090401ReserveDataRow = _
                        CType(dtReserveInfo.Rows(i), SC3090401DataSet.SC3090401ReserveDataRow)

                    ' 予約日時
                    If Not drCustomerInfo.IsREZ_DATETIMENull Then

                        CType(chipReserveArea.FindControl("ReserveDatetime"), Label).Text = drCustomerInfo.REZ_DATETIME.ToString(dateFormatHHmm, CultureInfo.CurrentCulture)
                    End If

                    '$01 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
                    '' サービス名称
                    'If Not drCustomerInfo.IsMERC_NAMENull Then

                    '    CType(chipReserveArea.FindControl("ServiceName"), Label).Text = drCustomerInfo.MERC_NAME
                    'End If
                    ' サービス分類名称
                    If Not drCustomerInfo.IsSVC_CLASS_NAMENull Then

                        CType(chipReserveArea.FindControl("ServiceName"), Label).Text = drCustomerInfo.SVC_CLASS_NAME
                    End If
                    '$01 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END

                    ' モデル名
                    If Not drCustomerInfo.IsMODEL_NAMENull Then

                        CType(chipReserveArea.FindControl("ModelName"), Label).Text = drCustomerInfo.MODEL_NAME
                    End If

                    ' 車両登録番号
                    If Not drCustomerInfo.IsREG_NUMNull Then

                        CType(chipReserveArea.FindControl("VehicleRegNum"), Label).Text = drCustomerInfo.REG_NUM
                    End If

                    ' 顧客名＋敬称
                    If drCustomerInfo.IsNAMETITLE_NAMENull _
                        Or drCustomerInfo.IsPOSITION_TYPENull Then

                        ' 敬称が無い場合、顧客名のみ代入
                        CType(chipReserveArea.FindControl("CustomerName"), Label).Text = drCustomerInfo.CST_NAME

                    Else

                        ' 敬称が名前よりも後の場合
                        If PositionTypeBehindCustName.Equals(drCustomerInfo.POSITION_TYPE) Then

                            CType(chipReserveArea.FindControl("CustomerName"), Label).Text = _
                                String.Concat(drCustomerInfo.CST_NAME, Space(1), drCustomerInfo.NAMETITLE_NAME)

                        Else
                            ' 敬称が名前よりも前の場合

                            CType(chipReserveArea.FindControl("CustomerName"), Label).Text = _
                                String.Concat(drCustomerInfo.NAMETITLE_NAME, Space(1), drCustomerInfo.CST_NAME)
                        End If
                    End If

                    ' サービス入庫ID
                    If Not drCustomerInfo.IsSVCIN_IDNull Then

                        CType(chipReserveArea.FindControl("ServiceinId"), HiddenField).Value = drCustomerInfo.SVCIN_ID.ToString
                    End If

                    ' 更新日時が入っている場合、来店済み
                    If Not drCustomerInfo.IsUPDATEDATENull Then

                        CType(chipReserveArea.FindControl("UpdateDate"), HiddenField).Value = drCustomerInfo.UPDATEDATE.ToString(CultureInfo.CurrentCulture)
                    End If
                Next

                ' 前のN件ボタン表示
                If 1 < inBeginIndex Then
                    Me.BackPage.Attributes("style") = "display:block;"
                Else
                    Me.BackPage.Attributes("style") = "display:none;"
                End If

                ' 次のN件ボタン表示
                If inEndIndex < reserveCount Then
                    Me.NextPage.Attributes("style") = "display:block;"
                Else
                    Me.NextPage.Attributes("style") = "display:none;"
                End If

                '読み込み中を非表示設定
                Me.BackPageLoad.Attributes("style") = "display:none;"
                Me.NextPageLoad.Attributes("style") = "display:none;"


                '表示件数を保持
                Me.AppointmentListBeginIndex.Value = CType(inBeginIndex, String)
                Me.AppointmentListEndIndex.Value = CType(inEndIndex, String)

                '予約一覧を表示する
                Me.RezListUpdatePanel.Attributes("style") = "display:block;"
                Me.NoSearchImage.Attributes("style") = "display:none;"
            Else

                ' 表示0件の文言を設定する
                If inAllDisplayFlag = AllDisplayFlagOff Then
                    Me.NoSearchWord.Attributes("style") = "display:none;"
                    Me.NoSearchNoShowWord.Attributes("style") = "display:block;"
                Else
                    Me.NoSearchNoShowWord.Attributes("style") = "display:none;"
                    Me.NoSearchWord.Attributes("style") = "display:block;"
                End If

                '取得できなかった場合は文言を表示する
                Me.RezListUpdatePanel.Attributes("style") = "display:none;"
                Me.NoSearchImage.Attributes("style") = "display:block;"
            End If

            ' UpdatePanelの更新処理を実行
            Me.RezListUpdatePanel.Update()

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Catch ex As OracleExceptionEx When ex.Number = 1013

            'ORACLEのタイムアウト処理
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} DB TIMEOUT:{2}" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                     , ex.Message))

            'DBタイムアウトのメッセージ表示
            Me.ShowMessageBox(MessageIdDbTimeout)

        Catch ex As Exception

            'エラーログの出力
            Logger.Error(ex.Message, ex)

            Throw

        End Try

    End Sub

#End Region


End Class
