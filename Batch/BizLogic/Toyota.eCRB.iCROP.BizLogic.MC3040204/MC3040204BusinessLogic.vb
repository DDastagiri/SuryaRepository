'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'MC3040204BusinessLogic.vb
'─────────────────────────────────────
'機能： RSS登録バッチ
'補足： 
'作成： 2011/12/01 TCS 藤井
'更新： 2012/02/15 TCS 藤井  【SALES_1A】号口(課題No.53)対応
'更新： 2013/06/30 TCS 山田 2013/10対応版 既存流用
'─────────────────────────────────────

Imports System
Imports System.IO
Imports System.Xml
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Batch
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Tool.Rss.Batch.DataAccess
Imports System.Globalization
Imports System.Reflection

''' <summary>
''' RSSファイルを読み込み、読み込んだRSS情報をi-CROPのDBに登録する機能
''' </summary>
''' <remarks></remarks>
Public Class MC3040204BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"
    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_ACCOUNT As String = "MC3040204"

    ''' <summary>
    ''' 更新日付比較時に使用("xxxx/xx/xx 23:59:59"のように使用する。)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_TIME As String = " 23:59:59"

    ''' <summary>
    ''' 削除フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DEL_FLG As String = "0" '削除以外

    ''' <summary>
    ''' DB登録最大文字数でカット時のITEM特定情報用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_CUT_ITEMPART As Integer = 10

    ' 2012/02/15 TCS 藤井 【SALES_1A】号口(課題No.53)対応 ADD START
    ''' <summary>
    ''' リネーム時の出力ファイル名用システム日付の形式
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DATE_FORMAT As String = "yyyyMMddHHmmss"

    ''' <summary>
    ''' アンダーバー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UNDERBAR As String = "_"
    ' 2012/02/15 TCS 藤井 【SALES_1A】号口(課題No.53)対応 ADD END

#Region "XMLファイルのタグ名"
    ''' <summary>
    ''' itemタグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_XML_ITEM As String = "item"

    ''' <summary>
    ''' titleタグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_XML_TITLE As String = "title"

    ''' <summary>
    ''' linkタグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_XML_LINK As String = "link"

    ''' <summary>
    ''' descriptionタグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_XML_DESCRIPTION As String = "description"

    ''' <summary>
    ''' pubDateタグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_XML_PUBDATE As String = "pubDate"

    ''' <summary>
    ''' categoryタグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_XML_CATEGORY As String = "category"
#End Region

#Region "各項目のDB登録カット文字数"
    ''' <summary>
    ''' ITEM_TITLEのDB登録最大文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_FIGURE_ITEM_TITLE As Integer = 50

    ''' <summary>
    ''' ITEM_URLのDB登録最大文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_FIGURE_ITEM_URL As Integer = 256

    ''' <summary>
    ''' ITEM_SELECTのDB登録最大文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_FIGURE_ITEM_SELECT As Integer = 250

    ''' <summary>
    ''' ITEM_CATEGORYのDB登録最大文字数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_FIGURE_ITEM_CATEGORY As Integer = 50
#End Region

#Region "処理対象テーブル"
    ''' <summary>
    ''' RSS記事情報テーブル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_TBL_RSS_ITEMDATA As String = "TBL_RSS_ITEMDATA"

    ''' <summary>
    ''' RSSサイト情報テーブル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_TBL_RSS_SITEDATA As String = "TBL_RSS_SITEDATA"

    ''' <summary>
    ''' RSS記事情報(退避)テーブル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_TBL_RSS_ITEMDATA_PAST As String = "TBL_RSS_ITEMDATA_PAST"
#End Region

#Region "処理"
    ''' <summary>
    ''' INSERT処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_INSERT As String = "INSERT"

    ''' <summary>
    ''' UPDATE処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_UPDATE As String = "UPDATE"

    ''' <summary>
    ''' DELETE処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DELETE As String = "DELETE"
#End Region

#Region "バッチ終了コード"
    ''' <summary>
    ''' バッチ終了コード：処理成功
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SUCCESS As Integer = 0

    ''' <summary>
    ''' バッチ終了コード：警告
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_WARN As Integer = 2
#End Region

