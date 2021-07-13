Imports Oracle.DataAccess.Client
Imports System.Data.Common
Imports System.Web

Namespace Toyota.eCRB.SystemFrameworks.Core
    Public Class DBSelectQuery(Of T As {New, DataTable})
        Inherits DBQuery

#Region "New"
        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="queryId">クエリID</param>
        ''' <param name="targetDatabase">DB接続先</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal queryId As String, Optional ByVal targetDatabase As DBQueryTarget = DBQueryTarget.iCROP)

            MyBase.New(queryId)
            targetDb = targetDatabase
        End Sub
#End Region

#Region "GetData"
        ''' <summary>
        ''' データ取得用メソッド(SELECT)
        ''' </summary>
        ''' <param name="timeout">コマンドタイムアウト</param>
        ''' <returns>DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetData(Optional ByVal timeout As Integer = 0) As T

            Using adapter As New OracleDataAdapter(Command)

                IsSuccess = False

                Try
                    If timeout = 0 Then
                        Command.CommandTimeout = DBTimeOut()
                    Else
                        Command.CommandTimeout = timeout
                    End If
                    Command.BindByName = True
                    If (Command.Connection Is Nothing) Then
                        Dim cm As New ConnectionManager
                        Command.Connection = cm.OpenConnection(TargetDB)
                    End If
                    Dim dt As New T()
                    StartTime = DateTime.Now
                    adapter.Fill(dt)
                    EndTime = DateTime.Now
                    IsSuccess = True
                    Return dt
                Catch ex As OracleException
                    Throw New OracleExceptionEx(ex, Command)
                Catch ex As Exception
                    Throw
                Finally
                    WriteTraceLog()
                    If IsSuccess Then
                        WriteSlowQueryLog()
                        WriteSqlTimeLog()
                    End If
                    If Not Command.Connection Is Nothing Then
                        Command.Connection.Close()
                        Command.Connection.Dispose()
                    End If
                End Try
            End Using

        End Function
#End Region

#Region "GetCount"
        ''' <summary>
        ''' データ取得用メソッド(SELECT COUNT)
        ''' </summary>
        ''' <param name="timeout">コマンドタイムアウト</param>
        ''' <returns>Count</returns>
        ''' <remarks></remarks>
        Public Function GetCount(Optional ByVal timeout As Integer = 0) As Integer

            Using adapter As New OracleDataAdapter(Command)

                Dim count As Integer
                IsSuccess = False

                Try
                    If (Command.Connection Is Nothing) Then
                        Dim cm As New ConnectionManager
                        Command.Connection = cm.OpenConnection(TargetDB)
                    End If
                    If timeout = 0 Then
                        Command.CommandTimeout = DBTimeOut()
                    Else
                        Command.CommandTimeout = timeout
                    End If
                    Command.BindByName = True
                    StartTime = DateTime.Now
                    count = CType(Command.ExecuteScalar(), Integer)
                    EndTime = DateTime.Now
                    IsSuccess = True
                    Return count
                Catch ex As OracleException
                    Throw New OracleExceptionEx(ex, Command)
                Catch ex As Exception
                    Throw
                Finally
                    WriteTraceLog()
                    If IsSuccess Then
                        WriteSlowQueryLog()
                        WriteSqlTimeLog()
                    End If
                    If Not Command.Connection Is Nothing Then
                        Command.Connection.Close()
                        Command.Connection.Dispose()
                    End If
                End Try
            End Using

        End Function
#End Region

    End Class
End Namespace
