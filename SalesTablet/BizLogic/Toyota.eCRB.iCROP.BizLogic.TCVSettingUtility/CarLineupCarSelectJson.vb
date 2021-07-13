Imports System.Runtime.Serialization

''' <summary>
''' car_lineup JSONファイル 車種選択データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class CarLineupCarSelectJson
    Inherits AbstractJson

    Private _defaultCarSeries As String
    Private _carList As List(Of CarLineupCarListJson)

    ''' <summary>
    ''' デフォルト車種シリーズの設定と取得を行う
    ''' </summary>
    ''' <value>デフォルト車種シリーズ</value>
    ''' <returns>デフォルト車種シリーズ</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property defaultCarSeries As String
        Get
            Return _defaultCarSeries
        End Get
        Set(value As String)
            _defaultCarSeries = value
        End Set
    End Property

    ''' <summary>
    ''' 車種リストの設定と取得を行う
    ''' </summary>
    ''' <value>車種リスト</value>
    ''' <returns>車種リスト</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property carList As List(Of CarLineupCarListJson)
        Get
            Return _carList
        End Get
        Set(value As List(Of CarLineupCarListJson))
            _carList = value
        End Set
    End Property

End Class
