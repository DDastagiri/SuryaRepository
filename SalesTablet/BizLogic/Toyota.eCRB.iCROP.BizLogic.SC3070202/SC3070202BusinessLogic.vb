Imports System.Text
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.Estimate.Quotation.DataAccess.IC3070201DataSet

Public Class SC3070202BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"
    ''' <summary>
    ''' 実行モード　見積情報取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MODE As Integer = 0

    ''' <summary>
    ''' 顧客区分　個人
    ''' </summary>
    ''' <remarks></remarks>
    Private Const INDIVIDUAL As String = "1"

    ''' <summary>
    ''' オプション最低表示数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MIN_OPTION As Integer = 13

    ''' <summary>
    ''' 下取り最低表示数
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MIN_TRADEIN As Integer = 2

    ''' <summary>
    ''' 費用項目コード　1
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ITEM_CODE_1 As String = "1"

    ''' <summary>
    ''' 費用項目コード　2
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ITEM_CODE_2 As String = "2"

    ''' <summary>
    ''' 日付変換ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONVERT_ID As Integer = 3

    ''' <summary>
    ''' プログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID As String = "SC3070202"

    ''' <summary>
    ''' 1: 名前の前に敬称(主に英語圏)、2: 名前の後ろに敬称(中国など)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const KeisyoZengo As String = "KEISYO_ZENGO"

    ''' <summary>
    ''' 敬称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const HonorificTitle As String = "HONORIFIC_TITLE"

    ''' <summary>
    ''' 列名　販売店コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_DLRCD = "DLRCD"

    ''' <summary>
    ''' 列名　保険会社コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_INSUCOMCD = "INSUCOMCD"

    ''' <summary>
    ''' 列名　保険種別
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_INSUKIND = "INSUKIND"

    ''' <summary>
    ''' 列名　デフォルト敬称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_DFLTNAMETITLE = "DEFOLTNAMETITLE"

    ''' <summary>
    ''' 列名　敬称位置
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_NAMETITLEPOSITION = "NAMETITLEPOSITION"

    ''' <summary>
    ''' 列名　メモ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_MEMO = "MEMO"

    ''' <summary>
    ''' 列名　年額（保険）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_AMOUNT = "AMOUNT"

    ''' <summary>
    ''' 列名　期間（月）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_PAYMENTPERIOD = "PAYMENTPERIOD"

    ''' <summary>
    ''' 列名　月額
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_MONTHLYPAYMENT = "MONTHLYPAYMENT"

    ''' <summary>
    ''' 列名　頭金
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_DEPOSIT = "DEPOSIT"

    ''' <summary>
    ''' 列名　ボーナス
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_BONUSPAYMENT = "BONUSPAYMENT"

    ''' <summary>
    ''' 列名　初回支払（日）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_DUEDATE = "DUEDATE"

    ''' <summary>
    ''' 列名　値引き額
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_DISCOUNTPRICE = "DISCOUNTPRICE"

    ''' <summary>
    ''' 納車予定日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_DELIDATE = "DELIDATE"

    ''' <summary>
    ''' 列名　お客様名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_NAME = "NAME"

    ''' <summary>
    ''' 列名　諸費用（金額）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_PRICE = "PRICE"

    ''' <summary>
    ''' 列名　保険会社名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_INSUCOMNM = "INSUCOMNM"

    ''' <summary>
    ''' 列名　保険種別名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_INSUKINDNM = "INSUKINDNM"

    ''' <summary>
    ''' 列名　融資会社コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_FINANCECOMCODE = "FINANCECOMCODE"

    ''' <summary>
    ''' 列名　融資会社名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_FINANCECOMNAME = "FINANCECOMNAME"

    ''' <summary>
    ''' 列名　顧客区分
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_CUSTPART = "CUSTPART"

    ''' <summary>
    ''' 列名　取り付け費用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_INSTALLCOST = "INSTALLCOST"

    ''' <summary>
    ''' 列名　支払い方法
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CLM_PAYMENTMETHOD = "PAYMENTMETHOD"

    ''' <summary>
    ''' テーブル名　見積保険情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_INSINFO = "IC3070201EstInsuranceInfo"

    ''' <summary>
    ''' テーブル名　支払い情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_PAYINFO = "IC3070201PaymentInfo"

    ''' <summary>
    ''' テーブル名　見積情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_ESTINFO = "IC3070201EstimationInfo"

    ''' <summary>
    ''' テーブル名　顧客情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_CUSTINFO = "IC3070201CustomerInfo"

    ''' <summary>
    ''' テーブル名　車両オプション情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_VCLOPTIONINFO = "IC3070201VclOptionInfo"

    ''' <summary>
    ''' テーブル名　下取り車両情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_TRADEINCARINFO = "IC3070201TradeincarInfo"

    ''' <summary>
    ''' テーブル名　諸費用情報(I/F)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const IFTBL_CHARGEINFO = "IC3070201ChargeInfo"

    ''' <summary>
    ''' テーブル名　保険会社情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_INSINFO = "SC3070202InsKindMast"

    ''' <summary>
    ''' テーブル名　融資会社情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_FINANCEINFO = "SC3070202FinanceComMast"

    ''' <summary>
    ''' テーブル名　システム環境情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const TBL_SYSTEMINFO = "SC3070202SystemEnvSetting"

    ''' <summary>
    ''' メッセージID　見積情報取得エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MSG_ID_IF As Integer = 9001

