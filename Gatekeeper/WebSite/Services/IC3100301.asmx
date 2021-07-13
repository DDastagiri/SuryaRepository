<%@ WebService Language="VB" Class="Toyota.eCRB.Visit.VisitResult.WebService.IC3100301" %>
Option Explicit On
Option Strict On

Imports System.Globalization
Imports System.IO
Imports System.Web.Services
Imports System.Xml.Serialization
Imports Toyota.eCRB.Common.VisitResult.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Visit.VisitResult.BizLogic

Namespace Toyota.eCRB.Visit.VisitResult.WebService
    
    ''' <summary>
    ''' IC3100301 来店実績更新インターフェース Webサービス
    ''' </summary>  
    ''' <remarks></remarks>
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Public Class IC3100301
        Inherits System.Web.Services.WebService
        
#Region "列挙体"

        ''' <summary>
        ''' メッセージIDの列挙体
        ''' </summary>
        ''' <remarks></remarks>
        Private Enum Result As Integer
            ''' <summary>
            ''' 正常終了
            ''' </summary>
            ''' <remarks></remarks>
            None = 0
            
            ''' <summary>
            ''' システムエラー
            ''' </summary>
            ''' <remarks></remarks>
            SystemError = 9999
        End Enum

#End Region

#Region "定数"

        ''' <summary>
        ''' プログラムID（来店実績更新インターフェース）
        ''' </summary>
        ''' <remarks></remarks>
        Protected Const ProgramId As String = "IC3100301"
        
        ''' <summary>
        ''' XML出力時の日時フォーマット
        ''' </summary>
        ''' <remarks>日付時刻のフォーマット</remarks>
        Private Const FormatDateTime As String = "yyyyMMddHHmmss"

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

