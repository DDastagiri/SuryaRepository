'Copyright c2009 Toyota Motor Corporation. All rights reserved. For internal use only.
Imports System.Web
Imports System.Web.Caching


Namespace Toyota.eCRB.SystemFrameworks.Web

    ''' <summary>
    ''' キャッシュにアクセスする機能を提供するクラスです。
    ''' </summary>
    ''' <remarks>キャッシュ内の情報は自動的に削除されません。
    ''' キャッシュ内の情報を削除したい場合は明示的に削除してください。
    ''' キャッシュにアクセスするキー、及び値はHttpContextクラスのCacheオブジェクトに
    ''' 格納します。</remarks>
    Public NotInheritable Class CachingManager

        ''' <summary>
        ''' コンストラクタです。
        ''' </summary>
        ''' <remarks>
        ''' インスタンスを生成させないようにするため、修飾子はPrivateです。</remarks>
        Private Sub New()

            ' do nothing

        End Sub

        ''' <summary>
        ''' キャッシュに値を格納します。
        ''' </summary>
        ''' <param name="key">キャッシュに格納するキー。
        ''' キーにNothingは指定できません。指定した場合、ArgumentNullExceptionがスローされます。</param>
        ''' <param name="value">キャッシュに格納する値。
        ''' 値にNothingは指定できません。指定した場合、ArgumentNullExceptionがスローされます。</param>
        ''' <remarks>既に同一なキーで値が格納されている場合、値を上書きします。</remarks>
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
        ''' キャッシュ内に指定のキーが格納されているかどうかを判断します。
        ''' </summary>
        ''' <param name="key">キャッシュ内で検索されるキー。
        ''' 値にNothingは指定できません。指定した場合、ArgumentNullExceptionがスローされます。</param>
        ''' <returns>キャッシュ内に特定のキーが格納されている場合はTrue、
        ''' 格納されていない場合はFalse。</returns>
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
        ''' キャッシュから値を取得します。
        ''' </summary>
        ''' <param name="key">キャッシュ内で検索されるキー。
        ''' 値にNothingは指定できません。指定した場合、ArgumentNullExceptionがスローされます。</param>
        ''' <returns>指定したキーに対応する値。</returns>
        ''' <remarks>指定のキーに対応する値が無い場合はNothingを戻します。</remarks>
        Public Shared Function [Get]( _
                ByVal key As String) As Object

            Return System.Web.HttpRuntime.Cache.Get(key)

        End Function

        ''' <summary>
        ''' キャッシュから指定したキーとその値を削除します。
        ''' </summary>
        ''' <param name="key">キャッシュ内で検索されるキー。
        ''' 値にNothingは指定できません。指定した場合、ArgumentNullExceptionがスローされます。</param>
        ''' <remarks>指定のキーに対応する値が無い場合は何も行いません。</remarks>
        Public Shared Sub Remove( _
                ByVal key As String)

            System.Web.HttpRuntime.Cache.Remove(key)

        End Sub

    End Class

End Namespace
