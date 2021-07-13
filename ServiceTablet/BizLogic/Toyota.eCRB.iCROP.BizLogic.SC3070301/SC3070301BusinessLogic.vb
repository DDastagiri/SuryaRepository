Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Globalization
Imports System.Web

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.BizLogic
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.Estimate.Order.DataAccess
Imports Toyota.eCRB.Estimate.Quotation.DataAccess.IC3070201DataSet

''' <summary>
''' 契約書印刷のビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class SC3070301BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3070301BusinessLogic

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        '処理なし
    End Sub

    ''' <summary>
    ''' プログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PROGRAM_ID As String = "SC3070301"
    ''' <summary>
    ''' 契約状況フラグ　契約済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACTFLG_CONTRACT As String = "1"
    ''' <summary>
    ''' 契約状況フラグ　キャンセル
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACTFLG_CANCEL As String = "2"
    ''' <summary>
    ''' 費用項目コード　手続き費用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ITEM_CODE_PURCHASE As String = "1"
    ''' <summary>
    ''' 費用項目コード　購入税
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ITEM_CODE_REGISTRAION As String = "2"
    ''' <summary>
    ''' 実行モード　見積情報取得用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MODE_ALL As Integer = 0
    ''' <summary>
    ''' 契約顧客種別 所有者
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACTCUSTTYPE_DEALER As String = "1"
    ''' <summary>
    ''' 支払区分　現金
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAYMENTMETHOD_MONEY As String = "1"
    ''' <summary>
    ''' 支払区分　ローン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAYMENTMETHOD_LOAN As String = "2"
    ''' <summary>
    ''' メッセージID　更新失敗
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MSG_ID_UPDATE As Integer = 902
    ''' <summary>
    ''' メッセージID　TACT連携失敗
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MSG_ID_TACT As Integer = 901
    ''' <summary>
    ''' メッセージID　見積情報のデータなし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MSG_ID_IF As Integer = 903
    ''' <summary>
    ''' Dictinay　key 終了コード
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DIC_KEY_ID As String = "ID"
    ''' <summary>
    ''' Dictinay　key メッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DIC_KEY_MSG As String = "MSG"
    ''' <summary>
    ''' Dictinay　key 契約書NO
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DIC_KEY_NO As String = "NO"
    ''' <summary>
    ''' 変換ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONVERT_ID As Integer = 3
    ''' <summary>
    ''' TBL_DLRENVSETTINGマスタのパラメータ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DLRENVSETTING_TACT As String = "TACT_ORDER_PATH"
    ''' <summary>
    ''' 見積情報DBのカラム名　値引き額
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISCOUNTPRICE As String = "DISCOUNTPRICE"
    ''' <summary>
    ''' 見積情報DBのカラム名　納車日
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DELIDATE As String = "DELIDATE"
    ''' <summary>
    ''' 納車日　表示用
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DELIDATE_SCREEN As String = "DELIDATE_SCREEN"
    ''' <summary>
    ''' 保険情報DBのカラム名　金額
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AMOUNT As String = "AMOUNT"
    ''' <summary>
    ''' DMS側の日付書式パラーメータ名
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DMS_DATETIMEFORMAT As String = "DMS_DATETIMEFORMAT"
    ''' <summary>
    ''' ログ出力メッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERROR_MSG As String = "Tact Error : ReturnId = "

    ''' <summary>
    ''' 契約情報の取得
    ''' </summary>
    ''' <param name="tbl">セッションデータテーブル</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>契約情報データテーブル</returns>
    ''' <remarks></remarks>
    Public Shared Function GetConstractInfo(ByVal tbl As SC3070301DataSet.SessionDataTable,
                                     ByRef msgId As Integer) As SC3070301DataSet.ConstractInfoDataTable

        'セッション情報取得
        Dim tblDataRow As SC3070301DataSet.SessionRow = Nothing
        If tbl IsNot Nothing Then
            tblDataRow = CType(tbl.Rows(0), SC3070301DataSet.SessionRow)
        End If

        '見積情報取得 
        Dim apiBiz As New IC3070201BusinessLogic
        Dim apiDt As IC3070201DataSet
        apiDt = apiBiz.GetEstimationInfo(tblDataRow.ESTIMATEID, MODE_ALL)
        If Not apiBiz.ResultId = 0 Then
            'エラー 契約情報がありません。
            msgId = MSG_ID_IF
            Return Nothing
        End If

        If apiDt.Tables("IC3070201EstimationInfo").Rows.Count = 0 Then
            'エラー　契約情報がありません。
            msgId = MSG_ID_IF
            Return Nothing
        End If

        '店舗情報の取得
        Dim branchBiz As New Branch
        Dim staff As StaffContext = StaffContext.Current
        Dim branchDt As BranchDataSet.BRANCHRow = branchBiz.GetBranch(staff.DlrCD, staff.BrnCD)

        '外装色コード取得
        Dim colorDt As SC3070301DataSet.MstextEriorDataTable = SC3070301TableAdapter.GetColorCode(CStr(apiDt.Tables("IC3070201EstimationInfo").Rows(0).Item("MODELCD")), CStr(apiDt.Tables("IC3070201EstimationInfo").Rows(0).Item("EXTCOLORCD")))
        msgId = 0

        Return SetContractInfo(apiDt, branchDt, tblDataRow.PAYMENTMETHOD, colorDt, msgId)
    End Function

    ''' <summary>
    ''' 契約書印刷フラグ更新
    ''' </summary>
    ''' <param name="tbl">セッションデータテーブル</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>更新結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdatePrintFlg(ByVal tbl As SC3070301DataSet.SessionDataTable,
                            ByRef msgId As Integer) As Boolean Implements ISC3070301BusinessLogic.UpdatePrintFlg

        'セッション情報の取得
        Dim tblDataRow As SC3070301DataSet.SessionRow = Nothing
        If tbl IsNot Nothing Then
            tblDataRow = CType(tbl.Rows(0), SC3070301DataSet.SessionRow)
        End If

        Dim staff As StaffContext = StaffContext.Current

        '更新に失敗していたらメッセージを表示
        Try
            '印刷フラグを更新
            If SC3070301TableAdapter.UpdatePrintFlg(tblDataRow.ESTIMATEID, staff.Account, PROGRAM_ID) = 0 Then
                'エラー
                msgId = MSG_ID_UPDATE
                Return False
            End If
        Catch ex As OracleExceptionEx
            msgId = MSG_ID_UPDATE
            Return False
        End Try

        If msgId = 0 Then
            msgId = 0
        End If

        Return True

    End Function

    ''' <summary>
    ''' 契約情報の更新（実行ボタン押下時）
    ''' </summary>
    ''' <param name="tbl">セッションデータテーブル</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <param name="constractDB">契約情報データテーブル</param>
    ''' <returns>更新結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdateConstractInfoSend(ByVal tbl As SC3070301DataSet.SessionDataTable,
                                            ByRef msgId As Integer,
                                            ByVal constractDB As SC3070301DataSet.ConstractInfoDataTable) As Boolean Implements ISC3070301BusinessLogic.UpdateConstractInfoSend

        Dim tblDataRow As SC3070301DataSet.SessionRow = Nothing
        If tbl IsNot Nothing Then
            tblDataRow = CType(tbl.Rows(0), SC3070301DataSet.SessionRow)
        End If

        Dim staff As StaffContext = StaffContext.Current

        '環境設定の取得
        Dim dealerEnvBiz As New DealerEnvSetting
        Dim dealerEnvDt As DlrEnvSettingDataSet.DLRENVSETTINGRow = dealerEnvBiz.GetEnvSetting("XXXXX", DLRENVSETTING_TACT)

        'TACT連携
        Dim res As Dictionary(Of String, String) = WebClient.RequestHttp(staff, tblDataRow, constractDB, dealerEnvDt)

        Dim constractNo As String = String.Empty

        If res.ContainsKey(DIC_KEY_ID) Then
            If Not "0".Equals(res.Item(DIC_KEY_ID)) Then
                msgId = MSG_ID_TACT
                Logger.Error(ERROR_MSG & res.Item(DIC_KEY_ID))
                Return False
            Else
                If res.ContainsKey(DIC_KEY_NO) Then
                    constractNo = res.Item(DIC_KEY_NO)
                End If
            End If
        Else
            msgId = MSG_ID_TACT
            Return False
        End If

        '契約書Noセット
        constractDB.Rows(0).Item("CONTRACTNO") = constractNo

        Dim payment As String = String.Empty
        If PAYMENTMETHOD_MONEY.Equals(tblDataRow.PAYMENTMETHOD) Then
            payment = PAYMENTMETHOD_LOAN
        Else
            payment = PAYMENTMETHOD_MONEY
        End If

        Dim dateNow As Date = Date.Now()
        Dim commit As Boolean = True
        Try
            '更新処理 
            '支払方法の削除フラグ更新
            If SC3070301TableAdapter.UpdateDelFlg(tblDataRow.ESTIMATEID, payment, staff.Account, PROGRAM_ID) = 0 Then
                commit = False
            End If

            '契約情報の更新
            If SC3070301TableAdapter.UpdateConstractInfo(tblDataRow.ESTIMATEID, CONTRACTFLG_CONTRACT, dateNow, constractNo, staff.Account, PROGRAM_ID) = 0 Then
                commit = False
            End If

            '更新処理がどちらかでも失敗していたらメッセージを表示
            If Not commit Then
                msgId = MSG_ID_UPDATE
                Return False
            End If

        Catch ex As OracleExceptionEx
            msgId = MSG_ID_UPDATE
            Return False
        End Try

        If msgId = 0 Then
            msgId = 0
        End If

        Return True
    End Function


    ''' <summary>
    ''' 契約情報の更新(キャンセルボタン押下時）
    ''' </summary>
    ''' <param name="tbl">セッションデータテーブル</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>更新結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdateConstractInfoCancel(ByVal tbl As SC3070301DataSet.SessionDataTable,
                                              ByRef msgId As Integer) As Boolean Implements ISC3070301BusinessLogic.UpdateConstractInfoCancel

        Dim tblDataRow As SC3070301DataSet.SessionRow = Nothing
        If tbl IsNot Nothing Then
            tblDataRow = CType(tbl.Rows(0), SC3070301DataSet.SessionRow)
        End If

        Dim staff As StaffContext = StaffContext.Current

        '更新に失敗していたらメッセージを表示
        Try
            '契約情報の更新
            If SC3070301TableAdapter.UpdateConstractInfo(tblDataRow.ESTIMATEID, CONTRACTFLG_CANCEL, Date.MinValue, Nothing, staff.Account, PROGRAM_ID) = 0 Then
                msgId = MSG_ID_UPDATE
                Return False
            End If
        Catch ex As OracleExceptionEx
            msgId = MSG_ID_UPDATE
            Return False
        End Try

        If msgId = 0 Then
            msgId = 0
        End If

        Return True

    End Function


    ''' <summary>
    ''' 取得した契約情報をデータテーブルにセットします。
    ''' </summary>
    ''' <param name="apiDt">見積情報データセット</param>
    ''' <param name="branchDt">店舗情報データテーブルの行</param>
    ''' <param name="method">支払方法区分</param>
    ''' <param name="colorDt">色情報データテーブル</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>契約情報データテーブル</returns>
    ''' <remarks></remarks>
    Private Shared Function SetContractInfo(ByVal apiDt As IC3070201DataSet,
                                      ByVal branchDt As BranchDataSet.BRANCHRow,
                                      ByVal method As String,
                                      ByVal colorDt As SC3070301DataSet.MstextEriorDataTable,
                                      ByRef msgId As Integer) As SC3070301DataSet.ConstractInfoDataTable

        Dim contractDt As SC3070301DataSet.ConstractInfoDataTable
        Dim rtnDt As SC3070301DataSet.ConstractInfoRow
        Dim apiDtRow As IC3070201EstimationInfoRow
        Dim apiInsuranceDtRow As IC3070201EstInsuranceInfoRow

        Using dt As New SC3070301DataSet.ConstractInfoDataTable
            rtnDt = dt.NewConstractInfoRow
            apiDtRow = CType(apiDt.Tables("IC3070201EstimationInfo").Rows(0), IC3070201EstimationInfoRow)
            apiInsuranceDtRow = CType(apiDt.Tables("IC3070201EstInsuranceInfo").Rows(0), IC3070201EstInsuranceInfoRow)

            '店舗名称
            rtnDt.STRNM_LOCAL = branchDt.STRNM_LOCAL

            '住所
            rtnDt.ADDR_LOCAL = branchDt.ADDR1_LOCAL

            'セールスTEL番号
            rtnDt.SALTEL = branchDt.SALTEL

            'セールスFAX番号
            rtnDt.SALFAXNO = branchDt.SALFAXNO

            'サービスTEL番号
            rtnDt.SRVSTEL = branchDt.SRVSTEL

            '見積管理ID
            rtnDt.ESTIMATEID = apiDtRow.ESTIMATEID

            For Each customer As IC3070201CustomerInfoRow In apiDt.Tables("IC3070201CustomerInfo").Rows()
                If CONTRACTCUSTTYPE_DEALER.Equals(customer.Item("CONTRACTCUSTTYPE")) Then
                    '買主区分
                    rtnDt.BUYERCUSTPART = GetCustomerDtCol(customer, "CUSTPART")

                    '買主名
                    rtnDt.BUYERNAME = GetCustomerDtCol(customer, "NAME")

                    '買主ID
                    rtnDt.BUYERSOCIALID = GetCustomerDtCol(customer, "SOCIALID")

                    '買主郵便番号
                    rtnDt.BUYERZIPCODE = GetCustomerDtCol(customer, "ZIPCODE")

                    '買主住所
                    rtnDt.BUYERADDRESS = GetCustomerDtCol(customer, "ADDRESS")

                    '買主電話番号
                    rtnDt.BUYERTELNO = GetCustomerDtCol(customer, "TELNO")

                    '買主携帯番号
                    rtnDt.BUYERMOBILE = GetCustomerDtCol(customer, "MOBILE")

                    '買主FAX
                    rtnDt.BUYERFAXNO = GetCustomerDtCol(customer, "FAXNO")

                    '買主E-Mail
                    rtnDt.BUYERMAIL = GetCustomerDtCol(customer, "EMAIL")

                Else
                    '名義人区分
                    rtnDt.HOLDERCUSTPART = GetCustomerDtCol(customer, "CUSTPART")

                    '名義人名
                    rtnDt.HOLDERNAME = GetCustomerDtCol(customer, "NAME")

                    '名義人ID
                    rtnDt.HOLDERSOCIALID = GetCustomerDtCol(customer, "SOCIALID")

                    '名義人郵便番号
                    rtnDt.HOLDERZIPCODE = GetCustomerDtCol(customer, "ZIPCODE")

                    '名義人住所
                    rtnDt.HOLDERADDRESS = GetCustomerDtCol(customer, "ADDRESS")

                    '名義人電話番号
                    rtnDt.HOLDERTELNO = GetCustomerDtCol(customer, "TELNO")

                    '名義人携帯番号
                    rtnDt.HOLDERMOBILE = GetCustomerDtCol(customer, "MOBILE")

                    '名義人FAX
                    rtnDt.HOLDERFAXNO = GetCustomerDtCol(customer, "FAXNO")

                    '名義人E-Mail
                    rtnDt.HOLDERMAIL = GetCustomerDtCol(customer, "EMAIL")
                End If
            Next

            '車両型号
            rtnDt.MODELNUMBER = apiDtRow.MODELNUMBER

            '車名コード
            rtnDt.SERIESCD = apiDtRow.SERIESCD

            '車名
            rtnDt.SERIESNM = apiDtRow.SERIESNM

            'グレード
            rtnDt.MODELNM = apiDtRow.MODELNM

            'サフィックス
            rtnDt.SUFFIXCD = apiDtRow.SUFFIXCD

            '外装コード
            If colorDt.Count = 0 Then
                rtnDt.EXTCOLORCD = ""
            Else
                rtnDt.EXTCOLORCD = CStr(colorDt.Rows(0).Item("COLOR_CD"))
            End If

            '外装色
            rtnDt.EXTCOLOR = apiDtRow.EXTCOLOR

            '内装色
            rtnDt.INTCOLOR = apiDtRow.INTCOLOR

            '車両価格
            rtnDt.BASEPRICE = apiDtRow.BASEPRICE

            'オプション価格
            Dim amountOption As Double = 0
            For Each op As IC3070201VclOptionInfoRow In apiDt.Tables("IC3070201VclOptionInfo").Rows()
                Dim installCost As Double = 0
                If Not IsDBNull(op.Item("INSTALLCOST")) Then
                    installCost = op.INSTALLCOST
                End If
                amountOption = amountOption + op.PRICE + installCost
            Next
            rtnDt.OPTIONPRICE = amountOption

            '保険費用
            rtnDt.INSUPRICE = CDbl(GetInsuranceDtCol(apiInsuranceDtRow, AMOUNT))

            '諸費用
            rtnDt.ITEMPRICE1 = 0
            rtnDt.ITEMPRICE2 = 0
            For Each item As IC3070201ChargeInfoRow In apiDt.Tables("IC3070201ChargeInfo").Rows()
                If ITEM_CODE_PURCHASE.Equals(item.ITEMCODE) Then
                    rtnDt.ITEMPRICE1 = item.PRICE
                ElseIf ITEM_CODE_REGISTRAION.Equals(item.ITEMCODE) Then
                    rtnDt.ITEMPRICE2 = item.PRICE
                End If
            Next

            '値引き額
            rtnDt.DISCOUNTPRICE = CDbl(GetApiDtCol(apiDtRow, DISCOUNTPRICE, "0"))

            'お支払方法
            rtnDt.DEPOSIT = 0
            rtnDt.DUEDATE = ""
            For Each payment As IC3070201PaymentInfoRow In apiDt.Tables("IC3070201PaymentInfo").Rows()
                If method.Equals(payment.Item("PAYMENTMETHOD")) Then
                    '頭金
                    rtnDt.DEPOSIT = payment.DEPOSIT
                    '支払期限
                    If Not IsDBNull(payment.Item("DUEDATE")) Then
                        rtnDt.DUEDATE = CStr(payment.DUEDATE)
                    End If
                End If
            Next

            '納車日 TACT連携用
            rtnDt.DELIDATE = GetApiDtCol(apiDtRow, DELIDATE, "0")

            '納車日 表示用
            rtnDt.DELIDATE_SCREEN = GetApiDtCol(apiDtRow, DELIDATE, "1")

            'メモ
            rtnDt.MEMO = GetApiDtCol(apiDtRow, "MEMO", "0")

            '保険区分
            rtnDt.INSUKIND = GetInsuranceDtCol(apiInsuranceDtRow, "INSUDVS")

            '未取引客ID
            rtnDt.CRCUSTID = apiDtRow.CRCUSTID

            '契約状況フラグ
            rtnDt.CONTRACTFLG = apiDtRow.CONTRACTFLG

            '契約書印刷フラグ
            rtnDt.CONTPRINTFLG = apiDtRow.CONTPRINTFLG

            '外装追加費用
            rtnDt.EXTAMOUNT = apiDtRow.EXTAMOUNT

            '内装追加費用
            rtnDt.INTAMOUNT = apiDtRow.INTAMOUNT

            '表示用車両価格（本体車両価格 - 値引き　+　外装追加費用　+　内装追加費用)
            rtnDt.VCLPRICE = rtnDt.BASEPRICE + rtnDt.EXTAMOUNT + rtnDt.INTAMOUNT - rtnDt.DISCOUNTPRICE

            'お支払い合計 
            rtnDt.PRICEAMOUNT = rtnDt.VCLPRICE + rtnDt.OPTIONPRICE + rtnDt.ITEMPRICE1 + rtnDt.ITEMPRICE2 + rtnDt.INSUPRICE

            '残金
            rtnDt.ONLYPAY = rtnDt.PRICEAMOUNT - rtnDt.DEPOSIT

            '契約書No
            rtnDt.CONTRACTNO = GetApiDtCol(apiDtRow, "CONTRACTNO", "0")

            dt.Rows.Add(rtnDt)
            contractDt = dt
        End Using

        If Not msgId = 0 Then
            msgId = 0
        End If

        Return contractDt
    End Function

    ''' <summary>
    ''' カラムのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="customer">顧客情報のカラム</param>
    ''' <param name="colName">カラム名</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Shared Function GetCustomerDtCol(ByVal customer As IC3070201CustomerInfoRow,
                                     ByVal colName As String) As String
        'DBNullの場合　""を返却
        If IsDBNull(customer.Item(colName)) Then
            Return ""
        End If

        Return CStr(customer.Item(colName))
    End Function

    ''' <summary>
    ''' 契約情報DtカラムのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="apiDtRow">契約情報Dt</param>
    ''' <param name="colName">カラム名</param>
    ''' <param name="flg">日付書式判定</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Shared Function GetApiDtCol(ByVal apiDtRow As IC3070201EstimationInfoRow,
                                     ByVal colName As String,
                                     ByVal flg As String) As String
        'DBNullの場合　""を返却
        If IsDBNull(apiDtRow.Item(colName)) Then
            If DISCOUNTPRICE.Equals(colName) Then
                Return "0"
            End If

            Return ""
        Else
            If DELIDATE.Equals(colName) Then
                If "0".Equals(flg) Then
                    'DMSの日付書式を取得
                    Dim sysEnv As New SystemEnvSetting
                    Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
                    sysEnvRow = sysEnv.GetSystemEnvSetting(DMS_DATETIMEFORMAT)
                    Dim dmsDatetimeFormt As String = sysEnvRow.PARAMVALUE
                    Return apiDtRow.DELIDATE.ToString(dmsDatetimeFormt, CultureInfo.CurrentCulture)
                Else
                    Return DateTimeFunc.FormatDate(CONVERT_ID, apiDtRow.DELIDATE)
                End If
            End If
        End If

        Return CStr(apiDtRow.Item(colName))
    End Function

    ''' <summary>
    ''' 保険情報DtのDBNull判定処理を行います。
    ''' </summary>
    ''' <param name="apiInsuranceDtRow">保険情報のカラム</param>
    ''' <param name="colName">カラム名</param>
    ''' <returns>DBNullの場合は""を返却</returns>
    ''' <remarks></remarks>
    Private Shared Function GetInsuranceDtCol(ByVal apiInsuranceDtRow As IC3070201EstInsuranceInfoRow,
                                     ByVal colName As String) As String
        'DBNullの場合　""を返却
        If IsDBNull(apiInsuranceDtRow.Item(colName)) Then
            If AMOUNT.Equals(colName) Then
                Return "0"
            End If

            Return ""
        End If

        Return CStr(apiInsuranceDtRow.Item(colName))
    End Function

End Class



