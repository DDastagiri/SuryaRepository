'-------------------------------------------------------------------------
'SC3180202TableAdapter.vb
'-------------------------------------------------------------------------
'機能：チェックシートプレビュー(データアクセスクラス)
'補足：
'作成：2014/02/01 工藤
'更新：2019/12/10 NCN 吉川（FS）次世代サービス業務における車両型式別点検の検証
'─────────────────────────────────────
Option Strict On
Option Explicit On
Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.ServerCheck.CheckResult.DataAccess.SC3180202.SC3180202DataSet

Namespace SC3180202DataSetTableAdapter

    ''' <summary>
    ''' チェックシートプレビューデータアクセスクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public NotInheritable Class SC3180202TableAdapter

        ''' <summary>
        ''' ヘッダー情報取得
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="brnCd">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <param name="isExistActive">Active存在フラグ</param>
        ''' <returns>ヘッダー情報</returns>
        ''' <remarks></remarks>
        Public Shared Function GetCheckSheetHeader(ByVal dlrCd As String, _
                                                   ByVal brnCd As String, _
                                                   ByVal roNum As String, _
                                                   ByVal isExistActive As Boolean) As SC3180202HeaderDataDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                               "{0} Start [dlrCd:{1}][brnCd:{2}][roNum:{3}]",
                               System.Reflection.MethodBase.GetCurrentMethod.Name,
                               dlrCd, brnCd, roNum))

            Using query As New DBSelectQuery(Of SC3180202HeaderDataDataTable)("SC3180202_001")
                Dim sql As New StringBuilder
                Dim headerDataDataTable As SC3180202HeaderDataDataTable


                '2017/01/24　ライフサイクル対応追加　START　↓↓↓
                If isExistActive Then
                    'SQL文作成
                    With sql
                        '2017/01/24　ライフサイクル対応（サービス来店社管理→販売店車両＋サービス入庫）　START　↓↓↓
                        .AppendLine(" SELECT")
                        .AppendLine("  /* SC3180202_001 */ ")
                        .AppendLine("     DISTINCT(VD.REG_NUM) AS VCLREGNO,")
                        .AppendLine("     MN.POSITION_TYPE,")
                        .AppendLine("     MN.NAMETITLE_NAME,")
                        .AppendLine("     SV.CONTACT_PERSON_NAME AS NAME,")
                        .AppendLine("     SV.RSLT_SVCIN_DATETIME,")
                        .AppendLine("     SV.SVCIN_MILE,")
                        .AppendLine("     SV.RO_NUM,")
                        .AppendLine("     TBU.USERNAME,")
                        .AppendLine("     SV.RSLT_DELI_DATETIME,")
                        .AppendLine("     SV.CONTACT_PHONE AS TELNO")
                        .AppendLine(" FROM")
                        .AppendLine("     TB_T_RO_INFO RI, ")
                        .AppendLine("     TB_T_SERVICEIN SV,")
                        .AppendLine("     TB_M_VEHICLE_DLR VD,  ")
                        .AppendLine("     TBL_USERS TBU,  ")
                        .AppendLine("     TB_M_CUSTOMER MC,  ")
                        .AppendLine("     TB_M_NAMETITLE MN  ")
                        .AppendLine(" WHERE")
                        .AppendLine("     RI.SVCIN_ID = SV.SVCIN_ID ")
                        .AppendLine(" AND SV.VCL_ID = VD.VCL_ID(+) ")
                        .AppendLine(" AND SV.DLR_CD = VD.DLR_CD(+) ")
                        .AppendLine(" AND SV.PIC_SA_STF_CD = TBU.ACCOUNT(+) ")
                        .AppendLine(" AND SV.CST_ID = MC.CST_ID(+) ")
                        .AppendLine(" AND MC.NAMETITLE_CD = MN.NAMETITLE_CD(+) ")
                        .AppendLine(" AND SV.DLR_CD=:DLR_CD")
                        .AppendLine(" AND SV.BRN_CD=:BRN_CD")
                        .AppendLine(" AND RI.RO_NUM=:RO_NUM")

                        '2014/07/17　サービス来店者管理へのリレーション修正　START　↓↓↓
                        '.AppendLine(" SELECT")
                        '.AppendLine("  /* SC3180202_001 */ ")
                        '.AppendLine("     MA.VCLREGNO,")
                        '.AppendLine("     MN.POSITION_TYPE,")
                        '.AppendLine("     MN.NAMETITLE_NAME,")
                        '.AppendLine("     MA.NAME,")
                        '.AppendLine("     SV.RSLT_SVCIN_DATETIME,")
                        '.AppendLine("     SV.SVCIN_MILE,")
                        '.AppendLine("     SV.RO_NUM,")
                        '.AppendLine("     TBU.USERNAME,")
                        '.AppendLine("     SV.RSLT_DELI_DATETIME,")
                        '.AppendLine("     MA.TELNO")
                        '.AppendLine(" FROM")
                        '.AppendLine("     TB_T_RO_INFO RI, ")
                        '.AppendLine("     TB_T_SERVICEIN SV,")
                        '.AppendLine("     TBL_SERVICE_VISIT_MANAGEMENT MA,  ")
                        '.AppendLine("     TBL_USERS TBU,  ")
                        '.AppendLine("     TB_M_CUSTOMER MC,  ")
                        '.AppendLine("     TB_M_NAMETITLE MN  ")
                        '.AppendLine(" WHERE")
                        '.AppendLine("     RI.SVCIN_ID = SV.SVCIN_ID ")
                        '.AppendLine(" AND RI.VISIT_ID = MA.VISITSEQ(+) ")
                        '.AppendLine(" AND SV.PIC_SA_STF_CD = TBU.ACCOUNT(+) ")
                        '.AppendLine(" AND MA.CUSTID = MC.CST_ID(+)  ")
                        '.AppendLine(" AND MC.NAMETITLE_CD = MN.NAMETITLE_CD(+)  ")
                        '.AppendLine(" AND SV.DLR_CD=:DLR_CD")
                        '.AppendLine(" AND SV.BRN_CD=:BRN_CD")
                        '.AppendLine(" AND RI.RO_NUM=:RO_NUM")
                        '2017/01/24　ライフサイクル対応（サービス来店社管理→販売店車両＋サービス入庫）　END　↑↑↑

                        '.AppendLine(" SELECT")
                        '.AppendLine("  /* SC3180202_001 */ ")
                        '.AppendLine("     MA.VCLREGNO,")
                        '.AppendLine("     MN.POSITION_TYPE,")
                        '.AppendLine("     MN.NAMETITLE_NAME,")
                        '.AppendLine("     MA.NAME,")
                        '.AppendLine("     SV.RSLT_SVCIN_DATETIME,")
                        '.AppendLine("     SV.SVCIN_MILE,")
                        '.AppendLine("     SV.RO_NUM,")
                        '.AppendLine("     TBU.USERNAME,")
                        '.AppendLine("     SV.RSLT_DELI_DATETIME,")
                        '.AppendLine("     MA.TELNO")
                        '.AppendLine(" FROM")
                        '.AppendLine("     TB_T_RO_INFO RI, ")
                        '.AppendLine("     TB_T_SERVICEIN SV,")
                        '.AppendLine("     TBL_SERVICE_VISIT_MANAGEMENT MA,  ")
                        '.AppendLine("     TBL_USERS TBU,  ")
                        '.AppendLine("     TB_M_CUSTOMER MC,  ")
                        '.AppendLine("     TB_M_NAMETITLE MN  ")
                        '.AppendLine(" WHERE")
                        '.AppendLine("     RI.SVCIN_ID = SV.SVCIN_ID ")
                        '.AppendLine(" AND SV.CST_ID = MA.CUSTID(+) ")
                        '.AppendLine(" AND SV.VCL_ID = MA.VCL_ID(+) ")
                        '.AppendLine(" AND SV.BRN_CD = MA.STRCD(+) ")
                        '.AppendLine(" AND SV.DLR_CD = MA.DLRCD(+)")
                        '.AppendLine(" AND SV.PIC_SA_STF_CD = TBU.ACCOUNT(+) ")
                        '.AppendLine(" AND MA.CUSTID = MC.CST_ID(+)  ")
                        '.AppendLine(" AND MC.NAMETITLE_CD = MN.NAMETITLE_CD(+)  ")
                        '.AppendLine(" AND SV.DLR_CD=:DLR_CD")
                        '.AppendLine(" AND SV.BRN_CD=:BRN_CD")
                        '.AppendLine(" AND RI.RO_NUM=:RO_NUM")
                        '2014/07/17　サービス来店者管理へのリレーション修正　END　　↑↑↑
                    End With
                Else
                    With sql
                        .AppendLine(" SELECT")
                        .AppendLine("  /* SC3180202_001 */ ")
                        .AppendLine("     DISTINCT(VD.REG_NUM) AS VCLREGNO,")
                        .AppendLine("     MN.POSITION_TYPE,")
                        .AppendLine("     MN.NAMETITLE_NAME,")
                        .AppendLine("     SV.CONTACT_PERSON_NAME AS NAME,")
                        .AppendLine("     SV.RSLT_SVCIN_DATETIME,")
                        .AppendLine("     SV.SVCIN_MILE,")
                        .AppendLine("     SV.RO_NUM,")
                        .AppendLine("     TBU.USERNAME,")
                        .AppendLine("     SV.RSLT_DELI_DATETIME,")
                        .AppendLine("     SV.CONTACT_PHONE AS TELNO")
                        .AppendLine(" FROM")
                        .AppendLine("     TB_H_RO_INFO RI, ")
                        .AppendLine("     TB_H_SERVICEIN SV,")
                        .AppendLine("     TB_M_VEHICLE_DLR VD,  ")
                        .AppendLine("     TBL_USERS TBU,  ")
                        .AppendLine("     TB_M_CUSTOMER MC,  ")
                        .AppendLine("     TB_M_NAMETITLE MN  ")
                        .AppendLine(" WHERE")
                        .AppendLine("     RI.SVCIN_ID = SV.SVCIN_ID ")
                        .AppendLine(" AND SV.VCL_ID = VD.VCL_ID(+) ")
                        .AppendLine(" AND SV.DLR_CD = VD.DLR_CD(+) ")
                        .AppendLine(" AND SV.PIC_SA_STF_CD = TBU.ACCOUNT(+) ")
                        .AppendLine(" AND SV.CST_ID = MC.CST_ID(+) ")
                        .AppendLine(" AND MC.NAMETITLE_CD = MN.NAMETITLE_CD(+) ")
                        .AppendLine(" AND SV.DLR_CD=:DLR_CD")
                        .AppendLine(" AND SV.BRN_CD=:BRN_CD")
                        .AppendLine(" AND RI.RO_NUM=:RO_NUM")
                    End With
                End If
                '2017/01/24　ライフサイクル対応追加　END　↑↑↑

                With query
                    .CommandText = sql.ToString()

                    'バインド変数
                    .AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
                    .AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCd)
                    .AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)
                End With

                headerDataDataTable = query.GetData

                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                           "{0} End [RowCount:{1}]",
                                          System.Reflection.MethodBase.GetCurrentMethod.Name,
                                          headerDataDataTable.Rows.Count))

                '検索結果返却
                Return headerDataDataTable
            End Using

        End Function

        ''' <summary>
        ''' 明細情報取得
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="brnCd">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <param name="isExistActive">Active存在フラグ</param>
        ''' <returns>明細情報</returns>
        ''' <remarks></remarks>
        Public Shared Function GetCheckSheetDetail(ByVal dlrCd As String, _
                                                   ByVal brnCd As String, _
                                                   ByVal roNum As String, _
                                                   ByVal isExistActive As Boolean) As SC3180202DetailDataDataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                               "{0} Start [dlrCd:{1}][brnCd:{2}][roNum:{3}]",
                               System.Reflection.MethodBase.GetCurrentMethod.Name,
                               dlrCd, brnCd, roNum))

            Using query As New DBSelectQuery(Of SC3180202DetailDataDataTable)("SC3180202_002")
                Dim sql As New StringBuilder
                Dim detailDataDataTable As SC3180202DetailDataDataTable


                '2017/01/24　ライフサイクル対応追加　START　↓↓↓
                If isExistActive Then
                    'SQL文作成
                    With sql
                        .AppendLine(" SELECT DISTINCT ")
                        .AppendLine("  /* SC3180202_002 */ ")
                        .AppendLine("     HD.APPROVAL_STATUS,")
                        .AppendLine("     HD.ADVICE_CONTENT,")
                        .AppendLine("     TDL.INSPEC_RSLT_CD,")
                        .AppendLine("     TDL.OPERATION_RSLT_ALREADY_REPLACE,")
                        .AppendLine("     TDL.OPERATION_RSLT_ALREADY_FIX,")
                        .AppendLine("     TDL.OPERATION_RSLT_ALREADY_CLEAN,")
                        .AppendLine("     TDL.OPERATION_RSLT_ALREADY_SWAP,")
                        .AppendLine("     TDL.RSLT_VAL_BEFORE,")
                        .AppendLine("     TDL.RSLT_VAL_AFTER,")
                        .AppendLine("     MDL.INSPEC_ITEM_NAME,")

                        '2014/07/09 サブ点検項目名称の取得元を変更
                        .AppendLine("     MDL.PRINT_INSPEC_ITEM_NAME,")
                        '.AppendLine("     MDL.SUB_INSPEC_ITEM_NAME,")
                        '.AppendLine("     MDL.HTML_INSPEC_ITEM_CD,")
                        .AppendLine("     MDL.INSPEC_ITEM_CD,")
                        .AppendLine("     MDL.DISP_TEXT_UNIT")
                        .AppendLine(" FROM")
                        '2014/07/17　不要テーブルのため参照削除　START　↓↓↓
                        '.AppendLine("     TB_M_INSPECTION_COMB CM,")
                        '2014/07/17　不要テーブルのため参照削除　END　　↑↑↑
                        .AppendLine("     TB_T_FINAL_INSPECTION_HEAD HD,")
                        .AppendLine("     TB_T_FINAL_INSPECTION_DETAIL TDL,  ")
                        .AppendLine("     TB_M_FINAL_INSPECTION_DETAIL MDL  ")
                        .AppendLine(" WHERE")
                        '.AppendLine("        HD.BRN_CD     = TDL.BRN_CD(+) ")
                        '.AppendLine("    AND HD.DLR_CD     = TDL.DLR_CD(+) ")
                        '.AppendLine("    AND HD.JOB_DTL_ID = TDL.JOB_DTL_ID(+)")

                        .AppendLine("        HD.JOB_DTL_ID = TDL.JOB_DTL_ID(+)")
                        .AppendLine("    AND TDL.INSPEC_ITEM_CD = MDL.INSPEC_ITEM_CD(+) ")
                        '2014/07/17　不要テーブルのため参照削除　START　↓↓↓
                        '.AppendLine("    AND CM.BRN_CD = HD.BRN_CD ")
                        '.AppendLine("    AND CM.DLR_CD = HD.DLR_CD ")
                        '2014/07/17　不要テーブルのため参照削除　END　　↑↑↑
                        .AppendLine("    AND HD.DLR_CD=:DLR_CD")
                        .AppendLine("    AND HD.BRN_CD=:BRN_CD")
                        .AppendLine("    AND HD.RO_NUM=:RO_NUM")
                    End With
                Else
                    With sql
                        .AppendLine(" SELECT DISTINCT ")
                        .AppendLine("  /* SC3180202_002 */ ")
                        .AppendLine("     HD.APPROVAL_STATUS,")
                        .AppendLine("     HD.ADVICE_CONTENT,")
                        .AppendLine("     TDL.INSPEC_RSLT_CD,")
                        .AppendLine("     TDL.OPERATION_RSLT_ALREADY_REPLACE,")
                        .AppendLine("     TDL.OPERATION_RSLT_ALREADY_FIX,")
                        .AppendLine("     TDL.OPERATION_RSLT_ALREADY_CLEAN,")
                        .AppendLine("     TDL.OPERATION_RSLT_ALREADY_SWAP,")
                        .AppendLine("     TDL.RSLT_VAL_BEFORE,")
                        .AppendLine("     TDL.RSLT_VAL_AFTER,")
                        .AppendLine("     MDL.INSPEC_ITEM_NAME,")
                        .AppendLine("     MDL.PRINT_INSPEC_ITEM_NAME,")
                        .AppendLine("     MDL.INSPEC_ITEM_CD,")
                        .AppendLine("     MDL.DISP_TEXT_UNIT")
                        .AppendLine(" FROM")
                        .AppendLine("     TB_H_FINAL_INSPECTION_HEAD HD,")
                        .AppendLine("     TB_H_FINAL_INSPECTION_DETAIL TDL,  ")
                        .AppendLine("     TB_M_FINAL_INSPECTION_DETAIL MDL  ")
                        .AppendLine(" WHERE")
                        .AppendLine("        HD.JOB_DTL_ID = TDL.JOB_DTL_ID(+)")
                        .AppendLine("    AND TDL.INSPEC_ITEM_CD = MDL.INSPEC_ITEM_CD(+) ")
                        .AppendLine("    AND HD.DLR_CD=:DLR_CD")
                        .AppendLine("    AND HD.BRN_CD=:BRN_CD")
                        .AppendLine("    AND HD.RO_NUM=:RO_NUM")
                    End With
                End If
                '2017/01/24　ライフサイクル対応追加　END　↑↑↑


                With query
                    .CommandText = sql.ToString()

                    'バインド変数
                    .AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
                    .AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCd)
                    .AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)
                End With

                detailDataDataTable = query.GetData

                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                                           "{0} End [RowCount:{1}]",
                                          System.Reflection.MethodBase.GetCurrentMethod.Name,
                                          detailDataDataTable.Rows.Count))

                '検索結果返却
                Return detailDataDataTable

            End Using

        End Function

        '2019/07/05　TKM要件:型式対応　START　↓↓↓
        ''' <summary>
        ''' モデルコード取得
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="brnCd">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <param name="isExistActive">Active存在フラグ</param>
        ''' <returns>ModelCode</returns>
        ''' <remarks></remarks>
        Public Shared Function GetModelCode(ByVal dlrCd As String, _
                                            ByVal brnCd As String, _
                                            ByVal roNum As String, _
                                            ByVal isExistActive As Boolean) As String
            Dim modelCode As String = String.Empty
            Dim dt As DataTable = GetModelDataTable(dlrCd, brnCd, roNum, isExistActive)
            If dt.Rows.Count <> 0 Then
                modelCode = dt.Rows(0).Item("MODEL_CD").ToString
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                            "{0} End  [RowCount:{1}][MODEL_CD:{2}]",
                        System.Reflection.MethodBase.GetCurrentMethod.Name,
                        dt.Rows.Count, modelCode))

            Return modelCode
        End Function

        ''' <summary>
        ''' 型式取得
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="brnCd">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <param name="isExistActive">Active存在フラグ</param>
        ''' <returns>型式</returns>
        ''' <remarks></remarks>
        Public Shared Function GetKatashiki(ByVal dlrCd As String, _
                                            ByVal brnCd As String, _
                                            ByVal roNum As String, _
                                            ByVal isExistActive As Boolean) As String
            Dim katashiki As String = String.Empty
            Dim dt As DataTable = GetModelDataTable(dlrCd, brnCd, roNum, isExistActive)
            If dt.Rows.Count <> 0 Then
                katashiki = dt.Rows(0).Item("VCL_KATASHIKI").ToString
            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                            "{0} End  [RowCount:{1}][VCL_KATASHIKI:{2}]",
                        System.Reflection.MethodBase.GetCurrentMethod.Name,
                        dt.Rows.Count, katashiki))

            Return katashiki

        End Function

        ''' <summary>
        ''' モデルコード取得
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="brnCd">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <param name="isExistActive">Active存在フラグ</param>
        ''' <returns>ModelCode</returns>
        ''' <remarks></remarks>
        Public Shared Function GetModelDataTable(ByVal dlrCd As String, _
                                            ByVal brnCd As String, _
                                            ByVal roNum As String, _
                                            ByVal isExistActive As Boolean) As DataTable

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                             "{0} Start [dlrCd:{1}][brnCd:{2}][roNum:{3}]",
                             System.Reflection.MethodBase.GetCurrentMethod.Name,
                             dlrCd, brnCd, roNum))

            Dim dt As New DataTable
            
            Using query As New DBSelectQuery(Of SC3180202DetailDataDataTable)("SC3180202_003")
                Dim sql As New StringBuilder

                '2017/01/24　ライフサイクル対応追加　START　↓↓↓
                If isExistActive Then
                    'SQL文作成
                    With sql
                        .AppendLine(" SELECT")
                        .AppendLine("  /* SC3180202_003 */ ")
                        .AppendLine("    TB_M_VEHICLE.MODEL_CD,")
                        .AppendLine("    TB_M_VEHICLE.VCL_KATASHIKI")
                        .AppendLine(" FROM")
                        .AppendLine("    TB_T_RO_INFO RI, ")
                        .AppendLine("    TB_T_SERVICEIN,")
                        .AppendLine("    TB_M_VEHICLE")
                        .AppendLine(" WHERE")
                        .AppendLine("    RI.SVCIN_ID = TB_T_SERVICEIN.SVCIN_ID AND")
                        .AppendLine("    TB_T_SERVICEIN.VCL_ID = TB_M_VEHICLE.VCL_ID AND")
                        .AppendLine("    TB_T_SERVICEIN.DLR_CD =:DLR_CD  AND")
                        .AppendLine("    TB_T_SERVICEIN.BRN_CD =:BRN_CD  AND")
                        .AppendLine("    RI.RO_NUM =:RO_NUM ")
                    End With
                Else
                    With sql
                        .AppendLine(" SELECT")
                        .AppendLine("  /* SC3180202_003 */ ")
                        .AppendLine("    TB_M_VEHICLE.MODEL_CD,")
                        .AppendLine("    TB_M_VEHICLE.VCL_KATASHIKI")
                        .AppendLine(" FROM")
                        .AppendLine("    TB_H_RO_INFO RI, ")
                        .AppendLine("    TB_H_SERVICEIN,")
                        .AppendLine("    TB_M_VEHICLE")
                        .AppendLine(" WHERE")
                        .AppendLine("    RI.SVCIN_ID = TB_H_SERVICEIN.SVCIN_ID AND")
                        .AppendLine("    TB_H_SERVICEIN.VCL_ID = TB_M_VEHICLE.VCL_ID AND")
                        .AppendLine("    TB_H_SERVICEIN.DLR_CD =:DLR_CD  AND")
                        .AppendLine("    TB_H_SERVICEIN.BRN_CD =:BRN_CD  AND")
                        .AppendLine("    RI.RO_NUM =:RO_NUM ")
                    End With
                End If
                '2017/01/24　ライフサイクル対応追加　END　↑↑↑

                With query
                    .CommandText = sql.ToString()

                    'バインド変数
                    .AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
                    .AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCd)
                    .AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)

                    dt = .GetData
                End With

                '検索結果返却
                Return dt

            End Using

        End Function
        '2019/07/05　TKM要件:型式対応　END　↑↑↑

        '2014/07/09 タイトルをデザイン固定にするため削除
        ' ''' <summary>
        ' ''' タイトル名取得
        ' ''' </summary>
        ' ''' <param name="dlrCd">販売店コード</param>
        ' ''' <param name="brnCd">店舗コード</param>
        ' ''' <returns>タイトル名</returns>
        ' ''' <remarks></remarks>
        'Public Shared Function GetTitleName(ByVal dlrCd As String, _
        '                                    ByVal brnCd As String) As DataTable

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture,
        '                       "{0} Start [dlrCd:{1}][brnCd:{2}]",
        '                       System.Reflection.MethodBase.GetCurrentMethod.Name,
        '                       dlrCd, brnCd))

        '    Using query As New DBSelectQuery(Of DataTable)("SC3180202_004", DBQueryTarget.DMS)
        '        Dim sql As New StringBuilder
        '        Dim titleDataTable As New DataTable

        '        'SQL文作成
        '        With sql
        '            .AppendLine("SELECT")
        '            .AppendLine("  /* SC3180202_004 */ ")
        '            '.AppendLine("   MIN(DTL.HTML_INSPEC_ITEM_CD) HTML_INSPEC_ITEM_CD,")
        '            .AppendLine("    MIN(DTL.INSPEC_ITEM_CD) INSPEC_ITEM_CD,")
        '            .AppendLine("    DTL.INSPEC_ITEM_NAME")
        '            .AppendLine(" FROM")
        '            .AppendLine("    TB_M_INSPECTION_COMB    COMB,")
        '            .AppendLine("    TB_M_FINAL_INSPECTION_DETAIL DTL")
        '            .AppendLine(" WHERE")
        '            .AppendLine("    COMB.INSPEC_ITEM_CD = DTL.INSPEC_ITEM_CD AND")
        '            .AppendLine("    COMB.DLR_CD = :DLR_CD AND")
        '            .AppendLine("    COMB.BRN_CD = :BRN_CD")
        '            .AppendLine(" GROUP BY DTL.INSPEC_ITEM_NAME ")
        '        End With

        '        With query
        '            .CommandText = sql.ToString()

        '            'バインド変数
        '            .AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
        '            .AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCd)

        '            titleDataTable = .GetData
        '        End With

        '        Logger.Info(String.Format(CultureInfo.InvariantCulture,
        '                                   "{0} End [RowCount:{1}]",
        '                                  System.Reflection.MethodBase.GetCurrentMethod.Name,
        '                                  titleDataTable.Rows.Count))

        '        '検索結果返却
        '        Return titleDataTable

        '    End Using

        'End Function

        '2014/06/27 不具合修正　Start
        ''' <summary>
        ''' アイテムコード並び順取得
        ''' </summary>
        ''' <param name="itemCode">アイテムコード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function GetItemCodeOrder(ByVal itemCode As String) As DataTable

            Using query As New DBSelectQuery(Of DataTable)("SC3180202_005")
                Dim sql As New StringBuilder
                Dim titleDataTable As New DataTable

                'SQL文作成
                With sql
                    .AppendLine(" SELECT ")
                    .AppendLine("  /* SC3180202_005 */ ")
                    .AppendLine("     DISP_NAME, ")
                    .AppendLine("     ORDER_NO ")
                    .AppendLine(" FROM ")
                    .AppendLine("     (SELECT ")
                    .AppendLine("         'REPLACE' AS DISP_NAME, ")
                    .AppendLine("         TO_NUMBER(DISP_OPE_ITEM_ALREADY_REPLACE) AS ORDER_NO ")
                    .AppendLine("     FROM ")
                    .AppendLine("         TB_M_FINAL_INSPECTION_DETAIL ")
                    .AppendLine("     WHERE ")
                    .AppendLine("         INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
                    .AppendLine("     UNION ALL    ")
                    .AppendLine("     SELECT ")
                    .AppendLine("         'FIX' AS DISP_NAME, ")
                    .AppendLine("         TO_NUMBER(DISP_OPE_ITEM_ALREADY_FIX) AS ORDER_NO ")
                    .AppendLine("     FROM ")
                    .AppendLine("         TB_M_FINAL_INSPECTION_DETAIL ")
                    .AppendLine("     WHERE ")
                    .AppendLine("         INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
                    .AppendLine("     UNION ALL    ")
                    .AppendLine("     SELECT ")
                    .AppendLine("         'CLEAN' AS DISP_NAME, ")
                    .AppendLine("         TO_NUMBER(DISP_OPE_ITEM_ALREADY_CLEAN) AS ORDER_NO ")
                    .AppendLine("     FROM ")
                    .AppendLine("         TB_M_FINAL_INSPECTION_DETAIL ")
                    .AppendLine("     WHERE ")
                    .AppendLine("         INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
                    .AppendLine("     UNION ALL    ")
                    .AppendLine("     SELECT ")
                    .AppendLine("         'SWAP' AS DISP_NAME, ")
                    .AppendLine("         TO_NUMBER(DISP_OPE_ITEM_ALREADY_SWAP) AS ORDER_NO ")
                    .AppendLine("     FROM ")
                    .AppendLine("         TB_M_FINAL_INSPECTION_DETAIL ")
                    .AppendLine("     WHERE ")
                    .AppendLine("         INSPEC_ITEM_CD = :INSPEC_ITEM_CD ")
                    .AppendLine("     )TB_M_FINAL_INSPECTION_DETAIL ")
                    .AppendLine(" ORDER BY ")
                    .AppendLine("     ORDER_NO ")
                End With

                With query
                    .CommandText = sql.ToString()
                    .AddParameterWithTypeValue("INSPEC_ITEM_CD", OracleDbType.NVarchar2, itemCode)
                    titleDataTable = .GetData
                End With

                '検索結果返却
                Return titleDataTable

            End Using

        End Function
        '2014/06/27 不具合修正　End

        '2014/07/28　DMS→ICROP変換処理追加　START　↓↓↓
        ''' <summary>
        ''' 006:販売店/店舗コード変換(DMS->iCROP)
        ''' </summary>
        ''' <param name="strDlrCd">基幹販売店コード</param>
        ''' <param name="strBrnCd">基幹店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ChangeDlrStrCodeToICROP(ByVal dealerCD As String _
                                                       , ByVal dmsCodeType As Integer _
                                                       , ByVal strDlrCd As String _
                                                       , ByVal strBrnCd As String) As SC3180202DataSet.IcropCodeMapDataTable

            Dim No As String = "SC3180202_006"
            Dim strMethodName As String = "ChangeDlrStrCodeToICROP"

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Start", strMethodName))
            'ログ出力 End *****************************************************************************

            Try
                Dim dt As New SC3180202DataSet.IcropCodeMapDataTable
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3180202_006 */ ")
                    .Append("        ICROP_CD_1 ")
                    .Append("      , ICROP_CD_2 ")
                    .Append(" FROM   TB_M_DMS_CODE_MAP ")
                    '2015/04/14 新販売店追加対応 start
                    '.Append(" WHERE  DLR_CD      = :DLR_CD ")
                    .Append(" WHERE  DLR_CD IN (:DLR_CD, :ALL_DLR_CD) ")
                    .Append(" AND    DMS_CD_1    = :DMS_CD_1 ")
                    .Append(" AND    DMS_CD_2    = :DMS_CD_2 ")
                    .Append(" AND    DMS_CD_TYPE = :DMS_CD_TYPE ")
                    .AppendLine(" ORDER BY DLR_CD ASC ")
                End With

                Using query As New DBSelectQuery(Of SC3180202DataSet.IcropCodeMapDataTable)(No)

                    query.CommandText = sql.ToString()

                    'query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCD)
                    query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDlrCd)
                    query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.NVarchar2, dealerCD)
                    query.AddParameterWithTypeValue("DMS_CD_1", OracleDbType.NVarchar2, strDlrCd)
                    query.AddParameterWithTypeValue("DMS_CD_2", OracleDbType.NVarchar2, strBrnCd)
                    query.AddParameterWithTypeValue("DMS_CD_TYPE", OracleDbType.NVarchar2, dmsCodeType)
                    '2015/04/14 新販売店追加対応 end

                    dt = query.GetData()

                    'ログ出力 Start ***************************************************************************
                    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_End", strMethodName))
                    'ログ出力 End *****************************************************************************

                    Return dt
                End Using

            Catch ex As Exception
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Debug(String.Format("{0}_Exception:", strMethodName) & ex.ToString)
                'ログ出力 End *****************************************************************************
                Return Nothing
            End Try
        End Function
        '2014/07/28　DMS→ICROP変換処理追加　END　　↑↑↑


        ''' <summary>
        ''' サービス入庫Active存在チェック
        ''' </summary>
        ''' <param name="dlrCd">販売店コード</param>
        ''' <param name="brnCd">店舗コード</param>
        ''' <param name="roNum">RO番号</param>
        ''' <returns>登録状態 True:Active False:Active以外</returns>
        ''' <remarks></remarks>
        Public Function IsExistServiceinActive(ByVal dlrCd As String, _
                                               ByVal brnCd As String, _
                                               ByVal roNum As String) As Boolean

            Logger.Info(String.Format(CultureInfo.InvariantCulture,
                             "{0} Start [dlrCd:{1}][brnCd:{2}][roNum:{3}]",
                             System.Reflection.MethodBase.GetCurrentMethod.Name,
                             dlrCd, brnCd, roNum))

            Using query As New DBSelectQuery(Of DataTable)("SC3180202_007")

                Dim sql As New StringBuilder
                Dim isExistActive As Boolean

                'SQL文作成
                With sql
                    .AppendLine(" SELECT")
                    .AppendLine("  /* SC3180202_007 */ ")
                    .AppendLine("    COUNT(1) AS COUNT")
                    .AppendLine(" FROM")
                    .AppendLine("    TB_T_SERVICEIN ")
                    .AppendLine(" WHERE")
                    .AppendLine("    DLR_CD =:DLR_CD AND")
                    .AppendLine("    BRN_CD =:BRN_CD AND")
                    .AppendLine("    RO_NUM =:RO_NUM ")
                End With

                With query
                    .CommandText = sql.ToString()

                    'バインド変数
                    .AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
                    .AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, brnCd)
                    .AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)
                End With

                '検索結果返却
                Dim dt As DataTable = query.GetData()
                Dim intCount As Integer

                If dt.Rows.Count <> 0 Then
                    intCount = CType(dt.Rows(0).Item("COUNT").ToString(), Integer)

                    If intCount = 0 Then
                        isExistActive = False
                    Else
                        isExistActive = True
                    End If

                End If

                Logger.Info(String.Format(CultureInfo.InvariantCulture,
                             "{0} End  [RowCount:{1}][IsExistActive:{2}]",
                            System.Reflection.MethodBase.GetCurrentMethod.Name,
                            dt.Rows.Count, isExistActive))

                Return isExistActive

            End Using

        End Function


        ''' <summary>
        ''' マスタにKATASHIKI登録されているか判定する
        ''' </summary>
        ''' <param name="strRoNum">R/O番号</param>
        ''' <param name="strDlrCd">販売店コード</param>
        ''' <param name="strBrnCd">店舗コード</param>
        ''' <returns>登録状態 DataTable TRANSACTION_EXIST : 1 or 0 , HISTORY_EXIST : 1 or 0 </returns>
        ''' <remarks>点検組み合わせマスタ、整備属性マスタに指定の販売店データが登録されているかをフラグで取得する</remarks>
        Public Function GetKatashikiExistMst(ByVal strRoNum As String, _
                                         ByVal strDlrCd As String, _
                                         ByVal strBrnCd As String) As DataTable

            Dim dt As DataTable

            '開始ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Using query As New DBSelectQuery(Of DataTable)("SC3180202_008")
                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT")
                    .Append("    /* SC3180202_008 */")
                    .Append("    CASE")
                    .Append("        WHEN IC.VCL_KATASHIKI <> MV.VCL_KATASHIKI")
                    .Append("    OR  NVL(IC.VCL_KATASHIKI, ' ') = ' '")
                    .Append("    OR  NVL(MV.VCL_KATASHIKI, ' ') = ' ' THEN '0'")
                    .Append("        ELSE '1'")
                    .Append("    END KATASHIKI_EXIST")
                    .Append(" FROM")
                    .Append("    TB_M_VEHICLE MV")
                    .Append("    LEFT OUTER JOIN")
                    .Append("        (")
                    .Append("            SELECT")
                    .Append("                IC.MODEL_CD,")
                    .Append("                IC.DLR_CD,")
                    .Append("                IC.BRN_CD,")
                    .Append("                IC.VCL_KATASHIKI")
                    .Append("            FROM")
                    .Append("                TB_M_INSPECTION_COMB IC")
                    .Append("            WHERE")
                    .Append("                IC.DLR_CD IN(:DLR_CD, 'XXXXX')")
                    .Append("            AND IC.BRN_CD IN(:BRN_CD, 'XXX')")
                    .Append("            ORDER BY")
                    .Append("                IC.VCL_KATASHIKI DESC")
                    .Append("        ) IC")
                    .Append("    ON  IC.MODEL_CD = MV.MODEL_CD")
                    .Append("    AND IC.VCL_KATASHIKI = MV.VCL_KATASHIKI")
                    .Append("    LEFT OUTER JOIN")
                    .Append("        (")
                    .Append("            SELECT")
                    .Append("                TSI.VCL_ID,")
                    .Append("                TSI.RO_NUM")
                    .Append("            FROM")
                    .Append("                TB_T_SERVICEIN TSI")
                    .Append("            WHERE")
                    .Append("                TSI.RO_NUM = :RO_NUM")
                    .Append("            AND TSI.DLR_CD = :DLR_CD")
                    .Append("            AND TSI.BRN_CD = :BRN_CD")
                    .Append("    UNION ")
                    .Append("            SELECT")
                    .Append("                HSI.VCL_ID,")
                    .Append("                HSI.RO_NUM")
                    .Append("            FROM")
                    .Append("                TB_H_SERVICEIN HSI")
                    .Append("            WHERE")
                    .Append("                HSI.RO_NUM = :RO_NUM")
                    .Append("            AND HSI.DLR_CD = :DLR_CD")
                    .Append("            AND HSI.BRN_CD = :BRN_CD")
                    .Append("            AND ROWNUM = 1")
                    .Append("        ) SI")
                    .Append("    ON  SI.RO_NUM = :RO_NUM")
                    .Append(" WHERE")
                    .Append("    ROWNUM = 1")
                    .Append("    AND MV.VCL_ID = SI.VCL_ID")
                End With
                'クエリ設定
                query.CommandText = sql.ToString()
                'パラメータ設定
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, strRoNum)     'R/O番号
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, strDlrCd)     '販売店
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strBrnCd)     '店舗
                '結果取得
                dt = query.GetData()
            End Using

            '終了ログ
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return dt
        End Function
    End Class

End Namespace
