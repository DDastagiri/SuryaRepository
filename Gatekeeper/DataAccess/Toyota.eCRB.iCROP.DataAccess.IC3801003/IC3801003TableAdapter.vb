'/*******************************************************************
' * COPYRIGHT (C) 2012 TOYOTA MOTOR CORPORATION All Rights Reserved *
' * Release Version xxx.xxx                                         *
' * History:                                                        *
' * 2012-1  Create by NEC.朱云辉                                   *
' *******************************************************************/
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003.IC3801003DataSet
Public Class IC3801003TableAdapter
    Inherits Global.System.ComponentModel.Component
    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
    End Sub
#Region "SA別未納車R/O一覧"
    ''' <summary>
    ''' 未交车的订单相关信息
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function GetOrderInfo(ByVal dealerCode As String, ByVal saCode As String) As IC3801003DataSet.IC3801003OrderInfoDataTable
        Using query As New DBSelectQuery(Of IC3801003DataSet.IC3801003OrderInfoDataTable)("IC3801003_001", DBQueryTarget.DMS)
            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append(" SELECT /* IC3801003_001 */")
                .Append("        T1.SALESDATE")
                .Append("      	,T1.ORDERCANCELDATE")
                .Append("      	,T1.DELETEFLAG")
                .Append("      	,T1.DEALERCODE")
                .Append("      	,T1.ORDERNO")
                .Append("      	,T1.SACODE")
                .Append("      	,T1.CUSTOMERID")
                .Append("      	,T1.CUSTOMERNAME")
                .Append("      	,T1.REGISTERNO")
                .Append("      	,T1.JDPFLAG")
                .Append("      	,T1.ORDERSTATUS")
                .Append("      	,T2.SFLAG")
                .Append("   FROM SRV_ORDER_F  T1")
                .Append("      	,SRV_ORDERADD_F T2	")
                .Append("  WHERE T1.ORDERNO = T2.ORDERNO(+)")
                .Append("    AND T1.DEALERCODE = T2.DEALERCODE(+)")
                .Append("    AND T1.DEALERCODE = :DEALERCODE")
                .Append("    AND T1.SACODE = :SACODE")
                .Append("    AND T1.ORDERSTATUS <> '8'")
                .Append("    AND T1.ORDERSTATUS <> '3'")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DEALERCODE", OracleDbType.Char, dealerCode)
            query.AddParameterWithTypeValue("SACODE", OracleDbType.Char, saCode)
            '検索結果返却
            Return query.GetData()
        End Using
    End Function

    ''' <summary>
    ''' 追加作业数的获得
    ''' </summary>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Function GetAddSrvCount(ByVal dealerCode As String, ByVal orderNo As String) As IC3801003AddSrvCountDataTable
        Using query As New DBSelectQuery(Of IC3801003AddSrvCountDataTable)("IC3801003_002", DBQueryTarget.DMS)
            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append(" SELECT /* IC3801003_002 */")
                .Append("        count(*) AS ADDSRVCOUNT")
                .Append("   FROM SRV_SRVRESULTADD_F	")
                .Append("  WHERE NVL(DELETEFLAG,'0') <> '1'")
                .Append("    AND DEALERCODE = :DEALERCODE 	")
                .Append("    AND ORDERNO = :ORDERNO ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DEALERCODE", OracleDbType.Char, dealerCode)
            query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, orderNo)
            '検索結果返却
            Return query.GetData()
        End Using
    End Function

    'debug
    Public Function GetNoDeliveryROList(ByVal dlrcd As String, ByVal saCode As String) As IC3801003NoDeliveryRODataTable

        Using query As New DBSelectQuery(Of IC3801003NoDeliveryRODataTable)("IC3801003_001")

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .Append(" SELECT /* IC3801003_001 */")
                .Append("        DLRCD")
                .Append("      , STRCD")
                .Append("      , SACODE")
                .Append("      , ORDERNO")
                .Append("      , ORDERNO_STATUS      AS ORDERSTATUS")
                .Append("      , SETTLEMENT_CANCEL   AS CANCELFLG")
                .Append("      , CUSTOMERCODE        AS CUSTOMERID")
                .Append("      , JDP_MARK            AS IFLAG")
                .Append("      , SSC_MARK            AS SFLAG")
                .Append("      , CUSTOMERNAME")
                .Append("      , VCLREGNO            AS REGISTERNO")
                .Append("      , APPROVAL_COUNT      AS ADDSRVCOUNT")
                .Append("      , DELIVERYHOPEDATE")
                .Append("   FROM TEST_SA_IF_MAIN")
                .Append("  WHERE DLRCD = '").Append(dlrcd).Append("'")
                .Append("    AND SACODE = '").Append(saCode + "@" + dlrcd).Append("'")
            End With

            query.CommandText = sql.ToString()

            Return query.GetData()


        End Using

    End Function
#End Region
End Class
