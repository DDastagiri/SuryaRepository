'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
' SC3140103.aspx.vb
'─────────────────────────────────────
'機能: メインメニュー(SA) コードビハインド
'補足: 
'作成: 2012/01/16 KN 森下
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web

Imports Toyota.eCRB.iCROP.BizLogic.SC3140103
Imports Toyota.eCRB.iCROP.DataAccess.SC3140103

Imports System.Globalization

Partial Class Pages_SC3140103
    Inherits BasePage

#Region " 定数定義"

    ' 画面ID
    Private Const APPLICATIONID As String = "SC3140103"
    Private Const MAINMENUID As String = "SC3140103"
    Private Const C_APPLICATIONID_CUSTOMERNEW As String = "SC3080207"   ' 新規顧客登録画面
    Private Const C_APPLICATIONID_CUSTOMEROUT As String = "SC3080208"   ' 顧客詳細画面
    Private Const C_APPLICATIONID_ORDERNEW As String = "SC3160213"      ' R/O作成画面
    Private Const C_APPLICATIONID_ORDEROUT As String = "SC3160208"      ' R/O参照画面
    Private Const C_APPLICATIONID_ORDERLIST As String = "SC3160101"     ' R/O一覧画面
    Private Const C_APPLICATIONID_WORK As String = "SC3170201"          ' 追加作業登録画面
    Private Const C_APPLICATIONID_APPROVAL As String = "SC3170301"      ' 追加作業承認画面
    Private Const C_APPLICATIONID_CHECKSHEET As String = "SC3180202"    ' チェックシート印刷画面
    Private Const C_APPLICATIONID_SETTLEMENT As String = "SC3160207"    ' 清算入力画面
    Private Const C_APPLICATIONID_ADD_LIST As String = "SC3170101"      ' 追加作業一覧画面

    ' 最大表示時間(秒・分)
    Private Const C_MAX_TIME As Long = (99 * 60 + 59)
    Private Const C_MAX_TIME_DISP As String = "99'59"
    Private Const C_MIN_TIME_DISP As String = ""

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_SUCCESS As Long = 0
    ''' <summary>
    ''' エラー:DBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_DBTIMEOUT As Long = 901
    ''' <summary>
    ''' エラー:該当データなし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_NOMATCH As Long = 902
    ''' <summary>
    ''' エラー:SAコードが異なる
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_RET_DIFFSACODE As Long = 1

    ' 2012/02/27 KN 西田【SERVICE_1】START
    Private Const C_DEFAULT_CHIP_SPACE As String = "&nbsp;"
    ' 2012/02/27 KN 西田【SERVICE_1】END

    ''' <summary>
    ''' カウンターエリアの表示レベル
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum CounterAreaLevel
        ''' <summary>通常</summary>
        NORMAL
        ''' <summary>警告</summary>
        WARN
        ''' <summary>異常</summary>
        ERR
    End Enum

    'アイコン表示スタイル
    Private Enum IconShowType
        RightIcnD
        RightIcnI
        RightIcnS
        WorkRightIcnD
        WorkRightIcnI
        WorkRightIcnS
        PopupRightIcnD
        PopupRightIcnI
        PopupRightIcnS
    End Enum

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
    End Enum

    ''' <summary>
    ''' メッセージID管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum MsgID
        ''' <summary>タイムアウト</summary>
        id901 = 901
        ''' <summary>その他</summary>
        id902 = 902
        ''' <summary>担当SA外</summary>
        id903 = 903
        ''' <summary>未取引客</summary>
        id904 = 904
    End Enum

#End Region

#Region " 変数定義"

    ' 現在時刻
    'カウンター対応
    Protected mNow As DateTime

    ' 受付_予約無し_警告表示標準時間（分）
    Private mlngReceptNoresWarningLt As Long = 0    ' 予約なし
    ' 受付_予約無し_異常表示標準時間（分）
    Private mlngReceptNoresAbnormalLt As Long = 0   ' 予約なし

    ' 受付_予約有り_警告表示標準時間（分）
    Private mlngReceptResWarningLt As Long = 0      ' 予約あり
    ' 受付_予約有り_異常表示標準時間（分）
    Private mlngReceptResAbnormalLt As Long = 0     ' 予約あり

    ' 追加承認_予約無し_警告表示標準時間（分）
    Private mlngAddworkNoresWarningLt As Long = 0   ' 予約なし
    ' 追加承認_予約無し_異常表示標準時間（分）
    Private mlngAddworkNoresAbnormalLt As Long = 0  ' 予約なし

    ' 追加承認_予約有り_警告表示標準時間（分）
    Private mlngAddworkResWarningLt As Long = 0     ' 予約あり
    ' 追加承認_予約有り_異常表示標準時間（分）
    Private mlngAddworkResAbnormallt As Long = 0    ' 予約あり

    ' 納車準備_異常表示標準時間（分）
    Private mlngDeliverypreAbnormalLt As Long = 0
    ' 納車作業_異常表示時標準間（分）
    Private mlngDeliverywrAbnormalLt As Long = 0

    '固定文字列
    Private mWordFixedString As String

#End Region

#Region " イベント処理 "
    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' ページロード時の処理です。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '開始ログ出力
        Dim logStart As New StringBuilder
        With logStart
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" Start")
        End With
        Logger.Info(logStart.ToString)

        Try
            If Not Me.IsPostBack Then
                'カウンターエリアで使用する経過時間設定
                Me.SetCounterTime()
                ' チップ情報取得
                Me.InitVisitChip()
            End If

            'フッターの制御
            InitFooterEvent()

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'タイムアウトエラーの場合は、メッセージを表示する
            ShowMessageBox(MsgID.id901)
        End Try

        '終了ログ出力
        Dim logEnd As New StringBuilder
        With logEnd
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" End")
        End With
        Logger.Info(logEnd.ToString)

    End Sub

    ' '''-----------------------------------------------------------------------
    ' ''' <summary>
    ' ''' チップ詳細ボタン(左)
    ' ''' </summary>
    ' ''' <param name="sender">イベント発生元</param>
    ' ''' <param name="e">イベント引数</param>
    ' ''' <remarks>
    ' ''' チップ詳細画面フッタ部の左ボタン押下時に当イベントが発生します。
    ' ''' </remarks>
    ' '''-----------------------------------------------------------------------
    'Protected Sub DetailButtonLeft_Click(sender As Object, e As System.EventArgs) Handles DetailButtonLeft.Click

    '    '開始ログ出力
    '    Dim logStart As New StringBuilder
    '    With logStart
    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '        .Append(" Start")
    '    End With
    '    Logger.Info(logStart.ToString)

    '    Try
    '        '遷移処理
    '        Me.NextScreenVisitChipDetailButton(DetailButtonLeft.Text)

    '    Catch ex As OracleExceptionEx When ex.Number = 1013
    '        'タイムアウトエラーの場合は、メッセージを表示する
    '        ShowMessageBox(MsgID.id901)
    '    End Try

    '    '終了ログ出力
    '    Dim logEnd As New StringBuilder
    '    With logEnd
    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '        .Append(" End")
    '    End With
    '    Logger.Info(logEnd.ToString)

    'End Sub

    ' '''-----------------------------------------------------------------------
    ' ''' <summary>
    ' ''' チップ詳細ボタン(右)
    ' ''' </summary>
    ' ''' <param name="sender">イベント発生元</param>
    ' ''' <param name="e">イベント引数</param>
    ' ''' <remarks>
    ' ''' チップ詳細画面フッタ部の右ボタン押下時に当イベントが発生します。
    ' ''' </remarks>
    ' '''-----------------------------------------------------------------------
    'Protected Sub DetailButtonRight_Click(sender As Object, e As System.EventArgs) Handles DetailButtonRight.Click

    '    '開始ログ出力
    '    Dim logStart As New StringBuilder
    '    With logStart
    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '        .Append(" Start")
    '    End With
    '    Logger.Info(logStart.ToString)

    '    Try
    '        '遷移処理
    '        Me.NextScreenVisitChipDetailButton(DetailButtonRight.Text)

    '    Catch ex As OracleExceptionEx When ex.Number = 1013
    '        'タイムアウトエラーの場合は、メッセージを表示する
    '        ShowMessageBox(MsgID.id901)
    '    End Try

    '    '終了ログ出力
    '    Dim logEnd As New StringBuilder
    '    With logEnd
    '        .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
    '        .Append(" End")
    '    End With
    '    Logger.Info(logEnd.ToString)

    'End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' チップ詳細ボタン(共通画面遷移時処理)
    ''' 2012/02/29 KN 森下【SERVICE_1】START
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' チップ詳細画面フッタ部の右ボタン押下時に当イベントが発生します。
    ''' </remarks>
    '''-----------------------------------------------------------------------
    Protected Sub DetailNextScreenCommonButton_Click(sender As Object, e As System.EventArgs) Handles DetailNextScreenCommonButton.Click
        '開始ログ出力
        Dim logStart As New StringBuilder
        With logStart
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" Start")
        End With
        Logger.Info(logStart.ToString)

        Try
            '遷移処理
            Me.NextScreenVisitChipDetailButton(Me.DetailClickButtonName.Value)

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'タイムアウトエラーの場合は、メッセージを表示する
            ShowMessageBox(MsgID.id901)
        Finally
            ' チップタイマー用に現在時刻取得
            Dim staffInfo As StaffContext = StaffContext.Current
            Me.mNow = DateTimeFunc.Now(staffInfo.DlrCD)
        End Try

        '終了ログ出力
        Dim logEnd As New StringBuilder
        With logEnd
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" End")
        End With
        Logger.Info(logEnd.ToString)
    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' チップの詳細ポップアップウィンドウに表示する情報を取得する為のダミーボタンクリック
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' チップ詳細画面ポップアップ表示のために隠しボタンである当ボタンを
    ''' クライアント側にてクリックすることでイベントが発生します。
    ''' </remarks>
    '''-----------------------------------------------------------------------
    Protected Sub DetailPopupButton_Click(sender As Object, e As System.EventArgs) Handles DetailPopupButton.Click

        '開始ログ出力
        Dim logStart As New StringBuilder
        With logStart
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" Start")
        End With
        Logger.Info(logStart.ToString)

        Try
            ' 現在時刻取得
            Dim staffInfo As StaffContext = StaffContext.Current
            Me.mNow = DateTimeFunc.Now(staffInfo.DlrCD)
            ' チップ詳細情報表示
            Me.InitVisitChipDetail()
            Me.ContentUpdatePanelDetail.Update()

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'タイムアウトエラーの場合は、メッセージを表示する
            ShowMessageBox(MsgID.id901)
            ' 2012/02/22 KN 森下【SERVICE_1】START
        Finally
            ' 詳細ポップアップウィンドウの読み込み中アイコン非表示
            Dim ctrlDiv As HtmlContainerControl _
                = CType(Me.ContentUpdatePanelDetail.FindControl("IconLoadingPopup"), HtmlContainerControl)
            ctrlDiv.Attributes("style") = "visibility: hidden"
            ' 2012/02/22 KN 森下【SERVICE_1】END
        End Try

        '終了ログ出力
        Dim logEnd As New StringBuilder
        With logEnd
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" End")
        End With
        Logger.Info(logEnd.ToString)
    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 通知リフレッシュ処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント引数</param>
    ''' <remarks>
    ''' 通知イベントが発生した際に、ダミーボタンである当ボタンを
    ''' クライアントにてクリックすることで当イベントが発生します。
    ''' </remarks>
    '''-----------------------------------------------------------------------
    Protected Sub MainPolling_Click(sender As Object, e As System.EventArgs) Handles MainPolling.Click

        '開始ログ出力
        Dim logStart As New StringBuilder
        With logStart
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" Start")
        End With
        Logger.Info(logStart.ToString)

        Try
            'カウンターエリアで使用する経過時間設定
            Me.SetCounterTime()
            ' チップ情報表示
            Me.InitVisitChip()
            Me.ContentUpdatePanel.Update()

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'タイムアウトエラーの場合は、メッセージを表示する
            ShowMessageBox(MsgID.id901)
        End Try

        '終了ログ出力
        Dim logEnd As New StringBuilder
        With logEnd
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" End")
        End With
        Logger.Info(logEnd.ToString)

    End Sub

#End Region

#Region " 非公開メソッド"

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' カウンターエリアで使用する経過時間設定
    ''' </summary>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub SetCounterTime()

        Dim bl As SC3140103BusinessLogic = New SC3140103BusinessLogic()

        ' ストール設定情報取得(標準時間専用)
        Using dt As SC3140103DataSet.SC3140103StallCtl2DataTable = bl.GetStallControl()
            If dt.Rows.Count > 0 Then
                Dim row As SC3140103DataSet.SC3140103StallCtl2Row = DirectCast(dt.Rows(0), SC3140103DataSet.SC3140103StallCtl2Row)
                Me.mlngReceptNoresWarningLt = row.RECEPT_NORES_WARNING_LT
                Me.mlngReceptNoresAbnormalLt = row.RECEPT_NORES_ABNORMAL_LT
                Me.mlngReceptResWarningLt = row.RECEPT_RES_WARNING_LT
                Me.mlngReceptResAbnormalLt = row.RECEPT_RES_ABNORMAL_LT
                Me.mlngAddworkNoresWarningLt = row.ADDWORK_NORES_WARNING_LT
                Me.mlngAddworkNoresAbnormalLt = row.ADDWORK_NORES_ABNORMAL_LT
                Me.mlngAddworkResWarningLt = row.ADDWORK_RES_WARNING_LT
                Me.mlngAddworkResAbnormallt = row.ADDWORK_RES_ABNORMAL_LT
                Me.mlngDeliverypreAbnormalLt = row.DELIVERYPRE_ABNORMAL_LT
                Me.mlngDeliverywrAbnormalLt = row.DELIVERYWR_ABNORMAL_LT
            End If
        End Using

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' チップ情報取得
    ''' </summary>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub InitVisitChip()

        ' 現在時刻取得
        Dim staffInfo As StaffContext = StaffContext.Current
        Me.mNow = DateTimeFunc.Now(staffInfo.DlrCD)

        Dim bl As SC3140103BusinessLogic = New SC3140103BusinessLogic(Me.mlngDeliverypreAbnormalLt, Me.mNow)

        ' チップ情報取得
        Using dt As SC3140103DataSet.SC3140103VisitChipDataTable = bl.GetVisitChip()
            ' 2012/02/27 KN 西田【SERVICE_1】START
            'アイコンの固定文字列取得
            Dim strRightIcnD As String = WebWordUtility.GetWord(APPLICATIONID, 7)
            Dim strRightIcnI As String = WebWordUtility.GetWord(APPLICATIONID, 8)
            Dim strRightIcnS As String = WebWordUtility.GetWord(APPLICATIONID, 9)

            '固定文字列「～」取得
            mWordFixedString = WebWordUtility.GetWord(APPLICATIONID, 32)
            '受付エリアチップ初期設定
            Me.InitReception(dt, strRightIcnD, strRightIcnI, strRightIcnS)
            '追加承認エリアチップ初期設定
            Me.InitApproval(dt, strRightIcnD, strRightIcnI, strRightIcnS)
            '納車準備エリアチップ初期設定
            Me.InitPreparation(dt, strRightIcnD, strRightIcnI, strRightIcnS)
            '納車作業エリアチップ初期設定
            Me.InitDelivery(dt, strRightIcnD, strRightIcnI, strRightIcnS)
            '作業中エリアチップ初期設定
            Me.InitWork(dt, strRightIcnD, strRightIcnI, strRightIcnS)
            ' 2012/02/27 KN 西田【SERVICE_1】END
        End Using

        ' 現在時刻取得(最新)
        Me.mNow = DateTimeFunc.Now(staffInfo.DlrCD)

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 受付エリアチップ初期設定
    ''' </summary>
    ''' <param name="dt">チップ情報</param>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub InitReception(ByVal dt As SC3140103DataSet.SC3140103VisitChipDataTable, ByVal strRightIcnD As String, ByVal strRightIcnI As String, ByVal strRightIcnS As String)

        ' コントロールにバインドする
        Me.ReceptionRepeater.DataSource = dt.Select(String.Format(CultureInfo.CurrentCulture, "DISP_DIV = '{0}'", SC3140103BusinessLogic.DisplayDivReception), "DISP_SORT")
        Me.ReceptionRepeater.DataBind()

        Dim rowList As SC3140103DataSet.SC3140103VisitChipRow() = DirectCast(ReceptionRepeater.DataSource, SC3140103DataSet.SC3140103VisitChipRow())

        ' 2012/02/27 KN 西田【SERVICE_1】START
        Dim reception As Control
        Dim row As SC3140103DataSet.SC3140103VisitChipRow

        Dim strRegistrationNumber As String
        Dim strCustomerName As String
        Dim strVisitTime As String
        Dim strRepresentativeWarehousing As String
        Dim strParkingNumber As String

        Dim strReserveMark As String
        Dim strJDPMark As String
        Dim strSSCMark As String
        Dim strStart As String

        Dim divDeskDevice As HtmlContainerControl
        Dim divElapsedTime As HtmlContainerControl
        ' -----------------------------------------------------
        ' データを設定する
        ' -----------------------------------------------------
        For i = 0 To ReceptionRepeater.Items.Count - 1

            reception = ReceptionRepeater.Items(i)
            row = rowList(i)

            strRegistrationNumber = row.VCLREGNO
            strCustomerName = row.CUSTOMERNAME
            strVisitTime = row.ITEM_DATE.ToString(CultureInfo.CurrentCulture)
            strRepresentativeWarehousing = row.MERCHANDISENAME
            strParkingNumber = row.PARKINGCODE

            strReserveMark = row.REZ_MARK
            strJDPMark = row.JDP_MARK
            strSSCMark = row.SSC_MARK
            strStart = row.DISP_START

            CType(reception.FindControl("RegistrationNumber"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
            CType(reception.FindControl("CustomerName"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
            CType(reception.FindControl("VisitTime"), HtmlContainerControl).InnerHtml = Me.SetTimeFromToAppend(Me.SetDateStringToString(strVisitTime), ChipArea.Reception)
            CType(reception.FindControl("RepresentativeWarehousing"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
            CType(reception.FindControl("ParkingNumber"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strParkingNumber, C_DEFAULT_CHIP_SPACE)

            If Me.SetNullToString(strReserveMark, "0").Equals("1") Then
                reception.FindControl("RightIcnD").Visible = True
            Else
                reception.FindControl("RightIcnD").Visible = False
            End If

            If Me.SetNullToString(strJDPMark, "0").Equals("1") Then
                reception.FindControl("RightIcnI").Visible = True
            Else
                reception.FindControl("RightIcnI").Visible = False
            End If

            If Me.SetNullToString(strSSCMark, "0").Equals("1") Then
                reception.FindControl("RightIcnS").Visible = True
            Else
                reception.FindControl("RightIcnS").Visible = False
            End If

            'アイコンの文言設定
            CType(reception.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
            CType(reception.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
            CType(reception.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS
            CType(reception.FindControl("ElapsedTime"), HtmlContainerControl).InnerText = C_MIN_TIME_DISP

            divDeskDevice = CType(reception.FindControl("ReceptionDeskDevice"), HtmlContainerControl)

            With divDeskDevice
                .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)
                .Attributes("orderNo") = row.ORDERNO
                .Attributes("approvalId") = row.APPROVAL_ID

                ' 仕掛中チェック
                If Me.SetNullToString(strStart, "0").Equals("0") Then
                    .Attributes("class") = "ColumnContentsBoder"
                Else
                    .Attributes("class") = "ColumnContentsBoder ColumnBoxAqua"
                End If
            End With
            
            divElapsedTime = DirectCast(reception.FindControl("ElapsedTime"), HtmlContainerControl)
            With divElapsedTime
                .InnerText = C_MIN_TIME_DISP
                .Attributes("name") = "procgroup"
                .Attributes("procdate") = CType(row.PROC_DATE, String)
                .Attributes("overclass1") = "ColumnTimeYellow"
                .Attributes("overclass2") = "ColumnTimeRed"

                ''警告
                If String.Compare(row.REZ_MARK, "0", StringComparison.Ordinal) <> 0 Then
                    ''黄色：予約有りの場合、SA振当済みから5分経過かつ10分以下
                    .Attributes("overseconds1") = CType(Me.mlngReceptResWarningLt * 60, String)
                Else
                    ''黄色：予約無しの場合、SA振当済みから10分経過かつ15分以下
                    .Attributes("overseconds1") = CType(Me.mlngReceptNoresWarningLt * 60, String)
                End If
                ''異常
                If String.Compare(row.REZ_MARK, "0", StringComparison.Ordinal) <> 0 Then
                    ''赤色：予約有りの場合、SA振当済みから5分経過かつ10分以下
                    .Attributes("overseconds2") = CType(Me.mlngReceptResAbnormalLt * 60, String)
                Else
                    ''赤色：予約無しの場合、SA振当済みから10分経過かつ15分以下
                    .Attributes("overseconds2") = CType(Me.mlngReceptNoresAbnormalLt * 60, String)
                End If
            End With
        Next
        ' 2012/02/27 KN 西田【SERVICE_1】END

        'データ表示件数を表示する
        Me.ReceptionDeskTipNumber.Text = ReceptionRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 追加承認エリアチップ初期設定
    ''' </summary>
    ''' <param name="dt">チップ情報</param>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub InitApproval(ByVal dt As SC3140103DataSet.SC3140103VisitChipDataTable, ByVal strRightIcnD As String, ByVal strRightIcnI As String, ByVal strRightIcnS As String)

        ' コントロールにバインドする
        Me.ApprovalRepeater.DataSource = dt.Select(String.Format(CultureInfo.CurrentCulture, "DISP_DIV = '{0}'", SC3140103BusinessLogic.DisplayDivApproval), "DISP_SORT")
        Me.ApprovalRepeater.DataBind()

        Dim rowList As SC3140103DataSet.SC3140103VisitChipRow() = DirectCast(ApprovalRepeater.DataSource, SC3140103DataSet.SC3140103VisitChipRow())

        ' 2012/02/27 KN 西田【SERVICE_1】START
        Dim approval As Control
        Dim row As SC3140103DataSet.SC3140103VisitChipRow

        Dim strRegistrationNumber As String
        Dim strCustomerName As String
        Dim strDeliveryPlanTime As String
        Dim strRepresentativeWarehousing As String
        Dim strChargeTechnician As String

        Dim strReserveMark As String
        Dim strJDPMark As String
        Dim strSSCMark As String
        Dim strStart As String

        Dim divDeskDevice As HtmlContainerControl
        Dim divElapsedTime As HtmlContainerControl
        ' -----------------------------------------------------
        ' データを設定する
        ' -----------------------------------------------------
        For i = 0 To ApprovalRepeater.Items.Count - 1

            approval = ApprovalRepeater.Items(i)
            row = rowList(i)

            strRegistrationNumber = row.VCLREGNO
            strCustomerName = row.CUSTOMERNAME
            strDeliveryPlanTime = row.ITEM_DATE.ToString(CultureInfo.CurrentCulture)
            strRepresentativeWarehousing = row.MERCHANDISENAME
            strChargeTechnician = row.STAFFNAME

            strReserveMark = row.REZ_MARK
            strJDPMark = row.JDP_MARK
            strSSCMark = row.SSC_MARK
            strStart = row.DISP_START

            CType(approval.FindControl("ApprovalRegistrationNumber"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
            CType(approval.FindControl("ApprovalCustomerName"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
            CType(approval.FindControl("ApprovalDeliveryPlanTime"), HtmlContainerControl).InnerHtml = Me.SetTimeFromToAppend(Me.SetDateStringToString(strDeliveryPlanTime), ChipArea.Approval)
            CType(approval.FindControl("ApprovalRepresentativeWarehousing"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
            CType(approval.FindControl("ApprovalChargeTechnician"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strChargeTechnician, C_DEFAULT_CHIP_SPACE)

            If Me.SetNullToString(strReserveMark, "0").Equals("1") Then
                approval.FindControl("RightIcnD").Visible = True
            Else
                approval.FindControl("RightIcnD").Visible = False
            End If

            If Me.SetNullToString(strJDPMark, "0").Equals("1") Then
                approval.FindControl("RightIcnI").Visible = True
            Else
                approval.FindControl("RightIcnI").Visible = False
            End If

            If Me.SetNullToString(strSSCMark, "0").Equals("1") Then
                approval.FindControl("RightIcnS").Visible = True
            Else
                approval.FindControl("RightIcnS").Visible = False
            End If

            'アイコンの文言設定
            CType(approval.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
            CType(approval.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
            CType(approval.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS

            divDeskDevice = CType(approval.FindControl("ApprovalDeskDevice"), HtmlContainerControl)
            With divDeskDevice
                .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)
                .Attributes("orderNo") = row.ORDERNO
                .Attributes("approvalId") = row.APPROVAL_ID
                ' 仕掛中チェック
                If Me.SetNullToString(strStart, "0").Equals("0") Then
                    .Attributes("class") = "ColumnContentsBoder"
                Else
                    .Attributes("class") = "ColumnContentsBoder ColumnBoxAqua"
                End If
            End With

            divElapsedTime = DirectCast(approval.FindControl("ApprovalElapsedTime"), HtmlContainerControl)
            With divElapsedTime
                .InnerText = C_MIN_TIME_DISP
                .Attributes("name") = "procgroup"
                .Attributes("procdate") = CType(row.PROC_DATE, String)
                .Attributes("overclass1") = "ColumnTimeYellow"
                .Attributes("overclass2") = "ColumnTimeRed"

                ''警告
                If String.Compare(row.REZ_MARK, "0", StringComparison.Ordinal) <> 0 Then
                    ''黄色：予約有りの場合、SA振当済みから5分経過かつ10分以下
                    .Attributes("overseconds1") = CType(Me.mlngAddworkResWarningLt * 60, String)
                Else
                    ''黄色：予約無しの場合、SA振当済みから10分経過かつ15分以下
                    .Attributes("overseconds1") = CType(Me.mlngAddworkNoresWarningLt * 60, String)
                End If
                ''異常
                If String.Compare(row.REZ_MARK, "0", StringComparison.Ordinal) <> 0 Then
                    ''赤色：予約有りの場合、SA振当済みから5分経過かつ10分以下
                    .Attributes("overseconds2") = CType(Me.mlngAddworkResAbnormallt * 60, String)
                Else
                    ''赤色：予約無しの場合、SA振当済みから10分経過かつ15分以下
                    .Attributes("overseconds2") = CType(Me.mlngAddworkNoresAbnormalLt * 60, String)
                End If
            End With
        Next
        ' 2012/02/27 KN 西田【SERVICE_1】END

        'データ表示件数を表示する
        Me.ApprovalNumber.Text = ApprovalRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 納車準備エリアチップ初期設定
    ''' </summary>
    ''' <param name="dt">チップ情報</param>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub InitPreparation(ByVal dt As SC3140103DataSet.SC3140103VisitChipDataTable, ByVal strRightIcnD As String, ByVal strRightIcnI As String, ByVal strRightIcnS As String)

        ' コントロールにバインドする
        Me.PreparationRepeater.DataSource = dt.Select(String.Format(CultureInfo.CurrentCulture, "DISP_DIV = '{0}'", SC3140103BusinessLogic.DisplayDivPreparation), "DISP_SORT")
        Me.PreparationRepeater.DataBind()

        Dim rowList As SC3140103DataSet.SC3140103VisitChipRow() = DirectCast(PreparationRepeater.DataSource, SC3140103DataSet.SC3140103VisitChipRow())

        ' 2012/02/27 KN 西田【SERVICE_1】START
        Dim preparation As Control
        Dim row As SC3140103DataSet.SC3140103VisitChipRow

        Dim strRegistrationNumber As String
        Dim strCustomerName As String
        Dim strDeliveryPlanTime As String
        Dim strRepresentativeWarehousing As String
        Dim strChargeTechnician As String

        Dim strReserveMark As String
        Dim strJDPMark As String
        Dim strSSCMark As String
        Dim strStart As String

        Dim divDeskDevice As HtmlContainerControl
        Dim divElapsedTime As HtmlContainerControl
        ' -----------------------------------------------------
        ' データを設定する
        ' -----------------------------------------------------
        For i = 0 To PreparationRepeater.Items.Count - 1

            preparation = PreparationRepeater.Items(i)
            row = rowList(i)

            strRegistrationNumber = row.VCLREGNO
            strCustomerName = row.CUSTOMERNAME
            strDeliveryPlanTime = row.ITEM_DATE.ToString(CultureInfo.CurrentCulture)
            strRepresentativeWarehousing = row.MERCHANDISENAME
            strChargeTechnician = row.STAFFNAME

            strReserveMark = row.REZ_MARK
            strJDPMark = row.JDP_MARK
            strSSCMark = row.SSC_MARK
            strStart = row.DISP_START

            CType(preparation.FindControl("PreparationRegistrationNumber"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
            CType(preparation.FindControl("PreparationCustomerName"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
            CType(preparation.FindControl("PreparationDeliveryPlanTime"), HtmlContainerControl).InnerHtml = Me.SetTimeFromToAppend(Me.SetDateStringToString(strDeliveryPlanTime), ChipArea.Preparation)
            CType(preparation.FindControl("PreparationRepresentativeWarehousing"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
            CType(preparation.FindControl("PreparationChargeTechnician"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strChargeTechnician, C_DEFAULT_CHIP_SPACE)

            If Me.SetNullToString(strReserveMark, "0").Equals("1") Then
                preparation.FindControl("RightIcnD").Visible = True
            Else
                preparation.FindControl("RightIcnD").Visible = False
            End If

            If Me.SetNullToString(strJDPMark, "0").Equals("1") Then
                preparation.FindControl("RightIcnI").Visible = True
            Else
                preparation.FindControl("RightIcnI").Visible = False
            End If

            If Me.SetNullToString(strSSCMark, "0").Equals("1") Then
                preparation.FindControl("RightIcnS").Visible = True
            Else
                preparation.FindControl("RightIcnS").Visible = False
            End If

            'アイコンの文言設定
            CType(preparation.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
            CType(preparation.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
            CType(preparation.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS

            divDeskDevice = CType(preparation.FindControl("PreparationDeskDevice"), HtmlContainerControl)
            With divDeskDevice
                .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)
                .Attributes("orderNo") = row.ORDERNO
                .Attributes("approvalId") = row.APPROVAL_ID
                ' 仕掛中チェック
                If Me.SetNullToString(strStart, "0").Equals("0") Then
                    .Attributes("class") = "ColumnContentsBoder"
                Else
                    .Attributes("class") = "ColumnContentsBoder ColumnBoxAqua"
                End If
            End With

            divElapsedTime = DirectCast(preparation.FindControl("PreparationElapsedTime"), HtmlContainerControl)
            With divElapsedTime
                .InnerText = C_MIN_TIME_DISP
                .Attributes("name") = "procgroup"
                .Attributes("procdate") = CType(row.PROC_DATE, String)
                .Attributes("overclass1") = "ColumnTimeRed"
                .Attributes("overclass2") = String.Empty
                .Attributes("overseconds1") = CType(-Me.mlngDeliverypreAbnormalLt * 60, String)
                .Attributes("overseconds2") = String.Empty
            End With
        Next
        ' 2012/02/27 KN 西田【SERVICE_1】END

        'データ表示件数を表示する
        Me.PreparationNumber.Text = PreparationRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 納車作業エリアチップ初期設定
    ''' </summary>
    ''' <param name="dt">チップ情報</param>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub InitDelivery(ByVal dt As SC3140103DataSet.SC3140103VisitChipDataTable, ByVal strRightIcnD As String, ByVal strRightIcnI As String, ByVal strRightIcnS As String)

        ' コントロールにバインドする
        Me.DeliveryRepeater.DataSource = dt.Select(String.Format(CultureInfo.CurrentCulture, "DISP_DIV = '{0}'", SC3140103BusinessLogic.DisplayDivDelivery), "DISP_SORT")
        Me.DeliveryRepeater.DataBind()

        Dim rowList As SC3140103DataSet.SC3140103VisitChipRow() = DirectCast(DeliveryRepeater.DataSource, SC3140103DataSet.SC3140103VisitChipRow())

        ' 2012/02/27 KN 西田【SERVICE_1】START
        Dim delivery As Control
        Dim row As SC3140103DataSet.SC3140103VisitChipRow

        Dim strRegistrationNumber As String
        Dim strCustomerName As String
        Dim strDeliveryPlanTime As String
        Dim strRepresentativeWarehousing As String
        Dim strChargeTechnician As String

        Dim strReserveMark As String
        Dim strJDPMark As String
        Dim strSSCMark As String
        Dim strStart As String

        Dim divDeskDevice As HtmlContainerControl
        Dim divElapsedTime As HtmlContainerControl
        ' -----------------------------------------------------
        ' データを設定する
        ' -----------------------------------------------------
        For i = 0 To DeliveryRepeater.Items.Count - 1

            delivery = DeliveryRepeater.Items(i)
            row = rowList(i)

            strRegistrationNumber = row.VCLREGNO
            strCustomerName = row.CUSTOMERNAME
            strDeliveryPlanTime = row.ITEM_DATE.ToString(CultureInfo.CurrentCulture)
            strRepresentativeWarehousing = row.MERCHANDISENAME
            strChargeTechnician = row.STAFFNAME

            strReserveMark = row.REZ_MARK
            strJDPMark = row.JDP_MARK
            strSSCMark = row.SSC_MARK
            strStart = row.DISP_START

            CType(delivery.FindControl("DeliveryRegistrationNumber"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
            CType(delivery.FindControl("DeliveryCustomerName"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
            CType(delivery.FindControl("DeliveryDeliveryPlanTime"), HtmlContainerControl).InnerHtml = Me.SetTimeFromToAppend(Me.SetDateStringToString(strDeliveryPlanTime), ChipArea.Delivery)
            CType(delivery.FindControl("DeliveryRepresentativeWarehousing"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)
            CType(delivery.FindControl("DeliveryChargeTechnician"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strChargeTechnician, C_DEFAULT_CHIP_SPACE)

            If Me.SetNullToString(strReserveMark, "0").Equals("1") Then
                delivery.FindControl("RightIcnD").Visible = True
            Else
                delivery.FindControl("RightIcnD").Visible = False
            End If

            If Me.SetNullToString(strJDPMark, "0").Equals("1") Then
                delivery.FindControl("RightIcnI").Visible = True
            Else
                delivery.FindControl("RightIcnI").Visible = False
            End If

            If Me.SetNullToString(strSSCMark, "0").Equals("1") Then
                delivery.FindControl("RightIcnS").Visible = True
            Else
                delivery.FindControl("RightIcnS").Visible = False
            End If

            'アイコンの文言設定
            CType(delivery.FindControl("RightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
            CType(delivery.FindControl("RightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
            CType(delivery.FindControl("RightIcnS"), HtmlContainerControl).InnerText = strRightIcnS

            divDeskDevice = CType(delivery.FindControl("DeliveryDeskDevice"), HtmlContainerControl)
            With divDeskDevice
                .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)
                .Attributes("orderNo") = row.ORDERNO
                .Attributes("approvalId") = row.APPROVAL_ID

                ' 仕掛中チェック
                If Me.SetNullToString(strStart, "0").Equals("0") Then
                    .Attributes("class") = "ColumnContentsBoder"
                Else
                    .Attributes("class") = "ColumnContentsBoder ColumnBoxAqua"
                End If
            End With

            divElapsedTime = DirectCast(delivery.FindControl("DeliveryElapsedTime"), HtmlContainerControl)
            With divElapsedTime
                .InnerText = C_MIN_TIME_DISP
                .Attributes("name") = "procgroup"
                .Attributes("procdate") = CType(row.PROC_DATE, String)
                .Attributes("overclass1") = "ColumnTimeRed"
                .Attributes("overclass2") = String.Empty
                .Attributes("overSeconds1") = CType(-Me.mlngDeliverywrAbnormalLt * 60, String)
                .Attributes("overSeconds2") = String.Empty
            End With
        Next
        ' 2012/02/27 KN 西田【SERVICE_1】END

        'データ表示件数を表示する
        Me.DeliveryNumber.Text = DeliveryRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 作業中エリアチップ初期設定
    ''' </summary>
    ''' <param name="dt">チップ情報</param>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub InitWork(ByVal dt As SC3140103DataSet.SC3140103VisitChipDataTable, ByVal strRightIcnD As String, ByVal strRightIcnI As String, ByVal strRightIcnS As String)

        ' コントロールにバインドする
        Me.WorkRepeater.DataSource = dt.Select(String.Format(CultureInfo.CurrentCulture, "DISP_DIV = '{0}'", SC3140103BusinessLogic.DisplayDivWork), "DISP_SORT")
        Me.WorkRepeater.DataBind()

        Dim rowList As SC3140103DataSet.SC3140103VisitChipRow() = DirectCast(WorkRepeater.DataSource, SC3140103DataSet.SC3140103VisitChipRow())

        ' 2012/02/27 KN 西田【SERVICE_1】START
        Dim work As Control
        Dim row As SC3140103DataSet.SC3140103VisitChipRow

        Dim strCompletionPlanTime As String
        Dim strRegistrationNumber As String
        Dim strCustomerName As String
        Dim strDeliveryPlanTime As String
        Dim strRepresentativeWarehousing As String
        Dim strAdditionalWorkNumber As String

        Dim strReserveMark As String
        Dim strJDPMark As String
        Dim strSSCMark As String
        Dim strStart As String

        Dim divDeskDevice As HtmlContainerControl
        Dim divElapsedTime As HtmlContainerControl
        ' -----------------------------------------------------
        ' データを設定する
        ' -----------------------------------------------------
        For i = 0 To WorkRepeater.Items.Count - 1

            work = WorkRepeater.Items(i)
            row = rowList(i)

            strCompletionPlanTime = row.PROC_DATE.ToString(CultureInfo.CurrentCulture)
            strRegistrationNumber = row.VCLREGNO
            strCustomerName = row.CUSTOMERNAME
            strDeliveryPlanTime = row.ITEM_DATE.ToString(CultureInfo.CurrentCulture)
            strRepresentativeWarehousing = row.MERCHANDISENAME
            strAdditionalWorkNumber = row.APPROVAL_COUNT.ToString(CultureInfo.CurrentCulture)

            strReserveMark = row.REZ_MARK
            strJDPMark = row.JDP_MARK
            strSSCMark = row.SSC_MARK
            strStart = row.DISP_START

            CType(work.FindControl("WorkTimeLag"), HtmlContainerControl).InnerText = C_MIN_TIME_DISP

            CType(work.FindControl("WorkCompletionPlanTime"), HtmlContainerControl).InnerHtml = Me.SetDateStringToString(strCompletionPlanTime)
            CType(work.FindControl("WorkRegistrationNumber"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strRegistrationNumber, C_DEFAULT_CHIP_SPACE)
            CType(work.FindControl("WorkCustomerName"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strCustomerName, C_DEFAULT_CHIP_SPACE)
            CType(work.FindControl("WorkDeliveryPlanTime"), HtmlContainerControl).InnerHtml = Me.SetTimeFromToAppend(Me.SetDateStringToString(strDeliveryPlanTime), ChipArea.Work)
            CType(work.FindControl("WorkRepresentativeWarehousing"), HtmlContainerControl).InnerHtml = Me.SetNullToString(strRepresentativeWarehousing, C_DEFAULT_CHIP_SPACE)

            Dim lngAdditionalWorkNumber As Long = 0
            If Not Long.TryParse(strAdditionalWorkNumber, lngAdditionalWorkNumber) Then
                lngAdditionalWorkNumber = 0
            End If
            If lngAdditionalWorkNumber > 0 Then
                ' 画像表示
                CType(work.FindControl("AdditionalWorkNumber"), HtmlContainerControl).InnerHtml = strAdditionalWorkNumber
                CType(work.FindControl("WorkIcon"), HtmlContainerControl).Attributes("class") = "Icn01"
            Else
                ' 画像非表示
                CType(work.FindControl("AdditionalWorkNumber"), HtmlContainerControl).InnerHtml = String.Empty
            End If

            If Me.SetNullToString(strReserveMark, "0").Equals("1") Then
                work.FindControl("WorkRightIcnD").Visible = True
            Else
                work.FindControl("WorkRightIcnD").Visible = False
            End If

            If Me.SetNullToString(strJDPMark, "0").Equals("1") Then
                work.FindControl("WorkRightIcnI").Visible = True
            Else
                work.FindControl("WorkRightIcnI").Visible = False
            End If

            If Me.SetNullToString(strSSCMark, "0").Equals("1") Then
                work.FindControl("WorkRightIcnS").Visible = True
            Else
                work.FindControl("WorkRightIcnS").Visible = False
            End If

            'アイコンの文言設定
            CType(work.FindControl("WorkRightIcnD"), HtmlContainerControl).InnerText = strRightIcnD
            CType(work.FindControl("WorkRightIcnI"), HtmlContainerControl).InnerText = strRightIcnI
            CType(work.FindControl("WorkRightIcnS"), HtmlContainerControl).InnerText = strRightIcnS

            divDeskDevice = CType(work.FindControl("Working"), HtmlContainerControl)
            With divDeskDevice
                .Attributes("visitNo") = row.VISITSEQ.ToString(CultureInfo.CurrentCulture)
                .Attributes("orderNo") = row.ORDERNO
                .Attributes("approvalId") = row.APPROVAL_ID

                ' 仕掛中チェック
                If Me.SetNullToString(strStart, "0").Equals("0") Then
                    .Attributes("class") = "ColumnContents02Boder"
                Else
                    .Attributes("class") = "ColumnContents02Boder ColumnBoxAqua"
                End If
            End With

            divElapsedTime = DirectCast(work.FindControl("WorkTimeLag"), HtmlContainerControl)
            With divElapsedTime
                .InnerText = C_MIN_TIME_DISP
                .Attributes("name") = "procgroup"
                .Attributes("procdate") = CType(row.PROC_DATE, String)
                .Attributes("overclass1") = "ColumnTimeRed"
                .Attributes("overclass2") = String.Empty
                .Attributes("overSeconds1") = "0"
                .Attributes("overSeconds2") = String.Empty
            End With
        Next
        ' 2012/02/27 KN 西田【SERVICE_1】END

        'データ表示件数を表示する
        Me.WorkNumber.Text = WorkRepeater.Items.Count.ToString(CultureInfo.CurrentCulture)

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' チップ詳細情報表示
    ''' </summary>
    ''' <remarks>
    ''' チップ詳細画面に表示する情報を取得し、設定します。
    ''' </remarks>
    '''-----------------------------------------------------------------------
    Private Sub InitVisitChipDetail()

        Dim bl As SC3140103BusinessLogic = New SC3140103BusinessLogic()
        Dim dt As SC3140103DataSet.SC3140103VisitChipDetailDataTable

        ' サービス来店者情報取得(チップ詳細)
        Dim visitSeq As Long = SetNullToLong(Me.DetailsVisitNo.Value)
        dt = bl.GetVisitChipDetail(visitSeq, Me.DetailsArea.Value)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim row As SC3140103DataSet.SC3140103VisitChipDetailRow = DirectCast(dt.Rows(0), SC3140103DataSet.SC3140103VisitChipDetailRow)

            If row.REZ_MARK.Equals("1") Then
                Me.DetailsRightIconD.Visible = True
            Else
                Me.DetailsRightIconD.Visible = False
            End If

            If row.JDP_MARK.Equals("1") Then
                Me.DetailsRightIconI.Visible = True
            Else
                Me.DetailsRightIconI.Visible = False
            End If

            If row.SSC_MARK.Equals("1") Then
                Me.DetailsRightIconS.Visible = True
            Else
                Me.DetailsRightIconS.Visible = False
            End If

            ' 登録番号
            Me.DetailsRegistrationNumber.Text = Me.SetNullToString(row.VCLREGNO)
            ' 車種
            Me.DetailsCarModel.Text = Me.SetNullToString(row.VEHICLENAME)
            ' モデル
            Me.DetailsModel.Text = Me.SetNullToString(row.MODELCODE)
            ' VIN
            Me.DetailsVin.Text = Me.SetNullToString(row.VIN)
            ' 走行距離
            If row.MILEAGE >= 0 Then
                Me.DetailsMileage.Text = String.Format(CultureInfo.CurrentCulture, "{0:#,0}", row.MILEAGE)
            Else
                Me.DetailsMileage.Text = String.Empty
            End If
            ' 納車予定日時
            If Me.IsDateTimeNull(row.REZ_DELI_DATE) Then
                Me.DetailsDeliveryCarDay.Text = String.Empty
            Else
                Me.DetailsDeliveryCarDay.Text = DateTimeFunc.FormatDate(3, row.REZ_DELI_DATE)
            End If
            ' 顧客名
            Me.DetailsCustomerName.Text = Me.SetNullToString(row.CUSTOMERNAME)
            ' 電話番号
            Me.DetailsPhoneNumber.Text = Me.SetNullToString(row.TELNO)
            ' 携帯番号
            Me.DetailsMobileNumber.Text = Me.SetNullToString(row.MOBILE)
            ' 代表入庫項目
            Me.DetailsRepresentativeWarehousing.Text = Me.SetNullToString(row.MERCHANDISENAME)

            ' 表示区分
            Select Case Me.DetailsArea.Value
                Case SC3140103BusinessLogic.DisplayDivReception       ' 受付
                    ' 来店時刻
                    Me.ItemTime.Text = WebWordUtility.GetWord(APPLICATIONID, 20)
                    Me.DetailsVisitTime.Text = Me.SetDateTimeToString(row.VISITTIMESTAMP)

                Case SC3140103BusinessLogic.DisplayDivApproval,
                     SC3140103BusinessLogic.DisplayDivPreparation,
                     SC3140103BusinessLogic.DisplayDivDelivery        ' 承認依頼・納車準備・納車作業
                    ' 納車予定日時
                    Me.ItemTime.Text = WebWordUtility.GetWord(APPLICATIONID, 28)
                    Me.DetailsVisitTime.Text = Me.SetDateTimeToString(row.REZ_DELI_DATE)

                Case SC3140103BusinessLogic.DisplayDivWork            ' 作業中
                    ' 作業開始 ～ 作業終了予定時刻
                    Me.ItemTime.Text = WebWordUtility.GetWord(APPLICATIONID, 25)
                    Me.DetailsVisitTime.Text = String.Format(CultureInfo.CurrentCulture, "{0}{1}{2}", Me.SetDateTimeToString(row.ACTUAL_STIME),
                                               WebWordUtility.GetWord(APPLICATIONID, 32),
                                               Me.SetDateTimeToString(row.ENDTIME))

                Case Else

            End Select

            ' ボタン表示
            Me.DetailButtonLeft.Text = Me.InitVisitChipDetailButton(row.BUTTON_LEFT)
            Me.DetailButtonLeft.Enabled = row.BUTTON_ENABLED_LEFT
            Me.DetailButtonRight.Text = Me.InitVisitChipDetailButton(row.BUTTON_RIGHT)
            Me.DetailButtonRight.Enabled = row.BUTTON_ENABLED_RIGHT

        End If

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' チップ詳細フッタボタン文言初期設定
    ''' </summary>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Function InitVisitChipDetailButton(ByVal button As String) As String

        ' ボタンチェック
        Select Case button
            Case SC3140103BusinessLogic.ButtonCustomer          ' 顧客詳細ボタン
                Return WebWordUtility.GetWord(APPLICATIONID, 24)

            Case SC3140103BusinessLogic.ButtonNewCustomer       ' 新規顧客登録ボタン
                Return WebWordUtility.GetWord(APPLICATIONID, 22)

            Case SC3140103BusinessLogic.ButtonNewRO             ' R/O作成ボタン
                Return WebWordUtility.GetWord(APPLICATIONID, 23)

            Case SC3140103BusinessLogic.ButtonRODisplay         ' R/O参照ボタン
                Return WebWordUtility.GetWord(APPLICATIONID, 26)

            Case SC3140103BusinessLogic.ButtonWork              ' 追加作業登録ボタン
                Return WebWordUtility.GetWord(APPLICATIONID, 27)

            Case SC3140103BusinessLogic.ButtonApproval          ' 追加作業承認ボタン
                Return WebWordUtility.GetWord(APPLICATIONID, 29)

            Case SC3140103BusinessLogic.ButtonCheckSheet        ' チェックシートボタン
                Return WebWordUtility.GetWord(APPLICATIONID, 30)

            Case SC3140103BusinessLogic.ButtonSettlement        ' 清算入力ボタン
                Return WebWordUtility.GetWord(APPLICATIONID, 31)

            Case Else
                Return String.Empty

        End Select

    End Function

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' チップ詳細からの画面遷移処理 (ボタン用)
    ''' </summary>
    ''' <param name="buttonText">ボタン名称</param>
    '''-----------------------------------------------------------------------
    Private Sub NextScreenVisitChipDetailButton(ByVal buttonText As String)

        ' 選択チップ取得
        Dim visitSeq As Long = SetNullToLong(Me.DetailsVisitNo.Value)
        Dim orderNo As String = SetNullToString(Me.DetailsOrderNo.Value)
        Dim approvalId As String = SetNullToString(Me.DetailsApprovalId.Value)

        ' ボタン名称に応じて遷移先変更
        Select Case buttonText
            Case WebWordUtility.GetWord(APPLICATIONID, 24)          ' 顧客詳細
                Me.RedirectCustomer(visitSeq)                       ' 顧客詳細画面に遷移

            Case WebWordUtility.GetWord(APPLICATIONID, 22)          ' 新規顧客登録
                Me.RedirectCustomer(visitSeq)                       ' 新規顧客登録画面に遷移

            Case WebWordUtility.GetWord(APPLICATIONID, 23)          ' R/O作成
                Me.RedirectOrderNew(visitSeq)                       ' R/O作成画面に遷移

            Case WebWordUtility.GetWord(APPLICATIONID, 26)          ' R/O参照
                Me.RedirectOrderDisp(orderNo)                       ' R/O参照画面に遷移

            Case WebWordUtility.GetWord(APPLICATIONID, 27)          ' 追加作業登録
                Me.RedirectWork(orderNo)                            ' 追加作業登録画面に遷移

            Case WebWordUtility.GetWord(APPLICATIONID, 29)          ' 追加作業承認
                Me.RedirectApproval(orderNo, approvalId)            ' 追加作業承認画面に遷移

            Case WebWordUtility.GetWord(APPLICATIONID, 30)          ' チェックシート
                Me.RedirectCheckSheet(orderNo)                      ' チェックシート印刷画面に遷移

            Case WebWordUtility.GetWord(APPLICATIONID, 31)          ' 清算入力
                Me.RedirectSettlement(orderNo)                      ' 清算印刷画面に遷移

            Case Else
                '遷移しない

        End Select

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 時間変換 (hh:mm) 又は (mm/dd)　
    ''' </summary>
    ''' <param name="time">対象時間</param>
    ''' <returns>変換値</returns>
    '''-----------------------------------------------------------------------
    Private Function SetDateTimeToString(ByVal time As DateTime) As String

        Dim strResult As String

        ' 日付チェック
        If time.Equals(DateTime.MinValue) Then
            Return String.Empty
        End If

        ' 時間範囲チェック
        If Me.mNow.ToString("yyyyMMdd", CultureInfo.CurrentCulture).Equals(time.ToString("yyyyMMdd", CultureInfo.CurrentCulture)) Then
            ' 当日 (hh:mm)
            strResult = DateTimeFunc.FormatDate(14, time)
        Else
            ' 上記以外 (mm/dd)
            strResult = DateTimeFunc.FormatDate(11, time)
        End If

        Return strResult

    End Function

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 時間変換 (hh:mm) 又は (mm/dd)　
    ''' </summary>
    ''' <param name="time">対象時間</param>
    ''' <returns>変換値</returns>
    '''-----------------------------------------------------------------------
    Private Function SetDateStringToString(ByVal time As String) As String

        ' 空白チェック
        If String.IsNullOrEmpty(time) Then
            Return String.Empty
        End If

        ' 日付チェック
        Dim result As DateTime
        If Not DateTime.TryParse(time, result) Then
            Return String.Empty
        End If
        If result.Equals(DateTime.MinValue) Then
            Return String.Empty
        End If

        Return SetDateTimeToString(result)

    End Function

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 文字列変換
    ''' </summary>
    ''' <param name="str"></param>
    ''' <returns>変換値</returns>
    '''-----------------------------------------------------------------------
    Private Function SetNullToString(ByVal str As String, Optional ByVal strNull As String = "") As String

        ' 空白チェック
        If String.IsNullOrEmpty(str) Then
            Return strNull
        End If
        If String.IsNullOrEmpty(str.Trim()) Then
            Return strNull
        End If

        Return str

    End Function

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 数値変換
    ''' </summary>
    ''' <param name="num"></param>
    ''' <returns>変換値</returns>
    '''-----------------------------------------------------------------------
    Private Function SetNullToLong(ByVal num As String, Optional ByVal lngNull As Long = 0) As Long

        Dim result As Long
        If Not Long.TryParse(num, result) Then
            result = lngNull
        End If
        If result = 0 Then
            result = lngNull
        End If

        Return result

    End Function

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 時間チェック
    ''' </summary>
    ''' <param name="time">対象時間</param>
    ''' <returns></returns>
    ''' <remarks>True: Null(MinValue), false: Null(MinValue)以外</remarks>
    '''-----------------------------------------------------------------------
    Private Function IsDateTimeNull(ByVal time As DateTime) As Boolean

        ' 日付チェック
        If time.Equals(DateTime.MinValue) Then
            Return True
        End If

        Return False

    End Function

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 固定文字列付与「～」
    ''' </summary>
    ''' <param name="appendTime">付与対象文字列</param>
    ''' <param name="ChipArea">工程管理エリア</param>
    ''' <returns>固定文字列付与値</returns>
    '''-----------------------------------------------------------------------
    Private Function SetTimeFromToAppend(ByVal appendTime As String, ByVal chipArea As ChipArea) As String

        ' 空白チェック
        If String.IsNullOrEmpty(appendTime) Then
            Return String.Empty
        End If

        ' 工程管理エリア確認
        Dim rtnVal As StringBuilder = New StringBuilder
        With rtnVal
            ' 工程管理エリア確認
            Select Case chipArea
                Case chipArea.Reception                                                         ' 受付エリア
                    ' XXX～
                    .Append(appendTime)
                    .Append(mWordFixedString)
                Case chipArea.Approval, chipArea.Preparation, chipArea.Delivery, chipArea.Work  ' 追加承認エリア、納車準備エリア、納車作業エリア、作業エリア
                    ' ～XXX
                    .Append(mWordFixedString)
                    .Append(appendTime)
                Case Else
                    .Append(appendTime)
            End Select
        End With

        Return rtnVal.ToString

    End Function

