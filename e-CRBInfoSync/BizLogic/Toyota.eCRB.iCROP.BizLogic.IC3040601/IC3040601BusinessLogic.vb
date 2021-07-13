Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.IC3040601
Imports System.Web
Imports System.Text
Imports System.Xml

Namespace IC3040601.BizLogic

    Public Class IC3040601BusinessLogic
        Inherits BaseBusinessComponent
        Implements IDisposable

        Private clsHeaderInfo As RequestInfo
        Private clsGlobal As GlobalValue

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()

        End Sub


        ''' <summary>
        ''' CardDAV(同期=Sync)処理のメイン
        ''' </summary>
        ''' <param name="Response">HTTPレスポンスクラスデータ（Output)</param>
        ''' <param name="Request">ワーク用HTTPレスポンスクラスデータ（Input)</param>
        ''' <param name="mapPath">実行パスストリングデータ（Input)</param>
        ''' <param name="reqPath">リクエストのパスストリングデータ（Input)</param>
        ''' <remarks>
        ''' 処理の実質メイン
        ''' １．ヘッダ情報を得る
        ''' ２．リクエストヘッダのボディを読む(XML)
        ''' ３．グローバル変数のインスタンス作成
        ''' ４．レスポンス変数のインスタンス生成（コンストラクタ）
        ''' ５．BASIC認証
        ''' ６．メソッド処理
        ''' <returns>なし</returns>
        ''' <history>
        '''		[M.Kaneko]    2011/12/01    Created
        '''		[M.Kaneko]    2011/12/22    Replace Logger変更、Try Exception追加
        ''' </history>
        ''' </remarks>
        <EnableCommit()>
        Public Sub CardDavMain(ByRef response As HttpResponse, ByVal request As HttpRequest, _
                              ByVal mapPath As String, ByVal reqPath As String)

            Logger.Debug("[CardDavMain] Start Method:" & request.HttpMethod.ToString)

            Try
                '１．ヘッダ情報を得る
                clsHeaderInfo = New RequestInfo(request, mapPath, reqPath)

                '２．リクエストヘッダのボディを読む(XML)
                Dim xmlDoc As XmlDocument = New XmlDocument()
                If Not request.ContentLength = 0 Then
                    xmlDoc.Load(request.InputStream)
                End If

                '３．グローバル変数のインスタンス作成
                clsGlobal = New GlobalValue
                clsGlobal.StrRootPath = mapPath  '実行のパスを代入（Load時しか取れない）

                '４．レスポンス変数のインスタンス生成（コンストラクタ）
                Dim clsRes As Response = New Response(response, request, clsHeaderInfo, clsGlobal, xmlDoc)

                '５．BASIC認証
                If clsHeaderInfo.GetHeaderInfo() Then '認証OK
                    '６．CardDAVの処理
                    clsRes.Dlrcd = clsHeaderInfo.Dlrcd '販売店コード
                    clsRes.Strcd = clsHeaderInfo.Strcd '店舗コード
                    cardDavProc(clsRes)          'メソッド処理を行う

                Else '認証NG　
                    clsRes.RequestCertify()     'BASIC認証を要求

                End If

            Catch ex As Exception
                Logger.Error("*[CardDavMain]  Exception:" & ex.ToString)
                response.StatusCode = GlobalConst.HTTP_STAT_500  '500 InternalError
            End Try

            Logger.Debug(" [CardDavMain] Exit" & request.HttpMethod.ToString)

        End Sub


        ''' <summary>
        ''' リクエストのxmlを読む
        ''' </summary>
        ''' <param name="Request"></param>
        ''' <remarks></remarks>
        Function GetRequestXml(ByVal request As HttpRequest) As String
            Logger.Debug("[GetRequestXml] Start")

            'リクエストのBODYを読む(XMLの読込み）
            Dim stRequest As System.IO.StreamReader
            stRequest = New System.IO.StreamReader(request.InputStream())

            Dim sBody As String = ""
            Dim sStream As New StringBuilder

            While Not stRequest.EndOfStream
                sBody = stRequest.ReadLine
                sStream.Append(sBody)
            End While
            stRequest.Close()

            Logger.Info(sStream.ToString, True)
            Logger.Debug(" [GetRequestXml] Exit")
            Return sStream.ToString
        End Function


        ''' <summary>
        ''' CardDAVメソッド処理
        ''' </summary>
        ''' <remarks></remarks>
        ''' <param name="clsRes">CardDAVレスポンスクラスデータ</param>
        Sub CardDavProc(ByRef clsRes As Response)
            Logger.Debug("[CardDavProc] Start")

            Select Case clsRes.StrMethod 'Method で処理を分ける
                Case "PROPFIND"
                    clsRes.resPropFind()

                Case "OPTIONS"
                    clsRes.resOptions()

                Case "REPORT"
                    clsRes.resReport()

                Case Else
                    'UNKNOWN
                    clsRes.SetStatus(GlobalConst.HTTP_STAT_501) 'Not implemented

            End Select

            Logger.Debug(" [cardDavProc] Exit")
        End Sub


        ''' <summary>
        ''' Dispose
        ''' </summary>
        ''' <remarks></remarks>
        Public Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            ' This object will be cleaned up by the Dispose method.
            ' Therefore, you should call GC.SupressFinalize to
            ' take this object off the finalization queue 
            ' and prevent finalization code for this object
            ' from executing a second time.
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' ルールセット用ダミー関数
        ''' </summary>
        ''' <param name="Value"></param>
        ''' <remarks></remarks>
        Protected Overridable Overloads Sub Dispose(ByVal value As Boolean)
            'ルールセット用なのでダミー

        End Sub
    End Class
End Namespace
