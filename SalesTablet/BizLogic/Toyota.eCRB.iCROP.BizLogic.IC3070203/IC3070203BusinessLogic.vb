'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3070203BusinessLogic.vb
'─────────────────────────────────────
'機能： 見積登録I/F
'補足： 
'作成： 2013/12/10 TCS 森
'更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール）
'更新： 2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展
'─────────────────────────────────────

Imports System.Text
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.Estimate.Quotation.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

''' <summary>
''' 見積情報登録I/F
''' ビジネスロジック層クラス
''' </summary>
''' <remarks></remarks>
Public Class IC3070203BusinessLogic
    Inherits BaseBusinessComponent

#Region "定数"
    ''' <summary>
    ''' エラーコード：処理正常終了(該当データ有）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrCodeSuccess As Short = 0
    ''' <summary>
    ''' エラーコード：システムエラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrCodeSys As Short = 9999

    ''' <summary>
    ''' エラーコード：モジュール固有エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrCodeOther As Short = 6000

    ''' <summary>
    ''' エラーコード：見積情報存在エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrCodeEstimate As Short = 1

    ''' <summary>
    ''' エラーコード：見積支払方法エラー
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ErrCodeEstPayment As Short = 2

    ''' <summary>
    ''' 顧客種別：所有者
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CustTypeOwner As String = "1"

    ''' <summary>
    ''' 顧客種別：使用者
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CustTypeUser As String = "2"

    ''' <summary>
    ''' 支払方法区分：現金
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAYMENTMETHOD_CASH As String = "1"

    ''' <summary>
    ''' 支払方法区分：ローン
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PAYMENTMETHOD_LONE As String = "2"

    ''' <summary>
    ''' デフォルト言語：英語
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEFAULT_LANG_EN As String = "en"

    ''' <summary>
    ''' 英語フラグ：0:現地語を使用してコンタクト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENG_FLG_OTHER As String = "0"

    ''' <summary>
    ''' 英語フラグ：1:英語を使用してコンタクト
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ENG_FLG_ENG As String = "1"

    ''' <summary>
    ''' 性別（XML入力値）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const GENDER_XML_MALE As String = "0" '男
    Private Const GENDER_XML_FEMALE As String = "1" '女
    Private Const GENDER_XML_BOTH As String = "2" '両方
    Private Const GENDER_XML_OTHER As String = " " 'その他

    ''' <summary>
    ''' 性別（顧客TBL）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const GENDER_CST_MALE As String = "0" '男
    Private Const GENDER_CST_FEMALE As String = "1" '女
    Private Const GENDER_CST_OTHER As String = "2" '不明
    Private Const GENDER_CST_BOTH As String = "3" '両方

    ''' <summary>
    ''' 見積実績フラグ:0：実績なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EST_ACT_FLG_NO As String = "0"
    ''' <summary>
    ''' 見積実績フラグ:1:実績あり
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EST_ACT_FLG_YES As String = "1"

    ''' <summary>
    ''' 見積実績フラグ:0:未印刷
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTPRINTFLG_NO As String = "0"
    ''' <summary>
    ''' 見積実績フラグ:1:印刷済
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTPRINTFLG_YES As String = "1"

    ''' <summary>
    ''' タグ有無：1：有
    ''' </summary>
    ''' <remarks></remarks>
    Public Const TagPresenceYes As String = "1"

    ''' <summary>
    ''' タグ有無：0：無
    ''' </summary>
    ''' <remarks></remarks>
    Public Const TagPresenceNo As String = "0"

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START

    ''' <summary>
    ''' 契約条件変更フラグがON
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACT_COND_CHG_FLG_ON As String = "1"

    ''' <summary>
    ''' 契約条件変更フラグがOFF
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONTRACT_COND_CHG_FLG_OFF As String = "0"

    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END
#End Region

#Region "メンバ変数"
    ''' <summary>
    ''' 終了コード
    ''' </summary>
    ''' <remarks></remarks>
    Private prpResultId As Short = 0

    ''' <summary>
    ''' 作成日
    ''' </summary>
    ''' <remarks></remarks>
    Private prpCreateDate As Date = DateTime.MinValue

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
            Return prpResultId
        End Get
    End Property
#End Region

#Region "Publicメソッド"
    ''' <summary>
    ''' 見積情報を登録する。
    ''' </summary>
    ''' <param name="estInfoDataSet">見積情報データセット</param>
    ''' <returns>見積情報登録結果データテーブル</returns>
    ''' <remarks>
    ''' </remarks>
    <EnableCommit()>
    Public Function SetEstimationInfo(ByVal estInfoDataSet As IC3070203DataSet
                                     ) As IC3070203DataSet.IC3070203EstResultDataTable

        ' 引数チェックはプレゼンテーション層で実施する
        If estInfoDataSet Is Nothing Then
            Throw New ArgumentException("Exception Occured", "estInfoDataSet")
        End If

        ' 見積情報登録結果データテーブル
        Dim estResultDT As IC3070203DataSet.IC3070203EstResultDataTable _
            = estInfoDataSet.IC3070203EstResult

        ' 見積情報登録結果データテーブル行
        Dim estResultRow As IC3070203DataSet.IC3070203EstResultRow _
            = estResultDT.NewIC3070203EstResultRow()

        Dim estPaymentInfo As IC3070203DataSet.IC3070203EstPaymentInfoRow = Nothing
        Dim estInsuranceInfo As IC3070203DataSet.IC3070203EstInsuranceInfoRow = Nothing
        Dim estUpdCustomerOwner As IC3070203DataSet.IC3070203EstUpdCustomerRow = Nothing
        Dim estUpdCustomerUser As IC3070203DataSet.IC3070203EstUpdCustomerRow = Nothing
        Dim customerTagPresenceOwner As IC3070203DataSet.IC3070203CustomerTagPresenceRow = Nothing
        Dim customerTagPresenceUser As IC3070203DataSet.IC3070203CustomerTagPresenceRow = Nothing

        ' 見積情報データテーブル
        Dim estimateInfo As IC3070203DataSet.IC3070203EstimationInfoRow _
            = estInfoDataSet.IC3070203EstimationInfo.Item(0)

        '見積支払方法
        If String.IsNullOrEmpty(estInfoDataSet.IC3070203EstPaymentInfo(0).PAYMENTMETHOD) = False Or _
            (estInfoDataSet.IC3070203EstPaymentInfo(0).IsDEPOSITNull = False Or _
            String.IsNullOrEmpty(estInfoDataSet.IC3070203EstPaymentInfo(0).DEPOSITPAYMENTMETHOD) = False) Then
            estPaymentInfo = estInfoDataSet.IC3070203EstPaymentInfo(0)
        End If

        '見積保険
        If TagPresenceYes.Equals(estInfoDataSet.IC3070203EstimationInfoTagPresence(0).Insurance) Then
            estInsuranceInfo = estInfoDataSet.IC3070203EstInsuranceInfo(0)
        End If

        '顧客
        If Not estInfoDataSet.IC3070203EstUpdCustomer Is Nothing Then
            For Each customerRow In estInfoDataSet.IC3070203EstUpdCustomer
                If CustTypeOwner.Equals(customerRow.CONTRACTCUSTTYPE) Then
                    '顧客(所有者)
                    estUpdCustomerOwner = customerRow
                ElseIf CustTypeUser.Equals(customerRow.CONTRACTCUSTTYPE) Then
                    '顧客(使用者)
                    estUpdCustomerUser = customerRow
                End If
            Next
        End If

        '顧客(タグ有無)
        If Not estInfoDataSet.IC3070203CustomerTagPresence Is Nothing Then
            For Each customerTagPresenceRow In estInfoDataSet.IC3070203CustomerTagPresence
                If CustTypeOwner.Equals(customerTagPresenceRow.CONTRACTCUSTTYPE) Then
                    '顧客(所有者)
                    customerTagPresenceOwner = customerTagPresenceRow
                ElseIf CustTypeUser.Equals(customerTagPresenceRow.CONTRACTCUSTTYPE) Then
                    '顧客(使用者)
                    customerTagPresenceUser = customerTagPresenceRow
                End If
            Next
        End If

        ' 登録処理結果
        estResultRow.IsSuccess = False

        ' 見積管理ID
        Dim estimateId As Long = estimateInfo.ESTIMATEID

        ' 見積情報登録処理
        Dim adapter As New IC3070203TableAdapter()

        Try

            ' 見積情報取得
            Dim getEstimateInfo As New IC3070201TableAdapter(0)
            Dim estimateInfoData As IC3070201DataSet.IC3070201EstimationInfoDataTable = _
                getEstimateInfo.GetEstimationInfoDataTable(estimateId, 0)

            ' 見積情報が取得できなかった場合
            If estimateInfoData.Count = 0 Then
                adapter = Nothing
                Me.prpResultId = ErrCodeOther + ErrCodeEstimate

                ' 見積情報登録結果データテーブルに結果をセット
                estResultRow.EstimateId = estimateId
                estResultRow.IsSuccess = False
                estResultRow.CreateDate = Me.prpCreateDate
                estResultDT.AddIC3070203EstResultRow(estResultRow)

                Return estResultDT
            End If

            '固有エラーチェック
            If String.IsNullOrEmpty(estInfoDataSet.IC3070203EstPaymentInfo(0).PAYMENTMETHOD) = True And _
                (estInfoDataSet.IC3070203EstPaymentInfo(0).IsDEPOSITNull = False Or
                 String.IsNullOrEmpty(estInfoDataSet.IC3070203EstPaymentInfo(0).DEPOSITPAYMENTMETHOD) = False) Then
                '支払方法区分がなしかつ、頭金または頭金支払方法区分が存在する場合エラー

                adapter = Nothing
                Me.prpResultId = ErrCodeOther + ErrCodeEstPayment

                ' 見積情報登録結果データテーブルに結果をセット
                estResultRow.EstimateId = estimateId
                estResultRow.IsSuccess = False
                estResultRow.CreateDate = Me.prpCreateDate
                estResultDT.AddIC3070203EstResultRow(estResultRow)

                Return estResultDT
            End If

            ' 顧客マスタ取得
            Dim cstId As Decimal = 0
            Dim cstData As IC3070203DataSet.IC3070203CustomerDataTable = Nothing
            If estimateInfoData(0).IsCRCUSTIDNull = False Then
                cstId = CDec(Trim(estimateInfoData(0).CRCUSTID))
                cstData = adapter.SelCustomer(cstId)
            End If

            If cstId <> 0 AndAlso Not cstData Is Nothing AndAlso cstData.Count > 0 Then
                ' 顧客ロック取得
                adapter.SelCstIdLock(cstId)
            End If

            
            ' 見積情報ロック取得
            adapter.GetEstimateinfoLock(estimateId)

            ' XMLに存在する項目のみ更新する
            ' 顧客更新
            If Not cstData Is Nothing AndAlso cstData.Count > 0 And Not estUpdCustomerOwner Is Nothing Then
                ' 顧客情報更新
                UpdateCustomerInfo(adapter, cstData, estUpdCustomerOwner, customerTagPresenceOwner)

                ' 顧客連絡時間帯更新
                UpdateCustomerTimeSlot(adapter, cstData, estUpdCustomerOwner, customerTagPresenceOwner)
            End If

            ' 見積情報更新
            UpdateEstimateInfo(adapter, estInfoDataSet, estimateInfoData.Item(0), estimateInfo, estInfoDataSet.IC3070203EstimationInfoTagPresence(0))

            ' 見積車両オプション情報更新
            Dim count As Integer = 0
            For Each estVclOptInfo In estInfoDataSet.IC3070203EstVclOptionInfo
                estVclOptInfo.ESTIMATEID = estimateId
                UpdateEstimateVclOpt(adapter, estVclOptInfo, estInfoDataSet.IC3070203EstVcloptionInfoTagPresence(count))
                count = count + 1
            Next

            ' 見積顧客情報更新
            Dim cstTagFlg As Boolean = False
            If Not estUpdCustomerOwner Is Nothing Or Not estUpdCustomerUser Is Nothing Then
                '<Customer>もしくは<Customer_User>どちらかのタグに値が設定されている
                cstTagFlg = True
            End If

            ' 所有者の場合
            UpdateEstimateCustomerInfo(adapter, estUpdCustomerOwner, estimateInfo, CustTypeOwner, cstTagFlg, customerTagPresenceOwner)

            ' 使用者の場合
            UpdateEstimateCustomerInfo(adapter, estUpdCustomerUser, estimateInfo, CustTypeUser, cstTagFlg, customerTagPresenceUser)

            ' 見積保険情報更新
            If Not estInsuranceInfo Is Nothing Then
                UpdateEstimateInsurance(adapter, estInsuranceInfo, estimateInfo, estInfoDataSet.IC3070203EstimationInfoTagPresence(0))
            End If

            ' 見積支払情報更新
            If Not estPaymentInfo Is Nothing Then
                UpdateEstimatePayment(adapter, estPaymentInfo, estimateInfo, estInfoDataSet.IC3070203EstimationInfoTagPresence(0))
            End If

            ' プロパティに結果をセット
            Me.prpResultId = adapter.ResultId

            ' 見積情報登録結果データテーブルに結果をセット
            estResultRow.EstimateId = estimateId
            estResultRow.IsSuccess = True
            estResultRow.CreateDate = Me.prpCreateDate
            estResultDT.AddIC3070203EstResultRow(estResultRow)

            ' 結果を返却
            Return estResultDT

        Catch ex As Exception
            Me.Rollback = True
            If adapter.ResultId <> ErrCodeSuccess Then
                Me.prpResultId = adapter.ResultId
            Else
                Me.prpResultId = ErrCodeSys
            End If
            Logger.Error(Me.prpResultId.ToString(CultureInfo.InvariantCulture), ex)

            Throw
        Finally
            adapter = Nothing
        End Try

    End Function
