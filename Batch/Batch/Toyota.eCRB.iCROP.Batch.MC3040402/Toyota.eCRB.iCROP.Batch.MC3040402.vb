Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Batch
Imports Toyota.eCRB.iCROP.BizLogic.MC3040402

Namespace Toyota.eCRB.iCROP.Batch
    Public Class MC3040402
        Implements IBatch

#Region "定数定義"
        ''' <summary>
        ''' モジュールID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MODULEID As String = "MC3040402"
        ''' <summary>
        ''' 空文字列
        ''' </summary>
        ''' <remarks></remarks>
        Private Const EmptyString As String = ""

#Region "バッチ終了コード"
        Private Enum Exitcode
            ''' <summary>
            ''' 正常終了
            ''' </summary>
            ''' <remarks></remarks>
            Nomarl = 0
            ''' <summary>
            ''' 異常終了
            ''' </summary>
            ''' <remarks></remarks>
            AbNomal = 10
        End Enum
#End Region

#Region "ログ出力メッセージID"
        ''' <summary>
        ''' MovePastCalendar関数用メッセージID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MSG_000 As Integer = 0
        Private Const MSG_101 As Integer = 101
        Private Const MSG_102 As Integer = 102
        Private Const MSG_103 As Integer = 103
#End Region

#Region "ログ出力レベル"
        Private Enum LOGLEVEL
            ERR = 1
            INFO = 2
            DEBUG = 3
        End Enum
#End Region


#End Region

#Region "呼び出し"
        Public Function Execute(ByVal args() As String) As Integer Implements SystemFrameworks.Batch.IBatch.Execute

            Dim result As Boolean

            OutputLog(MSG_101)

            'カレンダー情報削除処理（ビジネスロジック呼び出し）
            Using bizClass As New MC3040402BizLogic.MC3040402BussinessLogic

                ' ワークテーブルのトランケート処理
                result = bizClass.TruncateWKTable()

                If result = True Then
                    ' 退避カレンダーiCROP情報テーブル退避処理
                    result = bizClass.MovePastProc()
                End If

            End Using

            If result = True Then
                ' 正常終了
                OutputLog(MSG_102)
                Return Exitcode.Nomarl
            Else
                ' 異常終了
                OutputLog(MSG_103)
                Return Exitcode.AbNomal
            End If

        End Function
#End Region

#Region "Private関数"
        ''' <summary>
        ''' ログ出力処理
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub OutputLog(ByVal wordNo As Long)

            '出力メッセージの取得
            Dim LogMessage As String = BatchWordUtility.GetWord(MODULEID, wordNo)
            ' メッセージの編集
            LogMessage = CType(wordNo, String) & ":" & LogMessage

            'ログ出力
            Logger.Info(LogMessage)
            Logger.Debug(LogMessage)
        End Sub
#End Region

    End Class

End Namespace

