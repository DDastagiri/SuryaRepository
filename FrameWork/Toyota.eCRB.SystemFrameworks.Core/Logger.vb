Imports System.Globalization
Imports System.Text
Imports System.Web
Imports System.Web.Configuration
Imports System.Xml
Imports System.Linq
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' ���O�o�͋@�\��񋟂���N���X�ł��B
    ''' ���O�̏o�͐ݒ�́A�O���t�@�C���Ƃ��Ē�`����܂��B
    ''' </summary>
    ''' <remarks>
    ''' �����o�ɃA�N�Z�X���邽�߂ɁA�ÓI�N���X�̃C���X�^���X��錾����K�v�͂���܂���B
    ''' ���̃N���X�̓A�Z���u���O�Ɍ��J���܂��B
    ''' ���̃N���X�͌p���ł��܂���B
    ''' </remarks>
    Public NotInheritable Class Logger

        Private Shared _lock As New Object()

        Private Shared _PerformErrorThreshold As Double

        ''' <summary>
        ''' �g���[�X�̎��s��Ԃ��擾�܂��͐ݒ肵�܂�
        ''' </summary>
        ''' <remarks>
        ''' ���̃t���O�̓X���b�h�P�ʂŗL���E�������ݒ肳��܂��B
        ''' TraceOff�̐����try-finally�Ŋm���Ɍ��ɖ߂��悤�ɂ��܂��B
        ''' TraceOff��true�ɂ����܂܂ɂ���Ƃ��̃X���b�h�ŕʂ̏�����
        ''' ���s���ꂽ�ꍇ�Ƀ��O���o�͂���Ȃ����ۂ��������܂�
        ''' </remarks>
        Public Shared Property TraceOff As Boolean
            Get
                Return _traceOff
            End Get
            Set(ByVal value As Boolean)
                _traceOff = value
            End Set
        End Property

        <ThreadStatic()>
        Private Shared _traceOff As Boolean

        Private Const DefaultLogDateTimeFormat = "yyyy/MM/dd_HH:mm:ss.fff"

