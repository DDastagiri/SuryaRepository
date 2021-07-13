'-------------------------------------------------------------------------
'ServiceCommonClassBusinessLogic.vb
'-------------------------------------------------------------------------
'機能：サービス共通関数API
'補足：
'作成：2014/01/16 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
'更新：2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発
'更新：2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化
'更新：2015/08/17 TMEJ 明瀬 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新：2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化
'更新：2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更
'更新：2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応
'更新：2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001
'更新：2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能
'更新：2019/05/21 NSK 坂本 18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策
'更新：
'─────────────────────────────────────
Imports System.Text
Imports System.Net
Imports System.IO
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

Public Class ServiceCommonClassBusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "デフォルトコンストラクタ処理"
    '2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 START
    '●■●

    ' ''' <summary>
    ' ''' デフォルトコンストラクタ処理
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Sub New()
    'End Sub

    ''' <summary>
    ''' デフォルトコンストラクタ処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(Optional ByVal logFlg As Boolean = False)

        'If logFlg Then
        '
        '    Dim logPos As String = String.Empty
        '
        '    Using serviceCommonTa As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter
        '
        '        logPos = serviceCommonTa.GetSystemSettingValue("TABLETSMB_LOG_POS")(0).SETTING_VAL
        '
        '    End Using
        '
        '    TabletSMBLogPos = logPos.Split(CChar(","))
        '
        'End If

    End Sub

    '2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 END

#End Region

    '2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 START

#Region "暫定ログ出力"

    ''' <summary>
    ''' ログ出力箇所管理配列
    ''' </summary>
    ''' <remarks>
    ''' TB_M_SYSTEM_SETTINGから取得した0と1のカンマ区切り文字列を
    ''' カンマ(,)でSplitした値を保存する
    ''' 配列内の値が
    ''' 　0：ログ出力しない
    ''' 　1：ログ出力する
    ''' 　※上記以外の値があった場合はログ出力しない
    ''' </remarks>
    Private TabletSMBLogPos As String()

    ''' <summary>
    ''' ログ出力を実施(性能調査用の暫定ログ)
    ''' </summary>
    ''' <param name="num">ログ連番</param>
    ''' <param name="logString">ログに出力したい文字列</param>
    ''' <remarks></remarks>
    Public Sub OutputLog(ByVal num As Integer, _
                         ByVal logString As String)

        ''コンストラクタで取得している値だが、万が一消えている場合を考慮
        'If TabletSMBLogPos.Length = 0 Then
        '
        '    Dim logPos As String = String.Empty
        '
        '    Using serviceCommonTa As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter
        '
        '        logPos = serviceCommonTa.GetSystemSettingValue("TABLETSMB_LOG_POS")(0).SETTING_VAL
        '
        '    End Using
        '
        '    TabletSMBLogPos = logPos.Split(CChar(","))
        '
        'End If
        '
        'If "1".Equals(TabletSMBLogPos(num)) Then
        '    Logger.Error(logString)
        'End If

    End Sub

#End Region

    '2015/07/02 TMEJ 明瀬 ITXXXX_タブレットSMB性能調査 ログ出力強化 END

#Region "PublicEnum"

    ''' <summary>
    ''' 基幹コード区分
    ''' </summary>
    Public Enum DmsCodeType

        ''' <summary>
        ''' 区分なし
        ''' </summary>
        ''' <remarks></remarks>
        None = 0

        ''' <summary>
        ''' 販売店コード
        ''' </summary>
        ''' <remarks></remarks>
        DealerCode = 1

        ''' <summary>
        ''' 店舗コード
        ''' </summary>
        ''' <remarks></remarks>
        BranchCode = 2

        ''' <summary>
        ''' ストールID
        ''' </summary>
        ''' <remarks></remarks>
        StallId = 3

        ''' <summary>
        ''' 顧客分類
        ''' </summary>
        ''' <remarks></remarks>
        CustomerClass = 4

        ''' <summary>
        ''' 作業ステータス
        ''' </summary>
        ''' <remarks></remarks>
        WorkStatus = 5

        ''' <summary>
        ''' 中断理由区分
        ''' </summary>
        ''' <remarks></remarks>
        JobStopReasonType = 6

        ''' <summary>
        ''' チップステータス
        ''' </summary>
        ''' <remarks></remarks>
        ChipStatus = 7

        ''' <summary>
        ''' 希望連絡時間帯
        ''' </summary>
        ''' <remarks></remarks>
        ContactTimeZone = 8

        ''' <summary>
        ''' メーカー区分
        ''' </summary>
        ''' <remarks></remarks>
        MakerType = 9

    End Enum

    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 START

    ''' <summary>
    ''' 処理結果コード
    ''' </summary>
    Public Enum ResultCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        ''' <remarks></remarks>
        Success = 0

        ''' <summary>
        ''' 失敗
        ''' </summary>
        ''' <remarks></remarks>
        Failure = 9999

    End Enum

    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

