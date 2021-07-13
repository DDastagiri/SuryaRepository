'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250104.aspx.vb
'─────────────────────────────────────
'機能： 部品説明（新旧コンテンツ表示）画面 コードビハインド
'補足： 
'作成： 2014/08/XX NEC 上野
'更新： 
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports System.Data
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.SystemEnvSettingDataSet
Imports Toyota.eCRB.iCROP.BizLogic.SC3250104
Imports Toyota.eCRB.iCROP.DataAccess.SC3250104
Imports Toyota.eCRB.iCROP.BizLogic.SC3250104.SC3250104WebServiceClassBusinessLogic_CreateXml

''' <summary>
''' 部品説明画面
''' </summary>
''' <remarks></remarks>
Partial Class SC3250104
    Inherits BasePage

#Region "変数"

    ''' <summary>Getパラメーター格納</summary>
    Private Params As New Parameters
    Private ApproveFlg As Boolean
    Private KeepKey As String
    ''' <summary>
    ''' ROActiveフラグ true:Activeに存在する False:Activeに存在しない
    ''' </summary>
    ''' <remarks></remarks>
    Private isRoActive As Boolean = True

#End Region

#Region "定数"

    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_START As String = "Start"

    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_END As String = "End"

    ''' <summary>
    ''' セッション名("DealerCode")
    ''' </summary>
    Private Const SessionDealerCode As String = "DealerCode"

    ''' <summary>
    ''' セッション名("BranchCode")
    ''' </summary>
    Private Const SessionBranchCode As String = "BranchCode"

    ''' <summary>
    ''' セッション名("LoginUserID")
    ''' </summary>
    Private Const SessionLoginUserID As String = "LoginUserID"

    ''' <summary>
    ''' セッション名("SAChipID")
    ''' </summary>
    Private Const SessionSAChipID As String = "SAChipID"

    ''' <summary>
    ''' セッション名("BASREZID")
    ''' </summary>
    Private Const SessionBASREZID As String = "BASREZID"

    ''' <summary>
    ''' セッション名("R_O")
    ''' </summary>
    Private Const SessionRO As String = "R_O"

    ''' <summary>
    ''' セッション名("SEQ_NO")
    ''' </summary>
    Private Const SessionSEQNO As String = "SEQ_NO"

    ''' <summary>
    ''' セッション名("VIN_NO")
    ''' </summary>
    Private Const SessionVINNO As String = "VIN_NO"

    ''' <summary>
    ''' セッション名("ViewMode")
    ''' </summary>
    Private Const SessionViewMode As String = "ViewMode"

    ''' <summary>
    ''' セッション名("商品訴求用部位コード")
    ''' </summary>
    Private Const SessionReqPartCD As String = "ReqPartCD"

    ''' <summary>
    ''' セッション名("点検項目コード")
    ''' </summary>
    Private Const SessionInspecItemCD As String = "InspecItemCD"

    ''' <summary>サービス送信時の日付フォーマット</summary>
    Private Const SERVICE_DATE_FORMAT As String = "dd/MM/yyyy HH:mm:ss"

    ''' <summary>
    ''' 写真選択画面でキャンセルボタンタップ時の戻り値
    ''' </summary>
    Private Const CANCEL_EVENT As String = "cancelled"

#End Region

