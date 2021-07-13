
Imports System.IO
Imports System.Diagnostics
Imports System.Xml
Imports System.Reflection.MethodBase
Imports System.Web.HttpServerUtility
Imports ICSharpCode.SharpZipLib.Zip
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.TCVSettingUtility
Imports System.Web
Imports System.Globalization

''' <summary>
''' SC3050701 販売店サーバーへの転送機能ビジネスロジック層
''' </summary>
''' <remarks></remarks>
Public Class IC3050701BusinessLogic

#Region "定数"

    ''' <summary>
    ''' TCV物理パスパラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Const TCV_PATH As String = "TCV_PATH"

    ''' <summary>
    ''' 履歴ファイル格納パスパラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Const HISTORY_FILE_PATH As String = "TCV_SETTING_HISTORYFILE_PATH"

    ''' <summary>
    ''' 圧縮ファイル作成パスパラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Const ARCHIVE_FILE_PATH As String = "TCV_ARCHIVE_PATH"

    ''' <summary>
    ''' 圧縮ファイル分割容量パラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Const ARCHIVE_DEVIDE_SIZE As String = "TCV_ARCHIVE_DEVIDE_SIZE"

    ''' <summary>
    ''' 圧縮ファイル保持日数
    ''' </summary>
    ''' <remarks></remarks>
    Const ARCHIVE_SAVE_DAYS As String = "TCV_ARCHIVE_SAVE_DAYS"

    ''' <summary>
    ''' 圧縮ファイル拡張子
    ''' </summary>
    ''' <remarks></remarks>
    Const EXTENSION_ZIP As String = ".zip"

    ''' <summary>
    ''' IC3050701DataSet項目名(圧縮ファイル)
    ''' </summary>
    ''' <remarks></remarks>

    Const ARCHIVEDATA As String = "ArchiveData"

    ''' <summary>
    ''' IC3050701DataSet項目名(同期時刻)
    ''' </summary>
    ''' <remarks></remarks>
    Const REPTIME As String = "RepTime"

    ''' 
    ''' <summary>
    ''' IC3050701DataSet項目名(チェックコード)
    ''' </summary>
    ''' <remarks></remarks>
    Const CHECKCODE As String = "CheckCode"

    ''' 
    ''' <summary>
    ''' IC3050701DataSet項目名(分割件数)
    ''' </summary>
    ''' <remarks></remarks>
    Const DEVIDECOUNT As String = "DevideCount"

    ''' <summary>
    ''' IC3050701DataSet項目名(圧縮ファイル名)
    ''' </summary>
    ''' <remarks></remarks>
    Const ZIPFILENAME As String = "ZipFileName"

    ''' <summary>
    ''' IC3050701DataSet項目名(更新リスト用ファイル名)
    ''' </summary>
    ''' <remarks></remarks>
    Const UPDLISTFILENAME As String = "UpdListFileName"

    ''' <summary>
    ''' IC3050701DataSet項目名(メッセージID)
    ''' </summary>
    ''' <remarks></remarks>

    Const MSG_ID As String = "msgID"

    ''' <summary>
    ''' IC3050701DataSet項目名(メッセージ)
    ''' </summary>
    ''' <remarks></remarks>

    Const MSG_STR As String = "msg"

    ''' <summary>
    ''' 作成圧縮ファイルプレフィックス
    ''' </summary>
    ''' <remarks></remarks>
    Const ARCHIVE_PREFIX As String = "IC3050701ARCHIVE_"

    ''' <summary>
    ''' リターンコード(正常終了)
    ''' </summary>
    ''' <remarks></remarks>

    Const None As Integer = 0

    ''' <summary>
    ''' リターンコード(異常終了)
    ''' </summary>
    ''' <remarks></remarks>

    Const SystemError As Integer = 9999

    ''' <summary>
    ''' プログラムID（販売店サーバーへの転送）
    ''' </summary>
    ''' <remarks></remarks>
    Const ProgramIdLog As String = "IC3050701 IC3050701BusinessLogic-"

    ''' <summary>
    ''' 販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Const DistDlrCd As String = "00000"



#End Region

