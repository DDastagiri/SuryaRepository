'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports Oracle.DataAccess.Client
Imports System.Threading.Thread
Imports Toyota.eCRB.SystemFrameworks.Configuration

Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' データベースとの接続を管理するファクトリ。
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class ConnectionManager

        ' ''' <summary>
        ' ''' コンストラクタです。非可視です。
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Private Sub New()
        '    ' Do nothing.
        'End Sub

        ''' <summary>
        ''' Oracleデータベースとの接続を開き、接続を戻します。接続は失敗してもリトライを行います。
        ''' </summary>
        ''' <returns>Oracleデータベースとの接続。</returns>
        ''' <exception cref="OracleException">
        '''   Oracleデータベースへの接続のリトライがすべて失敗したときに発生します。</exception>
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

            '    ' Oracleに接続失敗したため、リトライ
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

            '            ' リトライ回数が最大に達したら例外を発生
            '            If maxRetry <= i Then

            '                Throw

            '            End If

            '            Continue For

            '        End Try

            '        ' Oracleに接続できたらループを抜ける
            '        Exit For

            '    Next

            'End Try

            Return connection

        End Function

    End Class

End Namespace
