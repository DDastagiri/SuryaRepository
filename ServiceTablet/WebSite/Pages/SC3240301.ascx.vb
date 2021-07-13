'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240301.ascx.vb
'─────────────────────────────────────
'機能： サブチップエリア
'補足： 
'作成： 2012/12/28 TMEJ 丁 タブレット版SMB機能開発(工程管理)
'更新： 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
'更新： 2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応
'更新： 2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）
'更新： 2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
'更新： 2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発
'更新： 2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化)
'更新： 2015/02/16 TMEJ 小澤 UAT-BTS-168 洗車完了通知がされているのに洗車完了されていない不具合修正
'更新： 2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発
'更新： 2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力
'更新： 2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化
'更新： 2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
'更新： 2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/07/19 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新： 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新： 
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Data
Imports System.Web.Script.Serialization
Imports System.Reflection
Imports Toyota.eCRB.SMB.SubChipBox.BizLogic
Imports Toyota.eCRB.SMB.SubChipBox.DataAccess
Imports Toyota.eCRB.CommonUtility.Common.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic.TabletSMBCommonClassBusinessLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess
'2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 START
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
'2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 START

Partial Class Pages_SC3240301
    Inherits System.Web.UI.UserControl
    Implements ICallbackEventHandler
#Region "定数"

    ''' <summary>
    ''' 自画面のページID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MY_PROGRAMID As String = "SC3240301"

    ''' <summary>
    ''' セッションキー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CUST_CONFIRMDATE As String = "CUST_CONFIRMDATE"                  'お客承認日時


    ''' <summary>
    ''' 予期外のエラーコード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ResultCode_TimeOut As Short = 901                 'データベースダイムアウト
    Private Const ResultCode_ExceptionError As Short = 902          '予期せぬエラー

    ''' <summary>
    ''' フッターボタンID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_RECEPTION As String = "100"                 '受付ボタン
    Private Const CBACK_ADDITIONALWORK As String = "200"            '追加作業ボタン
    Private Const CBACK_COMPLETIONINSPECTION As String = "300"      '完成検査ボタン
    Private Const CBACK_CARWASH As String = "400"                   '洗車ボタン
    Private Const CBACK_DELIVERDCAR As String = "500"               '納車待ちボタン
    Private Const CBACK_NOSHOW As String = "600"                    'NoShowボタン
    Private Const CBACK_STOP As String = "700"                      '中断ボタン
    Private Const CBACK_NOSHOW_MOVING As String = "602"             'NoShowエリアからストールに配置イベント
    Private Const CBACK_STOP_MOVING As String = "702"               '中断エリアからストールに配置イベント
    Private Const CBACK_ACTION_CANCEL As String = "2500"            'キャンセル
    Private Const CBACK_ACTION_WASHSTART As String = "2000"         '洗車開始
    Private Const CBACK_ACTION_WASHEND As String = "2100"           '洗車終了
    Private Const CBACK_ACTION_DELIVERY As String = "2200"          '納車
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    Private Const CBACK_ACTION_UNDO As String = "2600"              'undo 
    Private Const CBACK_ACTION_MOVETOWASH As String = "2900"          '洗車へ移動
    Private Const CBACK_ACTION_MOVETODELIWAIT As String = "3000"          '納車待ちへ移動
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
    '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
    Private Const CBACK_ACTION_FINISH_STOP_CHIP As String = "3200"       '中断終了
    '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END
    ''' <summary>
    ''' 受付ボタンの状態最新表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_REFRESH_RECEPTION As String = "101"
    ''' <summary>
    ''' 追加作業ボタンの状態最新表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_REFRESH_ADDITIONALWORK As String = "201"
    ''' <summary>
    ''' 完成検査ボタンの状態最新表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_REFRESH_COMPLETIONINSPECTIO As String = "301"
    ''' <summary>
    ''' 洗車ボタンの状態最新表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_REFRESH_CARWASH As String = "401"
    ''' <summary>
    ''' 納車待ちボタンの状態最新表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_REFRESH_DELIVERDCAR As String = "501"
    ''' <summary>
    ''' Noshowボタンの状態最新表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_REFRESH_NOSHOW As String = "601"
    ''' <summary>
    ''' 中断ボタンの状態最新表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_REFRESH_STOP As String = "701"
    ''' <summary>
    ''' 全ボタンの状態最新表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_REFRESH_ALLCONUT As String = "1000"
    ''' <summary>
    ''' 受け付けタブ(予約紐付け)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CBACK_RESERVE_ATTCHMENT As String = "102"

    ''' <summary>
    ''' バリデーションチェックで使用する正規表現パターン(hh:mm)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PATTERN_TIME As String = "^([01]?[0-9]|2[0-3]):([0-5][0-9])$"

