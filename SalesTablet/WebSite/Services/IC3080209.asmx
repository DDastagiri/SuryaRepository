<%@ WebService Language="VB" Class="IC3080209" %>

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CommonUtility.DataAccess
Imports Toyota.eCRB.CommonUtility.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Globalization
Imports System.Xml


''' <summary>
''' IC3080209 活動履歴I/F
''' </summary>
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class IC3080209
    Inherits System.Web.Services.WebService

#Region "定数"
    ''' <summary>
    ''' 日付時刻のフォーマット
    ''' </summary>
    Private FormatDatetime As String = "dd/MM/yyyy HH:mm:ss"
        
    ''' <summary>
    ''' エラーコード：XML Document不正
    ''' </summary>
    Private Const ErrCodeXmlDoc As Short = -1

    ''' <summary>
    ''' エラーコード：成功
    ''' </summary>
    Private Const ErrCodeSuccess As Short = 0

    ''' <summary>
    ''' エラーコード：項目必須エラー
    ''' </summary>
    Private Const ErrCodeItMust As Short = 2000

    ''' <summary>
    ''' エラーコード：項目型エラー
    ''' </summary>
    Private Const ErrCodeItType As Short = 3000

    ''' <summary>
    ''' エラーコード：項目サイズエラー
    ''' </summary>
    Private Const ErrCodeItSize As Short = 4000

    ''' <summary>
    ''' エラーコード：値チェックエラー
    ''' </summary>
    Private Const ErrCodeItValue As Short = 5000

    ''' <summary>
    ''' エラーコード：データ存在エラー
    ''' </summary>
    Private Const ErrCodeDataNothing As Short = 6001

    ''' <summary>
    ''' エラーコード：システムエラー
    ''' </summary>
    Private Const ErrCodeSys As Short = 9999

    ''' <summary>
    ''' 空文字の表示形式
    ''' </summary>
    Private Const NOTEXT As String = "-"

    ''' <summary>活動種類 全て</summary>
    Private Const ACTUALKIND_ALL As String = "0"

    ''' <summary>活動種類 セールス</summary>
    Private Const ACTUALKIND_SALES As String = "1"
    
    ''' <summary>活動種類 サービス</summary>
    Private Const ACTUALKIND_SERVICE As String = "2"

    ''' <summary>活動種類 CR</summary>
    Private Const ACTUALKIND_CR As String = "3"

    ''' <summary>活動種類 受注後活動</summary>
    Private Const ACTUALKIND_AFTER_ODR_ACT As String = "4"

#End Region

#Region "メンバ変数"
    
    ''' <summary>
    ''' 終了コード
    ''' </summary>
    Private ResultId As Integer
    
    ''' <summary>
    ''' メッセージ
    ''' </summary>
    Private Message As String
    
    ''' <summary>
    ''' 受信日時
    ''' </summary>
    Private ReceptionDate As DateTime
#End Region

#Region "内部クラス"
    ''' <summary>
    ''' 受信XMLのリクエストパラメータを保持する
    ''' </summary>
    Public Class Request
        Public DealerCode As String
        Public CustomerId As Long
        Public CustomerClass As String
        Public Vin As String
        Public BusinessCategory As String
        Public BeginRowNumber As Integer
        Public MaxRowCount As Integer
    End Class
