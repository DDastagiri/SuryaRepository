<%@ WebService Language="VB" Class="Toyota.eCRB.Estimate.Quotation.WebService.IC3070201" %>

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Microsoft.VisualBasic.Collection
Imports System.Collections.ObjectModel

Namespace Toyota.eCRB.Estimate.Quotation.WebService

    ' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
    ' <System.Web.Script.Services.ScriptService()> _
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    Public Class IC3070201
        Inherits System.Web.Services.WebService
    
#Region "定数"
    
        ''' <summary>
        ''' メッセージID
        ''' </summary>
        ''' <remarks>メッセージ識別コード(IC3070201) 見積情報取得</remarks>
        Private Const MESSAGEID_CONST As String = "IC3070201"
    
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
        ''' Commonタグ名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagCommon As String = "Common"
        
        ''' <summary>
        ''' EstimationInfoタグ名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstimationInfo As String = "EstimationInfo"
                
        ''' <summary>
        ''' 送信日付
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TransmissionDate_No As Short = 1

        ''' <summary>
        ''' 実行モード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const Mode_No As Short = 11
    
        ''' <summary>
        ''' 見積管理ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const EstimateId_No As Short = 21
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
        ''' 見積管理ID
        ''' </summary>
        ''' <remarks></remarks>
        Private estimateId_ As Long
        
        ''' <summary>
        ''' 実行モード
        ''' </summary>
        ''' <remarks></remarks>
        Private mode_ As Integer
        
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
        ''' 見積管理IDプロパティ
        ''' </summary>
        ''' <value>見積管理ID</value>
        ''' <returns>見積管理ID</returns>
        ''' <remarks></remarks>
        Public Property EstimateId As Long
            Get
                Return estimateId_
            End Get
            Set(value As Long)
                estimateId_ = value
            End Set
        End Property

        ''' <summary>
        ''' 実行モードプロパティ
        ''' </summary>
        ''' <value>実行モード</value>
        ''' <returns>実行モード</returns>
        ''' <remarks></remarks>
        Public Property Mode As Integer
            Get
                Return mode_
            End Get
            Set(value As Integer)
                If (value.Equals(0) Or value.Equals(1)) Then
                    mode_ = value
                Else
                    Me.ResultId = ErrCodeItValue + Me.ItemNumber(0)
                    Throw New ArgumentException("", Me.Itemname(0))
                End If
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
    
#Region "見積情報取得Webサービス"
        ''' <summary>
        ''' 見積情報取得Webサービス
        ''' </summary>
        ''' <param name="xsData">Request XML</param>
        ''' <returns>Response XML</returns>
        ''' <remarks></remarks>
        <WebMethod()> _
        Public Function GetEstimation(ByVal xsData As String) As Response
        
            'Response格納オブジェクト作成
            ResponseObject = New Response

            Try
            
                'Inputメッセージ受信日時取得
                Dim resReceptionData As String = DateTimeFunc.Now.ToString(FormatDatetime, CultureInfo.InvariantCulture)
            
                'Headオブジェクト格納
                ResponseObject.Head.MessageId = MESSAGEID_CONST                         'メッセージID
                ResponseObject.Head.ReceptionDate = resReceptionData                    '受信日付

                ' 受信XMLをログ出力
                Logger.Info("Request XML : " & xsData, True)
                
                ' 受信XMLをデータ格納用クラスにセット
                Me.SetData(xsData)
        
                '見積情報取得処理
                Dim IC3070201BusinessLogic As New IC3070201BusinessLogic
                    
                Try
                    Dim IC3070201DataSet As IC3070201DataSet = IC3070201BusinessLogic.GetEstimationInfo(Me.EstimateId, Me.Mode)
                    
                    '終了コード取得
                    Me.ResultId = IC3070201BusinessLogic.ResultId
                    
                    If Me.ResultId.Equals(0) Then
                        'Responseクラスへの格納処理
                        Me.SetEstimationInfo(IC3070201DataSet)
                    End If

                Catch
                    Me.ResultId = IC3070201BusinessLogic.ResultId
                    Throw
                Finally
                    IC3070201BusinessLogic = Nothing
                End Try
                                                                                          
                'Commonオブジェクト格納
                ResponseObject.Detail.Common.ResultId = CType(Me.ResultId, String)          '終了コード

                If Me.ResultId.Equals(0) Then
                    ResponseObject.Detail.Common.ResultMessage = MESSAGE_SUCCESS_CONST      'メッセージ                                                        
                Else
                    ResponseObject.Detail.Common.ResultMessage = MESSAGE_FAILURE_CONST      'メッセージ                                                        
                End If
                
            Catch ex As Exception
                If Me.ResultId.Equals(0) Then
                    Me.ResultId = ErrCodeSys
                End If
            
                'Commonオブジェクト格納
                ResponseObject.Detail.Common.ResultId = CType(Me.ResultId, String)          '終了コード
                ResponseObject.Detail.Common.ResultMessage = MESSAGE_FAILURE_CONST          'メッセージ

                'エラーログ出力
                Logger.Error("ResultId : " & Me.ResultId.ToString(CultureInfo.InvariantCulture), ex)
            
            Finally
                'Outputメッセージ送信日時取得
                Dim resTransmissionDate As String = DateTimeFunc.Now.ToString(FormatDatetime, CultureInfo.InvariantCulture)
                    
                'Headオブジェクト格納
                ResponseObject.Head.TransmissionDate = resTransmissionDate        '送信日付
                
                '終了コードログ出力
                Logger.Info("ResultId[" & Me.TransmissionDate.ToString(FormatDatetime, CultureInfo.InvariantCulture) & _
                            "] : " & Me.ResultId.ToString(CultureInfo.InvariantCulture), True)
                
            End Try
            
            Return ResponseObject
                        
        End Function
    
#End Region
    
#Region "Responseクラス格納処理"
        ''' <summary>
        ''' 見積情報取得結果オブジェクトへの格納処理
        ''' </summary>
        ''' <param name="dsEstimationInfo">取得結果データセット</param>
        ''' <remarks></remarks>
        Private Sub SetEstimationInfo(ByVal dsEstimationInfo As IC3070201DataSet)

            Try
                '見積管理ID
                ResponseObject.Detail.EstimationInfo.EstimateId = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("ESTIMATEID").ToString()
                
                '販売店コード
                ResponseObject.Detail.EstimationInfo.DlrCD = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("DLRCD")
                
                '店舗コード
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("STRCD")) Then
                    ResponseObject.Detail.EstimationInfo.StrCD = _
                        dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("STRCD")
                End If
                
                'Follow-up BOX内連番
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("FLLWUPBOX_SEQNO")) Then
                    ResponseObject.Detail.EstimationInfo.FllwupBox_SeqNo = _
                        dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("FLLWUPBOX_SEQNO").ToString()
                End If
                                
                '契約店舗コード
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CNT_STRCD")) Then
                    ResponseObject.Detail.EstimationInfo.Cnt_StrCD = _
                        dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CNT_STRCD")
                End If
                
                '契約スタッフ
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CNT_STAFF")) Then
                    ResponseObject.Detail.EstimationInfo.Cnt_Staff = _
                        dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CNT_STAFF")
                End If

                '顧客種別
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CSTKIND")) Then
                    ResponseObject.Detail.EstimationInfo.CstKind = _
                        dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CSTKIND")
                End If

                '顧客分類
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CUSTOMERCLASS")) Then
                    ResponseObject.Detail.EstimationInfo.CustomerClass = _
                        dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CUSTOMERCLASS")
                End If

                '活動先顧客コード
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CRCUSTID")) Then
                    ResponseObject.Detail.EstimationInfo.CRCustId = _
                        dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CRCUSTID")
                End If

                '基幹お客様コード
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CUSTID")) Then
                    ResponseObject.Detail.EstimationInfo.CustId = _
                        dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CUSTID")
                End If

                '納車予定日
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("DELIDATE")) Then
                    ResponseObject.Detail.EstimationInfo.DeliDate = _
                        CDate(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("DELIDATE")).ToString(FormatDatetime, CultureInfo.InvariantCulture)
                End If
                
                '値引き額
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("DISCOUNTPRICE")) Then
                    ResponseObject.Detail.EstimationInfo.DiscountPrice = _
                        dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("DISCOUNTPRICE").ToString()
                End If
                
                'メモ
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("MEMO")) Then
                    ResponseObject.Detail.EstimationInfo.Memo = _
                        dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("MEMO")
                End If

                '見積印刷日
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("ESTPRINTDATE")) Then
                    ResponseObject.Detail.EstimationInfo.EstprintDate = _
                        CDate(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("ESTPRINTDATE")).ToString(FormatDatetime, CultureInfo.InvariantCulture)
                End If

                '契約書№
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTRACTNO")) Then
                    ResponseObject.Detail.EstimationInfo.ContractNo = _
                        dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTRACTNO")
                End If

                '契約書印刷フラグ
                ResponseObject.Detail.EstimationInfo.ContPrintFlg = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTPRINTFLG")
                
                '契約状況フラグ
                ResponseObject.Detail.EstimationInfo.ContractFlg = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTRACTFLG")
                
                '契約完了日
                If Not IsDBNull(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTRACTDATE")) Then
                    ResponseObject.Detail.EstimationInfo.ContractDate = _
                        CDate(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CONTRACTDATE")).ToString(FormatDatetime, CultureInfo.InvariantCulture)
                End If

                '削除フラグ
                ResponseObject.Detail.EstimationInfo.DelFlg = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("DELFLG")

                'TCVバージョン
                ResponseObject.Detail.EstimationInfo.TcvVersion = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("TCVVERSION")
                
                'シリーズコード
                ResponseObject.Detail.EstimationInfo.SeriesCD = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("SERIESCD")

                'シリーズ名称
                ResponseObject.Detail.EstimationInfo.SeriesNM = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("SERIESNM")

                'モデルコード
                ResponseObject.Detail.EstimationInfo.ModelCD = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("MODELCD")

                'モデル名称
                ResponseObject.Detail.EstimationInfo.ModelNM = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("MODELNM")
                
                'ボディータイプ
                ResponseObject.Detail.EstimationInfo.BodyType = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("BODYTYPE")

                '駆動方式
                ResponseObject.Detail.EstimationInfo.DriveSystem = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("DRIVESYSTEM")

                '排気量
                ResponseObject.Detail.EstimationInfo.Displacement = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("DISPLACEMENT")

                'ミッションタイプ
                ResponseObject.Detail.EstimationInfo.Transmission = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("TRANSMISSION")

                'サフィックス
                ResponseObject.Detail.EstimationInfo.SuffixCD = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("SUFFIXCD")

                '外装色コード
                ResponseObject.Detail.EstimationInfo.ExtColorCD = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTCOLORCD")

                '外装色名称
                ResponseObject.Detail.EstimationInfo.ExtColor = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTCOLOR")

                '外装追加費用
                ResponseObject.Detail.EstimationInfo.ExtAmount = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTAMOUNT").ToString()

                '内装色コード
                ResponseObject.Detail.EstimationInfo.IntColorCD = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("INTCOLORCD")

                '内装色名称
                ResponseObject.Detail.EstimationInfo.IntColor = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("INTCOLOR")

                '内装追加費用
                ResponseObject.Detail.EstimationInfo.IntAmount = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("INTAMOUNT").ToString()

                '車両型号
                ResponseObject.Detail.EstimationInfo.ModelNumber = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("MODELNUMBER")

                '車両価格
                ResponseObject.Detail.EstimationInfo.BasePrice = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("BASEPRICE").ToString()

                '作成日
                ResponseObject.Detail.EstimationInfo.CreateDate = _
                    CDate(dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CREATEDATE")).ToString(FormatDatetime, CultureInfo.InvariantCulture)

                '作成ユーザアカウント
                ResponseObject.Detail.EstimationInfo.CreateAccount = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CREATEACCOUNT")

                '更新ユーザアカウント
                ResponseObject.Detail.EstimationInfo.UpdateAccount = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("UPDATEACCOUNT")

                '作成機能ID
                ResponseObject.Detail.EstimationInfo.CreateId = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("CREATEID")

                '更新機能ID
                ResponseObject.Detail.EstimationInfo.UpdateId = _
                    dsEstimationInfo.Tables("IC3070201EstimationInfo").Rows(0).Item("UPDATEID")
                    
                If dsEstimationInfo.Tables("IC3070201VclOptionInfo").Rows.Count > 0 Then

                    '見積オプション情報格納処理
                    ResponseObject.Detail.EstimationInfo.SetEstVcloptionInfo(Me.SetVclOptionInfo(dsEstimationInfo.Tables("IC3070201VclOptionInfo")))
                End If
                        
            Catch ex As Exception
                '例外発生時
                Me.ResultId = ErrCodeSys
                Throw
            End Try
                    
        End Sub
    
        ''' <summary>
        ''' 見積車両オプション情報格納処理
        ''' </summary>
        ''' <param name="dtVclOptionInfo">見積車両オプション情報データテーブル</param>
        ''' <returns>見積車両オプション情報結果格納オブジェクト</returns>
        ''' <remarks></remarks>
        Private Function SetVclOptionInfo(ByVal dtVclOptionInfo As IC3070201DataSet.IC3070201VclOptionInfoDataTable) As Collection(Of Root_EstVcloptionInfo)
        
            Dim arrEstVcloptionInfo = New Collection(Of Root_EstVcloptionInfo)

            Dim i As Integer = 0

            For Each dr As IC3070201DataSet.IC3070201VclOptionInfoRow In dtVclOptionInfo.Rows

                Dim estVcloptionInfo As New Root_EstVcloptionInfo
                arrEstVcloptionInfo.Add(estVcloptionInfo)
                '見積管理ID
                arrEstVcloptionInfo(i).EstimateId = CType(dr.ESTIMATEID, String)
                'オプション区分
                arrEstVcloptionInfo(i).OptionPart = dr.OPTIONPART
                'オプションコード
                arrEstVcloptionInfo(i).OptionCode = dr.OPTIONCODE
                'オプション名
                arrEstVcloptionInfo(i).OptionName = dr.OPTIONNAME
                '価格
                arrEstVcloptionInfo(i).Price = CType(dr.PRICE, String)
                '取付費用
                If Not dr.IsINSTALLCOSTNull Then
                    arrEstVcloptionInfo(i).InstallCost = CType(dr.INSTALLCOST, String)
                End If

                i = i + 1
            Next
                
            Return arrEstVcloptionInfo
        
        End Function

#End Region
         
#Region "Request XMLの格納処理"
        ''' <summary>
        ''' XMLタグの情報をデータ格納クラスにセットします。
        ''' </summary>
        ''' <param name="xsData">受信XML</param>
        ''' <remarks></remarks>
        Private Sub SetData(xsData As String)
        
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
            
            ' Common情報格納
            Me.InitCommon()
            Me.SetCommon()

            ' EstimationInfo情報格納
            Me.InitEstimationInfo()
            Me.SetEstimationInfo()
            
            xdoc = Nothing
        
        End Sub
#End Region
                
#Region "初期化"
        ''' <summary>
        ''' Headerタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitHead()
        
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
        End Sub

        ''' <summary>
        ''' Commonタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitCommon()
        
            ' 項目名称を設定
            Me.Itemname = {"Mode"}
            
            ' 項目Noを設定
            Me.ItemNumber = {Mode_No}
            
            ' 必須必須フラグを設定
            Me.Chkrequiredflg = {CheckRequired}
            
            ' 項目属性を設定
            Me.Attribute = {AttributeNum}
            
            ' 項目サイズを設定
            Me.Itemsize = {1}
        End Sub

        ''' <summary>
        ''' EstimationInfoタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitEstimationInfo()
        
            ' 項目名称を設定
            Me.Itemname = {"EstimateId"}
            
            ' 項目Noを設定
            Me.ItemNumber = {EstimateId_No}
            
            ' 必須必須フラグを設定
            Me.Chkrequiredflg = {CheckRequired}
            
            ' 項目属性を設定
            Me.Attribute = {AttributeNum}
            
            ' 項目サイズを設定
            Me.Itemsize = {10}

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
                Me.TransmissionDate = Me.GetElementValue(itemNo)

            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeItType + Me.ItemNumber(itemNo)
                End If
                Throw
            Finally
                nodeDocument = Nothing
                Me.NodeElement = Nothing
            End Try

        End Sub
        
        ''' <summary>
        ''' Commonタグ情報のプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetCommon()

            Dim itemNo As Short = 0             ' タグ番号
            Dim nodeList As XmlNodeList         ' XMLノードリスト
            Dim nodeDocument As XmlDocument     ' XML要素
            
            Try
                ' XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagCommon)
                
                ' XML要素を設定
                nodeDocument = New XmlDocument
                nodeDocument.LoadXml(nodeList.ItemOf(0).OuterXml)
                Me.NodeElement = nodeDocument.DocumentElement
            
                ' ModeタグのNodeListを取得する
                Me.Mode = Me.GetElementValue(itemNo)
                
            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeItType + Me.ItemNumber(itemNo)
                End If
                Throw
            Finally
                nodeDocument = Nothing
                Me.NodeElement = Nothing
            End Try
            
        End Sub

        ''' <summary>
        ''' EstimationInfoタグ情報のプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetEstimationInfo()

            Dim itemNo As Short = 0             ' タグ番号
            Dim nodeList As XmlNodeList         ' XMLノードリスト
            Dim nodeDocument As XmlDocument     ' XML要素
                        
            Try
                ' XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagEstimationInfo)
                
                ' XML要素を設定
                nodeDocument = New XmlDocument
                nodeDocument.LoadXml(nodeList.ItemOf(0).OuterXml)
                Me.NodeElement = nodeDocument.DocumentElement
                
                ' EstimateIdタグのNodeListを取得する                               
                Me.EstimateId = Me.GetElementValue(itemNo)
                
            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeItType + Me.ItemNumber(itemNo)
                End If
                Throw
            Finally
                nodeDocument = Nothing
                Me.NodeElement = Nothing
            End Try

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
                        If Not Validation.IsCorrectDigit(valueString, Me.Itemsize(no)) Then
                            Me.ResultId = ErrCodeItSize + Me.ItemNumber(no)
                            Throw New ArgumentException("", Me.Itemname(no))
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
                
            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeSys
                End If
                    
                Throw
            End Try
            
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
            
            Try
                Return DateTime.ParseExact(valueString, formatDate, Nothing)
                
            Catch ex As Exception
                Me.ResultId = errNumber
                Throw
            End Try

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
            Private estimationInfo_ As Root_EstimationInfo

            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                common_ = New Root_Commn
                estimationInfo_ = New Root_EstimationInfo
            End Sub
    
            ''' <summary>
            ''' デストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Dispose()
        
                If common_ IsNot Nothing Then
                    common_ = Nothing
                End If
        
                If estimationInfo_ IsNot Nothing Then
                    estimationInfo_.Dispose()
                    estimationInfo_ = Nothing
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
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="EstimationInfo", IsNullable:=False)> _
            Public Property EstimationInfo() As Root_EstimationInfo
                Get
                    Return estimationInfo_
                End Get
                Set(value As Root_EstimationInfo)
                    estimationInfo_ = value
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
        Public Class Root_EstimationInfo

            Private estimateId_ As String           '見積管理ID
            Private dlrCD_ As String                '販売店コード
            Private StrCD_ As String                '店舗コード
            Private fllwupBox_SeqNo_ As String      'Follow-up Box内連番
            Private cnt_StrCD_ As String            '契約店舗コード
            Private cnt_Staff_ As String            '契約スタッフ
            Private cstKind_ As String              '顧客種別
            Private customerClass_ As String        '顧客分類
            Private cRCustId_ As String             '活動先顧客コード
            Private custId_ As String               '基幹お客様コード
            Private deliDate_ As String             '納車予定日
            Private discountPrice_ As String        '値引き額
            Private memo_ As String                 'メモ
            Private estprintDate_ As String         '見積印刷日
            Private contractNo_ As String           '契約書№
            Private contPrintFlg_ As String         '契約書印刷フラグ
            Private contractFlg_ As String          '契約状況フラグ
            Private contractDate_ As String         '契約完了日
            Private delFlg_ As String               '削除フラグ
            Private tcvVersion_ As String           'TCVバージョン
    
            Private seriesCD_ As String             'シリーズコード
            Private seriesNM_ As String             'シリーズ名称
            Private modelCD_ As String              'モデルコード
            Private modelNM_ As String              'モデル名称
            Private bodyType_ As String             'ボディータイプ
            Private driveSystem_ As String          '駆動方式
            Private displacement_ As String         '排気量
            Private transmission_ As String         'ミッションタイプ
            Private suffixCD_ As String             'サフィックス
            Private extColorCD_ As String           '外装色コード
            Private extColor_ As String             '外装色名称
            Private extAmount_ As String            '外装追加費用
            Private intColorCD_ As String           '内装色コード
            Private intColor_ As String             '内装色名称
            Private intAmount_ As String            '内装追加費用
            Private modelNumber_ As String          '車両型号
            Private basePrice_ As String            '車両価格
    
            Private createDate_ As String           '作成日
            Private createAccount_ As String        '作成ユーザアカウント
            Private updateAccount_ As String        '更新ユーザアカウント
            Private createId_ As String             '作成機能ID
            Private updateId_ As String             '更新機能ID
    
            Private estVcloptionInfo_ As Collection(Of Root_EstVcloptionInfo)
    
            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                estimateId_ = String.Empty                '見積管理ID
                dlrCD_ = String.Empty                     '販売店コード
                StrCD_ = String.Empty                     '店舗コード
                fllwupBox_SeqNo_ = String.Empty           'Follow-up Box内連番
                cnt_StrCD_ = String.Empty                 '契約店舗コード
                cnt_Staff_ = String.Empty                 '契約スタッフ
                cstKind_ = String.Empty                   '顧客種別
                customerClass_ = String.Empty             '顧客分類
                cRCustId_ = String.Empty                  '活動先顧客コード
                custId_ = String.Empty                    '基幹お客様コード
                deliDate_ = String.Empty                  '納車予定日
                discountPrice_ = String.Empty             '値引き額
                memo_ = String.Empty                      'メモ
                estprintDate_ = String.Empty              '見積印刷日
                contractNo_ = String.Empty                '契約書№
                contPrintFlg_ = String.Empty              '契約書印刷フラグ
                contractFlg_ = String.Empty               '契約状況フラグ
                contractDate_ = String.Empty              '契約完了日
                delFlg_ = String.Empty                    '削除フラグ
                tcvVersion_ = String.Empty                'TCVバージョン

                seriesCD_ = String.Empty                  'シリーズコード
                seriesNM_ = String.Empty                  'シリーズ名称
                modelCD_ = String.Empty                   'モデルコード
                modelNM_ = String.Empty                   'モデル名称
                bodyType_ = String.Empty                  'ボディータイプ
                driveSystem_ = String.Empty               '駆動方式
                displacement_ = String.Empty              '排気量
                transmission_ = String.Empty              'ミッションタイプ
                suffixCD_ = String.Empty                  'サフィックス
                extColorCD_ = String.Empty                '外装色コード
                extColor_ = String.Empty                  '外装色名称
                extAmount_ = String.Empty                 '外装追加費用
                intColorCD_ = String.Empty                '内装色コード
                intColor_ = String.Empty                  '内装色名称
                intAmount_ = String.Empty                 '内装追加費用
                modelNumber_ = String.Empty               '車両型号
                basePrice_ = String.Empty                 '車両価格

                createDate_ = String.Empty                '作成日
                createAccount_ = String.Empty             '作成ユーザアカウント
                updateAccount_ = String.Empty             '更新ユーザアカウント
                createId_ = String.Empty                  '作成機能ID
                updateId_ = String.Empty                  '更新機能ID
                
            End Sub

            ''' <summary>
            ''' デストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Dispose()
        
                If estVcloptionInfo_ IsNot Nothing Then
                    estVcloptionInfo_ = Nothing
                End If
        
            End Sub
    
            ''' <summary>
            ''' 見積管理IDプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>見積管理ID</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="EstimateId", IsNullable:=False)> _
            Public Property EstimateId As String
                Get
                    Return estimateId_
                End Get
                Set(value As String)
                    estimateId_ = value
                End Set
            End Property


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
            ''' 店舗コードプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>店舗コード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="StrCd", IsNullable:=False)> _
            Public Property StrCD As String
                Get
                    Return StrCD_
                End Get
                Set(value As String)
                    StrCD_ = value
                End Set
            End Property
    
   
            ''' <summary>
            ''' Follow-up BOX内連番プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>Follow-up BOX内連番</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="FllwupBox_SeqNo", IsNullable:=False)> _
            Public Property FllwupBox_SeqNo As String
                Get
                    Return fllwupBox_SeqNo_
                End Get
                Set(value As String)
                    fllwupBox_SeqNo_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' 契約店舗コードプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>契約店舗コード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Cnt_StrCd", IsNullable:=False)> _
            Public Property Cnt_StrCD As String
                Get
                    Return cnt_StrCD_
                End Get
                Set(value As String)
                    cnt_StrCD_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' 契約スタッフプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>契約スタッフ</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Cnt_Staff", IsNullable:=False)> _
            Public Property Cnt_Staff As String
                Get
                    Return cnt_Staff_
                End Get
                Set(value As String)
                    cnt_Staff_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' 顧客種別プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>顧客種別</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="CstKind", IsNullable:=False)> _
            Public Property CstKind As String
                Get
                    Return cstKind_
                End Get
                Set(value As String)
                    cstKind_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' 顧客分類プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>顧客分類</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="CustomerClass", IsNullable:=False)> _
            Public Property CustomerClass As String
                Get
                    Return customerClass_
                End Get
                Set(value As String)
                    customerClass_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' 活動先顧客コードプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>活動先顧客コード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="CRCustId", IsNullable:=False)> _
            Public Property CRCustId As String
                Get
                    Return cRCustId_
                End Get
                Set(value As String)
                    cRCustId_ = value
                End Set
            End Property
    
        
            ''' <summary>
            ''' 基幹お客様コードプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>基幹お客様コード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="CustId", IsNullable:=False)> _
            Public Property CustId As String
                Get
                    Return custId_
                End Get
                Set(value As String)
                    custId_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' 納車予定日プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>納車予定日</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="DeliDate", IsNullable:=False)> _
            Public Property DeliDate As String
                Get
                    Return deliDate_
                End Get
                Set(value As String)
                    deliDate_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' 値引き額プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>値引き額</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="DiscountPrice", IsNullable:=False)> _
            Public Property DiscountPrice As String
                Get
                    Return discountPrice_
                End Get
                Set(value As String)
                    discountPrice_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' メモプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>メモ</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Memo", IsNullable:=False)> _
            Public Property Memo As String
                Get
                    Return memo_
                End Get
                Set(value As String)
                    memo_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' 見積印刷日プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>見積印刷日</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="EstprintDate", IsNullable:=False)> _
            Public Property EstprintDate As String
                Get
                    Return estprintDate_
                End Get
                Set(value As String)
                    estprintDate_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' 契約書Noプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>契約書No</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ContractNo", IsNullable:=False)> _
            Public Property ContractNo As String
                Get
                    Return contractNo_
                End Get
                Set(value As String)
                    contractNo_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' 契約書印刷フラグプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>契約書印刷フラグ</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ContPrintFlg", IsNullable:=False)> _
            Public Property ContPrintFlg As String
                Get
                    Return contPrintFlg_
                End Get
                Set(value As String)
                    contPrintFlg_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' 契約状況フラグプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>契約状況フラグ</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ContractFlg", IsNullable:=False)> _
            Public Property ContractFlg As String
                Get
                    Return contractFlg_
                End Get
                Set(value As String)
                    contractFlg_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' 契約完了日プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>契約完了日</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ContractDate", IsNullable:=False)> _
            Public Property ContractDate As String
                Get
                    Return contractDate_
                End Get
                Set(value As String)
                    contractDate_ = value
                End Set
            End Property


    
            ''' <summary>
            ''' 削除フラグプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>削除フラグ</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="DelFlg", IsNullable:=False)> _
            Public Property DelFlg As String
                Get
                    Return delFlg_
                End Get
                Set(value As String)
                    delFlg_ = value
                End Set
            End Property

            
            ''' <summary>
            ''' TCVバージョンプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>TCVバージョン</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="TcvVersion", IsNullable:=False)> _
            Public Property TcvVersion As String
                Get
                    Return tcvVersion_
                End Get
                Set(value As String)
                    tcvVersion_ = value
                End Set
            End Property            

            
            ''' <summary>
            ''' シリーズコード名称プロパティ
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
            ''' シリーズ名称プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>シリーズ名称</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="SeriesNm", IsNullable:=False)> _
            Public Property SeriesNM As String
                Get
                    Return seriesNM_
                End Get
                Set(value As String)
                    seriesNM_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' モデルコードプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>モデルコード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ModelCd", IsNullable:=False)> _
            Public Property ModelCD As String
                Get
                    Return modelCD_
                End Get
                Set(value As String)
                    modelCD_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' モデル名称プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>モデル名称</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ModelNm", IsNullable:=False)> _
            Public Property ModelNM As String
                Get
                    Return modelNM_
                End Get
                Set(value As String)
                    modelNM_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' ボディータイププロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>ボディータイプ</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="BodyType", IsNullable:=False)> _
            Public Property BodyType As String
                Get
                    Return bodyType_
                End Get
                Set(value As String)
                    bodyType_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' 駆動方式プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>駆動方式</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="DriveSystem", IsNullable:=False)> _
            Public Property DriveSystem As String
                Get
                    Return driveSystem_
                End Get
                Set(value As String)
                    driveSystem_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' 排気量プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>排気量</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Displacement", IsNullable:=False)> _
            Public Property Displacement As String
                Get
                    Return displacement_
                End Get
                Set(value As String)
                    displacement_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' ミッションタイププロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>ミッションタイプ</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Transmission", IsNullable:=False)> _
            Public Property Transmission As String
                Get
                    Return transmission_
                End Get
                Set(value As String)
                    transmission_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' サフィックスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>サフィックス</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="SuffixCd", IsNullable:=False)> _
            Public Property SuffixCD As String
                Get
                    Return suffixCD_
                End Get
                Set(value As String)
                    suffixCD_ = value
                End Set
            End Property


            ''' <summary>
            ''' 外装色コードプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>外装色コード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ExtColorCd", IsNullable:=False)> _
            Public Property ExtColorCD As String
                Get
                    Return extColorCD_
                End Get
                Set(value As String)
                    extColorCD_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' 外装色プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>外装色</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ExtColor", IsNullable:=False)> _
            Public Property ExtColor As String
                Get
                    Return extColor_
                End Get
                Set(value As String)
                    extColor_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' 外装追加費用プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>外装追加費用</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ExtAmount", IsNullable:=False)> _
            Public Property ExtAmount As String
                Get
                    Return extAmount_
                End Get
                Set(value As String)
                    extAmount_ = value
                End Set
            End Property


            ''' <summary>
            ''' 内装色コードプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>内装色コード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="IntColorCd", IsNullable:=False)> _
            Public Property IntColorCD As String
                Get
                    Return intColorCD_
                End Get
                Set(value As String)
                    intColorCD_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' 内装色プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>内装色</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="IntColor", IsNullable:=False)> _
            Public Property IntColor As String
                Get
                    Return intColor_
                End Get
                Set(value As String)
                    intColor_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' 内装追加費用プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>内装追加費用</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="IntAmount", IsNullable:=False)> _
            Public Property IntAmount As String
                Get
                    Return intAmount_
                End Get
                Set(value As String)
                    intAmount_ = value
                End Set
            End Property


            ''' <summary>
            ''' 車両型号プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>車両型号</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="ModelNumber", IsNullable:=False)> _
            Public Property ModelNumber As String
                Get
                    Return modelNumber_
                End Get
                Set(value As String)
                    modelNumber_ = value
                End Set
            End Property
    
    
            ''' <summary>
            ''' 車両価格プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>車両価格</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="BasePrice", IsNullable:=False)> _
            Public Property BasePrice As String
                Get
                    Return basePrice_
                End Get
                Set(value As String)
                    basePrice_ = value
                End Set
            End Property

        
            ''' <summary>
            ''' 作成日プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>作成日</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="CreateDate", IsNullable:=False)> _
            Public Property CreateDate As String
                Get
                    Return createDate_
                End Get
                Set(value As String)
                    createDate_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' 作成ユーザアカウントプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>作成ユーザアカウント</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="CreateAccount", IsNullable:=False)> _
            Public Property CreateAccount As String
                Get
                    Return createAccount_
                End Get
                Set(value As String)
                    createAccount_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' 更新ユーザアカウントプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>更新ユーザアカウント</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="UpdateAccount", IsNullable:=False)> _
            Public Property UpdateAccount As String
                Get
                    Return updateAccount_
                End Get
                Set(value As String)
                    updateAccount_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' 作成機能IDプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>作成機能ID</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="CreateId", IsNullable:=False)> _
            Public Property CreateId As String
                Get
                    Return createId_
                End Get
                Set(value As String)
                    createId_ = value
                End Set
            End Property

    
            ''' <summary>
            ''' 更新機能IDプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>更新機能ID</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="UpdateId", IsNullable:=False)> _
            Public Property UpdateId As String
                Get
                    Return updateId_
                End Get
                Set(value As String)
                    updateId_ = value
                End Set
            End Property

             
            ''' <summary>
            ''' EstVcloptionInfoクラスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>EstVcloptionInfoオブジェクト</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="EstVcloptionInfo", IsNullable:=False)> _
            Public ReadOnly Property EstVcloptionInfo() As Collection(Of Root_EstVcloptionInfo)
                Get
                    Return estVcloptionInfo_
                End Get
            End Property
 
            ''' <summary>
            ''' EstVcloptionInfoオブジェクト値格納処理
            ''' </summary>
            ''' <param name="value">EstVcloptionInfoオブジェクト</param>
            ''' <remarks></remarks>
            Public Sub SetEstVcloptionInfo(ByVal value As Collection(Of Root_EstVcloptionInfo))
                estVcloptionInfo_ = value
            End Sub
                                    
        End Class

        '''-----------------------------------------------------
        ''' <summary>
        ''' EstVcloptionInfoクラス
        ''' </summary>
        ''' <remarks></remarks>
        '''-----------------------------------------------------
        Public Class Root_EstVcloptionInfo

            Private estimateId_ As String       '見積管理ID
            Private optionPart_ As String       'オプション区分
            Private optionCode_ As String       'オプションコード
            Private optionName_ As String       'オプション名
            Private price_ As String            '価格
            Private installCost_ As String      '取付費用

            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                estimateId_ = String.Empty            '見積管理ID
                optionPart_ = String.Empty            'オプション区分
                optionCode_ = String.Empty            'オプションコード
                optionName_ = String.Empty            'オプション名
                price_ = String.Empty                 '価格
                installCost_ = String.Empty           '取付費用
            End Sub
    
            ''' <summary>
            ''' 見積管理IDプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>見積管理ID</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="EstimateId", IsNullable:=False)> _
            Public Property EstimateId As String
                Get
                    Return estimateId_
                End Get
                Set(value As String)
                    estimateId_ = value
                End Set
            End Property

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
            ''' オプション名プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>オプション名</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="OptionName", IsNullable:=False)> _
            Public Property OptionName As String
                Get
                    Return optionName_
                End Get
                Set(value As String)
                    optionName_ = value
                End Set
            End Property

            ''' <summary>
            ''' 価格プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>価格</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Price", IsNullable:=False)> _
            Public Property Price As String
                Get
                    Return price_
                End Get
                Set(value As String)
                    price_ = value
                End Set
            End Property
    
            ''' <summary>
            ''' 取付費用プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>取付費用</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="InstallCost", IsNullable:=False)> _
            Public Property InstallCost As String
                Get
                    Return installCost_
                End Get
                Set(value As String)
                    installCost_ = value
                End Set
            End Property    
    
        End Class
    
    End Class
    
End Namespace

