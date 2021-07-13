Imports System.Globalization
Imports System.Reflection.MethodBase
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Namespace SC3100302DataSetTableAdapters
    Public Class SC3100302DataTableTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数/Enum"
        '活動結果見登録
        Private Const NOT_REGISTER As String = "0"
        '受注後工程フォローマスタ-終了工程コード-未選択
        Private Const NOT_SELECT As String = "000"
        '見積情報-契約済
        Private Const DONE_CONTRACT As String = "1"

        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
        Private Const C_FLAG_ON As String = "1"
        Private Const C_FLAG_OFF As String = "0"
        Private Const C_XXXXX As String = "XXXXX"
        Private Const C_CONTACT_MTD As String = "CONTACT_MTD"
        Private Const C_DEVICE_TYPE As String = "01"
        Private Const C_BLANK As String = " "
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
#End Region

#Region "メンバ変数"
        Private DlrCd As String
        Private StrCd As String
        Private UserId As String
#End Region

#Region "コンストラクタ"
        Public Sub New(ByVal dlrcd As String, ByVal strcd As String, ByVal userid As String)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
              "{0} Start > Params:dlrcd=[{1}] strcd=[{2}] userid=[{3}]", _
              GetCurrentMethod().Name, _
              dlrcd, _
              strcd, _
              userid))
            Me.DlrCd = dlrcd
            Me.StrCd = strcd
            Me.UserId = userid
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
        End Sub
#End Region

