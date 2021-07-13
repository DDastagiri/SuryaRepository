'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080401TableAdapter.vb
'─────────────────────────────────────
'機能： ヘルプ依頼画面 テーブルアダプタ
'補足： 
'作成： 2012/01/30 TCS 鈴木(健)
'更新： 2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更
'                               （仕変によりSALES_STEP2以降、BMを依頼先から除外）
'更新： 2012/03/13 TCS 鈴木(健) 【SALES_2】価格相談画面と依頼先の表示順が統一されていない不具合修正
'                               ※Sales Step1B ユーザーテスト 問題管理No.0074
'更新： 2013/06/30 TCS 吉村     【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/11/28 TCS 森        Aカード情報相互連携開発
'更新： 2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────

Imports System.Globalization
Imports System.Reflection
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

''' <summary>
''' ヘルプ依頼画面
''' テーブルアダプタークラス
''' </summary>
''' <remarks></remarks>
Public Class SC3080401TableAdapter

#Region "定数"
    ''' <summary>
    ''' 通知依頼情報.最終ステータス：依頼
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StatusRequest As String = "1"

    ''' <summary>
    ''' 依頼内容マスタ.依頼種別：ヘルプ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ReqClassHelp As String = "03"

    ''' <summary>
    ''' 店舗コード：H/O
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StrcdHO As String = "000"

    ' 2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更 START
    ' ''' <summary>
    ' ''' 操作権限コード：Branch Manager
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const OperationBM As Short = 6
    ' 2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更 END

    ''' <summary>
    ''' 操作権限コード：Sales Consultant Manager
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationSCM As Short = 7

    ''' <summary>
    ''' 削除フラグ：削除以外
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DelFlgValid As String = "0"

    ''' <summary>
    ''' 機能ID：ヘルプ依頼画面
    ''' </summary>
    ''' <remarks></remarks>
    Private Const prpFunctionId As String = "SC3080401"

    ' 2013/11/28 TCS 森 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 操作権限コード：Sales Staff
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OperationSC As Short = 8
    ' 2013/11/28 TCS 森 Aカード情報相互連携開発 END
#End Region

