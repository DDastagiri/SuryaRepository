'-------------------------------------------------------------------------
'IC3810701DateSet.vb
'-------------------------------------------------------------------------
'機能：サービス標準LT取得API
'補足：
'作成：2012/05/11 KN 河原 【servive_2】
'更新：2012/08/28 TMEJ 河原  【SERVICE_2】標準時間取得項目(部品引取待標準時間・追加部品見積もり標準時間)を追加
'更新：2012/09/19 TMEJ 日比野【SERVICE_2】標準時間取得項目(来店工程標準時間)を追加
'更新： 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新：2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一
'更新：2019/10/31 NSK 鈴木【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット

Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace IC3810701DataSetTableAdapters

    ''' <summary>
    ''' サービス標準LT取得APIデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class IC3810701TableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "メソッド"

        ''' <summary>
        ''' サービス標準LT取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <returns>サービス標準LT情報</returns>
        ''' <remarks></remarks>
        ''' <hisitory>
        ''' 2012/08/28 TMEJ 河原 【SERVICE_2】標準時間取得項目(部品引取待標準時間・追加部品見積もり標準時間)を追加
        ''' 2012/09/19 TMEJ 日比野【SERVICE_2】標準時間取得項目(来店工程標準時間)を追加
        ''' 2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' 2019/10/31 NSK 鈴木 【（FS）MaaSビジネス向けサービス予約の登録オペレーションの効率化に向けた試験研究】[No156]予定入庫日時、予定納車日時の自動セット
        ''' </hisitory>
        Public Function GetDBStandardLTList(ByVal inDealerCode As String, _
                                            ByVal inStoreCode As String) _
                                            As IC3810701DataSet.StandardLTListDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                     , "{0}.{1} P1:{2} P2:{3} " _
                                     , Me.GetType.ToString _
                                     , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                     , inDealerCode, inStoreCode))

            Using query As New DBSelectQuery(Of IC3810701DataSet.StandardLTListDataTable)("IC3810701_001")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("   SELECT  /* IC3810701_001 */")

                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
                    '.AppendLine("            T1.RECEPT_STANDARD_LT ")
                    '.AppendLine("           ,T1.ADDWORK_STANDARD_LT ")

                    .AppendLine("            T1.ADDWORK_STANDARD_LT ")
                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
                    '.AppendLine("           ,T1.DELIVERYPRE_STANDARD_LT ")
                    '.AppendLine("           ,T1.DELIVERYWR_STANDARD_LT ")
                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

                    .AppendLine("           ,T1.PARTS_STANDARD_LT ")

                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
                    '.AppendLine("           ,T2.WASHTIME ")
                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

                    ' 2012/08/28 TMEJ 河原 【SERVICE_2】標準時間取得項目(部品引取待標準時間・追加部品見積もり標準時間)を追加 START
                    .AppendLine("           ,T1.PARTS_WAITING_STANDARD_LT ")        '部品引取待標準時間
                    .AppendLine("           ,T1.ADDPARTS_ESTIMATE_STANDARD_LT ")    '追加部品見積もり標準時間
                    ' 2012/08/28 TMEJ 河原 【SERVICE_2】標準時間取得項目(部品引取待標準時間・追加部品見積もり標準時間)を追加 END

                    ' 2012/09/19 TMEJ 日比野【SERVICE_2】標準時間取得項目(来店工程標準時間)を追加 START
                    .AppendLine("           ,T1.RECEPT_GUIDE_STANDARD_LT ")
                    ' 2012/09/19 TMEJ 日比野【SERVICE_2】標準時間取得項目(来店工程標準時間)を追加 END

                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    .AppendLine("           ,T3.STD_INSPECTION_TIME")
                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
                    .AppendLine("           ,T3.STD_CARWASH_TIME AS WASHTIME")
                    .AppendLine("           ,T3.STD_ACCEPTANCE_TIME AS RECEPT_STANDARD_LT")
                    .AppendLine("           ,T3.STD_DELI_PREPARATION_TIME AS DELIVERYPRE_STANDARD_LT")
                    .AppendLine("           ,T3.STD_DELI_TIME AS DELIVERYWR_STANDARD_LT")
                    .AppendLine("           ,T3.SCHE_SVCIN_DELI_AUTO_DISP_FLG")
                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

                    .AppendLine("    FROM       TBL_SERVICEINI T1")

                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
                    '.AppendLine("              ,TBL_STALLCTL T2 ")
                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                    .AppendLine("              ,TB_M_SERVICEIN_SETTING T3 ")
                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                    .AppendLine("   WHERE ")

                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
                    '.AppendLine("                   T1.DLRCD = T2.DLRCD(+) ")
                    '.AppendLine("     AND           T1.STRCD = T2.STRCD(+) ")
                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START

                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット START
                    ' .AppendLine("     AND           T1.DLRCD = T3.DLR_CD(+) ")
                    .AppendLine("                   T1.DLRCD = T3.DLR_CD(+) ")
                    ' 2019/10/31 NSK 鈴木 [No156]予定入庫日時、予定納車日時の自動セット END

                    .AppendLine("     AND           T1.STRCD = T3.BRN_CD(+) ")
                    '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
                    .AppendLine("     AND           T1.DLRCD = :DLRCD ")
                    .AppendLine("     AND           T1.STRCD = :STRCD ")
                End With

                query.CommandText = sql.ToString()

                '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inStoreCode)

                '2013/07/05 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                Dim dt As IC3810701DataSet.StandardLTListDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} QUERY:COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))
                Return dt
            End Using
        End Function

#End Region

    End Class
End Namespace

Partial Class IC3810701DataSet
End Class
