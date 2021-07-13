Imports System.Runtime.Serialization

''' <summary>
''' car_lineup JSONファイル 全データ格納クラス
''' </summary>
''' <remarks></remarks>
Public Class CarLineupCarSelectListJson
    Inherits AbstractJson

    Private _carselect As CarLineupCarSelectJson

    ''' <summary>
    ''' 車種選択の設定と取得を行う
    ''' </summary>
    ''' <value>車種選択</value>
    ''' <returns>車種選択</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property carselect As CarLineupCarSelectJson
        Get
            Return _carselect
        End Get
        Set(value As CarLineupCarSelectJson)
            _carselect = value
        End Set
    End Property

End Class
