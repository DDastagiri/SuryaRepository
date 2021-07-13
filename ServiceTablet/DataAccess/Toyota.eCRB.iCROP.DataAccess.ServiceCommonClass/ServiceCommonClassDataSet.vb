'-------------------------------------------------------------------------
'Partial Class ServiceCommonClassDataSet.vb
'-------------------------------------------------------------------------
'機能：サービス共通関数API
'補足：
'作成：2014/01/16 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
'更新：2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発
'更新：2015/08/17 TMEJ 明瀬 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新：2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化
'更新：2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応
'更新：2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能
'更新：2019/05/21 NSK 坂本 18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策
'更新：
'─────────────────────────────────────

Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace ServiceCommonClassDataSetTableAdapters

    ''' <summary>
    ''' サービス共通関数APIデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ServiceCommonClassTableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            '処理なし
        End Sub

#Region "定数"

        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
        ''' <summary>
        ''' 使用中フラグ(使用中)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const InuseFlgOn As String = "1"

        ''' <summary>
        ''' 削除フラグ(削除以外)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DelFlgOff As String = "0"

        ''' <summary>
        ''' SA権限フラグ(権限有り)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OrgnzSaFlgOn As String = "1"

        ''' <summary>
        ''' 実績フラグ(中断実績チップを含まない)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ResultsFlgOff As Long = 0

        ''' <summary>
        ''' 実績フラグ(実績チップを含む全てのチップ)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ResultsFlgOn As Long = 1

        ''' <summary>
        ''' キャンセルフラグ(キャンセルチップを含まない)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CancelFlgOff As Long = 0

        ''' <summary>
        ''' キャンセルフラグ(キャンセルチップを含む)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CancelFlgOn As Long = 1

        ''' <summary>
        ''' 在席状態(大分類)：スタンバイ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PresenceCategoryStandby As String = "1"

        ''' <summary>
        ''' 在席状態(大分類)：商談中
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PresenceCategoryNegotiate As String = "2"

        ''' <summary>
        ''' 在席状態(大分類)：離席中
        ''' </summary>
        ''' <remarks></remarks>
        Private Const PresenceCategoryLeaving As String = "3"
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

        '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START

        ''' <summary>
        ''' ストール利用ステータス:作業中
        ''' </summary>
        ''' <remarks></remarks>
        Private Const stallUseStetus02 As String = "02"

        ''' <summary>
        ''' ストール利用ステータス:作業計画の一部の作業が中断
        ''' </summary>
        ''' <remarks></remarks>
        Private Const stallUseStetus04 As String = "04"

        '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END

#End Region

