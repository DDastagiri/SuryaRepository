Imports Toyota.eCRB.iCROP.DataAccess.IC3040404
Imports System.Web
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization.CultureInfo

Namespace IC3040404.BizLogic

    ''' <summary>
    ''' リクエストヘッダの情報を処理・格納するクラス   2012/12/12
    ''' </summary>
    ''' <remarks>
    ''' BASIC認証の情報を含む
    ''' チェックインの確認
    ''' </remarks>
    Public Class RequestInfo

        'Private Variable
        Private InRequest As HttpRequest    'リクエスト情報
        Private InStrHead As String         'リクエストヘッダ
        Private InStrKey As String          'ヘッダのリクエストKey
        Private InStrUser As String         'ユーザー名
        Private InStrPass As String         'パスワード
        Private InOpeCode As String         'オペレーションコード 8または9

        'ヘッダとコンペアする定数
        Private Const StrAuthorize As String = "Authorization"
        Private Const StrContentLen As String = "Content-Length"
        Private Const IfMatch As String = "If-Match"
        Private Const IfNoneMatch As String = "If-None-Match"

        Private InEncode As Text.Encoding
        Private InPassedCertify As Boolean  '認証OK

        Private InMapPath As String         'マッピングのパス
        Private InRequestPath As String     'リクエストのパス

        Private InCtagPath As String        'ctag用のパス　エントリー時に設定 /account もしくは /
        Private InCtagDate As DateTime      'ctag用の日付　最初のリクエスト時に読込　レコードがなければ設定しその値

        Private InMatch As Boolean          'ヘッダにIf-Matchが見つかったとき
        'Private InNoneMatch As Boolean      'ヘッダにIf-None-Matchが見つかったとき

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="Request">HTTPリクエスト全部</param>
        ''' <param name="MapPath"></param>
        ''' <param name="RequestPath"></param>
        ''' <remarks>
        ''' コンストラクタはRequest,MapPath,RequetPathが必須
        ''' </remarks>
        Sub New(request As HttpRequest, mapPath As String, requestPath As String)
            InRequest = request
            InMapPath = mapPath
            InRequestPath = requestPath

            InMatch = False
            'InNoneMatch = False
        End Sub

        ''' <summary>
        ''' オペレーションコードを返す
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GetOpeCode As String
            Get
                Return InOpeCode
            End Get
        End Property

        ''' <summary>
        ''' TBL_CAL_CARD_LASTMODIFYを参照し、最終更新日時を得る
        ''' データがない場合はテーブルを更新し、現在の日時を返す
        ''' </summary>
        ''' <param name="upd">Trueのとき現時間で更新</param>
        ''' <returns>CtagDateの値
        ''' </returns>
        ''' <remarks></remarks>
        Public Function GetCtagDate(ByVal upd As Boolean) As DateTime
            Logger.Debug("[IC3040404RequestInfo:GetCtagDate] Start:" & InStrUser)

            Dim CtagDate As DateTime
            Dim StrUser As String = InStrUser
            Dim Answer As Boolean = False

            Using ModifyInfo As New DataAccess.IC3040404.IC3040404.Api.DataAccess.IC3040404DataTable
                Dim Ret As IC3040404DataSet.TableDataTableDataTable = ModifyInfo.GetLastModify(StrUser)
                Dim DataTable As Data.DataTable = Ret

                If DataTable.Rows.Count > 0 Then
                    'データがあった場合
                    If upd Then 'UPDATEフラグのときは現在時間で更新
                        'Update
                        Dim UpdateData As New DataAccess.IC3040404.IC3040404.Api.DataAccess.CalCardLastModify
                        Dim UpdateTime As DateTime = Now

                        With UpdateData
                            .StaffCD = StrUser
                            .CalUpdateDate = UpdateTime
                            .UpdateDate = UpdateTime
                            .UpdateAccount = StrUser
                            .UpdateId = GlobalConst.CalDavProgramId
                        End With
                        ModifyInfo.UpdateCalCardLastModify(UpdateData)
                        InCtagDate = UpdateTime
                    Else
                        InCtagDate = DataTable.Rows(0).Item("CALUPDATEDATE")
                    End If

                    Answer = True
                Else
                    'データがない場合 生成
                    Dim InsertData As New DataAccess.IC3040404.IC3040404.Api.DataAccess.CalCardLastModify
                    Dim InsertTime As DateTime = Now()

                    With InsertData
                        .StaffCD = StrUser
                        .CalUpdateDate = InsertTime
                        .CardUpdateDate = InsertTime '変更　Nothing → Now
                        .CreateDate = InsertTime
                        .UpdateDate = InsertTime
                        .CreateAccount = StrUser
                        .UpdateAccount = StrUser
                        .CreateId = GlobalConst.CalDavProgramId
                        .UpdateId = GlobalConst.CalDavProgramId
                    End With

                    InCtagDate = InsertTime
                    ModifyInfo.InsertCalCardLastModify(InsertData)

                End If

                CtagDate = InCtagDate
            End Using

            Dim ProcKind As String
            If Answer Then
                ProcKind = "(Update)"
            Else
                ProcKind = "(New)"
            End If

            Logger.Debug(" [IC3040404RequestInfo:GetCtagDate] Exit ctagDate" & ProcKind & ":" & CtagDate.ToString(CurrentCulture()))

            Return CtagDate
        End Function

        ''' <summary>
        ''' User名を返す
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GetUser
            Get
                Return InStrUser
            End Get
        End Property

        ''' <summary>
        ''' マップパスを返す
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GetMapPath As String
            Get
                Return InMapPath
            End Get
        End Property

        ''' <summary>
        ''' 新規のときにTrueを返す
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsInsert As String
            Get
                If InMatch Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        ''' <summary>
        ''' ctagのpathを返す
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GetCtagPath
            Get
                Return InCtagPath
            End Get
        End Property

        ''' <summary>
        ''' リクエストパスを返す
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GetReqPath
            Get
                Return InRequestPath
            End Get
        End Property


        ' ''' <summary>
        ' ''' ヘッダ情報を得る
        ' ''' </summary>
        ' ''' <returns>
        ' ''' 認証ができればTrue
        ' ''' </returns>
        ' ''' <remarks>
        ' ''' この関数でBASIC認証処理も行い、
        ' ''' 認証が通れば関数の返り値でTrueを返す
        ' ''' </remarks>
        'Public Function GetHeaderInfo() As Boolean
        '    Dim Certify As Boolean = False

        '    'ヘッダからBASIC認証を行う
        '    Certify = CertifyProc()

        '    '認証がOKのときのみヘッダ情報を読む
        '    '　ユーザーがわかるのでパスも設定する
        '    If Certify Then
        '        ReadHeaderInfo()
        '        InCtagPath = "/" & InStrUser
        '    End If

        '    Return Certify     '認証結果

        'End Function

        ''' <summary>
        ''' この関数でBASIC認証処理も行い、
        ''' 認証が通れば関数の返り値でTrueを返す
        ''' </summary>
        ''' <returns>認証ができればTrue</returns>
        ''' <remarks></remarks>
        ReadOnly Property GetHeaderInfo As Boolean
            Get
                Dim Certify As Boolean = False

                'ヘッダからBASIC認証を行う
                Certify = CertifyProc()

                '認証がOKのときのみヘッダ情報を読む
                '　ユーザーがわかるのでパスも設定する
                If Certify Then
                    ReadHeaderInfo()
                    InCtagPath = "/" & InStrUser
                End If

                Return Certify     '認証結果

            End Get
        End Property


        ''' <summary>
        ''' ヘッダ情報を読む
        ''' </summary>
        ''' <remarks>
        ''' Dummy Function
        ''' </remarks>
        Shared Sub ReadHeaderInfo()

            'Dummy Function

        End Sub


        ''' <summary>
        ''' ヘッダから認証を行う
        ''' </summary>
        ''' <remarks></remarks>
        Private Function CertifyProc() As Boolean

            'Log書き込み
            Logger.Debug("[IC3040404RequestInfo:CertifyProc] Start Method:" & InRequest.HttpMethod.ToString)

            InStrKey = ""  'リクエストKey　格納用
            InStrHead = "" 'リクエストヘッダ

            InPassedCertify = False '認証NG
            For i As Integer = 0 To InRequest.Headers.Count - 1
                InStrKey = InRequest.Headers.Keys(i).ToString
                InStrHead = InRequest.Headers(i).ToString
                Logger.Debug(InStrKey & " ::: " & InStrHead)

                'Matchを検出
                If InStrKey.IndexOf(IfMatch, StringComparison.CurrentCulture) >= 0 Then
                    InMatch = True
                End If
                'If InStrKey.IndexOf(IfNoneMatch, GlobalConst.Culture) >= 0 Then
                '    InNoneMatch = True
                'End If

                If IsCertify() Then
                    '認証リクエストあり
                    InPassedCertify = True

                    'Log出力(出力確認ログ）
                    'Logger.Info("[IC3040404RequestInfo:CertifyProc]" _
                    '            & "User" & " ::: " & _strUser & "  Password" & " ::: " & _strPass, True)
                    'パスワードは非表示
                    Logger.Debug("[IC3040404RequestInfo:CertifyProc]" _
                                & "User" & " ::: " & InStrUser)
                End If

            Next

            'リクエストBODYを読む(XMLの読込み）
            Dim StreamRequest As System.IO.StreamReader _
             = New System.IO.StreamReader(InRequest.InputStream())

            StreamRequest.Close()
            Logger.Debug(" [IC3040404RequestInfo:CertifyProc] Exit" & InRequest.HttpMethod.ToString)

            Return InPassedCertify

        End Function


        ''' <summary>
        ''' キーとヘッダを参照しBASIC認証する
        ''' </summary>
        ''' <returns>認証が通ればTrue</returns>
        ''' <remarks>
        ''' DBと照合し認証するが、現状はAuthorizationがあればOK
        ''' </remarks>
        Private Function IsCertify() As Boolean
            '戻り値
            Dim Answer As Boolean = False

            Try
                If InStrKey.IndexOf(StrAuthorize, StringComparison.CurrentCulture) >= 0 Then
                    'Authrization項目があった場合、BASIC認証を行う

                    'UserとパスワードをBASE64デコードする

                    'ヘッダをスペースで分離
                    Dim StrBasic() As String = InStrHead.Split(" ")
                    '2項目めをデコードする
                    Dim ConvStr As String = Decode(StrBasic(1), "UTF-8")

                    ':区切りを分離する
                    Dim StrInfo() As String = ConvStr.Split(":")

                    '返す値
                    InStrUser = StrInfo(0)  '1番目はUser
                    InStrPass = StrInfo(1)  '2番目はPassword

                    'BASIC認証を行う
                    Answer = CheckPassword(InStrUser, InStrPass)

                End If
            Catch ex As ApplicationException
                Logger.Error("[RequestInfo]IsCertify Error 認証処理でエラー:" & ex.ToString)

            End Try

            Return Answer

        End Function


        ''' <summary>
        ''' パスワードのチェック
        ''' </summary>
        ''' <param name="StrUser"></param>
        ''' <param name="StrPass"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' </remarks>
        Private Function CheckPassword(StrUser As String, StrPass As String) As Boolean
            Dim Ret As Boolean = False

            'ファイルから認証
            Dim Password As String = GetPassword(StrUser)
            If StrPass = Password Then
                Ret = True
            End If

            Return Ret
        End Function

        ''' <summary>
        ''' アカウントを与えパスワードを得る
        ''' </summary>
        ''' <param name="Account"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetPassword(account As String) As String

            Dim Password As String = ""
            InOpeCode = ""

            Try
                Dim Users As New Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic.Users
                Password = Users.GetUser(account, Nothing).Item("PASSWORD")
                InOpeCode = Users.GetUser(account, Nothing).Item("OPERATIONCODE") 'Add 2011/12/9

            Catch ex As ApplicationException

                Logger.Error("[RequestInfo]GetPassword Error パスワード情報が見つかりません")
            End Try

            Return Password
        End Function


        ''' <summary>
        ''' エンコード（変換）
        ''' </summary>
        ''' <param name="InpStr">エンコードする文字列</param>
        ''' <param name="EncStr">エンコード種別（既定値UTF-8）</param>
        ''' <returns>エンコードした文字列</returns>
        ''' <remarks>
        ''' ToBase64Stringのラッパ関数
        ''' </remarks>
        Public Function Encode(ByVal inpStr As String, ByVal encStr As String) As String
            InEncode = Encoding.GetEncoding(encStr)
            Return Convert.ToBase64String(InEncode.GetBytes(inpStr))
        End Function


        ''' <summary>
        ''' デコード（復元）
        ''' </summary>
        ''' <param name="InpStr">デコードする文字列</param>
        ''' <param name="EncStr">エンコード種別（既定値UTF-8）</param>
        ''' <returns>デコードした文字列</returns>
        ''' <remarks>
        ''' FromBase64Stringのラッパ関数
        ''' </remarks>
        Public Function Decode(ByVal inpStr As String, ByVal encStr As String) As String
            InEncode = Encoding.GetEncoding(encStr)
            Return InEncode.GetString(Convert.FromBase64String(inpStr))
        End Function

    End Class

End Namespace

