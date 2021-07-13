'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3090301.aspx.vb
'──────────────────────────────────
'機能： ゲートキーパーメイン
'補足： 
'作成： yyyy/MM/dd KN  x.xxxxxx
'更新： 2012/02/13 KN  y.nakamura STEP2開発 $01
'更新： 2012/05/23 KN  m.asano    性能改善　$02
'更新： 2013/03/13 TMEJ t.shimamura 来店歓迎オペレーション確立に向けたアプリ評価 $03
'更新： 2013/04/16 TMEJ m.asano   ウェルカムボード仕様変更対応 $04
'更新： 2013/10/15 TMEJ m.asano   次世代e-CRBセールス機能 新DB適応に向けた機能開発 $05
'更新： 2013/12/02 TMEJ t.shimamura   次世代e-CRBサービス 店舗展開に向けた標準作業確立 $06
'更新： 2014/01/07 TMEJ chin   TMEJ次世代サービス 工程管理機能開発 $06
'更新： 2015/02/18 TMEJ y.nakamura UAT課題#158 $07
'更新： 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 $08
'更新： 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 $09
'──────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.GateKeeper.GateKeeperMain.BizLogic
Imports Toyota.eCRB.GateKeeper.GateKeeperMain.DataAccess
Imports Toyota.eCRB.GateKeeper.GateKeeperMain.DataAccess.SC3090301DataSet
Imports Toyota.eCRB.GateKeeper.GateKeeperMain.DataAccess.SC3090301DataSetTableAdapters
'$09 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START 
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
'$09 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END

