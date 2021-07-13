Public Class XmlFamilyInformation
    Private _FamilyNo As String
    Private _FamilyCode As String
    Private _FamilyCodeName As String
    Private _BirthDay As String

    Public Property FamilyNo() As String
        Get
            Return _FamilyNo
        End Get
        Set(ByVal Value As String)
            _FamilyNo = Value
        End Set
    End Property

    Public Property FamilyCode() As String
        Get
            Return _FamilyCode
        End Get
        Set(ByVal Value As String)
            _FamilyCode = Value
        End Set
    End Property

    Public Property FamilyCodeName() As String
        Get
            Return _FamilyCodeName
        End Get
        Set(ByVal Value As String)
            _FamilyCodeName = Value
        End Set
    End Property

    Public Property BirthDay() As String
        Get
            Return _BirthDay
        End Get
        Set(ByVal Value As String)
            _BirthDay = Value
        End Set
    End Property


End Class
