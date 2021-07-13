'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3140102BusinessLogic.vb
'─────────────────────────────────────
'機能：ダッシュボード ビジネスロジック
'補足：
'作成：2012/01/16 KN 小林
'更新：2014/02/13 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発対応
'更新：
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.SC3140102.SC3140102DataSet
Imports Toyota.eCRB.iCROP.DataAccess.SC3140102.SC3140102DataSetTableAdapters

'2014/02/13 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発対応 START
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801005
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801005
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801005.IC3801005DataSet
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801005.IC3801005TableAdapter
'2014/02/13 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発対応 END

''' <summary>
''' SC3140102
''' </summary>
''' <remarks></remarks>
Public Class SC3140102BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"

    ''' <summary>
    ''' Log開始用文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_START As String = "Start"

    ''' <summary>
    ''' Log終了文言
    ''' </summary>
    ''' <remarks></remarks>
    Private Const LOG_END As String = "End"

#End Region

#Region " 外部IF処理"

    '''-------------------------------------------------------
    ''' <summary>
    ''' 目標・進捗率情報取得
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <returns>目標・進捗率情報データセット</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/02/13 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発対応
    ''' </history>
    '''-------------------------------------------------------
    Public Function GetIfStaffInformation(ByVal staffInfo As StaffContext) As DataTable

        '2014/02/13 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発対応 START
        'Public Function GetIfStaffInformation(ByVal staffInfo As StaffContext) As IC3801005SAKPIDataTable
        '2014/02/13 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発対応 END

        '開始ログ
        Logger.Info(String.Format(System.Globalization.CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_START))

        ' パラメータ情報
        ' IN:
        '   DLRCD   :販売店コード
        '   STRCD   :店舗コード
        '   SACODE  :SAコード
        ' OUT:
        '   データ作成日時（前月）
        '   入庫計画台数（前月）
        '   入庫計画台数（定期点検（前月））
        '   入庫計画台数（一般整備（前月））
        '   入庫売上計画金額（前月）
        '   入庫売上計画金額（定期点検（前月））
        '   入庫売上計画金額（一般整備（前月））
        '   入庫実績台数（前月）
        '   入庫実績台数（定期点検（前月））
        '   入庫実績台数（一般整備（前月））
        '   入庫売上実績金額（前月）
        '   入庫売上実績金額（定期点検（前月））
        '   入庫売上実績金額（一般整備（前月））
        '   データ作成日時（当月）
        '   入庫計画台数（当月）
        '   入庫計画台数（当月累積）
        '   入庫計画台数（定期点検（当月累積））
        '   入庫計画台数（一般整備（当月累積））
        '   入庫売上計画金額（当月）
        '   入庫売上計画金額（当月累積）
        '   入庫売上計画金額（定期点検（当月累積））
        '   入庫売上計画金額（一般整備（当月累積））
        '   入庫実績台数（当月累積）
        '   入庫実績台数（定期点検（当月累積））
        '   入庫実績台数（一般整備（当月累積））
        '   入庫売上実績金額（当月累積）
        '   入庫売上実績金額（定期点検（当月累積））
        '   入庫売上実績金額（一般整備（当月累積））
        '   データ作成日時（当日）
        '   入庫計画台数（当日）
        '   入庫売上計画金額（当日）
        '   入庫実績台数（当日）
        '   入庫売上実績金額（当日）

        '2014/02/13 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発対応 START
        'Dim dt As IC3801005SAKPIDataTable
        'Dim bl As IC3801005BusinessLogic = New IC3801005BusinessLogic

        Dim dt As New DataTable
        '2014/02/13 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発対応 END

        '検索処理
        ' SAコードの調整-「@」より前のSAコード取得
        Dim renameSACode As String = Me.SetRenameSACode(staffInfo)

        'IF用ログ
        Logger.Info(String.Format(System.Globalization.CultureInfo.CurrentCulture _
                          , "CALL IF:IC3801005BusinessLogic.GetSAKPI IN:dlrCd={0} strCd={1} dealerCode={2}" _
                          , staffInfo.DlrCD _
                          , staffInfo.BrnCD _
                          , renameSACode))

        '2014/02/13 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発対応 START
        'dt = bl.GetSAKPI(staffInfo.DlrCD, staffInfo.BrnCD, renameSACode)
        '2014/02/13 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発対応 END

        Logger.Info(String.Format(System.Globalization.CultureInfo.CurrentCulture _
                                  , "CALL IF:IC3801005BusinessLogic.GetSAKPI OUT:Count = {0}" _
                                  , dt.Rows.Count))

        '終了ログ
        Logger.Info(String.Format(System.Globalization.CultureInfo.CurrentCulture _
                                , "{0}.{1} {2}" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , LOG_END))

        '処理結果返却
        Return dt
    End Function

    '''-------------------------------------------------------
    ''' <summary>
    ''' SAコードの調整-「@」より前のSAコード取得
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <returns>「@」より前のSAコード</returns>
    ''' <remarks></remarks>
    '''-------------------------------------------------------
    Private Function SetRenameSACode(ByVal staffInfo As StaffContext) As String

        ' IF用にSAコードの調整-「@」より前の文字列取得
        Dim splitString() As String
        Dim renameSACode As String = staffInfo.Account
        splitString = renameSACode.Split(CType("@", Char))
        renameSACode = splitString(0)

        '処理結果返却
        Return renameSACode
    End Function

#End Region

End Class
