'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250101DataSet.vb
'─────────────────────────────────────
'機能： 商品訴求メイン（車両）DataSet.vb
'補足： 
'作成： 2014/02/XX NEC 鈴木
'更新： 2014/03/xx NEC 上野
'更新： 2014/04/xx NEC 脇谷
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports System.IO
Imports System.Text
Imports System.Xml.Serialization

Public Class SC3250101WebServiceClassBusinessLogic_CreateXml

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

#Region "ServiceItems"

    ''' <summary>
    ''' TOPSERV送信
    ''' </summary>
    ''' <remarks></remarks>
    <XmlRoot("ServiceItems")>
    Public Class ServiceItemsXmlDocumentClass

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
                        'Return String.Concat(CDataSectionFront, MessageIdValue, CDataSectionBack)

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
                        'Return String.Concat(CDataSectionFront, CountryCodeValue, CDataSectionBack)

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
                        'Return String.Concat(CDataSectionFront, LinkSystemCodeValue, CDataSectionBack)

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
            <XmlElementAttribute(ElementName:="Common", IsNullable:=True)> _
            Public Common As New CommonTag

            ''' <summary>
            ''' ServiceItemsタグ
            ''' </summary>
            ''' <remarks></remarks>
            <XmlElementAttribute(ElementName:="ServiceItemsForCart", IsNullable:=True)> _
            Public ServiceItems() As ServiceItemsTag


            ''' <summary>
            ''' JOBIDsタグ
            ''' </summary>
            ''' <remarks></remarks>
            <XmlElementAttribute(ElementName:="JOBIDs", IsNullable:=True)> _
            Public JOBIDs() As JOBIDsTag



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
                ''' SAChipIDタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private SAChipIDValue As String

                ''' <summary>
                ''' VINタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private VINValue As String

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

                ''' <summary>
                ''' SAChipIDタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="SAChipID", IsNullable:=False)> _
                Public Property SAChipID As String

                    Set(ByVal value As String)

                        SAChipIDValue = value

                    End Set

                    Get

                        If SAChipIDValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(SAChipIDValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return SAChipIDValue
                            'Return String.Concat(CDataSectionFront, SAChipID, CDataSectionBack)

                        End If

                    End Get

                End Property

                ''' <summary>
                ''' VINタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="VIN", IsNullable:=False)> _
                Public Property VIN As String

                    Set(ByVal value As String)

                        VINValue = value

                    End Set

                    Get

                        If VINValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(VINValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return VINValue
                            'Return String.Concat(CDataSectionFront, VIN, CDataSectionBack)

                        End If

                    End Get
                End Property
            End Class

            ''' <summary>
            ''' ServiceItemsXMLクラス
            ''' </summary>
            ''' <remarks></remarks>
            Public Class ServiceItemsTag

                ''' <summary>
                ''' ServiceItemCodeタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private ServiceItemCodeValue As String

                ''' <summary>
                ''' ServiceTypeCodeタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private ServiceTypeCodeValue As String

                ''' <summary>
                ''' ServiceItemCodeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="ServiceItemCode", IsNullable:=False)> _
                Public Property ServiceItemCode As String

                    Set(ByVal value As String)

                        ServiceItemCodeValue = value

                    End Set

                    Get

                        If ServiceItemCodeValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(ServiceItemCodeValue) Then

                            Return String.Empty

                        Else

                            Return ServiceItemCodeValue

                        End If

                    End Get

                End Property

                ''' <summary>
                ''' ServiceTypeCodeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="ServiceTypeCode", IsNullable:=False)> _
                Public Property ServiceTypeCode As String

                    Set(ByVal value As String)

                        ServiceTypeCodeValue = value

                    End Set

                    Get

                        If ServiceTypeCodeValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(ServiceTypeCodeValue) Then

                            Return String.Empty

                        Else

                            Return ServiceTypeCodeValue

                        End If

                    End Get
                End Property
            End Class


            ' ''' <summary>
            ' ''' ServiceItemsタグ用プロパティ
            ' ''' </summary>
            ' ''' <value></value>
            ' ''' <returns></returns>
            ' ''' <remarks></remarks>
            '<XmlElementAttribute(ElementName:="ServiceItems", IsNullable:=True)> _
            'Public Property ServiceItems() As ServiceItemsTag

            '    Set(ByVal value As ServiceItemsTag)

            '        ServiceItemsValue = value

            '    End Set

            '    Get

            '        Return ServiceItemsValue

            '    End Get

            'End Property


            ' ''' <summary>
            ' ''' JOBIDsタグ用プロパティ
            ' ''' </summary>
            ' ''' <value></value>
            ' ''' <returns></returns>
            ' ''' <remarks></remarks>
            '<XmlElementAttribute(ElementName:="JOBIDs", IsNullable:=True)> _
            'Public Property JOBIDs() As JOBIDsTag

            '    Set(ByVal value As JOBIDsTag)

            '        JOBIDsValue = value

            '    End Set

            '    Get

            '        Return JOBIDsValue

            '    End Get

            'End Property




            ''' <summary>
            ''' JOBIDsXMLクラス
            ''' </summary>
            ''' <remarks></remarks>
            Public Class JOBIDsTag

            End Class

        End Class
    End Class
#End Region

