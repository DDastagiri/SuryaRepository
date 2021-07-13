Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core


'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3070201TableAdapter.vb
'─────────────────────────────────────
'機能： 
'補足： 
'作成： 
'更新： 2012/07/30 TCS 高橋  兆方店展開
'更新： 2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発
'更新： 2013/01/18 TCS 上田  GL0871対応
'更新： 2013/02/04 TCS 橋本 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応
'更新： 2013/06/30 TCS 内藤 【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/06 TCS 森    Aカード情報相互連携開発
'更新： 2014/03/18 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応
'更新： 2018/05/01 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
'更新： 2018/06/15 TCS 舩橋  TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2019/02/14 TCS 河原 TKM UAT0651対応(タブレットで契約した見積のIDは論削済みでも取得するように修正)
'削除： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────

Public NotInheritable Class IC3070201TableAdapter

#Region "コンストラクタ"
    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <param name="mode">実行モード</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal mode As Integer)
        Me.Mode = mode
    End Sub
#End Region

#Region "定数"
    '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
    ''' <summary>
    ''' モデルコード　AHV41L-JEXGBC
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MODEL_CD_HV As String = "AHV41L-JEXGBC%"
    ''' <summary>
    ''' シリーズコード　CAMRY
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERIES_CODE_CAMRY As String = "CAMRY"
    ''' <summary>
    ''' シリーズコード　CMYHV
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SERIES_CODE_CMYHV As String = "CMYHV"
    '2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
#End Region

#Region "メンバー変数"
    ''' <summary>
    ''' 実行モード
    ''' </summary>
    ''' <remarks></remarks>
    Private mode_ As Integer
#End Region

#Region "プロパティ"
    ''' <summary>
    ''' 実行モードプロパティ
    ''' </summary>
    ''' <value>実行モード</value>
    ''' <returns>実行モード 0:全情報取得、1:車両情報のみ取得</returns>
    ''' <remarks></remarks>
    Public Property Mode As Integer
        Get
            Return mode_
        End Get
        Set(value As Integer)
            mode_ = value
        End Set
    End Property
#End Region

