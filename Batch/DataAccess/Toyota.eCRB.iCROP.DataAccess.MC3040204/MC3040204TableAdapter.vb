Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Reflection

#Region "RSS情報関連"
''' <summary>
''' Rss情報操作クラス
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class MC3040204TableAdapter

    '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SYSTEM = "MC3040204"

#End Region
    '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

#Region "デフォルトコンストラクタ処理"
    ''' <summary>
    ''' デフォルトコンストラクタ処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub
#End Region

#Region "RSSサイト情報取得処理"
    ''' <summary>
    ''' RSSサイト情報取得処理
    ''' </summary>
    ''' <returns>RSSサイト情報データテーブル</returns>
    ''' <param name="delflg">削除フラグ</param>
    ''' <remarks></remarks>
    Public Shared Function GetRssSiteData(ByVal delflg As String) As MC3040204DataSet.MC3040204RssSiteInfoDataTable
        'DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of MC3040204DataSet.MC3040204RssSiteInfoDataTable)("MC3040204_001")

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* MC3040204_001 */ ")
                .Append("        SITENO ")
                .Append("      , SITE_RSSURL ")
                .Append("      , SITE_LASTUPDATE ")
                .Append("      , TIMEDIFFERENCE ")
                .Append("   FROM TBL_RSS_SITEDATA ")
                .Append("  WHERE DELFLG = :DELFLG ")
            End With

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, delflg)

            'SQL実行(結果表を返却)
            Return query.GetData()

        End Using

    End Function
#End Region

#Region "RSSサイト情報更新処理"
    ''' <summary>
    ''' RSSサイト情報更新処理
    ''' </summary>
    ''' <param name="dt">RSSサイト情報データテーブル</param>
    ''' <returns>処理結果(件数)</returns>
    ''' <remarks></remarks>
    Public Shared Function UpDataRssSiteData(ByVal dt As MC3040204DataSet.MC3040204RssSiteInfoDataTable) As Integer
        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("MC3040204_003")
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* MC3040204_003 */ ")
                .Append("        TBL_RSS_SITEDATA ")
                .Append("    SET SITE_LASTUPDATE = :SITE_LASTUPDATE ")
                .Append("      , UPDATEDATE = SYSDATE ")
                .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT ")
                .Append("      , UPDATEID = :UPDATEID ")
                .Append("  WHERE SITENO = :SITENO ")
            End With

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            Dim dr As MC3040204DataSet.MC3040204RssSiteInfoRow = CType(dt.Rows(0), MC3040204DataSet.MC3040204RssSiteInfoRow)
            query.AddParameterWithTypeValue("SITE_LASTUPDATE", OracleDbType.Date, dr.SITE_LASTUPDATE)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, dr.ACCOUNT)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, dr.ACCOUNT)
            query.AddParameterWithTypeValue("SITENO", OracleDbType.Long, dr.SITENO)

            'SQL実行(影響行数を返却)
            Return query.Execute()
        End Using

    End Function
#End Region

