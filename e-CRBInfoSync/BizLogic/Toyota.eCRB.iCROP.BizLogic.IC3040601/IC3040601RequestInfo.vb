Imports Toyota.eCRB.iCROP.DataAccess.IC3040601
Imports System.Web
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace IC3040601.BizLogic

    ''' <summary>
    ''' リクエストヘッダの情報を処理・格納するクラス
    ''' </summary>
    ''' <remarks>
    ''' BASIC認証の情報を含む
    ''' </remarks>
    Public Class RequestInfo

        Private _request As HttpRequest 'リクエスト情報
        Private _strHead As String      'リクエストヘッダ
        Private _strKey As String
        Private _strUser As String      'ユーザー名
        Private _strPass As String      'パスワード

        Private Const STR_AUTH As String = "Authorization"
        Private Const STR_LENGTH As String = "Content-Length"

        Private _pEncode As Text.Encoding
        Private _PassedCertify As Boolean   '認証OK

        Private _mapPath As String
        Private _reqPath As String

        Private _DLRCD As String = ""
        Private _STRCD As String = ""

        Private _ctagDate As DateTime   'ctag用の日付　最初のリクエスト時に読込　レコードがなければ設定しその値

        Property Dlrcd As String
            Get
                Return _DLRCD
            End Get
            Set(ByVal value As String)
                _DLRCD = value
            End Set
        End Property

        Property Strcd As String
            Get
                Return _STRCD
            End Get
            Set(ByVal value As String)
                _STRCD = value
            End Set
        End Property


        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="Request"></param>
        ''' <remarks>
        ''' コンストラクタはRequestが必須
        ''' </remarks>
        Sub New(ByVal request As HttpRequest, ByVal mappath As String, ByVal reqPath As String)
            _request = request
            _mapPath = mappath
            _reqPath = reqPath
        End Sub


        ''' <summary>
        ''' User名を返す
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GetUser
            Get
                Return _strUser
            End Get
        End Property

        ''' <summary>
        ''' マップパスを返す
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property GetMapPath
            Get
                Return _mapPath
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
                Return _reqPath
            End Get
        End Property

#If 0 Then
        ''' <summary>
        ''' ヘッダ情報を得る
        ''' </summary>
        ''' <returns>
        ''' 認証ができればTrue
        ''' </returns>
        ''' <remarks>
        ''' この関数でBASIC認証処理も行い、
        ''' 認証が通れば関数の返り値でTrueを返す
        ''' </remarks>
        Public Function GetHeaderInfo() As Boolean
            Dim bCertify As Boolean = False

            'ヘッダからBASIC認証を行う
            bCertify = CertifyProc()

            '認証がOKのときのみヘッダ情報を読む
            If bCertify Then
                ReadHeaderInfo()
            End If

            Return bCertify     '認証結果

        End Function
#End If
        ''' <summary>
        ''' この関数でBASIC認証処理も行い、
        ''' 認証が通れば関数の返り値でTrueを返す
        ''' </summary>
        ''' <returns>認証ができればTrue</returns>
        ''' <remarks></remarks>
        ReadOnly Property GetHeaderInfo As Boolean
            Get
            Dim bCertify As Boolean = False

                'ヘッダからBASIC認証を行う
                bCertify = CertifyProc()

                '認証がOKのときのみヘッダ情報を読む
                If bCertify Then
                    ReadHeaderInfo()
                End If

                Return bCertify     '認証結果

            End Get
        End Property

        ''' <summary>
        ''' ヘッダ情報を読む
        ''' </summary>
        ''' <remarks>
        ''' スタブ
        ''' </remarks>
        Sub ReadHeaderInfo()

            '

        End Sub


        ''' <summary>
        ''' ヘッダから認証を行う
        ''' </summary>
        ''' <remarks></remarks>
        Private Function CertifyProc() As Boolean

            'Log書き込み
            Logger.Debug("[IC3040601RequestInfo:CertifyProc] Start Method:" & _request.HttpMethod.ToString)

            _strKey = ""  'リクエストKey　格納用
            _strHead = "" 'Key
            'Dim nLenRead As Integer '読み込んだLength(作業用)

            Dim i As Integer    '単純カウンタ
            '_nLen = 0
            _PassedCertify = False '認証NG
            For i = 0 To _request.Headers.Count - 1
                _strKey = _request.Headers.Keys(i).ToString
                _strHead = _request.Headers(i).ToString
                Logger.Debug(_strKey & " ::: " & _strHead)
                If bCertify() Then
                    '認証リクエストあり
                    _PassedCertify = True

                    'Log出力
                    Logger.Debug("[IC3040601RequestInfo:CertifyProc]" _
                                & "User" & " ::: " & _strUser)
                End If

                'nLenRead = nContentLength()
            Next

            'リクエストBODYを読む(XMLの読込み）
            Dim stRequest As System.IO.StreamReader
            stRequest = New System.IO.StreamReader(_request.InputStream())

            stRequest.Close()

            Logger.Debug(" [IC3040601RequestInfo:CertifyProc] Exit" & _request.HttpMethod.ToString)
            Return _PassedCertify

        End Function


        ''' <summary>
        ''' キーとヘッダを参照しBASIC認証する
        ''' </summary>
        ''' <returns>認証が通ればTrue</returns>
        ''' <remarks>
        ''' DBと照合し認証するが、現状はAuthorizationがあればOK
        ''' </remarks>
        Private Function bCertify() As Boolean
            '戻り値
            Dim bAns As Boolean = False

            Try
                If _strKey.IndexOf(STR_AUTH, StringComparison.CurrentCulture) >= 0 Then
                    'Authrization項目があった場合、BASIC認証を行う

                    'UserとパスワードをBASE64デコードする

                    'ヘッダをスペースで分離
                    Dim strBasic() As String = _strHead.Split(" ")
                    '2項目めをデコードする
                    Dim cnvStr As String = Decode(strBasic(1))

                    ':区切りを分離する
                    Dim strInfo() As String = cnvStr.Split(":")

                    '返す値
                    _strUser = strInfo(0)  '1番目はUser
                    _strPass = strInfo(1)  '2番目はPassword

                    'BASIC認証を行う
                    bAns = bCheckPassword(_strUser, _strPass)

                End If
            Catch ex As ApplicationException

            End Try

            Return bAns

        End Function

        ''' <summary>
        ''' TBL_CAL_CARD_LASTMODIFYを参照し、最終更新日時を得る
        ''' データがない場合はテーブルを更新し、現在の日時を返す
        ''' </summary>
        ''' <returns>DBから読み込んだ場合　DateTime時間
        ''' 設定した場合DateTime時間
        ''' どちらでも結果は
        ''' メンバ変数_ctagDate に入る
        ''' </returns>
        ''' <remarks></remarks>
        ReadOnly Property GetCtagDate As DateTime
            Get
                Logger.Debug("[IC3040601RequestInfo:GetCtagDate] Start")
                Dim ctagDate As DateTime = Nothing
                Dim strUser As String = _strUser

                Using modifyInfo As New DataAccess.IC3040601.IC3040601.Api.DataAccess.TblCalCardLastModify
                    Dim ret As IC3040601DataSet.TblCalCardLastModifyDataTable = modifyInfo.GetLastModifyInfo(strUser)

                    Dim dt As Data.DataTable = ret

                    If dt.Rows.Count > 0 Then
                        'データがあった場合
                        _ctagDate = dt.Rows(0).Item("CARDUPDATEDATE")
                    Else
                        'データがない場合 生成
                        'TODO:Insert
                        Dim indat As New DataAccess.IC3040601.IC3040601.Api.DataAccess.IC3040601CalCradLastModify
                        Dim dat As DateTime = Now

                        With indat
                            .Staffcd = strUser
                            .Calupdatedate = dat
                            .Cardupdatedate = dat
                            .Createdate = dat
                            .Updatedate = dat
                            .Createaccount = strUser
                            .Updateaccount = strUser
                            .Createid = GlobalConst.CARDDAV_PROGRM_ID
                            .Updateid = GlobalConst.CARDDAV_PROGRM_ID
                        End With

                        modifyInfo.InsertTblCalCardLastModify(indat)
                        _ctagDate = dat
                    End If

                    ctagDate = _ctagDate
                End Using

                Logger.Debug(" [IC3040601RequestInfo:GetCtagDate] Exit ctagDate" & ":" & ctagDate.ToString(Globalization.CultureInfo.CurrentCulture()))
                Return ctagDate

            End Get
        End Property

