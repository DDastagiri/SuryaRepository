<%@ WebService Language="VB" Class="Toyota.eCRB.eCRB.TCV.TCVSetting.WebService.IC3050701" %>

Imports System.IO
Imports System.Data
Imports System.Globalization
Imports System.Reflection.MethodBase
Imports System.Xml.Serialization
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.IC3050701
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.TCVSettingUtility
Imports System.Xml

Namespace Toyota.eCRB.eCRB.TCV.TCVSetting.WebService

    ' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
    ' <System.Web.Script.Services.ScriptService()> _
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    Public Class IC3050701
        Inherits System.Web.Services.WebService
    
#Region "定数"
        
        ''' <summary>
        ''' 正常終了
        ''' </summary>
        ''' <remarks></remarks>
        Protected Const RetSuccess As String = "0"
        
        ''' <summary>
        ''' 異常終了
        ''' </summary>
        ''' <remarks></remarks>
        Protected Const RetError As String = "9999"


        ''' <summary>
        ''' プログラムID（販売店サーバーへの転送）
        ''' </summary>
        ''' <remarks></remarks>
        Protected Const ProgramId As String = "IC3050701"
        
        ''' <summary>
        ''' XML出力時の日時フォーマット
        ''' </summary>
        ''' <remarks>日付時刻のフォーマット</remarks>
        Private Const FormatDateTime As String = "yyyyMMddHHmmss"
    
        ''' <summary>
        ''' プログラムID（販売店サーバーへの転送）
        ''' </summary>
        ''' <remarks></remarks>
        Protected Const ProgramIdLog As String = ProgramId + " IC3050701.asmx-"


#Region "メッセージ"
        
        ''' <summary>
        ''' メッセージ（成功）
        ''' </summary>
        ''' <remarks>応答結果メッセージ（Success.）</remarks>
        Private Const MessageSuccess As String = "Success"

        ''' <summary>
        ''' メッセージ（失敗）
        ''' </summary>
        ''' <remarks>応答結果メッセージ（Failure.）</remarks>
        Private Const MessageFailure As String = "Failure"