#Region "RSS記事情報登録処理"
    ''' <summary>
    ''' RSS記事情報登録処理
    ''' </summary>
    ''' <param name="dt">RSS記事情報データテーブル</param>
    ''' <returns>処理結果(件数)</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertRssItemData(ByVal dt As MC3040204DataSet.MC3040204RssItemDataTable) As Integer
        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("MC3040204_002")
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine("INSERT /*MC3040204_002*/ ")
                .AppendLine("  INTO TBL_RSS_ITEMDATA( ")
                .AppendLine("       SITENO ")
                .AppendLine("     , ITEMID ")
                .AppendLine("     , ITEM_TITLE ")
                .AppendLine("     , ITEM_URL ")
                .AppendLine("     , ITEM_SELECT ")
                .AppendLine("     , ITEM_CREATEDATE ")
                .AppendLine("     , ITEM_CATEGORY ")
                .AppendLine("     , CREATEDATE ")
                .AppendLine("     , UPDATEDATE ")
                .AppendLine("     , CREATEACCOUNT ")
                .AppendLine("     , UPDATEACCOUNT ")
                .AppendLine("     , CREATEID ")
                .AppendLine("     , UPDATEID ")
                .AppendLine(") ")
                .AppendLine("VALUES ( ")
                .AppendLine("       :SITENO ")
                .AppendLine("     , SEQ_RSSSITEITEMID.NEXTVAL ")
                .AppendLine("     , :ITEM_TITLE ")
                .AppendLine("     , :ITEM_URL ")
                .AppendLine("     , :ITEM_SELECT ")
                .AppendLine("     , :ITEM_CREATEDATE ")
                .AppendLine("     , :ITEM_CATEGORY ")
                .AppendLine("     , SYSDATE ")
                .AppendLine("     , SYSDATE ")
                .AppendLine("     , :CREATEACCOUNT ")
                .AppendLine("     , :UPDATEACCOUNT ")
                .AppendLine("     , :CREATEID ")
                .AppendLine("     , :UPDATEID ")
                .AppendLine(") ")
            End With

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            Dim dr As MC3040204DataSet.MC3040204RssItemRow = CType(dt.Rows(0), MC3040204DataSet.MC3040204RssItemRow)

            query.AddParameterWithTypeValue("SITENO", OracleDbType.Long, dr.SITENO)
            query.AddParameterWithTypeValue("ITEM_TITLE", OracleDbType.NVarchar2, dr.ITEM_TITLE)
            query.AddParameterWithTypeValue("ITEM_URL", OracleDbType.NVarchar2, dr.ITEM_URL)

            If dr.IsITEM_SELECTNull Then
                query.AddParameterWithTypeValue("ITEM_SELECT", OracleDbType.NVarchar2, DBNull.Value)
            Else
                query.AddParameterWithTypeValue("ITEM_SELECT", OracleDbType.NVarchar2, dr.ITEM_SELECT)
            End If

            query.AddParameterWithTypeValue("ITEM_CREATEDATE", OracleDbType.Date, dr.ITEM_CREATEDATE)

            If dr.IsITEM_CATEGORYNull Then
                query.AddParameterWithTypeValue("ITEM_CATEGORY", OracleDbType.NVarchar2, DBNull.Value)
            Else
                query.AddParameterWithTypeValue("ITEM_CATEGORY", OracleDbType.NVarchar2, dr.ITEM_CATEGORY)
            End If

            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.NVarchar2, dr.ACCOUNT)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, dr.ACCOUNT)
            query.AddParameterWithTypeValue("CREATEID", OracleDbType.NVarchar2, dr.ACCOUNT)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, dr.ACCOUNT)

            'SQL実行(影響行数を返却)
            Return query.Execute()
        End Using

    End Function
#End Region

#Region "RSS記事情報退避処理"
    ''' <summary>
    ''' RSS記事情報退避処理
    ''' </summary>
    ''' <param name="dt">RSS記事情報データテーブル</param>
    ''' <returns>処理結果(件数)</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertRssItemPast(ByVal dt As MC3040204DataSet.MC3040204RssItemDataTable) As Integer
        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("MC3040204_004")
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine("INSERT /* MC3040204_004 */ ")
                .AppendLine("  INTO TBL_RSS_ITEMDATA_PAST( ")
                .AppendLine("       SITENO ")
                .AppendLine("     , ITEMID ")
                .AppendLine("     , ITEM_TITLE ")
                .AppendLine("     , ITEM_URL ")
                .AppendLine("     , ITEM_SELECT ")
                .AppendLine("     , ITEM_CREATEDATE ")
                .AppendLine("     , ITEM_CATEGORY ")
                .AppendLine("     , CREATEDATE ")
                .AppendLine("     , UPDATEDATE ")
                .AppendLine("     , CREATEACCOUNT ")
                .AppendLine("     , UPDATEACCOUNT ")
                .AppendLine("     , CREATEID ")
                .AppendLine("     , UPDATEID ")
                .AppendLine(") ")
                .AppendLine("SELECT ")
                .AppendLine("       SITENO ")
                .AppendLine("     , ITEMID ")
                .AppendLine("     , ITEM_TITLE ")
                .AppendLine("     , ITEM_URL ")
                .AppendLine("     , ITEM_SELECT ")
                .AppendLine("     , ITEM_CREATEDATE ")
                .AppendLine("     , ITEM_CATEGORY ")
                .AppendLine("     , SYSDATE ")
                .AppendLine("     , SYSDATE ")
                .AppendLine("     , :CREATEACCOUNT ")
                .AppendLine("     , :UPDATEACCOUNT ")
                .AppendLine("     , :CREATEID ")
                .AppendLine("     , :UPDATEID ")
                .AppendLine("  FROM ")
                .AppendLine("       TBL_RSS_ITEMDATA ")
                .AppendLine(" WHERE ")
                .AppendLine("       ITEM_CREATEDATE <= :ITEM_CREATEDATE ")
            End With

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            Dim dr As MC3040204DataSet.MC3040204RssItemRow = CType(dt.Rows(0), MC3040204DataSet.MC3040204RssItemRow)
            query.AddParameterWithTypeValue("ITEM_CREATEDATE", OracleDbType.Date, dr.ITEM_CREATEDATE)
            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.NVarchar2, dr.ACCOUNT)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, dr.ACCOUNT)
            query.AddParameterWithTypeValue("CREATEID", OracleDbType.NVarchar2, dr.ACCOUNT)
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, dr.ACCOUNT)

            'SQL実行(影響行数を返却)
            Return query.Execute()
        End Using

    End Function

