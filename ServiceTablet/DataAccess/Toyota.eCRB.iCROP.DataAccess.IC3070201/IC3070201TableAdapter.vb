Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core


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

    ''' <summary>
    ''' 001.見積情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積情報DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetEstimationInfoDataTable(ByVal estimateId As Long) As IC3070201DataSet.IC3070201EstimationInfoDataTable

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201EstimationInfoDataTable)("IC3070201_001")
            Dim sql As New StringBuilder
            With sql
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
                .Append("  FROM TBL_ESTIMATEINFO A ")               '見積情報テーブル
                .Append("     , TBL_EST_VCLINFO B ")                '見積車両情報テーブル
                .Append(" WHERE A.ESTIMATEID = B.ESTIMATEID ")
                .Append("   AND A.ESTIMATEID = :ESTIMATEID ")
                .Append("   AND A.DELFLG = '0' ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            Return query.GetData()
        End Using
    End Function

#End Region

#Region "002.見積車両オプション情報取得"

    ''' <summary>
    ''' 002.見積車両オプション情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積車両オプション情報DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetVclOptionInfoDataTable(ByVal estimateId As Long) As IC3070201DataSet.IC3070201VclOptionInfoDataTable

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
                    .Append("   AND OPTIONPART = '1' ")
                End If

                .Append("ORDER BY ")
                .Append("       OPTIONPART ")               'オプション区分
                .Append("     , OPTIONCODE ")               'オプションコード

            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            Return query.GetData()
        End Using
    End Function

#End Region

#Region "003.見積顧客情報取得"

    ''' <summary>
    ''' 003.見積顧客情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積顧客情報DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetCustomerInfoDataTable(ByVal estimateId As Long) As IC3070201DataSet.IC3070201CustomerInfoDataTable


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
                .Append("  FROM TBL_EST_CUSTOMERINFO ")    '見積顧客情報テーブル
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")

                .Append("ORDER BY ")
                .Append("       CONTRACTCUSTTYPE ")         '契約顧客種別
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            Return query.GetData()
        End Using
    End Function

#End Region

#Region "004.見積諸費用情報取得"

    ''' <summary>
    ''' 004.見積諸費用情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積諸費用情報DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetChargeInfoDataTable(ByVal estimateId As Long) As IC3070201DataSet.IC3070201ChargeInfoDataTable

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
            Return query.GetData()
        End Using
    End Function

#End Region

#Region "005.見積支払方法情報取得"

    ''' <summary>
    ''' 005.見積支払方法情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積支払方法情報DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetPaymentInfoDataTable(ByVal estimateId As Long) As IC3070201DataSet.IC3070201PaymentInfoDataTable

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201PaymentInfoDataTable)("IC3070201_005")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070201_005 */ ")
                .Append("       ESTIMATEID ")                   '見積管理ID
                .Append("     , PAYMENTMETHOD ")                '支払方法区分
                .Append("     , FINANCECOMCODE ")               '融資会社コード
                .Append("     , PAYMENTPERIOD ")                '支払期間
                .Append("     , MONTHLYPAYMENT ")               '毎月返済額
                .Append("     , DEPOSIT ")                      '頭金
                .Append("     , BONUSPAYMENT ")                 'ボーナス時返済額
                .Append("     , DUEDATE ")                      '初回支払期限
                .Append("     , DELFLG ")                       '削除フラグ
                .Append("  FROM TBL_EST_PAYMENTINFO ")          '見積支払方法情報テーブル
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")

                .Append("ORDER BY ")
                .Append("       PAYMENTMETHOD ")                 '支払方法区分
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            Return query.GetData()
        End Using
    End Function

#End Region

#Region "006.見積下取車両情報取得"

    ''' <summary>
    ''' 006.見積下取車両情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積下取車両情報DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetTradeincarInfoDataTable(ByVal estimateId As Long) As IC3070201DataSet.IC3070201TradeincarInfoDataTable

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
            Return query.GetData()
        End Using
    End Function

#End Region

#Region "007.見積保険情報取得"

    ''' <summary>
    ''' 007.見積保険情報取得
    ''' </summary>
    ''' <param name="estimateId">見積管理ID</param>
    ''' <returns>見積保険情報DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetInsuranceInfoDataTable(ByVal estimateId As Long) As IC3070201DataSet.IC3070201EstInsuranceInfoDataTable

        Using query As New DBSelectQuery(Of IC3070201DataSet.IC3070201EstInsuranceInfoDataTable)("IC3070201_007")
            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* IC3070201_007 */ ")
                .Append("       ESTIMATEID ")                   '見積管理ID
                .Append("     , INSUDVS ")                      '保険区分
                .Append("     , INSUCOMCD ")                    '保険会社コード
                .Append("     , INSUKIND ")                     '保険種別
                .Append("     , AMOUNT ")                       '保険金額
                .Append("  FROM TBL_EST_INSURANCEINFO ")        '見積保険情報テーブル
                .Append(" WHERE ESTIMATEID = :ESTIMATEID ")
            End With
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ESTIMATEID", OracleDbType.Long, estimateId)
            Return query.GetData()
        End Using
    End Function

#End Region

End Class

