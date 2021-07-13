'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3290102BusinessLogic.vb
'──────────────────────────────────
'機能： リマインダー
'補足： 
'作成： 2014/06/10 TMEJ t.nagata
'──────────────────────────────────
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SalesManager.IrregularControl.DataAccess

Imports System.Web.UI
Imports System.Web.HttpUtility
Imports System.Web.UI.WebControls
Imports Toyota.eCRB.SalesManager.IrregularControl.DataAccess.SC3290102DataSetTableAdapters

''' <summary>
''' SC3290102
''' リマインダー ビジネスロジッククラス
''' </summary>
''' <remarks></remarks>
Public Class SC3290102BusinessLogic
    Inherits BaseBusinessComponent

    ''' <summary>
    ''' フォロー一覧の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="getBeginLine">取得開始行番号</param>
    ''' <param name="getEndLine">取得終了行番号</param>
    ''' <returns>取得したフォロー一覧データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetIrregularFollowList(ByVal dealerCode As String, _
                                                ByVal brnCode As String, _
                                                ByVal staffCode As String, _
                                                ByVal getBeginLine As Integer, _
                                                ByVal getEndLine As Integer) As SC3290102DataSet.SC3290102FollowListDataTable
        Dim startLog As New StringBuilder
        With startLog
            .Append("GetIrregularFollowList_Start ")
            .Append("dealerCode[" & dealerCode & "]")
            .Append(",brnCode[" & brnCode & "]")
            .Append(",staffCode[" & staffCode & "]")
            .Append(",getBeginLine[" & getBeginLine & "]")
            .Append(",getEndLine[" & getEndLine & "]")
        End With

        Logger.Info(startLog.ToString)

        'フォロー一覧の取得データテーブル
        Dim irregularFollowListDataTable As SC3290102DataSet.SC3290102FollowListDataTable = Nothing

        Using dataAdapter As New SC3290102TableAdapter

            'フォロー一覧情報の取得
            irregularFollowListDataTable = dataAdapter.GetIrregularFollowList(dealerCode, brnCode,
                                                                staffCode, getBeginLine, getEndLine)
        End Using
        '結果返却
        Dim endLog As New StringBuilder
        With endLog
            .Append("GetIrregularFollowList_End Ret:[")
            .Append(IsNothing(irregularFollowListDataTable))
            .Append("] ")
        End With

        Logger.Info(endLog.ToString)

        Return irregularFollowListDataTable

    End Function

    ''' <summary>
    ''' フォロー一覧の項目数の取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <returns>フォロー一覧の項目数</returns>
    ''' <remarks></remarks>
    Public Function GetIrregularFollowListCount(ByVal dealerCode As String, _
                                                    ByVal brnCode As String, _
                                                    ByVal staffCode As String) As SC3290102DataSet.SC3290102FollowListCountDataTable
        Dim startLog As New StringBuilder
        With startLog
            .Append("GetIrregularFollowListCount_Start ")
            .Append("dealerCode[" & dealerCode & "]")
            .Append(",brnCode[" & brnCode & "]")
            .Append(",staffCode[" & staffCode & "]")
        End With

        Logger.Info(startLog.ToString)

        'フォロー一覧項目数の取得データテーブル
        Dim irregularFollowListCountDataTable _
                                As SC3290102DataSet.SC3290102FollowListCountDataTable = Nothing

        Using dataAdapter As New SC3290102TableAdapter

            'フォロー一覧項目数の取得
            irregularFollowListCountDataTable _
                            = dataAdapter.GetIrregularFollowListCount(dealerCode, brnCode, staffCode)
        End Using

            '結果返却
            Dim endLog As New StringBuilder
            With endLog
                .Append("GetIrregularFollowListCount_End Ret:[")
            .Append(irregularFollowListCountDataTable.Rows.Count)
                .Append("] ")
            End With

            Logger.Info(endLog.ToString)

            Return irregularFollowListCountDataTable

    End Function

End Class
