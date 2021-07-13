Imports System.Globalization
Imports System.Reflection.MethodBase
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports System.IO
Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.TCVSettingUtility

''' <summary>
''' コンテンツメニュー設定画面のビジネスロジック層
''' </summary>
''' <remarks></remarks>
Public Class SC3050701BusinessLogic

#Region " コンストラクタ "
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        REM
    End Sub
#End Region

#Region " 定数 "

    ''' <summary>
    ''' ID:接頭語
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ID_PREFIX As String = "linker"

    ''' <summary>
    ''' アイコン画像ファイル:接頭語
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ICON_PREFIX As String = "footer_"

    ''' <summary>
    ''' 一覧行数(固定値)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LIST_COUNT As Integer = 4

    ''' <summary>
    ''' 更新リスト日付書式
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UPDATE_LIST_DATE_FORMAT As Integer = 15

    ''' <summary>
    ''' 更新リスト操作区分:ADD
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UPDATE_LIST_ADD As String = "ADD"

    ''' <summary>
    ''' 更新リスト操作区分:UPDATE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UPDATE_LIST_UPDATE As String = "UPDATE"

    ''' <summary>
    ''' 更新リスト操作区分:DELETE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UPDATE_LIST_DELETE As String = "DELETE"

    ''' <summary>
    ''' 表示順:削除行
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_DELETED As Integer = -1

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WORD_ERR_NOT_LATEST As Integer = 900

    ''' <summary>
    ''' 処理結果:正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultSucceed As Integer = 0

    ''' <summary>
    ''' 処理結果:異常終了
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultFailed As Integer = WORD_ERR_NOT_LATEST


#End Region

