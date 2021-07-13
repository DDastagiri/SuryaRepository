Imports Toyota.eCRB.SystemFrameworks.Configuration

Namespace Toyota.eCRB.SystemFrameworks.Core

    ''' <summary>
    ''' リリースバージョン情報へのアクセスを提供するクラスです。
    ''' </summary>
    Public NotInheritable Class VersionInformation

        ''' <summary>
        ''' コンストラクタです。
        ''' </summary>
        ''' <remarks>リリースバージョン情報が設定から取得できなかった場合は、1.0.0をリリースバージョンとみなします。</remarks>
        Public Sub New()
            _MajorVersion = 1
            _MinorVersion = 0
            _Revision = 0

            Dim sysConfig As SystemConfiguration = SystemConfiguration.Current
            Dim section As ClassSection = sysConfig.Manager.VersionInformation
            If (section IsNot Nothing) Then
                Dim setting As Setting = section.GetSetting(String.Empty)
                If (setting IsNot Nothing) Then
                    Dim versionString As String = CStr(setting.GetValue("Version"))
                    If (versionString IsNot Nothing) Then
                        Dim versionTokens As String() = versionString.Split(New Char() {"."c}, 3)
                        Dim versionNumber As Integer
                        If (1 <= versionTokens.Length AndAlso Integer.TryParse(versionTokens(0), versionNumber)) Then
                            _MajorVersion = versionNumber
                        End If
                        If (2 <= versionTokens.Length AndAlso Integer.TryParse(versionTokens(1), versionNumber)) Then
                            _MinorVersion = versionNumber
                        End If
                        If (3 <= versionTokens.Length AndAlso Integer.TryParse(versionTokens(2), versionNumber)) Then
                            _Revision = versionNumber
                        End If
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' リリースバージョンのメジャーバージョン番号を取得します。
        ''' </summary>
        Public ReadOnly Property MajorVersion As Integer
            Get
                Return _MajorVersion
            End Get
        End Property

        ''' <summary>
        ''' リリースバージョンのマイナーーバージョン番号を取得します。
        ''' </summary>
        Public ReadOnly Property MinorVersion As Integer
            Get
                Return _MinorVersion
            End Get
        End Property

        ''' <summary>
        ''' リリースバージョンのリビジョン番号を取得します。
        ''' </summary>
        Public ReadOnly Property Revision As Integer
            Get
                Return _Revision
            End Get
        End Property

        ''' <summary>
        ''' 現在のリリースバージョンと引数のバージョンを比較します。
        ''' </summary>
        ''' <param name="major">比較対象のバージョン（メジャーバージョン番号）</param>
        ''' <param name="minor">比較対象のバージョン（マイナーバージョン番号）</param>
        ''' <param name="revision">比較対象のバージョン（リビジョン番号）</param>
        ''' <returns>True … 現在のリリースバージョン＞＝引数のバージョン　　False … 現在のリリースバージョン＜引数のバージョン</returns>
        Public Shared Function IsEqualOrLaterThan(ByVal major As Integer, ByVal minor As Integer, ByVal revision As Integer) As Boolean
            Dim version = New VersionInformation()
            If (version.MajorVersion < major) Then
                Return False
            ElseIf (major = version.MajorVersion) Then
                If (version.MinorVersion < minor) Then
                    Return False
                ElseIf (minor = version.MinorVersion) Then
                    If (version.Revision < revision) Then
                        Return False
                    End If
                End If
            End If
            Return True
        End Function


        Private _MajorVersion As Integer
        Private _MinorVersion As Integer
        Private _Revision As Integer
    End Class
End Namespace

