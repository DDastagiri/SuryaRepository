'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080205DataTableTableAdapter.vb
'─────────────────────────────────────
'機能： 顧客編集 (データ)
'補足： 
'作成： 2011/11/07 TCS 安田
'更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2013/06/30 TCS 内藤 【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/11/27 TCS 各務 Aカード情報相互連携開発
'更新： 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）
'更新： 2014/04/01 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/04/21 TCS 松月 【A STEP2】サービス／保険担当店舗コード設定対応（号口切替BTS-355） 
'更新： 2014/05/01 TCS 松月 新PF残課題No.21  
'更新： 2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354)
'更新： 2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80)
'更新： 2014/08/01 TCS 市川 TMT切替BTS-113対応
'更新： 2016/11/28 TCS 曽出 （トライ店システム評価）基幹連携に伴う顧客車両情報管理機能評価　【TR-V4-TMT-20160623-001】
'更新： 2017/11/20 TCS 河原 TKM独自機能開発
'更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001)
'更新： 2019/03/06 TS  都築 組織を入力候補プルダウンで選択するときに大文字小文字関係なく候補に出るようにする((FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究)
'削除： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'更新： 2020/01/20 TS  岩田 TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045)
''─────────────────────────────────────
Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
'2013/06/30 TCS 内藤 2013/10対応版 既存流用 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
'2013/06/30 TCS 内藤 2013/10対応版 既存流用 END

