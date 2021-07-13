Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports System.Text
Imports System.Reflection.MethodBase
Imports System.Globalization


'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3070201BusinessLogic.vb
'─────────────────────────────────────
'機能： 見積情報取得IF処理
'補足： 
'作成： 
'更新： 2012/07/30 TCS 高橋  兆方店展開
'更新： 2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応
'更新： 2013/06/30 TCS 趙   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/06 TCS 森    Aカード情報相互連携開発
'更新： 2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応
'更新： 2018/05/01 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
'更新： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────



Public Class IC3070201BusinessLogic
    Inherits BaseBusinessComponent




#Region "定数"

#Region "終了コード"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>正常終了</remarks>
    Private Const NOMAL As Integer = 0
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>見積管理IDが未設定</remarks>
    Private Const ERR_EstimateIdIsNull As Integer = 2021
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>見積管理IDが数値以外</remarks>
    Private Const ERR_EstimateIdIsNotNumeric As Integer = 3021
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>見積管理IDが10桁以上</remarks>
    Private Const ERR_EstimateIdSizeOver As Integer = 4021
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>実行モードが未設定</remarks>
    Private Const ERR_ModeIsNull As Integer = 2011
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>実行モードが不正値</remarks>
    Private Const ERR_ModeIsNotCorrect As Integer = 5011
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>対象見積情報無し</remarks>
    Private Const ERR_EstInfoNothing As Integer = 6001
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks>システムエラー</remarks>
    Private Const ERR_SysErr As Integer = 9999

