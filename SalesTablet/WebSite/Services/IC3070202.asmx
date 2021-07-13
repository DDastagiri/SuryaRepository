<%@ WebService Language="VB" Class="Toyota.eCRB.Estimate.Quotation.WebService.IC3070202" %>

Imports System.Xml
Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess


Namespace Toyota.eCRB.Estimate.Quotation.WebService

#Region "見積情報登録I/Fクラス"
    ' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。

    ' <System.Web.Script.Services.ScriptService()> _
    ''' <summary>
    ''' 見積情報登録I/F
    ''' プレゼンテーション層クラス
    ''' </summary>
    ''' <remarks></remarks>
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    Public Class IC3070202
        Inherits System.Web.Services.WebService

#Region "定数定義"
        ''' <summary>
        ''' メッセージID
        ''' </summary>
        ''' <remarks>
        ''' メッセージに割り当てられた識別コード:IC3070202（見積情報登録）
        ''' </remarks>
        Private Const MessageId As String = "IC3070202"
    
        ''' <summary>
        ''' 応答結果のメッセージ（成功）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageSuccess As String = "Success"
    
        ''' <summary>
        ''' 応答結果のメッセージ（失敗）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MessageFailure As String = "Failure"

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
        ''' エラーコード：システムエラー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ErrCodeSys As Short = 9999
        
        ''' <summary>
        ''' 日付のフォーマット
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FormatDate As String = "yyyyMMdd"
        'Private Const FormatDate As String = "dd/MM/yyyy"
        ''' <summary>
        ''' 日付時刻のフォーマット
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FormatDatetime As String = "yyyyMMddHHmmss"
        'Private Const FormatDatetime As String = "dd/MM/yyyy HH:mm:ss"
        
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
        ''' EstVclOptionInfoタグ名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstVclOptionInfo As String = "EstVcloptionInfo"
        
        ''' <summary>
        ''' Headerタグ：送信メッセージ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagHeadMessageID As Short = 2
        ''' <summary>
        ''' Headerタグ：送信日付
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagHeadTransmissionDate As Short = 1

        ''' <summary>
        ''' Commonタグ：実行モード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagCommonMode As Short = 11
        ''' <summary>
        ''' Commonタグ：更新区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagCommonUpdateDvs As Short = 12
        ''' <summary>
        ''' Commonタグ：車両オプション更新区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagCommonVclOptionUpdateDvs As Short = 13

        ''' <summary>
        ''' 見積情報タグ：見積管理ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstEstimateId As Short = 21
        ''' <summary>
        ''' 見積情報タグ：販売店コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstDlrCD As Short = 22
        ''' <summary>
        ''' 見積情報タグ：店舗コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstStrCD As Short = 23
        ''' <summary>
        ''' 見積情報タグ：Follow-up BOX内連番
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstFllwupBoxSeqNo As Short = 24
        ''' <summary>
        ''' 見積情報タグ：契約店舗コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstCntStrCD As Short = 25
        ''' <summary>
        ''' 見積情報タグ：契約スタッフ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstCntStaff As Short = 26
        ''' <summary>
        ''' 見積情報タグ：顧客種別
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstCstKind As Short = 27
        ''' <summary>
        ''' 見積情報タグ：顧客分類
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstCustomerClass As Short = 28
        ''' <summary>
        ''' 見積情報タグ：活動先顧客コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstCRCustId As Short = 29
        ''' <summary>
        ''' 見積情報タグ：基幹お客様コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstCustId As Short = 30
        ''' <summary>
        ''' 見積情報タグ：納車予定日
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstDeliDate As Short = 31
        ''' <summary>
        ''' 見積情報タグ：値引き額
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstDiscountPrice As Short = 32
        ''' <summary>
        ''' 見積情報タグ：メモ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstMemo As Short = 33
        ''' <summary>
        ''' 見積情報タグ：見積印刷日
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstEstprintDate As Short = 34
        ''' <summary>
        ''' 見積情報タグ：契約書No
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstContractNo As Short = 35
        ''' <summary>
        ''' 見積情報タグ：契約書印刷フラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstContPrintFlg As Short = 36
        ''' <summary>
        ''' 見積情報タグ：契約状況フラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstContractFlg As Short = 37
        ''' <summary>
        ''' 見積情報タグ：契約完了日
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstContractDate As Short = 38
        ''' <summary>
        ''' 見積情報タグ：削除フラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstDelFlg As Short = 39
        ''' <summary>
        ''' 見積情報タグ：TCVバージョン
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstTcvVersion As Short = 40
        ''' <summary>
        ''' 見積情報タグ：シリーズコード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstSeriesCD As Short = 41
        ''' <summary>
        ''' 見積情報タグ：シリーズ名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstSeriesNM As Short = 42
        ''' <summary>
        ''' 見積情報タグ：モデルコード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstModelCD As Short = 43
        ''' <summary>
        ''' 見積情報タグ：モデル名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstModelNM As Short = 44
        ''' <summary>
        ''' 見積情報タグ：ボディータイプ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstBodyType As Short = 45
        ''' <summary>
        ''' 見積情報タグ：駆動方式
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstDriveSystem As Short = 46
        ''' <summary>
        ''' 見積情報タグ：排気量
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstDisplacement As Short = 47
        ''' <summary>
        ''' 見積情報タグ：ミッションタイプ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstTransmission As Short = 48
        ''' <summary>
        ''' 見積情報タグ：サフィックス
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstSuffixCD As Short = 49
        ''' <summary>
        ''' 見積情報タグ：外装色コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstExtColorCD As Short = 50
        ''' <summary>
        ''' 見積情報タグ：外装色名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstExtColor As Short = 51
        ''' <summary>
        ''' 見積情報タグ：外装追加費用
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstExtAmount As Short = 52
        ''' <summary>
        ''' 見積情報タグ：内装色コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstInteColorCD As Short = 53
        ''' <summary>
        ''' 見積情報タグ：内装色名称
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstInteColor As Short = 54
        ''' <summary>
        ''' 見積情報タグ：内装追加費用
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstInteAmount As Short = 55
        ''' <summary>
        ''' 見積情報タグ：車両型号
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstModelNumber As Short = 56
        ''' <summary>
        ''' 見積情報タグ：車両価格
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstBasePrice As Short = 57
        ''' <summary>
        ''' 見積情報タグ：作成日
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstCreateDate As Short = 58
        ''' <summary>
        ''' 見積情報タグ：作成ユーザアカウント
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstCreateAccount As Short = 59
        ''' <summary>
        ''' 見積情報タグ：更新ユーザアカウント
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstUpdateAccount As Short = 60
        ''' <summary>
        ''' 見積情報タグ：作成機能ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstCreateId As Short = 61
        ''' <summary>
        ''' 見積情報タグ：更新機能ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstUpdateId As Short = 62

        ''' <summary>
        ''' 見積車両オプション情報タグ：見積管理ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstVOEstimateId As Short = 71
        ''' <summary>
        ''' 見積車両オプション情報タグ：オプション区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstVOOptionPart As Short = 72
        ''' <summary>
        ''' 見積車両オプション情報タグ：オプションコード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstVOOptionCode As Short = 73
        ''' <summary>
        ''' 見積車両オプション情報タグ：オプション名
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstVOOptionName As Short = 74
        ''' <summary>
        ''' 見積車両オプション情報タグ：価格
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstVOPrice As Short = 75
        ''' <summary>
        ''' 見積車両オプション情報タグ：取付費用
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TagEstVOInstallCost As Short = 76
#End Region
    
#Region "メンバ変数"
        ''' <summary>
        ''' 項目名称
        ''' </summary>
        ''' <remarks>XMLタグの各項目の項目名称を保持する配列</remarks>
        Private Itemname() As String
        
        ''' <summary>
        ''' 項目番号
        ''' </summary>
        ''' <remarks>XMLタグの各項目の項目番号を保持する配列</remarks>
        Private ItemNumber() As Short
        
        ''' <summary>
        ''' 項目必須フラグ
        ''' </summary>
        ''' <remarks>XMLタグの各項目の必須チェック有無を保持する配列</remarks>
        Private Chkrequiredflg() As Short
        
        ''' <summary>
        ''' 項目属性
        ''' </summary>
        ''' <remarks>XMLタグの各項目の項目属性を保持する配列</remarks>
        Private Attribute() As Short
        
        ''' <summary>
        ''' 項目サイズ
        ''' </summary>
        ''' <remarks>XMLタグの各項目の項目サイズを保持する配列</remarks>
        Private Itemsize() As Double
        
        ''' <summary>
        ''' 項目初期値
        ''' </summary>
        ''' <remarks>XMLタグの各項目の初期値を保持する配列</remarks>
        Private DefaultValue() As String
        
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
        
        ''' <summary>
        ''' 送信日時（Request）
        ''' </summary>
        ''' <remarks>メッセージ送信日時(yyyyMMddhhmmss)</remarks>
        Private TransmissionDate As Date

        ''' <summary>
        ''' 実行モード
        ''' </summary>
        ''' <remarks>
        '''  0：見積の全情報を登録/更新
        '''  1：見積の車両情報のみ登録/更新
        ''' </remarks>
        Private Mode As Short

        ''' <summary>
        ''' 更新区分
        ''' </summary>
        ''' <remarks>
        '''  0：登録　
        '''  1：更新　
        '''  2：削除
        ''' </remarks>
        Private UpdateDvs As Short

        ''' <summary>
        ''' 車両オプション更新区分
        ''' </summary>
        ''' <remarks>
        '''  0：車両オプションを全て更新　
        '''  1：車両オプションをメーカーオプションのみ更新
        ''' </remarks>
        Private VcloptionUpdateDvs As Short

        ''' <summary>
        ''' 見積情報データテーブル
        ''' </summary>
        ''' <remarks>EstimationInfoタグ情報格納用のデータテーブル</remarks>
        Private EstimationInfoDT As IC3070202DataSet.IC3070202EstimationInfoDataTable
        
        ''' <summary>
        ''' 見積車両オプション情報データテーブル
        ''' </summary>
        ''' <remarks>EstVclOptionInfoタグ情報格納用のデータテーブル</remarks>
        Private EstVclOptionInfoDT As IC3070202DataSet.IC3070202EstVclOptionInfoDataTable
        
        ''' <summary>
        ''' 終了コード
        ''' </summary>
        ''' <remarks>応答結果のコード（"0"の場合は正常、それ以外の場合エラー）</remarks>
        Private ResultId As Short = ErrCodeSuccess
        
        ''' <summary>
        ''' 見積管理ID
        ''' </summary>
        ''' <remarks></remarks>
        Private EstimateId As Long = 0

        ''' <summary>
        ''' 作成日
        ''' </summary>
        ''' <remarks></remarks>
        Private CreateDate As String = String.Empty
#End Region
    
#Region "Publicメソッド"
        ''' <summary>
        ''' 見積情報を登録します。
        ''' </summary>
        ''' <param name="xsData">登録する見積情報のXML</param>
        ''' <returns>見積情報登録結果のXML</returns>
        ''' <remarks></remarks>
        <WebMethod()> _
        Public Function SetEstimation(ByVal xsData As String) As Response
        
            Dim retXml As Response = Nothing            ' 送信XML
            Dim retMessage As String = MessageFailure   ' メッセージ
            Dim receptionDate As String = String.Empty  ' 受信日時
            
            Try
                ' システム日付を取得する
                receptionDate = DateTimeFunc.Now().ToString(FormatDatetime, CultureInfo.InvariantCulture)

                ' 受信XMLをログ出力
                Logger.Info("Request XML : " & xsData, True)
            
                ' 見積情報データセットのインスタンス生成
                Using estInfoDataSet As IC3070202DataSet = New IC3070202DataSet
            
                    ' 受信XMLをプロパティにセット
                    Me.SetData(xsData, estInfoDataSet)
                    
                    ' 見積情報登録処理用
                    Dim bizLogic As IC3070202BusinessLogic = New IC3070202BusinessLogic     ' 見積情報登録I/Fビジネスロジック
                    Dim estResultDT As IC3070202DataSet.IC3070202EstResultDataTable         ' 見積情報登録結果データテーブル
                    
                    Try
                        ' 見積情報登録処理
                        estResultDT = bizLogic.SetEstimationInfo(estInfoDataSet, Me.UpdateDvs, Me.Mode, Me.VcloptionUpdateDvs, 1)
                        
                        ' 登録処理結果をセット
                        Me.ResultId = bizLogic.ResultId
                        Me.EstimateId = estResultDT.Item(0).EstimateId
                        Me.CreateDate = estResultDT.Item(0).CreateDate.ToString(FormatDatetime, CultureInfo.InvariantCulture)
                        
                        If Me.ResultId = ErrCodeSuccess Then
                            retMessage = MessageSuccess
                        End If
                        
                    Catch ex As Exception
                        Me.ResultId = bizLogic.ResultId
                            
                        Throw
                    Finally
                        bizLogic = Nothing
                        estResultDT = Nothing
                    End Try

                End Using
                
            Catch ex As Exception
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeSys
                End If
                Logger.Error(Me.ResultId.ToString(CultureInfo.InvariantCulture), ex)
            Finally
                ' 返却XMLを作成
                retXml = Me.GetResponseXml(receptionDate, retMessage)
                
                ' 終了コードをログ出力
                Logger.Info("ResultId[" & _
                            Me.TransmissionDate.ToString(FormatDatetime, CultureInfo.InvariantCulture) & _
                            "] : " & _
                            Me.ResultId.ToString(CultureInfo.InvariantCulture), _
                            True)
            End Try
        
            ' 結果を返却
            Return retXml
        
        End Function
#End Region
            
#Region "Privateメソッド"
        ''' <summary>
        ''' XMLタグの情報をデータ格納クラスにセットします。
        ''' </summary>
        ''' <param name="xsData">受信XML</param>
        ''' <param name="estInfoDataSet">見積情報データセット</param>
        ''' <remarks></remarks>
        Private Sub SetData(ByVal xsData As String, ByVal estInfoDataSet As IC3070202DataSet)
        
            ' XmlDocument生成
            Dim xdoc As New XmlDocument
            
            Try
                ' XML読み込み
                xdoc.LoadXml(xsData)
            Catch ex As Exception
                Me.ResultId = ErrCodeXmlDoc
                
                Throw
            End Try

            ' メンバ変数を設定
            Me.RootElement = xdoc.DocumentElement                                ' ルート要素
            Me.EstimationInfoDT = estInfoDataSet.IC3070202EstimationInfo         ' 見積情報データテーブル
            Me.EstVclOptionInfoDT = estInfoDataSet.IC3070202EstVclOptionInfo     ' 見積車両オプション情報データテーブル

            ' Header情報格納
            Me.InitHeader()
            Me.SetHeader()
            
            ' Common情報格納
            Me.InitCommon()
            Me.SetCommon()

            ' EstimationInfo情報格納
            Me.InitEstimationInfo()
            Me.SetEstimationInfo()
            
            ' EstVclOptionInfo情報格納
            Me.InitEstVclOptionInfo()
            Me.SetEstVclOptionInfo()
            
            xdoc = Nothing
        End Sub

#Region "初期化"
        ''' <summary>
        ''' Headerタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitHeader()
        
            ' 項目名称を設定
            Me.Itemname = {"MessageID", "TransmissionDate"}
            
            ' 項目Noを設定
            Me.ItemNumber = {TagHeadMessageID, TagHeadTransmissionDate}
            
            ' 必須必須フラグを設定
            Me.Chkrequiredflg = {CheckRequired, CheckRequired}
            
            ' 項目属性を設定
            Me.Attribute = {AttributeLegth, AttributeDatetime}
            
            ' 項目サイズを設定
            Me.Itemsize = {9, 0}
        End Sub

        ''' <summary>
        ''' Commonタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitCommon()
        
            ' 項目名称を設定
            Me.Itemname = {"Mode", "UpdateDvs", "VcloptionUpdateDvs"}
            
            ' 項目Noを設定
            Me.ItemNumber = {TagCommonMode, TagCommonUpdateDvs, TagCommonVclOptionUpdateDvs}
            
            ' 必須必須フラグを設定
            Me.Chkrequiredflg = {CheckRequired, CheckRequired, CheckRequired}
            
            ' 項目属性を設定
            Me.Attribute = {AttributeNum, AttributeNum, AttributeNum}
            
            ' 項目サイズを設定
            Me.Itemsize = {1, 1, 1}
        End Sub

        ''' <summary>
        ''' EstimationInfoタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitEstimationInfo()
        
            ' 項目名称を設定
            Me.Itemname = _
                {"EstimateId", "DlrCd", "StrCd", "FllwupBox_SeqNo", _
                 "Cnt_StrCd", "Cnt_Staff", "CstKind", "CustomerClass", _
                 "CRCustId", "CustId", "DeliDate", "DiscountPrice", _
                 "Memo", "EstprintDate", "ContractNo", "ContPrintFlg", _
                 "ContractFlg", "ContractDate", "DelFlg", "TcvVersion", _
                 "SeriesCd", "SeriesNm", "ModelCd", "ModelNm", "BodyType", _
                 "DriveSystem", "Displacement", "Transmission", _
                 "SuffixCd", "ExtColorCd", "ExtColor", "ExtAmount", _
                 "IntColorCd", "IntColor", "IntAmount", "ModelNumber", _
                 "BasePrice", "CreateDate", "CreateAccount", "UpdateAccount", _
                 "CreateId", "UpdateId"}
            
            ' 項目Noを設定
            Me.ItemNumber = _
                {TagEstEstimateId, TagEstDlrCD, TagEstStrCD, TagEstFllwupBoxSeqNo, _
                 TagEstCntStrCD, TagEstCntStaff, TagEstCstKind, TagEstCustomerClass, _
                 TagEstCRCustId, TagEstCustId, TagEstDeliDate, TagEstDiscountPrice, _
                 TagEstMemo, TagEstEstprintDate, TagEstContractNo, TagEstContPrintFlg, _
                 TagEstContractFlg, TagEstContractDate, TagEstDelFlg, TagEstTcvVersion, _
                 TagEstSeriesCD, TagEstSeriesNM, TagEstModelCD, TagEstModelNM, TagEstBodyType, _
                 TagEstDriveSystem, TagEstDisplacement, TagEstTransmission, _
                 TagEstSuffixCD, TagEstExtColorCD, TagEstExtColor, TagEstExtAmount, _
                 TagEstInteColorCD, TagEstInteColor, TagEstInteAmount, TagEstModelNumber, _
                 TagEstBasePrice, TagEstCreateDate, TagEstCreateAccount, TagEstUpdateAccount, _
                 TagEstCreateId, TagEstUpdateId}
            
            ' 必須フラグを設定
            Me.Chkrequiredflg = _
                {CheckRequired, CheckRequired, CheckNoRequired, CheckNoRequired, _
                 CheckRequired, CheckRequired, CheckNoRequired, CheckNoRequired, _
                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckNoRequired, _
                 CheckNoRequired, CheckNoRequired, CheckNoRequired, CheckRequired, _
                 CheckRequired, CheckRequired, CheckRequired, CheckRequired, CheckRequired, _
                 CheckRequired, CheckRequired, CheckRequired, _
                 CheckRequired, CheckRequired, CheckRequired, CheckRequired, _
                 CheckRequired, CheckRequired, CheckRequired, CheckRequired, _
                 CheckRequired, CheckRequired, CheckRequired, CheckRequired, _
                 CheckRequired, CheckRequired}
            
            ' 項目属性を設定
            Me.Attribute = _
                {AttributeNum, AttributeLegth, AttributeLegth, AttributeNum, _
                 AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                 AttributeLegth, AttributeLegth, AttributeDatetime, AttributeNum, _
                 AttributeLegth, AttributeDatetime, AttributeLegth, AttributeLegth, _
                 AttributeLegth, AttributeDatetime, AttributeLegth, AttributeLegth, _
                 AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, AttributeLegth, _
                 AttributeLegth, AttributeLegth, AttributeLegth, _
                 AttributeLegth, AttributeLegth, AttributeLegth, AttributeNum, _
                 AttributeLegth, AttributeLegth, AttributeNum, AttributeLegth, _
                 AttributeNum, AttributeDatetime, AttributeLegth, AttributeLegth, _
                 AttributeLegth, AttributeLegth}
            
            ' 項目サイズを設定
            Me.Itemsize = _
                 {10, 5, 3, 20, _
                 3, 20, 1, 1, _
                 20, 18, 0, 11.2, _
                 1024, 0, 32, 1, _
                 1, 0, 1, 6, _
                 64, 128, 32, 128, 64, _
                 64, 64, 64, _
                 32, 6, 50, 11.2, _
                 7, 50, 11.2, 32, _
                 11.2, 0, 20, 20, _
                 16, 16}
            
            ' 項目初期値を設定
            Me.DefaultValue = _
                {"", "", "", "", _
                 "", "", "", "", _
                 "", "", "", "", _
                 "", "", "", "0", _
                 "0", "", "0", "", _
                 "", "", "", "", "", _
                 "", "", "", _
                 "", "", "", "", _
                 "", "", "", "", _
                 "", "", "", "", _
                 "", ""}
            
            ' 更新区分が0（登録）の場合のみ必須チェックをOptionalに変更
            If Me.UpdateDvs = 0 Then
                ' 見積管理ID
                Me.Chkrequiredflg.SetValue(CheckNoRequired, Array.IndexOf(Me.Itemname, "EstimateId"))
                Me.EstimationInfoDT.ESTIMATEIDColumn.AllowDBNull = True
                ' 作成日
                Me.Chkrequiredflg.SetValue(CheckNoRequired, Array.IndexOf(Me.Itemname, "CreateDate"))
                Me.EstimationInfoDT.CREATEDATEColumn.AllowDBNull = True
            End If

        End Sub

        ''' <summary>
        ''' EstVclOptionInfoタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitEstVclOptionInfo()
        
            ' 項目名称を設定
            Me.Itemname = _
                {"EstimateId", "OptionPart", "OptionCode", "OptionName", _
                 "Price", "InstallCost"}
            
            ' 項目Noを設定
            Me.ItemNumber = _
                {TagEstVOEstimateId, TagEstVOOptionPart, TagEstVOOptionCode, TagEstVOOptionName, _
                 TagEstVOPrice, TagEstVOInstallCost}
            
            ' 必須必須フラグを設定
            Me.Chkrequiredflg = _
                {CheckRequired, CheckRequired, CheckRequired, CheckRequired, _
                 CheckRequired, CheckNoRequired}
            
            ' 項目属性を設定
            Me.Attribute = _
                {AttributeNum, AttributeLegth, AttributeLegth, AttributeLegth, _
                 AttributeNum, AttributeNum}
            
            ' 項目サイズを設定
            Me.Itemsize = _
                 {10, 1, 10, 64, _
                  11.2, 11.2}
            
            ' 更新区分が0（登録）の場合のみ必須チェックをOptionalに変更
            If Me.UpdateDvs = 0 Then
                ' 見積管理ID
                Me.Chkrequiredflg.SetValue(CheckNoRequired, Array.IndexOf(Me.Itemname, "EstimateId"))
                Me.EstVclOptionInfoDT.ESTIMATEIDColumn.AllowDBNull = True
            End If
            
        End Sub
#End Region

#Region "プロパティーセット"
        ''' <summary>
        ''' Headerタグ情報のプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetHeader()

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
            
                ' MessageIdタグのNodeListを取得する
                'Dim messageId As String = Me.GetElementValue(itemNo)
                
                ' TransmissionDateタグのNodeListを取得する
                itemNo += 1
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
                Me.Mode = Me.GetCommonElementValue(itemNo)

                ' UpdateDvsタグのNodeListを取得する
                itemNo += 1
                Me.UpdateDvs = Me.GetCommonElementValue(itemNo)

                ' VcloptionUpdateDvsタグのNodeListを取得する
                itemNo += 1
                Me.VcloptionUpdateDvs = Me.GetCommonElementValue(itemNo)
                
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
            
            ' 見積情報データテーブル行
            Dim estimationInfoRow As IC3070202DataSet.IC3070202EstimationInfoRow
            
            Try
                ' XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagEstimationInfo)
                
                ' XML要素を設定
                nodeDocument = New XmlDocument
                nodeDocument.LoadXml(nodeList.ItemOf(0).OuterXml)
                Me.NodeElement = nodeDocument.DocumentElement
                
                ' 見積情報データテーブルの新規行を作成
                estimationInfoRow = Me.EstimationInfoDT.NewRow()
                
                ' 編集開始
                estimationInfoRow.BeginEdit()
                                
                ' EstimateIdタグのNodeListを取得する
                itemNo = 0               
                estimationInfoRow(Me.EstimationInfoDT.ESTIMATEIDColumn) = Me.GetEstimationInfoElementValue(itemNo)

                ' DlrCdタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.DLRCDColumn) = Me.GetEstimationInfoElementValue(itemNo)

                ' StrCdタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.STRCDColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' FllwupBox_SeqNoタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.FLLWUPBOX_SEQNOColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' Cnt_StrCdタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CNT_STRCDColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' Cnt_StaffタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CNT_STAFFColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' CstKindタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CSTKINDColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' CustomerClassタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CUSTOMERCLASSColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' CRCustIdタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CRCUSTIDColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' CustIdタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CUSTIDColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' DeliDateタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.DELIDATEColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' DiscountPriceタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.DISCOUNTPRICEColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' MemoタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.MEMOColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' EstprintDateタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.ESTPRINTDATEColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' ContractNoタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CONTRACTNOColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' ContPrintFlgタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CONTPRINTFLGColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' ContractFlgタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CONTRACTFLGColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' ContractDateタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CONTRACTDATEColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' DelFlgタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.DELFLGColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' TcvVersionタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.TCVVERSIONColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' SeriesCdタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.SERIESCDColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' SeriesNmタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.SERIESNMColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' ModelCdタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.MODELCDColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' ModelNmタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.MODELNMColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' BodyTypeタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.BODYTYPEColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' DriveSystemタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.DRIVESYSTEMColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' DisplacementタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.DISPLACEMENTColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' TransmissionタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.TRANSMISSIONColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' SuffixCdタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.SUFFIXCDColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' ExtColorCdタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.EXTCOLORCDColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' ExtColorタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.EXTCOLORColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' ExtAmountタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.EXTAMOUNTColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' IntColorCdタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.INTCOLORCDColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' IntColorタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.INTCOLORColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' IntAmountタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.INTAMOUNTColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' ModelNumberタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.MODELNUMBERColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' BasePriceタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.BASEPRICEColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' CreateDateタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CREATEDATEColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' CreateAccountタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CREATEACCOUNTColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' UpdateAccountタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.UPDATEACCOUNTColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' CreateIdタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.CREATEIDColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                ' UpdateIdタグのNodeListを取得する
                itemNo += 1
                estimationInfoRow(Me.EstimationInfoDT.UPDATEIDColumn) = Me.GetEstimationInfoElementValue(itemNo)
                
                '2013/02/06 TCS 橋本 【A.STEP2】Add Start
                ' TCVで見積りを上書きした際に見積実績フラグを保持する
                Dim adapter As New IC3070201TableAdapter(0)
                '見積管理IDがある場合
                If Not IsDBNull(Me.GetEstimationInfoElementValue(0)) Then
                    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START 
                    Dim estInfoDt As IC3070201DataSet.IC3070201EstimationInfoDataTable = adapter.GetEstimationInfoDataTable(estimationInfoRow.ESTIMATEID, Mode)
                    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END 
                    If estInfoDt.Count > 0 Then
                        ' 見積情報から見積実績フラグを取得する
                        estimationInfoRow.EST_ACT_FLG = estInfoDt(0).EST_ACT_FLG
                    End If
                End If
                '2013/02/06 TCS 橋本 【A.STEP2】Add End

                ' 編集終了
                estimationInfoRow.EndEdit()
                
                ' 編集内容を反映
                Me.EstimationInfoDT.AddIC3070202EstimationInfoRow(estimationInfoRow)
                
            Catch dex As System.Data.DataException
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeSys
                End If
                Throw
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
        ''' EstVclOptionInfoタグ情報のプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetEstVclOptionInfo()

            Dim itemNo As Short = 0             ' タグ番号
            Dim nodeList As XmlNodeList         ' XMLノードリスト
            Dim nodeDocument As XmlDocument     ' XML要素
            
            ' 見積車両オプション情報データテーブル行
            Dim estVclOptionInfoRow As IC3070202DataSet.IC3070202EstVclOptionInfoRow
            
            Try
                ' XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagEstVclOptionInfo)
                
                ' XMLノードリスト内のXML要素分実行
                For Each elem As XmlElement In nodeList
                    
                    ' XML要素を設定
                    nodeDocument = New XmlDocument
                    nodeDocument.LoadXml(elem.OuterXml)
                    Me.NodeElement = nodeDocument.DocumentElement
                
                    ' 見積車両オプション情報データテーブルの新規行を作成
                    estVclOptionInfoRow = Me.EstVclOptionInfoDT.NewRow()
                
                    ' 編集開始
                    estVclOptionInfoRow.BeginEdit()
                                
                    ' EstimateIdタグのNodeListを取得する
                    itemNo = 0
                    estVclOptionInfoRow(Me.EstVclOptionInfoDT.ESTIMATEIDColumn) = Me.GetEstVclOptionInfoElementValue(itemNo)

                    ' OptionPartタグのNodeListを取得する
                    itemNo += 1
                    estVclOptionInfoRow(Me.EstVclOptionInfoDT.OPTIONPARTColumn) = Me.GetEstVclOptionInfoElementValue(itemNo)

                    ' OptionCodeタグのNodeListを取得する
                    itemNo += 1
                    estVclOptionInfoRow(Me.EstVclOptionInfoDT.OPTIONCODEColumn) = Me.GetEstVclOptionInfoElementValue(itemNo)
                
                    ' OptionNameタグのNodeListを取得する
                    itemNo += 1
                    estVclOptionInfoRow(Me.EstVclOptionInfoDT.OPTIONNAMEColumn) = Me.GetEstVclOptionInfoElementValue(itemNo)
                
                    ' PriceタグのNodeListを取得する
                    itemNo += 1
                    estVclOptionInfoRow(Me.EstVclOptionInfoDT.PRICEColumn) = Me.GetEstVclOptionInfoElementValue(itemNo)
                
                    ' InstallCostタグのNodeListを取得する
                    itemNo += 1
                    estVclOptionInfoRow(Me.EstVclOptionInfoDT.INSTALLCOSTColumn) = Me.GetEstVclOptionInfoElementValue(itemNo)
                
                    ' 編集終了
                    estVclOptionInfoRow.EndEdit()
                
                    ' 編集内容を反映
                    Me.EstVclOptionInfoDT.AddIC3070202EstVclOptionInfoRow(estVclOptionInfoRow)
                    
                    nodeDocument = Nothing
                    Me.NodeElement = Nothing
                Next
                
            Catch dex As System.Data.DataException
                If Me.ResultId = ErrCodeSuccess Then
                    Me.ResultId = ErrCodeSys
                End If
                Throw
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
        
