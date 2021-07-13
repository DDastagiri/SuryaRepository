'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Web
Imports System.Web.Caching


Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' �L���b�V���ɃA�N�Z�X����@�\��񋟂���N���X�ł��B
    ''' </summary>
    ''' <remarks>�L���b�V�����̏��͎����I�ɍ폜����܂���B
    ''' �L���b�V�����̏����폜�������ꍇ�͖����I�ɍ폜���Ă��������B
    ''' �L���b�V���ɃA�N�Z�X����L�[�A�y�ђl��HttpContext�N���X��Cache�I�u�W�F�N�g��
    ''' �i�[���܂��B</remarks>
    Public NotInheritable Class CachingManager

        ''' <summary>
        ''' �R���X�g���N�^�ł��B
        ''' </summary>
        ''' <remarks>
        ''' �C���X�^���X�𐶐������Ȃ��悤�ɂ��邽�߁A�C���q��Private�ł��B</remarks>
        Private Sub New()

            ' do nothing

        End Sub

        ''' <summary>
        ''' �L���b�V���ɒl���i�[���܂��B
        ''' </summary>
        ''' <param name="key">�L���b�V���Ɋi�[����L�[�B
        ''' �L�[��Nothing�͎w��ł��܂���B�w�肵���ꍇ�AArgumentNullException���X���[����܂��B</param>
        ''' <param name="value">�L���b�V���Ɋi�[����l�B
        ''' �l��Nothing�͎w��ł��܂���B�w�肵���ꍇ�AArgumentNullException���X���[����܂��B</param>
        ''' <remarks>���ɓ���ȃL�[�Œl���i�[����Ă���ꍇ�A�l���㏑�����܂��B</remarks>
        Public Shared Sub Put( _
                ByVal key As String, _
                ByVal value As Object)

            If CachingManager.ContainsKey(key) Then

                System.Web.HttpRuntime.Cache.Insert(key, value)

            Else

                Dim expireDate As DateTime = CDate("00:00:00")
                expireDate = Now.Date.Add(expireDate.TimeOfDay)

                expireDate = expireDate.AddDays(1)

                System.Web.HttpRuntime.Cache.Add(key, _
                        value, _
                        Nothing, _
                        expireDate, _
                        System.Web.Caching.Cache.NoSlidingExpiration, _
                        Caching.CacheItemPriority.NotRemovable, _
                        Nothing)

            End If

        End Sub

        ''' <summary>
        ''' �L���b�V�����Ɏw��̃L�[���i�[����Ă��邩�ǂ����𔻒f���܂��B
        ''' </summary>
        ''' <param name="key">�L���b�V�����Ō��������L�[�B
        ''' �l��Nothing�͎w��ł��܂���B�w�肵���ꍇ�AArgumentNullException���X���[����܂��B</param>
        ''' <returns>�L���b�V�����ɓ���̃L�[���i�[����Ă���ꍇ��True�A
        ''' �i�[����Ă��Ȃ��ꍇ��False�B</returns>
        ''' <remarks></remarks>
        Private Shared Function ContainsKey( _
                ByVal key As String) As Boolean

            If System.Web.HttpRuntime.Cache.Get(key) Is Nothing Then

                Return False

            Else

                Return True

            End If

        End Function

        ''' <summary>
        ''' �L���b�V������l���擾���܂��B
        ''' </summary>
        ''' <param name="key">�L���b�V�����Ō��������L�[�B
        ''' �l��Nothing�͎w��ł��܂���B�w�肵���ꍇ�AArgumentNullException���X���[����܂��B</param>
        ''' <returns>�w�肵���L�[�ɑΉ�����l�B</returns>
        ''' <remarks>�w��̃L�[�ɑΉ�����l�������ꍇ��Nothing��߂��܂��B</remarks>
        Public Shared Function [Get]( _
                ByVal key As String) As Object

            Return System.Web.HttpRuntime.Cache.Get(key)

        End Function

        ''' <summary>
        ''' �L���b�V������w�肵���L�[�Ƃ��̒l���폜���܂��B
        ''' </summary>
        ''' <param name="key">�L���b�V�����Ō��������L�[�B
        ''' �l��Nothing�͎w��ł��܂���B�w�肵���ꍇ�AArgumentNullException���X���[����܂��B</param>
        ''' <remarks>�w��̃L�[�ɑΉ�����l�������ꍇ�͉����s���܂���B</remarks>
        Public Shared Sub Remove( _
                ByVal key As String)

            System.Web.HttpRuntime.Cache.Remove(key)

        End Sub

    End Class

End Namespace
