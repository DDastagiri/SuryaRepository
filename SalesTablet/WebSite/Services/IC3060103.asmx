<%@ WebService Language="VB" Class="Toyota.eCRB.Assessment.Assessment.WebService.IC3060103" %>
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Assessment.Assessment
Imports System.Xml
Imports System.IO
Imports System.Data
Imports System.Globalization.CultureInfo

Namespace Toyota.eCRB.Assessment.Assessment.WebService
    
    ' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
    ' <System.Web.Script.Services.ScriptService()> _
    
    ''' <summary>
    ''' 中古車査定情報登録Webservice
    ''' </summary>
    ''' <remarks>中古車査定情報の登録を行います。</remarks>
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Public Class IC3060103
        Inherits System.Web.Services.WebService
    

#Region "定数定義"
    
        'メッセージID
        Private Const messageId As String = "IC3060103"
        
        '正常終了コード
        Private Const successCode As String = "000000"
   
        'XMLノード名
        Private Const nodeRoot As String = "RegistApprisalPrice"
        Private Const nodeHead As String = "Head"
        Private Const nodeMessageID As String = "MessageID"
        Private Const nodeTransmissionDate As String = "TransmissionDate"
        Private Const nodeDetail As String = "Detail"
        Private Const nodeCommon As String = "Common"
        Private Const nodeAssessmentNo As String = "AssessmentNo"
        Private Const nodeDealerCode As String = "DealerCode"
        Private Const nodeBranchCode As String = "BranchCode"
        Private Const nodeStaffCode As String = "StaffCode"
        Private Const nodeApprisalInfo As String = "ApprisalInfo"
        Private Const nodeMakerName As String = "MakerName"
        Private Const nodeVehicleName As String = "VehicleName"
        Private Const nodeRegistrationNo As String = "RegistrationNo"
        Private Const nodeInspectionDate As String = "InspectionDate"
        Private Const nodeApprisalPrice As String = "Apprisal_Price"
    
        'リターンメッセージ
        Private Const succsess As String = "Success"
        Private Const errXml As String = "XML Format Error"
        Private Const errMandatory As String = "Mandatory Error"
        Private Const errType As String = "Type Error"
        Private Const errSize As String = "Size Error"
        Private Const errValue As String = "Value Error"
        Private Const errAssessmentNoData As String = "Assessment NoData"
        Private Const errAssessmentCanceld As String = "Assessment Canceld"
        Private Const errSystem As String = "System Error"
    
        'ログ出力用メッセージ
        Private Const errAssessmentNoUpdate As String = "Assessment NoUpdate"
        Private Const errInputCheck As String = "Input Check Error"

        '日付形式
        Private Const dateTimeFormat As String = "yyyy/MM/dd HH:mm:ss"

#End Region
    
#Region "メンバ変数"

        ''' <summary>
        ''' XMLノード定義ディクショナリ
        ''' </summary>
        Private xmlNodeDic As New Dictionary(Of String, xmlDataDefine)
    
        ''' <summary>
        ''' 受信日時
        ''' </summary>
        Private valTeceptionDate As String = Now.ToString(dateTimeFormat, InvariantCulture)
    
        ' ''' <summary>
        ' ''' 送信日時
        ' ''' </summary>
        'Private valTransmissionDate As String
    
        ''' <summary>
        ''' 中古車査定情報登録対象項目リスト
        ''' </summary>
        ''' <remarks>更新対象項目名をリストに設定してください。</remarks>
        Private ucaAsesssmentUpColumnList As New List(Of String)
        
#End Region
    
