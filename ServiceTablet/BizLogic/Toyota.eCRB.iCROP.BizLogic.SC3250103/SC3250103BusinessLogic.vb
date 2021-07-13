'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250103BusinessLogic.vb
'─────────────────────────────────────
'機能： 部品説明画面 ビジネスロジック
'補足： 
'作成： 2014/08/XX NEC 上野
'更新： 
'更新： 
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports Toyota.eCRB.iCROP.DataAccess.SC3250103
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization
Imports System.Data

''' <summary>
''' 部品説明画面のビジネスロジック
''' </summary>
''' <remarks>部品説明のビジネスロジッククラス</remarks>
Public Class SC3250103BusinessLogic

    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "PublicConst"

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
    ''' コンテンツ表示設定に必要な情報を取得
    ''' </summary>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetInspecItemInfoData(ByVal strINSPEC_ITEM_CD As String) As SC3250103DataSet.InspecItemInfoDataTable

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}, strPART_NAME:[{3}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , strINSPEC_ITEM_CD))


        Using dsSC3250103 As New SC3250103DataSet

            '指定した点検項目コードの情報を取得
            Dim dtSC3250103 As New SC3250103DataSet.InspecItemInfoDataTable

            dtSC3250103 = dsSC3250103.GetInspecItemInfo(strINSPEC_ITEM_CD)

            '終了ログの記録
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2}, Retrun_Count:[{3}]" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dtSC3250103.Rows.Count))

            Return dtSC3250103

        End Using

    End Function

    ''' <summary>
    ''' URL作成処理
    ''' </summary>
    ''' <param name="inDisplayNumber">表示番号</param>
    ''' <param name="inParameterList">置換データリスト</param>
    ''' <returns>URL</returns>
    ''' <remarks></remarks>
    Public Function CreateURL(ByVal inDisplayNumber As Long, _
                               ByVal inParameterList As List(Of String), _
                               ByVal inDomain As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '戻り値宣言
        Dim returnURL As String = String.Empty

        Try
            'URL取得
            Dim dtDisplayRelation As SC3250103DataSet.DisplayRelationDataTable = Me.GetDisplayUrl(inDisplayNumber)

            'URL取得確認
            If 0 < dtDisplayRelation.Count Then
                '取得できた場合
                '戻り値に設定
                returnURL = dtDisplayRelation(0).DMS_DISP_URL

                'ドメイン名を置換する
                returnURL = returnURL.Replace("{0}", inDomain)

                'パラメーターを置換する
                Dim replaceType As Boolean = True
                Dim replacecount As Integer = 1
                While replaceType
                    '置換対象の文字列作成
                    Dim replaceWord As String = String.Concat("{", (replacecount).ToString(CultureInfo.CurrentCulture), "}")

                    '置換対象する文字列の存在チェック
                    If 0 <= returnURL.IndexOf(replaceWord) Then
                        '存在する場合
                        '置換するデータの確認
                        If replacecount <= inParameterList.Count Then
                            '存在する場合
                            '対象データに置換する
                            returnURL = returnURL.Replace(replaceWord, inParameterList(replacecount - 1))

                        Else
                            '存在しない場合
                            '空文字列に置換する
                            returnURL = returnURL.Replace(replaceWord, String.Empty)

                        End If
                    Else
                        '存在しない場合
                        'ループ終了
                        replaceType = False

                    End If

                    replacecount += 1
                End While

            Else
                '取得できなかった場合
                'ログ出力
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is nothing" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))

            End If
            dtDisplayRelation.Dispose()

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'ORACLEのタイムアウト処理
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} DB TIMEOUT:{2}" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                     , ex.Message))
        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END URL:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnURL))

        Return returnURL
    End Function

    ''' <summary>
    ''' 基幹コードへ変換処理
    ''' 販売店コード・店舗コード・アカウントをそれぞれ
    ''' 基幹販売店コード・基幹店舗コード・基幹アカウントに変換
    ''' </summary>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <remarks>基幹コード情報ROW</remarks>
    Public Function ChangeDmsCode(ByVal inStaffInfo As StaffContext) _
                                  As SC3250103DataSet.DmsCodeMapRow

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2} IN:DLRCD = {3} STRCD = {4} ACCOUNT = {5} " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inStaffInfo.DlrCD, inStaffInfo.BrnCD, inStaffInfo.Account))

        '基幹コードへ変換処理
        Dim dtDmsCodeMap As SC3250103DataSet.DmsCodeMapDataTable = _
            GetIcropToDmsCode(inStaffInfo.DlrCD, _
                                  SC3250103BusinessLogic.DmsCodeType.BranchCode, _
                                  inStaffInfo.DlrCD, _
                                  inStaffInfo.BrnCD, _
                                  String.Empty, _
                                  inStaffInfo.Account)

        '基幹コード情報Row
        Dim rowDmsCodeMap As SC3250103DataSet.DmsCodeMapRow

        '基幹コードへ変換処理結果チェック
        If dtDmsCodeMap IsNot Nothing AndAlso 0 < dtDmsCodeMap.Rows.Count Then
            '基幹コードへ変換処理成功

            'Rowに変換
            rowDmsCodeMap = DirectCast(dtDmsCodeMap.Rows(0), SC3250103DataSet.DmsCodeMapRow)

            '基幹アカウントチェック
            If rowDmsCodeMap.IsACCOUNTNull Then
                '値無し

                '空文字を設定する
                '基幹アカウント
                rowDmsCodeMap.ACCOUNT = String.Empty

            End If

            '基幹販売店コードチェック
            If rowDmsCodeMap.IsCODE1Null Then
                '値無し

                '空文字を設定する
                '基幹販売店コード
                rowDmsCodeMap.CODE1 = String.Empty

            End If

            '基幹店舗コードチェック
            If rowDmsCodeMap.IsCODE2Null Then
                '値無し

                '空文字を設定する
                '基幹店舗コード
                rowDmsCodeMap.CODE2 = String.Empty

            End If

        Else
            '基幹コードへ変換処理成功失敗

            '新しいRowを作成
            rowDmsCodeMap = DirectCast(dtDmsCodeMap.NewDmsCodeMapRow, SC3250103DataSet.DmsCodeMapRow)

            '空文字を設定する
            '基幹アカウント
            rowDmsCodeMap.ACCOUNT = String.Empty
            '基幹販売店コード
            rowDmsCodeMap.CODE1 = String.Empty
            '基幹店舗コード
            rowDmsCodeMap.CODE2 = String.Empty

        End If


        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                   , "{0}.{1} {2} dtDmsCodeMap:COUNT = {3}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , LOG_END _
                   , dtDmsCodeMap.Count))

        '結果返却
        Return rowDmsCodeMap

    End Function

    ' ''' <summary>
    ' ''' グラフ作成用グループコード存在チェック用
    ' ''' </summary>
    ' ''' <param name="inspecItemcd">点検項目コード</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function ChkPartsGroupCd(
    '                        ByVal inspecItemCd As String
    '                                        ) As Boolean
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START P1{2}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , inspecItemCd))

    '    '戻り値宣言
    '    Dim judge As Boolean = False

    '    Try
    '        'グループコードチェック
    '        Using dsSC3250103 As New SC3250103DataSet
    '            Using dt As SC3250103DataSet.PartsGroupCdDataTable = _
    '                dsSC3250103.ChkPartsGroupCd(inspecItemCd)
    '                If 0 < dt.Count Then
    '                    '取得できた場合
    '                    'グループコードが設定されていたらTrueをセット
    '                    If dt(0).PARTS_GROUP_CD.TrimEnd.Length > 0 Then
    '                        judge = True
    '                    End If
    '                End If
    '            End Using
    '        End Using

    '    Catch ex As OracleExceptionEx When ex.Number = 1013
    '        'ORACLEのタイムアウト処理
    '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                                 , "{0}.{1} DB TIMEOUT:{2}" _
    '                                 , Me.GetType.ToString _
    '                                 , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                 , ex.Message))
    '    End Try

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END Return:{2}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , judge.ToString))

    '    Return judge
    'End Function

    ''' <summary>
    ''' グラフコンテンツエリア表示チェック
    ''' </summary>
    ''' <param name="inspecItemcd">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ChkDisplayGraphArea(
                            ByVal dealerCd As String _
                            , ByVal branchCd As String _
                            , ByVal vclVin As String _
                            , ByVal inspecItemCd As String
                                            ) As Boolean
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , dealerCd _
                    , branchCd _
                    , vclVin _
                    , inspecItemCd))

        '戻り値宣言
        Dim judge As Boolean = False

        Using dsSC3250103 As New SC3250103DataSet
            'グループコード取得
            Dim partsGroupCd As String = GetPartsGroupCd(inspecItemCd)

            'グループコードが取得できたらグラフ表示データカウント取得
            If 0 < partsGroupCd.Length Then
                Dim Count As Integer = GetUpsellChartCount(vclVin, partsGroupCd, dealerCd, branchCd)

                'グラフ表示データを取得できたらTrueをセット
                If 0 < Count Then
                    judge = True
                End If

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END Return:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , judge.ToString))

        Return judge
    End Function

