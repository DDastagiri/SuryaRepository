Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Common.MainMenu.DataAccess
Imports Toyota.eCRB.Common.MainMenu.DataAccess.SC3010201DataSet
Imports System.Globalization
Imports System.Reflection

''' <summary> 
''' SC3010201(メインメニュー)
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3010201BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3010201BusinessLogic

#Region "定数"
    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SYSTEM As String = "SC3010201"
#End Region

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
    Public Function ReadMessageInfo() As SC3010201MessageDataTable Implements ISC3010201BusinessLogic.ReadMessageInfo

        Dim dt As SC3010201MessageDataTable
        'ログインユーザの情報を格納
        Dim context As StaffContext = StaffContext.Current

        '検索処理
        dt = SC3010201TableAdapter.GetMessageInfo(context.DlrCD, context.BrnCD)

        Dim nowDate As Date = DateTimeFunc.Now(context.DlrCD)

        '結果を編集
        For Each dr As SC3010201MessageRow In dt.Rows
            dr.CREATEDATE_DISP = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Notification, dr.CREATEDATE, nowDate, context.DlrCD)
        Next

        '処理結果返却
        Return dt
    End Function

    ''' <summary>
    ''' RSS情報を取得する。
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function ReadRssInfo() As SC3010201RssDataTable Implements ISC3010201BusinessLogic.ReadRssInfo

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

    ' 2012/01/23 TCS 相田 【SALES_1B】 START
    ''' <summary>
    ''' 連絡事項を削除する。
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function DeleteMessageInfo(ByVal messageNo As Long) As Boolean Implements ISC3010201BusinessLogic.DeleteMessageInfo

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        '連絡事項ロック取得
        SC3010201TableAdapter.GetMessageInfoLock(messageNo)
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        'ログインユーザの情報を格納
        Dim context As StaffContext = StaffContext.Current

        '更新処理
        SC3010201TableAdapter.UpdateMessageInfoDelFlg(messageNo, context.Account, C_SYSTEM)

        '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_End, Return:[{1}]",
                                  MethodBase.GetCurrentMethod.Name, "True"))
        ' ======================== ログ出力 終了 ========================
        '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

        '処理結果返却
        Return True
    End Function
    ' 2012/01/23 TCS 相田 【SALES_1B】 END
End Class
