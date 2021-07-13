'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080101DataTableTableAdapter.vb 
'─────────────────────────────────────
'機能： 顧客検索一覧 (データ)
'補足： 
'作成： 2011/11/18 TCS 安田
'更新： 2012/02/29 TCS 河原 【SALES_1B】
'更新： 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加
'更新： 2012/11/15 TCS 藤井 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2013/06/30 TCS 吉村 【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/03 TCS 森    Aカード情報相互連携開発
'更新： 2014/08/01 TCS 市川  TMT切替BTS-115対応
'更新： 2015/06/08 TCS 中村 TMT課題対応(#2)
'更新： 2018/07/10 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3
'更新： 2019/05/23 TS  重松 (FS)納車時オペレーションCS向上にむけた評価（UAT-0192）
'削除： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
'2012/02/29 TCS 河原【SALES_1B】START
Imports Toyota.eCRB.SystemFrameworks.Web
'2012/02/29 TCS 河原【SALES_1B】END

Public Class SC3080101TableAdapter

    ''' <summary>
    ''' 検索条件の種類
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IdSerchVclregno As Integer = 1    '車両登録No
    Public Const IdSerchName As Integer = 2        '顧客名称
    Public Const IdSerchVin As Integer = 3         'VIN
    Public Const IdSerchTel As Integer = 4         '電話番号/携帯番号
    ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
    Public Const IdSerchSolId As Integer = 6
    ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END

    ''' <summary>
    ''' ソート条件の種類
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IdSortName As Integer = 1          '顧客名称
    Public Const IdSortModel As Integer = 2         'モデル名称
    Public Const IdSortSsusername As Integer = 3    'セールススタッフ名称
    Public Const IdSortSausername As Integer = 4    'サービスアドバイザー名称

    ''' <summary>
    ''' ソート方向(昇順)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IdOrderAsc As Integer = 1

    ''' <summary>
    ''' ソート方向(降順)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IdOrderDesc As Integer = 2

    ''' <summary>
    ''' 検索方向 (1:前方一致)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IdSerchdirectionAfter As Integer = 1

    ''' <summary>
    ''' 検索方向 (2:あいまい検索)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IdSerchdirectionAll As Integer = 2

    ' 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 START
    ''' <summary>
    ''' 検索方向 (3:完全一致)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IdSerchdirectionSame As Integer = 3
    ' 2012/08/13 TCS 安田 【SALES_3】顧客編集より顧客検索機能追加 END

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL

    '2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 顧客件数取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="serchDirection">検索方向</param>
    ''' <param name="serchType">検索タイプ</param>
    ''' <param name="serchValue">検索テキスト</param>
    ''' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
    ''' <param name="searchOrg">組織ID(自チームのみ)</param>
    ''' 2013/12/03 TCS 森    Aカード情報相互連携開発 END
    ''' <returns>顧客件数</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCountCustomer(ByVal dlrcd As String, _
                                     ByVal serchDirection As Integer, _
                                     ByVal serchType As Integer, _
                                     ByVal serchValue As String, _
                                     ByVal searchOrg As String) As Integer

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCountCustomer_Start")
        'ログ出力 End *****************************************************************************

        If (serchType < 0) Then
            Return Nothing
        End If
        '2012/02/29 TCS 河原【SALES_1B】START
        Dim opeCd As Integer = StaffContext.Current.OpeCD
        Dim account As String = StaffContext.Current.Account
        '2012/02/29 TCS 河原【SALES_1B】END
        ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 START
        Dim leaderFlg As Boolean = StaffContext.Current.TeamLeader AndAlso searchOrg.Length > 0
        ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 END

        Dim sql As New StringBuilder
        With sql
            ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 START
            .AppendLine("SELECT /* SC3080101_201 */ ")
            .AppendLine("        COUNT(1) AS CNT ")
            .AppendLine("    FROM ")
            .AppendLine("		TB_M_CUSTOMER T1 ")
            .AppendLine("		INNER JOIN TB_M_CUSTOMER_DLR T2 ")
            .AppendLine("		ON T1.CST_ID = T2.CST_ID ")
            .AppendLine("       INNER JOIN TB_M_CUSTOMER_VCL T3 ")
            .AppendLine("       ON T2.DLR_CD = T3.DLR_CD ")
            .AppendLine("       AND T2.CST_ID = T3.CST_ID ")
            .AppendLine("       LEFT JOIN TB_M_VEHICLE_DLR T4 ")
            .AppendLine("       ON T3.DLR_CD = T4.DLR_CD ")
            .AppendLine("       AND T3.VCL_ID = T4.VCL_ID ")
            .AppendLine("       LEFT JOIN TB_M_VEHICLE T5 ")
            .AppendLine("       ON T3.VCL_ID = T5.VCL_ID ")
            .AppendLine("       LEFT JOIN TB_M_STAFF T6 ")
            .AppendLine("       ON T3.SLS_PIC_STF_CD = T6.STF_CD ")
            .AppendLine("       AND T6.INUSE_FLG = '1' ")
            .AppendLine("    WHERE ")
            ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 END

            '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            If SystemFrameworks.Core.iCROP.BizLogic.Operation.SSM = opeCd Then
                'マネージャ検索は前方一致のみ。
                '電話番号検索はGetTelSearchCountCustomerのみ使用する為処理不要
                Select Case serchType
                    Case IdSerchName      '顧客名称
                        .AppendLine("        T1.CST_NAME_SEARCH like :SEARCHPARAM || '%' AND ")
                    Case IdSerchVin       'VIN
                        .AppendLine("        T5.VCL_VIN_SEARCH like :SEARCHPARAM || '%' AND ")
                    Case IdSerchVclregno  '車両登録No
                        .AppendLine("        T4.REG_NUM_SEARCH like :SEARCHPARAM || '%' AND ")
                End Select
            Else
                '2014/08/01 TCS 市川 TMT切替BTS-115対応 END
                If (serchDirection = IdSerchdirectionAfter) Then
                    '前方一致
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        T1.CST_NAME_SEARCH like :NAME || '%' AND ")
                        Case IdSerchVin       'VIN
                            .Append("        T5.VCL_VIN_SEARCH like :VIN || '%' AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        T4.REG_NUM_SEARCH like :VCLREGNO || '%' AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            .Append("       (T1.CST_PHONE_SEARCH like :TELNO || '%' ")
                            .Append("    OR  T1.CST_MOBILE_SEARCH like :TELNO || '%') AND ")
                        Case Else
                    End Select
                ElseIf (serchDirection = IdSerchdirectionSame) Then

                    '完全一致
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        T1.CST_NAME_SEARCH = :NAME AND ")
                        Case IdSerchVin       'VIN
                            .Append("        T5.VCL_VIN_SEARCH = :VIN AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        T4.REG_NUM_SEARCH = :VCLREGNO AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            .Append("       (T1.CST_PHONE_SEARCH = :TELNO ")
                            .Append("    OR  T1.CST_MOBILE_SEARCH = :TELNO ) AND ")
                        Case Else
                    End Select
                Else
                    'あいまい検索
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        T1.CST_NAME_SEARCH like '%' || :NAME || '%' AND ")
                        Case IdSerchVin       'VIN
                            .Append("        T5.VCL_VIN_SEARCH like '%' || :VIN || '%' AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        T4.REG_NUM_SEARCH like '%' || :VCLREGNO || '%' AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            .Append("    (T1.CST_PHONE_SEARCH like '%' || :TELNO || '%' ")
                            .Append(" OR  T1.CST_MOBILE_SEARCH like '%' || :TELNO || '%') AND ")
                        Case Else
                    End Select
                End If

                '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            End If
            '2014/08/01 TCS 市川 TMT切替BTS-115対応 END

            ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 START
            .AppendLine("        T2.DLR_CD = :DLRCD AND ")
            .AppendLine("        ((T2.CST_TYPE ='1' AND ")
            .AppendLine("        Trim(T5.VCL_VIN) IS NOT NULL ) OR ")
            .AppendLine("        (T2.CST_TYPE ='2')) AND ")
            .AppendLine("        T3.OWNER_CHG_FLG = '0' AND")
            .AppendLine("        T3.CST_VCL_TYPE = '1' ")
            If leaderFlg Then
                .AppendLine("        AND T6.ORGNZ_ID IN (" & searchOrg & ") ")
                '2012/02/29 TCS 河原【SALES_1B】START
            ElseIf opeCd = SystemFrameworks.Core.iCROP.BizLogic.Operation.SSF Then
                .AppendLine("        AND T3.SLS_PIC_STF_CD = :STAFFCD ")
            End If
            '2012/02/29 TCS 河原【SALES_1B】END
            ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 END


        End With

        Using query As New DBSelectQuery(Of SC3080101DataSet.SC3080101CntDataTable)("SC3080101_201")

            '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            If SystemFrameworks.Core.iCROP.BizLogic.Operation.SSM = opeCd Then
                'MG検索はBind変数を使用せずリテラルにする。(性能対策)
                query.CommandText = sql.ToString() _
                     .Replace(":DLRCD", "'" & dlrcd & "'") _
                     .Replace(":SEARCHPARAM", "'" & serchValue.Replace("'", "''").Replace("\", "") & "'")
            Else
                '2014/08/01 TCS 市川 TMT切替BTS-115対応 END

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)          '販売店コード

                '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                Select Case serchType
                    Case IdSerchName      '顧客名称
                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, serchValue)
                    Case IdSerchVin       'VIN
                        query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, serchValue)
                    Case IdSerchVclregno  '車両登録No
                        query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, serchValue)
                    Case IdSerchTel       '電話番号/携帯番号
                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, serchValue)
                End Select

                '2012/02/29 TCS 河原【SALES_1B】START
                If leaderFlg Then

                ElseIf opeCd = SystemFrameworks.Core.iCROP.BizLogic.Operation.SSF Then
                    query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, account)
                End If
                '2012/02/29 TCS 河原【SALES_1B】END

                '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            End If
            '2014/08/01 TCS 市川 TMT切替BTS-115対応 END

            Dim cntTbl As SC3080101DataSet.SC3080101CntDataTable

            cntTbl = query.GetData()
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCountCustomer_End")
            'ログ出力 End *****************************************************************************

            Return CType(cntTbl.Item(0).Cnt, Integer)

        End Using
        '2013/06/30 TCS 吉村 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 顧客一覧取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="serchDirection">検索方向</param>
    ''' <param name="serchType">検索タイプ</param>
    ''' <param name="serchValue">検索テキスト</param>
    ''' <param name="sortType">ソート条件</param>
    ''' <param name="sortOrder">ソート方向</param>
    ''' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
    ''' <param name="searchOrg">組織ID(自チームのみ)</param>
    ''' 2013/12/03 TCS 森    Aカード情報相互連携開発 END
    ''' <returns>顧客一覧</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustomerList(ByVal dlrcd As String, _
                                     ByVal serchDirection As Integer, _
                                     ByVal serchType As Integer, _
                                     ByVal serchValue As String, _
                                     ByVal sortType As Integer, _
                                     ByVal sortOrder As Integer, _
                                     ByVal searchOrg As String) As SC3080101DataSet.SC3080101CustDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerList_Start")
        'ログ出力 End *****************************************************************************


        If (serchType < 0) Then
            Return Nothing
        End If
        If (sortType < 0) Then
            Return Nothing
        End If

        '2012/02/29 TCS 河原【SALES_1B】START
        Dim opeCd As Integer = StaffContext.Current.OpeCD
        Dim account As String = StaffContext.Current.Account
        '2012/02/29 TCS 河原【SALES_1B】END

        ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
        Dim leaderFlg As Boolean = StaffContext.Current.TeamLeader AndAlso searchOrg.Length > 0
        ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END

        Dim sql As New StringBuilder
        With sql
            ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 START
            .AppendLine("SELECT /* SC3080101_202 */ ")
            .AppendLine("            T2.CST_TYPE AS CSTKIND, ")
            .AppendLine("            CASE WHEN T1.FLEET_FLG = '0' THEN '1' ")
            .AppendLine("                 WHEN T1.FLEET_FLG = '1' THEN '0' ")
            .AppendLine("            END AS CUSTYPE, ")
            .AppendLine("            T1.CST_ID AS CRCUSTID , ")
            .AppendLine("            T2.IMG_FILE_LARGE AS IMAGEFILE_L , ")
            .AppendLine("            T2.IMG_FILE_MEDIUM AS IMAGEFILE_M , ")
            .AppendLine("            T2.IMG_FILE_SMALL AS IMAGEFILE_S , ")
            .AppendLine("            T1.NAMETITLE_NAME AS NAMETITLE , ")
            .AppendLine("            T1.CST_NAME AS NAME , ")
            .AppendLine("            T1.CST_PHONE AS TELNO , ")
            .AppendLine("            T1.CST_MOBILE AS MOBILE , ")
            '2019/05/23 TS  重松 (FS)納車時オペレーションCS向上にむけた評価（UAT-0192） START
            .AppendLine("            CASE WHEN T2.CST_TYPE = '1' THEN T6.MODEL_NAME ")
            .AppendLine("                 WHEN T2.CST_TYPE = '2' THEN T5.NEWCST_MODEL_NAME ")
            .AppendLine("            END AS SERIESNM,  ")
            '2019/05/23 TS  重松 (FS)納車時オペレーションCS向上にむけた評価（UAT-0192） END
            .AppendLine("            T4.REG_NUM AS VCLREGNO , ")
            .AppendLine("            T5.VCL_VIN AS VIN , ")
            .AppendLine("            T3.VCL_ID AS SEQNO , ")
            .AppendLine("            T7.USERNAME AS SSUSERNAME , ")
            .AppendLine("            T8.USERNAME AS SAUSERNAME , ")
            .AppendLine("            T3.SLS_PIC_STF_CD AS STAFFCD , ")
            .AppendLine("            T1.CST_SOCIALNUM ")
            '2018/07/10 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 START
            .AppendLine("           ,T4.IMP_VCL_FLG ")
            '2018/07/10 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 END
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
            .AppendLine("           ,NVL(T11.CST_JOIN_TYPE, ' ') AS CSTJOINTYPE ")
            .AppendLine("        FROM ")
            .AppendLine("		TB_M_CUSTOMER T1 ")
            .AppendLine("		INNER JOIN TB_M_CUSTOMER_DLR T2 ")
            .AppendLine("		ON T1.CST_ID = T2.CST_ID ")
            .AppendLine("       INNER JOIN TB_M_CUSTOMER_VCL T3 ")
            .AppendLine("       ON T2.DLR_CD = T3.DLR_CD ")
            .AppendLine("       AND T2.CST_ID = T3.CST_ID ")
            .AppendLine("       LEFT JOIN TB_M_VEHICLE_DLR T4 ")
            .AppendLine("       ON T3.DLR_CD = T4.DLR_CD ")
            .AppendLine("       AND T3.VCL_ID = T4.VCL_ID ")
            .AppendLine("       LEFT JOIN TB_M_VEHICLE T5 ")
            .AppendLine("       ON T3.VCL_ID = T5.VCL_ID ")
            .AppendLine("       LEFT JOIN TB_M_MODEL T6 ")
            .AppendLine("       ON T5.MODEL_CD = T6.MODEL_CD ")
            .AppendLine("       LEFT JOIN TBL_USERS T7 ")
            .AppendLine("       ON T3.SLS_PIC_STF_CD = RTRIM(T7.ACCOUNT) ")
            .AppendLine("       AND T7.DELFLG = '0' ")
            .AppendLine("       LEFT JOIN TBL_USERS T8 ")
            .AppendLine("       ON T3.SVC_PIC_STF_CD = RTRIM(T8.ACCOUNT) ")
            .AppendLine("       AND T8.DELFLG = '0' ")
            .AppendLine("       LEFT JOIN TB_M_STAFF T9 ")
            .AppendLine("       ON T3.SLS_PIC_STF_CD = T9.STF_CD ")
            .AppendLine("       AND T9.INUSE_FLG = '1' ")
            .AppendLine("       LEFT JOIN TB_M_PRIVATE_FLEET_ITEM T10 ")
            .AppendLine("           ON T1.PRIVATE_FLEET_ITEM_CD = T10.PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("           AND T1.FLEET_FLG = T10.FLEET_FLG ")
            .AppendLine("           AND T10.INUSE_FLG = '1' ")
            .AppendLine("       LEFT JOIN TB_LM_PRIVATE_FLEET_ITEM T11 ")
            .AppendLine("           ON T10.PRIVATE_FLEET_ITEM_CD = T11.PRIVATE_FLEET_ITEM_CD ")
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
            .AppendLine("        WHERE ")
            ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 END

            '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            If SystemFrameworks.Core.iCROP.BizLogic.Operation.SSM = opeCd Then
                'マネージャ検索は前方一致のみ。
                '電話番号検索はGetTelSearchCountCustomerのみ使用する為処理不要
                Select Case serchType
                    Case IdSerchName      '顧客名称
                        .AppendLine("        T1.CST_NAME_SEARCH like :SEARCHPARAM || '%' AND ")
                    Case IdSerchVin       'VIN
                        .AppendLine("        T5.VCL_VIN_SEARCH like :SEARCHPARAM || '%' AND ")
                    Case IdSerchVclregno  '車両登録No
                        .AppendLine("        T4.REG_NUM_SEARCH like :SEARCHPARAM || '%' AND ")
                End Select
            Else
                '2014/08/01 TCS 市川 TMT切替BTS-115対応 END
                If (serchDirection = IdSerchdirectionAfter) Then
                    '前方一致
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        T1.CST_NAME_SEARCH like :NAME || '%' AND ")
                        Case IdSerchVin       'VIN
                            .Append("        T5.VCL_VIN_SEARCH like :VIN || '%' AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        T4.REG_NUM_SEARCH like :VCLREGNO || '%' AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            .Append("       (T1.CST_PHONE_SEARCH like :TELNO || '%' ")
                            .Append("    OR  T1.CST_MOBILE_SEARCH like :TELNO || '%') AND ")
                        Case Else
                    End Select
                ElseIf (serchDirection = IdSerchdirectionSame) Then
                    '完全一致
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        T1.CST_NAME_SEARCH = :NAME AND ")
                        Case IdSerchVin       'VIN
                            .Append("        T5.VCL_VIN_SEARCH = :VIN AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        T4.REG_NUM_SEARCH = :VCLREGNO AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            .Append("       (T1.CST_PHONE_SEARCH = :TELNO ")
                            .Append("    OR  T1.CST_MOBILE_SEARCH = :TELNO ) AND ")
                        Case Else
                    End Select
                Else
                    'あいまい検索
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        T1.CST_NAME_SEARCH like '%' || :NAME || '%' AND ")
                        Case IdSerchVin       'VIN
                            .Append("        T5.VCL_VIN_SEARCH like '%' || :VIN || '%' AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        T4.REG_NUM_SEARCH like '%' || :VCLREGNO || '%' AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            .Append("       (T1.CST_PHONE_SEARCH like '%' || :TELNO || '%' ")
                            .Append("    OR  T1.CST_MOBILE_SEARCH like '%' || :TELNO || '%') AND ")
                        Case Else
                    End Select
                End If

                '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            End If
            '2014/08/01 TCS 市川 TMT切替BTS-115対応 END

            ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 START
            .AppendLine("            T2.DLR_CD = :DLRCD AND ")
            .AppendLine("            ((T2.CST_TYPE ='1' AND ")
            .AppendLine("            Trim(T5.VCL_VIN) IS NOT NULL ) OR ")
            .AppendLine("            (T2.CST_TYPE ='2')) AND ")
            .AppendLine("            T3.OWNER_CHG_FLG = '0' AND ")
            .AppendLine("            T3.CST_VCL_TYPE = '1' ")
            If leaderFlg Then
                .AppendLine("        AND T9.ORGNZ_ID IN (" & searchOrg & ") ")
                '2012/02/29 TCS 河原【SALES_1B】START
            ElseIf opeCd = SystemFrameworks.Core.iCROP.BizLogic.Operation.SSF Then
                .AppendLine("        AND T3.SLS_PIC_STF_CD = :STAFFCD ")
            End If
            '2012/02/29 TCS 河原【SALES_1B】END

            '1：顧客名称、2：モデル名称、3：セールススタッフ名称、4：サービスアドバイザー名称
            Select Case sortType
                Case IdSortName         '顧客名称
                    .Append("ORDER BY NAME ")
                    If (sortOrder <> IdOrderAsc) Then .Append(" DESC ")
                Case IdSortModel         'モデル名称
                    .Append("ORDER BY VCLREGNO ")
                    If (sortOrder <> IdOrderAsc) Then .Append(" DESC ")
                Case IdSortSsusername    'セールススタッフ名称
                    .Append("ORDER BY SSUSERNAME ")
                    If (sortOrder <> IdOrderAsc) Then .Append(" DESC ")
                Case IdSortSausername    'サービスアドバイザー名称
                    .Append("ORDER BY SAUSERNAME ")
                    If (sortOrder <> IdOrderAsc) Then .Append(" DESC ")
                Case Else
            End Select
            ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 END
        End With

        Using query As New DBSelectQuery(Of SC3080101DataSet.SC3080101CustDataTable)("SC3080101_202")
            '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            If SystemFrameworks.Core.iCROP.BizLogic.Operation.SSM = opeCd Then
                'MG検索はBind変数を使用せずリテラルにする。(性能対策)
                query.CommandText = sql.ToString() _
                     .Replace(":DLRCD", "'" & dlrcd & "'") _
                     .Replace(":SEARCHPARAM", "'" & serchValue.Replace("'", "''").Replace("\", "") & "'")
            Else
                '2014/08/01 TCS 市川 TMT切替BTS-115対応 END

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)          '販売店コード

                '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                Select Case serchType
                    Case IdSerchName      '顧客名称
                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, serchValue)
                    Case IdSerchVin       'VIN
                        query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, serchValue)
                    Case IdSerchVclregno  '車両登録No
                        query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, serchValue)
                    Case IdSerchTel       '電話番号/携帯番号
                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, serchValue)
                    Case Else
                End Select


                '2012/02/29 TCS 河原【SALES_1B】START
                ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 START
                If Not leaderFlg AndAlso opeCd = SystemFrameworks.Core.iCROP.BizLogic.Operation.SSF Then
                    ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 END
                    query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, account)
                End If
                '2012/02/29 TCS 河原【SALES_1B】END

                '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            End If
            '2014/08/01 TCS 市川 TMT切替BTS-115対応 END

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerList_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()

        End Using
        '2013/06/30 TCS 吉村 2013/10対応版　既存流用 END

    End Function


    '2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 顧客件数取得　(電話番号検索用ＳＱＬ)
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="serchDirection">検索方向</param>
    ''' <param name="serchType">検索タイプ</param>
    ''' <param name="serchValue">検索テキスト</param>
    ''' 2012/11/15 TCS 藤井 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
    ''' <param name="serchFlg">電話番号検索フラグ</param>
    ''' 2012/11/15 TCS 藤井 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
    ''' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
    ''' <param name="searchOrg">組織ID(店舗内全組織ID)</param>
    ''' 2013/12/03 TCS 森    Aカード情報相互連携開発 END
    ''' <returns>顧客件数</returns>
    ''' <remarks></remarks>
    Public Shared Function GetTelSearchCountCustomer(ByVal dlrcd As String, _
                                     ByVal serchDirection As Integer, _
                                     ByVal serchType As Integer, _
                                     ByVal serchValue As String, _
                                     ByVal serchFlg As Integer, _
                                     ByVal searchOrg As String) As Integer
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetTelSearchCountCustomer_Start")
        'ログ出力 End *****************************************************************************


        If (serchType < 0) Then
            Return Nothing
        End If

        '2012/02/29 TCS 河原【SALES_1B】START
        Dim opeCd As Integer = StaffContext.Current.OpeCD
        Dim account As String = StaffContext.Current.Account
        '2012/02/29 TCS 河原【SALES_1B】END

        ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
        Dim leaderFlg As Boolean = StaffContext.Current.TeamLeader AndAlso searchOrg.Length > 0
        ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END

        Dim sql As New StringBuilder
        With sql
            ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
            .AppendLine("SELECT /* SC3080101_203 */ ")
            .AppendLine("        COUNT(1) AS CNT ")
            .AppendLine("    FROM ")
            .AppendLine("		TB_M_CUSTOMER T1 ")
            .AppendLine("		INNER JOIN TB_M_CUSTOMER_DLR T2 ")
            .AppendLine("		ON T1.CST_ID = T2.CST_ID ")
            .AppendLine("       INNER JOIN TB_M_CUSTOMER_VCL T3 ")
            .AppendLine("       ON T2.DLR_CD = T3.DLR_CD ")
            .AppendLine("       AND T2.CST_ID = T3.CST_ID ")
            .AppendLine("       LEFT JOIN TB_M_VEHICLE_DLR T4 ")
            .AppendLine("       ON T3.DLR_CD = T4.DLR_CD ")
            .AppendLine("       AND T3.VCL_ID = T4.VCL_ID ")
            .AppendLine("       LEFT JOIN TB_M_VEHICLE T5 ")
            .AppendLine("       ON T3.VCL_ID = T5.VCL_ID ")
            .AppendLine("       LEFT JOIN TB_M_STAFF T6 ")
            .AppendLine("       ON T3.SLS_PIC_STF_CD = T6.STF_CD ")
            .AppendLine("       AND T6.INUSE_FLG = '1' ")
            .AppendLine("  WHERE ")
            .AppendLine("           T2.DLR_CD = :DLRCD AND ")
            .AppendLine("           ((T2.CST_TYPE ='1' AND ")
            .AppendLine("           Trim(T5.VCL_VIN) IS NOT NULL ) OR ")
            .AppendLine("           (T2.CST_TYPE ='2')) AND ")
            .AppendLine("           T3.OWNER_CHG_FLG = '0' AND ")
            .AppendLine("           T3.CST_VCL_TYPE = '1' ")
            '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            If SystemFrameworks.Core.iCROP.BizLogic.Operation.SSM = opeCd Then
                'マネージャ検索は前方一致のみ。
                Select Case serchType
                    Case IdSerchTel '電話番号
                        .Append("    AND (T1.CST_PHONE_SEARCH like :SEARCHPARAM || '%' ")
                        .Append("    OR  T1.CST_MOBILE_SEARCH like :SEARCHPARAM || '%') ")
                    Case IdSerchSolId '国民番号
                        .AppendLine("    AND T1.CST_SOCIALNUM_SEARCH like :SEARCHPARAM || '%' ")
                End Select
            Else
                '2014/08/01 TCS 市川 TMT切替BTS-115対応 END

                If serchFlg = 0 Then
                    If leaderFlg Then
                        .AppendLine("       AND T6.ORGNZ_ID IN (" & searchOrg & ") ")
                    ElseIf opeCd = SystemFrameworks.Core.iCROP.BizLogic.Operation.SSF Then
                        .AppendLine("       AND T3.SLS_PIC_STF_CD = :STAFFCD ")
                    End If
                End If
                If serchType = 4 Then
                    '電話番号で検索する場合
                    If (serchDirection = IdSerchdirectionAfter) Then
                        '前方一致
                        .Append("    AND (T1.CST_PHONE_SEARCH like :TELNO || '%' ")
                        .Append("    OR  T1.CST_MOBILE_SEARCH like :TELNO || '%') ")
                    ElseIf (serchDirection = IdSerchdirectionSame) Then
                        '完全一致
                        .Append("    AND (T1.CST_PHONE_SEARCH = :TELNO ")
                        .Append("    OR  T1.CST_MOBILE_SEARCH = :TELNO ) ")
                    Else
                        'あいまい検索
                        .Append("    AND (T1.CST_PHONE_SEARCH like '%' || :TELNO || '%' ")
                        .Append("    OR  T1.CST_MOBILE_SEARCH like '%' || :TELNO || '%') ")
                    End If
                ElseIf serchType = 6 Then
                    '国民番号で検索する場合
                    If (serchDirection = IdSerchdirectionAfter) Then
                        .AppendLine("    AND T1.CST_SOCIALNUM_SEARCH like :CST_SOCIALNUM || '%' ")
                    ElseIf (serchDirection = IdSerchdirectionSame) Then
                        '完全一致
                        .AppendLine("    AND T1.CST_SOCIALNUM_SEARCH = :CST_SOCIALNUM ")
                    Else
                        'あいまい検索
                        .AppendLine("    AND T1.CST_PHONE_SEARCH like '%' || :CST_SOCIALNUM || '%' ")
                    End If
                End If
                ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END

                '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            End If
            '2014/08/01 TCS 市川 TMT切替BTS-115対応 END
        End With

        Using query As New DBSelectQuery(Of SC3080101DataSet.SC3080101CntDataTable)("SC3080101_203")
            '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            If SystemFrameworks.Core.iCROP.BizLogic.Operation.SSM = opeCd Then
                'MG検索はBind変数を使用せずリテラルにする。(性能対策)
                query.CommandText = sql.ToString() _
                     .Replace(":DLRCD", "'" & dlrcd & "'") _
                     .Replace(":SEARCHPARAM", "'" & serchValue.Replace("'", "''").Replace("\", "") & "'")
            Else
                '2014/08/01 TCS 市川 TMT切替BTS-115対応 END
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)          '販売店コード

                '4: 電話番号/携帯番号 6:国民番号
                If serchType = 4 Then
                    query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, serchValue)
                ElseIf serchType = 6 Then
                    query.AddParameterWithTypeValue("CST_SOCIALNUM", OracleDbType.NVarchar2, serchValue)
                End If

                '2012/02/29 TCS 河原【SALES_1B】START
                ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 START
                If serchFlg = 0 AndAlso Not leaderFlg AndAlso opeCd = SystemFrameworks.Core.iCROP.BizLogic.Operation.SSF Then
                    ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 END
                    query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, account)
                End If
                '2012/02/29 TCS 河原【SALES_1B】END

                '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            End If
            '2014/08/01 TCS 市川 TMT切替BTS-115対応 END
            Dim cntTbl As SC3080101DataSet.SC3080101CntDataTable

            cntTbl = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetTelSearchCountCustomer_End")
            'ログ出力 End *****************************************************************************
            Return CType(cntTbl.Item(0).Cnt, Integer)

        End Using
        '2013/06/30 TCS 吉村 2013/10対応版　既存流用 END

    End Function
    '2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 顧客一覧取得　(電話番号検索用ＳＱＬ)
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="serchDirection">検索方向</param>
    ''' <param name="serchType">検索タイプ</param>
    ''' <param name="serchValue">検索テキスト</param>
    ''' <param name="sortType">ソート条件</param>
    ''' <param name="sortOrder">ソート方向</param>
    ''' 2012/11/15 TCS 藤井 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
    ''' <param name="serchFlg">電話番号検索フラグ</param>
    ''' 2012/11/15 TCS 藤井 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
    ''' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
    ''' <param name="searchOrg">組織ID(店舗内全組織ID)</param>
    ''' 2013/12/03 TCS 森    Aカード情報相互連携開発 END
    ''' <returns>顧客一覧</returns>
    ''' <remarks></remarks>
    Public Shared Function GetTelSearchCustomerList(ByVal dlrcd As String, _
                                     ByVal serchDirection As Integer, _
                                     ByVal serchType As Integer, _
                                     ByVal serchValue As String, _
                                     ByVal sortType As Integer, _
                                     ByVal sortOrder As Integer, _
                                     ByVal serchFlg As Integer, _
                                     ByVal searchOrg As String) As SC3080101DataSet.SC3080101CustDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetTelSearchCustomerList_Start")
        'ログ出力 End *****************************************************************************

        If (serchType < 0) Then
            Return Nothing
        End If
        If (sortType < 0) Then
            Return Nothing
        End If

        '2012/02/29 TCS 河原【SALES_1B】START
        Dim opeCd As Integer = StaffContext.Current.OpeCD
        Dim account As String = StaffContext.Current.Account
        '2012/02/29 TCS 河原【SALES_1B】END

        ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
        Dim leaderFlg As Boolean = StaffContext.Current.TeamLeader AndAlso searchOrg.Length > 0
        ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END

        Dim sql As New StringBuilder
        With sql
            ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 START
            .AppendLine("SELECT /* SC3080101_204 */ ")
            .AppendLine("         T2.CST_TYPE AS CSTKIND, ")
            .AppendLine("            CASE WHEN T1.FLEET_FLG = '0' THEN '1' ")
            .AppendLine("                 WHEN T1.FLEET_FLG = '1' THEN '0' ")
            .AppendLine("            END AS CUSTYPE, ")
            .AppendLine("            T1.CST_ID AS CRCUSTID , ")
            .AppendLine("            T2.IMG_FILE_LARGE AS IMAGEFILE_L , ")
            .AppendLine("            T2.IMG_FILE_MEDIUM AS IMAGEFILE_M , ")
            .AppendLine("            T2.IMG_FILE_SMALL AS IMAGEFILE_S , ")
            .AppendLine("            T1.NAMETITLE_NAME AS NAMETITLE , ")
            .AppendLine("            T1.CST_NAME AS NAME , ")
            .AppendLine("            T1.CST_PHONE AS TELNO , ")
            .AppendLine("            T1.CST_MOBILE AS MOBILE , ")
            '2019/05/23 TS  重松 (FS)納車時オペレーションCS向上にむけた評価（UAT-0192） START
            .AppendLine("            CASE WHEN T2.CST_TYPE = '1' THEN T6.MODEL_NAME ")
            .AppendLine("                 WHEN T2.CST_TYPE = '2' THEN T5.NEWCST_MODEL_NAME ")
            .AppendLine("            END AS SERIESNM,  ")
            '2019/05/23 TS  重松 (FS)納車時オペレーションCS向上にむけた評価（UAT-0192） END
            .AppendLine("            T4.REG_NUM AS VCLREGNO , ")
            .AppendLine("            T5.VCL_VIN AS VIN , ")
            .AppendLine("            T3.VCL_ID AS SEQNO , ")
            .AppendLine("            T7.USERNAME AS SSUSERNAME , ")
            .AppendLine("            T8.USERNAME AS SAUSERNAME , ")
            .AppendLine("            T3.SLS_PIC_STF_CD AS STAFFCD, ")
            .AppendLine("            T1.CST_SOCIALNUM ")
            '2018/07/10 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 START
            .AppendLine("            ,T4.IMP_VCL_FLG ")
            '2018/07/10 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 END
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
            .AppendLine("            ,NVL(T11.CST_JOIN_TYPE, ' ') AS CSTJOINTYPE ")
            .AppendLine("FROM ")
            .AppendLine("		TB_M_CUSTOMER T1 ")
            .AppendLine("		INNER JOIN TB_M_CUSTOMER_DLR T2 ")
            .AppendLine("		ON T1.CST_ID = T2.CST_ID ")
            .AppendLine("       INNER JOIN TB_M_CUSTOMER_VCL T3 ")
            .AppendLine("       ON T2.DLR_CD = T3.DLR_CD ")
            .AppendLine("       AND T2.CST_ID = T3.CST_ID ")
            .AppendLine("       LEFT JOIN TB_M_VEHICLE_DLR T4 ")
            .AppendLine("       ON T3.DLR_CD = T4.DLR_CD ")
            .AppendLine("       AND T3.VCL_ID = T4.VCL_ID ")
            .AppendLine("       LEFT JOIN TB_M_VEHICLE T5 ")
            .AppendLine("       ON T3.VCL_ID = T5.VCL_ID ")
            .AppendLine("       LEFT JOIN TB_M_MODEL T6 ")
            .AppendLine("       ON T5.MODEL_CD = T6.MODEL_CD ")
            .AppendLine("       LEFT JOIN TBL_USERS T7 ")
            .AppendLine("       ON T3.SLS_PIC_STF_CD = RTRIM(T7.ACCOUNT) ")
            .AppendLine("       AND T7.DELFLG = '0' ")
            .AppendLine("       LEFT JOIN TBL_USERS T8 ")
            .AppendLine("       ON T3.SVC_PIC_STF_CD = RTRIM(T8.ACCOUNT) ")
            .AppendLine("       AND T8.DELFLG = '0' ")
            .AppendLine("       LEFT JOIN TB_M_STAFF T9 ")
            .AppendLine("       ON T3.SLS_PIC_STF_CD = T9.STF_CD ")
            .AppendLine("       AND T9.INUSE_FLG = '1' ")
            .AppendLine("       LEFT JOIN TB_M_PRIVATE_FLEET_ITEM T10 ")
            .AppendLine("           ON T1.PRIVATE_FLEET_ITEM_CD = T10.PRIVATE_FLEET_ITEM_CD ")
            .AppendLine("           AND T1.FLEET_FLG = T10.FLEET_FLG ")
            .AppendLine("           AND T10.INUSE_FLG = '1' ")
            .AppendLine("       LEFT JOIN TB_LM_PRIVATE_FLEET_ITEM T11 ")
            .AppendLine("           ON T10.PRIVATE_FLEET_ITEM_CD = T11.PRIVATE_FLEET_ITEM_CD ")
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
            .AppendLine("WHERE ")
            .AppendLine("            T2.DLR_CD = :DLRCD AND ")
            .AppendLine("            ((T2.CST_TYPE ='1' AND ")
            .AppendLine("            Trim(T5.VCL_VIN) IS NOT NULL ) OR ")
            .AppendLine("            (T2.CST_TYPE ='2')) AND ")
            .AppendLine("            T3.OWNER_CHG_FLG = '0' AND ")
            .AppendLine("            T3.CST_VCL_TYPE = '1' ")
            '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            If SystemFrameworks.Core.iCROP.BizLogic.Operation.SSM = opeCd Then
                'マネージャ検索は前方一致のみ。
                Select Case serchType
                    Case IdSerchTel '電話番号
                        .Append("    AND (T1.CST_PHONE_SEARCH like :SEARCHPARAM || '%' ")
                        .Append("    OR  T1.CST_MOBILE_SEARCH like :SEARCHPARAM || '%') ")
                    Case IdSerchSolId '国民番号
                        .AppendLine("    AND T1.CST_SOCIALNUM_SEARCH like :SEARCHPARAM || '%' ")
                End Select
            Else
                '2014/08/01 TCS 市川 TMT切替BTS-115対応 END
                If serchFlg = 0 Then
                    If leaderFlg Then
                        .AppendLine("        AND T9.ORGNZ_ID IN (" & searchOrg & ") ")
                    ElseIf opeCd = SystemFrameworks.Core.iCROP.BizLogic.Operation.SSF Then
                        .AppendLine("        AND T3.SLS_PIC_STF_CD = :STAFFCD ")
                    End If
                End If
                If serchType = 4 Then
                    ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 END
                    If (serchDirection = IdSerchdirectionAfter) Then
                        '前方一致
                        .AppendLine("    AND  (T1.CST_PHONE_SEARCH like :TELNO || '%' ")
                        .AppendLine("    OR  T1.CST_MOBILE_SEARCH like :TELNO || '%') ")
                    ElseIf (serchDirection = IdSerchdirectionSame) Then
                        '完全一致
                        .AppendLine("    AND  (T1.CST_PHONE_SEARCH = :TELNO ")
                        .AppendLine("    OR  T1.CST_MOBILE_SEARCH = :TELNO ) ")
                    Else
                        'あいまい検索
                        .AppendLine("    AND  (T1.CST_PHONE_SEARCH like '%' || :TELNO || '%' ")
                        .AppendLine("    OR  T1.CST_MOBILE_SEARCH like '%' || :TELNO || '%') ")
                    End If
                    ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
                ElseIf serchType = 6 Then
                    '国民番号で検索する場合
                    If (serchDirection = IdSerchdirectionAfter) Then
                        '前方一致
                        .AppendLine("      AND T1.CST_SOCIALNUM_SEARCH like :CST_SOCIALNUM || '%' ")
                    ElseIf (serchDirection = IdSerchdirectionSame) Then
                        '完全一致
                        .AppendLine("      AND T1.CST_SOCIALNUM_SEARCH = :CST_SOCIALNUM ")
                    Else
                        'あいまい検索
                        .AppendLine("      AND T1.CST_SOCIALNUM_SEARCH like '%' || :CST_SOCIALNUM || '%' ")
                    End If
                    ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END
                End If
                '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            End If
            '2014/08/01 TCS 市川 TMT切替BTS-115対応 END

            ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 START
            '1：顧客名称、2：モデル名称、3：セールススタッフ名称、4：サービスアドバイザー名称
            Select Case sortType
                Case IdSortName         '顧客名称
                    .Append("ORDER BY NAME ")
                    If (sortOrder <> IdOrderAsc) Then .Append(" DESC ")
                Case IdSortModel         'モデル名称
                    .Append("ORDER BY VCLREGNO ")
                    If (sortOrder <> IdOrderAsc) Then .Append(" DESC ")
                Case IdSortSsusername    'セールススタッフ名称
                    .Append("ORDER BY SSUSERNAME ")
                    If (sortOrder <> IdOrderAsc) Then .Append(" DESC ")
                Case IdSortSausername    'サービスアドバイザー名称
                    .Append("ORDER BY SAUSERNAME ")
                    If (sortOrder <> IdOrderAsc) Then .Append(" DESC ")
                Case Else
            End Select
            ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 END
        End With

        Using query As New DBSelectQuery(Of SC3080101DataSet.SC3080101CustDataTable)("SC3080101_204")
            '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            If SystemFrameworks.Core.iCROP.BizLogic.Operation.SSM = opeCd Then
                'MG検索はBind変数を使用せずリテラルにする。(性能対策)
                query.CommandText = sql.ToString() _
                     .Replace(":DLRCD", "'" & dlrcd & "'") _
                     .Replace(":SEARCHPARAM", "'" & serchValue.Replace("'", "''").Replace("\", "") & "'")
            Else
                '2014/08/01 TCS 市川 TMT切替BTS-115対応 END

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                If serchType = 4 Then
                    query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, serchValue)
                End If

                ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 START
                If serchFlg = 0 AndAlso Not leaderFlg AndAlso opeCd = SystemFrameworks.Core.iCROP.BizLogic.Operation.SSF Then
                    ' 2013/12/25 TCS 市川 Aカード情報相互連携開発 END
                    query.AddParameterWithTypeValue("STAFFCD", OracleDbType.Char, account)
                End If

                ' 2013/12/03 TCS 森    Aカード情報相互連携開発 START
                If serchType = 6 Then
                    query.AddParameterWithTypeValue("CST_SOCIALNUM", OracleDbType.NVarchar2, serchValue)
                End If
                ' 2013/12/03 TCS 森    Aカード情報相互連携開発 END

                '2014/08/01 TCS 市川 TMT切替BTS-115対応 START
            End If
            '2014/08/01 TCS 市川 TMT切替BTS-115対応 END

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetTelSearchCustomerList_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()

        End Using
        '2013/06/30 TCS 吉村 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 権限情報取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="operationcode">権限コード</param>
    ''' <returns>SC3080205OrgCustomerDataTableDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetOperaType(ByVal dlrcd As String, _
                                        ByVal operationcode As Long) As SC3080101DataSet.SC3080101OperaTypeDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOperaType_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of SC3080101DataSet.SC3080101OperaTypeDataTable)("SC3080101_003")

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080101_003 */ ")
                .Append("    OPERATIONNAME ")
                .Append("FROM ")
                .Append("    TBL_OPERATIONTYPE ")
                .Append("WHERE ")
                .Append("    DLRCD = :DLRCD AND ")
                .Append("    OPERATIONCODE = :OPERATIONCODE ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
            query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Int64, operationcode)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetOperaType_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using
        '2013/06/30 TCS 吉村 2013/10対応版　既存流用 END

    End Function

    ''' <summary>
    ''' 店舗セールス組織取得
    ''' </summary>
    ''' <returns>SC3080101DataSet.SC3080101OrgDataTable</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBranchSalesOrganizations() As SC3080101DataSet.SC3080101OrgDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetBranchSalesOrganizations_Start")
        'ログ出力 End *****************************************************************************

        Dim dlrCd As String = StaffContext.Current.DlrCD
        Dim brnCd As String = StaffContext.Current.BrnCD

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3080101_205 */")
            .Append("    ORGNZ_ID")
            .Append("    ,PARENT_ORGNZ_ID")
            .Append("    ,ORGNZ_SC_FLG ")
            .Append("FROM ")
            .Append("    TB_M_ORGANIZATION ")
            .Append("WHERE ")
            .Append("    DLR_CD = :DLR_CD ")
            .Append("    AND BRN_CD = :BRN_CD ")
        End With

        Using query As New DBSelectQuery(Of SC3080101DataSet.SC3080101OrgDataTable)("SC3080101_205")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dlrCd)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, brnCd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerList_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function


    Public Function GetStringValue(ByVal val As String) As String
        Return val
    End Function

End Class

