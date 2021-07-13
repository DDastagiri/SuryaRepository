Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel

Public Class XmlFollowUp
    Private _SeqNo As String
    Private _FollowUpID As String
    Private _FollowUpNo As String
    Private _ParentFollowUpNo As String
    Private _PreFollowUpNo As String
    Private _FollowUpDate As String
    Private _PreFollowUpCreateDate As String
    Private _DemandStructure As String
    Private _DirectBillingFlg As String
    Private _FirstContactType As String
    Private _SourceID1 As String
    Private _SourceName1 As String
    Private _SourceID2 As String
    Private _SourceName2 As String
    Private _PotentialDivision As String
    Private _Vin As String
    Private _InterestDate As String
    Private _ProspectDate As String
    Private _HotDate As String
    Private _ReconsiderDate As String
    Private _OtherDLRPurchaseFlg As String
    Private _SalesTargetDate As String
    Private _PlannedBranchCode As String
    Private _PlannedAccount As String
    Private _Createdby As String
    Private _Createdate As String
    Private _Updatedby As String
    Private _Updatedate As String

    Private _SelectedSeries As Collection(Of XmlSelectedSeries)
    Private _CompetitorSeries As Collection(Of XmlCompetitorSeries)
    Private _Action As Collection(Of XmlAction)
    Private _SalesCondition As Collection(Of XmlSalesCondition)
    Private _NegotiationMemo As Collection(Of XmlNegotiationMemo)

    Public Property SeqNo() As String
        Get
            Return _SeqNo
        End Get
        Set(ByVal Value As String)
            _SeqNo = Value
        End Set
    End Property

    Public Property FollowUpID() As String
        Get
            Return _FollowUpID
        End Get
        Set(ByVal Value As String)
            _FollowUpID = Value
        End Set
    End Property

    Public Property FollowUpNo() As String
        Get
            Return _FollowUpNo
        End Get
        Set(ByVal Value As String)
            _FollowUpNo = Value
        End Set
    End Property

    Public Property ParentFollowUpNo() As String
        Get
            Return _ParentFollowUpNo
        End Get
        Set(ByVal Value As String)
            _ParentFollowUpNo = Value
        End Set
    End Property

    Public Property PreFollowUpNo() As String
        Get
            Return _PreFollowUpNo
        End Get
        Set(ByVal Value As String)
            _PreFollowUpNo = Value
        End Set
    End Property

    Public Property FollowUpDate() As String
        Get
            Return _FollowUpDate
        End Get
        Set(ByVal Value As String)
            _FollowUpDate = Value
        End Set
    End Property

    Public Property PreFollowUpCreateDate() As String
        Get
            Return _PreFollowUpCreateDate
        End Get
        Set(ByVal Value As String)
            _PreFollowUpCreateDate = Value
        End Set
    End Property

    Public Property DemandStructure() As String
        Get
            Return _DemandStructure
        End Get
        Set(ByVal Value As String)
            _DemandStructure = Value
        End Set
    End Property

    Public Property DirectBillingFlg() As String
        Get
            Return _DirectBillingFlg
        End Get
        Set(ByVal Value As String)
            _DirectBillingFlg = Value
        End Set
    End Property

    Public Property FirstContactType() As String
        Get
            Return _FirstContactType
        End Get
        Set(ByVal Value As String)
            _FirstContactType = Value
        End Set
    End Property

    Public Property SourceID1() As String
        Get
            Return _SourceID1
        End Get
        Set(ByVal Value As String)
            _SourceID1 = Value
        End Set
    End Property

    Public Property SourceName1() As String
        Get
            Return _SourceName1
        End Get
        Set(ByVal Value As String)
            _SourceName1 = Value
        End Set
    End Property

    Public Property SourceID2() As String
        Get
            Return _SourceID2
        End Get
        Set(ByVal Value As String)
            _SourceID2 = Value
        End Set
    End Property

    Public Property SourceName2() As String
        Get
            Return _SourceName2
        End Get
        Set(ByVal Value As String)
            _SourceName2 = Value
        End Set
    End Property

    Public Property PotentialDivision() As String
        Get
            Return _PotentialDivision
        End Get
        Set(ByVal Value As String)
            _PotentialDivision = Value
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

    Public Property InterestDate() As String
        Get
            Return _InterestDate
        End Get
        Set(ByVal Value As String)
            _InterestDate = Value
        End Set
    End Property

    Public Property ProspectDate() As String
        Get
            Return _ProspectDate
        End Get
        Set(ByVal Value As String)
            _ProspectDate = Value
        End Set
    End Property

    Public Property HotDate() As String
        Get
            Return _HotDate
        End Get
        Set(ByVal Value As String)
            _HotDate = Value
        End Set
    End Property

    Public Property ReconsiderDate() As String
        Get
            Return _ReconsiderDate
        End Get
        Set(ByVal Value As String)
            _ReconsiderDate = Value
        End Set
    End Property

    Public Property OtherDLRPurchaseFlg() As String
        Get
            Return _OtherDLRPurchaseFlg
        End Get
        Set(ByVal Value As String)
            _OtherDLRPurchaseFlg = Value
        End Set
    End Property

    Public Property SalesTargetDate() As String
        Get
            Return _SalesTargetDate
        End Get
        Set(ByVal Value As String)
            _SalesTargetDate = Value
        End Set
    End Property

    Public Property PlannedBranchCode() As String
        Get
            Return _PlannedBranchCode
        End Get
        Set(ByVal Value As String)
            _PlannedBranchCode = Value
        End Set
    End Property

    Public Property PlannedAccount() As String
        Get
            Return _PlannedAccount
        End Get
        Set(ByVal Value As String)
            _PlannedAccount = Value
        End Set
    End Property

    Public Property Createdby() As String
        Get
            Return _Createdby
        End Get
        Set(ByVal Value As String)
            _Createdby = Value
        End Set
    End Property

    Public Property Createdate() As String
        Get
            Return _Createdate
        End Get
        Set(ByVal Value As String)
            _Createdate = Value
        End Set
    End Property

    Public Property Updatedby() As String
        Get
            Return _Updatedby
        End Get
        Set(ByVal Value As String)
            _Updatedby = Value
        End Set
    End Property

    Public Property Updatedate() As String
        Get
            Return _Updatedate
        End Get
        Set(ByVal Value As String)
            _Updatedate = Value
        End Set
    End Property

    Public Property SelectedSeries As Collection(Of XmlSelectedSeries)
        Get
            Return _SelectedSeries
        End Get
        Set(ByVal Value As Collection(Of XmlSelectedSeries))
            _SelectedSeries = Value
        End Set
    End Property

    Public Property CompetitorSeries() As Collection(Of XmlCompetitorSeries)
        Get
            Return _CompetitorSeries
        End Get
        Set(ByVal Value As Collection(Of XmlCompetitorSeries))
            _CompetitorSeries = Value
        End Set
    End Property

    Public Property Action() As Collection(Of XmlAction)
        Get
            Return _Action
        End Get
        Set(ByVal Value As Collection(Of XmlAction))
            _Action = Value
        End Set
    End Property

    Public Property SalesCondition() As Collection(Of XmlSalesCondition)
        Get
            Return _SalesCondition
        End Get
        Set(ByVal Value As Collection(Of XmlSalesCondition))
            _SalesCondition = Value
        End Set
    End Property

    Public Property NegotiationMemo() As Collection(Of XmlNegotiationMemo)
        Get
            Return _NegotiationMemo
        End Get
        Set(ByVal Value As Collection(Of XmlNegotiationMemo))
            _NegotiationMemo = Value
        End Set
    End Property


End Class
