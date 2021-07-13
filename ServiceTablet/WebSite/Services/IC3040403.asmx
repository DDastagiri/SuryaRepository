<%@ WebService Language="VB" Class="IC3040403" %>

' asmxファイルを作成する際は、コードビハインドにしないで作成すること。
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Toyota.eCRB.iCROP.BizLogic.IC3040403
Imports System.Xml
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.iCROP.DataAccess.IC3040403.ConstCode
Imports Toyota.eCRB.iCROP.DataAccess.IC3040403
Imports Toyota.eCRB.iCROP.BizLogic.IC3040402

' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri2.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class IC3040403
    Inherits System.Web.Services.WebService
    
#Region "定数"
    ' XML宣言
    Private Const Xml_Version As String = "1.0"
    Private Const Xml_Encoding As String = "UTF-8"
    
#End Region
    
    ''' <summary>
    ''' スケジュール連携(変更・削除)
    ''' </summary>
    ''' <param name="xsData">取得したXML</param>
    ''' <returns>応答インターフェイス　スタッフ作業指示</returns>
    ''' <remarks></remarks>
    <WebMethod()> _
    Public Function RegistSchedule(ByVal xsData As String) As XmlDocument
        
        Dim returnXML As New XmlDocument()
                       
        returnXML = ScheduleCoordination(xsData)
                  
        Return returnXML
    End Function
    
#Region "Public関数"

    ''' <summary>
    ''' スケジュール管理処理メインメソッド
    ''' </summary>
    ''' <param name="xsData">取得したXML</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ScheduleCoordination(ByVal xsData As String) As XmlDocument

        Dim returnXml As New XmlDocument()
        Dim reqestXmlDocument As New XmlDocument()
        Dim startDateTime As DateTime = DateTimeFunc.Now()
        Dim returnCodeList As List(Of Integer) = New List(Of Integer)

        Dim bizClass As New IC3040403BizLogic.IC3040403BusinessLogic
        
        Try

            ' 開始ログ
            Logger.Info("IC3040403 Process Start")
            
            'DebugLog
            Logger.Debug("Start ScheduleCordination")

            '受信ログ
            Logger.Info(xsData, True)

            If xsData.Length = 0 Then
                Throw New ApplicationException((ReturnCode.XmlIncorrect + 0).ToString)
            End If
            reqestXmlDocument.PreserveWhitespace = True

            reqestXmlDocument.LoadXml(xsData)
            
            Dim xmlDataClass As New XmlRegistSchedule()

            Dim evacuationInfo As New IC3040402BusinessLogic(reqestXmlDocument)

            Dim registScheduleClone As XmlNode = bizClass.GetChildNode(reqestXmlDocument, XmlNameRegistSchedule, DataAssignment.ModeMandatory, ElementName.RegistSchedule).CloneNode(True)

            returnCodeList = GetRegistElementValue(registScheduleClone, xmlDataClass, evacuationInfo, bizClass)

            ' 戻り値のXMLを作成します。
            returnXml = CreateReturnXml(xmlDataClass.MessageId, xmlDataClass.CountryCode, returnCodeList, startDateTime)

            'DebugLog
            Logger.Debug("End ScheduleCordination")
                
            ' 正常終了ログ
            Logger.Info("IC3040403 Process NormalEnd")

            Return returnXml

        Catch ex As ApplicationException
            'DebugLog
            Logger.Debug("Error AppEx")

            ' リターンコードを返却
            returnCodeList.Add(Integer.Parse(ex.Message))
            returnXml = CreateReturnXml(EmptyString, EmptyString, returnCodeList, startDateTime)

            Logger.Error(ex.Message, ex)
            
            ' 異常終了ログ
            Logger.Info("IC3040403 Process AbNormalEnd")
            
            Return returnXml

        Catch ex As Exception
            'DebugLog
            Logger.Debug("Error Ex")

            ' リターンコードを返却
            returnCodeList.Add(ReturnCode.ErrCodeSys)
            returnXml = CreateReturnXml(EmptyString, EmptyString, returnCodeList, startDateTime)

            Logger.Error(ex.Message, ex)
            
            ' 異常終了ログ
            Logger.Info("IC3040403 Process AbNormalEnd")
            
            Return returnXml

        End Try
            
    End Function
#End Region