''' <summary>
''' SC3090301(ゲートキーパーメイン)
''' Webページのプレゼンテーション層
''' </summary>
''' <remarks></remarks>
Partial Class Pages_SC3090301
    Inherits BasePage

#Region "定数"

#Region "共通"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApplicationId As String = "SC3090301"

    ''' <summary>
    ''' 文字切り時の表示文字
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CharCutValue As String = "..."

    ''' <summary>
    ''' 空白文字
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultString As String = " "

    '$03 start ウェルカムボードへのPush
    ''' <summary>
    ''' 顧客区分：新規顧客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustomerClassNew As String = "0"
    '$03 end   ウェルカムボードへのPush

    '06 start  
    ''' <summary>
    ''' 次へボタンから画面を遷移した場合
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FromNextDataBottun As Integer = 1

    ''' <summary>
    ''' 前へボタンから画面を遷移した場合
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FromPreviewDataBottun As Integer = 2

    ''' <summary>
    ''' リフレッシュにより画面更新した場合
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FromDisplayReflesh As Integer = 3

    ''' <summary>
    ''' 新規登録画面から画面を遷移した場合
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FromNewRegistorDisplay As Integer = 4

    '$07 start UAT課題#158
    ''' <summary>
    ''' 予約フラグ：予約あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReservFlagOn As String = "1"

    ''' <summary>
    '''予約フラグ：予約なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReservFlagOff As String = "0"
    '$07 end UAT課題#158

    '$08 Start 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
    ''' <summary>
    ''' 予約一覧画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppointmentListPageId As String = "SC3090401"
    '$08 End 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
#End Region

#Region "DB関連"

    ''' <summary>
    ''' システム管理マスタ.パラメータ名:敬称表示位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KeisyoZengo As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' 敬称表示位置:前
    ''' </summary>
    ''' <remarks></remarks>
    Private Const HonorificTitleMae As String = "1"

    ''' <summary>
    ''' デフォルト値：DBの項目が未設定の場合に画面に表示する値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DefaultValue As String = "-"

    ' $06 start
    ''' <summary>
    ''' システム管理マスタ.パラメータ名:表示最大件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MaxDisplayCountParam As String = "GK_MAX_DISPLAY_NUMBER"

    ''' <summary>
    ''' システム管理マスタ.パラメータ名:読み込み件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NextOrPreviewDisplayCountParam As String = "GK_READ_NUMBER"
    ' $06 end

    '$09 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
    ''' <summary>
    ''' 販売店システム設定.設定名:セールスタブレット使用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UseFlgSalesTabletName As String = "USE_FLG_SALES_TABLET"

    ''' <summary>
    ''' 販売店システム設定.設定名:サービスタブレット使用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UseFlgServiceTabletName As String = "USE_FLG_SERVICE_TABLET"

    ''' <summary>
    ''' 販売店環境設定.パラメータ名:車両登録番号入力区分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VclRegNoInputTypeParam As String = "VCLREGNO_INPUT_TYPE"
    '$09 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END
#End Region

#Region "画面パラメータ"

    ''' <summary>
    ''' 画面表示タイプ:待機画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispTypeWait As String = "1"

    ''' <summary>
    ''' 画面表示タイプ:来店通知未送信データ存在時画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispTypeUnsend As String = "2"

    ''' <summary>
    ''' 画面表示タイプ:新規入力画面[車]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispTypeNewCar As String = "3"

    ''' <summary>
    ''' 画面表示タイプ:新規入力画面[歩き]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DispTypeNewWalk As String = "4"

    ''' <summary>
    ''' 来店手段：車
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitMeansCar As String = "1"

    ''' <summary>
    ''' 来店手段：歩き
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitMeansWalk As String = "2"

    ''' <summary>
    ''' 来店目的：セールス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitPurposeSales As String = "1"

    ''' <summary>
    ''' 来店目的：サービス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitPurposeService As String = "2"

    ''' <summary>
    ''' 来店目的：対象外
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VisitPurposeNotTarget As String = "3"

    ''' <summary>
    ''' ランプ表示最大数：偶数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LampMaxIndexEven As Integer = 16

    ''' <summary>
    ''' ランプ表示最大数：奇数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LampMaxIndexOdd As Integer = 17

    ''' <summary>
    ''' 文字切り桁数:車両登録番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordMaxLengthVclRegNo As Integer = 9

    ' $01 start step2開発
    ''' <summary>
    ''' 文字切り桁数:車両情報１
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordMaxLengthCarInfo1 As Integer = 11

    ''' <summary>
    ''' 文字切り桁数:車両情報２
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordMaxLengthCarInfo2 As Integer = 11

    ''' <summary>
    ''' 文字切り桁数:お客様名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordMaxLengthCustName As Integer = 13
    ' $01 end step2開発

    ''' <summary>
    ''' 文字切り桁数:送信ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordMaxLengthSubmit As Integer = 13

    ''' <summary>
    ''' 文字切り時付加文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AddDefaultValue As String = "..."

    '$07 Start UAT課題#158
    ''' <summary>
    ''' 予約マークフラグ：ON
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReservFlgExistAppointment As String = "1"
    '$07 End UAT課題#158

#End Region

#Region "文言ID"

    ''' <summary>
    ''' 文言ID：画面タイトル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdDispTitle As Integer = 1

    ''' <summary>
    ''' 文言ID：送信ボタン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdSubmit As Integer = 3

    ''' <summary>
    ''' 文言ID：新規お客様
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdNewCustomer As Integer = 2

    ''' <summary>
    ''' 文言ID：デフォルト値[氏名]：DBの項目が未設定の場合に画面に表示する値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdDefaultValueName As Integer = 6

    '$07 Start UAT課題#158
    ''' <summary>
    ''' 文言ID：予約マーク
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdAppointmentIcon As Integer = 18
    '$07 End UAT課題#158
#End Region

#Region "メッセージID"

    ''' <summary>
    ''' メッセージID:成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSuccess As Integer = 0

    ''' <summary>
    ''' メッセージID:エラー[送信済]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdErrorSend As Integer = 901

    ''' <summary>
    ''' メッセージID:エラー[削除済]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdErrorDelete As Integer = 904


    '$08 Start 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
    ''' <summary>
    ''' 文言ID：禁止文字入力チェックエラーメッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdInvalidCharacter As Integer = 906
    '$08 End 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加

#End Region

#End Region

#Region "イベント"

#Region "ページロード"

    ''' <summary>
    ''' ページロード時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Logger.Info("Page_Load_Start")

        ' PostBack時、初期表示処理は行わない。
        If IsPostBack = True Then

            ' Logger.Debug("Page_Load_001 IsPostBack")
            Return
        End If

        '各文言の設定
        SetWord()

        '待機画面を表示
        dispType.Value = DispTypeWait

        '未送信データ件数非表示
        lbl_unSendCount.Visible = False

        'コンテンツ部分の表示切替
        dispWait.Visible = True
        vclNoRead.Visible = False
        newCar.Visible = False
        newWalk.Visible = False

        Logger.Info("Page_Load_End")
    End Sub

#End Region

#Region "初回読み込み"

    ''' <summary>
    ''' 初回読み込みボタン押下時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub InitButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles initButton.Click

        Logger.Info("InitButtonClick_Start")

        ' 初期表示処理
        PageInit()

        Logger.Info("InitButtonClick_End")
    End Sub

#End Region

#Region "車ボタン押下"

    ''' <summary>
    ''' 車ボタン押下時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub CarButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles CarButton.Click

        Logger.Info("CarButtonClick_Start")

        'ボタン押下時の画面タイプ判定
        If String.Equals(dispType.Value, DispTypeNewCar) Then

            ' Logger.Debug("CarButtonClick_001 DispType Is NewCar ")

            '新規登録画面[車]の場合、初期表示処理を実行
            PageInit(1, FromNewRegistorDisplay)
            CurrentDisplayHeaderNumber.Value = "1"
        Else

            ' Logger.Debug("CarButtonClick_002 DispType Not NewCar ")

            '新規登録画面[車]の以外の場合、新規登録画面[車]を表示
            DispNew(DispTypeNewCar)

        End If

        Logger.Info("CarButtonClick_End")
    End Sub
#End Region

#Region "歩きボタン押下"

    ''' <summary>
    ''' 歩きボタン押下時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub PersonButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles PersonButton.Click

        Logger.Info("PersonButtonClick_Start")

        'ボタン押下時の画面タイプ判定
        If String.Equals(dispType.Value, DispTypeNewWalk) Then

            ' Logger.Debug("PersonButtonClick_001 DispType Is NewWalk ")

            '新規登録画面[歩き]の場合、初期表示処理を実行
            PageInit(1, FromNewRegistorDisplay)
            CurrentDisplayHeaderNumber.Value = "1"

        Else

            ' Logger.Debug("PersonButtonClick_002 DispType Not NewWalk ")

            '新規登録画面[歩き]の以外の場合、新規登録画面[歩き]を表示
            DispNew(DispTypeNewWalk)

        End If

        Logger.Info("PersonButtonClick_End")
    End Sub
#End Region

    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
#Region "予約一覧ボタン押下"
    ''' <summary>
    ''' 予約一覧ボタン押下時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub ReserveListButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReserveListButton.Click

        Logger.Info("ReserveListButton_Click_Start")

        Me.RedirectNextScreen(AppointmentListPageId)

        Logger.Info("ReserveListButton_Click_End")

    End Sub
#End Region
    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
#Region "送信ボタン押下"

    ''' <summary>
    ''' 送信ボタン押下時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub SubmitButtonClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles submitButton.Click

        Logger.Info("SubmitButtonClick_Start")

        'ログイン
        ' Logger.Debug("SubmitButtonClick_001 " & "Call_Start StaffContext.Current")
        Dim staffInfo As StaffContext = StaffContext.Current
        ' Logger.Debug("SubmitButtonClick_001 " & "Call_End   StaffContext.Current Ret[" & (staffInfo IsNot Nothing) & "]")
        Dim unsendRow As SC3090301DataSet.SC3090301VisitUnsentDataRow
        Using unsendTbl As New SC3090301DataSet.SC3090301VisitUnsentDataDataTable
            unsendRow = unsendTbl.NewSC3090301VisitUnsentDataRow
            unsendRow.CUSTKBN = CustomerClassNew
        End Using
        Dim sc3090301Biz As SC3090301BusinessLogic = New SC3090301BusinessLogic
        Dim msgID As Integer

        Select Case dispType.Value
            Case DispTypeNewCar

                '新規登録画面[車]の場合
                ' Logger.Debug("SubmitButtonClick_002 DispType is NewCar")

                '来店目的の判定
                If String.Equals(purposeType.Value, VisitPurposeSales) Then

                    'セールスの場合
                    ' Logger.Debug("SubmitButtonClick_003 VisitPurpose is Sales")

                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START

                    ''送信処理_新規
                    'msgID = sc3090301Biz.SendNewSales(staffInfo.DlrCD, staffInfo.BrnCD, _
                    '                         DateTimeFunc.FormatDateSD(1, visitDate.Value), _
                    '                         personNum.Value, VisitMeansCar, staffInfo.Account)

                    ' 車両登録番号のNULLチェック
                    If Not String.IsNullOrEmpty(Me.InputRegNumber.Value) Then

                        ' 車両登録番号の禁止文字チェック
                        If Not Validation.IsValidString(Me.InputRegNumber.Value) Then

                            ' 禁止文字入力チェックエラーの場合は、初期表示処理を実行しない
                            Me.ShowMessageBox(MessageIdInvalidCharacter)

                            Return
                        End If
                    End If


                    '送信処理_新規
                    msgID = sc3090301Biz.SendNewSales(staffInfo.DlrCD, staffInfo.BrnCD, _
                                             DateTimeFunc.FormatDateSD(1, visitDate.Value), _
                                             personNum.Value, VisitMeansCar, staffInfo.Account, Me.InputRegNumber.Value)

                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END


                ElseIf String.Equals(purposeType.Value, VisitPurposeService) Then

                    'サービスの場合
                    ' Logger.Debug("SubmitButtonClick_004 VisitPurpose is Service")

                    '2014/01/07 TMEJ chin   TMEJ次世代サービス 工程管理機能開発 $06 START
                    '送信処理_新規
                    'msgID = sc3090301Biz.SendNewService(staffInfo.DlrCD, staffInfo.BrnCD, _
                    '                         DateTimeFunc.FormatDateSD(1, visitDate.Value), _
                    '                         personNum.Value, VisitMeansCar, staffInfo.Account)
                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
                    'msgID = sc3090301Biz.SendNewService(staffInfo.DlrCD, staffInfo.BrnCD, _
                    '                         DateTimeFunc.FormatDateSD(1, visitDate.Value), _
                    '                         personNum.Value, VisitMeansCar, staffInfo.Account, staffInfo.UserName)

                    '2014/01/07 TMEJ chin   TMEJ次世代サービス 工程管理機能開発 $06 END

                    ' 車両登録番号のNULLチェック
                    If Not String.IsNullOrEmpty(Me.InputRegNumber.Value) Then

                        ' 車両登録番号の禁止文字チェック
                        If Not Validation.IsValidString(Me.InputRegNumber.Value) Then

                            ' 禁止文字入力チェックエラーの場合は、初期表示処理を実行しない
                            Me.ShowMessageBox(MessageIdInvalidCharacter)

                            Return
                        End If
                    End If

                    msgID = sc3090301Biz.SendNewService(staffInfo.DlrCD, staffInfo.BrnCD, _
                                             DateTimeFunc.FormatDateSD(1, visitDate.Value), _
                                             personNum.Value, VisitMeansCar, staffInfo.Account, staffInfo.UserName, Me.InputRegNumber.Value)
                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

                End If

            Case DispTypeNewWalk

                '新規登録画面[歩き]の場合
                ' Logger.Debug("SubmitButtonClick_005 DispType is NewWalk")

                '来店目的がセールスの場合
                If String.Equals(purposeType.Value, VisitPurposeSales) Then

                    'セールスの場合
                    ' Logger.Debug("SubmitButtonClick_006 VisitPurpose is Sales")

                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
                    '送信処理_新規
                    'msgID = sc3090301Biz.SendNewSales(staffInfo.DlrCD, staffInfo.BrnCD, _
                    '                             DateTimeFunc.FormatDateSD(1, visitDate.Value), _
                    '                             personNum.Value, VisitMeansWalk, staffInfo.Account)
                    '送信処理_新規
                    msgID = sc3090301Biz.SendNewSales(staffInfo.DlrCD, staffInfo.BrnCD, _
                             DateTimeFunc.FormatDateSD(1, visitDate.Value), _
                             personNum.Value, VisitMeansWalk, staffInfo.Account, Nothing)
                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

                ElseIf String.Equals(purposeType.Value, VisitPurposeService) Then

                    'サービスの場合
                    ' Logger.Debug("SubmitButtonClick_007 VisitPurpose is Service")

                    '2014/01/07 TMEJ chin   TMEJ次世代サービス 工程管理機能開発 $06 START
                    '送信処理_新規
                    'msgID = sc3090301Biz.SendNewService(staffInfo.DlrCD, staffInfo.BrnCD, _
                    '                         DateTimeFunc.FormatDateSD(1, visitDate.Value), _
                    '                         personNum.Value, VisitMeansWalk, staffInfo.Account)
                    '2014/01/07 TMEJ chin   TMEJ次世代サービス 工程管理機能開発 $06 END
                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
                    '送信処理_新規
                    'msgID = sc3090301Biz.SendNewService(staffInfo.DlrCD, staffInfo.BrnCD, _
                    '                         DateTimeFunc.FormatDateSD(1, visitDate.Value), _
                    '                         personNum.Value, VisitMeansWalk, staffInfo.Account, staffInfo.UserName)
                    msgID = sc3090301Biz.SendNewService(staffInfo.DlrCD, staffInfo.BrnCD, _
                         DateTimeFunc.FormatDateSD(1, visitDate.Value), _
                         personNum.Value, VisitMeansWalk, staffInfo.Account, staffInfo.UserName, Nothing)

                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
                End If

            Case DispTypeUnsend

                ' Logger.Debug("SubmitButtonClick_008 DispType is Unsend")

                '登録番号読取時画面の場合

                '画面情報取得
                Logger.Info("SubmitButtonClick_008_1 repCar MaxIndex[" & repCar.Items.Count & "]")
                Logger.Info("SubmitButtonClick_008_2 repCar SelectIndex[" & selectVclNoIndex.Value & "]")

                Dim repCust As Repeater = repCar.Items(selectVclNoIndex.Value - 1).FindControl("repCustomer")

                Logger.Info("SubmitButtonClick_008_3 repCustomer MaxIndex[" & repCust.Items.Count & "]")
                Logger.Info("SubmitButtonClick_008_4 repCustomer SelectIndex[" & selectCustIndex.Value & "]")

                unsendRow.VISITVCLSEQ = CType(repCust.Items(selectCustIndex.Value).FindControl("visitVclSeq"), HiddenField).Value
                unsendRow.VISITTIMESTAMP = CType(repCust.Items(selectCustIndex.Value).FindControl("visitTimestamp"), HiddenField).Value
                unsendRow.VCLREGNO = Server.HtmlDecode(CType(repCust.Items(selectCustIndex.Value).FindControl("vclRegNo"), Label).Text)
                unsendRow.CUSTCOUNT = CType(repCust.Items(selectCustIndex.Value).FindControl("custCount"), HiddenField).Value
                unsendRow.NAME = CType(repCust.Items(selectCustIndex.Value).FindControl("name"), Label).Text
                unsendRow.NAMETITLE = CType(repCust.Items(selectCustIndex.Value).FindControl("nameTitle"), Label).Text
                unsendRow.CUSTKBN = CType(repCust.Items(selectCustIndex.Value).FindControl("custKbn"), HiddenField).Value
                ' $01 start step2開発
                'unsendRow.CUSTPIC = DefaultString
                ' $01 end step2開発
                unsendRow.CARINFO1 = DefaultString
                unsendRow.CARINFO2 = DefaultString
                ' $06 start TMEJ次世代サービス 工程管理機能開発
                unsendRow.PROVINCE = CType(repCust.Items(selectCustIndex.Value).FindControl("provinceName"), Label).Text
                ' $06 end   TMEJ次世代サービス 工程管理機能開発
                ' $01 start step2開発
                'unsendRow.CARPIC = DefaultString
                ' $01 end step2開発
                unsendRow.CUSTCD = CType(repCust.Items(selectCustIndex.Value).FindControl("custCd"), HiddenField).Value
                unsendRow.STUFFCD = CType(repCust.Items(selectCustIndex.Value).FindControl("stuffCd"), HiddenField).Value
                unsendRow.SEX = CType(repCust.Items(selectCustIndex.Value).FindControl("sex"), HiddenField).Value
                unsendRow.VIN = CType(repCust.Items(selectCustIndex.Value).FindControl("vin"), HiddenField).Value
                unsendRow.SEQNO = CType(repCust.Items(selectCustIndex.Value).FindControl("seqNo"), HiddenField).Value
                unsendRow.SACODE = CType(repCust.Items(selectCustIndex.Value).FindControl("saCode"), HiddenField).Value

                ' $04 start ウェルカムボード仕様変更対応
                unsendRow.CUSTYPE = CType(repCust.Items(selectCustIndex.Value).FindControl("custType"), HiddenField).Value
                ' $04 end ウェルカムボード仕様変更対応

                repCust = Nothing

                '未送信済みチェック
                ' $11 start 削除フラグ判定追加
                Dim sendCheckMessage As Integer = 0
                sendCheckMessage = sc3090301Biz.IsUnsent(unsendRow.VISITVCLSEQ)
                If sendCheckMessage = 0 Then

                    ' Logger.Debug("SubmitButtonClick_009 Send Value Is UnSend")

                    '未送信の場合のみ処理実行
                    '顧客情報の有無判定
                    If unsendRow.CUSTCOUNT <> 0 Then

                        ' Logger.Debug("SubmitButtonClick_010 OrgOrNewCustomer")

                        '2014/01/07 TMEJ chin   TMEJ次世代サービス 工程管理機能開発 $06 START
                        '自社客・未取引客の場合
                        'msgID = sc3090301Biz.SendOrgOrNewCustomer(staffInfo.DlrCD,
                        '                                          staffInfo.BrnCD, _
                        '                                          personNum.Value, _
                        '                                          purposeType.Value, _
                        '                                          VisitMeansCar, _
                        '                                          staffInfo.Account, _
                        '                                          unsendRow)
                        msgID = sc3090301Biz.SendOrgOrNewCustomer(staffInfo.DlrCD,
                                                                  staffInfo.BrnCD, _
                                                                  personNum.Value, _
                                                                  purposeType.Value, _
                                                                  VisitMeansCar, _
                                                                  staffInfo.Account, _
                                                                  staffInfo.UserName, _
                                                                  unsendRow)
                        '2014/01/07 TMEJ chin   TMEJ次世代サービス 工程管理機能開発 $06 END

                    Else
                        ' Logger.Debug("SubmitButtonClick_011 NotCustomerInfo")

                        '顧客情報なし
                        '2014/01/07 TMEJ chin   TMEJ次世代サービス 工程管理機能開発 $06 START
                        'msgID = sc3090301Biz.SendNotCustomerInfo(staffInfo.DlrCD, _
                        '                                     staffInfo.BrnCD, _
                        '                                     personNum.Value, _
                        '                                     purposeType.Value, _
                        '                                     VisitMeansCar, _
                        '                                     staffInfo.Account, _
                        '                                     unsendRow)
                        msgID = sc3090301Biz.SendNotCustomerInfo(staffInfo.DlrCD, _
                                                             staffInfo.BrnCD, _
                                                             personNum.Value, _
                                                             purposeType.Value, _
                                                             VisitMeansCar, _
                                                             staffInfo.Account, _
                                                             staffInfo.UserName, _
                                                             unsendRow)
                        '2014/01/07 TMEJ chin   TMEJ次世代サービス 工程管理機能開発 $06 END

                    End If
                Else
                    Logger.Debug("SubmitButtonClick_012 Send Value Is Send or Deleted")
                    'エラーメッセージ表示
                    msgID = sendCheckMessage
                End If
                ' $11 end 削除フラグ判定追加

            Case Else
                ' Logger.Debug("SubmitButtonClick_013 ")
                Logger.Info("SubmitButtonClick_End ")
                Return

        End Select

        ' ＤＢ更新が正常終了している場合のみPush送信処理を行う。
        If msgID = MessageIdSuccess Then
            ' $11 STARTTMEJ次世代サービス 工程管理機能開発 START
            ' $05 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
            'msgID = sc3090301Biz.PushExecution(staffInfo.DlrCD, staffInfo.BrnCD, purposeType.Value)
            msgID = sc3090301Biz.PushExecution(purposeType.Value)
            ' $05 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発
            ' $11 STARTTMEJ次世代サービス 工程管理機能開発 END
        End If

        'エラーメッセージ表示
        If Not msgID = MessageIdSuccess Then

            'エラーが発生した場合はメッセージ表示
            ' Logger.Debug("SubmitButtonClick_014 ShowDefeatMessage")
            Me.ShowMessageBox(msgID)
        End If
        Dim preCurrentDisplayNumber As Integer = CInt(CurrentDisplayHeaderNumber.Value)
        '初期表示処理を実行
        If dispType.Value.Equals(DispTypeUnsend) Then

            '削除されていた場合は先頭表示
            If msgID.Equals(MessageIdErrorDelete) Then
                CurrentDisplayHeaderNumber.Value = "1"
            End If

            '登録番号読み込み画面なら次のレコードor待機画面
            PageInit(CInt(CurrentDisplayHeaderNumber.Value), FromDisplayReflesh)

            '表示していた情報の次レコード(現在位置そのまま)
            'スクロールさせる必要がある場合のみ
            '削除済みの場合は除く

            If CInt(unsetDataCount.Value) > 1 And CInt(unsetDataCount.Value) >= (CInt(CurrentDisplayHeaderNumber.Value) + CInt(selectVclNoIndex.Value)) - 1 _
                And CInt(selectVclNoIndex.Value) > 1 And Not msgID.Equals(MessageIdErrorDelete) Then
                autoScrollFlag.Value = FromDisplayReflesh
                preSelectVclNoIndex.Value = selectVclNoIndex.Value
            ElseIf CInt(unsetDataCount.Value) > 1 And CInt(unsetDataCount.Value) < CInt(CurrentDisplayHeaderNumber.Value) + CInt(selectVclNoIndex.Value) - 1 And Not msgID.Equals(MessageIdErrorDelete) _
                Or Not preCurrentDisplayNumber.Equals(CInt(CurrentDisplayHeaderNumber.Value)) And Not msgID.Equals(MessageIdErrorDelete) Then
                preSelectVclNoIndex.Value = CInt(unsetDataCount.Value) - CInt(CurrentDisplayHeaderNumber.Value) + 1
                autoScrollFlag.Value = FromDisplayReflesh
            End If

        Else

            '新規登録画面からなら1件目レコードor待機画面
            CurrentDisplayHeaderNumber.Value = "1"
            selectVclNoIndex.Value = "1"
            PageInit()
        End If




        Logger.Info("SubmitButtonClick_End")
    End Sub
#End Region

    ' $02 Start クルクル対応
#Region "リフレッシュ処理"
    ''' <summary>
    ''' 画面リフレッシュ処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub RefreshButtonClick(sender As Object, e As System.EventArgs) Handles refreshButton.Click

        Logger.Info("RefreshButtonClick_Start")
        ' 画面タイプ判断
        Dim type As String = Me.dispType.Value
        If String.Equals(type, DispTypeNewWalk) Then
            ' 新規登録画面[歩き]の場合、新規登録画面[歩き]を表示する。
            DispNew(DispTypeNewWalk)

        ElseIf String.Equals(type, DispTypeNewCar) Then
            ' 新規登録画面[車]の場合、新規登録画面[車]を表示する。
            DispNew(DispTypeNewCar)
        Else
            ' 上記以外(待機、登録番号読取画面)、初期表示処理を実行する。
            PageInit(CInt(CurrentDisplayHeaderNumber.Value))
        End If

        Logger.Info("RefreshButtonClick_End")
    End Sub

#End Region
    ' $02 End クルクル対応

    '$06 start
#Region "削除ボタン押下"
    ''' <summary>
    ''' 削除ボタン押下時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub AllDeleteButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles DeleteButton.Click
        Logger.Info("AllDeleteButton_Click_start")

        Dim dealerCode As String = StaffContext.Current.DlrCD
        Dim storeCode As String = StaffContext.Current.BrnCD
        Dim nowDate As Date = DateTimeFunc.Now
        Dim messageID As Integer = 0


        Dim Bizlogic As New SC3090301BusinessLogic
        messageID = Bizlogic.DeleteVisitVehicl(dealerCode, storeCode, nowDate)

        '正常に削除終了
        If messageID = 0 Then
            unsetDataCount.Value = "0"
            CurrentDisplayHeaderNumber.Value = "1"
            '未送信データ件数非表示
            lbl_unSendCount.Visible = False
            '登録番号読み取り画面の場合、初期表示
            If dispType.Value.Equals(DispTypeUnsend) Then
                '初期表示処理を実行
                PageInit()
            End If
            'エラーの場合メッセージ表示
        Else
            Me.ShowMessageBox(messageID)
        End If


    End Sub
#End Region

#Region "次のN件押下"
    ''' <summary>
    ''' 次のN件ボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub NextDataButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles NextDataShowButton.Click
        Dim preCurrentDisplayNumber As Integer = CInt(CurrentDisplayHeaderNumber.Value)
        '先頭レコード番号を進める
        CurrentDisplayHeaderNumber.Value = CStr(CInt(CurrentDisplayHeaderNumber.Value) + CInt(NextOrPreviewDisplayCountNumber.Value))
        '画面更新
        PageInit(CInt(CurrentDisplayHeaderNumber.Value), FromNextDataBottun)

        '表示していたレコードの次が無い場合
        If CInt(unsetDataCount.Value) < preCurrentDisplayNumber + CInt(selectVclNoIndex.Value) - 1 Then
            preSelectVclNoIndex.Value = CInt(unsetDataCount.Value) - CInt(CurrentDisplayHeaderNumber.Value) + 1
            autoScrollFlag.Value = "3"

            '表示していた情報の次レコードを画面表示
        ElseIf CInt(unsetDataCount.Value) > 1 And CInt(MaxDisplayCountNumber.Value) > CInt(NextOrPreviewDisplayCountNumber.Value) Then
            autoScrollFlag.Value = "1"
        End If



    End Sub
#End Region

#Region "前のN件押下"
    ''' <summary>
    ''' 次のN件ボタン押下時処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub PreviewDataButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles PreviewDataShowButton.Click

        '先頭レコードを戻す
        CurrentDisplayHeaderNumber.Value = CStr(CInt(CurrentDisplayHeaderNumber.Value) - CInt(NextOrPreviewDisplayCountNumber.Value))
        '画面更新
        PageInit(CInt(CurrentDisplayHeaderNumber.Value), FromPreviewDataBottun)

        '表示していた情報の前レコードを表示
        If CInt(unsetDataCount.Value) > 1 Then
            autoScrollFlag.Value = FromPreviewDataBottun
        End If

    End Sub
#End Region

    '$06 end
#End Region

#Region "Privateメソッド"
    '$06 start 引数追加
    ''' <summary>
    ''' ページの初期表示処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub PageInit(Optional ByVal HeaderNum As Integer = 1, Optional previewOrNextFlag As Integer = 0)
        '$06 end 引数追加

        ' Logger.Debug("PageInit_Start")

        If previewOrNextFlag.Equals(0) Then
            '各文言の設定
            SetWord()
            HeaderNum = CInt(CurrentDisplayHeaderNumber.Value)
        End If


        'ログイン情報取得
        ' Logger.Debug("PageInit_001 " & "Call_Start StaffContext.Current")
        Dim staffInfo As StaffContext = StaffContext.Current
        ' Logger.Debug("PageInit_001 " & "Call_End   StaffContext.Current Ret[" & (staffInfo IsNot Nothing) & "]")

        '現在日時の取得
        ' Logger.Debug("PageInit_002 " & "Call_Start DateTimeFunc.Now Pram[" & staffInfo.DlrCD & "]")
        Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)
        ' Logger.Debug("PageInit_002 " & "Call_End   DateTimeFunc.Now Ret[" & nowDate & "]")

        '$09 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
        ' 販売店システム設定から設定値を取得する
        Dim serviceCommonClass As New ServiceCommonClassBusinessLogic

        ' セールスタブレット使用フラグ
        SalesTabletUseFlg.Value = serviceCommonClass.GetDlrSystemSettingValueBySettingName(UseFlgSalesTabletName)

        ' サービスタブレット使用フラグ
        ServiceTabletUseFlg.Value = serviceCommonClass.GetDlrSystemSettingValueBySettingName(UseFlgServiceTabletName)


        ' 販売店環境設定から設定値を取得する
        Dim dlrEnvSet As New DealerEnvSetting

        ' 車両登録番号入力区分
        VclRegNoInputType.Value = dlrEnvSet.GetEnvSetting(staffInfo.DlrCD, VclRegNoInputTypeParam).PARAMVALUE
        '$09 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END

        Dim sc3090301Biz As SC3090301BusinessLogic = New SC3090301BusinessLogic

        '来店未送信データ件数の取得
        Dim unsentDataTotalCount As Integer = sc3090301Biz.GetUnsentDataTotalCount(staffInfo.DlrCD, staffInfo.BrnCD, nowDate)
        unsetDataCount.Value = unsentDataTotalCount
        '$06 start システム環境値の取得
        Dim sysEnvSet As New SystemEnvSetting
        Dim sysEnvSetTitlePosRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        ' 表示最大件数
        sysEnvSetTitlePosRow = sysEnvSet.GetSystemEnvSetting(MaxDisplayCountParam)
        MaxDisplayCountNumber.Value = CInt(sysEnvSetTitlePosRow.PARAMVALUE)
        ' 取得件数
        sysEnvSetTitlePosRow = sysEnvSet.GetSystemEnvSetting(NextOrPreviewDisplayCountParam)
        NextOrPreviewDisplayCountNumber.Value = CInt(sysEnvSetTitlePosRow.PARAMVALUE)
        '$06 end システム環境値の取得

        '未送信データ件数判定
        If unsentDataTotalCount = 0 Then
            ' Logger.Debug("PageInit_003 NotUnSendData")

            '未送信データが存在しない場合、待機画面を表示
            dispType.Value = DispTypeWait
            CurrentDisplayHeaderNumber.Value = "1"
            '未送信データ件数非表示
            lbl_unSendCount.Visible = False
            ' 次、前へボタン非表示
            lbl_PreviewDataCount.Style.Item("display") = "none"
            lbl_NextDataCount.Style.Item("display") = "none"

            ' ダミーデータの作成
            Dim sc3090301DataSet As New SC3090301DataSet
            Dim vclUnSentTblAdd As SC3090301VisitVehicleUnsentDataDataTable = sc3090301DataSet.SC3090301VisitVehicleUnsentData
            Dim unsentTbl As SC3090301VisitUnsentDataDataTable = sc3090301DataSet.SC3090301VisitUnsentData

            vclUnSentTblAdd.AddSC3090301VisitVehicleUnsentDataRow(1, _
                                                                  Date.Now, _
                                                                  " ")

            ' $01 start step2開発
            'unsentTbl.AddSC3090301VisitUnsentDataRow(1, _
            '                                        Date.Now, _
            '                                        " ", _
            '                                        0, _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        CStr(0), _
            '                                        "")
            ' $06 start TMEJ次世代サービス 工程管理機能開発
            '' $04 start ウェルカムボード仕様変更対応
            'unsentTbl.AddSC3090301VisitUnsentDataRow(1, _
            '                                        Date.Now, _
            '                                        " ", _
            '                                        0, _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        "", _
            '                                        CStr(0), _
            '                                        "",
            '                                        "")
            '' $04 end ウェルカムボード仕様変更対応
            unsentTbl.AddSC3090301VisitUnsentDataRow(1, _
                                                    Date.Now, _
                                                    " ", _
                                                    0, _
                                                    "", _
                                                    "", _
                                                    "", _
                                                    "", _
                                                    "", _
                                                    "", _
                                                    "", _
                                                    "", _
                                                    "", _
                                                    CStr(0), _
                                                    "", _
                                                    "", _
                                                    "", _
                                                    ReservFlagOff, _
                                                    "", _
                                                    "")
            ' $06 end TMEJ次世代サービス 工程管理機能開発
            ' $01 end step2開発

            'テーブルの関連付け
            sc3090301DataSet.Relations.Add("relation", _
                               sc3090301DataSet.Tables("SC3090301VisitVehicleUnsentData").Columns("VISITVCLSEQ"), _
                               sc3090301DataSet.Tables("SC3090301VisitUnsentData").Columns("VISITVCLSEQ"))

            repCar.DataSource = sc3090301DataSet
            repCar.DataBind()

            ' ダミーデータの表示切替
            Dim repCust As Repeater = repCar.Items(0).FindControl("repCustomer")

            repCar.Items(0).FindControl("input_right1").Visible = False
            repCust.Items(0).FindControl("CustInfo").Visible = True
            repCust.Items(0).FindControl("noCustInfo").Visible = False

            repCust.Items(0).FindControl("tableLine").Visible = False
            repCust.Items(0).FindControl("tableUp").Visible = False
            repCust.Items(0).FindControl("tableBottom").Visible = False
            repCust.Items(0).FindControl("noVclInfo").Visible = True

            ' 文言設定
            CType(repCust.Items(0).FindControl("noVclInfo"), Label).Text = _
                Server.HtmlEncode(WebWordUtility.GetWord(11))

            'コンテンツ部分の表示切替
            dispWait.Visible = False
            vclNoRead.Visible = True
            newCar.Visible = False
            newWalk.Visible = False

            ' Logger.Debug("PageInit_End")
            Return

        End If

        '--------------------------------------------------------------
        ' 登録番号読取時画面を表示
        '--------------------------------------------------------------
        '未送信データ件数を保持
        lbl_unSendCount.Text = unsentDataTotalCount
        If unsentDataTotalCount > 1 Then
            ' Logger.Debug("PageInit_004 UnSendData 1 Qreater")

            lbl_unSendCount.Visible = True

        Else
            ' Logger.Debug("PageInit_005 UnSendData 1 Count")

            lbl_unSendCount.Visible = False

        End If

        '先頭レコード番号のデータが無ければ表示範囲を戻す
        While (unsentDataTotalCount < CInt(CurrentDisplayHeaderNumber.Value))
            CurrentDisplayHeaderNumber.Value = CStr(CInt(CurrentDisplayHeaderNumber.Value) - CInt(NextOrPreviewDisplayCountNumber.Value))
            HeaderNum = CInt(CurrentDisplayHeaderNumber.Value)
        End While

        ' 最大表示件数を超えない場合はボタン表示処理を行わない
        If unsentDataTotalCount > CInt(MaxDisplayCountNumber.Value) Then
            displayPreviewNextBottun(unsentDataTotalCount, previewOrNextFlag, HeaderNum)
        Else
            lbl_PreviewDataCount.Style.Item("display") = "none"
            lbl_NextDataCount.Style.Item("display") = "none"
        End If

        Dim maxReadNumber As Integer = CInt(MaxDisplayCountNumber.Value) + HeaderNum - 1


        ' 来店未送信データの取得
        ' $06 start 引数追加
        Dim unsetDataDataSet As SC3090301DataSet _
            = sc3090301Biz.GetUnsentData(staffInfo.DlrCD, staffInfo.BrnCD, nowDate, HeaderNum, maxReadNumber)
        ' $06 end 引数追加

        'テーブルの関連付け
        unsetDataDataSet.Relations.Add("relation", _
                           unsetDataDataSet.Tables("SC3090301VisitVehicleUnsentData").Columns("VISITVCLSEQ"), _
                           unsetDataDataSet.Tables("SC3090301VisitUnsentData").Columns("VISITVCLSEQ"))

        '画面初期表示
        Me.dispType.Value = DispTypeUnsend

        '画面に各値を設定
        repCar.DataSource = unsetDataDataSet
        repCar.DataBind()
        SetDispValue()

        ' Logger.Debug("PageInit_End")
    End Sub

    ''' <summary>
    ''' 画面の各値を設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetDispValue()

        ' Logger.Debug("SetDispValue_Start")

        '敬称の表示位置を取得
        Dim sysEnvSet As New SystemEnvSetting
        Dim sysEnvSetRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        Logger.Info("SetDispValue_001 " & "Call_Start SystemEnvSetting.GetSystemEnvSetting Pram[" & KeisyoZengo & "]")
        sysEnvSetRow = sysEnvSet.GetSystemEnvSetting(KeisyoZengo)
        Logger.Info("SetDispValue_001 " & "Call_End   SystemEnvSetting.GetSystemEnvSetting Ret[" & (sysEnvSetRow IsNot Nothing) & "]")

        Dim repCarIndex As Integer
        Dim repCustIndex As Integer
        '車両登録番号分繰り返す
        For repCarIndex = 0 To repCar.Items.Count - 1

            ' 車両登録番号の文字切り
            Dim vclRegNo As String = CType(repCar.Items(repCarIndex).FindControl("txt_car_number1"), Label).Text
            ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
            'CType(repCar.Items(repCarIndex).FindControl("txt_car_number1"), Label).Text = WordFormat(vclRegNo, WordMaxLengthVclRegNo, AddDefaultValue)
            CType(repCar.Items(repCarIndex).FindControl("txt_car_number1"), Label).Text = vclRegNo
            ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

            Dim custInfoCount As Integer
            Dim repCust As Repeater = repCar.Items(repCarIndex).FindControl("repCustomer")

            '該当顧客情報分繰り返す
            For repCustIndex = 0 To repCust.Items.Count - 1

                'お客様件数取得
                custInfoCount = CType(repCust.Items(repCustIndex).FindControl("custCount"), HiddenField).Value

                ' 顧客情報の有無
                If custInfoCount = 0 Then

                    ' Logger.Debug("SetDispValuet_002 CustomerInfoNotExist")

                    '顧客情報が存在しない
                    repCust.Items(repCustIndex).FindControl("CustInfo").Visible = False
                    repCust.Items(repCustIndex).FindControl("noCustInfo").Visible = True
                    CType(repCust.Items(repCustIndex).FindControl("noCustInfo").FindControl("newCustomerWord"), Label).Text = _
                        Server.HtmlEncode(WebWordUtility.GetWord(WordIdNewCustomer))

                Else
                    ' Logger.Debug("SetDispValue_003 CustomerInfoExist")

                    '顧客情報が存在する
                    repCust.Items(repCustIndex).FindControl("CustInfo").Visible = True
                    repCust.Items(repCustIndex).FindControl("noCustInfo").Visible = False

                    'お客様名
                    Dim custName As String = CType(repCust.Items(repCustIndex).FindControl("nameDisp"), Label).Text
                    If String.IsNullOrEmpty(Trim(custName)) Then
                        ' Logger.Debug("SetDispValue_004 CustomerNameExist")

                        'お客様名が未設定
                        custName = Server.HtmlEncode(WebWordUtility.GetWord(WordIdDefaultValueName))

                    ElseIf String.Equals(sysEnvSetRow.PARAMVALUE, HonorificTitleMae) Then
                        ' Logger.Debug("SetDispValue_005 HonorificTitleMae")

                        '敬称表示位置が前
                        custName = CType(repCust.Items(repCustIndex).FindControl("nameTitleDisp"), Label).Text & " " & custName
                    Else
                        ' Logger.Debug("SetDispValue_006 HonorificTitleUshiro")

                        '敬称表示位置が後
                        custName = custName & " " & CType(repCust.Items(repCustIndex).FindControl("nameTitleDisp"), Label).Text

                    End If
                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
                    'CType(repCust.Items(repCustIndex).FindControl("custName"), Label).Text = WordFormat(Trim(custName), WordMaxLengthCustName, AddDefaultValue)
                    CType(repCust.Items(repCustIndex).FindControl("custName"), Label).Text = Trim(custName)
                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

                    '車両情報
                    Dim carInfo1 As String = CType(repCust.Items(repCustIndex).FindControl("carInfo1"), Label).Text
                    Dim carInfo2 As String = CType(repCust.Items(repCustIndex).FindControl("carInfo2"), Label).Text

                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
                    ''車両情報１・２共に値が設定されている場合
                    'carInfo1 = WordFormat(Trim(carInfo1), WordMaxLengthCarInfo1, AddDefaultValue)
                    'carInfo2 = WordFormat(Trim(carInfo2), WordMaxLengthCarInfo2, AddDefaultValue)
                    '車両情報１・２共に値が設定されている場合
                    carInfo1 = Trim(carInfo1)
                    carInfo2 = Trim(carInfo2)
                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
                    If Not String.IsNullOrEmpty(Trim(carInfo1)) And Not String.IsNullOrEmpty(Trim(carInfo2)) Then

                        ' Logger.Debug("SetDispValue_007 CarInfoNothing")

                        '改行して表示
                        CType(repCust.Items(repCustIndex).FindControl("carInfo"), Label).Text = carInfo1 & "<br />" & carInfo2
                    Else
                        ' Logger.Debug("SetDispValue_008 CarInfoExist")

                        '設定されている項目のみ設定
                        If Not String.IsNullOrEmpty(Trim(carInfo1)) Then

                            ' Logger.Debug("SetDispValue_009 carInfo1 IsNullOrEmpty")
                            CType(repCust.Items(repCustIndex).FindControl("carInfo"), Label).Text = carInfo1
                        Else

                            ' Logger.Debug("SetDispValue_010 carInfo2 IsNullOrEmpty")
                            CType(repCust.Items(repCustIndex).FindControl("carInfo"), Label).Text = carInfo2
                        End If
                    End If

                    ' $06 start TMEJ次世代サービス 工程管理機能開発
                    'Province情報
                    Dim provinceName As String = CType(repCust.Items(repCustIndex).FindControl("provinceName"), Label).Text

                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
                    'CType(repCust.Items(repCustIndex).FindControl("province"), Label).Text = "<br /><p>" & provinceName & "</p>"
                    CType(repCust.Items(repCustIndex).FindControl("province"), Label).Text = "<p>" & provinceName & "</p>"
                    ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

                    ' $06 end   TMEJ次世代サービス 工程管理機能開発

                    '$07 Start UAT課題#158
                    Dim reservFlg As String = CType(repCust.Items(repCustIndex).FindControl("reservFlg"), HiddenField).Value

                    If Not String.IsNullOrEmpty(reservFlg) AndAlso String.Equals(ReservFlgExistAppointment, reservFlg) Then
                        CType(repCust.Items(repCustIndex).FindControl("appointmentIcon"), Label).Text = _
                            Server.HtmlEncode(WebWordUtility.GetWord(WordIdAppointmentIcon))
                    Else
                        CType(repCust.Items(repCustIndex).FindControl("appointmentIcon"), Label).Visible = False
                    End If
                    '$07 End UAT課題#158

                End If

                ' 不要なフィールドを削除する。
                ' $01 start step2開発
                'CType(repCust.Items(repCustIndex).FindControl("custPic"), HiddenField).Visible = False
                ' $01 end step2開発
                CType(repCust.Items(repCustIndex).FindControl("carInfo1"), Label).Visible = False
                CType(repCust.Items(repCustIndex).FindControl("carInfo2"), Label).Visible = False
                ' $01 start step2開発
                'CType(repCust.Items(repCustIndex).FindControl("carPic"), HiddenField).Visible = False
                ' $01 end step2開発
            Next

            ' 顧客情報の有無
            If custInfoCount = 0 Then
                ' Logger.Debug("SetDispValue_011 CustomerInfoNotExist")

                '顧客情報が存在しない
                CType(repCar.Items(repCarIndex).FindControl("lampEvenAria"), Panel).Visible = False
                CType(repCar.Items(repCarIndex).FindControl("lampOddAria"), Panel).Visible = False

            Else
                ' Logger.Debug("SetDispValue_012 CustomerInfoExist")

                '顧客情報が存在する

                'スクロールランプの表示
                Dim lampTagName As String = String.Empty
                Dim lampMaxIndex As Integer
                Dim lampIndex As Integer

                '該当の顧客件数の判断
                If custInfoCount > LampMaxIndexOdd Then

                    ' Logger.Debug("SetDispValue_013 CustomerCount Is MaxCount")

                    '顧客件数が最大数より多い
                    CType(repCar.Items(repCarIndex).FindControl("lampOddAria"), Panel).Visible = True
                    CType(repCar.Items(repCarIndex).FindControl("lampEvenAria"), Panel).Visible = False

                ElseIf custInfoCount Mod 2 = 0 Then

                    ' Logger.Debug("SetDispValue_014 CustomerCount Is Even")

                    '顧客件数が偶数
                    CType(repCar.Items(repCarIndex).FindControl("lampEvenAria"), Panel).Visible = True
                    CType(repCar.Items(repCarIndex).FindControl("lampOddAria"), Panel).Visible = False

                    lampTagName = "input_lamp_even"
                    lampMaxIndex = LampMaxIndexEven

                Else

                    ' Logger.Debug("SetDispValue_015 CustomerCount Is Odd")

                    '顧客件数が奇数
                    CType(repCar.Items(repCarIndex).FindControl("lampOddAria"), Panel).Visible = True
                    CType(repCar.Items(repCarIndex).FindControl("lampEvenAria"), Panel).Visible = False

                    lampTagName = "input_lamp_odd"
                    lampMaxIndex = LampMaxIndexOdd
                End If

                If Not String.IsNullOrEmpty(lampTagName) Then

                    ' Logger.Debug("SetDispValue_016 lampTagName IsNullOrEmpty")

                    '不要なスクロールランプを削除
                    For lampIndex = 1 To custInfoCount

                        repCar.Items(repCarIndex).FindControl(lampTagName & lampIndex).Visible = True
                        repCar.Items(repCarIndex).FindControl(lampTagName & lampIndex & "_n").Visible = True

                    Next
                End If

            End If
        Next

        ' コンテンツ部分表示切替
        dispWait.Visible = False
        vclNoRead.Visible = True
        newCar.Visible = False
        newWalk.Visible = False

        ' Logger.Debug("SetDispValue_End")

    End Sub

    ''' <summary>
    ''' 新規入力画面の表示処理
    ''' </summary>
    ''' <param name="dispTypeVal">画面タイプ</param>
    ''' <remarks></remarks>
    Private Sub DispNew(ByVal dispTypeVal As String)

        ' Logger.Debug("DispNew_Start Pram[" & dispTypeVal & "]")

        ' $05 start 次、前へボタンの非表示
        lbl_PreviewDataCount.Style.Item("display") = "none"
        lbl_NextDataCount.Style.Item("display") = "none"

        'ログイン情報取得
        ' Logger.Debug("DispNew_001 " & "Call_Start StaffContext.Current")
        Dim staffInfo As StaffContext = StaffContext.Current
        ' Logger.Debug("DispNew_001 " & "Call_Start StaffContext.Current Ret[" & (staffInfo IsNot Nothing) & "]")

        '現在日時の取得
        ' Logger.Debug("DispNew_002 " & "Call_Start DateTimeFunc.Now Pram[" & staffInfo.DlrCD & "]")
        Dim nowDate As Date = DateTimeFunc.Now(staffInfo.DlrCD)
        ' Logger.Debug("DispNew_002 " & "Call_End   DateTimeFunc.Now Ret[" & nowDate & "]")

        '来店時間設定
        ' Logger.Debug("DispNew_003 " & "Call_Start DateTimeFunc.FormatDate Pram[" & 14 & "," & nowDate & "]")
        Dim formatDate As String = DateTimeFunc.FormatDate(14, nowDate)
        ' Logger.Debug("DispNew_003 " & "Call_End   DateTimeFunc.FormatDate Ret[" & lbl_time_newCar.Text & "]")

        If String.Equals(dispTypeVal, DispTypeNewCar) Then

            ' Logger.Debug("DispNew_004 DispType is NewCar")

            lbl_time_newCar.Text = formatDate

            dispWait.Visible = False
            vclNoRead.Visible = False
            newCar.Visible = True
            newWalk.Visible = False

        ElseIf String.Equals(dispTypeVal, DispTypeNewWalk) Then

            ' Logger.Debug("DispNew_005 DispType is NewWalk")

            lbl_time_newWalk.Text = formatDate

            dispWait.Visible = False
            vclNoRead.Visible = False
            newCar.Visible = False
            newWalk.Visible = True

        End If

        '画面タイプが待機の場合
        If String.Equals(dispType.Value, DispTypeWait) Then

            ' Logger.Debug("DispNew_006 DispType is Wait")

            '未送信データ件数アイコンを非表示
            lbl_unSendCount.Visible = False

        End If

        ' Logger.Debug("DispNew_007 " & "Call_Start DateTimeFunc.FormatDate Pram[" & 1 & "," & nowDate & "]")
        visitDate.Value = DateTimeFunc.FormatDate(1, nowDate)
        ' Logger.Debug("DispNew_007 " & "Call_End   DateTimeFunc.FormatDate Ret[" & visitDate.Value & "]")
        dispType.Value = dispTypeVal

        '文言設定
        SetWord()

        ' Logger.Debug("DispNew_End")
    End Sub

    ''' <summary>
    ''' 文言の設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetWord()

        ' Logger.Debug("SetWord_Start")

        ' Logger.Debug("SetWord_001 " & "Call_Start WebWordUtility.GetWord Pram[" & WordIdDispTitle & "]")
        CType(Page.Master.FindControl("MstPG_TitleLabel"), Label).Text = Server.HtmlEncode(WebWordUtility.GetWord(WordIdDispTitle))
        ' Logger.Debug("SetWord_001 " & "Call_Start WebWordUtility.GetWord Ret[" & CType(Page.Master.FindControl("MstPG_TitleLabel"), Label).Text & "]")

        ' Logger.Debug("SetWord_002 " & "Call_Start WebWordUtility.GetWord Pram[" & WordIdSubmit & "]")
        Dim submitButtonText As String = WebWordUtility.GetWord(WordIdSubmit)
        submitButtonText = WordFormat(submitButtonText, WordMaxLengthSubmit)
        ' Logger.Debug("SetWord_002 " & "Call_Start WebWordUtility.GetWord Ret[" & submitButtonText & "]")

        ' $02 Start クルクル対応
        'btn_submit_o.Text = submitButtonText
        'lbl_submit.Text = Server.HtmlEncode(submitButtonText)
        '$09 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 START
        'Me.wordSubmit_n.Text = Server.HtmlEncode(submitButtonText)
        'Me.wordSubmit_o.Text = Server.HtmlEncode(submitButtonText)
        '$09 2019/02/28 NSK 河谷 REQ-SVT-TMT-20180601-001 Gate Keeper機能の視認性操作性改善 END
        ' $02 End クルクル対応
        Me.AllDeleteText.Value = Server.HtmlEncode(WebWordUtility.GetWord(15))
        Me.pullDownString.Text = WebWordUtility.GetWord(8)
        Me.releaseString.Text = WebWordUtility.GetWord(9)
        Me.loadString.Text = WebWordUtility.GetWord(10)
        Me.DeleteText.Text = Server.HtmlEncode(WebWordUtility.GetWord(14))

        ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
        Me.ConfirmMessageText.Value = WebWordUtility.GetWord(905)
        ' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

        ' Logger.Debug("SetWord_End")
    End Sub

    ''' <summary>
    ''' 文言の文字切りを行います。
    ''' </summary>
    ''' <param name="wordVal">対象文字列</param>
    ''' <param name="length">桁数</param>
    ''' <param name="addVal">付加文字列</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function WordFormat(ByVal wordVal As String, ByVal length As Integer, Optional ByVal addVal As String = "") As String

        ' Logger.Debug("WordFormat_Start Pram[" & wordVal & length & addVal & "]")

        Dim decodeValue As String = Server.HtmlDecode(wordVal)

        '空文字か判定
        If String.IsNullOrEmpty(decodeValue) Then

            ' Logger.Debug("WordFormat_001 Value is NullOrEmpty")
            ' Logger.Debug("WordFormat_End Ret[" & wordVal & "]")
            Return wordVal

        End If

        '最大文字数以内であればそのまま返却
        If decodeValue.Length <= length Then

            ' Logger.Debug("WordFormat_002 Value is smallLength")
            ' Logger.Debug("WordFormat_End Ret[" & wordVal & "]")
            Return wordVal

        End If

        Dim retVal As String = Left(decodeValue, length) & addVal

        ' Logger.Debug("WordFormat_End Ret[" & retVal & "]")
        Return Server.HtmlEncode(retVal)

    End Function