#End Region

#Region "メンバ変数"
    ''' <summary>
    ''' 終了コード
    ''' </summary>
    ''' <remarks></remarks>
    Private endResultId As Short = 0

    ''' <summary>
    ''' 下取り合計額
    ''' </summary>
    ''' <remarks></remarks>
    Private amountTradeIn As Double = 0

    ''' <summary>
    ''' オプション合計額
    ''' </summary>
    ''' <remarks></remarks>
    Private amountOption As Double = 0

    ''' <summary>
    ''' オプション合計数
    ''' </summary>
    ''' <remarks></remarks>
    Private optionCount As Integer = 0

#End Region

#Region "プロパティ"
    ''' <summary>
    ''' 終了コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>0の場合は正常、それ以外の場合エラー</remarks>
    Public ReadOnly Property ResultId() As Short
        Get
            Return endResultId
        End Get
    End Property
#End Region

#Region "Publicメソッド"
    ''' <summary>
    ''' 初期表示情報を取得
    ''' </summary>
    ''' <param name="sessionTbl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetInitialData(ByVal sessionTbl As SC3070202DataSet.SC3070202SessionDataTable) As SC3070202DataSet

        'セッション情報取得
        Dim sessionDtRow As SC3070202DataSet.SC3070202SessionRow = Nothing
        If sessionTbl IsNot Nothing Then
            sessionDtRow = CType(sessionTbl.Rows(0), SC3070202DataSet.SC3070202SessionRow)
        End If

        '見積情報データセットをI/Fから取得 
        Dim estDtIC3070201 As IC3070201DataSet
        estDtIC3070201 = Me.GetEstimateInfo(sessionDtRow.ESTIMATEID)

        '見積情報データセットが取得できたかチェック
        If estDtIC3070201 Is Nothing Then
            '取得できていなければエラー
            Return Nothing
        End If

        '自画面データセット生成（返却用ではない）
        Using estDtSC3070202 As New SC3070202DataSet

            'DataSetの中のテーブルを全て削除(I/Fのやり方で統一)
            estDtSC3070202.Tables.Clear()

            '保険情報取得
            Dim insuranceDt As SC3070202DataSet.SC3070202InsKindMastDataTable = estDtSC3070202.SC3070202InsKindMast
            'I/Fのデータテーブル「IC3070201EstInsuranceInfo」にデータが１行もなかったら
            If estDtIC3070201.Tables(IFTBL_INSINFO).Rows.Count = 0 Then
                estDtIC3070201.Tables(IFTBL_INSINFO).Rows.Add(0, "", "", "", 0)
            Else
                insuranceDt = Me.GetInsComInfo(estDtIC3070201, estDtSC3070202)
            End If

            '支払い方法
            Dim payMethod As String = sessionDtRow.PAYMENTMETHOD.ToString(CultureInfo.CurrentCulture)

            '融資情報取得
            Dim financeDt As SC3070202DataSet.SC3070202FinanceComMastDataTable = estDtSC3070202.SC3070202FinanceComMast
            'I/Fのデータテーブル「IC3070201PaymentInfo」にデータが１行もなかったら
            If estDtIC3070201.Tables(IFTBL_PAYINFO).Rows.Count = 0 Then
                estDtIC3070201.Tables(IFTBL_PAYINFO).Rows.Add(0, "", "", 0, 0, 0, 0, 0, "")
            Else
                financeDt = Me.GetFinanceComInfo(estDtIC3070201, estDtSC3070202, payMethod)
            End If

            '見積情報取得I/Fで取得したテーブルのデータが存在しているか確認し、ない場合は空行を１行作成
            If estDtIC3070201.Tables(IFTBL_CUSTINFO).Rows.Count = 0 Then
                estDtIC3070201.Tables(IFTBL_CUSTINFO).Rows.Add(0, "", "", "", "", "", "", "", "", "", "")
            End If

            '販売店情報取得
            Dim dlrDt As SC3070202DataSet.SC3070202SystemEnvSettingDataTable = Me.GetDlrInfo()

            'データセットにテーブル追加
            estDtSC3070202.Tables.Add(insuranceDt)
            estDtSC3070202.Tables.Add(financeDt)
            estDtSC3070202.Tables.Add(dlrDt)

            '取得結果コードに成功コード（0）を設定
            endResultId = 0

            '取得結果の返却
            Return SetDataSet(estDtIC3070201, estDtSC3070202, payMethod)

        End Using

    End Function
#End Region