#Region "Request_Mileage"

    ''' <summary>
    ''' TOPSERV送信
    ''' </summary>
    ''' <remarks></remarks>
    <XmlRoot("Request_Mileage")>
    Public Class Request_MileageXmlDocumentClass

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
                        'Return String.Concat(CDataSectionFront, MessageIdValue, CDataSectionBack)
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
                        'Return String.Concat(CDataSectionFront, CountryCodeValue, CDataSectionBack)
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
                        'Return String.Concat(CDataSectionFront, LinkSystemCodeValue, CDataSectionBack)
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
            <XmlElementAttribute(ElementName:="Common", IsNullable:=True)> _
            Public Common As New CommonTag

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
                ''' R_Oタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private R_OValue As String

                ''' <summary>
                ''' BASREZIDタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private BASREZIDValue As String

                ''' <summary>
                ''' SAChipIDタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private SAChipIDValue As String

                ''' <summary>
                ''' VINタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private VINValue As String

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
                        If R_OValue Is Nothing Then
                            Return Nothing
                        ElseIf String.IsNullOrEmpty(R_OValue) Then
                            Return String.Empty
                        Else
                            Return R_OValue
                        End If
                    End Get
                End Property

                ''' <summary>
                ''' BASREZIDタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="BASREZID", IsNullable:=False)> _
                Public Property BASREZID As String
                    Set(ByVal value As String)
                        BASREZIDValue = value
                    End Set

                    Get
                        If BASREZIDValue Is Nothing Then
                            Return Nothing
                        ElseIf String.IsNullOrEmpty(BASREZIDValue) Then
                            Return String.Empty
                        Else
                            Return BASREZIDValue
                        End If
                    End Get
                End Property


                ''' <summary>
                ''' SAChipIDタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="SAChipID", IsNullable:=False)> _
                Public Property SAChipID As String

                    Set(ByVal value As String)

                        SAChipIDValue = value

                    End Set

                    Get

                        If SAChipIDValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(SAChipIDValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return SAChipIDValue
                            'Return String.Concat(CDataSectionFront, SAChipID, CDataSectionBack)

                        End If

                    End Get

                End Property

                ''' <summary>
                ''' VINタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="VIN", IsNullable:=False)> _
                Public Property VIN As String

                    Set(ByVal value As String)

                        VINValue = value

                    End Set

                    Get

                        If VINValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(VINValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return VINValue
                            'Return String.Concat(CDataSectionFront, VIN, CDataSectionBack)

                        End If

                    End Get
                End Property
            End Class
        End Class
    End Class
#End Region

#Region "RoThumbnailCount"

    ''' <summary>
    ''' TOPSERV送信
    ''' </summary>
    ''' <remarks></remarks>
    <XmlRoot("RoThumbnailCount")>
    Public Class RoThumbnailCountXmlDocumentClass

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
        <XmlElementAttribute(ElementName:="Head", IsNullable:=True)> _
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
                        'Return String.Concat(CDataSectionFront, MessageIdValue, CDataSectionBack)

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
                        'Return String.Concat(CDataSectionFront, CountryCodeValue, CDataSectionBack)

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
                        'Return String.Concat(CDataSectionFront, LinkSystemCodeValue, CDataSectionBack)

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
            <XmlElementAttribute(ElementName:="Common", IsNullable:=True)> _
            Public Common As New CommonTag

            ''' <summary>
            ''' CommonTagXMLクラス
            ''' </summary>
            ''' <remarks></remarks>
            Public Class CommonTag
                ''' <summary>
                ''' SAChipIDタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private SAChipIDValue As String

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
                ''' PictModeタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private PictModeValue As String

                ''' <summary>
                ''' LinkSysTypeタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private LinkSysTypeValue As String

                ''' <summary>
                ''' SAChipIDタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="SAChipID", IsNullable:=False)> _
                Public Property SAChipID As String
                    Set(ByVal value As String)
                        SAChipIDValue = value
                    End Set

                    Get
                        If SAChipIDValue Is Nothing Then
                            Return Nothing
                        ElseIf String.IsNullOrEmpty(SAChipIDValue) Then
                            Return String.Empty
                        Else
                            '値があるときのみCDATAセクションをつける
                            Return SAChipIDValue
                            'Return String.Concat(CDataSectionFront, SAChipID, CDataSectionBack)
                        End If
                    End Get
                End Property

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
                        If R_OValue Is Nothing Then
                            Return Nothing
                        ElseIf String.IsNullOrEmpty(R_OValue) Then
                            Return String.Empty
                        Else
                            Return R_OValue
                        End If
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
                        If R_O_SEQNOValue Is Nothing Then
                            Return Nothing
                        ElseIf String.IsNullOrEmpty(R_O_SEQNOValue) Then
                            Return String.Empty
                        Else
                            Return R_O_SEQNOValue
                        End If
                    End Get
                End Property

                ''' <summary>
                ''' PictModeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="PictMode", IsNullable:=False)> _
                Public Property PictMode As String
                    Set(ByVal value As String)
                        PictModeValue = value
                    End Set

                    Get
                        If PictModeValue Is Nothing Then
                            Return Nothing
                        ElseIf String.IsNullOrEmpty(PictModeValue) Then
                            Return String.Empty
                        Else
                            '値があるときのみCDATAセクションをつける
                            Return PictModeValue
                            'Return String.Concat(CDataSectionFront, VIN, CDataSectionBack)
                        End If
                    End Get
                End Property

                ''' <summary>
                ''' LinkSysTypeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="LinkSysType", IsNullable:=False)> _
                Public Property LinkSysType As String
                    Set(ByVal value As String)
                        LinkSysTypeValue = value
                    End Set

                    Get
                        If LinkSysTypeValue Is Nothing Then
                            Return Nothing
                        ElseIf String.IsNullOrEmpty(LinkSysTypeValue) Then
                            Return String.Empty
                        Else
                            '値があるときのみCDATAセクションをつける
                            Return LinkSysTypeValue
                            'Return String.Concat(CDataSectionFront, VIN, CDataSectionBack)
                        End If
                    End Get
                End Property
            End Class
        End Class
    End Class
#End Region



End Class
