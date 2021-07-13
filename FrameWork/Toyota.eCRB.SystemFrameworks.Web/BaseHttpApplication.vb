Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Globalization
Imports System.Web
Imports System.Web.SessionState
Imports System.IO
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web.Security
Imports System.Diagnostics
Imports System.Security.Principal
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Reflection
Imports System.Web.Caching
Imports System.Web.Configuration
Imports System.Threading
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Configuration

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' e-CRB Framework�p��ASP.NET�p�C�v���C�����������������N���X�ł��B
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Public Class BaseHttpApplication
        Inherits System.Web.HttpApplication

        ''' <summary>
        ''' �A�v���P�[�V�����ݒ�Z�N�V����
        ''' </summary>
        ''' <remarks></remarks>
        Private Const APP_NAMESPACE As String = "Toyota.eCRB.SystemFrameworks"

        ''' <summary>
        ''' �ő�W��T�[�o�[���̔��X���ʎq
        ''' </summary>
        Private Const MAX_INDIVIDUAL_ID As Byte = 9

        ''' <summary>
        ''' 500�G���[
        ''' </summary>
        Private Const WEB_500ERROR As String = "500"

        ''' <summary>
        ''' 500�G���[
        ''' </summary>
        Private Const LOCAL_IPADDRESS As String = "127.0.0.1"

        ''' <summary>
        ''' �N����N���X�̎����C���^�t�F�[�X
        ''' </summary>
        ''' <remarks></remarks>
        Private Const LOGIN_HOOK_INTERFACE As String = "ILoginHook"

        ''' <summary>
        ''' �������������ʔ���t���O
        ''' </summary>
        Private Shared _startupResult As Boolean = False

        ''' <summary>
        ''' ��������O
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _startupException As Exception

        ''' <summary>
        ''' ��������O
        ''' </summary>
        Public Shared ReadOnly Property StartupException As Exception
            Get
                Return _startupException
            End Get
        End Property

        ''' <summary>
        ''' Web�A�v���P�[�V�����N�����̍ŏ��̃C�x���g�Ƃ��Ĕ������܂��B
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
            Try
                Logger.TraceOff = True
                _startupResult = False

                _SetSystemConfiguration()

                ''LoggerUtility.Setlog4netSettingPrameter(String.Empty, SystemConfiguration.Current.GetRuntimeSetting(SystemConfigurationType.ApplicationId), String.Empty)

                '�t�b�N��荞��
                _LoadHooks()

                ' �A�v���P�[�V�������ʂŕK�v�ȃf�[�^��ǂݍ���
                ApplicationDataLoader.Load()

                ''SQLResponseLog�o�͗p�X���b�h����
                'If SqlResponseLogger.IsDebug() Then
                '    Dim t As New Thread(AddressOf SqlResponseLogger.WaitThread)
                '    t.Start()
                'End If

                _startupResult = True
            Catch ex As Exception
                _startupException = ex
                Try
                    Logger.Error("WebApp initialization failed.", ex)
                Catch ex2 As Exception
                    '�ݒ�t�@�C���̓��e�ɕs��������ꍇ�A���O�o�͂Ɏ��s����\��������
                End Try
            Finally
                Logger.TraceOff = False
            End Try
        End Sub


        ''' <summary>
        ''' ASP.NET ���v���ɉ�������Ƃ��ɁA
        ''' ���s�� HTTP �p�C�v���C�� �`�F�C���̍ŏ��̃C�x���g�Ƃ��Ĕ������܂��B
        ''' </summary>
        ''' <param name="sender">�C�x���g�𔭐��������N���X�̃C���X�^���X</param>
        ''' <param name="e">�C�x���g�f�[�^�B�C�x���g�������\�b�h�̍쐬���[������A��`���K�v�Ȉ����ł��B���O�ғ��o�͂ł͗��p���Ă��܂���</param>
        ''' <remarks>
        ''' </remarks>
        Private Sub BaseHttpApplication_BeginRequest( _
                                        ByVal sender As Object, _
                                        ByVal e As System.EventArgs) Handles Me.BeginRequest


            '���݂�URL�p�X���擾���A���[�J���ϐ�pathEnds�Ɋi�[����B
            Dim pathEnds As String = Me.Context.Request.Path

            If Not _CheckApplicationIsReady() Then
                '�A�v���P�[�V�����̏��������s�i�G���[��ʂɃ��_�C���N�g�j
                Dim reqUrl As String = Request.AppRelativeCurrentExecutionFilePath
                If Not reqUrl.StartsWith("~/Error/", StringComparison.OrdinalIgnoreCase) AndAlso _
                    pathEnds.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase) Then
                    Response.Redirect("~/Error/SC3010304.aspx")
                End If
                Return
            End If

            If (pathEnds.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase)) OrElse _
               (pathEnds.EndsWith(".asmx", StringComparison.OrdinalIgnoreCase)) OrElse _
               (pathEnds.EndsWith(".atsc", StringComparison.OrdinalIgnoreCase)) Then

                '�R���e�L�X�g�ɃA�v��ID�ƃA�N�Z�XURL���i�[����B
                LoggerWebUtility.SetAccessUrlInfo(Me.Context)
            End If

        End Sub

        ''' <summary>
        ''' ASP.NET ���C�x���g �n���h�� (�y�[�W�AXML Web �T�[�r�X�Ȃ�) �̎��s���J�n���钼�O�ɔ������܂��B
        ''' �ғ����O�o�͋@�\�ł́A���̃��\�b�h�ŉғ����O(�����J�n)�̏o�͏������s���܂��B
        ''' </summary>
        ''' <param name="sender">�C�x���g�̔������B</param>
        ''' <param name="e">�C�x���g�ɌŗL�̃f�[�^�B</param>
        ''' <remarks>���̃C�x���g�n���h���ł́AHTTP���N�G�X�g���̃��O�̏o�͂��s���܂��B</remarks>
        Private Sub BaseHttpApplication_PreRequestHandlerExecute( _
                                        ByVal sender As Object, _
                                        ByVal e As System.EventArgs) Handles Me.PreRequestHandlerExecute

            '--��Q���O--
            '1.���݂�URL�p�X���擾���A�������ɕϊ����ă��[�J���ϐ�pathEnds�Ɋi�[����B
            Dim pathEnds As String = Me.Context.Request.Path

            If Not _CheckApplicationIsReady() Then
                Return
            End If

            '2.pathEnds�̖��������񔻒�����{()
            '2.1.���������񂪁u.aspx�v�������́u.asmx�v�������́u.atsc�v�̏ꍇ
            If (pathEnds.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase)) OrElse _
               (pathEnds.EndsWith(".asmx", StringComparison.OrdinalIgnoreCase)) OrElse _
               (pathEnds.EndsWith(".atsc", StringComparison.OrdinalIgnoreCase)) Then

                '2.1.1.���O�C��ID���i�[���郍�[�J���ϐ����`����B
                Dim loginId As String

                '2.1.2.���݂̌������i�[���郍�[�J���ϐ����`����B
                Dim selectedRole As String = Nothing

                '2.1.3.���[�U��񂪃Z�b�V�����ɑ��݂��邩�m�F����B
                If System.Web.HttpContext.Current.Session IsNot Nothing AndAlso _
                    Me.Session(StaffContext.SESSION_KEY) IsNot Nothing Then

                    '2.1.3.1.���[�U��񂪃Z�b�V�����ɑ��݂���ꍇ
                    '2.1.3.1.1.���[�U�����擾����B
                    Dim staff As StaffContext = StaffContext.Current

                    '2.1.3.1.2.���O�C��ID���擾���A���[�J���ϐ�loginId�Ɋi�[����B
                    loginId = staff.Account

                    ''2.1.3.1.3.���݂̌������擾���ANothing�Ŗ����ꍇ�AString�Ɍ^�ϊ�����
                    ''  ���[�J���ϐ�selectedRole �Ɋi�[����B
                    'If staff.Account IsNot Nothing Then
                    '    selectedRole = Format(staff.Account, "00")
                    'End If
                    selectedRole = CStr(staff.OpeCD)
                Else
                    '2.1.3.2.���[�U��񂪃Z�b�V�����ɑ��݂��Ȃ��ꍇ

                    Dim urlTokens As String() = Me.Context.Request.Url.AbsolutePath.Split(New Char() {"/"c})
                    If (2 <= urlTokens.Length AndAlso urlTokens(urlTokens.Length - 2).Equals("Pages")) Then
                        'Pages�z���̃y�[�W��v�������ꍇ�́A���O�C���y�[�W�Ƀ��_�C���N�g������
                        Dim page As UI.Control = CType(HttpContext.Current.Handler, UI.Control)
                        Logger.Error("Session timeout occured. (redirected to login page)")
                        
                        '2014/09/03 TMEJ ���V IT9745_NextSTEP�T�[�r�X �T�[�r�X�Ɩ������]���p�A�v���̃V�X�e���e�X�g START

                        'Me.Context.Response.Redirect(page.ResolveClientUrl(CStr(SystemConfiguration.Current.Manager.ScreenUrl.GetSetting(String.Empty).GetValue("Login"))))

                        '�[�����ϐ�
                        Dim userAgentType As String = String.Empty

                        'Config�ϐ�
                        Dim tecnitianConfig As ConfigurationManager = SystemConfiguration.Current.Manager

                        'Config��UserAgent�̐ݒ�l�������[�v
                        For Each userAgent As Item In tecnitianConfig.LoginManager.GetSetting("UserAgent").Item
                            '�ݒ�l�̃f�[�^�擾
                            Dim userAgentRegEx As New Regex(userAgent.Value)

                            '���O�C���[���Ɛݒ�l�̃f�[�^�`�F�b�N
                            If (userAgentRegEx.IsMatch(HttpContext.Current.Request.UserAgent)) Then
                                '��v����ꍇ

                                '�ݒ�l����ݒ�
                                userAgentType = userAgent.Name

                                '���[�v�I��
                                Exit For

                            End If

                        Next

                        '���O�C������ƃ��O�C���[���̃`�F�b�N
                        If (String.Equals("iPod", userAgentType) OrElse String.Equals("iPhone", userAgentType)) Then
                            '���O�C���[�����uiPod�vor�uiPhone�v�̏ꍇ

                            'GK�̃��O�C���y�[�W��ݒ�
                            Me.Context.Response.Redirect(page.ResolveClientUrl(CStr(SystemConfiguration.Current.Manager.ScreenUrl.GetSetting(String.Empty).GetValue("GK_Login"))))

                        Else
                            '��L�ȊO�̏ꍇ

                            'WEB�̃��O�C���y�[�W��ݒ�
                            Me.Context.Response.Redirect(page.ResolveClientUrl(CStr(SystemConfiguration.Current.Manager.ScreenUrl.GetSetting(String.Empty).GetValue("WEB_Login"))))

                        End If
                        
                        '2014/09/03 TMEJ ���V IT9745_NextSTEP�T�[�r�X �T�[�r�X�Ɩ������]���p�A�v���̃V�X�e���e�X�g END

                        Return
                    Else
                        '2.1.3.2.1.Http�w�b�_����A���O�C��ID���擾����API���Ăяo���B
                        loginId = LoggerWebUtility.GetLoginIdFromHttpHeader(Me.Context)
                    End If
                End If

                '2.1.4.LoggerWebUtility.SetUserInfo���\�b�h���Ăяo���B
                LoggerWebUtility.SetUserInfo(Me.Context, loginId, selectedRole)
            End If
            '2.2.���̑��̏ꍇ
            ' �����Ȃ�
            '-- ��Q���O--
        End Sub

        ''' <summary>
        ''' ��������Ȃ���O���X���[�����Ɣ������܂��B 
        ''' </summary>
        ''' <param name="sender">�C�x���g�̔������B</param>
        ''' <param name="e">�C�x���g�ɌŗL�̃f�[�^�B</param>
        ''' <remarks>
        ''' ���̃C�x���g�n���h���ł́A��O�������O�ɏo�͂��܂��B
        ''' </remarks>
        Private Sub BaseHttpApplication_Error( _
                                              ByVal sender As Object, _
                                              ByVal e As System.EventArgs) Handles Me.Error

            If Not _CheckApplicationIsReady() Then
                Return
            End If

            ' �n���h������Ă��Ȃ��G���[�����������Ƃ��Ɏ��s����R�[�h�ł�
            Dim ex As Exception = Server.GetLastError
            If (TypeOf ex Is HttpUnhandledException) Then
                ex = ex.InnerException
            End If

            Dim guid As String = System.Guid.NewGuid().ToString

            Try
                Dim controlName As New Text.StringBuilder()
                controlName.Append(vbCrLf)
                For Each ctlName As String In Request.Form
                    If (ctlName IsNot Nothing) AndAlso (ctlName.IndexOf("__", StringComparison.OrdinalIgnoreCase) < 0) Then
                        With controlName
                            .Append(ctlName.Replace("ctl00$", "").Replace("ContentPlaceHolder1$", ""))
                            .Append(":")
                            .Append(Request.Params(ctlName).ToString())
                            .Append(";")
                        End With
                    End If
                Next

                If (TypeOf ex Is HttpException) Then
                    Dim httpEx As HttpException = CType(ex, HttpException)
                    If (httpEx.InnerException IsNot Nothing) Then
                        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0} ([{1}] {2})", guid, httpEx.GetHttpCode(), httpEx.Message) & vbCrLf & controlName.ToString(), httpEx)
                    Else
                        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0} ([{1}] {2})", guid, httpEx.GetHttpCode(), httpEx.Message) & vbCrLf & controlName.ToString(), httpEx.InnerException)
                    End If
                Else
                    Logger.Error(guid & vbCrLf & controlName.ToString(), ex)
                End If
                'If (TypeOf ex Is HttpException) Then
                '    Dim httpEx As HttpException = CType(ex, HttpException)
                '    If (httpEx.InnerException IsNot Nothing) Then
                '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0} ([{1}] {2})", guid, httpEx.GetHttpCode(), httpEx.Message) & vbCrLf & controlName.ToString(), httpEx)
                '    Else
                '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0} ([{1}] {2})", guid, httpEx.GetHttpCode(), httpEx.Message) & vbCrLf & controlName.ToString(), httpEx.InnerException)
                '    End If
                'Else
                '    Logger.Error(guid & vbCrLf & controlName.ToString(), ex)
                'End If

            Catch exp As Exception
                Logger.Error(guid, exp)
            End Try

            'HttpContext.Current.Items(APPLICATION_ERROR_ID) = guid

            _RedirectErrorPage(guid)
        End Sub

        ''' <summary>
        ''' ASP.NET �C�x���g �n���h���[ (�y�[�W�AXML Web �T�[�r�X�Ȃ�) �̎��s����������Ɣ������܂��B
        ''' </summary>
        ''' <param name="sender">�C�x���g�̔������B</param>
        ''' <param name="e">�C�x���g�ɌŗL�̃f�[�^�B</param>
        ''' <remarks></remarks>
        Private Sub BaseHttpApplication_PostRequestHandlerExecute(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PostRequestHandlerExecute

            '���݂�URL�p�X���擾���A�������ɕϊ����ă��[�J���ϐ�pathEnds�Ɋi�[����B
            Dim pathEnds As String = Me.Context.Request.Path

            If Not _CheckApplicationIsReady() Then
                Return
            End If

            '--���k�`��--
            'pathEnds�̖��������񔻒�����{
            '���������񂪁u.aspx�v�̏ꍇ
            '���AMe.Response.ContentType��Nothing�łȂ��ꍇ
            '���AMe.Response.ContentType��"image/"�Ŏn�܂�Ȃ��ꍇ
            If pathEnds.EndsWith(".aspx", StringComparison.OrdinalIgnoreCase) _
                AndAlso Not String.IsNullOrEmpty(Me.Response.ContentType) _
                AndAlso Not Me.Response.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase) Then
                '�v���E�U����M�\�ȃG���R�[�h�������擾����B
                Dim encoding As String = Me.Request.Headers.Get("Accept-Encoding")
                'Nothing�ł͖����ꍇ
                If encoding IsNot Nothing Then
                    'gzip���k�`�����܂ޏꍇ
                    If encoding.Contains("gzip") Then
                        'gzip���k�`����ݒ肷��B
                        Me.Response.Filter = New Compression.GZipStream(Me.Response.Filter, Compression.CompressionMode.Compress)
                        'HTTP�w�b�_�Ɉ��k�`����ݒ肷��B
                        Me.Response.AppendHeader("Content-Encoding", "gzip")
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' �A�v���P�[�V�����̏I�����ɌĂяo����܂��B
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
            If Not _CheckApplicationIsReady() Then
                Return
            End If

            If SqlResponseLogger.IsDebug() Then
                SqlResponseLogger.Debug(1)
            End If
        End Sub


        Private Function _CheckApplicationIsReady() As Boolean
            Return _startupResult
        End Function

        Private Sub _SetSystemConfiguration()

            Dim sysConfig As SystemConfiguration = SystemConfiguration.Current
            Dim section As System.Xml.XmlElement = DirectCast(System.Web.Configuration.WebConfigurationManager.GetSection(APP_NAMESPACE), System.Xml.XmlElement)
            If section Is Nothing Then
                Throw New FileNotFoundException(APP_NAMESPACE & " section was not found.")
            End If

            sysConfig.Manager = New Toyota.eCRB.SystemFrameworks.Configuration.ConfigurationManager(section)
            sysConfig.SetRuntimeSetting(SystemConfigurationType.ApplicationType, CStr(ApplicationType.Web))
            sysConfig.SetRuntimeSetting(SystemConfigurationType.ApplicationId, String.Empty)
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

        Private Sub _RedirectErrorPage(ByVal guid As String)
            'Private Sub _RedirectErrorPage()

            'Dim query As String = "?aspxerrorpath=" & HttpUtility.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath)
            Dim query As New StringBuilder
            query.Append("?aspxerrorpath=" & HttpUtility.UrlEncode(HttpContext.Current.Request.Url.AbsolutePath))
            query.Append("&apperrid=" & HttpUtility.UrlEncode(guid))

            'web.config����J�X�^���G���[�y�[�W�̏����擾����
            Dim section As Object = WebConfigurationManager.GetSection("system.web/customErrors")
            If section Is Nothing Then
                Return
            End If

            Dim errorSection As CustomErrorsSection = DirectCast(section, CustomErrorsSection)
            If errorSection.Mode = CustomErrorsMode.Off Then
                Return
            ElseIf errorSection.Mode = CustomErrorsMode.RemoteOnly Then
                If HttpContext.Current.Request.UserHostAddress.Equals(LOCAL_IPADDRESS) Then
                    Return
                End If
            End If

            Dim redirectUrl As String
            '500�G���[�p�̃J�X�^���G���[�y�[�W���ݒ肳��Ă��邩����
            If errorSection.Errors.Item(WEB_500ERROR) IsNot Nothing Then
                '�ݒ肠��
                redirectUrl = errorSection.Errors.Item(WEB_500ERROR).Redirect
            Else
                '�ݒ�Ȃ�
                redirectUrl = errorSection.DefaultRedirect
            End If

            '�ϊ�����URL��ԋp
            'HttpContext.Current.Server.Transfer(redirectUrl & query)
            HttpContext.Current.Response.Redirect(redirectUrl & query.ToString)
        End Sub

        ''' <summary>
        ''' �t�b�N��荞��
        ''' </summary>
        Private Sub _LoadHooks()
            Dim config As SystemConfiguration = SystemConfiguration.Current
            Dim systemClass As ClassSection = config.Manager.System
            If (systemClass IsNot Nothing) Then
                Dim hooksSection As Setting = systemClass.GetSetting("Hooks")
                If (hooksSection IsNot Nothing) Then
                    For Each item In hooksSection.Item
                        Try
                            Dim asm As Assembly = Assembly.Load(item.Name)

                            Dim className As String = String.Empty
                            For Each type In asm.GetTypes
                                If (item.Value.Equals("Login")) Then
                                    If type.GetInterface(LOGIN_HOOK_INTERFACE) IsNot Nothing Then
                                        className = type.FullName
                                        Exit For
                                    End If
                                End If
                            Next type

                            If (Not String.IsNullOrEmpty(className)) Then
                                Dim hook As Object = asm.CreateInstance(className)
                                config.Hooks.Add(hook)
                                Logger.Info(String.Format(CultureInfo.InvariantCulture, "SystemFramework load {0} as {1} hook", className, item.Value))
                            Else
                                Dim message As String = String.Format(CultureInfo.InvariantCulture, "No {0} implementation found in {1}.", LOGIN_HOOK_INTERFACE, item.Name)
                                Logger.Error(message)
                            End If
                        Catch ex As Exception
                            Logger.Error(String.Format(CultureInfo.InvariantCulture, "SystemFramework couldn't load {0} hook ({1})", item.Value, item.Name), ex)
                        End Try
                    Next

                End If
            End If
        End Sub

    End Class
End Namespace
