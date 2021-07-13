'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080205DataTableTableAdapter.vb
'─────────────────────────────────────
'機能： 顧客編集 (データ)
'補足： 
'作成： 2011/11/07 TCS 安田
'更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2013/06/30 TCS 内藤 【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'─────────────────────────────────────
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
    Public Const IdDlrcd As Integer = 1         '販売店コード
    Public Const IdStrcd As Integer = 2         '店舗コード
    Public Const IdStrcdstaff As Integer = 3         'スタッフ店舗コード
    Public Const IdStaffcd As Integer = 4         'スタッフコード
    Public Const IdOriginalId As Integer = 5         '自社客連番
    Public Const IdLinlSyscd As Integer = 6         '基幹SYSTEM識別コード
    Public Const IdCustcd As Integer = 7         '顧客コード
    Public Const IdCustype As Integer = 8         '顧客タイプ
    Public Const IdSocialid As Integer = 9         '国民番号
    Public Const IdName As Integer = 10         '氏名
    Public Const IdSex As Integer = 11         '性別
    Public Const IdBirthday As Integer = 12         '生年月日
    Public Const IdNameTitle As Integer = 13         '敬称
    Public Const IdFirstName As Integer = 14         'ファーストネーム
    Public Const IdMiddleName As Integer = 15         'ミドルネーム
    Public Const IdLastName As Integer = 16         'ラストネーム
    Public Const IdFamiryNameKana As Integer = 17         'お客様カナ姓
    Public Const IdOnesnameKana As Integer = 18         'お客様カナ名
    Public Const IdCompanyName As Integer = 19         '会社名称
    Public Const IdAddress As Integer = 20         '住所
    Public Const IdAddress1 As Integer = 21         '住所1
    Public Const IdAddress2 As Integer = 22         '住所2
    Public Const IdAddress3 As Integer = 23         '住所3
    Public Const IdDomicile As Integer = 24         '本籍
    Public Const IdCountry As Integer = 25         '国籍
    Public Const IdZipcode As Integer = 26         '郵便番号
    Public Const Idtelno As Integer = 27         '電話番号
    Public Const IdFaxno As Integer = 28         'FAX番号
    Public Const IdMobile As Integer = 29         '携帯電話番号
    Public Const IdEmail1 As Integer = 30         'e-MAILアドレス1
    Public Const IdEmail2 As Integer = 31         'e-MAILアドレス2
    Public Const IdBusinessTelno As Integer = 32         '勤め先電話番号
    Public Const IdIncome As Integer = 33         '収入
    Public Const IdContactTime As Integer = 34         '連絡可能時間帯
    Public Const IdOccupation As Integer = 35         '職業
    Public Const IdFamily As Integer = 36         '家族構成
    Public Const IdNname As Integer = 37         'ニックネーム
    Public Const IdDefaulyLang As Integer = 38         'デフォルト言語
    '2013/06/30 TCS 内藤 2013/10対応版 既存流用 START DEL
    '2013/06/30 TCS 内藤 2013/10対応版 既存流用 END
    Public Const IdTelFlg As Integer = 42         '顧客ＴＥＬ配信フラグ
    Public Const IdPapcFlg As Integer = 43         'Pアプローチ実施フラグ
    Public Const IdNayosecd As Integer = 44         '名寄せ先お客様コード
    Public Const IdNayoseldvs As Integer = 45         '名寄せ済み区分
    Public Const IdNayosedate As Integer = 46         '名寄せ実施日
    Public Const IdLexusKubun As Integer = 47         'レクサス区分
    Public Const IdDelFlg As Integer = 48         '削除フラグ
    Public Const IdDelDate As Integer = 49         '削除日
    Public Const IdTakeinDate As Integer = 50         'データ連携日
    Public Const IdCreateDate As Integer = 51         '作成日
    Public Const IdUpdateDate As Integer = 52         '更新日
    Public Const IdUpdateAccount As Integer = 53         '更新ユーザアカウント
    Public Const IdEmployeeName As Integer = 54         '担当者名
    Public Const IdEmployeeDepartment As Integer = 55         '担当者所属部署
    Public Const IdEmployeePosition As Integer = 56         '担当者役職
    Public Const IdUpdateFuncFlg As Integer = 57         '最終更新機能
    Public Const IdNameTitlecd As Integer = 58         '敬称コード

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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustomer_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080205_201 */ ")
                .Append("  TO_CHAR(CST_ID) AS CUSTCD, ")
                .Append("  CST_SOCIALNUM AS SOCIALID, ")
                .Append("  CASE WHEN FLEET_FLG = '0' THEN '1' ")
                .Append("       WHEN FLEET_FLG = '1' THEN '0' ")
                .Append("  END AS CUSTYPE, ")
                .Append("  CST_NAME AS NAME, ")
                .Append("  NAMETITLE_CD AS NAMETITLE_CD, ")
                .Append("  NAMETITLE_NAME AS NAMETITLE, ")
                .Append("  CST_ZIPCD AS ZIPCODE, ")
                .Append("  CST_ADDRESS AS ADDRESS, ")
                .Append("  CST_PHONE AS TELNO, ")
                .Append("  CST_MOBILE AS MOBILE, ")
                .Append("  CST_FAX AS FAXNO, ")
                .Append("  CST_BIZ_PHONE AS BUSINESSTELNO, ")
                .Append("  CST_EMAIL_1 AS EMAIL1, ")
                .Append("  CST_EMAIL_2 AS EMAIL2, ")
                .Append("  CST_GENDER AS SEX, ")
                .Append("  CST_BIRTH_DATE AS BIRTHDAY, ")
                .Append("  FLEET_PIC_NAME AS EMPLOYEENAME, ")
                .Append("  FLEET_PIC_DEPT AS EMPLOYEEDEPARTMENT, ")
                .Append("  FLEET_PIC_POSITION AS EMPLOYEEPOSITION, ")
                .Append("  UPDATE_FUNCTION_JUDGE AS UPDATEFUNCFLG, ")
                .Append("  ROW_LOCK_VERSION AS LOCKVERSION ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER ")
                .Append("WHERE ")
                .Append("  CST_ID = :ORIGINALID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetCustomer_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetNewcustomer_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080205_202 */ ")
                .Append("  TO_CHAR(T1.CST_ID) AS CSTID, ")
                .Append("  T1.CST_SOCIALNUM AS SOCIALID, ")
                .Append("  CASE WHEN FLEET_FLG = '0' THEN '1' ")
                .Append("       WHEN FLEET_FLG = '1' THEN '0' ")
                .Append("  END AS CUSTYPE, ")
                .Append("  T1.CST_NAME AS NAME, ")
                .Append("  T1.NAMETITLE_CD AS NAMETITLE_CD, ")
                .Append("  T1.NAMETITLE_NAME AS NAMETITLE, ")
                .Append("  T1.CST_ZIPCD AS ZIPCODE, ")
                .Append("  T1.CST_ADDRESS AS ADDRESS, ")
                .Append("  T1.CST_PHONE AS TELNO, ")
                .Append("  T1.CST_MOBILE AS MOBILE, ")
                .Append("  T1.CST_FAX AS FAXNO, ")
                .Append("  T1.CST_BIZ_PHONE AS BUSINESSTELNO, ")
                .Append("  T1.CST_EMAIL_1 AS EMAIL1, ")
                .Append("  T1.CST_EMAIL_2 AS EMAIL2, ")
                .Append("  T1.CST_GENDER AS SEX, ")
                .Append("  T1.CST_BIRTH_DATE AS BIRTHDAY, ")
                .Append("  T1.FLEET_PIC_NAME AS EMPLOYEENAME, ")
                .Append("  T1.FLEET_PIC_DEPT AS EMPLOYEEDEPARTMENT, ")
                .Append("  T1.FLEET_PIC_POSITION AS EMPLOYEEPOSITION, ")
                .Append("  T2.ACT_CAT_TYPE AS ACTVCTGRYID, ")
                .Append("  T2.OMIT_REASON_CD AS REASONID, ")
                .Append("　T1.CST_REG_STATUS　AS DUMMYNAMEFLG, ")
                .Append("  T1.ROW_LOCK_VERSION AS LOCKVERSION, ")
                .Append("  T2.ROW_LOCK_VERSION AS VCLLOCKVERSION ")
                .Append("FROM ")
                .Append("  TB_M_CUSTOMER T1, ")
                .Append("  TB_M_CUSTOMER_VCL T2 ")
                .Append("WHERE ")
                .Append("      T1.CST_ID = :CSTID ")
                .Append("  AND T1.CST_ID = T2.CST_ID ")
                .Append("  AND T2.DLR_CD = :DLRCD ")
                .Append("  AND T2.VCL_ID = :VCLID ")
            End With

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dlrcd)
            query.AddParameterWithTypeValue("CSTID", OracleDbType.Decimal, cstid)
            query.AddParameterWithTypeValue("VCLID", OracleDbType.Decimal, vclid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetNewcustomer_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 宋 2013/10対応版　既存流用 END

            Return query.GetData()

        End Using

    End Function

    ''' <summary>
    ''' 郵便番号辞書検索
    ''' </summary>
    ''' <param name="zipcode ">郵便番号 </param>
    ''' <returns>SC3080205OrgCustomerDataTableDataTable</returns>
    ''' <remarks></remarks>
    Public Function GetAddress(ByVal zipcode As String) As SC3080205DataSet.SC3080205ZipDataTable

        Using query As New DBSelectQuery(Of SC3080205DataSet.SC3080205ZipDataTable)("SC3080205_003")

            Dim sql As New StringBuilder
            '2013/06/30 TCS 宋 2013/10対応版　既存流用 START
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetAddress_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT /* SC3080205_003 */ ")
                .Append("    T2.STATE_NAME || T3.DISTRICT_NAME || T4.CITY_NAME || T1.LOCATION_NAME AS ADDRESS ")
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

            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ZIPCODE", OracleDbType.NVarchar2, zipcode)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetAddress_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetNametitle_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080205_004 */ ")
                .Append("  NAMETITLE_CD AS NAMETITLE_CD, ")
                .Append("  NAMETITLE_NAME AS NAMETITLE, ")
                .Append("  CASE WHEN NAMETITLE_TYPE = '0' THEN '0' ")
                .Append("       WHEN NAMETITLE_TYPE = '1' THEN '2' ")
                .Append("       WHEN NAMETITLE_TYPE = '2' THEN '1' ")
                .Append("       WHEN NAMETITLE_TYPE = '3' THEN '1' ")
                .Append("  END AS DISPFLG ")
                .Append("FROM ")
                .Append("  TB_M_NAMETITLE ")
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

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetNametitle_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            query.CommandText = sql.ToString()

            Return query.GetData()

        End Using

    End Function

