'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3810301DataSet.vb
'─────────────────────────────────────
'機能： R/O連携データアクセス
'補足： 
'作成： 2012/01/26 KN 瀧
'更新： 2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時)
'更新： 2012/03/26 KN 佐藤 【SERVICE_1】サービス来店者管理追加時の項目追加(振当ステータス)
'更新： 2012/04/06 KN 瀧 【SERVICE_1】R/Oキャンセル時、整備受注Noでもキャンセルできるようにする
'更新： 2012/04/11 KN 佐藤 【SERVICE_1】R/Oキャンセル時、初回予約IDを予約IDで更新
'更新： 2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する
'更新： 2012/12/03 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72）
'更新： 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新： 2013/06/17 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発
'更新：
'─────────────────────────────────────

Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace IC3810301DataSetTableAdapters
    Public Class IC3810301DataTableAdapter
        Inherits Global.System.ComponentModel.Component

        ' 2012/03/26 KN 佐藤 【SERVICE_1】サービス来店者管理追加時の項目追加(振当ステータス) START
#Region "定数"

        ''' <summary>
        ''' 振当ステータス（SA振当済）
        ''' </summary>
        ''' <remarks></remarks>
        Public Const AssignFinished As String = "2"
        ''' <summary>
        ''' サービスステータス"03":キャンセル
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SVC_STATUS02 As String = "02"
        ''' <summary>
        ''' サービスステータス"03":着工指示待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SVC_STATUS03 As String = "03"
        ''' <summary>
        ''' ストール利用ステータス"00":着工指示待ち
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SU_STATUS00 As String = "00"
        ''' <summary>
        ''' キャンセルフラグ:"0"
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CANCEL_FLG_0 As String = "0"

        ''' <summary>
        ''' RO連番省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RO_JOB_SEQ_DEFAULT As String = "-1"

        ''' <summary>
        ''' 省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DEFAULT_VALUE As String = " "

        ''' <summary>
        ''' 画面ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const APPLICATION_ID As String = "IC3810301"

