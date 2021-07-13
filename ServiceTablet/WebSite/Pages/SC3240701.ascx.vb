'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240701.ascx.vb
'─────────────────────────────────────
'機能： ストール使用不可設定
'補足： 
'作成： 2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
'更新： 
'─────────────────────────────────────
Imports System.Globalization
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.iCROP.DataAccess.SC3240701
Imports Toyota.eCRB.iCROP.BizLogic.SC3240701
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet

Partial Class Pages_SC3240701
    Inherits System.Web.UI.UserControl
    Implements ICallbackEventHandler

#Region "定数"

    ''' <summary>
    ''' 自画面のプログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UNAVAILABLE_PROGRAMID As String = "SC3240701"

    ''' <summary>
    ''' コールバック時に画面を作成する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_DISPCREATE As String = "UnavailableChip"

    ''' <summary>
    ''' コールバック時に登録処理をする
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_REGISTER As String = "RegisterUnavailableSetting"


#End Region


#Region "列挙体"
    ''' <summary>
    ''' 列挙体 コールバック結果コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum ResultCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        ''' <remarks></remarks>
        Success = 0

        ''' <summary>
        ''' 入力項目値チェックエラー
        ''' </summary>
        ''' <remarks></remarks>
        CheckError = 1

        ''' <summary>
        ''' DBアウトエラー
        ''' </summary>
        ''' <remarks></remarks>
        DBTimeOut = 2

        ''' <summary>
        ''' 予期せぬエラー
        ''' </summary>
        ''' <remarks></remarks>
        Failure = 3

        ''' <summary>
        ''' 非稼働日時の大小エラー
        ''' </summary>
        ''' <remarks></remarks>
        DateCheckError = 4

        ''' <summary>
        ''' 他予約チップとの重複エラー
        ''' </summary>
        ''' <remarks></remarks>
        CollisionError = 5

        ''' <summary>
        ''' 行ロックバージョンエラー
        ''' </summary>
        ''' <remarks></remarks>
        RowLockVersionError = 7

        ''' <summary>
        ''' チップが他ユーザーに削除されていたエラー
        ''' </summary>
        ''' <remarks></remarks>
        OtherDeleteError = 8


    End Enum

    ''' <summary>
    ''' 列挙体 エラー文言ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum ErrorCode

        ''' <summary>
        ''' DBタイムアウトエラー
        ''' </summary>
        ''' <remarks></remarks>
        DBTimeOut = 901

        ''' <summary>
        ''' 予期せぬエラー
        ''' </summary>
        ''' <remarks></remarks>
        Exception = 902

        ''' <summary>
        ''' 他チップとの重複エラー
        ''' </summary>
        ''' <remarks></remarks>
        CollisionError = 904

        ''' <summary>
        ''' 行ロックバージョンエラー
        ''' </summary>
        ''' <remarks></remarks>
        RowLockVersionError = 905

        ''' <summary>
        ''' データ存在チェックエラー
        ''' </summary>
        ''' <remarks></remarks>
        OtherDeleteError = 906

    End Enum
#End Region

#Region "メンバ変数"

    ''' <summary>
    ''' コールバックメソッドの呼び出し元に返却する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private callBackResult As String

    ''' <summary>
    ''' ユーザ情報（セッションより）
    ''' </summary>
    ''' <remarks></remarks>
    Private objStaffContext As StaffContext

#End Region

#Region "ページイベント"
    ''' <summary>
    ''' 画面ロードの処理を実施
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'ユーザ情報の取得.
        objStaffContext = StaffContext.Current

        'コールバックスクリプトの生成()
        ScriptManager.RegisterStartupScript(
            Me,
            Me.GetType(),
            "gCallbackSC3240701",
            String.Format(CultureInfo.InvariantCulture,
                          "gCallbackSC3240701.beginCallback = function () {{ {0}; }};",
                          Page.ClientScript.GetCallbackEventReference(Me, "gCallbackSC3240701.packedArgument", _
                                                                      "gCallbackSC3240701.endCallback", "", True)
                          ),
            True
        )

        '固定文言を設定
        Me.SetUnavailableHeaderWord()
        Me.SetUnavailableWord()

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub
#End Region

#Region "プライベートメソッド"

    ''' <summary>
    ''' 使用不可画面の固定文言設定する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetUnavailableWord()

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        '開始日時ラベル
        Me.StartDateTimeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, 4))

        '終了日時ラベル
        Me.FinishDateTimeWordLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, 5))

        'メモラベル
        Me.IdleMemoLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, 6))

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' ヘッダーエリアの固定文言を設定する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetUnavailableHeaderWord()

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

        'タイトル
        Me.UnavailableHeaderLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, 1))

        'キャンセルボタン
        Me.UnavailableCancelBtn.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, 2))

        '登録ボタン
        Me.UnavailableRegisterBtn.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, 3))

        OutputInfoLog(MethodBase.GetCurrentMethod.Name, False)

    End Sub

    ''' <summary>
    ''' 初期表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetUnavailableSettingInitData(ByVal startIdleTime As Date, ByVal finishIdleTime As Date, ByVal idleMemo As String)

        '作業開始予定日時
        Me.StartIdleDateTimeLabel.Text = DateTimeFunc.FormatDate(2, startIdleTime)
        Me.StartIdleDateTimeSelector.Value = startIdleTime

        '作業終了予定日時
        Me.FinishIdleDateTimeLabel.Text = DateTimeFunc.FormatDate(2, finishIdleTime)
        Me.FinishIdleDateTimeSelector.Value = finishIdleTime

        'メモ
        Me.IdleMemoTxt.Text = idleMemo

    End Sub
#End Region


#Region "コールバック"
    ''' <summary>
    ''' コールバック用文字列を返却
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

        Return Me.callBackResult
    End Function

    ''' <summary>
    ''' コールバックイベント時のハンドリング
    ''' </summary>
    ''' <param name="eventArgument">クライアントから渡されるJSON形式のパラメータ</param>
    ''' <remarks></remarks>
    Public Sub RaiseCallbackEvent(ByVal eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent

        'コールバック返却用内部クラスのインスタンスを生成
        Dim result As New CallBackResultClass
        Dim serializer As New JavaScriptSerializer

        Try
            'コールバック引数用内部クラス
            Dim argument As UnavailableCallBackArgumentClass

            'JSON形式の引数を内部クラス型に変換して受け取る
            argument = serializer.Deserialize(Of UnavailableCallBackArgumentClass)(eventArgument)

            If argument.Method.Equals(CBACK_DISPCREATE) Then
                '******************************
                '* 初期表示で画面の作成
                '******************************

                'コールバック呼び出し元に返却する文字列
                Dim resultString As String

                Try
                    'ストール非稼働IDが0の場合(新規作成)
                    If argument.StallIdleId = 0 Then

                        '初期表示
                        Me.SetUnavailableSettingInitData(argument.StartIdleTime, argument.FinishIdleTime, Nothing)

                        'クライアントへの返却用クラスに値を設定
                        '成功
                        result.ResultCode = ResultCode.Success
                        result.Message = String.Empty

                    ElseIf 0 < argument.StallIdleId Then
                        'ストール非稼働IDが0より大きい場合(更新)

                        '初期表示用データセット取得
                        Dim IdleInfo = Nothing

                        Using biz As New SC3240701BusinessLogic()
                            IdleInfo = biz.GetInitInfo(argument.StallIdleId)
                        End Using

                        '初期表示用のデータが存在しない
                        If IsNothing(IdleInfo) Then

                            resultString = Nothing
                            'クライアントへの返却用クラスに値を設定
                            'チップが他ユーザーに削除されていた
                            result.ResultCode = ResultCode.OtherDeleteError
                            result.Message = WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, ErrorCode.OtherDeleteError)
                        Else
                            Dim chipInfo As SC3240701DataSet.StallIdleInfoRow = DirectCast(IdleInfo.Rows(0), SC3240701DataSet.StallIdleInfoRow)

                            '初期表示
                            Me.SetUnavailableSettingInitData(chipInfo.IDLE_START_DATETIME, chipInfo.IDLE_END_DATETIME, chipInfo.IDLE_MEMO.Trim())

                            'クライアントへの返却用クラスに値を設定
                            '成功
                            result.ResultCode = ResultCode.Success
                            result.Message = String.Empty
                        End If
                    Else
                        'それ以外の場合
                        resultString = Nothing
                        'クライアントへの返却用クラスに値を設定
                        '予期せぬエラー
                        result.ResultCode = ResultCode.Failure
                        result.Message = WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, ErrorCode.Exception)

                    End If
                Catch ex As OracleExceptionEx When ex.Number = 1013
                    'DBタイムアウトエラー
                    result.ResultCode = ResultCode.DBTimeOut
                    result.Message = WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, ErrorCode.DBTimeOut)
                    resultString = Nothing

                End Try

                '作成した初期表示画面のHTMLを返却用文字列に設定
                Using sw As New System.IO.StringWriter(CultureInfo.InvariantCulture)

                    Dim writer As HtmlTextWriter = New HtmlTextWriter(sw)
                    Me.RenderControl(writer)
                    resultString = sw.GetStringBuilder().ToString
                End Using

                'クライアントへの返却用クラスに値を設定
                result.Caller = CBACK_DISPCREATE
                result.Contents = HttpUtility.HtmlEncode(resultString)

            ElseIf argument.Method.Equals(CBACK_REGISTER) Then
                '******************************
                '* 登録ボタン押下
                '******************************

                'チェック結果でエラーがあるとき
                If argument.ValidateCode <> 0 Then

                    'クライアントへの返却用クラスに値を設定
                    result.ResultCode = ResultCode.CheckError
                    result.Message = WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, argument.ValidateCode)
                Else

                    '現在日時取得
                    Dim dtNow As Date = DateTimeFunc.Now(objStaffContext.DlrCD)
                    'ローカル変数．ストール非稼働ID
                    Dim stallIdleId = 0

                    If argument.StallIdleId = 0 Then
                        '******************************
                        '* 新規作成
                        '******************************

                        '新規作成結果
                        Dim createResult = 0
                        Using bl As SC3240701BusinessLogic = New SC3240701BusinessLogic()

                            '使用不可チップ作成
                            '結果格納
                            createResult = bl.CreateUnavailableChip(argument.StallId, argument.StartIdleTime, argument.FinishIdleTime, argument.IdleMemo, dtNow, objStaffContext, stallIdleId)
                        End Using

                        If createResult = ErrorCode.DBTimeOut Then
                            'DBタイムアウトエラー
                            result.Contents = String.Empty
                            result.ResultCode = ResultCode.DBTimeOut
                            result.Message = WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, createResult)
                            result.UnavailableChipJson = String.Empty

                        ElseIf createResult = ErrorCode.CollisionError Then
                            '他予約チップとの重複
                            result.Contents = String.Empty
                            result.ResultCode = ResultCode.CollisionError
                            result.Message = WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, createResult)
                            result.UnavailableChipJson = String.Empty

                        ElseIf createResult = ErrorCode.Exception Then
                            '予期せぬエラー
                            result.Contents = String.Empty
                            result.ResultCode = ResultCode.Failure
                            result.Message = WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, createResult)
                            result.UnavailableChipJson = String.Empty

                        End If

                    ElseIf 0 < argument.StallIdleId Then
                        '******************************
                        '* 更新
                        '******************************

                        '更新結果
                        Dim updateResult = 0
                        Using bl As SC3240701BusinessLogic = New SC3240701BusinessLogic()

                            '更新処理
                            updateResult = bl.UpdateUnavailableChip(argument.StallIdleId, argument.StallId, argument.StartIdleTime, argument.FinishIdleTime, argument.IdleMemo, dtNow, objStaffContext, argument.RowLockVersion)

                            If updateResult = 0 Then
                                '結果が成功だった場合
                                stallIdleId = argument.StallIdleId

                            ElseIf updateResult = ErrorCode.DBTimeOut Then
                                'DBタイムアウトエラー
                                result.Contents = String.Empty
                                result.ResultCode = ResultCode.DBTimeOut
                                result.Message = WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, updateResult)
                                result.UnavailableChipJson = String.Empty

                            ElseIf updateResult = ErrorCode.CollisionError Then
                                '他予約チップとの重複
                                result.Contents = String.Empty
                                result.ResultCode = ResultCode.CollisionError
                                result.Message = WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, updateResult)
                                result.UnavailableChipJson = String.Empty

                            ElseIf updateResult = ErrorCode.RowLockVersionError Then
                                '行ロックバージョンエラー
                                result.Contents = String.Empty
                                result.ResultCode = ResultCode.RowLockVersionError
                                result.Message = WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, updateResult)
                                result.UnavailableChipJson = String.Empty

                            ElseIf updateResult = ErrorCode.Exception Then
                                '予期せぬエラー
                                result.Contents = String.Empty
                                result.ResultCode = ResultCode.Failure
                                result.Message = WebWordUtility.GetWord(UNAVAILABLE_PROGRAMID, updateResult)
                                result.UnavailableChipJson = String.Empty

                            End If
                        End Using
                    End If

                    '使用不可チップ情報
                    Dim UnavailableChipInfo = Nothing

                    '新規作成処理または更新処理が成功した場合
                    '（ストール非稼働IDは0より大きい値が入る）
                    If 0 < stallIdleId Then

                        Dim callBackContent = Nothing

                        '登録したストール使用不可情報をDBより取得
                        Using biz As New SC3240701BusinessLogic()
                            callBackContent = biz.GetInitInfo(stallIdleId)
                        End Using

                        'DataTableをJSON形式に変換
                        Using bl As New SC3240701BusinessLogic
                            UnavailableChipInfo = HttpUtility.HtmlEncode(bl.DataTableToJson(callBackContent))
                        End Using

                        result.Contents = String.Empty
                        result.ResultCode = ResultCode.Success
                        result.Message = String.Empty
                        result.UnavailableChipJson = UnavailableChipInfo
                    End If
                End If
            End If
            '処理結果をコールバック返却用文字列に設定
            Me.callBackResult = serializer.Serialize(result)

        Finally
            serializer = Nothing
            result = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' コールバック結果をクライアントに返すための内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackResultClass

        Private _caller As String
        Private _resultCode As Short
        Private _message As String
        Private _contents As String
        Private _unavailableChipJson As String

        ''' <summary>
        ''' 呼び出し元メソッド(JavaScript側)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Caller() As String
            Get
                Return _caller
            End Get
            Set(ByVal value As String)
                _caller = value
            End Set
        End Property

        ''' <summary>
        ''' 処理結果コード
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ResultCode() As Short
            Get
                Return _resultCode
            End Get
            Set(ByVal value As Short)
                _resultCode = value
            End Set
        End Property

        ''' <summary>
        ''' メッセージ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Message() As String
            Get
                Return _message
            End Get
            Set(ByVal value As String)
                _message = value
                '_message = value
            End Set
        End Property

        ''' <summary>
        ''' HTMLコンテンツ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Contents() As String
            Get
                Return _contents
            End Get
            Set(ByVal value As String)
                _contents = value
            End Set
        End Property

        ''' <summary>
        ''' JSON形式のデータ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UnavailableChipJson() As String
            Get
                Return _unavailableChipJson
            End Get
            Set(ByVal value As String)
                _unavailableChipJson = value
            End Set
        End Property
    End Class