#Region "001.見積情報取得"
    '2019/02/14 TCS 河原 TKM UAT0651対応(タブレットで契約した見積のIDは論削済みでも取得するように修正) START
    '2013/12/06 TCS 森    Aカード情報相互連携開発 START
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 001.見積情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積情報DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetEstimationInfoDataTable(ByVal estimateId As Long, _
                                               ByVal mode As Integer) As IC3070201DataSet.IC3070201EstimationInfoDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimationInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201EstimationInfoDataTable)("IC3070201_001")
            Dim sql As New StringBuilder
            With sql
                If (mode = 0) Then
                    .Append("SELECT /* IC3070201_001 */ ")
                    .Append("       A.ESTIMATEID ")                     '見積管理ID
                    .Append("     , A.DLRCD ")                          '販売店コード
                    .Append("     , A.STRCD ")                          '店舗コード
                    .Append("     , A.FLLWUPBOX_SEQNO ")                'Follow-up BOX内連番
                    .Append("     , A.CNT_STRCD ")                      '契約店舗コード
                    .Append("     , A.CNT_STAFF ")                      '契約スタッフ
                    .Append("     , A.CSTKIND ")                        '顧客種別
                    .Append("     , A.CUSTOMERCLASS ")                  '顧客分類
                    .Append("     , A.CRCUSTID ")                       '活動先顧客コード
                    .Append("     , A.CUSTID ")                         '基幹お客様コード
                    .Append("     , A.DELIDATE ")                       '納車予定日
                    .Append("     , A.DISCOUNTPRICE ")                  '値引き額
                    .Append("     , A.MEMO ")                           'メモ
                    .Append("     , A.ESTPRINTDATE ")                   '見積印刷日
                    .Append("     , A.CONTRACTNO ")                     '契約書№
                    .Append("     , A.CONTPRINTFLG ")                   '契約書印刷フラグ
                    .Append("     , A.CONTRACTFLG ")                    '契約状況フラグ
                    .Append("     , A.CONTRACTDATE ")                   '契約完了日
                    .Append("     , A.DELFLG ")                         '削除フラグ
                    .Append("     , A.TCVVERSION ")                     'TCVバージョン
                    .Append("     , B.SERIESCD ")                       'シリーズコード
                    .Append("     , B.SERIESNM ")                       'シリーズ名称
                    .Append("     , B.MODELCD ")                        'モデルコード
                    .Append("     , B.MODELNM ")                        'モデル名称
                    .Append("     , B.BODYTYPE ")                       'ボディータイプ
                    .Append("     , B.DRIVESYSTEM ")                    '駆動方式
                    .Append("     , B.DISPLACEMENT ")                   '排気量
                    .Append("     , B.TRANSMISSION ")                   'ミッションタイプ
                    .Append("     , B.SUFFIXCD ")                       'サフィックス
                    .Append("     , B.EXTCOLORCD ")                     '外装色コード
                    .Append("     , B.EXTCOLOR ")                       '外装色名称
                    .Append("     , B.EXTAMOUNT ")                      '外装追加費用
                    .Append("     , B.INTCOLORCD ")                     '内装色コード
                    .Append("     , B.INTCOLOR ")                       '内装色名称
                    .Append("     , B.INTAMOUNT ")                      '内装追加費用
                    .Append("     , B.MODELNUMBER ")                    '車両型号
                    .Append("     , B.BASEPRICE ")                      '車両価格
                    .Append("     , A.CREATEDATE ")                     '作成日
                    .Append("     , A.CREATEACCOUNT ")                  '作成ユーザアカウント
                    .Append("     , A.UPDATEACCOUNT ")                  '更新ユーザアカウント
                    .Append("     , A.CREATEID ")                       '作成機能ID
                    .Append("     , A.UPDATEID ")                       '更新機能ID
                    .Append("     , A.EST_ACT_FLG ")                    '見積実績フラグ
                    .Append("     , NVL(D.ACARD_NUM, NVL(E.ACARD_NUM, F.ACARD_NUM)) AS ACARD_NUM ") 'A-Card番号
                    .Append("     , B.SERIESCD AS EST_SERIESCD ")       'シリーズコード
                    .Append("     , A.CONTRACT_APPROVAL_STATUS ")       '契約承認ステータス
                    .Append("     , A.CONTRACT_APPROVAL_STAFF ")        '契約承認スタッフ
                    .Append("     , A.CONTRACT_APPROVAL_REQUESTDATE ")  '契約承認依頼日時
                    .Append("     , A.CONTRACT_APPROVAL_REQUESTSTAFF ") '契約承認依頼スタッフ
                    '2018/06/15 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 START
                    .Append("     , NVL(D.DIRECT_SALES_FLG, NVL(E.DIRECT_SALES_FLG, F.DIRECT_SALES_FLG)) AS DIRECT_SALES_FLG ") '直販フラグ
                    '2018/06/15 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 END
                    .Append("  FROM TBL_ESTIMATEINFO A ")               '見積情報テーブル
                    .Append("     , TBL_EST_VCLINFO B ")                '見積車両情報テーブル
                    .Append("     , TB_T_SALES D ")                     '商談テーブル
                    .Append("     , TB_T_SALES_TEMP E ")                '商談一時テーブル
                    .Append("     , TB_H_SALES F ")                     '商談(History)テーブル
                    .Append(" WHERE A.ESTIMATEID = B.ESTIMATEID ")
                    .Append("   AND A.FLLWUPBOX_SEQNO = D.SALES_ID(+) ")
                    .Append("   AND A.FLLWUPBOX_SEQNO = E.SALES_ID(+) ")
                    .Append("   AND A.FLLWUPBOX_SEQNO = F.SALES_ID(+) ")
                    .Append("   AND A.ESTIMATEID = :ESTIMATEID ")
                    .Append("   AND (A.DELFLG = '0' OR A.CONTRACT_APPROVAL_STATUS = '2') ")
                Else
                    .Append("SELECT ")
                    .Append("  /* IC3070201_201 */ ")
                    .Append("  A.ESTIMATEID , ")
                    .Append("  A.DLRCD , ")
                    .Append("  A.STRCD , ")
                    .Append("  A.FLLWUPBOX_SEQNO , ")
                    .Append("  A.CNT_STRCD , ")
                    .Append("  A.CNT_STAFF , ")
                    .Append("  A.CSTKIND , ")
                    .Append("  A.CUSTOMERCLASS , ")
                    .Append("  A.CRCUSTID , ")
                    .Append("  A.CUSTID , ")
                    .Append("  A.DELIDATE , ")
                    .Append("  A.DISCOUNTPRICE , ")
                    .Append("  A.MEMO , ")
                    .Append("  A.ESTPRINTDATE , ")
                    .Append("  A.CONTRACTNO , ")
                    .Append("  A.CONTPRINTFLG , ")
                    .Append("  A.CONTRACTFLG , ")
                    .Append("  A.CONTRACTDATE , ")
                    .Append("  A.DELFLG , ")
                    .Append("  A.TCVVERSION , ")
                    .Append("  C.VCLSERIES_CD AS SERIESCD , ")
                    .Append("  C.VCLSERIES_NAME AS SERIESNM , ")
                    .Append("  B.MODELCD, ")
                    .Append("  B.MODELNM, ")
                    .Append("  B.BODYTYPE , ")
                    .Append("  B.DRIVESYSTEM , ")
                    .Append("  B.DISPLACEMENT , ")
                    .Append("  B.TRANSMISSION , ")
                    .Append("  B.SUFFIXCD , ")
                    .Append("  B.EXTCOLORCD , ")
                    .Append("  B.EXTCOLOR , ")
                    .Append("  B.EXTAMOUNT , ")
                    .Append("  B.INTCOLORCD , ")
                    .Append("  B.INTCOLOR , ")
                    .Append("  B.INTAMOUNT , ")
                    .Append("  B.MODELNUMBER , ")
                    .Append("  B.BASEPRICE , ")
                    .Append("  A.CREATEDATE , ")
                    .Append("  A.CREATEACCOUNT , ")
                    .Append("  A.UPDATEACCOUNT , ")
                    .Append("  A.CREATEID , ")
                    .Append("  A.UPDATEID , ")
                    .Append("  A.EST_ACT_FLG , ")
                    .Append("  NVL(D.ACARD_NUM, NVL(E.ACARD_NUM, F.ACARD_NUM)) AS ACARD_NUM , ")
                    .Append("  B.SERIESCD AS EST_SERIESCD , ")
                    .Append("  A.CONTRACT_APPROVAL_STATUS , ")
                    .Append("  A.CONTRACT_APPROVAL_STAFF , ")
                    .Append("  A.CONTRACT_APPROVAL_REQUESTDATE , ")
                    .Append("  A.CONTRACT_APPROVAL_REQUESTSTAFF ")
                    '2018/06/15 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 START
                    .Append(" , NVL(D.DIRECT_SALES_FLG, NVL(E.DIRECT_SALES_FLG, F.DIRECT_SALES_FLG)) AS DIRECT_SALES_FLG ") '直販フラグ
                    '2018/06/15 TCS 舩橋 TKM Next Gen e-CRB Project Application development Block B-1 END
                    .Append(" FROM ")
                    .Append("  TBL_ESTIMATEINFO A , ")
                    .Append("  TBL_EST_VCLINFO B , ")
                    '車種世代番号が最新のデータを使用する
                    .Append("  (SELECT C1.CAR_NAME_CD_AI21, C1.VCLCLASS_GENE, C1.VCLSERIES_CD, C1.VCLSERIES_NAME ")
                    .Append("        ,  ROW_NUMBER() OVER(PARTITION BY C1.CAR_NAME_CD_AI21 ")
                    .Append("           ORDER BY C1.VCLCLASS_GENE DESC ) AS ROWCNT ")
                    .Append("   FROM TBL_MSTCARNAME C1) C, ")
                    .Append("  TB_T_SALES D , ")
                    .Append("  TB_T_SALES_TEMP E , ")
                    .Append("  TB_H_SALES F ")
                    .Append(" WHERE ")
                    .Append("      A.ESTIMATEID = B.ESTIMATEID ")
                    .Append("  AND B.SERIESCD = C.CAR_NAME_CD_AI21 ")
                    .Append("  AND A.FLLWUPBOX_SEQNO = D.SALES_ID(+) ")
                    .Append("  AND A.FLLWUPBOX_SEQNO = E.SALES_ID(+) ")
                    .Append("  AND A.FLLWUPBOX_SEQNO = F.SALES_ID(+) ")
                    .Append("  AND A.ESTIMATEID = :ESTIMATEID ")
                    .Append("  AND (A.DELFLG = '0' OR A.CONTRACT_APPROVAL_STATUS = '2') ")
                    .Append("  AND C.ROWCNT = 1 ")
                End If
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetEstimationInfoDataTable_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()
        End Using
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        '2013/12/06 TCS 森    Aカード情報相互連携開発 END
    End Function
    '2019/02/14 TCS 河原 TKM UAT0651対応(タブレットで契約した見積のIDは論削済みでも取得するように修正) END

