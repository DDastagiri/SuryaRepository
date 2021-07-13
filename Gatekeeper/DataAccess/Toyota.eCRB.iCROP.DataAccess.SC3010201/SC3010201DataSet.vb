Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace SC3010201DataSetTableAdapters

    ''' <summary>
    ''' メインメニューのデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3010201TableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
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
                    .Append("        WHERE A.SITENO = B.SITENO")
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

    End Class

End Namespace

Partial Class SC3010201DataSet
  

End Class
