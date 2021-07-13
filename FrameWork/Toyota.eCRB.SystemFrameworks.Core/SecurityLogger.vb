'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Text
Imports System.Web
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Configuration

Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' �Z�L�����e�B���O�̏o�͋@�\��񋟂���N���X�ł��B
    ''' ���O�̏o�͐ݒ�́A�O���t�@�C���Ƃ��Ē�`����܂��B
    ''' </summary>
    ''' <remarks>
    ''' �����o�ɃA�N�Z�X���邽�߂ɁA�ÓI�N���X�̃C���X�^���X��錾����K�v�͂���܂���B
    ''' ���̃N���X�̓A�Z���u���O�Ɍ��J���܂��B
    ''' ���̃N���X�͌p���ł��܂���B
    ''' </remarks>

    Public NotInheritable Class SecurityLogger

#Region "�萔"
        ''' <summary>
        ''' �����b�Z�[�W�擾�������܂��Ă��Ȃ����߁A�Œ蕶���o��
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SECURITY_LOGIN_MESSAGE As String = "login error"

        Private Const DefaultLogDateTimeFormat = "yyyy/MM/dd_HH:mm:ss.fff"

#End Region

#Region "�ϐ�"
        ''' <summary>
        ''' TraceLogger�C���X�^���X���i�[���܂��B
        ''' </summary>
        Private Shared _SecurityLogger As New TraceLogger("SecurityLog")

#End Region

#Region "SecurityLoggerInstance"
        ''' <summary>
        ''' TraceLogger�C���X�^���X���擾���܂��B
        ''' </summary>
        Private Shared ReadOnly Property SecurityLoggerInstance() As TraceLogger
            Get
                Return _SecurityLogger
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

#Region "Security"
        Private Shared SecurityLogDateTimeFormat As String = Nothing
        ''' <summary>
        ''' �Z�L�����e�B���O���o�͂���A�I�����C���pAPI�B
        ''' </summary>
        ''' <param name="msg">���b�Z�[�W</param>
        Public Shared Sub Security(ByVal msg As String)

            Dim now As DateTime = DateTime.Now

            ''�o�͐ݒ�ł͂Ȃ��̂ŏI��
            If Not LoggerUtility.IsEnableSecurityLogSetting Then
                Return
            End If

            If SecurityLogDateTimeFormat Is Nothing Then
                Dim listener = SecurityLoggerInstance.Listeners.OfType(Of Toyota.eCRB.SystemFrameworks.Core.TraceLogListener).FirstOrDefault
                If listener Is Nothing Then
                    SecurityLogDateTimeFormat = DefaultLogDateTimeFormat
                Else
                    SecurityLogDateTimeFormat = listener.LogDateTimeFormat
                End If
            End If


            Dim log As New StringBuilder  ''���O������
            log.Append(LoggerUtility.CreateWebHeader())
            log.Append(now.ToString(SecurityLogDateTimeFormat) & LoggerUtility.LogDelimiter)
            log.Append(msg)

            SecurityLoggerInstance.TraceInformation(log.ToString)

        End Sub
#End Region

    End Class
End Namespace