#Region "プロパティ"
    ''' <summary>
    ''' 機能ID：ヘルプ依頼画面
    ''' </summary>
    ''' <value></value>
    ''' <returns>機能ID：ヘルプ依頼画面</returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property FunctionId() As String
        Get
            Return prpFunctionId
        End Get
    End Property
#End Region

#Region "メソッド"

#Region "選択クエリ"
    '2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
    ''' <summary>
    ''' ヘルプ情報を取得します。
    ''' </summary>
    ''' <param name="dr">パラメータデータテーブル行</param>
    ''' <returns>ヘルプ情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetHelpInfo(ByVal dr As SC3080401DataSet.SC3080401ParameterRow) As SC3080401DataSet.SC3080401GetHelpInfoDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetHelpInfo_Start")
        'ログ出力 End *****************************************************************************

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080401_001 */ ")
            .Append("     HELP.HELPNO ")                                'ヘルプ情報.ヘルプNo
            .Append("   , HELP.TOACCOUNT ")                             'ヘルプ情報.送信先アカウント
            .Append("   , HELP.TOACCOUNTNAME ")                         'ヘルプ情報.送信先名
            .Append("   , HELP.ID ")                                    'ヘルプ情報.ID
            .Append("   , HMST.MSG_DLR ")                               '依頼内容マスタ.内容(現地語)
            .Append("   , HELP.CREATEDATE ")                            'ヘルプ情報.作成日（依頼日）
            .Append("   , HELP.NOTICEREQID ")                           'ヘルプ情報.通知依頼ID
            .Append("   , NREQ.STATUS ")                                '通知依頼情報.最終ステータス
            .Append("FROM ")
            .Append("    TBL_NOTICEHELPINFO HELP ")
            .Append("  , TBL_REQUESTINFOMST HMST ")
            .Append("  , TBL_NOTICEREQUEST NREQ ")
            .Append("WHERE ")
            .Append("    HMST.DLRCD = :DLRCD ")                         '依頼内容マスタ.販売店コード
            .Append("AND HMST.STRCD = :STRCD ")                         '依頼内容マスタ.店舗コード
            .Append("AND HMST.REQCLASS = :REQCLASS ")                   '依頼内容マスタ.依頼種別
            .Append("AND HELP.ID = HMST.ID ")                           'ヘルプ情報.ID = 依頼内容マスタ.ID
            .Append("AND HELP.NOTICEREQID = NREQ.NOTICEREQID ")         'ヘルプ情報.通知依頼ID = 通知依頼情報.通知依頼ID
            .Append("AND HELP.FLLWUPBOX_DLRCD = :FLLWUPBOX_DLRCD ")     'ヘルプ情報.Follow-up Box販売店コード
            .Append("AND HELP.FLLWUPBOX_STRCD = :FLLWUPBOX_STRCD ")     'ヘルプ情報.Follow-up Box店舗コード
            .Append("AND HELP.FLLWUPBOX_SEQNO = :FLLWUPBOX_SEQNO ")     'ヘルプ情報.Follow-up Box内連番
            .Append("AND NREQ.STATUS = :STATUS ")                       '通知依頼情報.最終ステータス
        End With

        ' DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of SC3080401DataSet.SC3080401GetHelpInfoDataTable)("SC3080401_001")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dr.DLRCD)                           '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, StrcdHO)                            '店舗コード（H/O）
            query.AddParameterWithTypeValue("REQCLASS", OracleDbType.Char, ReqClassHelp)                    '依頼種別
            query.AddParameterWithTypeValue("FLLWUPBOX_DLRCD", OracleDbType.Char, dr.FLLWUPBOX_DLRCD)       'Follow-up Box販売店コード
            query.AddParameterWithTypeValue("FLLWUPBOX_STRCD", OracleDbType.Char, dr.FLLWUPBOX_STRCD)       'Follow-up Box店舗コード
            query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, dr.FLLWUPBOX_SEQNO)      'Follow-up Box内連番
            query.AddParameterWithTypeValue("STATUS", OracleDbType.Char, StatusRequest)                     '最終ステータス

            ' SQLを実行
            Dim dt As SC3080401DataSet.SC3080401GetHelpInfoDataTable = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetHelpInfo_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 吉村 2013/10対応版　既存流用 END

            ' 結果を返却
            Return dt
        End Using

    End Function

    ''' <summary>
    ''' 依頼先情報を取得します。
    ''' </summary>
    ''' <param name="dr">パラメータデータテーブル行</param>
    ''' <returns>依頼先情報データテーブル</returns>
    ''' <remarks></remarks>
    ''' <History>
    '''  2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更
    '''                          （STEP2からの仕変によりBM不要化）
    '''  2012/03/13 TCS 鈴木(健) 【SALES_2】価格相談画面と依頼先の表示順が統一されていない不具合修正
    '''  2013/11/28 TCS 森        Aカード情報相互連携開発
    ''' </History>
    Public Function GetSendAccount(ByVal dr As SC3080401DataSet.SC3080401ParameterRow) As SC3080401DataSet.SC3080401GetSendAccountDataTable

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, IsNothing(dr):[{2}]",
                                  FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dr)))
        ' ======================== ログ出力 終了 ========================

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080401_002 */ ")
            .Append("     USR.PRESENCECATEGORY ")                           'ユーザーマスタ.在席状態（大分類）
            .Append("   , USR.PRESENCEDETAIL ")                             'ユーザーマスタ.在席状態（小分類）
            .Append("   , USR.OPERATIONCODE ")                              'ユーザーマスタ.操作権限コード
            .Append("   , USR.ACCOUNT ")                                    'ユーザーマスタ.アカウント
            .Append("   , USR.USERNAME ")                                   'ユーザーマスタ.スタッフネーム
            ' 2012/03/13 TCS 鈴木(健) 【SALES_2】価格相談画面と依頼先の表示順が統一されていない不具合修正 START
            .Append("   , CASE WHEN USR.PRESENCECATEGORY IN ( ")            '在籍状態がオンラインか否かを判定するフラグ
            .Append("                              :PRESENCECATEGORY_1 ")   'ユーザーマスタ.在席状態（大分類）：スタンバイ
            .Append("                            , :PRESENCECATEGORY_2 ")   'ユーザーマスタ.在席状態（大分類）：商談中
            .Append("                            , :PRESENCECATEGORY_3 ")   'ユーザーマスタ.在席状態（大分類）：退席中
            .Append("                                       ) ")
            .Append("          THEN 0 ELSE 1 ")
            .Append("     END AS ONLINESTATUS ")
            ' 2012/03/13 TCS 鈴木(健) 【SALES_2】価格相談画面と依頼先の表示順が統一されていない不具合修正 END
            .Append("   , DISP.SORTNO ")                                    'ユーザDISPLAY.表示順
            .Append("FROM ")
            .Append("    TBL_USERS USR ")
            .Append("  , TBL_USERDISPLAY DISP ")
            ' 2013/11/28 TCS 森        Aカード情報相互連携開発 START
            .Append("  , TB_M_STAFF STF")
            .Append("  , TB_M_ORGANIZATION ORGNZ ")
            ' 2013/11/28 TCS 森        Aカード情報相互連携開発 END
            .Append("WHERE ")
            .Append("    USR.ACCOUNT = DISP.ACCOUNT ")                      'ユーザーマスタ.アカウント = ユーザDISPLAY.アカウント
            ' 2013/11/28 TCS 森        Aカード情報相互連携開発 START
            .Append("AND RTRIM(USR.ACCOUNT) = STF.STF_CD ")                 'ユーザーマスタ.アカウント = スタッフマスタ.スタッフコード
            .Append("AND STF.ORGNZ_ID = ORGNZ.ORGNZ_ID(+) ")                'スタッフマスタ.組織ID = 組織マスタ.組織ID
            ' 2013/11/28 TCS 森        Aカード情報相互連携開発 END
            .Append("AND USR.DLRCD = :DLRCD ")                              'ユーザーマスタ.販売店コード
            .Append("AND RTRIM(USR.STRCD) = :STRCD ")                        'ユーザーマスタ.店舗コード
            ' 2013/11/28 TCS 森        Aカード情報相互連携開発 START
            .Append("AND (")
            ' 2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更 START
            '.Append("AND USR.OPERATIONCODE IN ( ")
            '.Append("                          :OPERATIONBM ")              'ユーザーマスタ.操作権限コード：BM
            '.Append("                        , :OPERATIONSCM ")             'ユーザーマスタ.操作権限コード：SCM
            '.Append("                     ) ")
            .Append(" USR.OPERATIONCODE = :OPERATIONSCM ")             'ユーザーマスタ.操作権限コード：SCM
            ' 2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更 END
            If dr.LEADERFLG = False Then
                .Append("    OR (")
                .Append("               STF.BRN_MANAGER_FLG = :BRN_MANAGER_FLG ")
                .Append("               AND STF.BRN_SC_FLG = :BRN_SC_FLG ")
                .Append("               AND ORGNZ.ORGNZ_SC_FLG = :ORGNZ_SC_FLG ")
                .Append("               AND ORGNZ.INUSE_FLG = :INUSE_FLG ")
                .Append("               AND USR.OPERATIONCODE = :OPERATIONSC")
                .Append("    )")
            End If
            .Append(")")
            ' 2013/11/28 TCS 森        Aカード情報相互連携開発 END
            .Append("AND USR.DELFLG = :DELFLG ")                            'ユーザーマスタ.削除フラグ：削除以外
            ' 2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更 START
            '.Append("ORDER BY USR.OPERATIONCODE DESC ")
            '.Append("       , DISP.SORTNO ")
            ' 2012/03/13 TCS 鈴木(健) 【SALES_2】価格相談画面と依頼先の表示順が統一されていない不具合修正 START
            '.Append("ORDER BY DISP.SORTNO ")
            .Append("ORDER BY ONLINESTATUS, DISP.SORTNO ")
            ' 2012/03/13 TCS 鈴木(健) 【SALES_2】価格相談画面と依頼先の表示順が統一されていない不具合修正 END
            ' 2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更 END
        End With

        ' DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of SC3080401DataSet.SC3080401GetSendAccountDataTable)("SC3080401_002")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            ' 2012/03/13 TCS 鈴木(健) 【SALES_2】価格相談画面と依頼先の表示順が統一されていない不具合修正 START
            query.AddParameterWithTypeValue("PRESENCECATEGORY_1", OracleDbType.Char, "1")                       '在籍状態（大分類）：スタンバイ
            query.AddParameterWithTypeValue("PRESENCECATEGORY_2", OracleDbType.Char, "2")                       '在籍状態（大分類）：商談中
            query.AddParameterWithTypeValue("PRESENCECATEGORY_3", OracleDbType.Char, "3")                       '在籍状態（大分類）：退席中
            ' 2012/03/13 TCS 鈴木(健) 【SALES_2】価格相談画面と依頼先の表示順が統一されていない不具合修正 END
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dr.DLRCD)                               '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, dr.STRCD)                               '店舗コード
            ' 2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更 START
            'query.AddParameterWithTypeValue("OPERATIONBM", OracleDbType.Int64, OperationBM)                     '操作権限コード：BM
            ' 2012/02/20 TCS 鈴木(健) 【SALES_1B】依頼先情報取得条件の変更 END
            query.AddParameterWithTypeValue("OPERATIONSCM", OracleDbType.Int64, OperationSCM)                   '操作権限コード：SCM
            ' 2013/11/28 TCS 森        Aカード情報相互連携開発 START
            If dr.LEADERFLG = False Then
                query.AddParameterWithTypeValue("BRN_MANAGER_FLG", OracleDbType.Char, "1")                          'スタッフマスタ.マネージャーフラグ
                query.AddParameterWithTypeValue("BRN_SC_FLG", OracleDbType.Char, "1")                               'スタッフマスタ.SCフラグ
                query.AddParameterWithTypeValue("ORGNZ_SC_FLG", OracleDbType.Char, "1")                             '組織マスタ.セールスフラグ
                query.AddParameterWithTypeValue("INUSE_FLG", OracleDbType.Char, "1")                                '組織マスタ.使用中フラグ
                query.AddParameterWithTypeValue("OPERATIONSC", OracleDbType.Int64, OperationSC)
            End If
            '操作権限コード:SC
            ' 2013/11/28 TCS 森        Aカード情報相互連携開発 END
            query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DelFlgValid)                           '削除フラグ：削除以外

            ' SQLを実行
            Dim dt As SC3080401DataSet.SC3080401GetSendAccountDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}.ascx {1}_End, GetSendAccount RowCount:[{2}]",
                                      FunctionId, MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            ' 結果を返却
            Return dt
        End Using

    End Function

    ''' <summary>
    ''' ヘルプマスタ情報を取得します。
    ''' </summary>
    ''' <param name="dr">パラメータデータテーブル行</param>
    ''' <returns>依頼内容マスタデータテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetHelpMst(ByVal dr As SC3080401DataSet.SC3080401ParameterRow) As SC3080401DataSet.SC3080401GetHelpMstDataTable


        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, IsNothing(dr):[{2}]",
                                  FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dr)))
        ' ======================== ログ出力 終了 ========================

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080401_003 */ ")
            .Append("     ID ")                                             'ID
            .Append("   , MSG_DLR ")                                        '内容(現地語)
            .Append("FROM ")
            .Append("    TBL_REQUESTINFOMST ")
            .Append("WHERE ")
            .Append("    DLRCD = :DLRCD ")                                  '販売店コード
            .Append("AND STRCD = :STRCD ")                            '店舗コード
            .Append("AND REQCLASS = :REQCLASS ")                            '依頼種別
            .Append("AND DELFLG = :DELFLG ")                                '削除フラグ
            .Append("ORDER BY SORTNO ")
        End With

        ' DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of SC3080401DataSet.SC3080401GetHelpMstDataTable)("SC3080401_003")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dr.DLRCD)                               '販売店コード
            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, StrcdHO)                                '店舗コード（H/O）
            query.AddParameterWithTypeValue("REQCLASS", OracleDbType.Char, ReqClassHelp)                        '依頼種別：ヘルプ
            query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DelFlgValid)                           '削除フラグ：削除以外

            ' SQLを実行
            Dim dt As SC3080401DataSet.SC3080401GetHelpMstDataTable = query.GetData()

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}.ascx {1}_End, GetHelpMst RowCount:[{2}]",
                                      FunctionId, MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
            ' ======================== ログ出力 終了 ========================

            ' 結果を返却
            Return dt
        End Using

    End Function

    ''' <summary>
    ''' ヘルプNoシーケンスからヘルプNoを取得します。
    ''' </summary>
    ''' <returns>ヘルプNo</returns>
    ''' <remarks>取得不可の場合は-1を返却します。</remarks>
    Public Function GetHelpNo() As Long

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start",
                                  FunctionId, MethodBase.GetCurrentMethod.Name))
        ' ======================== ログ出力 終了 ========================

        ' ヘルプNo
        Dim HelpNo As Long = -1

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080401_004 */ ")
            .Append("    SEQ_NOTICEHELPINFO_HELPNO.NEXTVAL AS HELPNO ")                 'ヘルプNoシーケンス
            .Append("FROM ")
            .Append("    DUAL ")
        End With

        ' DbSelectQueryインスタンス生成
        Using query As New DBSelectQuery(Of SC3080401DataSet.SC3080401GetHelpNoDataTable)("SC3080401_004")

            query.CommandText = sql.ToString()

            ' SQL実行
            Dim dt As SC3080401DataSet.SC3080401GetHelpNoDataTable = query.GetData()

            ' データが取得できた場合は、ヘルプNoを取得
            If dt.Rows.Count > 0 Then
                HelpNo = Convert.ToInt64(dt.Item(0).HELPNO)
            End If

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      "{0}.ascx {1}_End, HelpNo:[{2}]",
                                      FunctionId, MethodBase.GetCurrentMethod.Name, HelpNo.ToString(CultureInfo.InvariantCulture)))
            ' ======================== ログ出力 終了 ========================

            ' 結果を返却
            Return HelpNo
        End Using

    End Function
