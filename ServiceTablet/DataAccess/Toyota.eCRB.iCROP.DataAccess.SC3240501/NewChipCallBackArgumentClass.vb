'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'NewChipCallBackArgumentClass.vb
'─────────────────────────────────────
'機能： 新規予約作成
'補足： 
'作成： 2013/12/04 TMEJ 下村
'更新： 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
'─────────────────────────────────────

''' <summary>
''' コールバック用引数のクラス
''' </summary>
''' <remarks></remarks>
Public Class NewChipCallBackArgumentClass

    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
    '差分リフレッシュ用の基準日時
    Public Property PreRefreshDateTime As Date
    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

    Private _method As String
    Private _displayStartDate As Date
    Private _displayEndDate As Date
    Private _dlrCD As String
    Private _strCD As String
    Private _showDate As String
    Private _account As String
    Private _stallId As String
    Private _visitPlanTime As String
    Private _startPlanTime As String
    Private _finishPlanTime As String
    Private _deriveredPlanTime As String
    Private _newChipDispStartDate As Date
    Private _workTime As String
    Private _order As String
    Private _rezFlg As String
    Private _carWashFlg As String
    Private _waitingFlg As String
    Private _cstId As String
    Private _vin As String
    Private _vclId As String
    Private _cstVclType As String
    Private _saCode As String
    Private _regno As String
    Private _validateCode As Integer
    Private _vehicle As String
    Private _cstName As String
    Private _mobile As String
    Private _home As String
    Private _cstAddress As String
    Private _dmsCstCode As String
    Private _completeExaminationFlg As String
    Private _svcClassId As Decimal
    Private _mercId As Long
    Private _nameTitleCD As String
    Private _nameTitleName As String
    Private _rowLockVersion As Long
    Private _restFlg As Integer
    Private _stallStartTime As String
    Private _stallEndTime As String
    Private _inputStallStartTime As Date
    Private _inputStallEndTime As Date
    Private _searchedFlg As String


    ''' <summary>
    ''' コールバックメソッド名
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Method() As String
        Get
            Return Me._method
        End Get
        Set(ByVal value As String)
            Me._method = value
        End Set
    End Property

    ''' <summary>
    ''' チップ表示される開始時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DisplayStartDate() As Date
        Get
            Return Me._displayStartDate
        End Get
        Set(ByVal value As Date)
            Me._displayStartDate = value
        End Set
    End Property

    ''' <summary>
    ''' チップ表示される終了時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DisplayEndDate() As Date
        Get
            Return Me._displayEndDate
        End Get
        Set(ByVal value As Date)
            Me._displayEndDate = value
        End Set
    End Property

    ''' <summary>
    ''' 販売店コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DlrCD() As String
        Get
            Return Me._dlrCD
        End Get
        Set(ByVal value As String)
            Me._dlrCD = value
        End Set
    End Property

    ''' <summary>
    ''' 店舗コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StrCD() As String
        Get
            Return Me._strCD
        End Get
        Set(ByVal value As String)
            Me._strCD = value
        End Set
    End Property

    ''' <summary>
    ''' 工程管理画面で開いている日付
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowDate() As String
        Get
            Return Me._showDate
        End Get
        Set(ByVal value As String)
            Me._showDate = value
        End Set
    End Property

    ''' <summary>
    ''' ログインアカウント
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Account() As String
        Get
            Return Me._account
        End Get
        Set(ByVal value As String)
            Me._account = value
        End Set
    End Property

    ''' <summary>
    ''' ストールID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StallId() As String
        Get
            Return Me._stallId
        End Get
        Set(ByVal value As String)
            Me._stallId = value
        End Set
    End Property

    ''' <summary>
    ''' 来店予定時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property VisitPlanTime() As String
        Get
            Return Me._visitPlanTime
        End Get
        Set(ByVal value As String)
            Me._visitPlanTime = value
        End Set
    End Property

    ''' <summary>
    ''' 作業開始予定時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StartPlanTime() As String
        Get
            Return Me._startPlanTime
        End Get
        Set(ByVal value As String)
            Me._startPlanTime = value
        End Set
    End Property

    ''' <summary>
    ''' 作業終了予定時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FinishPlanTime() As String
        Get
            Return Me._finishPlanTime
        End Get
        Set(ByVal value As String)
            Me._finishPlanTime = value
        End Set
    End Property

    ''' <summary>
    ''' 納車予定時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DeriveredPlanTime() As String
        Get
            Return Me._deriveredPlanTime
        End Get
        Set(ByVal value As String)
            Me._deriveredPlanTime = value
        End Set
    End Property

    ''' <summary>
    ''' 表示開始日時
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NewChipDispStartDate() As Date
        Get
            Return Me._newChipDispStartDate
        End Get
        Set(ByVal value As Date)
            Me._newChipDispStartDate = value
        End Set
    End Property

    ''' <summary>
    ''' 作業時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property WorkTime() As String
        Get
            Return Me._workTime
        End Get
        Set(ByVal value As String)
            Me._workTime = value
        End Set
    End Property

    ''' <summary>
    ''' ご用命
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Order() As String
        Get
            Return Me._order
        End Get
        Set(ByVal value As String)
            Me._order = value
        End Set
    End Property

    ''' <summary>
    ''' 予約フラグ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RezFlg() As String
        Get
            Return Me._rezFlg
        End Get
        Set(ByVal value As String)
            Me._rezFlg = value
        End Set
    End Property

    ''' <summary>
    ''' 洗車フラグ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CarWashFlg() As String
        Get
            Return Me._carWashFlg
        End Get
        Set(ByVal value As String)
            Me._carWashFlg = value
        End Set
    End Property

    ''' <summary>
    ''' 待ち方フラグ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property WaitingFlg() As String
        Get
            Return Me._waitingFlg
        End Get
        Set(ByVal value As String)
            Me._waitingFlg = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CstId() As String
        Get
            Return Me._cstId
        End Get
        Set(ByVal value As String)
            Me._cstId = value
        End Set
    End Property

    ''' <summary>
    ''' VIN
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Vin() As String
        Get
            Return Me._vin
        End Get
        Set(ByVal value As String)
            Me._vin = value
        End Set
    End Property

    ''' <summary>
    ''' 車両ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property VclId() As String
        Get
            Return Me._vclId
        End Get
        Set(ByVal value As String)
            Me._vclId = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客車両区分
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CstVclType() As String
        Get
            Return Me._cstVclType
        End Get
        Set(ByVal value As String)
            Me._cstVclType = value
        End Set
    End Property

    ''' <summary>
    ''' SAコード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SACode() As String
        Get
            Return Me._saCode
        End Get
        Set(ByVal value As String)
            Me._saCode = value
        End Set
    End Property

    ''' <summary>
    ''' 登録No.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RegNo() As String
        Get
            Return Me._regno
        End Get
        Set(ByVal value As String)
            Me._regno = value
        End Set
    End Property

    ''' <summary>
    ''' 入力項目チェック結果コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ValidateCode() As Integer
        Get
            Return Me._validateCode
        End Get
        Set(ByVal value As Integer)
            Me._validateCode = value
        End Set
    End Property

    ''' <summary>
    ''' 車種
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Vehicle() As String
        Get
            Return Me._vehicle
        End Get
        Set(ByVal value As String)
            Me._vehicle = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客名
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CstName() As String
        Get
            Return Me._cstName
        End Get
        Set(ByVal value As String)
            Me._cstName = value
        End Set
    End Property

    ''' <summary>
    ''' 携帯番号
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Mobile() As String
        Get
            Return Me._mobile
        End Get
        Set(ByVal value As String)
            Me._mobile = value
        End Set
    End Property

    ''' <summary>
    ''' 電話番号
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Home() As String
        Get
            Return Me._home
        End Get
        Set(ByVal value As String)
            Me._home = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客住所
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CstAddress() As String
        Get
            Return Me._cstAddress
        End Get
        Set(ByVal value As String)
            Me._cstAddress = value
        End Set
    End Property

    ''' <summary>
    ''' 基幹顧客コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DmsCstCode() As String
        Get
            Return Me._dmsCstCode
        End Get
        Set(ByVal value As String)
            Me._dmsCstCode = value
        End Set
    End Property

    ''' <summary>
    ''' 完成検査有無
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CompleteExaminationFlg() As String
        Get
            Return Me._completeExaminationFlg
        End Get
        Set(ByVal value As String)
            Me._completeExaminationFlg = value
        End Set
    End Property

    ''' <summary>
    ''' 表示サービス分類ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SvcClassId() As Decimal
        Get
            Return Me._svcClassId
        End Get
        Set(ByVal value As Decimal)
            Me._svcClassId = value
        End Set
    End Property

    ''' <summary>
    ''' 表示商品ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MercId() As Long
        Get
            Return Me._mercId
        End Get
        Set(ByVal value As Long)
            Me._mercId = value
        End Set
    End Property

    ''' <summary>
    ''' 敬称コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NameTitleCD() As String
        Get
            Return Me._nameTitleCD
        End Get
        Set(ByVal value As String)
            Me._nameTitleCD = value
        End Set
    End Property

    ''' <summary>
    ''' 敬称名
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NameTitleName() As String
        Get
            Return Me._nameTitleName
        End Get
        Set(ByVal value As String)
            Me._nameTitleName = value
        End Set
    End Property

    ''' <summary>
    ''' 行ロックバージョン
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RowLockVersion() As Long
        Get
            Return Me._rowLockVersion
        End Get
        Set(ByVal value As Long)
            Me._rowLockVersion = value
        End Set
    End Property

    ''' <summary>
    ''' 休憩取得フラグ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RestFlg() As Integer
        Get
            Return Me._restFlg
        End Get
        Set(ByVal value As Integer)
            Me._restFlg = value
        End Set
    End Property

    ''' <summary>
    ''' 営業開始時間(HH:mm)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StallStartTime() As String
        Get
            Return Me._stallStartTime
        End Get
        Set(ByVal value As String)
            Me._stallStartTime = value
        End Set
    End Property

    ''' <summary>
    ''' 営業終了時間(HH:mm)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StallEndTime() As String
        Get
            Return Me._stallEndTime
        End Get
        Set(ByVal value As String)
            Me._stallEndTime = value
        End Set
    End Property

    ''' <summary>
    ''' 営業開始時間(Date)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InputStallStartTime() As Date
        Get
            Return Me._inputStallStartTime
        End Get
        Set(ByVal value As Date)
            Me._inputStallStartTime = value
        End Set
    End Property

    ''' <summary>
    ''' 営業終了時間(Date)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InputStallEndTime() As Date
        Get
            Return Me._inputStallEndTime
        End Get
        Set(ByVal value As Date)
            Me._inputStallEndTime = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客検索実行済みフラグ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SearchedFlg() As String
        Get
            Return Me._searchedFlg
        End Get
        Set(ByVal value As String)
            Me._searchedFlg = value
        End Set
    End Property

End Class