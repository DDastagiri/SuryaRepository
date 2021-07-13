'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3080209BusinessLogic.vb
'─────────────────────────────────────
'機能： 活動履歴I/F
'補足： 
'作成： 2015/04/10 TCS 外崎 タブレットSPM操作性機能向上（活動履歴表示）
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core

Public Class IC3080209BusinessLogic
    Inherits BaseBusinessComponent

    Public Function GetContactHistory(ByVal requestXml As String) As Response

    End Function

#Region "Responseクラス"
    ''' <summary>
    ''' Responseクラス(応答用XMLのIFクラス)
    ''' </summary>
    ''' <remarks>応答用のXML情報を格納するクラス</remarks>
    <System.Xml.Serialization.XmlRoot("Response", Namespace:="http://tempuri.org/Response.xsd")> _
    Public Class Response

        ''' <summary>
        ''' Headタグの定義
        ''' </summary>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElementAttribute(ElementName:="Head", IsNullable:=False)> _
        Private outHead As RootHead

        ''' <summary>
        ''' Detailタグの定義
        ''' </summary>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElementAttribute(ElementName:="Detail", IsNullable:=False)> _
        Private outDetail As RootDetail

        ''' <summary>
        ''' Headerタグ用プロパティ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Head() As RootHead
            Set(ByVal value As RootHead)
                outHead = value
            End Set
            Get
                Return outHead
            End Get
        End Property

        ''' <summary>
        ''' Detailタグ用プロパティ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Detail() As RootDetail
            Set(ByVal value As RootDetail)
                outDetail = value
            End Set
            Get
                Return outDetail
            End Get
        End Property

        ''' <summary>
        ''' Headタグ用クラス
        ''' </summary>
        ''' <remarks></remarks>
        Public Class RootHead

            ''' <summary>
            ''' TransmissionDateタグの定義
            ''' </summary>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="TransmissionDate",
                                                          IsNullable:=False)> _
            Private outTransmissionDate As String

            ''' <summary>
            ''' TransmissionDateタグ用のプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="TransmissionDate",
                                                          IsNullable:=False)> _
            Public Property TransmissionDate As String
                Get
                    Return outTransmissionDate
                End Get
                Set(ByVal value As String)
                    outTransmissionDate = value
                End Set
            End Property
        End Class

        ''' <summary>
        ''' Detailタグ用クラス
        ''' </summary>
        ''' <remarks></remarks>
        Public Class RootDetail

            ''' <summary>
            ''' Commonタグの定義
            ''' </summary>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Common",
                                                          IsNullable:=False)> _
            Private outCommon As DetailCommon

            ''' <summary>
            ''' Commonタグ用のプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Common() As DetailCommon
                Set(ByVal value As DetailCommon)
                    outCommon = value
                End Set
                Get
                    Return outCommon
                End Get
            End Property

            ''' <summary>
            ''' Commonタグ用クラス
            ''' </summary>
            ''' <remarks></remarks>
            Public Class DetailCommon

                ''' <summary>
                ''' NoticeRequestIdタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="NoticeRequestId",
                                                              IsNullable:=False)> _
                Private outNoticeRequestId As String

                ''' <summary>
                ''' ResultIdタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="ResultId",
                                                              IsNullable:=False)> _
                Private outResultId As String

                ''' <summary>
                ''' Messageタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="Message",
                                                              IsNullable:=False)> _
                Private outMessage As String

                ''' <summary>
                ''' NoticeRequestIdタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property NoticeRequestId() As String
                    Set(ByVal value As String)
                        outNoticeRequestId = value
                    End Set
                    Get
                        Return outNoticeRequestId
                    End Get
                End Property

                ''' <summary>
                ''' ResultIdタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property ResultId() As String
                    Set(ByVal value As String)
                        outResultId = value
                    End Set
                    Get
                        Return outResultId
                    End Get
                End Property

                ''' <summary>
                ''' Messageタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property Message() As String
                    Set(ByVal value As String)
                        outMessage = value
                    End Set
                    Get
                        Return outMessage
                    End Get
                End Property
            End Class
        End Class
    End Class
#End Region

End Class