#Region "zip形式圧縮ファイルの作成"

    ''' <summary>
    ''' zip形式圧縮ファイル作成
    ''' </summary>
    ''' <param name="syncDlrcd">販売店コード</param>
    ''' <param name="syncBeforeTime">前回同期時刻</param>
    ''' <param name="syncDevideNo">分割圧縮ファイル、取得№</param>
    ''' <param name="syncZipFileName">圧縮ファイル名</param>
    ''' <returns>zip形式圧縮ファイル</returns>
    ''' <remarks></remarks>

    Public Function CanExcute(ByVal syncDlrcd As String, ByVal syncBeforeTime As String, ByVal syncDevideNo As String, ByVal syncZipFileName As String) As IC3050701DataSet

        '開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("syncDlrcd", syncDlrcd, False))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("syncBeforeTime", syncBeforeTime, False))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("syncDevideNo", syncDevideNo, False))

        Using dtSet As New IC3050701DataSet

            Dim repTable As IC3050701DataSet.REPINFODataTableDataTable = dtSet.REPINFODataTable
            Dim repRow As IC3050701DataSet.REPINFODataTableRow = repTable.NewREPINFODataTableRow

            Dim fileListTable As IC3050701DataSet.FileListDataTable = dtSet.FileList

            Try

                '引数(販売店コード)の取得チェック
                If String.IsNullOrEmpty(syncDlrcd) Then

                    Dim msg As String = "Input Param Error syncDlrcd = nothing"

                    'ワーニングログ出力
                    Logger.Error(ProgramIdLog + msg)

                    repRow.Item(MSG_ID) = SystemError
                    repRow.Item(MSG_STR) = msg
                    repTable.AddREPINFODataTableRow(repRow)

                    Return dtSet
                End If

                '引数(分割圧縮ファイル、取得№)の取得チェック
                If String.IsNullOrEmpty(syncDevideNo) Then

                    Dim msg As String = "Input Param Error syncDevideNo = nothing"

                    'ワーニングログ出力
                    Logger.Error(ProgramIdLog + msg)

                    repRow.Item(MSG_ID) = SystemError
                    repRow.Item(MSG_STR) = msg
                    repTable.AddREPINFODataTableRow(repRow)

                    Return dtSet
                End If

                '引数(圧縮ファイル名)の取得チェック(初回以外は必須とする。)
                If Not "0".Equals(syncDevideNo) Then
                    If String.IsNullOrEmpty(syncZipFileName) Then

                        Dim msg As String = "Input Param Error syncZipFileName = nothing"

                        'ワーニングログ出力
                        Logger.Error(ProgramIdLog + msg)

                        repRow.Item(MSG_ID) = SystemError
                        repRow.Item(MSG_STR) = msg
                        repTable.AddREPINFODataTableRow(repRow)

                        Return dtSet
                    End If
                End If

                '環境変数値の取得チェック
                Dim chkEnvString As String = CheckEnvParam(syncDlrcd)

                '環境変数が未設定であれば処理をしない
                If chkEnvString.Length > 0 Then
                    'ワーニングログ出力
                    Logger.Error(ProgramIdLog + chkEnvString)

                    repRow.Item(MSG_ID) = SystemError
                    repRow.Item(MSG_STR) = chkEnvString
                    repTable.AddREPINFODataTableRow(repRow)

                    Return dtSet

                End If

                '同期時刻が末セットであれば、初回同期とみなす。
                If String.IsNullOrEmpty(syncBeforeTime) Then
                    syncBeforeTime = "00000000000000"
                End If

                Dim chkDlrEnv As New DealerEnvSetting
                '1圧縮ファイルの最大サイズを取得（バイト）
                Dim blockSize As String = ""

                Dim dlrGet As Boolean = True
                Try
                    blockSize = chkDlrEnv.GetEnvSetting(syncDlrcd, ARCHIVE_DEVIDE_SIZE).PARAMVALUE

                    If blockSize.Trim.Length = 0 Then
                        dlrGet = False
                    End If

                Catch ex As System.NullReferenceException
                    dlrGet = False
                End Try
                '販売店コードから取得できなかった場合
                If Not dlrGet Then
                    blockSize = chkDlrEnv.GetEnvSetting(DistDlrCd, ARCHIVE_DEVIDE_SIZE).PARAMVALUE
                End If

                blockSize = CStr(CLng(blockSize) * 1024)

                'ログ用変数
                Dim logRepTime As String = ""
                Dim logZipFileName As String = ""
                Dim logDevideCount As String = ""
                Dim logCheckCode As String = ""

                '初回アクセス時
                If "0".Equals(syncDevideNo) Then

                    Dim sysEnv As New SystemEnvSetting

                    '圧縮ファイル作成パスを取得
                    Dim archivePath As String = sysEnv.GetSystemEnvSetting(ARCHIVE_FILE_PATH).PARAMVALUE

                    '圧縮ファイル保持日数を取得
                    Dim archiveSaveDays As String = sysEnv.GetSystemEnvSetting(ARCHIVE_SAVE_DAYS).PARAMVALUE

                    '一定期間前の過去圧縮ファイルの削除(記録用更新ファイルも削除)
                    deleteZipFile(archivePath, archiveSaveDays)

                    '今回処理対象となる更新リストファイル名の取得
                    Dim updateList As List(Of String) = CreateTargetList(syncDlrcd, syncBeforeTime)

                    '更新リストから圧縮対象ファイル名の取得
                    Dim archiveList As List(Of String) = CreateArchiveTargetList(updateList)

                    Dim archiveFileName As String = ""
                    Dim beforeTime As String = ""
                    Dim afterTime As String = ""

                    '記録用更新リストファイル名
                    Dim updateListFileName As String = ""

                    '今回の圧縮対象が存在すれば圧縮ファイル名を求める
                    If archiveList.Count > 0 Then

                        beforeTime = Path.GetFileNameWithoutExtension(updateList(0)).Substring(0, 14)
                        afterTime = Path.GetFileNameWithoutExtension(updateList(updateList.Count - 1)).Substring(0, 14)

                        archiveFileName = ARCHIVE_PREFIX & syncDlrcd & "_" & beforeTime & "_" & afterTime & ".zip"
                        updateListFileName = ARCHIVE_PREFIX & syncDlrcd & "_" & beforeTime & "_" & afterTime & ".txt"
                    End If

                    '今回同期時刻の既定値は、前回同期時刻
                    '= 今回同期対象がなければ、前回同期時刻を返す
                    Dim nowRepTime As String = syncBeforeTime
                    '今回圧縮ファイルを作成していれば、今回同期時刻を更新
                    If archiveList.Count > 0 Then
                        nowRepTime = afterTime
                    End If

                    'TCVルートディレクトリの取得
                    Dim tcvPath As String

                    tcvPath = sysEnv.GetSystemEnvSetting(TCV_PATH).PARAMVALUE


                    '今回の同期対象があれば(本当に存在すれば)
                    If archiveList.Count > 0 AndAlso checkExistArchiveFile(tcvPath, archiveList) Then

                        '圧縮ファイルフルパス
                        Dim zipFilePath As String = archivePath & archiveFileName

                        '記録用更新リストフルパス
                        Dim updateListFilePath As String = archivePath & updateListFileName

                        '圧縮ファイルが存在しなければ新規に圧縮ファイルを作成
                        If Not File.Exists(zipFilePath) Then


                            '一時ディレクトリのパスの取得
                            Dim workPath As String = GetwkPath(archivePath)
                            '一時ディレクトリの作成
                            Directory.CreateDirectory(workPath)

                            '処理対象ファイルを一時ディレクトリにコピーする。
                            For i As Integer = 0 To archiveList.Count - 1

                                'ファイルの存在チェック
                                If File.Exists(tcvPath + archiveList(i)) Then
                                    '一時ディレクトリの作成
                                    CreatePath(archiveList(i), workPath)
                                    '一時ファイルの作成
                                    CreateTempFile(tcvPath, workPath, archiveList(i))

                                    'ファイルが存在しなくても、通常削除の可能性があるのでエラーとはしない。
                                    'ワーニングログを出力するのみ
                                Else
                                    Logger.Warn(ProgramIdLog + "not Exist archiveList File :" + tcvPath + archiveList(i))
                                    archiveList(i) = archiveList(i) + ":not Exist"
                                End If

                            Next

                            'Zipファイル作成処理
                            CreateZipFile(zipFilePath, workPath)

                            'MD5ハッシュコードを取得
                            Dim writeCheckCode As String = GetZipMD5(zipFilePath)

                            '記録用更新リストを作成
                            Dim updateFileName As String = archivePath & updateListFileName

                            writeUpdateFile(writeCheckCode, updateFileName, archiveList)

                            '作業用一時ディレクトリの削除
                            Directory.Delete(workPath, True)

                        End If


                        If "0".Equals(syncDevideNo) Then
                            '圧縮ファイルの分割件数を取得
                            Dim tempDevideCount As Integer = getDevideCount(zipFilePath, blockSize)
                            repRow.Item(DEVIDECOUNT) = tempDevideCount

                            logDevideCount = CStr(tempDevideCount)
                        End If

                        'MD5ハッシュコードを記録用更新リストから取得
                        Dim tmpCheckCode As String = readUpdateFileForCheckCode(updateListFilePath)

                        repRow.Item(CHECKCODE) = tmpCheckCode
                        '今回同期時刻をセット
                        repRow.Item(REPTIME) = nowRepTime
                        '今回圧縮ファイル名をセット
                        repRow.Item(ZIPFILENAME) = archiveFileName

                        logRepTime = nowRepTime
                        logZipFileName = archiveFileName
                        logCheckCode = tmpCheckCode

                        '送信するファイルを記録用更新リストから取得
                        archiveList = readUpdateFileForFileList(updateListFilePath)

                        '圧縮ファイルに含まれるファイルを返却値にセット
                        For i As Integer = 0 To archiveList.Count - 1
                            Dim fileRow As IC3050701DataSet.FileListRow = fileListTable.NewFileListRow
                            fileRow.Item(UPDLISTFILENAME) = archiveList(i)

                            fileListTable.AddFileListRow(fileRow)
                        Next

                    End If

                    '二回目移行
                Else

                    '圧縮ファイルフルパスを取得
                    Dim sysEnv As New SystemEnvSetting
                    Dim archivePath As String = sysEnv.GetSystemEnvSetting(ARCHIVE_FILE_PATH).PARAMVALUE

                    '圧縮ファイルのバイナリデータを取得
                    repRow.Item(ARCHIVEDATA) = readZipFile(archivePath & syncZipFileName, syncDevideNo, blockSize)

                End If

                repRow.Item(MSG_ID) = None
                repRow.Item(MSG_STR) = ""
                repTable.AddREPINFODataTableRow(repRow)

                '終了ログの出力
                Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetReturnDataSet(dtSet))

                Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam(REPTIME, logRepTime, False))
                Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam(ZIPFILENAME, logZipFileName, False))
                Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam(CHECKCODE, logCheckCode, False))
                Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam(DEVIDECOUNT, logDevideCount, False))

                Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

            Catch ex As Exception
                ' 異常終了ログ
                Logger.Info(ProgramIdLog + "Process AbNormalEnd")
                'ErroeLog
                Logger.Error(ProgramIdLog + ex.Message, ex)

                repRow.Item(MSG_ID) = SystemError
                repRow.Item(MSG_STR) = ex.Message

                repTable.AddREPINFODataTableRow(repRow)

            End Try

                Return dtSet

        End Using

    End Function
#End Region

#Region "パラメータチェックの実施"

    ''' <summary>
    ''' 環境変数パラメータチェックの実施
    ''' </summary>
    ''' <param name="syncDlrcd">販売店コード</param>
    ''' <returns>エラーメッセージ</returns>
    ''' <remarks></remarks>
    Private Function CheckEnvParam(ByVal syncDlrcd As String) As String

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        Dim chkString As String

        Dim chkSysEnv As New SystemEnvSetting
        Dim chkDlrEnv As New DealerEnvSetting

        Dim chkEnvString As String = ""

        '環境変数値の取得チェック
        Try
            chkString = chkSysEnv.GetSystemEnvSetting(ARCHIVE_FILE_PATH).PARAMVALUE

            If chkString.Trim.Length = 0 Then
                chkEnvString &= "SystemEnv Param Error ArchivePath = nothing; "
            End If

        Catch ex As System.NullReferenceException
            chkEnvString &= "SystemEnv Param Error ArchivePath = nothing; "
        End Try


        Try
            chkString = chkSysEnv.GetSystemEnvSetting(HISTORY_FILE_PATH).PARAMVALUE

            If chkString.Trim.Length = 0 Then
                chkEnvString &= "SystemEnv Param Error HistoryFilePath = nothing; "
            End If

        Catch ex As System.NullReferenceException
            chkEnvString &= "SystemEnv Param Error HistoryFilePath = nothing; "
        End Try

        Try
            chkString = chkSysEnv.GetSystemEnvSetting(TCV_PATH).PARAMVALUE

            If chkString.Trim.Length = 0 Then
                chkEnvString &= "SystemEnv Param Error TcvPath = nothing; "
            End If

        Catch ex As System.NullReferenceException
            chkEnvString &= "SystemEnv Param Error TcvPath = nothing; "
        End Try

        Dim dlrGet As Boolean = True

        Try
            chkString = chkDlrEnv.GetEnvSetting(syncDlrcd, ARCHIVE_DEVIDE_SIZE).PARAMVALUE
            If chkString.Trim.Length = 0 Then
                dlrGet = False
            End If

        Catch ex As System.NullReferenceException
            dlrGet = False
        End Try

        If Not dlrGet Then
            Try
                chkString = chkDlrEnv.GetEnvSetting(DistDlrCd, ARCHIVE_DEVIDE_SIZE).PARAMVALUE
                If chkString.Trim.Length = 0 Then
                    chkEnvString &= "DlrEnv Param Error ArchiveDevideSize = nothing; "
                    dlrGet = False
                End If

            Catch ex As System.NullReferenceException
                dlrGet = False
                chkEnvString &= "DlrEnv Param Error ArchiveDevideSize = nothing; "
            End Try

        End If


        'メソッド終了ログ出力
        Logger.Info(ProgramIdLog + "chkEnvString = " & chkEnvString)
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return chkEnvString

    End Function


#End Region


#Region "処理対象更新リストファイル名の取得"

    ''' <summary>
    ''' 処理対象更新リストファイル名の取得
    ''' </summary>
    ''' <param name="syncDlrcd">販売店コード</param>
    ''' <param name="syncBeforeTime">前回同期時刻</param>
    ''' <returns>作成対象ファイル名</returns>
    ''' <remarks></remarks>
    Private Function CreateTargetList(ByVal syncDlrcd As String, ByVal syncBeforeTime As String) As List(Of String)

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("syncDlrcd", syncDlrcd, False))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("syncBeforeTime", syncBeforeTime, False))

        Dim targetList As List(Of String) = New List(Of String)

        'ルートディレクトリの取得
        Dim sysEnv As New SystemEnvSetting
        Dim historyFilePath As String = sysEnv.GetSystemEnvSetting(HISTORY_FILE_PATH).PARAMVALUE
        sysEnv = Nothing

        If Not System.IO.Directory.Exists(historyFilePath) Then
            Return targetList
        End If

        '自販売店の更新リストを取得
        Dim fileList As String() = Directory.GetFiles(historyFilePath, "*" & syncDlrcd & "*")
        'Distの更新リストを取得
        Dim fileListDist As String() = Directory.GetFiles(historyFilePath, "*00000*")

        '自販売店と、Distの配列をマージ
        Dim mergedArray As String() = New String(fileList.Length + fileListDist.Length - 1) {}
        Array.Copy(fileList, mergedArray, fileList.Length)
        Array.Copy(fileListDist, 0, mergedArray, fileList.Length, fileListDist.Length)

        '更新リストのソート
        Array.Sort(mergedArray)

        For i As Integer = 0 To mergedArray.Count - 1
            Dim fileName As String = mergedArray(i)
            '前回同期時刻より後に作成されたファイルのみを取得
            If syncBeforeTime < Path.GetFileNameWithoutExtension(fileName).Substring(0, 14) Then
                targetList.Add(fileName)
                Logger.Info(ProgramIdLog + "UpdateList targetList.Add = " & fileName)
            End If
        Next

        'メソッド終了ログ出力
        Logger.Info(ProgramIdLog + "UpdateList targetList.count = " & targetList.Count)
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return targetList

    End Function


