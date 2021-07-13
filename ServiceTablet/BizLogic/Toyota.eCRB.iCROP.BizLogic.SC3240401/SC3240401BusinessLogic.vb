'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240401BusinessLogic.vb
'─────────────────────────────────────
'機能： チップ検索処理
'補足： 
'作成： 2013/07/24 TMEJ小澤	タブレット版SMB機能開発(工程管理)
'更新： 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新： 2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更
'更新： 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SMB.ChipSearch.DataAccess.SC3240401DataSetTableAdapters
Imports Toyota.eCRB.SMB.ChipSearch.DataAccess
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
'2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
Imports System.Text
'2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

Public Class SC3240401BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ''' <summary>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ' ''' 行追加ステータス（0：追加していない行）
    ''' 行追加ステータス（0：ストール上）
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AddRecordTypeOff As String = "0"
    ''' <summary>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ' ''' 行追加ステータス（1：追加した行）
    ''' 行追加ステータス（1：サブエリア）
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AddRecordTypeOn As String = "1"

    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ''' <summary>
    ''' ROステータス（20：FM承認待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrderStatusWaitingFmApproval As String = "20"

    ''' <summary>
    ''' ROステータス（50：着工指示待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrderStatusWorkOrderWait As String = "50"

    ''' <summary>
    ''' ROステータス（60：作業中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RepairOrderStatusWorking As String = "60"

    ''' <summary>
    ''' 仮置きフラグ（1：仮置き）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TempFlagOn As String = "1"

    ''' <summary>
    ''' 検索条件（5：RO一覧取得）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SearchTypeGetRepairOrder As String = "6"
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

    ''' <summary>
    ''' サービスステータス（07：洗車待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusWaitWash As String = "07"
    ''' <summary>
    ''' サービスステータス（08：洗車中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusWashing As String = "08"
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ''' <summary>
    ''' サービスステータス（11：預かり中）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusDropOffCustomer As String = "11"
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
    ''' <summary>
    ''' サービスステータス（12：納車待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ServiceStatusWaitDelivery As String = "12"

    ''' <summary>
    ''' ストール利用テータス（05：中断）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusStop As String = "05"
    ''' <summary>
    ''' ストール利用テータス（07：未来店客）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const StallUseStatusNoVisitor As String = "07"

    ''' <summary>
    ''' 完成検査ステータス（1：完成検査承認待ち）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApprovalStatusWaitApproval As String = "1"

    '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
    ''' <summary>
    ''' 検索条件（1：車両登録No）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SearchTypeRegisterNo As String = "1"
    '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

    ''' <summary>
    ''' リターンコード
    ''' </summary>
    Private Enum ReturnCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        Success = 0

        ''' <summary>
        ''' DBタイムアウト
        ''' </summary>
        ErrDBTimeout = 901

    End Enum

#End Region

#Region "メイン処理"

    ''' <summary>
    ''' 顧客一覧件数取得（予約有）
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inSearchType">検索条件</param>
    ''' <param name="inSearchValue">検索文字列</param>
    ''' <param name="inNowDate">現在日時</param>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ''' <param name="inBranchOperatingDateTime">当日の営業開始時刻</param>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
    ''' <returns>顧客情報取得件数</returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    'Public Function GetCustomerListCount(ByVal inDealerCode As String, _
    '                                 ByVal inBranchCode As String, _
    '                                 ByVal inSearchType As String, _
    '                                 ByVal inSearchValue As String, _
    '                                 ByVal inNowDate As Date) As Long
    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inSearchType = {4}" & _
    '         "inSearchValue = {5}, inNowDate = {6}" _
    '        , Me.GetType.ToString _
    '        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        , inDealerCode, inBranchCode, inSearchType, inSearchValue _
    '        , inNowDate.ToString(CultureInfo.CurrentCulture)))
    Public Function GetCustomerListCount(ByVal inDealerCode As String, _
                                         ByVal inBranchCode As String, _
                                         ByVal inSearchType As String, _
                                         ByVal inSearchValue As String, _
                                         ByVal inNowDate As Date, _
                                         ByVal inBranchOperatingDateTime As Date) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inSearchType = {4}" & _
                     "inSearchValue = {5}, inNowDate = {6}, inBranchOperatingDateTime = {7}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inBranchCode, inSearchType, inSearchValue _
                    , inNowDate.ToString(CultureInfo.CurrentCulture) _
                    , inBranchOperatingDateTime.ToString(CultureInfo.CurrentCulture)))
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

        '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
        If SearchTypeRegisterNo.Equals(inSearchType) Then
            Using smbCommonBiz As New ServiceCommonClassBusinessLogic
                '検索条件が車両登録Noの場合「*」と区切り文字を削除する
                inSearchValue = smbCommonBiz.ConvertVclRegNumWord(inSearchValue)
            End Using
            '区切り文字削除後の検索文字列が空の場合、検索結果0件として表示する(顧客を全件検索しない)
            If String.IsNullOrEmpty(inSearchValue) Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

                Return 0
            End If
        End If
        '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

        Using da As New SC3240401DataTableAdapter
            '全顧客一覧取得
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            'Dim dt As SC3240401DataSet.SC3240401CustomerInfoDataTable = _
            '    da.GetCustomerReserveList(inDealerCode, _
            '                          inBranchCode, _
            '                          inSearchType, _
            '                          inSearchValue, _
            '                          inNowDate, _
            '                          0, _
            '                          0, _
            '                          "", _
            '                          "", _
            '                          1)
            Dim dt As SC3240401DataSet.SC3240401CustomerInfoCountDataTable = _
                da.GetCustomerListCount(inDealerCode, _
                                        inBranchCode, _
                                        inSearchType, _
                                        inSearchValue, _
                                        inNowDate, _
                                        inBranchOperatingDateTime)
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return CType(dt.Rows(0)(0), Long)
        End Using
    End Function

    ''' <summary>
    ''' 顧客一覧取得（予約有）
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inSearchType">検索条件</param>
    ''' <param name="inSearchValue">検索文字列</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <param name="inStartIndex">開始行番号</param>
    ''' <param name="inEndIndex">終了行番号</param>
    ''' <param name="inSortModelCode">車両ソートフラグ</param>
    ''' <param name="inSortCustomerName">顧客ソートフラグ</param>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ''' <param name="inBranchOperatingDateTime">当日の営業開始時刻</param>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
    ''' <returns>顧客情報</returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    'Public Function GetCustomerList(ByVal inDealerCode As String, _
    '                            ByVal inBranchCode As String, _
    '                            ByVal inSearchType As String, _
    '                            ByVal inSearchValue As String, _
    '                            ByVal inNowDate As Date, _
    '                            ByVal inStartIndex As Long, _
    '                            ByVal inEndIndex As Long, _
    '                            ByVal inSortModelCode As String, _
    '                            ByVal inSortCustomerName As String) As SC3240401DataSet.SC3240401CustomerInfoDataTable
    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inSearchType = {4}" & _
    '         "inSearchValue = {5}, inNowDate = {6}, inStartIndex = {7}, inEndIndex = {8}" & _
    '         "inSortModelCode = {9}, inSortCustomerName = {10}" _
    '        , Me.GetType.ToString _
    '        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        , inDealerCode, inBranchCode, inSearchType, inSearchValue _
    '        , inNowDate.ToString(CultureInfo.CurrentCulture), inStartIndex.ToString(CultureInfo.CurrentCulture) _
    '        , inEndIndex.ToString(CultureInfo.CurrentCulture), inSortModelCode, inSortCustomerName))
    Public Function GetCustomerList(ByVal inDealerCode As String, _
                                    ByVal inBranchCode As String, _
                                    ByVal inSearchType As String, _
                                    ByVal inSearchValue As String, _
                                    ByVal inNowDate As Date, _
                                    ByVal inStartIndex As Long, _
                                    ByVal inEndIndex As Long, _
                                    ByVal inSortModelCode As String, _
                                    ByVal inSortCustomerName As String, _
                                    ByVal inBranchOperatingDateTime As Date) As SC3240401DataSet.SC3240401CustomerInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inSearchType = {4}" & _
                     "inSearchValue = {5}, inNowDate = {6}, inStartIndex = {7}, inEndIndex = {8}" & _
                     "inSortModelCode = {9}, inSortCustomerName = {10}, inBranchOperatingDateTime = {11}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inBranchCode, inSearchType, inSearchValue _
                    , inNowDate.ToString(CultureInfo.CurrentCulture), inStartIndex.ToString(CultureInfo.CurrentCulture) _
                    , inEndIndex.ToString(CultureInfo.CurrentCulture), inSortModelCode, inSortCustomerName _
                    , inBranchOperatingDateTime.ToString(CultureInfo.CurrentCulture)))
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

        '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
        If SearchTypeRegisterNo.Equals(inSearchType) Then
            Using smbCommonBiz As New ServiceCommonClassBusinessLogic
                '検索条件が車両登録Noの場合「*」と区切り文字を削除する
                inSearchValue = smbCommonBiz.ConvertVclRegNumWord(inSearchValue)
            End Using
        End If
        '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

        Using da As New SC3240401DataTableAdapter
            '顧客一覧取得
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            'Dim dt As SC3240401DataSet.SC3240401CustomerInfoDataTable = _
            '    da.GetCustomerReserveList(inDealerCode, _
            '                              inBranchCode, _
            '                              inSearchType, _
            '                              inSearchValue, _
            '                              inNowDate, _
            '                              inStartIndex, _
            '                              inEndIndex, _
            '                              inSortModelCode, _
            '                              inSortCustomerName)
            Dim dt As SC3240401DataSet.SC3240401CustomerInfoDataTable = _
                da.GetCustomerReserveList(inDealerCode, _
                              inBranchCode, _
                              inSearchType, _
                              inSearchValue, _
                              inNowDate, _
                              inStartIndex, _
                              inEndIndex, _
                              inSortModelCode, _
                              inSortCustomerName, _
                              inBranchOperatingDateTime)
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using
    End Function

    ''' <summary>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ' ''' 当日以降の予約情報取得
    ''' 予約情報取得
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inSearchType">検索条件</param>
    ''' <param name="inSearchValue">検索文字列</param>
    ''' <param name="inNowDate">現在日時</param>]
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ''' <param name="inBranchOperatingDateTime">当日の営業開始時刻</param>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
    ''' <param name="dtCustomerInfo">顧客一覧情報</param>
    ''' <returns>予約情報</returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    'Public Function GetReserveList(ByVal inDealerCode As String, _
    '                               ByVal inBranchCode As String, _
    '                               ByVal inSearchType As String, _
    '                               ByVal inSearchValue As String, _
    '                               ByVal inNowDate As Date, _
    '                               ByVal dtCustomerInfo As SC3240401DataSet.SC3240401CustomerInfoDataTable) As SC3240401DataSet.SC3240401ReserveInfoDataTable
    'Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inSearchType = {4}, inSearchValue = {5}" & _
    '         "inNowDate = {6}" _
    '        , Me.GetType.ToString _
    '        , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '        , inDealerCode, inBranchCode, inSearchType, inSearchValue _
    '        , inNowDate.ToString(CultureInfo.CurrentCulture)))
    Public Function GetReserveList(ByVal inDealerCode As String, _
                                   ByVal inBranchCode As String, _
                                   ByVal inSearchType As String, _
                                   ByVal inSearchValue As String, _
                                   ByVal inNowDate As Date, _
                                   ByVal inBranchOperatingDateTime As Date, _
                                   ByVal dtCustomerInfo As SC3240401DataSet.SC3240401CustomerInfoDataTable) As SC3240401DataSet.SC3240401ReserveInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inSearchType = {4}, inSearchValue = {5}" & _
                     "inNowDate = {6}, inBranchOperatingDateTime = {7}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inBranchCode, inSearchType, inSearchValue _
                    , inNowDate.ToString(CultureInfo.CurrentCulture) _
                    , inBranchOperatingDateTime.ToString(CultureInfo.CurrentCulture)))
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

        Using da As New SC3240401DataTableAdapter
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            Dim customerInfoList As New List(Of String)
            For Each drCustomerInfo As SC3240401DataSet.SC3240401CustomerInfoRow In dtCustomerInfo

                Dim customerInfo As New StringBuilder
                customerInfo.Append("(")
                customerInfo.Append(drCustomerInfo.CST_ID)
                customerInfo.Append(",")
                customerInfo.Append(drCustomerInfo.VCL_ID)
                customerInfo.Append(")")

                customerInfoList.Add(customerInfo.ToString)
            Next

            ''当日移行の予約情報取得
            'Dim dt As SC3240401DataSet.SC3240401ReserveInfoDataTable = _
            '    Me.MergeReserveList(da.GetReserveList(inDealerCode, _
            '                                          inBranchCode, _
            '                                          inSearchType, _
            '                                          inSearchValue, _
            '                                          inNowDate, _
            '                                          dtCustomerInfo))
            '予約情報取得
            Dim dtReserveInfo As SC3240401DataSet.SC3240401ReserveInfoDataTable = _
                da.GetReserveList(inDealerCode, _
                                  inBranchCode, _
                                  inSearchType, _
                                  inSearchValue, _
                                  inNowDate, _
                                  inBranchOperatingDateTime, _
                                  customerInfoList)

            '受付・追加作業サブエリアの予約情報の取得
            Dim dtReceptionAdditionalWork As SC3240401DataSet.SC3240401ReserveInfoDataTable = _
                da.GetReceptionAdditionalWorkReserveList(inDealerCode, _
                                                         inBranchCode, _
                                                         inSearchType, _
                                                         inSearchValue, _
                                                         customerInfoList)
            '取得した予約情報のマージ処理
            For Each drReceptionAdditionalWork As SC3240401DataSet.SC3240401ReserveInfoRow In dtReceptionAdditionalWork
                '新規ROW作成
                Dim drNewReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow = _
                    dtReserveInfo.NewSC3240401ReserveInfoRow

                '新規ROWに取得した受付・追加作業サブエリアの予約情報入れる
                For Each drReserveInfoColumn As DataColumn In dtReserveInfo.Columns
                    If (dtReserveInfo.Columns.Contains(drReserveInfoColumn.ColumnName)) Then
                        drNewReserveInfo(drReserveInfoColumn.ColumnName) = _
                            drReceptionAdditionalWork(drReserveInfoColumn.ColumnName)
                    End If
                Next
                dtReserveInfo.Rows.Add(drNewReserveInfo)
            Next
            'サブエリアのチップを検索結果にマージする
            Dim dt As SC3240401DataSet.SC3240401ReserveInfoDataTable = _
                Me.MergeReserveList(dtReserveInfo)
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using
    End Function

    ''' <summary>
    ''' ストール利用情報取得
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <returns>ストール利用情報</returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Public Function GetStallUseInfo(ByVal inStallUseId As Decimal) As SC3240401DataSet.SC3240401StallUseInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inStallUseId = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inStallUseId.ToString(CultureInfo.CurrentCulture)))

        Using da As New SC3240401DataTableAdapter
            'ストール利用情報を取得
            Dim dt As SC3240401DataSet.SC3240401StallUseInfoDataTable = _
                da.GetStallUseInfo(inStallUseId)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using
    End Function

    ''' <summary>
    ''' RO情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inRoNum">RO番号</param>
    ''' <param name="inRoSeq">RO連番</param>
    ''' <returns>ストール利用情報</returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Public Function GetRoInfo(ByVal inDealerCode As String, _
                              ByVal inBranchCode As String, _
                              ByVal inRoNum As String, _
                              ByVal inRoSeq As Decimal) As SC3240401DataSet.SC3240401RoInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inRoNum = {4}, inRoSeq = {5}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inBranchCode, inRoNum, inRoSeq))

        Using da As New SC3240401DataTableAdapter
            'RO情報を取得
            Dim dt As SC3240401DataSet.SC3240401RoInfoDataTable = _
                da.GetRoInfo(inDealerCode, inBranchCode, inRoNum, inRoSeq)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using
    End Function

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 顧客情報取得
    ' ''' </summary>
    ' ''' <param name="inDealerCode">販売店コード</param>
    ' ''' <param name="inCustomerId">顧客ID</param>
    ' ''' <param name="inVehiceleId">車両ID</param>
    ' ''' <returns>顧客情報</returns>
    ' ''' <remarks></remarks>
    ' ''' <hitory></hitory>
    'Public Function GetCustomerInfo(ByVal inDealerCode As String, _
    '                                ByVal inCustomerId As Decimal, _
    '                                ByVal inVehiceleId As Decimal) As SC3240401DataSet.SC3240401CustomerInfoDataTable
    ''' <summary>
    ''' 顧客情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inCustomerId">顧客ID</param>
    ''' <param name="inVehiceleId">車両ID</param>
    ''' <param name="inSvcinId">サービス入庫ID</param>
    ''' <returns>顧客情報</returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Public Function GetCustomerInfo(ByVal inDealerCode As String, _
                                    ByVal inCustomerId As Decimal, _
                                    ByVal inVehiceleId As Decimal, _
                                    ByVal inSvcinId As Decimal) As SC3240401DataSet.SC3240401CustomerInfoDataTable
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inDealerCode = {2}, inCustomerId = {3}, inVehiceleId = {4}, inSvcinId = {5}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inCustomerId.ToString(CultureInfo.CurrentCulture) _
                    , inVehiceleId.ToString(CultureInfo.CurrentCulture) _
                    , inSvcinId.ToString(CultureInfo.CurrentCulture)))

        Using da As New SC3240401DataTableAdapter
            '顧客情報取得
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'Dim dtCustomerInfo As SC3240401DataSet.SC3240401CustomerInfoDataTable = _
            '    da.GetCustomerInfo(inDealerCode, _
            '                       inCustomerId, _
            '                       inVehiceleId)
            Dim dtCustomerInfo As SC3240401DataSet.SC3240401CustomerInfoDataTable = _
                    da.GetCustomerInfo(inDealerCode, _
                                       inCustomerId, _
                                       inVehiceleId, _
                                       inSvcinId)
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dtCustomerInfo
        End Using
    End Function

    ''' <summary>
    ''' RO一覧情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inCustomerId">顧客ID</param>
    ''' <param name="inVehicleId">車両ID</param>
    ''' <param name="inNowDate">現在日時</param>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    ''' <param name="inBranchOperatingDateTime">店舗の営業開始日時</param>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
    ''' <returns>RO一覧情報</returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
    'Public Function GetOrderList(ByVal inDealerCode As String, _
    '                             ByVal inBranchCode As String, _
    '                             ByVal inCustomerId As Decimal, _
    '                             ByVal inVehicleId As Decimal, _
    '                             ByVal inNowDate As Date) As SC3240401DataSet.SC3240401ReserveInfoDataTable
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inCustomerId = {4}, inVehicleId = {5}, inNowDate = {6}" _
    '                , Me.GetType.ToString _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                , inDealerCode, inBranchCode _
    '                , inCustomerId.ToString(CultureInfo.CurrentCulture) _
    '                , inVehicleId.ToString(CultureInfo.CurrentCulture) _
    '                , inNowDate.ToString(CultureInfo.CurrentCulture)))

    Public Function GetOrderList(ByVal inDealerCode As String, _
                             ByVal inBranchCode As String, _
                             ByVal inCustomerId As Decimal, _
                             ByVal inVehicleId As Decimal, _
                             ByVal inNowDate As Date, _
                             ByVal inBranchOperatingDateTime As Date) As SC3240401DataSet.SC3240401ReserveInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inCustomerId = {4}, inVehicleId = {5}, inNowDate = {6}" & _
                     "inBranchOperatingDateTime = {7}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode, inBranchCode _
                    , inCustomerId.ToString(CultureInfo.CurrentCulture) _
                    , inVehicleId.ToString(CultureInfo.CurrentCulture) _
                    , inNowDate.ToString(CultureInfo.CurrentCulture) _
                    , inBranchOperatingDateTime.ToString(CultureInfo.CurrentCulture)))

        Dim customerInfoList As New List(Of String)

        Dim customerInfo As New StringBuilder
        customerInfo.Append("(")
        customerInfo.Append(inCustomerId)
        customerInfo.Append(",")
        customerInfo.Append(inVehicleId)
        customerInfo.Append(")")

        customerInfoList.Add(customerInfo.ToString)
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

        Using da As New SC3240401DataTableAdapter
            'RO一覧情報取得
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            'Dim dt As SC3240401DataSet.SC3240401ReserveInfoDataTable = _
            '    Me.MergeReserveList(da.GetOrderList(inDealerCode, _
            '                                        inBranchCode, _
            '                                        inCustomerId, _
            '                                        inVehicleId, _
            '                                        inNowDate))
            Dim dtOrderList As SC3240401DataSet.SC3240401ReserveInfoDataTable = _
                da.GetReserveList(inDealerCode, _
                                  inBranchCode, _
                                  SearchTypeGetRepairOrder, _
                                  " ", _
                                  inNowDate, _
                                  inBranchOperatingDateTime, _
                                  customerInfoList)

            '受付・追加作業サブエリアの予約情報の取得
            Dim dtReceptionAdditionalWork As SC3240401DataSet.SC3240401ReserveInfoDataTable = _
                da.GetReceptionAdditionalWorkReserveList(inDealerCode, _
                                                         inBranchCode, _
                                                         SearchTypeGetRepairOrder, _
                                                         " ", _
                                                         customerInfoList)

            '取得した予約情報のマージ処理
            For Each drReceptionAdditionalWork As SC3240401DataSet.SC3240401ReserveInfoRow In dtReceptionAdditionalWork
                '新規ROW作成
                Dim drNewReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow = _
                    dtOrderList.NewSC3240401ReserveInfoRow

                '新規ROWに取得した受付・追加作業サブエリアの予約情報入れる
                For Each drReserveInfoColumn As DataColumn In dtOrderList.Columns
                    If (dtOrderList.Columns.Contains(drReserveInfoColumn.ColumnName)) Then
                        drNewReserveInfo(drReserveInfoColumn.ColumnName) = _
                            drReceptionAdditionalWork(drReserveInfoColumn.ColumnName)
                    End If
                Next
                dtOrderList.Rows.Add(drNewReserveInfo)
            Next

            'サブエリアのチップをRO一覧にマージする
            Dim dt As SC3240401DataSet.SC3240401ReserveInfoDataTable = _
                Me.MergeReserveList(dtOrderList)
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Using
    End Function

#End Region

#Region "予約一覧マージ"

    ''' <summary>
    ''' 取得した予約一覧をマージする
    ''' </summary>
    ''' <param name="dtReserveInfo">当日以降の予約一覧</param>
    ''' <returns>マージ後の予約一覧</returns>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Function MergeReserveList(ByVal dtReserveInfo As SC3240401DataSet.SC3240401ReserveInfoDataTable) As SC3240401DataSet.SC3240401ReserveInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using dtNewReserveInfo As New SC3240401DataSet.SC3240401ReserveInfoDataTable
            '取得した当日予約一覧
            For Each drOldReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow In dtReserveInfo
                '新規ROW作成
                Dim drNewReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow = _
                    dtNewReserveInfo.NewSC3240401ReserveInfoRow

                '新規ROWに取得した情報を入れる
                For Each drReserveInfoColumn As DataColumn In dtReserveInfo.Columns
                    If (dtNewReserveInfo.Columns.Contains(drReserveInfoColumn.ColumnName)) Then
                        drNewReserveInfo(drReserveInfoColumn.ColumnName) = _
                            drOldReserveInfo(drReserveInfoColumn.ColumnName)
                    End If
                Next

                If StallUseStatusNoVisitor.Equals(drOldReserveInfo.STALL_USE_STATUS) Then
                    '「07：NoShow」の場合
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    ' '行追加のステータスを「1：追加した行」にする
                    '行追加のステータスを「1：サブエリア」にする
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                    drNewReserveInfo.ADDTYPE = AddRecordTypeOn

                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    ''第2ソートキーを加工（NoShowは一番下に表示するため「日付の最大値＋作業内容ID」にする）
                    'drNewReserveInfo.SORTKEY2_START_DATETIME = _
                    '    String.Concat(Date.MaxValue.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture), _
                    '                  drOldReserveInfo.JOB_DTL_ID.ToString(CultureInfo.CurrentCulture))
                    '第2ソートキーを加工（NoShowは一番下に表示するため「日付の最大値」にする）
                    drNewReserveInfo.SORTKEY2_START_DATETIME = _
                        Date.MaxValue.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture)

                ElseIf Not drOldReserveInfo.IsRO_STATUSNull() AndAlso _
                    New String() {RepairOrderStatusWorkOrderWait, _
                                  RepairOrderStatusWorking}.Contains(drOldReserveInfo.RO_STATUS) Then
                    'ROステータスが「50：着工指示待ちまたは60：作業中」の場合

                    '第2ソートキーを加工（日付最小値を設定）
                    drNewReserveInfo.SORTKEY2_START_DATETIME = _
                        Date.MinValue.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture)

                ElseIf Not drOldReserveInfo.IsTEMP_FLGNull() AndAlso _
                    TempFlagOn.Equals(drOldReserveInfo.TEMP_FLG) Then
                    '仮置きフラグが「1:仮置き」の場合
                    '行追加のステータスを「1：サブエリア」にする
                    drNewReserveInfo.ADDTYPE = AddRecordTypeOn

                    '第2ソートキーを加工（日付最小値を設定）
                    drNewReserveInfo.SORTKEY2_START_DATETIME = _
                        Date.MinValue.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture)
                    'Else
                    '    '第2ソートキーを加工
                    '    drNewReserveInfo.SORTKEY2_START_DATETIME = _
                    '        String.Concat(drNewReserveInfo.SORTKEY2_START_DATETIME, AddRecordTypeOff)
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                End If


                '情報を新規DataTableに格納
                dtNewReserveInfo.Rows.Add(drNewReserveInfo)

                '追加する行があれば追加する
                Me.AddNewReserveRecord(drOldReserveInfo, _
                                       dtNewReserveInfo, _
                                       dtReserveInfo, _
                                       drNewReserveInfo)

            Next

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dtNewReserveInfo
        End Using

    End Function

    ''' <summary>
    ''' 予約情報追加処理
    ''' </summary>
    ''' <param name="drOldReserveInfo">元の予約情報</param>
    ''' <param name="dtNewReserveInfo">格納用予約一覧</param>
    ''' <param name="dtOldReserveInfo">元の予約一覧</param>
    ''' <param name="drNewReserveInfo">格納用予約情報</param>
    ''' <remarks></remarks>
    ''' <hitory></hitory>
    Private Sub AddNewReserveRecord(ByVal drOldReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow, _
                                    ByVal dtNewReserveInfo As SC3240401DataSet.SC3240401ReserveInfoDataTable, _
                                    ByVal dtOldReserveInfo As SC3240401DataSet.SC3240401ReserveInfoDataTable, _
                                    ByVal drNewReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
        If AddRecordTypeOn.Equals(drOldReserveInfo.ADDTYPE) Then
            '追加行ステータスがすでに「1：サブエリア」の場合は行を追加しない

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return
        End If
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
        'If (New String() {ServiceStatusWaitWash, _
        '          ServiceStatusWashing, _
        '          ServiceStatusWaitDelivery}.Contains(drOldReserveInfo.SVC_STATUS)) Then
        ''サービスステータスが「07：洗車待ち、08：洗車中、12：納車待ち」の場合
        If (New String() {ServiceStatusWaitWash, _
                          ServiceStatusWashing, _
                          ServiceStatusDropOffCustomer, _
                          ServiceStatusWaitDelivery}.Contains(drOldReserveInfo.SVC_STATUS)) Then
            'サービスステータスが「07：洗車待ち、08：洗車中、11：預かり中、12：納車待ち」の場合
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

            '上記ステータスの行がすでに作成されているか確認する
            Dim serviceInId As Decimal = drOldReserveInfo.SVCIN_ID
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            'Dim drServiceAddType As Boolean = _
            '    (From drSearchNewReserveInfo In dtNewReserveInfo _
            '     Where drSearchNewReserveInfo.SVCIN_ID = serviceInId _
            '     And drSearchNewReserveInfo.ADDTYPE = AddRecordTypeOn _
            '     And (drSearchNewReserveInfo.SVC_STATUS = ServiceStatusWaitWash _
            '     Or drSearchNewReserveInfo.SVC_STATUS = ServiceStatusWashing _
            '     Or drSearchNewReserveInfo.SVC_STATUS = ServiceStatusWaitDelivery)).Count = 0
            Dim drServiceAddType As Boolean = _
                (From drSearchNewReserveInfo In dtNewReserveInfo _
                 Where drSearchNewReserveInfo.SVCIN_ID = serviceInId _
                 And drSearchNewReserveInfo.ADDTYPE = AddRecordTypeOn _
                 And (drSearchNewReserveInfo.SVC_STATUS = ServiceStatusWaitWash _
                 Or drSearchNewReserveInfo.SVC_STATUS = ServiceStatusWashing _
                 Or drSearchNewReserveInfo.SVC_STATUS = ServiceStatusDropOffCustomer _
                 Or drSearchNewReserveInfo.SVC_STATUS = ServiceStatusWaitDelivery)).Count = 0
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

            If drServiceAddType Then
                '新規行が追加されていない場合は新規行を追加する
                '新規ROW作成
                Dim drAddReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow = _
                    dtNewReserveInfo.NewSC3240401ReserveInfoRow

                '最後に作業終了したデータを取得する
                Dim lastWorkInfo As SC3240401DataSet.SC3240401ReserveInfoRow = _
                    (From drLastWorkInfo As SC3240401DataSet.SC3240401ReserveInfoRow In dtOldReserveInfo _
                     Where drLastWorkInfo.SVCIN_ID = serviceInId _
                     Order By drLastWorkInfo.END_DATETIME Descending Select drLastWorkInfo)(0)

                '最後に作業終了したデータを新規ROWに格納する
                For Each drReserveInfoColumn As DataColumn In dtOldReserveInfo.Columns
                    If (dtNewReserveInfo.Columns.Contains(drReserveInfoColumn.ColumnName)) Then
                        drAddReserveInfo(drReserveInfoColumn.ColumnName) = _
                            lastWorkInfo(drReserveInfoColumn.ColumnName)
                    End If
                Next

                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                ''第2ソートキー作成
                'drAddReserveInfo.SORTKEY2_START_DATETIME = _
                '    String.Concat(drAddReserveInfo.SORTKEY2_START_DATETIME, AddRecordTypeOff)

                ' '行追加のステータスを「1：追加した行」にする
                '行追加のステータスを「1：サブエリア」にする
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                drAddReserveInfo.ADDTYPE = AddRecordTypeOn

                '情報を新規DataTableに格納
                dtNewReserveInfo.Rows.Add(drAddReserveInfo)
            End If

        ElseIf ApprovalStatusWaitApproval.Equals(drOldReserveInfo.INSPECTION_STATUS) Then
            '完成検査フラグが「1：完成検査承認待ち」の場合は新規行を追加する

            '新規ROW作成
            Dim drAddReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow = _
                dtNewReserveInfo.NewSC3240401ReserveInfoRow

            '新規ROWに取得した情報を入れる
            For Each drReserveInfoColumn As DataColumn In dtOldReserveInfo.Columns
                If (dtNewReserveInfo.Columns.Contains(drReserveInfoColumn.ColumnName)) Then
                    drAddReserveInfo(drReserveInfoColumn.ColumnName) = _
                        drOldReserveInfo(drReserveInfoColumn.ColumnName)
                End If
            Next

            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            ''第2ソートキー作成
            'drAddReserveInfo.SORTKEY2_START_DATETIME = _
            '    String.Concat(drNewReserveInfo.SORTKEY2_START_DATETIME, AddRecordTypeOn)

            ' '行追加のステータスを「1：追加した行」にする
            '行追加のステータスを「1：サブエリア」にする
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
            drAddReserveInfo.ADDTYPE = AddRecordTypeOn

            '情報を新規DataTableに格納
            dtNewReserveInfo.Rows.Add(drAddReserveInfo)

        ElseIf StallUseStatusStop.Equals(drOldReserveInfo.STALL_USE_STATUS) Then
            'ストール利用ステータスが「05：中断」の場合

            '最後のチップ情報を取得する
            Dim serviceInId As Decimal = drOldReserveInfo.SVCIN_ID
            Dim jobDetailId As Decimal = drOldReserveInfo.JOB_DTL_ID
            Dim lastChipInfo As SC3240401DataSet.SC3240401ReserveInfoRow = _
                (From drLastWorkInfo As SC3240401DataSet.SC3240401ReserveInfoRow In dtOldReserveInfo _
                 Where drLastWorkInfo.SVCIN_ID = serviceInId _
                 And drLastWorkInfo.JOB_DTL_ID = jobDetailId _
                 Order By drLastWorkInfo.STALL_USE_ID Descending Select drLastWorkInfo)(0)

            If lastChipInfo.STALL_USE_ID = drOldReserveInfo.STALL_USE_ID AndAlso _
                StallUseStatusStop.Equals(lastChipInfo.STALL_USE_STATUS) Then
                '最後のチップ情報と一致し、中断の場合は行を追加する
                '新規ROW作成
                Dim drAddReserveInfo As SC3240401DataSet.SC3240401ReserveInfoRow = _
                    dtNewReserveInfo.NewSC3240401ReserveInfoRow

                '新規ROWに取得した情報を入れる
                For Each drReserveInfoColumn As DataColumn In dtOldReserveInfo.Columns
                    If (dtNewReserveInfo.Columns.Contains(drReserveInfoColumn.ColumnName)) Then
                        drAddReserveInfo(drReserveInfoColumn.ColumnName) = _
                            drOldReserveInfo(drReserveInfoColumn.ColumnName)
                    End If
                Next

                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                ''第2ソートキー作成
                'drAddReserveInfo.SORTKEY2_START_DATETIME = _
                '    String.Concat(drNewReserveInfo.SORTKEY2_START_DATETIME, AddRecordTypeOn)
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                ' '行追加のステータスを「1：追加した行」にする
                '行追加のステータスを「1：サブエリア」にする
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                drAddReserveInfo.ADDTYPE = AddRecordTypeOn

                '情報を新規DataTableに格納
                dtNewReserveInfo.Rows.Add(drAddReserveInfo)

            End If

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class

