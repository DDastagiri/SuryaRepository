Imports Toyota.eCRB.SystemFrameworks.Batch
Imports Toyota.eCRB.Tool.Rss.Batch.BizLogic

Public Class MC3040204
    Implements IBatch

    Public Function Execute(args() As String) As Integer Implements SystemFrameworks.Batch.IBatch.Execute
        Dim biz As New MC3040204BusinessLogic
        Return biz.RegistRssInfo()
    End Function
End Class
