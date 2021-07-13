'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080205BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客編集 (ビジネスロジック)
'補足： 
'作成： 2011/11/07 TCS 安田
'更新： 2012/01/26 TCS 安田 【SALES_1B】来店実績更新
'更新： 2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2013/06/30 TCS 趙　 【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/11/27 TCS 各務 Aカード情報相互連携開発
'更新： 2014/02/03 TCS 松月 【A STEP2】法人区分必須対応（号口切替BTS-68） 
'更新： 2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）
'更新： 2014/04/01 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/05/01 TCS 松月 新PF残課題No.21  
'更新： 2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354)
'更新： 2014/07/10 TCS 外崎 TMT要望（国民IDの必須制限解除）
'更新： 2015/06/08 TCS 中村 TMT課題対応(#2)
'更新： 2015/09/09 TCS 浅野 TR-SLT-TMT-20150626-001
'更新： 2016/11/28 TCS 曽出 （トライ店システム評価）基幹連携に伴う顧客車両情報管理機能評価　【TR-V4-TMT-20160623-001】
'更新： 2017/11/20 TCS 河原 TKM独自機能開発
'更新： 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究
'更新： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'更新： 2020/01/20 TS  岩田 TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045)
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
'2013/06/30 TCS 趙 2013/10対応版　既存流用 START
Imports Toyota.eCRB.SystemFrameworks.Web
'2013/06/30 TCS 趙 2013/10対応版　既存流用 END
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.Common.VisitResult.BizLogic
Imports Toyota.eCRB.CommonUtility.DataAccess



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

    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 機能パラメータ (性別表示設定フラグ)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const GenderDispSetting As String = "TABLET_GENDER_DISP_SETTING_FLG"

    ''' <summary>
    ''' 機能パラメータ (住所表示順フラグ)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const AddressDispDirection As String = "TABLET_ADDRESS_DISP_DIRECTION_FLG"

    ''' <summary>
    ''' 機能パラメータ (ラベル・敬称表示設定フラグ)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const LabelNametitleSetting As String = "TABLET_LABEL_NAMETITLE_SETTING_FLG"

    ''' <summary>
    ''' 機能パラメータ (住所データクレンジング可否フラグ)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const AddressDataCleansing As String = "TABLET_ADDRESS_DATACLEANSING_FLG"
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

    '2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 START
    ''' <summary>
    ''' 機能パラメータ（住所１自動入力フラグ）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Address1AutoInput As String = "TABLET_ADDRESS1_AUTOINPUT_FLG"
    '2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 END

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
    ''' <summary>
    ''' システム設定の指定パラメータ V3データ表示フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_ICROP_UPDATE_ORG_CST_ADD_FLG As String = "ICROP_UPDATE_ORG_CST_ADD_FLG"
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

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

    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 性別 (3:その他)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Other As String = "3"

    ''' <summary>
    ''' 性別 (2:不明)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Unknown As String = "2"
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
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

    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 個人法人項目 (Thai)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Thai As String = "P"
    ''' <summary>
    ''' 個人法人項目 (Foreigner)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Foreigner As String = "F"
    ''' <summary>
    ''' 個人法人項目 (Company)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const Company As String = "C"
    ''' <summary>
    ''' 個人法人項目 (Govt Org)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const GovtOrg As String = "G"
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END


    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
    ''' <summary>
    ''' 更新機能判定(UPDATE_FUNCTION_JUDGE)の列番号
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum UpdFuncJudgeColIndexCstDlr As Short
        ''' <summary>
        ''' 商業情報受取区分列
        ''' </summary>
        ''' <remarks></remarks>
        CommercialRecvType = 18
    End Enum
    ''' <summary>
    ''' 更新機能判定：基幹連携済み
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UpdFuncJudgeLinkedDMS As String = "1"
    ''' <summary>
    ''' 更新機能判定：基幹連携無し
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UpdFuncJudgeUnLinkedDMS As String = "0"
    ''' <summary>
    ''' 更新機能判定：タブレット編集可
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UpdFuncJudgeTabletEditable As String = "2"
    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' お客様情報クレンジング機能使用可否フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_USE_CUSTOMERDATA_CLEANSING_FLG As String = "USE_CUSTOMERDATA_CLEANSING_FLG"
    '2017/11/20 TCS 河原 TKM独自機能開発 END

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

        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

        ' 性別表示設定
        sysEnvRow = sysEnv.GetSystemEnvSetting(GenderDispSetting)
        If IsNothing(sysEnvRow) Then
            '取得できなかった場合、"0"を設定
            custDataRow.GENDER_DISP_SETTING = "0"
        Else
            custDataRow.GENDER_DISP_SETTING = sysEnvRow.PARAMVALUE
        End If

        ' 住所表示順
        sysEnvRow = sysEnv.GetSystemEnvSetting(AddressDispDirection)
        If IsNothing(sysEnvRow) Then
            '取得できなかった場合、"0"を設定
            custDataRow.ADDRESS_DISP_DIRECTION = "0"
        Else
            custDataRow.ADDRESS_DISP_DIRECTION = sysEnvRow.PARAMVALUE
        End If

        ' ラベル・敬称表示設定
        sysEnvRow = sysEnv.GetSystemEnvSetting(LabelNametitleSetting)
        If IsNothing(sysEnvRow) Then
            '取得できなかった場合、"0"を設定
            custDataRow.LABEL_NAMETITLE_SETTING = "0"
        Else
            custDataRow.LABEL_NAMETITLE_SETTING = sysEnvRow.PARAMVALUE
        End If

        ' 住所データクレンジング可否
        sysEnvRow = sysEnv.GetSystemEnvSetting(AddressDataCleansing)
        If IsNothing(sysEnvRow) Then
            '取得できなかった場合、"0"を設定
            custDataRow.ADDRESS_DATACLEANSING_FLG = "0"
        Else
            custDataRow.ADDRESS_DATACLEANSING_FLG = sysEnvRow.PARAMVALUE
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        '2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 START
        sysEnvRow = sysEnv.GetSystemEnvSetting(Address1AutoInput)
        If IsNothing(sysEnvRow) Then
            '取得できなかった場合、"0"を設定
            custDataRow.ADDRESS1_AUTOINPUT_FLG = "0"
        Else
            custDataRow.ADDRESS1_AUTOINPUT_FLG = sysEnvRow.PARAMVALUE
        End If
        '2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START 
        If (custDataRow.CUSTFLG = OrgCustFlg) Then
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            'Dim sysEnv As New SystemEnvSetting
            'Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
            sysEnvRow = sysEnv.GetSystemEnvSetting(IcropUpdateFlgOrgCustomer)
            custDataRow.ORGINPUTFLG = sysEnvRow.PARAMVALUE
        End If

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        Return custDataTbl

    End Function

    '2017/11/20 TCS 河原 TKM独自機能開発 START
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

        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        'クレンジング機能使用可否フラグ取得（取得できなければ0）
        Dim use_cleansing_flg As String
        Dim systemBiz As New SystemSetting
        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRow = systemBiz.GetSystemSetting(C_USE_CUSTOMERDATA_CLEANSING_FLG)

        If IsNothing(dataRow) Then
            use_cleansing_flg = "0"
        Else
            use_cleansing_flg = dataRow.SETTING_VAL
        End If
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
        If custDataRow.CST_ORGNZ_NAME <> "" Then
            ' 顧客組織名称が256文字より多い
            If Not Validation.IsCorrectDigit(custDataRow.CST_ORGNZ_NAME, 256) Then
                msgId = 4000903
                Return False
            End If

            ' 顧客組織名称に絵文字が入っている
            If Not Validation.IsValidString(custDataRow.CST_ORGNZ_NAME) Then
                msgId = 4000904
                Return False
            End If
        End If
        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

        'ファーストネームが未入力の場合
        If (enabledTable.Item(SC3080205TableAdapter.IdFirstName) = True) Then
            '新規登録時は必須チェックをしない
            If mode = ModeEdit Then
                If (String.IsNullOrEmpty(custDataRow.FIRSTNAME)) Then
                    'ラベル設定フラグがON かつ 個人法人項目が03(Company)または04(Govt Org)
                    If (custDataRow.LABEL_NAMETITLE_SETTING = "1" And _
                        (custDataRow.PRIVATE_FLEET_ITEM_CD = Company Or custDataRow.PRIVATE_FLEET_ITEM_CD = GovtOrg)) Then
                        '会社名を入力してください。
                        msgId = 40976
                    Else
                        'ファーストネームを入力してください。
                        msgId = 40902
                    End If
                    Return False
                End If
            End If

            'ファーストネームが64文字より多い
            If (Validation.IsCorrectDigit(custDataRow.FIRSTNAME, 64) = False) Then
                'ラベル設定フラグがON かつ 個人法人項目が03(Company)または04(Govt Org)
                If (custDataRow.LABEL_NAMETITLE_SETTING = "1" And _
                    (custDataRow.PRIVATE_FLEET_ITEM_CD = Company Or custDataRow.PRIVATE_FLEET_ITEM_CD = GovtOrg)) Then
                    '会社名を64文字以内で入力してください。
                    msgId = 40977
                Else
                    'ファーストネームを64文字以内で入力してください。
                    msgId = 40935
                End If
                Return False
            End If

            'ファーストネームに絵文字が入っている
            If (Validation.IsValidString(custDataRow.FIRSTNAME) = False) Then
                'ラベル設定フラグがON かつ 個人法人項目が03(Company)または04(Govt Org)
                If (custDataRow.LABEL_NAMETITLE_SETTING = "1" And _
                    (custDataRow.PRIVATE_FLEET_ITEM_CD = Company Or custDataRow.PRIVATE_FLEET_ITEM_CD = GovtOrg)) Then
                    '会社名は禁則文字以外で入力してください。
                    msgId = 40978
                Else
                    'ファーストネームは禁則文字以外で入力してください。
                    msgId = 40938
                End If
                Return False
            End If

            'ファーストネームに半角スペースが入っている
            If (Not String.IsNullOrEmpty(custDataRow.CUSTYPE)) Then
                If String.Equals(use_cleansing_flg, "1") And Not custDataRow.CUSTYPE.Equals(Houjin) Then
                    If ((Not String.IsNullOrEmpty(custDataRow.FIRSTNAME)) And (custDataRow.FIRSTNAME).IndexOf(" "c) >= 0) Then
                        'ラベル設定フラグがON かつ 個人法人項目が03(Company)または04(Govt Org)
                        If (custDataRow.LABEL_NAMETITLE_SETTING = "1" And _
                            (custDataRow.PRIVATE_FLEET_ITEM_CD = Company Or custDataRow.PRIVATE_FLEET_ITEM_CD = GovtOrg)) Then
                            '会社名に半角スペースが含まれています。
                            msgId = 40985
                        Else
                            'ファーストネームに半角スペースが含まれています。
                            msgId = 40982
                        End If
                        Return False
                    End If
                End If
            End If

        End If

        If (enabledTable.Item(SC3080205TableAdapter.IdMiddleName) = True) Then
            If (Not String.IsNullOrEmpty(custDataRow.MIDDLENAME)) Then
                'ミドルネームが64文字より多い	ミドルネームを64文字以内で入力してください。
                If (Validation.IsCorrectDigit(custDataRow.MIDDLENAME, 64) = False) Then
                    msgId = 40936
                    Return False
                End If

                'ミドルネームに絵文字が入っている
                If (Validation.IsValidString(custDataRow.MIDDLENAME) = False) Then
                    msgId = 40939
                    Return False
                End If

                'ミドルネームに半角スペースが入っている
                If ((Not String.IsNullOrEmpty(custDataRow.MIDDLENAME)) And (custDataRow.MIDDLENAME).IndexOf(" "c) >= 0) Then
                    'ミドルネームに半角スペースが含まれています。
                    msgId = 40983
                    Return False
                End If
            End If
        End If

        If (enabledTable.Item(SC3080205TableAdapter.IdLastName) = True) Then
            If (Not String.IsNullOrEmpty(custDataRow.LASTNAME)) Then
                'ラストネームが64文字より多い	
                If (Validation.IsCorrectDigit(custDataRow.LASTNAME, 64) = False) Then
                    'ラベル設定フラグがON かつ 個人法人項目が03(Company)または04(Govt Org)
                    If (custDataRow.LABEL_NAMETITLE_SETTING = "1" And _
                        (custDataRow.PRIVATE_FLEET_ITEM_CD = Company Or custDataRow.PRIVATE_FLEET_ITEM_CD = GovtOrg)) Then
                        '担当者を64文字以内で入力してください。
                        msgId = 40980
                    Else
                        'ラストネームを64文字以内で入力してください。
                        msgId = 40937
                    End If
                    Return False
                End If

                'ラストネームに絵文字が入っている
                If (Validation.IsValidString(custDataRow.LASTNAME) = False) Then
                    'ラベル設定フラグがON かつ 個人法人項目が03(Company)または04(Govt Org)
                    If (custDataRow.LABEL_NAMETITLE_SETTING = "1" And _
                        (custDataRow.PRIVATE_FLEET_ITEM_CD = Company Or custDataRow.PRIVATE_FLEET_ITEM_CD = GovtOrg)) Then
                        '担当者は禁則文字以外で入力してください。
                        msgId = 40981
                    Else
                        'ラストネームは禁則文字以外で入力してください。
                        msgId = 40940
                    End If
                    Return False
                End If

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
            '新規登録時は必須チェックをしない
            If mode = ModeEdit Then
                '携帯電話番号か自宅電話番号、どちらかを入力してください。
                If (String.IsNullOrEmpty(custDataRow.TELNO) AndAlso String.IsNullOrEmpty(custDataRow.MOBILE)) Then
                    msgId = 40907
                    Return False
                End If
            End If
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

        If ((enabledTable.Item(SC3080205TableAdapter.IdAddress1) = True) Or _
            (enabledTable.Item(SC3080205TableAdapter.IdAddress2) = True) Or _
            (enabledTable.Item(SC3080205TableAdapter.IdAddress3) = True)) Then

            Dim addressTotal As String = String.Empty
            If (Not String.IsNullOrEmpty(custDataRow.ADDRESS1)) Then
                addressTotal = addressTotal & custDataRow.ADDRESS1
                '住所1に絵文字が入っている
                If (Validation.IsValidString(custDataRow.ADDRESS1) = False) Then
                    msgId = 40942
                    Return False
                End If
                '住所1にカンマが入っている
                If ((Not String.IsNullOrEmpty(custDataRow.ADDRESS1)) And System.Text.RegularExpressions.Regex.IsMatch(custDataRow.ADDRESS1, ", ")) Then
                    msgId = 40987
                    Return False
                End If
            End If
            If (Not String.IsNullOrEmpty(custDataRow.ADDRESS2)) Then
                addressTotal = addressTotal & custDataRow.ADDRESS2
                '住所2に絵文字が入っている
                If (Validation.IsValidString(custDataRow.ADDRESS2) = False) Then
                    msgId = 40943
                    Return False
                End If
                '住所2にカンマが入っている
                If ((Not String.IsNullOrEmpty(custDataRow.ADDRESS2)) And System.Text.RegularExpressions.Regex.IsMatch(custDataRow.ADDRESS2, ", ")) Then
                    msgId = 40988
                    Return False
                End If
            End If
            If (Not String.IsNullOrEmpty(custDataRow.ADDRESS3)) Then
                addressTotal = addressTotal & custDataRow.ADDRESS3
                '住所3に絵文字が入っている
                If (Validation.IsValidString(custDataRow.ADDRESS3) = False) Then
                    msgId = 40944
                    Return False
                End If
                ''住所3にカンマが入っている
                'If ((Not String.IsNullOrEmpty(custDataRow.ADDRESS3)) And System.Text.RegularExpressions.Regex.IsMatch(custDataRow.ADDRESS3, ", ")) Then
                '    msgId = 40989
                '    Return False
                'End If
            End If
            If (Not String.IsNullOrEmpty(addressTotal)) Then
                '住所1＋住所2＋住所3の長さが252文字より多い(結合文字を含めると256文字)	住所を256文字以内で入力してください。
                If (Validation.IsCorrectDigit(addressTotal, 252) = False) Then
                    msgId = 40931
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
            '国民IDロジカルチェック
            If (custDataRow.LABEL_NAMETITLE_SETTING = "1") Then
                msgId = checkSocialId(custDataRow.PRIVATE_FLEET_ITEM_CD, custDataRow.SOCIALID)
                If (msgId > 0) Then
                    Return False
                End If
            End If
        End If

        If (enabledTable.Item(SC3080205TableAdapter.IdDomicile) = True) Then
            If (Not String.IsNullOrEmpty(custDataRow.DOMICILE)) Then
                '本籍が320文字より多い	本籍を320文字以内で入力してください。
                If (Validation.IsCorrectDigit(custDataRow.DOMICILE, 320) = False) Then
                    msgId = 40945
                    Return False
                End If

                '本籍に絵文字が入っている
                If (Validation.IsValidString(custDataRow.DOMICILE) = False) Then
                    msgId = 40946
                    Return False
                End If
            End If
        End If
        If (enabledTable.Item(SC3080205TableAdapter.IdCountry) = True) Then
            If (Not String.IsNullOrEmpty(custDataRow.COUNTRY)) Then
                '国籍が64文字より多い	国籍を64文字以内で入力してください。
                If (Validation.IsCorrectDigit(custDataRow.COUNTRY, 64) = False) Then
                    msgId = 40947
                    Return False
                End If

                '国籍に絵文字が入っている
                If (Validation.IsValidString(custDataRow.COUNTRY) = False) Then
                    msgId = 40948
                    Return False
                End If
            End If
        End If

        If enabledTable.Item(SC3080205TableAdapter.IdCstIncome) _
            AndAlso Not String.IsNullOrEmpty(custDataRow.CST_INCOME) Then
            '年収：32文字超過
            If Not Validation.IsCorrectDigit(custDataRow.CST_INCOME, 32) Then
                msgId = 40992
                Return False
            End If
            '年収：禁則文字（絵文字）
            If Not Validation.IsValidString(custDataRow.CST_INCOME) Then
                msgId = 40993
                Return False
            End If
        End If

        Return True

    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END

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
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
        Dim cstDlrDataTable As SC3080205DataSet.SC3080205CustDlrDataTable = Nothing
        Dim cstId As Decimal = 0
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialize_Start")
        'ログ出力 End *****************************************************************************
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        custDataRow = custDataTbl.Item(0)

        Using da As New SC3080205TableAdapter

            If (custDataRow.CUSTFLG = OrgCustFlg) Then
                '０：自社客
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialize custDataRow.DLRCD = " + custDataRow.DLRCD)
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialize custDataRow.ORIGINALID = " + custDataRow.ORIGINALID)
                'ログ出力 End *****************************************************************************
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                retCustDataTbl = da.GetCustomer(custDataRow.ORIGINALID)
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
                '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
                Decimal.TryParse(custDataRow.ORIGINALID, cstId)
                '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END
            Else
                '１：未取引客
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialize custDataRow.DLRCD = " + custDataRow.DLRCD)
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialize custDataRow.CSTID = " + custDataRow.CSTID)
                'ログ出力 End *****************************************************************************

                '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                retCustDataTbl = da.GetNewcustomer(custDataRow.DLRCD, custDataRow.CSTID, custDataRow.VCLID)
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 ENDEND
                '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
                Decimal.TryParse(custDataRow.CSTID, cstId)
                '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END
            End If

            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
            '顧客販売店データ取得(自社/未取引共通)
            If cstId > 0 Then cstDlrDataTable = da.GetCustomerDlr(custDataRow.DLRCD, cstId)
            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END
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

        '2016/11/28 TCS 曽出 （トライ店システム評価）基幹連携に伴う顧客車両情報管理機能評価　【TR-V4-TMT-20160623-001】START
        'ファーストネーム
        custDataRow.FIRSTNAME = DBValueToTrim(retCustDataRow.FIRSTNAME)

        'ミドルネーム
        custDataRow.MIDDLENAME = DBValueToTrim(retCustDataRow.MIDDLENAME)

        'ラストネーム
        custDataRow.LASTNAME = DBValueToTrim(retCustDataRow.LASTNAME)
        '2016/11/28 TCS 曽出 （トライ店システム評価）基幹連携に伴う顧客車両情報管理機能評価　【TR-V4-TMT-20160623-001】END

        '敬称コード
        custDataRow.NAMETITLE_CD = DBValueToTrim(retCustDataRow.NAMETITLE_CD)

        '敬称
        custDataRow.NAMETITLE = DBValueToTrim(retCustDataRow.NAMETITLE)

        '郵便番号
        custDataRow.ZIPCODE = DBValueToTrim(retCustDataRow.ZIPCODE)

        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        '住所
        'custDataRow.ADDRESS = DBValueToTrim(retCustDataRow.ADDRESS)

        '2017/11/20 TCS 河原 TKM独自機能開発 START
        '住所1
        'custDataRow.ADDRESS1 = DBValueToTrim(retCustDataRow.ADDRESS1)
        '区切り文字の「カンマスペース」のスペースが除去されてしまうためLTrimに変更
        custDataRow.ADDRESS1 = LTrim(retCustDataRow.ADDRESS1)
        '2017/11/20 TCS 河原 TKM独自機能開発 END

        '住所2
        custDataRow.ADDRESS2 = DBValueToTrim(retCustDataRow.ADDRESS2)

        '住所3
        custDataRow.ADDRESS3 = DBValueToTrim(retCustDataRow.ADDRESS3)

        '住所(州)
        custDataRow.ADDRESS_STATE = DBValueToTrim(retCustDataRow.ADDRESS_STATE)

        '住所(地域)
        custDataRow.ADDRESS_DISTRICT = DBValueToTrim(retCustDataRow.ADDRESS_DISTRICT)

        '住所(市)
        custDataRow.ADDRESS_CITY = DBValueToTrim(retCustDataRow.ADDRESS_CITY)

        '住所(地区)
        custDataRow.ADDRESS_LOCATION = DBValueToTrim(retCustDataRow.ADDRESS_LOCATION)
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

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

        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
        '年収
        custDataRow.CST_INCOME = DBValueToTrim(retCustDataRow.CST_INCOME)
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END

        '顧客更新フラグ
        custDataRow.UPDATEFUNCFLG = retCustDataRow.UPDATEFUNCFLG

        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        '本籍
        custDataRow.DOMICILE = DBValueToTrim(retCustDataRow.DOMICILE)

        '国籍
        custDataRow.COUNTRY = DBValueToTrim(retCustDataRow.COUNTRY)

        '個人法人項目
        custDataRow.PRIVATE_FLEET_ITEM_CD = DBValueToTrim(retCustDataRow.PRIVATE_FLEET_ITEM_CD)
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
        ' 顧客組織コード
        custDataRow.CST_ORGNZ_CD = DBValueToTrim(retCustDataRow.CST_ORGNZ_CD)

        ' 顧客組織入力区分
        custDataRow.CST_ORGNZ_INPUT_TYPE = DBValueToTrim(retCustDataRow.CST_ORGNZ_INPUT_TYPE)

        ' 顧客組織名称
        custDataRow.CST_ORGNZ_NAME = DBValueToTrim(retCustDataRow.CST_ORGNZ_NAME)

        ' 顧客サブカテゴリ2コード
        custDataRow.CST_SUBCAT2_CD = DBValueToTrim(retCustDataRow.CST_SUBCAT2_CD)

        ' 顧客サブカテゴリ2名称
        custDataRow.CST_SUBCAT2_NAME = DBValueToTrim(retCustDataRow.CST_SUBCAT2_NAME)

        ' ローカル顧客マスタ行ロックバージョン
        custDataRow.CST_LOCAL_ROW_LOCK_VERSION = retCustDataRow.CST_LOCAL_ROW_LOCK_VERSION
        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
        custDataRow.DUMMYNAMEFLG = retCustDataRow.DUMMYNAMEFLG
        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        custDataRow.LOCKVERSION = retCustDataRow.LOCKVERSION
        If (custDataRow.CUSTFLG <> OrgCustFlg) Then
            custDataRow.VCLLOCKVERSION = retCustDataRow.VCLLOCKVERSION
        End If

        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
        '販売店顧客行ロックバージョン
        custDataRow.CST_DLR_ROW_LOCK_VERSION = 0
        If Not cstDlrDataTable Is Nothing AndAlso cstDlrDataTable.Rows.Count > 0 Then
            '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
            Dim systemUpdateFlg As String
            Dim systemBiz As New SystemSetting
            Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
            dataRow = systemBiz.GetSystemSetting(C_ICROP_UPDATE_ORG_CST_ADD_FLG)

            If (dataRow Is Nothing) Then
                systemUpdateFlg = Nothing
            Else
                systemUpdateFlg = dataRow.SETTING_VAL
            End If
            '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

            '商業情報受取区分
            custDataRow.COMMERCIAL_RECV_TYPE = cstDlrDataTable(0).COMMERCIAL_RECV_TYPE
            custDataRow.IsReadOnly_COMMERCIAL_RECV_TYPE = False
            '自社客の場合、更新可能か判定する
            If (custDataRow.CUSTFLG = OrgCustFlg) Then
                custDataRow.IsReadOnly_COMMERCIAL_RECV_TYPE = IsReadOnlyJudgeCstDlr(systemUpdateFlg, cstDlrDataTable(0).UPDATE_FUNCTION_JUDGE, _
                                                                                    UpdFuncJudgeColIndexCstDlr.CommercialRecvType _
                                                                                    , String.IsNullOrEmpty(cstDlrDataTable(0).COMMERCIAL_RECV_TYPE.Trim()))
            End If
            '販売店顧客行ロックバージョン
            custDataRow.CST_DLR_ROW_LOCK_VERSION = cstDlrDataTable(0).ROW_LOCK_VERSION
            '変更保存
            custDataRow.Table.AcceptChanges()
        End If
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialize_End")
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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNameTitleList_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetNameTitleList_End")
            'ログ出力 End *****************************************************************************
            Return da.GetNametitle(dispflglist)
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        End Using

    End Function

    ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
    ''' <summary>
    ''' 顧客更新
    ''' </summary>
    ''' <param name="custDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客情報を更新する。</remarks>
    <EnableCommit()>
    Public Function UpdateCustomer(ByVal custDataTbl As SC3080205DataSet.SC3080205CustDataTable, ByVal enabledTable As Dictionary(Of Integer, Boolean), ByRef msgId As Integer, ByVal actEditFlg As Integer) As Integer
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
        msgId = 0
        Dim ret As Integer = 1
        Dim custDataRow As SC3080205DataSet.SC3080205CustRow
        custDataRow = custDataTbl.Item(0)
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        Dim cstId As String

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomer_Start")
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
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomer Step1")
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
                SC3080205TableAdapter.SelectCstLock(cstId)
            Catch ex As Exception
                Return 0
            End Try
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomer custDataRow.ORIGINALID = " + custDataRow.ORIGINALID)
            'ログ出力 End *****************************************************************************
            '顧客情報更新
            '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            ret = da.UpdateCustomer(cstId, _
                                    custDataRow.SOCIALID, _
                                    custDataRow.CUSTYPE, _
                                    custDataRow.NAME, _
                                    custDataRow.FIRSTNAME, _
                                    custDataRow.MIDDLENAME, _
                                    custDataRow.LASTNAME, _
                                    custDataRow.NAMETITLE_CD, _
                                    custDataRow.NAMETITLE, _
                                    custDataRow.ZIPCODE, _
                                    custDataRow.ADDRESS, _
                                    custDataRow.ADDRESS1, _
                                    custDataRow.ADDRESS2, _
                                    custDataRow.ADDRESS3, _
                                    custDataRow.ADDRESS_STATE, _
                                    custDataRow.ADDRESS_DISTRICT, _
                                    custDataRow.ADDRESS_CITY, _
                                    custDataRow.ADDRESS_LOCATION, _
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
                                    custDataRow.PRIVATE_FLEET_ITEM_CD, _
                                    custDataRow.DOMICILE, _
                                    custDataRow.COUNTRY, _
                                    custDataRow.CST_INCOME, _
                                    custDataRow.UPDATEFUNCFLG, _
                                    custDataRow.UPDATEACCOUNT,
                                    custDataRow.LOCKVERSION)
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
            '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
            If ret = 0 Then
                Me.Rollback = True
                Return -1
            End If

            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
            With custDataRow
                ' TB_LM_CUSTOMER に該当のレコードが存在しない場合は新規追加
                If .CST_LOCAL_ROW_LOCK_VERSION = -1 Then
                    ret = SC3080205TableAdapter.InsertCustomerLocal(cstId, .CST_ORGNZ_CD, .CST_ORGNZ_INPUT_TYPE, .CST_ORGNZ_NAME, .CST_SUBCAT2_CD, .UPDATEACCOUNT)
                Else
                    ret = da.UpdateCustomerLocal(cstId, .CST_ORGNZ_CD, .CST_ORGNZ_INPUT_TYPE, .CST_ORGNZ_NAME, .CST_SUBCAT2_CD, .UPDATEACCOUNT, .CST_LOCAL_ROW_LOCK_VERSION)
                End If
            End With

            If ret = 0 Then
                Me.Rollback = True
                Return -1
            End If
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

            '未取引客個人情報更新処理
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            If (custDataRow.CUSTFLG <> OrgCustFlg And actEditFlg = 1) Then
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomer custDataRow.CSTID = " + custDataRow.CSTID)
                'ログ出力 End *****************************************************************************              
                ret = da.UpdateNewcustomer(custDataRow.DLRCD, _
                                           custDataRow.CSTID, _
                                           CType(custDataRow.ACTVCTGRYID, String), _
                                           custDataRow.AC_MODFFUNCDVS, _
                                           custDataRow.REASONID, _
                                           custDataRow.UPDATEACCOUNT, _
                                           custDataRow.VCLLOCKVERSION, _
                                           custDataRow.VCLID)
                '2013/05/01 TCS 松月 新PF残課題No.21 Start
                If ret = 0 Then
                    Me.Rollback = True
                    Return -1
                End If

                ret = SC3080205TableAdapter.InsertCstVclActCat(custDataRow.DLRCD, _
                                           custDataRow.CSTID, _
                                           CType(custDataRow.ACTVCTGRYID, String),
                                           custDataRow.REASONID, _
                                           custDataRow.UPDATEACCOUNT,
                                           custDataRow.VCLID)
                '2013/05/01 TCS 松月 新PF残課題No.21 End

            End If
            If ret = 0 Then
                Me.Rollback = True
                Return -1
            End If

            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
            '商業情報受取区分更新
            Dim cstIdDecimal As Decimal = 0
            If Decimal.TryParse(cstId, cstIdDecimal) Then
                ret = UpdateCstCommercialRecvType(cstIdDecimal, custDataRow)
                If ret < 0 Then
                    'ログ出力 Start ***************************************************************************
                    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomer custDataRow.CSTID = " + custDataRow.CSTID + "/UpdateCstCommercialRecvType ret=" + CStr(ret))
                    'ログ出力 End *****************************************************************************              
                    Me.Rollback = True
                    Return -1
                End If
            End If
            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END

            '誘致最新化メソッド実行
            ret = InsertAttPlanNew(cstId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomer Step2")
            'ログ出力 End *****************************************************************************
            '更新に失敗していたらロールバック
            If ret = 0 Then
                Me.Rollback = True
                Return 0
            End If

        End Using

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCustomer_End")
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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertAttPlanNew_Start")
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
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertAttPlanNew_End")
        'ログ出力 End *****************************************************************************

        Return ret
    End Function
    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START DEL
    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END


    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
    ''' <summary>
    ''' 商業情報受取区分 更新
    ''' </summary>
    ''' <param name="cstRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateCstCommercialRecvType(ByVal cstId As Decimal, ByVal cstRow As SC3080205DataSet.SC3080205CustRow) As Integer
        Dim ret As Integer = -1
        Dim dt As SC3080205DataSet.SC3080205CustDlrDataTable = Nothing
        Dim sysUpdFlg As String = String.Empty

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCstCommercialRecvType_Start")
        'ログ出力 End *****************************************************************************

        If cstRow Is Nothing Then Return -1

        Using da As New SC3080205TableAdapter()

            '商業情報受取区分の変更確認
            dt = da.GetCustomerDlr(cstRow.DLRCD, cstId)
            If dt Is Nothing OrElse dt.Rows.Count = 0 Then Return -2
            If dt(0).ROW_LOCK_VERSION > cstRow.CST_DLR_ROW_LOCK_VERSION Then Return -3
            '変更の無い場合は正常終了とする。
            If dt(0).COMMERCIAL_RECV_TYPE.Equals(cstRow.COMMERCIAL_RECV_TYPE) Then Return 0

            '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
            Dim systemBiz As New SystemSetting
            Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
            dataRow = systemBiz.GetSystemSetting(C_ICROP_UPDATE_ORG_CST_ADD_FLG)

            If (dataRow Is Nothing) Then
                sysUpdFlg = Nothing
            Else
                '変更可能か判定(自社客のみ)
                sysUpdFlg = dataRow.SETTING_VAL
            End If
            '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

            If cstRow.CUSTFLG = OrgCustFlg Then
                If IsReadOnlyJudgeCstDlr(sysUpdFlg, dt(0).UPDATE_FUNCTION_JUDGE _
                                         , UpdFuncJudgeColIndexCstDlr.CommercialRecvType _
                                         , String.IsNullOrEmpty(dt(0).COMMERCIAL_RECV_TYPE.Trim())) Then
                    '変更不可の場合正常終了とする。
                    Return 0
                End If

                '変更可能の場合、UPDATE_FUNCTION_JUDGE更新
                dt(0).UPDATE_FUNCTION_JUDGE = dt(0).UPDATE_FUNCTION_JUDGE.Remove(UpdFuncJudgeColIndexCstDlr.CommercialRecvType - 1, 1).Insert(UpdFuncJudgeColIndexCstDlr.CommercialRecvType - 1, SC3080205BusinessLogic.UpdCD)
            End If

            '販売店顧客の更新
            ret = da.UpdateCustomerDlr(cstRow.DLRCD, cstId, cstRow.COMMERCIAL_RECV_TYPE, dt(0).UPDATE_FUNCTION_JUDGE, cstRow.UPDATEACCOUNT, cstRow.CST_DLR_ROW_LOCK_VERSION)
            If ret <> 1 Then Return -4

            '商業情報受取区分履歴の追加
            ret = da.InsertCommercialChgHis(cstRow.DLRCD, cstId, cstRow.COMMERCIAL_RECV_TYPE, cstRow.UPDATEACCOUNT)
            If ret <> 1 Then Return -5

        End Using

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateCstCommercialRecvType_End")
        'ログ出力 End *****************************************************************************

        Return ret
    End Function
    '2015/09/09 TCS 浅野 TR-V3-TMT-20150602-001 START
    ''' <summary>
    ''' 読み取り専用項目か判定する（販売店顧客）
    ''' </summary>
    ''' <param name="systemUpdateFlg">システム環境設定の手動入力可否用設定(TB_M_SYSTEM_SETTING・ICROP_UPDATE_ORG_CST_ADD_FLG)</param>
    ''' <param name="updateFunctionJudge">販売店顧客.更新機能判定(TB_M_CUSTOMER_DLR.UPDATE_FUNCTION_JUDGE)</param>
    ''' <param name="colIndex">更新機能判定 桁番号</param>
    ''' <param name="isEmptyValue">列の値が空か(半角スペース、1999/01/01 など)</param>
    ''' <returns>True：読み取り専用(タブレットによる更新不可) / False：編集可能</returns>
    ''' <remarks></remarks>
    Private Shared Function IsReadOnlyJudgeCstDlr(ByVal systemUpdateFlg As String, ByVal updateFunctionJudge As String _
                                           , ByVal colIndex As UpdFuncJudgeColIndexCstDlr _
                                           , ByVal isEmptyValue As Boolean) As Boolean
        Dim ret As Boolean = False
        Dim sysFlg As String = String.Empty
        Const SysUpdFlgDisabled As String = "0" '行の更新機能判定を無視して編集不可
        Const SysUpdFlgEnabled As String = "1"  '行の更新機能判定を無視して編集可
        'Const SysUpdFlgEnabledWithoutLinkDMS As String = "2"   '行の更新機能判定に依存
        Const SysUpdFlgEnabledwithEmptyValueWithoutLinkDMS As String = "3" '行の更新機能判定・入力値に依存

        If Not String.IsNullOrEmpty(systemUpdateFlg) Then
            If colIndex <= updateFunctionJudge.Length Then
                sysFlg = updateFunctionJudge.Substring(colIndex - 1, 1)
            Else
                '桁が足りない場合は編集可とする
                sysFlg = SysUpdFlgEnabled
            End If
        End If

        If sysFlg = SysUpdFlgDisabled Then
            ret = True
        ElseIf sysFlg <> SysUpdFlgEnabled Then
            '基幹連携された列は編集不可
            If updateFunctionJudge.Substring(colIndex - 1, 1).Equals(UpdFuncJudgeLinkedDMS) Then
                ret = True
                '基幹連携された値が空の場合編集可
                If sysFlg = SysUpdFlgEnabledwithEmptyValueWithoutLinkDMS AndAlso isEmptyValue Then
                    ret = False
                End If
            End If
        End If

        Return ret
    End Function
    '2015/09/09 TCS 浅野 TR-V3-TMT-20150602-001 END
    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END


    ''' <summary>
    ''' 別のスタッフによって顧客情報の登録が行われた
    ''' </summary>
    ''' <remarks></remarks>
    Public Const AlreadyUpdatedCustomerInfo As Integer = 5004

    ''' <summary>
    ''' 顧客新規登録
    ''' </summary>
    ''' <param name="custDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客情報を新規登録する。</remarks>
    <EnableCommit()>
    Public Function InsertCustomer(ByVal custDataTbl As SC3080205DataSet.SC3080205CustDataTable, ByRef msgId As Integer) As Integer

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCustomer_Start")
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCustomer Step1")
        'ログ出力 End *****************************************************************************

        msgId = 0
        Dim ret As Integer = 1
        Dim custDataRow As SC3080205DataSet.SC3080205CustRow
        Dim context As StaffContext = StaffContext.Current
        Dim account As String = context.Account
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
        custDataRow = custDataTbl.Item(0)

        'ブランクを半角一文字スペースにする
        Call EditDataRow(custDataRow)

        Using da As New SC3080205TableAdapter
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            Dim birthday As Nullable(Of DateTime)
            Dim smsflg As Nullable(Of Integer)
            Dim actvctgryid As Nullable(Of Long)
            Dim resonid As Nullable(Of Long)
            If (Not custDataRow.IsBIRTHDAYNull) Then
                birthday = custDataRow.BIRTHDAY
            End If
            If (Not custDataRow.IsSMSFLGNull) Then
                smsflg = custDataRow.SMSFLG
            End If
            If (Not custDataRow.IsACTVCTGRYIDNull) Then
                actvctgryid = custDataRow.ACTVCTGRYID
            End If
            If (Not custDataRow.IsREASONIDNull) Then
                resonid = CType(custDataRow.REASONID, Long)
            End If
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

            '2013/06/30 TCS 庄 2013/10対応版 START
            '顧客シーケンス采番
            Dim seqno As Decimal
            seqno = da.GetNewcustseq()

            custDataRow.CSTID = CStr(seqno)
            '2013/06/30 TCS 庄 2013/10対応版 END

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCustomer custDataRow.CSTID = " + custDataRow.CSTID)
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCustomer Step2")
            'ログ出力 End *****************************************************************************

            '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
            '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
            '未取引客個人情報新規作成
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            ret = da.InsertNewcustomer(custDataRow.CSTID, _
                                        custDataRow.CUSTYPE, _
                                        custDataRow.EMPLOYEENAME, _
                                        custDataRow.EMPLOYEEDEPARTMENT, _
                                        custDataRow.EMPLOYEEPOSITION, _
                                        custDataRow.SOCIALID, _
                                        custDataRow.NAME, _
                                        custDataRow.FIRSTNAME, _
                                        custDataRow.MIDDLENAME, _
                                        custDataRow.LASTNAME, _
                                        custDataRow.NAMETITLE_CD, _
                                        custDataRow.NAMETITLE, _
                                        custDataRow.SEX, _
                                        custDataRow.ZIPCODE, _
                                        custDataRow.ADDRESS, _
                                        custDataRow.ADDRESS1, _
                                        custDataRow.ADDRESS2, _
                                        custDataRow.ADDRESS3, _
                                        custDataRow.ADDRESS_STATE, _
                                        custDataRow.ADDRESS_DISTRICT, _
                                        custDataRow.ADDRESS_CITY, _
                                        custDataRow.ADDRESS_LOCATION, _
                                        custDataRow.TELNO, _
                                        custDataRow.MOBILE, _
                                        custDataRow.FAXNO, _
                                        custDataRow.BUSINESSTELNO, _
                                        custDataRow.EMAIL1, _
                                        custDataRow.EMAIL2, _
                                        birthday, _
                                        custDataRow.PRIVATE_FLEET_ITEM_CD, _
                                        custDataRow.DOMICILE, _
                                        custDataRow.COUNTRY, _
                                        custDataRow.CST_INCOME, _
                                        custDataRow.DUMMYNAMEFLG, _
                                        custDataRow.UPDATEACCOUNT)
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
            '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END
            '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END

            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）START
            '未取引客個人情報(販売店)新規作成
            ret = SC3080205TableAdapter.InsertNewcustome_dlr(custDataRow.DLRCD, custDataRow.CSTID, custDataRow.COMMERCIAL_RECV_TYPE, account)
            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動）END

            '未取引客個人情報（車両）新規作成
            ret = SC3080205TableAdapter.InsertNewcustomer_vcl(custDataRow.DLRCD, custDataRow.CSTID, actvctgryid, resonid, _
                                                                custDataRow.AC_MODFFUNCDVS, custDataRow.STRCDSTAFF, custDataRow.STAFFCD, account) '自社客連番、販売店コード、AC変更機、活動除外理由ID

            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
            With custDataRow
                ret = SC3080205TableAdapter.InsertCustomerLocal(.CSTID, .CST_ORGNZ_CD, .CST_ORGNZ_INPUT_TYPE, .CST_ORGNZ_NAME, .CST_SUBCAT2_CD, account)
            End With
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

            '誘致最新化メソッド実行
            InsertAttPlanNew(custDataRow.CSTID)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCustomer_End")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 NED
            '更新： 2012/01/26 TCS 安田 【SALES_1B】来店実績更新
            If (ret <> 0) Then
                If (custDataRow.IsVISITSEQNull() = False) Then
                    Dim returnID As Integer = 0
                    Dim biz As New UpdateSalesVisitBusinessLogic
                    biz.UpdateVisitCustomerInfo(custDataRow.VISITSEQ, _
                                                CType(custDataRow.CUSTFLG, String), _
                                                custDataRow.CSTID, _
                                                custDataRow.STAFFCD, _
                                                "SC3080205", _
                                                returnID)
                    If (returnID = AlreadyUpdatedCustomerInfo) Then
                        Me.Rollback = True
                        Return returnID 'すでに他のユーザーが更新ずみ
                    End If
                End If
            End If
            '更新： 2012/01/26 TCS 安田 【SALES_1B】来店実績更新


            '更新に失敗していたらロールバック
            If ret = 0 Then
                Me.Rollback = True
                Return 0
            End If

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertCustomer Step3")
            'ログ出力 End *****************************************************************************

        End Using

        Return ret

    End Function

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
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        Dim directionFlg As String
        directionFlg = custDataTbl.Item(0).ADDRESS_DISP_DIRECTION
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        zipcode = zipcode.Replace("-", "")

        Using da As New SC3080205TableAdapter
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            'Return da.GetAddress(zipcode)
            Return da.GetAddress(zipcode, directionFlg)
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
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

    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 個人法人項目リスト取得
    ''' </summary>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>個人法人項目リストを取得する。</remarks>
    Public Shared Function GetPrivateFleetItem(ByRef msgId As Integer) As SC3080205DataSet.SC3080205PrivateFleetItemDataTable

        msgId = 0

        Dim privateFleetItemList As SC3080205DataSet.SC3080205PrivateFleetItemDataTable
        Using da As New SC3080205TableAdapter
            privateFleetItemList = da.GetPrivateFleetItem()
        End Using

        Return privateFleetItemList

    End Function

    ''' <summary>
    ''' 州リスト取得
    ''' </summary>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>州リストを取得する。</remarks>
    Public Shared Function GetState(ByRef msgId As Integer) As SC3080205DataSet.SC3080205StateDataTable

        msgId = 0
        Using da As New SC3080205TableAdapter
            Return da.GetState()
        End Using

    End Function

    ''' <summary>
    ''' 地域リスト取得
    ''' </summary>
    ''' <param name="state">州コード(インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>地域リストを取得する。</remarks>
    Public Shared Function GetDistrict(ByVal state As String, ByRef msgId As Integer) As SC3080205DataSet.SC3080205DistrictDataTable

        msgId = 0
        Using da As New SC3080205TableAdapter
            Return da.GetDistrict(state)
        End Using

    End Function

    ''' <summary>
    ''' 市リスト取得
    ''' </summary>
    ''' <param name="state">州コード(インプット)</param>
    ''' <param name="district">地域コード(インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>市リストを取得する。</remarks>
    Public Shared Function GetCity(ByVal state As String, ByVal district As String, ByRef msgId As Integer) As SC3080205DataSet.SC3080205CityDataTable

        msgId = 0
        Using da As New SC3080205TableAdapter
            Return da.GetCity(state, district)
        End Using

    End Function

    ''' <summary>
    ''' 地区リスト取得
    ''' </summary>
    ''' <param name="state">州コード(インプット)</param>
    ''' <param name="district">地域コード(インプット)</param>
    ''' <param name="city">市コード(インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>地区リストを取得する。</remarks>
    Public Shared Function GetLocation(ByVal state As String, ByVal district As String, ByVal city As String, ByRef msgId As Integer) As SC3080205DataSet.SC3080205LocationDataTable

        msgId = 0
        Using da As New SC3080205TableAdapter
            Return da.GetLocation(state, district, city)
        End Using

    End Function

    ''' <summary>
    ''' 入力項目表示設定リスト取得
    ''' </summary>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>入力項目表示設定リストを取得する。</remarks>
    Public Shared Function GetInputItemSetting(ByRef msgId As Integer) As SC3080205DataSet.SC3080205InputItemSettingDataTable

        msgId = 0

        Dim chkTiming As String
        chkTiming = "01"

        Using da As New SC3080205TableAdapter
            Return da.GetInputItemSetting(chkTiming)
        End Using

    End Function

    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
    Public Shared Function GetCustOrgnzLocal(ByVal custOrgnzNameHead As String, ByVal privateFleetItemCd As String) As SC3080205DataSet.SC3080205CustOrgnzLocalDataTable
        Using da As New SC3080205TableAdapter
            Return da.GetCustOrgnzLocal(custOrgnzNameHead, privateFleetItemCd)
        End Using
    End Function

    Public Shared Function GetCustOrgnzLocal(ByVal privateFleetItemCd As String) As SC3080205DataSet.SC3080205CustOrgnzLocalDataTable
        Using da As New SC3080205TableAdapter
            Return da.GetCustOrgnzLocal(privateFleetItemCd)
        End Using
    End Function

    Public Shared Function GetCustSubCtgry2(ByVal private_fleet_item_cd As String, ByVal cst_orgnz_cd As String) As SC3080205DataSet.SC3080205CustSubCtgry2DataTable
        Using da As New SC3080205TableAdapter
            Return da.GetCustSubCtgry2(private_fleet_item_cd, cst_orgnz_cd)
        End Using
    End Function
    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

    '2020/01/20 TS 岩田 TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045) START
    ''' <summary>
    ''' Aカード番号件数取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="cstid">顧客ID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>Aカード番号件数取得を取得します。</remarks>
    Public Shared Function GetAcardNumCount(ByVal dlrcd As String, ByVal cstid As String) As SC3080205DataSet.SC3080205AcardNumCountDataTable
        Return SC3080205TableAdapter.GetAcardNumCount(dlrcd, cstid)
    End Function
    '2020/01/20 TS 岩田 TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045) END

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
    ''' 空白を、半角スペース１文字に変換する (DBに空白の場合に、半角スペース１文字しとして出力されているため)
    ''' </summary>
    ''' <param name="val">値</param>
    ''' <returns>変換値</returns>
    ''' <remarks>空白を、半角スペース１文字に変換。</remarks>
    Protected Shared Function BlanckToSpaceTrim1(ByVal val As String) As String

        If (String.IsNullOrEmpty(Trim(val))) Then
            Return " "
        End If

        Return Trim(val)

    End Function
    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END


    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    ''' <summary>
    ''' データ行を更新用に編集する
    ''' </summary>
    ''' <param name="custDataRow">データ行</param>
    ''' <remarks>データ行を更新用に編集する。</remarks>
    Protected Shared Sub EditDataRow(ByVal custDataRow As SC3080205DataSet.SC3080205CustRow)

        '国民ID、免許証番号等
        custDataRow.SOCIALID = BlanckToSpaceTrim1(custDataRow.SOCIALID)

        '個人/法人区分
        custDataRow.CUSTYPE = BlanckToSpace1(custDataRow.CUSTYPE)        '顧客タイプ

        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        ''顧客氏名
        'custDataRow.NAME = BlanckToSpaceTrim1(custDataRow.NAME)
        'ファーストネーム
        custDataRow.FIRSTNAME = BlanckToSpaceTrim1(custDataRow.FIRSTNAME)
        'ミドルネーム
        custDataRow.MIDDLENAME = BlanckToSpaceTrim1(custDataRow.MIDDLENAME)
        'ラストネーム
        custDataRow.LASTNAME = BlanckToSpaceTrim1(custDataRow.LASTNAME)
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        '敬称コード
        custDataRow.NAMETITLE_CD = BlanckToSpace1(custDataRow.NAMETITLE_CD)

        '敬称
        custDataRow.NAMETITLE = BlanckToSpace1(custDataRow.NAMETITLE)

        '郵便番号
        custDataRow.ZIPCODE = BlanckToSpaceTrim1(custDataRow.ZIPCODE)

        '住所
        custDataRow.ADDRESS = BlanckToSpace1(custDataRow.ADDRESS)
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        '住所1
        custDataRow.ADDRESS1 = BlanckToSpace1(custDataRow.ADDRESS1)
        '住所2
        custDataRow.ADDRESS2 = BlanckToSpace1(custDataRow.ADDRESS2)
        '住所3
        custDataRow.ADDRESS3 = BlanckToSpace1(custDataRow.ADDRESS3)
        '住所(州)
        custDataRow.ADDRESS_STATE = BlanckToSpace1(custDataRow.ADDRESS_STATE)
        '住所(地域)
        custDataRow.ADDRESS_DISTRICT = BlanckToSpace1(custDataRow.ADDRESS_DISTRICT)
        '住所(市)
        custDataRow.ADDRESS_CITY = BlanckToSpace1(custDataRow.ADDRESS_CITY)
        '住所(地区)
        custDataRow.ADDRESS_LOCATION = BlanckToSpace1(custDataRow.ADDRESS_LOCATION)
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        '自宅電話番号
        custDataRow.TELNO = BlanckToSpaceTrim1(custDataRow.TELNO)

        '携帯電話番号
        custDataRow.MOBILE = BlanckToSpaceTrim1(custDataRow.MOBILE)

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

        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        '本籍
        custDataRow.DOMICILE = BlanckToSpace1(custDataRow.DOMICILE)
        '国籍
        custDataRow.COUNTRY = BlanckToSpace1(custDataRow.COUNTRY)
        '個人法人項目
        custDataRow.PRIVATE_FLEET_ITEM_CD = BlanckToSpace1(custDataRow.PRIVATE_FLEET_ITEM_CD)
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
        '年収
        custDataRow.CST_INCOME = BlanckToSpace1(custDataRow.CST_INCOME)
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END

        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 START
        custDataRow.CST_ORGNZ_CD = BlanckToSpace1(custDataRow.CST_ORGNZ_CD)
        custDataRow.CST_ORGNZ_INPUT_TYPE = BlanckToSpace1(custDataRow.CST_ORGNZ_INPUT_TYPE)
        custDataRow.CST_ORGNZ_NAME = BlanckToSpace1(custDataRow.CST_ORGNZ_NAME)
        custDataRow.CST_SUBCAT2_CD = BlanckToSpace1(custDataRow.CST_SUBCAT2_CD)
        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-1 END

    End Sub

    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    '2014/07/10 TCS 外崎 TMT要望（国民IDの必須制限解除）START
    ''' <summary>
    ''' 国民IDロジカルチェック処理
    ''' </summary>
    ''' <param name="privateFleetItem">個人法人項目</param>
    ''' <param name="socialId">国民ID</param>
    ''' <returns>メッセージID(チェックOK時は0を返却)</returns>
    ''' <remarks>国民IDのロジカルチェックを行う。</remarks>
    Protected Shared Function checkSocialId(ByVal privateFleetItem As String, ByVal socialId As String) As Integer

        '個人法人項目の値によってチェック内容を変える
        If (privateFleetItem = Thai) Then
            '01(Thai)
            '入力はなくてもよい、ある場合は13桁、数字のみであること、13桁チェックを通ること
            If (String.IsNullOrEmpty(socialId)) Then
                Return 0
            Else
                If socialId.Length = 13 Then
                    If System.Text.RegularExpressions.Regex.IsMatch(socialId, "^[0-9]+$") Then
                        Return checkSocialId13(socialId)
                    Else
                        Return 40975
                    End If
                Else
                    Return 40975
                End If
            End If
        ElseIf (privateFleetItem = Foreigner) Then
            '02(Foreigner)
            '入力はなくてもよい、ある場合は英数字のみであること
            If (String.IsNullOrEmpty(socialId)) Then
                Return 0
            Else
                If System.Text.RegularExpressions.Regex.IsMatch(socialId, "^[0-9a-zA-Z]+$") Then
                    Return 0
                Else
                    Return 40975
                End If
            End If
        ElseIf (privateFleetItem = Company) Then
            '03(Company)
            '入力はなくてもよい、ある場合は10桁ないし13桁、数字のみであること、10桁ないし13桁チェックを通ること
            If (String.IsNullOrEmpty(socialId)) Then
                Return 0
            Else
                If socialId.Length = 10 Then
                    If System.Text.RegularExpressions.Regex.IsMatch(socialId, "^[0-9]+$") Then
                        Return checkSocialId10(socialId)
                    Else
                        Return 40975
                    End If
                ElseIf socialId.Length = 13 Then
                    If System.Text.RegularExpressions.Regex.IsMatch(socialId, "^[0-9]+$") Then
                        Return checkSocialId13(socialId)
                    Else
                        Return 40975
                    End If
                Else
                    Return 40975
                End If
            End If
        ElseIf (privateFleetItem = GovtOrg) Then
            '04(Govt Org)
            '入力はなくてもよい、ある場合は英数字のみであること
            If (String.IsNullOrEmpty(socialId)) Then
                Return 0
            Else
                If System.Text.RegularExpressions.Regex.IsMatch(socialId, "^[0-9a-zA-Z]+$") Then
                    Return 0
                Else
                    Return 40975
                End If
            End If
        Else
            'その他(未選択など)
            'チェックしない
            Return 0
        End If

    End Function
    '2014/07/10 TCS 外崎 TMT要望（国民IDの必須制限解除）END

    ''' <summary>
    ''' 国民IDチェック(13桁)
    ''' </summary>
    ''' <param name="socialId">国民ID</param>
    ''' <returns>メッセージID(チェックOK時は0を返却)</returns>
    ''' <remarks>国民IDのロジカルチェックを行う。</remarks>
    Protected Shared Function checkSocialId13(ByVal socialId As String) As Integer

        Dim multiplier As Integer() = New Integer(11) {13, 12, 11, 10, 9, 8, 7, 6, 5, 4, 3, 2}
        Dim sum As Integer = 0
        Dim chkNum As Integer = 0

        '①　各桁の数字×各桁に対応する係数を掛ける
        '②　①の値を合計する
        For n = 0 To 11
            sum = sum + (CInt(Val(socialId.Chars(n))) * multiplier(n))
        Next
        '③　②の値を11で割った余りを求める
        '④　11-③を求める
        chkNum = 11 - (sum Mod 11)
        '⑤　④の値の後ろ1桁が国民IDの最終桁と等しいか？
        If (chkNum Mod 10) = CInt(Val(socialId.Chars(12))) Then
            Return 0
        Else
            Return 40975
        End If

    End Function

    ''' <summary>
    ''' 国民IDチェック(10桁)
    ''' </summary>
    ''' <param name="socialId">国民ID</param>
    ''' <returns>メッセージID(チェックOK時は0を返却)</returns>
    ''' <remarks>国民IDのロジカルチェックを行う。</remarks>
    Protected Shared Function checkSocialId10(ByVal socialId As String) As Integer

        Dim multiplier As Integer() = New Integer(8) {3, 1, 3, 1, 3, 1, 3, 1, 3}
        Dim sum As Integer = 0
        Dim chkNum As Integer = 0

        '①　各桁の数字×各桁に対応する係数を掛ける
        '②　①の値を合計する
        For n = 0 To 8
            sum = sum + (CInt(Val(socialId.Chars(n))) * multiplier(n))
        Next
        '③　②の値を10で割った余りを求める
        '④　10-③を求める
        chkNum = 10 - (sum Mod 10)
        '⑤　④の値の後ろ1桁が国民IDの最終桁と等しいか？
        If (chkNum Mod 10) = CInt(Val(socialId.Chars(9))) Then
            Return 0
        Else
            Return 40975
        End If

    End Function
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

#End Region

End Class
