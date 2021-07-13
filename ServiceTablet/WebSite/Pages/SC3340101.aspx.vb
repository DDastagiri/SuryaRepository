'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3340101.aspx.vb
'─────────────────────────────────────
'機能： 洗車マンメインメニュー(CW)画面
'補足： 
'作成： 2015/01/05 TMEJ 範  　NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
'更新： 2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発
'更新： 2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
'更新：
'─────────────────────────────────────
Imports System.Web.UI
Imports System.Web.Script.Serialization
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CarWash.MainMenu.BizLogic
Imports Toyota.eCRB.CarWash.MainMenu.DataAccess
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.SystemFrameworks.Web.Controls

Partial Class Pages_SC3340101
    Inherits BasePage

#Region "定数"

    ''' <summary>
    ''' プログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRA_MID As String = "SC3340101"

    ''' <summary>
    ''' 初期表示時のフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAGELOAD_FLG As String = "0"

    ''' <summary>
    ''' 次のボタンを押すフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ADDLOAD_FLG As String = "1"

    ''' <summary>
    ''' 店内待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WAIT_IN As String = "0"

    ''' <summary>
    ''' 予約客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RFLG_RESERVE As String = "0"

    ''' <summary>
    ''' 洗車待ち
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SVCSTATUS_CARWASHWAIT As String = "07"

    ''' <summary>
    ''' 洗車中
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SVCSTATUS_CARWASHSTART As String = "08"


    ''' <summary>
    ''' 画面自動リフレッシュのシステム設定名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REFRESH_INTERVAL As String = "SC3340101_REFRESH_INTERVAL"

    ''' <summary>
    ''' N件
    ''' </summary>
    ''' <remarks></remarks>
    Private Const READ_COUNT As String = "SC3340101_DEFAULT_READ_COUNT"

    ''' <summary>
    ''' N件
    ''' </summary>
    ''' <remarks></remarks>
    Private SYS_COUNT As Long = 0

    ''' <summary>
    ''' N秒
    ''' </summary>
    ''' <remarks></remarks>
    Private REFRESH_TIME As Long = 0

    ''' <summary>
    ''' デフォルト日時
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MINDATE As String = "1900/01/01 0:00:00"
    '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
    ''' <summary>
    ''' Pアイコン表示フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ICON_FLAG_1 As String = "1"
    ''' <summary>
    ''' Lアイコン表示フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ICON_FLAG_2 As String = "2"

#Region "アイコン文言"
    ''' <summary>
    ''' Pマーク文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdPmark As String = "10001"
    ''' <summary>
    ''' Lマーク文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordIdLmark As String = "10002"
#End Region
    '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

#End Region

#Region "イベント処理メソッド"

    ''' <summary>
    ''' 画面ロードの処理を実施
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        If Not Me.Page.IsCallback Then

            'Hiddenコントロールの設定
            Me.SetHiddenValue()

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#Region "ボタンクリックイベント"

#Region "ポストバック用内部クラス"
    ''' <summary>
    ''' ポストバック用引数の内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class PostBackParamClass
        Public Property Method As String
        Public Property StallUseId As Decimal
        Public Property JobDtlId As Decimal
        Public Property SvcInId As Decimal
        Public Property RoNum As String
        Public Property RowLockVersion As Long
        Public Property PickDeliType As String
        Public Property ShowRowNum As Long
    End Class

