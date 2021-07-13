Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Globalization

''' <summary>
''' メインメニューのデータアクセスクラスです。
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3010201TableAdapter

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
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , "Toyota.eCRB.Common.MainMenu.DataAccess" _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using query As New DBSelectQuery(Of SC3010201DataSet.SC3010201MessageDataTable)("SC3010201_001")

            Dim sql As New StringBuilder
            Dim basedate As Date = DateTimeFunc.Now(dlrcd)

            '時分秒切捨て
            basedate = New Date(basedate.Year, basedate.Month, basedate.Day)

            'SQL文作成
            With sql
                .AppendLine(" SELECT /* SC3010201_001 */")
                .AppendLine("        MESSAGENO")
                .AppendLine("      , TITLE")
                .AppendLine("      , MESSAGE")
                .AppendLine("      , CREATEDATE")
                ' 2012/01/23 TCS 相田 【SALES_1B】 START
                .AppendLine("      , CREATESTAFFCD")
                ' 2012/01/23 TCS 相田 【SALES_1B】 END
                .AppendLine("   FROM TBL_MESSAGEINFO")
                .AppendLine("  WHERE DLRCD = :DLRCD")
                .AppendLine("    AND STRCD = :STRCD")
                .AppendLine("    AND DELFLG = '0'")
                .AppendLine("    AND TIMELIMIT >= :BASEDATE")
                .AppendLine("  ORDER BY CREATEDATE DESC")
            End With
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)
            query.AddParameterWithTypeValue("BASEDATE", OracleDbType.Date, basedate)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , "Toyota.eCRB.Common.MainMenu.DataAccess" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
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
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , "Toyota.eCRB.Common.MainMenu.DataAccess" _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using query As New DBSelectQuery(Of SC3010201DataSet.SC3010201RssDataTable)("SC3010201_002")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine(" SELECT /* SC3010201_002 */")
                .AppendLine("        ITEM_TITLE")
                .AppendLine("      , ITEM_URL")
                .AppendLine("      , ITEM_CREATEDATE")
                .AppendLine("   FROM (")
                .AppendLine("       SELECT B.ITEM_TITLE")
                .AppendLine("            , B.ITEM_URL")
                .AppendLine("            , B.ITEM_CREATEDATE")
                .AppendLine("            , ROW_NUMBER() OVER(ORDER BY B.ITEM_CREATEDATE DESC) AS SEQNO")
                .AppendLine("         FROM TBL_RSS_SITEMANAGER A")
                .AppendLine("            , TBL_RSS_ITEMDATA B")
                .AppendLine("            , TBL_RSS_SITEDATA C")
                .AppendLine("        WHERE A.SITENO = B.SITENO")
                .AppendLine("          AND A.SITENO = C.SITENO")
                .AppendLine("          AND C.DELFLG = '0'")
                .AppendLine("          AND A.DELFLG = '0'")
                .AppendLine("          AND (")
                .AppendLine("                  (A.DLRCD, A.STRCD) IN (SELECT 'XXXXX', 'XXX' FROM DUAL)")
                .AppendLine("               OR ")
                .AppendLine("                  (A.DLRCD, A.STRCD) IN (SELECT :DLRCD, :STRCD FROM DUAL)")
                .AppendLine("              )")
                .AppendLine("       )")
                .AppendLine("  WHERE SEQNO <= ").Append(MAX_RSSCOUNT)
                .AppendLine("  ORDER BY ITEM_CREATEDATE DESC")
            End With
            query.CommandText = sql.ToString()
            'バインド変数
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , "Toyota.eCRB.Common.MainMenu.DataAccess" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
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
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , "Toyota.eCRB.Common.MainMenu.DataAccess" _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3010201_003")

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine(" UPDATE /* SC3010201_003 */ ")
                .AppendLine("        TBL_MESSAGEINFO ")
                .AppendLine("    SET DELFLG = '1' ")
                .AppendLine("      , UPDATEDATE = SYSDATE ")
                .AppendLine("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                .AppendLine("      , UPDATEID = :UPDATEID ")
                .AppendLine("  WHERE MESSAGENO = :MESSAGENO ")
            End With

            query.CommandText = sql.ToString()

            'バインド変数
            query.AddParameterWithTypeValue("MESSAGENO", OracleDbType.Long, messageno)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, updateaccount)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateid)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , "Toyota.eCRB.Common.MainMenu.DataAccess" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            'SQL実行(影響行数を返却)
            Return query.Execute()
        End Using
    End Function
    ' 2012/01/23 TCS 藤井 【SALES_1B】 END
#End Region

End Class