#End Region

#Region "変数"
    ''' <summary>
    ''' メッセージ文言
    ''' </summary>
    ''' <remarks></remarks>
    Private message001 As String = BatchWordUtility.GetWord(1)
    Private message902 As String = BatchWordUtility.GetWord(902)
    Private message901 As String = BatchWordUtility.GetWord(901)
    Private message903 As String = BatchWordUtility.GetWord(903)
    Private message904 As String = BatchWordUtility.GetWord(904)
    Private message905 As String = BatchWordUtility.GetWord(905)
    Private message906 As String = BatchWordUtility.GetWord(906)

    ''' <summary>
    ''' バッチ終了コード
    ''' </summary>
    ''' <remarks></remarks>
    Private finishCode As Integer = C_SUCCESS

    ''' <summary>
    ''' RSSファイルの取得先フォルダ
    ''' </summary>
    ''' <remarks></remarks>
    Private rssPath As String

    ''' <summary>
    ''' RSSファイル情報の取込失敗時のファイル移動先
    ''' </summary>
    ''' <remarks></remarks>
    Private movePath As String

#End Region

#Region "Enum"
    ''' <summary>
    ''' RSSファイルElementタイプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum rssElement
        none
        title
        link
        description
        pubDate
        category
    End Enum
#End Region

