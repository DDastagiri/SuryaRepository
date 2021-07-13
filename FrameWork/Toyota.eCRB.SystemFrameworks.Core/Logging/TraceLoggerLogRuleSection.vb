Imports System
Imports System.Configuration
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Namespace Toyota.eCRB.SystemFrameworks.Core

    Public Class TraceLoggerLogRuleSection
        Inherits ConfigurationSection

        <ConfigurationProperty("LogRules", IsDefaultCollection:=False)>
        <ConfigurationCollection(GetType(LogRules), AddItemName:="add", ClearItemsName:="clear", RemoveItemName:="remove")>
        Public ReadOnly Property Rules As LogRules
            Get
                Return TryCast(Me("LogRules"), LogRules)
            End Get
        End Property
    End Class

    Public Class LogRules
        Inherits ConfigurationElementCollection
        'Implements IEnumerable(Of LogRuleConfigElement)

        Public Overrides ReadOnly Property CollectionType As ConfigurationElementCollectionType
            Get
                Return ConfigurationElementCollectionType.AddRemoveClearMap
            End Get
        End Property

        Protected Overrides Function CreateNewElement() As ConfigurationElement
            Return New LogRuleConfigElement()
        End Function

        Protected Overrides Function GetElementKey(ByVal element As ConfigurationElement) As Object
            Return (CType(element, LogRuleConfigElement)).Key
        End Function

        Protected Overrides Sub BaseAdd(ByVal element As ConfigurationElement)
            BaseAdd(element, False)
        End Sub

        'Public Overloads Function GetEnumerator() As IEnumerator(Of LogRuleConfigElement)
        '    '    'Dim count As Integer = MyBase.Count
        '    '    'For i As Integer = 0 To count - 1
        '    '    '    Yield(TryCast(MyBase.BaseGet(i), LogRuleConfigElement))
        '    '    'Next
        'End Function
    End Class

    Public Class LogRuleConfigElement
        Inherits ConfigurationElement

        <ConfigurationProperty("key", IsRequired:=True, IsKey:=True)>
        Public ReadOnly Property Key As String
            Get
                Return CStr(Me("key"))
            End Get
        End Property

        <ConfigurationProperty("value", IsRequired:=True)>
        Public ReadOnly Property Value As String
            Get
                Return CStr(Me("value"))
            End Get
        End Property

        <ConfigurationProperty("Level", DefaultValue:="Information", IsRequired:=False)>
        Public ReadOnly Property Level As String
            Get
                Return CStr(Me("Level"))
            End Get
        End Property
    End Class
End Namespace
