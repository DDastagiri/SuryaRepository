Imports System.Runtime.Serialization

''' <summary>
''' interior JSONファイル 全データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class InteriorListJson
    Inherits AbstractJson

    Dim _interior As List(Of InteriorJson)

    ''' <summary>
    ''' インテリア情報の設定と取得を行う
    ''' </summary>
    ''' <value>インテリア情報</value>
    ''' <returns>インテリア情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property interior As List(Of InteriorJson)
        Get
            Return _interior
        End Get
        Set(value As List(Of InteriorJson))
            _interior = value
        End Set
    End Property

End Class
