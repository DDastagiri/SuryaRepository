Imports System.Runtime.Serialization

''' <summary>
''' setting JSONファイル 全データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class SettingListJson
    Inherits AbstractJson

    Private _ALWAYS_DEFAULT_PARTS As Boolean
    Private _sales_point_info As SettingSalesPointInfoJson

    ''' <summary>
    ''' デフォルトパーツフラグの設定と取得を行う
    ''' </summary>
    ''' <value>デフォルトパーツフラグの設定</value>
    ''' <returns>デフォルトパーツフラグの設定</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property ALWAYS_DEFAULT_PARTS As Boolean
        Get
            Return _ALWAYS_DEFAULT_PARTS
        End Get
        Set(value As Boolean)
            _ALWAYS_DEFAULT_PARTS = value
        End Set
    End Property

    ''' <summary>
    ''' セールスポイント基本設定の設定と取得を行う
    ''' </summary>
    ''' <value>セールスポイント基本設定</value>
    ''' <returns>セールスポイント基本設定</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property sales_point_info As SettingSalesPointInfoJson
        Get
            Return _sales_point_info
        End Get
        Set(value As SettingSalesPointInfoJson)
            _sales_point_info = value
        End Set
    End Property

End Class
