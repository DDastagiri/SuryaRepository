Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Xml
Imports System.Text
Imports Toyota.eCRB.iCROP.DataAccess.IC3040404
Imports Toyota.eCRB.iCROP.DataAccess
Imports System.Globalization.CultureInfo
Imports System.Xml.XPath

Namespace IC3040404.BizLogic

    ''' <summary>
    ''' レスポンス処理 
    ''' </summary>
    ''' <remarks>
    ''' 必要なものはこのクラスのメンバ変数に記憶
    ''' （基本はprivate)
    ''' </remarks>
    ''' <History>
    ''' 2012/10/12 SKFC 浦野【ios6対応】時差対応
    ''' </History>
    Public Class Response

        Private InResponse As HttpResponse
        Private InRequest As HttpRequest
        Private InHeaderInfo As RequestInfo
        Private InGlobal As GlobalValue
        Private InStrMethod As String
        Private InXml As XmlDocument
        Private InVevent As String 'VCALENDARのリクエスト文字列

        Private Const CrLF As String = GlobalConst.CrLf  '改行文字

        'ヘッダのパターン
        Private HeadPattern As Integer = 1

        '行区切りの文字列
        Private Const Separator As String = vbCrLf
        Private Const StatusOK As Integer = 200
        Private Const StatusNG As Integer = 404


        ' 2012/10/12 SKFC 浦野【iOS6対応】時差対応 START
        Private TimeLocale As String
        Private TimeFrom As String
        Private TimeTo As String
        Private TimeName As String
        ' 2012/10/12 SKFC 浦野【iOS6対応】時差対応 END

        Private Const StartDate As String = "19700101T000000"

        Private Const TimeDiff As Double = 8.0 'China Timeの時差

        'アラームのメッセージは固定値の様子　なのでここで定数設定
        Private Const ValarmMessage As String = "This is an event reminder"

        'Uidの件数
        Private InUidCount As Integer

        '開始・終了時間
        Private InStartDate As DateTime
        Private InEndDate As DateTime

        ''' <summary>
        ''' Setter Getter
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Property StrMethod As String
            Get
                Return InStrMethod
            End Get
            Set(ByVal value As String)
                InStrMethod = value
            End Set
        End Property


        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="Response">レスポンス情報</param>
        ''' <param name="Request">リクエスト情報</param>
        ''' <param name="HeaderInfo">ヘッダ情報</param>
        ''' <param name="Globals">基本情報</param>
        ''' <param name="xml">リクエストのxml情報</param>
        ''' <param name="Vevent">VCALENDARのリクエスト文字列</param>
        ''' <remarks>
        ''' xml情報は
        ''' </remarks>
        ''' <History>
        ''' 2012/10/12 SKFC 浦野【ios6対応】時差対応
        ''' </History>
        Public Sub New(ByRef response As HttpResponse, ByVal request As HttpRequest, _
                ByVal headerInfo As RequestInfo, ByVal globals As GlobalValue, ByVal xml As IXPathNavigable, _
                ByVal vevent As String)
            Logger.Info("[IC3040404Response:New(constructor)] Start")

            InResponse = response
            InRequest = request
            InHeaderInfo = headerInfo
            InGlobal = globals
            InXml = xml      '事前に読み込んでおく（_Requestから読めないので)
            InVevent = vevent
            InStrMethod = request.HttpMethod.ToString     'ヘッダのリクエスト

            ' 2012/10/12 SKFC 浦野【iOS6対応】時差対応 START
            TimeLocale = InGlobal.DefaultLocal
            TimeFrom = InGlobal.DefaultLocalTime.ToUpper.Replace("GMT", "").Replace("CST", "")
            TimeTo = TimeFrom
            TimeName = InGlobal.DefaultLocalTime
            ' 2012/10/12 SKFC 浦野【iOS6対応】時差対応 END


            Logger.Info(" [IC3040404Response:New(constructor)] exit")

        End Sub


        ''' <summary>
        ''' ひとつのエレメントを作成
        ''' </summary>
        ''' <param name="Res">レスポンス文字列</param>
        ''' <param name="StrSpace">名前空間</param>
        ''' <param name="Elm">エレメント名</param>
        ''' <param name="NotFound">TrueのときNotFound用のデータを作成する。
        ''' 　　　　この場合、 <tagName/> だけのデータとなる。既定値はFalse
        ''' </param>
        ''' <returns>作成したエレメント情報の文字列</returns>
        ''' <remarks></remarks>
        Private Shared Function MakeOneElement(ByVal Res As String, ByVal StrSpace As String, ByVal Elm As String, Optional ByVal NotFound As Boolean = False) As String
            Dim Out As New StringBuilder
            Out.Length = 0

            '
            Dim Element As String
            If String.IsNullOrEmpty(StrSpace) Then
                Element = Elm
            Else
                Element = StrSpace & ":" & Elm
            End If

            If NotFound Then
                Out.Append("<" & Element & "/>" & Separator)
            Else
                If String.IsNullOrEmpty(Element) Then 'Rootカレンダー用（タグを作らない）
                    Out.Append(Res & Separator)     'Separatorは vbCrLf
                Else
                    Out.Append("<" & Element & ">" & Separator)
                    Out.Append(Res & Separator)     'Separatorは vbCrLf
                    Out.Append("</" & Element & ">" & Separator)
                End If
            End If

            Return Out.ToString
        End Function


        ''' <summary>
        ''' CTAGを取得する
        ''' </summary>
        ''' <param name="OptionalPath">付与したパス（なくても可）</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' DBから最終更新日を検索し、ctagを作成する
        ''' 最終更新日がない場合はテーブルに現在の日時を登録し
        ''' その結果のctagを得る
        ''' </remarks>
        Private Function GetECtagFromTable(Optional ByVal OptionalPath As String = "") As String

            'ファイルからctagの日付を得る
            Dim CtagDate As Date = Nothing
            CtagDate = InHeaderInfo.GetCtagDate(False)

            Dim Path As String = InHeaderInfo.GetCtagPath & OptionalPath

            Dim FormatDate As String = Format(CtagDate, "yyyy/MM/dd HH:mm:ss")

            '両端に"ダブルクオートをつける
            Dim Answer As String = """" & CreateMd5(Path, FormatDate) & """"

            Return Answer

        End Function


        ''' <summary>
        ''' PROPFINDメソッド
        ''' </summary>
        ''' <remarks>
        ''' このメソッドが呼ばれるときは、認証が終了している
        ''' </remarks>
        Public Sub ResPropFind()
            Logger.Info("[IC3040404Response:resPropFind] Start  User:" & InHeaderInfo.GetUser)

            WriteBasicHeader(InResponse)

            'xml出力
            Dim Root As XmlElement = Nothing
            If IsNothing(InXml.DocumentElement) Then
                'Request Bodyがない場合エラーを返す
                InResponse.StatusCode = GlobalConst.HttpStat422 'Unprocessible Entity
            Else
                Root = InXml.DocumentElement
                '正常時　MultiStatusを返す
                'ヘッダに追加
#If Not Debug Then
                InResponse.Headers.Add("DAV", GlobalConst.HeadDav)
                InResponse.Headers.Add("ETag", MakeEtag())
#End If
                '空行を出力
                InResponse.Write("")

                'ヘッダは未出力
                Dim HeaderOutput As Boolean = False
                HeadPattern = 1  '標準ヘッダ

                For Each Node As XmlNode In Root

                    'エレメントの取得
                    Dim Key() As String

                    Dim OkData As New StringBuilder '200 OKデータ格納場所
                    Dim NgData As New StringBuilder '404 Not Findデータ格納場所

                    OkData.Length = 0
                    NgData.Length = 0
                    For Each Elm As XmlElement In Node
                        Key = Elm.Name.Split(":")               '名前空間を分離
                        Dim Tag As String = Key(1)              '調べるkeyword
                        Dim Stat As Integer                     '返答の種類 =0 Ok(200) , =1 NG(404)
                        Dim StrSpace As String = "" '名前空間
                        Dim Res As String = GetResStrPropfind(Stat, StrSpace, Tag) '返答する内容

                        'OKとNGで一時保存の場所を変える
                        If Stat = StatusOK Then    'OK Data
                            OkData.Append(MakeOneElement(Res, StrSpace, Tag))
                        Else                'Not Found Data
                            NgData.Append(MakeOneElement(Res, StrSpace, Tag, True))
                        End If
                    Next

                    '全体ヘッダ出力
                    '名前空間出力で３パターンある
                    If Not HeaderOutput Then '出力していない場合
                        If HeadPattern = 1 Then  '名前空間がCしかない場合
                            OutRespXml(GlobalConst.HttpHead1)
                            'レスポンスのヘッダ
                            OutRespXml(String.Format(CurrentCulture(), GlobalConst.HttpResHead, GetPathInfo(GlobalConst.CalendarPath)))

                        ElseIf HeadPattern = 2 Then  'long long response
                            OutRespXml(GlobalConst.HttpHead2)
                            'レスポンスのヘッダ
                            '中身は次のロジック（If headPatten = 2）で出力

                        Else
                            OutRespXml(GlobalConst.HttpHead3)
                            'レスポンスのヘッダ
                            OutRespXml(String.Format(CurrentCulture(), GlobalConst.HttpResHead, GetPathInfo(GlobalConst.CalendarPath)))

                        End If
                        HeaderOutput = True
                    End If

                    If HeadPattern = 2 Then
                        'パターン2 は特殊処理
                        ' データ読込みで、パス関係とCTAGはデータ挿入
                        OutRespXml(String.Format(CurrentCulture(), _
                                                 GlobalResText.PropData, _
                                                 GetPathInfo(GlobalConst.RootPath), _
                                                 GetPathInfo(GlobalConst.HomePath), _
                                                 GetPathInfo(GlobalConst.CalendarPath), _
                                                 GetPathInfo(GlobalConst.DisplayName), _
                                                 GetPathInfo(GlobalConst.CalendarRelPath), _
                                                 GetECtagFromTable))

                    Else '生成するデータがある場合のみ

                        'レスポンスはmulti-status
                        If OkData.Length > 0 Or NgData.Length > 0 Then
                            '200 OK の部分
                            If OkData.Length > 0 Then
                                OutRespXml(GlobalConst.HttpResBodyHead)
                                OutRespXml(OkData.ToString)
                                OutRespXml(GlobalConst.HttpResFootProp200)
                            End If

                            '404 Not Foundの部分
                            If NgData.Length > 0 Then
                                OutRespXml(GlobalConst.HttpResBodyHead)
                                OutRespXml(NgData.ToString)
                                OutRespXml(GlobalConst.HttpResFootProp404)
                            End If
                        End If

                    End If

                    'オブジェクトの廃棄
                    OkData = Nothing
                    NgData = Nothing

                Next

                'フッタ出力
                OutRespXml(GlobalConst.HttpResFoot)
                InResponse.StatusCode = GlobalConst.HttpStat207 'MultiStatus

            End If

            Logger.Info(" [IC3040404Response:resPropFind] Exit")
        End Sub


        ''' <summary>
        ''' Hashのvalの値をインデクス（パターン番号）と内容に分離
        ''' </summary>
        ''' <param name="StrSpace">分離データ 名前空間</param>
        ''' <param name="Contents">分離データ　内容</param>
        ''' <param name="Values">val入力文字列</param>
        ''' <param name="Delimit">デリミタ（Optional)既定":"</param>
        ''' <returns>インデクス（パターン番号）</returns>
        ''' <remarks>
        ''' 入力値val が"12345:C:memo" の場合
        ''' 返り値は　12345(Integer)
        ''' contentsは "memo"　となる
        ''' valは内容がなくてもよい。その場合、contentsは""を返す
        ''' デリミタ（セパレータ）の既定値は":"でoptionalで変更可
        ''' 最初の1項目だけ分離するので、VBのsplitは使えない
        ''' 2011/11/30 名前空間を加味するように変更
        ''' </remarks>
        Private Shared Function SeparateTag(ByRef StrSpace As String, ByRef Contents As String, ByVal Values As String, _
                              Optional ByVal Delimit As String = ":") As Integer

            Dim Found As Integer = Values.IndexOf(Delimit, StringComparison.CurrentCulture)
            Dim Index As Integer = 0
            StrSpace = ""
            Contents = ""

            Try
                If Found >= 1 Then 'セパレータが見つかった場合
                    Index = CInt(Microsoft.VisualBasic.Left(Values, Found))
                    Contents = Values.Substring(Found + 1)

                    '次の分解
                    Found = Contents.IndexOf(Delimit, StringComparison.CurrentCulture)
                    If Found > 0 Then
                        StrSpace = Microsoft.VisualBasic.Left(Contents, Found)
                        Contents = Contents.Substring(Found + 1)
                        'Else sSpace=""のまま
                    Else '見つからない場合と"："以降がない場合
                        If Contents.Length > 1 Then
                            Contents = Contents.Substring(Found + 1)
                        Else
                            Contents = ""
                        End If
                    End If
                Else
                    Index = CInt(Values)
                    Contents = "" 'なし
                End If
            Catch ex As ApplicationException
                Logger.Error("[IC3040404Response:SeparateTag] Not Found Item-Index:" & Values)
            End Try

            Return Index
        End Function


        ''' <summary>
        ''' REPORT用　レスポンス処理
        ''' </summary>
        ''' <param name="status"></param>
        ''' <param name="StrSpace"></param>
        ''' <param name="key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function GetResStrReport(ByRef Status As Integer, ByRef StrSpace As String, ByVal Key As String) As String

            Dim Val As String = ""
            StrSpace = ""
            Status = StatusNG

            Try
                'パターンが少ないのハードコーディング
                Status = StatusOK 'OK
                If Key.IndexOf("getetag", StringComparison.CurrentCulture) >= 0 Then
                    Val = "getetag"
                    'ElseIf Key.IndexOf("comp-filter", StringComparison.CurrentCulture) > 0 Then
                    '    Val = "comp-filter"
                    'ElseIf Key.IndexOf("calendar-data", StringComparison.CurrentCulture) > 0 Then
                    '    Val = "calendar-data"
                Else
                    'key.IndexOf("getcontenttype", GlobalConst.Culture)
                    'key.IndexOf("schedule-tag", GlobalConst.Culture) > 0 Then
                    Status = StatusNG  'Not Found
                    Val = ""
                End If

            Catch ex As ApplicationException
                Logger.Error("[IC3040404Response:GetResStrReport] Not convert key:" & Key)
            End Try

            Return Val

        End Function

        ''' <summary>
        ''' アットマークを　%40にエスケープする
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>
        ''' icropではユーザー名に@が入るので、対応
        ''' @ を %40 で置き換える
        ''' 例　200005@42A10 → 200005%4042A10
        ''' （注）atmarkはjapaneseEnglishです
        ''' </remarks>
        Private Shared Function EscapeAtmark(ByVal InpStr As String) As String

            Dim Str As String = InpStr.Replace("@", "%40")

            Return Str

        End Function


        ''' <summary>
        ''' iPadに返すパス情報を返す
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetPathInfo(Optional ByVal PathKind As Integer = GlobalConst.RootPath) As String
            Dim Str As String = ""

            '2011/12/12 変更
            Dim RootPath As String = InGlobal.CalDavRootPath

            Select Case PathKind
                Case GlobalConst.RootPath      '0:root path      /DAV/CalDAV/IC3040404.aspx/
                    Str = RootPath

                Case GlobalConst.HomePath      '1:user path      /DAV/CalDAV/IC3040404.aspx/200005%4042A10/
                    Str = RootPath & EscapeAtmark(InHeaderInfo.GetUser()) & "/"

                Case GlobalConst.CalendarPath  '2:celendar path  /DAV/CalDAV/IC3040404.aspx/200005%4042A10/calendar/
                    Str = RootPath & EscapeAtmark(InHeaderInfo.GetUser()) & "/calendar/"

                Case GlobalConst.DisplayName   'カレンダーの表示名
                    Str = InGlobal.DisplayName

                Case GlobalConst.CalendarRelPath  'ユーザーNon-escape /200005@42A10/calendar/
                    Str = "/" & InHeaderInfo.GetUser() & "/calendar/"

            End Select

            Return Str
        End Function

        ''' <summary>
        ''' Propfindのパターンごとに返答処理
        ''' </summary>
        ''' <param name="Status"></param>
        ''' <param name="StrSpace">名前空間</param>
        ''' <param name="Key"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <History>
        ''' 2012/10/4 SKFC 浦野【ios6対応】未知のタグに対応
        ''' </History>
        Private Function GetResStrPropfind(ByRef Status As Integer, ByRef StrSpace As String, ByRef Key As String) As String
            Dim Value As String = ""

            Try
                ' 2012/10/4 SKFC 浦野【iOS6対応】未知のタグに対応 START
                'Value = XmlData.ResTable(Key)
                Value = XmlData.GetResTable(Key)
                ' 2012/10/4 SKFC 浦野【iOS6対応】未知のタグに対応 END

                'add-memberがあるときはパターン2
                If HeadPattern = 1 Then
                    If Key.IndexOf("add-member", StringComparison.CurrentCulture) >= 0 Then
                        HeadPattern = 2
                    ElseIf Key.IndexOf("calendar-home-set", StringComparison.CurrentCulture) >= 0 Then
                        HeadPattern = 3
                    End If
                End If

                'ヘッダのパターンを判定

                'セパレータ":"でパターン番号と名前空間と内容に分離
                Dim Index As Integer = 0   'パターン番号
                Dim Tag As String = ""     '内容
                StrSpace = ""

                Index = SeparateTag(StrSpace, Tag, Value)

                Status = StatusOK 'OK

                Select Case Index
                    Case 2000 'そのまま定型文を返す
                        Value = Tag

                        'Case 2002 '{0}メール　{1}パス　廃止
                        '    Const mailto As String = "mailto:"
                        'val = String.Format(sTag, mailto & strMail, GetPathInfo(GlobalConst.PATH_HOME))

                    Case 2003 'カレンダー名
                        Value = String.Format(CurrentCulture(), Tag, GetPathInfo(GlobalConst.DisplayName))

                    Case 2004 'getctag
                        Value = String.Format(CurrentCulture(), Tag, GetECtagFromTable())

                    Case 2005 'resource type 名前空間
                        Value = String.Format(CurrentCulture(), Tag, "C:")

                    Case 2010 'Root path add-member
                        'この処理は特殊なため別のDictionaryから引く
                        Value = XmlData.ResTableRoot("propstat")
                        Index = SeparateTag(StrSpace, Tag, Value)
                        Value = String.Format(CurrentCulture(), Tag, GetPathInfo(GlobalConst.RootPath), _
                                            GetPathInfo(GlobalConst.HomePath), _
                                            GetPathInfo(GlobalConst.CalendarPath), _
                                            GetPathInfo(GlobalConst.DisplayName), _
                                            GetPathInfo(GlobalConst.CalendarRelPath))
                        'キーをなしにする
                        Key = ""
                    Case 2100 'Root path
                        Value = String.Format(CurrentCulture(), Tag, GetPathInfo(GlobalConst.RootPath))

                    Case 2101 'Home path
                        Value = String.Format(CurrentCulture(), Tag, GetPathInfo(GlobalConst.HomePath))

                    Case 2102 'Calendar path
                        Value = String.Format(CurrentCulture(), Tag, GetPathInfo(GlobalConst.CalendarPath))

                    Case 2103 'Display name
                        Value = String.Format(CurrentCulture(), Tag, GetPathInfo(GlobalConst.DisplayName))

                    Case 2104 '相対Calendar path(non-escape)
                        Value = String.Format(CurrentCulture(), Tag, GetPathInfo(GlobalConst.CalendarRelPath))

                    Case Else '404
                        Status = StatusNG  'Not Found
                        Value = ""
                End Select

            Catch ex As ApplicationException
                Logger.Error("[IC3040404Response:GetResStrPropfind] Not convert key:" & Key)

            End Try

            Return Value

        End Function


        ''' <summary>
        ''' Etagを作成
        ''' </summary>
        ''' <param name="kind"></param>
        ''' <returns></returns>
        ''' <remarks>
        '''デフォルトはカレンダー用のEtag
        ''' 引数kindを指定すると、そのTagになる
        ''' 作成されるTagは　/200005@42A10/calendar/ と　日付から
        ''' 生成するMD5データとなる
        ''' </remarks>
        Private Function MakeEtag(Optional ByVal Kind As String = "calendar/") As String

            '作成方法はctagと同じ
            Dim Md5 As String = GetECtagFromTable(Kind)

            Return Md5
        End Function


        ''' <summary>
        ''' Bodyを出力
        ''' </summary>
        ''' <param name="Str"></param>
        ''' <remarks>
        ''' string builderで作成した文字列をそのまま
        ''' 出力すると改行が消えるのでこの関数を実装
        ''' </remarks>
        Private Sub OutRespXml(ByVal Str As String)

            Dim Works() As String = Str.Split(Separator)

            For Each Work As String In Works
                WriteExec(Work, "")
            Next
            'Logger.Info("RESPONSE:" & Str, True)

        End Sub


        ''' <summary>
        ''' レスポンスのボディを返す
        ''' </summary>
        ''' <param name="Str"></param>
        ''' <param name="LineFeed">既定値改行=vbLf オプション=""で無改行</param>
        ''' <remarks></remarks>
        Private Sub WriteExec(ByVal Str As String, Optional ByVal LineFeed As String = vbLf)

            ' 2012/10/23 SKFC 浦野【iOS6対応】ログ改修 START
            '以下の1行のコメントを除去すると詳細なレスポンスがログに出ます
            'Logger.Debug("RES_BODY:" & Str.Replace(vbCr, "").Replace(vbLf, "")) 'crlfを除去
            ' 2012/10/23 SKFC 浦野【iOS6対応】ログ改修 END

            InResponse.Write(Str & LineFeed)

        End Sub


        ''' <summary>
        ''' GETメソッド
        ''' </summary>
        ''' <remarks>
        ''' このメソッドが呼ばれるときは、認証が終了している
        ''' </remarks>
        Public Sub ResGet()
            Const ForGet As Boolean = True 'Get用

            Logger.Info("[IC3040404Response:resGet] Start  User:" & InHeaderInfo.GetUser)

            WriteBasicHeader(InResponse)

