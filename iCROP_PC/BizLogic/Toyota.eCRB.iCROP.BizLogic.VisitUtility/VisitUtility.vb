'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'VisitUtility.vb
'──────────────────────────────────
'機能： 来店機能の共通ロジック
'補足： 
'作成： yyyy/MM/dd KN  x.xxxxxx
'更新： 2012/02/13 KN  y.nakamura STEP2開発 $01
'更新： 2012/08/08 TMEJ 瀧 PUSHされない原因を調査
'更新： 2013/06/13 TMEJ t.shimamura 既存流用 $02
'更新： 2019/05/21 NSK M.Sakamoto 18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策
'──────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Web
Imports System.Globalization
Imports System.Xml
Imports Toyota.eCRB.Visit.Api.DataAccess
Imports Toyota.eCRB.Visit.Api.DataAccess.VisitUtilityDataSetTableAdapters
Imports Toyota.eCRB.SystemFrameworks.Web

''' <summary>
''' 来店機能の共通ロジックです。
''' </summary>
''' <remarks></remarks>
Public Class VisitUtility

#Region "表示文字列の調整"

    ''' <summary>
    ''' 文字列をデコードする
    ''' </summary>
    ''' <param name="original">調整対象文字列</param>
    ''' <param name="decodes">デコードフラグ</param>
    ''' <returns>デコードされた文字列</returns>
    ''' <remarks></remarks>
    Private Function HtmlDecode(ByVal original As String, ByVal decodes As Boolean) As String

        ' Logger.Info("HtmlDecode_Start Param[" & original & "," & decodes & "]")

        ' デコードされた文字列
        Dim decoded As String = Nothing

        ' デコードする場合
        If decodes Then
            ' Logger.Info("HtmlDecode_001 Decodes")
            decoded = HttpUtility.HtmlDecode(Trim(original))

            ' デコードしない場合
        Else
            ' Logger.Info("HtmlDecode_002 Not decodes")
            decoded = Trim(original)
        End If

        ' Logger.Info("HtmlDecode_End Ret[" & decoded & "]")

        ' 戻り値にデコードされた文字列を設定
        Return decoded

    End Function

    ''' <summary>
    ''' 文字列をエンコードする
    ''' </summary>
    ''' <param name="adjusted">調整された文字列</param>
    ''' <param name="decodes">デコードフラグ</param>
    ''' <returns>エンコードされた文字列</returns>
    ''' <remarks></remarks>
    Private Function HtmlEncode(ByVal adjusted As String, ByVal decodes As Boolean) As String

        ' Logger.Info("HtmlEncode_Start Param[" & adjusted & "," & decodes & "]")

        ' エンコードされた文字列
        Dim encoded As String = Nothing

        ' デコードした場合
        If decodes Then
            ' Logger.Info("HtmlEncode_001 Encodes")
            encoded = HttpUtility.HtmlEncode(adjusted)

            ' デコードしない場合
        Else
            ' Logger.Info("HtmlEncode_002 Not encodes")
            encoded = adjusted
        End If

        ' Logger.Info("HtmlEncode_End Ret[" & encoded & "]")

        ' 戻り値にエンコードされた文字列を設定
        Return encoded

    End Function

    ''' <summary>
    ''' 指定された文字数よりも長い文字列の場合に文字列の末尾部分をカットする。
    ''' </summary>
    ''' <param name="original">カット対象文字列</param>
    ''' <param name="length">文字数</param>
    ''' <param name="decodes">デコードフラグ</param>
    ''' <returns>カットされた文字列</returns>
    ''' <remarks></remarks>
    Public Function CutTailString( _
            ByVal original As String, ByVal length As Integer, _
            Optional ByVal decodes As Boolean = False) As String

        ' Logger.Info("CutTailString_Start Param[" & original & "," & length & "," & decodes & "]")

        ' カットされた文字列
        Dim returnValue As String = Nothing
        ' 編集する文字列をデコード
        Dim target As String = HtmlDecode(original, decodes)

        ' 空文字の場合
        If String.IsNullOrEmpty(target) Then
            ' Logger.Info("CutTailString_001 IsNullOrEmpty")
            ' Logger.Info("CutTailString_End Ret[" & returnValue & "]")
            Return returnValue
        End If

        ' Logger.Info("CutTailString_002")

        ' 調整した文字列
        Dim adjusted As String = Nothing

        ' 指定された文字数以下の場合
        If target.Length <= length Then
            ' Logger.Info("CutTailString_003 SmallLength")
            adjusted = target

            ' 指定された文字数よりも長い場合
        Else
            ' Logger.Info("CutTailString_004 LargeLength")
            adjusted = target.Substring(0, length)
        End If

        ' 調整した文字列をエンコード
        returnValue = HtmlEncode(adjusted, decodes)

        ' Logger.Info("CutTailString_End Ret[" & returnValue & "]")

        ' 戻り値にカットされた文字列を設定
        Return returnValue

    End Function

    ''' <summary>
    ''' 指定された文字数よりも長い文字列の場合に文字列の先頭部分をカットする。
    ''' </summary>
    ''' <param name="original">カット対象文字列</param>
    ''' <param name="length">文字数</param>
    ''' <param name="decodes">デコードフラグ</param>
    ''' <returns>カットされた文字列</returns>
    ''' <remarks></remarks>
    Public Function CutHeadString( _
            ByVal original As String, ByVal length As Integer, _
            Optional ByVal decodes As Boolean = False) As String

        ' Logger.Info("CutHeadString_Start Param[" & original & "," & length & "," & decodes & "]")

        ' カットされた文字列
        Dim returnValue As String = Nothing
        ' 編集する文字列をデコード
        Dim target As String = HtmlDecode(original, decodes)

        ' 空文字の場合
        If String.IsNullOrEmpty(target) Then
            ' Logger.Info("CutHeadString_001 IsNullOrEmpty")
            ' Logger.Info("CutHeadString_End Ret[" & returnValue & "]")
            Return returnValue
        End If

        ' Logger.Info("CutHeadString_002")

        ' 調整した文字列
        Dim adjusted As String = Nothing

        ' 指定された文字数以下の場合
        If target.Length <= length Then
            ' Logger.Info("CutHeadString_003 SmallLength")
            adjusted = target

            ' 指定された文字数よりも長い場合
        Else
            ' Logger.Info("CutHeadString_004 LargeLength")
            adjusted = target.Substring(target.Length - length)
        End If

        ' 調整した文字列をエンコード
        returnValue = HtmlEncode(adjusted, decodes)

        ' Logger.Info("CutHeadString_End Ret[" & returnValue & "]")

        ' 戻り値にカットされた文字列を設定
        Return returnValue

    End Function

