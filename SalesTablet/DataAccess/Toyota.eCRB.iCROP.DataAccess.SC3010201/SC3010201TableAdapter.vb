Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Globalization
Imports System.Reflection

''' <summary>
''' メインメニューのデータアクセスクラスです。
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3010201TableAdapter

#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SYSTEM = "SC3010201"

#End Region

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' 連絡事項を取得します。
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="strCd">店舗コード</param>
    ''' <returns>連絡事項一覧</returns>
    ''' <remarks></remarks>
    Public Shared Function GetMessageInfo(ByVal dlrcd As String, ByVal strcd As String) As SC3010201DataSet.SC3010201MessageDataTable

        Using query As New DBSelectQuery(Of SC3010201DataSet.SC3010201MessageDataTable)("SC3010201_001")

            Dim sql As New StringBuilder
            Dim basedate As Date = DateTimeFunc.Now(dlrcd)

            '時分秒切捨て
            basedate = New Date(basedate.Year, basedate.Month, basedate.Day)

            'SQL文作成
            With sql
                .Append(" SELECT /* SC3010201_001 */")
                .Append("        MESSAGENO")
                .Append("      , TITLE")
                .Append("      , MESSAGE")
                .Append("      , CREATEDATE")
                ' 2012/01/23 TCS 相田 【SALES_1B】 START
                .Append("      , CREATESTAFFCD")
                ' 2012/01/23 TCS 相田 【SALES_1B】 END
                .Append("   FROM TBL_MESSAGEINFO")
                .Append("  WHERE DLRCD = :DLRCD")
                .Append("    AND STRCD = :STRCD")
                .Append("    AND DELFLG = '0'")
                .Append("    AND TIMELIMIT >= :BASEDATE")
                .Append("  ORDER BY CREATEDATE DESC")
            End With
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
            query.AddParameterWithTypeValue("BASEDATE", OracleDbType.Date, basedate)

            '検索結果返却
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' RSSの最大取得件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAX_RSSCOUNT As Integer = 10

    ''' <summary>
    ''' RSS情報を取得します。
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="strCd">店舗コード</param>
    ''' <returns>RSS情報一覧</returns>
    ''' <remarks></remarks>
    Public Shared Function GetRssInfo(ByVal dlrcd As String, ByVal strcd As String) As SC3010201DataSet.SC3010201RssDataTable

        Using query As New DBSelectQuery(Of SC3010201DataSet.SC3010201RssDataTable)("SC3010201_002")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append(" SELECT /* SC3010201_002 */")
                .Append("        ITEM_TITLE")
                .Append("      , ITEM_URL")
                .Append("      , ITEM_CREATEDATE")
                .Append("   FROM (")
                .Append("       SELECT B.ITEM_TITLE")
                .Append("            , B.ITEM_URL")
                .Append("            , B.ITEM_CREATEDATE")
                .Append("            , ROW_NUMBER() OVER(ORDER BY B.ITEM_CREATEDATE DESC) AS SEQNO")
                .Append("         FROM TBL_RSS_SITEMANAGER A")
                .Append("            , TBL_RSS_ITEMDATA B")
                .Append("            , TBL_RSS_SITEDATA C")
                .Append("        WHERE A.SITENO = B.SITENO")
                .Append("          AND A.SITENO = C.SITENO")
                .Append("          AND C.DELFLG = '0'")
                .Append("          AND A.DELFLG = '0'")
                .Append("          AND (")
                .Append("                  (A.DLRCD, A.STRCD) IN (SELECT 'XXXXX', 'XXX' FROM DUAL)")
                .Append("               OR ")
                .Append("                  (A.DLRCD, A.STRCD) IN (SELECT :DLRCD, :STRCD FROM DUAL)")
                .Append("              )")
                .Append("       )")
                .Append("  WHERE SEQNO <= ").Append(MAX_RSSCOUNT)
                .Append("  ORDER BY ITEM_CREATEDATE DESC")
            End With
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)

            '検索結果返却
            Return query.GetData()
        End Using
    End Function

#Region "連絡事項 削除フラグ更新"
    ' 2012/01/23 TCS 藤井 【SALES_1B】 START
    ''' <summary>
    ''' 連絡事項の削除フラグを更新する。
    ''' </summary>
    ''' <param name="messageno">メッセージNo.</param>
    ''' <param name="updateaccount">更新アカウント</param>
    ''' <param name="updateid">更新ID</param>
    ''' <returns>処理結果件数</returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateMessageInfoDelFlg(ByVal messageno As Long, ByVal updateaccount As String, ByVal updateid As String) As Integer
        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3010201_003")

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* SC3010201_003 */ ")
                .Append("        TBL_MESSAGEINFO ")
                .Append("    SET DELFLG = '1' ")
                .Append("      , UPDATEDATE = SYSDATE ")
                .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                .Append("      , UPDATEID = :UPDATEID ")
                .Append("  WHERE MESSAGENO = :MESSAGENO ")
            End With

            query.CommandText = sql.ToString()

            'バインド変数
            query.AddParameterWithTypeValue("MESSAGENO", OracleDbType.Long, messageno)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateaccount)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateid)

            'SQL実行(影響行数を返却)
            Return query.Execute()
        End Using
    End Function
    ' 2012/01/23 TCS 藤井 【SALES_1B】 END
#End Region

    '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
    ''' <summary>
    ''' 連絡事項のロック取得
    ''' </summary>
    ''' <param name="messageno">メッセージNo.</param>
    ''' <remarks></remarks>
    Public Shared Sub GetMessageInfoLock(ByVal messageno As Long)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

            Using query As New DBSelectQuery(Of DataTable)("SC3010201_004")

                Dim env As New SystemEnvSetting
                Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* SC3010201_004 */ ")
                    .Append("        1 ")
                    .Append("   FROM TBL_MESSAGEINFO ")
                    .Append("  WHERE MESSAGENO = :MESSAGENO ")
                    .Append(sqlForUpdate)
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("MESSAGENO", OracleDbType.Long, messageno)
                query.GetData()

            End Using

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name, 1))
        ' ======================== ログ出力 終了 ========================

    End Sub
    '2013/06/30 TCS 山田 2013/10対応版 既存流用 END


End Class