#Region "メイン処理"
    ''' <summary>
    ''' RSS情報登録処理
    ''' </summary>
    ''' <returns>処理結果(処理成功:0,エラー:1,警告:2)</returns>
    ''' <remarks>本メソッドでは、以下のメソッドも利用しているため、例外情報に関しては、下記、メソッドを参照してください。</remarks>
    ''' <seealso>GetRssSiteData</seealso>
    ''' <seealso>UpDataRssSiteData</seealso>
    ''' <seealso>InsertRssItemData</seealso>
    ''' <seealso>InsertRssItemPast</seealso>
    ''' <seealso>DeleteRssItemData</seealso>
    ''' <seealso>DeleteRssItemPast</seealso>

    'EnableCommit属性を付与（メソッド全体が１トランザクション）
    <EnableCommit()>
    Public Function RegistRssInfo() As Integer

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        'RSSファイルフォルダのパス取得(DBから)
        rssPath = BatchSetting.GetValue("PATHINFO", "GetRSS", "")
        movePath = BatchSetting.GetValue("PATHINFO", "MoveRSS", "")

        'RSSサイト情報の取得
        Dim rssinfo As MC3040204DataSet.MC3040204RssSiteInfoDataTable
        rssinfo = MC3040204TableAdapter.GetRssSiteData(C_DEL_FLG)

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START

        'RSS記事情報の退避期限を取得
        Dim keepdate As Double = CDbl(BatchSetting.GetValue("TERMINFO", "ITEMKEEP", ""))
        Dim today As Date = Date.Today
        Dim targetdate As Date = today.AddDays(-keepdate)
        Dim ItemCreateDate As Date = DateTime.Parse((targetdate & C_TIME), CultureInfo.InvariantCulture)

        'RSS記事情報(退避)の削除期限を取得
        Dim keepdate_past As Double = CDbl(BatchSetting.GetValue("TERMINFO", "ITEMKEEP_PAST", ""))
        Dim targetdate_past As Date = today.AddDays(-keepdate_past)
        Dim ItemCreateDatePast As Date = DateTime.Parse((targetdate_past & C_TIME), CultureInfo.InvariantCulture)

        'RSSサイト情報ロック取得（RSS記事情報削除用）
        MC3040204TableAdapter.GetRssSiteItemLock(ItemCreateDate)

        'RSSサイト情報ロック取得（RSS記事情報(退避)削除用）
        MC3040204TableAdapter.GetRssSiteItemPastLock(ItemCreateDatePast)

        'RSSサイト情報ロック取得（RSSサイト情報更新用）
        MC3040204TableAdapter.GetRssSiteLock(rssinfo)
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        'RSSサイトの数
        Dim rssinfocnt As Integer = rssinfo.Rows.Count

        'RSSファイル取得,DBへ情報登録
        For i As Integer = 0 To rssinfocnt - 1
            '行を取得
            Dim dr As MC3040204DataSet.MC3040204RssSiteInfoRow = rssinfo.Item(i)

            '実際のファイルを元に記事の登録処理
            Dim maxDate As Date = GetRssFile(dr.SITENO, dr.SITE_RSSURL, dr.SITE_LASTUPDATE, dr.TIMEDIFFERENCE)

            'RSSサイト情報更新処理
            If Date.Compare(maxDate, dr.SITE_LASTUPDATE) > 0 Then
                UpdateRSSSiteData(dr.SITENO, maxDate)
            End If
        Next

        'RSS記事情報の退避
        MoveRssItemData()

        'RSS記事情報(退避)の削除
        DeleteRssItemPast()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name, finishCode))
        ' ======================== ログ出力 終了 ========================

        '処理結果の返却
        Return finishCode

    End Function

#End Region

#Region "取得関連"

#Region "RSSファイル取得,DBへ情報登録"
    ''' <summary>
    ''' RSSファイル取得,DBへ情報登録
    ''' </summary>
    ''' <param name="siteno">サイトNo.</param>
    ''' <param name="siteurl">サイトURL</param>
    ''' <param name="updatedate">RSS最終更新日付</param>
    ''' <param name="timedifference">時差</param>
    ''' <returns>RSSサイト最新更新日付</returns>
    ''' <remarks></remarks>
    Private Function GetRssFile(ByVal siteno As Long, siteurl As String, ByVal updatedate As Date, ByVal timedifference As Integer) As Date
        Dim rtnDate As Date = updatedate

        '実際のファイル名に変換
        Dim filename As String = siteurl.Replace("\", "").Replace("/", "").Replace(":", "").Replace("*", "").Replace("?", "").Replace("""", "").Replace("<", "").Replace(">", "")
        filename = filename & "*"

        '変換した名称を含むファイルを取得
        Dim files As String() = Directory.GetFiles(rssPath, filename, IO.SearchOption.TopDirectoryOnly)
        If files.Length = 0 Then
            Dim msg_warn1 As String = String.Format(CultureInfo.InvariantCulture, message901, filename)
            Logger.Info(msg_warn1)
            finishCode = C_WARN
        Else
            'ファイル分更新処理を行う
            For Each targetfile As String In files
                'RSSのXMLファイルの読み込み
                Dim dt As MC3040204DataSet.MC3040204RssItemDataTable
                Dim maxdate As Date
                dt = ReadRssFile(targetfile, timedifference)

                'RSS記事情報登録処理
                If dt.Rows.Count <> 0 AndAlso InsertRSSItemInfo(dt, siteno, rtnDate, Path.GetFileName(targetfile)) Then
                    'ファイルを削除する
                    File.Delete(targetfile)
                    Dim itemDatarow() As DataRow = dt.Select("ITEM_CREATEDATE = MAX(ITEM_CREATEDATE)", "")
                    maxdate = CDate(itemDatarow(0).Item("ITEM_CREATEDATE"))
                Else
                    '実際のファイル名の取得
                    Dim movefile As String = movePath & Path.DirectorySeparatorChar & Path.GetFileName(targetfile)
                    ' 2012/02/15 TCS 藤井 【SALES_1A】号口(課題No.53)対応 ADD START
                    'ファイルの存在確認
                    If (File.Exists(movefile)) Then
                        Dim sysDate As String = Date.Now.ToString(DATE_FORMAT, CultureInfo.InvariantCulture)
                        Dim rename As String = movefile & UNDERBAR & sysDate
                        File.Move(targetfile, rename)
                    Else
                        ' 2012/02/15 TCS 藤井 【SALES_1A】号口(課題No.53)対応 ADD END
                        'ファイルを別フォルダに移動
                        File.Move(targetfile, movefile)
                        ' 2012/02/15 TCS 藤井 【SALES_1A】号口(課題No.53)対応 ADD START
                    End If
                    ' 2012/02/15 TCS 藤井 【SALES_1A】号口(課題No.53)対応 ADD END

                End If

                If Date.Compare(rtnDate, maxdate) < 0 Then
                    rtnDate = maxdate
                End If

            Next
        End If
        Return rtnDate
    End Function

#End Region

#Region "XMLファイルの読み込み"
    ''' <summary>
    ''' XMLファイルの読み込み
    ''' </summary>
    ''' <param name="fileName">RSSファイル名</param>
    ''' <returns>RSS記事情報データテーブル</returns>
    ''' <param name="timedifference">時差</param>
    ''' <remarks></remarks>
    Private Function ReadRssFile(ByVal fileName As String, ByVal timedifference As Integer) As MC3040204DataSet.MC3040204RssItemDataTable
        Using dt As New MC3040204DataSet.MC3040204RssItemDataTable
            Dim dr As MC3040204DataSet.MC3040204RssItemRow = Nothing
            Dim cnt As Integer = 0
            Dim element As rssElement = rssElement.none

            Try
                Using reader As XmlReader = XmlReader.Create(fileName)

                    'XML読み取り処理
                    While reader.Read()
                        '要素 (例: <item>)
                        If reader.NodeType = XmlNodeType.Element Then

                            If String.Equals(reader.Name, C_XML_ITEM) Then
                                dt.Rows.Add(dt.NewRow)
                                dr = dt.Item(cnt)

                                cnt = cnt + 1
                            End If

                            If Not IsNothing(dr) Then
                                If String.Equals(reader.Name, C_XML_TITLE) Then
                                    element = rssElement.title
                                ElseIf String.Equals(reader.Name, C_XML_LINK) Then
                                    element = rssElement.link
                                ElseIf String.Equals(reader.Name, C_XML_DESCRIPTION) Then
                                    element = rssElement.description
                                ElseIf String.Equals(reader.Name, C_XML_PUBDATE) Then
                                    element = rssElement.pubDate
                                ElseIf String.Equals(reader.Name, C_XML_CATEGORY) Then
                                    element = rssElement.category
                                End If
                            End If
                        End If

                        'CDATAセクション (例:<![CDATA[xxxxx]]>),ノードのテキスト内容
                        If reader.NodeType = XmlNodeType.CDATA Or reader.NodeType = XmlNodeType.Text Then
                            If element = rssElement.title Then
                                '記事タイトル(DB登録桁数でカット)
                                dr.ITEM_TITLE = CutCharacter(reader.Value, C_FIGURE_ITEM_TITLE, fileName, C_XML_TITLE)
                                element = rssElement.none
                            ElseIf element = rssElement.link Then
                                '記事URL(DB登録桁数でカット)
                                dr.ITEM_URL = CutCharacter(reader.Value, C_FIGURE_ITEM_URL, fileName, C_XML_LINK)
                                element = rssElement.none
                            ElseIf element = rssElement.description Then
                                '記事内容(DB登録桁数でカット)
                                dr.ITEM_SELECT = CutCharacter(reader.Value, C_FIGURE_ITEM_SELECT, fileName, C_XML_DESCRIPTION)
                                element = rssElement.none
                            ElseIf element = rssElement.pubDate Then
                                '記事作成日時(日付の変換)
                                If Not String.IsNullOrEmpty(reader.Value) And IsDate(reader.Value) Then
                                    dr.ITEM_CREATEDATE = DateTime.Parse(reader.Value, CultureInfo.InvariantCulture).AddHours(timedifference)
                                End If
                                element = rssElement.none
                            ElseIf element = rssElement.category Then
                                'カテゴリ(DB登録桁数でカット)
                                dr.ITEM_CATEGORY = CutCharacter(reader.Value, C_FIGURE_ITEM_CATEGORY, fileName, C_XML_CATEGORY)
                                element = rssElement.none
                            End If
                        End If

                    End While
                End Using

            Catch ex As XmlException

                Dim msg_warn5 As String = String.Format(CultureInfo.InvariantCulture, message905, Path.GetFileName(fileName))
                Logger.Info(msg_warn5 & ex.Message)
                finishCode = C_WARN

                dt.Rows.Clear()
            End Try

            Return dt
        End Using
    End Function

#End Region

#End Region

#Region "登録関連"

#Region "RSS記事情報登録処理"
    ''' <summary>
    ''' RSS記事情報登録処理
    ''' </summary>
    ''' <param name="dt">RSS記事情報データテーブル</param>
    ''' <param name="siteno">サイトNo.</param>
    ''' <param name="updatedate">RSS最終更新日付</param>
    ''' <param name="targetfile">記事情報取得元ファイル名</param>
    ''' <returns>成功:True/失敗:False</returns>
    ''' <remarks></remarks>
    Private Function InsertRSSItemInfo(ByVal dt As MC3040204DataSet.MC3040204RssItemDataTable, ByVal siteno As Long, ByVal updatedate As Date, ByVal targetfile As String) As Boolean
        Using dtI As New MC3040204DataSet.MC3040204RssItemDataTable
            Dim rtnflg As Boolean = True

            For i As Integer = 0 To dt.Rows.Count - 1
                Dim dr As MC3040204DataSet.MC3040204RssItemRow = dt.Item(i)

                If Not dr.IsITEM_TITLENull _
                 AndAlso Not dr.IsITEM_URLNull _
                 AndAlso Not dr.IsITEM_CREATEDATENull _
                 AndAlso Not String.IsNullOrWhiteSpace(dr.ITEM_TITLE) _
                 AndAlso Not String.IsNullOrWhiteSpace(dr.ITEM_URL) Then

                    If (Date.Compare(dr.ITEM_CREATEDATE, updatedate) > 0) Then

                        dr.SITENO = siteno
                        dr.ACCOUNT = C_ACCOUNT
                        dtI.ImportRow(dr)

                        Dim insRssItemData As Integer = MC3040204TableAdapter.InsertRssItemData(dtI)

                        dtI.Rows(0).Delete()
                        Dim msg_Info1 As String = String.Format(CultureInfo.InvariantCulture, message001, C_TBL_RSS_ITEMDATA, C_INSERT, insRssItemData)
                        Logger.Info(msg_Info1)

                    Else
                        Dim msg_Info6 As String = String.Format(CultureInfo.InvariantCulture, message906, dr.ITEM_CREATEDATE, updatedate, dr.ITEM_TITLE, dr.ITEM_URL)
                        Logger.Info(msg_Info6)
                    End If
                Else

                    rtnflg = False
                    Dim msg_warn2 As String = String.Format(CultureInfo.InvariantCulture, message903, C_TBL_RSS_ITEMDATA, C_INSERT, targetfile, i + 1)
                    Logger.Warn(msg_warn2)
                    finishCode = C_WARN

                End If
            Next

            Return rtnflg
        End Using

    End Function

#End Region

#Region "RSSサイト情報更新処理"
    ''' <summary>
    ''' RSSサイト情報更新処理
    ''' </summary>
    ''' <param name="siteno">サイトNo.</param>
    ''' <param name="maxdate">RSSサイト最新更新日付</param>
    ''' <returns>成功:True/失敗:False</returns>
    ''' <remarks></remarks>
    Private Function UpdateRSSSiteData(ByVal siteno As Long, ByVal maxdate As Date) As Boolean
        Using dtU As New MC3040204DataSet.MC3040204RssSiteInfoDataTable
            Dim drU As MC3040204DataSet.MC3040204RssSiteInfoRow = dtU.NewMC3040204RssSiteInfoRow

            drU.SITENO = siteno
            drU.SITE_LASTUPDATE = maxdate
            drU.ACCOUNT = C_ACCOUNT

            dtU.Rows.Add(drU)

            Dim upRssSiteDate As Integer = MC3040204TableAdapter.UpDataRssSiteData(dtU)

            If upRssSiteDate <> 1 Then
                Dim msg_warn3 As String = String.Format(CultureInfo.InvariantCulture, message904, C_TBL_RSS_SITEDATA, C_UPDATE, siteno, maxdate)
                Logger.Warn(msg_warn3)
                finishCode = C_WARN
                Return False
            Else
                Dim msg_Info2 As String = String.Format(CultureInfo.InvariantCulture, message001, C_TBL_RSS_SITEDATA, C_UPDATE, upRssSiteDate)
                Logger.Info(msg_Info2)
                Return True
            End If
        End Using

    End Function

#End Region

#End Region

#Region "削除関連"

#Region "RSS記事情報退避処理"
    ''' <summary>
    ''' RSS記事情報退避処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub MoveRssItemData()

        'RSS記事情報の退避期限を取得
        Dim keepdate As Double = CDbl(BatchSetting.GetValue("TERMINFO", "ITEMKEEP", ""))
        Dim today As Date = Date.Today
        Dim targetdate As Date = today.AddDays(-keepdate)

        Using dtM As New MC3040204DataSet.MC3040204RssItemDataTable
            Dim drM As MC3040204DataSet.MC3040204RssItemRow = dtM.NewMC3040204RssItemRow

            drM.ITEM_CREATEDATE = DateTime.Parse((targetdate & C_TIME), CultureInfo.InvariantCulture)
            drM.ACCOUNT = C_ACCOUNT

            dtM.Rows.Add(drM)

            Dim insRssItemPast As Integer = MC3040204TableAdapter.InsertRssItemPast(dtM)

            Dim msg_Info3 As String = String.Format(CultureInfo.InvariantCulture, message001, C_TBL_RSS_ITEMDATA_PAST, C_INSERT, insRssItemPast)
            Logger.Info(msg_Info3)

            If insRssItemPast > 0 Then
                Dim delRssItemData As Integer = MC3040204TableAdapter.DeleteRssItemData(dtM)
                Dim msg_Info4 As String = String.Format(CultureInfo.InvariantCulture, message001, C_TBL_RSS_ITEMDATA, C_DELETE, delRssItemData)
                Logger.Info(msg_Info4)
            End If
        End Using

    End Sub

#End Region

#Region "RSS記事情報(退避)削除処理"
    ''' <summary>
    ''' RSS記事情報(退避)削除処理
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DeleteRssItemPast()

        'RSS記事情報(退避)の削除期限を取得
        Dim keepdate_past As Double = CDbl(BatchSetting.GetValue("TERMINFO", "ITEMKEEP_PAST", ""))
        Dim today As Date = Date.Today
        Dim targetdate_past As Date = today.AddDays(-keepdate_past)

        Using dtD As New MC3040204DataSet.MC3040204RssItemDataTable
            Dim drD As MC3040204DataSet.MC3040204RssItemRow = dtD.NewMC3040204RssItemRow

            drD.ITEM_CREATEDATE = DateTime.Parse((targetdate_past & C_TIME), CultureInfo.InvariantCulture)

            dtD.Rows.Add(drD)

            Dim delRssItemPast As Integer = MC3040204TableAdapter.DeleteRssItemPast(dtD)

            Dim msg_Info5 As String = String.Format(CultureInfo.InvariantCulture, message001, C_TBL_RSS_ITEMDATA_PAST, C_DELETE, delRssItemPast)
            Logger.Info(msg_Info5)
        End Using
    End Sub

#End Region

#End Region

#Region "文字列操作処理"
    ''' <summary>
    ''' DB登録用カット処理
    ''' </summary>
    ''' <param name="val">対象文字列</param>
    ''' <param name="cnt">カット文字数</param>
    ''' <param name="filename">ファイル名</param>
    ''' <param name="itemname">タグ名</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function CutCharacter(ByVal val As String, ByVal cnt As Integer, ByVal filename As String, ByVal itemname As String) As String
        Dim rtn As String = String.Empty
        Dim itempart As String = Left(val, C_CUT_ITEMPART)

        If val.Length > cnt Then
            rtn = Left(val, cnt)

            Dim msg_warn4 As String = String.Format(CultureInfo.InvariantCulture, message902, cnt, Path.GetFileName(filename), itemname, itempart)
            Logger.Info(msg_warn4)
            finishCode = C_WARN
        Else
            rtn = val
        End If

        Return rtn

    End Function
#End Region

End Class
