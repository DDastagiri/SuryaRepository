'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.

Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Diagnostics.CodeAnalysis
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' データ検証機能を提供します。
    ''' </summary>
    ''' <remarks>
    ''' このクラスはインスタンスを生成できません。静的メソッドを呼び出してください。
    ''' </remarks>
    Public NotInheritable Class Validation

        Private Const C_INVALIDCHARACTER_SETTING As String = "InvalidCharacters"
        Private Shared _suppressionCharList As List(Of String()) = Nothing
        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>インスタンス化を抑止</remarks>
        Private Sub New()
        End Sub

#Region "プロパティ"
        ''' <summary>
        ''' 禁止文字確認設定
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Shared ReadOnly Property SuppressionCharList() As List(Of String())
            Get
                If _suppressionCharList Is Nothing Then

                    Dim validationClass As ClassSection = SystemConfiguration.Current.Manager.Validation

                    Dim validationSetting As Setting = validationClass.GetSetting(C_INVALIDCHARACTER_SETTING)

                    If validationSetting Is Nothing OrElse validationSetting.Item.Count = 0 Then
                        _suppressionCharList = New List(Of String())
                        Return _suppressionCharList
                    End If

                    Dim check As New StringBuilder  ''設定の値が記述されているかの確認用

                    Dim tempSuppress As New List(Of String())
                    ''設定数ループ
                    For Each item In validationSetting.Item
                        ''カンマで複数選択されているのを分解
                        Dim charSets() As String = item.Value.ToString.Split(","c)
                        For Each value In charSets
                            ''-で範囲指定しているのを分解
                            Dim codes As String() = value.Split("-"c)
                            Dim isAdd As Boolean = True
                            For Each code In codes
                                If String.IsNullOrEmpty(code) AndAlso isAdd Then
                                    isAdd = False
                                End If
                            Next
                            If isAdd Then
                                tempSuppress.Add(value.Split("-"c))
                                check.Append(value)
                            End If
                        Next value
                    Next item

                    ''設定の値が記述されていないので終了
                    If tempSuppress.Count = 0 Then
                        _suppressionCharList = New List(Of String())
                        Return _suppressionCharList
                    End If

                    _suppressionCharList = tempSuppress
                End If

                Return _suppressionCharList
            End Get
        End Property
#End Region