#End Region

#Region "002.見積車両オプション情報取得"

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 002.見積車両オプション情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積車両オプション情報DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetVclOptionInfoDataTable(ByVal estimateId As Long) As IC3070201DataSet.IC3070201VclOptionInfoDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetVclOptionInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201VclOptionInfoDataTable)("IC3070201_002")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070201_002 */ ")
                .Append("       ESTIMATEID ")               '見積管理ID
                .Append("     , OPTIONPART ")               'オプション区分
                .Append("     , OPTIONCODE ")               'オプションコード
                .Append("     , OPTIONNAME ")               'オプション名
                .Append("     , PRICE ")                    '価格
                .Append("     , INSTALLCOST ")              '取付費用
                .Append("  FROM TBL_EST_VCLOPTIONINFO ")    '見積車両オプション情報
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")

                '実行モードが1の場合、メーカーオプションのみ取得
                If (Me.Mode.Equals(1)) Then
                    '2012/03/16 TCS 陳【SALES_2】EDIT START
                    '.Append("   AND OPTIONPART = '1' ")
                    .Append("   AND OPTIONPART IN ('1','2') ")
                    '2012/03/16 TCS 陳【SALES_2】EDIT END
                End If

                .Append("ORDER BY ")
                .Append("       OPTIONPART ")               'オプション区分
                .Append("     , OPTIONCODE ")               'オプションコード

            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetVclOptionInfoDataTable_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()
        End Using
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
    End Function

#End Region

