Imports Oracle.DataAccess.Client


Namespace Toyota.eCRB.SystemFrameworks.Core
    Public Class DBUpdateQuery
        Inherits DBQuery

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

        ''' <summary>
        ''' データ取得用メソッド(DML)
        ''' </summary>
        ''' <param name="timeout">コマンドタイムアウト</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function Execute(Optional ByVal timeout As Integer = 0) As Integer

            Using adapter As New OracleDataAdapter(Command)

                Dim count As Integer
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
                    StartTime = DateTime.Now
                    count = Command.ExecuteNonQuery()
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

    End Class
End Namespace