#Region "中古車査定情報登録WebMethod"
        ''' <summary>
        ''' 中古車査定情報登録WebMethod
        ''' </summary>
        ''' <param name="xsData">要求XML</param>
        ''' <returns>応答XML</returns>
        ''' <remarks>中古車査定情報へ連携データを反映します。</remarks>
        <WebMethod()> _
        Public Function IC3060103(ByVal xsData As String) As Interface_Response
        
            Logger.Info("IC3060103 Start")
            
            ' リターン結果
            Dim resultId As Integer = 0
    
            ' リターンメッセージ
            Dim resultMessage As String = succsess
            
            '応答XMLシリアライズクラス
            Dim responseCls As Interface_Response
        
            ' 要求XMLドキュメント
            Dim reqXmlDoc As XmlDocument
            
            Try
                
                '要求内容ログ出力
                Logger.Info(xsData, True)
       
                'XMLノード定義ディクショナリ作成
                setxmlDataDefineDictionary()
            
                ' 要求XMLドキュメントインスタンス生成
                reqXmlDoc = New XmlDocument
                
                Try
                    'XmlDocument作成
                    reqXmlDoc.LoadXml(xsData)
                Catch ex As Exception
                    'XML不正エラー
                    Throw New OriginalException(resultCode.errXml, errXml)
                End Try

                ' 中古車査定情報データテーブル トインスタンス作成
                Using ucaAsesssmentDt As New DataAccess.IC3060103DataSet.IC3060103SetUcarAssessmentDataTable
                                    
                    'データセット設定（インプットチェックを合わせて行う）
                    setAssessmentDataTable(ucaAsesssmentDt, reqXmlDoc.DocumentElement)

                    '査定価格登録
                    Dim bizLogicCls As New Bizlogic.IC3060103BizLogic(ucaAsesssmentDt, ucaAsesssmentUpColumnList)
                    Dim resultBiz As Bizlogic.IC3060103ResultCode
                    resultBiz = bizLogicCls.SetAssessmentPrice

                    Select Case resultBiz
                
                        Case Bizlogic.IC3060103ResultCode.ErrAssessmentCanceld
                    
                            '査定依頼キャンセル済み
                            Throw New OriginalException(resultCode.errAssessmentCanceld, errAssessmentCanceld)

                        Case Bizlogic.IC3060103ResultCode.ErrAssessmentNoData
                    
                            '査定情報存在エラー
                            Throw New OriginalException(resultCode.errAssessmentNoData, errAssessmentNoData)

                        Case Bizlogic.IC3060103ResultCode.ErrAssessmentNotUpdate
                    
                            '査定情報存在エラー（システムエラー）
                            Throw New OriginalException(resultCode.errSystem, errAssessmentNoUpdate)

                    End Select

                End Using

            Catch orgErr As OriginalException
    
                'アプリケーションエラー
                resultId = orgErr.ErrorCode
                resultMessage = orgErr.ErrorMessage
                Logger.Error(resultMessage, orgErr)
            Catch ex As Exception
        
                'システムエラー
                resultId = resultCode.errSystem
                resultMessage = errSystem
                Logger.Error(errSystem, ex)
            
            Finally
                'デバッグログ
                Logger.Debug("ResultID:" & resultId & " ReqestXML:" & xsData)
                
                '結果クラス生成
                responseCls = SetResultXml(resultId, resultMessage)
                
                reqXmlDoc = Nothing

            End Try
        
            '結果返却
            Return responseCls
            
            Logger.Info("IC3060103 End")
            
        End Function
