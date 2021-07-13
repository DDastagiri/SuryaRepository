Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.BizLogic.IC3040404.IC3040404.BizLogic
Imports System.Collections.Generic
Imports System.Text
Imports System.Globalization.CultureInfo

Partial Class Pages_IC3040404
    Inherits BasePage

    'Private enc As Encoding

    ''' <summary>
    ''' 起動時の処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベント
    ''' </param>
    ''' <remarks>イベントデータをデバッグ印刷
    ''' ビジネスロジック（bizClass.CalDavMain）のメインをCallする
    ''' </remarks>
    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info("[IC3040404:Page_Load] Start Method:" & Request.HttpMethod.ToString _
                    & "  PathInfo:" & Request.PathInfo)

        Try
            Using bizClass As New IC3040404BusinessLogic

                Dim mapPath As String = Server.MapPath("~")
                Logger.Debug("    Server.MapPath:" & mapPath)

                'リクエストパス情報
                Dim reqPath As String = Request.PathInfo
                Logger.Debug("    Request.PathInfo:" & reqPath)

                'ビジネスクラスのメインを呼ぶ
                bizClass.CalDavMain(Response, Request, mapPath, reqPath)

            End Using

        Catch ex As Exception
            Logger.Error(" [IC3040404:Page_Load] Exception Error ex:" & ex.ToString)
            Response.StatusCode = 500  '500 InternalError

        End Try

        Logger.Info(" [IC3040404:Page_Load] Exit(Normal)")

    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete

    End Sub

End Class