#Region "003.見積顧客情報取得"

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 003.見積顧客情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積顧客情報DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetCustomerInfoDataTable(ByVal estimateId As Long) As IC3070201DataSet.IC3070201CustomerInfoDataTable

        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201CustomerInfoDataTable)("IC3070201_003")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070201_003 */ ")
                .Append("       ESTIMATEID ")               '見積管理ID
                .Append("     , CONTRACTCUSTTYPE ")         '契約顧客種別
                .Append("     , CUSTPART ")                 '顧客区分
                .Append("     , NAME ")                     '氏名
                .Append("     , SOCIALID ")                 '国民番号
                .Append("     , ZIPCODE ")                  '郵便番号
                .Append("     , ADDRESS ")                  '住所
                .Append("     , TELNO ")                    '電話番号
                .Append("     , MOBILE ")                   '携帯電話番号
                .Append("     , FAXNO ")                    'FAX番号
                .Append("     , EMAIL ")                    'e-MAILアドレス
                ' 2013/12/06 TCS 森    Aカード情報相互連携開発 START
                .Append("     , PRIVATE_FLEET_ITEM_CD ")    '個人法人項目コード
                .Append("     , NAMETITLE_CD ")             '敬称コード
                .Append("     , NAMETITLE_NAME ")           '敬称
                .Append("     , FIRST_NAME ")               'ファーストネーム
                .Append("     , MIDDLE_NAME ")              'ミドルネーム
                .Append("     , LAST_NAME ")                'ラストネーム
                .Append("     , CST_ADDRESS_1 ")            '顧客住所1
                .Append("     , CST_ADDRESS_2 ")            '顧客住所2 
                .Append("     , CST_ADDRESS_3 ")            '顧客住所3 
                .Append("     , CST_ADDRESS_STATE ")        '顧客住所（州）
                .Append("     , CST_ADDRESS_DISTRICT ")     '顧客住所（地区）
                .Append("     , CST_ADDRESS_CITY ")         '顧客住所（市）
                .Append("     , CST_ADDRESS_LOCATION ")     '顧客住所（地域）
                ' 2013/12/06 TCS 森    Aカード情報相互連携開発 END
                .Append("  FROM TBL_EST_CUSTOMERINFO ")    '見積顧客情報テーブル
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")
                .Append("ORDER BY ")
                .Append("       CONTRACTCUSTTYPE ")         '契約顧客種別
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerInfoDataTable_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()
        End Using
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
    End Function

#End Region

#Region "004.見積諸費用情報取得"

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 004.見積諸費用情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積諸費用情報DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetChargeInfoDataTable(ByVal estimateId As Long) As IC3070201DataSet.IC3070201ChargeInfoDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetChargeInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201ChargeInfoDataTable)("IC3070201_004")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070201_004 */ ")
                .Append("       ESTIMATEID ")               '見積管理ID
                .Append("     , ITEMCODE ")                 '費用項目コード
                .Append("     , ITEMNAME ")                 '費用項目名
                .Append("     , PRICE ")                    '価格
                '$99 Ken-Suzuki Add Start
                .Append("     , CHARGEDVS ")                '諸費用区分
                '$99 Ken-Suzuki Add End
                .Append("  FROM TBL_EST_CHARGEINFO ")       '見積諸費用情報テーブル
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")

                .Append("ORDER BY ")
                .Append("       ITEMCODE ")                 '費用項目コード
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetChargeInfoDataTable_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()
        End Using
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
    End Function

#End Region


