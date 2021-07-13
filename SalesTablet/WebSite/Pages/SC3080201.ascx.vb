'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080201.ascx.vb
'─────────────────────────────────────
'機能： 顧客詳細
'補足： 
'作成： 2011/11/24 TCS 山口
'更新： 2012/01/26 TCS 安田      【SALES_1B】来店実績より表示処理
'更新： 2012/03/08 TCS 河原      【SALES_1B】コールバック時の文字列のエンコード処理追加
'更新： 2012/03/28 TCS 高橋      【SALES_2】自社客 国民ID変更不可
'更新： 2012/03/28 TCS 高橋      【SALES_2】自社客 チェックボックス入力制御(性別,個人法人)
'更新： 2012/04/12 TCS 安田      【SALES_2】営業キャンセルでシステムエラー（号口課題 No.111）
'更新： 2012/04/12 TCS 安田      【SALES_2】来店から車両登録が不可（ユーザー課題 No.49）
'更新： 2012/04/12 TCS 安田      【SALES_2】重要連絡の”接待日”と”日付”の間に-を入れる（ユーザー課題 No.37）
'更新： 2012/04/19 TCS 河原      【SALES_2】顔写真パス変更対応
'更新： 2012/04/26 TCS 河原      HTMLエンコード対応
'更新： 2012/04/27 TCS 安田      HTMLエンコード対応
'更新： 2012/06/01 TCS 河原      FS開発
'更新： 2012/08/23 TCS 山口      【A STEP2】次世代e-CRB 新車商談機能改善
'更新： 2013/01/22 TCS 河原      【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/06/30 TCS 内藤      【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/10/23 TCS 山田      次世代e-CRBセールス機能 新DB適応に向けた機能開発
'更新： 2013/11/27 TCS 市川,各務 Aカード情報相互連携開発
'更新： 2014/02/12 TCS 高橋      受注後フォロー機能開発
'更新： 2014/02/20 TCS 松月      【A STEP2】電話番号検索不具合対応（TR-V4-GTMC140219005）
'更新： 2014/03/18 TCS 市川      新PF開発No.41（商業情報活動)
'更新： 2014/04/01 TCS 松月      【A STEP2】TMT不具合対応
'更新： 2014/05/07 TCS 市川      PCとタブレットで顧客入力情報不一致対応(CHG-354)
'更新： 2014/06/20 TCS 市川      【A STEP2】電話番号検索不具合対応（TR-V4-GTMC140617004）の横転
'更新： 2014/08/01 TCS 外崎      性能改善(ポップアップ高速化対応)
'更新： 2014/08/28 TCS 外崎      TMT NextStep2 UAT-BTS D-117
'更新： 2014/10/02 TCS 河原      商業情報必須指定解除のタブレット側対応
'更新： 2014/11/21 TCS 河原      TMT B案
'更新： 2015/04/02 TCS 外崎      セールスタブレット:M014
'更新： 2015/04/10 TCS 外崎      タブレットSPM操作性機能向上（活動履歴表示）
'更新： 2015/06/08 TCS 中村      TMT課題対応(#2)
'更新： 2015/07/06 TCS 藤井      TR-V4-FTMS141028001(FTMS→TMTマージ)
'更新： 2016/11/28 TCS 曽出      （トライ店システム評価）基幹連携に伴う顧客車両情報管理機能評価　【TR-V4-TMT-20160623-001】
'更新： 2017/11/20 TCS 河原      TKM独自機能開発
'更新： 2018/04/24 TCS 前田      （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
'更新： 2018/06/18 TCS 前田       TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2018/08/27 TCS 佐々木    TKM Next Gen e-CRB Project Application development Block B-3
'更新： 2018/11/22 TCS 三浦      活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001)
'更新： 2018/12/13 TCS 中村(拓)  TKM SIT-0170
'更新： 2019/01/23 TS  三浦      UAT-0659
'更新： 2019/04/08 TS  舩橋      POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える
'更新： 2019/05/28 TS 髙橋(龍) 画像形式（拡張子）変更対応(TR-SVT-TMT-20170725-001)
'更新： 2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究
'更新： 2020/01/20 TS  岩田      TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045)
'更新： 2020/02/20 TS  河原      TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072)
'─────────────────────────────────────
Option Explicit On
Option Strict On

Imports System.Data
Imports System.Globalization

Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core

Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.CustomerInfo.Details.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.CommonUtility.DataAccess
Imports Toyota.eCRB.CommonUtility.BizLogic

Partial Class Pages_SC3080201
    Inherits System.Web.UI.UserControl
    Implements ICallbackEventHandler, ISC3080201Control

#Region "顧客編集、車両編集"

#Region "定数"

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary> セッションキー CONTACTROWVERSION</summary>
    Public Const SESSION_KEY_CONTACTROWVERSION As String = "SearchKey.SESSION_KEY_CONTACTROWVERSION"
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    ''' <summary> セッションキー 車両登録No</summary>
    Public Const SESSION_KEY_VCLREGNO As String = "SearchKey.VCLREGNO"

    '2012/01/26 TCS 安田 【SALES_1B】来店実績より表示処理 START
    ''' <summary> セッションキー 来店実績連番</summary>
    Private Const SESSION_KEY_VISITSEQ As String = "SearchKey.VISITSEQ"

    ''' <summary> セッションキー 顧客名称</summary>
    Public Const SESSION_KEY_TENTATIVENAME As String = "SearchKey.TENTATIVENAME"

    '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) START
    ''' <summary> セッションキー 電話番号</summary>
    Private Const SESSION_KEY_TELNO As String = "SearchKey.TELNO"
    '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) END

    ''' <summary> セッションキー 車両登録No初期表示フラグ (1:車両登録Noを表示する)</summary>
    Public Const SESSION_KEY_VCLREGNODISPFLG As String = "SearchKey.VCLREGNODISPFLG"
    '2012/01/26 TCS 安田 【SALES_1B】来店実績より表示処理 END

    ''' <summary>選択車両ID</summary>
    Private Const SESSION_KEY_SELECT_VCLID As String = "select_vclid"

    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    ''' <summary> セッションキー 来店実績の来店人数</summary>
    Private Const SESSION_KEY_WALKINNUM As String = "SearchKey.WALKINNUM"
    ''' <summary>
    ''' 検索文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SERCHSTRING As String = "searchString"
    ''' <summary>
    ''' 検索タイプ (1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SERCHTYPE As String = "searchType"
    ''' <summary>
    ''' 検索方向 (1:前方一致、2:あいまい検索、3:完全一致)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SERCHDIRECTION As String = "searchDirection"
    ''' <summary>
    ''' 電話番号検索フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_SERCHFLG As String = "searchTelFlg"
    ''' <summary>
    ''' 国民ID、免許証番号等
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_SOCIALID As String = "customerEdit.SOCIALID"
    ''' <summary>
    ''' 個人/法人区分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_CUSTYPE As String = "customerEdit.CUSTYPE"
    ''' <summary>
    ''' 顧客氏名
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SESSION_KEY_CUSTEDIT_NAME As String = "customerEdit.NAME"
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>
    ''' ファーストネーム
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SESSION_KEY_CUSTEDIT_FIRSTNAME As String = "customerEdit.FIRSTNAME"
    ''' <summary>
    ''' ミドルネーム
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SESSION_KEY_CUSTEDIT_MIDDLENAME As String = "customerEdit.MIDDLENAME"
    ''' <summary>
    ''' ラストネーム
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SESSION_KEY_CUSTEDIT_LASTNAME As String = "customerEdit.LASTNAME"
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
    ''' <summary>
    ''' 敬称コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_NAMETITLE_CD As String = "customerEdit.NAMETITLE_CD"
    ''' <summary>
    ''' 敬称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_NAMETITLE As String = "customerEdit.NAMETITLE"
    ''' <summary>
    ''' 郵便番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_ZIPCODE As String = "customerEdit.ZIPCODE"
    ''' <summary>
    ''' 住所
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_ADDRESS As String = "customerEdit.ADDRESS"
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 住所1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_ADDRESS1 As String = "customerEdit.ADDRESS1"
    ''' <summary>
    ''' 住所2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_ADDRESS2 As String = "customerEdit.ADDRESS2"
    ''' <summary>
    ''' 住所3
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_ADDRESS3 As String = "customerEdit.ADDRESS3"
    ''' <summary>
    ''' 住所(州)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_STATE As String = "customerEdit.STATE"
    ''' <summary>
    ''' 住所(地域)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_DISTRICT As String = "customerEdit.DISTRICT"
    ''' <summary>
    ''' 住所(市)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_CITY As String = "customerEdit.CITY"
    ''' <summary>
    ''' 住所(地区)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_LOCATION As String = "customerEdit.LOCATION"
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
    ''' <summary>
    ''' 自宅電話番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_TELNO As String = "customerEdit.TELNO"
    ''' <summary>
    ''' 携帯電話番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_MOBILE As String = "customerEdit.MOBILE"
    ''' <summary>
    ''' FAX番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_FAXNO As String = "customerEdit.FAXNO"
    ''' <summary>
    ''' 勤務地電話番号
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_BUSINESSTELNO As String = "customerEdit.BUSINESSTELNO"
    ''' <summary>
    ''' E-mailアドレス１
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_EMAIL1 As String = "customerEdit.EMAIL1"
    ''' <summary>
    ''' E-mailアドレス２
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_EMAIL2 As String = "customerEdit.EMAIL2"
    ''' <summary>
    ''' 性別
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_SEX As String = "customerEdit.SEX"
    ''' <summary>
    ''' 生年月日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_BIRTHDAY As String = "customerEdit.BIRTHDAY"
    ''' <summary>
    ''' SMS配信可否
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_SMSFLG As String = "customerEdit.SMSFLG"
    ''' <summary>
    ''' e-mail配信可否
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_EMAILFLG As String = "customerEdit.EMAILFLG"
    ''' <summary>
    ''' 担当者氏名（法人）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_EMPLOYEENAME As String = "customerEdit.EMPLOYEENAME"
    ''' <summary>
    ''' 担当者部署名（法人）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_EMPLOYEEDEPARTMENT As String = "customerEdit.EMPLOYEEDEPARTMENT"
    ''' <summary>
    ''' 役職（法人）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_EMPLOYEEPOSITION As String = "customerEdit.EMPLOYEEPOSITION"
    ''' <summary>
    ''' 活動区分ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_ACTVCTGRYID As String = "customerEdit.ACTVCTGRYID"
    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
    ''' <summary>
    ''' 商業情報受取区分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_COMMRECVTYPE As String = "customerEdit.COMMERCIAL_RECV_TYPE"
    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END
    '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
    Private Const SESSION_KEY_CUSTEDIT_INCOME As String = "customerEdit.INCOME"
    '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
    ''' <summary>
    ''' 活動除外理由ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_REASONID As String = "customerEdit.REASONID"
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 本籍
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_DOMICILE As String = "customerEdit.DOMICILE"
    ''' <summary>
    ''' 国籍
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_COUNTRY As String = "customerEdit.COUNTRY"
    ''' <summary>
    ''' 個人法人項目コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_PRIVATE_FLEET_ITEM_CD As String = "customerEdit.PRIVATE_FLEET_ITEM_CD"
    ''' <summary>
    ''' 行ロックバージョン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_LOCKVERSION As String = "customerEdit.LOCKVERSION"
    ''' <summary>
    ''' 行ロックバージョン(車両)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_VCLLOCKVERSION As String = "customerEdit.VCLLOCKVERSION"
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
    '2014/06/20 TCS 市川  【A STEP2】電話番号検索不具合対応（TR-V4-GTMC140617004）の横転 START
    ''' <summary>
    ''' 行ロックバージョン(販売店顧客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTEDIT_CSTDLRLOCKVERSION As String = "customerEdit.CST_DLR_ROW_LOCK_VERSION"
    '2014/06/20 TCS 市川  【A STEP2】電話番号検索不具合対応（TR-V4-GTMC140617004）の横転 END

    '2018/12/13 TCS 中村(拓) TKM SIT-0170 START
    Private Const SESSION_KEY_CUSTEDIT_CSTORGNZCD As String = "customerEdit.CST_ORGNZ_CD"
    Private Const SESSION_KEY_CUSTEDIT_CSTORGNZNAME As String = "customerEdit.CST_ORGNZ_NAME"
    Private Const SESSION_KEY_CUSTEDIT_CSTORGNZINPUTTYPE As String = "customerEdit.CST_ORGNZ_INPUT_TYPE"
    Private Const SESSION_KEY_CUSTEDIT_CSTSUBCAT2CD As String = "customerEdit.CST_SUBCAT2_CD"
    ' 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える START DEL
    ' 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える END
    Private Const SESSION_KEY_CUSTEDIT_CSTLOCALLOCKVERSION As String = "customerEdit.CST_LOCAL_ROW_LOCK_VERSION"
    '2018/12/13 TCS 中村(拓) TKM SIT-0170 END

    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
    ''' <summary>
    ''' 商業情報受取区分:未設定
    ''' </summary>
    ''' <remarks>既定値</remarks>
    Private Const COMMERCIAL_RECV_TYPE_EMPTY As String = " "
    ''' <summary>
    ''' 商業情報受取区分:受け取る
    ''' </summary>
    ''' <remarks></remarks>
    Private Const COMMERCIAL_RECV_TYPE_YES As String = "1"
    ''' <summary>
    ''' 商業情報受取区分:受け取らない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const COMMERCIAL_RECV_TYPE_NO As String = "0"
    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END
#End Region

#Region " メソット 車両編集"

    Private Property seqnoHidden As Object

    ''' <summary>
    ''' 車両編集-初期処理。
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>2012/04/12 TCS 安田 【SALES_2】営業キャンセルでシステムエラー（号口課題 No.111）</History>
    Protected Sub VehicleInitialize()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("VehicleInitialize Start")
        'ログ出力 End *****************************************************************************

        '前画面からセッション情報を取得
        Dim vehicleDataTbl As New SC3080206DataSet.SC3080206VehicleDataTable
        Dim vehicleDataRow As SC3080206DataSet.SC3080206VehicleRow
        vehicleDataRow = vehicleDataTbl.NewSC3080206VehicleRow
        vehicleDataTbl.Rows.Add(vehicleDataRow)       '追加する

        '表示キー設定
        If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SELECT_VCLID) = True) Then
            SetCarSeq()
        End If

        'セッション内の値をセットする
        Call SetSessionValue(vehicleDataRow)

        '初期表示用フラグ情報取得
        Dim msgID As Integer = 0
        SC3080206BusinessLogic.GetInitializeFlg(vehicleDataTbl, msgID)

        '自社客/未取引客フラグ 
        Me.custFlgHidden.Value = CStr(vehicleDataRow.CUSTFLG)

        'モデル未入力エラー
        Me.vehicleNoModelErrMsg.Value = WebWordUtility.GetWord(50912)

        '2012/08/23 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 START
        Me.DefaultMaker.Value = WebWordUtility.GetWord(50700)
        '2012/08/23 TCS 河原 【A STEP2】次世代e-CRB 新車商談機能改善 END

        '画面項目更新
        SetDisplayItem(vehicleDataRow.CUSTFLG)

        'モード (０：新規登録モード、１：編集モード)
        Dim mode As Integer = CType(Me.editVehicleModeHidden.Value, Integer)

        If (mode = SC3080206BusinessLogic.ModeEdit) Then

            '１：編集モード
            '初期表示情報取得
            vehicleDataTbl = SC3080206BusinessLogic.GetInitialize(vehicleDataTbl, msgID)

            '※１　表示・非表示判定
            '中古車認証区分使用フラグ（０：非表示、１：表示）
            'CPO区分
            If (vehicleDataRow.ASSURANFLG = SC3080206BusinessLogic.NoneDisplay) Then
                Me.cpoDisplayFlgPanel.Visible = False
            End If

            'テレマ使用可否フラグ（０：非表示、１：表示）
            'テレマ契約の有無
            '利用開始日
            '接続方法
            '緊急連絡先１
            '緊急連絡先２
            '緊急連絡先３
            '契約満了日
            'G-BOOK配信可否
            If (vehicleDataRow.TELEMAFLG = SC3080206BusinessLogic.NoneDisplay) Then
                Me.telemaDisplayFlgPanel.Visible = False
            End If

            '画面の値を設定する
            Call SetDisplayValues(vehicleDataTbl)
        Else
            '2012/01/26 TCS 安田 【SALES_1B】来店実績より表示処理 START
            '車両登録No
            Dim vclregno As String = String.Empty
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VCLREGNODISPFLG) = True) Then

                'セッション情報 車両登録No初期表示フラグ (1:車両登録Noを表示する)
                Dim regDispFlg As String = String.Empty
                '更新： 2012/04/12 TCS 安田 【SALES_2】営業キャンセルでシステムエラー（号口課題 No.111） START
                regDispFlg = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_VCLREGNODISPFLG, False), String)
                '更新： 2012/04/12 TCS 安田 【SALES_2】営業キャンセルでシステムエラー（号口課題 No.111） END

                If (regDispFlg.Equals(SC3080206BusinessLogic.RegNoDispBtn) = True) Then
                    If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VCLREGNO) = True) Then '念のため存在確認する
                        vclregno = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_VCLREGNO, False), String)
                    End If
                    Me.vehiclePopUpAutoOpenFlg.Value = SC3080206BusinessLogic.VehicleOpenFlg
                End If

            End If
            '2012/01/26 TCS 安田 【SALES_1B】来店実績より表示処理 END

            Me.vclregnoTextBox.Text = vclregno

            '変更前情報の保持
            Me.editVehicleModeBackHidden.Value = Me.editVehicleModeHidden.Value
        End If


        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        '活動区分リストのセット
        Dim wordList As New List(Of String)
        wordList.Add(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay1))
        wordList.Add(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay2))
        wordList.Add(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay3))
        wordList.Add(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay4))

        '活動区分リストのセット
        Dim dtActvctgry As New SC3080205DataSet.SC3080205ActvctgryDataTable
        dtActvctgry.Rows.Add(New Object() {SC3080205BusinessLogic.ActvctgryidDisplay1, wordList.Item(0)})
        dtActvctgry.Rows.Add(New Object() {SC3080205BusinessLogic.ActvctgryidDisplay2, wordList.Item(1)})
        dtActvctgry.Rows.Add(New Object() {SC3080205BusinessLogic.ActvctgryidDisplay3, wordList.Item(2)})
        dtActvctgry.Rows.Add(New Object() {SC3080205BusinessLogic.ActvctgryidDisplay4, wordList.Item(3)})

        With actvctgryRepeater
            .DataSource = dtActvctgry
            .DataBind()
        End With

        '断念理由リストのセット
        Dim giveupReasonTbl As SC3080205DataSet.SC3080205OmitreasonDataTable = _
            SC3080205BusinessLogic.GetGiveupReason(msgID)

        With reasonRepeater
            .DataSource = giveupReasonTbl
            .DataBind()
        End With

        '2012/04/27 TCS 安田 【SALES_2】HTMLEncode対応 START

        '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        '年式リストのセット
        Dim modelYearTbl As SC3080206DataSet.SC3080206ModelYearDataTable = SC3080206BusinessLogic.GetModelYear()

        If modelYearTbl.Count > 0 Then
            With modelYearRepeater
                .DataSource = modelYearTbl
                .DataBind()
            End With
        End If
        '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END 

        'カスタムコントロール設定
        If telemaDisplayFlgPanel.Visible = True Then
            Dim gbookCheckBoxScript As String = "$('#gbookCheckButton').CheckMark({ ""label"": """ & JavaScriptStringEncode(50039) & """, ""checkIconPosition"": ""left"", ""offIconUrl"": """", ""onIconUrl"": """" });"
            If gbookCheckButton.Enabled = False Then
                gbookCheckBoxScript = gbookCheckBoxScript & "$('#gbookCheckButton').CheckMark('disabled', true);"
            End If
            JavaScriptUtility.RegisterStartupScript(Me.Page, " <script>" & "$(function() {" & gbookCheckBoxScript & "});" & "</script>", "after")
        End If
        JavaScriptUtility.RegisterStartupScript(Me.Page, " <script>" & "$(function() { $('#actvctgryLabel2').CustomLabel({ ""useEllipsis"": ""true"" }); });" & "</script>", "after3")

        '2012/04/27 TCS 安田 【SALES_2】HTMLEncode対応 END
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("VehicleInitialize End")
        'ログ出力 End *****************************************************************************

    End Sub

    ''' <summary>
    ''' Hidden項目に車両のキー情報を設定
    ''' </summary>
    ''' <remarks>Hidden項目に車両のキー情報を設定</remarks>
    Protected Sub SetCarSeq()
        '自社客/未取引客フラグ (０：自社客、１：未取引客)
        Dim custflg As Short = Nothing
        '編集モード時のみ、未取引客となる場合がある
        custflg = CType(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), Short)
        If (custflg = SC3080206BusinessLogic.OrgCustflg) Then
            Me.selectVinHidden.Value = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, False), String) 'VIN
            Me.selectSeqnoHidden.Value = String.Empty 'SEQNO
        Else
            Me.selectVinHidden.Value = String.Empty 'VIN
            Me.selectSeqnoHidden.Value = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, False), String) 'SEQNO
        End If
    End Sub

    ''' <summary>
    ''' 自社客・未取引客に応じて画面項目の表示を変更
    ''' </summary>
    ''' <param name="CustFlg">自社客/未取引客フラグ</param>
    ''' <remarks></remarks>
    Protected Sub SetDisplayItem(ByVal custFlg As Integer)
        If (custFlg = SC3080206BusinessLogic.OrgCustflg) Then
            '０：自社客
            '※２　入力可・不可判定
            'AC以外の項目は、入力不可にする
            Me.makerTextBox.Enabled = False
            Me.modelTextBox.Enabled = False
            Me.gradeTextBox.Enabled = False
            Me.bdyclrTextBox.Enabled = False
            Me.vclregnoTextBox.Enabled = False
            Me.vinTextBox.Enabled = False
            Me.fueldvsTextBox.Enabled = False
            Me.enginenoTextBox.Enabled = False
            Me.baseTypeTextBox.Enabled = False
            Me.vclregdateTextBox.Enabled = False
            Me.vcldelidateDateTime.Enabled = False
            Me.vcldelidateTextBox.Enabled = False
            Me.registdateTextBox.Enabled = False
            Me.newvcldvsTextBox.Enabled = False
            Me.cponmTextBox.Enabled = False
            Me.mileageTextBox.Enabled = False
            Me.systemidTextBox.Enabled = False
            Me.regstatusTextBox.Enabled = False
            Me.contractstatusTextBox.Enabled = False
            Me.connectdvsTextBox.Enabled = False
            Me.contractstartdateTextBox.Enabled = False
            Me.contractenddateTextBox.Enabled = False
            Me.telematelnumber1TextBox.Enabled = False
            Me.telematelnumber2TextBox.Enabled = False
            Me.telematelnumber3TextBox.Enabled = False
            Me.vcldelidateDateTime.Visible = False
        Else
            Me.orgcustPanel0.Visible = False
            Me.orgcustPanel1.Visible = False
            Me.orgcustPanel2.Visible = False
            Me.orgcustPanel3.Visible = False
            Me.orgcustPanel4.Visible = False
            Me.vcldelidateTextBox.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' 車両編集-完了ボタンクリック時。
    ''' </summary>
    ''' <remarks></remarks>
    ''' <History>2012/04/12 TCS 安田 【SALES_2】来店から車両登録が不可（ユーザー課題 No.49）</History>
    Protected Function VehicleEditKanryo() As Integer

        Dim ret As Integer = 0
        Dim msgID As Integer = 0
        Dim bizClass As New SC3080206BusinessLogic
        Using vehicleDataTbl As New SC3080206DataSet.SC3080206VehicleDataTable

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("VehicleEditKanryo Start")
            'ログ出力 End *****************************************************************************

            Dim vehicleDataRow As SC3080206DataSet.SC3080206VehicleRow

            '自社客/未取引客フラグ (０：自社客、１：未取引客)
            Dim custflg As Short = Nothing
            '編集モード時のみ、未取引客となる場合がある
            custflg = CType(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), Short)

            '表示キー設定
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SELECT_VCLID) = True) Then
                If (custflg = SC3080206BusinessLogic.OrgCustflg) Then
                    Me.selectVinHidden.Value = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, False), String) 'VIN
                    Me.selectSeqnoHidden.Value = String.Empty  'SEQNO
                Else
                    Me.selectVinHidden.Value = String.Empty 'VIN
                    Me.selectSeqnoHidden.Value = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, False), String) 'SEQNO
                End If

            End If

            '画面の値を取得する
            Me.GetDisplayValues(vehicleDataTbl)
            vehicleDataRow = vehicleDataTbl.Item(0)

            'モード (０：新規登録モード、１：編集モード)
            Dim mode As Integer = Nothing
            mode = CType(Me.editVehicleModeHidden.Value, Integer)

            '未顧客のみ入力チェックをする
            If (custflg = SC3080206BusinessLogic.NewCustflg) Then

                'バリデーション判定
                If (SC3080206BusinessLogic.CheckValidation(vehicleDataTbl, msgID) = False) Then
                    'エラーメッセージを表示
                    Return msgID
                End If

            End If

            'セッションの値をDataRowにセットする
            Me.SetSessionValue(vehicleDataRow)

            If (mode = SC3080206BusinessLogic.ModeCreate) Then

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("VehicleEditKanryo InsertVehicle Start")
                'ログ出力 End *****************************************************************************

                '０：新規登録モード
                '車両新規登録
                ret = bizClass.InsertVehicle(vehicleDataTbl, msgID)
                'VIN：自社客 / 車両シーケンスNo.：未取引客
                'Me.SetValue(ScreenPos.Next, SESSION_KEY_VCLID, vehicleDataRow.SEQNO)
                'If (ret <= 0) Then
                '    '登録処理に失敗しました。
                '    'エラーメッセージを表示
                '    ShowMessageBox(914)
                '    Exit Function
                'End If

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("VehicleEditKanryo InsertVehicle End")
                'ログ出力 End *****************************************************************************

            Else
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("VehicleEditKanryo UpdateVehicle Start")
                'ログ出力 End *****************************************************************************

                '１：編集モード
                '車両更新
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                Dim actEditFlg As Integer
                If (Trim(Me.reasonidHidden.Value) <> Trim(Me.reasonidHidden_Old.Value) Or Me.actvctgryidHidden.Value <> Me.actvctgryidHidden_Old.Value) Then
                    actEditFlg = 1
                Else
                    actEditFlg = 0
                End If
                ret = bizClass.UpdateVehicle(vehicleDataTbl, msgID, actEditFlg)
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End

                '2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
                If (ret <= 0) Then
                    msgID = 901
                    Return msgID
                End If
                '2013/06/30 TCS 吉村 2013/10対応版　既存流用 END

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("VehicleEditKanryo UpdateVehicle End")
                'ログ出力 End *****************************************************************************

            End If

            '2012/04/12 TCS 安田 【SALES_2】来店から車両登録が不可（ユーザー課題 No.49） START
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VCLREGNODISPFLG) = True) Then
                RemoveValueBypass(ScreenPos.Current, SESSION_KEY_VCLREGNODISPFLG)
            End If
            '2012/04/12 TCS 安田 【SALES_2】来店から車両登録が不可（ユーザー課題 No.49） END

            '選択行情報保持
            If (custflg = SC3080206BusinessLogic.OrgCustflg) Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, CStr(vehicleDataRow.VIN))
            Else
                Me.SetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, CStr(vehicleDataRow.SEQNO))
            End If

            ' 車両編集－変更前情報の保持
            Call SetVehicleBackHidden()

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            'ポップアップを囲うPanelをVisible=Trueに
            Me.CustomerCarEditVisiblePanel.Visible = True
            Me.NameListActvctgryReasonListVisiblePanel.Visible = True
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("VehicleEditKanryo End")
            'ログ出力 End *****************************************************************************

            Return 0

        End Using

    End Function

    'セッションの値をDataRowにセットする
    Protected Sub SetSessionValue(ByVal vehicleDataRow As SC3080206DataSet.SC3080206VehicleRow)

        'モード (０：新規登録モード、１：編集モード)
        Dim mode As Integer = CType(Me.editVehicleModeHidden.Value, Integer)
        '自社客/未取引客フラグ (０：自社客、１：未取引客)
        Dim custflg As Short = CType(SC3080205BusinessLogic.NewCustFlg, Short)
        If (mode = SC3080205BusinessLogic.ModeEdit) Then
            '編集モード時のみ、未取引客となる場合がある
            custflg = CType(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), Short)
        End If

        '自社客連番/未取引客ユーザID
        Dim originalid As String = Nothing
        originalid = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
        'VIN
        Dim vin As String = Me.selectVinHidden.Value


        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current

        Dim dlrcd As String = context.DlrCD         '自身の販売店コード
        Dim strcd As String = context.BrnCD         '自身の店舗コード
        Dim account As String = context.Account     '自身のアカウント

        'セッション情報のセット
        vehicleDataRow.CUSTFLG = custflg
        If (vehicleDataRow.CUSTFLG = SC3080206BusinessLogic.OrgCustflg) Then

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetSessionValue originalid = " + originalid)
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetSessionValue vin = " + vin)
            'ログ出力 End *****************************************************************************

            '０：自社客
            '2013/06/30 TCS 未 2013/10対応版　既存流用 START
            vehicleDataRow.ORIGINALID = CDec(originalid)
            '2013/06/30 TCS 未 2013/10対応版　既存流用 END
            'VIN
            vehicleDataRow.VIN = vin

        Else
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetSessionValue originalid = " + originalid)
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetSessionValue SEQNO = " + Me.selectSeqnoHidden.Value)
            'ログ出力 End *****************************************************************************

            '１：未取引客
            '2013/06/30 TCS 未 2013/10対応版　既存流用 START
            vehicleDataRow.CSTID = CDec(originalid)
            '2013/06/30 TCS 未 2013/10対応版　既存流用 END

            '編集モード
            If (mode = SC3080206BusinessLogic.ModeEdit) Then
                'SEQNO
                vehicleDataRow.SEQNO = CType(Me.selectSeqnoHidden.Value, Long)
            End If

        End If

        '販売店コード
        vehicleDataRow.DLRCD = dlrcd
        '店舗コード
        vehicleDataRow.STRCD = strcd
        'AC変更アカウント
        vehicleDataRow.AC_MODFACCOUNT = account

    End Sub

    '画面の値を取得する
    Protected Sub GetDisplayValues(ByVal vehicleDataTbl As SC3080206DataSet.SC3080206VehicleDataTable)

        Dim vehicleDataRow As SC3080206DataSet.SC3080206VehicleRow = vehicleDataTbl.NewSC3080206VehicleRow

        'セッション内の値をセットする
        Me.SetSessionValue(vehicleDataRow)

        vehicleDataRow.MAKERNAME = Me.makerTextBox.Text             'メーカー
        vehicleDataRow.SERIESNM = Me.modelTextBox.Text            'モデル
        vehicleDataRow.VCLREGNO = Me.vclregnoTextBox.Text           '車両登録No.
        vehicleDataRow.VIN = Me.vinTextBox.Text                     'VIN
        If (Not String.IsNullOrEmpty(Me.selectSeqnoHidden.Value)) Then
            vehicleDataRow.SEQNO = CLng(Me.selectSeqnoHidden.Value)       'SEQNO
        End If
        If (Not IsNothing(Me.vcldelidateDateTime.Value)) Then
            vehicleDataRow.VCLDELIDATE = Me.vcldelidateDateTime.Value.Value     '納車日
        End If

        '活動区分ID
        If (String.IsNullOrEmpty(Me.actvctgryidHidden.Value)) Then
            vehicleDataRow.SetACTVCTGRYIDNull()
        Else
            vehicleDataRow.ACTVCTGRYID = CType(Me.actvctgryidHidden.Value, Integer)
        End If
        '活動区分変更アカウント
        'vehicleDataRow.AC_MODFACCOUNT = " "
        '活動区分変更機能
        vehicleDataRow.AC_MODFFUNCDVS = SC3080206BusinessLogic.ACModffuncdvsValue
        '活動除外理由ID
        If (String.IsNullOrEmpty(Me.reasonidHidden.Value)) Then
            vehicleDataRow.SetREASONIDNull()
        Else
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
            vehicleDataRow.REASONID = Me.reasonidHidden.Value
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        End If

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        If (vehicleDataRow.CUSTFLG = SC3080206BusinessLogic.OrgCustflg) Then
            If (String.IsNullOrEmpty(Me.cstvcllcverHidden.Value)) Then
                vehicleDataRow.CSTVCLLCVER = 0
            Else
                vehicleDataRow.CSTVCLLCVER = CType(Me.cstvcllcverHidden.Value, Long)
            End If
            If (String.IsNullOrEmpty(Me.cstvclidHidden.Value)) Then
                vehicleDataRow.VCLID = 0
            Else
                vehicleDataRow.VCLID = CType(Me.cstvclidHidden.Value, Decimal)
            End If
        Else
            If (String.IsNullOrEmpty(Me.vcllcverHidden.Value)) Then
                vehicleDataRow.VCLLCVER = 0
            Else
                vehicleDataRow.VCLLCVER = CType(Me.vcllcverHidden.Value, Long)
            End If
            If (String.IsNullOrEmpty(Me.vcldlrlcverHidden.Value)) Then
                vehicleDataRow.VCLDLRLCVER = 0
            Else
                vehicleDataRow.VCLDLRLCVER = CType(Me.vcldlrlcverHidden.Value, Long)
            End If
        End If
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

        '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        '走行距離
        If (String.IsNullOrEmpty(Me.vclMileTextBox.Text)) Then
            vehicleDataRow.VCL_MILE = String.Empty
        Else
            vehicleDataRow.VCL_MILE = Me.vclMileTextBox.Text
        End If
        '年式
        If (String.IsNullOrEmpty(Me.modelYearHidden.Value)) Then
            vehicleDataRow.MODEL_YEAR = String.Empty
        Else
            vehicleDataRow.MODEL_YEAR = Me.modelYearHidden.Value
        End If
        '
        If (String.IsNullOrEmpty(Me.lcVcldlrlcverHidden.Value)) Then
            vehicleDataRow.LC_VCLDLRLCVER = 0
        Else
            vehicleDataRow.LC_VCLDLRLCVER = CType(Me.lcVcldlrlcverHidden.Value, Long)
        End If
        '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

        vehicleDataTbl.AddSC3080206VehicleRow(vehicleDataRow)

    End Sub

    '画面の値を設定する
    Protected Sub SetDisplayValues(ByVal vehicleDataTbl As SC3080206DataSet.SC3080206VehicleDataTable)

        Dim vehicleDataRow As SC3080206DataSet.SC3080206VehicleRow = vehicleDataTbl.Item(0)

        Me.makerTextBox.Text = vehicleDataRow.MAKERNAME         'メーカー
        Me.modelTextBox.Text = vehicleDataRow.SERIESNM        'モデル
        Me.vclregnoTextBox.Text = vehicleDataRow.VCLREGNO       '車両登録No.
        Me.vinTextBox.Text = vehicleDataRow.VIN                 'VIN
        If (Not vehicleDataRow.IsSEQNONull()) Then
            Me.selectSeqnoHidden.Value = CStr(vehicleDataRow.SEQNO)   'SEQNO
        End If

        If (Not String.IsNullOrEmpty(vehicleDataRow.FUELDVS) AndAlso
            IsNumeric(vehicleDataRow.FUELDVS)) Then
            Me.fueldvsTextBox.Text = WebWordUtility.GetWord(SC3080206BusinessLogic.FueldvsStartDisplayno + _
                                                  CType(vehicleDataRow.FUELDVS, Integer))               '燃料区分
        End If

        Me.bdyclrTextBox.Text = vehicleDataRow.BDYCLRNM          '外鈑色名称
        'If (Not String.IsNullOrEmpty(vehicleDataRow.BDYCLRCD)) Then
        '    Me.bdyclrTextBox.Text = Me.bdyclrTextBox.Text & "(" & vehicleDataRow.BDYCLRCD & ")"
        'End If
        Me.enginenoTextBox.Text = vehicleDataRow.ENGINENO           'エンジンNo.
        Me.gradeTextBox.Text = vehicleDataRow.GRADE                 'グレード
        Me.baseTypeTextBox.Text = vehicleDataRow.BASETYPE           '型式

        If (Not vehicleDataRow.IsVCLREGDATENull) Then
            '車両登録日
            Me.vclregdateTextBox.Text = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, _
                                                                          vehicleDataRow.VCLREGDATE, _
                                                                          vehicleDataRow.DLRCD)

        End If
        If (Not vehicleDataRow.IsVCLDELIDATENull) Then
            '納車日
            Me.vcldelidateDateTime.Value = vehicleDataRow.VCLDELIDATE
            Me.vcldelidateTextBox.Text = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, _
                                                                       vehicleDataRow.VCLDELIDATE, _
                                                                       vehicleDataRow.DLRCD)
        End If
        If (Not vehicleDataRow.IsREGISTDATENull) Then
            '最終入庫日
            Me.registdateTextBox.Text = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, _
                                                                       vehicleDataRow.REGISTDATE, _
                                                                       vehicleDataRow.DLRCD)
        End If
        If (Not String.IsNullOrEmpty(vehicleDataRow.NEWVCLDVS) AndAlso
            IsNumeric(vehicleDataRow.NEWVCLDVS)) Then
            Me.newvcldvsTextBox.Text = WebWordUtility.GetWord(SC3080206BusinessLogic.FueldvsStartNewvcldvs + _
                                                  CType(vehicleDataRow.NEWVCLDVS, Integer))         '新・中区分
        End If
        If (vehicleDataRow.NEWVCLDVS.Equals(SC3080206BusinessLogic.NewvcldvsUsed)) Then
            Me.cponmTextBox.Text = vehicleDataRow.CPONM              'CPO区分
        Else
            '新・中区分が1:Used(Car以外の場合は)『-』表示
            Me.cponmTextBox.Text = "-"                               'CPO区分
        End If

        If (Not vehicleDataRow.IsMILEAGENull) Then
            Me.mileageTextBox.Text = Format(vehicleDataRow.MILEAGE, "#,##0")
        End If

        '活動区分ID
        If (Not vehicleDataRow.IsACTVCTGRYIDNull()) Then
            Me.actvctgryidHidden.Value = CType(vehicleDataRow.ACTVCTGRYID, String)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            Me.actvctgryidHidden_Old.Value = CType(vehicleDataRow.ACTVCTGRYID, String)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
        End If
        '活動区分変更アカウント
        'vehicleDataRow.AC_MODFACCOUNT = " "
        '活動区分変更機能
        'vehicleDataRow.AC_MODFFUNCDVS = Me.活動区分.Text
        '活動除外理由ID
        If (Not vehicleDataRow.IsREASONIDNull()) Then
            Me.reasonidHidden.Value = CType(vehicleDataRow.REASONID, String)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            Me.reasonidHidden_Old.Value = CType(vehicleDataRow.REASONID, String)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
        End If

        Me.systemidTextBox.Text = vehicleDataRow.MEMSYSTEMID             '会員ID
        If (Not String.IsNullOrEmpty(vehicleDataRow.MEMREGSTATUS) AndAlso
            IsNumeric(vehicleDataRow.MEMREGSTATUS)) Then
            Me.regstatusTextBox.Text = WebWordUtility.GetWord(SC3080206BusinessLogic.FueldvsStartMemregstatus + _
                                                  CType(vehicleDataRow.MEMREGSTATUS, Integer))         '会員ステータス
        End If
        If (Not String.IsNullOrEmpty(vehicleDataRow.CONTRACT_STATUS) AndAlso
            IsNumeric(vehicleDataRow.CONTRACT_STATUS)) Then
            '契約ｽﾃｰﾀｽ
            If (vehicleDataRow.CONTRACT_STATUS.Equals(SC3080206BusinessLogic.ContractStatusKeiyaku) Or _
                vehicleDataRow.CONTRACT_STATUS.Equals(SC3080206BusinessLogic.ContractStatusMoushikomi)) Then
                Me.contractstatusTextBox.Text = WebWordUtility.GetWord(SC3080206BusinessLogic.FueldvsStartContractStatuss + _
                                                      CType(vehicleDataRow.CONTRACT_STATUS, Integer))
            Else
                '上記以外：契約なし
                Me.contractstatusTextBox.Text = WebWordUtility.GetWord(SC3080206BusinessLogic.FueldvsStartContractStatussNo)
            End If
        End If
        'テレマ契約の接続区分（1:DCM接続、2:携帯接続）																				
        If (Not String.IsNullOrEmpty(vehicleDataRow.CONNECT_DVS) AndAlso
            IsNumeric(vehicleDataRow.CONNECT_DVS)) Then
            Me.connectdvsTextBox.Text = WebWordUtility.GetWord(SC3080206BusinessLogic.FueldvsStartConnectDvs + _
                                                  CType(vehicleDataRow.CONNECT_DVS, Integer))         '接続区分
        End If
        If (Not vehicleDataRow.IsCONTRACT_START_DATENull) Then  '利用開始日
            Me.contractstartdateTextBox.Text = DateTimeFunc.FormatDate(3, vehicleDataRow.CONTRACT_START_DATE)
            'Me.contractstartdateTextBox.Text = Format(vehicleDataRow.CONTRACT_START_DATE, "yyyy/MM/dd")
        End If
        'テレマ契約ステータスが4：契約中(Yes)の
        'テレマ契約情報が存在しかつ
        '契約満了日が空白の場合は｢Unlimited｣を固定で表示
        If (Not vehicleDataRow.IsCONTRACT_END_DATENull) Then    '契約満了日
            Me.contractenddateTextBox.Text = DateTimeFunc.FormatDate(3, vehicleDataRow.CONTRACT_END_DATE)
            'Me.contractenddateTextBox.Text = Format(vehicleDataRow.CONTRACT_END_DATE, "yyyy/MM/dd")
        Else
            If (vehicleDataRow.CONTRACT_STATUS.Equals(SC3080206BusinessLogic.ContractStatusKeiyaku)) Then
                Me.contractenddateTextBox.Text = WebWordUtility.GetWord(SC3080206BusinessLogic.Unlimited)
            End If
        End If
        Me.telematelnumber1TextBox.Text = vehicleDataRow.TELEMA_TELNUMBER1  '緊急連絡先1
        Me.telematelnumber2TextBox.Text = vehicleDataRow.TELEMA_TELNUMBER2  '緊急連絡先2
        Me.telematelnumber3TextBox.Text = vehicleDataRow.TELEMA_TELNUMBER3  '緊急連絡先3

        'G-BOOKフラグ
        If (vehicleDataRow.GBOOKFLG.Equals(SC3080206BusinessLogic.KibouSuru)) Then
            Me.gbookCheckButton.Checked = True      '希望する
        Else
            Me.gbookCheckButton.Checked = False     '希望しない
        End If

        If (vehicleDataRow.CUSTFLG = SC3080206BusinessLogic.OrgCustflg) Then
            '自社客時に初期化する
            'ACTVCTGRYID	活動区分ID						
            'REASONID		活動除外理由ID			
            Me.actvctgryidHidden.Value = CType(SC3080205BusinessLogic.InitActvctgryId, String)       '活動区分
            Me.reasonidHidden.Value = String.Empty       '断念理由
        End If

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        If (Not vehicleDataRow.IsVCLLCVERNull()) Then
            Me.vcllcverHidden.Value = CType(vehicleDataRow.VCLLCVER, String)
        End If
        If (Not vehicleDataRow.IsVCLDLRLCVERNull()) Then
            Me.vcldlrlcverHidden.Value = CType(vehicleDataRow.VCLDLRLCVER, String)
        End If
        If (vehicleDataRow.CUSTFLG = SC3080206BusinessLogic.OrgCustflg) Then
            If (Not vehicleDataRow.IsCSTVCLLCVERNull()) Then
                Me.cstvcllcverHidden.Value = CType(vehicleDataRow.CSTVCLLCVER, String)
            End If
            If (Not vehicleDataRow.IsVCLIDNull()) Then
                Me.cstvclidHidden.Value = CType(vehicleDataRow.VCLID, String)
            End If
        End If
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

        '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        Me.vclMileTextBox.Text = vehicleDataRow.VCL_MILE
        Me.modelYearHidden.Value = vehicleDataRow.MODEL_YEAR
        Me.modelYearNameHidden.Value = vehicleDataRow.MODEL_YEAR
        Me.modelYearLabel2.Text() = vehicleDataRow.MODEL_YEAR
        If (Not vehicleDataRow.IsLC_VCLDLRLCVERNull()) Then
            Me.lcVcldlrlcverHidden.Value = CType(vehicleDataRow.LC_VCLDLRLCVER, String)
        End If
        '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END 

        If (Not vehicleDataRow.IsACTVCTGRYIDNull()) Then

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetDisplayValues vehicleDataRow.IsACTVCTGRYIDNull Start")
            'ログ出力 End *****************************************************************************

            '活動区分リストのセット
            Dim wordList As New List(Of String)
            Dim word As String
            wordList.Add(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay1))
            wordList.Add(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay2))
            wordList.Add(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay3))
            wordList.Add(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay4))

            ''活動区分リストのセット
            'Dim dtActvctgry As New DataTable()
            'dtActvctgry.Columns.Add("Actvctgryid", GetType(Integer))
            'dtActvctgry.Columns.Add("ActvctgryName", GetType(String))
            Dim dtActvctgry As New SC3080205DataSet.SC3080205ActvctgryDataTable
            dtActvctgry.Rows.Add(New Object() {SC3080205BusinessLogic.ActvctgryidDisplay1, wordList.Item(0)})
            dtActvctgry.Rows.Add(New Object() {SC3080205BusinessLogic.ActvctgryidDisplay2, wordList.Item(1)})
            dtActvctgry.Rows.Add(New Object() {SC3080205BusinessLogic.ActvctgryidDisplay3, wordList.Item(2)})
            dtActvctgry.Rows.Add(New Object() {SC3080205BusinessLogic.ActvctgryidDisplay4, wordList.Item(3)})

            '文言マスタ取得
            Dim i As Integer = 0
            For i = 0 To wordList.Count - 1
                If (vehicleDataRow.ACTVCTGRYID = (i + 1)) Then
                    word = CType(wordList.Item(i), String)
                    Me.actvctgryLabel2.Text = word
                    Me.actvctgryNameHidden.Value = word
                End If
            Next

            Me.actvctgryidHidden.Value = CType(vehicleDataRow.ACTVCTGRYID, String)       '活動区分
            If (Not vehicleDataRow.IsREASONIDNull) Then
                Dim msgID As Integer = 0
                '2:情報不備
                Dim giveupReasonList As SC3080205DataSet.SC3080205OmitreasonDataTable = _
                    SC3080205BusinessLogic.GetGiveupReason(msgID)

                Dim giveupReasonRow As SC3080205DataSet.SC3080205OmitreasonRow
                For i = 0 To giveupReasonList.Count - 1
                    giveupReasonRow = giveupReasonList.Item(i)
                    '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) START
                    '2013/06/30 TCS 趙 2013/10対応版 既存流用 START
                    If (giveupReasonRow.ACT_CAT_TYPE = Me.actvctgryidHidden.Value And Convert.ToString(giveupReasonRow.REASONID) = vehicleDataRow.REASONID) Then
                        '2013/06/30 TCS 趙 2013/10対応版 既存流用 END
                        If (Not String.IsNullOrEmpty(Me.actvctgryLabel2.Text)) Then
                            '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) END
                            Me.actvctgryLabel2.Text = Me.actvctgryLabel2.Text & "-"
                        End If
                        Me.actvctgryLabel2.Text = Me.actvctgryLabel2.Text & giveupReasonRow.REASON
                        Me.reasonNameHidden.Value = giveupReasonRow.REASON
                    End If
                Next
                Me.reasonidHidden.Value = CType(vehicleDataRow.REASONID, String)      '断念理由
            End If

            '2012/04/27 TCS 安田 【SALES_2】HTMLEncode対応 START
            Me.actvctgryLabel2.Text = HttpUtility.HtmlEncode(Me.actvctgryLabel2.Text)
            '2012/04/27 TCS 安田 【SALES_2】HTMLEncode対応 END

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("SetDisplayValues vehicleDataRow.IsACTVCTGRYIDNull End")
            'ログ出力 End *****************************************************************************

        End If

        ' 車両編集－変更前情報の保持
        Call SetVehicleBackHidden()

    End Sub

    ''' <summary>
    ''' 車両編集－変更前情報の保持。
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetVehicleBackHidden()

        '変更前情報の保持
        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        'Me.makerTextBoxBackHidden.Value = Me.makerTextBox.Text
        'Me.modelTextBoxBackHidden.Value = Me.modelTextBox.Text
        'Me.vclregnoTextBoxBackHidden.Value = Me.vclregnoTextBox.Text
        'Me.vinTextBoxBackHidden.Value = Me.vinTextBox.Text
        'If (Me.vcldelidateDateTime.Value Is Nothing) Then
        '    Me.vcldelidateDateTimeBackHidden.Value = String.Empty
        'Else
        '    Me.vcldelidateDateTimeBackHidden.Value = Format(Me.vcldelidateDateTime.Value, "yyyy-MM-dd")
        'End If
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END
        Me.editVehicleModeBackHidden.Value = Me.editVehicleModeHidden.Value

        '自社客時のみ活動区分を保存
        If (Me.custFlgHidden.Value.Equals(CType(SC3080206BusinessLogic.OrgCustflg, String))) Then                  '活動除外理由
            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            'Me.actvctgryidHiddenBackHidden.Value = Me.actvctgryidHidden.Value               '活動区分
            'Me.reasonidHiddenBackHidden.Value = Me.reasonidHidden.Value
            'Me.actvctgryLabel2BackHidden.Value = Me.actvctgryLabel2.Text                    '活動除外名称
            'Me.actvctgryNameBackHidden.Value = Me.actvctgryNameHidden.Value                 '活動区分名称
            'Me.reasonNameBackHidden.Value = Me.reasonNameHidden.Value                       '活動断念理由名称
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END
        End If

    End Sub



#End Region

#Region " イベント 車両編集"

    Protected Function AppendVehicle(ByVal sender As Object, ByVal e As System.EventArgs) As Integer

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("AppendVehicle Start")
        'ログ出力 End *****************************************************************************

        '追加モードに変更する　(０：新規登録モード)
        Me.editModeHidden.Value = CStr(SC3080206BusinessLogic.ModeCreate)

        Me.makerTextBox.Text = String.Empty             'メーカー
        Me.modelTextBox.Text = String.Empty             'モデル
        Me.vclregnoTextBox.Text = String.Empty          '車両登録No.
        Me.vinTextBox.Text = String.Empty               'VIN
        Me.vcldelidateDateTime.Value = Nothing          '納車日

        '初期処理
        Call VehicleInitialize()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("AppendVehicle End")
        'ログ出力 End *****************************************************************************

        Return 0

    End Function

    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
    Protected Sub CustomerCarEditPopupOpenButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerCarEditPopupOpenButton.Click
        Logger.Info("CustomerCarEditPopupOpenButton_Click Start")

        'ポップアップを囲うPanelをVisible=Trueに
        Me.CustomerCarEditVisiblePanel.Visible = True
        Me.NameListActvctgryReasonListVisiblePanel.Visible = True

        '車両編集ポップアップ表示
        Me.VehicleInitialize()

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "CustomerCarEditPopUpOpenAfter", "startup")

        Logger.Info("CustomerCarEditPopupOpenButton_Click End")
    End Sub
    '2012/03/08 TCS 山口 【SALES_2】性能改善 END
#End Region

#Region " メソット 顧客情報編集"

    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 入力項目設定 対象項目ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TGT_ITEM_ID_FIRSTNAME As String = "01"             'ファーストネーム
    Private Const TGT_ITEM_ID_MIDDLENAME As String = "02"            'ミドルネーム
    Private Const TGT_ITEM_ID_LASTNAME As String = "03"              'ラストネーム
    Private Const TGT_ITEM_ID_SEX As String = "04"                   '性別
    Private Const TGT_ITEM_ID_NAMETITLE As String = "05"             '敬称
    Private Const TGT_ITEM_ID_CUSTYPE As String = "06"               '顧客区分(個人/法人)
    Private Const TGT_ITEM_ID_PRIVATE_FLEET_ITEM_CD As String = "07" '個人法人項目
    Private Const TGT_ITEM_ID_EMPLOYEENAME As String = "08"          '担当者氏名(法人)
    Private Const TGT_ITEM_ID_EMPLOYEEDEPARTMENT As String = "09"    '担当者部署(法人)
    Private Const TGT_ITEM_ID_EMPLOYEEPOSITION As String = "10"      '担当者役職(法人)
    Private Const TGT_ITEM_ID_MOBILE As String = "11"                '携帯番号
    Private Const TGT_ITEM_ID_TELNO As String = "12"                 '自宅電話番号
    Private Const TGT_ITEM_ID_BUSINESSTELNO As String = "13"         '勤務先電話番号
    Private Const TGT_ITEM_ID_FAXNO As String = "14"                 '自宅FAX番号
    Private Const TGT_ITEM_ID_ZIPCODE As String = "15"               '郵便番号
    Private Const TGT_ITEM_ID_ADDRESS1 As String = "16"              '住所1
    Private Const TGT_ITEM_ID_ADDRESS2 As String = "17"              '住所2
    Private Const TGT_ITEM_ID_ADDRESS3 As String = "18"              '住所3
    Private Const TGT_ITEM_ID_ADDRESS_STATE As String = "19"         '住所(州)
    Private Const TGT_ITEM_ID_ADDRESS_DISTRICT As String = "20"      '住所(地域)
    Private Const TGT_ITEM_ID_ADDRESS_CITY As String = "21"          '住所(市)
    Private Const TGT_ITEM_ID_ADDRESS_LOCATION As String = "22"      '住所(地区)
    Private Const TGT_ITEM_ID_DOMICILE As String = "23"              '本籍
    Private Const TGT_ITEM_ID_EMAIL1 As String = "24"                'e-Mail1
    Private Const TGT_ITEM_ID_EMAIL2 As String = "25"                'e-Mail2
    Private Const TGT_ITEM_ID_COUNTRY As String = "26"               '国籍
    Private Const TGT_ITEM_ID_SOCIALID As String = "27"              '国民ID
    Private Const TGT_ITEM_ID_BIRTHDAY As String = "28"              '誕生日
    Private Const TGT_ITEM_ID_ACTVCTGRYID As String = "29"           '活動区分ID
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

    '2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 START
    Private Const TGT_ITEM_ID_COMMERCIAL_RECV_TYPE As String = "36"  '商業情報受取区分
    '2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 END

    ''' <summary>
    ''' ロード次の処理を実施します。
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CustomerEditInitialize()

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditInitialize Start")
        'ログ出力 End *****************************************************************************

        '2012/04/27 TCS 安田 【SALES_2】HTMLEncode対応 START

        ''コールバックスプリクト登録
        'ScriptManager.RegisterStartupScript(Me, Me.GetType(), _
        '                                   "Callback", _
        '                                   String.Format(CultureInfo.InvariantCulture, _
        '                                                 "callback.beginCallback = function () {{ {0}; }};", _
        '                                                 Page.ClientScript.GetCallbackEventReference(Me, _
        '                                                                                             "callback.packedArgument", _
        '                                                                                             "callback.endCallback", _
        '                                                                                             "", _
        '                                                                                             True)), _
        '                                                 True)

        '前画面からセッション情報を取得
        'モード (０：新規登録モード、１：編集モード)
        Dim mode As Integer = CInt(Me.editModeHidden.Value)
        Dim msgID As Integer = 0
        Dim bizClass As New SC3080205BusinessLogic
        Dim custDataTbl As New SC3080205DataSet.SC3080205CustDataTable
        Dim custDataRow As SC3080205DataSet.SC3080205CustRow

        custDataRow = custDataTbl.NewSC3080205CustRow
        custDataTbl.Rows.Add(custDataRow)       '追加する

        'セッション内の値をセットする
        SetSessionValue(custDataRow)

        '初期表示用フラグ情報取得
        SC3080205BusinessLogic.GetInitializeFlg(custDataTbl, msgID)

        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        '既存不具合対応
        Me.postSearchVisibleHidden.Value = "1"
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        '郵便番号辞書検索使用可否
        Select Case custDataRow.POSTSRHFLG
            Case 0
                '住所検索ボタン	非表示
                '郵便番号   活性で表示
                '住所 活性で表示
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
                '既存不具合対応
                'Me.zipSerchButton.Visible = False
                Me.postSearchVisibleHidden.Value = "0"
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
                Me.zipcodeTextBox.Enabled = True
                Me.addressTextBox.Enabled = True
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
                Me.address2TextBox.Enabled = True
                Me.address3TextBox.Enabled = True
                Me.addressState.Enabled = True
                Me.addressDistrict.Enabled = True
                Me.addressCity.Enabled = True
                Me.addressLocation.Enabled = True
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
            Case 1
                '住所検索ボタン	活性で表示
                '郵便番号   活性で表示
                '住所 活性で表示
                Me.zipSerchButton.Enabled = True
                Me.zipcodeTextBox.Enabled = True
                Me.addressTextBox.Enabled = True
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
                Me.address2TextBox.Enabled = True
                Me.address3TextBox.Enabled = True
                Me.addressState.Enabled = True
                Me.addressDistrict.Enabled = True
                Me.addressCity.Enabled = True
                Me.addressLocation.Enabled = True
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
            Case 2
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
                ''住所検索ボタン	活性で表示 
                ''郵便番号   非活性で表示
                ''住所 非活性で表示
                'Me.zipSerchButton.Enabled = True
                'Me.zipcodeTextBox.Enabled = False
                'Me.addressTextBox.Enabled = False
                '住所検索ボタン	活性で表示
                '郵便番号   活性で表示
                '住所 活性で表示
                Me.zipSerchButton.Enabled = True
                Me.zipcodeTextBox.Enabled = True
                Me.addressTextBox.Enabled = True
                Me.address2TextBox.Enabled = True
                Me.address3TextBox.Enabled = True
                Me.addressState.Enabled = True
                Me.addressDistrict.Enabled = True
                Me.addressCity.Enabled = True
                Me.addressLocation.Enabled = True
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        End Select

        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        ' ラベル・敬称設定フラグセット
        Me.labelNametitleSettingHidden.Value = custDataRow.LABEL_NAMETITLE_SETTING
        ' 住所表示順フラグセット
        Me.addressDirectionHidden.Value = custDataRow.ADDRESS_DISP_DIRECTION
        ' 住所データクレンジング可否フラグセット
        Me.addressDataCleansingHidden.Value = custDataRow.ADDRESS_DATACLEANSING_FLG
        '2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 START
        ' 住所１自動入力フラグセット
        Me.address1AutoInputHidden.Value = custDataRow.ADDRESS1_AUTOINPUT_FLG
        '2019/06/06 TS  村井 (FS)サービススタッフ納期遵守オペレーション確立に向けた試験研究 END

        ' 性別表示設定
        If custDataRow.GENDER_DISP_SETTING.Length = 2 Then
            ' その他(1桁目)
            Dim otherDispFlg As String = custDataRow.GENDER_DISP_SETTING.Substring(0, 1)
            If (otherDispFlg = "1") Then
                Me.sexOtherCol.Visible = True
            Else
                Me.sexOtherCol.Visible = False
            End If
            ' 不明(2桁目)
            Dim unknownDispFlg As String = custDataRow.GENDER_DISP_SETTING.Substring(1, 1)
            If (unknownDispFlg = "1") Then
                Me.sexUnknownCol.Visible = True
            Else
                Me.sexUnknownCol.Visible = False
            End If
        Else
            Me.sexOtherCol.Visible = False
            Me.sexUnknownCol.Visible = False
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        'エラーセット
        '氏名未入力エラー
        Me.custNoNameErrMsg.Value = WebWordUtility.GetWord(40902)
        '電話番号 Or 携帯番号未入力エラー
        Me.custNoTelNoErrMsg.Value = WebWordUtility.GetWord(40907)
        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
        'ダミー名称フラグ＝1:ダミー名称で、氏名が変更されなかった場合のエラー
        Me.custNoDummyNameFlgErrMsg.Value = WebWordUtility.GetWord(40934)
        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
        '商業情報受取区分未選択エラー
        Me.custNoCommercialRecvType.Value = WebWordUtility.GetWord(40991)
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        Me.custNoMiddleNameErrMsg.Value = WebWordUtility.GetWord(40949) 'ミドルネーム未入力
        Me.custNoLastNameErrMsg.Value = WebWordUtility.GetWord(40950)   'ラストネーム未入力
        Me.custNoSexErrMsg.Value = WebWordUtility.GetWord(40951)    '性別未入力
        Me.custNoNameTitleErrMsg.Value = WebWordUtility.GetWord(40952)  '敬称未入力
        Me.custNoCustypeErrMsg.Value = WebWordUtility.GetWord(40953)    '個人/法人未入力
        Me.custNoPrivateFleetItemErrMsg.Value = WebWordUtility.GetWord(40954)   '個人法人項目未入力
        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
        Me.custNoOrgnzNameErrMsg.Value = WebWordUtility.GetWord(4000901) '顧客組織名称を入力してください。
        Me.custNoSubCtgry2ErrMsg.Value = WebWordUtility.GetWord(4000902) '顧客サブカテゴリ2を入力してください。
        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
        Me.custNoEmpNameErrMsg.Value = WebWordUtility.GetWord(40955)    '担当者氏名未入力
        Me.custNoEmpDeptErrMsg.Value = WebWordUtility.GetWord(40956)    '担当者部署未入力
        Me.custNoEmpPosErrMsg.Value = WebWordUtility.GetWord(40957) '担当者役職未入力
        Me.custNoFaxErrMsg.Value = WebWordUtility.GetWord(40958)    'FAX番号未入力
        Me.custNoBussinessTelErrMsg.Value = WebWordUtility.GetWord(40959)   '勤務先電話番号未入力
        Me.custNoZipErrMsg.Value = WebWordUtility.GetWord(40960)    '郵便番号未入力
        Me.custNoAddress1ErrMsg.Value = WebWordUtility.GetWord(40961)   '住所1未入力
        Me.custNoAddress2ErrMsg.Value = WebWordUtility.GetWord(40962)   '住所2未入力
        Me.custNoAddress3ErrMsg.Value = WebWordUtility.GetWord(40963)   '住所3未入力
        Me.custNoStateErrMsg.Value = WebWordUtility.GetWord(40964)  '住所(州)未入力
        Me.custNoDistrictErrMsg.Value = WebWordUtility.GetWord(40965)   '住所(地域)未入力
        Me.custNoCityErrMsg.Value = WebWordUtility.GetWord(40966)   '住所(市)未入力
        Me.custNoLocationErrMsg.Value = WebWordUtility.GetWord(40967)   '住所(地区)未入力
        Me.custNoDomicileErrMsg.Value = WebWordUtility.GetWord(40968)   '本籍未入力
        Me.custNoEmail1ErrMsg.Value = WebWordUtility.GetWord(40969) 'e-Mail1未入力
        Me.custNoEmail2ErrMsg.Value = WebWordUtility.GetWord(40970) 'e-Mail2未入力
        Me.custNoCountryErrMsg.Value = WebWordUtility.GetWord(40971)    '国籍未入力
        Me.custNoSocialIdErrMsg.Value = WebWordUtility.GetWord(40972)   '国民ID未入力
        Me.custNoBirtydayErrMsg.Value = WebWordUtility.GetWord(40973)   '誕生日未入力
        Me.custNoActvctgryErrMsg.Value = WebWordUtility.GetWord(40974)  '活動区分未入力
        Me.custNoFirmNameErrMsg.Value = WebWordUtility.GetWord(40976) 'Firm Name未入力
        Me.custNoContactPersonErrMsg.Value = WebWordUtility.GetWord(40977) 'Contact Person未入力
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        '敬称リスト取得
        Dim nameTitleList As SC3080205DataSet.SC3080205NameTitleDataTable = _
            SC3080205BusinessLogic.GetNameTitleList(custDataTbl, msgID)
        nameTitleRepeater.DataSource = nameTitleList
        nameTitleRepeater.DataBind()

        '活動区分リストのセット
        Dim wordList As New List(Of String)
        'Dim word As String
        wordList.Add(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay1))
        wordList.Add(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay2))
        wordList.Add(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay3))
        wordList.Add(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay4))

        '活動区分リストのセット
        'Dim dtActvctgry As New DataTable()
        'dtActvctgry.Columns.Add("Actvctgryid", GetType(Integer))
        'dtActvctgry.Columns.Add("ActvctgryName", GetType(String))
        Dim dtActvctgry As New SC3080205DataSet.SC3080205ActvctgryDataTable
        dtActvctgry.Rows.Add(New Object() {SC3080205BusinessLogic.ActvctgryidDisplay1, wordList.Item(0)})
        dtActvctgry.Rows.Add(New Object() {SC3080205BusinessLogic.ActvctgryidDisplay2, wordList.Item(1)})
        dtActvctgry.Rows.Add(New Object() {SC3080205BusinessLogic.ActvctgryidDisplay3, wordList.Item(2)})
        dtActvctgry.Rows.Add(New Object() {SC3080205BusinessLogic.ActvctgryidDisplay4, wordList.Item(3)})

        With actvctgryRepeater
            .DataSource = dtActvctgry
            .DataBind()
        End With

        '断念理由リストのセット
        Dim giveupReasonTbl As SC3080205DataSet.SC3080205OmitreasonDataTable = _
            SC3080205BusinessLogic.GetGiveupReason(msgID)

        With reasonRepeater
            .DataSource = giveupReasonTbl
            .DataBind()
        End With

        '活動区分リスト
        If (mode = SC3080205BusinessLogic.ModeCreate) Then
            '０：新規登録モード

            '活動区分の初期値セット
            Me.actvctgryidHidden.Value = CType(SC3080205BusinessLogic.InitActvctgryId, String)
            Me.actvctgryLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay1))

            Me.actvctgryNameHidden.Value = Me.actvctgryLabel.Text                 '活動区分名称
            Me.reasonNameHidden.Value = String.Empty                              '活動断念理由名称

            '担当者氏名（法人）	個人/法人区分が法人の場合のみ表示
            '担当者部署名（法人）
            '役職（法人）
            'Me.houjinPanel.Visible = False
        End If

        Me.useNameTitleHidden.Value = "1"   '敬称使用許可区分
        Me.useActvctgryHidden.Value = "1"   '活動区分使用許可区分
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        Me.usePrivateFleetItemHidden.Value = "1"   '個人法人項目使用許可区分
        Me.useStateHidden.Value = "1"   '住所(州)使用許可区分
        Me.useDistrictHidden.Value = "1"   '住所(地域)使用許可区分
        Me.useCityHidden.Value = "1"   '住所(市)使用許可区分
        Me.useLocationHidden.Value = "1"   '住所(地区)使用許可区分
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        '2012/01/26 TCS 安田 【SALES_1B】来店実績より表示処理 START
        '登録 ボタンキャプションの切り替え
        '追加時で、セッション情報:車両登録No≠NULLの場合に、「車両編集」ボタンにする
        Me.nextVehicleFlg.Value = SC3080205BusinessLogic.TourokuBtn   '登録
        If (mode = SC3080205BusinessLogic.ModeCreate) Then

            '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) START
            'セッションキー 顧客名称
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_TENTATIVENAME) = True) Then
                'スペースで分割してファースト～ラストネーム欄に設定
                Dim names As String() = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_TENTATIVENAME, False), String).Split(New Char() {" "c})

                'ファーストネーム
                If (names.Length >= 1) Then
                    Me.nameTextBox.Text = names(0)
                Else
                    Me.nameTextBox.Text = String.Empty
                End If
                'ミドルネーム
                If (names.Length >= 2) Then
                    Me.middleNameTextBox.Text = names(1)
                Else
                    Me.middleNameTextBox.Text = String.Empty
                End If
                'ラストネーム

                If (names.Length >= 3) Then
                    Dim sb As New System.Text.StringBuilder
                    sb.Append(names(2))
                    For i = 3 To names.Length - 1
                        sb.Append(New Char() {" "c})
                        sb.Append(names(i))
                    Next
                    Me.lastNameTextBox.Text = sb.ToString()
                Else
                    Me.lastNameTextBox.Text = String.Empty
                End If
            End If

            'セッションキー 電話番号
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_TELNO) = True) Then
                Me.mobileTextBox.Text = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_TELNO, False), String) '顧客電話番号
            End If
            '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) END

            'セッションキー 車両登録No
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VCLREGNO) = True) Then
                Dim vclregno As String
                vclregno = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_VCLREGNO, False), String) '車両登録No
                If (String.IsNullOrEmpty(vclregno) = False) Then
                    Me.nextVehicleFlg.Value = SC3080205BusinessLogic.NectVehicleBtn   '車両編集へ
                    Me.completionButtonLabel.Text = Me.nextVehicleLabel.Text
                End If
            End If
        End If
        '2012/01/26 TCS 安田 【SALES_1B】来店実績より表示処理 END

        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        '個人法人項目リスト取得
        Dim privateFleetItemList As SC3080205DataSet.SC3080205PrivateFleetItemDataTable = _
            SC3080205BusinessLogic.GetPrivateFleetItem(msgID)
        privateFleetItemRepeater.DataSource = privateFleetItemList
        privateFleetItemRepeater.DataBind()

        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
        ' サブカテゴリ2リスト取得
        Dim custSubCtgry2List As SC3080205DataSet.SC3080205CustSubCtgry2DataTable = Nothing
        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END

        '州リスト取得
        Dim stateList As SC3080205DataSet.SC3080205StateDataTable = _
            SC3080205BusinessLogic.GetState(msgID)
        stateRepeater.DataSource = stateList
        stateRepeater.DataBind()

        '入力項目設定
        Call SetDispSetting()
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        If (mode = SC3080205BusinessLogic.ModeEdit) Then

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditInitialize Mode Edit Start")
            'ログ出力 End *****************************************************************************

            '１：編集モード
            '初期表示情報取得
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
            custDataRow.VCLID = Me.vclupdateHidden.Value
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
            custDataTbl = SC3080205BusinessLogic.GetInitialize(custDataTbl, msgID)

            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            Dim stateExistFlg As Boolean = False
            Dim districtExistFlg As Boolean = False
            Dim cityExistFlg As Boolean = False
            '地域リスト取得(州がセットされていて、マスタに存在する場合のみ)
            If (Not String.IsNullOrEmpty(custDataRow.ADDRESS_STATE)) Then
                For Each stateRow In stateList
                    If (custDataRow.ADDRESS_STATE.Trim.Equals(stateRow.STATE_CD.Trim)) Then
                        stateExistFlg = True
                    End If
                Next
            End If
            Dim districtList As SC3080205DataSet.SC3080205DistrictDataTable
            If (stateExistFlg = True) Then
                districtList = SC3080205BusinessLogic.GetDistrict(custDataRow.ADDRESS_STATE, msgID)
            Else
                districtList = New SC3080205DataSet.SC3080205DistrictDataTable()
            End If
            districtRepeater.DataSource = districtList
            districtRepeater.DataBind()

            '市リスト取得(地域がセットされていて、マスタに存在する場合のみ)
            If (Not String.IsNullOrEmpty(custDataRow.ADDRESS_DISTRICT)) Then
                For Each districtRow In districtList
                    If (custDataRow.ADDRESS_DISTRICT.Trim.Equals(districtRow.DISTRICT_CD.Trim)) Then
                        districtExistFlg = True
                    End If
                Next
            End If
            Dim cityList As SC3080205DataSet.SC3080205CityDataTable
            If (stateExistFlg = True And districtExistFlg = True) Then
                cityList = SC3080205BusinessLogic.GetCity(custDataRow.ADDRESS_STATE, custDataRow.ADDRESS_DISTRICT, msgID)
            Else
                cityList = New SC3080205DataSet.SC3080205CityDataTable()
            End If
            cityRepeater.DataSource = cityList
            cityRepeater.DataBind()

            '地区リスト取得(市がセットされていて、マスタに存在する場合のみ)
            If (Not String.IsNullOrEmpty(custDataRow.ADDRESS_CITY)) Then
                For Each cityRow In cityList
                    If (custDataRow.ADDRESS_CITY.Trim.Equals(cityRow.CITY_CD.Trim)) Then
                        cityExistFlg = True
                    End If
                Next
            End If
            Dim locationList As SC3080205DataSet.SC3080205LocationDataTable
            If (stateExistFlg = True And districtExistFlg = True And cityExistFlg = True) Then
                locationList = SC3080205BusinessLogic.GetLocation(custDataRow.ADDRESS_STATE, custDataRow.ADDRESS_DISTRICT, _
                                                                  custDataRow.ADDRESS_CITY, msgID)
            Else
                locationList = New SC3080205DataSet.SC3080205LocationDataTable()
            End If
            locationRepeater.DataSource = locationList
            locationRepeater.DataBind()
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

            'SMS配信可否	SMS使用可否フラグ（０：非表示、１：表示）
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START 
            Me.smsPanel.Visible = False
            Me.emailPanel.Visible = False
            Me.dmailPanel.Visible = False
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END 

            If (Not custDataRow.CUSTYPE.Equals(SC3080205BusinessLogic.Houjin)) Then
                '担当者氏名（法人）	個人/法人区分が法人の場合のみ表示
                '担当者部署名（法人）
                '役職（法人）
                'Me.houjinPanel.Visible = False
            End If

            If (custDataRow.CUSTFLG = SC3080205BusinessLogic.NewCustFlg) Then
                Me.actvctgryPanel.Visible = True  '活動区分
            End If

            If (custDataRow.CUSTFLG = SC3080205BusinessLogic.OrgCustFlg) Then
                '０：自社客
                If (mode = SC3080205BusinessLogic.ModeEdit) Then

                    'ログ出力 Start ***************************************************************************
                    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditInitialize OrgCust SetEnable Start")
                    'ログ出力 End *****************************************************************************

                    '１：編集モード
                    '※４　入力可・不可判定
                    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
                    'SetTextEnable(Me.nameTextBox, custDataRow, custDataRow.NAME, SC3080205TableAdapter.IdName)        '氏名
                    SetTextEnable(Me.nameTextBox, custDataRow, custDataRow.NAME, SC3080205TableAdapter.IdFirstName)         'ファーストネーム
                    SetTextEnable(Me.middleNameTextBox, custDataRow, custDataRow.NAME, SC3080205TableAdapter.IdMiddleName)   'ミドルネーム
                    SetTextEnable(Me.lastNameTextBox, custDataRow, custDataRow.NAME, SC3080205TableAdapter.IdLastName)     'ラストネーム
                    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
                    Dim nullflg As Boolean = False
                    If (custDataRow.IsNAMETITLE_CDNull() OrElse String.IsNullOrEmpty(custDataRow.NAMETITLE_CD)) Then
                        nullflg = True
                    End If
                    If (SetEnable(custDataRow, nullflg, SC3080205TableAdapter.IdNameTitle) = False) Then
                        Me.useNameTitleHidden.Value = "0"   '敬称使用許可区分
                    End If
                    SetCheckEnable2(Me.manCheckBox, custDataRow, custDataRow.SEX, SC3080205TableAdapter.IdSex)         '男
                    SetCheckEnable2(Me.girlCheckBox, custDataRow, custDataRow.SEX, SC3080205TableAdapter.IdSex)         '女
                    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
                    SetCheckEnable2(Me.otherCheckBox, custDataRow, custDataRow.SEX, SC3080205TableAdapter.IdSex)         'その他
                    SetCheckEnable2(Me.unknownCheckBox, custDataRow, custDataRow.SEX, SC3080205TableAdapter.IdSex)         '不明
                    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
                    SetCheckEnable2(Me.kojinCheckBox, custDataRow, custDataRow.CUSTYPE, SC3080205TableAdapter.IdCustype)    '個人
                    SetCheckEnable2(Me.houjinCheckBox, custDataRow, custDataRow.CUSTYPE, SC3080205TableAdapter.IdCustype)    '法人
                    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
                    If (custDataRow.IsPRIVATE_FLEET_ITEM_CDNull() OrElse String.IsNullOrEmpty(custDataRow.PRIVATE_FLEET_ITEM_CD)) Then
                        nullflg = True
                    End If
                    If (SetEnable(custDataRow, nullflg, SC3080205TableAdapter.IdPrivateFleetItem) = False) Then
                        Me.usePrivateFleetItemHidden.Value = "0" '個人法人項目使用許可区分
                    End If
                    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
                    SetTextEnable(Me.employeenameTextBox, custDataRow, custDataRow.EMPLOYEENAME, SC3080205TableAdapter.IdEmployeeName) '担当者氏名
                    SetTextEnable(Me.employeedepartmentTextBox, custDataRow, custDataRow.EMPLOYEEDEPARTMENT, SC3080205TableAdapter.IdEmployeeDepartment)    '担当者部署名
                    SetTextEnable(Me.employeepositionTextBox, custDataRow, custDataRow.EMPLOYEEPOSITION, SC3080205TableAdapter.IdEmployeePosition)    '役職
                    SetTextEnable(Me.mobileTextBox, custDataRow, custDataRow.MOBILE, SC3080205TableAdapter.IdMobile)      '携帯
                    SetTextEnable(Me.telnoTextBox, custDataRow, custDataRow.TELNO, SC3080205TableAdapter.Idtelno)       '自宅
                    SetTextEnable(Me.businesstelnoTextBox, custDataRow, custDataRow.BUSINESSTELNO, SC3080205TableAdapter.IdBusinessTelno)   '勤務先
                    SetTextEnable(Me.faxnoTextBox, custDataRow, custDataRow.FAXNO, SC3080205TableAdapter.IdFaxno)        'FAX
                    SetTextEnable(Me.zipcodeTextBox, custDataRow, custDataRow.ZIPCODE, SC3080205TableAdapter.IdZipcode) '郵便番号
                    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
                    'SetTextEnable(Me.addressTextBox, custDataRow, custDataRow.ADDRESS, SC3080205TableAdapter.IdAddress)     '住所
                    SetTextEnable(Me.addressTextBox, custDataRow, custDataRow.ADDRESS1, SC3080205TableAdapter.IdAddress1)     '住所1
                    SetTextEnable(Me.address2TextBox, custDataRow, custDataRow.ADDRESS1, SC3080205TableAdapter.IdAddress2)     '住所2
                    SetTextEnable(Me.address3TextBox, custDataRow, custDataRow.ADDRESS1, SC3080205TableAdapter.IdAddress3)     '住所3
                    If (custDataRow.IsADDRESS_STATENull() OrElse String.IsNullOrEmpty(custDataRow.ADDRESS_STATE)) Then
                        nullflg = True
                    End If
                    If (SetEnable(custDataRow, nullflg, SC3080205TableAdapter.IdState) = False) Then
                        Me.useStateHidden.Value = "0" '住所(州)使用許可区分
                    End If
                    If (custDataRow.IsADDRESS_DISTRICTNull() OrElse String.IsNullOrEmpty(custDataRow.ADDRESS_DISTRICT)) Then
                        nullflg = True
                    End If
                    If (SetEnable(custDataRow, nullflg, SC3080205TableAdapter.IdDistrict) = False) Then
                        Me.useDistrictHidden.Value = "0" '住所(地域)使用許可区分
                    End If
                    If (custDataRow.IsADDRESS_CITYNull() OrElse String.IsNullOrEmpty(custDataRow.ADDRESS_CITY)) Then
                        nullflg = True
                    End If
                    If (SetEnable(custDataRow, nullflg, SC3080205TableAdapter.IdCity) = False) Then
                        Me.useCityHidden.Value = "0" '住所(市)使用許可区分
                    End If
                    If (custDataRow.IsADDRESS_LOCATIONNull() OrElse String.IsNullOrEmpty(custDataRow.ADDRESS_LOCATION)) Then
                        nullflg = True
                    End If
                    If (SetEnable(custDataRow, nullflg, SC3080205TableAdapter.IdLocation) = False) Then
                        Me.useLocationHidden.Value = "0" '住所(地区)使用許可区分
                    End If
                    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
                    SetTextEnable(Me.email1TextBox, custDataRow, custDataRow.EMAIL1, SC3080205TableAdapter.IdEmail1)    'EMail1
                    SetTextEnable(Me.email2TextBox, custDataRow, custDataRow.EMAIL2, SC3080205TableAdapter.IdEmail2)    'EMail2
                    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
                    SetTextEnable(Me.domicileTextBox, custDataRow, custDataRow.DOMICILE, SC3080205TableAdapter.IdDomicile)  '本籍
                    SetTextEnable(Me.countryTextBox, custDataRow, custDataRow.COUNTRY, SC3080205TableAdapter.IdCountry)     '国籍
                    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
                    '2012/03/28 TCS 高橋 【SALES_2】自社客 国民ID変更不可 START
                    'SetTextEnable(Me.socialidTextBox, custDataRow, custDataRow.SOCIALID, SC3080205TableAdapter.IdSocialid)   '国民ID
                    Me.socialidTextBox.Enabled = False            '国民ID(フラグの状態に関わらず編集不可)
                    '2012/03/28 TCS 高橋 【SALES_2】自社客 国民ID変更不可 END
                    SetDateEnable(Me.birthdayTextBox, custDataRow, custDataRow.IsBIRTHDAYNull, SC3080205TableAdapter.IdBirthday)   '誕生日
                    'GetImageButtonEnable(Me.nameTitleButton, custDataRow, custDataRow.NAMETITLE_CD, SC3080205DataTableTableAdapter.IdNAMETITLE_CD)    '敬称
                    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
                    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
                    '2012/03/28 TCS 高橋 【SALES_2】自社客 SMS　EMail 変更可 START
                    'SetCheckEnable(Me.smsCheckButton, custDataRow, custDataRow.IsSMSFLGNull, SC3080205TableAdapter.IdSmsFlg)   'SMS
                    'SetCheckEnable2(Me.emailCheckButton, custDataRow, custDataRow.EMAILFLG, SC3080205TableAdapter.IdEmailFlg) 'EMail	
                    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START  
                    Me.smsCheckButton.Enabled = False              'SMS                  フラグの状態に関わらず編集不可
                    Me.emailCheckButton.Enabled = False            'EMail                フラグの状態に関わらず編集不可
                    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END 
                    '2012/03/28 TCS 高橋 【SALES_2】自社客 SMS　EMail 変更可 END
                    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
                    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
                    Me.dmailCheckButton.Enabled = False
                    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
                    '商業情報受取区分
                    If Not custDataRow.IsIsReadOnly_COMMERCIAL_RECV_TYPENull Then
                        Me.commercialRecvType_Empty.Enabled = Not custDataRow.IsReadOnly_COMMERCIAL_RECV_TYPE
                        Me.commercialRecvType_Empty.Enabled = Not custDataRow.IsReadOnly_COMMERCIAL_RECV_TYPE
                        Me.commercialRecvType_Empty.Enabled = Not custDataRow.IsReadOnly_COMMERCIAL_RECV_TYPE
                    End If
                    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END
                    '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
                    SetTextEnable(Me.incomeTextBox, custDataRow, custDataRow.CST_INCOME, SC3080205TableAdapter.IdCstIncome) '収入
                    '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
                    '2012/03/08 TCS 安田 【SALES_2】性能改善 START
                    '活動区分使用許可区分
                    Me.useActvctgryHidden.Value = "0"
                    '2012/03/08 TCS 安田 【SALES_2】性能改善 END

                    'ログ出力 Start ***************************************************************************
                    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditInitialize OrgCust SetEnable End")
                    'ログ出力 End *****************************************************************************

                End If
                Me.actvctgryPanel.Visible = False  '活動区分
                'Me.dmailPanel.Visible = False     'D-Mail
            End If

            '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
            '電話番号検索ボタン制御
            '非活性
            Me.mobileSerchButtonImage.Visible = False
            Me.telnoSerchButtonImage.Visible = False
            'テキストボックスの幅を調整
            Me.mobileTextBox.Width = 340
            Me.telnoTextBox.Width = 340
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            '国民ID検索ボタン制御
            '非活性
            Me.socialIdSearchButtonImage.Visible = False
            'テキストボックスの幅を調整
            Me.socialidTextBox.Width = 340
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
            '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

            '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
            Me.nameBeforeHidden.Value = custDataRow.NAME    '氏名(変更確認用)

            If String.IsNullOrEmpty(Trim(custDataRow.DUMMYNAMEFLG)) Then
                Me.dummyNameFlgHidden.Value = SC3080205TableAdapter.DummyNameFlgOfficial   'ダミー名称フラグ
            Else
                Me.dummyNameFlgHidden.Value = custDataRow.DUMMYNAMEFLG  'ダミー名称フラグ
            End If

            '画面に値を設定する
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            'CustomerAreaSetting(custDataRow, wordList, nameTitleList)
            CustomerAreaSetting(custDataRow, wordList, nameTitleList, privateFleetItemList, stateList, districtList, cityList, locationList, custSubCtgry2List)
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
            
            '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditInitialize Mode Edit End")
            'ログ出力 End *****************************************************************************
        End If

        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
        '顧客検索画面からの戻りの場合、画面を復元
        If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHFLG) = True Then
            '顧客検索画面からの戻りの最初の一回のみ以下の処理を実行
            If CType(GetValue(ScreenPos.Current, SESSION_KEY_SERCHFLG, False), Integer) = 1 And _
                mode = SC3080205BusinessLogic.ModeCreate Then
                'セッションより画面に表示していた値を取得
                Me.GetSession(custDataRow)

                '2014/06/20 TCS 市川  【A STEP2】電話番号検索不具合対応（TR-V4-GTMC140617004）の横転 START
                custDataRow.CST_DLR_ROW_LOCK_VERSION = 0
                '2014/06/20 TCS 市川  【A STEP2】電話番号検索不具合対応（TR-V4-GTMC140617004）の横転 END

                '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
                Dim stateExistFlg As Boolean = False
                Dim districtExistFlg As Boolean = False
                Dim cityExistFlg As Boolean = False
                '地域リスト取得(州がセットされていて、マスタに存在する場合のみ)
                If (Not String.IsNullOrEmpty(custDataRow.ADDRESS_STATE)) Then
                    For Each stateRow In stateList
                        If (custDataRow.ADDRESS_STATE.Trim.Equals(stateRow.STATE_CD.Trim)) Then
                            stateExistFlg = True
                        End If
                    Next
                End If
                Dim districtList As SC3080205DataSet.SC3080205DistrictDataTable
                If (stateExistFlg = True) Then
                    districtList = SC3080205BusinessLogic.GetDistrict(custDataRow.ADDRESS_STATE, msgID)
                Else
                    districtList = New SC3080205DataSet.SC3080205DistrictDataTable()
                End If
                districtRepeater.DataSource = districtList
                districtRepeater.DataBind()

                '市リスト取得(地域がセットされていて、マスタに存在する場合のみ)
                If (Not String.IsNullOrEmpty(custDataRow.ADDRESS_DISTRICT)) Then
                    For Each districtRow In districtList
                        If (custDataRow.ADDRESS_DISTRICT.Trim.Equals(districtRow.DISTRICT_CD.Trim)) Then
                            districtExistFlg = True
                        End If
                    Next
                End If
                Dim cityList As SC3080205DataSet.SC3080205CityDataTable
                If (stateExistFlg = True And districtExistFlg = True) Then
                    cityList = SC3080205BusinessLogic.GetCity(custDataRow.ADDRESS_STATE, custDataRow.ADDRESS_DISTRICT, msgID)
                Else
                    cityList = New SC3080205DataSet.SC3080205CityDataTable()
                End If
                cityRepeater.DataSource = cityList
                cityRepeater.DataBind()

                '地区リスト取得(市がセットされていて、マスタに存在する場合のみ)
                If (Not String.IsNullOrEmpty(custDataRow.ADDRESS_CITY)) Then
                    For Each cityRow In cityList
                        If (custDataRow.ADDRESS_CITY.Trim.Equals(cityRow.CITY_CD.Trim)) Then
                            cityExistFlg = True
                        End If
                    Next
                End If
                Dim locationList As SC3080205DataSet.SC3080205LocationDataTable
                If (stateExistFlg = True And districtExistFlg = True And cityExistFlg = True) Then
                    locationList = SC3080205BusinessLogic.GetLocation(custDataRow.ADDRESS_STATE, custDataRow.ADDRESS_DISTRICT, _
                                                                      custDataRow.ADDRESS_CITY, msgID)
                Else
                    locationList = New SC3080205DataSet.SC3080205LocationDataTable()
                End If
                locationRepeater.DataSource = locationList
                locationRepeater.DataBind()
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

                '画面を復元
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
                'Me.CustomerAreaSetting(custDataRow, wordList, nameTitleList)
                Me.CustomerAreaSetting(custDataRow, wordList, nameTitleList, privateFleetItemList, stateList, districtList, cityList, locationList, custSubCtgry2List)
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END

                '電話番号検索フラグ初期化
                Me.RemoveValueBypass(ScreenPos.Current, SESSION_KEY_SERCHFLG)
            End If
        ElseIf mode = SC3080205BusinessLogic.ModeCreate Then
            '画面を初期化
            CustomerAreaClear()
        End If
        '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START  
        Me.smsPanel.Visible = False
        Me.emailPanel.Visible = False
        Me.dmailPanel.Visible = False
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END  

        'SMS配信可否	SMS使用可否フラグ（０：非表示、１：表示）
        'e-mail配信可否	e-mail使用可否フラグ（０：非表示、１：表示）
        'D-mail配信可否	D-mail使用可否フラグ（０：非表示、１：表示）
        If (Me.smsPanel.Visible = False And _
            Me.emailPanel.Visible = False And _
            Me.dmailPanel.Visible = False) Then
            Me.rmmPanel.Visible = False
        End If

        '表示フラグ項目を設定する
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL  
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END  
        Me.postsrhFlgHidden.Value = CType(custDataRow.POSTSRHFLG, String)
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL  
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END  
        Me.orginputFlgHidden.Value = CType(custDataRow.ORGINPUTFLG, String)

        '住所検索ボタンの活性制御をする
        Call SetEnabled()

        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        '顧客メモの情報をセッションに保持する
        'Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CUSTSEGMENT, CType(custDataRow.CUSTFLG, String))    '顧客区分 (1：自社客 / 2：未取引客)
        'Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CUSTOMERCLASS, "1")                                 '顧客分類 (1：所有者 / 2：使用者 / 3：その他)
        'If (mode = SC3080205BusinessLogic.ModeEdit) Then
        '    '活動先顧客コード
        '    If (custDataRow.CUSTFLG = SC3080205BusinessLogic.OrgCustFlg) Then
        '        '０：自社客
        '        Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CRCUSTID, custDataRow.ORIGINALID)
        '    Else
        '        '１：未顧客
        '        Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CRCUSTID, custDataRow.CSTID)
        '    End If

        '    Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CRCUSTNAME, Me.nameTextBox.Text)                '活動先顧客名
        'End If
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END

        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        ''顧客情報－変更前情報の保持
        'Call SetCustomerBackHidden()

        'カスタムコントロール設定
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'Dim sexCheckBoxScript As String = "$('#manCheckBox').CheckMark({ ""label"": """ & JavaScriptStringEncode(40008) & """, ""checkIconPosition"": ""left"", ""offIconUrl"": """", ""onIconUrl"": """" });" & _
        '                                  "$('#girlCheckBox').CheckMark({ ""label"": """ & JavaScriptStringEncode(40009) & """, ""checkIconPosition"": ""left"", ""offIconUrl"": """", ""onIconUrl"": """" });"
        Dim sexCheckBoxScript As String = "$('#manCheckBox').CheckMark({ ""label"": """ & JavaScriptStringEncode(40008) & """, ""checkIconPosition"": ""left"", ""offIconUrl"": """", ""onIconUrl"": """" });" & _
                                          "$('#girlCheckBox').CheckMark({ ""label"": """ & JavaScriptStringEncode(40009) & """, ""checkIconPosition"": ""left"", ""offIconUrl"": """", ""onIconUrl"": """" });" & _
                                          "$('#otherCheckBox').CheckMark({ ""label"": """ & JavaScriptStringEncode(40051) & """, ""checkIconPosition"": ""left"", ""offIconUrl"": """", ""onIconUrl"": """" });" & _
                                          "$('#unknownCheckBox').CheckMark({ ""label"": """ & JavaScriptStringEncode(40052) & """, ""checkIconPosition"": ""left"", ""offIconUrl"": """", ""onIconUrl"": """" });"
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END


        '2012/03/28 TCS 高橋 【SALES_2】 START
        '男入力可否設定
        If manCheckBox.Enabled = False Then
            sexCheckBoxScript &= "$('#manCheckBox').CheckMark('disabled', true);"
        End If
        '女入力可否設定
        If girlCheckBox.Enabled = False Then
            sexCheckBoxScript &= "$('#girlCheckBox').CheckMark('disabled', true);"
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'その他入力可否設定
        If otherCheckBox.Enabled = False Then
            sexCheckBoxScript &= "$('#otherCheckBox').CheckMark('disabled', true);"
        End If
        '不明入力可否設定
        If unknownCheckBox.Enabled = False Then
            sexCheckBoxScript &= "$('#unknownCheckBox').CheckMark('disabled', true);"
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        '2012/03/28 TCS 高橋 【SALES_2】 END
        Dim customerTypeCheckBoxScript As String = "$('#kojinCheckBox').CheckMark({ ""label"": """ & JavaScriptStringEncode(40011) & """, ""checkIconPosition"": ""left"", ""offIconUrl"": """", ""onIconUrl"": """" });" & _
                                                   "$('#houjinCheckBox').CheckMark({ ""label"": """ & JavaScriptStringEncode(40012) & """, ""checkIconPosition"": ""left"", ""offIconUrl"": """", ""onIconUrl"": """" });"
        '2012/03/28 TCS 高橋 【SALES_2】 START

        '2018/04/24 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        If Not (isPfTypeAvailable()) Then
            '2015/04/02 TCS 外崎 セールスタブレット:M014 START
            '個人／法人は選択不可（TMTでは、セールスタブレットは個人客のみ運用している）
            kojinCheckBox.Enabled = False
            houjinCheckBox.Enabled = False
            '2015/04/02 TCS 外崎 セールスタブレット:M014 END
            '2020/01/20 TS 岩田 TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045) START
        Else
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) = True) Then
                Dim cstid As String = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                Me.setKojinHoujinEnabled(cstid, custDataRow.CUSTYPE)
            End If
            '2020/01/20 TS 岩田 TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045) END
        End If
        '2018/04/24 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        '個人入力可否設定
        If kojinCheckBox.Enabled = False Then
            customerTypeCheckBoxScript &= "$('#kojinCheckBox').CheckMark('disabled', true);"
        End If
        '法人入力可否設定
        If houjinCheckBox.Enabled = False Then
            customerTypeCheckBoxScript &= "$('#houjinCheckBox').CheckMark('disabled', true);"
        End If
        '2012/03/28 TCS 高橋 【SALES_2】 END

        Dim smsCheckBoxScript As String = String.Empty
        Dim emailCheckBoxScript As String = String.Empty
        Dim dmailCheckBoxScript As String = String.Empty
        If rmmPanel.Visible = True Then
            If smsPanel.Visible = True Then

                smsCheckBoxScript = "$('#smsCheckButton').CheckMark({ ""label"": """ & JavaScriptStringEncode(40042) & """, ""checkIconPosition"": ""left"", ""offIconUrl"": """", ""onIconUrl"": """" });"
                If smsCheckButton.Enabled = False Then
                    smsCheckBoxScript = smsCheckBoxScript & "$('#smsCheckButton').CheckMark('disabled', true);"
                End If
            End If
            If emailPanel.Visible = True Then
                emailCheckBoxScript = "$('#emailCheckButton').CheckMark({ ""label"": """ & JavaScriptStringEncode(40043) & """, ""checkIconPosition"": ""left"", ""offIconUrl"": """", ""onIconUrl"": """" });"
                If emailCheckButton.Enabled = False Then
                    emailCheckBoxScript = emailCheckBoxScript & "$('#emailCheckButton').CheckMark('disabled', true);"
                End If
            End If
            If dmailPanel.Visible = True Then
                dmailCheckBoxScript = "$('#dmailCheckButton').CheckMark({ ""label"": """ & JavaScriptStringEncode(40044) & """, ""checkIconPosition"": ""left"", ""offIconUrl"": """", ""onIconUrl"": """" });"
                If dmailCheckButton.Enabled = False Then
                    dmailCheckBoxScript = dmailCheckBoxScript & "$('#dmailCheckButton').CheckMark('disabled', true);"
                End If
            End If
        End If

        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
        '商業情報受取区分
        Dim commercialRecvTypeScript As New System.Text.StringBuilder(3000)
        commercialRecvTypeScript.Append(createICROPCheckMarkInitScript(Me.commercialRecvType_Empty, COMMERCIAL_RECV_TYPE_EMPTY))
        commercialRecvTypeScript.Append(createICROPCheckMarkInitScript(Me.commercialRecvType_Yes, COMMERCIAL_RECV_TYPE_YES))
        commercialRecvTypeScript.Append(createICROPCheckMarkInitScript(Me.commercialRecvType_No, COMMERCIAL_RECV_TYPE_NO))

        Me.cust_flg_hidden.Value = CStr(custDataRow.CUSTFLG)

        JavaScriptUtility.RegisterStartupScript(Me.Page, " <script>" & "$(function() {" & sexCheckBoxScript & customerTypeCheckBoxScript & smsCheckBoxScript & emailCheckBoxScript & dmailCheckBoxScript & commercialRecvTypeScript.ToString() & "});" & "</script>", "after")
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END

        JavaScriptUtility.RegisterStartupScript(Me.Page, " <script>" & "$(function() { $('#actvctgryLabel').CustomLabel({ ""useEllipsis"": ""true"" }); });" & "</script>", "after3")

        '2012/04/27 TCS 安田 【SALES_2】HTMLEncode対応 END

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditInitialize End")
        'ログ出力 End *****************************************************************************

    End Sub

    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
    ''' <summary>
    ''' CheckMarkコントロールの初期化
    ''' </summary>
    ''' <param name="check">初期化するCheckMarkコントロール</param>
    ''' <param name="value">CheckMarkコントロールに設定するvalue属性</param>
    ''' <returns>初期化スクリプト</returns>
    ''' <remarks>コントロールがDisabledの場合、無効化スクリプトも自動付与します</remarks>
    Private Function createICROPCheckMarkInitScript(ByRef check As CheckMark, Optional ByVal value As String = "") As String
        Const initScript As String = "$('#{0}').CheckMark({{ 'label': '{1}', 'checkIconPosition': 'left', 'offIconUrl': '', 'onIconUrl': '' }})"
        Const disableScript As String = ".CheckMark('disabled', true)"
        Dim ret As New System.Text.StringBuilder(1000)

        'コントロール初期化メソッド
        ret.AppendFormat(initScript, check.ClientID, JavaScriptStringEncode(CInt(check.TextWordNo.ToString())))
        'コントロール無効化メソッド
        If Not Me.commercialRecvType_Empty.Enabled Then ret.Append(disableScript)
        If value <> String.Empty Then ret.AppendFormat(".attr('value','{0}')", value)
        ret.AppendLine(";")

        Return ret.ToString()
    End Function
    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END

    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 画面項目の表示・入力必須制御をする
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetDispSetting()

        Dim msgID As Integer = 0

        '隠しフィールドの初期値設定
        Me.inputSettingHidden01.Value = String.Empty
        Me.inputSettingHidden02.Value = String.Empty
        Me.inputSettingHidden03.Value = String.Empty
        Me.inputSettingHidden04.Value = String.Empty
        Me.inputSettingHidden05.Value = String.Empty
        Me.inputSettingHidden06.Value = String.Empty
        Me.inputSettingHidden07.Value = String.Empty
        Me.inputSettingHidden08.Value = String.Empty
        Me.inputSettingHidden09.Value = String.Empty
        Me.inputSettingHidden10.Value = String.Empty
        Me.inputSettingHidden11.Value = String.Empty
        Me.inputSettingHidden12.Value = String.Empty
        Me.inputSettingHidden13.Value = String.Empty
        Me.inputSettingHidden14.Value = String.Empty
        Me.inputSettingHidden15.Value = String.Empty
        Me.inputSettingHidden16.Value = String.Empty
        Me.inputSettingHidden17.Value = String.Empty
        Me.inputSettingHidden18.Value = String.Empty
        Me.inputSettingHidden19.Value = String.Empty
        Me.inputSettingHidden20.Value = String.Empty
        Me.inputSettingHidden21.Value = String.Empty
        Me.inputSettingHidden22.Value = String.Empty
        Me.inputSettingHidden23.Value = String.Empty
        Me.inputSettingHidden24.Value = String.Empty
        Me.inputSettingHidden25.Value = String.Empty
        Me.inputSettingHidden26.Value = String.Empty
        Me.inputSettingHidden27.Value = String.Empty
        Me.inputSettingHidden28.Value = String.Empty
        Me.inputSettingHidden29.Value = String.Empty
        '2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 START
        Me.inputSettingHidden36.Value = String.Empty
        '2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 END

        '入力項目設定リスト取得
        Dim inputSettingList As SC3080205DataSet.SC3080205InputItemSettingDataTable = _
            SC3080205BusinessLogic.GetInputItemSetting(msgID)

        '取得した入力項目設定を隠しフィールドにセット
        For Each inputSettingRow In inputSettingList
            Select Case inputSettingRow.TGT_ITEM_ID
                Case TGT_ITEM_ID_FIRSTNAME
                    Me.inputSettingHidden01.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_MIDDLENAME
                    Me.inputSettingHidden02.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_LASTNAME
                    Me.inputSettingHidden03.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_SEX
                    Me.inputSettingHidden04.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_NAMETITLE
                    Me.inputSettingHidden05.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_CUSTYPE
                    Me.inputSettingHidden06.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_PRIVATE_FLEET_ITEM_CD
                    Me.inputSettingHidden07.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_EMPLOYEENAME
                    Me.inputSettingHidden08.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_EMPLOYEEDEPARTMENT
                    Me.inputSettingHidden09.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_EMPLOYEEPOSITION
                    Me.inputSettingHidden10.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_MOBILE
                    Me.inputSettingHidden11.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_TELNO
                    Me.inputSettingHidden12.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_BUSINESSTELNO
                    Me.inputSettingHidden13.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_FAXNO
                    Me.inputSettingHidden14.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_ZIPCODE
                    Me.inputSettingHidden15.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_ADDRESS1
                    Me.inputSettingHidden16.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_ADDRESS2
                    Me.inputSettingHidden17.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_ADDRESS3
                    Me.inputSettingHidden18.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_ADDRESS_STATE
                    Me.inputSettingHidden19.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_ADDRESS_DISTRICT
                    Me.inputSettingHidden20.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_ADDRESS_CITY
                    Me.inputSettingHidden21.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_ADDRESS_LOCATION
                    Me.inputSettingHidden22.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_DOMICILE
                    Me.inputSettingHidden23.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_EMAIL1
                    Me.inputSettingHidden24.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_EMAIL2
                    Me.inputSettingHidden25.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_COUNTRY
                    Me.inputSettingHidden26.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_SOCIALID
                    Me.inputSettingHidden27.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_BIRTHDAY
                    Me.inputSettingHidden28.Value = inputSettingRow.DISP_SETTING_STATUS
                Case TGT_ITEM_ID_ACTVCTGRYID
                    Me.inputSettingHidden29.Value = inputSettingRow.DISP_SETTING_STATUS
                    '2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 START
                Case TGT_ITEM_ID_COMMERCIAL_RECV_TYPE
                    Me.inputSettingHidden36.Value = inputSettingRow.DISP_SETTING_STATUS
                    '2014/10/02 TCS 河原 商業情報必須指定解除のタブレット側対応 END
            End Select
        Next

    End Sub
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
    ''' <summary>
    ''' 画面の項目を設定
    ''' </summary>
    ''' <param name="custDataRow">画面表示用データ</param>
    ''' <param name="wordList">活動区分</param>
    ''' <param name="nameTitleList">敬称リスト</param>
    ''' <param name="privateFleetItemList">個人法人項目リスト</param>
    ''' <param name="stateList">州リスト</param>
    ''' <param name="districtList">地域リスト</param>
    ''' <param name="cityList">市リスト</param>
    ''' <param name="locationList">地域リスト</param>
    ''' <remarks></remarks>
    Private Sub CustomerAreaSetting(ByVal custDataRow As SC3080205DataSet.SC3080205CustRow, _
                                    ByVal wordList As List(Of String), _
                                    ByVal nameTitleList As SC3080205DataSet.SC3080205NameTitleDataTable, _
                                    ByVal privateFleetItemList As SC3080205DataSet.SC3080205PrivateFleetItemDataTable, _
                                    ByVal stateList As SC3080205DataSet.SC3080205StateDataTable, _
                                    ByVal districtList As SC3080205DataSet.SC3080205DistrictDataTable, _
                                    ByVal cityList As SC3080205DataSet.SC3080205CityDataTable, _
                                    ByVal LocationList As SC3080205DataSet.SC3080205LocationDataTable,
                                    ByVal custSubCtgry2List As SC3080205DataSet.SC3080205CustSubCtgry2DataTable)
        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
        Dim msgID As Integer = 0
        Dim word As String

        '2018/04/24 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        If "1".Equals(Me.Use_Customerdata_Cleansing_Flg.Value) And SC3080205BusinessLogic.Houjin.Equals(custDataRow.CUSTYPE) Then
            'クレンジング機能使用可の場合で、かつ、個人法人区分が”1”(法人)の場合

            'ファーストネーム
            Me.nameTextBox.Text = custDataRow.NAME
            'ミドルネーム
            Me.middleNameTextBox.Text = String.Empty
            'ラストネーム
            Me.lastNameTextBox.Text = String.Empty
        Else
            '上記以外の場合

            '値を設定する
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            '2016/11/28 TCS 曽出 （トライ店システム評価）基幹連携に伴う顧客車両情報管理機能評価　【TR-V4-TMT-20160623-001】START
            Dim firstName As String = custDataRow.FIRSTNAME
            Dim middleName As String = custDataRow.MIDDLENAME
            Dim lastName As String = custDataRow.LASTNAME
            Dim nameFML As String = firstName + middleName + lastName
            Dim name As String = custDataRow.NAME

            'ブランクの場合、半角スペースを取り除く処理はしない
            If (String.Empty.Equals(name) = False) Then
                name = Replace(custDataRow.NAME, " ", "")
            End If

            'ブランクの場合、半角スペースを取り除く処理はしない
            If (String.Empty.Equals(nameFML) = False) Then
                nameFML = Replace(nameFML, " ", "")
            End If

            'ファーストネーム、ミドルネーム、ラストネームと氏名が等しい場合、各DB値をファーストネーム、ミドルネーム、ラストネームに表示
            If (Not name.Equals(nameFML)) Then
                Dim names As String() = custDataRow.NAME.Split(New Char() {" "c})
                'ファーストネーム
                If (names.Length >= 1) Then
                    Me.nameTextBox.Text = names(0)
                Else
                    Me.nameTextBox.Text = String.Empty
                End If
                'ミドルネーム
                If (names.Length >= 2) Then
                    Me.middleNameTextBox.Text = names(1)
                Else
                    Me.middleNameTextBox.Text = String.Empty
                End If
                'ラストネーム
                '2015/06/08 TCS 中村 TMT課題対応(#2) START
                If (names.Length >= 3) Then
                    Dim sb As New System.Text.StringBuilder
                    sb.Append(names(2))
                    For i = 3 To names.Length - 1
                        sb.Append(New Char() {" "c})
                        sb.Append(names(i))
                    Next
                    Me.lastNameTextBox.Text = sb.ToString()
                Else
                    Me.lastNameTextBox.Text = String.Empty
                End If
                '2015/06/08 TCS 中村 TMT課題対応(#2) END
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
            Else
                Me.nameTextBox.Text = firstName
                Me.middleNameTextBox.Text = middleName
                Me.lastNameTextBox.Text = lastName
            End If
            '2016/11/28 TCS 曽出 （トライ店システム評価）基幹連携に伴う顧客車両情報管理機能評価　【TR-V4-TMT-20160623-001】END
        End If
        '2018/04/24 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        '敬称
        '敬称リスト
        If (Not String.IsNullOrEmpty(custDataRow.NAMETITLE_CD)) Then
            For Each nameRow In nameTitleList
                If (custDataRow.NAMETITLE_CD.Trim.Equals(nameRow.NAMETITLE_CD.Trim)) Then
                    Me.nameTitle.Text = nameRow.NAMETITLE
                    Me.nameTitleTextHidden.Value = nameRow.NAMETITLE
                End If
            Next
        End If

        Me.nameTitleHidden.Value = custDataRow.NAMETITLE_CD

        '敬称コードがない場合に、敬称をそのまま出力する
        If (String.IsNullOrEmpty(custDataRow.NAMETITLE_CD) = True) Then
            Me.nameTitle.Text = custDataRow.NAMETITLE
            Me.nameTitleTextHidden.Value = custDataRow.NAMETITLE
        End If

        '0:男性、1:女性
        If (custDataRow.SEX.Equals(SC3080205BusinessLogic.Otoko)) Then
            Me.manCheckBox.Checked = True  '男
        End If
        If (custDataRow.SEX.Equals(SC3080205BusinessLogic.Onna)) Then
            Me.girlCheckBox.Checked = True  '女
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        '3:その他、2:不明
        If (custDataRow.SEX.Equals(SC3080205BusinessLogic.Other)) Then
            Me.otherCheckBox.Checked = True  'その他
        End If
        If (custDataRow.SEX.Equals(SC3080205BusinessLogic.Unknown)) Then
            Me.unknownCheckBox.Checked = True  '不明
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        '0:法人 1:個人																				
        If (custDataRow.CUSTYPE.Equals(SC3080205BusinessLogic.Kojin)) Then
            Me.kojinCheckBox.Checked = True  '個人
        End If
        If (custDataRow.CUSTYPE.Equals(SC3080205BusinessLogic.Houjin)) Then
            Me.houjinCheckBox.Checked = True  '法人
        End If

        '2018/04/24 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        If Not (isPfTypeAvailable()) Then
            '2015/04/02 TCS 外崎 セールスタブレット:M014 START
            '個人／法人のどちらも選択されていない場合、個人を選択（TMTでは、セールスタブレットは個人客のみ運用している）
            If (Me.kojinCheckBox.Checked = False AndAlso Me.houjinCheckBox.Checked = False) Then
                Me.kojinCheckBox.Checked = True  '個人
            End If
            '2015/04/02 TCS 外崎 セールスタブレット:M014 END
        End If
        '2018/04/24 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        '個人法人項目リスト
        If (Not String.IsNullOrEmpty(custDataRow.PRIVATE_FLEET_ITEM_CD)) Then
            For Each privateFleetItemRow In privateFleetItemList
                If custDataRow.PRIVATE_FLEET_ITEM_CD.Trim.Equals(privateFleetItemRow.PRIVATE_FLEET_ITEM_CD.Trim) Then
                    Me.privateFleetItem.Text = privateFleetItemRow.PRIVATE_FLEET_ITEM_NAME
                    Me.privateFleetItemHidden.Value = privateFleetItemRow.PRIVATE_FLEET_ITEM_CD
                    Me.custOrgnzNameInputTypeHidden.Value = privateFleetItemRow.CST_ORGNZ_NAME_INPUT_TYPE
                    Exit For
                End If
            Next
        Else
            Me.privateFleetItem.Text = String.Empty
            Me.privateFleetItemHidden.Value = String.Empty
            Me.custOrgnzNameInputTypeHidden.Value = String.Empty
        End If
        '顧客組織名称
        If (custDataRow.CST_ORGNZ_INPUT_TYPE = "1") Then '1: マスタから選択, 2: 手入力
            If SC3080205BusinessLogic.GetCustOrgnzLocal(Me.privateFleetItemHidden.Value).Where(Function(e) e.CST_ORGNZ_CD = custDataRow.CST_ORGNZ_CD).Count = 1 Then
                Me.custOrgnz.Text = SC3080205BusinessLogic.GetCustOrgnzLocal(Me.privateFleetItemHidden.Value).Where(Function(e) e.CST_ORGNZ_CD = custDataRow.CST_ORGNZ_CD).FirstOrDefault().CST_ORGNZ_NAME
            End If
        Else
            Me.custOrgnz.Text = If(String.IsNullOrEmpty(custDataRow.CST_ORGNZ_NAME), String.Empty, custDataRow.CST_ORGNZ_NAME)
        End If
        Me.custOrgnzHidden.Value = If(String.IsNullOrEmpty(custDataRow.CST_ORGNZ_CD), String.Empty, custDataRow.CST_ORGNZ_CD)

        Me.custOrgnzInputTypeHidden.Value = If(String.IsNullOrEmpty(custDataRow.CST_ORGNZ_INPUT_TYPE), String.Empty, custDataRow.CST_ORGNZ_INPUT_TYPE)
        ' 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える START
        Me.custOrgnzNameHidden.Value = If(String.IsNullOrEmpty(Me.custOrgnz.Text), String.Empty, Me.custOrgnz.Text)
        ' 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える END
        '顧客組織名称リスト
        Me.custOrgnzRepeater.DataSource = SC3080205BusinessLogic.GetCustOrgnzLocal(Me.custOrgnz.Text, Me.privateFleetItemHidden.Value)
        Me.custOrgnzRepeater.DataBind()
        '顧客サブカテゴリ2
        If Not String.IsNullOrEmpty(custDataRow.CST_SUBCAT2_CD) Then
            ' 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える START
            Me.custSubCtgry2.Text = SC3080205BusinessLogic.GetCustSubCtgry2(Me.privateFleetItemHidden.Value, custDataRow.CST_ORGNZ_CD).First(Function(e) e.CST_SUBCAT2_CD = custDataRow.CST_SUBCAT2_CD).CST_SUBCAT2_NAME
            ' 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える END
            Me.custSubCtgry2Hidden.Value = custDataRow.CST_SUBCAT2_CD
        Else
            Me.custSubCtgry2.Text = String.Empty
            Me.custSubCtgry2Hidden.Value = String.Empty
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
        Me.employeenameTextBox.Text = custDataRow.EMPLOYEENAME        '担当者氏名
        Me.employeedepartmentTextBox.Text = custDataRow.EMPLOYEEDEPARTMENT     '担当者部署名
        Me.employeepositionTextBox.Text = custDataRow.EMPLOYEEPOSITION       '役職
        Me.mobileTextBox.Text = custDataRow.MOBILE     '携帯
        Me.telnoTextBox.Text = custDataRow.TELNO      '自宅
        Me.businesstelnoTextBox.Text = custDataRow.BUSINESSTELNO     '勤務先
        Me.faxnoTextBox.Text = custDataRow.FAXNO     'FAX
        Me.zipcodeTextBox.Text = custDataRow.ZIPCODE      '
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        '2017/11/20 TCS 河原 TKM独自機能開発 START
        'Me.addressTextBox.Text = custDataRow.ADDRESS        '住所
        Dim addresses As String()
        If (Me.addressDataCleansingHidden.Value = "1") Then

            If System.Text.RegularExpressions.Regex.IsMatch(custDataRow.ADDRESS1, ", ") Then
                'カンマ + スペースが含まれる場合、Address1を分割
                addresses = Split(custDataRow.ADDRESS1, ", ", -1, CompareMethod.Binary)
                '住所1
                If (addresses.Length >= 1) Then
                    Me.addressTextBox.Text = addresses(0)
                Else
                    Me.addressTextBox.Text = String.Empty
                End If
                '住所2
                If (addresses.Length >= 2) Then
                    Me.address2TextBox.Text = addresses(1)
                Else
                    Me.address2TextBox.Text = String.Empty
                End If
                '住所3
                If (addresses.Length >= 3) Then

                    Dim wkAddresses3 As String = String.Empty

                    For i = 2 To addresses.Length - 1
                        If i = 2 Then
                            wkAddresses3 = addresses(i)
                        Else
                            wkAddresses3 = wkAddresses3 & ", " & addresses(i)
                        End If
                    Next

                    Me.address3TextBox.Text = wkAddresses3

                Else
                    Me.address3TextBox.Text = String.Empty
                End If
            Else
                'カンマ + スペースが含まれない場合、DBのAddress1,2,3をそのままセット
                Me.addressTextBox.Text = custDataRow.ADDRESS1
                Me.address2TextBox.Text = custDataRow.ADDRESS2
                Me.address3TextBox.Text = custDataRow.ADDRESS3
            End If

        Else
            'データクレンジング実施しない場合…DBのAddress1,2,3をそのままセット
            Me.addressTextBox.Text = custDataRow.ADDRESS1
            Me.address2TextBox.Text = custDataRow.ADDRESS2
            Me.address3TextBox.Text = custDataRow.ADDRESS3
        End If
        '2017/11/20 TCS 河原 TKM独自機能開発 END

        '住所(州)リスト
        If (Not String.IsNullOrEmpty(custDataRow.ADDRESS_STATE)) Then
            For Each stateRow In stateList
                If (custDataRow.ADDRESS_STATE.Trim.Equals(stateRow.STATE_CD.Trim)) Then
                    Me.addressState.Text = stateRow.STATE_NAME
                    Me.stateHidden.Value = stateRow.STATE_CD
                End If
            Next
        Else
            Me.addressState.Text = String.Empty
            Me.stateHidden.Value = String.Empty
        End If
        '住所(地域)リスト
        If (Not String.IsNullOrEmpty(custDataRow.ADDRESS_DISTRICT)) Then
            For Each districtRow In districtList
                If (custDataRow.ADDRESS_DISTRICT.Trim.Equals(districtRow.DISTRICT_CD.Trim)) Then
                    Me.addressDistrict.Text = districtRow.DISTRICT_NAME
                    Me.districtHidden.Value = districtRow.DISTRICT_CD
                End If
            Next
        Else
            Me.addressDistrict.Text = String.Empty
            Me.districtHidden.Value = String.Empty
        End If
        '住所(市)リスト
        If (Not String.IsNullOrEmpty(custDataRow.ADDRESS_CITY)) Then
            For Each cityRow In cityList
                If (custDataRow.ADDRESS_CITY.Trim.Equals(cityRow.CITY_CD.Trim)) Then
                    Me.addressCity.Text = cityRow.CITY_NAME
                    Me.cityHidden.Value = cityRow.CITY_CD
                End If
            Next
        Else
            Me.addressCity.Text = String.Empty
            Me.cityHidden.Value = String.Empty
        End If
        '住所(地区)リスト
        If (Not String.IsNullOrEmpty(custDataRow.ADDRESS_LOCATION)) Then
            For Each locationRow In LocationList
                If (custDataRow.ADDRESS_LOCATION.Trim.Equals(locationRow.LOCATION_CD.Trim)) Then
                    Me.addressLocation.Text = locationRow.LOCATION_NAME
                    Me.locationHidden.Value = locationRow.LOCATION_CD
                End If
            Next
        Else
            Me.addressLocation.Text = String.Empty
            Me.locationHidden.Value = String.Empty
        End If
        Me.domicileTextBox.Text = custDataRow.DOMICILE     '本籍
        Me.countryTextBox.Text = custDataRow.COUNTRY     '国籍
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        Me.email1TextBox.Text = custDataRow.EMAIL1     '
        Me.email2TextBox.Text = custDataRow.EMAIL2     'EMail2
        Me.socialidTextBox.Text = custDataRow.SOCIALID       '国民ID
        If (Not custDataRow.IsBIRTHDAYNull()) Then
            Me.birthdayTextBox.Value = custDataRow.BIRTHDAY        '誕生日
        End If
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
        '商業情報受取区分
        If Not custDataRow.IsCOMMERCIAL_RECV_TYPENull Then
            getCommercialRecvTypeValue = custDataRow.COMMERCIAL_RECV_TYPE
        Else
            getCommercialRecvTypeValue = String.Empty
        End If
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
        Me.incomeTextBox.Text = custDataRow.CST_INCOME  '収入
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
        If (custDataRow.CUSTFLG = SC3080205BusinessLogic.NewCustFlg) Then
            '１：未取引客

            'ACTVCTGRYID	活動区分ID						
            'REASONID		活動除外理由ID			
            Me.actvctgryidHidden.Value = CType(SC3080205BusinessLogic.InitActvctgryId, String)       '活動区分
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            Me.actvctgryidHidden_Old.Value = CType(SC3080205BusinessLogic.InitActvctgryId, String)
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            Me.reasonidHidden.Value = String.Empty       '断念理由
            If (Not custDataRow.IsACTVCTGRYIDNull()) Then

                '文言マスタ取得
                Dim i As Integer = 0
                For i = 0 To wordList.Count - 1
                    If (custDataRow.ACTVCTGRYID = (i + 1)) Then
                        word = CType(wordList.Item(i), String)
                        Me.actvctgryLabel.Text = word
                        Me.actvctgryNameHidden.Value = word
                    End If
                Next

                Me.actvctgryidHidden.Value = CType(custDataRow.ACTVCTGRYID, String)       '活動区分
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                Me.actvctgryidHidden_Old.Value = CType(custDataRow.ACTVCTGRYID, String)
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                If (Not custDataRow.IsREASONIDNull()) Then
                    '2:情報不備
                    Dim giveupReasonList As SC3080205DataSet.SC3080205OmitreasonDataTable = _
                        SC3080205BusinessLogic.GetGiveupReason(msgID)

                    Dim giveupReasonRow As SC3080205DataSet.SC3080205OmitreasonRow
                    For i = 0 To giveupReasonList.Count - 1
                        giveupReasonRow = giveupReasonList.Item(i)
                        '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) START
                        If (giveupReasonRow.ACT_CAT_TYPE = Me.actvctgryidHidden.Value And giveupReasonRow.REASONID = custDataRow.REASONID) Then
                            '2018/11/22 TCS 三浦 活動区分「3,4」選択時、理由を選択できるようにする(TR-SLT-TMT-20180611-001) END
                            If (Not String.IsNullOrEmpty(Me.actvctgryLabel.Text)) Then
                                Me.actvctgryLabel.Text = Me.actvctgryLabel.Text & "-"
                            End If
                            Me.actvctgryLabel.Text = Me.actvctgryLabel.Text & giveupReasonRow.REASON
                            Me.reasonNameHidden.Value = giveupReasonRow.REASON
                        End If
                    Next
                    Me.reasonidHidden.Value = CType(custDataRow.REASONID, String)      '断念理由
                    ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                    Me.reasonidHidden_Old.Value = CType(custDataRow.REASONID, String)
                    ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
                End If
            End If

            Me.actvctgryLabel.Text = HttpUtility.HtmlEncode(Me.actvctgryLabel.Text)

        End If

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

        If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SERCHFLG) = False Then
            Me.updatefuncflgHidden.Value = custDataRow.UPDATEFUNCFLG  '最終更新機能
        End If

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        Me.smsCheckButton.Checked = False      'SMS
        Me.emailCheckButton.Checked = False '0:希望しない
        Me.dmailCheckButton.Checked = False     'DMail
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'Me.cusLockvrHidden.Value = CStr(custDataRow.LOCKVERSION)
        'If (custDataRow.CUSTFLG = SC3080205BusinessLogic.NewCustFlg) Then
        '    Me.cusVCLLockvrHidden.Value = CStr(custDataRow.VCLLOCKVERSION)
        'End If
        If (Not custDataRow.IsLOCKVERSIONNull()) Then
            Me.cusLockvrHidden.Value = CStr(custDataRow.LOCKVERSION)
        End If
        If (custDataRow.CUSTFLG = SC3080205BusinessLogic.NewCustFlg) Then
            If (Not custDataRow.IsVCLLOCKVERSIONNull()) Then
                Me.cusVCLLockvrHidden.Value = CStr(custDataRow.VCLLOCKVERSION)
            End If
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        '2014/06/20 TCS 市川  【A STEP2】電話番号検索不具合対応（TR-V4-GTMC140617004）の横転 START
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
        If Not custDataRow.IsCST_DLR_ROW_LOCK_VERSIONNull() Then Me.cusDLRLockvrHidden.Value = CStr(custDataRow.CST_DLR_ROW_LOCK_VERSION)
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END
        '2014/06/20 TCS 市川  【A STEP2】電話番号検索不具合対応（TR-V4-GTMC140617004）の横転 END

        ' 2018/09/05 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
        Me.CustomerLocalLockVersion.Value = CStr(custDataRow.CST_LOCAL_ROW_LOCK_VERSION)
        ' 2018/09/05 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END

    End Sub
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    ''' <summary>
    ''' 画面の項目を設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CustomerAreaClear()

        '値を設定する
        '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) START
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        If Not Me.ContainsKey(ScreenPos.Current, SESSION_KEY_TENTATIVENAME) Then
            Me.nameTextBox.Text = String.Empty        'ファーストネーム
            Me.middleNameTextBox.Text = String.Empty  'ミドルネーム
            Me.lastNameTextBox.Text = String.Empty    'ラストネーム
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) END

        '敬称
        Me.nameTitleHidden.Value = String.Empty

        '敬称コードがない場合に、敬称をそのまま出力する
        Me.nameTitle.Text = String.Empty
        Me.nameTitleTextHidden.Value = String.Empty

        '0:男性、1:女性
        Me.manCheckBox.Checked = False
        Me.girlCheckBox.Checked = False
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        '2:その他、3:不明
        Me.otherCheckBox.Checked = False
        Me.unknownCheckBox.Checked = False
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        '0:法人 1:個人
        '2015/04/02 TCS 外崎 セールスタブレット:M014 START
        '個人／法人は選択不可（TMTでは、セールスタブレットは個人客のみ運用している）
        'Me.kojinCheckBox.Checked = False
        Me.kojinCheckBox.Checked = True
        '2015/04/02 TCS 外崎 セールスタブレット:M014 END
        Me.houjinCheckBox.Checked = False
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        '個人法人項目コード
        Me.privateFleetItem.Text = String.Empty
        Me.privateFleetItemHidden.Value = String.Empty
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        Me.employeenameTextBox.Text = String.Empty         '担当者氏名
        Me.employeedepartmentTextBox.Text = String.Empty      '担当者部署名
        Me.employeepositionTextBox.Text = String.Empty        '役職
        '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) START
        If Not Me.ContainsKey(ScreenPos.Current, SESSION_KEY_TELNO) Then
            Me.mobileTextBox.Text = String.Empty      '携帯
        End If
        '2020/02/20 TS  河原 TKM Change request development for Next Gen e-CRB (CR008,CR060,CR072) END
        Me.telnoTextBox.Text = String.Empty       '自宅
        Me.businesstelnoTextBox.Text = String.Empty      '勤務先
        Me.faxnoTextBox.Text = String.Empty      'FAX
        Me.zipcodeTextBox.Text = String.Empty       '
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'Me.addressTextBox.Text = String.Empty         '住所
        Me.addressTextBox.Text = String.Empty          '住所1
        Me.address2TextBox.Text = String.Empty         '住所2
        Me.address3TextBox.Text = String.Empty         '住所3
        '住所(州)
        Me.addressState.Text = String.Empty
        Me.stateHidden.Value = String.Empty
        '住所(地域)
        Me.addressDistrict.Text = String.Empty
        Me.districtHidden.Value = String.Empty
        '住所(市)
        Me.addressCity.Text = String.Empty
        Me.cityHidden.Value = String.Empty
        '住所(地区)
        Me.addressLocation.Text = String.Empty
        Me.locationHidden.Value = String.Empty
        Me.domicileTextBox.Text = String.Empty     '本籍
        Me.countryTextBox.Text = String.Empty     '国籍
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        Me.email1TextBox.Text = String.Empty      '
        Me.email2TextBox.Text = String.Empty      'EMail2
        Me.socialidTextBox.Text = String.Empty        '国民ID
        Me.birthdayTextBox.Value = Nothing         '誕生日
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
        '商業情報受取区分
        Me.commercialRecvType_Empty.Checked = False
        Me.commercialRecvType_Yes.Checked = False
        Me.commercialRecvType_No.Checked = False
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
        Me.incomeTextBox.Text = String.Empty    '顧客収入
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
        '活動区分の初期値セット
        Me.actvctgryidHidden.Value = CType(SC3080205BusinessLogic.InitActvctgryId, String)
        Me.actvctgryLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(SC3080205BusinessLogic.ActvctgryidDisplay + SC3080205BusinessLogic.ActvctgryidDisplay1))
        Me.actvctgryNameHidden.Value = Me.actvctgryLabel.Text                 '活動区分名称
        Me.reasonNameHidden.Value = String.Empty                              '活動断念理由名称
        Me.reasonidHidden.Value = String.Empty

        Me.smsCheckButton.Checked = False      'SMS
        Me.emailCheckButton.Checked = False     'email
        ' 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える START
        Me.cstOrgnzNameRefType.Value = String.Empty
        Me.custOrgnzHidden.Value = String.Empty
        Me.custOrgnzNameInputTypeHidden.Value = String.Empty
        Me.custOrgnzInputTypeHidden.Value = String.Empty
        Me.custSubCtgry2Hidden.Value = String.Empty
        Me.custOrgnzNameHidden.Value = String.Empty
        Me.custOrgnz.Text = String.Empty
        Me.custSubCtgry2.Text = String.Empty
        ' 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える END
    End Sub
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

    '2012/04/27 TCS 安田 【SALES_2】HTMLEncode対応 START
    ''' <summary>
    ''' JavaScriptStringEncodeをする。
    ''' </summary>
    ''' <param name="wordid">文言ID</param>
    ''' <returns>JavaScriptStringEncodeした文字列</returns>
    ''' <remarks></remarks>
    Private Function JavaScriptStringEncode(ByVal wordid As Integer) As String

        Dim script As New StringBuilder
        script.Append(HttpUtility.JavaScriptStringEncode(DirectCast(WebWordUtility.GetWord(wordid), String).Replace("""", ""))).Append(" ")

        Return script.ToString

    End Function
    '2012/04/27 TCS 安田 【SALES_2】HTMLEncode対応 END

    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
    ' ''' <summary>
    ' ''' 顧客情報－変更前情報の保持。
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Protected Sub SetCustomerBackHidden()

    '    '変更前情報の保持
    '    Me.nameTextBoxBackHidden.Value = Me.nameTextBox.Text                    '氏名
    '    Me.nameTitleHiddenBackHidden.Value = Me.nameTitleHidden.Value           '敬称コード
    '    Me.nameTitleTextHiddenBackHidden.Value = Me.nameTitleTextHidden.Value   '敬称名称
    '    If (Me.manCheckBox.Checked = True) Then '男
    '        Me.manCheckBoxBackHidden.Value = "on"
    '    Else
    '        Me.manCheckBoxBackHidden.Value = String.Empty
    '    End If
    '    If (Me.girlCheckBox.Checked = True) Then    '女
    '        Me.girlCheckBoxBackHidden.Value = "on"
    '    Else
    '        Me.girlCheckBoxBackHidden.Value = String.Empty
    '    End If
    '    If (Me.kojinCheckBox.Checked = True) Then   '個人
    '        Me.kojinCheckBoxBackHidden.Value = "on"
    '    Else
    '        Me.kojinCheckBoxBackHidden.Value = String.Empty
    '    End If
    '    If (Me.houjinCheckBox.Checked = True) Then  '法人
    '        Me.houjinCheckBoxBackHidden.Value = "on"
    '    Else
    '        Me.houjinCheckBoxBackHidden.Value = String.Empty
    '    End If
    '    Me.employeenameTextBoxBackHidden.Value = Me.employeenameTextBox.Text            '担当者氏名
    '    Me.employeedepartmentTextBoxBackHidden.Value = Me.employeedepartmentTextBox.Text    '担当者部署名
    '    Me.employeepositionTextBoxBackHidden.Value = Me.employeepositionTextBox.Text    '役職
    '    Me.mobileTextBoxBackHidden.Value = Me.mobileTextBox.Text                        '携帯
    '    Me.telnoTextBoxBackHidden.Value = Me.telnoTextBox.Text                          '自宅
    '    Me.businesstelnoTextBoxBackHidden.Value = Me.businesstelnoTextBox.Text          '勤務先
    '    Me.faxnoTextBoxBackHidden.Value = Me.faxnoTextBox.Text                          'FAX
    '    Me.zipcodeTextBoxBackHidden.Value = Me.zipcodeTextBox.Text                      '郵便番号
    '    Me.addressTextBoxBackHiddenx.Value = Me.addressTextBox.Text                      '住所
    '    Me.email1TextBoxBackHidden.Value = Me.email1TextBox.Text                        'EMail1
    '    Me.email2TextBoxBackHidden.Value = Me.email2TextBox.Text                        'EMail2
    '    Me.socialidTextBoxBackHidden.Value = Me.socialidTextBox.Text                    '国民ID
    '    If (Me.birthdayTextBox.Value Is Nothing) Then                                      '誕生日
    '        Me.birthdayTextBoxBackHidden.Value = String.Empty
    '    Else
    '        Me.birthdayTextBoxBackHidden.Value = Format(Me.birthdayTextBox.Value, "yyyy-MM-dd")
    '    End If

    '    '未顧客時のみ活動区分を保存
    '    'If (Me.custFlgHidden.Value.Equals(CType(SC3080206BusinessLogic.NewCustflg, String))) Then
    '    Me.actvctgryidHiddenBackHidden.Value = Me.actvctgryidHidden.Value               '活動区分
    '    Me.reasonidHiddenBackHidden.Value = Me.reasonidHidden.Value                     '活動除外理由
    '    Me.actvctgryLabelBackHidden.Value = Me.actvctgryLabel.Text                      '活動除外名称

    '    Me.actvctgryNameBackHidden.Value = Me.actvctgryNameHidden.Value                 '活動区分名称
    '    Me.reasonNameBackHidden.Value = Me.reasonNameHidden.Value                       '活動断念理由名称
    '    'End If

    '    If (Me.smsCheckButton.Checked = True) Then                                      'SMS
    '        Me.smsCheckButtonBackHidden.Value = "on"
    '    Else
    '        Me.smsCheckButtonBackHidden.Value = String.Empty
    '    End If
    '    If (Me.emailCheckButton.Checked = True) Then                                    'EMail
    '        Me.emailCheckButtonBackHidden.Value = "on"
    '    Else
    '        Me.emailCheckButtonBackHidden.Value = String.Empty
    '    End If

    'End Sub
    '2012/03/08 TCS 山口 【SALES_2】性能改善 START

    ''' <summary>
    ''' セッションの値をDataRowにセットする。
    ''' </summary>
    ''' <param name="custDataRow">顧客情報DataRow</param>
    ''' <remarks></remarks>
    Protected Sub SetSessionValue(ByVal custDataRow As SC3080205DataSet.SC3080205CustRow)

        'モード (０：新規登録モード、１：編集モード)
        Dim mode As Integer = CType(Me.editModeHidden.Value, Integer)
        '自社客/未取引客フラグ (０：自社客、１：未取引客)
        Dim custflg As Short = CType(SC3080205BusinessLogic.NewCustFlg, Short)
        If (mode = SC3080205BusinessLogic.ModeEdit) Then
            '編集モード時のみ、未取引客となる場合がある
            custflg = CType(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), Short)
        End If
        '自社客連番/未取引客ユーザID
        Dim originalid As String = String.Empty
        If (CInt(Me.editModeHidden.Value) = SC3080205BusinessLogic.ModeEdit) Then
            originalid = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
        End If
        'VIN
        Dim vin As String = Me.selectVinHidden.Value

        'ログインユーザー情報取得用
        Dim context As StaffContext = StaffContext.Current

        Dim dlrcd As String = context.DlrCD         '自身の販売店コード
        Dim strcd As String = context.BrnCD         '自身の店舗コード
        Dim account As String = context.Account     '自身のアカウント
        Dim strcdstaff As String = context.BrnCD    '自身の店舗コード (スタッフ店舗コード)
        Dim staffcd As String = context.Account     '自身のアカウント (スタッフコード)

        'セッション情報のセット
        custDataRow.CUSTFLG = custflg
        If (custDataRow.CUSTFLG = SC3080205BusinessLogic.OrgCustFlg) Then
            '０：自社客
            custDataRow.ORIGINALID = originalid
        Else
            '１：未取引客
            custDataRow.CSTID = originalid
        End If
        'VIN
        custDataRow.VIN = vin
        '販売店コード
        custDataRow.DLRCD = dlrcd
        '店舗コード
        custDataRow.STRCD = strcd
        'スタッフ店舗コード
        custDataRow.STRCDSTAFF = strcdstaff
        'スタッフコード
        custDataRow.STAFFCD = staffcd
        'AC変更アカウント
        custDataRow.AC_MODFACCOUNT = account
        '更新アカウント
        custDataRow.UPDATEACCOUNT = account

        '更新： 2012/01/26 TCS 安田 【SALES_1B】来店実績より表示処理 START
        If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VISITSEQ) = True) Then
            custDataRow.VISITSEQ = CType(GetValue(ScreenPos.Current, SESSION_KEY_VISITSEQ, False), Long)
        End If
        '更新： 2012/01/26 TCS 安田 【SALES_1B】来店実績より表示処理 END

    End Sub

    ''' <summary>
    ''' 編集可能フラグを判定する (イメージボタン)。
    ''' </summary>
    ''' <param name="control">対象コントロール</param>
    ''' <param name="custDataRow">顧客情報DataRow</param>
    ''' <param name="dbValue">DB値</param>
    ''' <param name="index">項目Index (DB項目順)</param>
    ''' <remarks></remarks>
    Protected Sub GetImageButtonEnable(ByVal control As ImageButton, ByVal custDataRow As SC3080205DataSet.SC3080205CustRow, ByVal dbValue As String, ByVal index As Integer)

        If String.IsNullOrEmpty(dbValue) Then
            control.Enabled = SetEnable(custDataRow, True, index)
        Else
            control.Enabled = SetEnable(custDataRow, False, index)
        End If

    End Sub

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 編集可能フラグを判定する (テキスト入力)。
    ''' </summary>
    ''' <param name="control">対象コントロール</param>
    ''' <param name="custDataRow">顧客情報DataRow</param>
    ''' <param name="dbValue">DB値</param>
    ''' <param name="index">項目Index (DB項目順)</param>
    ''' <remarks></remarks>
    Protected Sub SetTextEnable(ByVal control As CustomTextBox, ByVal custDataRow As SC3080205DataSet.SC3080205CustRow, ByVal dbValue As String, ByVal index As Integer)

        If String.IsNullOrEmpty(dbValue) Then
            control.Enabled = SetEnable(custDataRow, True, index)
        Else
            control.Enabled = SetEnable(custDataRow, False, index)
        End If

        If String.Equals(Me.CleansingModeFlg.Value, "1") Then
            control.Enabled = True
        End If

    End Sub
    '2017/11/20 TCS 河原 TKM独自機能開発 EMD

    ''' <summary>
    ''' 編集可能フラグを判定する (日付入力)。
    ''' </summary>
    ''' <param name="control">対象コントロール</param>
    ''' <param name="custDataRow">顧客情報DataRow</param>
    ''' <param name="dbValue">DB値</param>
    ''' <param name="index">項目Index (DB項目順)</param>
    ''' <remarks></remarks>
    Protected Sub SetDateEnable(ByVal control As DateTimeSelector, ByVal custDataRow As SC3080205DataSet.SC3080205CustRow, ByVal dbValue As Boolean, ByVal index As Integer)

        control.Enabled = SetEnable(custDataRow, dbValue, index)

    End Sub

    ''' <summary>
    ''' 編集可能フラグを判定する (チェックボックス)。
    ''' </summary>
    ''' <param name="control">対象コントロール</param>
    ''' <param name="custDataRow">顧客情報DataRow</param>
    ''' <param name="dbValue">DB値がNulかどうか</param>
    ''' <param name="index">項目Index (DB項目順)</param>
    ''' <remarks></remarks>
    Protected Sub SetCheckEnable(ByVal control As CheckMark, ByVal custDataRow As SC3080205DataSet.SC3080205CustRow, ByVal dbValue As Boolean, ByVal index As Integer)

        control.Enabled = SetEnable(custDataRow, dbValue, index)

    End Sub

    ''' <summary>
    ''' 編集可能フラグを判定する (チェックボックス)。
    ''' </summary>
    ''' <param name="control">対象コントロール</param>
    ''' <param name="custDataRow">顧客情報DataRow</param>
    ''' <param name="dbValue">DB値</param>
    ''' <param name="index">項目Index (DB項目順)</param>
    ''' <remarks></remarks>
    Protected Sub SetCheckEnable2(ByVal control As CheckMark, ByVal custDataRow As SC3080205DataSet.SC3080205CustRow, ByVal dbValue As String, ByVal index As Integer)

        If String.IsNullOrEmpty(dbValue) Then
            control.Enabled = SetEnable(custDataRow, True, index)
        Else
            control.Enabled = SetEnable(custDataRow, False, index)
        End If

    End Sub

    ' ''' <summary>
    ' ''' 編集可能フラグを判定する (チェックボックス)。
    ' ''' </summary>
    ' ''' <param name="control">対象コントロール</param>
    ' ''' <param name="custDataRow">顧客情報DataRow</param>
    ' ''' <param name="dbValue">DB値がNulかどうか</param>
    ' ''' <param name="index">項目Index (DB項目順)</param>
    ' ''' <remarks></remarks>
    'Protected Sub SetCheckEnable(ByVal control As CheckBox, ByVal custDataRow As SC3080205DataSet.SC3080205CustRow, ByVal dbValue As Boolean, ByVal index As Integer)

    '    control.Enabled = SetEnable(custDataRow, dbValue, index)

    'End Sub

    ' ''' <summary>
    ' ''' 編集可能フラグを判定する (チェックボックス)。
    ' ''' </summary>
    ' ''' <param name="control">対象コントロール</param>
    ' ''' <param name="custDataRow">顧客情報DataRow</param>
    ' ''' <param name="dbValue">DB値</param>
    ' ''' <param name="index">項目Index (DB項目順)</param>
    ' ''' <remarks></remarks>
    'Protected Sub SetCheckEnable2(ByVal control As CheckBox, ByVal custDataRow As SC3080205DataSet.SC3080205CustRow, ByVal dbValue As String, ByVal index As Integer)

    '    If String.IsNullOrEmpty(dbValue) Then
    '        control.Enabled = SetEnable(custDataRow, True, index)
    '    Else
    '        control.Enabled = SetEnable(custDataRow, False, index)
    '    End If

    'End Sub

    ''' <summary>
    ''' 編集可能フラグを判定する
    ''' </summary>
    ''' <param name="custDataRow">顧客情報DataRow</param>
    ''' <param name="dbValueIsNull">DB値がNulかどうか</param>
    ''' <param name="index">項目Index (DB項目順)</param>
    ''' <remarks></remarks>
    Protected Function SetEnable(ByVal custDataRow As SC3080205DataSet.SC3080205CustRow, ByVal dbValueIsNull As Boolean, ByVal index As Integer) As Boolean

        Dim orginputflg As String = custDataRow.ORGINPUTFLG
        Dim options As String = custDataRow.UPDATEFUNCFLG
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'Dim CstEditTbl() As Integer = {0, 0, 0, 0, 8,
        '                               54, 55, 56, 0, 9,
        '                               10, 58, 13, 0, 0,
        '                               0, 0, 0, 0, 11,
        '                               0, 0, 26, 20, 0,
        '                               0, 0, 0, 0, 0,
        '                               0, 27, 29, 28, 0,
        '                               32, 30, 31, 12, 0,
        '                               0, 0, 0, 0}
        Dim CstEditTbl() As Integer = {0, 0, 0, 0, 8,
                                       54, 55, 56, 0, 9,
                                       10, 58, 13, 14, 15,
                                       16, 0, 0, 0, 11,
                                       24, 25, 26, 20, 21,
                                       22, 23, 59, 60, 61,
                                       62, 27, 29, 28, 0,
                                       32, 30, 31, 12, 40,
                                       0, 0, 0, 0, 63}
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END

        If (index > options.Length) Then
            'イレギュラーパターン
            Return False
        End If

        Dim orgch As Integer
        orgch = CType(orginputflg.Substring(CstEditTbl(index - 1) - 1, 1), Integer)

        '0（手動入力不可）
        If (orgch = 0) Then
            Return False
        End If

        '1（手動入力可）
        If (orgch = 1) Then
            Return True
        End If

        Dim optionVal As Integer
        optionVal = CType(options.Substring(index - 1, 1), Integer)

        '2（基幹連携がない場合のみ、手動入力可）
        If (orgch = 2) Then
            Select Case optionVal
                Case 0
                    '0（基幹連携なし）可
                    Return True
                Case 1
                    '1（基幹連携あり）不可
                    Return False
                Case 2
                    '2（i-CROP側で更新）可
                    Return True
            End Select
        End If

        '3（基幹連携がない、もしくは、基幹連携された値が空値の場合のみ、手動入力可）
        If (orgch = 3) Then
            Select Case optionVal
                Case 0
                    '0（基幹連携なし）可
                    Return True
                Case 1
                    '1（基幹連携あり）空値 可
                    '1（基幹連携あり）空値以外 可
                    If (dbValueIsNull = True) Then
                        Return True
                    Else
                        Return False
                    End If
                Case 2
                    '2（i-CROP側で更新）可
                    Return True
            End Select
        End If

        Return True

    End Function

    ''' <summary>
    ''' 画面の値を取得する
    ''' </summary>
    ''' <param name="custDataTbl">顧客情報DataTable</param>
    ''' <remarks></remarks>
    Protected Sub GetDisplayValues(ByVal custDataTbl As SC3080205DataSet.SC3080205CustDataTable)

        Dim custDataRow As SC3080205DataSet.SC3080205CustRow = custDataTbl.NewSC3080205CustRow

        'セッション内の値をセットする
        Me.SetSessionValue(custDataRow)

        '値を設定する
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'custDataRow.NAME = Me.nameTextBox.Text       '氏名
        '氏名
        '2018/04/24 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
        If "1".Equals(Me.Use_Customerdata_Cleansing_Flg.Value) And Me.houjinCheckBox.Checked = True Then
            'クレンジング機能が使用可で、かつ、法人にチェックが入っている場合
            '顧客氏名にファーストネームを設定
            '2019/01/23 TS 三浦 UAT-0659 START
            'custDataRow.NAME = Me.nameTextBox.Text & " "
            custDataRow.NAME = Me.nameTextBox.Text
            custDataRow.FIRSTNAME = Me.nameTextBox.Text        'ファーストネーム
            '2019/01/23 TS 三浦 UAT-0659 END
        Else
            '上記以外の場合
            '顧客氏名にファーストネーム＋ミドルネーム＋ラストネームを設定
            custDataRow.NAME = Me.nameTextBox.Text & " " & _
                               Me.middleNameTextBox.Text & " " & _
                               Me.lastNameTextBox.Text
            '2019/01/23 TS 三浦 UAT-0659 START
            custDataRow.FIRSTNAME = Me.nameTextBox.Text        'ファーストネーム
            custDataRow.MIDDLENAME = Me.middleNameTextBox.Text 'ミドルネーム
            custDataRow.LASTNAME = Me.lastNameTextBox.Text     'ラストネーム
            '2019/01/23 TS 三浦 UAT-0659 END
        End If
        '2018/04/24 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END
        '2019/01/23 TS 三浦 UAT-0659 START
        'custDataRow.FIRSTNAME = Me.nameTextBox.Text        'ファーストネーム
        'custDataRow.MIDDLENAME = Me.middleNameTextBox.Text 'ミドルネーム
        'custDataRow.LASTNAME = Me.lastNameTextBox.Text     'ラストネーム
        '2019/01/23 TS 三浦 UAT-0659 END
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        'TODO:？
        'GetVisibleFlg(Me.敬称, custDataRow.UPDATEFUNCFLG, custDataRow.ORGINPUTFLG, 13)    '敬称
        custDataRow.NAMETITLE_CD = Me.nameTitleHidden.Value       '敬称CD
        custDataRow.NAMETITLE = HttpUtility.HtmlDecode(Me.nameTitleTextHidden.Value.Trim)        '敬称

        '0:男性  1:女性			
        custDataRow.SEX = " "  '選択なし															
        If (Me.manCheckBox.Checked = True) Then
            custDataRow.SEX = SC3080205BusinessLogic.Otoko   '男
        End If
        If (Me.girlCheckBox.Checked = True) Then
            custDataRow.SEX = SC3080205BusinessLogic.Onna    '女
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        If (Me.otherCheckBox.Checked = True) Then
            custDataRow.SEX = SC3080205BusinessLogic.Other    'その他
        End If
        If (Me.unknownCheckBox.Checked = True) Then
            custDataRow.SEX = SC3080205BusinessLogic.Unknown  '不明
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        '0:法人 1:個人				
        custDataRow.CUSTYPE = " "   '選択なし
        If (Me.kojinCheckBox.Checked = True) Then
            custDataRow.CUSTYPE = SC3080205BusinessLogic.Kojin      '個人
        End If
        If (Me.houjinCheckBox.Checked = True) Then
            custDataRow.CUSTYPE = SC3080205BusinessLogic.Houjin      '法人
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        custDataRow.PRIVATE_FLEET_ITEM_CD = Me.privateFleetItemHidden.Value     '個人法人項目
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        custDataRow.EMPLOYEENAME = Me.employeenameTextBox.Text                  '担当者氏名
        custDataRow.EMPLOYEEDEPARTMENT = Me.employeedepartmentTextBox.Text      '担当者部署名
        custDataRow.EMPLOYEEPOSITION = Me.employeepositionTextBox.Text          '役職
        custDataRow.MOBILE = Me.mobileTextBox.Text                              '携帯
        custDataRow.TELNO = Me.telnoTextBox.Text                                '自宅
        custDataRow.BUSINESSTELNO = Me.businesstelnoTextBox.Text                '勤務先
        custDataRow.FAXNO = Me.faxnoTextBox.Text                                'FAX
        custDataRow.ZIPCODE = Me.zipcodeTextBox.Text                            '郵便番号
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'custDataRow.ADDRESS = Me.addressTextBox.Text                            '住所
        '住所…ここでは画面入力値をそのままセットする
        '(バリデーションチェックを行うため。後で登録用に編集する)
        custDataRow.ADDRESS1 = Me.addressTextBox.Text                           '住所1
        custDataRow.ADDRESS2 = Me.address2TextBox.Text                          '住所2
        custDataRow.ADDRESS3 = Me.address3TextBox.Text                          '住所3
        custDataRow.ADDRESS_STATE = Me.stateHidden.Value                        '住所(州)
        custDataRow.ADDRESS_DISTRICT = Me.districtHidden.Value                  '住所(地域)
        custDataRow.ADDRESS_CITY = Me.cityHidden.Value                          '住所(市)
        custDataRow.ADDRESS_LOCATION = Me.locationHidden.Value                  '住所(地区)
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        custDataRow.EMAIL1 = Me.email1TextBox.Text                              'EMail1
        custDataRow.EMAIL2 = Me.email2TextBox.Text                              'EMail2
        custDataRow.SOCIALID = Me.socialidTextBox.Text                          '国民ID
        If (Not IsNothing(Me.birthdayTextBox.Value)) Then
            custDataRow.BIRTHDAY = CType(Me.birthdayTextBox.Value, Date)        '誕生日
        End If
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        custDataRow.DOMICILE = Me.domicileTextBox.Text                          '本籍
        custDataRow.COUNTRY = Me.countryTextBox.Text                            '国籍
        custDataRow.ADDRESS_DISP_DIRECTION = Me.addressDirectionHidden.Value    '住所表示順フラグ
        custDataRow.LABEL_NAMETITLE_SETTING = Me.labelNametitleSettingHidden.Value    'ラベル・敬称表示設定フラグ
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
        custDataRow.COMMERCIAL_RECV_TYPE = getCommercialRecvTypeValue()         '商業情報受取区分
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
        custDataRow.CST_INCOME = Me.incomeTextBox.Text                          '収入
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END

        '活動区分ID
        If (String.IsNullOrEmpty(Me.actvctgryidHidden.Value)) Then
            custDataRow.SetACTVCTGRYIDNull()
        Else
            custDataRow.ACTVCTGRYID = CType(Me.actvctgryidHidden.Value, Integer)
        End If
        '活動区分変更アカウント
        'custDataRow.AC_MODFACCOUNT = " "
        '活動区分変更機能
        custDataRow.AC_MODFFUNCDVS = SC3080205BusinessLogic.ACModffuncdvsValue
        '活動除外理由ID
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        If (String.IsNullOrEmpty(Me.reasonidHidden.Value)) Then
            '       custDataRow.SetREASONIDNull()
        Else
            custDataRow.REASONID = Me.reasonidHidden.Value
        End If
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

        custDataRow.UPDATEFUNCFLG = Me.updatefuncflgHidden.Value  '最終更新機能



        '表示フラグ項目を設定する
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        If (String.IsNullOrEmpty(Me.postsrhFlgHidden.Value) = False) Then
            custDataRow.POSTSRHFLG = CType(Me.postsrhFlgHidden.Value, Short)
        End If
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        If (String.IsNullOrEmpty(Me.orginputFlgHidden.Value) = False) Then
            custDataRow.ORGINPUTFLG = Me.orginputFlgHidden.Value
        End If

        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        If (String.IsNullOrEmpty(Me.cusLockvrHidden.Value) = False) Then
            custDataRow.LOCKVERSION = CType(Me.cusLockvrHidden.Value, Long)
        End If
        If (custDataRow.CUSTFLG <> SC3080205BusinessLogic.OrgCustFlg) Then
            If (String.IsNullOrEmpty(Me.cusVCLLockvrHidden.Value) = False) Then
                custDataRow.VCLLOCKVERSION = CType(Me.cusVCLLockvrHidden.Value, Long)
            End If
        End If
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
        If (String.IsNullOrEmpty(Me.cusDLRLockvrHidden.Value) = False) Then
            custDataRow.CST_DLR_ROW_LOCK_VERSION = CType(Me.cusDLRLockvrHidden.Value, Long)
        End If
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END

        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
        custDataRow.CST_ORGNZ_CD = Me.custOrgnzHidden.Value
        custDataRow.CST_ORGNZ_INPUT_TYPE = Me.custOrgnzInputTypeHidden.Value
        If (Me.custOrgnzInputTypeHidden.Value = "1") Then '1: マスタから選択, 2: 手入力
            custDataRow.CST_ORGNZ_NAME = String.Empty
        Else
            custDataRow.CST_ORGNZ_NAME = Me.custOrgnz.Text
        End If
        custDataRow.CST_SUBCAT2_CD = Me.custSubCtgry2Hidden.Value
        custDataRow.CST_LOCAL_ROW_LOCK_VERSION = If(String.IsNullOrEmpty(Me.CustomerLocalLockVersion.Value), -1, CLng(Me.CustomerLocalLockVersion.Value))
        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END

        custDataTbl.Rows.Add(custDataRow)

    End Sub

    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
    ''' <summary>
    ''' 商業情報受取区分コントロール値取得/設定
    ''' </summary>
    ''' <value>商業情報受取区分値</value>
    ''' <returns>商業情報受取区分値</returns>
    ''' <remarks></remarks>
    Private Property getCommercialRecvTypeValue() As String
        Get
            Dim ret As String = String.Empty

            '商業情報受取区分
            If Me.commercialRecvType_Yes.Checked Then
                ret = COMMERCIAL_RECV_TYPE_YES
            ElseIf Me.commercialRecvType_No.Checked Then
                ret = COMMERCIAL_RECV_TYPE_NO
            Else
                ret = COMMERCIAL_RECV_TYPE_EMPTY
            End If

            Return ret
        End Get
        Set(ByVal value As String)
            Me.commercialRecvType_Empty.Checked = COMMERCIAL_RECV_TYPE_EMPTY.Equals(value)
            Me.commercialRecvType_Yes.Checked = COMMERCIAL_RECV_TYPE_YES.Equals(value)
            Me.commercialRecvType_No.Checked = COMMERCIAL_RECV_TYPE_NO.Equals(value)
        End Set
    End Property
    '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END

    ''' <summary>
    ''' 画面の値を取得する
    ''' </summary>
    ''' <param name="updatefuncflg">最終更新機能</param>
    ''' <param name="index">文字位置</param>
    ''' <param name="ch">変更文字</param>
    ''' <remarks></remarks>
    Protected Sub UpdateOption(ByRef updatefuncflg As String, ByVal index As Integer, ByVal ch As String)

        index = index - 1

        If (updatefuncflg.Length < index) Then
            Exit Sub
        End If

        updatefuncflg = updatefuncflg.Insert(index, ch)
        updatefuncflg = updatefuncflg.Remove(index + 1, 1)

    End Sub

    ''' <summary>
    ''' 住所検索ボタンの活性制御をする
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetEnabled()

        Me.zipSerchButton.Enabled = Not String.IsNullOrEmpty(Me.zipcodeTextBox.Text)

    End Sub

    ''' <summary>Enabled=True時の文字列</summary>
    Private Const EnabledTrue As String = "1"

    ''' <summary>
    ''' 画面の可視/非可視状態を取得する
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function GetEnabledTable() As Dictionary(Of Integer, Boolean)

        Dim result As New Dictionary(Of Integer, Boolean)

        result.Add(SC3080205TableAdapter.IdName, Me.nameTextBox.Enabled)               '氏名
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        result.Add(SC3080205TableAdapter.IdFirstName, Me.nameTextBox.Enabled)           'ファーストネーム
        result.Add(SC3080205TableAdapter.IdMiddleName, Me.middleNameTextBox.Enabled)    'ミドルネーム
        result.Add(SC3080205TableAdapter.IdLastName, Me.lastNameTextBox.Enabled)        'ラストネーム
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        If (Me.useNameTitleHidden.Value.Equals(EnabledTrue)) Then
            result.Add(SC3080205TableAdapter.IdNameTitle, True)       '敬称
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            result.Add(SC3080205TableAdapter.IdNameTitlecd, True)       '敬称コード
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        Else
            result.Add(SC3080205TableAdapter.IdNameTitle, False)      '敬称
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            result.Add(SC3080205TableAdapter.IdNameTitlecd, False)       '敬称コード
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        End If
        'result.Add(SC3080205DataTableTableAdapter.IdNameTitle, Me.nameTitleButton.Enabled)      '敬称氏名 TODO
        result.Add(SC3080205TableAdapter.IdSex, Me.manCheckBox.Enabled)                  '性別
        result.Add(SC3080205TableAdapter.IdCustype, Me.kojinCheckBox.Enabled)            '顧客タイプ
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        If (Me.usePrivateFleetItemHidden.Value.Equals(EnabledTrue)) Then
            result.Add(SC3080205TableAdapter.IdPrivateFleetItem, True)       '個人法人項目
        Else
            result.Add(SC3080205TableAdapter.IdPrivateFleetItem, False)      '個人法人項目
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        result.Add(SC3080205TableAdapter.IdEmployeeName, Me.employeenameTextBox.Enabled)   '担当者氏名 
        result.Add(SC3080205TableAdapter.IdEmployeeDepartment, Me.employeedepartmentTextBox.Enabled) '担当者部署名 
        result.Add(SC3080205TableAdapter.IdEmployeePosition, Me.employeepositionTextBox.Enabled)   '役職 
        result.Add(SC3080205TableAdapter.IdMobile, Me.mobileTextBox.Enabled)             '携帯
        result.Add(SC3080205TableAdapter.Idtelno, Me.telnoTextBox.Enabled)              '自宅
        result.Add(SC3080205TableAdapter.IdBusinessTelno, Me.businesstelnoTextBox.Enabled)    '勤務先
        result.Add(SC3080205TableAdapter.IdFaxno, Me.faxnoTextBox.Enabled)               'FAX
        result.Add(SC3080205TableAdapter.IdZipcode, Me.zipcodeTextBox.Enabled)        '郵便番号
        result.Add(SC3080205TableAdapter.IdAddress, Me.addressTextBox.Enabled)            '住所
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        result.Add(SC3080205TableAdapter.IdAddress1, Me.addressTextBox.Enabled)           '住所1
        result.Add(SC3080205TableAdapter.IdAddress2, Me.address2TextBox.Enabled)          '住所2
        result.Add(SC3080205TableAdapter.IdAddress3, Me.address3TextBox.Enabled)          '住所3
        If (Me.useStateHidden.Value.Equals(EnabledTrue)) Then
            result.Add(SC3080205TableAdapter.IdState, True)       '住所(州)
        Else
            result.Add(SC3080205TableAdapter.IdState, False)      '住所(州)
        End If
        If (Me.useDistrictHidden.Value.Equals(EnabledTrue)) Then
            result.Add(SC3080205TableAdapter.IdDistrict, True)       '住所(地域)
        Else
            result.Add(SC3080205TableAdapter.IdDistrict, False)      '住所(地域)
        End If
        If (Me.useCityHidden.Value.Equals(EnabledTrue)) Then
            result.Add(SC3080205TableAdapter.IdCity, True)       '住所(市)
        Else
            result.Add(SC3080205TableAdapter.IdCity, False)      '住所(市)
        End If
        If (Me.useLocationHidden.Value.Equals(EnabledTrue)) Then
            result.Add(SC3080205TableAdapter.IdLocation, True)       '住所(地区)
        Else
            result.Add(SC3080205TableAdapter.IdLocation, False)      '住所(地区)
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        result.Add(SC3080205TableAdapter.IdEmail1, Me.email1TextBox.Enabled)           'EMail1
        result.Add(SC3080205TableAdapter.IdEmail2, Me.email2TextBox.Enabled)           'EMail2
        result.Add(SC3080205TableAdapter.IdSocialid, Me.socialidTextBox.Enabled)         '国民ID
        result.Add(SC3080205TableAdapter.IdBirthday, Me.birthdayTextBox.Enabled)         '誕生日
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'result.Add(SC3080205TableAdapter.IdNameTitlecd, Me.nameTitle.Enabled)       '敬称
        result.Add(SC3080205TableAdapter.IdDomicile, Me.domicileTextBox.Enabled)      '本籍
        result.Add(SC3080205TableAdapter.IdCountry, Me.countryTextBox.Enabled)        '国籍
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
        result.Add(SC3080205TableAdapter.IdCstIncome, Me.incomeTextBox.Enabled) '顧客収入
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END

        If (Me.useActvctgryHidden.Value.Equals(EnabledTrue)) Then
            result.Add(SC3080205TableAdapter.IdACtvctgryid, True)   'AC
            result.Add(SC3080205TableAdapter.IdACModfaccount, True)   'AC変更アカウント
            result.Add(SC3080205TableAdapter.IdACModffuncDvs, True)   'AC変更機能
            result.Add(SC3080205TableAdapter.IdReasonId, True)   '活動除外理由ID
        Else
            result.Add(SC3080205TableAdapter.IdACtvctgryid, False)   'AC
            result.Add(SC3080205TableAdapter.IdACModfaccount, False)   'AC変更アカウント
            result.Add(SC3080205TableAdapter.IdACModffuncDvs, False)   'AC変更機能
            result.Add(SC3080205TableAdapter.IdReasonId, False)   '活動除外理由ID
        End If
        Return result

    End Function

    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    'Private Sub SetSession(ByVal telNo As String)
    ''' <summary>
    ''' 顧客検索一覧遷移前のセッション設定
    ''' </summary>
    ''' <param name="telNo">携帯番号or電話番号or国民ID</param>
    ''' <param name="searchType">検索タイプ</param>
    ''' <remarks></remarks>
    Private Sub SetSession(ByVal telNo As String, ByVal searchType As Integer)
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        'Next
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SERCHSTRING, telNo) '検索文字列
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'Me.SetValue(ScreenPos.Next, SESSION_KEY_SERCHTYPE, 4) '検索タイプ (4: 電話番号/携帯番号)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SERCHTYPE, searchType) '検索タイプ
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SERCHDIRECTION, 3) '検索方向 (3:完全一致)
        Me.SetValue(ScreenPos.Next, SESSION_KEY_SERCHFLG, 1) '電話番号検索フラグ
        If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VISITSEQ) = True Then
            '来店実績連番が存在する場合
            Dim visitSeq As Long = CType(GetValue(ScreenPos.Current, SESSION_KEY_VISITSEQ, False), Long)
            Me.SetValue(ScreenPos.Next, SESSION_KEY_VISITSEQ, visitSeq) '来店実績連番

            '来店人数
            If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_WALKINNUM) = True Then
                Dim walkinName As Integer = CType(GetValue(ScreenPos.Current, SESSION_KEY_WALKINNUM, False), Integer)
                SetValue(ScreenPos.Next, SESSION_KEY_WALKINNUM, walkinName)
            End If
        End If

        'Current
        Me.SetValue(ScreenPos.Current, SESSION_KEY_SERCHFLG, 1) '電話番号検索フラグ
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_SOCIALID, socialidTextBox.Text) '国民ID、免許証番号等
        If houjinCheckBox.Checked = True Then
            Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CUSTYPE, SC3080205BusinessLogic.Houjin) '個人/法人区分（0:法人）
        ElseIf kojinCheckBox.Checked = True Then
            Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CUSTYPE, SC3080205BusinessLogic.Kojin) '個人/法人区分（1:個人）
        Else
            Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CUSTYPE, " ") '個人/法人区分（未入力）
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_NAME, nameTextBox.Text) '顧客氏名
        '氏名
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_NAME,
                           nameTextBox.Text & " " & _
                           middleNameTextBox.Text & " " & _
                           lastNameTextBox.Text)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_FIRSTNAME, nameTextBox.Text) 'ファーストネーム
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_MIDDLENAME, middleNameTextBox.Text) 'ミドルネーム
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_LASTNAME, lastNameTextBox.Text) 'ラストネーム
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_NAMETITLE_CD, nameTitleHidden.Value) '敬称コード
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_NAMETITLE, nameTitleTextHidden.Value) '敬称
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_ZIPCODE, zipcodeTextBox.Text) '郵便番号
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_ADDRESS, addressTextBox.Text) '住所
        '住所1
        Dim wkAddress As String = String.Empty
        wkAddress = Me.addressTextBox.Text
        If (Not (String.IsNullOrEmpty(Me.address2TextBox.Text) And String.IsNullOrEmpty(Me.address3TextBox.Text))) Then
            wkAddress = wkAddress & ", " & Me.address2TextBox.Text
            If (Not String.IsNullOrEmpty(Me.address3TextBox.Text)) Then
                wkAddress = wkAddress & ", " & Me.address3TextBox.Text
            End If
        End If
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_ADDRESS1, wkAddress)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_STATE, stateHidden.Value) '住所(州)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_DISTRICT, districtHidden.Value) '住所(地域)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CITY, cityHidden.Value) '住所(市)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_LOCATION, locationHidden.Value) '住所(地区)
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_TELNO, telnoTextBox.Text) '自宅電話番号
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_MOBILE, mobileTextBox.Text) '携帯電話番号
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_FAXNO, faxnoTextBox.Text) 'FAX番号
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_BUSINESSTELNO, businesstelnoTextBox.Text) '勤務地電話番号
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_EMAIL1, email1TextBox.Text) 'E-mailアドレス１
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_EMAIL2, email2TextBox.Text) 'E-mailアドレス２
        If manCheckBox.Checked = True Then
            Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_SEX, SC3080205BusinessLogic.Otoko) '性別（0:男）
        ElseIf girlCheckBox.Checked = True Then
            Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_SEX, SC3080205BusinessLogic.Onna) '性別（1:女）
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        ElseIf otherCheckBox.Checked = True Then
            Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_SEX, SC3080205BusinessLogic.Other) '性別（3:その他）
        ElseIf unknownCheckBox.Checked = True Then
            Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_SEX, SC3080205BusinessLogic.Unknown) '性別（2:不明）
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        Else
            Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_SEX, " ") '性別（未入力）
        End If
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_BIRTHDAY, Me.birthdayHidden.Value) '生年月日
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START END
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_EMPLOYEENAME, employeenameTextBox.Text) '担当者氏名（法人）
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_EMPLOYEEDEPARTMENT, employeedepartmentTextBox.Text) '担当者部署名（法人）
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_EMPLOYEEPOSITION, employeepositionTextBox.Text) '役職（法人）
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_ACTVCTGRYID, actvctgryidHidden.Value)  '活動区分ID
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_COMMRECVTYPE, getCommercialRecvTypeValue())  '商業情報区分
        '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_INCOME, incomeTextBox.Text)    '収入
        '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_REASONID, reasonidHidden.Value) '活動除外理由ID
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_DOMICILE, domicileTextBox.Text) '本籍
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_COUNTRY, countryTextBox.Text) '国籍
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_PRIVATE_FLEET_ITEM_CD, privateFleetItemHidden.Value) '個人法人項目コード
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_LOCKVERSION, cusLockvrHidden.Value) '行ロックバージョン
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_VCLLOCKVERSION, cusVCLLockvrHidden.Value) '行ロックバージョン(車両)
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        '2014/06/20 TCS 市川  【A STEP2】電話番号検索不具合対応（TR-V4-GTMC140617004）の横転 START
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTDLRLOCKVERSION, cusDLRLockvrHidden.Value) '行ロックバージョン(販売店顧客)
        '2014/06/20 TCS 市川  【A STEP2】電話番号検索不具合対応（TR-V4-GTMC140617004）の横転 END

        ' 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える START
        '2018/12/13 TCS 中村(拓) TKM SIT-0170 START
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTORGNZCD, Me.custOrgnzHidden.Value)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTORGNZNAME, Me.custOrgnzNameHidden.Value)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTORGNZINPUTTYPE, Me.custOrgnzInputTypeHidden.Value)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTSUBCAT2CD, Me.custSubCtgry2Hidden.Value)
        Me.SetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTLOCALLOCKVERSION, Me.CustomerLocalLockVersion.Value)
        '2018/12/13 TCS 中村(拓) TKM SIT-0170 END
        ' 2019/04/08 TS 舩橋 POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える END

    End Sub

    ''' <summary>
    ''' 顧客検索一覧遷移後のセッション情報取得
    ''' </summary>
    ''' <param name="custDataRow"></param>
    ''' <remarks></remarks>
    Private Sub GetSession(ByVal custDataRow As SC3080205DataSet.SC3080205CustRow)

        '値を設定する
        custDataRow.NAME = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_NAME, False), String) '氏名
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        custDataRow.FIRSTNAME = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_FIRSTNAME, False), String) 'ファーストネーム
        custDataRow.MIDDLENAME = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_MIDDLENAME, False), String) 'ミドルネーム
        custDataRow.LASTNAME = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_LASTNAME, False), String) 'ラストネーム
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        custDataRow.NAMETITLE_CD = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_NAMETITLE_CD, False), String) '敬称CD
        custDataRow.NAMETITLE = HttpUtility.HtmlDecode(CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_NAMETITLE, False), String)) '敬称
        '0:男性  1:女性  " ":選択なし
        custDataRow.SEX = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_SEX, False), String)
        '0:法人 1:個人  " ":選択なし
        custDataRow.CUSTYPE = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CUSTYPE, False), String)
        custDataRow.EMPLOYEENAME = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_EMPLOYEENAME, False), String) '担当者氏名
        custDataRow.EMPLOYEEDEPARTMENT = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_EMPLOYEEDEPARTMENT, False), String) '担当者部署名
        custDataRow.EMPLOYEEPOSITION = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_EMPLOYEEPOSITION, False), String) '役職
        custDataRow.MOBILE = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_MOBILE, False), String) '携帯
        custDataRow.TELNO = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_TELNO, False), String) '自宅
        custDataRow.BUSINESSTELNO = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_BUSINESSTELNO, False), String) '勤務先
        custDataRow.FAXNO = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_FAXNO, False), String) 'FAX
        custDataRow.ZIPCODE = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_ZIPCODE, False), String) '郵便番号
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'custDataRow.ADDRESS = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_ADDRESS, False), String) '住所
        custDataRow.ADDRESS1 = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_ADDRESS1, False), String) '住所1
        custDataRow.ADDRESS_STATE = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_STATE, False), String) '住所(州)
        custDataRow.ADDRESS_DISTRICT = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_DISTRICT, False), String) '住所(地域)
        custDataRow.ADDRESS_CITY = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CITY, False), String) '住所(市)
        custDataRow.ADDRESS_LOCATION = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_LOCATION, False), String) '住所(地区)
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        custDataRow.EMAIL1 = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_EMAIL1, False), String) 'EMail1
        custDataRow.EMAIL2 = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_EMAIL2, False), String) 'EMail2
        custDataRow.SOCIALID = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_SOCIALID, False), String) '国民ID
        '誕生日
        If String.IsNullOrEmpty(CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_BIRTHDAY, False), String)) = False Then
            custDataRow.BIRTHDAY = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_BIRTHDAY, False), Date)
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        custDataRow.DOMICILE = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_DOMICILE, False), String) '本籍
        custDataRow.COUNTRY = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_COUNTRY, False), String) '国籍
        custDataRow.PRIVATE_FLEET_ITEM_CD = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_PRIVATE_FLEET_ITEM_CD, False), String) '個人法人項目
        If String.IsNullOrEmpty(CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_LOCKVERSION, False), String)) = False Then
            custDataRow.LOCKVERSION = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_LOCKVERSION, False), Long) '行ロックバージョン
        End If
        If String.IsNullOrEmpty(CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_VCLLOCKVERSION, False), String)) = False Then
            custDataRow.VCLLOCKVERSION = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_VCLLOCKVERSION, False), Long) '行ロックバージョン(車両)
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START DEL  
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END  

        '2014/06/20 TCS 市川  【A STEP2】電話番号検索不具合対応（TR-V4-GTMC140617004）の横転 START
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CUSTEDIT_COMMRECVTYPE) _
            AndAlso Not String.IsNullOrEmpty(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_COMMRECVTYPE, False).ToString()) Then
            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
            getCommercialRecvTypeValue = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_COMMRECVTYPE, False), String)  '商業情報区分
            '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) END
        End If
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTDLRLOCKVERSION) _
            AndAlso Not String.IsNullOrEmpty(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTDLRLOCKVERSION, False).ToString()) Then
            custDataRow.CST_DLR_ROW_LOCK_VERSION = 0
            Long.TryParse(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTDLRLOCKVERSION, False).ToString(), custDataRow.CST_DLR_ROW_LOCK_VERSION) '販売店顧客行ロック番号
        End If
        '2014/06/20 TCS 市川  【A STEP2】電話番号検索不具合対応（TR-V4-GTMC140617004）の横転 END        '活動区分ID
        If String.IsNullOrEmpty(CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_ACTVCTGRYID, False), String)) Then
            custDataRow.SetACTVCTGRYIDNull()
        Else
            custDataRow.ACTVCTGRYID = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_ACTVCTGRYID, False), Integer)
        End If
        '活動除外理由ID
        If String.IsNullOrEmpty(CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_REASONID, False), String)) Then
            custDataRow.SetREASONIDNull()
        Else
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START  
            custDataRow.REASONID = CStr(CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_REASONID, False), Integer))
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END  
        End If

        '2018/12/13 TCS 中村(拓) TKM SIT-0170 START
        custDataRow.CST_ORGNZ_CD = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTORGNZCD, False), String)
        custDataRow.CST_ORGNZ_NAME = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTORGNZNAME, False), String)
        custDataRow.CST_ORGNZ_INPUT_TYPE = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTORGNZINPUTTYPE, False), String)
        custDataRow.CST_SUBCAT2_CD = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTSUBCAT2CD, False), String)
        ' 2019/04/08 TS 舩橋 TKM POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える START DEL
        ' 2019/04/08 TS 舩橋 TKM POST-UAT3110 電話番号検索すると、サブカテゴリ２が消える END
        custDataRow.CST_LOCAL_ROW_LOCK_VERSION = 0
        If String.IsNullOrEmpty(CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTLOCALLOCKVERSION, False), String)) = False Then
            custDataRow.CST_LOCAL_ROW_LOCK_VERSION = CType(GetValue(ScreenPos.Current, SESSION_KEY_CUSTEDIT_CSTLOCALLOCKVERSION, False), Long)
        End If
        '2018/12/13 TCS 中村(拓) TKM SIT-0170 END

    End Sub
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

    '2017/11/16 TCS 河原 TKM独自機能開発 START
    Private Property CleansingErrorNo As Integer
    Private Property CleansingRslt As String

    ''' <summary>
    ''' お客様情報クレンジングチェック処理
    ''' </summary>
    ''' <returns>チェック結果 OK:True / NG:False</returns>
    ''' <remarks>お客様情報クレンジング判定を実施</remarks>
    Private Function CustomerDataCleansing() As Boolean

        'エラーNo初期化
        CleansingErrorNo = 0

        'クレンジング結果を初期化
        CleansingRslt = "0"
        Using custDataTbl As New SC3080205DataSet.SC3080205CustDataTable
            Dim custDataRow As SC3080205DataSet.SC3080205CustRow
            '画面の値を取得する
            Me.GetDisplayValues(custDataTbl)
            custDataRow = custDataTbl.Item(0)

            'ファーストネームが入力されていること
            If String.IsNullOrEmpty(Trim(custDataRow.FIRSTNAME)) Then
                CleansingErrorNo = 40995
                CleansingRslt = "1"
                Return False
            End If

            If String.Equals(custDataRow.CUSTYPE, SC3080205BusinessLogic.Kojin) Then
                '＜個人の場合＞
                'ファーストネームにブランクが入力されていないこと
                If System.Text.RegularExpressions.Regex.IsMatch(custDataRow.FIRSTNAME, "[ ]") Then
                    CleansingErrorNo = 40996
                    CleansingRslt = "1"
                    Return False
                End If

                'ファーストネームがアルファベット、"."、"/"、"&"、"("、")"、","、"-"のみで入力されていること
                If Not System.Text.RegularExpressions.Regex.IsMatch(custDataRow.FIRSTNAME, "^[a-zA-Z\./&\(\),\-]+$") Then
                    CleansingErrorNo = 40997
                    CleansingRslt = "1"
                    Return False
                End If

                'ファーストネームの1文字目がアルファベットであること
                If Not System.Text.RegularExpressions.Regex.IsMatch(custDataRow.FIRSTNAME(0), "[a-zA-Z]") Then
                    CleansingErrorNo = 40999
                    CleansingRslt = "1"
                    Return False
                End If

                'ミドルネームにブランクが入力されていないこと
                If System.Text.RegularExpressions.Regex.IsMatch(custDataRow.MIDDLENAME, "[ ]") Then
                    CleansingErrorNo = 41000
                    CleansingRslt = "1"
                    Return False
                End If

                'ミドルネームがアルファベット、"."、"/"、"&"、"("、")"、","、"-"のみで入力されていること、ただし記号は先頭での使用不可
                If Not String.IsNullOrEmpty(Trim(custDataRow.MIDDLENAME)) Then
                    If Not System.Text.RegularExpressions.Regex.IsMatch(custDataRow.MIDDLENAME, "^[a-zA-Z\./&\(\),\-]+$") Then
                        CleansingErrorNo = 41001
                        CleansingRslt = "1"
                        Return False
                    End If

                    'ミドルネームの1文字目がアルファベットであること
                    If Not System.Text.RegularExpressions.Regex.IsMatch(custDataRow.MIDDLENAME(0), "[a-zA-Z]") Then
                        CleansingErrorNo = 41002
                        CleansingRslt = "1"
                        Return False
                    End If

                    'ミドルネームにファーストネームと同じ内容が入力されていないこと
                    If String.Equals(custDataRow.FIRSTNAME, custDataRow.MIDDLENAME) Then
                        CleansingErrorNo = 41005
                        CleansingRslt = "1"
                        Return False
                    End If
                End If

                'ラストネームがアルファベット、"."、"/"、"&"、"("、")"、","、"-"のみで入力されていること、ただし記号は先頭での使用不可
                If Not String.IsNullOrEmpty(Trim(custDataRow.LASTNAME)) Then
                    If Not System.Text.RegularExpressions.Regex.IsMatch(custDataRow.LASTNAME, "^[a-zA-Z\./&\(\),\- ]+$") Then
                        CleansingErrorNo = 41003
                        CleansingRslt = "1"
                        Return False
                    End If

                    'ラストネームの1文字目がアルファベットであること
                    If Not System.Text.RegularExpressions.Regex.IsMatch(custDataRow.LASTNAME(0), "[a-zA-Z]") Then
                        CleansingErrorNo = 41004
                        CleansingRslt = "1"
                        Return False
                    End If

                    'ラストネームにファーストネームと同じ内容が入力されていないこと
                    If String.Equals(custDataRow.FIRSTNAME, custDataRow.LASTNAME) Then
                        CleansingErrorNo = 41005
                        CleansingRslt = "1"
                        Return False
                    End If
                End If
            ElseIf String.Equals(custDataRow.CUSTYPE, SC3080205BusinessLogic.Houjin) Then
                '＜法人の場合＞
                'ファーストネームがアルファベット、数値、"."、"/"、"&"、"("、")"、","、"-"のみで入力されていること
                If Not System.Text.RegularExpressions.Regex.IsMatch(custDataRow.FIRSTNAME, "^[a-zA-Z0-9\./&\(\),\- ]+$") Then
                    CleansingErrorNo = 40998
                    CleansingRslt = "1"
                    Return False
                End If
            End If

            '敬称が選択されていること
            If String.IsNullOrEmpty(custDataRow.NAMETITLE_CD) Then
                CleansingErrorNo = 41006
                CleansingRslt = "1"
                Return False
            End If

            '2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 DELETE

            '＜自宅電話番号が入力されている場合チェックを実施＞
            If Not String.IsNullOrEmpty(custDataRow.TELNO) Then
                '電話番号欄が数値とハイフンのみであること
                If Not System.Text.RegularExpressions.Regex.IsMatch(custDataRow.TELNO, "^[0-9\-]+$") Then
                    CleansingErrorNo = 41008
                    CleansingRslt = "1"
                    Return False
                End If

                'ハイフンが含まれていること
                If Not System.Text.RegularExpressions.Regex.IsMatch(custDataRow.TELNO, "-") Then
                    CleansingErrorNo = 41008
                    CleansingRslt = "1"
                    Return False
                End If

                Dim tel_std As String
                Dim tel_number As String

                Dim stdIndex As Integer
                stdIndex = custDataRow.TELNO.IndexOf("-")

                tel_std = custDataRow.TELNO.Substring(0, stdIndex)
                tel_number = custDataRow.TELNO.Substring(stdIndex + 1, custDataRow.TELNO.Length - stdIndex - 1)

                '市外局番が3～7桁であること
                If tel_std.Length < 3 Or tel_std.Length > 7 Then
                    CleansingErrorNo = 41009
                    CleansingRslt = "1"
                    Return False
                End If

                '電話番号が5～10桁であること
                If tel_number.Length < 5 Or tel_number.Length > 10 Then
                    CleansingErrorNo = 41010
                    CleansingRslt = "1"
                    Return False
                End If

            End If

            '＜携帯電話番号が入力されている場合チェックを実施＞
            If Not String.IsNullOrEmpty(custDataRow.MOBILE) Then
                '携帯電話番号が数値のみであること
                If Not System.Text.RegularExpressions.Regex.IsMatch(custDataRow.MOBILE, "^[0-9]+$") Then
                    CleansingErrorNo = 41011
                    CleansingRslt = "1"
                    Return False
                End If

                '携帯電話番号が10～13桁であること
                If custDataRow.MOBILE.Length < 10 Or custDataRow.MOBILE.Length > 13 Then
                    CleansingErrorNo = 41012
                    CleansingRslt = "1"
                    Return False
                End If
            End If

            '電話番号か携帯電話番号のどちらかが入力されていること
            If String.IsNullOrEmpty(custDataRow.TELNO) And String.IsNullOrEmpty(custDataRow.MOBILE) Then
                CleansingErrorNo = 41013
                CleansingRslt = "1"
                Return False
            End If

            '個人の場合のみ表示されるエリアへのチェック
            If custDataRow.CUSTFLG = 2 Then
                If Not String.IsNullOrEmpty(custDataRow.ADDRESS1) Then
                    '住所1にカンマ+スペースが入力されていないこと
                    If System.Text.RegularExpressions.Regex.IsMatch(custDataRow.ADDRESS1, ", ") Then
                        CleansingErrorNo = 41016
                        CleansingRslt = "1"
                        Return False
                    End If
                End If

                '住所1に禁則文字が使用されていないこと
                If (Validation.IsValidString(custDataRow.ADDRESS1) = False) Then
                    CleansingErrorNo = 40942
                    CleansingRslt = "1"
                    Return False
                End If

                If Not String.IsNullOrEmpty(custDataRow.ADDRESS2) Then
                    '住所2にカンマ+スペースが入力されていないこと
                    If System.Text.RegularExpressions.Regex.IsMatch(custDataRow.ADDRESS2, ", ") Then
                        CleansingErrorNo = 41017
                        CleansingRslt = "1"
                        Return False
                    End If
                End If

                '住所2に禁則文字が使用されていないこと
                If (Validation.IsValidString(custDataRow.ADDRESS2) = False) Then
                    CleansingErrorNo = 40943
                    CleansingRslt = "1"
                    Return False
                End If

                '住所3に禁則文字が使用されていないこと
                If (Validation.IsValidString(custDataRow.ADDRESS3) = False) Then
                    CleansingErrorNo = 40944
                    CleansingRslt = "1"
                    Return False
                End If

                '住所1～3の合計が256文字以下であること(登録時に区切り文字を追加するため、その4文字を除いた分でチェックする)
                If Not String.IsNullOrEmpty(custDataRow.ADDRESS1) And Not Validation.IsCorrectDigit(custDataRow.ADDRESS1 & custDataRow.ADDRESS2 & custDataRow.ADDRESS3, 252) Then
                    CleansingErrorNo = 41024
                    CleansingRslt = "1"
                    Return False
                End If

                '住所2が入力されている場合、住所1が入力されていること
                If Not String.IsNullOrEmpty(custDataRow.ADDRESS2) And String.IsNullOrEmpty(custDataRow.ADDRESS1) Then
                    CleansingErrorNo = 41018
                    CleansingRslt = "1"
                    Return False
                End If

                '住所3が入力されている場合、住所2が入力されていること
                If Not String.IsNullOrEmpty(custDataRow.ADDRESS3) And String.IsNullOrEmpty(custDataRow.ADDRESS2) Then
                    CleansingErrorNo = 41019
                    CleansingRslt = "1"
                    Return False
                End If

                '住所1～3と州、地区、市、地域の合計が320バイト以下であること
                If Not String.IsNullOrEmpty(custDataRow.ADDRESS1) Then
                    Dim wkAddress As String = String.Empty
                    wkAddress = CreateAddressText(custDataRow.ADDRESS1, custDataRow.ADDRESS2, custDataRow.ADDRESS3, custDataRow.ADDRESS_STATE, _
                                                  custDataRow.ADDRESS_DISTRICT, custDataRow.ADDRESS_CITY, custDataRow.ADDRESS_LOCATION)


                    If Not Validation.IsCorrectDigit(wkAddress, 320) Then
                        CleansingErrorNo = 41025
                        CleansingRslt = "1"
                        Return False
                    End If
                End If

                '住所1が入力されている場合、州が選択されていること
                If Not String.IsNullOrEmpty(custDataRow.ADDRESS1) And String.IsNullOrEmpty(custDataRow.ADDRESS_STATE) Then
                    CleansingErrorNo = 41020
                    CleansingRslt = "1"
                    Return False
                End If

                '住所1が入力されている場合、地区が選択されていること
                If Not String.IsNullOrEmpty(custDataRow.ADDRESS1) And String.IsNullOrEmpty(custDataRow.ADDRESS_DISTRICT) Then
                    CleansingErrorNo = 41021
                    CleansingRslt = "1"
                    Return False
                End If

                '住所1が入力されている場合、市が選択されていること
                If Not String.IsNullOrEmpty(custDataRow.ADDRESS1) And String.IsNullOrEmpty(custDataRow.ADDRESS_CITY) Then
                    CleansingErrorNo = 41022
                    CleansingRslt = "1"
                    Return False
                End If

                '住所1が入力されている場合、地域が選択されていること
                If Not String.IsNullOrEmpty(custDataRow.ADDRESS1) And String.IsNullOrEmpty(custDataRow.ADDRESS_LOCATION) Then
                    CleansingErrorNo = 41023
                    CleansingRslt = "1"
                    Return False
                End If

                '郵便番号が入力されていること
                If String.IsNullOrEmpty(custDataRow.ZIPCODE) Then
                    CleansingErrorNo = 41014
                    CleansingRslt = "1"
                    Return False
                End If

                '郵便番号が数字のみであること
                If Not System.Text.RegularExpressions.Regex.IsMatch(custDataRow.ZIPCODE, "^[0-9]+$") Then
                    CleansingErrorNo = 41015
                    CleansingRslt = "1"
                    Return False
                End If

                '郵便番号が32桁以下であること
                If custDataRow.ZIPCODE.Length > 32 Then
                    CleansingErrorNo = 41015
                    CleansingRslt = "1"
                    Return False
                End If
            End If
        End Using

        Return True
    End Function
    '2017/11/16 TCS 河原 TKM独自機能開発 END

#End Region

#Region "イベント 顧客情報編集"

    ''' <summary>
    ''' 完了ボタンクリック時。
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function CustomerEditKanryo() As Integer


        Dim ret As Integer = 0
        Dim msgID As Integer = 0
        Dim bizClass As New SC3080205BusinessLogic
        Using custDataTbl As New SC3080205DataSet.SC3080205CustDataTable

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditKanryo Start")
            'ログ出力 End *****************************************************************************

            Dim custDataRow As SC3080205DataSet.SC3080205CustRow

            'モード (０：新規登録モード、１：編集モード)
            Dim mode As Integer = CType(Me.editModeHidden.Value, Integer)

            '画面の可視/非可視状態
            Dim enabledTable As Dictionary(Of Integer, Boolean) = New Dictionary(Of Integer, Boolean)

            '画面の値を取得する
            Me.GetDisplayValues(custDataTbl)
            custDataRow = custDataTbl.Item(0)

            '画面の可視/非可視状態を取得する
            enabledTable = GetEnabledTable()

            '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            '新規登録かつ氏名が未入力の場合
            'If mode = SC3080205BusinessLogic.ModeCreate And String.IsNullOrEmpty(custDataRow.NAME) Then
            '    '氏名に月日時分（ダミー名称）をセット
            '    custDataRow.NAME = Format(DateTimeFunc.Now(StaffContext.Current.DlrCD), "MMddHHmm")
            '新規登録かつファーストネームが未入力の場合
            If mode = SC3080205BusinessLogic.ModeCreate And String.IsNullOrEmpty(custDataRow.FIRSTNAME) Then
                'ファーストネームに月日時分（ダミー名称）をセット
                custDataRow.FIRSTNAME = Format(DateTimeFunc.Now(StaffContext.Current.DlrCD), "MMddHHmm")
                '氏名に月日時分（ダミー名称）をセット
                custDataRow.NAME = custDataRow.FIRSTNAME
                'ダミー名称フラグ 1:ダミー名称
                custDataRow.DUMMYNAMEFLG = SC3080205TableAdapter.DummyNameFlgDummy
            Else
                'ダミー名称フラグ 0:正式名称
                custDataRow.DUMMYNAMEFLG = SC3080205TableAdapter.DummyNameFlgOfficial
            End If
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

            '2017/11/20 TCS 河原 TKM独自機能開発 START
            If String.Equals(Me.CleansingMode.Value, "0") Then
                'バリデーション判定
                If (SC3080205BusinessLogic.CheckValidation(custDataTbl, enabledTable, msgID, mode) = False) Then
                    'エラーメッセージを表示
                    Return msgID
                End If
            End If

            '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END
            If String.Equals(Me.Use_Customerdata_Cleansing_Flg.Value, "1") Then
                'クレンジング使用onの場合、クレンジングチェックを実施
                CustomerDataCleansing()
                Me.CleansingResult.Value = CleansingRslt
            Else
                'クレンジング使用offの場合
                Me.CleansingResult.Value = "0"
            End If

            If String.Equals(Me.CleansingMode.Value, "1") AndAlso Not CustomerDataCleansing() Then
                Return CleansingErrorNo
            End If
            '2017/11/20 TCS 河原 TKM独自機能開発 END
            'セッションの値をDataRowにセットする
            Me.SetSessionValue(custDataRow)

            '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
            '住所にDB登録用の値をセットする
            If (Me.addressDataCleansingHidden.Value = "1") Then
                '2017/11/20 TCS 河原 TKM独自機能開発 START
                'データクレンジング実施の場合
                '住所1
                'custDataRow.ADDRESS1 = Me.addressTextBox.Text
                'If (Not (String.IsNullOrEmpty(Me.address2TextBox.Text) And String.IsNullOrEmpty(Me.address3TextBox.Text))) Then
                '    custDataRow.ADDRESS1 = custDataRow.ADDRESS1 & ", " & Me.address2TextBox.Text
                '    If (Not String.IsNullOrEmpty(Me.address3TextBox.Text)) Then
                '        custDataRow.ADDRESS1 = custDataRow.ADDRESS1 & ", " & Me.address3TextBox.Text
                '    End If
                'End If
                If String.IsNullOrEmpty(Me.addressTextBox.Text) And String.IsNullOrEmpty(Me.address2TextBox.Text) And String.IsNullOrEmpty(Me.address3TextBox.Text) Then
                    custDataRow.ADDRESS1 = ", , "
                Else
                    custDataRow.ADDRESS1 = Me.addressTextBox.Text & ", " & Me.address2TextBox.Text & ", " & Me.address3TextBox.Text
                End If

                '住所2
                'custDataRow.ADDRESS2 = Me.addressLocation.Text
                'If (Not String.IsNullOrEmpty(Me.addressCity.Text)) Then
                '    If (Not String.IsNullOrEmpty(Me.addressLocation.Text)) Then
                '        custDataRow.ADDRESS2 = custDataRow.ADDRESS2 & ", "
                '    End If
                '    custDataRow.ADDRESS2 = custDataRow.ADDRESS2 & Me.addressCity.Text
                'End If
                If String.IsNullOrEmpty(Me.addressLocation.Text) And String.IsNullOrEmpty(Me.addressCity.Text) Then
                    custDataRow.ADDRESS2 = String.Empty
                Else
                    custDataRow.ADDRESS2 = Me.addressLocation.Text & ", " & Me.addressCity.Text
                End If

                '住所3
                'custDataRow.ADDRESS3 = Me.addressDistrict.Text
                'If (Not String.IsNullOrEmpty(Me.addressState.Text)) Then
                '    If (Not String.IsNullOrEmpty(Me.addressDistrict.Text)) Then
                '        custDataRow.ADDRESS3 = custDataRow.ADDRESS3 & ", "
                '    End If
                '    custDataRow.ADDRESS3 = custDataRow.ADDRESS3 & Me.addressState.Text
                'End If
                If String.IsNullOrEmpty(Me.addressDistrict.Text) And String.IsNullOrEmpty(Me.addressState.Text) Then
                    custDataRow.ADDRESS3 = String.Empty
                Else
                    custDataRow.ADDRESS3 = Me.addressDistrict.Text & ", " & Me.addressState.Text
                End If

                '住所
                Dim wkAddress As String = String.Empty

                wkAddress = CreateAddressText(Me.addressTextBox.Text, Me.address2TextBox.Text, Me.address3TextBox.Text, Me.addressState.Text, _
                                                Me.addressDistrict.Text, Me.addressCity.Text, Me.addressLocation.Text)

                '住所のトータルの長さが320文字を超えていないか？
                If (Not String.IsNullOrEmpty(wkAddress)) Then
                    If (Validation.IsCorrectDigit(wkAddress, 320) = False) Then
                        msgID = 40941
                        Return msgID
                    End If
                End If

                custDataRow.ADDRESS = wkAddress

            Else
                'データクレンジング実施しない場合
                '住所
                custDataRow.ADDRESS = Me.addressTextBox.Text
                If (Not (String.IsNullOrEmpty(Me.address2TextBox.Text) And String.IsNullOrEmpty(Me.address3TextBox.Text))) Then
                    custDataRow.ADDRESS = custDataRow.ADDRESS & ", " & Me.address2TextBox.Text
                    If (Not String.IsNullOrEmpty(Me.address3TextBox.Text)) Then
                        custDataRow.ADDRESS = custDataRow.ADDRESS & ", " & Me.address3TextBox.Text
                    End If
                End If
            End If
            '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

            If (mode = SC3080205BusinessLogic.ModeCreate) Then
                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditKanryo InsertCustomer Start")
                'ログ出力 End *****************************************************************************

                '０：新規登録モード
                '顧客新規登録
                ret = bizClass.InsertCustomer(custDataTbl, msgID)
                If (ret = SC3080205BusinessLogic.AlreadyUpdatedCustomerInfo) Then
                    '別のスタッフによって顧客情報の登録が行われた。
                    'エラーメッセージを表示
                    msgID = 40933
                    Return msgID
                End If

                'If (ret <= 0) Then
                '    '登録処理に失敗しました。
                '    'エラーメッセージを表示
                '    ShowMessageBox(919)
                '    Exit Sub
                'End If

                SetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, "2")                   '顧客種別
                SetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, "1")             '顧客分類
                SetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, custDataRow.CSTID)    '活動先顧客コード
                SetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, StaffContext.Current.Account)

                '2012/01/26 TCS 安田 【SALES_1B】来店実績より表示処理 START
                '車両編集時
                If (Me.nextVehicleFlg.Value.Equals(SC3080205BusinessLogic.NectVehicleBtn) = True) Then
                    'セッションキー 車両登録No初期表示フラグ (1:車両登録Noを表示する)
                    SetValue(ScreenPos.Current, SESSION_KEY_VCLREGNODISPFLG, SC3080206BusinessLogic.RegNoDispBtn)

                End If
                '2012/01/26 TCS 安田 【SALES_1B】来店実績より表示処理 END

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditKanryo InsertCustomer End")
                'ログ出力 End *****************************************************************************

            Else

                '１：編集モード
                If (custDataRow.CUSTFLG = SC3080205BusinessLogic.OrgCustFlg) Then

                    'ログ出力 Start ***************************************************************************
                    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditKanryo OrgCustFlg Start")
                    'ログ出力 End *****************************************************************************

                    '０：自社客
                    '当該項目が「表示」かつ「入力可」の場合、入力項目の更新機能フラグを”2”(i-CROP)としてフラグを生成する。
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdName), Boolean) = True) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdName, SC3080205BusinessLogic.UpdCD) '氏名
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdFirstName), Boolean) = True) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdFirstName, SC3080205BusinessLogic.UpdCD) 'ファーストネーム
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdMiddleName), Boolean) = True And _
                       (Me.inputSettingHidden02.Value = "1" Or Me.inputSettingHidden02.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdMiddleName, SC3080205BusinessLogic.UpdCD) 'ミドルネーム
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdLastName), Boolean) = True And _
                       (Me.inputSettingHidden03.Value = "1" Or Me.inputSettingHidden03.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdLastName, SC3080205BusinessLogic.UpdCD) 'ラストネーム
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdSex), Boolean) = True And _
                       (Me.inputSettingHidden04.Value = "1" Or Me.inputSettingHidden04.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdSex, SC3080205BusinessLogic.UpdCD) '性別
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdNameTitlecd), Boolean) = True And _
                       (Me.inputSettingHidden05.Value = "1" Or Me.inputSettingHidden05.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdNameTitlecd, SC3080205BusinessLogic.UpdCD) '敬称コード
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdNameTitle), Boolean) = True And _
                       (Me.inputSettingHidden05.Value = "1" Or Me.inputSettingHidden05.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdNameTitle, SC3080205BusinessLogic.UpdCD) '敬称
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdCustype), Boolean) = True And _
                       (Me.inputSettingHidden06.Value = "1" Or Me.inputSettingHidden06.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdCustype, SC3080205BusinessLogic.UpdCD)  '顧客タイプ
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdPrivateFleetItem), Boolean) = True And _
                       (Me.inputSettingHidden07.Value = "1" Or Me.inputSettingHidden07.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdPrivateFleetItem, SC3080205BusinessLogic.UpdCD) '個人法人項目
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdEmployeeName), Boolean) = True And _
                       (Me.inputSettingHidden08.Value = "1" Or Me.inputSettingHidden08.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdEmployeeName, SC3080205BusinessLogic.UpdCD) '担当者名
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdEmployeeDepartment), Boolean) = True And _
                       (Me.inputSettingHidden09.Value = "1" Or Me.inputSettingHidden09.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdEmployeeDepartment, SC3080205BusinessLogic.UpdCD) '担当者所属部署
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdEmployeePosition), Boolean) = True And _
                       (Me.inputSettingHidden10.Value = "1" Or Me.inputSettingHidden10.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdEmployeePosition, SC3080205BusinessLogic.UpdCD) '担当者役職
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdMobile), Boolean) = True) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdMobile, SC3080205BusinessLogic.UpdCD) '携帯電話番号
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.Idtelno), Boolean) = True) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.Idtelno, SC3080205BusinessLogic.UpdCD) '電話番号
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdBusinessTelno), Boolean) = True And _
                       (Me.inputSettingHidden13.Value = "1" Or Me.inputSettingHidden13.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdBusinessTelno, SC3080205BusinessLogic.UpdCD) '勤め先電話番号
                    End If
                    '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
                    If enabledTable.Item(SC3080205TableAdapter.IdCstIncome) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdCstIncome, SC3080205BusinessLogic.UpdCD) '収入
                    End If
                    '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdFaxno), Boolean) = True And _
                       (Me.inputSettingHidden14.Value = "1" Or Me.inputSettingHidden14.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdFaxno, SC3080205BusinessLogic.UpdCD) 'FAX番号
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdZipcode), Boolean) = True And _
                       (Me.inputSettingHidden15.Value = "1" Or Me.inputSettingHidden15.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdZipcode, SC3080205BusinessLogic.UpdCD) '郵便番号
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdAddress1), Boolean) = True And _
                       (Me.inputSettingHidden16.Value = "1" Or Me.inputSettingHidden16.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdAddress1, SC3080205BusinessLogic.UpdCD) '住所1
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdAddress2), Boolean) = True And _
                       (Me.inputSettingHidden17.Value = "1" Or Me.inputSettingHidden17.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdAddress2, SC3080205BusinessLogic.UpdCD) '住所2
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdAddress3), Boolean) = True And _
                       (Me.inputSettingHidden18.Value = "1" Or Me.inputSettingHidden18.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdAddress3, SC3080205BusinessLogic.UpdCD) '住所3
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdState), Boolean) = True And _
                       (Me.inputSettingHidden19.Value = "1" Or Me.inputSettingHidden19.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdState, SC3080205BusinessLogic.UpdCD) '住所(州)
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdDistrict), Boolean) = True And _
                       (Me.inputSettingHidden20.Value = "1" Or Me.inputSettingHidden20.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdDistrict, SC3080205BusinessLogic.UpdCD) '住所(地域)
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdCity), Boolean) = True And _
                       (Me.inputSettingHidden21.Value = "1" Or Me.inputSettingHidden21.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdCity, SC3080205BusinessLogic.UpdCD) '住所(市)
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdLocation), Boolean) = True And _
                       (Me.inputSettingHidden22.Value = "1" Or Me.inputSettingHidden22.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdLocation, SC3080205BusinessLogic.UpdCD) '住所(地区)
                    End If
                    '住所項目のいずれかが表示であれば
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdAddress), Boolean) = True And _
                       ((Me.inputSettingHidden16.Value = "1" Or Me.inputSettingHidden16.Value = "2") Or _
                        (Me.inputSettingHidden17.Value = "1" Or Me.inputSettingHidden17.Value = "2") Or _
                        (Me.inputSettingHidden18.Value = "1" Or Me.inputSettingHidden18.Value = "2") Or _
                        (Me.inputSettingHidden19.Value = "1" Or Me.inputSettingHidden19.Value = "2") Or _
                        (Me.inputSettingHidden20.Value = "1" Or Me.inputSettingHidden20.Value = "2") Or _
                        (Me.inputSettingHidden21.Value = "1" Or Me.inputSettingHidden21.Value = "2") Or _
                        (Me.inputSettingHidden22.Value = "1" Or Me.inputSettingHidden22.Value = "2"))) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdAddress, SC3080205BusinessLogic.UpdCD) '住所
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdDomicile), Boolean) = True And _
                       (Me.inputSettingHidden23.Value = "1" Or Me.inputSettingHidden23.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdDomicile, SC3080205BusinessLogic.UpdCD) '本籍
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdEmail1), Boolean) = True And _
                       (Me.inputSettingHidden24.Value = "1" Or Me.inputSettingHidden24.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdEmail1, SC3080205BusinessLogic.UpdCD) 'e-MAILアドレス1
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdEmail2), Boolean) = True And _
                       (Me.inputSettingHidden25.Value = "1" Or Me.inputSettingHidden25.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdEmail2, SC3080205BusinessLogic.UpdCD) 'e-MAILアドレス2
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdCountry), Boolean) = True And _
                       (Me.inputSettingHidden26.Value = "1" Or Me.inputSettingHidden26.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdCountry, SC3080205BusinessLogic.UpdCD) '国籍
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdSocialid), Boolean) = True And _
                       (Me.inputSettingHidden27.Value = "1" Or Me.inputSettingHidden27.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdSocialid, SC3080205BusinessLogic.UpdCD)  '国民番号
                    End If
                    If (CType(enabledTable.Item(SC3080205TableAdapter.IdBirthday), Boolean) = True And _
                       (Me.inputSettingHidden28.Value = "1" Or Me.inputSettingHidden28.Value = "2")) Then
                        UpdateOption(custDataRow.UPDATEFUNCFLG, SC3080205TableAdapter.IdBirthday, SC3080205BusinessLogic.UpdCD) '生年月日
                    End If
                    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

                    'ログ出力 Start ***************************************************************************
                    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditKanryo OrgCustFlg End")
                    'ログ出力 End *****************************************************************************

                End If

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditKanryo UpdateCustomer Start")
                'ログ出力 End *****************************************************************************

                '顧客更新
                '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
                custDataRow.VCLID = Me.vclupdateHidden.Value
                '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
                Dim actEditFlg As Integer
                If (Trim(Me.reasonidHidden.Value) <> Trim(Me.reasonidHidden_Old.Value) Or Me.actvctgryidHidden.Value <> Me.actvctgryidHidden_Old.Value) Then
                    actEditFlg = 1
                Else
                    actEditFlg = 0
                End If
                ret = bizClass.UpdateCustomer(custDataTbl, enabledTable, msgID, actEditFlg)
                ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End

                '2013/06/30 TCS 吉村 2013/10対応版　既存流用 START
                If (ret <= 0) Then
                    msgID = 901
                    Return msgID
                End If
                '2013/06/30 TCS 吉村 2013/10対応版　既存流用 END

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditKanryo UpdateCustomer End")
                'ログ出力 End *****************************************************************************

            End If

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            ''顧客メモの情報をセッションに保持する
            'If (custDataRow.CUSTFLG = SC3080205BusinessLogic.OrgCustFlg) Then
            '    '０：自社客
            '    Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CRCUSTID, custDataRow.ORIGINALID)
            'Else
            '    '１：未顧客
            '    Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CRCUSTID, custDataRow.CSTID)
            'End If
            ''顧客名
            'Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CRCUSTNAME, custDataRow.NAME)
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            ''顧客情報－変更前情報の保持
            'Call SetCustomerBackHidden()

            'ポップアップを囲うPanelをVisible=Falseに
            Me.CustomerEditVisiblePanel.Visible = False
            Me.NameListActvctgryReasonListVisiblePanel.Visible = False
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            '2017/11/20 TCS 河原 TKM独自機能開発 SATRT
            'クレンジングモード解除
            Me.CleansingMode.Value = "0"
            '2017/11/20 TCS 河原 TKM独自機能開発 END
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("CustomerEditKanryo End")
            'ログ出力 End *****************************************************************************

            Return 0

        End Using

    End Function

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 住所を結合したテキストを作成
    ''' </summary>
    ''' <param name="Address1"></param>
    ''' <param name="Address2"></param>
    ''' <param name="Address3"></param>
    ''' <param name="addressState"></param>
    ''' <param name="addressDistrict"></param>
    ''' <param name="addressCity"></param>
    ''' <param name="addressLocation"></param>
    ''' <remarks></remarks>
    Protected Function CreateAddressText(ByVal Address1 As String, ByVal Address2 As String, ByVal Address3 As String, ByVal addressState As String, _
                                         ByVal addressDistrict As String, ByVal addressCity As String, ByVal addressLocation As String) As String

        '住所
        Dim wkAddress As String = String.Empty

        If Not String.IsNullOrEmpty(Address1) Then
            wkAddress = Address1
        End If

        If Not String.IsNullOrEmpty(Address2) Then
            If Not String.IsNullOrEmpty(wkAddress) Then
                wkAddress = wkAddress & ", "
            End If
            wkAddress = wkAddress & Address2
        End If

        If Not String.IsNullOrEmpty(Address3) Then
            If Not String.IsNullOrEmpty(wkAddress) Then
                wkAddress = wkAddress & ", "
            End If
            wkAddress = wkAddress & Address3
        End If

        'LocationかCityが空でない場合、改行を追加
        If Not String.IsNullOrEmpty(addressLocation) Or Not String.IsNullOrEmpty(addressCity) Then
            If Not String.IsNullOrEmpty(wkAddress) Then
                wkAddress = wkAddress & ", " & vbCrLf
            End If
        End If

        If Not String.IsNullOrEmpty(addressLocation) Then
            wkAddress = wkAddress & addressLocation & ", "
        End If

        If Not String.IsNullOrEmpty(addressCity) Then
            wkAddress = wkAddress & addressCity
        End If

        'DistrictかStateが空でない場合、改行を追加
        If Not String.IsNullOrEmpty(addressDistrict) Or Not String.IsNullOrEmpty(addressState) Then
            If Not String.IsNullOrEmpty(wkAddress) Then
                wkAddress = wkAddress & ", " & vbCrLf
            End If
        End If

        If Not String.IsNullOrEmpty(addressDistrict) Then
            wkAddress = wkAddress & addressDistrict & ", "
        End If

        If Not String.IsNullOrEmpty(addressState) Then
            wkAddress = wkAddress & addressState
        End If

        '後ろのカンマスペースを削除
        If System.Text.RegularExpressions.Regex.IsMatch(wkAddress, ".*, $") Then
            wkAddress = Left(wkAddress, wkAddress.Length - 2)
        End If

        Return wkAddress

    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END


    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
    Protected Sub CustomerEditPopupOpenButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerEditPopupOpenButton.Click
        Logger.Info("CustomerEditPopupOpenButton_Click Start")

        'ポップアップを囲うPanelをVisible=Trueに
        Me.CustomerEditVisiblePanel.Visible = True
        Me.NameListActvctgryReasonListVisiblePanel.Visible = True

        '顧客編集ポップアップ表示
        Me.CustomerEditInitialize()

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "CustomerEditPopUpOpenAfter", "startup")

        '2017/11/20 TCS 河原 TKM独自機能開発 START 
        If String.Equals(Me.CleansingModeFlg.Value, "1") Then
            Me.customerTitleLabel.Text = Me.dataCleansingLabel.Text
            JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "CleansingMode", "CleansingMode")
        End If
        '2017/11/20 TCS 河原 TKM独自機能開発 END

        Logger.Info("CustomerEditPopupOpenButton_Click End")
    End Sub
    '2012/03/08 TCS 山口 【SALES_2】性能改善 END


    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>顧客検索画面　検索タイプ(4:電話番号/携帯番号)</summary>
    Private Const searchType_telNo As Integer = 4
    ''' <summary>顧客検索画面　検索タイプ(6:国民ID)</summary>
    Private Const searchType_socialId As Integer = 6
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 START
    ''' <summary>
    ''' 電話番号検索ボタン(携帯番号)押下時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub mobileSerchButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mobileSerchButton.Click
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'SetSession(mobileTextBox.Text)
        SetSession(mobileTextBox.Text, searchType_telNo)    '検索タイプ:4(電話番号/携帯番号)
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        telSerchFlgHidden.Value = "1"
    End Sub

    ''' <summary>
    ''' 電話番号検索ボタン(電話番号)押下時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub telnoSerchButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles telnoSerchButton.Click
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        'SetSession(telnoTextBox.Text)
        SetSession(telnoTextBox.Text, searchType_telNo)     '検索タイプ:4(電話番号/携帯番号)
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END
        telSerchFlgHidden.Value = "1"
    End Sub
    '2012/08/23 TCS 山口 【A STEP2】次世代e-CRB 新車商談機能改善 END

    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 国民ID検索ボタン押下時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub socialIdSearchButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles socialIdSearchButton.Click
        SetSession(socialidTextBox.Text, searchType_socialId)       '検索タイプ:6(国民ID)
        telSerchFlgHidden.Value = "1"
    End Sub
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

#End Region

#Region "コールバック処理"
    Private Const OKText As String = "1"

    Private Const ErrorText As String = "9"

    Private _callbackResult As String

    ''' <summary>
    ''' コールバック用文字列を返す
    ''' </summary>
    ''' <remarks></remarks>
    Public Function GetCallbackResult() As String Implements System.Web.UI.ICallbackEventHandler.GetCallbackResult

        Return _callbackResult

    End Function

    '住所検索
    Private Const MethodGetAddress As String = "GetAddress"
    '顧客編集　完了
    Private Const MethodCustomerUpdate As String = "CustomerUpdate"
    '車両編集　完了
    Private Const MethodVehicleUpdate As String = "VehicleUpdate"
    '車両追加
    Private Const MethodVehicleAppend As String = "VehicleAppend"
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    '地域リスト取得
    Private Const MethodGetDistrict As String = "GetDistrict"
    '市リスト取得
    Private Const MethodGetCity As String = "GetCity"
    '地区リスト取得
    Private Const MethodGetLocation As String = "GetLocation"
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

    ''' <summary>
    ''' コールバックイベントハンドリング
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub RaiseCallbackEvent(ByVal eventArgument As String) Implements System.Web.UI.ICallbackEventHandler.RaiseCallbackEvent

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("RaiseCallbackEvent Start")
        'ログ出力 End *****************************************************************************

        Dim tokens As String() = eventArgument.Split(New Char() {","c})

        '2012/03/08 TCS 河原 【SALES_1B】コールバック時の文字列のエンコード処理追加 START
        For i = 0 To tokens.Length - 1
            tokens(i) = HttpUtility.UrlDecode(tokens(i))
        Next
        '2012/03/08 TCS 河原 【SALES_1B】コールバック時の文字列のエンコード処理追加 END

        Dim method As String = tokens(0)
        Dim argument As String = tokens(1)
        Dim resultString As String = String.Empty

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("RaiseCallbackEvent method = " + method)
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("RaiseCallbackEvent eventArgument = " + eventArgument)
        'ログ出力 End *****************************************************************************

        '住所検索
        If (method.Equals(MethodGetAddress)) Then

            '住所検索
            Me.zipcodeTextBox.Text = tokens(1)
            resultString = ZipSerch()

            _callbackResult = method & "," & HttpUtility.HtmlEncode(resultString)

        End If

        '顧客編集　完了クリック
        If (method.Equals(MethodCustomerUpdate)) Then

            Try
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
                'Me.nameTextBox.Text = tokens(1)     '氏名
                'Me.nameTitleHidden.Value = tokens(2)     '敬称コード
                'If (tokens(3).Equals("on") = True) Then     '男
                '    Me.manCheckBox.Checked = True
                'Else
                '    Me.manCheckBox.Checked = False
                'End If
                '
                'If (tokens(4).Equals("on") = True) Then     '女
                '    Me.girlCheckBox.Checked = True
                'Else
                '    Me.girlCheckBox.Checked = False
                'End If
                'If (tokens(5).Equals("on") = True) Then     '個人
                '    Me.kojinCheckBox.Checked = True
                'Else
                '    Me.kojinCheckBox.Checked = False
                'End If
                '
                'If (tokens(6).Equals("on") = True) Then     '法人
                '    Me.houjinCheckBox.Checked = True
                'Else
                '    Me.houjinCheckBox.Checked = False
                'End If
                '
                'If (tokens(7).Equals("undefined") = True) Then
                '    Me.employeenameTextBox.Text = ""     '担当者氏名
                'Else
                '    Me.employeenameTextBox.Text = tokens(7)     '担当者氏名
                'End If
                'If (tokens(8).Equals("undefined") = True) Then
                '    Me.employeedepartmentTextBox.Text = ""     '担当者部署名
                'Else
                '    Me.employeedepartmentTextBox.Text = tokens(8)       '担当者部署名
                'End If
                'If (tokens(9).Equals("undefined") = True) Then
                '    Me.employeedepartmentTextBox.Text = ""     '役職
                'Else
                '    Me.employeepositionTextBox.Text = tokens(9)     '役職
                'End If
                '
                'Me.mobileTextBox.Text = tokens(10)       '携帯
                'Me.telnoTextBox.Text = tokens(11)        '自宅
                'Me.businesstelnoTextBox.Text = tokens(12)        '勤務先
                'Me.faxnoTextBox.Text = tokens(13)        'FAX
                '
                'Me.zipcodeTextBox.Text = tokens(14)      '郵便番号
                'Me.addressTextBox.Text = tokens(15)      '住所
                '
                'Me.email1TextBox.Text = tokens(16)       'E-Mail1
                'Me.email2TextBox.Text = tokens(17)       'E-Mail2
                '
                'Me.socialidTextBox.Text = tokens(18)     '国民ID
                '
                'If String.IsNullOrEmpty(tokens(19)) Then
                '    Me.birthdayTextBox.Value = Nothing      '誕生日
                'Else
                '    Me.birthdayTextBox.Value = CType(tokens(19), Date)     '誕生日
                'End If
                '
                'Me.actvctgryidHidden.Value = tokens(20)       '活動区分
                'Me.reasonidHidden.Value = tokens(21)          '断念理由
                '
                'If (tokens(22).Equals("on") = True) Then     'SMS
                '    Me.smsCheckButton.Checked = True
                'Else
                '    Me.smsCheckButton.Checked = False
                'End If
                'If (tokens(23).Equals("on") = True) Then     'EMail
                '    Me.emailCheckButton.Checked = True
                'Else
                '    Me.emailCheckButton.Checked = False
                'End If
                'Me.nameTitleTextHidden.Value = tokens(24)     '敬称
                Me.nameTextBox.Text = tokens(1)     'ファーストネーム
                Me.middleNameTextBox.Text = tokens(2)     'ミドルネーム
                Me.lastNameTextBox.Text = tokens(3)     'ラストネーム
                Me.nameTitleHidden.Value = tokens(4)     '敬称コード

                If (tokens(5).Equals("on") = True) Then     '男
                    Me.manCheckBox.Checked = True
                Else
                    Me.manCheckBox.Checked = False
                End If
                If (tokens(6).Equals("on") = True) Then     '女
                    Me.girlCheckBox.Checked = True
                Else
                    Me.girlCheckBox.Checked = False
                End If
                If (tokens(7).Equals("on") = True) Then     'その他
                    Me.otherCheckBox.Checked = True
                Else
                    Me.otherCheckBox.Checked = False
                End If
                If (tokens(8).Equals("on") = True) Then     '不明
                    Me.unknownCheckBox.Checked = True
                Else
                    Me.unknownCheckBox.Checked = False
                End If

                If (tokens(9).Equals("on") = True) Then     '個人
                    Me.kojinCheckBox.Checked = True
                Else
                    Me.kojinCheckBox.Checked = False
                End If
                If (tokens(10).Equals("on") = True) Then     '法人
                    Me.houjinCheckBox.Checked = True
                Else
                    Me.houjinCheckBox.Checked = False
                End If

                If (tokens(11).Equals("undefined") = True) Then
                    Me.employeenameTextBox.Text = ""     '担当者氏名
                Else
                    Me.employeenameTextBox.Text = tokens(11)     '担当者氏名
                End If
                If (tokens(12).Equals("undefined") = True) Then
                    Me.employeedepartmentTextBox.Text = ""     '担当者部署名
                Else
                    Me.employeedepartmentTextBox.Text = tokens(12)       '担当者部署名
                End If
                If (tokens(13).Equals("undefined") = True) Then
                    Me.employeedepartmentTextBox.Text = ""     '役職
                Else
                    Me.employeepositionTextBox.Text = tokens(13)     '役職
                End If

                Me.mobileTextBox.Text = tokens(14)       '携帯
                Me.telnoTextBox.Text = tokens(15)        '自宅
                Me.businesstelnoTextBox.Text = tokens(16)        '勤務先
                Me.faxnoTextBox.Text = tokens(17)        'FAX

                Me.zipcodeTextBox.Text = tokens(18)      '郵便番号
                Me.addressTextBox.Text = tokens(19)      '住所1
                Me.address2TextBox.Text = tokens(20)     '住所2
                Me.address3TextBox.Text = tokens(21)     '住所3
                Me.stateHidden.Value = tokens(22)        '住所(州)
                Me.addressState.Text = tokens(23)        '住所(州)(名称)
                Me.districtHidden.Value = tokens(24)     '住所(地域)
                Me.addressDistrict.Text = tokens(25)     '住所(地域)(名称)
                Me.cityHidden.Value = tokens(26)         '住所(市)
                Me.addressCity.Text = tokens(27)         '住所(市)(名称)
                Me.locationHidden.Value = tokens(28)     '住所(地区)
                Me.addressLocation.Text = tokens(29)     '住所(地区)(名称)

                Me.email1TextBox.Text = tokens(30)       'E-Mail1
                Me.email2TextBox.Text = tokens(31)       'E-Mail2

                Me.socialidTextBox.Text = tokens(32)     '国民ID

                If String.IsNullOrEmpty(tokens(33)) Then
                    Me.birthdayTextBox.Value = Nothing      '誕生日
                Else
                    Me.birthdayTextBox.Value = CType(tokens(33), Date)     '誕生日
                End If

                Me.actvctgryidHidden.Value = tokens(34)       '活動区分
                Me.reasonidHidden.Value = tokens(35)          '断念理由

                If (tokens(36).Equals("on") = True) Then     'SMS
                    Me.smsCheckButton.Checked = True
                Else
                    Me.smsCheckButton.Checked = False
                End If
                If (tokens(37).Equals("on") = True) Then     'EMail
                    Me.emailCheckButton.Checked = True
                Else
                    Me.emailCheckButton.Checked = False
                End If
                Me.nameTitleTextHidden.Value = tokens(38)     '敬称

                Me.privateFleetItemHidden.Value = tokens(39)  '個人法人項目
                Me.domicileTextBox.Text = tokens(40)          '本籍
                Me.countryTextBox.Text = tokens(41)           '国籍
                '2014/03/18 TCS 市川  新PF開発No.41（商業情報活動) START
                getCommercialRecvTypeValue = tokens(42)     '商業情報受取区分
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

                '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) START
                Me.incomeTextBox.Text = tokens(43)          '収入
                '2014/05/07 TCS 市川 PCとタブレットで顧客入力情報不一致対応(CHG-354) END

                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
                Me.custOrgnzHidden.Value = tokens(44)           '顧客組織コード
                Me.custOrgnzInputTypeHidden.Value = tokens(45)  '顧客組織入力区分
                Me.custOrgnz.Text = tokens(46)                  '顧客組織名
                Me.custSubCtgry2Hidden.Value = tokens(47)       '顧客サブカテゴリ2コード
                ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END

                '登録処理実行
                Dim msgID1 As Integer
                msgID1 = CustomerEditKanryo()
                '2017/11/20 TCS 河原 TKM独自機能開発 START
                If (msgID1 = 0) Then
                    _callbackResult = method & "," & "0" & ",," & CleansingRslt
                Else
                    resultString = WebWordUtility.GetWord(msgID1)               'エラーメッセージ取得
                    resultString = Replace(resultString, ",", "@@@")            'エラーメッセージにカンマが含まれるとエラーとなるため@@@に置換
                    _callbackResult = method & "," & "9" & "," & HttpUtility.HtmlEncode(resultString) & "," & CleansingRslt
                End If
                '2017/11/20 TCS 河原 TKM独自機能開発 END
            Catch ex As Exception

                Toyota.eCRB.SystemFrameworks.Core.Logger.Info(ex.Message)
                Toyota.eCRB.SystemFrameworks.Core.Logger.Error(method & " Error Log", ex)

                _callbackResult = method & "," & "9" & ",[DB UPDATE ERROR] method=" & method & vbCrLf & HttpUtility.HtmlEncode(ex.Message)

            End Try

        End If

        '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
        '地域コード取得
        If (method.Equals(MethodGetDistrict)) Then
            resultString = GetDistrictStr(tokens(1))
            _callbackResult = method & "," & HttpUtility.HtmlEncode(resultString)
        End If

        '市コード取得
        If (method.Equals(MethodGetCity)) Then
            resultString = GetCityStr(tokens(1), tokens(2))
            _callbackResult = method & "," & HttpUtility.HtmlEncode(resultString)
        End If

        '地区コード取得
        If (method.Equals(MethodGetLocation)) Then
            resultString = GetLocationStr(tokens(1), tokens(2), tokens(3))
            _callbackResult = method & "," & HttpUtility.HtmlEncode(resultString)
        End If
        '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

        '車両追加クリック
        If (method.Equals(MethodVehicleAppend)) Then

            '車両編集　初期化処理
            Call VehicleInitialize()

            _callbackResult = method & "," & "0" & ","

        End If

        '車両編集　完了クリック
        If (method.Equals(MethodVehicleUpdate)) Then

            Try

                Me.makerTextBox.Text = tokens(1)       'メーカー
                Me.modelTextBox.Text = tokens(2)       'モデル
                Me.vclregnoTextBox.Text = tokens(3)    '車両登録No
                Me.vinTextBox.Text = tokens(4)         'VIN

                If (IsDate(tokens(5))) Then
                    Me.vcldelidateDateTime.Value = CType(tokens(5), Date)         '納車日
                Else
                    Me.vcldelidateDateTime.Value = Nothing      '納車日
                End If
                Me.editVehicleModeHidden.Value = tokens(6)      '処理モード

                Me.actvctgryidHidden.Value = tokens(7)          '活動区分
                Me.reasonidHidden.Value = tokens(8)             '断念理由
                '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
                Me.vclMileTextBox.Text = tokens(9)              '走行距離
                Me.modelYearHidden.Value = tokens(10)           '年式
                '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

                '登録処理実行
                Dim msgID2 As Integer
                msgID2 = VehicleEditKanryo()
                If (msgID2 = 0) Then
                    _callbackResult = method & "," & "0" & ","
                Else
                    resultString = WebWordUtility.GetWord(msgID2)               'エラーメッセージ取得
                    '2012/04/27 TCS 安田 【SALES_2】HTMLEncode対応 START
                    _callbackResult = method & "," & "9" & "," & HttpUtility.HtmlEncode(resultString)
                    '2012/04/27 TCS 安田 【SALES_2】HTMLEncode対応 END
                End If

            Catch ex As Exception

                Toyota.eCRB.SystemFrameworks.Core.Logger.Info(ex.Message)
                Toyota.eCRB.SystemFrameworks.Core.Logger.Error(method & " Error Log", ex)

                _callbackResult = method & "," & "9" & ",[DB UPDATE ERROR] method=" & method & vbCrLf & HttpUtility.HtmlEncode(ex.Message)

            End Try
        End If

        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
        ' 変数のスコープを局所化するためのブロック
        If True Then
            Dim params As Dictionary(Of String, String) =
                tokens.Skip(1) _
                .Where(Function(token) token.Contains("="c)) _
                .Select(Function(token) token.Split("="c)) _
                .ToDictionary(Function(keyval) keyval.First(), Function(keyval) String.Join("=", keyval.Skip(1)))

            Select Case method
                Case "ApplyFormData"
                    ' JavaScript側で管理しているフォームデータをASP.NET側に通知する

                    Dim valueOrEmpty = GetValueOrDefault(params, "")
                    Dim valueOrOff = GetValueOrDefault(params, "off")

                    Me.nameTextBox.Text = valueOrEmpty("firstName")
                    Me.middleNameTextBox.Text = valueOrEmpty("middleName")
                    Me.lastNameTextBox.Text = valueOrEmpty("lastName")
                    Me.nameTitleHidden.Value = valueOrEmpty("nameTitle")

                    Me.manCheckBox.Checked = valueOrOff("man") = "on"
                    Me.girlCheckBox.Checked = valueOrOff("girl") = "on"
                    Me.otherCheckBox.Checked = valueOrOff("other") = "on"
                    Me.unknownCheckBox.Checked = valueOrOff("unknown") = "on"

                    Me.kojinCheckBox.Checked = valueOrOff("kojin") = "on"
                    Me.houjinCheckBox.Checked = valueOrOff("houjin") = "on"

                    Me.employeenameTextBox.Text = ChangeJSNoneObjectToEmpty(valueOrEmpty("employeeName"))
                    Me.employeedepartmentTextBox.Text = ChangeJSNoneObjectToEmpty(valueOrEmpty("employeeDepartment"))
                    Me.employeepositionTextBox.Text = ChangeJSNoneObjectToEmpty(valueOrEmpty("employeePosition"))

                    Me.mobileTextBox.Text = valueOrEmpty("mobile")
                    Me.telnoTextBox.Text = valueOrEmpty("telNo")
                    Me.businesstelnoTextBox.Text = valueOrEmpty("businessTelNo")
                    Me.faxnoTextBox.Text = valueOrEmpty("faxNo")

                    Me.zipcodeTextBox.Text = valueOrEmpty("zipCode")
                    Me.addressTextBox.Text = valueOrEmpty("address")
                    Me.address2TextBox.Text = valueOrEmpty("address2")
                    Me.address3TextBox.Text = valueOrEmpty("address3")
                    Me.stateHidden.Value = valueOrEmpty("state")
                    Me.addressState.Text = valueOrEmpty("addressState")
                    Me.districtHidden.Value = valueOrEmpty("district")
                    Me.addressDistrict.Text = valueOrEmpty("addressDistrict")
                    Me.cityHidden.Value = valueOrEmpty("city")
                    Me.addressCity.Text = valueOrEmpty("addressCity")
                    Me.locationHidden.Value = valueOrEmpty("location")
                    Me.addressLocation.Text = valueOrEmpty("addressLocation")

                    Me.email1TextBox.Text = valueOrEmpty("email1")
                    Me.email2TextBox.Text = valueOrEmpty("email2")

                    Me.socialidTextBox.Text = valueOrEmpty("socialId")

                    Dim birthDay = GetValueOrDefault(params, "birthDay", Nothing)
                    Me.birthdayTextBox.Value = If(String.IsNullOrEmpty(birthDay), Nothing, CType(birthDay, Date?))

                    Me.actvctgryidHidden.Value = valueOrEmpty("actvCtgryId")
                    Me.reasonidHidden.Value = valueOrEmpty("reasonId")

                    Me.smsCheckButton.Checked = valueOrOff("sms") = "on"
                    Me.emailCheckButton.Checked = valueOrOff("email") = "on"
                    Me.nameTitleTextHidden.Value = valueOrEmpty("nameTitleText")

                    Me.privateFleetItemHidden.Value = valueOrEmpty("privateFleetItem")
                    Me.domicileTextBox.Text = valueOrEmpty("domicile")
                    Me.countryTextBox.Text = valueOrEmpty("country")
                    getCommercialRecvTypeValue = valueOrEmpty("commercialRecvType")

                    Me.incomeTextBox.Text = valueOrEmpty("income")

                    Me.custOrgnzHidden.Value = valueOrEmpty("custOrgnz")
                    Me.custOrgnzInputTypeHidden.Value = valueOrEmpty("custOrgnzInputType")
                    Me.custOrgnz.Text = valueOrEmpty("custOrgnzName")
                    Me.custSubCtgry2Hidden.Value = valueOrEmpty("custSubCtgry2")

                Case "UpdateCustOrgnzList"
                    ' 顧客組織名称のリストを動的に表示するため、DBに検索をかける

                    Dim custOrgnzList As SC3080205DataSet.SC3080205CustOrgnzLocalDataTable
                    custOrgnzList = New SC3080205DataSet.SC3080205CustOrgnzLocalDataTable()

                    If params.ContainsKey("custOrgnzNameHead") AndAlso params.ContainsKey("privateFleetItemCd") Then
                        Dim inputTYPE = SC3080205BusinessLogic.GetPrivateFleetItem(0) _
                                        .Where(Function(e) e.PRIVATE_FLEET_ITEM_CD = params("privateFleetItemCd")) _
                                        .Select(Function(e) e.CST_ORGNZ_NAME_INPUT_TYPE).FirstOrDefault()

                        ' 0：Free field   1：Dropdown   2：Suggestive free field
                        Select Case inputTYPE
                            Case "0"
                                custOrgnzList = New SC3080205DataSet.SC3080205CustOrgnzLocalDataTable
                            Case "1"
                                custOrgnzList = SC3080205BusinessLogic.GetCustOrgnzLocal(params("privateFleetItemCd"))
                            Case "2"
                                If String.IsNullOrEmpty(params("custOrgnzNameHead")) Then
                                    custOrgnzList = New SC3080205DataSet.SC3080205CustOrgnzLocalDataTable
                                Else
                                    custOrgnzList = SC3080205BusinessLogic.GetCustOrgnzLocal(params("custOrgnzNameHead"), params("privateFleetItemCd"))
                                End If
                        End Select
                    End If

                    ' DataSource への代入によるリスト更新はUIに反映されなかったため、JSONを返却してJavaScript側で <li> を生成する手法を用いる
                    Dim sanitize As Func(Of String, String) = Function(x) x.Replace("\", "\\").Replace("""", "\""").Replace("/", "\/").Replace(vbTab, "\t")
                    Dim jsonObjects = custOrgnzList.GroupBy(Function(e) e.CST_ORGNZ_CD).Select(Function(x) String.Format("{{""orgnzCd"":""{0}"",""name"":""{1}""}}", x.FirstOrDefault.CST_ORGNZ_CD, sanitize(x.FirstOrDefault.CST_ORGNZ_NAME)))
                    Dim json = String.Format("[{0}]", String.Join(",", jsonObjects))

                    _callbackResult = json

                Case "ConfirmCustOrgnz"
                    ' 顧客組織名称をリストから選択、または入力した際の確定処理（サブカテゴリ2リスト表示に用いる）

                    Dim private_fleet_item_cd As String = params("privateFleetItemCd")
                    Dim cst_orgnz_cd As String = params("custOrgnzCd")
                    Const Template As String = "{{""orgnzCd"":""{0}"",""privatefleetItemCd"":""{1}"",""subcat2Cd"":""{2}"",""subcat2Name"":""{3}""}}"
                    Dim sanitize As Func(Of String, String) = Function(x) x.Replace("\", "\\").Replace("""", "\""").Replace("/", "\/").Replace(vbTab, "\t")
                    Dim data =
                        SC3080205BusinessLogic.GetCustSubCtgry2(private_fleet_item_cd, cst_orgnz_cd) _
                        .Select(Function(x) String.Format(Template, x.CST_ORGNZ_CD, x.PRIVATE_FLEET_ITEM_CD, x.CST_SUBCAT2_CD, x.CST_SUBCAT2_NAME))

                    If data.Count <> 0 Then
                        _callbackResult = "[" & String.Join(",", data.DefaultIfEmpty(String.Format(Template, " ", "", " ", ""))) & "]"
                    Else
                        _callbackResult = ""
                    End If

            End Select
        End If
        ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("RaiseCallbackEvent End")
        'ログ出力 End *****************************************************************************

    End Sub

    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
    ''' <summary>
    ''' 指定の <see cref="Dictionary(Of TKey, TValue)"/> からキーを指定して値を取り出す。キーが存在しない場合は、指定したデフォルト値で置き換える。
    ''' </summary>
    ''' <typeparam name="TKey">キーの型</typeparam>
    ''' <typeparam name="TValue">値の型</typeparam>
    ''' <param name="dict">対象の <see cref="Dictionary(Of TKey, TValue)"/></param>
    ''' <param name="key">キー</param>
    ''' <param name="defaultValue">デフォルト値</param>
    ''' <returns>キーが存在すれば、対応する値。なければデフォルト値</returns>
    Private Shared Function GetValueOrDefault(Of TKey, TValue)(ByVal dict As Dictionary(Of TKey, TValue), ByVal key As TKey, ByVal defaultValue As TValue) As TValue
        If dict Is Nothing Then Return Nothing
        Return If(dict.ContainsKey(key), dict(key), defaultValue)
    End Function

    ''' <summary>
    ''' 指定の <see cref="Dictionary(Of TKey, TValue)"/> からキーを指定して値を取り出すことのできるデリゲートを返す。この関数はキーが存在しない場合に、指定したデフォルト値を返す。
    ''' </summary>
    ''' <typeparam name="TKey">キーの型</typeparam>
    ''' <typeparam name="TValue">値の型</typeparam>
    ''' <param name="dict">対象の <see cref="Dictionary(Of TKey, TValue)"/></param>
    ''' <param name="defaultValue">デフォルト値</param>
    ''' <returns>キーを受け取って値を返すデリゲート。この関数はキーが存在しない場合に、指定したデフォルト値を返す。</returns>
    Private Shared Function GetValueOrDefault(Of TKey, TValue)(ByVal dict As Dictionary(Of TKey, TValue), ByVal defaultValue As TValue) As Func(Of TKey, TValue)
        Return Function(key) GetValueOrDefault(dict, key, defaultValue)
    End Function

    Private Shared Function ChangeJSNoneObjectToEmpty(ByVal target As String) As String
        Return If(target = "undefined" OrElse target = "null", "", target)
    End Function
    ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END

    ''' <summary>
    ''' 住所検索
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function ZipSerch() As String

        Dim ret As Integer = 0
        Dim resultString As String = String.Empty
        Dim msgID As Integer = 0
        Dim bizClass As New SC3080205BusinessLogic
        Using custDataTbl As New SC3080205DataSet.SC3080205CustDataTable

            '画面の値を取得する
            Me.GetDisplayValues(custDataTbl)

            '住所検索バリデーション判定
            If (SC3080205BusinessLogic.CheckAddressValidation(custDataTbl, msgID) = False) Then

                'エラーメッセージを表示
                resultString = ErrorText & "," & WebWordUtility.GetWord(msgID)

                Return resultString

            End If

            '住所検索
            Dim zipDataTl As SC3080205DataSet.SC3080205ZipDataTable = _
               SC3080205BusinessLogic.GetAddress(custDataTbl, msgID)

            '住所セット
            If (zipDataTl.Rows.Count >= 1) Then
                Dim zipDataRow As SC3080205DataSet.SC3080205ZipRow = _
                     zipDataTl.Item(0)

                '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
                ''指定された郵便番号は存在しません。
                'resultString = OKText & "," & zipDataRow.ADDRESS
                '返却値…住所、州(コード)、地域(コード)、市(コード)、地区(コード)
                resultString = OKText & "," & zipDataRow.ADDRESS.Trim & "," & zipDataRow.ADDRESS_STATE.Trim & _
                               "," & zipDataRow.ADDRESS_DISTRICT.Trim & "," & zipDataRow.ADDRESS_CITY.Trim & _
                               "," & zipDataRow.ADDRESS_LOCATION.Trim
                '2013/11/27 TCS 各務 Aカード情報相互連携開発 END

            Else

                '指定された郵便番号は存在しません。
                resultString = ErrorText & "," & WebWordUtility.GetWord(40917)

            End If

            Return resultString
        End Using
    End Function

    '2013/11/27 TCS 各務 Aカード情報相互連携開発 START
    ''' <summary>
    ''' 地域コード取得
    ''' </summary>
    ''' <param name="stateCode">州コード</param>
    ''' <remarks></remarks>
    Protected Function GetDistrictStr(ByVal stateCode As String) As String

        Dim ret As Integer = 0
        Dim resultString As String = String.Empty
        Dim msgID As Integer = 0

        '地域コード検索
        Dim districtList As SC3080205DataSet.SC3080205DistrictDataTable = _
           SC3080205BusinessLogic.GetDistrict(stateCode, msgID)

        '取得した地域コード(リスト)を文字列に変換
        'リスト各行の値をカンマ区切りで結合
        Dim temp As String = String.Empty
        For Each districtRow In districtList
            temp = temp & "," & districtRow.DISTRICT_CD
            temp = temp & "," & districtRow.DISTRICT_NAME
        Next

        resultString = OKText & temp

        Return resultString
    End Function

    ''' <summary>
    ''' 市コード取得
    ''' </summary>
    ''' <param name="stateCode">州コード</param>
    ''' <param name="districtCode">地域コード</param>
    ''' <remarks></remarks>
    Protected Function GetCityStr(ByVal stateCode As String, ByVal districtCode As String) As String

        Dim ret As Integer = 0
        Dim resultString As String = String.Empty
        Dim msgID As Integer = 0

        '市コード検索
        Dim cityList As SC3080205DataSet.SC3080205CityDataTable = _
           SC3080205BusinessLogic.GetCity(stateCode, districtCode, msgID)

        '取得した市コード(リスト)を文字列に変換
        'リスト各行の値をカンマ区切りで結合
        Dim temp As String = String.Empty
        For Each cityRow In cityList
            temp = temp & "," & cityRow.CITY_CD
            temp = temp & "," & cityRow.CITY_NAME
        Next

        resultString = OKText & temp

        Return resultString
    End Function

    ''' <summary>
    ''' 地区コード取得
    ''' </summary>
    ''' <param name="stateCode">州コード</param>
    ''' <param name="districtCode">地域コード</param>
    ''' <param name="cityCode">市コード</param>
    ''' <remarks></remarks>
    Protected Function GetLocationStr(ByVal stateCode As String, ByVal districtCode As String, ByVal cityCode As String) As String

        Dim ret As Integer = 0
        Dim resultString As String = String.Empty
        Dim msgID As Integer = 0

        '地区コード検索
        Dim locationList As SC3080205DataSet.SC3080205LocationDataTable = _
           SC3080205BusinessLogic.GetLocation(stateCode, districtCode, cityCode, msgID)

        '取得した地区コード(リスト)を文字列に変換
        'リスト各行の値をカンマ区切りで結合
        Dim temp As String = String.Empty
        For Each locationRow In locationList
            temp = temp & "," & locationRow.LOCATION_CD
            temp = temp & "," & locationRow.LOCATION_NAME
            temp = temp & "," & locationRow.ZIP_CD
        Next

        resultString = OKText & temp

        Return resultString
    End Function
    '2013/11/27 TCS 各務 Aカード情報相互連携開発 END


#End Region
#End Region

#Region "顧客詳細(顧客情報)"

#Region " 定数 "

    '顧客詳細セッションキー
    ''' <summary>販売店コード</summary>
    Private Const SESSION_KEY_DLRCD As String = "SearchKey.DLRCD"
    ''' <summary>店舗コード</summary>
    Private Const SESSION_KEY_STRCD As String = "SearchKey.STRCD"
    ''' <summary>顧客種別</summary>
    Private Const SESSION_KEY_CSTKIND As String = "SearchKey.CSTKIND"
    ''' <summary>顧客分類</summary>
    Private Const SESSION_KEY_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"
    ''' <summary>活動先顧客コード</summary>
    Private Const SESSION_KEY_CRCUSTID As String = "SearchKey.CRCUSTID"
    ''' <summary>車両ID</summary>
    Private Const SESSION_KEY_VCLID As String = "SearchKey.VCLID"
    ''' <summary>FOLLOW_UP_BOX</summary>
    Private Const SESSION_KEY_FOLLOW_UP_BOX As String = "SearchKey.FOLLOW_UP_BOX"
    ''' <summary>顧客メモクリア用</summary>
    Public Const SESSION_KEY_MEMO_INIT As String = "SearchKey.MEMOINIT"           '9:顧客メモ読み込み完了

    '2012/02/15 TCS 山口 【SALES_2】 START
    ''' <summary>回答ID</summary>
    Private Const SESSION_KEY_ANSWERID As String = "SearchKey.ANSWERID"
    ''' <summary>受注NO</summary>
    Private Const SESSION_KEY_ORDER_NO As String = "SearchKey.ORDER_NO"
    ''' <summary>自社客に紐付く未取引客ID</summary>
    Private Const SESSION_KEY_NEW_CUST_ID As String = "SearchKey.NEW_CUST_ID"
    '2012/02/15 TCS 山口 【SALES_2】 END

    '$01 Add Start
    ''' <summary>
    ''' 顧客名 + 敬称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_NAME As String = "SearchKey.NAME"

    ''' <summary>
    ''' 顧客名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SESSION_KEY_CUSTNAME As String = "SearchKey.CUSTNAME"
    '$01 Add End

    '$01 Add End

    '顧客メモセッションキー
    Public Const SESSION_KEY_MEMO_CUSTSEGMENT As String = "SearchKey.CUSTSEGMENT"      '顧客区分 (1：自社客 / 2：未取引客)
    Public Const SESSION_KEY_MEMO_CUSTOMERCLASS As String = "SearchKey.CUSTOMERCLASS"  '顧客分類 (1：所有者 / 2：使用者 / 3：その他)
    Public Const SESSION_KEY_MEMO_CRCUSTID As String = "SearchKey.CRCUSTID"            '活動先顧客コード
    Public Const SESSION_KEY_MEMO_CRCUSTNAME As String = "SearchKey.CRCUSTNAME"        '活動先顧客名

    '活動結果登録連携
    Public Const CONST_VCLINFO As String = "VCLINFO"

    ''' <summary>担当セールススタッフコード</summary>
    Private Const SESSION_KEY_SALESSTAFFCD As String = "SearchKey.SALESSTAFFCD"

    Private Const ImageFilePath As String = "~/styles/images/SC3080201/"
    ' 2019/05/28 TS 髙橋(龍) TR-SVT-TMT-20170725-001対応 START
    'Private Const ImageFileExt As String = ".png"
    Private Const C_FILE_UPLOAD_EXTENSION As String = "FILE_UPLOAD_EXTENSION" '画像アップロードのファイル拡張子設定
    Private Const ReplaceParameter As String = "{0}" '置換対象文字列
    ' 2019/05/28 TS 髙橋(龍) TR-SVT-TMT-20170725-001対応 END
    Private Const ClassString As String = "class"
    Private Const SelectedButtonString As String = "selectedButton"

    Private Const ContactFlgOn As String = "1"
    Private Const ContactFlgOff As String = "0"

    Private Const ORGCUSTFLG As String = "1" ' 自社客/未取引客フラグ (1：自社客)
    Private Const NEWCUSTFLG As String = "2" ' 自社客/未取引客フラグ (2：未取引客)

    Private Const NOTEXT As String = "-"

    Private Const CUSTYPE_CORPORATION As String = "0"
    Private Const CUSTYPE_PERSON As String = "1"


    '2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
    Private Const CST_JOIN_TYPE_I As String = "1"
    Private Const CST_JOIN_TYPE_C As String = "2"
    '2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END

    ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
    Private Const CST_VIP_FLG_ON As String = "1" '顧客VIPフラグ
    Private Const VCL_VIP_FLG_ON As String = "1" '車両VIPフラグ
    ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END
    ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
    Private Const VCL_VIP_FLG_LOYAL_CUSTOMER As String = "2" '車両VIPフラグ_LoyalCustomer
    ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

    '2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応 START
    Private Const FACEPIC_UPLOADPATH As String = "FACEPIC_UPLOADPATH"
    '2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応 END

    '2012/06/01 TCS 河原 FS開発 START
    Private Const SNSURL_SEARCH_RENREN As String = "SNSURL_SEARCH_RENREN"
    Private Const SNSURL_ACCOUNT_RENREN As String = "SNSURL_ACCOUNT_RENREN"
    Private Const SNSURL_SEARCH_KAIXIN As String = "SNSURL_SEARCH_KAIXIN"
    Private Const SNSURL_ACCOUNT_KAIXIN As String = "SNSURL_ACCOUNT_KAIXIN"
    Private Const SNSURL_SEARCH_WEIBO As String = "SNSURL_SEARCH_WEIBO"
    Private Const SNSURL_ACCOUNT_WEIBO As String = "SNSURL_ACCOUNT_WEIBO"
    Private Const SEARCH_BAIDU As String = "SEARCH_BAIDU"
    Private Const URL_SCHEME As String = "TABLET_BROWSER_URL_SCHEME"
    Private Const URL_SCHEMES As String = "TABLET_BROWSER_URL_SCHEMES"
    '2012/06/01 TCS 河原 FS開発 END

    Const C_PARAMKEY_DISP_OTHER_DLRNM = "DISP_OTHER_DLRNM"

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    Private Const C_USE_CUSTOMERDATA_CLEANSING_FLG As String = "USE_CUSTOMERDATA_CLEANSING_FLG"  'お客様情報クレンジング機能使用可否フラグ

    Private Const C_USE_DIRECT_BILLING_FLG As String = "USE_DIRECT_BILLING_FLG"                  'お直販機能使用可否フラグ  
    '2017/11/20 TCS 河原 TKM独自機能開発 END  

#Region " 顧客編集ポップアップ自動起動フラグ "
    Public Const CUSTOMER_POPUP_AUTOFLG_OFF As String = "0"
    Public Const CUSTOMER_POPUP_AUTOFLG_ON As String = "1"
#End Region

#Region " 希望連絡方法、時間帯"

    ''' <summary>
    ''' 時間帯クラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const TIMEZONECLASS_1 As Integer = 1
    Public Const TIMEZONECLASS_2 As Integer = 2

    Public Const CONTACTIMAGE_MOBILE As String = "icon_mobile.png"
    Public Const CONTACTIMAGE_HOME As String = "icon_home.png"
    Public Const CONTACTIMAGE_SMS As String = "icon_SMSmail.png"
    Public Const CONTACTIMAGE_EMAIL As String = "icon_Email.png"
    Public Const CONTACTIMAGE_DM As String = "icon_DM.png"

#End Region

#Region " コンタクト履歴 "
    ''' <summary>活動種類アイコン セールス</summary>
    Private Const ACTUALKIND_IMAGE_SALSE As String = "~/Styles/Images/SC3080201/scNscCurriculumListCarIcon1.png"
    ''' <summary>活動種類アイコン CR</summary>
    Private Const ACTUALKIND_IMAGE_CR As String = "~/Styles/Images/SC3080201/ico128.png"

    '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    ''' <summary>活動種類アイコン サービス</summary>
    Private Const ACTUALKIND_IMAGE_SERVICE As String = "~/Styles/Images/SC3080201/actualkind_image_service.png"
    '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    ''' <summary>活動種類 セールス</summary>
    Private Const ACTUALKIND_SALSE As String = "1"
    ''' <summary>活動種類 CR</summary>
    Private Const ACTUALKIND_CR As String = "3"

    '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    ''' <summary>活動種類 サービス</summary>
    Private Const ACTUALKIND_SERVICE As String = "2"
    '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 START
    ''' <summary>活動種類 受注後活動</summary>
    Private Const ACTUALKIND_AFTER_ODR_ACT As String = "4"
    '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 END

    ''' <summary>カウント表示</summary>
    Private Const COUNTVIEW_NO As String = "0" '表示無し
    Private Const COUNTVIEW_YES As String = "1" '表示あり

    '2012/02/15 TCS 山口 【SALES_2】 START
    ''' <summary>
    ''' ステータス セールス
    ''' 1: Walk-in、2: Prospect、3: Hot、4: Success、5: Give-up
    ''' 6: Allocation(振当てまち)、7: Paymen(入金待ち)、8: Delivery(納車待ち)、
    ''' 9: Allocation(振当てTACT)、10: Payment(入金TACT)、11: Delivery(納車TACT)、12: キャンセル
    ''' </summary>
    Private Const CRACTSTATUS_WALK_IN As String = "1"
    Private Const CRACTSTATUS_PROSPECT As String = "2"
    Private Const CRACTSTATUS_HOT As String = "3"
    Private Const CRACTSTATUS_SUCCESS As String = "4"
    Private Const CRACTSTATUS_GIVE_UP As String = "5"
    Private Const CRACTSTATUS_ALLOCATION As String = "6"
    Private Const CRACTSTATUS_PAYMEN As String = "7"
    Private Const CRACTSTATUS_DELIVERY As String = "8"
    Private Const CRACTSTATUS_ALLOCATION_TACT As String = "9"
    Private Const CRACTSTATUS_PAYMEN_TACT As String = "10"
    Private Const CRACTSTATUS_DELIVERY_TACT As String = "11"
    Private Const CRACTSTATUS_CANCEL As String = "12"

    Private Const CRACTSTATUS_ICON_PATH As String = "~/Styles/Images/SC3080201/"
    Private Const CRACTSTATUS_WALK_IN_ICON As String = "scNscCurriculumListStarIcon1.png"
    Private Const CRACTSTATUS_PROSPECT_ICON As String = "scNscCurriculumListStarIcon2.png"
    Private Const CRACTSTATUS_HOT_ICON As String = "scNscCurriculumListStarIcon3.png"
    Private Const CRACTSTATUS_SUCCESS_ICON As String = "scNscCurriculumListStarIcon4.png"
    Private Const CRACTSTATUS_GIVE_UP_ICON As String = "scNscCurriculumListStarIcon5.png"
    Private Const CRACTSTATUS_ALLOCATION_ICON As String = "nsc40icn06.png"
    Private Const CRACTSTATUS_PAYMEN_ICON As String = "nsc40icn04.png"
    Private Const CRACTSTATUS_DELIVERY_ICON As String = "nsc40icn02.png"
    Private Const CRACTSTATUS_ALLOCATION_TACT_ICON As String = "nsc40icn05.png"
    Private Const CRACTSTATUS_PAYMEN_TACT_ICON As String = "nsc00icn4.png"
    Private Const CRACTSTATUS_DELIVERY_TACT_ICON As String = "nsc00icn3.png"
    Private Const CRACTSTATUS_CANCEL_ICON As String = "cancelicn.png"

    ''' <summary>
    ''' ステータス 苦情
    ''' 1:1次対応中 、2: 最終対応中、3: 完了
    ''' </summary>
    Private Const CRACTSTATUS_RESPONSE As String = "1"
    Private Const CRACTSTATUS_LAST_RESPONSE As String = "2"
    Private Const CRACTSTATUS_END As String = "3"

    Private Const CRACTSTATUS_RESPONSE_ICON As String = "response.png"
    Private Const CRACTSTATUS_LAST_RESPONSE_ICON As String = "last_response.png"
    Private Const CRACTSTATUS_END_ICON As String = "end.png"

    ' ''' <summary>権限</summary>
    'Private Const OPERATIONCODE_CCM As String = "1" 'Call Centre Manager
    'Private Const OPERATIONCODE_CCO As String = "2" 'Call Centre Operator
    'Private Const OPERATIONCODE_AHO As String = "3" 'Assistant (H/O)
    'Private Const OPERATIONCODE_AB As String = "4" 'Assistant (Branch)
    'Private Const OPERATIONCODE_SGM As String = "5" 'Sales General Manager
    'Private Const OPERATIONCODE_BM As String = "6" 'Branch Manager
    'Private Const OPERATIONCODE_SSM As String = "7" 'Sales Manager
    'Private Const OPERATIONCODE_SS As String = "8" 'Sales Staff 
    'Private Const OPERATIONCODE_SA As String = "9" 'Service Adviser
    'Private Const OPERATIONCODE_SM As String = "10" 'Service Manager

    Private Const OPERATIONCODE_IMAGE_PATH As String = "~/Styles/Images/Authority/"
    'Private Const OPERATIONCODE_IMAGE_CCO As String = "CCO.png"
    'Private Const OPERATIONCODE_IMAGE_MANAGER As String = "Manager.png"
    'Private Const OPERATIONCODE_IMAGE_RECEPTIONIST As String = "Receptionist.png"
    'Private Const OPERATIONCODE_IMAGE_SA As String = "SA.png"
    'Private Const OPERATIONCODE_IMAGE_SC As String = "SC.png"

    ''' <summary>
    ''' 検索件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAGEROWS As Integer = 20
    ''' <summary>
    ''' 最大表示件数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MAXCACHEROWS As Integer = 100
    ''' <summary>
    ''' TACT取得キー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTACT_TABLET_DMS_NAME As String = "CONTACT_TABLET_DMS_NAME"

    '2012/02/15 TCS 山口 【SALES_2】 END
#End Region

#Region "顔写真"
    '顔写真
    Private Const IMAGEFILE_L As String = "_L"
    Private Const IMAGEFILE_M As String = "_M"
    Private Const IMAGEFILE_S As String = "_S"

#End Region

#Region "エラーメッセージ"
    Private Const ERRMSGID_10909 As Integer = 10909
    Private Const ERRMSGID_10910 As Integer = 10910
    Private Const ERRMSGID_10911 As Integer = 10911
#End Region

    '2018/04/24 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
#Region "個人法人区分使用可否フラグ"
    'システム設定名_個人法人区分使用可否フラグ
    Private Const PNAME_USED_FLG_PFTYPE As String = "USED_FLG_PFTYPE"
    '個人法人区分使用可否フラグ_ON（"1"：使用可）
    Private Const USED_FLG_PFTYPE_ON As String = "1"
#End Region
    '2018/04/24 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

#End Region

#Region "プロパティ"
    Private Property FamilyNumber As Integer

#End Region

#Region "Page_Load"
    ''' <summary>
    ''' ロード次の処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <remarks></remarks>
    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) START 
        If ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack Then
            Return
        End If
        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) END 

        '2017/11/20 TCS 河原 TKM独自機能開発 START
        'お客様情報クレンジング機能使用可否フラグの取得
        Me.Use_Customerdata_Cleansing_Flg.Value = ActivityInfoBusinessLogic.GetSystemSetting(C_USE_CUSTOMERDATA_CLEANSING_FLG)

        '直販機能使用可否フラグの取得
        Me.Use_Direct_Billing_Flg.Value = ActivityInfoBusinessLogic.GetSystemSetting(C_USE_DIRECT_BILLING_FLG)
        '2017/11/20 TCS 河原 TKM独自機能開発 End

        If Not Page.IsPostBack Then
            '初期表示
            _PageOpen()
        ElseIf Not Page.IsCallback Then

            Me.NameListActvctgryReasonListVisiblePanel.Visible = False
            Me.CustomerEditVisiblePanel.Visible = False
            Me.CustomerCarEditVisiblePanel.Visible = False
            Me.CustomerCarVisiblePanel.Visible = False
            Me.OccupationVisiblePanel.Visible = False
            Me.FamilyVisiblePanel.Visible = False
            Me.HobbyVisiblePanel.Visible = False
            Me.ContactVisiblePanel.Visible = False
        End If

        'CSSurvey設定
        If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) = True) Then
            _SetParameters()
            '活性
            SC3080215.Visible = True
        Else
            '非活性
            SC3080215.Visible = False
        End If
    End Sub

    ''' <summary>
    ''' ページ読み込みの処理を実施します。
    ''' </summary>
    ''' <param name="sender">イベント発生元</param>
    ''' <param name="e">イベントデータ</param>
    ''' <History>2012/04/12 TCS 安田 【SALES_2】来店から車両登録が不可（ユーザー課題 No.49）</History>
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) START 
        If ScriptManager.GetCurrent(Me.Page).IsInAsyncPostBack Then
            Return
        End If
        '2014/08/01 TCS 外崎 性能改善(ポップアップ高速化対応) END 

        If Page.IsPostBack Then
            '一部ポップアップを再表示する

            '活動先顧客コード(オリジナルID：自社客 / 未取引客連番：未取引客)
            Dim crcustId As String = String.Empty
            If ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) Then
                crcustId = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
            End If

            '既存客モードの場合のみ実行
            If Not String.IsNullOrEmpty(Trim(crcustId)) Then
                Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用
                '顧客種別(1：自社客 / 2：未取引客)
                Dim cstKind As String = String.Empty
                If ContainsKey(ScreenPos.Current, SESSION_KEY_CSTKIND) Then
                    cstKind = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                End If

                '2012/03/08 TCS 山口 【SALES_2】性能改善 START
                'If String.Equals(cstKind, ORGCUSTFLG) Then
                '    '自社客車両取得
                '    Me._ShowOrgVehicle(params, FIRSTLOAD)
                'ElseIf String.Equals(cstKind, NEWCUSTFLG) Then
                '    '未取引客車両取得
                '    Me._ShowNewVehicle(params, FIRSTLOAD)
                'End If

                ''家族ポップアップ再表示
                'Me.CustomerRelatedFamilyLoad()
                '2012/03/08 TCS 山口 【SALES_2】性能改善 END

                '顧客情報編集の初期処理
                'Call CustomerEditInitialize()
                'コールバックスプリクト登録
                ScriptManager.RegisterStartupScript(Me, Me.GetType(), _
                                                    "Callback", _
                                                    String.Format(CultureInfo.InvariantCulture, _
                                                                  "callback.beginCallback = function () {{ {0}; }};", _
                                                                  Page.ClientScript.GetCallbackEventReference(Me, _
                                                                                                              "callback.packedArgument", _
                                                                                                              "callback.endCallback", _
                                                                                                              "", _
                                                                                                              True)), _
                                                                  True)

                '2012/03/08 TCS 山口 【SALES_2】性能改善 START
                ''車両編集の初期処理
                'Call VehicleInitialize()
                '2012/03/08 TCS 山口 【SALES_2】性能改善 END

                'ポップアップ等抑制処理
                Me.SetControlSate()
            End If
        Else
            '2012/04/12 TCS 安田 【SALES_2】来店から車両登録が不可（ユーザー課題 No.49） START
            '車両編集を自動起動するフラグをセットする
            If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VCLREGNODISPFLG) = True) Then

                'セッション情報 車両登録No初期表示フラグ (1:車両登録Noを表示する)
                Dim regDispFlg As String = String.Empty
                regDispFlg = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_VCLREGNODISPFLG, False), String)

                If (regDispFlg.Equals(SC3080206BusinessLogic.RegNoDispBtn) = True) Then
                    Me.vehiclePopUpAutoOpenFlg.Value = SC3080206BusinessLogic.VehicleOpenFlg
                End If

            End If
            '2012/04/12 TCS 安田 【SALES_2】来店から車両登録が不可（ユーザー課題 No.49） END
        End If

        '2012/02/15 TCS 山口 【SALES_2】 START
        'CS Surveyボタン設定
        CSSurveyLabelOn.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10177))
        CSSurveyLabelOff.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10177))
        '件数取得
        Dim csSurveyCount As Integer = CType(SC3080215, Pages_SC3080215).CSSurveyCount
        If csSurveyCount = 0 Then
            CSSurveyButton.Visible = False
            CSSurveyButtonOff.Visible = True
        Else
            CSSurveyButton.Visible = True
            CSSurveyButtonOff.Visible = False
        End If
        '2012/02/15 TCS 山口 【SALES_2】 END

        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        Me.mainteamount.Value = WebWordUtility.GetWord(10197)
        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END



    End Sub
#End Region

#Region "初期処理"
    Private Sub _PageOpen()

        '顧客種別(1：自社客 / 2：未取引客)
        Dim cstKind As String = String.Empty
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CSTKIND) Then
            cstKind = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
        End If

        '活動先顧客コード(オリジナルID：自社客 / 未取引客連番：未取引客)
        Dim crcustId As String = String.Empty
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) Then
            crcustId = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
        End If

        '2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応 START
        GetCustomerPicturePath()
        '2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応 END

        '2012/06/01 TCS 河原 FS開発 START
        fsInitialize()
        '2012/06/01 TCS 河原 FS開発 END

        '既存客モード、新規登録モードの判定
        If String.IsNullOrEmpty(Trim(crcustId)) Then
            '新規登録モード

            '各表示欄を新規登録の状態に変更
            '///////////////////////////////////////////
            '顧客
            EditCustomerNamePanel.Visible = False
            NewCustomerNamePanel.Visible = True
            '車両
            EditCustomerCarTypePanel.Visible = False
            NewCustomerCarTypePanel.Visible = True
            '職業
            CustomerRelatedOccupationSelectedPanel.Visible = False
            CustomerRelatedOccupationNewPanel.Visible = True
            '家族
            CustomerRelatedFaimySelectedEditPanel.Visible = False
            CustomerRelatedFamilySelectedNewPanel.Visible = True
            '趣味
            CustomerRelatedHobbySelectedEditPanel.Visible = False
            CustomerRelatedHobbySelectedNewPanel.Visible = True
            '連絡
            CustomerRelatedContactSelectedEditPanel.Visible = False
            CustomerRelatedContactSelectedNewPanel.Visible = True
            'メモ
            EditCustomerMemoPanel.Visible = False
            NewCustomerMemoPanel.Visible = True
            '///////////////////////////////////////////

            '顧客編集ポップアップ以外のポップアップを実行不可にする
            '///////////////////////////////////////////
            Me.CustomerCarEditPopUpOpenEria1.Attributes.Item("onclick") = String.Empty
            Me.CustomerCarEditPopUpOpenEria2.Attributes.Item("onclick") = String.Empty
            Me.CustomerRelatedOccupationArea.Attributes.Item("onclick") = String.Empty
            Me.CustomerRelatedFamilyArea.Attributes.Item("onclick") = String.Empty
            Me.CustomerRelatedHobbyArea.Attributes.Item("onclick") = String.Empty
            Me.CustomerRelatedContactArea.Attributes.Item("onclick") = String.Empty
            Me.CustomerMemo_Click.Attributes.Item("onclick") = String.Empty
            '///////////////////////////////////////////

            '顧客編集、車両編集ポップアップ用の値セット
            selectVinHidden.Value = String.Empty
            selectSeqnoHidden.Value = String.Empty
            editModeHidden.Value = CStr(SC3080205BusinessLogic.ModeCreate)
            editVehicleModeHidden.Value = CStr(SC3080205BusinessLogic.ModeCreate)

            '顧客編集ポップアップ自動起動フラグON
            customerEditPopUpAutoOpenFlg.Value = CUSTOMER_POPUP_AUTOFLG_ON

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            ''顧客情報編集の初期処理
            'Call CustomerEditInitialize()
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            '$01ReLoad用URL作成
            CustomerReLoadURL.Value = Me.ResolveUrl("~/Pages/" & "SC3080201.aspx")

            '2012/02/15 TCS 山口 【SALES_2】 START
            '重要事項表示OFF
            '重要連絡表示フラグOFF
            ImportantContactArea.Visible = False
            'CostomRepeater高さ調節
            ContactHistoryRepeater.Height = 500
            'コンタクト履歴非表示
            ContactHistoryRepeater.Visible = False
            '2012/02/15 TCS 山口 【SALES_2】 END
        Else
            '既存客モード

            '全ポップアップを実行可にする
            '///////////////////////////////////////////
            '処理不要
            '///////////////////////////////////////////

            Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用

            '初期データ取得
            If String.Equals(cstKind, ORGCUSTFLG) Then
                '自社客取得、画面設定
                Me._ShowOrgCustomer(params)

                '自社客車両取得
                Me._ShowOrgVehicle(params)
            ElseIf String.Equals(cstKind, NEWCUSTFLG) Then
                '未取引客取得、画面設定
                Me._ShowNewCustomer(params)

                '未取引客車両取得
                Me._ShowNewVehicle(params)
            End If

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            ''顧客職業取得
            'Dim occupationDataTbl As SC3080201DataSet.SC3080201CustomerOccupationDataTable _
            '    = SC3080201BusinessLogic.GetOccupationData(params)
            'occupationDataTbl = SC3080201BusinessLogic.EditOccupatonData(occupationDataTbl)
            'Me._SetCustomerRelatedOccupationPopUp(occupationDataTbl)
            'Me._SetCustomerRelatedOccupationArea(occupationDataTbl)
            Me.SetOccupationButtonArea(params)
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            ''家族構成マスタ取得
            'Dim custFamilyMstDataTbl As SC3080201DataSet.SC3080201CustomerFamilyMstDataTable _
            '    = SC3080201BusinessLogic.GetCustFamilyMstData(params)

            'Me._SetCustomerRelatedFamilyPopUpRelationshipList(custFamilyMstDataTbl)

            ''顧客家族構成取得
            'Dim custFamilyDataTbl As SC3080201DataSet.SC3080201CustomerFamilyDataTable _
            '    = SC3080201BusinessLogic.GetCustFamilyData(params)
            ''不明行追加前に件数保持
            'Dim familyCount As Integer = custFamilyDataTbl.Rows.Count
            ''本人行編集、不明行追加
            'custFamilyDataTbl = SC3080201BusinessLogic.EditCustFamilyData(custFamilyDataTbl, custFamilyMstDataTbl)
            'Me._SetCustomerRelatedFamilyArea(familyCount)
            'Me._SetCustomerRelatedFamilyPopUpFamilyList(custFamilyDataTbl, familyCount)
            Me.SetFamilyButtonArea(params)
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            ''顧客趣味取得
            'Dim hobbyDataTbl As SC3080201DataSet.SC3080201CustomerHobbyDataTable _
            '    = SC3080201BusinessLogic.GetHobbyData(params)

            'Me._SetCustomerRelatedHobbyArea(hobbyDataTbl)
            Me.SetHobbyButtonArea(params)
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            ''希望コンタクト方法取得
            'Dim contactFlg As SC3080201DataSet.SC3080201ContactFlgDataTable _
            '    = SC3080201BusinessLogic.GetContactFlg(params)
            'Me._SetCustomerRelatedContactPopupContactTool(contactFlg)

            ''TIMEZONECLASS分ループ
            'For i = TIMEZONECLASS_1 To TIMEZONECLASS_2
            '    params.Rows(0).Item(params.TIMEZONECLASSColumn.ColumnName) = i

            '    '希望連絡時間帯取得
            '    Dim timeZoneDataTbl As SC3080201DataSet.SC3080201ContactTimeZoneDataTable _
            '        = SC3080201BusinessLogic.GetTimeZoneData(params)

            '    '希望連絡曜日取得
            '    Dim weekOfDayDataTbl As SC3080201DataSet.SC3080201ContactWeekOfDayDataTable _
            '        = SC3080201BusinessLogic.GetWeekOfDayData(params)
            '    'ポップアップ画面設定
            '    Me._SetCustomerRelatedContactPopup(timeZoneDataTbl, weekOfDayDataTbl, i)
            'Next
            Me.SetContactButtonArea(params)
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            '最新顧客メモ取得、画面設定
            Me._ShowLastCustMemo(params)

            '2012/02/15 TCS 山口 【SALES_2】 START
            '重要時効取得、画面設定
            Me._ShowImportantContact(params)
            '2012/02/15 TCS 山口 【SALES_2】 END

            'コンタクト履歴取得、画面設定
            Me._ShowContactHistory()

            '顧客編集ポップアップ自動起動フラグOFF
            customerEditPopUpAutoOpenFlg.Value = CUSTOMER_POPUP_AUTOFLG_OFF

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            ''顧客情報編集の初期処理
            'Call CustomerEditInitialize()
            ''車両編集の初期処理
            'Call VehicleInitialize()
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            'ポップアップ等抑制処理
            Me.SetControlSate()

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            '顧客メモの情報をセッションに保持する
            Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CUSTSEGMENT, cstKind) '顧客区分 (1：自社客 / 2：未取引客)
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            '2017/11/20 TCS 河原 TKM独自機能開発 START
            CustomerEditInitialize()

            If String.Equals(Me.Use_Customerdata_Cleansing_Flg.Value, "1") Then
                'クレンジング使用onの場合、クレンジングチェックを実施
                CustomerDataCleansing()
                Me.CleansingResult.Value = CleansingRslt
            Else
                'クレンジング使用offの場合
                Me.CleansingResult.Value = "0"
            End If

            '2017/11/20 TCS 河原 TKM独自機能開発 END

        End If
        '2012/02/15 TCS 山口 【SALES_2】 START
        'コールバックスプリクト登録
        ScriptManager.RegisterStartupScript(Me, Me.GetType(), _
                                           "Callback", _
                                           String.Format(CultureInfo.InvariantCulture, _
                                                         "callback.beginCallback = function () {{ {0}; }};", _
                                                         Page.ClientScript.GetCallbackEventReference(Me, _
                                                                                                     "callback.packedArgument", _
                                                                                                     "callback.endCallback", _
                                                                                                     "", _
                                                                                                     True)), _
                                                         True)
        '2012/02/15 TCS 山口 【SALES_2】 END

    End Sub

#End Region

#Region "各種データ取得、画面設定"
    ''' <summary>
    ''' 自社客情報の取得および画面設定
    ''' </summary>
    ''' <param name="params"></param>
    ''' <remarks></remarks>
    Private Sub _ShowOrgCustomer(ByVal params As SC3080201DataSet.SC3080201ParameterDataTable)

        '自社客取得
        Dim orgCustomerDataTbl As SC3080201DataSet.SC3080201OrgCustomerDataTable _
            = SC3080201BusinessLogic.GetOrgCustomerData(params)
        '画面設定
        Me._SetControlOrgCustomer(orgCustomerDataTbl)
    End Sub

    ''' <summary>
    ''' 未取引客情報の取得および画面設定
    ''' </summary>
    ''' <param name="params"></param>
    ''' <remarks></remarks>
    Private Sub _ShowNewCustomer(ByVal params As SC3080201DataSet.SC3080201ParameterDataTable)

        '未取引客取得
        Dim newCustomerDataTbl As SC3080201DataSet.SC3080201NewCustomerDataTable _
            = SC3080201BusinessLogic.GetNewCustomerData(params)
        '画面設定
        Me._SetControlNewCustomer(newCustomerDataTbl)
    End Sub

    ''' <summary>
    ''' 自社客車両情報の取得および画面設定
    ''' </summary>
    ''' <param name="params"></param>
    ''' <remarks></remarks>
    Private Sub _ShowOrgVehicle(ByVal params As SC3080201DataSet.SC3080201ParameterDataTable)

        '自社客車両取得
        Dim orgVehicleDataTbl As SC3080201DataSet.SC3080201OrgVehicleDataTable _
            = SC3080201BusinessLogic.GetOrgVehicleData(params)

        '2012/03/08 TCS 山口 【SALES_2】性能改善 START

        Dim editOrgVehicleDataTbl As New SC3080201DataSet.SC3080201OrgVehicleDataTable

        '取得した自社客車両データを並び替え
        editOrgVehicleDataTbl = Me._editOrgVehicleDataTbl(orgVehicleDataTbl, params)

        '画面設定 自社客車両エリア
        Me._setOrgCustomerCarTypeArea(editOrgVehicleDataTbl)

        ''キー取得
        'Dim paramsRow As SC3080201DataSet.SC3080201ParameterRow
        'paramsRow = CType(params.Rows(0), SC3080201DataSet.SC3080201ParameterRow)
        'Dim vclID As String = paramsRow.VCLID

        'Dim editOrgVehicleDataTbl As New SC3080201DataSet.SC3080201OrgVehicleDataTable
        'If String.IsNullOrEmpty(vclID) Then
        '    'キーが存在しない場合そのまま
        '    editOrgVehicleDataTbl = orgVehicleDataTbl
        '    '新規登録された場合、セッションにキーを登録
        '    If editOrgVehicleDataTbl.Rows.Count = 1 Then
        '        Me.SetValue(ScreenPos.Current, SESSION_KEY_VCLID, editOrgVehicleDataTbl.Rows(0).Item(editOrgVehicleDataTbl.KEYColumn.ColumnName))
        '        Me.customerCarsSelectedHiddenField.Value = CStr(editOrgVehicleDataTbl.Rows(0).Item(editOrgVehicleDataTbl.KEYColumn.ColumnName))
        '    End If
        'Else
        '    'キーが存在する場合、1行目に
        '    Dim copyDataRow As SC3080201DataSet.SC3080201OrgVehicleRow = CType(editOrgVehicleDataTbl.NewRow, SC3080201DataSet.SC3080201OrgVehicleRow)
        '    Dim keyFlg As Boolean = False
        '    For Each orgVehicleRow In orgVehicleDataTbl
        '        If String.Equals(orgVehicleRow.KEY, vclID) Then
        '            'コピー
        '            copyDataRow.ItemArray = orgVehicleRow.ItemArray
        '            keyFlg = True
        '        Else
        '            '別tableに追加
        '            editOrgVehicleDataTbl.Rows.Add(orgVehicleRow.ItemArray)
        '        End If
        '    Next
        '    '別table1行目に追加
        '    If keyFlg Then
        '        editOrgVehicleDataTbl.Rows.InsertAt(copyDataRow, 0)
        '    End If
        'End If

        ''初期読み込みの場合、選択行を強制的に1行目に
        'If String.Equals(flg, FIRSTLOAD) Then
        '    If editOrgVehicleDataTbl.Rows.Count > 0 Then
        '        Me.customerCarsSelectedHiddenField.Value = CStr(editOrgVehicleDataTbl.Rows(0).Item(editOrgVehicleDataTbl.KEYColumn.ColumnName))
        '    End If
        'End If

        'Me._SetControlOrgVehicle(editOrgVehicleDataTbl)
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END
    End Sub

    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
    ''' <summary>
    ''' 自社客車両情報の取得およびポップアップ設定
    ''' </summary>
    ''' <param name="params"></param>
    ''' <remarks></remarks>
    Private Sub _ShowOrgVehiclePopup(ByVal params As SC3080201DataSet.SC3080201ParameterDataTable)

        '自社客車両取得
        Dim orgVehicleDataTbl As SC3080201DataSet.SC3080201OrgVehicleDataTable _
            = SC3080201BusinessLogic.GetOrgVehicleData(params)

        Dim editOrgVehicleDataTbl As New SC3080201DataSet.SC3080201OrgVehicleDataTable

        '取得した自社客車両データを並び替え
        editOrgVehicleDataTbl = _editOrgVehicleDataTbl(orgVehicleDataTbl, params)

        '画面設定 自社客車両選択ポップアップ
        Me._SetControlOrgVehicle(editOrgVehicleDataTbl)

    End Sub
    '2012/03/08 TCS 山口 【SALES_2】性能改善 END

    ''' <summary>
    ''' 未取引客車両情報の取得および画面設定
    ''' </summary>
    ''' <param name="params"></param>
    ''' <remarks></remarks>
    Private Sub _ShowNewVehicle(ByVal params As SC3080201DataSet.SC3080201ParameterDataTable)

        '未取引客車両取得
        Dim newVehicleDataTbl As SC3080201DataSet.SC3080201NewVehicleDataTable _
            = SC3080201BusinessLogic.GetNewVehicleData(params)

        '2012/03/08 TCS 山口 【SALES_2】性能改善 START

        Dim editNewVehicleDataTbl As New SC3080201DataSet.SC3080201NewVehicleDataTable

        '取得した未取引客車両データを並び替え
        editNewVehicleDataTbl = _editNewVehicleDataTbl(newVehicleDataTbl, params)

        '画面設定 未取引客車両エリア
        Me._setNewCustomerCarTypeArea(editNewVehicleDataTbl)

        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        ''キー取得
        'Dim paramsRow As SC3080201DataSet.SC3080201ParameterRow
        'paramsRow = CType(params.Rows(0), SC3080201DataSet.SC3080201ParameterRow)
        'Dim vclID As String = paramsRow.VCLID

        'Dim editNewVehicleDataTbl As New SC3080201DataSet.SC3080201NewVehicleDataTable
        'If String.IsNullOrEmpty(vclID) Then
        '    'キーが存在しない場合そのまま
        '    editNewVehicleDataTbl = newVehicleDataTbl
        '    '新規登録された場合、セッションにキーを登録
        '    If editNewVehicleDataTbl.Rows.Count = 1 Then
        '        Me.SetValue(ScreenPos.Current, SESSION_KEY_VCLID, editNewVehicleDataTbl.Rows(0).Item(editNewVehicleDataTbl.KEYColumn.ColumnName))
        '        Me.customerCarsSelectedHiddenField.Value = CStr(editNewVehicleDataTbl.Rows(0).Item(editNewVehicleDataTbl.KEYColumn.ColumnName))
        '    End If
        'Else
        '    'キーが存在する場合、1行目に
        '    Dim copyDataRow As SC3080201DataSet.SC3080201NewVehicleRow = CType(editNewVehicleDataTbl.NewRow, SC3080201DataSet.SC3080201NewVehicleRow)
        '    Dim keyFlg As Boolean = False
        '    For Each newVehicleRow In newVehicleDataTbl
        '        If String.Equals(newVehicleRow.KEY, vclID) Then
        '            'コピー
        '            copyDataRow.ItemArray = newVehicleRow.ItemArray
        '            keyFlg = True
        '        Else
        '            '別tableに追加
        '            editNewVehicleDataTbl.Rows.Add(newVehicleRow.ItemArray)
        '        End If
        '    Next
        '    '別table1行目に追加
        '    If keyFlg Then
        '        editNewVehicleDataTbl.Rows.InsertAt(copyDataRow, 0)
        '    End If
        'End If

        ''初期読み込みの場合、選択行を強制的に1行目に
        'If String.Equals(flg, FIRSTLOAD) Then
        '    If editNewVehicleDataTbl.Rows.Count > 0 Then
        '        Me.customerCarsSelectedHiddenField.Value = CStr(editNewVehicleDataTbl.Rows(0).Item(editNewVehicleDataTbl.KEYColumn.ColumnName))
        '    End If
        'End If

        ''画面設定
        'Me._SetControlNewVehicle(editNewVehicleDataTbl)

        '2012/03/08 TCS 山口 【SALES_2】性能改善 END
    End Sub

    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
    ''' <summary>
    ''' 未取引客車両情報の取得およびポップアップ設定
    ''' </summary>
    ''' <param name="params"></param>
    ''' <remarks></remarks>
    Private Sub _ShowNewVehiclePopup(ByVal params As SC3080201DataSet.SC3080201ParameterDataTable)

        '未取引客車両取得
        Dim newVehicleDataTbl As SC3080201DataSet.SC3080201NewVehicleDataTable _
            = SC3080201BusinessLogic.GetNewVehicleData(params)

        Dim editNewVehicleDataTbl As New SC3080201DataSet.SC3080201NewVehicleDataTable

        '取得した未取引客車両データを並び替え
        editNewVehicleDataTbl = _editNewVehicleDataTbl(newVehicleDataTbl, params)

        '画面設定 未取引客車両選択ポップアップ
        Me._SetControlNewVehicle(editNewVehicleDataTbl)

    End Sub
    '2012/03/08 TCS 山口 【SALES_2】性能改善 END

    ''' <summary>
    ''' 顧客メモの取得および画面設定
    ''' </summary>
    ''' <param name="params"></param>
    ''' <remarks></remarks>
    Private Sub _ShowLastCustMemo(ByVal params As SC3080201DataSet.SC3080201ParameterDataTable)

        '最新顧客メモ取得
        Dim lastCustMemoDataTbl As SC3080201DataSet.SC3080201LastCustomerMemoDataTable _
            = SC3080201BusinessLogic.GetLastCustMemoData(params)
        '画面設定
        ControltimeLastCustMemo(lastCustMemoDataTbl)

    End Sub

    '2012/02/15 TCS 山口 【SALES_2】 START
    ''' <summary>
    ''' 重要事項の取得および画面設定
    ''' </summary>
    ''' <param name="params"></param>
    ''' <remarks></remarks>
    Private Sub _ShowImportantContact(ByVal params As SC3080201DataSet.SC3080201ParameterDataTable)
        '重要事項取得
        Dim importantContactTbl As SC3080201DataSet.SC3080201ImportantContactDataTable _
            = SC3080201BusinessLogic.GetImportantContact(params)

        Me._SetControlImportantContact(importantContactTbl)
    End Sub
    '2012/02/15 TCS 山口 【SALES_2】 END

    ''' <summary>
    ''' コンタクト履歴の取得および画面設定
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _ShowContactHistory()
        '2012/02/15 TCS 山口 【SALES_2】 START
        ''コンタクト履歴取得
        'Dim contactHistoryTbl As SC3080201DataSet.SC3080201ContactHistoryDataTable _
        '    = SC3080201BusinessLogic.GetContactHistoryData(params)

        'Me._SetControlContactHistory(contactHistoryTbl)

        '次の{0}件を読み込む...
        Me.ContactHistoryRepeater.ForwardPagerLabel = String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(10186), PAGEROWS)
        '前の{0}件を読み込む...
        Me.ContactHistoryRepeater.RewindPagerLabel = String.Format(CultureInfo.InvariantCulture, WebWordUtility.GetWord(10187), PAGEROWS)

        'PageRows、maxCacheRowsの設定
        Me.ContactHistoryRepeater.PageRows = PAGEROWS
        Me.ContactHistoryRepeater.MaxCacheRows = MAXCACHEROWS

        'web.configよりログイン権限でセールス・サービスの判断を行なう
        Dim val As String = _GetConfigValue(SystemConfiguration.Current.Manager.StaffDivision)
        If (val.Equals("Sales")) Then
            'セールスの場合、コンタクト履歴の初期タブを変更する
            ContactHistoryTabIndex.Value = "1"
        End If
        '2012/02/15 TCS 山口 【SALES_2】 END

    End Sub
    '2012/02/15 TCS 山口 【SALES_2】 START
    ''' <summary>
    ''' web.configの指定ノードの値を取得
    ''' </summary>
    ''' <param name="config">Configuration.ClassSection</param>
    ''' <returns>取得した値</returns>
    ''' <remarks></remarks>
    Private Function _GetConfigValue(ByVal config As Toyota.eCRB.SystemFrameworks.Configuration.ClassSection) As String
        Dim rntVal As String = String.Empty
        Dim staff As StaffContext = StaffContext.Current

        If config IsNot Nothing Then
            Dim setting As Toyota.eCRB.SystemFrameworks.Configuration.Setting = config.GetSetting(String.Empty)
            If (setting IsNot Nothing) Then
                rntVal = DirectCast(setting.GetValue(CStr(staff.OpeCD)), String)
            End If
        End If

        If rntVal Is Nothing Then
            rntVal = String.Empty
        End If

        Return rntVal
    End Function
    '2012/02/15 TCS 山口 【SALES_2】 END

    '2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応 START
    ''' <summary>
    ''' 顔写真アップロードパス取得
    ''' </summary>
    ''' <remarks>顔写真アップロードパス取得</remarks>
    Protected Sub GetCustomerPicturePath()
        Dim sysenv As New SystemEnvSetting
        Dim rw As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        rw = sysenv.GetSystemEnvSetting(FACEPIC_UPLOADPATH)
        FacePicUploadPath.Value = rw.PARAMVALUE
    End Sub
    '2012/04/19 TCS 河原 【SALES_2】顔写真パス変更対応 END

#End Region

#Region "パラメータセット"
    ''' <summary>
    ''' パラメータセット
    ''' </summary>
    ''' <returns>パラメータ管理テーブル</returns>
    ''' <remarks></remarks>
    Private Function _SetParameters() As SC3080201DataSet.SC3080201ParameterDataTable

        '販売店コード
        Dim dlrCd As String = StaffContext.Current.DlrCD

        '店舗コード
        Dim strCd As String = StaffContext.Current.BrnCD

        '顧客種別(1：自社客 / 2：未取引客)
        Dim cstKind As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)

        '顧客分類(1：所有者 / 2：使用者 / 3：その他)
        Dim customerClass As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)

        '活動先顧客コード(オリジナルID：自社客 / 未取引客連番：未取引客)
        Dim crcustId As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)

        '車両ID(VIN：自社客 / 車両シーケンスNo.：未取引客)
        Dim vclId As String = String.Empty
        If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_VCLID) = True) Then
            vclId = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_VCLID, False), String)
        End If

        '2012/02/15 TCS 山口 【SALES_2】 START
        '回答ID
        Dim anserId As String = String.Empty
        If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_ANSWERID) = True) Then
            anserId = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_ANSWERID, False), String)
        End If
        '自社客に紐付く未取引客ID
        Dim newCustId As String = String.Empty
        If (Me.ContainsKey(ScreenPos.Current, SESSION_KEY_NEW_CUST_ID) = True) Then
            newCustId = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_NEW_CUST_ID, False), String)
        End If

        'CSSurvery用プロパティセット
        CType(SC3080215, Pages_SC3080215).CrcustId = crcustId
        CType(SC3080215, Pages_SC3080215).CstKind = cstKind
        CType(SC3080215, Pages_SC3080215).CustomerClass = customerClass
        CType(SC3080215, Pages_SC3080215).AnswerId = anserId
        CType(SC3080215, Pages_SC3080215).DlrCD = dlrCd
        CType(SC3080215, Pages_SC3080215).TriggerClientId = CSSurveyButton.ID
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 START
        'If String.Equals(cstKind, ORGCUSTFLG) Then
        '    CType(SC3080215, Pages_SC3080215).CrNewCustId = newCustId
        'End If
        ' 2013/06/30 TCS 松月 2013/10対応版　既存流用 END
        '2012/02/15 TCS 山口 【SALES_2】 END

        Using params As New SC3080201DataSet.SC3080201ParameterDataTable '検索条件格納用

            Dim paramsDr As SC3080201DataSet.SC3080201ParameterRow
            paramsDr = params.NewSC3080201ParameterRow

            '検索条件セット
            paramsDr.DLRCD = dlrCd
            paramsDr.STRCD = strCd
            paramsDr.CSTKIND = cstKind
            paramsDr.CUSTOMERCLASS = customerClass
            paramsDr.CRCUSTID = crcustId
            paramsDr.VCLID = vclId
            '2012/02/15 TCS 山口 【SALES_2】 START
            paramsDr.NEWCUSTID = newCustId
            params.Rows.Add(paramsDr)
            '2012/02/15 TCS 山口 【SALES_2】 END

            Return params
        End Using

    End Function
#End Region

#Region "画面設定 自社客情報"
    ''' <summary>
    ''' 画面設定 自社客情報
    ''' </summary>
    ''' <param name="orgCustomerDt">自社客情報</param>
    ''' <remarks></remarks>
    Private Sub _SetControlOrgCustomer(ByVal orgCustomerDt As SC3080201DataSet.SC3080201OrgCustomerDataTable)
        Logger.Info("_SetControlOrgCustomer Start")

        If orgCustomerDt.Rows.Count < 1 Then
            '表示制御 登録前
            EditCustomerNamePanel.Visible = False
            NewCustomerNamePanel.Visible = True

            '顧客編集ポップアップ用値セット
            editModeHidden.Value = CStr(SC3080205BusinessLogic.ModeCreate)
        Else
            '表示制御 登録後
            EditCustomerNamePanel.Visible = True
            NewCustomerNamePanel.Visible = False

            Dim orgCustomerDataRow As SC3080201DataSet.SC3080201OrgCustomerRow
            orgCustomerDataRow = CType(orgCustomerDt.Rows(0), SC3080201DataSet.SC3080201OrgCustomerRow)

            '画面設定
            '顔写真
            If String.IsNullOrEmpty(Trim(orgCustomerDataRow.IMAGEFILE_S)) Then
                Me.facePicturePanel.Visible = True
                Me.facePictureButton.Visible = False
            Else
                Me.facePicturePanel.Visible = False
                Me.facePictureButton.Visible = True
                Me.facePictureButton.ImageUrl = ResolveClientUrl(orgCustomerDataRow.FACEPIC_UPLOADURL & orgCustomerDataRow.IMAGEFILE_S & "?" & Format(DateTimeFunc.Now(StaffContext.Current.DlrCD), "yyyyMMddhhmmss"))
            End If

            Me.customerIdTextBox.Value = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
            'Me.faceFileNameTimeHiddenField.Value = Format(DateTimeFunc.Now(StaffContext.Current.DlrCD), "yyyyMMddhhmmss") '顔写真ファイル名用

            '顧客名称、敬称
            NameLabel.Text = HttpUtility.HtmlEncode(Me._MakeCustomerTitle(orgCustomerDataRow.NAME, orgCustomerDataRow.KEISYO_ZENGO, orgCustomerDataRow.NAMETITLE))
            '顧客メモ画面用
            nameHiddenField.Value = orgCustomerDataRow.NAME

            '顧客コード
            If String.IsNullOrEmpty(Trim(orgCustomerDataRow.CUSTCD)) Then
                DmsLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10108) & " " & NOTEXT)
            Else
                DmsLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10108) & " " & orgCustomerDataRow.CUSTCD)
            End If
            '携帯電話
            If String.IsNullOrEmpty(Trim(orgCustomerDataRow.MOBILE)) Then
                MobileLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                MobileLabel.Text = HttpUtility.HtmlEncode(orgCustomerDataRow.MOBILE)
            End If
            '郵便番号
            If String.IsNullOrEmpty(Trim(orgCustomerDataRow.ZIPCODE)) Then
                ZIPLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                ZIPLabel.Text = HttpUtility.HtmlEncode(orgCustomerDataRow.ZIPCODE)
            End If
            '住所
            If String.IsNullOrEmpty(Trim(orgCustomerDataRow.ADDRESS)) Then
                AddressLabel.Text = NOTEXT
                Me.customerAddressTextBox.Text = NOTEXT
            Else
                '2013/11/27 TCS 市川 Aカード情報相互連携開発 START
                '改行コードを半角スペースに置き換える。
                AddressLabel.Text = Replace(orgCustomerDataRow.ADDRESS, vbCrLf, " ")
                '2013/11/27 TCS 市川 Aカード情報相互連携開発 END
                Me.customerAddressTextBox.Text = orgCustomerDataRow.ADDRESS
            End If
            '電話番号
            If String.IsNullOrEmpty(Trim(orgCustomerDataRow.TELNO)) Then
                TelLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                TelLabel.Text = HttpUtility.HtmlEncode(orgCustomerDataRow.TELNO)
            End If
            'Mail
            'MailLink
            If String.IsNullOrEmpty(Trim(orgCustomerDataRow.EMAIL1)) Then
                EmailLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
                Me.EmailLink.NavigateUrl = String.Empty
            Else
                EmailLabel.Text = HttpUtility.HtmlEncode(orgCustomerDataRow.EMAIL1)
                Me.EmailLink.NavigateUrl = "mailto:" & orgCustomerDataRow.EMAIL1
            End If
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
            '顧客カテゴリ
            If String.Equals(orgCustomerDataRow.CUSTCATEGORY, "0") Then
                custInfoCustCtgryLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(40011))
            ElseIf String.Equals(orgCustomerDataRow.CUSTCATEGORY, "1") Then
                custInfoCustCtgryLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(40012))
            Else
                custInfoCustCtgryLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            End If

            '顧客サブカテゴリ1（個人法人項目）
            If String.IsNullOrEmpty(Trim(orgCustomerDataRow.CUSTSUBCAT1)) Then
                custInfoCustSubCtgry1Label.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                custInfoCustSubCtgry1Label.Text = HttpUtility.HtmlEncode(orgCustomerDataRow.CUSTSUBCAT1)
            End If
            '顧客組織名称
            If String.IsNullOrEmpty(Trim(orgCustomerDataRow.CUSTORGNZNAME)) Then
                custInfoCustOrgnzNameLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                custInfoCustOrgnzNameLabel.Text = HttpUtility.HtmlEncode(orgCustomerDataRow.CUSTORGNZNAME)
            End If
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
            '担当スタッフ名
            If String.IsNullOrEmpty(Trim(orgCustomerDataRow.USERNAME)) Then
                CustomerCarSCNameLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                CustomerCarSCNameLabel.Text = HttpUtility.HtmlEncode(orgCustomerDataRow.USERNAME)
            End If

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            If Not orgCustomerDataRow.IsBIRTHDAYNull Then
                customerBirthday.Value = orgCustomerDataRow.BIRTHDAY
            End If
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
            'VIPアイコン表示
            If Not orgCustomerDataRow.IsVIP_FLGNull _
                AndAlso String.Equals(CST_VIP_FLG_ON, orgCustomerDataRow.VIP_FLG) Then
                VIP_Icon.Visible = True
            Else
                VIP_Icon.Visible = False
            End If
            ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END

            '2012/02/15 TCS 山口 【SALES_2】 START
            '顧客種別
            CustomerKind.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10173))

            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
            '顧客タイプ
            If String.Equals(orgCustomerDataRow.CST_JOIN_TYPE, CST_JOIN_TYPE_I) Then
                'Iマーク
                CustomerTypeIcon.Visible = True
                CustomerType.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(1010001))
            ElseIf String.Equals(orgCustomerDataRow.CST_JOIN_TYPE, CST_JOIN_TYPE_C) Then
                'Cマーク
                CustomerTypeIcon.Visible = True
                CustomerType.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(1010002))
            Else
                'その他
                CustomerTypeIcon.Visible = False
            End If
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END

            '2012/02/15 TCS 山口 【SALES_2】 END

            '顧客編集ポップアップ用値セット
            editModeHidden.Value = CStr(SC3080205BusinessLogic.ModeEdit)

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            '顧客名
            Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CRCUSTNAME, orgCustomerDataRow.NAME)
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            '2012/06/01 TCS 河原 FS開発 START
            If Not orgCustomerDataRow.IsSNSID_RENRENNull Then
                Snsid_Renren_Hidden.Value = orgCustomerDataRow.SNSID_RENREN
            End If

            If Not orgCustomerDataRow.IsSNSID_KAIXINNull Then
                Snsid_Kaixin_Hidden.Value = orgCustomerDataRow.SNSID_KAIXIN
            End If

            If Not orgCustomerDataRow.IsSNSID_WEIBONull Then
                Snsid_Weibo_Hidden.Value = orgCustomerDataRow.SNSID_WEIBO
            End If

            '2013/06/30 TCS 未 2013/10対応版　既存流用 START
            If Not orgCustomerDataRow.IsKEYWORDNull Then
                Keyword_Hidden.Value = Trim(orgCustomerDataRow.KEYWORD)
            End If
            '2013/06/30 TCS 未 2013/10対応版　既存流用 END
            '2012/06/01 TCS 河原 FS開発 END

            '2013/06/30 TCS 未 2013/10対応版　既存流用 START
            If Not orgCustomerDataRow.IsCUSTOMERLOCKVERSIONNull Then
                Me.CustomerLockVersion.Value = CType(orgCustomerDataRow.CUSTOMERLOCKVERSION, String)
            End If

            If Not orgCustomerDataRow.IsCUSTOMERDLRLOCKVERSIONNull Then
                Me.CustomerDLRLockVersion.Value = CType(orgCustomerDataRow.CUSTOMERDLRLOCKVERSION, String)
            End If
            '2013/06/30 TCS 未 2013/10対応版　既存流用 END
        End If
        Logger.Info("_SetControlOrgCustomer End")
    End Sub
#End Region

#Region "画面設定 未取引客情報"
    ''' <summary>
    ''' 画面設定 未取引客情報
    ''' </summary>
    ''' <param name="newCustomerDataTbl">自社客情報</param>
    ''' <remarks></remarks>
    Private Sub _SetControlNewCustomer(ByVal newCustomerDataTbl As SC3080201DataSet.SC3080201NewCustomerDataTable)
        Logger.Info("_SetControlNewCustomer Start")

        If newCustomerDataTbl.Rows.Count < 1 Then
            '表示制御 登録前
            EditCustomerNamePanel.Visible = False
            NewCustomerNamePanel.Visible = True

            '顧客編集ポップアップ用値セット
            editModeHidden.Value = CStr(SC3080205BusinessLogic.ModeCreate)
        Else
            '表示制御 登録後
            EditCustomerNamePanel.Visible = True
            NewCustomerNamePanel.Visible = False

            Dim newCustomerDataRow As SC3080201DataSet.SC3080201NewCustomerRow
            newCustomerDataRow = CType(newCustomerDataTbl.Rows(0), SC3080201DataSet.SC3080201NewCustomerRow)

            '画面設定
            '顔写真
            If String.IsNullOrEmpty(Trim(newCustomerDataRow.IMAGEFILE_S)) Then
                Me.facePicturePanel.Visible = True
                Me.facePictureButton.Visible = False
            Else
                Me.facePicturePanel.Visible = False
                Me.facePictureButton.Visible = True
                Me.facePictureButton.ImageUrl = ResolveClientUrl(newCustomerDataRow.FACEPIC_UPLOADURL & newCustomerDataRow.IMAGEFILE_S & "?" & Format(DateTimeFunc.Now(StaffContext.Current.DlrCD), "yyyyMMddhhmmss"))
            End If

            Me.customerIdTextBox.Value = DirectCast(Me.GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
            'Me.faceFileNameTimeHiddenField.Value = Format(DateTimeFunc.Now(StaffContext.Current.DlrCD), "yyyyMMddhhmmss") '顔写真ファイル名用

            '顧客名称、敬称
            NameLabel.Text = HttpUtility.HtmlEncode(Me._MakeCustomerTitle(newCustomerDataRow.NAME, newCustomerDataRow.KEISYO_ZENGO, newCustomerDataRow.NAMETITLE))
            '顧客メモ画面用
            nameHiddenField.Value = newCustomerDataRow.NAME
            '顧客コード
            DmsLabel.Text = String.Empty
            'DmsLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10108) & " " & NOTEXT)
            '携帯電話
            If String.IsNullOrEmpty(Trim(newCustomerDataRow.MOBILE)) Then
                MobileLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                MobileLabel.Text = HttpUtility.HtmlEncode(newCustomerDataRow.MOBILE)
            End If
            '郵便番号
            If String.IsNullOrEmpty(Trim(newCustomerDataRow.ZIPCODE)) Then
                ZIPLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                ZIPLabel.Text = HttpUtility.HtmlEncode(newCustomerDataRow.ZIPCODE)
            End If
            '住所
            If String.IsNullOrEmpty(Trim(newCustomerDataRow.ADDRESS)) Then
                AddressLabel.Text = NOTEXT
                Me.customerAddressTextBox.Text = NOTEXT
            Else
                '2013/11/27 TCS 市川 Aカード情報相互連携開発 START
                '改行コードを半角スペースに置き換える。
                AddressLabel.Text = Replace(newCustomerDataRow.ADDRESS, vbCrLf, " ")
                '2013/11/27 TCS 市川 Aカード情報相互連携開発 END
                Me.customerAddressTextBox.Text = newCustomerDataRow.ADDRESS
            End If
            '電話番号
            If String.IsNullOrEmpty(Trim(newCustomerDataRow.TELNO)) Then
                TelLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                TelLabel.Text = HttpUtility.HtmlEncode(newCustomerDataRow.TELNO)
            End If
            'Mail
            'MailLink
            If String.IsNullOrEmpty(Trim(newCustomerDataRow.EMAIL1)) Then
                EmailLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
                Me.EmailLink.NavigateUrl = String.Empty
            Else
                EmailLabel.Text = HttpUtility.HtmlEncode(newCustomerDataRow.EMAIL1)
                Me.EmailLink.NavigateUrl = "mailto:" & newCustomerDataRow.EMAIL1
            End If
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
            '顧客カテゴリ
            If String.Equals(newCustomerDataRow.CUSTCATEGORY, "0") Then
                custInfoCustCtgryLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(40011))
            ElseIf String.Equals(newCustomerDataRow.CUSTCATEGORY, "1") Then
                custInfoCustCtgryLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(40012))
            Else
                custInfoCustCtgryLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            End If
            '顧客サブカテゴリ1（個人法人項目）
            If String.IsNullOrEmpty(Trim(newCustomerDataRow.CUSTSUBCAT1)) Then
                custInfoCustSubCtgry1Label.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                custInfoCustSubCtgry1Label.Text = HttpUtility.HtmlEncode(newCustomerDataRow.CUSTSUBCAT1)
            End If
            '顧客組織名称
            If String.IsNullOrEmpty(Trim(newCustomerDataRow.CUSTORGNZNAME)) Then
                custInfoCustOrgnzNameLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                custInfoCustOrgnzNameLabel.Text = HttpUtility.HtmlEncode(newCustomerDataRow.CUSTORGNZNAME)
            End If
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
            '担当スタッフ名
            If String.IsNullOrEmpty(Trim(newCustomerDataRow.USERNAME)) Then
                CustomerCarSCNameLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                CustomerCarSCNameLabel.Text = HttpUtility.HtmlEncode(newCustomerDataRow.USERNAME)
            End If
            '担当SA名
            If String.IsNullOrEmpty(Trim(newCustomerDataRow.SAUSERNAME)) Then
                CustomerCarSANameLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                CustomerCarSANameLabel.Text = HttpUtility.HtmlEncode(newCustomerDataRow.SAUSERNAME)
            End If

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            If Not newCustomerDataRow.IsBIRTHDAYNull Then
                customerBirthday.Value = newCustomerDataRow.BIRTHDAY
            End If
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
            'VIPアイコン表示
            If Not newCustomerDataRow.IsVIP_FLGNull _
                AndAlso String.Equals(CST_VIP_FLG_ON, newCustomerDataRow.VIP_FLG) Then
                VIP_Icon.Visible = True
            Else
                VIP_Icon.Visible = False
            End If
            ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END

            '2012/02/15 TCS 山口 【SALES_2】 START
            '顧客種別
            CustomerKind.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10174))
         
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 START
            '顧客タイプ
            If String.Equals(newCustomerDataRow.CST_JOIN_TYPE, CST_JOIN_TYPE_I) Then
                'Iマーク
                CustomerTypeIcon.Visible = True
                CustomerType.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(1010001))
            ElseIf String.Equals(newCustomerDataRow.CST_JOIN_TYPE, CST_JOIN_TYPE_C) Then
                'Cマーク
                CustomerTypeIcon.Visible = True
                CustomerType.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(1010002))
            Else
                'その他
                CustomerTypeIcon.Visible = False
            End If
            ' 2018/08/27 TCS 佐々木 TKM Next Gen e-CRB Project Application development Block B-3 END
            '2012/02/15 TCS 山口 【SALES_2】 END

            '顧客編集ポップアップ用値セット
            editModeHidden.Value = CStr(SC3080205BusinessLogic.ModeEdit)

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            '顧客名
            Me.SetValue(ScreenPos.Current, SESSION_KEY_MEMO_CRCUSTNAME, newCustomerDataRow.NAME)
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END

            '2012/06/01 TCS 河原 FS開発 START
            If Not newCustomerDataRow.IsSNSID_RENRENNull Then
                Snsid_Renren_Hidden.Value = newCustomerDataRow.SNSID_RENREN
            End If

            If Not newCustomerDataRow.IsSNSID_KAIXINNull Then
                Snsid_Kaixin_Hidden.Value = newCustomerDataRow.SNSID_KAIXIN
            End If

            If Not newCustomerDataRow.IsSNSID_WEIBONull Then
                Snsid_Weibo_Hidden.Value = newCustomerDataRow.SNSID_WEIBO
            End If

            '2013/06/30 TCS 未 2013/10対応版　既存流用 START
            If Not newCustomerDataRow.IsKEYWORDNull Then
                Keyword_Hidden.Value = Trim(newCustomerDataRow.KEYWORD)
            End If
            '2013/06/30 TCS 未 2013/10対応版　既存流用 END
            '2012/06/01 TCS 河原 FS開発 END

            '2013/06/30 TCS 未 2013/10対応版　既存流用 START
            If Not newCustomerDataRow.IsCUSTOMERLOCKVERSIONNull Then
                Me.CustomerLockVersion.Value = CType(newCustomerDataRow.CUSTOMERLOCKVERSION, String)
            End If

            If Not newCustomerDataRow.IsCUSTOMERDLRLOCKVERSIONNull Then
                Me.CustomerDLRLockVersion.Value = CType(newCustomerDataRow.CUSTOMERDLRLOCKVERSION, String)
            End If
            '2013/06/30 TCS 未 2013/10対応版　既存流用 END
        End If
        Logger.Info("_SetControlNewCustomer End")
    End Sub
#End Region

#Region "敬称付名前作成"
    ''' <summary>
    ''' 敬称付名前作成
    ''' </summary>
    ''' <param name="name">名前</param>
    ''' <param name="pos">位置</param>
    ''' <param name="title">敬称</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function _MakeCustomerTitle(ByVal name As String, ByVal pos As String, ByVal title As String) As String

        Dim sb As New StringBuilder
        If pos.Equals("1") Then
            If Not String.IsNullOrEmpty(title) Then
                sb.Append(title)
                sb.Append(" ")
            End If
        End If

        sb.Append(name)

        If pos.Equals("2") Then
            If Not String.IsNullOrEmpty(title) Then
                sb.Append(" ")
                sb.Append(title)
            End If
        End If

        '$01 Add Start
        '顧客名をセッションに保存
        SetValue(ScreenPos.Current, SESSION_KEY_CUSTNAME, name)

        '顧客名 + 敬称をセッションに保存
        SetValue(ScreenPos.Current, SESSION_KEY_NAME, sb.ToString)
        '$01 Add End

        Return sb.ToString

    End Function

#End Region

#Region "画面設定 自社客車両"
    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
    ''' <summary>
    ''' 自社客車両データを並び替え
    ''' </summary>
    ''' <param name="orgVehicleDataTbl"></param>
    ''' <param name="params"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function _editOrgVehicleDataTbl(ByVal orgVehicleDataTbl As SC3080201DataSet.SC3080201OrgVehicleDataTable, _
                                            ByVal params As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201OrgVehicleDataTable

        'キー取得
        Dim paramsRow As SC3080201DataSet.SC3080201ParameterRow
        paramsRow = CType(params.Rows(0), SC3080201DataSet.SC3080201ParameterRow)
        Dim vclID As String = paramsRow.VCLID

        Dim editOrgVehicleDataTbl As New SC3080201DataSet.SC3080201OrgVehicleDataTable
        If String.IsNullOrEmpty(vclID) Then
            'キーが存在しない場合そのまま
            editOrgVehicleDataTbl = orgVehicleDataTbl
            '新規登録された場合、セッションにキーを登録
            If editOrgVehicleDataTbl.Rows.Count = 1 Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_VCLID, editOrgVehicleDataTbl.Rows(0).Item(editOrgVehicleDataTbl.KEYColumn.ColumnName))
                Me.customerCarsSelectedHiddenField.Value = CStr(editOrgVehicleDataTbl.Rows(0).Item(editOrgVehicleDataTbl.KEYColumn.ColumnName))
            End If
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
            Me.vclupdateHidden.Value = CStr(0)
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        Else
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
            Me.vclupdateHidden.Value = vclID
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
            'キーが存在する場合、1行目に
            Dim copyDataRow As SC3080201DataSet.SC3080201OrgVehicleRow = CType(editOrgVehicleDataTbl.NewRow, SC3080201DataSet.SC3080201OrgVehicleRow)
            Dim keyFlg As Boolean = False
            For Each orgVehicleRow In orgVehicleDataTbl
                If String.Equals(orgVehicleRow.KEY, vclID) Then
                    'コピー
                    copyDataRow.ItemArray = orgVehicleRow.ItemArray
                    keyFlg = True
                Else
                    '別tableに追加
                    editOrgVehicleDataTbl.Rows.Add(orgVehicleRow.ItemArray)
                End If
            Next
            '別table1行目に追加
            If keyFlg Then
                editOrgVehicleDataTbl.Rows.InsertAt(copyDataRow, 0)
            End If
        End If

        '初期読み込みの場合、選択行を強制的に1行目に
        If String.IsNullOrEmpty(Me.customerCarsSelectedHiddenField.Value) Then
            If editOrgVehicleDataTbl.Rows.Count > 0 Then
                Me.customerCarsSelectedHiddenField.Value = CStr(editOrgVehicleDataTbl.Rows(0).Item(editOrgVehicleDataTbl.KEYColumn.ColumnName))
            End If
        End If

        Return editOrgVehicleDataTbl
    End Function

    ''' <summary>
    ''' 画面設定 自社客車両エリア
    ''' </summary>
    ''' <param name="orgVehicleDataTbl"></param>
    ''' <remarks></remarks>
    Private Sub _setOrgCustomerCarTypeArea(ByVal orgVehicleDataTbl As SC3080201DataSet.SC3080201OrgVehicleDataTable)
        If orgVehicleDataTbl.Rows.Count < 1 Then
            '表示制御 登録前
            EditCustomerCarTypePanel.Visible = False
            NewCustomerCarTypePanel.Visible = True

            '車両編集ポップアップ用の値セット
            selectVinHidden.Value = String.Empty
            selectSeqnoHidden.Value = String.Empty
            editVehicleModeHidden.Value = CStr(SC3080205BusinessLogic.ModeCreate)

            '活動結果登録用の値をセット
            Me.SetValue(ScreenPos.Current, CONST_VCLINFO, String.Empty)
        Else
            '表示制御 登録後
            EditCustomerCarTypePanel.Visible = True
            NewCustomerCarTypePanel.Visible = False

            '列を追加
            orgVehicleDataTbl.Columns.Add("VCLDELIDATESTRING")
            orgVehicleDataTbl.Columns.Add("UPDATEDATESTRING")

            For Each drOrgVehicle In orgVehicleDataTbl
                If String.Equals(customerCarsSelectedHiddenField.Value, drOrgVehicle.KEY) Then
                    '選択行の処理
                    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
                    Me.vclupdateHidden.Value = customerCarsSelectedHiddenField.Value
                    '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
                    'データ編集
                    _EditControlOrgVehicle(drOrgVehicle)
                    'メイン画面の設定
                    _SetControlOrgVehicleMain(drOrgVehicle)
                End If
            Next
        End If

        '有車両台数
        If orgVehicleDataTbl.Rows.Count <= 1 Then
            '1台以下は表示しない
            CustomerCarTypeNumber.Visible = False
        Else
            CustomerCarTypeNumber.Visible = True
            CustomerCarTypeNumberLabel.Text = HttpUtility.HtmlEncode(CStr(orgVehicleDataTbl.Rows.Count))
        End If

    End Sub

    ''' <summary>
    ''' 画面設定 自社客車両
    ''' </summary>
    ''' <param name="orgVehicleDataTbl"></param>
    ''' <remarks></remarks>
    Private Sub _SetControlOrgVehicle(ByVal orgVehicleDataTbl As SC3080201DataSet.SC3080201OrgVehicleDataTable)

        '列を追加
        orgVehicleDataTbl.Columns.Add("VCLDELIDATESTRING")
        orgVehicleDataTbl.Columns.Add("UPDATEDATESTRING")

        '取得件数分ループしバインドの準備をする
        Dim count As Integer = 1
        For Each drOrgVehicle In orgVehicleDataTbl
            'データ編集
            _EditControlOrgVehicle(drOrgVehicle)

            If count = 1 And Not String.Equals(customerCarsSelectedHiddenField.Value, drOrgVehicle.KEY) Then
                '非選択行(1行目)の処理
                drOrgVehicle.CARTYPESELECTION = "scNscSelectionCassette1"
                drOrgVehicle.CARTYPESELECTIONITEM = "scNscCustomerCarTypeData1"
                drOrgVehicle.CARTYPESELECTIONSTYLET = "CarTypeBlack"
                drOrgVehicle.CARTYPESELECTIONSTYLETD = "CarTypeBoldTextBlack"
                drOrgVehicle.CARTYPESELECTIONSTYLED1 = "scNscSelectionList1Black"
                drOrgVehicle.CARTYPESELECTIONSTYLED2 = "scNscSelectionList2Black"
            ElseIf count <> 1 And Not String.Equals(customerCarsSelectedHiddenField.Value, drOrgVehicle.KEY) Then
                '非選択行(1行目以外)の処理
                drOrgVehicle.CARTYPESELECTION = "scNscSelectionCassette3"
                drOrgVehicle.CARTYPESELECTIONITEM = "scNscCustomerCarTypeData1"
                drOrgVehicle.CARTYPESELECTIONSTYLET = "CarTypeBlack"
                drOrgVehicle.CARTYPESELECTIONSTYLETD = "CarTypeBoldTextBlack"
                drOrgVehicle.CARTYPESELECTIONSTYLED1 = "scNscSelectionList1Black"
                drOrgVehicle.CARTYPESELECTIONSTYLED2 = "scNscSelectionList2Black"
            End If
            If String.Equals(customerCarsSelectedHiddenField.Value, drOrgVehicle.KEY) Then
                '選択行の処理
                drOrgVehicle.CARTYPESELECTION = "scNscSelectionCassette2"
                drOrgVehicle.CARTYPESELECTIONITEM = "scNscCustomerCarTypeData2"
                drOrgVehicle.CARTYPESELECTIONSTYLET = "CarTypeWhite"
                drOrgVehicle.CARTYPESELECTIONSTYLETD = "CarTypeBoldTextWhite"
                drOrgVehicle.CARTYPESELECTIONSTYLED1 = "scNscSelectionList1White"
                drOrgVehicle.CARTYPESELECTIONSTYLED2 = "scNscSelectionList2White"
                'メイン画面の設定
                _SetControlOrgVehicleMain(drOrgVehicle)
            End If

            drOrgVehicle.INDEX = CShort(count)
            count = count + 1
        Next

        'ポップアップの設定
        CarTypeRepeater.DataSource = orgVehicleDataTbl
        CarTypeRepeater.DataBind()

    End Sub

    ' ''' <summary>
    ' ''' 画面設定 自社客車両
    ' ''' </summary>
    ' ''' <param name="orgVehicleDataTbl"></param>
    ' ''' <remarks></remarks>
    'Private Sub _SetControlOrgVehicle(ByVal orgVehicleDataTbl As SC3080201DataSet.SC3080201OrgVehicleDataTable)

    '    If orgVehicleDataTbl.Rows.Count < 1 Then
    '        '表示制御 登録前
    '        EditCustomerCarTypePanel.Visible = False
    '        NewCustomerCarTypePanel.Visible = True

    '        '車両編集ポップアップ用の値セット
    '        selectVinHidden.Value = String.Empty
    '        selectSeqnoHidden.Value = String.Empty
    '        editVehicleModeHidden.Value = CStr(SC3080205BusinessLogic.ModeCreate)

    '        '活動結果登録用の値をセット
    '        Me.SetValue(ScreenPos.Current, CONST_VCLINFO, String.Empty)
    '    Else
    '        '表示制御 登録後
    '        EditCustomerCarTypePanel.Visible = True
    '        NewCustomerCarTypePanel.Visible = False

    '        orgVehicleDataTbl.Columns.Add("VCLDELIDATESTRING")
    '        orgVehicleDataTbl.Columns.Add("UPDATEDATESTRING")

    '        '取得件数分ループしバインドの準備をする
    '        Dim count As Integer = 1
    '        For Each drOrgVehicle In orgVehicleDataTbl
    '            'データ編集
    '            _EditControlOrgVehicle(drOrgVehicle)

    '            If count = 1 And Not String.Equals(customerCarsSelectedHiddenField.Value, drOrgVehicle.KEY) Then
    '                '非選択行(1行目)の処理
    '                drOrgVehicle.CARTYPESELECTION = "scNscSelectionCassette1"
    '                drOrgVehicle.CARTYPESELECTIONITEM = "scNscCustomerCarTypeData1"
    '                drOrgVehicle.CARTYPESELECTIONSTYLET = "CarTypeBlack"
    '                drOrgVehicle.CARTYPESELECTIONSTYLETD = "CarTypeBoldTextBlack"
    '                drOrgVehicle.CARTYPESELECTIONSTYLED1 = "scNscSelectionList1Black"
    '                drOrgVehicle.CARTYPESELECTIONSTYLED2 = "scNscSelectionList2Black"
    '            ElseIf count <> 1 And Not String.Equals(customerCarsSelectedHiddenField.Value, drOrgVehicle.KEY) Then
    '                '非選択行(1行目以外)の処理
    '                drOrgVehicle.CARTYPESELECTION = "scNscSelectionCassette3"
    '                drOrgVehicle.CARTYPESELECTIONITEM = "scNscCustomerCarTypeData1"
    '                drOrgVehicle.CARTYPESELECTIONSTYLET = "CarTypeBlack"
    '                drOrgVehicle.CARTYPESELECTIONSTYLETD = "CarTypeBoldTextBlack"
    '                drOrgVehicle.CARTYPESELECTIONSTYLED1 = "scNscSelectionList1Black"
    '                drOrgVehicle.CARTYPESELECTIONSTYLED2 = "scNscSelectionList2Black"
    '            End If
    '            If String.Equals(customerCarsSelectedHiddenField.Value, drOrgVehicle.KEY) Then
    '                '選択行の処理
    '                drOrgVehicle.CARTYPESELECTION = "scNscSelectionCassette2"
    '                drOrgVehicle.CARTYPESELECTIONITEM = "scNscCustomerCarTypeData2"
    '                drOrgVehicle.CARTYPESELECTIONSTYLET = "CarTypeWhite"
    '                drOrgVehicle.CARTYPESELECTIONSTYLETD = "CarTypeBoldTextWhite"
    '                drOrgVehicle.CARTYPESELECTIONSTYLED1 = "scNscSelectionList1White"
    '                drOrgVehicle.CARTYPESELECTIONSTYLED2 = "scNscSelectionList2White"
    '                'メイン画面の設定
    '                _SetControlOrgVehicleMain(drOrgVehicle)
    '            End If

    '            '全データ統一処理
    '            'If String.Equals(Convert.ToString(drOrgVehicle.LOGO_NOTSELECTED), String.Empty) Or _
    '            'String.Equals(Convert.ToString(drOrgVehicle.LOGO_SELECTED), String.Empty) Then
    '            'If String.IsNullOrEmpty(drOrgVehicle.LOGO_NOTSELECTED) Or _
    '            '   String.IsNullOrEmpty(drOrgVehicle.LOGO_SELECTED) Then
    '            '    'ラベル表示
    '            '    drOrgVehicle.SHOWLABEL = "True"
    '            '    drOrgVehicle.SHOWLOGO = "False"
    '            'Else
    '            '    'モデルロゴ表示
    '            '    drOrgVehicle.SHOWLABEL = "False"
    '            '    drOrgVehicle.SHOWLOGO = "True"

    '            'End If

    '            ''走行距離
    '            'If String.IsNullOrEmpty(drOrgVehicle.MILEAGE) Then
    '            '    drOrgVehicle.MILEAGE = NOTEXT & WebWordUtility.GetWord(10130)
    '            'Else
    '            '    drOrgVehicle.MILEAGE = drOrgVehicle.MILEAGE & WebWordUtility.GetWord(10130)
    '            'End If

    '            'Dim sc As StaffContext = StaffContext.Current

    '            ''納車日
    '            'If drOrgVehicle.IsVCLDELIDATENull Then
    '            '    drOrgVehicle.Item("VCLDELIDATESTRING") = NOTEXT
    '            'Else
    '            '    drOrgVehicle.Item("VCLDELIDATESTRING") = DateTimeFunc.FormatDate(3, drOrgVehicle.VCLDELIDATE)
    '            'End If

    '            ''更新日
    '            'If drOrgVehicle.IsUPDATEDATENull Then
    '            '    drOrgVehicle.Item("UPDATEDATESTRING") = NOTEXT
    '            'Else
    '            '    drOrgVehicle.Item("UPDATEDATESTRING") = WebWordUtility.GetWord(10120) & " " & DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, drOrgVehicle.UPDATEDATE, sc.DlrCD)
    '            'End If

    '            drOrgVehicle.INDEX = CShort(count)

    '            count = count + 1
    '        Next

    '        '有車両台数
    '        If orgVehicleDataTbl.Rows.Count <= 1 Then
    '            '1台以下は表示しない
    '            CustomerCarTypeNumber.Visible = False
    '        Else
    '            CustomerCarTypeNumber.Visible = True
    '            CustomerCarTypeNumberLabel.Text = HttpUtility.HtmlEncode(CStr(orgVehicleDataTbl.Rows.Count))

    '        End If

    '        'ポップアップの設定
    '        CarTypeRepeater.DataSource = orgVehicleDataTbl
    '        CarTypeRepeater.DataBind()
    '    End If

    'End Sub

    '2012/03/08 TCS 山口 【SALES_2】性能改善 END


    ''' <summary>
    ''' 自社客車両 取得したデータを編集
    ''' </summary>
    ''' <param name="drOrgVehicle"></param>
    ''' <remarks></remarks>
    Private Sub _EditControlOrgVehicle(ByVal drOrgVehicle As SC3080201DataSet.SC3080201OrgVehicleRow)
        If String.IsNullOrEmpty(drOrgVehicle.LOGO_NOTSELECTED) Or _
           String.IsNullOrEmpty(drOrgVehicle.LOGO_SELECTED) Then
            'ラベル表示
            drOrgVehicle.SHOWLABEL = "True"
            drOrgVehicle.SHOWLOGO = "False"
        Else
            'モデルロゴ表示
            drOrgVehicle.SHOWLABEL = "False"
            drOrgVehicle.SHOWLOGO = "True"

        End If

        'メーカー名
        If String.IsNullOrEmpty(Trim(drOrgVehicle.SERIESCD)) Then
            drOrgVehicle.SERIESCD = NOTEXT
        Else
            drOrgVehicle.SERIESCD = HttpUtility.HtmlEncode(drOrgVehicle.SERIESCD)
        End If
        'モデル名
        If String.IsNullOrEmpty(Trim(drOrgVehicle.SERIESNM)) Then
            drOrgVehicle.SERIESNM = NOTEXT
        Else
            drOrgVehicle.SERIESNM = HttpUtility.HtmlEncode(drOrgVehicle.SERIESNM)
        End If
        'グレード
        If String.IsNullOrEmpty(Trim(drOrgVehicle.GRADE)) Then
            drOrgVehicle.GRADE = NOTEXT
        Else
            drOrgVehicle.GRADE = HttpUtility.HtmlEncode(drOrgVehicle.GRADE)
        End If
        '外鈑色名称
        If String.IsNullOrEmpty(Trim(drOrgVehicle.BDYCLRNM)) Then
            drOrgVehicle.BDYCLRNM = NOTEXT
        Else
            drOrgVehicle.BDYCLRNM = HttpUtility.HtmlEncode(drOrgVehicle.BDYCLRNM)
        End If
        '車両登録No
        If String.IsNullOrEmpty(Trim(drOrgVehicle.VCLREGNO)) Then
            drOrgVehicle.VCLREGNO = NOTEXT
        Else
            drOrgVehicle.VCLREGNO = HttpUtility.HtmlEncode(drOrgVehicle.VCLREGNO)
        End If
        'VIN
        If String.IsNullOrEmpty(Trim(drOrgVehicle.VIN)) Then
            drOrgVehicle.VIN = NOTEXT
        Else
            drOrgVehicle.VIN = HttpUtility.HtmlEncode(drOrgVehicle.VIN)
        End If
        ''納車日
        'If drOrgVehicle.IsVCLDELIDATENull Then
        '    CustomerCarsVCLDateLabel.Text = NOTEXT
        'Else
        '    CustomerCarsVCLDateLabel.Text = HttpUtility.HtmlEncode(DateTimeFunc.FormatDate(3, drOrgVehicle.VCLDELIDATE))
        'End If
        '納車日
        If drOrgVehicle.IsVCLDELIDATENull Then
            drOrgVehicle.Item("VCLDELIDATESTRING") = NOTEXT
        Else
            drOrgVehicle.Item("VCLDELIDATESTRING") = HttpUtility.HtmlEncode(DateTimeFunc.FormatDate(3, drOrgVehicle.VCLDELIDATE))
        End If

        '走行距離
        If String.IsNullOrEmpty(Trim(drOrgVehicle.MILEAGE)) Then
            drOrgVehicle.MILEAGE = HttpUtility.HtmlEncode(NOTEXT & " " & WebWordUtility.GetWord(10130))
        Else
            drOrgVehicle.MILEAGE = HttpUtility.HtmlEncode(drOrgVehicle.MILEAGE & " " & WebWordUtility.GetWord(10130))
        End If

        '更新日
        Dim sc As StaffContext = StaffContext.Current
        If drOrgVehicle.IsUPDATEDATENull Then
            drOrgVehicle.Item("UPDATEDATESTRING") = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10120) & " " & NOTEXT)
        Else
            drOrgVehicle.Item("UPDATEDATESTRING") = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10120) & " " & DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, drOrgVehicle.UPDATEDATE, sc.DlrCD))
        End If
        '担当SA名
        If String.IsNullOrEmpty(Trim(drOrgVehicle.USERNAME)) Then
            drOrgVehicle.USERNAME = NOTEXT
        Else
            drOrgVehicle.USERNAME = HttpUtility.HtmlEncode(drOrgVehicle.USERNAME)
        End If
        ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        '年式
        If String.IsNullOrEmpty(Trim(drOrgVehicle.MODEL_YEAR)) Then
            drOrgVehicle.MODEL_YEAR = NOTEXT
        Else
            drOrgVehicle.MODEL_YEAR = HttpUtility.HtmlEncode(drOrgVehicle.MODEL_YEAR)
        End If

        '走行距離
        If String.IsNullOrEmpty(Trim(drOrgVehicle.VCL_MILE)) Then
            drOrgVehicle.VCL_MILE = HttpUtility.HtmlEncode(NOTEXT & " " & WebWordUtility.GetWord(10130))
        Else
            drOrgVehicle.VCL_MILE = HttpUtility.HtmlEncode(drOrgVehicle.VCL_MILE & " " & WebWordUtility.GetWord(10130))
        End If
        ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
    End Sub
#End Region

#Region "画面設定 未取引客車両"
    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
    ''' <summary>
    ''' 未取引客車両データを並び替え
    ''' </summary>
    ''' <param name="newVehicleDataTbl"></param>
    ''' <param name="params"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function _editNewVehicleDataTbl(ByVal newVehicleDataTbl As SC3080201DataSet.SC3080201NewVehicleDataTable, _
                                            ByVal params As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201NewVehicleDataTable

        'キー取得
        Dim paramsRow As SC3080201DataSet.SC3080201ParameterRow
        paramsRow = CType(params.Rows(0), SC3080201DataSet.SC3080201ParameterRow)
        Dim vclID As String = paramsRow.VCLID

        Dim editNewVehicleDataTbl As New SC3080201DataSet.SC3080201NewVehicleDataTable
        If String.IsNullOrEmpty(vclID) Then
            'キーが存在しない場合そのまま
            editNewVehicleDataTbl = newVehicleDataTbl
            '新規登録された場合、セッションにキーを登録
            If editNewVehicleDataTbl.Rows.Count = 1 Then
                Me.SetValue(ScreenPos.Current, SESSION_KEY_VCLID, editNewVehicleDataTbl.Rows(0).Item(editNewVehicleDataTbl.KEYColumn.ColumnName))
                Me.customerCarsSelectedHiddenField.Value = CStr(editNewVehicleDataTbl.Rows(0).Item(editNewVehicleDataTbl.KEYColumn.ColumnName))
            End If
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
            Me.vclupdateHidden.Value = CStr(0)
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
        Else
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
            Me.vclupdateHidden.Value = vclID
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
            'キーが存在する場合、1行目に
            Dim copyDataRow As SC3080201DataSet.SC3080201NewVehicleRow = CType(editNewVehicleDataTbl.NewRow, SC3080201DataSet.SC3080201NewVehicleRow)
            Dim keyFlg As Boolean = False
            For Each newVehicleRow In newVehicleDataTbl
                If String.Equals(newVehicleRow.KEY, vclID) Then
                    'コピー
                    copyDataRow.ItemArray = newVehicleRow.ItemArray
                    keyFlg = True
                Else
                    '別tableに追加
                    editNewVehicleDataTbl.Rows.Add(newVehicleRow.ItemArray)
                End If
            Next
            '別table1行目に追加
            If keyFlg Then
                editNewVehicleDataTbl.Rows.InsertAt(copyDataRow, 0)
            End If
        End If

        '初期読み込みの場合、選択行を強制的に1行目に
        If String.IsNullOrEmpty(Me.customerCarsSelectedHiddenField.Value) Then
            If editNewVehicleDataTbl.Rows.Count > 0 Then
                Me.customerCarsSelectedHiddenField.Value = CStr(editNewVehicleDataTbl.Rows(0).Item(editNewVehicleDataTbl.KEYColumn.ColumnName))
            End If
        End If

        Return editNewVehicleDataTbl
    End Function

    ''' <summary>
    ''' 画面設定 未取引客車両エリア
    ''' </summary>
    ''' <param name="newVehicleDataTbl"></param>
    ''' <remarks></remarks>
    Private Sub _setNewCustomerCarTypeArea(ByVal newVehicleDataTbl As SC3080201DataSet.SC3080201NewVehicleDataTable)
        If newVehicleDataTbl.Rows.Count < 1 Then
            '表示制御 登録前
            EditCustomerCarTypePanel.Visible = False
            NewCustomerCarTypePanel.Visible = True

            '車両編集ポップアップ用の値セット
            selectVinHidden.Value = String.Empty
            selectSeqnoHidden.Value = String.Empty
            editVehicleModeHidden.Value = CStr(SC3080205BusinessLogic.ModeCreate)

            '活動結果登録用の値をセット
            Me.SetValue(ScreenPos.Current, CONST_VCLINFO, String.Empty)
        Else
            '表示制御 登録後
            EditCustomerCarTypePanel.Visible = True
            NewCustomerCarTypePanel.Visible = False

            '列を追加
            newVehicleDataTbl.Columns.Add("VCLDELIDATESTRING")
            newVehicleDataTbl.Columns.Add("UPDATEDATESTRING")

            For Each drNewVehicle In newVehicleDataTbl
                If String.Equals(customerCarsSelectedHiddenField.Value, drNewVehicle.KEY) Then
                    '選択行の処理
                    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
                    Me.vclupdateHidden.Value = customerCarsSelectedHiddenField.Value
                    '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
                    'データ編集
                    _EditControlNewVehicleMain(drNewVehicle)
                    'メイン画面の設定
                    _SetControlNewVehicleMain(drNewVehicle)
                End If
            Next
        End If

        '有車両台数
        If newVehicleDataTbl.Rows.Count <= 1 Then
            '1台以下は表示しない
            CustomerCarTypeNumber.Visible = False
        Else
            CustomerCarTypeNumber.Visible = True
            CustomerCarTypeNumberLabel.Text = HttpUtility.HtmlEncode(CStr(newVehicleDataTbl.Rows.Count))
        End If

    End Sub

    ''' <summary>
    ''' 画面設定 未取引客車両
    ''' </summary>
    ''' <param name="newVehicleDataTbl"></param>
    ''' <remarks></remarks>
    Private Sub _SetControlNewVehicle(ByVal newVehicleDataTbl As SC3080201DataSet.SC3080201NewVehicleDataTable)

        '列を追加
        newVehicleDataTbl.Columns.Add("VCLDELIDATESTRING")
        newVehicleDataTbl.Columns.Add("UPDATEDATESTRING")

        '取得件数分ループしバインドの準備をする
        Dim count As Integer = 1
        For Each drNewVehicle In newVehicleDataTbl
            'データ編集
            _EditControlNewVehicleMain(drNewVehicle)

            If count = 1 And Not String.Equals(customerCarsSelectedHiddenField.Value, drNewVehicle.KEY) Then
                '非選択行(1行目)の処理
                drNewVehicle.CARTYPESELECTION = "scNscSelectionCassette1"
                drNewVehicle.CARTYPESELECTIONITEM = "scNscCustomerCarTypeData1"
                drNewVehicle.CARTYPESELECTIONSTYLET = "CarTypeBlack"
                drNewVehicle.CARTYPESELECTIONSTYLETD = "CarTypeBoldTextBlack"
                drNewVehicle.CARTYPESELECTIONSTYLED1 = "scNscSelectionList1Black"
                drNewVehicle.CARTYPESELECTIONSTYLED2 = "scNscSelectionList2Black"
            ElseIf count <> 1 And Not String.Equals(customerCarsSelectedHiddenField.Value, drNewVehicle.KEY) Then
                '非選択行(1行目以外)の処理
                drNewVehicle.CARTYPESELECTION = "scNscSelectionCassette3"
                drNewVehicle.CARTYPESELECTIONITEM = "scNscCustomerCarTypeData1"
                drNewVehicle.CARTYPESELECTIONSTYLET = "CarTypeBlack"
                drNewVehicle.CARTYPESELECTIONSTYLETD = "CarTypeBoldTextBlack"
                drNewVehicle.CARTYPESELECTIONSTYLED1 = "scNscSelectionList1Black"
                drNewVehicle.CARTYPESELECTIONSTYLED2 = "scNscSelectionList2Black"
            End If
            If String.Equals(customerCarsSelectedHiddenField.Value, drNewVehicle.KEY) Then
                '選択行の処理
                drNewVehicle.CARTYPESELECTION = "scNscSelectionCassette2"
                drNewVehicle.CARTYPESELECTIONITEM = "scNscCustomerCarTypeData2"
                drNewVehicle.CARTYPESELECTIONSTYLET = "CarTypeWhite"
                drNewVehicle.CARTYPESELECTIONSTYLETD = "CarTypeBoldTextWhite"
                drNewVehicle.CARTYPESELECTIONSTYLED1 = "scNscSelectionList1White"
                drNewVehicle.CARTYPESELECTIONSTYLED2 = "scNscSelectionList2White"
            End If

            drNewVehicle.INDEX = CShort(count)
            count = count + 1
        Next

        'ポップアップの設定
        CarTypeRepeater.DataSource = newVehicleDataTbl
        CarTypeRepeater.DataBind()

    End Sub

    ' ''' <summary>
    ' ''' 画面設定 未取引客車両
    ' ''' </summary>
    ' ''' <param name="newVehicleDataTbl"></param>
    ' ''' <remarks></remarks>
    'Private Sub _SetControlNewVehicle(ByVal newVehicleDataTbl As SC3080201DataSet.SC3080201NewVehicleDataTable)

    '    If newVehicleDataTbl.Rows.Count < 1 Then
    '        ''表示制御 登録前
    '        'EditCustomerCarTypePanel.Visible = False
    '        'NewCustomerCarTypePanel.Visible = True

    '        ''車両編集ポップアップ用の値セット
    '        'selectVinHidden.Value = String.Empty
    '        'selectSeqnoHidden.Value = String.Empty
    '        'editVehicleModeHidden.Value = CStr(SC3080205BusinessLogic.ModeCreate)

    '        ''活動結果登録用の値をセット
    '        'Me.SetValue(ScreenPos.Current, CONST_VCLINFO, String.Empty)
    '    Else
    '        '表示制御 登録後
    '        EditCustomerCarTypePanel.Visible = True
    '        NewCustomerCarTypePanel.Visible = False

    '        newVehicleDataTbl.Columns.Add("VCLDELIDATESTRING")
    '        newVehicleDataTbl.Columns.Add("UPDATEDATESTRING")

    '        '取得件数分ループしバインドの準備をする
    '        Dim count As Integer = 1
    '        For Each drNewVehicle In newVehicleDataTbl
    '            'データ編集
    '            _EditControlNewVehicleMain(drNewVehicle)

    '            If count = 1 And Not String.Equals(customerCarsSelectedHiddenField.Value, drNewVehicle.KEY) Then
    '                '非選択行(1行目)の処理
    '                drNewVehicle.CARTYPESELECTION = "scNscSelectionCassette1"
    '                drNewVehicle.CARTYPESELECTIONITEM = "scNscCustomerCarTypeData1"
    '                drNewVehicle.CARTYPESELECTIONSTYLET = "CarTypeBlack"
    '                drNewVehicle.CARTYPESELECTIONSTYLETD = "CarTypeBoldTextBlack"
    '                drNewVehicle.CARTYPESELECTIONSTYLED1 = "scNscSelectionList1Black"
    '                drNewVehicle.CARTYPESELECTIONSTYLED2 = "scNscSelectionList2Black"
    '            ElseIf count <> 1 And Not String.Equals(customerCarsSelectedHiddenField.Value, drNewVehicle.KEY) Then
    '                '非選択行(1行目以外)の処理
    '                drNewVehicle.CARTYPESELECTION = "scNscSelectionCassette3"
    '                drNewVehicle.CARTYPESELECTIONITEM = "scNscCustomerCarTypeData1"
    '                drNewVehicle.CARTYPESELECTIONSTYLET = "CarTypeBlack"
    '                drNewVehicle.CARTYPESELECTIONSTYLETD = "CarTypeBoldTextBlack"
    '                drNewVehicle.CARTYPESELECTIONSTYLED1 = "scNscSelectionList1Black"
    '                drNewVehicle.CARTYPESELECTIONSTYLED2 = "scNscSelectionList2Black"
    '            End If
    '            If String.Equals(customerCarsSelectedHiddenField.Value, drNewVehicle.KEY) Then
    '                '選択行の処理
    '                drNewVehicle.CARTYPESELECTION = "scNscSelectionCassette2"
    '                drNewVehicle.CARTYPESELECTIONITEM = "scNscCustomerCarTypeData2"
    '                drNewVehicle.CARTYPESELECTIONSTYLET = "CarTypeWhite"
    '                drNewVehicle.CARTYPESELECTIONSTYLETD = "CarTypeBoldTextWhite"
    '                drNewVehicle.CARTYPESELECTIONSTYLED1 = "scNscSelectionList1White"
    '                drNewVehicle.CARTYPESELECTIONSTYLED2 = "scNscSelectionList2White"
    '                'メイン画面の設定
    '                _SetControlNewVehicleMain(drNewVehicle)
    '            End If

    '            ''全データ統一処理
    '            'If String.Equals(Convert.ToString(drNewVehicle.LOGO_NOTSELECTED), String.Empty) Or _
    '            '   String.Equals(Convert.ToString(drNewVehicle.LOGO_SELECTED), String.Empty) Then
    '            '    'ラベル表示
    '            '    drNewVehicle.SHOWLABEL = "True"
    '            '    drNewVehicle.SHOWLOGO = "False"
    '            'Else
    '            '    'モデルロゴ表示
    '            '    drNewVehicle.SHOWLABEL = "False"
    '            '    drNewVehicle.SHOWLOGO = "True"

    '            'End If

    '            ''納車日
    '            'If drNewVehicle.IsVCLDELIDATENull Then
    '            'Else
    '            '    drNewVehicle.Item("VCLDELIDATESTRING") = DateTimeFunc.FormatDate(3, drNewVehicle.VCLDELIDATE)
    '            'End If

    '            drNewVehicle.INDEX = CShort(count)
    '            count = count + 1
    '        Next

    '        '有車両台数
    '        If newVehicleDataTbl.Rows.Count <= 1 Then
    '            '1台以下は表示しない
    '            CustomerCarTypeNumber.Visible = False
    '        Else
    '            CustomerCarTypeNumber.Visible = True
    '            CustomerCarTypeNumberLabel.Text = HttpUtility.HtmlEncode(CStr(newVehicleDataTbl.Rows.Count))

    '        End If

    '        'ポップアップの設定
    '        CarTypeRepeater.DataSource = newVehicleDataTbl
    '        CarTypeRepeater.DataBind()
    '    End If

    'End Sub

    '2012/03/08 TCS 山口 【SALES_2】性能改善 END

    ''' <summary>
    ''' 未取引客車両 取得したデータを編集
    ''' </summary>
    ''' <param name="drNewVehicle"></param>
    ''' <remarks></remarks>
    Private Sub _EditControlNewVehicleMain(ByVal drNewVehicle As SC3080201DataSet.SC3080201NewVehicleRow)
        '全データ統一処理
        If String.IsNullOrEmpty(drNewVehicle.LOGO_NOTSELECTED) Or _
           String.IsNullOrEmpty(drNewVehicle.LOGO_SELECTED) Then
            'ラベル表示
            drNewVehicle.SHOWLABEL = "True"
            drNewVehicle.SHOWLOGO = "False"
        Else
            'モデルロゴ表示
            drNewVehicle.SHOWLABEL = "False"
            drNewVehicle.SHOWLOGO = "True"

        End If

        'メーカー名
        If String.IsNullOrEmpty(Trim(drNewVehicle.SERIESCD)) Then
            drNewVehicle.SERIESCD = NOTEXT
        Else
            drNewVehicle.SERIESCD = HttpUtility.HtmlEncode(drNewVehicle.SERIESCD)
        End If
        'モデル名
        If String.IsNullOrEmpty(Trim(drNewVehicle.SERIESNM)) Then
            drNewVehicle.SERIESNM = NOTEXT
        Else
            drNewVehicle.SERIESNM = HttpUtility.HtmlEncode(drNewVehicle.SERIESNM)
        End If
        '車両登録VCLREGNO
        If String.IsNullOrEmpty(Trim(drNewVehicle.VCLREGNO)) Then
            drNewVehicle.VCLREGNO = NOTEXT
        Else
            drNewVehicle.VCLREGNO = HttpUtility.HtmlEncode(drNewVehicle.VCLREGNO)
        End If
        'VIN
        If String.IsNullOrEmpty(Trim(drNewVehicle.VIN)) Then
            drNewVehicle.VIN = NOTEXT
        Else
            drNewVehicle.VIN = HttpUtility.HtmlEncode(drNewVehicle.VIN)
        End If
        ''納車日
        'If drNewVehicle.IsVCLDELIDATENull Then
        '    CustomerCarsVCLDateLabel.Text = String.Empty
        'Else
        '    CustomerCarsVCLDateLabel.Text = HttpUtility.HtmlEncode(DateTimeFunc.FormatDate(3, drNewVehicle.VCLDELIDATE))
        'End If
        '納車日
        If drNewVehicle.IsVCLDELIDATENull Then
            drNewVehicle.Item("VCLDELIDATESTRING") = NOTEXT
        Else
            drNewVehicle.Item("VCLDELIDATESTRING") = DateTimeFunc.FormatDate(3, drNewVehicle.VCLDELIDATE)
        End If

        'グレード
        drNewVehicle.GRADE = NOTEXT
        '外鈑色名称
        drNewVehicle.BDYCLRNM = NOTEXT
        '走行距離
        drNewVehicle.MILEAGE = HttpUtility.HtmlEncode(NOTEXT & " " & WebWordUtility.GetWord(10130))
        '更新日
        drNewVehicle.Item("UPDATEDATESTRING") = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10120) & " " & NOTEXT)
        ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        '年式
        If String.IsNullOrEmpty(Trim(drNewVehicle.MODEL_YEAR)) Then
            drNewVehicle.MODEL_YEAR = NOTEXT
        Else
            drNewVehicle.MODEL_YEAR = HttpUtility.HtmlEncode(drNewVehicle.MODEL_YEAR)
        End If

        '走行距離
        If String.IsNullOrEmpty(Trim(drNewVehicle.VCL_MILE)) Then
            drNewVehicle.VCL_MILE = HttpUtility.HtmlEncode(NOTEXT & " " & WebWordUtility.GetWord(10130))
        Else
            drNewVehicle.VCL_MILE = HttpUtility.HtmlEncode(drNewVehicle.VCL_MILE & " " & WebWordUtility.GetWord(10130))
        End If
        ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
    End Sub

#End Region

#Region "画面設定 車両選択ポップアップ"
    Protected Sub CarTypeRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles CarTypeRepeater.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim carTypeDivMain As HtmlGenericControl = CType(e.Item.FindControl("carTypeDivMain"), HtmlGenericControl)
            Dim carTypeLogoLbl As HtmlGenericControl = CType(e.Item.FindControl("carTypeLogoLbl"), HtmlGenericControl)
            Dim customerCarSeriesTable As HtmlTable = CType(e.Item.FindControl("customerCarSeriesTable"), HtmlTable)
            Dim customerCarSeriesCdLabel As CustomLabel = CType(e.Item.FindControl("customerCarSeriesCdLabel"), CustomLabel)
            Dim customerCarSeriesNmTd As HtmlTableCell = CType(e.Item.FindControl("customerCarSeriesNmTd"), HtmlTableCell)
            Dim customerCarSeriesNmLabel As CustomLabel = CType(e.Item.FindControl("customerCarSeriesNmLabel"), CustomLabel)
            Dim carTypeLogoImg As HtmlGenericControl = CType(e.Item.FindControl("carTypeLogoImg"), HtmlGenericControl)
            Dim carTypeLogoP As Image = CType(e.Item.FindControl("carTypeLogoP"), Image)
            Dim customerCarGradeDiv As HtmlGenericControl = CType(e.Item.FindControl("customerCarGradeDiv"), HtmlGenericControl)
            Dim customerCarGradeLabelP As CustomLabel = CType(e.Item.FindControl("customerCarGradeLabelP"), CustomLabel)
            Dim customerCarsBdyclrnmDiv As HtmlGenericControl = CType(e.Item.FindControl("customerCarsBdyclrnmDiv"), HtmlGenericControl)
            Dim customerCarsBdyclrnmLabelP As CustomLabel = CType(e.Item.FindControl("customerCarsBdyclrnmLabelP"), CustomLabel)
            Dim customerCarsRightTable As HtmlTable = CType(e.Item.FindControl("customerCarsRightTable"), HtmlTable)
            Dim customerCarsRegLabelP As CustomLabel = CType(e.Item.FindControl("customerCarsRegLabelP"), CustomLabel)
            Dim customerCarsVINLabelP As CustomLabel = CType(e.Item.FindControl("customerCarsVINLabelP"), CustomLabel)
            Dim customerCarsVCLDateLabelP As CustomLabel = CType(e.Item.FindControl("customerCarsVCLDateLabelP"), CustomLabel)
            Dim customerCarsKmLabelP As CustomLabel = CType(e.Item.FindControl("customerCarsKmLabelP"), CustomLabel)
            Dim customerCarsDateLabelP As CustomLabel = CType(e.Item.FindControl("customerCarsKmLabelP"), CustomLabel)
            Dim logoNotSelectid As HiddenField = CType(e.Item.FindControl("logoNotSelectid"), HiddenField)
            Dim logoSelectid As HiddenField = CType(e.Item.FindControl("logoSelectid"), HiddenField)
            Dim customerCarKey As HiddenField = CType(e.Item.FindControl("customerCarKey"), HiddenField)

            Dim data As DataRowView = CType(e.Item.DataItem, DataRowView)

            '名前にINDEXを設定
            With carTypeDivMain
                .ID = .ID & CStr(data("INDEX"))
            End With
            With carTypeLogoLbl
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarSeriesTable
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarSeriesCdLabel
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarSeriesNmTd
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarSeriesNmLabel
                .ID = .ID & CStr(data("INDEX"))
            End With
            With carTypeLogoImg
                .ID = .ID & CStr(data("INDEX"))
            End With
            With carTypeLogoP
                .ID = .ID & CStr(data("INDEX"))
                If String.Equals(customerCarsSelectedHiddenField.Value, CStr(data("KEY"))) Then
                    .ImageUrl = ResolveClientUrl((data("LOGO_SELECTED")).ToString)
                Else
                    .ImageUrl = ResolveClientUrl((data("LOGO_NOTSELECTED")).ToString)
                End If
            End With
            With customerCarGradeDiv
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarGradeLabelP
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarsBdyclrnmDiv
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarsBdyclrnmLabelP
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarsRightTable
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarsRegLabelP
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarsVINLabelP
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarsVCLDateLabelP
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarsKmLabelP
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarsDateLabelP
                .ID = .ID & CStr(data("INDEX"))
            End With
            With logoNotSelectid
                .ID = .ID & CStr(data("INDEX"))
            End With
            With logoSelectid
                .ID = .ID & CStr(data("INDEX"))
            End With
            With customerCarKey
                .ID = .ID & CStr(data("INDEX"))
            End With

            Dim selectCarTypePanel As Panel = CType(e.Item.FindControl("SelectCarTypePanel"), Panel)
            selectCarTypePanel.Attributes.Item("onclick") = "selectCarTypeClick(" & CStr(data("INDEX")) & ");return false;"

        End If
    End Sub
#End Region

#Region "画面設定 自社客車両 メイン画面"
    ''' <summary>
    ''' 画面設定 自社客車両 メイン画面
    ''' </summary>
    ''' <param name="drOrgVehicle"></param>
    ''' <remarks></remarks>
    Private Sub _SetControlOrgVehicleMain(ByVal drOrgVehicle As SC3080201DataSet.SC3080201OrgVehicleRow)
        'メイン画面の設定
        '担当SA名
        CustomerCarSANameLabel.Text = drOrgVehicle.USERNAME
        'モデルロゴ確認
        If String.IsNullOrEmpty(drOrgVehicle.LOGO_NOTSELECTED) Or _
          String.IsNullOrEmpty(drOrgVehicle.LOGO_SELECTED) Then
            'ラベル表示
            carTypeLogoLbl.Visible = True
            carTypeLogoImg.Visible = False
        Else
            'モデルロゴ
            CarTypeLogo.ImageUrl = ResolveClientUrl(drOrgVehicle.LOGO_NOTSELECTED)

            'モデルロゴ表示
            carTypeLogoLbl.Visible = False
            carTypeLogoImg.Visible = True
        End If
        'メーカー名
        customerCarSeriesCdLabel.Text = drOrgVehicle.SERIESCD
        'モデル名
        customerCarSeriesNmLabel.Text = drOrgVehicle.SERIESNM
        'グレード
        CustomerCarGradeLabel.Text = drOrgVehicle.GRADE
        '外鈑色名称
        CustomerCarsBdyclrnmLabel.Text = drOrgVehicle.BDYCLRNM
        '車両登録No
        CustomerCarsRegLabel.Text = drOrgVehicle.VCLREGNO
        'VIN
        CustomerCarsVINLabel.Text = drOrgVehicle.VIN
        '納車日 
        CustomerCarsVCLDateLabel.Text = CStr(drOrgVehicle.Item("VCLDELIDATESTRING"))
        '最新走行距離
        CustomerCarsKmLabel.Text = drOrgVehicle.MILEAGE
        '更新日
        CustomerCarsDateLabel.Text = CStr(drOrgVehicle.Item("UPDATEDATESTRING"))

        ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        '年式
        CustomerCarsModelYearLabel.Text = drOrgVehicle.MODEL_YEAR
        '走行距離
        CustomerCarsDistanceCoveredLabel.Text = drOrgVehicle.VCL_MILE
        ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

        ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
        ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        'JDP_Icon
        If Not drOrgVehicle.IsIMP_VCL_FLGNull AndAlso String.Equals(drOrgVehicle.IMP_VCL_FLG, VCL_VIP_FLG_LOYAL_CUSTOMER) Then
            Me.JDP_Icon.Visible = True
        Else
            Me.JDP_Icon.Visible = False
        End If
        ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

        ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END

        '車両編集ポップアップ用の値セット
        Me.SetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, CStr(drOrgVehicle.KEY))
        'selectVinHidden.Value = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, False), String) 'VIN
        'selectSeqnoHidden.Value = String.Empty
        editVehicleModeHidden.Value = CStr(SC3080205BusinessLogic.ModeEdit)

        '活動結果登録用の値をセット
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
        'Me.SetValue(ScreenPos.Current, CONST_VCLINFO, drOrgVehicle.KEY) 'VIN
        Me.SetValue(ScreenPos.Current, CONST_VCLINFO, drOrgVehicle.KEY_VCL) 'SEQNO
        '2013/06/30 TCS 黄 2013/10対応版　既存流用 END
    End Sub
    'Private Sub _SetControlOrgVehicleMain(ByVal drOrgVehicle As SC3080201DataSet.SC3080201OrgVehicleRow)
    '    'メイン画面の設定
    '    '担当SA名
    '    CustomerCarSANameLabel.Text = HttpUtility.HtmlEncode(drOrgVehicle.USERNAME)
    '    'モデルロゴ確認
    '    If String.Equals(Convert.ToString(drOrgVehicle.LOGO_NOTSELECTED), String.Empty) Or _
    '       String.Equals(Convert.ToString(drOrgVehicle.LOGO_SELECTED), String.Empty) Then
    '        'ラベル表示
    '        carTypeLogoLbl.Visible = True
    '        carTypeLogoImg.Visible = False
    '    Else
    '        'モデルロゴ
    '        CarTypeLogo.ImageUrl = ResolveClientUrl(drOrgVehicle.LOGO_NOTSELECTED)

    '        'モデルロゴ表示
    '        carTypeLogoLbl.Visible = False
    '        carTypeLogoImg.Visible = True
    '    End If
    '    'メーカー名
    '    customerCarSeriesCdLabel.Text = HttpUtility.HtmlEncode(drOrgVehicle.SERIESCD)
    '    'モデル名
    '    customerCarSeriesNmLabel.Text = HttpUtility.HtmlEncode(drOrgVehicle.SERIESNM)
    '    'グレード
    '    CustomerCarGradeLabel.Text = HttpUtility.HtmlEncode(drOrgVehicle.GRADE)
    '    '外鈑色名称
    '    CustomerCarsBdyclrnmLabel.Text = HttpUtility.HtmlEncode(drOrgVehicle.BDYCLRNM)
    '    '車両登録No
    '    CustomerCarsRegLabel.Text = HttpUtility.HtmlEncode(drOrgVehicle.VCLREGNO)
    '    'VIN
    '    CustomerCarsVINLabel.Text = HttpUtility.HtmlEncode(drOrgVehicle.VIN)
    '    '納車日 TODO:日付
    '    If drOrgVehicle.IsVCLDELIDATENull Then
    '        CustomerCarsVCLDateLabel.Text = String.Empty
    '    Else
    '        CustomerCarsVCLDateLabel.Text = HttpUtility.HtmlEncode(DateTimeFunc.FormatDate(3, drOrgVehicle.VCLDELIDATE))
    '    End If
    '    '最新走行距離
    '    CustomerCarsKmLabel.Text = HttpUtility.HtmlEncode(Convert.ToString(drOrgVehicle.MILEAGE) & WebWordUtility.GetWord(10130))
    '    Dim sc As StaffContext = StaffContext.Current
    '    '更新日
    '    If drOrgVehicle.IsUPDATEDATENull Then
    '        CustomerCarsDateLabel.Text = String.Empty
    '    Else
    '        CustomerCarsDateLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10120) & " " & DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, drOrgVehicle.UPDATEDATE, sc.DlrCD))
    '    End If

    '    '車両編集ポップアップ用の値セット
    '    Me.SetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, CStr(drOrgVehicle.KEY))
    '    'selectVinHidden.Value = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, False), String) 'VIN
    '    'selectSeqnoHidden.Value = String.Empty
    '    editVehicleModeHidden.Value = CStr(SC3080205BusinessLogic.ModeEdit)

    '    '活動結果登録用の値をセット
    '    Me.SetValue(ScreenPos.Current, CONST_VCLINFO, drOrgVehicle.KEY) 'VIN
    'End Sub
#End Region

#Region "画面設定 未取引客車両 メイン画面"
    ''' <summary>
    ''' 画面設定 未取引客車両 メイン画面
    ''' </summary>
    ''' <param name="drNewVehicle"></param>
    ''' <remarks></remarks>
    Private Sub _SetControlNewVehicleMain(ByVal drNewVehicle As SC3080201DataSet.SC3080201NewVehicleRow)
        'メイン画面の設定
        'モデルロゴ確認
        If String.IsNullOrEmpty(drNewVehicle.LOGO_NOTSELECTED) Or _
           String.IsNullOrEmpty(drNewVehicle.LOGO_SELECTED) Then
            'ラベル表示
            carTypeLogoLbl.Visible = True
            carTypeLogoImg.Visible = False
        Else
            'モデルロゴ
            CarTypeLogo.ImageUrl = ResolveClientUrl(drNewVehicle.LOGO_NOTSELECTED)

            'モデルロゴ表示
            carTypeLogoLbl.Visible = False
            carTypeLogoImg.Visible = True
        End If
        'メーカー名
        customerCarSeriesCdLabel.Text = drNewVehicle.SERIESCD
        'モデル名
        customerCarSeriesNmLabel.Text = drNewVehicle.SERIESNM
        '車両登録No
        CustomerCarsRegLabel.Text = drNewVehicle.VCLREGNO
        'VIN
        CustomerCarsVINLabel.Text = drNewVehicle.VIN
        '納車日        
        CustomerCarsVCLDateLabel.Text = CStr(drNewVehicle.Item("VCLDELIDATESTRING"))

        'グレード
        CustomerCarGradeLabel.Text = drNewVehicle.GRADE
        '外鈑色名称
        CustomerCarsBdyclrnmLabel.Text = drNewVehicle.BDYCLRNM
        '走行距離
        CustomerCarsKmLabel.Text = drNewVehicle.MILEAGE
        '更新日
        CustomerCarsDateLabel.Text = (CStr(drNewVehicle.Item("UPDATEDATESTRING")))

        ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        '年式
        CustomerCarsModelYearLabel.Text = drNewVehicle.MODEL_YEAR
        '走行距離
        CustomerCarsDistanceCoveredLabel.Text = drNewVehicle.VCL_MILE
        ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

        ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
        ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        'JDP_Icon
        If Not drNewVehicle.IsIMP_VCL_FLGNull AndAlso String.Equals(drNewVehicle.IMP_VCL_FLG, VCL_VIP_FLG_LOYAL_CUSTOMER) Then
            Me.JDP_Icon.Visible = True
        Else
            Me.JDP_Icon.Visible = False
        End If
        ' 2018/06/15 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END
        ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END

        '車両編集ポップアップ用の値セット
        Me.SetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, CStr(drNewVehicle.KEY))
        'selectVinHidden.Value = String.Empty
        'selectSeqnoHidden.Value = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, False), String) 'SEQNO
        editVehicleModeHidden.Value = CStr(SC3080205BusinessLogic.ModeEdit)

        '活動結果登録用の値をセット
        Me.SetValue(ScreenPos.Current, CONST_VCLINFO, drNewVehicle.KEY) 'SEQNO
    End Sub
    'Private Sub _SetControlNewVehicleMain(ByVal drNewVehicle As SC3080201DataSet.SC3080201NewVehicleRow)
    '    'メイン画面の設定
    '    'モデルロゴ確認
    '    If String.Equals(Convert.ToString(drNewVehicle.LOGO_NOTSELECTED), String.Empty) Or _
    '       String.Equals(Convert.ToString(drNewVehicle.LOGO_SELECTED), String.Empty) Then
    '        'ラベル表示
    '        carTypeLogoLbl.Visible = True
    '        carTypeLogoImg.Visible = False
    '    Else
    '        'モデルロゴ
    '        CarTypeLogo.ImageUrl = ResolveClientUrl(drNewVehicle.LOGO_NOTSELECTED)

    '        'モデルロゴ表示
    '        carTypeLogoLbl.Visible = False
    '        carTypeLogoImg.Visible = True
    '    End If
    '    'メーカー名
    '    customerCarSeriesCdLabel.Text = HttpUtility.HtmlEncode(drNewVehicle.SERIESCD)
    '    'モデル名
    '    customerCarSeriesNmLabel.Text = HttpUtility.HtmlEncode(drNewVehicle.SERIESNM)
    '    '車両登録No
    '    CustomerCarsRegLabel.Text = HttpUtility.HtmlEncode(drNewVehicle.VCLREGNO)
    '    'VIN
    '    CustomerCarsVINLabel.Text = HttpUtility.HtmlEncode(drNewVehicle.VIN)
    '    '納車日
    '    If drNewVehicle.IsVCLDELIDATENull Then
    '        CustomerCarsVCLDateLabel.Text = String.Empty
    '    Else
    '        CustomerCarsVCLDateLabel.Text = HttpUtility.HtmlEncode(DateTimeFunc.FormatDate(3, drNewVehicle.VCLDELIDATE))
    '    End If

    '    '車両編集ポップアップ用の値セット
    '    Me.SetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, CStr(drNewVehicle.KEY))
    '    'selectVinHidden.Value = String.Empty
    '    'selectSeqnoHidden.Value = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, False), String) 'SEQNO
    '    editVehicleModeHidden.Value = CStr(SC3080205BusinessLogic.ModeEdit)

    '    '活動結果登録用の値をセット
    '    Me.SetValue(ScreenPos.Current, CONST_VCLINFO, drNewVehicle.KEY) 'SEQNO
    'End Sub
#End Region

#Region "職業ポップアップ関連"
    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
#Region "職業ボタンエリア初期表示"
    Private Sub SetOccupationButtonArea(ByVal params As SC3080201DataSet.SC3080201ParameterDataTable)
        Logger.Info("SetOccupationButtonArea Start")

        '顧客職業取得
        Dim occupationDataTbl As SC3080201DataSet.SC3080201CustomerOccupationDataTable _
                               = SC3080201BusinessLogic.GetOccupationData(params)
        occupationDataTbl = SC3080201BusinessLogic.EditOccupatonData(occupationDataTbl)

        '職業ボタンエリアのみ設定
        Me._SetCustomerRelatedOccupationArea(occupationDataTbl)

        Logger.Info("SetOccupationButtonArea End")
    End Sub
#End Region

#Region "職業ポップアップ起動処理"

    ''' <summary>
    ''' 職業ポップアップ起動処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub OccupationOpenButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles OccupationOpenButton.Click
        Logger.Info("OccupationOpenButton_Click Start")

        'ポップアップを囲うPanelをVisible=Trueに
        Me.OccupationVisiblePanel.Visible = True

        '職業表示
        Me._SetCustomerRelatedOccupation()

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "occupationPopupOpen", "startup")

        Logger.Info("OccupationOpenButton_Click End")
    End Sub

#End Region
    '2012/03/08 TCS 山口 【SALES_2】性能改善 END

#Region "職業ポップアップ表示エリアの作成"
    ''' <summary>
    ''' 職業ポップアップ表示エリアの設定
    ''' </summary>
    ''' <param name="dt"></param>
    ''' <remarks></remarks>
    Private Sub _SetCustomerRelatedOccupationArea(ByVal dt As SC3080201DataSet.SC3080201CustomerOccupationDataTable)

        Dim drs As SC3080201DataSet.SC3080201CustomerOccupationRow() _
            = CType(dt.Select("SELECTION = 1"), SC3080201DataSet.SC3080201CustomerOccupationRow())

        If drs.Count = 0 Then
            Me.CustomerRelatedOccupationNewPanel.Visible = True
            Me.CustomerRelatedOccupationSelectedPanel.Visible = False
        Else
            Me.CustomerRelatedOccupationNewPanel.Visible = False
            Me.CustomerRelatedOccupationSelectedPanel.Visible = True

            Dim dr As SC3080201DataSet.SC3080201CustomerOccupationRow = drs(0)
            Me.CustomerRelatedOccupationSelectedImage.BackImageUrl = ResolveClientUrl(dr.ICONPATH_VIEWONLY)
            Me.CustomerRelatedOccupationSelectedLabel.Text = HttpUtility.HtmlEncode(dr.OCCUPATION)
        End If

    End Sub
#End Region

#Region "職業選択ポップアップ作成"
    ''' <summary>
    ''' 職業選択ポップアップ作成
    ''' </summary>
    ''' <param name="occupationDataTbl"></param>
    ''' <remarks></remarks>
    Private Sub _SetCustomerRelatedOccupationPopUp(ByVal occupationDataTbl As SC3080201DataSet.SC3080201CustomerOccupationDataTable)

        Me.OccupationPopuupTitlePage1.Value = WebWordUtility.GetWord(10122)
        Me.OccupationPopuupTitlePage2.Value = WebWordUtility.GetWord(10123)
        Me.OccupationOtherErrMsg.Value = WebWordUtility.GetWord(10902)
        '2012/04/26 TCS 河原 HTMLエンコード対応 START
        Me.CustomerRelatedOccupationRegistButton.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10126))
        '2012/04/26 TCS 河原 HTMLエンコード対応 END
        Me.CustomerRelatedOccupationOtherCustomTextBox.Text = String.Empty

        ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
        'マスタ件数に依存する表示調整
        Dim gridRowsCount As Integer = 1

        If Not occupationDataTbl Is Nothing AndAlso occupationDataTbl.Rows.Count > 0 Then
            gridRowsCount = occupationDataTbl.Rows.Count \ 4
            If occupationDataTbl.Rows.Count Mod 4 > 0 Then gridRowsCount += 1
        End If

        Select Case gridRowsCount
            Case 6
                Me.CustomerRelatedOccupationPopupArea.CssClass = "row6"
                Me.OccupationPopopBody.Height = Unit.Pixel(475)
                Me.occupationPopOverForm_1.Height = Unit.Pixel(475)
            Case 5
                Me.CustomerRelatedOccupationPopupArea.CssClass = "row5"
                Me.OccupationPopopBody.Height = Unit.Pixel(395)
                Me.occupationPopOverForm_1.Height = Unit.Pixel(395)
            Case 4
                Me.CustomerRelatedOccupationPopupArea.CssClass = "row4"
                Me.OccupationPopopBody.Height = Unit.Pixel(315)
                Me.occupationPopOverForm_1.Height = Unit.Pixel(315)
            Case 3
                Me.CustomerRelatedOccupationPopupArea.CssClass = "row3"
                Me.OccupationPopopBody.Height = Unit.Pixel(235)
                Me.occupationPopOverForm_1.Height = Unit.Pixel(235)
            Case 2
                Me.CustomerRelatedOccupationPopupArea.CssClass = "row2"
                Me.OccupationPopopBody.Height = Unit.Pixel(155)
                Me.occupationPopOverForm_1.Height = Unit.Pixel(155)
            Case 1
                Me.CustomerRelatedOccupationPopupArea.CssClass = "row1"
                Me.OccupationPopopBody.Height = Unit.Pixel(75)
                Me.occupationPopOverForm_1.Height = Unit.Pixel(75)

            Case Else
                Me.CustomerRelatedOccupationPopupArea.CssClass = "row7"
                Me.OccupationPopopBody.Height = Unit.Pixel(555)
                Me.occupationPopOverForm_1.Height = Unit.Pixel(555)
        End Select
        ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END

        'ポップアップ動的アイコン設定
        Me.CustomerRelatedOccupationButtonRepeater.DataSource = occupationDataTbl
        Me.CustomerRelatedOccupationButtonRepeater.DataBind()

    End Sub

    Protected Sub OccupationButtonAria_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles CustomerRelatedOccupationButtonRepeater.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dr As SC3080201DataSet.SC3080201CustomerOccupationRow = _
                CType(CType(e.Item.DataItem, DataRowView).Row, SC3080201DataSet.SC3080201CustomerOccupationRow)

            Dim occupationPanel As Panel = CType(e.Item.FindControl("CustomerRelatedOccupationPanel"), Panel)
            If dr.SELECTION.Equals("1") Then
                occupationPanel.BackImageUrl = ResolveClientUrl(dr.ICONPATH_SELECTED)
            Else
                occupationPanel.BackImageUrl = ResolveClientUrl(dr.ICONPATH_NOTSELECTED)
            End If

            Dim occupationLabel As Label = CType(e.Item.FindControl("CustomerRelatedOccupationText"), Label)
            occupationLabel.Text = HttpUtility.HtmlEncode(dr.OCCUPATION)
            If dr.SELECTION.Equals("1") Then
                occupationLabel.CssClass = occupationLabel.CssClass & " selectedFont"
                If dr.OTHER.Equals("1") Then
                    Me.CustomerRelatedOccupationOtherCustomTextBox.Text = dr.OCCUPATION
                End If

            End If

            '初期選択状態を保持
            Dim occupationSelectedField As HiddenField = CType(e.Item.FindControl("CustomerRelatedOccupationSelectedHiddenField"), HiddenField)
            occupationSelectedField.Value = dr.SELECTION

            If dr.OTHER.Equals("1") And dr.SELECTION.Equals("0") Then

                Dim occupationLink As LinkButton = CType(e.Item.FindControl("CustomerRelatedOccupationHyperLink"), LinkButton)
                occupationLink.OnClientClick = "setPopupOccupationPage('page2'," & dr.OCCUPATIONNO & ");return false;"

            End If

            Dim occupationFiled As HiddenField = CType(e.Item.FindControl("CustomerRelatedOccupationIdHiddenField"), HiddenField)
            occupationFiled.Value = CStr(dr.OCCUPATIONNO)

        End If

    End Sub
#End Region

#Region "職業選択時イベント"
    ''' <summary>
    ''' 職業選択時イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CustomerRelatedOccupationButtonRepeater_ItemCommand(ByVal sender As Object, ByVal e As RepeaterCommandEventArgs) Handles CustomerRelatedOccupationButtonRepeater.ItemCommand
        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        'ポップアップを囲うPanelをVisible=Falseに
        Me.OccupationVisiblePanel.Visible = False
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END

        '職業登録処理
        '2013/06/30 TCS 未 2013/10対応版　既存流用 START
        Me._RegistCustomerRelatedOccupation(CType(e.Item.FindControl("CustomerRelatedOccupationIdHiddenField"), HiddenField).Value, _
                                            " ", _
                                            CType(e.Item.FindControl("CustomerRelatedOccupationSelectedHiddenField"), HiddenField).Value)
        '2013/06/30 TCS 未 2013/10対応版　既存流用 END

        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        ''職業再表示
        'Me._SetCustomerRelatedOccupation()
        SetOccupationButtonArea(Me._SetParameters())
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END
    End Sub
#End Region

#Region "職業(その他)入力完了イベント"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub CustomerRelatedOccupationRegistButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerRelatedOccupationRegistButton.Click

        '禁則文字チェック
        If Not Validation.IsValidString(Me.CustomerRelatedOccupationOtherCustomTextBox.Text) Then
            ShowMessageBox(ERRMSGID_10909)
        Else
            '職業登録処理(その他)
            Me._RegistCustomerRelatedOccupation(Me.CustomerRelatedOccupationOtherIdHiddenField.Value, _
                                                Me.CustomerRelatedOccupationOtherCustomTextBox.Text, _
                                                String.Empty)

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            'ポップアップを囲うPanelをVisible=Falseに
            Me.OccupationVisiblePanel.Visible = False
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END
        End If
        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        ''職業再表示
        'Me._SetCustomerRelatedOccupation()
        SetOccupationButtonArea(Me._SetParameters())
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END
    End Sub
#End Region

#Region "職業再表示"
    Private Sub _SetCustomerRelatedOccupation()
        '顧客職業取得
        Dim occupationDataTbl As SC3080201DataSet.SC3080201CustomerOccupationDataTable _
            = SC3080201BusinessLogic.GetOccupationData(_SetParameters())
        occupationDataTbl = SC3080201BusinessLogic.EditOccupatonData(occupationDataTbl)
        Me._SetCustomerRelatedOccupationPopUp(occupationDataTbl)
        Me._SetCustomerRelatedOccupationArea(occupationDataTbl)

        'CustomTextBox
        JavaScriptUtility.RegisterStartupScript(Me.Page, " <script>" & "$(function() { $('#CustomerRelatedOccupationOtherCustomTextBox').CustomTextBox({ ""useEllipsis"": ""true"" }); });" & "</script>", "after")
    End Sub
#End Region

#Region "職業登録処理"
    ''' <summary>
    ''' 職業登録処理
    ''' </summary>
    ''' <param name="OccupationNo"></param>
    ''' <param name="otherOccupation"></param>
    ''' <remarks></remarks>
    Private Sub _RegistCustomerRelatedOccupation(ByVal OccupationNo As String, ByVal otherOccupation As String, ByVal selectedFlg As String)

        Using params As New SC3080201DataSet.SC3080201InsertCstOccupationDataTable '検索条件格納用

            Dim paramsDr As SC3080201DataSet.SC3080201InsertCstOccupationRow
            paramsDr = params.NewSC3080201InsertCstOccupationRow

            '登録値設定
            paramsDr.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
            paramsDr.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
            paramsDr.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
            '2013/06/30 TCS 未 2013/10対応版　既存流用 START
            paramsDr.ROWLOCKVERSION = CLng(Me.CustomerLockVersion.Value)
            If Not String.Equals(selectedFlg, "1") Then
                paramsDr.OCCUPATIONNO = OccupationNo
                paramsDr.OTHEROCCUPATION = otherOccupation
                If Not String.IsNullOrEmpty(Me.CustomerLockVersion.Value) Then
                    paramsDr.ROWLOCKVERSION = CLng(Me.CustomerLockVersion.Value)
                End If
            End If
            '2013/06/30 TCS 未 2013/10対応版　既存流用 END

            params.Rows.Add(paramsDr)

            Dim bizClass As New SC3080201BusinessLogic
            If bizClass.InsertCstOccupation(params) Then
                '2013/06/30 TCS 未 2013/10対応版　既存流用 START
                Me.CustomerLockVersion.Value = CStr(CLng(Me.CustomerLockVersion.Value) + 1)
                '2013/06/30 TCS 未 2013/10対応版　既存流用 END
                Exit Sub
            Else
                '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
                Call ShowMessageBox(901)
                '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            End If
        End Using
    End Sub
#End Region
    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
    '#Region "職業キャンセル押下処理"
    '    Protected Sub CustomerRelatedOccupationCancelButton_Click(sender As Object, e As System.EventArgs) Handles CustomerRelatedOccupationCancelButton.Click
    '        '職業再表示
    '        Me._SetCustomerRelatedOccupation()
    '    End Sub
    '#End Region
    '2012/03/08 TCS 山口 【SALES_2】性能改善 END
#End Region

#Region "家族ポップアップ関連"
    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
#Region "家族ボタンエリア初期表示"
    Private Sub SetFamilyButtonArea(ByVal params As SC3080201DataSet.SC3080201ParameterDataTable)
        Logger.Info("SetFamilyButtonArea Start")

        '顧客家族構成取得
        Dim custFamilyDataTbl As SC3080201DataSet.SC3080201CustomerFamilyDataTable _
            = SC3080201BusinessLogic.GetCustFamilyData(params)

        '件数保持
        Dim familyCount As Integer = custFamilyDataTbl.Rows.Count

        '家族ボタンエリアのみ設定
        Me._SetCustomerRelatedFamilyArea(familyCount)

        CustomerRelatedFaimySelectedEditPanel.Visible = True
        CustomerRelatedFamilySelectedNewPanel.Visible = False

        Logger.Info("SetFamilyButtonArea End")
    End Sub
#End Region

#Region "家族ポップアップ起動処理"

    ''' <summary>
    ''' 家族ポップアップ起動処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FamilyOpenButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles FamilyOpenButton.Click
        Logger.Info("FamilyOpenButton_Click Start")

        'ポップアップを囲うPanelをVisible=Trueに
        Me.FamilyVisiblePanel.Visible = True

        '家族表示
        Me.CustomerRelatedFamilyLoad()

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "familyPopupOpen", "startup")

        Logger.Info("FamilyOpenButton_Click End")
    End Sub

#End Region
    '2012/03/08 TCS 山口 【SALES_2】性能改善 END
#Region "画面設定 家族編集画面ポップアップボタン"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub _SetCustomerRelatedFamilyArea(ByVal familyCount As Integer)

        Me.FamilyPopuupTitlePage1.Value = WebWordUtility.GetWord(10147)
        Me.FamilyPopuupTitlePage2.Value = WebWordUtility.GetWord(10153)
        Me.FamilyPopuupTitlePage3.Value = WebWordUtility.GetWord(10158)

        '2012/04/26 TCS 河原 HTMLエンコード対応 START
        Me.CustomerRelatedFamilyRegistButton.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10126))
        '2012/04/26 TCS 河原 HTMLエンコード対応 END
        'Me.CustomerRelatedFamilyPopUpCancelButton.Text = WebWordUtility.GetWord(10125)

        Me.RelationOtherErrMsgHidden.Value = WebWordUtility.GetWord(10905)

        Me.familyOtherRelationshipTextBox.Text = String.Empty

        Me.FamilyCountLabel.Text = HttpUtility.HtmlEncode(familyCount)
        Me.FamilyCount.Value = CStr(familyCount)

        If 1 = familyCount Then
            Me.CustomerRelatedFamilySelectedImage.BackImageUrl = "~/Styles/Images/SC3080201/scNscCustomerInfoIcon2.png"
        Else
            Me.CustomerRelatedFamilySelectedImage.BackImageUrl = "~/Styles/Images/SC3080201/scNscCustomerInfoIcon2.png"
        End If


    End Sub
#End Region

#Region "画面設定 家族ポップアップ続柄一覧作成"
    ''' <summary>
    ''' 画面設定 家族続柄マスタ
    ''' </summary>
    ''' <param name="FamilyMstDataTbl"></param>
    ''' <remarks></remarks>
    Private Sub _SetCustomerRelatedFamilyPopUpRelationshipList(ByVal familyMstDataTbl As SC3080201DataSet.SC3080201CustomerFamilyMstDataTable)

        FamilyRelationshipRepeater.DataSource = familyMstDataTbl
        FamilyRelationshipRepeater.DataBind()

    End Sub

    Protected Sub FamilyRelationshipRepeater_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles FamilyRelationshipRepeater.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim data As SC3080201DataSet.SC3080201CustomerFamilyMstRow _
                = CType(CType(e.Item.DataItem, DataRowView).Row, SC3080201DataSet.SC3080201CustomerFamilyMstRow)


            With CType(e.Item.FindControl("familyRelationshipLabel_No"), CustomLabel)
                .ID = .ID & "_" & data.FAMILYRELATIONSHIPNO
                .Text = HttpUtility.HtmlEncode(data.FAMILYRELATIONSHIP)
            End With

            With CType(e.Item.FindControl("familyRelationshipNoHidden_No"), HiddenField)
                .ID = .ID & "_" & data.FAMILYRELATIONSHIPNO
                .Value = CStr(data.FAMILYRELATIONSHIPNO)
            End With

            With CType(e.Item.FindControl("familyRelationshipList_No"), HtmlGenericControl)

                .ID = .ID & "_" & data.FAMILYRELATIONSHIPNO
                If data.OTHERUNKNOWN.Equals("1") Then
                    .Attributes.Add("onclick", "setPopupFamilyPage('page3','page2'," & data.FAMILYRELATIONSHIPNO & ")")
                    Me.RelationOtherWordHidden.Value = data.FAMILYRELATIONSHIP
                    Me.RelationOtherNoHidden.Value = CStr(data.FAMILYRELATIONSHIPNO)
                Else
                    .Attributes.Add("onclick", "selectFamilyRelationship('" & data.FAMILYRELATIONSHIPNO & "')")
                End If

            End With

        End If

    End Sub
#End Region

#Region "画面設定 家族ポップアップ家族構成一覧作成"
    ''' <summary>
    ''' 画面設定 顧客家族構成
    ''' </summary>
    ''' <param name="FamilyDataTbl"></param>
    ''' <remarks></remarks>
    Private Sub _SetCustomerRelatedFamilyPopUpFamilyList(ByVal FamilyDataTbl As SC3080201DataSet.SC3080201CustomerFamilyDataTable, _
                                                         ByVal familyCount As Integer)

        '人数ボタン選択解除
        For i = 1 To 10
            DirectCast(Me.FindControl("FamilyCount" & i), System.Web.UI.HtmlControls.HtmlAnchor).Attributes.Remove("class")
        Next i

        '人数ボタン選択
        Select Case familyCount
            Case 1
                Me.FamilyCount1.Attributes.Add("class", "selectedButton")
            Case 2
                Me.FamilyCount2.Attributes.Add("class", "selectedButton")
            Case 3
                Me.FamilyCount3.Attributes.Add("class", "selectedButton")
            Case 4
                Me.FamilyCount4.Attributes.Add("class", "selectedButton")
            Case 5
                Me.FamilyCount5.Attributes.Add("class", "selectedButton")
            Case 6
                Me.FamilyCount6.Attributes.Add("class", "selectedButton")
            Case 7
                Me.FamilyCount7.Attributes.Add("class", "selectedButton")
            Case 8
                Me.FamilyCount8.Attributes.Add("class", "selectedButton")
            Case 9
                Me.FamilyCount9.Attributes.Add("class", "selectedButton")
            Case 10
                Me.FamilyCount10.Attributes.Add("class", "selectedButton")
        End Select

        Me.FamilyNumber = familyCount


        CustomerRelatedFaimySelectedEditPanel.Visible = True
        CustomerRelatedFamilySelectedNewPanel.Visible = False

        familyBirthdayList.DataSource = FamilyDataTbl
        familyBirthdayList.DataBind()
    End Sub

    Protected Sub FamilyBirthdayList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles familyBirthdayList.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim data As SC3080201DataSet.SC3080201CustomerFamilyRow _
                = CType(CType(e.Item.DataItem, DataRowView).Row, SC3080201DataSet.SC3080201CustomerFamilyRow)

            With CType(e.Item.FindControl("familyBirthdayList_Row"), HtmlGenericControl)
                If FamilyNumber = (e.Item.ItemIndex + 1) Then
                    .Attributes.Add("class", "familyBirthdayListAreaNoBorder")
                ElseIf FamilyNumber < (e.Item.ItemIndex + 1) Then
                    .Attributes.Add("class", "displaynone")
                End If
            End With

            With CType(e.Item.FindControl("familyBirthdayListRelationLabel_Row"), CustomLabel)
                .Text = HttpUtility.HtmlEncode(data.FAMILYRELATIONSHIP)
                If Not e.Item.ItemIndex = 0 Then
                    .Attributes.Add("onclick", "setPopupFamilyPage('page2','page1'," & e.Item.ItemIndex & ")")
                End If
            End With

            CType(e.Item.FindControl("familyBirthdayListRelationNoHidden_Row"), HiddenField).Value = CStr(data.FAMILYRELATIONSHIPNO)
            CType(e.Item.FindControl("familyBirthdayListFamilyNoHidden_Row"), HiddenField).Value = CStr(data.FAMILYNO)
            CType(e.Item.FindControl("familyBirthdayListRelationOtherHidden_Row"), HiddenField).Value = CStr(data.OTHERFAMILYRELATIONSHIP)

            With CType(e.Item.FindControl("familyBirthdayListBirthdayDate_Row"), DateTimeSelector)
                If Not data.IsBIRTHDAYNull Then
                    .Value = data.BIRTHDAY
                    CType(e.Item.FindControl("familyBirthdayHidden_Row"), HiddenField).Value = CStr(data.BIRTHDAY)
                End If

                If e.Item.ItemIndex = 0 Then
                    .Enabled = False
                End If
            End With

        End If

    End Sub

#End Region

#Region "家族登録押下イベント"
    Protected Sub CustomerRelatedFamilyRegistButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerRelatedFamilyRegistButton.Click

        '登録処理
        Me.RegistCustomerRelatedFamily()

        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        ''ポップアップ再表示
        'Me.CustomerRelatedFamilyLoad()
        SetFamilyButtonArea(Me._SetParameters())
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END
    End Sub

    ''' <summary>
    ''' 家族ポップアップ再表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CustomerRelatedFamilyLoad()
        Dim params As SC3080201DataSet.SC3080201ParameterDataTable = _SetParameters()

        '家族構成マスタ取得
        Dim custFamilyMstDataTbl As SC3080201DataSet.SC3080201CustomerFamilyMstDataTable _
            = SC3080201BusinessLogic.GetCustFamilyMstData(params)

        Me._SetCustomerRelatedFamilyPopUpRelationshipList(custFamilyMstDataTbl)

        '顧客家族構成取得
        Dim custFamilyDataTbl As SC3080201DataSet.SC3080201CustomerFamilyDataTable _
            = SC3080201BusinessLogic.GetCustFamilyData(params)
        '不明行追加前に件数保持
        Dim familyCount As Integer = custFamilyDataTbl.Rows.Count
        '本人行編集、不明行追加
        custFamilyDataTbl = SC3080201BusinessLogic.EditCustFamilyData(custFamilyDataTbl, custFamilyMstDataTbl)
        Me._SetCustomerRelatedFamilyArea(familyCount)
        Me._SetCustomerRelatedFamilyPopUpFamilyList(custFamilyDataTbl, familyCount)

        CustomerRelatedFaimySelectedEditPanel.Visible = True
        CustomerRelatedFamilySelectedNewPanel.Visible = False

        'JavaScriptUtility.RegisterStartupFunctionCallScript(Page, "bindFingerScroll", "startup")
        'CustomTextBox
        JavaScriptUtility.RegisterStartupScript(Me.Page, " <script>" & "$(function() { $('#familyOtherRelationshipTextBox').CustomTextBox({ ""useEllipsis"": ""true"" }); });" & "</script>", "after")
    End Sub

#End Region

#Region "家族登録処理"
    Private Sub RegistCustomerRelatedFamily()

        Using dt As New SC3080201DataSet.SC3080201InsertCstFamilyDataTable
            ''1番目は顧客家族構成削除、家族人数登録用に使用
            For i As Integer = 0 To CInt(Me.FamilyCount.Value) - 1
                Dim dr As SC3080201DataSet.SC3080201InsertCstFamilyRow = dt.NewSC3080201InsertCstFamilyRow

                dr.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                dr.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                dr.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                '2013/06/30 TCS 未 2013/10対応版　既存流用 START
                dr.ROWLOCKVERSION = CLng(Me.CustomerDLRLockVersion.Value)
                '2013/06/30 TCS 未 2013/10対応版　既存流用 END
                dr.FAMILYNO = i
                dr.FAMILYRELATIONSHIPNO = CInt(CType(familyBirthdayList.Items(i).FindControl("familyBirthdayListRelationNoHidden_Row"), HiddenField).Value)
                '禁則文字チェック
                If Not i = 0 Then
                    Dim otherFamilyRelationShip As String = CStr(CType(familyBirthdayList.Items(i).FindControl("familyBirthdayListRelationOtherHidden_Row"), HiddenField).Value)
                    If Not String.IsNullOrEmpty(otherFamilyRelationShip) AndAlso Not Validation.IsValidString(otherFamilyRelationShip) Then
                        ShowMessageBox(ERRMSGID_10910)
                        Return
                    End If
                End If
                dr.OTHERFAMILYRELATIONSHIP = CStr(CType(familyBirthdayList.Items(i).FindControl("familyBirthdayListRelationOtherHidden_Row"), HiddenField).Value)
                'If Not CType(familyBirthdayList.Items(i).FindControl("familyBirthdayListBirthdayDate_Row"), DateTimeSelector).Value Is Nothing Then
                '    dr.BIRTHDAY = CDate(CType(familyBirthdayList.Items(i).FindControl("familyBirthdayListBirthdayDate_Row"), DateTimeSelector).Value)
                'End If
                'If Not CType(familyBirthdayList.Items(i).FindControl("familyBirthdayHidden_Row"), HiddenField).Value Is Nothing Then
                '    dr.BIRTHDAY = CDate((CType(familyBirthdayList.Items(i).FindControl("familyBirthdayHidden_Row"), HiddenField).Value))
                'End If
                If Not String.IsNullOrEmpty(CType(familyBirthdayList.Items(i).FindControl("familyBirthdayHidden_Row"), HiddenField).Value) Then
                    dr.BIRTHDAY = CType((CType(familyBirthdayList.Items(i).FindControl("familyBirthdayHidden_Row"), HiddenField).Value), Date)
                    'dr.BIRTHDAY = CDate((CType(familyBirthdayList.Items(i).FindControl("familyBirthdayHidden_Row"), HiddenField).Value))
                End If
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
                dr.NUMBEROFFAMILY = Me.FamilyCount.Value
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

                dt.Rows.Add(dr)

            Next i
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
            Dim bizClass As New SC3080201BusinessLogic
            If (Not bizClass.InsertCstFamily(dt)) Then
                Call ShowMessageBox(901)
            End If
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            'ポップアップを囲うPanelをVisible=falseに
            Me.FamilyVisiblePanel.Visible = False
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START 
            Me.CustomerDLRLockVersion.Value = CStr(CLng(Me.CustomerDLRLockVersion.Value) + 1)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        End Using
    End Sub
#End Region
    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
    '#Region "家族キャンセル押下処理"
    '    Protected Sub CustomerRelatedFamilyCancelButton_Click(sender As Object, e As System.EventArgs) Handles CustomerRelatedFamilyCancelButton.Click
    '        'ポップアップ再表示
    '        Me.CustomerRelatedFamilyLoad()
    '    End Sub
    '#End Region
    '2012/03/08 TCS 山口 【SALES_2】性能改善 END
#End Region

#Region "趣味ポップアップ関連"
    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
#Region "趣味ボタンエリア初期表示"
    Private Sub SetHobbyButtonArea(ByVal params As SC3080201DataSet.SC3080201ParameterDataTable)
        Logger.Info("SetHobbyButtonArea Start")

        '顧客趣味取得
        Dim hobbyDataTbl As SC3080201DataSet.SC3080201CustomerHobbyDataTable _
            = SC3080201BusinessLogic.GetHobbyData(params)

        '趣味ボタンエリアのみ設定
        Me._SetCustomerRelatedHobbyArea(hobbyDataTbl)

        Logger.Info("SetHobbyButtonArea End")
    End Sub
#End Region

#Region "趣味ポップアップ起動処理"

    ''' <summary>
    ''' 趣味ポップアップ起動処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub HobbyOpenButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles HobbyOpenButton.Click
        Logger.Info("HobbyOpenButton_Click Start")

        'ポップアップを囲うPanelをVisible=Trueに
        Me.HobbyVisiblePanel.Visible = True

        '趣味表示
        Me.CustomerRelatedHobbyLoad()

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "hobbyPopupOpen", "startup")

        Logger.Info("HobbyOpenButton_Click End")
    End Sub

#End Region

#Region "画面設定 趣味ポップアップ作成"
    Private Sub _SetCustomerRelatedHobbyPopUp(ByVal dt As SC3080201DataSet.SC3080201CustomerHobbyDataTable)
        Dim otherDrs As SC3080201DataSet.SC3080201CustomerHobbyRow() _
            = CType(dt.Select("SORTNO_1ST = '1' AND OTHER = '1'"), SC3080201DataSet.SC3080201CustomerHobbyRow())

        If otherDrs.Count() = 0 Then
            Me.CustomerRelatedHobbyPopupOtherHobbyNo.Value = ""
            Me.CustomerRelatedHobbyPopupOtherHobbyDefaultText.Value = ""
        Else
            Me.CustomerRelatedHobbyPopupOtherHobbyNo.Value = CStr(otherDrs(0).HOBBYNO)
            Me.CustomerRelatedHobbyPopupOtherHobbyDefaultText.Value = otherDrs(0).HOBBY
        End If

        '取得件数保持
        Dim selCountDrs As SC3080201DataSet.SC3080201CustomerHobbyRow() _
            = CType(dt.Select("SORTNO_1ST = '1'"), SC3080201DataSet.SC3080201CustomerHobbyRow())
        CustomerRelatedHobbyPopupRowCount.Value = CStr(selCountDrs.Count())

        '取得したデータの編集
        dt = SC3080201BusinessLogic.EditHobbyData(dt)
        'その他テキストボックス初期化
        Me.CustomerRelatedHobbyPopupOtherText.Text = String.Empty

        ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 START
        'マスタ件数に依存する表示調整
        Dim gridRowsCount As Integer = 1

        If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
            gridRowsCount = dt.Rows.Count \ 4
            If dt.Rows.Count Mod 4 > 0 Then gridRowsCount += 1
        End If

        Select Case gridRowsCount
            Case 6
                Me.CustomerRelatedHobbyPopupArea.CssClass = "row6"
                Me.HobbyPopupBody.Height = Unit.Pixel(475)
                Me.CustomerRelatedHobbyPopupPage1.Height = Unit.Pixel(475)
            Case 5
                Me.CustomerRelatedHobbyPopupArea.CssClass = "row5"
                Me.HobbyPopupBody.Height = Unit.Pixel(395)
                Me.occupationPopOverForm_1.Height = Unit.Pixel(395)
            Case 4
                Me.CustomerRelatedHobbyPopupArea.CssClass = "row4"
                Me.HobbyPopupBody.Height = Unit.Pixel(315)
                Me.CustomerRelatedHobbyPopupPage1.Height = Unit.Pixel(315)
            Case 3
                Me.CustomerRelatedHobbyPopupArea.CssClass = "row3"
                Me.HobbyPopupBody.Height = Unit.Pixel(235)
                Me.CustomerRelatedHobbyPopupPage1.Height = Unit.Pixel(235)
            Case 2
                Me.CustomerRelatedHobbyPopupArea.CssClass = "row2"
                Me.HobbyPopupBody.Height = Unit.Pixel(155)
                Me.CustomerRelatedHobbyPopupPage1.Height = Unit.Pixel(155)
            Case 1
                Me.CustomerRelatedHobbyPopupArea.CssClass = "row1"
                Me.HobbyPopupBody.Height = Unit.Pixel(75)
                Me.CustomerRelatedHobbyPopupPage1.Height = Unit.Pixel(75)

            Case Else
                Me.CustomerRelatedHobbyPopupArea.CssClass = "row7"
                Me.HobbyPopupBody.Height = Unit.Pixel(555)
                Me.CustomerRelatedHobbyPopupPage1.Height = Unit.Pixel(555)
        End Select
        ' 2013/11/27 TCS 市川 Aカード情報相互連携開発 END

        Me.CustomerRelatedHobbyPopupSelectButtonRepeater.DataSource = dt
        Me.CustomerRelatedHobbyPopupSelectButtonRepeater.DataBind()

    End Sub
#End Region
    '2012/03/08 TCS 山口 【SALES_2】性能改善 END

#Region "画面設定 趣味ポップアップ"
    Private Sub _SetCustomerRelatedHobbyArea(ByVal dt As SC3080201DataSet.SC3080201CustomerHobbyDataTable)

        '2012/04/26 TCS 河原 HTMLエンコード対応 START
        Me.registCustomerRelatedHobbyButton.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10126))
        '2012/04/26 TCS 河原 HTMLエンコード対応 END
        Me.CustomerRelatedHobbyPopupTitlePage1.Value = WebWordUtility.GetWord(10127)
        Me.CustomerRelatedHobbyPopupTitlePage2.Value = WebWordUtility.GetWord(10128)
        Me.HobbyOthererrMsg.Value = WebWordUtility.GetWord(10907)


        Dim selDrs As SC3080201DataSet.SC3080201CustomerHobbyRow() _
            = CType(dt.Select("SELECTION = '1' AND SORTNO_1ST = '1'"), SC3080201DataSet.SC3080201CustomerHobbyRow())

        If selDrs.Count = 0 Then
            Me.CustomerRelatedHobbySelectedNewPanel.Visible = True
            Me.CustomerRelatedHobbySelectedEditPanel.Visible = False
        Else
            Me.CustomerRelatedHobbySelectedNewPanel.Visible = False
            Me.CustomerRelatedHobbySelectedEditPanel.Visible = True

            Me.HobbyCountLabel.Text = HttpUtility.HtmlEncode(selDrs.Count)
            Me.CustomerRelatedHobbySelectedImage.BackImageUrl = ResolveClientUrl(selDrs(0).ICONPATH_VIEWONLY)
            Me.CustomerRelatedHobbySelectedLabel.Text = HttpUtility.HtmlEncode(selDrs(0).HOBBY)

        End If

        Dim selOtherDrs As SC3080201DataSet.SC3080201CustomerHobbyRow() _
            = CType(dt.Select("SELECTION = '1' AND SORTNO_1ST = '2'"), SC3080201DataSet.SC3080201CustomerHobbyRow())
        If selDrs.Count = 1 And selOtherDrs.Count = 1 Then
            'その他のみ選択の場合
            Me.CustomerRelatedHobbySelectedLabel.Text = HttpUtility.HtmlEncode(selOtherDrs(0).HOBBY)
        End If

        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        'Dim otherDrs As SC3080201DataSet.SC3080201CustomerHobbyRow() _
        '    = CType(dt.Select("SORTNO_1ST = '1' AND OTHER = '1'"), SC3080201DataSet.SC3080201CustomerHobbyRow())

        'If otherDrs.Count() = 0 Then
        '    Me.CustomerRelatedHobbyPopupOtherHobbyNo.Value = ""
        '    Me.CustomerRelatedHobbyPopupOtherHobbyDefaultText.Value = ""
        'Else
        '    Me.CustomerRelatedHobbyPopupOtherHobbyNo.Value = CStr(otherDrs(0).HOBBYNO)
        '    Me.CustomerRelatedHobbyPopupOtherHobbyDefaultText.Value = otherDrs(0).HOBBY
        'End If


        ''取得件数保持
        'Dim selCountDrs As SC3080201DataSet.SC3080201CustomerHobbyRow() _
        '    = CType(dt.Select("SORTNO_1ST = '1'"), SC3080201DataSet.SC3080201CustomerHobbyRow())
        'CustomerRelatedHobbyPopupRowCount.Value = CStr(selCountDrs.Count())

        ''取得したデータの編集
        'dt = SC3080201BusinessLogic.EditHobbyData(dt)
        ''その他テキストボックス初期化
        'Me.CustomerRelatedHobbyPopupOtherText.Text = String.Empty

        'Me.CustomerRelatedHobbyPopupSelectButtonRepeater.DataSource = dt
        'Me.CustomerRelatedHobbyPopupSelectButtonRepeater.DataBind()
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END

    End Sub

    Private Sub CustomerRelatedHobbyPopupSelectButtonRepeater_ItemDataBound1(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) _
        Handles CustomerRelatedHobbyPopupSelectButtonRepeater.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dr As SC3080201DataSet.SC3080201CustomerHobbyRow = _
                CType(CType(e.Item.DataItem, DataRowView).Row, SC3080201DataSet.SC3080201CustomerHobbyRow)

            With CType(e.Item.FindControl("CustomerRelatedHobbyPopupSelectButtonPanel_Row"), Panel)

                If dr.SELECTION.Equals("1") Then
                    .BackImageUrl = ResolveClientUrl(dr.ICONPATH_SELECTED)
                Else
                    .BackImageUrl = ResolveClientUrl(dr.ICONPATH_NOTSELECTED)
                End If

                If dr.OTHER.Equals("1") Then
                    .Attributes.Add("onclick", "setCustomerRelatedHobbyPopupPage('page2','" & e.Item.ItemIndex & "');")
                Else
                    .Attributes.Add("onclick", "selectCustomerRelatedHobbyPopupButton('" & e.Item.ItemIndex & "');")
                End If

            End With

            With CType(e.Item.FindControl("CustomerRelatedHobbyPopupSelectButtonTitleLabel_Row"), Label)
                .Text = HttpUtility.HtmlEncode(dr.HOBBY)
                If dr.SELECTION.Equals("1") Then
                    .CssClass = "selectedButton ellipsis"
                End If
            End With
            If dr.OTHER.Equals("1") Then
                CustomerRelatedHobbyPopupOtherHiddenField.Value = dr.HOBBY
            End If

            CType(e.Item.FindControl("CustomerRelatedHobbyPopupSelectButtonOther_Row"), HiddenField).Value = dr.OTHER
            CType(e.Item.FindControl("CustomerRelatedHobbyPopupSelectButtonHobbyNo_Row"), HiddenField).Value = CStr(dr.HOBBYNO)
            CType(e.Item.FindControl("CustomerRelatedHobbyPopupSelectButtonCheck_Row"), HiddenField).Value = dr.SELECTION
            CType(e.Item.FindControl("CustomerRelatedHobbyPopupSelectedButtonPath_Row"), HiddenField).Value = ResolveClientUrl(dr.ICONPATH_SELECTED)
            CType(e.Item.FindControl("CustomerRelatedHobbyPopupNotSelectedButtonPath_Row"), HiddenField).Value = ResolveClientUrl(dr.ICONPATH_NOTSELECTED)

        End If

    End Sub
#End Region

#Region "趣味登録押下イベント"
    Protected Sub registCustomerRelatedHobbyButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles registCustomerRelatedHobbyButton.Click
        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        'ポップアップを囲うPanelをVisible=falseに
        Me.HobbyVisiblePanel.Visible = False
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END

        '禁則文字チェック
        If Not Validation.IsValidString(Me.CustomerRelatedHobbyPopupOtherHiddenField.Value) Then
            ShowMessageBox(ERRMSGID_10911)
        Else
            '登録処理
            Me.RegistCustomerRelatedHobby()

            '2012/03/08 TCS 山口 【SALES_2】性能改善 START
            'ポップアップを囲うPanelをVisible=falseに
            Me.FamilyVisiblePanel.Visible = False
            '2012/03/08 TCS 山口 【SALES_2】性能改善 END
        End If

        'ポップアップ再表示
        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        'Me.CustomerRelatedHobbyLoad()
        SetHobbyButtonArea(Me._SetParameters())
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END

    End Sub

    ''' <summary>
    ''' 趣味ポップアップ再表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CustomerRelatedHobbyLoad()
        Dim params As SC3080201DataSet.SC3080201ParameterDataTable = _SetParameters()

        '顧客趣味取得
        Dim hobbyDataTbl As SC3080201DataSet.SC3080201CustomerHobbyDataTable _
            = SC3080201BusinessLogic.GetHobbyData(params)

        Me._SetCustomerRelatedHobbyArea(hobbyDataTbl)
        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        Me._SetCustomerRelatedHobbyPopUp(hobbyDataTbl)
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END

        'CustomTextBox
        JavaScriptUtility.RegisterStartupScript(Me.Page, " <script>" & "$(function() { $('#CustomerRelatedHobbyPopupOtherText').CustomTextBox({ ""useEllipsis"": ""true"" }); });" & "</script>", "after")
    End Sub

#End Region

#Region "趣味登録処理"
    Private Sub RegistCustomerRelatedHobby()

        Using dt As New SC3080201DataSet.SC3080201InsertCstHobbyDataTable
            For i As Integer = 0 To CInt(CustomerRelatedHobbyPopupRowCount.Value) - 1
                If String.Equals(CType(CustomerRelatedHobbyPopupSelectButtonRepeater.Items(i).FindControl("CustomerRelatedHobbyPopupSelectButtonCheck_Row"), HiddenField).Value, "1") Then
                    '選択されているもののみ登録
                    Dim drInsert As SC3080201DataSet.SC3080201InsertCstHobbyRow = dt.NewSC3080201InsertCstHobbyRow

                    drInsert.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                    drInsert.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                    drInsert.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                    drInsert.HOBBYNO = CInt(CType(CustomerRelatedHobbyPopupSelectButtonRepeater.Items(i).FindControl("CustomerRelatedHobbyPopupSelectButtonHobbyNo_Row"), HiddenField).Value)

                    If String.Equals(CType(CustomerRelatedHobbyPopupSelectButtonRepeater.Items(i).FindControl("CustomerRelatedHobbyPopupSelectButtonOther_Row"), HiddenField).Value, "1") Then
                        'その他の場合
                        drInsert.OTHERHOBBY = CustomerRelatedHobbyPopupOtherHiddenField.Value
                    Else
                        drInsert.OTHERHOBBY = String.Empty
                    End If

                    dt.Rows.Add(drInsert)

                End If
            Next i

            Dim bizClass As New SC3080201BusinessLogic
            If dt.Rows.Count = 0 Then
                '削除のみ行う
                Dim drDelete As SC3080201DataSet.SC3080201InsertCstHobbyRow = dt.NewSC3080201InsertCstHobbyRow
                drDelete.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                drDelete.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                drDelete.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                dt.Rows.Add(drDelete)

                bizClass.InsertCstHobby(dt)
            Else
                bizClass.InsertCstHobby(dt)

            End If
        End Using
    End Sub
#End Region

    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
    '#Region "趣味キャンセル押下処理"
    '    Protected Sub CustomerRelatedHobbyPopupCancelButton_Click(sender As Object, e As System.EventArgs) Handles CustomerRelatedHobbyPopupCancelButton.Click
    '        'ポップアップ再表示
    '        Me.CustomerRelatedHobbyLoad()
    '    End Sub
    '#End Region
    '2012/03/08 TCS 山口 【SALES_2】性能改善 END

#End Region

#Region "希望連絡方法ポップアップ作成"
    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
#Region "希望連絡方法ボタンエリア初期表示"
    Private Sub SetContactButtonArea(ByVal params As SC3080201DataSet.SC3080201ParameterDataTable)
        Logger.Info("SetContactButtonArea Start")

        '希望コンタクト方法取得
        Dim contactFlg As SC3080201DataSet.SC3080201ContactFlgDataTable _
            = SC3080201BusinessLogic.GetContactFlg(params)

        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        Dim contactSetflg As String = SC3080201BusinessLogic.GetContactSetFlg(params)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

        '希望連絡方法ボタンエリアのみ設定
        Me._SetCustomerRelatedContactPopupContactTool(contactFlg)

        Logger.Info("SetContactButtonArea End")
    End Sub
#End Region

#Region "希望連絡方法ポップアップ起動処理"

    ''' <summary>
    ''' 希望連絡方法ポップアップ起動処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ContactOpenButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContactOpenButton.Click
        Logger.Info("ContactOpenButton_Click Start")

        'ポップアップを囲うPanelをVisible=Trueに
        Me.ContactVisiblePanel.Visible = True

        '希望連絡方法表示
        Me.CustomerRelatedContactLoad()

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "contactPopupOpen", "startup")

        Logger.Info("ContactOpenButton_Click End")
    End Sub
#End Region

#Region "希望連絡方法ポップアップ設定"
    '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    Private Sub _SetCustomerRelatedHobbyPopUp(ByVal contactFlgTbl As SC3080201DataSet.SC3080201ContactFlgDataTable, _
                                              ByVal contactSetflg As String)
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        Dim drContactFlg As SC3080201DataSet.SC3080201ContactFlgRow = CType(contactFlgTbl.Rows(0), SC3080201DataSet.SC3080201ContactFlgRow)

        Dim mobileFlg As String = drContactFlg.CONTACTMOBILEFLG
        Dim homeFlg As String = drContactFlg.CONTACTHOMEFLG
        Dim smsFlg As String = drContactFlg.CONTACTSMSFLG
        Dim emailFlg As String = drContactFlg.CONTACTEMAILFLG
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        Dim dmFlg As String = drContactFlg.CONTACTDMFLG
        Dim dmUseallFlg As Long = CLng(Mid(contactSetflg, 1, 1))
        Dim dmUsesmsFlg As Long = CLng(Mid(contactSetflg, 2, 1))
        Dim dmUseemailFlg As Long = CLng(Mid(contactSetflg, 3, 1))
        Dim dmUsedmailFlg As Long = CLng(Mid(contactSetflg, 4, 1))
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

        Dim selectedCssClass As String = "scNscPopUpContactSelectBtnMiddleOn"
        Dim selectedString As String = "selected"

        Dim mobileIconFileName As String = "scNscContactSelectIconMobile"

        If mobileFlg.Equals(ContactFlgOn) Then
            Me.ContactToolMobileLi.Attributes.Add(ClassString, selectedCssClass)
            Me.ContactToolMobileHidden.Value = ContactFlgOn
            Me.ContactToolMobileImage.CssClass = selectedString & " ContactToolIcon"
        Else
            Me.ContactToolMobileLi.Attributes.Remove(ClassString)
            Me.ContactToolMobileHidden.Value = ContactFlgOff
            Me.ContactToolMobileImage.CssClass = "ContactToolIcon"
        End If

        Dim homeIconFileName As String = "scNscContactSelectIconTel"
        If homeFlg.Equals(ContactFlgOn) Then
            Me.ContactToolTelLi.Attributes.Add(ClassString, selectedCssClass)
            Me.ContactToolTelHidden.Value = ContactFlgOn
            Me.ContactToolTelImage.CssClass = selectedString & " ContactToolIcon"
        Else
            Me.ContactToolTelLi.Attributes.Remove(ClassString)
            Me.ContactToolTelHidden.Value = ContactFlgOff
            Me.ContactToolTelImage.CssClass = "ContactToolIcon"
        End If

        Dim smsIconFileName As String = "scNscContactSelectIconSMS"
        If smsFlg.Equals(ContactFlgOn) Then
            Me.ContactToolSMSLi.Attributes.Add(ClassString, selectedCssClass)
            Me.ContactToolSMSHidden.Value = ContactFlgOn
            Me.ContactToolSMSImage.CssClass = selectedString & " ContactToolIcon"
        Else
            Me.ContactToolSMSLi.Attributes.Remove(ClassString)
            Me.ContactToolSMSHidden.Value = ContactFlgOff
            Me.ContactToolSMSImage.CssClass = "ContactToolIcon"
        End If

        Dim emailIconFileName As String = "scNscContactSelectIconEmail"
        If emailFlg.Equals(ContactFlgOn) Then
            Me.ContactToolEmailLi.Attributes.Add(ClassString, selectedCssClass)
            Me.ContactToolEmailHidden.Value = ContactFlgOn
            Me.ContactToolEmailImage.CssClass = selectedString & " ContactToolIcon"
        Else
            Me.ContactToolEmailLi.Attributes.Remove(ClassString)
            Me.ContactToolEmailHidden.Value = ContactFlgOff
            Me.ContactToolEmailImage.CssClass = "ContactToolIcon"
        End If

        Dim dmIconFileName As String = "scNscContactSelectIconDM"
        If dmFlg.Equals(ContactFlgOn) Then
            Me.ContactToolDMLi.Attributes.Add(ClassString, selectedCssClass)
            Me.ContactToolDMHidden.Value = ContactFlgOn
            Me.ContactToolDMImage.CssClass = selectedString & " ContactToolIcon"
        Else
            Me.ContactToolDMLi.Attributes.Remove(ClassString)
            Me.ContactToolDMHidden.Value = ContactFlgOff
            Me.ContactToolDMImage.CssClass = "ContactToolIcon"
        End If
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        If dmUseallFlg = 1 Then
            Me.ContactToolSMSLi.Visible = True
            Me.ContactToolEmailLi.Visible = True
            Me.ContactToolDMLi.Visible = True
            If dmUsesmsFlg = 0 Then
                Me.ContactToolSMSLi.Visible = False
            End If
            If dmUseemailFlg = 0 Then
                Me.ContactToolEmailLi.Visible = False
            End If
            If dmUsedmailFlg = 0 Then
                Me.ContactToolDMLi.Visible = False
            End If
        Else
            Me.ContactToolSMSLi.Visible = False
            Me.ContactToolEmailLi.Visible = False
            Me.ContactToolDMLi.Visible = False
        End If
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
    End Sub
#End Region
    '2012/03/08 TCS 山口 【SALES_2】性能改善 END
#Region "希望連絡方法設定"
    ''' <summary>
    ''' 希望連絡方法設定
    ''' </summary>
    ''' <param name="contactFlgTbl"></param>
    ''' <remarks></remarks>
    Private Sub _SetCustomerRelatedContactPopupContactTool(ByVal contactFlgTbl As SC3080201DataSet.SC3080201ContactFlgDataTable)

        Dim drContactFlg As SC3080201DataSet.SC3080201ContactFlgRow = CType(contactFlgTbl.Rows(0), SC3080201DataSet.SC3080201ContactFlgRow)

        Dim mobileFlg As String = drContactFlg.CONTACTMOBILEFLG
        Dim homeFlg As String = drContactFlg.CONTACTHOMEFLG
        Dim smsFlg As String = drContactFlg.CONTACTSMSFLG
        Dim emailFlg As String = drContactFlg.CONTACTEMAILFLG
        Dim dmFlg As String = drContactFlg.CONTACTDMFLG

        If mobileFlg.Equals(ContactFlgOn) OrElse _
            homeFlg.Equals(ContactFlgOn) OrElse _
            smsFlg.Equals(ContactFlgOn) OrElse _
            emailFlg.Equals(ContactFlgOn) OrElse _
            dmFlg.Equals(ContactFlgOn) Then
            '希望連絡方法が一つでも選択済み

            CustomerRelatedContactMobileLabel.Visible = False
            CustomerRelatedContactHomeLabel.Visible = False
            CustomerRelatedContactSMSLabel.Visible = False
            CustomerRelatedContactEMailLabel.Visible = False
            CustomerRelatedContactDMLabel.Visible = False
            CustomerRelatedContactTelImg.Visible = False
            CustomerRelatedContactMailImg.Visible = False

            If mobileFlg.Equals(ContactFlgOn) OrElse homeFlg.Equals(ContactFlgOn) Then
                CustomerRelatedContactTelImg.Visible = True

                If mobileFlg.Equals(ContactFlgOn) Then
                    CustomerRelatedContactTelImg.ImageUrl = ImageFilePath & CONTACTIMAGE_MOBILE
                    CustomerRelatedContactMobileLabel.Visible = True
                ElseIf homeFlg.Equals(ContactFlgOn) Then
                    CustomerRelatedContactTelImg.ImageUrl = ImageFilePath & CONTACTIMAGE_HOME
                    CustomerRelatedContactHomeLabel.Visible = True
                End If
            End If

            If smsFlg.Equals(ContactFlgOn) OrElse emailFlg.Equals(ContactFlgOn) OrElse dmFlg.Equals(ContactFlgOn) Then
                CustomerRelatedContactMailImg.Visible = True

                If smsFlg.Equals(ContactFlgOn) Then
                    CustomerRelatedContactMailImg.ImageUrl = ImageFilePath & CONTACTIMAGE_SMS
                    CustomerRelatedContactSMSLabel.Visible = True
                ElseIf emailFlg.Equals(ContactFlgOn) Then
                    CustomerRelatedContactMailImg.ImageUrl = ImageFilePath & CONTACTIMAGE_EMAIL
                    CustomerRelatedContactEMailLabel.Visible = True
                ElseIf dmFlg.Equals(ContactFlgOn) Then
                    CustomerRelatedContactMailImg.ImageUrl = ImageFilePath & CONTACTIMAGE_DM
                    CustomerRelatedContactDMLabel.Visible = True
                End If
            End If
            Me.CustomerRelatedContactSelectedNewPanel.Visible = False
            Me.CustomerRelatedContactSelectedEditPanel.Visible = True

        Else
            '希望連絡方法が選択されていない
            Me.CustomerRelatedContactSelectedNewPanel.Visible = True
            Me.CustomerRelatedContactSelectedEditPanel.Visible = False
        End If

        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        '2012/04/26 TCS 河原 HTMLエンコード対応 START
        Me.ContactHeaderRegistLinkButton.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10126))
        '2012/04/26 TCS 河原 HTMLエンコード対応 END
        'Dim selectedCssClass As String = "scNscPopUpContactSelectBtnMiddleOn"
        'Dim selectedString As String = "selected"

        'Dim mobileIconFileName As String = "scNscContactSelectIconMobile"
        'If mobileFlg.Equals(ContactFlgOn) Then
        '    Me.ContactToolMobileLi.Attributes.Add(ClassString, selectedCssClass)
        '    Me.ContactToolMobileHidden.Value = ContactFlgOn
        '    Me.ContactToolMobileImage.CssClass = selectedString & " ContactToolIcon"
        'Else
        '    Me.ContactToolMobileLi.Attributes.Remove(ClassString)
        '    Me.ContactToolMobileHidden.Value = ContactFlgOff
        '    Me.ContactToolMobileImage.CssClass = "ContactToolIcon"
        'End If

        'Dim homeIconFileName As String = "scNscContactSelectIconTel"
        'If homeFlg.Equals(ContactFlgOn) Then
        '    Me.ContactToolTelLi.Attributes.Add(ClassString, selectedCssClass)
        '    Me.ContactToolTelHidden.Value = ContactFlgOn
        '    Me.ContactToolTelImage.CssClass = selectedString & " ContactToolIcon"
        'Else
        '    Me.ContactToolTelLi.Attributes.Remove(ClassString)
        '    Me.ContactToolTelHidden.Value = ContactFlgOff
        '    Me.ContactToolTelImage.CssClass = "ContactToolIcon"
        'End If

        'Dim smsIconFileName As String = "scNscContactSelectIconSMS"
        'If smsFlg.Equals(ContactFlgOn) Then
        '    Me.ContactToolSMSLi.Attributes.Add(ClassString, selectedCssClass)
        '    Me.ContactToolSMSHidden.Value = ContactFlgOn
        '    Me.ContactToolSMSImage.CssClass = selectedString & " ContactToolIcon"
        'Else
        '    Me.ContactToolSMSLi.Attributes.Remove(ClassString)
        '    Me.ContactToolSMSHidden.Value = ContactFlgOff
        '    Me.ContactToolSMSImage.CssClass = "ContactToolIcon"
        'End If

        'Dim emailIconFileName As String = "scNscContactSelectIconEmail"
        'If emailFlg.Equals(ContactFlgOn) Then
        '    Me.ContactToolEmailLi.Attributes.Add(ClassString, selectedCssClass)
        '    Me.ContactToolEmailHidden.Value = ContactFlgOn
        '    Me.ContactToolEmailImage.CssClass = selectedString & " ContactToolIcon"
        'Else
        '    Me.ContactToolEmailLi.Attributes.Remove(ClassString)
        '    Me.ContactToolEmailHidden.Value = ContactFlgOff
        '    Me.ContactToolEmailImage.CssClass = "ContactToolIcon"
        'End If

        'Dim dmIconFileName As String = "scNscContactSelectIconDM"
        'If dmFlg.Equals(ContactFlgOn) Then
        '    Me.ContactToolDMLi.Attributes.Add(ClassString, selectedCssClass)
        '    Me.ContactToolDMHidden.Value = ContactFlgOn
        '    Me.ContactToolDMImage.CssClass = selectedString & " ContactToolIcon"
        'Else
        '    Me.ContactToolDMLi.Attributes.Remove(ClassString)
        '    Me.ContactToolDMHidden.Value = ContactFlgOff
        '    Me.ContactToolDMImage.CssClass = "ContactToolIcon"
        'End If
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END


    End Sub
#End Region

#Region "希望連絡曜日設定"
    ''' <summary>
    ''' 希望連絡曜日設定
    ''' </summary>
    ''' <param name="monFlg"></param>
    ''' <param name="tueFlg"></param>
    ''' <param name="wedFlg"></param>
    ''' <param name="turFlg"></param>
    ''' <param name="friFlg"></param>
    ''' <param name="satFlg"></param>
    ''' <param name="sunFlg"></param>
    ''' <param name="timeZoneClass"></param>
    ''' <remarks></remarks>
    Private Sub _SetCustomerRelatedContactPopupWeekOfDay(ByVal monFlg As String, _
                                                         ByVal tueFlg As String, _
                                                         ByVal wedFlg As String, _
                                                         ByVal turFlg As String, _
                                                         ByVal friFlg As String, _
                                                         ByVal satFlg As String, _
                                                         ByVal sunFlg As String, _
                                                         ByVal timeZoneClass As Integer)

        Dim selectedCssClass As String = "scNscPopUpDaySelectBtnSmallOn"
        Dim selectedString As String = "selected"
        Dim timeZone As String = CStr(timeZoneClass)

        If monFlg.Equals(ContactFlgOn) Then
            CType(Me.FindControl("ContactWeek" & timeZone & "MonLi"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
            CType(Me.FindControl("ContactWeek" & timeZone & "MonHidden"), HiddenField).Value = ContactFlgOn
        Else

            CType(Me.FindControl("ContactWeek" & timeZone & "MonLi"), HtmlGenericControl).Attributes.Remove(ClassString)
            CType(Me.FindControl("ContactWeek" & timeZone & "MonHidden"), HiddenField).Value = ContactFlgOff
        End If

        If tueFlg.Equals(ContactFlgOn) Then
            CType(Me.FindControl("ContactWeek" & timeZone & "TueLi"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
            CType(Me.FindControl("ContactWeek" & timeZone & "TueHidden"), HiddenField).Value = ContactFlgOn
        Else

            CType(Me.FindControl("ContactWeek" & timeZone & "TueLi"), HtmlGenericControl).Attributes.Remove(ClassString)
            CType(Me.FindControl("ContactWeek" & timeZone & "TueHidden"), HiddenField).Value = ContactFlgOff
        End If

        If wedFlg.Equals(ContactFlgOn) Then
            CType(Me.FindControl("ContactWeek" & timeZone & "WedLi"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
            CType(Me.FindControl("ContactWeek" & timeZone & "WedHidden"), HiddenField).Value = ContactFlgOn
        Else

            CType(Me.FindControl("ContactWeek" & timeZone & "WedLi"), HtmlGenericControl).Attributes.Remove(ClassString)
            CType(Me.FindControl("ContactWeek" & timeZone & "WedHidden"), HiddenField).Value = ContactFlgOff
        End If

        If turFlg.Equals(ContactFlgOn) Then
            CType(Me.FindControl("ContactWeek" & timeZone & "TurLi"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
            CType(Me.FindControl("ContactWeek" & timeZone & "TurHidden"), HiddenField).Value = ContactFlgOn
        Else

            CType(Me.FindControl("ContactWeek" & timeZone & "TurLi"), HtmlGenericControl).Attributes.Remove(ClassString)
            CType(Me.FindControl("ContactWeek" & timeZone & "TurHidden"), HiddenField).Value = ContactFlgOff
        End If

        If friFlg.Equals(ContactFlgOn) Then
            CType(Me.FindControl("ContactWeek" & timeZone & "FriLi"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
            CType(Me.FindControl("ContactWeek" & timeZone & "FriHidden"), HiddenField).Value = ContactFlgOn
        Else

            CType(Me.FindControl("ContactWeek" & timeZone & "FriLi"), HtmlGenericControl).Attributes.Remove(ClassString)
            CType(Me.FindControl("ContactWeek" & timeZone & "FriHidden"), HiddenField).Value = ContactFlgOff
        End If

        If satFlg.Equals(ContactFlgOn) Then
            CType(Me.FindControl("ContactWeek" & timeZone & "SatLi"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
            CType(Me.FindControl("ContactWeek" & timeZone & "SatHidden"), HiddenField).Value = ContactFlgOn
        Else

            CType(Me.FindControl("ContactWeek" & timeZone & "SatLi"), HtmlGenericControl).Attributes.Remove(ClassString)
            CType(Me.FindControl("ContactWeek" & timeZone & "SatHidden"), HiddenField).Value = ContactFlgOff
        End If

        If sunFlg.Equals(ContactFlgOn) Then
            CType(Me.FindControl("ContactWeek" & timeZone & "SunLi"), HtmlGenericControl).Attributes.Add(ClassString, selectedCssClass)
            CType(Me.FindControl("ContactWeek" & timeZone & "SunHidden"), HiddenField).Value = ContactFlgOn
        Else

            CType(Me.FindControl("ContactWeek" & timeZone & "SunLi"), HtmlGenericControl).Attributes.Remove(ClassString)
            CType(Me.FindControl("ContactWeek" & timeZone & "SunHidden"), HiddenField).Value = ContactFlgOff
        End If


    End Sub
#End Region

#Region "希望連絡曜日、時間設定"
    ''' <summary>
    ''' 希望連絡曜日、時間設定
    ''' </summary>
    ''' <param name="timeZoneDataTbl"></param>
    ''' <param name="weekOfDayDataTbl"></param>
    ''' <param name="timeZoneClass"></param>
    ''' <remarks></remarks>
    Private Sub _SetCustomerRelatedContactPopup(ByVal timeZoneDataTbl As SC3080201DataSet.SC3080201ContactTimeZoneDataTable, _
                                                ByVal weekOfDayDataTbl As SC3080201DataSet.SC3080201ContactWeekOfDayDataTable, _
                                                ByVal timeZoneClass As Integer)

        If weekOfDayDataTbl.Rows.Count = 1 Then
            Dim drWeekOfDayDataTbl As SC3080201DataSet.SC3080201ContactWeekOfDayRow = CType(weekOfDayDataTbl.Rows(0), SC3080201DataSet.SC3080201ContactWeekOfDayRow)
            Me._SetCustomerRelatedContactPopupWeekOfDay(drWeekOfDayDataTbl.MONDAY, _
                                                        drWeekOfDayDataTbl.TUESWDAY, _
                                                        drWeekOfDayDataTbl.WEDNESDAY, _
                                                        drWeekOfDayDataTbl.THURSDAY, _
                                                        drWeekOfDayDataTbl.FRIDAY, _
                                                        drWeekOfDayDataTbl.SATURDAY, _
                                                        drWeekOfDayDataTbl.SUNDAY, _
                                                        timeZoneClass)
        Else
            CType(Me.FindControl("ContactWeek" & timeZoneClass & "MonHidden"), HiddenField).Value = ContactFlgOff
            CType(Me.FindControl("ContactWeek" & timeZoneClass & "TueHidden"), HiddenField).Value = ContactFlgOff
            CType(Me.FindControl("ContactWeek" & timeZoneClass & "WedHidden"), HiddenField).Value = ContactFlgOff
            CType(Me.FindControl("ContactWeek" & timeZoneClass & "TurHidden"), HiddenField).Value = ContactFlgOff
            CType(Me.FindControl("ContactWeek" & timeZoneClass & "FriHidden"), HiddenField).Value = ContactFlgOff
            CType(Me.FindControl("ContactWeek" & timeZoneClass & "SatHidden"), HiddenField).Value = ContactFlgOff
            CType(Me.FindControl("ContactWeek" & timeZoneClass & "SunHidden"), HiddenField).Value = ContactFlgOff
        End If
        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        'Me.ContactHeaderRegistLinkButton.Text = WebWordUtility.GetWord(10126)
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END
        Me.ContactErrMsg.Value = WebWordUtility.GetWord(10908)

        '希望連絡曜日設定
        CType(Me.FindControl("ContactTime" & CStr(timeZoneClass) & "Count"), HiddenField).Value = CStr(timeZoneDataTbl.Rows.Count)
        CType(Me.FindControl("ContactTime" & CStr(timeZoneClass) & "Repeater"), Repeater).DataSource = timeZoneDataTbl
        CType(Me.FindControl("ContactTime" & CStr(timeZoneClass) & "Repeater"), Repeater).DataBind()

    End Sub

    Private Sub ContactTime1Repeater_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles ContactTime1Repeater.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dr As SC3080201DataSet.SC3080201ContactTimeZoneRow = _
                CType(CType(e.Item.DataItem, DataRowView).Row, SC3080201DataSet.SC3080201ContactTimeZoneRow)

            CType(e.Item.FindControl("ContactTime1Label_Row"), CustomLabel).Text = HttpUtility.HtmlEncode(dr.CONTACTTIMEZONETITLE)

            With CType(e.Item.FindControl("ContactTime1Li_Row"), HtmlGenericControl)
                .Attributes.Item("onclick") = "selectContactTime(1," & e.Item.ItemIndex & ");"
                CType(e.Item.FindControl("ContactTimeZoneNo1Hidden_Row"), HiddenField).Value = CStr(dr.CONTACTTIMEZONENO)
                If dr.CONTACTTIMEZONESELECT.Equals("1") Then
                    .Attributes.Item("class") = "scNscPopUpContactSelectBtnMiddleOn"
                    CType(e.Item.FindControl("ContactTime1Hidden_Row"), HiddenField).Value = ContactFlgOn
                Else
                    CType(e.Item.FindControl("ContactTime1Hidden_Row"), HiddenField).Value = ContactFlgOff
                End If
            End With

        End If

    End Sub

    Private Sub ContactTime2Repeater_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs) Handles ContactTime2Repeater.ItemDataBound

        If (e.Item.ItemType = ListItemType.Item OrElse e.Item.ItemType = ListItemType.AlternatingItem) Then

            Dim dr As SC3080201DataSet.SC3080201ContactTimeZoneRow = _
                CType(CType(e.Item.DataItem, DataRowView).Row, SC3080201DataSet.SC3080201ContactTimeZoneRow)

            CType(e.Item.FindControl("ContactTime2Label_Row"), CustomLabel).Text = HttpUtility.HtmlEncode(dr.CONTACTTIMEZONETITLE)

            With CType(e.Item.FindControl("ContactTime2Li_Row"), HtmlGenericControl)
                .Attributes.Item("onclick") = "selectContactTime(2," & e.Item.ItemIndex & ");"
                CType(e.Item.FindControl("ContactTimeZoneNo2Hidden_Row"), HiddenField).Value = CStr(dr.CONTACTTIMEZONENO)
                If dr.CONTACTTIMEZONESELECT.Equals("1") Then
                    .Attributes.Item("class") = "scNscPopUpContactSelectBtnMiddleOn"
                    CType(e.Item.FindControl("ContactTime2Hidden_Row"), HiddenField).Value = ContactFlgOn
                Else
                    CType(e.Item.FindControl("ContactTime2Hidden_Row"), HiddenField).Value = ContactFlgOff
                End If
            End With

        End If

    End Sub
#End Region

#Region "希望連絡方法登録押下イベント"
    Protected Sub ContactHeaderRegistLinkButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ContactHeaderRegistLinkButton.Click
        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        'ポップアップを囲うPanelをVisible=falseに
        Me.ContactVisiblePanel.Visible = False
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END

        '登録処理
        Me.RegistCustomerRelatedContact()
        'ポップアップ再表示
        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        'Me.CustomerRelatedContactLoad()
        SetContactButtonArea(Me._SetParameters())
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END

    End Sub

    ''' <summary>
    ''' 希望コンタクト方法ポップアップ再表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CustomerRelatedContactLoad()
        Dim params As SC3080201DataSet.SC3080201ParameterDataTable = _SetParameters()

        '希望コンタクト方法取得
        Dim contactFlg As SC3080201DataSet.SC3080201ContactFlgDataTable _
            = SC3080201BusinessLogic.GetContactFlg(params)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Dim contactSetflg As String = SC3080201BusinessLogic.GetContactSetFlg(params)
        Me._SetCustomerRelatedContactPopupContactTool(contactFlg)
        SetValue(ScreenPos.Current, SESSION_KEY_CONTACTROWVERSION, contactFlg.Rows(0).Item("ROW_LOCK_VERSION"))
        '2012/03/08 TCS 山口 【SALES_2】性能改善 START
        Me._SetCustomerRelatedHobbyPopUp(contactFlg, contactSetflg)
        '2012/03/08 TCS 山口 【SALES_2】性能改善 END
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        'TIMEZONECLASS分ループ
        For i = TIMEZONECLASS_1 To TIMEZONECLASS_2
            params.Rows(0).Item(params.TIMEZONECLASSColumn.ColumnName) = i

            '希望連絡時間帯取得
            Dim timeZoneDataTbl As SC3080201DataSet.SC3080201ContactTimeZoneDataTable _
                = SC3080201BusinessLogic.GetTimeZoneData(params)

            '希望連絡曜日取得
            Dim weekOfDayDataTbl As SC3080201DataSet.SC3080201ContactWeekOfDayDataTable _
                = SC3080201BusinessLogic.GetWeekOfDayData(params)
            'ポップアップ画面設定
            Me._SetCustomerRelatedContactPopup(timeZoneDataTbl, weekOfDayDataTbl, i)
        Next
    End Sub

#End Region

#Region "希望連絡方法登録処理"
    Private Sub RegistCustomerRelatedContact()

        Using dtInfo As New SC3080201DataSet.SC3080201InsertCstContactInfoDataTable, _
              dtWeekOfDay As New SC3080201DataSet.SC3080201InsertCstContactInfoDataTable, _
              dtTime As New SC3080201DataSet.SC3080201InsertCstContactInfoDataTable

            Dim drInfo As SC3080201DataSet.SC3080201InsertCstContactInfoRow = dtInfo.NewSC3080201InsertCstContactInfoRow

            '基本情報
            drInfo.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
            drInfo.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)

            '2013/06/30 TCS 庄 2013/10対応版　既存流用 START 
            drInfo.ROWLOCKVERSION = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CONTACTROWVERSION, False), Long)
            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

            '連絡方法
            drInfo.CONTACTMOBILEFLG = ContactToolMobileHidden.Value
            drInfo.CONTACTHOMEFLG = ContactToolTelHidden.Value
            drInfo.CONTACTSMSFLG = ContactToolSMSHidden.Value
            drInfo.CONTACTEMAILFLG = ContactToolEmailHidden.Value
            drInfo.CONTACTDMFLG = ContactToolDMHidden.Value

            dtInfo.Rows.Add(drInfo)
            '希望コンタクト方法登録処理
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
            Dim bizClass As New SC3080201BusinessLogic
            If (Not bizClass.InsertCstContactInfo(dtInfo)) Then
                Call ShowMessageBox(901)
            End If
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            '時間帯クラスの数だけループ
            For i As Integer = TIMEZONECLASS_1 To TIMEZONECLASS_2

                Dim drWeekOfDay As SC3080201DataSet.SC3080201InsertCstContactInfoRow = dtWeekOfDay.NewSC3080201InsertCstContactInfoRow
                '基本情報
                drWeekOfDay.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                drWeekOfDay.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                drWeekOfDay.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                '時間帯クラス
                drWeekOfDay.TIMEZONECLASS = i
                '連絡曜日
                drWeekOfDay.MONDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "MonHidden"), HiddenField).Value
                drWeekOfDay.TUESWDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "TueHidden"), HiddenField).Value
                drWeekOfDay.WEDNESDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "WedHidden"), HiddenField).Value
                drWeekOfDay.THURSDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "TurHidden"), HiddenField).Value
                drWeekOfDay.FRIDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "FriHidden"), HiddenField).Value
                drWeekOfDay.SATURDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "SatHidden"), HiddenField).Value
                drWeekOfDay.SUNDAY = CType(Me.FindControl("ContactWeek" & CStr(i) & "SunHidden"), HiddenField).Value

                dtWeekOfDay.Rows.Add(drWeekOfDay)

                '希望連絡時間件数だけループ
                Dim count As Integer = CInt(CType(Me.FindControl("ContactTime" & CStr(i) & "Count"), HiddenField).Value)
                For j As Integer = 0 To count - 1

                    Dim drTimeInsert As SC3080201DataSet.SC3080201InsertCstContactInfoRow = dtTime.NewSC3080201InsertCstContactInfoRow
                    '時間帯クラス
                    drTimeInsert.TIMEZONECLASS = i
                    '基本情報
                    drTimeInsert.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                    drTimeInsert.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                    drTimeInsert.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)

                    '連絡時間帯
                    Select Case i
                        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
                        Case TIMEZONECLASS_1
                            If String.Equals(CType(ContactTime1Repeater.Items(j).FindControl("ContactTime1Hidden_Row"), HiddenField).Value, ContactFlgOn) Then
                                drTimeInsert.CONTACTTIMEZONENO = CType(ContactTime1Repeater.Items(j).FindControl("ContactTimeZoneNo1Hidden_Row"), HiddenField).Value
                                dtTime.Rows.Add(drTimeInsert)
                            End If
                        Case TIMEZONECLASS_2
                            If String.Equals(CType(ContactTime2Repeater.Items(j).FindControl("ContactTime2Hidden_Row"), HiddenField).Value, ContactFlgOn) Then
                                drTimeInsert.CONTACTTIMEZONENO = CType(ContactTime2Repeater.Items(j).FindControl("ContactTimeZoneNo2Hidden_Row"), HiddenField).Value
                                dtTime.Rows.Add(drTimeInsert)
                            End If
                            '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
                    End Select
                Next j
            Next i
            '希望連絡曜日登録処理
            bizClass.InsertCstContactWeekOfDay(dtWeekOfDay)
            '希望連絡時間登録処理
            If dtTime.Rows.Count = 0 Then
                '削除のみ行う
                Dim drTimeDelete As SC3080201DataSet.SC3080201InsertCstContactInfoRow = dtTime.NewSC3080201InsertCstContactInfoRow
                '基本情報
                drTimeDelete.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
                drTimeDelete.CUSTOMERCLASS = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CUSTOMERCLASS, False), String)
                drTimeDelete.CRCUSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
                dtTime.Rows.Add(drTimeDelete)
                bizClass.InsertCstContactTime(dtTime)
            Else
                bizClass.InsertCstContactTime(dtTime)
            End If
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START 
            Me.CustomerDLRLockVersion.Value = CStr(CLng(Me.CustomerDLRLockVersion.Value) + 1)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        End Using
    End Sub
#End Region

    '2012/03/08 TCS 山口 【SALES_2】性能改善 START
    '#Region "希望連絡方法キャンセル押下処理"
    '    Protected Sub CustomerRelatedContactPopupCancelButton_Click(sender As Object, e As System.EventArgs) Handles CustomerRelatedContactPopupCancelButton.Click
    '        'ポップアップ再表示
    '        Me.CustomerRelatedContacLoad()
    '    End Sub
    '#End Region
    '2012/03/08 TCS 山口 【SALES_2】性能改善 END

#End Region

#Region "画面設定 重要連絡"

    ''' <summary>
    ''' ページ読み込みの処理を実施します。
    ''' </summary>
    ''' <param name="importantContactTbl"></param>
    ''' <History>2012/04/12 TCS 安田 【SALES_2】来店から車両登録が不可（ユーザー課題 No.49）
    '''          2012/04/12 TCS 安田 【SALES_2】重要連絡の”接待日”と”日付”の間に-を入れる（ユーザー課題 No.37）
    ''' </History>
    Private Sub _SetControlImportantContact(ByVal importantContactTbl As SC3080201DataSet.SC3080201ImportantContactDataTable)
        Logger.Info("_SetControlImportantContact Start")

        If importantContactTbl.Rows.Count >= 1 Then
            '重要事項表示ON
            Dim importantContactRow As SC3080201DataSet.SC3080201ImportantContactRow
            importantContactRow = CType(importantContactTbl.Rows(0), SC3080201DataSet.SC3080201ImportantContactRow)

            '受付日時
            If importantContactRow.IsRCP_DATENull Then
                ReceptionDateLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10179) & NOTEXT)
            Else

                '2012/04/12 TCS 安田 【SALES_2】重要連絡の”接待日”と”日付”の間に-を入れる（ユーザー課題 No.37） START
                'ReceptionDateLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10179) & DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, importantContactRow.RCP_DATE, StaffContext.Current.DlrCD))
                ReceptionDateLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10179) & ": " & DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, importantContactRow.RCP_DATE, StaffContext.Current.DlrCD))
                '2012/04/12 TCS 安田 【SALES_2】重要連絡の”接待日”と”日付”の間に-を入れる（ユーザー課題 No.37） END
            End If
            '苦情／重要度／カテゴリー
            ComplaintCategoryLabel.Text = HttpUtility.HtmlEncode(importantContactRow.CLMCATEGORY.Replace("%1", WebWordUtility.GetWord(10180)).Replace("%2", WebWordUtility.GetWord(10185)))
            '苦情概要
            If importantContactRow.IsCOMPLAINT_OVERVIEWNull Then
                ComplaintOverviewLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                ComplaintOverviewLabel.Text = HttpUtility.HtmlEncode(importantContactRow.COMPLAINT_OVERVIEW)
            End If

            '苦情内容
            If importantContactRow.IsCOMPLAINT_DETAILNull Then
                ComplaintDetailLabel.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                ComplaintDetailLabel.Text = HttpUtility.HtmlEncode(importantContactRow.COMPLAINT_DETAIL)
            End If
            'ステータス
            Select Case importantContactRow.STATUS.ToString
                Case CRACTSTATUS_RESPONSE
                    '1次対応中
                    ComplaintStatusLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10181))
                Case CRACTSTATUS_LAST_RESPONSE
                    '最終対応中
                    ComplaintStatusLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10182))
                Case CRACTSTATUS_END
                    '完了
                    ComplaintStatusLabel.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10183))
                Case Else
            End Select
            '担当者名
            If importantContactRow.IsUSERNAMENull Then
                ComplaintAccount.Text = HttpUtility.HtmlEncode(NOTEXT)
            Else
                ComplaintAccount.Text = HttpUtility.HtmlEncode(importantContactRow.USERNAME)
            End If
            '担当者（アイコン）
            ComplaintAccountImg.ImageUrl = Me.ResolveClientUrl(OPERATIONCODE_IMAGE_PATH & importantContactRow.ICON_IMGFILE)
            '重要連絡表示フラグON
            ImportantContactArea.Visible = True
            'CostomRepeater高さ調節
            ContactHistoryRepeater.Height = 404
            ContactHistoryListArea.Attributes.Add("style", "height:419px;")
            'ContactHistoryListBox.Attributes.Add("style", "height:404px;")
        Else
            '重要事項表示OFF
            '重要連絡表示フラグOFF
            ImportantContactArea.Visible = False
            'CostomRepeater高さ調節
            ContactHistoryRepeater.Height = 500
            ContactHistoryListArea.Attributes.Add("style", "height:516px;")
            'ContactHistoryListBox.Attributes.Add("style", "height:500px;")

        End If
        Logger.Info("_SetControlImportantContact End")
    End Sub
#End Region

#Region "画面設定 コンタクト履歴"
    '2012/02/15 TCS 山口 【SALES_2】 START
    ''' <summary>
    ''' コンタクト履歴の表示イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ContactHistoryRepeater_ClientCallback(ByVal sender As Object, ByVal e As Toyota.eCRB.SystemFrameworks.Web.Controls.ClientCallbackEventArgs) Handles ContactHistoryRepeater.ClientCallback
        Logger.Info("ContactHistoryRepeater_ClientCallback Start")

        '検索条件 Hidden、パラメータ
        Dim tabIndexHdn As String = ContactHistoryTabIndex.Value
        Dim tabIndexPrm As String = CStr(e.Arguments("criteria"))
        Dim tabIndex As String = String.Empty

        If String.IsNullOrEmpty(tabIndexPrm) Then
            'パラメータがない場合、Hidden値を検索条件に
            tabIndex = tabIndexHdn
        Else
            'パラメータがある場合、パラメータ値を検索条件に
            tabIndex = tabIndexPrm
            'Hidden値にパラメータ値をセット
            tabIndexHdn = tabIndexPrm
        End If

        Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用
        'コンタクト履歴取得

        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        Dim vin As String = String.Empty
        If Me.ContainsKey(ScreenPos.Current, SESSION_KEY_SELECT_VCLID) Then
            vin = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, False), String)
        End If
        vin = Me.customerCarsSelectedHiddenField.Value
        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
        Dim contactHistoryTbl As ActivityInfoDataSet.ActivityInfoContactHistoryDataTable _
            = SC3080201BusinessLogic.GetContactHistoryData(params, tabIndex, vin)

        ContactHistoryCountHidden.Value = CStr(contactHistoryTbl.Rows.Count)

        '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 START
        Dim actidList As New List(Of Decimal)
        Dim afterOdrFllwSeqList As New List(Of Decimal)

        '2014/11/21 TCS 河原 TMT B案 START
        For Each dr As ActivityInfoDataSet.ActivityInfoContactHistoryRow In contactHistoryTbl
            If Not dr.IsACT_IDNull Then
                If dr.ACT_ID > 0 AndAlso actidList.Contains(dr.ACT_ID) = False Then
                    '活動ID
                    actidList.Add(dr.ACT_ID)
                ElseIf dr.AFTER_ODR_FLLW_SEQ > 0 AndAlso afterOdrFllwSeqList.Contains(dr.AFTER_ODR_FLLW_SEQ) = False Then
                    '受注後工程フォロー結果連番
                    afterOdrFllwSeqList.Add(dr.AFTER_ODR_FLLW_SEQ)
                End If
            End If
        Next
        '2014/11/21 TCS 河原 TMT B案 END

        '受注後活動名称取得
        Dim afterOdrActNameTbl As ActivityInfoDataSet.ActivityInfoContactAfterOdrActDataTable _
            = ActivityInfoBusinessLogic.GetContactAfterOdrAct(actidList, afterOdrFllwSeqList)

        '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 END

        Dim beginRowIndex As Integer = 0
        If (Integer.TryParse(CType(e.Arguments("beginRowIndex"), String), beginRowIndex)) Then

            Dim sbRows As New StringBuilder(2000)
            Dim blnFirstElement As Boolean = True
            Dim sc As StaffContext = StaffContext.Current

            If contactHistoryTbl IsNot Nothing Then
                For i As Integer = beginRowIndex To (beginRowIndex + ContactHistoryRepeater.MaxCacheRows - 1)
                    If i >= contactHistoryTbl.Rows.Count Then
                        Exit For
                    End If

                    '２個目以降の要素には、先頭にカンマを付加する
                    If blnFirstElement Then
                        blnFirstElement = False
                    Else
                        sbRows.Append(",")
                    End If

                    '取得したデータを編集
                    '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 START
                    Dim editContactHistoryRow As ActivityInfoDataSet.ActivityInfoContactHistoryRow = EditContactHistory(contactHistoryTbl.Item(i), sc, afterOdrActNameTbl)
                    '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 END

                    'パラメータ編集
                    sbRows = EditContactHistoryData(editContactHistoryRow, i, sbRows)

                Next

                'ここで設定する値を使用して、aspx内のjavascriptでHTMLを動的に生成する
                e.Results("@rows") = "[" & sbRows.ToString() & "]"
            End If
        Else
            e.Results("@rows") = "[]"
        End If
        Logger.Info("ContactHistoryRepeater_ClientCallback End")
    End Sub

    ''' <summary>
    ''' コンタクト履歴編集用のパラメータ編集
    ''' </summary>
    ''' <param name="editContactHistoryRow"></param>
    ''' <param name="i"></param>
    ''' <param name="sbRows"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function EditContactHistoryData(ByVal editContactHistoryRow As ActivityInfoDataSet.ActivityInfoContactHistoryRow, _
                                            ByVal i As Integer, ByVal sbRows As StringBuilder) As StringBuilder
        Logger.Info("EditContactHistoryData Start")

        Dim actualKindImg As String = editContactHistoryRow.ACTUALKINDIMG '活動種類アイコン
        Dim actualDateString As String = editContactHistoryRow.ACTUALDATESTRING '活動日
        Dim contact As String = editContactHistoryRow.CONTACT '活動内容
        Dim crActStatusImg As String = editContactHistoryRow.CRACTSTATUSIMG 'ステータス(アイコン)
        Dim operationCodeImg As String = editContactHistoryRow.OPERATIONCODEIMG '実施者権限(アイコン)
        Dim userName As String = editContactHistoryRow.USERNAME '実施者名
        Dim complaintOverview As String = editContactHistoryRow.COMPLAINT_OVERVIEW '苦情概要
        Dim actualDetail As String = editContactHistoryRow.ACTUAL_DETAIL '苦情対応内容
        Dim memo As String = editContactHistoryRow.MEMO '苦情メモ
        Dim actualKind As String = editContactHistoryRow.ACTUALKIND '活動種類

        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        Dim mileage As String = Nothing
        Dim infomation As String = Nothing
        Dim mainteamount As String = Nothing
        Dim vclregno As String = Nothing
        Dim menteinfo As String = Nothing
        Dim wkString As String = Nothing

        If editContactHistoryRow.ACTUALKIND = "2" Then
            mileage = editContactHistoryRow.MILEAGE
            infomation = editContactHistoryRow.DLRNICNM_LOCAL
            mainteamount = editContactHistoryRow.MAINTEAMOUNT
            vclregno = editContactHistoryRow.VCLREGNO

            Dim originalid As String = editContactHistoryRow.ORIGINALID
            Dim vin As String = editContactHistoryRow.VIN
            Dim dlrcd As String = editContactHistoryRow.DLRCD
            Dim mileageseq As Long = editContactHistoryRow.MILEAGESEQ
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            Dim serviceInInfoTbl As ActivityInfoDataSet.ActivityInfoServiceInInfoDataTable _
                = ActivityInfoBusinessLogic.GetServiceInInfo(originalid, vin, dlrcd)
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
            Dim serviceInInfoRow As ActivityInfoDataSet.ActivityInfoServiceInInfoRow

            '取得結果があれば
            If Not IsNothing(serviceInInfoTbl) Then
                menteinfo = NOTEXT
                For j As Integer = 0 To serviceInInfoTbl.Count - 1
                    serviceInInfoRow = serviceInInfoTbl.Item(j)
                    wkString = wkString & serviceInInfoRow.INSPECTNM & "||"
                    wkString = wkString & serviceInInfoRow.SERVICECD & "||"
                    wkString = wkString & serviceInInfoRow.SV_PR & "||"
                    wkString = wkString & serviceInInfoRow.SERVICENAME & "||"
                    wkString = wkString & serviceInInfoRow.INSPECSEQ & "|||"
                    If Not String.IsNullOrEmpty(Trim(serviceInInfoRow.SERVICECD)) Then
                        menteinfo = serviceInInfoRow.SERVICENAME
                    End If
                Next
            End If
        End If

        Return sbRows.AppendFormat(CultureInfo.InvariantCulture(), _
                                   "{{ ""NO"" : {0}, " & _
                                   """ACTUALKINDIMG"" : ""{1}""," & _
                                   """ACTUALDATESTRING"" : ""{2}""," & _
                                   """CONTACT"" : ""{3}""," & _
                                   """CRACTSTATUSIMG"" : ""{4}""," & _
                                   """OPERATIONCODEIMG"" : ""{5}""," & _
                                   """USERNAME"" : ""{6}""," & _
                                   """COMPLAINT_OVERVIEW"" : ""{7}""," & _
                                   """ACTUAL_DETAIL"" : ""{8}""," & _
                                   """MEMO"" : ""{9}""," & _
                                   """ACTUALKIND"" : ""{10}""," & _
                                   """COLORFLG"" : {11}," & _
                                   """MILEAGE"" : ""{12}""," & _
                                   """INFOMATION"" : ""{13}""," & _
                                   """MENTEINFO"" : ""{14}""," & _
                                   """MAINTEAMOUNT"" : ""{15}""," & _
                                   """VCLREGNO"" : ""{16}""," & _
                                   """SERVICEININFO"" : ""{17}""}}", _
                                   (i + 1), _
                                   HttpUtility.JavaScriptStringEncode(actualKindImg), _
                                   HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(actualDateString)), _
                                   HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(contact)), _
                                   HttpUtility.JavaScriptStringEncode(crActStatusImg), _
                                   HttpUtility.JavaScriptStringEncode(operationCodeImg), _
                                   HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(userName)), _
                                   HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(complaintOverview)), _
                                   HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(actualDetail)), _
                                   HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(memo)), _
                                   HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(actualKind)), _
                                   i Mod 2, _
                                   HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(mileage)), _
                                   HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(infomation)), _
                                   HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(menteinfo)), _
                                   HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(mainteamount)), _
                                   HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(vclregno)), _
                                   HttpUtility.JavaScriptStringEncode(HttpUtility.HtmlEncode(wkString))
                                   )
        Logger.Info("EditContactHistoryData End")
        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
    End Function

    '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 START
    ''' <summary>
    ''' 取得したコンタクト履歴を編集
    ''' </summary>
    ''' <param name="contactHistoryRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function EditContactHistory(ByVal contactHistoryRow As ActivityInfoDataSet.ActivityInfoContactHistoryRow, _
                                        ByVal sc As StaffContext, _
                                        ByVal afterOdrActName As ActivityInfoDataSet.ActivityInfoContactAfterOdrActDataTable _
                                        ) As ActivityInfoDataSet.ActivityInfoContactHistoryRow
        '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 END
        Logger.Info("EditContactHistory Start")
        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        '活動種類アイコン設定
        Select Case contactHistoryRow.ACTUALKIND
            '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 START
            Case ACTUALKIND_SALSE, ACTUALKIND_AFTER_ODR_ACT
                '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 END
                'セールス
                contactHistoryRow.ACTUALKINDIMG = Me.ResolveClientUrl(ACTUALKIND_IMAGE_SALSE)
            Case ACTUALKIND_SERVICE
                'サービス
                contactHistoryRow.ACTUALKINDIMG = Me.ResolveClientUrl(ACTUALKIND_IMAGE_SERVICE)
            Case ACTUALKIND_CR
                'CR
                contactHistoryRow.ACTUALKINDIMG = Me.ResolveClientUrl(ACTUALKIND_IMAGE_CR)
            Case Else
        End Select
        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END 

        '活動日設定
        If contactHistoryRow.IsACTUALDATENull Then
            contactHistoryRow.ACTUALDATESTRING = NOTEXT
        Else
            If contactHistoryRow.ACTUALDATE.Hour = 0 And
                contactHistoryRow.ACTUALDATE.Minute = 0 Then

                '時間指定なし
                contactHistoryRow.ACTUALDATESTRING = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, contactHistoryRow.ACTUALDATE, DateTimeFunc.Now(sc.DlrCD), sc.DlrCD, False)

            Else
                '時間指定あり
                contactHistoryRow.ACTUALDATESTRING = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, contactHistoryRow.ACTUALDATE, DateTimeFunc.Now(sc.DlrCD), sc.DlrCD)

            End If
        End If

        '活動内容設定
        If contactHistoryRow.IsCONTACTNull Then
            contactHistoryRow.CONTACT = NOTEXT
        End If

        If String.Equals(contactHistoryRow.COUNTVIEW, COUNTVIEW_YES) Then
            'カウント表示が"1"
            contactHistoryRow.CONTACT = contactHistoryRow.CONTACTCOUNT & WebWordUtility.GetWord(10156) & contactHistoryRow.CONTACT

        Else
            'カウント表示が"1"以外
            Select Case contactHistoryRow.ACTUALKIND
                Case ACTUALKIND_SALSE
                    '%3をTACTに変換
                    Dim sysEnv As New SystemEnvSetting
                    Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

                    sysEnvRow = sysEnv.GetSystemEnvSetting(CONTACT_TABLET_DMS_NAME)

                    contactHistoryRow.CONTACT = contactHistoryRow.CONTACT.Replace("%3", sysEnvRow.PARAMVALUE)
                Case ACTUALKIND_CR
                    '%1を苦情、%2を／に変換
                    contactHistoryRow.CONTACT = contactHistoryRow.CONTACT.Replace("%1", WebWordUtility.GetWord(10180)).Replace("%2", WebWordUtility.GetWord(10185))

                Case Else
            End Select
        End If

        '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 START
        '活動内容(受注後活動内容)
        '2014/11/21 TCS 河原 TMT B案 START
        If Not contactHistoryRow.IsACT_IDNull AndAlso (contactHistoryRow.ACTUALKIND = ACTUALKIND_SALSE Or contactHistoryRow.ACTUALKIND = ACTUALKIND_AFTER_ODR_ACT) _
            AndAlso (contactHistoryRow.ACT_ID > 0 Or contactHistoryRow.AFTER_ODR_FLLW_SEQ > 0) Then
            '2014/11/21 TCS 河原 TMT B案 END
            '受注時に受注後活動した、または受注後の場合

            Dim namesStart As String = WebWordUtility.GetWord(10203)
            Dim namesEnd As String = WebWordUtility.GetWord(10204)
            Dim actNameSeparator As String = WebWordUtility.GetWord(10205)
            Dim sb As New StringBuilder
            Dim isFirst As Boolean = True

            If contactHistoryRow.ACT_ID > 0 OrElse contactHistoryRow.AFTER_ODR_FLLW_SEQ > 0 Then
                For Each dr In afterOdrActName
                    '活動ID、受注後工程フォロー結果連番が一致する受注後活動名称を、取得した順に区切り文字で連結
                    If (contactHistoryRow.ACT_ID > 0 AndAlso dr.ACT_ID = contactHistoryRow.ACT_ID) _
                        OrElse (contactHistoryRow.AFTER_ODR_FLLW_SEQ > 0 AndAlso dr.AFTER_ODR_FLLW_SEQ = contactHistoryRow.AFTER_ODR_FLLW_SEQ) Then

                        If isFirst = False Then
                            sb.Append(actNameSeparator)
                        End If
                        isFirst = False
                        Dim odrActName As String = String.Empty
                        If Not dr.IsAFTER_ODR_ACT_NAMENull AndAlso Not " ".Equals(dr.AFTER_ODR_ACT_NAME) Then
                            '名称が取得できた場合
                            odrActName = dr.AFTER_ODR_ACT_NAME
                        Else
                            odrActName = NOTEXT
                        End If
                        sb.Append(odrActName)
                    End If
                Next

                If sb.Length > 0 Then
                    contactHistoryRow.CONTACT &= (namesStart & sb.ToString & namesEnd)
                End If
            End If

        End If
        '2014/02/12 TCS 高橋 受注後フォロー機能開発に向けたシステム設計 END

        'ステータス(アイコン)設定
        contactHistoryRow.CRACTSTATUSIMG = setStatus(contactHistoryRow.ACTUALKIND, contactHistoryRow.CRACTSTATUS)

        '実施者
        If contactHistoryRow.IsUSERNAMENull Then
            contactHistoryRow.USERNAME = NOTEXT
        End If

        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        '実施者権限(アイコン)
        If contactHistoryRow.IsICON_IMGFILENull Or String.IsNullOrEmpty(Trim(contactHistoryRow.ICON_IMGFILE)) Then
            contactHistoryRow.OPERATIONCODEIMG = String.Empty
        Else
            contactHistoryRow.OPERATIONCODEIMG = Me.ResolveClientUrl(OPERATIONCODE_IMAGE_PATH & contactHistoryRow.ICON_IMGFILE)
        End If
        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

        'CR
        If String.Equals(contactHistoryRow.ACTUALKIND, ACTUALKIND_CR) Then
            '苦情概要
            If contactHistoryRow.IsCOMPLAINT_OVERVIEWNull Then
                contactHistoryRow.COMPLAINT_OVERVIEW = NOTEXT
            End If
            '苦情対応内容
            If contactHistoryRow.IsACTUAL_DETAILNull Then
                contactHistoryRow.ACTUAL_DETAIL = NOTEXT
            End If

            '苦情メモ
            If contactHistoryRow.IsMEMONull Then
                contactHistoryRow.MEMO = NOTEXT
            End If
        End If

        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        If String.Equals(contactHistoryRow.ACTUALKIND, ACTUALKIND_SERVICE) Then

            '走行距離設定
            If Not contactHistoryRow.IsMILEAGENull Then
                contactHistoryRow.MILEAGE = Trim(contactHistoryRow.MILEAGE) & WebWordUtility.GetWord(10130)
            Else
                contactHistoryRow.MILEAGE = NOTEXT
            End If

            '整備価格設定
            If Not contactHistoryRow.IsMAINTEAMOUNTNull Then
                contactHistoryRow.MAINTEAMOUNT = contactHistoryRow.MAINTEAMOUNT
            Else
                contactHistoryRow.MAINTEAMOUNT = NOTEXT
            End If

            '車両登録No.設定
            If Not contactHistoryRow.IsVCLREGNONull Then
                If String.IsNullOrEmpty(Trim(contactHistoryRow.VCLREGNO)) Then
                    contactHistoryRow.VCLREGNO = NOTEXT
                End If
            Else
                contactHistoryRow.VCLREGNO = NOTEXT
            End If

            '他販売店名表示フラグ取得
            Dim sysEnv As New SystemEnvSetting
            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            sysEnvRow = sysEnv.GetSystemEnvSetting(C_PARAMKEY_DISP_OTHER_DLRNM)
            Dim disp_other_dlrnm = sysEnvRow.PARAMVALUE

            '基幹システム名取得
            Dim BasesystemNMDt As ActivityInfoDataSet.ActivityInfoBasesystemNMDataTable = ActivityInfoBusinessLogic.GetBasesystemNM()
            Dim BasesystemNMRw As ActivityInfoDataSet.ActivityInfoBasesystemNMRow = BasesystemNMDt.Item(0)
            Dim BasesystemNM = BasesystemNMRw.BASESYSTEMNM

            '他販売店名を表示するかどうか
            If String.Equals(contactHistoryRow.DLRCD, StaffContext.Current.DlrCD) Then
                '自身の販売店の場合
                If Not contactHistoryRow.IsDLRNICNM_LOCALNull Then
                    contactHistoryRow.DLRNICNM_LOCAL = BasesystemNM & " (" & Trim(contactHistoryRow.DLRNICNM_LOCAL) & ")"
                Else
                    contactHistoryRow.DLRNICNM_LOCAL = Nothing
                End If
            Else
                '他販売店の場合
                '整備費、実施スタッフは非表示
                contactHistoryRow.MAINTEAMOUNT = "***"
                contactHistoryRow.USERNAME = NOTEXT
                contactHistoryRow.OPERATIONCODEIMG = String.Empty

                If String.Equals(disp_other_dlrnm, "1") Then
                    If Not contactHistoryRow.IsDLRNICNM_LOCALNull Then
                        contactHistoryRow.DLRNICNM_LOCAL = BasesystemNM & " (" & Trim(contactHistoryRow.DLRNICNM_LOCAL) & ")"
                    Else
                        contactHistoryRow.DLRNICNM_LOCAL = Nothing
                    End If
                Else
                    contactHistoryRow.DLRNICNM_LOCAL = BasesystemNM & " (" & WebWordUtility.GetWord(10198) & ")"
                End If
            End If
        End If

        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

        Return contactHistoryRow
        Logger.Info("EditContactHistory End")
    End Function

    ''' <summary>
    ''' ステータス設定
    ''' </summary>
    ''' <param name="actualKind"></param>
    ''' <param name="crActStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function setStatus(ByVal actualKind As String, ByVal crActStatus As String) As String
        Logger.Info("setStatus Start")
        Dim crActStatusImg As String = String.Empty

        Select Case actualKind
            Case ACTUALKIND_SALSE
                'セールス
                Select Case crActStatus
                    Case CRACTSTATUS_WALK_IN
                        'Walk-in
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_WALK_IN_ICON)
                    Case CRACTSTATUS_PROSPECT
                        'Prospect
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_PROSPECT_ICON)
                    Case CRACTSTATUS_HOT
                        'Hot
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_HOT_ICON)
                    Case CRACTSTATUS_SUCCESS
                        'Success
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_SUCCESS_ICON)
                    Case CRACTSTATUS_GIVE_UP
                        'Give-up
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_GIVE_UP_ICON)
                    Case CRACTSTATUS_ALLOCATION
                        'Allocation(振当てまち)
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_ALLOCATION_ICON)
                    Case CRACTSTATUS_PAYMEN
                        'Paymen(入金待ち)
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_PAYMEN_ICON)
                    Case CRACTSTATUS_DELIVERY
                        'Delivery(納車待ち)
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_DELIVERY_ICON)
                    Case CRACTSTATUS_ALLOCATION_TACT
                        'Allocation(振当てTACT)
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_ALLOCATION_TACT_ICON)
                    Case CRACTSTATUS_PAYMEN_TACT
                        'Payment(入金TACT)
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_PAYMEN_TACT_ICON)
                    Case CRACTSTATUS_DELIVERY_TACT
                        'Delivery(納車TACT)
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_DELIVERY_TACT_ICON)
                    Case CRACTSTATUS_CANCEL
                        'キャンセル
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_CANCEL_ICON)
                    Case Else
                        crActStatusImg = String.Empty
                End Select
            Case ACTUALKIND_CR
                'CR
                Select Case crActStatus
                    Case CRACTSTATUS_RESPONSE
                        '1次対応中
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_RESPONSE_ICON)
                    Case CRACTSTATUS_LAST_RESPONSE
                        '最終対応中
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_LAST_RESPONSE_ICON)
                    Case CRACTSTATUS_END
                        '完了
                        crActStatusImg = Me.ResolveClientUrl(CRACTSTATUS_ICON_PATH & CRACTSTATUS_END_ICON)
                    Case Else
                        crActStatusImg = String.Empty
                End Select
            Case Else

        End Select

        Return crActStatusImg
        Logger.Info("setStatus End")
    End Function
    '2012/02/15 TCS 山口 【SALES_2】 END
#End Region

#Region "画面設定 メモ"
    ''' <summary>
    ''' 画面設定 メモ
    ''' </summary>
    ''' <param name="lastCustMemoTbl"></param>
    ''' <remarks></remarks>
    Private Sub ControltimeLastCustMemo(ByVal lastCustMemoTbl As SC3080201DataSet.SC3080201LastCustomerMemoDataTable)
        If lastCustMemoTbl.Rows.Count >= 1 Then
            Dim lastCustMemoDataRow As SC3080201DataSet.SC3080201LastCustomerMemoRow
            lastCustMemoDataRow = CType(lastCustMemoTbl.Rows(0), SC3080201DataSet.SC3080201LastCustomerMemoRow)

            '更新日
            CustomerMemoDayLabel.Text = HttpUtility.HtmlEncode(DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, _
                                                                       lastCustMemoDataRow.UPDATEDATE, _
                                                                       StaffContext.Current.DlrCD))
            'メモ
            CustomerMemoLabel.Text = lastCustMemoDataRow.MEMO
        Else
            '更新日
            CustomerMemoDayLabel.Text = "　"
            'メモ
            CustomerMemoLabel.Text = String.Empty
        End If

    End Sub

#End Region

#Region "顧客情報再表示"
    ''' <summary>
    ''' 顧客情報再表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub customerInfoUpdateButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles customerInfoUpdateButton.Click

        Dim biz As New SC3080201BusinessLogic
        Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters()
        Dim paramsDr As SC3080201DataSet.SC3080201ParameterRow = CType(params.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        ' 2019/05/28 TS 髙橋(龍) TR-SVT-TMT-20170725-001対応 START
        'システム設定テーブルから画像アップロードのファイル拡張子を取得
        Dim ImageFileExt = ActivityInfoBusinessLogic.GetSystemSetting(C_FILE_UPLOAD_EXTENSION)
        ' 2019/05/28 TS 髙橋(龍) TR-SVT-TMT-20170725-001対応 END

        '顔写真登録
        Using paramInsert As New SC3080201DataSet.SC3080201InsertImageFileDataTable

            Dim paramInsertDr As SC3080201DataSet.SC3080201InsertImageFileRow
            paramInsertDr = paramInsert.NewSC3080201InsertImageFileRow

            'パラメータ設定
            paramInsertDr.CRCUSTID = customerIdTextBox.Value
            '2013/06/30 TCS 未 2013/10対応版　既存流用 START
            Dim context As StaffContext = StaffContext.Current
            paramInsertDr.dlrcd = context.DlrCD     '自身の販売店コード
            '2013/06/30 TCS 未 2013/10対応版　既存流用 END
            paramInsertDr.CSTKIND = paramsDr.CSTKIND
            ' 2019/05/28 TS 髙橋(龍) TR-SVT-TMT-20170725-001対応 START
            '取得した画像アップロードのファイル拡張子と画像のファイル名を組み合わせる
            paramInsertDr.IMAGEFILE_L = ImageFileExt.Replace(ReplaceParameter, customerIdTextBox.Value & IMAGEFILE_L)
            paramInsertDr.IMAGEFILE_M = ImageFileExt.Replace(ReplaceParameter, customerIdTextBox.Value & IMAGEFILE_M)
            paramInsertDr.IMAGEFILE_S = ImageFileExt.Replace(ReplaceParameter, customerIdTextBox.Value & IMAGEFILE_S)
            'paramInsertDr.IMAGEFILE_L = customerIdTextBox.Value & IMAGEFILE_L & ImageFileExt
            'paramInsertDr.IMAGEFILE_M = customerIdTextBox.Value & IMAGEFILE_M & ImageFileExt
            'paramInsertDr.IMAGEFILE_S = customerIdTextBox.Value & IMAGEFILE_S & ImageFileExt
            ' 2019/05/28 TS 髙橋(龍) TR-SVT-TMT-20170725-001対応 END
            'paramInsertDr.IMAGEFILE_L = customerIdTextBox.Value & faceFileNameTimeHiddenField.Value & IMAGEFILE_L & ImageFileExt
            'paramInsertDr.IMAGEFILE_M = customerIdTextBox.Value & faceFileNameTimeHiddenField.Value & IMAGEFILE_M & ImageFileExt
            'paramInsertDr.IMAGEFILE_S = customerIdTextBox.Value & faceFileNameTimeHiddenField.Value & IMAGEFILE_S & ImageFileExt
            '2013/06/30 TCS 未 2013/10対応版　既存流用 START
            paramInsertDr.ROWLOCKVERSION = CLng(Me.CustomerDLRLockVersion.Value)
            '2013/06/30 TCS 未 2013/10対応版　既存流用 END
            paramInsert.Rows.Add(paramInsertDr)
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
            If (Not biz.InsertImageFile(paramInsert)) Then
                Call ShowMessageBox(901)
            End If
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

            '初期データ取得
            If String.Equals(paramsDr.CSTKIND, ORGCUSTFLG) Then
                '自社客取得
                Me._SetControlOrgCustomer(SC3080201BusinessLogic.GetOrgCustomerData(params))
            ElseIf String.Equals(paramsDr.CSTKIND, NEWCUSTFLG) Then
                '未取引客取得
                Me._SetControlNewCustomer(SC3080201BusinessLogic.GetNewCustomerData(params))
            End If

        End Using

    End Sub
#End Region

#Region "その他ポップアップ関連"

#Region "顧客編集遷移処理"

    ''' <summary>
    ''' 顧客情報再表示用 顧客情報更新時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub customerReload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles customerReload.Click
        '顧客情報取得、画面設定
        Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用
        Dim cstKind As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
        If String.Equals(cstKind, ORGCUSTFLG) Then
            '自社客取得、画面設定
            Me._ShowOrgCustomer(params)
        ElseIf String.Equals(cstKind, NEWCUSTFLG) Then
            '未取引客取得、画面設定
            Me._ShowNewCustomer(params)
        End If

    End Sub

    '$01 Redirect廃止
    ' ''' <summary>
    ' ''' 画面全体再表示用 顧客情報新規登録時
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub customerReloadAll_Click(sender As Object, e As System.EventArgs) Handles customerReloadAll.Click

    '    '顧客メモを再読み込みする
    '    RemoveValueBypass(ScreenPos.Current, SESSION_KEY_MEMO_INIT)
    '    
    '    RedirectNextScreen("SC3080201")

    'End Sub


    ' ''' <summary>
    ' ''' 次画面に遷移します。
    ' ''' </summary>
    ' ''' <param name="appId">画面ID</param>
    ' ''' <remarks></remarks>
    'Public Sub RedirectNextScreen(ByVal appId As String)

    '    '画面遷移履歴Listを取得
    '    Dim aspxFileName As String = appId & ".aspx"

    '    '画面遷移履歴を追加
    '    Dim canonicalUrl As String = Me.ResolveUrl("~/Pages/" & aspxFileName)

    '    '画面遷移
    '    Me.Response.Redirect(canonicalUrl)
    'End Sub


#End Region

#Region "車両編集遷移処理"
    ''' <summary>
    ''' 車両情報再表示用
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub customerCarReload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles customerCarReload.Click
        '車両情報取得、画面設定
        Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用
        Dim cstKind As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)

        '車両編集で編集したキー情報を保持
        Me.customerCarsSelectedHiddenField.Value = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, False), String)

        If String.Equals(cstKind, ORGCUSTFLG) Then
            '自社客車両取得
            Me._ShowOrgVehicle(params)
        ElseIf String.Equals(cstKind, NEWCUSTFLG) Then
            '未取引客車両取得
            Me._ShowNewVehicle(params)
        End If

    End Sub

#End Region

#Region "車両選択遷移処理"
    '2012/03/08 TCS 山口 【SALES_2】性能改善 START

    ''' <summary>
    ''' 車両選択ポップアップ起動処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub CustomerCarSelectPopupOpenButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerCarSelectPopupOpenButton.Click
        Logger.Info("CustomerCarSelectPopupOpenButton_Click Start")

        'ポップアップを囲うPanelをVisible=Trueに
        Me.CustomerCarVisiblePanel.Visible = True

        Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用
        Dim cstKind As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)

        If String.Equals(cstKind, ORGCUSTFLG) Then
            '自社客車両取得
            Me._ShowOrgVehiclePopup(params)
        ElseIf String.Equals(cstKind, NEWCUSTFLG) Then
            '未取引客車両取得
            Me._ShowNewVehiclePopup(params)
        End If

        'スクリプトの登録
        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "CustomerCarSelectPopUpOpenAfter", "startup")

        Logger.Info("CustomerCarSelectPopupOpenButton_Click End")
    End Sub

    ''' <summary>
    ''' 車両選択ポップアップ選択後の車両情報再表示用
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub customerCarButtonDummy_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles customerCarButtonDummy.Click

        'ポップアップを囲うPanelをVisible=Falseに
        Me.CustomerCarVisiblePanel.Visible = False

        Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用
        Dim cstKind As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)

        '選択行情報保持
        Me.SetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, CStr(Me.customerCarsSelectedHiddenField.Value))

        If String.Equals(cstKind, ORGCUSTFLG) Then
            '自社客車両取得
            Me._ShowOrgVehicle(params)
        ElseIf String.Equals(cstKind, NEWCUSTFLG) Then
            '未取引客車両取得
            Me._ShowNewVehicle(params)
        End If

    End Sub

    ' ''' <summary>
    ' ''' 車両選択ポップアップ後の車両情報再表示用
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub customerCarButtonDummy_Click(sender As Object, e As System.EventArgs) Handles customerCarButtonDummy.Click

    '    '車両情報取得、画面設定
    '    Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用
    '    Dim cstKind As String = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)

    '    '選択行情報保持
    '    Me.SetValue(ScreenPos.Current, SESSION_KEY_SELECT_VCLID, CStr(Me.customerCarsSelectedHiddenField.Value))

    '    If String.Equals(cstKind, ORGCUSTFLG) Then
    '        '自社客車両取得
    '        Me._ShowOrgVehicle(params)
    '    ElseIf String.Equals(cstKind, NEWCUSTFLG) Then
    '        '未取引客車両取得
    '        Me._ShowNewVehicle(params)
    '    End If

    '    '車両編集の初期処理
    '    Call VehicleInitialize()

    'End Sub

    '2012/03/08 TCS 山口 【SALES_2】性能改善 END
#End Region

#Region "顧客メモ遷移処理"
    '2012/03/15 TCS 安田 【SALES_2】性能改善 Add Start
    'SC3080204へ処理を移行する
    '' <summary>
    ' ''' 顧客メモオープン時の処理
    ' ''' </summary>
    ' ''' <param name="sender"></param>
    ' ''' <param name="e"></param>
    ' ''' <remarks></remarks>
    'Protected Sub CustomerMemoEditOpenButton_Click(sender As Object, e As System.EventArgs) Handles CustomerMemoEditOpenButton.Click

    '    'スクリプトで顧客メモポップアップを起動
    '    JavaScriptUtility.RegisterStartupFunctionCallScript(CType(Me.Parent.Parent.Parent.Parent.Parent, BasePage), "commitCompleteOpenCustomerMemoEdit", "after")

    'End Sub
    '2012/03/15 TCS 安田 【SALES_2】性能改善 Add End

    ''' <summary>
    ''' 顧客メモクローズ時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub CustomerMemoEditCloseButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles CustomerMemoEditCloseButton.Click
        '最新顧客メモ取得、画面設定
        Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用
        Me._ShowLastCustMemo(params)
    End Sub
#End Region

#Region "ポップアップ抑制"
    ''' <summary>
    ''' ポップアップ画面の表示を抑制する
    ''' </summary>
    ''' <remarks>
    ''' ユーザが顧客担当以外で、継続中の活動がない場合に
    ''' 顧客情報の編集等を不可にする
    ''' </remarks>
    Private Sub SetControlSate()
        'Return
        Dim staffInfo As StaffContext = StaffContext.Current
        Dim dlrcd As String = staffInfo.DlrCD       '販売店コード
        Dim strcd As String = staffInfo.BrnCD       '店舗コード
        Dim cstKind As String = ""                  '顧客種別
        Dim cstid As String = ""                    '顧客ID
        Dim account As String = staffInfo.Account   'ログインスタッフのアカウント
        Dim salesStaffAccount As String = ""        '顧客担当のアカウント

        If ContainsKey(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD) Then
            salesStaffAccount = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_SALESSTAFFCD, False), String)
        End If
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CSTKIND) Then
            cstKind = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
        End If
        If ContainsKey(ScreenPos.Current, SESSION_KEY_CRCUSTID) Then
            cstid = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
        End If

        '2013/11/27 TCS 市川 Aカード情報相互連携開発 START
        '一時対応中以外
        '2013/01/22 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
        If (Not String.Equals(StaffContext.Current.PresenceCategory, "2") Or
            (Not String.Equals(StaffContext.Current.PresenceDetail, "1") And Not String.Equals(StaffContext.Current.PresenceDetail, "3"))) And
            StaffContext.Current.OpeCD = Operation.SSF _
                AndAlso Not staffInfo.TeamLeader _
                AndAlso Toyota.eCRB.CommonUtility.BizLogic.ActivityInfoBusinessLogic.IsMyTeamMember(salesStaffAccount) Then
            '2013/01/22 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
            '2013/11/27 TCS 市川 Aカード情報相互連携開発 END
            ''2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            If String.Equals(account, salesStaffAccount) = False _
                AndAlso Not SC3080201BusinessLogic.IsExistsNotCompleteAction(cstid, account) Then
                '    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
                'ログインスタッフが顧客担当でない、かつ担当している活動で、継続中のものが存在しない場合
                '顧客編集ポップアップ以外のポップアップを実行不可にする

                Me.CustomerPhotoArea.Attributes.Item("onclick") = String.Empty              '写真選択ポップアップ抑制
                Me.P1.Attributes.Item("onclick") = String.Empty                             '顧客詳細編集ポップアップ抑制
                Me.PAddress.Attributes.Item("onclick") = String.Empty                       '地図表示抑制
                Me.CustomerCarTypeNumber.Attributes.Item("onclick") = String.Empty          '保有車両選択ポップアップ抑制
                Me.CustomerCarEditPopUpOpenEria1.Attributes.Item("onclick") = String.Empty  '車両詳細編集ポップアップ抑制
                Me.CustomerCarEditPopUpOpenEria2.Attributes.Item("onclick") = String.Empty
                Me.CustomerRelatedOccupationArea.Attributes.Item("onclick") = String.Empty  '職業編集ポップアップ抑制
                Me.CustomerRelatedFamilyArea.Attributes.Item("onclick") = String.Empty      '家族編集ポップアップ抑制
                Me.CustomerRelatedHobbyArea.Attributes.Item("onclick") = String.Empty       '趣味編集ポップアップ抑制
                Me.CustomerRelatedContactArea.Attributes.Item("onclick") = String.Empty     '接触方法編集ポップアップ抑制
                Me.CustomerMemo_Click.Attributes.Item("onclick") = String.Empty             '顧客メモ編集ポップアップ抑制

                Me.ReadOnlyFlagHidden.Value = "1"
            Else
                Me.ReadOnlyFlagHidden.Value = "0"
            End If
        Else
            Me.CustomerPhotoArea.Attributes.Item("onclick") = "photoSelectOpen();"              '写真選択ポップアップ抑制
            Me.P1.Attributes.Item("onclick") = "CustomerEditPopUpOpen();"                             '顧客詳細編集ポップアップ抑制
            Me.PAddress.Attributes.Item("onclick") = "googleMapOpen();"                       '地図表示抑制
            Me.CustomerCarTypeNumber.Attributes.Item("onclick") = "CustomerCarSelectPopUpOpen();"          '保有車両選択ポップアップ抑制
            Me.CustomerCarEditPopUpOpenEria1.Attributes.Item("onclick") = "CustomerCarEditPopUpOpen();"  '車両詳細編集ポップアップ抑制
            Me.CustomerCarEditPopUpOpenEria2.Attributes.Item("onclick") = "CustomerCarEditPopUpOpen();"
            Me.CustomerRelatedOccupationArea.Attributes.Item("onclick") = "setPopupOccupationPageOpen();"  '職業編集ポップアップ抑制
            Me.CustomerRelatedFamilyArea.Attributes.Item("onclick") = "setPopupFamilyPageOpen();"      '家族編集ポップアップ抑制
            Me.CustomerRelatedHobbyArea.Attributes.Item("onclick") = "setPopupHobbyPageOpen();"       '趣味編集ポップアップ抑制
            Me.CustomerRelatedContactArea.Attributes.Item("onclick") = "setPopupContactPageOpen();"     '接触方法編集ポップアップ抑制
            Me.CustomerMemo_Click.Attributes.Item("onclick") = "setPopupCustomerMemoOpen();"             '顧客メモ編集ポップアップ抑制
            Me.ReadOnlyFlagHidden.Value = "0"
        End If
    End Sub
#End Region

#End Region

    '2012/06/01 TCS 河原 FS開発 START
#Region "FS開発機能"

    ''' <summary>
    ''' SNSサイトのURL取得
    ''' </summary>
    ''' <remarks>SNSサイトのURL取得</remarks>
    Private Sub fsInitialize()

        '各SNSサイトのURL取得
        Dim sysenv As New SystemEnvSetting
        Dim rw As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

        'renrneの検索画面URL
        rw = sysenv.GetSystemEnvSetting(SNSURL_SEARCH_RENREN)
        Snsurl_Search_Renren_Hidden.Value = rw.PARAMVALUE

        'renrenのユーザー画面URL
        rw = sysenv.GetSystemEnvSetting(SNSURL_ACCOUNT_RENREN)
        Snsurl_Account_Renren_Hidden.Value = rw.PARAMVALUE

        '2013/10/23 TCS 山田 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD START
        'SYSENVSETTINGの値が空白の場合
        If String.IsNullOrEmpty(Trim(Snsurl_Search_Renren_Hidden.Value)) _
        OrElse String.IsNullOrEmpty(Trim(Snsurl_Account_Renren_Hidden.Value)) Then
            'リンクボタンを表示しない
            Me.Icon_Renren.Visible = False
        End If
        '2013/10/23 TCS 山田 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD END

        'kaixinの検索画面URL
        rw = sysenv.GetSystemEnvSetting(SNSURL_SEARCH_KAIXIN)
        Snsurl_Search_Kaixin_Hidden.Value = rw.PARAMVALUE

        'kaixinのユーザー画面URL
        rw = sysenv.GetSystemEnvSetting(SNSURL_ACCOUNT_KAIXIN)
        Snsurl_Account_Kaixin_Hidden.Value = rw.PARAMVALUE

        '2013/10/23 TCS 山田 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD START
        'SYSENVSETTINGの値が空白の場合
        If String.IsNullOrEmpty(Trim(Snsurl_Search_Kaixin_Hidden.Value)) _
        OrElse String.IsNullOrEmpty(Trim(Snsurl_Account_Kaixin_Hidden.Value)) Then
            'リンクボタンを表示しない
            Me.Icon_Kaixin.Visible = False
        End If
        '2013/10/23 TCS 山田 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD END

        'weiboの検索画面URL
        rw = sysenv.GetSystemEnvSetting(SNSURL_SEARCH_WEIBO)
        Snsurl_Search_Weibo_Hidden.Value = rw.PARAMVALUE

        'weiboのユーザー画面URL
        rw = sysenv.GetSystemEnvSetting(SNSURL_ACCOUNT_WEIBO)
        Snsurl_Account_Weibo_Hidden.Value = rw.PARAMVALUE

        '2013/10/23 TCS 山田 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD START
        'SYSENVSETTINGの値が空白の場合
        If String.IsNullOrEmpty(Trim(Snsurl_Search_Weibo_Hidden.Value)) _
        OrElse String.IsNullOrEmpty(Trim(Snsurl_Account_Weibo_Hidden.Value)) Then
            'リンクボタンを表示しない
            Me.Icon_Weibo.Visible = False
        End If
        '2013/10/23 TCS 山田 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD END

        'baiduの検索画面
        rw = sysenv.GetSystemEnvSetting(SEARCH_BAIDU)
        Search_Baidu_Hidden.Value = rw.PARAMVALUE

        '2013/10/23 TCS 山田 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD START
        'SYSENVSETTINGの値が空白の場合
        If String.IsNullOrEmpty(Trim(Search_Baidu_Hidden.Value)) Then
            'リンクボタンを表示しない
            Me.KeywordSearch.Visible = False
        End If
        '2013/10/23 TCS 山田 次世代e-CRBセールス機能 新DB適応に向けた機能開発 ADD END

        '別ブラウザのURLスキーム
        rw = sysenv.GetSystemEnvSetting(URL_SCHEME)
        Url_Scheme_Hidden.Value = rw.PARAMVALUE

        rw = sysenv.GetSystemEnvSetting(URL_SCHEMES)
        Url_Schemes_Hidden.Value = rw.PARAMVALUE

        '登録用ポップアップのボタン文言設定
        Me.SnsIdPopUpCompleteButton.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10126))
        Me.KeywordSearchPopUpCompleteButton.Text = HttpUtility.HtmlEncode(WebWordUtility.GetWord(10126))

    End Sub

    ''' <summary>
    ''' SNSID登録処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SnsIdPopUpCompleteButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SnsIdPopUpCompleteButton.Click

        '入力チェック
        If Not String.IsNullOrEmpty(Me.SnsIdInputPopupInputText.Text) Then
            '件多数チェック
            If Not Validation.IsCorrectDigit(Me.SnsIdInputPopupInputText.Text, 128) Then
                JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "commonClearTimer", "startup")

                If Me.SnsOpenMode.Value.Equals("1") Then
                    ShowMessageBox(10919)
                ElseIf Me.SnsOpenMode.Value.Equals("2") Then
                    ShowMessageBox(10920)
                ElseIf Me.SnsOpenMode.Value.Equals("3") Then
                    ShowMessageBox(10921)
                End If

                Return
            End If

            '禁則文字チェック
            If Not Validation.IsValidString(Me.SnsIdInputPopupInputText.Text) Then
                JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "commonClearTimer", "startup")

                If Me.SnsOpenMode.Value.Equals("1") Then
                    ShowMessageBox(10923)
                ElseIf Me.SnsOpenMode.Value.Equals("2") Then
                    ShowMessageBox(10924)
                ElseIf Me.SnsOpenMode.Value.Equals("3") Then
                    ShowMessageBox(10925)
                End If

                Return
            End If
        End If

        Using dtInfo As New SC3080201DataSet.SC3080201CustSnsIdDataTable

            Dim drInfo As SC3080201DataSet.SC3080201CustSnsIdRow = dtInfo.NewSC3080201CustSnsIdRow

            '基本情報
            drInfo.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
            drInfo.CSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)
            drInfo.ROWLOCKVERSION = CLng(Me.CustomerDLRLockVersion.Value)

            'SNSID
            drInfo.MODE = Me.SnsOpenMode.Value
            drInfo.SNSID = Trim(Me.SnsIdInputPopupInputText.Text)
            If String.IsNullOrEmpty(drInfo.SNSID) Then
                drInfo.SNSID = " "    '当DB項目のデフォルト値が半角のスペース
            End If

            dtInfo.Rows.Add(drInfo)

            'SNSID登録処理
            Dim bizClass As New SC3080201BusinessLogic
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
            If (Not bizClass.UpdateSnsId(dtInfo)) Then
                Call ShowMessageBox(901)
            End If
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
            '画面内Hidden項目反映
            Me.CustomerDLRLockVersion.Value = CStr(CLng(Me.CustomerDLRLockVersion.Value) + 1)
            If Me.SnsOpenMode.Value.Equals("1") Then
                Me.Snsid_Renren_Hidden.Value = Trim(Me.SnsIdInputPopupInputText.Text)
            ElseIf Me.SnsOpenMode.Value.Equals("2") Then
                Me.Snsid_Kaixin_Hidden.Value = Trim(Me.SnsIdInputPopupInputText.Text)
            Else
                Me.Snsid_Weibo_Hidden.Value = Trim(Me.SnsIdInputPopupInputText.Text)
            End If

        End Using

        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "commonClearTimer", "startup")

    End Sub

    ''' <summary>
    ''' キーワード登録処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub KeywordSearchPopUpCompleteButton_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles KeywordSearchPopUpCompleteButton.Click

        '入力チェック
        If Not String.IsNullOrEmpty(Me.KeywordSearchInputPopupInputText.Text) Then
            '件多数チェック
            If Not Validation.IsCorrectDigit(Me.KeywordSearchInputPopupInputText.Text, 256) Then
                JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "commonClearTimer", "startup")
                ShowMessageBox(10922)
                Return
            End If

            '禁則文字チェック
            If Not Validation.IsValidString(Me.KeywordSearchInputPopupInputText.Text) Then
                JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "commonClearTimer", "startup")
                ShowMessageBox(10926)
                Return
            End If
        End If

        Using dtInfo As New SC3080201DataSet.SC3080201CustKeywordDataTable

            Dim drInfo As SC3080201DataSet.SC3080201CustKeywordRow = dtInfo.NewSC3080201CustKeywordRow

            '基本情報
            drInfo.CSTKIND = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CSTKIND, False), String)
            drInfo.CSTID = DirectCast(GetValue(ScreenPos.Current, SESSION_KEY_CRCUSTID, False), String)

            'キーワード
            drInfo.KEYWORD = Trim(Me.KeywordSearchInputPopupInputText.Text)
            '2013/06/30 TCS 未 2013/10対応版　既存流用 START
            If String.IsNullOrEmpty(drInfo.KEYWORD) Then
                drInfo.KEYWORD = " "    '当DB項目のデフォルト値が半角のスペース
            End If
            drInfo.ROWLOCKVERSION = CLng(Me.CustomerDLRLockVersion.Value)
            '2013/06/30 TCS 未 2013/10対応版　既存流用 END

            dtInfo.Rows.Add(drInfo)

            'キーワード登録処理
            Dim bizClass As New SC3080201BusinessLogic
            '2013/06/30 TCS 未 2013/10対応版　既存流用 START
            If bizClass.UpdateKeyword(dtInfo) Then
                '画面内Hidden項目反映
                Me.Keyword_Hidden.Value = Trim(Me.KeywordSearchInputPopupInputText.Text)
                Me.CustomerDLRLockVersion.Value = CStr(CLng(Me.CustomerDLRLockVersion.Value) + 1)
            Else
                Call ShowMessageBox(901)
            End If
            '2013/06/30 TCS 未 2013/10対応版　既存流用 END
        End Using

        JavaScriptUtility.RegisterStartupFunctionCallScript(Me.Page, "commonClearTimer", "startup")

    End Sub

#End Region
    '2012/06/01 TCS 河原 FS開発 END

    '2018/04/24 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START

#Region "（トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証"

    ''' <summary>
    '''個人法人区分使用可否フラグ判定
    ''' </summary>
    ''' <remarks></remarks>
    Private Function isPfTypeAvailable() As Boolean
        Dim sysenv As New SystemEnvSetting
        Dim rw As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
        rw = sysenv.GetSystemEnvSetting(PNAME_USED_FLG_PFTYPE)
        Dim ret As Boolean = USED_FLG_PFTYPE_ON.Equals(rw.PARAMVALUE)
        Return ret
    End Function

#End Region
    '2018/04/24 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

#End Region

#Region " ページクラス処理のバイパス処理 "
    Private Sub SetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal value As Object)
        GetPageInterface().SetValueBypass(pos, key, value)
    End Sub

    Private Function GetValue(ByVal pos As ScreenPos, ByVal key As String, ByVal removeFlg As Boolean) As Object
        Return GetPageInterface().GetValueBypass(pos, key, removeFlg)
    End Function

    Private Sub ShowMessageBox(ByVal wordNo As Integer, ByVal ParamArray wordParam() As String)
        GetPageInterface().ShowMessageBoxBypass(wordNo, wordParam)
    End Sub

    Private Function ContainsKey(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String) As Boolean
        Return GetPageInterface().ContainsKeyBypass(pos, key)
    End Function

    Private Sub RemoveValueBypass(ByVal pos As Toyota.eCRB.SystemFrameworks.Web.ScreenPos, ByVal key As String)
        GetPageInterface().RemoveValueBypass(pos, key)
    End Sub

    Private Function GetPageInterface() As ICustomerDetailControl
        Return CType(Me.Page, ICustomerDetailControl)
    End Function

    Public Sub RegistActivityAfter() Implements Toyota.eCRB.CustomerInfo.Details.BizLogic.ISC3080201Control.RegistActivityAfter
        'コンタクト履歴取得、画面設定
        Dim params As SC3080201DataSet.SC3080201ParameterDataTable = Me._SetParameters() '検索条件格納用
        _ShowContactHistory()
        '2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) ADD START
        '顧客メモエリアの最新化
        _ShowLastCustMemo(params)
        '2015/07/06 TCS 藤井  TR-V4-FTMS141028001(FTMS→TMTマージ) ADD END
    End Sub
#End Region

    '2020/01/20 TS 岩田 TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045) START
#Region "個人/法人区分非活性制御"
    ''' 個人/法人区分非活性制御
    ''' <param name="custid">顧客ID</param>
    ''' <param name="custype">個人/法人区分値</param>
    ''' </summary>
    ''' <remarks>個人/法人区分非活性制御</remarks>
    Private Sub setKojinHoujinEnabled(ByVal custid As String, ByVal custype As String)
        Dim dt As SC3080205DataSet.SC3080205AcardNumCountDataTable = Nothing
        Dim dlrcd As String = StaffContext.Current.DlrCD
        dt = SC3080205BusinessLogic.GetAcardNumCount(dlrcd, CStr(custid))

        If (dt(0).ACARD_ROWS_COUNT + dt(0).ACARD_HIS_ROWS_COUNT > 0) Then
            If (String.IsNullOrEmpty(custype.Trim) = False) Then
                kojinCheckBox.Enabled = False
                houjinCheckBox.Enabled = False
            End If
        End If
    End Sub
#End Region
    '2020/01/20 TS 岩田 TKM Change request development for Next Gen e-CRB (CR004,CR011,CR041,CR044,CR045) END

End Class