#Region "IsHankakuAlphabet"
        ''' <summary>
        ''' 半角アルファベット文字列の判別を行います
        ''' </summary>
        ''' <param name="target">検証対象の文字列</param>
        ''' <returns>
        ''' 検証対象の文字列が半角アルファベット文字列の場合はTrue、それ以外の場合はFalse。
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' パラメータの検証対象文字列がnull参照（VBではNothing）。
        ''' </exception>
        ''' <remarks>
        ''' <para><B>[機能詳細]</B></para>
        ''' <para>検証対象が半角アルファベット文字列であるかを検証します。</para>
        ''' <para>半角アルファベット文字列の場合は、<c><b>True</b></c></para>
        ''' <para>半角アルファベット文字列以外の文字列の場合は、<c><b>False</b></c></para>
        ''' <para>　</para>
        ''' <para><b>[注意事項]</b></para>
        ''' <para>デフォルトでは正規表現パターン「^[a-zA-Z]+$」で検証を行っています。</para>
        ''' <para>
        ''' なお、この値は外部構成ファイルの「HankakuAlphabetFormat」要素の値を変更することによって変更することが可能です。
        ''' </para>
        ''' <para>
        ''' デフォルトの正規表現パターンでは検索対象の文字列が空文字（"")の場合、戻り値はFalseになります。
        ''' </para>
        ''' <para>　</para>
        ''' <para><b>[サンプル]</b></para>
        ''' <example>
        ''' <code>
        '''      Private Sub ValidationTest()
        '''
        '''          Dim targetString As String = "abcde"
        '''          Dim isValid As Boolean = Validation.IsHankakuAlphabet(targetString)
        '''
        '''     End Sub        
        ''' </code>
        ''' </example>
        ''' </remarks>
        Public Shared Function IsHankakuAlphabet(ByVal target As String) As Boolean

            If String.IsNullOrEmpty(target) Then
                Return False
            End If

            Dim validationClass As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = _
                    SystemConfiguration.Current.Manager.Validation

            Dim validationSetting As Toyota.eCRB.SystemFrameworks.Configuration.Setting = _
                    validationClass.GetSetting(String.Empty)

            Dim regexSetting As String = DirectCast(validationSetting.GetValue("HankakuAlphabetFormat"), String)

            ''正規表現パターンが存在しない場合、比較できないのでTrueを返す
            If String.IsNullOrEmpty(regexSetting) Then
                Return True
            End If

            If Regex.IsMatch(target, regexSetting) Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "IsHankakuEisu"
        ''' <summary>
        ''' 半角英数文字列の判別を行います
        ''' </summary>
        ''' <param name="target">検証対象の文字列</param>
        ''' <returns>
        ''' 検証対象の文字列が半角英数文字列の場合はTrue、それ以外の場合はFalse。
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' パラメータの検証対象文字列がnull参照（VBではNothing）。
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[機能詳細]</b></para>
        ''' <para>検証対象が半角英数文字列であるかを検証します。</para>
        ''' <para>半角英数文字列の場合は、<c><b>True</b></c></para>
        ''' <para>半角英数文字列以外の文字列の場合は、<c><b>False</b></c></para>
        ''' <para>　</para>
        ''' <para><b>[注意事項]</b></para>
        ''' <para>デフォルトでは正規表現パターン「^[a-zA-Z0-9]+$」で検証を行っています。</para>
        ''' <para>
        ''' なお、この値は外部構成ファイルの「HankakuEisuFormat」要素の値を変更することによって変更することが可能です。
        ''' </para>
        ''' <para>
        ''' デフォルトの正規表現パターンでは検索対象の文字列が空文字（"")の場合、戻り値はFalseになります。
        ''' </para>
        ''' <para>　</para>
        ''' <para><b>[サンプル]</b></para>
        ''' <example>
        ''' <code>
        '''      Private Sub ValidationTest()
        '''
        '''          Dim targetString As String = "abcde"
        '''          Dim isValid As Boolean = Validation.IsHankakuEisu(targetString)
        '''
        '''     End Sub        
        ''' </code>
        ''' </example>
        ''' </remarks>
        Public Shared Function IsHankakuEisu(ByVal target As String) As Boolean

            If String.IsNullOrEmpty(target) Then
                Return False
            End If

            Dim validationClass As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = _
                    SystemConfiguration.Current.Manager.Validation

            Dim validationSetting As Toyota.eCRB.SystemFrameworks.Configuration.Setting = _
                    validationClass.GetSetting(String.Empty)

            Dim regexSetting As String = DirectCast(validationSetting.GetValue("HankakuEisuFormat"), String)

            ''正規表現パターンが存在しない場合、比較できないのでTrueを返す
            If String.IsNullOrEmpty(regexSetting) Then
                Return True
            End If

            If Regex.IsMatch(target, regexSetting) Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "IsHankakuNumber"
        ''' <summary>
        ''' 半角数文字列の判別を行います
        ''' </summary>
        ''' <param name="target">検証対象の文字列</param>
        ''' <returns>
        ''' 検証対象の文字列が半角数文字列の場合はTrue、それ以外の場合はFalse。
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' パラメータの検証対象文字列がnull参照（VBではNothing）。
        ''' </exception>
        Public Shared Function IsHankakuNumber(ByVal target As String) As Boolean

            If String.IsNullOrEmpty(target) Then
                Return False
            End If

            Dim validationClass As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = _
                    SystemConfiguration.Current.Manager.Validation

            Dim validationSetting As Toyota.eCRB.SystemFrameworks.Configuration.Setting = _
                    validationClass.GetSetting(String.Empty)

            Dim regexSetting As String = DirectCast(validationSetting.GetValue("HankakuNumberFormat"), String)

            ''正規表現パターンが存在しない場合、比較できないのでTrueを返す
            If String.IsNullOrEmpty(regexSetting) Then
                Return True
            End If

            If Regex.IsMatch(target, regexSetting) Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "IsMail"
        ''' <summary>
        ''' メールアドレスの判別を行います
        ''' </summary>
        ''' <param name="target">検証対象の文字列</param>
        ''' <returns>
        ''' 検証対象の文字列がメールアドレスの場合はTrue、それ以外の場合はFalse。
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' パラメータの検証対象文字列がnull参照（VBではNothing）。
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[機能詳細]</b></para>
        ''' <para>検証対象がメールアドレスであるかを検証します。</para>
        ''' <para>メールアドレスの場合は、<c><b>True</b></c></para>
        ''' <para>メールアドレス以外の書式の場合は、<c><b>False</b></c></para>
        ''' <para>　</para>
        ''' <para><b>[注意事項]</b></para>
        ''' <para>デフォルトでは正規表現パターン「\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*」で検証を行っています。</para>
        ''' <para>
        ''' なお、この値は外部構成ファイルの「MailAddress」要素の値を変更することによって変更することが可能です。
        ''' </para>
        ''' <para>
        ''' デフォルトの正規表現パターンでは検索対象の文字列が空文字（"")の場合、戻り値はFalseになります。
        ''' </para>
        ''' <para>　</para>
        ''' <para><b>[サンプル]</b></para>
        ''' <example>
        ''' <code>
        '''      Private Sub ValidationTest()
        '''
        '''          Dim targetString As String = "abcde@microsoft.com"
        '''          Dim isValid As Boolean = Validation.IsMail(targetString)
        '''
        '''     End Sub        
        ''' </code>
        ''' </example>
        ''' </remarks>
        Public Shared Function IsMail(ByVal target As String) As Boolean

            If String.IsNullOrEmpty(target) Then
                Return False
            End If

            Dim validationClass As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = _
                    SystemConfiguration.Current.Manager.Validation

            Dim validationSetting As Toyota.eCRB.SystemFrameworks.Configuration.Setting = _
                    validationClass.GetSetting(String.Empty)

            Dim regexSetting As String = DirectCast(validationSetting.GetValue("MailAddressFormat"), String)

            ''正規表現パターンが存在しない場合、比較できないのでTrueを返す
            If String.IsNullOrEmpty(regexSetting) Then
                Return True
            End If

            If Regex.IsMatch(target, regexSetting) Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "IsCorrectDigit"
        ''' <summary>
        ''' 指定対象が指定の文字数以内かを検証します
        ''' </summary>
        ''' <param name="target">検証対象の文字列</param>
        ''' <param name="thresholdDigit">桁数の閾値</param>
        ''' <returns>
        ''' 検証対象の文字列の文字数が閾値以下の場合はTrue、それ以外の場合はFalse。
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' パラメータの検証対象文字列がnull参照（VBではNothing）。
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[機能詳細]</b></para>
        ''' <para>検証対象が桁数の閾値以内の検証を行います。</para>
        ''' <para>閾値以下の場合は、<c><b>True</b></c></para>
        ''' <para>閾値より大きい場合は、<c><b>False</b></c></para>
        ''' <para>　</para>
        ''' <para><b>[注意事項]</b></para>
        ''' <para>閾値が0より小さい場合、戻り値は常にFalseになります（検索対象がnullの場合を除く）。</para>
        ''' <para>パラメータとして指摘できる閾値は最大値は 2,147,483,647（Int32型の最大値、<see cref="Int32.MaxValue" />）まで指定可能。</para>
        ''' <para><b>[サンプル]</b></para>
        ''' <example>
        ''' <code>
        '''      Private Sub ValidationTest()
        '''
        '''          Dim targetString As String = "microsoft"
        '''          Dim isValid As Boolean = Validation.IsCorrectDigit(targetString, 10)
        '''
        '''     End Sub        
        ''' </code>
        ''' </example>
        ''' </remarks>
        Public Shared Function IsCorrectDigit(ByVal target As String, ByVal thresholdDigit As Integer) As Boolean

            If String.IsNullOrEmpty(target) Then
                Return False
            End If

            If thresholdDigit <= 0 Then
                Return False
            End If

            If (target.Length <= thresholdDigit) Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "IsCorrectByte"
        ''' <summary>
        ''' 指定対象が指定のバイト数以内かを検証します
        ''' </summary>
        ''' <param name="target">検証対象の文字列</param>
        ''' <param name="thresholdByte">バイト数の閾値</param>
        ''' <returns>
        ''' 検証対象の文字列のバイト数が閾値以下の場合はTrue、それ以外の場合はFalse。
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' パラメータの検証対象文字列がnull参照（VBではNothing）。
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[機能詳細]</b></para>
        ''' <para>検証対象がバイト数の閾値以内の検証を行います</para>
        ''' <para>閾値以下の場合は、<c><b>True</b></c></para>
        ''' <para>閾値より大きい場合は、<c><b>False</b></c></para>
        ''' <para>　</para>
        ''' <para><b>[注意事項]</b></para>
        ''' <para>バイト数の判定は文字列をUTF-8に変換し判定を行っています。</para>
        ''' <para>パラメータの閾値が0より小さい場合、戻り値は常にFalseになります（検索対象がnullの場合を除く）。</para>
        ''' <para>パラメータとして指摘できる閾値は最大値は 2,147,483,647（Int32型の最大値、<see cref="Int32.MaxValue" />）まで指定可能。</para>
        ''' <para>　</para>
        ''' <para><b>[サンプル]</b></para>
        ''' <example>
        ''' <code>
        '''      Private Sub ValidationTest()
        '''
        '''          Dim targetString As String = "microsoft"
        '''          Dim isValid As Boolean = Validation.IsCorrectByte(targetString, 10)
        '''
        '''     End Sub        
        ''' </code>
        ''' </example>
        ''' </remarks>
        <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", Scope:="member", Justification:="Byte数を返すメソッドなのでByteが名前に入っていても問題ない")> _
        Public Shared Function IsCorrectByte(ByVal target As String, ByVal thresholdByte As Integer) As Boolean

            If String.IsNullOrEmpty(target) Then
                Return False
            End If

            If thresholdByte <= 0 Then
                Return False
            End If

            Dim charcode As Text.Encoding = Text.Encoding.GetEncoding("utf-8")
            If (charcode.GetByteCount(target) <= thresholdByte) Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "IsCorrectPattern"
        ''' <summary>
        ''' 検証対象が正規表現パターンかを検証します
        ''' </summary>
        ''' <param name="target">検証対象の文字列</param>
        ''' <param name="pattern">正規表現パターンの文字列</param>
        ''' <returns>
        ''' 検証対象の文字列が正規表現パターンに一致する場合はTrue、それ以外の場合はFalse。
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' <para>パラメータの検証対象文字列がnull参照（VBではNothing）。</para>
        ''' <para>パラメータの正規表現パターン文字列がnull参照（VBではNothing）。</para>
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[機能詳細]</b></para>
        ''' <para>検証対象が正規表現パターンに一致するかを検証します</para>
        ''' <para>正規表現パターンと一致する場合は、<c><b>True</b></c></para>
        ''' <para>正規表現パターンと一致しない場合は、<c><b>False</b></c></para>
        ''' <para>　</para>
        ''' <para><b>[注意事項]</b></para>
        ''' <para>特になし</para>
        ''' <para>　</para>
        ''' <para><b>[サンプル]</b></para>
        ''' <example>
        ''' <code>
        '''      Private Sub ValidationTest()
        '''
        '''          Dim targetString As String = "1"
        '''          Dim isValid As Boolean = Validation.IsCorrectPattern(targetString, "^d{1}$")
        '''
        '''     End Sub        
        ''' </code>
        ''' </example>
        ''' </remarks>
        Public Shared Function IsCorrectPattern(ByVal target As String, ByVal pattern As String) As Boolean
            If Regex.IsMatch(target, pattern) Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "IsContainTag"
        ''' <summary>
        ''' 指定の文字列に禁則文字を含むかを検証します。
        ''' </summary>
        ''' <param name="target">検証対象の文字列</param>
        ''' <returns>検証対象の文字列に正規表現パターンに一致する文字が含まれない場合はTrue、含まれる場合はFalse</returns>
        ''' <exception cref="ArgumentNullException">
        ''' パラメータの検証対象文字列がnull参照（VBではNothing）。
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[機能詳細]</b></para>
        ''' <para>検証対象に禁則文字を含むかの検証を行います</para>
        ''' <para>含む場合は、<c><b>True</b></c></para>
        ''' <para>含まない場合は、<c><b>False</b></c></para>
        ''' <para>　</para>
        ''' <para><b>[注意事項]</b></para>
        ''' <para>デフォルトでは正規表現パターン「&lt;[a-zA-Z0-9]」で検証を行っています。</para>
        ''' <para>
        ''' なお、この値は外部構成ファイルの「KinsokuFormat」要素の値を変更することによって変更することが可能です。
        ''' </para>
        ''' <para>
        ''' デフォルトの正規表現パターンでは検索対象の文字列が空文字（"")の場合、戻り値はFalseになります。
        ''' </para>
        ''' <para><b>[サンプル]</b></para>
        ''' <example>
        ''' <code>
        '''      Private Sub ValidationTest()
        '''
        '''          Dim targetString As String = "microsoft"
        '''          Dim isValid As Boolean = Validation.IsContainKinsoku(targetString)
        '''
        '''     End Sub        
        ''' </code>
        ''' </example>
        ''' </remarks>
        Public Shared Function IsContainTag(ByVal target As String) As Boolean

            If String.IsNullOrEmpty(target) Then
                Return False
            End If

            Dim validationClass As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = _
                    SystemConfiguration.Current.Manager.Validation

            Dim validationSetting As Toyota.eCRB.SystemFrameworks.Configuration.Setting = _
                    validationClass.GetSetting(String.Empty)

            Dim regexSetting As String = DirectCast(validationSetting.GetValue("TagFormat"), String)

            ''正規表現パターンが存在しない場合、比較できないのでTrueを返す
            If String.IsNullOrEmpty(regexSetting) Then
                Return True
            End If

            If Regex.IsMatch(target, regexSetting) Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "IsDate"
        ''' <summary>
        ''' 指定の文字列が有効な日付かを検証します。
        ''' </summary>
        ''' <param name="convID">
        ''' 検証対象の日付型
        ''' </param>  
        ''' <param name="target">検証対象の文字列</param>
        ''' <returns>検証対象が変換可能な場合はTrue、不可能な場合はFalse</returns>
        ''' <exception cref="ArgumentNullException">
        ''' パラメータの検証対象文字列がnull参照（VBではNothing）。
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[機能詳細]</b></para>
        ''' <para>指定の文字列が有効な日付かの検証を行います</para>
        ''' <para>有効な日付の場合は、<c><b>True</b></c></para>
        ''' <para>有効な日付ではない場合は、<c><b>False</b></c></para>
        ''' <para><b>[サンプル]</b></para>
        ''' <example>
        ''' <code>
        '''      Private Sub ValidationTest()
        '''
        '''          Dim targetString As String = "2011/08/01"
        '''          Dim isValid As Boolean = Validation.IsDate(3,targetString)
        '''
        '''     End Sub        
        ''' </code>
        ''' </example>
        ''' </remarks>
        Public Shared Function IsDate(ByVal convId As Integer, ByVal target As String) As Boolean

            Dim formtdate As String = DateTimeForm.GetDateTimeForm(convId)

            If String.IsNullOrEmpty(formtdate) Then
                Return False
            End If

            'IsDate = False

            'If String.IsNullOrEmpty(CStr(kind)) Then
            '    Return False
            'End If

            '文字数の確認
            If Not Len(Replace(formtdate, "%1", "2000")) = Len(target) Then
                Return False
            End If

            '初期化
            Dim year As String = "2000"     '年
            Dim month As String = "01"      '月
            Dim day As String = "01"        '日
            Dim hour As String = "00"       '時
            Dim minute As String = "00"     '分
            Dim second As String = "00"     '秒
            Dim lenposition As Integer = 1  'チェック位置
            Dim i As Integer = 1

            Do Until i >= Len(formtdate)
                If String.Equals(Mid(formtdate, i, 1), "%") Then
                    Select Case Mid(formtdate, i, 2)
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
                    If String.Equals(Mid(formtdate, i, 1), Mid(target, lenposition, 1)) Then
                    Else
                        Return False
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

            If Microsoft.VisualBasic.IsDate(tempdate.ToString()) = False Then
                Return False
            End If

            Return True

        End Function