#End Region

#Region "圧縮対象ファイル名の取得"

    ''' <summary>
    ''' 圧縮対象ファイル名の取得
    ''' </summary>
    ''' <param name="updateList">更新リスト</param>
    ''' <returns>作成対象ファイル名</returns>
    ''' <remarks></remarks>
    Private Function CreateArchiveTargetList(ByVal updateList As List(Of String)) As List(Of String)

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        Dim targetList = New List(Of String)

        For i As Integer = 0 To updateList.Count - 1

            Dim xDocument As XmlDocument = New XmlDocument
            Dim xRoot As XmlElement
            Dim xSousaKubun As XmlNodeList
            Dim xFilePath As XmlNodeList

            '履歴ファイルの読み込み
            xDocument.Load(updateList(i))

            'ルート要素の取得
            xRoot = xDocument.DocumentElement

            xSousaKubun = xRoot.GetElementsByTagName("FileAccess")
            xFilePath = xRoot.GetElementsByTagName("FilePath")

            '処理対象のファイル名称を取得
            For j As Integer = 0 To xSousaKubun.Count - 1
                If (xSousaKubun.Item(j).InnerText) <> "DELETE" Then
                    targetList.Add(xFilePath.Item(j).InnerText)
                    Logger.Info(ProgramIdLog + "ToArchive targetList.Add = " & xFilePath.Item(j).InnerText)
                End If
            Next
        Next

        'メソッド終了ログ出力
        Logger.Info(ProgramIdLog + "ToArchive targetList.count = " & targetList.Count)
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return targetList

    End Function