#End Region
        
        
#Region "データテーブル設定"
        
        ''' <summary>
        ''' データテーブル設定
        ''' </summary>
        ''' <param name="dt">中古車査定情報データテーブル</param>
        ''' <param name="reqXmlElement">要求XMLルート要素</param>
        ''' <remarks>要求XMLを中古車査定情報データテーブルへ設定する。</remarks>
        Private Sub SetAssessmentDataTable(ByRef dt As DataAccess.IC3060103DataSet.IC3060103SetUcarAssessmentDataTable, ByVal reqXmlElement As XmlElement)

            'データテーブル未使用項目チェック
            chekReqXmlValue(reqXmlElement, nodeMessageID)
            chekReqXmlValue(reqXmlElement, nodeDealerCode)
            chekReqXmlValue(reqXmlElement, nodeBranchCode)
            ChekReqXmlValue(reqXmlElement, nodeTransmissionDate)
            
            ''送信日時取得
            'Dim valTransDateWk As Date
            'valTransDateWk =
            'valTransmissionDate = valTransDateWk.ToString(dateTimeFormat, InvariantCulture)

            'データテーブル使用項目セット
            Dim myRow As DataRow
            myRow = dt.NewRow

            '編集開始
            myRow.BeginEdit()
            
            '査定No------------------------------------------------------------------------------------
            myRow(dt.ASSESSMENTNOColumn.ColumnName) = chekReqXmlValue(reqXmlElement, nodeAssessmentNo)
            '中古車査定情報登録対象項目Dictionary設定
            If reqXmlElement.GetElementsByTagName(nodeAssessmentNo).Count > 0 Then
                ucaAsesssmentUpColumnList.Add(dt.ASSESSMENTNOColumn.ColumnName)
            End If
            
            '更新アカウント------------------------------------------------------------------------------------
            myRow(dt.STAFFCDColumn.ColumnName) = chekReqXmlValue(reqXmlElement, nodeStaffCode)
            '中古車査定情報登録対象項目Dictionary設定
            If reqXmlElement.GetElementsByTagName(nodeStaffCode).Count > 0 Then
                ucaAsesssmentUpColumnList.Add(dt.STAFFCDColumn.ColumnName)
            End If
            
            'メーカー名------------------------------------------------------------------------------------
            myRow(dt.MAKERNAMEColumn.ColumnName) = chekReqXmlValue(reqXmlElement, nodeMakerName)
            '中古車査定情報登録対象項目Dictionary設定
            If reqXmlElement.GetElementsByTagName(nodeMakerName).Count > 0 Then
                ucaAsesssmentUpColumnList.Add(dt.MAKERNAMEColumn.ColumnName)
            End If
            
            '車名------------------------------------------------------------------------------------
            myRow(dt.VEHICLENAMEColumn.ColumnName) = chekReqXmlValue(reqXmlElement, nodeVehicleName)
            '中古車査定情報登録対象項目Dictionary設定
            If reqXmlElement.GetElementsByTagName(nodeVehicleName).Count > 0 Then
                ucaAsesssmentUpColumnList.Add(dt.VEHICLENAMEColumn.ColumnName)
            End If
            
            '検査日------------------------------------------------------------------------------------
            myRow(dt.INSPECTIONDATEColumn.ColumnName) = chekReqXmlValue(reqXmlElement, nodeInspectionDate)
            '中古車査定情報登録対象項目Dictionary設定
            If reqXmlElement.GetElementsByTagName(nodeInspectionDate).Count > 0 Then
                ucaAsesssmentUpColumnList.Add(dt.INSPECTIONDATEColumn.ColumnName)
            End If
            
            '登録番号------------------------------------------------------------------------------------
            myRow(dt.REGISTRATIONNOColumn.ColumnName) = chekReqXmlValue(reqXmlElement, nodeRegistrationNo)
            '中古車査定情報登録対象項目Dictionary設定
            If reqXmlElement.GetElementsByTagName(nodeRegistrationNo).Count > 0 Then
                ucaAsesssmentUpColumnList.Add(dt.REGISTRATIONNOColumn.ColumnName)
            End If
            
            '提示価格------------------------------------------------------------------------------------
            myRow(dt.APPRISAL_PRICEColumn.ColumnName) = ChekReqXmlValue(reqXmlElement, nodeApprisalPrice)
            '中古車査定情報登録対象項目Dictionary設定
            If reqXmlElement.GetElementsByTagName(nodeApprisalPrice).Count > 0 Then
                ucaAsesssmentUpColumnList.Add(dt.APPRISAL_PRICEColumn.ColumnName)
            End If
            
            '更新機能ID------------------------------------------------------------------------------------
            myRow(dt.UPDATEIDColumn.ColumnName) = messageId
            
            '編集行追加
            dt.AddIC3060103SetUcarAssessmentRow(myRow)


        End Sub
    
        ''' <summary>
        ''' XMLノード値チェック
        ''' </summary>
        ''' <param name="reqXmlElement">要求XMLルート要素</param>
        ''' <param name="xmlNodeName">ノード名</param>
        ''' <returns>値オブジェクト</returns>
        ''' <remarks></remarks>
        Private Function ChekReqXmlValue(ByVal reqXmlElement As XmlElement, ByVal xmlNodeName As String) As Object
        
            Dim valueObj As Object = Convert.DBNull

            '対象カラムの構造定義取得-----------------------------------
            Dim xmlDefine As xmlDataDefine
            xmlDefine = xmlNodeDic.Item(xmlNodeName)
            'No
            Dim nodeNo As Integer = xmlDefine.nodeNo
            '必須
            Dim nodeMandatory As xmlMandatory = xmlDefine.nodeMandatory
            '型
            Dim nodeType As xmlNodeType = xmlDefine.nodeType
            'サイズ
            Dim nodeSize As Integer = xmlDefine.nodeSize
            '値(ノードの値が決まっている場合は配列として列挙)
            Dim nodeValue() As String = xmlDefine.nodeValue


            '指定タグのNodeListを取得する------------------------------
            Dim node As XmlNodeList = reqXmlElement.GetElementsByTagName(xmlNodeName)
               
            '指定したタグの存在有無により値をSet
            Dim valueString As String
            If node.Count > 0 Then
                '指定したタグが存在したのでInnerTextプロパティで値を取得する
                valueString = Trim(node.Item(0).InnerText)
                
            Else
                valueString = ""
            End If

            '値チェック--------------------------------------------
            If [String].IsNullOrEmpty(valueString) Then
                '必須チェック
                If nodeMandatory = xmlMandatory.mandatoryCulumn Then
                    'エラー（必須エラー）
                    Throw New OriginalException(resultCode.errMandatory + nodeNo, errMandatory)
                End If

            Else

                '項目属性チェック
                Select Case nodeType

                    Case xmlNodeType.xmlString_Byte
                        '文字列サイズチェック（byteチェック）
                        If Not Validation.IsCorrectByte(valueString, nodeSize) Then
                            'エラー（サイズエラー）
                            Throw New OriginalException(resultCode.errSize + nodeNo, errSize)
                        End If

                        '返却値設定
                        valueObj = valueString

                    Case xmlNodeType.xmlString_Length
                        '文字列サイズチェック（長さチェック）
                        If Not Validation.IsCorrectDigit(valueString, nodeSize) Then
                            'エラー（サイズエラー）
                            Throw New OriginalException(resultCode.errSize + nodeNo, errSize)
                        End If
                    
                        '返却値設定
                        valueObj = valueString

                    Case xmlNodeType.xmlNnumber
                        '数値型チェック
                        If Not Validation.IsHankakuNumber(valueString) Then
                            'エラー（型エラー）
                            Throw New OriginalException(resultCode.errType + nodeNo, errType)
                        End If
                        '数値サイズチェック
                        If Not Validation.IsCorrectByte(valueString, nodeSize) Then
                            'エラー（サイズエラー）
                            Throw New OriginalException(resultCode.errSize + nodeNo, errSize)
                        End If
                    
                        '返却値設定
                        valueObj = CInt(valueString)

                    Case xmlNodeType.xmlDateTime
                        '日付型変換
                        Try
                            '返却値設定
                            valueObj = DateTime.ParseExact(valueString, dateTimeFormat, Nothing)

                        Catch ex As Exception
                            'エラー（型エラー）
                            Throw New OriginalException(resultCode.errType + nodeNo, errType)

                        End Try
                    Case Else

                End Select

                '項目値チェック
                If Not nodeValue Is Nothing Then
            
                    Dim result As Boolean = False
                    Dim valueCheck As String
                    Dim i As Integer

                    For i = 0 To UBound(nodeValue, 1)

                        valueCheck = nodeValue(i)

                        If valueString.Equals(valueCheck) Then
                            result = True
                            Exit For
                        End If
                    Next

                    If Not result Then
                        'エラー（値チェックエラー）
                        Throw New OriginalException(resultCode.errValue + nodeNo, errValue)

                    End If
                
                End If

            End If
        
            Return valueObj

        End Function
    
        ''' <summary>
        ''' Responseクラス生成
        ''' </summary>
        ''' <param name="id">結果ID</param>
        ''' <param name="msg">結果メッセージ</param>
        ''' <returns>Responseクラス</returns>
        ''' <remarks></remarks>
        Private Function SetResultXml(ByVal id As Integer, ByVal msg As String) As Interface_Response


            'Head設定
            Dim responseHead As New Interface_Response.Root_Head
            responseHead.MessageId = messageId
            responseHead.ReceptionDate = valTeceptionDate
            responseHead.TransmissionDate = Now.ToString(dateTimeFormat, InvariantCulture)

            'Common設定
            Dim responseCommon As New Interface_Response.Root_Detail.Detail_Common
            If id = 0 Then
                responseCommon.ResultId = successCode
            Else
                responseCommon.ResultId = id
            End If
            responseCommon.Message = msg

            'Detail設定
            Dim responseDetail As New Interface_Response.Root_Detail
            responseDetail.Common = responseCommon

            'Response設定
            Dim response As New Interface_Response
            response.Head = responseHead
            response.Detail = responseDetail

            Return response

        End Function