#End Region

#Region "内部処理"

    ''' <summary>
    ''' 画面URL情報取得
    ''' </summary>
    ''' <param name="inDisplayNumber">表示番号</param>
    ''' <returns>URL情報</returns>
    ''' <remarks></remarks>
    Private Function GetDisplayUrl(ByVal inDisplayNumber As Long) As SC3250103DataSet.DisplayRelationDataTable

        '開始ログの記録
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} {2}, inDisplayNumber:[{3}] " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , LOG_START _
                  , inDisplayNumber))

        '画面URL情報取得
        Using dsSC3250103 As New SC3250103DataSet

            Dim dt As SC3250103DataSet.DisplayRelationDataTable = _
                dsSC3250103.TB_M_DISP_RELATION_Select(inDisplayNumber)

            '終了ログの記録
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} {2}, Retrun_Count:[{3}]" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , LOG_END _
                       , dt.Rows.Count))


            Return dt

        End Using

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
    Private Function GetIcropToDmsCode(ByVal dealerCD As String, _
                                      ByVal dmsCodeType As DmsCodeType, _
                                      ByVal icropCD1 As String, _
                                      ByVal icropCD2 As String, _
                                      ByVal icropCD3 As String, _
                                      Optional account As String = "") As SC3250103DataSet.DmsCodeMapDataTable

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
        Dim dt As SC3250103DataSet.DmsCodeMapDataTable

        Using ta As New SC3250103DataSet
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
        Else
            'アカウント情報と取得項目のチェック
            If Not (String.IsNullOrEmpty(account)) AndAlso _
               (dmsCodeType = SC3250103BusinessLogic.DmsCodeType.DealerCode OrElse _
               dmsCodeType = SC3250103BusinessLogic.DmsCodeType.BranchCode OrElse _
               dmsCodeType = SC3250103BusinessLogic.DmsCodeType.StallId) Then
                'アカウントが存在する場合且つ、販売店・店舗・ストールの情報を取得する場合
                '変換したアカウントを格納
                dt(0).ACCOUNT = account.Split(CChar("@"))(0)
            End If
        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} QUERY:COUNT = {2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dt.Count))

        Return dt

    End Function

    ''' <summary>
    ''' グラフ作成用グループコード取得
    ''' </summary>
    ''' <param name="inspecItemcd">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPartsGroupCd(
                            ByVal inspecItemCd As String
                                            ) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START P1:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inspecItemCd))

        '戻り値宣言
        Dim groupCd As String = String.Empty

        Try
            'グループコード取得
            Using dsSC3250103 As New SC3250103DataSet
                Using dt As SC3250103DataSet.PartsGroupCdDataTable = _
                    dsSC3250103.ChkPartsGroupCd(inspecItemCd)
                    If 0 < dt.Count Then
                        '取得できた場合
                        'グループコードをセット
                        If dt(0).PARTS_GROUP_CD.TrimEnd.Length > 0 Then
                            groupCd = dt(0).PARTS_GROUP_CD.TrimEnd
                        End If
                    End If
                End Using
            End Using

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'ORACLEのタイムアウト処理
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} DB TIMEOUT:{2}" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                     , ex.Message))
        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END Return:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , groupCd))

        Return groupCd
    End Function

    ''' <summary>
    ''' グラフ作成用データカウント取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetUpsellChartCount(
                            ByVal vclVin As String _
                            , ByVal partsGroupCd As String _
                            , ByVal dealerCd As String _
                            , ByVal branchCd As String _
                                            ) As Integer
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , vclVin _
                    , partsGroupCd _
                    , dealerCd _
                    , branchCd))

        '戻り値宣言
        Dim returnCnt As Integer

        Try
            'グラフ表示データカウント取得
            Using dsSC3250103 As New SC3250103DataSet
                returnCnt = dsSC3250103.GetUpsellChartDataCount(vclVin, partsGroupCd, dealerCd, branchCd)
            End Using

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'ORACLEのタイムアウト処理
            Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} DB TIMEOUT:{2}" _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                     , ex.Message))
        End Try

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END Return:{2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnCnt))

        Return returnCnt
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
