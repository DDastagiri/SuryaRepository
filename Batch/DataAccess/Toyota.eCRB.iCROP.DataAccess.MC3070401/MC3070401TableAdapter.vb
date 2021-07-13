'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'MC3070401TableAdapter.vb
'─────────────────────────────────────
'機能： オススメ情報集計バッチ データアクセス
'補足： 
'作成： 2012/02/24 TCS 鈴木(健)
'更新： 2013/06/30 TCS 武田 2013/10対応版　既存流用
'─────────────────────────────────────

Imports System.Globalization
Imports System.Reflection
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' オススメ情報集計バッチ
''' テーブルアダプタークラス
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class MC3070401TableAdapter

#Region "定数"
    ''' <summary>
    ''' 日付時刻のフォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Private Const prpFormatDatetime As String = "yyyy/MM/dd HH:mm:ss"

    ''' <summary>
    ''' 作成ユーザーアカウント
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CreateUserAccount As String = "SYSTEM"

    ''' <summary>
    ''' 機能ID：オススメ情報集計バッチ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const prpFunctionId As String = "MC3070401"

    ''' <summary>
    ''' 契約状況フラグ：契約済
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ContractFlgCompleted As String = "1"

    ''' <summary>
    ''' オプション区分：メーカー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OptionPartMaker As String = "1"

    ''' <summary>
    ''' オプション区分：販売店
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OptionPartDealer As String = "2"

    ''' <summary>
    ''' 削除フラグ：削除以外
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DelFlgValid As String = "0"
#End Region

#Region "プロパティ"
    ''' <summary>
    ''' 機能ID：オススメ情報集計バッチ
    ''' </summary>
    ''' <value></value>
    ''' <returns>機能ID：オススメ情報集計バッチ</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property FunctionId() As String
        Get
            Return prpFunctionId
        End Get
    End Property

    ''' <summary>
    ''' 日付時刻のフォーマット
    ''' </summary>
    ''' <value></value>
    ''' <returns>日付時刻のフォーマット</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property FormatDatetime() As String
        Get
            Return prpFormatDatetime
        End Get
    End Property
#End Region

#Region "コンストラクタ"
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub
#End Region

#Region "メソッド"
#Region "削除クエリ"
    ''' <summary>
    ''' 購入率集計情報をTRUNCATEします。
    ''' </summary>
    ''' <returns>処理結果（成功[0>=]/失敗[-1]）</returns>
    ''' <remarks></remarks>
    Public Shared Function TranRecommendedSummary() As Integer

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("TRUNCATE /* MC3070401_001 */ ")
            .Append("    TABLE ")
            .Append("    TBL_PURCHASERATE ")
        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3080401_001")

            query.CommandText = sql.ToString()

            ' SQL実行
            query.Execute()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, 0))
            ' ======================== ログ出力 終了 ========================

            ' 処理結果を返却
            Return 0
        End Using
    End Function
#End Region

