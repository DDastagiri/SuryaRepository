Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Common.MainMenu.DataAccess
Imports Toyota.eCRB.Common.MainMenu.DataAccess.SC3010201DataSet


''' <summary>
''' SC3010201(メインメニュー)
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3010201BusinessLogic
    Inherits BaseBusinessComponent

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' 連絡事項を取得する。
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Shared Function ReadMessageInfo() As SC3010201MessageDataTable

        Dim dt As SC3010201MessageDataTable
        'ログインユーザの情報を格納
        Dim context As StaffContext = StaffContext.Current

        '検索処理
        dt = SC3010201TableAdapter.GetMessageInfo(context.DlrCD, context.BrnCD)

        Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)

        '結果を編集
        For Each dr As SC3010201MessageRow In dt.Rows
            dr.CREATEDATE_DISP = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dr.CREATEDATE, nowDate, context.DlrCD)
        Next

        '処理結果返却
        Return dt
    End Function

    ''' <summary>
    ''' RSS情報を取得する。
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Shared Function ReadRssInfo() As SC3010201RssDataTable

        Dim dt As SC3010201RssDataTable
        'ログインユーザの情報を格納
        Dim context As StaffContext = StaffContext.Current

        '検索処理
        dt = SC3010201TableAdapter.GetRssInfo(context.DlrCD, context.BrnCD)

        '結果を編集
        For Each dr As SC3010201RssRow In dt.Rows
            '日付
            dr.ITEM_CREATEDATE_DAY = DateTimeFunc.FormatDate(11, dr.ITEM_CREATEDATE)
            '時間
            dr.ITEM_CREATEDATE_TIME = DateTimeFunc.FormatDate(14, dr.ITEM_CREATEDATE)
        Next

        '処理結果返却
        Return dt
    End Function

End Class
