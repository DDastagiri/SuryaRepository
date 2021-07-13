Imports System.Xml
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.DlrEnvSettingDataSet
Imports Toyota.eCRB.iCROP.BizLogic.IC3040401
Imports Toyota.eCRB.iCROP.BizLogic.CalenderXmlCreateClass.BizLogic
Imports Toyota.eCRB.Common.MainMenu.DataAccess
Imports Toyota.eCRB.Common.MainMenu.DataAccess.SC3010203DataSet


''' <summary>
''' SC3010203(SCメイン)
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3010203BusinessLogic
    Inherits BaseBusinessComponent

#Region " コンストラクタ "
    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
    End Sub
#End Region

#Region " XML用定数 "
    ''' <summary>
    ''' TODO時間指定あり
    ''' </summary>
    Private Const TIMEFLG_TIME As String = "1"
    ''' <summary>
    ''' TODO時間指定なし
    ''' </summary>
    Private Const TIMEFLG_NOTIME As String = "0"
    ''' <summary>
    ''' 遅れ
    ''' </summary>
    Private Const DELAYFLG_DELAY As String = "1"
    ''' <summary>
    ''' 遅れなし
    ''' </summary>
    Private Const DELAYFLG_NODELAY As String = "0"
    ''' <summary>
    ''' データ作成区分-ICROP
    ''' </summary>
    Private Const CREATEDATADIV_ICROP As String = "1"
    ''' <summary>
    ''' データ作成区分-NATIVE
    ''' </summary>
    Private Const CREATEDATADIV_NATIVE As String = "2"
    ''' <summary>
    ''' データ作成区分-遅れ
    ''' </summary>
    Private Const CREATEDATADIV_DELAY As String = "D"
    ''' <summary>
    ''' データ作成区分-完了
    ''' </summary>
    Private Const CREATEDATADIV_COMP As String = "C"
    ''' <summary>
    ''' スケジュール区分-来店
    ''' </summary>
    Private Const SCHEDULEDIV_WALKIN As String = "0"
    ''' <summary>
    ''' スケジュール区分-入庫予約
    ''' </summary>
    Private Const SCHEDULEDIV_SERVICE As String = "1"
    ''' <summary>
    ''' 完了フラグ-未完了
    ''' </summary>
    Private Const COMPFLG_NOCOMP As String = "0"
    ''' <summary>
    ''' 完了フラグ-完了
    ''' </summary>
    Private Const COMPFLG_COMP As String = "1"
    ''' <summary>
    ''' CalDav登録API-区分-追加
    ''' </summary>
    Private Const CALDAV_ACTIONTYPE_ADD As String = "0"
#End Region

#Region " CalDav連携Xml取得・解析処理 "

    ''' <summary>
    ''' スケジュール情報取得
    ''' </summary>
    ''' <returns>XML形式の文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function ReadMySchedule() As String

        Try
            Dim service As New ClassLibraryBusinessLogic
            Dim context As StaffContext = StaffContext.Current      'スタッフ情報
            Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)   '現在日時
            '現在日時より時分秒切捨て
            Dim truncNow As Date = New Date(nowDate.Year, nowDate.Month, nowDate.Day)
            Dim dt As SC3010203TodoColorDataTable

            'スケジュール取得
            Dim xmlText As String = service.GetCalender(truncNow, _
                                                        New Date(truncNow.Year, truncNow.Month, truncNow.Day, 23, 59, 59), _
                                                        context.Account, _
                                                        CType(context.OpeCD, String))
            'パース
            Dim xml As New XmlDocument
            'Logger.Debug("ClassLibraryBusinessLogic.GetCalender value=" & xmlText)
            xml.LoadXml(xmlText)

            '色情報取得
            dt = SC3010203BusinessLogic.GetChipColorInfo()

            'Detailタグ分繰り返し
            For Each detailNode As XmlNode In xml.SelectNodes("Calendar/Detail")

                Dim common As Dictionary(Of String, String)
                'Commonタグ読み出し
                common = SC3010203BusinessLogic.CreateElementsData(detailNode.SelectSingleNode("Common"), _
                                               {"CreateLocation", "ScheduleDiv"})
                'Todoの編集
                'Dim todoList As List(Of Dictionary(Of String, String)) = SC3010203BusinessLogic.EditTodo(dt, xml, context, detailNode, common, nowDate)
                SC3010203BusinessLogic.EditTodo(dt, xml, context, detailNode, common, nowDate)
                'スケジュールの編集
                SC3010203BusinessLogic.EditSchedule(dt, xml, detailNode, common, nowDate)
            Next

            Return xml.OuterXml
        Catch ex As Exception
            Logger.Error(ex.Message, ex)
            Throw
        End Try

    End Function

    ''' <summary>
    ''' チップ背景色を取得します。
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Shared Function GetChipColorInfo() As SC3010203TodoColorDataTable

        Dim dt As SC3010203TodoColorDataTable

        '検索処理
        dt = SC3010203TableAdapter.ReadChipColorSetting()

        '処理結果返却
        Return dt

    End Function

    ''' <summary>
    ''' TODOノードの編集
    ''' </summary>
    ''' <param name="doc">xmlドキュメント</param>
    ''' <param name="detailNode">Detailノード</param>
    ''' <param name="common">Commonタグの内容</param>
    ''' <remarks></remarks>
    Private Shared Sub EditTodo(ByVal dt As SC3010203TodoColorDataTable, _
                         ByVal doc As XmlDocument, _
                         ByVal context As StaffContext, _
                         ByVal detailNode As XmlNode, _
                         ByVal common As Dictionary(Of String, String), _
                         ByVal nowDate As Date)

        Dim todoList As New List(Of Dictionary(Of String, String))

        For Each todoNode As XmlNode In detailNode.SelectNodes("VTodo")

            Dim truncNow As Date = New Date(nowDate.Year, nowDate.Month, nowDate.Day)
            Dim todo As Dictionary(Of String, String)

            'タグ情報読み取り
            todo = SC3010203BusinessLogic.CreateElementsData(todoNode, {"ContactNo", "DtStart", "Due", "XiCropColor", "TimeFlg", "TodoID"})

            '納期を日付型に変換
            Dim due As Date = Date.ParseExact(todo("Due"), "yyyy/MM/dd HH:mm:ss", Nothing)
            Dim truncDue As Date = New Date(due.Year, due.Month, due.Day)

            '開始日時
            Dim dtSt As Nullable(Of Date)
            If (todo("DtStart").Trim.Length <= 0) Then
                '指定なし
                dtSt = Nothing
            Else
                '設定あり
                dtSt = Date.ParseExact(todo("DtStart"), "yyyy/MM/dd HH:mm:ss", Nothing)
            End If

            '時間指定なし or 過去日のTODOは納期のみ表示
            If todo("TimeFlg").Equals(TIMEFLG_NOTIME) Or truncDue < truncNow Then
                dtSt = Nothing
            End If

            '表示用日付タグ作成
            todoNode.AppendChild(SC3010203BusinessLogic.CreateDispDateElement(doc, nowDate, dtSt, due, context.DlrCD, todo("TimeFlg")))
            '遅れフラグタグ作成
            Dim delayElement As XmlElement = SC3010203BusinessLogic.CreateDelayElement(doc, nowDate, due, todo("TimeFlg"))
            todoNode.AppendChild(delayElement)
            '遅れフラグを保存
            todo("Delay") = delayElement.InnerText

            '色・アイコン
            Dim backColor As String = todo("XiCropColor")
            Dim contactNo As Integer
            If todo("ContactNo").Trim.Length > 0 Then
                contactNo = Integer.Parse(todo("ContactNo"), CultureInfo.InvariantCulture)
            End If

            SC3010203BusinessLogic.TodoColorChange(dt, doc, todoNode, common, backColor, contactNo)

            'リストに登録
            todoList.Add(todo)
        Next

    End Sub

    ''' <summary>
    ''' スケジュールノードの編集
    ''' </summary>
    ''' <param name="doc">xmlドキュメント</param>
    ''' <param name="detailNode">Detailノード</param>
    ''' <param name="common">Commonタグの内容</param>
    ''' <remarks></remarks>
    Private Shared Sub EditSchedule(ByVal dt As SC3010203TodoColorDataTable, _
                         ByVal doc As XmlDocument, _
                         ByVal detailNode As XmlNode, _
                         ByVal common As Dictionary(Of String, String), _
                         ByVal nowDate As Date)



        For Each sheduleNode As XmlNode In detailNode.SelectNodes("VEvent")

            Dim truncNow As Date = New Date(nowDate.Year, nowDate.Month, nowDate.Day)
            Dim shedule As Dictionary(Of String, String)

            'タグ情報読み取り
            shedule = SC3010203BusinessLogic.CreateElementsData(sheduleNode, {"ContactNo", "DtStart", "DtEnd", "XiCropColor", "LinkTodoID"})

            '遅れ判定
            Dim delay As String = DELAYFLG_NODELAY

            '遅れエレメントを作成
            Dim delayElement As XmlElement = doc.CreateElement("Delay")
            delayElement.InnerText = delay
            sheduleNode.AppendChild(delayElement)

            '色・アイコン
            Dim backColor As String = shedule("XiCropColor")
            Dim contactNo As Integer
            If shedule("ContactNo").Trim.Length > 0 Then
                contactNo = Integer.Parse(shedule("ContactNo"), CultureInfo.InvariantCulture)
            End If
            SC3010203BusinessLogic.TodoColorChange(dt, doc, sheduleNode, common, backColor, contactNo)


            '開始・終了を設定
            Dim dtStart As Date = Date.ParseExact(shedule("DtStart"), "yyyy/MM/dd HH:mm:ss", Nothing)
            Dim dtEnd As Date = Date.ParseExact(shedule("DtEnd"), "yyyy/MM/dd HH:mm:ss", Nothing)

            '日跨ぎスケジュール考慮(スケジュール時間を今日に収める)
            If truncNow.ToString("yyyyMMdd", CultureInfo.InvariantCulture) > dtStart.ToString("yyyyMMdd", CultureInfo.InvariantCulture) Then
                '前日からのスケジュール
                dtStart = truncNow
                '差し替え
                sheduleNode.SelectSingleNode("DtStart").InnerText = dtStart.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
            End If

            If truncNow.ToString("yyyyMMdd", CultureInfo.InvariantCulture) < dtEnd.ToString("yyyyMMdd", CultureInfo.InvariantCulture) Then
                '翌日までのスケジュール
                dtEnd = New Date(truncNow.Year, truncNow.Month, truncNow.Day, 23, 59, 59)
                '差し替え
                sheduleNode.SelectSingleNode("DtEnd").InnerText = dtEnd.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
            ElseIf truncNow.ToString("yyyyMMdd", CultureInfo.InvariantCulture) > dtEnd.ToString("yyyyMMdd", CultureInfo.InvariantCulture) Then
                dtEnd = New Date(truncNow.Year, truncNow.Month, truncNow.Day, dtEnd.Hour, dtEnd.Minute, dtEnd.Second)
                sheduleNode.SelectSingleNode("DtEnd").InnerText = dtEnd.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
            End If

        Next

    End Sub

    ''' <summary>
    ''' ノード内のタグ情報を取得します。
    ''' </summary>
    ''' <param name="node">ノード</param>
    ''' <param name="tagNames">読み込みを行うタグ名の配列</param>
    ''' <returns>ハッシュ</returns>
    ''' <remarks></remarks>
    Private Shared Function CreateElementsData(ByVal node As XmlNode, ByVal tagNames() As String) As Dictionary(Of String, String)

        Dim dict As New Dictionary(Of String, String)

        '指定タグ名分ループ
        For Each tagName As String In tagNames
            If node.SelectNodes(tagName).Count >= 1 Then
                'タグあり
                dict.Add(tagName, node.SelectSingleNode(tagName).InnerText)
            Else
                'タグなし
                dict.Add(tagName, String.Empty)
            End If
        Next

        '処理結果返却
        Return dict
    End Function

    ''' <summary>
    ''' 色情報取得
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <param name="node"></param>
    ''' <param name="common"></param>
    ''' <param name="backcolor"></param>
    ''' <param name="contactNo"></param>
    ''' <remarks></remarks>
    Private Shared Sub TodoColorChange(ByVal dt As SC3010203TodoColorDataTable, _
                                ByVal doc As XmlDocument, _
                                ByVal node As XmlNode, _
                                ByVal common As Dictionary(Of String, String), _
                                ByVal backcolor As String, _
                                ByVal contactNo As Integer)


        'アイコン
        Dim iconPath As String = String.Empty
        If contactNo > 0 Then
            'LINQ問い合わせd
            Dim rows As IEnumerable(Of SC3010203TodoColorRow) = _
                    From n In dt Where n.CONTACTNO = contactNo Select n
            'アイコンパス格納
            For Each row In rows
                iconPath = row.ICONPATH
            Next
            'アプリケーション相対パスから、クライアントパスに変換
            iconPath = VirtualPathUtility.ToAbsolute(iconPath)
        End If

        'アイコンパスエレメント作成
        Dim iconElement As XmlElement = doc.CreateElement("IconPath")
        iconElement.InnerText = iconPath
        node.AppendChild(iconElement)

        Dim cngBackColor As String = backcolor.Replace("""", "")
        If node.SelectNodes("XiCropColor").Count > 0 Then
            'トリム処理
            node.SelectSingleNode("XiCropColor").InnerText = cngBackColor
        End If

        Dim compFlg As String = COMPFLG_NOCOMP
        If node.SelectNodes("CompFlg").Count > 0 Then
            '完了フラグ取得
            compFlg = node.SelectSingleNode("CompFlg").InnerText
        End If

        Dim scheduleColor As String = String.Empty
        '色指定がある場合は、オリジナル色を別タグに退避 (以降で遅れ、完了の色で上書きするため）
        If node.SelectNodes("XiCropColor").Count > 0 Then
            scheduleColor = node.SelectSingleNode("XiCropColor").InnerText
        End If

        '遅れフラグ
        Dim delay As String = node.SelectSingleNode("Delay").InnerText

        'LINQ問い合わせd
        Dim rowsBack As IEnumerable(Of SC3010203TodoColorRow) = Nothing

        If common("CreateLocation").Equals(CREATEDATADIV_NATIVE) Then
            '個人スケジュール
            rowsBack = From n In dt Where n.CREATEDATADIV = CREATEDATADIV_NATIVE
        ElseIf compFlg.Equals(COMPFLG_COMP) Then
            '完了
            rowsBack = From n In dt Where n.CREATEDATADIV = CREATEDATADIV_COMP
        ElseIf delay.Equals(DELAYFLG_DELAY) Then
            '遅れ
            rowsBack = From n In dt Where n.CREATEDATADIV = CREATEDATADIV_DELAY
        End If

        If rowsBack IsNot Nothing Then

            '背景色取得
            For Each rowBack In rowsBack
                cngBackColor = rowBack.BACKGROUNDCOLOR
                If common("CreateLocation").Equals(CREATEDATADIV_NATIVE) Then
                    '個人スケジュール
                    scheduleColor = rowBack.BACKGROUNDCOLOR
                End If
            Next

            'Todo用背景色設定
            If node.SelectNodes("XiCropColor").Count > 0 Then
                '更新
                node.SelectSingleNode("XiCropColor").InnerText = cngBackColor
            Else
                '追加
                Dim backElement As XmlElement = doc.CreateElement("XiCropColor")
                backElement.InnerText = cngBackColor
                node.AppendChild(backElement)
            End If

        End If

        'スケジュール用背景色設定
        Dim sheduleColorElement As XmlElement = doc.CreateElement("ScheduleColor")
        sheduleColorElement.InnerText = scheduleColor
        node.AppendChild(sheduleColorElement)

    End Sub

    ''' <summary>
    ''' 表示用日付書式を作成
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="fromDate">開始</param>
    ''' <param name="toDate">終了</param>
    ''' <returns>日付文字列エレメント</returns>
    ''' <remarks></remarks>
    Private Shared Function CreateDispDateElement(ByVal doc As XmlDocument, _
                                                  ByVal nowDate As Date, _
                                                  ByVal fromDate As Nullable(Of Date), _
                                                  ByVal toDate As Date, _
                                                  ByVal dlrCd As String, _
                                                  ByVal TimeFlg As String) As XmlElement

        Dim dispText As String

        If fromDate Is Nothing Then
            '納期のみ表示
            If TimeFlg.Equals(TIMEFLG_NOTIME) Then
                '時間指定なし
                dispText = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, toDate, nowDate, dlrCd, False)
            Else
                '時間指定あり
                dispText = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, toDate, nowDate, dlrCd)
            End If

        Else
            'FROM-TOで表示
            Dim sb As New StringBuilder
            sb.Append(DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, fromDate.Value, nowDate, dlrCd))
            sb.Append(WebWordUtility.GetWord(8))
            sb.Append(DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, toDate, nowDate, dlrCd))
            dispText = sb.ToString()
        End If

        '表示用日付ノード作成
        Dim dispDateNode As XmlElement = doc.CreateElement("DispTime")
        dispDateNode.InnerText = dispText

        Return dispDateNode
    End Function

    ''' <summary>
    ''' 遅れフラグエレメントを取得します。
    ''' </summary>
    ''' <param name="doc">XMLドキュメント</param>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="targetDate">対象日時</param>
    ''' <returns>遅れエレメント</returns>
    ''' <remarks></remarks>
    Private Shared Function CreateDelayElement(ByVal doc As XmlDocument, ByVal nowDate As Date, _
                                        ByVal targetDate As Date, ByVal timeFlg As String) As XmlElement

        '遅れフラグ
        Dim delay As String = SC3010203BusinessLogic.CheckDelay(nowDate, targetDate, timeFlg)
        Dim delayElement As XmlElement = doc.CreateElement("Delay")
        delayElement.InnerText = delay

        '処理結果返却
        Return delayElement

    End Function

    ''' <summary>
    ''' 遅れチェック
    ''' </summary>
    ''' <param name="nowDate">現在日時</param>
    ''' <param name="targetDate">対象日時</param>
    ''' <returns>遅れフラグ</returns>
    ''' <remarks></remarks>
    Private Shared Function CheckDelay(ByVal nowDate As Date, ByVal targetDate As Date, ByVal timeFlg As String) As String

        If timeFlg.Equals(TIMEFLG_TIME) Then
            '時間指定あり
            If nowDate > targetDate Then
                '遅れ
                Return DELAYFLG_DELAY
            Else
                'なし
                Return DELAYFLG_NODELAY
            End If
        Else
            '時間指定なし
            Dim truncNow As Date = New Date(nowDate.Year, nowDate.Month, nowDate.Day)
            If truncNow.Subtract(targetDate).TotalDays > 0 Then
                '遅れ
                Return DELAYFLG_DELAY
            Else
                'なし
                Return DELAYFLG_NODELAY
            End If
        End If
    End Function
#End Region

#Region " スケジュール登録API呼び出し "
    ''' <summary>
    ''' スケジュール登録
    ''' </summary>
    ''' <param name="registData"></param>
    ''' <remarks></remarks>
    Public Shared Function RegistMySchedule(ByVal registData As SC3010203CalDavRegistInfoDataTable) As String

        If Not (registData IsNot Nothing AndAlso registData.Count >= 0) Then
            '検証registData
            Throw New ArgumentException("SC3010203BusinessLogic.RegistMySchedule", "registData")
        End If

        Dim context As StaffContext = StaffContext.Current      'スタッフ情報

        Dim dlrEnvSetting As New DealerEnvSetting
        Dim envSettingRow As DLRENVSETTINGRow = dlrEnvSetting.GetEnvSetting(context.DlrCD, "CALDAV_WEBSERVICE_URL")

        '登録情報設定
        Dim row As SC3010203CalDavRegistInfoRow = CType(registData.Rows(0), SC3010203CalDavRegistInfoRow)

        Try
            'サービスエージェントを設定
            Using service As New IC3040401BusinessLogic
                service.CreateCommon()
                service.ActionType = CALDAV_ACTIONTYPE_ADD
                service.DealerCode = row.DLRCD
                service.BranchCode = row.BRNCD
                service.ScheduleId = row.SCHEDULEID
                service.ScheduleDivision = SCHEDULEDIV_WALKIN
                service.ActivityCreateStaffCode = context.Account
                service.CreateScheduleInfo()
                service.TodoId(0) = row.TODOID
                service.StartTime(0) = row.STARTTIME.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
                service.EndTime(0) = row.ENDTIME.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture)
                '実行
                Return service.SendScheduleInfo(envSettingRow.PARAMVALUE)
            End Using
            'Return 0
        Catch ex As Exception
            Logger.Error(ex.Message, ex)
            Throw
        End Try

    End Function
#End Region

#Region " 次画面遷移情報取得 "
    ''' <summary>
    ''' 活動先の顧客情報を取得する。
    ''' </summary>
    ''' <param name="dtParam">引数</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustInfo(ByVal dtParam As SC3010203CustInfoDataTable) As SC3010203CustInfoDataTable

        If Not (dtParam IsNot Nothing AndAlso dtParam.Count >= 0) Then
            '検証エラー
            Throw New ArgumentException("SC3010203BusinessLogic.GetCustInfo", "dtParam")
        End If

        Dim dt As SC3010203CustInfoDataTable

        '検索処理
        dt = SC3010203TableAdapter.GetCustInfo(dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO)

        '処理結果返却
        Return dt

    End Function
#End Region

End Class