#Region "メソッド"

        ''' <summary>
        ''' ServiceCommonClass_001:i-CROP→DMSの値に変換された値を基幹コードマップテーブルから取得する
        ''' </summary>
        ''' <param name="allDealerCD">全販売店を意味するワイルドカード販売店コード</param>
        ''' <param name="dealerCD">販売店コード</param>
        ''' <param name="dmsCodeType">基幹コード区分</param>
        ''' <param name="icropCD1">iCROPコード1</param>
        ''' <param name="icropCD2">iCROPコード2</param>
        ''' <param name="icropCD3">iCROPコード3</param>
        ''' <returns>DmsCodeMapDataTable</returns>
        ''' <remarks></remarks>
        Public Function GetIcropToDmsCode(ByVal allDealerCD As String, _
                                          ByVal dealerCD As String, _
                                          ByVal dmsCodeType As Integer, _
                                          ByVal icropCD1 As String, _
                                          ByVal icropCD2 As String, _
                                          ByVal icropCD3 As String) As ServiceCommonClassDataSet.DmsCodeMapDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} ", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      allDealerCD, _
                                      dealerCD, _
                                      dmsCodeType, _
                                      icropCD1, _
                                      icropCD2, _
                                      icropCD3))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* ServiceCommonClass_001 */ ")
                .AppendLine(" 		     DMS_CD_1 CODE1 ")                '基幹コード1
                .AppendLine(" 		   , DMS_CD_2 CODE2 ")                '基幹コード2
                .AppendLine(" 		   , DMS_CD_3 CODE3 ")                '基幹コード3
                .AppendLine("     FROM ")
                .AppendLine(" 		     TB_M_DMS_CODE_MAP ")             '基幹コードマップ
                .AppendLine("    WHERE ")
                .AppendLine(" 		     DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
                .AppendLine(" 	   AND   DMS_CD_TYPE = :DMS_CD_TYPE ")
                .AppendLine(" 	   AND   ICROP_CD_1 = :ICROP_CD_1 ")

                If Not String.IsNullOrEmpty(icropCD2) Then
                    .AppendLine(" 	   AND   ICROP_CD_2 = :ICROP_CD_2 ")
                End If

                If Not String.IsNullOrEmpty(icropCD3) Then
                    .AppendLine(" 	   AND   ICROP_CD_3 = :ICROP_CD_3 ")
                End If

                .AppendLine(" ORDER BY DLR_CD ASC ")
            End With

            Dim dt As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

            Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.DmsCodeMapDataTable)("ServiceCommonClass_001")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCD)
                query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, allDealerCD)
                query.AddParameterWithTypeValue("DMS_CD_TYPE", OracleDbType.NVarchar2, dmsCodeType)
                query.AddParameterWithTypeValue("ICROP_CD_1", OracleDbType.NVarchar2, icropCD1)

                If Not String.IsNullOrEmpty(icropCD2) Then
                    query.AddParameterWithTypeValue("ICROP_CD_2", OracleDbType.NVarchar2, icropCD2)
                End If

                If Not String.IsNullOrEmpty(icropCD3) Then
                    query.AddParameterWithTypeValue("ICROP_CD_3", OracleDbType.NVarchar2, icropCD3)
                End If

                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dt.Count))

            Return dt

        End Function

        ''' <summary>
        ''' ServiceCommonClass_002:DMS→i-CROPの値に変換された値を基幹コードマップテーブルから取得する
        ''' </summary>
        ''' <param name="allDealerCD">全販売店を意味するワイルドカード販売店コード</param>
        ''' <param name="dealerCD">販売店コード</param>
        ''' <param name="dmsCodeType">基幹コード区分</param>
        ''' <param name="dmsCD1">DMSコード1</param>
        ''' <param name="dmsCD2">DMSコード2</param>
        ''' <param name="dmsCD3">DMSコード3</param>
        ''' <returns>DmsCodeMapDataTable</returns>
        ''' <remarks></remarks>
        Public Function GetDmsToIcropCode(ByVal allDealerCD As String, _
                                          ByVal dealerCD As String, _
                                          ByVal dmsCodeType As Integer, _
                                          ByVal dmsCD1 As String, _
                                          ByVal dmsCD2 As String, _
                                          ByVal dmsCD3 As String) As ServiceCommonClassDataSet.DmsCodeMapDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} ", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      allDealerCD, _
                                      dealerCD, _
                                      dmsCodeType, _
                                      dmsCD1, _
                                      dmsCD2, _
                                      dmsCD3))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* ServiceCommonClass_002 */ ")
                .AppendLine(" 		     ICROP_CD_1 CODE1 ")                'iCROPコード1
                .AppendLine(" 		   , ICROP_CD_2 CODE2 ")                'iCROPコード2
                .AppendLine(" 		   , ICROP_CD_3 CODE3 ")                'iCROPコード3
                .AppendLine("     FROM ")
                .AppendLine(" 		     TB_M_DMS_CODE_MAP ")               '基幹コードマップ
                .AppendLine("    WHERE ")
                .AppendLine(" 		     DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
                .AppendLine(" 	   AND   DMS_CD_TYPE = :DMS_CD_TYPE ")
                .AppendLine(" 	   AND   DMS_CD_1 = :DMS_CD_1 ")

                If Not String.IsNullOrEmpty(dmsCD2) Then
                    .AppendLine(" 	   AND   DMS_CD_2 = :DMS_CD_2 ")
                End If

                If Not String.IsNullOrEmpty(dmsCD3) Then
                    .AppendLine(" 	   AND   DMS_CD_3 = :DMS_CD_3 ")
                End If

                .AppendLine(" ORDER BY DLR_CD ASC ")
            End With

            Dim dt As ServiceCommonClassDataSet.DmsCodeMapDataTable = Nothing

            Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.DmsCodeMapDataTable)("ServiceCommonClass_002")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCD)
                query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, allDealerCD)
                query.AddParameterWithTypeValue("DMS_CD_TYPE", OracleDbType.NVarchar2, dmsCodeType)
                query.AddParameterWithTypeValue("DMS_CD_1", OracleDbType.NVarchar2, dmsCD1)

                If Not String.IsNullOrEmpty(dmsCD2) Then
                    query.AddParameterWithTypeValue("DMS_CD_2", OracleDbType.NVarchar2, dmsCD2)
                End If

                If Not String.IsNullOrEmpty(dmsCD3) Then
                    query.AddParameterWithTypeValue("DMS_CD_3", OracleDbType.NVarchar2, dmsCD3)
                End If

                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dt.Count))

            Return dt

        End Function

        '18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 START
        ' ''' <summary>
        ' ''' ServiceCommonClass_003:システム設定から設定値を取得する
        ' ''' </summary>
        ' ''' <param name="settingName">システム設定名</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetSystemSettingValue(ByVal settingName As String) As ServiceCommonClassDataSet.SystemSettingDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                              "{0}.{1} P1:{2} ", _
        '                              Me.GetType.ToString, _
        '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                              settingName))

        '    Dim sql As New StringBuilder
        '    With sql
        '        .AppendLine(" SELECT /* ServiceCommonClass_003 */ ")
        '        .AppendLine(" 		 SETTING_VAL ")
        '        .AppendLine("   FROM ")
        '        .AppendLine(" 		 TB_M_SYSTEM_SETTING ")
        '        .AppendLine("  WHERE ")
        '        .AppendLine(" 		 SETTING_NAME = :SETTING_NAME ")
        '    End With

        '    Dim dt As ServiceCommonClassDataSet.SystemSettingDataTable = Nothing

        '    Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.SystemSettingDataTable)("ServiceCommonClass_003")
        '        query.CommandText = sql.ToString()
        '        query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)

        '        dt = query.GetData()
        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                              "{0}.{1} QUERY:COUNT = {2}", _
        '                              Me.GetType.ToString, _
        '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                              dt.Count))

        '    Return dt

        'End Function

        ' ''' <summary>
        ' ''' ServiceCommonClass_004:販売店システム設定から設定値を取得する
        ' ''' </summary>
        ' ''' <param name="dealerCode">販売店コード</param>
        ' ''' <param name="branchCode">店舗コード</param>
        ' ''' <param name="allDealerCode">全店舗を示す販売店コード</param>
        ' ''' <param name="allBranchCode">全店舗を示す店舗コード</param>
        ' ''' <param name="settingName">販売店システム設定名</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetDlrSystemSettingValue(ByVal dealerCode As String, _
        '                                         ByVal branchCode As String, _
        '                                         ByVal allDealerCode As String, _
        '                                         ByVal allBranchCode As String, _
        '                                         ByVal settingName As String) As ServiceCommonClassDataSet.SystemSettingDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                              "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} ", _
        '                              Me.GetType.ToString, _
        '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                              dealerCode, _
        '                              branchCode, _
        '                              allDealerCode, _
        '                              allBranchCode, _
        '                              settingName))

        '    Dim sql As New StringBuilder
        '    With sql
        '        .AppendLine("   SELECT /* ServiceCommonClass_004 */ ")
        '        .AppendLine(" 		   SETTING_VAL ")
        '        .AppendLine("     FROM ")
        '        .AppendLine(" 		   TB_M_SYSTEM_SETTING_DLR ")
        '        .AppendLine("    WHERE ")
        '        .AppendLine(" 		   DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
        '        .AppendLine(" 	   AND BRN_CD IN (:BRN_CD, :ALL_BRN_CD) ")
        '        .AppendLine("      AND SETTING_NAME = :SETTING_NAME ")
        '        .AppendLine(" ORDER BY ")
        '        .AppendLine("          DLR_CD ASC, BRN_CD ASC ")
        '    End With

        '    Dim dt As ServiceCommonClassDataSet.SystemSettingDataTable = Nothing

        '    Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.SystemSettingDataTable)("ServiceCommonClass_004")
        '        query.CommandText = sql.ToString()
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
        '        query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, allDealerCode)
        '        query.AddParameterWithTypeValue("ALL_BRN_CD", OracleDbType.NVarchar2, allBranchCode)
        '        query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)

        '        dt = query.GetData()
        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                              "{0}.{1} QUERY:COUNT = {2}", _
        '                              Me.GetType.ToString, _
        '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                              dt.Count))

        '    Return dt

        'End Function
        '18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 END

        ''' <summary>
        ''' ServiceCommonClass_005:新文言テーブルから文言を取得する
        ''' </summary>
        ''' <param name="wordCode">文言コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetNewWordMasterInfo(ByVal wordCode As String) As ServiceCommonClassDataSet.WordMasterDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} P1:{2} ", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      wordCode))

            Dim sql As New StringBuilder
            With sql
                sql.AppendLine("SELECT /* ServiceCommonClass_005 */ ")
                sql.AppendLine("       WORD_TYPE ")
                sql.AppendLine("      ,NVL(TRIM(WORD_VAL), TRIM(WORD_VAL_ENG)) AS WORD  ")
                sql.AppendLine("  FROM ")
                sql.AppendLine("       TB_M_WORD ")
                sql.AppendLine(" WHERE ")
                sql.AppendLine("       WORD_CD = :WORD_CD ")
            End With

            Dim dt As ServiceCommonClassDataSet.WordMasterDataTable = Nothing

            Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.WordMasterDataTable)("ServiceCommonClass_005")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("WORD_CD", OracleDbType.NVarchar2, wordCode)

                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dt.Count))

            Return dt

        End Function

        '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 START

        ''' <summary>
        ''' ServiceCommonClass_006:サービス基幹連携除外エラーからレコード件数を取得する
        ''' </summary>
        ''' <param name="inInterfaceType">インターフェース区分(1:予約送信 / 2:ステータス送信 / 3:作業実績送信)</param>
        ''' <param name="inDmsResultCode">DMS結果コード(DMSから返却されたXML内の結果コード)</param>
        ''' <returns>レコード件数テーブル</returns>
        ''' <remarks></remarks>
        Public Function GetOmitDmsErrorCount(ByVal inInterfaceType As String, _
                                             ByVal inDmsResultCode As String) As ServiceCommonClassDataSet.RowCountDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} P1:{2} P2:{3}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inInterfaceType, _
                                      inDmsResultCode))

            Dim sql As New StringBuilder
            With sql
                sql.AppendLine(" SELECT /* ServiceCommonClass_006 */ ")
                sql.AppendLine("        COUNT(1) COUNT ")
                sql.AppendLine("   FROM ")
                sql.AppendLine("        TB_M_SVC_LINK_OMIT_ERROR ")
                sql.AppendLine("  WHERE ")
                sql.AppendLine("        INTERFACE_TYPE = :INTERFACE_TYPE ")
                sql.AppendLine("    AND OMIT_DMS_END_CD = :OMIT_DMS_END_CD ")
            End With

            Dim dt As ServiceCommonClassDataSet.RowCountDataTable = Nothing

            Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.RowCountDataTable)("ServiceCommonClass_006")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("INTERFACE_TYPE", OracleDbType.NVarchar2, inInterfaceType)
                query.AddParameterWithTypeValue("OMIT_DMS_END_CD", OracleDbType.NVarchar2, inDmsResultCode)

                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dt.Count))

            Return dt

        End Function

        ''' <summary>
        ''' ServiceCommonClass_007:サービスDMS納車実績ワークの情報を取得する
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <returns>サービスDMS納車実績ワークテーブル</returns>
        ''' <remarks></remarks>
        Public Function GetWorkServiceDmsResultDeliveryData(ByVal inServiceInId As Decimal) As ServiceCommonClassDataSet.WorkServiceDmsResultDeliveryDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} P1:{2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inServiceInId))

            Dim sql As New StringBuilder
            With sql
                sql.AppendLine("   SELECT /* ServiceCommonClass_007 */ ")
                sql.AppendLine("          SVCIN_ID ")
                sql.AppendLine("        , RSLT_DELI_DATETIME AS DMS_RSLT_DELI_DATETIME ")
                sql.AppendLine("     FROM ")
                sql.AppendLine("          TB_W_SVC_DMS_RSLT_DELI ")
                sql.AppendLine("    WHERE ")
                sql.AppendLine("          SVCIN_ID = :SVCIN_ID ")
            End With

            Dim dt As ServiceCommonClassDataSet.WorkServiceDmsResultDeliveryDataTable = Nothing

            Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.WorkServiceDmsResultDeliveryDataTable)("ServiceCommonClass_007")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)

                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dt.Count))

            Return dt

        End Function

        ''' <summary>
        ''' ServiceCommonClass_008:サービスDMS納車実績ワークからデータを削除する
        ''' </summary>
        ''' <param name="inServiceInId">サービス入庫ID</param>
        ''' <returns>削除レコード件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteWorkServiceDmsResultDeliveryData(ByVal inServiceInId As Decimal) As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} P1:{2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inServiceInId))

            Dim sql As New StringBuilder
            With sql
                sql.AppendLine("   DELETE /* ServiceCommonClass_008 */ ")
                sql.AppendLine("     FROM ")
                sql.AppendLine("          TB_W_SVC_DMS_RSLT_DELI ")
                sql.AppendLine("    WHERE ")
                sql.AppendLine("          SVCIN_ID = :SVCIN_ID ")
            End With

            Dim queryCount As Integer = 0

            Using query As New DBUpdateQuery("ServiceCommonClass_008")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inServiceInId)

                'SQL実行(影響行数を返却)
                queryCount = query.Execute

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      queryCount))

            Return queryCount

        End Function

        '2015/04/23 TMEJ 明瀬 DMS連携版サービスタブレット強制納車機能追加開発 END

        '2015/08/17 TMEJ 明瀬 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        ''' <summary>
        ''' ServiceCommonClass_009:顧客車両情報を取得する
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inCstId">顧客ID</param>
        ''' <param name="inVclId">車両ID</param>
        ''' <returns>顧客車両情報</returns>
        ''' <remarks></remarks>
        Public Function GetCustomerVehicleData(ByVal inDealerCode As String, _
                                               ByVal inCstId As Decimal, _
                                               ByVal inVclId As Decimal) As ServiceCommonClassDataSet.CustomerVehicleDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} P1:{2} P1:{3} P1:{4}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inDealerCode, _
                                      inCstId, _
                                      inVclId))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* ServiceCommonClass_009 */")
                .AppendLine("          A.CST_VCL_TYPE ")        '顧客車両区分
                .AppendLine("        , B.CST_TYPE ")            '顧客種別
                .AppendLine("        , C.CST_NAME ")            '顧客氏名
                .AppendLine("     FROM ")
                .AppendLine("          TB_M_CUSTOMER_VCL A ")   '「販売店顧客車両」テーブル
                .AppendLine("        , TB_M_CUSTOMER_DLR B ")   '「販売店顧客」テーブル
                .AppendLine("        , TB_M_CUSTOMER C ")       '「顧客」テーブル
                .AppendLine("    WHERE ")
                .AppendLine("          A.CST_ID = B.CST_ID ")
                .AppendLine("      AND A.DLR_CD = B.DLR_CD ")
                .AppendLine("      AND B.CST_ID = C.CST_ID ")
                .AppendLine("      AND A.DLR_CD = :DLR_CD ")
                .AppendLine("      AND A.CST_ID = :CST_ID")
                .AppendLine("      AND A.VCL_ID = :VCL_ID ")
                .AppendLine(" ORDER BY ")
                .AppendLine("          B.CST_TYPE, A.CST_VCL_TYPE ")
            End With

            Dim dt As ServiceCommonClassDataSet.CustomerVehicleDataTable = Nothing

            Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.CustomerVehicleDataTable)("ServiceCommonClass_009")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, inCstId)
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVclId)

                'SQL実行
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dt.Count))

            Return dt

        End Function

        '2015/08/17 TMEJ 明瀬 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START
        ''' <summary>
        ''' ServiceCommonClass_010:特定のDMSエラーコードから文言コードを取得する
        ''' </summary>
        ''' <param name="inTypeCD">区分種別コード</param>
        ''' <param name="inTypeVal">区分値</param>
        ''' <returns>文言コード</returns>
        ''' <remarks></remarks>
        Public Function ParticularDmsErrorCodeToWordCode(ByVal inTypeCD As String, _
                                                         ByVal inTypeVal As String) As ServiceCommonClassDataSet.WordRelationDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} P1:{2} P2:{3}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      inTypeCD, _
                                      inTypeVal))

            Dim sql As New StringBuilder
            With sql
                .AppendLine("   SELECT /* ServiceCommonClass_010 */")
                .AppendLine("          WORD_CD ") '文言コード
                .AppendLine("     FROM ")
                .AppendLine("          TB_M_WORD_RELATION ") '「文言紐付けマスタ」テーブル
                .AppendLine("    WHERE ")
                .AppendLine("          TYPE_CD  = :TYPE_CD ")
                .AppendLine("      AND TYPE_VAL = :TYPE_VAL ")
            End With

            Dim dt As ServiceCommonClassDataSet.WordRelationDataTable = Nothing

            Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.WordRelationDataTable)("ServiceCommonClass_010")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("TYPE_CD", OracleDbType.NVarchar2, inTypeCD)
                query.AddParameterWithTypeValue("TYPE_VAL", OracleDbType.NVarchar2, inTypeVal)

                'SQL実行
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dt.Count))

            Return dt

        End Function
        '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START

        ''' <summary>
        ''' サービス入庫IDからストールリスト取得
        ''' </summary>
        ''' <param name="searchServiceInId">検索予約ID</param>
        ''' <param name="resultsFlg">実績フラグ(0：中断実績チップを含まない、1：実績チップを含む全てのチップ)</param>
        ''' <param name="cancelFlg">キャンセルフラグ(0：キャンセルチップを含まない、1：キャンセルチップを含む)</param>
        ''' <returns>ストール情報</returns>
        ''' <remarks></remarks>
        Public Function GetDBStallListToReserve(ByVal searchServiceInId As Decimal, _
                                                ByVal resultsFlg As Long, _
                                                ByVal cancelFlg As Long) As ServiceCommonClassDataSet.StallInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} P1:{2} P2:{3} P3:{4}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      searchServiceInId, _
                                      resultsFlg, _
                                      cancelFlg))

            Dim dt As ServiceCommonClassDataSet.StallInfoDataTable = Nothing
            Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.StallInfoDataTable)("ServiceCommonClass_011")
                Dim sql As New StringBuilder

                With sql
                    .AppendLine(" SELECT /* ServiceCommonClass_011 */ ")
                    .AppendLine("        T3.STALL_ID ")
                    .AppendLine("   FROM TB_T_SERVICEIN T1 ")
                    .AppendLine("      , TB_T_JOB_DTL T2 ")
                    .AppendLine("      , TB_T_STALL_USE T3 ")
                    .AppendLine("  WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                    .AppendLine("    AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                    .AppendLine("    AND T1.SVCIN_ID = :SVCIN_ID ")

                    ' キャンセルチップを含まない場合
                    If cancelFlg = CancelFlgOff Then
                        .AppendLine("    AND T2.CANCEL_FLG = :CANCEL_FLG ")
                        query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlgOff)
                    End If

                    ' 実績チップを含まない場合
                    If resultsFlg = ResultsFlgOff Then
                        .AppendLine("    AND EXISTS ( ")
                        .AppendLine("        SELECT ")
                        .AppendLine("               1 ")
                        .AppendLine("          FROM TB_T_STALL_USE T4 ")
                        .AppendLine("         WHERE T4.JOB_DTL_ID = T2.JOB_DTL_ID ")
                        .AppendLine("         GROUP BY T4.JOB_DTL_ID ")
                        .AppendLine("        HAVING T3.STALL_USE_ID = MAX(T4.STALL_USE_ID) ")
                        .AppendLine("    ) ")
                    End If
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, searchServiceInId)

                'SQL実行
                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dt.Count))
            Return dt

        End Function

        ''' <summary>
        ''' 指定ストールIDからスタッフ情報取得(ストールグループ設定有り)
        ''' </summary>
        ''' <param name="stallIdList">ストールIDリスト</param>
        ''' <param name="operationCodeList">オペレーションコードリスト</param>
        ''' <returns>スタッフ情報リスト</returns>
        ''' <remarks></remarks>
        Public Function GetDBStaffInfoToStall(ByVal stallIdList As List(Of Decimal), _
                                              ByVal operationCodeList As List(Of Decimal)) As ServiceCommonClassDataSet.StaffInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} P1:{2} P2:{3}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      stallIdList.Count, _
                                      operationCodeList.Count))

            Dim dt As ServiceCommonClassDataSet.StaffInfoDataTable = Nothing

            Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.StaffInfoDataTable)("ServiceCommonClass_012")
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("   SELECT /* ServiceCommonClass_012 */ ")
                    .AppendLine("          T2.ACCOUNT ")
                    .AppendLine("        , T2.OPERATIONCODE ")
                    .AppendLine("     FROM TB_M_STAFF T1 ")
                    .AppendLine("        , TBL_USERS T2 ")
                    .AppendLine("        , TB_M_ORGANIZATION T3 ")
                    .AppendLine("        , TB_M_STALL_GROUP T4 ")
                    .AppendLine("        , TB_M_STALL_STALL_GROUP T5 ")
                    .AppendLine("    WHERE T1.STF_CD = T2.ACCOUNT ")
                    .AppendLine("      AND T1.ORGNZ_ID = T3.ORGNZ_ID ")
                    .AppendLine("      AND T3.ORGNZ_ID = T4.ORGNZ_ID ")
                    .AppendLine("      AND T4.STALL_GROUP_ID = T5.STALL_GROUP_ID ")
                    .AppendLine("      AND T2.DELFLG = :DELFLG ")
                    .AppendLine("      AND T3.INUSE_FLG = :INUSE_FLG ")
                    .AppendLine("      AND T3.ORGNZ_SA_FLG = :ORGNZ_SA_FLG ")

                    ' ストールID
                    If (stallIdList IsNot Nothing) AndAlso (0 < stallIdList.Count) Then
                        .Append("    AND T5.STALL_ID IN (")
                        Dim i As Integer = 1
                        For Each stallId As Decimal In stallIdList
                            .Append(" :STALL_ID" & CStr(i))
                            query.AddParameterWithTypeValue("STALL_ID" & CStr(i), OracleDbType.Decimal, stallId)
                            If Not stallIdList.Count() = i Then
                                .Append(",")
                            End If
                            i = i + 1
                        Next
                        .Append(" ) ")
                    End If

                    ' オペレーションコード
                    If (operationCodeList IsNot Nothing) AndAlso (0 < operationCodeList.Count) Then
                        .Append("    AND T2.OPERATIONCODE IN (")
                        Dim i As Integer = 1
                        For Each operationCd As Decimal In operationCodeList
                            .Append(" :OPERATIONCODE" & CStr(i))
                            query.AddParameterWithTypeValue("OPERATIONCODE" & CStr(i), OracleDbType.Decimal, operationCd)
                            If Not operationCodeList.Count() = i Then
                                .Append(",")
                            End If
                            i = i + 1
                        Next
                        .Append(" ) ")

                    End If

                    .Append("    AND T2.PRESENCECATEGORY IN ( :Standby, :Negotiate, :Leaving )")
                    .AppendLine(" ORDER BY T2.OPERATIONCODE ASC ")

                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("INUSE_FLG", OracleDbType.NVarchar2, InuseFlgOn)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DelFlgOff)
                query.AddParameterWithTypeValue("ORGNZ_SA_FLG", OracleDbType.NVarchar2, OrgnzSaFlgOn)
                query.AddParameterWithTypeValue("Standby", OracleDbType.Char, PresenceCategoryStandby)
                query.AddParameterWithTypeValue("Negotiate", OracleDbType.Char, PresenceCategoryNegotiate)
                query.AddParameterWithTypeValue("Leaving", OracleDbType.Char, PresenceCategoryLeaving)

                'SQL実行
                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dt.Count))
            Return dt

        End Function

        ''' <summary>
        ''' 指定権限コードからスタッフ情報取得(ストールグループ設定無し)
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="branchCode">店舗コード</param>
        ''' <param name="operationCodeList">オペレーションコードリスト</param>
        ''' <returns>スタッフ情報リスト</returns>
        ''' <remarks></remarks>
        Public Function GetDBStaffInfoToOpeCode(ByVal dealerCode As String, _
                                                ByVal branchCode As String, _
                                                ByVal operationCodeList As List(Of Decimal)) As ServiceCommonClassDataSet.StaffInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} P1:{2} P2:{3} P3:{4}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dealerCode, _
                                      branchCode, _
                                      operationCodeList.Count))

            Dim dt As ServiceCommonClassDataSet.StaffInfoDataTable = Nothing
            Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.StaffInfoDataTable)("ServiceCommonClass_013")
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("   SELECT /* ServiceCommonClass_013 */ ")
                    .AppendLine("          T2.ACCOUNT ")
                    .AppendLine("        , T2.OPERATIONCODE ")
                    .AppendLine("     FROM TB_M_STAFF T1 ")
                    .AppendLine("        , TBL_USERS T2 ")
                    .AppendLine("        , TB_M_STALL_GROUP T3 ")
                    .AppendLine("    WHERE T1.STF_CD = T2.ACCOUNT ")
                    .AppendLine("      AND T1.ORGNZ_ID = T3.ORGNZ_ID(+) ")
                    .AppendLine("      AND T2.DELFLG = :DELFLG ")
                    .AppendLine("      AND T3.STALL_GROUP_ID IS NULL ")
                    .AppendLine("      AND T2.DLRCD = :DLRCD ")
                    .AppendLine("      AND T2.STRCD = :STRCD ")

                    If (operationCodeList IsNot Nothing) AndAlso (0 < operationCodeList.Count) Then
                        .Append("    AND T2.OPERATIONCODE IN (")
                        Dim i As Integer = 1
                        For Each operationCd As Decimal In operationCodeList
                            .Append(" :OPERATIONCODE" & CStr(i))
                            query.AddParameterWithTypeValue("OPERATIONCODE" & CStr(i), OracleDbType.Decimal, operationCd)
                            If Not operationCodeList.Count() = i Then
                                .Append(",")
                            End If
                            i = i + 1
                        Next
                        .Append(" ) ")

                    End If

                    .Append("    AND T2.PRESENCECATEGORY IN ( :Standby, :Negotiate, :Leaving )")
                    .AppendLine(" ORDER BY T2.OPERATIONCODE ASC ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, branchCode)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DelFlgOff)
                query.AddParameterWithTypeValue("Standby", OracleDbType.Char, PresenceCategoryStandby)
                query.AddParameterWithTypeValue("Negotiate", OracleDbType.Char, PresenceCategoryNegotiate)
                query.AddParameterWithTypeValue("Leaving", OracleDbType.Char, PresenceCategoryLeaving)

                'SQL実行
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.{1} QUERY:COUNT = {2}", _
                                      Me.GetType.ToString, _
                                      System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                      dt.Count))
            Return dt
        End Function

        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

        '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 START

        ''' <summary>
        ''' 作業中の関連チップがあるストール名を取得する
        ''' </summary>
        ''' <param name="rezId">予約ID(STLL_USE_ID)</param>
        ''' <returns>ストール名</returns>
        ''' <remarks></remarks>
        Public Function GetStallInfoOfRelationChip(ByVal rezId As Decimal) As ServiceCommonClassDataSet.StallNameDataTable

            Dim dt As ServiceCommonClassDataSet.StallNameDataTable = Nothing
            Using query As New DBSelectQuery(Of ServiceCommonClassDataSet.StallNameDataTable)("ServiceCommonClass_014")
                Dim sql As New StringBuilder

                With sql
                    .Append("SELECT /* ServiceCommonClass_014 */ ")
                    .Append("       T4.STALL_ID")
                    .Append(",      T4.STALL_NAME_SHORT AS STALLNAME")
                    .Append("  FROM TB_T_SERVICEIN T1 ")
                    .Append("     , TB_T_JOB_DTL T2 ")
                    .Append("     , TB_T_STALL_USE T3 ")
                    .Append("     , TB_M_STALL T4 ")
                    .Append(" WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                    .Append("   AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                    .Append("   AND T4.STALL_ID = T3.STALL_ID ")
                    .Append("   AND T3.STALL_USE_STATUS IN (:STALL_USE_STATUS02, :STALL_USE_STATUS04)")
                    .Append("   AND T2.CANCEL_FLG = :DEL_FLG_OFF")
                    .Append("   AND T1.SVCIN_ID IN (")
                    .Append("           SELECT T5.SVCIN_ID")
                    .Append("           FROM   TB_T_JOB_DTL T5,")
                    .Append("                  TB_T_STALL_USE T6")
                    .Append("           WHERE T5.JOB_DTL_ID = T6.JOB_DTL_ID")
                    .Append("           AND    T6.STALL_USE_ID = :STALL_USE_ID")
                    .Append(")")
                    .Append("ORDER BY T4.STALL_ID")

                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, rezId)      'ストール利用ID
                query.AddParameterWithTypeValue("STALL_USE_STATUS02", OracleDbType.NVarchar2, stallUseStetus02) 'ストール利用ステータス:作業中
                query.AddParameterWithTypeValue("STALL_USE_STATUS04", OracleDbType.NVarchar2, stallUseStetus04) 'ストール利用ステータス:作業指示の一部の作業が中断
                query.AddParameterWithTypeValue("DEL_FLG_OFF", OracleDbType.NVarchar2, DelFlgOff) '削除フラグ(削除以外)

                dt = query.GetData()

            End Using

            Return dt

        End Function

        '2017/01/17 NSK 竹中 TR-SVT-TMT-20151019-002 TCメイン画面のエラーメッセージが理解不可能 END

#End Region

    End Class
End Namespace