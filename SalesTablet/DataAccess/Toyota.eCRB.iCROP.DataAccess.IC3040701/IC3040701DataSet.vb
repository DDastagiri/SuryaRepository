'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3040701DataSet.vb
'─────────────────────────────────────
'機能： テンプレート取得インターフェイス データアクセスクラス
'補足： 
'作成： 2014/05/13 TMEJ 曽山
'更新： 2016/01/07 NSK nakamura PRJ1504572_(トライ店システム評価)メールテンプレート機能強化(敬称置換文字追加) $01
'─────────────────────────────────────

Imports System.Linq
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace IC3040701DataSetTableAdapters

    ''' <summary>
    ''' テンプレート取得インターフェイス データアクセスクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IC3040701TableAdapters
        Inherits Global.System.ComponentModel.Component

#Region "定数"
        ''' <summary>
        ''' テンプレート区分
        ''' </summary>
        Private Class TemplateClass
            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks>インスタンス生成抑制のため</remarks>
            Private Sub New()
            End Sub

            ''' <summary>
            ''' e-Mail
            ''' </summary>
            Public Const EMail As String = "1"

            ''' <summary>
            ''' Line
            ''' </summary>
            Public Const Line As String = "2"
        End Class

        ''' <summary>
        ''' 顧客区分
        ''' </summary>
        Private Class CstType
            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks>インスタンス生成抑制のため</remarks>
            Private Sub New()
            End Sub

            ''' <summary>
            ''' 自社客
            ''' </summary>
            Public Const ExistCustomer As String = "1"

            ''' <summary>
            ''' 未取引客
            ''' </summary>
            Public Const NewCustomer As String = "2"
        End Class

        ''' <summary>
        ''' 距離期間区分
        ''' </summary>
        ''' <remarks></remarks>
        Private Class MileTermType
            ''' <summary>
            ''' コンストラクタ
            ''' </summary>
            ''' <remarks>インスタンス生成抑制のため</remarks>
            Private Sub New()
            End Sub

            ''' <summary>
            ''' 距離
            ''' </summary>
            Public Const Mile As String = "1"

            ''' <summary>
            ''' 期間
            ''' </summary>
            Public Const Term As String = "2"
        End Class
#End Region

