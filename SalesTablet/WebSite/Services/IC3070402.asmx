<%@ WebService Language="VB" Class="Toyota.eCRB.Estimate.Recommended.WebService.IC3070402" %>

'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3070402.asmx
'─────────────────────────────────────
'機能： 顧客属性取得IF プレゼンテーション
'補足： 
'作成： 2012/03/07 TCS 陳
'更新： 2013/06/30 TCS 武田 2013/10対応版　既存流用
'─────────────────────────────────────

Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols

Imports System.Xml
Imports System.Xml.Serialization
Imports System.Text
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Recommended.BizLogic
Imports Toyota.eCRB.Estimate.Recommended.DataAccess
Imports Microsoft.VisualBasic.Collection
Imports System.Collections.ObjectModel
Imports System.Reflection
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.Estimate.Recommended.WebService

    ' この Web サービスを、スクリプトから ASP.NET AJAX を使用して呼び出せるようにするには、次の行のコメントを解除します。
    ' <System.Web.Script.Services.ScriptService()> _
    <WebService(Namespace:="http://tempuri.org/")> _
    <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
    Public Class IC3070402
        Inherits System.Web.Services.WebService
    
#Region "定数"
    
        ''' <summary>
        ''' メッセージID
        ''' </summary>
        ''' <remarks>メッセージ識別コード(IC3070402) 顧客属性取得IF</remarks>
        Private Const MESSAGEID_CONST As String = "IC3070402"
    
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
        ''' 敬称前後
        ''' </summary>
        ''' <remarks>敬称前後</remarks>
        Private Const KEISYO_ZENGO As String = "KEISYO_ZENGO"
    
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
        Private Const TagCommon As String = "Common"
                
        ''' <summary>
        ''' 送信日付
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TransmissionDate_No As Short = 1

        ''' <summary>
        ''' 活動先顧客コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CrCustID_NO As Short = 201
        
        ''' <summary>
        ''' 販売店コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DlrCd_NO As Short = 202
        
        ''' <summary>
        ''' 店舗コード
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StrCd_NO As Short = 203
        
        ''' <summary>
        ''' Follow-up BOX内連番
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FllwupBox_SeqNo_NO As Short = 204
        
        ''' <summary>
        ''' 顧客種別
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CstKind_NO As Short = 205
        
        ''' <summary>
        ''' 敬称位置フラグ(前)        
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BeforeFLG As String = "1"
        
        ''' <summary>
        ''' 敬称位置フラグ(後)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AfterFLG As String = "2"
    
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
        ''' 送信日時（Request）
        ''' </summary>
        ''' <remarks>メッセージ送信日時(yyyyMMddHHmmss)</remarks>
        Private transmissionDate_ As Date
        
        ''' <summary>
        ''' 活動先顧客コード
        ''' </summary>
        ''' <remarks></remarks>
        Private CrCustID_ As String
        
        ''' <summary>
        ''' 活顧客種別
        ''' </summary>
        ''' <remarks></remarks>
        Private CSTKind_ As String
        
        ''' <summary>
        ''' 販売店コード
        ''' </summary>
        ''' <remarks></remarks>
        Private DlrCd_ As String
        
        ''' <summary>
        ''' 店舗コード
        ''' </summary>
        ''' <remarks></remarks>
        Private StrCd_ As String
        
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
        ''' <summary>
        ''' Follow-up BOX内連番
        ''' </summary>
        ''' <remarks></remarks>
        Private FllwupBox_SeqNo_ As Decimal
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
        
        ''' <summary>
        ''' 顧客種別
        ''' </summary>
        ''' <remarks></remarks>
        Private CstKindNo_ As Integer
        
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
        
        ''' <summary>
        ''' 活動先顧客コードプロパティ
        ''' </summary>
        ''' <value>活動先顧客コード</value>
        ''' <returns>活動先顧客コード</returns>
        ''' <remarks></remarks>
        Public Property CRCustId As String
            Get
                Return CrCustID_
            End Get
            Set(value As String)
                CrCustID_ = value
            End Set
        End Property
        
        ''' <summary>
        ''' 顧客種別ロパティ
        ''' </summary>
        ''' <value>顧客種別</value>
        ''' <returns>顧客種別</returns>
        ''' <remarks></remarks>
        Public Property CSTKind As String
            Get
                Return CSTKind_
            End Get
            Set(value As String)
                CSTKind_ = value
            End Set
        End Property
        
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
        ''' 店舗コードプロパティ
        ''' </summary>
        ''' <value>店舗コード</value>
        ''' <returns>店舗コード</returns>
        ''' <remarks></remarks>
        Public Property StrCD As String
            Get
                Return StrCd_
            End Get
            Set(value As String)
                StrCd_ = value
            End Set
        End Property
        
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
        ''' <summary>
        ''' Follow-up BOX内連番プロパティ
        ''' </summary>
        ''' <value>Follow-up BOX内連番</value>
        ''' <returns>Follow-up BOX内連番</returns>
        ''' <remarks></remarks>
        Public Property FllwupBox_SeqNo As Decimal
            Get
                Return FllwupBox_SeqNo_
            End Get
            Set(value As Decimal)
                FllwupBox_SeqNo_ = value
            End Set
        End Property
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
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
    
#Region "顧客属性取得Webサービス"
        ''' <summary>
        ''' 顧客属性取得Webサービス
        ''' </summary>
        ''' <param name="xsData">Request XML</param>
        ''' <returns>Response XML</returns>
        ''' <remarks></remarks>
        <WebMethod()> _
        Public Function GetRecommendedOption(ByVal xsData As String) As Response
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[Request XML :{2}]",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, xsData),
                                      True)
            ' ======================== ログ出力 終了 ========================
        
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
                
                '顧客属性取得処理
                Dim IC3070402BusinessLogic As New IC3070402BusinessLogic
  
                Try
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    Dim IC3070402DataSet As IC3070402DataSet = IC3070402BusinessLogic.GetCustAttribute(Me.CRCustId)
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                    
                    '終了コード取得
                    Me.ResultId = IC3070402BusinessLogic.ResultId
                    
                    'Responseクラスへの格納処理
                    Me.SetAttribute(IC3070402DataSet)

                Catch
                    Me.ResultId = ErrCodeSys
                    Throw
                Finally
                    IC3070402BusinessLogic = Nothing
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

                'エラーログ出力
                ' ======================== ログ出力 開始 ========================
                Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                           "ProgramID:[{0}], MessageID:[{1}]",
                                           IC3070402TableAdapter.FunctionId, Me.ResultId.ToString(CultureInfo.InvariantCulture)),
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
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, Me.ResultId.ToString(CultureInfo.InvariantCulture)),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
            Return ResponseObject
                        
        End Function
    
#End Region
    
#Region "Responseクラス格納処理"
        ''' <summary>
        '''顧客属性取得結果オブジェクトへの格納処理
        ''' </summary>
        ''' <param name="dsAttribute">取得結果データセット</param>
        ''' <remarks></remarks>
        Private Sub SetAttribute(ByVal dsAttribute As IC3070402DataSet)
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[IC3070402DataSet:{2}]",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dsAttribute)),
                                      True)
            ' ======================== ログ出力 終了 ========================

            'Try
            '活動先顧客コード(検索条件)
            ResponseObject.Detail.Attribute.CRCustId = Me.CRCustId
            
            '顧客名情報の設定
            If (dsAttribute.Tables("IC3070402CstName").Rows.Count > 0) Then
                Dim name As String = dsAttribute.Tables("IC3070402CstName").Rows(0).Item("NAME")
                Dim nametitle = String.Empty
                Dim dr As IC3070402DataSet.IC3070402CstNameRow = dsAttribute.Tables("IC3070402CstName").Rows(0)
                If (Not dr.IsNAMETITLENull) Then
                    nametitle = Trim(dsAttribute.Tables("IC3070402CstName").Rows(0).Item("NAMETITLE"))
                End If
                ResponseObject.Detail.Attribute.CRCustName = MakeCustomerTitle(name, nametitle)
            End If
                
            '顧客職業情報の設定
            If (dsAttribute.Tables("IC3070402Cstoccupation").Rows.Count > 0) Then
                ResponseObject.Detail.Attribute.OccupationNo = _
                    dsAttribute.Tables("IC3070402Cstoccupation").Rows(0).Item("OCCUPATIONNO")
                ResponseObject.Detail.Attribute.OccupationName = _
                    dsAttribute.Tables("IC3070402Cstoccupation").Rows(0).Item("OCCUPATION")
            End If

            '家族続柄情報設定
            If (dsAttribute.Tables("IC3070402Cstfamily").Rows.Count > 0) Then
                ResponseObject.Detail.Attribute.Families = Me.SetFamiliesInfo(dsAttribute.Tables("IC3070402Cstfamily"))
            End If
                
            '顧客趣味情報設定
            If (dsAttribute.Tables("IC3070402Csthobby").Rows.Count > 0) Then
                ResponseObject.Detail.Attribute.Hobbies = Me.SetHobbiesInfo(dsAttribute.Tables("IC3070402Csthobby"))
            End If
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
                                      True)
            ' ======================== ログ出力 終了 ========================
                    
        End Sub
        
        ''' <summary>
        ''' 顧客家族情報格納処理
        ''' </summary>
        ''' <param name="dtCstFamily">顧客家族情報</param>
        ''' <returns>顧客家族情報結果格納オブジェクト</returns>
        ''' <remarks></remarks>
        Private Function SetFamiliesInfo(ByVal dtCstFamily As IC3070402DataSet.IC3070402CstfamilyDataTable) As Root_Families
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[IC3070402CstfamilyDataTable:{2}]",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dtCstFamily)),
                                      True)
            ' ======================== ログ出力 終了 ========================
            Dim families = New Root_Families
            '家族情報の取得
            families.SetFamily(Me.SetFamilyInfo(dtCstFamily))
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End,[Root_Families:{2}]",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, families),
                                      True)
            ' ======================== ログ出力 終了 ========================
            Return families
        
        End Function
        
        ''' <summary>
        ''' 家族成員詳細情報格納処理
        ''' </summary>
        ''' <param name="dtCstfamily">顧客家族情報</param>
        ''' <returns>顧客家族情報格納オブジェクト</returns>
        ''' <remarks></remarks>
        Private Function SetFamilyInfo(ByVal dtCstfamily As IC3070402DataSet.IC3070402CstfamilyDataTable) As Collection(Of Root_Family)
           
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[IC3070402CstfamilyDataTable:{2}]",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dtCstfamily)),
                                      True)
            ' ======================== ログ出力 終了 ========================
            Dim arrFamily = New Collection(Of Root_Family)
            Dim i As Integer = 0
            
            For Each dr As IC3070402DataSet.IC3070402CstfamilyRow In dtCstfamily.Rows
                Dim FamilyInfo As New Root_Family
                arrFamily.Add(FamilyInfo)
                '家族続柄No
                arrFamily(i).FamilyrelationshipNo = dr.FAMILYRELATIONSHIPNO
                '続柄名称
                arrFamily(i).FamilyrelationshipName = dr.FAMILYRELATIONSHIP
                '生年月日 (yyyyMMdd)
                If (Not dr.IsBIRTHDAYNull) Then
                    arrFamily(i).Birthday = dr.BIRTHDAY.ToString(FormatDate, CultureInfo.InvariantCulture)
                End If

                i = i + 1
            Next
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End,[Root_Families:{2}]",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, arrFamily),
                                      True)
            ' ======================== ログ出力 終了 ========================
            Return arrFamily
        
        End Function
        
        ''' <summary>
        ''' 顧客趣味情報格納処理
        ''' </summary>
        ''' <param name="dtCstHobby">顧客趣味情報</param>
        ''' <returns>顧客趣味情報結果格納オブジェクト</returns>
        ''' <remarks></remarks>
        Private Function SetHobbiesInfo(ByVal dtCstHobby As IC3070402DataSet.IC3070402CsthobbyDataTable) As Root_Hobbies
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[IC3070402CsthobbyDataTable:{2}]",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dtCstHobby)),
                                      True)
            ' ======================== ログ出力 終了 ========================

            Dim hobbies = New Root_Hobbies
           
            '顧客趣味情報の取得
            hobbies.SetHobbyInfo(Me.SetHobbyInfo(dtCstHobby))
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End,[Root_Hobbies:{2}]",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, hobbies),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
            Return hobbies
        
        End Function
        
        ''' <summary>
        ''' 顧客趣味詳細情報格納処理
        ''' </summary>
        ''' <param name="dtCstHobby">顧客趣味詳細情報</param>
        ''' <returns>顧客趣味詳細情報格納オブジェクト</returns>
        ''' <remarks></remarks>
        Private Function SetHobbyInfo(ByVal dtCstHobby As IC3070402DataSet.IC3070402CsthobbyDataTable) As Collection(Of Root_Hobby)
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[IC3070402CsthobbyDataTable:{2}]",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dtCstHobby)),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
            Dim arrHobby = New Collection(Of Root_Hobby)
            Dim i As Integer = 0
            
            For Each dr As IC3070402DataSet.IC3070402CsthobbyRow In dtCstHobby.Rows
                Dim HobbyInfo As New Root_Hobby
                arrHobby.Add(HobbyInfo)
                '趣味No
                arrHobby(i).HobbyNo = CType(dr.HOBBYNO, String)
                '趣味名称
                arrHobby(i).HobbyName = CType(dr.HOBBY, String)
                
                i = i + 1
            Next
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End,[Root_Hobbies:{2}]",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, arrHobby),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
            Return arrHobby
        
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
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, xsData),
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
            Me.InitCommon()
            Me.SetCommon()
            xdoc = Nothing
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
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
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
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
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
                                      True)
            ' ======================== ログ出力 終了 ========================
            
        End Sub
        
        ''' <summary>
        ''' Conditionsタグ情報の初期化
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub InitCommon()
                      
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
                                      True)
            ' ======================== ログ出力 終了 ========================   
        
            ' 項目名称を設定
            Me.Itemname = {"CRCustID", "DlrCd", "StrCd", "FllwupBox_SeqNo", "CstKind"}
            
            ' 項目Noを設定
            Me.ItemNumber = {CrCustID_NO, DlrCd_NO, StrCd_NO, FllwupBox_SeqNo_NO, CstKind_NO}
            
            ' 必須必須フラグを設定
            Me.Chkrequiredflg = {CheckRequired, CheckRequired, CheckNoRequired, CheckNoRequired, CheckRequired}
            
            ' 項目属性を設定
            Me.Attribute = {AttributeLegth, AttributeLegth, AttributeLegth, AttributeNum, AttributeLegth}
            
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' 項目サイズを設定
            Me.Itemsize = {20, 5, 3, 20, 1}
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
               
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
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
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
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
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, Me.ResultId.ToString(CultureInfo.InvariantCulture)),
                                      True)
            ' ======================== ログ出力 終了 ========================

        End Sub
        
        ''' <summary>
        ''' 販売店コードのプロパティーセット
        ''' </summary>
        ''' <remarks>
        ''' XMLオブジェクトより、プロパティを設定します。
        ''' </remarks>
        Private Sub SetCommon()
            
                        
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                          "[{0}]_[{1}]_Start",
                          IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name),
                          True)
            ' ======================== ログ出力 終了 ========================   
            
            
            Dim nodeList As XmlNodeList         ' XMLノードリスト
            Dim nodeDocument As XmlDocument     ' XML要素
            Dim itemNo As Integer = 0
            Try
                ' XMLノードリスト取得
                nodeList = Me.RootElement.GetElementsByTagName(TagCommon)
                
                ' XML要素を設定
                nodeDocument = New XmlDocument
                nodeDocument.LoadXml(nodeList.ItemOf(0).OuterXml)
                Me.NodeElement = nodeDocument.DocumentElement
                    
                ' 顧客コードタグのNodeListを取得する 
                itemNo = 0
                Me.CRCustId = Me.GetElementValue(itemNo)
                ' 販売店コードタグのNodeListを取得する 
                itemNo = 1
                Me.DlrCD = Me.GetElementValue(itemNo)
                ' 店舗コードタグのNodeListを取得する 
                itemNo = 2
                Me.StrCD = Me.GetElementValue(itemNo)
                ' Follow-up BOX内連番タグのNodeListを取得する 
                itemNo = 3
                If (Not IsDBNull(Me.GetElementValue(itemNo))) Then
                    Me.FllwupBox_SeqNo = Me.GetElementValue(itemNo)
                End If
                
                ' 顧客種別タグのNodeListを取得する 
                itemNo = 4
                If (Not IsDBNull(Me.GetElementValue(itemNo))) Then
                    Me.CSTKind = Me.GetElementValue(itemNo)
                End If

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
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, Me.ResultId.ToString(CultureInfo.InvariantCulture)),
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
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, no),
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
                        ' 顧客種別の場合は、1(自社客)と2(未取引客)のみ許可チッェク
                        If no = 4 Then
                            If Not (String.Equals(valueString, "1") Or String.Equals(valueString, "2")) Then
                                Me.ResultId = ErrCodeItValue + Me.ItemNumber(no)
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
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, valueObj),
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
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, valueString, formatDate, errNumber),
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
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, Me.ResultId.ToString(CultureInfo.InvariantCulture)),
                                      True)
            ' ======================== ログ出力 終了 ========================    
            
        End Function
        
        ''' <summary>
        ''' 敬称付名前作成
        ''' </summary>
        ''' <param name="custName"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function MakeCustomerTitle(ByVal custName As String, ByVal nameTitle As String) As String
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_Start,[custName:{2}],[nameTitle:{3}]",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, custName, nameTitle),
                                      True)
            ' ======================== ログ出力 終了 ========================

            '敬称位置取得
            Dim sys As New SystemEnvSetting
            Dim sysPosition As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sys.GetSystemEnvSetting(KEISYO_ZENGO)

            '敬称位置
            Dim nameTitlePos As String
            If sysPosition Is Nothing Then
                nameTitlePos = BeforeFLG
            Else
                nameTitlePos = sysPosition.PARAMVALUE
            End If

            '敬称付き名前の組み立て
            Dim sb As New StringBuilder
            If nameTitlePos.Equals(BeforeFLG) Then
                If Not String.IsNullOrEmpty(nameTitle) Then
                    sb.Append(nameTitle)
                    sb.Append(" ")
                End If
            End If

            sb.Append(custName)

            If nameTitlePos.Equals(AfterFLG) Then
                If Not String.IsNullOrEmpty(nameTitle) Then
                    sb.Append(" ")
                    sb.Append(nameTitle)
                End If
            End If
            
            Dim resultValue As String = sb.ToString
            Return resultValue
            
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "[{0}]_[{1}]_End, resultValue :[{2}]",
                                      IC3070402TableAdapter.FunctionId, MethodBase.GetCurrentMethod.Name, resultValue.ToString(CultureInfo.InvariantCulture)),
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
            Private Attribute_ As Root_Attribute

            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                common_ = New Root_Commn
                Attribute_ = New Root_Attribute
            End Sub
    
            ''' <summary>
            ''' デストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Dispose()
        
                If common_ IsNot Nothing Then
                    common_ = Nothing
                End If
        
                If Attribute_ IsNot Nothing Then
                    Attribute_.Dispose()
                    Attribute_ = Nothing
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
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Attribute", IsNullable:=False)> _
            Public Property Attribute() As Root_Attribute
                Get
                    Return Attribute_
                End Get
                Set(value As Root_Attribute)
                    Attribute_ = value
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
        Public Class Root_Attribute
            Private crCustId_ As String                '活動先顧客コード
            Private crCustName_ As String              '顧客名
            Private occupationNo_ As String            '職業No
            Private occupationName_ As String          '職業名
            Private families_ As Root_Families         '顧客家族構成
            Private hobbies_ As Root_Hobbies           '顧客趣味
    
            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                crCustId_ = String.Empty
                crCustName_ = String.Empty
                occupationNo_ = String.Empty
                occupationName_ = String.Empty
                families_ = New Root_Families
                hobbies_ = New Root_Hobbies
            End Sub

            ''' <summary>
            ''' デストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub Dispose()
        
                If families_ IsNot Nothing Then
                    families_ = Nothing
                End If
                
                If hobbies_ IsNot Nothing Then
                    hobbies_ = Nothing
                End If
        
            End Sub
    
            ''' <summary>
            ''' 活動先顧客コードプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>活動先顧客コード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="CRCustID", IsNullable:=False)> _
            Public Property CRCustId As String
                Get
                    Return crCustId_
                End Get
                Set(value As String)
                    crCustId_ = value
                End Set
            End Property
            
            ''' <summary>
            ''' 顧客名プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>顧客名コード</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="CustName", IsNullable:=False)> _
            Public Property CRCustName As String
                Get
                    Return crCustName_
                End Get
                Set(value As String)
                    crCustName_ = value
                End Set
            End Property


            ''' <summary>
            ''' 職業Noプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>職業No</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="OccupationNo", IsNullable:=False)> _
            Public Property OccupationNo As String
                Get
                    Return occupationNo_
                End Get
                Set(value As String)
                    occupationNo_ = value
                End Set
            End Property
            
            ''' <summary>
            ''' 職業名プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>職業名</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="OccupationName", IsNullable:=False)> _
            Public Property OccupationName As String
                Get
                    Return occupationName_
                End Get
                Set(value As String)
                    occupationName_ = value
                End Set
            End Property
            
            ''' <summary>
            ''' Familiesクラスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>Familiesオブジェクト</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Families", IsNullable:=False)> _
            Public Property Families() As Root_Families
                Get
                    Return families_
                End Get
                Set(value As Root_Families)
                    families_ = value
                End Set
            End Property
            
            ''' <summary>
            ''' Familiesクラスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>Familiesオブジェクト</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Hobbies", IsNullable:=False)> _
            Public Property Hobbies() As Root_Hobbies
                Get
                    Return hobbies_
                End Get
                Set(value As Root_Hobbies)
                    hobbies_ = value
                End Set
            End Property
  
        End Class

        '''-----------------------------------------------------
        ''' <summary>
        ''' Root_Familiesクラス
        ''' </summary>
        ''' <remarks></remarks>
        '''-----------------------------------------------------
        Public Class Root_Families

            Private family_ As Collection(Of Root_Family)

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
        
                If family_ IsNot Nothing Then
                    family_ = Nothing
                End If
        
            End Sub
            
            ''' <summary>
            ''' Familiesクラスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>Familieオブジェクト</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Family", IsNullable:=False)> _
            Public ReadOnly Property Families() As Collection(Of Root_Family)
                Get
                    Return family_
                End Get
            End Property
            
            ''' <summary>
            ''' Familiesオブジェクト値格納処理
            ''' </summary>
            ''' <param name="value">Familiesオブジェクト</param>
            ''' <remarks></remarks>
            Public Sub SetFamily(ByVal value As Collection(Of Root_Family))
                family_ = value
            End Sub

        End Class
        
        '''-----------------------------------------------------
        ''' <summary>
        ''' Root_Familyクラス
        ''' </summary>
        ''' <remarks></remarks>
        '''-----------------------------------------------------
        Public Class Root_Family
            
            Private familyrelationshipNo_ As String       '家族続柄No
            Private familyrelationshipName_ As String     '続柄名称
            Private birthday_ As String                   '生年月日 (yyyyMMdd)

            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                familyrelationshipNo_ = String.Empty
                familyrelationshipName_ = String.Empty
                birthday_ = String.Empty
            End Sub
            
            ''' <summary>
            ''' 家族続柄Noプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>家族続柄No</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="FamilyrelationshipNo", IsNullable:=False)> _
            Public Property FamilyrelationshipNo As String
                Get
                    Return familyrelationshipNo_
                End Get
                Set(value As String)
                    familyrelationshipNo_ = value
                End Set
            End Property
            
            ''' <summary>
            ''' 続柄名称プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>続柄名称</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="FamilyrelationshipName", IsNullable:=False)> _
            Public Property FamilyrelationshipName As String
                Get
                    Return familyrelationshipName_
                End Get
                Set(value As String)
                    familyrelationshipName_ = value
                End Set
            End Property
            
            ''' <summary>
            ''' 生年月日プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>生年月日</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Birthday", IsNullable:=False)> _
            Public Property Birthday As String
                Get
                    Return birthday_
                End Get
                Set(value As String)
                    birthday_ = value
                End Set
            End Property
            
            
        End Class
        
        '''-----------------------------------------------------
        ''' <summary>
        ''' Root_Hobbiesクラス
        ''' </summary>
        ''' <remarks></remarks>
        '''-----------------------------------------------------
        Public Class Root_Hobbies

            Private hobby_ As Collection(Of Root_Hobby)

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
        
                If hobby_ IsNot Nothing Then
                    hobby_ = Nothing
                End If
        
            End Sub
            
            ''' <summary>
            ''' Familiesクラスプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>Familieオブジェクト</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Hobby", IsNullable:=False)> _
            Public ReadOnly Property Hobbies() As Collection(Of Root_Hobby)
                Get
                    Return hobby_
                End Get
            End Property
            
            ''' <summary>
            ''' Familiesオブジェクト値格納処理
            ''' </summary>
            ''' <param name="value">Familiesオブジェクト</param>
            ''' <remarks></remarks>
            Public Sub SetHobbyInfo(ByVal value As Collection(Of Root_Hobby))
                hobby_ = value
            End Sub

        End Class
        
        '''-----------------------------------------------------
        ''' <summary>
        ''' Root_Hobbyクラス
        ''' </summary>
        ''' <remarks></remarks>
        '''-----------------------------------------------------
        Public Class Root_Hobby
            
            Private hobbyNo_ As String       '趣味No
            Private hobbyName_ As String     '趣味名称
            
            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks></remarks>
            Public Sub New()
                '初期化処理
                hobbyNo_ = String.Empty
                hobbyName_ = String.Empty
            End Sub
            
            ''' <summary>
            ''' 趣味Noプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>趣味No</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="HobbyNo", IsNullable:=False)> _
            Public Property HobbyNo As String
                Get
                    Return hobbyNo_
                End Get
                Set(value As String)
                    hobbyNo_ = value
                End Set
            End Property
            
            ''' <summary>
            ''' 趣味名称プロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns>趣味名称</returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="HobbyName", IsNullable:=False)> _
            Public Property HobbyName As String
                Get
                    Return hobbyName_
                End Get
                Set(value As String)
                    hobbyName_ = value
                End Set
            End Property
            
        End Class
    
    End Class
    
End Namespace

