'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240501DataSet.vb
'─────────────────────────────────────
'機能： 新規予約作成
'補足： 
'作成： 2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
'更新： 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新： 2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001
'更新： 2018/04/19 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports System.Globalization
Imports System.Reflection

Namespace SC3240501DataSetTableAdapters
    Public Class SC3240501DataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"
        ''' <summary>
        ''' 自画面のプログラムID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const NEWCHIP_PROGRAMID As String = "SC3240501"

        ''' <summary>
        ''' Log開始用文言
        ''' </summary>
        ''' <remarks></remarks>
        Private Const LOG_START As String = "Start"

        ''' <summary>
        ''' Log終了文言
        ''' </summary>
        ''' <remarks></remarks>
        Private Const LOG_END As String = "End"

        ''' <summary>
        ''' エラー:DBタイムアウト
        ''' </summary>
        ''' <remarks></remarks>
        Private Const C_RET_DBTIMEOUT As Long = 909   '901

        ''' <summary>
        ''' 使用中フラグ（1：使用中）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const INUSE_TYPE_USE As String = "1"

        ''' <summary>
        ''' あいまい検索用
        ''' </summary>
        ''' <remarks></remarks>
        Private Const LikeWord As String = "%"

        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        ''' <summary>
        ''' 顧客車両区分(1：Owner)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VehicleOwner As String = "1"

        ''' <summary>
        ''' 顧客車両区分(2：User)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VehicleUser As String = "2"

        ''' <summary>
        ''' オーナーチェンジフラグ(0：未設定)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const NoChange As String = "0"


        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END


#End Region