#End Region

#Region "ログ出力メソッド"

    ''' <summary>
    ''' 引数のないInfoレベルのログを出力する
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="isStart">True:Startログ/False:Endログ</param>
    ''' <remarks></remarks>
    Private Sub OutputInfoLog(ByVal method As String, ByVal isStart As Boolean)

        If isStart Then
            Logger.Info(UNAVAILABLE_PROGRAMID & ".ascx " & method & "_Start")
        Else
            Logger.Info(UNAVAILABLE_PROGRAMID & ".ascx " & method & "_End")
        End If

    End Sub

    ''' <summary>
    ''' 引数のあるInfoレベルのログを出力する
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="isStart">True:Startログ/False:Endログ</param>
    ''' <param name="argString">フォーマット用文字列</param>
    ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ''' <remarks></remarks>
    Private Sub OutputInfoLog(ByVal method As String, ByVal isStart As Boolean, ByVal argString As String, ByVal ParamArray args() As Object)

        Dim logString As String = String.Empty

        If isStart Then
            logString = UNAVAILABLE_PROGRAMID & ".ascx " & method & "_Start" & argString
            Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))
        Else
            logString = UNAVAILABLE_PROGRAMID & ".ascx " & method & "_End" & argString
            Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))
        End If

    End Sub

    ''' <summary>
    ''' 引数のあるInfoレベルのログを出力する
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="argString">フォーマット用文字列</param>
    ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ''' <remarks></remarks>
    Private Sub OutputWarnLog(ByVal method As String, ByVal argString As String, ByVal ParamArray args() As Object)

        Dim logString As String = String.Empty

        logString = UNAVAILABLE_PROGRAMID & ".ascx " & method & argString
        Logger.Warn(String.Format(CultureInfo.InvariantCulture, logString, args))

    End Sub

    ''' <summary>
    ''' エラーログを出力する
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="argString">フォーマット用文字列</param>
    ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ''' <remarks></remarks>
    Private Sub OutputErrLog(ByVal method As String, ByVal argString As String, ByVal ParamArray args() As Object)

        Dim logString As String = String.Empty

        logString = UNAVAILABLE_PROGRAMID & ".ascx " & method & "_Error" & argString
        Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))

    End Sub

    ''' <summary>
    ''' エラーログを出力する ※例外オブジェクトあり
    ''' </summary>
    ''' <param name="method">メソッド名</param>
    ''' <param name="ex">例外オブジェクト</param>
    ''' <param name="argString">フォーマット用文字列</param>
    ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
    ''' <remarks></remarks>
    Private Sub OutputErrExLog(ByVal method As String, ByVal ex As Exception, ByVal argString As String, ByVal ParamArray args() As Object)

        Dim logString As String = String.Empty

        logString = UNAVAILABLE_PROGRAMID & ".ascx " & method & "_Error" & argString
        Logger.Error(String.Format(CultureInfo.InvariantCulture, logString, args), ex)

    End Sub
#End Region
End Class