#Region "公開メソッド"
        ''' <summary>
        ''' 顧客情報を顧客マスタから取得する。
        ''' </summary>
        ''' <param name="custmId">顧客ID</param>
        ''' <returns>顧客情報を返却する。</returns>
        ''' <remarks></remarks>
        Public Function GetCustomerInfo(ByVal custmId As Decimal) As IC3040701DataSet.IC3040701CustomerInfoDataTable
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(LogUtil.GetLogParam("custmId", CStr(custmId), False))

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3040701DataSet.IC3040701CustomerInfoDataTable)("IC3040701_001")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("SELECT  /* IC3040701_001 */")
                    .AppendLine("        CST.CST_NAME")
                    .AppendLine("      , CST.FLEET_PIC_NAME")
                    .AppendLine("      , CST.FLEET_PIC_DEPT")
                    .AppendLine("      , CST.FLEET_PIC_POSITION")
                    .AppendLine("      , CASE WHEN CST.CST_EMAIL_1 = N' ' THEN ")
                    .AppendLine("            CASE WHEN CST.CST_EMAIL_2 = N' ' THEN NULL")
                    .AppendLine("            ELSE CST.CST_EMAIL_2")
                    .AppendLine("            END")
                    .AppendLine("        ELSE CST.CST_EMAIL_1")
                    .AppendLine("        END AS CST_EMAIL")
                    ' $01 start PRJ1504572_(トライ店システム評価)メールテンプレート機能強化(敬称置換文字追加)
                    .AppendLine("      , CST.NAMETITLE_NAME")
                    ' $01 end   PRJ1504572_(トライ店システム評価)メールテンプレート機能強化(敬称置換文字追加)
                    .AppendLine("  FROM")
                    .AppendLine("      TB_M_CUSTOMER CST")
                    .AppendLine(" WHERE")
                    .AppendLine("      CST.CST_ID = :CST_ID")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, custmId)

                Dim dt As IC3040701DataSet.IC3040701CustomerInfoDataTable = query.GetData()
                Dim AcquisitionNumber As Integer = dt.Count

                Logger.Info(LogUtil.GetReturnParam(CStr(AcquisitionNumber)))
                Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return dt
            End Using
        End Function

        ''' <summary>
        ''' 販売店・店舗情報を、販売店マスタ・店舗マスタから取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <returns>販売店・店舗情報を返却する。</returns>
        ''' <remarks></remarks>
        Public Function GetDealerBranchInfo(ByVal dealerCode As String, ByVal storeCode As String) _
            As IC3040701DataSet.IC3040701DealerBranchInfoDataTable
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(LogUtil.GetLogParam("dealerCode", dealerCode, False) & _
                    LogUtil.GetLogParam("storeCode", storeCode, True))

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3040701DataSet.IC3040701DealerBranchInfoDataTable)("IC3040701_002")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("SELECT  /* IC3040701_002 */")
                    .AppendLine("        DLR.DLR_NAME")
                    .AppendLine("      , DLR.DLR_URL")
                    .AppendLine("      , BRN.BRN_NAME")
                    .AppendLine("  FROM")
                    .AppendLine("        TB_M_DEALER DLR")
                    .AppendLine("      , TB_M_BRANCH BRN")
                    .AppendLine(" WHERE")
                    .AppendLine("          DLR.DLR_CD = :DLR_CD")
                    .AppendLine("      AND DLR.DLR_CD = BRN.DLR_CD")
                    .AppendLine("      AND BRN.BRN_CD = :BRN_CD")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, storeCode)

                Dim dt As IC3040701DataSet.IC3040701DealerBranchInfoDataTable = query.GetData()
                Dim AcquisitionNumber As Integer = dt.Count

                Logger.Info(LogUtil.GetReturnParam(CStr(AcquisitionNumber)))
                Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return dt
            End Using
        End Function

        ''' <summary>
        ''' スタッフ名をスタッフマスタから取得する。
        ''' </summary>
        ''' <param name="staffCode">スタッフコード</param>
        ''' <returns>スタッフ名を返却する。</returns>
        ''' <remarks></remarks>
        Public Function GetStaffInfo(ByVal staffCode As String) _
            As IC3040701DataSet.IC3040701StaffInfoDataTable
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(LogUtil.GetLogParam("staffCode", staffCode, False))

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3040701DataSet.IC3040701StaffInfoDataTable)("IC3040701_003")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("SELECT /* IC3040701_003 */")
                    .AppendLine("       STF.STF_NAME")
                    .AppendLine("  FROM")
                    .AppendLine("      TB_M_STAFF STF")
                    .AppendLine(" WHERE")
                    .AppendLine("          STF.STF_CD = :STF_CD")
                    .AppendLine("      AND STF.INUSE_FLG = '1'")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("STF_CD", OracleDbType.NVarchar2, staffCode)

                Dim dt As IC3040701DataSet.IC3040701StaffInfoDataTable = query.GetData()
                Dim AcquisitionNumber As Integer = dt.Count

                Logger.Info(LogUtil.GetReturnParam(CStr(AcquisitionNumber)))
                Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return dt
            End Using
        End Function

        ''' <summary>
        ''' 車両関連情報を取得する。
        ''' </summary>
        ''' <param name="salesId">商談ID</param>
        ''' <returns>車両関連情報を返却する。ｓ</returns>
        ''' <remarks></remarks>
        Public Function GetVehicleInfo(ByVal salesId As Decimal) _
            As VehicleInfo
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(LogUtil.GetLogParam("salesId", CStr(salesId), False))

            Dim vehicleInfo As New VehicleInfo

            Using salesInfo As IC3040701DataSet.IC3040701SalesInfoDataTable = GetSalesInfo(salesId)

                If salesInfo.Count < 1 Then
                    Logger.Info(LogUtil.GetReturnParam(vehicleInfo.ToString))
                    Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
                    Return vehicleInfo
                End If

                Dim vclId As Decimal = salesInfo(0).VCL_ID
                Dim custType As String = salesInfo(0).CST_TYPE

                'yyyy/MM/dd形式
                If Not salesInfo(0).IsRECOMMEND_SCHE_DATENull() Then
                    vehicleInfo.CRDate = DateTimeFunc.FormatDate(3, salesInfo(0).RECOMMEND_SCHE_DATE)
                End If

                Dim isExist As Boolean = CstType.ExistCustomer.Equals(custType)
                Dim isNew As Boolean = CstType.NewCustomer.Equals(custType)

                ' 車両IDが未設定、顧客種別が"1"、"2"以外の場合は、空白となるため検索する必要がない
                If 0 < vclId AndAlso (isExist OrElse isNew) Then
                    Using modelInfo As IC3040701DataSet.IC3040701ModelInfoDataTable = GetModelInfo(vclId)
                        If 0 < modelInfo.Count Then
                            vehicleInfo.MakerName = If(isExist, modelInfo(0).MAKER_NAME, modelInfo(0).NEWCST_MAKER_NAME)
                            vehicleInfo.SeriesName = If(isExist, modelInfo(0).MODEL_NAME, modelInfo(0).NEWCST_MODEL_NAME)
                        End If
                    End Using
                End If

                Dim mileageTermType As String = salesInfo(0).MILE_TERM_TYPE
                Dim dealerCode As String = salesInfo(0).DLR_CD
                Dim svcCd As String = salesInfo(0).SVC_CD

                Dim isMile As Boolean = MileTermType.Mile.Equals(mileageTermType)
                Dim isTerm As Boolean = MileTermType.Term.Equals(mileageTermType)

                ' 距離期間区分が"1"、"2"以外の場合はサービス名が空白となるため、検索する必要はない
                If isMile OrElse isTerm Then
                    Using serviceInfo As IC3040701DataSet.IC3040701ServiceInfoDataTable = GetServiceInfo(dealerCode, svcCd)
                        If 0 < serviceInfo.Count Then
                            vehicleInfo.ServiceName = If(isMile, serviceInfo(0).SVC_NAME_MILE, serviceInfo(0).SVC_NAME_TERM)
                        End If
                    End Using
                End If
            End Using

            Logger.Info(LogUtil.GetReturnParam(vehicleInfo.ToString))
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
            Return vehicleInfo
        End Function

#End Region

#Region "非公開メソッド"

        ''' <summary>
        ''' 誘致・用件に関する情報を商談マスタから取得する。
        ''' </summary>
        ''' <param name="salesId">商談ID</param>
        ''' <returns>誘致・用件に関する情報を返却する。</returns>
        ''' <remarks></remarks>
        Private Function GetSalesInfo(ByVal salesId As Long) As IC3040701DataSet.IC3040701SalesInfoDataTable
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(LogUtil.GetLogParam("salesId", CStr(salesId), False))

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3040701DataSet.IC3040701SalesInfoDataTable)("IC3040701_004")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("SELECT  /* IC3040701_004 */")
                    .AppendLine("        A.DLR_CD")
                    .AppendLine("      , A.RECOMMEND_SCHE_DATE")
                    .AppendLine("      , A.VCL_ID")
                    .AppendLine("      , A.SVC_CD")
                    .AppendLine("      , A.MILE_TERM_TYPE")
                    .AppendLine("      , CSTDLR.CST_TYPE")
                    .AppendLine("  FROM")
                    .AppendLine("      (SELECT")
                    .AppendLine("               SALES.DLR_CD")
                    .AppendLine("             , SALES.CST_ID")
                    .AppendLine("             , ATT.RECOMMEND_SCHE_DATE")
                    .AppendLine("             , NVL(ATT.VCL_ID, REQ.VCL_ID) AS VCL_ID")
                    .AppendLine("             , ATT.SVC_CD")
                    .AppendLine("             , ATT.MILE_TERM_TYPE")
                    .AppendLine("         FROM ")
                    .AppendLine("               TB_T_SALES SALES")
                    .AppendLine("             , TB_T_ATTRACT ATT")
                    .AppendLine("             , TB_T_REQUEST REQ")
                    .AppendLine("        WHERE ")
                    .AppendLine("                 SALES.SALES_ID = :SALES_ID")
                    .AppendLine("             AND SALES.ATT_ID = ATT.ATT_ID(+)")
                    .AppendLine("             AND SALES.REQ_ID = REQ.REQ_ID(+)")
                    .AppendLine("       UNION ALL")
                    .AppendLine("       SELECT")
                    .AppendLine("               SALES.DLR_CD")
                    .AppendLine("             , SALES.CST_ID")
                    .AppendLine("             , ATT.RECOMMEND_SCHE_DATE")
                    .AppendLine("             , NVL(ATT.VCL_ID, REQ.VCL_ID) AS VCL_ID")
                    .AppendLine("             , ATT.SVC_CD")
                    .AppendLine("             , ATT.MILE_TERM_TYPE")
                    .AppendLine("         FROM ")
                    .AppendLine("               TB_H_SALES SALES")
                    .AppendLine("             , TB_H_ATTRACT ATT")
                    .AppendLine("             , TB_H_REQUEST REQ")
                    .AppendLine("        WHERE ")
                    .AppendLine("                 SALES.SALES_ID = :SALES_ID")
                    .AppendLine("             AND SALES.ATT_ID = ATT.ATT_ID(+)")
                    .AppendLine("             AND SALES.REQ_ID = REQ.REQ_ID(+)")
                    .AppendLine("      ) A")
                    .AppendLine("      , TB_M_CUSTOMER_DLR CSTDLR")
                    .AppendLine(" WHERE")
                    .AppendLine("          VCL_ID IS NOT NULL")
                    .AppendLine("      AND A.DLR_CD = CSTDLR.DLR_CD(+)")
                    .AppendLine("      AND A.CST_ID = CSTDLR.CST_ID(+)")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("SALES_ID", OracleDbType.Decimal, salesId)

                Dim dt As IC3040701DataSet.IC3040701SalesInfoDataTable = query.GetData()
                Dim AcquisitionNumber As Integer = dt.Count

                Logger.Info(LogUtil.GetReturnParam(CStr(AcquisitionNumber)))
                Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return dt
            End Using
        End Function

        ''' <summary>
        ''' モデル名・メーカー名を車両マスタから取得する。
        ''' </summary>
        ''' <param name="vclId">車両ID</param>
        ''' <returns>モデル名・メーカー名を返却する。</returns>
        ''' <remarks></remarks>
        Private Function GetModelInfo(ByVal vclId As Decimal) As IC3040701DataSet.IC3040701ModelInfoDataTable
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(LogUtil.GetLogParam("vclId", CStr(vclId), False))

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3040701DataSet.IC3040701ModelInfoDataTable)("IC3040701_005")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("SELECT  /* IC3040701_005 */")
                    .AppendLine("        MODEL.MODEL_NAME")
                    .AppendLine("      , VCL.NEWCST_MODEL_NAME")
                    .AppendLine("      , MAKER.MAKER_NAME")
                    .AppendLine("      , VCL.NEWCST_MAKER_NAME")
                    .AppendLine("  FROM")
                    .AppendLine("      TB_M_VEHICLE VCL")
                    .AppendLine("      , TB_M_MODEL MODEL")
                    .AppendLine("      , TB_M_MAKER MAKER")
                    .AppendLine(" WHERE")
                    .AppendLine("          VCL.VCL_ID = :VCL_ID")
                    .AppendLine("      AND VCL.MODEL_CD = MODEL.MODEL_CD(+)")
                    .AppendLine("      AND NVL(MODEL.INUSE_FLG, '1') = '1'")
                    .AppendLine("      AND MODEL.MAKER_CD = MAKER.MAKER_CD(+)")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, vclId)

                Dim dt As IC3040701DataSet.IC3040701ModelInfoDataTable = query.GetData()
                Dim AcquisitionNumber As Integer = dt.Count

                Logger.Info(LogUtil.GetReturnParam(CStr(AcquisitionNumber)))
                Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return dt
            End Using
        End Function

        ''' <summary>
        ''' サービス情報を定期点検サービスマスタから取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="svcCd">サービスコード</param>
        ''' <returns>サービス名(距離)、サービス名(期間)を返却する。</returns>
        ''' <remarks>指定した販売店コードで一致しない場合、販売店コード'XXXXX'のサービスからも検索する。</remarks>
        Private Function GetServiceInfo(ByVal dealerCode As String, ByVal svcCd As String) As IC3040701DataSet.IC3040701ServiceInfoDataTable
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(LogUtil.GetLogParam("dealerCode", dealerCode, False) & _
                    LogUtil.GetLogParam("svcCd", svcCd, True))

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3040701DataSet.IC3040701ServiceInfoDataTable)("IC3040701_005")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("SELECT  /* IC3040701_006 */")
                    .AppendLine("        SVC.SVC_NAME_MILE")
                    .AppendLine("      , SVC.SVC_NAME_TERM")
                    .AppendLine("  FROM")
                    .AppendLine("      (")
                    .AppendLine("         SELECT ")
                    .AppendLine("                 S.SVC_NAME_MILE")
                    .AppendLine("               , S.SVC_NAME_TERM")
                    .AppendLine("               , CASE WHEN S.DLR_CD = N'XXXXX' THEN 1")
                    .AppendLine("                 ELSE 0")
                    .AppendLine("                 END AS SORTORDER")
                    .AppendLine("           FROM")
                    .AppendLine("               TB_M_SERVICE S")
                    .AppendLine("          WHERE")
                    .AppendLine("                  (S.DLR_CD = :DLR_CD OR S.DLR_CD = N'XXXXX')")
                    .AppendLine("               AND S.SVC_CD = :SVC_CD")
                    .AppendLine("       ORDER BY")
                    .AppendLine("               SORTORDER")
                    .AppendLine("      ) SVC")
                    .AppendLine(" WHERE")
                    .AppendLine("      ROWNUM <= 1")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("SVC_CD", OracleDbType.NVarchar2, svcCd)

                Dim dt As IC3040701DataSet.IC3040701ServiceInfoDataTable = query.GetData()
                Dim AcquisitionNumber As Integer = dt.Count

                Logger.Info(LogUtil.GetReturnParam(CStr(AcquisitionNumber)))
                Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return dt
            End Using
        End Function

        ''' <summary>
        ''' テンプレートを雛型マスタより取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="displayId">画面ID</param>
        ''' <param name="tempClass">テンプレート区分</param>
        ''' <returns>
        ''' テンプレートの本文、件名を返却する。
        ''' </returns>
        ''' <remarks>
        ''' テンプレート区分が「1：e-Mail」の場合、「雛型マスタ．e-Mail本文」、「雛型マスタ．e-Mail件名」を返却する。
        ''' （※雛型マスタ．シーケンス番号が複数続く場合、全てのレコードの「雛型マスタ．e-Mail本文」を結合して返却する。）
        ''' テンプレート区分が「2：LINE」の場合、「雛型マスタ．LINE本文」を返却する。
        ''' テンプレート区分が上記以外の場合、空文字を返す。
        ''' ※LINE本文は最大文字数が500文字の為、統合の処理は行わない。）
        ''' </remarks>
        Public Function GetTemplateInfo(ByVal dealerCode As String, ByVal storeCode As String, _
                                        ByVal displayId As String, ByVal tempClass As String) _
            As ReplacedTemplate
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
            Logger.Info(LogUtil.GetLogParam("dealerCode", dealerCode, False) & _
                    LogUtil.GetLogParam("storeCode", storeCode, True) & _
                    LogUtil.GetLogParam("displayId", displayId, True) & _
                    LogUtil.GetLogParam("tempClass", tempClass, True))

            'DBSelectQueryインスタンス生成
            Using query As New DBSelectQuery(Of IC3040701DataSet.IC3040701TemplateInfoDataTable)("IC3040701_007")
                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("  SELECT  /* IC3040701_007 */")
                    .AppendLine("          TMP.ORGMAILSUBJECT")
                    .AppendLine("        , TMP.ORGMAILTEXT")
                    .AppendLine("        , TMP.LINETEXT")
                    .AppendLine("    FROM")
                    .AppendLine("        TBL_TEMPLATE@RMM TMP")
                    .AppendLine("   WHERE")
                    .AppendLine("            RTRIM(TMP.DLRCD) = :DLR_CD")
                    .AppendLine("        AND RTRIM(TMP.STRCD) = :STR_CD")
                    .AppendLine("        AND TMP.DISPLAYID = :DISPLAYID")
                    .AppendLine("        AND TMP.DELFLG = '0'")
                    .AppendLine("ORDER BY")
                    .AppendLine("          TMP.TEMPLATEID")
                    .AppendLine("        , TMP.SEQNO")
                End With

                query.CommandText = sql.ToString()

                'SQLパラメータ設定
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STR_CD", OracleDbType.NVarchar2, storeCode)
                query.AddParameterWithTypeValue("DISPLAYID", OracleDbType.NVarchar2, displayId)

                Dim dt As IC3040701DataSet.IC3040701TemplateInfoDataTable = query.GetData()
                Dim AcquisitionNumber As Integer = dt.Count
                Dim template As New ReplacedTemplate

                If AcquisitionNumber = 0 Then
                    template.Result = ReplacedTemplate.TemplateResult.NotFound

                    Logger.Info(LogUtil.GetReturnParam(template.ToString))
                    Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
                    Return template
                End If

                ' テンプレート区分がe-Mailの場合、全てのレコードの「雛型マスタ．e-Mail本文」を結合して返却する。
                If TemplateClass.EMail.Equals(tempClass) Then

                    Dim sb As New StringBuilder

                    For i = 0 To AcquisitionNumber - 1
                        sb.Append(dt(i).ORGMAILTEXT)
                    Next

                    template.Subject = dt(0).ORGMAILSUBJECT
                    template.Text = sb.ToString

                ElseIf TemplateClass.Line.Equals(tempClass) Then
                    template.Text = dt(0).LINETEXT
                Else
                    template.Text = String.Empty
                End If

                template.Result = ReplacedTemplate.TemplateResult.Success
                template.TemplateClass = tempClass

                Logger.Info(LogUtil.GetReturnParam(template.ToString))
                Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))

                Return template
            End Using
        End Function

#End Region

    End Class

End Namespace

Partial Public Class IC3040701DataSet

End Class