#Region "顧客数取得"
        ''' <summary>
        ''' 顧客数を取得する
        ''' </summary>
        ''' <param name="inDealerCd">販売店コード</param>
        ''' <param name="inStoreCd">店舗コード</param>
        ''' <param name="inRegisterNo">登録No.</param>
        ''' <param name="inVinNo">VIN</param>
        ''' <param name="inCustomerName">名前</param>
        ''' <param name="inTelNo">TEL</param>
        ''' <returns></returns>
        ''' <remarks>
        ''' 後方一致検索はSQLコストが高く遅いため、
        ''' 後方一致検索の場合はリバースカラムに前方一致で当てる
        ''' </remarks>
        Public Function GetCustomerCount(ByVal inDealerCD As String,
                                         ByVal inStoreCD As String,
                                         ByVal inRegisterNo As String,
                                         ByVal inVinNo As String,
                                         ByVal inCustomerName As String,
                                         ByVal inTelNo As String) As Long

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                          "[inDealerCD:{0}][inStoreCD:{1}][inRegisterNo:{2}][inVinNo:{3}][inCustomerName:{4}][inTelNo:{5}]", _
                          inDealerCD, _
                          inStoreCD, _
                          inRegisterNo, _
                          inVinNo, _
                          inCustomerName, _
                          inTelNo)

            Dim cnt As Long = 0

            '検索条件の判定用（0:登録No.／1:VIN／2:名前／3:TEL）
            Dim searchType As Integer = 0

            If Not String.IsNullOrEmpty(inRegisterNo) Then
                '「登録No.」が条件として入力されている場合
                searchType = 0

            ElseIf Not String.IsNullOrEmpty(inVinNo) Then
                '「VIN」が条件として入力されている場合
                searchType = 1

            ElseIf Not String.IsNullOrEmpty(inCustomerName) Then
                '「名前」が条件として入力されている場合
                searchType = 2

            ElseIf Not String.IsNullOrEmpty(inTelNo) Then
                '「TEL」が条件として入力されている場合
                searchType = 3
            End If

            Try
                Using query As New DBSelectQuery(Of SC3240501DataSet.SC3240501CustomerCountDataTable)("SC3240501_001")
                    Dim sql As New StringBuilder
                    With sql
                        .Append(" SELECT /* SC3240501_001 */ ")
                        .Append(" 	   COUNT(1) AS CNT ")
                        .Append("   FROM ( ")
                        .Append("     SELECT DISTINCT ")
                        .Append(" 	       MT1.CST_ID ")
                        .Append(" 	     , MT1.VCL_ID ")
                        '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 START
                        '同一顧客ID・車両IDで顧客車両区分違いを検索できるようにするため、同一顧客ID・車両IDで顧客車両区分が違う場合を別個にカウント
                        .Append(" 	     , MT1.CST_VCL_TYPE ")
                        '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 END
                        .Append("       FROM ( ")
                        .Append("         SELECT ")
                        .Append(" 	           CV1.CST_ID ")
                        .Append(" 		     , CV1.VCL_ID ")
                        '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 START
                        '同一顧客ID・車両IDで顧客車両区分違いを検索できるようにするため、同一顧客ID・車両IDで顧客車両区分が違う場合を別個にカウント
                        .Append(" 		     , CV1.CST_VCL_TYPE ")
                        '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 END
                        .Append(" 	      FROM ")
                        .Append(" 		  	   TB_M_CUSTOMER_VCL CV1 ")
                        .Append(" 		     , TB_M_CUSTOMER C1 ")
                        .Append(" 		     , TB_M_VEHICLE_DLR VD1 ")
                        .Append(" 	  	     , TB_M_VEHICLE V1 ")
                        .Append(" 		     , TB_M_CUSTOMER_DLR CD1 ")
                        .Append(" 	     WHERE ")
                        .Append(" 			   CV1.CST_ID = C1.CST_ID ")
                        .Append(" 	       AND CV1.DLR_CD = VD1.DLR_CD ")
                        .Append(" 	       AND CV1.VCL_ID = VD1.VCL_ID ")
                        .Append(" 	       AND CV1.VCL_ID = V1.VCL_ID ")
                        .Append(" 	       AND CV1.DLR_CD = CD1.DLR_CD ")
                        .Append(" 	       AND CV1.CST_ID = CD1.CST_ID ")
                        .Append(" 	       AND CV1.DLR_CD = :DLR_CD ")
                        .Append(" 	       AND VD1.DLR_CD = :DLR_CD ")
                        .Append(" 	       AND CD1.DLR_CD = :DLR_CD ")
                        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                        '.Append(" 	       AND CV1.CST_VCL_TYPE <> N'4' ")
                        .Append(" 	       AND CV1.CST_VCL_TYPE IN(:CST_VCL_TYPE_1,:CST_VCL_TYPE_2) ")
                        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                        .Append(" 	       AND CV1.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")

                        If searchType = 0 Then
                            '検索条件：登録No.（後方一致）
                            '.Append(" AND VD1.REG_NUM_SEARCH_REV LIKE UPPER(:REG_NUM_SEARCH_REV) ")

                            ''リバースカラムに前方一致で当てるために文字列を反転
                            'Dim registerNo As String = StrReverse(inRegisterNo.Trim()) & LikeWord
                            'query.AddParameterWithTypeValue("REG_NUM_SEARCH_REV", OracleDbType.NVarchar2, registerNo)

                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 START 
                            '.Append(" 	             AND VD1.REG_NUM_SEARCH_REV LIKE UPPER('")
                            '.Append(StrReverse(inRegisterNo.Trim()) & LikeWord)
                            '.Append("') ")
                            .Append(" 	             AND VD1.REG_NUM_SEARCH_REV LIKE UPPER(:REG_NUM) ")
                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 END

                        ElseIf searchType = 1 Then
                            '検索条件：VIN（後方一致）
                            '.Append(" AND V1.VCL_VIN_SEARCH_REV LIKE UPPER(:REG_NUM_SEARCH_REV) ")

                            ''リバースカラムに前方一致で当てるために文字列を反転
                            'Dim vinNo As String = StrReverse(inVinNo.Trim()) & LikeWord
                            'query.AddParameterWithTypeValue("REG_NUM_SEARCH_REV", OracleDbType.NVarchar2, vinNo)

                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 START 
                            '.Append(" 	             AND V1.VCL_VIN_SEARCH_REV LIKE UPPER('")
                            '.Append(StrReverse(inVinNo.Trim()) & LikeWord)
                            '.Append("') ")
                            .Append(" 	             AND V1.VCL_VIN_SEARCH_REV LIKE UPPER(:VCL_VIN) ")
                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 END

                        ElseIf searchType = 2 Then
                            '検索条件：名前（前方一致）
                            '.Append(" AND C1.CST_NAME_SEARCH LIKE UPPER(:CST_NAME_SEARCH) ")

                            'Dim customerName As String = inCustomerName.Trim() & LikeWord
                            'query.AddParameterWithTypeValue(":CST_NAME_SEARCH", OracleDbType.NVarchar2, customerName)

                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 START 
                            '.Append(" 	             AND C1.CST_NAME_SEARCH LIKE UPPER('")
                            '.Append(inCustomerName.Trim() & LikeWord)
                            '.Append("') ")
                            .Append(" 	             AND C1.CST_NAME_SEARCH LIKE UPPER(:CST_NAME) ")
                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 END

                        ElseIf searchType = 3 Then
                            '.Append(" AND C1.CST_PHONE_SEARCH = :TEL ")

                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 START 
                            '.Append(" AND C1.CST_PHONE_SEARCH = '")
                            '.Append(inTelNo.Trim().Replace("-", ""))
                            '.Append("'")
                            .Append(" AND C1.CST_PHONE_SEARCH = :TEL_NO")
                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 END

                            .Append(" 	 UNION ALL ")
                            .Append(" 	    SELECT ")
                            .Append(" 			   CV2.CST_ID ")
                            .Append(" 		     , CV2.VCL_ID ")
                            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 START
                            '同一顧客ID・車両IDで顧客車両区分違いを検索できるようにするため、同一顧客ID・車両IDで顧客車両区分が違う場合を別個にカウント
                            .Append(" 		     , CV2.CST_VCL_TYPE ")
                            '2016/06/09 NSK 皆川 TR-V4-TMT-20160107-001 END
                            .Append(" 	      FROM  ")
                            .Append(" 		  	   TB_M_CUSTOMER_VCL CV2 ")
                            .Append(" 		     , TB_M_CUSTOMER C2 ")
                            .Append(" 		     , TB_M_VEHICLE_DLR VD2 ")
                            .Append(" 		     , TB_M_VEHICLE V2 ")
                            .Append(" 		     , TB_M_CUSTOMER_DLR CD2 ")
                            .Append(" 	    WHERE ")
                            .Append(" 			   CV2.CST_ID = C2.CST_ID ")
                            .Append(" 		   AND CV2.DLR_CD = VD2.DLR_CD ")
                            .Append(" 	   	   AND CV2.VCL_ID = VD2.VCL_ID ")
                            .Append(" 	   	   AND CV2.VCL_ID = V2.VCL_ID ")
                            .Append(" 	   	   AND CV2.DLR_CD = CD2.DLR_CD ")
                            .Append(" 	   	   AND CV2.CST_ID = CD2.CST_ID ")
                            .Append(" 	   	   AND CV2.DLR_CD = :DLR_CD ")
                            .Append(" 	   	   AND VD2.DLR_CD = :DLR_CD ")
                            .Append(" 	   	   AND CD2.DLR_CD = :DLR_CD ")
                            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                            '.Append(" 	   	   AND CV2.CST_VCL_TYPE <> N'4' ")
                            .Append(" 	       AND CV2.CST_VCL_TYPE IN(:CST_VCL_TYPE_1,:CST_VCL_TYPE_2) ")
                            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                            .Append(" 	   	   AND CV2.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")
                            '.Append(" 	             AND C2.CST_MOBILE_SEARCH = :TEL ")

                            'Dim tel As String = inTelNo.Trim().Replace("-", "")
                            'query.AddParameterWithTypeValue("TEL", OracleDbType.NVarchar2, tel)

                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 START 
                            '.Append(" 	   	   AND C2.CST_MOBILE_SEARCH = '")
                            '.Append(inTelNo.Trim().Replace("-", ""))
                            '.Append("'")
                            .Append(" 	   	   AND C2.CST_MOBILE_SEARCH = :TEL_NO")
                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 END

                        End If

                        .Append("            ) MT1 ")
                        .Append("         ) ")

                    End With

                    query.CommandText = sql.ToString()
                    query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCD)

                    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                    '顧客車両区分(1：所有者)
                    query.AddParameterWithTypeValue("CST_VCL_TYPE_1", OracleDbType.NVarchar2, VehicleOwner)
                    '顧客車両区分(2：使用者)
                    query.AddParameterWithTypeValue("CST_VCL_TYPE_2", OracleDbType.NVarchar2, VehicleUser)
                    'オーナーチェンジフラグ(0：未設定)
                    query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, NoChange)

                    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                    '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 START
                    Select Case searchType
                        Case 0 '登録No.
                            query.AddParameterWithTypeValue("REG_NUM", OracleDbType.NVarchar2, StrReverse(String.Concat(LikeWord, inRegisterNo.Trim())))
                        Case 1 'VIN
                            query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, StrReverse(String.Concat(LikeWord, inVinNo.Trim())))
                        Case 2 '名前
                            query.AddParameterWithTypeValue("CST_NAME", OracleDbType.NVarchar2, String.Concat(inCustomerName.Trim(), LikeWord))
                        Case 3 '電話番号
                            query.AddParameterWithTypeValue("TEL_NO", OracleDbType.NVarchar2, inTelNo.Trim().Replace("-", ""))
                    End Select
                    '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 END

                    Dim dt As SC3240501DataSet.SC3240501CustomerCountDataTable = query.GetData()
                    cnt = dt.Item(0).CNT

                End Using

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'ORACLEのタイムアウトのみ処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , C_RET_DBTIMEOUT _
                                        , ex.Message))
                Throw ex

            End Try

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", cnt)

            Return cnt

        End Function