#Region "Private関数"
    
    ''' <summary>
    ''' RegistElement要素内にある要素を専用のクラスに格納します。
    ''' </summary>
    ''' <param name="registScheduleClone"></param>
    ''' <param name="xmlRegistScheduleClass"></param>
    ''' <returns>戻り値のXMLを作成する際に必要なリターンコード</returns>
    ''' <remarks></remarks>
    Private Function GetRegistElementValue(ByVal registScheduleClone As XmlNode, ByVal xmlRegistScheduleClass As XmlRegistSchedule, ByVal evacuationInfo As IC3040402BusinessLogic, ByVal bizClass As IC3040403BizLogic.IC3040403BusinessLogic) As List(Of Integer)

        ' Head要素内を取得します。
        xmlRegistScheduleClass = bizClass.GetHeadElementValue(registScheduleClone, xmlRegistScheduleClass)

        'スケジュール詳細要素初期化
        xmlRegistScheduleClass.InitialDetailList()

        ' リターンコードを格納するリスト
        Dim returnCodeList As List(Of Integer) = New List(Of Integer)

        ' Detail要素は複数ある場合があるので、ForEach文で回してListに格納する。
        For Each detailXml As XmlNode In bizClass.GetChildNode(registScheduleClone, XmlNameDetailName, DataAssignment.ModeMandatory, True, ElementName.RegistSchedule)

            Dim xmlDetailClass As New XmlDetail()

            Try

                Dim detailClone As XmlNode = detailXml.CloneNode(True)

                ' Detail要素の値を取得します。
                xmlDetailClass = bizClass.GetDetailElementValue(detailClone, xmlDetailClass)
                xmlRegistScheduleClass.DetailList.Add(xmlDetailClass)

                ' 取得したDetailCodeの値をDBに格納します
                bizClass.ProcessingDataBase(xmlDetailClass)

                returnCodeList.Add(ReturnCode.Successful)
                
                Logger.Info("RETCD:[" + CType((ReturnCode.Successful + 0), String) + "]DLRCD:[" + xmlDetailClass.DealerCode + "]STRCD:[" + xmlDetailClass.BranchCode _
                                                      + "]SCHDIV:[" + xmlDetailClass.ScheduleDiv + "]SCHID:[" + xmlDetailClass.ScheduleId + "]")
                
            Catch ex As ApplicationException
                ' アプリケーションエラー
                'Logger.Error(ex.Message, ex)

                Dim errorCode As String = ex.Message

                Dim unregistReason As String

                If (ReturnCode.UniqueError + ReturnCode.StaffCodeError) = Integer.Parse(errorCode) Then

                    unregistReason = 1

                ElseIf ReturnCode.DataBaseError <= Integer.Parse(errorCode) Then

                    unregistReason = 2

                Else

                    unregistReason = 3

                End If

                evacuationInfo.EvacuationScheduleInfo(xmlDetailClass.DealerCode, xmlDetailClass.BranchCode, _
                                                      xmlDetailClass.ScheduleDiv, xmlDetailClass.ScheduleId, unregistReason)
                
                returnCodeList.Add(Integer.Parse(ex.Message))

                Logger.Info("RETCD:[" + CType(ex.Message, String) + "]DLRCD:[" + xmlDetailClass.DealerCode + "]STRCD:[" + xmlDetailClass.BranchCode _
                                                      + "]SCHDIV:[" + xmlDetailClass.ScheduleDiv + "]SCHID:[" + xmlDetailClass.ScheduleId + "]")
                
            Catch ex As Exception
                ' システムエラー
                returnCodeList.Add(ReturnCode.ErrCodeSys)
                
                Logger.Info("RETCD:[" + CType(ex.Message, String) + "]DLRCD:[" + xmlDetailClass.DealerCode + "]STRCD:[" + xmlDetailClass.BranchCode _
                                                                      + "]SCHDIV:[" + xmlDetailClass.ScheduleDiv + "]SCHID:[" + xmlDetailClass.ScheduleId + "]")
                
                Logger.Error(ex.Message, ex)

            End Try

        Next

        Return returnCodeList

    End Function
     
    ''' <summary>
    ''' 戻り値のXMLを作成します。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateReturnXml(ByVal messageId As String, ByVal countryCode As String, ByVal returnCodeList As List(Of Integer), ByVal startDateTime As DateTime) As XmlDocument

        Dim messageList As List(Of Integer) = New List(Of Integer)

        messageList = returnCodeList

        Dim returnXml As New XmlDocument()

        Dim xmlDeclaration As XmlDeclaration = returnXml.CreateXmlDeclaration(Xml_Version, Xml_Encoding, Nothing)
        returnXml.AppendChild(xmlDeclaration)

        Dim responseElement As XmlElement = returnXml.CreateElement(XmlNameResponse)
        returnXml.AppendChild(responseElement)

        Dim headElement As XmlElement = returnXml.CreateElement(XmlNameHead)
        responseElement.AppendChild(headElement)

        Dim messageIdElement As XmlElement = returnXml.CreateElement(XmlNameMessageId)
        messageIdElement.InnerText = messageId
        headElement.AppendChild(messageIdElement)

        Dim countryCodeElement As XmlElement = returnXml.CreateElement(XmlNameCountryCode)
        countryCodeElement.InnerText = countryCode
        headElement.AppendChild(countryCodeElement)

        Dim receptionDateElement As XmlElement = returnXml.CreateElement(XmlNameReceptionDate)
        receptionDateElement.InnerText = startDateTime
        headElement.AppendChild(receptionDateElement)

        Dim transmissionDateElement As XmlElement = returnXml.CreateElement(XmlNameTransmissionDate)
        transmissionDateElement.InnerText = DateTimeFunc.Now()
        headElement.AppendChild(transmissionDateElement)

        Dim i As Integer
        For i = 0 To returnCodeList.Count - 1

            Dim detailElement As XmlElement = returnXml.CreateElement(XmlNameDetailName)
            responseElement.AppendChild(detailElement)

            Dim commonElement As XmlElement = returnXml.CreateElement(XmlNameCommonName)
            detailElement.AppendChild(commonElement)

            Dim resultIdElement As XmlElement = returnXml.CreateElement(XmlNameResultId)
            resultIdElement.InnerText = returnCodeList(i)
            commonElement.AppendChild(resultIdElement)

            Dim messageElement As XmlElement = returnXml.CreateElement(XmlNameMessage)
            messageElement.InnerText = "" 'messageList(i)
            commonElement.AppendChild(messageElement)

        Next i

        Return returnXml

    End Function
    
#End Region
End Class
