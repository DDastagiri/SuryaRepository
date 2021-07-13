'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100104.aspx.vb
'──────────────────────────────────
'機能： お客様チップ作成
'補足： 
'作成： 2013/09/05 TMEJ m.asano
'更新： 2015/11/10 TMEJ t.komure  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 $01
'更新： 2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新：
'──────────────────────────────────

Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Visit.ReceptionistMain.BizLogic
Imports Toyota.eCRB.Visit.Api.BizLogic
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSet
Imports System.Web.Configuration
Imports System.Web.Services
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitReceptionDataSet

''' <summary>
''' SC3100104
''' お客様チップ作成 プレゼンテーション層クラス
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3100104
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' セッションキー（敬称の前後位置）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyNameTitlePos As String = "nameTitlePos"

    ''' <summary>
    ''' セッションキー（苦情情報日数(N日)）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionKeyComplaintDateCount As String = "complaintDateCount"

    ''' <summary>
    ''' システム環境設定パラメータ（敬称前後）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitlePotision As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' 苦情情報日数(N日)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ComplaintDisplayDate As String = "COMPLAINT_DISPLAYDATE"

    ''' <summary>
    ''' ソート条件：電話番号、担当SC名の昇順
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SortTypeSLR As String = "2"

    ''' <summary>
    ''' 敬称位置：前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NameTitlePositionFront As String = "1"

    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppId As String = "SC3100104"

    ''' <summary>
    ''' オラクルエラーコード:タイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrorCodeOraDBTimeout As Integer = 900

    ''' <summary>
    ''' システム環境設定値：検索最大数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MaxResultParamName As String = "SLR_MAX_CUSTOMER_SEARCH_RESSULT"

    ' $01 START (トライ店システム評価)SMBチップ検索の絞り込み方法変更
    ''' <summary>
    ''' 検索タイプ：車両登録No
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SearchTypeRegNumber As String = "1"
    ' $01 END (トライ店システム評価)SMBチップ検索の絞り込み方法変更

    '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    ''' <summary>
    ''' Lマーク表示フラグ（2：表示）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LIconFlagOn As String = "2"
    '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

#Region "文言"

    Private Const WordIdSearchTypeCustomerName As String = "4"
    Private Const WordIdSearchTypeTelephone As String = "5"
    Private Const WordIdSearchTypeVehicleNo As String = "6"
    Private Const WordIdSearchTypeVehicleVIN As String = "7"
    Private Const WordIdColumNameCustomerName As String = "9"
    Private Const WordIdColumNameTelephone As String = "10"
    Private Const WordIdColumNameVehicle As String = "11"
    Private Const WordIdColumNameSalesStaff As String = "12"
    Private Const WordSearchBoxPlaceHolder As String = "8"
    Private Const WordIdNotFoundLiteral As String = "13"
    Private Const WordIdOverFlowLiteral1 As String = "14"
    Private Const WordIdOverFlowLiteral2 As String = "15"
    ' $01 start 国民ID検索
    Private Const WordSearchTypeSocialNumber As String = "16"
    ' $01 end 国民ID検索
    '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    Private Const WordIdLmark As String = "10001"
    '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
#End Region

#End Region

#Region "イベント処理"

#Region "ページロード時の処理"

    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load_Start")

        ' PostBack時、初期表示処理は行わない。
        If Me.IsPostBack Then

            ' PostBackでロードパネルを非表示
            Me.LoadSpinPanel.Visible = False
            Logger.Info("Page_Load_End PostBack")
            Return

        End If
        ' ロードパネルを表示
        Me.LoadSpinPanel.Visible = True
        Logger.Info("Page_Load_End")

    End Sub

#End Region

#Region "非同期処理"

#Region "検索ボタン押下時"

    ''' <summary>
    ''' 検索ボタン押下時処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CustomerSearchButton_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles CustomerSearchButton.Click
        Logger.Info("CostomerSerchButtom_Click Start")

        ' 画面入力情報の取得
        Dim searchType As String = SerchType.Value
        Dim searchText As String = Server.HtmlDecode(Me.InputSearchText.Value)

        ' 検索条件テキストの入力チェック
        ' 検索条件のチェック
        If String.IsNullOrEmpty(Trim(searchText)) OrElse _
            SelectedSearchTypeFlag.Value = "0" Then
            Return
        End If

        ' 顧客一覧の取得
        Dim loginStaff As StaffContext = StaffContext.Current     ' ログインスタッフ
        Dim utilityBusinessLogic As New VisitReceptionBusinessLogic
        Dim serchTextList As New List(Of String)                  ' 検索文字列(GKとの処理共通化によりリストで渡す)

        ' $01 START (トライ店システム評価)SMBチップ検索の絞り込み方法変更
        ' 検索タイプが車両登録Noの場合、車両登録番号検索ワード変換を実施する
        If SearchTypeRegNumber.Equals(searchType) Then
            searchText = utilityBusinessLogic.ConvertVclRegNumWord(searchText)
        End If
        ' $02 END (トライ店システム評価)SMBチップ検索の絞り込み方法変更

        serchTextList.Add(searchText)
        Dim sysEnvSet As New SystemEnvSetting
        Dim maxResult As String = sysEnvSet.GetSystemEnvSetting(MaxResultParamName).PARAMVALUE '検索結果表示最大値

        Using dataSet As VisitReceptionCustomerListDataTable = _
            utilityBusinessLogic.GetCustomerList(loginStaff.DlrCD, loginStaff.BrnCD, _
                                          searchType, serchTextList, SortTypeSLR)
            ' 結果の件数チェック
            If dataSet Is Nothing OrElse dataSet.Count <= 0 Then

                ' 0件時のパネルを表示し終了
                CustomerNotFound.Visible = True
                CustomerOverFlow.Visible = False
                CustomerList.Visible = False
                Me.CustomerNotFoundLiteral.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdNotFoundLiteral))

                LoadingAnimation.Visible = False
                Me.CustomerSerchEnd.Value = "1"
                Return
                ' 検索結果が多い
            ElseIf dataSet.Count > CInt(maxResult) Then
                ' メッセージを表示し終了
                CustomerOverFlow.Visible = True
                CustomerNotFound.Visible = False
                CustomerList.Visible = False

                Me.OverFlowLiteral1.Text = _
                    Server.HtmlEncode(String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(AppId, WordIdOverFlowLiteral1), maxResult))
                Me.OverFlowLiteral2.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdOverFlowLiteral2))

                LoadingAnimation.Visible = False
                Me.CustomerSerchEnd.Value = "1"
                Return
            End If

            ' 敬称前後
            Dim nameTitlePosition As String = _
                GetParameter(SessionKeyNameTitlePos, NameTitlePotision)
            'リピーターに情報をセット
            Me.CustomerRepeater.DataSource = dataSet
            Me.CustomerRepeater.DataBind()

            ' 件数分表示する
            For i = 0 To CustomerRepeater.Items.Count - 1

                Dim customer As Control = CustomerRepeater.Items(i)
                Dim targetCustomerRow As VisitReceptionCustomerListRow = dataSet.Rows(i)

                '情報を表示する
                Me.ShowCustomerList(customer, targetCustomerRow, nameTitlePosition)

            Next

            ' メッセージを表示し終了
            CustomerOverFlow.Visible = False
            CustomerNotFound.Visible = False
            CustomerList.Visible = True

            ' 画面更新(非同期)
            UpdateAreaCustomerList.Update()

            Logger.Debug(Request.RawUrl.ToString)
        End Using

        utilityBusinessLogic = Nothing

        Logger.Info("CostomerSerchButtom_Click End")
    End Sub

#End Region

#Region "チップ作成ボタン押下時"

    ''' <summary>
    ''' チップ作成ボタン押下時処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CreateChipButton_Click() Handles RegisterButton.Click

        '人数押下判定
        If SelectedPersonNumberFlag.Value = "0" Then
            Return
        End If

        Dim messgae As Decimal = 0

        ' ログイン情報取得
        Dim loginStaff As StaffContext = StaffContext.Current

        ' チップ作成対象が既存顧客の場合
        If SelectedCustomerFlag.Value = "1" Then
            Logger.Info("New customer Chip Create")
            ' 苦情情報を取得する。
            ' 苦情情報日数を取得
            Dim complaintDateCount As String = _
                GetParameter(SessionKeyComplaintDateCount, ComplaintDisplayDate)


            Dim utility As New VisitUtilityBusinessLogic

            Dim isClaimInfo As Boolean = _
                utility.HasClaimInfo(SelectedCustKubun.Value, _
                                            SelectedCustID.Value, _
                                            DateTimeFunc.Now(loginStaff.DlrCD), _
                                            complaintDateCount)

            ' 顧客情報作成
            Using Adapter As New VisitReceptionVisitSalesDataTable
                Dim insertRow = Adapter.NewVisitReceptionVisitSalesRow
                Dim businessLogic As New SC3100104BusinessLogic
                insertRow.VISITPERSONNUMBER = SelectedPersonNumber.Value
                insertRow.DEALERCODE = loginStaff.DlrCD
                insertRow.STORECODE = loginStaff.BrnCD
                insertRow.FUNCTIONID = AppId
                insertRow.CREATEACCOUNT = loginStaff.Account
                insertRow.CUSTNAME = SelectedCustName.Value
                insertRow.CUSTNAMETITLE = SelectedCustNameTitle.Value
                insertRow.CUSTOMERID = SelectedCustID.Value
                insertRow.CUSTOMERSEGMENT = SelectedCustKubun.Value
                insertRow.STAFFCODE = SelectedCustStaffCode.Value
                insertRow.VEHICLEREGNO = SelectedRegNo.Value
                insertRow.VIN = SelectedVIN.Value

                messgae = businessLogic.CreateCustomerChip(insertRow, isClaimInfo)

                '結果を返却
                If messgae <> 0 Then

                    Logger.Info("CreateChipButton_Click  Me.MessageId <> 0 ")

                    '対応のエラーメッセージを表示し、画面を再描画
                    Me.ShowMessageBox(messgae, WebWordUtility.GetWord(AppId, ErrorCodeOraDBTimeout))
                    Return
                Else
                    ' 正常終了の場合はPush処理
                    businessLogic.PushExecution(insertRow, isClaimInfo)
                End If

                Me.CreateChipEndFlg.Value = "1"

            End Using
        Else
            Logger.Info("Chip Create Start")
            '新規顧客
            Using Adapter As New VisitReceptionVisitSalesDataTable
                Dim insertRow = Adapter.NewVisitReceptionVisitSalesRow
                Dim businessLogic As New SC3100104BusinessLogic
                insertRow.VISITPERSONNUMBER = SelectedPersonNumber.Value
                insertRow.DEALERCODE = loginStaff.DlrCD
                insertRow.STORECODE = loginStaff.BrnCD
                insertRow.FUNCTIONID = AppId
                insertRow.CREATEACCOUNT = loginStaff.Account

                messgae = businessLogic.CreateCustomerChip(insertRow, False)

                '結果を返却
                If messgae <> 0 Then

                    Logger.Info("CreateChipButton_Click  Me.MessageId <> 0 ")

                    '対応のエラーメッセージを表示し、画面を再描画
                    Me.ShowMessageBox(messgae, WebWordUtility.GetWord(AppId, ErrorCodeOraDBTimeout))
                    Return
                Else
                    ' 正常終了の場合はPush処理
                    businessLogic.PushExecution(insertRow, False)
                End If
                Me.CreateChipEndFlg.Value = "1"
            End Using

        End If

    End Sub

#End Region

#End Region

#End Region

#Region "非公開メソッド"

#Region "初期表示処理"

    ''' <summary>
    ''' 初期表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PageInit()

        ' 文言の設定
        Me.SearchTypeCustomerName.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdSearchTypeCustomerName))
        Me.SearchTypeTelephone.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdSearchTypeTelephone))
        Me.SearchTypeVehicleNo.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdSearchTypeVehicleNo))
        Me.SearchTypeVehicleVin.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdSearchTypeVehicleVIN))
        Me.ColumNameCustomerName.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdColumNameCustomerName))
        Me.ColumNameTelephone.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdColumNameTelephone))
        Me.ColumNameVehicle.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdColumNameVehicle))
        Me.ColumNameSalesStaff.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordIdColumNameSalesStaff))
        Me.SearchTextString.Attributes.Add("placeholder", WebWordUtility.GetWord(AppId, WordSearchBoxPlaceHolder))
        ' $01 start 国民ID検索
        Me.SearchTypeSocialNumber.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordSearchTypeSocialNumber))
        Me.ColumNameSocialNumber.Text = Server.HtmlEncode(WebWordUtility.GetWord(AppId, WordSearchTypeSocialNumber))
        ' $01 end 国民ID検索

    End Sub

#End Region

#Region "パラメータ取得"

    ''' <summary>
    ''' パラメータ取得
    ''' </summary>
    ''' <param name="SessionName">セッション名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetParameter(ByVal SessionName As String, ByVal ParameterName As String) As String
        Dim returnValue As String = String.Empty

        If Me.ContainsKey(ScreenPos.Current, SessionName) Then
            ' セッションに設定されていればその値を使用する。
            ' 親画面の受付メインにて設定されている(はず)。
            returnValue = Me.GetValue(ScreenPos.Current, SessionName, False)
        Else
            Dim sysEnvSet As New SystemEnvSetting
            Dim sysEnvSetTitlePosRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
            sysEnvSetTitlePosRow = sysEnvSet.GetSystemEnvSetting(ParameterName)
            returnValue = sysEnvSetTitlePosRow.PARAMVALUE
            sysEnvSet = Nothing
            sysEnvSetTitlePosRow = Nothing
        End If

        Return returnValue
    End Function


#End Region

#Region "顧客リストの作成"
    ''' <summary>
    ''' 初期化処理
    ''' </summary>
    ''' <param name="customerControl">HTMLコントロール</param>
    ''' <param name="nameTitlePosition">敬称前後</param>
    ''' <param name="targetCustomerRow">顧客情報</param>
    ''' <remarks>表示</remarks>
    Private Sub ShowCustomerList(ByVal customerControl As Control, _
                                 ByVal targetCustomerRow As VisitReceptionCustomerListRow, _
                                 ByVal nameTitlePosition As String)

        Logger.Info("ShowCustomerList_Start")

        Dim name As String   'お客様名
        Dim telno As String = targetCustomerRow.TELNO.Trim         '電話番号
        Dim mobile As String = targetCustomerRow.MOBILE.Trim       '携帯番号
        Dim vclRegNo As String = targetCustomerRow.VCLREGNO.Trim   '登録番号
        Dim seriesName As String = targetCustomerRow.SERIESNM.Trim 'シリーズ名
        Dim vin As String = targetCustomerRow.VIN.Trim             'VIN
        Dim staffName As String = targetCustomerRow.STUFFNAME.Trim '担当スタッフ名
        Dim custKubun As String = targetCustomerRow.CUSTKBN.Trim   '顧客区分
        Dim custCode As String = targetCustomerRow.CUSTCD.Trim     '顧客ID
        Dim socialNum As String = targetCustomerRow.SOSCIALNUM.Trim '国民ID
        '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        Dim impVclFlg As String = targetCustomerRow.IMP_VCL_FLG.Trim 'Lマークフラグ
        '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

        'データ加工
        '名前作成
        If (targetCustomerRow.NAMETITLE.Trim.Length > 0) Then
            If nameTitlePosition.Equals(NameTitlePositionFront) Then
                name = targetCustomerRow.NAMETITLE.Trim + " " + targetCustomerRow.NAME.Trim
            Else
                name = targetCustomerRow.NAME.Trim + " " + targetCustomerRow.NAMETITLE.Trim
            End If
        Else
            name = targetCustomerRow.NAME.Trim
            If String.IsNullOrEmpty(name) Then
                name = "-"
            End If

        End If

        If String.IsNullOrEmpty(telno) Then
            telno = "-"
        End If

        If String.IsNullOrEmpty(mobile) Then
            mobile = "-"
        End If

        If String.IsNullOrEmpty(vclRegNo) Then
            vclRegNo = "-"
        End If

        If String.IsNullOrEmpty(seriesName) Then
            seriesName = "-"
        End If

        If String.IsNullOrEmpty(vin) Then
            vin = "-"
        End If

        If String.IsNullOrEmpty(staffName) Then
            staffName = "-"
        End If

        ' $01 start 国民ID
        If String.IsNullOrEmpty(socialNum) Then
            socialNum = "-"
        End If
        ' $01 end 国民ID

        CType(customerControl.FindControl("CustomerNameLiteral"), Literal).Text = Server.HtmlEncode(name)
        CType(customerControl.FindControl("TelePhoneNumberLiteral"), Literal).Text = Server.HtmlEncode(telno)
        CType(customerControl.FindControl("MobilePhoneNumberLiteral"), Literal).Text = Server.HtmlEncode(mobile)
        CType(customerControl.FindControl("RegNoLiteral"), Literal).Text = Server.HtmlEncode(vclRegNo)
        CType(customerControl.FindControl("VehicleNameLiteral"), Literal).Text = Server.HtmlEncode(seriesName)
        CType(customerControl.FindControl("VINLiteral"), Literal).Text = Server.HtmlEncode(vin)
        CType(customerControl.FindControl("StaffNameLiteral"), Literal).Text = Server.HtmlEncode(staffName)
        ' $01 start 国民ID
        CType(customerControl.FindControl("SocialNumberLiteral"), Literal).Text = Server.HtmlEncode(socialNum)
        ' $01 end 国民ID
        '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        'Lマーク文言
        Dim Lword As String = WebWordUtility.GetWord(AppId, WordIdLmark)
        CType(customerControl.FindControl("Lmark"), HtmlContainerControl).InnerText = Lword

        If LIconFlagOn.Equals(impVclFlg) Then
            'フラグが2のとき、Lマークを表示
            customerControl.FindControl("Lmark").Visible = True
        Else
            'フラグが2以外のとき、Lマークを非表示
            customerControl.FindControl("Lmark").Visible = False
        End If
        '2018/07/19 NSK a.kani TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        Logger.Info("ShowCustomerList End")
    End Sub
#End Region

#Region "画面更新処理"
    ''' <summary>
    ''' 画面更新処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub LoadSpinButton_Click(sender As Object, e As System.EventArgs) Handles LoadSpinButton.Click
        Me.PageInit()
    End Sub
#End Region

#End Region

End Class
