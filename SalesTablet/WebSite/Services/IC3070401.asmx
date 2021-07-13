
<%@ WebService Language="VB" Class="Toyota.eCRB.Estimate.Recommended.WebService.IC3070401" %>

'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3070401.asmx
'─────────────────────────────────────
'機能： オススメ情報取得IF 
'補足： 
'作成： 2012/03/01 TCS 陳
'更新： 
'─────────────────────────────────────

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Recommended.BizLogic
Imports Toyota.eCRB.Estimate.Recommended.DataAccess
Imports Microsoft.VisualBasic.Collection
Imports System.Collections.ObjectModel


Namespace Toyota.eCRB.Estimate.Recommended.WebService

    ' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
    ' <System.Web.Script.Services.ScriptService()> _
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    Public Class IC3070401
        Inherits System.Web.Services.WebService
    
#Region "定数"
    
        ''' <summary>
        ''' メッセージID
        ''' </summary>
        ''' <remarks>メッセージ識別コード(IC3070401) オススメ情報取得</remarks>
        Private Const MESSAGEID_CONST As String = "IC3070401"
    
        ''' <summary>
        ''' メッセージ(成功)
        ''' </summary>
        ''' <remarks>応答結果メッセージ(Success.)</remarks>
        Private Const MESSAGE_SUCCESS_CONST As String = "Success"
    
        ''' <summary>
        ''' メッセージ(失敗)
        ''' </summary>
        ''' <remarks>応答結果メッセージ(Failure.)</remarks>
        Private Const MESSAGE_FAILURE_CONST As String = "Failure"
        
    
        ''' <summary>
        ''' 日付時刻のフォーマット
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FormatDate As String = "yyyyMMdd"
    
        ''' <summary>
        ''' 日付時刻のフォーマット
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FormatDatetime As String = "yyyyMMddHHmmss"

        ''' <summary>
        ''' 必須項目：チェックなし
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CheckNoRequired As Short = 0

        ''' <summary>
        ''' 必須項目：チェックあり
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CheckRequired As Short = 1

        ''' <summary>
        ''' 属性値：Byteチェック
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AttributeByte As Short = 0

        ''' <summary>
        ''' 属性値：文字数チェック
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AttributeLegth As Short = 1

        ''' <summary>
        ''' 属性値：Numericチェック
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AttributeNum As Short = 2

        ''' <summary>
        ''' 属性値：Dateチェック
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AttributeDate As Short = 3

        ''' <summary>
        ''' 属性値：Datetimeチェック
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AttributeDatetime As Short = 4
        
        ''' <summary>
        ''' エラーコード：処理正常終了(該当データ有）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeSuccess As Short = 0

        ''' <summary>
        ''' エラーコード：XML Document不正
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeXmlDoc As Short = -1

        ''' <summary>
        ''' エラーコード：項目必須エラー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeItMust As Short = 2000

        ''' <summary>
        ''' エラーコード：項目型エラー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeItType As Short = 3000

        ''' <summary>
        ''' エラーコード：項目サイズエラー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeItSize As Short = 4000

        ''' <summary>
        ''' エラーコード：値チェックエラー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeItValue As Short = 5000

        ''' <summary>
        ''' エラーコード：データ存在エラー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeDataNothing As Short = 6001

        ''' <summary>
        ''' エラーコード：システムエラー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeSys As Short = 9999
        
        ''' <summary>
        ''' Headerタグ名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagHead As String = "Head"
        
        ''' <summary>
        ''' Conditionsタグ名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagConditions As String = "Conditions"
                
        ''' <summary>
        ''' 送信日付
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TransmissionDate_No As Short = 1

        ''' <summary>
        ''' 販売店コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DlrCd_No As Short = 101
        
        ''' <summary>
        ''' シリーズコード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SeriesCd_No As Short = 102
        
        ''' <summary>
        ''' モデルコード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ModelCd_No As Short = 103
    
#End Region

#Region "メンバ変数"
    
        ''' <summary>
        ''' 終了コード
        ''' </summary>
        ''' <remarks></remarks>
        Private ResultId As Integer
    
        ''' <summary>
        ''' 取得結果格納オブジェクト
        ''' </summary>
        ''' <remarks></remarks>
        Private ResponseObject As Response
        
        ''' <summary>
        ''' 販売店コード
        ''' </summary>
        ''' <remarks></remarks>
        Private DlrCd_ As String
        
        ''' <summary>
        ''' シリーズコード
        ''' </summary>
        ''' <remarks></remarks>
        Private SeriesCd_ As String
        
        ''' <summary>
        ''' モデルコード
        ''' </summary>
        ''' <remarks></remarks>
        Private ModelCd_ As String
        
        ''' <summary>
        ''' 送信日時（Request）
        ''' </summary>
        ''' <remarks>メッセージ送信日時(yyyyMMddHHmmss)</remarks>
        Private transmissionDate_ As Date

        ''' <summary>
        ''' 項目名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Itemname() As String
        
        ''' <summary>
        ''' 項目番号
        ''' </summary>
        ''' <remarks></remarks>
        Private ItemNumber() As Short
        
        ''' <summary>
        ''' 項目必須フラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Chkrequiredflg() As Short
        
        ''' <summary>
        ''' 項目属性
        ''' </summary>
        ''' <remarks></remarks>
        Private Attribute() As Short
        
        ''' <summary>
        ''' 項目サイズ
        ''' </summary>
        ''' <remarks></remarks>
        Private Itemsize() As Double
        
        ''' <summary>
        ''' XMLタグのルート要素
        ''' </summary>
        ''' <remarks></remarks>
        Private RootElement As XmlElement
        
        ''' <summary>
        ''' XMLタグの要素
        ''' </summary>
        ''' <remarks>受信XML各タグの要素</remarks>
        Private NodeElement As XmlElement
#End Region
    
#Region "プロパティ"
    
        ''' <summary>
        ''' 販売店コードプロパティ
        ''' </summary>
        ''' <value>販売店コード</value>
        ''' <returns>販売店コード</returns>
        ''' <remarks></remarks>
        Public Property DlrCD As String
            Get
                Return DlrCd_
            End Get
            Set(value As String)
                DlrCd_ = value
            End Set
        End Property

        ''' <summary>
        ''' シリーズコードプロパティ
        ''' </summary>
        ''' <value>シリーズコード</value>
        ''' <returns>シリーズコード</returns>
        ''' <remarks></remarks>
        Public Property SeriesCD As String
            Get
                Return SeriesCd_
            End Get
            Set(value As String)
                SeriesCd_ = value
            End Set
        End Property
        
        ''' <summary>
        ''' モデルコードプロパティ
        ''' </summary>
        ''' <value>モデルコード</value>
        ''' <returns>モデルコード</returns>
        ''' <remarks></remarks>
        Public Property ModelCD As String
            Get
                Return ModelCd_
            End Get
            Set(value As String)
                ModelCd_ = value
            End Set
        End Property
        
        ''' <summary>
        ''' 送信日時プロパティ
        ''' </summary>
        ''' <value>送信日時</value>
        ''' <returns>送信日時</returns>
        ''' <remarks></remarks>
        Public Property TransmissionDate As Date
            Get
                Return transmissionDate_
            End Get
            Set(value As Date)
                transmissionDate_ = value
            End Set
        End Property
                
#End Region
       
#Region "コンストラクタ"
    
        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>初期化処理</remarks>
        Public Sub New()
            Me.ResultId = 0
        End Sub
    
#End Region
    
#Region "オススメ情報取得Webサービス"
        ''' <summary>
        ''' オススメ情報取得Webサービス
        ''' </summary>
        ''' <param name="xsData">Request XML</param>
        ''' <returns>Response XML</returns>
        ''' <remarks></remarks>
        <WebMethod()> _
        Public Function GetRecommendedOption(ByVal xsData As String) As Response
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[Request XML :{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, xsData),
                                      True)
            ' ======================== ログ出力 終了 ========================
        
            'Response格納オブジェクト作成
            ResponseObject = New Response
            ' ヘルプ依頼画面のデータセット生成
            Using ds As IC3070401DataSet = New IC3070401DataSet
                Try
                    
                    'Inputメッセージ受信日時取得
                    Dim resReceptionData As String = DateTimeFunc.Now.ToString(FormatDatetime, CultureInfo.InvariantCulture)
            
                    'Headオブジェクト格納
                    ResponseObject.Head.MessageId = MESSAGEID_CONST                         'メッセージID
                    ResponseObject.Head.ReceptionDate = resReceptionData                    '受信日付
                
                    ' 受信XMLをデータ格納用クラスにセット
                    Me.SetData(xsData)
                    
                    'オススメ情報取得処理
                    Dim IC3070401BusinessLogic As New IC3070401BusinessLogic

                    Try
                        Dim IC3070401DataSet As IC3070401DataSet = IC3070401BusinessLogic.GetRecommended(Me.DlrCD,
                                                                                                         Me.SeriesCD,
                                                                                                         Me.ModelCD)
                        '終了コード取得
                        Me.ResultId = IC3070401BusinessLogic.ResultId
                        'Responseクラスへの格納処理
                        Me.SetRecommended(IC3070401DataSet)

                    Catch
                        Me.ResultId = ErrCodeSys
                        Throw
                    Finally
                        IC3070401BusinessLogic = Nothing
                    End Try
                                                                                          
                    'Commonオブジェクト格納
                    ResponseObject.Detail.Common.ResultId = CType(Me.ResultId, String)          '終了コード

                    If Me.ResultId.Equals(0) Then
                        ResponseObject.Detail.Common.ResultMessage = MESSAGE_SUCCESS_CONST      'メッセージ                                                        
                    Else
                        ResponseObject.Detail.Common.ResultMessage = MESSAGE_FAILURE_CONST      'メッセージ                                                        
                    End If
                
                Catch ex As Exception
                    'Commonオブジェクト格納
                    ResponseObject.Detail.Common.ResultId = CType(Me.ResultId, String)          '終了コード
                    ResponseObject.Detail.Common.ResultMessage = MESSAGE_FAILURE_CONST          'メッセージ

                    ' ======================== ログ出力 開始 ========================
                    Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                               "ProgramID:[{0}], MessageID:[{1}]",
                                               IC3070401TableAdapter.FunctionId, Me.ResultId.ToString(CultureInfo.InvariantCulture)),
                                               ex)
                    ' ======================== ログ出力 開始 ========================
            
                Finally
                    'Outputメッセージ送信日時取得
                    Dim resTransmissionDate As String = DateTimeFunc.Now.ToString(FormatDatetime, CultureInfo.InvariantCulture)
                    
                    'Headオブジェクト格納
                    ResponseObject.Head.TransmissionDate = resTransmissionDate        '送信日付
                
                End Try
                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          "[{0}]_[{1}]_End, MessageID:[{2}]",
                                          IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, Me.ResultId.ToString(CultureInfo.InvariantCulture)),
                                          True)
                ' ======================== ログ出力 終了 ========================
            
                Return ResponseObject
            End Using
        End Function
    