#Region "各タグデータの取得"
        ''' <summary>
        ''' Commonタグのデータを取得します。
        ''' </summary>
        ''' <param name="No">項目No</param>
        ''' <returns>XMLから取り出した値</returns>
        ''' <remarks>
        ''' XMLからデータを取り出し、必須／属性／サイズチェックを実施します。
        ''' </remarks>
        Private Function GetCommonElementValue(ByVal no As Short) As Object
            
            ' 返却するオブジェクトを取得
            Dim valueObj As Object = Me.GetElementValue(no)
            
            ' チェック結果
            Dim isValid As Boolean = True
            
            ' 実行モードの値チェック
            If Not Me.IsValidMode(no, valueObj) Then
                isValid = False
            End If
            
            ' 更新区分の値チェック
            If Not Me.IsValidUpdateDvs(no, valueObj) Then
                isValid = False
            End If
            
            ' 車両オプション更新区分の値チェック
            If Not Me.IsValidVclOptionUpdateDvs(no, valueObj) Then
                isValid = False
            End If
            
            ' チェック結果がNGの場合
            If Not isValid Then
                Me.ResultId = ErrCodeItValue + Me.ItemNumber(no)
                Throw New ArgumentException("", Me.Itemname(no))
            End If
            
            Return valueObj
            
        End Function
        
        ''' <summary>
        ''' EstimationInfoタグのデータを取得します。
        ''' </summary>
        ''' <param name="No">項目No</param>
        ''' <returns>XMLから取り出した値</returns>
        ''' <remarks>
        ''' XMLからデータを取り出し、必須／属性／サイズチェックを実施します。
        ''' </remarks>
        Private Function GetEstimationInfoElementValue(ByVal no As Short) As Object
            
            ' 返却するオブジェクトを取得
            Dim valueObj As Object = Me.GetElementValue(no)
            
            ' チェック結果
            Dim isValid As Boolean = True
            
            ' 顧客種別の値チェック
            If Not Me.IsValidCstKind(no, valueObj) Then
                isValid = False
            End If
            
            ' 顧客分類の値チェック
            If Not Me.IsValidCustomerClass(no, valueObj) Then
                isValid = False
            End If

            ' 契約書印刷フラグの値チェック
            If Not Me.IsValidContPrintFlg(no, valueObj) Then
                isValid = False
            End If

            ' 契約状況フラグの値チェック
            If Not Me.IsValidContractFlg(no, valueObj) Then
                isValid = False
            End If

            ' 削除フラグの値チェック
            If Not Me.IsValidDelFlg(no, valueObj) Then
                isValid = False
            End If

            ' チェック結果がNGの場合
            If Not isValid Then
                Me.ResultId = ErrCodeItValue + Me.ItemNumber(no)
                Throw New ArgumentException("", Me.Itemname(no))
            End If
            
            ' 初期値の設定
            valueObj = Me.SetDefaultValue(no, valueObj)
            
            Return valueObj
            
        End Function
        
        ''' <summary>
        ''' EstVclOptionInfoタグのデータを取得します。
        ''' </summary>
        ''' <param name="No">項目No</param>
        ''' <returns>XMLから取り出した値</returns>
        ''' <remarks>
        ''' XMLからデータを取り出し、必須／属性／サイズチェックを実施します。
        ''' </remarks>
        Private Function GetEstVclOptionInfoElementValue(ByVal no As Short) As Object
            
            ' 返却するオブジェクトを取得
            Dim valueObj As Object = Me.GetElementValue(no)
            
            ' チェック結果
            Dim isValid As Boolean = True
            
            ' オプション区分の値チェック
            If Not Me.IsValidOptionPart(no, valueObj) Then
                isValid = False
            End If
            
            ' チェック結果がNGの場合
            If Not isValid Then
                Me.ResultId = ErrCodeItValue + Me.ItemNumber(no)
                Throw New ArgumentException("", Me.Itemname(no))
            End If
            
            Return valueObj
            
        End Function
