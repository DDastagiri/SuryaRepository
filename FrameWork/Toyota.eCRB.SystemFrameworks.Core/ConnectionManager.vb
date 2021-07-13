'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports Oracle.DataAccess.Client
Imports System.Threading.Thread
Imports Toyota.eCRB.SystemFrameworks.Configuration

Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' �f�[�^�x�[�X�Ƃ̐ڑ����Ǘ�����t�@�N�g���B
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class ConnectionManager

        ' ''' <summary>
        ' ''' �R���X�g���N�^�ł��B����ł��B
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Private Sub New()
        '    ' Do nothing.
        'End Sub

        ''' <summary>
        ''' Oracle�f�[�^�x�[�X�Ƃ̐ڑ����J���A�ڑ���߂��܂��B�ڑ��͎��s���Ă����g���C���s���܂��B
        ''' </summary>
        ''' <returns>Oracle�f�[�^�x�[�X�Ƃ̐ڑ��B</returns>
        ''' <exception cref="OracleException">
        '''   Oracle�f�[�^�x�[�X�ւ̐ڑ��̃��g���C�����ׂĎ��s�����Ƃ��ɔ������܂��B</exception>
        ''' <remarks></remarks>
        Public Function OpenConnection(ByVal targetDB As DBQueryTarget) As OracleConnection
            'Public Shared Function OpenConnection(ByVal targetDB As DBQueryTarget) As OracleConnection

            Dim connection As New OracleConnection()

            If targetDB = DBQueryTarget.iCROP Then
                connection.ConnectionString = SystemConfiguration.Current.GetRuntimeSetting(SystemConfigurationType.iCROPConnectionString)
            ElseIf targetDB = DBQueryTarget.DMS Then
                connection.ConnectionString = SystemConfiguration.Current.GetRuntimeSetting(SystemConfigurationType.DMSConnectionString)
            End If

            ''Try
            If connection.State = ConnectionState.Closed Then
                connection.Open()
            End If
            'Catch e As OracleException

            '    ' Oracle�ɐڑ����s�������߁A���g���C
            '    Dim config As ClassSection = SystemConfiguration.Current.Manager.ConnectionManager
            '    Dim setting As Setting = config.GetSetting(String.Empty)
            '    Dim maxRetry As Int32 = DirectCast(setting.GetValue("MaxConnectionOpenRetry"), Int32)
            '    Dim waitMSec As Int32 = DirectCast(setting.GetValue("ConnectionOpenRetryWaitMSec"), Int32)

            '    maxRetry = maxRetry - 1

            '    For i As Integer = 0 To maxRetry

            '        Sleep(waitMSec)

            '        Try

            '            If connection.State = ConnectionState.Closed Then
            '                connection.Open()
            '            End If

            '        Catch ee As OracleException

            '            ' ���g���C�񐔂��ő�ɒB�������O�𔭐�
            '            If maxRetry <= i Then

            '                Throw

            '            End If

            '            Continue For

            '        End Try

            '        ' Oracle�ɐڑ��ł����烋�[�v�𔲂���
            '        Exit For

            '    Next

            'End Try

            Return connection

        End Function

    End Class

End Namespace