#End Region

    ''' <summary>
    ''' メソッド名
    ''' </summary>
    ''' <remarks>ログ出力用(メソッド名)</remarks>
    Private Const METHODNAME As String = "GetEstimationInfo "

    ''' <summary>
    ''' 開始ログ    
    ''' </summary>
    ''' <remarks>ログ出力用(開始)</remarks>
    Private Const STARTLOG As String = "START "

    ''' <summary>
    ''' 終了ログ
    ''' </summary>
    ''' <remarks>ログ出力用(終了)</remarks>
    Private Const ENDLOG As String = "END "

    '更新： 2013/12/06 TCS 森    Aカード情報相互連携開発 START
    ''' <summary>
    ''' 外装飾コード：先頭の空白除去して渡す処理実行フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENVSETTINGKEY_USE_UNTRIMMED_COLOR_CD As String = "USE_UNTRIMMED_COLOR_CD"

    ''' <summary>
    ''' 外装飾コードが４桁の場合のみ空白を除去する
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EXTCOLORCD_CONV_LENGTH As Integer = 4
    '更新： 2013/12/06 TCS 森    Aカード情報相互連携開発 END

    '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 START

    ''' <summary>
    ''' 基幹コード区分:店舗(2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_CODE_TYPE_BRANCH As String = "2"

    ''' <summary>
    ''' プログラム設定検索条件（プログラムコード）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_PROGRAM_SETTING_PROGRAM_CD As String = "IC3070201"

    ''' <summary>
    ''' プログラム設定検索条件（設定セクション）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_PROGRAM_SETTING_SETTING_SECTION As String = "IC3070201"

    ''' <summary>
    ''' プログラム設定検索条件（設定キー）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_PROGRAM_SETTING_SETTING_KEY As String = "DMS_CODE_MAP_BRN_CD"

    ''' <summary>
    ''' 使用基幹コード(基幹コード2)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_CODE_MAP_DMS_CD_2 As String = "DMS_CD_2"

    ''' <summary>
    ''' 使用基幹コード(基幹コード3)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_DMS_CODE_MAP_DMS_CD_3 As String = "DMS_CD_3"

    '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 END

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
    Private Const C_USE_CUSTOMERDATA_CLEANSING_FLG As String = "USE_CUSTOMERDATA_CLEANSING_FLG"
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

#End Region

#Region "メンバ変数"
    ''' <summary>
    ''' 終了コード
    ''' </summary>
    ''' <remarks></remarks>
    Private resultId_ As Integer
#End Region

#Region "プロパティ"
    ''' <summary>
    ''' 終了コード
    ''' </summary>
    ''' <value>終了コード</value>
    ''' <returns>終了コード</returns>
    ''' <remarks>0の場合は正常、それ以外の場合エラー</remarks>
    Public Property ResultId As Integer
        Get
            Return resultId_
        End Get
        Set(value As Integer)
            resultId_ = value
        End Set
    End Property
#End Region

#Region "コンストラクタ"
    ''' <summary>
    ''' 初期化処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        resultId_ = 0
    End Sub
#End Region

#Region "001.見積情報取得"
    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 001.見積情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <param name="mode">実行モード 0:見積全情報取得、1:見積車両情報のみ取得、2:見積全情報取得＋顧客Tblより情報取得</param>
    ''' <param name="changemode">TCVフラグ 0:ＴＣＶ以外、1:ＴＣＶ</param>
    ''' <returns>IC3070201DataSet</returns>
    ''' <remarks>見積管理IDを条件に見積情報の取得を行う</remarks>
    Public Function GetEstimationInfo(ByVal estimateId As Long, _
                                      ByVal mode As Integer, _
                                      ByVal changemode As Integer) As IC3070201DataSet
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
        'デバッグログ(開始)
        '開始ログ出力
        Dim startLogInfo As New StringBuilder
        startLogInfo.Append(METHODNAME)
        startLogInfo.Append(STARTLOG)
        Logger.Info(startLogInfo.ToString())

        '結果返却用DataSet作成
        Using retIC3070201DataSet As New IC3070201DataSet

            retIC3070201DataSet.Tables.Clear()

            ' -----------------------------------------------
            ' -- 入力チェック
            ' -----------------------------------------------

            '見積管理IDチェック
            If (IsNothing(estimateId)) Then
                '見積管理IDが未設定
                ResultId = ERR_EstimateIdIsNull

                'ログ出力
                Logger.Error("ResultId : " & CType(ResultId, String))

                Return retIC3070201DataSet

            ElseIf Not Validation.IsHankakuNumber(CType(estimateId, String)) Then
                '見積管理IDが半角数値以外
                ResultId = ERR_EstimateIdIsNotNumeric

                'ログ出力
                Logger.Error("ResultId : " & CType(ResultId, String))

                Return retIC3070201DataSet

            ElseIf CType(estimateId, String).Length > 10 Then
                '見積管理IDが10桁以上
                ResultId = ERR_EstimateIdSizeOver

                'ログ出力
                Logger.Error("ResultId : " & CType(ResultId, String))

                Return retIC3070201DataSet

            End If

            '実行モードチェック
            If (IsNothing(mode)) Then
                '実行モードが未設定
                ResultId = ERR_ModeIsNull

                'ログ出力
                Logger.Error("ResultId : " & CType(ResultId, String))

                Return retIC3070201DataSet

            ElseIf Not ((mode.Equals(0) Or mode.Equals(1) Or mode.Equals(2))) Then
                '実行モードが不正値
                ResultId = ERR_ModeIsNotCorrect

                'ログ出力
                Logger.Error("ResultId : " & CType(ResultId, String))

                Return retIC3070201DataSet

            End If


            ' -----------------------------------------------
            ' -- 見積情報取得処理
            ' -----------------------------------------------

            '取得データ格納用DataTable作成
            Dim retESTIMATIONINFODataTbl As IC3070201DataSet.IC3070201EstimationInfoDataTable = Nothing
            Dim retEST_VCLOPTIONINFODataTbl As IC3070201DataSet.IC3070201VclOptionInfoDataTable = Nothing
            Dim retEST_CUSTOMERINFODataTbl As IC3070201DataSet.IC3070201CustomerInfoDataTable = Nothing
            Dim retEST_CHARGEINFODataTbl As IC3070201DataSet.IC3070201ChargeInfoDataTable = Nothing
            Dim retEST_PAYMENTINFODataTbl As IC3070201DataSet.IC3070201PaymentInfoDataTable = Nothing
            Dim retEST_TRADEINCARINFODataTbl As IC3070201DataSet.IC3070201TradeincarInfoDataTable = Nothing
            Dim retEST_INSURANCEINFODataTbl As IC3070201DataSet.IC3070201EstInsuranceInfoDataTable = Nothing
            ' 2013/12/06 TCS 森    Aカード情報相互連携開発 START
            Dim retEST_CustomerInfoDetailTbl As IC3070201DataSet.IC3070201CustomerInfoDetailDataTable = Nothing
            Dim retEST_UserInfo As IC3070201DataSet.IC3070201UsersDataTable = Nothing
            Dim dtDmsCd As IC3070201DataSet.IC3070201DmsCdDataTable = Nothing
            Dim retEst_Picture As IC3070201DataSet.IC3070201PictureDataTable = Nothing
            ' 2013/12/06 TCS 森    Aカード情報相互連携開発 END

            ' 見積情報登録処理
            Dim adapter As New IC3070201TableAdapter(mode)

            Try
                '見積情報取得
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                retESTIMATIONINFODataTbl = adapter.GetEstimationInfoDataTable(estimateId, changemode)
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START
                ' TCV時に、外装飾コード：先頭の空白除去して渡す処理実行
                Me.ChangeExtColor(changemode, retESTIMATIONINFODataTbl)
                ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END

                '2012/07/30 TCS 高橋  兆方店展開 START
                Dim dlrcd As String = ""
                If retESTIMATIONINFODataTbl IsNot Nothing AndAlso retESTIMATIONINFODataTbl.Rows.Count > 0 Then
                    Dim dr As IC3070201DataSet.IC3070201EstimationInfoRow = CType(retESTIMATIONINFODataTbl.Rows(0), IC3070201DataSet.IC3070201EstimationInfoRow)
                    dlrcd = dr.DLRCD
                End If
                '2012/07/30 TCS 高橋  兆方店展開 END


                '見積車両オプション情報取得
                retEST_VCLOPTIONINFODataTbl = adapter.GetVclOptionInfoDataTable(estimateId)

                '実行モードが0or2の場合、見積顧客/見積諸費用/見積支払方法/見積下取車両の情報も取得する
                If mode.Equals(0) Or mode.Equals(2) Then

                    '見積顧客情報取得
                    retEST_CUSTOMERINFODataTbl = adapter.GetCustomerInfoDataTable(estimateId)

                    '見積諸費用情報取得
                    retEST_CHARGEINFODataTbl = adapter.GetChargeInfoDataTable(estimateId)

                    '2012/07/30 TCS 高橋  兆方店展開 START
                    '見積支払方法情報取得
                    'retEST_PAYMENTINFODataTbl = adapter.GetPaymentInfoDataTable(estimateId)
                    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                    retEST_PAYMENTINFODataTbl = adapter.GetPaymentInfoDataTable(CStr(estimateId))
                    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
                    '2012/07/30 TCS 高橋  兆方店展開 END

                    '見積下取車両情報取得
                    retEST_TRADEINCARINFODataTbl = adapter.GetTradeincarInfoDataTable(estimateId)

                    '見積保険情報取得
                    retEST_INSURANCEINFODataTbl = adapter.GetInsuranceInfoDataTable(estimateId)

                    ' 2013/12/06 TCS 森    Aカード情報相互連携開発 START
                End If

                If Not retESTIMATIONINFODataTbl.Count() = 0 Then
                    '実行モードが2の場合、顧客情報も取得する
                    If mode.Equals(2) Then
                        Dim cstid As String = String.Empty
                        If retESTIMATIONINFODataTbl.Item(0).IsCRCUSTIDNull = False Then
                            cstid = retESTIMATIONINFODataTbl.Item(0).CRCUSTID
                        End If
                        '顧客情報取得
                        retEST_CustomerInfoDetailTbl = adapter.GetCustomerInfoDetailDataTable(retESTIMATIONINFODataTbl.Item(0).DLRCD, cstid)

                        Dim slsPicStfCd As String = String.Empty
                        If Not retEST_CustomerInfoDetailTbl.Count() = 0 AndAlso _
                            retEST_CustomerInfoDetailTbl.Item(0).IsSLS_PIC_STF_CDNull = False Then
                            slsPicStfCd = retEST_CustomerInfoDetailTbl.Item(0).SLS_PIC_STF_CD

                            'CST_OCCUPATION_IDを基幹コードマップで変換
                            retEST_CustomerInfoDetailTbl.Item(0).CST_OCCUPATION_ID = _
                                adapter.GetDmsCdOccupationId(retEST_CustomerInfoDetailTbl.Item(0).CST_OCCUPATION_ID)
                        End If
                        '2018/05/02 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
                        'お客様情報クレンジング機能使用可否フラグの取得

                        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
                        Dim CleansingIsAvailable As Boolean = False
                        Dim systemBiz As New SystemSetting
                        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
                        dataRow = systemBiz.GetSystemSetting(C_USE_CUSTOMERDATA_CLEANSING_FLG)

                        If (dataRow Is Nothing) Then
                            CleansingIsAvailable = False
                        Else
                            If (dataRow.SETTING_VAL.Equals("1")) Then
                                CleansingIsAvailable = True
                            Else
                                CleansingIsAvailable = False
                            End If
                        End If
                        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

                        If Not retEST_CustomerInfoDetailTbl.Count() = 0 Then
                            If CleansingIsAvailable And "1".Equals(retEST_CustomerInfoDetailTbl.Item(0).FLEET_FLG) Then
                                'クレンジング機能が使用可の場合で、かつ、個人法人区分が”1”(法人)の場合

                                retEST_CustomerInfoDetailTbl.Item(0).FIRST_NAME = retEST_CustomerInfoDetailTbl.Item(0).CST_NAME
                                retEST_CustomerInfoDetailTbl.Item(0).MIDDLE_NAME = String.Empty
                                retEST_CustomerInfoDetailTbl.Item(0).LAST_NAME = String.Empty
                            Else
                                '個人法人区分が”1”(法人)以外の場合

                                Dim firstName As String = retEST_CustomerInfoDetailTbl.Item(0).FIRST_NAME
                                Dim middleName As String = retEST_CustomerInfoDetailTbl.Item(0).MIDDLE_NAME
                                Dim lastName As String = retEST_CustomerInfoDetailTbl.Item(0).LAST_NAME
                                Dim nameFML As String = firstName + middleName + lastName
                                Dim name As String = retEST_CustomerInfoDetailTbl.Item(0).CST_NAME

                                '顧客氏名の半角スペースを除去
                                If (String.Empty.Equals(name) = False) Then
                                    name = Replace(name, " ", "")
                                End If

                                'ファーストネーム＋ミドルネーム＋ラストネームの半角スペースを除去
                                If (String.Empty.Equals(nameFML) = False) Then
                                    nameFML = Replace(nameFML, " ", "")
                                End If

                                'ファーストネーム＋ミドルネーム＋ラストネームと氏名が等しくない場合
                                If (Not name.Equals(nameFML)) Then
                                    '顧客氏名を半角スペースで分割
                                    Dim names As String() = retEST_CustomerInfoDetailTbl.Item(0).CST_NAME.Split(New Char() {" "c})

                                    'ファーストネーム
                                    If (names.Length >= 1) Then
                                        retEST_CustomerInfoDetailTbl.Item(0).FIRST_NAME = names(0)
                                    Else
                                        retEST_CustomerInfoDetailTbl.Item(0).FIRST_NAME = String.Empty
                                    End If

                                    'ミドルネーム
                                    If (names.Length >= 2) Then
                                        retEST_CustomerInfoDetailTbl.Item(0).MIDDLE_NAME = names(1)
                                    Else
                                        retEST_CustomerInfoDetailTbl.Item(0).MIDDLE_NAME = String.Empty
                                    End If

                                    'ラストネーム
                                    If (names.Length >= 3) Then
                                        Dim sb As New System.Text.StringBuilder
                                        sb.Append(names(2))
                                        For i = 3 To names.Length - 1
                                            sb.Append(New Char() {" "c})
                                            sb.Append(names(i))
                                        Next
                                        retEST_CustomerInfoDetailTbl.Item(0).LAST_NAME = sb.ToString()
                                    Else
                                        retEST_CustomerInfoDetailTbl.Item(0).LAST_NAME = String.Empty
                                    End If
                                End If
                            End If
                        End If
                        '2018/05/02 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
                        ' ユーザ名取得
                        retEST_UserInfo = adapter.GetUser(slsPicStfCd)
                    End If


                    Dim strCd As String = String.Empty
                    If retESTIMATIONINFODataTbl.Item(0).IsSTRCDNull = False Then
                        strCd = retESTIMATIONINFODataTbl.Item(0).STRCD
                    ElseIf retESTIMATIONINFODataTbl.Item(0).IsCNT_STRCDNull = False Then
                        strCd = retESTIMATIONINFODataTbl.Item(0).CNT_STRCD
                    End If

                    '基幹コード取得
                    '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 START
                    Dim dmsCodeMap As New DmsCodeMap
                    Dim drDmsCodeMap As DmsCodeMapDataSet.DMSCODEMAPRow = dmsCodeMap.GetDmsCodeMap(C_DMS_CODE_TYPE_BRANCH,
                                                                                                   Trim(retESTIMATIONINFODataTbl.Item(0).DLRCD),
                                                                                                   Trim(strCd))

                    '空のDataTableを用意
                    dtDmsCd = New IC3070201DataSet.IC3070201DmsCdDataTable

                    If Not drDmsCodeMap Is Nothing Then
                        'プログラム設定より使用するDMS店舗コードを選択
                        Dim programSettingV4 As New ProgramSettingV4
                        Dim drProgramSettingV4 As ProgramSettingV4DataSet.PROGRAMSETTINGV4Row = _
                                                 programSettingV4.GetProgramSettingV4(C_DMS_PROGRAM_SETTING_PROGRAM_CD,
                                                                                      C_DMS_PROGRAM_SETTING_SETTING_SECTION,
                                                                                      C_DMS_PROGRAM_SETTING_SETTING_KEY)

                        Dim strBrnCd As String = String.Empty
                        If Not drProgramSettingV4 Is Nothing Then
                            Dim strProgramSettingV4Val As String = drProgramSettingV4.SETTING_VAL
                            If C_DMS_CODE_MAP_DMS_CD_3.Equals(strProgramSettingV4Val) Then
                                strBrnCd = drDmsCodeMap.DMS_CD_3
                            Else
                                strBrnCd = drDmsCodeMap.DMS_CD_2
                            End If
                        Else
                            'プログラム設定が取得できなかった場合は基幹コード2を取得
                            strBrnCd = drDmsCodeMap.DMS_CD_2
                        End If

                        'DMS店舗コードをDataTableへ設定
                        Dim drDmsCd As IC3070201DataSet.IC3070201DmsCdRow = CType(dtDmsCd.NewRow, IC3070201DataSet.IC3070201DmsCdRow)
                        drDmsCd.DMS_CD_1 = drDmsCodeMap.DMS_CD_1
                        drDmsCd.DMS_CD_2 = strBrnCd
                        dtDmsCd.Rows.Add(drDmsCd)

                    End If

                    '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 END

                    '画像URL取得
                    retEst_Picture = adapter.GetPicture(retESTIMATIONINFODataTbl.Item(0).SERIESCD)

                    '型式画像取得
                    Dim retEst_KatashikiPicture As IC3070201DataSet.IC3070201KatashikiPictureDataTable = _
                            adapter.GetPiGetKatashikiPicturecture(retESTIMATIONINFODataTbl.Item(0).MODELCD, retESTIMATIONINFODataTbl.Item(0).EXTCOLORCD)

                    '型式画像が取得できた場合は、型式画像を使用する。（注意：モデルマスタに登録されていない場合を考慮しない）
                    If ((retEst_KatashikiPicture.Rows.Count() > 0) And (retEst_Picture.Rows.Count() > 0)) Then
                        retEst_Picture.Item(0).MODEL_PICTURE = retEst_KatashikiPicture.Item(0).VCL_PICTURE
                    End If

                End If

                ' 2013/12/06 TCS 森    Aカード情報相互連携開発 END

            Catch oex As OracleExceptionEx
                ResultId = ERR_SysErr
                Logger.Error("ResultId : " & CType(ERR_SysErr, String), oex)

                Throw
            Finally
                adapter = Nothing
            End Try

            '取得データテーブルをデータセットに格納
            retIC3070201DataSet.Tables.Add(retESTIMATIONINFODataTbl)
            retIC3070201DataSet.Tables.Add(retEST_VCLOPTIONINFODataTbl)

            '実行モードが0の場合
            If mode.Equals(0) Or mode.Equals(2) Then
                retIC3070201DataSet.Tables.Add(retEST_CUSTOMERINFODataTbl)
                retIC3070201DataSet.Tables.Add(retEST_CHARGEINFODataTbl)
                retIC3070201DataSet.Tables.Add(retEST_PAYMENTINFODataTbl)
                retIC3070201DataSet.Tables.Add(retEST_TRADEINCARINFODataTbl)
                retIC3070201DataSet.Tables.Add(retEST_INSURANCEINFODataTbl)
                ' 2013/12/06 TCS 森    Aカード情報相互連携開発 START
            End If

            If Not retESTIMATIONINFODataTbl.Count() = 0 Then
                If mode.Equals(2) Then
                    retIC3070201DataSet.Tables.Add(retEST_CustomerInfoDetailTbl)
                    retIC3070201DataSet.Tables.Add(retEST_UserInfo)
                End If

                retIC3070201DataSet.Tables.Add(dtDmsCd)
                retIC3070201DataSet.Tables.Add(retEst_Picture)
            End If

            ' 2013/12/06 TCS 森    Aカード情報相互連携開発 END

            '見積情報の取得件数を確認
            If retESTIMATIONINFODataTbl.Rows.Count.Equals(1) Then
                '正常終了
                ResultId = NOMAL
            Else
                '対象データが無し
                ResultId = ERR_EstInfoNothing
                Logger.Error("ResultId : " & CType(ResultId, String))
            End If


            'デバッグログ(終了)
            '終了ログ出力
            Dim endLogInfo As New StringBuilder
            endLogInfo.Append(METHODNAME)
            endLogInfo.Append(ENDLOG)
            Logger.Info(endLogInfo.ToString())

            Return retIC3070201DataSet

        End Using

    End Function


#End Region


    '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 支払い総額取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>Double</returns>
    ''' <remarks>見積管理IDに紐付く支払い総額を取得する</remarks>
    ''' <History>
    '''   2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応
    ''' </History>
    Public Function GetTotalPrice(ByVal estimateId As Long, ByVal changemode As Integer) As Double
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
        Dim ds As IC3070201DataSet
        Dim vclInfo As IC3070201DataSet.IC3070201EstimationInfoDataTable
        Dim vclOptionInfo As IC3070201DataSet.IC3070201VclOptionInfoDataTable
        Dim vclChargeInfo As IC3070201DataSet.IC3070201ChargeInfoDataTable
        Dim vclTradeCarInfo As IC3070201DataSet.IC3070201TradeincarInfoDataTable
        Dim vclInsuranceInfo As IC3070201DataSet.IC3070201EstInsuranceInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "Start {0}", GetCurrentMethod.Name), True)

        '2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応 START
        '※未使用となった為、削除
        'Dim dlrcd As String '販売店コード
        '2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応 END

        Dim basePrice As Double = 0 '車両本体価格
        Dim exteriorPrice As Double = 0 '外装追加費用
        Dim interiorPrice As Double = 0 '内装追加費用
        Dim discountPrice As Double = 0 '値引き額
        Dim optionPrice As Double = 0 'オプション合計額
        Dim chargePrice As Double = 0 '諸費用
        Dim insurancePrice As Double = 0 '保険費用
        Dim tradeCarPrice As Double = 0 '下取り合計額
        Dim totalPrice As Double = 0 '支払い総額

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
        ds = Me.GetEstimationInfo(estimateId, 0, changemode)
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
        vclInfo = DirectCast(ds.Tables("IC3070201EstimationInfo"), IC3070201DataSet.IC3070201EstimationInfoDataTable)
        vclOptionInfo = DirectCast(ds.Tables("IC3070201VclOptionInfo"), IC3070201DataSet.IC3070201VclOptionInfoDataTable)
        vclChargeInfo = DirectCast(ds.Tables("IC3070201ChargeInfo"), IC3070201DataSet.IC3070201ChargeInfoDataTable)
        vclTradeCarInfo = DirectCast(ds.Tables("IC3070201TradeincarInfo"), IC3070201DataSet.IC3070201TradeincarInfoDataTable)
        vclInsuranceInfo = DirectCast(ds.Tables("IC3070201EstInsuranceInfo"), IC3070201DataSet.IC3070201EstInsuranceInfoDataTable)

        '車両価格取得
        With vclInfo(0)
            '2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応 START
            '※未使用となった為、削除
            'dlrcd = .DLRCD
            '2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応 END

            basePrice = .BASEPRICE
            exteriorPrice = .EXTAMOUNT
            interiorPrice = .INTAMOUNT
            If .IsDISCOUNTPRICENull = False Then
                discountPrice = .DISCOUNTPRICE
            End If
        End With

        'オプション価格合計額取得
        For Each row In vclOptionInfo
            If row.IsINSTALLCOSTNull Then
                optionPrice = optionPrice + row.PRICE
            Else
                optionPrice = optionPrice + row.PRICE + row.INSTALLCOST
            End If
        Next

        '諸費用取得
        For Each row In vclChargeInfo
            If row.IsPRICENull = False Then
                chargePrice = chargePrice + row.PRICE
            End If
        Next

        '2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応 START
        ''諸費用データが存在しない場合は、諸費用を計算する(TCVで見積作成に遷移し、保存しなかった場合)
        'If vclChargeInfo.Count = 0 Then
        '    chargePrice = GetChargeInfo(dlrcd, estimateId, basePrice, discountPrice, exteriorPrice, interiorPrice)
        'End If
        '2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応 END

        '保険費用取得
        For Each row In vclInsuranceInfo
            If row.IsAMOUNTNull = False Then
                insurancePrice = insurancePrice + row.AMOUNT
            End If
        Next

        '下取り合計額取得
        For Each row In vclTradeCarInfo
            tradeCarPrice = tradeCarPrice + row.ASSESSEDPRICE
        Next


        totalPrice = basePrice + exteriorPrice + interiorPrice + optionPrice + chargePrice + insurancePrice - discountPrice - tradeCarPrice


        vclInfo.Dispose()
        vclOptionInfo.Dispose()
        vclChargeInfo.Dispose()
        vclTradeCarInfo.Dispose()
        vclInsuranceInfo.Dispose()


        Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0}", GetCurrentMethod.Name), True)

        Return totalPrice
    End Function

    '2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応 START
    ' ''' <summary>
    ' ''' 諸費用取得
    ' ''' </summary>
    ' ''' <param name="dlrcd">販売店コード</param>
    ' ''' <param name="estimateId">見積管理ID</param>
    ' ''' <param name="basePrice">車両本体価格</param>
    ' ''' <param name="discountPrice">値引き額</param>
    ' ''' <param name="exteriorPrice">外装追加費用</param>
    ' ''' <param name="interiorPrice">内装追加費用</param>
    ' ''' <returns>Double</returns>
    ' ''' <remarks>見積管理IDに諸費用を取得する(諸費用が登録されてない場合用)
    ' ''' 　　　　　TCVから遷移した初回は、諸費用データが作成されない為、
    ' ''' 　　　　　当メソッドにて、諸費用の計算を行う
    ' ''' </remarks>
    'Private Function GetChargeInfo(ByVal dlrcd As String,
    '                               ByVal estimateId As Long,
    '                               ByVal basePrice As Double,
    '                               ByVal discountPrice As Double,
    '                               ByVal exteriorPrice As Double,
    '                               ByVal interiorPrice As Double) As Double

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "Start {0}", GetCurrentMethod.Name), True)

    '    Dim sysEnv As New SystemEnvSetting
    '    Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
    '    Dim adapter As New IC3070201TableAdapter(0)
    '    Dim vclPurchaseTax As IC3070201DataSet.IC3070201VclPurchaseTaxDataTable
    '    Dim vclPrice As Double '車両価格
    '    Dim vclTaxRatio As Double = 0 '車両購入税率
    '    Dim vclAddTaxRatio As Double = 0 '増値税
    '    Dim vclPurchaseMinTax As Double = 0 '車両購入税（最低価格）
    '    Dim chargePrice As Double = 0 '諸費用

    '    With sysEnv
    '        '車両購入税率を取得する
    '        sysEnvRow = .GetSystemEnvSetting("EST_VCLTAX_RATIO")
    '        If IsNothing(sysEnvRow) = False Then
    '            vclTaxRatio = Double.Parse(sysEnvRow.PARAMVALUE, CultureInfo.InvariantCulture)
    '        End If
    '        '増値税を取得する
    '        sysEnvRow = .GetSystemEnvSetting("EST_VCLADDTAX_RATIO")
    '        If IsNothing(sysEnvRow) = False Then
    '            vclAddTaxRatio = Double.Parse(sysEnvRow.PARAMVALUE, CultureInfo.InvariantCulture)
    '        End If
    '    End With

    '    '車両購入税（最低価格）を取得する

    '    vclPurchaseTax = adapter.GetPurchaseMinimumTax(dlrcd, estimateId)
    '    If vclPurchaseTax.Count = 1 Then
    '        vclPurchaseMinTax = vclPurchaseTax(0).MINIMUMPRICE
    '    End If

    '    '車両価格を求める(車両本体価格　＋　外装色追加費用　＋　内装色追加費用　－　値引き額)
    '    vclPrice = basePrice + exteriorPrice + interiorPrice - discountPrice

    '    '増値税反映
    '    If vclAddTaxRatio <> 0 Then
    '        chargePrice = vclPrice / vclAddTaxRatio
    '    End If

    '    '車両購入税率反映
    '    If vclTaxRatio <> 0 Then
    '        chargePrice = chargePrice * (vclTaxRatio / 100)
    '    End If

    '    '車両購入税が最低価格を下回った場合は、最低価格を車両購入税とする
    '    If vclPurchaseMinTax > chargePrice Then
    '        chargePrice = vclPurchaseMinTax
    '    End If


    '    vclPurchaseTax.Dispose()

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "End {0}", GetCurrentMethod.Name), True)

    '    '少数点以下を切り捨て
    '    Return Math.Floor(chargePrice)
    'End Function
    '2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応 END
    '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END

    ''' <summary>
    ''' 通知依頼取得
    ''' </summary>
    ''' <param name="noticeReqId">通知依頼ID</param>
    ''' <returns>通知依頼</returns>
    Public Function GetNoticeRequest(ByVal noticeReqId As Long) As IC3070201DataSet.IC3070201NoticeRequestRow
        Dim adapter As New IC3070201TableAdapter(0)
        Return adapter.GetNoticeRequest(noticeReqId)(0)
    End Function


    ' 2013/12/12 TCS 森 Aカード情報相互連携開発 START

    ''' <summary>
    ''' システム設定値を取得する
    ''' </summary>
    ''' <param name="sysEnvName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSysEnvSettingValue(ByVal sysEnvName As String) As String
        Dim dr As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        Dim env As SystemEnvSetting = Nothing

        env = New SystemEnvSetting()
        dr = env.GetSystemEnvSetting(sysEnvName)
        If Not dr Is Nothing Then
            Return dr.PARAMVALUE.Trim()
        End If

        Return String.Empty

    End Function

    ''' <summary>
    ''' TCV時に、外装飾コード：先頭の空白除去して渡す処理実行
    ''' </summary>
    ''' <param name="changemode">TCVフラグ 0:ＴＣＶ以外、1:ＴＣＶ</param>
    ''' <param name="estimageTbl">IC3070201DataSet.IC3070201EstimationInfoDataTable</param>
    ''' <remarks>見積管理IDを条件に見積情報の取得を行う</remarks>
    Public Sub ChangeExtColor(ByVal changemode As Integer,
                                ByVal estimageTbl As IC3070201DataSet.IC3070201EstimationInfoDataTable)

        'TCV時(changemode <> 0)に、設定がONの場合に先頭の空白を除去する
        Logger.Info("ChangeEXTCOLORCD1 changemode = " & changemode)
        Dim sysEnvVal As String = GetSysEnvSettingValue(ENVSETTINGKEY_USE_UNTRIMMED_COLOR_CD)
        If ((changemode <> 0) AndAlso "1".Equals(sysEnvVal)) Then
            '桁数が４桁で先頭が空白の場合のみ処理する
            If estimageTbl IsNot Nothing AndAlso
                    estimageTbl.Rows.Count > 0 AndAlso
                    estimageTbl.Item(0).EXTCOLORCD.Length = EXTCOLORCD_CONV_LENGTH Then

                Dim startChar As String
                startChar = estimageTbl.Item(0).EXTCOLORCD.Substring(0, 1)

                If (startChar.Equals(" ")) Then
                    Logger.Info("estimageTbl.Item(0).EXTCOLORCD Before = " & estimageTbl.Item(0).EXTCOLORCD)
                    estimageTbl.Item(0).EXTCOLORCD = estimageTbl.Item(0).EXTCOLORCD.Substring(1, (EXTCOLORCD_CONV_LENGTH - 1))
                    Logger.Info("estimageTbl.Item(0).EXTCOLORCD After = " & estimageTbl.Item(0).EXTCOLORCD)
                End If
            End If
        End If

    End Sub

    ' 2013/12/12 TCS 森 Aカード情報相互連携開発 END

End Class