#End Region

#Region "IsRegNo"
        ''' <summary>
        ''' RegNoの判別を行います
        ''' </summary>
        ''' <param name="target">検証対象の文字列</param>
        ''' <returns>
        ''' 検証対象の文字列がRegNoの場合はTrue、それ以外の場合はFalse。
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' パラメータの検証対象文字列がnull参照（VBではNothing）。
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[機能詳細]</b></para>
        ''' <para>検証対象がRegNoであるかを検証します。</para>
        ''' <para>RegNoの場合は、<c><b>True</b></c></para>
        ''' <para>RegNo以外の書式の場合は、<c><b>False</b></c></para>
        ''' <para>　</para>
        ''' <para><b>[注意事項]</b></para>
        ''' <para>デフォルトでは正規表現パターン「\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*」で検証を行っています。</para>
        ''' <para>
        ''' なお、この値は外部構成ファイルの「RegNoFormat」要素の値を変更することによって変更することが可能です。
        ''' </para>
        ''' <para>
        ''' デフォルトの正規表現パターンでは検索対象の文字列が空文字（"")の場合、戻り値はFalseになります。
        ''' </para>
        ''' </remarks>
        Public Shared Function IsRegNo(ByVal target As String) As Boolean

            If String.IsNullOrEmpty(target) Then
                Return False
            End If

            Dim validationClass As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = _
                    SystemConfiguration.Current.Manager.Validation

            Dim validationSetting As Toyota.eCRB.SystemFrameworks.Configuration.Setting = _
                    validationClass.GetSetting(String.Empty)

            Dim regexSetting As String = DirectCast(validationSetting.GetValue("RegNoFormat"), String)

            ''正規表現パターンが存在しない場合、比較できないのでTrueを返す
            If String.IsNullOrEmpty(regexSetting) Then
                Return True
            End If

            If Regex.IsMatch(target, regexSetting) Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "IsVin"
        ''' <summary>
        ''' Vinの判別を行います
        ''' </summary>
        ''' <param name="target">検証対象の文字列</param>
        ''' <returns>
        ''' 検証対象の文字列がVinの場合はTrue、それ以外の場合はFalse。
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' パラメータの検証対象文字列がnull参照（VBではNothing）。
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[機能詳細]</b></para>
        ''' <para>検証対象がVinであるかを検証します。</para>
        ''' <para>Vinの場合は、<c><b>True</b></c></para>
        ''' <para>Vin以外の書式の場合は、<c><b>False</b></c></para>
        ''' <para>　</para>
        ''' <para><b>[注意事項]</b></para>
        ''' <para>デフォルトでは正規表現パターン「\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*」で検証を行っています。</para>
        ''' <para>
        ''' なお、この値は外部構成ファイルの「VinFormat」要素の値を変更することによって変更することが可能です。
        ''' </para>
        ''' <para>
        ''' デフォルトの正規表現パターンでは検索対象の文字列が空文字（"")の場合、戻り値はFalseになります。
        ''' </para>
        ''' </remarks>
        Public Shared Function IsVin(ByVal target As String) As Boolean

            If String.IsNullOrEmpty(target) Then
                Return False
            End If

            Dim validationClass As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = _
                    SystemConfiguration.Current.Manager.Validation

            Dim validationSetting As Toyota.eCRB.SystemFrameworks.Configuration.Setting = _
                    validationClass.GetSetting(String.Empty)

            Dim regexSetting As String = DirectCast(validationSetting.GetValue("VinFormat"), String)

            ''正規表現パターンが存在しない場合、比較できないのでTrueを返す
            If String.IsNullOrEmpty(regexSetting) Then
                Return True
            End If

            If Regex.IsMatch(target, regexSetting) Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "IsPhoneNumber"
        ''' <summary>
        ''' PhoneNumberの判別を行います
        ''' </summary>
        ''' <param name="target">検証対象の文字列</param>
        ''' <returns>
        ''' 検証対象の文字列がPhoneNumberの場合はTrue、それ以外の場合はFalse。
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' パラメータの検証対象文字列がnull参照（VBではNothing）。
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[機能詳細]</b></para>
        ''' <para>検証対象がPhoneNumberであるかを検証します。</para>
        ''' <para>PhoneNumberの場合は、<c><b>True</b></c></para>
        ''' <para>PhoneNumber以外の書式の場合は、<c><b>False</b></c></para>
        ''' <para>　</para>
        ''' <para><b>[注意事項]</b></para>
        ''' <para>デフォルトでは正規表現パターン「\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*」で検証を行っています。</para>
        ''' <para>
        ''' なお、この値は外部構成ファイルの「PhoneNumberFormat」要素の値を変更することによって変更することが可能です。
        ''' </para>
        ''' <para>
        ''' デフォルトの正規表現パターンでは検索対象の文字列が空文字（"")の場合、戻り値はFalseになります。
        ''' </para>
        ''' </remarks>
        Public Shared Function IsPhoneNumber(ByVal target As String) As Boolean

            If String.IsNullOrEmpty(target) Then
                Return False
            End If

            Dim validationClass As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = _
                    SystemConfiguration.Current.Manager.Validation

            Dim validationSetting As Toyota.eCRB.SystemFrameworks.Configuration.Setting = _
                    validationClass.GetSetting(String.Empty)

            Dim regexSetting As String = DirectCast(validationSetting.GetValue("PhoneNumberFormat"), String)

            ''正規表現パターンが存在しない場合、比較できないのでTrueを返す
            If String.IsNullOrEmpty(regexSetting) Then
                Return True
            End If

            If Regex.IsMatch(target, regexSetting) Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "IsMobilePhoneNumber"
        ''' <summary>
        ''' MobilePhoneNumberの判別を行います
        ''' </summary>
        ''' <param name="target">検証対象の文字列</param>
        ''' <returns>
        ''' 検証対象の文字列がMobilePhoneNumberの場合はTrue、それ以外の場合はFalse。
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' パラメータの検証対象文字列がnull参照（VBではNothing）。
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[機能詳細]</b></para>
        ''' <para>検証対象がMobilePhoneNumberであるかを検証します。</para>
        ''' <para>MobilePhoneNumberの場合は、<c><b>True</b></c></para>
        ''' <para>MobilePhoneNumber以外の書式の場合は、<c><b>False</b></c></para>
        ''' <para>　</para>
        ''' <para><b>[注意事項]</b></para>
        ''' <para>デフォルトでは正規表現パターン「\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*」で検証を行っています。</para>
        ''' <para>
        ''' なお、この値は外部構成ファイルの「MobilePhoneNumberFormat」要素の値を変更することによって変更することが可能です。
        ''' </para>
        ''' <para>
        ''' デフォルトの正規表現パターンでは検索対象の文字列が空文字（"")の場合、戻り値はFalseになります。
        ''' </para>
        ''' </remarks>
        Public Shared Function IsMobilePhoneNumber(ByVal target As String) As Boolean

            If String.IsNullOrEmpty(target) Then
                Return False
            End If

            Dim validationClass As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = _
                    SystemConfiguration.Current.Manager.Validation

            Dim validationSetting As Toyota.eCRB.SystemFrameworks.Configuration.Setting = _
                    validationClass.GetSetting(String.Empty)

            Dim regexSetting As String = DirectCast(validationSetting.GetValue("MobilePhoneNumberFormat"), String)

            ''正規表現パターンが存在しない場合、比較できないのでTrueを返す
            If String.IsNullOrEmpty(regexSetting) Then
                Return True
            End If

            If Regex.IsMatch(target, regexSetting) Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "IsPostalCode"
        ''' <summary>
        ''' PostalCodeの判別を行います
        ''' </summary>
        ''' <param name="target">検証対象の文字列</param>
        ''' <returns>
        ''' 検証対象の文字列がPostalCodeの場合はTrue、それ以外の場合はFalse。
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' パラメータの検証対象文字列がnull参照（VBではNothing）。
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[機能詳細]</b></para>
        ''' <para>検証対象がPostalCodeであるかを検証します。</para>
        ''' <para>PostalCodeの場合は、<c><b>True</b></c></para>
        ''' <para>PostalCode以外の書式の場合は、<c><b>False</b></c></para>
        ''' <para>　</para>
        ''' <para><b>[注意事項]</b></para>
        ''' <para>デフォルトでは正規表現パターン「\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*」で検証を行っています。</para>
        ''' <para>
        ''' なお、この値は外部構成ファイルの「PostalCodeFormat」要素の値を変更することによって変更することが可能です。
        ''' </para>
        ''' <para>
        ''' デフォルトの正規表現パターンでは検索対象の文字列が空文字（"")の場合、戻り値はFalseになります。
        ''' </para>
        ''' </remarks>
        Public Shared Function IsPostalCode(ByVal target As String) As Boolean

            If String.IsNullOrEmpty(target) Then
                Return False
            End If

            Dim validationClass As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection = _
                    SystemConfiguration.Current.Manager.Validation

            Dim validationSetting As Toyota.eCRB.SystemFrameworks.Configuration.Setting = _
                    validationClass.GetSetting(String.Empty)

            Dim regexSetting As String = DirectCast(validationSetting.GetValue("PostalCodeFormat"), String)

            ''正規表現パターンが存在しない場合、比較できないのでTrueを返す
            If String.IsNullOrEmpty(regexSetting) Then
                Return True
            End If

            If Regex.IsMatch(target, regexSetting) Then
                Return True
            Else
                Return False
            End If

        End Function
#End Region

#Region "IsValidString"
        ''' <summary>
        ''' 文字列の中に、システムが定める禁止文字が含まれていないか判断します。
        ''' </summary>
        ''' <param name="target">書式チェック対象の文字列</param>
        ''' <returns>True:禁止文字列は含まれていない False:禁止文字列が含まれている (Targetが空文字の場合は、Trueを返します)</returns>
        ''' <remarks>文字列の中に、システムが定める禁止文字が含まれていないか判断します。</remarks>
        Public Shared Function IsValidString(ByVal target As String) As Boolean
            ''対象が空なので終了
            If String.IsNullOrEmpty(target) Then
                Return True
            End If

            ''禁止書式がないので終了
            Dim supress As List(Of String()) = SuppressionCharList
            If supress.Count = 0 Then
                Return True
            End If

            Dim chars As Char() = target.ToCharArray()
            Dim charCode As Integer = 0

            For i = 0 To target.Length - 1

                ''サロゲートペアの文字か確認
                If Char.IsSurrogate(target, i) Then
                    ''サロゲートペアのUnicodeポイントを取得
                    charCode = Char.ConvertToUtf32(chars(i), chars(i + 1))
                    ''サロゲートペアの1文字で2文字分(2Byte+2Byte)使用するのでカウンタを一個進める
                    i = i + 1
                Else
                    charCode = AscW(chars(i))
                End If

                For Each suppressChar In supress

                    If suppressChar.Count = 1 Then
                        ''設定が個別指定の場合は一致か確認
                        If Convert.ToInt32(suppressChar(0), 16) = charCode Then
                            Return False
                        End If
                    Else
                        ''設定が範囲指定の場合は大小関係で比較
                        If Convert.ToInt32(suppressChar(0), 16) <= charCode AndAlso charCode <= Convert.ToInt32(suppressChar(1), 16) Then
                            Return False
                        End If
                    End If
                Next suppressChar

            Next i

            Return True

        End Function
#End Region

    End Class

End Namespace
