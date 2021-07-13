'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Text
Imports System.Web
Imports System.Xml
Imports System.Globalization
Imports System.Web.Configuration
Imports System.Web.SessionState
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Oracle.DataAccess.Client

Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' ���O�o�͂̂��߂̃��[�e�B���e�B�@�\��񋟂���N���X�ł��B
    ''' ���O�̏o�͐ݒ�́A�O���t�@�C���Ƃ��Ē�`����܂��B
    ''' </summary>
    ''' <remarks>
    ''' �����o�ɃA�N�Z�X���邽�߂ɁA�ÓI�N���X�̃C���X�^���X��錾����K�v�͂���܂���B
    ''' ���̃N���X�̓A�Z���u���O�Ɍ��J���܂��B���̃N���X�͌p���ł��܂���B
    ''' </remarks>
    Public NotInheritable Class LoggerUtility

#Region "�萔"
        ''' <summary>
        ''' �R���e�L�X�g�ɒl���i�[����Ƃ��̃L�[�i���O�C��ID�j���i�[���܂��B
        ''' </summary>
        Public Const ContextKeyLoginId As String = "LoggerUtility.loginId"

        ''' <summary>
        ''' �R���e�L�X�g�ɒl���i�[����Ƃ��̃L�[�i���݂̌����j���i�[���܂��B
        ''' </summary>
        Public Const ContextKeySelectedRole As String = "LoggerUtility.selectedRole"

        ''' <summary>
        ''' �R���e�L�X�g�ɒl���i�[����Ƃ��̃L�[�i�A�N�Z�XURL�j���i�[���܂��B
        ''' </summary>
        Public Const ContextKeyAccessUrl As String = "LoggerUtility.accessUrl"

        ''' <summary>
        ''' �R���e�L�X�g�ɒl���i�[����Ƃ��̃L�[�i�A�v��ID�j���i�[���܂��B
        ''' </summary>
        Public Const ContextKeyAplId As String = "LoggerUtility.aplId"

        ''' <summary>
        ''' ���O�̃f���~�^���i�[���܂��B
        ''' </summary>
        Friend Const LogDelimiter As String = " "
        ''' <summary>
        ''' ���O�̃Z�b�V����ID�̃f�t�H���g��������i�[���܂��B
        ''' </summary>
        Private Const LogDefaultSessionID As String = "------------------------"
        ''' <summary>
        ''' ���O�̃��O�C��ID�̃f�t�H���g��������i�[���܂��B
        ''' </summary>
        Private Const LogDefaultLoginID As String = "------------"
        ''' <summary>
        ''' ���O�̌����̌������i�[���܂��B
        ''' </summary>
        Private Const DigitSelectedRole As Integer = 2
        ''' <summary>
        ''' ���O�̌��݂̌����̃f�t�H���g��������i�[���܂��B
        ''' </summary>
        Private Const LogDefaultSelectedRole As String = "--"
        ''' <summary>
        ''' ���O�̃��x��������̌������i�[���܂��B
        ''' </summary>
        Private Const DigitLabel As Integer = 3
        ''' <summary>
        ''' ���O�̃��x��������̃t�H�[�}�b�g���i�[���܂��B
        ''' </summary>
        Private Const LogLabelFormat As String = "000"
        ''' <summary>
        ''' ���O�̃A�v��ID�̌������i�[���܂��B
        ''' </summary>
        Friend Const DigitApliID As Integer = 10
        ''' <summary>
        ''' ���O�̃A�v��ID�̃f�t�H���g��������i�[���܂��B
        ''' </summary>
        Friend Const LogDefaultAplID As String = "----------"
        ''' <summary>
        ''' ���O�̃A�N�Z�XURL�̌������i�[���܂��B
        ''' </summary>
        Friend Const DigitAccessUrl As Integer = 15
        ''' <summary>
        ''' �G���R�[�h�̃C���X�^���X���i�[���܂��B
        ''' �ő包�����J�E���g����Ƃ��ɗ��p����B
        ''' </summary>
        Private Shared Encoder As Encoding = Encoding.GetEncoding("utf-8")

        ''' <summary>
        ''' config�t�@�C���ɐݒ肷��̔��X�R�[�h��Class�v�f���B
        ''' </summary>
        Private Const LoggerClass As String = "Logger"

        ''' <summary>
        ''' config�t�@�C���ɐݒ肷��̔��X�R�[�h��Setting�v�f���B
        ''' </summary>
        Private Const LoggerSetting As String = "EventLogInfo"

        ''' <summary>
        ''' config�t�@�C���ɐݒ肷��̔��X�R�[�h��Item�v�f���B
        ''' </summary>
        Private Const LoggerItem As String = "DealerCode"

        ''' <summary>
        ''' �C�x���g���O�̃\�[�X���ڂ̐ړ���B
        ''' </summary>
        Private Const SourceNameHeader As String = "i-CROP"

        ''' <summary>
        ''' �C�x���g���O�̃\�[�X���ڂ̋�؂蕶���B
        ''' </summary>
        Private Const SourceNameDelimiter As String = "_"

        ''' <summary>
        ''' ��r�Ώ�(������0)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const Zero As Integer = 0

        Friend Const OutputEnableSetting As String = "true"

        'Friend Const SettingParameterTraceLogUser = "TraceLogUser"
        'Friend Const SettingParameterBaseDeirectory = "BaseDirectory"
        'Friend Const SettingParameterApplication = "Application"
        'Friend Const SettingParameterReceiveLogDirectory = "ReceiveLogDirectory"

        'Private Const SettingErrorLogggerName As String = "ErrorLogger"
        'Private Const SettingTraceLogggerName As String = "TraceLogger"
        'Private Const SettingReceiveLoggerName As String = "ReceiveLogger"
#End Region

#Region "�ϐ�"
        'Private Shared _baseDirectry As String = Nothing

        Private Shared _isEnableErrorLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnableWarnLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnableInfoLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnableTraceLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnableReceiveLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnableSecurityLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnableSqlPerformanceLog As Nullable(Of Boolean) = Nothing
        Private Shared _isEnablePerformLogSetting As Nullable(Of Boolean) = Nothing
        Private Shared _isEnablePerformErrorLogSetting As Nullable(Of Boolean) = Nothing

        'Private Shared _isEnableTraceLogUsers As String = Nothing

        Private Shared _ErrorLogger As New TraceLogger("ErrorLog")
        Private Shared _WarnLogger As New TraceLogger("WarnLog")
        Private Shared _InfoLogger As New TraceLogger("InfoLog")
        Private Shared _TraceLogger As New TraceLogger("TraceLog")
        Private Shared _ReceiveLogger As New TraceLogger("ReceiveLog")
        Private Shared _PerformLogger As New TraceLogger("PerformLog")
        Private Shared _PerformErrorLogger As New TraceLogger("PerformErrorLog")
        Private Shared _PerformErrorThreshold As Nullable(Of Integer)

        Private Shared _SessionStateCookieName As String
        ''���\�Ή� Add Start
        'Private Shared _isEnableInfoErrorLogLevelSetting As Nullable(Of Boolean) = Nothing
        'Private Shared _isEnableInfoReceiveLogLevelSetting As Nullable(Of Boolean) = Nothing

        'Private Shared _isEnableWarnErrorLogLevelSetting As Nullable(Of Boolean) = Nothing

        'Private Shared _isEnableErrorLogLevelSetting As Nullable(Of Boolean) = Nothing

        'Private Shared _isEnableTraceLogLevelSetting As Nullable(Of Boolean) = Nothing
        ''���\�Ή� Add End
        'Private Shared _errorLoggerInstance As Log4Net.ILog
        'Private Shared _receiveLoggerInstance As Log4Net.ILog
        'Private Shared _traceLoggerInstance As Log4Net.ILog
        'Private Shared _eventLogSource As String = SourceNameHeader

#End Region

#Region "�v���p�e�B"

#Region "ErrorLoggerInstance"
        Friend Shared ReadOnly Property ErrorLoggerInstance() As TraceLogger
            Get
                Return _ErrorLogger
            End Get
        End Property
#End Region

#Region "WarnLoggerInstance"
        Friend Shared ReadOnly Property WarnLoggerInstance() As TraceLogger
            Get
                Return _WarnLogger
            End Get
        End Property
#End Region

#Region "InfoLoggerInstance"
        Friend Shared ReadOnly Property InfoLoggerInstance() As TraceLogger
            Get
                Return _InfoLogger
            End Get
        End Property
#End Region

#Region "ReceiveLoggerInstance"
        Friend Shared ReadOnly Property ReceiveLoggerInstance() As TraceLogger
            Get
                Return _ReceiveLogger
            End Get
        End Property
#End Region

#Region "TraceLoggerInstance"
        Friend Shared ReadOnly Property TraceLoggerInstance() As TraceLogger
            Get
                Return _TraceLogger
            End Get
        End Property
#End Region

#Region "PerformanceTraceLoggerInstance"
        Friend Shared ReadOnly Property PerformLoggerInstance() As TraceLogger
            Get
                Return _PerformLogger
            End Get
        End Property
#End Region

#Region "PerformanceErrorTraceLoggerInstance"
        Friend Shared ReadOnly Property PerformErrorLoggerInstance() As TraceLogger
            Get
                Return _PerformErrorLogger
            End Get
        End Property
#End Region

#End Region


#Region "IsEnableErrorLogSetting"
        ''' <summary>
        ''' �G���[���O�o�͐ݒ�
        ''' </summary>
        ''' <returns>Web.config�Ɏw�肳��Ă���G���[���O�o�͐ݒ�</returns>
        Friend Shared ReadOnly Property IsEnableErrorLogSetting() As Boolean
            Get
                If _isEnableErrorLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableErrorLogSetting = False
                        Return _isEnableErrorLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableErrorLogSetting = False
                        Return _isEnableErrorLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableErrorLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableErrorLogSetting = False
                        Return _isEnableErrorLogSetting.Value
                    End If

                    _isEnableErrorLogSetting = True

                End If

                Return _isEnableErrorLogSetting.Value
            End Get
        End Property
#End Region

#Region "IsEnableWarnLogSetting"
        ''' <summary>
        ''' �x�����O�o�͐ݒ�
        ''' </summary>
        ''' <returns>Web.config�Ɏw�肳��Ă���x�����O�o�͐ݒ�</returns>
        Friend Shared ReadOnly Property IsEnableWarnLogSetting() As Boolean
            Get
                If _isEnableWarnLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableWarnLogSetting = False
                        Return _isEnableWarnLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableWarnLogSetting = False
                        Return _isEnableWarnLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableWarnLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableWarnLogSetting = False
                        Return _isEnableWarnLogSetting.Value
                    End If

                    _isEnableWarnLogSetting = True

                End If

                Return _isEnableWarnLogSetting.Value
            End Get
        End Property
#End Region

#Region "IsEnableInfoLogSetting"
        ''' <summary>
        ''' ��񃍃O�o�͐ݒ�
        ''' </summary>
        ''' <returns>Web.config�Ɏw�肳��Ă����񃍃O�o�͐ݒ�</returns>
        Friend Shared ReadOnly Property IsEnableInfoLogSetting() As Boolean
            Get
                If _isEnableInfoLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableInfoLogSetting = False
                        Return _isEnableInfoLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableInfoLogSetting = False
                        Return _isEnableInfoLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableInfoLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableInfoLogSetting = False
                        Return _isEnableInfoLogSetting.Value
                    End If

                    _isEnableInfoLogSetting = True

                End If

                Return _isEnableInfoLogSetting.Value
            End Get
        End Property
#End Region

#Region "IsEnableTraceLogSetting"
        ''' <summary>
        ''' �g���[�X���O�o�͐ݒ�
        ''' </summary>
        ''' <returns>Web.config�Ɏw�肳��Ă���G���[���O�o�͐ݒ�</returns>
        Friend Shared ReadOnly Property IsEnableTraceLogSetting() As Boolean
            Get
                If _isEnableTraceLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableTraceLogSetting = False
                        Return _isEnableTraceLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableTraceLogSetting = False
                        Return _isEnableTraceLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableTraceLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableTraceLogSetting = False
                        Return _isEnableTraceLogSetting.Value
                    End If

                    _isEnableTraceLogSetting = True
                End If

                Return _isEnableTraceLogSetting.Value

            End Get
        End Property
#End Region

#Region "IsEnableReceiveLogSetting"
        ''' <summary>
        ''' �g���[�X���O�o�͐ݒ�
        ''' </summary>
        ''' <returns>Web.config�Ɏw�肳��Ă���G���[���O�o�͐ݒ�</returns>
        Friend Shared ReadOnly Property IsEnableReceiveLogSetting() As Boolean
            Get
                If _isEnableReceiveLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableReceiveLogSetting = False
                        Return _isEnableReceiveLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableReceiveLogSetting = False
                        Return _isEnableReceiveLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableReceiveLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableReceiveLogSetting = False
                        Return _isEnableReceiveLogSetting.Value
                    End If

                    _isEnableReceiveLogSetting = True
                End If

                Return _isEnableReceiveLogSetting.Value

            End Get
        End Property
#End Region

#Region "IsEnablePerformLogSetting"
        ''' <summary>
        ''' �T�[�o�������� �o�͐ݒ�
        ''' </summary>
        ''' <returns>Web.config�Ɏw�肳��Ă���G���[���O�o�͐ݒ�</returns>
        Public Shared ReadOnly Property IsEnablePerformLogSetting() As Boolean
            Get
                If _isEnablePerformLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnablePerformLogSetting = False
                        Return _isEnablePerformLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnablePerformLogSetting = False
                        Return _isEnablePerformLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnablePerformLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnablePerformLogSetting = False
                        Return _isEnablePerformLogSetting.Value
                    End If

                    _isEnablePerformLogSetting = True

                End If

                Return _isEnablePerformLogSetting.Value
            End Get
        End Property
#End Region

#Region "IsEnablePerformErrorLogSetting"
        ''' <summary>
        ''' �T�[�o�������Ԃ�臒l�𒴂����ۂɏo�͂���G���[�o�͐ݒ�
        ''' </summary>
        ''' <returns>Web.config�Ɏw�肳��Ă���G���[���O�o�͐ݒ�</returns>
        Public Shared ReadOnly Property IsEnablePerformErrorLogSetting() As Boolean
            Get
                If _isEnablePerformErrorLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnablePerformErrorLogSetting = False
                        Return _isEnablePerformErrorLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnablePerformErrorLogSetting = False
                        Return _isEnablePerformErrorLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnablePerformErrorLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnablePerformErrorLogSetting = False
                        Return _isEnablePerformErrorLogSetting.Value
                    End If

                    _isEnablePerformErrorLogSetting = True

                End If

                Return _isEnablePerformErrorLogSetting.Value
            End Get
        End Property
#End Region

#Region "IsEnableSecurityLogSetting"
        ''' <summary>
        ''' �G���[���O�o�͐ݒ�
        ''' </summary>
        ''' <returns>Web.config�Ɏw�肳��Ă���G���[���O�o�͐ݒ�</returns>
        Public Shared ReadOnly Property IsEnableSecurityLogSetting() As Boolean
            Get
                If _isEnableSecurityLogSetting Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableSecurityLogSetting = False
                        Return _isEnableSecurityLogSetting.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableSecurityLogSetting = False
                        Return _isEnableSecurityLogSetting.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableSecurityLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableSecurityLogSetting = False
                        Return _isEnableSecurityLogSetting.Value
                    End If

                    _isEnableSecurityLogSetting = True

                End If

                Return _isEnableSecurityLogSetting.Value
            End Get
        End Property
#End Region

#Region "IsEnableSqlPerformanceLog"
        ''' <summary>
        ''' �G���[���O�o�͐ݒ�
        ''' </summary>
        ''' <returns>Web.config�Ɏw�肳��Ă���G���[���O�o�͐ݒ�</returns>
        Friend Shared ReadOnly Property IsEnableSqlPerformanceLog() As Boolean
            Get
                If _isEnableSqlPerformanceLog Is Nothing Then

                    Dim config As ClassSection = SystemConfiguration.Current.Manager.LogManager
                    If config Is Nothing Then
                        _isEnableSqlPerformanceLog = False
                        Return _isEnableSqlPerformanceLog.Value
                    End If

                    Dim setting As Setting = config.GetSetting(String.Empty)
                    If setting Is Nothing Then
                        _isEnableSqlPerformanceLog = False
                        Return _isEnableSqlPerformanceLog.Value
                    End If

                    Dim item As String = CType(setting.GetValue("EnableSqlPerformanceLog"), String)

                    If Not OutputEnableSetting.Equals(item, StringComparison.OrdinalIgnoreCase) Then
                        _isEnableSqlPerformanceLog = False
                        Return _isEnableSqlPerformanceLog.Value
                    End If

                    _isEnableSqlPerformanceLog = True

                End If

                Return _isEnableSqlPerformanceLog.Value
            End Get
        End Property
#End Region

#Region "PerformErrorThresholdMilliSecond"
        Private Const DefaultPerformErrorThreshold As Integer = 5000
        ''' <summary>
        ''' �T�[�o�������Ԓ���臒l�i�~���b�j
        ''' </summary>
        ''' <returns>Web.config�Ɏw�肳��Ă���G���[���O�o�͐ݒ�</returns>
        Public Shared ReadOnly Property PerformErrorThresholdMilliSecond() As Integer
            Get
                If _PerformErrorThreshold Is Nothing Then

                    Dim val As String = System.Configuration.ConfigurationManager.AppSettings.[Get]("PerformErrorThreshold")
                    If String.IsNullOrEmpty(val) Then
                        _PerformErrorThreshold = DefaultPerformErrorThreshold
                    Else
                        _PerformErrorThreshold = CType(ComponentModel.TypeDescriptor.GetConverter(GetType(Integer)).ConvertFrom(val), Integer)
                    End If
                End If
                Return _PerformErrorThreshold.Value
            End Get
        End Property
#End Region

#Region "SessionStateCookieName"
        ''' <summary>
        ''' SessionState CookieName �ݒ�
        ''' </summary>
        ''' <returns>Web.config�Ɏw�肳��Ă���G���[���O�o�͐ݒ�</returns>
        Public Shared ReadOnly Property SessionStateCookieName() As String
            Get
                If _SessionStateCookieName Is Nothing Then
                    _SessionStateCookieName = (New System.Web.Configuration.SessionStateSection()).CookieName
                End If
                Return _SessionStateCookieName
            End Get
        End Property
#End Region

#Region "New"
        ''' <summary>
        ''' �R���X�g���N�^�ł��B�C���X�^���X�𐶐������Ȃ��悤�ɂ��邽�߁A�C���q��Private�ł��B
        ''' </summary>
        Private Sub New()


        End Sub
#End Region

#Region "FormatElement"
        ''' <summary>
        ''' ��������A�w�肵�������񒷂ɐ��`����B�s��������𔼊p�X�y�[�X�Ńp�f�B���O�A
        ''' �������͎w����I�[�o�[������������폜����B
        ''' </summary>
        ''' <param name="value">���`�Ώۂ̕�����B</param>
        ''' <param name="maxByte">������ő�o�C�g���B</param>
        ''' <returns>���`����������B</returns>
        Friend Shared Function FormatElement(ByVal value As String, _
                                             ByVal maxByte As Integer) As String

            '1.����value�̕�����̃o�C�g��������maxLength�ň������l���A���[�J���ϐ�spaceLength�Ɋi�[����B
            Dim spaceLength As Integer = maxByte - Encoder.GetByteCount(value)

            '2.1 �Ŏ擾�����l�𔻒肷��B
            If spaceLength = Zero Then
                '2.1.spaceLength��0�̏ꍇ
                ' ����value�����̂܂ܕԂ��B
                Return value

            ElseIf 0 < spaceLength Then
                '2.2.spaceLength��0�����傫���ꍇ
                ' ����value�̕�����̃o�C�g��������maxByte�ɂȂ�܂ŉE�����󔒂Ńp�f�B���O���A���������������Ԃ��B
                '2.2.1.���[�J���ϐ�returnStr ���`����B�����l�́ANew StringBuilder(value)�Ƃ���B
                Dim sbLarge As New StringBuilder(value)

                '2.2.2.returnStr�ɁAspaceLength�̐������󔒕�����ǉ�����B
                sbLarge.Append(String.Empty.PadRight(spaceLength))

                '2.2.3.�쐬�����������Ԃ��B
                Return sbLarge.ToString()

            Else
                '2.3.2.1�A2.2 �ȊO�̏ꍇ
                ' ����value�̕�����̃o�C�g�����A����maxByte�̐��ɂȂ�悤�ɕ�������폜���A���`�����������Ԃ��B
                '2.3.1.���[�J���ϐ�returnStr ���`����B
                ' �����l�́AStringBuilder(LoggerUtility.GetOmittedString(value, maxByte))�Ƃ���B
                Dim sbElse As New StringBuilder(LoggerUtility.GetOmittedString(value, maxByte))
                '�i�܂��ALoggingUtility.GetOmittedString���\�b�h�ŕԂ����l�́A1�o�C�g�������2�o�C�g������̍�����
                ' ������̏ꍇ�A����maxLength���1�o�C�g�s������ꍇ������̂ŁA�s�������o�C�g�̓X�y�[�X�����Ŗ��߂�
                ' �������s���K�v�����邽�߉��L���������{����B�j

                '2.3.2.����maxLength ����A2.3.1 �Ő�������Shift_JIS�����R�[�h�Ƃ����Ƃ��̃o�C�g���������A
                ' ���[�J���ϐ�length�Ɋi�[����B
                Dim count As Integer = maxByte - Encoder.GetByteCount(sbElse.ToString())

                '2.3.3.count�𔻒肷��B
                If 0 < count Then
                    '2.3.3.1.count��0���傫���ꍇ�A
                    'returnStr�ɁAcount�̐������󔒕�����ǉ�����B
                    sbElse.Append(String.Empty.PadRight(count))
                End If
                '2.3.3.2.���̑�
                '���̏��������{�B

                '2.3.4.�쐬�����������Ԃ��B
                Return sbElse.ToString()
            End If

        End Function
#End Region

#Region "FormatElementNotDelete"
        ''' <summary>
        ''' ��������A�w�肵�������񒷂ɐ��`����B�s��������𔼊p�X�y�[�X�Ńp�f�B���O����B
        ''' </summary>
        ''' <param name="value">���`�Ώۂ̕�����B</param>
        ''' <param name="maxByte">������ő�o�C�g���B</param>
        ''' <returns>���`����������B</returns>
        Friend Shared Function FormatElementNotDelete(ByVal value As String, _
                                             ByVal maxByte As Integer) As String

            '1.����value�̕�����̃o�C�g��������maxLength�ň������l���A���[�J���ϐ�spaceLength�Ɋi�[����B
            Dim spaceLength As Integer = maxByte - Encoder.GetByteCount(value)

            If 0 < spaceLength Then
                '2.spaceLength��0�����傫���ꍇ
                ' ����value�̕�����̃o�C�g��������maxByte�ɂȂ�܂ŉE�����󔒂Ńp�f�B���O���A���������������Ԃ��B
                '2.1.���[�J���ϐ�returnStr ���`����B�����l�́ANew StringBuilder(value)�Ƃ���B
                Dim returnStr As New StringBuilder(value)

                '2.2.returnStr�ɁAspaceLength�̐������󔒕�����ǉ�����B
                returnStr.Append(String.Empty.PadRight(spaceLength))

                '2.3.�쐬�����������Ԃ��B
                Return returnStr.ToString()

            Else
                '2�ȊO�̏ꍇ

                ' ����value�����̂܂ܕԂ��B
                Return value
            End If

        End Function
#End Region

#Region "ConvertLabel"
        ' ''' <summary>
        ' ''' ���x���ԍ����R���̕�����ɐ��`����B
        ' ''' </summary>
        ' ''' <param name="labelNo">���x���ԍ��B</param>
        ' ''' <returns>���`�����R���̕�����B</returns>
        'Friend Shared Function ConvertLabel(ByVal labelNo As Integer) As String
        '    '1.labelNo�𕶎���ɕϊ����ă��[�J���ϐ�labelStr�Ɋi�[����B
        '    Dim labelStr As String = labelNo.ToString(CultureInfo.InvariantCulture)

        '    '2.labelStr�̕����񐔂��J�E���g���A���[�J���ϐ�labelLength�Ɋi�[����B
        '    Dim labelLength As Integer = labelStr.Length

        '    '3.labelLength�̐����r����B
        '    If labelLength = DigitLabel Then
        '        '3.1.3�i���O�ɏo�͂��郉�x���̕����񐔁A�萔:DIGIT_LABEL�j�Ɠ������Ƃ�
        '        '1�Ő��������������Ԃ��܂��B
        '        Return labelStr
        '    ElseIf labelLength < DigitLabel Then
        '        '3.2.3�i���O�ɏo�͂��郉�x���̕����񐔁A�萔:DIGIT_LABEL�j��菬�����Ƃ�

        '        '������labelStr��3���ɂȂ�悤�ɍ�����0�Ŗ��߁A���������������Ԃ��B
        '        Return Format(labelNo, LogLabelFormat)
        '    Else
        '        '3.3.3.1�A3.2�ȊO�̏ꍇ
        '        '������labelStr�̉E��3�����擾���A�擾�����������Ԃ��B
        '        Return labelStr.Substring(labelLength - DigitLabel, DigitLabel)
        '    End If
        'End Function
#End Region

#Region "GetKeyElementInfo"
        ''' <summary>
        ''' ���O�o�͕�������擾����B�Z�b�V����ID�A���O�C��ID�A���[�U�������R���e�L�X�g����擾���A������ɐ��`����B
        ''' </summary>
        ''' <param name="context">�R���e�L�X�g�B</param>
        ''' <returns>���`����������B</returns>
        Friend Shared Function GetKeyElementInfo(ByVal context As HttpContext) As String
            '1.���O�o�͕�����i�Z�b�V����ID�A���O�C��ID�A���[�U�����j���擾����B
            '1.1. �Z�b�V�������擾���A���[�J���ϐ�session�Ɋi�[����B
            Dim session As HttpSessionState = context.Session

            '1.2. ���[�J���ҏWsesssionId ���`����B
            Dim sessionId As String

            '1.3.�Z�b�V�����̗L���𔻒肷��B
            If Not IsNothing(session) Then
                '1.3.1.�Z�b�V���������݂���ꍇ
                '���[�J���ϐ�sessionId�ɃZ�b�V����ID���i�[����B
                sessionId = session.SessionID
            Else
                If String.IsNullOrEmpty(SessionStateCookieName) Then
                    '1.3.2.�Z�b�V���������݂��Ȃ��ꍇ
                    '���[�J���ϐ�sessionId�ɁA"------------------------" ���i�[����B�i�n�C�t��- ��24�j
                    sessionId = LogDefaultSessionID
                Else
                    'SessionState��CookieName �ŃN�b�L�[���������āASessionId�̎擾�����݂�
                    Dim ses = context.Request.Cookies.Get(SessionStateCookieName)
                    If ses Is Nothing Then
                        sessionId = LogDefaultSessionID
                    Else
                        sessionId = ses.Value
                    End If
                End If
            End If

            '1.4.�R���e�L�X�g����A�萔CONTEXT_KEY_LOGINID ���L�[�Ƃ��Ēl���擾���AString�ɃL���X�g���A
            '���[�J���ϐ�loginId�Ɋi�[����B
            Dim loginId As String = DirectCast(context.Items(ContextKeyLoginId), String)

            '1.5.���[�J���ϐ�loginId��Nothing�ł��邩���肷��B
            If String.IsNullOrEmpty(loginId) Then
                '1.5.1.Nothing�̏ꍇ
                '���[�J���ϐ�loginId�ɁA"------------" ���i�[����B�i�n�C�t��- ��12�j
                loginId = LogDefaultLoginID
            End If
            '1.5.2.Nothing�łȂ��ꍇ
            '�����Ȃ��B


            '1.6.�R���e�L�X�g����A�萔CONTEXT_KEY_SELECTEDROLE ���L�[�Ƃ��Ēl���擾���AString�ɃL���X�g���A
            '���[�J���ϐ�selectedRole�Ɋi�[����B
            Dim selectedRole As String = DirectCast(context.Items(ContextKeySelectedRole), String)

            '1.7.���[�J���ϐ�selectedRole��Nothing�ł��邩���肷��B
            If IsNothing(selectedRole) Then
                '1.7.1.Nothing�̏ꍇ
                '���[�J���ϐ�selectedRole�ɁA"--" ���i�[����B�i�n�C�t��- ��2�j
                selectedRole = LogDefaultSelectedRole
            Else
                '1.7.2.Nothing�łȂ��ꍇ
                '���݂̌��������[�J���ϐ�selectedRole�Ɋi�[����B
                '���݂̌�����1���̏ꍇ�A�E���ɋ󔒕�����1�ǉ�����B
                selectedRole = selectedRole.PadRight(DigitSelectedRole)
            End If

            '2.���O�o�͗p���b�Z�[�W�̐������s���B
            '2.1.���[�J���ϐ�returnInfo���`���AStringBuilder�I�u�W�F�N�g�𐶐����A�i�[����B
            Dim returnInfo As New StringBuilder

            '2.2.���[�J���ϐ�returnInfo�ɁA�Z�b�V����ID������ƁA��؂蕶��" "�iLoggerUtility.LOG_DELIM�j��ǉ�����B
            returnInfo.Append(sessionId).Append(LoggerUtility.LogDelimiter)

            '2.3.���[�J���ϐ�returnInfo�ɁA���O�C��ID������ƁA��؂蕶��" "�iLoggerUtility.LOG_DELIM�j��ǉ�����B
            returnInfo.Append(loginId).Append(LoggerUtility.LogDelimiter)

            '2.4.���[�J���ϐ�returnInfo�ɁA���݂̃��[�U����������ƁA��؂蕶��" "�iLoggerUtility.LOG_DELIM�j��ǉ�����B
            '���[�U�����������ǉ�����ہA
            returnInfo.Append(selectedRole).Append(LoggerUtility.LogDelimiter)

            '3.���������������Ԃ��B
            Return returnInfo.ToString()

        End Function
#End Region

#Region "GetOmittedString"
        ''' <summary>
        ''' �w��o�C�g���iUTF-8�ɂăJ�E���g�j���̕�������Ԃ��B�]���ȕ�����́A�폜�����B
        ''' </summary>
        ''' <param name="value">�Ώە�����B</param>
        ''' <param name="maxByte">������ő�o�C�g���B</param>
        ''' <returns>���`����������B</returns>
        Friend Shared Function GetOmittedString(ByVal value As String, _
                                                ByVal maxByte As Integer) As String

            '������ő�o�C�g���̔����̃o�C�g�T�C�Y���擾����B
            Dim halfByteSize As Integer = maxByte \ 2

            '�����̕����񐔂��i�[����
            Dim valueLength As Integer = value.Length

            '�����̕����񐔂ƁA������̍ő�o�C�g���̔����̃o�C�g���Ƃ̔�r
            If halfByteSize < valueLength Then
                '�ԋp���镶����̕��������i�[����B
                Dim resultValueLength As Integer = halfByteSize

                '�ԋp���镶����̃o�C�g�����i�[����B
                Dim resultByteSize As Integer = _
                             Encoder.GetByteCount(value.Substring(0, halfByteSize))

                '������ő�o�C�g���̔����̒l�ȍ~�̈����̕������Char�z��Ŏ擾����B
                Dim tailHalfByteChar As Char() = _
                         value.ToCharArray(halfByteSize, valueLength - halfByteSize)

                'Char�z�񐔕��A�������J��Ԃ��B������1�����o���A�o�C�g���𔻒肷��B
                For Each cs As Char In tailHalfByteChar
                    resultByteSize += Encoder.GetByteCount(cs)
                    If maxByte < resultByteSize Then
                        Return value.Substring(0, resultValueLength)
                    Else
                        resultValueLength += 1
                    End If
                Next cs
            End If
            '�w��o�C�g�������̏ꍇ�A�l�����̂܂ܕԂ�
            Return value
        End Function
#End Region

#Region "EventLogSourceName"
        ' ''' <summary>
        ' ''' �C�x���g���O�̃\�[�X���ڂɕ\�����镶�����Ԃ��B
        ' ''' </summary>
        'Public Shared ReadOnly Property EventLogSourceName() As String
        '    Get
        '        '1.�萔SOURCE_NAME_HEADER�ƁA�N���X�ϐ�_eventLogSource�̔�r�B
        '        '1.1.�萔SOURCE_NAME_HEADER�ƁA�N���X�ϐ�_eventLogSource�̕����񂪓������ꍇ�B
        '        If SourceNameHeader.Equals(_eventLogSource) Then

        '            '1.1.1.���[�J���ϐ�dealerCode�i����:String�A
        '            '      �����l: GetDealerCode()�̖߂�l)���`���܂��B
        '            Dim dealerCode As String = GetDealerCode()

        '            '1.1.2.���[�J���ϐ�dealerCode��Nothing����B
        '            If dealerCode IsNot Nothing Then
        '                '1.1.2.1.���[�J���ϐ�dealerCode��Nothing�łȂ��ꍇ�A
        '                '        �N���X�ϐ�_eventLogSource�ɁA���L�Ő���������������i�[����B
        '                '        _eventLogSource = �萔SOURCE_NAME_HEADER & _
        '                '                           �萔SOURCE_NAME_DELIMITER & _
        '                '                           ���[�J���ϐ�dealerCode
        '                _eventLogSource = SourceNameHeader & _
        '                                    SourceNameDelimiter & _
        '                                    dealerCode
        '            End If

        '        End If
        '        '1.2.��L�ȊO�̏ꍇ�B
        '        '�����Ȃ��B

        '        '2.�N���X�ϐ�_eventLogSource�̒l��Ԃ��B
        '        Return _eventLogSource
        '    End Get
        'End Property
#End Region

#Region "GetDealerCode"
        ' ''' <summary>
        ' ''' Web.Config�Aexe.config �t�@�C���ɐݒ肳��Ă���A�̔��X�R�[�h���擾����B
        ' ''' �̔��X�R�[�h���擾�ł��Ȃ��ꍇ��Nothing��Ԃ��B
        ' ''' </summary>
        ' ''' <returns>�̔��X�R�[�h</returns>
        'Private Shared Function GetDealerCode() As String
        '    '1.���[�J���ϐ�config�i����:Toyota.eCRB.SystemFrameworks.Configuration.ClassSection�A
        '    '  �����l:ConfigurationManager.GetClassSection(����: �萔LOGGER_CLASS))���`���܂��B
        '    Dim config As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = _
        '        SystemConfiguration.Current.Manager.GetClassSection(LoggerClass)

        '    '2.���[�J���ϐ�config �̔���B
        '    If config Is Nothing Then
        '        '2.1.���[�J���ϐ�config���ANothing�̏ꍇ�B
        '        '    Nothing��Ԃ��B
        '        Return Nothing
        '    End If
        '    '2.2.��L�ȊO�̏ꍇ�B
        '    '    �����Ȃ��B

        '    '3.���[�J���ϐ�setting�i����:Toyota.eCRB.SystemFrameworks.Configuration.Setting�A
        '    '  �����l:���[�J���ϐ�config.GetSetting(����: �萔LOGGER_SETTING))���`���܂��B
        '    Dim setting As Toyota.eCRB.SystemFrameworks.Configuration.Setting = config.GetSetting(LoggerSetting)

        '    '4.���[�J���ϐ�setting �̔���B
        '    If setting Is Nothing Then
        '        '4.1.���[�J���ϐ�setting���ANothing�̏ꍇ�B
        '        '    Nothing ��Ԃ��B
        '        Return Nothing
        '    End If
        '    '4.2.��L�ȊO�̏ꍇ�B
        '    '    �����Ȃ��B

        '    '5.���[�J���ϐ�dealerCode�i����:Object�A
        '    '  �����l:���[�J���ϐ�setting.GetValue(����: �萔LOGGER_ITEM))���`���܂��B
        '    Dim dealerCode As Object = setting.GetValue(LoggerItem)

        '    '6.���[�J���ϐ�dealerCode �̔���B
        '    If dealerCode Is Nothing Then
        '        '6.1.���[�J���ϐ�dealerCode���ANothing�̏ꍇ�B
        '        '    Nothing ��Ԃ��B
        '        Return Nothing
        '    End If
        '    '6.2.��L�ȊO�̏ꍇ�B
        '    '    �����Ȃ��B

        '    '7.���[�J���ϐ�dealerCode��String��DirectCast���A�l��Ԃ��B
        '    Return DirectCast(dealerCode, String)
        'End Function
#End Region

#Region "SetLog4netSettingPrameter"
        Public Shared Sub Setlog4netSettingPrameter(ByVal user As String, ByVal app As String, ByVal receiver As String)
            Exit Sub
            '    If String.IsNullOrEmpty(user) Then
            '        log4net.ThreadContext.Properties(SettingParameterTraceLogUser) = String.Empty
            '    Else
            '        log4net.ThreadContext.Properties(SettingParameterTraceLogUser) = "." & user
            '    End If

            '    log4net.ThreadContext.Properties(SettingParameterBaseDeirectory) = LoggerUtility.BaseDeirectory & "\"

            '    If Not String.IsNullOrEmpty(app) Then
            '        app = app & "\"
            '    End If
            '    log4net.ThreadContext.Properties(SettingParameterApplication) = app

            '    If (String.IsNullOrEmpty(receiver)) Then
            '        log4net.ThreadContext.Properties(SettingParameterReceiveLogDirectory) = String.Empty
            '    Else
            '        log4net.ThreadContext.Properties(SettingParameterReceiveLogDirectory) = receiver & "\"
            '    End If

            '    log4net.Config.XmlConfigurator.Configure()

        End Sub
#End Region

#Region "CreateWebHeader"
        Public Shared Function CreateWebHeader() As String

            If Not CStr(ApplicationType.Web).Equals(SystemConfiguration.Current.GetRuntimeSetting(SystemConfigurationType.ApplicationType)) Then
                Return String.Empty
            End If

            Dim context As HttpContext = HttpContext.Current
            Dim log As New StringBuilder

            Dim aplId As String = Nothing
            If Not IsNothing(context) Then
                ''���O�C��ID�A�Z�b�V����ID�A���[�U����ǉ�
                log.Append(GetKeyElementInfo(context))

                ''�A�v��ID�̎擾
                aplId = DirectCast(context.Items(ContextKeyAplId), String)
            End If

            If String.IsNullOrEmpty(aplId) Then
                ''"--------"�i8�����̃n�C�t���j��aplId�Ɋi�[����B
                aplId = LogDefaultAplID
            Else
                ''��������w�茅���Ő��`����B
                aplId = FormatElement(aplId, DigitApliID)
            End If

            log.Append(aplId)
            log.Append(LogDelimiter)

            Dim accessUrl As String = Nothing
            If Not IsNothing(context) Then
                accessUrl = DirectCast(context.Items(LoggerUtility.ContextKeyAccessUrl), String)
            End If
            If Not IsNothing(accessUrl) Then
                accessUrl = LoggerUtility.GetOmittedString(accessUrl, LoggerUtility.DigitAccessUrl)
                log.Append(accessUrl).Append(LoggerUtility.LogDelimiter)
            End If

            Return log.ToString

        End Function
#End Region

#Region "CreateParameterString"
        ''' <summary>
        ''' �N�G���̃p�����[�^�𕶎���ɂ��܂��B
        ''' </summary>
        ''' <returns>�p�����[�^�[�̕�����</returns>
        ''' <remarks>�N�G���̃p�����[�^�𕶎���ɂ��܂��B</remarks>
        Public Shared Function CreateParameterString(ByVal param As OracleParameterCollection) As String

            If param Is Nothing Then
                Return String.Empty
            End If

            Dim paramStr As New StringBuilder
            For i As Integer = 0 To param.Count - 1
                paramStr.Append(" [" & param.Item(i).ToString() & "]" & param.Item(i).Value.ToString())
            Next

            Return paramStr.ToString

        End Function
#End Region

    End Class
End Namespace