#End Region

#Region "Push送信"

    ''' <summary>
    ''' アプリケーション設定 - Pushサーバー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppSettingPushServerAddress As String = "PushServerAddress"

    ''' <summary>
    ''' アプリケーション設定 - Pushサーバー（PC基盤用）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppSettingPushServerAddressPC As String = "PushServerAddressPC"

    ''' <summary>
    ''' Push送信のリクエストメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RequstMethod As String = "POST"

    ''' <summary>
    ''' Push送信のリクエストタイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RequstContentType As String = "application/x-www-form-urlencoded"

    ' $01 start step2開発
    ''' <summary>
    ''' TBL_USERS フィールド名：在席分類
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PresenceCategory As String = "PRESENCECATEGORY"

    ''' <summary>
    ''' 在席状態：オフライン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StaffStatusOffline As String = "4"
    ' $01 end   step2開発

    ' $02 start 
    ''' <summary>
    ''' GWサーバドメイン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const GatewayServerDomain As String = "GATEWAY_SVR_DOMAIN"

    ''' <summary>
    ''' GetConnectInfoメソッドURL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const GetConnectInfoURL As String = "/Push/PushServiceConnectInfo.asmx/GetConnectInfo"

    ''' <summary>
    ''' Push送信処理URL
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LegacyMessageSendURL As String = "/Push/LegacyMessageSend.aspx"

    ''' <summary>
    ''' パラメータ：販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ParameterDlrcd As String = "dlrcd="

    ''' <summary>
    ''' レスポンスXMLのPushサーバアドレスノード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const XMLNodePushSerberURL As String = "//PushServiceConnectInfo/ConnectInfo"
    ' $02 end

    ' $01 start step2開発
    ''' <summary>
    ''' オンラインユーザの取得
    ''' </summary>
    ''' <param name="usersDataTable">ユーザのデータテーブル</param>
    ''' <returns>オンラインユーザ絞込み後のデータテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetOnlineUsers(ByVal usersDataTable As UsersDataSet.USERSDataTable) As UsersDataSet.USERSDataTable

        Logger.Info("VisitUtility.GetOnlineUsers_Start Param[InputUserDataTableCount = " & usersDataTable.Count & "]")

        'オンラインユーザ
        Dim onlineUsers As New UsersDataSet.USERSDataTable

        'すべてのデータ行
        For Each dr As UsersDataSet.USERSRow In usersDataTable
            'スタッフの在席状態取得
            Dim staffStatus As String = dr.Item(PresenceCategory).ToString
            '在席状態がオフライン、NULLではない場合にオンラインとする
            If (Not String.IsNullOrEmpty(staffStatus)) AndAlso (Not String.Equals(staffStatus, StaffStatusOffline)) Then
                ' Logger.Info("VisitUtility.GetOnlineUsers_001 OnlineUserAccount[" & dr.ACCOUNT & "]")
                'オンラインユーザへデータロウを追加
                onlineUsers.ImportRow(dr)
            Else
                ' Logger.Info("VisitUtility.GetOnlineUsers_002 OfflineUserAccount[" & dr.ACCOUNT & "]")
            End If
        Next

        Logger.Info("VisitUtility.GetOnlineUsers_End Ret[OutputUserDataTableCount = " & onlineUsers.Count & "]")

        Return onlineUsers
    End Function
    ' $01 end   step2開発

    ''' <summary>
    ''' Push送信を行う。
    ''' </summary>
    ''' <param name="postMsg">送信メッセージ</param>
    ''' <remarks></remarks>
    Public Sub SendPush(ByVal postMsg As String)

        ' Logger.Info("VisitUtility.SendPush_Start Send Param[" & postMsg & "]")

        SendPushPrivate(postMsg, AppSettingPushServerAddress)

        ' Logger.Info("VisitUtility.SendPush_End Send Ret[]")

    End Sub

    ''' <summary>
    ''' Push送信を行う（PC基盤用）。
    ''' </summary>
    ''' <param name="postMsg">送信メッセージ</param>
    ''' <remarks></remarks>
    Public Sub SendPushPC(ByVal postMsg As String)

        ' Logger.Info("VisitUtility.SendPushPC_Start Send Param[" & postMsg & "]")

        SendPushPrivate(postMsg, AppSettingPushServerAddressPC)

        ' Logger.Info("VisitUtility.SendPushPC_End Send Ret[]")

    End Sub

    ''' <summary>
    ''' Push送信を行う。
    ''' </summary>
    ''' <param name="postMsg">送信メッセージ</param>
    ''' <param name="pushServerAddressKey">アプリケーション設定（Pushサーバー）のキー名</param>
    ''' <remarks></remarks>
    ''' 
    ''' <history>
    ''' ''2012/08/08 TMEJ 瀧 PUSHされない原因を調査
    ''' </history>
    Private Sub SendPushPrivate(ByVal postMsg As String, ByVal pushServerAddressKey As String)

        Logger.Info("VisitUtility.SendPushPrivate_Start Send Param[" & postMsg & "]")

        '戻り値用変数
        Dim returnFlag As Boolean = True
        Dim context As StaffContext = StaffContext.Current
        Try

            ' POST送信する文字列をバイト配列に変換
            Dim postDataBytes() As Byte = Encoding.UTF8.GetBytes(postMsg)
            postMsg = Nothing
            ' $02 start Pushサーバアドレス取得方法の変更対応

            '18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 START
            ' システム設定より「ゲートウェイサーバドメイン」を取得する。
            'Dim gateWayAddress As VisitUtilityDataSet.VisitUtilityGateWayDomainDataTable

            'Using dataAdapter As New VisitUtilityDataSetTableAdapter

            '    gateWayAddress = dataAdapter.GetSystemSettingDealer(GatewayServerDomain)
            'End Using

            ''Pushサーバーアドレス
            'Dim pushServerAddress As String = String.Empty

            '' 接続先Pushサーバアドレスを取得する
            'Dim pushServerAddressURL As String = GetConnectInfo(pushServerDealerCode, CStr(gateWayAddress.Item(0)(0)))

            Dim systemSetting As New SystemSetting

            Dim row As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow _
                = systemSetting.GetSystemSetting(GatewayServerDomain)

            'Pushサーバーアドレス
            Dim pushServerAddress As String = String.Empty

            ' 接続先Pushサーバアドレスを取得する
            Dim pushServerAddressURL As String = GetConnectInfo(context.DlrCD, CStr(row.SETTING_VAL))
            '18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 END

            Logger.Info("VisitUtility.SendPushPrivate_001 pushServerAddress[" & pushServerAddressURL & "]")

            pushServerAddress = "http://" & pushServerAddressURL & LegacyMessageSendURL

            ' $02 end Pushサーバアドレス取得方法の変更対応

            ' POST送信するURL指定
            Dim uri As Uri = New Uri(pushServerAddress)
            Dim webRequst As WebRequest = WebRequest.Create(uri)

            webRequst.Method = RequstMethod
            webRequst.ContentType = RequstContentType
            webRequst.ContentLength = postDataBytes.Length

            'データをPOST送信するためのStreamを取得
            Using postStream As Stream = webRequst.GetRequestStream
                postStream.Write(postDataBytes, 0, postDataBytes.Length)
            End Using

            'Response取得
            Using webResponse As WebResponse = webRequst.GetResponse()

                Using streamReader As StreamReader = New StreamReader(webResponse.GetResponseStream())

                    Dim responseText = streamReader.ReadToEnd()
                    Logger.Info("VisitUtility.SendPushPrivate_002 responseText[" & responseText & "]")

                End Using

            End Using

        Catch ex As WebException
            ''2012/08/08 TMEJ 瀧 PUSHされない原因を調査 START
            'ステータス出力
            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                       "Status : {0}", _
                                       ex.Status))
            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                       "Message : {0}", _
                                       ex.Message))
            'HTTPプロトコルエラーかどうか調べる

            If ex.Status = WebExceptionStatus.ProtocolError _
                AndAlso ex.Response IsNot Nothing Then
                'HttpWebResponseを取得
                Dim errres As HttpWebResponse = _
                    CType(ex.Response, HttpWebResponse)
                '応答したURIを表示する
                Logger.Error(errres.ResponseUri.AbsolutePath)
                '応答ステータスコードを表示する
                Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}:{1}", _
                                            errres.StatusCode, _
                                            errres.StatusDescription))
            End If
            ''2012/08/08 TMEJ 瀧 PUSHされない原因を調査 END
        Catch ex As Exception

            'push失敗時のエラーを出力
            Logger.Info("VisitUtility.SendPushPrivate_003 pushFailed")
            Logger.Error("VisitUtility.SendPushPrivate_Error", ex)
            returnFlag = False

        End Try

        Logger.Info("VisitUtility.SendPushPrivate_End Send Ret[" & returnFlag & "]")

    End Sub
    ' $02 start Pushサーバアドレス取得方法の変更対応
    ''' <summary>
    ''' 接続情報の取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="gateWayDomain">ゲートウェイドメイン</param>
    Private Function GetConnectInfo(ByVal dlrcd As String, ByVal gateWayDomain As String) As String

        Dim connectInfo As String = ""
        Dim connectInfoURL As String = "http://" & gateWayDomain & GetConnectInfoURL
        Dim parameter As String = ParameterDlrcd & HttpUtility.UrlEncode(dlrcd) & "&"

        ' POST送信する文字列をバイト配列に変換
        Dim postDataBytes() As Byte = Encoding.UTF8.GetBytes(parameter)
        parameter = Nothing


        '  リクエストの作成
        Dim uri As Uri = New Uri(connectInfoURL)
        Dim webRequst As WebRequest = WebRequest.Create(uri)

        webRequst.Method = RequstMethod
        webRequst.ContentType = RequstContentType
        webRequst.ContentLength = postDataBytes.Length

        'データをPOST送信するためのStreamを取得
        Using postStream As Stream = webRequst.GetRequestStream
            postStream.Write(postDataBytes, 0, postDataBytes.Length)
        End Using
        Try
            '  レスポンスの取得と読み込み
            Using webResponse As WebResponse = webRequst.GetResponse()

                Using streamReader As StreamReader = New StreamReader(webResponse.GetResponseStream())

                    Dim responseText = streamReader.ReadToEnd()
                    Logger.Info("VisitUtility.SendPushPrivate_002 responseText[" & responseText & "]")

                    ' レスポンスから接続先を抽出
                    Dim Xml As New XmlDocument
                    Xml.LoadXml(responseText)
                    connectInfo = Xml.SelectSingleNode(XMLNodePushSerberURL).InnerText

                End Using

            End Using
        Catch ex As Exception
            connectInfo = Nothing

        End Try

        Return connectInfo
    End Function
    ' $02 end Pushサーバアドレス取得方法の変更対応
#End Region

End Class