#End Region

#Region "RSS記事情報削除処理"
    ''' <summary>
    ''' RSS記事情報削除処理
    ''' </summary>
    ''' <param name="dt">RSS記事情報データテーブル</param>
    ''' <returns>処理結果(件数)</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteRssItemData(ByVal dt As MC3040204DataSet.MC3040204RssItemDataTable) As Integer
        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("MC3040204_005")
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine("DELETE /* MC3040204_005 */ ")
                .AppendLine("  FROM TBL_RSS_ITEMDATA ")
                .AppendLine(" WHERE ITEM_CREATEDATE <= :ITEM_CREATEDATE ")
            End With

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            Dim dr As MC3040204DataSet.MC3040204RssItemRow = CType(dt.Rows(0), MC3040204DataSet.MC3040204RssItemRow)
            query.AddParameterWithTypeValue("ITEM_CREATEDATE", OracleDbType.Date, dr.ITEM_CREATEDATE)

            'SQL実行(影響行数を返却)
            Return query.Execute()
        End Using
    End Function

#End Region

#Region "RSS記事情報(退避)削除処理"
    ''' <summary>
    ''' RSS記事情報(退避)削除処理
    ''' </summary>
    ''' <param name="dt">RSS記事情報データテーブル</param>
    ''' <returns>処理結果(件数)</returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteRssItemPast(ByVal dt As MC3040204DataSet.MC3040204RssItemDataTable) As Integer
        'DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("MC3040204_006")
            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .AppendLine("DELETE /* MC3040204_006 */ ")
                .AppendLine("  FROM TBL_RSS_ITEMDATA_PAST ")
                .AppendLine(" WHERE ITEM_CREATEDATE <= :ITEM_CREATEDATE ")
            End With

            query.CommandText = sql.ToString()

            'SQLパラメータ設定
            Dim dr As MC3040204DataSet.MC3040204RssItemRow = CType(dt.Rows(0), MC3040204DataSet.MC3040204RssItemRow)
            query.AddParameterWithTypeValue("ITEM_CREATEDATE", OracleDbType.Date, dr.ITEM_CREATEDATE)

            'SQL実行(影響行数を返却)
            Return query.Execute()
        End Using
    End Function

#End Region

    '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