#Region "005.見積支払方法情報取得"

    '2013/12/06 TCS 森    Aカード情報相互連携開発 START
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 005.見積支払方法情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積支払方法情報DataTable</returns>
    ''' <remarks></remarks>
    ''' <History>
    '''  2013/01/18 TCS 上田  GL0871対応
    '''  2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応
    ''' </History>
    Public Function GetPaymentInfoDataTable(ByVal estimateId As String) As IC3070201DataSet.IC3070201PaymentInfoDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPaymentInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201PaymentInfoDataTable)("IC3070201_202")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT ")
                .Append("  /* IC3070201_202 */ ")
                .Append("  A.ESTIMATEID , ")
                .Append("  A.PAYMENTMETHOD , ")
                .Append("  A.FINANCECOMCODE , ")
                .Append("  A.PAYMENTPERIOD , ")
                .Append("  A.MONTHLYPAYMENT , ")
                .Append("  A.DEPOSIT , ")
                .Append("  A.BONUSPAYMENT , ")
                .Append("  A.DUEDATE , ")
                .Append("  A.DELFLG , ")
                .Append("  B.FNC_COMPANY_NAME AS FINANCECOMNAME , ")
                .Append("  A.SELECTFLG , ")
                .Append("  TRUNC(A.INTERESTRATE, 3) AS INTERESTRATE , ")
                .Append("  A.DEPOSITPAYMENTMETHOD ")
                .Append("FROM ")
                .Append("  TBL_EST_PAYMENTINFO A , ")
                .Append("  TB_M_FINANCE_COMPANY B ")
                .Append("WHERE ")
                .Append("      RTRIM(A.FINANCECOMCODE) =B.FNC_COMPANY_CD(+) ")
                .Append("  AND A.ESTIMATEID = :ESTIMATEID ")
                .Append("ORDER BY ")
                .Append("  A.PAYMENTMETHOD ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPaymentInfoDataTable_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()
        End Using
    End Function
    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
    '2013/12/06 TCS 森    Aカード情報相互連携開発 END

#End Region

#Region "006.見積下取車両情報取得"

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 006.見積下取車両情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積下取車両情報DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetTradeincarInfoDataTable(ByVal estimateId As Long) As IC3070201DataSet.IC3070201TradeincarInfoDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetTradeincarInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201TradeincarInfoDataTable)("IC3070201_006")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070201_006 */ ")
                .Append("       ESTIMATEID ")                   '見積管理ID
                .Append("     , SEQNO ")                        '連番
                .Append("     , ASSESSMENTNO ")                 '査定№
                .Append("     , VEHICLENAME ")                  '車名
                .Append("     , ASSESSEDPRICE ")                '提示価格
                .Append("  FROM TBL_EST_TRADEINCARINFO ")       '見積下取車両情報テーブル
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")
                .Append("ORDER BY ")
                .Append("       SEQNO ")                        '連番
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetTradeincarInfoDataTable_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()
        End Using
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
    End Function

#End Region

#Region "007.見積保険情報取得"

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 007.見積保険情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積保険情報DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetInsuranceInfoDataTable(ByVal estimateId As Long) As IC3070201DataSet.IC3070201EstInsuranceInfoDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInsuranceInfoDataTable_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201EstInsuranceInfoDataTable)("IC3070201_007")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070201_007 */ ")
                .Append("       ESTIMATEID ")                   '見積管理ID
                .Append("     , INSUDVS ")                      '保険区分
                ' 2014/03/18 TCS 松月 TMT不具合対応 Modify Start
                .Append("     , TRIM(INSUCOMCD) AS INSUCOMCD ")                    '保険会社コード
                ' 2014/03/18 TCS 松月 TMT不具合対応 Modify End
                .Append("     , INSUKIND ")                     '保険種別
                .Append("     , AMOUNT ")                       '保険金額
                .Append("  FROM TBL_EST_INSURANCEINFO ")        '見積保険情報テーブル
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInsuranceInfoDataTable_End")
            'ログ出力 End *****************************************************************************
            Return query.GetData()
        End Using
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
    End Function

#End Region

    '2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応 START
    ''2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 START
    ' ''' <summary>
    ' ''' 車両購入税最低価格取得
    ' ''' </summary>
    ' ''' <param name="estimateId">見積管理ID</param>
    ' ''' <returns>Double</returns>
    ' ''' <remarks></remarks>
    'Public Function GetPurchaseMinimumTax(ByVal dlrcd As String,
    '                                      ByVal estimateId As Long) As IC3070201DataSet.IC3070201VclPurchaseTaxDataTable
    '    Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201VclPurchaseTaxDataTable)("IC3070201_008")
    '        Dim sql As New StringBuilder
    '        With sql
    '            .AppendLine("SELECT /* IC3070201_008 */")
    '            .AppendLine("       EST_VCL.ESTIMATEID")
    '            .AppendLine("     , EST_VCL.SERIESCD")
    '            .AppendLine("     , EST_VCL.MODELCD")
    '            .AppendLine("     , TAX.MINIMUMPRICE")
    '            .AppendLine("  FROM TBL_ESTIMATEINFO EST")
    '            .AppendLine("     , TBL_EST_VCLINFO EST_VCL")
    '            .AppendLine("     , (")
    '            .AppendLine("        SELECT A.SERIESCD")
    '            .AppendLine("             , A.MODELCD")
    '            .AppendLine("             , A.MINIMUMPRICE")
    '            .AppendLine("             , ROW_NUMBER() OVER (PARTITION BY A.SERIESCD,A.MODELCD ORDER BY DIV) AS RNUM")
    '            .AppendLine("          FROM (")
    '            .AppendLine("                SELECT SERIESCD")
    '            .AppendLine("                     , MODELCD")
    '            .AppendLine("                     , MINIMUMPRICE")
    '            .AppendLine("                     , CASE WHEN DLRCD = :DLRCD THEN 1")
    '            .AppendLine("                            WHEN DLRCD = 'XXXXX' THEN 2")
    '            .AppendLine("                       END AS DIV")
    '            .AppendLine("                  FROM TBL_EST_VCLPURCHASETAXMAST")
    '            .AppendLine("                 WHERE DLRCD IN (:DLRCD, 'XXXXX')")
    '            .AppendLine("                 ORDER BY DIV")
    '            .AppendLine("            ) A")
    '            .AppendLine("       ) TAX")
    '            .AppendLine(" WHERE EST_VCL.ESTIMATEID = EST.ESTIMATEID")
    '            .AppendLine("   AND TAX.SERIESCD = CASE WHEN EST_VCL.SERIESCD = :SERIES_CODE_CAMRY AND EST_VCL.MODELCD LIKE :MODEL_CD_HV THEN CAST(:SERIES_CODE_CMYHV AS NVARCHAR2(64))")
    '            .AppendLine("                           ELSE EST_VCL.SERIESCD")
    '            .AppendLine("                      END")
    '            .AppendLine("   AND TAX.MODELCD = EST_VCL.MODELCD")
    '            .AppendLine("   AND EST.ESTIMATEID = :ESTIMATEID")
    '            .AppendLine("   AND EST.DELFLG = '0'")
    '            .AppendLine("   AND TAX.RNUM = 1")
    '        End With
    '        query.CommandText = sql.ToString()
    '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
    '        query.AddParameterWithTypeValue("SERIES_CODE_CAMRY", OracleDbType.NVarchar2, SERIES_CODE_CAMRY)
    '        query.AddParameterWithTypeValue("MODEL_CD_HV", OracleDbType.Varchar2, MODEL_CD_HV)
    '        query.AddParameterWithTypeValue("SERIES_CODE_CMYHV", OracleDbType.NVarchar2, SERIES_CODE_CMYHV)
    '        query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
    '        Return query.GetData()
    '    End Using

    'End Function
    ''2012/11/05 TCS 神本 【A.STEP2】次世代e-CRB  新車タブレット横展開に向けた機能開発 END
    '2013/03/08 TCS 坪根 【A.STEP2】新車タブレット見積り画面機能拡充対応 END


    ' 2013/12/06 TCS 森    Aカード情報相互連携開発 START