#End Region
        ' 2012/03/26 KN 佐藤 【SERVICE_1】サービス来店者管理追加時の項目追加(振当ステータス) END

        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

        '   ''' <summary>
        '   ''' サービス来店者管理テーブルの存在チェック
        '   ''' </summary>
        '   ''' <param name="rowIN">R/O画面仕掛中反映/R/Oキャンセル引数</param>
        '   ''' <returns>サービス来店者管理キー情報</returns>
        '   ''' <remarks></remarks>
        '   ''' 
        '   ''' <history>
        '   ''' 2012/04/06 KN 瀧 【SERVICE_1】R/Oキャンセル時、整備受注Noでもキャンセルできるようにする
        '   ''' </history>
        '   Public Function GetVisitKey(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow) As IC3810301DataSet.IC3810301VisitKeyDataTable
        '       Try
        '           ''引数をログに出力
        '           Dim args As New List(Of String)
        '           ' DataRow内の項目を列挙
        '           Me.AddLogData(args, rowIN)
        '           ''開始ログの出力
        '           Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} IN:{2}" _
        '               , Me.GetType.ToString _
        '               , MethodBase.GetCurrentMethod.Name _
        '               , String.Join(", ", args.ToArray())))

        '           Using query As New DBSelectQuery(Of IC3810301DataSet.IC3810301VisitKeyDataTable)("IC3810301_001")
        '               ''SQLの設定
        '               Dim sql As New StringBuilder
        '               sql.AppendLine("SELECT /* IC3810301_001 */")
        '               '2012/04/06 KN 瀧 【SERVICE_1】R/Oキャンセル時、整備受注Noでもキャンセルできるようにする START
        '               'sql.AppendLine("       SACODE")
        '               sql.AppendLine("       VISITSEQ")
        'sql.AppendLine("     , SACODE")
        '' 2012/07/05 西岡 事前準備対応 START
        'sql.AppendLine("     , ASSIGNSTATUS")
        '' 2012/07/05 西岡 事前準備対応 END
        '               '2012/04/06 KN 瀧 【SERVICE_1】R/Oキャンセル時、整備受注Noでもキャンセルできるようにする END
        '               sql.AppendLine("  FROM TBL_SERVICE_VISIT_MANAGEMENT")

        '               '2012/04/06 KN 瀧 【SERVICE_1】R/Oキャンセル時、整備受注Noでもキャンセルできるようにする START
        '               'sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
        '               If rowIN.IsVISITSEQNull = False _
        '                   AndAlso (rowIN.VISITSEQ > 0) Then
        '                   sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
        '                   query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
        '               Else
        '                   sql.AppendLine(" WHERE ORDERNO = :ORDERNO")
        '                   query.AddParameterWithTypeValue("ORDERNO", OracleDbType.NVarchar2, rowIN.ORDERNO)
        '               End If
        '               '2012/04/06 KN 瀧 【SERVICE_1】R/Oキャンセル時、整備受注Noでもキャンセルできるようにする END

        '               sql.AppendLine("   AND DLRCD = :DLRCD")
        '               sql.AppendLine("   AND STRCD = :STRCD")
        '               sql.AppendLine(" ORDER BY VISITSEQ DESC")

        '               query.CommandText = sql.ToString()
        '               ''パラメータの設定
        '               'query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
        '               query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, rowIN.DLRCD)
        '               query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, rowIN.STRCD)
        '               ''SQLの実行
        '               Using dt As IC3810301DataSet.IC3810301VisitKeyDataTable = query.GetData()
        '                   ''終了ログの出力
        '                   Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                       , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                       , Me.GetType.ToString _
        '                       , MethodBase.GetCurrentMethod.Name _
        '                       , dt.Rows.Count))
        '                   Return dt
        '               End Using
        '           End Using
        '       Finally

        '       End Try
        '   End Function

        '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する START
        ' ''' <summary>
        ' ''' R/O画面仕掛中反映(新規追加)
        ' ''' </summary>
        ' ''' <param name="rowIN">R/O画面仕掛中反映</param>
        ' ''' <returns>来店実績連番</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' 2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時)
        ' ''' 2012/06/11 KN 小澤【SERVICE_2事前準備】新規時はサービス来店管理TBLに新規レコードを追加しない
        ' ''' </history>
        'Public Function InsertVisitOrder(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow) As Long
        '    Try
        '        ''引数をログに出力
        '        Dim args As New List(Of String)
        '        ' DataRow内の項目を列挙
        '        Me.AddLogData(args, rowIN)
        '        ''開始ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} IN:{2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , String.Join(", ", args.ToArray())))

        '        Dim visitseq As Long = 0
        '        Using query As New DBSelectQuery(Of DataTable)("IC3810301_101")
        '            ''SQLの設定
        '            Dim sqlNextVal As New StringBuilder
        '            sqlNextVal.AppendLine("SELECT /* IC3810301_101 */")
        '            sqlNextVal.AppendLine("       SEQ_SERVICE_VISIT_MANAGEMENT.NEXTVAL AS VISITSEQ")
        '            sqlNextVal.AppendLine("  FROM DUAL")
        '            query.CommandText = sqlNextVal.ToString()
        '            Using dt As DataTable = query.GetData()
        '                visitseq = CType(dt.Rows(0)("VISITSEQ"), Long)
        '            End Using
        '        End Using
        '        ''SQLの設定
        '        Dim sqlInsert As New StringBuilder
        '        sqlInsert.AppendLine("INSERT /* IC3810301_102 */")
        '        sqlInsert.AppendLine("       INTO TBL_SERVICE_VISIT_MANAGEMENT (")
        '        sqlInsert.AppendLine("       VISITSEQ")
        '        sqlInsert.AppendLine("     , DLRCD")
        '        sqlInsert.AppendLine("     , STRCD")

        '        ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) START
        '        sqlInsert.AppendLine("     , VISITTIMESTAMP")
        '        ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) END

        '        sqlInsert.AppendLine("     , SACODE")

        '        ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) START
        '        sqlInsert.AppendLine("     , ASSIGNTIMESTAMP")
        '        ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) END

        '        ' 2012/03/26 KN 佐藤 【SERVICE_1】サービス来店者管理追加時の項目追加(振当ステータス) START
        '        sqlInsert.AppendLine("     , ASSIGNSTATUS")
        '        ' 2012/03/26 KN 佐藤 【SERVICE_1】サービス来店者管理追加時の項目追加(振当ステータス) END

        '        sqlInsert.AppendLine("     , ORDERNO")
        '        sqlInsert.AppendLine("     , REGISTKIND")
        '        sqlInsert.AppendLine("     , CREATEDATE")
        '        sqlInsert.AppendLine("     , UPDATEDATE")
        '        sqlInsert.AppendLine("     , CREATEACCOUNT")
        '        sqlInsert.AppendLine("     , UPDATEACCOUNT")
        '        sqlInsert.AppendLine("     , CREATEID")
        '        sqlInsert.AppendLine("     , UPDATEID")
        '        sqlInsert.AppendLine(")")
        '        sqlInsert.AppendLine("VALUES (")
        '        sqlInsert.AppendLine("       :VISITSEQ")
        '        sqlInsert.AppendLine("     , :DLRCD")
        '        sqlInsert.AppendLine("     , :STRCD")

        '        ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) START
        '        sqlInsert.AppendLine("     , :VISITTIMESTAMP")
        '        ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) END

        '        sqlInsert.AppendLine("     , :SACODE")

        '        ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) START
        '        sqlInsert.AppendLine("     , :ASSIGNTIMESTAMP")
        '        ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) END

        '        ' 2012/03/26 KN 佐藤 【SERVICE_1】サービス来店者管理追加時の項目追加(振当ステータス) START
        '        sqlInsert.AppendLine("     , :ASSIGNSTATUS")
        '        ' 2012/03/26 KN 佐藤 【SERVICE_1】サービス来店者管理追加時の項目追加(振当ステータス) END

        '        sqlInsert.AppendLine("     , :ORDERNO")
        '        sqlInsert.AppendLine("     , :REGISTKIND")
        '        sqlInsert.AppendLine("     , SYSDATE")
        '        sqlInsert.AppendLine("     , SYSDATE")
        '        sqlInsert.AppendLine("     , :CREATEACCOUNT")
        '        sqlInsert.AppendLine("     , :UPDATEACCOUNT")
        '        sqlInsert.AppendLine("     , :CREATEID")
        '        sqlInsert.AppendLine("     , :UPDATEID")
        '        sqlInsert.AppendLine(")")

        '        Using query As New DBUpdateQuery("IC3810301_102")

        '            query.CommandText = sqlInsert.ToString()

        '            ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) START
        '            ''システム日時の取得
        '            Dim sysDate As Date = Toyota.eCRB.SystemFrameworks.Core.DateTimeFunc.Now(rowIN.DLRCD)
        '            ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) END

        '            ''パラメータの設定
        '            ''来店実績連番
        '            query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, visitseq)
        '            ''販売店コード
        '            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '            ''店舗コード
        '            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
        '            ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) START
        '            ''来店日時
        '            query.AddParameterWithTypeValue("VISITTIMESTAMP", OracleDbType.Date, sysDate)
        '            ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) END
        '            ''振当SA
        '            query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, rowIN.SACODE)
        '            ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) START
        '            ''SA割当日時
        '            query.AddParameterWithTypeValue("ASSIGNTIMESTAMP", OracleDbType.Date, sysDate)
        '            ''2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時) END
        '            ' 2012/03/26 KN 佐藤 【SERVICE_1】サービス来店者管理追加時の項目追加(振当ステータス) START
        '            '振当ステータス
        '            query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.Char, AssignFinished)
        '            ' 2012/03/26 KN 佐藤 【SERVICE_1】サービス来店者管理追加時の項目追加(振当ステータス) END
        '            ''整備受注No
        '            If (rowIN.IsORDERNONull = True) Then
        '                query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, DBNull.Value)
        '            Else
        '                query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)
        '            End If
        '            ''登録区分 1:SAが登録
        '            query.AddParameterWithTypeValue("REGISTKIND", OracleDbType.Char, "1")
        '            ''作成日
        '            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
        '            ''更新日
        '            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
        '            ''作成機能ID
        '            query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, rowIN.SYSTEM)
        '            ''更新機能ID
        '            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, rowIN.SYSTEM)
        '            ''SQLの実行
        '            query.Execute()
        '        End Using
        '        ''終了ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:VISITSEQ = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , visitseq))
        '        Return visitseq
        '    Finally

        '    End Try
        'End Function
        '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する END

        ' ''' <summary>
        ' ''' R/O画面仕掛中反映(修正更新)
        ' ''' </summary>
        ' ''' <param name="rowIN">R/O画面仕掛中反映</param>
        ' ''' <returns>更新件数</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' </history>
        'Public Overloads Function UpdateVisitOrder(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow, _
        '                                           ByVal nowDate As Date) As Long
        '    Try
        '        ''引数をログに出力
        '        Dim args As New List(Of String)
        '        ' DataRow内の項目を列挙
        '        Me.AddLogData(args, rowIN)
        '        ''開始ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} IN:{2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , String.Join(", ", args.ToArray())))

        '        Using query As New DBUpdateQuery("IC3810301_103")
        '            ''SQLの設定
        '            Dim sql As New StringBuilder
        '            sql.AppendLine("UPDATE /* IC3810301_103 */")
        '            sql.AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
        '            sql.AppendLine("   SET ORDERNO = :ORDERNO")
        '            sql.AppendLine("     , UPDATEDATE = :UPDATE_DATETIME")
        '            sql.AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")
        '            sql.AppendLine("     , UPDATEID = :UPDATEID")
        '            sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
        '            sql.AppendLine("   AND DLRCD = :DLRCD")
        '            sql.AppendLine("   AND STRCD = :STRCD")
        '            ' 2012/07/05 西岡 事前準備対応 START
        '            'sql.AppendLine("   AND SACODE = :SACODE")
        '            sql.AppendLine("   AND (SACODE = :SACODE OR ASSIGNSTATUS <> :ASSIGNSTATUS)")
        '            ' 2012/07/05 西岡 事前準備対応 END
        '            query.CommandText = sql.ToString()

        '            ''パラメータの設定
        '            '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        '            'If (rowIN.IsORDERNONull = True) Then
        '            '    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, DBNull.Value)
        '            'Else
        '            '    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)
        '            'End If
        '            'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
        '            'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, rowIN.SYSTEM)
        '            'query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, nowDate)
        '            'query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
        '            'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '            'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
        '            'query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, rowIN.SACODE)
        '            '' 2012/07/05 西岡 事前準備対応 START
        '            'query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.Char, AssignFinished)
        '            '' 2012/07/05 西岡 事前準備対応 END

        '            If (rowIN.IsORDERNONull = True) Then
        '                query.AddParameterWithTypeValue("ORDERNO", OracleDbType.NVarchar2, DBNull.Value)
        '            Else
        '                query.AddParameterWithTypeValue("ORDERNO", OracleDbType.NVarchar2, rowIN.ORDERNO)
        '            End If
        '            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, rowIN.ACCOUNT)
        '            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, rowIN.SYSTEM)
        '            query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, nowDate)
        '            query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
        '            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, rowIN.DLRCD)
        '            query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, rowIN.STRCD)
        '            query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, rowIN.SACODE)
        '            query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, AssignFinished)
        '            '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '            ''SQLの実行
        '            Dim ret As Integer = query.Execute()
        '            ''終了ログの出力
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , MethodBase.GetCurrentMethod.Name _
        '                , ret))
        '            Return ret
        '        End Using
        '    Finally

        '    End Try
        'End Function

        ''2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する START
        ' ''' <summary>
        ' ''' サービス入庫R/O登録
        ' ''' </summary>
        ' ''' <param name="rowIN">R/O画面仕掛中反映引数</param>
        ' ''' <param name="nowDate">現在日時</param>
        ' ''' <returns>更新レコード件数</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' </history>
        'Public Function UpdateDBOrderReserveSave(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow, _
        '                                         ByVal nowDate As Date) As Integer
        '    ''引数をログに出力
        '    Dim args As New List(Of String)
        '    ' DataRow内の項目を列挙
        '    Me.AddLogData(args, rowIN)
        '    ''開始ログの出力
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '        , "{0}.{1} IN:{2}" _
        '        , Me.GetType.ToString _
        '        , MethodBase.GetCurrentMethod.Name _
        '        , String.Join(", ", args.ToArray())))

        '    'DBSelectQueryインスタンス生成
        '    Using query As New DBUpdateQuery("IC3810301_104")
        '        'SQL組み立て
        '        Dim sql As New StringBuilder
        '        With sql
        '            ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        '            '.Append("UPDATE /* IC3810301_104 */ ")
        '            '.Append("       TBL_STALLREZINFO T1")
        '            '.Append("   SET T1.ORDERNO = :ORDERNO")                '整備受注No
        '            ''2012/12/03 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
        '            '.Append("     , T1.ACCOUNT_PLAN = :ACCOUNT_PLAN")      '受付担当予定者
        '            ''2012/12/03 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END
        '            '.Append("     , T1.UPDATEACCOUNT = :UPDATEACCOUNT")    '更新ユーザーアカウント
        '            '.Append("     , T1.UPDATEDATE = :NOWDATE")             '更新日
        '            '.Append("     , T1.UPDATE_COUNT = UPDATE_COUNT + 1")   '更新カウント
        '            '.Append(" WHERE ")
        '            '.Append("       T1.DLRCD = :DLRCD")
        '            '.Append("   AND T1.STRCD = :STRCD")
        '            '.Append("   AND T1.REZID = :REZID")
        '            '.Append("   AND NOT EXISTS (")
        '            '.Append("       SELECT 1")
        '            '.Append("         FROM TBL_STALLREZINFO T2")
        '            '.Append("        WHERE T2.DLRCD = T1.DLRCD")
        '            '.Append("          AND T2.STRCD = T1.STRCD")
        '            '.Append("          AND T2.REZID = T1.REZID")
        '            '.Append("          AND T2.STOPFLG = '0'")
        '            '.Append("          AND T2.CANCELFLG = '1'")
        '            '.Append("       )")

        '            .Append("UPDATE /* IC3810301_104 */ ")
        '            .Append("       TB_T_SERVICEIN T1 ")
        '            .Append("   SET T1.RO_NUM = :RO_NUM ")
        '            .Append("     , T1.PIC_SA_STF_CD = :PIC_SA_STF_CD ")
        '            .Append(" WHERE T1.SVCIN_ID = :SVCIN_ID ")
        '            .Append("   AND NOT EXISTS ( ")
        '            .Append("                   SELECT ")
        '            .Append("                          1 ")
        '            .Append("                     FROM TB_T_SERVICEIN T2 ")
        '            .Append("                    WHERE T2.SVCIN_ID = T1.SVCIN_ID ")
        '            .Append("                      AND T2.SVC_STATUS = :SVC_STATUS02) ")
        '            ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
        '        End With
        '        query.CommandText = sql.ToString()
        '        'SQLパラメータ設定

        '        ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        '        'query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)
        '        ''2012/12/03 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） START
        '        'query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.Varchar2, rowIN.SACODE)
        '        ''2012/12/03 TMEJ 小澤 【SERVICE_2】SAストール予約受付機能開発（仕分けNo.72） END
        '        'query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, nowDate)
        '        'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
        '        'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '        'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
        '        'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, rowIN.REZID)

        '        query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, rowIN.ORDERNO)
        '        query.AddParameterWithTypeValue("PIC_SA_STF_CD", OracleDbType.NVarchar2, rowIN.SACODE)
        '        query.AddParameterWithTypeValue("SVC_STATUS02", OracleDbType.NVarchar2, SVC_STATUS02)
        '        query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Int64, rowIN.REZID)
        '        ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '        Dim ret As Integer = query.Execute()
        '        ''終了ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ret))

        '        Return ret
        '    End Using
        'End Function
        ''2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する END

        ' ''' <summary>
        ' ''' R/Oキャンセル
        ' ''' </summary>
        ' ''' <param name="rowIN">R/Oキャンセル引数</param>
        ' ''' <returns>削除件数</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' 2012/04/06 KN 瀧 【SERVICE_1】R/Oキャンセル時、整備受注Noでもキャンセルできるようにする
        ' ''' 2012/04/11 KN 佐藤 【SERVICE_1】R/Oキャンセル時、初回予約IDを予約IDで更新
        ' ''' </history>
        'Public Function DeleteVisitOrder(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow, _
        '                                 ByVal nowDate As Date) As Long
        '    Try
        '        ''引数をログに出力
        '        Dim args As New List(Of String)
        '        ' DataRow内の項目を列挙
        '        Me.AddLogData(args, rowIN)
        '        ''開始ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} IN:{2}" _
        '            , Me.GetType.ToString _
        '            , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '            , String.Join(", ", args.ToArray())))

        '        ''サービス来店管理の修正更新
        '        ''整理受注NoのNULLクリア
        '        Using query As New DBUpdateQuery("IC3810301_201")
        '            ''SQLの設定
        '            Dim sql As New StringBuilder
        '            sql.AppendLine("UPDATE /* IC3810301_201 */")
        '            sql.AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
        '            sql.AppendLine("   SET ORDERNO = NULL")
        '            ' 2012/04/11 KN 佐藤 【SERVICE_1】R/Oキャンセル時、初回予約IDを予約IDで更新 START
        '            sql.AppendLine("     , FREZID = REZID")
        '            ' 2012/04/11 KN 佐藤 【SERVICE_1】R/Oキャンセル時、初回予約IDを予約IDで更新 END
        '            sql.AppendLine("     , SACODE = DECODE(REGISTKIND, '1', NULL, SACODE)")
        '            sql.AppendLine("     , ASSIGNSTATUS = DECODE(REGISTKIND, '1', '4', ASSIGNSTATUS)")
        '            '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        '            'sql.AppendLine("     , UPDATEDATE = SYSDATE")
        '            sql.AppendLine("     , UPDATEDATE = :UPDATE_DATETIME")
        '            '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
        '            sql.AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")
        '            sql.AppendLine("     , UPDATEID = :UPDATEID")
        '            sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
        '            sql.AppendLine("   AND DLRCD = :DLRCD")
        '            sql.AppendLine("   AND STRCD = :STRCD")
        '            ' 2012/07/05 西岡 事前準備対応 START
        '            'sql.AppendLine("   AND SACODE = :SACODE")
        '            sql.AppendLine("   AND (SACODE = :SACODE OR ASSIGNSTATUS <> :ASSIGNSTATUS)")
        '            ' 2012/07/05 西岡 事前準備対応 END
        '            query.CommandText = sql.ToString()
        '            ''パラメータの設定
        '            '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        '            'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
        '            'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, rowIN.SYSTEM)
        '            'query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, nowDate)
        '            'query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
        '            'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '            'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
        '            'query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, rowIN.SACODE)
        '            '' 2012/07/05 西岡 事前準備対応 START
        '            'query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.Char, AssignFinished)
        '            '' 2012/07/05 西岡 事前準備対応 END

        '            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, rowIN.ACCOUNT)
        '            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, rowIN.SYSTEM)
        '            query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, nowDate)
        '            query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
        '            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, rowIN.DLRCD)
        '            query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, rowIN.STRCD)
        '            query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, rowIN.SACODE)
        '            query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, AssignFinished)
        '            '2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
        '            ''SQLの実行
        '            Dim ret As Integer = query.Execute()
        '            ''終了ログの出力
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , MethodBase.GetCurrentMethod.Name _
        '                , ret))
        '            Return ret
        '        End Using
        '    Finally

        '    End Try
        'End Function

        ''2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する START
        ' ''' <summary>
        ' ''' サービス入庫情報取得
        ' ''' </summary>
        ' ''' <param name="rowIN">R/O画面仕掛中反映</param>
        ' ''' <returns>更新対象の予約ID情報</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' </history>
        'Public Function GetStallReseveInfo(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow) As IC3810301DataSet.IC3810301StallReserveInfoDataTable
        '    Try
        '        ''引数をログに出力
        '        Dim args As New List(Of String)
        '        ' DataRow内の項目を列挙
        '        Me.AddLogData(args, rowIN)
        '        ''開始ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} IN:{2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , String.Join(", ", args.ToArray())))

        '        Using query As New DBSelectQuery(Of IC3810301DataSet.IC3810301StallReserveInfoDataTable)("IC3810301_002")
        '            ''SQLの設定
        '            Dim sql As New StringBuilder

        '            ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        '            'sql.Append("SELECT /* IC3810301_002 */ ")
        '            'sql.Append("       T1.REZID ")
        '            'sql.Append("  FROM TBL_STALLREZINFO T1 ")
        '            'sql.Append(" WHERE T1.DLRCD = :DLRCD ")
        '            'sql.Append("   AND T1.STRCD = :STRCD ")
        '            'sql.Append("   AND T1.ORDERNO = :ORDERNO ")
        '            'sql.Append("   AND NOT EXISTS ( ")
        '            'sql.Append("       SELECT 1 ")
        '            'sql.Append("         FROM TBL_STALLREZINFO T4 ")
        '            'sql.Append("        WHERE T4.DLRCD = T1.DLRCD ")
        '            'sql.Append("          AND T4.STRCD = T1.STRCD ")
        '            'sql.Append("          AND T4.REZID = T1.REZID ")
        '            'sql.Append("          AND T4.STOPFLG = '0' ")
        '            'sql.Append("          AND T4.CANCELFLG = '1' ")
        '            'sql.Append("       ) ")

        '            sql.Append("SELECT /* IC3810301_002 */ ")
        '            sql.Append("       T1.SVCIN_ID AS REZID  ")
        '            sql.Append("     , T1.SVC_STATUS AS SVC_STATUS ")
        '            sql.Append("     , T1.ROW_LOCK_VERSION AS ROW_LOCK_VERSION ")
        '            sql.Append("     , T3.STALL_USE_ID AS STALL_USE_ID ")
        '            sql.Append("     , T3.STALL_USE_STATUS AS STALL_USE_STATUS ")
        '            sql.Append("  FROM TB_T_SERVICEIN T1 ")
        '            sql.Append("     , TB_T_JOB_DTL T2  ")
        '            sql.Append("     , TB_T_STALL_USE T3  ")
        '            sql.Append(" WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
        '            sql.Append("   AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
        '            sql.Append("   AND T1.DLR_CD = :DLR_CD ")
        '            sql.Append("   AND T1.BRN_CD = :BRN_CD ")
        '            sql.Append("   AND T1.RO_NUM = :RO_NUM ")
        '            sql.Append("   AND T2.DLR_CD = :DLR_CD ")
        '            sql.Append("   AND T2.BRN_CD = :BRN_CD ")
        '            sql.Append("   AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
        '            ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '            query.CommandText = sql.ToString()

        '            'パラメータの設定
        '            ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        '            'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '            'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
        '            'query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)

        '            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, rowIN.DLRCD)
        '            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, rowIN.STRCD)
        '            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, rowIN.ORDERNO)
        '            query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_FLG_0)
        '            ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '            'SQLの実行
        '            Using dt As IC3810301DataSet.IC3810301StallReserveInfoDataTable = query.GetData()
        '                ''終了ログの出力
        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                    , Me.GetType.ToString _
        '                    , MethodBase.GetCurrentMethod.Name _
        '                    , dt.Rows.Count))
        '                Return dt
        '            End Using
        '        End Using
        '    Finally

        '    End Try
        'End Function

        ' ''' <summary>
        ' ''' サービス入庫R/O削除
        ' ''' </summary>
        ' ''' <param name="rowIN">削除情報</param>
        ' ''' <param name="nowDate">現在日時</param>
        ' ''' <returns>更新レコード件数</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' </history>
        'Public Function DeleteDBOrderReserveSave(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow, _
        '                                         ByVal nowDate As Date, _
        '                                         ByVal serviceStatus As String) As Integer
        '    ''引数をログに出力
        '    Dim args As New List(Of String)
        '    ' DataRow内の項目を列挙
        '    Me.AddLogData(args, rowIN)
        '    ''開始ログの出力
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '        , "{0}.{1} IN:{2}" _
        '        , Me.GetType.ToString _
        '        , MethodBase.GetCurrentMethod.Name _
        '        , String.Join(", ", args.ToArray())))

        '    'DBSelectQueryインスタンス生成
        '    Using query As New DBUpdateQuery("IC3810301_202")
        '        'SQL組み立て
        '        Dim sql As New StringBuilder
        '        With sql
        '            ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        '            '.Append("UPDATE /* IC3810301_202 */ ")
        '            '.Append("       TBL_STALLREZINFO T1")
        '            '.Append("   SET T1.ORDERNO = NULL")                    '整備受注No
        '            '.Append("     , T1.INSTRUCT = NULL")                   '着工指示区分
        '            '.Append("     , T1.WORKSEQ = NULL")                    '作業連番
        '            '.Append("     , T1.MERCHANDISEFLAG = NULL")            '部品準備完了フラグ
        '            '.Append("     , T1.UPDATEACCOUNT = :UPDATEACCOUNT")    '更新ユーザーアカウント
        '            '.Append("     , T1.UPDATEDATE = :NOWDATE")             '更新日
        '            '.Append("     , T1.UPDATE_COUNT = UPDATE_COUNT + 1")   '更新カウント
        '            '.Append(" WHERE ")
        '            '.Append("       T1.DLRCD = :DLRCD")
        '            '.Append("   AND T1.STRCD = :STRCD")
        '            '.Append("   AND T1.REZID = :REZID")

        '            .Append("UPDATE /* IC3810301_202 */ ")
        '            .Append("       TB_T_SERVICEIN ")
        '            .Append("   SET RO_NUM = :DEFAULT_VALUE ")
        '            '2013/11/08 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計(サービスステータス"03"削除対応) STAR
        '            'If serviceStatus.Equals("04") Then
        '            '    .Append("     , SVC_STATUS = :SVC_STATUS03 ")
        '            '    query.AddParameterWithTypeValue("SVC_STATUS03", OracleDbType.NVarchar2, SVC_STATUS03)
        '            'End If
        '            '2013/11/08 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 STAR
        '            .Append(" WHERE DLR_CD = :DLR_CD ")
        '            .Append("   AND BRN_CD = :BRN_CD ")
        '            .Append("   AND SVCIN_ID = :SVCIN_ID ")
        '            ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計(サービスステータス"03"削除対応) END

        '        End With
        '        query.CommandText = sql.ToString()
        '        'SQLパラメータ設定

        '        ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        '        'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
        '        'query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, nowDate)
        '        'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '        'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
        '        'query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, rowIN.REZID)

        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, rowIN.DLRCD)
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, rowIN.STRCD)
        '        query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Int64, rowIN.REZID)
        '        query.AddParameterWithTypeValue("DEFAULT_VALUE", OracleDbType.NVarchar2, DEFAULT_VALUE) '省略値
        '        ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '        Dim ret As Integer = query.Execute()
        '        ''終了ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ret))

        '        Return ret
        '    End Using
        'End Function
        ''2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する END

        '' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        ' ''' <summary>
        ' ''' 作業内容　作業連番削除
        ' ''' </summary>
        ' ''' <param name="rowIN"></param>
        ' ''' <param name="nowDate"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function DeleteDBWorkSeq(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow, _
        '                                ByVal nowDate As Date) As Integer
        '    ''引数をログに出力
        '    Dim args As New List(Of String)
        '    ' DataRow内の項目を列挙
        '    Me.AddLogData(args, rowIN)
        '    ''開始ログの出力
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '        , "{0}.{1} IN:{2}" _
        '        , Me.GetType.ToString _
        '        , MethodBase.GetCurrentMethod.Name _
        '        , String.Join(", ", args.ToArray())))

        '    'DBSelectQueryインスタンス生成
        '    Using query As New DBUpdateQuery("IC3810301_203")
        '        'SQL組み立て
        '        Dim sql As New StringBuilder
        '        With sql
        '            .Append("UPDATE /* IC3810301_203 */ ")
        '            .Append("       TB_T_JOB_DTL ")
        '            .Append("   SET RO_JOB_SEQ = :RO_JOB_SEQ ")
        '            .Append("     , UPDATE_DATETIME = :UPDATE_DATETIME ")
        '            .Append("     , UPDATE_STF_CD = :UPDATE_STF_CD ")
        '            .Append("     , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")
        '            .Append("     , ROW_UPDATE_ACCOUNT = :UPDATE_STF_CD ")
        '            .Append("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
        '            .Append("     , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
        '            .Append(" WHERE DLR_CD = :DLR_CD ")
        '            .Append("   AND BRN_CD = :BRN_CD ")
        '            .Append("   AND SVCIN_ID = :SVCIN_ID ")
        '            .Append("   AND CANCEL_FLG = :CANCEL_FLG_0 ")
        '        End With
        '        query.CommandText = sql.ToString()
        '        'SQLパラメータ設定

        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, rowIN.DLRCD)
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, rowIN.STRCD)
        '        query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, rowIN.ACCOUNT)
        '        query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, nowDate)
        '        query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Int64, rowIN.REZID)
        '        query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, APPLICATION_ID)
        '        query.AddParameterWithTypeValue("RO_JOB_SEQ", OracleDbType.NVarchar2, RO_JOB_SEQ_DEFAULT)
        '        query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CANCEL_FLG_0)

        '        Dim ret As Integer = query.Execute()
        '        ''終了ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ret))

        '        Return ret
        '    End Using
        'End Function

        ' ''' <summary>
        ' ''' ストール利用R/O削除
        ' ''' </summary>
        ' ''' <param name="rowIN"></param>
        ' ''' <param name="nowDate"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function DeleteDBOrderStallUse(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow, _
        '                                      ByVal nowDate As Date, _
        '                                      ByVal stallUseId As Long, _
        '                                      ByVal stallUseStatus As String) As Integer
        '    ''引数をログに出力
        '    Dim args As New List(Of String)
        '    ' DataRow内の項目を列挙
        '    Me.AddLogData(args, rowIN)
        '    ''開始ログの出力
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '        , "{0}.{1} IN:{2}" _
        '        , Me.GetType.ToString _
        '        , MethodBase.GetCurrentMethod.Name _
        '        , String.Join(", ", args.ToArray())))

        '    'DBSelectQueryインスタンス生成
        '    Using query As New DBUpdateQuery("IC3810301_204")
        '        'SQL組み立て
        '        Dim sql As New StringBuilder
        '        With sql
        '            .Append("UPDATE /* IC3810301_204 */ ")
        '            .Append("       TB_T_STALL_USE ")
        '            .Append("   SET PARTS_FLG = :DEFAULT_VALUE ")

        '            If stallUseStatus.Equals("01") Then
        '                .Append("     , STALL_USE_STATUS = :SU_STATUS00 ")
        '                query.AddParameterWithTypeValue("SU_STATUS00", OracleDbType.NVarchar2, SU_STATUS00)
        '            End If

        '            .Append("     , UPDATE_DATETIME = :UPDATE_DATETIME ")
        '            .Append("     , UPDATE_STF_CD = :UPDATE_STF_CD ")
        '            .Append("     , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")
        '            .Append("     , ROW_UPDATE_ACCOUNT = :UPDATE_STF_CD ")
        '            .Append("     , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
        '            .Append("     , ROW_LOCK_VERSION = (ROW_LOCK_VERSION + 1) ")
        '            .Append(" WHERE DLR_CD = :DLR_CD ")
        '            .Append("   AND BRN_CD = :BRN_CD ")
        '            .Append("   AND STALL_USE_ID = :STALL_USE_ID ")
        '        End With
        '        query.CommandText = sql.ToString()
        '        'SQLパラメータ設定

        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, rowIN.DLRCD)
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, rowIN.STRCD)
        '        query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, rowIN.ACCOUNT)
        '        query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, nowDate)
        '        query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Int64, stallUseId)
        '        query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, APPLICATION_ID)
        '        query.AddParameterWithTypeValue("DEFAULT_VALUE", OracleDbType.NVarchar2, DEFAULT_VALUE)

        '        Dim ret As Integer = query.Execute()
        '        ''終了ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ret))

        '        Return ret
        '    End Using
        'End Function

        ' ''' <summary>
        ' ''' サービス入庫行ロックヴァージョン取得
        ' ''' </summary>
        ' ''' <param name="rowIN"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetRowLockVersion(ByVal rowIN As IC3810301DataSet.IC3810301inOrderSaveRow) As IC3810301DataSet.IC3810301GetRowLockVersionDataTable
        '    Try
        '        ''引数をログに出力
        '        Dim args As New List(Of String)
        '        ' DataRow内の項目を列挙
        '        Me.AddLogData(args, rowIN)
        '        ''開始ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} IN:{2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , String.Join(", ", args.ToArray())))

        '        Using query As New DBSelectQuery(Of IC3810301DataSet.IC3810301GetRowLockVersionDataTable)("IC3810301_003")
        '            ''SQLの設定
        '            Dim sql As New StringBuilder

        '            sql.Append("SELECT /* IC3810301_002 */ ")
        '            sql.Append("       T1.ROW_LOCK_VERSION ")
        '            sql.Append("     , T1.SVCIN_ID ")
        '            sql.Append("  FROM TB_T_SERVICEIN T1 ")
        '            sql.Append("     , TB_T_JOB_DTL T2 ")
        '            sql.Append(" WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
        '            sql.Append("   AND T2.JOB_DTL_ID = :JOB_DTL_ID ")
        '            sql.Append("   AND T1.DLR_CD = :DLR_CD ")
        '            sql.Append("   AND T1.BRN_CD = :BRN_CD ")


        '            query.CommandText = sql.ToString()

        '            query.AddParameterWithTypeValue("JOB_DTL_ID", OracleDbType.NVarchar2, rowIN.REZID)
        '            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, rowIN.DLRCD)
        '            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, rowIN.STRCD)

        '            'SQLの実行
        '            Using dt As IC3810301DataSet.IC3810301GetRowLockVersionDataTable = query.GetData()
        '                ''終了ログの出力
        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                    , Me.GetType.ToString _
        '                    , MethodBase.GetCurrentMethod.Name _
        '                    , dt.Rows.Count))
        '                Return dt
        '            End Using
        '        End Using
        '    Finally

        '    End Try
        'End Function
        ' 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        ''' <summary>
        ''' RO情報登録処理
        ''' </summary>
        ''' <param name="rowIN">RO情報データロウ</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発
        ''' </history>
        Public Function InsertRepairOrderInfo(ByVal rowIN As IC3810301DataSet.IC3810301RepairOrderInfoRow) As Integer

            ''引数をログに出力
            Dim args As New List(Of String)
            ' DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))


            ''SQLの設定
            Dim sql As New StringBuilder
            sql.AppendLine(" INSERT /* IC3810301_001 */")
            sql.AppendLine("   INTO TB_T_RO_INFO( ")
            sql.AppendLine("        RO_RELATION_ID ")
            sql.AppendLine("      , RO_NUM ")
            sql.AppendLine("      , RO_SEQ ")
            sql.AppendLine("      , SVCIN_ID ")
            sql.AppendLine("      , VISIT_ID ")
            sql.AppendLine("      , DLR_CD ")
            sql.AppendLine("      , BRN_CD ")
            sql.AppendLine("      , RO_STATUS ")
            sql.AppendLine("      , RO_CREATE_STF_CD ")
            sql.AppendLine("      , RO_CREATE_DATETIME ")
            sql.AppendLine("      , RO_CHECK_STF_CD ")
            sql.AppendLine("      , RO_CHECK_DATETIME ")
            sql.AppendLine("      , RO_APPROVAL_DATETIME ")
            sql.AppendLine("      , ROW_CREATE_DATETIME ")
            sql.AppendLine("      , ROW_CREATE_ACCOUNT ")
            sql.AppendLine("      , ROW_CREATE_FUNCTION ")
            sql.AppendLine("      , ROW_UPDATE_DATETIME ")
            sql.AppendLine("      , ROW_UPDATE_ACCOUNT ")
            sql.AppendLine("      , ROW_UPDATE_FUNCTION ")
            sql.AppendLine("      , ROW_LOCK_VERSION ")
            sql.AppendLine("  ) ")
            sql.AppendLine("VALUES (  ")
            sql.AppendLine("        :RO_RELATION_ID ")
            sql.AppendLine("      , :RO_NUM ")
            sql.AppendLine("      , :RO_SEQ ")
            sql.AppendLine("      , :SVCIN_ID ")
            sql.AppendLine("      , :VISIT_ID ")
            sql.AppendLine("      , :DLR_CD ")
            sql.AppendLine("      , :BRN_CD ")
            sql.AppendLine("      , :RO_STATUS ")
            sql.AppendLine("      , :RO_CREATE_STF_CD ")
            sql.AppendLine("      , :RO_CREATE_DATETIME ")
            sql.AppendLine("      , :RO_CHECK_STF_CD ")
            sql.AppendLine("      , :RO_CHECK_DATETIME ")
            sql.AppendLine("      , :RO_APPROVAL_DATETIME ")
            sql.AppendLine("      , :ROW_CREATE_DATETIME ")
            sql.AppendLine("      , :ROW_CREATE_ACCOUNT ")
            sql.AppendLine("      , :ROW_CREATE_FUNCTION ")
            sql.AppendLine("      , :ROW_UPDATE_DATETIME ")
            sql.AppendLine("      , :ROW_UPDATE_ACCOUNT ")
            sql.AppendLine("      , :ROW_UPDATE_FUNCTION ")
            sql.AppendLine("      , :ROW_LOCK_VERSION ")
            sql.AppendLine(" )  ")

            ' DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3810301_001")
                query.CommandText = sql.ToString()

                ' バインド変数定義
                query.AddParameterWithTypeValue("RO_RELATION_ID", OracleDbType.Decimal, rowIN.RO_RELATION_ID)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, rowIN.RO_NUM)
                query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Int64, rowIN.RO_JOB_SEQ)
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, rowIN.SVCIN_ID)
                query.AddParameterWithTypeValue("VISIT_ID", OracleDbType.Int64, rowIN.VISIT_SEQ)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, rowIN.DLR_CD)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, rowIN.BRN_CD)
                query.AddParameterWithTypeValue("RO_STATUS", OracleDbType.NVarchar2, rowIN.RO_STATUS)
                query.AddParameterWithTypeValue("RO_CREATE_STF_CD", OracleDbType.NVarchar2, rowIN.RO_CREATE_STF_CD)
                query.AddParameterWithTypeValue("RO_CREATE_DATETIME", OracleDbType.Date, rowIN.RO_CREATE_DATETIME)
                query.AddParameterWithTypeValue("RO_CHECK_STF_CD", OracleDbType.NVarchar2, rowIN.RO_CREATE_STF_CD)
                query.AddParameterWithTypeValue("RO_CHECK_DATETIME", OracleDbType.Date, rowIN.RO_CHECK_DATETIME)
                query.AddParameterWithTypeValue("RO_APPROVAL_DATETIME", OracleDbType.Date, rowIN.RO_APPROVAL_DATETIME)
                query.AddParameterWithTypeValue("ROW_CREATE_DATETIME", OracleDbType.Date, rowIN.ROW_CREATE_DATETIME)
                query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, rowIN.ROW_CREATE_ACCOUNT)
                query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, rowIN.ROW_CREATE_FUNCTION)
                query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, rowIN.ROW_UPDATE_DATETIME)
                query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, rowIN.ROW_UPDATE_ACCOUNT)
                query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, rowIN.ROW_UPDATE_FUNCTION)
                query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Int64, rowIN.ROW_LOCK_VERSION)

                Dim ret As Integer = query.Execute()

                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , ret))

                Return ret

            End Using

        End Function
        ''' <summary>
        ''' 作業ID取得
        ''' </summary>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        ''' <history>
        '''  2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット 新DB適応に向けた機能設計 
        ''' </history>
        Public ReadOnly Property GetRepiarOrderRelationId() As IC3810301DataSet.IC3810301RoRelationIdDataTable

            Get
                ' DBSelectQueryインスタンス生成
                Using query As New DBSelectQuery(Of IC3810301DataSet.IC3810301RoRelationIdDataTable)("IC3810301_002")

                    Logger.Info("[S]GetSequenceJobId()")

                    Dim sql_1 As New StringBuilder

                    ' SQL文の作成
                    With sql_1
                        .Append(" SELECT /* IC3810301_002 */")
                        .Append(" SQ_RO_RELATION_ID.NEXTVAL AS RO_RELATION_ID FROM DUAL ")
                    End With

                    query.CommandText = sql_1.ToString()

                    Logger.Info("[E]GetSequenceJobId()")

                    'SQL実行
                    Return query.GetData()

                End Using
            End Get
        End Property
        '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

        ''' <summary>
        ''' DataRow内の項目を列挙(ログ出力用)
        ''' </summary>
        ''' <param name="args">ログ項目のコレクション</param>
        ''' <param name="row">対象となるDataRow</param>
        ''' <remarks></remarks>
        Private Sub AddLogData(ByVal args As List(Of String), ByVal row As DataRow)
            For Each column As DataColumn In row.Table.Columns
                If row.IsNull(column.ColumnName) = True Then
                    args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, row(column.ColumnName)))
                End If
            Next
        End Sub

    End Class

End Namespace
Partial Class IC3810301DataSet
End Class
