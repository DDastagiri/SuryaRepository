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
    ''' ���t�ϊ��p�^�[���ł��B
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum ElapsedDateFormat
        ''' <summary>
        ''' ��{���[���ł̕ϊ����s���܂��B
        ''' </summary>
        Normal
        ''' <summary>
        ''' �ʒm���[���ł̕ϊ����s���܂��B
        ''' </summary>
        Notification
    End Enum

    ''' <summary>
    ''' ���t�ϊ��E�擾�@�\��񋟂��܂��B
    ''' </summary>
    ''' <remarks>
    ''' ���̃N���X�̓C���X�^���X�𐶐��ł��܂���B�ÓI���\�b�h���Ăяo���Ă��������B
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
        ''' �R���X�g���N�^
        ''' </summary>
        ''' <remarks>�C���X�^���X����}�~</remarks>
        Private Sub New()
        End Sub
#End Region

#Region "FormatDate"

        ''' <summary>
        ''' ���ʕ����̋@�\�h�c
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ElapsedDisplayID As String = "ELAPSEDDATE"
        Private Const DayOfWeekDisplayID As String = "DAYOFWEEK"

        ''' <summary>
        ''' ����"nowDate"�̌��ݓ����ƈ���"targetdate"���r���A���̌o�ߊ��Ԃɉ��������t��������擾���܂��B
        ''' </summary>
        ''' <param name="dateFormat">���t�ϊ��p�^�[��</param>
        ''' <param name="targetdate">���ݓ����Ɣ�r�������</param>
        ''' <param name="nowDate">���ݓ���(<see cref="DateTimeFunc.Now"/> �Ŏ擾�����l���w��)</param>
        ''' <param name="dlrCd">�̔��X�R�[�h</param>
        ''' <param name="timeSpecify">
        ''' ���Ԏw��t���O (����"dateFormat"�̎��ԏ����܂ߏ����ϊ����邩�ǂ����̃t���O)<br/>
        ''' True�̏ꍇ�ň���"targetdate"��"nowDate"���������t�̏ꍇ�A�߂�l�ɂ͎��Ԃ̏�񂪕Ԃ���܂��B<br/>
        ''' False�̏ꍇ��"targetdate"��"nowDate"���������t�̏ꍇ�A�߂�l�ɂ́u�����v��\���������Ԃ���܂��B
        ''' </param>
        ''' <returns>�ϊ�����</returns>
        ''' <remarks></remarks>
        Public Shared Function FormatElapsedDate(ByVal dateFormat As ElapsedDateFormat, _
                                                 ByVal targetdate As Date, _
                                                 ByVal nowDate As Date, _
                                                 ByVal dlrCD As String, _
                                                 ByVal timeSpecify As Boolean) As String

            '�����b�̐؎̂�
            Dim truncNowDate As Date = New Date(nowDate.Year, nowDate.Month, nowDate.Day)
            Dim truncTargetDate As Date = New Date(targetdate.Year, targetdate.Month, targetdate.Day)
            '���̍�
            Dim totalDays As Integer = CType(truncNowDate.Subtract(truncTargetDate).TotalDays, Integer)
            Dim convID As Integer = -1
            Dim word As String = String.Empty
            Dim returnText As String = String.Empty

            If targetdate.Year < nowDate.Year Then
                '���N���͂���ȑO
                convID = 3
            ElseIf totalDays = 1 Then
                '���
                word = WordResourceManager.GetWordData(ElapsedDisplayID, dlrCD, 1)
            ElseIf totalDays >= 2 Then
                '2���ȏ�O(���N)
                convID = 11
            ElseIf totalDays <= -1 Then
                '������
                If dateFormat = ElapsedDateFormat.Normal Then
                    If targetdate.Year > nowDate.Year Then
                        '���N�ȍ~
                        convID = 3
                    Else
                        '������(���N)
                        convID = 11
                    End If
                End If
            ElseIf totalDays = 0 Then
                '����
                If timeSpecify Then
                    '���Ԏw�肠��
                    Select Case dateFormat
                        Case ElapsedDateFormat.Normal
                            '��{
                            convID = 14
                        Case Else
                            '�ʒm
                            If targetdate <= nowDate Then
                                Dim totalMinutes As Double = nowDate.Subtract(targetdate).TotalMinutes
                                If totalMinutes < 1.0R Then
                                    '�P���ȓ�
                                    word = WordResourceManager.GetWordData(ElapsedDisplayID, dlrCD, 2)
                                ElseIf Math.Floor(totalMinutes) <= 59.0R Then
                                    '59���ȓ�
                                    word = Math.Floor(totalMinutes) & WordResourceManager.GetWordData(ElapsedDisplayID, dlrCD, 3)
                                Else
                                    '1���Ԉȏ�
                                    Dim totalHour As Double = Math.Floor(nowDate.Subtract(targetdate).TotalHours + 0.5R)
                                    word = WordResourceManager.GetWordData(ElapsedDisplayID, dlrCD, 4) & Math.Floor(totalHour)
                                    word &= WordResourceManager.GetWordData(ElapsedDisplayID, dlrCD, 5)
                                End If
                            End If
                    End Select
                Else
                    '���Ԏw��Ȃ�
                    word = WordResourceManager.GetWordData(ElapsedDisplayID, dlrCD, 6)
                End If
            End If

            If Not String.IsNullOrEmpty(word) Then
                '�������\��
                returnText = word
            ElseIf convID > -1 Then
                '���t����
                returnText = DateTimeFunc.FormatDate(convID, targetdate)
            End If

            Return returnText
        End Function

        ''' <summary>
        ''' ����"nowDate"�̌��ݓ����ƈ���"targetdate"���r���A���̌o�ߊ��Ԃɉ��������t��������擾���܂��B
        ''' </summary>
        ''' <param name="dateFormat">���t�ϊ��p�^�[��</param>
        ''' <param name="targetdate">���ݓ����Ɣ�r�������</param>
        ''' <param name="nowDate">���ݓ���(<see cref="DateTimeFunc.Now"/> �Ŏ擾�����l���w��)</param>
        ''' <param name="dlrCd">�����l���ׂ̈̔̔��X�R�[�h</param>
        ''' <returns>�ϊ�����</returns>
        ''' <remarks></remarks>
        Public Shared Function FormatElapsedDate(ByVal dateFormat As ElapsedDateFormat, _
                                                 ByVal targetdate As Date, _
                                                 ByVal nowDate As Date, _
                                                 ByVal dlrCD As String) As String
            '�Ăяo��
            Return DateTimeFunc.FormatElapsedDate(dateFormat, targetdate, nowDate, dlrCD, True)
        End Function

        ''' <summary>
        ''' ���ݓ����ƈ���"targetdate"���r���A���̌o�ߊ��Ԃɉ��������t��������擾���܂��B
        ''' </summary>
        ''' <param name="dateFormat">���t�ϊ��p�^�[��</param>
        ''' <param name="targetdate">���ݓ����Ɣ�r�������</param>
        ''' <param name="dlrCd">�����l���ׂ̈̔̔��X�R�[�h</param>
        ''' <param name="strCd">�����l���ׂ̈̓X�܃R�[�h</param>
        ''' <returns>�ϊ�����</returns>
        ''' <remarks>
        ''' ���̃��\�b�h�͌Ăяo�����x�Ɍ��ݓ������c�a�T�[�o�[�ɖ₢���킹�邽�߁A�P��̏����ŕ����񂱂̃��\�b�h���Ăяo���ꍇ�́A
        ''' �����Ɍ��ݓ�����n���ق��̃��\�b�h���g�p���ĉ������B
        ''' </remarks>
        Public Shared Function FormatElapsedDate(ByVal dateFormat As ElapsedDateFormat, _
                                                 ByVal targetdate As Date, _
                                                 ByVal dlrCD As String, _
                                                 Optional ByVal strCD As String = "000") As String
            '�Ăяo��
            Return DateTimeFunc.FormatElapsedDate(dateFormat, targetdate, DateTimeFunc.Now(dlrCD, strCD), dlrCD, True)
        End Function

        ''' <summary>
        ''' ���t�t�H�[�}�b�g�̕ϊ����s���܂�(Date�^��String�^)
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
        ''' <param name="targetdate">�ϊ��Ώە�����</param>
        ''' <param name="dlrCd">�̔��X�R�[�h</param>
        ''' <returns></returns>
        ''' <remarks>�j�����܂ރt�H�[�}�b�g�ɕϊ�����ꍇ�́AdlrCd�������w�肷��K�v������܂��B</remarks>
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
        ''' ���t�E�����`�����Œ蒷�ϊ�������Ŏw�肵�āA���t�t�H�[�}�b�g�̕ϊ����s���܂��B�Œ蒷�ϊ�������ƕϊ��Ώە�����́A���������񒷂ł���K�v������܂��B
        ''' </summary>
        ''' <param name="conv">
        ''' �Œ蒷�ϊ�������i�ȉ��̕����u���b�N�̑g�ݍ��킹�j
        '''�gyy�h�gyyyy�h�F�N�A�gMM�h�F���A�gdd�h�F���A�hHH�h�F���i24���ԕ\�L�j�A�gmm�h�F���A�i���L�ȊO�̕�����j�F���e��������
        ''' </param>
        ''' <param name="targetDate">�ϊ��Ώ�</param>
        ''' <returns>�ϊ�����</returns>
        ''' <remarks>
        ''' ���t�E�����`�����Œ蒷�ϊ�������Ŏw�肵�āA���t�t�H�[�}�b�g�̕ϊ����s���܂��B
        ''' �Œ蒷�ϊ�������ƕϊ��Ώە�����́A���������񒷂ł���K�v������܂��B
        ''' �ϊ��ł��Ȃ��ꍇ��FormatException���X���[����܂��B
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
        ''' ���n�̌��ݓ������擾���܂��B
        ''' </summary>
        ''' <param name="dlrCd">�����l���ׂ̈̔̔��X�R�[�h</param>
        ''' <param name="strCD">�����l���ׂ̈̓X�܃R�[�h (���w��̏ꍇ��"000")</param>
        ''' <returns>�������l���������ݓ���</returns>
        ''' <remarks>
        ''' ���n�̌��ݓ������擾���܂��B
        ''' DB�T�[�o�[�̃N���b�N�����TBL_AREAMASTER�e�[�u���̎��������g�p���ĎZ�o���܂��B
        ''' �������Ȃ��ꍇ��DB�T�[�o�[�̃N���b�N�ɂȂ�܂��B
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
        '        ''' ���t�t�H�[�}�b�g�̕ϊ����s���܂�(String�^��String�^)
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
        '        ''' �ϊ��Ώە�����B�ȉ��̌`���݂̂��T�|�[�g���Ă��܂��B
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
        '            '���͂��ꂽ�l�������̗���̏ꍇ(������14����)
        '            If (Len(targetdate) = 14 And IsNumeric(targetdate) And String.Equals(Left(targetdate, 1), "-") = False) Then
        '                vDate(0) = Left(targetdate, 4)
        '                vDate(1) = Mid(targetdate, 5, 2)
        '                vDate(2) = Mid(targetdate, 7, 2)
        '                vDate(3) = Mid(targetdate, 9, 2)
        '                vDate(4) = Mid(targetdate, 11, 2)
        '                vDate(5) = Mid(targetdate, 13, 2)

        '                '���͂��ꂽ�l�������̗���̏ꍇ(������8����)
        '            ElseIf (Len(targetdate) = 8 And IsNumeric(targetdate) And String.Equals(Left(targetdate, 1), "-") = False) Then
        '                vDate(0) = Left(targetdate, 4)
        '                vDate(1) = Mid(targetdate, 5, 2)
        '                vDate(2) = Mid(targetdate, 7, 2)
        '                vDate(3) = "00"
        '                vDate(4) = "00"
        '                vDate(5) = "00"

        '                '���͂��ꂽ�l�����t�^�ɕϊ��ł���ꍇ
        '            ElseIf IsDate(targetdate) Then
        '                vDate(0) = Right("0000" & Year(CDate(targetdate)), 4)
        '                vDate(1) = Right("00" & Month(CDate(targetdate)), 2)
        '                vDate(2) = Right("00" & Day(CDate(targetdate)), 2)
        '                vDate(3) = Right("00" & Hour(CDate(targetdate)), 2)
        '                vDate(4) = Right("00" & Minute(CDate(targetdate)), 2)
        '                vDate(5) = Right("00" & Second(CDate(targetdate)), 2)

        '                '���̑��̏ꍇ�͖��T�|�[�g
        '            Else
        '                Return String.Empty
        '            End If

        '            '���t�^�ƔF���ł��Ȃ��ꍇ�͖��T�|�[�g
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

        '            '�u������
        '            Dim i As Integer = Nothing
        '            For i = 1 To 6
        '                formatdate = Replace(formatdate, "%" & i, vDate(i - 1))
        '            Next

        '            'TODO:
        '            ''�T�̖��̂̒u��
        '            'formatdate = Replace(formatdate, "%7", HttpWordUtility.GetWord("HEADER", Weekday(CDate(tempdate.ToString()))))

        '            ''���̖��̂̒u��
        '            'formatdate = Replace(formatdate, "%8", HttpWordUtility.GetWord("HEADER", 7 + CLng(vDate(1))))

        '            ''�N�i2���j�̒u��
        '            'formatdate = Replace(formatdate, "%9", Right(vDate(0), 2))

        '            Return formatdate

        '        End Function
#End Region

#Region "FormatDateSD"
        ''' <summary>
        ''' ���t�t�H�[�}�b�g�̕ϊ����s���܂�(String�^��Date�^)
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
        ''' <param name="target">�ϊ��Ώە�����</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function FormatDateSD(ByVal convId As Integer, ByVal target As String) As Nullable(Of Date)

            Dim formatdate As String
            formatdate = DateTimeForm.GetDateTimeForm(convId)

            FormatDateSD = Nothing

            '�ϊ��t�H�[�}�b�g���w�肳��Ă��Ȃ��ꍇ�G���[
            If String.IsNullOrEmpty(CStr(convId)) Then
                Return FormatDateSD
            End If

            '�������̊m�F
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

            '���t�Ƃ��Đ��藧���Ă��邩�m�F
            If IsDate(tempdate.ToString()) = False Then
                Return FormatDateSD
            End If

            FormatDateSD = CDate(tempdate.ToString())

        End Function
#End Region

#Region "GetDate"
        ''' <summary>
        ''' ���t�擾�֐�
        ''' </summary>
        ''' <param name="convID">
        ''' 1:yyyymmdd
        ''' 2:yyyy/mm/dd
        ''' </param>
        ''' <param name="defference">�T�[�o�̃��[�J����������̎���</param>
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
        ''' �����擾�֐�
        ''' </summary>
        ''' <param name="convID">
        ''' 1:hhmmss
        ''' 2:hh:mm:ss
        ''' </param>
        ''' <param name="timeDiff">�T�[�o���[�J����������̎���</param>
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
        ''' �w�肳�ꂽ���t�t�H�[�}�b�g��ԋp
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
        ''' �w�肳�ꂽ���t�t�H�[�}�b�g��ԋp(SQL�p��DD/MM/YYYY HH24:MI:SS�Ȃǂ̏������擾���܂�)
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
