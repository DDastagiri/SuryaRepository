'-------------------------------------------------------------------------
'SC3180203BusinessLogic.vb
'-------------------------------------------------------------------------
'機能：チェックシートプレビュー(ビジネスロジック)
'補足：
'作成：2014/02/01 工藤
'更新：2019/12/10 NCN 吉川（FS）次世代サービス業務における車両型式別点検の検証
'─────────────────────────────────────

Option Explicit On
Imports System.Text
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.ServerCheck.CheckResult.DataAccess
Imports Toyota.eCRB.ServerCheck.CheckResult.DataAccess.SC3180203.SC3180203DataSet
Imports Toyota.eCRB.ServerCheck.CheckResult.DataAccess.SC3180203.SC3180203DataSetTableAdapter
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
''' <summary>
''' チェックシートプレビュービジネスクラス
''' </summary>
''' <remarks></remarks>
Public Class SC3180203BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"
    ''' <summary>
    ''' 全販売店を意味するワイルドカード販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AllDealerCode As String = "XXXXX"

    ''' <summary>
    ''' 販売店システム設定 設定名：メーカー区分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SettingNameMakerType As String = "MAKER_TYPE"

    ''' <summary>
    ''' メーカー区分（レクサス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MakerKbnLexus As String = "2"

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

    ''' <summary>
    ''' ヘッダー情報取得
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="brnCd">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="isExistActive">Active存在フラグ</param>
    ''' <returns>ヘッダー情報</returns>
    ''' <remarks></remarks>
    Public Function GetHeaderData(ByVal dlrCd As String, _
                                  ByVal brnCd As String, _
                                  ByVal roNum As String, _
                                  ByVal isExistActive As Boolean) As SC3180203HeaderDataDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim headerDataDataTable As New SC3180203HeaderDataDataTable

        '検索処理
        headerDataDataTable = SC3180203TableAdapter.GetCheckSheetHeader(dlrCd, brnCd, roNum, isExistActive)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return headerDataDataTable

    End Function

    ''' <summary>
    ''' 明細情報取得
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="brnCd">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <returns>明細情報</returns>
    ''' <remarks></remarks>
    Public Function GetDetailData(ByVal dlrCd As String, _
                                  ByVal brnCd As String, _
                                  ByVal roNum As String, _
                                  ByVal isExistActive As Boolean) As SC3180203DetailDataDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim detailDataDataTable As New SC3180203DetailDataDataTable

        '検索処理
        detailDataDataTable = SC3180203TableAdapter.GetCheckSheetDetail(dlrCd, brnCd, roNum, isExistActive)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return detailDataDataTable

    End Function

    ''' <summary>
    ''' 車種別のテンプレート取り込む
    ''' </summary>
    ''' <param name="filePath">ファイルパス</param>
    ''' <returns>車種別のテンプレート</returns>
    ''' <remarks></remarks>
    Public Function GetTemplateFile(ByVal filePath As String) As String
        Dim template As String = String.Empty

        If System.IO.File.Exists(filePath) Then
            Dim reader As New System.IO.StreamReader(filePath, System.Text.Encoding.UTF8)

            While (reader.Peek() >= 0)
                Dim stBuffer As String = reader.ReadLine()
                template &= stBuffer & System.Environment.NewLine
            End While

            reader.Close()
        End If

        Return template
    End Function

    ''' <summary>
    ''' モデルコード取得
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="brnCd">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="isExistActive">Active存在フラグ</param>
    ''' <returns>モデルコード</returns>
    ''' <remarks></remarks>
    Public Function GetModelCode(ByVal dlrCd As String, _
                                 ByVal brnCd As String, _
                                 ByVal roNum As String, _
                                 ByVal isExistActive As Boolean) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索処理
        Dim modelCode As String = String.Empty
        modelCode = SC3180203TableAdapter.GetModelCode(dlrCd, brnCd, roNum, isExistActive)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return modelCode

    End Function

    '2019/07/05　TKM要件:型式対応　START　↓↓↓
    ''' <summary>
    ''' 型式取得
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="brnCd">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="isExistActive">Active存在フラグ</param>
    ''' <returns>型式</returns>
    ''' <remarks></remarks>
    Public Function GetKatashiki(ByVal dlrCd As String, _
                                 ByVal brnCd As String, _
                                 ByVal roNum As String, _
                                 ByVal isExistActive As Boolean) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索処理
        Dim katashiki As String = String.Empty
        katashiki = SC3180203TableAdapter.GetKatashiki(dlrCd, brnCd, roNum, isExistActive)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return katashiki

    End Function
	'2019/07/05　TKM要件:型式対応　END　↑↑↑

    '2014/07/09 タイトルをデザイン固定にするため削除
    ' ''' <summary>
    ' ''' タイトル名取得
    ' ''' </summary>
    ' ''' <param name="dlrCd">販売店コード</param>
    ' ''' <param name="brnCd">店舗コード</param>
    ' ''' <returns>タイトル名</returns>
    ' ''' <remarks></remarks>
    'Public Function GetTitleName(ByVal dlrCd As String, _
    '                             ByVal brnCd As String) As DataTable

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    Dim titleDataTable As New DataTable

    '    '検索処理
    '    titleDataTable = SC3180203TableAdapter.GetTitleName(dlrCd, brnCd)

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} END" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    '処理結果返却
    '    Return titleDataTable

    'End Function

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
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

    '2014/06/27 不具合修正　Start
    ''' <summary>
    ''' アイテムコード並び順取得
    ''' </summary>
    ''' <param name="itemCode">アイテムコード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetItemCodeOrder(ByVal itemCode As String) As DataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim itemOrder As New DataTable

        '検索処理
        itemOrder = SC3180203TableAdapter.GetItemCodeOrder(itemCode)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return itemOrder

    End Function
    '2014/06/27 不具合修正　End

    '2014/07/28　DMS→ICROP変換処理追加　START　↓↓↓
    ''' <summary>
    ''' DMS販売店/店舗コード→ICROP販売店/店舗コード変換
    ''' </summary>
    ''' <param name="strDlrCd">基幹販売店コード</param>
    ''' <param name="strBrnCd">基幹店舗コード</param>
    ''' <remarks></remarks>
    Public Sub GetDmsToIcropCode(ByRef strDlrCd As String, ByRef strBrnCd As String)

        '開始ログの記録
        Logger.Error(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START, strDlrCd:[{2}], strBrnCd:[{3}]" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , strDlrCd _
            , strBrnCd))

        'DMS販売店/店舗コードをキーにICROP販売店/店舗コードを取得
        Dim dtSC3180203 As New IcropCodeMapDataTable
        Dim dsSC3180203 As New SC3180203TableAdapter

        dtSC3180203 = dsSC3180203.ChangeDlrStrCodeToICROP(AllDealerCode, DmsCodeType.BranchCode, strDlrCd, strBrnCd)

        '取得したデータチェック
        If dtSC3180203.Count <= 0 Then
            'データが取得できなかった場合

            'ログを記録
            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}.{1} Info：No data found. ", _
                                       Me.GetType.ToString, _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
        Else
            'データが取得できた場合

            '販売店コードチェック
            If String.IsNullOrWhiteSpace(dtSC3180203(0).ICROP_CD_1) Then
                '販売店コード取得できなかった
                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                           "{0}.{1} Info：ICROP DealerCode not found . ", _
                                           Me.GetType.ToString, _
                                           System.Reflection.MethodBase.GetCurrentMethod.Name))
            Else
                '販売店コード取得できた
                strDlrCd = dtSC3180203(0).ICROP_CD_1
            End If

            '店舗コードチェック
            If String.IsNullOrWhiteSpace(dtSC3180203(0).ICROP_CD_2) Then
                '店舗コード取得できなかった
                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                           "{0}.{1} Info：ICROP BranchCode not found . ", _
                                           Me.GetType.ToString, _
                                           System.Reflection.MethodBase.GetCurrentMethod.Name))
            Else
                '店舗コード取得できた
                strBrnCd = dtSC3180203(0).ICROP_CD_2
            End If

        End If

        '終了ログの記録
        Logger.Error(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END, Return: strDlrCd:[{2}], strBrnCd:[{3}]" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , strDlrCd _
            , strBrnCd))

    End Sub
    '2014/07/28　DMS→ICROP変換処理追加　END　　↑↑↑

    ''' <summary>
    ''' レクサス判定を行う。
    ''' </summary>
    ''' <returns>True:レクサス店 False:トヨタ店</returns>
    ''' <remarks></remarks>
    Public Function isLexus() As Boolean

        Logger.Error(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name
            ))

        Dim makerType As String = String.Empty
        Dim isLxs As Boolean = False

        Using biz As New ServiceCommonClassBusinessLogic

            makerType = biz.GetDlrSystemSettingValueBySettingName(SettingNameMakerType)

            If String.IsNullOrEmpty(makerType) Then
                isLxs = False

            Else
                If makerType.Trim().Equals(MakerKbnLexus) Then
                    isLxs = True
                Else
                    isLxs = False
                End If
            End If
        End Using

        Logger.Error(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END, Return: isLexus:[{2}]" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , isLxs.ToString
            ))

        Return isLxs
    End Function

    ''' <summary>
    ''' サービス入庫Active存在チェック
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="brnCd">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    ''' <returns>登録状態</returns>
    ''' <remarks></remarks>
    Public Function IsExistServiceinActive(ByVal dlrCd As String, _
                                           ByVal brnCd As String, _
                                           ByVal roNum As String) As Boolean

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '検索処理
        Dim isExistActive As Boolean
        Dim dsSC3180203 As New SC3180203TableAdapter
        isExistActive = dsSC3180203.IsExistServiceinActive(dlrCd, brnCd, roNum)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '処理結果返却
        Return isExistActive

    End Function

    ''' <summary>
    ''' マスタに車両型式が登録されているか判定する
    ''' </summary>
    ''' <param name="strRoNum">R/O番号</param>
    ''' <param name="strDlrCd">販売店コード</param>
    ''' <param name="strBrnCd">店舗コード</param>
    ''' <returns>登録状態</returns>
    ''' <remarks></remarks>
    Public Function GetKatashikiExist(ByVal strRoNum As String, ByVal strDlrCd As String, ByVal strBrnCd As String) As Boolean

        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim tableAdapter As New SC3180203TableAdapter
        Dim dt As DataTable = tableAdapter.GetKatashikiExistMst(strRoNum, strDlrCd, strBrnCd)
        Dim katashiki_exist As Boolean = False
        If dt.Rows.Count > 0 Then
            katashiki_exist = "1".Equals(dt(0)("KATASHIKI_EXIST").ToString())
        End If
        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} END [Result=KATASHIKI_EXIST:{2}]" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name _
                  , katashiki_exist))

        Return katashiki_exist
    End Function
End Class