#Region "Webサービス"
        
        ''' <summary>
        ''' ログイン
        ''' </summary>
        ''' <returns>応答用のXML</returns>
        ''' <remarks>指定されたログインアカウントに対して、商談終了処理(来店実績情報の更新処理)を実施する。</remarks>
        <WebMethod(EnableSession:=True)> _
        Public Function UpdateVisitLogin() As Response
            
            Logger.Info("UpdateVisitLogin_Start")

            ' 受信日時
            Logger.Info("UpdateVisitLogin_001 Call_Start DateTimeFunc.Now")
            Dim receptionDate As Date = DateTimeFunc.Now()
            Logger.Info(New StringBuilder( _
                    "UpdateVisitLogin_001 Call_End DateTimeFunc.Now Ret[").Append( _
                    receptionDate).Append("]").ToString())
            ' 終了コード
            Dim resultId As Integer = Result.None
            ' メッセージ
            Dim message As String = Nothing
            ' 更新件数
            Dim count As Integer = 0
            
            Try
                ' 来店実績更新_ログインを実施
                Dim bl As New IC3100301BusinessLogic
                count = bl.UpdateVisitLogin(ProgramId, resultId)
                
                ' 処理に成功した場合
                ' かつ更新件数が1以上の場合
                If Result.None = resultId _
                    AndAlso 0 < count Then
                    Logger.Info("UpdateVisitLogin_002")
                    ' Push送信
                    bl.PushUpdateVisitLogin()
                End If

                bl = Nothing
                
                ' 処理の途中で例外が発生した場合
            Catch ex As Exception
                Logger.Info("UpdateVisitLogin_003")

                ' 終了コードが設定されていない場合
                If Result.None = resultId Then
                    Logger.Info("UpdateVisitLogin_004")
                    ' システムエラー
                    resultId = Result.SystemError
                End If

                Logger.Warn(New StringBuilder("ResultId: ").Append(resultId).ToString())
                Logger.Warn(New StringBuilder("Exception: ").Append(ex.Message).ToString())
            End Try
            
            ' 処理に成功した場合
            If Result.None = resultId Then
                Logger.Info("UpdateVisitLogin_005")
                message = MessageSuccess
                
                ' 処理に失敗した場合
            Else
                Logger.Info("UpdateVisitLogin_006")
                message = MessageFailure
            End If
            
            ' 送信日時
            Logger.Info("UpdateVisitLogin_007 Call_Start DateTimeFunc.Now")
            Dim transmissionDate As Date = DateTimeFunc.Now()
            Logger.Info(New StringBuilder( _
                    "UpdateVisitLogin_007 Call_End DateTimeFunc.Now Ret[").Append( _
                    transmissionDate).Append("]").ToString())
            
            ' 応答用のXMLの生成
            Dim retXml As Response = GetResponseXml(receptionDate, transmissionDate, resultId, _
                    message, count)
  
            Using writer As New StringWriter(CultureInfo.InvariantCulture)
                Dim outXml As New XmlSerializer(GetType(Response))
                outXml.Serialize(writer, retXml)
                outXml = Nothing
                Logger.Debug(New StringBuilder("ResponseXML: ").Append(writer).ToString())
            End Using
            
            Logger.Info(New StringBuilder("UpdateVisitLogin_End Ret[").Append(retXml).Append( _
                    "]").ToString())

            ' 戻り値に応答用のXMLを設定
            Return retXml
        
        End Function

#Region "応答用XML作成"

        ''' <summary>
        ''' 応答用のXMLを生成する
        ''' </summary>
        ''' <param name="receptionDate">受信日時</param>
        ''' <param name="transmissionDate">送信日時</param>        
        ''' <param name="resultId">終了コード</param>
        ''' <param name="message">応答結果のメッセージ</param>
        ''' <param name="count">更新件数</param>        
        ''' <returns>生成したXMLオブジェクト</returns>
        ''' <remarks></remarks>
        Private Function GetResponseXml( _
                ByVal receptionDate As Date, ByVal transmissionDate As Date, _
                ByVal resultId As Integer, ByVal message As String, ByVal count As Integer) _
                As Response

            Logger.Info(New StringBuilder("GetResponseXml_Start Param[").Append( _
                    receptionDate).Append(", ").Append(transmissionDate).Append(", ").Append( _
                    resultId).Append(", ").Append(message).Append(", ").Append(count).Append( _
                    "]").ToString())
            
            ' Headerに値をセット
            Dim headTag As New Response.RootHead
            headTag.MessageId = ProgramId
            headTag.ReceptionDate = receptionDate.ToString(FormatDateTime, _
                    CultureInfo.InvariantCulture)
            headTag.TransmissionDate = transmissionDate.ToString(FormatDateTime, _
                    CultureInfo.InvariantCulture)
            
            ' Commonに値をセット
            Dim commonTag As New Response.RootDetail.DetailCommon
            commonTag.ResultId = resultId.ToString(CultureInfo.InvariantCulture)
            commonTag.Message = message

            ' UpdateInfoに値をセット
            Dim updateInfoTag As New Response.RootDetail.DetailUpdateInfo
            updateInfoTag.Count = count.ToString(CultureInfo.InvariantCulture)
            
            ' Detailに値をセット
            Dim detailTag As New Response.RootDetail
            detailTag.Common = commonTag
            detailTag.UpdateInfo = updateInfoTag
            
            ' Responseに値をセット
            Dim responseTag As New Response
            responseTag.Head = headTag
            responseTag.Detail = detailTag

            Logger.Info(New StringBuilder("GetResponseXml_End Ret[").Append(responseTag).Append( _
                    "]").ToString())

            ' 戻り値に生成したXMLオブジェクトを設定
            Return responseTag

        End Function

#End Region

#End Region

    End Class

    ''' <summary>
    ''' Responseクラス（応答用XMLのIFクラス）
    ''' </summary>
    ''' <remarks>応答用のXML情報を格納するクラス</remarks>
    <System.Xml.Serialization.XmlRoot("Response", Namespace:="http://tempuri.org/Response.xsd")> _
    Public Class Response
        
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
            ''' UpdateInfoタグの定義
            ''' </summary>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="UpdateInfo", IsNullable:=False)> _
            Private outUpdateInfo As DetailUpdateInfo

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
            ''' UpdateInfoタグ用のプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property UpdateInfo() As DetailUpdateInfo
                Set(ByVal value As DetailUpdateInfo)
                    outUpdateInfo = value
                End Set
                Get
                    Return outUpdateInfo
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

#Region "UpdateInfoタグ"
            
            ''' <summary>
            ''' UpdateInfoタグ用クラス
            ''' </summary>
            ''' <remarks></remarks>
            Public Class DetailUpdateInfo
                
#Region "タグの定義"
                
                ''' <summary>
                ''' Countタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="Count", IsNullable:=False)> _
                Private outCount As String

#End Region

#Region "タグ用のプロパティ"
                
                ''' <summary>
                ''' Countタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property Count() As String
                    Set(ByVal value As String)
                        outCount = value
                    End Set
                    Get
                        Return outCount
                    End Get
                End Property

#End Region

            End Class
            
#End Region
            
        End Class

#End Region

    End Class
    
End Namespace