#End Region
#End Region
    
        ''' <summary>
        ''' 販売店サーバーへの転送機能
        ''' </summary>
        ''' <returns>zip形式圧縮ファイル</returns>
        ''' <remarks></remarks>
        <WebMethod()> _
        Public Function CreateZip(ByVal xsData As String) As TcvSettingResponse
        
            Dim responseTag As TcvSettingResponse = Nothing
            '受信日時の取得
            Dim receptionDate As String = DateTimeFunc.FormatDate(15, DateTimeFunc.Now)

        
            Try
                '開始ログ出力
                Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
                '受信ログ
                Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogParam("xsData", CType(xsData, String), False))
        
                'リクエストXML格納変数
                Dim reqestXmlDocument As New XmlDocument()

                '入力XMLの取得とチェック
                reqestXmlDocument.PreserveWhitespace = True
                reqestXmlDocument.LoadXml(xsData)
        
                Dim detail As XmlNodeList = reqestXmlDocument.GetElementsByTagName("BasicInfo")
                Dim syncBeforeTimeNode As XmlNode = detail.Item(0).item("BeforeReplicationTime")
        
                Dim syncBeforeTime As String = syncBeforeTimeNode.InnerXml
                
                Dim syncDlrCdNode As XmlNode = detail.Item(0).Item("DlrCd")
        
                Dim syncDlrcd As String = syncDlrCdNode.InnerXml
                
                Dim syncDevideNoNode As XmlNode = detail.Item(0).Item("DevideNo")
        
                Dim syncDevideNo As String = syncDevideNoNode.InnerXml

                Dim syncZipFileNameNode As XmlNode = detail.Item(0).Item("ZipFileName")
                Dim syncZipFileName As String = syncZipFileNameNode.InnerXml

                '圧縮ファイルの作成ビジネスロジック(前回同期時刻を渡す)       
                '圧縮ファイル作成処理クラスを呼び出す
                Dim executeClass As New IC3050701BusinessLogic
                Using dtset As IC3050701DataSet = executeClass.CanExcute(syncDlrcd, syncBeforeTime, syncDevideNo,syncZipFileName)
                     
                    '送信日時の取得
                    Dim transmissionDate As String = DateTimeFunc.FormatDate(15, DateTimeFunc.Now)
                
                    'レスポンスデータの定義
                    Dim res As New TcvSettingResponse
                
                    'HeadタグのメッセージIDをセット
                    Dim headTag As New TcvSettingResponse.RootHead
        
                    'プログラムIDをセット
                    headTag.MessageId = ProgramId
                    '受信日時をセット
                    headTag.ReceptionDate = receptionDate
                    '送信日時をセット
                    headTag.TransmissionDate = transmissionDate
                
                    ' DetailタグのCommonに値をセット
                    Dim commonTag As New TcvSettingResponse.RootDetail.DetailCommon
                    Dim detailTag As New TcvSettingResponse.RootDetail
                    
                    Dim msgId As String = RetError
                    Dim msg As String = Nothing
                    Dim archiveData As Byte() = Nothing
                    Dim repTime As String = Nothing
                    Dim devideCount As String = Nothing
                    Dim checkCode As String = Nothing
                    Dim zipFileName As String = Nothing
                    
                    Dim rep As IC3050701DataSet.REPINFODataTableDataTable = dtset.REPINFODataTable
                    For i As Integer = 0 To rep.Count - 1
                        Dim repRow As IC3050701DataSet.REPINFODataTableRow = rep.Item(i)
                            
                        If Not rep.Item(i).IsmsgIdNull Then
                            msgId = repRow.Item("msgId")
                        End If
                        
                        If Not rep.Item(i).IsmsgNull Then
                            msg = repRow.Item("msg")
                        End If

                        
                        If Not rep.Item(i).IsArchiveDataNull Then
                            '圧縮ファイルのバイトデータをセット      
                            archiveData = repRow.Item("ArchiveData")
                        End If
                        
                        If Not rep.Item(i).IsRepTimeNull Then
                            '圧縮ファイルのバイトデータをセット      
                            repTime = CStr(repRow.Item("RepTime"))
                        End If
                        
                        If Not rep.Item(i).IsDevideCountNull Then
                            '圧縮ファイルの分割件数をセット      
                            devideCount = CStr(repRow.Item("DevideCount"))
                        End If
                        
                        If Not rep.Item(i).IsCheckCodeNull Then
                            '圧縮ファイルのハッシュコードをセット      
                            checkCode = CStr(repRow.Item("CheckCode"))
                        End If
                        
                        If Not rep.Item(i).IsZipFileNameNull Then
                            '圧縮ファイル名をセット      
                            zipFileName = CStr(repRow.Item("ZipFileName"))
                        End If

                    Next
                    
                    '記録用更新リスト情報
                    Dim updList As IC3050701DataSet.FileListDataTable = dtset.FileList
                    
                    Dim updFileList = New List(Of String)
                    For i As Integer = 0 To updList.Count - 1
                        Dim updListRow As IC3050701DataSet.FileListRow = updList.Item(i)
                        
                        If Not updList.Item(i).IsUpdListFileNameNull Then
                            '圧縮ファイル名をセット 
                             updFileList.Add(updListRow.Item("UpdListFileName"))
                        End If

                    Next

                    '正常時
                    If msgId = RetSuccess Then
                        commonTag.Message = MessageSuccess
                        commonTag.ResultId = msgId
                        ' archiveInfoTagに値をセット
                        Dim repDataTag As New TcvSettingResponse.RootDetail.DetailRepData
                        
                        repDataTag.ArchiveData = archiveData
                        repDataTag.ReplicationTime = repTime
                        repDataTag.DevideCount = devideCount
                        repDataTag.CheckCode = checkCode
                        repDataTag.ZipFileName = zipFileName
                        
                        repDataTag.UpdFileList = updFileList
                        
                        ' Detailに値をセット
                        detailTag.ReplicationData = repDataTag
                        
                        Logger.Info(ProgramIdLog + "Message : " + commonTag.Message)
                        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogWarn(commonTag.ResultId))
                    ElseIf msgId = RetError Then
                        '異常時
                        commonTag.Message = MessageFailure & ":" & msg
                        commonTag.ResultId = RetError
                        Logger.Info(ProgramIdLog + "Message : " + commonTag.Message)
                        Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogWarn(commonTag.ResultId))
                    End If
            
                    detailTag.Common = commonTag

                    ' Responseに値をセット
                    responseTag = New TcvSettingResponse
                    responseTag.Head = headTag
                    responseTag.Detail = detailTag

                    '終了ログ出力
                    Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetReturnDataSet(dtset))
                    Logger.Info(ProgramIdLog + TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))
                
                End Using
            
            Catch ex As Exception
                'DebugLog
                Logger.Error(ProgramIdLog + ex.Message, ex)
                ' 異常終了ログ
                Logger.Info(ProgramIdLog + "Process AbNormalEnd")
            
                'レスポンスデータの定義
                responseTag = New TcvSettingResponse
                
                'HeadタグのメッセージIDをセット
                Dim headTag As New TcvSettingResponse.RootHead
        
                'プログラムIDをセット
                headTag.MessageId = ProgramId
                '受信日時をセット
                headTag.ReceptionDate = receptionDate
            
                '送信日時の取得
                Dim transmissionDate As String = DateTimeFunc.FormatDate(15, DateTimeFunc.Now)
            
                '送信日時をセット
                headTag.TransmissionDate = transmissionDate
            
                ' DetailタグのCommonに値をセット
                Dim commonTag As New TcvSettingResponse.RootDetail.DetailCommon
                Dim detailTag As New TcvSettingResponse.RootDetail
            
                '異常時(例外発生時)
                commonTag.Message = MessageFailure & ":" & ex.Message
                commonTag.ResultId = RetError
            
                detailTag.Common = commonTag
            
                responseTag.Head = headTag
                responseTag.Detail = detailTag
            
            End Try
        
            Using writer As New StringWriter(CultureInfo.InvariantCulture)
                Dim outXml As New XmlSerializer(GetType(TcvSettingResponse))
                outXml.Serialize(writer, responseTag)
                outXml = Nothing
            End Using

        
            Return responseTag

        End Function

    End Class

    ''' <summary>
    ''' Responseクラス（応答用XMLのIFクラス）
    ''' </summary>
    ''' <remarks>応答用のXML情報を格納するクラス</remarks>
    <System.Xml.Serialization.XmlRoot("Response", Namespace:="http://tempuri.org/Response.xsd")> _
    Public Class TcvSettingResponse
        
#Region "タグの定義"
        
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

#End Region

#Region "タグ用プロパティ"
        
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

#End Region

#Region "Headタグ"
        
        ''' <summary>
        ''' Headタグ用クラス
        ''' </summary>
        ''' <remarks></remarks>
        Public Class RootHead
            
#Region "タグの定義"
            
            ''' <summary>
            ''' MessageIDタグの定義
            ''' </summary>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="MessageID", IsNullable:=False)> _
            Private outMessageID As String

            ''' <summary>
            ''' ReceptionDateタグの定義
            ''' </summary>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ReceptionDate", IsNullable:=False)> _
            Private outReceptionDate As String

            ''' <summary>
            ''' TransmissionDateタグの定義
            ''' </summary>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="TransmissionDate", IsNullable:=False)> _
            Private outTransmissionDate As String
            