#End Region


#Region "作業用一時ディレクトリパスの取得"

    ''' <summary>
    ''' 作業用一時ディレクトリパスの取得
    ''' </summary>
    ''' <param name="archivePath">圧縮ファイル作成フォルダパス</param>
    ''' <returns>作成対象ファイル名</returns>
    ''' <remarks></remarks>
    Private Function GetwkPath(ByVal archivePath As String) As String

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("archivePath", archivePath, False))

        '作成予定のファイル名の取得
        GetwkPath = archivePath + DateTimeFunc.FormatDate(15, DateTimeFunc.Now) + "000"


        'メソッド終了ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetReturnParam(GetwkPath))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return GetwkPath

    End Function
#End Region

#Region "圧縮ファイル格納用、作業用一時ディレクトリの作成"

    ''' <summary>
    ''' 圧縮ファイル格納用、作業用一時ディレクトリの作成
    ''' </summary>
    ''' <param name="zipList">圧縮対象ファイル名</param>
    ''' <param name="tempPath">作業用一時ディレクトリ名</param>
    ''' <remarks></remarks>
    Private Sub CreatePath(ByVal zipList As String, ByVal tempPath As String)

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("zipList", zipList, False))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("tempPath", tempPath, False))

        Dim path1 As String = Path.GetDirectoryName(zipList)
        Dim zipPath As String = tempPath + "\" + path1

        'ディレクトリの作成
        If Not Directory.Exists(zipPath) Then
            Directory.CreateDirectory(zipPath)
        End If

        'メソッド終了ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub
