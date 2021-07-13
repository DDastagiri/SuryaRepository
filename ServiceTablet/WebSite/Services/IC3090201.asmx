<%@ WebService Language="VB" Class="Toyota.eCRB.GateKeeper.GateNoticeSend.WebService.IC3090201" %>

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Xml
Imports System.Xml.Serialization
Imports System.IO
Imports System.Globalization

Imports Toyota.eCRB.SystemFrameworks.Core

Imports Toyota.eCRB.GateKeeper.GateNoticeSend.BizLogic

Namespace Toyota.eCRB.GateKeeper.GateNoticeSend.WebService

    ''' <summary>
    ''' IC3090201 来店通知送信IF Webサービス
    ''' </summary>
    ''' <remarks></remarks>
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Public Class IC3090201
        Inherits System.Web.Services.WebService

#Region "定数"

        ''' <summary>
        ''' プログラムID(来店通知送信インタフェース)
        ''' </summary>
        ''' <remarks></remarks>
        Protected Const MessageId As String = "IC3090201"

        ''' <summary>
        ''' メッセージ(成功)
        ''' </summary>
        ''' <remarks>応答結果メッセージ(Success.)</remarks>
        Private Const MessageSuccess As String = "Success"

        ''' <summary>
        ''' メッセージ(失敗)
        ''' </summary>
        ''' <remarks>応答結果メッセージ(Failure.)</remarks>
        Private Const MessageFailure As String = "Failure"

        ''' <summary>
        ''' XML出力時の日時フォーマット
        ''' </summary>
        ''' <remarks>日付時刻のフォーマット</remarks>
        Private Const FormatDateTime As String = "yyyyMMddHHmmss"

#Region "タグ名称"

        ''' <summary>
        ''' Headerタグ名称
        ''' </summary>
        ''' <remarks>Headerタグ</remarks>
        Private Const TagHead As String = "Head"

        ''' <summary>
        ''' Commonタグ名称
        ''' </summary>
        ''' <remarks>Commonタグ</remarks>
        Private Const TagCommon As String = "Common"

        ''' <summary>
        ''' TransmissionDateタグ名称
        ''' </summary>
        ''' <remarks>TransmissionDateタグ</remarks>
        Private Const TagTransmissionDate As String = "TransmissionDate"

        ''' <summary>
        ''' DlrCdタグ名称
        ''' </summary>
        ''' <remarks>DlrCdタグ</remarks>
        Private Const TagDlrCd As String = "DlrCd"


        ''' <summary>
        ''' StrCdタグ名称
        ''' </summary>
        ''' <remarks>StrCdタグ</remarks>
        Private Const TagStrCd As String = "StrCd"


        ''' <summary>
        ''' VclRegNoタグ名称
        ''' </summary>
        ''' <remarks>VclRegNoタグ</remarks>
        Private Const TagVclRegNo As String = "VclRegNo"

#End Region

#Region "入力文字数チェック"

        ''' <summary>
        ''' 販売店コードの最大文字数
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MaxDlrCd As Integer = 5

        ''' <summary>
        ''' 店舗コードの最大文字数
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MaxStrCd As Integer = 3

        ''' <summary>
        ''' 車両登録No.の最大文字数
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MaxVclRegNo As Integer = 32

        ''' <summary>
        ''' TransmissionDateの最大文字数
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MaxTransmissionDate As Integer = 14

#End Region

#Region "入力チェックタイプ"

        ''' <summary>
        ''' 販売店コードのチェックタイプ
        ''' </summary>
        ''' <remarks>文字数、マルチバイト</remarks>
        Private Const TypeDlrCd As Integer = 1

        ''' <summary>
        ''' 店舗コードの最大文字数
        ''' </summary>
        ''' <remarks>文字数、マルチバイト</remarks>
        Private Const TypeStrCd As Integer = 1

        ''' <summary>
        ''' 車両登録No.の最大文字数
        ''' </summary>
        ''' <remarks>文字数</remarks>
        Private Const TypeVclRegNo As Integer = 2

        ''' <summary>
        ''' TransmissionDateの最大文字数
        ''' </summary>
        ''' <remarks>日付</remarks>
        Private Const TypeTransmissionDate As Integer = 3

#End Region

