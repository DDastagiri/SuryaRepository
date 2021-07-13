'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3290104DataSet.vb
'─────────────────────────────────────
'機能： フォロー設定データアクセス
'補足： 
'作成： 2014/06/11 TMEJ t.mizumoto
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'─────────────────────────────────────
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization

' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.SystemSettingDataSet
' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

Namespace SC3290104DataSetTableAdapters

    ''' <summary>
    ''' SC3290104 フォロー設定画面 データ層
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3290104TableAdapter
        Inherits Global.System.ComponentModel.Component


#Region "定数"

        ''' <summary>
        ''' 機能ID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AppId As String = "SC3290104"

        ''' <summary>
        ''' DB初期値（数値）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DBDefaultValueNumber As Integer = 0

        ''' <summary>
        ''' DB初期値（文字列）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DBDefaultValueString As String = " "

        ''' <summary>
        ''' システム設定：異常猶予日数
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SystemSettingIrregPostponementDay As String = "IRREG_POSTPONEMENT_DAY"

        ''' <summary>
        ''' フォロー完了フラグ：完了
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FllwCompleteFlgComplete As String = "1"

        ''' <summary>
        ''' フォロー完了フラグ：未完了
        ''' </summary>
        ''' <remarks></remarks>
        Private Const FllwCompleteFlgNotComplete As String = "0"

        ''' <summary>
        ''' 重点フォローフラグ：登録なし
        ''' </summary>
        ''' <remarks></remarks>
        Private Const EmphasisFlgNotRegister As String = "0"

#End Region

#Region "コンストラクタ"

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>処理なし</remarks>
        Public Sub New()

        End Sub

#End Region