Public Class SC3080205TableAdapter
    Implements System.IDisposable

    Private _enabledTable As Dictionary(Of Integer, Boolean)

    ''' <summary>
    ''' 画面の可視/非可視状態
    ''' </summary>
    ''' <returns>画面の可視/非可視状態</returns>
    ''' <remarks></remarks>
    Public Function EnabledTable() As Dictionary(Of Integer, Boolean)

        Return _enabledTable

    End Function

    ''' <summary>
    ''' 画面の可視/非可視状態
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetEnabledTable(ByVal tbl As Dictionary(Of Integer, Boolean))

        _enabledTable = tbl

    End Sub

    ''' <summary>
    ''' 自社客個人情報項目リスト
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IdCustype As Integer = 5         '顧客タイプ 8 ⇒ 5
    Public Const IdSocialid As Integer = 10         '国民番号 9 ⇒ 10
    Public Const IdName As Integer = 11         '氏名 10 ⇒ 11
    Public Const IdSex As Integer = 20         '性別 11 ⇒ 20
    Public Const IdBirthday As Integer = 39         '生年月日 12 ⇒ 39
    Public Const IdNameTitle As Integer = 13         '敬称
    Public Const IdAddress As Integer = 24         '住所 20 ⇒ 24
    Public Const IdZipcode As Integer = 23         '郵便番号 26 ⇒ 23
    Public Const Idtelno As Integer = 32         '電話番号 27 ⇒ 32
    Public Const IdFaxno As Integer = 34         'FAX番号 28 ⇒ 34
    Public Const IdMobile As Integer = 33         '携帯電話番号 29 ⇒ 33
    Public Const IdEmail1 As Integer = 37         'e-MAILアドレス1 30 ⇒ 37
    Public Const IdEmail2 As Integer = 38         'e-MAILアドレス2 31 ⇒ 38
    Public Const IdBusinessTelno As Integer = 36         '勤め先電話番号 32 ⇒ 36
    '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
    Public Const IdCstIncome As Integer = 40         '顧客収入
    '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
    Public Const IdEmployeeName As Integer = 6         '担当者名 54 ⇒ 6
    Public Const IdEmployeeDepartment As Integer = 7         '担当者所属部署 55 ⇒ 7
    Public Const IdEmployeePosition As Integer = 8         '担当者役職 56 ⇒ 8
    Public Const IdNameTitlecd As Integer = 12         '敬称コード 58 ⇒ 12
    '2013/06/30 TCS 内藤 2013/10対応版 既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版 既存流用 END
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    Public Const IdFirstName As Integer = 14         'ファーストネーム 14 ⇒ 14
    Public Const IdMiddleName As Integer = 15         'ミドルネーム 15 ⇒ 15
    Public Const IdLastName As Integer = 16         'ラストネーム 16 ⇒ 16
    Public Const IdDomicile As Integer = 21         '本籍 24 ⇒ 21
    Public Const IdCountry As Integer = 22         '国籍 25 ⇒ 22
    Public Const IdAddress1 As Integer = 25         '住所1 21 ⇒ 25
    Public Const IdAddress2 As Integer = 26         '住所2 22 ⇒ 26
    Public Const IdAddress3 As Integer = 27         '住所3 23 ⇒ 27
    Public Const IdState As Integer = 28         '住所(州) 新規(59) ⇒ 28
    Public Const IdDistrict As Integer = 29         '住所(地域) 新規(60) ⇒ 29
    Public Const IdCity As Integer = 30         '住所(市) 新規(61) ⇒ 30
    Public Const IdLocation As Integer = 31         '住所(地区) 新規(62) ⇒ 31
    Public Const IdPrivateFleetItem As Integer = 45         '個人法人項目コード 新規(63) ⇒ 45
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

    ''' <summary>
    ''' 未顧客個人情報項目リスト (100～)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IdACtvctgryid As Integer = 100            'AC
    Public Const IdACModfaccount As Integer = 101         'AC変更アカウント
    Public Const IdACModffuncDvs As Integer = 102         'AC変更機能
    Public Const IdReasonId As Integer = 103               '活動除外理由ID

    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    ''' <summary>
    ''' ダミー名称フラグ
    ''' 0:正式名称　1:ダミー名称
    ''' </summary>
    Public Const DummyNameFlgOfficial As String = "0"
    Public Const DummyNameFlgDummy As String = "1"
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 住所表示順フラグ
    ''' 0:大→小　1:小→大
    ''' </summary>
    Public Const DirectionFlgForward As String = "0"
    Public Const DirectionFlgBack As String = "1"
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

    '2014/08/01 TCS 市川 TMT切替BTS-113対応 START
    ''' <summary>
    ''' システム環境設定キー値：州(地区/市/地域も同じ)選択リストの並び順設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SYSENVKEY_STATELIST_SORT_BY_NAME As String = "USE_STATELIST_SORT_BY_NAME"
    ''' <summary>
    ''' USE_STATELIST_SORT_BY_NAMEに対する設定値(名称順利用)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const STATELIST_SORT_BY_NAME As String = "1"

    ''' <summary>
    ''' 州(地区/市/地域も同じ)選択リストの並び順を名称順にするか否か
    ''' </summary>
    ''' <value></value>
    ''' <returns>True：名称順/False：コード順(グローバル仕様)</returns>
    ''' <remarks></remarks>
    Private ReadOnly Property IsStateListSortByName() As Boolean
        Get
            Dim env As SystemEnvSetting = Nothing
            Dim dr As iCROP.DataAccess.SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            Dim ret As Boolean = False

            Try
                env = New SystemEnvSetting()
                dr = env.GetSystemEnvSetting(SYSENVKEY_STATELIST_SORT_BY_NAME)
                ret = (Not dr Is Nothing AndAlso dr.PARAMVALUE.Equals(STATELIST_SORT_BY_NAME))
            Finally
                env = Nothing
                dr = Nothing
            End Try

            Return ret
        End Get
    End Property
    '2014/08/01 TCS 市川 TMT切替BTS-113対応 END

    '2013/06/30 TCS 宋 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 自社客個人情報取得
    ''' </summary>
    ''' <param name="originalid">自社客連番</param>
    ''' <returns>SC3080205CustDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetCustomer(ByVal originalid As String) As SC3080205DataSet.SC3080205CustDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205CustDataTable)("SC3080205_201")

            Dim sql As New StringBuilder

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomer_Start")
            'ログ出力 End *****************************************************************************

            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            With sql
                .Append("SELECT ")
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("    /* SC3080205_201 */ ")
                .Append("    TO_CHAR(T1.CST_ID) AS CUSTCD, ")
                .Append("    T1.CST_SOCIALNUM AS SOCIALID, ")
                .Append("    CASE WHEN T1.FLEET_FLG = '0' THEN '1' ")
                .Append("         WHEN T1.FLEET_FLG = '1' THEN '0' ")
                .Append("    END AS CUSTYPE, ")
                .Append("    T1.CST_NAME AS NAME, ")
                '2016/11/28 TCS 曽出 （トライ店システム評価）基幹連携に伴う顧客車両情報管理機能評価　【TR-V4-TMT-20160623-001】START
                .Append("    T1.FIRST_NAME AS FIRSTNAME, ")
                .Append("    T1.MIDDLE_NAME AS MIDDLENAME, ")
                .Append("    T1.LAST_NAME AS LASTNAME, ")
                '2016/11/28 TCS 曽出 （トライ店システム評価）基幹連携に伴う顧客車両情報管理機能評価　【TR-V4-TMT-20160623-001】END
                .Append("    T1.NAMETITLE_CD AS NAMETITLE_CD, ")
                .Append("    T1.NAMETITLE_NAME AS NAMETITLE, ")
                .Append("    T1.CST_ZIPCD AS ZIPCODE, ")
                .Append("    T1.CST_ADDRESS_1 AS ADDRESS1, ")
                .Append("    T1.CST_ADDRESS_2 AS ADDRESS2, ")
                .Append("    T1.CST_ADDRESS_3 AS ADDRESS3, ")
                .Append("    T1.CST_ADDRESS_STATE AS ADDRESS_STATE, ")
                .Append("    T1.CST_ADDRESS_DISTRICT AS ADDRESS_DISTRICT, ")
                .Append("    T1.CST_ADDRESS_CITY AS ADDRESS_CITY, ")
                .Append("    T1.CST_ADDRESS_LOCATION AS ADDRESS_LOCATION, ")
                .Append("    T1.CST_PHONE AS TELNO, ")
                .Append("    T1.CST_MOBILE AS MOBILE, ")
                .Append("    T1.CST_FAX AS FAXNO, ")
                .Append("    T1.CST_BIZ_PHONE AS BUSINESSTELNO, ")
                .Append("    T1.CST_EMAIL_1 AS EMAIL1, ")
                .Append("    T1.CST_EMAIL_2 AS EMAIL2, ")
                .Append("    T1.CST_GENDER AS SEX, ")
                .Append("    (CASE WHEN T1.CST_BIRTH_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
                .Append("              NULL ")
                .Append("          ELSE T1.CST_BIRTH_DATE END) AS BIRTHDAY, ")
                .Append("    T1.FLEET_PIC_NAME AS EMPLOYEENAME, ")
                .Append("    T1.FLEET_PIC_DEPT AS EMPLOYEEDEPARTMENT, ")
                .Append("    T1.FLEET_PIC_POSITION AS EMPLOYEEPOSITION, ")
                .Append("    T1.CST_DOMICILE AS DOMICILE, ")
                .Append("    T1.CST_COUNTRY AS COUNTRY, ")
                .Append("    T1.PRIVATE_FLEET_ITEM_CD AS PRIVATE_FLEET_ITEM_CD, ")
                .Append("    T1.UPDATE_FUNCTION_JUDGE AS UPDATEFUNCFLG, ")
                '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
                .Append("    T1.CST_INCOME, ")
                '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
                .Append("     CASE WHEN T2.CST_ORGNZ_INPUT_TYPE = '1' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('1','2') AND CORG.CST_ORGNZ_CD = T2.CST_ORGNZ_CD THEN NVL(CORG.CST_ORGNZ_CD, ' ') ")
                .Append("          WHEN T2.CST_ORGNZ_INPUT_TYPE = '2' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('0','2') THEN N' ' ")
                .Append("          ELSE N' ' END AS CST_ORGNZ_CD, ")
                .Append("     NVL2(PFI.PRIVATE_FLEET_ITEM_CD,NVL(T2.CST_ORGNZ_INPUT_TYPE, ' '),N' ') AS CST_ORGNZ_INPUT_TYPE, ")
                .Append("     CASE WHEN T2.CST_ORGNZ_INPUT_TYPE = '1' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('1','2') AND CORG.CST_ORGNZ_CD = T2.CST_ORGNZ_CD THEN NVL(CORG.CST_ORGNZ_NAME, ' ') ")
                .Append("          WHEN T2.CST_ORGNZ_INPUT_TYPE = '2' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('0','2') THEN NVL(T2.CST_ORGNZ_NAME, ' ') ")
                .Append("          ELSE N' ' END AS CST_ORGNZ_NAME, ")
                .Append("     CASE WHEN T2.CST_ORGNZ_INPUT_TYPE = '1' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('1','2') AND CORG.CST_ORGNZ_CD = CSUB2.CST_ORGNZ_CD THEN NVL(T2.CST_SUBCAT2_CD, ' ') ")
                .Append("          WHEN T2.CST_ORGNZ_INPUT_TYPE = '2' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('0','2') THEN NVL(T2.CST_SUBCAT2_CD, ' ') ")
                .Append("          ELSE N' ' END AS CST_SUBCAT2_CD, ")
                .Append("     CASE WHEN T2.CST_ORGNZ_INPUT_TYPE = '1' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('1','2') AND CORG.CST_ORGNZ_CD = CSUB2.CST_ORGNZ_CD THEN NVL(CSUB2.CST_SUBCAT2_NAME, ' ') ")
                .Append("          WHEN T2.CST_ORGNZ_INPUT_TYPE = '2' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('0','2') THEN NVL(CSUB2.CST_SUBCAT2_NAME, ' ') ")
                .Append("          ELSE N' ' END AS CST_SUBCAT2_NAME, ")
                .Append("     T1.ROW_LOCK_VERSION AS LOCKVERSION, ")
                .Append("     NVL(T2.ROW_LOCK_VERSION, -1) AS CST_LOCAL_ROW_LOCK_VERSION ")
                .Append("FROM ")
                .Append("           TB_M_CUSTOMER T1 ")
                .Append(" LEFT JOIN TB_LM_CUSTOMER T2 ON T1.CST_ID = T2.CST_ID ")
                .Append(" LEFT JOIN TB_M_PRIVATE_FLEET_ITEM PFI ON PFI.PRIVATE_FLEET_ITEM_CD = T1.PRIVATE_FLEET_ITEM_CD AND PFI.FLEET_FLG = T1.FLEET_FLG AND PFI.INUSE_FLG = '1' ")
                .Append(" LEFT JOIN TB_LM_PRIVATE_FLEET_ITEM PFIL ON PFIL.PRIVATE_FLEET_ITEM_CD = PFI.PRIVATE_FLEET_ITEM_CD ")
                .Append(" LEFT JOIN TB_LM_CUSTOMER_ORGANIZATION CORG ON CORG.CST_ORGNZ_CD = T2.CST_ORGNZ_CD AND CORG.PRIVATE_FLEET_ITEM_CD = PFI.PRIVATE_FLEET_ITEM_CD AND CORG.INUSE_FLG = '1' ")
                .Append(" LEFT JOIN TB_LM_CUSTOMER_SUBCATEGORY2 CSUB2 ON CSUB2.CST_SUBCAT2_CD = T2.CST_SUBCAT2_CD AND CSUB2.PRIVATE_FLEET_ITEM_CD = PFI.PRIVATE_FLEET_ITEM_CD AND CSUB2.INUSE_FLG = '1' ")
                .Append("WHERE ")
                .Append("    T1.CST_ID = :ORIGINALID ")
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
            End With
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomer_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 宋 2013/10対応版　既存流用 END

            Return query.GetData()

        End Using

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END


    '2013/06/30 TCS 宋 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客個人情報取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstid">未取引客ユーザーID</param>
    ''' <param name="vclid">VCLID</param>
    ''' <returns>SC3080205CustDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetNewcustomer(ByVal dlrcd As String, _
                                   ByVal cstid As String, _
                                   ByVal vclid As String) As SC3080205DataSet.SC3080205CustDataTable
        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205CustDataTable)("SC3080205_202")
            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewcustomer_Start")
            'ログ出力 End *****************************************************************************

            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080205_202 */ ")
                .Append("  TO_CHAR(T1.CST_ID) AS CSTID, ")
                .Append("  T1.CST_SOCIALNUM AS SOCIALID, ")
                .Append("  CASE WHEN T1.FLEET_FLG = '0' THEN '1' ")
                .Append("       WHEN T1.FLEET_FLG = '1' THEN '0' ")
                .Append("  END AS CUSTYPE, ")
                .Append("  T1.CST_NAME AS NAME, ")
                '2016/11/28 TCS 曽出 （トライ店システム評価）基幹連携に伴う顧客車両情報管理機能評価　【TR-V4-TMT-20160623-001】START
                .Append("  FIRST_NAME AS FIRSTNAME, ")
                .Append("  MIDDLE_NAME AS MIDDLENAME, ")
                .Append("  LAST_NAME AS LASTNAME, ")
                '2016/11/28 TCS 曽出 （トライ店システム評価）基幹連携に伴う顧客車両情報管理機能評価　【TR-V4-TMT-20160623-001】END
                .Append("  T1.NAMETITLE_CD AS NAMETITLE_CD, ")
                .Append("  T1.NAMETITLE_NAME AS NAMETITLE, ")
                .Append("  T1.CST_ZIPCD AS ZIPCODE, ")
                .Append("  T1.CST_ADDRESS_1 AS ADDRESS1, ")
                .Append("  T1.CST_ADDRESS_2 AS ADDRESS2, ")
                .Append("  T1.CST_ADDRESS_3 AS ADDRESS3, ")
                .Append("  T1.CST_ADDRESS_STATE AS ADDRESS_STATE, ")
                .Append("  T1.CST_ADDRESS_DISTRICT AS ADDRESS_DISTRICT, ")
                .Append("  T1.CST_ADDRESS_CITY AS ADDRESS_CITY, ")
                .Append("  T1.CST_ADDRESS_LOCATION AS ADDRESS_LOCATION, ")
                .Append("  T1.CST_PHONE AS TELNO, ")
                .Append("  T1.CST_MOBILE AS MOBILE, ")
                .Append("  T1.CST_FAX AS FAXNO, ")
                .Append("  T1.CST_BIZ_PHONE AS BUSINESSTELNO, ")
                .Append("  T1.CST_EMAIL_1 AS EMAIL1, ")
                .Append("  T1.CST_EMAIL_2 AS EMAIL2, ")
                .Append("  T1.CST_GENDER AS SEX, ")
                .Append("  CASE WHEN T1.CST_BIRTH_DATE = TO_DATE('1900/1/1', 'YYYY/MM/DD HH24:MI:SS') THEN ")
                .Append("            NULL ")
                .Append("       ELSE T1.CST_BIRTH_DATE END AS BIRTHDAY, ")
                .Append("  T1.FLEET_PIC_NAME AS EMPLOYEENAME, ")
                .Append("  T1.FLEET_PIC_DEPT AS EMPLOYEEDEPARTMENT, ")
                .Append("  T1.FLEET_PIC_POSITION AS EMPLOYEEPOSITION, ")
                .Append("  CV.ACT_CAT_TYPE AS ACTVCTGRYID, ")
                .Append("  CV.OMIT_REASON_CD AS REASONID, ")
                .Append("　T1.CST_REG_STATUS　AS DUMMYNAMEFLG, ")
                .Append("  T1.CST_DOMICILE AS DOMICILE, ")
                .Append("  T1.CST_COUNTRY AS COUNTRY, ")
                .Append("  T1.PRIVATE_FLEET_ITEM_CD AS PRIVATE_FLEET_ITEM_CD, ")
                '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
                .Append("  T1.CST_INCOME, ")
                '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
                .Append("  T1.ROW_LOCK_VERSION AS LOCKVERSION, ")
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("  CV.ROW_LOCK_VERSION AS VCLLOCKVERSION, ")
                .Append("     CASE WHEN T2.CST_ORGNZ_INPUT_TYPE = '1' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('1','2') AND CORG.CST_ORGNZ_CD = T2.CST_ORGNZ_CD THEN NVL(CORG.CST_ORGNZ_CD, ' ') ")
                .Append("          WHEN T2.CST_ORGNZ_INPUT_TYPE = '2' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('0','2') THEN N' ' ")
                .Append("          ELSE N' ' END AS CST_ORGNZ_CD, ")
                .Append("     NVL2(PFI.PRIVATE_FLEET_ITEM_CD,NVL(T2.CST_ORGNZ_INPUT_TYPE, ' '),N' ') AS CST_ORGNZ_INPUT_TYPE, ")
                .Append("     CASE WHEN T2.CST_ORGNZ_INPUT_TYPE = '1' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('1','2') AND CORG.CST_ORGNZ_CD = T2.CST_ORGNZ_CD THEN NVL(CORG.CST_ORGNZ_NAME, ' ') ")
                .Append("          WHEN T2.CST_ORGNZ_INPUT_TYPE = '2' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('0','2') THEN NVL(T2.CST_ORGNZ_NAME, ' ') ")
                .Append("          ELSE N' ' END AS CST_ORGNZ_NAME, ")
                .Append("     CASE WHEN T2.CST_ORGNZ_INPUT_TYPE = '1' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('1','2') AND CORG.CST_ORGNZ_CD = CSUB2.CST_ORGNZ_CD THEN NVL(T2.CST_SUBCAT2_CD, ' ') ")
                .Append("          WHEN T2.CST_ORGNZ_INPUT_TYPE = '2' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('0','2') THEN NVL(T2.CST_SUBCAT2_CD, ' ') ")
                .Append("          ELSE N' ' END AS CST_SUBCAT2_CD, ")
                .Append("     CASE WHEN T2.CST_ORGNZ_INPUT_TYPE = '1' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('1','2') AND CORG.CST_ORGNZ_CD = CSUB2.CST_ORGNZ_CD THEN NVL(CSUB2.CST_SUBCAT2_NAME, ' ') ")
                .Append("          WHEN T2.CST_ORGNZ_INPUT_TYPE = '2' AND PFIL.CST_ORGNZ_NAME_INPUT_TYPE IN ('0','2') THEN NVL(CSUB2.CST_SUBCAT2_NAME, ' ') ")
                .Append("          ELSE N' ' END AS CST_SUBCAT2_NAME, ")
                .Append("  NVL(T2.ROW_LOCK_VERSION, -1) AS CST_LOCAL_ROW_LOCK_VERSION ")
                .Append("FROM ")
                .Append("            TB_M_CUSTOMER T1 ")
                .Append(" INNER JOIN TB_M_CUSTOMER_VCL CV ON CV.DLR_CD = :DLRCD AND CV.CST_ID = T1.CST_ID AND CV.VCL_ID = :VCLID")
                .Append(" LEFT JOIN TB_LM_CUSTOMER T2 ON T1.CST_ID = T2.CST_ID ")
                .Append(" LEFT JOIN TB_M_PRIVATE_FLEET_ITEM PFI ON PFI.PRIVATE_FLEET_ITEM_CD = T1.PRIVATE_FLEET_ITEM_CD AND PFI.FLEET_FLG = T1.FLEET_FLG AND PFI.INUSE_FLG = '1' ")
                .Append(" LEFT JOIN TB_LM_PRIVATE_FLEET_ITEM PFIL ON PFIL.PRIVATE_FLEET_ITEM_CD = PFI.PRIVATE_FLEET_ITEM_CD ")
                .Append(" LEFT JOIN TB_LM_CUSTOMER_ORGANIZATION CORG ON CORG.CST_ORGNZ_CD = T2.CST_ORGNZ_CD AND CORG.PRIVATE_FLEET_ITEM_CD = PFI.PRIVATE_FLEET_ITEM_CD AND CORG.INUSE_FLG = '1' ")
                .Append(" LEFT JOIN TB_LM_CUSTOMER_SUBCATEGORY2 CSUB2 ON CSUB2.CST_SUBCAT2_CD = T2.CST_SUBCAT2_CD AND CSUB2.PRIVATE_FLEET_ITEM_CD = PFI.PRIVATE_FLEET_ITEM_CD AND CSUB2.INUSE_FLG = '1' ")
                .Append("WHERE ")
                .Append("      T1.CST_ID = :CSTID ")
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
            End With
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("VCLID", OracleDbType.Decimal, vclid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewcustomer_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 宋 2013/10対応版　既存流用 END

            Return query.GetData()

        End Using

    End Function

    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
    ''' <summary>
    ''' 販売店顧客情報取得
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="cstId">顧客ID</param>
    ''' <returns></returns>
    ''' <remarks>自社客・未取引客共通</remarks>
    Public Function GetCustomerDlr(ByVal dlrCd As String, ByVal cstId As Decimal) As SC3080205DataSet.SC3080205CustDlrDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerDlr_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder(10000)
        Dim ret As SC3080205DataSet.SC3080205CustDlrDataTable = Nothing

        With sql
            .AppendLine("SELECT /* SC3080205_218 */ ")
            .AppendLine("     CST_TYPE ")
            .AppendLine("    ,COMMERCIAL_RECV_TYPE ")
            .AppendLine("    ,UPDATE_FUNCTION_JUDGE ")
            .AppendLine("    ,ROW_LOCK_VERSION ")
            .AppendLine("FROM TB_M_CUSTOMER_DLR  ")
            .AppendLine("WHERE ")
            .AppendLine("    DLR_CD = :DLR_CD ")
            .AppendLine("    AND CST_ID = :CST_ID ")
        End With

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205CustDlrDataTable)("SC3080205_218")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstId)

            ret = query.GetData()
        End Using

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerDlr_End")
        'ログ出力 End *****************************************************************************

        Return ret
    End Function

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL
    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END

    ''' <summary>
    ''' 郵便番号辞書検索
    ''' </summary>
    ''' <param name="zipcode ">郵便番号 </param>
    ''' <param name="directionFlg ">住所表示順フラグ </param>
    ''' <returns>SC3080205OrgCustomerDataTableDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetAddress(ByVal zipcode As String, ByVal directionFlg As String) As SC3080205DataSet.SC3080205ZipDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205ZipDataTable)("SC3080205_003")

            Dim sql As New StringBuilder
            '2013/06/30 TCS 宋 2013/10対応版　既存流用 START
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAddress_Start")
            'ログ出力 End *****************************************************************************

            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            'With sql
            '   .Append("SELECT /* SC3080205_003 */ ")
            '   .Append("    T2.STATE_NAME || T3.DISTRICT_NAME || T4.CITY_NAME || T1.LOCATION_NAME AS ADDRESS ")
            '   .Append("FROM ")
            '   .Append("    TB_M_LOCATION T1, ")
            '   .Append("    TB_M_STATE T2, ")
            '   .Append("    TB_M_DISTRICT T3, ")
            '   .Append("    TB_M_CITY T4 ")
            '   .Append("WHERE ")
            '   .Append("    T1.STATE_CD = T2.STATE_CD AND ")
            '   .Append("    T1.STATE_CD = T3.STATE_CD AND ")
            '   .Append("    T1.DISTRICT_CD = T3.DISTRICT_CD AND ")
            '   .Append("    T1.STATE_CD = T4.STATE_CD AND ")
            '   .Append("    T1.DISTRICT_CD = T4.DISTRICT_CD AND ")
            '   .Append("    T1.CITY_CD = T4.CITY_CD AND ")
            '   .Append("    T1.ZIP_CD = :ZIPCODE ")
            'End With
            With sql
                .Append("SELECT /* SC3080205_003 */ ")
                If directionFlg = DirectionFlgBack Then
                    .Append("    T1.LOCATION_NAME || T4.CITY_NAME || T3.DISTRICT_NAME || T2.STATE_NAME AS ADDRESS, ")
                Else
                    .Append("    T2.STATE_NAME || T3.DISTRICT_NAME || T4.CITY_NAME || T1.LOCATION_NAME AS ADDRESS, ")
                End If
                .Append("    T2.STATE_CD AS ADDRESS_STATE, ")
                .Append("    T3.DISTRICT_CD AS ADDRESS_DISTRICT, ")
                .Append("    T4.CITY_CD AS ADDRESS_CITY, ")
                .Append("    T1.LOCATION_CD AS ADDRESS_LOCATION ")
                .Append("FROM ")
                .Append("    TB_M_LOCATION T1, ")
                .Append("    TB_M_STATE T2, ")
                .Append("    TB_M_DISTRICT T3, ")
                .Append("    TB_M_CITY T4 ")
                .Append("WHERE ")
                .Append("    T1.STATE_CD = T2.STATE_CD AND ")
                .Append("    T1.STATE_CD = T3.STATE_CD AND ")
                .Append("    T1.DISTRICT_CD = T3.DISTRICT_CD AND ")
                .Append("    T1.STATE_CD = T4.STATE_CD AND ")
                .Append("    T1.DISTRICT_CD = T4.DISTRICT_CD AND ")
                .Append("    T1.CITY_CD = T4.CITY_CD AND ")
                .Append("    T1.ZIP_CD = :ZIPCODE ")
            End With
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ZIPCODE", OracleDbType.NVarchar2, zipcode)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetAddress_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 宋 2013/10対応版　既存流用 END

            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' 敬称マスタ取得
    ''' </summary>
    ''' <returns>SC3080205OrgCustomerDataTableDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetNametitle(ByVal dispflglist As List(Of String)) As SC3080205DataSet.SC3080205NameTitleDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205NameTitleDataTable)("SC3080205_004")

            Dim sql As New StringBuilder
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNametitle_Start")
            'ログ出力 End *****************************************************************************

            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            'With sql
            '   .Append("SELECT ")
            '   .Append("  /* SC3080205_004 */ ")
            '   .Append("  NAMETITLE_CD AS NAMETITLE_CD, ")
            '   .Append("  NAMETITLE_NAME AS NAMETITLE, ")
            '   .Append("  CASE WHEN NAMETITLE_TYPE = '0' THEN '0' ")
            '   .Append("       WHEN NAMETITLE_TYPE = '1' THEN '2' ")
            '   .Append("       WHEN NAMETITLE_TYPE = '2' THEN '1' ")
            '   .Append("       WHEN NAMETITLE_TYPE = '3' THEN '1' ")
            '   .Append("  END AS DISPFLG ")
            '   .Append("FROM ")
            '   .Append("  TB_M_NAMETITLE ")
            '   .Append("WHERE ")
            '   .Append("  INUSE_FLG = '1' ")
            '   If (Not IsNothing(dispflglist) AndAlso (dispflglist.Count > 0)) Then
            '       If (dispflglist.Count = 1) Then
            '           .Append("AND NAMETITLE_TYPE = '" & CType(dispflglist.Item(0), String) & "' ")
            '       Else
            '           Dim i As Integer = 0
            '           .Append("AND NAMETITLE_TYPE IN ( ")
            '           For i = 0 To dispflglist.Count - 1
            '               If (i > 0) Then
            '                   .Append(" , ")
            '               End If
            '               .Append(" '" & CType(dispflglist.Item(i), String) & "' ")
            '           Next
            '               .Append(" ) ")
            '       End If
            '   End If
            '   .Append("ORDER BY ")
            '   .Append("    NAMETITLE_CD ")
            'End With
            With sql
                .Append("SELECT ")
                .Append("  /* SC3080205_004 */ ")
                '2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) START
                .Append("  T1.NAMETITLE_CD AS NAMETITLE_CD, ")
                '2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) END
                .Append("  NAMETITLE_NAME AS NAMETITLE, ")
                .Append("  CASE WHEN NAMETITLE_TYPE = '0' THEN '0' ")
                .Append("       WHEN NAMETITLE_TYPE = '1' THEN '2' ")
                .Append("       WHEN NAMETITLE_TYPE = '2' THEN '1' ")
                .Append("       WHEN NAMETITLE_TYPE = '3' THEN '1' ")
                .Append("  END AS DISPFLG, ")
                '2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) START
                .Append("  NVL(T2.PRIVATE_FLEET_ITEM_CD,' ')  AS PRIVATE_FLEET_ITEM_CD ")
                '2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) END
                .Append("FROM ")
                '2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) START
                .Append("  TB_M_NAMETITLE T1 ")
                .Append("  LEFT JOIN TB_M_PFITEM_NAMETITLE T2 ")
                .Append("  ON T1.NAMETITLE_CD = T2.NAMETITLE_CD ")
                '2014/07/15 TCS 市川 複数の個人法人項目コードにて単一の敬称コードを表示させる(TMT-UAT-BTS-80) END
                .Append("WHERE ")
                .Append("  INUSE_FLG = '1' ")
                If (Not IsNothing(dispflglist) AndAlso (dispflglist.Count > 0)) Then
                    If (dispflglist.Count = 1) Then
                        .Append("AND NAMETITLE_TYPE = '" & CType(dispflglist.Item(0), String) & "' ")
                    Else
                        Dim i As Integer = 0
                        .Append("AND NAMETITLE_TYPE IN ( ")
                        For i = 0 To dispflglist.Count - 1
                            If (i > 0) Then
                                .Append(" , ")
                            End If
                            .Append(" '" & CType(dispflglist.Item(i), String) & "' ")
                        Next
                        .Append(" ) ")
                    End If
                End If
                .Append("ORDER BY ")
                .Append("    NAMETITLE_CD ")
            End With
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNametitle_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            query.CommandText = sql.ToString()

            Return query.GetData()

        End Using

    End Function

    '2013/06/30 TCS 宋 2013/10対応版　既存流用 START
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
    ''' <summary>
    ''' 自社客個人情報更新
    ''' </summary>
    ''' <param name="originalid">自社客連番</param>
    ''' <param name="socialid">国民ID、免許証番号等</param>
    ''' <param name="custype">個人/法人区分</param>
    ''' <param name="name">顧客指名</param>
    ''' <param name="firstname">ファーストネーム</param>
    ''' <param name="middlename">ミドルネーム</param>
    ''' <param name="lastname">ラストネーム</param>
    ''' <param name="nametitlecd">敬称コード</param>
    ''' <param name="nametitle">敬称</param>
    ''' <param name="zipcode">郵便番号</param>
    ''' <param name="address">住所</param>
    ''' <param name="address1">住所1</param>
    ''' <param name="address2">住所2</param>
    ''' <param name="address3">住所3</param>
    ''' <param name="state">住所(州)</param>
    ''' <param name="district">住所(地域)</param>
    ''' <param name="city">住所(市)</param>
    ''' <param name="location">住所(地区)</param>
    ''' <param name="telno">自宅電話番号</param>
    ''' <param name="mobile">携帯電話番号</param>
    ''' <param name="faxno">FAX番号</param>
    ''' <param name="businesstelno">勤務地電話番号</param>
    ''' <param name="email1">E-mailアドレス１</param>
    ''' <param name="email2">E-mailアドレス２</param>
    ''' <param name="sex">性別</param>
    ''' <param name="birthday">生年月日</param>
    ''' <param name="employeename">担当者氏名（法人）</param>
    ''' <param name="employeedepartment">担当者部署名（法人）</param>
    ''' <param name="employeeposition">役職（法人）</param>
    ''' <param name="privatefleetitem">個人法人項目コード</param>
    ''' <param name="domicile">本籍</param>
    ''' <param name="country">国籍</param>
    ''' <param name="cstIncome">収入</param>
    ''' <param name="updatefuncflg">顧客更新フラグ</param>
    ''' <param name="updateaccount">更新アカウント</param>
    ''' <param name="lockversion">ロックバージョン</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Function UpdateCustomer(ByVal originalid As String, _
                                    ByVal socialid As String, _
                                    ByVal custype As String, _
                                    ByVal name As String, _
                                    ByVal firstname As String, _
                                    ByVal middlename As String, _
                                    ByVal lastname As String, _
                                    ByVal nametitlecd As String, _
                                    ByVal nametitle As String, _
                                    ByVal zipcode As String, _
                                    ByVal address As String, _
                                    ByVal address1 As String, _
                                    ByVal address2 As String, _
                                    ByVal address3 As String, _
                                    ByVal state As String, _
                                    ByVal district As String, _
                                    ByVal city As String, _
                                    ByVal location As String, _
                                    ByVal telno As String, _
                                    ByVal mobile As String, _
                                    ByVal faxno As String, _
                                    ByVal businesstelno As String, _
                                    ByVal email1 As String, _
                                    ByVal email2 As String, _
                                    ByVal sex As String, _
                                    ByVal birthday As Nullable(Of DateTime), _
                                    ByVal employeename As String, _
                                    ByVal employeedepartment As String, _
                                    ByVal employeeposition As String, _
                                    ByVal privatefleetitem As String, _
                                    ByVal domicile As String, _
                                    ByVal country As String, _
                                    ByVal cstIncome As String, _
                                    ByVal updatefuncflg As String, _
                                    ByVal updateaccount As String, _
                                    ByVal lockversion As Long) As Integer
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomer_Start")
        'ログ出力 End *****************************************************************************

        If (IsEnabled(IdCustype) = True) Then
            If (custype = "0") Then
                custype = "1"
            Else
                If (custype = "1") Then
                    custype = "0"
                End If
            End If
        End If
        If (birthday Is Nothing) Then
            birthday = #1/1/1900#
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        '  With sql
        '      .Append("UPDATE ")
        '      .Append("    /* SC3080205_204 */ ")
        '      .Append("    TB_M_CUSTOMER ")
        '      .Append("SET ")
        '      If (IsEnabled(IdSocialid) = True) Then .Append("    CST_SOCIALNUM = :SOCIALID, ") '国民ID、免許証番号等
        '      If (IsEnabled(IdCustype) = True) Then .Append("    FLEET_FLG = :CUSTYPE, ") '個人/法人区分
        '      If (IsEnabled(IdName) = True) Then .Append("    CST_NAME = :NAME, ") '顧客氏名
        '      If (IsEnabled(IdNameTitlecd) = True) Then .Append("    NAMETITLE_CD = :NAMETITLE_CD, ") '敬称コード
        '      If (IsEnabled(IdNameTitle) = True) Then .Append("    NAMETITLE_NAME = :NAMETITLE, ") '敬称
        '      If (IsEnabled(IdZipcode) = True) Then .Append("    CST_ZIPCD = :ZIPCODE, ") '郵便番号
        '      If (IsEnabled(IdAddress) = True) Then .Append("    CST_ADDRESS = :ADDRESS, ") '住所
        '      If (IsEnabled(Idtelno) = True) Then .Append("    CST_PHONE = :TELNO, ") '自宅電話番号
        '      If (IsEnabled(IdMobile) = True) Then .Append("    CST_MOBILE = :MOBILE, ") '携帯電話番号
        '      If (IsEnabled(IdFaxno) = True) Then .Append("    CST_FAX = :FAXNO, ") 'FAX番号
        '      If (IsEnabled(IdBusinessTelno) = True) Then .Append("    CST_BIZ_PHONE = :BUSINESSTELNO, ") '勤務地電話番号
        '      If (IsEnabled(IdEmail1) = True) Then .Append("    CST_EMAIL_1 = :EMAIL1, ") 'E-MAILアドレス１
        '      If (IsEnabled(IdEmail2) = True) Then .Append("    CST_EMAIL_2 = :EMAIL2, ") 'E-MAILアドレス２
        '      If (IsEnabled(IdSex) = True) Then .Append("    CST_GENDER = :SEX, ") '性別
        '      If (IsEnabled(IdBirthday) = True) Then .Append("    CST_BIRTH_DATE = :BIRTHDAY, ") '生年月日
        '      If (IsEnabled(IdEmployeeName) = True) Then .Append("    FLEET_PIC_NAME = :EMPLOYEENAME, ") '担当者氏名（法人）
        '      If (IsEnabled(IdEmployeeDepartment) = True) Then .Append("    FLEET_PIC_DEPT = :EMPLOYEEDEPARTMENT, ") '担当者部署名（法人）
        '      If (IsEnabled(IdEmployeePosition) = True) Then .Append("    FLEET_PIC_POSITION = :EMPLOYEEPOSITION, ") '役職（法人）
        '      If Not (updatefuncflg = "") Then .Append("    UPDATE_FUNCTION_JUDGE = :UPDATEFUNCFLG, ")
        '      .Append("    CST_REG_STATUS = '0', ")
        '      .Append("    ROW_UPDATE_DATETIME = SYSDATE, ")
        '      .Append("    ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT, ")
        '      .Append("    ROW_UPDATE_FUNCTION = 'SC3080205', ")
        '      .Append("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
        '      .Append("WHERE ")
        '      .Append("        CST_ID = :ORIGINALID ")
        '      .Append("    AND ROW_LOCK_VERSION = :ROWLOCKVERSION ")
        '  End With
        With sql
            .Append("UPDATE ")
            .Append("    /* SC3080205_204 */ ")
            .Append("    TB_M_CUSTOMER ")
            .Append("SET ")
            If (IsEnabled(IdSocialid) = True) Then .Append("    CST_SOCIALNUM = :SOCIALID, ") '国民ID、免許証番号等
            If (IsEnabled(IdCustype) = True) Then .Append("    FLEET_FLG = :CUSTYPE, ") '個人/法人区分
            If (IsEnabled(IdName) = True) Then .Append("    CST_NAME = :NAME, ") '顧客氏名
            If (IsEnabled(IdFirstName) = True) Then .Append("    FIRST_NAME = :FIRST_NAME, ") 'ファーストネーム
            If (IsEnabled(IdMiddleName) = True) Then .Append("    MIDDLE_NAME = :MIDDLE_NAME, ") 'ミドルネーム
            If (IsEnabled(IdLastName) = True) Then .Append("    LAST_NAME = :LAST_NAME, ") 'ラストネーム
            If (IsEnabled(IdNameTitlecd) = True) Then .Append("    NAMETITLE_CD = :NAMETITLE_CD, ") '敬称コード
            If (IsEnabled(IdNameTitle) = True) Then .Append("    NAMETITLE_NAME = :NAMETITLE, ") '敬称
            If (IsEnabled(IdZipcode) = True) Then .Append("    CST_ZIPCD = :ZIPCODE, ") '郵便番号
            If (IsEnabled(IdAddress) = True) Then .Append("    CST_ADDRESS = :ADDRESS, ") '住所
            If (IsEnabled(IdAddress1) = True) Then .Append("    CST_ADDRESS_1 = :ADDRESS1, ") '住所1
            If (IsEnabled(IdAddress2) = True) Then .Append("    CST_ADDRESS_2 = :ADDRESS2, ") '住所2
            If (IsEnabled(IdAddress3) = True) Then .Append("    CST_ADDRESS_3 = :ADDRESS3, ") '住所3
            If (IsEnabled(IdState) = True) Then .Append("    CST_ADDRESS_STATE = :STATE, ") '住所(州)
            If (IsEnabled(IdDistrict) = True) Then .Append("    CST_ADDRESS_DISTRICT = :DISTRICT, ") '住所(地域)
            If (IsEnabled(IdCity) = True) Then .Append("    CST_ADDRESS_CITY = :CITY, ") '住所(市)
            If (IsEnabled(IdLocation) = True) Then .Append("    CST_ADDRESS_LOCATION = :LOCATION, ") '住所(地区)
            If (IsEnabled(Idtelno) = True) Then .Append("    CST_PHONE = :TELNO, ") '自宅電話番号
            If (IsEnabled(IdMobile) = True) Then .Append("    CST_MOBILE = :MOBILE, ") '携帯電話番号
            If (IsEnabled(IdFaxno) = True) Then .Append("    CST_FAX = :FAXNO, ") 'FAX番号
            If (IsEnabled(IdBusinessTelno) = True) Then .Append("    CST_BIZ_PHONE = :BUSINESSTELNO, ") '勤務地電話番号
            If (IsEnabled(IdEmail1) = True) Then .Append("    CST_EMAIL_1 = :EMAIL1, ") 'E-MAILアドレス１
            If (IsEnabled(IdEmail2) = True) Then .Append("    CST_EMAIL_2 = :EMAIL2, ") 'E-MAILアドレス２
            If (IsEnabled(IdSex) = True) Then .Append("    CST_GENDER = :SEX, ") '性別
            If (IsEnabled(IdBirthday) = True) Then .Append("    CST_BIRTH_DATE = :BIRTHDAY, ") '生年月日
            If (IsEnabled(IdEmployeeName) = True) Then .Append("    FLEET_PIC_NAME = :EMPLOYEENAME, ") '担当者氏名（法人）
            If (IsEnabled(IdEmployeeDepartment) = True) Then .Append("    FLEET_PIC_DEPT = :EMPLOYEEDEPARTMENT, ") '担当者部署名（法人）
            If (IsEnabled(IdEmployeePosition) = True) Then .Append("    FLEET_PIC_POSITION = :EMPLOYEEPOSITION, ") '役職（法人）
            If (IsEnabled(IdPrivateFleetItem) = True) Then .Append("    PRIVATE_FLEET_ITEM_CD = :PRIVATE_FLEET_ITEM, ") '役職（法人）
            If (IsEnabled(IdDomicile) = True) Then .Append("    CST_DOMICILE = :DOMICILE, ") '役職（法人）
            If (IsEnabled(IdCountry) = True) Then .Append("    CST_COUNTRY = :COUNTRY, ") '役職（法人）
            '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
            If IsEnabled(IdCstIncome) Then .Append("    CST_INCOME = :CST_INCOME , ") '顧客収入
            '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
            If Not (updatefuncflg = "") Then .Append("    UPDATE_FUNCTION_JUDGE = :UPDATEFUNCFLG, ")
            .Append("    CST_REG_STATUS = '0', ")
            .Append("    ROW_UPDATE_DATETIME = SYSDATE, ")
            .Append("    ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION = 'SC3080205', ")
            .Append("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
            .Append("WHERE ")
            .Append("        CST_ID = :ORIGINALID ")
            .Append("    AND ROW_LOCK_VERSION = :ROWLOCKVERSION ")
        End With
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        Using query As New DBUpdateQuery("SC3080205_204")
            query.CommandText = sql.ToString()

            If (IsEnabled(IdSocialid) = True) Then query.AddParameterWithTypeValue("SOCIALID", OracleDbType.NVarchar2, socialid) '国民番号
            If (IsEnabled(IdCustype) = True) Then query.AddParameterWithTypeValue("CUSTYPE", OracleDbType.NVarchar2, custype) '顧客タイプ
            If (IsEnabled(IdName) = True) Then query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, name) '氏名
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            If (IsEnabled(IdFirstName) = True) Then query.AddParameterWithTypeValue("FIRST_NAME", OracleDbType.NVarchar2, firstname) 'ファーストネーム
            If (IsEnabled(IdMiddleName) = True) Then query.AddParameterWithTypeValue("MIDDLE_NAME", OracleDbType.NVarchar2, middlename) 'ミドルネーム
            If (IsEnabled(IdLastName) = True) Then query.AddParameterWithTypeValue("LAST_NAME", OracleDbType.NVarchar2, lastname) 'ラストネーム
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
            If (IsEnabled(IdNameTitlecd) = True) Then query.AddParameterWithTypeValue("NAMETITLE_CD", OracleDbType.NVarchar2, nametitlecd) '敬称コード
            If (IsEnabled(IdNameTitle) = True) Then query.AddParameterWithTypeValue("NAMETITLE", OracleDbType.NVarchar2, nametitle) '敬称
            If (IsEnabled(IdZipcode) = True) Then query.AddParameterWithTypeValue("ZIPCODE", OracleDbType.NVarchar2, zipcode) '郵便番号
            If (IsEnabled(IdAddress) = True) Then query.AddParameterWithTypeValue("ADDRESS", OracleDbType.NVarchar2, address) '住所
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            If (IsEnabled(IdAddress1) = True) Then query.AddParameterWithTypeValue("ADDRESS1", OracleDbType.NVarchar2, address1) '住所1
            If (IsEnabled(IdAddress2) = True) Then query.AddParameterWithTypeValue("ADDRESS2", OracleDbType.NVarchar2, address2) '住所2
            If (IsEnabled(IdAddress3) = True) Then query.AddParameterWithTypeValue("ADDRESS3", OracleDbType.NVarchar2, address3) '住所3
            If (IsEnabled(IdState) = True) Then query.AddParameterWithTypeValue("STATE", OracleDbType.NVarchar2, state) '住所(州)
            If (IsEnabled(IdDistrict) = True) Then query.AddParameterWithTypeValue("DISTRICT", OracleDbType.NVarchar2, district) '住所(地域)
            If (IsEnabled(IdCity) = True) Then query.AddParameterWithTypeValue("CITY", OracleDbType.NVarchar2, city) '住所(市)
            If (IsEnabled(IdLocation) = True) Then query.AddParameterWithTypeValue("LOCATION", OracleDbType.NVarchar2, location) '住所(地区)
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
            If (IsEnabled(Idtelno) = True) Then query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, telno) '電話番号
            If (IsEnabled(IdMobile) = True) Then query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, mobile) '携帯電話番号
            If (IsEnabled(IdFaxno) = True) Then query.AddParameterWithTypeValue("FAXNO", OracleDbType.NVarchar2, faxno) 'FAX番号
            If (IsEnabled(IdBusinessTelno) = True) Then query.AddParameterWithTypeValue("BUSINESSTELNO", OracleDbType.NVarchar2, businesstelno) '勤め先電話番号
            If (IsEnabled(IdEmail1) = True) Then query.AddParameterWithTypeValue("EMAIL1", OracleDbType.NVarchar2, email1) 'E-MAILアドレス1
            If (IsEnabled(IdEmail2) = True) Then query.AddParameterWithTypeValue("EMAIL2", OracleDbType.NVarchar2, email2) 'E-MAILアドレス2
            If (IsEnabled(IdSex) = True) Then query.AddParameterWithTypeValue("SEX", OracleDbType.NVarchar2, sex) '性別
            If (IsEnabled(IdBirthday) = True) Then query.AddParameterWithTypeValue("BIRTHDAY", OracleDbType.Date, birthday) '生年月日
            If (IsEnabled(IdEmployeeName) = True) Then query.AddParameterWithTypeValue("EMPLOYEENAME", OracleDbType.NVarchar2, employeename) '担当者名
            If (IsEnabled(IdEmployeeDepartment) = True) Then query.AddParameterWithTypeValue("EMPLOYEEDEPARTMENT", OracleDbType.NVarchar2, employeedepartment) '担当者所属部署
            If (IsEnabled(IdEmployeePosition) = True) Then query.AddParameterWithTypeValue("EMPLOYEEPOSITION", OracleDbType.NVarchar2, employeeposition) '担当者役職
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            If (IsEnabled(IdPrivateFleetItem) = True) Then query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM", OracleDbType.NVarchar2, privatefleetitem) '個人法人項目コード
            If (IsEnabled(IdDomicile) = True) Then query.AddParameterWithTypeValue("DOMICILE", OracleDbType.NVarchar2, domicile) '本籍
            If (IsEnabled(IdCountry) = True) Then query.AddParameterWithTypeValue("COUNTRY", OracleDbType.NVarchar2, country) '国籍
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
            '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
            If IsEnabled(IdCstIncome) Then query.AddParameterWithTypeValue("CST_INCOME", OracleDbType.NVarchar2, cstIncome) '顧客収入
            '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
            If Not (updatefuncflg = "") Then query.AddParameterWithTypeValue("UPDATEFUNCFLG", OracleDbType.NVarchar2, updatefuncflg) '最終更新機能
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount) '更新アカウント
            query.AddParameterWithTypeValue("ROWLOCKVERSION", OracleDbType.Int64, lockversion)

            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomer_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 宋 2013/10対応版　既存流用 END

            Return query.Execute()

        End Using

    End Function
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

    '2013/06/30 TCS 宋 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客個人情報更新
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstid">自社客連番</param>
    ''' <param name="actvctgryid">AC</param>
    ''' <param name="acmodffuncdvs">AC変更機能</param>
    ''' <param name="reasonid">活動除外理由ID</param>
    ''' <param name="updateaccount">更新アカウント</param>
    ''' <param name="lockversion">ロックバージョン</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Function UpdateNewcustomer(ByVal dlrcd As String, _
                                      ByVal cstid As String, _
                                      ByVal actvctgryid As String, _
                                      ByVal acmodffuncdvs As String, _
                                      ByVal reasonid As String, _
                                      ByVal updateaccount As String, _
                                      ByVal lockversion As Long, _
                                      ByVal vclid As String) As Integer

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateNewcustomer_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("UPDATE /* SC3080205_205 */ ")
            .Append("    TB_M_CUSTOMER_VCL ")
            .Append("SET ")
            If (IsEnabled(IdACtvctgryid) = True) Then .Append("    ACT_CAT_TYPE = :ACTVCTGRYID, ")
            If (IsEnabled(IdACModffuncDvs) = True) Then .Append("    ACT_CAT_UPDATE_FUNCTION = :AC_MODFFUNCDVS, ")
            If (IsEnabled(IdReasonId) = True) Then
                If reasonid = "" Then
                    .Append("    OMIT_REASON_CD = ' ', ")
                Else
                    .Append("    OMIT_REASON_CD = :REASONID, ")
                End If
            End If
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            .Append("    ACT_CAT_UPDATE_DATETIME = SYSDATE, ")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            ' 2014/05/01 TCS 松月 新PF残課題No.21 Modify Start
            .Append("    ACT_CAT_UPDATE_STF_CD = :UPDATEACCOUNT, ")
            ' 2014/05/01 TCS 松月 新PF残課題No.21 Modify End
            .Append("    ROW_UPDATE_DATETIME = SYSDATE, ")
            .Append("    ROW_UPDATE_ACCOUNT = :UPDATEACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION = 'SC3080205', ")
            .Append("    ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1 ")
            .Append("WHERE ")
            .Append("    DLR_CD = :DLRCD AND ")
            .Append("    CST_ID = :CSTID AND ")
            .Append("    VCL_ID = :VCLID AND ")
            .Append("    ROW_LOCK_VERSION = :ROWLOCKVERSION ")
        End With

        Using query As New DBUpdateQuery("SC3080205_205")

            query.CommandText = sql.ToString()
            If (IsEnabled(IdACtvctgryid) = True) Then query.AddParameterWithTypeValue("ACTVCTGRYID", OracleDbType.NVarchar2, actvctgryid)
            If (IsEnabled(IdACModffuncDvs) = True) Then query.AddParameterWithTypeValue("AC_MODFFUNCDVS", OracleDbType.NVarchar2, acmodffuncdvs)
            If (IsEnabled(IdReasonId) = True) Then
                If reasonid = "" Then
                Else
                    query.AddParameterWithTypeValue("REASONID", OracleDbType.NVarchar2, reasonid)
                End If
            End If
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("ROWLOCKVERSION", OracleDbType.Int64, lockversion)
            query.AddParameterWithTypeValue("VCLID", OracleDbType.Decimal, vclid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateNewcustomer_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 宋 2013/10対応版　既存流用 END

            Return query.Execute()

        End Using

    End Function

    '項目毎の可視/非可視状態を取得する
    Private Function IsEnabled(ByVal ID As Integer) As Boolean

        If (CType(EnabledTable.Item(ID), Boolean) = True) Then
            Return True
        Else
            Return False
        End If

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
    ''' <summary>
    ''' 未取引客個人情報新規作成
    ''' </summary>
    ''' <param name="cstid">未取引客ユーザーID</param>
    ''' <param name="custype">法人/個人</param>
    ''' <param name="employeename">担当者名</param>
    ''' <param name="employeedepartment">担当者所属部署</param>
    ''' <param name="employeeposition">担当者役職</param>
    ''' <param name="socialid">国民番号</param>
    ''' <param name="name">氏名</param>
    ''' <param name="firstname">ファーストネーム</param>
    ''' <param name="middlename">ミドルネーム</param>
    ''' <param name="lastname">ラストネーム</param>
    ''' <param name="nametitlecd">敬称コード</param>
    ''' <param name="nametitle">敬称名称</param>
    ''' <param name="sex">性別</param>
    ''' <param name="zipcode">郵便番号</param>
    ''' <param name="address">住所</param>
    ''' <param name="address1">住所1</param>
    ''' <param name="address2">住所2</param>
    ''' <param name="address3">住所3</param>
    ''' <param name="state">住所(州)</param>
    ''' <param name="district">住所(地域)</param>
    ''' <param name="city">住所(市)</param>
    ''' <param name="location">住所(地区)</param>
    ''' <param name="telno">電話番号</param>
    ''' <param name="mobile">携帯電話番号</param>
    ''' <param name="faxno">FAX番号</param>
    ''' <param name="businesstelno">勤め先電話番号</param>
    ''' <param name="email1">E-MAILアドレス1</param>
    ''' <param name="email2">E-MAILアドレス2</param>
    ''' <param name="birthday">生年月日</param>
    ''' <param name="privatefleetitem">個人法人項目コード</param>
    ''' <param name="domicile">本籍</param>
    ''' <param name="country">国籍</param>
    ''' <param name="dummyNameFlg">ダミー名称フラグ</param>
    ''' <param name="updateaccount">更新アカウント</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Function InsertNewcustomer(ByVal cstid As String, _
                                        ByVal custype As String, _
                                        ByVal employeename As String, _
                                        ByVal employeedepartment As String, _
                                        ByVal employeeposition As String, _
                                        ByVal socialid As String, _
                                        ByVal name As String, _
                                        ByVal firstname As String, _
                                        ByVal middlename As String, _
                                        ByVal lastname As String, _
                                        ByVal nametitlecd As String, _
                                        ByVal nametitle As String, _
                                        ByVal sex As String, _
                                        ByVal zipcode As String, _
                                        ByVal address As String, _
                                        ByVal address1 As String, _
                                        ByVal address2 As String, _
                                        ByVal address3 As String, _
                                        ByVal state As String, _
                                        ByVal district As String, _
                                        ByVal city As String, _
                                        ByVal location As String, _
                                        ByVal telno As String, _
                                        ByVal mobile As String, _
                                        ByVal faxno As String, _
                                        ByVal businesstelno As String, _
                                        ByVal email1 As String, _
                                        ByVal email2 As String, _
                                        ByVal birthday As Nullable(Of DateTime), _
                                        ByVal privatefleetitem As String, _
                                        ByVal domicile As String, _
                                        ByVal country As String, _
                                        ByVal cstIncome As String, _
                                        ByVal dummyNameFlg As String, _
                                        ByVal updateaccount As String) As Integer
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertNewcustomer_Start")
        'ログ出力 End *****************************************************************************

        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'With sql
        '    .Append("INSERT /* SC3080205_206 */ ")
        '    .Append("INTO ")
        '    .Append("    TB_M_CUSTOMER ")
        '    .Append("( ")
        '    .Append("    CST_ID, ")
        '    .Append("    DMS_CST_CD, ")
        '    .Append("    DMS_CST_CD_DISP, ")
        '    .Append("    NEWCST_CD, ")
        '    .Append("    ORGCST_CD, ")
        '    .Append("    FLEET_FLG, ")
        '    .Append("    FLEET_PIC_NAME, ")
        '    .Append("    FLEET_PIC_DEPT, ")
        '    .Append("    FLEET_PIC_POSITION, ")
        '    .Append("    CST_SOCIALNUM_TYPE, ")
        '    .Append("    CST_SOCIALNUM, ")
        '    .Append("    CST_NAME, ")
        '    .Append("    NAMETITLE_CD, ")
        '    .Append("    NAMETITLE_NAME, ")
        '    .Append("    FIRST_NAME, ")
        '    .Append("    MIDDLE_NAME, ")
        '    .Append("    LAST_NAME, ")
        '    .Append("    FIRST_NAME_KANA, ")
        '    .Append("    LAST_NAME_KANA, ")
        '    .Append("    NICK_NAME, ")
        '    .Append("    CST_GENDER, ")
        '    .Append("    CST_DOMICILE, ")
        '    .Append("    CST_COUNTRY, ")
        '    .Append("    CST_ZIPCD, ")
        '    .Append("    CST_ADDRESS, ")
        '    .Append("    CST_ADDRESS_1, ")
        '    .Append("    CST_ADDRESS_2, ")
        '    .Append("    CST_ADDRESS_3, ")
        '    .Append("    CST_ADDRESS_STATE, ")
        '    .Append("    CST_ADDRESS_DISTRICT, ")
        '    .Append("    CST_ADDRESS_CITY, ")
        '    .Append("    CST_ADDRESS_LOCATION, ")
        '    .Append("    CST_PHONE, ")
        '    .Append("    CST_MOBILE, ")
        '    .Append("    CST_FAX, ")
        '    .Append("    CST_COMPANY_NAME, ")
        '    .Append("    CST_BIZ_PHONE, ")
        '    .Append("    CST_EMAIL_1, ")
        '    .Append("    CST_EMAIL_2, ")
        '    If Not (birthday Is Nothing) Then
        '        .Append("    CST_BIRTH_DATE, ")
        '    End If
        '    .Append("    CST_INCOME, ")
        '    .Append("    CST_OCCUPATION_ID, ")
        '    .Append("    CST_OCCUPATION, ")
        '    .Append("    MARITAL_TYPE, ")
        '    .Append("    ENG_FLG, ")
        '    .Append("    DMS_TYPE, ")
        '    .Append("    UPDATE_FUNCTION_JUDGE, ")
        '    .Append("    CST_REG_STATUS, ")
        '    .Append("    ROW_CREATE_DATETIME, ")
        '    .Append("    ROW_CREATE_ACCOUNT, ")
        '    .Append("    ROW_CREATE_FUNCTION, ")
        '    .Append("    ROW_UPDATE_DATETIME, ")
        '    .Append("    ROW_UPDATE_ACCOUNT, ")
        '    .Append("    ROW_UPDATE_FUNCTION, ")
        '    .Append("    ROW_LOCK_VERSION ")
        '    .Append(") ")
        '    .Append("VALUES ")
        '    .Append("( ")
        '    .Append("    :CSTID, ")
        '    .Append("    ' ', ")
        '    .Append("    ' ', ")
        '    .Append("    ' ', ")
        '    .Append("    ' ', ")
        '    .Append("    :CUSTYPE, ")
        '    .Append("    :EMPLOYEENAME, ")
        '    .Append("    :EMPLOYEEDEPARTMENT, ")
        '    .Append("    :EMPLOYEEPOSITION, ")
        '    .Append("    ' ', ")
        '    .Append("    :SOCIALID, ")
        '    .Append("    :NAME, ")
        '    .Append("    :NAMETITLE_CD, ")
        '    .Append("    :NAMETITLE, ")
        '    .Append("    ' ', ")
        '    .Append("    ' ', ")
        '    .Append("    ' ', ")
        '    .Append("    ' ', ")
        '    .Append("    ' ', ")
        '    .Append("    ' ', ")
        '    .Append("    :SEX, ")
        '    .Append("    ' ', ")
        '    .Append("    ' ', ")
        '    .Append("    :ZIPCODE, ")
        '    .Append("    :ADDRESS, ")
        '    .Append("    :ADDRESS1, ")
        '    .Append("    :ADDRESS2, ")
        '    .Append("    :ADDRESS3, ")
        '    .Append("    ' ', ")
        '    .Append("    ' ', ")
        '    .Append("    ' ', ")
        '    .Append("    ' ', ")
        '    .Append("    :TELNO, ")
        '    .Append("    :MOBILE, ")
        '    .Append("    :FAXNO, ")
        '    .Append("    ' ', ")
        '    .Append("    :BUSINESSTELNO, ")
        '    .Append("    :EMAIL1, ")
        '    .Append("    :EMAIL2, ")
        '    If Not (birthday Is Nothing) Then
        '        .Append("    :BIRTHDAY, ")
        '    End If
        '    .Append("    ' ', ")
        '    .Append("    0, ")
        '    .Append("    ' ', ")
        '    .Append("    ' ', ")
        '    .Append("    '0', ")
        '    .Append("    '0', ")
        '    .Append("    '11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111', ")
        '    .Append("    :DUMMYFLG, ")
        '    .Append("    SYSDATE, ")
        '    .Append("    :UPDATEACCOUNT, ")
        '    .Append("    'SC3080205', ")
        '    .Append("    SYSDATE, ")
        '    .Append("    :UPDATEACCOUNT, ")
        '    .Append("    'SC3080205', ")
        '    .Append("    0 ")
        '    .Append(") ")
        'End With
        With sql
            .Append("INSERT /* SC3080205_206 */ ")
            .Append("INTO ")
            .Append("    TB_M_CUSTOMER ")
            .Append("( ")
            .Append("    CST_ID, ")
            .Append("    DMS_CST_CD, ")
            .Append("    DMS_CST_CD_DISP, ")
            .Append("    NEWCST_CD, ")
            .Append("    ORGCST_CD, ")
            .Append("    FLEET_FLG, ")
            .Append("    FLEET_PIC_NAME, ")
            .Append("    FLEET_PIC_DEPT, ")
            .Append("    FLEET_PIC_POSITION, ")
            .Append("    CST_SOCIALNUM_TYPE, ")
            .Append("    CST_SOCIALNUM, ")
            .Append("    CST_NAME, ")
            .Append("    NAMETITLE_CD, ")
            .Append("    NAMETITLE_NAME, ")
            .Append("    FIRST_NAME, ")
            .Append("    MIDDLE_NAME, ")
            .Append("    LAST_NAME, ")
            .Append("    FIRST_NAME_KANA, ")
            .Append("    LAST_NAME_KANA, ")
            .Append("    NICK_NAME, ")
            .Append("    CST_GENDER, ")
            .Append("    CST_DOMICILE, ")
            .Append("    CST_COUNTRY, ")
            .Append("    CST_ZIPCD, ")
            .Append("    CST_ADDRESS, ")
            .Append("    CST_ADDRESS_1, ")
            .Append("    CST_ADDRESS_2, ")
            .Append("    CST_ADDRESS_3, ")
            .Append("    CST_ADDRESS_STATE, ")
            .Append("    CST_ADDRESS_DISTRICT, ")
            .Append("    CST_ADDRESS_CITY, ")
            .Append("    CST_ADDRESS_LOCATION, ")
            .Append("    CST_PHONE, ")
            .Append("    CST_MOBILE, ")
            .Append("    CST_FAX, ")
            .Append("    CST_COMPANY_NAME, ")
            .Append("    CST_BIZ_PHONE, ")
            .Append("    CST_EMAIL_1, ")
            .Append("    CST_EMAIL_2, ")
            If Not (birthday Is Nothing) Then
                .Append("    CST_BIRTH_DATE, ")
            End If
            .Append("    CST_INCOME, ")
            .Append("    CST_OCCUPATION_ID, ")
            .Append("    CST_OCCUPATION, ")
            .Append("    MARITAL_TYPE, ")
            .Append("    ENG_FLG, ")
            .Append("    DMS_TYPE, ")
            .Append("    UPDATE_FUNCTION_JUDGE, ")
            .Append("    PRIVATE_FLEET_ITEM_CD, ")
            .Append("    CST_REG_STATUS, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ")
            .Append("( ")
            .Append("    :CSTID, ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    :CUSTYPE, ")
            .Append("    :EMPLOYEENAME, ")
            .Append("    :EMPLOYEEDEPARTMENT, ")
            .Append("    :EMPLOYEEPOSITION, ")
            .Append("    ' ', ")
            .Append("    :SOCIALID, ")
            .Append("    :NAME, ")
            .Append("    :NAMETITLE_CD, ")
            .Append("    :NAMETITLE, ")
            .Append("    :FIRSTNAME, ")
            .Append("    :MIDDLENAME, ")
            .Append("    :LASTNAME, ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    :SEX, ")
            .Append("    :DOMICILE, ")
            .Append("    :COUNTRY, ")
            .Append("    :ZIPCODE, ")
            .Append("    :ADDRESS, ")
            .Append("    :ADDRESS1, ")
            .Append("    :ADDRESS2, ")
            .Append("    :ADDRESS3, ")
            .Append("    :STATE, ")
            .Append("    :DISTRICT, ")
            .Append("    :CITY, ")
            .Append("    :LOCATION, ")
            .Append("    :TELNO, ")
            .Append("    :MOBILE, ")
            .Append("    :FAXNO, ")
            .Append("    ' ', ")
            .Append("    :BUSINESSTELNO, ")
            .Append("    :EMAIL1, ")
            .Append("    :EMAIL2, ")
            If Not (birthday Is Nothing) Then
                .Append("    :BIRTHDAY, ")
            End If
            '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
            .Append("    :CST_INCOME, ")
            '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
            .Append("    0, ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    '0', ")
            .Append("    '0', ")
            .Append("    '11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111', ")
            .Append("    :PRIVATE_FLEET_ITEM, ")
            .Append("    :DUMMYFLG, ")
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080205', ")
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080205', ")
            .Append("    0 ")
            .Append(") ")
        End With
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        Using query As New DBUpdateQuery("SC3080205_206")

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)   '未取引客ユーザーID
            If custype = "1" Then
                custype = "0"
            Else
                If custype = "0" Then
                    custype = "1"
                End If
            End If
            query.AddParameterWithTypeValue("CUSTYPE", OracleDbType.NVarchar2, custype)
            query.AddParameterWithTypeValue("EMPLOYEENAME", OracleDbType.NVarchar2, employeename)   '担当者名
            query.AddParameterWithTypeValue("EMPLOYEEDEPARTMENT", OracleDbType.NVarchar2, employeedepartment)   '担当者所属部署
            query.AddParameterWithTypeValue("EMPLOYEEPOSITION", OracleDbType.NVarchar2, employeeposition)   '担当者役職
            query.AddParameterWithTypeValue("SOCIALID", OracleDbType.NVarchar2, socialid)   '国民番号
            query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, name)   '氏名
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            query.AddParameterWithTypeValue("FIRSTNAME", OracleDbType.NVarchar2, firstname)   'ファーストネーム
            query.AddParameterWithTypeValue("MIDDLENAME", OracleDbType.NVarchar2, middlename)   'ミドルネーム
            query.AddParameterWithTypeValue("LASTNAME", OracleDbType.NVarchar2, lastname)   'ラストネーム
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
            query.AddParameterWithTypeValue("NAMETITLE_CD", OracleDbType.NVarchar2, nametitlecd)   '敬称コード
            query.AddParameterWithTypeValue("NAMETITLE", OracleDbType.NVarchar2, nametitle)   '敬称名称
            query.AddParameterWithTypeValue("SEX", OracleDbType.NVarchar2, sex)   '性別
            query.AddParameterWithTypeValue("ZIPCODE", OracleDbType.NVarchar2, zipcode)   '郵便番号
            query.AddParameterWithTypeValue("ADDRESS", OracleDbType.NVarchar2, address)   '住所
            If address1 = String.Empty Then
                address1 = " "
            End If
            If address2 = String.Empty Then
                address2 = " "
            End If
            If address3 = String.Empty Then
                address3 = " "
            End If
            query.AddParameterWithTypeValue("ADDRESS1", OracleDbType.NVarchar2, address1)   '住所1
            query.AddParameterWithTypeValue("ADDRESS2", OracleDbType.NVarchar2, address2)   '住所2
            query.AddParameterWithTypeValue("ADDRESS3", OracleDbType.NVarchar2, address3)   '住所3
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            query.AddParameterWithTypeValue("STATE", OracleDbType.NVarchar2, state)   '住所(州)
            query.AddParameterWithTypeValue("DISTRICT", OracleDbType.NVarchar2, district)   '住所(地域)
            query.AddParameterWithTypeValue("CITY", OracleDbType.NVarchar2, city)   '住所(市)
            query.AddParameterWithTypeValue("LOCATION", OracleDbType.NVarchar2, location)   '住所(地区)
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
            query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, telno)   '電話番号
            query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, mobile)   '携帯電話番号
            query.AddParameterWithTypeValue("FAXNO", OracleDbType.NVarchar2, faxno)   'FAX番号
            query.AddParameterWithTypeValue("BUSINESSTELNO", OracleDbType.NVarchar2, businesstelno)   '勤め先電話番号
            query.AddParameterWithTypeValue("EMAIL1", OracleDbType.NVarchar2, email1)   'E-MAILアドレス1
            query.AddParameterWithTypeValue("EMAIL2", OracleDbType.NVarchar2, email2)   'E-MAILアドレス2
            If Not (birthday Is Nothing) Then
                query.AddParameterWithTypeValue("BIRTHDAY", OracleDbType.Date, birthday)   '生年月日
            End If
            '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
            query.AddParameterWithTypeValue("CST_INCOME", OracleDbType.NVarchar2, cstIncome)    '年収
            '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM", OracleDbType.NVarchar2, privatefleetitem)   '個人法人項目コード
            query.AddParameterWithTypeValue("DOMICILE", OracleDbType.NVarchar2, domicile)   '本籍
            query.AddParameterWithTypeValue("COUNTRY", OracleDbType.NVarchar2, country)   '国籍
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)
            query.AddParameterWithTypeValue("DUMMYFLG", OracleDbType.NVarchar2, dummyNameFlg)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertNewcustomer_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.Execute()

        End Using

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 顧客シーケンス采番
    ''' </summary>
    ''' <returns>SC3080205OrgCustomerDataTableDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetNewcustseq() As Decimal

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205SeqDataTable)("SC3080205_207")

            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewcustseq_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT /* SC3080205_207 */ ")
                .Append("    SQ_CUSTOMER.NEXTVAL AS SEQ ")
                .Append("FROM ")
                .Append("    DUAL ")
            End With
            query.CommandText = sql.ToString()

            Dim seqTbl As SC3080205DataSet.SC3080205SeqDataTable

            seqTbl = query.GetData()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNewcustseq_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return seqTbl.Item(0).Seq

        End Using

    End Function

    ''' <summary>
    ''' 断念理由リスト取得
    ''' </summary>
    ''' <returns>SC3080205OrgCustomerDataTableDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetGiveupReason() As SC3080205DataSet.SC3080205OmitreasonDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205OmitreasonDataTable)("SC3080205_029")

            Dim sql As New StringBuilder
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetGiveupReason_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT /* SC3080205_208 */ ")
                '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) START
                .Append("    ACT_CAT_TYPE, ")
                '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) END
                .Append("    OMIT_REASON_CD AS REASONID, ")
                .Append("    OMIT_REASON AS REASON ")
                .Append("FROM ")
                .Append("    TB_M_OMIT_REASON ")
                .Append("WHERE ")
                '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) START
                .Append("    ACT_CAT_TYPE in ('2','3','4') ")
                '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) END
                .Append("    AND DISP_FLG = '1' ")
                .Append("    AND INUSE_FLG = '1' ")
                .Append("ORDER BY ")
                .Append("    OMIT_REASON_CD ")
            End With

            query.CommandText = sql.ToString()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetGiveupReason_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.GetData()

        End Using

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 顧客テーブルロック処理
    ''' </summary>
    ''' <param name="cstid">顧客ID</param>
    ''' <remarks></remarks>
    Public Shared Sub SelectCstLock(ByVal cstid As String)

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectCstLock_Start")
        'ログ出力 End *****************************************************************************
        Using query As New DBSelectQuery(Of DataTable)("SC3080205_209")

            Dim env As New SystemEnvSetting
            Dim sqlForUpdate As String = " FOR UPDATE WAIT " + env.GetLockWaitTime()

            Dim sql As New StringBuilder

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080205_209 */ ")
                .Append("1 ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER ")
                .Append("WHERE ")
                .Append("  CST_ID = :CSTID ")
                .Append(sqlForUpdate)
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)

            query.GetData()
        End Using

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectCstLock_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Sub


    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
    ''' <summary>
    ''' 未取引客個人情報(販売店)新規作成
    ''' </summary>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="cstid">顧客ID </param>
    ''' <param name="commercialRecvType">商業情報受取区分</param>
    ''' <param name="updateaccount">作成アカウント</param>
    ''' <remarks></remarks>
    Public Shared Function InsertNewcustome_dlr(ByVal dlrcd As String, _
                           ByVal cstid As String, _
                           ByVal commercialRecvType As String, _
                           ByVal updateaccount As String) As Integer
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertNewcustome_dlr_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT ")
            .Append("    /* SC3080205_210 */ ")
            .Append("INTO TB_M_CUSTOMER_DLR ( ")
            .Append("    DLR_CD, ")
            .Append("    CST_ID, ")
            .Append("    TERRITORY_FLG, ")
            .Append("    CONTACT_MTD_DM, ")
            .Append("    CONTACT_MTD_PHONE, ")
            .Append("    CONTACT_MTD_MOBILE, ")
            .Append("    CONTACT_MTD_EMAIL, ")
            .Append("    CONTACT_MTD_DMAIL, ")
            .Append("    CONTACT_MTD_SMS, ")
            .Append("    IMG_FILE_LARGE, ")
            .Append("    IMG_FILE_MEDIUM, ")
            .Append("    IMG_FILE_SMALL, ")
            .Append("    FAMILY_AMOUNT, ")
            .Append("    SNS_1_ACCOUNT, ")
            .Append("    SNS_2_ACCOUNT, ")
            .Append("    SNS_3_ACCOUNT, ")
            .Append("    INTERNET_KEYWORD, ")
            .Append("    VIP_FLG, ")
            .Append("    UPDATE_FUNCTION_JUDGE, ")
            .Append("    CST_TYPE, ")
            .Append("    LAST_CALL_PHONE, ")
            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
            .Append("    COMMERCIAL_RECV_TYPE, ")
            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :DLRCD, ")
            .Append("    :CSTID, ")
            .Append("    '0', ")
            .Append("    '0', ")
            .Append("    '0', ")
            .Append("    '0', ")
            .Append("    '0', ")
            .Append("    '0', ")
            .Append("    '0', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    0, ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    '0', ")
            .Append("    '11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111', ")
            .Append("    '2', ")
            .Append("    ' ', ")
            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
            .Append("    :COMMERCIAL_RECV_TYPE, ")
            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080205', ")
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080205', ")
            .Append("0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080205_210")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
            query.AddParameterWithTypeValue("COMMERCIAL_RECV_TYPE", OracleDbType.NVarchar2, commercialRecvType)
            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertNewcustome_dlr_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.Execute()

        End Using

    End Function

    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
    ''' <summary>
    ''' 顧客個人情報(販売店)更新
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="cstId">顧客ID</param>
    ''' <param name="commercialRecvType">商業情報受取区分</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>更新行数</returns>
    ''' <remarks>ToDo：更新機能判定</remarks>
    Public Function UpdateCustomerDlr(ByVal dlrCd As String, _
                           ByVal cstId As Decimal, _
                           ByVal commercialRecvType As String, _
                           ByVal updatefunctionJudge As String, _
                           ByVal updateAccount As String, _
                           ByVal rowLockVersion As Decimal) As Integer

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomerDlr_Start")
        'ログ出力 End *****************************************************************************

        Dim sql As New StringBuilder(10000)
        Dim ret As Integer = 0

        With sql
            .AppendLine("UPDATE /* SC3080205_220 */ ")
            .AppendLine("TB_M_CUSTOMER_DLR ")
            .AppendLine("SET COMMERCIAL_RECV_TYPE = :COMMERCIAL_RECV_TYPE ")
            .AppendLine("    ,UPDATE_FUNCTION_JUDGE = :UPDATE_FUNCTION_JUDGE ")
            .AppendLine("    ,ROW_UPDATE_DATETIME = SYSDATE ")
            .AppendLine("    ,ROW_UPDATE_ACCOUNT = :UPDATE_STF_CD ")
            .AppendLine("    ,ROW_UPDATE_FUNCTION = 'SC3080205' ")
            .AppendLine("    ,ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1 ")
            .AppendLine("WHERE  ")
            .AppendLine("    DLR_CD = :DLR_CD ")
            .AppendLine("    AND CST_ID = :CST_ID ")
            .AppendLine("    AND ROW_LOCK_VERSION = :ROW_LOCK_VERSION ")
        End With

        Using query As New DBUpdateQuery("SC3080205_220")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstId)
            query.AddParameterWithTypeValue("COMMERCIAL_RECV_TYPE", OracleDbType.NVarchar2, commercialRecvType)
            query.AddParameterWithTypeValue("UPDATE_FUNCTION_JUDGE", OracleDbType.NVarchar2, updatefunctionJudge)
            query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, updateAccount)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Decimal, rowLockVersion)

            ret = query.Execute()
        End Using

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomerDlr_End")
        'ログ出力 End *****************************************************************************
        Return ret
    End Function

    ''' <summary>
    ''' 商業情報受取区分変更履歴 新規作成
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="cstId">顧客ID</param>
    ''' <param name="commercialRecvType">商業情報受取区分</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <returns>登録行数</returns>
    ''' <remarks>販売店顧客の商業情報受取区分を変更した時に、履歴(変更時データ)を追加する。</remarks>
    Public Function InsertCommercialChgHis(ByVal dlrCd As String, _
                           ByVal cstId As Decimal, _
                           ByVal commercialRecvType As String, _
                           ByVal updateAccount As String) As Integer

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCstCommercialRecv_Start")
        'ログ出力 End *****************************************************************************

        Dim ret As Integer = -1
        Dim sql As New StringBuilder(10000)

        With sql
            .AppendLine("INSERT /* SC3080205_221 */ ")
            .AppendLine("INTO TB_M_COMMERCIAL_CHG_HIS ")
            .AppendLine("( ")
            .AppendLine("    DLR_CD ")
            .AppendLine("    ,CST_ID ")
            .AppendLine("    ,CST_COMMERCIAL_RECV_SEQ ")
            .AppendLine("    ,CHG_DATETIME ")
            .AppendLine("    ,CHG_STF_CD ")
            .AppendLine("    ,COMMERCIAL_RECV_TYPE ")
            .AppendLine("    ,ROW_CREATE_DATETIME ")
            .AppendLine("    ,ROW_CREATE_ACCOUNT ")
            .AppendLine("    ,ROW_CREATE_FUNCTION ")
            .AppendLine("    ,ROW_UPDATE_DATETIME ")
            .AppendLine("    ,ROW_UPDATE_ACCOUNT ")
            .AppendLine("    ,ROW_UPDATE_FUNCTION ")
            .AppendLine("    ,ROW_LOCK_VERSION ")
            .AppendLine(")  ")
            .AppendLine("SELECT ")
            .AppendLine("    :DLR_CD ")
            .AppendLine("    ,:CST_ID ")
            .AppendLine("    ,NVL(MAX(CST_COMMERCIAL_RECV_SEQ),0) + 1 ")
            .AppendLine("    ,SYSDATE ")
            .AppendLine("    ,:UPDATE_STF_CD ")
            .AppendLine("    ,:COMMERCIAL_RECV_TYPE ")
            .AppendLine("    ,SYSDATE ")
            .AppendLine("    ,:UPDATE_STF_CD ")
            .AppendLine("    ,'SC3080205' ")
            .AppendLine("    ,SYSDATE ")
            .AppendLine("    ,:UPDATE_STF_CD ")
            .AppendLine("    ,'SC3080205' ")
            .AppendLine("    ,0 ")
            .AppendLine("FROM TB_M_COMMERCIAL_CHG_HIS ")
            .AppendLine("WHERE  ")
            .AppendLine("    DLR_CD = :DLR_CD  ")
            .AppendLine("    AND CST_ID = :CST_ID ")
        End With

        Using query As New DBUpdateQuery("SC3080205_221")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, cstId)
            query.AddParameterWithTypeValue("COMMERCIAL_RECV_TYPE", OracleDbType.NVarchar2, commercialRecvType)
            query.AddParameterWithTypeValue("UPDATE_STF_CD", OracleDbType.NVarchar2, updateAccount)

            ret = query.Execute()
        End Using

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCstCommercialRecv_End")
        'ログ出力 End *****************************************************************************

        Return ret
    End Function
    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客個人情報(車両)新規作成
    ''' </summary>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="cstid">顧客ID </param>
    ''' <param name="actvctgryid">活動分類区分 </param>
    ''' <param name="reasonid">活動除外理由コード </param>
    ''' <param name="ac_modffuncdvs">活動分類区分変更機能 </param>
    ''' <param name="strcdstaff">セールス担当店舗コード </param>
    ''' <param name="staffcd">セールス担当スタッフコード </param>
    ''' <param name="updateaccount">作成アカウント</param>
    ''' <remarks></remarks>
    Public Shared Function InsertNewcustomer_vcl(ByVal dlrcd As String, _
                           ByVal cstid As String, _
                           ByVal actvctgryid As Nullable(Of Long), _
                           ByVal reasonid As Nullable(Of Long), _
                           ByVal ac_modffuncdvs As String, _
                           ByVal strcdstaff As String, _
                           ByVal staffcd As String, _
                           ByVal updateaccount As String) As Integer

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertNewcustomer_vcl_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT /* SC3080205_211 */ ")
            .Append("INTO ")
            .Append("    TB_M_CUSTOMER_VCL ")
            .Append("( ")
            .Append("    DLR_CD, ")
            .Append("    CST_ID, ")
            .Append("    VCL_ID, ")
            .Append("    CST_VCL_TYPE, ")
            .Append("    ACT_CAT_TYPE, ")
            .Append("    OMIT_REASON_CD, ")
            .Append("    ACT_CAT_UPDATE_FUNCTION, ")
            .Append("    SLS_PIC_BRN_CD, ")
            .Append("    SLS_PIC_STF_CD, ")
            .Append("    SVC_PIC_BRN_CD, ")
            .Append("    SVC_PIC_STF_CD, ")
            .Append("    INS_PIC_BRN_CD, ")
            .Append("    INS_PIC_STF_CD, ")
            .Append("    OWNER_CHG_FLG, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ")
            .Append("( ")
            .Append("    :DLRCD, ")
            .Append("    :CSTID, ")
            .Append("     0, ")
            .Append("    '1', ")
            .Append("    :ACTVCTGRYID, ")
            If reasonid Is Nothing Then
                .Append("    ' ', ")
            Else
                .Append("    :REASONID, ")
            End If
            .Append("    :AC_MODFFUNCDVS, ")
            .Append("    :STRCDSTAFF, ")
            .Append("    :STAFFCD, ")
            '2014/04/21 TCS 松月 【A STEP2】サービス／保険担当店舗コード設定対応（号口切替BTS-355） START
            .Append("    :STRCDSTAFF_S, ")
            '2014/04/21 TCS 松月 【A STEP2】サービス／保険担当店舗コード設定対応（号口切替BTS-355） END
            .Append("    ' ', ")
            '2014/04/21 TCS 松月 【A STEP2】サービス／保険担当店舗コード設定対応（号口切替BTS-355） START
            .Append("    :STRCDSTAFF_H, ")
            '2014/04/21 TCS 松月 【A STEP2】サービス／保険担当店舗コード設定対応（号口切替BTS-355） END
            .Append("    ' ', ")
            .Append("    '0', ")
            .Append("     SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080205', ")
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080205', ")
            .Append("    0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080205_211")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("ACTVCTGRYID", OracleDbType.NVarchar2, actvctgryid)
            If reasonid Is Nothing Then
            Else
                query.AddParameterWithTypeValue("REASONID", OracleDbType.NVarchar2, reasonid)
            End If
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            query.AddParameterWithTypeValue("AC_MODFFUNCDVS", OracleDbType.NVarchar2, " ")
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            query.AddParameterWithTypeValue("STRCDSTAFF", OracleDbType.NVarchar2, strcdstaff)
            query.AddParameterWithTypeValue("STRCDSTAFF_S", OracleDbType.NVarchar2, strcdstaff)
            query.AddParameterWithTypeValue("STRCDSTAFF_H", OracleDbType.NVarchar2, strcdstaff)
            query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, staffcd)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertNewcustomer_vcl_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致グループ所属顧客最新化情報取得
    ''' </summary>
    ''' <param name="cstid ">顧客ID </param>
    ''' <returns>SC3080205AttGroupDataTableDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetSqAttGroupCstTgt(ByVal cstid As Decimal) As SC3080205DataSet.SC3080205AttGroupDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205AttGroupDataTable)("SC3080205_212")

            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqAttGroupCstTgt_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080205_212 */ ")
                .Append("  DLR_CD AS DLRCD, ")
                .Append("  CST_ID AS CSTID ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER_VCL ")
                .Append("WHERE ")
                .Append("      CST_ID = :CSTID ")
                .Append("  AND CST_VCL_TYPE = '1' ")
                .Append("GROUP BY ")
                .Append("  DLR_CD, ")
                .Append("  CST_ID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqAttGroupCstTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致グループ所属顧客最新化シーケンス取得
    ''' </summary>
    ''' <returns>SC3080205OAttSeqDataTableDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetSqAttGroupCstTgt() As SC3080205DataSet.SC3080205AttSeqDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205AttSeqDataTable)("SC3080205_213")

            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqAttGroupCstTgt_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080205_213 */ ")
                .Append("  SQ_ATTGROUP_CST_NEW.NEXTVAL AS SEQ ")
                .Append("FROM ")
                .Append("  DUAL ")
            End With

            query.CommandText = sql.ToString()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqAttGroupCstTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致グループ所属顧客最新化
    ''' </summary>
    ''' <param name="seq_no">誘致グループ所属顧客最新化ID </param>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="cstid">顧客ID </param>
    ''' <param name="updateaccount">作成アカウント</param>
    ''' <remarks></remarks>
    Public Shared Function InsertAttGroupCstTgt(ByVal seq_no As Decimal, _
                           ByVal dlrcd As String, _
                           ByVal cstid As String, _
                           ByVal updateaccount As String) As Integer

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertAttGroupCstTgt_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT ")
            .Append("    /* SC3080205_214 */ ")
            .Append("INTO TB_T_ATTGROUP_CST_NEW_TGT ( ")
            .Append("    ATTGROUP_CST_NEW_ID, ")
            .Append("    DLR_CD, ")
            .Append("    CST_ID, ")
            .Append("    REG_DATETIME, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :SEQ_NO, ")
            .Append("    :DLRCD, ")
            .Append("    :CSTID, ")
            .Append("    SYSDATE, ")
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080205', ")
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080205', ")
            .Append("    0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080205_214")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SEQ_NO", OracleDbType.Decimal, seq_no)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertAttGroupCstTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致最新化用情報取得
    ''' </summary>
    ''' <param name="cstid ">顧客ID </param>
    ''' <returns>SC3080205PlanNewDataTableDataTable</returns>
    ''' <remarks></remarks>
    Public Function SelectPlanNewTgt(ByVal cstid As String) As SC3080205DataSet.SC3080205PlanNewDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205PlanNewDataTable)("SC3080205_215")

            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectPlanNewTgt_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080205_215 */ ")
                .Append("  DLR_CD AS DLRCD , ")
                .Append("  CST_ID AS CSTID , ")
                .Append("  VCL_ID AS VCLID ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER_VCL ")
                .Append("WHERE ")
                .Append("      CST_ID = :CSTID ")
                .Append("  AND CST_VCL_TYPE = '1' ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SelectPlanNewTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致最新化シーケンス取得
    ''' </summary>
    ''' <returns>SC3080205PlanSeqDataTableDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetSqPlanNewTgt() As SC3080205DataSet.SC3080205PlanSeqDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205PlanSeqDataTable)("SC3080205_216")

            Dim sql As New StringBuilder
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqPlanNewTgt_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080205_216 */ ")
                .Append("  SQ_ATT_NEW_TGT.NEXTVAL AS SEQ ")
                .Append("FROM ")
                .Append("  DUAL ")
            End With

            query.CommandText = sql.ToString()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetSqPlanNewTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致グループ所属顧客最新化
    ''' </summary>
    ''' <param name="seq_no">計画最新化対象ID </param>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="cstid">顧客ID </param>
    ''' <param name="vclid">車両ID </param>
    ''' <param name="updateaccount">作成アカウント</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertPlanNewTgt(ByVal seq_no As String, _
                           ByVal dlrcd As String, _
                           ByVal cstid As String, _
                           ByVal vclid As String, _
                           ByVal updateaccount As String) As Integer

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertPlanNewTgt_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT ")
            .Append("    /* SC3080205_217 */ ")
            .Append("INTO TB_T_ATT_NEW_TGT ( ")
            .Append("    PLAN_NEW_TGT_ID, ")
            .Append("    DLR_CD, ")
            .Append("    CST_ID, ")
            .Append("    VCL_ID, ")
            .Append("    REG_DATETIME, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :SEQ_NO, ")
            .Append("    :DLRCD, ")
            .Append("    :CSTID, ")
            .Append("    :VCLID, ")
            .Append("    SYSDATE, ")
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080205', ")
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080205', ")
            .Append("    0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080205_109")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("SEQ_NO", OracleDbType.Decimal, seq_no)
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("VCLID", OracleDbType.Decimal, vclid)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertPlanNewTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END 

    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 個人法人項目リスト取得
    ''' </summary>
    ''' <returns>SC3080205PrivateFleetItemDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetPrivateFleetItem() As SC3080205DataSet.SC3080205PrivateFleetItemDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205PrivateFleetItemDataTable)("SC3080205_218")

            Dim sql As New StringBuilder
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPrivateFleetItem_Start")

            With sql
                .Append("SELECT /* SC3080205_218 */ ")
                .Append("    T1.PRIVATE_FLEET_ITEM_CD AS PRIVATE_FLEET_ITEM_CD, ")
                .Append("    T1.FLEET_FLG AS FLEET_FLG, ")
                .Append("    T1.PRIVATE_FLEET_ITEM AS PRIVATE_FLEET_ITEM, ")
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
                .Append("    NVL(T2.WORD_VAL_ENG, ' ') AS PRIVATE_FLEET_ITEM_NAME, ")
                .Append("    NVL(T3.CST_JOIN_TYPE, ' ') AS CST_JOIN_TYPE, ")
                .Append("    NVL(T3.CST_ORGNZ_NAME_REFERENCE_TYPE, ' ') AS CST_ORGNZ_NAME_REFERENCE_TYPE, ")
                .Append("    NVL(T3.CST_ORGNZ_NAME_INPUT_TYPE, ' ') AS CST_ORGNZ_NAME_INPUT_TYPE, ")
                .Append("    NVL(T3.CST_ORGNZ_NAME_DISP_TYPE, ' ') AS CST_ORGNZ_NAME_DISP_TYPE ")
                .Append("FROM ")
                .Append("    TB_M_PRIVATE_FLEET_ITEM T1, ")
                .Append("    TB_M_WORD T2, ")
                .Append("    TB_LM_PRIVATE_FLEET_ITEM T3 ")
                .Append("WHERE ")
                .Append("    T1.INUSE_FLG = '1' AND ")
                .Append("    T1.PRIVATE_FLEET_ITEM = T2.WORD_CD(+) AND ")
                .Append("    T1.PRIVATE_FLEET_ITEM_CD = T3.PRIVATE_FLEET_ITEM_CD(+) ")
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END
                .Append("ORDER BY ")
                .Append("    T1.SORT_ORDER, T1.PRIVATE_FLEET_ITEM_CD ")
            End With

            query.CommandText = sql.ToString()

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPrivateFleetItem_End")

            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' 州リスト取得
    ''' </summary>
    ''' <returns>SC3080205StateDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetState() As SC3080205DataSet.SC3080205StateDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205StateDataTable)("SC3080205_219")

            Dim sql As New StringBuilder
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetState_Start")

            With sql
                .Append("SELECT /* SC3080205_219 */ ")
                .Append("    STATE_CD AS STATE_CD, ")
                .Append("    STATE_NAME AS STATE_NAME ")
                .Append("FROM ")
                .Append("    TB_M_STATE ")
                .Append("WHERE ")
                .Append("    INUSE_FLG = '1' ")
                .Append("ORDER BY ")
                '2014/08/01 TCS 市川 TMT切替BTS-113対応 START
                If IsStateListSortByName Then
                    .Append("    STATE_NAME ")
                Else
                    .Append("    STATE_CD ")
                End If
                '2014/08/01 TCS 市川 TMT切替BTS-113対応 END
            End With

            query.CommandText = sql.ToString()

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetState_End")

            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' 地域リスト取得
    ''' </summary>
    ''' <param name="state">州コード </param>
    ''' <returns>SC3080205DistrictDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetDistrict(ByVal state As String) As SC3080205DataSet.SC3080205DistrictDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205DistrictDataTable)("SC3080205_220")

            Dim sql As New StringBuilder
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetDistrict_Start")

            With sql
                .Append("SELECT /* SC3080205_220 */ ")
                .Append("      DISTRICT_CD AS DISTRICT_CD, ")
                .Append("      DISTRICT_NAME AS DISTRICT_NAME ")
                .Append("FROM ")
                .Append("      TB_M_DISTRICT ")
                .Append("WHERE ")
                .Append("      STATE_CD = :STATE_CD ")
                .Append("  AND INUSE_FLG = '1' ")
                .Append("ORDER BY ")
                '2014/08/01 TCS 市川 TMT切替BTS-113対応 START
                If IsStateListSortByName Then
                    .Append("      DISTRICT_NAME ")
                Else
                    .Append("      DISTRICT_CD ")
                End If
                '2014/08/01 TCS 市川 TMT切替BTS-113対応 END
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("STATE_CD", OracleDbType.NVarchar2, state)

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetDistrict_End")

            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' 市リスト取得
    ''' </summary>
    ''' <param name="state">州コード </param>
    ''' <param name="district">地域コード </param>
    ''' <returns>SC3080205CityDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetCity(ByVal state As String, ByVal district As String) As SC3080205DataSet.SC3080205CityDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205CityDataTable)("SC3080205_221")

            Dim sql As New StringBuilder
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCity_Start")

            With sql
                .Append("SELECT /* SC3080205_221 */ ")
                .Append("      CITY_CD AS CITY_CD, ")
                .Append("      CITY_NAME AS CITY_NAME ")
                .Append("FROM ")
                .Append("      TB_M_CITY ")
                .Append("WHERE ")
                .Append("      STATE_CD = :STATE_CD ")
                .Append("  AND DISTRICT_CD = :DISTRICT_CD ")
                .Append("  AND INUSE_FLG = '1' ")
                .Append("ORDER BY ")
                '2014/08/01 TCS 市川 TMT切替BTS-113対応 START
                If IsStateListSortByName Then
                    .Append("      CITY_NAME ")
                Else
                    .Append("      CITY_CD ")
                End If
                '2014/08/01 TCS 市川 TMT切替BTS-113対応 END
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("STATE_CD", OracleDbType.NVarchar2, state)
            query.AddParameterWithTypeValue("DISTRICT_CD", OracleDbType.NVarchar2, district)

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCity_End")

            Return query.GetData()

        End Using

    End Function

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 地区リスト取得
    ''' </summary>
    ''' <param name="state">州コード </param>
    ''' <param name="district">地域コード </param>
    ''' <param name="city">市コード </param>
    ''' <returns>SC3080205LocationDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetLocation(ByVal state As String, ByVal district As String, ByVal city As String) As SC3080205DataSet.SC3080205LocationDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205LocationDataTable)("SC3080205_222")

            Dim sql As New StringBuilder
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLocation_Start")

            With sql
                .Append("SELECT /* SC3080205_222 */ ")
                .Append("      LOCATION_CD AS LOCATION_CD, ")
                .Append("      LOCATION_NAME AS LOCATION_NAME, ")
                .Append("      ZIP_CD AS ZIP_CD ")
                .Append("FROM ")
                .Append("      TB_M_LOCATION ")
                .Append("WHERE ")
                .Append("      STATE_CD = :STATE_CD ")
                .Append("  AND DISTRICT_CD = :DISTRICT_CD ")
                .Append("  AND CITY_CD = :CITY_CD ")
                .Append("  AND INUSE_FLG = '1' ")
                .Append("ORDER BY ")
                '2014/08/01 TCS 市川 TMT切替BTS-113対応 START
                If IsStateListSortByName Then
                    .Append("      LOCATION_NAME ")
                Else
                    .Append("      LOCATION_CD ")
                End If
                '2014/08/01 TCS 市川 TMT切替BTS-113対応 END
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("STATE_CD", OracleDbType.NVarchar2, state)
            query.AddParameterWithTypeValue("DISTRICT_CD", OracleDbType.NVarchar2, district)
            query.AddParameterWithTypeValue("CITY_CD", OracleDbType.NVarchar2, city)

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetLocation_End")

            Return query.GetData()

        End Using

    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
    Public Function GetCustOrgnzLocal(ByVal custOrgnzNameHead As String, ByVal privateFleetItemCd As String) As SC3080205DataSet.SC3080205CustOrgnzLocalDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205CustOrgnzLocalDataTable)("SC3080205_201806_temp1")

            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustOrgnzLocal_Start")

            '2019/03/06 TS 都築 (FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究 START
            query.CommandText =
                <s>
                    SELECT /* SC3080205_201806_temp1 */
                        T1.CST_ORGNZ_CD AS CST_ORGNZ_CD,
                        T1.CST_ORGNZ_NAME AS CST_ORGNZ_NAME,
                        NVL(T2.CST_SUBCAT2_CD, ' ') AS CST_SUBCAT2_CD,
                        NVL(T2.CST_SUBCAT2_NAME, ' ') AS CST_SUBCAT2_NAME
                    FROM
                        TB_LM_CUSTOMER_ORGANIZATION T1
                    LEFT JOIN TB_LM_CUSTOMER_SUBCATEGORY2 T2
                        ON T1.PRIVATE_FLEET_ITEM_CD = T2.PRIVATE_FLEET_ITEM_CD AND T1.CST_ORGNZ_CD = T2.CST_ORGNZ_CD
                    WHERE
                        T1.INUSE_FLG = '1' AND
                        -- LOWER(T1.CST_ORGNZ_NAME) LIKE LOWER(:CST_ORGNZ_NAME) AND
                        T1.CST_ORGNZ_NAME_SEARCH LIKE UPPER(:CST_ORGNZ_NAME) AND
                        T1.PRIVATE_FLEET_ITEM_CD = :PRIVATE_FLEET_ITEM_CD
                    ORDER BY
                        T1.PRIVATE_FLEET_ITEM_CD, T1.SORT_ORDER
                </s>.Value
            '2019/03/06 TS 都築 (FS)営業スタッフ納期遵守オペレーション確立に向けた試験研究 END
            query.AddParameterWithTypeValue("CST_ORGNZ_NAME", OracleDbType.NVarchar2, custOrgnzNameHead & "%")
            query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM_CD", OracleDbType.NVarchar2, privateFleetItemCd)

            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustOrgnzLocal_End")

            Return query.GetData()

        End Using

    End Function

    Public Function GetCustOrgnzLocal(ByVal privateFleetItemCd As String) As SC3080205DataSet.SC3080205CustOrgnzLocalDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205CustOrgnzLocalDataTable)("SC3080205_201806_temp2")

            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustOrgnzLocal_Start")

            query.CommandText =
                <s>
                    SELECT /* SC3080205_201806_temp2 */
                        T1.CST_ORGNZ_CD AS CST_ORGNZ_CD,
                        T1.CST_ORGNZ_NAME AS CST_ORGNZ_NAME,
                        ' ' AS CST_SUBCAT2_CD,
                        ' ' AS CST_SUBCAT2_NAME
                    FROM
                        TB_LM_CUSTOMER_ORGANIZATION T1
                    RIGHT JOIN TB_M_PRIVATE_FLEET_ITEM T2
                        ON T1.PRIVATE_FLEET_ITEM_CD = T2.PRIVATE_FLEET_ITEM_CD
                    WHERE
                        T1.INUSE_FLG = '1' AND
                        T1.PRIVATE_FLEET_ITEM_CD = :PRIVATE_FLEET_ITEM_CD
                    ORDER BY
                        T1.SORT_ORDER, T1.PRIVATE_FLEET_ITEM_CD
                </s>.Value
            query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM_CD", OracleDbType.NVarchar2, privateFleetItemCd)

            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustOrgnzLocal_End")

            Return query.GetData()

        End Using

    End Function

    Public Function GetCustSubCtgry2(ByVal private_fleet_item_cd As String, ByVal cst_orgnz_cd As String) As SC3080205DataSet.SC3080205CustSubCtgry2DataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205CustSubCtgry2DataTable)("SC3080205_201806_temp3")

            Dim Sql As New StringBuilder
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustSubCtgry2_Start")

            With Sql
                .Append("SELECT /* SC3080205_201806_temp3 */ ")
                .Append("    T1.CST_SUBCAT2_CD AS CST_SUBCAT2_CD, ")
                .Append("    T1.CST_SUBCAT2_NAME AS CST_SUBCAT2_NAME, ")
                .Append("    T1.PRIVATE_FLEET_ITEM_CD AS PRIVATE_FLEET_ITEM_CD, ")
                .Append("    T1.CST_ORGNZ_CD AS CST_ORGNZ_CD ")
                .Append("FROM ")
                .Append("( ")
                .Append("  SELECT CST_SUBCAT2_CD,CST_SUBCAT2_NAME,PRIVATE_FLEET_ITEM_CD,CST_ORGNZ_CD,SORT_ORDER ")
                .Append("  ,ROW_NUMBER() OVER (PARTITION BY CST_SUBCAT2_NAME ORDER BY SORT_ORDER) RNUM ")
                .Append("  FROM TB_LM_CUSTOMER_SUBCATEGORY2 ")
                .Append("  WHERE INUSE_FLG = '1' ")
                .Append("  AND PRIVATE_FLEET_ITEM_CD = :PRIVATE_FLEET_ITEM_CD ")
                If Not String.IsNullOrWhiteSpace(cst_orgnz_cd) Then
                    .Append("  AND CST_ORGNZ_CD = :CST_ORGNZ_CD ")
                End If
                .Append(" ) T1 ")
                .Append(" WHERE T1.RNUM = 1 ")
                .Append("ORDER BY ")
                .Append("    T1.SORT_ORDER ")
            End With

            query.CommandText = Sql.ToString()
            query.AddParameterWithTypeValue("PRIVATE_FLEET_ITEM_CD", OracleDbType.NVarchar2, private_fleet_item_cd)
            If Not String.IsNullOrWhiteSpace(cst_orgnz_cd) Then
                query.AddParameterWithTypeValue("CST_ORGNZ_CD", OracleDbType.NVarchar2, cst_orgnz_cd)
            End If
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustSubCtgry2_End")

            Return query.GetData()

        End Using

    End Function

    Public Shared Function InsertCustomerLocal(
            ByVal custId As String,
            ByVal custOrgnzCd As String,
            ByVal custOrgnzInputType As String,
            ByVal custOrgnzName As String,
            ByVal custSubCtgry2Cd As String,
            ByVal updateAccount As String) As Integer

        Dim ret As Integer

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertNewcustomerLocal_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBUpdateQuery("SC3080205_201806_temp4")
            query.CommandText =
                <s>
                    INSERT /* SC3080205_201806_temp4 */
                    INTO TB_LM_CUSTOMER (
                        CST_ID,
                        CST_ORGNZ_CD,
                        CST_ORGNZ_INPUT_TYPE,
                        CST_ORGNZ_NAME,
                        CST_SUBCAT2_CD,
                        ROW_CREATE_DATETIME,
                        ROW_CREATE_ACCOUNT,
                        ROW_CREATE_FUNCTION,
                        ROW_UPDATE_DATETIME,
                        ROW_UPDATE_ACCOUNT,
                        ROW_UPDATE_FUNCTION,
                        ROW_LOCK_VERSION
                    ) VALUES (
                        :CST_ID,
                        :CST_ORGNZ_CD,
                        :CST_ORGNZ_INPUT_TYPE,
                        :CST_ORGNZ_NAME,
                        :CST_SUBCAT2_CD,
                        SYSDATE,
                        :UPDATE_ACCOUNT,
                        'SC3080205',
                        SYSDATE,
                        :UPDATE_ACCOUNT,
                        'SC3080205',
                        0
                    )
                </s>.Value
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, custId)
            query.AddParameterWithTypeValue("CST_ORGNZ_CD", OracleDbType.NVarchar2, custOrgnzCd)
            query.AddParameterWithTypeValue("CST_ORGNZ_INPUT_TYPE", OracleDbType.NVarchar2, custOrgnzInputType)
            query.AddParameterWithTypeValue("CST_ORGNZ_NAME", OracleDbType.NVarchar2, custOrgnzName)
            query.AddParameterWithTypeValue("CST_SUBCAT2_CD", OracleDbType.NVarchar2, custSubCtgry2Cd)
            query.AddParameterWithTypeValue("UPDATE_ACCOUNT", OracleDbType.NVarchar2, updateAccount)

            ret = query.Execute()
        End Using

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertNewcustomerLocal_End")
        'ログ出力 End *****************************************************************************
        Return ret
    End Function

    Public Function UpdateCustomerLocal(
            ByVal custId As String,
            ByVal custOrgnzCd As String,
            ByVal custOrgnzInputType As String,
            ByVal custOrgnzName As String,
            ByVal custSubCtgry2Cd As String,
            ByVal updateAccount As String,
            ByVal rowLockVersion As Decimal) As Integer

        Dim ret As Integer

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateCustomerLocal_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBUpdateQuery("SC3080205_201806_temp4")
            query.CommandText =
                <s>
                    UPDATE /* SC3080205_201806_temp5 */
                        TB_LM_CUSTOMER
                    SET
                        CST_ORGNZ_CD = :CST_ORGNZ_CD,
                        CST_ORGNZ_INPUT_TYPE = :CST_ORGNZ_INPUT_TYPE,
                        CST_ORGNZ_NAME = :CST_ORGNZ_NAME,
                        CST_SUBCAT2_CD = :CST_SUBCAT2_CD,
                        ROW_UPDATE_DATETIME = SYSDATE,
                        ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT,
                        ROW_UPDATE_FUNCTION = 'SC3080205',
                        ROW_LOCK_VERSION = :ROW_LOCK_VERSION + 1
                    WHERE
                        CST_ID = :CST_ID AND
                        ROW_LOCK_VERSION = :ROW_LOCK_VERSION
                </s>.Value
            query.AddParameterWithTypeValue("CST_ORGNZ_CD", OracleDbType.NVarchar2, custOrgnzCd)
            query.AddParameterWithTypeValue("CST_ORGNZ_INPUT_TYPE", OracleDbType.NVarchar2, custOrgnzInputType)
            query.AddParameterWithTypeValue("CST_ORGNZ_NAME", OracleDbType.NVarchar2, custOrgnzName)
            query.AddParameterWithTypeValue("CST_SUBCAT2_CD", OracleDbType.NVarchar2, custSubCtgry2Cd)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, updateAccount)
            query.AddParameterWithTypeValue("ROW_LOCK_VERSION", OracleDbType.Decimal, rowLockVersion)
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, custId)

            ret = query.Execute()
        End Using

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateCustomerLocal_End")
        'ログ出力 End *****************************************************************************
        Return ret
    End Function
    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END


    ''' <summary>
    ''' 入力項目設定リスト取得
    ''' </summary>
    ''' <param name="chktiming">チェックタイミング区分 </param>
    ''' <returns>SC3080205InputItemSettingDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetInputItemSetting(ByVal chktiming As String) As SC3080205DataSet.SC3080205InputItemSettingDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205InputItemSettingDataTable)("SC3080205_223")

            Dim sql As New StringBuilder
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInputItemSetting_Start")

            With sql
                .Append("SELECT /* SC3080205_223 */ ")
                .Append("      TGT_ITEM_ID AS TGT_ITEM_ID, ")
                .Append("      TGT_ITEM AS TGT_ITEM, ")
                .Append("      DISP_SETTING_STATUS AS DISP_SETTING_STATUS ")
                .Append("FROM ")
                .Append("      TBL_INPUT_ITEM_SETTING ")
                .Append("WHERE ")
                .Append("      CHECK_TIMING_TYPE = :CHECK_TIMING_TYPE ")
                .Append("ORDER BY ")
                .Append("      TGT_ITEM_ID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("CHECK_TIMING_TYPE", OracleDbType.NVarchar2, chktiming)

            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInputItemSetting_End")

            Return query.GetData()

        End Using

    End Function
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

    '2013/05/01 TCS 松月 新PF残課題No.21 Start
    ''' <summary>
    ''' 活動分類区分変更履歴新規作成
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstid">自社客連番</param>
    ''' <param name="actvctgryid">AC</param>
    ''' <param name="reasonid">活動除外理由ID</param>
    ''' <param name="updateaccount">更新アカウント</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Shared Function InsertCstVclActCat(ByVal dlrcd As String, _
                                      ByVal cstid As String, _
                                      ByVal actvctgryid As String,
                                      ByVal reasonid As String, _
                                      ByVal updateaccount As String,
                                      ByVal vclid As String) As Integer

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCstVclActCat_Start")
        'ログ出力 End *****************************************************************************

        With sql
            .Append("INSERT ")
            .Append("    /* SC3080205_231 */ ")
            .Append("INTO TB_M_CUSTOMER_VCL_ACT_CAT ( ")
            .Append("    DLR_CD, ")
            .Append("    CST_ID, ")
            .Append("    VCL_ID, ")
            .Append("    CST_VCL_TYPE, ")
            .Append("    CST_VCL_ACT_CAT_SEQ, ")
            .Append("    CHG_DATETIME, ")
            .Append("    CHG_STF_CD, ")
            .Append("    CHG_REASON, ")
            .Append("    ACT_CAT_TYPE, ")
            .Append("    OMIT_REASON_CD, ")
            .Append("    ROW_CREATE_DATETIME, ")
            .Append("    ROW_CREATE_ACCOUNT, ")
            .Append("    ROW_CREATE_FUNCTION, ")
            .Append("    ROW_UPDATE_DATETIME, ")
            .Append("    ROW_UPDATE_ACCOUNT, ")
            .Append("    ROW_UPDATE_FUNCTION, ")
            .Append("    ROW_LOCK_VERSION ")
            .Append(") ")
            .Append("VALUES ( ")
            .Append("    :DLRCD, ")
            .Append("    :CSTID, ")
            .Append("    :VCLID, ")
            .Append("    '1', ")
            .Append("    NVL( (SELECT MAX(CST_VCL_ACT_CAT_SEQ) FROM TB_M_CUSTOMER_VCL_ACT_CAT WHERE DLR_CD = :DLRCD AND CST_ID = :CSTID AND VCL_ID = :VCLID ),0) + 1, ")
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    ' ', ")
            If actvctgryid = "" Then
                .Append("    ' ', ")
            Else
                .Append("    :ACTCATID, ")
            End If
            If reasonid = "" Then
                .Append("    ' ', ")
            Else
                .Append("    :REASONID, ")
            End If
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080205', ")
            .Append("    SYSDATE, ")
            .Append("    :UPDATEACCOUNT, ")
            .Append("    'SC3080205', ")
            .Append("0 ")
            .Append(") ")
        End With

        Using query As New DBUpdateQuery("SC3080205_231")

            query.CommandText = sql.ToString()

            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("VCLID", OracleDbType.Decimal, vclid)
            If actvctgryid = "" Then
            Else
                query.AddParameterWithTypeValue("ACTCATID", OracleDbType.NVarchar2, actvctgryid)
            End If
            If reasonid = "" Then
            Else
                query.AddParameterWithTypeValue("REASONID", OracleDbType.NVarchar2, reasonid)
            End If

            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCstVclActCat_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/05/01 TCS 松月 新PF残課題No.21 End

    '2020/01/20 TS 岩田 TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045) START
    ''' <summary>
    ''' Aカード番号件数取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstid">顧客ID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>Aカード番号件数取得を取得します。</remarks>
    Public Shared Function GetAcardNumCount(ByVal dlrcd As String, ByVal cstid As String) As SC3080205DataSet.SC3080205AcardNumCountDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205AcardNumCountDataTable)("SC3080205_232")

            Dim sql As New StringBuilder
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetAcardNumCount_Start")

            With sql
                .Append("SELECT /* SC3080205_232 */")
                .Append("   SLST.ACARD_ROWS_COUNT")
                .Append("   ,SLSH.ACARD_HIS_ROWS_COUNT")
                .Append(" FROM ")
                .Append("   (SELECT")
                .Append("       COUNT(1) AS ACARD_ROWS_COUNT")
                .Append("    FROM TB_T_SALES")
                .Append("    WHERE DLR_CD = :DLRCD")
                .Append("    AND CST_ID  = :CSTID")
                .Append("    AND ACARD_NUM !=' '")
                .Append("    AND ROWNUM<=1")
                .Append("   ) SLST")
                .Append("   ,(SELECT")
                .Append("       COUNT(1) AS ACARD_HIS_ROWS_COUNT")
                .Append("    FROM TB_H_SALES")
                .Append("    WHERE DLR_CD = :DLRCD")
                .Append("    AND CST_ID  = :CSTID")
                .Append("    AND ACARD_NUM !=' '")
                .Append("    AND ROWNUM<=1")
                .Append("   ) SLSH")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)

            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetAcardNumCount_End")

            Return query.GetData()

        End Using

    End Function

    '2020/01/20 TS 岩田 TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045) END

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    '2013/06/30 TCS 宋 2013/10対応版　既存流用 START
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        '2013/06/30 TCS 宋 2013/10対応版　既存流用 END
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class