#Region "チェック項目"

        ''' <summary>
        ''' 文字数とバイト数チェック
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CheckSizeAndByte As Integer = 1

        ''' <summary>
        ''' 文字数チェック
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CheckSize As Integer = 2

        ''' <summary>
        ''' 日付チェック
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CheckDate As Integer = 3

#End Region

#Region "エラータイプ"

        ''' <summary>
        ''' マルチバイト
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrorItMulti As Integer = 1

        ''' <summary>
        ''' 文字数オーバー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrorIsSize As Integer = 2

        ''' <summary>
        ''' 空欄
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrorIsEmpty As Integer = 3
        
        ''' <summary>
        ''' Null
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrorIsNull As Integer = 4
        

#End Region

#Region "メッセージID"

        ''' <summary>
        ''' 正常終了
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageIdSuccess As Integer = 0

        ''' <summary>
        ''' XMLタグ不正
        ''' </summary>
        ''' <remarks>XMLタグ不正</remarks>
        Private Const MessageIdXmlError As Integer = -1

        ''' <summary>
        ''' 販売店コードが空欄またはNull
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageIdDlrCdIsNullOrEmpty As Integer = 2001

        ''' <summary>
        ''' 店舗コードが空欄またはNull
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageIdStrCdIsNullOrEmpty As Integer = 2002

        ''' <summary>
        ''' 車両登録No.が空欄またはNull
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageIdVclRegNoIsNullOrEmpty As Integer = 2003
        
        ''' <summary>
        ''' メッセージ送信日時がNull
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageIdTransmissionDateIsNull As Integer = 2004

        ''' <summary>
        ''' 販売店コードがマルチバイト文字を含む
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageIdDlrCdIsMultiByte As Integer = 3001

        ''' <summary>
        ''' 店舗コードがマルチバイト文字を含む
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageIdStrCdIsMultiByte As Integer = 3002

        ''' <summary>
        ''' 販売店コードが規定文字数以上
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageIdDlrCdItSize As Integer = 4001

        ''' <summary>
        ''' 店舗コードが規定文字数以上
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageIdStrCdItSize As Integer = 4002

        ''' <summary>
        ''' 車両登録No.が規定文字数以上
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageIdVclRegNoItSize As Integer = 4003

        ''' <summary>
        ''' システムエラー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageIdSystemError As Integer = 9999
#End Region

#End Region

#Region "メンバ変数"

        ''' <summary>
        ''' 終了コード
        ''' </summary>
        ''' <remarks>応答結果のコード（"0"の場合は正常、それ以外の場合エラー）</remarks>
        Private ResultId As Integer = MessageIdSuccess

        ''' <summary>
        ''' XMLタグのルート要素
        ''' </summary>
        ''' <remarks>受信XMLタグのルート要素</remarks>
        Private RootElement As XmlElement

        ''' <summary>
        ''' XMLタグの要素
        ''' </summary>
        ''' <remarks>受信XML各タグの要素</remarks>
        Private NodeElement As XmlElement

#End Region

