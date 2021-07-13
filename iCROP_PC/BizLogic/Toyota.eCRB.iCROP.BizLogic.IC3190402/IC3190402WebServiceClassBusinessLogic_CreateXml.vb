'─────────────────────────────────────
'IC3190402WebServiceClassBusinessLogic_CreateXml.vb
'─────────────────────────────────────
'機能： 部品ステータス情報取得XML作成クラス定義
'補足： XML作成用クラスの定義
'作成： 2014/XX/XX NEC村瀬
'更新：
'─────────────────────────────────────

Imports System.IO
Imports System.Text
Imports System.Xml.Serialization

Partial Class IC3190402BusinessLogic

#Region "定数"

    ''' <summary>
    ''' CDATAセクション文字列フロント部分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CDataSectionFront As String = "<![CDATA["


    ''' <summary>
    ''' CDATAセクション文字列バック部分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CDataSectionBack As String = "]]>"

#End Region


    ''' <summary>
    ''' 部品ステータス連携XMLクラス
    ''' </summary>
    ''' <remarks></remarks>
    <XmlRoot("Parts")>
    Public Class PartsSearchXmlDocumentClass

        ''' <summary>
        ''' Headタグ
        ''' </summary>
        ''' <remarks></remarks>
        Private HeadValue As New HeadTag

        ''' <summary>
        ''' Detailタグ
        ''' </summary>
        ''' <remarks></remarks>
        Private DetailValue As New DetailTag

        ''' <summary>
        ''' Headタグ用プロパティ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlElementAttribute(ElementName:="head", IsNullable:=True)> _
        Public Property Head As HeadTag
            Set(ByVal value As HeadTag)
                HeadValue = value
            End Set
            Get
                Return HeadValue
            End Get
        End Property

        ''' <summary>
        ''' Detailタグ用プロパティ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        <XmlElementAttribute(ElementName:="Detail", IsNullable:=True)> _
        Public Property Detail As DetailTag
            Set(ByVal value As DetailTag)
                DetailValue = value
            End Set
            Get
                Return DetailValue
            End Get
        End Property

        ''' <summary>
        ''' HeadTagXMLクラス
        ''' </summary>
        ''' <remarks></remarks>
        Public Class HeadTag

            ''' <summary>
            ''' MessageIDタグ
            ''' </summary>
            ''' <remarks></remarks>
            Private MessageIdValue As String

            ''' <summary>
            ''' CountryCodeタグ
            ''' </summary>
            ''' <remarks></remarks>
            Private CountryCodeValue As String

            ''' <summary>
            ''' LinkSystemCodeタグ
            ''' </summary>
            ''' <remarks></remarks>
            Private LinkSystemCodeValue As String

            ''' <summary>
            ''' TransmissionDateタグ
            ''' </summary>
            ''' <remarks></remarks>
            Private TransmissionDateValue As String

            ''' <summary>
            ''' MessageIDタグ用プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            <XmlElementAttribute(ElementName:="MessageID", IsNullable:=False)> _
            Public Property MessageId As String
                Set(ByVal value As String)
                    MessageIdValue = value
                End Set
                Get
                    If String.IsNullOrEmpty(MessageIdValue) Then
                        Return String.Empty
                    Else
                        '値があるときのみCDATAセクションをつける
                        Return MessageIdValue
                    End If

                End Get

            End Property

            ''' <summary>
            ''' CountryCodeタグ用プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            <XmlElementAttribute(ElementName:="CountryCode", IsNullable:=False)> _
            Public Property CountryCode As String
                Set(ByVal value As String)
                    CountryCodeValue = value
                End Set
                Get
                    If String.IsNullOrEmpty(CountryCodeValue) Then
                        Return String.Empty
                    Else
                        '値があるときのみCDATAセクションをつける
                        Return CountryCodeValue
                    End If
                End Get
            End Property

            ''' <summary>
            ''' LinkSystemCodeタグ用プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            <XmlElementAttribute(ElementName:="LinkSystemCode", IsNullable:=False)> _
            Public Property LinkSystemCode As String
                Set(ByVal value As String)
                    LinkSystemCodeValue = value
                End Set
                Get
                    If String.IsNullOrEmpty(LinkSystemCodeValue) Then
                        Return String.Empty
                    Else
                        '値があるときのみCDATAセクションをつける
                        Return LinkSystemCodeValue
                    End If
                End Get
            End Property

            ''' <summary>
            ''' TransmissionDateタグ用プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            <XmlElementAttribute(ElementName:="TransmissionDate", IsNullable:=False)> _
            Public Property TransmissionDate As String
                Set(ByVal value As String)
                    TransmissionDateValue = value
                End Set
                Get
                    Return TransmissionDateValue
                End Get
            End Property

        End Class

        ''' <summary>
        ''' DetailTagXMLクラス
        ''' </summary>
        ''' <remarks></remarks>
        Public Class DetailTag

            ''' <summary>
            ''' Commonタグ
            ''' </summary>
            ''' <remarks></remarks>
            Private CommonValue As New CommonTag

            ''' <summary>
            ''' PartsSearchConditionタグ
            ''' </summary>
            ''' <remarks></remarks>
            Private PartsSearchConditionValue As New List(Of PartsSearchConditionTag)

            ''' <summary>
            ''' Commonタグ用プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            <XmlElementAttribute(ElementName:="Common", IsNullable:=True)> _
            Public Property Common As CommonTag
                Set(ByVal value As CommonTag)
                    CommonValue = value
                End Set
                Get
                    Return CommonValue
                End Get
            End Property

            ''' <summary>
            ''' SearchConditionタグ用プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            <XmlElementAttribute(ElementName:="PartsSearchCondition", IsNullable:=True)> _
            Public Property PartsSearchCondition As List(Of PartsSearchConditionTag)
                Set(ByVal value As List(Of PartsSearchConditionTag))
                    PartsSearchConditionValue = value
                End Set
                Get
                    Return PartsSearchConditionValue
                End Get
            End Property

            ''' <summary>
            ''' CommonTagXMLクラス
            ''' </summary>
            ''' <remarks></remarks>
            Public Class CommonTag

                ''' <summary>
                ''' DealerCodeタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private DealerCodeValue As String

                ''' <summary>
                ''' BranchCodeタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private BranchCodeValue As String

                ''' <summary>
                ''' DealerCodeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="DealerCode", IsNullable:=False)> _
                Public Property DealerCode As String
                    Set(ByVal value As String)
                        DealerCodeValue = value
                    End Set
                    Get
                        If DealerCodeValue Is Nothing Then
                            Return Nothing
                        ElseIf String.IsNullOrEmpty(DealerCodeValue) Then
                            Return String.Empty
                        Else
                            Return DealerCodeValue
                        End If
                    End Get
                End Property

                ''' <summary>
                ''' BranchCodeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="BranchCode", IsNullable:=False)> _
                Public Property BranchCode As String
                    Set(ByVal value As String)
                        BranchCodeValue = value
                    End Set
                    Get
                        If BranchCodeValue Is Nothing Then
                            Return Nothing
                        ElseIf String.IsNullOrEmpty(BranchCodeValue) Then
                            Return String.Empty
                        Else
                            Return BranchCodeValue
                        End If
                    End Get
                End Property

            End Class

            ''' <summary>
            ''' SearchConditionTagXMLクラス
            ''' </summary>
            ''' <remarks></remarks>
            Public Class PartsSearchConditionTag

                ''' <summary>
                ''' R_Oタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private R_OValue As String

                ''' <summary>
                ''' R_O_SEQNOタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private R_O_SEQNOValue As String

                ''' <summary>
                ''' R_Oタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="R_O", IsNullable:=False)> _
                Public Property R_O As String
                    Set(ByVal value As String)
                        R_OValue = value
                    End Set
                    Get
                        Return R_OValue
                    End Get
                End Property

                ''' <summary>
                ''' R_O_SEQNOタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="R_O_SEQNO", IsNullable:=False)> _
                Public Property R_O_SEQNO As String
                    Set(ByVal value As String)
                        R_O_SEQNOValue = value
                    End Set
                    Get
                        Return R_O_SEQNOValue
                    End Get
                End Property

            End Class

        End Class

    End Class

End Class
