'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3810301BusinessLogic.vb
'─────────────────────────────────────
'機能： R/O連携ビジネスロジック
'補足： 
'作成： 2012/01/26 KN 瀧
'更新： 2012/03/22 KN 瀧 【SERVICE_1】サービス来店者管理追加時の項目追加(来店日時、SA割当日時)
'更新： 2012/03/26 KN 佐藤 【SERVICE_1】サービス来店者管理追加時の項目追加(振当ステータス)
'更新： 2012/04/06 KN 瀧 【SERVICE_1】R/Oキャンセル時、整備受注Noでもキャンセルできるようにする
'更新： 2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する
'更新： 2013/06/17 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新： 2013/06/17 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発
'更新： 
'─────────────────────────────────────

Imports System.Xml
Imports System.Text
Imports System.Web
Imports System.Reflection
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.IC3810301
Imports Toyota.eCRB.iCROP.DataAccess.IC3810301.IC3810301DataSetTableAdapters
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic.SMBCommonClassBusinessLogic

''' <summary>
''' IC3810301
''' </summary>
''' <remarks>R/O連携</remarks>
Public Class IC3810301BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultSuccess As Long = 0
    ''' <summary>
    ''' エラー:SAコードが異なる
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultDiffSACode As Long = 1
    ''' <summary>
    ''' エラー:DBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultDBTimeout As Long = 901
    ''' <summary>
    ''' エラー:該当データなし
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultNoMatch As Long = 902
    ''' <summary>
    ''' エラー:登録失敗
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultInsertNG As Long = 903

    ' 2012/07/05 西岡 事前準備対応 START
    ''' <summary>
    ''' 振当ステータス（SA振当済）
    ''' </summary>
    ''' <remarks></remarks>
    Public Const AssignFinished As String = "2"
    ' 2012/07/05 西岡 事前準備対応 END

    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    ''' <summary>
    ''' 画面ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const APPLICATION_ID As String = "IC3810301"
    ''' <summary>
    ''' 登録区分:サービス入庫テーブル
    ''' </summary>
    Public Const RegisterServiceIn As RegisterType = 0
    ''' <summary>
    ''' 活動ID（未設定）
    ''' </summary>
    Public Const NoActivityId As Long = 0
    ''' <summary>
    ''' キャンセルフラグ:0
    ''' </summary>
    Public Const CancelFlg As String = "0"
    '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
    ''' <summary>
    ''' 文字列省略値:" "
    ''' </summary>
    Public Const defaultValueString As String = " "
    ''' <summary>
    ''' ROステータス省略値:" "
    ''' </summary>
    Public Const defaultRoStatus As String = "00"
    ''' <summary>
    ''' 日時省略値:"1900/01/01 00:00:00"
    ''' </summary>
    Public Const defaultValueData As String = "1900/01/01 00:00:00"
    ''' <summary>
    ''' シーケンス省略値:" -1"
    ''' </summary>
    Public Const defaultValueSeq As Long = -1
    ''' <summary>
    ''' 行ロックバージョン省略値
    ''' </summary>
    ''' <remarks></remarks>
    Public Const defaultRowVersion As Long = 0

#End Region

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END


    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START

    ' ''' <summary>
    ' ''' R/O画面仕掛中反映
    ' ''' </summary>
    ' ''' <param name="rowIN">R/O画面仕掛中反映引数</param>
    ' ''' <returns>登録結果</returns>
    ' ''' <remarks></remarks>
    ' ''' 
    ' ''' <history>
    ' ''' 2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する
    ' ''' </history>
    '<EnableCommit()>
    'Public Function AddOrderSave(ByVal rowIN As IC3810301inOrderSaveRow) As Long


    '    Try
    '        ''引数をログに出力
    '        Dim args As New List(Of String)
    '        ' DataRow内の項目を列挙
    '        Me.AddLogData(args, rowIN)
    '        ''開始ログの出力
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '            , "{0}.{1} IN:{2}" _
    '            , Me.GetType.ToString _
    '            , MethodBase.GetCurrentMethod.Name _
    '            , String.Join(", ", args.ToArray())))

    '        Using da As New IC3810301DataTableAdapter
    '            '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する START
    '            '現在日時を取得
    '            Dim nowDate As Date = DateTimeFunc.Now(rowIN.DLRCD, rowIN.STRCD)
    '            'ストール予約TBL更新件数
    '            Dim updateCount As Integer = 0

    '            '予約IDがある場合はストール予約TBLを更新する
    '            If Not (rowIN.IsREZIDNull) AndAlso rowIN.REZID > 0 Then

    '                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '                'サービス入庫情報を取得する
    '                Dim dtGetRowLockVersion As IC3810301GetRowLockVersionDataTable = da.GetRowLockVersion(rowIN)

    '                Dim drGetRowLockVersion As IC3810301GetRowLockVersionRow = Nothing

    '                If Not dtGetRowLockVersion.Rows.Count = 0 Then


    '                    'サービス入庫情報を格納
    '                    drGetRowLockVersion = DirectCast(dtGetRowLockVersion.Rows(0), IC3810301GetRowLockVersionRow)

    '                    '引数の予約IDにサービス入庫IDを格納
    '                    rowIN.REZID = drGetRowLockVersion.SVCIN_ID

    '                    'サービス入庫テーブルのロック処理
    '                    Dim sarviceInLock As Integer = LockServiceInTable(rowIN.REZID, _
    '                                                                      drGetRowLockVersion.ROW_LOCK_VERSION, _
    '                                                                      CancelFlg, _
    '                                                                      rowIN.ACCOUNT, _
    '                                                                      nowDate)

    '                    'サービス入庫テーブルのロックに失敗した場合
    '                    If (sarviceInLock <> ResultSuccess) Then
    '                        Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                       , "{0}.{1} OUT:RETURNCODE = {2}" _
    '                       , Me.GetType.ToString _
    '                       , MethodBase.GetCurrentMethod.Name _
    '                       , ResultNoMatch))
    '                        Return ResultNoMatch
    '                    End If

    '                    'ストール予約TBL更新
    '                    updateCount = da.UpdateDBOrderReserveSave(rowIN, nowDate)
    '                End If
    '                '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


    '                '更新件数が0件の場合はエラーにする
    '                If updateCount = 0 Then
    '                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                        , "{0}.{1} OUT:RETURNCODE = {2}" _
    '                        , Me.GetType.ToString _
    '                        , MethodBase.GetCurrentMethod.Name _
    '                        , ResultNoMatch))
    '                    Return ResultNoMatch
    '                End If
    '            End If
    '            '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する END

    '            ''来店実績番号の入力チェック
    '            If rowIN.IsVISITSEQNull = False _
    '                AndAlso (rowIN.VISITSEQ > 0) Then
    '                ''来店実績番号が入力されている場合、修正更新
    '                ''サービス来店者キー情報の取得
    '                Using dtVisit As IC3810301VisitKeyDataTable = da.GetVisitKey(rowIN)
    '                    If dtVisit.Rows.Count = 0 Then
    '                        ''該当データが存在しない場合
    '                        'ロールバックとログを出力する
    '                        Me.Rollback = True
    '                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                            , "{0}.{1} OUT:RETURNCODE = {2}" _
    '                            , Me.GetType.ToString _
    '                            , MethodBase.GetCurrentMethod.Name _
    '                            , ResultNoMatch))
    '                        Return ResultNoMatch
    '                    End If
    '                    Dim rowVK As IC3810301VisitKeyRow = DirectCast(dtVisit.Rows(0), IC3810301VisitKeyRow)
    '                    If (rowVK.IsSACODENull = True) _
    '                     OrElse (String.Compare(rowIN.SACODE, rowVK.SACODE, True, CultureInfo.CurrentCulture) <> 0) Then
    '                        ' 2012/07/05 西岡 事前準備対応 START
    '                        ' SA振当済みの場合
    '                        If (AssignFinished.Equals(rowVK.ASSIGNSTATUS)) Then
    '                            ''SAコードチェック
    '                            ''SAコードが異なる場合はエラー
    '                            'ロールバックとログを出力する
    '                            Me.Rollback = True
    '                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                             , "{0}.{1} OUT:RETURNCODE = {2}" _
    '                             , Me.GetType.ToString _
    '                             , MethodBase.GetCurrentMethod.Name _
    '                             , ResultDiffSACode))
    '                            Return ResultDiffSACode
    '                        End If
    '                        ' 2012/07/05 西岡 事前準備対応 END
    '                    End If
    '                End Using

    '                ''修正更新処理
    '                da.UpdateVisitOrder(rowIN, nowDate)

    '                '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する START
    '                'Else
    '                ''来店実績番号が入力されていない場合、新規登録
    '                ''新規登録処理
    '                'rowIN.VISITSEQ = da.InsertVisitOrder(rowIN)
    '                '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する END
    '            End If
    '            '        2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '            '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する START
    '            'ストール予約TBLの更新件数が0件でない場合はストール予約履歴TBLの登録する
    '            'If updateCount <> 0 Then
    '            '    Using commonClass As New SMBCommonClassBusinessLogic
    '            '        Dim commonReturnCode As Long =TART
    '            '           commonClass.RegisterStallReserveHis(rowIN.DLRCD, _
    '            '                                               rowIN.STRCD, _
    '            '                                               rowIN.REZID, _
    '            '                                               nowDate, _
    '            '                                               RegisterType.ReserveHisIndividual)
    '            '        'ORACLEのタイムアウト場合
    '            '        If commonReturnCode = ReturnCode.ErrDBTimeout Then
    '            '            'ロールバックとログを出力する
    '            '            Me.Rollback = True
    '            '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '            '                , "{0}.{1} OUT:RETURNCODE = {2}" _
    '            '                , Me.GetType.ToString _
    '            '                , MethodBase.GetCurrentMethod.Name _
    '            '                , ResultDBTimeout))
    '            '            Return ResultDBTimeout
    '            '        End If
    '            '    End Using
    '            'End If
    '            '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する END
    '            '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END
    '        End Using
    '        ''終了ログの出力
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '            , "{0}.{1} OUT:RETURNCODE = {2}" _
    '            , Me.GetType.ToString _
    '            , MethodBase.GetCurrentMethod.Name _
    '            , ResultSuccess))
    '        Return ResultSuccess
    '    Catch ex As OracleExceptionEx When ex.Number = 1013
    '        ''ORACLEのタイムアウトのみ処理
    '        Me.Rollback = True
    '        ''終了ログの出力
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '            , "{0}.{1} OUT:RETURNCODE = {2}" _
    '            , Me.GetType.ToString _
    '            , MethodBase.GetCurrentMethod.Name _
    '            , ResultDBTimeout))
    '        Return ResultDBTimeout
    '    Catch ex As Exception
    '        Me.Rollback = True
    '        ''エラーログの出力
    '        Logger.Error(ex.Message, ex)
    '        Throw
    '    Finally
    '        ''終了処理

    '    End Try
    'End Function

    ' ''' <summary>
    ' ''' R/Oキャンセル
    ' ''' </summary>
    ' ''' <param name="rowIN">R/Oキャンセル引数</param>
    ' ''' <returns>登録結果</returns>
    ' ''' <remarks></remarks>
    ' ''' 
    ' ''' <history>
    ' ''' 2012/04/06 KN 瀧 【SERVICE_1】R/Oキャンセル時、整備受注Noでもキャンセルできるようにする
    ' ''' 2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する
    ' ''' </history>
    '<EnableCommit()>
    'Public Function DeleteOrderSave(ByVal rowIN As IC3810301inOrderSaveRow) As Long

    '    '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する START
    '    'ストール予約履歴を登録する際に必要になるのでここで宣言しておく
    '    Dim dtStallReserveInfo As IC3810301StallReserveInfoDataTable = Nothing
    '    '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する END
    '    Try
    '        ''引数をログに出力
    '        Dim args As New List(Of String)
    '        ' DataRow内の項目を列挙
    '        Me.AddLogData(args, rowIN)
    '        ''開始ログの出力
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '            , "{0}.{1} IN:{2}" _
    '            , Me.GetType.ToString _
    '            , MethodBase.GetCurrentMethod.Name _
    '            , String.Join(", ", args.ToArray())))

    '        Dim da As New IC3810301DataTableAdapter
    '        '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する START
    '        '現在日時を取得
    '        Dim nowDate As Date = DateTimeFunc.Now(rowIN.DLRCD, rowIN.STRCD)
    '        '更新対象のストール予約情報を取得する
    '        dtStallReserveInfo = da.GetStallReseveInfo(rowIN)

    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '        Dim drStallReserveInfo As IC3810301StallReserveInfoRow = Nothing

    '        '更新対象のサービス入庫情報を取得できた場合
    '        If Not dtStallReserveInfo.Rows.Count = 0 Then
    '            '更新対象のサービス入庫情報を格納
    '            drStallReserveInfo = DirectCast(dtStallReserveInfo.Rows(0), IC3810301StallReserveInfoRow)

    '            'サービス入庫テーブルのロック処理
    '            Dim sarviceInLock As Integer = LockServiceInTable(drStallReserveInfo.REZID, _
    '                                                              drStallReserveInfo.ROW_LOCK_VERSION, _
    '                                                              CancelFlg, _
    '                                                              rowIN.ACCOUNT, _
    '                                                              nowDate)
    '            'サービス入庫テーブルのロックに失敗した場合
    '            If (sarviceInLock <> ResultSuccess) Then
    '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '               , "{0}.{1} OUT:RETURNCODE = {2}" _
    '               , Me.GetType.ToString _
    '               , MethodBase.GetCurrentMethod.Name _
    '               , ResultNoMatch))
    '                Return ResultNoMatch
    '            End If

    '            ''ストール予約TBL更新
    '            'For Each drStallReserveInfo As IC3810301StallReserveInfoRow In dtStallReserveInfo
    '            '    rowIN.REZID = drStallReserveInfo.REZID
    '            '    da.DeleteDBOrderReserveSave(rowIN, nowDate)
    '            'Next




    '            rowIN.REZID = drStallReserveInfo.REZID


    '            'サービス入庫R/O削除
    '            If da.DeleteDBOrderReserveSave(rowIN, _
    '                                           nowDate, _
    '                                           drStallReserveInfo.SVC_STATUS) <= 0 Then

    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                             , "{0}.{1}.{3} OUT:RETURNCODE = {2}" _
    '                             , Me.GetType.ToString _
    '                             , MethodBase.GetCurrentMethod.Name _
    '                             , ResultNoMatch _
    '                             , "SERVICEIN_UPDATE_NG"))

    '                Me.Rollback = True
    '                Return ResultNoMatch

    '            End If


    '            '作業連番削除
    '            If da.DeleteDBWorkSeq(rowIN, nowDate) <= 0 Then
    '                Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                             , "{0}.{1}.{3} OUT:RETURNCODE = {2}" _
    '                             , Me.GetType.ToString _
    '                             , MethodBase.GetCurrentMethod.Name _
    '                             , ResultNoMatch _
    '                             , "JOBDTL_UPDATE_NG"))

    '                Me.Rollback = True
    '                Return ResultNoMatch

    '            End If

    '            '取得した行数分ループ
    '            For Each eachDate As IC3810301StallReserveInfoRow In dtStallReserveInfo.Rows

    '                'ストール利用R/O削除
    '                If da.DeleteDBOrderStallUse(rowIN, _
    '                                         nowDate, _
    '                                         eachDate.STALL_USE_ID, _
    '                                         eachDate.STALL_USE_STATUS) <= 0 Then

    '                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1}.{3} OUT:RETURNCODE = {2}" _
    '                             , Me.GetType.ToString _
    '                             , MethodBase.GetCurrentMethod.Name _
    '                             , ResultNoMatch _
    '                             , "STALLUSE_UPDATE_NG"))

    '                    Me.Rollback = True
    '                    Return ResultNoMatch

    '                End If

    '            Next


    '        End If

    '        '2013/06/17 TMEJ 成澤 【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '        '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する END

    '        ''サービス来店者キー情報の取得
    '        Dim dtVisit As IC3810301VisitKeyDataTable = da.GetVisitKey(rowIN)
    '        If dtVisit.Rows.Count = 0 Then
    '            ''該当データが存在しない場合
    '            'ログを出力する
    '            '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する START
    '            Me.Rollback = True
    '            '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する END
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} OUT:RETURNCODE = {2}" _
    '                , Me.GetType.ToString _
    '                , MethodBase.GetCurrentMethod.Name _
    '                , ResultNoMatch))
    '            Return ResultNoMatch
    '        End If

    '        '2012/04/06 KN 瀧 【SERVICE_1】R/Oキャンセル時、整備受注Noでもキャンセルできるようにする START
    '        rowIN.VISITSEQ = DirectCast(dtVisit.Rows(0), IC3810301VisitKeyRow).VISITSEQ
    '        '2012/04/06 KN 瀧 【SERVICE_1】R/Oキャンセル時、整備受注Noでもキャンセルできるようにする END

    '        Dim rowVK As IC3810301VisitKeyRow = DirectCast(dtVisit.Rows(0), IC3810301VisitKeyRow)
    '        If (rowVK.IsSACODENull = True) _
    '            OrElse (String.Compare(rowIN.SACODE, rowVK.SACODE, True, CultureInfo.CurrentCulture) <> 0) Then
    '            ' 2012/07/05 西岡 事前準備対応 START
    '            ' SA振当済みの場合
    '            If (AssignFinished.Equals(rowVK.ASSIGNSTATUS)) Then
    '                ''SAコードチェック
    '                ''SAコードが異なる場合はエラー
    '                'ログを出力する
    '                '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する START
    '                Me.Rollback = True
    '                '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する END
    '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                 , "{0}.{1} OUT:RETURNCODE = {2}" _
    '                 , Me.GetType.ToString _
    '                 , MethodBase.GetCurrentMethod.Name _
    '                 , ResultDiffSACode))
    '                Return ResultDiffSACode
    '            End If
    '        End If
    '        ''キャンセル処理
    '        da.DeleteVisitOrder(rowIN, nowDate)

    '        '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する START
    '        'ストール予約TBLの更新件数が0件でない場合はストール予約履歴TBLの登録する
    '        'For Each drStallReserveInfo As IC3810301StallReserveInfoRow In dtStallReserveInfo
    '        '    Dim commonClass As New SMBCommonClassBusinessLogic
    '        '            Dim commonReturnCode As Long =
    '        '             commonClass.RegisterStallReserveHis(rowIN.DLRCD, _
    '        '                      rowIN.STRCD, _
    '        '                      drStallReserveInfo.REZID, _
    '        '                      nowDate, _
    '        '                      Nothing)
    '        ' ''ORACLEのタイムアウト場合
    '        'If commonReturnCode = ReturnCode.ErrDBTimeout Then
    '        '	'ロールバックとログを出力する
    '        '	Me.Rollback = True
    '        '	Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        '		, "{0}.{1} OUT:RETURNCODE = {2}" _
    '        '		, Me.GetType.ToString _
    '        '		, MethodBase.GetCurrentMethod.Name _
    '        '		, ResultDBTimeout))
    '        '	Return ResultDBTimeout
    '        'End If
    '        'Next
    '        '2012/06/11 KN 小澤【SERVICE_2事前準備】ストール予約TBLの整備受注Noを更新する END

    '        ''終了ログの出力
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '         , "{0}.{1} OUT:RETURNCODE = {2}" _
    '         , Me.GetType.ToString _
    '         , MethodBase.GetCurrentMethod.Name _
    '         , ResultSuccess))
    '        Return ResultSuccess
    '    Catch ex As OracleExceptionEx When ex.Number = 1013
    '        ''ORACLEのタイムアウトのみ処理
    '        Me.Rollback = True
    '        ''終了ログの出力
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '         , "{0}.{1} OUT:RETURNCODE = {2}" _
    '         , Me.GetType.ToString _
    '         , MethodBase.GetCurrentMethod.Name _
    '         , ResultDBTimeout))
    '        Return ResultDBTimeout
    '    Catch ex As Exception
    '        Me.Rollback = True
    '        ''エラーログの出力
    '        Logger.Error(ex.Message, ex)
    '        Throw
    '    Finally

    '    End Try
    'End Function

    ''' <summary>
    ''' R/O情報登録
    ''' </summary>
    ''' <param name="serviceInId">サービス入庫ID</param>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="braunchCode">店舗コード</param>
    ''' <param name="visitSequence">訪問連番</param>
    ''' <param name="account">スタッフコード</param>
    ''' <param name="nowDataTime">処理日時</param>
    ''' <param name="applicationId">画面ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    '''  2013/06/17 TMEJ 成澤 【開発】IT9611_次世代サービス 工程管理機能開発
    ''' </history>
    Public Function InsertRepairOrderInfo(ByVal serviceInId As Decimal, _
                                          ByVal dealerCode As String, _
                                          ByVal braunchCode As String, _
                                          ByVal visitSequence As Long, _
                                          ByVal account As String, _
                                          ByVal nowDataTime As Date, _
                                          ByVal applicationId As String) As Long

        ''開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} IN.SVCIN_ID:{5}, DLR_CD:{7}, BRN_CD:{8}VISIT_SEQ:{2}, STF_CD:{3}, DATETIME:{4}, APPLICATION_ID{6}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , visitSequence.ToString(CultureInfo.CurrentCulture()) _
            , account _
            , nowDataTime.ToString(CultureInfo.CurrentCulture()) _
            , serviceInId.ToString(CultureInfo.CurrentCulture()) _
            , applicationId _
            , dealerCode _
            , braunchCode))

        '戻り値宣言
        Dim insertResult As Integer = ResultInsertNG

        '訪問連番が0以上である場合はのみ登録処理を行う
        If 0 < visitSequence Then

            Try

                'アダプター宣言
                Using adapter As New IC3810301DataTableAdapter

                    ''自動採番のRO連番IDを取得
                    Dim dtRepiarOrderRelationId As IC3810301DataSet.IC3810301RoRelationIdDataTable = adapter.GetRepiarOrderRelationId()

                    'RO連番IDを変数に格納
                    Dim repiarOrderRelationId As Decimal = 0
                    repiarOrderRelationId = CType(dtRepiarOrderRelationId.Rows.Item(0).Item("RO_RELATION_ID"), Decimal)

                    'データセット宣言
                    Using dtRepairOrderInfo As New IC3810301DataSet.IC3810301RepairOrderInfoDataTable
                        
                        'データロウ宣言
                        Dim drRepairOrderInfo As IC3810301DataSet.IC3810301RepairOrderInfoRow = _
                            CType(dtRepairOrderInfo.NewRow(), IC3810301DataSet.IC3810301RepairOrderInfoRow)

                        'データロウに値を格納
                        drRepairOrderInfo.RO_RELATION_ID = repiarOrderRelationId                      'ROリレーションID
                        drRepairOrderInfo.SVCIN_ID = serviceInId                                      'サービス入庫ID
                        drRepairOrderInfo.DLR_CD = dealerCode                                         '販売店コード
                        drRepairOrderInfo.BRN_CD = braunchCode                                        '店舗コード
                        drRepairOrderInfo.VISIT_SEQ = visitSequence                                   '訪問連番
                        drRepairOrderInfo.RO_NUM = defaultValueString                                 'RO番号
                        drRepairOrderInfo.RO_JOB_SEQ = defaultValueSeq                                'RO作業連番
                        drRepairOrderInfo.RO_STATUS = defaultRoStatus                                 'ROステータス
                        drRepairOrderInfo.RO_CHECK_STF_CD = defaultValueString                        'RO確認スタッフ
                        drRepairOrderInfo.RO_CHECK_DATETIME = Date.Parse(defaultValueData)            'RO確認日時
                        drRepairOrderInfo.RO_APPROVAL_DATETIME = Date.Parse(defaultValueData)         'RO承認日時
                        drRepairOrderInfo.ROW_CREATE_FUNCTION = applicationId                         '行作成機能
                        drRepairOrderInfo.ROW_UPDATE_FUNCTION = applicationId                         '行更新機能
                        drRepairOrderInfo.ROW_LOCK_VERSION = defaultRowVersion                        '行ロックバージョン
                        drRepairOrderInfo.RO_CREATE_DATETIME = nowDataTime                            'RO作成日時
                        drRepairOrderInfo.ROW_CREATE_DATETIME = nowDataTime                           '行作成日時
                        drRepairOrderInfo.ROW_UPDATE_DATETIME = nowDataTime                           '行更新日時
                        drRepairOrderInfo.RO_CREATE_STF_CD = account                                  'RO作成スタッフ
                        drRepairOrderInfo.ROW_CREATE_ACCOUNT = account                                '行作成スタッフ
                        drRepairOrderInfo.ROW_UPDATE_ACCOUNT = account                                '行更新スタッフ


                        'RO情報登録処理
                        If adapter.InsertRepairOrderInfo(drRepairOrderInfo) <= 0 Then

                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                  , "{0}.{1}.{3} OUT:RETURNCODE = {2}" _
                               , Me.GetType.ToString _
                               , MethodBase.GetCurrentMethod.Name _
                               , ResultNoMatch _
                               , "RepairOrderInfo_INSERT_NG"))

                            '登録失敗
                            insertResult = ResultInsertNG

                        End If

                        '登録成功
                        insertResult = ResultSuccess
                    End Using
                End Using
                'エラーログの出力
            Catch ex As OracleExceptionEx When ex.Number = 1013
                'ORACLEのタイムアウトのみ処理
                ''終了ログの出力
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , ResultDBTimeout))
                insertResult = ResultDBTimeout

            Catch ex As Exception
                Logger.Error(ex.Message, ex)
            End Try

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
         , "{0}.{1} OUT:RETURNCODE = {2}" _
         , Me.GetType.ToString _
         , MethodBase.GetCurrentMethod.Name _
         , ResultSuccess))

        Return insertResult
    End Function

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END



    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 START
    ' ''' <summary>
    ' ''' DataRow内の項目を列挙(ログ出力用)
    ' ''' </summary>
    ' ''' <param name="args">ログ項目のコレクション</param>
    ' ''' <param name="row">対象となるDataRow</param>
    ' ''' <remarks></remarks>
    'Private Sub AddLogData(ByVal args As List(Of String), ByVal row As DataRow)
    '    For Each column As DataColumn In row.Table.Columns
    '        If row.IsNull(column.ColumnName) = True Then
    '            args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
    '        Else
    '            args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, row(column.ColumnName)))
    '        End If
    '    Next
    'End Sub

    '2013/06/26 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
    '#Region "サービス入庫テーブルロック処理"

    ' ''' <summary>
    ' ''' サービス入庫テーブルロック処理
    ' ''' </summary>
    ' ''' <param name="serviceInid">サービス入庫ID</param>
    ' ''' <param name="updateCount">行ロックバージョン</param>
    ' ''' <param name="cancelFlg">キャンセルフラグ</param>
    ' ''' <param name="account">アカウント</param>
    ' ''' <param name="updateDate">更新日</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    ' ''' 
    ' ''' <history>
    ' ''' 2013/06/26 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ' ''' </history>
    'Private Function LockServiceInTable(ByVal serviceInid As Long, _
    '                                    ByVal updateCount As Long, _
    '                                    ByVal cancelFlg As String, _
    '                                    ByVal account As String, _
    '                                    ByVal updateDate As Date) As Integer
    '    ' 戻り値を設定
    '    LockServiceInTable = ResultSuccess

    '    'SMBコモンクラスの定義
    '    Using SmbCommonClass As New SMBCommonClassBusinessLogic

    '        'サービス入庫テーブルのロック処理
    '        If SmbCommonClass.LockServiceInTable(serviceInid, _
    '                                             updateCount, _
    '                                             cancelFlg, _
    '                                             account,
    '                                             updateDate, _
    '                                             APPLICATION_ID) <> ResultSuccess Then
    '            ' 戻り値にエラーを設定
    '            LockServiceInTable = ResultNoMatch

    '        End If
    '    End Using

    '    Return LockServiceInTable
    'End Function

    '#End Region
    '2013/06/26 TMEJ 成澤 【開発】IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '2013/12/10　TMEJ　成澤【開発】IT9611_次世代サービス 工程管理機能開発 END

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: マネージ状態を破棄します (マネージ オブジェクト)。
            End If

            ' TODO: アンマネージ リソース (アンマネージ オブジェクト) を解放し、下の Finalize() をオーバーライドします。
            ' TODO: 大きなフィールドを null に設定します。
        End If
        Me.disposedValue = True
    End Sub

    ' TODO: 上の Dispose(ByVal disposing As Boolean) にアンマネージ リソースを解放するコードがある場合にのみ、Finalize() をオーバーライドします。
    'Protected Overrides Sub Finalize()
    '    ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class