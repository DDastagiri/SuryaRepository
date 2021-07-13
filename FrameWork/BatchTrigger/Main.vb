Imports System.Configuration
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Text
Imports System.Threading
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Configuration

Module Main

#Region "定数"
    ''' <summary>
    ''' アプリケーション設定セクション
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_APP_NAMESPACE As String = "Toyota.eCRB.SystemFrameworks"
    ''' <summary>
    ''' 起動オプション(2重起動禁止)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_OP_SINGLETHRED As String = "/s"
    ''' <summary>
    ''' パスの結合
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_PATH_JOIN As String = "\"
    ''' <summary>
    ''' 呼び出し拡張子
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DLL_EXT As String = ".dll"
    ''' <summary>
    ''' 起動先クラスの実装インタフェース
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_LOAD_INTERFACE As String = "IBatch"


    Private Const C_ERROR_CD As Integer = 100
#End Region

#Region "変数"
    Private _execApp As String = Nothing
    Private _paramList As New List(Of String)
    Private _single As Boolean = False
#End Region

#Region "Main()"
    Function Main() As Integer

        Dim mutex As System.Threading.Mutex = Nothing
        Dim isCreate As Boolean = False
        Dim result As Integer = C_ERROR_CD

        ''共通基盤のロード処理
        Try
            _GetApplicationName()
            _SetSystemConfiguration()
            _GetCommandLineArgs()
            LoggerUtility.Setlog4netSettingPrameter(String.Empty, _execApp, String.Empty)

        Catch ex As Exception
            Dim message As String = "Invalid configuration"
            Console.Error.WriteLine(message & "(" & ex.Message & ")")
            Return C_ERROR_CD
        End Try

        Try
            Logger.Info("Start Batch:" & _execApp)

            ''多重起動チェック
            If _single Then
                mutex = New System.Threading.Mutex(True, _execApp, isCreate)
                If Not isCreate Then
                    Dim message As String = _execApp & " is already running."
                    Logger.Warn(message)
                    Console.Error.WriteLine(message)
                    Return C_ERROR_CD
                End If
            End If

            Dim dllPath As New StringBuilder
            dllPath.Append(_GetAppPath())
            dllPath.Append(C_PATH_JOIN)
            dllPath.Append("Toyota.eCRB.iCROP.Batch." & _execApp)
            dllPath.Append(C_DLL_EXT)

            Dim asm As Assembly
            Try
                asm = Assembly.LoadFile(dllPath.ToString)
            Catch ex As Exception
                Dim message As String = "Couldn't load " & dllPath.ToString()
                Logger.Error(message, ex)
                Console.Error.WriteLine(message & "(" & ex.Message & ")")
                Return C_ERROR_CD
            End Try

            Dim className As String = String.Empty
            For Each type In asm.GetTypes
                If type.GetInterface(C_LOAD_INTERFACE) IsNot Nothing Then
                    className = type.FullName
                    Exit For
                End If
            Next type
            If (String.IsNullOrEmpty(className)) Then
                Dim message As String = String.Format(CultureInfo.InvariantCulture, "No {0} implementation found in {1}.", C_LOAD_INTERFACE, dllPath.ToString())
                Logger.Error(message)
                Console.Error.WriteLine(message)
                Return C_ERROR_CD
            End If

            Try
                Dim library As IBatch = DirectCast(asm.CreateInstance(className), IBatch)
                result = library.Execute(_paramList.ToArray())
            Catch ex As Exception
                Dim message As String = String.Format(CultureInfo.InvariantCulture, "{0} threw exception. ({1})", className, dllPath.ToString())
                Logger.Error(message, ex)
                Console.Error.WriteLine(message & "(" & ex.Message & ")")
                Return C_ERROR_CD
            End Try

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "End Batch:{0} ({1})", _execApp, result))

        Catch ex As Exception
            Dim message As String = "Unexpected error occured."
            Console.Error.WriteLine(message & "(" & ex.Message & ")")
            Try
                Logger.Error(message, ex)
            Catch ex2 As Exception
                Console.Error.WriteLine(ex2.ToString())
            End Try
            Return C_ERROR_CD

        Finally
            If (mutex IsNot Nothing) Then
                mutex.ReleaseMutex()
                mutex.Dispose()
            End If
        End Try

        Return result

    End Function
#End Region

#Region "_GetApplicationName()"
    Private Sub _GetApplicationName()

        Dim appPath As String = Assembly.GetEntryAssembly.Location
        _execApp = Path.GetFileNameWithoutExtension(appPath)

    End Sub
#End Region

#Region "_SetSystemConfiguration()"

    Private Sub _SetSystemConfiguration()

        Dim sysConfig As SystemConfiguration = SystemConfiguration.Current

        Dim section As System.Xml.XmlElement = DirectCast(System.Configuration.ConfigurationManager.GetSection(C_APP_NAMESPACE), System.Xml.XmlElement)
        If section Is Nothing Then
            Throw New FileNotFoundException(C_APP_NAMESPACE & " section was not found.")
        End If

        sysConfig.Manager = New Toyota.eCRB.SystemFrameworks.Configuration.ConfigurationManager(section)
        sysConfig.SetRuntimeSetting(SystemConfigurationType.ApplicationType, CStr(ApplicationType.Batch))
        sysConfig.SetRuntimeSetting(SystemConfigurationType.ApplicationId, _execApp)
        _SetConnectionStringsItem(sysConfig, SystemConfigurationType.iCROPConnectionString, "My.MySettings.ConnectionString")
        _SetConnectionStringsItem(sysConfig, SystemConfigurationType.DMSConnectionString, "My.MySettings.2ndConnectionString")

    End Sub

    Private Sub _SetConnectionStringsItem(ByVal sysConfig As SystemConfiguration, ByVal type As SystemConfigurationType, ByVal name As String)
        Dim value As Object = System.Configuration.ConfigurationManager.ConnectionStrings.Item(name)
        If (value Is Nothing) Then
            Throw New FileNotFoundException(String.Format(CultureInfo.InvariantCulture, "[{0}] is not defined.", name))
        End If

        sysConfig.SetRuntimeSetting(type, value.ToString())
    End Sub

#End Region

#Region "_GetCommandLineArgs()"
    Private Sub _GetCommandLineArgs()

        For Each arg As String In My.Application.CommandLineArgs

            Dim add As Boolean = True

            If arg.Equals(C_OP_SINGLETHRED, StringComparison.OrdinalIgnoreCase) Then
                _single = True
                add = False
            End If

            If add Then
                _paramList.Add(arg)
            End If

        Next arg

    End Sub

#End Region

#Region "_GetAppPath() As String"
    Private Function _GetAppPath() As String

        Return My.Application.Info.DirectoryPath.ToString()

    End Function
#End Region

End Module
