Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Reflection.MethodBase

''' <summary>
''' TCV機能設定 共通ロジック
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class TcvSettingUtilityBusinessLogic

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
    ''' <summary>履歴ファイルのタイムスタンプとアカウントを接続する文字列</summary>
    Private Const HISTORY_CONNECT_CHAR As String = "_"
    ''' <summary>履歴ファイルの拡張子</summary>
    Private Const HISTORY_CONNECT_EXTENSION As String = ".xml"
    ''' <summary>履歴ファイル ルートノード名</summary>
    Private Const HISTORY_ROOT_NODE As String = "Root"
    ''' <summary>履歴ファイル XMLのバージョン</summary>
    Private Const HISTORY_XML_VERSION As String = "1.0"
    ''' <summary>履歴ファイル ReplicationFileInfo ノード名</summary>
    Private Const HISTORY_REPLICATIONFILEINFO_NODE As String = "ReplicationFileInfo"
    ''' <summary>履歴ファイル FileAccess ノード名</summary>
    Private Const HISTORY_FILEACCESS_NODE As String = "FileAccess"
    ''' <summary>履歴ファイル FilePath ノード名</summary>
    Private Const HISTORY_FILEPATH_NODE As String = "FilePath"
#End Region

#Region " tcv_web JSONファイル データ取得 "
    ''' <summary>
    ''' tcv_web JSONファイルを読み込み、クラスオブジェクトに変換します。
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carID">車種ID</param>
    ''' <returns>クラスオブジェクト</returns>
    ''' <remarks></remarks>
    Public Shared Function GetTcvWeb(
        ByVal tcvPath As String,
        ByVal carId As String
    ) As TcvWebListJson

        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(GetLogParam("carId", carId, True))

        'ファイル名取得
        Dim file As String = JsonUtilCommon.GetJsonFilePath(
                                tcvPath,
                                TcvSettingConstants.TcvWebJsonPath,
                                carId
                             )

        'JSONデータを文字列として取得
        '(書込みがないので排他エラーがない)
        Dim readValue As String = JsonUtilCommon.GetValue(file)

        'シリアライザ生成
        Dim serializer As DataContractJsonSerializer = New DataContractJsonSerializer(GetType(TcvWebListJson))


        '置換したデータをクラスオブジェクト化
        Dim tcvWebList As TcvWebListJson = Nothing
        Using readStream As New MemoryStream(Encoding.UTF8.GetBytes(readValue))
            Dim readObject As Object = serializer.ReadObject(readStream)
            tcvWebList = TryCast(readObject, TcvWebListJson)
        End Using


        '終了ログ出力
        Logger.Info(GetReturnTcvWebJson(tcvWebList))
        Logger.Info(GetLogMethod(GetCurrentMethod.Name, False))

        Return tcvWebList

    End Function

#End Region

#Region " setting JSONファイル データ取得 "
    ''' <summary>
    ''' setting JSONファイルを読み込み、クラスオブジェクトに変換します。
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="jsonPath">JSONファイルパス</param>
    ''' <param name="carID">車種ID</param>
    ''' <returns>クラスオブジェクト</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSetting(
        ByVal tcvPath As String,
        ByVal jsonPath As String,
        ByVal carId As String
    ) As SettingListJson

        '開始ログ出力
        Logger.Info(getLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(GetLogParam("jsonPath", jsonPath, True))
        Logger.Info(GetLogParam("carId", carId, True))

        'ファイル名取得
        Dim file As String = JsonUtilCommon.GetJsonFilePath(
                                tcvPath,
                                jsonPath,
                                carID
                             )

        'JSONデータを文字列として取得
        '(書込みがないので排他エラーがない)
        Dim readValue As String = JsonUtilCommon.GetValue(file)

        'シリアライザ生成
        Dim serializer As DataContractJsonSerializer = New DataContractJsonSerializer(GetType(SettingListJson))


        '置換したデータをクラスオブジェクト化
        Dim settingList As SettingListJson = Nothing
        Using readStream As New MemoryStream(Encoding.UTF8.GetBytes(readValue))
            Dim readObject As Object = serializer.ReadObject(readStream)
            settingList = TryCast(readObject, SettingListJson)
        End Using


        '終了ログ出力
        Logger.Info(GetReturnSettingJson(settingList))
        Logger.Info(getLogMethod(GetCurrentMethod.Name, False))

        Return settingList

    End Function

#End Region

#Region " car_lineup JSONファイル データ取得 "
    ''' <summary>
    ''' car_lineup JSONファイルを読み込み、クラスオブジェクトに変換します。
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <returns>クラスオブジェクト</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCarLineup(
        ByVal tcvPath As String
    ) As CarLineupCarSelectListJson

        '開始ログ出力
        Logger.Info(GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(GetLogParam("tcvPath", tcvPath, False))

        'ファイル名取得
        Dim file As String = JsonUtilCommon.GetJsonFilePath(
                                tcvPath,
                                TcvSettingConstants.CarLineupJsonPath,
                                String.Empty
                             )

        'JSONデータを文字列として取得
        '(書込みがないので排他エラーがない)
        Dim readValue As String = JsonUtilCommon.GetValue(file)

        'シリアライザ生成
        Dim serializer As DataContractJsonSerializer = New DataContractJsonSerializer(GetType(CarLineupCarSelectListJson))


        '置換したデータをクラスオブジェクト化
        Dim carSelectList As CarLineupCarSelectListJson = Nothing
        Using readStream As New MemoryStream(Encoding.UTF8.GetBytes(readValue))
            Dim readObject As Object = serializer.ReadObject(readStream)
            carSelectList = TryCast(readObject, CarLineupCarSelectListJson)
        End Using


        '終了ログ出力
        Logger.Info(GetReturnCarLineupJson(carSelectList))
        Logger.Info(GetLogMethod(GetCurrentMethod.Name, False))

        Return carSelectList

    End Function

#End Region

#Region " 履歴ファイル作成 "
    ''' <summary>
    ''' 履歴ファイルを作成します。
    ''' </summary>
    ''' <param name="HistoryPath">履歴ファイルを作成するパス</param>
    ''' <param name="timeStamp">タイムスタンプ</param>
    ''' <param name="acount">アカウント</param>
    ''' <param name="repFileInfoList">履歴ファイル情報</param>
    ''' <remarks></remarks>
    Public Shared Sub CreateRepFile(
        ByVal historyPath As String,
        ByVal timeStamp As String,
        ByVal acount As String,
        ByVal repFileInfoList As ReplicationFileRoot
    )

        '開始ログ出力
        Logger.Info(GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(GetLogParam("historyPath", historyPath, False))
        Logger.Info(GetLogParam("timeStamp", timeStamp, True))
        Logger.Info(GetLogParam("jsonPath", acount, True))
        Logger.Info(GetLogParam("jsonPath", CType(repFileInfoList.Root.Count, String), True))

        Dim historyFile As New StringBuilder
        Dim fileName As New StringBuilder
        fileName.Append(timeStamp)
        fileName.Append(HISTORY_CONNECT_CHAR)
        fileName.Append(acount)
        fileName.Append(HISTORY_CONNECT_EXTENSION)
        historyFile.Append(Path.Combine(historyPath, fileName.ToString))

        'ディレクトリの作成
        If Not Directory.Exists(historyPath) Then
            Directory.CreateDirectory(historyPath)
        End If

        Using sw As New StreamWriter(historyFile.ToString, False, Encoding.UTF8)
            'XML本体
            Dim xDocument As XmlDocument = New XmlDocument

            '宣言ノード
            Dim xDeclaration As XmlDeclaration =
                xDocument.CreateXmlDeclaration(
                    HISTORY_XML_VERSION,
                    Encoding.UTF8.BodyName,
                    Nothing)
            xDocument.AppendChild(xDeclaration)

            'ルートノード
            Dim xRoot As XmlElement = xDocument.CreateElement(HISTORY_ROOT_NODE)
            xDocument.AppendChild(xRoot)

            For Each repFile As ReplicationFileInfo In repFileInfoList.Root

                'ReplicationFileInfo ノード 作成
                Dim nodeRepFileInfo As XmlElement = xDocument.CreateElement(HISTORY_REPLICATIONFILEINFO_NODE)

                'FileAccess ノード追加
                Dim nodeFileAccess As XmlElement = xDocument.CreateElement(HISTORY_FILEACCESS_NODE)
                Dim valueFileAccess As XmlText = xDocument.CreateTextNode(repFile.FileAccess)
                nodeFileAccess.AppendChild(valueFileAccess)
                nodeRepFileInfo.AppendChild(nodeFileAccess)

                'FilePath ノード追加
                Dim nodeFilePath As XmlElement = xDocument.CreateElement(HISTORY_FILEPATH_NODE)
                Dim valueFilePath As XmlText = xDocument.CreateTextNode(repFile.FilePath)
                nodeFilePath.AppendChild(valueFilePath)
                nodeRepFileInfo.AppendChild(nodeFilePath)

                'ReplicationFileInfo ノード 追加
                xRoot.AppendChild(nodeRepFileInfo)

            Next

            '保存処理 (文字コードはUTF-8、改行コードはLf)
            sw.NewLine = vbLf
            xDocument.Save(sw)
        End Using


        '終了ログ出力
        Logger.Info(GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

#End Region

#Region "ログデータ加工処理"
    ''' <summary>
    ''' ログデータ（メソッド）
    ''' </summary>
    ''' <param name="methodName">メソッド名</param>
    ''' <param name="isMethodStart">True：「method start」を表示、False：「method end」を表示</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function GetLogMethod(
        ByVal methodName As String,
        ByVal isMethodStart As Boolean
    ) As String
        Dim sb As New StringBuilder
        With sb
            .Append("[")
            .Append(methodName)
            .Append("]")
            If isMethodStart Then
                .Append(" method start")
            Else
                .Append(" method end")
            End If
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログデータ（引数）
    ''' </summary>
    ''' <param name="paramName">引数名</param>
    ''' <param name="paramData">引数値</param>
    ''' <param name="isKanma">True：引数名の前に「,」を表示、False：特になし</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function GetLogParam(
        ByVal paramName As String,
        ByVal paramData As String,
        ByVal isKanma As Boolean
    ) As String
        Dim sb As New StringBuilder
        With sb
            If isKanma Then
                .Append(",")
            End If
            .Append(paramName)
            .Append("=")
            .Append(paramData)
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログデータ（戻り値）
    ''' </summary>
    ''' <param name="paramData">引数値</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function GetReturnParam(
        ByVal paramData As String
    ) As String
        Dim sb As New StringBuilder
        With sb
            .Append("Return=")
            .Append(paramData)
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログデータ（エラー）
    ''' </summary>
    ''' <param name="resultId">終了コード</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function GetLogWarn(
        ByVal resultId As String
    ) As String
        Dim sb As New StringBuilder
        With sb
            .Append("ResultId : ")
            .Append(resultId)
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログデータ（エラー）
    ''' </summary>
    ''' <param name="ex">例外エラー</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function GetLogWarnException(
        ByVal ex As Exception
    ) As String
        Dim sb As New StringBuilder
        With sb
            .Append("Exception : ")
            .Append(ex.Message)
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログデータ（DataSet）
    ''' 　データセット内部のデータテーブルの件数からログ出力用文字列を作成する
    ''' </summary>
    ''' <param name="paramDataSet">引数値</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function GetReturnDataSet(ByVal paramDataSet As DataSet) As String
        Dim sb As New StringBuilder
        With sb
            .Append("Return DataSet =")
            If paramDataSet Is Nothing Then
                .Append(" Count:0")
            Else
                Dim i As Integer = 0
                For Each dt As DataTable In paramDataSet.Tables
                    If 0 < i Then
                        .Append(",")
                    End If
                    .Append(dt.TableName)
                    .Append(" Count:")
                    .Append(CStr(dt.Rows.Count))

                    i = i + 1
                Next dt
            End If
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログのカウント出力 (格納クラス用)
    ''' </summary>
    ''' <param name="name">クラス名</param>
    ''' <param name="target">格納クラス</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCountLog(
        ByVal name As String,
        ByVal target As AbstractJson
    ) As String
        Dim sb As New StringBuilder
        With sb
            .Append(name)
            .Append(" Count:")
            If target Is Nothing Then
                .Append("0")
                Return sb.ToString()
            Else
                .Append("1")
            End If
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログのカウント出力 (格納クラス用)
    ''' </summary>
    ''' <param name="name">クラス名</param>
    ''' <param name="target">格納クラス(配列)</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCountLog(
        ByVal name As String,
        ByVal target() As AbstractJson
    ) As String
        Dim sb As New StringBuilder
        With sb
            .Append(name)
            .Append(" Count:")
            If target Is Nothing Then
                .Append("0")
                Return sb.ToString()
            Else
                .Append(target.Count)
            End If
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' ログのカウント出力 (格納クラス用)
    ''' </summary>
    ''' <param name="name">クラス名</param>
    ''' <param name="target">格納クラス(配列)</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCountLog(
        ByVal name As String,
        ByVal target As Array
    ) As String
        Dim sb As New StringBuilder
        With sb
            .Append(name)
            .Append(" Count:")
            If target Is Nothing Then
                .Append("0")
                Return sb.ToString()
            Else
                .Append(target.Length)
            End If
        End With
        Return sb.ToString
    End Function

    ''' <summary>
    ''' tcv_web 格納クラスログ出力
    ''' </summary>
    ''' <param name="paramJsonData">tcv_web JSONファイル</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Shared Function GetReturnTcvWebJson(
        ByVal paramJsonData As TcvWebListJson
    ) As String
        Dim sb As New StringBuilder
        With sb
            .Append("Return TcvWebListJson =")
            '全データ
            .Append(GetCountLog("tcv_web", paramJsonData))
            'ファイル情報
            .Append(GetCountLog(",fileinfo", paramJsonData.fileinfo))
            '車種情報
            .Append(GetCountLog(",car", paramJsonData.car))
            '再生環境情報
            .Append(GetCountLog(",player_info", paramJsonData.player_info))
            If Not paramJsonData.player_info Is Nothing Then
                .Append(GetCountLog(",introduction", paramJsonData.player_info.introduction))
            End If
            'グレード情報
            .Append(GetCountLog(",grade", paramJsonData.grade.ToArray))
            'ボディカラー情報
            .Append(GetCountLog(",exterior_color", paramJsonData.exterior_color.ToArray))
            'インテリアカラー情報
            .Append(GetCountLog(",interior_color", paramJsonData.interior_color.ToArray))
            'パーツ情報
            .Append(GetCountLog(",parts", paramJsonData.parts.ToArray))
            '排他情報
            .Append(GetCountLog(",haita", paramJsonData.haita.ToArray))
        End With

        Return sb.ToString

    End Function

    ''' <summary>
    ''' setting 格納クラスログ出力
    ''' </summary>
    ''' <param name="paramJsonData">setting JSONファイル</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Shared Function GetReturnSettingJson(
        ByVal paramJsonData As SettingListJson
    ) As String
        Dim sb As New StringBuilder
        With sb
            .Append("Return SettingListJson =")
            '全データ
            .Append(GetCountLog("setting", paramJsonData))
        End With

        Return sb.ToString

    End Function

    ''' <summary>
    ''' car_lineup 格納クラスログ出力
    ''' </summary>
    ''' <param name="paramJsonData">car_lineup JSONファイル</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Shared Function GetReturnCarLineupJson(
        ByVal paramJsonData As CarLineupCarSelectListJson
    ) As String
        Dim sb As New StringBuilder
        With sb
            .Append("Return CarLineupCarListJson =")
            '全データ
            If paramJsonData Is Nothing Then
                .Append(GetCountLog("car_lineup", paramJsonData))
            ElseIf paramJsonData.carselect Is Nothing Then
                .Append(GetCountLog("car_lineup", paramJsonData.carselect))
            Else
                .Append(GetCountLog("car_lineup", paramJsonData.carselect.carList.ToArray))
            End If
        End With

        Return sb.ToString

    End Function

#End Region

End Class