#End Region

    ''' <summary>
    ''' 初期表示（データ表示用）処理を行う。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub MainLoadingButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMainLoading.Click

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Me.SetCarWashInfo(PAGELOAD_FLG)

        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "UpdatePanel1", "AfterUpdatePanel();", True)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))


    End Sub

    ''' <summary>
    ''' 次のN件ダミーボタンイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub AddLoadingButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLoading.Click

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}_Start.", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name))

        Me.SetCarWashInfo(ADDLOAD_FLG)

        ScriptManager.RegisterStartupScript(UpdatePanel1, UpdatePanel1.GetType(), "UpdatePanel1", "AfterNextUpdatePanel();", True)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 洗車開始処理を行う。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub CarWashStartButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCarWashStart.Click

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Me.ClickEvent()

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 洗車Undoダミーボタンイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub CarWashUndoButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCarWashUndo.Click

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Me.ClickEvent()

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 洗車スキップダミーボタンイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub CarWashSkipButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCarWashSkip.Click

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Me.ClickEvent()

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 洗車終了ボタンクリックイベント
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub CarWashFinishButton(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCarWashFinish.Click

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Me.ClickEvent()

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 洗車開始、終了などダミーボタンのクリックイベント
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClickEvent()

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim result As Long = ActionResult.Success

        If Not String.IsNullOrEmpty(Me.hidPostBackParamClass.Value) Then

            Dim serializer = New JavaScriptSerializer
            Dim argument As PostBackParamClass = New PostBackParamClass
            Dim staffInfo As StaffContext = StaffContext.Current

            argument = serializer.Deserialize(Of PostBackParamClass)(Me.hidPostBackParamClass.Value)

            If CheckParams(argument) Then
                '貰った引数チェック問題ない場合

                Select Case argument.Method

                    Case "ClickBtnStart"
                        '洗車開始ボタンを押すイベント

                        result = Me.StartWashCar(argument.SvcInId, _
                                                    argument.JobDtlId, _
                                                    argument.StallUseId, _
                                                    argument.RowLockVersion)

                    Case "ClickBtnSkip"
                        '洗車スキップボタンを押すイベント

                        result = Me.SkipWashCar(argument.SvcInId, _
                                                argument.JobDtlId, _
                                                argument.StallUseId, _
                                                argument.PickDeliType, _
                                                argument.RowLockVersion, _
                                                argument.RoNum)

                    Case "ClickBtnFinish"
                        '洗車終了ボタンを押すイベント

                        result = Me.FinishWashCar(argument.SvcInId, _
                                                    argument.JobDtlId, _
                                                    argument.StallUseId, _
                                                    argument.PickDeliType, _
                                                    argument.RowLockVersion, _
                                                    argument.RoNum)

                    Case "ClickBtnUndo"
                        '洗車Undoボタンを押すイベント

                        result = Me.UndoWashCar(argument.SvcInId, _
                                                argument.JobDtlId, _
                                                argument.StallUseId, _
                                                argument.RowLockVersion)

                End Select

            Else
                '引数チェックエラーの場合

                '予期せぬエラーを設定
                result = ActionResult.ExceptionError

            End If

        Else

            '予期せぬエラーを設定
            result = ActionResult.ExceptionError

        End If

        'エラー文言を取得
        Me.hidErrorMeg.Value = HttpUtility.HtmlEncode(GetErrorMessage(result))



        If result <> ActionResult.Success Then
            'エラーの場合、エラーログを出す

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                        "{0}_ErrorCode:{1}", _
                                        System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                        result))

        Else
            'ほかの場合、Infoログを出す

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                        "{0}_End", _
                                        System.Reflection.MethodBase.GetCurrentMethod.Name))

        End If

    End Sub

    ''' <summary>
    ''' パラメータチェック
    ''' </summary>
    ''' <param name="inArgument">チェック用のパラメータ</param>
    ''' <returns>チェック結果</returns>
    ''' <remarks></remarks>
    Private Function CheckParams(ByVal inArgument As PostBackParamClass) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        'inArgumentのチェック
        If IsNothing(inArgument) Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}_End. inArgument is nothing.", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return False

        End If

        'Methodのチェック
        If String.IsNullOrEmpty(inArgument.Method) Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}_End. Parameter Method check error.", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return False

        End If

        'SvcInIdのチェック
        If IsNothing(inArgument.SvcInId) OrElse inArgument.SvcInId = 0 Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}_End. Parameter SvcInId check error.", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return False

        End If

        'JobDtlIdのチェック
        If IsNothing(inArgument.JobDtlId) OrElse inArgument.JobDtlId = 0 Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}_End. Parameter JobDtlId check error.", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return False

        End If

        'StallUseIdのチェック
        If IsNothing(inArgument.StallUseId) OrElse inArgument.StallUseId = 0 Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}_End. Parameter StallUseId check error.", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return False

        End If

        'RowLockVersionのチェック
        If IsNothing(inArgument.RowLockVersion) Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}_End. Parameter RowLockVersion check error.", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return False

        End If

        If inArgument.Method = "ClickBtnSkip" OrElse inArgument.Method = "ClickBtnFinish" Then
            '洗車スキップまたは終了する場合

            'PickDeliTypeのチェック
            If IsNothing(inArgument.PickDeliType) Then

                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                           "{0}_End. Parameter PickDeliType check error.", _
                                           System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return False

            End If

            'RoNumのチェック
            If IsNothing(inArgument.RoNum) Then

                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                           "{0}_End. Parameter RoNum check error.", _
                                           System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return False

            End If

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End.Return True", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return True

    End Function