　　　'2013/06/30 TCS 宋 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 自社客個人情報更新
    ''' </summary>
    ''' <param name="originalid">自社客連番</param>
    ''' <param name="socialid">国民ID、免許証番号等</param>
    ''' <param name="custype">個人/法人区分</param>
    ''' <param name="name">顧客氏名</param>
    ''' <param name="nametitlecd">敬称コード</param>
    ''' <param name="nametitle">敬称</param>
    ''' <param name="zipcode">郵便番号</param>
    ''' <param name="address">住所</param>
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
    ''' <param name="updatefuncflg">顧客更新フラグ</param>
    ''' <param name="updateaccount">更新アカウント</param>
    ''' <param name="lockversion">ロックバージョン</param>
    ''' <returns>更新成功[True]/失敗[False]</returns>
    ''' <remarks></remarks>
    Public Function UpdateCustomer(ByVal originalid As String, _
                                    ByVal socialid As String, _
                                    ByVal custype As String, _
                                    ByVal name As String, _
                                    ByVal nametitlecd As String, _
                                    ByVal nametitle As String, _
                                    ByVal zipcode As String, _
                                    ByVal address As String, _
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
                                    ByVal updatefuncflg As String, _
                                    ByVal updateaccount As String, _
                                    ByVal lockversion As Long) As Integer
        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateCustomer_Start")
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
        With sql
            .Append("UPDATE ")
            .Append("    /* SC3080205_204 */ ")
            .Append("    TB_M_CUSTOMER ")
            .Append("SET ")
            If (IsEnabled(IdSocialid) = True) Then .Append("    CST_SOCIALNUM = :SOCIALID, ") '国民ID、免許証番号等
            If (IsEnabled(IdCustype) = True) Then .Append("    FLEET_FLG = :CUSTYPE, ") '個人/法人区分
            If (IsEnabled(IdName) = True) Then .Append("    CST_NAME = :NAME, ") '顧客氏名
            If (IsEnabled(IdNameTitlecd) = True) Then .Append("    NAMETITLE_CD = :NAMETITLE_CD, ") '敬称コード
            If (IsEnabled(IdNameTitle) = True) Then .Append("    NAMETITLE_NAME = :NAMETITLE, ") '敬称
            If (IsEnabled(IdZipcode) = True) Then .Append("    CST_ZIPCD = :ZIPCODE, ") '郵便番号
            If (IsEnabled(IdAddress) = True) Then .Append("    CST_ADDRESS = :ADDRESS, ") '住所
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

        Using query As New DBUpdateQuery("SC3080205_204")
            query.CommandText = sql.ToString()

            If (IsEnabled(IdSocialid) = True) Then query.AddParameterWithTypeValue("SOCIALID", OracleDbType.NVarchar2, socialid) '国民番号
            If (IsEnabled(IdCustype) = True) Then query.AddParameterWithTypeValue("CUSTYPE", OracleDbType.NVarchar2, custype) '顧客タイプ
            If (IsEnabled(IdName) = True) Then query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, name) '氏名
            If (IsEnabled(IdNameTitlecd) = True) Then query.AddParameterWithTypeValue("NAMETITLE_CD", OracleDbType.NVarchar2, nametitlecd) '敬称コード
            If (IsEnabled(IdNameTitle) = True) Then query.AddParameterWithTypeValue("NAMETITLE", OracleDbType.NVarchar2, nametitle) '敬称
            If (IsEnabled(IdZipcode) = True) Then query.AddParameterWithTypeValue("ZIPCODE", OracleDbType.NVarchar2, zipcode) '郵便番号
            If (IsEnabled(IdAddress) = True) Then query.AddParameterWithTypeValue("ADDRESS", OracleDbType.NVarchar2, address) '住所
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
            If Not (updatefuncflg = "") Then query.AddParameterWithTypeValue("UPDATEFUNCFLG", OracleDbType.NVarchar2, updatefuncflg) '最終更新機能
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount) '更新アカウント
            query.AddParameterWithTypeValue("ROWLOCKVERSION", OracleDbType.Int64, lockversion)

            query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Decimal, originalid)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateCustomer_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 宋 2013/10対応版　既存流用 END

            Return query.Execute()

        End Using

    End Function

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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateNewcustomer_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateNewcustomer_End")
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
    ''' <param name="nametitlecd">敬称コード</param>
    ''' <param name="nametitle">敬称名称</param>
    ''' <param name="sex">性別</param>
    ''' <param name="zipcode">郵便番号</param>
    ''' <param name="address">住所</param>
    ''' <param name="address1">住所1</param>
    ''' <param name="address2">住所2</param>
    ''' <param name="address3">住所3</param>
    ''' <param name="telno">電話番号</param>
    ''' <param name="mobile">携帯電話番号</param>
    ''' <param name="faxno">FAX番号</param>
    ''' <param name="businesstelno">勤め先電話番号</param>
    ''' <param name="email1">E-MAILアドレス1</param>
    ''' <param name="email2">E-MAILアドレス2</param>
    ''' <param name="birthday">生年月日</param>
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
                                        ByVal nametitlecd As String, _
                                        ByVal nametitle As String, _
                                        ByVal sex As String, _
                                        ByVal zipcode As String, _
                                        ByVal address As String, _
                                        ByVal address1 As String, _
                                        ByVal address2 As String, _
                                        ByVal address3 As String, _
                                        ByVal telno As String, _
                                        ByVal mobile As String, _
                                        ByVal faxno As String, _
                                        ByVal businesstelno As String, _
                                        ByVal email1 As String, _
                                        ByVal email2 As String, _
                                        ByVal birthday As Nullable(Of DateTime), _
                                        ByVal dummyNameFlg As String, _
                                        ByVal updateaccount As String) As Integer

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertNewcustomer_Start")
        'ログ出力 End *****************************************************************************

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
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    :SEX, ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    :ZIPCODE, ")
            .Append("    :ADDRESS, ")
            .Append("    :ADDRESS1, ")
            .Append("    :ADDRESS2, ")
            .Append("    :ADDRESS3, ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
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
            .Append("    ' ', ")
            .Append("    0, ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    '11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111', ")
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
            query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, telno)   '電話番号
            query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, mobile)   '携帯電話番号
            query.AddParameterWithTypeValue("FAXNO", OracleDbType.NVarchar2, faxno)   'FAX番号
            query.AddParameterWithTypeValue("BUSINESSTELNO", OracleDbType.NVarchar2, businesstelno)   '勤め先電話番号
            query.AddParameterWithTypeValue("EMAIL1", OracleDbType.NVarchar2, email1)   'E-MAILアドレス1
            query.AddParameterWithTypeValue("EMAIL2", OracleDbType.NVarchar2, email2)   'E-MAILアドレス2
            If Not (birthday Is Nothing) Then
                query.AddParameterWithTypeValue("BIRTHDAY", OracleDbType.Date, birthday)   '生年月日
            End If
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)
            query.AddParameterWithTypeValue("DUMMYFLG", OracleDbType.NVarchar2, dummyNameFlg)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertNewcustomer_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetNewcustseq_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetNewcustseq_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetGiveupReason_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT /* SC3080205_208 */ ")
                .Append("    OMIT_REASON_CD AS REASONID, ")
                .Append("    OMIT_REASON AS REASON ")
                .Append("FROM ")
                .Append("    TB_M_OMIT_REASON ")
                .Append("ORDER BY ")
                .Append("    OMIT_REASON_CD ")
            End With

            query.CommandText = sql.ToString()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetGiveupReason_End")
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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SelectCstLock_Start")
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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SelectCstLock_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

    End Sub


    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 未取引客個人情報(販売店)新規作成
    ''' </summary>
    ''' <param name="dlrcd">販売店コード </param>
    ''' <param name="cstid">顧客ID </param>
    ''' <param name="updateaccount">作成アカウント</param>
    ''' <remarks></remarks>
    Public Shared Function InsertNewcustome_dlr(ByVal dlrcd As String, _
                           ByVal cstid As String, _
                           ByVal updateaccount As String) As Integer

        Dim sql As New StringBuilder
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertNewcustome_dlr_Start")
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
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    0, ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    '11111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111111', ")
            .Append("    '2', ")
            .Append("    ' ', ")
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
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertNewcustome_dlr_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

            Return query.Execute()

        End Using

    End Function

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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertNewcustomer_vcl_Start")
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
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
            .Append("    ' ', ")
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
            query.AddParameterWithTypeValue("AC_MODFFUNCDVS", OracleDbType.NVarchar2, ac_modffuncdvs)
            query.AddParameterWithTypeValue("STRCDSTAFF", OracleDbType.NVarchar2, strcdstaff)
            query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, staffcd)
            query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, updateaccount)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertNewcustomer_vcl_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSqAttGroupCstTgt_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSqAttGroupCstTgt_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSqAttGroupCstTgt_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSqAttGroupCstTgt_End")
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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertAttGroupCstTgt_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertAttGroupCstTgt_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SelectPlanNewTgt_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("SelectPlanNewTgt_End")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSqPlanNewTgt_Start")
            'ログ出力 End *****************************************************************************

            With sql
                .Append("SELECT ")
                .Append("  /* SC3080205_216 */ ")
                .Append("  SQ_PLAN_NEW_TGT.NEXTVAL AS SEQ ")
                .Append("FROM ")
                .Append("  DUAL ")
            End With

            query.CommandText = sql.ToString()

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetSqPlanNewTgt_End")
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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertPlanNewTgt_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertPlanNewTgt_End")
            'ログ出力 End *****************************************************************************

            Return query.Execute()

        End Using

    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END 



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


