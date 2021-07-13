Option Strict On
Option Explicit On

Imports System.Globalization
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Diagnostics.CodeAnalysis
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' 日付変換パターンです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ElapsedDateFormat
        ''' <summary>
        ''' 基本ルールでの変換を行います。
        ''' </summary>
        Normal
        ''' <summary>
        ''' 通知ルールでの変換を行います。
        ''' </summary>
        Notification
    End Enum

    ''' <summary>
    ''' 日付変換・取得機能を提供します。
    ''' </summary>
    ''' <remarks>
    ''' このクラスはインスタンスを生成できません。静的メソッドを呼び出してください。
    ''' </remarks>
    Public NotInheritable Class DateTimeFunc

        Private Const _FormatYear4 As String = "yyyy"
        Private Const _FormatYear2 As String = "yy"
        Private Const _FormatMonth As String = "MM"
        Private Const _FormatDay As String = "dd"
        Private Const _FormatHour As String = "HH"
        Private Const _FormatMinutes As String = "mm"

#Region "New"
        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>インスタンス化を抑止</remarks>
        Private Sub New()
        End Sub
#End Region

#Region "FormatDate"

        ''' <summary>
        ''' 共通文言の機能ＩＤ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ElapsedDisplayID As String = "ELAPSEDDATE"
        Private Const DayOfWeekDisplayID As String = "DAYOFWEEK"

        ''' <summary>
        ''' 引数"nowDate"の現在日時と引数"targetdate"を比較し、その経過期間に応じた日付文字列を取得します。
        ''' </summary>
        ''' <param name="dateFormat">日付変換パターン</param>
        ''' <param name="targetdate">現在日時と比較する日時</param>
        ''' <param name="nowDate">現在日時(<see cref="DateTimeFunc.Now"/> で取得した値を指定)</param>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="timeSpecify">
        ''' 時間指定フラグ (引数"dateFormat"の時間情報も含め書式変換するかどうかのフラグ)<br/>
        ''' Trueの場合で引数"targetdate"と"nowDate"が同じ日付の場合、戻り値には時間の情報が返されます。<br/>
        ''' Falseの場合で"targetdate"と"nowDate"が同じ日付の場合、戻り値には「今日」を表す文言が返されます。
        ''' </param>
        ''' <returns>変換結果</returns>
        ''' <remarks></remarks>
        Public Shared Function FormatElapsedDate(ByVal dateFormat As ElapsedDateFormat, _
                                                 ByVal targetdate As Date, _
                                                 ByVal nowDate As Date, _
                                                 ByVal dlrCD As String, _
                                                 ByVal timeSpecify As Boolean) As String

            '時分秒の切捨て
            Dim truncNowDate As Date = New Date(nowDate.Year, nowDate.Month, nowDate.Day)
            Dim truncTargetDate As Date = New Date(targetdate.Year, targetdate.Month, targetdate.Day)
            '日の差
            Dim totalDays As Integer = CType(truncNowDate.Subtract(truncTargetDate).TotalDays, Integer)
            Dim convID As Integer = -1
            Dim word As String = String.Empty
            Dim returnText As String = String.Empty

            If targetdate.Year < nowDate.Year Then
                '去年又はそれ以前
                convID = 3
            ElseIf totalDays = 1 Then
                '昨日
                word = WordResourceManager.GetWordData(ElapsedDisplayID, dlrCD, 1)
            ElseIf totalDays >= 2 Then
                '2日以上前(同年)
                convID = 11
            ElseIf totalDays <= -1 Then
                '未来日
                If dateFormat = ElapsedDateFormat.Normal Then
                    If targetdate.Year > nowDate.Year Then
                        '来年以降
                        convID = 3
                    Else
                        '未来日(同年)
                        convID = 11
                    End If
                End If
            ElseIf totalDays = 0 Then
                '同日
                If timeSpecify Then
                    '時間指定あり
                    Select Case dateFormat
                        Case ElapsedDateFormat.Normal
                            '基本
                            convID = 14
                        Case Else
                            '通知
                            If targetdate <= nowDate Then
                                Dim totalMinutes As Double = nowDate.Subtract(targetdate).TotalMinutes
                                If totalMinutes < 1.0R Then
                                    '１分以内
                                    word = WordResourceManager.GetWordData(ElapsedDisplayID, dlrCD, 2)
                                ElseIf Math.Floor(totalMinutes) <= 59.0R Then
                                    '59分以内
                                    word = Math.Floor(totalMinutes) & WordResourceManager.GetWordData(ElapsedDisplayID, dlrCD, 3)
                                Else
                                    '1時間以上
                                    Dim totalHour As Double = Math.Floor(nowDate.Subtract(targetdate).TotalHours + 0.5R)
                                    word = WordResourceManager.GetWordData(ElapsedDisplayID, dlrCD, 4) & Math.Floor(totalHour)
                                    word &= WordResourceManager.GetWordData(ElapsedDisplayID, dlrCD, 5)
                                End If
                            End If
                    End Select
                Else
                    '時間指定なし
                    word = WordResourceManager.GetWordData(ElapsedDisplayID, dlrCD, 6)
                End If
            End If

            If Not String.IsNullOrEmpty(word) Then
                '文言より表示
                returnText = word
            ElseIf convID > -1 Then
                '日付書式
                returnText = DateTimeFunc.FormatDate(convID, targetdate)
            End If

            Return returnText
        End Function

        ''' <summary>
        ''' 引数"nowDate"の現在日時と引数"targetdate"を比較し、その経過期間に応じた日付文字列を取得します。
        ''' </summary>
        ''' <param name="dateFormat">日付変換パターン</param>
        ''' <param name="targetdate">現在日時と比較する日時</param>
        ''' <param name="nowDate">現在日時(<see cref="DateTimeFunc.Now"/> で取得した値を指定)</param>
        ''' <param name="dlrCd">時差考慮の為の販売店コード</param>
        ''' <returns>変換結果</returns>
        ''' <remarks></remarks>
        Public Shared Function FormatElapsedDate(ByVal dateFormat As ElapsedDateFormat, _
                                                 ByVal targetdate As Date, _
                                                 ByVal nowDate As Date, _
                                                 ByVal dlrCD As String) As String
            '呼び出し
            Return DateTimeFunc.FormatElapsedDate(dateFormat, targetdate, nowDate, dlrCD, True)
        End Function

        ''' <summary>
        ''' 現在日時と引数"targetdate"を比較し、その経過期間に応じた日付文字列を取得します。
        ''' </summary>
        ''' <param name="dateFormat">日付変換パターン</param>
        ''' <param name="targetdate">現在日時と比較する日時</param>
        ''' <param name="dlrCd">時差考慮の為の販売店コード</param>
        ''' <param name="strCd">時差考慮の為の店舗コード</param>
        ''' <returns>変換結果</returns>
        ''' <remarks>
        ''' このメソッドは呼び出される度に現在日時をＤＢサーバーに問い合わせるため、１回の処理で複数回このメソッドを呼び出す場合は、
        ''' 引数に現在日時を渡すほうのメソッドを使用して下さい。
        ''' </remarks>
        Public Shared Function FormatElapsedDate(ByVal dateFormat As ElapsedDateFormat, _
                                                 ByVal targetdate As Date, _
                                                 ByVal dlrCD As String, _
                                                 Optional ByVal strCD As String = "000") As String
            '呼び出し
            Return DateTimeFunc.FormatElapsedDate(dateFormat, targetdate, DateTimeFunc.Now(dlrCD, strCD), dlrCD, True)
        End Function

        ''' <summary>
        ''' 日付フォーマットの変換を行います(Date型⇒String型)
        ''' </summary>
        ''' <param name="convID">
        ''' 1:%3/%2/%1 %4:%5:%6
        ''' 2:%3/%2/%1 %4:%5
        ''' 3:%3/%2/%1
        ''' 4:%3/%2/%1 %4:%5:%6
        ''' 5:%3/%2/%1 %4:%5
        ''' 6:%3/%2/%1
        ''' 7:%3/%2/%1 (%7)
        ''' 8:%3/%2/%1 %4:%5 (%7)
        ''' 9:%3%2%1
        ''' 10:%2/%1
        ''' 11:%3/%2
        ''' 12:%2%1
        ''' 13:%3%2
        ''' 14:%4:%5
        ''' 15:%3%2%1%4%5%6
        ''' 16:%3/%2 %7
        ''' 17:%4:%5:%6
        ''' 18:%3%2%1 %4:%5
        ''' 19:%3/%2/%1
        ''' 20:%3/%2/%1
        ''' 21:%2/%9
        ''' 22:%8 %3
        ''' 23:%1%2%3%4%5
        ''' 24:%7
        ''' </param>
        ''' <param name="targetdate">変換対象文字列</param>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <returns></returns>
        ''' <remarks>曜日を含むフォーマットに変換する場合は、dlrCd引数を指定する必要があります。</remarks>
        Public Shared Function FormatDate(ByVal convId As Integer, ByVal targetdate As Date, Optional ByVal dlrCD As String = Nothing) As String

            Dim targetformat As String = GetDateFormat(convId)

            Dim dayOfWeekWord As String = String.Empty
            dayOfWeekWord = WordResourceManager.GetWordData(DayOfWeekDisplayID, dlrCD, CInt(targetdate.DayOfWeek))
            If (Not String.IsNullOrEmpty(dayOfWeekWord)) Then
                targetformat = targetformat.Replace("ddd", "@@@")
            End If
 
            Dim result As String = targetdate.ToString(targetformat, CultureInfo.CurrentCulture)

            If (Not String.IsNullOrEmpty(dayOfWeekWord)) Then
                Return result.Replace("@@@", dayOfWeekWord)
            Else
                Return result
            End If

        End Function
