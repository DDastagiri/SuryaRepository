'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'VisitUtilityDataSet.vb
'──────────────────────────────────
'機能： 来店共通
'補足： 
'作成： - 
'更新： 2013/10/15 TMEJ m.asano   次世代e-CRBセールス機能 新DB適応に向けた機能開発 $01
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'──────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace VisitUtilityDataSetTableAdapters

    ''' <summary>
    ''' 共通テーブルアダプター
    ''' </summary>
    ''' <remarks></remarks>
    Public Class VisitUtilityDataSetTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' 苦情情報ステータス（1次対応中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ClaimStatusFirst As String = "1"

        ''' <summary>
        ''' 苦情情報ステータス（最終対応中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ClaimStatusLast As String = "2"

        ''' <summary>
        ''' 苦情情報ステータス（完了）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ClaimStatusComplete As String = "3"

        ''' <summary>
        ''' 苦情情報紐付け関係フラグ(親)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RelationFlgOn As String = "1"

        ''' <summary>
        ''' 苦情情報付け関係フラグ(なし)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RelationFlgOff As String = "0"

        ' $01 start
        ''' <summary>
        ''' 用件内容(苦情)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const BizTypeClaim As String = "3"
        ' $01 end

#End Region

#Region "苦情情報有無の取得"

        ''' <summary>
        ''' 苦情情報有無の取得
        ''' </summary>
        ''' <param name="customerKind">販売店コード</param>
        ''' <param name="customerCode">店舗コード</param>
        ''' <param name="completeDate">完了表示日時</param>
        ''' <returns>苦情情報有無</returns>
        ''' <remarks></remarks>
        Public Function HasClaimInfo(ByVal customerKind As String, _
                                     ByVal customerCode As String, _
                                     ByVal completeDate As Date _
                                     ) As Boolean

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                ' $01 start
                .Append(" SELECT /* VisitUtility_001 */ ")
                .Append("     SUM(CLAIMCOUNT) AS CLAIMCOUNT ")
                .Append(" FROM ")
                .Append(" ( ")
                .Append("     SELECT ")
                .Append("            COUNT(1) AS CLAIMCOUNT ")
                .Append("       FROM TB_T_COMPLAINT CLM ")
                .Append("          , TB_T_COMPLAINT_DETAIL CLMD ")
                .Append("          , TB_T_REQUEST TR ")
                .Append("      WHERE CLM.CMPL_ID = CLMD.CMPL_ID ")
                .Append("        AND CLM.REQ_ID = TR.REQ_ID ")
                .Append("        AND TR.CST_ID = :INSDID ")
                .Append("        AND CLM.RELATION_TYPE IN (:RELATIONFLG0, :RELATIONFLG1) ")
                .Append("        AND CLMD.CMPL_DETAIL_ID = ( ")
                .Append("                 SELECT MAX(CLMDM.CMPL_DETAIL_ID) ")
                .Append("                   FROM TB_T_COMPLAINT_DETAIL CLMDM ")
                .Append("                  WHERE CLM.CMPL_ID = CLMDM.CMPL_ID ")
                .Append("            ) ")
                .Append("        AND ( ")
                .Append("                 CLM.CMPL_STATUS IN (:CLAIMSTATUS1, :CLAIMSTATUS2) ")
                .Append("                 OR ( ")
                .Append("                          CLM.CMPL_STATUS = :CLAIMSTATUS3 ")
                .Append("                      AND CLMD.FIRST_LAST_ACT_TYPE = :CLAIMSTATUS2 ")
                .Append("                      AND CLMD.ACT_DATETIME >= :ACTUALDATE ")
                .Append("                   ) ")
                .Append("            ) ")
                .Append("     UNION ALL ")
                .Append("     SELECT ")
                .Append("            COUNT(1) AS CLAIMCOUNT ")
                .Append("       FROM TB_H_COMPLAINT CLM ")
                .Append("          , TB_H_COMPLAINT_DETAIL CLMD ")
                .Append("          , TB_H_REQUEST TR ")
                .Append("      WHERE CLM.CMPL_ID = CLMD.CMPL_ID ")
                .Append("        AND CLM.REQ_ID = TR.REQ_ID ")
                .Append("        AND TR.CST_ID = :INSDID ")
                .Append("        AND CLM.RELATION_TYPE IN (:RELATIONFLG0, :RELATIONFLG1) ")
                .Append("        AND CLMD.CMPL_DETAIL_ID = ( ")
                .Append("                 SELECT MAX(CLMDM.CMPL_DETAIL_ID) ")
                .Append("                   FROM TB_H_COMPLAINT_DETAIL CLMDM ")
                .Append("                  WHERE CLM.CMPL_ID = CLMDM.CMPL_ID ")
                .Append("            ) ")
                .Append("        AND ( ")
                .Append("                 CLM.CMPL_STATUS IN (:CLAIMSTATUS1, :CLAIMSTATUS2) ")
                .Append("                 OR ( ")
                .Append("                          CLM.CMPL_STATUS = :CLAIMSTATUS3 ")
                .Append("                      AND CLMD.FIRST_LAST_ACT_TYPE = :CLAIMSTATUS2 ")
                .Append("                      AND CLMD.ACT_DATETIME >= :ACTUALDATE ")
                .Append("                   ) ")
                .Append("            ) ")
                .Append(" ) ")
                ' $01 end
            End With

            'DbSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of VisitUtilityDataSet.VisitUtilityClaimCountDataTable)("VisitUtility_001")
                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("INSDID", OracleDbType.Char, customerCode)
                query.AddParameterWithTypeValue("ACTUALDATE", OracleDbType.Date, completeDate)
                ' $01 start 型変換
                query.AddParameterWithTypeValue("CLAIMSTATUS1", OracleDbType.NVarchar2, ClaimStatusFirst)
                query.AddParameterWithTypeValue("CLAIMSTATUS2", OracleDbType.NVarchar2, ClaimStatusLast)
                query.AddParameterWithTypeValue("CLAIMSTATUS3", OracleDbType.NVarchar2, ClaimStatusComplete)
                query.AddParameterWithTypeValue("RELATIONFLG0", OracleDbType.NVarchar2, RelationFlgOff)
                query.AddParameterWithTypeValue("RELATIONFLG1", OracleDbType.NVarchar2, RelationFlgOn)
                ' $01 end 型変換

                Dim hasClaim As Boolean = False

                If query.GetData()(0).CLAIMCOUNT > 0 Then
                    hasClaim = True
                End If

                Return hasClaim
            End Using

        End Function
