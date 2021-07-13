'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'XmlCustomer.vb
'─────────────────────────────────────
'Ver.  : SA01_LC_001
'Create: 
'Update: 2018/07/05[SA01_LC_001] NSK Niiya TKM Next Gen e-CRB Project Application development Block B-2 SI No.1,2,4,5,6 $01
'─────────────────────────────────────
Public Class XmlCustomer
    Private _CustomerID As String
    Private _SeqNo As String
    Private _CustomerSegment As String
    Private _NewcustomerID As String
    Private _CustomerCode As String
    Private _EnquiryCustomerCode As String
    Private _SalesStaffCode As String
    Private _CustomerType As String
    Private _SubCustomerType As String
    '$01  start
    Private _SubCustomerType2 As String
    Private _OrganizationName As String
    '$01 end
    Private _SocialID As String
    Private _Sex As String
    Private _BirthDay As String
    Private _NameTitleCode As String
    Private _NameTitle As String
    Private _Name1 As String
    Private _Name2 As String
    Private _Name3 As String
    Private _SubName1 As String
    Private _CompanyName As String
    Private _EmployeeName As String
    Private _EmployeeDepartment As String
    Private _EmployeePosition As String
    Private _Address As String
    Private _Address1 As String
    Private _Address2 As String
    Private _Address3 As String
    Private _Domicile As String
    Private _Country As String
    Private _ZipCode As String
    Private _StateCode As String
    Private _StateName As String
    Private _DistrictCode As String
    Private _DistrictName As String
    Private _CityCode As String
    Private _CityName As String
    Private _LocationCode As String
    Private _LocationName As String
    Private _TelNumber As String
    Private _FaxNumber As String
    Private _Mobile As String
    Private _EMail1 As String
    Private _EMail2 As String
    Private _BusinessTelNumber As String
    Private _Income As String
    Private _ContactTime As String
    Private _OccupationID As String
    Private _Occupation As String
    Private _DefaultLang As String
    Private _CustomerMemo As String
    Private _CreateDate As String
    Private _UpdateDate As String
    Private _DeleteDate As String

    Public Property CustomerID() As String
        Get
            Return _CustomerID
        End Get
        Set(ByVal Value As String)
            _CustomerID = Value
        End Set
    End Property

    Public Property SeqNo() As String
        Get
            Return _SeqNo
        End Get
        Set(ByVal Value As String)
            _SeqNo = Value
        End Set
    End Property

    Public Property CustomerSegment() As String
        Get
            Return _CustomerSegment
        End Get
        Set(ByVal Value As String)
            _CustomerSegment = Value
        End Set
    End Property

    Public Property NewcustomerID() As String
        Get
            Return _NewcustomerID
        End Get
        Set(ByVal Value As String)
            _NewcustomerID = Value
        End Set
    End Property

    Public Property CustomerCode() As String
        Get
            Return _CustomerCode
        End Get
        Set(ByVal Value As String)
            _CustomerCode = Value
        End Set
    End Property

    Public Property EnquiryCustomerCode() As String
        Get
            Return _EnquiryCustomerCode
        End Get
        Set(ByVal Value As String)
            _EnquiryCustomerCode = Value
        End Set
    End Property

    Public Property SalesStaffCode() As String
        Get
            Return _SalesStaffCode
        End Get
        Set(ByVal Value As String)
            _SalesStaffCode = Value
        End Set
    End Property

    Public Property CustomerType() As String
        Get
            Return _CustomerType
        End Get
        Set(ByVal Value As String)
            _CustomerType = Value
        End Set
    End Property

    Public Property SubCustomerType() As String
        Get
            Return _SubCustomerType
        End Get
        Set(ByVal Value As String)
            _SubCustomerType = Value
        End Set
    End Property

    '$01 start
    Public Property SubCustomerType2() As String
        Get
            Return _SubCustomerType2
        End Get
        Set(ByVal Value As String)
            _SubCustomerType2 = Value
        End Set
    End Property
    '$01 end

    '$01 start
    Public Property OrganizationName() As String
        Get
            Return _OrganizationName
        End Get
        Set(ByVal Value As String)
            _OrganizationName = Value
        End Set
    End Property
    '$01 end

    Public Property SocialID() As String
        Get
            Return _SocialID
        End Get
        Set(ByVal Value As String)
            _SocialID = Value
        End Set
    End Property

    Public Property Sex() As String
        Get
            Return _Sex
        End Get
        Set(ByVal Value As String)
            _Sex = Value
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

    Public Property NameTitleCode() As String
        Get
            Return _NameTitleCode
        End Get
        Set(ByVal Value As String)
            _NameTitleCode = Value
        End Set
    End Property

    Public Property NameTitle() As String
        Get
            Return _NameTitle
        End Get
        Set(ByVal Value As String)
            _NameTitle = Value
        End Set
    End Property

    Public Property Name1() As String
        Get
            Return _Name1
        End Get
        Set(ByVal Value As String)
            _Name1 = Value
        End Set
    End Property

    Public Property Name2() As String
        Get
            Return _Name2
        End Get
        Set(ByVal Value As String)
            _Name2 = Value
        End Set
    End Property

    Public Property Name3() As String
        Get
            Return _Name3
        End Get
        Set(ByVal Value As String)
            _Name3 = Value
        End Set
    End Property

    Public Property SubName1() As String
        Get
            Return _SubName1
        End Get
        Set(ByVal Value As String)
            _SubName1 = Value
        End Set
    End Property

    Public Property CompanyName() As String
        Get
            Return _CompanyName
        End Get
        Set(ByVal Value As String)
            _CompanyName = Value
        End Set
    End Property

    Public Property EmployeeName() As String
        Get
            Return _EmployeeName
        End Get
        Set(ByVal Value As String)
            _EmployeeName = Value
        End Set
    End Property

    Public Property EmployeeDepartment() As String
        Get
            Return _EmployeeDepartment
        End Get
        Set(ByVal Value As String)
            _EmployeeDepartment = Value
        End Set
    End Property

    Public Property EmployeePosition() As String
        Get
            Return _EmployeePosition
        End Get
        Set(ByVal Value As String)
            _EmployeePosition = Value
        End Set
    End Property

    Public Property Address() As String
        Get
            Return _Address
        End Get
        Set(ByVal Value As String)
            _Address = Value
        End Set
    End Property

    Public Property Address1() As String
        Get
            Return _Address1
        End Get
        Set(ByVal Value As String)
            _Address1 = Value
        End Set
    End Property

    Public Property Address2() As String
        Get
            Return _Address2
        End Get
        Set(ByVal Value As String)
            _Address2 = Value
        End Set
    End Property

    Public Property Address3() As String
        Get
            Return _Address3
        End Get
        Set(ByVal Value As String)
            _Address3 = Value
        End Set
    End Property

    Public Property Domicile() As String
        Get
            Return _Domicile
        End Get
        Set(ByVal Value As String)
            _Domicile = Value
        End Set
    End Property

    Public Property Country() As String
        Get
            Return _Country
        End Get
        Set(ByVal Value As String)
            _Country = Value
        End Set
    End Property

    Public Property ZipCode() As String
        Get
            Return _ZipCode
        End Get
        Set(ByVal Value As String)
            _ZipCode = Value
        End Set
    End Property

    Public Property StateCode() As String
        Get
            Return _StateCode
        End Get
        Set(ByVal Value As String)
            _StateCode = Value
        End Set
    End Property

    Public Property StateName() As String
        Get
            Return _StateName
        End Get
        Set(ByVal Value As String)
            _StateName = Value
        End Set
    End Property

    Public Property DistrictCode() As String
        Get
            Return _DistrictCode
        End Get
        Set(ByVal Value As String)
            _DistrictCode = Value
        End Set
    End Property

    Public Property DistrictName() As String
        Get
            Return _DistrictName
        End Get
        Set(ByVal Value As String)
            _DistrictName = Value
        End Set
    End Property

    Public Property CityCode() As String
        Get
            Return _CityCode
        End Get
        Set(ByVal Value As String)
            _CityCode = Value
        End Set
    End Property

    Public Property CityName() As String
        Get
            Return _CityName
        End Get
        Set(ByVal Value As String)
            _CityName = Value
        End Set
    End Property

    Public Property LocationCode() As String
        Get
            Return _LocationCode
        End Get
        Set(ByVal Value As String)
            _LocationCode = Value
        End Set
    End Property

    Public Property LocationName() As String
        Get
            Return _LocationName
        End Get
        Set(ByVal Value As String)
            _LocationName = Value
        End Set
    End Property

    Public Property TelNumber() As String
        Get
            Return _TelNumber
        End Get
        Set(ByVal Value As String)
            _TelNumber = Value
        End Set
    End Property

    Public Property FaxNumber() As String
        Get
            Return _FaxNumber
        End Get
        Set(ByVal Value As String)
            _FaxNumber = Value
        End Set
    End Property

    Public Property Mobile() As String
        Get
            Return _Mobile
        End Get
        Set(ByVal Value As String)
            _Mobile = Value
        End Set
    End Property

    Public Property EMail1() As String
        Get
            Return _EMail1
        End Get
        Set(ByVal Value As String)
            _EMail1 = Value
        End Set
    End Property

    Public Property EMail2() As String
        Get
            Return _EMail2
        End Get
        Set(ByVal Value As String)
            _EMail2 = Value
        End Set
    End Property

    Public Property BusinessTelNumber() As String
        Get
            Return _BusinessTelNumber
        End Get
        Set(ByVal Value As String)
            _BusinessTelNumber = Value
        End Set
    End Property

    Public Property Income() As String
        Get
            Return _Income
        End Get
        Set(ByVal Value As String)
            _Income = Value
        End Set
    End Property

    Public Property ContactTime() As String
        Get
            Return _ContactTime
        End Get
        Set(ByVal Value As String)
            _ContactTime = Value
        End Set
    End Property

    Public Property OccupationID() As String
        Get
            Return _OccupationID
        End Get
        Set(ByVal Value As String)
            _OccupationID = Value
        End Set
    End Property

    Public Property Occupation() As String
        Get
            Return _Occupation
        End Get
        Set(ByVal Value As String)
            _Occupation = Value
        End Set
    End Property

    Public Property DefaultLang() As String
        Get
            Return _DefaultLang
        End Get
        Set(ByVal Value As String)
            _DefaultLang = Value
        End Set
    End Property

    Public Property CustomerMemo() As String
        Get
            Return _CustomerMemo
        End Get
        Set(ByVal Value As String)
            _CustomerMemo = Value
        End Set
    End Property

    Public Property CreateDate() As String
        Get
            Return _CreateDate
        End Get
        Set(ByVal Value As String)
            _CreateDate = Value
        End Set
    End Property

    Public Property UpdateDate() As String
        Get
            Return _UpdateDate
        End Get
        Set(ByVal Value As String)
            _UpdateDate = Value
        End Set
    End Property

    Public Property DeleteDate() As String
        Get
            Return _DeleteDate
        End Get
        Set(ByVal Value As String)
            _DeleteDate = Value
        End Set
    End Property


End Class
