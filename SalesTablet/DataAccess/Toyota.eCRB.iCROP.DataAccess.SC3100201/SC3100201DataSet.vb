'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3100201DataSet.vb
'──────────────────────────────────
'機能： 未対応来店客
'補足： 
'作成： 2011/12/12 KN  k.nagasawa
'更新： 2012/08/28 TMEJ m.okamura 新車受付機能改善 $01
'更新： 2013/03/01 TMEJ t.shimamura 新車タブレット受付画面管理指標変更対応 $02
'更新： 2013/05/24 TMEJ t.shimamura 【A.STEP2】次世代e-CRB新車タブレット　新DB適応に向けた機能開発 $03
'──────────────────────────────────

Imports Oracle.DataAccess.Client
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization

Namespace SC3100201DataSetTableAdapters
    ''' <summary>
    ''' 未対応来店客のデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3100201TableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' 削除フラグ - 有効データ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DelflgOn As String = "0"

        ''' <summary>
        ''' 削除フラグ - 削除データ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DelflgOff As String = "1"

        ''' <summary>
        ''' 表示種別 - ブロードキャスト対応来店実績一覧
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DispClassBroadcast As String = "02"

        ''' <summary>
        ''' 表示種別 - セールススタッフ指定対応来店実績一覧
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DispClassStaffSpecify As String = "01"

        ''' <summary>
        ''' 顧客分類 - 所有者
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CustomerClassOwner As String = "1"

        ''' <summary>
        ''' 依頼種別 - 来店
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ReqClassVisit As String = "04"

        ''' <summary>
        ''' 来店実績ステータス - フリー
        ''' </summary>
        Private Const VisitStatusFree As String = "01"

        ''' <summary>
        ''' 来店実績ステータス - フリー(ブロードキャスト)
        ''' </summary>
        Private Const VisitStatusFreeBroud As String = "02"

        ''' <summary>
        ''' 来店実績ステータス - 調整中
        ''' </summary>
        Private Const VisitStatusAdjust As String = "03"

        ''' <summary>
        ''' 来店実績ステータス - 確定(ブロードキャスト)
        ''' </summary>
        Private Const VisitStatusDefinitionBroud As String = "04"

        ''' <summary>
        ''' 来店実績ステータス - 確定
        ''' </summary>
        Private Const VisitStatusDefinition As String = "05"

        ''' <summary>
        ''' 来店実績ステータス - 待ち
        ''' </summary>
        Private Const VisitStatusWait As String = "06"

        ''' <summary>
        ''' 来店実績ステータス - 商談中
        ''' </summary>
        Private Const VisitStatusSalesStart As String = "07"

        ' $01 start 複数顧客に対する商談平行対応
        ''' <summary>
        ''' 来店実績ステータス - 商談中断
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusNegotiateStop As String = "09"
        ' $01 end   複数顧客に対する商談平行対応

        ' $02 start 納車作業ステータス対応
        ''' <summary>
        ''' 来店実績ステータス - 納車作業中
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VisitStatusDeliverlyStart As String = "11"
        ' $02 end   納車作業ステータス対応


#End Region

#Region "コンストラクタ"

        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

            ' 処理なし

        End Sub

#End Region

#Region "未対応来店客一覧"

        ''' <summary>
        ''' ブロードキャスト対応来店実績一覧を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="account">アカウント</param>
        ''' <param name="visitTimestampStart">来店日時開始</param>
        ''' <param name="visitTimestampEnd">来店日時終了</param>
        ''' <returns>ブロードキャスト来店実績データセット</returns>
        ''' <remarks></remarks>
        Public Function GetVisitBroadcast(ByVal dealerCode As String, _
                                          ByVal storeCode As String, _
                                          ByVal account As String, _
                                          ByVal visitTimestampStart As Date, _
                                          ByVal visitTimestampEnd As Date) As SC3100201DataSet.NotDealVisitDataTable

            Dim startSb As New StringBuilder
            startSb.Append(dealerCode).Append(", ")
            startSb.Append(storeCode).Append(", ")
            startSb.Append(account).Append(", ")
            startSb.Append(visitTimestampStart).Append(", ")
            startSb.Append(visitTimestampEnd)
            Logger.Info("GetVisitBroadcast_Start Param[" & startSb.ToString() & "]")
            startSb = Nothing

            ' ブロードキャスト対応来店実績
            Dim dt As SC3100201DataSet.NotDealVisitDataTable = Nothing

            Using query As New DBSelectQuery( _
                    Of SC3100201DataSet.NotDealVisitDataTable)("SC3100201_001")

                ' SQL文作成
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3100201_001 */")
                    .Append("        VS.VISITSEQ")
                    .Append("      , VS.VISITSTATUS")
                    .Append("      , VS.VISITTIMESTAMP")
                    .Append("      , VS.CUSTSEGMENT")
                    .Append("      , VS.CUSTID")
                    .Append("      , VS.TENTATIVENAME")
                    .Append("      , VS.VCLREGNO")
                    .Append("      , VS.VISITMEANS")
                    .Append("      , VS.VISITPERSONNUM")
                    .Append("      , VS.SALESTABLENO")
                    .Append("      , VS.STAFFCD AS CUSTSTAFFCD")
                    .Append("      , US.USERNAME AS CUSTSTAFFNAME")
                    .Append("      , VS.ACCOUNT AS DEALSTAFFCD")
                    .Append("      , UA.USERNAME AS DEALSTAFFNAME")
                    .Append("      , UA.ORG_IMGFILE AS DEALSTAFFIMAGE")
                    .Append("      , :DISPCLASS AS DISPCLASS")
                    .Append("      , TO_CHAR(VS.UPDATEDATE ,'YYYY/MM/DD HH24:MI:SS') AS UPDATEDATE")
                    .Append("   FROM TBL_VISIT_SALES VS")
                    .Append("      , TBL_VISITDEAL_NOTICE NOTICE")
                    .Append("      , TBL_USERS US")
                    .Append("      , TBL_USERS UA")
                    .Append("  WHERE VS.VISITSEQ = NOTICE.VISITSEQ")
                    .Append("    AND VS.STAFFCD = US.ACCOUNT(+)")
                    .Append("    AND VS.ACCOUNT = UA.ACCOUNT(+)")
                    .Append("    AND VS.DLRCD = :DLRCD")
                    .Append("    AND VS.STRCD = :STRCD")
                    .Append("    AND VS.VISITTIMESTAMP >= :VISITTIMESTAMP_START")
                    .Append("    AND VS.VISITTIMESTAMP <= :VISITTIMESTAMP_END")
                    .Append("    AND VS.VISITSTATUS = :VISITSTATUS")
                    .Append("    AND NOTICE.ACCOUNT = :ACCOUNT")
                    .Append("    AND NOTICE.DELFLG = :NOTICE_DELFLG")
                    .Append("    AND US.DELFLG(+) = :US_DELFLG")
                    .Append("    AND UA.DELFLG(+) = :UA_DELFLG")
                    .Append("  ORDER BY VS.VISITTIMESTAMP")
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("VISITTIMESTAMP_START", OracleDbType.Date, _
                        visitTimestampStart)
                query.AddParameterWithTypeValue("VISITTIMESTAMP_END", OracleDbType.Date, _
                        visitTimestampEnd)
                query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, _
                        VisitStatusFreeBroud)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)
                query.AddParameterWithTypeValue("NOTICE_DELFLG", OracleDbType.Char, DelflgOn)
                query.AddParameterWithTypeValue("US_DELFLG", OracleDbType.Char, DelflgOn)
                query.AddParameterWithTypeValue("UA_DELFLG", OracleDbType.Char, DelflgOn)
                query.AddParameterWithTypeValue("DISPCLASS", OracleDbType.Varchar2, _
                        DispClassBroadcast)

                ' SQLの実行
                dt = query.GetData()

            End Using

            Logger.Info("GetVisitBroadcast_End Ret[NotDealVisitDataTable[" & dt.TableName & "[Count = " & dt.Count & "]]")

            ' 検索結果返却
            Return dt

        End Function

        ''' <summary>
        ''' セールススタッフ指定対応来店実績一覧を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="dealAccount">対応担当アカウント</param>
        ''' <param name="visitTimestampStart">来店日時開始</param>
        ''' <param name="visitTimestampEnd">来店日時終了</param>
        ''' <returns>セールススタッフ指定来店実績データセット</returns>
        ''' <remarks></remarks>
        Public Function GetVisitStaffSpecify(ByVal dealerCode As String, _
                                             ByVal storeCode As String, _
                                             ByVal dealAccount As String, _
                                             ByVal visitTimestampStart As Date, _
                                             ByVal visitTimestampEnd As Date) As SC3100201DataSet.NotDealVisitDataTable

            Dim startSb As New StringBuilder
            startSb.Append(dealerCode).Append(", ")
            startSb.Append(storeCode).Append(", ")
            startSb.Append(dealAccount).Append(", ")
            startSb.Append(visitTimestampStart).Append(", ")
            startSb.Append(visitTimestampEnd)
            Logger.Info("GetVisitStaffSpecify_Start Param[" & startSb.ToString() & "]")
            startSb = Nothing

            ' セールススタッフ指定来店実績
            Dim dt As SC3100201DataSet.NotDealVisitDataTable = Nothing

            Using query As New DBSelectQuery( _
                    Of SC3100201DataSet.NotDealVisitDataTable)("SC3100201_002")

                ' SQL文作成
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3100201_002 */")
                    .Append("        VS.VISITSEQ")
                    .Append("      , VS.VISITSTATUS")
                    .Append("      , VS.VISITTIMESTAMP")
                    .Append("      , VS.CUSTSEGMENT")
                    .Append("      , VS.CUSTID")
                    .Append("      , VS.TENTATIVENAME")
                    .Append("      , VS.VCLREGNO")
                    .Append("      , VS.VISITMEANS")
                    .Append("      , VS.VISITPERSONNUM")
                    .Append("      , VS.SALESTABLENO")
                    .Append("      , VS.STAFFCD AS CUSTSTAFFCD")
                    .Append("      , US.USERNAME AS CUSTSTAFFNAME")
                    .Append("      , VS.ACCOUNT AS DEALSTAFFCD")
                    .Append("      , UA.USERNAME AS DEALSTAFFNAME")
                    .Append("      , UA.ORG_IMGFILE AS DEALSTAFFIMAGE")
                    .Append("      , :DISPCLASS AS DISPCLASS")
                    .Append("      , TO_CHAR(VS.UPDATEDATE ,'YYYY/MM/DD HH24:MI:SS') AS UPDATEDATE")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("      , VS.STOPTIME AS STOPTIME")
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append("   FROM TBL_VISIT_SALES VS")
                    .Append("      , TBL_USERS US")
                    .Append("      , TBL_USERS UA")
                    .Append("  WHERE VS.STAFFCD = US.ACCOUNT(+)")
                    .Append("    AND VS.ACCOUNT = UA.ACCOUNT(+)")
                    .Append("    AND VS.DLRCD = :DLRCD")
                    .Append("    AND VS.STRCD = :STRCD")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("    AND NVL(VS.STOPTIME, VS.VISITTIMESTAMP) BETWEEN :VISITTIMESTAMP_START")
                    .Append("                                                AND :VISITTIMESTAMP_END")
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append("    AND VS.ACCOUNT = :ACCOUNT")
                    .Append("    AND VS.VISITSTATUS IN (:VISITSTATUS_ADJUST")
                    .Append("                        ,  :VISITSTATUS_DIFINITION_BROUD")
                    .Append("                        ,  :VISITSTATUS_DIFINITION")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("                        ,  :VISITSTATUS_WAIT")
                    .Append("                        ,  :VISITSTATUS_SALES_STOP)")
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append("    AND US.DELFLG(+) = :US_DELFLG")
                    .Append("    AND UA.DELFLG(+) = :UA_DELFLG")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("  ORDER BY NVL(VS.STOPTIME, VS.VISITTIMESTAMP) ASC")
                    ' $01 end   複数顧客に対する商談平行対応
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("VISITTIMESTAMP_START", OracleDbType.Date, _
                        visitTimestampStart)
                query.AddParameterWithTypeValue("VISITTIMESTAMP_END", OracleDbType.Date, _
                        visitTimestampEnd)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, dealAccount)
                query.AddParameterWithTypeValue("VISITSTATUS_ADJUST", OracleDbType.Char, _
                        VisitStatusAdjust)
                query.AddParameterWithTypeValue("VISITSTATUS_DIFINITION_BROUD", OracleDbType.Char, _
                        VisitStatusDefinitionBroud)
                query.AddParameterWithTypeValue("VISITSTATUS_DIFINITION", OracleDbType.Char, _
                        VisitStatusDefinition)
                query.AddParameterWithTypeValue("VISITSTATUS_WAIT", OracleDbType.Char, _
                        VisitStatusWait)
                ' $01 start 複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("VISITSTATUS_SALES_STOP", OracleDbType.Char, _
                        VisitStatusNegotiateStop)
                ' $01 end   複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("US_DELFLG", OracleDbType.Char, DelflgOn)
                query.AddParameterWithTypeValue("UA_DELFLG", OracleDbType.Char, DelflgOn)
                query.AddParameterWithTypeValue("DISPCLASS", OracleDbType.Varchar2, _
                        DispClassStaffSpecify)

                ' SQLの実行
                dt = query.GetData()

            End Using

            Logger.Info("GetVisitStaffSpecify_End Ret[" & dt.TableName & "[Count = " & dt.Count & "]]")

            ' 検索結果返却
            Return dt

        End Function

#End Region

#Region "参照情報一覧"

        ''' <summary>
        ''' 顧客担当来店実績一覧を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="customerAccount">顧客担当アカウント</param>
        ''' <param name="visitTimestampStart">来店日時開始</param>
        ''' <param name="visitTimestampEnd">来店日時終了</param>
        ''' <returns>顧客担当来店実績データセット</returns>
        ''' <remarks></remarks>
        Public Function GetVisitCustomerStaff( _
                ByVal dealerCode As String, ByVal storeCode As String, _
                ByVal customerAccount As String, ByVal visitTimestampStart As Date, _
                ByVal visitTimestampEnd As Date) As SC3100201DataSet.NotDealVisitDataTable

            Dim startSb As New StringBuilder
            startSb.Append(dealerCode).Append(", ")
            startSb.Append(storeCode).Append(", ")
            startSb.Append(customerAccount).Append(", ")
            startSb.Append(visitTimestampStart).Append(", ")
            startSb.Append(visitTimestampEnd)
            Logger.Info("GetVisitCustomerStaff_Start Param[" & startSb.ToString() & "]")
            startSb = Nothing

            ' 顧客担当来店実績
            Dim dt As SC3100201DataSet.NotDealVisitDataTable = Nothing

            Using query As New DBSelectQuery( _
                    Of SC3100201DataSet.NotDealVisitDataTable)("SC3100201_003")

                ' SQL文作成
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3100201_003 */")
                    .Append("        VS.VISITSEQ")
                    .Append("      , VS.VISITSTATUS")
                    .Append("      , VS.VISITTIMESTAMP")
                    .Append("      , VS.CUSTSEGMENT")
                    .Append("      , VS.CUSTID")
                    .Append("      , VS.TENTATIVENAME")
                    .Append("      , VS.VCLREGNO")
                    .Append("      , VS.VISITMEANS")
                    .Append("      , VS.VISITPERSONNUM")
                    .Append("      , VS.SALESTABLENO")
                    .Append("      , VS.STAFFCD AS CUSTSTAFFCD")
                    .Append("      , US.USERNAME AS CUSTSTAFFNAME")
                    .Append("      , VS.ACCOUNT AS DEALSTAFFCD")
                    .Append("      , UA.USERNAME AS DEALSTAFFNAME")
                    .Append("      , UA.ORG_IMGFILE AS DEALSTAFFIMAGE")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("      , VS.STOPTIME AS STOPTIME")
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append("   FROM TBL_VISIT_SALES VS")
                    .Append("      , TBL_USERS US")
                    .Append("      , TBL_USERS UA")
                    .Append("  WHERE VS.STAFFCD = US.ACCOUNT(+)")
                    .Append("    AND VS.ACCOUNT = UA.ACCOUNT(+)")
                    .Append("    AND VS.DLRCD = :DLRCD")
                    .Append("    AND VS.STRCD = :STRCD")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("    AND NVL(VS.STOPTIME, VS.VISITTIMESTAMP) BETWEEN :VISITTIMESTAMP_START")
                    .Append("                                                AND :VISITTIMESTAMP_END")
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append("    AND VS.STAFFCD = :STAFFCD")
                    .Append("    AND ((VS.ACCOUNT IS NULL")
                    .Append("    AND VS.VISITSTATUS = :VISITSTATUS_FREE)")
                    .Append("     OR (VS.ACCOUNT <> VS.STAFFCD")
                    .Append("    AND VS.VISITSTATUS IN (:VISITSTATUS_ADJUST")
                    .Append("                         , :VISITSTATUS_DIFINITION")
                    .Append("                         , :VISITSTATUS_WAIT")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("                         , :VISITSTATUS_SALES_START")
                    .Append("                         , :VISITSTATUS_SALES_STOP")
                    ' $02 start 納車作業ステータス対応
                    .Append("                         , :VISITSTATUS_DELIVERLY_START)))")
                    ' $02 end 納車作業ステータス対応
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append("    AND US.DELFLG(+) = :US_DELFLG")
                    .Append("    AND UA.DELFLG(+) = :UA_DELFLG")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("  ORDER BY NVL(VS.STOPTIME, VS.VISITTIMESTAMP) ASC")
                    ' $01 end   複数顧客に対する商談平行対応
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("VISITTIMESTAMP_START", OracleDbType.Date, _
                        visitTimestampStart)
                query.AddParameterWithTypeValue("VISITTIMESTAMP_END", OracleDbType.Date, _
                        visitTimestampEnd)
                query.AddParameterWithTypeValue("STAFFCD", OracleDbType.Varchar2, customerAccount)
                query.AddParameterWithTypeValue("VISITSTATUS_FREE", OracleDbType.Char, _
                        VisitStatusFree)
                query.AddParameterWithTypeValue("VISITSTATUS_ADJUST", OracleDbType.Char, _
                        VisitStatusAdjust)
                query.AddParameterWithTypeValue("VISITSTATUS_DIFINITION", OracleDbType.Char, _
                        VisitStatusDefinition)
                query.AddParameterWithTypeValue("VISITSTATUS_WAIT", OracleDbType.Char, _
                        VisitStatusWait)
                query.AddParameterWithTypeValue("VISITSTATUS_SALES_START", OracleDbType.Char, _
                        VisitStatusSalesStart)
                ' $01 start 複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("VISITSTATUS_SALES_STOP", OracleDbType.Char, _
                        VisitStatusNegotiateStop)
                ' $01 end   複数顧客に対する商談平行対応
                ' $02 start 納車作業ステータス対応
                query.AddParameterWithTypeValue("VISITSTATUS_DELIVERLY_START", OracleDbType.Char, _
                        VisitStatusDeliverlyStart)
                ' $02 end 納車作業ステータス対応
                query.AddParameterWithTypeValue("US_DELFLG", OracleDbType.Char, DelflgOn)
                query.AddParameterWithTypeValue("UA_DELFLG", OracleDbType.Char, DelflgOn)

                ' SQLの実行
                dt = query.GetData()

            End Using

            Logger.Info("GetVisitCustomerStaff_End Ret[" & dt.TableName & "[Count = " & dt.Count & "]]")

            ' 検索結果返却
            Return dt

        End Function

        ''' <summary>
        ''' 案内通知来店実績一覧を取得する。
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="toAccount">送信先アカウント</param>
        ''' <param name="visitTimestampStart">来店日時開始</param>
        ''' <param name="visitTimestampEnd">来店日時終了</param>
        ''' <returns>案内通知来店実績データセット</returns>
        ''' <remarks></remarks>
        Public Function GetVisitReceiveNotice( _
                ByVal dealerCode As String, ByVal storeCode As String, ByVal toAccount As String, _
                ByVal visitTimestampStart As Date, ByVal visitTimestampEnd As Date) _
                As SC3100201DataSet.NotDealVisitDataTable

            Dim startSb As New StringBuilder
            startSb.Append(dealerCode).Append(", ")
            startSb.Append(storeCode).Append(", ")
            startSb.Append(toAccount).Append(", ")
            startSb.Append(visitTimestampStart).Append(", ")
            startSb.Append(visitTimestampEnd)
            Logger.Info("GetVisitReceiveNotice_Start Param[" & startSb.ToString() & "]")
            startSb = Nothing

            ' 案内通知来店実績
            Dim dt As SC3100201DataSet.NotDealVisitDataTable = Nothing

            Using query As New DBSelectQuery( _
                    Of SC3100201DataSet.NotDealVisitDataTable)("SC3100201_004")

                ' SQL文作成
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3100201_004 */")
                    .Append("        VS.VISITSEQ")
                    .Append("      , VS.VISITSTATUS")
                    .Append("      , VS.VISITTIMESTAMP")
                    .Append("      , VS.CUSTSEGMENT")
                    .Append("      , VS.CUSTID")
                    .Append("      , VS.TENTATIVENAME")
                    .Append("      , VS.VCLREGNO")
                    .Append("      , VS.VISITMEANS")
                    .Append("      , VS.VISITPERSONNUM")
                    .Append("      , VS.SALESTABLENO")
                    .Append("      , VS.STAFFCD AS CUSTSTAFFCD")
                    .Append("      , US.USERNAME AS CUSTSTAFFNAME")
                    .Append("      , VS.ACCOUNT AS DEALSTAFFCD")
                    .Append("      , UA.USERNAME AS DEALSTAFFNAME")
                    .Append("      , UA.ORG_IMGFILE AS DEALSTAFFIMAGE")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("      , VS.STOPTIME AS STOPTIME")
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append("   FROM TBL_NOTICEINFO NI")
                    .Append("      , TBL_NOTICEREQUEST RI")
                    .Append("      , TBL_VISIT_SALES VS")
                    .Append("      , TBL_USERS US")
                    .Append("      , TBL_USERS UA")
                    .Append("  WHERE NI.NOTICEREQID = RI.NOTICEREQID")
                    .Append("    AND RI.REQCLASSID = VS.VISITSEQ")
                    .Append("    AND VS.STAFFCD = US.ACCOUNT(+)")
                    .Append("    AND VS.ACCOUNT = UA.ACCOUNT(+)")
                    .Append("    AND VS.DLRCD = :DLRCD")
                    .Append("    AND VS.STRCD = :STRCD")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("    AND NVL(VS.STOPTIME, VS.VISITTIMESTAMP) BETWEEN :VISITTIMESTAMP_START")
                    .Append("                                                AND :VISITTIMESTAMP_END")
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append("    AND NI.TOACCOUNT = :TOACCOUNT")
                    .Append("    AND NI.SENDDATE >= :VISITTIMESTAMP_START")
                    .Append("    AND NI.SENDDATE <= :VISITTIMESTAMP_END")
                    .Append("    AND RI.NOTICEREQCTG = :NOTICEREQCTG")
                    .Append("    AND VS.VISITSTATUS IN ( :VISITSTATUS_FREE")
                    .Append("                          , :VISITSTATUS_ADJUST")
                    .Append("                          , :VISITSTATUS_DIFINITION")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("                          , :VISITSTATUS_WAIT")
                    .Append("                          , :VISITSTATUS_SALES_STOP)")
                    ' $01 end   複数顧客に対する商談平行対応
                    .Append("    AND US.DELFLG(+) = :US_DELFLG")
                    .Append("    AND UA.DELFLG(+) = :UA_DELFLG")
                    ' $01 start 複数顧客に対する商談平行対応
                    .Append("  ORDER BY NVL(VS.STOPTIME, VS.VISITTIMESTAMP) ASC")
                    ' $01 end   複数顧客に対する商談平行対応
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("VISITTIMESTAMP_START", OracleDbType.Date, _
                        visitTimestampStart)
                query.AddParameterWithTypeValue("VISITTIMESTAMP_END", OracleDbType.Date, _
                        visitTimestampEnd)
                query.AddParameterWithTypeValue("TOACCOUNT", OracleDbType.Varchar2, toAccount)
                query.AddParameterWithTypeValue("NOTICEREQCTG", OracleDbType.Char, ReqClassVisit)
                query.AddParameterWithTypeValue("VISITSTATUS_FREE", OracleDbType.Char, _
                        VisitStatusFree)
                query.AddParameterWithTypeValue("VISITSTATUS_ADJUST", OracleDbType.Char, _
                        VisitStatusAdjust)
                query.AddParameterWithTypeValue("VISITSTATUS_DIFINITION", OracleDbType.Char, _
                        VisitStatusDefinition)
                query.AddParameterWithTypeValue("VISITSTATUS_WAIT", OracleDbType.Char, _
                        VisitStatusWait)
                ' $01 start 複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("VISITSTATUS_SALES_STOP", OracleDbType.Char, _
                        VisitStatusNegotiateStop)
                ' $01 end   複数顧客に対する商談平行対応
                query.AddParameterWithTypeValue("US_DELFLG", OracleDbType.Char, DelflgOn)
                query.AddParameterWithTypeValue("UA_DELFLG", OracleDbType.Char, DelflgOn)

                ' SQLの実行
                dt = query.GetData()

            End Using

            Logger.Info("GetVisitReceiveNotice_End Ret[" & dt.TableName & "[Count = " & dt.Count & "]]")

            ' 検索結果返却
            Return dt

        End Function

#End Region

#Region "個人情報取得"

        ''' <summary>
        ''' 自社客個人情報を取得する。
        ''' </summary>
        ''' <param name="customerId">自社客連番</param>
        ''' <returns>自社客個人情報データセット</returns>
        ''' <remarks></remarks>
        Public Function GetOrgCustomer( _
                ByVal customerId As String) As SC3100201DataSet.CustomerDataTable

            Logger.Info("GetOrgCustomer_Start Param[" & customerId & "]")

            ' 自社客個人情報
            Dim dt As SC3100201DataSet.CustomerDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3100201DataSet.CustomerDataTable)("SC3100201_005")

                ' SQL文作成
                Dim sql As New StringBuilder
                With sql
                    ' $03 start
                    .Append("SELECT /* SC3100201_005 */")
                    .Append("        MC.CST_NAME AS NAME")
                    .Append("      , MC.NAMETITLE_NAME AS NAMETITLE")
                    .Append("      , MCD.IMG_FILE_SMALL AS IMAGEFILE")
                    .Append("   FROM TB_M_CUSTOMER MC")
                    .Append("      , TB_M_CUSTOMER_DLR MCD")
                    .Append("  WHERE MC.CST_ID = MCD.CST_ID(+)")
                    .Append("    AND MC.CST_ID = :ORIGINALID")
                    ' $03 end
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数
                ' $03 start 桁数変更対応
                query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, customerId)
                ' $03 end
                ' SQLの実行
                dt = query.GetData()

            End Using

            Logger.Info("GetOrgCustomer_End Ret[" & dt.TableName & "[Count = " & dt.Count & "]]")

            ' 検索結果返却
            Return dt

        End Function

        ''' <summary>
        ''' 未取引客個人情報を取得する。
        ''' </summary>
        ''' <param name="customerId">未取引客ユーザID</param>
        ''' <returns>未取引客個人情報データセット</returns>
        ''' <remarks></remarks>
        Public Function GetNewCustomer( _
                ByVal customerId As String) As SC3100201DataSet.CustomerDataTable

            Logger.Info("GetNewCustomer_Start Param[" & customerId & "]")

            ' 未取引客個人情報
            Dim dt As SC3100201DataSet.CustomerDataTable = Nothing

            Using query As New DBSelectQuery(Of SC3100201DataSet.CustomerDataTable)("SC3100201_006")

                ' SQL文作成
                Dim sql As New StringBuilder
                With sql
                    ' $03 start
                    .Append(" SELECT /* SC3100201_006 */")
                    .Append("        MC.CST_NAME AS NAME")
                    .Append("      , MC.NAMETITLE_NAME AS NAMETITLE")
                    .Append("      , MCD.IMG_FILE_SMALL AS IMAGEFILE")
                    .Append("   FROM TB_M_CUSTOMER MC")
                    .Append("      , TB_M_CUSTOMER_DLR MCD")
                    .Append("  WHERE MC.CST_ID = MCD.CST_ID(+)")
                    .Append("    AND MC.CST_ID = :CSTID")
                    '$03 end
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数
                ' $03 start 桁数変更対応
                query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, customerId)
                '$03 end 桁数変更対応

                ' SQLの実行
                dt = query.GetData()

            End Using

            Logger.Info("GetNewCustomer_End Ret[NotDealVisitDataTable[Count = " & dt.Count & "]]")

            ' 検索結果返却
            Return dt

        End Function

#End Region

#Region "来店客の対応処理"

        ''' <summary>
        ''' 来店実績情報を取得する。
        ''' </summary>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <returns>来店実績</returns>
        ''' <remarks></remarks>
        Public Function GetVisit(ByVal visitSeq As Long) As SC3100201DataSet.VisitSalesDataTable

            Logger.Info("GetVisit_Start Param[" & visitSeq & "]")

            ' 来店実績
            Dim dt As SC3100201DataSet.VisitSalesDataTable = Nothing

            Using query As New DBSelectQuery( _
                    Of SC3100201DataSet.VisitSalesDataTable)("SC3100201_007")

                ' SQL文作成
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3100201_007 */")
                    .Append("        VISITSEQ")
                    .Append("      , VISITSTATUS")
                    .Append("      , ACCOUNT AS DEALSTAFFCD")
                    .Append("      , CUSTSEGMENT")
                    .Append("      , :CUSTCLASS AS CUSTCLASS")
                    .Append("      , CUSTID")
                    .Append("   FROM TBL_VISIT_SALES")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数
                query.AddParameterWithTypeValue("CUSTCLASS", OracleDbType.Char, CustomerClassOwner)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSeq)

                ' SQLの実行
                dt = query.GetData()

            End Using

            Logger.Info("GetVisit_End Ret[NotDealVisitDataTable[Count = " & dt.Count & "]]")

            ' 検索結果返却
            Return dt

        End Function

        ''' <summary>
        ''' 対応依頼通知の存在有無を取得する。
        ''' </summary>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <param name="account">アカウント</param>
        ''' <returns>存在有無</returns>
        ''' <remarks></remarks>
        Public Function ExistsVisitDealRequestNotice( _
                ByVal visitSeq As Long, ByVal account As String) As Boolean

            Dim startSb As New StringBuilder
            startSb.Append(visitSeq).Append(", ")
            startSb.Append(account)
            Logger.Info("ExistsVisitDealRequestNotice_Start Param[" & startSb.ToString() & "]")
            startSb = Nothing

            ' 存在有無
            Dim isExists As Boolean = False

            Using query As New DBSelectQuery( _
                    Of SC3100201DataSet.VisitSalesDataTable)("SC3100201_008")

                ' SQL文作成
                Dim sql As New StringBuilder
                With sql
                    .Append(" SELECT /* SC3100201_008 */")
                    .Append("        VISITSEQ")
                    .Append("   FROM TBL_VISITDEAL_NOTICE")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                    .Append("    AND ACCOUNT = :ACCOUNT")
                    .Append("    AND DELFLG = :DELFLG")
                    .Append("    AND ROWNUM <= 1")
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSeq)
                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, account)
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DelflgOn)

                ' SQLの実行
                Using dt As SC3100201DataSet.VisitSalesDataTable = query.GetData()

                    ' レコードが取得できた場合
                    If 0 < dt.Rows.Count Then
                        Logger.Info("ExistsVisitDealRequestNotice_001")

                        ' 存在する
                        isExists = True

                    End If

                End Using

            End Using

            Logger.Info("ExistsVisitDealRequestNotice_End Ret[" & isExists & "]")

            ' 検索結果返却
            Return isExists

        End Function

        ''' <summary>
        ''' 来店実績データの来店客対応更新を実施する。
        ''' </summary>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <param name="visitStatus">来店実績ステータス</param>
        ''' <param name="isUpdateDealAccount">対応担当アカウントの更新有無</param>
        ''' <param name="dealAccount">対応担当アカウント</param>
        ''' <param name="updateDate">取得更新日時</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <param name="updateId">更新機能ID</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function UpdateVisit(ByVal visitSeq As Long, _
                                    ByVal visitStatus As String, _
                                    ByVal isUpdateDealAccount As Boolean, _
                                    ByVal dealAccount As String, _
                                    ByVal updateDate As String, _
                                    ByVal updateAccount As String, _
                                    ByVal updateId As String) As Boolean

            Dim startSb As New StringBuilder
            startSb.Append(visitSeq).Append(", ")
            startSb.Append(visitStatus).Append(", ")
            startSb.Append(isUpdateDealAccount).Append(", ")
            startSb.Append(dealAccount).Append(", ")
            startSb.Append(updateDate).Append(", ")
            startSb.Append(updateAccount).Append(", ")
            startSb.Append(updateId)
            Logger.Info("UpdateVisit_Start Param[" & startSb.ToString() & "]")
            startSb = Nothing

            ' 更新対象レコード件数
            Dim record As Integer = 0

            Using query As New DBUpdateQuery("SC3100201_009")

                ' SQL文作成
                Dim sql As New StringBuilder
                With sql
                    .Append(" UPDATE /* SC3100201_009 */")
                    .Append("        TBL_VISIT_SALES")
                    .Append("    SET VISITSTATUS = :VISITSTATUS")
                End With

                If isUpdateDealAccount Then
                    Logger.Info("UpdateVisit_001")
                    sql.Append("      , ACCOUNT = :ACCOUNT")
                End If

                With sql
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEID = :UPDATEID")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                    .Append("    AND UPDATEDATE = :UPDATEDATE")
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数
                query.AddParameterWithTypeValue("VISITSTATUS", OracleDbType.Char, visitStatus)
                If isUpdateDealAccount Then
                    Logger.Info("UpdateVisit_002")
                    query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, dealAccount)
                End If
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, _
                        updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSeq)
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, DateTime.Parse(updateDate, CultureInfo.InvariantCulture()))                    '更新日

                ' SQLの実行
                record = query.Execute()

            End Using

            ' 処理結果
            Dim isSuccess As Boolean = False

            ' 実行結果が0件超過の場合
            If 0 < record Then
                Logger.Info("UpdateVisit_001")

                ' 処理結果に成功を設定
                isSuccess = True
            End If

            Logger.Info("UpdateVisit_End Ret[" & isSuccess & "]")

            ' 戻り値に処理結果を設定
            Return isSuccess

        End Function

        ''' <summary>
        ''' 対応依頼通知データの論理削除更新を実施する。
        ''' </summary>
        ''' <param name="visitSeq">来店実績連番</param>
        ''' <param name="updateAccount">更新アカウント</param>
        ''' <param name="updateId">更新機能ID</param>
        ''' <returns>処理結果</returns>
        ''' <remarks></remarks>
        Public Function UpdateVisitDealRequestNotice( _
                ByVal visitSeq As Long, ByVal updateAccount As String, ByVal updateId As String) _
                As Boolean

            Dim startSb As New StringBuilder
            startSb.Append(visitSeq).Append(", ")
            startSb.Append(updateAccount).Append(", ")
            startSb.Append(updateId)
            Logger.Info("UpdateVisitDealRequestNotice_Start Param[" & startSb.ToString() & "]")
            startSb = Nothing

            Using query As New DBUpdateQuery("SC3100201_010")

                ' SQL文作成
                Dim sql As New StringBuilder
                With sql
                    .Append(" UPDATE /* SC3100201_010 */")
                    .Append("        TBL_VISITDEAL_NOTICE")
                    .Append("    SET DELFLG = :DELFLG")
                    .Append("      , UPDATEDATE = SYSDATE")
                    .Append("      , UPDATEACCOUNT = :UPDATEACCOUNT")
                    .Append("      , UPDATEID = :UPDATEID")
                    .Append("  WHERE VISITSEQ = :VISITSEQ")
                End With

                query.CommandText = sql.ToString()
                sql = Nothing

                ' バインド変数
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DelflgOff)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, _
                        updateAccount)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, updateId)
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, visitSeq)

                ' SQLの実行
                query.Execute()

            End Using

            Logger.Info("UpdateVisitDealRequestNotice_End Ret[True]")

            ' 戻り値に処理結果を設定
            Return True

        End Function

#End Region

    End Class

End Namespace

Partial Public Class SC3100201DataSet
End Class