#Region "来店通知送信Webサービス"

        ''' <summary>
        ''' 来店通知送信Webサービス
        ''' </summary>
        ''' <param name="xsData">Request URL</param>
        ''' <exception cref="Exception">処理で出た例外</exception>
        ''' <returns>Response URL</returns>
        ''' <remarks>来店通知を送信</remarks>
        <WebMethod()> _
        Public Function GateNotice(ByVal xsData As String) As Response

            'Webサービス開始ログ出力
            Logger.Info("GateNotice_Start Param[" & xsData & "]")
            
            '出力するメッセージ
            Dim responseMessage As String = MessageSuccess

            '生成用XML
            Dim retXml As Response = Nothing

            'Inputメッセージ受信日時
            Dim receptionDate As String = String.Empty

            '来店通知送信用インスタンス生成
            Dim sendGate As New IC3090201BusinessLogic

            'Input情報格納用
            Dim inputData As New Collections.Hashtable

            Try

                'ログ出力
                Logger.Info("RequestXML : " & xsData)
                
                'Inputメッセージ受信日時ログ出力開始
                Logger.Info("GateNotice_001 " & "Call_Start DataTimeFunc.Now ")

                'Inputメッセージ受信日時を取得
                receptionDate = DateTimeFunc.Now.ToString(FormatDateTime, CultureInfo.InvariantCulture)
                
                'Inputメッセージ受信日時ログ出力終了
                Logger.Info("GateNotice_001 " & "Call_End DataTimeFunc.Now Ret[" & receptionDate & "]")

                'XML読み込み、解析情報を返す
                inputData = Me.SetData(xsData)

                '来店通知送信
                Me.ResultId = sendGate.SendGateNotice(inputData(TagDlrCd), inputData(TagStrCd), inputData(TagVclRegNo))
                
                'エラーが来た場合メッセージ格納
                If Me.ResultId <> MessageIdSuccess Then
                    
                    Logger.Info("GateNotice_002 " & "MessageError")

                    'エラーメッセージ格納
                    responseMessage = MessageFailure
              
                End If

            Catch ex As Exception

                Logger.Info("GateNotice_003 " & "ExceptionError")

                'エラーメッセージ格納
                responseMessage = MessageFailure

                '値を設定せずエラーが来た場合はシステムエラー
                If Me.ResultId = MessageIdSuccess Then

                    Logger.Info("GateNotice_004 " & "SystemError")
                    
                    Me.ResultId = MessageIdSystemError
                End If

                Logger.Warn("ResultId : " & CStr(ResultId))
                Logger.Warn("Exception:" & ex.Message)

            Finally
                
                Logger.Info("GateNotice_005 " & "GetResponseXml")
                
                'XMLの生成
                retXml = Me.GetResponseXml(receptionDate, responseMessage)
  
                'ログ出力
                Using writer As New StringWriter(CultureInfo.InvariantCulture())
                    
                    Dim outXml As New XmlSerializer(GetType(Response))
                    outXml.Serialize(writer, retXml)
                    Logger.Info("ResponseXML : " & writer.ToString)

                End Using
            End Try
            
            'Webサービス終了ログ出力
            Dim endLogInfo As New StringBuilder
            endLogInfo.Append("GateNotice_End ")
            endLogInfo.Append("Ret[" & retXml.ToString & "]")
            Logger.Info(endLogInfo.ToString)

            '生成したXMLを返却
            Return retXml

        End Function

#End Region

#Region "XML解析"

        ''' <summary>
        ''' Request XML解析
        ''' </summary>
        ''' <param name="xsData">解析するXMLの文字列</param>
        ''' <exception cref="Exception">解析時のエラー</exception>
        ''' <remarks>Request XML解析を行う</remarks>
        Private Function SetData(ByVal xsData As String) As Hashtable
            
            'SetData開始ログ出力
            Logger.Info("SetData_Start Param[" & xsData & "]")

            '読み込み用XML
            Dim xdoc As New XmlDocument

            '戻り値用
            Dim retInputData As New Hashtable

            Try

                'XML読み込み
                xdoc.LoadXml(xsData)

                ' メンバ変数を設定
                Me.RootElement = xdoc.DocumentElement

            Catch ex As XmlException

                Logger.Info("SetData_001 XmlExceptionError")

                'XML読み込み失敗時は終了コードをセットして処理終了
                Me.ResultId = MessageIdXmlError
                                
                'SetData終了ログ出力
                Logger.Info("SetData_End Throw")
                Throw
            End Try
            
            'Headerタグの情報を取得
            Me.SetHeader()

            'Commonタグの情報を取得
            Me.SetCommon(retInputData)

            'SetData終了ログ出力
            Dim setDataEndLogInfo As New StringBuilder
            setDataEndLogInfo.Append("SetData_End ")
            setDataEndLogInfo.Append("ret[" & retInputData.ToString & "]")
            Logger.Info(setDataEndLogInfo.ToString())
            
            Return retInputData
        End Function
#End Region

#Region "プロパティーセット"

        ''' <summary>
        ''' Headerタグ情報のプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定
        ''' </remarks>
        Private Sub SetHeader()

            'SetHeader開始ログ出力
            Logger.Info("SetHeader_Start")
            
            'XMLノードリスト
            Dim nodeList As XmlNodeList

            'XML要素
            Dim nodeDocument As XmlDocument

            Try
                'XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagHead)

                'XML要素を設定
                nodeDocument = New XmlDocument
                nodeDocument.LoadXml(nodeList.ItemOf(0).OuterXml)
                Me.NodeElement = nodeDocument.DocumentElement

                ' TransmissionDateタグのNodeListを取得
                Me.GetElementValue(TagTransmissionDate, MaxTransmissionDate, _
                                   TypeTransmissionDate)
            
            Catch ex As NullReferenceException

                Logger.Info("SetHeader_001 NullReferenceException")
                
                '関係のないタグだった場合
                Me.ResultId = MessageIdXmlError
                Logger.Info("SetHeader_End Throw")
                Throw
            Finally

                nodeDocument = Nothing
                Me.NodeElement = Nothing
            End Try
            
            'SetHeader終了ログ出力
            Logger.Info("SetHeader_End")

        End Sub

        ''' <summary>
        ''' Commonタグ情報のプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定
        ''' </remarks>
        Private Sub SetCommon(ByVal commonInput As Hashtable)

            'SetCommon開始ログ出力
            Dim setCommonStartLogInfo As New StringBuilder
            setCommonStartLogInfo.Append("SetCommon_Start ")
            setCommonStartLogInfo.Append("param[" & commonInput.ToString & "]")
            Logger.Info(setCommonStartLogInfo.ToString())
            
            ' XMLノードリスト
            Dim nodeList As XmlNodeList
            
            ' XML要素
            Dim nodeDocument As XmlDocument

            Try
                ' XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagCommon)

                ' XML要素を設定
                nodeDocument = New XmlDocument
                nodeDocument.LoadXml(nodeList.ItemOf(0).OuterXml)
                Me.NodeElement = nodeDocument.DocumentElement

                ' ModeタグのNodeListを取得する
                commonInput(TagDlrCd) = Me.GetElementValue(TagDlrCd, MaxDlrCd, TypeDlrCd)

                ' UpdateDvsタグのNodeListを取得する
                commonInput(TagStrCd) = Me.GetElementValue(TagStrCd, MaxStrCd, TypeStrCd)

                ' VcloptionUpdateDvsタグのNodeListを取得する
                commonInput(TagVclRegNo) = Me.GetElementValue(TagVclRegNo, MaxVclRegNo, TypeVclRegNo)
                
            Catch ex As NullReferenceException
                
                Logger.Info("SetCommon_001 NullReferenceException")

                '関係のないタグだった場合
                Me.ResultId = MessageIdXmlError
                Logger.Info("SetCommon_End Throw")
                Throw
            Finally

                nodeDocument = Nothing
                Me.NodeElement = Nothing
            End Try
            
            'SetCommon終了ログ出力
            Logger.Info("SetCommon_End")

        End Sub

#End Region

#Region "XML内のデータ取得"

        ''' <summary>
        ''' 解析した情報からデータを取得
        ''' </summary>
        ''' <param name="tagName">取得するタグの名前</param>
        ''' <param name="maximum">最大文字数</param>
        ''' <param name="type">チェックタイプ</param>
        ''' <returns>解析した文字列</returns>
        ''' <remarks>解析して取得した文字列を返却</remarks>
        Private Function GetElementValue(ByVal tagName As String, ByVal maximum As Integer, ByVal type As Integer) As String

            'GetElementValue開始ログ出力
            Dim getElementValueStartLogInfo As New StringBuilder
            getElementValueStartLogInfo.Append("GetElementValue_Start ")
            getElementValueStartLogInfo.Append("param1[" & tagName & "]")
            getElementValueStartLogInfo.Append(",param2[" & maximum & "]")
            getElementValueStartLogInfo.Append(",param3[" & tagName & "]")
            Logger.Info(getElementValueStartLogInfo.ToString())
            
            ' 返却するオブジェクト
            Dim valueStr As String = String.Empty

            '指定タグのNodeListを取得する
            Dim node As XmlNodeList = Me.NodeElement.GetElementsByTagName(tagName)

            '指定したタグの存在有無により値をSet
            Dim valueString As String = String.Empty

            Try
                
                'タグの存在チェック
                If 0 < node.Count Then
                    
                    Logger.Info("GetElementValue_001 node.Count > 0")

                    '指定したタグが存在したのでInnerTextプロパティで値を取得する
                    valueString = Trim(node.Item(0).InnerText)
                Else

                    Logger.Info("GetElementValue_002 node.Count <= 0")
                    
                    '必須項目で値がない場合はエラー
                    Me.ResultId = GetErrorId(tagName, ErrorIsNull)
                    Throw New ArgumentException("", Me.ResultId)
                End If

                Select Case type

                    Case CheckSizeAndByte
                        
                        Logger.Info("GetElementValue_003 CheckSizeAndByte:" & CheckSizeAndByte)

                        If String.IsNullOrEmpty(valueString) Then

                            Logger.Info("GetElementValue_004 IsNullOrEmpty:" & tagName)
                            
                            '文字数が空欄、またはNULLの場合はエラー
                            Me.ResultId = GetErrorId(tagName, ErrorIsEmpty)
                            Throw New ArgumentException("", Me.ResultId)
                        End If

                        If Validation.IsCorrectDigit(valueString, maximum) = False Then

                            Logger.Info("GetElementValue_005 ErrorIsSize:" & tagName)
                            
                            '文字数が上限を超えていた場合はエラー
                            Me.ResultId = GetErrorId(tagName, ErrorIsSize)
                            Throw New ArgumentException("", Me.ResultId)
                        End If

                        If Validation.IsCorrectByte(valueString, valueString.Length) = False Then

                            Logger.Info("GetElementValue_006 ErrorItMulti:" & tagName)
                            
                            'バイト数が上限を超えていた場合はエラー
                            Me.ResultId = GetErrorId(tagName, ErrorItMulti)
                            Throw New ArgumentException("", Me.ResultId)
                        End If

                    Case CheckSize
                        
                        Logger.Info("GetElementValue_007 CheckSize:" & CheckSize)

                        If String.IsNullOrEmpty(valueString) Then

                            Logger.Info("GetElementValue_008 IsNullOrEmpty:" & tagName)
                            
                            '文字数が空欄、またはNULLの場合はエラー
                            Me.ResultId = GetErrorId(tagName, ErrorIsEmpty)
                            Throw New ArgumentException("", Me.ResultId)
                        End If

                        If Validation.IsCorrectDigit(valueString, maximum) = False Then

                            Logger.Info("GetElementValue_009 ErrorIsSize:" & tagName)
                            
                            '文字数が上限を超えていた場合はエラー
                            Me.ResultId = GetErrorId(tagName, ErrorIsSize)
                            Throw New ArgumentException("", Me.ResultId)
                        End If

                    Case CheckDate

                        Logger.Info("GetElementValue_010 CheckDate:" & CheckDate)
                        
                        '日付の形式が正しいかチェック
                        ConvertDateTime(valueString)

                    Case Else
                        
                        Logger.Info("GetElementValue_011 Case Else(error)")
                        Throw New ArgumentException("", Me.ResultId)
                End Select

            Catch ex As ArgumentException

                Logger.Info("GetElementValue_012 ArgumentException")
                
                '対象外エラーが出た場合はシステムエラー
                If Me.ResultId = MessageIdSuccess Then

                    Me.ResultId = MessageIdSystemError
                End If
                
                Logger.Info("GetElementValue_End Throw")
                Throw
            End Try

            '文字列格納
            valueStr = valueString
            
            'GetElementValue終了ログ出力
            Dim getElementValueEndLogInfo As New StringBuilder
            getElementValueEndLogInfo.Append("GetElementValue_End ")
            getElementValueEndLogInfo.Append("Ret[" & valueStr & "]")
            Logger.Info(getElementValueEndLogInfo.ToString())

            ' 結果を返却
            Return valueStr

        End Function

#End Region

#Region "日時の書式チェック"
    
        ''' <summary>
        ''' 日時の書式に合わせて変換チェックを行う
        ''' </summary>
        ''' <param name="checkString">チェック対象文字列</param>
        ''' <remarks></remarks>
        Private Sub ConvertDateTime(ByVal checkString As String)
            
            'GetElementValue開始ログ出力
            Dim convertDateTimeStartLogInfo As New StringBuilder
            convertDateTimeStartLogInfo.Append("ConvertDateTime_Start ")
            convertDateTimeStartLogInfo.Append("param [" & checkString & "]")
            Logger.Info(convertDateTimeStartLogInfo.ToString())
            
            Try

                '指定されたフォーマット書式の日付に変換
                DateTime.ParseExact(checkString, FormatDateTime, Nothing)
            Catch ex As FormatException

                Logger.Info("ConvertDateTime_001 FormatException")
                
                Me.ResultId = MessageIdXmlError
                Logger.Info("ConvertDateTime_End Throw")
                Throw
            End Try
            
            'GetElementValue終了ログ出力
            Logger.Info("ConvertDateTime_End")
            
        End Sub
