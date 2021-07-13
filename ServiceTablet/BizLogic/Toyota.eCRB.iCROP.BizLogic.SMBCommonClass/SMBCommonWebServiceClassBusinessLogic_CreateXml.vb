'─────────────────────────────────────
'SMBCommonWebServiceClassBusinessLogic_CreateXml.vb
'─────────────────────────────────────
'機能： SMBCommonWebServiceClassXML作成クラス定義
'補足： XML作成用クラスの定義
'作成： 2013/08/20 TMEJ 河原 【A.STEP2】次世代e-CRBサービスタブレット 新DB適応に向けた機能開発
'更新： 2013/12/18 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
'更新：
'─────────────────────────────────────

Imports System.IO
Imports System.Text
Imports System.Xml.Serialization


Partial Class SMBCommonClassBusinessLogic


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
    ''' 予約連携XMLクラス
    ''' </summary>
    ''' <remarks></remarks>
    <XmlRoot("Update_Reserve")>
    Public Class XmlDocumentClass

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
                        Return String.Concat(CDataSectionFront, MessageIdValue, CDataSectionBack)

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
                        Return String.Concat(CDataSectionFront, CountryCodeValue, CDataSectionBack)

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
                        Return String.Concat(CDataSectionFront, LinkSystemCodeValue, CDataSectionBack)

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
            ''' ReserveInformationタグ
            ''' </summary>
            ''' <remarks></remarks>
            Private ReserveInformationValue As New ReserveInformationTag



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
            ''' ReserveInformationタグ用プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            <XmlElementAttribute(ElementName:="ReserveInformation", IsNullable:=True)> _
            Public Property ReserveInformation As ReserveInformationTag

                Set(ByVal value As ReserveInformationTag)

                    ReserveInformationValue = value

                End Set

                Get

                    Return ReserveInformationValue

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
                ''' CustomerCodeタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private CustomerCodeValue As String

                ''' <summary>
                ''' SalesBookingNumberタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private SalesBookingNumberValue As String

                ''' <summary>
                ''' Vinタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private VinValue As String



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

                            '値があるときのみCDATAセクションをつける
                            Return String.Concat(CDataSectionFront, DealerCodeValue, CDataSectionBack)

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

                            '値があるときのみCDATAセクションをつける
                            Return String.Concat(CDataSectionFront, BranchCodeValue, CDataSectionBack)

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
                            Return String.Concat(CDataSectionFront, StaffCodeValue, CDataSectionBack)

                        End If

                    End Get

                End Property

                ''' <summary>
                ''' CustomerCodeタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="CustomerCode", IsNullable:=False)> _
                Public Property CustomerCode As String

                    Set(ByVal value As String)

                        CustomerCodeValue = value

                    End Set

                    Get

                        If CustomerCodeValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(CustomerCodeValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return String.Concat(CDataSectionFront, CustomerCodeValue, CDataSectionBack)

                        End If

                    End Get

                End Property

                ''' <summary>
                ''' SalesBookingNumberタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="SalesBookingNumber", IsNullable:=False)> _
                Public Property SalesBookingNumber As String

                    Set(ByVal value As String)

                        SalesBookingNumberValue = value

                    End Set

                    Get

                        If SalesBookingNumberValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(SalesBookingNumberValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return String.Concat(CDataSectionFront, SalesBookingNumberValue, CDataSectionBack)

                        End If

                    End Get

                End Property

                ''' <summary>
                ''' Vinタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElement(ElementName:="Vin", IsNullable:=False)> _
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
                            Return String.Concat(CDataSectionFront, VinValue, CDataSectionBack)

                        End If

                    End Get

                End Property

            End Class

            ''' <summary>
            ''' ReserveInformationTagXMLクラス
            ''' </summary>
            ''' <remarks></remarks>
            Public Class ReserveInformationTag

                ''' <summary>
                ''' Reserve_CustomerInformationタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private ReserveCustomerInformationValue As New ReserveCustomerInformationTag

                ''' <summary>
                ''' Reserve_VehicleInformationタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private ReserveVehicleInformationValue As New ReserveVehicleInformationTag

                ''' <summary>
                ''' Reserve_ServiceInformationタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private ReserveServiceInformationValue As New ReserveServiceInformationTag

                ''' <summary>
                ''' SeqNoタグ
                ''' </summary>
                ''' <remarks></remarks>
                Private SeqNoValue As String

                ''' <summary>
                ''' REZIDタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private ReserveIdValue As String

                ''' <summary>
                ''' BASREZIDタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private BasReserveIdValue As String

                ''' <summary>
                ''' PREZIDタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private PReserveIdValue As String

                ''' <summary>
                ''' STATUSタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private StatusValue As String

                ''' <summary>
                ''' WALKINタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private WalkInValue As String

                ''' <summary>
                ''' SMSFLGタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private SmsFlgValue As String

                ''' <summary>
                ''' CANCELFLGタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private CancelFlagValue As String

                ''' <summary>
                ''' NOSHOWFLGタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private NoShowFlagValue As String

                ''' <summary>
                ''' WORKORDERFLGタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private WorkOrderFlagValue As String

                ''' <summary>
                ''' ACCOUNT_PLANタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private AcountPlanValue As String

                ''' <summary>
                ''' MEMOタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private MemoValue As String

                ''' <summary>
                ''' UPDATEACCOUNTタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private UpdateAccountValue As String

                ''' <summary>
                ''' R_Oタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private OerderNoValue As String

                '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                ' ''' <summary>
                ' ''' RO_JOB_SEQタグ
                ' ''' </summary>
                ' ''' <remarks></remarks>                
                'Private OerderJobSeqValue As String
                '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                ''' <summary>
                ''' ROW_LOCK_VERSIONタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private RowLockVersionValue As String

                '2013/12/18 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
                ''' <summary>
                ''' CST_VCL_TYPEタグ
                ''' </summary>
                ''' <remarks></remarks>                
                Private CstVclTypeValue As String
                '2013/12/18 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

                ''' <summary>
                ''' Reserve_CustomerInformationタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="Reserve_CustomerInformation", IsNullable:=True)> _
                Public Property ReserveCustomerInformation As ReserveCustomerInformationTag

                    Set(ByVal value As ReserveCustomerInformationTag)

                        ReserveCustomerInformationValue = value

                    End Set

                    Get

                        Return ReserveCustomerInformationValue

                    End Get

                End Property

                ''' <summary>
                ''' Reserve_VehicleInformationTagタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="Reserve_VehicleInformation", IsNullable:=True)> _
                Public Property ReserveVehicleInformation As ReserveVehicleInformationTag

                    Set(ByVal value As ReserveVehicleInformationTag)

                        ReserveVehicleInformationValue = value

                    End Set

                    Get

                        Return ReserveVehicleInformationValue

                    End Get

                End Property

                ''' <summary>
                ''' Reserve_ServiceInformationタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="Reserve_ServiceInformation", IsNullable:=True)> _
                Public Property ReserveServiceInformation As ReserveServiceInformationTag

                    Set(ByVal value As ReserveServiceInformationTag)

                        ReserveServiceInformationValue = value

                    End Set

                    Get

                        Return ReserveServiceInformationValue

                    End Get

                End Property

                ''' <summary>
                ''' SeqNoタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="SeqNo", IsNullable:=False)> _
                Public Property SeqNo As String

                    Set(ByVal value As String)

                        SeqNoValue = value

                    End Set

                    Get

                        Return SeqNoValue

                    End Get

                End Property

                ''' <summary>
                ''' REZIDタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="REZID", IsNullable:=False)> _
                Public Property ReserveId As String

                    Set(ByVal value As String)

                        ReserveIdValue = value

                    End Set

                    Get

                        Return ReserveIdValue

                    End Get

                End Property

                ''' <summary>
                ''' BASREZIDタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="BASREZID", IsNullable:=False)> _
                Public Property BasReserveId As String

                    Set(ByVal value As String)

                        BasReserveIdValue = value

                    End Set

                    Get

                        If BasReserveIdValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(BasReserveIdValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return String.Concat(CDataSectionFront, BasReserveIdValue, CDataSectionBack)

                        End If

                    End Get

                End Property

                ''' <summary>
                ''' PREZIDタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="PREZID", IsNullable:=False)> _
                Public Property PReserveId As String

                    Set(ByVal value As String)

                        PReserveIdValue = value

                    End Set

                    Get

                        Return PReserveIdValue

                    End Get

                End Property

                ''' <summary>
                ''' STATUSタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="STATUS", IsNullable:=False)> _
                Public Property Status As String

                    Set(ByVal value As String)

                        StatusValue = value

                    End Set

                    Get

                        Return StatusValue

                    End Get

                End Property

                ''' <summary>
                ''' WALKINタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="WALKIN", IsNullable:=False)> _
                Public Property WalkIn As String

                    Set(ByVal value As String)

                        WalkInValue = value

                    End Set

                    Get

                        Return WalkInValue

                    End Get

                End Property

                ''' <summary>
                ''' SMSFLGタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="SMSFLG", IsNullable:=False)> _
                Public Property SmsFlg As String

                    Set(ByVal value As String)

                        SmsFlgValue = value

                    End Set

                    Get

                        Return SmsFlgValue

                    End Get

                End Property

                ''' <summary>
                ''' CANCELFLGタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="CANCELFLG", IsNullable:=False)> _
                Public Property CancelFlg As String

                    Set(ByVal value As String)

                        CancelFlagValue = value

                    End Set

                    Get

                        Return CancelFlagValue

                    End Get

                End Property

                ''' <summary>
                ''' NOSHOWFLGタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="NOSHOWFLG", IsNullable:=False)> _
                Public Property NoShowFlg As String

                    Set(ByVal value As String)

                        NoShowFlagValue = value

                    End Set

                    Get

                        Return NoShowFlagValue

                    End Get

                End Property

                ''' <summary>
                ''' WORKORDERFLGタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="WORKORDERFLG", IsNullable:=False)> _
                Public Property WorkOrderFlg As String

                    Set(ByVal value As String)

                        WorkOrderFlagValue = value

                    End Set

                    Get

                        Return WorkOrderFlagValue

                    End Get

                End Property

                ''' <summary>
                ''' ACCOUNT_PLANタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="ACCOUNT_PLAN", IsNullable:=False)> _
                Public Property AcountPlan As String

                    Set(ByVal value As String)

                        AcountPlanValue = value

                    End Set

                    Get

                        If AcountPlanValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(AcountPlanValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return String.Concat(CDataSectionFront, AcountPlanValue, CDataSectionBack)

                        End If

                    End Get

                End Property

                ''' <summary>
                ''' MEMOタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="MEMO", IsNullable:=False)> _
                Public Property Memo As String

                    Set(ByVal value As String)

                        MemoValue = value

                    End Set

                    Get

                        If MemoValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(MemoValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return String.Concat(CDataSectionFront, MemoValue, CDataSectionBack)

                        End If

                    End Get

                End Property

                ''' <summary>
                ''' UPDATEACCOUNTタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="UPDATEACCOUNT", IsNullable:=False)> _
                Public Property UpdateAccount As String

                    Set(ByVal value As String)

                        UpdateAccountValue = value

                    End Set

                    Get

                        If UpdateAccountValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(UpdateAccountValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return String.Concat(CDataSectionFront, UpdateAccountValue, CDataSectionBack)

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
                Public Property OerderNo As String

                    Set(ByVal value As String)

                        OerderNoValue = value

                    End Set

                    Get

                        If OerderNoValue Is Nothing Then

                            Return Nothing

                        ElseIf String.IsNullOrEmpty(OerderNoValue) Then

                            Return String.Empty

                        Else

                            '値があるときのみCDATAセクションをつける
                            Return String.Concat(CDataSectionFront, OerderNoValue, CDataSectionBack)

                        End If

                    End Get

                End Property

                '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                ' ''' <summary>
                ' ''' RO_JOB_SEQタグ用プロパティ
                ' ''' </summary>
                ' ''' <value></value>
                ' ''' <returns></returns>
                ' ''' <remarks></remarks>
                '<XmlElementAttribute(ElementName:="RO_JOB_SEQ", IsNullable:=False)> _
                'Public Property OerderJobSeq As String

                '    Set(ByVal value As String)

                '        OerderJobSeqValue = value

                '    End Set

                '    Get

                '        Return OerderJobSeqValue

                '    End Get

                'End Property
                '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                ''' <summary>
                ''' ROW_LOCK_VERSIONタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="ROW_LOCK_VERSION", IsNullable:=False)> _
                Public Property RowLockVersion As String

                    Set(ByVal value As String)

                        RowLockVersionValue = value

                    End Set

                    Get

                        Return RowLockVersionValue

                    End Get

                End Property

                '2013/12/18 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
                ''' <summary>
                ''' CST_VCL_TYPEタグ用プロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                <XmlElementAttribute(ElementName:="CST_VCL_TYPE", IsNullable:=False)> _
                Public Property CstVclType As String

                    Set(ByVal value As String)

                        CstVclTypeValue = value

                    End Set

                    Get

                        Return CstVclTypeValue

                    End Get

                End Property
                '2013/12/18 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END



                ''' <summary>
                ''' Reserve_CustomerInformationTagXMLクラス
                ''' </summary>
                ''' <remarks></remarks>
                Public Class ReserveCustomerInformationTag

                    '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    ''' <summary>
                    ''' CST_IDタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private CstIdValue As String
                    '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                    ''' <summary>
                    ''' CUSTCDタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private CustCodeValue As String

                    ''' <summary>
                    ''' CUSTOMERNAMEタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private CustomerNameValue As String

                    ''' <summary>
                    ''' CUSTOMERCLASSタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private CustomerClassValue As String

                    ''' <summary>
                    ''' TELNOタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private TelNoValue As String

                    ''' <summary>
                    ''' MOBILEタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private MobileValue As String

                    ''' <summary>
                    ''' EMAILタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private EmailValue As String

                    ''' <summary>
                    ''' ZIPCODEタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private ZipCodeValue As String

                    ''' <summary>
                    ''' ADDRESSタグ
                    ''' </summary>
                    ''' <remarks></remarks>
                    Private AddressValue As String

                    '2013/12/18 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START

                    ''' <summary>
                    ''' NAMETITLECDタグ
                    ''' </summary>
                    ''' <remarks></remarks>
                    Private NameTitleCDValue As String

                    ''' <summary>
                    ''' NAMETITLENAMEタグ
                    ''' </summary>
                    ''' <remarks></remarks>
                    Private NameTitleNameValue As String

                    '2013/12/18 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

                    '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    ''' <summary>
                    ''' CST_IDタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="CST_ID", IsNullable:=False)> _
                    Public Property CstId As String

                        Set(ByVal value As String)

                            CstIdValue = value

                        End Set

                        Get

                            Return CstIdValue

                        End Get

                    End Property
                    '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                    ''' <summary>
                    ''' CUSTCDタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="CUSTCD", IsNullable:=False)> _
                    Public Property CustCode As String

                        Set(ByVal value As String)

                            CustCodeValue = value

                        End Set

                        Get

                            If CustCodeValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(CustCodeValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, CustCodeValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    ''' <summary>
                    ''' CUSTOMERNAMEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="CUSTOMERNAME", IsNullable:=False)> _
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
                                Return String.Concat(CDataSectionFront, CustomerNameValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    ''' <summary>
                    ''' CUSTOMERCLASSタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="CUSTOMERCLASS", IsNullable:=False)> _
                    Public Property CustomerClass As String

                        Set(ByVal value As String)

                            CustomerClassValue = value

                        End Set

                        Get

                            Return CustomerClassValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' TELNOタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="TELNO", IsNullable:=False)> _
                    Public Property TelNo As String

                        Set(ByVal value As String)

                            TelNoValue = value

                        End Set

                        Get

                            If TelNoValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(TelNoValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, TelNoValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    ''' <summary>
                    ''' MOBILEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="MOBILE", IsNullable:=False)> _
                    Public Property Mobile As String

                        Set(ByVal value As String)

                            MobileValue = value

                        End Set

                        Get

                            If MobileValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(MobileValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, MobileValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    ''' <summary>
                    ''' EMAILタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="EMAIL", IsNullable:=False)> _
                    Public Property Email As String

                        Set(ByVal value As String)

                            EmailValue = value

                        End Set

                        Get

                            If EmailValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(EmailValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, EmailValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    ''' <summary>
                    ''' ZIPCODEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="ZIPCODE", IsNullable:=False)> _
                    Public Property ZipCode As String

                        Set(ByVal value As String)

                            ZipCodeValue = value

                        End Set

                        Get

                            If ZipCodeValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(ZipCodeValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, ZipCodeValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    ''' <summary>
                    ''' ADDRESSタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="ADDRESS", IsNullable:=False)> _
                    Public Property Address As String

                        Set(ByVal value As String)

                            AddressValue = value

                        End Set

                        Get

                            If AddressValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(AddressValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, AddressValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    '2013/12/18 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START

                    ''' <summary>
                    ''' NAMETITLECDタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="NAMETITLECD", IsNullable:=False)> _
                    Public Property NameTitleCD As String

                        Set(ByVal value As String)

                            NameTitleCDValue = value

                        End Set

                        Get

                            If NameTitleCDValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(NameTitleCDValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, NameTitleCDValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    ''' <summary>
                    ''' NAMETITLENAMEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="NAMETITLENAME", IsNullable:=False)> _
                    Public Property NameTitleName As String

                        Set(ByVal value As String)

                            NameTitleNameValue = value

                        End Set

                        Get

                            If NameTitleNameValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(NameTitleNameValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, NameTitleNameValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    '2013/12/18 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

                End Class

                ''' <summary>
                ''' Reserve_VehicleInformationTagXMLクラス
                ''' </summary>
                ''' <remarks></remarks>
                Public Class ReserveVehicleInformationTag

                    '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    ''' <summary>
                    ''' VCL_IDタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private VclIdValue As String
                    '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                    ''' <summary>
                    ''' VCLREGNOタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private VehicleNoValue As String

                    ''' <summary>
                    ''' VINタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private VinValue As String

                    ''' <summary>
                    ''' MAKERCDタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private MakerCodeValue As String

                    ''' <summary>
                    ''' SERIESCDタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private SeriesCodeValue As String

                    ''' <summary>
                    ''' SERIESNMタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private SeriesNameValue As String

                    ''' <summary>
                    ''' BASETYPEタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private BaseTypeValue As String

                    ''' <summary>
                    ''' MILEAGEタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private MileageValue As String


                    '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
                    ''' <summary>
                    ''' VCL_IDタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="VCL_ID", IsNullable:=False)> _
                    Public Property VclId As String

                        Set(ByVal value As String)

                            VclIdValue = value

                        End Set

                        Get

                            Return VclIdValue

                        End Get

                    End Property
                    '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

                    ''' <summary>
                    ''' VCLREGNOタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="VCLREGNO", IsNullable:=False)> _
                    Public Property VehicleNo As String

                        Set(ByVal value As String)

                            VehicleNoValue = value

                        End Set

                        Get

                            If VehicleNoValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(VehicleNoValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, VehicleNoValue, CDataSectionBack)

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
                                Return String.Concat(CDataSectionFront, VinValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    ''' <summary>
                    ''' MAKERCDタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="MAKERCD", IsNullable:=False)> _
                    Public Property MakerCode As String

                        Set(ByVal value As String)

                            MakerCodeValue = value

                        End Set

                        Get

                            If MakerCodeValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(MakerCodeValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, MakerCodeValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    ''' <summary>
                    ''' SERIESCDタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="SERIESCD", IsNullable:=False)> _
                    Public Property SeriesCode As String

                        Set(ByVal value As String)

                            SeriesCodeValue = value

                        End Set

                        Get

                            If SeriesCodeValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(SeriesCodeValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, SeriesCodeValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    ''' <summary>
                    ''' SERIESNMタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="SERIESNM", IsNullable:=False)> _
                    Public Property SeriesName As String

                        Set(ByVal value As String)

                            SeriesNameValue = value

                        End Set

                        Get

                            If SeriesNameValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(SeriesNameValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, SeriesNameValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    ''' <summary>
                    ''' BASETYPEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="BASETYPE", IsNullable:=False)> _
                    Public Property BaseType As String

                        Set(ByVal value As String)

                            BaseTypeValue = value

                        End Set

                        Get

                            If BaseTypeValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(BaseTypeValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, BaseTypeValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    ''' <summary>
                    ''' MILEAGEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="MILEAGE", IsNullable:=False)> _
                    Public Property Mileage As String

                        Set(ByVal value As String)

                            MileageValue = value

                        End Set

                        Get

                            Return MileageValue

                        End Get

                    End Property


                End Class

                ''' <summary>
                ''' Reserve_ServiceInformationTagXMLクラス
                ''' </summary>
                ''' <remarks></remarks>
                Public Class ReserveServiceInformationTag

                    ''' <summary>
                    ''' STALLIDタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private StallIdValue As String

                    ''' <summary>
                    ''' STARTTIMEタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private StartTimeValue As String

                    ''' <summary>
                    ''' ENDTIMEタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private EndTimeValue As String

                    ''' <summary>
                    ''' WORKTIMEタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private WorkTimeValue As String

                    ''' <summary>
                    ''' BREAKFLGタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private BreakFlagValue As String

                    ''' <summary>
                    ''' WASHFLGタグ
                    ''' </summary>
                    ''' <remarks></remarks>                   
                    Private WashFlagValue As String

                    ''' <summary>
                    ''' INSPECTIONFLGタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private InspectionFlagValue As String

                    ''' <summary>
                    ''' MERCHANDISECDタグ
                    ''' </summary>
                    ''' <remarks></remarks>                
                    Private MerchandiseCodeValue As String

                    ''' <summary>
                    ''' MNTNCDタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private MntnCodeValue As String

                    ''' <summary>
                    ''' SERVICECODEタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private ServiceCodeValue As String

                    ''' <summary>
                    ''' REZ_RECEPTIONタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private ReserveReceptionValue As String

                    ''' <summary>
                    ''' REZ_PICK_DATEタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private ReservePickDateValue As String

                    ''' <summary>
                    ''' REZ_PICK_LOCタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private ReservePickLocValue As String

                    ''' <summary>
                    ''' REZ_PICK_TIMEタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private ReservePickTimeValue As String

                    ''' <summary>
                    ''' REZ_DELI_DATEタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private ReserveDeliDateValue As String

                    ''' <summary>
                    ''' REZ_DELI_LOCタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private ReserveDeliLocValue As String

                    ''' <summary>
                    ''' REZ_DELI_TIMEタグ
                    ''' </summary>
                    ''' <remarks></remarks>                    
                    Private ReserveDeliTimeValue As String



                    ''' <summary>
                    ''' STALLIDタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="STALLID", IsNullable:=False)> _
                    Public Property StallId As String

                        Set(ByVal value As String)

                            StallIdValue = value

                        End Set

                        Get

                            Return StallIdValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' STARTTIMEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="STARTTIME", IsNullable:=False)> _
                    Public Property StartTime As String

                        Set(ByVal value As String)

                            StartTimeValue = value

                        End Set

                        Get

                            Return StartTimeValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' ENDTIMEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="ENDTIME", IsNullable:=False)> _
                    Public Property EndTime As String

                        Set(ByVal value As String)

                            EndTimeValue = value

                        End Set

                        Get

                            Return EndTimeValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' WORKTIMEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="WORKTIME", IsNullable:=False)> _
                    Public Property WorkTime As String

                        Set(ByVal value As String)

                            WorkTimeValue = value

                        End Set

                        Get

                            Return WorkTimeValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' BREAKFLGタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="BREAKFLG", IsNullable:=False)> _
                    Public Property BreakFlg As String

                        Set(ByVal value As String)

                            BreakFlagValue = value

                        End Set

                        Get

                            Return BreakFlagValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' WASHFLGタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="WASHFLG", IsNullable:=False)> _
                    Public Property WashFlg As String

                        Set(ByVal value As String)

                            WashFlagValue = value

                        End Set

                        Get

                            Return WashFlagValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' INSPECTIONFLGタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="INSPECTIONFLG", IsNullable:=False)> _
                    Public Property InspectionFlg As String

                        Set(ByVal value As String)

                            InspectionFlagValue = value

                        End Set

                        Get

                            Return InspectionFlagValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' MERCHANDISECDタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="MERCHANDISECD", IsNullable:=False)> _
                    Public Property MerchandiseCode As String

                        Set(ByVal value As String)

                            MerchandiseCodeValue = value

                        End Set

                        Get

                            Return MerchandiseCodeValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' MNTNCDタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="MNTNCD", IsNullable:=False)> _
                    Public Property MntnCode As String

                        Set(ByVal value As String)

                            MntnCodeValue = value

                        End Set

                        Get

                            If MntnCodeValue Is Nothing Then

                                Return Nothing

                            ElseIf String.IsNullOrEmpty(MntnCodeValue) Then

                                Return String.Empty

                            Else

                                '値があるときのみCDATAセクションをつける
                                Return String.Concat(CDataSectionFront, MntnCodeValue, CDataSectionBack)

                            End If

                        End Get

                    End Property

                    ''' <summary>
                    ''' SERVICECODEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="SERVICECODE", IsNullable:=False)> _
                    Public Property ServiceCode As String

                        Set(ByVal value As String)

                            ServiceCodeValue = value

                        End Set

                        Get

                            Return ServiceCodeValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' REZ_RECEPTIONタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="REZ_RECEPTION", IsNullable:=False)> _
                    Public Property ReserveReception As String

                        Set(ByVal value As String)

                            ReserveReceptionValue = value

                        End Set

                        Get

                            Return ReserveReceptionValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' REZ_PICK_DATEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="REZ_PICK_DATE", IsNullable:=False)> _
                    Public Property ReservePickDate As String

                        Set(ByVal value As String)

                            ReservePickDateValue = value

                        End Set

                        Get

                            Return ReservePickDateValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' REZ_PICK_LOCタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="REZ_PICK_LOC", IsNullable:=False)> _
                    Public Property ReservePickLoc As String

                        Set(ByVal value As String)

                            ReservePickLocValue = value

                        End Set

                        Get

                            Return ReservePickLocValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' REZ_PICK_TIMEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="REZ_PICK_TIME", IsNullable:=False)> _
                    Public Property ReservePickTime As String

                        Set(ByVal value As String)

                            ReservePickTimeValue = value

                        End Set

                        Get

                            Return ReservePickTimeValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' REZ_DELI_DATEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="REZ_DELI_DATE", IsNullable:=False)> _
                    Public Property ReserveDeliDate As String

                        Set(ByVal value As String)

                            ReserveDeliDateValue = value

                        End Set

                        Get

                            Return ReserveDeliDateValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' REZ_DELI_LOCタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="REZ_DELI_LOC", IsNullable:=False)> _
                    Public Property ReserveDeliLoc As String

                        Set(ByVal value As String)

                            ReserveDeliLocValue = value

                        End Set

                        Get

                            Return ReserveDeliLocValue

                        End Get

                    End Property

                    ''' <summary>
                    ''' REZ_DELI_TIMEタグ用プロパティ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    <XmlElementAttribute(ElementName:="REZ_DELI_TIME", IsNullable:=False)> _
                    Public Property ReserveDeliTime As String

                        Set(ByVal value As String)

                            ReserveDeliTimeValue = value

                        End Set

                        Get

                            Return ReserveDeliTimeValue

                        End Get

                    End Property

                End Class

            End Class

        End Class

    End Class

End Class