#Region "文言ID"
    ''' <summary>
    ''' 文言ID管理
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum WordID
        ''' <summary>データベースとの接続でタイムアウトが発生しました。再度処理を行ってください。</summary>
        id001 = 901
        ''' <summary>予期せぬエラーが発生しました。画面を再表示してから再度処理を行ってください。</summary>
        id002 = 902
        ''' <summary>他のチップと配置時間が重複します。</summary>
        id003 = 903
        ''' <summary>そのチップは、既に他のユーザーによって変更が加えられています。画面を再表示してから再度処理を行ってください。</summary>
        id004 = 904
        ''' <summary>該当ストールに対して、他のユーザーが変更を行なっています。時間を置いて再度処理を行ってください。</summary>
        id005 = 905
        ''' <summary>選択チップは追加作業が完了していないため、(%1)できません。</summary>
        id006 = 906
        ''' <summary>営業開始時間(%1:%2)以降に配置してください。</summary>
        id007 = 907
        ''' <summary>営業終了時間(%1:%2)以内に配置してください。</summary>
        id008 = 908
        ''' <summary>選択したチップを削除しますか？</summary>
        id009 = 909
        ''' <summary>1つのチップにR/Oと追加作業、または複数の追加作業を紐づけることはできません。</summary>
        id010 = 910
        ''' <summary>他システムとの連携に失敗したため、情報は更新されませんでした。画面を再表示し、再度処理を行なってください。</summary>
        id011 = 911

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ''' <summary>清算準備が完了していないため、納車できません。</summary>
        id012 = 912
        ''' <summary>他システムとの連携時に、タイムアウトが発生しました。画面を再表示し、状況が改善されない場合はシステム管理者に連絡してください。</summary>
        id013 = 913
        ''' <summary>他システムとの連携時に、他システム側でエラーが発生しました。他システムの管理者に連絡してください。</summary>
        id014 = 914
        ''' <summary>他システムとの連携時にエラーが発生しました。システム管理者に連絡してください。</summary>
        id015 = 915
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
    End Enum
#End Region

#End Region

#Region "メンバ変数"
    ''' <summary>
    ''' コールバックメソッドの呼び出し元に返却する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private subchipDataJson As String
    ''' <summary>
    ''' ストール開始時間
    ''' </summary>
    ''' <remarks></remarks>
    Private m_strStallStartTime As String = "08:00"
    ''' <summary>
    ''' ストール終了時間
    ''' </summary>
    ''' <remarks></remarks>
    Private m_strStallEndTime As String = "23:00"

    '2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 START
    Private LogServiceCommonBiz As New ServiceCommonClassBusinessLogic(True)
    '2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 END

#End Region

#Region "ページクラス処理のバイパス処理"

    ''' <summary>
    ''' セッション情報から値を取得します。
    ''' </summary>
    ''' <param name="pos"></param>
    ''' <param name="key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ContainsKey(ByVal pos As ScreenPos, ByVal key As String) As Boolean

        Return GetPageInterface().ContainsKeyBypass(pos, key)

    End Function

    ''' <summary>
    ''' セッション情報から値を取得します。
    ''' </summary>
    ''' <param name="pos"></param>
    ''' <param name="key"></param>
    ''' <param name="removeFlg"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object

        Return GetPageInterface().GetValueCommonBypass(pos, key, removeFlg)

    End Function

    ''' <summary>
    ''' セッション情報に値を設定します。
    ''' </summary>
    ''' <param name="pos"></param>
    ''' <param name="key"></param>
    ''' <param name="val"></param>
    ''' <remarks></remarks>
    Private Sub SetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal val As Object)

        GetPageInterface().SetValueCommonBypass(pos, key, val)

    End Sub

    ''' <summary>
    ''' 顧客詳細画面のインターフェイスを取得します。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPageInterface() As ICommonSessionControl

        Return CType(Me.Page, ICommonSessionControl)

    End Function
#End Region

#Region "画面ロード"

    ''' <summary>
    ''' 画面ロードの処理
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'コールバックスクリプトの生成
        ScriptManager.RegisterStartupScript(
            Me,
            Me.GetType(),
            "gCallbackSC3240301",
            String.Format(CultureInfo.InvariantCulture,
                          "gCallbackSC3240301.beginCallback = function () {{ {0}; }};",
                          Page.ClientScript.GetCallbackEventReference(Me, "gCallbackSC3240301.packedArgument", _
                                                                      "gCallbackSC3240301.endCallback", "", True)
                          ),
            True
        )

        If Not Me.Page.IsCallback AndAlso Not Me.Page.IsPostBackEventControlRegistered Then
            'hiddenコントロールにclient端用の文言を設定する
            Me.SendWordToClient()

            'フッターボタンにチップ数と遅れ状態をhiddenコントロールに設定する
            Me.hidJsonDatasubchip.Value = GetInitializationButtonInfo()
            '受付ボタン点滅のためのセッションCUST_CONFIRMDATE設定
            Dim userContext As StaffContext = StaffContext.Current
            If Me.ContainsKey(ScreenPos.Current, CUST_CONFIRMDATE) Then
                'セッションに存在であれば、hiddenコントロールに値を設定する
                Me.hiddenupdatetime.Value = CType(GetValue(ScreenPos.Current, "CUST_CONFIRMDATE", False), String)

            Else
                'セッションに存在しない場合は現在時間を設定する
                SetValue(ScreenPos.Current, "CUST_CONFIRMDATE", DateTimeFunc.Now(userContext.DlrCD))
                Me.hiddenupdatetime.Value = CType(DateTimeFunc.Now(userContext.DlrCD), String)
            End If
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

    End Sub
#End Region

#Region "データ取得メソッド"

#Region "受付チップ情報の取得処理"
    ''' <summary>
    ''' 受付チップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetReceptionChip(ByVal dlrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            'Dim dtChipInfo As SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable = blSC3240301.GetReceptionChipData(dlrCode, brnCode)
            Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetReceptionChipData(dlrCode, brnCode)
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

            '最新のチップ情報を取れたので点滅を消すため、セッションパラメタを現在時間を設定する
            If ContainsKey(ScreenPos.Current, "CUST_CONFIRMDATE") Then
                SetValue(ScreenPos.Current, "CUST_CONFIRMDATE", DateTimeFunc.Now(dlrCode))
            End If
            '受信したデータをJSON形式に変換する
            subchipDataJson = Me.DataTableToJson(dtChipInfo)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        End Using
    End Sub
#End Region

#Region "追加作業チップ情報の取得処理"
    ''' <summary>
    ''' 追加作業チップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetAddWorkChip(ByVal dlrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            'Dim dtChipInfo As SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable
            'dtChipInfo = blSC3240301.GetAddWorkChipData(dlrCode, brnCode)
            Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetAddWorkChipData(dlrCode, brnCode)
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
            '受信したデータをJSON形式に変換する
            subchipDataJson = Me.DataTableToJson(dtChipInfo)
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub
#End Region

#Region "完成検査チップ情報の取得処理"
    ''' <summary>
    ''' 完成検査チップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetCompletedInspectionChip(ByVal dlrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable
            dtChipInfo = blSC3240301.GetCompletedInspectionChipData(dlrCode, brnCode)
            '受信したデータをJSON形式に変換する
            subchipDataJson = Me.DataTableToJson(dtChipInfo)
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub
#End Region

#Region "洗車チップ情報の取得処理"
    ''' <summary>
    ''' 洗車チップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetCarWashChip(ByVal dlrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            'サブチップの最新状態を取得する
            Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable
            dtChipInfo = blSC3240301.GetCarWashChipData(dlrCode, brnCode)
            '受信したデータをJSON形式に変換する
            subchipDataJson = Me.DataTableToJson(dtChipInfo)
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub
#End Region

#Region "納車待ちチップ情報の取得処理"
    ''' <summary>
    ''' 納車待ちチップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetDeliverdChip(ByVal dlrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
          , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            'サブチップの最新状態を取得する
            Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = _
                blSC3240301.GetDeliverdChipData(dlrCode, brnCode)
            '受信したデータをJSON形式に変換する
            subchipDataJson = Me.DataTableToJson(dtChipInfo)
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub
#End Region

#Region "中断チップ情報の取得処理"
    ''' <summary>
    ''' 中断チップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetStopChip(ByVal dlrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
          , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            'サブチップの最新状態を取得する
            Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetStopChipData(dlrCode, brnCode)
            '受信したデータをJSON形式に変換する
            subchipDataJson = Me.DataTableToJson(dtChipInfo)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        End Using
    End Sub
#End Region

#Region "NoShowチップ情報の取得処理"
    ''' <summary>
    ''' NoShowチップ情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetNoShowChip(ByVal dlrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            'サブチップの最新状態を取得する
            Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetNoShowChipData(dlrCode, brnCode)
            '受信したデータをJSON形式に変換する
            subchipDataJson = Me.DataTableToJson(dtChipInfo)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        End Using
    End Sub
#End Region

#Region "初期化時ボタンの情報の取得処理"
    ''' <summary>
    ''' 初期化時ボタンの情報の取得処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetInitializationButtonInfo() As String
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim allAreaCountinfo As String = ""
        Dim userContext As StaffContext = StaffContext.Current
        Dim dlrCd As String = userContext.DlrCD
        Dim brnCd As String = userContext.BrnCD

        '各エリアのチップカウンターを取得する
        Dim dtDeliverdChip As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationDeliverdButtonInfo(dlrCd, brnCd)
        Dim dtCarWashChip As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationCarWashButtonInfo(dlrCd, brnCd)
        Dim dtComplInsChip As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationComplInsButtonInfo(dlrCd, brnCd)
        Dim dtAddworkChip As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationADDWorkButtonInfo(dlrCd, brnCd)
        Dim dtReceptionChip As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationReceptionButtonInfo(dlrCd, brnCd)
        Dim dtNoShowChip As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationNoShowButtonInfo(dlrCd, brnCd)
        Dim dtStopChip As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationStopButtonInfo(dlrCd, brnCd)

        Using dtUnionChip As New SC3240301DataSet.SC3240301ChipCountDataTable
            '全てのエリアをまとめにする
            dtUnionChip.Merge(dtDeliverdChip)
            dtUnionChip.Merge(dtCarWashChip)
            dtUnionChip.Merge(dtComplInsChip)
            dtUnionChip.Merge(dtAddworkChip)
            dtUnionChip.Merge(dtReceptionChip)
            dtUnionChip.Merge(dtNoShowChip)
            dtUnionChip.Merge(dtStopChip)
            'データをJSON形式に変換する
            allAreaCountinfo = Me.DataTableToJson(dtUnionChip)
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return allAreaCountinfo

    End Function
#End Region

#Region "納車待ちボタンの情報の取得処理"
    ''' <summary>
    ''' 納車待ちボタンの情報を取得し、JSON形式の文字列データを格納する処理.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetInitializationDeliverdButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            Dim userContext As StaffContext = StaffContext.Current
            '納車ボタンの情報を取得する
            Dim dtChipInfo As SC3240301DataSet.SC3240301ChipCountDataTable = blSC3240301.GetDeliverdButtonInfo(dlrCode, brnCode)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return dtChipInfo
        End Using
    End Function
#End Region

#Region "洗車ボタンの情報の取得処理"
    ''' <summary>
    ''' 洗車ボタンの情報を取得.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetInitializationCarWashButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            Dim userContext As StaffContext = StaffContext.Current
            '洗車ボタンの情報を取得する
            Dim dtChipCntInfo As SC3240301DataSet.SC3240301ChipCountDataTable = blSC3240301.GetCarWashButtonInfo(dlrCode, brnCode)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return dtChipCntInfo
        End Using
    End Function
#End Region

#Region "完成検査ボタンの情報の取得処理"
    ''' <summary>
    ''' 完成検査ボタンの情報を取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetInitializationComplInsButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
          , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            Dim userContext As StaffContext = StaffContext.Current
            '完成検査ボタンの情報を取得する
            Dim dtChipCntInfo As SC3240301DataSet.SC3240301ChipCountDataTable = blSC3240301.GetCompletedInspectionButtonInfo(dlrCode, brnCode)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return dtChipCntInfo
        End Using
    End Function
#End Region

#Region "追加作業ボタンの情報の取得処理"
    ''' <summary>
    ''' 追加作業ボタンの情報を取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetInitializationADDWorkButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            Dim userContext As StaffContext = StaffContext.Current
            '追加作業ボタンの情報を取得する
            Dim dtChipCntInfo As SC3240301DataSet.SC3240301ChipCountDataTable = blSC3240301.GetAddWorkButtonInfo(dlrCode, brnCode)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return dtChipCntInfo
        End Using
    End Function
#End Region

#Region "受付ボタンの情報の取得処理"
    ''' <summary>
    ''' 受付ボタンの情報を取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetInitializationReceptionButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            Dim userContext As StaffContext = StaffContext.Current
            '受付ボタンの情報を取得する
            Dim dtChipCntInfo As SC3240301DataSet.SC3240301ChipCountDataTable = blSC3240301.GetReceptionButtonInfo(dlrCode, brnCode)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return dtChipCntInfo
        End Using
    End Function
#End Region

#Region "NoShowボタンの情報の取得処理"
    ''' <summary>
    ''' NoShowボタンの情報を取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetInitializationNoShowButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            Dim userContext As StaffContext = StaffContext.Current
            'NoShowボタンの情報を取得する
            Dim dtChipCntInfo As SC3240301DataSet.SC3240301ChipCountDataTable = blSC3240301.GetNoShowButtonInfo(dlrCode, brnCode)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return dtChipCntInfo
        End Using
    End Function
#End Region

#Region "中断ボタンの情報の取得処理"
    ''' <summary>
    ''' 中断ボタンの情報を取得.
    ''' </summary>
    ''' <remarks></remarks>
    Private Function GetInitializationStopButtonInfo(ByVal dlrCode As String, ByVal brnCode As String) As SC3240301DataSet.SC3240301ChipCountDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
          , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using blSC3240301 As New SC3240301BusinessLogic
            Dim userContext As StaffContext = StaffContext.Current
            '中断ボタンの情報を取得する
            Dim dtChipCntInfo As SC3240301DataSet.SC3240301ChipCountDataTable = blSC3240301.GetStopButtonInfo(dlrCode, brnCode)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return dtChipCntInfo
        End Using
    End Function
#End Region

#Region "受付ボタンの最新状態の取得"
    ''' <summary>
    ''' 受付ボタンの最新状態の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetReceptionButtonInfo(ByVal dlrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '受付エリアのCOUNT、遅れ状態を取得する
        Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationReceptionButtonInfo(dlrCode, brnCode)
        subchipDataJson = Me.DataTableToJson(dt)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub
#End Region

#Region "追加作業ボタンの最新状態の取得"
    ''' <summary>
    ''' 追加作業ボタンの最新状態の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetUpdateADDWorkChip(ByVal dlrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '追加作業エリアのCOUNT、遅れ状態を取得する
        Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationADDWorkButtonInfo(dlrCode, brnCode)
        subchipDataJson = Me.DataTableToJson(dt)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub
#End Region

#Region "完成検査ボタンの最新状態の取得"
    ''' <summary>
    ''' 完成検査ボタンの最新状態の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetUpdateComplInsChip(ByVal dlrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '完成検査エリアのCOUNT、遅れ状態を取得する
        Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationComplInsButtonInfo(dlrCode, brnCode)
        subchipDataJson = Me.DataTableToJson(dt)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub
#End Region

#Region "洗車ボタンの最新状態の取得"
    ''' <summary>
    ''' 洗車ボタンの最新状態の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetUpdateCarWashChip(ByVal dlrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '洗車エリアのCOUNT、遅れ状態を取得する
        Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationCarWashButtonInfo(dlrCode, brnCode)
        subchipDataJson = Me.DataTableToJson(dt)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub
#End Region

#Region "納車待ちボタンの最新状態の取得"
    ''' <summary>
    ''' 納車待ちボタンの最新状態の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetUpdateDeliChip(ByVal blrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '納車エリアのCOUNT、遅れ状態を取得する
        Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationDeliverdButtonInfo(blrCode, brnCode)
        subchipDataJson = Me.DataTableToJson(dt)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub
#End Region

#Region "NoShowボタンの最新状態の取得"
    ''' <summary>
    ''' NoShowボタンの最新状態の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetUpdateNoShowChip(ByVal dlrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        'NoShowエリアのCOUNT、遅れ状態を取得する
        Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationNoShowButtonInfo(dlrCode, brnCode)
        subchipDataJson = Me.DataTableToJson(dt)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub
#End Region

#Region "中断ボタンの最新状態の取得"
    ''' <summary>
    ''' 中断ボタンの最新状態の取得
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub GetUpdateStopChip(ByVal dlrCode As String, ByVal brnCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
         , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '中断エリアのCOUNT、遅れ状態を取得する
        Dim dt As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationStopButtonInfo(dlrCode, brnCode)
        subchipDataJson = Me.DataTableToJson(dt)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub
#End Region

#End Region

#Region "データ更新メソッド"

    '2013/12/02 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 洗車undo処理
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inRowLockVersion">ROWロックバージョン</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    Private Function UpdateUndoWashing(ByVal inServiceInId As Decimal, _
                                       ByVal inJobDtlId As Decimal, _
                                       ByVal inStallUseId As Decimal, _
                                       ByVal inRowLockVersion As Long) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. inServiceInId = {1}, inStallUseId = {2}, inRowLockVersion = {3} " _
                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                 , inServiceInId _
                                 , inStallUseId _
                                 , inRowLockVersion))
        '戻りコード
        Dim retrurnCode As Long = 0
        Using biz As New TabletSMBCommonClassBusinessLogic
            '洗車中チップをUndoする
            retrurnCode = biz.UndoWashingChip(inServiceInId, _
                                              inJobDtlId, _
                                              inStallUseId, _
                                              inRowLockVersion, _
                                              MY_PROGRAMID)

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

            'If retrurnCode = ActionResult.Success Then
            '    'Undo通知出す
            '    biz.SendNoticeByUndoWashingChip(inServiceInId)
            'End If

            If retrurnCode = ActionResult.Success _
            OrElse retrurnCode = ActionResult.WarningOmitDmsError Then
                '処理結果が下記の場合
                '　　0(成功)、または
                '-9000(DMS除外エラーの警告)

                'Undo通知出す
                biz.SendNoticeByUndoWashingChip(inServiceInId)

            End If

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return retrurnCode
    End Function
    '2013/12/02 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' 洗車開始処理
    ''' </summary>
    ''' <param name="inServiceInID">サービス入庫ID</param>
    ''' <param name="inRowLockVersion">ROWロックバージョン</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/02/16 TMEJ 小澤 UAT-BTS-168 洗車完了通知がされているのに洗車完了されていない不具合修正
    ''' </history>
    Private Function UpdateWashStart(ByVal inServiceInID As Decimal, _
                                     ByVal inJobDtlId As Decimal, _
                                     ByVal inStallUseId As Decimal, _
                                     ByVal inRowLockVersion As Long) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '戻りコード
        Dim retrurnCode As Long = 0
        Dim userInfo As StaffContext = StaffContext.Current
        Using biz As New TabletSMBCommonClassBusinessLogic
            'チップを洗車開始する
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            'retrurnCode = biz.UpdateChipWashStart(inServiceInID
            '                          inRoNum, _
            '                          inRowLockVersion, _
            '                          MY_PROGRAMID)
            retrurnCode = biz.UpdateChipWashStart(inServiceInID, _
                                                  inJobDtlId, _
                                                  inStallUseId, _
                                                  inRowLockVersion, _
                                                  MY_PROGRAMID)

            '2015/02/16 TMEJ 小澤 UAT-BTS-168 洗車完了通知がされているのに洗車完了されていない不具合修正 START

            ''指定SAにPush送信(自分以外)
            'biz.SendNamedSAPush(inServiceInID, userInfo.DlrCD, userInfo.BrnCD, userInfo.Account)

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

            ''処理結果チェック
            'If retrurnCode = ActionResult.Success Then
            '    '成功した場合

            '    '指定SAにPush送信(自分以外)
            '    biz.SendNamedSAPush(inServiceInID, userInfo.DlrCD, userInfo.BrnCD, userInfo.Account)

            '    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START
            '    'CW権限にPUSHする
            '    biz.SendAllCWPush(userInfo.DlrCD, userInfo.BrnCD, userInfo.Account)
            '    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

            'End If

            '処理結果チェック
            If retrurnCode = ActionResult.Success _
            OrElse retrurnCode = ActionResult.WarningOmitDmsError Then
                '処理結果が下記の場合
                '　　0(成功)、または
                '-9000(DMS除外エラーの警告)

                '指定SAにPush送信(自分以外)
                biz.SendNamedSAPush(inServiceInID, userInfo.DlrCD, userInfo.BrnCD, userInfo.Account)

                'CW権限にPUSHする
                biz.SendAllCWPush(userInfo.DlrCD, userInfo.BrnCD, userInfo.Account)

            End If

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            '2015/02/16 TMEJ 小澤 UAT-BTS-168 洗車完了通知がされているのに洗車完了されていない不具合修正 END

        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return retrurnCode
    End Function

    ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
    ' ''' <summary>
    ' ''' 洗車終了処理
    ' ''' </summary>
    ' ''' <param name="inServiceInID">サービス入庫ID</param>
    ' ''' <param name="inJobDtlId">作業内容ID</param>
    ' ''' <param name="inStallUseId">ストール利用ID</param>
    ' ''' <param name="inPickDeliType">引取納車区分</param>
    ' ''' <param name="inRowLockVersion">ROWロックバージョン</param>
    ' ''' <returns>戻り値</returns>
    ' ''' <remarks></remarks>
    ' ''' 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    'Private Function UpdateWashEnd(ByVal inServiceInID As Decimal, _
    '                               ByVal inJobDtlId As Decimal, _
    '                               ByVal inStallUseId As Decimal, _
    '                               ByVal inPickDeliType As String, _
    '                               ByVal inRowLockVersion As Long) As Long
    ''' <summary>
    ''' 洗車終了処理
    ''' </summary>
    ''' <param name="inServiceInID">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inPickDeliType">引取納車区分</param>
    ''' <param name="inRowLockVersion">ROWロックバージョン</param>
    ''' <param name="inRoNum">RO番号</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/02/16 TMEJ 小澤 UAT-BTS-168 洗車完了通知がされているのに洗車完了されていない不具合修正
    ''' </history>
    Private Function UpdateWashEnd(ByVal inServiceInID As Decimal, _
                                   ByVal inJobDtlId As Decimal, _
                                   ByVal inStallUseId As Decimal, _
                                   ByVal inPickDeliType As String, _
                                   ByVal inRowLockVersion As Long, _
                                   ByVal inRoNum As String) As Long
        'Private Function UpdateWashEnd(ByVal inServiceInID As Decimal, _
        '                       ByVal inPickDeliType As String, _
        '                       ByVal inRoNum As String, _
        '                       ByVal inRowLockVersion As Long) As Long

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '戻りコード
        Dim retrurnCode As Long = 0
        Dim userInfo As StaffContext = StaffContext.Current
        Using biz As New TabletSMBCommonClassBusinessLogic
            'チップを洗車終了する
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            'retrurnCode = biz.UpdateChipWashEnd(inServiceInID, _
            '                        inPickDeliType, _
            '                        inRoNum, _
            '                        inRowLockVersion, _
            '                        MY_PROGRAMID)

            '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
            'retrurnCode = biz.UpdateChipWashEnd(inServiceInID, _
            '                        inJobDtlId, _
            '                        inStallUseId, _
            '                        inPickDeliType, _
            '                        inRowLockVersion, _
            '                        MY_PROGRAMID)
            retrurnCode = biz.UpdateChipWashEnd(inServiceInID, _
                                                inJobDtlId, _
                                                inStallUseId, _
                                                inPickDeliType, _
                                                inRowLockVersion, _
                                                MY_PROGRAMID, _
                                                inRoNum)
            '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

            '2015/02/16 TMEJ 小澤 UAT-BTS-168 洗車完了通知がされているのに洗車完了されていない不具合修正 START
            ''通知APIを呼ぶ
            ''情報取得
            'Using tabletSMBCommonDataAdapter As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter
            '    Dim dtNoticeInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassNoticeInfoDataTable = _
            '        tabletSMBCommonDataAdapter.GetNoticeInfo(inServiceInID, userInfo.DlrCD, userInfo.BrnCD)
            '    '通知処理
            '    biz.WashEndNoticePush(dtNoticeInfo, userInfo)
            '    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
            'End Using

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

            ''処理結果チェック
            'If retrurnCode = ActionResult.Success Then
            '    '成功した場合

            '    '通知APIを呼ぶ
            '    '情報取得
            '    Using tabletSMBCommonDataAdapter As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter

            '        Dim dtNoticeInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassNoticeInfoDataTable = _
            '            tabletSMBCommonDataAdapter.GetNoticeInfo(inServiceInID, userInfo.DlrCD, userInfo.BrnCD)

            '        '通知処理
            '        biz.WashEndNoticePush(dtNoticeInfo, userInfo)

            '    End Using

            'End If

            '処理結果チェック
            If retrurnCode = ActionResult.Success _
            OrElse retrurnCode = ActionResult.WarningOmitDmsError Then
                '処理結果が下記の場合
                '　　0(成功)、または
                '-9000(DMS除外エラーの警告)

                '通知APIを呼ぶ
                '情報取得
                Using tabletSMBCommonDataAdapter As New TabletSMBCommonClassDataSetTableAdapters.TabletSMBCommonClassDataAdapter

                    Dim dtNoticeInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassNoticeInfoDataTable = _
                        tabletSMBCommonDataAdapter.GetNoticeInfo(inServiceInID, userInfo.DlrCD, userInfo.BrnCD)

                    '通知処理
                    biz.WashEndNoticePush(dtNoticeInfo, userInfo)

                End Using

            End If

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            '2015/02/16 TMEJ 小澤 UAT-BTS-168 洗車完了通知がされているのに洗車完了されていない不具合修正 END

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return retrurnCode
    End Function

    ''' <summary>
    ''' 納車処理
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール使用ID</param>
    ''' <param name="inRoNum">RO番号</param>
    ''' <param name="inRowLockVersion">ROWロックバージョン</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    Private Function UpdateDelivery(ByVal inServiceInID As Decimal, _
                                    ByVal inJobDtlId As Decimal, _
                                    ByVal inStallUseId As Decimal, _
                                    ByVal inRoNum As String, _
                                    ByVal inRowLockVersion As Long) As Long
        'Private Function UpdateDelivery(ByVal inServiceInID As Decimal, _
        '                                ByVal inRoNum As String, _
        '                                ByVal inRowLockVersion As Long) As Long
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '戻りコード
        Dim retrurnCode As Long = 0
        Using biz As New TabletSMBCommonClassBusinessLogic
            'チップを納車する
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            'retrurnCode = biz.UpdateChipDelivery(inServiceInID, _
            '                         inRoNum, _
            '                         inRowLockVersion, _
            '                         MY_PROGRAMID)
            retrurnCode = biz.UpdateChipDelivery(inServiceInID, _
                                                 inJobDtlId, _
                                                 inStallUseId, _
                                                 inRoNum, _
                                                 inRowLockVersion, _
                                                 MY_PROGRAMID)
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return retrurnCode
    End Function
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 洗車へ移動処理
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inRowLockVersion">ROWロックバージョン</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    Private Function MoveToWash(ByVal inServiceInID As Decimal, _
                                ByVal inJobDtlId As Decimal, _
                                ByVal inStallUseId As Decimal, _
                                ByVal inRowLockVersion As Long) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '戻りコード
        Dim retrurnCode As Long = 0
        Using biz As New TabletSMBCommonClassBusinessLogic
            'チップを洗車待ちに更新する
            retrurnCode = biz.ChipMoveToWash(inServiceInID, _
                                             inJobDtlId, _
                                             inStallUseId, _
                                             inRowLockVersion, _
                                             MY_PROGRAMID)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return retrurnCode
    End Function

    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
    ' ''' <summary>
    ' ''' 納車待ちへ移動処理
    ' ''' </summary>
    ' ''' <param name="inServiceInID">サービス入庫ID</param>
    ' ''' <param name="inJobDtlId">作業内容ID</param>
    ' ''' <param name="inStallUseId">ストール利用ID</param>
    ' ''' <param name="inPickDeliType">引取納車区分</param>
    ' ''' <param name="inRowLockVersion">ROWロックバージョン</param>
    ' ''' <returns>戻り値</returns>
    ' ''' <remarks></remarks>
    'Private Function MoveToDeliWait(ByVal inServiceInID As Decimal, _
    '                                ByVal inJobDtlId As Decimal, _
    '                                ByVal inStallUseId As Decimal, _
    '                                ByVal inPickDeliType As String, _
    '                                ByVal inRowLockVersion As Long) As Long
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
    '             , System.Reflection.MethodBase.GetCurrentMethod.Name))
    ''' <summary>
    ''' 納車待ちへ移動処理
    ''' </summary>
    ''' <param name="inServiceInID">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inPickDeliType">引取納車区分</param>
    ''' <param name="inRowLockVersion">ROWロックバージョン</param>
    ''' <param name="inRoNum">RO番号</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
    ''' </history>
    Private Function MoveToDeliWait(ByVal inServiceInID As Decimal, _
                                    ByVal inJobDtlId As Decimal, _
                                    ByVal inStallUseId As Decimal, _
                                    ByVal inPickDeliType As String, _
                                    ByVal inRowLockVersion As Long, _
                                    ByVal inRoNum As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start inServiceInID={1}, inJobDtlId={2}, inStallUseId={3}, inPickDeliType={4}, inRowLockVersion={5}, inRoNum={6}" _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
                 , inServiceInID _
                 , inJobDtlId _
                 , inStallUseId _
                 , inPickDeliType _
                 , inRowLockVersion _
                 , inRoNum))

        '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

        '戻りコード
        Dim retrurnCode As Long = 0
        Using biz As New TabletSMBCommonClassBusinessLogic

            '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
            ''チップを納車待ちに更新する
            'retrurnCode = biz.ChipMoveToDeliWait(inServiceInID, _
            '                                     inJobDtlId, _
            '                                     inStallUseId, _
            '                                     inPickDeliType, _
            '                                     inRowLockVersion, _
            '                                     MY_PROGRAMID)
            'チップを納車待ちに更新する
            retrurnCode = biz.ChipMoveToDeliWait(inServiceInID, _
                                                 inJobDtlId, _
                                                 inStallUseId, _
                                                 inPickDeliType, _
                                                 inRowLockVersion, _
                                                 MY_PROGRAMID, _
                                                 inRoNum)
            '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

            '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

            ''処理結果チェック
            'If retrurnCode = 0 Then
            '    '成功した場合

            '    '納車待ちへ移動Push処理を行う
            '    biz.ToDeliWaitNoticePush()

            'End If

            '処理結果チェック
            If retrurnCode = ActionResult.Success _
            OrElse retrurnCode = ActionResult.WarningOmitDmsError Then
                '処理結果が下記の場合
                '　　0(成功)、または
                '-9000(DMS除外エラーの警告)

                '納車待ちへ移動Push処理を行う
                biz.ToDeliWaitNoticePush()

            End If

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return retrurnCode
    End Function
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
    ''' <summary>
    ''' キャンセル処理
    ''' </summary>
    ''' <param name="inArgument">コールバックオブジェクト</param>
    ''' <returns>戻り値　retrurnCode</returns>
    ''' <remarks></remarks>
    Private Function UpdateCancel(ByVal inArgument As CallBackArgumentClass) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim userContext As StaffContext = StaffContext.Current
        'Dim nowDate As Date = DateTimeFunc.Now(userContext.DlrCD)
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        Using biz As New TabletSMBCommonClassBusinessLogic
            'チップをキャンセルする
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            'Dim resultCode As Long = biz.DeleteStallChip(inArgument.StalluseId, _
            '                                             userContext, _
            '                                             MY_PROGRAMID, _
            '                                             inArgument.RowLockVersion)
            Dim resultCode As Long = biz.DeleteStallChip(inArgument.StalluseId, _
                                                         MY_PROGRAMID, _
                                                         inArgument.RowLockVersion)
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return resultCode
        End Using
    End Function

    ''' <summary>
    ''' サブチップ移動処理
    ''' </summary>
    ''' <param name="inArgument">コールバックオブジェクト</param>
    ''' <param name="dtStartDate">開始時間</param>
    ''' <param name="dtEndDate">終了時間</param>
    ''' <param name="nowDate">現在時間</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    Private Function SubChipMoving(ByVal inArgument As CallBackArgumentClass, _
                                        ByVal dtStartDate As Date, _
                                        ByVal dtEndDate As Date, _
                                        ByVal nowDate As Date) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim userContext As StaffContext = StaffContext.Current
        Dim dlrCd = userContext.DlrCD
        Dim brnCd = userContext.BrnCD
        Dim account = userContext.Account
        '戻りコード
        Dim retrurnCode As Long = 0

        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        Dim restFlg = String.Empty

        Using tabBiz As New TabletSMBCommonClassBusinessLogic

            If tabBiz.IsRestAutoJudge() Then
                '休憩を自動判定する場合
                restFlg = RestTimeGetFlgGetRest
            Else
                '自動判定しない場合
                restFlg = inArgument.RestFlg
            End If

        End Using
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        Using biz As New SC3240301BusinessLogic
            If CBACK_RESERVE_ATTCHMENT.Equals(inArgument.ButtonID) Then
                '受付チップをストール上に配置する場合
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                ''更新： 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                ''retrurnCode = biz.ReceptionChipMoveResize(inArgument.ServiceInId, _
                ''                     inArgument.JobDtlId, _
                ''                     inArgument.StalluseId, _
                ''                     inArgument.StallId, _
                ''                     inArgument.ScheStartDatetime, _
                ''                     inArgument.ScheWorkTime, _
                ''                     inArgument.RestFlg, _
                ''                     dtStartDate, _
                ''                     dtEndDate, _
                ''                     nowDate, _
                ''                     account, _
                ''                     inArgument.RowLockVersion, _
                ''                     MY_PROGRAMID, _
                ''                     inArgument.ScheDeliDatetime, _
                ''                     inArgument.MainteCd, _
                ''                     inArgument.WorkSeq, _
                ''                     inArgument.RoNum, _
                ''                     inArgument.PickDeliType, _
                ''                     inArgument.ScheSvcinDateTime)
                ''2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
                ''                retrurnCode = biz.ReceptionChipMoveResize(inArgument.ServiceInId, _
                ''                    inArgument.JobDtlId, _
                ''                    inArgument.StalluseId, _
                ''                    inArgument.StallId, _
                ''                    inArgument.ScheStartDatetime, _
                ''                    inArgument.ScheWorkTime, _
                ''                    inArgument.RestFlg, _
                ''                    dtStartDate, _
                ''                    dtEndDate, _
                ''                    nowDate, _
                ''                    account, _
                ''                    inArgument.RowLockVersion, _
                ''                    MY_PROGRAMID, _
                ''                    inArgument.ScheDeliDatetime, _
                ''                    inArgument.MainteCd, _
                ''                    inArgument.WorkSeq, _
                ''                    inArgument.RoNum, _
                ''                    inArgument.PickDeliType, _
                ''                    inArgument.ScheSvcinDateTime, _
                ''                    inArgument.InspectionNeedFlg)

                'retrurnCode = biz.ReceptionChipMoveResize(inArgument.ServiceInId, _
                '                    inArgument.JobDtlId, _
                '                    inArgument.StalluseId, _
                '                    inArgument.StallId, _
                '                    inArgument.ScheStartDatetime, _
                '                    inArgument.ScheWorkTime, _
                '                    inArgument.RestFlg, _
                '                    dtStartDate, _
                '                    dtEndDate, _
                '                    nowDate, _
                '                    account, _
                '                    inArgument.RowLockVersion, _
                '                    MY_PROGRAMID, _
                '                    inArgument.ScheDeliDatetime, _
                '                    inArgument.MainteCd, _
                '                    inArgument.WorkSeq, _
                '                    inArgument.RoNum, _
                '                    inArgument.PickDeliType, _
                '                    inArgument.ScheSvcinDateTime, _
                '                    inArgument.InspectionNeedFlg, _
                '                    inArgument.TempFlg)
                ''2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END
                ''更新： 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

                retrurnCode = biz.ReceptionChipMoveResize(inArgument.ServiceInId, _
                                    inArgument.JobDtlId, _
                                    inArgument.StalluseId, _
                                    inArgument.StallId, _
                                    inArgument.ScheStartDatetime, _
                                    inArgument.ScheWorkTime, _
                                    restFlg, _
                                    dtStartDate, _
                                    dtEndDate, _
                                    nowDate, _
                                    account, _
                                    inArgument.RowLockVersion, _
                                    MY_PROGRAMID, _
                                    inArgument.ScheDeliDatetime, _
                                    inArgument.MainteCd, _
                                    inArgument.WorkSeq, _
                                    inArgument.RoNum, _
                                    inArgument.PickDeliType, _
                                    inArgument.ScheSvcinDateTime, _
                                    inArgument.InspectionNeedFlg, _
                                    inArgument.TempFlg)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
            Else
                'NoShow、中断チップをストール上に配置する場合
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'retrurnCode = biz.SubChipMoveResize(inArgument.StalluseId, _
                '                     inArgument.StallId, _
                '                     inArgument.ScheStartDatetime, _
                '                     inArgument.ScheWorkTime, _
                '                     inArgument.RestFlg, _
                '                     dtStartDate, _
                '                     dtEndDate, _
                '                     nowDate, _
                '                     account, _
                '                     inArgument.RowLockVersion, _
                '                     MY_PROGRAMID)
                retrurnCode = biz.SubChipMoveResize(inArgument.StalluseId, _
                                     inArgument.StallId, _
                                     inArgument.ScheStartDatetime, _
                                     inArgument.ScheWorkTime, _
                                     restFlg, _
                                     dtStartDate, _
                                     dtEndDate, _
                                     nowDate, _
                                     account, _
                                     inArgument.RowLockVersion, _
                                     MY_PROGRAMID)
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                ''2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'If retrurnCode = ActionResult.Success AndAlso CBACK_NOSHOW_MOVING.Equals(inArgument.ButtonID) Then
                '    'Push
                '    biz.NoShowChipMovePush()
                'End If
                ''2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

                If retrurnCode = ActionResult.Success _
                OrElse retrurnCode = ActionResult.WarningOmitDmsError Then
                    '処理結果が下記の場合
                    '　　0(成功)、または
                    '-9000(DMS除外エラーの警告)

                    If CBACK_NOSHOW_MOVING.Equals(inArgument.ButtonID) Then
                        'NoShow再配置イベントの場合

                        'NoShowチップ再配置Push
                        biz.NoShowChipMovePush()
                    End If
                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            End If
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
            Return retrurnCode
        End Using
    End Function