#If 0 Then

        ''' <summary>
        ''' 'Content-Length項目があった場合、文字数を返す
        ''' </summary>
        ''' <returns>文字列長を返す</returns>
        ''' <remarks>
        ''' 取得できない場合も0を返す</remarks>
        Private Function nContentLength() As Integer

            Dim nAns As Integer = 0 '戻り値

            Try
                If _strKey.IndexOf(STR_LENGTH) >= 0 Then
                    'Content-Length項目があった場合、文字数を返す

                    nAns = CInt(_strHead)

                End If
            Catch ex As ApplicationException

            End Try

            Return nAns

        End Function
#End If

        ''' <summary>
        ''' パスワードのチェック
        ''' </summary>
        ''' <param name="strUser"></param>
        ''' <param name="strPass"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function bCheckPassword(ByVal strUser As String, ByVal strPass As String) As Boolean
            Dim bRet As Boolean = False

            'パスワードのチェック
            'ファイルから認証
            Dim password As String = GetPassword(strUser)
            If strPass = password Then
                bRet = True
            End If

            Return bRet
        End Function


        ''' <summary>
        ''' アカウントを与えパスワードを得る
        ''' </summary>
        ''' <param name="Account"></param>
        ''' <returns></returns>
        ''' <remarks>_DLRCD、_STRCDをセット</remarks>
        Public Function GetPassword(ByVal account As String) As String
            Dim Password As String = ""
            Dim dlrcd As String = ""
            Dim strcd As String = ""

            Try
                Dim Users As New Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic.Users
                Password = Users.GetUser(account, Nothing).Item("PASSWORD")
                dlrcd = Users.GetUser(account, Nothing).Item("DLRCD")
                strcd = Users.GetUser(account, Nothing).Item("STRCD")

                Logger.Debug(" [IC3040601RequestInfo:GetPassword] " & "DLRCD =" & dlrcd & "STRCD =" & strcd)
            Catch ex As ApplicationException
                Logger.Error("GetPassword Error パスワード情報が見つかりません")
            End Try
            _DLRCD = dlrcd
            _STRCD = strcd

            Return Password
        End Function

        ''' <summary>
        ''' エンコード（変換）
        ''' </summary>
        ''' <param name="str"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' ToBase64Stringのラッパ関数
        ''' </remarks>
        Public Function Encode(ByVal str As String, Optional ByVal encStr As String = "UTF-8") As String
            _pEncode = Encoding.GetEncoding(encStr)
            Return Convert.ToBase64String(_pEncode.GetBytes(str))
        End Function


        ''' <summary>
        ''' デコード（復元）
        ''' </summary>
        ''' <param name="str"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' FromBase64Stringのラッパ関数
        ''' </remarks>
        Public Function Decode(ByVal str As String, Optional ByVal encStr As String = "UTF-8") As String
            _pEncode = Encoding.GetEncoding(encStr)
            Return _pEncode.GetString(Convert.FromBase64String(str))
        End Function

    End Class

End Namespace