#End Region

#Region "FormatString"
        ''' <summary>
        ''' 日付・時刻形式を固定長変換文字列で指定して、日付フォーマットの変換を行います。固定長変換文字列と変換対象文字列は、同じ文字列長である必要があります。
        ''' </summary>
        ''' <param name="conv">
        ''' 固定長変換文字列（以下の文字ブロックの組み合わせ）
        '''“yy”“yyyy”：年、“MM”：月、“dd”：日、”HH”：時（24時間表記）、“mm”：分、（左記以外の文字列）：リテラル文字
        ''' </param>
        ''' <param name="targetDate">変換対象</param>
        ''' <returns>変換結果</returns>
        ''' <remarks>
        ''' 日付・時刻形式を固定長変換文字列で指定して、日付フォーマットの変換を行います。
        ''' 固定長変換文字列と変換対象文字列は、同じ文字列長である必要があります。
        ''' 変換できない場合はFormatExceptionがスローされます。
        ''' </remarks>
        Public Shared Function FormatString(ByVal conv As String, ByVal targetDate As String) As Date

            If conv Is Nothing OrElse targetDate Is Nothing Then
                Throw New FormatException("target")
            End If

            If Not conv.Length = targetDate.Length Then
                Throw New FormatException("target")
            End If

            Dim chars As Char() = conv.ToCharArray()
            Dim tmpComb As String = Nothing
            Dim convList As New List(Of Dictionary(Of String, String))
            Dim convSet As Dictionary(Of String, String) = Nothing

            For i = 0 To conv.Length - 1

                Select Case chars(i).ToString
                    Case "y"
                        tmpComb = chars(i) & chars(i + 1) & chars(i + 2) & chars(i + 3)
                        If tmpComb.Equals(_FormatYear4, StringComparison.Ordinal) Then
                            convSet = New Dictionary(Of String, String)
                            convSet.Add("format", _FormatYear4)
                            convSet.Add("value", targetDate.Substring(i, _FormatYear4.Length))
                            convList.Add(convSet)
                            i = i + 3
                        ElseIf tmpComb.Substring(0, 2).Equals(_FormatYear2) Then
                            convSet = New Dictionary(Of String, String)
                            convSet.Add("format", _FormatYear2)
                            convSet.Add("value", targetDate.Substring(i, _FormatYear2.Length))
                            convList.Add(convSet)
                            i = i + 1
                        End If
                    Case "M"
                        tmpComb = chars(i) & chars(i + 1)
                        If tmpComb.Equals(_FormatMonth, StringComparison.Ordinal) Then
                            convSet = New Dictionary(Of String, String)
                            convSet.Add("format", _FormatMonth)
                            convSet.Add("value", targetDate.Substring(i, _FormatMonth.Length))
                            convList.Add(convSet)
                            i = i + 1
                        End If
                    Case "d"
                        tmpComb = chars(i) & chars(i + 1)
                        If tmpComb.Equals(_FormatDay, StringComparison.Ordinal) Then
                            convSet = New Dictionary(Of String, String)
                            convSet.Add("format", _FormatDay)
                            convSet.Add("value", targetDate.Substring(i, _FormatDay.Length))
                            convList.Add(convSet)
                            i = i + 1
                        End If
                    Case "H"
                        tmpComb = chars(i) & chars(i + 1)
                        If tmpComb.Equals(_FormatHour, StringComparison.Ordinal) Then
                            convSet = New Dictionary(Of String, String)
                            convSet.Add("format", _FormatHour)
                            convSet.Add("value", targetDate.Substring(i, _FormatHour.Length))
                            convList.Add(convSet)
                            i = i + 1
                        End If
                    Case "m"
                        tmpComb = chars(i) & chars(i + 1)
                        If tmpComb.Equals(_FormatMinutes, StringComparison.Ordinal) Then
                            convSet = New Dictionary(Of String, String)
                            convSet.Add("format", _FormatMinutes)
                            convSet.Add("value", targetDate.Substring(i, _FormatMinutes.Length))
                            convList.Add(convSet)
                            i = i + 1
                        End If
                End Select
            Next i

            Dim sbFormat As New StringBuilder
            Dim sbValue As New StringBuilder
            For Each comb In convList
                sbFormat.Append(comb("format"))
                sbValue.Append(comb("value"))
            Next comb

            Dim convDate As Date
            If Not DateTime.TryParseExact(sbValue.ToString, sbFormat.ToString, System.Globalization.DateTimeFormatInfo.InvariantInfo, System.Globalization.DateTimeStyles.None, convDate) Then
                Throw New FormatException("target")
            End If

            Return convDate

        End Function