#End Region

    ''' <summary>
    ''' 次の(前の)N件ボタンの表示処理
    ''' </summary>
    ''' <param name="unsentDataTotalCount">未送信データ件数</param>
    ''' <param name="previewOrNextFlag">遷移方法 0:リフレッシュまたは初期表示 1:次へボタンから 2:前へボタンから</param>
    ''' <param name="HeaderNum">現在表示しているデータの先頭データが何件目のデータであるか(0 origin)</param>
    ''' <remarks></remarks>
    Private Sub displayPreviewNextBottun(unsentDataTotalCount As Integer, previewOrNextFlag As Integer, HeaderNum As Integer)

        Dim nextCount As Integer = unsentDataTotalCount - (HeaderNum + CInt(MaxDisplayCountNumber.Value) - 1)
        If nextCount > CInt(NextOrPreviewDisplayCountNumber.Value) Then
            nextCount = CInt(NextOrPreviewDisplayCountNumber.Value)
        End If

        Me.Text_PreviewDataCount.Text = Server.HtmlEncode(String.Format(WebWordUtility.GetWord(ApplicationId, "17"), CInt(NextOrPreviewDisplayCountNumber.Value)))
        Me.Text_NextDataCount.Text = Server.HtmlEncode(String.Format(WebWordUtility.GetWord(ApplicationId, "16"), nextCount))

        ' 今表示しているレコードが先頭かつ前にデータが存在する場合
        ' データ更新の場合で前にN件存在する
        ' 次へボタンを押して遷移した、かつ最大表示件数 > 読み込み件数
        ' 先頭データの送信かつ送信より次があるとき
        If previewOrNextFlag.Equals(0) And HeaderNum <> 1 _
            Or previewOrNextFlag.Equals(1) And HeaderNum <> 1 And MaxDisplayCountNumber.Value.Equals(NextOrPreviewDisplayCountNumber.Value) _
            Or previewOrNextFlag.Equals(3) And HeaderNum <> 1 Then
            lbl_PreviewDataCount.Style.Item("display") = "inline-block"
            lbl_NextDataCount.Style.Item("display") = "none"

            ' 今表示しているレコードが表示範囲の末尾かつ後ろにデータが存在する場合
            ' 前へボタンを押して遷移した場合
            ' 最後尾データの送信かつ後ろにデータがある
        ElseIf previewOrNextFlag.Equals(2) And unsentDataTotalCount > CInt(MaxDisplayCountNumber.Value) _
            And MaxDisplayCountNumber.Value.Equals(NextOrPreviewDisplayCountNumber.Value) _
            Or previewOrNextFlag.Equals(3) And selectVclNoIndex.Value.Equals(MaxDisplayCountNumber.Value) Then

            If nextCount > 0 Then
                lbl_PreviewDataCount.Style.Item("display") = "none"
                lbl_NextDataCount.Style.Item("display") = "inline-block"
            Else
                lbl_PreviewDataCount.Style.Item("display") = "none"
                lbl_NextDataCount.Style.Item("display") = "none"
            End If
        Else
            lbl_PreviewDataCount.Style.Item("display") = "none"
            lbl_NextDataCount.Style.Item("display") = "none"
        End If

    End Sub

End Class
