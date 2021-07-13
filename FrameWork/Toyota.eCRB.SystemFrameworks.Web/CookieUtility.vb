Imports System.Web

Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' �T�[�o�T�C�h�pCookie�����N���X�ł��B
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class CookieUtility

        'Cookie�i�[�p�L�[��
        Private Const SERVER_SIDE_COOKIE_KEY As String = "ServerSideCookie"

        'Cookie�i�[�p��؂蕶��
        Private Const SERVER_SIDE_COOKIE_DELIMITER As Char = "$"c

        'Cookie�i�[�l�̋�؂蕶��
        Private Const SERVER_SIDE_COOKIE_EQUAL As Char = "="c

        ''' <summary>
        ''' �R���X�g���N�^�ł�
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub New()
        End Sub


        ''' <summary>
        ''' Cookie�i�[����
        ''' </summary>
        ''' <param name="context">Cookie�����ΏۂƂȂ�HttpContext</param>
        ''' <param name="key">�i�[����l�̃L�[��</param>
        ''' <param name="value">�i�[����l</param>
        ''' <remarks></remarks>
        Public Shared Sub SetCookie(ByVal context As HttpContext, ByVal key As String, ByVal value As String)

            Dim isNew As Boolean = True
            'Cookie����T�[�o�T�C�hCookie���擾
            '��؂蕶����Split���Ĕz�񉻂���
            Dim cookieArr As String() = GetCookieArray(context)

            '�z��̗v�f�����[�v
            For i As Integer = 0 To cookieArr.Length - 1
                '������Split�����擪�̗v�f����v����ꍇ
                If String.Equals(cookieArr(i).Split(SERVER_SIDE_COOKIE_EQUAL)(0), key) Then
                    '�z��̗v�f���X�V����
                    cookieArr(i) = key & SERVER_SIDE_COOKIE_EQUAL & value
                    isNew = False
                    Exit For
                End If
            Next

            '�V�K�ǉ��̒l�̏ꍇ
            If isNew Then
                '�z��ɒǉ�����
                Array.Resize(cookieArr, cookieArr.Length + 1)
                cookieArr(cookieArr.Length - 1) = key & SERVER_SIDE_COOKIE_EQUAL & value
            End If

            '�z�����؂蕶����Join���ACookie�Ɋi�[
            SetCookieArray(context, cookieArr)

        End Sub

        ''' <summary>
        ''' Cookie�擾����
        ''' </summary>
        ''' <param name="context">Cookie�����ΏۂƂȂ�HttpContext</param>
        ''' <param name="key">Cookie����擾����l�̃L�[��</param>
        ''' <returns>Cookie����擾�����l</returns>
        ''' <remarks></remarks>
        Public Shared Function GetCookie(ByVal context As HttpContext, ByVal key As String) As String

            'Cookie����T�[�o�T�C�hCookie���擾
            '��؂蕶����Split���Ĕz�񉻂���
            Dim cookieArr As String() = GetCookieArray(context)

            '�z��̗v�f�����[�v
            For i As Integer = 0 To cookieArr.Length - 1
                '������Split�����擪�̗v�f����v����ꍇ
                If String.Equals(cookieArr(i).Split(SERVER_SIDE_COOKIE_EQUAL)(0), key) Then
                    '�����̗v�f��߂�l�Ƃ��Ė߂�
                    Return cookieArr(i).Split(SERVER_SIDE_COOKIE_EQUAL)(1)
                End If
            Next

            Return Nothing
        End Function

        ''' <summary>
        ''' Cookie�폜����
        ''' </summary>
        ''' <param name="context">Cookie�����ΏۂƂȂ�HttpContext</param>
        ''' <param name="key">Cookie����폜����l�̃L�[��</param>
        ''' <remarks></remarks>
        Public Shared Sub RemoveCookie(ByVal context As HttpContext, ByVal key As String)

            'Cookie����T�[�o�T�C�hCookie���擾
            '��؂蕶����Split���Ĕz�񉻂���
            Dim cookieArr As String() = GetCookieArray(context)

            '�z���List�^�ɕϊ�����
            Dim cookieList As New List(Of String)
            cookieList.AddRange(cookieArr)

            '�폜�Ώۂ����݂���
            Dim exists As Boolean = False

            '�z��̗v�f�����[�v
            For i As Integer = 0 To cookieList.Count - 1
                '������Split�����擪�̗v�f����v����ꍇ
                If String.Equals(cookieList.Item(i).Split(SERVER_SIDE_COOKIE_EQUAL)(0), key) Then
                    '�Y���̗v�f���폜
                    cookieList.RemoveAt(i)
                    exists = True
                    Exit For
                End If
            Next

            '�폜�Ώۂ����݂���ꍇ�ACookie�ɔ��f
            If exists Then
                '�z�����؂蕶����Join���ACookie�Ɋi�[
                SetCookieArray(context, cookieList.ToArray)
            End If
        End Sub


        ''' <summary>
        ''' Cookie�z��擾����
        ''' </summary>
        ''' <param name="context">Cookie�����ΏۂƂȂ�HttpContext</param>
        ''' <returns>Cookie����擾�����l�̊i�[���ꂽ�z��</returns>
        ''' <remarks></remarks>
        Public Shared Function GetCookieArray(ByVal context As HttpContext) As String()

            '����context��Nothing�̏ꍇ
            If context Is Nothing Then
                '��O�Ƃ���ArgumentNullException���X���[����
                Throw New ArgumentNullException("context")
            End If

            Dim serverSideCookie As String

            'Cookie����T�[�o�T�C�hCookie���擾
            'Respose����Cookie�擾
            serverSideCookie = context.Server.UrlDecode(context.Response.Cookies(SERVER_SIDE_COOKIE_KEY).Value)

            If String.IsNullOrEmpty(serverSideCookie) Then
                'Respose�ɑ��݂��Ȃ��ꍇ�ARequest����Respose�ɐݒ�
                context.Response.Cookies(SERVER_SIDE_COOKIE_KEY).Value = context.Request.Cookies(SERVER_SIDE_COOKIE_KEY).Value
                serverSideCookie = context.Server.UrlDecode(context.Response.Cookies(SERVER_SIDE_COOKIE_KEY).Value)
            End If

            '��؂蕶����Split���Ĕz�񉻂���
            Dim cookieArr As String()
            If Not String.IsNullOrEmpty(serverSideCookie) Then
                cookieArr = serverSideCookie.Split(SERVER_SIDE_COOKIE_DELIMITER)
            Else
                cookieArr = New String() {}
            End If

            Return cookieArr
        End Function


        ''' <summary>
        ''' Cookie�z��i�[����
        ''' </summary>
        ''' <param name="context">Cookie�����ΏۂƂȂ�HttpContext</param>
        ''' <param name="cookieArr">�i�[����l�̔z��</param>
        ''' <remarks></remarks>
        Public Shared Sub SetCookieArray(ByVal context As HttpContext, ByVal cookieArr As String())

            '����context��Nothing�̏ꍇ
            If context Is Nothing Then
                '��O�Ƃ���ArgumentNullException���X���[����
                Throw New ArgumentNullException("context")
            End If

            '�z�����؂蕶����Join���ACookie�Ɋi�[
            Dim setValue As String = String.Join(SERVER_SIDE_COOKIE_DELIMITER, cookieArr)
            context.Response.Cookies(SERVER_SIDE_COOKIE_KEY).Value = context.Server.UrlEncode(setValue)
        End Sub

    End Class

End Namespace