#Region "RSSサイト情報ロック取得（RSS記事情報削除用）"
    ''' <summary>
    ''' RSSサイト情報ロック取得（RSS記事情報削除用）
    ''' </summary>
    ''' <param name="ItemCreateDate">RSS記事情報の退避期限</param>
    ''' <remarks></remarks>
    Public Shared Sub GetRssSiteItemLock(ByVal itemCreateDate As Date)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of DataTable)("MC3040204_007")

            Dim sql As New StringBuilder

            With sql
                .Append("   SELECT /* MC3040204_007 */ ")
                .Append("          1 ")
                .Append("     FROM TBL_RSS_SITEDATA T1 ")
                .Append("        , TBL_RSS_ITEMDATA T2 ")
                .Append("    WHERE T1.SITENO = T2.SITENO ")
                .Append("      AND T2.ITEM_CREATEDATE <= :ITEM_CREATEDATE ")
                .Append("   FOR UPDATE OF T1.SITENO ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ITEM_CREATEDATE", OracleDbType.Date, itemCreateDate)
            query.GetData()

        End Using

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name, 1))
        ' ======================== ログ出力 終了 ========================

    End Sub
#End Region

#Region "RSSサイト情報ロック取得（RSS記事情報(退避)削除用）"
    ''' <summary>
    ''' RSSサイト情報ロック取得（RSS記事情報(退避)削除用）
    ''' </summary>
    ''' <param name="ItemCreateDatePast">RSS記事情報(退避)の削除期限</param>
    ''' <remarks></remarks>
    Public Shared Sub GetRssSiteItemPastLock(ByVal itemCreateDatePast As Date)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of DataTable)("MC3040204_008")

            Dim sql As New StringBuilder

            With sql
                .Append("   SELECT /* MC3040204_008 */ ")
                .Append("          1 ")
                .Append("     FROM TBL_RSS_SITEDATA T1 ")
                .Append("        , TBL_RSS_ITEMDATA_PAST T2 ")
                .Append("    WHERE T1.SITENO = T2.SITENO ")
                .Append("      AND T2.ITEM_CREATEDATE <= :ITEM_CREATEDATE_PAST ")
                .Append("   FOR UPDATE OF T1.SITENO ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ITEM_CREATEDATE_PAST", OracleDbType.Date, itemCreateDatePast)
            query.GetData()

        End Using

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name, 1))
        ' ======================== ログ出力 終了 ========================

    End Sub
#End Region

#Region "RSSサイト情報ロック取得（RSSサイト情報更新用）"
    ''' <summary>
    ''' RSSサイト情報ロック取得（RSSサイト情報更新用）
    ''' </summary>
    ''' <param name="rssinfo">RSSサイト情報</param>
    ''' <remarks></remarks>
    Public Shared Sub GetRssSiteLock(ByVal rssinfo As MC3040204DataSet.MC3040204RssSiteInfoDataTable)

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        If rssinfo.Count = 0 Then
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, Nothing))
            ' ======================== ログ出力 終了 ========================
            Exit Sub
        End If

        Using query As New DBSelectQuery(Of DataTable)("MC3040204_009")

            Dim sql As New StringBuilder

            ' SITENOのWHERE条件文字列
            Dim sqlSiteNo As New StringBuilder

            sqlSiteNo.Append("          T1.SITENO IN (")
            Dim count As Long = 1
            Dim siteNo As String
            For Each row As MC3040204DataSet.MC3040204RssSiteInfoRow In rssinfo
                ' SQL作成
                siteNo = String.Format(CultureInfo.CurrentCulture, "SITENO{0}", count)
                If count > 1 Then
                    sqlSiteNo.Append(String.Format(CultureInfo.CurrentCulture, ", :{0} ", siteNo))
                Else
                    sqlSiteNo.Append(String.Format(CultureInfo.CurrentCulture, "  :{0} ", siteNo))
                End If

                ' パラメータ作成
                query.AddParameterWithTypeValue(siteNo, OracleDbType.Long, row.SITENO)
                count += 1
            Next
            sqlSiteNo.Append(" ) ")

            With sql
                .Append("   SELECT /* MC3040204_009 */ ")
                .Append("          1 ")
                .Append("     FROM TBL_RSS_SITEDATA T1 ")
                .Append("    WHERE ")
                .Append(sqlSiteNo.ToString())
                .Append("   FOR UPDATE ")
            End With

            query.CommandText = sql.ToString()
            query.GetData()

        End Using

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name, 1))
        ' ======================== ログ出力 終了 ========================

    End Sub
#End Region
    '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

End Class

#End Region