#End Region

    
    ''' <summary>
    ''' 顧客のコンタクト履歴を取得する
    ''' </summary>
    ''' <param name="xsData">受信XML</param>
    ''' <returns>応答XML</returns>
    ''' <remarks>顧客詳細画面のコンタクト履歴と同等の内容を返却する</remarks>
    <WebMethod()> _
    Public Function GetContactHistory(ByVal xsData As String) As XmlDocument
        '初期化
        Me.ResultId = ErrCodeSuccess
        Me.Message = ""
        Me.ReceptionDate = DateTimeFunc.Now()

        Dim responseXml As XmlDocument = Nothing

        ' 受信XMLをログ出力
        Logger.Info("Request XML : " & xsData, True)
        
        Try
            '入力フォーマットチェック
            Dim request As Request = ParseRequestXml(xsData)
            If (request Is Nothing) Then
                responseXml = GetResponseXml(Nothing)
            Else
                Dim tabIndex As String = ""
                Select Case request.BusinessCategory
                    Case "0"
                        tabIndex = ActivityInfoTableAdapter.CONTACTHISTORY_TAB_ALL
                    Case "1"
                        tabIndex = ActivityInfoTableAdapter.CONTACTHISTORY_TAB_SERVICE
                    Case "2"
                        tabIndex = ActivityInfoTableAdapter.CONTACTHISTORY_TAB_SALES
                    Case "3"
                        tabIndex = ActivityInfoTableAdapter.CONTACTHISTORY_TAB_CR
                End Select
                
                Dim contactHistoryTbl As ActivityInfoDataSet.ActivityInfoContactHistoryDataTable = ActivityInfoBusinessLogic.GetContactHistoryData( _
                    request.CustomerClass, request.CustomerId, request.DealerCode, request.CustomerClass, "", tabIndex, request.Vin)

                '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 START
                Dim actidList As New List(Of Decimal)
                Dim afterOdrFllwSeqList As New List(Of Decimal)

                '2014/11/21 TCS 河原 TMT B案 START
                For Each dr As ActivityInfoDataSet.ActivityInfoContactHistoryRow In contactHistoryTbl
                    If Not dr.IsACT_IDNull Then
                        If dr.ACT_ID > 0 AndAlso actidList.Contains(dr.ACT_ID) = False Then
                            '活動ID
                            actidList.Add(dr.ACT_ID)
                        ElseIf dr.AFTER_ODR_FLLW_SEQ > 0 AndAlso afterOdrFllwSeqList.Contains(dr.AFTER_ODR_FLLW_SEQ) = False Then
                            '受注後工程フォロー結果連番
                            afterOdrFllwSeqList.Add(dr.AFTER_ODR_FLLW_SEQ)
                        End If
                    End If
                Next

                '受注後活動名称取得
                Dim afterOdrActNameTbl As ActivityInfoDataSet.ActivityInfoContactAfterOdrActDataTable _
                    = ActivityInfoBusinessLogic.GetContactAfterOdrAct(actidList, afterOdrFllwSeqList)

                Dim contactHistory As New ActivityInfoDataSet.ActivityInfoContactHistoryDataTable

                For i As Integer = (request.BeginRowNumber - 1) To ((request.BeginRowNumber - 1) + (request.MaxRowCount - 1))
                    If i >= contactHistoryTbl.Rows.Count Then
                        Exit For
                    End If

                    '取得したデータを編集
                    '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 START
                    Dim editContactHistoryRow As ActivityInfoDataSet.ActivityInfoContactHistoryRow = EditContactHistory(contactHistoryTbl.Item(i), request.DealerCode, afterOdrActNameTbl)
                    '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 END

                    contactHistory.ImportRow(contactHistoryTbl.Item(i))
                Next
        
                Dim serviceInInfoTbl As ActivityInfoDataSet.ActivityInfoServiceInInfoDataTable = Nothing
                If (request.Vin <> Nothing) Then
                    serviceInInfoTbl = ActivityInfoBusinessLogic.GetServiceInInfo(request.CustomerId, request.Vin, request.DealerCode)
                End If
        
                responseXml = GetResponseXml(contactHistory, serviceInInfoTbl, request.BeginRowNumber)
            End If

        Catch ex As Exception
            'システムエラー
            Logger.Error("IC3080209 System Error", ex)

            Me.ResultId = ErrCodeSys
            Me.Message = ex.Message
            responseXml = GetResponseXml(Nothing)
        End Try

        ' 送信XMLをログ出力
        Logger.Info("Response XML : " & responseXml.OuterXml, True)
        
        Return responseXml
    End Function


    '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 START
    ''' <summary>
    ''' 取得したコンタクト履歴を編集
    ''' </summary>
    ''' <param name="contactHistoryRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function EditContactHistory(ByVal contactHistoryRow As ActivityInfoDataSet.ActivityInfoContactHistoryRow, _
                                        ByVal dlrCd As String, _
                                        ByVal afterOdrActName As ActivityInfoDataSet.ActivityInfoContactAfterOdrActDataTable _
                                        ) As ActivityInfoDataSet.ActivityInfoContactHistoryRow
        '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 END
        Logger.Debug("EditContactHistory Start")
        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START

        '活動日設定
        If contactHistoryRow.IsACTUALDATENull Then
            contactHistoryRow.ACTUALDATESTRING = NOTEXT
        Else
            If contactHistoryRow.ACTUALDATE.Hour = 0 And
                contactHistoryRow.ACTUALDATE.Minute = 0 Then

                '時間指定なし
                contactHistoryRow.ACTUALDATESTRING = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, contactHistoryRow.ACTUALDATE, DateTimeFunc.Now(dlrCd), dlrCd, False)

            Else
                '時間指定あり
                contactHistoryRow.ACTUALDATESTRING = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, contactHistoryRow.ACTUALDATE, DateTimeFunc.Now(dlrCd), dlrCd)

            End If
        End If

        '活動内容設定
        If contactHistoryRow.IsCONTACTNull Then
            contactHistoryRow.CONTACT = NOTEXT
        End If

        If String.Equals(contactHistoryRow.COUNTVIEW, "1") Then
            'カウント表示が"1"
            contactHistoryRow.CONTACT = contactHistoryRow.CONTACTCOUNT & WebWordUtility.GetWord("SC3080201", 10156) & contactHistoryRow.CONTACT

        Else
            'カウント表示が"1"以外
            Select Case contactHistoryRow.ACTUALKIND
                Case ACTUALKIND_SALES
                    '%3をTACTに変換
                    Dim sysEnv As New SystemEnvSetting
                    Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

                    sysEnvRow = sysEnv.GetSystemEnvSetting("CONTACT_TABLET_DMS_NAME")

                    contactHistoryRow.CONTACT = contactHistoryRow.CONTACT.Replace("%3", sysEnvRow.PARAMVALUE)
                Case ACTUALKIND_CR
                    '%1を苦情、%2を／に変換
                    contactHistoryRow.CONTACT = contactHistoryRow.CONTACT.Replace("%1", WebWordUtility.GetWord("SC3080201", 10180)).Replace("%2", WebWordUtility.GetWord("SC3080201", 10185))

                Case Else
            End Select
        End If

        '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 START
        '活動内容(受注後活動内容)
        '2014/11/21 TCS 河原 TMT B案 START
        If Not contactHistoryRow.IsACT_IDNull AndAlso (contactHistoryRow.ACTUALKIND = ACTUALKIND_SALES Or contactHistoryRow.ACTUALKIND = ACTUALKIND_AFTER_ODR_ACT) _
            AndAlso (contactHistoryRow.ACT_ID > 0 Or contactHistoryRow.AFTER_ODR_FLLW_SEQ > 0) Then
            '2014/11/21 TCS 河原 TMT B案 END
            '受注時に受注後活動した、または受注後の場合

            Dim namesStart As String = WebWordUtility.GetWord("SC3080201", 10203)
            Dim namesEnd As String = WebWordUtility.GetWord("SC3080201", 10204)
            Dim actNameSeparator As String = WebWordUtility.GetWord("SC3080201", 10205)
            Dim sb As New StringBuilder
            Dim isFirst As Boolean = True

            If contactHistoryRow.ACT_ID > 0 OrElse contactHistoryRow.AFTER_ODR_FLLW_SEQ > 0 Then
                For Each dr In afterOdrActName
                    '活動ID、受注後工程フォロー結果連番が一致する受注後活動名称を、取得した順に区切り文字で連結
                    If (contactHistoryRow.ACT_ID > 0 AndAlso dr.ACT_ID = contactHistoryRow.ACT_ID) _
                        OrElse (contactHistoryRow.AFTER_ODR_FLLW_SEQ > 0 AndAlso dr.AFTER_ODR_FLLW_SEQ = contactHistoryRow.AFTER_ODR_FLLW_SEQ) Then

                        If isFirst = False Then
                            sb.Append(actNameSeparator)
                        End If
                        isFirst = False
                        Dim odrActName As String = String.Empty
                        If Not dr.IsAFTER_ODR_ACT_NAMENull AndAlso Not " ".Equals(dr.AFTER_ODR_ACT_NAME) Then
                            '名称が取得できた場合
                            odrActName = dr.AFTER_ODR_ACT_NAME
                        Else
                            odrActName = NOTEXT
                        End If
                        sb.Append(odrActName)
                    End If
                Next

                If sb.Length > 0 Then
                    contactHistoryRow.CONTACT &= (namesStart & sb.ToString & namesEnd)
                End If
            End If

        End If
        '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 END

        '実施者
        If contactHistoryRow.IsUSERNAMENull() Then
            contactHistoryRow.USERNAME = NOTEXT
        End If
        If contactHistoryRow.IsOPERATIONCODENull() Then
            contactHistoryRow.OPERATIONCODE = ""
        End If
        If contactHistoryRow.IsICON_IMGFILENull() Then
            contactHistoryRow.ICON_IMGFILE = ""
        End If
                

        'CR
        If String.Equals(contactHistoryRow.ACTUALKIND, ACTUALKIND_CR) Then
            '苦情概要
            If contactHistoryRow.IsCOMPLAINT_OVERVIEWNull Then
                contactHistoryRow.COMPLAINT_OVERVIEW = NOTEXT
            End If
            '苦情対応内容
            If contactHistoryRow.IsACTUAL_DETAILNull Then
                contactHistoryRow.ACTUAL_DETAIL = NOTEXT
            End If

            '苦情メモ
            If contactHistoryRow.IsMEMONull Then
                contactHistoryRow.MEMO = NOTEXT
            End If
        End If

        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        If String.Equals(contactHistoryRow.ACTUALKIND, ACTUALKIND_SERVICE) Then

            '走行距離設定
            If Not contactHistoryRow.IsMILEAGENull Then
                contactHistoryRow.MILEAGE = Trim(contactHistoryRow.MILEAGE) & WebWordUtility.GetWord("SC3080201", 10130)
            Else
                contactHistoryRow.MILEAGE = NOTEXT
            End If

            '整備価格設定
            If Not contactHistoryRow.IsMAINTEAMOUNTNull Then
                contactHistoryRow.MAINTEAMOUNT = contactHistoryRow.MAINTEAMOUNT
            Else
                contactHistoryRow.MAINTEAMOUNT = NOTEXT
            End If

            '車両登録No.設定
            If Not contactHistoryRow.IsVCLREGNONull Then
                If String.IsNullOrEmpty(Trim(contactHistoryRow.VCLREGNO)) Then
                    contactHistoryRow.VCLREGNO = NOTEXT
                End If
            Else
                contactHistoryRow.VCLREGNO = NOTEXT
            End If

            '他販売店名表示フラグ取得
            Dim sysEnv As New SystemEnvSetting
            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            sysEnvRow = sysEnv.GetSystemEnvSetting("DISP_OTHER_DLRNM")
            Dim disp_other_dlrnm = sysEnvRow.PARAMVALUE

            '基幹システム名取得
            Dim BasesystemNMDt As ActivityInfoDataSet.ActivityInfoBasesystemNMDataTable = ActivityInfoBusinessLogic.GetBasesystemNM()
            Dim BasesystemNMRw As ActivityInfoDataSet.ActivityInfoBasesystemNMRow = BasesystemNMDt.Item(0)
            Dim BasesystemNM = BasesystemNMRw.BASESYSTEMNM

            '他販売店名を表示するかどうか
            If String.Equals(contactHistoryRow.DLRCD, dlrCd) Then
                '自身の販売店の場合
                If Not contactHistoryRow.IsDLRNICNM_LOCALNull Then
                    contactHistoryRow.DLRNICNM_LOCAL = BasesystemNM & " (" & Trim(contactHistoryRow.DLRNICNM_LOCAL) & ")"
                Else
                    contactHistoryRow.DLRNICNM_LOCAL = Nothing
                End If
            Else
                '他販売店の場合
                '整備費、実施スタッフは非表示
                contactHistoryRow.MAINTEAMOUNT = "***"
                contactHistoryRow.USERNAME = NOTEXT
                contactHistoryRow.OPERATIONCODEIMG = String.Empty

                If String.Equals(disp_other_dlrnm, "1") Then
                    If Not contactHistoryRow.IsDLRNICNM_LOCALNull Then
                        contactHistoryRow.DLRNICNM_LOCAL = BasesystemNM & " (" & Trim(contactHistoryRow.DLRNICNM_LOCAL) & ")"
                    Else
                        contactHistoryRow.DLRNICNM_LOCAL = Nothing
                    End If
                Else
                    contactHistoryRow.DLRNICNM_LOCAL = BasesystemNM & " (" & WebWordUtility.GetWord("SC3080201", 10198) & ")"
                End If
            End If
        End If

        Return contactHistoryRow
    End Function
    
    Private Function ParseRequestXml(xsData As String) As Request
        
        ' XmlDocument生成
        Dim xdoc As New XmlDocument
            
        Try
            ' XML読み込み
            xdoc.LoadXml(xsData)
            
        Catch ex As Exception
            'XML読み込み失敗時は終了コードをセットして処理終了
            Me.ResultId = ErrCodeXmlDoc
            Me.Message = "Invalid xml format."
            Return Nothing
        End Try
        
        
        Dim request As New Request
        request.DealerCode = GetRequestValue(xdoc, "/Request/Detail/Common/DealerCode", 1, "DealerCode", True, 5, 5)
        If (Me.ResultId <> ErrCodeSuccess) Then
            Return Nothing
        End If

        request.CustomerId = GetRequestValueAsLong(xdoc, "/Request/Detail/Common/CustomerId", 2, "CustomerId", True, 1, Long.MaxValue)
        If (Me.ResultId <> ErrCodeSuccess) Then
            Return Nothing
        End If

        request.CustomerClass = GetRequestValue(xdoc, "/Request/Detail/Common/CustomerClass", 3, "CustomerClass", True, 1, 1, New String() {"1", "2", "3"})
        If (Me.ResultId <> ErrCodeSuccess) Then
            Return Nothing
        End If

        request.BusinessCategory = GetRequestValue(xdoc, "/Request/Detail/FilterCondition/BusinessCategory", 10, "BusinessCategory", True, 1, 1, New String() {"0", "1", "2", "3"})
        If (Me.ResultId <> ErrCodeSuccess) Then
            Return Nothing
        End If

        request.BeginRowNumber = GetRequestValueAsLong(xdoc, "/Request/Detail/FilterCondition/BeginRowNumber", 11, "BeginRowNumber", True, 1, 9999)
        If (Me.ResultId <> ErrCodeSuccess) Then
            Return Nothing
        End If

        request.MaxRowCount = GetRequestValueAsLong(xdoc, "/Request/Detail/FilterCondition/MaxRowCount", 12, "MaxRowCount", True, 1, 9999)
        If (Me.ResultId <> ErrCodeSuccess) Then
            Return Nothing
        End If

        Dim vinIsMandatory As Boolean = (request.BusinessCategory = "1")
        request.Vin = GetRequestValue(xdoc, "/Request/Detail/Common/Vin", 4, "Vin", vinIsMandatory, 1, 128)
        If (Me.ResultId <> ErrCodeSuccess) Then
            Return Nothing
        End If

        Return request
    End Function

    Private Function GetRequestValue(ByVal xdoc As XmlDocument, ByVal xpath As String, ByVal nodeId As Short, ByVal nodeName As String, ByVal isMandatory As Boolean, ByVal minLength As Short, ByVal maxLength As Short, Optional ByVal domain As String() = Nothing) As String
        Dim node As XmlNode = xdoc.SelectSingleNode(xpath)
        If (node Is Nothing) Then
            If (isMandatory) Then
                Me.ResultId = ErrCodeItMust + nodeId
                Me.Message = String.Format(CultureInfo.InvariantCulture, "{0} is mandatory.", nodeName)
            End If
            Return Nothing
        Else
            Dim nodeValue As String = node.InnerText
            If (nodeValue.Length = 0) Then
                If (isMandatory) Then
                    Me.ResultId = ErrCodeItMust + nodeId
                    Me.Message = String.Format(CultureInfo.InvariantCulture, "{0} is mandatory.", nodeName)
                End If
            ElseIf (nodeValue.Length < minLength OrElse maxLength < nodeValue.Length) Then
                Me.ResultId = ErrCodeItSize + nodeId
                Me.Message = String.Format(CultureInfo.InvariantCulture, "{0} length is invalid.", nodeName)
            ElseIf (domain IsNot Nothing) Then
                Dim inDomainRange As Boolean = False
                For Each d As String In domain
                    If (d = nodeValue) Then
                        inDomainRange = True
                        Exit For
                    End If
                Next
                If (inDomainRange = False) Then
                    Me.ResultId = ErrCodeItValue + nodeId
                    Me.Message = String.Format(CultureInfo.InvariantCulture, "{0} has invalid value.", nodeName)
                End If
            End If
            Return nodeValue
        End If
        
    End Function
    
    Private Function GetRequestValueAsLong(ByVal xdoc As XmlDocument, ByVal xpath As String, ByVal nodeId As Short, ByVal nodeName As String, ByVal isMandatory As Boolean, ByVal minLength As Long, ByVal maxLength As Long) As Long
        Dim stringValue As String = GetRequestValue(xdoc, xpath, nodeId, nodeName, isMandatory, 1, 20)
        If (Me.ResultId <> ErrCodeSuccess) Then
            Return Nothing
        End If
        
        Dim longValue As Long
        If (Long.TryParse(stringValue, longValue) = False) Then
            Me.ResultId = ErrCodeItType + nodeId
            Me.Message = String.Format(CultureInfo.InvariantCulture, "{0} format is invalid.", nodeName)
            Return Nothing
        ElseIf (longValue < minLength OrElse maxLength < longValue) Then
            Me.ResultId = ErrCodeItSize + nodeId
            Me.Message = String.Format(CultureInfo.InvariantCulture, "{0} size is invalid.", nodeName)
        End If
        
        Return longValue
    End Function
    
    Private Function GetResponseXml(ByVal contact As ActivityInfoDataSet.ActivityInfoContactHistoryDataTable, Optional ByVal serviceInfo As ActivityInfoDataSet.ActivityInfoServiceInInfoDataTable = Nothing, Optional ByVal beginRowNumber As Integer = 1) As XmlDocument
        Dim responseXml As New XmlDocument
        responseXml.LoadXml( _
            "<Response>" & _
            "<Head>" & _
            "<MessageID>IC3080209</MessageID>" & _
            "<ReceptionDate></ReceptionDate>" & _
            "<TransmissionDate></TransmissionDate>" & _
            "</Head>" & _
            "<Detail>" & _
            "<Common>" & _
            "<ResultId>0</ResultId>" & _
            "<Message>Success</Message>" & _
            "</Common>" & _
            "<ContactHistory>" & _
            "</ContactHistory>" & _
            "</Detail>" & _
            "</Response>")
        
        responseXml.SelectSingleNode("/Response/Head/ReceptionDate").InnerText = Me.ReceptionDate.ToString(FormatDatetime, CultureInfo.InvariantCulture)
        responseXml.SelectSingleNode("/Response/Head/TransmissionDate").InnerText = DateTimeFunc.Now.ToString(FormatDatetime, CultureInfo.InvariantCulture)

        If (Me.ResultId <> ErrCodeSuccess) Then
            responseXml.SelectSingleNode("/Response/Detail/Common/ResultId").InnerText = Me.ResultId
            responseXml.SelectSingleNode("/Response/Detail/Common/Message").InnerText = Me.Message
        Else
            Dim rowNumber = beginRowNumber
            Dim contactHistoryNode As XmlNode = responseXml.SelectSingleNode("/Response/Detail/ContactHistory")
            For Each row As ActivityInfoDataSet.ActivityInfoContactHistoryRow In contact
                
                Dim businessCategory As String = ""
                Select Case row.ACTUALKIND
                    Case "1", "4"
                        businessCategory = "2"
                    Case "2"
                        businessCategory = "1"
                    Case "3"
                        businessCategory = "3"
                End Select
                
                Dim contactNode As XmlNode = responseXml.CreateElement("Contact")
                SetResponseValue(contactNode, "RowNumber", rowNumber)
                SetResponseValue(contactNode, "BusinessCategory", businessCategory)
                SetResponseValue(contactNode, "ContactDate", row.ACTUALDATE.ToString(FormatDatetime, CultureInfo.InvariantCulture))
                SetResponseValue(contactNode, "StaffOperation", row.OPERATIONCODE)
                SetResponseValue(contactNode, "StaffOperationIconPath", row.ICON_IMGFILE)
                SetResponseValue(contactNode, "StaffName", row.USERNAME)
                
                Select Case businessCategory
                    Case "1"
                        Dim serviceCode As String = ""
                        Dim serviceName As String = NOTEXT
                        If (serviceInfo IsNot Nothing) Then
                            For Each svcin As ActivityInfoDataSet.ActivityInfoServiceInInfoRow In serviceInfo
                                If Not svcin.IsSERVICECDNull() AndAlso Not String.IsNullOrEmpty(Trim(svcin.SERVICECD)) Then
                                    serviceCode = svcin.SERVICECD
                                End If
                                If Not svcin.IsSERVICENAMENull() AndAlso Not String.IsNullOrEmpty(Trim(svcin.SERVICENAME)) Then
                                    serviceName = svcin.SERVICENAME
                                End If
                            Next
                        End If

                        Dim serviceNode As XmlNode = responseXml.CreateElement("Service")
                        SetResponseValue(serviceNode, "Mileage", row.MILEAGE)
                        SetResponseValue(serviceNode, "VehicleRegistrationNumber", row.VCLREGNO)
                        SetResponseValue(serviceNode, "ServiceCode", serviceCode)
                        SetResponseValue(serviceNode, "ServiceName", serviceName)
                        SetResponseValue(serviceNode, "MaintenanceCost", row.MAINTEAMOUNT)
                        
                        If (serviceInfo IsNot Nothing) Then
                            Dim inspectionSequenceNo As Integer = 1
                            For Each svcin As ActivityInfoDataSet.ActivityInfoServiceInInfoRow In serviceInfo
                                Dim inspectionNode As XmlNode = responseXml.CreateElement("Inspection")
                                SetResponseValue(inspectionNode, "InspectionSequenceNo", inspectionSequenceNo)
                                SetResponseValue(inspectionNode, "InspectionName", svcin.INSPECTNM)
                                serviceNode.AppendChild(inspectionNode)
                                inspectionSequenceNo += 1
                            Next
                        End If
                        
                        contactNode.AppendChild(serviceNode)
                    
                    Case "2"
                        Dim salesNode As XmlNode = responseXml.CreateElement("Sales")
                        SetResponseValue(salesNode, "Title", row.CONTACT)
                        If (String.IsNullOrEmpty(row.CRACTSTATUS)) Then
                            SetResponseValue(salesNode, "ActivityStatus", "0")
                        Else
                            SetResponseValue(salesNode, "ActivityStatus", row.CRACTSTATUS)
                        End If

                        contactNode.AppendChild(salesNode)

                    Case "3"
                        Dim complaintNode As XmlNode = responseXml.CreateElement("Complaint")
                        SetResponseValue(complaintNode, "Title", row.CONTACT)
                        SetResponseValue(complaintNode, "ComplaintStatus", row.CRACTSTATUS)
                        SetResponseValue(complaintNode, "Summary", row.COMPLAINT_OVERVIEW)
                        SetResponseValue(complaintNode, "Answer", row.ACTUAL_DETAIL)
                        SetResponseValue(complaintNode, "Memo", row.MEMO)

                        contactNode.AppendChild(complaintNode)
                    
                End Select
                
                contactHistoryNode.AppendChild(contactNode)
                rowNumber += 1
            Next
            
        End If
        
        Return responseXml
    End Function

    Private Sub SetResponseValue(ByVal parent As XmlNode, name As String, value As String)
        Dim node As XmlNode = parent.OwnerDocument.CreateElement(name)
        node.InnerText = value
        parent.AppendChild(node)
    End Sub

End Class