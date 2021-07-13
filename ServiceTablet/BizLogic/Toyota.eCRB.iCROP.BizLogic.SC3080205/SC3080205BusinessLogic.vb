'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080205BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客編集 (ビジネスロジック)
'補足： 
'作成： 2011/11/07 TCS 安田
'更新： 2012/01/26 TCS 安田 【SALES_1B】来店実績更新
'更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2013/06/30 TCS 趙　 【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
'2013/06/30 TCS 趙 2013/10対応版　既存流用 START
Imports Toyota.eCRB.SystemFrameworks.Web
'2013/06/30 TCS 趙 2013/10対応版　既存流用 END
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.iCROP.DataAccess.SC3080205



''' <summary>
''' SC3080205(Edit Customer)
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class SC3080205BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"
    ''' <summary>
    ''' 自社客/未取引客フラグ (1：自社客)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const OrgCustFlg As Integer = 1

    ''' <summary>
    ''' 自社客/未取引客フラグ (2：未取引客)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const NewCustFlg As Integer = 2

    ''' <summary>
    ''' 非表示フラグ (0：非表示)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Hihyouji As Short = 0

    ''' <summary>
    ''' 活動区分ID
    ''' </summary>
    ''' <remarks></remarks>
    Public Const InitActvctgryId As Integer = 1

    ''' <summary>
    ''' 機能パラメータ (SMS)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UsedFlgSms As String = "USED_FLG_SMS"

    ''' <summary>
    ''' 機能パラメータ (E-Mail)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UsedFlgEmail As String = "USED_FLG_E-MAIL"

    ''' <summary>
    ''' 活動区分変更機能 (1:Call（Call画面の Customer Detailより変更）)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ACModffuncdvsValue As String = "1"

    ''' <summary>
    ''' 機能パラメータ (D-Mail)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UsedFlgDmail As String = "USED_FLG_D-MAIL"

    ''' <summary>
    ''' 機能パラメータ (郵便番号辞書検索使用可否)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UsedFlgAddressSearch As String = "USED_FLG_ADDRESS_SEARCH"

    ''' <summary>
    ''' 機能パラメータ (自社客個人情報入力可能状態フラグ)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const IcropUpdateFlgOrgCustomer As String = "ICROP_UPDATE_FLG_ORGCUSTOMER"

    ''' <summary>
    ''' モード (０：新規登録モード)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ModeCreate As Integer = 0

    ''' <summary>
    ''' モード (１：編集モード)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ModeEdit As Integer = 1

    ''' <summary>
    ''' 機能設定マスタ設定値　(1:使用可)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UseFuncstatus As Integer = 1

    ''' <summary>
    ''' 最終更新機能フラグ (2（i-CROP側で更新）)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UpdCD As String = "2"

    ''' <summary>
    ''' 性別 (0:男性)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Otoko As String = "0"

    ''' <summary>
    ''' 性別 (1:女性)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Onna As String = "1"

    ''' <summary>
    ''' 顧客タイプ (0:法人)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Houjin As String = "0"

    ''' <summary>
    ''' 顧客タイプ (1:個人)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Kojin As String = "1"

    ''' <summary>
    ''' 0:希望しない
    ''' </summary>
    ''' <remarks></remarks>
    Public Const KibouNai As String = "0"

    ''' <summary>
    ''' 1:希望する
    ''' </summary>
    ''' <remarks></remarks>
    Public Const KibouSuru As String = "1"

    ''' <summary>
    ''' 敬称リスト　(表示フラグ)
    ''' 0:常に表示  1: 個人のみ表示  2: 法人のみ表示
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const IdNametitleAll As String = "0"
    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    Protected Const IdNametitleKojinOnna As String = "3"
    Protected Const IdNametitleKojinOtoko As String = "2"
    Protected Const IdNametitleHojin As String = "1"
    '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

    ''' <summary>
    ''' 文言マスタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ActvctgryidDisplay As Integer = 60000
    Public Const ActvctgryidDisplay1 As Integer = 1
    Public Const ActvctgryidDisplay2 As Integer = 2
    Public Const ActvctgryidDisplay3 As Integer = 3
    Public Const ActvctgryidDisplay4 As Integer = 4

    ''' <summary>
    ''' 未取引客IDに付加する文字列
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const NewCstid As String = "NCST"

    ''' <summary>
    ''' 未取引客IDの数値部分書式
    ''' </summary>
    ''' <remarks></remarks>
    Protected Const NewCstidNoFormat As String = "0000000000"

    ''' <summary>
    ''' 1:車両編集ボタン
    ''' </summary>
    Public Const NectVehicleBtn As String = "1"

    ''' <summary>
    ''' 0:登録ボタン
    ''' </summary>
    Public Const TourokuBtn As String = "0"

#End Region

#Region "Publicメソット"
    ''' <summary>
    ''' 初期表示用フラグ情報取得
    ''' </summary>
    ''' <param name="custDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>初期表示時の表示用フラグを取得する。</remarks>
    Public Shared Function GetInitializeFlg(ByVal custDataTbl As SC3080205DataSet.SC3080205CustDataTable, ByRef msgId As Integer) As SC3080205DataSet.SC3080205CustDataTable

        msgId = 0
        Dim custDataRow As SC3080205DataSet.SC3080205CustRow

        Dim settionFlg As Integer = 0
        Dim funcSetting As New FunctionSetting

        custDataRow = custDataTbl.Item(0)

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        '郵便番号辞書検索使用可否
        settionFlg = funcSetting.GetiCROPFunctionSetting(custDataRow.DLRCD, UsedFlgAddressSearch)
        custDataRow.POSTSRHFLG = CType(settionFlg, Short)

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START 
        If (custDataRow.CUSTFLG = OrgCustFlg) Then
            Dim sysEnv As New SystemEnvSetting
            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            sysEnvRow = sysEnv.GetSystemEnvSetting(IcropUpdateFlgOrgCustomer)
            custDataRow.ORGINPUTFLG = sysEnvRow.PARAMVALUE
        End If
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        Return custDataTbl

    End Function

    ''' <summary>
    ''' バリデーション判定
    ''' </summary>
    ''' <param name="custDataTbl">データセット (インプット)</param>
    ''' <param name="enabledTable">可視/非可視リスト</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <param name="mode">モード (0：新規登録モード、1：編集モード)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>バリデーションを判定する。</remarks>
    Public Shared Function CheckValidation(ByVal custDataTbl As SC3080205DataSet.SC3080205CustDataTable, ByVal enabledTable As Dictionary(Of Integer, Boolean), _
                                           ByRef msgId As Integer, ByVal mode As Integer) As Boolean

        msgId = 0
        Dim custDataRow As SC3080205DataSet.SC3080205CustRow

        custDataRow = custDataTbl.Item(0)

        '氏名が未入力の場合	氏名を入力してください。
        If (enabledTable.Item(SC3080205TableAdapter.IdName) = True) Then
            '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
            '新規登録時は必須チェックをしない
            If mode = ModeEdit Then
                If (String.IsNullOrEmpty(custDataRow.NAME)) Then
                    msgId = 40902
                    Return False
                End If
            End If
            '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

            '氏名が256文字より多い	氏名を256文字以内で入力してください。
            If (Validation.IsCorrectDigit(custDataRow.NAME, 256) = False) Then
                msgId = 40902
                Return False
            End If

            '氏名に絵文字が入っている
            If (Validation.IsValidString(custDataRow.NAME) = False) Then
                msgId = 40924
                Return False
            End If
        End If

        If (Not String.IsNullOrEmpty(custDataRow.CUSTYPE)) Then
            If (custDataRow.CUSTYPE.Equals(Houjin) = True) Then
                '1:法人
                If (Not String.IsNullOrEmpty(custDataRow.EMPLOYEENAME)) Then
                    If (enabledTable.Item(SC3080205TableAdapter.IdEmployeeName) = True) Then
                        '担当者氏名(法人)が256文字より多い	担当者氏名(法人)を256文字以内で入力してください。
                        If (Validation.IsCorrectDigit(custDataRow.EMPLOYEENAME, 256) = False) Then
                            msgId = 40903
                            Return False
                        End If

                        '法人氏名に絵文字が入っている
                        If (Validation.IsValidString(custDataRow.EMPLOYEENAME) = False) Then
                            msgId = 40925
                            Return False
                        End If
                    End If
                End If
                If (Not String.IsNullOrEmpty(custDataRow.EMPLOYEEDEPARTMENT)) Then
                    If (enabledTable.Item(SC3080205TableAdapter.IdEmployeeDepartment) = True) Then
                        '担当者部署名(法人)が64文字より多い	担当者部署名(法人) を64文字以内で入力してください。
                        If (Validation.IsCorrectDigit(custDataRow.EMPLOYEEDEPARTMENT, 64) = False) Then
                            msgId = 40904
                            Return False
                        End If

                        '担当者部署名(法人)に絵文字が入っている
                        If (Validation.IsValidString(custDataRow.EMPLOYEEDEPARTMENT) = False) Then
                            msgId = 40926
                            Return False
                        End If
                    End If

                End If
                If (Not String.IsNullOrEmpty(custDataRow.EMPLOYEEPOSITION)) Then
                    If (enabledTable.Item(SC3080205TableAdapter.IdEmployeePosition) = True) Then
                        '役職(法人)が64文字より多い	役職(法人) を64文字以内で入力してください。
                        If (Validation.IsCorrectDigit(custDataRow.EMPLOYEEPOSITION, 64) = False) Then
                            msgId = 40905
                            Return False
                        End If

                        '役職(法人)に絵文字が入っている
                        If (Validation.IsValidString(custDataRow.EMPLOYEEPOSITION) = False) Then
                            msgId = 40927
                            Return False
                        End If
                    End If
                End If
            End If
        End If

        If ((enabledTable.Item(SC3080205TableAdapter.Idtelno) = True) Or _
            (enabledTable.Item(SC3080205TableAdapter.IdMobile) = True)) Then
            '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
            '新規登録時は必須チェックをしない
            If mode = ModeEdit Then
                '携帯電話番号か自宅電話番号、どちらかを入力してください。
                If (String.IsNullOrEmpty(custDataRow.TELNO) AndAlso String.IsNullOrEmpty(custDataRow.MOBILE)) Then
                    msgId = 40907
                    Return False
                End If
            End If
            '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END
        End If
        If (enabledTable.Item(SC3080205TableAdapter.IdMobile) = True) Then
            If (Not String.IsNullOrEmpty(custDataRow.MOBILE)) Then
                '携帯電話番号を電話番号形式で入力してください。
                If (Validation.IsMobilePhoneNumber(custDataRow.MOBILE) = False) Then
                    msgId = 40908
                    Return False
                End If
                '携帯電話番号を128文字以内で入力してください。
                If (Validation.IsCorrectDigit(custDataRow.MOBILE, 128) = False) Then
                    msgId = 40906
                    Return False
                End If
            End If
        End If
        If (enabledTable.Item(SC3080205TableAdapter.Idtelno) = True) Then
            If (Not String.IsNullOrEmpty(custDataRow.TELNO)) Then
                '自宅電話番号が数字またはハイフン（-）　以外 	自宅電話番号を64文字以内、電話番号形式で入力してください。
                If (Validation.IsPhoneNumber(custDataRow.TELNO) = False) Then
                    msgId = 40910
                    Return False
                End If
                '自宅電話番号が64文字より多い	自宅電話番号を64文字以内、電話番号形式で入力してください。
                If (Validation.IsCorrectDigit(custDataRow.TELNO, 64) = False) Then
                    msgId = 40909
                    Return False
                End If
            End If
        End If
        If (enabledTable.Item(SC3080205TableAdapter.IdBusinessTelno) = True) Then
            '勤務先電話番号が数字またはハイフン（-）　以外	勤務先電話番号を64文字以内、電話番号形式で入力してください。
            If (Not String.IsNullOrEmpty(custDataRow.BUSINESSTELNO)) Then
                If (Validation.IsPhoneNumber(custDataRow.BUSINESSTELNO) = False) Then
                    msgId = 40912
                    Return False
                End If
                '勤務先電話番号が64文字より多い	勤務先電話番号を64文字以内、電話番号形式で入力してください。
                If (Validation.IsCorrectDigit(custDataRow.BUSINESSTELNO, 64) = False) Then
                    msgId = 40911
                    Return False
                End If
            End If
        End If
        If (enabledTable.Item(SC3080205TableAdapter.IdFaxno) = True) Then
            If (Not String.IsNullOrEmpty(custDataRow.FAXNO)) Then
                '自宅FAX番号が数字またはハイフン（-）　以外	自宅FAX番号を64文字以内、電話番号形式で入力してください。
                If (Validation.IsPhoneNumber(custDataRow.FAXNO) = False) Then
                    msgId = 40914
                    Return False
                End If
                '自宅FAX番号が64文字より多い	自宅FAX番号を64文字以内、電話番号形式で入力してください。
                If (Validation.IsCorrectDigit(custDataRow.FAXNO, 64) = False) Then
                    msgId = 40913
                    Return False
                End If
            End If
        End If
        If (enabledTable.Item(SC3080205TableAdapter.IdZipcode) = True) Then
            If (Not String.IsNullOrEmpty(custDataRow.ZIPCODE)) Then
                '郵便番号が半角文字以外	郵便番号を32文字以内、半角文字で入力してください。
                If (Validation.IsPostalCode(custDataRow.ZIPCODE) = False) Then
                    msgId = 40916
                    Return False
                End If
                '郵便番号が32文字より多い	郵便番号を32文字以内、半角文字で入力してください。
                If (Validation.IsCorrectDigit(custDataRow.ZIPCODE, 32) = False) Then
                    msgId = 40915
                    Return False
                End If
            End If
        End If
        If (enabledTable.Item(SC3080205TableAdapter.IdAddress) = True) Then
            If (Not String.IsNullOrEmpty(custDataRow.ADDRESS)) Then
                '住所が320文字より多い	住所を320文字以内で入力してください。
                If (Validation.IsCorrectDigit(custDataRow.ADDRESS, 320) = False) Then
                    msgId = 40931
                    Return False
                End If
                '住所に絵文字が入っている
                If (Validation.IsValidString(custDataRow.ADDRESS) = False) Then
                    msgId = 40932
                    Return False
                End If
            End If
        End If
        If (enabledTable.Item(SC3080205TableAdapter.IdEmail1) = True) Then
            If (Not String.IsNullOrEmpty(custDataRow.EMAIL1)) Then
                'E-Mail1をメールアドレス形式で入力してください。
                If (Validation.IsMail(custDataRow.EMAIL1) = False) Then
                    msgId = 40919
                    Return False
                End If
                'E-Mail1が128文字より多い	E-Mail1を128文字以内、半角文字で入力してください。
                If (Validation.IsCorrectDigit(custDataRow.EMAIL1, 128) = False) Then
                    msgId = 40918
                    Return False
                End If

                'E-Mail1に絵文字が入っている
                If (Validation.IsValidString(custDataRow.EMAIL1) = False) Then
                    msgId = 40928
                    Return False
                End If
            End If
        End If
        If (enabledTable.Item(SC3080205TableAdapter.IdEmail2) = True) Then
            If (Not String.IsNullOrEmpty(custDataRow.EMAIL2)) Then
                'E-Mail2をメールアドレス形式で入力してください。
                If (Validation.IsMail(custDataRow.EMAIL2) = False) Then
                    msgId = 40921
                    Return False
                End If
                'E-Mail2が128文字より多い	E-Mail2を128文字以内、半角文字で入力してください。
                If (Validation.IsCorrectDigit(custDataRow.EMAIL2, 128) = False) Then
                    msgId = 40920
                    Return False
                End If

                'E-Mail2に絵文字が入っている
                If (Validation.IsValidString(custDataRow.EMAIL2) = False) Then
                    msgId = 40929
                    Return False
                End If
            End If
        End If
        If (enabledTable.Item(SC3080205TableAdapter.IdSocialid) = True) Then
            If (Not String.IsNullOrEmpty(custDataRow.SOCIALID)) Then
                '国民IDが32文字より多い	国民IDを32文字以内で入力してください。
                If (Validation.IsCorrectDigit(custDataRow.SOCIALID, 32) = False) Then
                    msgId = 40923
                    Return False
                End If

                '国民IDに絵文字が入っている
                If (Validation.IsValidString(custDataRow.SOCIALID) = False) Then
                    msgId = 40930
                    Return False
                End If
            End If
        End If

        Return True

    End Function

    ''' <summary>
    ''' 初期表示情報取得
    ''' </summary>
    ''' <param name="custDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>顧客情報を取得する。</remarks>
    Public Shared Function GetInitialize(ByVal custDataTbl As SC3080205DataSet.SC3080205CustDataTable, ByRef msgId As Integer) As SC3080205DataSet.SC3080205CustDataTable

        msgId = 0
        Dim retCustDataTbl As SC3080205DataSet.SC3080205CustDataTable
        Dim retCustDataRow As SC3080205DataSet.SC3080205CustRow
        Dim custDataRow As SC3080205DataSet.SC3080205CustRow

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetInitialize_Start")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        custDataRow = custDataTbl.Item(0)

        Using da As New SC3080205TableAdapter

            If (custDataRow.CUSTFLG = OrgCustFlg) Then
                '０：自社客
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetInitialize custDataRow.DLRCD = " + custDataRow.DLRCD)
                Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetInitialize custDataRow.ORIGINALID = " + custDataRow.ORIGINALID)
                'ログ出力 End *****************************************************************************
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                retCustDataTbl = da.GetCustomer(custDataRow.ORIGINALID)
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
            Else
                '１：未取引客
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetInitialize custDataRow.DLRCD = " + custDataRow.DLRCD)
                Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetInitialize custDataRow.CSTID = " + custDataRow.CSTID)
                'ログ出力 End *****************************************************************************

                '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START
                'retCustDataTbl = da.GetNewcustomer(custDataRow.DLRCD, custDataRow.CSTID, custDataRow.VCLID)
                retCustDataTbl = da.GetCustomer(custDataRow.CSTID)
                '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
            End If

        End Using

        '取得できなかった場合の処理 (例外処理とする)
        'If (retCustDataTbl.Rows.Count = 0) Then
        '    Return custDataTbl
        'End If

        retCustDataRow = retCustDataTbl.Item(0)
        ''顧客ID/未取引客ID
        'If (custDataRow.CUSTFLG = OrgCustFlg) Then
        '    '０：自社客
        '    custDataRow.CUSTCD = retCustDataRow.CUSTCD
        'Else
        '    '１：未取引客
        '    custDataRow.CUSTCD = retCustDataRow.CSTID
        'End If

        '国民ID、免許証番号等
        custDataRow.SOCIALID = DBValueToTrim(retCustDataRow.SOCIALID)

        '個人/法人区分
        custDataRow.CUSTYPE = retCustDataRow.CUSTYPE        '顧客タイプ

        '顧客氏名
        custDataRow.NAME = DBValueToTrim(retCustDataRow.NAME)

        '敬称コード
        custDataRow.NAMETITLE_CD = DBValueToTrim(retCustDataRow.NAMETITLE_CD)

        '敬称
        custDataRow.NAMETITLE = DBValueToTrim(retCustDataRow.NAMETITLE)

        '郵便番号
        custDataRow.ZIPCODE = DBValueToTrim(retCustDataRow.ZIPCODE)

        '住所
        custDataRow.ADDRESS = DBValueToTrim(retCustDataRow.ADDRESS)

        '自宅電話番号
        custDataRow.TELNO = DBValueToTrim(retCustDataRow.TELNO)

        '携帯電話番号
        custDataRow.MOBILE = DBValueToTrim(retCustDataRow.MOBILE)

        'FAX番号
        custDataRow.FAXNO = DBValueToTrim(retCustDataRow.FAXNO)

        '勤務地電話番号
        custDataRow.BUSINESSTELNO = DBValueToTrim(retCustDataRow.BUSINESSTELNO)

        'E-mailアドレス１
        custDataRow.EMAIL1 = DBValueToTrim(retCustDataRow.EMAIL1)

        'E-mailアドレス２
        custDataRow.EMAIL2 = DBValueToTrim(retCustDataRow.EMAIL2)

        '生年月日
        If (Not retCustDataRow.IsBIRTHDAYNull()) Then
            custDataRow.BIRTHDAY = retCustDataRow.BIRTHDAY
        End If

        '性別
        custDataRow.SEX = DBValueToTrim(retCustDataRow.SEX)


        If (custDataRow.CUSTFLG = NewCustFlg) Then
            '活動区分
            If (Not retCustDataRow.IsREASONIDNull()) Then
                custDataRow.REASONID = retCustDataRow.REASONID
            Else
                custDataRow.SetREASONIDNull()
            End If

            'AC
            If (Not retCustDataRow.IsACTVCTGRYIDNull()) Then
                custDataRow.ACTVCTGRYID = retCustDataRow.ACTVCTGRYID
            Else
                custDataRow.SetACTVCTGRYIDNull()
            End If
        End If

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

        '担当者氏名（法人）
        custDataRow.EMPLOYEENAME = DBValueToTrim(retCustDataRow.EMPLOYEENAME)

        '担当者部署名（法人）
        custDataRow.EMPLOYEEDEPARTMENT = DBValueToTrim(retCustDataRow.EMPLOYEEDEPARTMENT)

        '役職（法人）
        custDataRow.EMPLOYEEPOSITION = DBValueToTrim(retCustDataRow.EMPLOYEEPOSITION)

        '顧客更新フラグ
        custDataRow.UPDATEFUNCFLG = retCustDataRow.UPDATEFUNCFLG

        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
        custDataRow.DUMMYNAMEFLG = retCustDataRow.DUMMYNAMEFLG
        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        custDataRow.LOCKVERSION = retCustDataRow.LOCKVERSION
        '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない START
        'If (custDataRow.CUSTFLG <> OrgCustFlg) Then
        '    custDataRow.VCLLOCKVERSION = retCustDataRow.VCLLOCKVERSION
        'End If
        '2016/06/30 NSK 皆川 TR-SVT-TMT-20150922-001 画面で画像プロフィールが表示しない END
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetInitialize_End")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

        Return custDataTbl

    End Function

    ''' <summary>
    ''' 敬称リスト取得
    ''' </summary>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>敬称リストを取得する。</remarks>
    Public Shared Function GetNameTitleList(ByVal custDataTbl As SC3080205DataSet.SC3080205CustDataTable, ByRef msgId As Integer) As SC3080205DataSet.SC3080205NameTitleDataTable

        msgId = 0
        Dim dispflglist As New List(Of String)
        Dim custDataRow As SC3080205DataSet.SC3080205CustRow
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetNameTitleList_Start")
        'ログ出力 End *****************************************************************************
        custDataRow = custDataTbl.Item(0)

        If (String.IsNullOrEmpty(custDataRow.CUSTYPE.Trim())) Then

            'すべて
            dispflglist.Add(IdNametitleAll)
            dispflglist.Add(IdNametitleKojinOtoko)
            dispflglist.Add(IdNametitleKojinOnna)
            dispflglist.Add(IdNametitleHojin)

        Else

            '0:個人
            If (custDataRow.CUSTYPE.Equals(Kojin)) Then
                dispflglist.Add(IdNametitleKojinOtoko)
                dispflglist.Add(IdNametitleKojinOnna)
            End If

            '1:法人
            If (custDataRow.CUSTYPE.Equals(Houjin)) Then
                dispflglist.Add(IdNametitleHojin)
            End If

        End If

        Using da As New SC3080205TableAdapter
            '検索処理
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("GetNameTitleList_End")
            'ログ出力 End *****************************************************************************
            Return da.GetNametitle(dispflglist)
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        End Using

    End Function

    ''' <summary>
    ''' 顧客更新
    ''' </summary>
    ''' <param name="custDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客情報を更新する。</remarks>
    <EnableCommit()>
    Public Function UpdateCustomer(ByVal custDataTbl As SC3080205DataSet.SC3080205CustDataTable, ByVal enabledTable As Dictionary(Of Integer, Boolean), ByRef msgId As Integer) As Integer

        msgId = 0
        Dim ret As Integer = 1
        Dim custDataRow As SC3080205DataSet.SC3080205CustRow
        custDataRow = custDataTbl.Item(0)
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        Dim cstId As String

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateCustomer_Start")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

        'ブランクを半角一文字スペースにする
        Call EditDataRow(custDataRow)

        Using da As New SC3080205TableAdapter

            '画面の可視/非可視状態のセット
            da.SetEnabledTable(enabledTable)
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START DEL
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            Dim birthday As Nullable(Of DateTime)
            Dim smsflg As String = Nothing
            Dim actvctgryid As Nullable(Of Long)
            Dim resonid As String
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
            If (Not custDataRow.IsBIRTHDAYNull) Then
                birthday = custDataRow.BIRTHDAY
            End If
            If (Not custDataRow.IsSMSFLGNull) Then
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                smsflg = CType(custDataRow.SMSFLG, String)
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
            End If
            If (Not custDataRow.IsACTVCTGRYIDNull) Then
                actvctgryid = custDataRow.ACTVCTGRYID
            End If
            If (Not custDataRow.IsREASONIDNull) Then
                resonid = custDataRow.REASONID
            End If

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateCustomer Step1")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START DEL
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            '顧客テーブルロック処理
            If (custDataRow.CUSTFLG <> OrgCustFlg) Then
                cstId = custDataRow.CSTID
            Else
                cstId = custDataRow.ORIGINALID
            End If
            Try
                SC3080205TableAdapter.SelectCstLock(custDataRow.CUSTCD)
            Catch ex As Exception
                Return 0
            End Try
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateCustomer custDataRow.ORIGINALID = " + custDataRow.ORIGINALID)
            'ログ出力 End *****************************************************************************
            '顧客情報更新
            ret = da.UpdateCustomer(cstId, _
                                    custDataRow.SOCIALID, _
                                    custDataRow.CUSTYPE, _
                                    custDataRow.NAME, _
                                    custDataRow.NAMETITLE_CD, _
                                    custDataRow.NAMETITLE, _
                                    custDataRow.ZIPCODE, _
                                    custDataRow.ADDRESS, _
                                    custDataRow.TELNO, _
                                    custDataRow.MOBILE, _
                                    custDataRow.FAXNO, _
                                    custDataRow.BUSINESSTELNO, _
                                    custDataRow.EMAIL1, _
                                    custDataRow.EMAIL2, _
                                    custDataRow.SEX, _
                                    birthday, _
                                    custDataRow.EMPLOYEENAME, _
                                    custDataRow.EMPLOYEEDEPARTMENT, _
                                    custDataRow.EMPLOYEEPOSITION, _
                                    custDataRow.UPDATEFUNCFLG, _
                                    custDataRow.UPDATEACCOUNT,
                                    custDataRow.LOCKVERSION)
            If ret = 0 Then
                Me.Rollback = True
                Return -1
            End If
            '未取引客個人情報更新処理
            If (custDataRow.CUSTFLG <> OrgCustFlg) Then
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateCustomer custDataRow.CSTID = " + custDataRow.CSTID)
                'ログ出力 End *****************************************************************************              
                ret = da.UpdateNewcustomer(custDataRow.DLRCD, _
                                           custDataRow.CSTID, _
                                           CType(custDataRow.ACTVCTGRYID, String), _
                                           custDataRow.AC_MODFFUNCDVS, _
                                           custDataRow.REASONID, _
                                           custDataRow.UPDATEACCOUNT, _
                                           custDataRow.VCLLOCKVERSION, _
                                           custDataRow.VCLID)
            End If
            If ret = 0 Then
                Me.Rollback = True
                Return -1
            End If
            '誘致最新化メソッド実行
            ret = InsertAttPlanNew(cstId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateCustomer Step2")
            'ログ出力 End *****************************************************************************
            '更新に失敗していたらロールバック
            If ret = 0 Then
                Me.Rollback = True
                Return 0
            End If

        End Using

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("UpdateCustomer_End")
        'ログ出力 End *****************************************************************************
        Return ret
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

    End Function

    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致最新化
    ''' </summary>
    ''' <param name="CUSTCD">データセット (インプット)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function InsertAttPlanNew(ByVal CUSTCD As String) As Integer
        Dim context As StaffContext = StaffContext.Current
        Dim account As String = context.Account
        Dim subcustTbl As SC3080205DataSet.SC3080205AttGroupDataTable
        Dim i As Integer
        Dim n As Integer
        Dim SEQ_NO As Decimal
        Dim ret As Integer

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertAttPlanNew_Start")
        'ログ出力 End *****************************************************************************

        Using da As New SC3080205TableAdapter
            '誘致グループ所属顧客最新化用情報取得
            subcustTbl = da.GetSqAttGroupCstTgt(CDec(CUSTCD)) '入力顧客ID 

            If (subcustTbl.Rows.Count > 0) Then
                For i = 0 To subcustTbl.Rows.Count - 1
                    '誘致グループ所属顧客最新化シーケンス取得
                    SEQ_NO = da.GetSqAttGroupCstTgt().Item(0).Seq 'シーケンス
                    '誘致グループ所属顧客最新化
                    ret = SC3080205TableAdapter.InsertAttGroupCstTgt(SEQ_NO, subcustTbl.Item(i).DLRCD, subcustTbl.Item(i).CSTID, account)
                Next
            End If
            '誘致最新化用情報取得
            Dim subcustTbl2 As SC3080205DataSet.SC3080205PlanNewDataTable
            subcustTbl2 = da.SelectPlanNewTgt(CUSTCD) '入力顧客ID，返回：  販売店コード、顧客ID、車両ID
            If (subcustTbl2.Rows.Count > 0) Then
                For n = 1 To subcustTbl2.Rows.Count - 1
                    '誘致最新化シーケンス取得
                    'SEQ_NO = da.GetSqPlanNewTgt() '返回：シーケンス
                    SEQ_NO = da.GetSqPlanNewTgt().Item(0).Seq '返回：シーケンス
                    '誘致最新化
                    ret = SC3080205TableAdapter.InsertPlanNewTgt(CType(SEQ_NO, String), subcustTbl2.Item(n).DLRCD, subcustTbl2.Item(n).CSTID, subcustTbl2.Item(n).VCLID, account) '入力：シーケンス、販売店コード、顧客ID、車両(IDVCLID)
                Next
            End If
        End Using

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertAttPlanNew_End")
        'ログ出力 End *****************************************************************************

        Return ret
    End Function
    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END


    ''' <summary>
    ''' 別のスタッフによって顧客情報の登録が行われた
    ''' </summary>
    ''' <remarks></remarks>
    Public Const AlreadyUpdatedCustomerInfo As Integer = 5004

    ' ''' <summary>
    ' ''' 顧客新規登録
    ' ''' </summary>
    ' ''' <param name="custDataTbl">データセット (インプット)</param>
    ' ''' <param name="msgId">メッセージID</param>
    ' ''' <returns>処理結果</returns>
    ' ''' <remarks>顧客情報を新規登録する。</remarks>
    '<EnableCommit()>
    'Public Function InsertCustomer(ByVal custDataTbl As SC3080205DataSet.SC3080205CustDataTable, ByRef msgId As Integer) As Integer

    '    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertCustomer_Start")
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertCustomer Step1")
    '    'ログ出力 End *****************************************************************************

    '    msgId = 0
    '    Dim ret As Integer = 1
    '    Dim custDataRow As SC3080205DataSet.SC3080205CustRow
    '    Dim context As StaffContext = StaffContext.Current
    '    Dim account As String = context.Account
    '    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
    '    custDataRow = custDataTbl.Item(0)

    '    'ブランクを半角一文字スペースにする
    '    Call EditDataRow(custDataRow)

    '    Using da As New SC3080205TableAdapter
    '        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    '        Dim birthday As Nullable(Of DateTime)
    '        Dim smsflg As Nullable(Of Integer)
    '        Dim actvctgryid As Nullable(Of Long)
    '        Dim resonid As Nullable(Of Long)
    '        If (Not custDataRow.IsBIRTHDAYNull) Then
    '            birthday = custDataRow.BIRTHDAY
    '        End If
    '        If (Not custDataRow.IsSMSFLGNull) Then
    '            smsflg = custDataRow.SMSFLG
    '        End If
    '        If (Not custDataRow.IsACTVCTGRYIDNull) Then
    '            actvctgryid = custDataRow.ACTVCTGRYID
    '        End If
    '        If (Not custDataRow.IsREASONIDNull) Then
    '            resonid = CType(custDataRow.REASONID, Long)
    '        End If
    '        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

    '        '2013/06/30 TCS 庄 2013/10対応版 START
    '        '顧客シーケンス采番
    '        Dim seqno As Decimal
    '        seqno = da.GetNewcustseq()

    '        custDataRow.CSTID = CStr(seqno)
    '        '2013/06/30 TCS 庄 2013/10対応版 END

    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertCustomer custDataRow.CSTID = " + custDataRow.CSTID)
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertCustomer Step2")
    '        'ログ出力 End *****************************************************************************

    '        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    '        '未取引客個人情報新規作成
    '        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    '        ret = da.InsertNewcustomer(custDataRow.CSTID, _
    '                                    custDataRow.CUSTYPE, _
    '                                    custDataRow.EMPLOYEENAME, _
    '                                    custDataRow.EMPLOYEEDEPARTMENT, _
    '                                    custDataRow.EMPLOYEEPOSITION, _
    '                                    custDataRow.SOCIALID, _
    '                                    custDataRow.NAME, _
    '                                    custDataRow.NAMETITLE_CD, _
    '                                    custDataRow.NAMETITLE, _
    '                                    custDataRow.SEX, _
    '                                    custDataRow.ZIPCODE, _
    '                                    custDataRow.ADDRESS, _
    '                                    custDataRow.ADDRESS1, _
    '                                    custDataRow.ADDRESS2, _
    '                                    custDataRow.ADDRESS3, _
    '                                    custDataRow.TELNO, _
    '                                    custDataRow.MOBILE, _
    '                                    custDataRow.FAXNO, _
    '                                    custDataRow.BUSINESSTELNO, _
    '                                    custDataRow.EMAIL1, _
    '                                    custDataRow.EMAIL2, _
    '                                    birthday, _
    '                                    custDataRow.DUMMYNAMEFLG, _
    '                                    custDataRow.UPDATEACCOUNT)
    '        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
    '        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

    '        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    '        '未取引客個人情報(販売店)新規作成
    '        ret = SC3080205TableAdapter.InsertNewcustome_dlr(custDataRow.DLRCD, custDataRow.CSTID, account)

    '        '未取引客個人情報（車両）新規作成
    '        ret = SC3080205TableAdapter.InsertNewcustomer_vcl(custDataRow.DLRCD, custDataRow.CSTID, actvctgryid, resonid, _
    '                                                            custDataRow.AC_MODFFUNCDVS, custDataRow.STRCDSTAFF, custDataRow.STAFFCD, account) '自社客連番、販売店コード、AC変更機、活動除外理由ID

    '        '誘致最新化メソッド実行
    '        InsertAttPlanNew(custDataRow.CSTID)
    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertCustomer_End")
    '        'ログ出力 End *****************************************************************************
    '        '2013/06/30 TCS 趙 2013/10対応版　既存流用 NED
    '        '更新： 2012/01/26 TCS 安田 【SALES_1B】来店実績更新
    '        If (ret <> 0) Then
    '            If (custDataRow.IsVISITSEQNull() = False) Then
    '                Dim returnID As Integer = 0
    '                Dim biz As New UpdateSalesVisitBusinessLogic
    '                biz.UpdateVisitCustomerInfo(custDataRow.VISITSEQ, _
    '                                            CType(custDataRow.CUSTFLG, String), _
    '                                            custDataRow.CSTID, _
    '                                            custDataRow.STAFFCD, _
    '                                            "SC3080205", _
    '                                            returnID)
    '                If (returnID = AlreadyUpdatedCustomerInfo) Then
    '                    Me.Rollback = True
    '                    Return returnID 'すでに他のユーザーが更新ずみ
    '                End If
    '            End If
    '        End If
    '        '更新： 2012/01/26 TCS 安田 【SALES_1B】来店実績更新


    '        '更新に失敗していたらロールバック
    '        If ret = 0 Then
    '            Me.Rollback = True
    '            Return 0
    '        End If

    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Debug("InsertCustomer Step3")
    '        'ログ出力 End *****************************************************************************

    '    End Using

    '    Return ret

    'End Function

    ''' <summary>
    ''' 住所検索バリデーション判定
    ''' </summary>
    ''' <param name="custDataTbl">メッセージID</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>住所検索の際のバリデーションを判定する。</remarks>
    Public Shared Function CheckAddressValidation(ByVal custDataTbl As SC3080205DataSet.SC3080205CustDataTable, ByRef msgId As Integer) As Boolean

        msgId = 0
        Dim custDataRow As SC3080205DataSet.SC3080205CustRow
        If (custDataTbl.Rows.Count = 0) Then
            custDataRow = Nothing
        Else
            custDataRow = custDataTbl.Item(0)
        End If

        '郵便番号が半角文字以外	郵便番号を32文字以内、半角文字で入力してください。
        If (Validation.IsPostalCode(custDataRow.ZIPCODE) = False) Then
            msgId = 40916
            Return False
        End If
        '郵便番号が32文字より多い	郵便番号を32文字以内、半角文字で入力してください。
        If (Validation.IsCorrectDigit(custDataRow.ZIPCODE, 32) = False) Then
            msgId = 40915
            Return False
        End If

        Return True

    End Function

    ''' <summary>
    ''' 住所検索
    ''' </summary>
    ''' <param name="custDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>住所を取得する。</remarks>
    Public Shared Function GetAddress(ByVal custDataTbl As SC3080205DataSet.SC3080205CustDataTable, ByRef msgId As Integer) As SC3080205DataSet.SC3080205ZipDataTable

        msgId = 0
        Dim zipcode As String
        zipcode = custDataTbl.Item(0).ZIPCODE

        zipcode = zipcode.Replace("-", "")

        Using da As New SC3080205TableAdapter
            Return da.GetAddress(zipcode)
        End Using

    End Function

    ''' <summary>
    ''' 断念理由リスト取得
    ''' </summary>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>断念リストを取得する。</remarks>
    Public Shared Function GetGiveupReason(ByRef msgId As Integer) As SC3080205DataSet.SC3080205OmitreasonDataTable

        msgId = 0
        Using da As New SC3080205TableAdapter
            Return da.GetGiveupReason()
        End Using

    End Function
#End Region

#Region "Protectedメソット"

    ''' <summary>
    ''' Trimする (DBに空白の場合に、半角スペース１文字しとして出力されているため)
    ''' </summary>
    ''' <param name="val">値</param>
    ''' <returns>Trim値</returns>
    ''' <remarks>文字列をTrimする。</remarks>
    Protected Shared Function DBValueToTrim(ByVal val As String) As String

        If (String.IsNullOrEmpty(val) = True) Then
            Return String.Empty
        End If

        Return val.Trim

    End Function

    ''' <summary>
    ''' 空白を、半角スペース１文字に変換する (DBに空白の場合に、半角スペース１文字しとして出力されているため)
    ''' </summary>
    ''' <param name="val">値</param>
    ''' <returns>変換値</returns>
    ''' <remarks>空白を、半角スペース１文字に変換。</remarks>
    Protected Shared Function BlanckToSpace1(ByVal val As String) As String

        If (String.IsNullOrEmpty(val)) Then
            Return " "
        End If

        Return val

    End Function

    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    ''' <summary>
    ''' データ行を更新用に編集する
    ''' </summary>
    ''' <param name="custDataRow">データ行</param>
    ''' <remarks>データ行を更新用に編集する。</remarks>
    Protected Shared Sub EditDataRow(ByVal custDataRow As SC3080205DataSet.SC3080205CustRow)
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        '国民ID、免許証番号等
        custDataRow.SOCIALID = BlanckToSpace1(custDataRow.SOCIALID)

        '個人/法人区分
        custDataRow.CUSTYPE = BlanckToSpace1(custDataRow.CUSTYPE)        '顧客タイプ

        '顧客氏名
        custDataRow.NAME = BlanckToSpace1(custDataRow.NAME)

        '敬称コード
        custDataRow.NAMETITLE_CD = BlanckToSpace1(custDataRow.NAMETITLE_CD)

        '敬称
        custDataRow.NAMETITLE = BlanckToSpace1(custDataRow.NAMETITLE)

        '郵便番号
        custDataRow.ZIPCODE = BlanckToSpace1(custDataRow.ZIPCODE)

        '住所
        custDataRow.ADDRESS = BlanckToSpace1(custDataRow.ADDRESS)

        '自宅電話番号
        custDataRow.TELNO = BlanckToSpace1(custDataRow.TELNO)

        '携帯電話番号
        custDataRow.MOBILE = BlanckToSpace1(custDataRow.MOBILE)

        'FAX番号
        custDataRow.FAXNO = BlanckToSpace1(custDataRow.FAXNO)

        '勤務地電話番号
        custDataRow.BUSINESSTELNO = BlanckToSpace1(custDataRow.BUSINESSTELNO)

        'E-mailアドレス１
        custDataRow.EMAIL1 = BlanckToSpace1(custDataRow.EMAIL1)

        'E-mailアドレス２
        custDataRow.EMAIL2 = BlanckToSpace1(custDataRow.EMAIL2)

        '生年月日

        '性別
        custDataRow.SEX = BlanckToSpace1(custDataRow.SEX)

        'AC変更アカウント
        'If (custDataRow.IsACTVCTGRYIDNull) Then
        '    custDataRow.ACTVCTGRYID = SC3080205BusinessLogic.InitActvctgryId
        'End If

        'AC変更アカウント
        custDataRow.AC_MODFACCOUNT = BlanckToSpace1(custDataRow.AC_MODFACCOUNT)

        'AC変更機能
        custDataRow.AC_MODFFUNCDVS = BlanckToSpace1(custDataRow.AC_MODFFUNCDVS)

        'SMS配信可否

        'e-mail配信可否
        custDataRow.EMAILFLG = BlanckToSpace1(custDataRow.EMAILFLG)

        'D-mail配信可否

        '担当者氏名（法人）
        custDataRow.EMPLOYEENAME = BlanckToSpace1(custDataRow.EMPLOYEENAME)

        '担当者部署名（法人）
        custDataRow.EMPLOYEEDEPARTMENT = BlanckToSpace1(custDataRow.EMPLOYEEDEPARTMENT)

        '役職（法人）
        custDataRow.EMPLOYEEPOSITION = BlanckToSpace1(custDataRow.EMPLOYEEPOSITION)

    End Sub

#End Region

End Class