#End Region

#Region "圧縮対象ファイルの移動"

    ''' <summary>
    ''' 圧縮対象ファイルを作業用一時ディレクトリへ移動
    ''' </summary>
    ''' <param name="tcvPath">TCVディレクトリ名</param>
    ''' <param name="tempPath">作業用一時ディレクトリ名</param>
    ''' <param name="zipList">圧縮対象ファイル名</param>
    ''' <remarks></remarks>
    Private Sub CreateTempFile(ByVal tcvPath As String, ByVal tempPath As String, ByVal zipList As String)

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("tempPath", tempPath, False))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("zipList", zipList, False))

        'ファイルを開く
        Using fs As New System.IO.FileStream(tcvPath + zipList, _
            System.IO.FileMode.Open, _
            System.IO.FileAccess.Read, _
            System.IO.FileShare.None)
            Dim bs(CInt(fs.Length - 1)) As Byte

            'ファイルを読み込む
            fs.Read(bs, 0, bs.Length)

            Dim fullPath As String = tempPath + "\" + zipList
            Using fs2 As System.IO.FileStream = System.IO.File.Create(fullPath)

                'ファイルの出力
                fs2.Write(bs, 0, bs.Length)

            End Using
        End Using
        'メソッド終了ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub
#End Region

#Region "圧縮ファイルの作成"

    ''' <summary>
    ''' 圧縮対象ファイル作成
    ''' </summary>
    ''' <param name="zipFileName">圧縮ファイル名</param>
    ''' <param name="tempPath">作業用一時ディレクトリ名</param>
    ''' <remarks></remarks>
    Private Sub CreateZipFile(ByVal zipFileName As String, ByVal tempPath As String)

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("zipFileName", zipFileName, False))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("tempPath", tempPath, False))

        Dim fastZip As New FastZip()

        '空のフォルダの圧縮許可
        fastZip.CreateEmptyDirectories = True
        'ZIP64での圧縮許可
        fastZip.UseZip64 = UseZip64.Off
        'サブディレクトリの圧縮許可
        Dim recurse As Boolean = True

        '圧縮ファイル作成
        fastZip.CreateZip(zipFileName, tempPath, recurse, Nothing, Nothing)

        'メソッド終了ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub
#End Region

#Region "圧縮ファイルの分割件数を求める"

    ''' <summary>
    ''' 作成した圧縮ファイルのサイズから分割件数を求める
    ''' </summary>
    ''' <param name="zipFilePath">圧縮ファイルフルパス</param>
    ''' <param name="syncDevideBlockSize">分割指定サイズ(バイト)</param>
    ''' <returns>分割件数</returns>
    ''' <remarks></remarks>
    Private Function getDevideCount(ByVal zipFilePath As String, ByVal syncDevideBlockSize As String) As Integer

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("zipFilePath", zipFilePath, False))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("syncDevideBlockSize", syncDevideBlockSize, False))

        Dim ret As Integer

        '圧縮ファイルの読み込み
        Using fs As New FileStream(zipFilePath, FileMode.Open, FileAccess.Read)

            Dim tmp As Double = CLng(fs.Length) / CLng(syncDevideBlockSize)

            ret = CInt(Math.Ceiling(tmp))

            'メソッド終了ログ出力
            Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetReturnParam(CStr(ret)))
            Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

            Return ret
        End Using

    End Function