#Region "JSON変換"

    ''' <summary>
    '''   DataTableをJSON文字列に変換する
    ''' </summary>
    ''' <param name="dataTable">変換対象 DataSet</param>
    ''' <returns>JSON文字列</returns>
    Private Function DataTableToJson(ByVal dataTable As DataTable) As String
        Dim resultMain As New Dictionary(Of String, Object)
        Dim JSerializer As New JavaScriptSerializer

        If dataTable Is Nothing Then
            Return JSerializer.Serialize(resultMain)
        End If

        For Each dr As DataRow In dataTable.Rows
            Dim result As New Dictionary(Of String, Object)

            For Each dc As DataColumn In dataTable.Columns
                result.Add(dc.ColumnName, dr(dc).ToString)
            Next
            resultMain.Add("Key" + CType(resultMain.Count + 1, String), result)
        Next

        Return JSerializer.Serialize(resultMain)
    End Function

#End Region

#End Region

#Region "コールバック関連メッソド"
    ''' <summary>
    '''  コールバック用JSONデータを返却
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult
        Return subchipDataJson
    End Function

    ''' <summary>
    ''' コールバック関数
    ''' </summary>
    ''' <param name="eventArgument">クライアントからの引数</param>
    ''' <remarks></remarks>
    Private Sub RaiseCallbackEvent(ByVal eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent
        Logger.Info(MY_PROGRAMID & ".ascx " & "RaiseCallbackEvent.S" & eventArgument)
        Dim returnCode As Long
        Dim subButtonID As String = ""
        Dim rsltDeliDate As New Date
        Dim result As New CallBackResultClass
        Dim userContext As StaffContext = StaffContext.Current
        Dim dlrCd As String = userContext.DlrCD
        Dim brnCd As String = userContext.BrnCD
        Dim account As String = userContext.Account
        Dim nowDate As Date = DateTimeFunc.Now(dlrCd)
        Dim serializer = New JavaScriptSerializer
        GetBranchOperationgHours(userContext)
        'コールバック引数用内部クラスのインスタンスを生成し、JSON形式の引数を内部クラス型に変換して受け取る
        Dim argument As New CallBackArgumentClass
        argument = serializer.Deserialize(Of CallBackArgumentClass)(eventArgument)

        '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
        If CBACK_RESERVE_ATTCHMENT.Equals(argument.ButtonID) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} ⑧SC3240301_受付エリアからのチップ配置処理 START" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
        End If
        '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

        Using blSC3240301 As New SC3240301BusinessLogic
            Try
                'サブエリアチップ情報を取得
                Me.GetSubAreaChipInfo(argument.ButtonID)
                'サブエリアボタンの状態更新、件数を取得する
                Me.GetSubAreaCount(argument.ButtonID)
                Select Case argument.ButtonID
                    Case CBACK_REFRESH_ALLCONUT
                        subchipDataJson = GetInitializationButtonInfo()
                        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                        'Case CBACK_ACTION_WASHSTART, CBACK_ACTION_WASHEND, CBACK_ACTION_DELIVERY
                    Case CBACK_ACTION_WASHSTART, CBACK_ACTION_WASHEND, CBACK_ACTION_DELIVERY, CBACK_ACTION_MOVETOWASH, CBACK_ACTION_MOVETODELIWAIT, CBACK_ACTION_UNDO
                        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                        '洗車開始、洗車終了、納車ボタン、洗車へ移動、納車待ちへ移動
                        Me.CarwashDeliCar(argument, serializer)

                        '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
                        '分岐条件値が"3200"(中断終了ボタンタップ)の場合
                    Case CBACK_ACTION_FINISH_STOP_CHIP
                        '次工程へ移動処理を実行
                        returnCode = blSC3240301.FinishStopChip(argument.StalluseId, nowDate, argument.RowLockVersion)

                        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                        ''中断処理結果が0(成功)の場合
                        'If returnCode = ActionResult.Success Then

                        '    '中断処理通知を出す処理
                        '    blSC3240301.SendNoticeByStopFinish(userContext, argument.StalluseId)

                        '    '以上の処理結果から必要な値をCallback処理結果クラスのプロパティにセットし、同クラスをJSON文字列に変換する
                        '    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetStopChipData(dlrCd, brnCd)
                        '    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = Me.GetInitializationStopButtonInfo(dlrCd, brnCd)
                        '    result.JobStopArea = HttpUtility.HtmlEncode(Me.DataTableToJson(dtChipInfo))
                        '    result.JobStopButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))

                        '    '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する
                        '    Dim dtCarWashButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationCarWashButtonInfo(dlrCd, brnCd)
                        '    Dim dtDroppfButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationDeliverdButtonInfo(dlrCd, brnCd)
                        '    Dim dtComplInsButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationComplInsButtonInfo(dlrCd, brnCd)
                        '    result.DropofButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtDroppfButtonInfo))
                        '    result.CarWashButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtCarWashButtonInfo))
                        '    result.ComplInsButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtComplInsButtonInfo))

                        'End If

                        If returnCode = ActionResult.Success _
                        OrElse returnCode = ActionResult.WarningOmitDmsError Then
                            '次工程へ移動の処理結果が下記の場合
                            '　　0(成功)、または
                            '-9000(DMS除外エラーの警告)

                            '次工程へ移動による通知を出す処理
                            blSC3240301.SendNoticeByStopFinish(userContext, argument.StalluseId)

                            '以上の処理結果から必要な値をCallback処理結果クラスのプロパティにセットし、同クラスをJSON文字列に変換する

                            '中断エリアのチップ情報
                            Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = _
                                blSC3240301.GetStopChipData(dlrCd, brnCd)

                            '中断エリアのボタン情報
                            Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                                Me.GetInitializationStopButtonInfo(dlrCd, brnCd)

                            result.JobStopArea = HttpUtility.HtmlEncode(Me.DataTableToJson(dtChipInfo))
                            result.JobStopButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))

                            '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する

                            '洗車エリアのボタン情報
                            Dim dtCarWashButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                                GetInitializationCarWashButtonInfo(dlrCd, brnCd)

                            '納車待ちエリアのボタン情報
                            Dim dtDroppfButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                                GetInitializationDeliverdButtonInfo(dlrCd, brnCd)

                            '完成検査エリアのボタン情報
                            Dim dtComplInsButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                                GetInitializationComplInsButtonInfo(dlrCd, brnCd)

                            result.DropofButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtDroppfButtonInfo))
                            result.CarWashButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtCarWashButtonInfo))
                            result.ComplInsButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtComplInsButtonInfo))

                        End If

                        '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                        Dim dtStartDate As Date

                        If argument.ShowDate <> Date.MinValue Then

                            dtStartDate = Date.Parse(argument.ShowDate & " " & m_strStallStartTime & ":00", CultureInfo.InvariantCulture)

                        End If

                        'サービス入庫IDを取得する
                        result.StallChip = HttpUtility.HtmlEncode(Me.DataTableToJson( _
                                            blSC3240301.GetStallChipAfterOperation(userContext.DlrCD, _
                                                                                   userContext.BrnCD, _
                                                                                   dtStartDate, _
                                                                                   argument.PreRefreshDateTime)))

                        'イベントID
                        result.ButtonID = argument.ButtonID
                        'サブエリアID
                        result.SubButtonID = CBACK_STOP
                        result.ResultCode = returnCode
                        result.Message = HttpUtility.HtmlEncode(GetErrorMessage(returnCode))
                        Me.subchipDataJson = serializer.Serialize(result)

                        '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

                    Case CBACK_ACTION_CANCEL
                        'キャンセルボタン処理
                        returnCode = UpdateCancel(argument)
                        Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetNoShowChipData(dlrCd, brnCd)
                        Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationNoShowButtonInfo(dlrCd, brnCd)
                        result.NoShowArea = HttpUtility.HtmlEncode(Me.DataTableToJson(dtChipInfo))
                        result.NoShowButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))
                        'イベントID
                        result.ButtonID = argument.ButtonID
                        'サブエリアID
                        result.SubButtonID = CBACK_NOSHOW
                        result.ResultCode = returnCode
                        result.Message = HttpUtility.HtmlEncode(GetErrorMessage(returnCode))
                        Me.subchipDataJson = serializer.Serialize(result)
                    Case CBACK_NOSHOW_MOVING, CBACK_STOP_MOVING, CBACK_RESERVE_ATTCHMENT
                        'NoShowエリア、中断エリア、受付エリアからストールに移動する
                        Me.SubChipMove(argument, serializer, nowDate)
                End Select

                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
                If CBACK_RESERVE_ATTCHMENT.Equals(argument.ButtonID) Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} ⑧SC3240301_受付エリアからのチップ配置処理 END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
                End If
                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

            Catch ex As OracleExceptionEx When ex.Number = 1013
                'イベントID
                result.ButtonID = argument.ButtonID
                'サブエリアID
                result.SubButtonID = CBACK_RECEPTION
                result.SubChipKey = argument.SubChipKey
                result.ResultCode = ResultCode_TimeOut
                result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, WordID.id001))
                Me.subchipDataJson = serializer.Serialize(result)
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod.Name & " Error:", ex)
            Catch ex As Exception
                'イベントID
                result.ButtonID = argument.ButtonID
                'サブエリアID
                result.SubButtonID = CBACK_RECEPTION
                result.ResultCode = ResultCode_ExceptionError
                result.SubChipKey = argument.SubChipKey
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'result.Message = HttpUtility.HtmlEncode(WebWordUtility.GetWord(MY_PROGRAMID, WordID.id002))
                'DMS連携エラー対応
                Dim errMessage As String = WebWordUtility.GetWord(MY_PROGRAMID, WordID.id002)
                If ActionResult.IC3802503ResultTimeOutError.ToString().Equals(ex.Message) Then
                    errMessage = GetErrorMessage(WordID.id013)
                ElseIf ActionResult.IC3802503ResultDmsError.ToString().Equals(ex.Message) Then
                    errMessage = GetErrorMessage(WordID.id014)
                ElseIf ActionResult.IC3802503ResultOtherError.ToString().Equals(ex.Message) Then
                    errMessage = GetErrorMessage(WordID.id015)
                End If
                result.Message = HttpUtility.HtmlEncode(errMessage)
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                Me.subchipDataJson = serializer.Serialize(result)
                Logger.Error(System.Reflection.MethodBase.GetCurrentMethod.Name & " Error:", ex)
            Finally
                serializer = Nothing
                argument = Nothing
            End Try
        End Using

    End Sub

    ''' <summary>
    ''' サブエリアチップ情報取得
    ''' </summary>
    ''' <param name="actionCode">アクションコード</param>
    ''' <remarks></remarks>
    Private Sub GetSubAreaChipInfo(ByVal actionCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim userContext As StaffContext = StaffContext.Current
        Dim dlrCd As String = userContext.DlrCD
        Dim brnCd As String = userContext.BrnCD
        Select Case actionCode
            Case CBACK_RECEPTION
                '受付チップ取得
                Me.GetReceptionChip(dlrCd, brnCd)
            Case CBACK_ADDITIONALWORK
                '追加作業チップ取得
                Me.GetAddWorkChip(dlrCd, brnCd)
            Case CBACK_COMPLETIONINSPECTION
                '完成検査チップ取得
                Me.GetCompletedInspectionChip(dlrCd, brnCd)
            Case CBACK_CARWASH
                '洗車チップ取得
                Me.GetCarWashChip(dlrCd, brnCd)
            Case CBACK_DELIVERDCAR
                '納車待ちチップ取得
                Me.GetDeliverdChip(dlrCd, brnCd)
            Case CBACK_NOSHOW
                'NoShowチップ取得
                Me.GetNoShowChip(dlrCd, brnCd)
            Case CBACK_STOP
                '中断チップ取得
                Me.GetStopChip(dlrCd, brnCd)
        End Select
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' サブエリアチップ数と遅れ情報取得
    ''' </summary>
    ''' <param name="actionCode">アクションコード</param>
    ''' <remarks></remarks>
    Private Sub GetSubAreaCount(ByVal actionCode As String)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim userContext As StaffContext = StaffContext.Current
        Dim dlrCd As String = userContext.DlrCD
        Dim brnCd As String = userContext.BrnCD
        Select Case actionCode
            Case CBACK_REFRESH_RECEPTION
                '受付ボタンの状態更新、件数を取得する
                Me.GetReceptionButtonInfo(dlrCd, brnCd)
            Case CBACK_REFRESH_ADDITIONALWORK
                '追加作業ボタンの状態更新、件数を取得する
                Me.GetUpdateADDWorkChip(dlrCd, brnCd)
            Case CBACK_REFRESH_COMPLETIONINSPECTIO
                '完成検査ボタンの状態更新、件数を取得する
                Me.GetUpdateComplInsChip(dlrCd, brnCd)
            Case CBACK_REFRESH_CARWASH
                '洗車ボタンの状態更新、件数を取得する
                Me.GetUpdateCarWashChip(dlrCd, brnCd)
            Case CBACK_REFRESH_DELIVERDCAR
                '納車待ちボタンの状態更新、件数を取得する
                Me.GetUpdateDeliChip(dlrCd, brnCd)
            Case CBACK_REFRESH_NOSHOW
                'NoShowボタンの状態更新、件数を取得する
                Me.GetUpdateNoShowChip(dlrCd, brnCd)
            Case CBACK_REFRESH_STOP
                '中断ボタンの状態更新、件数を取得する
                Me.GetUpdateStopChip(dlrCd, brnCd)
        End Select
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' 洗車開始、洗車終了、納車処理
    ''' </summary>
    ''' <param name="argument">クライアントからの引数</param>
    ''' <param name="serializer">シリアライズ</param>
    ''' <remarks></remarks>
    Private Sub CarwashDeliCar(ByVal argument As CallBackArgumentClass, _
                               ByVal serializer As JavaScriptSerializer)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim returnCode As Long
        Dim subButtonID As String = ""
        Dim result As New CallBackResultClass
        Dim userContext As StaffContext = StaffContext.Current
        Dim dlrCd As String = userContext.DlrCD
        Dim brnCd As String = userContext.BrnCD
        Dim account As String = userContext.Account
        Dim dtStartDate As Date
        Dim dtEndDate As Date
        If argument.ShowDate <> Date.MinValue Then
            dtStartDate = Date.Parse(argument.ShowDate & " " & m_strStallStartTime & ":00", CultureInfo.InvariantCulture)
            dtEndDate = Date.Parse(argument.ShowDate & " " & m_strStallEndTime & ":00", CultureInfo.InvariantCulture)
        End If
        Using blSC3240301 As New SC3240301BusinessLogic
            If CBACK_ACTION_WASHSTART.Equals(argument.ButtonID) Then
                '洗車開始処理
                returnCode = Me.UpdateWashStart(argument.ServiceInId, _
                                                argument.JobDtlId, _
                                                argument.StalluseId, _
                                                argument.RowLockVersion)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If returnCode = ActionResult.Success Then
                '    '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する
                '    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetCarWashChipData(dlrCd, brnCd)
                '    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationCarWashButtonInfo(dlrCd, brnCd)
                '    result.CarwrashArea = HttpUtility.HtmlEncode(DataTableToJson(dtChipInfo))
                '    result.CarWashButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))
                'End If

                If returnCode = ActionResult.Success _
                OrElse returnCode = ActionResult.WarningOmitDmsError Then
                    '処理結果が下記の場合
                    '　　0(成功)、または
                    '-9000(DMS除外エラーの警告)

                    '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する

                    '洗車エリアチップ情報
                    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = _
                        blSC3240301.GetCarWashChipData(dlrCd, brnCd)

                    '洗車エリアボタン情報
                    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                        GetInitializationCarWashButtonInfo(dlrCd, brnCd)

                    result.CarwrashArea = HttpUtility.HtmlEncode(Me.DataTableToJson(dtChipInfo))
                    result.CarWashButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                '戻り値を格納
                subButtonID = CBACK_CARWASH

            ElseIf CBACK_ACTION_WASHEND.Equals(argument.ButtonID) Then
                '洗車終了処理

                '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'returnCode = Me.UpdateWashEnd(argument.ServiceInId, _
                '                              argument.PickDeliType, _
                '                              argument.RoNum, _
                '                              argument.RowLockVersion)
                'returnCode = Me.UpdateWashEnd(argument.ServiceInId, _
                '              argument.JobDtlId, _
                '              argument.StalluseId, _
                '              argument.PickDeliType, _
                '              argument.RowLockVersion)
                returnCode = Me.UpdateWashEnd(argument.ServiceInId, _
                                              argument.JobDtlId, _
                                              argument.StalluseId, _
                                              argument.PickDeliType, _
                                              argument.RowLockVersion, _
                                              argument.RoNum)
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
                '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If returnCode = ActionResult.Success Then
                '    '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する
                '    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetCarWashChipData(dlrCd, brnCd)
                '    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationCarWashButtonInfo(dlrCd, brnCd)
                '    Dim dtDroppfButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationDeliverdButtonInfo(dlrCd, brnCd)
                '    result.CarwrashArea = HttpUtility.HtmlEncode(DataTableToJson(dtChipInfo))
                '    result.DropofButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtDroppfButtonInfo))
                '    result.CarWashButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))
                'End If

                If returnCode = ActionResult.Success _
                OrElse returnCode = ActionResult.WarningOmitDmsError Then
                    '処理結果が下記の場合
                    '　　0(成功)、または
                    '-9000(DMS除外エラーの警告)

                    '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する

                    '洗車エリアチップ情報
                    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = _
                        blSC3240301.GetCarWashChipData(dlrCd, brnCd)

                    '洗車エリアボタン情報
                    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                        GetInitializationCarWashButtonInfo(dlrCd, brnCd)

                    '納車待ちエリアボタン情報
                    Dim dtDroppfButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                        GetInitializationDeliverdButtonInfo(dlrCd, brnCd)

                    result.CarwrashArea = HttpUtility.HtmlEncode(Me.DataTableToJson(dtChipInfo))
                    result.DropofButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtDroppfButtonInfo))
                    result.CarWashButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                '戻り値を格納
                subButtonID = CBACK_CARWASH
            ElseIf CBACK_ACTION_DELIVERY.Equals(argument.ButtonID) Then
                '納車処理
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                'returnCode = Me.UpdateDelivery(argument.ServiceInId, _
                '                               argument.RoNum, _
                '                               argument.RowLockVersion)
                returnCode = Me.UpdateDelivery(argument.ServiceInId, _
                                               argument.JobDtlId, _
                                               argument.StalluseId, _
                                               argument.RoNum, _
                                               argument.RowLockVersion)
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                ''戻り値格納
                'If returnCode = ActionResult.Success Then
                '    '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する
                '    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetDeliverdChipData(dlrCd, brnCd)
                '    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationDeliverdButtonInfo(dlrCd, brnCd)
                '    result.DropofButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))
                '    result.DropoffArea = HttpUtility.HtmlEncode(DataTableToJson(dtChipInfo))
                'End If

                '戻り値格納
                If returnCode = ActionResult.Success _
                OrElse returnCode = ActionResult.WarningOmitDmsError Then
                    '処理結果が下記の場合
                    '　　0(成功)、または
                    '-9000(DMS除外エラーの警告)

                    '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する

                    '納車待ちエリアチップ情報
                    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = _
                        blSC3240301.GetDeliverdChipData(dlrCd, brnCd)

                    '納車待ちエリアボタン情報
                    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                        GetInitializationDeliverdButtonInfo(dlrCd, brnCd)

                    result.DropofButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))
                    result.DropoffArea = HttpUtility.HtmlEncode(Me.DataTableToJson(dtChipInfo))

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                subButtonID = CBACK_DELIVERDCAR
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            ElseIf CBACK_ACTION_UNDO.Equals(argument.ButtonID) Then
                '洗車のundo処理
                returnCode = Me.UpdateUndoWashing(argument.ServiceInId, _
                                                  argument.JobDtlId, _
                                                  argument.StalluseId, _
                                                  argument.RowLockVersion)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If returnCode = ActionResult.Success Then
                '    '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する
                '    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetCarWashChipData(dlrCd, brnCd)
                '    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationCarWashButtonInfo(dlrCd, brnCd)
                '    result.CarwrashArea = HttpUtility.HtmlEncode(DataTableToJson(dtChipInfo))
                '    result.CarWashButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))
                'End If

                If returnCode = ActionResult.Success _
                OrElse returnCode = ActionResult.WarningOmitDmsError Then
                    '処理結果が下記の場合
                    '　　0(成功)、または
                    '-9000(DMS除外エラーの警告)

                    '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する

                    '洗車エリアチップ情報
                    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = _
                        blSC3240301.GetCarWashChipData(dlrCd, brnCd)

                    '洗車エリアボタン情報
                    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                        GetInitializationCarWashButtonInfo(dlrCd, brnCd)

                    result.CarwrashArea = HttpUtility.HtmlEncode(Me.DataTableToJson(dtChipInfo))
                    result.CarWashButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                '戻り値を格納
                subButtonID = CBACK_CARWASH

            ElseIf CBACK_ACTION_MOVETOWASH.Equals(argument.ButtonID) Then
                '洗車へ移動処理
                returnCode = Me.MoveToWash(argument.ServiceInId, _
                                           argument.JobDtlId, _
                                           argument.StalluseId, _
                                           argument.RowLockVersion)

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                ''戻り値格納
                'If returnCode = ActionResult.Success Then
                '    '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する
                '    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetDeliverdChipData(dlrCd, brnCd)
                '    Dim dtDeliButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationDeliverdButtonInfo(dlrCd, brnCd)
                '    Dim dtCarWashButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationCarWashButtonInfo(dlrCd, brnCd)
                '    result.DropofButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtDeliButtonInfo))
                '    result.DropoffArea = HttpUtility.HtmlEncode(DataTableToJson(dtChipInfo))
                '    result.CarWashButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtCarWashButtonInfo))
                'End If

                '戻り値格納
                If returnCode = ActionResult.Success _
                OrElse returnCode = ActionResult.WarningOmitDmsError Then
                    '処理結果が下記の場合
                    '　　0(成功)、または
                    '-9000(DMS除外エラーの警告)

                    '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する

                    '納車待ちエリアチップ情報
                    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = _
                        blSC3240301.GetDeliverdChipData(dlrCd, brnCd)

                    '納車待ちエリアボタン情報
                    Dim dtDeliButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                        GetInitializationDeliverdButtonInfo(dlrCd, brnCd)

                    '洗車エリアボタン情報
                    Dim dtCarWashButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                        GetInitializationCarWashButtonInfo(dlrCd, brnCd)

                    result.DropofButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtDeliButtonInfo))
                    result.DropoffArea = HttpUtility.HtmlEncode(Me.DataTableToJson(dtChipInfo))
                    result.CarWashButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtCarWashButtonInfo))

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                subButtonID = CBACK_DELIVERDCAR
            ElseIf CBACK_ACTION_MOVETODELIWAIT.Equals(argument.ButtonID) Then
                '納車待ちへ移動

                '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
                'returnCode = Me.MoveToDeliWait(argument.ServiceInId, _
                '               argument.JobDtlId, _
                '               argument.StalluseId, _
                '               argument.PickDeliType, _
                '               argument.RowLockVersion)
                returnCode = Me.MoveToDeliWait(argument.ServiceInId, _
                                               argument.JobDtlId, _
                                               argument.StalluseId, _
                                               argument.PickDeliType, _
                                               argument.RowLockVersion, _
                                               argument.RoNum)
                '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If returnCode = ActionResult.Success Then
                '    '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する
                '    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetCarWashChipData(dlrCd, brnCd)
                '    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationCarWashButtonInfo(dlrCd, brnCd)
                '    Dim dtDroppfButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationDeliverdButtonInfo(dlrCd, brnCd)
                '    result.CarwrashArea = HttpUtility.HtmlEncode(DataTableToJson(dtChipInfo))
                '    result.DropofButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtDroppfButtonInfo))
                '    result.CarWashButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))
                'End If

                If returnCode = ActionResult.Success _
                OrElse returnCode = ActionResult.WarningOmitDmsError Then
                    '処理結果が下記の場合
                    '　　0(成功)、または
                    '-9000(DMS除外エラーの警告)

                    '処理成功した場合、サブチップ再表示のため、サブチップ情報とボタンの情報を取得する

                    '洗車エリアチップ情報
                    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = _
                        blSC3240301.GetCarWashChipData(dlrCd, brnCd)

                    '洗車エリアボタン情報
                    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                        GetInitializationCarWashButtonInfo(dlrCd, brnCd)

                    '納車待ちエリアボタン情報
                    Dim dtDroppfButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                        GetInitializationDeliverdButtonInfo(dlrCd, brnCd)

                    result.CarwrashArea = HttpUtility.HtmlEncode(DataTableToJson(dtChipInfo))
                    result.DropofButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtDroppfButtonInfo))
                    result.CarWashButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                '戻り値を格納
                subButtonID = CBACK_CARWASH
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
            End If
            'イベントID
            result.ButtonID = argument.ButtonID
            'サブエリアID
            result.SubButtonID = subButtonID
            result.ResultCode = returnCode
            If returnCode <> 0 Then
                result.Message = HttpUtility.HtmlEncode(GetErrorMessage(returnCode))
            End If
            'update操作をした後のチップ情報を取得する

            '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
            ''サービス入庫IDを取得する
            'Dim svcinIdList As New List(Of Decimal)
            'svcinIdList.Add(argument.ServiceInId)
            'result.StallChip = HttpUtility.HtmlEncode(Me.DataTableToJson( _
            '                    blSC3240301.GetStallChipBySvcinId(nowDate, _
            '                                                  userContext, _
            '                                                  svcinIdList)))

            'サービス入庫IDを取得する
            result.StallChip = HttpUtility.HtmlEncode(Me.DataTableToJson( _
                                blSC3240301.GetStallChipAfterOperation(userContext.DlrCD, _
                                                                       userContext.BrnCD, _
                                                                       dtStartDate, _
                                                                       argument.PreRefreshDateTime)))

            '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END
        End Using
        Me.subchipDataJson = serializer.Serialize(result)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub


    ''' <summary>
    ''' サブチップMOVE処理
    ''' </summary>
    ''' <param name="argument">クライアントからの引数</param>
    ''' <param name="serializer">シリアライズ</param>
    ''' <param name="nowDate">現在時間</param>
    ''' <remarks></remarks>
    Private Sub SubChipMove(ByVal argument As CallBackArgumentClass, _
                               ByVal serializer As JavaScriptSerializer, _
                               ByVal nowDate As Date)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim returnCode As Long
        Dim result As New CallBackResultClass
        Dim userContext As StaffContext = StaffContext.Current
        Dim dlrCd As String = userContext.DlrCD
        Dim brnCd As String = userContext.BrnCD
        Dim dtStartDate As Date
        Dim dtEndDate As Date
        If argument.ShowDate <> Date.MinValue Then
            dtStartDate = Date.Parse(argument.ShowDate & " " & m_strStallStartTime & ":00", CultureInfo.InvariantCulture)
            dtEndDate = Date.Parse(argument.ShowDate & " " & m_strStallEndTime & ":00", CultureInfo.InvariantCulture)
        End If
        Using blSC3240301 As New SC3240301BusinessLogic

            returnCode = SubChipMoving(argument, dtStartDate, dtEndDate, nowDate)

            'イベントID
            result.ButtonID = argument.ButtonID
            'サブエリアID、サブエリアチップ情報を設定する
            If CBACK_NOSHOW_MOVING.Equals(argument.ButtonID) Then

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If returnCode = ActionResult.Success Then
                '    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetNoShowChipData(dlrCd, brnCd)
                '    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationNoShowButtonInfo(dlrCd, brnCd)
                '    result.NoShowButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))
                '    result.NoShowArea = HttpUtility.HtmlEncode(Me.DataTableToJson(dtChipInfo))
                'End If

                If returnCode = ActionResult.Success _
                OrElse returnCode = ActionResult.WarningOmitDmsError Then
                    '処理結果が下記の場合
                    '　　0(成功)、または
                    '-9000(DMS除外エラーの警告)

                    'NoShowエリアチップ情報
                    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = _
                        blSC3240301.GetNoShowChipData(dlrCd, brnCd)

                    'NoShowエリアボタン情報
                    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                        GetInitializationNoShowButtonInfo(dlrCd, brnCd)

                    result.NoShowButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))
                    result.NoShowArea = HttpUtility.HtmlEncode(Me.DataTableToJson(dtChipInfo))

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                result.SubButtonID = CBACK_NOSHOW
            ElseIf CBACK_STOP_MOVING.Equals(argument.ButtonID) Then

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                If returnCode = ActionResult.Success _
                OrElse returnCode = ActionResult.WarningOmitDmsError Then
                    '処理結果が下記の場合
                    '　　0(成功)、または
                    '-9000(DMS除外エラーの警告)

                    '中断エリアチップ情報
                    Dim dtChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = _
                        blSC3240301.GetStopChipData(dlrCd, brnCd)

                    '中断エリアボタン情報
                    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                        GetInitializationStopButtonInfo(dlrCd, brnCd)

                    result.JobStopButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))
                    result.JobStopArea = HttpUtility.HtmlEncode(Me.DataTableToJson(dtChipInfo))
                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                result.SubButtonID = CBACK_STOP
            ElseIf CBACK_RESERVE_ATTCHMENT.Equals(argument.ButtonID) Then

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

                'If returnCode = ActionResult.Success Then

                '    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                '    ' Dim dtreceptionChipInfo As SC3240301DataSet.SC3240301SubReceptionChipInfoDataTable = blSC3240301.GetReceptionChipData(dlrCd, brnCd)
                '    Dim dtreceptionChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = blSC3240301.GetReceptionChipData(dlrCd, brnCd)
                '    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

                '    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationReceptionButtonInfo(dlrCd, brnCd)

                '    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                '    Dim dtNoShowButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = GetInitializationNoShowButtonInfo(dlrCd, brnCd)
                '    result.NoShowButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtNoShowButtonInfo))
                '    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

                '    result.ReceptionButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))
                '    result.ReceptionArea = HttpUtility.HtmlEncode(Me.DataTableToJson(dtreceptionChipInfo))
                'End If

                If returnCode = ActionResult.Success _
                OrElse returnCode = ActionResult.WarningOmitDmsError Then
                    '処理結果が下記の場合
                    '　　0(成功)、または
                    '-9000(DMS除外エラーの警告)

                    LogServiceCommonBiz.OutputLog(32, "●■● 1.8 受付チップ情報の取得 START")

                    '受付エリアチップ情報
                    Dim dtreceptionChipInfo As SC3240301DataSet.SC3240301SubChipInfoDataTable = _
                        blSC3240301.GetReceptionChipData(dlrCd, brnCd)

                    LogServiceCommonBiz.OutputLog(32, "●■● 1.8 受付チップ情報の取得[取得件数：" & dtreceptionChipInfo.Count & "] END")
                    '■■■■■⑫～④の間の詳細ログ STRAT■■■■■

                    '■■■■■受付ボタンの情報を取得 2-1 1-0-0-0-0-0 START■■■■■
                    LogServiceCommonBiz.OutputLog(35, "●■● 2.1 受付ボタンの情報を取得 START")

                    '受付エリアボタン情報
                    Dim dtButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                        GetInitializationReceptionButtonInfo(dlrCd, brnCd)

                    LogServiceCommonBiz.OutputLog(35, "●■● 2.1 受付ボタンの情報を取得[取得件数:" & dtButtonInfo.Count & "] END")
                    '■■■■■受付ボタンの情報を取得 (★件数表示) 2-1 1-0-0-0-0-0 END■■■■■

                    '■■■■■NoShowボタンの情報を取得 2-15 2-0-0-0-0-0 START■■■■■
                    LogServiceCommonBiz.OutputLog(49, "●■● 2.2 NoShowボタンの情報を取得 START")

                    'NoShowエリアボタン情報
                    Dim dtNoShowButtonInfo As SC3240301DataSet.SC3240301ChipCountDataTable = _
                        GetInitializationNoShowButtonInfo(dlrCd, brnCd)

                    LogServiceCommonBiz.OutputLog(49, "●■● 2.2 NoShowボタンの情報を取得[取得件数:" & dtNoShowButtonInfo.Count & "]  END")
                    '■■■■■NoShowボタンの情報を取得(★件数表示) 2-15 2-0-0-0-0-0 ENDT■■■■■

                    result.ReceptionArea = HttpUtility.HtmlEncode(Me.DataTableToJson(dtreceptionChipInfo))
                    result.ReceptionButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtButtonInfo))
                    result.NoShowButtonInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtNoShowButtonInfo))

                End If

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                result.SubButtonID = CBACK_RECEPTION
            End If
            'update操作をした後のチップ情報を取得する
            'サービス入庫IDを取得する
            Dim svcinIdList As New List(Of Decimal)
            svcinIdList.Add(argument.ServiceInId)
            '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
            'Dim dtStallChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = _
            '    blSC3240301.GetStallChipBySvcinId(nowDate, _
            '                                        userContext, _
            '                                        svcinIdList)

            'サービス入庫IDを取得する
            Dim dtStallChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassStallChipInfoDataTable = _
                    blSC3240301.GetStallChipAfterOperation(userContext.DlrCD, _
                                                           userContext.BrnCD, _
                                                           dtStartDate, _
                                                           argument.PreRefreshDateTime)

            '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END
            'リレーションチップ情報を取得
            Dim dtRelationChipInfo As TabletSMBCommonClassDataSet.TabletSmbCommonClassRelationChipInfoDataDataTable = _
                            blSC3240301.GetAllRelationChipInfo(svcinIdList)
            'リレーションチップ情報をclientに渡す
            result.RelationChipInfo = HttpUtility.HtmlEncode(Me.DataTableToJson(dtRelationChipInfo))
            result.StallChip = HttpUtility.HtmlEncode(Me.DataTableToJson(dtStallChipInfo))
            result.ResultCode = returnCode
            result.Message = HttpUtility.HtmlEncode(GetErrorMessage(returnCode))
            result.SubChipKey = argument.SubChipKey
            Me.subchipDataJson = serializer.Serialize(result)

            '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 START
            Dim targetStallUseId As Decimal = 0

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START

            'If result.ResultCode = 0 Then
            '    '成功の場合

            '    If argument.StalluseId = 0 Then
            '        '受付エリアからストールに配置する時、ある場合チップが新規なので、ストール利用IDが持っていない

            '        targetStallUseId = _
            '            blSC3240301.GetMaxStallUseIdGroupByServiceId(argument.ServiceInId, _
            '                                                         userContext.DlrCD, _
            '                                                         userContext.BrnCD)

            '    Else
            '        'NoShow、中断サブエリアからストール上に配置する時、ストール利用IDを保持している

            '        targetStallUseId = argument.StalluseId

            '    End If

            'End If

            If result.ResultCode = ActionResult.Success _
            OrElse result.ResultCode = ActionResult.WarningOmitDmsError Then
                '処理結果が下記の場合
                '　　0(成功)、または
                '-9000(DMS除外エラーの警告)                

                If argument.StalluseId = 0 Then
                    '受付エリアからストールに配置する時、ある場合チップが新規なので、ストール利用IDが持っていない

                    targetStallUseId = _
                        blSC3240301.GetMaxStallUseIdGroupByServiceId(argument.ServiceInId, _
                                                                     userContext.DlrCD, _
                                                                     userContext.BrnCD)

                Else
                    'NoShow、中断サブエリアからストール上に配置する時、ストール利用IDを保持している

                    targetStallUseId = argument.StalluseId

                End If

            End If

            '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

            If 0 < targetStallUseId Then
                'ストール利用IDがあれば

                'チップ移動、リサイズ
                blSC3240301.SendNoticeWhenSetChipToBeLated(targetStallUseId, _
                                                           DateTimeFunc.Now(userContext.DlrCD), _
                                                           userContext)

            End If

            '2015/02/09 TMEJ 範 DMS連携版サービスタブレット SMB納車予定時刻通知機能開発 END

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        End Using
    End Sub
#Region "コールバック用内部クラス"
    ''' <summary>
    ''' コールバック用引数の内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackArgumentClass

        Public Property ButtonID As String                  'ボタンID
        Public Property ServiceInId As Decimal               'サービス入庫ID
        Public Property PickDeliType As String              '引取納車区分
        Public Property AddWorkStatus As String             '追加作業起票申請状態
        Public Property OrderNo As String                   '整備受注No
        Public Property StallId As Decimal                   'ストールID
        Public Property RoNum As String                     'RO番号
        Public Property JobDtlId As Decimal                    '作業内容ID
        Public Property StallUseStatus As String            'ストール利用ステータス
        Public Property TempFlg As String                   '仮置きフラグ
        Public Property CstId As Decimal                    '顧客ID
        Public Property CarWashRsltId As Decimal               '洗車実績ID
        Public Property CarWashNeedFlg As String            '洗車必要フラグ
        Public Property SvcStatus As String                 'サービスステータス
        Public Property UnStallId As Decimal                   'ストール非稼働ID
        Public Property RowLockVersion As Long              '行ロックバージョン
        Public Property StalluseId As Decimal                  'ストール利用ID
        Public Property ScheStartDatetime As Date           '予定開始日時
        Public Property ScheEndDatetime As Date             '予定終了日時
        Public Property ScheWorkTime As Long                '予定作業時間
        Public Property RestFlg As String                   '休憩取得フラグ
        Public Property SubChipKey As String                'サブチップキー
        Public Property ShowDate As Date                  '当ページ日付
        Public Property MainteCd As String                  '整備コード
        Public Property WorkSeq As Long                     '顧客承認連番 
        Public Property MercId As Decimal                      '商品ID
        Public Property SvcClassId As Decimal                  'サービス分類ID
        Public Property AcceptanceType As String            '受付区分
        Public Property ScheDeliDatetime As Date           '予定納車日時
        Public Property InspectionNeedFlg As String           '検査必要フラグ
        Public Property SubChipBoxId As String           'サブチップボックスId
        Public Property ResvStatus As String           '予約ステータス
        Public Property ScheSvcinDateTime As Date           '予定入庫日時
        '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
        Public Property PreRefreshDateTime As Date          '差分リフレッシュ日時
        '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END
    End Class
#End Region

#Region "クライアントに返すクラス"
    ''' <summary>
    ''' コールバック結果をクライアントに返すための内部クラス
    ''' </summary>
    ''' <remarks></remarks>
    Private Class CallBackResultClass

        ''' <summary>
        ''' 受付エリア
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ReceptionArea As String

        ''' <summary>
        ''' 洗車エリア
        ''' </summary>
        ''' <remarks></remarks>
        Public Property CarwrashArea As String

        ''' <summary>
        ''' 納車エリア
        ''' </summary>
        ''' <remarks></remarks>
        Public Property DropoffArea As String

        ''' <summary>
        ''' NoShowエリア
        ''' </summary>
        ''' <remarks></remarks>
        Public Property NoShowArea As String

        ''' <summary>
        ''' 中断エリア
        ''' </summary>
        ''' <remarks></remarks>
        Public Property JobStopArea As String

        ''' <summary>
        ''' 受付ボタンの情報
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ReceptionButtonInfo As String

        ''' <summary>
        ''' 洗車ボタンの情報
        ''' </summary>
        ''' <remarks></remarks>
        Public Property CarWashButtonInfo As String

        '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
        ''' <summary>
        ''' 完成検査エリアの情報
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ComplInsButtonInfo As String
        '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

        ''' <summary>
        ''' 納車ボタンの情報
        ''' </summary>
        ''' <remarks></remarks>
        Public Property DropofButtonInfo As String

        ''' <summary>
        ''' NoShowボタンの情報
        ''' </summary>
        ''' <remarks></remarks>
        Public Property NoShowButtonInfo As String

        ''' <summary>
        ''' 中断ボタンの情報
        ''' </summary>
        ''' <remarks></remarks>
        Public Property JobStopButtonInfo As String

        ''' <summary>
        ''' ストールチップ
        ''' </summary>
        ''' <remarks></remarks>
        Public Property StallChip As String

        ''' <summary>
        ''' リレーションチップ
        ''' </summary>
        ''' <remarks></remarks>
        Public Property RelationChipInfo As String

        ''' <summary>
        ''' 呼び出し元メソッド(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ButtonID As String

        ''' <summary>
        ''' 呼び出し元メソッド(JavaScript側)
        ''' </summary>
        ''' <remarks></remarks>
        Public Property SubButtonID As String

        ''' <summary>
        ''' 処理結果コード
        ''' </summary>
        ''' <remarks></remarks>
        Public Property ResultCode As Long

        ''' <summary>
        ''' メッセージ
        ''' </summary>
        ''' <remarks></remarks>
        Public Property Message As String

        ''' <summary>
        ''' サブチップKEY
        ''' </summary>
        ''' <remarks></remarks>
        Public Property SubChipKey As String

    End Class

#End Region

#End Region
    Private Sub GetBranchOperationgHours(ByVal userContext As StaffContext)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
                 , System.Reflection.MethodBase.GetCurrentMethod.Name))
        Using businessLogic As New SC3240301BusinessLogic
            Dim dtStartTime As Date
            Dim dtEndTime As Date
            Dim dt As TabletSMBCommonClassDataSet.TabletSmbCommonClassBranchOperatingHoursDataTable = businessLogic.GetBranchOperatingHours(userContext)
            dtStartTime = CType(dt(0)(0), Date)
            dtEndTime = CType(dt(0)(1), Date)

            'ストール開始時間
            m_strStallStartTime = Format(dtStartTime, "HH:mm")
            'ストール終了時間
            m_strStallEndTime = Format(dtEndTime, "HH:mm")
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    End Sub

    ''' <summary>
    ''' hiddenコントロールにclient端用の文言を設定する
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SendWordToClient()
        Dim sbWord As StringBuilder = New StringBuilder
        sbWord.Append("{")
        '全てwordをループする
        For Each i As WordID In [Enum].GetValues(GetType(WordID))
            With sbWord
                .Append("""")
                .Append(Convert.ToInt32(i).ToString(CultureInfo.InvariantCulture))
                .Append(""":""")
                .Append(WebWordUtility.GetWord(MY_PROGRAMID, CType(i, Decimal)))
                .Append(""",")
            End With
        Next i

        '最後の","を削除する
        sbWord.Remove(sbWord.Length - 1, 1)
        sbWord.Append("}")
        Me.hidSubMsgData.Value = sbWord.ToString()
    End Sub

    ''' <summary>
    ''' 操作結果により、エラー文言を取得
    ''' </summary>
    ''' <param name="inValue">操作結果</param>
    ''' <returns>エラー文言</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/01 TMEJ 明瀬 TMT２販社号口後フォロー BTS-281
    ''' </history>
    Private Function GetErrorMessage(ByVal inValue As Long) As String
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S." _
          , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim rtMessage As String = ""
        'エラーコードにより、エラータイプを分類する
        Select Case inValue
            Case ActionResult.OverlapError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 903)
            Case ActionResult.RowLockVersionError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 904)
            Case ActionResult.LockStallError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 905)
            Case ActionResult.DBTimeOutError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 901)
            Case ActionResult.DmsLinkageError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 911)
            Case ActionResult.ExceptionError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 902)
            Case ActionResult.NoDataFound
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 902)
            Case ActionResult.CheckAddWorkError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 906)
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            Case ActionResult.CheckInvoicePrintDateTimeError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 912)
            Case ActionResult.IC3802503ResultTimeOutError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 913)
            Case ActionResult.IC3802503ResultDmsError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 914)
            Case ActionResult.IC3802503ResultOtherError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 915)
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

                '2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応 START
            Case ActionResult.UnablePlanChipInWashingError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 916)
            Case ActionResult.UnablePlanChipInInspectingError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 917)
            Case ActionResult.UnablePlanChipAfterDeliveriedError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 918)
                '2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応 END

                '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START
            Case ActionResult.NoJobResultDataError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 919)
                '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 START
            Case ActionResult.WarningOmitDmsError
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 920)
                '2015/04/23 TMEJ 明瀬  DMS連携版サービスタブレット強制納車機能追加開発 END

                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            Case ActionResult.ChipOverlapUnavailableError
                'ストール使用不可と重複する配置である場合のエラー
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 921)
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

                '2015/10/08 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 Start

            Case ActionResult.IC3800903ResultRangeLower To ActionResult.IC3800903ResultRangeUpper
                '予約送信IFエラー(エラーコードが8000番台)
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, inValue)

                '2015/10/08 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 End

                '2015/04/01 TMEJ 明瀬 TMT２販社号口後フォロー BTS-281 START
            Case Else
                '予期せぬエラー
                rtMessage = WebWordUtility.GetWord(MY_PROGRAMID, 902)
                '2015/04/01 TMEJ 明瀬 TMT２販社号口後フォロー BTS-281 END

        End Select
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return rtMessage
    End Function
End Class