#End Region

#Region "挿入クエリ"
    '2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
    ''' <summary>
    ''' ヘルプ情報を挿入します。
    ''' </summary>
    ''' <param name="dr">パラメータデータテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function SetHelpInfo(ByVal dr As SC3080401DataSet.SC3080401ParameterRow) As Boolean

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetHelpInfo_Start")
        'ログ出力 End *****************************************************************************

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("INSERT /* SC3080401_005 */ ")
            .Append("INTO ")
            .Append("    TBL_NOTICEHELPINFO ")
            .Append("( ")
            .Append("    HELPNO ")                  'ヘルプNo
            .Append("  , ID ")                      'ID
            .Append("  , CRCUSTID ")                '活動先顧客コード
            .Append("  , CSTKIND ")                 '顧客種別
            .Append("  , CUSTOMERCLASS ")           '顧客分類
            .Append("  , FLLWUPBOX_DLRCD ")         'Follow-up Box販売店コード
            .Append("  , FLLWUPBOX_STRCD ")         'Follow-up Box店舗コード
            .Append("  , FLLWUPBOX_SEQNO ")         'Follow-up Box内連番
            .Append("  , TOACCOUNT ")               '送信先アカウント
            .Append("  , TOACCOUNTNAME ")           '送信先名
            .Append("  , NOTICEREQID ")             '通知依頼ID
            .Append("  , CREATEDATE ")              '作成日
            .Append("  , UPDATEDATE ")              '更新日
            .Append("  , CREATEACCOUNT ")           '作成ユーザアカウント
            .Append("  , UPDATEACCOUNT ")           '更新ユーザアカウント
            .Append("  , CREATEID ")                '作成機能ID
            .Append("  , UPDATEID ")                '更新機能ID
            .Append(") ")
            .Append("VALUES ")
            .Append("( ")
            .Append("    :HELPNO ")                  'ヘルプNo
            .Append("  , :ID ")                      'ID
            .Append("  , :CRCUSTID ")                '活動先顧客コード
            .Append("  , :CSTKIND ")                 '顧客種別
            .Append("  , :CUSTOMERCLASS ")           '顧客分類
            .Append("  , :FLLWUPBOX_DLRCD ")         'Follow-up Box販売店コード
            .Append("  , :FLLWUPBOX_STRCD ")         'Follow-up Box店舗コード
            .Append("  , :FLLWUPBOX_SEQNO ")         'Follow-up Box内連番
            .Append("  , :TOACCOUNT ")               '送信先アカウント
            .Append("  , :TOACCOUNTNAME ")           '送信先名
            .Append("  , :NOTICEREQID ")             '通知依頼ID
            .Append("  , SYSDATE ")                  '作成日
            .Append("  , SYSDATE ")                  '更新日
            .Append("  , :CREATEACCOUNT ")           '作成ユーザアカウント
            .Append("  , :UPDATEACCOUNT ")           '更新ユーザアカウント
            .Append("  , :CREATEID ")                '作成機能ID
            .Append("  , :UPDATEID ")                '更新機能ID
            .Append(") ")
        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3080401_005")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("HELPNO", OracleDbType.Long, dr.HELPNO)                         'ヘルプNo
            query.AddParameterWithTypeValue("ID", OracleDbType.Char, dr.ID)                                 'ID
            query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, dr.CRCUSTID)                     '活動先顧客コード
            query.AddParameterWithTypeValue("CSTKIND", OracleDbType.Char, dr.CSTKIND)                       '顧客種別
            query.AddParameterWithTypeValue("CUSTOMERCLASS", OracleDbType.Char, dr.CUSTOMERCLASS)           '顧客分類
            'Follow-up Box販売店コード
            If dr.IsFLLWUPBOX_DLRCDNull Then
                query.AddParameterWithTypeValue("FLLWUPBOX_DLRCD", OracleDbType.Char, DBNull.Value)
            Else
                query.AddParameterWithTypeValue("FLLWUPBOX_DLRCD", OracleDbType.Char, dr.FLLWUPBOX_DLRCD)
            End If
            'Follow-up Box店舗コード
            If dr.IsFLLWUPBOX_DLRCDNull Then
                query.AddParameterWithTypeValue("FLLWUPBOX_STRCD", OracleDbType.Char, DBNull.Value)
            Else
                query.AddParameterWithTypeValue("FLLWUPBOX_STRCD", OracleDbType.Char, dr.FLLWUPBOX_STRCD)
            End If
            'Follow-up Box内連番
            If dr.IsFLLWUPBOX_DLRCDNull Then
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, DBNull.Value)
            Else
                query.AddParameterWithTypeValue("FLLWUPBOX_SEQNO", OracleDbType.Decimal, dr.FLLWUPBOX_SEQNO)
            End If
            query.AddParameterWithTypeValue("TOACCOUNT", OracleDbType.Varchar2, dr.TOACCOUNT)               '送信先アカウント
            query.AddParameterWithTypeValue("TOACCOUNTNAME", OracleDbType.NVarchar2, dr.TOACCOUNTNAME)      '送信先名
            query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, DBNull.Value)                '通知依頼ID
            query.AddParameterWithTypeValue("CREATEACCOUNT", OracleDbType.Varchar2, dr.FROMACCOUNT)         '作成ユーザアカウント
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, dr.FROMACCOUNT)         '更新ユーザアカウント
            query.AddParameterWithTypeValue("CREATEID", OracleDbType.Varchar2, FunctionId)                  '作成機能ID
            query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, FunctionId)                  '更新機能ID

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetHelpInfo_End")
            'ログ出力 End *****************************************************************************
            ' SQL実行（結果を返却）
            If query.Execute() > 0 Then
                Return True
            Else
                Return False
            End If
            '2013/06/30 TCS 吉村 2013/10対応版　既存流用 END
        End Using
    End Function