#End Region

#Region "圧縮ファイルの読み込み"

    ''' <summary>
    ''' 作成した圧縮ファイルをbyte形式で読み込む
    ''' </summary>
    ''' <param name="zipFilePath">圧縮ファイルフルパス</param>
    ''' <param name="syncDevideNo">分割指定№</param>
    ''' <param name="syncDevideBlockSize">分割指定サイズ(KB)</param>
    ''' <returns>圧縮ファイル(byte形式)</returns>
    ''' <remarks></remarks>
    Private Function readZipFile(ByVal zipFilePath As String, ByVal syncDevideNo As String, ByVal syncDevideBlockSize As String) As Byte()

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("zipFilePath", zipFilePath, False))

        '圧縮ファイルの読み込み
        Using fs As New FileStream(zipFilePath, FileMode.Open, FileAccess.Read)

            Dim partNo = CLng(syncDevideNo) - 1
            Dim partSize = CLng(syncDevideBlockSize)

            '読み込み開始バイトを求める。
            Dim startPos As Long = partNo * partSize
            '読み込み終了バイトを求める。
            Dim endPos As Long = startPos + partSize

            '取得終了位置がファイルサイズを超えてしまう場合は、ファイルサイズを取得サイズとする。
            If endPos > fs.Length Then
                endPos = fs.Length
            End If

            '今回取得サイズを求める
            Dim retSize As Long = endPos - startPos
            Dim bs(CInt(retSize - 1)) As Byte

            fs.Seek(startPos, SeekOrigin.Begin)
            fs.Read(bs, 0, bs.Length)

            'メソッド終了ログ出力
            Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

            Return bs
        End Using

    End Function

