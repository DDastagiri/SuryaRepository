Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.BizLogic.IC3040601.IC3040601.BizLogic
Imports System.Collections.Generic

Partial Class Pages_IC3040601
    Inherits System.Web.UI.Page

    'Private enc As Encoding

    ''' <summary>
    ''' ロード時の処理 CardDAV用
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e"></param>
    ''' <remarks>イベントデータ</remarks>
    Private Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Logger.Info("[IC3040601:Page_Load] Start Method:" & Request.HttpMethod.ToString _
                    & "  PathInfo:" & Request.PathInfo)

        Try
            Using bizClass As New IC3040601BusinessLogic

                Dim mapPath As String = Server.MapPath("~")
                Logger.Debug("    Server.MapPath:" & mapPath)

                'リクエストパス情報
                Dim reqPath As String = Request.PathInfo
                Logger.Debug("    Request.PathInfo:" & reqPath)

                'ビジネスクラスのメインを呼ぶ
                bizClass.CardDavMain(Response, Request, mapPath, reqPath)

            End Using

        Catch ex As Exception
            Response.StatusCode = 500  '500 InternalError
            Logger.Error(" [IC3040601:Page_Load] Exception Error ex:" & ex.ToString)

        End Try

        Logger.Info(" [IC3040601:Page_Load] Exit(Normal)")

    End Sub

End Class