#End Region
        
#Region "XML内のデータ取得"
        ''' <summary>
        ''' XML内のデータを取得します。
        ''' </summary>
        ''' <param name="No">項目No</param>
        ''' <returns>XMLから取り出した値</returns>
        ''' <remarks>
        ''' XMLからデータを取り出し、必須／属性／サイズチェックを実施します。
        ''' </remarks>
        Private Function GetElementValue(ByVal no As Short) As Object
            
            ' 返却するオブジェクト
            Dim valueObj As Object = Nothing

            Try
                '指定タグのNodeListを取得する
                Dim node As XmlNodeList = Me.NodeElement.GetElementsByTagName(Itemname(no))

                '指定したタグの存在有無により値をSet
                Dim valueString As String = String.Empty
                If node.Count > 0 Then
                    '指定したタグが存在したのでInnerTextプロパティで値を取得する
                    valueString = RTrim(node.Item(0).InnerText)
                Else
                    valueString = ""
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
                
                Dim itemSizeDbl As Double = Me.Itemsize(no)
                Dim itemSizeStr As String = Me.Itemsize(no).ToString(CultureInfo.InvariantCulture)
                    
                ' 属性別のチェック
                Select Case Attribute(no)
                    
                    Case AttributeByte
                        ' 属性：Byteチェック
                        
                        If valueString.Length > 0 Then
                            If Not Validation.IsCorrectByte(valueString, itemSizeDbl) Then
                                Me.ResultId = ErrCodeItSize + Me.ItemNumber(no)
                                Throw New ArgumentException("", Me.Itemname(no))
                            End If
                        End If
                        
                    Case AttributeLegth
                        ' 属性：文字数チェック
                        
                        If valueString.Length > 0 Then
                            If Not Validation.IsCorrectDigit(valueString, itemSizeDbl) Then
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
                            Dim utf8 As New UTF8Encoding
                            
                            ' 全角文字はエラー
                            If valueString.Length <> utf8.GetByteCount(valueString) Then
                                Me.ResultId = ErrCodeItType + Me.ItemNumber(no)
                                Throw New ArgumentException("", Me.Itemname(no))
                            End If
                            ' 数値型
                            If Not IsNumeric(valueString) Then
                                Me.ResultId = ErrCodeItType + Me.ItemNumber(no)
                                Throw New ArgumentException("", Me.Itemname(no))
                            Else
                                ' 小数の桁数を取得
                                Dim dec As Integer
                                If itemSizeStr.IndexOf(".", StringComparison.OrdinalIgnoreCase) > 0 Then
                                    dec = CInt(Mid(itemSizeStr, itemSizeStr.IndexOf(".", StringComparison.OrdinalIgnoreCase) + 2))
                                Else
                                    dec = 0
                                End If

                                ' 整数部分をチェック
                                If Math.Abs(Int(CDec(valueString))).ToString(CultureInfo.InvariantCulture).Length > Int(itemSizeDbl) - dec Then
                                    Me.ResultId = ErrCodeItSize + Me.ItemNumber(no)
                                    Throw New ArgumentException("", Me.Itemname(no))
                                Else
                                    ' 小数チェック存在時に小数部分の桁数をチェック(小数点が存在するときのみ)
                                    If valueString.IndexOf(".", StringComparison.OrdinalIgnoreCase) > 0 Then
                                        ' 小数部分を取得し、小数桁をチェック
                                        If Mid(valueString, valueString.IndexOf(".", StringComparison.OrdinalIgnoreCase) + 2).Length > dec Then
                                            If dec = 0 Then
                                                Me.ResultId = ErrCodeItType + Me.ItemNumber(no)
                                                Throw New ArgumentException("", Me.Itemname(no))
                                            Else
                                                Me.ResultId = ErrCodeItSize + Me.ItemNumber(no)
                                                Throw New ArgumentException("", Me.Itemname(no))
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                            
                            utf8 = Nothing
                        End If
                        
                    Case AttributeDate
                        ' 属性：Dateチェック
                        
                        ' 空の場合はDBNull値をセット
                        If valueString = "" Then
                            valueObj = Convert.DBNull
                        Else
                            ' 指定されたフォーマットの日付書式か
                            valueObj = ConvertDateTime(valueString, FormatDate, ErrCodeItType + Me.ItemNumber(no))
                        End If

                    Case AttributeDatetime
                        ' 属性：DateTimeチェック
                        
                        ' 空の場合はDBNull値をセット
                        If valueString = "" Then
                            valueObj = Convert.DBNull
                        Else
                            ' 指定されたフォーマットの日付時刻書式か
                            valueObj = ConvertDateTime(valueString, FormatDatetime, ErrCodeItType + Me.ItemNumber(no))
                        End If
                        
                    Case Else
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
                ' 指定されたフォーマット書式の日付に変換
                Return DateTime.ParseExact(valueString, formatDate, Nothing)
            Catch ex As Exception
                Me.ResultId = errNumber
                Throw
            End Try

        End Function