#Region "挿入クエリ"
    ''' <summary>
    ''' 購入率集計情報を挿入します。
    ''' </summary>
    ''' <param name="startDate">集計対象開始日時</param>
    ''' <param name="endDate">集計対象終了日時</param>
    ''' <returns>処理結果（成功[0>=]/失敗[-1]）</returns>
    ''' <remarks></remarks>
    Public Shared Function InsRecommendedSummary(ByVal startDate As Date, ByVal endDate As Date) As Integer

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start, startDate:[{1}], endDate:[{2}]",
                                  MethodBase.GetCurrentMethod.Name,
                                  startDate.ToString(FormatDatetime, CultureInfo.InvariantCulture),
                                  endDate.ToString(FormatDatetime, CultureInfo.InvariantCulture)))
        ' ======================== ログ出力 終了 ========================

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("INSERT /* MC3070401_002 */ ")
            .Append("INTO ")
            .Append("    TBL_PURCHASERATE ")
            .Append("( ")
            .Append("    DLRCD ")                   '販売店コード
            .Append("  , SERIESCD ")                'シリーズコード
            .Append("  , MODELCD ")                 'モデルコード
            .Append("  , OPTIONPART ")              'オプション区分
            .Append("  , OPTIONCODE ")              'オプションコード
            .Append("  , CONTRACTQUANTITY ")        '契約台数
            .Append("  , CONTRACTCOUNT ")           '契約数
            .Append("  , RATE ")                    '購入率
            .Append("  , CREATEDATE ")              '作成日
            .Append("  , UPDATEDATE ")              '更新日
            .Append("  , CREATEACCOUNT ")           '作成ユーザアカウント
            .Append("  , UPDATEACCOUNT ")           '更新ユーザアカウント
            .Append("  , CREATEID ")                '作成機能ID
            .Append("  , UPDATEID ")                '更新機能ID
            .Append(") ")
            .Append("SELECT ")
            .Append("     VCL.DLRCD ")                                                              '契約車両集計.販売店コード
            .Append("   , VCL.SERIESCD ")                                                           '契約車両集計.シリーズコード
            .Append("   , VCL.MODELCD ")                                                            '契約車両集計.モデルコード
            .Append("   , OPT.OPTIONPART ")                                                         '契約車両オプション集計.オプション区分
            .Append("   , OPT.OPTIONCODE ")                                                         '契約車両オプション集計.オプションコード
            .Append("   , VCL.CONTRACTQUANTITY ")                                                   '契約車両集計.契約車両数
            .Append("   , OPT.CONTRACTCOUNT ")                                                      '契約車両オプション集計.契約車両オプション数
            .Append("   , ROUND(OPT.CONTRACTCOUNT / VCL.CONTRACTQUANTITY * 100, 2) AS RATE ")       '契約車両オプション数 ÷ 契約車両数 × 100
            .Append("   , SYSDATE AS CREATEDATE ")                                                  '作成日
            .Append("   , SYSDATE AS UPDATEDATE ")                                                  '更新日
            .Append("   , :CREATEACCOUNT AS CREATEACCOUNT ")                                        '作成ユーザアカウント
            .Append("   , :UPDATEACCOUNT AS UPDATEACCOUNT ")                                        '更新ユーザアカウント
            .Append("   , :CREATEID AS CREATEID ")                                                  '作成機能ID
            .Append("   , :UPDATEID AS UPDATEID ")                                                  '更新機能ID
            .Append("FROM ")
            ' ========== 契約車両集計用のサブクエリ 開始 ==========
            .Append("    ( ")
            .Append("    SELECT ")
            .Append("         EI.DLRCD ")                                   '見積情報.販売店コード
            .Append("       , EV.SERIESCD ")                                '見積車両情報.シリーズコード
            .Append("       , EV.MODELCD ")                                 '見積車両情報.モデルコード
            .Append("       , COUNT(1) AS CONTRACTQUANTITY ")               '販売店、モデルごとの契約車両数
            .Append("    FROM ")
            .Append("        TBL_ESTIMATEINFO EI ")
            .Append("      , TBL_EST_VCLINFO EV ")
            .Append("    WHERE ")
            .Append("        EI.ESTIMATEID = EV.ESTIMATEID ")               '見積情報.見積管理ID = 見積車両情報.見積管理ID
            .Append("    AND EI.CONTRACTFLG = :CONTRACTFLG ")               '見積情報.契約状況フラグ
            .Append("    AND EI.CONTRACTDATE >= :STARTDATE ")               '見積情報.契約完了日 集計開始日時
            .Append("    AND EI.CONTRACTDATE < :ENDDATE ")                  '見積情報.契約完了日 集計終了日時
            .Append("    AND EI.DELFLG = :DELFLG ")                         '見積情報.削除フラグ：削除以外
            .Append("    GROUP BY ")
            .Append("         EI.DLRCD ")
            .Append("       , EV.SERIESCD ")
            .Append("       , EV.MODELCD ")
            .Append("    ) VCL ")                                           '契約車両集計テーブル
            ' ========== 契約車両集計用のサブクエリ 終了 ==========
            ' ========== 契約車両オプション集計用のサブクエリ 開始 ==========
            .Append("  , ( ")
            .Append("    SELECT ")
            .Append("         EI.DLRCD ")                                   '見積情報.販売店コード
            .Append("       , EV.SERIESCD ")                                '見積車両情報.シリーズコード
            .Append("       , EV.MODELCD ")                                 '見積車両情報.モデルコード
            .Append("       , EO.OPTIONPART ")                              '見積車両オプション情報.オプション区分
            .Append("       , EO.OPTIONCODE ")                              '見積車両オプション情報.オプションコード
            .Append("       , COUNT(1) AS CONTRACTCOUNT ")                  '販売店、モデル、オプションごとの契約車両オプション数
            .Append("    FROM ")
            .Append("        TBL_ESTIMATEINFO EI ")
            .Append("      , TBL_EST_VCLINFO EV ")
            .Append("      , TBL_EST_VCLOPTIONINFO EO ")
            .Append("    WHERE ")
            .Append("        EI.ESTIMATEID = EV.ESTIMATEID ")               '見積情報.見積管理ID = 見積車両情報.見積管理ID
            .Append("    AND EI.ESTIMATEID = EO.ESTIMATEID ")               '見積情報.見積管理ID = 見積車両オプション情報.見積管理ID
            .Append("    AND EI.CONTRACTFLG = :CONTRACTFLG ")               '見積情報.契約状況フラグ
            .Append("    AND EI.CONTRACTDATE >= :STARTDATE ")               '見積情報.契約完了日 集計開始日時
            .Append("    AND EI.CONTRACTDATE < :ENDDATE ")                  '見積情報.契約完了日 集計終了日時
            .Append("    AND EI.DELFLG = :DELFLG ")                         '見積情報.削除フラグ：削除以外
            .Append("    AND EO.OPTIONPART IN ( ")                          '見積車両オプション情報情報.オプション区分
            .Append("                           :OptionPartMaker ")         'オプション区分：メーカー
            .Append("                         , :OptionPartDealer ")        'オプション区分：販売店
            .Append("                          ) ")
            .Append("    GROUP BY ")
            .Append("         EI.DLRCD , EV.SERIESCD , EV.MODELCD , EO.OPTIONPART , EO.OPTIONCODE ")
            .Append("    ) OPT ")
            ' ========== 契約車両オプション集計用のサブクエリ 終了 ==========
            .Append("WHERE ")
            .Append("    VCL.DLRCD = OPT.DLRCD ")                           '契約車両集計.販売店コード = 契約車両オプション集計.販売店コード
            .Append("AND VCL.SERIESCD = OPT.SERIESCD ")                     '契約車両集計.シリーズコード = 契約車両オプション集計.シリーズコード
            .Append("AND VCL.MODELCD = OPT.MODELCD ")                       '契約車両集計.モデルコード = 契約車両オプション集計.モデルコード
        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3080401_002")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("CONTRACTFLG", OracleDbType.Char, ContractFlgCompleted)         '契約状況フラグ：契約済
            query.AddParameterWithTypeValue("STARTDATE", OracleDbType.Date, startDate)                      '集計対象開始日時
            query.AddParameterWithTypeValue("ENDDATE", OracleDbType.Date, endDate)                          '集計対象終了日時
            query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DelFlgValid)                       '削除フラグ：削除以外
            query.AddParameterWithTypeValue("OptionPartMaker", OracleDbType.Char, OptionPartMaker)          'オプション区分：メーカー
            query.AddParameterWithTypeValue("OptionPartDealer", OracleDbType.Char, OptionPartDealer)        'オプション区分：販売店
            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, CreateUserAccount)      '作成ユーザアカウント
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, CreateUserAccount)      '更新ユーザアカウント
            query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, FunctionId)                  '作成機能ID
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, FunctionId)                  '更新機能ID

            ' SQL実行
            Dim retCount As Integer = query.Execute()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, retCount))
            ' ======================== ログ出力 終了 ========================

            ' 結果を返却
            Return retCount
        End Using
    End Function
#End Region

    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
#Region "ロック取得クエリ"
    ''' <summary>
    ''' 購入率集計情報のロックを取得。
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub GetPurchaseRateLock()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of DataTable)("MC3070401_003")

            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* MC3070401_003 */ ")
                .Append("       1 ")
                .Append("  FROM TBL_PURCHASERATE ")
                .Append("   FOR UPDATE ")
            End With

            query.CommandText = sql.ToString()

            ' SQL実行
            query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, "1"))
            ' ======================== ログ出力 終了 ========================

        End Using

    End Sub
#End Region
    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

#End Region

End Class
