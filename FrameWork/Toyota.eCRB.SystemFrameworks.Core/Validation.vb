'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.

Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Diagnostics.CodeAnalysis
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' �f�[�^���؋@�\��񋟂��܂��B
    ''' </summary>
    ''' <remarks>
    ''' ���̃N���X�̓C���X�^���X�𐶐��ł��܂���B�ÓI���\�b�h���Ăяo���Ă��������B
    ''' </remarks>
    Public NotInheritable Class Validation

        Private Const C_INVALIDCHARACTER_SETTING As String = "InvalidCharacters"
        Private Shared _suppressionCharList As List(Of String()) = Nothing
        ''' <summary>
        ''' �R���X�g���N�^
        ''' </summary>
        ''' <remarks>�C���X�^���X����}�~</remarks>
        Private Sub New()
        End Sub

#Region "�v���p�e�B"
        ''' <summary>
        ''' �֎~�����m�F�ݒ�
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

                    Dim check As New StringBuilder  ''�ݒ�̒l���L�q����Ă��邩�̊m�F�p

                    Dim tempSuppress As New List(Of String())
                    ''�ݒ萔���[�v
                    For Each item In validationSetting.Item
                        ''�J���}�ŕ����I������Ă���̂𕪉�
                        Dim charSets() As String = item.Value.ToString.Split(","c)
                        For Each value In charSets
                            ''-�Ŕ͈͎w�肵�Ă���̂𕪉�
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

                    ''�ݒ�̒l���L�q����Ă��Ȃ��̂ŏI��
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
        ''' ���p�A���t�@�x�b�g������̔��ʂ��s���܂�
        ''' </summary>
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <returns>
        ''' ���ؑΏۂ̕����񂪔��p�A���t�@�x�b�g������̏ꍇ��True�A����ȊO�̏ꍇ��False�B
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' �p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B
        ''' </exception>
        ''' <remarks>
        ''' <para><B>[�@�\�ڍ�]</B></para>
        ''' <para>���ؑΏۂ����p�A���t�@�x�b�g������ł��邩�����؂��܂��B</para>
        ''' <para>���p�A���t�@�x�b�g������̏ꍇ�́A<c><b>True</b></c></para>
        ''' <para>���p�A���t�@�x�b�g������ȊO�̕�����̏ꍇ�́A<c><b>False</b></c></para>
        ''' <para>�@</para>
        ''' <para><b>[���ӎ���]</b></para>
        ''' <para>�f�t�H���g�ł͐��K�\���p�^�[���u^[a-zA-Z]+$�v�Ō��؂��s���Ă��܂��B</para>
        ''' <para>
        ''' �Ȃ��A���̒l�͊O���\���t�@�C���́uHankakuAlphabetFormat�v�v�f�̒l��ύX���邱�Ƃɂ���ĕύX���邱�Ƃ��\�ł��B
        ''' </para>
        ''' <para>
        ''' �f�t�H���g�̐��K�\���p�^�[���ł͌����Ώۂ̕����񂪋󕶎��i"")�̏ꍇ�A�߂�l��False�ɂȂ�܂��B
        ''' </para>
        ''' <para>�@</para>
        ''' <para><b>[�T���v��]</b></para>
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

            ''���K�\���p�^�[�������݂��Ȃ��ꍇ�A��r�ł��Ȃ��̂�True��Ԃ�
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
        ''' ���p�p��������̔��ʂ��s���܂�
        ''' </summary>
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <returns>
        ''' ���ؑΏۂ̕����񂪔��p�p��������̏ꍇ��True�A����ȊO�̏ꍇ��False�B
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' �p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[�@�\�ڍ�]</b></para>
        ''' <para>���ؑΏۂ����p�p��������ł��邩�����؂��܂��B</para>
        ''' <para>���p�p��������̏ꍇ�́A<c><b>True</b></c></para>
        ''' <para>���p�p��������ȊO�̕�����̏ꍇ�́A<c><b>False</b></c></para>
        ''' <para>�@</para>
        ''' <para><b>[���ӎ���]</b></para>
        ''' <para>�f�t�H���g�ł͐��K�\���p�^�[���u^[a-zA-Z0-9]+$�v�Ō��؂��s���Ă��܂��B</para>
        ''' <para>
        ''' �Ȃ��A���̒l�͊O���\���t�@�C���́uHankakuEisuFormat�v�v�f�̒l��ύX���邱�Ƃɂ���ĕύX���邱�Ƃ��\�ł��B
        ''' </para>
        ''' <para>
        ''' �f�t�H���g�̐��K�\���p�^�[���ł͌����Ώۂ̕����񂪋󕶎��i"")�̏ꍇ�A�߂�l��False�ɂȂ�܂��B
        ''' </para>
        ''' <para>�@</para>
        ''' <para><b>[�T���v��]</b></para>
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

            ''���K�\���p�^�[�������݂��Ȃ��ꍇ�A��r�ł��Ȃ��̂�True��Ԃ�
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
        ''' ���p��������̔��ʂ��s���܂�
        ''' </summary>
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <returns>
        ''' ���ؑΏۂ̕����񂪔��p��������̏ꍇ��True�A����ȊO�̏ꍇ��False�B
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' �p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B
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

            ''���K�\���p�^�[�������݂��Ȃ��ꍇ�A��r�ł��Ȃ��̂�True��Ԃ�
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
        ''' ���[���A�h���X�̔��ʂ��s���܂�
        ''' </summary>
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <returns>
        ''' ���ؑΏۂ̕����񂪃��[���A�h���X�̏ꍇ��True�A����ȊO�̏ꍇ��False�B
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' �p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[�@�\�ڍ�]</b></para>
        ''' <para>���ؑΏۂ����[���A�h���X�ł��邩�����؂��܂��B</para>
        ''' <para>���[���A�h���X�̏ꍇ�́A<c><b>True</b></c></para>
        ''' <para>���[���A�h���X�ȊO�̏����̏ꍇ�́A<c><b>False</b></c></para>
        ''' <para>�@</para>
        ''' <para><b>[���ӎ���]</b></para>
        ''' <para>�f�t�H���g�ł͐��K�\���p�^�[���u\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*�v�Ō��؂��s���Ă��܂��B</para>
        ''' <para>
        ''' �Ȃ��A���̒l�͊O���\���t�@�C���́uMailAddress�v�v�f�̒l��ύX���邱�Ƃɂ���ĕύX���邱�Ƃ��\�ł��B
        ''' </para>
        ''' <para>
        ''' �f�t�H���g�̐��K�\���p�^�[���ł͌����Ώۂ̕����񂪋󕶎��i"")�̏ꍇ�A�߂�l��False�ɂȂ�܂��B
        ''' </para>
        ''' <para>�@</para>
        ''' <para><b>[�T���v��]</b></para>
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

            ''���K�\���p�^�[�������݂��Ȃ��ꍇ�A��r�ł��Ȃ��̂�True��Ԃ�
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
        ''' �w��Ώۂ��w��̕������ȓ��������؂��܂�
        ''' </summary>
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <param name="thresholdDigit">������臒l</param>
        ''' <returns>
        ''' ���ؑΏۂ̕�����̕�������臒l�ȉ��̏ꍇ��True�A����ȊO�̏ꍇ��False�B
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' �p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[�@�\�ڍ�]</b></para>
        ''' <para>���ؑΏۂ�������臒l�ȓ��̌��؂��s���܂��B</para>
        ''' <para>臒l�ȉ��̏ꍇ�́A<c><b>True</b></c></para>
        ''' <para>臒l���傫���ꍇ�́A<c><b>False</b></c></para>
        ''' <para>�@</para>
        ''' <para><b>[���ӎ���]</b></para>
        ''' <para>臒l��0��菬�����ꍇ�A�߂�l�͏��False�ɂȂ�܂��i�����Ώۂ�null�̏ꍇ�������j�B</para>
        ''' <para>�p�����[�^�Ƃ��Ďw�E�ł���臒l�͍ő�l�� 2,147,483,647�iInt32�^�̍ő�l�A<see cref="Int32.MaxValue" />�j�܂Ŏw��\�B</para>
        ''' <para><b>[�T���v��]</b></para>
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
        ''' �w��Ώۂ��w��̃o�C�g���ȓ��������؂��܂�
        ''' </summary>
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <param name="thresholdByte">�o�C�g����臒l</param>
        ''' <returns>
        ''' ���ؑΏۂ̕�����̃o�C�g����臒l�ȉ��̏ꍇ��True�A����ȊO�̏ꍇ��False�B
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' �p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[�@�\�ڍ�]</b></para>
        ''' <para>���ؑΏۂ��o�C�g����臒l�ȓ��̌��؂��s���܂�</para>
        ''' <para>臒l�ȉ��̏ꍇ�́A<c><b>True</b></c></para>
        ''' <para>臒l���傫���ꍇ�́A<c><b>False</b></c></para>
        ''' <para>�@</para>
        ''' <para><b>[���ӎ���]</b></para>
        ''' <para>�o�C�g���̔���͕������UTF-8�ɕϊ���������s���Ă��܂��B</para>
        ''' <para>�p�����[�^��臒l��0��菬�����ꍇ�A�߂�l�͏��False�ɂȂ�܂��i�����Ώۂ�null�̏ꍇ�������j�B</para>
        ''' <para>�p�����[�^�Ƃ��Ďw�E�ł���臒l�͍ő�l�� 2,147,483,647�iInt32�^�̍ő�l�A<see cref="Int32.MaxValue" />�j�܂Ŏw��\�B</para>
        ''' <para>�@</para>
        ''' <para><b>[�T���v��]</b></para>
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
        <SuppressMessage("Microsoft.Naming", "CA1720:AvoidTypeNamesInParameters", Scope:="member", Justification:="Byte����Ԃ����\�b�h�Ȃ̂�Byte�����O�ɓ����Ă��Ă����Ȃ�")> _
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
        ''' ���ؑΏۂ����K�\���p�^�[���������؂��܂�
        ''' </summary>
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <param name="pattern">���K�\���p�^�[���̕�����</param>
        ''' <returns>
        ''' ���ؑΏۂ̕����񂪐��K�\���p�^�[���Ɉ�v����ꍇ��True�A����ȊO�̏ꍇ��False�B
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' <para>�p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B</para>
        ''' <para>�p�����[�^�̐��K�\���p�^�[��������null�Q�ƁiVB�ł�Nothing�j�B</para>
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[�@�\�ڍ�]</b></para>
        ''' <para>���ؑΏۂ����K�\���p�^�[���Ɉ�v���邩�����؂��܂�</para>
        ''' <para>���K�\���p�^�[���ƈ�v����ꍇ�́A<c><b>True</b></c></para>
        ''' <para>���K�\���p�^�[���ƈ�v���Ȃ��ꍇ�́A<c><b>False</b></c></para>
        ''' <para>�@</para>
        ''' <para><b>[���ӎ���]</b></para>
        ''' <para>���ɂȂ�</para>
        ''' <para>�@</para>
        ''' <para><b>[�T���v��]</b></para>
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
        ''' �w��̕�����ɋ֑��������܂ނ������؂��܂��B
        ''' </summary>
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <returns>���ؑΏۂ̕�����ɐ��K�\���p�^�[���Ɉ�v���镶�����܂܂�Ȃ��ꍇ��True�A�܂܂��ꍇ��False</returns>
        ''' <exception cref="ArgumentNullException">
        ''' �p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[�@�\�ڍ�]</b></para>
        ''' <para>���ؑΏۂɋ֑��������܂ނ��̌��؂��s���܂�</para>
        ''' <para>�܂ޏꍇ�́A<c><b>True</b></c></para>
        ''' <para>�܂܂Ȃ��ꍇ�́A<c><b>False</b></c></para>
        ''' <para>�@</para>
        ''' <para><b>[���ӎ���]</b></para>
        ''' <para>�f�t�H���g�ł͐��K�\���p�^�[���u&lt;[a-zA-Z0-9]�v�Ō��؂��s���Ă��܂��B</para>
        ''' <para>
        ''' �Ȃ��A���̒l�͊O���\���t�@�C���́uKinsokuFormat�v�v�f�̒l��ύX���邱�Ƃɂ���ĕύX���邱�Ƃ��\�ł��B
        ''' </para>
        ''' <para>
        ''' �f�t�H���g�̐��K�\���p�^�[���ł͌����Ώۂ̕����񂪋󕶎��i"")�̏ꍇ�A�߂�l��False�ɂȂ�܂��B
        ''' </para>
        ''' <para><b>[�T���v��]</b></para>
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

            ''���K�\���p�^�[�������݂��Ȃ��ꍇ�A��r�ł��Ȃ��̂�True��Ԃ�
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
        ''' �w��̕����񂪗L���ȓ��t�������؂��܂��B
        ''' </summary>
        ''' <param name="convID">
        ''' ���ؑΏۂ̓��t�^
        ''' </param>  
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <returns>���ؑΏۂ��ϊ��\�ȏꍇ��True�A�s�\�ȏꍇ��False</returns>
        ''' <exception cref="ArgumentNullException">
        ''' �p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[�@�\�ڍ�]</b></para>
        ''' <para>�w��̕����񂪗L���ȓ��t���̌��؂��s���܂�</para>
        ''' <para>�L���ȓ��t�̏ꍇ�́A<c><b>True</b></c></para>
        ''' <para>�L���ȓ��t�ł͂Ȃ��ꍇ�́A<c><b>False</b></c></para>
        ''' <para><b>[�T���v��]</b></para>
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

            '�������̊m�F
            If Not Len(Replace(formtdate, "%1", "2000")) = Len(target) Then
                Return False
            End If

            '������
            Dim year As String = "2000"     '�N
            Dim month As String = "01"      '��
            Dim day As String = "01"        '��
            Dim hour As String = "00"       '��
            Dim minute As String = "00"     '��
            Dim second As String = "00"     '�b
            Dim lenposition As Integer = 1  '�`�F�b�N�ʒu
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
        ''' RegNo�̔��ʂ��s���܂�
        ''' </summary>
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <returns>
        ''' ���ؑΏۂ̕�����RegNo�̏ꍇ��True�A����ȊO�̏ꍇ��False�B
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' �p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[�@�\�ڍ�]</b></para>
        ''' <para>���ؑΏۂ�RegNo�ł��邩�����؂��܂��B</para>
        ''' <para>RegNo�̏ꍇ�́A<c><b>True</b></c></para>
        ''' <para>RegNo�ȊO�̏����̏ꍇ�́A<c><b>False</b></c></para>
        ''' <para>�@</para>
        ''' <para><b>[���ӎ���]</b></para>
        ''' <para>�f�t�H���g�ł͐��K�\���p�^�[���u\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*�v�Ō��؂��s���Ă��܂��B</para>
        ''' <para>
        ''' �Ȃ��A���̒l�͊O���\���t�@�C���́uRegNoFormat�v�v�f�̒l��ύX���邱�Ƃɂ���ĕύX���邱�Ƃ��\�ł��B
        ''' </para>
        ''' <para>
        ''' �f�t�H���g�̐��K�\���p�^�[���ł͌����Ώۂ̕����񂪋󕶎��i"")�̏ꍇ�A�߂�l��False�ɂȂ�܂��B
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

            ''���K�\���p�^�[�������݂��Ȃ��ꍇ�A��r�ł��Ȃ��̂�True��Ԃ�
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
        ''' Vin�̔��ʂ��s���܂�
        ''' </summary>
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <returns>
        ''' ���ؑΏۂ̕�����Vin�̏ꍇ��True�A����ȊO�̏ꍇ��False�B
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' �p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[�@�\�ڍ�]</b></para>
        ''' <para>���ؑΏۂ�Vin�ł��邩�����؂��܂��B</para>
        ''' <para>Vin�̏ꍇ�́A<c><b>True</b></c></para>
        ''' <para>Vin�ȊO�̏����̏ꍇ�́A<c><b>False</b></c></para>
        ''' <para>�@</para>
        ''' <para><b>[���ӎ���]</b></para>
        ''' <para>�f�t�H���g�ł͐��K�\���p�^�[���u\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*�v�Ō��؂��s���Ă��܂��B</para>
        ''' <para>
        ''' �Ȃ��A���̒l�͊O���\���t�@�C���́uVinFormat�v�v�f�̒l��ύX���邱�Ƃɂ���ĕύX���邱�Ƃ��\�ł��B
        ''' </para>
        ''' <para>
        ''' �f�t�H���g�̐��K�\���p�^�[���ł͌����Ώۂ̕����񂪋󕶎��i"")�̏ꍇ�A�߂�l��False�ɂȂ�܂��B
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

            ''���K�\���p�^�[�������݂��Ȃ��ꍇ�A��r�ł��Ȃ��̂�True��Ԃ�
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
        ''' PhoneNumber�̔��ʂ��s���܂�
        ''' </summary>
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <returns>
        ''' ���ؑΏۂ̕�����PhoneNumber�̏ꍇ��True�A����ȊO�̏ꍇ��False�B
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' �p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[�@�\�ڍ�]</b></para>
        ''' <para>���ؑΏۂ�PhoneNumber�ł��邩�����؂��܂��B</para>
        ''' <para>PhoneNumber�̏ꍇ�́A<c><b>True</b></c></para>
        ''' <para>PhoneNumber�ȊO�̏����̏ꍇ�́A<c><b>False</b></c></para>
        ''' <para>�@</para>
        ''' <para><b>[���ӎ���]</b></para>
        ''' <para>�f�t�H���g�ł͐��K�\���p�^�[���u\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*�v�Ō��؂��s���Ă��܂��B</para>
        ''' <para>
        ''' �Ȃ��A���̒l�͊O���\���t�@�C���́uPhoneNumberFormat�v�v�f�̒l��ύX���邱�Ƃɂ���ĕύX���邱�Ƃ��\�ł��B
        ''' </para>
        ''' <para>
        ''' �f�t�H���g�̐��K�\���p�^�[���ł͌����Ώۂ̕����񂪋󕶎��i"")�̏ꍇ�A�߂�l��False�ɂȂ�܂��B
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

            ''���K�\���p�^�[�������݂��Ȃ��ꍇ�A��r�ł��Ȃ��̂�True��Ԃ�
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
        ''' MobilePhoneNumber�̔��ʂ��s���܂�
        ''' </summary>
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <returns>
        ''' ���ؑΏۂ̕�����MobilePhoneNumber�̏ꍇ��True�A����ȊO�̏ꍇ��False�B
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' �p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[�@�\�ڍ�]</b></para>
        ''' <para>���ؑΏۂ�MobilePhoneNumber�ł��邩�����؂��܂��B</para>
        ''' <para>MobilePhoneNumber�̏ꍇ�́A<c><b>True</b></c></para>
        ''' <para>MobilePhoneNumber�ȊO�̏����̏ꍇ�́A<c><b>False</b></c></para>
        ''' <para>�@</para>
        ''' <para><b>[���ӎ���]</b></para>
        ''' <para>�f�t�H���g�ł͐��K�\���p�^�[���u\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*�v�Ō��؂��s���Ă��܂��B</para>
        ''' <para>
        ''' �Ȃ��A���̒l�͊O���\���t�@�C���́uMobilePhoneNumberFormat�v�v�f�̒l��ύX���邱�Ƃɂ���ĕύX���邱�Ƃ��\�ł��B
        ''' </para>
        ''' <para>
        ''' �f�t�H���g�̐��K�\���p�^�[���ł͌����Ώۂ̕����񂪋󕶎��i"")�̏ꍇ�A�߂�l��False�ɂȂ�܂��B
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

            ''���K�\���p�^�[�������݂��Ȃ��ꍇ�A��r�ł��Ȃ��̂�True��Ԃ�
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
        ''' PostalCode�̔��ʂ��s���܂�
        ''' </summary>
        ''' <param name="target">���ؑΏۂ̕�����</param>
        ''' <returns>
        ''' ���ؑΏۂ̕�����PostalCode�̏ꍇ��True�A����ȊO�̏ꍇ��False�B
        ''' </returns>
        ''' <exception cref="ArgumentNullException">
        ''' �p�����[�^�̌��ؑΏە�����null�Q�ƁiVB�ł�Nothing�j�B
        ''' </exception>
        ''' <remarks>
        ''' <para><b>[�@�\�ڍ�]</b></para>
        ''' <para>���ؑΏۂ�PostalCode�ł��邩�����؂��܂��B</para>
        ''' <para>PostalCode�̏ꍇ�́A<c><b>True</b></c></para>
        ''' <para>PostalCode�ȊO�̏����̏ꍇ�́A<c><b>False</b></c></para>
        ''' <para>�@</para>
        ''' <para><b>[���ӎ���]</b></para>
        ''' <para>�f�t�H���g�ł͐��K�\���p�^�[���u\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*�v�Ō��؂��s���Ă��܂��B</para>
        ''' <para>
        ''' �Ȃ��A���̒l�͊O���\���t�@�C���́uPostalCodeFormat�v�v�f�̒l��ύX���邱�Ƃɂ���ĕύX���邱�Ƃ��\�ł��B
        ''' </para>
        ''' <para>
        ''' �f�t�H���g�̐��K�\���p�^�[���ł͌����Ώۂ̕����񂪋󕶎��i"")�̏ꍇ�A�߂�l��False�ɂȂ�܂��B
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

            ''���K�\���p�^�[�������݂��Ȃ��ꍇ�A��r�ł��Ȃ��̂�True��Ԃ�
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
        ''' ������̒��ɁA�V�X�e������߂�֎~�������܂܂�Ă��Ȃ������f���܂��B
        ''' </summary>
        ''' <param name="target">�����`�F�b�N�Ώۂ̕�����</param>
        ''' <returns>True:�֎~������͊܂܂�Ă��Ȃ� False:�֎~�����񂪊܂܂�Ă��� (Target���󕶎��̏ꍇ�́ATrue��Ԃ��܂�)</returns>
        ''' <remarks>������̒��ɁA�V�X�e������߂�֎~�������܂܂�Ă��Ȃ������f���܂��B</remarks>
        Public Shared Function IsValidString(ByVal target As String) As Boolean
            ''�Ώۂ���Ȃ̂ŏI��
            If String.IsNullOrEmpty(target) Then
                Return True
            End If

            ''�֎~�������Ȃ��̂ŏI��
            Dim supress As List(Of String()) = SuppressionCharList
            If supress.Count = 0 Then
                Return True
            End If

            Dim chars As Char() = target.ToCharArray()
            Dim charCode As Integer = 0

            For i = 0 To target.Length - 1

                ''�T���Q�[�g�y�A�̕������m�F
                If Char.IsSurrogate(target, i) Then
                    ''�T���Q�[�g�y�A��Unicode�|�C���g���擾
                    charCode = Char.ConvertToUtf32(chars(i), chars(i + 1))
                    ''�T���Q�[�g�y�A��1������2������(2Byte+2Byte)�g�p����̂ŃJ�E���^����i�߂�
                    i = i + 1
                Else
                    charCode = AscW(chars(i))
                End If

                For Each suppressChar In supress

                    If suppressChar.Count = 1 Then
                        ''�ݒ肪�ʎw��̏ꍇ�͈�v���m�F
                        If Convert.ToInt32(suppressChar(0), 16) = charCode Then
                            Return False
                        End If
                    Else
                        ''�ݒ肪�͈͎w��̏ꍇ�͑召�֌W�Ŕ�r
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
