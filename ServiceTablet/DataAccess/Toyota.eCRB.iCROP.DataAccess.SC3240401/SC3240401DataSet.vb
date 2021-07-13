'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240401DataSet.vb
'─────────────────────────────────────
'機能： チップ検索 データセット
'補足： 
'作成： 2013/07/24 TMEJ小澤	タブレット版SMB機能開発(工程管理)
'更新： 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/02/27 TMEJ 小澤 タブレット版SMB チーフテクニシャン機能開発
'更新： 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする
'更新： 2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
'更新： 2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SMB.ChipSearch.DataAccess.SC3240401DataSet

Namespace SC3240401DataSetTableAdapters
    Public Class SC3240401DataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' サービスステータス（02：キャンセル）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiceStatusCancel As String = "02"

        ''' <summary>
        ''' キャンセルフラグ（0：有効）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CancelTypeEffective As String = "0"

        ''' <summary>
        ''' あいまい検索用
        ''' </summary>
        ''' <remarks></remarks>
        Private Const LikeWord As String = "%"

        ''' <summary>
        ''' 日付最小値文字列
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DateMinValue As String = "1900/01/01 00:00:00"

        ''' <summary>
        ''' 検索条件（0：車両登録No）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SearchTypeRegisterNo As String = "1"
        ''' <summary>
        ''' 検索条件（1：顧客氏名）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SearchTypeCustomerName As String = "2"
        ''' <summary>
        ''' 検索条件（2：VIN）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SearchTypeVin As String = "3"

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

        ''' <summary>
        ''' 仮置きフラグ（0：未配置）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TempFlgOff As String = "0"

        ' ''' <summary>
        ' ''' 検索条件（3：電話番号、携帯番号）
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Private Const SearchTypeTelMobile As String = "4"
        ''' <summary>
        ''' DMS予約番号
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SearchTypeDMSJobDtlId As String = "4"
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        ''' <summary>
        ''' 検索条件（4：RO番号）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SearchTypeOrderNo As String = "5"

        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
        ''' <summary>
        ''' 検索条件（5：RO一覧取得）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SearchTypeGetRepairOrder As String = "6"
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

        ''' <summary>
        ''' 車両ソートタイプ（1：昇順）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SortTypeVehicleAcs As String = "1"
        ''' <summary>
        ''' 車両ソートタイプ（2：降順）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SortTypeVehicleDesc As String = "2"
        ''' <summary>
        ''' 顧客ソートタイプ（1：昇順）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SortTypeCustomerAcs As String = "1"
        ''' <summary>
        ''' 顧客ソートタイプ（2：降順）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SortTypeCustomerDesc As String = "2"

        ''' <summary>
        ''' 受付区分（1：WalkIn）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AcceptanceTypeWalkIn As String = "1"

        ''' <summary>
        ''' ストールID（0：WalkIn用）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallIdWalkIn As Decimal = 0

        ''' <summary>
        ''' 行追加ステータス（0：追加していない行）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AddRecordTypeOff As String = "0"

        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
        ''' <summary>
        ''' ROステータス（20：FM承認待ち）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RoStatusWaitingForeManApprove As String = "20"

        ''' <summary>
        ''' ROステータス（50：着工指示待ち）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RoStatusCustomerApprove As String = "50"

        ''' <summary>
        ''' ROステータス（60：作業中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RoStatusWorking As String = "60"

        ''' <summary>
        ''' 着工指示フラグ（0：未指示）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StartworkInstructFlgOff As String = "0"
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
        '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        ''' <summary>
        ''' アイコンのフラグ（1：表示）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOn = "1"
        ''' <summary>
        ''' アイコンのフラグ（2：非表示）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOff = "0"
        '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

#End Region

#Region "メイン"

        ''' <summary>
        ''' 顧客一覧取得（予約有）
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inSearchType">検索条件</param>
        ''' <param name="inSearchValue">検索値</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inStartIndex">開始取得番号</param>
        ''' <param name="inEndIndex">終了取得番号</param>
        ''' <param name="inSortModelCode">モデルコートのソート有無</param>
        ''' <param name="inSortCustomerName">顧客指名のソート有無</param>
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
        ''' <param name="inBranchOperatingDateTime">当日の営業開始時刻</param>
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
        ''' <returns>顧客一覧取得（予約有）</returns>
        ''' <remarks></remarks>
        ''' <hitory></hitory>
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
        'Public Function GetCustomerReserveList(ByVal inDealerCode As String, _
        '                                       ByVal inBranchCode As String, _
        '                                       ByVal inSearchType As String, _
        '                                       ByVal inSearchValue As String, _
        '                                       ByVal inNowDate As Date, _
        '                                       ByVal inStartIndex As Long, _
        '                                       ByVal inEndIndex As Long, _
        '                                       ByVal inSortModelCode As String, _
        '                                       ByVal inSortCustomerName As String, _
        '                                       Optional inAllCountType As Integer = 0) As SC3240401CustomerInfoDataTable
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inSearchType = {4}" & _
        '     "inSearchValue = {5}, inNowDate = {6}, inStartIndex = {7}, inEndIndex = {8}" & _
        '     "inSortModelCode = {9}, inSortCustomerName = {10}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , inDealerCode, inBranchCode, inSearchType, inSearchValue _
        '    , inNowDate.ToString(CultureInfo.CurrentCulture), inStartIndex.ToString(CultureInfo.CurrentCulture) _
        '    , inEndIndex.ToString(CultureInfo.CurrentCulture), inSortModelCode, inSortCustomerName))
        Public Function GetCustomerReserveList(ByVal inDealerCode As String, _
                                       ByVal inBranchCode As String, _
                                       ByVal inSearchType As String, _
                                       ByVal inSearchValue As String, _
                                       ByVal inNowDate As Date, _
                                       ByVal inStartIndex As Long, _
                                       ByVal inEndIndex As Long, _
                                       ByVal inSortModelCode As String, _
                                       ByVal inSortCustomerName As String, _
                                       ByVal inBranchOperatingDateTime As Date) As SC3240401CustomerInfoDataTable
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

            'データ格納用
            Dim dt As SC3240401CustomerInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine("SELECT /* SC3240401_001 */ ")
                .AppendLine("       Z1.CST_ID ")
                .AppendLine("      ,Z1.VCL_ID ")
                .AppendLine("      ,Z1.REG_NUM ")
                .AppendLine("      ,Z1.MODEL_NAME ")
                .AppendLine("      ,Z1.VCL_VIN ")
                .AppendLine("      ,Z1.CST_NAME ")
                .AppendLine("      ,Z1.NAMETITLE_NAME ")
                .AppendLine("      ,Z1.CST_TYPE ")
                .AppendLine("      ,Z1.FLEET_FLG ")
                .AppendLine("      ,Z1.CST_PHONE ")
                .AppendLine("      ,Z1.CST_MOBILE ")
                .AppendLine("      ,Z1.IMG_FILE ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("      ,Z1.POSITION_TYPE ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                .AppendLine("      ,Z1.IMP_VCL_FLG ")
                .AppendLine("      ,Z1.SML_AMC_FLG ")
                .AppendLine("      ,Z1.EW_FLG ")
                .AppendLine("      ,Z1.TLM_MBR_FLG ")
                '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 START
                .AppendLine("      ,Z1.CST_JOIN_TYPE ")
                '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 END
                .AppendLine("  FROM ")
                .AppendLine("       (SELECT ROWNUM AS ROW_COUNT ")
                .AppendLine("              ,E1.CST_ID ")
                .AppendLine("              ,E1.VCL_ID ")
                .AppendLine("              ,E1.REG_NUM ")
                .AppendLine("              ,E1.MODEL_NAME ")
                .AppendLine("              ,E1.VCL_VIN ")
                .AppendLine("              ,E1.CST_NAME ")
                .AppendLine("              ,E1.NAMETITLE_NAME ")
                .AppendLine("              ,E1.CST_TYPE ")
                .AppendLine("              ,E1.FLEET_FLG ")
                .AppendLine("              ,E1.CST_PHONE ")
                .AppendLine("              ,E1.CST_MOBILE ")
                .AppendLine("              ,E1.IMG_FILE ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("              ,E1.POSITION_TYPE ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                .AppendLine("              ,E1.IMP_VCL_FLG ")
                .AppendLine("              ,E1.SML_AMC_FLG ")
                .AppendLine("              ,E1.EW_FLG ")
                .AppendLine("              ,E1.TLM_MBR_FLG ")
                '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 START
                .AppendLine("              ,E1.CST_JOIN_TYPE ")
                '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 END
                .AppendLine("          FROM ")
                .AppendLine("               (SELECT W1.CST_ID ")
                .AppendLine("                      ,W1.VCL_ID ")
                .AppendLine("                      ,W1.REG_NUM ")
                .AppendLine("                      ,W1.MODEL_CD ")
                .AppendLine("                      ,W1.MODEL_NAME ")
                .AppendLine("                      ,W1.VCL_VIN ")
                .AppendLine("                      ,W1.CST_NAME ")
                .AppendLine("                      ,W1.NAMETITLE_NAME ")
                .AppendLine("                      ,W1.CST_TYPE ")
                .AppendLine("                      ,W1.FLEET_FLG ")
                .AppendLine("                      ,W1.CST_PHONE ")
                .AppendLine("                      ,W1.CST_MOBILE ")
                .AppendLine("                      ,W1.IMG_FILE ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("                      ,W1.POSITION_TYPE ")
                '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                .AppendLine("                      ,W1.IMP_VCL_FLG ")
                .AppendLine("                      ,W1.SML_AMC_FLG ")
                .AppendLine("                      ,W1.EW_FLG ")
                .AppendLine("                      ,W1.TLM_MBR_FLG ")
                '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 START
                .AppendLine("                      ,W1.CST_JOIN_TYPE ")
                '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 END
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("                  FROM (SELECT A1.CST_ID ")
                .AppendLine("                              ,A1.VCL_ID ")
                .AppendLine("                              ,TRIM(A5.REG_NUM) AS REG_NUM ")
                .AppendLine("                              ,TRIM(A6.MODEL_CD) AS MODEL_CD ")
                .AppendLine("                              ,NVL(TRIM(A6.MODEL_NAME), TRIM(A4.NEWCST_MODEL_NAME)) AS MODEL_NAME ")
                .AppendLine("                              ,TRIM(A4.VCL_VIN) AS VCL_VIN ")
                .AppendLine("                              ,TRIM(A2.CST_NAME) AS CST_NAME ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("                              ,TRIM(A2.NAMETITLE_NAME) AS NAMETITLE_NAME ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("                              ,TRIM(A3.CST_TYPE) AS CST_TYPE ")
                .AppendLine("                              ,TRIM(A2.FLEET_FLG) AS FLEET_FLG ")
                .AppendLine("                              ,TRIM(A2.CST_PHONE) AS CST_PHONE ")
                .AppendLine("                              ,TRIM(A2.CST_MOBILE) AS CST_MOBILE ")
                .AppendLine("                              ,TRIM(A3.IMG_FILE_SMALL) AS IMG_FILE ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("                              ,TRIM(A7.NAMETITLE_NAME) AS NAMETITLE_NAME ")
                .AppendLine("                              ,TRIM(A7.POSITION_TYPE) AS POSITION_TYPE ")
                '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                .AppendLine("                              ,NVL(TRIM(A5.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                .AppendLine("                              ,NVL(TRIM(A8.SML_AMC_FLG), :ICON_FLAG_OFF) AS SML_AMC_FLG ")
                .AppendLine("                              ,NVL(TRIM(A8.EW_FLG), :ICON_FLAG_OFF) AS EW_FLG ")
                .AppendLine("                              ,CASE ")
                .AppendLine("                                    WHEN A9.VCL_VIN IS NOT NULL THEN :ICON_FLAG_ON ")
                .AppendLine("                               ELSE :ICON_FLAG_OFF ")
                .AppendLine("                               END AS TLM_MBR_FLG ")
                '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 START
                .AppendLine("                           　　,NVL(TRIM(A10.CST_JOIN_TYPE), :ICON_FLAG_OFF) AS CST_JOIN_TYPE")
                '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 END
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("                          FROM TB_M_CUSTOMER_VCL A1 ")
                .AppendLine("                              ,TB_M_CUSTOMER A2 ")
                .AppendLine("                              ,TB_M_CUSTOMER_DLR A3 ")
                .AppendLine("                              ,TB_M_VEHICLE A4 ")
                .AppendLine("                              ,TB_M_VEHICLE_DLR A5 ")
                .AppendLine("                              ,TB_M_MODEL A6 ")
                '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                .AppendLine("                              ,TB_LM_VEHICLE A8 ")
                .AppendLine("                              ,TB_LM_TLM_MEMBER A9 ")
                '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 START
                .AppendLine("                              ,TB_LM_PRIVATE_FLEET_ITEM A10 ")
                '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 END
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("                              ,(SELECT S1.NAMETITLE_CD ")
                .AppendLine("                                      ,S1.NAMETITLE_NAME ")
                .AppendLine("                                      ,S1.POSITION_TYPE ")
                .AppendLine("                                 FROM TB_M_NAMETITLE S1 ")
                .AppendLine("                                WHERE S1.INUSE_FLG = N'1' ) A7 ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                .AppendLine("                         WHERE A1.CST_ID = A2.CST_ID ")
                .AppendLine("                           AND A1.DLR_CD = A3.DLR_CD ")
                .AppendLine("                           AND A1.CST_ID = A3.CST_ID ")
                .AppendLine("                           AND A1.VCL_ID = A4.VCL_ID ")
                .AppendLine("                           AND A1.DLR_CD = A5.DLR_CD ")
                .AppendLine("                           AND A1.VCL_ID = A5.VCL_ID ")
                .AppendLine("                           AND A4.MODEL_CD = A6.MODEL_CD(+) ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("                           AND A2.NAMETITLE_CD = A7.NAMETITLE_CD(+) ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                .AppendLine("                           AND A4.VCL_ID = A8.VCL_ID(+) ")
                .AppendLine("                           AND A4.VCL_VIN = A9.VCL_VIN(+) ")
                '2018/06/27 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 START
                .AppendLine("                           AND A2.PRIVATE_FLEET_ITEM_CD = A10.PRIVATE_FLEET_ITEM_CD(+) ")
                '2018/07/17 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 END
                .AppendLine("                           AND A1.DLR_CD = :DLR_CD ")
                .AppendLine("                           AND A3.DLR_CD = :DLR_CD ")
                .AppendLine("                           AND A5.DLR_CD = :DLR_CD ")

                '検索条件追加
                If SearchTypeRegisterNo.Equals(inSearchType) Then
                    '車両登録No（後方一致）
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    '.AppendLine("                           AND A5.REG_NUM_SEARCH LIKE (UPPER(:REG_NUM_SEARCH)) ")
                    .AppendLine("                           AND A5.REG_NUM_SEARCH_REV LIKE (UPPER(:REG_NUM_SEARCH_REV)) ")
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                ElseIf SearchTypeCustomerName.Equals(inSearchType) Then
                    '顧客氏名（前方一致）
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    '.AppendLine("                           AND A2.CST_NAME_SEARCH LIKE (:CST_NAME_SEARCH) ")
                    .AppendLine("                           AND A2.CST_NAME_SEARCH LIKE (UPPER(:CST_NAME_SEARCH)) ")
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                ElseIf SearchTypeVin.Equals(inSearchType) Then
                    'VIN（後方一致）
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    '.AppendLine("                           AND A4.VCL_VIN_SEARCH LIKE (UPPER(:VCL_VIN_SEARCH)) ")
                    .AppendLine("                           AND A4.VCL_VIN_SEARCH_REV LIKE (UPPER(:VCL_VIN_SEARCH_REV)) ")
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    'ElseIf SearchTypeTelMobile.Equals(inSearchType) Then
                    '    '電話番号、携帯番号（完全一致）
                    '    .AppendLine("                           AND (A2.CST_PHONE_SEARCH = :CST_PHONE_SEARCH OR A2.CST_MOBILE_SEARCH = :CST_MOBILE_SEARCH) ")
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                End If
                .AppendLine("                           ) W1 ")
                .AppendLine("                      ,(SELECT T1.CST_ID ")
                .AppendLine("                              ,T1.VCL_ID ")
                .AppendLine("                          FROM TB_T_SERVICEIN T1 ")
                .AppendLine("                              ,TB_T_JOB_DTL T2 ")
                .AppendLine("                              ,TB_T_STALL_USE T3 ")
                .AppendLine("                         WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("                           AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine("                           AND T1.DLR_CD = :DLR_CD ")
                .AppendLine("                           AND T1.BRN_CD = :BRN_CD ")
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                '.AppendLine("                           AND NOT EXISTS (SELECT 1 ")
                '.AppendLine("                                             FROM TB_T_SERVICEIN M1 ")
                '.AppendLine("                                            WHERE M1.SVCIN_ID = T1.SVCIN_ID ")
                '.AppendLine("                                              AND M1.SVC_STATUS = :SVC_STATUS_02) ")
                '.AppendLine("                           AND NOT EXISTS (SELECT 1 ")
                '.AppendLine("                                             FROM TB_T_SERVICEIN D1 ")
                '.AppendLine("                                                 ,TB_T_JOB_DTL D2 ")
                '.AppendLine("                                                 ,TB_T_STALL_USE D3 ")
                '.AppendLine("                                            WHERE D1.SVCIN_ID = D2.SVCIN_ID ")
                '.AppendLine("                                              AND D2.JOB_DTL_ID = D3.JOB_DTL_ID ")
                '.AppendLine("                                              AND D1.SVCIN_ID = T1.SVCIN_ID ")
                '.AppendLine("                                              AND D2.JOB_DTL_ID = T2.JOB_DTL_ID ")
                '.AppendLine("                                              AND D3.STALL_USE_ID = T3.STALL_USE_ID ")
                '.AppendLine("                                              AND D1.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_1 ")
                '.AppendLine("                                              AND D3.STALL_ID = :STALL_ID_0) ")
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END 
                .AppendLine("                           AND T2.DLR_CD = :DLR_CD ")
                .AppendLine("                           AND T2.BRN_CD = :BRN_CD ")
                .AppendLine("                           AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("                           AND T3.DLR_CD = :DLR_CD ")
                .AppendLine("                           AND T3.BRN_CD = :BRN_CD ")
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                '.AppendLine("                           AND T3.TEMP_FLG = :TEMP_FLG_0 ")
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                If SearchTypeDMSJobDtlId.Equals(inSearchType) Then
                    'DMS予約番号（後方一致）
                    .AppendLine("                           AND T2.DMS_JOB_DTL_ID LIKE (UPPER(:DMS_JOB_DTL_ID)) ")
                End If
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                '.AppendLine("                           AND CASE WHEN T3.RSLT_START_DATETIME <> :MINDATE THEN T3.RSLT_START_DATETIME ")
                '.AppendLine("                                    ELSE T3.SCHE_START_DATETIME END >= TRUNC(:NOWDATE) ")
                '.AppendLine("                           AND CASE WHEN T3.RSLT_START_DATETIME <> :MINDATE THEN T3.RSLT_START_DATETIME ")
                '.AppendLine("                                    ELSE T3.SCHE_START_DATETIME END <= TRUNC(:MAXDATE) ")
                .AppendLine("  AND (")
                .AppendLine("          T1.SVC_STATUS IN ('00', '01', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12')")
                .AppendLine("          OR T3.RSLT_END_DATETIME >= :BRANCH_OPERATION_DATE")
                .AppendLine("  )")
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                '検索条件追加
                If SearchTypeOrderNo.Equals(inSearchType) Then
                    'RO番号（後方一致）
                    .AppendLine("                           AND T1.RO_NUM LIKE (:RO_NUM) ")

                End If

                .AppendLine("                      GROUP BY T1.CST_ID, T1.VCL_ID) W2 ")
                .AppendLine("                 WHERE W1.CST_ID = W2.CST_ID ")
                .AppendLine("                   AND W1.VCL_ID = W2.VCL_ID ")
                'ソート条件追加
                If SortTypeVehicleAcs.Equals(inSortModelCode) Then
                    'モデルコードの昇順
                    .AppendLine("                 ORDER BY W1.MODEL_CD ASC ")

                ElseIf SortTypeVehicleDesc.Equals(inSortModelCode) Then
                    'モデルコードの降順
                    .AppendLine("                 ORDER BY W1.MODEL_CD DESC ")

                ElseIf SortTypeCustomerAcs.Equals(inSortCustomerName) Then
                    '顧客氏名の昇順
                    .AppendLine("                 ORDER BY W1.CST_NAME ASC ")

                ElseIf SortTypeCustomerDesc.Equals(inSortCustomerName) Then
                    '顧客氏名の降順
                    .AppendLine("                 ORDER BY W1.CST_NAME DESC ")

                End If
                .AppendLine("                 ) E1 ")
                .AppendLine("       ) Z1 ")

                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                ''カウント取得の場合は不要
                'If inAllCountType = 0 Then
                '    .AppendLine(" WHERE Z1.ROW_COUNT BETWEEN :STARTINDEX AND :ENDINDEX ")
                'End If
                .AppendLine(" WHERE Z1.ROW_COUNT BETWEEN :STARTINDEX AND :ENDINDEX ")
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
            End With

            Using query As New DBSelectQuery(Of SC3240401CustomerInfoDataTable)("SC3240401_001")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                'query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                'query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_1", OracleDbType.NVarchar2, AcceptanceTypeWalkIn)
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                ''Tempエリアのチップを検索して、出てこないように
                'query.AddParameterWithTypeValue("TEMP_FLG_0", OracleDbType.NVarchar2, TempFlgOff)
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                'query.AddParameterWithTypeValue("STALL_ID_0", OracleDbType.Decimal, StallIdWalkIn)
                'query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                'query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
                'query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)
                query.AddParameterWithTypeValue("BRANCH_OPERATION_DATE", OracleDbType.Date, inBranchOperatingDateTime)
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                '2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                '2018/06/12 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

                '検索条件追加
                If SearchTypeRegisterNo.Equals(inSearchType) Then
                    '車両登録No（後方一致）
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    'query.AddParameterWithTypeValue("REG_NUM_SEARCH", OracleDbType.NVarchar2, String.Concat(LikeWord, inSearchValue))
                    query.AddParameterWithTypeValue("REG_NUM_SEARCH_REV", OracleDbType.NVarchar2, StrReverse(String.Concat(LikeWord, inSearchValue)))
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                ElseIf SearchTypeCustomerName.Equals(inSearchType) Then
                    '顧客氏名（前方一致）
                    query.AddParameterWithTypeValue("CST_NAME_SEARCH", OracleDbType.NVarchar2, String.Concat(inSearchValue, LikeWord))

                ElseIf SearchTypeVin.Equals(inSearchType) Then
                    'VIN（後方一致）
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    'query.AddParameterWithTypeValue("VCL_VIN_SEARCH", OracleDbType.NVarchar2, String.Concat(LikeWord, inSearchValue))
                    query.AddParameterWithTypeValue("VCL_VIN_SEARCH_REV", OracleDbType.NVarchar2, StrReverse(String.Concat(LikeWord, inSearchValue)))
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    'ElseIf SearchTypeTelMobile.Equals(inSearchType) Then
                    ''電話番号、携帯番号（完全一致）
                    'query.AddParameterWithTypeValue("CST_PHONE_SEARCH", OracleDbType.NVarchar2, inSearchValue.Replace("-", ""))
                    'query.AddParameterWithTypeValue("CST_MOBILE_SEARCH", OracleDbType.NVarchar2, inSearchValue.Replace("-", ""))
                ElseIf SearchTypeDMSJobDtlId.Equals(inSearchType) Then
                    'DMS予約番号（後方一致）
                    query.AddParameterWithTypeValue("DMS_JOB_DTL_ID", OracleDbType.NVarchar2, String.Concat(LikeWord, inSearchValue))
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                End If

                '検索条件追加
                If SearchTypeOrderNo.Equals(inSearchType) Then
                    'RO番号（後方一致）
                    query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, String.Concat(LikeWord, inSearchValue))

                End If

                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                ''カウント取得の場合は不要
                'If inAllCountType = 0 Then
                '    query.AddParameterWithTypeValue("STARTINDEX", OracleDbType.Long, inStartIndex)
                '    query.AddParameterWithTypeValue("ENDINDEX", OracleDbType.Long, inEndIndex)
                'End If
                query.AddParameterWithTypeValue("STARTINDEX", OracleDbType.Long, inStartIndex)
                query.AddParameterWithTypeValue("ENDINDEX", OracleDbType.Long, inEndIndex)
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                'データ取得
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
            Return dt
        End Function

        ''' <summary>
        ''' 顧客一覧件数取得(予約有)
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inSearchType">検索条件</param>
        ''' <param name="inSearchValue">検索値</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <param name="inBranchOperatingDateTime">当日の営業開始時刻</param>
        ''' <returns>顧客一覧件数取得(予約有)</returns>
        ''' <remarks></remarks>
        ''' <hitory></hitory>
        Public Function GetCustomerListCount(ByVal inDealerCode As String, _
                                             ByVal inBranchCode As String, _
                                             ByVal inSearchType As String, _
                                             ByVal inSearchValue As String, _
                                             ByVal inNowDate As Date, _
                                             ByVal inBranchOperatingDateTime As Date) As SC3240401CustomerInfoCountDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inSearchType = {4}," & _
                         "inSearchValue = {5}, inNowDate = {6}, inBranchOperatingDateTime = {7}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inBranchCode, inSearchType, inSearchValue _
                        , inNowDate.ToString(CultureInfo.CurrentCulture) _
                        , inBranchOperatingDateTime.ToString(CultureInfo.CurrentCulture)))

            'データ格納用
            Dim dt As SC3240401CustomerInfoCountDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine("SELECT /* SC3240401_008 */")
                .AppendLine("    COUNT(1) AS COUNT")
                .AppendLine("    FROM (")
                .AppendLine("        SELECT A1.CST_ID")
                .AppendLine("              ,A1.VCL_ID")
                .AppendLine("          FROM TB_M_CUSTOMER_VCL A1")
                .AppendLine("              ,TB_M_CUSTOMER A2")
                .AppendLine("              ,TB_M_CUSTOMER_DLR A3")
                .AppendLine("              ,TB_M_VEHICLE A4")
                .AppendLine("              ,TB_M_VEHICLE_DLR A5")
                .AppendLine("         WHERE A1.CST_ID = A2.CST_ID")
                .AppendLine("           AND A1.DLR_CD = A3.DLR_CD")
                .AppendLine("           AND A1.CST_ID = A3.CST_ID")
                .AppendLine("           AND A1.VCL_ID = A4.VCL_ID")
                .AppendLine("           AND A1.DLR_CD = A5.DLR_CD")
                .AppendLine("           AND A1.VCL_ID = A5.VCL_ID")
                .AppendLine("           AND A1.DLR_CD = :DLR_CD")
                .AppendLine("           AND A3.DLR_CD = :DLR_CD")
                .AppendLine("           AND A5.DLR_CD = :DLR_CD")

                '検索条件追加
                If SearchTypeRegisterNo.Equals(inSearchType) Then
                    '車両登録No（後方一致）
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    '.AppendLine("       AND A5.REG_NUM_SEARCH LIKE (UPPER(:REG_NUM_SEARCH)) ")
                    .AppendLine("       AND A5.REG_NUM_SEARCH_REV LIKE (UPPER(:REG_NUM_SEARCH_REV)) ")
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                ElseIf SearchTypeCustomerName.Equals(inSearchType) Then
                    '顧客氏名（前方一致）
                    .AppendLine("       AND A2.CST_NAME_SEARCH LIKE (UPPER(:CST_NAME_SEARCH)) ")

                ElseIf SearchTypeVin.Equals(inSearchType) Then
                    'VIN（後方一致）
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    '.AppendLine("       AND A4.VCL_VIN_SEARCH LIKE (UPPER(:VCL_VIN_SEARCH)) ")
                    .AppendLine("       AND A4.VCL_VIN_SEARCH_REV LIKE (UPPER(:VCL_VIN_SEARCH_REV)) ")
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                End If

                .AppendLine("    ) W1")
                .AppendLine("    ,(SELECT T1.CST_ID")
                .AppendLine("            ,T1.VCL_ID")
                .AppendLine("        FROM TB_T_SERVICEIN T1")
                .AppendLine("            ,TB_T_JOB_DTL T2")
                .AppendLine("            ,TB_T_STALL_USE T3")
                .AppendLine("       WHERE T1.SVCIN_ID = T2.SVCIN_ID")
                .AppendLine("         AND T2.JOB_DTL_ID = T3.JOB_DTL_ID")
                .AppendLine("         AND T1.DLR_CD = :DLR_CD")
                .AppendLine("         AND T1.BRN_CD = :BRN_CD")
                .AppendLine("         AND T2.DLR_CD = :DLR_CD")
                .AppendLine("         AND T2.BRN_CD = :BRN_CD")
                .AppendLine("         AND T2.CANCEL_FLG = :CANCEL_FLG_0")
                .AppendLine("         AND T3.DLR_CD = :DLR_CD")
                .AppendLine("         AND T3.BRN_CD = :BRN_CD")
                .AppendLine("         AND (")
                .AppendLine("             T1.SVC_STATUS IN ('00', '01', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12')")
                .AppendLine("             OR T3.RSLT_END_DATETIME >= :BRANCH_OPERATION_DATE")
                .AppendLine("         )")

                '検索条件追加
                If SearchTypeDMSJobDtlId.Equals(inSearchType) Then
                    'DMS予約番号（後方一致）
                    .AppendLine("     AND T2.DMS_JOB_DTL_ID LIKE (UPPER(:DMS_JOB_DTL_ID)) ")
                End If

                If SearchTypeOrderNo.Equals(inSearchType) Then
                    'RO番号（後方一致）
                    .AppendLine("     AND T1.RO_NUM LIKE (:RO_NUM) ")
                End If

                .AppendLine("    GROUP BY T1.CST_ID, T1.VCL_ID")
                .AppendLine("    ) W2")
                .AppendLine("    WHERE W1.CST_ID = W2.CST_ID")
                .AppendLine("      AND W1.VCL_ID = W2.VCL_ID")
            End With

            Using query As New DBSelectQuery(Of SC3240401CustomerInfoCountDataTable)("SC3240401_008")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                query.AddParameterWithTypeValue("BRANCH_OPERATION_DATE", OracleDbType.Date, inBranchOperatingDateTime)

                '検索条件追加
                If SearchTypeRegisterNo.Equals(inSearchType) Then
                    '車両登録No（後方一致）
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    'query.AddParameterWithTypeValue("REG_NUM_SEARCH", OracleDbType.NVarchar2, String.Concat(LikeWord, inSearchValue))
                    query.AddParameterWithTypeValue("REG_NUM_SEARCH_REV", OracleDbType.NVarchar2, StrReverse(String.Concat(LikeWord, inSearchValue)))
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                ElseIf SearchTypeCustomerName.Equals(inSearchType) Then
                    '顧客氏名（前方一致）
                    query.AddParameterWithTypeValue("CST_NAME_SEARCH", OracleDbType.NVarchar2, String.Concat(inSearchValue, LikeWord))

                ElseIf SearchTypeVin.Equals(inSearchType) Then
                    'VIN（後方一致）
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                    'query.AddParameterWithTypeValue("VCL_VIN_SEARCH", OracleDbType.NVarchar2, String.Concat(LikeWord, inSearchValue))
                    query.AddParameterWithTypeValue("VCL_VIN_SEARCH_REV", OracleDbType.NVarchar2, StrReverse(String.Concat(LikeWord, inSearchValue)))
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                ElseIf SearchTypeDMSJobDtlId.Equals(inSearchType) Then
                    'DMS予約番号（後方一致）
                    query.AddParameterWithTypeValue("DMS_JOB_DTL_ID", OracleDbType.NVarchar2, String.Concat(LikeWord, inSearchValue))

                ElseIf SearchTypeOrderNo.Equals(inSearchType) Then
                    'RO番号（後方一致）
                    query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, String.Concat(LikeWord, inSearchValue))

                End If

                'データ取得
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
            Return dt
        End Function

        ''' <summary>
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
        ' ''' 当日移行の予約情報取得
        ''' 予約情報取得
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inSearchType">検索条件</param>
        ''' <param name="inSearchValue">検索値</param>
        ''' <param name="inNowDate">現在日時</param>
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
        ' ''' <param name="dtCustomerInfo">顧客一覧情報</param>
        ''' <param name="customerInfoList">顧客IDと車両IDのリスト</param>
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
        ''' <returns>予約情報</returns>
        ''' <remarks></remarks>
        ''' <hitory>
        ''' 2014/02/27 TMEJ 小澤 タブレット版SMB チーフテクニシャン機能開発
        ''' </hitory>
        '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
        'Public Function GetReserveList(ByVal inDealerCode As String, _
        '                       ByVal inBranchCode As String, _
        '                       ByVal inSearchType As String, _
        '                       ByVal inSearchValue As String, _
        '                       ByVal inNowDate As Date, _
        '                       ByVal dtCustomerInfo As SC3240401CustomerInfoDataTable) As SC3240401ReserveInfoDataTable
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inSearchType = {4}, inSearchValue = {5}" & _
        '                 "inNowDate = {6}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inDealerCode, inBranchCode, inSearchType, inSearchValue _
        '                , inNowDate.ToString(CultureInfo.CurrentCulture)))
        Public Function GetReserveList(ByVal inDealerCode As String, _
                                       ByVal inBranchCode As String, _
                                       ByVal inSearchType As String, _
                                       ByVal inSearchValue As String, _
                                       ByVal inNowDate As Date, _
                                       ByVal inBranchOperatingDateTime As Date, _
                                       ByVal customerInfoList As List(Of String)) As SC3240401ReserveInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inSearchType = {4}, inSearchValue = {5}" & _
                         "inNowDate = {6}, inBranchOperatingDateTime = {7}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inBranchCode, inSearchType, inSearchValue _
                        , inNowDate.ToString(CultureInfo.CurrentCulture) _
                        , inBranchOperatingDateTime.ToString(CultureInfo.CurrentCulture)))
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

            'SQL格納用
            Dim sqlCustomerIdAndVehicleId As New StringBuilder

            'カウンター
            Dim i As Integer = 1
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            'Dim j As Integer = 0
            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
            Dim k As Integer = 1

            '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
            'For Each drCustomerInfo As SC3240401CustomerInfoRow In dtCustomerInfo
            For Each customerInfo As String In customerInfoList
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                'IN句の1000行制限の制御
                If k = 1000 Then

                    sqlCustomerIdAndVehicleId.Append(")")
                    sqlCustomerIdAndVehicleId.Append(" OR (T1.CST_ID, T2.VCL_ID) IN ( ")

                    k = 1
                End If

                '整備受注NOと枝番
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                'sqlCustomerIdAndVehicleId.Append("(")
                'sqlCustomerIdAndVehicleId.Append(drCustomerInfo.CST_ID)
                'sqlCustomerIdAndVehicleId.Append(",")
                'sqlCustomerIdAndVehicleId.Append(drCustomerInfo.VCL_ID)
                'sqlCustomerIdAndVehicleId.Append(")")
                sqlCustomerIdAndVehicleId.Append(customerInfo)
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                'If Not k = 999 AndAlso Not dtCustomerInfo.Count() = i Then
                If Not k = 999 AndAlso Not customerInfoList.Count = i Then
                    '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                    sqlCustomerIdAndVehicleId.Append(",")
                End If
                i = i + 1
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                'j = j + 1
                '2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                k = k + 1
            Next

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine("SELECT /* SC3240401_002 */ ")
                .AppendLine("       T1.CST_ID ")
                .AppendLine("      ,T1.VCL_ID ")
                .AppendLine("      ,T1.SVCIN_ID ")
                .AppendLine("      ,T2.JOB_DTL_ID ")
                .AppendLine("      ,T3.STALL_USE_ID ")
                .AppendLine("      ,T1.SVC_STATUS ")
                .AppendLine("      ,T2.INSPECTION_STATUS ")
                .AppendLine("      ,T3.STALL_USE_STATUS ")
                .AppendLine("      ,CASE ")
                .AppendLine("            WHEN T3.RSLT_START_DATETIME <> :MINDATE THEN T3.RSLT_START_DATETIME ")
                .AppendLine("            ELSE T3.SCHE_START_DATETIME END AS START_DATETIME ")
                .AppendLine("      ,CASE ")
                .AppendLine("            WHEN T3.RSLT_END_DATETIME <> :MINDATE THEN T3.RSLT_END_DATETIME ")
                .AppendLine("            WHEN T3.PRMS_END_DATETIME <> :MINDATE THEN T3.PRMS_END_DATETIME ")
                .AppendLine("            ELSE T3.SCHE_END_DATETIME END AS END_DATETIME ")
                .AppendLine("      ,T1.RESV_STATUS ")
                .AppendLine("      ,NVL(TRIM(T4.STALL_NAME_SHORT), TRIM(T4.STALL_NAME)) AS STALL_NAME ")
                .AppendLine("      ,NVL(TRIM(CONCAT(T5.UPPER_DISP, T5.LOWER_DISP)), NVL(TRIM(T7.SVC_CLASS_NAME), T7.SVC_CLASS_NAME_ENG)) AS SERVICE_NAME ")
                .AppendLine("      ,DECODE(T1.RSLT_SVCIN_DATETIME, :MINDATE, TO_DATE(NULL), T1.RSLT_SVCIN_DATETIME) AS RSLT_SVCIN_DATETIME ")
                .AppendLine("      ,T6.STF_NAME ")
                .AppendLine("      ,:ADDTYPE_NONE AS ADDTYPE ")
                .AppendLine("      ,DECODE(T1.RSLT_SVCIN_DATETIME, :MINDATE, 1, 0) AS SORTKEY1_RSLT_SVCIN_TYPE ")
                .AppendLine("      ,TO_CHAR(CASE                                                                      ")
                .AppendLine("                    WHEN T3.RSLT_START_DATETIME <> :MINDATE THEN T3.RSLT_START_DATETIME ")
                .AppendLine("                    ELSE T3.SCHE_START_DATETIME END, 'YYYYMMDDHH24MI') AS SORTKEY2_START_DATETIME ")
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                .AppendLine("      ,T3.TEMP_FLG ")
                .AppendLine("      ,'1' AS SORTKEY3 ")
                If SearchTypeGetRepairOrder.Equals(inSearchType) Then
                    .AppendLine("      ,T2.DMS_JOB_DTL_ID ")
                    .AppendLine("      ,T8.VCL_VIN ")
                    .AppendLine("      ,T9.VISIT_ID AS VISITSEQ ")
                    .AppendLine("      ,T9.RO_NUM ")
                Else
                    .AppendLine("      ,N' ' AS RO_NUM ")
                End If
                .AppendLine("	   ,0 AS RO_SEQ ")
                .AppendLine("	   ,N' ' AS RO_STATUS ")
                .AppendLine("      ,CASE ")
                .AppendLine("            WHEN T1.RSLT_SVCIN_DATETIME <> :MINDATE THEN T1.RSLT_SVCIN_DATETIME ")
                .AppendLine("            ELSE :MAXDATE END AS CUSTOMER_APPROVAL_DATETIME ")
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                .AppendLine("  FROM ")
                .AppendLine("       TB_T_SERVICEIN T1 ")
                .AppendLine("      ,TB_T_JOB_DTL T2 ")
                .AppendLine("      ,TB_T_STALL_USE T3 ")
                .AppendLine("      ,TB_M_STALL T4 ")
                .AppendLine("      ,TB_M_MERCHANDISE T5 ")
                .AppendLine("      ,TB_M_STAFF T6 ")
                .AppendLine("      ,TB_M_SERVICE_CLASS T7 ")
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                If SearchTypeGetRepairOrder.Equals(inSearchType) Then
                    .AppendLine("		 ,TB_M_VEHICLE T8")
                    .AppendLine("		 ,( SELECT T11.SVCIN_ID")
                    .AppendLine("				  ,MAX(T11.RO_NUM) AS RO_NUM")
                    .AppendLine("				  ,MAX(T11.VISIT_ID) AS VISIT_ID")
                    .AppendLine("			FROM TB_T_RO_INFO T11")
                    .AppendLine("			GROUP BY T11.SVCIN_ID")
                    .AppendLine("		 ) T9")
                End If
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                .AppendLine(" WHERE ")
                .AppendLine("       T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine("   AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                '.AppendLine("   AND T3.STALL_ID = T4.STALL_ID ")
                .AppendLine("   AND T3.STALL_ID = T4.STALL_ID(+) ")
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                .AppendLine("   AND T2.MERC_ID = T5.MERC_ID(+) ")
                .AppendLine("   AND T1.PIC_SA_STF_CD = T6.STF_CD(+) ")
                .AppendLine("   AND T2.SVC_CLASS_ID = T7.SVC_CLASS_ID(+) ")
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                If SearchTypeGetRepairOrder.Equals(inSearchType) Then
                    .AppendLine("	  AND T1.VCL_ID = T8.VCL_ID")
                    .AppendLine("	  AND T1.SVCIN_ID = T9.SVCIN_ID")
                End If
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                .AppendLine("   AND T1.DLR_CD = :DLR_CD ")
                .AppendLine("   AND T1.BRN_CD = :BRN_CD ")
                .AppendLine("   AND T2.DLR_CD = :DLR_CD ")
                .AppendLine("   AND T2.BRN_CD = :BRN_CD ")
                .AppendLine("   AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine("   AND T3.DLR_CD = :DLR_CD ")
                .AppendLine("   AND T3.BRN_CD = :BRN_CD ")
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                '.AppendLine("   AND T3.TEMP_FLG = :TEMP_FLG_0 ")
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                '.AppendLine("   AND CASE WHEN T3.RSLT_START_DATETIME <> :MINDATE THEN T3.RSLT_START_DATETIME ")
                '.AppendLine("            ELSE T3.SCHE_START_DATETIME END >= TRUNC(:NOWDATE) ")
                '.AppendLine("   AND CASE WHEN T3.RSLT_START_DATETIME <> :MINDATE THEN T3.RSLT_START_DATETIME ")
                '.AppendLine("            ELSE T3.SCHE_START_DATETIME END <= TRUNC(:MAXDATE) ")
                .AppendLine("  AND (")
                .AppendLine("          T1.SVC_STATUS IN ('00', '01', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12')")
                .AppendLine("          OR T3.RSLT_END_DATETIME >= :BRANCH_OPERATION_DATE")
                .AppendLine("  )")
                '.AppendLine("   AND T4.DLR_CD = :DLR_CD ")
                '.AppendLine("   AND T4.BRN_CD = :BRN_CD ")
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                '2014/02/27 TMEJ 小澤 タブレット版SMB チーフテクニシャン機能開発 START
                .AppendLine("   AND T3.STALL_USE_ID = (SELECT MAX(K.STALL_USE_ID) ")
                .AppendLine("                            FROM TB_T_STALL_USE K ")
                .AppendLine("                           WHERE K.JOB_DTL_ID = T2.JOB_DTL_ID) ")
                '2014/02/27 TMEJ 小澤 タブレット版SMB チーフテクニシャン機能開発 END

                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                '.AppendLine("   AND NOT EXISTS (SELECT 1 ")
                '.AppendLine("                     FROM TB_T_SERVICEIN E1 ")
                '.AppendLine("                    WHERE E1.SVCIN_ID = T1.SVCIN_ID ")
                '.AppendLine("                      AND E1.SVC_STATUS = :SVC_STATUS_02) ")
                '.AppendLine("   AND NOT EXISTS (SELECT 1 ")
                '.AppendLine("                     FROM TB_T_SERVICEIN D1 ")
                '.AppendLine("                         ,TB_T_JOB_DTL D2 ")
                '.AppendLine("                         ,TB_T_STALL_USE D3 ")
                '.AppendLine("                    WHERE D1.SVCIN_ID = D2.SVCIN_ID ")
                '.AppendLine("                      AND D2.JOB_DTL_ID = D3.JOB_DTL_ID ")
                '.AppendLine("                      AND D1.SVCIN_ID = T1.SVCIN_ID ")
                '.AppendLine("                      AND D2.JOB_DTL_ID = T2.JOB_DTL_ID ")
                '.AppendLine("                      AND D3.STALL_USE_ID = T3.STALL_USE_ID ")
                '.AppendLine("                      AND D1.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_1 ")
                '.AppendLine("                      AND D3.STALL_ID = :STALL_ID_0) ")
                .AppendLine("   AND (T3.STALL_ID <> 0 OR T3.TEMP_FLG = '1') ")

                '検索条件追加
                If SearchTypeDMSJobDtlId.Equals(inSearchType) Then
                    'DMS予約番号（後方一致）
                    .AppendLine("    AND T1.SVCIN_ID IN ")
                    .AppendLine("    ( ")
                    .AppendLine("        SELECT T20.SVCIN_ID ")
                    .AppendLine("          FROM TB_T_JOB_DTL T20 ")
                    .AppendLine("         WHERE T20.DMS_JOB_DTL_ID LIKE (UPPER(:DMS_JOB_DTL_ID)) ")
                    .AppendLine("    ) ")
                End If
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                '検索条件追加
                If SearchTypeOrderNo.Equals(inSearchType) Then
                    'RO番号（後方一致）
                    .AppendLine("   AND T1.RO_NUM LIKE (:RO_NUM) ")

                End If

                .AppendLine("   AND (T1.CST_ID, T1.VCL_ID) IN ( ")
                .AppendLine(sqlCustomerIdAndVehicleId.ToString)
                .AppendLine("   ) ")

            End With

            Using query As New DBSelectQuery(Of SC3240401ReserveInfoDataTable)("SC3240401_002")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                'query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)
                'query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_1", OracleDbType.NVarchar2, AcceptanceTypeWalkIn)
                'query.AddParameterWithTypeValue("STALL_ID_0", OracleDbType.Decimal, StallIdWalkIn)
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                ''Tempエリアのチップを検索して、出てこないように
                'query.AddParameterWithTypeValue("TEMP_FLG_0", OracleDbType.NVarchar2, TempFlgOff)
                ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                'query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)
                query.AddParameterWithTypeValue("ADDTYPE_NONE", OracleDbType.NVarchar2, AddRecordTypeOff)
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする START
                query.AddParameterWithTypeValue("BRANCH_OPERATION_DATE", OracleDbType.Date, inBranchOperatingDateTime)

                '検索条件追加
                If SearchTypeDMSJobDtlId.Equals(inSearchType) Then
                    'DMS予約番号（後方一致）
                    query.AddParameterWithTypeValue("DMS_JOB_DTL_ID", OracleDbType.NVarchar2, String.Concat(LikeWord, inSearchValue))
                End If
                ' 2017/07/12 NSK 河谷 REQ-SVT-TMT-20160906-001 チップ検索の検索範囲をPC-SMBと同じにする END

                '検索条件追加
                If SearchTypeOrderNo.Equals(inSearchType) Then
                    'RO番号（後方一致）
                    query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, String.Concat(LikeWord, inSearchValue))

                End If

                '検索結果返却
                Dim dt As SC3240401ReserveInfoDataTable = query.GetData()
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END OUT:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
                Return dt
            End Using

        End Function

        ''' <summary>
        ''' 受付・追加作業サブエリアの予約情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inSearchType">検索条件</param>
        ''' <param name="inSearchValue">検索値</param>
        ''' <param name="customerInfoList">顧客IDと車両IDのリスト</param>
        ''' <returns>予約情報</returns>
        ''' <remarks></remarks>
        ''' <hitory>
        ''' </hitory>
        Public Function GetReceptionAdditionalWorkReserveList(ByVal inDealerCode As String, _
                                       ByVal inBranchCode As String, _
                                       ByVal inSearchType As String, _
                                       ByVal inSearchValue As String, _
                                       ByVal customerInfoList As List(Of String)) As SC3240401ReserveInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inSearchType = {4}, inSearchValue = {5}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inDealerCode, inBranchCode, inSearchType, inSearchValue))

            'SQL格納用
            Dim sqlCustomerIdAndVehicleId As New StringBuilder

            'カウンター
            Dim i As Integer = 1
            Dim j As Integer = 1

            For Each customerInfo As String In customerInfoList

                'IN句の1000行制限の制御
                If j = 1000 Then

                    sqlCustomerIdAndVehicleId.Append(")")
                    sqlCustomerIdAndVehicleId.Append(" OR (T1.CST_ID, T2.VCL_ID) IN ( ")

                    j = 1
                End If

                '整備受注NOと枝番
                sqlCustomerIdAndVehicleId.Append(customerInfo)

                If Not j = 999 AndAlso Not customerInfoList.Count = i Then
                    sqlCustomerIdAndVehicleId.Append(",")
                End If
                i = i + 1
                j = j + 1
            Next

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine("SELECT /* SC3240401_006 */ ")
                .AppendLine("     T1.CST_ID")
                .AppendLine("    ,T1.VCL_ID")
                .AppendLine("    ,T1.SVCIN_ID")
                .AppendLine("    ,T2.JOB_DTL_ID")
                .AppendLine("    ,T3.STALL_USE_ID")
                .AppendLine("    ,T1.SVC_STATUS")
                .AppendLine("    ,T2.INSPECTION_STATUS")
                .AppendLine("    ,T3.STALL_USE_STATUS")
                .AppendLine("    ,CASE WHEN T3.RSLT_START_DATETIME <> :MINDATE THEN T3.RSLT_START_DATETIME ")
                .AppendLine("          ELSE T3.SCHE_START_DATETIME END AS START_DATETIME ")
                .AppendLine("    ,:MINDATE AS END_DATETIME")
                .AppendLine("    ,T1.RESV_STATUS")
                .AppendLine("    ,N' ' AS STALL_NAME")
                .AppendLine("    ,NVL(TRIM(CONCAT(T5.UPPER_DISP, T5.LOWER_DISP)), NVL(TRIM(T7.SVC_CLASS_NAME), T7.SVC_CLASS_NAME_ENG)) AS SERVICE_NAME")
                .AppendLine("    ,DECODE(T1.RSLT_SVCIN_DATETIME, :MINDATE, TO_DATE(NULL), T1.RSLT_SVCIN_DATETIME) AS RSLT_SVCIN_DATETIME")
                .AppendLine("    ,T6.STF_NAME")
                .AppendLine("    ,N'1' AS ADDTYPE")
                .AppendLine("    ,0 AS SORTKEY1_RSLT_SVCIN_TYPE")
                .AppendLine("    ,TO_CHAR(CASE WHEN T4.RO_STATUS IN (:RO_STATUS_50, :RO_STATUS_60) THEN :MINDATE ")
                .AppendLine("                  WHEN T3.RSLT_START_DATETIME <> :MINDATE THEN T3.RSLT_START_DATETIME ")
                .AppendLine("                  ELSE T3.SCHE_START_DATETIME END, 'YYYYMMDDHH24MI') AS SORTKEY2_START_DATETIME ")
                .AppendLine("    ,CASE WHEN T4.RO_STATUS IN :RO_STATUS_20 THEN '0' ")
                .AppendLine("          ELSE '1' END AS SORTKEY3 ")
                .AppendLine("    ,T3.TEMP_FLG ")
                .AppendLine("    ,T4.RO_NUM ")
                .AppendLine("    ,T4.RO_SEQ ")
                .AppendLine("    ,T4.RO_STATUS ")
                .AppendLine("    ,T4.RO_APPROVAL_DATETIME AS CUSTOMER_APPROVAL_DATETIME")
                If SearchTypeGetRepairOrder.Equals(inSearchType) Then
                    .AppendLine("      ,T2.DMS_JOB_DTL_ID ")
                    .AppendLine("      ,T8.VCL_VIN ")
                    .AppendLine("      ,T4.VISIT_ID AS VISITSEQ ")
                End If
                .AppendLine("FROM")
                .AppendLine("	  TB_T_SERVICEIN T1")
                .AppendLine("    , TB_T_JOB_DTL T2")
                .AppendLine("    , TB_T_STALL_USE T3")
                .AppendLine("    , TB_T_RO_INFO T4")
                .AppendLine("    , TB_M_MERCHANDISE T5")
                .AppendLine("    , TB_M_STAFF T6")
                .AppendLine("    , TB_M_SERVICE_CLASS T7")
                If SearchTypeGetRepairOrder.Equals(inSearchType) Then
                    .AppendLine("		 ,TB_M_VEHICLE T8")
                End If
                .AppendLine("WHERE")
                .AppendLine("	T1.SVCIN_ID = T2.SVCIN_ID")
                .AppendLine("	AND T2.JOB_DTL_ID = T3.JOB_DTL_ID")
                .AppendLine("	AND T1.SVCIN_ID = T4.SVCIN_ID")
                .AppendLine("	AND T1.RO_NUM <> ' '")
                .AppendLine("	AND T2.MERC_ID = T5.MERC_ID (+)")
                .AppendLine("	AND T1.PIC_SA_STF_CD = T6.STF_CD (+)")
                .AppendLine("	AND T2.SVC_CLASS_ID = T7.SVC_CLASS_ID (+)")
                If SearchTypeGetRepairOrder.Equals(inSearchType) Then
                    .AppendLine("	  AND T1.VCL_ID = T8.VCL_ID")
                End If
                .AppendLine("	AND T1.DLR_CD = :DLR_CD")
                .AppendLine("	AND T1.BRN_CD = :BRN_CD")
                .AppendLine("	AND T2.DLR_CD = :DLR_CD")
                .AppendLine("	AND T2.BRN_CD = :BRN_CD")
                .AppendLine("	AND T3.DLR_CD = :DLR_CD")
                .AppendLine("	AND T3.BRN_CD = :BRN_CD")
                .AppendLine("	AND T4.DLR_CD = :DLR_CD")
                .AppendLine("	AND T4.BRN_CD = :BRN_CD")
                .AppendLine("	AND T2.JOB_DTL_ID = (SELECT MIN(T6.JOB_DTL_ID)")
                .AppendLine("    					 FROM TB_T_JOB_DTL T6")
                .AppendLine("    					 WHERE T6.SVCIN_ID = T1.SVCIN_ID")
                .AppendLine("    					 AND T6.CANCEL_FLG = :CANCEL_FLG_0)")
                .AppendLine("	AND T3.STALL_USE_ID = (SELECT MAX(STALL_USE_ID)")
                .AppendLine(" 						   FROM TB_T_STALL_USE T12")
                .AppendLine("    					   WHERE T12.JOB_DTL_ID = T2.JOB_DTL_ID)")
                .AppendLine("	AND")
                .AppendLine("	(")
                .AppendLine("    	(")
                .AppendLine("        	T4.RO_STATUS IN (:RO_STATUS_50, :RO_STATUS_60)")
                .AppendLine("        	AND EXISTS (")
                .AppendLine("               SELECT 1")
                .AppendLine("            	FROM TB_T_JOB_INSTRUCT T13")
                .AppendLine("                  , TB_T_JOB_DTL T15")
                .AppendLine("            	   , TB_T_STALL_USE T16")
                .AppendLine("            	WHERE T13.RO_NUM = T4.RO_NUM")
                .AppendLine("            	  AND T13.RO_SEQ = T4.RO_SEQ")
                .AppendLine("            	  AND T13.JOB_DTL_ID = T15.JOB_DTL_ID")
                .AppendLine("            	  AND T13.JOB_DTL_ID = T16.JOB_DTL_ID")
                .AppendLine("            	  AND T15.DLR_CD = :DLR_CD")
                .AppendLine("            	  AND T15.BRN_CD = :BRN_CD")
                .AppendLine("            	  AND T15.CANCEL_FLG = :CANCEL_FLG_0")
                .AppendLine("            	  AND T13.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG_0")
                .AppendLine("        	      AND EXISTS (")
                .AppendLine("            	      SELECT 1")
                .AppendLine("            	        FROM TB_T_STALL_USE T17")
                .AppendLine("                      WHERE T15.JOB_DTL_ID = T17.JOB_DTL_ID")
                .AppendLine("                     HAVING MAX(T17.STALL_USE_ID) = T16.STALL_USE_ID")
                .AppendLine("                 )")
                .AppendLine("            	  AND T16.TEMP_FLG = :TEMP_FLG_0")
                .AppendLine("        	 )")
                .AppendLine("    	)")
                .AppendLine("   	OR  T4.RO_STATUS IN (:RO_STATUS_20)	")
                .AppendLine("	)")

                '検索条件追加
                If SearchTypeDMSJobDtlId.Equals(inSearchType) Then
                    'DMS予約番号（後方一致）
                    .AppendLine("    AND T1.SVCIN_ID IN ")
                    .AppendLine("    ( ")
                    .AppendLine("        SELECT T20.SVCIN_ID ")
                    .AppendLine("          FROM TB_T_JOB_DTL T20 ")
                    .AppendLine("         WHERE T20.DMS_JOB_DTL_ID LIKE (UPPER(:DMS_JOB_DTL_ID)) ")
                    .AppendLine("    ) ")
                End If

                '検索条件追加
                If SearchTypeOrderNo.Equals(inSearchType) Then
                    'RO番号（後方一致）
                    .AppendLine("   AND T1.RO_NUM LIKE (:RO_NUM) ")

                End If

                .AppendLine("   AND (T1.CST_ID, T1.VCL_ID) IN ( ")
                .AppendLine(sqlCustomerIdAndVehicleId.ToString)
                .AppendLine("   ) ")
            End With

            Using query As New DBSelectQuery(Of SC3240401ReserveInfoDataTable)("SC3240401_006")
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                query.AddParameterWithTypeValue("RO_STATUS_20", OracleDbType.NVarchar2, RoStatusWaitingForeManApprove)
                query.AddParameterWithTypeValue("RO_STATUS_50", OracleDbType.NVarchar2, RoStatusCustomerApprove)
                query.AddParameterWithTypeValue("RO_STATUS_60", OracleDbType.NVarchar2, RoStatusWorking)
                query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG_0", OracleDbType.NVarchar2, StartworkInstructFlgOff)
                query.AddParameterWithTypeValue("TEMP_FLG_0", OracleDbType.NVarchar2, TempFlgOff)

                '検索条件追加
                If SearchTypeDMSJobDtlId.Equals(inSearchType) Then
                    'DMS予約番号（後方一致）
                    query.AddParameterWithTypeValue("DMS_JOB_DTL_ID", OracleDbType.NVarchar2, String.Concat(LikeWord, inSearchValue))
                End If

                '検索条件追加
                If SearchTypeOrderNo.Equals(inSearchType) Then
                    'RO番号（後方一致）
                    query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, String.Concat(LikeWord, inSearchValue))

                End If

                '検索結果返却
                Dim dt As SC3240401ReserveInfoDataTable = query.GetData()
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END OUT:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' 予約情報取得
        ''' </summary>
        ''' <param name="inStallUseId">ストール利用ID</param>
        ''' <returns>予約情報</returns>
        ''' <remarks></remarks>
        ''' <hitory></hitory>
        Public Function GetStallUseInfo(ByVal inStallUseId As Decimal) As SC3240401StallUseInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inStallUseId = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inStallUseId.ToString(CultureInfo.CurrentCulture)))

            Dim dt As SC3240401StallUseInfoDataTable

            Using query As New DBSelectQuery(Of SC3240401StallUseInfoDataTable)("SC3240401_003")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("SELECT /* SC3240401_003 */ ")
                    .AppendLine("       T3.STALL_USE_ID ")
                    .AppendLine("      ,CASE ")
                    .AppendLine("            WHEN T3.RSLT_START_DATETIME = :MINDATE THEN T3.SCHE_START_DATETIME ")
                    .AppendLine("            ELSE T3.RSLT_START_DATETIME END AS START_DATE ")
                    .AppendLine("      ,T3.STALL_USE_STATUS ")
                    .AppendLine("      ,T1.SVC_STATUS ")
                    .AppendLine("      ,T2.INSPECTION_STATUS ")
                    .AppendLine("  FROM ")
                    .AppendLine("       TB_T_SERVICEIN T1 ")
                    .AppendLine("      ,TB_T_JOB_DTL T2 ")
                    .AppendLine("      ,TB_T_STALL_USE T3 ")
                    .AppendLine(" WHERE ")
                    .AppendLine("       T1.SVCIN_ID = T2.SVCIN_ID ")
                    .AppendLine("   AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                    .AppendLine("   AND T3.STALL_USE_ID = :STALL_USE_ID ")
                    .AppendLine("   AND T1.SVC_STATUS <> :SVC_STATUS_02 ")
                    .AppendLine("   AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("STALL_USE_ID", OracleDbType.Decimal, inStallUseId)
                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)


                '検索結果返却
                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
            Return dt
        End Function

        ''' <summary>
        ''' RO情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inRoNum">RO番号</param>
        ''' <param name="inRoSeq">RO連番</param>
        ''' <returns>RO情報</returns>
        ''' <remarks></remarks>
        ''' <hitory></hitory>
        Public Function GetRoInfo(ByVal inDealerCode As String, _
                                  ByVal inBranchCode As String, _
                                  ByVal inRoNum As String, _
                                  ByVal inRoSeq As Decimal) As SC3240401RoInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inRoNum = {4}, inRoSeq = {5}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inDealerCode, inBranchCode, inRoNum, inRoSeq))

            Dim dt As SC3240401RoInfoDataTable
            Using query As New DBSelectQuery(Of SC3240401RoInfoDataTable)("SC3240401_007")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("SELECT /* SC3240401_007 */ ")
                    .AppendLine("	 T1.DLR_CD ")
                    .AppendLine("	,T1.BRN_CD ")
                    .AppendLine("	,T1.RO_NUM ")
                    .AppendLine("	,T1.RO_SEQ ")
                    .AppendLine("	,T1.RO_STATUS ")
                    .AppendLine("FROM ")
                    .AppendLine("	TB_T_RO_INFO T1 ")
                    .AppendLine("WHERE ")
                    .AppendLine("	    T1.DLR_CD = :DLR_CD ")
                    .AppendLine("	AND T1.BRN_CD = :BRN_CD ")
                    .AppendLine("	AND T1.RO_NUM = :RO_NUM ")
                    .AppendLine("	AND T1.RO_SEQ = :RO_SEQ ")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRoNum)
                query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Decimal, inRoSeq)

                dt = query.GetData()
            End Using
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END OUT:COUNT = {2}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
            Return dt
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
        '                                ByVal inVehiceleId As Decimal) As SC3240401CustomerInfoDataTable
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START IN:inDealerCode = {2}, inCustomerId = {3}, inVehiceleId = {4}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inDealerCode, inCustomerId.ToString(CultureInfo.CurrentCulture) _
        '                , inVehiceleId.ToString(CultureInfo.CurrentCulture)))
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
                                        ByVal inSvcinId As Decimal) As SC3240401CustomerInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} START IN:inDealerCode = {2}, inCustomerId = {3}, inVehiceleId = {4}, inSvcinId = {5}" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , inDealerCode, inCustomerId.ToString(CultureInfo.CurrentCulture) _
                                    , inVehiceleId.ToString(CultureInfo.CurrentCulture) _
                                    , inSvcinId.ToString(CultureInfo.CurrentCulture)))
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
            Dim dt As SC3240401CustomerInfoDataTable

            Using query As New DBSelectQuery(Of SC3240401CustomerInfoDataTable)("SC3240401_004")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("SELECT /* SC3240401_004 */ ")
                    .AppendLine("       A1.DLR_CD ")
                    .AppendLine("      ,A2.CST_NAME AS CST_NAME ")
                    .AppendLine("      ,A5.REG_NUM AS REG_NUM ")
                    .AppendLine("      ,A4.VCL_VIN AS VCL_VIN ")
                    .AppendLine("      ,A4.VCL_KATASHIKI AS VCL_KATASHIKI ")
                    .AppendLine("      ,A2.CST_PHONE AS CST_PHONE ")
                    .AppendLine("      ,A2.CST_MOBILE AS CST_MOBILE ")
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    .AppendLine("      ,NVL(TRIM(A2.DMS_CST_CD) , A6.DMSID) AS DMS_CST_CD  ")
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                    .AppendLine("  FROM ")
                    .AppendLine("       TB_M_CUSTOMER_VCL A1 ")
                    .AppendLine("      ,TB_M_CUSTOMER A2 ")
                    .AppendLine("      ,TB_M_VEHICLE A4 ")
                    .AppendLine("      ,TB_M_VEHICLE_DLR A5 ")
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    .AppendLine("      ,TBL_SERVICE_VISIT_MANAGEMENT A6 ")
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                    .AppendLine(" WHERE ")
                    .AppendLine("       A1.CST_ID = A2.CST_ID ")
                    .AppendLine("   AND A1.VCL_ID = A4.VCL_ID ")
                    .AppendLine("   AND A1.DLR_CD = A5.DLR_CD ")
                    .AppendLine("   AND A1.VCL_ID = A5.VCL_ID ")
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                    .AppendLine("   AND A1.CST_ID = A6.CUSTID(+) ")
                    .AppendLine("   AND A1.VCL_ID = A6.VCL_ID(+) ")
                    .AppendLine("   AND A6.FREZID(+) = :SVCIN_ID ")
                    .AppendLine("   AND A6.DLRCD(+) = :DLR_CD ")
                    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
                    .AppendLine("   AND A1.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND A5.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND A1.CST_ID = :CST_ID ")
                    .AppendLine("   AND A1.VCL_ID = :VCL_ID ")

                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, inCustomerId)
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVehiceleId)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.NVarchar2, inSvcinId)
                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

                '検索結果返却
                dt = query.GetData()
            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
            Return dt
        End Function

        ' ''' <summary>
        ' ''' RO一覧取得
        ' ''' </summary>
        ' ''' <param name="inDealerCode">販売店コード</param>
        ' ''' <param name="inBranchCode">店舗コード</param>
        ' ''' <param name="inCustomerId">顧客ID</param>
        ' ''' <param name="inVehicleId">車両ID</param>
        ' ''' <param name="inNowDate">現在日時</param>
        ' ''' <returns>RO一覧</returns>
        ' ''' <remarks></remarks>
        ' ''' <hitory></hitory>
        'Public Function GetOrderList(ByVal inDealerCode As String, _
        '                             ByVal inBranchCode As String, _
        '                             ByVal inCustomerId As Decimal, _
        '                             ByVal inVehicleId As Decimal, _
        '                             ByVal inNowDate As Date) As SC3240401ReserveInfoDataTable
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START IN:inDealerCode = {2}, inBranchCode = {3}, inCustomerId = {4}, inVehicleId = {5}, inNowDate = {6}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inDealerCode, inBranchCode _
        '                , inCustomerId.ToString(CultureInfo.CurrentCulture) _
        '                , inVehicleId.ToString(CultureInfo.CurrentCulture) _
        '                , inNowDate.ToString(CultureInfo.CurrentCulture)))

        '    Dim dt As SC3240401ReserveInfoDataTable

        '    Dim sql As New StringBuilder

        '    'SQL文作成
        '    With sql
        '        .AppendLine("SELECT /* SC3240401_005 */ ")
        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '        .AppendLine("      DISTINCT ")
        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        '        .AppendLine("       T1.RO_NUM ")
        '        .AppendLine("      ,T1.CST_ID ")
        '        .AppendLine("      ,T1.VCL_ID ")
        '        .AppendLine("      ,T1.SVCIN_ID ")
        '        .AppendLine("      ,T2.JOB_DTL_ID ")
        '        .AppendLine("      ,T3.STALL_USE_ID ")
        '        .AppendLine("      ,T1.SVC_STATUS ")
        '        .AppendLine("      ,T2.INSPECTION_STATUS ")
        '        .AppendLine("      ,T3.STALL_USE_STATUS ")
        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '        .AppendLine("      ,T9.VISIT_ID AS VISITSEQ ")
        '        .AppendLine("      ,T2.DMS_JOB_DTL_ID ")
        '        .AppendLine("      ,T8.VCL_VIN ")
        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        '        .AppendLine("      ,CASE ")
        '        .AppendLine("            WHEN T3.RSLT_START_DATETIME <> :MINDATE THEN T3.RSLT_START_DATETIME ")
        '        .AppendLine("            ELSE T3.SCHE_START_DATETIME END AS START_DATETIME ")
        '        .AppendLine("      ,CASE ")
        '        .AppendLine("            WHEN T3.RSLT_END_DATETIME <> :MINDATE THEN T3.RSLT_END_DATETIME ")
        '        .AppendLine("            WHEN T3.PRMS_END_DATETIME <> :MINDATE THEN T3.PRMS_END_DATETIME ")
        '        .AppendLine("            ELSE T3.SCHE_END_DATETIME END AS END_DATETIME ")
        '        .AppendLine("      ,T1.RESV_STATUS ")
        '        .AppendLine("      ,NVL(TRIM(T4.STALL_NAME_SHORT), TRIM(T4.STALL_NAME)) AS STALL_NAME ")
        '        .AppendLine("      ,NVL(TRIM(CONCAT(T5.UPPER_DISP, T5.LOWER_DISP)), NVL(TRIM(T7.SVC_CLASS_NAME), T7.SVC_CLASS_NAME_ENG)) AS SERVICE_NAME ")
        '        .AppendLine("      ,DECODE(T1.RSLT_SVCIN_DATETIME, :MINDATE, TO_DATE(NULL), T1.RSLT_SVCIN_DATETIME) AS RSLT_SVCIN_DATETIME ")
        '        .AppendLine("      ,T6.STF_NAME ")
        '        .AppendLine("      ,:ADDTYPE_NONE AS ADDTYPE ")
        '        .AppendLine("      ,DECODE(T1.RSLT_SVCIN_DATETIME, :MINDATE, 1, 0) AS SORTKEY1_RSLT_SVCIN_TYPE ")
        '        .AppendLine("      ,TO_CHAR(CASE                                                                      ")
        '        .AppendLine("                    WHEN T3.RSLT_START_DATETIME <> :MINDATE THEN T3.RSLT_START_DATETIME ")
        '        .AppendLine("                    ELSE T3.SCHE_START_DATETIME END, 'YYYYMMDDHH24MI') AS SORTKEY2_START_DATETIME ")
        '        .AppendLine("  FROM ")
        '        .AppendLine("       TB_T_SERVICEIN T1 ")
        '        .AppendLine("      ,TB_T_JOB_DTL T2 ")
        '        .AppendLine("      ,TB_T_STALL_USE T3 ")
        '        .AppendLine("      ,TB_M_STALL T4 ")
        '        .AppendLine("      ,TB_M_MERCHANDISE T5 ")
        '        .AppendLine("      ,TB_M_STAFF T6 ")
        '        .AppendLine("      ,TB_M_SERVICE_CLASS T7 ")
        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '        .AppendLine("      ,TB_M_VEHICLE T8 ")
        '        .AppendLine("      ,TB_T_RO_INFO T9 ")
        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        '        .AppendLine(" WHERE ")
        '        .AppendLine("       T1.SVCIN_ID = T2.SVCIN_ID ")
        '        .AppendLine("   AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
        '        .AppendLine("   AND T3.STALL_ID = T4.STALL_ID ")
        '        .AppendLine("   AND T2.MERC_ID = T5.MERC_ID(+) ")
        '        .AppendLine("   AND T1.PIC_SA_STF_CD = T6.STF_CD(+) ")
        '        .AppendLine("   AND T2.SVC_CLASS_ID = T7.SVC_CLASS_ID(+) ")
        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '        .AppendLine("   AND T1.VCL_ID = T8.VCL_ID(+) ")
        '        .AppendLine("   AND T1.SVCIN_ID = T9.SVCIN_ID  ")
        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        '        .AppendLine("   AND T1.DLR_CD = :DLR_CD ")
        '        .AppendLine("   AND T1.BRN_CD = :BRN_CD ")
        '        .AppendLine("   AND T1.CST_ID = :CST_ID ")
        '        .AppendLine("   AND T1.VCL_ID = :VCL_ID ")
        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '        '.AppendLine("   AND TRIM(T1.RO_NUM) IS NOT NULL ")
        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        '        .AppendLine("   AND T2.DLR_CD = :DLR_CD ")
        '        .AppendLine("   AND T2.BRN_CD = :BRN_CD ")
        '        .AppendLine("   AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
        '        .AppendLine("   AND T3.DLR_CD = :DLR_CD ")
        '        .AppendLine("   AND T3.BRN_CD = :BRN_CD ")
        '        .AppendLine("   AND T3.SCHE_START_DATETIME >= TRUNC(:NOWDATE) ")
        '        .AppendLine("   AND T3.SCHE_START_DATETIME <= TRUNC(:MAXDATE) ")
        '        .AppendLine("   AND T4.DLR_CD = :DLR_CD ")
        '        .AppendLine("   AND T4.BRN_CD = :BRN_CD ")
        '        .AppendLine("   AND NOT EXISTS (SELECT 1 ")
        '        .AppendLine("                     FROM TB_T_SERVICEIN E1 ")
        '        .AppendLine("                    WHERE E1.SVCIN_ID = T1.SVCIN_ID ")
        '        .AppendLine("                      AND E1.SVC_STATUS = :SVC_STATUS_02) ")
        '        .AppendLine("   AND NOT EXISTS (SELECT 1 ")
        '        .AppendLine("                     FROM TB_T_SERVICEIN D1 ")
        '        .AppendLine("                         ,TB_T_JOB_DTL D2 ")
        '        .AppendLine("                         ,TB_T_STALL_USE D3 ")
        '        .AppendLine("                    WHERE D1.SVCIN_ID = D2.SVCIN_ID ")
        '        .AppendLine("                      AND D2.JOB_DTL_ID = D3.JOB_DTL_ID ")
        '        .AppendLine("                      AND D1.SVCIN_ID = T1.SVCIN_ID ")
        '        .AppendLine("                      AND D2.JOB_DTL_ID = T2.JOB_DTL_ID ")
        '        .AppendLine("                      AND D3.STALL_USE_ID = T3.STALL_USE_ID ")
        '        .AppendLine("                      AND D1.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_1 ")
        '        .AppendLine("                      AND D3.STALL_ID = :STALL_ID_0) ")

        '    End With

        '    Using query As New DBSelectQuery(Of SC3240401ReserveInfoDataTable)("SC3240401_005")
        '        query.CommandText = sql.ToString()
        '        'バインド変数
        '        query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
        '        query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)
        '        query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_1", OracleDbType.NVarchar2, AcceptanceTypeWalkIn)
        '        query.AddParameterWithTypeValue("STALL_ID_0", OracleDbType.Decimal, StallIdWalkIn)
        '        query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
        '        query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
        '        query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)
        '        query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, inCustomerId)
        '        query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVehicleId)
        '        query.AddParameterWithTypeValue("ADDTYPE_NONE", OracleDbType.NVarchar2, AddRecordTypeOff)

        '        '検索結果返却
        '        dt = query.GetData()
        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} END OUT:COUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
        '    Return dt
        'End Function

#End Region

    End Class

End Namespace

Partial Class SC3240401DataSet
End Class
