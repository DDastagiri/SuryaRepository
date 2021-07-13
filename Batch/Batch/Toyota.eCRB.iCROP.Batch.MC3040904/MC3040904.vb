'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━'
'MC3040904.vb                                                              '
'─────────────────────────────────────'
'機能： ステータス変更                                                   　'
'補足：                                                                    '
'作成： 2012/02/16 TCS 小林                                                '
'─────────────────────────────────────'

Imports Toyota.eCRB.SystemFrameworks.Batch
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Tool.DataMaintenance.Batch.BizLogic
Imports System.Globalization

Public Class MC3040904
    Implements IBatch

#Region "メッセージ文言"

    Private message001 As String = BatchWordUtility.GetWord(901)
    Private message002 As String = BatchWordUtility.GetWord(902)

#End Region

#Region "バッチ終了コード"

    ''' <summary>
    ''' 正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SUCCESS As Integer = 0

#End Region

    Public Function Execute(args() As String) As Integer Implements SystemFrameworks.Batch.IBatch.Execute

        '開始ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, message001, String.Empty))

        Dim resultCode As Integer = SUCCESS

        Dim biz As New MC3040904BusinessLogic
        resultCode = biz.UpdateStatusInfo

        '終了ログ出力
        Logger.Info(String.Format(CultureInfo.InvariantCulture, message002, CStr(resultCode)))

        Return resultCode

    End Function

End Class
