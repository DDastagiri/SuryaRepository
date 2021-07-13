'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'CalenderXmlCreateClassBusinessLogic.vb
'─────────────────────────────────────
'機能： カレンダー情報取得API
'補足： 
'作成： 
'更新： 2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応
'更新： 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV
'更新： 2019/04/24 SKFC 上田 TKM PostUAT-3097
'─────────────────────────────────────

Imports Toyota.eCRB.iCROP.DataAccess.CalenderXmlCreateClass
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Xml

Namespace BizLogic
    Public Class ClassLibraryBusinessLogic
        Implements IDisposable

#Region "定数"

        'ログ出力
        Private Const Log_ProcessStart As String = "Start Prosess GetCalender"
        Private Const Log_ProcessNormalEnd As String = "Normal End Prosess GetCalender"
        Private Const Log_ProcessAbNormalEnd As String = "AbNormal Prosess End GetCalender"

        ' XML宣言
        Private Const Xml_Version As String = "1.0"
        Private Const Xml_Encoding As String = "UTF-8"

        ' 除外日の検索用
        Private Const Date_Encoding As String = "yyyyMMdd"

        '日付書式
        Private Const DateTime_Encoding As String = "yyyy/MM/dd HH:mm:ss"

#End Region
#Region "Public関数"

        ''' <summary>
        ''' カレンダーのXMLを作成します。
        ''' </summary>
        ''' <param name="startTime">検索範囲の開始時間</param>
        ''' <param name="endTime">検索範囲の終了時間</param>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <param name="permission">操作権限コード</param>
        ''' <returns>XML(String型)</returns>
        ''' <remarks></remarks>
        Public Function GetCalender(ByVal startTime As Date, _
                                            ByVal endTime As Date, _
                                            ByVal staffCode As String, _
                                            ByVal permission As String) As String

            Logger.Info(Log_ProcessStart)

            Try

                Dim dataTable As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable

                InputCheck(startTime, endTime, staffCode, permission)

                ' 引数を元にして、DataTableを取得します
                '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                'dataTable = GetElementCalendarXml(startTime, endTime, staffCode, permission)
                dataTable = GetElementCalendarXml(ConstClass.ActionTypeToday, startTime, endTime, Nothing, Nothing, staffCode, permission)
                '2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END

                Dim returnDocString As String = Nothing

                ' DataTableの値を元にして、XMLを作成する
                Dim returnXml As New XmlDocument()

                ' XML宣言をする
                Dim xmlDeclaration As XmlDeclaration = returnXml.CreateXmlDeclaration(Xml_Version, Xml_Encoding, Nothing)
                returnXml.AppendChild(xmlDeclaration)

                ' Calendar要素を追加する
                Dim calendarElement As XmlElement = returnXml.CreateElement(ConstClass.XmlElementCalendar)
                returnXml.AppendChild(calendarElement)

                If Not dataTable.Count = 0 Then

                    ' Detail要素を追加する
                    Logger.Debug("Start Create Detail")
                    ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                    'makeDetailElements(calendarElement, dataTable, startTime, endTime, staffCode, permission)
                    makeDetailElements(calendarElement, dataTable, startTime, endTime, startTime, endTime, staffCode, permission)
                    ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
                    Logger.Debug("End Create Detail")

                Else
                    Logger.Debug("Do Not Found Detail Data")

                End If

                ' XMLの型をXMLDocument型からString型へと変更する
                Dim docElement As XmlElement = returnXml.DocumentElement
                returnDocString = docElement.OuterXml

                dataTable.Dispose()

                Logger.Debug(returnDocString)

                Logger.Info(Log_ProcessNormalEnd)

                Return returnDocString

            Catch ex As ApplicationException
                Logger.Error("ApplicationException Throw:" & ex.Message)
                Logger.Info(Log_ProcessAbNormalEnd)
                Throw

            Catch ex As SystemException
                Logger.Error("SystemException Throw:" & ex.Message)
                Logger.Info(Log_ProcessAbNormalEnd)
                Throw

            End Try

        End Function


        ''' <summary>
        ''' カレンダーのXMLを作成します。(受注後工程データの取得)
        ''' </summary>
        ''' <param name="DeclearCD">検索条件の販売店コード</param>
        ''' <param name="BranchCD">検索条件の店舗コード</param>
        ''' <param name="ScheduleID">検索条件のスケジュールID</param>
        ''' <param name="ScheduleDiv">検索条件のスケジュール区分</param>
        ''' <returns>XML(String型)</returns>
        ''' <remarks></remarks>
        ''' <history>2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>
        Public Function GetOrdersReceived(ByVal DeclearCD As String, _
                                          ByVal BranchCD As String, _
                                          ByVal ScheduleID As String, _
                                          ByVal ScheduleDiv As String) As String

            Logger.Info("Start Prosess GetOrdersReceived")

            Try

                Dim dataTable As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable

                '引数の入力チェック
                InputCheckOrdersReceived(DeclearCD, BranchCD, ScheduleID, ScheduleDiv)

                ' 引数を元にして、DataTableを取得します
                dataTable = GetElementOrdersReceivedXml(DeclearCD, BranchCD, ScheduleID, ScheduleDiv)

                Dim returnDocString As String = Nothing

                ' DataTableの値を元にして、XMLを作成する
                Dim returnXml As New XmlDocument()

                ' XML宣言をする
                Dim xmlDeclaration As XmlDeclaration = returnXml.CreateXmlDeclaration(Xml_Version, Xml_Encoding, Nothing)
                returnXml.AppendChild(xmlDeclaration)

                ' Calendar要素を追加する
                Dim calendarElement As XmlElement = returnXml.CreateElement(ConstClass.XmlElementCalendar)
                returnXml.AppendChild(calendarElement)

                If Not dataTable.Count = 0 Then

                    ' Detail要素を追加する
                    Logger.Debug("Start Create Detail")
                    makeDetailOrdersReceivedElements(calendarElement, dataTable, DeclearCD, BranchCD, ScheduleID, ScheduleDiv)
                    Logger.Debug("End Create Detail")

                Else
                    Logger.Debug("Do Not Found Detail Data")

                End If

                ' XMLの型をXMLDocument型からString型へと変更する
                Dim docElement As XmlElement = returnXml.DocumentElement
                returnDocString = docElement.OuterXml

                dataTable.Dispose()

                Logger.Debug(returnDocString)

                Logger.Info("Normal End Prosess GetOrdersReceived")

                Return returnDocString

            Catch ex As ApplicationException
                Logger.Error("ApplicationException Throw:" & ex.Message)
                Logger.Info("AbNormal Prosess End  GetOrdersReceived")
                Throw

            Catch ex As SystemException
                Logger.Error("SystemException Throw:" & ex.Message)
                Logger.Info("AbNormal Prosess End GetOrdersReceived")
                Throw

            End Try

        End Function

        ''' <summary>
        ''' カレンダー日付のXMLを作成します。
        ''' </summary>
        ''' <param name="actiontype">処理区分(1:当日情報取得、2：過去情報取得、3：未来情報取得)</param>
        ''' <param name="startTime1">検索範囲の開始日時1</param>
        ''' <param name="endTime1">検索範囲の終了日時1</param>
        ''' <param name="startTime2">検索範囲の開始日時2</param>
        ''' <param name="endTime2">検索範囲の終了日時2</param>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <param name="permission">操作権限コード</param>
        ''' <returns>XML(String型)</returns>
        ''' <history>2014/04/25 SKFC 上田 NEXTSTEP_CALDAV 追加</history>
        ''' <remarks></remarks>
        Public Function GetDayCalender(ByVal actionType As String, _
                                    ByVal startTime1 As Date, _
                                    ByVal endTIme1 As Date, _
                                    ByVal startTime2 As Date, _
                                    ByVal endTime2 As Date, _
                                    ByVal staffCode As String, _
                                    ByVal permission As String) As String

            Logger.Info("Start Prosess GetDayCalender")
            Logger.Debug("actionType=" & actionType & _
                         ", startTime1=" & startTime1.ToString & _
                         ", endTime1=" & endTIme1.ToString & _
                         ", startTime2=" & startTime2.ToString & _
                         ", endTime2=" & endTime2.ToString & _
                         ", staffCode=" & staffCode & _
                         ", permission=" & permission)
            Try

                Dim dataTable As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable

                InputCheckDateCalender(actiontype, startTime1, endTIme1, startTime2, endTime2, staffCode, permission)

                ' 引数を元にして、DataTableを取得します
                dataTable = GetElementCalendarXml(actiontype, startTime1, endTIme1, startTime2, endTime2, staffCode, permission)

                Dim returnDocString As String = Nothing

                ' DataTableの値を元にして、XMLを作成する
                Dim returnXml As New XmlDocument()

                ' XML宣言をする
                Dim xmlDeclaration As XmlDeclaration = returnXml.CreateXmlDeclaration(Xml_Version, Xml_Encoding, Nothing)
                returnXml.AppendChild(xmlDeclaration)

                ' Calendar要素を追加する
                Dim calendarElement As XmlElement = returnXml.CreateElement(ConstClass.XmlElementCalendar)
                returnXml.AppendChild(calendarElement)

                If Not dataTable.Count = 0 Then

                    ' Detail要素を追加する
                    Logger.Debug("Start Create Detail")
                    If Validation.Equals(actiontype, ConstClass.ActionTypeToday) Then
                        makeDetailElements(calendarElement, dataTable, startTime1, endTIme1, startTime1, endTIme1, staffCode, permission)
                    Else
                        makeDetailElements(calendarElement, dataTable, startTime2, endTime2, startTime1, endTIme1, staffCode, permission)
                    End If
                    Logger.Debug("End Create Detail")

                Else
                    Logger.Debug("Do Not Found Detail Data")

                End If

                ' XMLの型をXMLDocument型からString型へと変更する
                Dim docElement As XmlElement = returnXml.DocumentElement
                returnDocString = docElement.OuterXml

                dataTable.Dispose()

                Logger.Debug(returnDocString)

                Logger.Info("Normal End Prosess GetDayCalender")

                Return returnDocString

            Catch ex As ApplicationException
                Logger.Error("ApplicationException Throw:" & ex.Message)
                Logger.Info("AbNormal Prosess End  GetDayCalender")
                Throw

            Catch ex As SystemException
                Logger.Error("SystemException Throw:" & ex.Message)
                Logger.Info("AbNormal Prosess End  GetDayCalender")
                Throw

            End Try

        End Function

        ''' <summary>
        ''' Disposeメソッド
        ''' </summary>
        ''' <remarks></remarks>
        Public Overloads Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
        Protected Overridable Overloads Sub Dispose(ByVal disposing As Boolean)

            If disposing Then

            End If

        End Sub

