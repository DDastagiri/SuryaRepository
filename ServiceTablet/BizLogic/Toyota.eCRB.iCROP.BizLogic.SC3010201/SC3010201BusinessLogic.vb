Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.Common.MainMenu.DataAccess
Imports Toyota.eCRB.Common.MainMenu.DataAccess.SC3010201DataSet
Imports System.Globalization


''' <summary> 
''' SC3010201(メインメニュー)
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class SC3010201BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3010201BusinessLogic

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
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

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

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '処理結果返却
        Return dt
    End Function

    ''' <summary>
    ''' RSS情報を取得する。
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function ReadRssInfo() As SC3010201RssDataTable Implements ISC3010201BusinessLogic.ReadRssInfo
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

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

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
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
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログインユーザの情報を格納
        Dim context As StaffContext = StaffContext.Current

        '更新処理
        SC3010201TableAdapter.UpdateMessageInfoDelFlg(messageNo, context.DlrCD, context.BrnCD)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
        '処理結果返却
        Return True
    End Function
    ' 2012/01/23 TCS 相田 【SALES_1B】 END
End Class