#If Not Debug Then
            '_Response.ContentType = "application/octet-stream"
            '_Response.Headers.Add("ETag", MakeEtag())
#End If

            Dim UidList As New StringBuilder
            UidList.Length = 0

            'ByRefを削除
            UidList.Append(GetVeventList())
            Dim Rec As Integer = UidCount

            'vbCrLfで分離
            Dim UniqueId() As String = UidList.ToString.Split(vbCrLf)
            Dim Dat As DateTime
            Dim EventData As New StringBuilder

            EventData.Length = 0
            'ヘッダを書く
            MakeVcalendarhead(EventData, ForGet)

            If Rec >= 0 Then
                For i As Integer = 0 To Rec
                    '
                    UniqueId(i) = UniqueId(i).Trim 'スペース等を除去
                    Dat = GlobalConst.DateNothingValue    'NULL DATEに変更　2011/12/15
                    MakeVevent(EventData, Dat, UniqueId(i)) '参照データはリカレンスなし
                    Logger.Info("        GET Method  uid(" & i.ToString(CurrentCulture()) & ")=" & UniqueId(i), True)
                Next
            Else
                Logger.Info("        GET Method  No Data", True)
            End If

            EventData.Append("END:VCALENDAR" & vbCrLf)

            '空行を出力
            InResponse.Write("")

            '作成したDataの出力
            OutRespXml(EventData.ToString)

            InResponse.StatusCode = GlobalConst.HttpStat200 'OK

            '出力件数をログ出力
            Logger.Info(" [IC3040404Response:resGet] Exit  RECORD:" & (Rec + 1).ToString(CurrentCulture()))

        End Sub

        ' 2012/10/11 SKFC 浦野【iOS6対応】時差対応 START
        ''' <summary>
        ''' GMT+XX:YYをDouble型に変換
        ''' </summary>
        ''' <param name="gmt"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 1時間が1.0 で 30分が0.5(符号付)
        ''' サポートタイプ
        ''' 　GMT7,GMT07,GMT+7,GMT+07
        '''   GMT7:0,GMT07:00,GMT+7:00,GMT+07:00
        '''   GMT-7,GMT-7:00,GMT-07:00
        '''   GMTはgmtやUTCやutcでもよい
        '''  GMTやUTCがない場合で hhmm +hhmm -hhmmもサポート
        ''' </remarks>
        Function GmtToDouble(ByVal gmt As String) As Double
            Dim g As String = gmt
            Dim dbl As Double = 0
            Dim minute As Double = 0
            Dim hour As Double = 0
            gmt = gmt.ToUpper()
            Try
                If 0 <= gmt.IndexOf("GMT") Or 0 <= gmt.IndexOf("UTC") Then
                    gmt = gmt.Replace("GMT", "").Replace("UTC", "")
                    Dim n As Integer = gmt.IndexOf(":")
                    If 0 <= n Then
                        ':分がある
                        hour = CDbl(gmt.Substring(0, n))
                        minute = CDbl(gmt.Substring(n + 1)) / 60.0#
                        If hour < 0 Then
                            dbl = hour - minute
                        Else
                            dbl = hour + minute
                        End If
                    Else
                        dbl = CDbl(gmt)
                    End If
                Else
                    Dim len As Integer = gmt.Length
                    If len = 4 Then 'HHMM 正時間
                        hour = CDbl(gmt.Substring(0, 2))
                        minute = CDbl(gmt.Substring(2, 2)) / 60
                        dbl = hour + minute
                    ElseIf len = 5 Then 'sHHMM sは符号
                        Dim sign As String = gmt.Substring(0, 1)
                        hour = CDbl(gmt.Substring(1, 2))
                        minute = CDbl(gmt.Substring(3, 2)) / 60
                        If String.Equals("-", sign) Then
                            dbl = -hour - minute
                        ElseIf String.Equals("+", sign) Then
                            dbl = hour + minute
                        End If
                    End If
                End If
            Catch

            End Try

            'Debug.Print(g & "=" & dbl.ToString)
            Return dbl

        End Function
        ' 2012/10/11 SKFC 浦野【iOS6対応】時差対応 END

        ''' <summary>
        ''' PUTメソッド
        ''' </summary>
        ''' <remarks>
        ''' PUTはヘッダで判別する　 If-None-Match を含む場合　新規（Insert）
        '''                   　　　If-Matchを含む場合　　　　更新（update）
        ''' </remarks>
        ''' <History>
        ''' 2012/10/12 SKFC 浦野【ios6対応】時差対応
        ''' </History>
        Public Sub ResPut()
            Const MaxAlarmCount As Integer = 2    'アラームは2件まで登録
            Const MaxAttendeeCount As Integer = 2 'ATENNDEEは2件まで
            Const AllDay As String = "1"
            Logger.Info("[IC3040404Response:ResPut] Start  User:" & InHeaderInfo.GetUser)
            Logger.Info("[IC3040404Response:ResPut](PUT):" & InVevent, True) 'ログ（再出力）
            WriteBasicHeader(InResponse)             '基本ヘッダを出力
            Dim StrBody() As String = InVevent.Split(vbCrLf)
            Dim IsStart As Boolean = False           'VEVENT開始
            Dim Valarm As Boolean = False           'VEVENT中のみ確認
            Dim Vcal As New VeventData              '時差　Insertで東京の場合 -1、その他は1
            Dim Attendee As New StringBuilder       '(機能凍結）予定出席者 2件まで登録 Max 128Byte/件
            Attendee.Length = 0
            Dim AttendRec As Integer = 0

            ' 2012/10/12 SKFC 浦野【iOS6対応】時差対応 START
            Dim ServerTimeLag As Double = GmtToDouble(InGlobal.DefaultLocalTime)
            Dim iPadTimeLag As Double = 0
            Dim bIsStandard As Boolean = False
            ' 2012/10/12 SKFC 浦野【iOS6対応】時差対応 END

            For Each Item As String In StrBody
                Dim ItemStr As String = Item.Trim               '文字列をTrimする
                Dim SplitStr() As String = ItemStr.Split(":")   '文字列を分解
                Dim TagName As String = SplitStr(0).Trim        'Tag名
                Dim Purpose As String = ""                      'データ
                If SplitStr.GetUpperBound(0) > 0 Then Purpose = SplitStr(1).Trim
                If String.Equals(TagName, "END") And String.Equals(Purpose, "VCALENDAR") Then Exit For '終わり？

                ' 2012/10/12 SKFC 浦野【iOS6対応】時差対応 START
                If String.Equals(ItemStr, "BEGIN:STANDARD") Then
                    bIsStandard = True
                End If
                If String.Equals(ItemStr, "END:STANDARD") Then
                    bIsStandard = False
                End If

                'STANDARD中にTZOFFSETTOEが来たらiPadのTimeLagと判定 【サマータイム未対応】
                If bIsStandard AndAlso 0 <= TagName.IndexOf("TZOFFSETTO") Then
                    If UBound(SplitStr) > 0 Then
                        iPadTimeLag = GmtToDouble(SplitStr(1))    'GMT...以降を抜く    
                    End If
                End If
                ' 2012/10/12 SKFC 浦野【iOS6対応】時差対応 END

                If IsStart Then
                    If String.Equals(TagName, "END") And String.Equals(Purpose, "VEVENT") Then
                        'If-Match（新規でも既存のある場合はUpdateにする）ポカよけ
                        Dim insertFlag As Boolean = InHeaderInfo.IsInsert
                        If AlreadyUidData(Vcal.UniqueId) Then
                            insertFlag = False
                        End If
                        RequestExecuter(Vcal, insertFlag) 'DB書込み処理
                        IsStart = False
                    Else 'VALARM中を判定
                        If String.Equals(TagName, "BEGIN") And String.Equals(Purpose, "VALARM") Then Valarm = True
                        If String.Equals(TagName, "END") And String.Equals(Purpose, "VALARM") Then Valarm = False
                        Dim Detail() As String = TagName.Split(";") 'tagNameはさらにセミコロンを含む場合あり
                        Dim tag = Detail(0)
                        Select Case tag
                            Case "SUMMARY"
                                Purpose = Mid(ItemStr, 9) '9文字目から拾いなおす（:対策）
                                Vcal.Title = UnEscStr(Purpose)
                            Case "LOCATION"
                                Purpose = Mid(ItemStr, 10) '10文字目から拾いなおす（:対策）
                                Vcal.Place = UnEscStr(Purpose)
                            Case "DTSTART"
                                'If TagName.IndexOf(InGlobal.TokyoLocal, StringComparison.CurrentCulture) >= 0 Then
                                '    Vcal.TimeLag = -1   '時差1時間
                                'End If
                                ' 2012/10/11 SKFC 浦野【iOS6対応】時差対応 START
                                Vcal.TimeLag = ServerTimeLag - iPadTimeLag
                                ' 2012/10/11 SKFC 浦野【iOS6対応】時差対応 END
                                If ItemStr.IndexOf("VALUE=DATE", StringComparison.CurrentCulture) >= 0 Then
                                    Vcal.AllDay = AllDay '終日
                                End If
                                Vcal.StartTime = Purpose
                            Case "DTEND"
                                Vcal.EndTime = Purpose
                            Case "URL"
                                Vcal.URInfo = Purpose
                            Case "UID"
                                ' 2012/10/24 SKFC 浦野【iOS6対応】iOS6ではVALARMにもUIDがあるので対応 START
                                If Not Valarm Then
                                    'Valam中を拾わない
                                    Vcal.UniqueId = Purpose
                                End If
                                ' 2012/10/24 SKFC 浦野【iOS6対応】iOS6ではVALARMにもUIDがあるので対応 START
                            Case "DESCRIPTION" 'Valam中を拾わない
                                If Not Valarm Then
                                    Purpose = Mid(ItemStr, 13) '13文字目から拾いなおす（:対策）
                                    Vcal.Memo = UnEscStr(ReduceColor(Purpose))
                                End If
                            Case "EXDATE"   '除外日 2011/12/17実装
                                If Vcal.ExDateCount > 0 Then Vcal.ExDate.Append(vbCrLf) '2件め以降はセパレータ
                                Vcal.ExDate.Append(Purpose)
                                Vcal.ExDateCount += 1
                            Case "TRIGGER" '通知は2件のみ受付
                                If Vcal.NotifyCount < MaxAlarmCount Then ' 0 または 1 のとき
                                    Dim count As Integer = Vcal.NotifyCount
                                    Dim vNotify As String
                                    If String.Equals(Vcal.AllDay, "1") Then
                                        vNotify = PutNotifyTime(Purpose, True)   '終日
                                    Else
                                        vNotify = PutNotifyTime(Purpose)
                                    End If
                                    '終日の場合コード表にない時間がくる。この場合、vNotifyには"0"を入れる
                                    If Not String.Equals(vNotify, "0") Then '時間→終日の対応
                                        Vcal.Notify(count) = vNotify
                                        Vcal.NotifyCount = count + 1
                                    End If
                                End If
                            Case "RRULE"
                                Vcal.Rrule = Purpose
                            Case "ATTENDEE" '2件のみ保存 2011/12/17 未サポートに変更
                                If AttendRec < MaxAttendeeCount Then
                                    If AttendRec >= 1 Then Attendee.Append("#") '#がセパレータ
                                    Dim append As String = Mid(ItemStr, 10).Replace(vbCrLf & " ", "") '10文字目から保存
                                    Attendee.Append(append)
                                    AttendRec += 1
                                    Vcal.Attendee = Attendee.ToString 'パラメータ変数に代入
                                End If
                            Case "RECURRENCE-ID"
                                Vcal.Recur = Purpose 'リカレンスで新設したデータ
                        End Select
                    End If
                Else
                    If String.Equals(TagName, "BEGIN") And String.Equals(Purpose, "VEVENT") Then
                        Vcal.Refresh() '構造体を初期化
                        IsStart = True
                    End If
                End If
            Next

            If InHeaderInfo.IsInsert Then
                InResponse.StatusCode = GlobalConst.HttpStat201 '新規 Created
            Else
                InResponse.StatusCode = GlobalConst.HttpStat204 '更新 No Content
            End If

            ' 2012/10/23 SKFC 浦野【iOS6対応】ログ改修 START
            Logger.Info(" [IC3040404Response:resPut] Exit ServerTime:" & ServerTimeLag.ToString & " iPadTime:" & iPadTimeLag.ToString)
            ' 2012/10/23 SKFC 浦野【iOS6対応】ログ改修 END
        End Sub


        ''' <summary>
        ''' Uidのデータがある場合はTrue
        ''' </summary>
        ''' <param name="UniqueId"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' if-None-Match（新規）でも操作タイミングによっては既存データ
        ''' の場合があるので対応
        ''' </remarks>
        Private Shared Function AlreadyUidData(ByVal UniqueId As String) As Boolean
            Dim Answer As Boolean = False

            Using ModifyInfo As New DataAccess.IC3040404.IC3040404.Api.DataAccess.IC3040404DataTable
                Dim Ret As IC3040404DataSet.TableDataTableDataTable = _
                            ModifyInfo.SelectEventItem(UniqueId, " ")

                If Not IsNothing(Ret) Then
                    If Ret.Rows.Count > 0 Then
                        Answer = True
                    End If
                End If
            End Using

            Return Answer
        End Function


        ''' <summary>
        ''' 対象文字をエスケープする
        ''' </summary>
        ''' <param name="str"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 対象は , ; \
        ''' </remarks>
        Private Shared Function EscStr(ByVal Str As String) As String
            Dim Out As String = Str

            'エスケープを付与 \(Backスラッシュ）いったん全角の￥にする
            Out = Str.Replace("\\", "￥").Replace("\", "\\").Replace(",", "\,").Replace(";", "\;")
            '￥を半角の\にもどす \と同じ
            Out = Out.Replace("￥", "\\")

            '\nを設定
            Out = Out.Replace(vbCrLf, "\n")
            Out = Out.Replace(vbLf, "\n")   'LFの場合もある

            Return Out
        End Function

        ''' <summary>
        ''' エスケープをもとに戻す
        ''' </summary>
        ''' <param name="str"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 対象は , ; \
        ''' </remarks>
        Private Shared Function UnEscStr(ByVal Str As String) As String
            Dim Out As String = Str

            'エスケープ文字を戻す
            Out = Str.Replace("\n", vbCrLf) '改行をvbCrLfにする

            ',はエスケープしすぎる場合があるので考慮
            Out = Out.Replace("\\,", "\,")

            Out = Out.Replace("\\", "\").Replace("\,", ",").Replace("\;", ";").Replace("\ ", "\")

            Return Out
        End Function


        ''' <summary>
        ''' MEMOの文字列中のcolorタグを除去 
        ''' </summary>
        ''' <param name="str"></param>
        ''' 
        ''' <returns></returns>
        ''' <remarks>
        ''' \nもvbCrlfに置換
        ''' </remarks>
        Private Shared Function ReduceColor(ByVal Str As String, Optional ByVal CutStr As String = "color=""") As String

            Dim Out As New StringBuilder
            Out.Length = 0

            If Not String.IsNullOrEmpty(Str) Then
                'メモに文字列がある場合のみ処理
                Dim length As Integer = CutStr.Length + 1
                '削除する文字列
                Dim Start As Integer = Str.IndexOf(CutStr, StringComparison.CurrentCulture)

                If Start >= 0 Then
                    Dim Part As String = ""
                    If Start > 0 Then
                        If String.Equals(Mid(Str, Start - 1, 2), vbCrLf) _
                        Or String.Equals(Mid(Str, Start - 1, 2), "\n") Then
                            '改行があるとき
                            Out.Append(Microsoft.VisualBasic.Left(Str, Start - 2))
                            Part = Mid(Str, Start + length + 1)
                        Else
                            Out.Append(Microsoft.VisualBasic.Left(Str, Start))
                            Part = Mid(Str, Start + length)
                        End If

                    End If
                    Dim Terminate As Integer = Part.IndexOf("""", StringComparison.CurrentCulture) '終わりのダブルクオート 
                    If Terminate >= 0 Then
                        Out.Append(Mid(Part, Terminate + 2))
                    End If
                Else
                    '文字列に含まないときは何もしない
                    Out.Append(Str)
                End If

            End If

            Dim OutData As String

            '\nをvbCrlfに変換
            If Out.Length > 0 Then
                OutData = Out.ToString.Replace("\n", vbCrLf)
            Else
                OutData = Out.ToString
            End If
            'オブジェクトを廃棄
            Out = Nothing

            Return OutData
        End Function

        ''' <summary>
        ''' カレンダーデータの登録
        ''' </summary>
        ''' <param name="vCal">登録するVEVENT情報</param>
        ''' <param name="insertFlag">trueのとき insert (新規データ）
        ''' false（更新データ）
        ''' </param>
        ''' <remarks>
        ''' PUTは新規データと更新と両方の情報がくる
        ''' </remarks>
        ''' <History>
        ''' 2012/10/12 SKFC 浦野【ios6対応】時差対応
        ''' </History>
        Private Sub RequestExecuter(ByVal Vcal As VeventData, ByVal InsertFlag As Boolean)
            ExecutePutLog(Vcal) 'ログ出力
            Dim EventId As String = ""
            Dim StrUser As String = InHeaderInfo.GetUser()
            'Insert or Update
            Using ModifyInfo As New DataAccess.IC3040404.IC3040404.Api.DataAccess.IC3040404DataTable
                Dim InData As New DataAccess.IC3040404.IC3040404.Api.DataAccess.CalEventItem
                Dim NowDate As DateTime = Now '現在時刻
                With InData
                    .CalId = "NATIVE"
                    .UniqueId = Vcal.UniqueId
                    .RecurrenceId = " " 'NULLにできないので一文字スペース
                    .AlldayFlg = Vcal.AllDay '変更 2011/12/12
                    If String.Equals(Vcal.AllDay, "1") Then '終日
                        .TimeFlg = "0"  '時間指定なし
                    Else
                        .TimeFlg = "1"  '時間指定あり
                    End If

                    ' 2012/10/12 SKFC 浦野【iOS6対応】時差対応 START
                    '東京の場合は1時間減ず（ChinaTimeベース）⇒変更
                    'TimeLagで補正
                    '.StartTime = DateAdd(DateInterval.Hour, Vcal.TimeLag, ToDateTime(Vcal.StartTime))
                    '.EndTime = DateAdd(DateInterval.Hour, Vcal.TimeLag, ToDateTime(Vcal.EndTime))
                    .StartTime = ToDateTime(Vcal.StartTime).AddHours(Vcal.TimeLag)
                    .EndTime = ToDateTime(Vcal.EndTime).AddHours(Vcal.TimeLag)

                    ' 2012/10/12 SKFC 浦野【iOS6対応】時差対応 END

                    '終日の場合は終了時間を1日前にする 2011/12/26 Update
                    If String.Equals(.AlldayFlg, "1") Then
                        .EndTime = DateAdd(DateInterval.Day, -1.0, .EndTime)
                    End If
                    If String.IsNullOrEmpty(Vcal.Rrule) Then
                        .RruleFlg = "0"
                        .RruleFreq = "NONE"
                        .RruleUntil = GlobalConst.DateNothingValue
                        .RruleText = ""
                    Else
                        Dim freq As String = ""
                        Dim interval As String = Nothing  '通常は1、2週間ごとのときのみ2
                        Dim until As DateTime = #12:00:00 AM#
                        'RRULEを解析
                        .RruleText = AnalyzeRrule(freq, interval, until, Vcal.Rrule)
                        .RruleFlg = "1"
                        .RruleFreq = freq
                        .RruleInterval = interval
                        .RruleUntil = until
                    End If
                    .DelFlg = "0"
                    .DelDate = GlobalConst.DateNothingValue
                    .Url = Vcal.URInfo
                    .Memo = Vcal.Memo
                    .Location = Vcal.Place
                    .Summary = Vcal.Title
                    .ActStaffCD = StrUser 'nativeの場合はACTとRECの両方にスタッフコードを設定する
                    .RecStaffCD = StrUser 'nativeの場合はACTとRECの両方にスタッフコードを設定する
                    .CreateDate = NowDate
                    .UpdateDate = NowDate
                    .CreateAccount = StrUser
                    .UpdateAccount = StrUser
                    .CreateId = GlobalConst.CalDavProgramId
                    .UpdateId = GlobalConst.CalDavProgramId
                    .Attendee = Vcal.Attendee
                    .RecurrenceId = Vcal.Recur
                End With
                If InsertFlag Then  'Insert
                    ModifyInfo.InsertEventItem(InData)
                Else                'Update　　本体のときにリカレンスを削除
                    If String.IsNullOrEmpty(Vcal.Recur.Trim) Then
                        With InData  '削除フラグをセット
                            .DelFlg = "1"
                            .DelDate = Now
                        End With
                        ModifyInfo.DeleteEventItem(InData, True)
                        With InData '削除フラグを戻す 
                            .DelFlg = "0"
                            .DelDate = GlobalConst.DateNothingValue
                        End With
                    End If
                    'リカレンスのある場合は新規を考慮
                    If Not String.IsNullOrEmpty(Vcal.Recur.Trim) Then 'スペース" "も除外
                        ModifyInfo.InsertEventItem(InData) 'リカレンスは新規
                    Else
                        ModifyInfo.UpdateEventItem(InData) 'リカレンスは更新
                    End If
                End If
                'イベントIDを知る
                Dim ret As IC3040404DataSet.TableDataTableDataTable = _
                    ModifyInfo.SelectEventItem(Vcal.UniqueId, Vcal.Recur)
                EventId = ret.Rows(0).Item("EVENTID")
            End Using
            If Vcal.ExDateCount >= 0 Then '除外日がある場合　Exdateレコードを作成
                InsertExDate(Vcal, StrUser, EventId, InsertFlag)
            End If
            If Vcal.NotifyCount >= 0 Then '通知がある場合 EventAlarmを作成
                InsertAlarmData(Vcal, StrUser, EventId, InsertFlag)
            End If
            'Dim cTagDate As Date = Now 'ctagの日付を更新()
            'cTagDate = InHeaderInfo.GetCtagDate(True) '引数をTrueでctag情報を更新
        End Sub

        ''' <summary>
        ''' PUTのログ
        ''' </summary>
        ''' <param name="vCal"></param>
        ''' <remarks></remarks>
        Private Sub ExecutePutLog(ByVal Vcal As VeventData)
            With Vcal
                Logger.Debug("[IC3040404Response:executePutLog PutDump] Start User:" & InHeaderInfo.GetUser)
                Logger.Debug("Title:" & Vcal.Title)
                Logger.Debug("Place:" & Vcal.Place)
                Logger.Debug("Start:" & Vcal.StartTime)
                Logger.Debug("End:" & Vcal.EndTime)
                Logger.Debug("Repeat:" & Vcal.Rrule)
                Logger.Debug("TimeLag:" & Vcal.TimeLag.ToString(CurrentCulture()))

                For i As Integer = 0 To Vcal.NotifyCount - 1
                    Logger.Debug("Notify:" & (i + 1).ToString(CurrentCulture()) & Vcal.Notify(i))
                Next

                Logger.Debug("URL:" & Vcal.URInfo)
                Logger.Debug("Memo:" & Vcal.Memo)
                Logger.Debug("UID:" & Vcal.UniqueId)
                Logger.Debug(" [IC3040404Response:executePutLog PutDump] End")
            End With

        End Sub

        ''' <summary>
        ''' 除外日をDBに登録
        ''' </summary>
        ''' <param name="vcal"></param>
        ''' <param name="strUser"></param>
        ''' <param name="eventid"></param>
        ''' <param name="insertFlag"></param>
        ''' <remarks></remarks>
        Private Shared Sub InsertExDate(ByVal Vcal As VeventData, ByVal StrUser As String, ByVal EventId As String, ByVal InsertFlag As Boolean)
            Using ExDateInfo As New DataAccess.IC3040404.IC3040404.Api.DataAccess.IC3040404DataTable

                Dim InpExcept As New DataAccess.IC3040404.IC3040404.Api.DataAccess.CalEventExDate
                Dim ExceptDate As DateTime = Now '現在時刻

                With InpExcept
                    .EventId = EventId
                    .CreateDate = ExceptDate
                    .UpdateDate = ExceptDate
                    .CreateAccount = StrUser
                    .UpdateAccount = StrUser
                    .CreateId = GlobalConst.CalDavProgramId
                    .UpdateId = GlobalConst.CalDavProgramId
                    If Not InsertFlag Then
                        '更新時はイベント全アラームデータを物理削除する
                        ExDateInfo.DeleteEventExDate(InpExcept)
                    End If

                    Dim ExDate() As String = Vcal.ExDate.ToString.Split(vbCrLf)
                    For i As Integer = 0 To Vcal.ExDateCount - 1
                        Logger.Debug("ExDate:" & (i + 1).ToString(CurrentCulture()) & ExDate(i))
                        .SeqNo = i + 1    '1オリジンなので+1
                        .ExDate = DateAdd(DateInterval.Hour, Vcal.TimeLag, ToDateTime(ExDate(i)))

                        'データInsert
                        ExDateInfo.InsertEventExDate(InpExcept)

                    Next

                End With
            End Using

        End Sub

        ''' <summary>
        ''' アラーム(通知)をDBに登録
        ''' </summary>
        ''' <param name="vcal">カレンダ情報</param>
        ''' <param name="strUser">ログインユーザー</param>
        ''' <param name="eventid">EventID</param>
        ''' <param name="insertFlag">Trueのとき挿入</param>
        ''' <remarks></remarks>
        Private Shared Sub InsertAlarmData(ByVal Vcal As VeventData, ByVal StrUser As String, ByVal EventId As String, ByVal InsertFlag As Boolean)
            Using alarmInfo As New DataAccess.IC3040404.IC3040404.Api.DataAccess.IC3040404DataTable

                Dim IndatAlarm As New DataAccess.IC3040404.IC3040404.Api.DataAccess.CalEventAlarm
                Dim AlarmDate As DateTime = Now '現在時刻

                With IndatAlarm
                    .EventId = EventId
                    .CreateDate = AlarmDate
                    .UpdateDate = AlarmDate
                    .CreateAccount = StrUser
                    .UpdateAccount = StrUser
                    .CreateId = GlobalConst.CalDavProgramId
                    .UpdateId = GlobalConst.CalDavProgramId
                    If Not InsertFlag Then
                        '更新時はイベント全アラームデータを物理削除する
                        alarmInfo.DeleteEventAlarm(IndatAlarm)
                    End If

                    For i As Integer = 0 To Vcal.NotifyCount - 1
                        Logger.Debug("Notify:" & (i + 1).ToString(CurrentCulture()) & Vcal.Notify(i))
                        .SeqNo = i + 1    '1オリジンなので+1
                        .StartTrigger = Vcal.Notify(i)

                        'データInsert
                        alarmInfo.InsertEventAlarm(IndatAlarm)

                    Next
                End With
            End Using

        End Sub


        ''' <summary>
        ''' RRULE文字列を解析
        ''' </summary>
        ''' <param name="freq"></param>
        ''' <param name="interval"></param>
        ''' <param name="until"></param>
        ''' <param name="str"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function AnalyzeRrule(ByRef Freq As String, ByRef Interval As String, ByRef Until As DateTime, ByVal Str As String) As String

            '既定値
            Freq = "DAILY"
            Interval = "1"     'Interval
            'untilの既定値は無し
            Until = GlobalConst.DateNothingValue

            'さらにセミコロンで分離

            Dim tag() As String = Str.Split(";")
            For i As Integer = 0 To tag.GetUpperBound(0)
                Dim dat As String = tag(i)
                Select Case dat
                    Case "FREQ=DAILY"
                        Freq = "DAILY"
                        Until = GlobalConst.DateHighValue
                    Case "FREQ=WEEKLY"
                        Freq = "WEEKLY"
                        Until = GlobalConst.DateHighValue
                    Case "FREQ=WEEKLY:INTERVAL=2"
                        Freq = "WEEKLY"
                        Interval = "2"
                        Until = GlobalConst.DateHighValue
                    Case "FREQ=MONTHLY"
                        Freq = "MONTHLY"
                        Until = GlobalConst.DateHighValue
                    Case "FREQ=YEARLY"
                        Freq = "YEARLY"
                        Until = GlobalConst.DateHighValue
                    Case Else
                        If dat.IndexOf("INTERVAL", StringComparison.CurrentCulture) >= 0 Then
                            Interval = Mid(dat, 10) '=の次の文字
                        End If

                        If dat.IndexOf("UNTIL", StringComparison.CurrentCulture) >= 0 Then
                            Dim d As String = Mid(dat, 7)
                            '時差を付加する　2011/12/29
                            'Until = ToDateTime(d)
                            Until = DateAdd(DateInterval.Hour, TimeDiff, ToDateTime(d))
                        End If

                End Select
            Next

            Return Str '入力のまま

        End Function

        ''' <summary>
        ''' 文字列 YYYYMMDDThhmmss または YYYYMMDDThhmmssZをDateTime型に変換
        ''' </summary>
        ''' <param name="str"></param>
        ''' <returns></returns>
        ''' <remarks>YYYYMMDDThhmmssまたはYYYYMMDDThhmmssZに対応
        ''' 追加：終日の場合、VALUE=DATE:20111213　がくる　→　対応
        ''' </remarks>
        Private Shared Function ToDateTime(ByVal Str As String, Optional ByVal allday As Boolean = False) As DateTime
            Dim RetDate As DateTime = GlobalConst.DateNothingValue
            Str = Str.Trim 'ホワイトスペースを削除

            '15文字または16文字のみ対応
            '15文字　YYYYMMDDThhmmss
            '16文字　YYYYMMDDThhmmssZ
            If Str.Length = 15 OrElse Str.Length = 16 Then
                Dim LongDate As New StringBuilder
                LongDate.Append(Left(Str, 4) & "/" _
                & Mid(Str, 5, 2) & "/" _
                & Mid(Str, 7, 2))

                '終日でない場合
                If Not allday Then
                    LongDate.Append(" " & Mid(Str, 10, 2) & ":" _
                    & Mid(Str, 12, 2) & ":" _
                    & Mid(Str, 14, 2))
                End If
                RetDate = LongDate.ToString

            ElseIf Str.Length = 8 OrElse Str.Length = 9 Then
                '
                Dim ShortDate As New StringBuilder
                ShortDate.Append(Left(Str, 4) & "/" _
                & Mid(Str, 5, 2) & "/" _
                & Mid(Str, 7, 2))

                '終日でない場合
                If Not allday Then
                    ShortDate.Append(" 00:00:00")
                End If
                RetDate = ShortDate.ToString
            End If

            Return RetDate

        End Function


        ''' <summary>
        ''' 基本ヘッダを出力
        ''' </summary>
        ''' <remarks>
        ''' 全体の処理で普遍のヘッダをレスポンス出力
        ''' </remarks>
        Private Shared Sub WriteBasicHeader(ByRef Response As HttpResponse)

            'デバッグではヘッダに書き込めないため #If Not Debugになっている
#If Not Debug Then

            Response.Headers.Remove("Server")
            Response.Headers.Add("Server", GlobalConst.HeadServer)
            Response.ContentEncoding = System.Text.Encoding.UTF8
            Response.ContentType = "text/xml"

            'Logger 出力
            Logger.Debug("HEADER:" & "Server:" & GlobalConst.HeadServer)
            Logger.Debug("HEADER:" & "ContentEncoding:" & System.Text.Encoding.UTF8.ToString)
            Logger.Debug("HEADER:" & "ContentType:" & "text/xml")
#End If

        End Sub

        '廃止
        ' ''' <summary>
        ' ''' ポスト処理
        ' ''' </summary>
        ' ''' <remarks>
        ' ''' 何もしない
        ' ''' </remarks>
        'Shared Sub ResPost()

        '    'Postに関しては何もしない

        'End Sub


        '廃止
        ' ''' <summary>
        ' ''' リクエストヘッダ
        ' ''' </summary>
        ' ''' <remarks>
        ' ''' 何もしない
        ' ''' </remarks>
        'Shared Sub ResHead()

        '    'Headに関しては何もしない

        'End Sub


        ''' <summary>
        ''' DELETE 処理
        ''' </summary>
        ''' <remarks>
        ''' DELETEは　Request.PathInfoから対象のデータ(uid)を特定する
        ''' </remarks>
        Sub ResDelete()
            Dim Rec As Integer = 0 '実行した件数

            Logger.Info("[IC3040404Response:resDelete] Start  User:" & InHeaderInfo.GetUser)
            WriteBasicHeader(InResponse)

            Dim uid As String = PickUid(InRequest.PathInfo())

            If Not String.IsNullOrEmpty(uid) Then
                'uidが取れた場合のみ
                Logger.Info("   DELETE uid:" & uid)

                Using modifyInfo As New DataAccess.IC3040404.IC3040404.Api.DataAccess.IC3040404DataTable

                    Dim indat As New DataAccess.IC3040404.IC3040404.Api.DataAccess.CalEventItem
                    Dim dat As DateTime = Now '現在時刻
                    Dim strUser As String = InHeaderInfo.GetUser()
                    With indat
                        .CalId = "NATIVE"
                        .UniqueId = uid
                        .DelFlg = "1"   '削除
                        .DelDate = dat
                        .RecurrenceId = "" '対象UID全件削除
                        .UpdateDate = dat
                        .CreateAccount = strUser
                        .UpdateAccount = strUser
                        .CreateId = GlobalConst.CalDavProgramId
                        .UpdateId = GlobalConst.CalDavProgramId
                    End With

                    Rec = modifyInfo.DeleteEventItem(indat)

                End Using

                'ctagの日付を更新()
                'Dim cTagDate As Date = Now
                'cTagDate = InHeaderInfo.GetCtagDate(True)

            End If

            'リターン値は通常の考え方と異なるので注意
            If Rec = 0 Then
                'できなかった場合は、OKを返す
                InResponse.StatusCode = GlobalConst.HttpStat200 'MultiStatus
            Else
                'OKの場合は　204（No Content）を返す
                InResponse.StatusCode = GlobalConst.HttpStat204 'NoContent

            End If

            Logger.Info(" [IC3040404Response:resDelete] Exit")

        End Sub

        ''' <summary>
        ''' ステータスをメンバー変数に設定
        ''' </summary>
        ''' <param name="status"></param>
        ''' <remarks></remarks>
        Sub SetStatus(ByVal status As Integer)
            InResponse.StatusCode = status
        End Sub


        ''' <summary>
        ''' BASIC認証を要求
        ''' </summary>
        ''' <remarks>
        ''' BASIC認証なしにHTTPメソッドが呼ばれたときにBASIC認証の要求を返す
        ''' </remarks>
        Public Sub RequestCertify()
            WriteBasicHeader(InResponse)
            Dim sHostIP As String = InRequest.ServerVariables("SERVER_NAME")  'ホストのIP取得

#If Not Debug Then

            InResponse.Headers.Add("WWW-Authenticate", "Basic realm=" & sHostIP)
            InResponse.ContentType = "text/html"
#End If
            InResponse.StatusCode = GlobalConst.HttpStat401 '401 Authenticate要求

        End Sub


        ''' <summary>
        ''' オプション処理
        ''' </summary>
        ''' <remarks>
        ''' 固定の文字列を返す
        ''' </remarks>
        Sub ResOptions()

            WriteBasicHeader(InResponse)

#If Not Debug Then
            InResponse.Headers.Add("DAV", GlobalConst.HeadDav)
            InResponse.Headers.Add("Allow", GlobalConst.HeadAllow)
#End If
            InResponse.StatusCode = GlobalConst.HttpStat200  'OK

        End Sub


        ''' <summary>
        ''' レポート処理
        ''' </summary>
        ''' <remarks>
        ''' 対応の種類が少ないので propfindのようにdictionary 引きを行わない
        ''' </remarks>
        Sub ResReport()
            Logger.Info("[IC3040404Response:resReport] Start  User:" & InHeaderInfo.GetUser)
            WriteBasicHeader(InResponse)

            If IsNothing(InXml) OrElse IsNothing(InXml.DocumentElement) Then 'Request Bodyがない
                InResponse.StatusCode = GlobalConst.HttpStat422 'Unprocessible Entity
            Else
                Dim Root As XmlElement = InXml.DocumentElement
                Dim StartDate As DateTime = GlobalConst.DateNothingValue  '0001/01/01 00:00:01
                Dim EndDate As DateTime = GlobalConst.DateHighValue       '9999/12/31 23:59:59
                Dim EtagFlag As Boolean = False     'etag作成フラグ
                Dim CalendarFlag As Boolean = False 'VCALENDAR作成フラグ
                Dim VeventList As Boolean = False   'Trueのときetag list を返す

                'メンバ変数にも代入
                InStartDate = StartDate
                InEndDate = EndDate

                'ヘッダに追加
#If Not Debug Then
                InResponse.Headers.Add("DAV", GlobalConst.HeadDav)
                InResponse.Headers.Add("ETag", MakeEtag())
#End If

                'uidリストの格納場所
                Dim UidList As New StringBuilder
                UidList.Length = 0
                Dim UidListCount As Integer = -1

                '404 Not Findデータ格納場所  
                Dim NgData As New StringBuilder
                NgData.Length = 1

                'ヘッダは未出力
                'Dim HeaderOutput As Boolean = False
                HeadPattern = 11  'Reportの標準ヘッダ

                For Each Node As XmlNode In Root
                    'エレメントの取得
                    Dim Key() As String
                    If Node.NodeType = XmlNodeType.Element Then
                        If Node.Name.IndexOf("href", StringComparison.CurrentCulture) > 0 Then
                            'ReDim Preserve uidList(uidListCount)
                            If UidListCount >= 0 Then '2番目から改行をつける
                                UidList.Append(vbCrLf)
                            End If
                            UidList.Append(PickUid(Node.InnerXml)) 'uidのみを抽出する関数
                            '文字列にuidを蓄積
                            UidListCount += 1
                        Else
                            Try
                                For Each Elm As XmlElement In Node
                                    Key = Elm.Name.Split(":")           '名前空間を分離
                                    Dim Tag As String = ""
                                    If Key.GetUpperBound(0) > 0 Then Tag = Key(1) '調べるkeyword

                                    Dim Stat As Integer      '返答の種類 =0 Ok(200) , =1 NG(404)
                                    Dim StrSpace As String = "" '名前空間（使わない）
                                    Dim Res As String = GetResStrReport(Stat, StrSpace, Tag) '返答する内容

                                    'veventがある=etaglistを出す
                                    If Elm.OuterXml.IndexOf("VEVENT", StringComparison.CurrentCulture) >= 0 Then
                                        VeventList = True
                                    End If

                                    If Elm.Name.IndexOf("comp-filter", StringComparison.CurrentCulture) > 0 Then
                                        GetStartEndTime(Elm)
                                        StartDate = InStartDate
                                        EndDate = InEndDate
                                    End If

                                    If Stat = StatusOK Then    'OK Data
                                        'elementの内容によりフラグを付ける
                                        If Tag.IndexOf("getetag", StringComparison.CurrentCulture) >= 0 Then
                                            EtagFlag = True
                                        End If
                                    Else    'Not Found Data
                                        If Tag.IndexOf("calendar-data", StringComparison.CurrentCulture) >= 0 Then
                                            CalendarFlag = True
                                        Else
                                            NgData.Append(MakeOneElement(Res, StrSpace, Tag, True))
                                        End If
                                    End If
                                Next

                            Catch ex As ApplicationException
                                Logger.Error("resReport:XML処理で失敗 ex:" & ex.ToString)
                            End Try
                        End If
                    End If
                Next

                '空行を出力
                InResponse.Write("")

                '全体ヘッダ出力
                If CalendarFlag Then 'CALENDARのある場合
                    OutRespXml(GlobalConst.HttpHead12)           '<?xml....
                Else '標準
                    OutRespXml(GlobalConst.HttpHeadStd)           '<?xml....
                End If

                'VCALENDAR出力時
                If CalendarFlag And UidListCount >= 0 Then
                    '
                    OutVcalendar(UidList, UidListCount, EtagFlag)
                    'フッタ出力
                    OutRespXml(GlobalConst.HttpResFoot2)

                ElseIf VeventList Then  'etaglistを出力する時
                    OutVeventList(StartDate, EndDate)

                    'フッタ出力
                    OutRespXml(GlobalConst.HttpResFoot2)
                    '404 Not Foundの部分
                ElseIf NgData.Length > 0 Then
                    OutRespXml("<response>")
                    OutRespXml(GlobalConst.HttpResBodyHead)
                    OutRespXml(NgData.ToString)
                    OutRespXml(GlobalConst.HttpResFootProp404)
                End If

                'オブジェクトの廃棄
                UidList = Nothing
                NgData = Nothing

                InResponse.StatusCode = GlobalConst.HttpStat207 'MultiStatus

            End If

            Logger.Info(" [IC3040404Response:resReport] Exit")
        End Sub


        ''' <summary>
        ''' XMLから開始終了時間を取得
        ''' </summary>
        ''' <param name="elm"></param>
        ''' <remarks>
        ''' ルールセットにより、関数化
        ''' </remarks>
        Private Sub GetStartEndTime(ByVal elm As Xml.XmlElement)

            For Each elm2 As XmlElement In elm
                '時間項目を取得
                Debug.WriteLine(elm2.InnerXml)
                '値があれば　time-range　が入っている
                Dim elmXml As String = elm2.InnerXml
                If elmXml.IndexOf("time-range", StringComparison.CurrentCulture) >= 0 Then
                    'TimeRangeあり
                    If elmXml.IndexOf("start", StringComparison.CurrentCulture) >= 0 Then InStartDate = GetXmlToDate(elmXml)
                    If elmXml.IndexOf("end", StringComparison.CurrentCulture) >= 0 Then InEndDate = GetXmlToDate(elmXml, "end")
                End If
            Next

        End Sub


        ''' <summary>
        ''' UIDのデータ数
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ReadOnly Property UidCount As Integer
            Get
                Return InUidCount
            End Get
        End Property


        ' ''' <summary>
        ' ''' VEVENTの出力(uid）　参照用　
        ' ''' </summary>
        ' ''' <returns>UID</returns>
        ' ''' <remarks>
        ' ''' 参照用　ユーザーの全件取得
        ' ''' 対象条件は　OPERATIONCODEが8か9
        ' ''' で、ICROPINFOのSCHEDULEDIVが0の場合のEVENTITEM
        ' ''' 対象件数は　PropertyのUidCountで取得する
        ' ''' </remarks>
        'Function GetVeventList() As String

        '    Dim Ope As String = InHeaderInfo.GetOpeCode  'オペレーションコード　8または9
        '    Dim Kind As Integer = 1 'カレンダー参照用
        '    Dim Account As String = InHeaderInfo.GetUser
        '    Dim Rec As Integer = -1
        '    Dim Uid As New StringBuilder
        '    Uid.Length = 0

        '    Using EventInfo As New DataAccess.IC3040404.IC3040404.Api.DataAccess.IC3040404DataTable
        '        Dim Ret As IC3040404DataSet.TableDataTableDataTable = _
        '                    EventInfo.GetEventItem(Account, _
        '                                           GlobalConst.DateLowValue, _
        '                                           GlobalConst.DateHighValue, _
        '                                           Ope, _
        '                                           Kind)

        '        Dim CtagDate As DateTime = GlobalConst.DateLowValue

        '        If Not IsNothing(Ret) Then
        '            'レコード1件ごとに処理を行う
        '            For Each Row As DataRow In Ret
        '                If Rec >= 0 Then
        '                    Uid.Append(vbCrLf)
        '                End If
        '                Uid.Append(Row.Item("UNIQUEID"))
        '                Rec += 1
        '            Next
        '        End If

        '    End Using

        '    '件数
        '    InUidCount = Rec

        '    'UID
        '    Return Uid.ToString

        'End Function

        ''' <summary>
        '''  VEVENTの出力(uid）　参照用
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' ルールセットでプロパティにしなさいと指示
        ''' 
        ''' 参照用　ユーザーの全件取得
        ''' 対象条件は　OPERATIONCODEが8か9
        ''' で、ICROPINFOのSCHEDULEDIVが0の場合のEVENTITEM
        ''' 対象件数は　PropertyのUidCountで取得する
        ''' </remarks>
        ReadOnly Property GetVeventList As String
            Get
                Dim Ope As String = InHeaderInfo.GetOpeCode  'オペレーションコード　8または9
                Dim Kind As Integer = 1 'カレンダー参照用
                Dim Account As String = InHeaderInfo.GetUser
                Dim Rec As Integer = -1
                Dim Uid As New StringBuilder
                Uid.Length = 0

                Using EventInfo As New DataAccess.IC3040404.IC3040404.Api.DataAccess.IC3040404DataTable
                    Dim Ret As IC3040404DataSet.TableDataTableDataTable = _
                                EventInfo.GetEventItem(Account, _
                                                       GlobalConst.DateLowValue, _
                                                       GlobalConst.DateHighValue, _
                                                       Ope, _
                                                       Kind)

                    Dim CtagDate As DateTime = GlobalConst.DateLowValue

                    If Not IsNothing(Ret) Then
                        'レコード1件ごとに処理を行う
                        For Each Row As DataRow In Ret
                            If Rec >= 0 Then
                                Uid.Append(vbCrLf)
                            End If
                            Uid.Append(Row.Item("UNIQUEID"))
                            Rec += 1
                        Next
                    End If

                End Using

                '件数
                InUidCount = Rec

                'UID
                Return Uid.ToString

            End Get
        End Property


        ''' <summary>
        ''' VEVENTの出力（etagのリスト）
        ''' </summary>
        ''' <param name="StartDate">開始日</param>
        ''' <param name="EndDate">終了日</param>
        ''' <remarks></remarks>
        Sub OutVeventList(ByVal startDate As DateTime, ByVal endDate As DateTime)
            Dim OpeCode As String = InHeaderInfo.GetOpeCode
            Dim Kind As Integer = 0 'カレンダー（ネイティブ）用

            Dim Account As String = InHeaderInfo.GetUser
            Using EventInfo As New DataAccess.IC3040404.IC3040404.Api.DataAccess.IC3040404DataTable
                Dim Ret As IC3040404DataSet.TableDataTableDataTable = _
                            EventInfo.GetEventItem(Account, startDate, endDate, OpeCode, Kind)
                Dim Uid As String
                Dim CtagDate As DateTime = GlobalConst.DateLowValue
                Dim Path As String
                Dim Etag As String
                Dim Dat As String
                If Not IsNothing(Ret) Then
                    'レコード1件ごとに処理を行う
                    For Each row As DataRow In Ret
                        Uid = row.Item("UNIQUEID")
                        CtagDate = row.Item("UPDATEDATE")
                        Path = MakeIcsName(Uid) 'icsファイル名
                        Dat = Format(CtagDate, "yyyy/MM/dd HH:mm:ss")
                        Etag = CreateMd5(Path, Dat)

                        'Xml出力
                        OutRespXml("<response>" & CrLF)
                        OutRespXml("<href>" & Path & "</href>" & CrLF)
                        OutRespXml("<propstat>" & CrLF)
                        OutRespXml("<prop>" & CrLF)
                        OutRespXml("<getetag>""" & Etag & """</getetag>" & CrLF)

                        OutRespXml(GlobalConst.HttpResFootProp200)

                        OutRespXml("</response>" & CrLF)
                    Next
                End If

            End Using

        End Sub


        ''' <summary>
        ''' uidからicsファイル名を作成
        ''' </summary>
        ''' <param name="uid">uid</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' パスと拡張子を付けたファイル名を作成
        ''' </remarks>
        Private Function MakeIcsName(ByVal Uid As String) As String

            Dim Str As New StringBuilder
            Str.Length = 0
            Str.Append(GetPathInfo(GlobalConst.CalendarPath) & Uid & ".ics")

            Return Str.ToString

        End Function


        ''' <summary>
        ''' VCALENDARを返す
        ''' </summary>
        ''' <param name="uidList">uidを格納した文字列　データ区切りはCrLf</param>
        ''' <param name="uidListCount">uidの要素数</param>
        ''' <param name="etagflag">trueのとき、getetagタグをつける</param>
        ''' <remarks></remarks>
        Private Sub OutVcalendar(ByVal UidList As StringBuilder, ByVal UidListCount As Integer, ByVal EtagFlag As Boolean)
            'VCALENDARのデータを作成
            Dim Dat As DateTime
            Dim EventData As New StringBuilder
            Dim Path As String
            Dim Etag As String
            Dim Uid() As String = UidList.ToString.Split(vbCrLf)

            For i As Integer = 0 To UidListCount
                Uid(i) = Uid(i).Trim

                'OutRespXml(uid & CrLF)
                Dat = GlobalConst.DateLowValue

                EventData.Length = 0
                MakeVcalendar(EventData, Dat, Uid(i))

                Path = MakeIcsName(Uid(i))
                Etag = CreateMd5(Path, Dat)

                OutRespXml(String.Format(CurrentCulture(), GlobalConst.HttpResHead, MakeIcsName(Uid(i))))
                OutRespXml(GlobalConst.HttpResBodyHead)
                If EtagFlag Then
                    OutRespXml("<getetag>""" & Etag & """</getetag>" & CrLF)
                End If

                'xmlを出力
                OutRespXml(EventData.ToString)

                'calendarデータの終了
                OutRespXml("</C:calendar-data>" & CrLF)

                OutRespXml(GlobalConst.HttpResFootProp200)
                OutRespXml("<propstat>" & CrLF)
                OutRespXml("<prop>" & CrLF)
                OutRespXml("<schedule-tag/>" & CrLF)
                OutRespXml(GlobalConst.HttpResFootProp404)
                OutRespXml("</response>" & CrLF)

            Next

        End Sub


        ''' <summary>
        ''' uidを含むxmlからuidのみをピックアップする
        ''' </summary>
        ''' <param name="str"></param>
        ''' <returns>ピックアップした文字列</returns>
        ''' <remarks>
        ''' パス名とファイルの拡張子があれば削除する
        ''' </remarks>
        Private Shared Function PickUid(ByVal str As String) As String
            Dim Uid As String = ""

            '最後の/以降を切り取る
            Dim StrSplit() As String = str.Split("/")
            Uid = StrSplit(StrSplit.GetUpperBound(0)) '最後の文字列
            Uid = Uid.Replace(".ics", "")

            Return Uid

        End Function


        ''' <summary>
        ''' xmlから時間を抽出
        ''' </summary>
        ''' <param name="d"></param>
        ''' <param name="kind">startのときはDATE_LOW_VALUEを既定値
        ''' 　　　　　　　　　その他は(end)はDATE_HIGH_VALUEで返す</param>
        ''' <returns>日時型の値</returns>
        ''' <remarks>xmlからyyyymmddTHHMMSS　の文字列を抽出し
        ''' 一旦、yyyy/mm/dd HH:MM:SSの文字列に変換し、日時型にする
        ''' </remarks>
        Private Shared Function GetXmlToDate(ByVal d As String, Optional ByVal kind As String = "start") As DateTime
            Dim Ret As DateTime
            Dim StartNum As Integer = d.IndexOf(kind, StringComparison.CurrentCulture)
            Dim Str As String = ""
            Dim EndNum As Integer

            Const SubtractSize As Integer = 7       '7は"start="の7文字
            Const TimeStringSize As Integer = 15    '時間文字列の文字数

            '初期設定
            If String.Equals(kind, "start") Then
                Ret = GlobalConst.DateLowValue
            Else
                Ret = GlobalConst.DateHighValue
            End If

            Try
                If StartNum >= 0 Then
                    Str = d.Substring(StartNum + SubtractSize)
                    '次の"の位置
                    EndNum = Str.IndexOf("""", StringComparison.CurrentCulture)
                    If EndNum > 0 Then
                        Str = Left(Str, EndNum - 1) '最後のZを除く（オフセットが0なので、"を含め2文字前）
                    End If
                End If

                If Str.Length = TimeStringSize Then

                    Dim StrData As New StringBuilder
                    StrData.Length = 0
                    StrData.Append(Left(Str, 4) & "/" & Mid(Str, 5, 2) & "/" & Mid(Str, 7, 2) & " ")
                    StrData.Append(Mid(Str, 10, 2) & ":" & Mid(Str, 12, 2) & ":" & Mid(Str, 14, 2))

                    Ret = CDateLike(StrData.ToString)

                End If

            Catch ex As ApplicationException
                Logger.Error("[IC3040404Response:GetXmlToDate] Not Pickup from xml:" & d)

            End Try

            Return Ret

        End Function


        ''' <summary>
        ''' VCALENDARデータを作成（iCal用）
        ''' </summary>
        ''' <param name="eventData"></param>
        ''' <param name="dat"></param>
        ''' <param name="uid"></param>
        ''' <remarks></remarks>
        Private Sub MakeVcalendar(ByRef EventData As StringBuilder, ByRef Dat As DateTime, ByVal Uid As String)

            'ヘッダを書く
            MakeVcalendarhead(EventData)

            'イベントアイテムを条件にそって取得する

            MakeVevent(EventData, Dat, Uid)

            '終了
            EventData.Append("END:VCALENDAR" & CrLF)

        End Sub


        ''' <summary>
        ''' VCALENDAR情報を作成する
        ''' </summary>
        ''' <param name="eventData">作成したVCALENDAR情報</param>
        ''' <remarks>
        ''' 更新日はctag作成用に必要
        ''' </remarks>
        Private Sub MakeVcalendarhead(ByRef EventData As StringBuilder, Optional ByVal ForGet As Boolean = False)

            ''200 OKデータ格納場所
            'eventData.Length = 0

            For Each Key As String In XmlData.ResVcalendar.Keys

                Dim StrVal As String = XmlData.ResVcalendar(Key)

                'セパレータ":"でパターン番号と名前空間と内容に分離
                Dim IndexNum As Integer = 0       '変換インデクス
                Dim Contents As String = ""     '内容
                Dim StrSpace As String = ""       '名前空間
                IndexNum = SeparateTag(StrSpace, Contents, StrVal)

                'インデクスごとの処理
                Select Case IndexNum
                    Case 0      '何もつけない
                        EventData.Append(Key & vbCrLf)

                    Case 98 'X-WR-CALNAME
                        '照会のときは、カレンダー名（照会名）
                        If ForGet Then
                            EventData.Append(Key & ":" & _
                                 InGlobal.ReferenceName & vbCrLf)
                        End If

                    Case 99 'スタートタグ
                        If ForGet Then
                            EventData.Append("BEGIN:VCALENDAR" & vbCrLf)
                        Else
                            EventData.Append(Key & vbCrLf)
                        End If

                    Case 100    'そのまま連結
                        EventData.Append(Key & ":" & Contents & vbCrLf)
                    Case 102    'ロケール
                        EventData.Append(Key & ":" & String.Format(CurrentCulture(), Contents, TimeLocale) & CrLF)
                    Case 103    'TimeZone From
                        EventData.Append(Key & ":" & String.Format(CurrentCulture(), Contents, TimeFrom) & CrLF)
                    Case 104    'TimeZone To
                        EventData.Append(Key & ":" & String.Format(CurrentCulture(), Contents, TimeTo) & CrLF)
                    Case 105    'TimeZone Name
                        EventData.Append(Key & ":" & String.Format(CurrentCulture(), Contents, TimeName) & CrLF)
                    Case 106    'DT Start
                        EventData.Append(Key & ":" & String.Format(CurrentCulture(), Contents, StartDate) & CrLF)
                    Case Else   '0と同じ
                        EventData.Append(Key & CrLF)

                End Select
            Next

        End Sub


        ' ''' <summary>
        ' ''' DBから予定を拾い、VEVENTデータをデータ数だけ
        ' ''' VEVENTデータを作成する
        ' ''' </summary>
        ' ''' <param name="EventData">作成したイベントデータ</param>
        ' ''' <param name="Account">対象のアカウント</param>
        ' ''' <param name="StartDate">対象開始日</param>
        ' ''' <param name="EndDate">対象終了日</param>
        ' ''' <param name="Kind">カレンダー用=0 照会用=1　既定値0</param>
        ' ''' <remarks>
        ' ''' デフォルトはiCal用(kind=0)
        ' ''' </remarks>
        'Private Sub MakeVevent(ByRef EventData As StringBuilder, Account As String, _
        '                StartDate As Date, EndDate As Date, Optional Kind As Integer = 0)
        '    Dim Ope As String = InHeaderInfo.GetOpeCode

        '    Using EventInfo As New DataAccess.IC3040404.IC3040404.Api.DataAccess.IC3040404DataTable
        '        Dim Ret As IC3040404DataSet.TableDataTableDataTable = _
        '                    EventInfo.GetEventItem(Account, StartDate, EndDate, Ope, Kind)
        '        Dim Uid As String = String.Empty
        '        Dim NewUid As String = String.Empty

        '        If Not IsNothing(Ret) Then
        '            'レコード1件ごとに処理を行う
        '            Dim AllDay As Boolean
        '            For Each Row As DataRow In Ret.Rows
        '                'newUid = row.Item("UNIQUEID")
        '                'Uniqueidとリカレンスの併せたものをマッチングキーとする
        '                NewUid = Row.Item("UNIQUEID") & Row.Item("RECURRENCEID")
        '                If String.Equals(Row.Item("ALLDAYFLG"), "1") Then
        '                    AllDay = True   '終日用
        '                Else
        '                    AllDay = False
        '                End If
        '                If Uid <> NewUid Then
        '                    'イベントをクローズしていないとき
        '                    If Not String.IsNullOrEmpty(Uid) Then
        '                        EventData.Append(MakeEventEnd())
        '                    End If
        '                    'アラームはBODYで作成 2011/12/20
        '                    EventData.Append(MakeEventBody(Row))
        '                    Uid = NewUid
        '                End If
        '            Next

        '            '最後のVEVENTをクローズ
        '            If Not String.IsNullOrEmpty(Uid) Then
        '                EventData.Append(MakeEventEnd())
        '            End If

        '        End If
        '    End Using

        'End Sub


        ''' <summary>
        ''' VEVENT情報を作成する
        ''' </summary>
        ''' <param name="EventData">作成したVEVENT</param>
        ''' <param name="UpdateTime">更新日 ctag保存用に使う</param>
        ''' <param name="Uid">対象とするユニークID</param>
        ''' <param name="Recur">リカレンスID(option)</param>
        ''' <remarks>
        ''' ユニークIDでイベントアイテムを引く
        ''' </remarks>
        Private Sub MakeVevent(ByRef EventData As StringBuilder, ByRef UpdateTime As DateTime, ByVal Uid As String, _
                               Optional ByVal Recur As String = "")

            Using EventInfo As New DataAccess.IC3040404.IC3040404.Api.DataAccess.IC3040404DataTable
                Dim Ret As IC3040404DataSet.TableDataTableDataTable = _
                            EventInfo.SelectEventItem(Uid, Recur)

                Dim NewUid As String = String.Empty 'マッチングキー
                Dim OldUid As String = String.Empty 'マッチングキー

                If Not IsNothing(Ret) Then
                    'レコードごとの処理を行う
                    For Each Row As DataRow In Ret
                        'Uniqueidとリカレンスの併せたものをマッチングキーとする
                        NewUid = Row.Item("UNIQUEID") & Row.Item("RECURRENCEID")
                        UpdateTime = CDateLike(Row.Item("UPDATEDATE"))
                        If Not String.Equals(OldUid, NewUid) Then
                            'イベントをクローズしていないとき
                            '　(oldUidに何かあるとき）
                            If Not String.IsNullOrEmpty(OldUid) Then
                                EventData.Append(MakeEventEnd())
                            End If
                            'アラームはBODYで作成 2011/12/20
                            EventData.Append(MakeEventBody(Row))
                            OldUid = NewUid
                        End If
                    Next

                    '最後のVEVENTをクローズ
                    If Not String.IsNullOrEmpty(Uid) Then
                        EventData.Append(MakeEventEnd())
                    End If

                End If
            End Using
        End Sub

        ''' <summary>
        ''' 文字を数値に変換
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' DBNullのときもゼロを返す
        ''' </remarks>
        Private Shared Function CNullInt(ByVal Obj As Object) As Integer
            Dim Ret As Integer = 0
            Try
                If Not IsDBNull(Obj) Then
                    Ret = CInt(Obj)
                End If
            Catch ex As ApplicationException
                Ret = 0
            End Try

            Return Ret
        End Function

        ''' <summary>
        ''' EVENT BODY を rowから作成する
        ''' </summary>
        ''' <param name="row">1件分のレコード</param>
        ''' <returns>作成したBODY文字列</returns>
        ''' <remarks>
        ''' 開始タグ　BEGIN:VEVENTはつける
        ''' 終了タグ　END:VEVENTは別関数でつける
        ''' </remarks>
        Private Function MakeEventBody(ByVal Row As DataRow) As String
            Dim EventData As New StringBuilder
            Dim DefaultLocal As String = InGlobal.DefaultLocal

            EventData.Length = 0

            With EventData
                .Append("BEGIN:VEVENT" & CrLF)

                '予定出席者 機能削除(2011/12/17) 将来復活する可能性があるのでコード削除しない
                If Not IsDBNull(Row.Item("ATTENDEE")) Then
                    Dim Str As String = Row.Item("ATTENDEE")
                    Dim Attendee() As String = Str.Split("#")

                    For Each Atn In Attendee
                        .Append("ATTENDEE;" & Atn.Trim & CrLF)
                    Next
                End If

                '時間指定か終日かで動きが変化
                Dim AlldayFlag As Boolean = False
                If String.Equals(Row.Item("ALLDAYFLG"), "1") Then
                    AlldayFlag = True
                End If
                .Append("DTSTART" & PrettyDate(Row.Item("STARTTIME"), DefaultLocal, AlldayFlag) & CrLF)
                .Append("DTEND" & PrettyDate(Row.Item("ENDTIME"), DefaultLocal, AlldayFlag, True) & CrLF)

                'Add 除外日（EXDATE）の情報付加 2012/12/17
                Dim ExDate As String = GetExdateInfo(Row.Item("EVENTID"))
                If Not String.IsNullOrEmpty(ExDate) Then
                    .Append(ExDate)
                End If

                'RRULEはテキスト保存のデータをそのまま出力
                If String.Equals(Row.Item("RRULEFLG"), "1") Then
                    .Append("RRULE:" & Row.Item("RRULE_TEXT") & CrLF)
                End If

                .Append("DTSTAMP" & PrettyDate(Row.Item("ENDTIME")) & "Z" & CrLF)

                'ADD 2011/12/23
                If Not IsDBNull(Row.Item("URL")) Then
                    .Append("URL:" & Row.Item("URL") & CrLF)
                End If
                .Append("UID:" & PrettyUID(Row.Item("UNIQUEID")) & CrLF)
                .Append("CREATED" & PrettyDate(Row.Item("CREATEDATE")) & "Z" & CrLF)
                Dim description As String = GetDescription(Row)
                If Not String.IsNullOrEmpty(description) Then   'descriptionは「メモ」
                    .Append("DESCRIPTION:" & EscStr(description) & CrLF)
                End If
                .Append("LAST-MODIFIED" & PrettyDate(Row.Item("UPDATEDATE")) & "Z" & CrLF)
                If Not IsDBNull(Row.Item("LOCATION")) Then
                    .Append("LOCATION:" & EscStr(Row.Item("LOCATION")) & CrLF)  '場所(2011/12/20修正)
                End If

                .Append("SEQUENCE:0" & CrLF)                        '固定値
                .Append("STATUS:CONFIRMED" & CrLF)                  '固定値
                '2011/12/30　修正
                If IsDBNull(Row.Item("SUMMARY")) Then
                    .Append("SUMMARY:新規イベント" & CrLF)         '既定のタイトル
                Else
                    .Append("SUMMARY:" & EscStr(Row.Item("SUMMARY")) & CrLF)    'タイトル
                End If
                .Append("TRANSP:OPAQUE" & CrLF)                     '固定値

                'AlarmデータをEXDATEと同様に取得 2011/12/20
                Dim AlarmInfo As String = GetAlarmInfo(Row.Item("EVENTID"), AlldayFlag)
                If Not String.IsNullOrEmpty(AlarmInfo) Then
                    .Append(AlarmInfo)
                End If

                '繰り返しデータで、該当のみを変更した場合
                '　新しいデータをリカレンスID（元の日時）をつけて作成する
                Dim Recur As String = Row.Item("RECURRENCEID").trim
                If Not String.IsNullOrEmpty(Recur) Then
                    .Append("RECURRENCE-ID;TZID=" & DefaultLocal & ":" & Recur & CrLF)
                End If

            End With

            Return EventData.ToString

        End Function

        ''' <summary>
        ''' 除外日のデータを取得
        ''' </summary>
        ''' <param name="eventid"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetExdateInfo(ByVal eventid As String) As String
            '実装例
            'exdate = "EXDATE;TZID=Asia/Shanghai:20111220T140000" & vbCrLf
            'exdate &= "EXDATE;TZID=Asia/Shanghai:20111222T140000" & vbCrLf
            Dim DefaultLocal As String = InGlobal.DefaultLocal
            Dim ExDate As New StringBuilder
            ExDate.Length = 0

            Using ExDateInfo As New DataAccess.IC3040404.IC3040404.Api.DataAccess.IC3040404DataTable
                Dim Ret As IC3040404DataSet.TableDataTableDataTable = _
                            ExDateInfo.GetEventExDate(eventid)

                If Not IsNothing(Ret) Then
                    'レコード1件ごとに処理を行う
                    For Each row As DataRow In Ret
                        ExDate.Append("EXDATE" & PrettyDate(row.Item("EXDATE"), DefaultLocal) & vbCrLf)
                    Next
                End If

            End Using

            Return ExDate.ToString

        End Function


        ''' <summary>
        ''' アラーム情報をMAX2件取得する
        ''' </summary>
        ''' <param name="eventid">EventId</param>
        ''' <param name="allday">終日はTrue</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function GetAlarmInfo(ByVal EventId As String, ByVal AllDay As Boolean) As String
            '
            Dim AlarmData As New StringBuilder
            Const MaxAlarmRec As Integer = 2
            AlarmData.Length = 0

            Using AlarmInfo As New DataAccess.IC3040404.IC3040404.Api.DataAccess.IC3040404DataTable
                Dim Ret As IC3040404DataSet.TableDataTableDataTable = _
                            AlarmInfo.GetAlarm(EventId)

                'アラームデータがないときは何もしない
                If Not IsNothing(Ret) Then
                    '2件まで処理をおこなう
                    ' min(rows, MAX_ALARM_REC)
                    Dim Rec As Integer = Ret.Rows.Count '1オリジン

                    If Rec > MaxAlarmRec Then '2件まで
                        Rec = MaxAlarmRec
                    End If

                    For i As Integer = 0 To Rec - 1
                        Dim Row As DataRow = Ret.Rows(i)
                        AlarmData.Append(MakeAlarmEvent(Row, AllDay))
                    Next

                End If

            End Using

            Return AlarmData.ToString

        End Function

        ''' <summary>
        ''' MEMO欄に色情報を作成する
        ''' </summary>
        ''' <param name="row">1件のレコード</param>
        ''' <returns>編集したMEMO欄</returns>
        ''' <remarks>
        ''' フィールド"ICROPCOLOR"に色情報があれば
        ''' MEMO欄に付記した文字列を返却する
        ''' 結果がない場合は "" を返す
        ''' iPadの仕様ではカンマ(,)をエスケープするが
        ''' ネィティブアプリ上は、今はしない仕様である。
        ''' エスケープしたい場合は、最初の定数値escape
        ''' をtrueで定義する
        ''' </remarks>
        Private Shared Function GetDescription(ByVal Row As DataRow) As String
            'カンマを\でエスケープしたい場合、Trueにする
            'Const escape As Boolean = False

            Dim Str As New StringBuilder
            Str.Length = 0

            Dim Memo As String = String.Empty
            Dim Colors As String = String.Empty

            If Not IsDBNull(Row.Item("MEMO")) Then
                'エスケープは escStrで後でまとめて実行
                Memo = Trim(Row.Item("MEMO"))
                If Not String.IsNullOrEmpty(Memo) Then
                    Str.Append(Memo)
                End If
            End If

            If Not IsDBNull(Row.Item("ICROPCOLOR")) Then
                Colors = "color=" & Trim(Row.Item("ICROPCOLOR"))
                'エスケープは escStrで後でまとめて実行
                If Not String.IsNullOrEmpty(Colors) Then
                    If String.IsNullOrEmpty(Memo) Then
                        Str.Append(Colors)
                    Else
                        'vbCrLfに修正 2011/12/25
                        Str.Append(vbCrLf & Colors)
                    End If
                End If
            End If

            Return Str.ToString

        End Function

        ''' <summary>
        ''' VALARM EVENTをrowから作成する
        ''' </summary>
        ''' <param name="row">1件分のアラーム（通知）レコード</param>
        ''' <returns>作成したVALARM文字列</returns>
        ''' <remarks>
        ''' この関数は、一組分のBEGIN:VALARMからEND:VALARMを作成する
        ''' iPadは　ACTION:DISPLAY　しかない様子（EMAILはない）
        ''' メッセージはiPadでは使用していないようなので固定
        ''' </remarks>
        Private Shared Function MakeAlarmEvent(ByVal Row As DataRow, ByVal AllDay As Boolean) As String
            Dim EventData As New StringBuilder
            EventData.Length = 0

            With EventData
                Dim NotifyCode As Integer = CNullInt(Row.Item("STARTTRIGGER"))
                Dim Notify As String
                '終日かどうか
                If AllDay Then
                    Notify = GetNotifyTime(NotifyCode, True)
                Else
                    Notify = GetNotifyTime(NotifyCode)
                End If

                If Not String.IsNullOrEmpty(Notify) Then
                    .Append("BEGIN:VALARM" & CrLF)
                    .Append("ACTION:DISPLAY" & CrLF)
                    .Append("DESCRIPTION:" & ValarmMessage & CrLF)
                    .Append("TRIGGER:" & Notify & CrLF)
                    .Append("END:VALARM" & CrLF)
                End If

            End With

            Return EventData.ToString
        End Function


        ''' <summary>
        ''' 通知時間をコード表から引く
        ''' </summary>
        ''' <param name="TimeCode">通知時間コード</param>
        ''' <returns>通知時間（VALARM用）</returns>
        ''' <remarks>
        ''' コード表にないコードの場合はstring.emptyを返す
        ''' VBでは定数配列の初期化が難しいので関数で実装
        ''' "-PT5M"などの省略表記で返却する
        ''' </remarks>
        Private Shared Function GetNotifyTime(ByVal TimeCode As Integer, Optional ByVal AllDay As Boolean = False) As String
            Dim Ret As String = ""

            If AllDay Then '終日
                Select Case TimeCode
                    Case 1  '予定時刻  9:00
                        Ret = "PT9H"
                    Case 7 '1日前      9:00
                        Ret = "-PT15H"
                    Case 8 '2日前      9:00
                        Ret = "-P1DT15H"
                    Case 9 '1週間前    9:00
                        Ret = "-P6DT15H"
                    Case Else '不明
                        Ret = String.Empty
                End Select
            Else
                Select Case TimeCode
                    Case 1  '予定時刻
                        Ret = "PT0S"
                    Case 2  '5分前
                        Ret = "-PT5M"
                    Case 3  '15分前
                        Ret = "-PT15M"
                    Case 4  '30分前
                        Ret = "-PT30M"
                    Case 5  '1時間前
                        Ret = "-PT1H"
                    Case 6 '2時間前
                        Ret = "-PT2H"
                    Case 7 '1日前
                        Ret = "-P1D"
                    Case 8 '2日前
                        Ret = "-P2D"
                    Case Else '不明
                        Ret = String.Empty
                End Select
            End If

            Return Ret

        End Function

        ''' <summary>
        ''' 時間から時間コードを返す
        ''' GetNotifyTimeの逆処理
        ''' </summary>
        ''' <param name="StrCode">時間を示す文字列</param>
        ''' <returns>変換した数字の文字列</returns>
        ''' <remarks>
        ''' 変換できない場合は　"0"の予定時刻を返す
        ''' 対応パターンは、省略表記と通常表記
        ''' コード変更　2011/12/12 0オリジンが　1オリジンに
        ''' </remarks>
        Private Shared Function PutNotifyTime(ByVal StrCode As String, Optional ByVal AllDay As Boolean = False) As String
            Dim Ret As String = ""

            If AllDay Then '終日
                Ret = NotifyCodeAllDay(StrCode)

            Else
                Ret = NotifyCodeTime(StrCode)

            End If

            Return Ret

        End Function

        ''' <summary>
        ''' 終日の場合の通知時間のコード引き
        ''' </summary>
        ''' <param name="StrCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function NotifyCodeAllDay(ByVal StrCode As String) As String
            Dim Ret As String = ""
            Select Case StrCode
                Case "PT9H"     '当日    9:00
                    Ret = "1"
                Case "-PT15H"   '1日前   9:00
                    Ret = "7"
                Case "-P1DT15H" '2日前   9:00
                    Ret = "8"
                Case "-P6DT15H" '1週間前 9:00
                    Ret = "9"
                Case Else '不明
                    Ret = "0"   'なしにする
            End Select
            Return Ret
        End Function

        ''' <summary>
        ''' 時間指定の場合の通知時間のコード引き
        ''' </summary>
        ''' <param name="StrCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function NotifyCodeTime(ByVal StrCode As String) As String
            Dim Ret As String
            Select Case StrCode
                Case "PT0S", "P0DT0H0M0S" '予定時刻
                    Ret = "1"
                Case "-PT5M", "-P0DT0H5M0S"  '5分前
                    Ret = "2"
                Case "-PT15M", "-P0DT0H15M0S" '15分前
                    Ret = "3"
                Case "-PT30M", "-P0DT0H30M0S" '30分前
                    Ret = "4"
                Case "-PT1H", "-P0DT1H0M0S" '1時間前
                    Ret = "5"
                Case "-PT2H", "-P0DT2H0M0S" '2時間前
                    Ret = "6"
                Case "-P1D", "-P1DT0H0M0S" '1日前
                    Ret = "7"
                Case "-P2D", "-P2DT0H0M0S" '2日前
                    Ret = "8"
                Case Else '不明
                    Ret = "0" 'なし
            End Select
            Return Ret
        End Function


        ''' <summary>
        ''' VEVENTのENDコマンドを渡す
        ''' </summary>
        ''' <returns>VEVENTの終端文字列を返す</returns>
        ''' <remarks>
        ''' 返却文字列は "END:VEVENT" 
        ''' </remarks>
        Private Shared Function MakeEventEnd() As String

            Dim str As String = "END:VEVENT" & CrLF

            Return str

        End Function


        ''' <summary>
        ''' uidを整形して返す
        ''' </summary>
        ''' <param name="uid"></param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 文字列はtoupper（大文字）化する
        ''' 1        9    13   17   21
        ''' FC3BB3A0-ADE0-45DF-9D6F-355573B93DEFのような形式
        ''' 8桁　4桁　4桁　4桁　12桁に整形
        ''' 入力文字数が32文字以外はtoupperし、そのまま返す
        ''' </remarks>
        Private Shared Function PrettyUID(ByVal Uid As String) As String
            Dim Str As New StringBuilder
            Dim Pos() As Integer = {1, 9, 13, 17, 21}   '切出す位置　1オリジン
            Dim Len() As Integer = {8, 4, 4, 4, 12}     '切出すサイズ
            Const StrSize As Integer = 32

            Str.Length = 0

            Try
                Uid = Uid.ToUpper(CurrentCulture())
                If Uid.Length = StrSize Then
                    Dim iRepeat As Integer = Pos.GetUpperBound(0)
                    For i As Integer = 0 To iRepeat
                        Str.Append(Mid(Uid, Pos(i), Len(i)))
                        If i <> iRepeat Then '最後は継続(-)がない
                            Str.Append("-")
                        End If
                    Next
                Else
                    Str.Append(Uid)
                End If
            Catch ex As ApplicationException
                Logger.Error("[IC3040404Response:PrettyUID] Not Convert uid:" & Uid)
            End Try

            Return Str.ToString
        End Function


        ''' <summary>
        ''' DateをYYYYMMDDTHHmmSSに変換
        ''' </summary>
        ''' <param name="Dat">変換する日付</param>
        ''' <param name="AllDay">終日のときTrue</param>
        ''' <param name="AllDayAdjust">終了日のときTrue
        ''' alldayがTrueのとき、一日後にする
        ''' </param>
        ''' <returns>変換後のString
        ''' 変換結果の書式
        ''' 地域名がある場合　;TZID=ロケール名:yyyyMMddTHHmmss
        ''' 地域名がない場合　yyyyMMddTHHmmss
        ''' </returns>
        ''' <remarks>
        ''' 日付が不正またはNothingなどのときは
        ''' String.Emptyを返す
        ''' 終日の場合、終了日はiCropは当日、iPadは翌日を返す。
        ''' alldayとalldayAdjustがTrueの場合、日付を翌日にする。2011/12/26修正
        ''' </remarks>
        Private Shared Function PrettyDate(ByVal Dat As Date, Optional ByVal Locale As String = "", _
                                    Optional ByVal AllDay As Boolean = False, _
                                    Optional ByVal AllDayAdjust As Boolean = False) As String
            Dim Ret As String = String.Empty
            Dim Str As New StringBuilder
            Str.Length = 0

            Try
                'Local指定がある場合は前に付ける
                If Not String.IsNullOrEmpty(Locale) Then
                    Str.Append(";TZID=")
                    Str.Append(Locale)
                End If

                If AllDay Then
                    Str.Append(";VALUE=DATE")
                    '終日補正 2011/12/26
                    If AllDayAdjust Then
                        Dat = DateAdd(DateInterval.Day, 1.0, Dat)
                    End If
                End If

                Str.Append(":")
                Str.Append(Format(Dat, "yyyyMMdd"))

                If Not AllDay Then
                    Str.Append("T")
                    Str.Append(Format(Dat, "HHmmss"))
                End If

                Ret = Str.ToString

            Catch ex As ApplicationException
                Ret = String.Empty
                Logger.Error("[IC3040404Response:PrettyDate] Not Convert to String:" & Dat.ToString(CurrentCulture()))
            End Try

            Str = Nothing

            Return Ret

        End Function

        ''' <summary>
        ''' MD5 に変換した文字列を作成する
        ''' </summary>
        ''' <param name="Path">変換元のパス</param>
        ''' <param name="Dat">変換元の日付の文字列</param>
        ''' <returns>MD5に変換した文字列</returns>
        ''' <remarks></remarks>
        Private Shared Function CreateMd5(ByVal Path As String, ByVal Dat As String) As String

            'パスと日付を連結する
            Dim Str As New StringBuilder(Path)
            Str.Append(Dat)

            '文字列をbyte型配列に変換する
            Dim ByteArray As Byte() = System.Text.Encoding.UTF8.GetBytes(Str.ToString)

            'MD5CryptoServiceProviderオブジェクトを作成
            Using Md5 As New System.Security.Cryptography.MD5CryptoServiceProvider()

                'ハッシュ値を計算する
                Dim ByteData As Byte() = Md5.ComputeHash(ByteArray)

                'byte型配列を16進数の文字列に変換
                Dim Result As New System.Text.StringBuilder()
                Dim Byt As Byte
                For Each Byt In ByteData
                    Result.Append(Byt.ToString("x2", CurrentCulture()))
                Next Byt


                Return Result.ToString

            End Using

        End Function

        ''' <summary>
        ''' CDateの代替関数
        ''' </summary>
        ''' <param name="strDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared Function CDateLike(ByVal StrDate As String) As DateTime
            Dim RetDate As DateTime = Nothing

            Try
                RetDate = StrDate

            Catch ex As ApplicationException
                Logger.Error("[IC3040404Response:CDateLike] Not Convert to DateTime:" & StrDate)
            End Try

            Return RetDate

        End Function

    End Class

End Namespace