#End Region
#Region "Private関数"


        ''' <summary>
        ''' 入力チェックを行います
        ''' </summary>
        ''' <param name="startTime">検索範囲の開始時間</param>
        ''' <param name="endTime">検索範囲の終了時間</param>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <param name="permission">操作権限コード</param>
        ''' <remarks></remarks>
        Private Sub InputCheck(ByVal startTime As String, _
                                ByVal endTime As String, _
                                ByVal staffCode As String, _
                                ByVal permission As String)

            ' 終了日付が開始日付より小さい
            If Not IsCheckDate(startTime, endTime) Then

                ' Exceptionを設定します。
                Logger.Error(ErrorCode.SetTimeError)
                Throw New ApplicationException(ErrorCode.SetTimeError)

            End If

            ' スタッフコードが設定されているかチェックします
            If Validation.Equals(staffCode, Nothing) Then

                ' Exceptionを設定します。
                Logger.Error(ErrorCode.NotStaffCode)
                Throw New ApplicationException(ErrorCode.NotStaffCode)

            End If

            ' 操作権限コードが設定されているかチェックします
            If Validation.Equals(permission, Nothing) Then

                ' Exceptionを設定します。
                Logger.Error(ErrorCode.NotPermission)
                Throw New ApplicationException(ErrorCode.NotPermission)

            End If

        End Sub

        ''' <summary>
        ''' 入力チェックを行います(受注後工程用)
        ''' </summary>
        ''' <param name="DeclearCD">検索条件の販売店コード</param>
        ''' <param name="BranchCD">検索条件の店舗コード</param>
        ''' <param name="ScheduleID">検索条件のスケジュールID</param>
        ''' <param name="ScheduleDiv">検索条件のスケジュール区分</param>
        ''' <remarks></remarks>
        ''' <history>2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>
        Private Sub InputCheckOrdersReceived(ByVal DeclearCD As String, _
                                            ByVal BranchCD As String, _
                                            ByVal ScheduleID As String, _
                                            ByVal ScheduleDiv As String)

            ' 販売店コードが設定されているかチェックします
            If Validation.Equals(DeclearCD, Nothing) Then

                ' Exceptionを設定します。
                Logger.Error(ErrorCode.NotDealerCode)
                Throw New ApplicationException(ErrorCode.NotDealerCode)

            End If

            ' 店舗コードが設定されているかチェックします
            If Validation.Equals(BranchCD, Nothing) Then

                ' Exceptionを設定します。
                Logger.Error(ErrorCode.NotBranchCode)
                Throw New ApplicationException(ErrorCode.NotBranchCode)

            End If

            ' スケジュールIDが設定されているかチェックします
            If Validation.Equals(ScheduleID, Nothing) Then

                ' Exceptionを設定します。
                Logger.Error(ErrorCode.NotScheduleID)
                Throw New ApplicationException(ErrorCode.NotScheduleID)

            End If

            ' スケジュール区分が設定されているかチェックします
            If Validation.Equals(ScheduleDiv, Nothing) Then

                ' Exceptionを設定します。
                Logger.Error(ErrorCode.NotScheduleDiv)
                Throw New ApplicationException(ErrorCode.NotScheduleDiv)

            End If

        End Sub

        ''' <summary>
        ''' 入力チェックを行います(カレンダー日付情報用)
        ''' </summary>
        ''' <param name="Actiontype">処理区分(1：当日情報取得、2：過去情報取得、3：未来情報取得</param>
        ''' <param name="startTime1">検索範囲の開始日時1</param>
        ''' <param name="endTime1">検索範囲の終了日時1</param>
        ''' <param name="startTime2">検索範囲の開始日時2</param>
        ''' <param name="endTime2">検索範囲の終了日時2</param>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <param name="permission">操作権限コード</param>
        ''' <remarks></remarks>
        Private Sub InputCheckDateCalender(ByVal ActionType As String, _
                                           ByVal startTime1 As Date, _
                                           ByVal endTime1 As Date, _
                                           ByVal startTime2 As Date, _
                                           ByVal endTime2 As Date, _
                                           ByVal staffCode As String, _
                                           ByVal permission As String)


            '処理区分が正しい値がチェックします
            If Not Validation.Equals(ActionType, ConstClass.ActionTypeToday) And _
                Not Validation.Equals(ActionType, ConstClass.ActionTypeDone) And _
                Not Validation.Equals(ActionType, ConstClass.ActionTypeFuture) Then

                ' Exceptionを設定します。
                Logger.Error(ErrorCode.ActionTypeError)
                Throw New ApplicationException(ErrorCode.ActionTypeError)

            End If

            ' 終了日付が開始日付より小さい
            If Not IsCheckDate(startTime1, endTime1) Then

                ' Exceptionを設定します。
                Logger.Error(ErrorCode.SetTimeError)
                Throw New ApplicationException(ErrorCode.SetTimeError)

            End If

            ' 過去、未来の場合
            If Validation.Equals(ActionType, ConstClass.ActionTypeDone) Or _
                Validation.Equals(ActionType, ConstClass.ActionTypeFuture) Then

                ' 終了日付2が開始日付2より小さい
                If Not IsCheckDate(startTime2, endTime2) Then

                    ' Exceptionを設定します。
                    Logger.Error(ErrorCode.SetTimeError2)
                    Throw New ApplicationException(ErrorCode.SetTimeError2)

                End If
            End If

            ' スタッフコードが設定されているかチェックします
            If Validation.Equals(staffCode, Nothing) Then

                ' Exceptionを設定します。
                Logger.Error(ErrorCode.NotStaffCode)
                Throw New ApplicationException(ErrorCode.NotStaffCode)

            End If

            ' 操作権限コードが設定されているかチェックします
            If Validation.Equals(permission, Nothing) Then

                ' Exceptionを設定します。
                Logger.Error(ErrorCode.NotPermission)
                Throw New ApplicationException(ErrorCode.NotPermission)

            End If

        End Sub

        ''' <summary>
        ''' カレンダー要素内を作成する値を取得します。
        ''' </summary>
        ''' <param name="Actiontype">処理区分(1:当日情報取得、2：過去情報取得、3：未来情報取得)</param>
        ''' <param name="startTime">検索範囲の開始時間</param>
        ''' <param name="endTime">検索範囲の終了時間</param>
        ''' <param name="startTime2">検索範囲の開始時間</param>
        ''' <param name="endTime2">検索範囲の終了時間</param>
        ''' <param name="staffCode">スタッフ名</param>
        ''' <param name="permission">権限</param>
        ''' <returns>条件で取得された値</returns>
        ''' <remarks></remarks>
        ''' <history>2014/04/25 SKFC 上田 NEXTSTEP_CALDAV 引数変更</history>
        <EnableCommit()>
        Private Function GetElementCalendarXml(ByVal actionType As String, _
                                               ByVal startTime As Date, _
                                               ByVal endTime As Date, _
                                               ByVal startTime2 As Date, _
                                               ByVal endTime2 As Date, _
                                               ByVal staffCode As String, _
                                               ByVal permission As String) As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable
            'Private Function GetElementCalendarXml(ByVal startTime As Date, _
            '                                    ByVal endTime As Date, _
            '                                    ByVal staffCode As String, _
            '                                    ByVal permission As String) As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable

            Using adapter As New DataAccess.CalenderXmlCreateClass.DataAccess.SelectCreateCalendarDataTable

                ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                'Return adapter.GetSelectCalendarTable(startTime, endTime, staffCode, permission)
                Return adapter.GetSelectCalendarTable(actionType, startTime, endTime, startTime2, endTime2, staffCode, permission)
                ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
            End Using

        End Function

        ''' <summary>
        ''' カレンダー要素内を作成する値を取得します。(受注後工程)
        ''' </summary>
        ''' <param name="DeclearCD">検索条件の販売店コード</param>
        ''' <param name="BranchCD">検索条件の店舗コード</param>
        ''' <param name="ScheduleID">検索条件のスケジュールID</param>
        ''' <param name="ScheduleDiv">検索条件のスケジュール区分</param>
        ''' <remarks></remarks>
        ''' <history>2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>
        <EnableCommit()>
        Private Function GetElementOrdersReceivedXml(ByVal DeclearCD As String, _
                                            ByVal BranchCD As String, _
                                            ByVal ScheduleID As String, _
                                            ByVal ScheduleDiv As String) As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable

            Using adapter As New DataAccess.CalenderXmlCreateClass.DataAccess.SelectCreateCalendarDataTable


                Return adapter.GetSelectOrdersReceivedTable(DeclearCD, BranchCD, ScheduleID, ScheduleDiv)

            End Using

        End Function

        ''' <summary>
        ''' TODOの繰り返し除外日を取得する
        ''' </summary>
        ''' <param name="startTime">検索範囲の開始時間</param>
        ''' <param name="endTime">検索範囲の終了時間</param>
        ''' <param name="staffCode">スタッフ名</param>
        ''' <param name="permission">権限</param>
        ''' <returns>条件で取得された値</returns>
        ''' <remarks></remarks>
        <EnableCommit()>
        Private Function GetTodoExDate(ByVal startTime As Date, _
                                       ByVal endTime As Date, _
                                       ByVal staffCode As String, _
                                       ByVal permission As String) As CalenderXmlCreateClassDataSet.ExDateTableDataTable

            Using adapter As New DataAccess.CalenderXmlCreateClass.DataAccess.ExDateTable

                ' DataAcsessに接続
                Return adapter.GetTodoExDate(startTime, endTime, staffCode, permission)

            End Using

        End Function

        ''' <summary>
        ''' Eventの繰り返し除外日を取得する
        ''' </summary>
        ''' <param name="startTime">検索範囲の開始時間</param>
        ''' <param name="endTime">検索範囲の終了時間</param>
        ''' <param name="staffCode">スタッフ名</param>
        ''' <param name="permission">権限</param>
        ''' <returns>条件で取得された値</returns>
        ''' <remarks></remarks>
        <EnableCommit()>
        Private Function GetEventExDate(ByVal startTime As Date, _
                                        ByVal endTime As Date, _
                                        ByVal staffCode As String, _
                                        ByVal permission As String) As CalenderXmlCreateClassDataSet.ExDateTableDataTable

            Using adapter As New DataAccess.CalenderXmlCreateClass.DataAccess.ExDateTable

                ' DataAcsessに接続
                Return adapter.GetEventExDate(startTime, endTime, staffCode, permission)

            End Using

        End Function


        ''' <summary>
        ''' Detail要素を作成します
        ''' </summary>
        ''' <param name="calendarElement">カレンダー要素のXmlElement</param>
        ''' <param name="dataTable">要素を作成するDataTable</param>
        ''' <param name="todoStartTime">Todoの開始時間</param>
        ''' <param name="todoEndTime">Todoの終了時間</param>
        ''' <param name="eventStartTime">Eventの開始時間</param>
        ''' <param name="eventEndTime">Eventの終了時間</param>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <param name="permission">操作権限コード</param>
        ''' <remarks></remarks>
        ''' <history>2014/04/25 SKFC 上田 NEXTSTEP_CALDAV 引数変更</history>
        Private Sub makeDetailElements(ByVal calendarElement As XmlElement, _
                                            ByVal dataTable As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable, _
                                            ByVal todoStartTime As Date, _
                                            ByVal todoEndTime As Date, _
                                            ByVal eventStartTime As Date, _
                                            ByVal eventEndTime As Date, _
                                            ByVal staffCode As String, _
                                            ByVal permission As String)
            'Private Sub makeDetailElements(ByVal calendarElement As XmlElement, _
            '                                    ByVal dataTable As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable, _
            '                                    ByVal startTime As Date, _
            '                                    ByVal endTime As Date, _
            '                                    ByVal staffCode As String, _
            '                                    ByVal permission As String)


            ' 各要素を作成するフラグ
            Dim isAlart As Boolean = False
            Dim isCommon As Boolean = False
            Dim isScheduleInfo As Boolean = False
            Dim detailFlgCalenderId As String = Nothing

            Dim detailElement As XmlElement = Nothing

            'アラーム専用要素を作成する。
            Dim alarmList As List(Of Integer) = New List(Of Integer)

            ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
            '' TODOの繰り返し除外日を取得する
            'Dim todoExDateDataTable As CalenderXmlCreateClassDataSet.ExDateTableDataTable = GetTodoExDate(startTime, endTime, staffCode, permission)

            '' Eventの繰り返し除外日を取得する
            'Dim eventExDateDataTable As CalenderXmlCreateClassDataSet.ExDateTableDataTable = GetEventExDate(startTime, endTime, staffCode, permission)

            ' TODOの繰り返し除外日を取得する
            Dim todoExDateDataTable As CalenderXmlCreateClassDataSet.ExDateTableDataTable = GetTodoExDate(todoStartTime, todoEndTime, staffCode, permission)

            ' Eventの繰り返し除外日を取得する
            Dim eventExDateDataTable As CalenderXmlCreateClassDataSet.ExDateTableDataTable = GetEventExDate(eventStartTime, eventEndTime, staffCode, permission)
            ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END

            For i As Integer = 0 To dataTable.Count - 1

                isAlart = False
                Dim makeVtodoVeventFlg As Boolean = False

                ' 現在行を取得する
                Dim nowDataRow As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableRow = dataTable.Rows(i)

                '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 START
                ' アラームがある場合、アラームの要素を先に作成します
                If nowDataRow.ALARM_TRIGGER IsNot Nothing Then
                    alarmList.Add(nowDataRow.ALARM_TRIGGER)
                End If
                '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 END

                ' 最終行でなければ、以下の処理
                If Not (dataTable.Count - 1 = i) Then

                    ' 次の行を取得する
                    Dim nextDataRow As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableRow = dataTable.Rows(i + 1)

                    '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 DEL START
                    '' アラームがある場合、アラームの要素を先に作成します
                    'If nowDataRow.ALARM_TRIGGER IsNot Nothing Then

                    '    alarmList.Add(nowDataRow.ALARM_TRIGGER)

                    'End If
                    '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 DEL END

                    ' 次の行が全く同じデータの場合、今回の行の処理は行いません
                    If (Validation.Equals(nowDataRow.CARENDAR_ID, nextDataRow.CARENDAR_ID) AndAlso
                            Validation.Equals(nowDataRow.TODO_ID, nextDataRow.TODO_ID) AndAlso
                                Validation.Equals(nowDataRow.EVENT_ID, nextDataRow.EVENT_ID) AndAlso
                                    Validation.Equals(nowDataRow.TODOEVENT_FLG, nextDataRow.TODOEVENT_FLG)) Then

                        isAlart = True

                    End If
                End If

                ' 各要素を作成するフラグをOFFにします
                isCommon = False
                isScheduleInfo = False

                ' カレンダーＩＤの値が違う場合、Detailを作成します
                If Not Validation.Equals(nowDataRow.CARENDAR_ID, detailFlgCalenderId) Then

                    ' Detail作成フラグ用のカレンダーIDの値を現在の値に変更する
                    detailFlgCalenderId = nowDataRow.CARENDAR_ID

                    detailElement = calendarElement.OwnerDocument.CreateElement(ConstClass.XmlElementDetail)

                    ' Common,ScheduleInfoの作成フラグをonにします。
                    isCommon = True

                    ' カレンダーIDがNothingでなければ、ScheduleInfo要素を作成する
                    If nowDataRow.CARENDAR_ID IsNot Nothing Then

                        isScheduleInfo = True

                    End If

                End If

                If isCommon = True Then

                    ' Common要素内作成メソッドを呼び出す
                    makeCommonElements(detailElement, dataTable.Rows(i))

                End If


                If isScheduleInfo = True Then

                    ' ScheduleInfo要素内作成メソッドを呼び出す
                    makeScheduleInfoElements(detailElement, dataTable.Rows(i))

                End If

                ' VTodo要素処理、Rルール処理
                If nowDataRow.TODOEVENT_FLG = 1 And Not isAlart Then

                    ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                    'makeVtodoVeventFlg = CheckRrule(nowDataRow, startTime, endTime, todoExDateDataTable, detailElement, alarmList)
                    makeVtodoVeventFlg = CheckRrule(nowDataRow, todoStartTime, todoEndTime, todoExDateDataTable, detailElement, alarmList)
                    ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
                ElseIf nowDataRow.TODOEVENT_FLG = 2 And Not isAlart Then

                    ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
                    'makeVtodoVeventFlg = CheckRrule(nowDataRow, startTime, endTime, eventExDateDataTable, detailElement, alarmList)
                    makeVtodoVeventFlg = CheckRrule(nowDataRow, eventStartTime, eventEndTime, eventExDateDataTable, detailElement, alarmList)
                    ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END

                End If

                ' VTODOないしVEVENTが作成された場合は、Detail要素をカレンダー要素に格納する
                If makeVtodoVeventFlg = True Then

                    calendarElement.AppendChild(detailElement)
                    alarmList = New List(Of Integer)
                End If

            Next i

        End Sub

        ''' <summary>
        ''' カレンダーのXMLを作成します。
        ''' </summary>
        ''' <param name="calendarElement">カレンダー情報エレメント</param>
        ''' <param name="dataTable">データテーブル</param>
        ''' <param name="DeclearCD">検索条件の販売店コード</param>
        ''' <param name="BranchCD">検索条件の店舗コード</param>
        ''' <param name="ScheduleID">検索条件のスケジュールID</param>
        ''' <param name="ScheduleDiv">検索条件のスケジュール区分</param>
        ''' <remarks></remarks>
        ''' <history>2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>
        ''' <history>2012/03/09 SKFC 加藤 【SALES_2】DATE型→STRING型</history>
        Private Sub makeDetailOrdersReceivedElements(ByVal calendarElement As XmlElement, _
                                            ByVal dataTable As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable, _
                                            ByVal DeclearCD As String, _
                                            ByVal BranchCD As String, _
                                            ByVal ScheduleID As String, _
                                            ByVal ScheduleDiv As String)
            'Private Sub makeDetailOrdersReceivedElements(ByVal calendarElement As XmlElement, _
            '                                    ByVal dataTable As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableDataTable, _
            '                                    ByVal DeclearCD As Date, _
            '                                    ByVal BranchCD As Date, _
            '                                    ByVal ScheduleID As String, _
            '                                    ByVal ScheduleDiv As String)


            ' 各要素を作成するフラグ
            Dim isAlart As Boolean = False
            Dim isCommon As Boolean = False
            Dim isScheduleInfo As Boolean = False
            Dim detailFlgCalenderId As String = Nothing

            Dim detailElement As XmlElement = Nothing

            'アラーム専用要素を作成する。
            Dim alarmList As List(Of Integer) = New List(Of Integer)

            For i As Integer = 0 To dataTable.Count - 1

                isAlart = False
                Dim makeVtodoVeventFlg As Boolean = False

                ' 現在行を取得する
                Dim nowDataRow As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableRow = dataTable.Rows(i)

                '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 START
                ' アラームがある場合、アラームの要素を先に作成します
                If nowDataRow.ALARM_TRIGGER IsNot Nothing Then
                    alarmList.Add(nowDataRow.ALARM_TRIGGER)
                End If
                '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 END

                ' 最終行でなければ、以下の処理
                If Not (dataTable.Count - 1 = i) Then

                    ' 次の行を取得する
                    Dim nextDataRow As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableRow = dataTable.Rows(i + 1)

                    '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 DEL START
                    ' アラームがある場合、アラームの要素を先に作成します
                    'If nowDataRow.ALARM_TRIGGER IsNot Nothing Then

                    '    alarmList.Add(nowDataRow.ALARM_TRIGGER)

                    'End If
                    '2012/04/04 SKFC 上田 【SALES_2】受注後工程の対応 DEL END

                    ' 次の行が全く同じデータの場合、今回の行の処理は行いません
                    If (Validation.Equals(nowDataRow.CARENDAR_ID, nextDataRow.CARENDAR_ID) AndAlso
                            Validation.Equals(nowDataRow.TODO_ID, nextDataRow.TODO_ID) AndAlso
                                Validation.Equals(nowDataRow.EVENT_ID, nextDataRow.EVENT_ID) AndAlso
                                    Validation.Equals(nowDataRow.TODOEVENT_FLG, nextDataRow.TODOEVENT_FLG)) Then

                        isAlart = True

                    End If
                End If

                ' 各要素を作成するフラグをOFFにします
                isCommon = False
                isScheduleInfo = False

                ' カレンダーＩＤの値が違う場合、Detailを作成します
                If Not Validation.Equals(nowDataRow.CARENDAR_ID, detailFlgCalenderId) Then

                    ' Detail作成フラグ用のカレンダーIDの値を現在の値に変更する
                    detailFlgCalenderId = nowDataRow.CARENDAR_ID

                    detailElement = calendarElement.OwnerDocument.CreateElement(ConstClass.XmlElementDetail)

                    ' Common,ScheduleInfoの作成フラグをonにします。
                    isCommon = True

                    ' カレンダーIDがNothingでなければ、ScheduleInfo要素を作成する
                    If nowDataRow.CARENDAR_ID IsNot Nothing Then

                        isScheduleInfo = True

                    End If

                End If

                If isCommon = True Then

                    ' Common要素内作成メソッドを呼び出す
                    makeCommonElements(detailElement, dataTable.Rows(i))

                End If


                If isScheduleInfo = True Then

                    ' ScheduleInfo要素内作成メソッドを呼び出す
                    makeScheduleInfoElements(detailElement, dataTable.Rows(i))

                End If

                ' VTodo要素処理、Rルール処理
                If nowDataRow.TODOEVENT_FLG = 1 And Not isAlart Then

                    makeVtodoVeventFlg = CheckRruleOrdersReceived(nowDataRow, detailElement, alarmList)
                ElseIf nowDataRow.TODOEVENT_FLG = 2 And Not isAlart Then

                    makeVtodoVeventFlg = CheckRruleOrdersReceived(nowDataRow, detailElement, alarmList)

                End If

                ' VTODOないしVEVENTが作成された場合は、Detail要素をカレンダー要素に格納する
                If makeVtodoVeventFlg = True Then

                    calendarElement.AppendChild(detailElement)
                    alarmList = New List(Of Integer)
                End If

            Next i

        End Sub


        ''' <summary>
        ''' Common要素を作成します
        ''' </summary>
        ''' <param name="detailElements">Detail要素のXML</param>
        ''' <param name="dataRow">DataRow</param>
        ''' <returns>要素の値</returns>
        ''' <remarks></remarks>
        Private Function makeCommonElements(ByVal detailElements As XmlElement, ByVal dataRow As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableRow) As XmlElement

            Dim commonElement As XmlElement = detailElements.OwnerDocument.CreateElement(ConstClass.XmlElementCommon)
            detailElements.AppendChild(commonElement)

            ' Common要素の中の要素を作成します。
            ' カレンダーIDが存在しなければ、
            If Validation.Equals(dataRow.CARENDAR_ID, ConstClass.SqlCalendarIdNative) Then
                ' CreateLocation要素の値をNativeに設定します。
                setElementValue(commonElement, ConstClass.XmlElementCreateLocation, ConstClass.XmlCreateLocationNative)

            Else
                ' CreateLocation要素の値をICropに設定します。
                setElementValue(commonElement, ConstClass.XmlElementCreateLocation, ConstClass.XmlCreateLocationIcrop)

            End If

            setElementValue(commonElement, ConstClass.XmlElementDealerCode, dataRow.DEALER_CODE)
            setElementValue(commonElement, ConstClass.XmlElementBranchCode, dataRow.BRANCH_CODE)
            setElementValue(commonElement, ConstClass.XmlElementScheduleId, dataRow.SCHEDULE_ID)
            setElementValue(commonElement, ConstClass.XmlElementScheduleDiv, dataRow.SCHEDULE_DIV)

            Return detailElements

        End Function

        ''' <summary>
        ''' ScheduleInfo要素を作成します
        ''' </summary>
        ''' <param name="detailElements">Detail要素のXML</param>
        ''' <param name="dataRow">DataRow</param>
        ''' <returns>要素の値</returns>
        ''' <remarks></remarks>
        Private Function makeScheduleInfoElements(ByVal detailElements As XmlElement, ByVal dataRow As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableRow) As XmlElement

            Dim scheduleInfoElement As XmlElement = detailElements.OwnerDocument.CreateElement(ConstClass.XmlElementScheduleInfo)
            detailElements.AppendChild(scheduleInfoElement)

            ' ScheduleInfo要素の中の要素を作成します。
            setElementValue(scheduleInfoElement, ConstClass.XmlElementCustomerDiv, dataRow.CUSTOMER_DIV)
            setElementValue(scheduleInfoElement, ConstClass.XmlElementCustomerCode, dataRow.CUSTOMER_CODE)
            setElementValue(scheduleInfoElement, ConstClass.XmlElementDmsId, dataRow.DMS_ID)
            setElementValue(scheduleInfoElement, ConstClass.XmlElementCustomerName, dataRow.CUSTOMER_NAME)
            setElementValue(scheduleInfoElement, ConstClass.XmlElementReceptionDiv, dataRow.RECEPTION_DIV)

            Return detailElements

        End Function

        ''' <summary>
        ''' Rルール処理を行います
        ''' </summary>
        ''' <param name="dataRow">判別するDataRow</param>
        ''' <param name="startTime">開始時間</param>
        ''' <param name="endTime">終了時間</param>
        ''' <param name="exDateDataTable">繰り返し除外日が入ったDataTable</param>
        ''' <remarks></remarks>
        Private Function CheckRrule(ByVal dataRow As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableRow, _
                                            ByVal startTime As Date, _
                                            ByVal endTime As Date, _
                                            ByVal exDateDataTable As CalenderXmlCreateClassDataSet.ExDateTableDataTable, _
                                            ByVal detailElement As XmlElement, _
                                            ByVal alarmList As List(Of Integer)) As Boolean

            ' VTODO，VEVENT要素を作成したかどうか
            Dim makeVtodoVeventFlg As Boolean = False

            ' Rルールを行うか判別する
            Select Case dataRow.R_RULE_FLG

                Case ConstClass.RRuleFlgOff
                    ' R-Rule処理を行わない場合
                    If dataRow.TODOEVENT_FLG = ConstClass.TodoEventFlgTodo Then

                        ' VTodo要素内作成メソッドを呼び出す
                        makeVTodoElements(detailElement, dataRow, alarmList)
                        makeVtodoVeventFlg = True
                    ElseIf dataRow.TODOEVENT_FLG = ConstClass.TodoEventFlgEvent Then

                        ' VEvent要素内作成メソッドを呼び出す
                        makeVEventElements(detailElement, dataRow, alarmList)
                        makeVtodoVeventFlg = True
                    End If

                Case Else
                    ' Rルール処理を行う場合

                    ' R-ruleの繰り返し周期が日付ごとの場合
                    Dim startDate As Date = DateTimeFunc.FormatString(DateTime_Encoding, dataRow.START_TIME)
                    Dim endDate As Date = DateTimeFunc.FormatString(DateTime_Encoding, dataRow.END_TIME)

                    'インターバルが0以下の場合、エラー
                    If dataRow.R_RULE_INTERVAL Is Nothing Then
                        Logger.Info("Wrong R_RULE_INTERVAL IS Nothing")

                    ElseIf CType(dataRow.R_RULE_INTERVAL, Integer) < 0 Then
                        Logger.Info("Wrong R_RULE_INTERVAL [" & dataRow.R_RULE_INTERVAL & "]")

                    Else
                        ' 開始時間がチェック終了日、又はR-ruleの終了日を超えるまで、ループします。
                        'While isCheckDate(startDate, endTime) And isCheckDate(endTime, dataRow.R_RULE_UNTIL)
                        While IsCheckDate(startDate, endTime) And IsCheckDate(startDate, dataRow.R_RULE_UNTIL)
                            Logger.Debug("StartDate" & CType(startDate, Date))
                            Logger.Debug("endDate" & CType(endDate, Date))
                            Logger.Debug("R_RULE_UNTIL" & dataRow.R_RULE_UNTIL)

                            Dim startDays As Integer = startDate.Day
                            Dim intervalTime As Integer = DateDiff(DateInterval.Second, startDate, endDate)

                            ' 検索条件に合致した場合、除外日検索をします
                            If IsIncludeDate(startTime, endTime, startDate, endDate) Then

                                ' 除外日でなければ、データを格納します。
                                If dataRow.TODOEVENT_FLG = ConstClass.TodoEventFlgTodo AndAlso IsBrackDates(startDate, dataRow.TODO_ID, exDateDataTable) Then

                                ElseIf dataRow.TODOEVENT_FLG = ConstClass.TodoEventFlgEvent AndAlso IsBrackDates(startDate, dataRow.EVENT_ID, exDateDataTable) Then

                                Else

                                    dataRow.START_TIME = Format(startDate, DateTime_Encoding)
                                    dataRow.END_TIME = Format(endDate, DateTime_Encoding)

                                    ' 開始時間、終了時間を入れ替えた値のものをXMLで作成する
                                    If dataRow.TODOEVENT_FLG = ConstClass.TodoEventFlgTodo Then

                                        ' VTodo要素内作成メソッドを呼び出す
                                        makeVTodoElements(detailElement, dataRow, alarmList)
                                        makeVtodoVeventFlg = True

                                    ElseIf dataRow.TODOEVENT_FLG = ConstClass.TodoEventFlgEvent Then

                                        ' VEvent要素内作成メソッドを呼び出す
                                        makeVEventElements(detailElement, dataRow, alarmList)
                                        makeVtodoVeventFlg = True

                                    End If

                                End If

                            End If

                            Dim isCommonIncrementFlg As Boolean = False
                            Dim addStartTime As Date

                            ' 次の値（Ｒルール分進めた日付）を設定します。
                            Select Case dataRow.R_RULE_FREQ

                                Case ConstClass.RRuleFreqDaily
                                    ' 日毎にチェック

                                    startDate = DateAdd(DateInterval.Day, CType(dataRow.R_RULE_INTERVAL, Integer), startDate)
                                    endDate = DateAdd(DateInterval.Day, CType(dataRow.R_RULE_INTERVAL, Integer), endDate)

                                Case ConstClass.RRuleFreqWeekly
                                    ' 週毎にチェック

                                    startDate = DateAdd(DateInterval.WeekOfYear, CType(dataRow.R_RULE_INTERVAL, Integer), startDate)
                                    endDate = DateAdd(DateInterval.WeekOfYear, CType(dataRow.R_RULE_INTERVAL, Integer), endDate)

                                Case ConstClass.RRuleFreqMonthly
                                    ' 月毎にチェック
                                    Dim monthCount As Integer = 0

                                    While isCommonIncrementFlg = False

                                        monthCount = monthCount + 1

                                        addStartTime = DateAdd(DateInterval.Month, CType(dataRow.R_RULE_INTERVAL, Integer) * monthCount, startDate)

                                        If addStartTime.Day = startDays Then

                                            isCommonIncrementFlg = True

                                        End If

                                    End While
                                    startDate = addStartTime
                                    endDate = DateAdd(DateInterval.Second, intervalTime, addStartTime)

                                Case ConstClass.RRuleFreqYearly
                                    ' 年毎にチェック
                                    Dim yearCount As Integer = 0

                                    While isCommonIncrementFlg = False

                                        yearCount = yearCount + 1

                                        addStartTime = DateAdd(DateInterval.Year, CType(dataRow.R_RULE_INTERVAL, Integer) * yearCount, startDate)

                                        If addStartTime.Day = startDays Then

                                            isCommonIncrementFlg = True

                                        End If

                                    End While
                                    startDate = addStartTime
                                    endDate = DateAdd(DateInterval.Second, intervalTime, addStartTime)

                                Case Else
                                    Logger.Info("Wrong R_RULE_FREQ [" & dataRow.R_RULE_FREQ & "]")
                                    Exit While

                            End Select

                        End While

                    End If

            End Select

            Return makeVtodoVeventFlg

        End Function

        ''' <summary>
        ''' Rルール処理を行います
        ''' </summary>
        ''' <param name="dataRow">判別するDataRow</param>
        ''' <param name="detailElement">カレンダー情報エレメント</param>
        ''' <param name="alarmList">アラームデータテーブル</param>
        ''' <remarks></remarks>
        ''' <history>2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START</history>        
        Private Function CheckRruleOrdersReceived(ByVal dataRow As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableRow, _
                                            ByVal detailElement As XmlElement, _
                                            ByVal alarmList As List(Of Integer)) As Boolean

            ' VTODO，VEVENT要素を作成したかどうか
            Dim makeVtodoVeventFlg As Boolean = False

            ' Rルールを行うか判別する
            Select Case dataRow.R_RULE_FLG

                Case ConstClass.RRuleFlgOff
                    ' R-Rule処理を行わない場合
                    If dataRow.TODOEVENT_FLG = ConstClass.TodoEventFlgTodo Then

                        ' VTodo要素内作成メソッドを呼び出す
                        makeVTodoElements(detailElement, dataRow, alarmList)
                        makeVtodoVeventFlg = True
                    ElseIf dataRow.TODOEVENT_FLG = ConstClass.TodoEventFlgEvent Then

                        ' VEvent要素内作成メソッドを呼び出す
                        makeVEventElements(detailElement, dataRow, alarmList)
                        makeVtodoVeventFlg = True
                    End If

            End Select

            Return makeVtodoVeventFlg

        End Function



        ''' <summary>
        ''' 除外日であるかどうか確かめます。
        ''' </summary>
        ''' <param name="startTime">開始時間</param>
        ''' <param name="ids">todoId/eventId</param>
        ''' <param name="exDateDataTable">除外日DataTable</param>
        ''' <returns>除外日であればTure、そうでなければFalse</returns>
        ''' <remarks></remarks>
        Private Function IsBrackDates(ByVal startTime As Date, _
                                      ByVal ids As String, _
                                      ByVal exDateDataTable As CalenderXmlCreateClassDataSet.ExDateTableDataTable)

            For i As Integer = 0 To exDateDataTable.Count - 1

                Dim exDateDataRow As CalenderXmlCreateClassDataSet.ExDateTableRow = exDateDataTable.Rows(i)

                ' IDが同一のものを検索します。
                If Validation.Equals(exDateDataRow.IDS, ids) Then

                    ' IDが同一の場合、日付が同一か調べます
                    If Validation.Equals(Format(exDateDataRow.EXDATE, Date_Encoding), Format(startTime, Date_Encoding)) Then

                        ' 除外日チェックに使用した値を削除します
                        exDateDataTable.RemoveExDateTableRow(exDateDataTable.Rows(i))
                        Return True

                    End If

                End If
            Next i

            ' 合致するものが無い場合、除外日がなかったとします
            Return False

        End Function

        ''' <summary>
        ''' 左辺の日付よりも、右辺の日付のほうが新しいか判別する関数
        ''' </summary>
        ''' <param name="oldDate">古いと思われる日付</param>
        ''' <param name="newDate">新しいと思われる日付</param>
        ''' <returns>右辺のほうが新しい場合はTrue、古い場合はFalse</returns>
        ''' <remarks></remarks>
        Private Function IsCheckDate(ByVal oldDate As Date, ByVal newDate As Date) As Boolean

            Dim diff As Long = DateDiff(DateInterval.Second, oldDate, newDate)

            If 0 <= diff Then

                Return True

            Else

                Return False

            End If

        End Function

        ''' <summary>
        ''' 開始時間と終了時間の間にチェックしたい時間値が含まれるかチェックします。
        ''' </summary>
        ''' <param name="startDate">開始時間</param>
        ''' <param name="endDate">終了時間</param>
        ''' <param name="checkstartTime">開始チェック時間</param>
        ''' <param name="checkendTime">終了チェック時間</param>
        ''' <returns>含まれる：Ture ／含まれない：False</returns>
        ''' <remarks></remarks>
        Private Function IsIncludeDate(ByVal startDate As Date, _
                                       ByVal endDate As Date, _
                                       ByVal checkstartTime As Date, _
                                       ByVal checkendTime As Date) As Boolean

            ' チェックしたい開始時間が、終了時間よりも古いことを確認します。
            Dim diffStartEnd As Long = DateDiff(DateInterval.Second, checkstartTime, endDate)

            If diffStartEnd < 0 Then

                Return False

            End If

            ' チェックしたい終了時間が、開始時間よりも新しいことを確認します。
            Dim diffEndStart As Long = DateDiff(DateInterval.Second, startDate, checkendTime)

            If diffEndStart < 0 Then

                Return False

            End If

            Return True

        End Function

        ''' <summary>
        ''' Todo要素を作成します
        ''' </summary>
        ''' <param name="detailElements">XML要素</param>
        ''' <param name="dataRow">対象行</param>
        ''' <param name="alarmList">アラームのリスト</param>
        ''' <returns>現在取得中の行番号</returns>
        ''' <remarks></remarks>
        Private Function makeVTodoElements(ByVal detailElements As XmlElement, _
                                           ByVal dataRow As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableRow, _
                                           ByVal alarmList As List(Of Integer)) As Integer

            Dim vTodoElement As XmlElement = detailElements.OwnerDocument.CreateElement(ConstClass.XmlElementVTodo)
            detailElements.AppendChild(vTodoElement)

            ' VTodo要素の中の要素を作成します。
            setElementValue(vTodoElement, ConstClass.XmlElementContactNo, dataRow.CONTACT_NO)
            ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
            setElementValue(vTodoElement, ConstClass.XmlElementContactName, dataRow.CONTACT_NAME)
            setElementValue(vTodoElement, ConstClass.XmlElementActOdrName, dataRow.ACT_ODR_NAME)
            ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
            setElementValue(vTodoElement, ConstClass.XmlElementSummary, dataRow.SUMMARY)
            setElementValue(vTodoElement, ConstClass.XmlElementDTStart, dataRow.START_TIME)
            setElementValue(vTodoElement, ConstClass.XmlElementDue, dataRow.END_TIME)
            setElementValue(vTodoElement, ConstClass.XmlElementTimeFlg, dataRow.TIME_FLG)
            setElementValue(vTodoElement, ConstClass.XmlElementAllDayFlg, dataRow.ALLDAY_FLG)
            setElementValue(vTodoElement, ConstClass.XmlElementDescription, dataRow.MEMO)
            setElementValue(vTodoElement, ConstClass.XmlElementXICropColor, dataRow.ICROPCOLOR)
            '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
            setElementValue(vTodoElement, ConstClass.XmlElementProcessDiv, dataRow.PROCESS_DIV)
            '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END

            ' アラーム要素が空でなかった場合、格納します
            For Each alarm As Integer In alarmList

                Dim alarmElement As XmlElement = detailElements.OwnerDocument.CreateElement(ConstClass.XmlElementVAlarm)
                vTodoElement.AppendChild(alarmElement)
                setElementValue(alarmElement, ConstClass.XmlElementTrigger, alarm)

            Next

            ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
            setElementValue(vTodoElement, ConstClass.XmlElementOdrDiv, dataRow.ODR_DIV)
            setElementValue(vTodoElement, ConstClass.XmlElementAfterOdrID, dataRow.AFTER_ODR_ACT_ID)
            ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
            setElementValue(vTodoElement, ConstClass.XmlElementTodoId, dataRow.TODO_ID)
            setElementValue(vTodoElement, ConstClass.XmlElementCompFlg, dataRow.COMP_FLG)
            ' TODOイベントに紐付くEventIDが取得できている場合、EventFlgをTrueとする。
            If dataRow.EVENT_ID IsNot Nothing Then

                setElementValue(vTodoElement, ConstClass.XmlElementEventFlg, ConstClass.EventFlgTrue)

            Else

                setElementValue(vTodoElement, ConstClass.XmlElementEventFlg, ConstClass.EventFlgFalse)
            End If

            Return Nothing

        End Function

        ''' <summary>
        ''' Event要素を作成します
        ''' </summary>
        ''' <param name="detailElements">XML要素</param>
        ''' <param name="dataRow">対象行</param>
        ''' <param name="alarmList">アラームのリスト</param>
        ''' <returns>現在取得中の行番号</returns>
        ''' <remarks></remarks>
        Private Function makeVEventElements(ByVal detailElements As XmlElement, _
                                           ByVal dataRow As CalenderXmlCreateClassDataSet.SelectCreateCalendarDataTableRow, _
                                           ByVal alarmList As List(Of Integer)) As Integer

            Dim vEventElement As XmlElement = detailElements.OwnerDocument.CreateElement(ConstClass.XmlElementVEvent)
            detailElements.AppendChild(vEventElement)

            ' VEvent要素の中の要素を作成します。
            setElementValue(vEventElement, ConstClass.XmlElementContactNo, dataRow.CONTACT_NO)
            ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
            setElementValue(vEventElement, ConstClass.XmlElementContactName, dataRow.CONTACT_NAME)
            setElementValue(vEventElement, ConstClass.XmlElementActOdrName, dataRow.ACT_ODR_NAME)
            ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
            setElementValue(vEventElement, ConstClass.XmlElementSummary, dataRow.SUMMARY)
            setElementValue(vEventElement, ConstClass.XmlElementDTStart, dataRow.START_TIME)
            setElementValue(vEventElement, ConstClass.XmlElementDTEnd, dataRow.END_TIME)
            setElementValue(vEventElement, ConstClass.XmlElementTimeFlg, dataRow.TIME_FLG)
            setElementValue(vEventElement, ConstClass.XmlElementAllDayFlg, dataRow.ALLDAY_FLG)
            setElementValue(vEventElement, ConstClass.XmlElementDescription, dataRow.MEMO)
            setElementValue(vEventElement, ConstClass.XmlElementXICropColor, dataRow.ICROPCOLOR)
            '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 START
            setElementValue(vEventElement, ConstClass.XmlElementProcessDiv, dataRow.PROCESS_DIV)
            '2012/03/01 SKFC 加藤 【SALES_2】受注後工程の対応 END

            ' アラーム要素が空でなかった場合、格納します
            For Each alarm As Integer In alarmList

                Dim alarmElement As XmlElement = detailElements.OwnerDocument.CreateElement(ConstClass.XmlElementVAlarm)
                vEventElement.AppendChild(alarmElement)
                setElementValue(alarmElement, ConstClass.XmlElementTrigger, alarm)

            Next

            ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV START
            setElementValue(vEventElement, ConstClass.XmlElementOdrDiv, dataRow.ODR_DIV)
            setElementValue(vEventElement, ConstClass.XmlElementAfterOdrID, dataRow.AFTER_ODR_ACT_ID)
            ' 2014/04/25 SKFC 上田 NEXTSTEP_CALDAV END
            ' 2019/04/24 TKM PostUAT-3097 START
            setElementValue(vEventElement, ConstClass.XmlElementEventId, dataRow.UNIQUE_ID)
            ' 2019/04/24 TKM PostUAT-3097 END
            setElementValue(vEventElement, ConstClass.XmlElementLinkTodoId, dataRow.TODO_ID)
            setElementValue(vEventElement, ConstClass.XmlElementUpdateDate, dataRow.UPDATE_DATE)

            Return Nothing

        End Function

        ''' <summary>
        ''' 要素に文字列を持った要素を作成します。
        ''' </summary>
        ''' <param name="parentsElement">親要素</param>
        ''' <param name="ElementName">子要素名</param>
        ''' <param name="ElementValue">文字列</param>
        ''' <returns>作成した要素</returns>
        ''' <remarks></remarks>
        Private Function setElementValue(ByVal parentsElement As XmlElement, ByVal ElementName As String, ByVal ElementValue As String)

            If ElementValue IsNot Nothing Then

                Dim element As XmlElement = parentsElement.OwnerDocument.CreateElement(ElementName)
                element.InnerText = ElementValue
                parentsElement.AppendChild(element)

                Return parentsElement

            End If

            Return Nothing

        End Function

#End Region

    End Class

End Namespace
