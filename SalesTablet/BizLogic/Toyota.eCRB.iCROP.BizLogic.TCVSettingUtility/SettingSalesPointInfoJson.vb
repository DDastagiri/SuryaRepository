Imports System.Runtime.Serialization

''' <summary>
''' setting JSONファイル セールスポイント基本設定データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class SettingSalesPointInfoJson
    Inherits AbstractJson

    Private _def_exterior_angle As String
    Private _def_interior_id As String

    ''' <summary>
    ''' 外装デフォルトアングルの設定と取得を行う
    ''' </summary>
    ''' <value>外装デフォルトアングル</value>
    ''' <returns>外装デフォルトアングル</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property def_exterior_angle As String
        Get
            Return _def_exterior_angle
        End Get
        Set(value As String)
            _def_exterior_angle = value
        End Set
    End Property

    ''' <summary>
    ''' 内装デフォルトIDの設定と取得を行う
    ''' </summary>
    ''' <value>内装デフォルトID</value>
    ''' <returns>内装デフォルトID</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property def_interior_id As String
        Get
            Return _def_interior_id
        End Get
        Set(value As String)
            _def_interior_id = value
        End Set
    End Property

End Class