#End Region

#Region "Privateメソッド"
    ''' <summary>
    ''' 顧客氏名、住所を編集する
    ''' </summary>
    ''' <param name="editStr1">編集対象1</param>
    ''' <param name="editStr2">編集対象2</param>
    ''' <param name="editStr3">編集対象3</param>
    ''' <returns>編集後文字列</returns>
    ''' <remarks></remarks>
    Private Function editNameAddress(ByVal editStr1 As String, _
                                     ByVal editStr2 As String, _
                                     ByVal editStr3 As String) As String

        Dim editStr As New StringBuilder
        Dim returnStr As String = String.Empty

        If Not String.IsNullOrWhiteSpace(editStr1) Then
            editStr.Append(editStr1)
        End If
        editStr.Append(IC3070203TableAdapter.StringDefValue)

        If Not String.IsNullOrWhiteSpace(editStr2) Then
            editStr.Append(editStr2)
        End If
        editStr.Append(IC3070203TableAdapter.StringDefValue)

        If Not String.IsNullOrWhiteSpace(editStr3) Then
            editStr.Append(editStr3)
        End If

        If String.IsNullOrWhiteSpace(editStr.ToString) Then
            'スペースのみの場合、半角スペースに
            returnStr = IC3070203TableAdapter.StringDefValue
        Else
            returnStr = editStr.ToString
        End If

        Return returnStr

    End Function

    ''' <summary>
    ''' 顧客更新情報チェック・登録
    ''' </summary>
    ''' <param name="cstData">顧客情報(DB)</param>
    ''' <param name="estUpdCustomer">顧客情報(XML)</param>
    ''' <param name="customerTagPresence">顧客タグ有無情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateCustomerInfo(ByVal adapter As IC3070203TableAdapter, _
                                        ByVal cstData As IC3070203DataSet.IC3070203CustomerDataTable, _
                                        ByVal estUpdCustomer As IC3070203DataSet.IC3070203EstUpdCustomerRow, _
                                        ByVal customerTagPresence As IC3070203DataSet.IC3070203CustomerTagPresenceRow) As Boolean

        Dim result As Boolean = True

        Dim cstDataRow As IC3070203DataSet.IC3070203CustomerRow = _
            CType(cstData.Rows(0), IC3070203DataSet.IC3070203CustomerRow)

        ' 顧客マスタ更新可否フラグ
        Dim updCstFlg As Boolean = False
        '名前変更フラグ
        Dim updNameFlg As Boolean = False
        '住所変更フラグ
        Dim updAddFlg As Boolean = False

        Try
            ' 法人フラグ
            If TagPresenceYes.Equals(customerTagPresence.CustomerType) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.FLEET_FLG) Then
                    '値有り
                    ' 法人フラグ変換処理※区分値が異なるため
                    If estUpdCustomer.FLEET_FLG = "0" Then
                        cstDataRow.FLEET_FLG = "1"
                    Else
                        cstDataRow.FLEET_FLG = "0"
                    End If
                Else
                    '値無し
                    cstDataRow.FLEET_FLG = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 個人法人項目コード
            If TagPresenceYes.Equals(customerTagPresence.SubCustomerType) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.PRIVATE_FLEET_ITEM_CD) Then
                    '値有り
                    cstDataRow.PRIVATE_FLEET_ITEM_CD = estUpdCustomer.PRIVATE_FLEET_ITEM_CD
                Else
                    '値無し
                    cstDataRow.PRIVATE_FLEET_ITEM_CD = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客識別番号
            If TagPresenceYes.Equals(customerTagPresence.SocialID) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_SOCIALNUM) Then
                    '値有り
                    cstDataRow.CST_SOCIALNUM = estUpdCustomer.CST_SOCIALNUM
                Else
                    '値無し
                    cstDataRow.CST_SOCIALNUM = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 性別区分
            If TagPresenceYes.Equals(customerTagPresence.Sex) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_GENDER) Then
                    '値有り
                    '性別変換
                    Select Case estUpdCustomer.CST_GENDER
                        Case GENDER_XML_MALE '男
                            cstDataRow.CST_GENDER = GENDER_CST_MALE
                        Case GENDER_XML_FEMALE '女
                            cstDataRow.CST_GENDER = GENDER_CST_FEMALE
                        Case GENDER_XML_BOTH '両方
                            cstDataRow.CST_GENDER = GENDER_CST_BOTH
                        Case GENDER_XML_OTHER 'その他
                            cstDataRow.CST_GENDER = GENDER_CST_OTHER
                        Case Else
                            cstDataRow.CST_GENDER = GENDER_CST_OTHER
                    End Select
                Else
                    '値無し
                    cstDataRow.CST_GENDER = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客誕生日
            If TagPresenceYes.Equals(customerTagPresence.BirthDay) Then
                'タグ有り
                If Not estUpdCustomer.IsCST_BIRTH_DATENull Then
                    '値有り
                    cstDataRow.CST_BIRTH_DATE = estUpdCustomer.CST_BIRTH_DATE
                Else
                    '値無し
                    cstDataRow.CST_BIRTH_DATE = DateTime.Parse("1900/01/01 00:00:00")
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 敬称コード
            If TagPresenceYes.Equals(customerTagPresence.NameTitleCode) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.NAMETITLE_CD) Then
                    '値有り
                    cstDataRow.NAMETITLE_CD = estUpdCustomer.NAMETITLE_CD
                Else
                    '値無し
                    cstDataRow.NAMETITLE_CD = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 敬称
            If TagPresenceYes.Equals(customerTagPresence.NameTitle) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.NAMETITLE_NAME) Then
                    '値有り
                    cstDataRow.NAMETITLE_NAME = estUpdCustomer.NAMETITLE_NAME
                Else
                    '値無し
                    cstDataRow.NAMETITLE_NAME = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' ファーストネーム
            If TagPresenceYes.Equals(customerTagPresence.Name1) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.FIRST_NAME) Then
                    '値有り
                    cstDataRow.FIRST_NAME = estUpdCustomer.FIRST_NAME
                Else
                    '値無し
                    cstDataRow.FIRST_NAME = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
                updNameFlg = True
            End If

            ' ミドルネーム
            If TagPresenceYes.Equals(customerTagPresence.Name2) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.MIDDLE_NAME) Then
                    '値有り
                    cstDataRow.MIDDLE_NAME = estUpdCustomer.MIDDLE_NAME
                Else
                    '値無し
                    cstDataRow.MIDDLE_NAME = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
                updNameFlg = True
            End If

            ' ラストネーム
            If TagPresenceYes.Equals(customerTagPresence.Name3) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.LAST_NAME) Then
                    '値有り
                    cstDataRow.LAST_NAME = estUpdCustomer.LAST_NAME
                Else
                    '値無し
                    cstDataRow.LAST_NAME = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
                updNameFlg = True
            End If

            ' 顧客氏名 
            If updNameFlg Then
                'ファーストネーム、ミドルネーム、ラストネームが変更時のみ更新
                cstDataRow.CST_NAME = editNameAddress(cstDataRow.FIRST_NAME, _
                                                      cstDataRow.MIDDLE_NAME, _
                                                      cstDataRow.LAST_NAME)
            End If

            ' ニックネーム
            If TagPresenceYes.Equals(customerTagPresence.SubName1) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.NICK_NAME) Then
                    '値有り
                    cstDataRow.NICK_NAME = estUpdCustomer.NICK_NAME
                Else
                    '値無し
                    cstDataRow.NICK_NAME = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客会社名
            If TagPresenceYes.Equals(customerTagPresence.CompanyName) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_COMPANY_NAME) Then
                    '値有り
                    cstDataRow.CST_COMPANY_NAME = estUpdCustomer.CST_COMPANY_NAME
                Else
                    '値無し
                    cstDataRow.CST_COMPANY_NAME = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 法人担当者名
            If TagPresenceYes.Equals(customerTagPresence.EmployeeName) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.FLEET_PIC_NAME) Then
                    '値有り
                    cstDataRow.FLEET_PIC_NAME = estUpdCustomer.FLEET_PIC_NAME
                Else
                    '値無し
                    cstDataRow.FLEET_PIC_NAME = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 法人担当者所属部署
            If TagPresenceYes.Equals(customerTagPresence.EmployeeDepartment) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.FLEET_PIC_DEPT) Then
                    '値有り
                    cstDataRow.FLEET_PIC_DEPT = estUpdCustomer.FLEET_PIC_DEPT
                Else
                    '値無し
                    cstDataRow.FLEET_PIC_DEPT = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 法人担当者役職
            If TagPresenceYes.Equals(customerTagPresence.EmployeePosition) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.FLEET_PIC_POSITION) Then
                    '値有り
                    cstDataRow.FLEET_PIC_POSITION = estUpdCustomer.FLEET_PIC_POSITION
                Else
                    '値無し
                    cstDataRow.FLEET_PIC_POSITION = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客住所1 
            If TagPresenceYes.Equals(customerTagPresence.Address1) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_ADDRESS_1) Then
                    '値有り
                    cstDataRow.CST_ADDRESS_1 = estUpdCustomer.CST_ADDRESS_1
                Else
                    '値無し
                    cstDataRow.CST_ADDRESS_1 = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
                updAddFlg = True
            End If

            ' 顧客住所2 
            If TagPresenceYes.Equals(customerTagPresence.Address2) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_ADDRESS_2) Then
                    '値有り
                    cstDataRow.CST_ADDRESS_2 = estUpdCustomer.CST_ADDRESS_2
                Else
                    '値無し
                    cstDataRow.CST_ADDRESS_2 = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
                updAddFlg = True
            End If

            ' 顧客住所3 
            If TagPresenceYes.Equals(customerTagPresence.Address3) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_ADDRESS_3) Then
                    '値有り
                    cstDataRow.CST_ADDRESS_3 = estUpdCustomer.CST_ADDRESS_3
                Else
                    '値無し
                    cstDataRow.CST_ADDRESS_3 = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
                updAddFlg = True
            End If

            ' 顧客住所 
            If updAddFlg Then
                '顧客住所1、顧客住所2、顧客住所3が変更時のみ更新
                cstDataRow.CST_ADDRESS = editNameAddress(cstDataRow.CST_ADDRESS_1, _
                                                         cstDataRow.CST_ADDRESS_2, _
                                                         cstDataRow.CST_ADDRESS_3)
                '桁数オーバーを切り捨て
                If cstDataRow.CST_ADDRESS.Length > 320 Then
                    cstDataRow.CST_ADDRESS = cstDataRow.CST_ADDRESS.Substring(0, 320)
                End If
            End If

            ' 本籍
            If TagPresenceYes.Equals(customerTagPresence.Domicile) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_DOMICILE) Then
                    '値有り
                    cstDataRow.CST_DOMICILE = estUpdCustomer.CST_DOMICILE
                Else
                    '値無し
                    cstDataRow.CST_DOMICILE = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 国籍
            If TagPresenceYes.Equals(customerTagPresence.Country) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_COUNTRY) Then
                    '値有り
                    cstDataRow.CST_COUNTRY = estUpdCustomer.CST_COUNTRY
                Else
                    '値無し
                    cstDataRow.CST_COUNTRY = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客郵便番号 
            If TagPresenceYes.Equals(customerTagPresence.ZipCode) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_ZIPCD) Then
                    '値有り
                    cstDataRow.CST_ZIPCD = estUpdCustomer.CST_ZIPCD
                Else
                    '値無し
                    cstDataRow.CST_ZIPCD = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客住所（州）
            If TagPresenceYes.Equals(customerTagPresence.StateCode) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_ADDRESS_STATE) Then
                    '値有り
                    cstDataRow.CST_ADDRESS_STATE = estUpdCustomer.CST_ADDRESS_STATE
                Else
                    '値無し
                    cstDataRow.CST_ADDRESS_STATE = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客住所（地区）
            If TagPresenceYes.Equals(customerTagPresence.DistrictCode) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_ADDRESS_DISTRICT) Then
                    '値有り
                    cstDataRow.CST_ADDRESS_DISTRICT = estUpdCustomer.CST_ADDRESS_DISTRICT
                Else
                    '値無し
                    cstDataRow.CST_ADDRESS_DISTRICT = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客住所（市）
            If TagPresenceYes.Equals(customerTagPresence.CityCode) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_ADDRESS_CITY) Then
                    '値有り
                    cstDataRow.CST_ADDRESS_CITY = estUpdCustomer.CST_ADDRESS_CITY
                Else
                    '値無し
                    cstDataRow.CST_ADDRESS_CITY = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客住所（地域）
            If TagPresenceYes.Equals(customerTagPresence.LocationCode) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_ADDRESS_LOCATION) Then
                    '値有り
                    cstDataRow.CST_ADDRESS_LOCATION = estUpdCustomer.CST_ADDRESS_LOCATION
                Else
                    '値無し
                    cstDataRow.CST_ADDRESS_LOCATION = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客電話番号 
            If TagPresenceYes.Equals(customerTagPresence.TelNumber) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_PHONE) Then
                    '値有り
                    cstDataRow.CST_PHONE = estUpdCustomer.CST_PHONE
                Else
                    '値無し
                    cstDataRow.CST_PHONE = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客FAX番号 
            If TagPresenceYes.Equals(customerTagPresence.FaxNumber) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_FAX) Then
                    '値有り
                    cstDataRow.CST_FAX = estUpdCustomer.CST_FAX
                Else
                    '値無し
                    cstDataRow.CST_FAX = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客携帯電話番号
            If TagPresenceYes.Equals(customerTagPresence.Mobile) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_MOBILE) Then
                    '値有り
                    cstDataRow.CST_MOBILE = estUpdCustomer.CST_MOBILE
                Else
                    '値無し
                    cstDataRow.CST_MOBILE = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客EMAILアドレス1 
            If TagPresenceYes.Equals(customerTagPresence.EMail1) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_EMAIL_1) Then
                    '値有り
                    cstDataRow.CST_EMAIL_1 = estUpdCustomer.CST_EMAIL_1
                Else
                    '値無し
                    cstDataRow.CST_EMAIL_1 = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客EMAILアドレス2 
            If TagPresenceYes.Equals(customerTagPresence.EMail2) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_EMAIL_2) Then
                    '値有り
                    cstDataRow.CST_EMAIL_2 = estUpdCustomer.CST_EMAIL_2
                Else
                    '値無し
                    cstDataRow.CST_EMAIL_2 = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客勤め先電話番号 
            If TagPresenceYes.Equals(customerTagPresence.BusinessTelNumber) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_BIZ_PHONE) Then
                    '値有り
                    cstDataRow.CST_BIZ_PHONE = estUpdCustomer.CST_BIZ_PHONE
                Else
                    '値無し
                    cstDataRow.CST_BIZ_PHONE = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客収入
            If TagPresenceYes.Equals(customerTagPresence.Income) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_INCOME) Then
                    '値有り
                    cstDataRow.CST_INCOME = estUpdCustomer.CST_INCOME
                Else
                    '値無し
                    cstDataRow.CST_INCOME = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客職業ID
            If TagPresenceYes.Equals(customerTagPresence.OccupationID) Then
                'タグ有り
                If Not estUpdCustomer.IsCST_OCCUPATION_IDNull Then
                    '値有り(i-CROPコードに変換)
                    cstDataRow.CST_OCCUPATION_ID = adapter.GetIcropCdOccupationId(estUpdCustomer.CST_OCCUPATION_ID)
                    If String.IsNullOrEmpty(cstDataRow.CST_OCCUPATION_ID) Then
                        cstDataRow.CST_OCCUPATION_ID = CStr(IC3070203TableAdapter.NumDefValue)
                    End If
                Else
                    '値無し
                    cstDataRow.CST_OCCUPATION_ID = CStr(IC3070203TableAdapter.NumDefValue)
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客職業
            If TagPresenceYes.Equals(customerTagPresence.Occupation) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.CST_OCCUPATION) Then
                    '値有り
                    cstDataRow.CST_OCCUPATION = estUpdCustomer.CST_OCCUPATION
                Else
                    '値無し
                    cstDataRow.CST_OCCUPATION = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 結婚区分
            If TagPresenceYes.Equals(customerTagPresence.Family) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.MARITAL_TYPE) Then
                    '値有り
                    cstDataRow.MARITAL_TYPE = estUpdCustomer.MARITAL_TYPE
                Else
                    '値無し
                    cstDataRow.MARITAL_TYPE = IC3070203TableAdapter.StringDefValue
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' デフォルト言語
            If TagPresenceYes.Equals(customerTagPresence.DefaultLang) Then
                'タグ有り
                If Not String.IsNullOrEmpty(estUpdCustomer.DEFAULT_LANG) Then
                    '値有り
                    cstDataRow.DEFAULT_LANG = estUpdCustomer.DEFAULT_LANG
                Else
                    '値無し
                    cstDataRow.DEFAULT_LANG = IC3070203TableAdapter.StringDefValue
                End If
                'タグ有りの場合、英語フラグを更新する
                If DEFAULT_LANG_EN.Equals(cstDataRow.DEFAULT_LANG) Then
                    cstDataRow.ENG_FLG = ENG_FLG_ENG
                ElseIf IC3070203TableAdapter.StringDefValue.Equals(cstDataRow.DEFAULT_LANG) Then
                    cstDataRow.ENG_FLG = IC3070203TableAdapter.StringDefValue
                Else
                    cstDataRow.ENG_FLG = ENG_FLG_OTHER
                End If
                ' 更新フラグ
                updCstFlg = True
            End If

            ' 顧客マスタ更新
            If updCstFlg Then
                result = adapter.UpdateCustomer(cstDataRow)
            End If

            Return result

        Catch ex As Exception
            Me.Rollback = True
            If adapter.ResultId <> ErrCodeSuccess Then
                Me.prpResultId = adapter.ResultId
            Else
                Me.prpResultId = ErrCodeSys
            End If
            Logger.Error(Me.prpResultId.ToString(CultureInfo.InvariantCulture), ex)

            Throw
        Finally
            adapter = Nothing
        End Try

    End Function

    ''' <summary>
    ''' 顧客連絡時間帯更新
    ''' </summary>
    ''' <param name="cstData">顧客情報(DB)</param>
    ''' <param name="estUpdCustomer">顧客情報(XML)</param>
    ''' <param name="customerTagPresence">顧客タグ有無情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateCustomerTimeSlot(ByVal adapter As IC3070203TableAdapter, _
                                            ByVal cstData As IC3070203DataSet.IC3070203CustomerDataTable, _
                                            ByVal estUpdCustomer As IC3070203DataSet.IC3070203EstUpdCustomerRow, _
                                            ByVal customerTagPresence As IC3070203DataSet.IC3070203CustomerTagPresenceRow) As Boolean
        Dim result As Boolean = True
        Dim cstDataRow As IC3070203DataSet.IC3070203CustomerRow = _
            CType(cstData.Rows(0), IC3070203DataSet.IC3070203CustomerRow)

        If TagPresenceYes.Equals(customerTagPresence.ContactTime) Then
            'タグ有り
            ' 顧客連絡時間帯削除
            adapter.DeleteCstContactTimeslot(cstDataRow.CST_ID)

            If estUpdCustomer.IsCONTACT_TIMESLOTNull = False Then
                '値有り

                '連絡時間帯リスト作成
                Dim contractTimeList As New List(Of Long)
                Dim cnt As Long = 2
                Dim num As Long = estUpdCustomer.CONTACT_TIMESLOT
                While 0 < num
                    Dim flg As Long = num Mod cnt

                    If 0 <> flg Then
                        contractTimeList.Add(CLng(cnt / 2))
                        num = CLng(num - cnt / 2)

                    Else
                        If num = cnt Then
                            contractTimeList.Add(cnt)
                            Exit While
                        End If

                    End If

                    cnt = 2 * cnt
                End While

                ' 顧客連絡時間帯登録
                For Each contractTime In contractTimeList
                    result = adapter.InsertCstContactTimeslot(cstDataRow.CST_ID, contractTime)
                Next
            Else
                '値無し
                '何もしない
            End If
        End If

        Return result

    End Function


    ''' <summary>
    ''' 見積情報更新
    ''' </summary>
    ''' <param name="adapter">アダプタ</param>
    ''' <param name="estInfoDataSet">見積情報データセット</param>
    ''' <param name="estimateInfoData">更新予定の見積情報(DB)</param>
    ''' <param name="estimateInfo">見積更新情報(XML)</param>
    ''' <param name="estimationInfoTagPresence">見積タグ有無情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateEstimateInfo(ByVal adapter As IC3070203TableAdapter, _
                                        ByVal estInfoDataSet As IC3070203DataSet, _
                                        ByVal estimateInfoData As IC3070201DataSet.IC3070201EstimationInfoRow, _
                                        ByVal estimateInfo As IC3070203DataSet.IC3070203EstimationInfoRow, _
                                        ByVal estimationInfoTagPresence As IC3070203DataSet.IC3070203EstimationInfoTagPresenceRow) As Boolean

        Dim updEstInfoFlg As Boolean = False
        Dim result As Boolean = True

        ' 見積管理ID
        estimateInfoData.ESTIMATEID = estimateInfo.ESTIMATEID

        ' 納車予定日
        Dim deliDate As Nullable(Of Date)
        If TagPresenceYes.Equals(estimationInfoTagPresence.DeliDate) Then
            'タグ有り
            If Not estimateInfo.IsDELIDATENull Then
                '値有り
                deliDate = estimateInfo.DELIDATE
            Else
                '値無し
                deliDate = Nothing
            End If
            ' フラグ設定
            updEstInfoFlg = True
        Else
            'タグ無し
            If Not estimateInfoData.IsDELIDATENull Then
                deliDate = estimateInfoData.DELIDATE
            End If
        End If


        ' 値引き額
        Dim discountPrice As Nullable(Of Double)
        If TagPresenceYes.Equals(estimationInfoTagPresence.DiscountPrice) Then
            'タグ有り
            If Not estimateInfo.IsDISCOUNTPRICENull Then
                '値有り
                discountPrice = estimateInfo.DISCOUNTPRICE
            Else
                '値無し
                discountPrice = Nothing
            End If
            ' フラグ設定
            updEstInfoFlg = True
        Else
            'タグ無し
            If Not estimateInfoData.IsDISCOUNTPRICENull Then
                discountPrice = estimateInfoData.DISCOUNTPRICE
            End If
        End If

        ' メモ
        Dim memo As String = String.Empty
        If TagPresenceYes.Equals(estimationInfoTagPresence.Memo) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estimateInfo.MEMO) Then
                '値有り
                memo = estimateInfo.MEMO
            Else
                '値無し
                memo = Nothing
            End If
            ' フラグ設定
            updEstInfoFlg = True
        Else
            'タグ無し
            If Not estimateInfoData.IsMEMONull Then
                memo = estimateInfoData.MEMO
            End If
        End If

        ' 見積印刷日
        Dim estPrintDate As Nullable(Of Date)
        If TagPresenceYes.Equals(estimationInfoTagPresence.EstprintDate) Then
            'タグ有り
            If Not estimateInfo.IsESTPRINTDATENull Then
                '値有り
                estPrintDate = estimateInfo.ESTPRINTDATE
            Else
                '値無し
                estPrintDate = Nothing
            End If
            ' フラグ設定
            updEstInfoFlg = True
        Else
            'タグ無し
            If Not estimateInfoData.IsESTPRINTDATENull Then
                estPrintDate = estimateInfoData.ESTPRINTDATE
            End If
        End If

        ' 契約書印刷フラグ
        If TagPresenceYes.Equals(estimationInfoTagPresence.ContPrintFlg) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estimateInfo.CONTPRINTFLG) Then
                '値有り
                estimateInfoData.CONTPRINTFLG = estimateInfo.CONTPRINTFLG
            Else
                '値無し
                estimateInfoData.CONTPRINTFLG = IC3070203TableAdapter.StringDefValueZero
            End If
            ' フラグ設定
            updEstInfoFlg = True
        End If

        ' 見積実績フラグ
        Dim estActFlg As String = estimateInfoData.EST_ACT_FLG
        'タグ契約書印刷フラグ判定
        If estimateInfo.IsESTPRINTDATENull Then
            'タグ見積印刷日なし
            'そのまま
        ElseIf estimateInfoData.IsESTPRINTDATENull _
            OrElse Date.Compare(estimateInfo.ESTPRINTDATE, estimateInfoData.ESTPRINTDATE) = 1 Then
            'タグ見積印刷日あり
            'かつ (DB見積印刷日 = null または タグ見積印刷日＞DB見積印刷日)
            estActFlg = EST_ACT_FLG_YES
        Else
            'タグ見積印刷日<=DB見積印刷日
            'そのまま
        End If

        '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
        '注文承認されているかチェックする
        Dim ckBookAfter As Boolean = adapter.CheckBookAfter(estimateInfo.ESTIMATEID)

        '変更前の契約条件変更フラグ
        Dim dtBeforeEstIfo As IC3070203DataSet.IC3070203EstChangeInfoDataTable =
            adapter.GetEstBeforeChangeInfo(estInfoDataSet.IC3070203EstimationInfo.Item(0).ESTIMATEID)

        Dim estChgFlg As String = dtBeforeEstIfo.Item(0).CONTRACT_COND_CHG_FLG

        '受注後の場合チェックするで、契約条件変更フラグがOFFの場合に変更がないかをチェックする
        If ((ckBookAfter = True) AndAlso estChgFlg.Equals(CONTRACT_COND_CHG_FLG_OFF)) Then

            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
            Logger.Info("契約変更確認（受注後）")
            '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

            '契約情報に変更がないかを確認する
            estChgFlg = GetOdrConfChangFlg(adapter, estInfoDataSet, dtBeforeEstIfo)

        End If
        '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END

        ' 見積情報更新
        If updEstInfoFlg Then
            result = adapter.UpdateEstimateinfo(estimateInfoData.ESTIMATEID, _
                                                deliDate, _
                                                discountPrice, _
                                                memo, _
                                                estPrintDate, _
                                                estimateInfoData.CONTPRINTFLG, _
                                                estActFlg, _
                                                estChgFlg)
        End If

        Return result

    End Function


    ''' <summary>
    ''' 見積車両オプション情報更新
    ''' </summary>
    ''' <param name="estVclOptInfo">見積車両オプション情報(XML)</param>
    ''' <param name="EstVclOptionInfoTagPresence">見積車両オプションタグ有無情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateEstimateVclOpt(ByVal adapter As IC3070203TableAdapter, _
                                          ByVal estVclOptInfo As IC3070203DataSet.IC3070203EstVclOptionInfoRow, _
                                          ByVal EstVclOptionInfoTagPresence As IC3070203DataSet.IC3070203EstVcloptionInfoTagPresenceRow) As Boolean

        ' 見積車両オプション更新可否フラグ
        Dim updEstVclOptFlg As Boolean = False

        ' 見積車両オプション情報取得
        Dim vclOptData As IC3070203DataSet.IC3070203EstVclOptionInfoDataTable = _
            adapter.SelectEstVcloptioninfo(estVclOptInfo.ESTIMATEID, _
                                            estVclOptInfo.OPTIONPART, _
                                            estVclOptInfo.OPTIONCODE)

        Dim vclOptDataRow As IC3070203DataSet.IC3070203EstVclOptionInfoRow = Nothing

        If vclOptData.Count > 0 Then
            vclOptDataRow = CType(vclOptData.Rows(0), IC3070203DataSet.IC3070203EstVclOptionInfoRow)
        Else
            vclOptDataRow = vclOptData.NewIC3070203EstVclOptionInfoRow
        End If

        ' 車両オプション情報更新可否チェック
        ' 見積管理ID
        vclOptDataRow.ESTIMATEID = estVclOptInfo.ESTIMATEID

        ' オプション区分
        vclOptDataRow.OPTIONPART = estVclOptInfo.OPTIONPART

        ' オプションコード
        vclOptDataRow.OPTIONCODE = estVclOptInfo.OPTIONCODE

        ' オプション名
        vclOptDataRow.OPTIONNAME = estVclOptInfo.OPTIONNAME

        ' 価格
        vclOptDataRow.PRICE = estVclOptInfo.PRICE

        ' 取付費用
        If TagPresenceYes.Equals(EstVclOptionInfoTagPresence.InstallCost) Then
            'タグ有り
            If Not estVclOptInfo.IsINSTALLCOSTNull Then
                '値有り
                vclOptDataRow.INSTALLCOST = estVclOptInfo.INSTALLCOST
            Else
                '値無し
                vclOptDataRow.INSTALLCOST = Nothing
            End If
        End If

        If estVclOptInfo.IsDELETEDATENull = False Then
            ' 削除
            adapter.DeleteEstVcloptioninfo(vclOptDataRow)

        Else
            If vclOptData.Count > 0 Then
                ' 更新
                updEstVclOptFlg = adapter.UpdVcloptionInfo(vclOptDataRow)
            Else
                ' 新設
                updEstVclOptFlg = adapter.InsertEstVcloptioninfo(vclOptDataRow)
            End If
        End If

        Return updEstVclOptFlg

    End Function


    ''' <summary>
    ''' 見積顧客情報追加・更新
    ''' </summary>
    ''' <param name="estCstInfo">見積顧客情報(XML)</param>
    ''' <param name="estimateInfo">見積情報</param>
    ''' <param name="cstType">顧客種別</param>
    ''' <param name="cstTagFlg">タグ値存在フラグ</param>
    ''' <param name="customerTagPresence">顧客タグ有無情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateEstimateCustomerInfo(ByVal adapter As IC3070203TableAdapter, _
                                                ByVal estCstInfo As IC3070203DataSet.IC3070203EstUpdCustomerRow, _
                                                ByVal estimateInfo As IC3070203DataSet.IC3070203EstimationInfoRow, _
                                                ByVal cstType As String, _
                                                ByVal cstTagFlg As Boolean, _
                                                ByVal customerTagPresence As IC3070203DataSet.IC3070203CustomerTagPresenceRow) As Boolean

        Dim result As Boolean = True

        ' 見積顧客更新可否フラグ
        Dim updEstCstFlg As Boolean = False
        '名前変更フラグ
        Dim updNameFlg As Boolean = False
        '住所変更フラグ
        Dim updAddFlg As Boolean = False

        ' 存在チェック
        Dim estCstTbl As IC3070203DataSet.IC3070203EstCustomerInfoDataTable _
            = adapter.SelectEstCustomerinfo(estimateInfo.ESTIMATEID, cstType)

        Dim estCst As IC3070203DataSet.IC3070203EstCustomerInfoRow = Nothing

        If estCstTbl.Count > 0 Then
            If estCstInfo Is Nothing Then
                '処理終了
                Return result
            End If
            '更新
            estCst = estCstTbl.Item(0)
        Else
            If estCstInfo Is Nothing Then
                '更新対象が無い場合
                If cstTagFlg Then
                    'タグ値存在フラグTrueの場合、新規空レコード追加
                    result = adapter.InsertEstCustomer(estimateInfo.ESTIMATEID, cstType)
                End If
                '処理終了
                Return result
            End If

            '新規追加
            estCst = estCstTbl.NewIC3070203EstCustomerInfoRow()
        End If

        ' 更新項目をマージする

        ' 顧客区分
        If TagPresenceYes.Equals(customerTagPresence.CustomerType) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.FLEET_FLG) Then
                '値有り
                If estCstInfo.FLEET_FLG = "0" Then
                    estCst.CUSTPART = "2"
                Else
                    estCst.CUSTPART = "1"
                End If
            Else
                '値無し
                estCst.CUSTPART = Nothing
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' 個人法人項目コード
        If TagPresenceYes.Equals(customerTagPresence.SubCustomerType) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.PRIVATE_FLEET_ITEM_CD) Then
                '値有り
                estCst.PRIVATE_FLEET_ITEM_CD = estCstInfo.PRIVATE_FLEET_ITEM_CD
            Else
                '値無し
                estCst.PRIVATE_FLEET_ITEM_CD = IC3070203TableAdapter.StringDefValue
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' 国民番号
        If TagPresenceYes.Equals(customerTagPresence.SocialID) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.CST_SOCIALNUM) Then
                '値有り
                estCst.SOCIALID = estCstInfo.CST_SOCIALNUM
            Else
                '値無し
                estCst.SOCIALID = Nothing
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' 敬称コード
        If TagPresenceYes.Equals(customerTagPresence.NameTitleCode) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.NAMETITLE_CD) Then
                '値有り
                estCst.NAMETITLE_CD = estCstInfo.NAMETITLE_CD
            Else
                '値無し
                estCst.NAMETITLE_CD = IC3070203TableAdapter.StringDefValue
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' 敬称
        If TagPresenceYes.Equals(customerTagPresence.NameTitle) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.NAMETITLE_NAME) Then
                '値有り
                estCst.NAMETITLE_NAME = estCstInfo.NAMETITLE_NAME
            Else
                '値無し
                estCst.NAMETITLE_NAME = IC3070203TableAdapter.StringDefValue
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' ファーストネーム
        If TagPresenceYes.Equals(customerTagPresence.Name1) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.FIRST_NAME) Then
                '値有り
                estCst.FIRST_NAME = estCstInfo.FIRST_NAME
            Else
                '値無し
                estCst.FIRST_NAME = IC3070203TableAdapter.StringDefValue
            End If
            ' フラグ設定
            updEstCstFlg = True
            updNameFlg = True
        End If

        ' ミドルネーム
        If TagPresenceYes.Equals(customerTagPresence.Name2) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.MIDDLE_NAME) Then
                '値有り
                estCst.MIDDLE_NAME = estCstInfo.MIDDLE_NAME
            Else
                '値無し
                estCst.MIDDLE_NAME = IC3070203TableAdapter.StringDefValue
            End If
            ' フラグ設定
            updEstCstFlg = True
            updNameFlg = True
        End If

        ' ラストネーム
        If TagPresenceYes.Equals(customerTagPresence.Name3) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.LAST_NAME) Then
                '値有り
                estCst.LAST_NAME = estCstInfo.LAST_NAME
            Else
                '値無し
                estCst.LAST_NAME = IC3070203TableAdapter.StringDefValue
            End If
            ' フラグ設定
            updEstCstFlg = True
            updNameFlg = True
        End If

        ' 氏名
        If updNameFlg Then
            'ファーストネーム、ミドルネーム、ラストネームが変更時のみ更新
            Dim nameEdit1 As String = String.Empty
            If Not estCst.IsFIRST_NAMENull Then
                nameEdit1 = estCst.FIRST_NAME
            End If

            Dim nameEdit2 As String = String.Empty
            If Not estCst.IsMIDDLE_NAMENull Then
                nameEdit2 = estCst.MIDDLE_NAME
            End If
            Dim nameEdit3 As String = String.Empty
            If Not estCst.IsLAST_NAMENull Then
                nameEdit3 = estCst.LAST_NAME
            End If
            estCst.NAME = editNameAddress(nameEdit1, nameEdit2, nameEdit3)
        End If

        ' 顧客住所1
        If TagPresenceYes.Equals(customerTagPresence.Address1) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.CST_ADDRESS_1) Then
                '値有り
                estCst.CST_ADDRESS_1 = estCstInfo.CST_ADDRESS_1
            Else
                '値無し
                estCst.CST_ADDRESS_1 = IC3070203TableAdapter.StringDefValue
            End If
            ' フラグ設定
            updEstCstFlg = True
            updAddFlg = True
        End If

        ' 顧客住所2 
        If TagPresenceYes.Equals(customerTagPresence.Address2) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.CST_ADDRESS_2) Then
                '値有り
                estCst.CST_ADDRESS_2 = estCstInfo.CST_ADDRESS_2
            Else
                '値無し
                estCst.CST_ADDRESS_2 = IC3070203TableAdapter.StringDefValue
            End If
            ' フラグ設定
            updEstCstFlg = True
            updAddFlg = True
        End If

        ' 顧客住所3 
        If TagPresenceYes.Equals(customerTagPresence.Address3) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.CST_ADDRESS_3) Then
                '値有り
                estCst.CST_ADDRESS_3 = estCstInfo.CST_ADDRESS_3
            Else
                '値無し
                estCst.CST_ADDRESS_3 = IC3070203TableAdapter.StringDefValue
            End If
            ' フラグ設定
            updEstCstFlg = True
            updAddFlg = True
        End If

        ' 住所
        If updAddFlg Then
            '顧客住所1、顧客住所2、顧客住所3が変更時のみ更新
            Dim addressEdit1 As String = String.Empty
            If Not estCst.IsCST_ADDRESS_1Null Then
                addressEdit1 = estCst.CST_ADDRESS_1
            End If

            Dim addressEdit2 As String = String.Empty
            If Not estCst.IsCST_ADDRESS_2Null Then
                addressEdit2 = estCst.CST_ADDRESS_2
            End If
            Dim addressEdit3 As String = String.Empty
            If Not estCst.IsCST_ADDRESS_3Null Then
                addressEdit3 = estCst.CST_ADDRESS_3
            End If
            estCst.ADDRESS = editNameAddress(addressEdit1, addressEdit2, addressEdit3)
            '桁数オーバーを切り捨て
            If estCst.ADDRESS.Length > 320 Then
                estCst.ADDRESS = estCst.ADDRESS.Substring(0, 320)
            End If
        End If

        ' 郵便番号
        If TagPresenceYes.Equals(customerTagPresence.ZipCode) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.CST_ZIPCD) Then
                '値有り
                estCst.ZIPCODE = estCstInfo.CST_ZIPCD
            Else
                '値無し
                estCst.ZIPCODE = Nothing
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' 顧客住所（州）
        If TagPresenceYes.Equals(customerTagPresence.StateCode) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.CST_ADDRESS_STATE) Then
                '値有り
                estCst.CST_ADDRESS_STATE = estCstInfo.CST_ADDRESS_STATE
            Else
                '値無し
                estCst.CST_ADDRESS_STATE = IC3070203TableAdapter.StringDefValue
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' 顧客住所（地区）
        If TagPresenceYes.Equals(customerTagPresence.DistrictCode) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.CST_ADDRESS_DISTRICT) Then
                '値有り
                estCst.CST_ADDRESS_DISTRICT = estCstInfo.CST_ADDRESS_DISTRICT
            Else
                '値無し
                estCst.CST_ADDRESS_DISTRICT = IC3070203TableAdapter.StringDefValue
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' 顧客住所（市）
        If TagPresenceYes.Equals(customerTagPresence.CityCode) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.CST_ADDRESS_CITY) Then
                '値有り
                estCst.CST_ADDRESS_CITY = estCstInfo.CST_ADDRESS_CITY
            Else
                '値無し
                estCst.CST_ADDRESS_CITY = IC3070203TableAdapter.StringDefValue
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' 顧客住所（地域）
        If TagPresenceYes.Equals(customerTagPresence.LocationCode) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.CST_ADDRESS_LOCATION) Then
                '値有り
                estCst.CST_ADDRESS_LOCATION = estCstInfo.CST_ADDRESS_LOCATION
            Else
                '値無し
                estCst.CST_ADDRESS_LOCATION = IC3070203TableAdapter.StringDefValue
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' 電話番号
        If TagPresenceYes.Equals(customerTagPresence.TelNumber) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.CST_PHONE) Then
                '値有り
                estCst.TELNO = estCstInfo.CST_PHONE
            Else
                '値無し
                estCst.TELNO = Nothing
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' FAX番号
        If TagPresenceYes.Equals(customerTagPresence.FaxNumber) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.CST_FAX) Then
                '値有り
                estCst.FAXNO = estCstInfo.CST_FAX
            Else
                '値無し
                estCst.FAXNO = Nothing
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' 携帯電話番号
        If TagPresenceYes.Equals(customerTagPresence.Mobile) Then
            If Not String.IsNullOrEmpty(estCstInfo.CST_MOBILE) Then
                '値有り
                estCst.MOBILE = estCstInfo.CST_MOBILE
            Else
                '値無し
                estCst.MOBILE = Nothing
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' e-MAILアドレス
        If TagPresenceYes.Equals(customerTagPresence.EMail1) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estCstInfo.CST_EMAIL_1) Then
                '値有り
                estCst.EMAIL = estCstInfo.CST_EMAIL_1
            Else
                '値無し
                estCst.EMAIL = Nothing
            End If
            ' フラグ設定
            updEstCstFlg = True
        End If

        ' 追加・更新判定処理
        ' 更新可否フラグのチェックもここで行う
        If estCstTbl.Count > 0 Then
            ' 更新
            ' フラグチェックはここ
            If updEstCstFlg Then
                result = adapter.UpdateEstCustomerinfo(estCst)
            End If

        Else
            ' 新設
            ' フラグチェックはここ
            If updEstCstFlg Then
                estCst.CONTRACTCUSTTYPE = cstType
                result = adapter.InsertEstCustomerinfo(estimateInfo.ESTIMATEID, estCst)
            End If

        End If

        Return result

    End Function


    ''' <summary>
    ''' 見積保険情報更新
    ''' </summary>
    ''' <param name="estInsuranceInfo">見積保険情報</param>
    ''' <param name="estimateInfo">見積情報</param>
    ''' <param name="estimationInfoTagPresence">見積タグ有無情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateEstimateInsurance(ByVal adapter As IC3070203TableAdapter, _
                                             ByVal estInsuranceInfo As IC3070203DataSet.IC3070203EstInsuranceInfoRow, _
                                             ByVal estimateInfo As IC3070203DataSet.IC3070203EstimationInfoRow, _
                                             ByVal estimationInfoTagPresence As IC3070203DataSet.IC3070203EstimationInfoTagPresenceRow) As Boolean

        ' 見積保険更新可否フラグ
        Dim updEstInsFlg As Boolean = False

        Dim result As Boolean = True

        ' 存在チェック
        Dim estInsuData As IC3070203DataSet.IC3070203EstInsuranceInfoDataTable _
            = adapter.SelectEstInsuranceinfo(estimateInfo.ESTIMATEID)

        Dim estInsu As IC3070203DataSet.IC3070203EstInsuranceInfoRow = Nothing

        If estInsuData.Count > 0 Then
            estInsu = estInsuData.Item(0)
        Else
            estInsu = estInsuData.NewIC3070203EstInsuranceInfoRow()
        End If

        ' 更新項目をマージする

        ' 保険区分
        If TagPresenceYes.Equals(estimationInfoTagPresence.Insurance) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estInsuranceInfo.INSUDVS) Then
                '値有り
                estInsu.INSUDVS = estInsuranceInfo.INSUDVS
            Else
                '値無し
                estInsu.INSUDVS = "1"
            End If
            ' フラグ設定
            updEstInsFlg = True
        End If

        If estInsuData.Count > 0 Then
            If updEstInsFlg Then
                ' 更新
                result = adapter.UpdateEstInsuranceinfo(estimateInfo.ESTIMATEID, _
                                                        estInsu.INSUDVS)
            End If
        Else
            If updEstInsFlg Then
                ' 新設
                result = adapter.InsertEstInsuranceinfo(estimateInfo.ESTIMATEID, _
                                                        estInsu)
            End If
        End If

        Return result

    End Function


    ''' <summary>
    ''' 見積支払情報更新
    ''' </summary>
    ''' <param name="estPaymentInfo">見積支払情報</param>
    ''' <param name="estimateInfo">見積情報</param>
    ''' <param name="estimationInfoTagPresence">見積タグ有無情報</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateEstimatePayment(ByVal adapter As IC3070203TableAdapter, _
                                           ByVal estPaymentInfo As IC3070203DataSet.IC3070203EstPaymentInfoRow, _
                                           ByVal estimateInfo As IC3070203DataSet.IC3070203EstimationInfoRow, _
                                           ByVal estimationInfoTagPresence As IC3070203DataSet.IC3070203EstimationInfoTagPresenceRow) As Boolean

        ' 見積支払更新可否フラグ
        Dim updEstPayFlg As Boolean = False

        Dim result As Boolean = True

        ' 存在チェック
        Dim estPayData As IC3070203DataSet.IC3070203EstPaymentInfoDataTable _
            = adapter.SelectEstPaymentinfo(estimateInfo.ESTIMATEID, estPaymentInfo.PAYMENTMETHOD)

        Dim estPay As IC3070203DataSet.IC3070203EstPaymentInfoRow = Nothing


        If estPayData.Count > 0 Then
            estPay = estPayData.Item(0)
        Else
            estPay = estPayData.NewIC3070203EstPaymentInfoRow()
        End If

        '更新項目をマージする

        ' 見積管理ID
        estPay.ESTIMATEID = estimateInfo.ESTIMATEID

        ' 支払方法区分
        If TagPresenceYes.Equals(estimationInfoTagPresence.PaymentStyle) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estPaymentInfo.PAYMENTMETHOD) Then
                '値有り
                estPay.PAYMENTMETHOD = estPaymentInfo.PAYMENTMETHOD
            End If
            ' フラグ設定
            updEstPayFlg = True
        End If

        ' 頭金
        If TagPresenceYes.Equals(estimationInfoTagPresence.Deposit) Then
            'タグ有り
            If Not estPaymentInfo.IsDEPOSITNull Then
                '値有り
                estPay.DEPOSIT = estPaymentInfo.DEPOSIT
            Else
                '値無し
                estPay.DEPOSIT = Nothing
            End If
            ' フラグ設定
            updEstPayFlg = True
        End If

        ' 頭金支払方法区分
        If TagPresenceYes.Equals(estimationInfoTagPresence.DepositPaymentStyle) Then
            'タグ有り
            If Not String.IsNullOrEmpty(estPaymentInfo.DEPOSITPAYMENTMETHOD) Then
                '値有り
                estPay.DEPOSITPAYMENTMETHOD = estPaymentInfo.DEPOSITPAYMENTMETHOD
            Else
                '値無し
                estPay.DEPOSITPAYMENTMETHOD = Nothing
            End If
            ' フラグ設定
            updEstPayFlg = True
        End If

        If estPayData.Count > 0 Then
            If updEstPayFlg Then
                ' 更新(選択フラグを全て未選択に)
                result = adapter.UpdateEstPaymentinfoSelectFlg(estPay.ESTIMATEID)
                ' 更新
                result = adapter.UpdateEstPaymentinfo(estPay, IC3070203TableAdapter.SelectFlgSelect)
            End If
        Else
            If updEstPayFlg Then
                ' 新設
                If PAYMENTMETHOD_CASH.Equals(estPay.PAYMENTMETHOD) Then
                    '現金、選択
                    result = adapter.InsertEstPaymentinfo(estPay, _
                                                          PAYMENTMETHOD_CASH, _
                                                          IC3070203TableAdapter.SelectFlgSelect)
                    'ローン、未選択
                    result = adapter.InsertEstPaymentinfo(estPay, _
                                                          PAYMENTMETHOD_LONE, _
                                                          IC3070203TableAdapter.SelectFlgNotSelect)
                Else
                    '現金、未選択
                    result = adapter.InsertEstPaymentinfo(estPay, _
                                                          PAYMENTMETHOD_CASH, _
                                                          IC3070203TableAdapter.SelectFlgNotSelect)
                    'ローン、選択
                    result = adapter.InsertEstPaymentinfo(estPay, _
                                                          PAYMENTMETHOD_LONE, _
                                                          IC3070203TableAdapter.SelectFlgSelect)
                End If
            End If
        End If

        Return result

    End Function


    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） START
    ''' <summary>
    ''' 契約情報に変更がないかを確認する
    ''' </summary>
    ''' <param name="adapter"></param>
    ''' <param name="estInfoDataSet"></param>
    ''' <param name="dtBeforeEstIfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetOdrConfChangFlg(ByVal adapter As IC3070203TableAdapter,
                                        ByVal estInfoDataSet As IC3070203DataSet,
                                        ByVal dtBeforeEstIfo As IC3070203DataSet.IC3070203EstChangeInfoDataTable) As String

        '変更前の契約条件変更フラグ
        Dim estChgFlg As String = dtBeforeEstIfo.Item(0).CONTRACT_COND_CHG_FLG

        Dim insTagFlg As Boolean = False        '見積保険タグ存在フラグ
        Dim payTagFlg As Boolean = False        '見積支払タグ存在フラグ
        Dim payDspTagFlg As Boolean = False     '見積頭金支払タグ存在フラグ

        Dim updINSUDVS As String = String.Empty                 '見積支払方法
        Dim updPAYMENTMETHOD As String = String.Empty           '支払方法区分
        Dim updDEPOSITPAYMENTMETHOD As String = String.Empty    '頭金支払方法区分

        '入力値の頭金支払方法区分
        Dim beforeDEPOSITPAYMENTMETHOD As String = String.Empty

        ' 見積保険タグ情報
        Dim estimationInfoTagPresence As IC3070203DataSet.IC3070203EstimationInfoTagPresenceRow =
            estInfoDataSet.IC3070203EstimationInfoTagPresence(0)

        '見積保険
        Dim estInsuranceInfo As IC3070203DataSet.IC3070203EstInsuranceInfoRow = Nothing
        If TagPresenceYes.Equals(estInfoDataSet.IC3070203EstimationInfoTagPresence(0).Insurance) Then
            estInsuranceInfo = estInfoDataSet.IC3070203EstInsuranceInfo(0)

            'タグ有り
            If Not String.IsNullOrEmpty(estInsuranceInfo.INSUDVS) Then
                '値有り
                updINSUDVS = estInsuranceInfo.INSUDVS
            Else
                '値無し
                updINSUDVS = "1"
            End If

            insTagFlg = True

        End If

        '見積支払方法
        Dim estPaymentInfo As IC3070203DataSet.IC3070203EstPaymentInfoRow = Nothing
        If TagPresenceYes.Equals(estInfoDataSet.IC3070203EstimationInfoTagPresence(0).PaymentStyle) Then

            estPaymentInfo = estInfoDataSet.IC3070203EstPaymentInfo(0)

            ' 支払方法区分
            'タグ有り
            If Not String.IsNullOrEmpty(estPaymentInfo.PAYMENTMETHOD) Then
                '値有り
                updPAYMENTMETHOD = estPaymentInfo.PAYMENTMETHOD
            Else
                '値無し (変更前の支払方法と同じ)
                updPAYMENTMETHOD = dtBeforeEstIfo.Item(0).PAYMENTMETHOD
            End If

            payTagFlg = True
        End If

        ' 頭金支払方法区分
        If TagPresenceYes.Equals(estimationInfoTagPresence.DepositPaymentStyle) Then

            estPaymentInfo = estInfoDataSet.IC3070203EstPaymentInfo(0)

            ' 頭金支払方法区分
            'タグ有り
            If (Not estPaymentInfo.IsDEPOSITPAYMENTMETHODNull()) Then
                '値有り
                updDEPOSITPAYMENTMETHOD = estPaymentInfo.DEPOSITPAYMENTMETHOD
            Else
                '値無し
                updDEPOSITPAYMENTMETHOD = String.Empty
            End If

            '更新前の頭金支払情報
            If (Not dtBeforeEstIfo.Item(0).IsDEPOSITPAYMENTMETHODNull()) Then
                beforeDEPOSITPAYMENTMETHOD = dtBeforeEstIfo.Item(0).DEPOSITPAYMENTMETHOD.TrimEnd
            Else
                beforeDEPOSITPAYMENTMETHOD = String.Empty
            End If

            payDspTagFlg = True

        End If

        Dim estUpdateFlg As Boolean = False   '見積契約情報更新フラグ

        '見積保険更新フラグ
        '見積保険情報.保険区分
        If ((insTagFlg = True) AndAlso
            (Not dtBeforeEstIfo.Item(0).INSUDVS.Equals(updINSUDVS))) Then
            estUpdateFlg = True
        End If

        '見積支払情報更新フラグ
        '見積支払情報.支払方法区分
        If ((payTagFlg = True) AndAlso
            (Not dtBeforeEstIfo.Item(0).PAYMENTMETHOD.Equals(updPAYMENTMETHOD))) Then
            estUpdateFlg = True
        End If

        '見積支払情報更新フラグ
        '見積支払情報.頭金支払方法区分
        If ((payDspTagFlg = True) AndAlso
            (Not beforeDEPOSITPAYMENTMETHOD.Equals(updDEPOSITPAYMENTMETHOD))) Then
            estUpdateFlg = True
        End If

        '見積契約情報が変更されたかをチェックする
        If (estUpdateFlg = True) Then
            '変更されたので、変更フラグをONにする
            estChgFlg = CONTRACT_COND_CHG_FLG_ON
        End If

        '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 START
        Logger.Info("11.INSUDVS=" & dtBeforeEstIfo.Item(0).INSUDVS)
        Logger.Info("12.INSUDVS=" & updINSUDVS)
        Logger.Info("13.PAYMENTMETHOD=" & dtBeforeEstIfo.Item(0).PAYMENTMETHOD)
        Logger.Info("14.PAYMENTMETHOD=" & updPAYMENTMETHOD)
        Logger.Info("15.DEPOSITPAYMENTMETHOD=" & beforeDEPOSITPAYMENTMETHOD)
        Logger.Info("16.DEPOSITPAYMENTMETHOD=" & updDEPOSITPAYMENTMETHOD)
        '2019/10/16 TCS 河原 [TMTレスポンススロー] SLT基盤への横展 END

        Return estChgFlg

    End Function
    '更新： 2014/05/28 TCS 安田 受注時説明機能開発（受注後工程スケジュール） END
#End Region

End Class
