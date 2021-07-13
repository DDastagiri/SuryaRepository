'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3810401DataSet.vb
'─────────────────────────────────────
'機能： R/O,REZ連携データアクセス
'補足： 
'作成： 2012/01/26 KN 瀧
'更新： 2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更
'更新： 2012/02/17 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加
'更新： 2012/02/23 KN 瀧 【SERVICE_1】ストップフラグの変更(TEMPorWALKINの判定を追加)
'更新： 2012/02/24 KN 瀧 【SERVICE_1】ストール予約テーブルの更新方法を変更
'更新： 2012/02/27 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加
'更新： 2012/03/03 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加２
'更新： 2012/03/03 KN 瀧 【SERVICE_1】引数に車名、モデルコードを追加
'更新： 
'─────────────────────────────────────

Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace IC3810401DataSetTableAdapters
    Public Class IC3810401DataTableAdapter
        Inherits Global.System.ComponentModel.Component

        '2012/02/23 KN 瀧 【SERVICE_1】ストップフラグの変更(TEMPorWALKINの判定を追加) START
        ''' <summary>
        ''' WALKIN利用フラグの確認
        ''' </summary>
        ''' <param name="dealerCD">販売店コード</param>
        ''' <value></value>
        ''' <returns>WALKIN利用フラグのON:true/OFF:false</returns>
        ''' <remarks></remarks>
        ''' 2012/02/23 KN 瀧 【SERVICE_1】ストップフラグの変更(TEMPorWALKINの判定を追加)
        ''' <history>
        ''' </history>
        Private ReadOnly Property IsWalkinUse(ByVal dealerCD As String) As Boolean
            Get
                Const WALKIN_USE_FLG As String = "WALKIN_USE_FLG"
                Static flag As Boolean?
                If flag.HasValue = False Then
                    Dim row As DlrEnvSettingDataSet.DLRENVSETTINGRow = (New DealerEnvSetting).GetEnvSetting(dealerCD, WALKIN_USE_FLG)
                    flag = ((row IsNot Nothing) AndAlso (String.Equals(row.PARAMVALUE, "1") = True))
                End If
                Return flag.Value
            End Get
        End Property

        ''' <summary>
        ''' WALKIN利用フラグの確認
        ''' </summary>
        ''' <param name="dealerCD">販売店コード</param>
        ''' <value></value>
        ''' <returns>WALKIN利用フラグのON:true/OFF:false</returns>
        ''' <remarks></remarks>
        ''' 2012/02/23 KN 瀧 【SERVICE_1】ストップフラグの変更(TEMPorWALKINの判定を追加)
        ''' <history>
        ''' </history>
        Private ReadOnly Property BaseTypeAll(ByVal dealerCD As String) As String
            Get
                Const BASETYPE_ALL As String = "BASETYPE_ALL"
                Static value As String
                If String.IsNullOrEmpty(value) = True Then
                    Dim row As DlrEnvSettingDataSet.DLRENVSETTINGRow = (New DealerEnvSetting).GetEnvSetting(dealerCD, BASETYPE_ALL)
                    value = If(row IsNot Nothing, row.PARAMVALUE, "*")
                End If
                Return value
            End Get
        End Property
        '2012/02/23 KN 瀧 【SERVICE_1】ストップフラグの変更(TEMPorWALKINの判定を追加) END

        ''' <summary>
        ''' ストール予約テーブルキー情報の取得
        ''' </summary>
        ''' <param name="rowIN">予約情報更新引数</param>
        ''' <returns>ストール予約キー情報</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' 2012/02/24 KN 瀧 【SERVICE_1】ストール予約テーブルの更新方法を変更
        ''' </history>
        Public Function GetStallKey(ByVal rowIN As IC3810401DataSet.IC3810401InOrderSaveRow) As IC3810401DataSet.IC3810401StallKeyDataTable
            ''引数をログに出力
            Dim args As New List(Of String)
            ' DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            Using query As New DBSelectQuery(Of IC3810401DataSet.IC3810401StallKeyDataTable)("IC3810401_001")
                ''SQLの設定
                Dim sql As New StringBuilder
                sql.AppendLine("SELECT /* IC3810401_001 */")
                sql.AppendLine("       REZID")
                sql.AppendLine("  FROM TBL_STALLREZINFO")
                sql.AppendLine(" WHERE DLRCD = :DLRCD")
                sql.AppendLine("   AND STRCD = :STRCD")

                '2012/02/24 KN 瀧 【SERVICE_1】ストール予約テーブルの更新方法を変更 START
                'sql.AppendLine("   AND REZID = :REZID")
                If rowIN.IsREZIDNull = False Then
                    sql.AppendLine("   AND REZID = :REZID")
                Else
                    sql.AppendLine("   AND ORDERNO = :ORDERNO")
                End If
                '2012/02/24 KN 瀧 【SERVICE_1】ストール予約テーブルの更新方法を変更 END

                ''パラメータの設定
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)

                '2012/02/24 KN 瀧 【SERVICE_1】ストール予約テーブルの更新方法を変更 START
                'query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, rowIN.REZID)
                If rowIN.IsREZIDNull = False Then
                    query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, rowIN.REZID)
                Else
                    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)
                End If
                '2012/02/24 KN 瀧 【SERVICE_1】ストール予約テーブルの更新方法を変更 END

                ''SQLの実行
                query.CommandText = sql.ToString()
                Using dt As IC3810401DataSet.IC3810401StallKeyDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using

        End Function

        ''' <summary>
        ''' サービス来店者管理テーブルの存在チェック
        ''' </summary>
        ''' <param name="rowIN">予約情報更新引数</param>
        ''' <returns>サービス来店者管理キー情報</returns>
        ''' <remarks></remarks>
        '''
        ''' <history>
        ''' 2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更
        ''' </history>
        Public Function GetVisitKey(ByVal rowIN As IC3810401DataSet.IC3810401InOrderSaveRow) As IC3810401DataSet.IC3810401VisitKeyDataTable
            ''引数をログに出力
            Dim args As New List(Of String)
            ' DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            ''SQLの設定
            Dim sql As New StringBuilder
            sql.AppendLine("SELECT /* IC3810401_002 */")
            sql.AppendLine("       SACODE")
            sql.AppendLine("  FROM TBL_SERVICE_VISIT_MANAGEMENT")

            '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 START
            'sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
            If rowIN.IsVISITSEQNull = False Then
                sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
            Else
                sql.AppendLine(" WHERE ORDERNO = :ORDERNO")
            End If
            '更新： 2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 END

            sql.AppendLine("   AND DLRCD = :DLRCD")
            sql.AppendLine("   AND STRCD = :STRCD")

            Using query As New DBSelectQuery(Of IC3810401DataSet.IC3810401VisitKeyDataTable)("IC3810401_002")

                ''パラメータの設定
                '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 START
                'query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
                If rowIN.IsVISITSEQNull = False Then
                    query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
                Else
                    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)
                End If
                '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 END

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                ''SQLの実行
                query.CommandText = sql.ToString()
                Using dt As IC3810401DataSet.IC3810401VisitKeyDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using

        End Function

        '更新： 2012/02/27 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加 START
        ''' <summary>
        ''' 自社客個人情報テーブルの取得
        ''' </summary>
        ''' <param name="rowIN">予約情報更新引数</param>
        ''' <returns>自社客個人情報</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' 2012/02/27 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加
        ''' 2012/03/03 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加２
        ''' </history>
        Public Function GetCustomerInfo(ByVal rowIN As IC3810401DataSet.IC3810401InOrderSaveRow) As IC3810401DataSet.IC3810401CustomerInfoDataTable
            ''引数をログに出力
            Dim args As New List(Of String)
            ' DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            ''顧客コードが未入力の場合
            If rowIN.IsCUSTOMERCODENull = True _
                OrElse rowIN.CUSTOMERCODE.Trim.Length = 0 Then
                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , 0))
                Return New IC3810401DataSet.IC3810401CustomerInfoDataTable
            End If

            Using query As New DBSelectQuery(Of IC3810401DataSet.IC3810401CustomerInfoDataTable)("IC3810401_003")
                ''SQLの設定
                Dim sql As New StringBuilder
                sql.AppendLine("SELECT /* IC3810401_003 */")
                '2012/03/03 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加２ START
                'sql.AppendLine("      ZIPCODE")
                sql.AppendLine("       ORIGINALID")
                sql.AppendLine("     , ZIPCODE")
                '2012/03/03 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加２ END
                sql.AppendLine("     , ADDRESS")
                sql.AppendLine("     , TELNO")
                sql.AppendLine("     , MOBILE")
                sql.AppendLine("  FROM TBLORG_CUSTOMER")
                sql.AppendLine(" WHERE DLRCD = :DLRCD")
                'sql.AppendLine(" AND STRCD = :STRCD")
                sql.AppendLine("   AND CUSTCD = :CUSTCD")
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                query.AddParameterWithTypeValue("CUSTCD", OracleDbType.NVarchar2, rowIN.CUSTOMERCODE)
                ''SQLの実行
                query.CommandText = sql.ToString()
                Using dt As IC3810401DataSet.IC3810401CustomerInfoDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using

        End Function

        ''' <summary>
        ''' 自社客車両情報テーブルの取得
        ''' </summary>
        ''' <param name="rowIN">予約情報更新引数</param>
        ''' <returns>自社客車両情報</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' </history>
        Public Function GetVehicleInfo(ByVal rowIN As IC3810401DataSet.IC3810401InOrderSaveRow) As IC3810401DataSet.IC3810401VehicleInfoDataTable
            ''引数をログに出力
            Dim args As New List(Of String)
            ' DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            ''VINと車両NOの両方が未入力の場合
            If (rowIN.IsVINNull = True OrElse rowIN.VIN.Trim.Length = 0) _
                AndAlso (rowIN.IsVCLREGNONull = True OrElse rowIN.VCLREGNO.Trim.Length = 0) Then
                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , 0))
                Return New IC3810401DataSet.IC3810401VehicleInfoDataTable
            End If

            Using query As New DBSelectQuery(Of IC3810401DataSet.IC3810401VehicleInfoDataTable)("IC3810401_004")
                ''SQLの設定
                Dim sql As New StringBuilder
                sql.AppendLine("SELECT /* IC3810401_004 */")
                sql.AppendLine("       VCLREGNO")
                sql.AppendLine("     , VIN")
                sql.AppendLine("     , SERIESNM")
                sql.AppendLine("     , BASETYPE")
                sql.AppendLine("  FROM TBLORG_VCLINFO")
                sql.AppendLine(" WHERE DLRCD = :DLRCD")
                'sql.AppendLine(" AND STRCD = :STRCD")
                If rowIN.IsVINNull = False _
                    AndAlso rowIN.VIN.Trim.Length > 0 Then
                    sql.AppendLine("   AND VIN = :VIN")
                Else
                    sql.AppendLine("   AND VCLREGNO = :VCLREGNO")
                End If
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                If rowIN.IsVINNull = False _
                  AndAlso rowIN.VIN.Trim.Length > 0 Then
                    query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowIN.VIN)
                Else
                    query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, rowIN.VCLREGNO)
                End If
                ''SQLの実行
                query.CommandText = sql.ToString()
                Using dt As IC3810401DataSet.IC3810401VehicleInfoDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))
                    Return dt
                End Using
            End Using

        End Function

        ''' <summary>
        ''' サービス所要時間の取得
        ''' </summary>
        ''' <param name="rowIN">予約情報更新引数</param>
        ''' <returns>サービス所要時間</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' </history>
        Public Function GetServiceInfo(ByVal rowIN As IC3810401DataSet.IC3810401InOrderSaveRow _
                                      , ByVal rowVI As IC3810401DataSet.IC3810401VehicleInfoRow) As IC3810401DataSet.IC3810401ServiceInfoDataTable
            ''引数をログに出力
            Dim args As New List(Of String)
            ' DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            ''サービスコード
            Using dtSI As New IC3810401DataSet.IC3810401ServiceInfoDataTable
                Dim rowSI As IC3810401DataSet.IC3810401ServiceInfoRow = dtSI.NewIC3810401ServiceInfoRow

                ''整備コードが存在する場合
                If rowIN.IsMNTNCDNull = False _
                    AndAlso rowIN.MNTNCD.Trim.Length > 0 Then
                    ''基幹整備コード変換マスタより商品コードを取得
                    Using query As New DBSelectQuery(Of DataTable)("IC3810401_005")
                        ''SQLの設定
                        Dim sqlML As New StringBuilder
                        sqlML.AppendLine("SELECT /* IC3810401_005 */")
                        sqlML.AppendLine("       SEQ")
                        sqlML.AppendLine("     , MERCHANDISECD")
                        sqlML.AppendLine("  FROM (")
                        sqlML.AppendLine("    SELECT")
                        sqlML.AppendLine("           1 AS SEQ")
                        sqlML.AppendLine("         , MERCHANDISECD")
                        sqlML.AppendLine("      FROM TBL_MAINTELINK")
                        sqlML.AppendLine("     WHERE DLRCD = :DLRCD")
                        sqlML.AppendLine("       AND BASETYPE = :BASETYPEALL")
                        sqlML.AppendLine("       AND MNTNCD = :MNTNCD")

                        ''販売店コード
                        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                        ''基本型式
                        query.AddParameterWithTypeValue("BASETYPEALL", OracleDbType.NVarchar2, Me.BaseTypeAll(rowIN.DLRCD))
                        ''整備コード
                        query.AddParameterWithTypeValue("MNTNCD", OracleDbType.NVarchar2, rowIN.MNTNCD)

                        If rowVI.IsBASETYPENull = False _
                            AndAlso rowVI.BASETYPE.Trim.Length > 0 Then
                            sqlML.AppendLine(" UNION ALL")
                            sqlML.AppendLine("    SELECT")
                            sqlML.AppendLine("           2 AS SEQ")
                            sqlML.AppendLine("         , MERCHANDISECD")
                            sqlML.AppendLine("      FROM TBL_MAINTELINK")
                            sqlML.AppendLine("     WHERE DLRCD = :DLRCD")
                            sqlML.AppendLine("       AND BASETYPE = :BASETYPE")
                            sqlML.AppendLine("       AND MNTNCD = :MNTNCD")
                            ''基本型式
                            query.AddParameterWithTypeValue("BASETYPE", OracleDbType.NVarchar2, rowVI.BASETYPE)
                        End If
                        sqlML.AppendLine("    )")
                        sqlML.AppendLine(" ORDER BY SEQ")

                        ''SQLの実行
                        query.CommandText = sqlML.ToString()
                        Using dtMD As DataTable = query.GetData()
                            If dtMD.Rows.Count > 0 Then
                                ''商品コード
                                rowSI.MERCHANDISECD = dtMD.Rows(0)("MERCHANDISECD").ToString()
                            End If
                        End Using
                    End Using
                End If

                ''商品コードが存在する場合
                If rowSI.IsMERCHANDISECDNull = False _
                    AndAlso rowSI.MERCHANDISECD.Trim.Length > 0 Then

                    ''サービス設定に存在しなかった場合、
                    ''商品マスタよりサービスコードを取得
                    Using query As New DBSelectQuery(Of DataTable)("IC3810401_006")
                        ''SQLの設定
                        Dim sqlMD As New StringBuilder
                        sqlMD.AppendLine("SELECT /* IC3810401_006 */")
                        sqlMD.AppendLine("       SERVICECD")
                        sqlMD.AppendLine("     , SERVICECODE")
                        sqlMD.AppendLine("  FROM TBL_MERCHANDISEMST")
                        sqlMD.AppendLine(" WHERE DLRCD = :DLRCD")
                        sqlMD.AppendLine("   AND MERCHANDISECD = :MERCHANDISECD")
                        sqlMD.AppendLine("   AND DELFLG = '0'")
                        sqlMD.AppendLine("   AND SERVICECODE IS NOT NULL")
                        ''販売店コード
                        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                        ''商品コード
                        query.AddParameterWithTypeValue("MERCHANDISECD", OracleDbType.Char, rowSI.MERCHANDISECD)
                        ''SQLの実行
                        query.CommandText = sqlMD.ToString()
                        Using dtMD As DataTable = query.GetData()
                            If dtMD.Rows.Count > 0 Then
                                ''サービスコード
                                rowSI.SERVICECD = dtMD.Rows(0)("SERVICECD").ToString()
                                ''サービスカテゴリー
                                rowSI.SERVICECODE = dtMD.Rows(0)("SERVICECODE").ToString()
                            End If
                        End Using
                    End Using
                    ''サービス設定からサービス所要時間を取得
                    Using query As New DBSelectQuery(Of DataTable)("IC3810401_007")
                        ''SQLの設定
                        Dim sqlSS As New StringBuilder
                        sqlSS.AppendLine("SELECT /* IC3810401_007 */")
                        sqlSS.AppendLine("       SERVICETIME")
                        sqlSS.AppendLine("  FROM TBL_SERVICESETTING")
                        sqlSS.AppendLine(" WHERE MERCHANDISECD = :MERCHANDISECD")
                        sqlSS.AppendLine(" AND DLRCD = :DLRCD")
                        sqlSS.AppendLine(" AND STRCD = :STRCD")
                        sqlSS.AppendLine(" AND SERVICETIME IS NOT NULL")
                        ''商品コード
                        query.AddParameterWithTypeValue("MERCHANDISECD", OracleDbType.Char, rowSI.MERCHANDISECD)
                        ''販売店コード
                        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                        ''店舗コード
                        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                        ''SQLの実行
                        query.CommandText = sqlSS.ToString()
                        Using dtSS As DataTable = query.GetData()
                            If dtSS.Rows.Count > 0 Then
                                ''サービス所要時間
                                rowSI.SERVICETIME = Convert.ToDecimal(dtSS.Rows(0)("SERVICETIME"), CultureInfo.CurrentCulture)
                            End If
                        End Using
                    End Using
                End If
                ''サービスコードが取得できていない場合
                If rowSI.IsSERVICECODENull = True Then
                    ''30:GRを設定
                    rowSI.SERVICECODE = "30"
                End If
                ''サービス所要時間が取得できていない場合
                If rowSI.IsSERVICETIMENull = True Then
                    ''とりあえず規定値15分を設定
                    rowSI.SERVICETIME = 15
                    ''ストールサービスマスタからサービス所要時間を取得
                    Using query As New DBSelectQuery(Of DataTable)("IC3810401_008")
                        ''SQLの設定
                        Dim sqlSV As New StringBuilder
                        sqlSV.AppendLine("SELECT /* IC3810401_008 */")
                        sqlSV.AppendLine("       SVCTIME")
                        sqlSV.AppendLine("  FROM TBL_SSERVICE")
                        sqlSV.AppendLine(" WHERE SERVICECODE = :SERVICECODE")
                        sqlSV.AppendLine("   AND DLRCD = :DLRCD")
                        sqlSV.AppendLine("   AND STRCD = :STRCD")
                        sqlSV.AppendLine("   AND DELFLG = '0'")
                        ''サービスカテゴリー
                        query.AddParameterWithTypeValue("SERVICECODE", OracleDbType.Char, rowSI.SERVICECODE)
                        '販売店コード
                        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                        ''店舗コード
                        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                        ''SQLの実行
                        query.CommandText = sqlSV.ToString()
                        Using dt As DataTable = query.GetData()
                            If dt.Rows.Count > 0 Then
                                ''サービス所要時間
                                rowSI.SERVICETIME = Convert.ToDecimal(dt.Rows(0)("SVCTIME"), CultureInfo.CurrentCulture)
                            End If
                        End Using
                    End Using
                End If

                dtSI.Rows.Add(rowSI)
                Return dtSI
            End Using

        End Function
        '2012/02/27 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加 END

        ''' <summary>
        ''' ストール予約(新規追加)
        ''' </summary>
        ''' <param name="rowIN">予約情報更新引数</param>
        ''' <param name="rowVI">自社客車両情報</param>
        ''' <param name="rowCI">自社客個人情報</param>
        ''' <returns>予約ID</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' 2012/02/17 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加
        ''' 2012/02/27 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加
        ''' 2012/03/03 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加２
        ''' 2012/03/03 KN 瀧 【SERVICE_1】引数に車名、モデルコードを追加
        ''' </history>
        Public Function InsertStallOrder(ByVal rowIN As IC3810401DataSet.IC3810401InOrderSaveRow _
                                        , ByVal rowCI As IC3810401DataSet.IC3810401CustomerInfoRow _
                                        , ByVal rowVI As IC3810401DataSet.IC3810401VehicleInfoRow _
                                        , ByVal rowSI As IC3810401DataSet.IC3810401ServiceInfoRow) As Long
            ''引数をログに出力
            Dim args As New List(Of String)
            ' DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            Me.AddLogData(args, rowCI)
            Me.AddLogData(args, rowVI)
            Me.AddLogData(args, rowSI)
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            ''現在時刻の取得
            Dim sysDate As Date = DateTimeFunc.Now(rowIN.DLRCD)

            Dim rezid As Long = 0
            Using query As New DBSelectQuery(Of DataTable)("IC3810401_101")
                ''SQLの設定
                Dim sqlNextVal As New StringBuilder
                sqlNextVal.AppendLine("SELECT /* IC3810401_101 */")
                sqlNextVal.AppendLine("       SEQ_STALLREZINFO_REZID.NEXTVAL AS REZID")
                sqlNextVal.AppendLine("  FROM DUAL")
                ''SQLの実行
                query.CommandText = sqlNextVal.ToString()
                Using dt As DataTable = query.GetData()
                    rezid = CType(dt.Rows(0)("REZID"), Long)
                End Using
            End Using
            Try
                '2012/03/03 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加２ START
                '2012/02/27 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加 START
                '2012/02/17 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加 START
                ''SQLの設定
                Dim sqlInsert As New StringBuilder
                sqlInsert.AppendLine("INSERT /* IC3810401_102 */")
                sqlInsert.AppendLine("  INTO TBL_STALLREZINFO (")
                sqlInsert.AppendLine("       DLRCD")
                sqlInsert.AppendLine("     , STRCD")
                sqlInsert.AppendLine("     , REZID")
                'sqlInsert.AppendLine("     , BASREZID")
                sqlInsert.AppendLine("     , STALLID")
                sqlInsert.AppendLine("     , STARTTIME")
                sqlInsert.AppendLine("     , ENDTIME")
                sqlInsert.AppendLine("     , CUSTCD")
                'sqlInsert.AppendLine("     , PERMITID")
                sqlInsert.AppendLine("     , CUSTOMERNAME")
                sqlInsert.AppendLine("     , TELNO")
                sqlInsert.AppendLine("     , MOBILE")
                'sqlInsert.AppendLine("     , EMAIL1")
                sqlInsert.AppendLine("     , VEHICLENAME")
                sqlInsert.AppendLine("     , VCLREGNO")
                'sqlInsert.AppendLine("     , SERVICECODE")
                sqlInsert.AppendLine("     , REZDATE")
                'sqlInsert.AppendLine("     , NETREZID")
                sqlInsert.AppendLine("     , STATUS")
                'sqlInsert.AppendLine("     , INSDID")
                sqlInsert.AppendLine("     , VIN")
                sqlInsert.AppendLine("     , CUSTOMERFLAG")
                'sqlInsert.AppendLine("     , CUSTVCLRE_SEQNO")
                sqlInsert.AppendLine("     , MERCHANDISECD")
                sqlInsert.AppendLine("     , SERVICEMSTCD")
                sqlInsert.AppendLine("     , ZIPCODE")
                sqlInsert.AppendLine("     , ADDRESS")
                sqlInsert.AppendLine("     , MODELCODE")
                'sqlInsert.AppendLine("     , MILEAGE")
                sqlInsert.AppendLine("     , WASHFLG")
                sqlInsert.AppendLine("     , WALKIN")
                sqlInsert.AppendLine("     , SERVICECODE_S")
                'sqlInsert.AppendLine("     , REZ_RECEPTION")
                sqlInsert.AppendLine("     , REZ_WORK_TIME")
                'sqlInsert.AppendLine("     , REZ_PICK_DATE")
                sqlInsert.AppendLine("     , REZ_PICK_LOC")
                sqlInsert.AppendLine("     , REZ_PICK_TIME")
                'sqlInsert.AppendLine("     , REZ_PICK_FIX")
                sqlInsert.AppendLine("     , REZ_DELI_DATE")
                sqlInsert.AppendLine("     , REZ_DELI_LOC")
                sqlInsert.AppendLine("     , REZ_DELI_TIME")
                'sqlInsert.AppendLine("     , REZ_DELI_FIX")
                'sqlInsert.AppendLine("     , UPDATE_COUNT")
                sqlInsert.AppendLine("     , STOPFLG")
                'sqlInsert.AppendLine("     , PREZID")
                'sqlInsert.AppendLine("     , REZCHILDNO")
                'sqlInsert.AppendLine("     , ACTUAL_STIME")
                'sqlInsert.AppendLine("     , ACTUAL_ETIME")
                'sqlInsert.AppendLine("     , CRRY_TYPE")
                'sqlInsert.AppendLine("     , CRRYINTIME")
                'sqlInsert.AppendLine("     , CRRYOUTTIME")
                'sqlInsert.AppendLine("     , MEMO")
                'sqlInsert.AppendLine("     , STOPMEMO")
                sqlInsert.AppendLine("     , STRDATE")
                'sqlInsert.AppendLine("     , NETDEVICESFLG")
                sqlInsert.AppendLine("     , ACCOUNT_PLAN")
                sqlInsert.AppendLine("     , INPUTACCOUNT")
                sqlInsert.AppendLine("     , CANCELFLG")
                sqlInsert.AppendLine("     , CREATEDATE")
                sqlInsert.AppendLine("     , UPDATEDATE")
                sqlInsert.AppendLine("     , UPDATEACCOUNT")
                'sqlInsert.AppendLine("     , SMSFLG")
                sqlInsert.AppendLine("     , REZTYPE")
                'sqlInsert.AppendLine("     , REZFIX")
                'sqlInsert.AppendLine("     , TELEMA_CONTRACT_FLG")
                'sqlInsert.AppendLine("     , DELIVERY_FLG")
                'sqlInsert.AppendLine("     , INSPECTIONFLG")
                sqlInsert.AppendLine("     , CRCUSTID")
                'sqlInsert.AppendLine("     , CUSTOMERCLASS")
                sqlInsert.AppendLine("     , MNTNCD")
                'sqlInsert.AppendLine("     , STALLWAIT_REZID")
                'sqlInsert.AppendLine("     , RESTFLG")
                sqlInsert.AppendLine("     , ORDERNO")
                sqlInsert.AppendLine(")")
                sqlInsert.AppendLine("VALUES (")
                sqlInsert.AppendLine("       :DLRCD")
                sqlInsert.AppendLine("     , :STRCD")
                sqlInsert.AppendLine("     , :REZID")
                'sqlInsert.AppendLine("     , :BASREZID")
                sqlInsert.AppendLine("     , :STALLID")
                sqlInsert.AppendLine("     , :STARTTIME")
                sqlInsert.AppendLine("     , :ENDTIME")
                sqlInsert.AppendLine("     , :CUSTCD")
                'sqlInsert.AppendLine("     , :PERMITID")
                sqlInsert.AppendLine("     , :CUSTOMERNAME")
                sqlInsert.AppendLine("     , :TELNO")
                sqlInsert.AppendLine("     , :MOBILE")
                'sqlInsert.AppendLine("     , :EMAIL1")
                sqlInsert.AppendLine("     , :VEHICLENAME")
                sqlInsert.AppendLine("     , :VCLREGNO")
                'sqlInsert.AppendLine("     , :SERVICECODE")
                sqlInsert.AppendLine("     , :REZDATE")
                'sqlInsert.AppendLine("     , :NETREZID")
                sqlInsert.AppendLine("     , :STATUS")
                'sqlInsert.AppendLine("     , :INSDID")
                sqlInsert.AppendLine("     , :VIN")
                sqlInsert.AppendLine("     , :CUSTOMERFLAG")
                'sqlInsert.AppendLine("     , :CUSTVCLRE_SEQNO")
                sqlInsert.AppendLine("     , :MERCHANDISECD")
                sqlInsert.AppendLine("     , :SERVICEMSTCD")
                sqlInsert.AppendLine("     , :ZIPCODE")
                sqlInsert.AppendLine("     , :ADDRESS")
                sqlInsert.AppendLine("     , :MODELCODE")
                'sqlInsert.AppendLine("     , :MILEAGE")
                sqlInsert.AppendLine("     , :WASHFLG")
                sqlInsert.AppendLine("     , :WALKIN")
                sqlInsert.AppendLine("     , :SERVICECODE_S")
                'sqlInsert.AppendLine("     , :REZ_RECEPTION")
                sqlInsert.AppendLine("     , :REZ_WORK_TIME")
                'sqlInsert.AppendLine("     , :REZ_PICK_DATE")
                sqlInsert.AppendLine("     , :REZ_PICK_LOC")
                sqlInsert.AppendLine("     , :REZ_PICK_TIME")
                'sqlInsert.AppendLine("     , :REZ_PICK_FIX")
                sqlInsert.AppendLine("     , :REZ_DELI_DATE")
                sqlInsert.AppendLine("     , :REZ_DELI_LOC")
                sqlInsert.AppendLine("     , :REZ_DELI_TIME")
                'sqlInsert.AppendLine("     , :REZ_DELI_FIX")
                'sqlInsert.AppendLine("     , :UPDATE_COUNT")
                sqlInsert.AppendLine("     , :STOPFLG")
                'sqlInsert.AppendLine("     , :PREZID")
                'sqlInsert.AppendLine("     , :REZCHILDNO")
                'sqlInsert.AppendLine("     , :ACTUAL_STIME")
                'sqlInsert.AppendLine("     , :ACTUAL_ETIME")
                'sqlInsert.AppendLine("     , :CRRY_TYPE")
                'sqlInsert.AppendLine("     , :CRRYINTIME")
                'sqlInsert.AppendLine("     , :CRRYOUTTIME")
                'sqlInsert.AppendLine("     , :MEMO")
                'sqlInsert.AppendLine("     , :STOPMEMO")
                sqlInsert.AppendLine("     , :STRDATE")
                'sqlInsert.AppendLine("     , :NETDEVICESFLG")
                sqlInsert.AppendLine("     , :ACCOUNT_PLAN")
                sqlInsert.AppendLine("     , :INPUTACCOUNT")
                sqlInsert.AppendLine("     , :CANCELFLG")
                sqlInsert.AppendLine("     , SYSDATE")
                sqlInsert.AppendLine("     , SYSDATE")
                sqlInsert.AppendLine("     , :UPDATEACCOUNT")
                'sqlInsert.AppendLine("     , :SMSFLG")
                sqlInsert.AppendLine("     , :REZTYPE")
                'sqlInsert.AppendLine("     , :REZFIX")
                'sqlInsert.AppendLine("     , :TELEMA_CONTRACT_FLG")
                'sqlInsert.AppendLine("     , :DELIVERY_FLG")
                'sqlInsert.AppendLine("     , :INSPECTIONFLG")
                sqlInsert.AppendLine("     , :CRCUSTID")
                'sqlInsert.AppendLine("     , :CUSTOMERCLASS")
                sqlInsert.AppendLine("     , :MNTNCD")
                'sqlInsert.AppendLine("     , :STALLWAIT_REZID")
                'sqlInsert.AppendLine("     , :RESTFLG")
                sqlInsert.AppendLine("     , :ORDERNO")
                sqlInsert.AppendLine(" )")
                '2012/02/17 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加 END
                '2012/02/27 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加 END
                '2012/03/03 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加２ END

                Using query As New DBUpdateQuery("IC3810401_102")
                    ''SQLの実行
                    query.CommandText = sqlInsert.ToString()
                    '2012/03/03 KN 瀧 【SERVICE_1】引数に車名、モデルコードを追加 START
                    '2012/03/03 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加２ START
                    '2012/02/27 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加 START
                    '2012/02/17 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加 START
                    ''パラメータの設定
                    '販売店コード
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                    '店舗コード
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                    '予約ID
                    query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, rezid)
                    'ストールID
                    query.AddParameterWithTypeValue("STALLID", OracleDbType.Decimal, 0)
                    '使用開始日時→時分秒は0で登録
                    query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, sysDate.Date)
                    '使用終了日時→時分秒は0で登録
                    query.AddParameterWithTypeValue("ENDTIME", OracleDbType.Date, sysDate.Date)
                    '顧客コード
                    If (rowIN.IsCUSTOMERCODENull = False _
                        AndAlso rowIN.CUSTOMERCODE.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("CUSTCD", OracleDbType.NVarchar2, rowIN.CUSTOMERCODE)
                    Else
                        query.AddParameterWithTypeValue("CUSTCD", OracleDbType.NVarchar2, Space(1))
                    End If
                    '顧客氏名
                    If (rowIN.IsCUSTOMERNAMENull = False _
                        AndAlso rowIN.CUSTOMERNAME.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("CUSTOMERNAME", OracleDbType.NVarchar2, rowIN.CUSTOMERNAME)
                    Else
                        query.AddParameterWithTypeValue("CUSTOMERNAME", OracleDbType.NVarchar2, Space(1))
                    End If
                    ''電話番号
                    If (rowIN.IsTELNONull = False _
                        AndAlso rowIN.TELNO.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, rowIN.TELNO)
                    ElseIf (rowCI.IsTELNONull = False _
                        AndAlso rowCI.TELNO.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, rowCI.TELNO)
                    Else
                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, Space(1))
                    End If
                    ''携帯番号
                    If (rowIN.IsMOBILENull = False _
                       AndAlso rowIN.MOBILE.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, rowIN.MOBILE)
                    ElseIf (rowCI.IsMOBILENull = False _
                        AndAlso rowCI.MOBILE.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, rowCI.MOBILE)
                    Else
                        query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, Space(1))
                    End If
                    '車名
                    If (rowIN.IsVEHICLENAMENull = False _
                        AndAlso rowIN.VEHICLENAME.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("VEHICLENAME", OracleDbType.NVarchar2, rowIN.VEHICLENAME)
                    ElseIf (rowVI.IsSERIESNMNull = False _
                        AndAlso rowVI.SERIESNM.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("VEHICLENAME", OracleDbType.NVarchar2, rowVI.SERIESNM)
                    Else
                        query.AddParameterWithTypeValue("VEHICLENAME", OracleDbType.NVarchar2, Space(1))
                    End If
                    '登録ナンバー
                    If (rowIN.IsVCLREGNONull = False _
                        AndAlso rowIN.VCLREGNO.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, rowIN.VCLREGNO)
                    ElseIf (rowVI.IsVCLREGNONull = False _
                        AndAlso rowVI.VCLREGNO.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, rowVI.VCLREGNO)
                    Else
                        query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, Space(1))
                    End If
                    ''予約日時
                    query.AddParameterWithTypeValue("REZDATE", OracleDbType.Date, sysDate)
                    'ステータス→1:ストール本予約
                    query.AddParameterWithTypeValue("STATUS", OracleDbType.Decimal, 1)
                    'VIN
                    If (rowIN.IsVINNull = False _
                        AndAlso rowIN.VIN.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowIN.VIN)
                    ElseIf (rowVI.IsVINNull = False _
                        AndAlso rowVI.VIN.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowVI.VIN)
                    Else
                        query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, Space(1))
                    End If
                    ''識別フラグ→0:自社客
                    query.AddParameterWithTypeValue("CUSTOMERFLAG", OracleDbType.Char, "0")
                    '商品コード
                    If (rowSI.IsMERCHANDISECDNull = False _
                        AndAlso rowSI.MERCHANDISECD.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("MERCHANDISECD", OracleDbType.Char, rowSI.MERCHANDISECD)
                    Else
                        query.AddParameterWithTypeValue("MERCHANDISECD", OracleDbType.Char, Space(1))
                    End If
                    'サービスマスタコード
                    If (rowSI.IsSERVICECDNull = False _
                        AndAlso rowSI.SERVICECD.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("SERVICEMSTCD", OracleDbType.Char, rowSI.SERVICECD)
                    Else
                        query.AddParameterWithTypeValue("SERVICEMSTCD", OracleDbType.Char, Space(1))
                    End If
                    ''郵便番号
                    If (rowCI.IsZIPCODENull = False _
                        AndAlso rowCI.TELNO.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("ZIPCODE", OracleDbType.NVarchar2, rowCI.ZIPCODE)
                    Else
                        query.AddParameterWithTypeValue("ZIPCODE", OracleDbType.NVarchar2, Space(1))
                    End If
                    ''住所
                    If (rowCI.IsADDRESSNull = False _
                        AndAlso rowCI.ADDRESS.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("ADDRESS", OracleDbType.NVarchar2, rowCI.ADDRESS)
                    Else
                        query.AddParameterWithTypeValue("ADDRESS", OracleDbType.NVarchar2, Space(1))
                    End If
                    'モデルコード
                    If (rowIN.IsMODELCODENull = False _
                        AndAlso rowIN.MODELCODE.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, rowIN.MODELCODE)
                    ElseIf (rowVI.IsBASETYPENull = False _
                        AndAlso rowVI.BASETYPE.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, rowVI.BASETYPE)
                    Else
                        query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, Space(1))
                    End If
                    '洗車フラグ
                    If (rowIN.IsWASHFLGNull = False _
                       AndAlso rowIN.WASHFLG.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("WASHFLG", OracleDbType.Char, rowIN.WASHFLG)
                    Else
                        query.AddParameterWithTypeValue("WASHFLG", OracleDbType.Char, "0")
                    End If
                    '来店フラグ→1:来店
                    query.AddParameterWithTypeValue("WALKIN", OracleDbType.Char, "1")
                    'サービスコード(ストール管理用)
                    If (rowSI.IsSERVICECODENull = False _
                       AndAlso rowSI.SERVICECODE.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("SERVICECODE_S", OracleDbType.Char, rowSI.SERVICECODE)
                    Else
                        query.AddParameterWithTypeValue("SERVICECODE_S", OracleDbType.Char, "30")
                    End If
                    '予定_作業時間→サービス所要時間より取得
                    If rowSI.IsSERVICETIMENull = False Then
                        query.AddParameterWithTypeValue("REZ_WORK_TIME", OracleDbType.Decimal, rowSI.SERVICETIME)
                    Else
                        query.AddParameterWithTypeValue("REZ_WORK_TIME", OracleDbType.Decimal, 15)
                    End If
                    '予約_引取_場所
                    query.AddParameterWithTypeValue("REZ_PICK_LOC", OracleDbType.Char, DBNull.Value)
                    '予約_引取_所要時間
                    query.AddParameterWithTypeValue("REZ_PICK_TIME", OracleDbType.Decimal, DBNull.Value)
                    '予約_納車_希望日時時刻
                    If (rowIN.IsDELIVERYDATENull = False _
                        AndAlso rowIN.DELIVERYDATE > Date.MinValue) Then
                        query.AddParameterWithTypeValue("REZ_DELI_DATE", OracleDbType.Char, rowIN.DELIVERYDATE.ToString("yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture))
                    Else
                        query.AddParameterWithTypeValue("REZ_DELI_DATE", OracleDbType.Char, DBNull.Value)
                    End If
                    '予約_納車_場所
                    query.AddParameterWithTypeValue("REZ_DELI_LOC", OracleDbType.Char, DBNull.Value)
                    '予約_納車_所要時間
                    query.AddParameterWithTypeValue("REZ_DELI_TIME", OracleDbType.Decimal, DBNull.Value)
                    '中断フラグ
                    If (Me.IsWalkinUse(rowIN.DLRCD) = False) Then
                        ''WALIN利用フラグがOFFの場合
                        '中断フラグ→2:Temp
                        query.AddParameterWithTypeValue("STOPFLG", OracleDbType.Char, "2")
                    Else
                        ''WALIN利用フラグがONの場合
                        '中断フラグ→5:WALIN
                        query.AddParameterWithTypeValue("STOPFLG", OracleDbType.Char, "5")
                    End If
                    '入庫日時
                    query.AddParameterWithTypeValue("STRDATE", OracleDbType.Date, sysDate)
                    '受付担当予定者
                    query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.Varchar2, rowIN.SACODE)
                    '入力オペレータ
                    query.AddParameterWithTypeValue("INPUTACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
                    'キャンセルフラグ→1:取消し
                    query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.Char, "1")
                    '更新ユーザアカウント
                    query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
                    '予約区分→2:基幹連携(飛び込み客)
                    query.AddParameterWithTypeValue("REZTYPE", OracleDbType.Char, "2")
                    '活動先顧客コード
                    If rowCI.IsORIGINALIDNull = False _
                        AndAlso rowCI.ORIGINALID.Trim.Length > 0 Then
                        query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, rowCI.ORIGINALID)
                    Else
                        query.AddParameterWithTypeValue("CRCUSTID", OracleDbType.Char, Space(1))
                    End If
                    '整備コード
                    If (rowIN.IsMNTNCDNull = False _
                        AndAlso rowIN.MNTNCD.Trim.Length > 0) Then
                        query.AddParameterWithTypeValue("MNTNCD", OracleDbType.NVarchar2, rowIN.MNTNCD)
                    Else
                        query.AddParameterWithTypeValue("MNTNCD", OracleDbType.NVarchar2, DBNull.Value)
                    End If
                    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)
                    '2012/02/17 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加 END
                    '2012/02/27 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加 END
                    '2012/03/03 KN 瀧 【SERVICE_1】ストール予約テーブルの登録項目を追加２ END
                    '2012/03/03 KN 瀧 【SERVICE_1】引数に車名、モデルコードを追加 END

                    query.Execute()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:REZID = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , rezid))
                End Using
            Finally
            End Try

            Return rezid

        End Function

        ''' <summary>
        ''' ストール予約(修正更新)
        ''' </summary>
        ''' <param name="rowIN">予約情報更新引数</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' 2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更
        ''' </history>
        Public Overloads Function UpdateStallOrder(ByVal rowIN As IC3810401DataSet.IC3810401InOrderSaveRow) As Long
            ''引数をログに出力
            Dim args As New List(Of String)
            ' DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            Using query As New DBUpdateQuery("IC3810401_103")
                ''SQLの設定
                Dim sql As New StringBuilder
                sql.AppendLine("UPDATE /* IC3810401_103 */")
                sql.AppendLine("       TBL_STALLREZINFO")
                sql.AppendLine("   SET WASHFLG = :WASHFLG")
                sql.AppendLine("     , REZ_DELI_DATE = :REZ_DELI_DATE")
                sql.AppendLine("     , ACCOUNT_PLAN = :ACCOUNT_PLAN")

                '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 START
                'sql.AppendLine("     , ORDERNO = :ORDERNO")
                '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 END

                sql.AppendLine("     , UPDATEDATE = SYSDATE")
                sql.AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")
                sql.AppendLine(" WHERE DLRCD = :DLRCD")
                sql.AppendLine("   AND STRCD = :STRCD")
                sql.AppendLine("   AND REZID = :REZID")

                ''パラメータの設定
                '洗車フラグ
                If (rowIN.IsWASHFLGNull = False _
                   AndAlso rowIN.WASHFLG.Trim.Length > 0) Then
                    query.AddParameterWithTypeValue("WASHFLG", OracleDbType.Char, rowIN.WASHFLG)
                Else
                    query.AddParameterWithTypeValue("WASHFLG", OracleDbType.Char, "0")
                End If
                '予約_納車_希望日時時刻
                If (rowIN.IsDELIVERYDATENull = False _
                    AndAlso rowIN.DELIVERYDATE > Date.MinValue) Then
                    query.AddParameterWithTypeValue("REZ_DELI_DATE", OracleDbType.Char, rowIN.DELIVERYDATE.ToString("yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture))
                Else
                    query.AddParameterWithTypeValue("REZ_DELI_DATE", OracleDbType.Char, DBNull.Value)
                End If
                '受付担当予定者
                query.AddParameterWithTypeValue("ACCOUNT_PLAN", OracleDbType.Varchar2, rowIN.SACODE)

                '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 START
                'If (rowIN.IsORDERNONull = True) Then
                '    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, DBNull.Value)
                'Else
                '    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)
                'End If
                '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 END
                '更新ユーザアカウント
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)

                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                '店舗コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                '予約ID
                query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, rowIN.REZID)
                ''SQLの実行
                query.CommandText = sql.ToString()
                Dim ret As Integer = query.Execute()
                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                   , Me.GetType.ToString _
                   , System.Reflection.MethodBase.GetCurrentMethod.Name _
                   , ret))
                Return ret
            End Using


        End Function

        ''' <summary>
        ''' ストール予約履歴(新規追加)
        ''' </summary>
        ''' <param name="rowIN">予約情報更新引数</param>
        ''' <returns>追加件数</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' </history>
        Public Function InsertStallHis(ByVal rowIN As IC3810401DataSet.IC3810401InOrderSaveRow, ByVal newHis As Boolean) As Decimal
            ''引数をログに出力
            Dim args As New List(Of String)
            ' DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            Try
                ''SQLの設定
                Dim sql As New StringBuilder
                sql.AppendLine("INSERT /* IC3810401_104 */")
                sql.AppendLine("  INTO TBL_STALLREZHIS (")
                sql.AppendLine("       DLRCD")
                sql.AppendLine("     , STRCD")
                sql.AppendLine("     , REZID")
                sql.AppendLine("     , SEQNO")
                sql.AppendLine("     , UPDDVSID")
                sql.AppendLine("     , STALLID")
                sql.AppendLine("     , STARTTIME")
                sql.AppendLine("     , ENDTIME")
                sql.AppendLine("     , CUSTCD")
                sql.AppendLine("     , PERMITID")
                sql.AppendLine("     , CUSTOMERNAME")
                sql.AppendLine("     , TELNO")
                sql.AppendLine("     , MOBILE")
                sql.AppendLine("     , EMAIL1")
                sql.AppendLine("     , VEHICLENAME")
                sql.AppendLine("     , VCLREGNO")
                sql.AppendLine("     , SERVICECODE")
                sql.AppendLine("     , SERVICECODE_S")
                sql.AppendLine("     , REZDATE")
                sql.AppendLine("     , NETREZID")
                sql.AppendLine("     , STATUS")
                sql.AppendLine("     , INSDID")
                sql.AppendLine("     , VIN")
                sql.AppendLine("     , CUSTOMERFLAG")
                sql.AppendLine("     , CUSTVCLRE_SEQNO")
                sql.AppendLine("     , SERVICEMSTCD")
                sql.AppendLine("     , ZIPCODE")
                sql.AppendLine("     , ADDRESS")
                sql.AppendLine("     , MODELCODE")
                sql.AppendLine("     , MILEAGE")
                sql.AppendLine("     , WASHFLG")
                sql.AppendLine("     , INSPECTIONFLG")
                sql.AppendLine("     , WALKIN")
                sql.AppendLine("     , REZ_RECEPTION")
                sql.AppendLine("     , REZ_WORK_TIME")
                sql.AppendLine("     , REZ_PICK_DATE")
                sql.AppendLine("     , REZ_PICK_LOC")
                sql.AppendLine("     , REZ_PICK_TIME")
                sql.AppendLine("     , REZ_DELI_DATE")
                sql.AppendLine("     , REZ_DELI_LOC")
                sql.AppendLine("     , REZ_DELI_TIME")
                sql.AppendLine("     , UPDATE_COUNT")
                sql.AppendLine("     , STOPFLG")
                sql.AppendLine("     , PREZID")
                sql.AppendLine("     , REZCHILDNO")
                sql.AppendLine("     , ACTUAL_STIME")
                sql.AppendLine("     , ACTUAL_ETIME")
                sql.AppendLine("     , CRRY_TYPE")
                sql.AppendLine("     , CRRYINTIME")
                sql.AppendLine("     , CRRYOUTTIME")
                sql.AppendLine("     , MEMO")
                sql.AppendLine("     , STRDATE")
                sql.AppendLine("     , NETDEVICESFLG")
                sql.AppendLine("     , INPUTACCOUNT")
                sql.AppendLine("     , INFOUPDATEDATE")
                sql.AppendLine("     , INFOUPDATEACCOUNT")
                sql.AppendLine("     , CREATEDATE")
                sql.AppendLine("     , UPDATEDATE")
                sql.AppendLine("     , HIS_FLG")
                sql.AppendLine("     , MERCHANDISECD")
                sql.AppendLine("     , BASREZID")
                sql.AppendLine("     , ACCOUNT_PLAN")
                sql.AppendLine("     , RSSTATUS")
                sql.AppendLine("     , RSDATE")
                sql.AppendLine("     , UPDATESERVER")
                sql.AppendLine("     , REZTYPE")
                sql.AppendLine("     , CRCUSTID")
                sql.AppendLine("     , CUSTOMERCLASS")
                sql.AppendLine("     , STALLWAIT_REZID")
                sql.AppendLine("     , ORDERNO")
                sql.AppendLine(") ")
                sql.AppendLine("SELECT")
                sql.AppendLine("       DLRCD")
                sql.AppendLine("     , STRCD")
                sql.AppendLine("     , REZID")
                If newHis = True Then
                    sql.AppendLine("     , 1")
                Else
                    sql.AppendLine("     , (")
                    sql.AppendLine("       SELECT NVL(MAX(SEQNO) + 1, 1)")
                    sql.AppendLine("         FROM TBL_STALLREZHIS")
                    sql.AppendLine("        WHERE DLRCD = :DLRCD")
                    sql.AppendLine("          AND STRCD = :STRCD")
                    sql.AppendLine("          AND REZID = :REZID")
                    sql.AppendLine("       )")
                End If
                sql.AppendLine("     , '0'")
                sql.AppendLine("     , STALLID")
                sql.AppendLine("     , STARTTIME")
                sql.AppendLine("     , ENDTIME")
                sql.AppendLine("     , CUSTCD")
                sql.AppendLine("     , PERMITID")
                sql.AppendLine("     , CUSTOMERNAME")
                sql.AppendLine("     , TELNO")
                sql.AppendLine("     , MOBILE")
                sql.AppendLine("     , EMAIL1")
                sql.AppendLine("     , VEHICLENAME")
                sql.AppendLine("     , VCLREGNO")
                sql.AppendLine("     , SERVICECODE")
                sql.AppendLine("     , SERVICECODE_S")
                sql.AppendLine("     , REZDATE")
                sql.AppendLine("     , NETREZID")
                sql.AppendLine("     , STATUS")
                sql.AppendLine("     , INSDID")
                sql.AppendLine("     , VIN")
                sql.AppendLine("     , CUSTOMERFLAG")
                sql.AppendLine("     , CUSTVCLRE_SEQNO")
                sql.AppendLine("     , SERVICEMSTCD")
                sql.AppendLine("     , ZIPCODE")
                sql.AppendLine("     , ADDRESS")
                sql.AppendLine("     , MODELCODE")
                sql.AppendLine("     , MILEAGE")
                sql.AppendLine("     , WASHFLG")
                sql.AppendLine("     , INSPECTIONFLG")
                sql.AppendLine("     , WALKIN")
                sql.AppendLine("     , REZ_RECEPTION")
                sql.AppendLine("     , REZ_WORK_TIME")
                sql.AppendLine("     , REZ_PICK_DATE")
                sql.AppendLine("     , REZ_PICK_LOC")
                sql.AppendLine("     , REZ_PICK_TIME")
                sql.AppendLine("     , REZ_DELI_DATE")
                sql.AppendLine("     , REZ_DELI_LOC")
                sql.AppendLine("     , REZ_DELI_TIME")
                sql.AppendLine("     , UPDATE_COUNT")
                sql.AppendLine("     , STOPFLG")
                sql.AppendLine("     , PREZID")
                sql.AppendLine("     , REZCHILDNO")
                sql.AppendLine("     , ACTUAL_STIME")
                sql.AppendLine("     , ACTUAL_ETIME")
                sql.AppendLine("     , CRRY_TYPE")
                sql.AppendLine("     , CRRYINTIME")
                sql.AppendLine("     , CRRYOUTTIME")
                sql.AppendLine("     , MEMO")
                sql.AppendLine("     , STRDATE")
                sql.AppendLine("     , NETDEVICESFLG")
                sql.AppendLine("     , INPUTACCOUNT")
                sql.AppendLine("     , UPDATEDATE")
                sql.AppendLine("     , UPDATEACCOUNT")
                sql.AppendLine("     , SYSDATE")
                sql.AppendLine("     , SYSDATE")
                If newHis = True Then
                    sql.AppendLine("     , '0'")
                Else
                    sql.AppendLine("     , '1'")
                End If
                sql.AppendLine("     , MERCHANDISECD")
                sql.AppendLine("     , BASREZID")
                sql.AppendLine("     , ACCOUNT_PLAN")
                sql.AppendLine("     , '99'")
                sql.AppendLine("     , SYSDATE")
                sql.AppendLine("     , ''")
                sql.AppendLine("     , REZTYPE")
                sql.AppendLine("     , CRCUSTID")
                sql.AppendLine("     , CUSTOMERCLASS")
                sql.AppendLine("     , STALLWAIT_REZID")
                sql.AppendLine("     , ORDERNO")
                sql.AppendLine("  FROM TBL_STALLREZINFO")
                sql.AppendLine(" WHERE DLRCD = :DLRCD")
                sql.AppendLine("   AND STRCD = :STRCD")
                sql.AppendLine("   AND REZID = :REZID")
                Using query As New DBUpdateQuery("IC3810401_104")
                    query.CommandText = sql.ToString()
                    ''パラメータの設定
                    query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                    query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                    query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, rowIN.REZID)
                    ''SQLの実行
                    Dim ret As Integer = query.Execute()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , ret))
                    Return ret
                End Using
            Finally
            End Try

        End Function

        ''' <summary>
        ''' 予約情報更新(修正更新)
        ''' </summary>
        ''' <param name="rowIN">予約情報更新引数</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' 
        ''' <history>
        ''' 2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更
        ''' </history>
        Public Overloads Function UpdateVisitOrder(ByVal rowIN As IC3810401DataSet.IC3810401InOrderSaveRow) As Long
            ''引数をログに出力
            Dim args As New List(Of String)
            ' DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} IN:{2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , String.Join(", ", args.ToArray())))

            ''SQLの設定
            Dim sql As New StringBuilder
            sql.AppendLine("UPDATE /* IC3810401_201 */")
            sql.AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")

            '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 START
            'sql.AppendLine("   SET ORDERNO = :ORDERNO")
            'sql.AppendLine("     , FREZID = :FREZID")
            sql.AppendLine("   SET FREZID = :FREZID")
            '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 END

            sql.AppendLine("     , UPDATEDATE = SYSDATE")
            sql.AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")
            sql.AppendLine("     , UPDATEID = :UPDATEID")

            '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 START
            'sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
            If rowIN.IsVISITSEQNull = False Then
                sql.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
            Else
                sql.AppendLine(" WHERE ORDERNO = :ORDERNO")
            End If
            '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 END

            sql.AppendLine("   AND DLRCD = :DLRCD")
            sql.AppendLine("   AND STRCD = :STRCD")
            sql.AppendLine("   AND SACODE = :SACODE")

            Using query As New DBUpdateQuery("IC3810401_201")
                ''パラメータの設定
                'If (rowIN.IsORDERNONull = True) Then
                '    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, DBNull.Value)
                'Else
                '    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)
                'End If
                If (rowIN.IsREZIDNull = True) Then
                    query.AddParameterWithTypeValue("FREZID", OracleDbType.Decimal, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("FREZID", OracleDbType.Decimal, rowIN.REZID)
                End If
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, rowIN.SYSTEM)

                '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 START
                'query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
                If rowIN.IsVISITSEQNull = False Then
                    query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, rowIN.VISITSEQ)
                Else
                    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowIN.ORDERNO)
                End If
                '2012/02/16 KN 瀧 【SERVICE_1】サービス来店者管理テーブルの更新方法を変更 END

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
                query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, rowIN.SACODE)
                ''SQLの実行
                query.CommandText = sql.ToString()
                Dim ret As Integer = query.Execute()
                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ret))
                Return ret
            End Using

        End Function

        ''' <summary>
        ''' DataRow内の項目を列挙(ログ出力用)
        ''' </summary>
        ''' <param name="args">ログ項目のコレクション</param>
        ''' <param name="row">対象となるDataRow</param>
        ''' <remarks></remarks>
        Private Sub AddLogData(ByVal args As List(Of String), ByVal row As DataRow)
            For Each column As DataColumn In row.Table.Columns
                If row.IsNull(column.ColumnName) = True Then
                    args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
                Else
                    args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, row(column.ColumnName)))
                End If
            Next
        End Sub
    End Class

End Namespace

Partial Class IC3810401DataSet
End Class
