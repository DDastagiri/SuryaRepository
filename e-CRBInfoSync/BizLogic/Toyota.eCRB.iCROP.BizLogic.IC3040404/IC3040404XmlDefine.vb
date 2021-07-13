Imports System.Runtime.Serialization

Namespace IC3040404.BizLogic

    ''' <summary>
    ''' Xml変換テーブルクラス   2012/12/12
    ''' </summary>
    ''' <typeparam name="TKey"></typeparam>
    ''' <typeparam name="TValue"></typeparam>
    ''' <remarks></remarks>
    ''' 
    <SerializableAttribute()> _
    Public Class XmlDefine(Of TKey, TValue)
        Inherits System.Collections.Generic.Dictionary(Of TKey, TValue)
        Implements ISerializable


        Public Sub New()
            '
        End Sub

        'ルールセット用
        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)

            MyBase.New(info, context)

        End Sub

        Public Shared Function Build() As XmlDefine(Of TKey, TValue)

            Dim BuildData As New XmlDefine(Of TKey, TValue)
            Return BuildData
        End Function

        Public Function Response(ByVal key As TKey, ByVal value As TValue) As XmlDefine(Of TKey, TValue)
            Me(key) = value
            Return Me
        End Function

    End Class

End Namespace