#End Region

#Region "更新クエリ"
    ''' <summary>
    ''' ヘルプ情報を更新します。
    ''' </summary>
    ''' <param name="dr">パラメータデータテーブル行</param>
    ''' <returns>処理結果（成功[True]/失敗[False]）</returns>
    ''' <remarks></remarks>
    Public Function UpdateHelpInfo(ByVal dr As SC3080401DataSet.SC3080401ParameterRow) As Boolean

        ' ======================== ログ出力 開始 ========================
        Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                  "{0}.ascx {1}_Start, IsNothing(dr):[{2}]",
                                  FunctionId, MethodBase.GetCurrentMethod.Name, IsNothing(dr)))
        ' ======================== ログ出力 終了 ========================

        ' SQL組み立て
        Dim sql As New StringBuilder
        With sql
            .Append("UPDATE /* SC3080401_006 */ ")
            .Append("    TBL_NOTICEHELPINFO ")
            .Append("SET ")
            .Append("    NOTICEREQID = :NOTICEREQID ")              '通知依頼ID
            .Append("  , UPDATEDATE = SYSDATE ")                    '更新日
            .Append("WHERE ")
            .Append("    HELPNO = :HELPNO ")                        'ヘルプNo
        End With

        ' DbUpdateQueryインスタンス生成
        Using query As New DBUpdateQuery("SC3080401_006")

            query.CommandText = sql.ToString()

            ' SQLパラメータ設定
            query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, dr.NOTICEREQID)              '通知依頼ID
            query.AddParameterWithTypeValue("HELPNO", OracleDbType.Long, dr.HELPNO)                         'ヘルプNo

            ' SQL実行（結果を返却）
            If query.Execute() > 0 Then

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          "{0}.ascx {1}_End, IsSuccess:[{2}]",
                                          FunctionId, MethodBase.GetCurrentMethod.Name, True))
                ' ======================== ログ出力 終了 ========================

                Return True
            Else

                ' ======================== ログ出力 開始 ========================
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
                Logger.Error(String.Format(CultureInfo.InvariantCulture,
                                           "{0}.ascx {1}_End, IsSuccess:[{2}]",
                                          FunctionId, MethodBase.GetCurrentMethod.Name, False))
                '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END
                ' ======================== ログ出力 終了 ========================

                Return False
            End If
        End Using
    End Function
#End Region

#End Region

End Class
