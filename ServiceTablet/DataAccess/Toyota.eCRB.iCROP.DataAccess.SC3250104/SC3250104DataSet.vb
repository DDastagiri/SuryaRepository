'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3250104DataSet.vb
'─────────────────────────────────────
'機能： 部品説明/前回履歴DataSet.vb
'補足： 
'作成： 2014/08/XX NEC 村瀬
'更新： 2014/08/xx 
'─────────────────────────────────────

Option Explicit On

Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Configuration
Imports Toyota.eCRB.SystemFrameworks.Core

Partial Class SC3250104DataSet

    ''' <summary>
    ''' 001:デフォルトのパーツ写真（Old,New）のファイル名、選択した写真ファイル名を取得する
    ''' </summary>
    ''' <param name="strKeepKey">現在保持しているキー</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDefaultPartsFileName(
                        ByVal strKeepKey As String, _
                        ByVal strINSPEC_ITEM_CD As String, _
                        ByVal strDLR_CD As String, _
                        ByVal strBRN_CD As String, _
                        ByVal isRoActive As Boolean
                                            ) As SC3250104DataSet.DefaultPartsFileDataTable

        Dim sqlNo As String = "SC3250104_001"
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Dim sql As New StringBuilder
        'SQL文作成
        With sql
            .Append("SELECT /* SC3250104_001 */ ")
            .Append("	M1.INSPEC_ITEM_CD ")
            .Append("	,NVL(M1.NEW_PARTS_FILE_NAME, ' ') AS NEW_PARTS_FILE_NAME ")
            .Append("	,NVL(M1.OLD_PARTS_FILE_NAME, ' ') AS OLD_PARTS_FILE_NAME ")
            .Append("	,NVL(T1.SEL_PICTURE_URL, ' ') AS SEL_PICTURE_URL ")
            .Append("FROM ")
            .Append("	(SELECT ")
            .Append("		INSPEC_ITEM_CD ")
            .Append("		,NEW_PARTS_FILE_NAME ")
            .Append("		,OLD_PARTS_FILE_NAME ")
            .Append("	FROM ")
            .Append("		TB_M_FINAL_INSPECTION_DETAIL ")
            .Append("	WHERE ")
            .Append("		INSPEC_ITEM_CD = :INSPEC_ITEM_CD ) M1 ")
            .Append("	,(SELECT ")
            .Append("		:INSPEC_ITEM_CD AS INSPEC_ITEM_CD ")
            .Append("		,SEL_PICTURE_URL ")
            .Append("	FROM ")
            If isRoActive Then
                .Append("		TB_T_SEL_PICTURE ")
            Else
                .Append("		TB_H_SEL_PICTURE ")
            End If

            .Append("	WHERE ")
            .Append("		DLR_CD = :DLR_CD AND ")
            .Append("		BRN_CD = :BRN_CD AND ")
            .Append("		RO_NUM = :RO_NUM AND ")
            .Append("		INSPEC_ITEM_CD = :INSPEC_ITEM_CD ) T1 ")

            .Append("WHERE ")
            .Append("	M1.INSPEC_ITEM_CD = T1.INSPEC_ITEM_CD(+) ")
        End With

        Using query As New DBSelectQuery(Of SC3250104DataSet.DefaultPartsFileDataTable)(sqlNo)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, strBRN_CD)
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.Char, strKeepKey)
            query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.Char, strINSPEC_ITEM_CD)

            sql = Nothing

            Using dt As SC3250104DataSet.DefaultPartsFileDataTable = query.GetData
                Return dt
            End Using
        End Using

    End Function

    ''' <summary>
    ''' 002:写真選択画面で選択した写真のファイルパスを取得する
    ''' </summary>
    ''' <param name="strKeepKey">現在保持しているキー</param>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSelectedPartsFileName(
                                ByVal strKeepKey As String,
                                ByVal strDLR_CD As String,
                                ByVal strBRN_CD As String,
                                ByVal strINSPEC_ITEM_CD As String
                                            ) As SC3250104DataSet.SelectedPartsFileDataTable

        Dim sqlNo As String = "SC3250104_002"
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Dim sql As New StringBuilder
        'SQL文作成
        With sql
            .Append("SELECT /* SC3250104_002 */ ")
            .Append("	SEL_PICTURE_URL ")
            .Append("FROM ")
            .Append("	TB_T_SEL_PICTURE ")
            .Append("WHERE ")
            .Append("	DLR_CD = :DLR_CD ")
            .Append("	AND BRN_CD = :BRN_CD ")
            .Append("	AND RO_NUM = :RO_NUM ")
            .Append("	AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")

        End With

        Using query As New DBSelectQuery(Of SC3250104DataSet.SelectedPartsFileDataTable)(sqlNo)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, strBRN_CD)
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.Char, strKeepKey)
            query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.Char, strINSPEC_ITEM_CD)

            sql = Nothing

            Using dt As SC3250104DataSet.SelectedPartsFileDataTable = query.GetData
                Return dt
            End Using
        End Using

    End Function

    ''' <summary>
    ''' 003:写真選択画面で選択した写真のファイルパスを新規登録する
    ''' </summary>
    ''' <param name="strKeepKey">現在保持しているキー</param>
    ''' <param name="strSEL_PICTURE_URL">写真URL</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InsertPartsFileName(ByVal strKeepKey As String _
                                        , ByVal strSEL_PICTURE_URL As String _
                                        , ByVal strSTF_CD As String _
                                        , ByVal strDLR_CD As String,
                                          ByVal strBRN_CD As String,
                                          ByVal strINSPEC_ITEM_CD As String
                                        ) As Integer

        Dim No As String = "SC3250104_003"
        Dim strMethodName As String = "InsertPartsFileName"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Try
            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append("INSERT /* SC3250104_003 */ ")
                .Append("  INTO TB_T_SEL_PICTURE ")
                .Append("( ")
                .Append("	 DLR_CD ")
                .Append("	,BRN_CD ")
                .Append("	,RO_NUM ")
                .Append("	,INSPEC_ITEM_CD ")
                .Append("	,SEL_PICTURE_URL ")
                .Append("	,ROW_CREATE_DATETIME ")
                .Append("	,ROW_CREATE_ACCOUNT ")
                .Append("	,ROW_CREATE_FUNCTION ")
                .Append("	,ROW_UPDATE_DATETIME ")
                .Append("	,ROW_UPDATE_ACCOUNT ")
                .Append("	,ROW_UPDATE_FUNCTION ")
                .Append("	,ROW_LOCK_VERSION ")

                .Append(") ")
                .Append("VALUES ")
                .Append("( ")
                .Append("       :DLR_CD ")
                .Append("     , :BRN_CD ")
                .Append("     , :RO_NUM ")
                .Append("     , :INSPEC_ITEM_CD ")
                .Append("     , :SEL_PICTURE_URL ")
                .Append("     , SYSDATE ")
                .Append("     , :STF_CD ")
                .Append("     , 'SC3250104' ")
                .Append("     , SYSDATE ")
                .Append("     , :STF_CD ")
                .Append("     , 'SC3250104' ")
                .Append("     , 0 ")

                .Append(") ")
            End With

            Using query As New DBUpdateQuery(No)
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, strDLR_CD)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, strBRN_CD)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, strKeepKey)
                query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.Char, strINSPEC_ITEM_CD)
                query.AddParameterWithTypeValue("SEL_PICTURE_URL", OracleDbType.NVarchar2, strSEL_PICTURE_URL)
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, strSTF_CD)

                Dim ret As Integer = query.Execute()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
                'ログ出力 End *****************************************************************************

                Return ret
            End Using

        Catch ex As Exception
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
            'ログ出力 End *****************************************************************************
            Return 99
        End Try

    End Function

    ''' <summary>
    ''' 004:写真選択画面で選択した写真のファイルパスを更新する
    ''' </summary>
    ''' <param name="strKeepKey">現在保持しているキー</param>
    ''' <param name="strSEL_PICTURE_URL">写真URL</param>
    ''' <param name="strSTF_CD">スタッフコード</param>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdatePartsFileName(ByVal strKeepKey As String _
                                        , ByVal strSEL_PICTURE_URL As String _
                                        , ByVal strSTF_CD As String _
                                        , ByVal strDLR_CD As String,
                                          ByVal strBRN_CD As String,
                                          ByVal strINSPEC_ITEM_CD As String
                                        ) As Integer

        Dim No As String = "SC3250104_004"
        Dim strMethodName As String = "UpdatePartsFileName"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Try
            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append("UPDATE /* SC3250104_004 */ ")
                .Append("	TB_T_SEL_PICTURE ")
                .Append("SET  ")
                .Append("	SEL_PICTURE_URL = :SEL_PICTURE_URL ")
                .Append("	,ROW_UPDATE_DATETIME = SYSDATE ")
                .Append("	,ROW_UPDATE_ACCOUNT = :STF_CD ")
                .Append("	,ROW_UPDATE_FUNCTION = 'SC3250104' ")
                .Append("WHERE ")
                .Append("   DLR_CD = :DLR_CD ")
                .Append("   AND BRN_CD = :BRN_CD ")
                .Append("	AND RO_NUM = :RO_NUM ")
                .Append("   AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")

            End With

            Using query As New DBUpdateQuery(No)
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, strDLR_CD)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, strBRN_CD)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, strKeepKey)
                query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.Char, strINSPEC_ITEM_CD)
                query.AddParameterWithTypeValue("SEL_PICTURE_URL", OracleDbType.NVarchar2, strSEL_PICTURE_URL)
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, strSTF_CD)

                Dim ret As Integer = query.Execute()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
                'ログ出力 End *****************************************************************************

                Return ret
            End Using

        Catch ex As Exception
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
            'ログ出力 End *****************************************************************************
            Return 99
        End Try
    End Function

    ''' <summary>
    ''' 005:販売店システム設定から設定値を取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="allDealerCode">全店舗を示す販売店コード</param>
    ''' <param name="allBranchCode">全店舗を示す店舗コード</param>
    ''' <param name="settingName">販売店システム設定名</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDlrSystemSettingValue(ByVal dealerCode As String, _
                                             ByVal branchCode As String, _
                                             ByVal allDealerCode As String, _
                                             ByVal allBranchCode As String, _
                                             ByVal settingName As String) As SC3250104DataSet.SystemSettingDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  dealerCode, _
                                  branchCode, _
                                  allDealerCode, _
                                  allBranchCode, _
                                  settingName))

        Dim sql As New StringBuilder
        With sql
            .Append("   SELECT /* SC3250104_005 */ ")
            .Append(" 		   SETTING_VAL ")
            .Append("     FROM ")
            .Append(" 		   TB_M_SYSTEM_SETTING_DLR ")
            .Append("    WHERE ")
            .Append(" 		   DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
            .Append(" 	   AND BRN_CD IN (:BRN_CD, :ALL_BRN_CD) ")
            .Append("      AND SETTING_NAME = :SETTING_NAME ")
            .Append(" ORDER BY ")
            .Append("          DLR_CD ASC, BRN_CD ASC ")
        End With

        Dim dt As SC3250104DataSet.SystemSettingDataTable = Nothing

        Using query As New DBSelectQuery(Of SC3250104DataSet.SystemSettingDataTable)("SC3250104_005")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
            query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, allDealerCode)
            query.AddParameterWithTypeValue("ALL_BRN_CD", OracleDbType.NVarchar2, allBranchCode)
            query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.NVarchar2, settingName)

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
    ''' 006:i-CROP→DMSの値に変換された値を基幹コードマップテーブルから取得する
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
                                      ByVal icropCD3 As String) As SC3250104DataSet.DmsCodeMapDataTable

        Dim sql As New StringBuilder
        With sql
            .Append("   SELECT /* SC3250104_006 */ ")
            .Append(" 		     DMS_CD_1 CODE1 ")                '基幹コード1
            .Append(" 		   , DMS_CD_2 CODE2 ")                '基幹コード2
            .Append(" 		   , DMS_CD_3 CODE3 ")                '基幹コード3
            .Append("     FROM ")
            .Append(" 		     TB_M_DMS_CODE_MAP ")             '基幹コードマップ
            .Append("    WHERE ")
            .Append(" 		     DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
            .Append(" 	   AND   DMS_CD_TYPE = :DMS_CD_TYPE ")
            .Append(" 	   AND   ICROP_CD_1 = :ICROP_CD_1 ")

            If Not String.IsNullOrEmpty(icropCD2) Then
                .AppendLine(" 	   AND   ICROP_CD_2 = :ICROP_CD_2 ")
            End If

            If Not String.IsNullOrEmpty(icropCD3) Then
                .AppendLine(" 	   AND   ICROP_CD_3 = :ICROP_CD_3 ")
            End If

            .AppendLine(" ORDER BY DLR_CD ASC ")
        End With

        Dim dt As SC3250104DataSet.DmsCodeMapDataTable = Nothing

        Using query As New DBSelectQuery(Of SC3250104DataSet.DmsCodeMapDataTable)("SC3250104_006")
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

        Return dt

    End Function


    ''' <summary>
    ''' 007:写真選択画面で選択した写真のファイルパスを削除する
    ''' </summary>
    ''' <param name="strKeepKey">現在保持しているキー</param>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <param name="strINSPEC_ITEM_CD">点検項目コード</param>
    ''' <returns></returns>
    ''' <remarks>2014/09/30　写真選択画面でキャンセルボタンタップ時に写真をデフォルトに戻す仕様追加</remarks>
    Public Function DeletePartsFileName(
                                ByVal strKeepKey As String,
                                ByVal strDLR_CD As String,
                                ByVal strBRN_CD As String,
                                ByVal strINSPEC_ITEM_CD As String
                                       ) As Integer

        Dim No As String = "SC3250104_007"
        Dim strMethodName As String = "DeletePartsFileName"
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
        'ログ出力 End *****************************************************************************

        Try
            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append("DELETE FROM /* SC3250104_007 */ ")
                .Append("	TB_T_SEL_PICTURE ")
                .Append("WHERE ")
                .Append("   DLR_CD = :DLR_CD ")
                .Append("   AND BRN_CD = :BRN_CD ")
                .Append("	AND RO_NUM = :RO_NUM ")
                .Append("   AND INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")

            End With

            Using query As New DBUpdateQuery(No)
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, strDLR_CD)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, strBRN_CD)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, strKeepKey)
                query.AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.Char, strINSPEC_ITEM_CD)

                Dim ret As Integer = query.Execute()

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
                'ログ出力 End *****************************************************************************

                Return ret
            End Using

        Catch ex As Exception
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
            'ログ出力 End *****************************************************************************
            Return 99
        End Try
    End Function

    ''' <summary>
    ''' RO情報に紐付くSAChipIDが存在するか判定する 
    ''' </summary>
    ''' <param name="strRO_NUM">R/O番号</param>
    ''' <param name="strDLR_CD">販売店コード</param>
    ''' <param name="strBRN_CD">店舗コード</param>
    ''' <returns></returns>
    Public Function ExistSAChipID(
                        ByVal strRO_NUM As String, _
                        ByVal strDLR_CD As String, _
                        ByVal strBRN_CD As String, _
                        ByVal isRoActive As Boolean
                                 ) As SC3250104DataSet.SAchipIDExistCheckDataTable

        Dim sqlNo As String = "SC3250104_008"
        Dim methodName As String = System.Reflection.MethodBase.GetCurrentMethod.Name

        Dim sql As New StringBuilder
        'SQL文作成
        With sql
            .Append("SELECT /* SC3250104_008 */ ")
            .Append("    VISIT_ID ")
            .Append("FROM ")
            If isRoActive Then
                .Append("    TB_T_RO_INFO ")
            Else
                .Append("    TB_H_RO_INFO ")
            End If
            .Append("WHERE ")
            .Append("        DLR_CD = :DLR_CD ")
            .Append("    AND BRN_CD = :BRN_CD ")
            .Append("    AND RO_NUM = :RO_NUM ")

        End With

        Using query As New DBSelectQuery(Of SC3250104DataSet.SAchipIDExistCheckDataTable)(sqlNo)
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, strDLR_CD)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, strBRN_CD)
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.Char, strRO_NUM)
            sql = Nothing

            Using dt As SC3250104DataSet.SAchipIDExistCheckDataTable = query.GetData
                Return dt
            End Using
        End Using

    End Function

    '2017/XX/XX ライフサイクル対応　↓
    Public Function ChkExistParamRoActive(ByVal argDlrCd As String, _
                                          ByVal argBrncd As String, _
                                          ByVal argRoNum As String) As Boolean

        Dim ret As Boolean = False
        '開始ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using query As New DBSelectQuery(Of DataTable)("SC3250101_044")
            Dim sql As New StringBuilder

            With sql
                .AppendLine(" SELECT ")
                .AppendLine("        COUNT(1) AS CNT")
                .AppendLine("    FROM")
                .AppendLine("        TB_T_SERVICEIN")
                .AppendLine("    WHERE ")
                .AppendLine("        DLR_CD = :DLR_CD")
                .AppendLine("    AND BRN_CD = :BRN_CD")
                .AppendLine("    AND RO_NUM = :RO_NUM")
            End With
            'クエリ設定
            query.CommandText = sql.ToString()
            'パラメータ設定
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, argDlrCd)     '販売店
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, argBrncd)     '店舗
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, argRoNum)     'RO番号
            '結果取得
            Dim dt As DataTable = query.GetData()

            If dt.Rows(0).Item("CNT").ToString <> CStr(0) Then
                ret = True
            End If
        End Using

        '終了ログ
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return ret

    End Function
    '2017/XX/XX ライフサイクル対応　↑

End Class