#End Region

#Region "定数"

    ''' <summary>
    ''' 全販売店を意味するワイルドカード販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllDealerCode As String = "XXXXX"

    ''' <summary>
    ''' 全店舗を意味するワイルドカード店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllBranchCode As String = "XXX"

    '2015/08/17 TMEJ 明瀬 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

    ''' <summary>
    ''' 文言テーブルの画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MyWordDisplayId As String = "ServiceCommon"

    ''' <summary>
    ''' 顧客車両区分：所有者
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CstVclTypeOwner As String = "1"

    ''' <summary>
    ''' 顧客車両区分：使用者
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CstVclTypeUser As String = "2"

    ''' <summary>
    ''' 顧客車両区分：その他
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CstVclTypeOther As String = "3"

    ''' <summary>
    ''' 顧客種別：自社客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OwnCst As String = "1"

    ''' <summary>
    ''' 顧客種別：未取引客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotDealCst As String = "2"

    '2015/08/17 TMEJ 明瀬 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

    ''' <summary>
    ''' 文言紐付マスタの区分値のワイルドカード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WordRelationTypeValAll As String = "X"

    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

    '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START

    ''' <summary>
    ''' システム設定名（車両登録番号の区切文字）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SysRegNumDlmtr As String = "REG_NUM_DELIMITER"

    '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' i-CROP→DMSの値に変換された値を取得する
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="dmsCodeType">基幹コード区分</param>
    ''' <param name="icropCD1">iCROPコード1</param>
    ''' <param name="icropCD2">iCROPコード2</param>
    ''' <param name="icropCD3">iCROPコード3</param>
    ''' <param name="account">アカウント</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 基幹コード区分(1～7)によって、引数に設定する値が異なる
    ''' ※ここに全て記載すると非常に長くなるため、TB_M_DMS_CODE_MAPのテーブル定義書を参照して下さい
    ''' </remarks>
    Public Function GetIcropToDmsCode(ByVal dealerCD As String, _
                                      ByVal dmsCodeType As DmsCodeType, _
                                      ByVal icropCD1 As String, _
                                      ByVal icropCD2 As String, _
                                      ByVal icropCD3 As String, _
                                      Optional account As String = "") As ServiceCommonClassDataSet.DmsCodeMapDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dealerCD, _
                                  CType(dmsCodeType, Integer), _
                                  icropCD1, _
                                  icropCD2, _
                                  icropCD3))

        '戻り値
        Dim dt As ServiceCommonClassDataSet.DmsCodeMapDataTable

        Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter
            dt = ta.GetIcropToDmsCode(AllDealerCode, _
                                      dealerCD, _
                                      dmsCodeType, _
                                      icropCD1, _
                                      icropCD2, _
                                      icropCD3)
        End Using

        If dt.Count <= 0 Then

            'データが取得できない場合
            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}.{1} WARN：No data found. ", _
                                       Me.GetType.ToString, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))

        End If

        'アカウント情報と取得項目のチェック
        If Not (String.IsNullOrEmpty(account)) AndAlso _
           (dmsCodeType = ServiceCommonClassBusinessLogic.DmsCodeType.DealerCode OrElse _
           dmsCodeType = ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode OrElse _
           dmsCodeType = ServiceCommonClassBusinessLogic.DmsCodeType.StallId) Then
            'アカウントが存在する場合且つ、販売店・店舗・ストールの情報を取得する場合
            '変換したアカウントを格納
            dt(0).ACCOUNT = account.Split(CChar("@"))(0)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} QUERY:COUNT = {2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dt.Count))

        Return dt

    End Function

    ''' <summary>
    ''' DMS→i-CROPの値に変換された値を取得する
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="dmsCodeType">基幹コード区分</param>
    ''' <param name="dmsCD1">基幹コード1</param>
    ''' <param name="dmsCD2">基幹コード2</param>
    ''' <param name="dmsCD3">基幹コード3</param>
    ''' <param name="dmsAccount">基幹アカウント</param>
    ''' <returns></returns>
    ''' <remarks>
    ''' 基幹コード区分(1～7)によって、引数に設定する値が異なる
    ''' ※ここに全て記載すると非常に長くなるため、TB_M_DMS_CODE_MAPのテーブル定義書を参照して下さい
    ''' </remarks>
    Public Function GetDmsToIcropCode(ByVal dealerCD As String, _
                                      ByVal dmsCodeType As DmsCodeType, _
                                      ByVal dmsCD1 As String, _
                                      ByVal dmsCD2 As String, _
                                      ByVal dmsCD3 As String, _
                                      Optional dmsAccount As String = "") As ServiceCommonClassDataSet.DmsCodeMapDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dealerCD, _
                                  CType(dmsCodeType, Integer), _
                                  dmsCD1, _
                                  dmsCD2, _
                                  dmsCD3, _
                                  dmsAccount))

        '戻り値
        Dim dt As ServiceCommonClassDataSet.DmsCodeMapDataTable

        Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter
            dt = ta.GetDmsToIcropCode(AllDealerCode, _
                                      dealerCD, _
                                      dmsCodeType, _
                                      dmsCD1, _
                                      dmsCD2, _
                                      dmsCD3)
        End Using

        If dt.Count <= 0 Then

            'データが取得できない場合
            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}.{1} WARN：No data found. ", _
                                       Me.GetType.ToString, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))

        End If

        'アカウント情報と取得項目のチェック
        If Not (String.IsNullOrEmpty(dmsAccount)) AndAlso _
           (dmsCodeType = ServiceCommonClassBusinessLogic.DmsCodeType.DealerCode OrElse _
           dmsCodeType = ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode OrElse _
           dmsCodeType = ServiceCommonClassBusinessLogic.DmsCodeType.StallId) Then
            'アカウントが存在する場合且つ、販売店・店舗・ストールの情報を取得する場合
            '変換したアカウントを格納
            dt(0).ACCOUNT = String.Concat(dmsAccount, "@", dt(0).CODE1)

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} QUERY:COUNT = {2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dt.Count))

        Return dt

    End Function

    ''' <summary>
    ''' システム設定値を設定値名を条件に取得する
    ''' </summary>
    ''' <param name="settingName">システム設定値名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSystemSettingValueBySettingName(ByVal settingName As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:settingName={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  settingName))

        '戻り値
        Dim retValue As String = String.Empty

        '18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 START
        '自分のテーブルアダプタークラスインスタンスを生成
        'Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter

        '    'システム設定から取得
        '    Dim dt As ServiceCommonClassDataSet.SystemSettingDataTable _
        '        = ta.GetSystemSettingValue(settingName)

        'If 0 < dt.Count Then

        '    設定値を取得
        '    retValue = dt.Item(0).SETTING_VAL

        'End If

        'End Using

        Dim systemSetting As New SystemSetting

        Dim row As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow _
                = systemSetting.GetSystemSetting(settingName)

            If row IsNot Nothing Then

                '設定値を取得
            retValue = row.SETTING_VAL

            End If

        '18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:{1}={2}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  settingName, _
                                  retValue))

        Return retValue

    End Function

    ''' <summary>
    ''' 販売店システム設定値を設定値名を条件に取得する
    ''' </summary>
    ''' <param name="settingName">販売店システム設定値名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDlrSystemSettingValueBySettingName(ByVal settingName As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:settingName={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  settingName))

        '戻り値
        Dim retValue As String = String.Empty

        'ログイン情報
        Dim userContext As StaffContext = StaffContext.Current

        '18PRJ00XXX_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 START
        ''自分のテーブルアダプタークラスインスタンスを生成
        'Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter

        '    '販売店システム設定から取得
        '    Dim dt As ServiceCommonClassDataSet.SystemSettingDataTable _
        '                            = ta.GetDlrSystemSettingValue(userContext.DlrCD, _
        '                                                                      userContext.BrnCD, _
        '                                                                      AllDealerCode, _
        '                                                                      AllBranchCode, _
        '                                                                      settingName)

        '    If 0 < dt.Count Then

        '        '設定値を取得
        '        retValue = dt.Item(0).SETTING_VAL

        '    End If

        'End Using
        Dim systemSetting As New SystemSettingDlr

        Dim row As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow _
                = systemSetting.GetEnvSetting(userContext.DlrCD, _
                                                    userContext.BrnCD, _
                                                    settingName)

            If row IsNot Nothing Then

                '設定値を取得
                retValue = row.SETTING_VAL

            End If

        '18PRJ00XXX_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:{1}={2}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  settingName, _
                                  retValue))

        Return retValue

    End Function        

    ''' <summary>
    ''' 新文言マスタの文言を文言コードを条件に取得する
    ''' </summary>
    ''' <param name="wordCode">文言コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNewWordMasterInfo(ByVal wordCode As String) As ServiceCommonClassDataSet.WordMasterDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S IN:wordCode={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  wordCode))

        '戻り値
        Dim dt As ServiceCommonClassDataSet.WordMasterDataTable

        '自分のテーブルアダプタークラスインスタンスを生成
        Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter

            '販売店システム設定から取得
            dt = ta.GetNewWordMasterInfo(wordCode)

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_E OUT:COUNT={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dt.Count))

        Return dt

    End Function

    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 START

    ''' <summary>
    ''' サービス基幹連携のエラーコードが除外対象かどうかを確認
    ''' </summary>
    ''' <param name="inInterfaceType">インターフェース区分(1:予約送信 / 2:ステータス送信 / 3:作業実績送信)</param>
    ''' <param name="inDmsResultCode">DMS結果コード(DMSから返却されたXML内の結果コード)</param>
    ''' <returns>True：除外コードに該当する / False：除外コードに該当しない</returns>
    ''' <remarks></remarks>
    Public Function IsOmitDmsErrorCode(ByVal inInterfaceType As String, _
                                       ByVal inDmsResultCode As String) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_S IN:inInterfaceType={2}, inDmsResultCode={3}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inInterfaceType, _
                                  inDmsResultCode))

        '戻り値(初期値はFalse「除外コードに該当しない」)
        Dim retValue As Boolean = False

        '自分のテーブルアダプタークラスインスタンスを生成
        Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter

            'サービス基幹連携除外エラーからレコード件数を取得する
            Dim dt As ServiceCommonClassDataSet.RowCountDataTable _
                                    = ta.GetOmitDmsErrorCount(inInterfaceType, _
                                                              inDmsResultCode)

            '該当するレコードが1件以上
            If 0 < dt(0).Count Then

                '「除外コードに該当する」と判定
                retValue = True

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_E OUT:IsOmitDmsErrorCode={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  retValue))

        Return retValue

    End Function

    ''' <summary>
    ''' サービスDMS納車実績ワークを取得
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <returns>サービスDMS納車実績ワークテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetWorkServiceDmsResultDelivery(ByVal inServiceInId As Decimal) As ServiceCommonClassDataSet.WorkServiceDmsResultDeliveryDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_S IN:inServiceInId={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inServiceInId))

        '戻り値
        Dim retTable As ServiceCommonClassDataSet.WorkServiceDmsResultDeliveryDataTable = Nothing

        '自分のテーブルアダプタークラスインスタンスを生成
        Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter

            'サービスDMS納車実績ワークからデータを取得する
            retTable = ta.GetWorkServiceDmsResultDeliveryData(inServiceInId)

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_E OUT:RowCount={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  retTable.Count))

        Return retTable

    End Function

    ''' <summary>
    ''' サービスDMS納車実績ワークを削除
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <returns>
    ''' 削除レコードが0件の場合、ResultCode.Failure(9999)を返却
    ''' それ以外の場合、ResultCode.Success(0)を返却
    ''' </returns>
    ''' <remarks></remarks>
    Public Function DeleteWorkServiceDmsResultDelivery(ByVal inServiceInId As Decimal) As ResultCode

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_S IN:inServiceInId={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inServiceInId))

        '戻り値(初期値はSuccess:0)
        Dim returnCode As ResultCode = ResultCode.Success

        '自分のテーブルアダプタークラスインスタンスを生成
        Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter

            'サービスDMS納車実績ワークからデータを削除する
            Dim deleteCount As Integer = ta.DeleteWorkServiceDmsResultDeliveryData(inServiceInId)

            '削除レコードが0件の場合
            If deleteCount = 0 Then

                '戻り値にResultCode.Failure(9999)を設定
                returnCode = ResultCode.Failure

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_E OUT:returnCode={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  returnCode))

        Return returnCode

    End Function

    '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

    '2015/08/17 TMEJ 明瀬 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

    ''' <summary>
    ''' 顧客車両区分を付与した顧客氏名を取得
    ''' </summary>
    ''' <param name="inCstName">顧客氏名</param>
    ''' <param name="inCstVclType">顧客車両区分</param>
    ''' <param name="inCstType">顧客種別</param>
    ''' <returns>
    ''' 「顧客氏名 + 半角スペース(1) + (顧客車両区分)」
    ''' 例：テスト太郎 (Owner)
    ''' </returns>
    ''' <remarks></remarks>
    Public Function GetCstNameWithCstVclType(ByVal inCstName As String, _
                                             ByVal inCstVclType As String, _
                                             ByVal inCstType As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_S IN:inCstName={2}, inCstVclType={3}, inCstType={4}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inCstName, _
                                  inCstVclType, _
                                  inCstType))

        '戻り値
        Dim returnValue As String = String.Empty

        '引数の顧客氏名が存在しない場合
        If String.IsNullOrWhiteSpace(inCstName) Then

            Return returnValue

        End If

        '戻り値に引数の顧客氏名を設定
        returnValue = inCstName

        '「({0})」の文言を取得　※{0}に「所有者」等が入る
        Dim word As String = WebWordUtility.GetWord(MyWordDisplayId, 1)

        Dim cstVclTypeWord As String = String.Empty

        '顧客種別の判定(1:自社客, 2:未取引客)

        If OwnCst.Equals(inCstType) Then
            '自社客の場合

            Select Case inCstVclType

                Case CstVclTypeOwner
                    '(所有者)
                    cstVclTypeWord = String.Format(CultureInfo.InvariantCulture, _
                                                   word, _
                                                   WebWordUtility.GetWord(MyWordDisplayId, 2))

                Case CstVclTypeUser
                    '(使用者)
                    cstVclTypeWord = String.Format(CultureInfo.InvariantCulture, _
                                                   word, _
                                                   WebWordUtility.GetWord(MyWordDisplayId, 3))

                Case CstVclTypeOther
                    '(その他)
                    cstVclTypeWord = String.Format(CultureInfo.InvariantCulture, _
                                                   word, _
                                                   WebWordUtility.GetWord(MyWordDisplayId, 4))

                Case Else
                    '空文字
                    cstVclTypeWord = String.Empty

            End Select


        ElseIf NotDealCst.Equals(inCstType) Then
            '未取引客の場合

            '(未取)
            cstVclTypeWord = String.Format(CultureInfo.InvariantCulture, _
                                           word, _
                                           WebWordUtility.GetWord(MyWordDisplayId, 5))

        Else

            '空文字
            cstVclTypeWord = String.Empty

        End If

        '顧客氏名 + 半角スペース(1) + (顧客車両区分)の文字列を作成
        returnValue = inCstName + Space(1) + cstVclTypeWord

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_E OUT:returnValue={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  returnValue))

        Return returnValue

    End Function

    '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 START
    ' ''' <summary>
    ' ''' 顧客車両区分を付与した顧客氏名を取得
    ' ''' </summary>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="inCstId">顧客ID</param>
    ' ''' <param name="inVclId">車両ID</param>
    ' ''' <returns>
    ' ''' 「顧客氏名 + 半角スペース(1) + (顧客車両区分)」
    ' ''' 例：テスト太郎 (Owner)
    ' ''' </returns>
    ' ''' <remarks></remarks>
    'Public Function GetCstNameWithCstVclType(ByVal inDealerCode As String, _
    '                                         ByVal inCstId As Decimal, _
    '                                         ByVal inVclId As Decimal) As String

    ''' <summary>
    ''' 顧客車両区分を付与した顧客氏名を取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inCstId">顧客ID</param>
    ''' <param name="inVclId">車両ID</param>
    ''' <param name="inCstVclType">顧客車両区分</param>
    ''' <returns>
    ''' 「顧客氏名 + 半角スペース(1) + (顧客車両区分)」
    ''' 例：テスト太郎 (Owner)
    ''' </returns>
    ''' <remarks></remarks>
    Public Function GetCstNameWithCstVclType(ByVal inDealerCode As String, _
                                         ByVal inCstId As Decimal, _
                                         ByVal inVclId As Decimal, _
                                         ByVal inCstVclType As String) As String
        '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_S IN:inDealerCode={2}, inCstId={3}, inVclId={4}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inDealerCode, _
                                  inCstId, _
                                  inVclId))

        '戻り値
        Dim returnValue As String = String.Empty

        Dim cstVclTable As ServiceCommonClassDataSet.CustomerVehicleDataTable = Nothing

        Dim cstVclRow As ServiceCommonClassDataSet.CustomerVehicleRow = Nothing

        Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter

            '顧客車両情報を取得
            cstVclTable = ta.GetCustomerVehicleData(inDealerCode, _
                                                    inCstId, _
                                                    inVclId)

        End Using

        If Not IsNothing(cstVclTable) _
        AndAlso 0 < cstVclTable.Count Then

            '複数行返却されている場合、
            '同一販売店コード、同一顧客ID、同一車両IDで所有者、使用者、その他のデータが少なくとも2つ以上存在している
            'その場合、所有者、使用者、その他の順で優先的にデータを取得
            '※通常では考えられない
            cstVclRow = cstVclTable(0)

            '顧客氏名 + 半角スペース(1) + (顧客車両区分)の文字列を作成

            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 START
            '同一顧客ID・車両IDの場合でも予約取得時の顧客車両区分が表示されるようにするため、サービス入庫．顧客車両区分を引数に指定するように修正
            'returnValue = Me.GetCstNameWithCstVclType(cstVclRow.CST_NAME, _
            '                                          cstVclRow.CST_VCL_TYPE, _
            '                                          cstVclRow.CST_TYPE)
            returnValue = Me.GetCstNameWithCstVclType(cstVclRow.CST_NAME, _
                                                      inCstVclType, _
                                                      cstVclRow.CST_TYPE)
            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 END

        Else

            returnValue = String.Empty

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.{1}_E OUT:returnValue={2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      returnValue))

        Return returnValue

    End Function

    '2015/08/17 TMEJ 明瀬 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START
    ''' <summary>
    ''' 特定のDMSエラーコードから文言コードを取得
    ''' </summary>
    ''' <param name="inTypeCode">文言紐付けマスタ 区分種別コード</param>
    ''' <param name="inDmsErrorCode">DMSエラーコード</param>
    ''' <returns>エラーコード(文言コード)</returns>
    ''' <remarks></remarks>
    Public Function ParticularDmsErrorCodeToWordCode(ByVal inTypeCode As String, _
                                                     ByVal inDmsErrorCode As String) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_S IN:inTypeCode={2}, inDmsErrorCode={3}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inTypeCode, _
                                  inDmsErrorCode))

        '戻り値
        Dim returnValue As Integer = 0

        Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter

            '文言コード情報を取得
            Dim wordCdTable As ServiceCommonClassDataSet.WordRelationDataTable =
                ta.ParticularDmsErrorCodeToWordCode(inTypeCode, inDmsErrorCode)

            If Not IsNothing(wordCdTable) _
            AndAlso 0 < wordCdTable.Count Then
                '文言コード情報が存在する場合
                'DataTableの1行目の文言を返却
                Dim wordCdRow As ServiceCommonClassDataSet.WordRelationRow = wordCdTable(0)

                '文言コードをパース
                If Not Integer.TryParse(wordCdRow.WORD_CD, returnValue) Then
                    'パースに失敗した場合-1を返却
                    returnValue = -1

                End If

            Else
                '文言コード情報が存在しない場合
                '文言コード情報（共通定義）を取得
                Dim wordCdTableTypeValAll As ServiceCommonClassDataSet.WordRelationDataTable =
                    ta.ParticularDmsErrorCodeToWordCode(inTypeCode, WordRelationTypeValAll)

                If Not IsNothing(wordCdTableTypeValAll) _
                    AndAlso 0 < wordCdTableTypeValAll.Count Then
                    '文言コード情報（共通定義）が存在する場合
                    'DataTableの1行目の文言を返却
                    Dim wordCdRowTypeValAll As ServiceCommonClassDataSet.WordRelationRow = wordCdTableTypeValAll(0)

                    '文言コードをパース
                    If Not Integer.TryParse(wordCdRowTypeValAll.WORD_CD, returnValue) Then
                        'パースに失敗した場合-1を返却
                        returnValue = -1

                    End If

                Else
                    '文言コード情報（共通定義）が存在しない場合
                    '-1を返却
                    returnValue = -1

                End If

            End If

        End Using


        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.{1}_E OUT:returnValue={2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      returnValue))

        Return returnValue

    End Function

    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

    '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
    ''' <summary>
    ''' 車両登録番号検索ワード変換
    ''' </summary>
    ''' <param name="inSearchWord">検索ワード</param>
    ''' <returns>「*」と区切り文字を取り除いた検索ワード</returns>
    ''' <remarks></remarks>
    Public Function ConvertVclRegNumWord(ByVal inSearchWord As String) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_S IN:inSearchWord={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inSearchWord))

        '区切り文字を取得
        Dim regNumDlmtr As String = GetSystemSettingValueBySettingName(SysRegNumDlmtr)

        '区切り文字が存在する場合
        If Not String.IsNullOrEmpty(regNumDlmtr) Then

            '文字間に入力された'*'を検索文字列より削除
            inSearchWord = inSearchWord.Replace("*", String.Empty)

            '取得された区切文字を'*'で分割
            Dim regNumDlmtrList As List(Of String) = regNumDlmtr.Split("*"c).ToList

            For Each dlmtr As String In regNumDlmtrList
                '区切り文字を'*'で分割
                inSearchWord = inSearchWord.Replace(dlmtr, String.Empty)
            Next

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_E OUT:returnValue={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inSearchWord))

        Return inSearchWord

    End Function
    '2015/11/10 TM 皆川 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ''' <summary>
    ''' サービス入庫IDからストールリスト取得
    ''' </summary>
    ''' <param name="searchServiceInId">検索予約ID</param>
    ''' <param name="resultsFlg">実績フラグ(0：中断実績チップを含まない、1：実績チップを含む全てのチップ)</param>
    ''' <param name="cancelFlg">キャンセルフラグ(0：キャンセルチップを含まない、1：キャンセルチップを含む)</param>
    ''' <returns>ストール情報</returns>
    ''' <remarks></remarks>
    Public Function GetStallListToReserve(ByVal searchServiceInId As Decimal, _
                                          ByVal resultsFlg As Long, _
                                          ByVal cancelFlg As Long) As ServiceCommonClassDataSet.StallInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_S IN:searchServiceInId={2}, resultsFlg={3}, cancelFlg={4}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  searchServiceInId, _
                                  resultsFlg, _
                                  cancelFlg))

        ' サービス入庫IDが指定されていない場合は、空のデータテーブルを返却する。
        If searchServiceInId <= 0 Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      0))

            Return New ServiceCommonClassDataSet.StallInfoDataTable
        End If

        Dim dt As ServiceCommonClassDataSet.StallInfoDataTable
        Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter
            dt = ta.GetDBStallListToReserve(searchServiceInId, resultsFlg, cancelFlg)
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} QUERY:COUNT = {2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dt.Count))
        Return dt
    End Function

    ''' <summary>
    ''' ストールから通知Push送信先リスト取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallIdList">ストールIDリスト</param>
    ''' <param name="operationCodeList">オペレーションコードリスト</param>
    ''' <returns>スタッフ情報</returns>
    ''' <remarks></remarks>
    Public Function GetNoticeSendAccountListToStall(ByVal dealerCode As String, _
                                                    ByVal branchCode As String, _
                                                    ByVal stallIdList As List(Of Decimal), _
                                                    ByVal operationCodeList As List(Of Decimal)) As ServiceCommonClassDataSet.StaffInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1}_S IN:dealerCode={2}, branchCode={3}, stallIdList={4}, operationCodeList={5}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dealerCode, _
                                  branchCode, _
                                  IsNothing(stallIdList).ToString, _
                                  IsNothing(operationCodeList).ToString))

        Using staffInfoWork As New ServiceCommonClassDataSet.StaffInfoDataTable

            ' 取得対象のオペレーションコードリストが未指定の場合
            If IsNothing(operationCodeList) OrElse operationCodeList.Count <= 0 Then
                Return New ServiceCommonClassDataSet.StaffInfoDataTable
            End If

            Using ta As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter
                Using mergeStaffInfo As New ServiceCommonClassDataSet.StaffInfoDataTable
                    ' ストールIDが指定されている場合
                    If IsNothing(stallIdList) = False OrElse stallIdList.Count > 0 Then
                        ' 指定ストールIDよりスタッフ情報を取得しマージ
                        Dim staffInfoToStall As ServiceCommonClassDataSet.StaffInfoDataTable = _
                            ta.GetDBStaffInfoToStall(stallIdList, operationCodeList)
                        mergeStaffInfo.Merge(staffInfoToStall)
                    End If

                    ' 指定権限コードよりスタッフ情報を取得しマージ
                    Dim staffInfoToOpeCode As ServiceCommonClassDataSet.StaffInfoDataTable = _
                            ta.GetDBStaffInfoToOpeCode(dealerCode, branchCode, operationCodeList)
                    mergeStaffInfo.Merge(staffInfoToOpeCode)

                    ' 重複行を排除する
                    Using view As New DataView(mergeStaffInfo)
                        Dim distStaffInfo As DataTable = view.ToTable(True, {"ACCOUNT", "OPERATIONCODE"})
                        For Each row As DataRow In distStaffInfo.Rows
                            staffInfoWork.AddStaffInfoRow(row.Item(0).ToString, row.Item(1).ToString)
                        Next
                    End Using
                End Using
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} QUERY:COUNT = {2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  staffInfoWork.Count))

            '返却用データテーブル
            Dim retrunDt As ServiceCommonClassDataSet.StaffInfoDataTable = _
                CType(staffInfoWork.Copy(), ServiceCommonClassDataSet.StaffInfoDataTable)
            Return retrunDt
        End Using
    End Function

    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

    '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START

    ''' <summary>
    ''' 作業中の関連チップの配置されているストール名を取得する
    ''' </summary>
    ''' <param name="rezId">予約ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <returns>ストール名</returns>
    ''' <remarks></remarks>
    Public Function GetStallNameWithRelationChip(ByVal rezId As String, _
                                                 ByVal stallId As Decimal) As String

        Using adapter As New ServiceCommonClassDataSetTableAdapters.ServiceCommonClassTableAdapter
            Dim dt As ServiceCommonClassDataSet.StallNameDataTable = Nothing
            '引数チェック
            If Not String.IsNullOrEmpty(rezId) Then
                dt = adapter.GetStallInfoOfRelationChip(CDec(rezId))
            End If

            'データが取れなかったら空文字を返す
            If IsNothing(dt) OrElse dt.Count <= 0 Then
                Return String.Empty
            End If

            '作業中の関連チップが同じストールにある場合、空文字を返す
            If dt(0).STALL_ID = CStr(stallId) OrElse String.IsNullOrWhiteSpace(dt(0).STALLNAME) Then
                Return String.Empty
            End If

            Return CStr(dt(0).STALLNAME)

        End Using
    End Function

    '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END


#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class