Option Strict On
Option Explicit On

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801001
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801001
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801004
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801004
Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.BizLogic.IC3801110
Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801110
Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.BizLogic.IC3801113
Imports Toyota.eCRB.DMSLinkage.RepairOrderCreate.DataAccess.IC3801113

'2012/03/01 子チップ作業・部品情報取得対応 上田 Start
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801006
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801006
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801007
Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801007
'2012/03/01 子チップ作業・部品情報取得対応 上田 End
Imports System.Text

Public Class SC3150102BusinessLogic

    ''' <summary>
    ''' R/O基本情報の取得処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="repairOrderNumber">オーダーNo.</param>
    ''' <returns>R/O基本情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetRepairOrderBaseData(ByVal dealerCode As String, ByVal repairOrderNumber As String) _
                                                            As IC3801001DataSet.IC3801001OrderCommDataTable

        Logger.Info("GetRepairOrderBaseData Start param1:" + dealerCode + _
                                                " param2:" + repairOrderNumber)

        Dim IC3801001BizLogic As IC3801001BusinessLogic = New IC3801001BusinessLogic
        Dim dt As IC3801001DataSet.IC3801001OrderCommDataTable

        If (String.IsNullOrEmpty(repairOrderNumber)) Then
            'dt = New IC3801001DataSet.IC3801001OrderCommDataTable
            dt = Nothing
        Else
            'R/O基本情報の取得.
            dt = IC3801001BizLogic.GetROBaseInfoList(dealerCode, repairOrderNumber)

        End If

        Logger.Info("GetRepairOrderBaseData End")
        Return dt

    End Function

    '2012/03/01 子チップ作業・部品情報取得対応 上田 Start
    ''' <summary>
    ''' 作業項目情報の取得処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="repairOrderNumber">オーダーNo.</param>
    ''' <param name="childNumber">子予約連番</param>
    ''' <returns>作業項目データ</returns>
    ''' <remarks></remarks>
    Public Function GetServiceDetailData(ByVal dealerCode As String, _
                                         ByVal repairOrderNumber As String, _
                                         ByVal childNumber As String) As IC3801110DataSet.IC3801110SrvDetailDataTableCommDataTable

        Logger.Info("GetServiceDetailData Start param1:" + dealerCode + _
                                              " param2:" + repairOrderNumber + _
                                              " param3:" + childNumber)

        Dim dt As IC3801110DataSet.IC3801110SrvDetailDataTableCommDataTable

        If (String.IsNullOrEmpty(repairOrderNumber)) Then
            dt = Nothing
            '2012/03/03 日比野 ログ出力処理追加 START 
            Logger.Info("ServiceDetailData is Nothing")
            '2012/03/03 日比野 ログ出力処理追加 END
        Else
            If String.IsNullOrEmpty(childNumber) OrElse childNumber = "0" OrElse childNumber = "998" Then ' 子予約連番はすでに「-1」されている
                '関連チップがないチップ 又は、関連チップ(親チップ)
                Dim IC3801110BizLogic As IC3801110BusinessLogic = New IC3801110BusinessLogic

                '作業内容の取得
                dt = IC3801110BizLogic.GetSrvDetailList(dealerCode, repairOrderNumber)

                '2012/03/03 日比野 ログ出力処理追加 START 
                OutPutIFLog(dt, "IC3801110BizLogic.GetSrvDetailList")
                '2012/03/03 日比野 ログ出力処理追加 END
            Else
                '関連チップ(子チップ)
                Dim IC3801006BizLogic As IC3801006BusinessLogic = New IC3801006BusinessLogic
                Dim dtChildInfomation As IC3801006DataSet.IC3801006ServiceDetailInfoDataTable

                Dim addSeq As Integer = 0
                Integer.TryParse(childNumber, addSeq)

                '作業内容取得(子チップ)
                dtChildInfomation = IC3801006BizLogic.GetServiceDetailList(dealerCode, _
                                                                           repairOrderNumber, _
                                                                           addSeq)

                '2012/03/03 日比野 ログ出力処理追加 START 
                OutPutIFLog(dtChildInfomation, "IC3801006BizLogic.GetServiceDetailList")
                '2012/03/03 日比野 ログ出力処理追加 END

                dt = New IC3801110DataSet.IC3801110SrvDetailDataTableCommDataTable

                '戻り値データ用に変換する
                For i = 0 To dtChildInfomation.Rows.Count - 1

                    Dim dr As IC3801110DataSet.IC3801110SrvDetailDataTableCommRow = dt.NewIC3801110SrvDetailDataTableCommRow
                    Dim drChildInfomation As IC3801006DataSet.IC3801006ServiceDetailInfoRow = DirectCast(dtChildInfomation.Rows(i), IC3801006DataSet.IC3801006ServiceDetailInfoRow)

                    With dr
                        .DEALERCODE = drChildInfomation.DealerCode                  '販売店コード
                        .BRNCD = drChildInfomation.BrnCd                            '店舗コード
                        .ORDERNO = drChildInfomation.OrderNo                        '受注NO
                        .SRVNAME = drChildInfomation.SrvName                        '整備名称
                        .HRTYPE = drChildInfomation.HRType                          'HR区分
                        .WORKHOURS = drChildInfomation.WorkHours                    '工数
                        .SELLWORKPRICE = drChildInfomation.SellWorkPrice            '技術料
                        .WORKPRICE = drChildInfomation.WorkPrice                    '技術料(原価)
                    End With

                    '行追加
                    dt.Rows.Add(dr)
                Next

            End If
        End If

        Logger.Info("GetServiceDetailData End")
        Return dt

    End Function

    ''' <summary>
    ''' 部品詳細情報の取得処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="repairOrderNumber">オーダーNo.</param>
    ''' <param name="childNumber">子予約連番</param>
    ''' <returns>部品詳細情報</returns>
    ''' <remarks></remarks>
    Public Function GetPartsDetailData(ByVal dealerCode As String, _
                                       ByVal repairOrderNumber As String, _
                                       ByVal childNumber As String) As IC3801113DataSet.IC3801113PartsDataTable

        Logger.Info("GetPartsDetailData Start param1:" + dealerCode + _
                                            " param2:" + repairOrderNumber + _
                                            " param3:" + childNumber)

        Dim dt As IC3801113DataSet.IC3801113PartsDataTable

        If (String.IsNullOrEmpty(repairOrderNumber)) Then
            dt = Nothing
            '2012/03/03 日比野 ログ出力処理追加 START 
            Logger.Info("PartsDetailData is Nothing")
            '2012/03/03 日比野 ログ出力処理追加 END
        Else
            If String.IsNullOrEmpty(childNumber) OrElse childNumber = "0" OrElse childNumber = "998" Then ' 子予約連番はすでに「-1」されている
                '関連チップがないチップ 又は、関連チップ(親チップ)
                Dim IC3801113BizLogic As IC3801113BusinessLogic = New IC3801113BusinessLogic

                '部品詳細情報の取得
                dt = IC3801113BizLogic.GetSrvPartsDetailList(dealerCode, repairOrderNumber)

                '2012/03/03 日比野 ログ出力処理追加 START
                OutPutIFLog(dt, "IC3801113BizLogic.GetSrvPartsDetailList")
                '2012/03/03 日比野 ログ出力処理追加 END

            Else
                '関連チップ(子チップ)
                Dim IC3801007BizLogic As IC3801007BusinessLogic = New IC3801007BusinessLogic
                Dim dtChildInfomation As IC3801007DataSet.IC3801007PartsDetailInfoDataTable

                Dim addSeq As Integer = 0
                Integer.TryParse(childNumber, addSeq)

                '部品詳細情報の取得
                dtChildInfomation = IC3801007BizLogic.GetPartsDetailList(dealerCode,
                                                                         repairOrderNumber,
                                                                         addSeq)

                '2012/03/03 日比野 ログ出力処理追加 START
                OutPutIFLog(dtChildInfomation, "IC3801007BizLogic.GetPartsDetailList")
                '2012/03/03 日比野 ログ出力処理追加 END

                dt = New IC3801113DataSet.IC3801113PartsDataTable

                '戻り値データ用に変換する
                For i = 0 To dtChildInfomation.Rows.Count - 1

                    Dim dr As IC3801113DataSet.IC3801113PartsRow = dt.NewIC3801113PartsRow
                    Dim drChildInfomation As IC3801007DataSet.IC3801007PartsDetailInfoRow = DirectCast(dtChildInfomation.Rows(i), IC3801007DataSet.IC3801007PartsDetailInfoRow)

                    With dr
                        .Dealercode = drChildInfomation.DealerCode              '販売店コード
                        .Brncd = drChildInfomation.BrnCd                        '店舗コード
                        .Orderno = drChildInfomation.OrderNo                    '受注NO
                        .Partstype = drChildInfomation.PartsType                '部品区分
                        .Partsname = drChildInfomation.PartsName                '品名
                        .Quantity = drChildInfomation.Quantity.ToString         '数量
                        .Srvtypename = drChildInfomation.SrvTypeName            '整備区分名称
                        .Unit = drChildInfomation.Unit                          '単位
                        .Boflag = drChildInfomation.BoFlg                       'BOFLG
                    End With

                    '行追加
                    dt.Rows.Add(dr)
                Next
            End If
        End If

        Logger.Info("GetPartsDetailData End")
        Return dt

    End Function

    '2012/03/01 子チップ作業・部品情報取得対応 上田 End

    ''' <summary>
    ''' 履歴情報の取得処理
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="repairOrderNumber">オーダーNo.</param>
    ''' <returns>部品詳細情報</returns>
    ''' <remarks></remarks>
    Public Function GetHistoryData(ByVal dealerCode As String, ByVal repairOrderNumber As String) _
                                                            As IC3801004DataSet.IC3801004OderSrvDataTable

        Logger.Info("GetHistoryData Start param1:" + dealerCode + _
                                                " param2:" + repairOrderNumber)

        Dim IC3801004BizLogic As IC3801004BusinessLogic = New IC3801004BusinessLogic
        Dim dt As IC3801004DataSet.IC3801004OderSrvDataTable

        If (String.IsNullOrEmpty(repairOrderNumber)) Then
            dt = Nothing
        Else
            '履歴情報の取得
            dt = IC3801004BizLogic.GetHistoryROList(dealerCode, repairOrderNumber)

        End If

        Logger.Info("GetHistoryData End")
        Return dt

    End Function

    '2012/03/03 日比野 ログ出力処理追加 START
    ''' <summary>
    ''' ログ出力(IF戻り値用)
    ''' </summary>
    ''' <param name="dt">戻り値(DataTable)</param>
    ''' <param name="ifName">使用IF名</param>
    ''' <remarks></remarks>
    Private Sub OutPutIFLog(ByVal dt As DataTable, ByVal ifName As String)

        Logger.Info(ifName + " Result START " + " OutPutCount: " + (dt.Rows.Count).ToString)

        Dim log As New Text.StringBuilder

        For j = 0 To dt.Rows.Count - 1

            log = New Text.StringBuilder()
            Dim dr As DataRow = dt.Rows(j)

            log.Append("RowNum: " + (j + 1).ToString + " -- ")

            For i = 0 To dt.Columns.Count - 1
                log.Append(dt.Columns(i).Caption)
                If IsDBNull(dr(i)) Then
                    log.Append(" IS NULL")
                Else
                    log.Append(" = ")
                    log.Append(dr(i).ToString)
                End If

                If i <= dt.Columns.Count - 2 Then
                    log.Append(", ")
                End If
            Next

            Logger.Info(log.ToString)
        Next

        Logger.Info(ifName + " Result END ")

    End Sub
    '2012/03/03 日比野 ログ出力処理追加 END

End Class