#End Region

#Region " 画面遷移メソッド"

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 顧客詳細画面・新規顧客登録画面に遷移
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <remarks>
    ''' 来店実績連番を元に、
    ''' 顧客詳細画面又は、新規顧客登録画面に遷移します。
    ''' </remarks>
    '''-----------------------------------------------------------------------
    Private Sub RedirectCustomer(ByVal visitSeq As Long)

        Dim bl As SC3140103BusinessLogic = New SC3140103BusinessLogic
        Dim dt As SC3140103DataSet.SC3140103VisitDataTable

        ' サービス来店実績取得
        dt = bl.GetVisitChipDetail(visitSeq)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
            Dim row As SC3140103DataSet.SC3140103VisitRow = DirectCast(dt.Rows(0), SC3140103DataSet.SC3140103VisitRow)

            ' 顧客区分チェック
            If Me.SetNullToString(row.CUSTSEGMENT, "0").Equals(SC3140103BusinessLogic.CustomerSegmentON) Then
                ' 自社客

                Dim logOutCust As StringBuilder = New StringBuilder(String.Empty)
                With logOutCust
                    .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
                    .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", C_APPLICATIONID_CUSTOMEROUT))
                    .Append(String.Format(CultureInfo.CurrentCulture, "VISITSEQ = {0}", row.VISITSEQ))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", NAME = {0}", row.CUSTOMERNAME))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", REGISTERNO = {0}", row.VCLREGNO))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", VINNO = {0}", row.VIN))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", MODELCODE = {0}", row.MODELCODE))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", TEL1 = {0}", row.TELNO))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", TEL2 = {0}", row.MOBILE))
                    ' 2012/02/17 KN 森下【SERVICE_1】START
                    .Append(String.Format(CultureInfo.CurrentCulture, ", CRDEALERCODE = {0}", row.DLRCD))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", FLAG = {0}", "1"))
                    ' 2012/02/17 KN 森下【SERVICE_1】END
                End With
                Logger.Info(logOutCust.ToString())

                ' 次画面遷移パラメータ設定
                Me.SetValue(ScreenPos.Next, "Redirect.VISITSEQ", row.VISITSEQ)     ' 来店者ID
                Me.SetValue(ScreenPos.Next, "Redirect.NAME", row.CUSTOMERNAME)     ' 顧客名
                Me.SetValue(ScreenPos.Next, "Redirect.REGISTERNO", row.VCLREGNO)   ' 車両登録No
                Me.SetValue(ScreenPos.Next, "Redirect.VINNO", row.VIN)             ' ＶＩＮ
                Me.SetValue(ScreenPos.Next, "Redirect.MODELCODE", row.MODELCODE)   ' モデルコード
                Me.SetValue(ScreenPos.Next, "Redirect.TEL1", row.TELNO)            ' 電話番号
                Me.SetValue(ScreenPos.Next, "Redirect.TEL2", row.MOBILE)           ' 携帯番号
                ' 2012/02/17 KN 森下【SERVICE_1】START
                Me.SetValue(ScreenPos.Next, "Redirect.CRDEALERCODE", row.DLRCD)    ' DLRコード
                Me.SetValue(ScreenPos.Next, "Redirect.FLAG", "1")                  ' 固定フラグ
                ' 2012/02/17 KN 森下【SERVICE_1】END

                ' 顧客詳細画面に遷移
                Me.RedirectNextScreen(C_APPLICATIONID_CUSTOMEROUT)
            Else
                ' 未取引客

                Dim logNewCust As StringBuilder = New StringBuilder(String.Empty)
                With logNewCust
                    .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
                    .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", C_APPLICATIONID_CUSTOMERNEW))
                    .Append(String.Format(CultureInfo.CurrentCulture, "VISITSEQ = {0}", row.VISITSEQ))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", NAME = {0}", row.CUSTOMERNAME))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", REGISTERNO = {0}", row.VCLREGNO))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", VINNO = {0}", row.VIN))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", MODELCODE = {0}", row.MODELCODE))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", TEL1 = {0}", row.TELNO))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", TEL2 = {0}", row.MOBILE))
                End With
                Logger.Info(logNewCust.ToString())

                ' 次画面遷移パラメータ設定
                Me.SetValue(ScreenPos.Next, "Redirect.VISITSEQ", row.VISITSEQ)     ' 来店者ID
                Me.SetValue(ScreenPos.Next, "Redirect.NAME", row.CUSTOMERNAME)     ' 顧客名 
                Me.SetValue(ScreenPos.Next, "Redirect.REGISTERNO", row.VCLREGNO)   ' 車両登録No
                Me.SetValue(ScreenPos.Next, "Redirect.VINNO", row.VIN)             ' ＶＩＮ
                Me.SetValue(ScreenPos.Next, "Redirect.MODELCODE", row.MODELCODE)   ' モデルコード
                Me.SetValue(ScreenPos.Next, "Redirect.TEL1", row.TELNO)            ' 電話番号
                Me.SetValue(ScreenPos.Next, "Redirect.TEL2", row.MOBILE)           ' 携帯番号

                ' 新規顧客登録画面に遷移
                Me.RedirectNextScreen(C_APPLICATIONID_CUSTOMERNEW)
            End If

        End If

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 新規顧客登録画面に遷移
    ''' </summary>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub RedirectCustomerNew()

        Dim logNewCust As StringBuilder = New StringBuilder(String.Empty)
        With logNewCust
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0}", C_APPLICATIONID_CUSTOMERNEW))
        End With
        Logger.Info(logNewCust.ToString())

        ' 新規顧客登録画面に遷移
        Me.RedirectNextScreen(C_APPLICATIONID_CUSTOMERNEW)
    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' R/O作成画面に遷移
    ''' </summary>
    ''' <param name="visitSeq">来店実績連番</param>
    ''' <remarks>
    ''' 来店実績連番を元に来店管理情報を取得し、
    ''' 当該情報の整備受注NOが未発行の場合は、新規に整備受注NOを発行し、
    ''' R/O作成画面に遷移します。
    ''' また当該情報の顧客が未取引客の場合、R/O作成画面には遷移しません。
    ''' </remarks>
    '''-----------------------------------------------------------------------
    Private Sub RedirectOrderNew(ByVal visitSeq As Long)

        Dim staffInfo As StaffContext = StaffContext.Current

        ' 整備受注NO 作成情報(0-整備受注NO、1-UpDate結果)
        Dim createOrderInformation(2) As String

        Dim bl As SC3140103BusinessLogic = New SC3140103BusinessLogic
        Dim dt As SC3140103DataSet.SC3140103VisitDataTable

        ' サービス来店実績取得
        dt = bl.GetVisitChipDetail(visitSeq)

        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then

            Dim row As SC3140103DataSet.SC3140103VisitRow = DirectCast(dt.Rows(0), SC3140103DataSet.SC3140103VisitRow)

            ' 顧客区分チェック
            If Me.SetNullToString(row.CUSTSEGMENT, "0").Equals(SC3140103BusinessLogic.CustomerSegmentON) Then
                ' 自社客

                ' 整備受注NO
                Dim orderNo As String = String.Empty

                ' 整備受注NO発行チェック
                If Not String.IsNullOrEmpty(row.ORDERNO) Then
                    ' 発行済み
                    orderNo = Me.SetNullToString(row.ORDERNO)
                Else
                    ' 整備受注NO反映確認番号
                    Dim UpDateCheck As Long

                    ' 未発行
                    ' 整備受注NO発行処理
                    createOrderInformation = bl.GetIFCreateOrderNo(row, visitSeq, staffInfo) ' 外部インターフェース(BMTS)
                    ' 反映結果格納
                    UpDateCheck = Long.Parse(createOrderInformation(1), CultureInfo.CurrentCulture)
                    ' 反映結果確認
                    Select Case UpDateCheck
                        Case C_RET_SUCCESS
                            orderNo = createOrderInformation(0) ' 整備受注NO
                        Case C_RET_DBTIMEOUT
                            Me.ShowMessageBox(MsgID.id901)   ' タイムアウト
                            Return
                        Case C_RET_NOMATCH
                            Me.ShowMessageBox(MsgID.id902)   ' その他
                            Return
                        Case C_RET_DIFFSACODE
                            Me.ShowMessageBox(MsgID.id903)   ' 担当SA外
                            Return
                        Case Else
                            Me.ShowMessageBox(MsgID.id902)   ' その他
                            Return
                    End Select

                End If

                Dim logNewOrder As StringBuilder = New StringBuilder(String.Empty)
                With logNewOrder
                    .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
                    .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", C_APPLICATIONID_ORDERNEW))
                    .Append(String.Format(CultureInfo.CurrentCulture, "OrderNo = {0}", orderNo))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", VISITSEQ = {0}", row.VISITSEQ))
                    .Append(String.Format(CultureInfo.CurrentCulture, ", REZID = {0}", row.FREZID))
                End With
                Logger.Info(logNewOrder.ToString())

                ' 次画面遷移パラメータ設定
                Me.SetValue(ScreenPos.Next, "OrderNo", orderNo)           ' R/O ID
                Me.SetValue(ScreenPos.Next, "VISITSEQ", row.VISITSEQ)     ' 来店者ID
                Me.SetValue(ScreenPos.Next, "REZID", row.FREZID)          ' 予約ID

                ' R/O作成画面に遷移
                Me.RedirectNextScreen(C_APPLICATIONID_ORDERNEW)

            Else
                ' 未取引客はR/O作成画面に遷移させない
                Me.ShowMessageBox(MsgID.id904)  ' 未取引客
                Return
            End If

        End If

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' R/O参照画面に遷移
    ''' </summary>
    ''' <param name="orderNo">整備受注NO</param>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub RedirectOrderDisp(ByVal orderNo As String)

        Dim logOutOrder As StringBuilder = New StringBuilder(String.Empty)
        With logOutOrder
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", C_APPLICATIONID_ORDEROUT))
            .Append(String.Format(CultureInfo.CurrentCulture, "OrderNo = {0}", orderNo))
        End With
        Logger.Info(logOutOrder.ToString())

        ' 次画面遷移パラメータ設定
        Me.SetValue(ScreenPos.Next, "OrderNo", orderNo)   ' R/O ID

        ' R/O参照画面に遷移
        Me.RedirectNextScreen(C_APPLICATIONID_ORDEROUT)
    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' R/O一覧画面に遷移
    ''' </summary>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub RedirectOrderList()

        Dim logOrderList As StringBuilder = New StringBuilder(String.Empty)
        With logOrderList
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0}", C_APPLICATIONID_ORDERLIST))
        End With
        Logger.Info(logOrderList.ToString())

        ' R/O一覧画面に遷移
        Me.RedirectNextScreen(C_APPLICATIONID_ORDERLIST)
    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 追加作業登録画面に遷移
    ''' </summary>
    ''' <param name="orderNo">整備受注NO</param>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub RedirectWork(ByVal orderNo As String)

        Dim logWork As StringBuilder = New StringBuilder(String.Empty)
        With logWork
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", C_APPLICATIONID_WORK))
            .Append(String.Format(CultureInfo.CurrentCulture, "ORDERNO = {0}", orderNo))
            .Append(String.Format(CultureInfo.CurrentCulture, ", EDITFLG = {0}", 0))
            .Append(String.Format(CultureInfo.CurrentCulture, ", SRVADDSEQ = {0}", 0))
        End With
        Logger.Info(logWork.ToString())

        ' 次画面遷移パラメータ設定
        Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNo)   ' R/O ID
        Me.SetValue(ScreenPos.Next, "Redirect.EDITFLG", 0)         ' 編集フラグ
        Me.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", 0)       ' 追加作業ユニークID

        ' 追加作業登録画面に遷移
        Me.RedirectNextScreen(C_APPLICATIONID_WORK)

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 追加作業承認画面に遷移
    ''' </summary>
    ''' <param name="orderNo">整備受注NO</param>
    ''' <param name="approvalId">追加承認待ちID</param>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub RedirectApproval(ByVal orderNo As String, ByVal approvalId As String)

        Dim logApproval As StringBuilder = New StringBuilder(String.Empty)
        With logApproval
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", C_APPLICATIONID_APPROVAL))
            .Append(String.Format(CultureInfo.CurrentCulture, "ORDERNO = {0}", orderNo))
            .Append(String.Format(CultureInfo.CurrentCulture, ", SRVADDSEQ = {0}", approvalId))
            .Append(String.Format(CultureInfo.CurrentCulture, ", EDITFLG = {0}", 0))
        End With
        Logger.Info(logApproval.ToString())

        '次画面遷移パラメータ設定
        Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNo)       ' R/O ID
        Me.SetValue(ScreenPos.Next, "Redirect.SRVADDSEQ", approvalId)  ' 追加作業ユニークID
        Me.SetValue(ScreenPos.Next, "Redirect.EDITFLG", 0)             ' 編集フラグ

        ' 追加作業承認画面に遷移
        Me.RedirectNextScreen(C_APPLICATIONID_APPROVAL)

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' チェックシート印刷画面に遷移
    ''' </summary>
    ''' <param name="orderNo">整備受注NO</param>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub RedirectCheckSheet(ByVal orderNo As String)

        Dim logCheckSheet As StringBuilder = New StringBuilder(String.Empty)
        With logCheckSheet
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", C_APPLICATIONID_CHECKSHEET))
            .Append(String.Format(CultureInfo.CurrentCulture, "ORDERNO = {0}", orderNo))
        End With
        Logger.Info(logCheckSheet.ToString())

        ' 次画面遷移パラメータ設定
        Me.SetValue(ScreenPos.Next, "Redirect.ORDERNO", orderNo)   ' R/O ID

        ' チェックシート印刷画面に遷移
        Me.RedirectNextScreen(C_APPLICATIONID_CHECKSHEET)

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 清算印刷画面に遷移
    ''' </summary>
    ''' <param name="orderNo">整備受注NO</param>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub RedirectSettlement(ByVal orderNo As String)

        Dim logSettlement As StringBuilder = New StringBuilder(String.Empty)
        With logSettlement
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0} IN:", C_APPLICATIONID_SETTLEMENT))
            .Append(String.Format(CultureInfo.CurrentCulture, "OrderNo = {0}", orderNo))
        End With
        Logger.Info(logSettlement.ToString())

        ' 次画面遷移パラメータ設定
        Me.SetValue(ScreenPos.Next, "OrderNo", orderNo)   ' R/O ID

        ' 清算印刷画面に遷移
        Me.RedirectNextScreen(C_APPLICATIONID_SETTLEMENT)

    End Sub

