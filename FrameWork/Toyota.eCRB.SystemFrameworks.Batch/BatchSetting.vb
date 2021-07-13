Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Batch.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace Toyota.eCRB.SystemFrameworks.Batch

    Friend Class UnsafeNativeMethods

        Private Sub New()

        End Sub

        Friend Declare Function GetPrivateProfileString _
        Lib "kernel32.DLL" Alias "GetPrivateProfileStringA" (lpApplocationName As String, _
                                                 ByVal lpKeyName As String, _
                                                 ByVal lpDefault As String, _
                                                 ByVal lpReturnedString As String, _
                                                 ByVal nSize As UInt32, _
                                                 ByVal lpFileName As String) As UInt32

        Friend Declare Function GetPrivateProfileSectionNames _
        Lib "kernel32.DLL" Alias "GetPrivateProfileSectionNamesA" (ByVal lpszReturnBuffer As String, _
                                                         ByVal nSize As UInt32, _
                                                         ByVal lpFileName As String) As UInt32
    End Class

    ''' <summary>
    ''' 設定値を提供します。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class BatchSetting

        ''' <summary>
        ''' 現在要求されているプログラムIDの設定値を取得します。
        ''' </summary>
        ''' <param name="section">セレクション</param>
        ''' <param name="key">パラメータ</param>
        ''' <param name="defaultValue">値</param>
        ''' <returns>
        ''' 該当した設定値<br/>
        ''' 該当する設定値が存在しない場合、defaultValueが設定されていなければString.Emptyを返し、<br/>
        ''' defaultValueが設定されていれば、defaultValueを返却します。<br/>
        ''' </returns>
        ''' <remarks></remarks>
        Public Shared Function GetValue(ByVal section As String, ByVal key As String, Optional ByVal defaultValue As String = Nothing) As String
            Return BatchSettingManager.GetSettingValue(GetCurrntID(), section, key, defaultValue)
        End Function

        ''' <summary>
        ''' 共通の設定値を取得します
        ''' </summary>
        ''' <param name="section">セレクション</param>
        ''' <param name="key">パラメータ</param>
        ''' <param name="defaultValue">値</param>
        ''' <returns>共通の設定値</returns>
        ''' 該当する設定値が存在しない場合、defaultValueが設定されていなければString.Emptyを返し、<br/>
        ''' defaultValueが設定されていれば、defaultValueを返却します。<br/>
        ''' <remarks></remarks>
        Public Shared Function GetCommonValue(ByVal section As String, ByVal key As String, Optional ByVal defaultValue As String = Nothing) As String
            Return BatchSettingManager.GetSettingValue("XXXXXXXXX", section, key, defaultValue)
        End Function

        ''' <summary>
        ''' 現在の要求にたいするプログラムIDを取得します。
        ''' </summary>
        ''' <returns>機能ID</returns>
        ''' <remarks></remarks>
        Private Shared Function GetCurrntID() As String

            'Dim fullPath As String = System.Reflection.Assembly.GetExecutingAssembly().Location
            Dim fullPath As String = System.Reflection.Assembly.GetEntryAssembly().Location
            Dim id As String
            Dim index As Integer = fullPath.IndexOf("MC", StringComparison.OrdinalIgnoreCase)
            id = fullPath.Substring(index, 9)

            Return id

        End Function

        ''' <summary>
        ''' インスタンスの生成をできないようにするためのデフォルトのコンストラクタです。
        ''' </summary>
        ''' <remarks>
        ''' </remarks>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' プログラム設定の取得、および管理を行うクラスです。
        ''' </summary>
        ''' <remarks></remarks>
        Private Class BatchSettingManager
            ''' <summary>
            ''' 設定管理　iniファイル
            ''' KEY:画面ID VALUE(KEY:プログラムID VALUE:(KEY:セクション VALUE:(KEY:キー VALUE:値)))の連想配列
            ''' </summary>
            ''' <remarks></remarks>
            Private Shared _settingTable As Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, String)))

            ''' <summary>
            ''' 設定管理　DB値
            ''' KEY:画面ID VALUE(KEY:プログラムID VALUE:(KEY:セクション VALUE:(KEY:キー VALUE:値)))の連想配列
            ''' </summary>
            ''' <remarks></remarks>
            Private Shared _settingDbTable As Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, String)))

            ''' -----------------------------------------------------------------------------
            ''' <summary>
            ''' 共通iniファイルの名称
            ''' </summary>
            ''' -----------------------------------------------------------------------------
            Private Const COMMON_FILE_NAME As String = "common"

            ''' -----------------------------------------------------------------------------
            ''' <summary>
            ''' 共通ID
            ''' </summary>
            ''' -----------------------------------------------------------------------------
            Private Const COMMON_ID As String = "XXXXXXXXX"

            ''' -----------------------------------------------------------------------------
            ''' <summary>
            ''' iniファイルの拡張子
            ''' </summary>
            ''' -----------------------------------------------------------------------------
            Private Const FILE_EXTENSION As String = ".ini"

            ''' <summary>
            ''' インスタンスの生成をできないようにするためのデフォルトのコンストラクタです。
            ''' </summary>
            ''' <remarks>
            ''' このクラスはインスタンスを生成できません。静的メソッドを呼び出してください。
            ''' </remarks>
            Private Sub New()
            End Sub

            ''' <summary>
            ''' 引数で指定されたプログラムID、セクション、キーに該当する設定値を取得します
            ''' </summary>
            ''' <param name="programID">プログラムID</param>
            ''' <param name="section">セクション</param>
            ''' <param name="key">キー</param>
            ''' <param name="defaultValue"></param>
            ''' <returns>
            ''' 該当する文言が存在した場合、その文言。<br />
            ''' 存在しなかった場合はString.Emptyを返却します。
            ''' </returns>
            ''' <remarks></remarks>
            Friend Shared Function GetSettingValue(ByVal programID As String, ByVal section As String, ByVal key As String, Optional ByVal defaultValue As String = Nothing) As String

                Dim rtnStr As String = String.Empty
                If Not defaultValue Is Nothing Then
                    rtnStr = defaultValue
                End If

                If _settingTable Is Nothing Then
                    _settingTable = New Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, String)))
                End If

                If _settingDbTable Is Nothing Then
                    _settingDbTable = New Dictionary(Of String, Dictionary(Of String, Dictionary(Of String, String)))
                End If

                If Not _settingTable.ContainsKey(programID) Then
                    'iniファイルの設定値取得
                    GetFileData(COMMON_FILE_NAME)
                    If Not COMMON_ID.Equals(programID) Then
                        GetFileData(programID)
                    End If
                End If

                If _settingTable.ContainsKey(programID) Then
                    If _settingTable(programID).ContainsKey(section) Then
                        If _settingTable(programID)(section).ContainsKey(key) Then
                            Return _settingTable(programID)(section).Item(key)
                        End If
                    End If
                End If

                If Not _settingDbTable.ContainsKey(programID) Then
                    'DBの設定値取得
                    GetDbData(programID)
                End If

                If _settingDbTable.ContainsKey(programID) Then
                    If _settingDbTable(programID).ContainsKey(section) Then
                        If _settingDbTable(programID)(section).ContainsKey(key) Then
                            Return _settingDbTable(programID)(section).Item(key)
                        End If
                    End If
                End If

                Return rtnStr.Trim()
            End Function

            ''' <summary>
            ''' iniファイルの設定値を取得します。
            ''' </summary>
            ''' <param name="programID">プログラムID OR common</param>
            ''' <remarks></remarks>
            Private Shared Sub GetFileData(ByVal programID As String)

                '設定ファイルのパスを取得
                Dim base As String = System.AppDomain.CurrentDomain.BaseDirectory
                Dim filePath As New System.Text.StringBuilder
                filePath.Append(base)
                filePath.Append(programID)
                filePath.Append(FILE_EXTENSION)

                'iniファイルが存在する場合
                If File.Exists(filePath.ToString()) = True Then
                    Dim dic As New Dictionary(Of String, Dictionary(Of String, String))

                    'iniファイルのsectionをすべて取得
                    Dim sections As String() = Nothing
                    Dim buffer As String = Space(1024)
                    Dim sectionCnt As UInt32 = UnsafeNativeMethods.GetPrivateProfileSectionNames(buffer, 256, filePath.ToString())
                    If Not sectionCnt = UInt32.MinValue Then
                        sections = buffer.ToString().Split(Char.MinValue)
                    End If

                    '取得したsectionのキーをすべて取得し、キーに紐づく値を取得
                    Dim keyBuffer As String
                    Dim valBuffer As String
                    Dim keys As String() = Nothing
                    For Each sel In sections
                        keyBuffer = Space(1024)
                        Dim keyCnt As UInt32 = UnsafeNativeMethods.GetPrivateProfileString(sel, Nothing, "", keyBuffer, 256, filePath.ToString())
                        If Not keyCnt = UInt32.MinValue Then
                            keys = keyBuffer.ToString().Split(Char.MinValue)
                            dic.Add(sel, New Dictionary(Of String, String))
                            For Each k In keys
                                valBuffer = Space(1024)
                                Dim val As UInt32 = UnsafeNativeMethods.GetPrivateProfileString(sel, k, "", valBuffer, 256, filePath.ToString())
                                If Not val = UInt32.MinValue Then
                                    dic(sel).Add(k, valBuffer.ToString().Trim())
                                End If
                            Next
                        End If
                    Next

                    If Not _settingTable.ContainsKey(programID) Then
                        _settingTable.Add(programID, dic)
                    End If
                End If
            End Sub

            ''' <summary>
            ''' DBから設定値を取得します。
            ''' </summary>
            ''' <param name="programID">プログラムID OR common</param>
            ''' <remarks></remarks>
            Private Shared Sub GetDbData(ByVal programID As String)

                '格納テーブル
                Dim dt As ProgramSettingDataSet.ProgramSettingTableDataTable = ProgramSettingTableAdapter.GetProgramSettingTableByProgramId(programID)

                Dim lastProgramId As String = String.Empty
                Dim lastSection As String = String.Empty
                Dim dic As New Dictionary(Of String, Dictionary(Of String, String))
                Dim keyDic As New Dictionary(Of String, String)
                Dim cnt As Integer = 1

                For Each dr As ProgramSettingDataSet.ProgramSettingTableRow In dt.Rows
                    If lastSection.Equals(dr.SECTION) Then
                        keyDic.Add(dr.KEY, dr.VALUE)
                    ElseIf cnt = 1 Then
                        keyDic.Add(dr.KEY, dr.VALUE)
                    Else
                        dic.Add(lastSection, keyDic)
                        keyDic = New Dictionary(Of String, String)
                        keyDic.Add(dr.KEY, dr.VALUE)
                    End If

                    If dt.Rows.Count = cnt Then
                        dic.Add(dr.SECTION, keyDic)
                        _settingDbTable.Add(dr.PROGRAMID, dic)
                    End If

                    If Not lastProgramId.Equals(dr.PROGRAMID) And Not cnt = 1 Then
                        _settingDbTable.Add(lastProgramId, dic)
                        dic = New Dictionary(Of String, Dictionary(Of String, String))
                    End If

                    cnt = cnt + 1
                    lastProgramId = dr.PROGRAMID
                    lastSection = dr.SECTION
                Next

                dt.Dispose()

            End Sub

        End Class
    End Class

End Namespace