#Region "公開メソッド"

        ''' <summary>
        ''' 異常項目フォローの取得
        ''' </summary>
        ''' <param name="irregFllwId">異常フォローID</param>
        ''' <param name="irregClassCode">異常分類コード</param>
        ''' <param name="irregItemCode">異常項目コード</param>
        ''' <param name="stfCode">スタッフコード</param>
        ''' <param name="nowDate">現在日時</param>
        ''' <returns>引数で指定した値に一致した異常項目フォロー状況を返却。データが存在しない場合はNothingを返却。</returns>
        ''' <remarks></remarks>
        Public Function GetIrregularFollowInfo(ByVal irregFllwId As String, ByVal irregClassCode As String, ByVal irregItemCode As String, ByVal stfCode As String, _
                                               ByVal nowDate As Date) As SC3290104DataSet.SC3290104IrregFllwRow

            Using query As New DBSelectQuery(Of SC3290104DataSet.SC3290104IrregFllwDataTable)("SC3290104_001")

                Dim sql As New StringBuilder

                ' 異常詳細画面からの呼び出しの場合
                If irregFllwId = DBDefaultValueString Then

                    With sql
                        .Append(" SELECT /* SC3290104_001 */")
                        .Append("        IRREG_FLLW_ID")
                        .Append("      , IRREG_CLASS_CD")
                        .Append("      , IRREG_ITEM_CD")
                        .Append("      , STF_CD")
                        .Append("      , FLLW_PIC_STF_CD")
                        .Append("      , FLLW_COMPLETE_FLG")
                        .Append("      , FLLW_EXPR_DATE")
                        .Append("      , FLLW_MEMO")
                        .Append("   FROM TB_T_IRREG_FLLW")
                        .Append("  WHERE IRREG_CLASS_CD = :IRREG_CLASS_CD")
                        .Append("    AND IRREG_ITEM_CD = :IRREG_ITEM_CD")
                        .Append("    AND STF_CD = :STF_CD")
                        .Append("    AND (")
                        .Append("            FLLW_COMPLETE_FLG = '" + FllwCompleteFlgNotComplete + "'")
                        .Append("         OR (FLLW_COMPLETE_FLG = '" + FllwCompleteFlgComplete + "' AND FLLW_EXPR_DATE >= :FLLW_EXPR_DATE)")
                        .Append("    )")
                    End With

                    query.CommandText = sql.ToString()
                    query.AddParameterWithTypeValue("IRREG_CLASS_CD", OracleDbType.NVarchar2, irregClassCode)
                    query.AddParameterWithTypeValue("IRREG_ITEM_CD", OracleDbType.NVarchar2, irregItemCode)
                    query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, stfCode)
                    query.AddParameterWithTypeValue("FLLW_EXPR_DATE", OracleDbType.Date, nowDate.Date)

                Else

                    With sql
                        .Append(" SELECT /* SC3290104_002 */")
                        .Append("        IRREG_FLLW_ID")
                        .Append("      , IRREG_CLASS_CD")
                        .Append("      , IRREG_ITEM_CD")
                        .Append("      , STF_CD")
                        .Append("      , FLLW_PIC_STF_CD")
                        .Append("      , FLLW_COMPLETE_FLG")
                        .Append("      , FLLW_EXPR_DATE")
                        .Append("      , FLLW_MEMO")
                        .Append("   FROM TB_T_IRREG_FLLW")
                        .Append("  WHERE IRREG_FLLW_ID = :IRREG_FLLW_ID")
                    End With

                    query.CommandText = sql.ToString()
                    query.AddParameterWithTypeValue("IRREG_FLLW_ID", OracleDbType.NVarchar2, irregFllwId)

                End If

                Dim dataTable As SC3290104DataSet.SC3290104IrregFllwDataTable
                dataTable = query.GetData()

                If 0 < dataTable.Rows.Count Then
                    Return dataTable(0)
                End If

                Return Nothing

            End Using

        End Function

        ''' <summary>
        ''' 異常項目フォローの設定
        ''' </summary>
        ''' <param name="row">異常項目フォローの行</param>
        ''' <param name="account">更新するアカウント</param>
        ''' <param name="nowDate">現在日時</param>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
        ''' </history>
        Public Sub SetIrregularFollowInfo(ByVal row As SC3290104DataSet.SC3290104IrregFllwRow, ByVal account As String, ByVal nowDate As Date)

            ' フォロー期日の計算
            Dim fllwExprDate As Date = row.FLLW_EXPR_DATE

            If row.FLLW_COMPLETE_FLG = FllwCompleteFlgComplete Then

                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
                '' フォロー完了の猶予期日
                'Dim systemSettingRow As SC3290104DataSet.SC3290104SystemSettingRow
                'systemSettingRow = Me.GetSystemSetting(SystemSettingIrregPostponementDay)

                ' フォロー完了の猶予期日
                Dim systemSetting = New SystemSetting
                Dim systemSettingRow As TB_M_SYSTEM_SETTINGRow = systemSetting.GetSystemSetting(SystemSettingIrregPostponementDay)
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

                fllwExprDate = nowDate.Date.AddDays(Integer.Parse(systemSettingRow.SETTING_VAL, CultureInfo.InvariantCulture))
            End If

            If row.IRREG_FLLW_ID = DBDefaultValueNumber Then

                Using query As New DBUpdateQuery("SC3290104_003")

                    Dim sql As New StringBuilder
                    With sql
                        .Append(" DELETE /* SC3290104_003 */")
                        .Append("   FROM TB_T_IRREG_FLLW")
                        .Append("  WHERE IRREG_CLASS_CD = :IRREG_CLASS_CD")
                        .Append("    AND IRREG_ITEM_CD = :IRREG_ITEM_CD")
                        .Append("    AND STF_CD = :STF_CD")
                    End With

                    query.CommandText = sql.ToString()

                    query.AddParameterWithTypeValue("IRREG_CLASS_CD", OracleDbType.NVarchar2, row.IRREG_CLASS_CD)
                    query.AddParameterWithTypeValue("IRREG_ITEM_CD", OracleDbType.NVarchar2, row.IRREG_ITEM_CD)
                    query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, row.STF_CD)

                    query.Execute()

                End Using

                Using query As New DBUpdateQuery("SC3290104_004")

                    Dim sql As New StringBuilder
                    With sql
                        .Append(" INSERT /* SC3290104_004 */")
                        .Append("   INTO TB_T_IRREG_FLLW (")
                        .Append("        IRREG_FLLW_ID")
                        .Append("      , IRREG_CLASS_CD")
                        .Append("      , IRREG_ITEM_CD")
                        .Append("      , STF_CD")
                        .Append("      , FLLW_PIC_STF_CD")
                        .Append("      , FLLW_COMPLETE_FLG")
                        .Append("      , FLLW_EXPR_DATE")
                        .Append("      , FLLW_MEMO")
                        .Append("      , ROW_CREATE_DATETIME")
                        .Append("      , ROW_CREATE_ACCOUNT")
                        .Append("      , ROW_CREATE_FUNCTION")
                        .Append("      , ROW_UPDATE_DATETIME")
                        .Append("      , ROW_UPDATE_ACCOUNT")
                        .Append("      , ROW_UPDATE_FUNCTION")
                        .Append("      , ROW_LOCK_VERSION")
                        .Append(" )")
                        .Append(" VALUES (")
                        .Append("        SQ_IRREG_FLLW_ID.NEXTVAL")
                        .Append("      , :IRREG_CLASS_CD")
                        .Append("      , :IRREG_ITEM_CD")
                        .Append("      , :STF_CD")
                        .Append("      , :FLLW_PIC_STF_CD")
                        .Append("      , :FLLW_COMPLETE_FLG")
                        .Append("      , :FLLW_EXPR_DATE")
                        .Append("      , :FLLW_MEMO")
                        .Append("      , SYSDATE")
                        .Append("      , :ROW_CREATE_ACCOUNT")
                        .Append("      , :ROW_CREATE_FUNCTION")
                        .Append("      , SYSDATE")
                        .Append("      , :ROW_CREATE_ACCOUNT")
                        .Append("      , :ROW_CREATE_FUNCTION")
                        .Append("      , :ROW_LOCK_VERSION")
                        .Append(" )")
                    End With

                    query.CommandText = sql.ToString()

                    query.AddParameterWithTypeValue("IRREG_CLASS_CD", OracleDbType.NVarchar2, row.IRREG_CLASS_CD)
                    query.AddParameterWithTypeValue("IRREG_ITEM_CD", OracleDbType.NVarchar2, row.IRREG_ITEM_CD)
                    query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, row.STF_CD)
                    query.AddParameterWithTypeValue("FLLW_PIC_STF_CD", OracleDbType.NVarchar2, row.FLLW_PIC_STF_CD)
                    query.AddParameterWithTypeValue("FLLW_COMPLETE_FLG", OracleDbType.NVarchar2, row.FLLW_COMPLETE_FLG)
                    query.AddParameterWithTypeValue("FLLW_EXPR_DATE", OracleDbType.Date, fllwExprDate)
                    query.AddParameterWithTypeValue("FLLW_MEMO", OracleDbType.NVarchar2, row.FLLW_MEMO)
                    query.AddParameterWithTypeValue("ROW_CREATE_ACCOUNT", OracleDbType.NVarchar2, account)
                    query.AddParameterWithTypeValue("ROW_CREATE_FUNCTION", OracleDbType.NVarchar2, AppId)
                    query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Long, DBDefaultValueNumber)

                    query.Execute()

                End Using

            Else

                Using query As New DBUpdateQuery("SC3290104_005")

                    Dim sql As New StringBuilder
                    With sql
                        .Append(" UPDATE /* SC3290104_005 */")
                        .Append("        TB_T_IRREG_FLLW")
                        .Append("    SET FLLW_PIC_STF_CD = :FLLW_PIC_STF_CD")
                        .Append("      , FLLW_COMPLETE_FLG = :FLLW_COMPLETE_FLG")
                        .Append("      , FLLW_EXPR_DATE = :FLLW_EXPR_DATE")
                        .Append("      , FLLW_MEMO = :FLLW_MEMO")
                        .Append("      , ROW_UPDATE_DATETIME = SYSDATE")
                        .Append("      , ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT")
                        .Append("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION")
                        .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1")
                        .Append("  WHERE IRREG_FLLW_ID = :IRREG_FLLW_ID")
                    End With

                    query.CommandText = sql.ToString()

                    query.AddParameterWithTypeValue("FLLW_PIC_STF_CD", OracleDbType.NVarchar2, row.FLLW_PIC_STF_CD)
                    query.AddParameterWithTypeValue("FLLW_COMPLETE_FLG", OracleDbType.NVarchar2, row.FLLW_COMPLETE_FLG)
                    query.AddParameterWithTypeValue("FLLW_EXPR_DATE", OracleDbType.Date, fllwExprDate)
                    query.AddParameterWithTypeValue("FLLW_MEMO", OracleDbType.NVarchar2, row.FLLW_MEMO)
                    query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, account)
                    query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, AppId)
                    query.AddParameterWithTypeValue("IRREG_FLLW_ID", OracleDbType.Decimal, row.IRREG_FLLW_ID)

                    query.Execute()

                End Using

                ' フォロー完了へ更新する場合
                If row.FLLW_COMPLETE_FLG = FllwCompleteFlgComplete Then

                    ' SPMピンの取得
                    Dim spmPinDataTable As SC3290104DataSet.SC3290104SpmPinDataTable
                    spmPinDataTable = Me.GetSpmPin(row.IRREG_FLLW_ID)

                    ' SPMピンが存在している場合
                    If 0 < spmPinDataTable.Rows.Count Then

                        Using query As New DBUpdateQuery("SC3290104_006")

                            Dim sql As New StringBuilder
                            With sql
                                .Append(" UPDATE /* SC3290104_006 */")
                                .Append("        TB_T_SPM_PIN")
                                .Append("    SET EMPHASIS_FLG = '" & EmphasisFlgNotRegister & "'")
                                .Append("      , IRREG_FLLW_ID = " & DBDefaultValueNumber)
                                .Append("      , ROW_UPDATE_DATETIME = SYSDATE")
                                .Append("      , ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT")
                                .Append("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION")
                                .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1")
                                .Append("  WHERE IRREG_FLLW_ID = :IRREG_FLLW_ID")
                            End With

                            query.CommandText = sql.ToString()

                            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, account)
                            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, AppId)
                            query.AddParameterWithTypeValue("IRREG_FLLW_ID", OracleDbType.Decimal, row.IRREG_FLLW_ID)

                            query.Execute()

                        End Using

                    End If

                End If

            End If

        End Sub

#End Region

#Region "非公開メソッド"

        ''' <summary>
        ''' SPMピンの取得
        ''' </summary>
        ''' <param name="irregFllwId">異常フォローID</param>
        ''' <returns>引数で指定した値に一致したSPMピンを返却。</returns>
        ''' <remarks></remarks>
        Private Function GetSpmPin(ByVal irregFllwId As Decimal) _
            As SC3290104DataSet.SC3290104SpmPinDataTable

            Using query As New DBSelectQuery(Of SC3290104DataSet.SC3290104SpmPinDataTable)("SC3290104_007")

                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3290104_007 */")
                    .Append("        SPM_PIN_ID")
                    .Append("      , EMPHASIS_FLG")
                    .Append("      , CHIP_MEMO_FLG")
                    .Append("      , CHIP_MEMO")
                    .Append("      , IRREG_FLLW_ID")
                    .Append("   FROM TB_T_SPM_PIN")
                    .Append("  WHERE IRREG_FLLW_ID = :IRREG_FLLW_ID")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("IRREG_FLLW_ID", OracleDbType.Decimal, irregFllwId)

                Return query.GetData()

            End Using

        End Function

        ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
        ' ''' <summary>
        ' ''' システム設定から設定値を取得します。
        ' ''' </summary>
        ' ''' <param name="settingName">設定名</param>
        ' ''' <returns>システム設定の行</returns>
        ' ''' <remarks></remarks>
        'Private Function GetSystemSetting(ByVal settingName As String) As SC3290104DataSet.SC3290104SystemSettingRow

        '    'DbSelectQueryインスタンス生成
        '    Using query As New DBSelectQuery(Of SC3290104DataSet.SC3290104SystemSettingDataTable)("SC3290104_008")

        '        Dim sql As New StringBuilder
        '        With sql
        '            .Append("SELECT /* SC3290104_008 */")
        '            .Append("       SETTING_NAME ")
        '            .Append("     , SETTING_VAL ")
        '            .Append("  FROM ")
        '            .Append("       TB_M_SYSTEM_SETTING ")
        '            .Append(" WHERE SETTING_NAME = :SETTINGNAME ")
        '        End With

        '        query.CommandText = sql.ToString()

        '        'SQLパラメータ設定
        '        query.AddParameterWithTypeValue("SETTINGNAME", OracleDbType.NVarchar2, settingName)
        '        Return query.GetData()(0)
        '    End Using
        'End Function
        '' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

#End Region

    End Class

End Namespace

Partial Class SC3290104DataSet
End Class