#Region "New"
        ''' <summary>
        ''' �R���X�g���N�^�ł��B�C���X�^���X�𐶐������Ȃ��悤�ɂ��邽�߁A�C���q��Private�ł��B
        ''' </summary>
        Private Sub New()

        End Sub
#End Region

#Region "[Error]"
        Private Shared ErrorLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' �G���[���O���o�͂��܂��B�i���O���x���FERROR�j
        ''' </summary>
        ''' <param name="msg">���b�Z�[�W</param>
        ''' <param name="ex">�G���[�̌����ƂȂ�����O�i����ꍇ�̂ݎw�肷��j</param>
        ''' <remarks>�G���[���O���o�͂��܂��B�i���O���x���FERROR�j</remarks>
        Public Shared Sub [Error](ByVal msg As String, Optional ByVal ex As Exception = Nothing)

            If TraceOff Then Return
            Dim now As DateTime = DateTime.Now

            ''�o�͐ݒ�ł͂Ȃ��̂ŏI��
            If Not LoggerUtility.IsEnableErrorLogSetting Then
                Return
            End If

            If ErrorLogDateTimeFormat Is Nothing Then
                Dim listener = LoggerUtility.ErrorLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                If listener Is Nothing Then
                    ErrorLogDateTimeFormat = DefaultLogDateTimeFormat
                Else
                    ErrorLogDateTimeFormat = listener.LogDateTimeFormat
                End If
            End If


            Dim log As New StringBuilder  ''���O������
            log.Append(LoggerUtility.CreateWebHeader())

            log.Append(now.ToString(ErrorLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            log.Append(msg & LoggerUtility.LogDelimiter)

            If ex Is Nothing Then
                LoggerUtility.ErrorLoggerInstance.TraceEvent(TraceEventType.Error, TraceCategory.AppError, log.ToString())
                ''instance.Error(log.ToString)
            Else
                If TypeOf ex Is OracleExceptionEx Then
                    Dim oraex As OracleExceptionEx = DirectCast(ex, OracleExceptionEx)
                    log.Append(vbCrLf)
                    log.Append("SQL:" & oraex.CommandText)
                    log.Append(LoggerUtility.CreateParameterString(oraex.Parameters))
                    log.Append(vbCrLf)
                End If
                log.Append(ex.ToString)
                LoggerUtility.ErrorLoggerInstance.TraceEvent(TraceEventType.Error, TraceCategory.AppError, log.ToString)
            End If
        End Sub
#End Region

#Region "Warn"
        Private Shared WarnLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' �x�����O���o�͂��܂��B�i���O���x���FWARN�j
        ''' </summary>
        ''' <param name="msg">���b�Z�[�W</param>
        ''' <remarks>�G���[���O���o�͂��܂��B�i���O���x���FWARN�j</remarks>
        Public Shared Sub Warn(ByVal msg As String)

            If TraceOff Then Return
            Dim now As DateTime = DateTime.Now

            ''�o�͐ݒ�ł͂Ȃ��̂ŏI��
            If Not LoggerUtility.IsEnableErrorLogSetting Then
                Return
            End If

            If WarnLogDateTimeFormat Is Nothing Then
                Dim listener = LoggerUtility.WarnLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                If listener Is Nothing Then
                    WarnLogDateTimeFormat = DefaultLogDateTimeFormat
                Else
                    WarnLogDateTimeFormat = listener.LogDateTimeFormat
                End If
            End If

            Dim log As New StringBuilder
            ''�w�b�_�[(�Z�b�V����ID�A�A�J�E���g�A�����A���ID)�܂ō쐬
            log.Append(LoggerUtility.CreateWebHeader())

            log.Append(now.ToString(WarnLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            log.Append(msg)

            LoggerUtility.WarnLoggerInstance.TraceEvent(TraceEventType.Warning, TraceCategory.AppWarning, log.ToString)

        End Sub
#End Region

#Region "Info"
        Private Shared ReceiveLogDateTimeFormat As String = Nothing
        Private Shared InfoLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' ��񃍃O���o�͂��܂��B�i���O���x���FINFO�j
        ''' </summary>
        ''' <param name="msg">���b�Z�[�W</param>
        ''' <param name="receiveLog">��M���O�ɏo�͂���ꍇ�̂�True�B</param>
        ''' <remarks>���O���o�͂��܂��B�i���O���x���FINFO�j</remarks>
        Public Shared Sub Info(ByVal msg As String, Optional ByVal receiveLog As Boolean = False)

            If TraceOff Then Return
            Dim now As DateTime = DateTime.Now

            '���\�Ή� Add Start
            If (receiveLog) Then
                ''�o�͐ݒ�ł͂Ȃ��̂ŏI��
                If Not LoggerUtility.IsEnableReceiveLogSetting Then
                    Return
                End If
            Else
                ''�o�͐ݒ�ł͂Ȃ��̂ŏI��
                If Not LoggerUtility.IsEnableInfoLogSetting Then
                    Return
                End If
            End If
            '���\�Ή� Add End

            Dim instance As TraceLogger
            If (receiveLog) Then
                instance = LoggerUtility.ReceiveLoggerInstance
                If ReceiveLogDateTimeFormat Is Nothing Then
                    Dim listener = LoggerUtility.ReceiveLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                    If listener Is Nothing Then
                        ReceiveLogDateTimeFormat = DefaultLogDateTimeFormat
                    Else
                        ReceiveLogDateTimeFormat = listener.LogDateTimeFormat
                    End If
                End If
            Else
                instance = LoggerUtility.InfoLoggerInstance
                If InfoLogDateTimeFormat Is Nothing Then
                    Dim listener = LoggerUtility.InfoLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                    If listener Is Nothing Then
                        InfoLogDateTimeFormat = DefaultLogDateTimeFormat
                    Else
                        InfoLogDateTimeFormat = listener.LogDateTimeFormat
                    End If
                End If
            End If

            Dim log As New StringBuilder
            ''�w�b�_�[(�Z�b�V����ID�A�A�J�E���g�A�����A���ID)�܂ō쐬
            log.Append(LoggerUtility.CreateWebHeader())
            If (receiveLog) Then
                log.Append(now.ToString(ReceiveLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            Else
                log.Append(now.ToString(InfoLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            End If
            log.Append(msg)

            instance.TraceEvent(TraceEventType.Information, TraceCategory.AppInformation, log.ToString)

        End Sub
#End Region

#Region "Debug"
        Private Shared DebugLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' �g���[�X���O���o�͂��܂��B�i���O���x���FDEBUG�j
        ''' </summary>
        ''' <param name="msg">���b�Z�[�W</param>
        ''' <remarks>�g���[�X���O���o�͂��܂��B�i���O���x���FDEBUG�j</remarks>
        Public Overloads Shared Sub Debug(ByVal msg As String)

            If TraceOff Then Return
            Dim now As DateTime = DateTime.Now

            ''�o�͐ݒ�ł͂Ȃ��̂ŏI��
            If Not LoggerUtility.IsEnableTraceLogSetting Then
                Return
            End If

            If DebugLogDateTimeFormat Is Nothing Then
                Dim listener = LoggerUtility.TraceLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                If listener Is Nothing Then
                    DebugLogDateTimeFormat = DefaultLogDateTimeFormat
                Else
                    DebugLogDateTimeFormat = listener.LogDateTimeFormat
                End If
            End If

            Dim log As New StringBuilder

            ''�w�b�_�[(�Z�b�V����ID�A�A�J�E���g�A�����A���ID)�܂ō쐬
            log.Append(LoggerUtility.CreateWebHeader())

            log.Append(now.ToString(DebugLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            log.Append(msg)

            LoggerUtility.TraceLoggerInstance.TraceEvent(TraceEventType.Verbose, TraceCategory.AppDebug, log.ToString)

        End Sub
#End Region

#Region "Perform"
        Private Shared PerformLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' �T�[�o�[�������Ԃ����O�o�͂��܂��B�i���O���x���FINFO�j
        ''' </summary>
        ''' <param name="msg">���b�Z�[�W</param>
        ''' <remarks>���O���o�͂��܂��B�i���O���x���FINFO�j</remarks>
        Public Shared Sub Perform(ByVal msg As String)

            If TraceOff Then Return
            Dim now As DateTime = DateTime.Now

            ''�o�͐ݒ�ł͂Ȃ��̂ŏI��
            If Not LoggerUtility.IsEnablePerformLogSetting Then
                Return
            End If

            If PerformLogDateTimeFormat Is Nothing Then
                Dim listener = LoggerUtility.PerformLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                If listener Is Nothing Then
                    PerformLogDateTimeFormat = DefaultLogDateTimeFormat
                Else
                    PerformLogDateTimeFormat = listener.LogDateTimeFormat
                End If
            End If


            Dim log As New StringBuilder
            ''�w�b�_�[(�Z�b�V����ID�A�A�J�E���g�A�����A���ID)�܂ō쐬
            log.Append(LoggerUtility.CreateWebHeader())

            log.Append(now.ToString(PerformLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            log.Append(msg)

            LoggerUtility.PerformLoggerInstance.TraceEvent(TraceEventType.Information, TraceCategory.AppInformation, log.ToString)
        End Sub
#End Region

#Region "PerformError"
        Private Shared PerformErrorLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' �T�[�o�[�������ԃG���[�����O�o�͂��܂��B�i���O���x���FERROR�j
        ''' </summary>
        ''' <param name="msg">���b�Z�[�W</param>
        ''' <remarks>���O���o�͂��܂��B�i���O���x���FINFO�j</remarks>
        Public Shared Sub PerformError(ByVal msg As String)

            If TraceOff Then Return
            Dim now As DateTime = DateTime.Now

            ''�o�͐ݒ�ł͂Ȃ��̂ŏI��
            If Not LoggerUtility.IsEnablePerformErrorLogSetting Then
                Return
            End If

            If PerformErrorLogDateTimeFormat Is Nothing Then
                Dim listener = LoggerUtility.PerformErrorLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                If listener Is Nothing Then
                    PerformErrorLogDateTimeFormat = DefaultLogDateTimeFormat
                Else
                    PerformErrorLogDateTimeFormat = listener.LogDateTimeFormat
                End If
            End If

            Dim log As New StringBuilder
            ''�w�b�_�[(�Z�b�V����ID�A�A�J�E���g�A�����A���ID)�܂ō쐬
            log.Append(LoggerUtility.CreateWebHeader())

            log.Append(now.ToString(PerformErrorLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            log.Append(msg)

            LoggerUtility.PerformErrorLoggerInstance.TraceEvent(TraceEventType.Error, TraceCategory.ProcessOverThreshold, log.ToString)
        End Sub
#End Region


    End Class
End Namespace