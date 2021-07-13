Imports Toyota.eCRB.TrialRide.TrialRidePreparation.DataAccess
Imports Toyota.eCRB.TrialRide.TrialRidePreparation.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization


''' <summary>
''' 試乗入力(パネル内表示) プレゼンテーション層クラス
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3110101
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppId As String = "SC3110101"

    ''' <summary>
    ''' モデル写真のシルエットアイコン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SilhouetteCar As String = "../Styles/Images/SC3110101/notCarImage.png"

    ''' <summary>
    ''' 試乗車が使用中か判断する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UsedCar As String = "1"

#Region "メッセージID"

    ''' <summary>
    ''' 試乗車データが存在しない場合のメッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotExistCar As Integer = 21

#End Region

#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' メッセージID
    ''' </summary>
    ''' <remarks></remarks>
    Protected MessageId As Integer = 0

#End Region

#Region "初期表示イベント"

    ''' <summary>
    ''' 初期表示イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load_Start")

        'ログイン情報チェック
        Logger.Info("Page_Load_001 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        Logger.Info("Page_Load_001 " & "Call_End   StaffContext.Current")

        '権限を設定
        Me.authority.Value = loginStaff.OpeCD

        ' PostBack時、初期表示処理は行わない。
        If Me.IsPostBack Then

            Me.LoadSpinPanel.Visible = False
            Logger.Info("Page_Load_End PostBack")
            Return
        End If

        Me.LoadSpinPanel.Visible = True
        Me.NotTestDriveCarPanel.Visible = False
        Me.TestDriveCarPanel.Visible = False


        Logger.Info("Page_Load_End")
    End Sub
#End Region

#Region "初期化処理"

    ''' <summary>
    ''' 初期化処理
    ''' </summary>
    ''' <param name="initDataTable">DBから取得した情報</param>
    ''' <remarks>件数をチェックし、表示する内容を決める</remarks>
    Private Sub SetInit(ByVal initDataTable As SC3110101DataSet.SC3110101TestDriveCarInfoDataTable)

        Dim setInitStartLog As New StringBuilder
        With setInitStartLog
            .Append("SetInit_Start")
            .Append("param[")
            .Append(initDataTable)
            .Append("]")
        End With
        Logger.Info(setInitStartLog.ToString)

        '結果の件数チェック
        If initDataTable Is Nothing OrElse initDataTable.Count <= 0 Then

            '0件の場合のパネルを表示
            Logger.Info("SetInit_001 Count = 0")
            Me.NotCarStatus.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(AppId, NotExistCar))
            Me.NotTestDriveCarPanel.Visible = True
            Me.TestDriveCarPanel.Visible = False
        Else

            Logger.Info("SetInit_002 Count > 0")

            'リピーターに情報をセット
            Me.TestCarList.DataSource = initDataTable
            Me.TestCarList.DataBind()

            '情報を格納する
            SetCarStatus()

            '件数パネルの表示
            Me.NotTestDriveCarPanel.Visible = False
            Me.TestDriveCarPanel.Visible = True

        End If

        Logger.Info("SetInit_End initDataTable")
    End Sub

#End Region

#Region "試乗車情報を格納する"

    ''' <summary>
    ''' 画面に表示する試乗車情報を格納する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetCarStatus()

        Logger.Info("SetCarStatus_Start initDataTable Is Nothing")

        For Each carData As RepeaterItem In Me.TestCarList.Items

            'モデル画像が空欄の場合、シルエットアイコンにする
            If String.IsNullOrEmpty(CType(carData.FindControl("modelPicture"), HiddenField).Value) Then

                Logger.Info("SetCarStatus_001 carPicture Is Nothing")
                CType(carData.FindControl("carPicture"), Image).ImageUrl = SilhouetteCar
            Else
                CType(carData.FindControl("carPicture"), Image).ImageUrl = CType(carData.FindControl("modelPicture"), HiddenField).Value
            End If

            Logger.Info("SetCarStatus_002 set carPicture Status")

            'モデルロゴが空欄の場合、シリーズ名を入れる処理にする
            If String.IsNullOrEmpty(CType(carData.FindControl("modelLogo"), HiddenField).Value) Then

                Logger.Info("SetCarStatus_003 carNamePicture Is Nothing")
                CType(carData.FindControl("carNamePicture"), Image).Visible = False
                CType(carData.FindControl("NotLogo"), Label).Visible = True
                CType(carData.FindControl("NotLogo"), Label).Text = (CType(carData.FindControl("testDriveCarName"), Label).Text)
            Else

                Logger.Info("SetCarStatus_004 set carNamePicture Status")
                CType(carData.FindControl("carNamePicture"), Image).Visible = True
                CType(carData.FindControl("NotLogo"), Label).Visible = False
                CType(carData.FindControl("carNamePicture"), Image).ImageUrl = CType(carData.FindControl("modelLogo"), HiddenField).Value
                CType(carData.FindControl("carNamePicture"), Image).AlternateText = (CType(carData.FindControl("testDriveCarName"), Label).Text)
            End If
        Next

        Logger.Info("SetCarStatus_End")

    End Sub

#End Region

#Region "登録ボタン押下イベント"

    ''' <summary>
    ''' 登録ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub RegisterButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RegisterButton.Click

        Logger.Info("RegisterButton_Click_Start")

        'ログイン情報チェック
        Logger.Info("RegisterButton_Click_001 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        Logger.Info("RegisterButton_Click_001 " & "Call_End   StaffContext.Current")

        Using insertCarStatus As New SC3110101DataSet.SC3110101InsertTestDriveCarStatusDataTable

            '取ってきたときのやつと更新のやつを比較する
            Dim i As Integer = 0
            For Each item As RepeaterItem In Me.TestCarList.Items

                Dim target1 As String = CType(item.FindControl("testDriveCarStatus"), HiddenField).Value
                Dim target2 As String = Request.Form(String.Format(CultureInfo.CurrentCulture(), "BeforeStatus{0}", i))

                '一致していなかったら更新がされている
                If String.Equals(target1, target2) = False Then

                    Logger.Info("RegisterButton_Click_002 " & "NotEqual")

                    'レコードを追加する
                    Dim insertRows As SC3110101DataSet.SC3110101InsertTestDriveCarStatusRow = insertCarStatus.NewSC3110101InsertTestDriveCarStatusRow
                    insertRows.DLRCD = loginStaff.DlrCD
                    insertRows.TESTDRIVECARID = CType(item.FindControl("testDriveCarId"), HiddenField).Value
                    insertRows.TESTDRIVECARSTATUS = target1

                    '試乗車の変更後のステータスを確認する
                    If String.Equals(target1, UsedCar) = True Then

                        insertRows.ACCOUNT = loginStaff.Account
                    Else

                        '未使用にした場合はnothing
                        insertRows.ACCOUNT = Nothing
                    End If

                    insertRows.UPDATEDATE = CType(item.FindControl("updateDate"), HiddenField).Value
                    insertCarStatus.Rows.Add(insertRows)
                End If

                Logger.Info("RegisterButton_Click_003 " & "Next")
                i += 1

            Next

            If insertCarStatus Is Nothing OrElse insertCarStatus.Count = 0 Then

                '更新なし
                Logger.Info("RegisterButton_Click_End insertCarStatus Is Nothing")

                '試乗車空き状況確認画面再表示
                GetInitialDisplay()

                Return
            Else

                Logger.Info("RegisterButton_Click_005 insertCarStatus Is Nothing")

                '試乗車ステータスの更新
                Dim updateStatus As New SC3110101BusinessLogic
                Me.MessageId = updateStatus.UpdateTestDriveCarStatus(insertCarStatus, loginStaff.Account)

                '結果を返却
                If Me.MessageId <> 0 Then

                    Logger.Info("RegisterButton_Click_006  Me.MessageId <> 0 ")

                    '対応のエラーメッセージを表示し、画面を再描画
                    Me.ShowMessageBox(Me.MessageId, WebWordUtility.GetWord(AppId, Me.MessageId))
                End If

                Logger.Info("RegisterButton_Click_007 reLoad")

                '試乗車空き状況確認画面再表示
                GetInitialDisplay()

            End If
        End Using
        Logger.Info("RegisterButton_Click_End")
    End Sub
#End Region

#Region "スピンアイコン表示時の初期化処理"

    ''' <summary>
    ''' スピンアイコン表示時の初期化処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub LoadSpinButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LoadSpinButton.Click

        Logger.Info("LoadSpinButton_Click_Start")

        'ログイン情報チェック
        Logger.Info("LoadSpinButton_Click_001 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        Logger.Info("LoadSpinButton_Click_001 " & "Call_End   StaffContext.Current")

        '権限を設定
        Me.authority.Value = loginStaff.OpeCD

        '表示に使う情報を設定するDataTable
        GetInitialDisplay()

        Logger.Info("LoadSpinButton_Click_End")
    End Sub
#End Region

#Region "初期化表示"

    ''' <summary>
    ''' 初期化表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetInitialDisplay()

        Logger.Info("GetInitialDisplay_Start")

        'ログイン情報チェック
        Logger.Info("GetInitialDisplay_001 " & "Call_Start StaffContext.Current")
        Dim loginStaff As StaffContext = StaffContext.Current
        Logger.Info("GetInitialDisplay_001 " & "Call_End   StaffContext.Current")

        '初期表示データ取得
        Dim getInit As New SC3110101BusinessLogic

        '表示に使う情報を設定するDataTable
        Using dataSet As SC3110101DataSet.SC3110101TestDriveCarInfoDataTable = getInit.GetInitialDisplay(loginStaff.DlrCD, loginStaff.BrnCD)

            SetInit(dataSet)
        End Using
        Logger.Info("GetInitialDisplay_End")
    End Sub
#End Region

End Class