#Region "008.CustomerInfoDetail"

    ''' <summary>
    ''' 顧客情報詳細取得
    ''' </summary>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="cstId">顧客コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCustomerInfoDetailDataTable(ByVal dlrCd As String, ByVal cstId As String) As IC3070201DataSet.IC3070201CustomerInfoDetailDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerInfoDetailDataTable_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201CustomerInfoDetailDataTable)("IC3070201_008")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070201_008 */ ")
                .Append("       T1.CST_ID, ")                    '顧客ID
                .Append("       T1.DMS_CST_CD_DISP, ")           '基幹顧客コード（表示用）
                .Append("       T1.NEWCST_CD, ")                 '未取引客コード 
                .Append("       T1.ORGCST_CD, ")                 '自社客コード 
                .Append("       T2.SLS_PIC_STF_CD, ")            'セールス担当スタッフコード
                .Append("       T1.FLEET_FLG, ")                 '法人フラグ 
                .Append("       T1.CST_SOCIALNUM, ")             '顧客識別番号区分
                .Append("       T1.CST_GENDER, ")                '性別区分
                .Append("       T1.CST_BIRTH_DATE, ")            '顧客誕生日
                .Append("       T1.NAMETITLE_CD, ")              '敬称コード
                .Append("       T1.NAMETITLE_NAME, ")            '敬称
                .Append("       T1.CST_NAME, ")                  '顧客氏名 
                .Append("       T1.FIRST_NAME, ")                'ファーストネーム
                .Append("       T1.MIDDLE_NAME, ")               'ミドルネーム
                .Append("       T1.LAST_NAME, ")                 'ラストネーム
                .Append("       T1.NICK_NAME, ")                 'ニックネーム
                .Append("       T1.CST_COMPANY_NAME, ")          '顧客会社名
                .Append("       T1.FLEET_PIC_NAME, ")            '法人担当者名
                .Append("       T1.FLEET_PIC_DEPT, ")            '法人担当者所属部署
                .Append("       T1.FLEET_PIC_POSITION, ")        '法人担当者役職
                .Append("       T1.CST_ADDRESS, ")               '顧客住所 
                .Append("       T1.CST_ADDRESS_1, ")             '顧客住所1 
                .Append("       T1.CST_ADDRESS_2, ")             '顧客住所2 
                .Append("       T1.CST_ADDRESS_3, ")             '顧客住所3 
                .Append("       T1.CST_DOMICILE, ")              '本籍
                .Append("       T1.CST_COUNTRY, ")               '国籍
                .Append("       T1.CST_ZIPCD, ")                 '顧客郵便番号 
                .Append("       T1.CST_ADDRESS_STATE, ")         '顧客住所（州）
                .Append("       T1.CST_ADDRESS_DISTRICT, ")      '顧客住所（地区）
                .Append("       T1.CST_ADDRESS_CITY, ")          '顧客住所（市）
                .Append("       T1.CST_ADDRESS_LOCATION, ")      '顧客住所（地域）
                .Append("       T1.CST_PHONE, ")                 '顧客電話番号 
                .Append("       T1.CST_FAX, ")                   '顧客FAX番号 
                .Append("       T1.CST_MOBILE, ")                '顧客携帯電話番号 
                .Append("       T1.CST_EMAIL_1, ")               '顧客EMAILアドレス1 
                .Append("       T1.CST_EMAIL_2, ")               '顧客EMAILアドレス2 
                .Append("       T1.CST_BIZ_PHONE, ")             '顧客勤め先電話番号 
                .Append("       T1.CST_INCOME, ")                '顧客収入
                .Append("       T1.CST_OCCUPATION_ID, ")         '顧客職業ID
                .Append("       T1.CST_OCCUPATION, ")            '顧客職業
                .Append("       T1.MARITAL_TYPE, ")              '結婚区分
                .Append("       T1.DEFAULT_LANG, ")              'デフォルト言語
                .Append("       T1.PRIVATE_FLEET_ITEM_CD, ")     '個人法人項目コード
                .Append("       T1.DMS_NEWCST_CD_DISP, ")        '見込み客ＤＭＳコード
                .Append("       T4.CONTACT_TIMESLOT, ")          '連絡時間帯ID
                .Append("       T3.CST_TYPE ")                   '顧客種別
                .Append("  FROM TB_M_CUSTOMER T1, ")             '顧客マスタ
                .Append("       TB_M_CUSTOMER_VCL T2, ")         '販売店顧客車両
                .Append("       TB_M_CUSTOMER_DLR T3,  ")
                .Append("    (SELECT CST_ID, SUM(CONTACT_TIMESLOT) CONTACT_TIMESLOT ")
                .Append("      FROM TB_M_CST_CONTACT_TIMESLOT ")
                .Append("     WHERE CST_ID = :CST_ID AND TIMESLOT_CLASS = '1' GROUP BY CST_ID) T4 ")
                .Append("  WHERE T1.CST_ID = T2.CST_ID ")
                .Append("  AND T1.CST_ID = T4.CST_ID(+) ")
                .Append("    AND EXISTS ( ")
                .Append("             SELECT 1 ")
                .Append("               FROM ( ")
                .Append("                SELECT T5.DLR_CD,T5.CST_ID,T5.VCL_ID ")
                .Append("                     ,  ROW_NUMBER() OVER(PARTITION BY T5.DLR_CD,T5.CST_ID,T5.VCL_ID ")
                .Append("                        ORDER BY T5.ROW_UPDATE_DATETIME DESC) AS ROWCNT ")
                .Append("                FROM TB_M_CUSTOMER_VCL  T5 ")
                .Append("                WHERE T5.DLR_CD = :DLR_CD ")
                .Append("                  AND T5.CST_ID = :CST_ID ")
                .Append("                  AND T5.CST_VCL_TYPE = :CST_VCL_TYPE1 ")
                .Append("                  AND T5.OWNER_CHG_FLG = :OWNER_CHG_FLG0 ")
                .Append("               ) T6 ")
                .Append("              WHERE T6.ROWCNT = 1 ")
                .Append("                AND T6.DLR_CD  = T2.DLR_CD ")
                .Append("                AND T6.CST_ID  = T2.CST_ID ")
                .Append("                AND T6.VCL_ID  = T2.VCL_ID ")
                .Append("             ) ")
                .Append("  AND T2.CST_VCL_TYPE = :CST_VCL_TYPE1 ")
                .Append("  AND T2.OWNER_CHG_FLG = :OWNER_CHG_FLG0 ")
                .Append("  AND T2.DLR_CD = :DLR_CD ")
                .Append("  AND T2.CST_ID = :CST_ID ")
                .Append("  AND T3.DLR_CD = T2.DLR_CD ")
                .Append("  AND T3.CST_ID = T2.CST_ID ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dlrCd)
            query.AddParameterWithTypeValue("CST_VCL_TYPE1", OracleDbType.NVarchar2, "1")
            query.AddParameterWithTypeValue("OWNER_CHG_FLG0", OracleDbType.NVarchar2, "0")
            query.AddParameterWithTypeValue("CST_ID", OracleDbType.NVarchar2, cstId)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetCustomerInfoDetailDataTable_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function


#End Region

#Region "ユーザ名取得"
    ''' <summary>
    ''' スタッフ情報取得
    ''' </summary>
    ''' <param name="staffcd">スタッフコード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetUser(ByVal staffcd As String) As IC3070201DataSet.IC3070201UsersDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetUser_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201UsersDataTable)("IC3070201_013")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070201_013 */ ")
                .Append("       USERNAME ")            'スタッフネーム
                .Append("  FROM TBL_USERS ")            '顧客連絡時間帯
                .Append("  WHERE ACCOUNT = :ACCOUNT ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, staffcd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetUser_End")
            'ログ出力 End *****************************************************************************

            Return query.GetData()

        End Using

    End Function


#End Region

#Region "基幹コード取得"

    '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 START
    '共通基盤使用により削除

    ' ''' <summary>
    ' ''' 基幹コード取得(販売店コード、店舗コード)
    ' ''' </summary>
    ' ''' <param name="DLRCD">販売店コード</param>
    ' ''' <param name="STRCD">店舗コード</param>
    ' ''' <returns>取得結果</returns>
    ' ''' <remarks></remarks>
    'Public Function GetDmsCd(ByVal DLRCD As String, _
    '                         ByVal STRCD As String) As IC3070201DataSet.IC3070201DmsCdDataTable
    '    'ログ出力 Start ***************************************************************************
    '    Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetDmsCd_Start")
    '    'ログ出力 End *****************************************************************************

    '    Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201DmsCdDataTable)("IC3070201_010")
    '        Dim sql As New StringBuilder
    '        With sql
    '            .Append("SELECT /* IC3070201_010 */ ")
    '            .Append("  DMS_CD_1 , ")
    '            .Append("  DMS_CD_2 ")
    '            .Append("  FROM TB_M_DMS_CODE_MAP ")
    '            .Append("  WHERE DLR_CD = 'XXXXX' ")
    '            .Append("    AND DMS_CD_TYPE = '2' ")
    '            .Append("    AND ICROP_CD_1 = :ICROP_CD_1 ")
    '            .Append("    AND ICROP_CD_2 = :ICROP_CD_2 ")
    '            .Append("  ORDER BY ROW_UPDATE_DATETIME ")
    '        End With
    '        query.CommandText = sql.ToString()
    '        query.AddParameterWithTypeValue("ICROP_CD_1", OracleDbType.NVarchar2, Trim(DLRCD))
    '        query.AddParameterWithTypeValue("ICROP_CD_2", OracleDbType.NVarchar2, Trim(STRCD))

    '        'ログ出力 Start ***************************************************************************
    '        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetDmsCd_End")
    '        'ログ出力 End *****************************************************************************
    '        Dim ResultSet As IC3070201DataSet.IC3070201DmsCdDataTable = query.GetData()

    '        Return ResultSet

    '    End Using


    'End Function
    '2016/04/26 TCS 山口 （トライ店システム評価）他システム連携における複数店舗コード変換対応 END

    ''' <summary>
    ''' 基幹コード取得(顧客職業ID)
    ''' </summary>
    ''' <param name="OccupationId">顧客職業ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Function GetDmsCdOccupationId(ByVal OccupationId As String) As String
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetDmsCdOccupationId_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of DataTable)("IC3070201_014")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070201_014 */ ")
                .Append("  DMS_CD_1 ")
                .Append("  FROM TB_M_DMS_CODE_MAP ")
                .Append("  WHERE DLR_CD = 'XXXXX' ")
                .Append("    AND DMS_CD_TYPE = '12' ")
                .Append("    AND ICROP_CD_1 = :ICROP_CD_1 ")
                .Append("  ORDER BY ROW_UPDATE_DATETIME ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ICROP_CD_1", OracleDbType.NVarchar2, Trim(OccupationId))

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetDmsCdOccupationId_End")
            'ログ出力 End *****************************************************************************
            Dim ResultSet As DataTable = query.GetData()

            If ResultSet.Rows.Count > 0 Then
                Return CStr(ResultSet.Rows(0).Item("DMS_CD_1"))
            Else
                Return String.Empty
            End If

        End Using
    End Function