#Region " パブリック メソッド "

    ''' <summary>
    ''' コンテンツメニュー情報を取得します。
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="tcvHttp">TCVURL</param>
    ''' <param name="carId">車両ID</param>
    ''' <returns>コンテンツメニュー情報</returns>
    ''' <remarks></remarks>
    Public Function GetContentsMenuInfo(
        ByVal tcvPath As String,
        ByVal tcvHttp As String,
        ByVal carId As String
    ) As FooterListJson

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvHttp", tcvHttp, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))

        'JSOファイルパス取得
        Dim footerJsonPath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.ContentsMenuJsonPath, carId)

        'コンテンツメニュー情報取得
        Dim contentsMenuInfo As FooterListJson = GetFooter(footerJsonPath)

        'コンテンツメニュー一覧を調整
        Adjust(contentsMenuInfo.footerMap, tcvHttp, carId)

        'JSONファイルの更新日時取得
        contentsMenuInfo.TimeStamp = GetFileTimeStamp(footerJsonPath)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("footerMap", contentsMenuInfo.footerMap.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(contentsMenuInfo.TimeStamp))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        'コンテンツメニュー情報を返却
        Return contentsMenuInfo

    End Function

    ''' <summary>
    ''' コンテンツメニュー情報を更新します。
    ''' </summary>
    ''' <param name="contentsMenuInfo">コンテンツメニュー情報</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carId">車両ID</param>
    ''' <param name="account"></param>
    ''' <param name="updateListPath">更新リスト格納パス</param>
    ''' <returns>正常時は0、異常時はエラーメッセージIDを返します。</returns>
    ''' <remarks></remarks>
    Public Function UpdateContentsMenuInfo(
        ByVal contentsMenuInfo As FooterListJson,
        ByVal tcvPath As String,
        ByVal carId As String,
        ByVal account As String,
        ByVal updateListPath As String
    ) As Integer

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("TimeStamp", contentsMenuInfo.TimeStamp, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("account", account, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("updateListPath", updateListPath, True))

        'JSOファイルパス取得
        Dim footerJsonPath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.ContentsMenuJsonPath, carId)

        'JSONファイル更新
        Dim resultId As Integer = UpdateFooter(contentsMenuInfo, footerJsonPath, tcvPath, carId)

        If resultId = ResultSucceed Then

            '更新リスト情報生成
            Dim replicationRoot As New ReplicationFileRoot
            Dim replicationInfo As New ReplicationFileInfo
            replicationInfo.FilePath = footerJsonPath.Replace(tcvPath, "")
            replicationInfo.FileAccess = UPDATE_LIST_UPDATE
            replicationRoot.Root.Add(replicationInfo)

            'ファイルアップロード(更新リスト情報の設定を兼ねる)
            UploadFile(tcvPath, contentsMenuInfo.footerMap, replicationRoot)

            '更新リスト作成
            CallCreateTcvArchiveFile(replicationRoot, updateListPath, account)

        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(resultId.ToString(CultureInfo.InvariantCulture)))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        '処理結果を返却
        Return resultId

    End Function

#End Region

#Region " プライベート メソッド "

#Region " JSON取得 "

    ''' <summary>
    ''' footer.jsonの情報を取得します。
    ''' </summary>
    ''' <param name="footerJsonPath">footer.jsonのパス</param>
    ''' <returns>フッター情報</returns>
    ''' <remarks></remarks>
    Private Function GetFooter(
        ByVal footerJsonPath As String
    ) As FooterListJson

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("footerJsonPath", footerJsonPath, True))

        'フッター情報取得
        Dim footerJsonValue As String = JsonUtilCommon.GetValue(footerJsonPath)
        Dim footerJson As FooterListJson

        'フッター情報を変換
        Dim serializer As New DataContractJsonSerializer(GetType(FooterListJson))
        Using stream As New MemoryStream(Encoding.UTF8.GetBytes(footerJsonValue))
            footerJson = DirectCast(serializer.ReadObject(stream), FooterListJson)
        End Using

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        'フッター情報を返却
        Return footerJson

    End Function

#End Region

#Region " JSON更新 "

    ''' <summary>
    ''' コンテンツメニュー情報をJSONファイルに書き込みます。
    ''' またコンテンツメニュー一覧の内容が書き変えられます。
    ''' </summary>
    ''' <param name="contentsMenuInfo">コンテンツメニュー情報</param>
    ''' <param name="footerJsonPath">footer.jsonのパス</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carId">車両ID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function UpdateFooter(
        ByVal contentsMenuInfo As FooterListJson,
        ByVal footerJsonPath As String,
        ByVal tcvPath As String,
        ByVal carId As String
    ) As Integer

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))

        '更新用のコンテンツメニュー一覧を取得
        Dim updateContentsMenuList As List(Of FooterJson) = ToUpdateContentsMenuList(contentsMenuInfo.footerMap, tcvPath, carId)

        '更新情報を生成
        Dim updateContentsMenuInfo As New FooterListJson
        updateContentsMenuInfo.default_URI_scheme = contentsMenuInfo.default_URI_scheme
        updateContentsMenuInfo.footerMap = updateContentsMenuList

        'JSONファイルに出力する文字列に変換
        Dim serializer As New DataContractJsonSerializer(GetType(FooterListJson))
        Dim writeValue As String = JsonUtilCommon.GetWriteValue(updateContentsMenuInfo, serializer, {})

        'JSONファイルに書き込み
        Dim msgId As String = JsonUtilCommon.SetValue(footerJsonPath, writeValue, contentsMenuInfo.TimeStamp)
        Dim resultId As Integer = ResultSucceed
        If Not String.IsNullOrEmpty(msgId) Then
            resultId = Integer.Parse(msgId, CultureInfo.InvariantCulture)
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(msgId))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return resultId

    End Function

#End Region

#Region " コンテンツメニュー一覧加工 "

    ''' <summary>
    ''' コンテンツメニュー一覧を表示用に調整します。
    ''' </summary>
    ''' <param name="contentsMenuList">コンテンツメニュー一覧</param>
    ''' <param name="tcvHttp">TCVURL</param>
    ''' <param name="carId">車両ID</param>
    ''' <remarks></remarks>
    Private Sub Adjust(
        ByVal contentsMenuList As List(Of FooterJson),
        ByVal tcvHttp As String,
        ByVal carId As String
    )

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '設定する表示順
        Dim order As Integer = 1

        '付加情報を設定
        For Each contentsMenu As FooterJson In contentsMenuList

            'IDの接頭語を除去
            contentsMenu.id = contentsMenu.id.ToUpperInvariant.Replace(ID_PREFIX.ToUpperInvariant, String.Empty)

            'アイコン画像のファイル名を抽出
            Dim fileName As String = Path.GetFileName(contentsMenu.imageFile.normal)

            'アイコン画像情報を設定
            If Not String.IsNullOrEmpty(fileName) Then
                contentsMenu.IconPath = VirtualPathUtility.AppendTrailingSlash(tcvHttp) & BindSeries(TcvSettingConstants.ContentsMenuImagePath, carId) & fileName
                contentsMenu.IconNameNew = fileName
                contentsMenu.IconNameOld = fileName
            End If

            '表示順を設定
            contentsMenu.Order = order
            order += 1

        Next

        '所定の行数になるまで不足分を空行で埋める
        For i As Integer = order To LIST_COUNT
            Dim contentsMenu As New FooterJson
            contentsMenuList.Add(contentsMenu)
        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' コンテンツメニュー一覧をJSONファイルに書き込める状態にします。
    ''' 戻り値は引数のコンテンツメニュー一覧とは異なるインスタンスを返します。
    ''' </summary>
    ''' <param name="contentsMenuList">コンテンツメニュー一覧</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carId">車両ID</param>
    ''' <returns>JSONファイルに書き込むためのコンテンツメニュー一覧</returns>
    ''' <remarks></remarks>
    Private Function ToUpdateContentsMenuList(
        ByVal contentsMenuList As List(Of FooterJson),
        ByVal tcvPath As String,
        ByVal carId As String
    ) As List(Of FooterJson)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        'IDの最大値を取得
        Dim maxId As Integer = GetMaxId(contentsMenuList)

        'ソート
        contentsMenuList.Sort(AddressOf CompareForJson)

        Dim updateContentsMenuList As New List(Of FooterJson)
        For Each contentsMenu As FooterJson In contentsMenuList
            'アップロードおよび削除時に使用する
            '処理対象ディレクトリを無条件に設定
            contentsMenu.IconPath = Path.Combine(tcvPath, BindSeries(TcvSettingConstants.ContentsMenuUploadPath, carId))

            '必須項目が設定されていれば処理する
            If Not String.IsNullOrEmpty(contentsMenu.name) Then
                '新規行判定フラグ
                Dim isAddRow As Boolean = False
                '新規行のIDを設定
                If String.IsNullOrEmpty(contentsMenu.id) Then
                    maxId += 1
                    contentsMenu.id = maxId.ToString(CultureInfo.InvariantCulture)
                    '新規行とする
                    isAddRow = True
                End If

                'ファイル情報を設定
                If IsNothing(contentsMenu.PostedFile) Then
                    '新規行でない場合
                    If Not isAddRow Then
                        '削除指示された(旧ファイルがあり新ファイルがない)場合
                        If Not String.IsNullOrEmpty(contentsMenu.IconNameOld) AndAlso String.IsNullOrEmpty(contentsMenu.IconNameNew) Then
                            '画像ファイルパスを消去
                            contentsMenu.imageFile.normal = String.Empty
                        End If
                    Else
                        '新規行の場合
                        contentsMenu.imageFile.normal = String.Empty
                    End If
                Else
                    'アップロードするファイル情報を設定
                    Dim extension As String = Path.GetExtension(contentsMenu.IconNameNew)
                    contentsMenu.IconNameNew = ICON_PREFIX & contentsMenu.id & extension
                    contentsMenu.imageFile.normal = Path.Combine(BindSeries(TcvSettingConstants.ContentsMenuPath, carId), contentsMenu.IconNameNew)
                End If

                'その他項目を設定
                contentsMenu.id = ID_PREFIX & contentsMenu.id   'ID(接頭語を付与)
                contentsMenu.url_name = contentsMenu.id         '名称
                '新規行の場合
                If isAddRow Then
                    contentsMenu.exists = True                                      '有無フラグ
                    contentsMenu.imageFile.on = String.Empty                        '選択時画像ファイル
                    contentsMenu.imageFile.disable = String.Empty                   '選択不可時画像ファイル
                Else
                    '更新行の場合
                    contentsMenu.exists = contentsMenu.exists                       '有無フラグ
                    contentsMenu.imageFile.on = contentsMenu.imageFile.on           '選択時画像ファイル
                    contentsMenu.imageFile.disable = contentsMenu.imageFile.disable '選択不可時画像ファイル
                End If
                '更新対象に追加
                updateContentsMenuList.Add(contentsMenu)
            End If
        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return updateContentsMenuList

    End Function

    ''' <summary>
    ''' ソート条件を定義します。JSONファイルを更新する時に使用します。
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CompareForJson(ByVal x As FooterJson, ByVal y As FooterJson) As Integer

        Dim result As Integer = 0

        '表示順の昇順
        If x.Order > y.Order Then
            result = 1
        ElseIf x.Order < y.Order Then
            result = -1
        End If

        Return result

    End Function

    ''' <summary>
    ''' ソート条件を定義します。ファイルをアップロードする時に使用します。
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CompareForUpload(ByVal x As FooterJson, ByVal y As FooterJson) As Integer

        Dim result As Integer = 0

        Dim isDeletedX As Boolean = False
        If Not String.IsNullOrEmpty(x.IconNameOld) AndAlso String.IsNullOrEmpty(x.IconNameNew) Then
            isDeletedX = True
        End If

        Dim isDeletedY As Boolean = False
        If Not String.IsNullOrEmpty(y.IconNameOld) AndAlso String.IsNullOrEmpty(y.IconNameNew) Then
            isDeletedY = True
        End If

        '削除された行を上位へ
        If Not isDeletedX AndAlso isDeletedY Then
            result = 1
        ElseIf isDeletedX AndAlso Not isDeletedY Then
            result = -1
        Else
            result = CompareForJson(x, y)
        End If

        Return result

    End Function

#End Region

#Region " ファイルアップロード "

    ''' <summary>
    ''' ファイルをアップロードします。
    ''' </summary>
    ''' <param name="tcvPath"></param>
    ''' <param name="contentsMenuList"></param>
    ''' <param name="replicationRoot"></param>
    ''' <remarks></remarks>
    Private Sub UploadFile(
        ByVal tcvPath As String,
        ByVal contentsMenuList As List(Of FooterJson),
        ByVal replicationRoot As ReplicationFileRoot
    )

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '再ソート(削除データを優先処理)
        contentsMenuList.Sort(AddressOf CompareForUpload)

        For Each contentsMenu As FooterJson In contentsMenuList

            Dim replicationInfo As ReplicationFileInfo = Nothing

            '新旧ファイルの有無を設定
            Dim hasOldFile As Boolean = Not String.IsNullOrEmpty(contentsMenu.IconNameOld)
            Dim hasNewFile As Boolean = Not IsNothing(contentsMenu.PostedFile)

            '新旧ファイルが存在する場合
            If hasOldFile AndAlso hasNewFile Then
                '新旧ファイルのファイル名が同じ場合
                If contentsMenu.IconNameOld.Equals(contentsMenu.IconNameNew) Then
                    '新ファイルで旧ファイルを上書き
                    replicationInfo = UpdateFile(tcvPath, contentsMenu)
                    replicationRoot.Root.Add(replicationInfo)
                Else
                    '新旧ファイルが異なる場合
                    '新ファイル作成
                    replicationInfo = AddFile(tcvPath, contentsMenu)
                    replicationRoot.Root.Add(replicationInfo)

                    '旧ファイル削除
                    replicationInfo = DeleteFile(tcvPath, contentsMenu)
                    replicationRoot.Root.Add(replicationInfo)
                End If
            ElseIf hasOldFile Then
                '旧ファイルが存在し、かつ削除指示された場合
                '旧ファイル削除
                If String.IsNullOrEmpty(contentsMenu.IconNameNew) Then
                    replicationInfo = DeleteFile(tcvPath, contentsMenu)
                    replicationRoot.Root.Add(replicationInfo)
                End If
            ElseIf hasNewFile Then
                '新ファイルのみが存在する場合
                '新ファイル作成
                replicationInfo = AddFile(tcvPath, contentsMenu)
                replicationRoot.Root.Add(replicationInfo)
            End If
        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' ファイルを作成します。
    ''' </summary>
    ''' <param name="contentsMenu">コンテンツメニュー情報</param>
    ''' <returns>更新リスト情報</returns>
    ''' <remarks></remarks>
    Private Function AddFile(ByVal tcvPath As String, ByVal contentsMenu As FooterJson) As ReplicationFileInfo

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        Dim filePath As String = Path.Combine(contentsMenu.IconPath, contentsMenu.IconNameNew)
        contentsMenu.PostedFile.SaveAs(filePath)
        Dim replicationInfo As New ReplicationFileInfo
        replicationInfo.FilePath = filePath.Replace(tcvPath, "")
        replicationInfo.FileAccess = UPDATE_LIST_ADD

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return replicationInfo

    End Function

    ''' <summary>
    ''' ファイルを上書きします。
    ''' </summary>
    ''' <param name="contentsMenu">コンテンツメニュー情報</param>
    ''' <returns>更新リスト情報</returns>
    ''' <remarks></remarks>
    Private Function UpdateFile(ByVal tcvPath As String, ByVal contentsMenu As FooterJson) As ReplicationFileInfo

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        Dim filePath As String = Path.Combine(contentsMenu.IconPath, contentsMenu.IconNameNew)
        contentsMenu.PostedFile.SaveAs(filePath)
        Dim replicationInfo As New ReplicationFileInfo
        replicationInfo.FilePath = filePath.Replace(tcvPath, "")
        replicationInfo.FileAccess = UPDATE_LIST_UPDATE

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return replicationInfo

    End Function

    ''' <summary>
    ''' ファイルを削除します。
    ''' </summary>
    ''' <param name="contentsMenu">コンテンツメニュー情報</param>
    ''' <returns>更新リスト情報</returns>
    ''' <remarks></remarks>
    Private Function DeleteFile(ByVal tcvPath As String, ByVal contentsMenu As FooterJson) As ReplicationFileInfo

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        Dim filePath As String = Path.Combine(contentsMenu.IconPath, contentsMenu.IconNameOld)
        File.Delete(filePath)
        Dim replicationInfo As New ReplicationFileInfo
        replicationInfo.FilePath = filePath.Replace(tcvPath, "")
        replicationInfo.FileAccess = UPDATE_LIST_DELETE

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return replicationInfo

    End Function

