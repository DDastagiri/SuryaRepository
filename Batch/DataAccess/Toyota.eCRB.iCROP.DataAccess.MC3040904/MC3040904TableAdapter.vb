'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━'
'MC3040904TableAdapter.vb                                                  '            '
'─────────────────────────────────────'
'機能： ステータス変更                                                   　'
'補足：                                                                    '
'作成： 2012/02/16 TCS 小林                                                '
'更新： 2013/02/27 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/06/30 TCS 山田 2013/10対応版 既存流用
'─────────────────────────────────────'

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports System.Reflection

Public NotInheritable Class MC3040904TableAdapter
    Inherits Global.System.ComponentModel.Component

#Region "定数"

    '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_SYSTEM = "MC3040904"

#End Region
    '2013/06/30 TCS 山田 2013/10対応版 既存流用 END

#Region "来店実績ステータス更新処理"
    ''' <summary>
    ''' 来店実績ステータス更新処理
    ''' </summary>
    ''' <returns>処理結果(件数)</returns>
    ''' <remarks>
    ''' 来店実績ステータスが商談中のデータを商談終了に更新する。
    ''' </remarks>
    Public Function UpdateVisitStatus() As Integer

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("MC3040904_001")

            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* MC3040904_001 */ ")
                .Append("        TBL_VISIT_SALES ")             'セールス来店実績テーブル
                .Append("    SET VISITSTATUS = '08' ")          '来店実績ステータス
                .Append("      , UPDATEDATE = SYSDATE ")        '更新日
                .Append("      , UPDATEACCOUNT = ' ' ")         '更新アカウント
                .Append("      , UPDATEID = 'MC3040904' ")      '更新機能ID
                .Append("WHERE VISITSTATUS = '07' ")
                .Append("  AND UPDATEDATE < TRUNC(SYSDATE) ")
            End With

            query.CommandText = sql.ToString()

            'SQL実行(影響行数を返却)
            Return query.Execute()

        End Using

    End Function
#End Region

    ' 2013/02/27 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
#Region "来店実績ステータス(納車作業中)更新処理"
    ''' <summary>
    ''' 来店実績ステータス(納車作業中)更新処理
    ''' </summary>
    ''' <returns>処理結果(件数)</returns>
    ''' <remarks>
    ''' 来店実績ステータスが納車作業中のデータを納車作業終了に更新する。
    ''' </remarks>
    Public Function UpdateVisitStatusDelivery() As Integer

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("MC3040904_003")

            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* MC3040904_003 */ ")
                .Append("        TBL_VISIT_SALES ")             'セールス来店実績テーブル
                .Append("    SET VISITSTATUS = '12' ")          '来店実績ステータス
                .Append("      , UPDATEDATE = SYSDATE ")        '更新日
                .Append("      , UPDATEACCOUNT = ' ' ")         '更新アカウント
                .Append("      , UPDATEID = 'MC3040904' ")      '更新機能ID
                .Append("WHERE VISITSTATUS = '11' ")
                .Append("  AND UPDATEDATE < TRUNC(SYSDATE) ")
            End With

            query.CommandText = sql.ToString()

            'SQL実行(影響行数を返却)
            Return query.Execute()

        End Using

    End Function
#End Region
    ' 2013/02/27 TCS 山田 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END

#Region "スタッフ在席状態ステータス更新処理"
    ''' <summary>
    ''' スタッフ在席状態ステータス更新処理
    ''' </summary>
    ''' <returns>処理結果(件数)</returns>
    ''' <remarks>
    ''' スタッフ在席状態ステータスがオフライン以外
    ''' のデータをオフラインに更新する。
    ''' </remarks>
    Public Function UpdatePresenceStatus() As Integer

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("MC3040904_002")

            ' SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" UPDATE /* MC3040904_002 */ ")
                .Append("        TBL_USERS ")                       'ユーザマスタ
                .Append("    SET PRESENCECATEGORY = '4' ")          '在席状態(大分類)
                .Append("      , PRESENCEDETAIL = '0' ")            '在席状態(小分類)
                .Append("      , PRESENCEUPDATEDATE = SYSDATE ")    '在席状態更新日
                .Append("      , UPDATEDATE = SYSDATE ")            '更新日
                .Append("      , UPDATEACCOUNT = ' ' ")             '更新ユーザアカウント
                .Append("WHERE PRESENCECATEGORY <> '4' ")
                .Append("   OR PRESENCEDETAIL <> '0' ")
            End With

            query.CommandText = sql.ToString()

            'SQL実行(影響行数を返却)
            Return query.Execute()

        End Using

    End Function
#End Region

    '2013/06/30 TCS 山田 2013/10対応版 既存流用 START
#Region "セールス来店実績ロック取得処理"
    ''' <summary>
    ''' セールス来店実績ロック取得
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GetVisitSalesLock()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of DataTable)("MC3040904_004")

            Dim sql As New StringBuilder

            With sql
                .Append("  SELECT /* MC3040904_004 */ ")
                .Append("         1 ")
                .Append("    FROM TBL_VISIT_SALES ")
                .Append(" WHERE ( VISITSTATUS = '07' ")
                .Append("   OR VISITSTATUS = '11' ) ")
                .Append("   AND UPDATEDATE < TRUNC(SYSDATE) ")
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

#Region "ユーザマスタロック取得処理"
    ''' <summary>
    ''' ユーザマスタロック取得
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GetUsersLock()

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  " {0}_Start",
                                  MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        Using query As New DBSelectQuery(Of DataTable)("MC3040904_005")

            Dim sql As New StringBuilder

            With sql
                .Append("   SELECT /* MC3040904_005 */ ")
                .Append("          1 ")
                .Append("     FROM TBL_USERS ")
                .Append("  WHERE PRESENCECATEGORY <> '4' ")
                .Append("     OR PRESENCEDETAIL <> '0' ")
                .Append("    FOR UPDATE ")
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