#End Region

#Region " フッター制御 "

    ''' <summary>
    ''' メインメニュー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAIN_MENU As Integer = 100
    ''' <summary>
    ''' 顧客情報
    ''' </summary>
    Private Const CUSTOMER_INFORMATION As Integer = 200
    ''' <summary>
    ''' 説明ツール
    ''' </summary>
    Private Const SUBMENU_EXPLANATION_TOOL As Integer = 700
    ''' <summary>
    ''' R/O作成
    ''' </summary>
    Private Const SUBMENU_RO_MAKE As Integer = 600
    ''' <summary>
    ''' スケジューラ
    ''' </summary>
    Private Const SUBMENU_SCHEDULER As Integer = 400
    ''' <summary>
    ''' 電話帳
    ''' </summary>
    Private Const SUBMENU_TELEPHONE_BOOK As Integer = 500
    ''' <summary>
    ''' 追加作業一覧
    ''' </summary>
    Private Const SUBMENU_ADD_LIST As Integer = 1100

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' フッター制御
    ''' </summary>
    ''' <param name="commonMaster">マスターページ</param>
    ''' <param name="category">カテゴリ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Public Overrides Function DeclareCommonMasterFooter(commonMaster As CommonMasterPage, ByRef category As FooterMenuCategory) As Integer()
        '自ページの所属メニューを宣言
        category = FooterMenuCategory.MainMenu

        '（表示・非表示に関わらず）使用するサブメニューボタンを宣言
        Return New Integer() {}
    End Function

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' フッターボタンの制御
    ''' </summary>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub InitFooterEvent()

        'ヘッダ表示設定
        '戻るボタン非活性化
        CType(Me.Master.Master, CommonMasterPage).IsRewindButtonEnabled = False

        'フッタ表示設定
        'サブメニューボタンを設定（イベントハンドラ割り当て）
        '説明ツール(STEP1では非活性)
        Dim explanationToolButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_EXPLANATION_TOOL)
        explanationToolButton.Enabled = False
        'R/O作成
        Dim roMakeButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_RO_MAKE)
        AddHandler roMakeButton.Click, AddressOf roMakeButton_Click
        ' 2012/02/23 KN 上田【SERVICE_1】START
        roMakeButton.OnClientClick = "return FooterButtonControl();"
        ' 2012/02/23 KN 上田【SERVICE_1】END
        '追加作業一覧
        Dim addListButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_ADD_LIST)
        AddHandler addListButton.Click, AddressOf addListButton_Click
        ' 2012/02/23 KN 上田【SERVICE_1】START
        addListButton.OnClientClick = "return FooterButtonControl();"
        ' 2012/02/23 KN 上田【SERVICE_1】END
        'スケジューラ
        Dim schedulerButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_SCHEDULER)
        schedulerButton.OnClientClick = "return schedule.appExecute.executeCaleNew();"
        '電話帳
        Dim telephoneBookButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(SUBMENU_TELEPHONE_BOOK)
        telephoneBookButton.OnClientClick = "return schedule.appExecute.executeCont();"

        'メニューボタンを設定（イベントハンドラ割り当て）
        'メインメニュー再表示
        Dim mainMenuButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.MainMenu)
        AddHandler mainMenuButton.Click, AddressOf mainMenuButton_Click
        ' 2012/02/23 KN 上田【SERVICE_1】START
        mainMenuButton.OnClientClick = "return FooterButtonControl();"
        ' 2012/02/23 KN 上田【SERVICE_1】END
        '顧客情報画面
        Dim customerButton As CommonMasterFooterButton = CType(Me.Master.Master, CommonMasterPage).GetFooterButton(FooterMenuCategory.Customer)
        AddHandler customerButton.Click, AddressOf customerButton_Click
        ' 2012/02/23 KN 上田【SERVICE_1】START
        customerButton.OnClientClick = "return FooterButtonControl();"
        ' 2012/02/23 KN 上田【SERVICE_1】END

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' メインメニューへ遷移する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub mainMenuButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ出力
        Dim logStart As New StringBuilder
        With logStart
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" Start")
        End With
        Logger.Info(logStart.ToString)

        Try

            Dim logMainMenu As StringBuilder = New StringBuilder(String.Empty)
            With logMainMenu
                .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
                .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0}", MAINMENUID))
            End With
            Logger.Info(logMainMenu.ToString())

            ' 再表示
            Me.RedirectNextScreen(MAINMENUID)

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'タイムアウトエラーの場合は、メッセージを表示する
            ShowMessageBox(MsgID.id901)
        End Try

        '終了ログ出力
        Dim logEnd As New StringBuilder
        With logEnd
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" End")
        End With
        Logger.Info(logEnd.ToString)

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 顧客情報画面へ遷移する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>
    ''' チップが未選択の場合
    ''' 　⇒ 新規顧客登録画面に遷移します。
    ''' チップが選択状態で且つ、受付エリアのチップを選択した場合
    ''' 　⇒ 顧客情報画面に遷移します。
    ''' </remarks>
    '''-----------------------------------------------------------------------
    Private Sub customerButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ出力
        Dim logStart As New StringBuilder
        With logStart
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" Start")
        End With
        Logger.Info(logStart.ToString)

        Try
            ' 選択チップチェック
            Dim visitSeq As Long = SetNullToLong(Me.DetailsVisitNo.Value)
            If visitSeq = 0 Then
                ' チップ未選択

                ' 新規顧客登録画面に遷移
                Me.RedirectCustomerNew()

            Else
                ' 顧客情報画面へ遷移
                Me.RedirectCustomer(visitSeq)

            End If

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'タイムアウトエラーの場合は、メッセージを表示する
            ShowMessageBox(MsgID.id901)
            ' 2012/02/24 KN 森下【SERVICE_1】START
        Finally
            ' チップタイマー用に現在時刻取得
            Dim staffInfo As StaffContext = StaffContext.Current
            Me.mNow = DateTimeFunc.Now(staffInfo.DlrCD)
            ' 2012/02/24 KN 森下【SERVICE_1】END
        End Try

        '終了ログ出力
        Dim logEnd As New StringBuilder
        With logEnd
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" End")
        End With
        Logger.Info(logEnd.ToString)

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' R/O作成画面へ遷移する。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks>
    ''' チップが未選択の場合
    ''' 　⇒ R/O一覧画面に遷移します。
    ''' チップが選択状態で且つ、受付エリアのチップを選択した場合
    ''' 　⇒ R/O作成画面に遷移します。
    ''' チップが選択状態で且つ、受付エリア以外のチップを選択した場合
    ''' 　⇒ R/O参照画面に遷移します。
    ''' </remarks>
    '''-----------------------------------------------------------------------
    Private Sub roMakeButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ出力
        Dim logStart As New StringBuilder
        With logStart
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" Start")
        End With
        Logger.Info(logStart.ToString)

        Try
            ' 選択エリアチェック
            Dim detailArea As Long = SetNullToLong(Me.DetailsArea.Value)
            ' 選択チップチェック
            Dim visitSeq As Long = SetNullToLong(Me.DetailsVisitNo.Value)
            If visitSeq = 0 Then
                ' チップ未選択

                ' R/O一覧画面へ遷移
                RedirectOrderList()
            ElseIf detailArea = CType(ChipArea.Reception, Long) Then
                ' チップ選択状態かつ受付エリアのチップを選択

                ' R/O作成画面へ遷移
                Me.RedirectOrderNew(visitSeq)
            Else
                ' チップ選択状態かつ受付エリア以外のチップを選択

                ' 選択チップのオーダーNo
                Dim orderNo As String = SetNullToString(Me.DetailsOrderNo.Value)

                ' R/O参照画面へ遷移
                Me.RedirectOrderDisp(orderNo)
            End If

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'タイムアウトエラーの場合は、メッセージを表示する
            ShowMessageBox(MsgID.id901)
            ' 2012/02/24 KN 森下【SERVICE_1】START
        Finally
            ' チップタイマー用に現在時刻取得
            Dim staffInfo As StaffContext = StaffContext.Current
            Me.mNow = DateTimeFunc.Now(staffInfo.DlrCD)
            ' 2012/02/24 KN 森下【SERVICE_1】END
        End Try

        '終了ログ出力
        Dim logEnd As New StringBuilder
        With logEnd
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" End")
        End With
        Logger.Info(logEnd.ToString)

    End Sub

    '''-----------------------------------------------------------------------
    ''' <summary>
    ''' 追加作業一覧画面に遷移
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    '''-----------------------------------------------------------------------
    Private Sub addListButton_Click(sender As Object, e As CommonMasterFooterButtonClickEventArgs)

        '開始ログ出力
        Dim logStart As New StringBuilder
        With logStart
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" Start")
        End With
        Logger.Info(logStart.ToString)

        Try

            Dim logAddList As StringBuilder = New StringBuilder(String.Empty)
            With logAddList
                .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
                .Append(String.Format(CultureInfo.CurrentCulture, " Redirect {0}", C_APPLICATIONID_ADD_LIST))
            End With
            Logger.Info(logAddList.ToString())

            ' 追加作業一覧画面に遷移
            Me.RedirectNextScreen(C_APPLICATIONID_ADD_LIST)

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'タイムアウトエラーの場合は、メッセージを表示する
            ShowMessageBox(MsgID.id901)
        End Try

        '終了ログ出力
        Dim logEnd As New StringBuilder
        With logEnd
            .Append(System.Reflection.MethodBase.GetCurrentMethod.Name)
            .Append(" End")
        End With
        Logger.Info(logEnd.ToString)

    End Sub

#End Region

End Class