#End Region
    
#Region "リーターンコード,リターンメッセージ"

        ''' <summary>
        ''' リーターンコード
        ''' </summary>
        ''' <remarks>リターンコードの列挙体です。</remarks>
        Private Enum ResultCode As Integer
            ''' <summary>
            ''' 処理正常終了
            ''' </summary>
            Succsess = 0

            ''' <summary>
            ''' XML不正エラー
            ''' </summary>
            ErrXml = -1
        
            ''' <summary>
            ''' 項目必須エラー
            ''' </summary>
            ErrMandatory = 200000
        
            ''' <summary>
            ''' 項目型エラー
            ''' </summary>
            ErrType = 300000
        
            ''' <summary>
            ''' 項目サイズエラー
            ''' </summary>
            ErrSize = 400000
        
            ''' <summary>
            ''' 値チェックエラー
            ''' </summary>
            ErrValue = 500000
        
        
            ''' <summary>
            ''' 査定情報存在エラー
            ''' </summary>
            ErrAssessmentNoData = 600001

            ''' <summary>
            ''' 査定依頼キャンセル済み
            ''' </summary>
            ErrAssessmentCanceld = 600002
        
            ''' <summary>
            ''' 想定外エラー
            ''' </summary>
            ErrSystem = 999999
        End Enum
    
#End Region
        
        
        
#Region "XMLインプットチェック用ノード定義"

        ''' <summary>
        ''' XMLノード定義ディクショナリ作成
        ''' </summary>
        ''' <remarks>XMLノード定義ディクショナリを作成します。</remarks>
        Private Sub SetxmlDataDefineDictionary()
        
            'XMLノード定義ディクショナリ設定
            With xmlNodeDic
                'メッセージID
                .Add(nodeMessageID, New XmlDataDefine(1, XmlMandatory.MandatoryCulumn, XmlNodeType.XmlString_Byte, 9, {"IC3060103"}))
                '共通タグ
                .Add(nodeTransmissionDate, New XmlDataDefine(2, XmlMandatory.MandatoryCulumn, XmlNodeType.XmlDateTime, Nothing, Nothing))
                '査定No
                .Add(nodeAssessmentNo, New XmlDataDefine(3, XmlMandatory.MandatoryCulumn, XmlNodeType.XmlNnumber, 9, Nothing))
                '販売店コード
                .Add(nodeDealerCode, New XmlDataDefine(4, XmlMandatory.MandatoryCulumn, XmlNodeType.XmlString_Byte, 5, Nothing))
                '店舗コード
                .Add(nodeBranchCode, New XmlDataDefine(5, XmlMandatory.MandatoryCulumn, XmlNodeType.XmlString_Byte, 3, Nothing))
                '中古車スタッフコード
                .Add(nodeStaffCode, New XmlDataDefine(6, XmlMandatory.MandatoryCulumn, XmlNodeType.XmlString_Length, 26, Nothing))
                'メーカー名
                .Add(nodeMakerName, New XmlDataDefine(7, XmlMandatory.OptionalCulumn, XmlNodeType.XmlString_Length, 128, Nothing))
                '車名
                .Add(nodeVehicleName, New XmlDataDefine(8, XmlMandatory.OptionalCulumn, XmlNodeType.XmlString_Length, 256, Nothing))
                '登録番号
                .Add(nodeRegistrationNo, New XmlDataDefine(9, XmlMandatory.OptionalCulumn, XmlNodeType.XmlString_Length, 128, Nothing))
                '検査日
                .Add(nodeInspectionDate, New XmlDataDefine(10, XmlMandatory.OptionalCulumn, XmlNodeType.XmlDateTime, Nothing, Nothing))
                '提示価格
                .Add(nodeApprisalPrice, New XmlDataDefine(11, XmlMandatory.MandatoryCulumn, XmlNodeType.XmlNnumber, 9, Nothing))
            End With
        End Sub
    
    
        ''' <summary>
        ''' XMLノードタイプ列挙体
        ''' </summary>
        Private Enum XmlNodeType
            ''' <summary>
            ''' 文字列(Byte数制限)
            ''' </summary>        
            XmlString_Byte

            ''' <summary>
            ''' 文字列(文字数制限)
            ''' </summary>        
            XmlString_Length
        
            ''' <summary>
            ''' 日付（年月日時分秒）
            ''' </summary>
            XmlDateTime
        
            ''' <summary>
            ''' 数値
            ''' </summary>
            XmlNnumber
        End Enum

    
        ''' <summary>
        ''' XMLノード必須列挙体
        ''' </summary>
        Private Enum XmlMandatory
            ''' <summary>
            ''' 必須
            ''' </summary>        
            MandatoryCulumn
        
            ''' <summary>
            ''' オプショナル
            ''' </summary>
            OptionalCulumn
        End Enum

        ''' <summary>
        ''' XMLノード定義構クラス
        ''' </summary>
        Private Class XmlDataDefine
        
            'No
            Public NodeNo As Integer
            '必須
            Public NodeMandatory As XmlMandatory
            '型
            Public NodeType As XmlNodeType
            'サイズ
            Public NodeSize As Integer
            '値
            'ノードの値が決まっている場合は配列で列挙する。
            Public NodeValue() As String
	
            ''' <summary>
            ''' XMLノード定義構クラス設定
            ''' </summary>
            ''' <param name="n">ノードNo</param>
            ''' <param name="t">タイプ</param>
            ''' <param name="s">サイズ</param>
            ''' <param name="v">値配列</param>
            ''' <remarks></remarks>
            Sub New(ByVal n As Integer, ByVal m As XmlMandatory, ByVal t As XmlNodeType, ByVal s As Integer, ByVal v() As String)
                NodeNo = n
                NodeMandatory = m
                NodeType = t
                NodeSize = s
                NodeValue = v
            End Sub
        
        End Class