#End Region

#Region "エラーID取得"

        ''' <summary>
        ''' エラーID取得
        ''' </summary>
        ''' <param name="tagName">タグ情報</param>
        ''' <param name="type">エラーのタイプ</param>
        ''' <returns>エラーID</returns>
        ''' <remarks></remarks>
        Private Function GetErrorId(ByVal tagName As String, ByVal type As Integer) As Integer

            'GetErrorId開始ログ出力
            Dim getErrorIdStartLogInfo As New StringBuilder
            getErrorIdStartLogInfo.Append("GetErrorId_Start ")
            getErrorIdStartLogInfo.Append("param1 [" & tagName & "]")
            getErrorIdStartLogInfo.Append(",param2 [" & type & "]")
            Logger.Info(getErrorIdStartLogInfo.ToString())
            
            'TODO
            'エラーID格納変数
            Dim returnId As Integer = MessageIdSuccess
            
            'DlrCdタグでのエラー
            If String.Equals(tagName, TagDlrCd) Then
                
                Logger.Info("GetErrorId_001 DlrCd")

                Select Case type
                    Case ErrorIsSize
                    
                        Logger.Info("GetErrorId_End Ret[" & MessageIdDlrCdItSize & "]")
                        
                        '販売店コードの文字数が上限を超えていた場合
                        Return MessageIdDlrCdItSize
                    Case ErrorIsEmpty
                        
                        Logger.Info("GetErrorId_End Ret[" & MessageIdDlrCdIsNullOrEmpty & "]")
                       
                        '販売店コードが空欄だった場合
                        Return MessageIdDlrCdIsNullOrEmpty
                    Case ErrorIsNull
                        
                        Logger.Info("GetErrorId_End Ret[" & MessageIdDlrCdIsNullOrEmpty & "]")
                       
                        '販売店コードがNULLだった場合
                        Return MessageIdDlrCdIsNullOrEmpty
                    Case ErrorItMulti
                    
                        Logger.Info("GetErrorId_End Ret[" & MessageIdDlrCdIsMultiByte & "]")
                       
                        '販売店コードにマルチバイトコードが存在した場合
                        Return MessageIdDlrCdIsMultiByte
                    Case Else
                        
                        Logger.Info("GetErrorId_End Ret[" & MessageIdSystemError & "]")
                       
                        '別の例外が発生した場合
                        Return MessageIdSystemError
                End Select

            End If

            'StrCdタグでのエラー
            If String.Equals(tagName, TagStrCd) Then

                Logger.Info("GetErrorId_002 StrCd")

                Select Case type
                    Case ErrorIsSize
                        
                        Logger.Info("GetErrorId_End Ret[" & MessageIdStrCdItSize & "]")
                    
                        '店舗コードの文字数が上限を超えていた場合
                        Return MessageIdStrCdItSize
                    Case ErrorIsEmpty
                        
                        Logger.Info("GetErrorId_End Ret[" & MessageIdStrCdIsNullOrEmpty & "]")
                       
                        '店舗コードが空欄だった場合
                        Return MessageIdStrCdIsNullOrEmpty
                    Case ErrorIsNull
                        
                        Logger.Info("GetErrorId_End Ret[" & MessageIdStrCdIsNullOrEmpty & "]")
                       
                        '店舗コードがNULLだった場合
                        Return MessageIdStrCdIsNullOrEmpty
                    Case ErrorItMulti
                    
                        Logger.Info("GetErrorId_End Ret[" & MessageIdStrCdIsMultiByte & "]")
                       
                        '店舗コードにマルチバイトコードが存在した場合
                        Return MessageIdStrCdIsMultiByte
                    Case Else
                        
                        Logger.Info("GetErrorId_End Ret[" & MessageIdSystemError & "]")
                       
                        '別の例外が発生した場合
                        Return MessageIdSystemError
                End Select

            End If

            If String.Equals(tagName, TagVclRegNo) Then

                Logger.Info("GetErrorId_003 VclRegNo")

                Select Case type
                    Case ErrorIsSize
                    
                        Logger.Info("GetErrorId_End Ret[" & MessageIdVclRegNoItSize & "]")
                    
                        '車両登録No.の文字数が上限を超えていた場合
                        Return MessageIdVclRegNoItSize
                    Case ErrorIsEmpty
                        
                        Logger.Info("GetErrorId_End Ret[" & MessageIdVclRegNoIsNullOrEmpty & "]")
                    
                        '車両登録No.が空欄だった場合
                        Return MessageIdVclRegNoIsNullOrEmpty
                    Case ErrorIsNull
                    
                        Logger.Info("GetErrorId_End Ret[" & MessageIdVclRegNoIsNullOrEmpty & "]")
                        
                        '車両登録No.がNULLだった場合
                        Return MessageIdVclRegNoIsNullOrEmpty
                    Case Else
                        
                        Logger.Info("GetErrorId_End Ret[" & MessageIdSystemError & "]")
                        
                        '別の例外が発生した場合
                        Return MessageIdSystemError
                End Select
            End If
            
            If String.Equals(tagName, TagTransmissionDate) And type = ErrorIsNull Then

                          
                'メッセージ送信日時がNULLの場合
                Logger.Info("GetErrorId_End Ret[" & MessageIdTransmissionDateIsNull & "]")
                Return MessageIdTransmissionDateIsNull

            End If

            Logger.Info("GetErrorId_End Ret[" & returnId & "]")
               
            ' 結果を返却
            Return returnId

        End Function

#End Region

#Region "応答用XML作成"

        ''' <summary>
        ''' 応答用のXMLを生成する
        ''' </summary>
        ''' <param name="receptionDate">Inputメッセージ受信日時</param>
        ''' <param name="retMessage">応答結果のメッセージ</param>
        ''' <returns>生成したXMLオブジェクト</returns>
        ''' <remarks></remarks>
        Private Function GetResponseXml(ByVal receptionDate As String, ByVal retMessage As String) As Response

            'GetErrorId開始ログ出力
            Dim getResponseXmlStartLogInfo As New StringBuilder
            getResponseXmlStartLogInfo.Append("GetResponseXml_Start ")
            getResponseXmlStartLogInfo.Append("param1 [" & receptionDate & "]")
            getResponseXmlStartLogInfo.Append(",param2 [" & retMessage & "]")
            Logger.Info(getResponseXmlStartLogInfo.ToString())
            
            Logger.Info("GetResponseXml_001 " & "Call_Start DateTimeFunc.Now")
            
            ' システム日付を取得する
            Dim transmissionDate As String = DateTimeFunc.Now.ToString(FormatDateTime, CultureInfo.InvariantCulture)

            Logger.Info("GetResponseXml_001 " & "Call_End DateTimeFunc.Now Ret[" & transmissionDate & "]")
            
            ' Responseクラス生成
            Dim createResponse As Response = New Response()

            ' Headerクラスに値をセット
            Dim createRespHead As Response.RootHead = New Response.RootHead()
            createRespHead.MessageId = MessageId
            createRespHead.ReceptionDate = receptionDate
            createRespHead.TransmissionDate = transmissionDate

            ' Detailクラス生成
            Dim createRespDetail As Response.RootDetail = New Response.RootDetail()

            ' Commonクラスに値をセット
            Dim createRespCommon As Response.RootDetail.DetailCommon = New Response.RootDetail.DetailCommon()
            createRespCommon.ResultId = Me.ResultId.ToString(CultureInfo.InvariantCulture)
            createRespCommon.Message = retMessage

            'Commonにセットした値をDetailに反映
            createRespDetail.Common = createRespCommon

            'Header,Detailにセットした値をResponseに反映
            createResponse.Head = createRespHead
            createResponse.Detail = createRespDetail

            'GetErrorId開始ログ出力
            Dim getResponseXmlEndLogInfo As New StringBuilder
            getResponseXmlEndLogInfo.Append("GetResponseXml_End ")
            getResponseXmlEndLogInfo.Append("Ret[" & createResponse.ToString & "]")
            Logger.Info(getResponseXmlEndLogInfo.ToString())

            '生成したResponseオブジェクトを返却
            Return createResponse

        End Function
#End Region

    End Class

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
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Common", IsNullable:=False)> _
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
    
End Namespace