#End Region

#Region "過去圧縮ファイルの削除"

    ''' <summary>
    ''' 過去の圧縮ファイルを削除する(記録用更新ファイルも削除)
    ''' </summary>
    ''' <param name="zipFilePath">圧縮ファイルフォルダパス</param>
    ''' <param name="archiveSaveDays">圧縮ファイル保持日数</param>
    ''' <remarks></remarks>
    Private Sub deleteZipFile(ByVal zipFilePath As String, ByVal archiveSaveDays As String)

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("zipFilePath", zipFilePath, False))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("archiveSaveDays", archiveSaveDays, False))

        If Not System.IO.Directory.Exists(zipFilePath) Then
            Return
        End If

        '本日日付の取得
        Dim toDay As Date = DateTimeFunc.Now()

        Dim deleteFileWildName As String

        deleteFileWildName = ARCHIVE_PREFIX + "*.zip"

        For Each stPath As String In System.IO.Directory.GetFileSystemEntries(zipFilePath, deleteFileWildName)

            '更新日の取得
            Dim dtCreate As DateTime = System.IO.File.GetLastWriteTime(stPath)

            '経過日数の算出
            Dim totalDays = CType(toDay.Subtract(dtCreate).TotalDays, Integer)

            'n日以上経過していれば
            If totalDays > CInt(archiveSaveDays) Then
                '圧縮ファイルの削除
                System.IO.File.Delete(stPath)
            End If

        Next stPath

        deleteFileWildName = ARCHIVE_PREFIX + "*.txt"

        For Each stPath As String In System.IO.Directory.GetFileSystemEntries(zipFilePath, deleteFileWildName)

            '更新日の取得
            Dim dtCreate As DateTime = System.IO.File.GetLastWriteTime(stPath)

            '経過日数の算出
            Dim totalDays = CType(toDay.Subtract(dtCreate).TotalDays, Integer)

            'n日以上経過していれば
            If totalDays > CInt(archiveSaveDays) Then
                '記録用更新ファイルの削除
                System.IO.File.Delete(stPath)
            End If
        Next stPath


        'メソッド終了ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))
    End Sub
#End Region