#End Region

#Region "Now"
        ''' <summary>
        ''' 現地の現在日時を取得します。
        ''' </summary>
        ''' <param name="dlrCd">時差考慮の為の販売店コード</param>
        ''' <param name="strCD">時差考慮の為の店舗コード (未指定の場合は"000")</param>
        ''' <returns>時差を考慮した現在日時</returns>
        ''' <remarks>
        ''' 現地の現在日時を取得します。
        ''' DBサーバーのクロックおよびTBL_AREAMASTERテーブルの時差情報を使用して算出します。
        ''' 引数がない場合はDBサーバーのクロックになります。
        ''' </remarks>
        Public Shared Function Now(Optional ByVal dlrCD As String = Nothing, Optional ByVal strCD As String = "000") As DateTime

            Dim dateNow As Date
            If String.IsNullOrEmpty(dlrCD) Then
                dateNow = DateTimeBizLogic.GetNow()
            Else
                dateNow = DateTimeBizLogic.GetNow(dlrCD, strCD)
            End If

            Return dateNow

        End Function
#End Region

#Region "FormatDateSS"
        '        ''' <summary>
        '        ''' 日付フォーマットの変換を行います(String型⇒String型)
        '        ''' </summary>
        '        ''' <param name="convID">
        '        ''' 1:%3/%2/%1 %4:%5:%6
        '        ''' 2:%3/%2/%1 %4:%5
        '        ''' 3:%3/%2/%1
        '        ''' 4:%3/%2/%1 %4:%5:%6
        '        ''' 5:%3/%2/%1 %4:%5
        '        ''' 6:%3/%2/%1
        '        ''' 7:%3/%2/%1 (%7)
        '        ''' 8:%3/%2/%1 %4:%5 (%7)
        '        ''' 9:%3%2%1
        '        ''' 10:%2/%1
        '        ''' 11:%3/%2
        '        ''' 12:%2%1
        '        ''' 13:%3%2
        '        ''' 14:%4:%5
        '        ''' 15:%3%2%1%4%5%6
        '        ''' 16:%3/%2 %7
        '        ''' 17:%4:%5:%6
        '        ''' 18:%3%2%1 %4:%5
        '        ''' 19:%3/%2/%1
        '        ''' 20:%3/%2/%1
        '        ''' 21:%2/%9
        '        ''' 22:%8 %3
        '        ''' </param>
        '        ''' <param name="targetdate">
        '        ''' 変換対象文字列。以下の形式のみをサポートしています。
        '        ''' <para>yyyyMMdd</para>
        '        ''' </param>
        '        ''' <returns></returns>
        '        ''' <remarks></remarks>
        '        Public Shared Function FormatDateSS(ByVal convID As Integer, ByVal targetdate As String) As String

        '            targetdate = Trim(targetdate & "")

        '            If String.IsNullOrEmpty(CStr(convID)) Then
        '                Return String.Empty
        '            End If

        '            Dim vDate(7) As String
        '            '入力された値が数字の羅列の場合(文字列長14文字)
        '            If (Len(targetdate) = 14 And IsNumeric(targetdate) And String.Equals(Left(targetdate, 1), "-") = False) Then
        '                vDate(0) = Left(targetdate, 4)
        '                vDate(1) = Mid(targetdate, 5, 2)
        '                vDate(2) = Mid(targetdate, 7, 2)
        '                vDate(3) = Mid(targetdate, 9, 2)
        '                vDate(4) = Mid(targetdate, 11, 2)
        '                vDate(5) = Mid(targetdate, 13, 2)

        '                '入力された値が数字の羅列の場合(文字列長8文字)
        '            ElseIf (Len(targetdate) = 8 And IsNumeric(targetdate) And String.Equals(Left(targetdate, 1), "-") = False) Then
        '                vDate(0) = Left(targetdate, 4)
        '                vDate(1) = Mid(targetdate, 5, 2)
        '                vDate(2) = Mid(targetdate, 7, 2)
        '                vDate(3) = "00"
        '                vDate(4) = "00"
        '                vDate(5) = "00"

        '                '入力された値が日付型に変換できる場合
        '            ElseIf IsDate(targetdate) Then
        '                vDate(0) = Right("0000" & Year(CDate(targetdate)), 4)
        '                vDate(1) = Right("00" & Month(CDate(targetdate)), 2)
        '                vDate(2) = Right("00" & Day(CDate(targetdate)), 2)
        '                vDate(3) = Right("00" & Hour(CDate(targetdate)), 2)
        '                vDate(4) = Right("00" & Minute(CDate(targetdate)), 2)
        '                vDate(5) = Right("00" & Second(CDate(targetdate)), 2)

        '                'その他の場合は未サポート
        '            Else
        '                Return String.Empty
        '            End If

        '            '日付型と認識できない場合は未サポート
        '            Dim tempdate As New System.Text.StringBuilder
        '            tempdate.Append(vDate(0))
        '            tempdate.Append("/")
        '            tempdate.Append(vDate(1))
        '            tempdate.Append("/")
        '            tempdate.Append(vDate(2))
        '            tempdate.Append(" ")
        '            tempdate.Append(vDate(3))
        '            tempdate.Append(":")
        '            tempdate.Append(vDate(4))
        '            tempdate.Append(":")
        '            tempdate.Append(vDate(5))

        '            If IsDate(tempdate.ToString()) = False Then
        '                Return String.Empty
        '            Else
        '                tempdate = tempdate
        '            End If

        '            Dim formatdate As String
        '            formatdate = DateTimeForm.GetDateTimeForm(convID)

        '            '置換処理
        '            Dim i As Integer = Nothing
        '            For i = 1 To 6
        '                formatdate = Replace(formatdate, "%" & i, vDate(i - 1))
        '            Next

        '            'TODO:
        '            ''週の名称の置換
        '            'formatdate = Replace(formatdate, "%7", HttpWordUtility.GetWord("HEADER", Weekday(CDate(tempdate.ToString()))))

        '            ''月の名称の置換
        '            'formatdate = Replace(formatdate, "%8", HttpWordUtility.GetWord("HEADER", 7 + CLng(vDate(1))))

        '            ''年（2桁）の置換
        '            'formatdate = Replace(formatdate, "%9", Right(vDate(0), 2))

        '            Return formatdate

        '        End Function
