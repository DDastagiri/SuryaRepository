'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240101DataSet.vb
'─────────────────────────────────────
'機能： タブレットSMBの工程管理メインのデータセット
'補足： 
'作成： 2013/06/05 TMEJ 張 タブレット版SMB機能開発(工程管理)
'更新： 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応
'更新： 2015/06/10 TMEJ 河原 問連「TR-V4-TMT-20150529-009」商品無しの場合でもストール表示 対応
'更新： 2017/10/04 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新： 2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット
'更新：
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Oracle.DataAccess.Client
Imports System.Globalization


Namespace SC3240101DataSetTableAdapters
    Public Class SC3240101DataAdapter
        Inherits Global.System.ComponentModel.Component
#Region "取得処理"
        ''' <summary>
        ''' 指定店舗の全ストールの情報を取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stallidList">ストールIDリスト</param>
        ''' <returns>全ストールの情報を格納したDataTable</returns>
        ''' 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''' Public Function GetAllStall(ByVal dealerCode As String, ByVal branchCode As String) As SC3240101DataSet.SC3240101AllStallDataTable
        Public Function GetAllStall(ByVal dealerCode As String, _
                                    ByVal branchCode As String, _
                                    Optional ByVal stallidList As List(Of Decimal) = Nothing) As SC3240101DataSet.SC3240101AllStallDataTable
            Dim stallids As String = ""

            If Not (IsNothing(stallidList) OrElse stallidList.Count = 0) Then
                'サービス入庫IDを「svcinid1,svcinid2,…svcinidN」のstringに変更する
                Dim sbStallid As New StringBuilder
                For Each stallid As String In stallidList
                    sbStallid.Append(stallid)
                    sbStallid.Append(",")
                Next
                stallids = sbStallid.ToString()
                '最後のコンマを削除する
                stallids = stallids.Substring(0, stallids.Length - 1)
            End If
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("	SELECT /* SC3240101_001 */ ")
                .AppendLine("	       S.STALL_ID STALLID ")
                .AppendLine("	     , S.STALL_NAME STALLNAME ")
                .AppendLine("	     , S.STALL_NAME_SHORT STALLNAME_S ")
                .AppendLine("	  FROM ")
                .AppendLine("		   TB_M_STALL S ")
                .AppendLine("	 WHERE ")
                .AppendLine("	       DLR_CD = :DLRCD ")
                .AppendLine("	   AND BRN_CD = :STRCD ")
                .AppendLine("	   AND INUSE_FLG = N'1' ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                If Not String.IsNullOrEmpty(stallids.Trim()) Then
                    .AppendLine("	   AND STALL_ID IN ( ")
                    .AppendLine(stallids)
                    .AppendLine("	                   ) ")
                End If
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                '2015/06/10 TMEJ 河原 問連「TR-V4-TMT-20150529-009」商品無しの場合でもストール表示 対応 START

                '商品が紐づかないストールも表示されるように対応する為、以下コメントアウト

                '.AppendLine("	   AND EXISTS ( ")
                '.AppendLine("		              SELECT 1 ")
                '.AppendLine("		                FROM TB_M_SERVICE_CLASS T1 ")
                '.AppendLine("		               WHERE S.SVC_CLASS_ID = T1.SVC_CLASS_ID ")
                '.AppendLine("		                 AND INUSE_FLG = N'1' ")
                '.AppendLine("	              ) ")
                '.AppendLine("	   AND EXISTS ( ")
                '.AppendLine("	                  SELECT 1 ")
                '.AppendLine("	             	    FROM TB_M_BRANCH_SERVICE_CLASS T3 ")
                '.AppendLine("	             	   WHERE ")
                '.AppendLine("	              			 S.DLR_CD = T3.DLR_CD ")
                '.AppendLine("	              		 AND S.BRN_CD = T3.BRN_CD ")
                '.AppendLine("	              		 AND S.SVC_CLASS_ID = T3.SVC_CLASS_ID ")
                '.AppendLine("	              		 AND DLR_CD  = :DLRCD ")
                '.AppendLine("	              		 AND BRN_CD  = :STRCD ")
                '.AppendLine("	              ) ")

                '2015/06/10 TMEJ 河原 問連「TR-V4-TMT-20150529-009」商品無しの場合でもストール表示 対応 END

                .AppendLine(" ORDER BY ")
                .AppendLine("	       S.SORT_ORDER ")
                .AppendLine("        , S.STALL_ID ")
            End With

            Using query As New DBSelectQuery(Of SC3240101DataSet.SC3240101AllStallDataTable)("SC3240101_001")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ' ''' <summary>
        ' ''' 指定店舗の指定日の全ストールの情報と配属テクニシャン名を取得
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <param name="workDate">作業日付(yyyyMMdd)</param>
        ' ''' <returns></returns>
        'Public Function GetAllStallStaff(ByVal dealerCode As String, _
        '                                 ByVal branchCode As String, _
        '                                 ByVal workDate As String) As SC3240101DataSet.SC3240101StallStaffDataTable

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}, workDate={3}" _
        '                              , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, workDate))

        '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START

        ' ''' <summary>
        ' ''' 指定店舗の指定日の全ストールの情報と配属テクニシャン名を取得
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <returns></returns>
        'Public Function GetAllStallStaff(ByVal dealerCode As String, _
        '                                 ByVal branchCode As String) As SC3240101DataSet.SC3240101StallStaffDataTable

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}" _
        '                              , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode))

        ''' <summary>
        ''' 指定店舗の指定日の全ストールの情報と配属テクニシャン名を取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stfStallDispType">スタッフストール表示区分</param>
        ''' <returns></returns>
        Public Function GetAllStallStaff(ByVal dealerCode As String, _
                                         ByVal branchCode As String, _
                                         ByVal stfStallDispType As String) As SC3240101DataSet.SC3240101StallStaffDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}" _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode))
            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* SC3240101_002 */ ")
                .AppendLine("          T1.STALL_ID STALLID ")
                .AppendLine("        , T3.STALL_NAME_SHORT STALLNAME_S ")
                .AppendLine("        , T2.STF_NAME USERNAME ")
                .AppendLine("     FROM ")
                .AppendLine("          TB_M_STAFF_STALL T1 ")
                .AppendLine("        , TB_M_STAFF T2 ")
                .AppendLine("        , TB_M_STALL T3 ")
                .AppendLine("    WHERE ")
                .AppendLine("          T1.STF_CD = T2.STF_CD ")
                .AppendLine("      AND T3.STALL_ID = T1.STALL_ID ")
                '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
                '.AppendLine("      AND T2.BRN_OPERATOR_FLG = N'1' ")
                'ストール表示区分により、テクニシャン権限が違う
                If ("0").Equals(stfStallDispType) Then
                    '0:店舗M、店舗SA、店舗O
                    .AppendLine("    AND (  ")
                    .AppendLine("               T2.BRN_MANAGER_FLG = N'1' ")
                    .AppendLine("           OR  T2.BRN_OPERATOR_FLG = N'1' ")
                    .AppendLine("           OR  T2.BRN_SA_FLG = N'1' ")
                    .AppendLine("        )  ")
                ElseIf ("2").Equals(stfStallDispType) Then
                    '2:店舗SA、店舗O
                    .AppendLine("    AND (  ")
                    .AppendLine("               T2.BRN_OPERATOR_FLG = N'1' ")
                    .AppendLine("           OR  T2.BRN_SA_FLG = N'1' ")
                    .AppendLine("        )  ")
                ElseIf ("3").Equals(stfStallDispType) Then
                    '3:店舗M、店舗O
                    .AppendLine("    AND (  ")
                    .AppendLine("               T2.BRN_MANAGER_FLG = N'1' ")
                    .AppendLine("           OR  T2.BRN_OPERATOR_FLG = N'1' ")
                    .AppendLine("        )  ")
                Else
                    'ディフォルト(1):店舗O
                    .AppendLine("    AND T2.BRN_OPERATOR_FLG = N'1' ")
                End If
                '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END
                .AppendLine("      AND T2.INUSE_FLG = N'1' ")
                .AppendLine("      AND EXISTS ( ")
                .AppendLine("        	         SELECT 1 ")
                .AppendLine("                      FROM TB_M_ORGANIZATION T4 ")
                .AppendLine("                     WHERE T2.ORGNZ_ID = T4.ORGNZ_ID ")
                .AppendLine("                       AND DLR_CD = :DLRCD ")
                .AppendLine("                       AND BRN_CD = :STRCD ")
                .AppendLine("                       AND ORGNZ_SA_FLG = N'1' ")
                .AppendLine("                       AND INUSE_FLG  = N'1' ")
                .AppendLine("                 ) ")
                .AppendLine(" ORDER BY ")
                .AppendLine("         T3.SORT_ORDER ")
                .AppendLine("       , T3.STALL_ID ")
            End With

            Using query As New DBSelectQuery(Of SC3240101DataSet.SC3240101StallStaffDataTable)("SC3240101_002")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using

        End Function

        '2017/10/11 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        ''' <summary>
        ''' 洗車標準所要時間を取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        'Public Function GetStandardWashTime(ByVal dealerCode As String, ByVal branchCode As String) As Long

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}" _
        '                              , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode))

        '    Dim washTime As Long = 0

        '    Dim sql As New StringBuilder
        '    With sql
        '        .AppendLine(" SELECT /* SC3240101_003 */ ")
        '        .AppendLine("        STD_CARWASH_TIME COL1 ")
        '        .AppendLine("   FROM TB_M_SERVICEIN_SETTING  ")
        '        .AppendLine("  WHERE DLR_CD= :DLRCD ")
        '        .AppendLine("    AND BRN_CD= :STRCD ")
        '    End With

        '    Using query As New DBSelectQuery(Of SC3240101DataSet.SC3240101NumberValueDataTable)("SC3240101_003")
        '        query.CommandText = sql.ToString()

        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)

        '        Dim dt As DataTable = query.GetData()

        '        If dt.Rows.Count = 1 Then
        '            washTime = CType(dt.Rows(0).Item("COL1"), Long)
        '        Else
        '            Logger.Warn(String.Format(CultureInfo.InvariantCulture, "{0}. dealerCode={1}, branchCode={2}, Rows.Count={3}", _
        '                                      System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, dt.Rows.Count))
        '        End If
        '    End Using

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. washTime={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, washTime))
        '    Return washTime
        'End Function
        '2017/10/11 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

        ' 2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット START
        ' ''' <summary>
        ' ''' 納車作業工程標準作業時間(分)を取得
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        'Public Function GetStandardDeliveryTime(ByVal dealerCode As String, ByVal branchCode As String) As SC3240101DataSet.SC3240101StandardTimeDataTable

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}" _
        '                              , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode))

        '    Dim sql As New StringBuilder
        '    With sql
        '        .AppendLine(" SELECT /* SC3240101_004 */ ")
        '        .AppendLine("         DELIVERYPRE_STANDARD_LT ")
        '        .AppendLine("       , DELIVERYWR_STANDARD_LT ")
        '        .AppendLine("   FROM  TBL_SERVICEINI  ")
        '        .AppendLine("  WHERE  DLRCD= :DLRCD ")
        '        .AppendLine("    AND  STRCD= :STRCD ")
        '    End With

        '    Dim dt As SC3240101DataSet.SC3240101StandardTimeDataTable
        '    Using query As New DBSelectQuery(Of SC3240101DataSet.SC3240101StandardTimeDataTable)("SC3240101_004")
        '        query.CommandText = sql.ToString()

        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)

        '        dt = query.GetData()

        '    End Using

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E.", System.Reflection.MethodBase.GetCurrentMethod.Name))

        '    Return dt
        'End Function
        ' 2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット END

        ''' <summary>
        ''' 中断メモテンプレートを取得
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="brncd">店舗コード</param>
        ''' <returns>中断メモテンプレートテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetStopMemoTemplate(ByVal dlrcd As String, ByVal brncd As String) As SC3240101DataSet.SC3240101StringValueDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dlrcd={1}, brncd={2}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, dlrcd, brncd))

            '関連チップがある
            Dim sql As New StringBuilder
            With sql
                .AppendLine("    SELECT /* SC3240101_005 */ ")
                .AppendLine("           STOP_MEMO_TEMPLATE COL1 ")
                .AppendLine("      FROM ")
                .AppendLine("           TB_M_STOP_MEMO_TEMPLATE ")
                .AppendLine("     WHERE ")
                .AppendLine("           DLR_CD=:DLR_CD ")
                .AppendLine("       AND BRN_CD=:BRN_CD ")
                .AppendLine("  ORDER BY  ")
                .AppendLine("           SORT_ORDER ")
            End With

            Using query As New DBSelectQuery(Of SC3240101DataSet.SC3240101StringValueDataTable)("SC3240101_006")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brncd)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E.", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

        '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
        ' ''' <summary>
        ' ''' 指定店舗の全てテクニシャン名を取得する(ストールIDないのテクニシャンも)
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <returns></returns>
        'Public Function GetAllTechnicianByBrn(ByVal dealerCode As String, _
        '                                 ByVal branchCode As String) As SC3240101DataSet.SC3240101StallStaffDataTable

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}" _
        '                              , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode))

        ''' <summary>
        ''' 指定店舗の全てテクニシャン名を取得する(ストールIDないのテクニシャンも)
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stfStallDispType">スタッフストール表示区分</param>
        ''' <returns></returns>
        Public Function GetAllTechnicianByBrn(ByVal dealerCode As String, _
                                              ByVal branchCode As String, _
                                              ByVal stfStallDispType As String) As SC3240101DataSet.SC3240101StallStaffDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}, stfStallDispType={3}" _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode, stfStallDispType))

            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END


            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* SC3240101_006 */ ")
                .AppendLine("          T3.STALL_ID AS STALLID  ")
                .AppendLine("        , T4.STALL_NAME_SHORT AS STALLNAME_S  ")
                .AppendLine("        , T1.STF_NAME AS USERNAME ")
                .AppendLine("        , T1.STF_CD ")
                .AppendLine("        , T3.ROW_LOCK_VERSION ")
                .AppendLine("     FROM ")
                .AppendLine("          TB_M_STAFF T1 ")
                .AppendLine("        , TB_M_ORGANIZATION T2 ")
                .AppendLine("        , TB_M_STAFF_STALL T3 ")
                .AppendLine("        , TB_M_STALL T4 ")
                .AppendLine("    WHERE ")
                .AppendLine("          T1.ORGNZ_ID = T2.ORGNZ_ID ")
                .AppendLine("      AND T1.STF_CD = T3.STF_CD(+) ")
                .AppendLine("      AND T3.STALL_ID = T4.STALL_ID(+) ")
                .AppendLine("      AND T2.DLR_CD = :DLR_CD ")
                .AppendLine("      AND T2.BRN_CD = :BRN_CD ")
                .AppendLine("      AND T2.INUSE_FLG = N'1' ")
                .AppendLine("      AND T2.ORGNZ_SA_FLG = N'1' ")
                .AppendLine("      AND T1.INUSE_FLG = N'1' ")
                '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
                '.AppendLine("      AND T1.BRN_OPERATOR_FLG = N'1' ")

                'ストール表示区分により、テクニシャン権限が違う
                If ("0").Equals(stfStallDispType) Then
                    '0:店舗M、店舗SA、店舗O
                    .AppendLine("    AND (  ")
                    .AppendLine("               T1.BRN_MANAGER_FLG = N'1' ")
                    .AppendLine("           OR  T1.BRN_OPERATOR_FLG = N'1' ")
                    .AppendLine("           OR  T1.BRN_SA_FLG = N'1' ")
                    .AppendLine("        )  ")
                ElseIf ("2").Equals(stfStallDispType) Then
                    '2:店舗SA、店舗O
                    .AppendLine("    AND (  ")
                    .AppendLine("               T1.BRN_OPERATOR_FLG = N'1' ")
                    .AppendLine("           OR  T1.BRN_SA_FLG = N'1' ")
                    .AppendLine("        )  ")
                ElseIf ("3").Equals(stfStallDispType) Then
                    '3:店舗M、店舗O
                    .AppendLine("    AND (  ")
                    .AppendLine("               T1.BRN_MANAGER_FLG = N'1' ")
                    .AppendLine("           OR  T1.BRN_OPERATOR_FLG = N'1' ")
                    .AppendLine("        )  ")
                Else
                    'ディフォルト(1):店舗O
                    .AppendLine("    AND T1.BRN_OPERATOR_FLG = N'1' ")
                End If
                '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END
                .AppendLine("     ORDER BY ")
                .AppendLine("     T4.SORT_ORDER ")
                .AppendLine("   , USERNAME ")
            End With

            Using query As New DBSelectQuery(Of SC3240101DataSet.SC3240101StallStaffDataTable)("SC3240101_006")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function

        ''' <summary>
        ''' ストールスタッフテーブルからスタッフより、行ロックバージョンを取得する
        ''' </summary>
        ''' <param name="staffCodes">スタッフコード</param>
        ''' <returns></returns>
        Public Function GetAllStallStaffRowlockVersion(ByVal staffCodes As String) As SC3240101DataSet.SC3240101StallStaffDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. staffCodes={1}" _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name, staffCodes))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* SC3240101_007 */ ")
                .AppendLine("          STF_CD ")
                .AppendLine("        , ROW_LOCK_VERSION ")
                .AppendLine("     FROM ")
                .AppendLine("          TB_M_STAFF_STALL ")
                .AppendLine("    WHERE ")
                .AppendLine("          STF_CD IN ( ")
                .AppendLine(staffCodes)
                .AppendLine("                 ) ")
            End With

            Using query As New DBSelectQuery(Of SC3240101DataSet.SC3240101StallStaffDataTable)("SC3240101_007")
                query.CommandText = sql.ToString()
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ' ''' <summary>
        ' ''' 指定ストールのテクニシャン人数を取得する
        ' ''' </summary>
        ' ''' <param name="stallId">ストールID</param>
        ' ''' <returns></returns>
        'Public Function GetStaffCountByStallid(ByVal stallId As Decimal) As SC3240101DataSet.SC3240101NumberValueDataTable

        '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
        ' ''' <summary>
        ' ''' 指定ストールのテクニシャン人数を取得する
        ' ''' </summary>
        ' ''' <param name="stallId">ストールID</param>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetStaffCountByStallid(ByVal stallId As Decimal, _
        '                                       ByVal dealerCode As String, _
        '                                       ByVal branchCode As String) As SC3240101DataSet.SC3240101NumberValueDataTable

        ''' <summary>
        ''' 指定ストールのテクニシャン人数を取得する
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="stfStallDispType">スタッフストール表示区分</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetStaffCountByStallid(ByVal stallId As Decimal, _
                                               ByVal dealerCode As String, _
                                               ByVal branchCode As String, _
                                               ByVal stfStallDispType As String) As SC3240101DataSet.SC3240101NumberValueDataTable

            '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallId={1}" _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name, stallId))

            Dim sql As New StringBuilder
            With sql
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("   SELECT /* SC3240101_008 */ ")
                '.AppendLine("          COUNT(1) COL1")
                '.AppendLine("     FROM ")
                '.AppendLine("          TB_M_STAFF_STALL ")
                '.AppendLine("    WHERE ")
                '.AppendLine("          STALL_ID = :STALL_ID ")
                '.AppendLine("     AND  ROWNUM <= 5 ")
                .AppendLine(" SELECT /* SC3240101_008 */ ")
                .AppendLine("          COUNT(1) COL1")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_STAFF_STALL T1 ")
                .AppendLine("      , TB_M_STAFF T2 ")
                .AppendLine("  WHERE T1.STF_CD = T2.STF_CD ")
                .AppendLine("    AND T1.STALL_ID = :STALL_ID  ")
                '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 START
                '.AppendLine("    AND T2.BRN_OPERATOR_FLG = N'1' ")
                'ストール表示区分により、テクニシャン権限が違う
                If ("0").Equals(stfStallDispType) Then
                    '0:店舗M、店舗SA、店舗O
                    .AppendLine("    AND (  ")
                    .AppendLine("               T2.BRN_MANAGER_FLG = N'1' ")
                    .AppendLine("           OR  T2.BRN_OPERATOR_FLG = N'1' ")
                    .AppendLine("           OR  T2.BRN_SA_FLG = N'1' ")
                    .AppendLine("        )  ")
                ElseIf ("2").Equals(stfStallDispType) Then
                    '2:店舗SA、店舗O
                    .AppendLine("    AND (  ")
                    .AppendLine("               T2.BRN_OPERATOR_FLG = N'1' ")
                    .AppendLine("           OR  T2.BRN_SA_FLG = N'1' ")
                    .AppendLine("        )  ")
                ElseIf ("3").Equals(stfStallDispType) Then
                    '3:店舗M、店舗O
                    .AppendLine("    AND (  ")
                    .AppendLine("               T2.BRN_MANAGER_FLG = N'1' ")
                    .AppendLine("           OR  T2.BRN_OPERATOR_FLG = N'1' ")
                    .AppendLine("        )  ")
                Else
                    'ディフォルト(1):店舗O
                    .AppendLine("    AND T2.BRN_OPERATOR_FLG = N'1' ")
                End If
                '2014/07/02 TMEJ 張 BTS-283 「サービスマネージャをTCとして表示」対応 END
                .AppendLine("    AND T2.INUSE_FLG = N'1' ")
                .AppendLine("    AND EXISTS (  ")
                .AppendLine("        	      SELECT 1  ")
                .AppendLine("                   FROM TB_M_ORGANIZATION T4  ")
                .AppendLine("                  WHERE T2.ORGNZ_ID = T4.ORGNZ_ID  ")
                .AppendLine("                    AND DLR_CD = :DLRCD  ")
                .AppendLine("                    AND BRN_CD = :STRCD  ")
                .AppendLine("                    AND ORGNZ_SA_FLG = N'1'  ")
                .AppendLine("                    AND INUSE_FLG  = N'1'  ")
                .AppendLine("               )  ")
                .AppendLine("     AND  ROWNUM <= 5 ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            End With

            Using query As New DBSelectQuery(Of SC3240101DataSet.SC3240101NumberValueDataTable)("SC3240101_008")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)

                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return query.GetData()
            End Using
        End Function


        '2017/10/04 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
        ''' <summary>
        ''' 標準洗車時間、標準検査時間、標準納車時間を取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <returns></returns>
        ''' <history>
        ''' 2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット
        ''' </history>
        Public Function GetServiceSetting(ByVal dealerCode As String, ByVal branchCode As String) As SC3240101DataSet.SC3240101ServiceSettingDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. dealerCode={1}, branchCode={2}" _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name, dealerCode, branchCode))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" SELECT /* SC3240101_009 */ ")
                .AppendLine("        STD_CARWASH_TIME ")
                .AppendLine("      , STD_INSPECTION_TIME ")

                ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
                .AppendLine("      , STD_DELI_PREPARATION_TIME ")
                .AppendLine("      , STD_DELI_TIME ")
                ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

                .AppendLine("   FROM TB_M_SERVICEIN_SETTING  ")
                .AppendLine("  WHERE DLR_CD= :DLRCD ")
                .AppendLine("    AND BRN_CD= :STRCD ")
            End With
            Dim dt As SC3240101DataSet.SC3240101ServiceSettingDataTable
            Using query As New DBSelectQuery(Of SC3240101DataSet.SC3240101ServiceSettingDataTable)("SC3240101_009")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, branchCode)

                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E.", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Function
        '2017/10/04 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
#End Region

#Region "更新処理"
        ''' <summary>
        ''' スタッフストールテーブルのストールidを更新する
        ''' </summary>
        ''' <param name="stallId">ストールID</param>
        ''' <param name="staffCodes">スタッフコード</param>
        ''' <param name="updateDate">更新日時</param>
        ''' <param name="systemId">更新クラス</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Function UpdateStaffStallSetStallId(ByVal stallId As Decimal, _
                                                   ByVal staffCodes As String, _
                                                   ByVal updateDate As Date, _
                                                   ByVal updateAccount As String, _
                                                   ByVal systemId As String) As Long

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallId={1}, staffCodes={2}" _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name, stallId, staffCodes))

            'DBUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("SC3240101_201")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine(" UPDATE /* SC3240101_201 */ ")
                    .AppendLine("       TB_M_STAFF_STALL ")
                    .AppendLine("    SET ")
                    .AppendLine("       STALL_ID = :STALL_ID ")
                    .AppendLine("     , ROW_UPDATE_DATETIME = :UPDATE_DATETIME ")
                    .AppendLine("     , ROW_UPDATE_ACCOUNT = :UPDATE_STF_CD ")
                    .AppendLine("     , ROW_UPDATE_FUNCTION = :SYSTEMID ")
                    .AppendLine("     , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .AppendLine(" WHERE STF_CD IN ( ")
                    .AppendLine(staffCodes)
                    .AppendLine("                 ) ")
                End With
                query.CommandText = sql.ToString()

                'SQLパラメータ設定値
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, updateDate)
                query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, updateAccount)
                query.AddParameterWithTypeValue("SYSTEMID", OracleDbType.NVarchar2, systemId)

                'SQL実行(影響行数を返却)
                Dim queryCount As Long = query.Execute

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function
#End Region

#Region "挿入処理"
        ''' <summary>
        ''' スタッフストールテーブルに一行を挿入する
        ''' </summary>
        ''' <param name="stallId">ストールid</param>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <param name="updateDate">更新日時</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <param name="systemId">更新クラス</param>
        ''' <returns>1:正常終了、その他:更新失敗</returns>
        Public Function InsertTblStaffJob(ByVal stallId As Decimal, _
                                          ByVal staffCode As String, _
                                          ByVal updateDate As Date, _
                                          ByVal updateAccount As String, _
                                          ByVal systemId As String) As Integer

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S. stallId={1}, staffCode={2} ", _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, stallId, staffCode))

            Dim sql As New StringBuilder
            With sql
                .AppendLine(" INSERT /* SC3240101_301 */ ")
                .AppendLine("   INTO TB_M_STAFF_STALL (")
                .AppendLine("           STF_CD ")
                .AppendLine("         , STALL_ID ")
                .AppendLine("         , ROW_CREATE_DATETIME ")
                .AppendLine("         , ROW_UPDATE_DATETIME ")
                .AppendLine("         , ROW_CREATE_ACCOUNT ")
                .AppendLine("         , ROW_UPDATE_ACCOUNT ")
                .AppendLine("         , ROW_CREATE_FUNCTION ")
                .AppendLine("         , ROW_UPDATE_FUNCTION ")
                .AppendLine("         , ROW_LOCK_VERSION ) ")
                .AppendLine(" VALUES ( ")
                .AppendLine("           :STF_CD ")
                .AppendLine("         , :STALL_ID ")
                .AppendLine("         , :UPDATE_DATETIME ")
                .AppendLine("         , :UPDATE_DATETIME ")
                .AppendLine("         , :UPDATE_ACCOUNT ")
                .AppendLine("         , :UPDATE_ACCOUNT ")
                .AppendLine("         , :UPDATE_FUNCTION ")
                .AppendLine("         , :UPDATE_FUNCTION ")
                .AppendLine("         , 0 ) ")
            End With

            Using query As New DBUpdateQuery("SC3240101_301")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, staffCode)
                query.AddParameterWithTypeValue("STALL_ID", OracleDbType.Decimal, stallId)
                query.AddParameterWithTypeValue("UPDATE_DATETIME", OracleDbType.Date, updateDate)
                query.AddParameterWithTypeValue("UPDATE_ACCOUNT", OracleDbType.NVarchar2, updateAccount)
                query.AddParameterWithTypeValue("UPDATE_FUNCTION", OracleDbType.NVarchar2, systemId)

                'SQL実行(影響行数を返却)
                Dim queryCount As Integer = query.Execute
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E queryCount={1}", System.Reflection.MethodBase.GetCurrentMethod.Name, queryCount))
                Return queryCount
            End Using

        End Function
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
#End Region
    End Class

End Namespace