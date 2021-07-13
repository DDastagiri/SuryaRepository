Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Web.Script.Serialization
Imports System.Globalization
Imports Toyota.eCRB.AssistantSA.MainMenu.BizLogic
Imports Toyota.eCRB.AssistantSA.MainMenu.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Partial Class Pages_SC3320101
    Inherits BasePage
    Implements ICallbackEventHandler


#Region "メンバ変数"
    ''' <summary>
    ''' コールバックメソッドの呼び出し元に返却する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private returnDataJson As String
#End Region

#Region "定数"
    ''' <summary>
    ''' プログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMID As String = "SC3320101"

    ''' <summary>
    ''' CallBack処理名： "Refresh"
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Method_Refresh As String = "Refresh"

    ''' <summary>
    ''' CallBack処理名： "UpDate"
    ''' </summary>
    ''' <remarks></remarks>
    Private Const Method_UpDate As String = "UpDate"



    ''' <summary>
    ''' 処理結コード： [0:成功]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUCCESAS As Integer = 0

    ''' <summary>
    ''' 処理結果コード： [-1:予期せぬエラー]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERR_CODE_UNEXPECTED As Integer = -1

    ''' <summary>
    ''' 処理結果コード： [1:禁止文字が入力されているエラー]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERR_CODE_VALIDATION As Integer = 1

    ''' <summary>
    ''' 処理結果コード： [2:ロケーションが変更されていない時に登録ボタンを押された]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERR_CODE_NOTCHANGE As Integer = 2

    ''' <summary>
    ''' 処理結果コード： [3:OracleのTimeoutエラーが発生した場合]
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERR_CODE_TIMEOUT As Integer = 3

    ''' <summary>
    ''' 画面自動リフレッシュのシステム設定名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AssistantSARefreshInterval As String = "ASA_REFRESH_INTERVAL"

#End Region

#Region "イベント処理メソッド"
    ''' <summary>
    ''' 画面ロードの処理を実施
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        'コールバックスクリプトの生成
        ScriptManager.RegisterStartupScript(
            Me,
            Me.GetType(),
            "gCallbackSC3320101",
            String.Format(CultureInfo.InvariantCulture,
                          "gCallbackSC3320101.beginCallback = function () {{ {0}; }};",
                          Page.ClientScript.GetCallbackEventReference(Me, "gCallbackSC3320101.packedArgument", _
                                                                      "gCallbackSC3320101.endCallback", "", True)
                          ),
            True
        )


        If Not Me.Page.IsCallback Then

            'Hiddenコントロールの設定
            Me.SetHiddenValue()

        End If
    End Sub