#End Region

#Region "顧客リスト取得"
        ''' <summary>
        ''' 顧客リストを取得する
        ''' </summary>
        ''' <param name="inDealerCd">販売店コード</param>
        ''' <param name="inStoreCd">店舗コード</param>
        ''' <param name="inRegisterNo">登録No.</param>
        ''' <param name="inVinNo">VIN</param>
        ''' <param name="inCustomerName">名前</param>
        ''' <param name="inTelNo">TEL</param>
        ''' <param name="inStartRowNo">開始行</param>
        ''' <param name="inEndRowNo">終了行</param>
        ''' <returns>DataTable</returns>
        ''' <remarks>
        ''' 後方一致検索はSQLコストが高く遅いため、
        ''' 後方一致検索の場合はリバースカラムに前方一致で当てる
        ''' </remarks>
        Public Function GetCustomerList(ByVal inDealerCD As String,
                                        ByVal inStoreCD As String,
                                        ByVal inRegisterNo As String,
                                        ByVal inVinNo As String,
                                        ByVal inCustomerName As String,
                                        ByVal inTelNo As String,
                                        ByVal inStartRowNo As Long,
                                        ByVal inEndRowNo As Long) As SC3240501DataSet.SC3240501CustomerListDataTable

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, _
                      "[inDealerCD:{0}][inStoreCD:{1}][inRegisterNo:{2}][inVinNo:{3}][inCustomerName:{4}][inTelNo:{5}][inStartRowNo:{6}][inEndRowNo:{7}]", _
                      inDealerCD, inStoreCD, inRegisterNo, inVinNo, inCustomerName, inTelNo, inStartRowNo, inEndRowNo)

            '返却用データテーブル
            Dim dtResult As SC3240501DataSet.SC3240501CustomerListDataTable

            '検索条件の判定用（0:登録No.／1:VIN／2:名前／3:TEL）
            Dim searchType As Integer = 0

            If Not String.IsNullOrEmpty(inRegisterNo) Then
                '「登録No.」が条件として入力されている場合
                searchType = 0

            ElseIf Not String.IsNullOrEmpty(inVinNo) Then
                '「VIN」が条件として入力されている場合
                searchType = 1

            ElseIf Not String.IsNullOrEmpty(inCustomerName) Then
                '「名前」が条件として入力されている場合
                searchType = 2

            ElseIf Not String.IsNullOrEmpty(inTelNo) Then
                '「TEL」が条件として入力されている場合
                searchType = 3
            End If

            Try
                Using query As New DBSelectQuery(Of SC3240501DataSet.SC3240501CustomerListDataTable)("SC3240501_002")
                    Dim sql As New StringBuilder
                    With sql
                        .Append(" SELECT /* SC3240501_002 */ ")
                        .Append("        Z.DLR_CD ")
                        .Append("  	   , Z.CST_ID ")
                        .Append(" 	   , Z.VCL_ID ")
                        .Append(" 	   , Z.CST_VCL_TYPE ")
                        .Append(" 	   , Z.SVC_PIC_STF_CD ")
                        .Append(" 	   , Z.CST_PHONE ")
                        .Append(" 	   , Z.CST_MOBILE ")
                        .Append(" 	   , Z.CST_NAME ")
                        .Append(" 	   , Z.CST_ADDRESS ")
                        .Append(" 	   , Z.NAMETITLE_CD ")
                        .Append(" 	   , Z.NAMETITLE_NAME ")
                        .Append(" 	   , Z.DMS_CST_CD ")
                        .Append(" 	   , Z.REG_NUM ")
                        .Append(" 	   , Z.VCL_VIN ")
                        .Append(" 	   , Z.STF_NAME ")
                        .Append(" 	   , Z.MODEL_NAME ")
                        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                        .Append(" 	   , Z.CST_TYPE ")
                        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                        .Append("   FROM ( ")
                        .Append("     SELECT ")
                        .Append(" 	         MT2.DLR_CD ")
                        .Append("  	       , MT2.CST_ID ")
                        .Append(" 	       , MT2.VCL_ID ")
                        .Append(" 	       , MT2.CST_VCL_TYPE ")
                        .Append(" 	       , MT2.SVC_PIC_STF_CD ")
                        .Append(" 	       , MT2.CST_PHONE ")
                        .Append(" 	       , MT2.CST_MOBILE ")
                        .Append(" 	       , MT2.CST_NAME ")
                        .Append(" 	       , MT2.CST_ADDRESS ")
                        .Append(" 	       , MT2.NAMETITLE_CD ")
                        .Append(" 	       , MT2.NAMETITLE_NAME ")
                        .Append(" 	       , MT2.DMS_CST_CD ")
                        .Append(" 	       , MT2.REG_NUM ")
                        .Append(" 	       , MT2.VCL_VIN ")
                        .Append(" 	       , MT2.STF_NAME ")
                        .Append(" 	       , MT2.MODEL_NAME ")
                        .Append(" 	       , ROWNUM AS ROW_COUNT ")
                        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                        .Append(" 	       , MT2.CST_TYPE ")
                        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                        .Append("       FROM ( ")
                        .Append(" 	      SELECT DISTINCT ")
                        .Append(" 	             MT1.DLR_CD ")
                        .Append("  	           , MT1.CST_ID ")
                        .Append(" 	           , MT1.VCL_ID ")
                        .Append(" 	           , MT1.CST_VCL_TYPE ")
                        .Append(" 	           , MT1.SVC_PIC_STF_CD ")
                        .Append(" 	           , MT1.CST_PHONE ")
                        .Append(" 	           , MT1.CST_MOBILE ")
                        .Append(" 	           , MT1.CST_NAME ")
                        .Append(" 	           , MT1.CST_ADDRESS ")
                        .Append(" 	           , MT1.NAMETITLE_CD ")
                        .Append(" 	           , MT1.NAMETITLE_NAME ")
                        .Append(" 	           , MT1.DMS_CST_CD ")
                        .Append(" 	           , MT1.REG_NUM ")
                        .Append(" 	           , MT1.VCL_VIN ")
                        .Append(" 	           , MT1.STF_NAME ")
                        .Append(" 	           , MT1.MODEL_NAME ")
                        .Append(" 	           , MT1.CST_TYPE ")
                        .Append("           FROM ( ")
                        .Append("   	      SELECT ")
                        .Append(" 		             CV1.DLR_CD ")
                        .Append(" 	 	           , CV1.CST_ID ")
                        .Append(" 		           , CV1.VCL_ID ")
                        .Append(" 		           , CV1.CST_VCL_TYPE ")
                        .Append(" 		           , CV1.SVC_PIC_STF_CD ")
                        .Append(" 		           , C1.CST_PHONE ")
                        .Append(" 		           , C1.CST_MOBILE ")
                        .Append(" 		           , C1.CST_NAME ")
                        .Append(" 		           , C1.CST_ADDRESS ")
                        .Append(" 		           , C1.NAMETITLE_CD ")
                        .Append(" 		           , C1.NAMETITLE_NAME ")
                        .Append(" 		           , C1.DMS_CST_CD ")
                        .Append(" 		           , VD1.REG_NUM ")
                        .Append(" 		           , V1.VCL_VIN ")
                        .Append(" 		           , S1.STF_NAME ")
                        .Append(" 		           , NVL(M1.MODEL_NAME, V1.NEWCST_MODEL_NAME) AS MODEL_NAME ")
                        .Append(" 		           , CD1.CST_TYPE ")
                        .Append(" 	            FROM ")
                        .Append(" 		             TB_M_CUSTOMER_VCL CV1 ")
                        .Append(" 		           , TB_M_CUSTOMER C1 ")
                        .Append(" 		           , TB_M_VEHICLE_DLR VD1 ")
                        .Append(" 		           , TB_M_VEHICLE V1 ")
                        .Append(" 		           , TB_M_CUSTOMER_DLR CD1 ")
                        .Append(" 		           , TB_M_STAFF S1 ")
                        .Append(" 	               , TB_M_MODEL M1 ")
                        .Append(" 	           WHERE  ")
                        .Append(" 	                 CV1.CST_ID = C1.CST_ID ")
                        .Append(" 	             AND CV1.DLR_CD = VD1.DLR_CD ")
                        .Append(" 	             AND CV1.VCL_ID = VD1.VCL_ID ")
                        .Append(" 	             AND CV1.VCL_ID = V1.VCL_ID ")
                        .Append(" 	             AND CV1.DLR_CD = CD1.DLR_CD ")
                        .Append(" 	             AND CV1.CST_ID = CD1.CST_ID ")
                        .Append(" 	             AND CV1.SVC_PIC_STF_CD = S1.STF_CD(+) ")
                        .Append(" 	             AND V1.MODEL_CD = M1.MODEL_CD(+) ")
                        .Append(" 	             AND CV1.DLR_CD = :DLR_CD ")
                        .Append(" 	             AND VD1.DLR_CD = :DLR_CD ")
                        .Append(" 	             AND CD1.DLR_CD = :DLR_CD ")
                        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                        '.Append(" 	             AND CV1.CST_VCL_TYPE <> N'4' ")
                        .Append(" 	             AND CV1.CST_VCL_TYPE IN(:CST_VCL_TYPE_1,:CST_VCL_TYPE_2) ")
                        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                        .Append(" 	             AND CV1.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")

                        If searchType = 0 Then
                            '検索条件：登録No.（後方一致）
                            '.Append(" AND VD1.REG_NUM_SEARCH_REV LIKE UPPER(:REG_NUM_SEARCH_REV) ")

                            ''リバースカラムに前方一致で当てるために文字列を反転
                            'Dim registerNo As String = StrReverse(inRegisterNo.Trim()) & LikeWord
                            'query.AddParameterWithTypeValue("REG_NUM_SEARCH_REV", OracleDbType.NVarchar2, registerNo)

                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 START 
                            '.Append(" 	             AND VD1.REG_NUM_SEARCH_REV LIKE UPPER('")
                            '.Append(StrReverse(inRegisterNo.Trim()) & LikeWord)
                            '.Append("') ")
                            .Append(" 	             AND VD1.REG_NUM_SEARCH_REV LIKE UPPER(:REG_NUM) ")
                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 END

                        ElseIf searchType = 1 Then
                            '検索条件：VIN（後方一致）
                            '.Append(" AND V1.VCL_VIN_SEARCH_REV LIKE UPPER(:REG_NUM_SEARCH_REV) ")

                            ''リバースカラムに前方一致で当てるために文字列を反転
                            'Dim vinNo As String = StrReverse(inVinNo.Trim()) & LikeWord
                            'query.AddParameterWithTypeValue("REG_NUM_SEARCH_REV", OracleDbType.NVarchar2, vinNo)

                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 START 
                            '.Append(" 	             AND V1.VCL_VIN_SEARCH_REV LIKE UPPER('")
                            '.Append(StrReverse(inVinNo.Trim()) & LikeWord)
                            '.Append("') ")
                            .Append(" 	             AND V1.VCL_VIN_SEARCH_REV LIKE UPPER(:VCL_VIN) ")
                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 END

                        ElseIf searchType = 2 Then
                            '検索条件：名前（前方一致）
                            '.Append(" AND C1.CST_NAME_SEARCH LIKE UPPER(:CST_NAME_SEARCH) ")

                            'Dim customerName As String = inCustomerName.Trim() & LikeWord
                            'query.AddParameterWithTypeValue(":CST_NAME_SEARCH", OracleDbType.NVarchar2, customerName)

                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 START 
                            '.Append(" 	             AND C1.CST_NAME_SEARCH LIKE UPPER('")
                            '.Append(inCustomerName.Trim() & LikeWord)
                            '.Append("') ")
                            .Append(" 	             AND C1.CST_NAME_SEARCH LIKE UPPER(:CST_NAME) ")
                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 END

                        ElseIf searchType = 3 Then
                            '検索条件：TEL（完全一致）
                            '.Append(" 	             AND C1.CST_PHONE_SEARCH = :TEL ")

                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 START 
                            '.Append(" 	             AND C1.CST_PHONE_SEARCH = '")
                            '.Append(inTelNo.Trim().Replace("-", ""))
                            '.Append("'")
                            .Append(" 	             AND C1.CST_PHONE_SEARCH = :TEL_NO")
                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 END

                            .Append("          UNION ALL ")
                            .Append("             SELECT ")
                            .Append(" 		             CV2.DLR_CD ")
                            .Append(" 	 	           , CV2.CST_ID ")
                            .Append(" 		           , CV2.VCL_ID ")
                            .Append(" 		           , CV2.CST_VCL_TYPE ")
                            .Append(" 		           , CV2.SVC_PIC_STF_CD ")
                            .Append(" 		           , C2.CST_PHONE ")
                            .Append(" 		           , C2.CST_MOBILE ")
                            .Append(" 		           , C2.CST_NAME ")
                            .Append(" 		           , C2.CST_ADDRESS ")
                            .Append(" 		           , C2.NAMETITLE_CD ")
                            .Append(" 		           , C2.NAMETITLE_NAME ")
                            .Append(" 		           , C2.DMS_CST_CD ")
                            .Append(" 		           , VD2.REG_NUM ")
                            .Append(" 		           , V2.VCL_VIN ")
                            .Append(" 		           , S2.STF_NAME ")
                            .Append(" 		           , NVL(M2.MODEL_NAME, V2.NEWCST_MODEL_NAME) AS MODEL_NAME ")
                            .Append(" 		           , CD2.CST_TYPE ")
                            .Append(" 	            FROM ")
                            .Append(" 		             TB_M_CUSTOMER_VCL CV2 ")
                            .Append(" 		           , TB_M_CUSTOMER C2 ")
                            .Append(" 		           , TB_M_VEHICLE_DLR VD2 ")
                            .Append(" 		           , TB_M_VEHICLE V2 ")
                            .Append(" 		           , TB_M_CUSTOMER_DLR CD2 ")
                            .Append(" 		           , TB_M_STAFF S2 ")
                            .Append(" 	               , TB_M_MODEL M2 ")
                            .Append(" 	           WHERE ")
                            .Append(" 	                 CV2.CST_ID = C2.CST_ID ")
                            .Append(" 	             AND CV2.DLR_CD = VD2.DLR_CD ")
                            .Append(" 	             AND CV2.VCL_ID = VD2.VCL_ID ")
                            .Append(" 	             AND CV2.VCL_ID = V2.VCL_ID ")
                            .Append(" 	             AND CV2.DLR_CD = CD2.DLR_CD ")
                            .Append(" 	             AND CV2.CST_ID = CD2.CST_ID ")
                            .Append(" 	             AND CV2.SVC_PIC_STF_CD = S2.STF_CD(+) ")
                            .Append(" 	             AND V2.MODEL_CD = M2.MODEL_CD(+) ")
                            .Append(" 	             AND CV2.DLR_CD = :DLR_CD ")
                            .Append(" 	             AND VD2.DLR_CD = :DLR_CD ")
                            .Append(" 	             AND CD2.DLR_CD = :DLR_CD ")
                            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                            '.Append(" 	   	         AND CV2.CST_VCL_TYPE <> N'4' ")
                            .Append(" 	             AND CV2.CST_VCL_TYPE IN(:CST_VCL_TYPE_1,:CST_VCL_TYPE_2) ")
                            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                            .Append(" 	             AND CV2.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")
                            '.Append(" 	             AND C2.CST_MOBILE_SEARCH = :TEL ")

                            'Dim tel As String = inTelNo.Trim().Replace("-", "")
                            'query.AddParameterWithTypeValue("TEL", OracleDbType.NVarchar2, tel)

                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 START 
                            '.Append(" 	             AND C2.CST_MOBILE_SEARCH = '")
                            '.Append(inTelNo.Trim().Replace("-", ""))
                            '.Append("'")
                            .Append(" 	             AND C2.CST_MOBILE_SEARCH = :TEL_NO")
                            '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 END

                        End If

                        .Append("                ) MT1 ")

                        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                        'Select Case searchType
                        '   Case 0
                        '検索条件：登録No.
                        '.Append(" 	ORDER BY MT1.REG_NUM ASC, MT1.VCL_VIN ASC, MT1.CST_TYPE ASC ")
                        '   Case 1
                        '検索条件：VIN
                        '.Append(" 	ORDER BY MT1.VCL_VIN ASC, MT1.CST_TYPE ASC ")
                        ' Case Else
                        '検索条件：名前、TEL
                        '.Append(" 	ORDER BY MT1.CST_NAME, MT1.REG_NUM ASC, MT1.VCL_VIN ASC, MT1.CST_TYPE ASC ")
                        'End Select

                        Select Case searchType
                            Case 0
                                '検索条件：登録No.
                                .Append(" 	ORDER BY MT1.REG_NUM ASC, MT1.VCL_VIN ASC, MT1.CST_TYPE ASC, MT1.CST_VCL_TYPE ASC ")
                            Case 1
                                '検索条件：VIN
                                .Append(" 	ORDER BY MT1.VCL_VIN ASC, MT1.CST_TYPE ASC, MT1.CST_VCL_TYPE ASC ")
                            Case Else
                                '検索条件：名前、TEL
                                .Append(" 	ORDER BY MT1.CST_NAME, MT1.REG_NUM ASC, MT1.VCL_VIN ASC, MT1.CST_TYPE ASC, MT1.CST_VCL_TYPE ASC ")
                        End Select

                        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                        .Append("            ) MT2 ")
                        .Append("        ) Z ")
                        .Append(" WHERE Z.ROW_COUNT BETWEEN :STARTINDEX AND :ENDINDEX ")
                    End With

                    query.CommandText = sql.ToString()
                    query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCD)
                    query.AddParameterWithTypeValue("STARTINDEX", OracleDbType.Long, inStartRowNo)
                    query.AddParameterWithTypeValue("ENDINDEX", OracleDbType.Long, inEndRowNo)

                    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

                    '顧客車両区分(1：所有者)
                    query.AddParameterWithTypeValue("CST_VCL_TYPE_1", OracleDbType.NVarchar2, VehicleOwner)
                    '顧客車両区分(2：使用者)
                    query.AddParameterWithTypeValue("CST_VCL_TYPE_2", OracleDbType.NVarchar2, VehicleUser)
                    'オーナーチェンジフラグ(0：未設定)
                    query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, NoChange)

                    '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                    '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 START
                    Select Case searchType
                        Case 0 '登録No.
                            query.AddParameterWithTypeValue("REG_NUM", OracleDbType.NVarchar2, StrReverse(String.Concat(LikeWord, inRegisterNo.Trim())))
                        Case 1 'VIN
                            query.AddParameterWithTypeValue("VCL_VIN", OracleDbType.NVarchar2, StrReverse(String.Concat(LikeWord, inVinNo.Trim())))
                        Case 2 '名前
                            query.AddParameterWithTypeValue("CST_NAME", OracleDbType.NVarchar2, String.Concat(inCustomerName.Trim(), LikeWord))
                        Case 3 '電話番号
                            query.AddParameterWithTypeValue("TEL_NO", OracleDbType.NVarchar2, inTelNo.Trim().Replace("-", ""))
                    End Select
                    '2018/05/01 NSK 井本 TR-V4-TMT-20171117-001 シングルクォ－テ－ションによるエラー対応 END

                    dtResult = query.GetData()

                End Using

            Catch ex As OracleExceptionEx When ex.Number = 1013

                'ORACLEのタイムアウトのみ処理
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                                        , "{0}.{1} OUT:RETURNCODE = {2} {3}" _
                                        , Me.GetType.ToString _
                                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                        , C_RET_DBTIMEOUT _
                                        , ex.Message))
                Throw ex

            End Try

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", dtResult.Rows.Count)

            Return dtResult

        End Function

