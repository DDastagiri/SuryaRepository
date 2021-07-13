Imports System.Globalization
Imports System.Runtime.Serialization.Json
Imports System.IO
Imports System.Text
Imports System.Threading
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Reflection.MethodBase

''' <summary>
''' JSONファイル操作共通処理
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class JsonUtilCommon

#Region " コンストラクタ "
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        REM
    End Sub
#End Region

#Region " 定数 "
    ''' <summary>JSONファイルパス置き換え文字</summary>
    Public Const ReplaceFileString As String = "[Series]"

    ''' <summary>JSONファイルパス置き換え文字</summary>
    Public Const ReplaceFileString2 As String = "[Dealer]"

    ''' <summary> メッセージID </summary>
    Private Const OPEN_FAILED_IO As String = "900"
    ''' <summary>JSONファイルパス置き換え文字</summary>
    Private Const WRITE_FAILED_COMPETITION As String = "900"

    ''' <summary>エスケープ文字削除前の値</summary>
    Private Const WRITE_ESCAPE_BEFORE As String = "\/"
    ''' <summary>エスケープ文字削除後の値</summary>
    Private Const WRITE_ESCAPE_AFTER As String = "/"

    ''' <summary>更新日時比較時に使用するタイムスタンプの書式</summary>
    Public Const TimeStampFormat As String = "yyyyMMddHHmmssfff"

    ''' <summary>読み込みリトライ回数</summary>
    Private Const READ_RETRY_COUNT As Integer = 3
    ''' <summary>読み込みリトライ待機時間(ミリ秒)</summary>
    Private Const READ_RETRY_INTERVAL As Integer = 1000
#End Region

#Region " メソッド "
    ''' <summary>
    ''' JSONファイルのパスを取得します。
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="jsonPath">JSONファイルパス</param>
    ''' <param name="carID">車種ID</param>
    ''' <returns>JSONファイルパス</returns>
    ''' <remarks></remarks>
    Public Shared Function GetJsonFilePath(
        ByVal tcvPath As String,
        ByVal jsonPath As String,
        ByVal carId As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("jsonPath", jsonPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))

        'ファイル名取得
        Dim filePath As String = Path.Combine(tcvPath, jsonPath)

        '車種ID置き換え
        filePath = filePath.Replace(JsonUtilCommon.ReplaceFileString, carId)


        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(filePath))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return filePath

    End Function

    ''' <summary>
    ''' JSONファイルのパスを取得します。
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="jsonPath">JSONファイルパス</param>
    ''' <param name="carID">車種ID</param>
    ''' <param name="dealerCd">販売店コード</param>
    ''' <returns>JSONファイルパス</returns>
    ''' <remarks></remarks>
    Public Shared Function GetJsonFilePath(
        ByVal tcvPath As String,
        ByVal jsonPath As String,
        ByVal carId As String,
        ByVal dealerCD As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("jsonPath", jsonPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCD, True))

        'ファイル名取得
        Dim filePath As String = Path.Combine(tcvPath, jsonPath)

        '車種ID置き換え
        filePath = filePath.Replace(JsonUtilCommon.ReplaceFileString, carId)

        '販売店コード置き換え
        filePath = filePath.Replace(JsonUtilCommon.ReplaceFileString2, dealerCD)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(filePath))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return filePath

    End Function

    ''' <summary>
    ''' ファイルを読み込み内容を取得します。
    ''' </summary>
    ''' <param name="file">ファイルパス</param>
    ''' <returns>ファイルの内容</returns>
    ''' <remarks></remarks>
    Public Shared Function GetValue(
        ByVal file As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("file", file, False))

        Dim value As String = String.Empty

        '読み込みに失敗した場合、所定の回数リトライする
        For i As Integer = 0 To READ_RETRY_COUNT
            Try
                'ファイル読み込み
                Using stream As New StreamReader(file)
                    value = stream.ReadToEnd
                End Using

                '完了したら抜ける
                Exit For

            Catch ex As IOException
                '読み込みに失敗
                If i = READ_RETRY_COUNT Then
                    'リトライ回数が規定値に達した場合は例外をスロー
                    Logger.Error("Could not open file for read.")
                    Throw
                Else
                    'リトライ回数が規定値に達するまではリトライ
                    Logger.Warn("Could not open file for read. Retry=" & i + 1)
                    Thread.Sleep(READ_RETRY_INTERVAL)
                End If
            End Try
        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(value))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return value

    End Function

    ''' <summary>
    ''' ファイルを書き込みます。(更新日付のチェックを行う)
    ''' </summary>
    ''' <param name="file">ファイルパス</param>
    ''' <param name="value">書き込む内容</param>
    ''' <param name="timeStamp">読込時のファイル更新日時</param>
    ''' <returns>メッセージID(空文字は成功)</returns>
    ''' <remarks></remarks>
    Public Shared Function SetValue(
        ByVal file As String,
        ByVal value As String,
        ByVal timeStamp As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("file", file, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("value", value, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("timeStamp", CType(timeStamp, String), True))

        Dim result As String

        Try
            Dim jsonFileInfo As New FileInfo(file)

            '読込時と現在のファイル更新日時を比較
            If timeStamp.Equals(jsonFileInfo.LastWriteTime.ToString(JsonUtilCommon.TimeStampFormat, CultureInfo.InvariantCulture)) Then
                'ファイル更新日時が一致する場合は書込処理
                result = SetValue(file, value)
            Else
                'ファイル更新日時に差異がある場合はエラー
                Logger.Info(TcvSettingUtilityBusinessLogic.GetLogWarn(WRITE_FAILED_COMPETITION))
                result = WRITE_FAILED_COMPETITION
            End If

        Catch ex As IOException
            '書き込みに失敗
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogWarn(OPEN_FAILED_IO))
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogWarnException(ex))
            result = OPEN_FAILED_IO

        End Try


        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(value))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return result

    End Function

    ''' <summary>
    ''' ファイルを書き込みます。
    ''' </summary>
    ''' <param name="file">ファイルパス</param>
    ''' <param name="value">書き込む内容</param>
    ''' <returns>メッセージID(空文字は成功)</returns>
    ''' <remarks></remarks>
    Public Shared Function SetValue(
        ByVal file As String,
        ByVal value As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("file", file, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("value", value, True))

        Dim result As String

        Try
            Using stream As New StreamWriter(file)
                stream.Write(value)
            End Using

            '書き込みに成功
            result = String.Empty

        Catch ex As IOException
            '書き込みに失敗
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogWarn(OPEN_FAILED_IO))
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogWarnException(ex))
            result = OPEN_FAILED_IO

        End Try


        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(value))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return result

    End Function

    ''' <summary>
    ''' 特定の項目名を置換します。
    ''' </summary>
    ''' <param name="value">文字列</param>
    ''' <param name="replaceValues">置き換え文字リスト</param>
    ''' <returns>置換後の文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function ReplaceProperty(
        ByVal value As String,
        ByVal replaceValues()() As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("value", value, False))
        If replaceValues Is Nothing Then
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("replaceValues", "0", True))
        Else
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("replaceValues", CType(replaceValues.Count, String), True))
        End If


        Dim result As String = Optimize(value, True, replaceValues)


        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(result))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return result
    End Function

    ''' <summary>
    ''' 特定の項目名を復元します。
    ''' </summary>
    ''' <param name="value">文字列</param>
    ''' <param name="replaceValues">置き換え文字リスト</param>
    ''' <returns>復元後の文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function RestoreProperty(
        ByVal value As String,
        ByVal replaceValues()() As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("value", value, False))
        If replaceValues Is Nothing Then
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("replaceValues", "0", True))
        Else
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("replaceValues", CType(replaceValues.Count, String), True))
        End If


        Dim result As String = Optimize(value, False, replaceValues)


        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(result))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return result
    End Function

    ''' <summary>
    ''' 読み込み、書き込み時に項目名を最適化します。
    ''' </summary>
    ''' <param name="value">文字列</param>
    ''' <param name="isReplase">置換する場合はTrue、復元する場合はFalse</param>
    ''' <param name="replaceValues">置き換え文字リスト</param>
    ''' <returns>最適化した文字列</returns>
    ''' <remarks></remarks>
    Private Shared Function Optimize(
        ByVal value As String,
        ByVal isReplase As Boolean,
        ByVal replaceValues()() As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("value", value, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("isReplase", CType(isReplase, String), True))
        If replaceValues Is Nothing Then
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("replaceValues", "0", True))
        Else
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("replaceValues", CType(replaceValues.Count, String), True))
        End If


        Dim indexOld As Integer
        Dim indexNew As Integer
        If isReplase Then
            '置換時の使用Index
            indexOld = 0
            indexNew = 1
        Else
            '復元時の使用Index
            indexOld = 1
            indexNew = 0
        End If

        For i As Integer = 0 To replaceValues.Length - 1
            value = value.Replace(replaceValues(i)(indexOld), replaceValues(i)(indexNew))
        Next


        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(value))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return value

    End Function

    ''' <summary>
    ''' JSONファイルへの出力データの取得を行います。
    ''' </summary>
    ''' <param name="abstract">書き込むデータ情報</param>
    ''' <param name="serializer">シリアライザ</param>
    ''' <param name="replaceValues">置き換え文字リスト</param>
    ''' <returns>JSON出力データ</returns>
    ''' <remarks></remarks>
    Public Shared Function GetWriteValue(
        ByVal abstract As AbstractJson,
        ByVal serializer As DataContractJsonSerializer,
        ByVal replaceValues()() As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam(
                    "abstract",
                    TcvSettingUtilityBusinessLogic.GetCountLog("abstract", abstract),
                    False))
        If serializer Is Nothing Then
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("serializer", "0", True))
        Else
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("serializer", "1", True))
        End If
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("replaceValues", CType(replaceValues.Count, String), True))


        Dim writeValue As String

        Using writeStream As New MemoryStream

            'クラスオブジェクトを文字列化
            serializer.WriteObject(writeStream, abstract)
            Dim json() As Byte = writeStream.ToArray()
            writeValue = Encoding.UTF8.GetString(json, 0, json.Length)

            'プロパティ名を復元
            writeValue = JsonUtilCommon.RestoreProperty(writeValue, replaceValues)

            '｢\/｣ を ｢/｣ に置き換える(GetStringメソッドで ｢/｣ が ｢\/｣に変換されるため)
            writeValue = writeValue.Replace(WRITE_ESCAPE_BEFORE, WRITE_ESCAPE_AFTER)

        End Using


        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(writeValue))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return writeValue

    End Function

#End Region

End Class
