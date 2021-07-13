'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250105BusinessLogic.vb
'─────────────────────────────────────
'機能： 部品説明画面（部品交換情報） ビジネスロジック
'補足： 
'作成： 2014/08/XX NEC 上野
'更新： 
'更新： 
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports Toyota.eCRB.iCROP.DataAccess.SC3250105
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization
Imports System.Data

''' <summary>
''' 部品説明画面のビジネスロジック
''' </summary>
''' <remarks>部品説明のビジネスロジッククラス</remarks>
Public Class SC3250105BusinessLogic

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

    ' ''' <summary>
    ' ''' 前回部品交換日時及び走行距離取得処理
    ' ''' </summary>
    ' ''' <param name="dealerCode">販売店コード</param>
    ' ''' <param name="branchCode">店舗コード</param>
    ' ''' <param name="vclVin">VIN</param>
    ' ''' <param name="inspecItemcd">点検項目コード</param>
    ' ''' <param name="roNum">RO番号(任意)</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetPreviosReplacementData(
    '                    ByVal dealerCode As String,
    '                    ByVal branchCode As String, _
    '                    ByVal roNum As String, _
    '                    ByVal vclVin As String, _
    '                    ByVal inspecItemCd As String
    '                                        ) As SC3250105DataSet.PreviosReplacementDataTable

    '    'メソッド名取得
    '    Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

    '    'ログ出力
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
    '                            "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6} P5:{7}", _
    '                            Me.GetType.ToString, _
    '                            methodName, _
    '                            ConsLogStart, _
    '                            dealerCode, _
    '                            branchCode, _
    '                            roNum, _
    '                            vclVin, _
    '                            inspecItemCd
    '                            ))

    '    Using dt As SC3250105DataSet.PreviosReplacementDataTable _
    '            = SC3250105DataSet.GetPreviosReplacement(dealerCode, branchCode, roNum, vclVin, inspecItemCd)

    '        'ログ出力
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
    '                                "{0}.{1} {2}  QUERY:COUNT = {3}", _
    '                                Me.GetType.ToString, _
    '                                methodName, _
    '                                ConsLogEnd, _
    '                                dt.Rows.Count
    '                                ))

    '        Return dt
    '    End Using

    'End Function

    ''' <summary>
    ''' 前回部品交換日時及び走行距離取得処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="vclVin">VIN</param>
    ''' <param name="inspecItemcd">点検項目コード</param>
    ''' <param name="strNewCar">新車で納車直後に表示する走行距離内容</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPreviosReplacementData(
                        ByVal dealerCode As String,
                        ByVal roNum As String, _
                        ByVal vclVin As String, _
                        ByVal inspecItemCd As String, _
                        ByVal strNewCar As String _
                                            ) As SC3250105DataSet.PreviosReplacementDataTable


        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} P1:{3} P2:{4} P3:{5} P4:{6} P5:{7}" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , dealerCode _
                  , roNum _
                  , vclVin _
                  , inspecItemCd _
                  , strNewCar))


        Using ds As New SC3250105DataSet

            '戻り値用データテーブル作成
            Dim retDataTable As New SC3250105DataSet.PreviosReplacementDataTable
            Dim retRow As SC3250105DataSet.PreviosReplacementRow = DirectCast(retDataTable.NewRow(), SC3250105DataSet.PreviosReplacementRow)


            '①前回交換時の日時を取得
            Dim dt As New SC3250105DataSet.PreviosReplacementDateTimeDataTable
            dt = ds.GetPreviosReplacementDatetime(vclVin, inspecItemCd, roNum)

            If dt IsNot Nothing AndAlso 0 < dt.Count Then
                '*****交換履歴が取得できた場合
                Logger.Info("GetPreviosReplacementDatetime")
                '②入庫履歴より走行距離を取得

                '戻り値用データテーブルに前回交換時の日時を挿入
                If Not dt(0).IsREPLACED_DATETIMENull Then
                    retRow.REPLACED_DATETIME = dt(0).REPLACED_DATETIME
                    Logger.Info(String.Format("REPLACED_DATETIME:[{0}]", retRow.REPLACED_DATETIME))
                End If

                'DLR_CDの空白チェック
                If dt(0).IsDLR_CDNull OrElse String.IsNullOrWhiteSpace(dt(0).DLR_CD) Then
                    '販売店コードが空白
                    retDataTable.Rows.Add(retRow)
                    Return retDataTable
                End If

                'BRN_CDの空白チェック
                If dt(0).IsBRN_CDNull OrElse String.IsNullOrWhiteSpace(dt(0).BRN_CD) Then
                    '販売店コードが空白
                    retDataTable.Rows.Add(retRow)
                    Return retDataTable
                End If

                'RO_NUMの空白チェック
                If dt(0).IsRO_NUMNull OrElse String.IsNullOrWhiteSpace(dt(0).RO_NUM) Then
                    '販売店コードが空白
                    retDataTable.Rows.Add(retRow)
                    Return retDataTable
                End If

                'DMS変換処理
                Dim DmsDataTable As SC3250105DataSet.DmsCodeMapDataTable = GetIcropToDmsCode(dealerCode, _
                                     SC3250105BusinessLogic.DmsCodeType.BranchCode, _
                                     dt(0).DLR_CD, _
                                     dt(0).BRN_CD, _
                                     String.Empty, _
                                     String.Empty)

                Dim DMS_BRN_CD As String
                If 0 < DmsDataTable.Count Then
                    If Not DmsDataTable(0).IsCODE2Null AndAlso Not String.IsNullOrWhiteSpace(DmsDataTable(0).CODE2) Then
                        'DMS変換後の店舗コードを取得
                        DMS_BRN_CD = DmsDataTable(0).CODE2
                    Else
                        'DMS変換失敗時は変換前の店舗コードを入れる
                        DMS_BRN_CD = dt(0).BRN_CD
                    End If
                Else
                    'DMS変換取得失敗時は変換前の店舗コードを入れる
                    DMS_BRN_CD = dt(0).BRN_CD
                End If

                '入庫管理番号作成
                Dim strSVCIN_NUM As String = GetSVCIN_NUM(DMS_BRN_CD, dt(0).RO_NUM)

                '入庫履歴より走行距離を取得
                Dim dt2 As SC3250105DataSet.PreviosReplacementMileageDataTable
                dt2 = ds.GetPreviosReplacementMileage(dt(0).DLR_CD, strSVCIN_NUM)

                If dt2 IsNot Nothing AndAlso 0 < dt2.Count Then
                    '戻り値用データテーブルに走行距離を挿入
                    If dt2(0).REG_MILE = 0 Then
                        retRow.REG_MILE = "0km"
                        Logger.Info(String.Format("REG_MILE:[{0}]", retRow.REG_MILE))
                    Else
                        retRow.REG_MILE = Format(dt2(0).REG_MILE, "#,#km")
                        Logger.Info(String.Format("REG_MILE:[{0}]", retRow.REG_MILE))
                    End If
                End If

            Else
                '*****交換履歴が無い場合
                Logger.Info("GetVehicleDeliveryDate")
                '③納車日と車両区分を取得する
                Dim dtVehicleDelivery As SC3250105DataSet.VehicleDeliveryDataTable
                dtVehicleDelivery = ds.GetVehicleDeliveryDate(vclVin)

                If dtVehicleDelivery IsNot Nothing AndAlso 0 < dtVehicleDelivery.Count Then
                    '取得成功
                    If Not dtVehicleDelivery(0).IsDELI_DATENull Then
                        '戻り値用データテーブルに納車日を挿入
                        retRow.REPLACED_DATETIME = dtVehicleDelivery(0).DELI_DATE
                        Logger.Info(String.Format("REPLACED_DATETIME:[{0}]", retRow.REPLACED_DATETIME))
                    End If

                    '車両区分で新車「0」ならば戻り値用データテーブルに「NewCarDelivery」を挿入
                    If Not String.IsNullOrWhiteSpace(dtVehicleDelivery(0).VCL_TYPE) Then
                        Logger.Info(String.Format("VCL_TYPE:[{0}]", dtVehicleDelivery(0).VCL_TYPE))
                        If dtVehicleDelivery(0).VCL_TYPE = "0" Then
                            retRow.REG_MILE = strNewCar
                            Logger.Info(String.Format("REG_MILE:[{0}]", retRow.REG_MILE))
                        End If
                    End If
                End If

            End If

            '戻り値用データテーブルに追加
            retDataTable.Rows.Add(retRow)

            '終了ログの記録
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2}, Return_Count:[{3}]" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , retDataTable.Count))

            Return retDataTable

        End Using

    End Function

    ''' <summary>
    ''' 入庫管理番号取得
    ''' </summary>
    ''' <returns>入庫管理番号</returns>
    ''' <remarks>入庫管理番号の書式変換を行う</remarks>
    Public Function GetSVCIN_NUM(ByVal strBRN_CD As String _
                                 , ByVal strRO_NUM As String _
                                 ) As String

        '開始ログの記録
        Logger.Info(String.Format("GetSVCIN_NUM_START, strBRN_CD:[{0}]", strBRN_CD))

        '①「販売店システム設定」より、「入庫管理番号利用フラグ」を取得する。
        Dim SVCIN_FLG As String = Me.GetDlrSystemSettingValueBySettingName("SVCIN_NUM_USE_FLG")

        '②「入庫管理番号利用フラグ」が０の場合、書式変換を行う
        Dim SVCIN_Num As String = String.Empty
        If Not String.IsNullOrWhiteSpace(SVCIN_FLG) Then
            If SVCIN_FLG = "0" Then
                Dim SVCIN_Format As String = Me.GetDlrSystemSettingValueBySettingName("SVCIN_NUM_FORMAT")
                If Not String.IsNullOrWhiteSpace(SVCIN_Format) Then
                    SVCIN_Num = Replace(Replace(SVCIN_Format, "[RO_NUM]", strRO_NUM), "[DMS_BRN_CD]", strBRN_CD)
                End If
            End If
        End If

        '終了ログの記録
        Logger.Info(String.Format("GetSVCIN_NUM_END, Return:[{0}]", SVCIN_Num))

        Return SVCIN_Num

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
        Using ta As New SC3250105DataSet

            '販売店システム設定から取得
            Dim dt As SC3250105DataSet.SystemSettingDataTable _
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
                                      Optional account As String = "") As SC3250105DataSet.DmsCodeMapDataTable

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
        Dim dt As SC3250105DataSet.DmsCodeMapDataTable

        Using ta As New SC3250105DataSet
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
           (dmsCodeType = SC3250105BusinessLogic.DmsCodeType.DealerCode OrElse _
           dmsCodeType = SC3250105BusinessLogic.DmsCodeType.BranchCode OrElse _
           dmsCodeType = SC3250105BusinessLogic.DmsCodeType.StallId) Then
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