#End Region

    ''' <summary>
    ''' CallBack終わり、クライアントに返すJsonデータ
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult
        Return Me.returnDataJson
    End Function

    ''' <summary>
    ''' CallBackメイン関数
    ''' </summary>
    ''' <param name="eventArgument">クライアントから渡されたパラメータ</param>
    ''' <remarks></remarks>
    Private Sub RaiseCallbackEvent(ByVal eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent
        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S. eventArgument={1}" _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  eventArgument))

        Dim serializer = New JavaScriptSerializer
        '返却クラス
        Dim result As New CallBackResultClass
        '処理結果コード
        Dim resultCode As Integer
        '最新のHTMLコンテンツ
        Dim displayContents As String
        'JSON形式の引数を内部クラス型に変換して受け取る
        Dim argument As New List(Of CallBackArgumentClass)
        argument = serializer.Deserialize(Of List(Of CallBackArgumentClass))(eventArgument)

        Try
            'メッソド名がリフレッシュ以外の場合は更新モード
            If Not Method_Refresh.Equals(argument(0).MethodName) Then
                '更新データリストをループして更新
                For Each visitInfo As CallBackArgumentClass In argument

                    '更新前の禁止文字チェック
                    If Not Validation.IsValidString(visitInfo.ParkingCode) Then
                        'エラーの場合はループ空離脱、エラーメッセージを取得する
                        Dim errMessage As String = GetErrorMessage(ERR_CODE_VALIDATION)
                        '禁止文字が入力された車両登録番号をエラーメッセージに表示する
                        errMessage = errMessage.Replace("{0}", visitInfo.RegNum)
                        result.Message = HttpUtility.HtmlEncode(errMessage)
                        '結果コードを 1:不正文字エラーに設定する
                        result.ResultCode = ERR_CODE_VALIDATION
                        '返却変数にシリアル化された JSON 文字列を設定
                        Me.returnDataJson = serializer.Serialize(result)
                        'エラーログ出力
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} RETURNCODE = {2} " _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , ERR_CODE_VALIDATION))
                        Exit Sub
                    End If

                Next

                '更新処理を行う
                resultCode = Me.UpdLocationCode(argument)
            End If

            result.ResultCode = resultCode

            '画面の最新情報を取得
            displayContents = Me.GetMyDisplayCreateData()
            result.Contents = HttpUtility.HtmlEncode(displayContents)

            '更新成功の場合メッセージに空文字を設定
            If SUCCESAS.Equals(resultCode) Then
                result.Message = String.Empty

                'ログ出力
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_E. RETURNCODE = {1}" _
                                          , System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                          SUCCESAS))
            Else
                'エラーの場合はエラーメッセージを取得する
                result.Message = HttpUtility.HtmlEncode(GetErrorMessage(resultCode))

                'エラーログ出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} RETURNCODE = {2} " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , resultCode))
            End If

            '返却変数にシリアル化された JSON 文字列を設定
            Me.returnDataJson = serializer.Serialize(result)

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'DBタイムアウトの場合
            result.ResultCode = ERR_CODE_TIMEOUT
            '文言を設定する
            result.Message = HttpUtility.HtmlEncode(GetErrorMessage(ERR_CODE_TIMEOUT))
            '画面の最新情報を取得
            displayContents = Me.GetMyDisplayCreateData()
            result.Contents = HttpUtility.HtmlEncode(displayContents)
            '返却変数にシリアル化された JSON 文字列を設定
            Me.returnDataJson = serializer.Serialize(result)
            'エラーログ出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} RETURNCODE = {2} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ERR_CODE_TIMEOUT))
        Catch ex As Exception
            '予期せぬエラーの場合
            result.ResultCode = ERR_CODE_UNEXPECTED
            '文言を設定する
            result.Message = HttpUtility.HtmlEncode(GetErrorMessage(ERR_CODE_UNEXPECTED))
            '画面の最新情報を取得
            displayContents = Me.GetMyDisplayCreateData()
            result.Contents = HttpUtility.HtmlEncode(displayContents)
            '返却変数にシリアル化された JSON 文字列を設定
            Me.returnDataJson = serializer.Serialize(result)
            'エラーログ出力
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} RETURNCODE = {2} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ERR_CODE_UNEXPECTED))
        Finally
            serializer = Nothing
            argument = Nothing
        End Try
    End Sub

#Region "コールバック用内部クラス"
    ''' <summary>
    ''' コールバック用引数の内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackArgumentClass

        ''' <summary>
        ''' 来店シーケンス
        ''' </summary>
        ''' <remarks></remarks>
        Public Property VisitSeq As Long
        ''' <summary>
        ''' ロケーションコード
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ParkingCode As String

        ''' <summary>
        ''' CallBack処理名
        ''' </summary>
        ''' <remarks></remarks>
        Public Property MethodName As String

        ''' <summary>
        ''' 車両登録番号
        ''' </summary>
        ''' <remarks></remarks>
        Public Property RegNum As String

    End Class
#End Region

#Region "Privateメッソド"
    ''' <summary>
    ''' 画面を作成するために必要な情報を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetMyDisplayCreateData() As String
        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S." _
                                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim SC3320101bl As New SC3320101BusinessLogic
        Try
            '初期表示用データセットを取得
            Dim InitDs As SC3320101DataSet.SC3320101VisitInfoDataTable = SC3320101bl.GetInitInfoForDisplay()

            'チップ詳細(小)(大)にそれぞれ表示用データを設定
            If InitDs.Count > 0 Then
                Me.VisitServiceInfoRepeater.DataSource = InitDs
                Me.VisitServiceInfoRepeater.DataBind()
            Else
                Me.VisitServiceInfoRepeater.DataSource = Nothing
                Me.VisitServiceInfoRepeater.DataBind()
            End If

            '上記で作成した画面のHTMLを返却する
            Using sw As New System.IO.StringWriter(CultureInfo.InvariantCulture)

                Dim writer As HtmlTextWriter = New HtmlTextWriter(sw)
                Me.Page.Master.FindControl("content").FindControl("MM_Main_Contents").RenderControl(writer)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_E." _
                                         , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return sw.GetStringBuilder().ToString
            End Using

        Finally
            SC3320101bl = Nothing
        End Try

    End Function


    ''' <summary>
    ''' ロケーション番号を更新
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdLocationCode(ByVal arg As List(Of CallBackArgumentClass)) As Integer
        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))

        If arg.Count > 0 Then
            Dim SC3320101bl As New SC3320101BusinessLogic
            Dim dtVisitInfo As New SC3320101DataSet.SC3320101VisitInfoDataTable

            '返却コード
            Dim retCode As Integer
            'スタフ情報
            Dim staffInfo As StaffContext = StaffContext.Current
            '現在時間
            Dim nowDateTime As Date = DateTimeFunc.Now(staffInfo.DlrCD)

            Try
                'ループでリストのデータを更新する
                For Each visitInfo As CallBackArgumentClass In arg
                    '更新用のデータ行を作成
                    Dim drVisitInfo As SC3320101DataSet.SC3320101VisitInfoRow _
                        = dtVisitInfo.NewSC3320101VisitInfoRow
                    drVisitInfo.VISITSEQ = visitInfo.VisitSeq
                    drVisitInfo.PARKINGCODE = visitInfo.ParkingCode
                    'ロケーション番号を更新する
                    retCode = SC3320101bl.UpdLocationInfo(drVisitInfo, _
                                                          nowDateTime, _
                                                          staffInfo.Account)
                Next

                'ログ出力
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}_E. retCode={1}", _
                                        System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                        retCode))
                Return retCode

            Finally
                SC3320101bl = Nothing
                dtVisitInfo = Nothing
            End Try
        Else
            'ロケーションが変更されていない時に登録ボタンを押された
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} RETURNCODE = {2} " _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , ERR_CODE_VALIDATION))
            Return ERR_CODE_NOTCHANGE

        End If

    End Function

    ''' <summary>
    ''' 操作結果により、エラー文言を取得
    ''' </summary>
    ''' <param name="inValue">結果コード</param>
    ''' <returns>エラー文言</returns>
    ''' <remarks></remarks>
    Private Function GetErrorMessage(ByVal inValue As Integer) As String
        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim rtMessage As String = ""
        'エラーコードにより、エラータイプを分類する
        Select Case inValue
            Case ERR_CODE_VALIDATION
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 904)
            Case ERR_CODE_NOTCHANGE
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 905)
            Case ERR_CODE_UNEXPECTED
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 902)
            Case ERR_CODE_TIMEOUT
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 901)
        End Select

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
               , "{0}.{1}" _
               , Me.GetType.ToString _
               , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return rtMessage
    End Function

    ''' <summary>
    ''' Hiddenコントロールに値設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetHiddenValue()
        'ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S." _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '更新日時は全てこの値を使用する
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim updDate As String = DateTimeFunc.Now(staffInfo.DlrCD).ToString

        'ヘッダー文言
        Me.HeadTitleHidden.Value = WebWordUtility.GetWord(MY_PROGRAMID, 1)
        ''ロケーションが変更されていない時のエラー文言
        'Me.NotChangeErrMsgHidden.Value = WebWordUtility.GetWord(MY_PROGRAMID, 905)
        ''-RFIDを読取った時にテキストが選択されていないの文言
        'Me.NotSelectedErrMsgHidden.Value = WebWordUtility.GetWord(MY_PROGRAMID, 906)
        'サーバの時間
        Me.ServerTimeHidden.Value = updDate

        'システム設定から画面自動リフレッシュ時間単位を取得する
        Dim systemEnv As New SystemEnvSetting
        Dim refreshTimeInterval As String = _
            systemEnv.GetSystemEnvSetting(AssistantSARefreshInterval).PARAMVALUE

        'MM/ddとHH:mmのデータフォーマットを取得する
        Me.hidDateFormatMMdd.Value = DateTimeFunc.GetDateFormat(11)
        Me.hidDateFormatHHmm.Value = DateTimeFunc.GetDateFormat(14)

        '取れなかった場合は60を設定する
        If String.IsNullOrEmpty(refreshTimeInterval) Then
            Me.RefureshTimeHidden.Value = "60"
        Else
            Me.RefureshTimeHidden.Value = refreshTimeInterval
        End If

        'ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                   , "{0}.{1}" _
                                   , Me.GetType.ToString _
                                   , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub
#End Region

#Region "クライアントに返すクラス"
    ''' <summary>
    ''' コールバック結果をクライアントに返すための内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackResultClass

        ''' <summary>
        ''' 処理結果コード
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ResultCode As Long

        ''' <summary>
        ''' コンテンツ
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Contents As String

        ''' <summary>
        ''' メッセージ
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Message As String

        ''' <summary>
        ''' 来店シーケンス
        ''' </summary>
        ''' <remarks></remarks>
        Public Property VisitSeq As Long

        ''' <summary>
        ''' ロケーションコード
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ParkingCode As String


    End Class

#End Region

End Class