#End Region

#Region "DropDownList情報取得"
        ''' <summary>
        ''' 販売店コード・店舗コードに紐付く整備種類情報を取得する
        ''' </summary>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="strCD">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSvcClassList(ByVal dlrCD As String, ByVal strCD As String) As SC3240501DataSet.SC3240501SvcClassListDataTable

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[dlrCD:{0}][strCD:{1}]", dlrCD, strCD)

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3240501_003 */ ")
                .Append("        B.SVC_CLASS_ID || ',' || A.STD_WORKTIME AS SVCID_TIME ")                'サービス分類ID,標準作業時間
                .Append("      , NVL(TRIM(B.SVC_CLASS_NAME), B.SVC_CLASS_NAME_ENG) AS SVC_CLASS_NAME ")  'サービス分類名称
                .Append("      , B.SVC_CLASS_TYPE ")                                                     'サービス分類区分 (1:EM 2:PM 3:GR 4:PDS 5:BP)
                .Append("      , A.CARWASH_NEED_FLG ")                                                   '洗車必要フラグ
                .Append(" FROM ")
                .Append("        TB_M_BRANCH_SERVICE_CLASS A ")     '店舗サービス分類
                .Append("      , TB_M_SERVICE_CLASS B ")            'サービス分類マスタ
                .Append(" WHERE ")
                .Append("        A.SVC_CLASS_ID = B.SVC_CLASS_ID ")
                .Append("    AND A.DLR_CD = :DLR_CD ")
                .Append("    AND A.BRN_CD = :BRN_CD ")
                .Append("    AND B.INUSE_FLG = :INUSE_FLG_1 ")
                .Append(" ORDER BY ")
                .Append("        A.SORT_ORDER ")
            End With

            Using query As New DBSelectQuery(Of SC3240501DataSet.SC3240501SvcClassListDataTable)("SC3240501_003")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)               '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strCD)               '店舗コード
                query.AddParameterWithTypeValue("INUSE_FLG_1", OracleDbType.NVarchar2, INUSE_TYPE_USE) '使用中フラグ

                'SQL実行
                Dim rtnDt As SC3240501DataSet.SC3240501SvcClassListDataTable = query.GetData()

                OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

                Return rtnDt

            End Using

        End Function

        ''' <summary>
        ''' 販売店コード・店舗コード・サービス分類IDに紐付く商品情報を取得する
        ''' </summary>
        ''' <param name="dlrCD">販売店コード</param>
        ''' <param name="strCD">店舗コード</param>
        ''' <param name="svcClassId">サービス分類ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetMercList(ByVal dlrCD As String, ByVal strCD As String, ByVal svcClassId As Decimal) As SC3240501DataSet.SC3240501MercListDataTable

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[dlrCD:{0}][strCD:{1}][svcClassId:{2}]", dlrCD, strCD, svcClassId)

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3240501_004 */ ")
                .Append("        B.MERC_ID || ',' || A.STD_WORKTIME AS MERCID_TIME ")       '商品ID,標準作業時間
                .Append("      , NVL(TRIM(B.MERC_NAME), B.MERC_NAME_ENG) AS MERC_NAME ")    '商品名称
                .Append(" FROM ")
                .Append("        TB_M_BRANCH_MERCHANDISE A ")        '店舗商品
                .Append("      , TB_M_MERCHANDISE B ")               '商品マスタ
                .Append(" WHERE ")
                .Append("        A.MERC_ID = B.MERC_ID ")
                .Append("    AND A.DLR_CD = :DLR_CD ")
                .Append("    AND A.BRN_CD = :BRN_CD ")
                .Append("    AND B.INUSE_FLG = :INUSE_FLG_1 ")
                .Append("    AND B.SVC_CLASS_ID = :SVC_CLASS_ID")
                .Append(" ORDER BY ")
                .Append("        A.SORT_ORDER ")
            End With

            Using query As New DBSelectQuery(Of SC3240501DataSet.SC3240501MercListDataTable)("SC3240501_004")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCD)               '販売店コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, strCD)               '店舗コード
                query.AddParameterWithTypeValue("SVC_CLASS_ID", OracleDbType.Decimal, svcClassId)      'サービス分類ID
                query.AddParameterWithTypeValue("INUSE_FLG_1", OracleDbType.NVarchar2, INUSE_TYPE_USE) '使用中フラグ

                'SQL実行
                Dim rtnDt As SC3240501DataSet.SC3240501MercListDataTable = query.GetData()

                OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

                Return rtnDt

            End Using

        End Function

        ''' <summary>
        ''' サービス分類IDを条件にサービス分類コードを取得する
        ''' </summary>
        ''' <param name="svcClassId">サービス分類ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSvcClassCD(ByVal svcClassId As Decimal) As String

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, True, "[svcClassId:{0}]", svcClassId)

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3240501_005 */ ")
                .Append("        SVC_CLASS_CD ")                    'サービス分類コード
                .Append(" FROM ")
                .Append("        TB_M_SERVICE_CLASS ")              'サービス分類マスタ
                .Append(" WHERE ")
                .Append("        SVC_CLASS_ID = :SVC_CLASS_ID ")
            End With

            Using query As New DBSelectQuery(Of SC3240501DataSet.SC3240501SvcClassCDDataTable)("SC3240501_005")
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("SVC_CLASS_ID", OracleDbType.Decimal, svcClassId)      'サービス分類ID

                'SQL実行
                Dim dt As SC3240501DataSet.SC3240501SvcClassCDDataTable = query.GetData()

                '戻り値
                Dim retValue As String = String.Empty

                If 0 < dt.Count Then
                    'サービス分類コードを取得
                    retValue = dt.Rows(0).Item(0).ToString()
                End If

                OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[retValue:{0}]", retValue)

                Return retValue

            End Using

        End Function

        ''' <summary>
        ''' 敬称情報を取得する
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSvcNameTitle() As SC3240501DataSet.SC3240501NameTitleListDataTable

            OutputInfoLog(MethodBase.GetCurrentMethod.Name, True)

            'SQL組み立て
            Dim sql As New StringBuilder
            With sql
                .Append(" SELECT /* SC3240501_006 */ ")
                .Append("        NAMETITLE_CD ")                '敬称コード
                .Append("      , NAMETITLE_NAME ")              '敬称名称
                .Append(" FROM ")
                .Append("        TB_M_NAMETITLE ")              '敬称マスタ
                .Append(" WHERE ")
                .Append("        INUSE_FLG = :INUSE_FLG_1 ")
            End With

            Using query As New DBSelectQuery(Of SC3240501DataSet.SC3240501NameTitleListDataTable)("SC3240501_006")
                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("INUSE_FLG_1", OracleDbType.NVarchar2, INUSE_TYPE_USE)      '使用中フラグ

                'SQL実行
                Dim rtnDt As SC3240501DataSet.SC3240501NameTitleListDataTable = query.GetData()

                OutputInfoLog(MethodBase.GetCurrentMethod.Name, False, "[rowCount:{0}]", rtnDt.Rows.Count)

                Return rtnDt

            End Using

        End Function
#End Region

#Region "ログ出力メソッド"
        ''' <summary>
        ''' 引数のないInfoレベルのログを出力する
        ''' </summary>
        ''' <param name="method">メソッド名</param>
        ''' <param name="isStart">True:Startログ/False:Endログ</param>
        ''' <remarks></remarks>
        Private Sub OutputInfoLog(ByVal method As String, ByVal isStart As Boolean)

            If isStart Then
                Logger.Info(NEWCHIP_PROGRAMID & ".ascx " & method & "_Start")
            Else
                Logger.Info(NEWCHIP_PROGRAMID & ".ascx " & method & "_End")
            End If

        End Sub

        ''' <summary>
        ''' 引数のあるInfoレベルのログを出力する
        ''' </summary>
        ''' <param name="method">メソッド名</param>
        ''' <param name="isStart">True:Startログ/False:Endログ</param>
        ''' <param name="argString">フォーマット用文字列</param>
        ''' <param name="args">フォーマット用文字列に当てはめる引数値</param>
        ''' <remarks></remarks>
        Private Sub OutputInfoLog(ByVal method As String, ByVal isStart As Boolean, ByVal argString As String, ParamArray args() As Object)

            Dim logString As String = String.Empty

            If isStart Then
                logString = NEWCHIP_PROGRAMID & ".ascx " & method & "_Start" & argString
                Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))
            Else
                logString = NEWCHIP_PROGRAMID & ".ascx " & method & "_End" & argString
                Logger.Info(String.Format(CultureInfo.InvariantCulture, logString, args))
            End If

        End Sub
#End Region

    End Class

End Namespace

Partial Class SC3240501DataSet
End Class