#End Region
    
#Region "Responseクラス格納処理"
        ''' <summary>
        ''' オススメ情報取得結果オブジェクトへの格納処理
        ''' </summary>
        ''' <param name="dsRecommended">取得結果データセット</param>
        ''' <remarks></remarks>
        Private Sub SetRecommended(ByVal dsRecommended As IC3070401DataSet)
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[IC3070401DataSet:{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dsRecommended)),
                                      True)
            ' ======================== ログ出力 終了 ========================
            Try
                '販売店コード(検索条件)
                ResponseObject.Detail.Recommended.DlrCD = Me.DlrCD
                'シリーズコード(検索条件)
                ResponseObject.Detail.Recommended.SeriesCD = Me.SeriesCD
                'モデルズ情報設定
                ResponseObject.Detail.Recommended.Models = Me.SetModelsInfo(dsRecommended.Tables("IC3070401Purchaserate"))
            Catch ex As Exception
                '例外発生時
                Me.ResultId = ErrCodeSys
                Throw
                
            End Try
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
                                      True)
            ' ======================== ログ出力 終了 ========================
                    
        End Sub
          
        ''' <summary>
        ''' モデルズ情報格納処理
        ''' </summary>
        ''' <param name="dtPurchaserate">オススメ情報データテーブル</param>
        ''' <returns>モデルズ情報結果格納オブジェクト</returns>
        ''' <remarks></remarks>
        Private Function SetModelsInfo(ByVal dtPurchaserate As IC3070401DataSet.IC3070401PurchaserateDataTable) As Root_Models
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[IC3070401DataSet:{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dtPurchaserate)),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
            Dim models = New Root_Models
            'モデルズ情報の取得
            models.SetModel(Me.SetModelInfo(dtPurchaserate))
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, Result Root_Models :[{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, models),
                                      True)
            ' ======================== ログ出力 終了 ========================

            Return models
        
        End Function
        ''' <summary>
        ''' モデル情報格納処理
        ''' </summary>
        ''' <param name="dtPurchaserate">オススメ情報データテーブル</param>
        ''' <returns>モデル情報結果格納オブジェクト</returns>
        ''' <remarks></remarks>
        Private Function SetModelInfo(ByVal dtPurchaserate As IC3070401DataSet.IC3070401PurchaserateDataTable) As Collection(Of Root_Model)
           
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[IC3070401DataSet:{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dtPurchaserate)),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
            Dim arrModel = New Collection(Of Root_Model)
            Dim tempModelCd = String.Empty
            Dim i As Integer = 0
            If (dtPurchaserate.Count > 0) Then
                For Each dr As IC3070401DataSet.IC3070401PurchaserateRow In dtPurchaserate.Rows
                    If (Not dr.MODELCD.Equals(tempModelCd)) Then
                        Dim modelInfo As New Root_Model
                        arrModel.Add(modelInfo)
                        'モデルコード(検索条件)
                        arrModel(i).ModelCD = CType(dr.MODELCD, String)
                        tempModelCd = dr.MODELCD
                        arrModel(i).Options.SetOptions(Me.SetOptionsInfo(dtPurchaserate, tempModelCd))
                        i = i + 1
                    End If
                Next
            End If
                
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, Result Root_Model :[{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, arrModel),
                                      True)
                                          
            ' ======================== ログ出力 終了 ========================
            
            Return arrModel
        
        End Function
        
        ''' <summary>
        ''' オプション情報格納処理
        ''' </summary>
        ''' <param name="dtPurchaserate">オススメ情報データテーブル</param>
        ''' <returns>オプション情報結果格納オブジェクト</returns>
        ''' <remarks></remarks>
        Private Function SetOptionsInfo(ByVal dtPurchaserate As IC3070401DataSet.IC3070401PurchaserateDataTable, ByVal modelCd As String) As Collection(Of Root_Option)
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[IC3070401DataSet:{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dtPurchaserate)),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
            Dim arrOptionInfo = New Collection(Of Root_Option)
            Dim i As Integer = 0
            
            For Each dr As IC3070401DataSet.IC3070401PurchaserateRow In dtPurchaserate.Rows

                If (dr.MODELCD.Equals(modelCd)) Then
                    Dim OptionsInfo As New Root_Option
                    arrOptionInfo.Add(OptionsInfo)
                    'オプション区分
                    arrOptionInfo(i).OptionPart = CType(dr.OPTIONPART, String)
                    'オプションコード
                    arrOptionInfo(i).OptionCode = CType(dr.OPTIONCODE, String)
                    '購入率
                    arrOptionInfo(i).Rate = CType(dr.RATE, String)
                    i = i + 1
                End If
            Next
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, Result Root_Option :[{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, arrOptionInfo),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
            Return arrOptionInfo
        
        End Function

#End Region
         
#Region "Request XMLの格納処理"
        ''' <summary>
        ''' XMLタグの情報をデータ格納クラスにセットします。
        ''' </summary>
        ''' <param name="xsData">受信XML</param>
        ''' <remarks></remarks>
        Private Sub SetData(xsData As String)
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[xsData:{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, xsData),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
            ' XmlDocument生成
            Dim xdoc As New XmlDocument
            
            Try
                ' XML読み込み
                xdoc.LoadXml(xsData)
            Catch ex As Exception
                'XML読み込み失敗時は終了コードをセットして処理終了
                Me.ResultId = ErrCodeXmlDoc
                Throw
            End Try

            ' メンバ変数を設定
            Me.RootElement = xdoc.DocumentElement                                ' ルート要素
            Dim transmissionDate As String = Me.TransmissionDate                 ' 送信日時
            
            ' Header情報格納
            Me.InitHead()
            Me.SetHead()

            ' 販売店コード、シリーズコード、モデルコードを格納
            Me.InitConditions()
            Me.SetConditions()
            xdoc = Nothing
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
        End Sub
#End Region
                
#Region "初期化"
        ''' <summary>
        ''' Headerタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitHead()
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
                                      True)
            ' ======================== ログ出力 終了 ========================    
        
            ' 項目名称を設定
            Me.Itemname = {"TransmissionDate"}
            
            ' 項目Noを設定
            Me.ItemNumber = {TransmissionDate_No}
            
            ' 必須必須フラグを設定
            Me.Chkrequiredflg = {CheckRequired}
            
            ' 項目属性を設定
            Me.Attribute = {AttributeDatetime}
            
            ' 項目サイズを設定
            Me.Itemsize = {0}
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
        End Sub
        
        ''' <summary>
        ''' Conditionsタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitConditions()
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
                                      True)
            ' ======================== ログ出力 終了 ========================   
        
            ' 項目名称を設定
            Me.Itemname = {"DlrCd", "SeriesCd", "ModelCd"}
            
            ' 項目Noを設定
            Me.ItemNumber = {DlrCd_No, SeriesCd_No, ModelCd_No}
            
            ' 必須必須フラグを設定
            Me.Chkrequiredflg = {CheckRequired, CheckRequired, CheckNoRequired}
            
            ' 項目属性を設定
            Me.Attribute = {AttributeLegth, AttributeLegth, AttributeLegth}
            
            ' 項目サイズを設定
            Me.Itemsize = {5, 64, 32}
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
                                      True)
                                          
            ' ======================== ログ出力 終了 ========================
            
        End Sub

