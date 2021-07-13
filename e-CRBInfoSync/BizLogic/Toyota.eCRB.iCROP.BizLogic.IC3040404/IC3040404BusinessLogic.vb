Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.IC3040404
Imports System.Web
Imports System.Text
Imports System.Xml
Imports System.Globalization

Namespace IC3040404.BizLogic

    Public Class IC3040404BusinessLogic
        Inherits BaseBusinessComponent
        Implements IDisposable

        Private InHeaderInfo As RequestInfo
        Private InGlobalVal As GlobalValue

        ''' <summary>
        ''' コンストラクタ 2012/12/12
        ''' </summary>
        ''' <remarks></remarks>
        Sub New()
            '
        End Sub

        ''' <summary>
        ''' CalDAV(同期=Sync)処理のメイン
        ''' </summary>
        ''' <param name="Response">HTTPレスポンス（Output)</param>
        ''' <param name="Request">HTTPリクエスト（Input)</param>
        ''' <param name="mapPath">実行パス（Input)</param>
        ''' <param name="reqPath">リクエストのパス（Input)</param>
        ''' <remarks>
        ''' 処理のメイン
        ''' </remarks>
        '''         
        <EnableCommit()>
        Public Sub CalDavMain(ByRef response As HttpResponse, ByVal request As HttpRequest, _
                              ByVal mapPath As String, ByVal reqPath As String)

            Logger.Info("[CalDavMain] Start Method:" & request.HttpMethod.ToString)

            Try

                'ヘッダ情報を得る
                InHeaderInfo = New RequestInfo(request, mapPath, reqPath)

                'リクエストヘッダのボディを読む(XML)
                Dim XmlDoc As XmlDocument = New XmlDocument()
                Dim StrBody As String = ""  'BODY文字列　PUTのときVCALENDARデータ
                'PROPFINDなどはワーク作業用（Xmlに代入

                'PUTとDELETEのときはXMLにしない
                Dim Str As String = request.HttpMethod.ToString
                If String.Equals(Str, "PUT") Then
                    'PUTのときはVCALENDARの文字列が来るので改行を入れる
                    StrBody = GetRequestXml(request, True)

                ElseIf Not String.Equals(Str, "DELETE") Then
                    'DELETEは何もしない（BODYなし）
                    StrBody = GetRequestXml(request, False)
                    If Not String.IsNullOrEmpty(StrBody) Then
                        Try
                            XmlDoc.LoadXml(StrBody)
                        Catch ex As ApplicationException
                            XmlDoc = Nothing
                        End Try
                    End If
                End If

                'グローバル変数のインスタンス作成
                InGlobalVal = New GlobalValue
                InGlobalVal.RootPath = mapPath  '実行のパスを代入（Load時しか取れないので）

                'レスポンス変数のインスタンス生成（コンストラクタ）
                Dim BizResponse As Response = New Response(response, request, InHeaderInfo, InGlobalVal, XmlDoc, StrBody)

                If InHeaderInfo.GetHeaderInfo() Then '認証OK
                    CalDavProc(BizResponse)          'メソッド処理を行う

                Else '認証NG　
                    BizResponse.RequestCertify()     'BASIC認証を要求

                End If

            Catch ex As Exception
                Logger.Info("*[CalDavMain]  Exception" & ex.ToString)
                response.StatusCode = GlobalConst.HttpStat500  '500 InternalError

            End Try

            Logger.Info(" [CalDavMain] Exit" & request.HttpMethod.ToString)
        End Sub


        ''' <summary>
        ''' リクエストのxmlを読む
        ''' </summary>
        ''' <param name="Request"></param>
        ''' <remarks></remarks>
        Shared Function GetRequestXml(ByVal request As HttpRequest, ByVal separate As Boolean) As String

            Logger.Info("[GetRequestXml] Start")

            'リクエストのBODYを読む(XMLの読込み）
            Dim StreamRequest As System.IO.StreamReader
            StreamRequest = New System.IO.StreamReader(request.InputStream())

            Dim StrBody As String = ""
            Dim StrStream As New StringBuilder
            Dim Attention As Boolean = False '通知フラグ（通知のみ途中に改行が入るので)
            Dim BodyList As New StringBuilder
            BodyList.Length = 0

            While Not StreamRequest.EndOfStream
                StrBody = StreamRequest.ReadLine

                '通知中で、次の行が継続でない場合は改行を入れる
                'If atnflag And Microsoft.VisualBasic.Left(sBody, 1) = " " Then
                If Attention And String.Equals(Microsoft.VisualBasic.Left(StrBody, 1), " ") Then
                    StrBody = StrBody.Trim '最初のスペースを除去
                    Attention = False
                End If

                'ATTENDEE の場合は改行を保留する
                'If Microsoft.VisualBasic.Left(sBody, 8) = "ATTENDEE" Then
                If String.Equals(Microsoft.VisualBasic.Left(StrBody, 8), "ATTENDEE") Then
                    Attention = True
                End If

                StrStream.Append(StrBody)
                'セパレータ（改行を入れる場合）
                If separate And Not Attention Then
                    StrStream.Append(vbCrLf)
                End If

                ' 2012/10/23 SKFC 浦野【iOS6対応】ログ改修 START
                'ログ用に保存
                BodyList.Append(StrBody & vbCrLf)

            End While

            Logger.Info("Request Body:" & BodyList.ToString, True)

            '以下のコメントをはずすと詳細をログに書きます
            'Logger.Debug("Request Body:" & BodyList.ToString)

            StreamRequest.Close()

            '通常のログ
            Logger.Info(" [GetRequestXml] Exit")
            ' 2012/10/23 SKFC 浦野【iOS6対応】ログ改修 END

            Return StrStream.ToString

        End Function


        ''' <summary>
        ''' メソッド処理
        ''' </summary>
        ''' <remarks>
        ''' サポートするVerbだけ列挙
        ''' </remarks>
        Shared Sub CalDavProc(ByRef bizResponse As Response)
            Logger.Info("[calDavProc] Start")

            Select Case BizResponse.StrMethod 'Method で処理を分ける
                Case "PUT"
                    '変更
                    bizResponse.ResPut()

                Case "GET"
                    '参照
                    bizResponse.ResGet()

                Case "POST"
                    '何もしないので呼び出し廃止 2011/12/30
                    '    BizResponse.ResPost()

                Case "HEAD"
                    '何もしないので呼び出し廃止 2011/12/30
                    '    bizResponse.ResHead()

                Case "PROPFIND"
                    'サーバー情報の取得等

                    bizResponse.ResPropFind()

                Case "OPTIONS"
                    'サーバー情報の取得等
                    bizResponse.ResOptions()

                Case "REPORT"
                    '同期
                    bizResponse.ResReport()

                Case "DELETE"
                    BizResponse.ResDelete()

                Case Else
                    'UNKNOWN
                    BizResponse.SetStatus(GlobalConst.HttpStat501) 'Not implemented

            End Select

            Logger.Info(" [calDavProc] Exit")
        End Sub


        ''' <summary>
        ''' Dispose
        ''' </summary>
        ''' <remarks>Using用のデストラクタ
        ''' </remarks>
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