#End Region

#Region "タグのプロパティ"
            
            ''' <summary>
            ''' MessageID用タグのプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property MessageId() As String
                Set(ByVal value As String)
                    outMessageID = value
                End Set
                Get
                    Return outMessageID
                End Get
            End Property

            ''' <summary>
            ''' ReceptionDateタグ用のプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ReceptionDate", IsNullable:=False)> _
            Public Property ReceptionDate As String
                Get
                    Return outReceptionDate
                End Get
                Set(ByVal value As String)
                    outReceptionDate = value
                End Set
            End Property

            ''' <summary>
            ''' TransmissionDateタグ用のプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="TransmissionDate", IsNullable:=False)> _
            Public Property TransmissionDate As String
                Get
                    Return outTransmissionDate
                End Get
                Set(ByVal value As String)
                    outTransmissionDate = value
                End Set
            End Property

#End Region

        End Class

#End Region

#Region "Detailタグ"
        
        ''' <summary>
        ''' Detailタグ用クラス
        ''' </summary>
        ''' <remarks></remarks>
        Public Class RootDetail

#Region "タグの定義"
            
            ''' <summary>
            ''' Commonタグの定義
            ''' </summary>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Common", IsNullable:=False)> _
            Private outCommon As DetailCommon
            
            ''' <summary>
            ''' ReplicationDataタグの定義
            ''' </summary>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ReplicationData", IsNullable:=False)> _
            Private outRepData As DetailRepData

