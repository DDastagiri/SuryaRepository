Imports System
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Public Class XmlProspectCustomer
    Private _Head As XmlHead
    Private _Common As XmlCommon
    Private _FollowUp As XmlFollowUp
    Private _CompetitorSeries As XmlCompetitorSeries
    Private _Action As XmlAction
    Private _SalesCondition As XmlSalesCondition
    Private _SelectItem As XmlSelectItem
    Private _NegotiationMemo As XmlNegotiationMemo
    Private _FollowUpResult As XmlFollowUpResult
    Private _Vehicle As Collection(Of XmlVehicle)
    Private _Customer As XmlCustomer
    Private _FamilyInformation As XmlFamilyInformation
    Private _Hobby As XmlHobby

    Private _SalesLocal As XmlSalesLocal

    Public Property Head() As XmlHead
        Get
            Return _Head
        End Get
        Set(ByVal Value As XmlHead)
            _Head = Value
        End Set
    End Property

    Public Property Common() As XmlCommon
        Get
            Return _Common
        End Get
        Set(ByVal Value As XmlCommon)
            _Common = Value
        End Set
    End Property

    Public Property FollowUp() As XmlFollowUp
        Get
            Return _FollowUp
        End Get
        Set(ByVal Value As XmlFollowUp)
            _FollowUp = Value
        End Set
    End Property

    Public Property CompetitorSeries() As XmlCompetitorSeries
        Get
            Return _CompetitorSeries
        End Get
        Set(ByVal Value As XmlCompetitorSeries)
            _CompetitorSeries = Value
        End Set
    End Property

    Public Property Action() As XmlAction
        Get
            Return _Action
        End Get
        Set(ByVal Value As XmlAction)
            _Action = Value
        End Set
    End Property

    Public Property SalesCondition() As XmlSalesCondition
        Get
            Return _SalesCondition
        End Get
        Set(ByVal Value As XmlSalesCondition)
            _SalesCondition = Value
        End Set
    End Property

    Public Property SelectItem() As XmlSelectItem
        Get
            Return _SelectItem
        End Get
        Set(ByVal Value As XmlSelectItem)
            _SelectItem = Value
        End Set
    End Property

    Public Property NegotiationMemo() As XmlNegotiationMemo
        Get
            Return _NegotiationMemo
        End Get
        Set(ByVal Value As XmlNegotiationMemo)
            _NegotiationMemo = Value
        End Set
    End Property

    Public Property FollowUpResult() As XmlFollowUpResult
        Get
            Return _FollowUpResult
        End Get
        Set(ByVal Value As XmlFollowUpResult)
            _FollowUpResult = Value
        End Set
    End Property

    Public Property Vehicle() As Collection(Of XmlVehicle)
        Get
            Return _Vehicle
        End Get
        Set(ByVal Value As Collection(Of XmlVehicle))
            _Vehicle = Value
        End Set
    End Property

    Public Property Customer() As XmlCustomer
        Get
            Return _Customer
        End Get
        Set(ByVal Value As XmlCustomer)
            _Customer = Value
        End Set
    End Property

    Public Property FamilyInformation() As XmlFamilyInformation
        Get
            Return _FamilyInformation
        End Get
        Set(ByVal Value As XmlFamilyInformation)
            _FamilyInformation = Value
        End Set
    End Property

    Public Property Hobby() As XmlHobby
        Get
            Return _Hobby
        End Get
        Set(ByVal Value As XmlHobby)
            _Hobby = Value
        End Set
    End Property

    Public Property SalesLocal() As XmlSalesLocal
        Get
            Return _SalesLocal
        End Get
        Set(ByVal Value As XmlSalesLocal)
            _SalesLocal = Value
        End Set
    End Property

End Class