#End Region
        
#Region "プロパティーセット"
        ''' <summary>
        ''' Headerタグ情報のプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetHead()
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
                                      True)
            ' ======================== ログ出力 終了 ========================   
            
            Dim itemNo As Short = 0             ' タグ番号
            Dim nodeList As XmlNodeList         ' XMLノードリスト
            Dim nodeDocument As XmlDocument     ' XML要素
            
            Try
                ' XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagHead)
                
                ' XML要素を設定
                nodeDocument = New XmlDocument
                nodeDocument.LoadXml(nodeList.ItemOf(0).OuterXml)
                Me.NodeElement = nodeDocument.DocumentElement
            
                ' TransmissionDateタグのNodeListを取得する
                Me.TransmissionDate = Me.GetElementValue(0)

            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeItType + Me.ItemNumber(itemNo)
                End If
                Throw
            Finally
                nodeDocument = Nothing
                Me.NodeElement = Nothing
            End Try
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, Result ResultId :[{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, Me.ResultId.ToString(CultureInfo.InvariantCulture)),
                                      True)
            ' ======================== ログ出力 終了 ========================

        End Sub
        
        ''' <summary>
        ''' 販売店コードのプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetConditions()
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                          "[{0}]_[{1}]_Start",
                          IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
                          True)
            ' ======================== ログ出力 終了 ========================   
            
            Dim nodeList As XmlNodeList         ' XMLノードリスト
            Dim nodeDocument As XmlDocument     ' XML要素
            Dim itemNo As Integer = 0
            Try
                ' XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagConditions)
                
                ' XML要素を設定
                nodeDocument = New XmlDocument
                nodeDocument.LoadXml(nodeList.ItemOf(0).OuterXml)
                Me.NodeElement = nodeDocument.DocumentElement
                    
                ' 販売店コードタグのNodeListを取得する                               
                Me.DlrCD = Me.GetElementValue(0)
                ' シリーズコードタグのNodeListを取得する                               
                Me.SeriesCD = Me.GetElementValue(1)
                ' モデルコードタグのNodeListを取得する                               
                Me.ModelCD = Me.GetElementValue(2)
                
            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeItType + Me.ItemNumber(itemNo)
                End If
                Throw
            Finally
                nodeDocument = Nothing
                Me.NodeElement = Nothing
            End Try
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, Result ResultId :[{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, Me.ResultId.ToString(CultureInfo.InvariantCulture)),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
        End Sub
        
#End Region
 
#Region "XML内のデータ取得"
        ''' <summary>
        ''' XML内のデータを取得します。
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <returns>XMLから取り出した値</returns>
        ''' <remarks>
        ''' XMLからデータを取り出し、必須／属性／サイズチェックを実施します。
        ''' </remarks>
        Private Function GetElementValue(ByVal no As Short) As Object
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[no:{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, no),
                                      True)
            ' ======================== ログ出力 終了 ========================  
            
            ' 返却するオブジェクト
            Dim valueObj As Object = Nothing

            Try
                '指定タグのNodeListを取得する
                Dim node As XmlNodeList = Me.NodeElement.GetElementsByTagName(Me.Itemname(no))

                '指定したタグの存在有無により値をSet
                Dim valueString As String = String.Empty
                If node.Count > 0 Then
                    '指定したタグが存在したのでInnerTextプロパティで値を取得する
                    valueString = RTrim(node.Item(0).InnerText)
                Else
                    Me.ResultId = ErrCodeItMust + Me.ItemNumber(no)
                    Throw New ArgumentException("", Me.Itemname(no))
                End If

                ' 禁則文字チェック
                If Validation.IsContainTag(valueString) Then
                    Me.ResultId = ErrCodeItValue + Me.ItemNumber(no)
                    Throw New ArgumentException("", Me.Itemname(no))
                End If
               
                '文字列格納
                valueObj = valueString

                ' 必須項目チェック
                If CheckRequired = Chkrequiredflg(no) Then
                    If valueString.Length = 0 Then
                        Me.ResultId = ErrCodeItMust + Me.ItemNumber(no)
                        Throw New ArgumentException("", Me.Itemname(no))
                    End If
                End If
                
                ' 属性別のチェック
                Select Case Attribute(no)
                    
                    Case AttributeByte
                        ' 属性：Byteチェック                        
                        If Not Validation.IsCorrectByte(valueString, Me.Itemsize(no)) Then
                            Me.ResultId = ErrCodeItSize + Me.ItemNumber(no)
                            Throw New ArgumentException("", Me.Itemname(no))
                        End If
                        
                    Case AttributeLegth
                        ' 属性：文字数チェック
                        If (Not String.IsNullOrEmpty(valueString)) Then
                            If Not Validation.IsCorrectDigit(valueString, Me.Itemsize(no)) Then
                                Me.ResultId = ErrCodeItSize + Me.ItemNumber(no)
                                Throw New ArgumentException("", Me.Itemname(no))
                            End If
                        End If
                        
                    Case AttributeNum
                        ' 属性：Numericチェック
                        
                        ' 空の場合はDBNull値をセット
                        If valueString = "" Then
                            valueObj = Convert.DBNull
                        Else
                            ' 半角数字か
                            If Not Validation.IsHankakuNumber(valueString) Then
                                Me.ResultId = ErrCodeItType + Me.ItemNumber(no)
                                Throw New ArgumentException("", Me.Itemname(no))
                            End If
                            
                            '文字数チェック
                            If Not Validation.IsCorrectDigit(valueString, Me.Itemsize(no)) Then
                                Me.ResultId = ErrCodeItSize + Me.ItemNumber(no)
                                Throw New ArgumentException("", Me.Itemname(no))
                            End If
                        End If
                    Case AttributeDate
                        ' 属性：Dateチェック
                        
                        ' 空の場合はDBNull値をセット
                        If valueString = "" Then
                            valueObj = Convert.DBNull
                        Else
                            ' 指定されたフォーマットのDate日付か
                            ' 指定されたフォーマットの日付書式か
                            valueObj = ConvertDateTime(valueString, FormatDate, ErrCodeItType + Me.ItemNumber(no))
                        End If
                                              
                    Case AttributeDatetime
                        ' 属性：DateTimeチェック
                    
                        ' 空の場合はDBNull値をセット
                        If valueString = "" Then
                            valueObj = Convert.DBNull
                        Else
                            ' 指定されたフォーマットのDateTimeか
                            ' 指定されたフォーマットの日付時刻書式か
                            valueObj = ConvertDateTime(valueString, FormatDatetime, ErrCodeItType + Me.ItemNumber(no))
                        End If
                        
                    Case Else
                        ' 属性：不明な属性
                        ' 属性：不明な属性
                        Me.ResultId = ErrCodeSys
                        Throw New ArgumentOutOfRangeException(Me.Itemname(no), valueObj, "Invalid Attribute kind")
                End Select
                
            Finally
            End Try
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, Result valueObj :[{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, valueObj),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
            ' 結果を返却
            Return valueObj
        End Function
        
        ''' <summary>
        ''' 日付の書式に合わせて変換を行う。
        ''' </summary>
        ''' <param name="valueString">XMLの取り出し値（Check String）</param>
        ''' <param name="FormatDate">日付/時刻のフォーマット書式</param>
        ''' <param name="ErrNumber">エラーコード</param>
        ''' <returns>XMLから取り出した値</returns>
        ''' <remarks></remarks>
        Private Function ConvertDateTime(ByVal valueString As String, ByVal formatDate As String, ByVal errNumber As Short) As Object
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[valueString:{2}],[formatDate:{3}],[errNumber:{4}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, valueString, formatDate, errNumber),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
            Try
                Return DateTime.ParseExact(valueString, formatDate, Nothing)
                
            Catch ex As Exception
                Me.ResultId = errNumber
                Throw
            End Try
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, Result ResultId :[{2}]",
                                      IC3070401TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, Me.ResultId.ToString(CultureInfo.InvariantCulture)),
                                      True)
            ' ======================== ログ出力 終了 ========================    
            
        End Function
#End Region

        ''' -----------------------------------------------------
        ''' <summary>
        ''' Responseクラス(応答用XMLのI/Fクラス)
        ''' </summary>
        ''' <remarks></remarks>
        ''' -----------------------------------------------------
        <System.Xml.Serialization.XmlRoot("Response", Namespace:="http://tempuri.org/Response.xsd")> _
        Public Class Response
    
            Private head_ As Root_Head
            Private detail_ As Root_Detail

            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                head_ = New Root_Head
                detail_ = New Root_Detail
            End Sub
    
            ''' <summary>
            ''' デストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Dispose()

                If head_ IsNot Nothing Then
                    head_ = Nothing
                End If

                If detail_ IsNot Nothing Then
                    detail_.Dispose()
                    detail_ = Nothing
                End If

            End Sub
    
            ''' <summary>
            ''' Headクラスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>Headクラスオブジェクト</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Head", IsNullable:=False)> _
            Public Property Head As Root_Head
                Get
                    Return head_
                End Get
                Set(value As Root_Head)
                    head_ = value
                End Set
            End Property

            ''' <summary>
            ''' Detailクラスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>Detailクラスオブジェクト</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Detail", IsNullable:=False)> _
            Public Property Detail As Root_Detail
                Get
                    Return detail_
                End Get
                Set(value As Root_Detail)
                    detail_ = value
                End Set
            End Property
    
        End Class

        '''-----------------------------------------------------
        ''' <summary>
        ''' Root_Headクラス
        ''' </summary>
        ''' <remarks></remarks>
        '''-----------------------------------------------------
        Public Class Root_Head

            Private messageId_ As String
            Private receptionDate_ As String
            Private transmissionDate_ As String

            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                messageId_ = String.Empty
                receptionDate_ = String.Empty
                transmissionDate_ = String.Empty
            End Sub
    
            ''' <summary>
            ''' メッセージIDプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>メッセージID</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="MessageID", IsNullable:=False)> _
            Public Property MessageId As String
                Get
                    Return messageId_
                End Get
                Set(value As String)
                    messageId_ = value
                End Set
            End Property

            ''' <summary>
            ''' 受信日付プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>受信日付</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ReceptionDate", IsNullable:=False)> _
            Public Property ReceptionDate As String
                Get
                    Return receptionDate_
                End Get
                Set(value As String)
                    receptionDate_ = value
                End Set
            End Property

            ''' <summary>
            ''' 送信日付プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>送信日付</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="TransmissionDate", IsNullable:=False)> _
            Public Property TransmissionDate As String
                Get
                    Return transmissionDate_
                End Get
                Set(value As String)
                    transmissionDate_ = value
                End Set
            End Property
            
        End Class

        '''-----------------------------------------------------
        ''' <summary>
        ''' Root_Detailクラス
        ''' </summary>
        ''' <remarks></remarks>
        '''-----------------------------------------------------
        Public Class Root_Detail

            Private common_ As Root_Commn
            Private Recommended_ As Root_Recommended

            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                common_ = New Root_Commn
                Recommended_ = New Root_Recommended
            End Sub
    
            ''' <summary>
            ''' デストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Dispose()
        
                If common_ IsNot Nothing Then
                    common_ = Nothing
                End If
        
                If Recommended_ IsNot Nothing Then
                    Recommended_.Dispose()
                    Recommended_ = Nothing
                End If
            End Sub
            
    
            ''' <summary>
            ''' Commonクラスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>Commonクラスオブジェクト</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Common", IsNullable:=False)> _
            Public Property Common() As Root_Commn
                Get
                    Return common_
                End Get
                Set(value As Root_Commn)
                    common_ = value
                End Set
            End Property

            ''' <summary>
            ''' EstimationInfoクラスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>EstimationInfoクラスオブジェクト</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Recommended", IsNullable:=False)> _
            Public Property Recommended() As Root_Recommended
                Get
                    Return Recommended_
                End Get
                Set(value As Root_Recommended)
                    Recommended_ = value
                End Set
            End Property
            
        End Class

        '''-----------------------------------------------------
        ''' <summary>
        ''' Root_Commnクラス
        ''' </summary>
        ''' <remarks></remarks>
        '''-----------------------------------------------------
        Public Class Root_Commn

            Private resultId_ As String
            Private message_ As String

            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                resultId_ = String.Empty
                message_ = String.Empty
            End Sub
    
            ''' <summary>
            ''' 終了コードプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>終了コード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ResultId", IsNullable:=False)> _
            Public Property ResultId As String
                Get
                    Return resultId_
                End Get
                Set(value As String)
                    resultId_ = value
                End Set
            End Property

            ''' <summary>
            ''' メッセージプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>メッセージ</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Message", IsNullable:=False)> _
            Public Property ResultMessage As String
                Get
                    Return message_
                End Get
                Set(value As String)
                    message_ = value
                End Set
            End Property
            
        End Class

        '''-----------------------------------------------------
        ''' <summary>
        ''' EstimationInfoクラス
        ''' </summary>
        ''' <remarks></remarks>
        '''-----------------------------------------------------
        Public Class Root_Recommended
            Private dlrCD_ As String                '販売店コード
            Private seriesCD_ As String             'シリーズコード
            Private models_ As Root_Models          'モデルズ
    
            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                dlrCD_ = String.Empty               '販売店コード
                seriesCD_ = String.Empty            'シリーズコード
                models_ = New Root_Models           'モデルズ
            End Sub

            ''' <summary>
            ''' デストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Dispose()
        
                If models_ IsNot Nothing Then
                    models_ = Nothing
                End If
        
            End Sub
    
            ''' <summary>
            ''' 販売店コードプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>販売店コード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="DlrCd", IsNullable:=False)> _
            Public Property DlrCD As String
                Get
                    Return dlrCD_
                End Get
                Set(value As String)
                    dlrCD_ = value
                End Set
            End Property


            ''' <summary>
            ''' シリーズコードプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>シリーズコード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="SeriesCd", IsNullable:=False)> _
            Public Property SeriesCD As String
                Get
                    Return seriesCD_
                End Get
                Set(value As String)
                    seriesCD_ = value
                End Set
            End Property
            
            ''' <summary>
            ''' Modelsクラスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>Familiesオブジェクト</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Models", IsNullable:=False)> _
            Public Property Models() As Root_Models
                Get
                    Return models_
                End Get
                Set(value As Root_Models)
                    models_ = value
                End Set
            End Property
               
        End Class

        '''-----------------------------------------------------
        ''' <summary>
        ''' Modelsクラス
        ''' </summary>
        ''' <remarks></remarks>
        '''-----------------------------------------------------
        Public Class Root_Models

            Private model_ As Collection(Of Root_Model)

            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
            End Sub
            
            ''' <summary>
            ''' デストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Dispose()
        
                If model_ IsNot Nothing Then
                    model_ = Nothing
                End If
        
            End Sub
            
            ''' <summary>
            ''' Modelsクラスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>Modelオブジェクト</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Model", IsNullable:=False)> _
            Public ReadOnly Property Models() As Collection(Of Root_Model)
                Get
                    Return model_
                End Get
            End Property
            
            ''' <summary>
            ''' Modelsオブジェクト値格納処理
            ''' </summary>
            ''' <param name="value">Modelオブジェクト</param>
            ''' <remarks></remarks>
            Public Sub SetModel(ByVal value As Collection(Of Root_Model))
                model_ = value
            End Sub

        End Class
        
        '''-----------------------------------------------------
        ''' <summary>
        ''' モデルクラス
        ''' </summary>
        ''' <remarks></remarks>
        '''-----------------------------------------------------
        Public Class Root_Model
            
            Private modelCD_ As String       'モデルコード
            Private options_ As Root_Options              'オプションズ

            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                options_ = New Root_Options
            End Sub
            
            ''' <summary>
            ''' デストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Dispose()
        
                If options_ IsNot Nothing Then
                    options_ = Nothing
                End If
        
            End Sub
    
            ''' <summary>
            ''' モデルコードプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>モデルコード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ModelCd", IsNullable:=True)> _
            Public Property ModelCD As String
                Get
                    Return ModelCd_
                End Get
                Set(value As String)
                    ModelCd_ = value
                End Set
            End Property
            
            ''' <summary>
            ''' Optionsクラスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>Optionsクラスオブジェクト</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Options", IsNullable:=True)> _
            Public Property Options() As Root_Options
                Get
                    Return options_
                End Get
                Set(value As Root_Options)
                    options_ = value
                End Set
            End Property
            
        End Class
        
        '''-----------------------------------------------------
        ''' <summary>
        ''' オプションズクラス
        ''' </summary>
        ''' <remarks></remarks>
        '''-----------------------------------------------------
        Public Class Root_Options
            
            Private option_ As Collection(Of Root_Option)   'オプションズ

            ''' <summary>
            ''' デストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Dispose()
        
                If Options IsNot Nothing Then
                    option_ = Nothing
                End If
        
            End Sub
    
            ''' <summary>
            ''' オプションクラスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>オプションオブジェクト</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Option", IsNullable:=True)> _
            Public ReadOnly Property Options() As Collection(Of Root_Option)
                Get
                    Return option_
                End Get
            End Property
 
            ''' <summary>
            ''' オプションオブジェクト値格納処理
            ''' </summary>
            ''' <param name="value">オプションオブジェクト</param>
            ''' <remarks></remarks>
            Public Sub SetOptions(ByVal value As Collection(Of Root_Option))
                option_ = value
            End Sub
            
        End Class
        
        '''-----------------------------------------------------
        ''' <summary>
        ''' オプションクラス
        ''' </summary>
        ''' <remarks></remarks>
        '''-----------------------------------------------------
        Public Class Root_Option
            
            Private optionPart_ As String       'オプション区分
            Private optionCode_ As String       'オプションコード
            Private rate_ As String             '購入率

            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                optionPart_ = String.Empty      'オプション区分
                optionCode_ = String.Empty      'オプションコード
                rate_ = String.Empty            '購入率
            End Sub
            
            ''' <summary>
            ''' オプション区分プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>オプション区分</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="OptionPart", IsNullable:=False)> _
            Public Property OptionPart As String
                Get
                    Return optionPart_
                End Get
                Set(value As String)
                    optionPart_ = value
                End Set
            End Property
            
            ''' <summary>
            ''' オプションコードプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>オプションコード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="OptionCode", IsNullable:=False)> _
            Public Property OptionCode As String
                Get
                    Return optionCode_
                End Get
                Set(value As String)
                    optionCode_ = value
                End Set
            End Property
            
            ''' <summary>
            ''' 購入率プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>購入率</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Rate", IsNullable:=False)> _
            Public Property Rate As String
                Get
                    Return rate_
                End Get
                Set(value As String)
                    rate_ = value
                End Set
            End Property
            
        End Class
    
    End Class
    
End Namespace