#Region "クラス"

    ''' <summary>
    ''' Getパラメーター格納用クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class Parameters
        ''' <summary>販売店コード</summary>
        Public DealerCode As String
        ''' <summary>店舗コード</summary>
        Public BranchCode As String
        ''' <summary>ログインユーザID</summary>
        Public LoginUserID As String
        ''' <summary>SAChipID</summary>
        Public SAChipID As String
        ''' <summary>BASREZID</summary>
        Public BASREZID As String
        ''' <summary>R/O</summary>
        Public R_O As String
        ''' <summary>SEQ_NO</summary>
        Public SEQ_NO As String
        ''' <summary>VIN_NO</summary>
        Public VIN_NO As String
        ''' <summary>ViewMode 1=Readonly / 0=Edit</summary>
        Public ViewMode As String
        ''' <summary>ReqPartCD（商品訴求用部位コード）</summary>
        Public ReqPartCD As String
        ''' <summary>InspecItemCD（点検項目コード）</summary>
        Public InspecItemCD As String

    End Class

#End Region

#Region "イベントハンドラ"

    ''' <summary>
    ''' Page_Loadイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '***パラメータを取得する
        GetParams()

        '顧客承認判定
        If Params.R_O = "" Then
            'RO情報が空であるため、顧客承認前である
            ApproveFlg = False
            'hdnKeepKeyが存在する場合は写真選択後である
            If hdnKeepKey.Value = "" Then
                '顧客承認前はSAChipIDがキーとなる
                KeepKey = Params.SAChipID
                
            Else
                'hdnKeepKey情報を取得しセットする
                KeepKey = hdnKeepKey.Value

            End If
        Else

            '2017/XX/XX ライフサイクル対応　↓
            Using Biz As New SC3250104BusinessLogic

                '販売店コード、店舗コード、RO番号により、開いていたROがActiveであるか確認する。
                isRoActive = Biz.ChkExistParamRoActive(Params.DealerCode, Params.BranchCode, Params.R_O)

            End Using
            '2017/XX/XX ライフサイクル対応　↑

            'RO情報があるため、顧客承認後である
            ApproveFlg = True
            'hdnKeepKeyが存在する場合は写真選択後である
            If hdnKeepKey.Value = "" Then

                'SAChipID存在判定
                Using Biz As New SC3250104BusinessLogic
                    '顧客承認後はROに紐付くSAChipIDがあれば優先的にそちらをキーとする
                    '紐付くSAChipIDがない場合にはRO情報がキーになる
                    KeepKey = Biz.GetSAChipID(Params.R_O, Params.DealerCode, Params.BranchCode, isRoActive)

                End Using

            Else
                'hdnKeepKey情報を取得しセットする
                KeepKey = hdnKeepKey.Value

            End If
        End If

        '***ポストバックの場合の処理
        If IsPostBack Then
            '写真選択画面で選択した画像ファイルがあればデータベースに登録する
            RegisterFilePath()
        End If

        '***初期処理
        InitProc()

        '隠し項目へキーをセット
        hdnKeepKey.Value = KeepKey

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub


#End Region

#Region "ページ関連処理"

    ''' <summary>
    ''' 初期処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub InitProc()

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '***HiddenFieldの初期化
        hdnRegisterFile.Value = ""

        '***Oldパーツ写真とNewパーツ写真を表示する
        Using Biz As New SC3250104BusinessLogic

            'デフォルトのOldパーツ写真とNewパーツ写真を表示する
            Dim dtFileName As New SC3250104DataSet.DefaultPartsFileDataTable
            dtFileName = Biz.GetDefaultPartsFileNameData(Params.R_O, Params.InspecItemCD, Params.DealerCode, Params.BranchCode, ApproveFlg, Params.SAChipID, KeepKey, isRoActive)

            If dtFileName IsNot Nothing AndAlso 0 < dtFileName.Count Then
                'データベースより取得できた
                'Oldパーツの写真を表示する
                If Not String.IsNullOrWhiteSpace(dtFileName(0).SEL_PICTURE_URL) Then
                    '選択した写真ファイルパスが取得できた
                    imageOldParts.ImageUrl = ResolveUrl(dtFileName(0).SEL_PICTURE_URL)
                    'Logger.Info("★imageOldParts.ImageUrl:" & ResolveUrl(dtFileName(0).SEL_PICTURE_URL))
                ElseIf Not String.IsNullOrWhiteSpace(dtFileName(0).OLD_PARTS_FILE_NAME) Then
                    'Oldパーツのデフォルトファイルパスが取得できた
                    imageOldParts.ImageUrl = ResolveUrl(dtFileName(0).OLD_PARTS_FILE_NAME)
                    'Logger.Info("★imageOldParts.ImageUrl:" & ResolveUrl(dtFileName(0).OLD_PARTS_FILE_NAME))
                Else
                    imageOldParts.ImageUrl = ""
                End If

                'Newパーツの写真を表示する
                If Not String.IsNullOrWhiteSpace(dtFileName(0).NEW_PARTS_FILE_NAME) Then
                    'Newパーツのファイルパスが取得できた
                    imageNewParts.ImageUrl = ResolveUrl(dtFileName(0).NEW_PARTS_FILE_NAME)
                    'Logger.Info("★imageNewParts.ImageUrl:" & ResolveUrl(dtFileName(0).NEW_PARTS_FILE_NAME))
                End If

            End If

            dtFileName.Dispose()

        End Using

        '***写真枚数を取得する
        'カメラアイコンの非活性化
        ThumbnailCount.Attributes.Add("class", "PartsIcon_Disable")
        ThumbnailCount.InnerHtml = ""

        'RO番号があれば写真枚数を取得とリンク作成する
        'If Not String.IsNullOrWhiteSpace(Params.R_O) Then
        Dim sendxml_RoThumbnailCount As RoThumbnailCountXmlDocumentClass = CreateXMLOfRoThumbnailCount(SC3250104WebServiceClassBusinessLogic.GetRoThumbnailCount_Info.WebServiceIDValue)

        Using BizSrv As New SC3250104WebServiceClassBusinessLogic

            '写真枚数の取得
            Dim retxml_RoThumbnailCount As SC3250104DataSet.RoThumbnailCountDataTable = BizSrv.CallGetRoThumbnailCountWebService(sendxml_RoThumbnailCount)
            If retxml_RoThumbnailCount IsNot Nothing AndAlso 0 < retxml_RoThumbnailCount.Rows.Count Then
                If String.IsNullOrEmpty(retxml_RoThumbnailCount.Rows(0)("RoThumbnailCount").ToString) = False Then
                    If 0 < Integer.Parse(retxml_RoThumbnailCount.Rows(0)("RoThumbnailCount").ToString) Then
                        '写真があった
                        If isRoActive Then
                            ThumbnailCount.Attributes.Add("class", "PartsIcon")
                        End If

                        ThumbnailCount.InnerHtml = retxml_RoThumbnailCount.Rows(0)("RoThumbnailCount").ToString
                    End If
                End If
            End If


            '// 写真選択ポップアップ画面用パラメータ作成
            Dim Target As StringBuilder = New StringBuilder
            With Target
                .AppendFormat("?DealerCode={0}", Params.DealerCode)
                .AppendFormat("&BranchCode={0}", Params.BranchCode)
                .AppendFormat("&SAChipID={0}", Params.SAChipID)
                .AppendFormat("&BASREZID={0}", Params.BASREZID)
                .AppendFormat("&R_O={0}", Params.R_O)
                .Append("&SEQ_NO=0")
                .AppendFormat("&VIN_NO={0}", Params.VIN_NO)
                .Append("&PictMode=1")
                .Append("&ViewMode=1")
                .Append("&LinkSysType=1")
                .AppendFormat("&LoginUserID={0}", Params.LoginUserID)
            End With

            Dim cameraUrl As String = Target.ToString

            Logger.Info(String.Format("CameraURL:[{0}]", cameraUrl))

            If retxml_RoThumbnailCount IsNot Nothing AndAlso 0 < retxml_RoThumbnailCount.Rows.Count Then
                If String.IsNullOrEmpty(retxml_RoThumbnailCount.Rows(0)("RoThumbnailCount").ToString) = False Then
                    If 0 < Integer.Parse(retxml_RoThumbnailCount.Rows(0)("RoThumbnailCount").ToString) Then
                        If isRoActive Then
                            ThumbnailCount.Attributes.Add("onclick", String.Format("ShowUrlSchemeNoTitlePopup('{0}');", cameraUrl))
                        End If
                    End If
                End If
            End If

            'テスト
            'ThumbnailCount.Attributes.Add("onclick", String.Format("ShowUrlSchemeNoTitlePopup('{0}');", cameraUrl))

        End Using

        'End If

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' パラメータを取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetParams()

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))


        '販売店コード、店舗コード、ログインIDは基盤から取得するためコメント化
        ''販売店コード(DealerCode)
        'If Me.ContainsKey(ScreenPos.Current, SessionDealerCode) Then
        '    Params.DealerCode = DirectCast(GetValue(ScreenPos.Current, SessionDealerCode, False), String)
        'End If

        ''店舗コード(BranchCode)
        'If Me.ContainsKey(ScreenPos.Current, SessionBranchCode) Then
        '    Params.BranchCode = DirectCast(GetValue(ScreenPos.Current, SessionBranchCode, False), String)
        'End If

        ''ログインID(LoginUserID)
        'If Me.ContainsKey(ScreenPos.Current, SessionLoginUserID) Then
        '    Params.LoginUserID = DirectCast(GetValue(ScreenPos.Current, SessionLoginUserID, False), String)
        'End If

        '来店実績連番(SAChipID)
        If Me.ContainsKey(ScreenPos.Current, SessionSAChipID) Then
            Params.SAChipID = DirectCast(GetValue(ScreenPos.Current, SessionSAChipID, False), String)
        End If

        'DMS予約ID（BASREZID）
        If Me.ContainsKey(ScreenPos.Current, SessionBASREZID) Then
            Params.BASREZID = DirectCast(GetValue(ScreenPos.Current, SessionBASREZID, False), String)
        End If

        'RO番号（R_O）
        If Me.ContainsKey(ScreenPos.Current, SessionRO) Then
            Params.R_O = DirectCast(GetValue(ScreenPos.Current, SessionRO, False), String)
        End If

        'RO作業連番（SEQ_NO）
        If Me.ContainsKey(ScreenPos.Current, SessionSEQNO) Then
            Params.SEQ_NO = DirectCast(GetValue(ScreenPos.Current, SessionSEQNO, False), String)
        End If

        'VIN（VIN_NO）
        If Me.ContainsKey(ScreenPos.Current, SessionVINNO) Then
            Params.VIN_NO = DirectCast(GetValue(ScreenPos.Current, SessionVINNO, False), String)
        End If

        '編集モード（ViewMode）
        If Me.ContainsKey(ScreenPos.Current, SessionViewMode) Then
            Params.ViewMode = DirectCast(GetValue(ScreenPos.Current, SessionViewMode, False), String)
        End If

        '商品訴求用部位コード（ReqPartCD）
        If Me.ContainsKey(ScreenPos.Current, SessionReqPartCD) Then
            Params.ReqPartCD = DirectCast(GetValue(ScreenPos.Current, SessionReqPartCD, False), String)
        End If

        '点検項目コード（InspecItemCD）
        If Me.ContainsKey(ScreenPos.Current, SessionInspecItemCD) Then
            Params.InspecItemCD = DirectCast(GetValue(ScreenPos.Current, SessionInspecItemCD, False), String)
        End If

        '販売店コード、店舗コード、店舗コードは基盤から情報を取得する

        Dim staffInfo As StaffContext = StaffContext.Current

        If String.IsNullOrWhiteSpace(Params.DealerCode) Then
            Params.DealerCode = staffInfo.DlrCD
        End If
        If String.IsNullOrWhiteSpace(Params.BranchCode) Then
            Params.BranchCode = staffInfo.BrnCD
        End If
        If String.IsNullOrWhiteSpace(Params.LoginUserID) Then
            Params.LoginUserID = staffInfo.Account
        End If

        'ユーザーIDに@が無ければ、「スタッフ識別文字列 + "@" + 販売店コード」の形にする
        If Not Params.LoginUserID.Contains("@") Then
            Params.LoginUserID = String.Format("{0}@{1}", Params.LoginUserID, Params.DealerCode)
        End If


        '***取得したパラメータ情報をログに記録
        Logger.Error(String.Format("Params:DealerCode:[{0}], BranchCode:[{1}], LoginUserID:[{2}], SAChipID:[{3}], BASREZID:[{4}], R_O:[{5}], SEQ_NO:[{6}], VIN_NO:[{7}], ViewMode:[{8}]", _
                                  Params.DealerCode, _
                                  Params.BranchCode, _
                                  Params.LoginUserID, _
                                  Params.SAChipID, _
                                  Params.BASREZID, _
                                  Params.R_O, _
                                  Params.SEQ_NO, _
                                  Params.VIN_NO, _
                                  Params.ViewMode))

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

    ''' <summary>
    ''' 写真選択画面で選択した画像を登録する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub RegisterFilePath()

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        '画像ファイル名が登録されていなかったら処理を終了する
        If hdnRegisterFile.Value = "" Then
            Exit Sub
        End If

        '2014/09/30　写真選択画面でキャンセルボタンタップ時に写真をデフォルトに戻す仕様追加　START　↓↓↓
        '画像ファイルの登録・削除処理
        Using Biz As New SC3250104BusinessLogic

            If hdnRegisterFile.Value = CANCEL_EVENT Then
                '画像ファイルの削除処理
                Biz.DeleteSelectedPartsFileName(KeepKey, Params.DealerCode, Params.BranchCode, Params.InspecItemCD)

            Else
                'パスを変換する（テスト用）
                'hdnRegisterFile.Value = "../" & hdnRegisterFile.Value

                '画像ファイルの登録処理
                Biz.RegistSelectedPartsFileName(Params.R_O _
                                                , hdnRegisterFile.Value _
                                                , Params.LoginUserID _
                                                , Params.DealerCode _
                                                , Params.BranchCode _
                                                , Params.InspecItemCD _
                                                , ApproveFlg _
                                                , KeepKey)

            End If

        End Using
        '2014/09/30　写真選択画面でキャンセルボタンタップ時に写真をデフォルトに戻す仕様追加　END　　↑↑↑

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

#End Region

#Region "Webサービス関連"

#Region "XML作成 RoThumbnailCount"
    ''' <summary>
    ''' XML作成(HeadTag)
    ''' </summary>
    ''' <returns>XMLドキュメントクラス</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateXMLOfRoThumbnailCount(ByVal WebServiceID As String) As RoThumbnailCountXmlDocumentClass
        Dim inXmlClass As New RoThumbnailCountXmlDocumentClass

        Logger.Info(String.Format("CreateXMLOfRoThumbnailCount_Start, WebServiceID:[{0}]", WebServiceID))

        'メッセージID
        inXmlClass.Head.MessageId = WebServiceID

        '国コード
        inXmlClass.Head.CountryCode = EnvironmentSetting.CountryCode

        '基幹SYSTEM識別コード(0固定)
        inXmlClass.Head.LinkSystemCode = "0"

        'TansmissionDate
        inXmlClass.Head.TransmissionDate = Format(DateTime.Now, SERVICE_DATE_FORMAT).ToString

        CreateDetailOfMileage(inXmlClass, "0")

        Logger.Info("CreateXMLOfRoThumbnailCount_End")

        Return inXmlClass
    End Function

    ''' <summary>
    ''' XML作成(DetailTag)
    ''' </summary>
    ''' <param name="sendXml">XML Template</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Sub CreateDetailOfMileage(ByRef sendXml As RoThumbnailCountXmlDocumentClass, _
                                      Optional ByVal roSeq As String = "")

        Dim Cmn As New RoThumbnailCountXmlDocumentClass.DetailTag.CommonTag

        '写真枚数取得Service時は"0"固定対応
        If roSeq = "" Then
            roSeq = Params.SEQ_NO
        End If

        Cmn.SAChipID = Params.SAChipID
        Cmn.DealerCode = Params.DealerCode
        Cmn.BranchCode = Params.BranchCode
        Cmn.R_O = Params.R_O
        'Cmn.R_O_SEQNO = Params.SEQ_NO
        Cmn.R_O_SEQNO = roSeq
        Cmn.PictMode = "1" '写真区分(1:追加作業（規定値）、2:外観チェック)
        Cmn.LinkSysType = "1" 'SYSTEM連携種別(1：基幹販売店/店舗コード(規定値)、0:iCROP販売店/店舗コード)
        sendXml.Detail.Common = Cmn

    End Sub
#End Region

#End Region

End Class

