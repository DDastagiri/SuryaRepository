Imports System.Runtime.Serialization

''' <summary>
''' tcv_web JSONファイル 再生環境情報データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class TcvWebPlayerInfoJson
    Inherits AbstractJson

    Private _introduction As TcvWebIntroductionJson

    ''' <summary>
    ''' 環境の設定と取得を行う
    ''' </summary>
    ''' <value>環境</value>
    ''' <returns>環境</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property introduction As TcvWebIntroductionJson
        Get
            Return _introduction
        End Get
        Set(value As TcvWebIntroductionJson)
            _introduction = value
        End Set
    End Property

End Class