#Region "Privateメソッド"
    ''' <summary>
    ''' I/Fから見積情報取得
    ''' </summary>
    ''' <param name="estimateId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetEstimateInfo(ByVal estimateId As Long) As IC3070201DataSet
        Dim apiBiz As New IC3070201BusinessLogic
        Dim apiDt As IC3070201DataSet
        apiDt = apiBiz.GetEstimationInfo(estimateId, MODE)

        '見積情報取得I/Fの結果IDが0でない、または見積情報テーブルにデータがない場合
        If Not apiBiz.ResultId = 0 _
        OrElse apiDt.Tables(IFTBL_ESTINFO).Rows.Count = 0 Then
            'エラー
            endResultId = MSG_ID_IF
            Return Nothing
        End If

        Return apiDt
    End Function

    ''' <summary>
    ''' 保険情報取得
    ''' </summary>
    ''' <param name="estDtIC3070201"></param>
    ''' <param name="estDtSC3070202"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetInsComInfo(ByVal estDtIC3070201 As IC3070201DataSet, ByVal estDtSC3070202 As SC3070202DataSet) As SC3070202DataSet.SC3070202InsKindMastDataTable

        '販売店コード
        Dim dlrCd As String = CStr(estDtIC3070201.Tables(IFTBL_ESTINFO).Rows(0).Item(CLM_DLRCD))
        '保険会社コード
        Dim insuranceComCd As String = String.Empty
        '保険種別
        Dim insuranceKind As String = String.Empty

        If IsDBNull(estDtIC3070201.Tables(IFTBL_INSINFO).Rows(0).Item(CLM_INSUCOMCD)) Then
            '保険会社コードがNULLの場合は、SQLを発行しない
            Return estDtSC3070202.SC3070202InsKindMast

        ElseIf IsDBNull(estDtIC3070201.Tables(IFTBL_INSINFO).Rows(0).Item(CLM_INSUKIND)) Then
            '保険種別のみがNULLの場合は、SQLを発行する
            insuranceComCd = CStr(estDtIC3070201.Tables(IFTBL_INSINFO).Rows(0).Item(CLM_INSUCOMCD))
            insuranceKind = String.Empty
        Else
            '両方ある場合は、SQLを発行する
            insuranceComCd = CStr(estDtIC3070201.Tables(IFTBL_INSINFO).Rows(0).Item(CLM_INSUCOMCD))
            insuranceKind = CStr(estDtIC3070201.Tables(IFTBL_INSINFO).Rows(0).Item(CLM_INSUKIND))
        End If

        Dim da As New SC3070202TableAdapter
        '検索処理
        Return da.GetInsuranceComInfo(dlrCd, insuranceComCd, insuranceKind)

    End Function

    ''' <summary>
    ''' 融資情報取得
    ''' </summary>
    ''' <param name="estDtIC3070201"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFinanceComInfo(ByVal estDtIC3070201 As IC3070201DataSet, _
                                       ByVal estDtSC3070202 As SC3070202DataSet, _
                                       ByVal payMethod As String) As SC3070202DataSet.SC3070202FinanceComMastDataTable

        '販売店コード
        Dim dlrCd As String = CStr(estDtIC3070201.Tables(IFTBL_ESTINFO).Rows(0).Item(CLM_DLRCD))
        '融資会社コード
        Dim financeCd As String = String.Empty

        '支払い方法データテーブルの行ループ
        For Each paymentRow As IC3070201PaymentInfoRow In estDtIC3070201.Tables(IFTBL_PAYINFO).Rows()
            '現金のレコードを使用するか、ローンのレコードを使用するかをセッションの支払い方法で判別
            If payMethod.Equals(paymentRow.Item(CLM_PAYMENTMETHOD)) Then
                If IsDBNull(paymentRow.Item(CLM_FINANCECOMCODE)) Then
                    '融資会社コードがNULLの場合、SQLを発行しない
                    Return estDtSC3070202.SC3070202FinanceComMast
                Else
                    '融資会社コードがある場合、SQLを発行
                    financeCd = CStr(paymentRow.Item(CLM_FINANCECOMCODE))
                End If
            End If
        Next

        Dim da As New SC3070202TableAdapter
        '検索処理
        Return da.GetFinanceComInfo(dlrCd, financeCd)

    End Function

    ''' <summary>
    ''' 販売店情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDlrInfo() As SC3070202DataSet.SC3070202SystemEnvSettingDataTable

        Dim sysenvDataRow As SC3070202DataSet.SC3070202SystemEnvSettingRow

        Using sysenvDataTbl As New SC3070202DataSet.SC3070202SystemEnvSettingDataTable
            sysenvDataRow = sysenvDataTbl.NewSC3070202SystemEnvSettingRow

            Dim sys As New SystemEnvSetting
            Dim sysPosition As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sys.GetSystemEnvSetting(KeisyoZengo)
            Dim sysDefoltTitle As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = sys.GetSystemEnvSetting(HonorificTitle)

            '敬称の位置を取得
            If (sysPosition Is Nothing) Then
                sysenvDataRow.NAMETITLEPOSITION = "1"
            Else
                sysenvDataRow.NAMETITLEPOSITION = sysPosition.PARAMVALUE
            End If

            '敬称のデフォルト値を取得
            If (sysDefoltTitle Is Nothing) Then
                sysenvDataRow.DEFOLTNAMETITLE = ""
            Else
                sysenvDataRow.DEFOLTNAMETITLE = sysDefoltTitle.PARAMVALUE
            End If

            sysenvDataTbl.AddSC3070202SystemEnvSettingRow(sysenvDataRow)

            Return sysenvDataTbl
        End Using
    End Function

    ''' <summary>
    ''' 見積印刷日を更新
    ''' </summary>
    ''' <param name="sessionTbl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdatePrintDate(ByVal sessionTbl As SC3070202DataSet.SC3070202SessionDataTable) As Boolean

        Dim tblDataRow As SC3070202DataSet.SC3070202SessionRow = Nothing
        If sessionTbl IsNot Nothing Then
            tblDataRow = CType(sessionTbl.Rows(0), SC3070202DataSet.SC3070202SessionRow)
        End If

        Dim staff As StaffContext = StaffContext.Current
        Dim ret As Integer = 1

        Dim da As New SC3070202TableAdapter
        '見積印刷日更新
        ret = da.UpdatePrintDate(tblDataRow.ESTIMATEID, staff.Account, PROGRAM_ID)

        '更新に失敗していたらロールバック
        If ret = 0 Then
            'エラー
            Me.Rollback = True
            Return False
        End If

        Return True

    End Function

    ''' <summary>
    ''' 取得した見積情報をデータセットにセット
    ''' </summary>
    ''' <param name="estDtIC3070201"></param>
    ''' <param name="estDtSC3070202"></param>
    ''' <param name="payMethod"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetDataSet(ByVal estDtIC3070201 As IC3070201DataSet,
                                ByVal estDtSC3070202 As SC3070202DataSet,
                                ByVal payMethod As String) As SC3070202DataSet

        '見積情報取得I/Fで取得したデータ
        Dim apiEstimateDtRow As IC3070201EstimationInfoRow
        Dim apiInsuranceDtRow As IC3070201EstInsuranceInfoRow
        'Dim apiPaymentDtRow As IC3070201PaymentInfoRow

        '結果返却用DataSet生成
        Using retSC3070202DataSet As New SC3070202DataSet

            'DataSetの中のテーブルを初期化（I/Fのやり方で統一）
            retSC3070202DataSet.Tables.Clear()

            'SC3070202の見積情報データテーブル生成
            Dim estimateInfoDt As SC3070202DataSet.SC3070202EstimateInfoDataTable
            Dim estimateInfoRow As SC3070202DataSet.SC3070202EstimateInfoRow
            estimateInfoDt = retSC3070202DataSet.SC3070202EstimateInfo
            estimateInfoRow = estimateInfoDt.NewSC3070202EstimateInfoRow

            'I/Fのデータテーブル行取得
            apiEstimateDtRow = CType(estDtIC3070201.Tables(IFTBL_ESTINFO).Rows(0), IC3070201EstimationInfoRow)
            apiInsuranceDtRow = CType(estDtIC3070201.Tables(IFTBL_INSINFO).Rows(0), IC3070201EstInsuranceInfoRow)

            'お客様名
            estimateInfoRow.CUSTOMERNM = Me.MakeCustomerTitle(estDtIC3070201, estDtSC3070202)

            '車種
            estimateInfoRow.CARTYPE = apiEstimateDtRow.SERIESNM

            'グレード/スペック
            estimateInfoRow.GRADESPEC = apiEstimateDtRow.MODELNM

            '日付
            estimateInfoRow.SYSDATE = DateTimeFunc.FormatDate(CONVERT_ID, Today)

            '販売店情報セット
            Me.SetBranchInfo(estimateInfoRow)

            'セールススタッフ
            estimateInfoRow.SALESSTAFF = StaffContext.Current.UserName

            'ボディータイプ
            estimateInfoRow.BODYTYPE = apiEstimateDtRow.BODYTYPE

            '排気量
            estimateInfoRow.DISPLACEMENT = apiEstimateDtRow.DISPLACEMENT

            '駆動
            estimateInfoRow.DRIVING = apiEstimateDtRow.DRIVESYSTEM

            'ミッション
            estimateInfoRow.MISSION = apiEstimateDtRow.TRANSMISSION

            '外装色
            estimateInfoRow.OUTERCOLOR = apiEstimateDtRow.EXTCOLOR

            '内装色
            estimateInfoRow.INNERCOLOR = apiEstimateDtRow.INTCOLOR

            '車両型号
            estimateInfoRow.CARNO = apiEstimateDtRow.MODELNUMBER

            '車両本体価格（基本価格＋外装色金額＋内装色金額）
            '2011/01/05 myose modify start
            'estimateInfoRow.BASEPRICE = apiEstimateDtRow.BASEPRICE
            estimateInfoRow.BASEPRICE = apiEstimateDtRow.BASEPRICE + apiEstimateDtRow.EXTAMOUNT + apiEstimateDtRow.INTAMOUNT
            '2011/01/05 myose modify end

            'メモ
            estimateInfoRow.MEMO = Me.GetDbNullCheckedString(apiEstimateDtRow(CLM_MEMO))

            '諸費用情報のセット
            Me.SetAmountItem(estDtIC3070201, estimateInfoRow)

            If estDtSC3070202.Tables(TBL_INSINFO).Rows.Count = 0 Then
                '保険会社
                estimateInfoRow.INSURANCECOM = String.Empty
                '種類
                estimateInfoRow.INSURANCETYPE = String.Empty
            Else
                '保険会社
                estimateInfoRow.INSURANCECOM = CStr(estDtSC3070202.Tables(TBL_INSINFO).Rows(0).Item(CLM_INSUCOMNM))
                '2011/01/16 myose modify start
                'estimateInfoRow.INSURANCETYPE = CStr(estDtSC3070202.Tables(TBL_INSINFO).Rows(0).Item(CLM_INSUKINDNM))
                '種類
                If IsDBNull((apiInsuranceDtRow(CLM_INSUKIND))) Then
                    estimateInfoRow.INSURANCETYPE = String.Empty
                Else
                    estimateInfoRow.INSURANCETYPE = CStr(estDtSC3070202.Tables(TBL_INSINFO).Rows(0).Item(CLM_INSUKINDNM))
                End If
                '2011/01/16 myose modify end
            End If

            '年額
            estimateInfoRow.YEARLYAMOUNT = Me.GetDbNullCheckedDouble(apiInsuranceDtRow(CLM_AMOUNT))

            If estDtSC3070202.Tables(TBL_FINANCEINFO).Rows.Count = 0 Then
                '融資会社
                estimateInfoRow.FINANCECOM = String.Empty
            Else
                '融資会社
                estimateInfoRow.FINANCECOM = CStr(estDtSC3070202.Tables(TBL_FINANCEINFO).Rows(0).Item(CLM_FINANCECOMNAME))
            End If

            '支払い方法データテーブルの行ループ
            For Each payment As IC3070201PaymentInfoRow In estDtIC3070201.Tables(IFTBL_PAYINFO).Rows()
                '現金のレコードを使用するか、ローンのレコードを使用するかをセッションの支払い方法で判別
                If payMethod.Equals(payment.Item(CLM_PAYMENTMETHOD)) Then

                    'お支払い方法
                    estimateInfoRow.PAYMENT = payment.PAYMENTMETHOD

                    '期間（月）
                    estimateInfoRow.PERIOD = Me.GetDbNullCheckedShort(payment(CLM_PAYMENTPERIOD))

                    '月額
                    estimateInfoRow.MONTHLY = Me.GetDbNullCheckedDouble(payment(CLM_MONTHLYPAYMENT))

                    '頭金
                    estimateInfoRow.DEPOSIT = Me.GetDbNullCheckedDouble(payment(CLM_DEPOSIT))

                    'ボーナス
                    estimateInfoRow.BONUS = Me.GetDbNullCheckedDouble(payment(CLM_BONUSPAYMENT))

                    '初回支払（日）
                    estimateInfoRow.FIRSTPAYMENT = Me.GetDbNullCheckedShort(payment(CLM_DUEDATE))
                End If
            Next

            '下取り情報のセット
            Me.SetTradeInCarInfo(estDtIC3070201, retSC3070202DataSet)

            '下取り合計額
            estimateInfoRow.TRADEINSAMMARY = amountTradeIn

            '値引き額
            estimateInfoRow.DISCOUNT = Me.GetDbNullCheckedDouble(apiEstimateDtRow(CLM_DISCOUNTPRICE))

            '納車予定日
            estimateInfoRow.DELIVERY = DateTimeFunc.FormatDate(CONVERT_ID, Me.GetDbNullCheckedDate(apiEstimateDtRow(CLM_DELIDATE)))

            'オプション情報のセット
            '2011/01/05 myose modify start
            'Me.SetOptionInfo(estDtIC3070201, retSC3070202DataSet, apiEstimateDtRow)
            Me.SetOptionInfo(estDtIC3070201, retSC3070202DataSet)
            '2011/01/05 myose modify end

            'オプションの合計額格納
            estimateInfoRow.OPTIONPRICE = amountOption

            '車両価格合計額（車両本体価格－値引き額）
            '2011/01/05 myose modify start
            'estimateInfoRow.CARPRICESAMMARY = estimateInfoRow.BASEPRICE + estimateInfoRow.OPTIONPRICE
            estimateInfoRow.CARPRICESAMMARY = estimateInfoRow.BASEPRICE - estimateInfoRow.DISCOUNT
            '2011/01/05 myose modify end

            '支払い総額（車両価格合計額＋オプション合計額＋諸費用合計額＋保険の年額－下取り車両合計額）
            '2011/01/05 myose modify start
            'estimateInfoRow.PAYMENTSAMMARY = estimateInfoRow.CARPRICESAMMARY + estimateInfoRow.EXPENSESAMMARY + estimateInfoRow.YEARLYAMOUNT - estimateInfoRow.TRADEINSAMMARY - estimateInfoRow.DISCOUNT
            estimateInfoRow.PAYMENTSAMMARY = estimateInfoRow.CARPRICESAMMARY + estimateInfoRow.OPTIONPRICE + _
                                             estimateInfoRow.EXPENSESAMMARY + estimateInfoRow.YEARLYAMOUNT - _
                                             estimateInfoRow.TRADEINSAMMARY

            '2011/01/05 myose modify end

            '追加行数
            estimateInfoRow.ADDROWCOUNT = Me.CountAddRows(estDtIC3070201)

            '見積情報テーブル行追加
            estimateInfoDt.Rows.Add(estimateInfoRow)

            '返却用データセットにテーブルを格納
            retSC3070202DataSet.Tables.Add(estimateInfoDt)

            If Not endResultId = 0 Then
                endResultId = 0
            End If

            Return retSC3070202DataSet
        End Using

    End Function

    ''' <summary>
    ''' 敬称付名前作成
    ''' </summary>
    ''' <param name="estDtIC3070201"></param>
    ''' <param name="estDtSC3070202"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function MakeCustomerTitle(ByVal estDtIC3070201 As IC3070201DataSet,
                                    ByVal estDtSC3070202 As SC3070202DataSet) As String

        'SC3070202販売店テーブルをSC3070202のデータセットから取得
        Dim dlrDt As SC3070202DataSet.SC3070202SystemEnvSettingDataTable
        dlrDt = CType(estDtSC3070202.Tables(TBL_SYSTEMINFO), SC3070202DataSet.SC3070202SystemEnvSettingDataTable)

        '敬称位置
        Dim nameTitlePos As String = CStr(dlrDt.Rows(0).Item(CLM_NAMETITLEPOSITION))

        Dim apiCustomerDtRow As IC3070201CustomerInfoRow
        apiCustomerDtRow = CType(estDtIC3070201.Tables(IFTBL_CUSTINFO).Rows(0), IC3070201CustomerInfoRow)

        'お客様名
        Dim name As String = GetDbNullCheckedString(apiCustomerDtRow(CLM_NAME))
        '敬称
        Dim nameTitle As String = String.Empty
        '顧客区分
        Dim custPart As String = Me.GetDbNullCheckedString(apiCustomerDtRow(CLM_CUSTPART))
        '顧客区分が個人のときのみ敬称表示する
        If custPart.Equals(INDIVIDUAL) Then
            '敬称取得
            nameTitle = CStr(dlrDt.Rows(0).Item(CLM_DFLTNAMETITLE))
        End If

        '敬称付き名前の組み立て
        Dim sb As New StringBuilder
        If nameTitlePos.Equals("1") Then
            If Not String.IsNullOrEmpty(nameTitle) Then
                sb.Append(nameTitle)
                sb.Append(" ")
            End If
        End If

        sb.Append(name)

        If nameTitlePos.Equals("2") Then
            If Not String.IsNullOrEmpty(nameTitle) Then
                sb.Append(" ")
                sb.Append(nameTitle)
            End If
        End If

        Return sb.ToString

    End Function

    ''' <summary>
    ''' 諸費用情報を見積情報テーブル行にセット
    ''' </summary>
    ''' <param name="estDtIC3070201"></param>
    ''' <param name="estimateInfoRow"></param>
    ''' <remarks></remarks>
    Private Sub SetAmountItem(ByVal estDtIC3070201 As IC3070201DataSet, ByVal estimateInfoRow As SC3070202DataSet.SC3070202EstimateInfoRow)

        '諸費用
        Dim amountItem1 As Double = 0
        Dim amountItem2 As Double = 0
        For Each item As IC3070201ChargeInfoRow In estDtIC3070201.Tables(IFTBL_CHARGEINFO).Rows()
            If ITEM_CODE_1.Equals(item.ITEMCODE) Then
                '諸費用１
                amountItem1 = Me.GetDbNullCheckedDouble(item(CLM_PRICE))
            ElseIf ITEM_CODE_2.Equals(item.ITEMCODE) Then
                '諸費用２
                amountItem2 = Me.GetDbNullCheckedDouble(item(CLM_PRICE))
            End If
        Next
        estimateInfoRow.ITEMPRICE1 = amountItem1
        estimateInfoRow.ITEMPRICE2 = amountItem2
        '諸費用合計額
        estimateInfoRow.EXPENSESAMMARY = estimateInfoRow.ITEMPRICE1 + estimateInfoRow.ITEMPRICE2

    End Sub

    ''' <summary>
    ''' 店舗情報を見積情報テーブル行にセット
    ''' </summary>
    ''' <param name="estimateInfoRow"></param>
    ''' <remarks></remarks>
    Private Sub SetBranchInfo(ByVal estimateInfoRow As SC3070202DataSet.SC3070202EstimateInfoRow)
        '店舗情報取得
        Dim branchBiz As New Branch
        Dim staff As StaffContext = StaffContext.Current
        Dim branchDt As BranchDataSet.BRANCHRow = branchBiz.GetBranch(staff.DlrCD, staff.BrnCD)

        '販売店
        estimateInfoRow.BRANCHNM = branchDt.STRNM_LOCAL

        '電話番号
        estimateInfoRow.TELNO = branchDt.SALTEL

    End Sub

    ''' <summary>
    ''' 下取り車両の情報をセット
    ''' </summary>
    ''' <param name="estDtIC3070201"></param>
    ''' <param name="rtnDataSet"></param>
    ''' <remarks></remarks>
    Private Sub SetTradeInCarInfo(ByVal estDtIC3070201 As IC3070201DataSet, ByVal rtnDataSet As SC3070202DataSet)

        '下取り一覧取得
        Dim tradeInCarInfoDt As SC3070202DataSet.SC3070202EstTradeInCarInfoDataTable
        Dim tradeInCarInfoRow As SC3070202DataSet.SC3070202EstTradeInCarInfoRow
        tradeInCarInfoDt = rtnDataSet.SC3070202EstTradeInCarInfo

        For Each tradeIn As IC3070201TradeincarInfoRow In estDtIC3070201.Tables(IFTBL_TRADEINCARINFO).Rows()
            '下取り価格の合計額に追加する
            amountTradeIn = amountTradeIn + tradeIn.ASSESSEDPRICE

            '下取り車両情報を保持する
            tradeInCarInfoRow = tradeInCarInfoDt.NewSC3070202EstTradeInCarInfoRow

            tradeInCarInfoRow.VEHICLENAME = tradeIn.VEHICLENAME
            tradeInCarInfoRow.ASSESSEDPRICE = tradeIn.ASSESSEDPRICE

            tradeInCarInfoDt.Rows.Add(tradeInCarInfoRow)

        Next

        '返却用データセットにテーブルを格納
        rtnDataSet.Tables.Add(tradeInCarInfoDt)
    End Sub

    '2011/01/05 myose modify start
    ' ''' <summary>
    ' ''' オプションの情報をセット
    ' ''' </summary>
    ' ''' <param name="estDtIC3070201"></param>
    ' ''' <param name="rtnDataSet"></param>
    ' ''' <param name="apiEstDtRow"></param>
    ' ''' <remarks></remarks>
    'Private Sub SetOptionInfo(ByVal estDtIC3070201 As IC3070201DataSet, ByVal rtnDataSet As SC3070202DataSet, _
    '                          ByVal apiEstDtRow As IC3070201EstimationInfoRow)

    '    'オプション一覧取得
    '    Dim optionInfoDt As SC3070202DataSet.SC3070202EstVclOptionInfoDataTable
    '    Dim optionInfoRow As SC3070202DataSet.SC3070202EstVclOptionInfoRow
    '    optionInfoDt = rtnDataSet.SC3070202EstVclOptionInfo

    '    '外装色に金額がある場合、オプションに追加
    '    If apiEstDtRow.EXTAMOUNT > 0 Then
    '        optionInfoRow = optionInfoDt.NewSC3070202EstVclOptionInfoRow

    '        optionInfoRow.OPTIONNAME = apiEstDtRow.EXTCOLOR
    '        optionInfoRow.PRICE = apiEstDtRow.EXTAMOUNT

    '        optionInfoDt.Rows.Add(optionInfoRow)

    '        'オプションの合計額に追加
    '        amountOption += apiEstDtRow.EXTAMOUNT
    '        'オプションの合計数に追加
    '        optionCount += 1
    '    End If

    '    '内装色に金額がある場合、オプションに追加
    '    If apiEstDtRow.INTAMOUNT > 0 Then
    '        optionInfoRow = optionInfoDt.NewSC3070202EstVclOptionInfoRow

    '        optionInfoRow.OPTIONNAME = apiEstDtRow.INTCOLOR
    '        optionInfoRow.PRICE = apiEstDtRow.INTAMOUNT

    '        optionInfoDt.Rows.Add(optionInfoRow)

    '        'オプションの合計額に追加
    '        amountOption += apiEstDtRow.INTAMOUNT
    '        'オプションの合計数に追加
    '        optionCount += 1
    '    End If

    '    For Each op As IC3070201VclOptionInfoRow In estDtIC3070201.Tables(IFTBL_VCLOPTIONINFO).Rows()
    '        'オプション価格の合計額に追加
    '        amountOption = amountOption + op.PRICE + Me.GetDbNullCheckedDouble(op(CLM_INSTALLCOST))

    '        'オプション情報を保持する
    '        optionInfoRow = optionInfoDt.NewSC3070202EstVclOptionInfoRow

    '        optionInfoRow.OPTIONNAME = op.OPTIONNAME
    '        optionInfoRow.PRICE = op.PRICE + Me.GetDbNullCheckedDouble(op(CLM_INSTALLCOST))

    '        optionInfoDt.Rows.Add(optionInfoRow)
    '    Next

    '    '返却用データセットにテーブルを格納
    '    rtnDataSet.Tables.Add(optionInfoDt)
    'End Sub

    ''' <summary>
    ''' オプションの情報をセット
    ''' </summary>
    ''' <param name="estDtIC3070201"></param>
    ''' <param name="rtnDataSet"></param>
    ''' <remarks></remarks>
    Private Sub SetOptionInfo(ByVal estDtIC3070201 As IC3070201DataSet, ByVal rtnDataSet As SC3070202DataSet)

        'オプション一覧取得
        Dim optionInfoDt As SC3070202DataSet.SC3070202EstVclOptionInfoDataTable
        Dim optionInfoRow As SC3070202DataSet.SC3070202EstVclOptionInfoRow
        optionInfoDt = rtnDataSet.SC3070202EstVclOptionInfo

        For Each op As IC3070201VclOptionInfoRow In estDtIC3070201.Tables(IFTBL_VCLOPTIONINFO).Rows()
            'オプション価格の合計額に追加
            amountOption = amountOption + op.PRICE + Me.GetDbNullCheckedDouble(op(CLM_INSTALLCOST))

            'オプション情報を保持する
            optionInfoRow = optionInfoDt.NewSC3070202EstVclOptionInfoRow

            optionInfoRow.OPTIONNAME = op.OPTIONNAME
            optionInfoRow.PRICE = op.PRICE + Me.GetDbNullCheckedDouble(op(CLM_INSTALLCOST))

            optionInfoDt.Rows.Add(optionInfoRow)
        Next

        '返却用データセットにテーブルを格納
        rtnDataSet.Tables.Add(optionInfoDt)
    End Sub
    '2011/01/05 myose modify end

    ''' <summary>
    ''' 追加行数を数える
    ''' </summary>
    ''' <param name="estDtIC3070201"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CountAddRows(ByVal estDtIC3070201 As IC3070201DataSet) As String

        'I/Fオプションテーブルの行数をカウント
        optionCount += estDtIC3070201.Tables(IFTBL_VCLOPTIONINFO).Rows.Count
        'I/F下取り車両テーブルの行数をカウント
        Dim tradeInCount As Integer = estDtIC3070201.Tables(IFTBL_TRADEINCARINFO).Rows.Count
        '各々の追加行数を計算（表示行数 - 標準行数）
        Dim optionAddRowCount As Integer = optionCount - MIN_OPTION
        Dim tradeInAddRowCount As Integer = tradeInCount - MIN_TRADEIN

        Dim rtnVal As String = String.Empty

        If 0 < optionAddRowCount AndAlso tradeInAddRowCount <= optionAddRowCount Then
            'オプション追加行数が標準行数より大きい、かつ下取り車両の追加行数より大きい場合
            rtnVal = optionAddRowCount.ToString(CultureInfo.CurrentCulture())
        ElseIf 0 < tradeInAddRowCount AndAlso optionAddRowCount <= tradeInAddRowCount Then
            '下取り車両追加行数が標準行数より大きい、かつオプションの追加行数より大きい場合
            rtnVal = tradeInAddRowCount.ToString(CultureInfo.CurrentCulture())
        Else
            rtnVal = "0"
        End If

        Return rtnVal

    End Function

    ''' <summary>
    ''' DBNULLチェックをした値を返却する（double）
    ''' </summary>
    ''' <param name="objColumn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDbNullCheckedDouble(ByVal objColumn As Object) As Double

        Dim rtnVal As Double = 0

        If Not IsDBNull(objColumn) Then
            rtnVal = CDbl(objColumn)
        End If

        Return rtnVal

    End Function

    ''' <summary>
    ''' DBNULLチェックをした値を返却する（String）
    ''' </summary>
    ''' <param name="objColumn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDbNullCheckedString(ByVal objColumn As Object) As String

        Dim rtnVal As String = String.Empty

        If Not IsDBNull(objColumn) Then
            rtnVal = CStr(objColumn)
        End If

        Return rtnVal

    End Function

    ''' <summary>
    ''' DBNULLチェックをした値を返却する（Short）
    ''' </summary>
    ''' <param name="objColumn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDbNullCheckedShort(ByVal objColumn As Object) As Short

        Dim rtnVal As Short = 0

        If Not IsDBNull(objColumn) Then
            rtnVal = CShort(objColumn)
        End If

        Return rtnVal

    End Function

    ''' <summary>
    ''' DBNULLチェックをした値を返却する（Date）
    ''' </summary>
    ''' <param name="objColumn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetDbNullCheckedDate(ByVal objColumn As Object) As Date

        Dim rtnVal As Date = Date.MinValue

        If Not IsDBNull(objColumn) Then
            rtnVal = CType(objColumn, Date)
        End If

        Return rtnVal

    End Function

#End Region

End Class
