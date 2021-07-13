'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'XmlVehicle.vb
'─────────────────────────────────────
'Ver.  : SA01_LC_001
'Create: 
'Update: 2018/07/05[SA01_LC_001] NSK Niiya TKM Next Gen e-CRB Project Application development Block B-2 SI No.1,2,4,5,6 $01
'─────────────────────────────────────
Public Class XmlVehicle
    Private _VehicleSeqNo As String
    Private _SeriesCode As String
    Private _SeriesName As String
    Private _Vin As String
    Private _VehicleRegistrationNumber As String
    Private _VehicleDeliveryDate As String
    '$01 start
    Private _VehicleMile As String
    Private _VehicleModelYear As String
    '$01 end

    Public Property VehicleSeqNo() As String
        Get
            Return _VehicleSeqNo
        End Get
        Set(ByVal Value As String)
            _VehicleSeqNo = Value
        End Set
    End Property

    Public Property SeriesCode() As String
        Get
            Return _SeriesCode
        End Get
        Set(ByVal Value As String)
            _SeriesCode = Value
        End Set
    End Property

    Public Property SeriesName() As String
        Get
            Return _SeriesName
        End Get
        Set(ByVal Value As String)
            _SeriesName = Value
        End Set
    End Property

    Public Property Vin() As String
        Get
            Return _Vin
        End Get
        Set(ByVal Value As String)
            _Vin = Value
        End Set
    End Property

    Public Property VehicleRegistrationNumber() As String
        Get
            Return _VehicleRegistrationNumber
        End Get
        Set(ByVal Value As String)
            _VehicleRegistrationNumber = Value
        End Set
    End Property

    Public Property VehicleDeliveryDate() As String
        Get
            Return _VehicleDeliveryDate
        End Get
        Set(ByVal Value As String)
            _VehicleDeliveryDate = Value
        End Set
    End Property

    '$01 start
    Public Property VehicleMile() As String
        Get
            Return _VehicleMile
        End Get
        Set(ByVal Value As String)
            _VehicleMile = Value
        End Set
    End Property
    '$01 end

    '$01 start
    Public Property VehicleModelYear() As String
        Get
            Return _VehicleModelYear
        End Get
        Set(ByVal Value As String)
            _VehicleModelYear = Value
        End Set
    End Property
    '$01 end

End Class
