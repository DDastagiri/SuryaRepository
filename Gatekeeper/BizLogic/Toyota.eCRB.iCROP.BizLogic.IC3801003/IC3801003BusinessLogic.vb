'/*******************************************************************
' * COPYRIGHT (C) 2012 TOYOTA MOTOR CORPORATION All Rights Reserved *
' * Release Version xxx.xxx                                         *
' * History:                                                        *
' * 2012-1  Create by NEC.朱云霎・                                  *
' *******************************************************************/
Imports System.Xml
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801003.IC3801003DataSet
Public Class IC3801003BusinessLogic
    Inherits BaseBusinessComponent

    Public Sub New()
        '初始化

    End Sub

    'Public Function GetNoDeliveryROList(ByVal dlrCD As String, ByVal saCode As String) As IC3801003NoDeliveryRODataTable

    '    '定荵唄A別未納車R/O的数据集
    '    Dim dtIC3801003NoDeliveryRODataTable As IC3801003NoDeliveryRODataTable = New IC3801003NoDeliveryRODataTable
    '    dtIC3801003NoDeliveryRODataTable.Dispose()

    '    '定荵沿齪｢荳ｴ譌ｶ蜿・ﾊ 邀ｻ型荳ｺIC3801003TableAdapter  ,并螳棊瘟ｻ
    '    Dim adptIC3801003TableAdapter As IC3801003TableAdapter = New IC3801003TableAdapter
    '    '隹・pIC3801003TableAdapter的方法闔ｷ得RO相蜈ｳ信息数据集
    '    Dim dtIC3801003OrderInfoDataTable As IC3801003OrderInfoDataTable = adptIC3801003TableAdapter.GetOrderInfo(dlrCD, saCode)

    '    '蟇ｹ取得的RO相蜈ｳ信息数据集霑寫s空判定
    '    If dtIC3801003OrderInfoDataTable.Rows.Count <= 0 Then
    '        '若空的隸掾C返回空的SA別未納車R/O的数据集
    '        Return dtIC3801003NoDeliveryRODataTable
    '    End If

    '    '循邇ｯ扈儡A別未納車R/O的数据集襍句ｼ
    '    For i = 0 To dtIC3801003OrderInfoDataTable.Rows.Count - 1
    '        '定荵唄A別未納車R/O的数据集的蟇ｹ蠎箔I数据行
    '        Dim drIC3801003NoDeliveryRORow As IC3801003NoDeliveryRORow = CType(dtIC3801003NoDeliveryRODataTable.NewRow, IC3801003NoDeliveryRORow)
    '        '定荵嘘O相蜈ｳ信息数据行并襍句ｼ
    '        Dim drIC3801003OrderInfoRow As IC3801003OrderInfoRow = CType(dtIC3801003OrderInfoDataTable.Rows(i), IC3801003OrderInfoRow)
    '        '定荵芽執得追加作荳壽ｻ数的方法的参数
    '        Dim varDealerCode As String = drIC3801003OrderInfoRow.DEALERCODE
    '        Dim varOrderNo As String = drIC3801003OrderInfoRow.ORDERNO
    '        '隹・pIC3801003TableAdapterd的方法闔ｷ取追加作荳壽ｻ数的数据集
    '        Dim dtIC3801003AddSrvCountDataTable As IC3801003AddSrvCountDataTable = adptIC3801003TableAdapter.GetAddSrvCount(varDealerCode, varOrderNo)

    '        '扈儡A別未納車R/O的数据行襍句ｼ
    '        drIC3801003NoDeliveryRORow.ORDERNO = drIC3801003OrderInfoRow.ORDERNO
    '        drIC3801003NoDeliveryRORow.ORDERSTATUS = drIC3801003OrderInfoRow.ORDERSTATUS
    '        '根据蟇ｹ蛻除譬㍽ｦ是否荳ｺ1与隶｢蜊募唖除日期是否荳ｺ空的判定扈吝唖除譬㍽ｦ做襍句ｼ
    '        If drIC3801003OrderInfoRow.DELETEFLAG.Equals("1") And Not drIC3801003OrderInfoRow.Is_ORDERCANCELDATE_Null() Then
    '            drIC3801003NoDeliveryRORow.CANCELFLG = "1"
    '        Else
    '            drIC3801003NoDeliveryRORow.CANCELFLG = "0"
    '        End If
    '        drIC3801003NoDeliveryRORow.CUSTOMERID = drIC3801003OrderInfoRow.CUSTOMERID
    '        drIC3801003NoDeliveryRORow.IFLAG = drIC3801003OrderInfoRow.JDPFLAG
    '        drIC3801003NoDeliveryRORow.SFLAG = drIC3801003OrderInfoRow.SFLAG
    '        drIC3801003NoDeliveryRORow.CUSTOMERNAME = drIC3801003OrderInfoRow.CUSTOMERNAME
    '        drIC3801003NoDeliveryRORow.REGISTERNO = drIC3801003OrderInfoRow.REGISTERNO
    '        '根据追加作荳壽ｻ数的数据集是否荳ｺ空的判定，扈剪ﾇ加作荳壽ｻ数襍句ｼ
    '        If dtIC3801003AddSrvCountDataTable.Rows.Count <= 0 Then
    '            drIC3801003NoDeliveryRORow.ADDSRVCOUNT = "0"
    '        End If
    '        drIC3801003NoDeliveryRORow.ADDSRVCOUNT = CType(dtIC3801003AddSrvCountDataTable.Rows(0)("ADDSRVCOUNT"), String)
    '        '扈儡A別未納車R/O数据集襍句ｼ
    '        dtIC3801003NoDeliveryRODataTable.AddIC3801003NoDeliveryRORow(drIC3801003NoDeliveryRORow)
    '    Next
    '    Return dtIC3801003NoDeliveryRODataTable
    'End Function
    ''' <summary>
    ''' SA別未納車R/O一覧数据集的返回 .
    ''' </summary>
    ''' <param name="dlrCD">扈城楳店代遐‥lrCD</param>
    ''' <param name="saCode">閨訣H代遐《aCode</param>
    ''' <remarks></remarks>
    Public Function GetNoDeliveryROList(ByVal dlrcd As String, ByVal saCode As String, Optional ByVal isRez As String = "") As IC3801003NoDeliveryRODataTable

        Dim dt As IC3801003NoDeliveryRODataTable

        Dim da As New IC3801003TableAdapter
        dt = da.GetNoDeliveryROList(dlrcd, saCode)

        For Each row As IC3801003NoDeliveryRORow In dt
            row.GetType()
        Next

        Return dt

    End Function
End Class