#End Region

#Region " ユーティリティ "

    ''' <summary>
    ''' ファイルの最終更新日時を取得します。
    ''' </summary>
    ''' <param name="filePath">ファイルパス</param>
    ''' <returns>最終更新日時</returns>
    ''' <remarks></remarks>
    Private Function GetFileTimeStamp(ByVal filePath As String) As String
        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("filePath", filePath, False))
        Dim timestamp As String
        If File.Exists(filePath) Then
            Dim fileInfo As New FileInfo(filePath)
            timestamp = fileInfo.LastWriteTime.ToString(JsonUtilCommon.TimeStampFormat, CultureInfo.InvariantCulture)
        Else
            timestamp = String.Empty
        End If
        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(timestamp))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))
        Return timestamp
    End Function

    ''' <summary>
    ''' 更新対象となる行のうちIDの最大値を取得します。
    ''' </summary>
    ''' <param name="contentsMenuList">コンテンツメニュー一覧</param>
    ''' <returns>IDの最大値</returns>
    ''' <remarks></remarks>
    Private Function GetMaxId(ByVal contentsMenuList As List(Of FooterJson)) As Integer

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        Dim maxId As Integer = 0
        For Each contentsMenu As FooterJson In contentsMenuList
            If Not String.IsNullOrEmpty(contentsMenu.name) Then
                Dim id As Integer
                If Integer.TryParse(contentsMenu.id, id) Then
                    If maxId < id Then
                        maxId = id
                    End If
                End If
            End If
        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(maxId.ToString(CultureInfo.InvariantCulture)))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return maxId

    End Function

    ''' <summary>
    ''' 更新リストを作成します。
    ''' </summary>
    ''' <param name="replicationRoot">更新リスト情報</param>
    ''' <param name="updateListPath">更新リストファイルパス</param>
    ''' <param name="account">アカウント</param>
    ''' <remarks></remarks>
    Private Sub CallCreateTcvArchiveFile(
        ByVal replicationRoot As ReplicationFileRoot,
        ByVal updateListPath As String,
        ByVal account As String
    )

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("updateListPath", updateListPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("account", account, False))

        '現在日時取得
        Dim timeStamp As String = DateTimeFunc.FormatDate(UPDATE_LIST_DATE_FORMAT, DateTimeFunc.Now)

        '更新リスト作成
        TcvSettingUtilityBusinessLogic.CreateRepFile(updateListPath, timeStamp, account, replicationRoot)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' 文字列に車両IDをバインドします。
    ''' </summary>
    ''' <param name="value">文字列</param>
    ''' <param name="carId">車両ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function BindSeries(ByVal value As String, ByVal carId As String) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("value", value, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))

        Dim result As String = value.Replace(JsonUtilCommon.ReplaceFileString, carId)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(result))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return result

    End Function

#End Region

#End Region

End Class