#End Region

    ''' <summary>
    ''' 画像取得
    ''' </summary>
    ''' <param name="SeriesCd">シリーズコード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPicture(ByVal SeriesCd As String) As IC3070201DataSet.IC3070201PictureDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPicture_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201PictureDataTable)("IC3070201_011")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070201_011 */ ")
                .Append("  MODEL_PICTURE ,")
                .Append("  LOGO_PICTURE ")
                .Append(" FROM TB_M_MODEL ")
                .Append(" WHERE MODEL_CD = :MODEL_CD ")
                .Append("  AND INUSE_FLG = '1' ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("MODEL_CD", OracleDbType.NVarchar2, SeriesCd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPicture_End")
            'ログ出力 End *****************************************************************************
            Dim ResultSet As IC3070201DataSet.IC3070201PictureDataTable = query.GetData()

            Return ResultSet

        End Using


    End Function

    ''' <summary>
    ''' 型式画像取得
    ''' </summary>
    ''' <param name="modelCd">モデルコード</param>
    ''' <param name="colorCd">カラーコード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPiGetKatashikiPicturecture(ByVal modelCd As String, ByVal colorCd As String) As IC3070201DataSet.IC3070201KatashikiPictureDataTable
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPicture_Start")
        'ログ出力 End *****************************************************************************

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201KatashikiPictureDataTable)("IC3070201_015")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070201_015 */ ")
                .Append("  VCL_PICTURE ")
                .Append(" FROM TB_M_KATASHIKI_PICTURE ")
                .Append(" WHERE VCL_KATASHIKI = :VCL_KATASHIKI ")
                .Append("   AND BODYCLR_CD = :BODYCLR_CD ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("VCL_KATASHIKI", OracleDbType.NVarchar2, modelCd)
            query.AddParameterWithTypeValue("BODYCLR_CD", OracleDbType.NVarchar2, colorCd)

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetPiGetKatashikiPicturecture")
            'ログ出力 End *****************************************************************************
            Dim ResultSet As IC3070201DataSet.IC3070201KatashikiPictureDataTable = query.GetData()

            Return ResultSet

        End Using


    End Function

    ''' <summary>
    ''' 通知依頼情報取得
    ''' </summary>
    ''' <param name="noticeReqId">通知依頼ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNoticeRequest(ByVal noticeReqId As Long) As IC3070201DataSet.IC3070201NoticeRequestDataTable
        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201NoticeRequestDataTable)("IC3070201_012")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070201_012 */ ")
                .Append("  NOTICEREQCTG ,")
                .Append("  STATUS ")
                .Append(" FROM TBL_NOTICEREQUEST ")
                .Append(" WHERE NOTICEREQID = :NOTICEREQID ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Long, noticeReqId)

            Return query.GetData()
        End Using
    End Function

    ' 2013/12/06 TCS 森    Aカード情報相互連携開発 END

    '2018/05/01 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 DEL
    '2018/05/01 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

End Class