#End Region


#End Region

#Region "開始など関数"

    ''' <summary>
    ''' 洗車開始登録
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール使用ID</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    Private Function StartWashCar(ByVal inServiceInId As Decimal, _
                                  ByVal inJobDtlId As Decimal, _
                                  ByVal inStallUseId As Decimal, _
                                  ByVal inRowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim result As Long

        Using biz As New SC3340101BusinessLogic

            '洗車開始登録
            result = biz.RegisterCarWashStart(inServiceInId, _
                                              inJobDtlId, _
                                              inStallUseId, _
                                              inRowLockVersion)

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        '実行結果を戻す
        Return result

    End Function

    ''' <summary>
    ''' 洗車終了登録
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inPickDeliType">引取納車区分</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inRONum">RO番号</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    Private Function FinishWashCar(ByVal inServiceInId As Decimal, _
                                   ByVal inJobDtlId As Decimal, _
                                   ByVal inStallUseId As Decimal, _
                                   ByVal inPickDeliType As String, _
                                   ByVal inRowLockVersion As Long, _
                                   ByVal inRoNum As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim result As Long

        Using biz As New SC3340101BusinessLogic

            '洗車開始登録
            result = biz.RegisterCarWashFinish(inServiceInId, _
                                               inJobDtlId, _
                                               inStallUseId, _
                                               inPickDeliType, _
                                               inRowLockVersion, _
                                               inRoNum)

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        '実行結果を戻す
        Return result

    End Function

    ''' <summary>
    ''' 洗車Undo登録
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール使用ID</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    Private Function UndoWashCar(ByVal inServiceInId As Decimal, _
                                 ByVal inJobDtlId As Decimal, _
                                 ByVal inStallUseId As Decimal, _
                                 ByVal inRowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim result As Long

        Using biz As New SC3340101BusinessLogic

            '洗車開始登録
            result = biz.RegisterCarWashUndo(inServiceInId, _
                                             inJobDtlId, _
                                             inStallUseId, _
                                             inRowLockVersion)

        End Using


        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        '実行結果を戻す
        Return result

    End Function

    ''' <summary>
    ''' 洗車スキップ登録
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール使用ID</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    Private Function SkipWashCar(ByVal inServiceInId As Decimal, _
                                 ByVal inJobDtlId As Decimal, _
                                 ByVal inStallUseId As Decimal, _
                                 ByVal inPickDeliType As String, _
                                 ByVal inRowLockVersion As Long, _
                                 ByVal inRoNum As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim result As Long

        Using biz As New SC3340101BusinessLogic

            '洗車開始登録
            result = biz.RegisterCarWashSkip(inServiceInId, _
                                             inJobDtlId, _
                                             inStallUseId, _
                                             inPickDeliType, _
                                             inRowLockVersion, _
                                             inRoNum)

        End Using


        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        '実行結果を戻す
        Return result

    End Function

    ''' <summary>
    ''' 操作結果により、エラー文言を取得
    ''' </summary>
    ''' <param name="inValue">操作結果</param>
    ''' <returns>エラー文言</returns>
    ''' <remarks></remarks>
    Private Function GetErrorMessage(ByVal inValue As Long) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_Start.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim rtMessage As String = ""

        'エラーコードにより、エラータイプを分類する
        Select Case inValue

            Case ActionResult.Success
                '成功の場合
                rtMessage = ""

            Case ActionResult.RowLockVersionError
                '排他エラーの場合
                rtMessage = WebWordUtility.GetWord(MY_PROGRA_MID, 903)

            Case ActionResult.LockStallError
                rtMessage = WebWordUtility.GetWord(MY_PROGRA_MID, 903)

            Case ActionResult.DBTimeOutError
                rtMessage = WebWordUtility.GetWord(MY_PROGRA_MID, 901)

            Case ActionResult.DmsLinkageError
                rtMessage = WebWordUtility.GetWord(MY_PROGRA_MID, 904)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

            Case ActionResult.WarningOmitDmsError
                'DMS除外エラーの警告の場合
                rtMessage = WebWordUtility.GetWord(MY_PROGRA_MID, 905)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            Case Else
                'ほかの場合、予期せぬエラーを戻す
                rtMessage = WebWordUtility.GetWord(MY_PROGRA_MID, 902)

        End Select

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End. Return={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  rtMessage))

        Return rtMessage

    End Function

#End Region

#Region "洗車情報設定"
    ''' <summary>
    ''' 洗車情報を取得して設定処理を行う
    ''' </summary>
    ''' <param name="inShowFlg">処理区分</param>
    ''' <remarks></remarks>
    Private Sub SetCarWashInfo(ByVal inShowFlg As String)

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                 "{0}_Start.", _
                                 System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim staffInfo As StaffContext = StaffContext.Current

        Dim dtCarWashInfo As SC3340101DataSet.SC3340101CarWashInfoDataTable

        Dim nRownum As Long = 20

        If PAGELOAD_FLG.Equals(inShowFlg) Then

            '初期表示の場合

            '表示件数にアプリ基盤の件数値を取得
            nRownum = Me.GetNextCount()

        Else

            '上記以外 

            '表示件数に画面件数+基盤の件数(Clientから貰った)を設定
            If Not String.IsNullOrEmpty(Me.hidPostBackParamClass.Value) Then

                nRownum = CType(Me.hidPostBackParamClass.Value, Long)

            End If

        End If

        Using biz3340101 As New SC3340101BusinessLogic

            Try

                '洗車情報を取得する
                dtCarWashInfo = Me.GetCarWashInfo(staffInfo.DlrCD, staffInfo.BrnCD, nRownum)

                Me.CarWashHiddenInfo.Value = HttpUtility.HtmlEncode(biz3340101.DataTableToJson(dtCarWashInfo))

                If dtCarWashInfo.Count > 0 Then
                    '洗車情報存在の場合

                    'Repeaterにバインド
                    Me.CarWashRepeater.DataSource = dtCarWashInfo
                    Me.CarWashRepeater.DataBind()

                    Dim dateTimeNow As Date = DateTimeFunc.Now(staffInfo.DlrCD)

                    '取得した洗車表示件数をループ
                    For i = 0 To Me.CarWashRepeater.Items.Count - 1

                        'Repeater取得
                        Dim CarWashRepeaterArea As Control = Me.CarWashRepeater.Items(i)

                        'DataRow取得(洗車情報)
                        Dim drCarWash As SC3340101DataSet.SC3340101CarWashInfoRow = _
                            CType(dtCarWashInfo(i), SC3340101DataSet.SC3340101CarWashInfoRow)

                        If SVCSTATUS_CARWASHSTART.Equals(drCarWash.SVC_STATUS) Then

                            '洗車中の場合、緑に設定する
                            CType(CarWashRepeaterArea.FindControl("divHeadBox"),  _
                                System.Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("class", "HeadBox Green")

                        Else
                            '上記以外

                            '青いに設定する
                            CType(CarWashRepeaterArea.FindControl("divHeadBox"),  _
                                System.Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("class", "HeadBox Blue")

                        End If

                        '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
                        'Pマーク文言
                        CType(CarWashRepeaterArea.FindControl("PIcon"), HtmlContainerControl).InnerText = WebWordUtility.GetWord(MY_PROGRA_MID, WordIdPmark)
                        'Lマーク文言
                        CType(CarWashRepeaterArea.FindControl("LIcon"), HtmlContainerControl).InnerText = WebWordUtility.GetWord(MY_PROGRA_MID, WordIdLmark)

                        If ICON_FLAG_1.Equals(drCarWash.IMP_VCL_FLG) Then
                            'IMP_VCL_FLGが1の場合、Pアイコン表示
                            CType(CarWashRepeaterArea.FindControl("PIcon"),  _
                                System.Web.UI.HtmlControls.HtmlGenericControl).Visible = True
                            CType(CarWashRepeaterArea.FindControl("LIcon"),  _
                                    System.Web.UI.HtmlControls.HtmlGenericControl).Visible = False
                        ElseIf ICON_FLAG_2.Equals(drCarWash.IMP_VCL_FLG) Then
                            'IMP_VCL_FLGが2の場合、Lアイコン表示
                            CType(CarWashRepeaterArea.FindControl("PIcon"),  _
                                System.Web.UI.HtmlControls.HtmlGenericControl).Visible = False
                            CType(CarWashRepeaterArea.FindControl("LIcon"),  _
                                    System.Web.UI.HtmlControls.HtmlGenericControl).Visible = True
                        Else
                            'それ以外は表示しない
                            CType(CarWashRepeaterArea.FindControl("PIcon"),  _
                                System.Web.UI.HtmlControls.HtmlGenericControl).Visible = False
                            CType(CarWashRepeaterArea.FindControl("LIcon"),  _
                                    System.Web.UI.HtmlControls.HtmlGenericControl).Visible = False
                        End If
                        '2018/07/09 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

                        '店外待ちの場合、アイコン非表示
                        If Not WAIT_IN.Equals(drCarWash.PICK_DELI_TYPE) Then

                            CType(CarWashRepeaterArea.FindControl("divPickDeli"),  _
                                System.Web.UI.HtmlControls.HtmlGenericControl).Visible = False

                        End If

                        '予約客ではないの場合、アイコン非表示
                        If Not RFLG_RESERVE.Equals(drCarWash.ACCEPTANCE_TYPE) Then

                            CType(CarWashRepeaterArea.FindControl("divAcceptanceType"),  _
                                System.Web.UI.HtmlControls.HtmlGenericControl).Visible = False

                        End If

                        '遅れの設定
                        If drCarWash.SCHE_DELI_DATETIME.Date <> Date.Parse(MINDATE, CultureInfo.InvariantCulture) Then
                            '納車予定日時を設定した場合

                            If DateDiff("s", dateTimeNow, drCarWash.SCHE_DELI_DATETIME) < 0 Then

                                '実績遅れ

                                '背景色を設定
                                CType(CarWashRepeaterArea.FindControl("divDelayColor"),  _
                                    System.Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("class", "DelayState")

                                CType(CarWashRepeaterArea.FindControl("divFootBox"),  _
                                    System.Web.UI.HtmlControls.HtmlGenericControl).Attributes.Add("class", "FootBox DelayL")

                            Else

                                If Not IsDBNull(drCarWash.PLAN_DELAYDATE) _
                                    AndAlso DateDiff("s", dateTimeNow, drCarWash.PLAN_DELAYDATE) < 0 _
                                    AndAlso Date.Compare(drCarWash.PLAN_DELAYDATE, Date.Parse(MINDATE, CultureInfo.InvariantCulture)) <> 0 Then

                                    '見込み遅れ

                                    '背景色を設定
                                    CType(CarWashRepeaterArea.FindControl("lblScheDeliDate"),  _
                                        CustomLabel).CssClass = "TimeBox DelayBkColor"

                                End If

                            End If

                        End If

                    Next

                    Dim nextCount As String = WebWordUtility.GetWord(MY_PROGRA_MID, 12)
                    Dim disNextCount As String = WebWordUtility.GetWord(MY_PROGRA_MID, 13)

                    If dtCarWashInfo.Count < nRownum Then
                        '洗車件数が初期表示件数より、少ない場合

                        '次のN件ボタンを非表示
                        divCarCount.Attributes("style") = "display:none;"

                    Else
                        '上記以外

                        '洗車情報件数を取得
                        Dim nCarWashCount As Long = Me.GetCarWashCount(staffInfo.DlrCD, staffInfo.BrnCD)

                        If nCarWashCount > dtCarWashInfo.Count Then
                            '残り洗車バナー件数まだいる場合
                            '次のN件ボタンを表示
                            divCarCount.Attributes("style") = ""

                            '残り件数
                            Dim diffCount As Integer = nCarWashCount - dtCarWashInfo.Count

                            If diffCount > SYS_COUNT Then

                                diffCount = SYS_COUNT

                            End If

                            'N件を表示する
                            Dim nextCountReplace As String = nextCount.Replace("{0}", diffCount)
                            Me.CarCountHtml.InnerText = nextCountReplace
                            Me.hidPostBackParamClass.Value = diffCount + nRownum
                            Me.DisCarCountHtml.InnerText = disNextCount.Replace("{0}", diffCount)

                        Else

                            '次のN件ボタンを非表示
                            divCarCount.Attributes("style") = "display:none;"

                        End If

                    End If

                ElseIf dtCarWashInfo.Count = 0 Then
                    '0件の場合、ボタンは表示しない

                    '次のN件ボタンを非表示
                    divCarCount.Attributes("style") = "display:none;"

                    'Repeaterにバインド
                    Me.CarWashRepeater.DataSource = dtCarWashInfo
                    Me.CarWashRepeater.DataBind()

                End If

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'ORACLEのタイムアウト処理
                Me.ShowMessageBox(901)

            End Try

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}_End", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name))

        End Using

    End Sub

    ''' <summary>
    ''' 洗車バナー情報の取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inRowNum">バナー取得件数</param>
    ''' <returns>洗車情報</returns>
    ''' <remarks></remarks>
    Private Function GetCarWashInfo(ByVal inDealerCode As String, _
                                    ByVal inBranchCode As String, _
                                    ByVal inRowNum As Long) As SC3340101DataSet.SC3340101CarWashInfoDataTable

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                 "{0}_Start. inDealerCode={1}, inBranchCode={2}, inRowNum={3}", _
                                 System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                 inDealerCode, _
                                 inBranchCode, _
                                 inRowNum))

        Dim dtChipInfo As SC3340101DataSet.SC3340101CarWashInfoDataTable

        Using blSC3340101 As New SC3340101BusinessLogic
            '洗車バナー情報を取得する
            dtChipInfo = blSC3340101.GetCarWashInfo(inDealerCode, _
                                                    inBranchCode, _
                                                    inRowNum)

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return dtChipInfo

    End Function

    ''' <summary>
    ''' 洗車バナー件数の取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <returns>洗車情報件数</returns>
    ''' <remarks></remarks>
    Private Function GetCarWashCount(ByVal inDealerCode As String, _
                                     ByVal inBranchCode As String) As Long

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                 "{0}_Start.", _
                                 System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim rtCount As Long

        Using blSC3340101 As New SC3340101BusinessLogic

            '洗車バナー個数を取得する
            rtCount = blSC3340101.GetCarWashInfoCount(inDealerCode, inBranchCode)

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                              "{0}_End. Return={1}", _
                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
                              rtCount))

        Return rtCount

    End Function

#End Region

#Region "Hiddenコントロールに値設定"

    ''' <summary>
    ''' Hiddenコントロールに値設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetHiddenValue()

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                 "{0}_Start. ", _
                                 System.Reflection.MethodBase.GetCurrentMethod.Name))

        If String.IsNullOrEmpty(Me.HeadTitleHidden.Value) Then

            'ヘッダー文言
            Me.HeadTitleHidden.Value = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRA_MID, 1))

        End If


        If String.IsNullOrEmpty(Me.ServerTimeHidden.Value) Then

            '更新日時は全てこの値を使用する
            Dim staffInfo As StaffContext = StaffContext.Current

            'サーバの時間
            Me.ServerTimeHidden.Value = _
                DateTimeFunc.Now(staffInfo.DlrCD).ToString(CultureInfo.CurrentCulture)

        End If

        'MM/ddとHH:mmのデータフォーマットを取得する
        If String.IsNullOrEmpty(Me.hidDateFormatMMdd.Value) Then

            Me.hidDateFormatMMdd.Value = DateTimeFunc.GetDateFormat(11)

        End If

        If String.IsNullOrEmpty(Me.hidDateFormatHHmm.Value) Then

            Me.hidDateFormatHHmm.Value = DateTimeFunc.GetDateFormat(14)

        End If

        'システム設定から画面自動リフレッシュ時間単位を取得する
        Dim systemEnv As New SystemEnvSetting

        'N秒はシステム設定値(TBL_SYSTEMENVSETTING. SC3340101_REFRESH_INTERVAL)
        If REFRESH_TIME = 0 Then

            REFRESH_TIME = _
                CType(systemEnv.GetSystemEnvSetting(REFRESH_INTERVAL).PARAMVALUE, Long)

        End If

        'N件はシステム設定値(TBL_SYSTEMENVSETTING. SC3340101_DEFAULT_READ_COUNT)を取得
        If SYS_COUNT = 0 Then

            SYS_COUNT = _
                CType(systemEnv.GetSystemEnvSetting(READ_COUNT).PARAMVALUE, Long)

        End If

        Me.RefreshTimeHidden.Value = REFRESH_TIME

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' N件の取得
    ''' </summary>
    ''' <returns>N件</returns>
    ''' <remarks></remarks>
    Private Function GetNextCount() As Long

        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                 "{0}_Start. ", _
                                 System.Reflection.MethodBase.GetCurrentMethod.Name))

        If SYS_COUNT = 0 Then
            '取得してない場合

            Dim systemEnv As New SystemEnvSetting

            'N件はシステム設定値(TBL_SYSTEMENVSETTING. SC3340101_DEFAULT_READ_COUNT)を取得
            SYS_COUNT = _
                CType(systemEnv.GetSystemEnvSetting(READ_COUNT).PARAMVALUE, Long)

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_End Return={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  SYS_COUNT))

        Return SYS_COUNT

    End Function

#End Region

End Class