#End Region

#Region "FormatDateSD"
        ''' <summary>
        ''' 日付フォーマットの変換を行います(String型⇒Date型)
        ''' </summary>
        ''' <param name="convID">
        ''' 1:%3/%2/%1 %4:%5:%6
        ''' 2:%3/%2/%1 %4:%5
        ''' 3:%3/%2/%1
        ''' 4:%3/%2/%1 %4:%5:%6
        ''' 5:%3/%2/%1 %4:%5
        ''' 6:%3/%2/%1
        ''' 7:%3/%2/%1 (%7)
        ''' 8:%3/%2/%1 %4:%5 (%7)
        ''' 9:%3%2%1
        ''' 10:%2/%1
        ''' 11:%3/%2
        ''' 12:%2%1
        ''' 13:%3%2
        ''' 14:%4:%5
        ''' 15:%3%2%1%4%5%6
        ''' 16:%3/%2 %7
        ''' 17:%4:%5:%6
        ''' 18:%3%2%1 %4:%5
        ''' 19:%3/%2/%1
        ''' 20:%3/%2/%1
        ''' 21:%2/%9
        ''' 22:%8 %3
        ''' </param>
        ''' <param name="target">変換対象文字列</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function FormatDateSD(ByVal convId As Integer, ByVal target As String) As Nullable(Of Date)

            Dim formatdate As String
            formatdate = DateTimeForm.GetDateTimeForm(convId)

            FormatDateSD = Nothing

            '変換フォーマットが指定されていない場合エラー
            If String.IsNullOrEmpty(CStr(convId)) Then
                Return FormatDateSD
            End If

            '文字数の確認
            If Len(Replace(formatdate, "%1", "2000")) <> Len(target) Then
                Return FormatDateSD
            End If

            Dim year As String = "2000"
            Dim month As String = "01"
            Dim day As String = "01"
            Dim hour As String = "00"
            Dim minute As String = "00"
            Dim second As String = "00"
            Dim i As Integer = 1
            Dim lenposition As Integer = 1

            Do Until i >= Len(formatdate)
                If String.Equals(Mid(formatdate, i, 1), "%") Then
                    Select Case Mid(formatdate, i, 2)
                        Case "%1"
                            year = Mid(target, lenposition, 4)
                            lenposition = lenposition + 4
                        Case "%2"
                            month = Mid(target, lenposition, 2)
                            lenposition = lenposition + 2
                        Case "%3"
                            day = Mid(target, lenposition, 2)
                            lenposition = lenposition + 2
                        Case "%4"
                            hour = Mid(target, lenposition, 2)
                            lenposition = lenposition + 2
                        Case "%5"
                            minute = Mid(target, lenposition, 2)
                            lenposition = lenposition + 2
                        Case "%6"
                            second = Mid(target, lenposition, 2)
                            lenposition = lenposition + 2
                        Case "%9"
                            year = Mid(target, lenposition, 2)
                            lenposition = lenposition + 2
                    End Select
                    i = i + 2
                Else
                    If String.Equals(Mid(formatdate, i, 1), Mid(target, lenposition, 1)) = False Then
                        Return FormatDateSD
                    End If
                    i = i + 1
                    lenposition = lenposition + 1
                End If
            Loop

            Dim tempdate As New System.Text.StringBuilder
            tempdate.Append(year)
            tempdate.Append("/")
            tempdate.Append(month)
            tempdate.Append("/")
            tempdate.Append(day)
            tempdate.Append(" ")
            tempdate.Append(hour)
            tempdate.Append(":")
            tempdate.Append(minute)
            tempdate.Append(":")
            tempdate.Append(second)

            '日付として成り立っているか確認
            If IsDate(tempdate.ToString()) = False Then
                Return FormatDateSD
            End If

            FormatDateSD = CDate(tempdate.ToString())

        End Function
