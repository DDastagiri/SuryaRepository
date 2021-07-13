'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250105.aspx.vb
'─────────────────────────────────────
'機能： 部品説明（交換部品情報）画面 コードビハインド
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
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.iCROP.BizLogic.SC3250105
Imports Toyota.eCRB.iCROP.DataAccess.SC3250105.SC3250105DataSet

''' <summary>
''' 部品説明画面
''' </summary>
''' <remarks></remarks>
Partial Class SC3250105
    Inherits BasePage

#Region "変数"

    ''' <summary>Getパラメーター格納</summary>
    Private Params As New Parameters

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

        ' 表示処理
        InitProc()

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

        '***パラメータを取得する
        GetParams()

        '***前回部品交換日時と前回部品交換時走行距離を表示する
        GetPreviosReplacement()

        ''***今回の走行距離を表示する
        'GetCarMileage()

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
    ''' 前回部品交換年月日及び走行距離の取得、表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetPreviosReplacement()

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START))

        Using biz As SC3250105BusinessLogic = New SC3250105BusinessLogic
            '前回部品交換年月日と走行距離を取得する
            Dim resultDataTable As PreviosReplacementDataTable _
                    = biz.GetPreviosReplacementData(Params.DealerCode, _
                                                    Params.R_O, _
                                                    Params.VIN_NO, _
                                                    Params.InspecItemCD, _
                                                    WebWordUtility.GetWord(3) _
                                                    )

            If resultDataTable IsNot Nothing AndAlso 0 < resultDataTable.Count Then
                '---取得できた

                '前回部品交換日時を表示
                If Not resultDataTable(0).IsREPLACED_DATETIMENull Then
                    If resultDataTable(0).REPLACED_DATETIME.ToString("yyyy/MM/dd") <> "1900/01/01" Then
                        LastChangeDate.InnerHtml = resultDataTable(0).REPLACED_DATETIME.ToString("yyyy/MM/dd")
                    End If
                End If

                '前回部品交換時走行距離を表示
                If Not resultDataTable(0).IsREG_MILENull Then
                    LastChangeMileage.InnerHtml = resultDataTable(0).REG_MILE
                End If

            Else
                '---取得できなかったときの処理

                '前回部品交換日時
                LastChangeDate.InnerHtml = ""

                '前回部品交換時走行距離
                LastChangeMileage.InnerHtml = ""

            End If

        End Using

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END))

    End Sub

#End Region

End Class