#End Region
    
        
        
#Region "Responseクラス"
        ''' <summary>
        ''' Responseクラス
        ''' </summary>
        ''' <remarks>応答XML作成用シリアライズクラス</remarks>
        <Serialization.XmlRoot("Response", Namespace:="http://tempuri.org/")> _
        Public Class Interface_Response
            
            Private nodeDetail As Root_Detail
            Private nodeHead As Root_Head

            ''' <summary>
            ''' Head要素
            ''' </summary>
            ''' <value>Headクラス</value>
            ''' <returns>Headクラス</returns>
            <System.Xml.Serialization.XmlElementAttribute(Elementname:="Head")> _
            Public Property Head As Root_Head
                Set(ByVal value As Root_Head)
                    nodeHead = value
                End Set
                Get
                    Return nodeHead
                End Get
            End Property
            
            ''' <summary>
            ''' Detail要素
            ''' </summary>
            ''' <value>Detailクラス</value>
            ''' <returns>Detailクラス</returns>
            <System.Xml.Serialization.XmlElementAttribute(Elementname:="Detail")> _
            Public Property Detail As Root_Detail
                Set(ByVal value As Root_Detail)
                    nodeDetail = value
                End Set
                Get
                    Return nodeDetail
                End Get
            End Property

            ''' <summary>
            ''' Headクラス
            ''' </summary>
            Public Class Root_Head
                Private nodeMessageId As String
                Private nodeReceptionDate As String
                Private nodeTransmissionDate As String
                
                ''' <summary>
                ''' MessageIdノード
                ''' </summary>
                ''' <value>MessageId</value>
                ''' <returns>MessageId</returns>
                <System.Xml.Serialization.XmlElementAttribute(Elementname:="MessageID")> _
                Public Property MessageId As String
                    Set(ByVal value As String)
                        nodeMessageId = value
                    End Set
                    Get
                        Return nodeMessageId
                    End Get
                End Property
                
                ''' <summary>
                ''' ReceptionDateノード
                ''' </summary>
                ''' <value>ReceptionDate</value>
                ''' <returns>ReceptionDate</returns>
                <System.Xml.Serialization.XmlElementAttribute(Elementname:="ReceptionDate")> _
                Public Property ReceptionDate As String
                    Set(ByVal value As String)
                        nodeReceptionDate = value
                    End Set
                    Get
                        Return nodeReceptionDate
                    End Get
                End Property
                
                ''' <summary>
                ''' TransmissionDateノード
                ''' </summary>
                ''' <value>TransmissionDate</value>
                ''' <returns>TransmissionDate</returns>
                <System.Xml.Serialization.XmlElementAttribute(Elementname:="TransmissionDate")> _
                Public Property TransmissionDate As String
                    Set(ByVal value As String)
                        nodeTransmissionDate = value
                    End Set
                    Get
                        Return nodeTransmissionDate
                    End Get
                End Property
            End Class

            ''' <summary>
            ''' Detailクラス
            ''' </summary>
            Public Class Root_Detail
                
                Private nodeCommon As Detail_Common

                ''' <summary>
                ''' Common要素
                ''' </summary>
                ''' <value>Commonクラス</value>
                ''' <returns>Commonクラス</returns>
                <System.Xml.Serialization.XmlElementAttribute(Elementname:="Common")> _
                Public Property Common As Detail_Common
                    Set(ByVal value As Detail_Common)
                        nodeCommon = value
                    End Set
                    Get
                        Return nodeCommon
                    End Get
                End Property

                ''' <summary>
                ''' Detailクラス
                ''' </summary>
                Public Class Detail_Common
                    
                    Private nodeResultId As String
                    Private nodeMessage As String
                    
                    ''' <summary>
                    ''' ResultIdノード
                    ''' </summary>
                    ''' <value>ResultId</value>
                    ''' <returns>ResultId</returns>
                    <System.Xml.Serialization.XmlElementAttribute(Elementname:="ResultId")> _
                    Public Property ResultId As String
                        Set(ByVal value As String)
                            nodeResultId = value
                        End Set
                        Get
                            Return nodeResultId
                        End Get
                    End Property
                    
                    ''' <summary>
                    ''' Messageノード
                    ''' </summary>
                    ''' <value>Message</value>
                    ''' <returns>Message</returns>
                    <System.Xml.Serialization.XmlElementAttribute(Elementname:="Message")> _
                    Public Property Message As String
                        Set(ByVal value As String)
                            nodeMessage = value
                        End Set
                        Get
                            Return nodeMessage
                        End Get
                    End Property
                End Class
            End Class
        End Class

#End Region
    
    
    
#Region "独自例外クラス"
        
        ''' <summary>
        ''' 独自例外クラス
        ''' </summary>
        ''' <remarks></remarks>
        <SerializableAttribute()> _
        Private Class OriginalException
            Inherits Exception
            
            Private cd As Integer
            Private msg As String
            
            ''' <summary>
            ''' エラーコード
            ''' </summary>
            Friend ReadOnly Property ErrorCode As Integer
                Get
                    Return cd
                End Get
            End Property
            
            ''' <summary>
            ''' エラーメッセージ
            ''' </summary>
            Friend ReadOnly Property ErrorMessage As String
                Get
                    Return msg
                End Get
            End Property
            
            ''' <summary>
            ''' エラークラス生成コンストラクタ
            ''' </summary>
            ''' <param name="errorCode">エラーコード</param>
            ''' <param name="errorMessage">エラーメッセージ</param>
            ''' <remarks></remarks>
            Friend Sub New(ByVal errorCode As Integer, ByVal errorMessage As String)
                MyBase.New(errorMessage)
                cd = errorCode
                msg = errorMessage
            End Sub
            
            ''' <summary>
            ''' エラークラス生成コンストラクタ
            ''' </summary>
            ''' <param name="seri">シリアライゼイションインフォ</param>
            ''' <param name="con">シリアライゼイションコンテキスト</param>
            ''' <remarks></remarks>
            Protected Sub New(ByVal seri As System.Runtime.Serialization.SerializationInfo, _
                            ByVal con As System.Runtime.Serialization.StreamingContext)
                MyBase.New(seri, con)
            End Sub
        End Class
#End Region
       
    End Class   'IC3060103
       
End Namespace