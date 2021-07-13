'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080103DataSet.vb
'─────────────────────────────────────
'機能： 顧客検索 データセット
'補足： 
'作成： 2013/12/20 TMEJ 陳	TMEJ次世代サービス 工程管理機能開発
'更新： 2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正
'更新： 2014/11/26 TMEJ 小澤 次世代サービスタブレット 導入後稼働確認No3
'更新： 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
'更新： 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新： 2016/06/29 NSK 小牟禮 TR-SVT-TMT-20160510-002 TOPSERVとi-CROPの登録番号が異なる
'更新： 2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない
'更新： 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新： 2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示
'更新： 2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策
'更新：
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports System.Globalization
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.CustomerInfo.Search.DataAccess.SC3080103DataSet


Namespace SC3080103DataSetTableAdapters
    Public Class SC3080103DataTableAdapter
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
        ''' OwnerChangeTypeNone
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OwnerChangeTypeNone As String = "0"

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
        ''' <summary>
        ''' 検索条件（3：電話番号、携帯番号）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SearchTypeTelMobile As String = "4"
        ''' <summary>
        ''' 検索条件（4：RO番号）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SearchTypeOrderNo As String = "5"

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
        ''' SAソートタイプ（1：昇順）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SortTypeSAAcs As String = "1"
        ''' <summary>
        ''' SAソートタイプ（2：降順）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SortTypeSADesc As String = "2"
        ''' <summary>
        ''' SCソートタイプ（1：昇順）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SortTypeSCAcs As String = "1"
        ''' <summary>
        ''' SCソートタイプ（2：降順）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SortTypeSCDesc As String = "2"

        '2014/11/26 TMEJ 小澤 次世代サービスタブレット 導入後稼働確認No3 START

        ''' <summary>
        ''' 受付区分（0：予約客）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AcceptanceTypeReserve As String = "0"

        '2014/11/26 TMEJ 小澤 次世代サービスタブレット 導入後稼働確認No3 END

        ''' <summary>
        ''' 受付区分（1：WalkIn）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AcceptanceTypeWalkIn As String = "1"

        ''' <summary>
        ''' ストールID（0：WalkIn用）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallIdWalkIn As Long = 0

        ''' <summary>
        ''' 行追加ステータス（0：追加していない行）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AddRecordTypeOff As String = "0"

        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        ''' <summary>
        ''' 顧客車両区分数
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CstVclTypeCnt As String = "4"

        ''' <summary>
        ''' オーナーチェンジフラグ(0：未設定)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OwnerTypeUnset As String = "0"

        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
        '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        ''' <summary>
        ''' アイコンのフラグ(0：対象外)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOff As String = "0"
        ''' <summary>
        ''' アイコンのフラグ(1：対象内)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOn As String = "1"
        '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

        '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
        ''' <summary>
        ''' IN句に指定できる最大件数:1000
        ''' </summary>
        ''' <remarks></remarks>
        Private Const InQueryMax As Integer = 1000
        '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END

#End Region

#Region "メイン"
        '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
        ' ''' <summary>
        ' ''' SC3080103_001:敬称＋敬称位置＋顧客名を取得
        ' ''' </summary>
        ' ''' <param name="inCstID">顧客ID</param>
        ' ''' <returns>敬称＋敬称位置＋顧客名</returns>
        ' ''' <remarks></remarks>
        'Public Function GetCustomerIDAndNameTitleAndEmail(ByVal inCstID As String) As SC3080103CustomerInfoDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START IN:inCstID = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inCstID))

        '    'データ格納用
        '    Dim dt As SC3080103CustomerInfoDataTable

        '    Dim sql As New StringBuilder

        '    'SQL文作成
        '    With sql
        '        .AppendLine("SELECT /* SC3080103_001 */ ")
        '        .AppendLine("       T1.CST_ID ")
        '        .AppendLine("      ,NVL(TRIM(T2.NAMETITLE_NAME), :SPACE_1) AS NAMETITLE_NAME ")
        '        .AppendLine("      ,NVL(TRIM(CST_EMAIL_1), :SPACE_1) AS CST_EMAIL_1 ")
        '        .AppendLine("      ,NVL(TRIM(T2.POSITION_TYPE), :SPACE_1) AS POSITION_TYPE ")
        '        .AppendLine("   FROM ")
        '        .AppendLine("        TB_M_CUSTOMER  T1 ")
        '        .AppendLine("       ,TB_M_NAMETITLE T2 ")
        '        .AppendLine("   WHERE ")
        '        .AppendLine("       T1.DMS_CST_CD = :CST_ID ")
        '        .AppendLine("   AND T1.NAMETITLE_CD = T2.NAMETITLE_CD(+)")
        '        '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START
        '        .AppendLine("   ORDER BY T1.DMS_TAKEIN_DATETIME DESC ")
        '        '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END
        '    End With

        '    Using query As New DBSelectQuery(Of SC3080103CustomerInfoDataTable)("SC3080103_001")
        '        query.CommandText = sql.ToString()
        '        'バインド変数
        '        query.AddParameterWithTypeValue("CST_ID", OracleDbType.NVarchar2, inCstID)
        '        query.AddParameterWithTypeValue("SPACE_1", OracleDbType.NVarchar2, Space(1))

        '        'データ取得
        '        dt = query.GetData()

        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} END OUT:COUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
        '    Return dt

        'End Function


        ' ''' <summary>
        ' ''' SC3080103_002:SA名を取得
        ' ''' </summary>
        ' ''' <param name="inUserCD">SAユーザID</param>
        ' ''' <returns>SA名</returns>
        ' ''' <remarks></remarks>
        'Public Function GetSAName(ByVal inUserCD As String) As SC3080103CustomerInfoDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START IN:inUserCD = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inUserCD))

        '    'データ格納用
        '    Dim dt As SC3080103CustomerInfoDataTable

        '    Dim sql As New StringBuilder

        '    'SQL文作成
        '    With sql
        '        .AppendLine("SELECT /* SC3080103_002 */ ")
        '        .AppendLine("       NVL(TRIM(USERNAME), :SPACE_1) AS SA ")
        '        .AppendLine("  FROM ")
        '        .AppendLine("       TBL_USERS ")
        '        .AppendLine("  WHERE ")
        '        .AppendLine("       ACCOUNT = :ACCOUNT ")
        '    End With

        '    Using query As New DBSelectQuery(Of SC3080103CustomerInfoDataTable)("SC3080103_002")
        '        query.CommandText = sql.ToString()
        '        'バインド変数
        '        query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inUserCD)
        '        query.AddParameterWithTypeValue("SPACE_1", OracleDbType.NVarchar2, Space(1))

        '        'データ取得
        '        dt = query.GetData()

        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} END OUT:COUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
        '    Return dt

        'End Function

        ' ''' <summary>
        ' ''' SC3080103_003:SC名を取得
        ' ''' </summary>
        ' ''' <param name="inUserCD">SCユーザID</param>
        ' ''' <returns>SC名</returns>
        ' ''' <remarks></remarks>
        'Public Function GetSCName(ByVal inUserCD As String) As SC3080103CustomerInfoDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START IN:inUserCD = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inUserCD))

        '    'データ格納用
        '    Dim dt As SC3080103CustomerInfoDataTable

        '    Dim sql As New StringBuilder

        '    'SQL文作成
        '    With sql
        '        .AppendLine("SELECT /* SC3080103_003 */ ")
        '        .AppendLine("       NVL(TRIM(USERNAME), :SPACE_1) AS SC ")
        '        .AppendLine("  FROM ")
        '        .AppendLine("       TBL_USERS ")
        '        .AppendLine("  WHERE ")
        '        .AppendLine("       ACCOUNT = :ACCOUNT ")
        '    End With

        '    Using query As New DBSelectQuery(Of SC3080103CustomerInfoDataTable)("SC3080103_003")
        '        query.CommandText = sql.ToString()
        '        'バインド変数
        '        query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, inUserCD)
        '        query.AddParameterWithTypeValue("SPACE_1", OracleDbType.NVarchar2, Space(1))

        '        'データ取得
        '        dt = query.GetData()

        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} END OUT:COUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
        '    Return dt

        'End Function

        ' ''' <summary>
        ' ''' SC3080103_004:モデル名を取得
        ' ''' </summary>
        ' ''' <param name="inModelCD">モデルコード</param>
        ' ''' <param name="inVclID">VCLID</param>
        ' ''' <returns>モデル名</returns>
        ' ''' <remarks></remarks>
        'Public Function GetModelName(ByVal inModelCD As String, _
        '                             ByVal inVclID As Decimal) As SC3080103CustomerInfoDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START IN:inModelCD = {2}, inVclID = {3}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inModelCD, inVclID))

        '    'データ格納用
        '    Dim dt As SC3080103CustomerInfoDataTable

        '    Dim sql As New StringBuilder

        '    'SQL文作成
        '    With sql
        '        .AppendLine("SELECT /* SC3080103_004 */ ")
        '        .AppendLine("       NVL(TRIM(A1.MODEL_NAME), NVL(TRIM(A2.NEWCST_MODEL_NAME), :SPACE_1 )) AS MODEL_NAME  ")
        '        .AppendLine("  FROM ")
        '        .AppendLine("        TB_M_MODEL   A1 ")
        '        .AppendLine("       ,TB_M_VEHICLE A2 ")
        '        .AppendLine("  WHERE ")
        '        .AppendLine("           A2.MODEL_CD = A1.MODEL_CD(+)  ")
        '        .AppendLine("       AND A2.VCL_ID   = :VCL_ID  ")
        '        .AppendLine("       AND A1.MODEL_CD = :MODEL_CD ")
        '    End With

        '    Using query As New DBSelectQuery(Of SC3080103CustomerInfoDataTable)("SC3080103_004")
        '        query.CommandText = sql.ToString()
        '        'バインド変数
        '        query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVclID)
        '        query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, inModelCD)
        '        query.AddParameterWithTypeValue("SPACE_1", OracleDbType.NVarchar2, Space(1))

        '        'データ取得
        '        dt = query.GetData()

        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} END OUT:COUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
        '    Return dt

        'End Function


        ' ''' <summary>
        ' ''' SC3080103_005:エリア名
        ' ''' </summary>
        ' ''' <param name="inDlrCD">販売店コード</param>
        ' ''' <param name="inVclID">車両ID</param>
        ' ''' <returns>エリア名</returns>
        ' ''' <remarks></remarks>
        'Public Function GetAreaName(ByVal inDlrCD As String, _
        '                            ByVal inVclID As Decimal) As SC3080103CustomerInfoDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START IN:inDlrCD = {2}, inVclID = {3}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inDlrCD, inVclID))

        '    'データ格納用
        '    Dim dt As SC3080103CustomerInfoDataTable

        '    Dim sql As New StringBuilder

        '    'SQL文作成
        '    With sql
        '        .AppendLine("SELECT /* SC3080103_005 */ ")
        '        .AppendLine("       NVL(REG_AREA_NAME, :SPACE_1) AS REG_AREA_NAME  ")
        '        .AppendLine("  FROM ")
        '        .AppendLine("        TB_M_VEHICLE_DLR   A1 ")
        '        .AppendLine("       ,TB_M_REG_AREA A2 ")
        '        .AppendLine("  WHERE ")
        '        .AppendLine("           A2.REG_AREA_CD = A1.REG_AREA_CD  ")
        '        .AppendLine("       AND A1.DLR_CD = :DLR_CD  ")
        '        .AppendLine("       AND A1.VCL_ID = :VCL_ID ")
        '    End With

        '    Using query As New DBSelectQuery(Of SC3080103CustomerInfoDataTable)("SC3080103_005")
        '        query.CommandText = sql.ToString()
        '        'バインド変数
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDlrCD)
        '        query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVclID)
        '        query.AddParameterWithTypeValue("SPACE_1", OracleDbType.NVarchar2, Space(1))

        '        'データ取得
        '        dt = query.GetData()

        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} END OUT:COUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
        '    Return dt

        'End Function

        ' ''' <summary>
        ' ''' SC3080103_006:イメージ取得
        ' ''' </summary>
        ' ''' <param name="inDealerCode">店舗コード</param>
        ' ''' <param name="inCstID">顧客ID</param>
        ' ''' <returns>イメージ</returns>
        ' ''' <remarks></remarks>
        'Public Function GetImageFileSmall(ByVal inDealerCode As String, _
        '                                       ByVal inCstID As Decimal) As SC3080103CustomerInfoDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} START IN:inDealerCode = {2}, inCstID = {3}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inDealerCode, inCstID))

        '    'データ格納用
        '    Dim dt As SC3080103CustomerInfoDataTable

        '    Dim sql As New StringBuilder

        '    'SQL文作成
        '    With sql
        '        .AppendLine("SELECT /* SC3080103_006 */ ")
        '        .AppendLine("       NVL(TRIM(IMG_FILE_SMALL), :SPACE_1) AS IMG_FILE ")
        '        .AppendLine("  FROM ")
        '        .AppendLine("       TB_M_CUSTOMER_DLR ")
        '        .AppendLine("  WHERE ")
        '        .AppendLine("           CST_ID = :CST_ID ")
        '        .AppendLine("       AND DLR_CD = :DLR_CD ")
        '    End With

        '    Using query As New DBSelectQuery(Of SC3080103CustomerInfoDataTable)("SC3080103_006")
        '        query.CommandText = sql.ToString()
        '        'バインド変数
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
        '        query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, inCstID)
        '        query.AddParameterWithTypeValue("SPACE_1", OracleDbType.NVarchar2, Space(1))

        '        'データ取得
        '        dt = query.GetData()

        '    End Using

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} END OUT:COUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))
        '    Return dt

        'End Function
        '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END

        ''' <summary>
        ''' SC3080103_008:RO作成日
        ''' </summary>
        ''' <param name="inSvcinID">入庫ID</param>
        ''' <returns>RO作成日</returns>
        ''' <remarks></remarks>
        Public Function GetROCreateDate(ByVal inSvcinID As Decimal) As Date

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inSvcinID = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inSvcinID.ToString(CultureInfo.CurrentCulture)))

            'データ格納用
            Dim dt As SC3080103CustomerInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine("SELECT /* SC3080103_008 */ ")
                .AppendLine("        A2.RO_CREATE_DATETIME  ")
                .AppendLine("  FROM ")
                .AppendLine("        TBL_SERVICE_VISIT_MANAGEMENT A1, ")
                .AppendLine("        TB_T_RO_INFO A2 ")
                .AppendLine("  WHERE ")
                .AppendLine("        A2.VISIT_ID = A1.VISITSEQ  ")
                .AppendLine("    AND A1.REZID = :REZID  ")
            End With

            Using query As New DBSelectQuery(Of SC3080103CustomerInfoDataTable)("SC3080103_008")
                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, inSvcinID)

                'データ取得
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            If dt.Count <= 0 Then
                Return Nothing
            End If

            If dt(0).IsRO_CREATE_DATETIMENull Then
                Return Nothing
            End If

            Return dt(0).RO_CREATE_DATETIME

        End Function

        '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
        ' ''' <summary>
        ' ''' SC3080103_009:予約状況チェック
        ' ''' </summary>
        ' ''' <param name="inDealerCode">販売店コード</param>
        ' ''' <param name="inBranchCode">店舗コード</param>
        ' ''' <param name="inCstID">VIN</param>
        ' ''' <param name="inVclID">車両Number</param>
        ' ''' <param name="inNowDate">現在日時</param>
        ' ''' <returns>顧客IDと車両ID取得（予約有）</returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' ''' 2014/11/26 TMEJ 小澤 次世代サービスタブレット 導入後稼働確認No3
        ' ''' 2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ' ''' </history>
        'Public Function GetApointmentCst(ByVal inDealerCode As String, _
        '                                       ByVal inBranchCode As String, _
        '                                       ByVal inCstID As Decimal, _
        '                                       ByVal inVclID As Decimal, _
        '                                       ByVal inNowDate As Date) As Boolean
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} " _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '    Using query As New DBSelectQuery(Of SC3080103CustomerInfoDataTable)("SC3080103_009")
        '        'SQLの設定
        '        Dim sql As New StringBuilder
        '        With sql
        '            .AppendLine("SELECT /* SC3080103_009 */ ")
        '            .AppendLine("         1 ")
        '            .AppendLine(" FROM TB_T_SERVICEIN T1 ")
        '            .AppendLine("     ,TB_T_JOB_DTL T2 ")
        '            .AppendLine("     ,TB_T_STALL_USE T3 ")

        '            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        '            .AppendLine("     ,TB_M_CUSTOMER_VCL T4 ")

        '            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        '            .AppendLine(" WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
        '            .AppendLine("  AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")

        '            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        '            .AppendLine("  AND T1.DLR_CD = T4.DLR_CD ")
        '            .AppendLine("  AND T1.CST_ID = T4.CST_ID ")
        '            .AppendLine("  AND T1.VCL_ID = T4.VCL_ID ")

        '            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        '            .AppendLine("  AND T1.DLR_CD = :DLR_CD ")
        '            .AppendLine("  AND T1.BRN_CD = :BRN_CD ")
        '            .AppendLine("  AND NOT EXISTS (SELECT 1 ")
        '            .AppendLine("                    FROM TB_T_SERVICEIN M1 ")
        '            .AppendLine("                   WHERE M1.SVCIN_ID = T1.SVCIN_ID ")
        '            .AppendLine("                     AND M1.SVC_STATUS = :SVC_STATUS_02) ")
        '            .AppendLine("  AND NOT EXISTS (SELECT 1 ")
        '            .AppendLine("                    FROM TB_T_SERVICEIN D1 ")
        '            .AppendLine("                        ,TB_T_JOB_DTL D2 ")
        '            .AppendLine("                        ,TB_T_STALL_USE D3 ")
        '            .AppendLine("                   WHERE D1.SVCIN_ID = D2.SVCIN_ID ")
        '            .AppendLine("                     AND D2.JOB_DTL_ID = D3.JOB_DTL_ID ")
        '            .AppendLine("                     AND D1.SVCIN_ID = T1.SVCIN_ID ")
        '            .AppendLine("                     AND D2.JOB_DTL_ID = T2.JOB_DTL_ID ")
        '            .AppendLine("                     AND D3.STALL_USE_ID = T3.STALL_USE_ID ")
        '            .AppendLine("                     AND D1.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_1 ")
        '            .AppendLine("                     AND D3.STALL_ID = :STALL_ID_0) ")
        '            .AppendLine("  AND T2.DLR_CD = :DLR_CD ")
        '            .AppendLine("  AND T2.BRN_CD = :BRN_CD ")
        '            .AppendLine("  AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
        '            .AppendLine("  AND T3.DLR_CD = :DLR_CD ")
        '            .AppendLine("  AND T3.BRN_CD = :BRN_CD ")

        '            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        '            .AppendLine("  AND T4.DLR_CD = :DLR_CD ")
        '            .AppendLine("  AND T4.VCL_ID = :VCL_ID ")
        '            .AppendLine("  AND T4.CST_VCL_TYPE <> :CST_VCL_TYPE_FLG_4 ")
        '            .AppendLine("  AND T4.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")

        '            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        '            '2014/11/26 TMEJ 小澤 次世代サービスタブレット 導入後稼働確認No3 START

        '            '.AppendLine("  AND CASE WHEN T3.RSLT_START_DATETIME <> :MINDATE THEN T3.RSLT_START_DATETIME ")
        '            '.AppendLine("           ELSE T3.SCHE_START_DATETIME END >= TRUNC(:NOWDATE) ")
        '            '.AppendLine("  AND CASE WHEN T3.RSLT_START_DATETIME <> :MINDATE THEN T3.RSLT_START_DATETIME ")
        '            '.AppendLine("           ELSE T3.SCHE_START_DATETIME END <= TRUNC(:MAXDATE) ")

        '            .AppendLine("  AND T1.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_0 ")
        '            .AppendLine("  AND T3.SCHE_START_DATETIME >= TRUNC(:NOWDATE) ")
        '            .AppendLine("  AND T3.SCHE_START_DATETIME <= TRUNC(:MAXDATE) ")
        '            .AppendLine("  AND T3.RSLT_START_DATETIME = :MINDATE ")

        '            '2014/11/26 TMEJ 小澤 次世代サービスタブレット 導入後稼働確認No3 END

        '            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        '            '$03 コメントアウト修正
        '            '.AppendLine("  AND T1.CST_ID = :CST_ID ")

        '            '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        '            .AppendLine("  AND T1.VCL_ID = :VCL_ID ")
        '        End With

        '        query.CommandText = sql.ToString()

        '        'パラメータの設定

        '        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        '        '$03 修正にてバインド不要のためコメントアウト
        '        'query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, inCstID)

        '        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        '        query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVclID)
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
        '        query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
        '        query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)

        '        '2014/11/26 TMEJ 小澤 次世代サービスタブレット 導入後稼働確認No3 START

        '        'query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_1", OracleDbType.NVarchar2, CancelTypeEffective)

        '        query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_1", OracleDbType.NVarchar2, AcceptanceTypeWalkIn)
        '        query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_0", OracleDbType.NVarchar2, AcceptanceTypeReserve)

        '        '2014/11/26 TMEJ 小澤 次世代サービスタブレット 導入後稼働確認No3 END

        '        query.AddParameterWithTypeValue("STALL_ID_0", OracleDbType.Long, StallIdWalkIn)
        '        query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
        '        query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
        '        query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
        '        query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)

        '        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        '        query.AddParameterWithTypeValue("CST_VCL_TYPE_FLG_4", OracleDbType.NVarchar2, CstVclTypeCnt)
        '        query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, OwnerTypeUnset)

        '        '2015/09/01 TMEJ 井上 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        '        ''SQLの実行
        '        Using dt As SC3080103CustomerInfoDataTable = query.GetData()
        '            ''終了ログの出力
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , dt.Rows.Count))

        '            Return dt.Count > 0

        '        End Using
        '    End Using
        'End Function

        ' ''' <summary>
        ' ''' SC3080103_010:車両ID取得
        ' ''' </summary>
        ' ''' <param name="inDealerCode">販売店コード</param>
        ' ''' <param name="inVin">VIN</param>
        ' ''' <param name="inRegNum">車両Number</param>
        ' ''' <returns>車両ID取得</returns>
        ' ''' <remarks></remarks>
        'Public Function GetVCLID(ByVal inDealerCode As String, _
        '                                       ByVal inVin As String, _
        '                                       ByVal inRegNum As String) As SC3080103CustomerInfoDataTable
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} " _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '    Using query As New DBSelectQuery(Of SC3080103CustomerInfoDataTable)("SC3080103_010")
        '        'SQLの設定
        '        Dim sql As New StringBuilder
        '        With sql
        '            .AppendLine("SELECT /* SC3080103_010*/ ")
        '            .AppendLine("       Q1.CST_ID ")
        '            .AppendLine("      ,Q1.VCL_ID ")
        ''2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
        '            .AppendLine("      ,Q1.SSC_MARK ")
        ''2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
        ''2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        '            .AppendLine("      ,Q1.PL_MARK")
        '            .AppendLine("      ,Q1.MB_MARK")
        '            .AppendLine("      ,Q1.E_MARK")
        '            .AppendLine("      ,Q1.TLM_MBR_FLG")
        ''2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        '            .AppendLine("FROM  ")
        '            .AppendLine("(  ")
        '            .AppendLine("    SELECT  ")
        '            .AppendLine("           T1.CST_ID ")
        '            .AppendLine("          ,T1.VCL_ID ")
        ''2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
        '            .AppendLine("          ,T2.SPECIAL_CAMPAIGN_TGT_FLG AS SSC_MARK ")
        ''2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END
        ''2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        '            .AppendLine("          ,NVL(TRIM(T3.IMP_VCL_FLG), :ICON_FLAG_OFF )AS PL_MARK")
        '            .AppendLine("          ,NVL(TRIM(T6.SML_AMC_FLG), :ICON_FLAG_OFF )AS MB_MARK")
        '            .AppendLine("          ,NVL(TRIM(T6.EW_FLG), :ICON_FLAG_OFF)AS E_MARK")
        '            .AppendLine("          ,CASE ")
        '            .AppendLine("                 WHEN T7.VCL_VIN IS NULL THEN :ICON_FLAG_OFF")
        '            .AppendLine("                 ELSE :ICON_FLAG_ON")
        '            .AppendLine("           END AS TLM_MBR_FLG")
        ''2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        '            .AppendLine("      FROM ")
        '            .AppendLine("           TB_M_CUSTOMER_VCL T1 ")
        '            .AppendLine("          ,TB_M_VEHICLE T2 ")
        '            .AppendLine("          ,TB_M_VEHICLE_DLR T3 ")
        '            .AppendLine("          ,TB_M_CUSTOMER_DLR T4 ")
        '            .AppendLine("          ,TB_M_CUSTOMER T5 ")
        ''2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        '            .AppendLine("          ,TB_LM_VEHICLE T6")
        '            .AppendLine("          ,TB_LM_TLM_MEMBER T7")
        ''2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        '            .AppendLine("     WHERE ")
        '            .AppendLine("           T1.VCL_ID = T2.VCL_ID ")
        '            .AppendLine("       AND T1.DLR_CD = T3.DLR_CD ")
        '            .AppendLine("       AND T2.VCL_ID = T3.VCL_ID ")
        '            .AppendLine("       AND T1.DLR_CD = T4.DLR_CD ")
        '            .AppendLine("       AND T1.CST_ID = T4.CST_ID ")
        '            .AppendLine("       AND T1.CST_ID = T5.CST_ID ")
        ''2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        '            .AppendLine("       AND T2.VCL_ID = T6.VCL_ID(+)")
        '            .AppendLine("       AND T2.VCL_VIN = T7.VCL_VIN(+)")
        '2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END
        '            .AppendLine("       AND T1.DLR_CD = :DLR_CD ")
        '            .AppendLine("       AND T1.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")
        '            ' 2016/06/30 NSK 小牟禮 TR-SVT-TMT-20160510-002 TOPSERVとi-CROPの登録番号が異なる START
        '            ' VINで一致するが車両登録番号が異なる場合でも紐付け可能となるように条件を修正
        '            '.AppendLine("       AND ((T2.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) AND T3.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH)) OR ")
        '            .AppendLine("       AND ((T2.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH)) OR ")
        '            ' 2016/06/30 NSK 小牟禮 TR-SVT-TMT-20160510-002 TOPSERVとi-CROPの登録番号が異なる END
        '            .AppendLine("            (T2.VCL_VIN_SEARCH = UPPER(:VCL_VIN_SEARCH) AND T3.REG_NUM_SEARCH = N' ') OR ")
        '            .AppendLine("            (T2.VCL_VIN_SEARCH = N' ' AND T3.REG_NUM_SEARCH = UPPER(:REG_NUM_SEARCH))) ")
        '            .AppendLine("     ORDER BY T5.DMS_TAKEIN_DATETIME DESC ")
        '            .AppendLine("             ,T4.CST_TYPE ASC ")
        '            .AppendLine("             ,T3.REG_NUM DESC ")
        '            .AppendLine("             ,T2.VCL_VIN DESC ")
        '            .AppendLine("             ,T2.VCL_ID DESC ")
        '            .AppendLine(") Q1   ")
        '            .AppendLine("WHERE  ")
        '            .AppendLine("       ROWNUM = 1 ")
        '        End With

        '        query.CommandText = sql.ToString()

        '        'パラメータの設定
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
        '        query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, OwnerChangeTypeNone)
        '        query.AddParameterWithTypeValue("VCL_VIN_SEARCH", OracleDbType.NVarchar2, inVin)
        '        query.AddParameterWithTypeValue("REG_NUM_SEARCH", OracleDbType.NVarchar2, inRegNum)
        ''2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 START
        '        query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
        '        query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)
        ''2018/06/22 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、Lマークなどを表示 END

        '        ''SQLの実行
        '        Using dt As SC3080103CustomerInfoDataTable = query.GetData()
        '            ''終了ログの出力
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , dt.Rows.Count))

        '            Return dt

        '        End Using
        '    End Using
        'End Function
        '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END

        ''' <summary>
        ''' SC3080103_011:RO作業連番を取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inOrderNo">RO番号</param>
        ''' <returns>RO作業連番</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正
        ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
        ''' </history>
        Public Function GetROJobSeq(ByVal inDealerCode As String, _
                                    ByVal inBranchCode As String, _
                                    ByVal inOrderNo As String) As String

            '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 START

            'Public Function GetROJobSeq(ByVal inRONUM As String) As String

            '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 END

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START

            'Public Function GetROJobSeq(ByVal inDealerCode As String, _
            '                            ByVal inBranchCode As String, _
            '                            ByVal inRONUM As String) As String

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inDealerCode = {2},inBranchCode = {3},inOrderNo = {4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inBranchCode, inOrderNo))

            'データ格納用
            Dim dt As SC3080103CustomerInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql

                '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 START

                '.AppendLine("SELECT /* SC3080103_011*/ ")
                '.AppendLine("        MIN(A1.RO_SEQ) AS RO_JOB_SEQ  ")
                '.AppendLine("  FROM ")
                '.AppendLine("        TB_T_RO_INFO   A1 ")
                '.AppendLine("  WHERE ")
                '.AppendLine("        A1.RO_NUM = :RO_NUM  ")

                .AppendLine("SELECT /* SC3080103_011 */ ")
                .AppendLine("       MIN(A1.RO_SEQ) AS RO_JOB_SEQ ")
                .AppendLine("  FROM ")
                .AppendLine("       TB_T_RO_INFO A1 ")
                .AppendLine(" WHERE ")
                .AppendLine("       A1.DLR_CD = :DLR_CD ")
                .AppendLine("   AND A1.BRN_CD = :BRN_CD ")
                .AppendLine("   AND A1.RO_NUM = :RO_NUM ")

                '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 END

            End With

            Using query As New DBSelectQuery(Of SC3080103CustomerInfoDataTable)("SC3080103_011")
                query.CommandText = sql.ToString()
                'バインド変数

                '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 START

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)

                '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 END

                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRONUM)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inOrderNo)
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                'データ取得
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            If dt.Count <= 0 Then
                Return Nothing
            End If

            If dt(0).IsRO_JOB_SEQNull Then
                Return Nothing
            End If

            Return dt(0).RO_JOB_SEQ

        End Function

        ''' <summary>
        ''' SC3080103_012:予約情報取得
        ''' </summary>
        ''' <param name="inSvcId">入庫ID</param>
        ''' <returns>予約情報</returns>
        ''' <remarks></remarks>
        ''' <hitory></hitory>
        Public Function GetStallUseInfo(ByVal inSvcId As Decimal) As SC3080103StallUseInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inSvcId = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inSvcId.ToString(CultureInfo.CurrentCulture)))

            Dim dt As SC3080103StallUseInfoDataTable

            Using query As New DBSelectQuery(Of SC3080103StallUseInfoDataTable)("SC3080103_012")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("SELECT /* SC3080103_012 */ ")
                    .AppendLine("       1 ")
                    .AppendLine("  FROM ")
                    .AppendLine("       TB_T_SERVICEIN T1 ")
                    .AppendLine(" WHERE ")
                    .AppendLine("       T1.SVCIN_ID = :SVCIN_ID ")
                    .AppendLine("   AND T1.SVC_STATUS IN ('00','01','03','04','05' ) ")
                End With

                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, inSvcId)

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
        ''' SC3080103_013:顧客情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inCustomerId">顧客ID</param>
        ''' <param name="inVehiceleId">車両ID</param>
        ''' <returns>顧客情報</returns>
        ''' <remarks></remarks>
        ''' <hitory></hitory>
        Public Function GetCustomerInfo(ByVal inDealerCode As String, _
                                        ByVal inCustomerId As Decimal, _
                                        ByVal inVehiceleId As Decimal) As SC3080103CustomerInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inDealerCode = {2}, inCustomerId = {3}, inVehiceleId = {4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inCustomerId.ToString(CultureInfo.CurrentCulture) _
                        , inVehiceleId.ToString(CultureInfo.CurrentCulture)))

            Dim dt As SC3080103CustomerInfoDataTable

            Using query As New DBSelectQuery(Of SC3080103CustomerInfoDataTable)("SC3080103_013")
                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .AppendLine("SELECT /* SC3080103_013 */ ")
                    .AppendLine("       A1.DLR_CD ")
                    .AppendLine("      ,A2.DMS_CST_CD AS DMS_CST_CD ")
                    .AppendLine("      ,A2.CST_NAME AS CST_NAME ")
                    .AppendLine("      ,A5.REG_NUM AS REG_NUM ")
                    .AppendLine("      ,A4.VCL_VIN AS VCL_VIN ")
                    .AppendLine("      ,A4.VCL_KATASHIKI AS VCL_KATASHIKI ")
                    .AppendLine("      ,A2.CST_PHONE AS CST_PHONE ")
                    .AppendLine("      ,A2.CST_MOBILE AS CST_MOBILE ")
                    .AppendLine("      ,CASE  ")
                    .AppendLine("       WHEN    ")
                    .AppendLine("            A6.CST_ID IS NULL   ")
                    .AppendLine("            THEN A7.CST_TYPE  ")
                    .AppendLine("            ELSE N'1'     ")
                    .AppendLine("       END AS CST_TYPE  ")
                    .AppendLine("  FROM ")
                    .AppendLine("       TB_M_CUSTOMER_VCL A1 ")
                    .AppendLine("      ,TB_M_CUSTOMER A2 ")
                    .AppendLine("      ,TB_M_VEHICLE A4 ")
                    .AppendLine("      ,TB_M_VEHICLE_DLR A5 ")
                    .AppendLine("      ,TBL_SERVICEIN_APPEND A6 ")
                    .AppendLine("      ,TB_M_CUSTOMER_DLR A7 ")
                    .AppendLine(" WHERE ")
                    .AppendLine("       A1.CST_ID = A2.CST_ID ")
                    .AppendLine("   AND A1.VCL_ID = A4.VCL_ID ")
                    .AppendLine("   AND A1.DLR_CD = A5.DLR_CD ")
                    .AppendLine("   AND A1.VCL_ID = A5.VCL_ID ")
                    .AppendLine("   AND A1.CST_ID = A6.CST_ID(+) ")
                    .AppendLine("   AND A1.CST_ID = A7.CST_ID ")
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
        ''' SC3080103_014:RO存在するかチェック
        ''' </summary>
        ''' <param name="inVisitSeq">来店番号</param>
        ''' <remarks></remarks>
        Public Function CheckRoExists(ByVal inVisitSeq As Decimal) As Boolean
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Using query As New DBSelectQuery(Of SC3080103CustomerInfoDataTable)("SC3080103_014")
                'SQLの設定
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("SELECT /* SC3080103_014 */ ")
                    .AppendLine("         1 ")
                    .AppendLine(" FROM TB_T_RO_INFO T1 ")
                    .AppendLine(" WHERE T1.VISIT_ID = :VISIT_ID ")
                End With

                query.CommandText = sql.ToString()

                'パラメータの設定
                query.AddParameterWithTypeValue("VISIT_ID", OracleDbType.Decimal, inVisitSeq)

                'SQLの実行
                Using dt As SC3080103CustomerInfoDataTable = query.GetData()
                    ''終了ログの出力
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count))

                    Return dt.Count > 0

                End Using
            End Using
        End Function

        ''' <summary>
        ''' SC3080103_015:ROステータスコード取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <param name="inOrderNo">RO番号</param>
        ''' <returns>ROステータスコード</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正
        ''' 2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発
        ''' </history>
        Public Function GetROStatusCode(ByVal inDealerCode As String, _
                                        ByVal inBranchCode As String, _
                                        ByVal inOrderNo As String) As String

            '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 START

            'Public Function GetROStatusCode(ByVal inRONUM As String) As String

            '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 END

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START

            'Public Function GetROStatusCode(ByVal inDealerCode As String, _
            '                            ByVal inBranchCode As String, _
            '                            ByVal inRONUM As String) As String

            '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inDealerCode = {2},inBranchCode = {3},inOrderNo = {4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inBranchCode, inOrderNo))

            'データ格納用
            Dim dt As SC3080103ReserveInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql

                '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 START

                '.AppendLine("SELECT /* SC3080103_015*/ ")
                '.AppendLine("        MIN(A1.RO_STATUS) AS RO_STATUS_CODE  ")
                '.AppendLine("  FROM ")
                '.AppendLine("        TB_T_RO_INFO   A1 ")
                '.AppendLine("  WHERE ")
                '.AppendLine("        A1.RO_NUM = :RO_NUM  ")

                .AppendLine("SELECT /* SC3080103_015 */ ")
                .AppendLine("       MIN(A1.RO_STATUS) AS RO_STATUS_CODE ")
                .AppendLine("  FROM ")
                .AppendLine("       TB_T_RO_INFO   A1 ")
                .AppendLine(" WHERE ")
                .AppendLine("       A1.DLR_CD = :DLR_CD ")
                .AppendLine("   AND A1.BRN_CD = :BRN_CD ")
                .AppendLine("   AND A1.RO_NUM = :RO_NUM ")

                '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 END

            End With

            Using query As New DBSelectQuery(Of SC3080103ReserveInfoDataTable)("SC3080103_015")
                query.CommandText = sql.ToString()
                'バインド変数

                '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 START

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)

                '2014/09/16 TMEJ 小澤 BTS不具合対応 RO_INFO検索時は販売店と店舗も条件に入れるように修正 END

                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
                'query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inRONUM)
                query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, inOrderNo)
                '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

                'データ取得
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Rows.Count.ToString(CultureInfo.CurrentCulture)))

            If dt.Count <= 0 Then
                Return Nothing
            End If

            If dt(0).IsRO_STATUS_CODENull Then
                Return Nothing
            End If

            Return dt(0).RO_STATUS_CODE

        End Function

        '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START

        ''' <summary>
        ''' SC3080103_016:来店情報を取得
        ''' </summary>
        ''' <param name="inVisitSequence">来店実績連番</param>
        ''' <returns>来店情報</returns>
        ''' <remarks></remarks>
        Public Function GetVisitManagmentInfo(ByVal inVisitSequence As Long) As SC3080103VisitManagmentInfoDataTable
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inVisitiSequence = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inVisitSequence.ToString(CultureInfo.CurrentCulture)))

            'データ格納用
            Dim dt As SC3080103VisitManagmentInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql

                .AppendLine("SELECT /* SC3080103_016 */ ")
                .AppendLine("       VISITNAME ")
                .AppendLine("      ,VISITTELNO ")
                .AppendLine("  FROM ")
                .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                .AppendLine(" WHERE ")
                .AppendLine("       T1.VISITSEQ = :VISITSEQ ")

            End With

            Using query As New DBSelectQuery(Of SC3080103VisitManagmentInfoDataTable)("SC3080103_016")
                query.CommandText = sql.ToString()
                'バインド変数

                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Long, inVisitSequence)

                'データ取得
                dt = query.GetData()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END OUT:COUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , dt.Count.ToString(CultureInfo.CurrentCulture)))
            Return dt

        End Function

        '2015/01/27 TMEJ 小澤 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

        '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
        ''' <summary>
        ''' SC3080103_017:アカウント情報取得
        ''' </summary>
        ''' <param name="inAccountList">アカウントのリスト</param>
        ''' <returns>ユーザー情報DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetAccountInfoList(ByVal inAccountList As List(Of String)) As SC3080103UserInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inAccountList = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inAccountList.Count.ToString(CultureInfo.CurrentCulture)))

            'IN句用文字列生成
            Dim inQuery As String = GetInQuery(inAccountList, "ACCOUNT")

            'データ格納用
            Dim dt As SC3080103UserInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine("SELECT /* SC3080103_017 */  ")
                .AppendLine("       ACCOUNT, ")
                .AppendLine("       NVL(TRIM(USERNAME), :SPACE_1) AS USERNAME  ")
                .AppendLine("  FROM  ")
                .AppendLine("       TBL_USERS  ")
                .AppendLine("  WHERE  ")
                .AppendLine(inQuery)
            End With

            Using query As New DBSelectQuery(Of SC3080103UserInfoDataTable)("SC3080103_017")
                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("SPACE_1", OracleDbType.NVarchar2, Space(1))

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
        ''' SC3080103_018:顧客情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inDmsCstCdList">基幹顧客コードのリスト</param>
        ''' <returns>顧客情報DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetCustomerInfoList(ByVal inDealerCode As String,
                                            ByVal inDmsCstCdList As List(Of String)) As SC3080103AdditionCustomerInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inDealerCode = {2},inDmsCstCdList = {3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode _
                        , inDmsCstCdList.Count.ToString(CultureInfo.CurrentCulture)))

            'IN句用文字列生成
            Dim inQuery As String = GetInQuery(inDmsCstCdList, "T1.DMS_CST_CD")

            'データ格納用
            Dim dt As SC3080103AdditionCustomerInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine("SELECT /* SC3080103_018 */")
                .AppendLine("       T1.DMS_CST_CD ")
                .AppendLine("      ,T1.CST_ID ")
                .AppendLine("      ,NVL(TRIM(T1.CST_EMAIL_1), :SPACE_1) AS CST_EMAIL_1 ")
                .AppendLine("      ,NVL(TRIM(T2.NAMETITLE_NAME), :SPACE_1) AS NAMETITLE_NAME ")
                .AppendLine("      ,NVL(TRIM(T2.POSITION_TYPE), :SPACE_1) AS POSITION_TYPE ")
                .AppendLine("      ,NVL(TRIM(T3.IMG_FILE_SMALL), :SPACE_1) AS IMG_FILE_SMALL ")
                .AppendLine("      ,T1.DMS_TAKEIN_DATETIME")
                .AppendLine("   FROM ")
                .AppendLine("        TB_M_CUSTOMER  T1 ")
                .AppendLine("       ,TB_M_NAMETITLE T2 ")
                .AppendLine("       ,TB_M_CUSTOMER_DLR T3")
                .AppendLine("   WHERE ")
                .AppendLine("       T1.NAMETITLE_CD = T2.NAMETITLE_CD(+)")
                .AppendLine("   AND T1.CST_ID = T3.CST_ID(+)")
                .AppendLine("   AND T3.DLR_CD(+) = :DLR_CD")
                .AppendLine("   AND ")
                .AppendLine(inQuery)
            End With

            Using query As New DBSelectQuery(Of SC3080103AdditionCustomerInfoDataTable)("SC3080103_018")
                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("SPACE_1", OracleDbType.NVarchar2, Space(1))
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)

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
        ''' SC3080103_019:車両情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inVinList">VINのリスト</param>
        ''' <param name="inRegNoList">車両登録番号のリスト</param>
        ''' <returns>車両情報DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetVehicleInfoList(ByVal inDealerCode As String,
                                           ByVal inVinList As List(Of String),
                                           ByVal inRegNoList As List(Of String)) As SC3080103VehicleInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inDealerCode = {2},inVinList = {3},inRegnoList = {4}," _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode _
                        , inVinList.Count.ToString(CultureInfo.CurrentCulture) _
                        , inRegNoList.Count.ToString(CultureInfo.CurrentCulture)))

            'データ格納用
            Dim dt As SC3080103VehicleInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                If 0 < inVinList.Count Then
                    'VINで取得するクエリ生成
                    'IN句用文字列生成
                    Dim inQueryVin As String = GetInQuery(inVinList, "T2.VCL_VIN_SEARCH")
                    .AppendLine("SELECT /* SC3080103_019 */")
                    .Append(GetVehicleInfoListQuery())
                    .AppendLine("    AND ")
                    .AppendLine(inQueryVin)
                End If

                If 0 < inRegNoList.Count Then
                    '車両登録番号,VIN両方指定有りの場合はUNION ALLで連結
                    If 0 < inVinList.Count Then
                        .AppendLine(" UNION ALL ")
                    End If
                    '車両登録番号で取得するクエリ生成
                    'IN句用文字列生成
                    Dim inQueryRegNo As String = GetInQuery(inRegNoList, "T3.REG_NUM_SEARCH")
                    '車両登録番号での検索の場合はヒント句を指定
                    .AppendLine("SELECT /*+ INDEX(T3 TB_M_VEHICLE_DLR_IX3) */ /* SC3080103_019 */")
                    .Append(GetVehicleInfoListQuery())
                    .AppendLine("    AND ")
                    .AppendLine("           T2.VCL_VIN_SEARCH = :SPACE_1")
                    .AppendLine("            AND ")
                    .AppendLine(inQueryRegNo)
                End If

            End With

            Using query As New DBSelectQuery(Of SC3080103VehicleInfoDataTable)("SC3080103_019")
                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("SPACE_1", OracleDbType.NVarchar2, Space(1))
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, OwnerChangeTypeNone)
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                query.AddParameterWithTypeValue("ICON_FLAG_ON", OracleDbType.NVarchar2, IconFlagOn)

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
        ''' SC3080103_020:予約状況取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">顧客コード</param>
        ''' <param name="inVclIdList">車両IDのリスト</param>
        ''' <param name="inNowDate">当日の日付</param>
        ''' <returns>予約状況DataTable</returns>
        ''' <remarks></remarks>
        Public Function GetApointmentInfoList(ByVal inDealerCode As String, _
                                              ByVal inBranchCode As String, _
                                              ByVal inVclIdList As List(Of Decimal), _
                                              ByVal inNowDate As Date) As SC3080103ApointmentInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START IN:inDealerCode = {2},inBranchCode = {3},inVclIdList = {4},inNowDate = {5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode _
                        , inBranchCode _
                        , inVclIdList.Count.ToString(CultureInfo.CurrentCulture) _
                        , inNowDate.ToString(CultureInfo.CurrentCulture)))

            'IN句用文字列生成
            Dim inQuery As String = GetInQuery(inVclIdList, "T1.VCL_ID")

            'データ格納用
            Dim dt As SC3080103ApointmentInfoDataTable

            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine(" SELECT /* SC3080103_020 */")
                .AppendLine("     T1.VCL_ID ")
                .AppendLine(" FROM TB_T_SERVICEIN T1 , ")
                .AppendLine("      TB_T_JOB_DTL T2 , ")
                .AppendLine("      TB_T_STALL_USE T3 , ")
                .AppendLine("      TB_M_CUSTOMER_VCL T4 ")
                .AppendLine(" WHERE T1.SVCIN_ID = T2.SVCIN_ID ")
                .AppendLine(" AND T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                .AppendLine(" AND T1.DLR_CD = T4.DLR_CD ")
                .AppendLine(" AND T1.CST_ID = T4.CST_ID ")
                .AppendLine(" AND T1.VCL_ID = T4.VCL_ID ")
                .AppendLine(" AND T1.DLR_CD = :DLR_CD ")
                .AppendLine(" AND T1.BRN_CD = :BRN_CD ")
                .AppendLine(" AND T1.SVC_STATUS <> :SVC_STATUS_02 ")
                .AppendLine(" AND T2.DLR_CD = :DLR_CD ")
                .AppendLine(" AND T2.BRN_CD = :BRN_CD ")
                .AppendLine(" AND T2.CANCEL_FLG = :CANCEL_FLG_0 ")
                .AppendLine(" AND T3.DLR_CD = :DLR_CD ")
                .AppendLine(" AND T3.BRN_CD = :BRN_CD ")
                .AppendLine(" AND T4.DLR_CD = :DLR_CD ")
                .AppendLine(" AND T4.CST_VCL_TYPE <> :CST_VCL_TYPE_FLG_4 ")
                .AppendLine(" AND T4.OWNER_CHG_FLG = :OWNER_CHG_FLG_0 ")
                .AppendLine(" AND T1.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_0 ")
                .AppendLine(" AND T3.SCHE_START_DATETIME >= TRUNC(:NOWDATE) ")
                .AppendLine(" AND T3.SCHE_START_DATETIME <= TRUNC(:MAXDATE) ")
                .AppendLine(" AND T3.RSLT_START_DATETIME = :MINDATE ")
                .AppendLine(" AND ")
                .AppendLine(inQuery)
            End With

            Using query As New DBSelectQuery(Of SC3080103ApointmentInfoDataTable)("SC3080103_020")
                query.CommandText = sql.ToString()
                'バインド変数
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, inBranchCode)
                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, ServiceStatusCancel)
                query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_0", OracleDbType.NVarchar2, AcceptanceTypeReserve)
                query.AddParameterWithTypeValue("CST_VCL_TYPE_FLG_4", OracleDbType.NVarchar2, CstVclTypeCnt)
                query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, OwnerTypeUnset)
                query.AddParameterWithTypeValue("CANCEL_FLG_0", OracleDbType.NVarchar2, CancelTypeEffective)
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(DateMinValue, CultureInfo.CurrentCulture))
                query.AddParameterWithTypeValue("MAXDATE", OracleDbType.Date, Date.MaxValue)

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
        ''' 車両情報取得クエリ共通部分生成
        ''' </summary>
        ''' <returns>車両情報取得クエリ</returns>
        ''' <remarks></remarks>
        Private Function GetVehicleInfoListQuery() As String
            '車両情報取得クエリ共通部分生成
            Dim sql As New StringBuilder

            'SQL文作成
            With sql
                .AppendLine("       T2.VCL_VIN_SEARCH,")
                .AppendLine("       T3.REG_NUM_SEARCH,")
                .AppendLine("       T1.VCL_ID,")
                .AppendLine("       T2.MODEL_CD,")
                .AppendLine("       NVL(TRIM(T6.MODEL_NAME), NVL(TRIM(T2.NEWCST_MODEL_NAME),:SPACE_1)) AS MODEL_NAME,")
                .AppendLine("       NVL(T7.REG_AREA_NAME, :SPACE_1) AS REG_AREA_NAME,")
                .AppendLine("       T5.DMS_TAKEIN_DATETIME,")
                .AppendLine("       T4.CST_TYPE,")
                .AppendLine("       T3.REG_NUM,")
                .AppendLine("       T2.VCL_VIN,")
                .AppendLine("       T2.SPECIAL_CAMPAIGN_TGT_FLG AS SSC_MARK,")
                .AppendLine("       NVL(TRIM(T3.IMP_VCL_FLG), :ICON_FLAG_OFF )AS PL_MARK,")
                .AppendLine("       NVL(TRIM(T8.SML_AMC_FLG), :ICON_FLAG_OFF )AS MB_MARK,")
                .AppendLine("       NVL(TRIM(T8.EW_FLG), :ICON_FLAG_OFF)AS E_MARK,")
                .AppendLine("       CASE ")
                .AppendLine("             WHEN T9.VCL_VIN IS NULL THEN :ICON_FLAG_OFF")
                .AppendLine("             ELSE :ICON_FLAG_ON")
                .AppendLine("       END AS TLM_MBR_FLG")
                .AppendLine("    FROM TB_M_CUSTOMER_VCL T1 ,")
                .AppendLine("        TB_M_VEHICLE T2 ,")
                .AppendLine("        TB_M_VEHICLE_DLR T3 ,")
                .AppendLine("        TB_M_CUSTOMER_DLR T4 ,")
                .AppendLine("        TB_M_CUSTOMER T5,")
                .AppendLine("        TB_M_MODEL T6,")
                .AppendLine("        TB_M_REG_AREA T7,")
                .AppendLine("        TB_LM_VEHICLE T8,")
                .AppendLine("        TB_LM_TLM_MEMBER T9")
                .AppendLine("    WHERE T1.VCL_ID = T2.VCL_ID")
                .AppendLine("    AND T1.DLR_CD = T3.DLR_CD")
                .AppendLine("    AND T2.VCL_ID = T3.VCL_ID")
                .AppendLine("    AND T1.DLR_CD = T4.DLR_CD")
                .AppendLine("    AND T1.CST_ID = T4.CST_ID")
                .AppendLine("    AND T1.CST_ID = T5.CST_ID")
                .AppendLine("    AND T2.MODEL_CD = T6.MODEL_CD(+)")
                .AppendLine("    AND T3.REG_AREA_CD = T7.REG_AREA_CD(+)")
                .AppendLine("    AND T2.VCL_ID = T8.VCL_ID(+)")
                .AppendLine("    AND T2.VCL_VIN = T9.VCL_VIN(+)")
                .AppendLine("    AND T1.DLR_CD = :DLR_CD")
                .AppendLine("    AND T1.OWNER_CHG_FLG = :OWNER_CHG_FLG_0")
            End With

            Return sql.ToString()
        End Function

        ''' <summary>
        ''' SQLのIN句作成
        ''' </summary>
        ''' <param name="valueList">検索条件のリスト</param>
        ''' <param name="rowName">列名</param>
        ''' <returns>IN句部分の文字列</returns>
        ''' <remarks></remarks>
        Private Function GetInQuery(ByVal valueList As List(Of String), ByVal rowName As String) As String
            'IN句用文字列生成
            Dim sbIn As New StringBuilder
            If InQueryMax < valueList.Count Then
                '1000件超過の場合は全体を括弧に入れる
                sbIn.Append("(")
            End If
            sbIn.Append(rowName)
            sbIn.Append(" IN ( ")
            Dim count As Integer = 0
            For Each val As String In valueList
                If InQueryMax <= count Then
                    'IN句内の値が1000個に達したら別のIN句をORで連結する
                    '末尾のカンマを削除
                    sbIn.Length -= 1
                    sbIn.Append(")")
                    sbIn.Append(" OR ")
                    sbIn.Append(rowName)
                    sbIn.Append(" IN (")
                    count = 0
                End If
                sbIn.Append(String.Format(CultureInfo.CurrentCulture, " '{0}' ,", val.Replace("'", "''")))
                count += 1
            Next
            '末尾のカンマを削除
            sbIn.Length -= 1
            'IN句の括弧閉じる
            sbIn.Append(") ")
            If InQueryMax < valueList.Count Then
                '1000件超過の場合は全体を括弧に入れる
                sbIn.Append(")")
            End If

            Return sbIn.ToString()
        End Function
        ''' <summary>
        ''' SQLのIN句作成(Decimal用)
        ''' </summary>
        ''' <param name="valueList">検索条件のリスト</param>
        ''' <param name="rowName">列名</param>
        ''' <returns>IN句部分の文字列</returns>
        ''' <remarks></remarks>
        Private Function GetInQuery(ByVal valueList As List(Of Decimal), ByVal rowName As String) As String
            'IN句用文字列生成
            Dim sbIn As New StringBuilder
            If InQueryMax < valueList.Count Then
                '1000件超過の場合は全体を括弧に入れる
                sbIn.Append("(")
            End If
            sbIn.Append(rowName)
            sbIn.Append(" IN ( ")
            Dim count As Integer = 0
            For Each val As Decimal In valueList
                If InQueryMax <= count Then
                    'IN句内の値が1000個に達したら別のIN句をORで連結する
                    '末尾のカンマを削除
                    sbIn.Length -= 1
                    sbIn.Append(")")
                    sbIn.Append(" OR ")
                    sbIn.Append(rowName)
                    sbIn.Append(" IN (")
                    count = 0
                End If
                sbIn.Append(String.Format(CultureInfo.CurrentCulture, " {0} ,", val.ToString(CultureInfo.CurrentCulture)))
                count += 1
            Next
            '末尾のカンマを削除
            sbIn.Length -= 1
            'IN句の括弧閉じる
            sbIn.Append(") ")
            If InQueryMax < valueList.Count Then
                '1000件超過の場合は全体を括弧に入れる
                sbIn.Append(")")
            End If

            Return sbIn.ToString()
        End Function
        '2019/03/05 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END

#End Region

    End Class

End Namespace

Partial Class SC3080103DataSet
    Partial Class SC3080103CustomerInfoDataTable

    End Class

End Class