#End Region

#Region "各タグの値チェック"
        ''' <summary>
        ''' 実行モードの値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値：0 or 1</remarks>
        Private Function IsValidMode(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagCommonMode Then
                isValid = True
            ElseIf valueObj = 0 Or valueObj = 1 Then
                isValid = True
            End If

            Return isValid

        End Function

        ''' <summary>
        ''' 更新区分の値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値：0 or 1 or 2</remarks>
        Private Function IsValidUpdateDvs(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagCommonUpdateDvs Then
                isValid = True
            ElseIf valueObj = 0 Or valueObj = 1 Or valueObj = 2 Then
                isValid = True
            End If

            Return isValid

        End Function

        ''' <summary>
        ''' 車両オプション更新区分の値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値：0 or 1</remarks>
        Private Function IsValidVclOptionUpdateDvs(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagCommonVclOptionUpdateDvs Then
                isValid = True
            ElseIf valueObj = 0 Or valueObj = 1 Then
                isValid = True
            End If

            Return isValid

        End Function

        ''' <summary>
        ''' 顧客種別の値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値："1" or "2" or ""</remarks>
        Private Function IsValidCstKind(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False
            
            If Me.ItemNumber(no) <> TagEstCstKind Then
                isValid = True
            ElseIf valueObj.Equals("1") Or valueObj.Equals("2") Or valueObj.Equals("") Then
                isValid = True
            End If

            Return isValid

        End Function

        ''' <summary>
        ''' 顧客分類の値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値："1" or "2" or "3" or ""</remarks>
        Private Function IsValidCustomerClass(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagEstCustomerClass Then
                isValid = True
            ElseIf valueObj.Equals("1") Or valueObj.Equals("2") Or valueObj.Equals("3") Or valueObj.Equals("") Then
                isValid = True
            End If

            Return isValid

        End Function

        ''' <summary>
        ''' 契約書印刷フラグの値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値："0" or "1" or ""</remarks>
        Private Function IsValidContPrintFlg(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagEstContPrintFlg Then
                isValid = True
            ElseIf valueObj.Equals("0") Or valueObj.Equals("1") Or valueObj.Equals("") Then
                isValid = True
            End If

            Return isValid

        End Function

        ''' <summary>
        ''' 契約状況フラグの値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値："0" or "1" or "2" or ""</remarks>
        Private Function IsValidContractFlg(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagEstContractFlg Then
                isValid = True
            ElseIf valueObj.Equals("0") Or valueObj.Equals("1") Or valueObj.Equals("2") Or valueObj.Equals("") Then
                isValid = True
            End If

            Return isValid

        End Function

        ''' <summary>
        ''' 削除フラグの値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値："0" or "1" or ""</remarks>
        Private Function IsValidDelFlg(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagEstDelFlg Then
                isValid = True
            ElseIf valueObj.Equals("0") Or valueObj.Equals("1") Or valueObj.Equals("") Then
                isValid = True
            End If

            Return isValid

        End Function

        ''' <summary>
        ''' オプション区分の値チェック
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>True：チェックOK、False：チェックNG</returns>
        ''' <remarks>許容値："1" or "2"</remarks>
        Private Function IsValidOptionPart(ByVal no As Short, ByVal valueObj As Object) As Boolean
            
            Dim isValid As Boolean = False

            If Me.ItemNumber(no) <> TagEstVOOptionPart Then
                isValid = True
            ElseIf valueObj.Equals("1") Or valueObj.Equals("2") Then
                isValid = True
            End If

            Return isValid

        End Function
#End Region

#Region "デフォルト値設定"
        ''' <summary>
        ''' 値が空の場合、初期値を設定します。
        ''' </summary>
        ''' <param name="no">項目No</param>
        ''' <param name="valueObj">値</param>
        ''' <returns>デフォルト値</returns>
        ''' <remarks></remarks>
        Private Function SetDefaultValue(ByVal no As Short, ByVal valueObj As Object) As Object
            
            If Not String.IsNullOrEmpty(Me.DefaultValue(no)) Then
                If String.IsNullOrEmpty(valueObj) Then
                    valueObj = Me.DefaultValue(no)
                End If
            End If
            
            Return valueObj
        End Function
#End Region
        
#Region "応答用XML作成"
        ''' <summary>
        ''' 応答用インターフェイスを返却します。
        ''' </summary>
        ''' <param name="receptionDate">受信日時</param>
        ''' <param name="retMessage">メッセージ</param>
        ''' <returns>応答用インターフェイス</returns>
        ''' <remarks></remarks>
        Private Function GetResponseXml(ByVal receptionDate As String, ByVal retMessage As String) As Response
            
            ' システム日付を取得する
            Dim transmissionDate As String = DateTimeFunc.Now().ToString(FormatDatetime, CultureInfo.InvariantCulture)
            
            ' Responseクラス生成
            Dim iResponse As Response = New Response()
            
            ' Headerクラスに値をセット
            Dim iRespHead As Response.Root_Head = New Response.Root_Head()
            iRespHead.MessageId = MessageId
            iRespHead.ReceptionDate = receptionDate
            iRespHead.TransmissionDate = transmissionDate
            
            ' Detailクラス生成
            Dim iRespDetail As Response.Root_Detail = New Response.Root_Detail()
            
            ' Commonクラスに値をセット
            Dim iRespCommon As Response.Root_Detail.Detail_Common = New Response.Root_Detail.Detail_Common()
            iRespCommon.ResultId = Me.ResultId.ToString(CultureInfo.InvariantCulture)
            iRespCommon.Message = retMessage
                
            ' EstimationInfoクラスに値をセット
            Dim iRespEst As Response.Root_Detail.Detail_EstimationInfo = New Response.Root_Detail.Detail_EstimationInfo()
            iRespEst.EstimateId = Me.EstimateId.ToString(CultureInfo.InvariantCulture)
            iRespEst.CreateDate = Me.CreateDate
                
            ' DetailクラスにCommon、EstimationInfoをセット
            iRespDetail.Common = iRespCommon
            iRespDetail.EstimationInfo = iRespEst
            
            ' ResponseクラスにHeader、Detailをセット
            iResponse.Head = iRespHead
            iResponse.Detail = iRespDetail
            
            Return iResponse

        End Function
#End Region
        
#Region "応答用XMLインターフェイス"
        ''' <summary>
        ''' 応答用XMLのインターーフェイスクラス
        ''' </summary>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlRoot("Response", Namespace:="http://tempuri.org/Response.xsd")> _
        Public Class Response

            ''' <summary>
            ''' Headタグ
            ''' </summary>
            ''' <remarks></remarks>
            Public Class Root_Head
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="MessageID", IsNullable:=False)> _
                Private prpMessageID As String
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="ReceptionDate", IsNullable:=False)> _
                Private prpReceptionDate As String
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="TransmissionDate", IsNullable:=False)> _
                Private prpTransmissionDate As String
                
                ''' <summary>
                ''' メッセージID
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property MessageId() As String
                    Set(ByVal value As String)
                        prpMessageID = value
                    End Set
                    Get
                        Return prpMessageID
                    End Get
                End Property
                
                ''' <summary>
                ''' 受信日時
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property ReceptionDate() As String
                    Set(ByVal value As String)
                        prpReceptionDate = value
                    End Set
                    Get
                        Return prpReceptionDate
                    End Get
                End Property
                
                ''' <summary>
                ''' 送信日時
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property TransmissionDate() As String
                    Set(ByVal value As String)
                        prpTransmissionDate = value
                    End Set
                    Get
                        Return prpTransmissionDate
                    End Get
                End Property
                    
            End Class

            ''' <summary>
            ''' Detailタグ
            ''' </summary>
            ''' <remarks></remarks>
            Public Class Root_Detail

                ''' <summary>
                ''' Commonタグ
                ''' </summary>
                ''' <remarks></remarks>
                Public Class Detail_Common
                    <System.Xml.Serialization.XmlElementAttribute(ElementName:="ResultId", IsNullable:=False)> _
                    Private prpResultId As String
                    <System.Xml.Serialization.XmlElementAttribute(ElementName:="Message", IsNullable:=False)> _
                    Private prpMessage As String
                    
                    ''' <summary>
                    ''' 終了コード
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    Public Property ResultId() As String
                        Set(ByVal value As String)
                            prpResultId = value
                        End Set
                        Get
                            Return prpResultId
                        End Get
                    End Property

                    ''' <summary>
                    ''' メッセージ
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    Public Property Message() As String
                        Set(ByVal value As String)
                            prpMessage = value
                        End Set
                        Get
                            Return prpMessage
                        End Get
                    End Property
                End Class

                ''' <summary>
                ''' EstimationInfoタグ
                ''' </summary>
                ''' <remarks></remarks>
                Public Class Detail_EstimationInfo
                    <System.Xml.Serialization.XmlElementAttribute(ElementName:="EstimateId", IsNullable:=False)> _
                    Private prpEstimateId As String
                    <System.Xml.Serialization.XmlElementAttribute(ElementName:="CreateDate", IsNullable:=False)> _
                    Private prpCreateDate As String

                    ''' <summary>
                    ''' 見積管理ID
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    Public Property EstimateId() As String
                        Set(ByVal value As String)
                            prpEstimateId = value
                        End Set
                        Get
                            Return prpEstimateId
                        End Get
                    End Property
                    
                    ''' <summary>
                    ''' 作成日
                    ''' </summary>
                    ''' <value></value>
                    ''' <returns></returns>
                    ''' <remarks></remarks>
                    Public Property CreateDate() As String
                        Set(ByVal value As String)
                            prpCreateDate = value
                        End Set
                        Get
                            Return prpCreateDate
                        End Get
                    End Property
                End Class

                <System.Xml.Serialization.XmlElementAttribute(ElementName:="Common", IsNullable:=False)> _
                Private prpCommon As Detail_Common
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="EstimationInfo", IsNullable:=False)> _
                Private prpEstimationInfo As Detail_EstimationInfo

                ''' <summary>
                ''' Commonタグ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property Common() As Detail_Common
                    Set(ByVal value As Detail_Common)
                        prpCommon = value
                    End Set
                    Get
                        Return prpCommon
                    End Get
                End Property

                ''' <summary>
                ''' Estimationタグ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property EstimationInfo() As Detail_EstimationInfo
                    Set(ByVal value As Detail_EstimationInfo)
                        prpEstimationInfo = value
                    End Set
                    Get
                        Return prpEstimationInfo
                    End Get
                End Property
            End Class

            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Head", IsNullable:=False)> _
            Private prpHead As Root_Head
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Detail", IsNullable:=False)> _
            Private prpDetail As Root_Detail

            
            ''' <summary>
            ''' Headerタグ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Head() As Root_Head
                Set(ByVal value As Root_Head)
                    prpHead = value
                End Set
                Get
                    Return prpHead
                End Get
            End Property
            
            ''' <summary>
            ''' Detailタグ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Detail() As Root_Detail
                Set(ByVal value As Root_Detail)
                    prpDetail = value
                End Set
                Get
                    Return prpDetail
                End Get
            End Property
        End Class
#End Region
        
#End Region

    End Class
#End Region

End Namespace