#End Region

#Region "GetDate"
        ''' <summary>
        ''' 日付取得関数
        ''' </summary>
        ''' <param name="convID">
        ''' 1:yyyymmdd
        ''' 2:yyyy/mm/dd
        ''' </param>
        ''' <param name="defference">サーバのローカル日時からの時差</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDate(ByVal convId As Integer, ByVal defference As Integer) As String

            Dim tempdate As Date = Nothing

            tempdate = DateTime.Now.AddHours(defference)

            If convId = 1 Then
                'GetDate = Year(tempdate) & Right(("00" & Month(tempdate)), 2) & Right(("00" & Day(tempdate)), 2)
                Return tempdate.ToString("yyyyMMdd", CultureInfo.CurrentCulture)
            ElseIf convId = 2 Then
                'GetDate = Year(tempdate) & "/" & Right(("00" & Month(tempdate)), 2) & "/" & Right(("00" & Day(tempdate)), 2)
                Return tempdate.ToString("yyyy/MM/dd", CultureInfo.CurrentCulture)
            Else
                GetDate = String.Empty
            End If

        End Function
#End Region

#Region "GetTime"
        ''' <summary>
        ''' 時刻取得関数
        ''' </summary>
        ''' <param name="convID">
        ''' 1:hhmmss
        ''' 2:hh:mm:ss
        ''' </param>
        ''' <param name="timeDiff">サーバローカル日時からの時差</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetTime(ByVal convId As Integer, ByVal timeDiff As Integer) As String

            Dim tempdate As Date
            tempdate = DateTime.Now.AddHours(timeDiff)

            If convId = 1 Then
                'GetTime = Right(("00" & Hour(tempdate)), 2) & Right(("00" & Minute(tempdate)), 2) & Right(("00" & Second(tempdate)), 2)
                Return tempdate.ToString("HHmmss", CultureInfo.CurrentCulture)
            ElseIf (convId = 2) Then
                Return tempdate.ToString("HH:mm:ss", CultureInfo.CurrentCulture)
                'GetTime = Right(("00" & Hour(tempdate)), 2) & ":" & Right(("00" & Minute(tempdate)), 2) & ":" & Right(("00" & Second(tempdate)), 2)
            Else
                Return String.Empty
            End If

        End Function