#Region "メソッド"
        ''' <summary>
        ''' 来店実績一覧取得
        ''' </summary>
        ''' <returns>SC3100302VisitActualDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectVisitActualList() As SC3100302DataSet.SC3100302VisitActualDataTable
            Dim sql As New StringBuilder
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start ", GetCurrentMethod().Name))

            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
            With sql
                .AppendLine(" SELECT /* SC3100302_001 */ ")
                .AppendLine(" /* 前日以前の来店実績 */ ")
                .AppendLine("        SALES.DLRCD ")
                .AppendLine("      , SALES.STRCD ")
                .AppendLine("      , SALES.BRANCH_PLAN ")
                .AppendLine("      , SALES.FLLWUPBOX_SEQNO ")
                .AppendLine("      , TALLY.SALESBKGNO ")
                .AppendLine("      , TALLY.VCLASIDATE ")
                .AppendLine("      , TALLY.VCLDELIDATE ")
                .AppendLine("      , TALLY.WAITING_OBJECT ")
                .AppendLine("      , SALES.ACTUALACCOUNT ")
                .AppendLine("      , USERS.USERNAME TEMP_STAFFNAME ")
                .AppendLine("      , USERS.OPERATIONCODE TEMP_STAFF_OPERATIONCODE ")
                .AppendLine("      , OPE.ICON_IMGFILE TEMP_STAFF_OPERATIONCODE_ICON ")
                .AppendLine("      , SALES.STARTTIME ")
                .AppendLine("      , SALES.ENDTIME ")
                .AppendLine("      , SALES.CUSTSEGMENT ")
                .AppendLine("      , SALES.CUSTOMERCLASS ")
                .AppendLine("      , SALES.CRCUSTID ")
                .AppendLine("      , SALES.REGISTFLG ")
                .AppendLine("      , SALES.ACCOUNT_PLAN ")
                .AppendLine(" FROM   TBL_FLLWUPBOX_SALES SALES ")
                .AppendLine("      , TBL_ESTIMATEINFO EST ")
                .AppendLine("      , TBL_SALESBKGTALLY TALLY ")
                .AppendLine("      , TBL_USERS USERS ")
                .AppendLine("      , TBL_OPERATIONTYPE OPE ")
                .AppendLine(" WHERE  SALES.FLLWUPBOX_SEQNO = EST.FLLWUPBOX_SEQNO(+) ")
                .AppendLine("   AND  EST.DELFLG(+) = '0' ")
                .AppendLine("   AND  EST.CONTRACTFLG(+) = :DONE_CONTRACT ")
                .AppendLine("   AND  EST.DLRCD = TALLY.DLRCD(+) ")
                .AppendLine("   AND  TRIM(EST.CONTRACTNO) = TALLY.SALESBKGNO(+) ")
                .AppendLine("   AND  TALLY.CANCELFLG(+) = '0' ")
                .AppendLine("   AND  TALLY.CUSTDELFLG(+) = '0' ")
                .AppendLine("   AND  TALLY.DELFLG(+) = '0' ")
                .AppendLine("   AND  SALES.ACTUALACCOUNT = USERS.ACCOUNT(+) ")
                .AppendLine("   AND  USERS.OPERATIONCODE = OPE.OPERATIONCODE ")
                .AppendLine("   AND  USERS.DLRCD = OPE.DLRCD ")
                .AppendLine("   AND  OPE.STRCD = '000' ")
                .AppendLine("   AND  SALES.DLRCD = :DLRCD ")
                .AppendLine("   AND  SALES.BRANCH_PLAN = :BRANCH_PLAN ")
                .AppendLine("   AND  SALES.ACCOUNT_PLAN = :ACCOUNT_PLAN ")
                .AppendLine("   AND  SALES.REGISTFLG = '0' ")
                .AppendLine("   AND  SALES.ENDTIME < :NOW ")
                .AppendLine("   AND  SALES.ENDTIME IS NOT NULL ")
                .AppendLine(" UNION ALL ")
                .AppendLine(" SELECT /* 当日の来店実績 */ ")
                .AppendLine("        SALES.DLRCD ")
                .AppendLine("      , SALES.STRCD ")
                .AppendLine("      , SALES.BRANCH_PLAN ")
                .AppendLine("      , SALES.FLLWUPBOX_SEQNO ")
                .AppendLine("      , TALLY.SALESBKGNO ")
                .AppendLine("      , TALLY.VCLASIDATE ")
                .AppendLine("      , TALLY.VCLDELIDATE ")
                .AppendLine("      , TALLY.WAITING_OBJECT ")
                .AppendLine("      , SALES.ACTUALACCOUNT ")
                .AppendLine("      , USERS.USERNAME ")
                .AppendLine("      , USERS.OPERATIONCODE ")
                .AppendLine("      , OPE.ICON_IMGFILE ")
                .AppendLine("      , SALES.STARTTIME ")
                .AppendLine("      , SALES.ENDTIME ")
                .AppendLine("      , SALES.CUSTSEGMENT ")
                .AppendLine("      , SALES.CUSTOMERCLASS ")
                .AppendLine("      , SALES.CRCUSTID ")
                .AppendLine("      , SALES.REGISTFLG ")
                .AppendLine("      , SALES.ACCOUNT_PLAN ")
                .AppendLine(" FROM   TBL_FLLWUPBOX_SALES SALES ")
                .AppendLine("      , TBL_ESTIMATEINFO EST ")
                .AppendLine("      , TBL_SALESBKGTALLY TALLY ")
                .AppendLine("      , TBL_USERS USERS ")
                .AppendLine("      , TBL_OPERATIONTYPE OPE ")
                .AppendLine(" WHERE  SALES.FLLWUPBOX_SEQNO = EST.FLLWUPBOX_SEQNO(+) ")
                .AppendLine("   AND  EST.DELFLG(+) = '0' ")
                .AppendLine("   AND  EST.CONTRACTFLG(+) = :DONE_CONTRACT ")
                .AppendLine("   AND  EST.DLRCD = TALLY.DLRCD(+) ")
                .AppendLine("   AND  TRIM(EST.CONTRACTNO) = TALLY.SALESBKGNO(+) ")
                .AppendLine("   AND  TALLY.CANCELFLG(+) = '0' ")
                .AppendLine("   AND  TALLY.CUSTDELFLG(+) = '0' ")
                .AppendLine("   AND  TALLY.DELFLG(+) = '0' ")
                .AppendLine("   AND  SALES.ACTUALACCOUNT = USERS.ACCOUNT(+) ")
                .AppendLine("   AND  USERS.OPERATIONCODE = OPE.OPERATIONCODE ")
                .AppendLine("   AND  USERS.DLRCD = OPE.DLRCD ")
                .AppendLine("   AND  OPE.STRCD = '000' ")
                .AppendLine("   AND  SALES.DLRCD = :DLRCD ")
                .AppendLine("   AND  SALES.BRANCH_PLAN = :BRANCH_PLAN ")
                .AppendLine("   AND  SALES.ACCOUNT_PLAN = :ACCOUNT_PLAN ")
                .AppendLine("   AND  SALES.ENDTIME >= :NOW ")
                .AppendLine(" ORDER BY REGISTFLG ")
                .AppendLine("        , STARTTIME ")
            End With
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

            Using query As New DBSelectQuery(Of SC3100302DataSet.SC3100302VisitActualDataTable)("SC3100302_001")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, Me.DlrCd)
                query.AddParameterWithTypeValue("BRANCH_PLAN", OracleDbType.Char, Me.StrCd)
                query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.Char, Me.UserId)
                query.AddParameterWithTypeValue("DONE_CONTRACT", OracleDbType.Char, DONE_CONTRACT)
                query.AddParameterWithTypeValue("NOW", OracleDbType.Date, DateTimeFunc.Now(Me.DlrCd).Date)
                Return query.GetData()
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
        End Function

        ''' <summary>
        ''' 接触アイコン取得
        ''' </summary>
        ''' <returns>SC3100302VisitActualDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectContactIcon() As SC3100302DataSet.SC3100302ContactIconDataTable
            Dim sql As New StringBuilder
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start ", GetCurrentMethod().Name))
            With sql
                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
                .AppendLine(" SELECT /* SC3100302_002 */ ")
                .AppendLine("        T1.CONTACT_MTD AS CONTACTNO ")
                .AppendLine("      , NVL(T4.ICON_PATH, T5.ICON_PATH) AS ICONPATH ")
                .AppendLine("   FROM TB_M_CONTACT_MTD T1 ")
                .AppendLine("      , (SELECT T2.FIRST_KEY ")
                .AppendLine("              , T2.ICON_PATH  ")
                .AppendLine("           FROM TB_M_IMG_PATH_CONTROL T2 ")
                .AppendLine("          WHERE T2.DLR_CD = :DLR_CD ")
                .AppendLine("            AND T2.TYPE_CD = :CONTACT_MTD ")
                .AppendLine("            AND T2.DEVICE_TYPE = :DEVICE_TYPE ")
                .AppendLine("            AND T2.SECOND_KEY = :BLANK ) T4 ")
                .AppendLine("      , (SELECT T3.FIRST_KEY ")
                .AppendLine("              , T3.ICON_PATH  ")
                .AppendLine("           FROM TB_M_IMG_PATH_CONTROL T3 ")
                .AppendLine("          WHERE T3.DLR_CD = :XXXXX ")
                .AppendLine("            AND T3.TYPE_CD = :CONTACT_MTD ")
                .AppendLine("            AND T3.DEVICE_TYPE = :DEVICE_TYPE ")
                .AppendLine("            AND T3.SECOND_KEY = :BLANK ) T5 ")
                .AppendLine("   WHERE T1.CONTACT_MTD = T4.FIRST_KEY(+) ")
                .AppendLine("     AND T1.CONTACT_MTD = T5.FIRST_KEY(+) ")
                .AppendLine("     AND T1.INUSE_FLG = :FLAG_ON ")
                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
            End With

            Using query As New DBSelectQuery(Of SC3100302DataSet.SC3100302ContactIconDataTable)("SC3100302_002")
                query.CommandText = sql.ToString()
                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, Me.DlrCd)
                query.AddParameterWithTypeValue("XXXXX", OracleDbType.Char, C_XXXXX)
                query.AddParameterWithTypeValue("CONTACT_MTD", OracleDbType.Char, C_CONTACT_MTD)
                query.AddParameterWithTypeValue("DEVICE_TYPE", OracleDbType.Char, C_DEVICE_TYPE)
                query.AddParameterWithTypeValue("FLAG_ON", OracleDbType.Char, C_FLAG_ON)
                query.AddParameterWithTypeValue("BLANK", OracleDbType.Char, C_BLANK)
                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
                Return query.GetData()
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
        End Function

        ''' <summary>
        ''' 受注後工程アイコン取得
        ''' </summary>
        ''' <returns>SC3100302VisitActualDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectAfterFollowtIcon() As SC3100302DataSet.SC3100302AfterFollowtIconDataTable
            Dim sql As New StringBuilder
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start ", GetCurrentMethod().Name))

            With sql
                .AppendLine("SELECT /* SC3100302_003 */")
                .AppendLine("       STARTPROCESSCD")
                .AppendLine("     , ICON_TODOTIP")
                .AppendLine("  FROM TBL_AFTERFOLLOWSETTING")
                .AppendLine(" WHERE DLRCD = 'XXXXX'")
                .AppendLine("   AND ENDPROCESSCD = :ENDPROCESSCD")
                .AppendLine("   AND DELFLG = '0'")
            End With

            Using query As New DBSelectQuery(Of SC3100302DataSet.SC3100302AfterFollowtIconDataTable)("SC3100302_003")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("ENDPROCESSCD", OracleDbType.Char, NOT_SELECT)
                Return query.GetData()
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
        End Function

        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
        ''' <summary>
        ''' 顧客車両情報取得
        ''' </summary>
        ''' <returns>SC3100302VisitActualDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectCustomerVehicleInfo(ByVal salesid As Decimal) As SC3100302DataSet.SC3100302CustomerNameDataTable
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
            Dim sql As New StringBuilder

            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start Params:salesid=[{1}]", GetCurrentMethod().Name, salesid))

            With sql
                .AppendLine(" SELECT /* SC3100302_004 */ ")
                .AppendLine("        T1.DLR_CD ")
                .AppendLine("      , T2.VCL_ID ")
                .AppendLine("      , T2.CST_ID ")
                .AppendLine("      , T2.REC_CST_VCL_TYPE AS CST_VCL_TYPE ")
                .AppendLine("   FROM TB_T_SALES T1 ")
                .AppendLine("      , TB_T_REQUEST T2 ")
                .AppendLine("  WHERE T1.REQ_ID = T2.REQ_ID ")
                .AppendLine("    AND T1.SALES_ID = :SALES_ID ")
                .AppendLine("  UNION ALL ")
                .AppendLine(" SELECT ")
                .AppendLine("        T3.DLR_CD ")
                .AppendLine("      , T4.VCL_ID ")
                .AppendLine("      , T4.CST_ID ")
                .AppendLine("      , T4.CST_VCL_TYPE ")
                .AppendLine("   FROM TB_T_SALES T3 ")
                .AppendLine("      , TB_T_ATTRACT T4 ")
                .AppendLine("  WHERE T3.ATT_ID = T4.ATT_ID ")
                .AppendLine("    AND T3.SALES_ID = :SALES_ID ")
            End With
            '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

            Using query As New DBSelectQuery(Of SC3100302DataSet.SC3100302CustomerNameDataTable)("SC3100302_004")
                query.CommandText = sql.ToString()

                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesid)
                '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END

                Return query.GetData()
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
        End Function

        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 START
        ''' <summary>
        ''' 顧客車両情報取得(受注後)
        ''' </summary>
        ''' <returns>SC3100302VisitActualDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectCustomerVehicleInfoHistory(ByVal salesid As Decimal) As SC3100302DataSet.SC3100302CustomerNameDataTable
            Dim sql As New StringBuilder

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start Params:salesid=[{1}]", GetCurrentMethod().Name, salesid))

            With sql
                .AppendLine(" SELECT /* SC3100302_007 */ ")
                .AppendLine("        T1.DLR_CD ")
                .AppendLine("      , T2.VCL_ID ")
                .AppendLine("      , T2.CST_ID ")
                .AppendLine("      , T2.REC_CST_VCL_TYPE AS CST_VCL_TYPE ")
                .AppendLine("   FROM TB_H_SALES T1 ")
                .AppendLine("      , TB_H_REQUEST T2 ")
                .AppendLine("  WHERE T1.REQ_ID = T2.REQ_ID ")
                .AppendLine("    AND T1.SALES_ID = :SALES_ID ")
                .AppendLine("  UNION ALL ")
                .AppendLine(" SELECT ")
                .AppendLine("        T3.DLR_CD ")
                .AppendLine("      , T4.VCL_ID ")
                .AppendLine("      , T4.CST_ID ")
                .AppendLine("      , T4.CST_VCL_TYPE ")
                .AppendLine("   FROM TB_H_SALES T3 ")
                .AppendLine("      , TB_H_ATTRACT T4 ")
                .AppendLine("  WHERE T3.ATT_ID = T4.ATT_ID ")
                .AppendLine("    AND T3.SALES_ID = :SALES_ID ")
            End With

            Using query As New DBSelectQuery(Of SC3100302DataSet.SC3100302CustomerNameDataTable)("SC3100302_007")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesid)

                Return query.GetData()
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
        End Function

        ''' <summary>
        ''' 敬称付き顧客名称取得(所有者)
        ''' </summary>
        ''' <returns>SC3100302VisitActualDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectCustomerNameWithNameTitleOwner(ByVal custid As Decimal, ByVal dlrcd As String) As SC3100302DataSet.SC3100302CustomerNameDataTable

            Dim sql As New StringBuilder

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start Params:custid=[{1}]", GetCurrentMethod().Name, custid))

            With sql
                .AppendLine(" SELECT /* SC3100302_005 */ ")
                .AppendLine("        T1.CST_NAME AS NAME ")
                .AppendLine("      , T1.NAMETITLE_NAME AS NAMETITLE ")
                .AppendLine("      , T2.CST_TYPE ")
                .AppendLine("   FROM TB_M_CUSTOMER T1 ")
                .AppendLine("      , TB_M_CUSTOMER_DLR T2 ")
                .AppendLine("  WHERE T1.CST_ID = T2.CST_ID ")
                .AppendLine("    AND T1.CST_ID = :CST_ID ")
                .AppendLine("    AND T2.DLR_CD = :DLR_CD ")
            End With

            Using query As New DBSelectQuery(Of SC3100302DataSet.SC3100302CustomerNameDataTable)("SC3100302_005")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, custid)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)

                Return query.GetData()
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
        End Function

        ''' <summary>
        ''' 敬称付き顧客名称取得(所有者以外)
        ''' </summary>
        ''' <returns>SC3100302VisitActualDataTable</returns>
        ''' <remarks></remarks>
        Public Function SelectCustomerNameWithNameTitleNotOwner(ByVal dlrcd As String, ByVal vclid As Decimal) As SC3100302DataSet.SC3100302CustomerNameDataTable

            Dim sql As New StringBuilder

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Start Params:dlrcd=[{1}] vclid=[{2}]", GetCurrentMethod().Name, dlrcd, vclid))

            With sql
                .AppendLine(" SELECT /* SC3100302_006 */ ")
                .AppendLine("          T1.CST_NAME AS NAME ")
                .AppendLine("        , T1.NAMETITLE_NAME AS NAMETITLE ")
                .AppendLine("        , T2.CST_ID ")
                .AppendLine("        , T3.CST_TYPE ")
                .AppendLine("     FROM TB_M_CUSTOMER T1 ")
                .AppendLine("        , TB_M_CUSTOMER_VCL T2 ")
                .AppendLine("        , TB_M_CUSTOMER_DLR T3 ")
                .AppendLine("    WHERE T1.CST_ID = T2.CST_ID ")
                .AppendLine("      AND T2.DLR_CD = T3.DLR_CD ")
                .AppendLine("      AND T2.CST_ID = T3.CST_ID ")
                .AppendLine("      AND T2.DLR_CD = :DLR_CD ")
                .AppendLine("      AND T2.VCL_ID = :VCL_ID ")
                .AppendLine("      AND T2.CST_VCL_TYPE = :FLAG_ON ")
                .AppendLine("      AND T2.OWNER_CHG_FLG = :FLAG_OFF ")
            End With

            Using query As New DBSelectQuery(Of SC3100302DataSet.SC3100302CustomerNameDataTable)("SC3100302_006")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrcd)
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, vclid)
                query.AddParameterWithTypeValue("FLAG_ON", OracleDbType.Char, C_FLAG_ON)
                query.AddParameterWithTypeValue("FLAG_OFF", OracleDbType.Char, C_FLAG_OFF)

                Return query.GetData()
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} End", GetCurrentMethod().Name))
        End Function
        '2013/06/30 TCS 坂井 2013/10対応版 既存流用 END
#End Region

    End Class

End Namespace


Partial Class SC3100302DataSet
    Partial Class SC3100302VisitActualDataTable

        Private Sub SC3100302VisitActualDataTable_ColumnChanging(sender As System.Object, e As System.Data.DataColumnChangeEventArgs) Handles Me.ColumnChanging
            If (e.Column.ColumnName = Me.CUSTOMERCLASSColumn.ColumnName) Then
                'ユーザー コードをここに追加してください
            End If

        End Sub

    End Class

End Class