#End Region

#Region "TBL_USERSから一覧を取得"
        ''' <summary>
        ''' TBL_USERSから一覧を取得します。
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="operationCodeList">オペレーションコード</param>
        ''' <param name="presenceCategoryList">在席状態</param>
        ''' <param name="deleteCode">削除フラグ</param>
        ''' <param name="account">アカウント</param>
        ''' <returns>VisitUtilityUsersDataTable</returns>
        ''' <remarks>
        ''' TBL_USERSから一覧を取得します。
        ''' </remarks>
        Public Shared Function GetUsers(ByVal dealerCode As String,
                                        ByVal storeCode As String,
                                        ByVal operationCodeList As List(Of Decimal),
                                        ByVal presenceCategoryList As List(Of String),
                                        ByVal deleteCode As String,
                                        Optional ByVal account As String = Nothing) As VisitUtilityDataSet.VisitUtilityUsersDataTable

            Using query As New DBSelectQuery(Of VisitUtilityDataSet.VisitUtilityUsersDataTable)("VisitUtility_002")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* VisitUtility_002 */ ")
                    .Append("        ACCOUNT, ")
                    .Append("        DLRCD, ")
                    .Append("        STRCD, ")
                    .Append("        OPERATIONCODE, ")
                    .Append("        PRESENCECATEGORY, ")
                    ' $01 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
                    .Append("        USERNAME")
                    ' $01 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発
                    .Append("   FROM ")
                    .Append("        TBL_USERS ")
                    .Append("  WHERE ")
                    .Append("        DLRCD = :DLRCD ")
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                    If Not String.IsNullOrEmpty(storeCode) Then
                        .Append("    AND STRCD = :STRCD ")
                        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                    End If
                    If (operationCodeList IsNot Nothing) AndAlso (0 < operationCodeList.Count) Then
                        .Append("    AND OPERATIONCODE IN (")
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
                    If (presenceCategoryList IsNot Nothing) AndAlso (0 < presenceCategoryList.Count) Then
                        .Append("    AND PRESENCECATEGORY IN (")
                        Dim j As Integer = 1
                        For Each presenceCateory As String In presenceCategoryList
                            .Append(" :PRESENCECATEGORY" & CStr(j))
                            query.AddParameterWithTypeValue("PRESENCECATEGORY" & CStr(j), OracleDbType.Char, presenceCateory)
                            If Not presenceCategoryList.Count() = j Then
                                .Append(",")
                            End If
                            j = j + 1
                        Next
                        .Append(" ) ")

                    End If
                    If Not String.IsNullOrEmpty(deleteCode) Then
                        .Append("    AND DELFLG = :DELFLG ")
                        query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, deleteCode)
                    End If
                    ' $01 START 次世代e-CRBセールス機能 新DB適応に向けた機能開発
                    If Not String.IsNullOrEmpty(account) Then
                        .Append(" AND ACCOUNT = :ACCOUNT ")
                        query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                    End If
                    ' $01 END   次世代e-CRBセールス機能 新DB適応に向けた機能開発
                End With

                query.CommandText = sql.ToString()

                Return query.GetData()

            End Using

        End Function
#End Region

        ' $01 start 再構築環境テーブルからの設定値取得
#Region "環境設置値取得"

        ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
        ' ''' <summary>
        ' ''' TB_M_SYSTEM_SETTING_DLRから設定値を取得します。
        ' ''' </summary>
        ' ''' <param name="settingName">削除フラグ</param>
        ' ''' <returns>VisitUtilityGateWayDomainDataTable</returns>
        ' ''' <remarks>
        ' ''' TB_M_SYSTEM_SETTING_DLRから設定値をを取得します。
        ' ''' </remarks>
        'Public Function GetSystemSettingDealer(ByVal settingName As String) As VisitUtilityDataSet.VisitUtilityGateWayDomainDataTable

        '    Dim sql As New StringBuilder

        '    With sql
        '        .Append("SELECT ")
        '        .Append("       SETTING_VAL ")
        '        .Append("  FROM ")
        '        .Append("       TB_M_SYSTEM_SETTING ")
        '        .Append(" WHERE SETTING_NAME = :SETTINGNAME ")
        '    End With

        '    'DbSelectQueryインスタンス生成
        '    Using query As New DBSelectQuery(Of VisitUtilityDataSet.VisitUtilityGateWayDomainDataTable)("VisitUtility_003")
        '        query.CommandText = sql.ToString()

        '        'SQLパラメータ設定
        '        query.AddParameterWithTypeValue("SETTINGNAME", OracleDbType.Char, settingName)
        '        Return query.GetData()
        '    End Using
        'End Function
        ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END
#End Region
        ' $01 end 再構築環境テーブルからの設定値取得

    End Class
End Namespace

Partial Class VisitUtilityDataSet
End Class