#End Region

#Region "タグ用のプロパティ"
            
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
            ''' ReplicationDataタグ用のプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property ReplicationData() As DetailRepData
                Set(ByVal value As DetailRepData)
                    outRepData = value
                End Set
                Get
                    Return outRepData
                End Get
            End Property
            
#End Region

#Region "Commonタグ"
            
            ''' <summary>
            ''' Commonタグ用クラス
            ''' </summary>
            ''' <remarks></remarks>
            Public Class DetailCommon

#Region "タグの定義"
                
                ''' <summary>
                ''' ResultIdタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="ResultId", IsNullable:=False)> _
                Private outResultId As String

                ''' <summary>
                ''' Messageタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="Message", IsNullable:=True)> _
                Private outMessage As String

#End Region

#Region "タグ用のプロパティ"
                
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

#End Region

            End Class

#End Region

#Region "ReplicationDataタグ"
            
            ''' <summary>
            ''' ReplicationDataタグ用クラス
            ''' </summary>
            ''' <remarks></remarks>
            Public Class DetailRepData
                
#Region "タグの定義"
                
                ''' <summary>
                ''' CheckCodeタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="CheckCode", IsNullable:=False)> _
                Private outCheckCode As String

                ''' <summary>
                ''' DevideCountタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="DevideCount", IsNullable:=False)> _
                Private outDevideCount As Integer
                
                ''' <summary>
                ''' ArchiveDataタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="ArchiveData", IsNullable:=False)> _
                Private outArchiveData As Byte()
            
                ''' <summary>
                ''' RepTimeタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="ReplicationTime", IsNullable:=False)> _
                Private outRepTime As String
                
                ''' <summary>
                ''' ZipFileNameタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="ZipFileName", IsNullable:=False)> _
                Private outZipFileName As String

                ''' <summary>
                ''' UpdFileListタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="UpdFileList", IsNullable:=False)> _
                Private outUpdFileList As List(Of String)



#End Region

#Region "タグ用のプロパティ"
                
                ''' <summary>
                ''' CheckCodeタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property CheckCode As String
                    Set(ByVal value As String)
                        outCheckCode = value
                    End Set
                    Get
                        Return outCheckCode
                    End Get
                End Property
                
                ''' <summary>
                ''' DevideCountタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property DevideCount As Integer
                    Set(ByVal value As Integer)
                        outDevideCount = value
                    End Set
                    Get
                        Return outDevideCount
                    End Get
                End Property
               
                ''' <summary>
                ''' ArchiveDataタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property ArchiveData As Byte()
                    Set(ByVal value As Byte())
                        outArchiveData = value
                    End Set
                    Get
                        Return outArchiveData
                    End Get
                End Property
            
                ''' <summary>
                ''' RepTimeタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property ReplicationTime As String
                    Set(ByVal value As String)
                        outRepTime = value
                    End Set
                    Get
                        Return outRepTime
                    End Get
                End Property
                
                ''' <summary>
                ''' ZipFileNameタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property ZipFileName As String
                    Set(ByVal value As String)
                        outZipFileName = value
                    End Set
                    Get
                        Return outZipFileName
                    End Get
                End Property
                
                ''' <summary>
                ''' FileListタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property UpdFileList As List(Of String)
                    Set(ByVal value As List(Of String))
                        outUpdFileList = value
                    End Set
                    Get
                        Return outUpdFileList
                    End Get
                End Property



#End Region

            End Class
            
#End Region
            
        End Class

#End Region

    End Class
End Namespace