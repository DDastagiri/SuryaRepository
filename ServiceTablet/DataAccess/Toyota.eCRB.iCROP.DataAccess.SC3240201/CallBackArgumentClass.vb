'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'CallBackArgumentClass.vb
'─────────────────────────────────────
'機能： チップ詳細
'補足： 
'作成： 2013/07/31 TMEJ 岩城 タブレット版SMB機能開発(工程管理)
'更新： 2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
'更新： 2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発
'更新： 2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
'─────────────────────────────────────

''' <summary>
''' コールバック用引数のクラス
''' </summary>
''' <remarks></remarks>
Public Class CallBackArgumentClass

    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
    '差分リフレッシュ用の基準日時
    Public Property PreRefreshDateTime As Date
    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

    Private _method As String
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    'Private _rezId As Long
    Private _rezId As Decimal
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    Private _orderNo As String
    Private _visitPlanTime As String
    Private _startPlanTime As String
    Private _subStartPlanTime As String
    Private _finishPlanTime As String
    Private _subFinishPlanTime As String
    Private _deriveredPlanTime As String
    Private _startProcessTime As String
    Private _finishProcessTime As String
    Private _planWorkTime As String
    Private _subPlanWorkTime As String
    Private _procWorkTime As String
    Private _order As String
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    'Private _failure As String
    'Private _result As String
    'Private _advice As String
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    Private _rOInfoChangeFlg As String
    Private _dlrCD As String
    Private _strCD As String
    Private _validateCode As Integer
    Private _account As String
    Private _rezFlg As String
    Private _carWashFlg As String
    Private _waitingFlg As String
    Private _fixItemCodeList As List(Of String)
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    'Private _fixItemSeqList As List(Of String)
    Private _jobinstrucDtlIdList As List(Of String)
    Private _jobInstructSeqList As List(Of String)
    Private _jobInstructIdList As List(Of String)
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    Private _rOJobSeqList As List(Of String)
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    'Private _srvAddSeqList As List(Of String)
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    Private _matchingRezIdList As List(Of String)
    Private _beforeMatchingRezIdList As List(Of String)
    Private _stallId As String
    Private _subStallId As String
    Private _dispStartTime As String
    Private _dispEndTime As String
    Private _pRezSeq As Long
    Private _notMatchRezIdList As List(Of String)

    Private _subAreaId As String
    Private _srvAddSeq As String
    Private _showDate As String
    Private _rezIdList As List(Of String)
    Private _rezIdStallUseStatusList As List(Of String)
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    'Private _roJobSeq2List As List(Of String)
    Private _invisibleInstructFlgList As List(Of String)
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    Private _stallStartTime As String
    Private _stallEndTime As String

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    'Private _svcInId As Long
    'Private _jobDtlId As Long
    'Private _stallUseId As Long
    'Private _rowLockVersion As Long
    'Private _svcClassId As Long
    Private _svcInId As Decimal
    Private _jobDtlId As Decimal
    Private _stallUseId As Decimal
    Private _rowLockVersion As Long
    Private _svcClassId As Decimal
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    Private _mercId As Long
    Private _chipDispStartDate As Date
    Private _restFlg As Integer
    Private _subRestFlg As String
    Private _inputStallStartTime As Date
    Private _inputStallEndTime As Date
    Private _stallUseStatus As String
    Private _prmsEndTime As String
    Private _roJobSeq As String
    Private _roNum As String
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    'Private _cstId As Long
    'Private _vclId As Long
    Private _cstId As Decimal
    Private _vclId As Decimal
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
    Private _resvStatus As String

    Private _cstName As String
    Private _mobile As String
    Private _home As String
    Private _regNo As String
    Private _vin As String
    Private _vehicle As String
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    Private _completeExaminationFlg As String
    Private _memo As String
    Private _cstAddress As String
    Private _fleetFlg As String
    Private _dmsCstCD As String
    Private _nameTitleName As String
    Private _positionType As String
    Private _cstType As String
    Private _dmsJobDtlId As String
    Private _visitSeq As String
    Private _visitVin As String
    Private _invoiceDateTime As Date
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
    Private _stopReasonType As String
    Private _stopMemo As String
    Private _jobInstructId As String
    Private _jobInstructSeq As String
    Private _RestartJobFlg As String
    Private _FinishStopJobFlg As String
    Private _StallWaitTime As Long
    Private _ChipFinishFlg As String
    Private _ChipStopFlg As String
    Private _ChipStartFlg As String
    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

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

    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 予約ID
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property RezId() As Long
    '    Get
    '        Return Me._rezId
    '    End Get
    '    Set(ByVal value As Long)
    '        Me._rezId = value
    '    End Set
    'End Property
    ''' <summary>
    ''' 予約ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RezId() As Decimal
        Get
            Return Me._rezId
        End Get
        Set(ByVal value As Decimal)
            Me._rezId = value
        End Set
    End Property
    '2013/12/06 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' RO番号
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OrderNo() As String
        Get
            Return Me._orderNo
        End Get
        Set(ByVal value As String)
            Me._orderNo = value
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
    ''' 作業開始予定時間（サブチップ用）
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SubStartPlanTime() As String
        Get
            Return Me._subStartPlanTime
        End Get
        Set(ByVal value As String)
            Me._subStartPlanTime = value
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
    ''' 作業終了予定時間（サブチップ用）
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SubFinishPlanTime() As String
        Get
            Return Me._subFinishPlanTime
        End Get
        Set(ByVal value As String)
            Me._subFinishPlanTime = value
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
    ''' 作業開始実績時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StartProcessTime() As String
        Get
            Return Me._startProcessTime
        End Get
        Set(ByVal value As String)
            Me._startProcessTime = value
        End Set
    End Property

    ''' <summary>
    ''' 作業終了実績時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FinishProcessTime() As String
        Get
            Return Me._finishProcessTime
        End Get
        Set(ByVal value As String)
            Me._finishProcessTime = value
        End Set
    End Property

    ''' <summary>
    ''' 予定作業時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PlanWorkTime() As String
        Get
            Return Me._planWorkTime
        End Get
        Set(ByVal value As String)
            Me._planWorkTime = value
        End Set
    End Property

    ''' <summary>
    ''' 予定作業時間（サブチップ用）
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SubPlanWorkTime() As String
        Get
            Return Me._subPlanWorkTime
        End Get
        Set(ByVal value As String)
            Me._subPlanWorkTime = value
        End Set
    End Property

    ''' <summary>
    ''' 実績作業時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ProcWorkTime() As String
        Get
            Return Me._procWorkTime
        End Get
        Set(ByVal value As String)
            Me._procWorkTime = value
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

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    '    ''' <summary>
    '    ''' 故障原因
    '    ''' </summary>
    '    ''' <value></value>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Property Failure() As String
    '        Get
    '            Return Me._failure
    '        End Get
    '        Set(ByVal value As String)
    '            Me._failure = value
    '        End Set
    '    End Property
    '
    '    ''' <summary>
    '    ''' 診断結果
    '    ''' </summary>
    '    ''' <value></value>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Property Result() As String
    '        Get
    '            Return Me._result
    '        End Get
    '        Set(ByVal value As String)
    '            Me._result = value
    '        End Set
    '    End Property
    '
    '    ''' <summary>
    '    ''' アドバイス
    '    ''' </summary>
    '    ''' <value></value>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Property Advice() As String
    '        Get
    '            Return Me._advice
    '        End Get
    '        Set(ByVal value As String)
    '            Me._advice = value
    '        End Set
    '    End Property
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' RO情報変更フラグ（0:変更なし／1:変更あり）
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ROInfoChangeFlg() As String
        Get
            Return Me._rOInfoChangeFlg
        End Get
        Set(ByVal value As String)
            Me._rOInfoChangeFlg = value
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
    ''' 更新用の整備コードリスト
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FixItemCodeList() As List(Of String)
        Get
            Return Me._fixItemCodeList
        End Get
        Set(ByVal value As List(Of String))
            Me._fixItemCodeList = value
        End Set
    End Property

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 更新用の整備連番リスト
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property FixItemSeqList() As List(Of String)
    '    Get
    '        Return Me._fixItemSeqList
    '    End Get
    '    Set(ByVal value As List(Of String))
    '        Me._fixItemSeqList = value
    '    End Set
    'End Property

    ''' <summary>
    ''' 更新用の作業内容IDリスト
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property JobinstrucDtlIdList() As List(Of String)
        Get
            Return Me._jobinstrucDtlIdList
        End Get
        Set(ByVal value As List(Of String))
            Me._jobinstrucDtlIdList = value
        End Set
    End Property

    ''' <summary>
    ''' 更新用の作業指示枝番リスト
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property JobInstructSeqList() As List(Of String)
        Get
            Return Me._jobInstructSeqList
        End Get
        Set(ByVal value As List(Of String))
            Me._jobInstructSeqList = value
        End Set
    End Property

    ''' <summary>
    ''' 更新用の作業指示IDリスト
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property JobInstructIdList() As List(Of String)
        Get
            Return Me._jobInstructIdList
        End Get
        Set(ByVal value As List(Of String))
            Me._jobInstructIdList = value
        End Set
    End Property
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' 更新用の作業連番リスト
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ROJobSeqList() As List(Of String)
        Get
            Return Me._rOJobSeqList
        End Get
        Set(ByVal value As List(Of String))
            Me._rOJobSeqList = value
        End Set
    End Property

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 更新用の枝番リスト
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property SrvAddSeqList() As List(Of String)
    '    Get
    '        Return Me._srvAddSeqList
    '    End Get
    '    Set(ByVal value As List(Of String))
    '        Me._srvAddSeqList = value
    '    End Set
    'End Property
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' 更新用の予約IDリスト(更新時の、予約IDリスト)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MatchingRezIdList() As List(Of String)
        Get
            Return Me._matchingRezIdList
        End Get
        Set(ByVal value As List(Of String))
            Me._matchingRezIdList = value
        End Set
    End Property

    ''' <summary>
    ''' 更新用の予約IDリスト(チップ詳細を開いた直後の、予約IDリスト)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BeforeMatchingRezIdList() As List(Of String)
        Get
            Return Me._beforeMatchingRezIdList
        End Get
        Set(ByVal value As List(Of String))
            Me._beforeMatchingRezIdList = value
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
    ''' ストールID（サブチップ用）
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SubStallId() As String
        Get
            Return Me._subStallId
        End Get
        Set(ByVal value As String)
            Me._subStallId = value
        End Set
    End Property

    ''' <summary>
    ''' 表示用開始時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DispStartTime() As String
        Get
            Return Me._dispStartTime
        End Get
        Set(ByVal value As String)
            Me._dispStartTime = value
        End Set
    End Property

    ''' <summary>
    ''' 表示用終了時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DispEndTime() As String
        Get
            Return Me._dispEndTime
        End Get
        Set(ByVal value As String)
            Me._dispEndTime = value
        End Set
    End Property

    ''' <summary>
    ''' 予約管理連番
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PRezSeq() As Long
        Get
            Return Me._pRezSeq
        End Get
        Set(ByVal value As Long)
            Me._pRezSeq = value
        End Set
    End Property

    ''' <summary>
    ''' 整備が紐付かない予約IDのリスト
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NotMatchingRezIdList() As List(Of String)
        Get
            Return Me._notMatchRezIdList
        End Get
        Set(ByVal value As List(Of String))
            Me._notMatchRezIdList = value
        End Set
    End Property

    ''' <summary>
    ''' サブチップボックスのID(5:受付/15:追加作業/14:完成検査/16,17:洗車/18:納車/Empty:ストール)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SubAreaId() As String
        Get
            Return Me._subAreaId
        End Get
        Set(ByVal value As String)
            Me._subAreaId = value
        End Set
    End Property

    ''' <summary>
    ''' 枝番
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SrvAddSeq() As String
        Get
            Return Me._srvAddSeq
        End Get
        Set(ByVal value As String)
            Me._srvAddSeq = value
        End Set
    End Property

    ''' <summary>
    ''' 表示日時(yyyy/MM/ddの文字列)
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
    ''' チップエリアに表示した予約IDのリスト
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RezIdList() As List(Of String)
        Get
            Return Me._rezIdList
        End Get
        Set(ByVal value As List(Of String))
            Me._rezIdList = value
        End Set
    End Property

    ''' <summary>
    ''' チップエリアに表示した予約のストール利用ステータスリスト
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RezIdStallUseStatusList() As List(Of String)
        Get
            Return Me._rezIdStallUseStatusList
        End Get
        Set(ByVal value As List(Of String))
            Me._rezIdStallUseStatusList = value
        End Set
    End Property

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' チップエリアに表示した予約の作業連番リスト
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property ROJobSeq2List() As List(Of String)
    '    Get
    '        Return Me._roJobSeq2List
    '    End Get
    '    Set(ByVal value As List(Of String))
    '        Me._roJobSeq2List = value
    '    End Set
    'End Property

    ''' <summary>
    ''' チップエリアに表示した予約の着工支持フラグリスト
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InvisibleInstructFlgList() As List(Of String)
        Get
            Return Me._invisibleInstructFlgList
        End Get
        Set(ByVal value As List(Of String))
            Me._invisibleInstructFlgList = value
        End Set
    End Property
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

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

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' サービス入庫ID
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property SvcInId() As Long
    '    Get
    '        Return Me._svcInId
    '    End Get
    '    Set(ByVal value As Long)
    '        Me._svcInId = value
    '    End Set
    'End Property

    ' ''' <summary>
    ' ''' 作業内容ID
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property JobDtlId() As Long
    '    Get
    '        Return Me._jobDtlId
    '    End Get
    '    Set(ByVal value As Long)
    '        Me._jobDtlId = value
    '    End Set
    'End Property

    ' ''' <summary>
    ' ''' ストール利用ID
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property StallUseId() As Long
    '    Get
    '        Return Me._stallUseId
    '    End Get
    '    Set(ByVal value As Long)
    '        Me._stallUseId = value
    '    End Set
    'End Property

    ''' <summary>
    ''' サービス入庫ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SvcInId() As Decimal
        Get
            Return Me._svcInId
        End Get
        Set(ByVal value As Decimal)
            Me._svcInId = value
        End Set
    End Property

    ''' <summary>
    ''' 作業内容ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property JobDtlId() As Decimal
        Get
            Return Me._jobDtlId
        End Get
        Set(ByVal value As Decimal)
            Me._jobDtlId = value
        End Set
    End Property

    ''' <summary>
    ''' ストール利用ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StallUseId() As Decimal
        Get
            Return Me._stallUseId
        End Get
        Set(ByVal value As Decimal)
            Me._stallUseId = value
        End Set
    End Property
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

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

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 表示サービス分類ID
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property SvcClassId() As Long
    '    Get
    '        Return Me._svcClassId
    '    End Get
    '    Set(ByVal value As Long)
    '        Me._svcClassId = value
    '    End Set
    'End Property

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
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

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
    ''' チップ表示開始日時
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChipDispStartDate() As Date
        Get
            Return Me._chipDispStartDate
        End Get
        Set(ByVal value As Date)
            Me._chipDispStartDate = value
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
    ''' 休憩取得フラグ（サブチップ用）
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SubRestFlg() As String
        Get
            Return Me._subRestFlg
        End Get
        Set(ByVal value As String)
            Me._subRestFlg = value
        End Set
    End Property

    ''' <summary>
    ''' チップ詳細画面で指定した日時の営業開始時間
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
    ''' チップ詳細画面で指定した日時の営業終了時間
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
    ''' ストール利用ステータス
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StallUseStatus() As String
        Get
            Return Me._stallUseStatus
        End Get
        Set(ByVal value As String)
            Me._stallUseStatus = value
        End Set
    End Property

    ''' <summary>
    ''' 見込終了日時
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PrmsEndTime() As String
        Get
            Return Me._prmsEndTime
        End Get
        Set(ByVal value As String)
            Me._prmsEndTime = value
        End Set
    End Property

    ''' <summary>
    ''' 作業連番
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ROJobSeq() As String
        Get
            Return Me._roJobSeq
        End Get
        Set(ByVal value As String)
            Me._roJobSeq = value
        End Set
    End Property

    ''' <summary>
    ''' RO番号
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RONum() As String
        Get
            Return Me._roNum
        End Get
        Set(ByVal value As String)
            Me._roNum = value
        End Set
    End Property

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 顧客ID
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property CstId() As Long
    '    Get
    '        Return Me._cstId
    '    End Get
    '    Set(ByVal value As Long)
    '        Me._cstId = value
    '    End Set
    'End Property

    ' ''' <summary>
    ' ''' 車両IID
    ' ''' </summary>
    ' ''' <value></value>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Property VclId() As Long
    '    Get
    '        Return Me._vclId
    '    End Get
    '    Set(ByVal value As Long)
    '        Me._vclId = value
    '    End Set
    'End Property

    ''' <summary>
    ''' 顧客ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CstId() As Decimal
        Get
            Return Me._cstId
        End Get
        Set(ByVal value As Decimal)
            Me._cstId = value
        End Set
    End Property

    ''' <summary>
    ''' 車両IID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property VclId() As Decimal
        Get
            Return Me._vclId
        End Get
        Set(ByVal value As Decimal)
            Me._vclId = value
        End Set
    End Property
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' 予約ステータス
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ResvStatus() As String
        Get
            Return Me._resvStatus
        End Get
        Set(ByVal value As String)
            Me._resvStatus = value
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
    ''' 携帯電話番号
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
    ''' 登録No.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RegNo() As String
        Get
            Return Me._regNo
        End Get
        Set(ByVal value As String)
            Me._regNo = value
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

    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 完成検査フラグ
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
    ''' メモ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Memo() As String
        Get
            Return Me._memo
        End Get
        Set(ByVal value As String)
            Me._memo = value
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
    ''' 法人フラグ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FleetFlg() As String
        Get
            Return Me._fleetFlg
        End Get
        Set(ByVal value As String)
            Me._fleetFlg = value
        End Set
    End Property
    
    ''' <summary>
    ''' 基幹顧客コード
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DmsCstCD() As String
        Get
            Return Me._dmsCstCD
        End Get
        Set(ByVal value As String)
            Me._dmsCstCD = value
        End Set
    End Property

    ''' <summary>
    ''' 敬称
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
    ''' 配置区分
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PositionType() As String
        Get
            Return Me._positionType
        End Get
        Set(ByVal value As String)
            Me._positionType = value
        End Set
    End Property

    ''' <summary>
    ''' 顧客種別
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CstType() As String
        Get
            Return Me._cstType
        End Get
        Set(ByVal value As String)
            Me._cstType = value
        End Set
    End Property

    ''' <summary>
    ''' 基幹作業内容ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DmsJobDtlId() As String
        Get
            Return Me._dmsJobDtlId
        End Get
        Set(ByVal value As String)
            Me._dmsJobDtlId = value
        End Set
    End Property

    ''' <summary>
    ''' 来店者実績連番
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property VisitSeq() As String
        Get
            Return Me._visitSeq
        End Get
        Set(ByVal value As String)
            Me._visitSeq = value
        End Set
    End Property

    ''' <summary>
    ''' 来店者VIN
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property VisitVin() As String
        Get
            Return Me._visitVin
        End Get
        Set(ByVal value As String)
            Me._visitVin = value
        End Set
    End Property

    ''' <summary>
    ''' 清算準備完了日時
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InvoiceDateTime() As Date
        Get
            Return Me._invoiceDateTime
        End Get
        Set(ByVal value As Date)
            Me._invoiceDateTime = value
        End Set
    End Property
    '2013/12/04 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END

    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 中断理由
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StopReasonType() As String
        Get
            Return Me._stopReasonType
        End Get
        Set(ByVal value As String)
            Me._stopReasonType = value
        End Set
    End Property

    ''' <summary>
    ''' 中断MEMO
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StopMemo() As String
        Get
            Return Me._stopMemo
        End Get
        Set(ByVal value As String)
            Me._stopMemo = value
        End Set
    End Property

    ''' <summary>
    ''' 作業指示ID
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property JobInstructId() As String
        Get
            Return Me._jobInstructId
        End Get
        Set(ByVal value As String)
            Me._jobInstructId = value
        End Set
    End Property

    ''' <summary>
    ''' 作業指示シーケンス
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property JobInstructSeq() As String
        Get
            Return Me._jobInstructSeq
        End Get
        Set(ByVal value As String)
            Me._jobInstructSeq = value
        End Set
    End Property

    ''' <summary>
    ''' 再開フラグ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RestartJobFlg() As String
        Get
            Return Me._RestartJobFlg
        End Get
        Set(ByVal value As String)
            Me._RestartJobFlg = value
        End Set
    End Property

    ''' <summary>
    ''' ストール待ち時間
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StallWaitTime() As Long
        Get
            Return Me._StallWaitTime
        End Get
        Set(ByVal value As Long)
            Me._StallWaitTime = value
        End Set
    End Property

    ''' <summary>
    ''' 中断Job終了フラグ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FinishStopJobFlg() As String
        Get
            Return Me._FinishStopJobFlg
        End Get
        Set(ByVal value As String)
            Me._FinishStopJobFlg = value
        End Set
    End Property

    ''' <summary>
    ''' チップ終了フラグ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChipFinishFlg() As String
        Get
            Return Me._ChipFinishFlg
        End Get
        Set(ByVal value As String)
            Me._ChipFinishFlg = value
        End Set
    End Property

    ''' <summary>
    ''' チップ中断フラグ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChipStopFlg() As String
        Get
            Return Me._ChipStopFlg
        End Get
        Set(ByVal value As String)
            Me._ChipStopFlg = value
        End Set
    End Property


    ''' <summary>
    ''' チップ開始フラグ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChipStartFlg() As String
        Get
            Return Me._ChipStartFlg
        End Get
        Set(ByVal value As String)
            Me._ChipStartFlg = value
        End Set
    End Property
    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

End Class