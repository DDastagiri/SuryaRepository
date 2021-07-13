'─────────────────────────────────────
'IC3800709WebServiceClassBusinessLogic_CreateXml.vb
'─────────────────────────────────────
'機能： 顧客検索用情報取得XML作成クラス定義
'補足： XML作成用クラスの定義
'作成： 2013/12/26 TMEJ 陳　 TMEJ次世代サービス 工程管理機能開発
'更新：
'─────────────────────────────────────

Imports System.IO
Imports System.Text
Imports System.Xml.Serialization


Partial Class IC3800709BusinessLogic


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
    ''' 顧客検索連携XMLクラス
    ''' </summary>
    ''' <remarks></remarks>
    <XmlRoot("GetCustomer")>
    Public Class CustomerSearchXmlDocumentClass

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
            Private CommonValue As New CommonTag

            ''' <summary>
            ''' SearchConditionタグ
            ''' </summary>
            ''' <remarks></remarks>
            Private SearchConditionValue As New SearchConditionTag



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
            <XmlElementAttribute(ElementName:="SearchCondition", IsNullable:=True)> _
            Public Property SearchCondition As SearchConditionTag

                Set(ByVal value As SearchConditionTag)

                    SearchConditionValue = value

                End Set

                Get

                    Return SearchConditionValue

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
                ''' StaffCodeタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private StaffCodeValue As String


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
                ''' StaffCodeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="StaffCode", IsNullable:=False)> _
                Public Property StaffCode As String

                    Set(ByVal value As String)

                        StaffCodeValue = value

                    End Set

                    Get

                        If StaffCodeValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(StaffCodeValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return StaffCodeValue
                            'Return String.Concat(CDataSectionFront, StaffCodeValue, CDataSectionBack)

                        End If

                    End Get

                End Property

            End Class

            ''' <summary>
            ''' SearchConditionTagXMLクラス
            ''' </summary>
            ''' <remarks></remarks>
            Public Class SearchConditionTag

                ''' <summary>
                ''' Startタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private StartValue As String

                ''' <summary>
                ''' Countタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private CountValue As String

                ''' <summary>
                ''' Sort1タグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private Sort1Value As String

                ''' <summary>
                ''' Sort2タグ
                ''' </summary>
                ''' <remarks></remarks>
                Private Sort2Value As String

                ''' <summary>
                ''' VclRegNoタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private VclRegNoValue As String

                ''' <summary>
                ''' VclRegNo_MatchTypeタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private VclRegNo_MatchTypeValue As String

                ''' <summary>
                ''' CustomerNameタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private CustomerNameValue As String

                ''' <summary>
                ''' CustomerName_MatchTypeタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private CustomerName_MatchValue As String

                ''' <summary>
                ''' Vinタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private VinValue As String

                ''' <summary>
                ''' Vin_MatchTypeタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private Vin_MatchTypeValue As String

                ''' <summary>
                ''' BasRezidタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private BasRezidValue As String

                ''' <summary>
                ''' BasRezid_MatchTypeタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private BasRezid_MatchTypeValue As String

                ''' <summary>
                ''' R_Oタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private R_OValue As String

                ''' <summary>
                ''' R_O_MatchTypeタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private R_O_MatchTypeValue As String

                ''' <summary>
                ''' TelNumberタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private TelNumberValue As String

                ''' <summary>
                ''' TelNumber_MatchTypeタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private TelNumber_MatchTypeValue As String



                ''' <summary>
                ''' Startタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="Start", IsNullable:=True)> _
                Public Property Start As String

                    Set(ByVal value As String)

                        StartValue = value

                    End Set

                    Get

                        Return StartValue

                    End Get

                End Property

                ''' <summary>
                ''' CountTagタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="Count", IsNullable:=True)> _
                Public Property Count As String

                    Set(ByVal value As String)

                        CountValue = value

                    End Set

                    Get

                        Return CountValue

                    End Get

                End Property

                ''' <summary>
                ''' Sort1タグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="Sort1", IsNullable:=True)> _
                Public Property Sort1 As String

                    Set(ByVal value As String)

                        Sort1Value = value

                    End Set

                    Get

                        Return Sort1Value

                    End Get

                End Property

                ''' <summary>
                ''' Sort2タグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="Sort2", IsNullable:=False)> _
                Public Property Sort2 As String

                    Set(ByVal value As String)

                        Sort2Value = value

                    End Set

                    Get

                        Return Sort2Value

                    End Get

                End Property

                ''' <summary>
                ''' VclRegNoタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="VclRegNo", IsNullable:=False)> _
                Public Property VclRegNo As String

                    Set(ByVal value As String)

                        VclRegNoValue = value

                    End Set

                    Get

                        If VclRegNoValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(VclRegNoValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return VclRegNoValue
                            'Return String.Concat(CDataSectionFront, VclRegNoValue, CDataSectionBack)

                        End If

                    End Get

                End Property

                ''' <summary>
                ''' VclRegNo_MatchTypeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="VclRegNo_MatchType", IsNullable:=False)> _
                Public Property VclRegNo_MatchType As String

                    Set(ByVal value As String)

                        VclRegNo_MatchTypeValue = value

                    End Set

                    Get

                        Return VclRegNo_MatchTypeValue

                    End Get

                End Property

                ''' <summary>
                ''' CustomerNameタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="CustomerName", IsNullable:=False)> _
                Public Property CustomerName As String

                    Set(ByVal value As String)

                        CustomerNameValue = value

                    End Set

                    Get

                        If CustomerNameValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(CustomerNameValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return CustomerNameValue
                            'Return String.Concat(CDataSectionFront, CustomerNameValue, CDataSectionBack)

                        End If

                    End Get

                End Property

                ''' <summary>
                ''' CustomerName_MatchTypeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="CustomerName_MatchType", IsNullable:=False)> _
                Public Property CustomerName_MatchType As String

                    Set(ByVal value As String)

                        CustomerName_MatchValue = value

                    End Set

                    Get

                        Return CustomerName_MatchValue

                    End Get

                End Property

                ''' <summary>
                ''' Vinタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="Vin", IsNullable:=False)> _
                Public Property Vin As String

                    Set(ByVal value As String)

                        VinValue = value

                    End Set

                    Get

                        If VinValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(VinValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return VinValue
                            'Return String.Concat(CDataSectionFront, VinValue, CDataSectionBack)

                        End If

                    End Get

                End Property

                ''' <summary>
                ''' Vin_MatchTypeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="Vin_MatchType", IsNullable:=False)> _
                Public Property Vin_MatchType As String

                    Set(ByVal value As String)

                        Vin_MatchTypeValue = value

                    End Set

                    Get

                        Return Vin_MatchTypeValue

                    End Get

                End Property

                ''' <summary>
                ''' BasRezidタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="BasRezid", IsNullable:=False)> _
                Public Property BasRezid As String

                    Set(ByVal value As String)

                        BasRezidValue = value

                    End Set

                    Get

                        Return BasRezidValue

                    End Get

                End Property

                ''' <summary>
                ''' BasRezid_MatchTypeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="BasRezid_MatchType", IsNullable:=False)> _
                Public Property BasRezid_MatchType As String

                    Set(ByVal value As String)

                        BasRezid_MatchTypeValue = value

                    End Set

                    Get

                        Return BasRezid_MatchTypeValue

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

                        Return R_OValue

                    End Get

                End Property

                ''' <summary>
                ''' R_O_MatchTypeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="R_O_MatchType", IsNullable:=False)> _
                Public Property R_O_MatchType As String

                    Set(ByVal value As String)

                        R_O_MatchTypeValue = value

                    End Set

                    Get

                        Return R_O_MatchTypeValue

                    End Get

                End Property

                ''' <summary>
                ''' TelNumberタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="TelNumber", IsNullable:=False)> _
                Public Property TelNumber As String

                    Set(ByVal value As String)

                        TelNumberValue = value

                    End Set

                    Get

                        If TelNumberValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(TelNumberValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return TelNumberValue
                            'Return String.Concat(CDataSectionFront, TelNumberValue, CDataSectionBack)

                        End If

                    End Get

                End Property

                ''' <summary>
                ''' TelNumber_MatchTypeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="TelNumber_MatchType", IsNullable:=False)> _
                Public Property TelNumber_MatchType As String

                    Set(ByVal value As String)

                        TelNumber_MatchTypeValue = value

                    End Set

                    Get

                        Return TelNumber_MatchTypeValue

                    End Get

                End Property

            End Class

        End Class

    End Class

End Class