#Region "MD5取得処理"

    ''' <summary>
    ''' 圧縮ファイルのハッシュコードを取得する
    ''' </summary>
    ''' <param name="zipFilePath">圧縮ファイルフォルダパス</param>
    ''' <remarks></remarks>
    Private Function GetZipMD5(ByVal zipFilePath As String) As String

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("zipFilePath", zipFilePath, False))

        Dim result As New System.Text.StringBuilder()

        'ファイルを開く
        Using fs As New System.IO.FileStream( _
            zipFilePath, _
            System.IO.FileMode.Open, _
            System.IO.FileAccess.Read, _
            System.IO.FileShare.Read)

            'MD5CryptoServiceProviderオブジェクトを作成 
            Using md5 As New System.Security.Cryptography.MD5CryptoServiceProvider()

                'ハッシュ値を計算する 
                Dim bs As Byte() = md5.ComputeHash(fs)

                'byte型配列を16進数の文字列に変換 
                For Each b As Byte In bs
                    result.Append(b.ToString("x2", CultureInfo.CurrentCulture()))
                Next
            End Using

        End Using

        Dim ret As String = result.ToString
        'メソッド終了ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetReturnParam(ret))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return ret

    End Function
#End Region

#Region "記録用更新ファイルの作成"

    ''' <summary>
    ''' 記録用の更新ファイルを作成する
    ''' </summary>
    ''' <param name="tmpCheckCode">MD5チェックコード</param>
    ''' <param name="updateFilePath">更新ファイルパス</param>
    ''' <param name="writeFilePath">記録するファイル名</param>
    ''' <remarks></remarks>
    Private Sub writeUpdateFile(ByVal tmpCheckCode As String, ByVal updateFilePath As String, ByVal writeFilePath As List(Of String))

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("tmpCheckCode", tmpCheckCode, False))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("updateFilePath", updateFilePath, False))

        'ファイルの書き込み
        Using sw As New System.IO.StreamWriter(updateFilePath, _
                                                 True, _
                                                System.Text.Encoding.GetEncoding("UTF-8"))
            '先頭にMD5ハッシュコードを書き込み
            sw.Write(tmpCheckCode & vbNewLine)


            For i As Integer = 0 To writeFilePath.Count - 1

                Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("writeFilePath", writeFilePath(i), False))

                sw.Write(writeFilePath(i) & vbNewLine)
            Next

        End Using

        'メソッド終了ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))
    End Sub
#End Region

#Region "記録用更新ファイルの読み込み(チェックコードの取得)"

    ''' <summary>
    ''' 記録用の更新ファイルを読み込む
    ''' </summary>
    ''' <param name="updateFilePath">更新ファイルパス</param>
    ''' <remarks></remarks>
    Private Function readUpdateFileForCheckCode(ByVal updateFilePath As String) As String

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("updateFilePath", updateFilePath, False))

        Dim ret As String = ""

        'ファイルの書き込み
        Using cReader As New System.IO.StreamReader(updateFilePath, _
                                                System.Text.Encoding.GetEncoding("UTF-8"))

            '一行目のチェックコードのみ読み込む
            If cReader.Peek() >= 0 Then
                ret = cReader.ReadLine()
            End If

        End Using

        'メソッド終了ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetReturnParam(ret))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return ret
    End Function
#End Region

#Region "記録用更新ファイルの読み込み(更新対象ファイルの取得)"

    ''' <summary>
    ''' 記録用の更新ファイルを読み込む
    ''' </summary>
    ''' <param name="updateFilePath">更新ファイルパス</param>
    ''' <remarks></remarks>
    Private Function readUpdateFileForFileList(ByVal updateFilePath As String) As List(Of String)

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("updateFilePath", updateFilePath, False))

        Dim ret = New List(Of String)

        'ファイルの書き込み
        Using cReader As New System.IO.StreamReader(updateFilePath, _
                                                System.Text.Encoding.GetEncoding("UTF-8"))

            '一行目はチェックコードなので読みとばす
            If cReader.Peek() >= 0 Then
                cReader.ReadLine()
            End If

            ' 読み込みできる文字がなくなるまで繰り返す
            While (cReader.Peek() >= 0)
                ' ファイルを 1 行ずつ読み込む
                Dim stBuffer As String = cReader.ReadLine()
                ' 読み込んだものを追加で格納する
                ret.Add(stBuffer)
            End While

        End Using

        'メソッド終了ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetReturnParam(CStr(ret.Count)))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return ret
    End Function
#End Region

#Region "圧縮対象ファイルが存在するかのチェック"

    ''' <summary>
    ''' 記録用の更新ファイルを読み込む
    ''' </summary>
    ''' <param name="tcvPath">tcvフォルダパス</param>
    ''' <param name="archiveList">圧縮対象ファイル</param>
    ''' <remarks></remarks>
    Private Function checkExistArchiveFile(ByVal tcvPath As String, ByVal archiveList As List(Of String)) As Boolean

        'メソッド開始ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("archiveList count", CStr(archiveList.Count), False))

        Dim ret As Boolean = False

        For i As Integer = 0 To archiveList.Count - 1
            'ファイルの存在チェック
            If File.Exists(tcvPath + archiveList(i)) Then
                ret = True
                Exit For
            End If
        Next

        'メソッド終了ログ出力
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetReturnParam(CStr(ret)))
        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))


        Return ret
    End Function
#End Region


End Class