#End Region

#Region "GetDateFormat"
        ''' <summary>
        ''' 指定された日付フォーマットを返却
        ''' </summary>
        ''' <param name="convID">
        ''' 1:%3/%2/%1 %4:%5:%6
        ''' 2:%3/%2/%1 %4:%5
        ''' 3:%3/%2/%1
        ''' 4:%3/%2/%1 %4:%5:%6
        ''' 5:%3/%2/%1 %4:%5
        ''' 6:%3/%2/%1
        ''' 7:%3/%2/%1 (%7)
        ''' 8:%3/%2/%1 %4:%5 (%7)
        ''' 9:%3%2%1
        ''' 10:%2/%1
        ''' 11:%3/%2
        ''' 12:%2%1
        ''' 13:%3%2
        ''' 14:%4:%5
        ''' 15:%3%2%1%4%5%6
        ''' 16:%3/%2 %7
        ''' 17:%4:%5:%6
        ''' 18:%3%2%1 %4:%5
        ''' 19:%3/%2/%1
        ''' 20:%3/%2/%1
        ''' 21:%2/%9
        ''' 22:%8 %3
        ''' 23:%1%2%3%4%5
        ''' 24:%7
        ''' </param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetDateFormat(ByVal convId As Integer) As String

            Dim format As String = Nothing

            format = DateTimeForm.GetDateTimeForm(convId)

            format = Replace(format, "%1", "yyyy")
            format = Replace(format, "%2", "MM")
            format = Replace(format, "%3", "dd")
            format = Replace(format, "%4", "HH")
            format = Replace(format, "%5", "mm")
            format = Replace(format, "%6", "ss")
            format = Replace(format, "%7", "ddd")
            format = Replace(format, "%8", "MMM")
            format = Replace(format, "%9", "yy")

            Return format

        End Function
#End Region

#Region "GetSqlDateFormat"
        ''' <summary>
        ''' 指定された日付フォーマットを返却(SQL用にDD/MM/YYYY HH24:MI:SSなどの書式を取得します)
        ''' </summary>
        ''' <param name="convID">
        ''' 1:%3/%2/%1 %4:%5:%6
        ''' 2:%3/%2/%1 %4:%5
        ''' 3:%3/%2/%1
        ''' 4:%3/%2/%1 %4:%5:%6
        ''' 5:%3/%2/%1 %4:%5
        ''' 6:%3/%2/%1
        ''' 7:%3/%2/%1 (%7)
        ''' 8:%3/%2/%1 %4:%5 (%7)
        ''' 9:%3%2%1
        ''' 10:%2/%1
        ''' 11:%3/%2
        ''' 12:%2%1
        ''' 13:%3%2
        ''' 14:%4:%5
        ''' 15:%3%2%1%4%5%6
        ''' 16:%3/%2 %7
        ''' 17:%4:%5:%6
        ''' 18:%3%2%1 %4:%5
        ''' 19:%3/%2/%1
        ''' 20:%3/%2/%1
        ''' 21:%2/%9
        ''' 22:%8 %3
        ''' 23:%1%2%3%4%5
        ''' 24:%7
        ''' </param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetSqlDateFormat(ByVal convId As Integer) As String

            Dim format As String = Nothing

            format = DateTimeForm.GetDateTimeForm(convId)

            format = Replace(format, "%1", "YYYY")
            format = Replace(format, "%2", "MM")
            format = Replace(format, "%3", "DD")
            format = Replace(format, "%4", "HH24")
            format = Replace(format, "%5", "MI")
            format = Replace(format, "%6", "SS")
            format = Replace(format, "%7", "DY")
            format = Replace(format, "%8", "FMMM")
            format = Replace(format, "%9", "RR")

            Return format

        End Function
#End Region

    End Class

End Namespace
