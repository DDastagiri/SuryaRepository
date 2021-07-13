'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250104BusinessLogic.vb
'─────────────────────────────────────
'機能： 部品説明画面 ビジネスロジック
'補足： 
'作成： 2014/08/XX NEC 上野
'更新： 
'更新： 
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports Toyota.eCRB.iCROP.DataAccess.SC3250104
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization
Imports System.Data

''' <summary>
''' 部品説明画面のビジネスロジック
''' </summary>
''' <remarks>部品説明のビジネスロジッククラス</remarks>
Public Class SC3250104BusinessLogic

    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "PublicConst"

#Region "ログ文言"
    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConsLogStart As String = "Start"

    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ConsLogEnd As String = "End"
#End Region


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

    ''' <summary>サービス戻り値(ResultID)：ServiceSuccess</summary>
    Private Const ServiceSuccess As String = "0"

    Public Const DATABASE_ERROR As Integer = -2

    Public Const WEBSERVICE_ERROR As Integer = -1

    ''' <summary>
    ''' メーカータイプ
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum MakerType
        ''' <summary>1: トヨタ</summary>
        TOYOTA = 1
        ''' <summary>2:レクサス</summary>
        LEXUS
        ''' <summary>3:その他</summary>
        ELSE_MAKER
    End Enum

    ''' <summary>
    ''' タイミング
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DEF_TIMING
        ''' <summary>車両情報特定前</summary>
        Public Const UNKNOWN_VEHICLE As String = "0"
        ''' <summary>R/O発行前（顧客承認前）</summary>
        Public Const BEFORE_PUBLISH As String = "10"
        ''' <summary>R/O発行後（顧客承認後）</summary>
        Public Const AFTER_PUBLISH As String = "50"
        ''' <summary>追加作業起票後（PS見積もり後）</summary>
        Public Const AFTER_ADD_WK_MAKE As String = "35"
        ''' <summary>Close Job後</summary>
        Public Const COMPLETE As String = "85"
        ''' <summary>キャンセル</summary>
        Public Const CANCEL As String = "99"
    End Class

    ''' <summary>
    ''' 点検種類
    ''' </summary>
    ''' <remarks></remarks>
    Public Class InspectionType
        Public RESULT As String = String.Empty
        Public SUGGEST As String = String.Empty
    End Class

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
    ''' 全販売店を意味するワイルドカード販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllDealerCode As String = "XXXXX"

    ''' <summary>
    ''' 全店舗を意味するワイルドカード店舗コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllBranchCode As String = "XXX"

#End Region

#Region "公開メソッド"

    ''' <summary>
    ''' デフォルトのOldパーツ写真とNewパーツ写真のファイルパスを取得する
    ''' </summary>
    ''' <param name="strRO_NUM">R/O番号</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="ApproveFlg">顧客承認フラグ</param>
    ''' <param name="strSAChipID">SAChipID</param>
    ''' <param name="strKeepKey">現在保持しているキー</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDefaultPartsFileNameData(
                        ByVal strRO_NUM As String, _
                        ByVal strINSPEC_ITEM_CD As String, _
                        ByVal strDLR_CD As String, _
                        ByVal strBRN_CD As String, _
                        ByVal ApproveFlg As Boolean, _
                        ByVal strSAChipID As String, _
                        ByRef strKeepKey As String, _
                        ByVal isRoActive As Boolean) As SC3250104DataSet.DefaultPartsFileDataTable

        'メソッド名取得
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name
        Dim retDt As SC3250104DataSet.DefaultPartsFileDataTable

        'ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6} P5:{7} P6:{8} P7:{9}", _
                                Me.GetType.ToString, _
                                methodName, _
                                ConsLogStart, _
                                strRO_NUM, _
                                strINSPEC_ITEM_CD, _
                                strDLR_CD, _
                                strBRN_CD, _
                                ApproveFlg.ToString, _
                                strSAChipID, _
                                strKeepKey
                                ))

        Using ds As New SC3250104DataSet
            Using dt As SC3250104DataSet.DefaultPartsFileDataTable _
                    = ds.GetDefaultPartsFileName(strKeepKey, strINSPEC_ITEM_CD, strDLR_CD, strBRN_CD, isRoActive)
                retDt = dt
            End Using

            '「承認済」かつ、SAChipID検索にて取得できなかった場合RO情報をキーとして検索
            'パターンとして、SAChipIDとROが紐付くが承認後に新規に作られた場合にはRO情報がキーとなっている
            If ApproveFlg = True Then
                If strRO_NUM <> strKeepKey And retDt.Item(0).SEL_PICTURE_URL.Trim() = "" Then

                    Using dt2 As SC3250104DataSet.DefaultPartsFileDataTable _
                            = ds.GetDefaultPartsFileName(strRO_NUM, strINSPEC_ITEM_CD, strDLR_CD, strBRN_CD, isRoActive)
                        retDt = dt2
                    End Using
                    'RO情報にて取得した場合、キーはRO情報となる
                    strKeepKey = strRO_NUM

                End If
            End If
        End Using

        'ログ出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                "{0}.{1} {2}  QUERY:COUNT = {3}", _
                                Me.GetType.ToString, _
                                methodName, _
                                ConsLogEnd, _
                                retDt.Rows.Count
                                ))

        Return retDt
    End Function

    ''' <summary>
    ''' 写真選択画面で選択した写真のファイルパスをデータベースに登録する
    ''' </summary>
    ''' <param name="strRO_NUM">R/O番号</param>
    ''' <param name="strSEL_PICTURE_URL">写真URL</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <param name="ApproveFlg">顧客承認フラグ</param>
    ''' <param name="strKeepKey">現在保持しているキー</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function RegistSelectedPartsFileName(
                        ByVal strRO_NUM As String, _
                        ByVal strSEL_PICTURE_URL As String, _
                        ByVal strSTF_CD As String, _
                        ByVal strDLR_CD As String, _
                        ByVal strBRN_CD As String, _
                        ByVal strINSPEC_ITEM_CD As String, _
                        ByVal ApproveFlg As Boolean, _
                        ByRef strKeepKey As String
                                            ) As Boolean

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}, P1:[{3}], P2:[{4}], P3:[{5}], P4:[{6}], P5:[{7}], P6:[{8}], P7:[{9}], P8:[{10}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strRO_NUM _
                  , strSEL_PICTURE_URL _
                  , strSTF_CD _
                  , strDLR_CD _
                  , strBRN_CD _
                  , strINSPEC_ITEM_CD _
                  , ApproveFlg.ToString _
                  , strKeepKey))

        Dim returnCode As Boolean = True

        Using ds As New SC3250104DataSet
            '既に登録されているか確認
            Dim dt As New SC3250104DataSet.SelectedPartsFileDataTable
            dt = ds.GetSelectedPartsFileName(strKeepKey, strDLR_CD, strBRN_CD, strINSPEC_ITEM_CD)

            Dim ret As Integer = 0
            If 0 < dt.Count Then
                '更新作業
                ret = ds.UpdatePartsFileName(strKeepKey _
                                             , strSEL_PICTURE_URL _
                                             , strSTF_CD _
                                             , strDLR_CD _
                                             , strBRN_CD _
                                             , strINSPEC_ITEM_CD)
            Else
                '承認後の新規登録は RO情報 をキーとして行う
                If ApproveFlg = true Then
                    strKeepKey = strRO_NUM
                End If

                '新規登録
                ret = ds.InsertPartsFileName(strKeepKey _
                                             , strSEL_PICTURE_URL _
                                             , strSTF_CD _
                                             , strDLR_CD _
                                             , strBRN_CD _
                                             , strINSPEC_ITEM_CD)
            End If

            '登録・更新処理に失敗していたらロールバック
            If ret = 0 Or ret = 99 Then
                Me.Rollback = True
                returnCode = False
            End If

        End Using

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}, Retrun:[{3}]" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , returnCode))

        Return returnCode
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

        '自分のテーブルアダプタークラスインスタンスを生成
        Using ta As New SC3250104DataSet

            '販売店システム設定から取得
            Dim dt As SC3250104DataSet.SystemSettingDataTable _
                                    = ta.GetDlrSystemSettingValue(userContext.DlrCD, _
                                                                              userContext.BrnCD, _
                                                                              AllDealerCode, _
                                                                              AllBranchCode, _
                                                                              settingName)

            If 0 < dt.Count Then

                '設定値を取得
                retValue = dt.Item(0).SETTING_VAL

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}_S OUT:{1}={2}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  settingName, _
                                  retValue))

        Return retValue

    End Function

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
                                      Optional account As String = "") As SC3250104DataSet.DmsCodeMapDataTable

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
        Dim dt As SC3250104DataSet.DmsCodeMapDataTable

        Using ta As New SC3250104DataSet
            dt = ta.GetIcropToDmsCode(AllDealerCode, _
                                      dealerCD, _
                                      dmsCodeType, _
                                      icropCD1, _
                                      icropCD2, _
                                      icropCD3)
        End Using

        If dt.Count <= 0 Then

            'データが取得できない場合
            Logger.Warn(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}.{1} WARN：No data found. ", _
                                       Me.GetType.ToString, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))

        End If

        'アカウント情報と取得項目のチェック
        If Not (String.IsNullOrEmpty(account)) AndAlso _
           (dmsCodeType = SC3250104BusinessLogic.DmsCodeType.DealerCode OrElse _
           dmsCodeType = SC3250104BusinessLogic.DmsCodeType.BranchCode OrElse _
           dmsCodeType = SC3250104BusinessLogic.DmsCodeType.StallId) Then
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
    ''' 写真選択画面で選択した写真のファイルパスをデータベースから削除する
    ''' </summary>
    ''' <param name="strKeepKey">現在保持しているキー</param>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks>2014/09/30　写真選択画面でキャンセルボタンタップ時に写真をデフォルトに戻す仕様追加</remarks>
    <EnableCommit()>
    Public Function DeleteSelectedPartsFileName(
                                        ByVal strKeepKey As String, _
                                        ByVal strDLR_CD As String, _
                                        ByVal strBRN_CD As String, _
                                        ByVal strINSPEC_ITEM_CD As String
                                                ) As Boolean

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}, P1:[{3}] P2:[{4}] P3:[{5}] P4:[{6}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strKeepKey _
                  , strDLR_CD _
                  , strBRN_CD _
                  , strINSPEC_ITEM_CD))

        Dim returnCode As Boolean = True

        Using ds As New SC3250104DataSet
            '既に登録されているか確認
            Dim dt As New SC3250104DataSet.SelectedPartsFileDataTable
            dt = ds.GetSelectedPartsFileName(strKeepKey, strDLR_CD, strBRN_CD, strINSPEC_ITEM_CD)

            If 0 < dt.Count Then
                Dim ret As Integer = 0

                '削除作業
                ret = ds.DeletePartsFileName(strKeepKey, strDLR_CD, strBRN_CD, strINSPEC_ITEM_CD)

                '削除処理に失敗していたらロールバック
                If ret = 0 Or ret = 99 Then
                    Me.Rollback = True
                    returnCode = False
                End If

            End If
        End Using

        '終了ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2}, Retrun:[{3}]" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , returnCode))

        Return returnCode
    End Function

    ''' <summary>
    ''' RO情報に紐付くSAChipIDを取得する。存在しない場合RO番号を返却する 
    ''' </summary>
    ''' <param name="strRO_NUM">R/O番号</param>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <returns></returns>
    Public Function GetSAChipID(
                        ByVal strRO_NUM As String, _
                        ByVal strDLR_CD As String, _
                        ByVal strBRN_CD As String, _
                        ByVal isRoActive As Boolean
                                 ) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
          , "{0}.{1} {2}, P1:[{3}] P2:[{4}] P3:[{5}]" _
          , Me.GetType.ToString _
          , System.Reflection.MethodBase.GetCurrentMethod.Name _
          , LOG_START _
          , strRO_NUM _
          , strDLR_CD _
          , strBRN_CD))

        Using ds As New SC3250104DataSet

            Dim dt As New SC3250104DataSet.SAchipIDExistCheckDataTable
            '判定処理実行
            dt = ds.ExistSAChipID(strRO_NUM, strDLR_CD, strBRN_CD, isRoActive)
            'VISIT_IDが取得できなかった場合は、紐付くSAChipIDが存在しないことになる
            If dt.Item(0).VISIT_ID = "0" Then

                '終了ログの記録
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} {2}, Retrun:[{3}]" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , LOG_END _
                           , strRO_NUM))

                '紐付くSAChipIDが存在しないため、RO情報をキーとする
                Return strRO_NUM
            Else

                '終了ログの記録
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} {2}, Retrun:[{3}]" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , LOG_END _
                           , dt.Item(0).VISIT_ID))

                '紐付くSAChipIDが存在するため、SAChipIDをキーとする
                Return dt.Item(0).VISIT_ID
            End If
        End Using

    End Function

    '2017/XX/XX ライフサイクル対応　↓
    ''' <summary>
    ''' RO Active存在チェック ActiveにROが存在する場合に真を帰す
    ''' </summary>
    ''' <param name="argDlrCd">販売店コード</param>
    ''' <param name="argBrnCd">店舗コード</param>
    ''' <param name="argRoNum">RO番号</param>
    ''' <returns>True:Activeに存在する False:Activeに存在しない</returns>
    ''' <remarks></remarks>
    Public Function ChkExistParamRoActive(ByVal argDlrCd As String, _
                                           ByVal argBrnCd As String, _
                                           ByVal argRoNum As String) As Boolean

        Dim isExistActive As Boolean = False

        'If String.IsNullOrEmpty(argRoNum) OrElse String.IsNullorWhiteSpace(argRoNum) Then
        '   isExistActive = True
        '   Return isExistActive
        'End If

        Using dcSC3250104 As New SC3250104DataSet

            isExistActive = dcSC3250104.ChkExistParamRoActive(argDlrCd, argBrnCd, argRoNum)

        End Using

        Return isExistActive

    End Function
    '2017/XX/XX ライフサイクル対応　↑
#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class

