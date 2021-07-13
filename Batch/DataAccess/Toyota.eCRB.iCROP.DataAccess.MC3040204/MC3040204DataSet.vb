Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

#Region "RSS情報関連"
''' <summary>
''' Rss情報操作クラス
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class MC3040204TableAdapter

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

End Class

#End Region

