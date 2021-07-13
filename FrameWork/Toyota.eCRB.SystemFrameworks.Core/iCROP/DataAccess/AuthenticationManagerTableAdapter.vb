Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Globalization
Imports System.Reflection

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

    Public NotInheritable Class AuthenticationManagerTableAdapter

        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 業務端末識別子-IPアドレス文字列
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TYPE_IP As String = "1"

        ' 2014/02/26 TCS 葛西 TR-V4-GTMC140207005 START
        ''' <summary>
        ''' 業務端末識別子-デバイス識別子(UUID）文字列
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TYPE_MAC As String = "3"
        ' 2014/02/26 TCS 葛西 TR-V4-GTMC140207005 END
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        Private Sub New()

        End Sub

        Public Shared Function IsUser(ByVal account As String, ByVal pass As String) As Integer

            Using query As New DBSelectQuery(Of DataTable)("AUTHENTICATIONMANAGER_001")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* AUTHENTICATIONMANAGER_001 */ ")
                    .Append("        COUNT(1) ")
                    .Append("   FROM TBL_USERS A ")
                    .Append("  WHERE A.ACCOUNT = :ACCOUNT ")
                    .Append("    AND A.PASSWORD = :PASSWORD ")
                    .Append("    AND A.DELFLG = '0' ")
                    .Append("    AND ROWNUM <= 1 ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
                query.AddParameterWithTypeValue("PASSWORD", OracleDbType.NVarchar2, pass)

                Return query.GetCount()

            End Using

        End Function

        Public Shared Function GetClientIPData(ByVal clip As String, ByVal clip_c As String) As AuthenticationManagerDataSet.TBL_CLIENT_IPDataTable

            Using query As New DBSelectQuery(Of AuthenticationManagerDataSet.TBL_CLIENT_IPDataTable)("AUTHENTICATIONMANAGER_002")

                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_Start, clip:[{1}], clip_c:[{2}]",
                                          MethodBase.GetCurrentMethod.Name,
                                          clip, clip_c))
                ' ======================== ログ出力 終了 ========================
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

                Dim sql As New StringBuilder

                With sql
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    .Append(" SELECT /* AUTHENTICATIONMANAGER_002 */ ")
                    .Append("        CLIENT_IDENTITY_VAL AS IP ")
                    .Append("      , DLR_CD AS DLRCD ")
                    .Append("   FROM TB_M_CLIENT ")
                    .Append("  WHERE CLIENT_IDENTITY_VAL IN (:CLIP,:CLIP_C) ")
                    .Append("    AND CLIENT_IDENTITY_TYPE = :TYPE_IP ")
                    .Append("  ORDER BY CLIENT_IDENTITY_VAL DESC ")
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                End With

                query.CommandText = sql.ToString()
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                query.AddParameterWithTypeValue("CLIP", OracleDbType.NVarchar2, clip)
                query.AddParameterWithTypeValue("CLIP_C", OracleDbType.NVarchar2, clip_c)
                query.AddParameterWithTypeValue("TYPE_IP", OracleDbType.NVarchar2, TYPE_IP)

                Dim dt As AuthenticationManagerDataSet.TBL_CLIENT_IPDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            End Using

        End Function

        Public Shared Function GetLoginTimeData(ByVal account As String, ByVal strcd As String) As AuthenticationManagerDataSet.LOGINTIMEDataTable

            Using query As New DBSelectQuery(Of AuthenticationManagerDataSet.LOGINTIMEDataTable)("AUTHENTICATIONMANAGER_003")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* AUTHENTICATIONMANAGER_003 */ ")
                    .Append("        B.LOGIN_STARTTIME ")
                    .Append("      , B.LOGIN_ENDTIME ")
                    .Append("   FROM TBL_USERS A ")
                    .Append("       ,TBL_OPERATIONTYPE B ")
                    .Append("  WHERE A.DLRCD = B.DLRCD ")
                    .Append("    AND A.OPERATIONCODE = B.OPERATIONCODE ")
                    .Append("    AND A.ACCOUNT = :ACCOUNT ")
                    .Append("    AND B.STRCD = :STRCD ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)

                Return query.GetData()

            End Using

        End Function

        Public Shared Function GetClientMacAddressData(ByVal macAddress As String) As AuthenticationManagerDataSet.TBL_CLIENT_MACADDRESSDataTable

            Using query As New DBSelectQuery(Of AuthenticationManagerDataSet.TBL_CLIENT_MACADDRESSDataTable)("AUTHENTICATIONMANAGER_004")

                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_Start, macAddress:[{1}]",
                                          MethodBase.GetCurrentMethod.Name,
                                          macAddress))
                ' ======================== ログ出力 終了 ========================
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

                Dim sql As New StringBuilder

                With sql
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    .Append(" SELECT /* AUTHENTICATIONMANAGER_004 */ ")
                    .Append("        CLIENT_IDENTITY_VAL AS MACADDRESS ")
                    .Append("      , DLR_CD AS DLRCD")
                    .Append("   FROM TB_M_CLIENT ")
                    .Append("  WHERE CLIENT_IDENTITY_VAL = :MACADDRESS ")
                    .Append("    AND CLIENT_IDENTITY_TYPE = :TYPE_MAC ")
                    .Append("  ORDER BY ROW_UPDATE_DATETIME DESC ")
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("MACADDRESS", OracleDbType.Char, macAddress)
                query.AddParameterWithTypeValue("TYPE_MAC", OracleDbType.NVarchar2, TYPE_MAC)

                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                Dim dt As AuthenticationManagerDataSet.TBL_CLIENT_MACADDRESSDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            End Using

        End Function

        Public Shared Function InsertClientMacAddress(ByVal account As String, ByVal macAddress As String, ByVal dlrCD As String) As Integer

            Using query As New DBUpdateQuery("AUTHENTICATIONMANAGER_005")

                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_Start, account:[{1}], macAddress:[{2}], dlrCD:[{3}]",
                                          MethodBase.GetCurrentMethod.Name,
                                          account,
                                          macAddress,
                                          dlrCD))
                ' ======================== ログ出力 終了 ========================
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

                Dim sql As New StringBuilder

                With sql
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    .Append(" INSERT INTO TB_M_CLIENT /* AUTHENTICATIONMANAGER_005 */ ")
                    .Append("( ")
                    .Append("    DLR_CD ")
                    .Append("  , CLIENT_IDENTITY_VAL ")
                    .Append("  , CLIENT_IDENTITY_TYPE ")
                    .Append("  , LAST_LOGIN_STF_CD ")
                    .Append("  , LAST_LOGIN_DATETIME ")
                    .Append("  , ROW_CREATE_DATETIME ")
                    .Append("  , ROW_CREATE_ACCOUNT ")
                    .Append("  , ROW_CREATE_FUNCTION ")
                    .Append("  , ROW_UPDATE_DATETIME ")
                    .Append("  , ROW_UPDATE_ACCOUNT ")
                    .Append("  , ROW_UPDATE_FUNCTION ")
                    .Append("  , ROW_LOCK_VERSION ")
                    .Append(") ")
                    .Append("VALUES ")
                    .Append("( ")
                    .Append("    :DLRCD ")
                    .Append("  , :MACADDRESS ")
                    ' 2014/02/26 TCS 葛西 TR-V4-GTMC140207005 START
                    .Append("  , '3' ")
                    ' 2014/02/26 TCS 葛西 TR-V4-GTMC140207005 END
                    .Append("  , ' ' ")
                    .Append("  , TO_DATE('1900/01/01 00:00:00', 'YYYY/MM/DD HH24:MI:SS') ")
                    .Append("  , SYSDATE ")
                    .Append("  , :ACCOUNT ")
                    .Append("  , 'FRAMEWORK' ")
                    .Append("  , SYSDATE ")
                    .Append("  , :ACCOUNT ")
                    .Append("  , 'FRAMEWORK' ")
                    .Append("  , 0 ")
                    .Append(") ")
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("MACADDRESS", OracleDbType.NVarchar2, macAddress)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrCD)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)

                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                Dim rtn As Integer = query.Execute()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, rtn.ToString(CultureInfo.CurrentCulture)))
                ' ======================== ログ出力 終了 ========================

                Return rtn
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
            End Using

        End Function

        ''' <summary>
        ''' ステータス状況取得
        ''' </summary>
        ''' <param name="account"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function SelectStatusMacaddress(ByVal account As String) As AuthenticationManagerDataSet.STATUS_MACADDRESSDataTable

            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, account:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      account))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            Using query As New DBSelectQuery(Of AuthenticationManagerDataSet.STATUS_MACADDRESSDataTable)("AUTHENTICATIONMANAGER_006")

                Dim sql As New StringBuilder

                With sql
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    .Append(" SELECT /* AUTHENTICATIONMANAGER_006 */ ")
                    .Append("        NVL(T1.PRESENCECATEGORY,0) AS PRESENCECATEGORY ")
                    .Append("      , NVL(T1.PRESENCEDETAIL,0) AS PRESENCEDETAIL ")
                    .Append("      , T2.CLIENT_IDENTITY_VAL AS MACADDRESS")
                    .Append("   FROM TBL_USERS T1 ")
                    .Append("      , TB_M_CLIENT T2 ")
                    .Append("  WHERE T1.ACCOUNT = T2.LAST_LOGIN_STF_CD ")
                    ' 2014/02/26 TCS 葛西 TR-V4-GTMC140207005 対応 START
                    .Append("    AND CLIENT_IDENTITY_TYPE = :TYPE_MAC ")
                    ' 2014/02/26 TCS 葛西 TR-V4-GTMC140207005 対応 END
                    .Append("    AND T1.ACCOUNT = :ACCOUNT ")
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Char, account)
                ' 2014/02/26 TCS 葛西 TR-V4-GTMC140207005 対応 START
                query.AddParameterWithTypeValue("TYPE_MAC", OracleDbType.NVarchar2, TYPE_MAC)
                ' 2014/02/26 TCS 葛西 TR-V4-GTMC140207005 対応 END

                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                Dim dt As AuthenticationManagerDataSet.STATUS_MACADDRESSDataTable = query.GetData()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, dt.Rows.Count))
                ' ======================== ログ出力 終了 ========================

                Return dt
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            End Using
        End Function

        ''' <summary>
        ''' 使用中ユーザアカウントの更新
        ''' </summary>
        ''' <param name="account">ユーザアカウント</param>
        ''' <param name="macAddress">macAddress</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        Public Shared Function UpdateClientMacAddress(ByVal account As String, ByVal macAddress As String) As Integer

            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, account:[{1}], macAddress:[{2}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      account,
                                      macAddress))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            Using query As New DBUpdateQuery("AUTHENTICATIONMANAGER_007")

                Dim sql As New StringBuilder

                With sql
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    .Append(" UPDATE TB_M_CLIENT SET /* AUTHENTICATIONMANAGER_007 */ ")
                    .Append("        LAST_LOGIN_STF_CD = :ACCOUNT ")
                    .Append("      , LAST_LOGIN_DATETIME = SYSDATE ")
                    .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .Append("  WHERE CLIENT_IDENTITY_VAL = :MACADDRESS ")
                    .Append("    AND CLIENT_IDENTITY_TYPE = :TYPE_MAC ")
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("MACADDRESS", OracleDbType.NVarchar2, macAddress)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
                query.AddParameterWithTypeValue("TYPE_MAC", OracleDbType.NVarchar2, TYPE_MAC)

                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                Dim rtn As Integer = query.Execute()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, rtn.ToString(CultureInfo.CurrentCulture)))
                ' ======================== ログ出力 終了 ========================

                Return rtn
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            End Using
        End Function

        ''' <summary>
        ''' [TB_M_CLIENT]の使用中アカウントのクリア
        ''' </summary>
        ''' <param name="account"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ClearClientMacAddress(ByVal account As String) As Integer

            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, account:[{1}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      account))
            ' ======================== ログ出力 終了 ========================
            ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            Using query As New DBUpdateQuery("AUTHENTICATIONMANAGER_008")

                Dim sql As New StringBuilder

                With sql
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                    .Append(" UPDATE TB_M_CLIENT SET /* AUTHENTICATIONMANAGER_008 */ ")
                    .Append("        LAST_LOGIN_STF_CD = ' ' ")
                    .Append("      , LAST_LOGIN_DATETIME = TO_DATE('1900/01/01 00:00:00', 'YYYY/MM/DD HH24:MI:SS') ")
                    .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
                    .Append("  WHERE LAST_LOGIN_STF_CD = :ACCOUNT ")
                    ' 2014/02/26 TCS 葛西 TR-V4-GTMC140207005 対応 START
                    .Append("    AND CLIENT_IDENTITY_TYPE = :TYPE_MAC ")
                    ' 2014/02/26 TCS 葛西 TR-V4-GTMC140207005 対応 END
                    ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
                ' 2014/02/26 TCS 葛西 TR-V4-GTMC140207005 対応 START
                query.AddParameterWithTypeValue("TYPE_MAC", OracleDbType.NVarchar2, TYPE_MAC)
                ' 2014/02/26 TCS 葛西 TR-V4-GTMC140207005 対応 END

                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
                Dim rtn As Integer = query.Execute()

                ' ======================== ログ出力 開始 ========================
                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                          " {0}_End, Return:[{1}]",
                                          MethodBase.GetCurrentMethod.Name, rtn.ToString(CultureInfo.CurrentCulture)))
                ' ======================== ログ出力 終了 ========================

                Return rtn
                ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

            End Using
        End Function

        ''' <summary>
        ''' (PC用)重複ログインチェック
        ''' </summary>
        ''' <param name="account">ユーザアカウント</param>
        ''' <returns>件数</returns>
        ''' <remarks></remarks>
        Public Shared Function CheckStatus(ByVal account As String) As Integer

            Using query As New DBSelectQuery(Of DataTable)("AUTHENTICATIONMANAGER_009")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* AUTHENTICATIONMANAGER_009 */ ")
                    .Append("        COUNT(1) ")
                    .Append("   FROM TBL_USERS A ")
                    .Append("  WHERE A.ACCOUNT = :ACCOUNT ")
                    .Append("    AND A.PRESENCECATEGORY= '4' ")
                    .Append("    AND A.PRESENCEDETAIL= '0' ")
                    .Append("    AND ROWNUM <= 1 ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)

                Return query.GetCount()

            End Using

        End Function
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 START
        ''' <summary>
        ''' 業務端末マスタのロック取得
        ''' </summary>
        ''' <param name="macAddress">macAddress</param>
        ''' <param name="account">ユーザアカウント</param>
        ''' <remarks></remarks>
        Public Shared Sub GetClientLock(ByVal macAddress As String, ByVal account As String)

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_Start, macAddress:[{1}], account:[{2}]",
                                      MethodBase.GetCurrentMethod.Name,
                                      macAddress,
                                      account))
            ' ======================== ログ出力 終了 ========================

            Using query As New DBSelectQuery(Of DataTable)("AUTHENTICATIONMANAGER_010")

                Dim env As New SystemEnvSetting
                Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* AUTHENTICATIONMANAGER_010 */ ")
                    .Append("        1 ")
                    .Append("   FROM TB_M_CLIENT ")
                    .Append("  WHERE (CLIENT_IDENTITY_VAL = :MACADDRESS ")
                    .Append("    AND CLIENT_IDENTITY_TYPE = :TYPE_MAC) ")
                    .Append("     OR LAST_LOGIN_STF_CD = :ACCOUNT ")
                    .Append(sqlForUpdate)
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("MACADDRESS", OracleDbType.NVarchar2, macAddress)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)
                query.AddParameterWithTypeValue("TYPE_MAC", OracleDbType.NVarchar2, TYPE_MAC)
                query.GetData()

            End Using

            ' ======================== ログ出力 開始 ========================
            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                      " {0}_End, Return:[{1}]",
                                      MethodBase.GetCurrentMethod.Name, "1"))
            ' ======================== ログ出力 終了 ========================

        End Sub
        ' 2013/06/30 TCS 武田 2013/10対応版　既存流用 END

        ''' <summary>
        ''' Open AM認証時のユーザ突合
        ''' </summary>
        ''' <param name="account">ユーザアカウント</param>
        ''' <returns>件数</returns>
        ''' <remarks></remarks>
        Public Shared Function IsUser(ByVal account As String) As Integer

            Using query As New DBSelectQuery(Of DataTable)("AUTHENTICATIONMANAGER_011")

                Dim sql As New StringBuilder

                With sql
                    .Append(" SELECT /* AUTHENTICATIONMANAGER_011 */ ")
                    .Append("        COUNT(1) ")
                    .Append("   FROM TBL_USERS A ")
                    .Append("  WHERE A.ACCOUNT = :ACCOUNT ")
                    .Append("    AND A.DELFLG = '0' ")
                    .Append("    AND ROWNUM <= 1 ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, account)

                Return query.GetCount()

            End Using

        End Function

    End Class

End Namespace

