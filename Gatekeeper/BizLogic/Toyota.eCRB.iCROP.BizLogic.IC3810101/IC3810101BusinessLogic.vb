'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3810101BusinessLogic.vb
'─────────────────────────────────────
'機能： 来店連携ビジネスロジック
'補足： 
'作成： 2012/01/26 KN 瀧
'更新： 2012/02/28 KN 瀧 【SERVICE_1】管理予約IDがNULLの時データが取得できない不具合を修正
'更新： 2012/04/06 KN 瀧 【SERVICE_1】サービスコードの集約方法を変更
'更新： 2012/04/10 KN 瀧 【SERVICE_1】ストール予約情報の取得条件の変更
'更新： 2012/04/11 KN 佐藤 【SERVICE_1】対応済みチップと紐付く不具合を修正
'更新： 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する
'更新： 2012/04/12 KN 佐藤 【SERVICE_1】予約の紐付け時、顧客名が変更されている可能性があるため、条件から外す
'更新： 2012/04/13 KN 瀧 【SERVICE_1】サービスコードの集約方法を戻す
'更新： 2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発
'更新： 2012/08/10 TMEJ 瀧 【SERVICE_2】入庫日付替え処理の追加
'更新： 2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応
'更新： 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新： 2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発
'更新： 2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発
'更新： 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新： 2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更
'更新： 2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される
'更新： 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
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
Imports Toyota.eCRB.iCROP.DataAccess.IC3810101.IC3810101DataSet
Imports Toyota.eCRB.iCROP.DataAccess.IC3810101.IC3810101DataSetTableAdapters
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.Visit.Api.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess.ConstCode
Imports Toyota.eCRB.Visit.Api.BizLogic

''' <summary>
''' IC3810101
''' </summary>
''' <remarks>来店連携インターフェース</remarks>
Public Class IC3810101BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ''' <summary>
    ''' 成功
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultSuccess As Long = 0

    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

    ' ''' <summary>
    ' ''' エラー:SAコードが異なる
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Const ResultDiffSACode As Long = 1

    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

    ''' <summary>
    ''' エラー:DBタイムアウト
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultDBTimeout As Long = 901

    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

    ' ''' <summary>
    ' ''' エラー:該当データなし
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Const ResultNoMatch As Long = 902

    ' ''' <summary>
    ' ''' エラー:
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Const RegisterError As Long = 908

    '' 2012/04/11 KN 佐藤 【SERVICE_1】対応済みチップと紐付く不具合を修正 START
    ' ''' <summary>
    ' ''' 実績ステータス:未入庫
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Const StatusNoReceiving As String = "00"

    ' ''' <summary>
    ' ''' 実績ステータス:入庫
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Const StatusReceiving As String = "10"

    ' ''' <summary>
    ' ''' 実績ステータス:仮置き
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Const StatusTemporary As String = "32"

    ' ''' <summary>
    ' ''' 実績ステータス:未来店客
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Public Const StatusNoVisit As String = "33"

    '' 2012/04/11 KN 佐藤 【SERVICE_1】対応済みチップと紐付く不具合を修正 END

    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    ''' <summary>
    ''' アプリケーションID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApplicationID As String = "IC3810101"

    ''' <summary>
    ''' １：自社客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustSegmentMyCustomer As String = "1"

    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

    ' ''' <summary>
    ' ''' ２：未取引客
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const CustSegmentNewCustomer As String = "2"

    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

    '2014/01/17 TMEJ 陳 TMEJ次世代サービス 工程管理機能開発 START
    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

    ' ''' <summary>
    ' ''' 権限コード：案内係
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const OperationCodeSvr As Integer = 52

    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

    ''' <summary>
    ''' DELETE FLG
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DelFlg_0 As String = "0"

    ''' <summary>
    ''' 敬称利用区分("1"：後方)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeBack As String = "1"

    ''' <summary>
    ''' 敬称利用区分("2"：前方)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeFront As String = "2"

    '2014/01/17 TMEJ 陳 TMEJ次世代サービス 工程管理機能開発 END

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '2014/01/17 TMEJ 陳 TMEJ次世代サービス 工程管理機能開発 START

    ''' <summary>
    ''' 文言
    ''' </summary>
    Private Enum WordID

        ''' <summary>
        ''' ご来店
        ''' </summary>
        Visit = 1
        ''' <summary>
        ''' お客様
        ''' </summary>
        Customer = 2
        ''' <summary>
        ''' ～
        ''' </summary>
        Mark = 3

    End Enum

    '2014/01/17 TMEJ 陳 TMEJ次世代サービス 工程管理機能開発 END

    ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
    ''' <summary>
    ''' 顧客車両区分(所有者)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const VehicleTypeOwner As String = "1"

    ''' <summary>
    ''' 2：見込み客
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CustSegmentNewCustomer As String = "2"
    ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

#End Region

    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 START

#Region "プロパティ"

    'VisitSeq保持
    Private visitSeqValue As Long

    ''' <summary>
    ''' VisitSeq保持用プロパティ
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property VisitSeqInserted As Long

        Set(ByVal value As Long)

            visitSeqValue = value

        End Set

        Get

            Return visitSeqValue

        End Get

    End Property

#End Region

    '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 END

    ''' <summary>
    ''' サービス来店者登録
    ''' </summary>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="storeCD">店舗コード</param>
    ''' <param name="visitTime">来店日時</param>
    ''' <param name="vehicleRegistration">車両登録No</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="customerId">顧客コード</param>
    ''' <param name="staffCD">顧客担当スタッフコード</param>
    ''' <param name="visitPerson">来店人数</param>
    ''' <param name="visitMeans">来店手段</param>
    ''' <param name="vin">VIN</param>
    ''' <param name="vehicleID">車両ID</param>
    ''' <param name="sex">性別</param>
    ''' <param name="name">氏名</param>
    ''' <param name="defaultSACode">SAコード</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="system">機能ID</param>
    ''' <param name="serviceinId">サービス入庫ID</param>
    ''' <returns>登録結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' 2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11
    ''' 2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発
    ''' 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
    ''' </history>
    <EnableCommit()>
    Public Function InsertServiceVisit(ByVal dealerCD As String _
                                     , ByVal storeCD As String _
                                     , ByVal visitTime As Date _
                                     , ByVal vehicleRegistration As String _
                                     , ByVal customerSegment As String _
                                     , ByVal customerId As String _
                                     , ByVal staffCD As String _
                                     , ByVal visitPerson As Short _
                                     , ByVal visitMeans As String _
                                     , ByVal vin As String _
                                     , ByVal vehicleId As Decimal _
                                     , ByVal sex As String _
                                     , ByVal name As String _
                                     , ByVal defaultSACode As String _
                                     , ByVal account As String _
                                     , ByVal userName As String _
                                     , ByVal system As String _
                                     , ByVal serviceinId As Decimal
                                       ) As Long
        '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
        'Public Function InsertServiceVisit(ByVal dealerCD As String _
        '                                 , ByVal storeCD As String _
        '                                 , ByVal visitTime As Date _
        '                                 , ByVal vehicleRegistration As String _
        '                                 , ByVal customerSegment As String _
        '                                 , ByVal customerId As String _
        '                                 , ByVal staffCD As String _
        '                                 , ByVal visitPerson As Short _
        '                                 , ByVal visitMeans As String _
        '                                 , ByVal vin As String _
        '                                 , ByVal vehicleId As Decimal _
        '                                 , ByVal sex As String _
        '                                 , ByVal name As String _
        '                                 , ByVal defaultSACode As String _
        '                                 , ByVal account As String _
        '                                 , ByVal userName As String _
        '                                 , ByVal system As String _
        '                                   ) As Long
        '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
        'Public Function InsertServiceVisit(ByVal dealerCD As String _
        '                                 , ByVal storeCD As String _
        '                                 , ByVal visitTime As Date _
        '                                 , ByVal vehicleRegistration As String _
        '                                 , ByVal customerSegment As String _
        '                                 , ByVal customerId As String _
        '                                 , ByVal staffCD As String _
        '                                 , ByVal visitPerson As Short _
        '                                 , ByVal visitMeans As String _
        '                                 , ByVal vin As String _
        '                                 , ByVal vehicleId As Long _
        '                                 , ByVal sex As String _
        '                                 , ByVal name As String _
        '                                 , ByVal defaultSACode As String _
        '                                 , ByVal account As String _
        '                                 , ByVal system As String _
        '                                   ) As Long
        '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END
        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        'Public Function InsertServiceVisit(ByVal dealerCD As String _
        '                               , ByVal storeCD As String _
        '                               , ByVal visitTime As Date _
        '                               , ByVal vehicleRegistration As String _
        '                               , ByVal customerSegment As String _
        '                               , ByVal customerId As String _
        '                               , ByVal staffCD As String _
        '                               , ByVal visitPerson As Short _
        '                               , ByVal visitMeans As String _
        '                               , ByVal vin As String _
        '                               , ByVal sequenceNo As Long _
        '                               , ByVal sex As String _
        '                               , ByVal name As String _
        '                               , ByVal defaultSACode As String _
        '                               , ByVal account As String _
        '                               , ByVal system As String _
        '                               ) As Long

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} P9:{10} P10:{11} P11:{12} P12:{13} P13:{14} P14:{15} P15:{16} P16:{17} " _
                       , Me.GetType.ToString _
                       , MethodBase.GetCurrentMethod.Name _
                       , dealerCD, storeCD, visitTime, vehicleRegistration, customerSegment _
                       , customerId, staffCD, visitPerson, visitMeans, vin _
                       , vehicleId, sex, name, defaultSACode, account, system))
        Try
            Using dt As New IC3810101inServiceVisitDataTable

                Dim row As IC3810101inServiceVisitRow = dt.NewIC3810101inServiceVisitRow

                row.DLRCD = dealerCD
                row.STRCD = storeCD

                If visitTime <> Date.MinValue Then
                    row.VISITTIMESTAMP = visitTime
                End If

                row.VCLREGNO = vehicleRegistration
                row.CUSTSEGMENT = customerSegment

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                '0を設定
                row.CUSTOMERCODE = 0

                '顧客ID
                '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START
                'If Not Long.TryParse(customerId, row.CUSTOMERCODE) Then

                '    '0を設定
                '    row.CUSTOMERCODE = 0

                'End If

                If Not Decimal.TryParse(customerId, row.CUSTOMERCODE) Then

                    '0を設定
                    row.CUSTOMERCODE = 0

                End If
                '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                row.STAFFCD = staffCD
                row.VISITPERSONNUM = visitPerson
                row.VISITMEANS = visitMeans
                row.VIN = vin

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'row.SEQNO = sequenceNo
                row.VCL_ID = vehicleId

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                row.SEX = sex
                row.CUSTOMERNAME = name
                row.DEFAULTSACODE = defaultSACode
                row.ACCOUNT = account
                '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 START
                row.USERNAME = userName
                '2014/01/17 TMEJ 陳   TMEJ次世代サービス 工程管理機能開発 $11 END
                row.SYSTEM = system
                '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
                row.SVCIN_ID = serviceinId
                '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

                Return InsertServiceVisit(row)

            End Using

        Catch ex As OracleExceptionEx When ex.Number = 1013

            ''ORACLEのタイムアウトのみ処理
            Me.Rollback = True

            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT:RETURNCODE = {2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , ResultDBTimeout))

            Return ResultDBTimeout

        Catch ex As Exception

            Me.Rollback = True
            ''エラーログの出力

            Logger.Error(ex.Message, ex)
            Throw
        End Try
    End Function

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    ' ''' <summary>
    ' ''' サービス来店者登録
    ' ''' </summary>
    ' ''' <param name="rowIN">サービス来店者引数</param>
    ' ''' <returns>登録結果</returns>
    ' ''' <remarks></remarks>
    ' ''' 
    ' ''' <history>
    ' ''' 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する
    ' ''' </history>
    'Protected Function InsertServiceVisit(ByVal rowIN As IC3810101inServiceVisitRow) As Long

    '    ''引数をログに出力
    '    Dim args As New List(Of String)
    '    ' DataRow内の項目を列挙
    '    Me.AddLogData(args, rowIN)
    '    ''開始ログの出力
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} IN:{2}" _
    '        , Me.GetType.ToString _
    '        , MethodBase.GetCurrentMethod.Name _
    '        , String.Join(", ", args.ToArray())))

    '    Using da As New IC3810101DataTableAdapter

    '        ''2012/01/06 追加 サービス来店者情報に基幹顧客ID,モデルコード,電話番号,携帯番号項目の追加
    '        ''来客者キー情報の取得
    '        Dim dtCustomer As IC3810101CustomerKeyDataTable = da.GetCustomerKey(rowIN)
    '        Dim rowCustomer As IC3810101CustomerKeyRow
    '        If dtCustomer.Rows.Count = 0 Then
    '            ''来客者情報が存在しない場合
    '            rowCustomer = dtCustomer.NewIC3810101CustomerKeyRow
    '        Else
    '            ''来客者情報が存在する場合
    '            rowCustomer = DirectCast(dtCustomer.Rows(0), IC3810101CustomerKeyRow)
    '        End If

    '        ''ストール予約キー情報の取得
    '        ' 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する START
    '        'Dim dtStall As IC3810101StallKeyDataTable = da.GetStallKey(rowIN)
    '        Dim dtStall As IC3810101StallKeyDataTable = da.GetStallKey(rowIN, rowCustomer)
    '        ' 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する END
    '        Dim rowStall As IC3810101StallKeyRow
    '        If dtStall.Rows.Count = 0 Then
    '            ''ストール予約が存在しない場合
    '            rowStall = dtStall.NewIC3810101StallKeyRow
    '            rowStall.REZID = -1
    '            rowStall.SERVICECODE_CONV = " "
    '        Else
    '            ' 2012/04/11 KN 佐藤 【SERVICE_1】対応済みチップと紐付く不具合を修正 START
    '            ' ''ストール予約が存在する場合
    '            'rowStall = DirectCast(dtStall.Rows(0), IC3810101StallKeyRow)

    '            ''有効予約のチェック
    '            rowStall = Nothing
    '            For Each rowStallCheck As IC3810101StallKeyRow In dtStall.Rows
    '                ''有効予約の場合
    '                If ReserveCheck(rowIN, rowStallCheck) = True Then
    '                    ''予約情報を取得する
    '                    rowStall = rowStallCheck
    '                    Exit For
    '                End If
    '            Next
    '            ''有効予約が存在しない場合
    '            If rowStall Is Nothing Then
    '                rowStall = dtStall.NewIC3810101StallKeyRow
    '                rowStall.REZID = -1
    '                rowStall.SERVICECODE_CONV = " "
    '            End If
    '            ' 2012/04/11 KN 佐藤 【SERVICE_1】対応済みチップと紐付く不具合を修正 END
    '        End If

    '        '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 START
    '        '「予約情報：顧客区分=1：未取引客 AND サービス来店者引数：顧客区分=1：自社客」の場合
    '        If 0 < rowStall.REZID AndAlso "1".Equals(rowStall.CUSTOMERFLAG) AndAlso "1".Equals(rowIN.CUSTSEGMENT) Then
    '            Dim updateStallCount As Integer = da.UpdateStallReserveInfo(rowIN, rowCustomer, rowStall)

    '            If updateStallCount <> 0 Then
    '                Using commonClass As New SMBCommonClassBusinessLogic
    '                    Dim updateStallHistoryCount As Long =
    '                        commonClass.RegisterStallReserveHis(rowIN.DLRCD, _
    '                                                            rowIN.STRCD, _
    '                                                            rowStall.REZID, _
    '                                                            DateTimeFunc.Now, _
    '                                                            RegisterType.ReserveHisIndividual)
    '                End Using
    '            End If
    '        End If
    '        '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 END

    '        ''サービス来店者登録
    '        rowIN.VISITSEQ = da.InsertServiceVisit(rowIN, rowCustomer, rowStall)
    '        ''ログの出力
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '            , "{0}.{1} DATA:VISITSEQ = {2}" _
    '            , Me.GetType.ToString _
    '            , MethodBase.GetCurrentMethod.Name _
    '            , rowIN.VISITSEQ))
    '    End Using
    '    ''終了ログの出力
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} OUT:RETURNCODE = {2}" _
    '        , Me.GetType.ToString _
    '        , MethodBase.GetCurrentMethod.Name _
    '        , ResultSuccess))
    '    Return ResultSuccess
    'End Function

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    ''' <summary>
    ''' サービス来店者登録
    ''' </summary>
    ''' <param name="rowIN">サービス来店者引数</param>
    ''' <returns>登録結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発
    ''' </history>
    Protected Function InsertServiceVisit(ByVal rowIN As IC3810101inServiceVisitRow) As Long

        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, rowIN)

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

        Using IC3810101Dac As New IC3810101DataTableAdapter

            Dim dtVisitRegistInfo As New IC3810101VisitRegistInfoDataTable
            '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
            ''顧客IDと車両登録番号の確認
            'If 0 < rowIN.CUSTOMERCODE _
            '    OrElse 0 < rowIN.VCL_ID Then
            '    '顧客IDと車両IDのどちらかが存在している場合

            '    '予約情報の取得
            '    dtVisitRegistInfo = IC3810101Dac.GetReserveInfo(rowIN)


            'End If

            ' サービス入庫IDが存在している場合
            If 0 < rowIN.SVCIN_ID Then
                'サービス入庫IDから予約情報を取得

                dtVisitRegistInfo = IC3810101Dac.GetReserveInfoBySvcinId(rowIN.SVCIN_ID)

                '顧客IDと車両登録番号の確認
            ElseIf 0 < rowIN.CUSTOMERCODE _
                OrElse 0 < rowIN.VCL_ID Then
                '顧客IDと車両IDのどちらかが存在している場合

                '予約情報の取得
                dtVisitRegistInfo = IC3810101Dac.GetReserveInfo(rowIN)

            End If
            '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END



            Dim rowVisitRegistInfo As IC3810101VisitRegistInfoRow

            '予約情報の取得確認
            If dtVisitRegistInfo.Count = 0 Then
                '予約情報が取得できなかった場合

                '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
                ''イレギュラー対応
                ''自社客で来店して、未取引客で予約がある場合
                ''未取引客で送信され、自社客で予約がある場合の予約検索処理
                'rowVisitRegistInfo = GetReserveInfoIrregular(rowIN, IC3810101Dac)

                ' サービス入庫IDが「-1:予約を引き当てない」の場合
                If rowIN.SVCIN_ID < 0 Then

                    '空の行を作成
                    rowVisitRegistInfo = dtVisitRegistInfo.NewIC3810101VisitRegistInfoRow

                    '顧客コード、
                    rowVisitRegistInfo.CUSTCD = 0
                    rowVisitRegistInfo.VCL_ID = 0
                    rowVisitRegistInfo.REZID = -1
                Else
                    'イレギュラー対応
                    '自社客で来店して、未取引客で予約がある場合
                    '未取引客で送信され、自社客で予約がある場合の予約検索処理
                    rowVisitRegistInfo = GetReserveInfoIrregular(rowIN, IC3810101Dac)
                End If
                '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
            Else
                '予約情報が取得できた場合

                '行に変換
                rowVisitRegistInfo = DirectCast(dtVisitRegistInfo.Rows(0), IC3810101VisitRegistInfoRow)

            End If

                '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

                ''サービス来店者登録
                'rowIN.VISITSEQ = IC3810101Dac.InsertServiceVisit(rowIN, rowVisitRegistInfo)

                '現在日時取得
                Dim nowDate As Date = DateTimeFunc.Now(rowIN.DLRCD)

            '車両登録番号のチェック
            '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
            'If Not (rowIN.IsVCLREGNONull) AndAlso Not (String.IsNullOrEmpty(rowIN.VCLREGNO)) Then
            If Not (rowIN.IsVCLREGNONull) AndAlso Not (String.IsNullOrEmpty(rowIN.VCLREGNO)) AndAlso rowIN.SVCIN_ID <= 0 Then
                '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END
                '存在する場合
                '来店件数情報を取得する
                Dim dtVisitInfoCount As IC3810101VisitInfoCountDataTable = _
                    IC3810101Dac.GetVisitCountInfo(rowIN, _
                                                   rowVisitRegistInfo, _
                                                   nowDate)

                '来店件数情報のチェック
                If dtVisitInfoCount(0).VISITINFO_COUNT = 0 Then
                    '件数は0件の場合
                    'サービス来店者登録
                    rowIN.VISITSEQ = IC3810101Dac.InsertServiceVisit(rowIN, rowVisitRegistInfo, nowDate)

                Else
                    '上記以外の場合
                    '来店IDに「-1」を固定で設定
                    '※来店通知をするしないの判定に使用するため
                    rowIN.VISITSEQ = -1

                End If

            Else
                '上記以外の場合
                'サービス来店者登録
                rowIN.VISITSEQ = IC3810101Dac.InsertServiceVisit(rowIN, rowVisitRegistInfo, nowDate)

            End If

            '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

            '後処理
            dtVisitRegistInfo.Dispose()
            dtVisitRegistInfo = Nothing

            '2014/01/17 TMEJ 陳 TMEJ次世代サービス 工程管理機能開発 START

            'VisitSeq保持
            Me.visitSeqValue = rowIN.VISITSEQ

            '2014/01/17 TMEJ 陳 TMEJ次世代サービス 工程管理機能開発 END


            'ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} DATA:VISITSEQ = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , rowIN.VISITSEQ))

        End Using

        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , ResultSuccess))

        Return ResultSuccess

    End Function

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START
    ''' <summary>
    ''' サービス来店者取消
    ''' </summary>
    ''' <param name="visitSeqList">選択した予約に紐付く来店実績連番のリスト</param>
    ''' <returns>登録結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Function DeleteServiceVisit(ByVal visitSeqList As List(Of Decimal)) As Long

        Try

            '開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            Using IC3810101Dac As New IC3810101DataTableAdapter

                ' 選択した予約に紐付くサービス来店者管理をPastに退避する
                Dim insertCount As Integer = IC3810101Dac.InsertServiceVisitMngPast(visitSeqList)

                ' サービス来店者管理退避に成功した場合
                If 0 < insertCount Then

                    ' 選択した予約に紐付くサービス来店者管理を削除する
                    IC3810101Dac.DeleteServiceVisitMng(visitSeqList)

                End If

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                      , "{0}.{1} END" _
                                      , Me.GetType.ToString _
                                      , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return ResultSuccess

        Catch ex As OracleExceptionEx When ex.Number = 1013

            ''ORACLEのタイムアウトのみ処理
            Me.Rollback = True

            ''終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} OUT:RETURNCODE = {2}" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name _
                , ResultDBTimeout))

            Return ResultDBTimeout

        Catch ex As Exception

            Me.Rollback = True
            ''エラーログの出力

            Logger.Error(ex.Message, ex)
            Throw
        End Try

    End Function
    '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    ''' <summary>
    ''' 予約情報取得(イレギュラー対応)
    ''' </summary>
    ''' <param name="rowIN">サービス来店者引数</param>
    ''' <returns>予約情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function GetReserveInfoIrregular(ByVal rowIN As IC3810101inServiceVisitRow, _
                                             ByVal IC3810101Dac As IC3810101DataTableAdapter) _
                                             As IC3810101VisitRegistInfoRow

        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, rowIN)

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))


        Dim dtVisitRegistInfo As IC3810101VisitRegistInfoDataTable

        Dim rowVisitRegistInfo As IC3810101VisitRegistInfoRow


        '来店した顧客の顧客種別の判定
        If Not rowIN.IsCUSTSEGMENTNull AndAlso CustSegmentMyCustomer.Equals(rowIN.CUSTSEGMENT) Then
            '自社客

            '顧客IDと車両登録番号の確認
            If 0 < rowIN.CUSTOMERCODE _
                OrElse 0 < rowIN.VCL_ID Then
                '顧客IDと車両IDのどちらかが存在している場合

                '自社客予約情報の取得(自社客で送信されて未取引客での予約がある場合のフォロー)
                dtVisitRegistInfo = IC3810101Dac.GetMyCustReserveInfo(rowIN)

            Else
                'どちらも存在しない場合

                'NOTHING
                dtVisitRegistInfo = Nothing

            End If

        Else
            '未取引客

            '車両登録番号確認
            If rowIN.IsVCLREGNONull OrElse String.IsNullOrEmpty(rowIN.VCLREGNO) Then
                '車両登録番号が存在しない

                'NOTHING
                dtVisitRegistInfo = Nothing

            Else
                '車両登録番号が存在する

                '未取引客予約情報の取得(未取引客で送信されて自社客での予約がある場合のフォロー)
                dtVisitRegistInfo = IC3810101Dac.GetNewCustReserveInfo(rowIN)

            End If

        End If

        ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
        ' 車両登録番号が存在しかつ予約情報が取得できなかった場合
        If (rowIN.IsVCLREGNONull = False OrElse String.IsNullOrEmpty(rowIN.VCLREGNO) = False) AndAlso _
           (dtVisitRegistInfo Is Nothing OrElse dtVisitRegistInfo.Count = 0) Then

            ' 2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
            '' 全ユーザー予約情報取得
            'AllUserReserveInfo(dtVisitRegistInfo, rowIN, IC3810101Dac)

            Dim beforeVclRegNo As String = rowIN.VCLREGNO

            '車両登録番号検索ワード変換を行う
            Dim visitReception As New VisitReceptionBusinessLogic
            rowIN.VCLREGNO = visitReception.ConvertVclRegNumWord(rowIN.VCLREGNO)

            ' 全ユーザー予約情報取得
            AllUserReserveInfo(dtVisitRegistInfo, rowIN, IC3810101Dac)

            rowIN.VCLREGNO = beforeVclRegNo
            ' 2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

        End If

        ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        '予約情報の取得確認
        If dtVisitRegistInfo Is Nothing OrElse dtVisitRegistInfo.Count = 0 Then
            '取得できなかった場合

            '顧客情報だけを再度取得する
            rowVisitRegistInfo = Me.GetCustomerInfo(rowIN, IC3810101Dac)

            '予約IDに-1を設定
            rowVisitRegistInfo.REZID = -1

        Else
            '取得できた場合

            '行に変換
            rowVisitRegistInfo = DirectCast(dtVisitRegistInfo.Rows(0), IC3810101VisitRegistInfoRow)

        End If


        '終了ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OUT:RETURNCODE = {2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , ResultSuccess))

        Return rowVisitRegistInfo

    End Function

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

    ''' <summary>
    ''' 全ユーザー予約情報確認
    ''' </summary>
    ''' <param name="dtVisitRegistInfo">予約情報</param>
    ''' <param name="rowIN">サービス来店者引数</param>
    ''' <param name="IC3810101Dac">Dac</param>
    ''' <remarks></remarks>
    Private Sub AllUserReserveInfo( _
                    ByRef dtVisitRegistInfo As IC3810101VisitRegistInfoDataTable, _
                    ByVal rowIN As IC3810101inServiceVisitRow, _
                    ByVal IC3810101Dac As IC3810101DataTableAdapter)

        ' 全ユーザー予約情報の取得
        dtVisitRegistInfo = IC3810101Dac.GetAllUserReserveInfo(rowIN)

        ' 全ユーザー予約情報が取得できた場合
        If dtVisitRegistInfo IsNot Nothing AndAlso dtVisitRegistInfo.Count <> 0 Then

            Dim vclId As Decimal = 0

            ' 取得した予約情報の顧客車両区分判断
            If VehicleTypeOwner.Equals(dtVisitRegistInfo(0).CST_VCL_TYPE) = False Then

                ' 1：所有者以外の場合、車両IDを保持
                vclId = dtVisitRegistInfo(0).VCL_ID
            Else

                ' 上記以外の場合
                '取得した予約情報の顧客が未取引客 かつ GKから送信された顧客が自社客の場合
                If CustSegmentNewCustomer.Equals(dtVisitRegistInfo(0).CUSTOMERFLAG) AndAlso _
                   CustSegmentMyCustomer.Equals(rowIN.CUSTSEGMENT) Then

                    ' GKから送信された車両IDを保持
                    vclId = rowIN.VCL_ID
                End If

            End If

            ' ローカル変数.車両IDに有効な値が設定されている場合
            If vclId > 0 Then

                ' オーナー顧客情報を取得する
                Dim dtOwnerCustomerInfo As IC3810101OwnerCustomerInfoDataTable =
                    IC3810101Dac.GetOwnerCustomerInfo(rowIN.DLRCD, vclId)

                ' オーナー顧客情報が取得できた場合
                If dtOwnerCustomerInfo IsNot Nothing AndAlso dtOwnerCustomerInfo.Count <> 0 Then

                    ' 予約情報の値をオーナーの情報に書き換える
                    dtVisitRegistInfo(0).CUSTOMERFLAG = dtOwnerCustomerInfo(0).CST_TYPE
                    dtVisitRegistInfo(0).CUSTOMERNAME = dtOwnerCustomerInfo(0).CST_NAME
                    dtVisitRegistInfo(0).TELNO = dtOwnerCustomerInfo(0).CST_PHONE
                    dtVisitRegistInfo(0).MOBILE = dtOwnerCustomerInfo(0).CST_MOBILE
                    dtVisitRegistInfo(0).SEX = dtOwnerCustomerInfo(0).CST_GENDER
                    dtVisitRegistInfo(0).DMSID = dtOwnerCustomerInfo(0).DMS_CST_CD
                    dtVisitRegistInfo(0).VIN = dtOwnerCustomerInfo(0).VCL_VIN
                    dtVisitRegistInfo(0).MODELCODE = dtOwnerCustomerInfo(0).VCL_KATASHIKI

                End If

            End If

        End If
    End Sub

    ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    ''' <summary>
    ''' 顧客・車両情報取得
    ''' </summary>
    ''' <param name="rowIN">サービス来店者引数</param>
    ''' <returns>顧客情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
    ''' </history>
    Private Function GetCustomerInfo(ByVal rowIN As IC3810101inServiceVisitRow, _
                                     ByVal IC3810101Dac As IC3810101DataTableAdapter) _
                                     As IC3810101VisitRegistInfoRow

        '引数をログに出力
        Dim args As New List(Of String)

        'DataRow内の項目を列挙
        Me.AddLogData(args, rowIN)

        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} IN:{2}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , String.Join(", ", args.ToArray())))

        Using dtVisitRegistInfo As New IC3810101VisitRegistInfoDataTable

            '新しい行の作成
            Dim rowVisitRegistInfo As IC3810101VisitRegistInfoRow = _
                      dtVisitRegistInfo.NewIC3810101VisitRegistInfoRow

            '車両ID・顧客IDのみ初期値設定
            rowVisitRegistInfo.CUSTCD = 0
            rowVisitRegistInfo.VCL_ID = 0

            '顧客コードの確認
            If 0 < rowIN.CUSTOMERCODE Then
                '顧客IDが存在する場合

                '顧客情報の取得
                Dim dtCustomerInfo As IC3810101CustomerInfoDataTable = IC3810101Dac.GetCustomerInfo(rowIN)

                '顧客情報取得確認
                If 0 < dtCustomerInfo.Count Then
                    '取得成功

                    '行に変換
                    Dim rowCustomerInfo As IC3810101CustomerInfoRow = _
                        DirectCast(dtCustomerInfo.Rows(0), IC3810101CustomerInfoRow)

                    '顧客ID
                    rowVisitRegistInfo.CUSTCD = rowCustomerInfo.CUSTCD

                    '基幹顧客コード
                    If Not rowCustomerInfo.IsDMSIDNull Then

                        rowVisitRegistInfo.DMSID = rowCustomerInfo.DMSID

                    End If

                    '顧客氏名
                    If Not rowCustomerInfo.IsCUSTOMERNAMENull Then

                        rowVisitRegistInfo.CUSTOMERNAME = rowCustomerInfo.CUSTOMERNAME

                    End If

                    '電話番号
                    If Not rowCustomerInfo.IsTELNONull Then

                        rowVisitRegistInfo.TELNO = rowCustomerInfo.TELNO

                    End If

                    '携帯番号
                    If Not rowCustomerInfo.IsMOBILENull Then

                        rowVisitRegistInfo.MOBILE = rowCustomerInfo.MOBILE

                    End If

                    '性別
                    If Not rowCustomerInfo.IsSEXNull Then

                        rowVisitRegistInfo.SEX = rowCustomerInfo.SEX

                    End If

                    '顧客種別
                    rowVisitRegistInfo.CUSTOMERFLAG = rowCustomerInfo.CUSTOMERFLAG

                End If
            End If

            '車両IDの確認
            If 0 < rowIN.VCL_ID Then
                '車両IDが存在する場合

                '車両情報の取得
                Dim dtVehicleInfo As IC3810101VehicleInfoDataTable = IC3810101Dac.GetVehicleInfo(rowIN)

                '車両情報取得確認
                If 0 < dtVehicleInfo.Count Then
                    '取得成功

                    '行に変換
                    Dim rowVehicleInfo As IC3810101VehicleInfoRow = _
                        DirectCast(dtVehicleInfo.Rows(0), IC3810101VehicleInfoRow)

                    '車両ID
                    rowVisitRegistInfo.VCL_ID = rowVehicleInfo.VCL_ID

                    'VIN
                    If Not rowVehicleInfo.IsVINNull Then

                        rowVisitRegistInfo.VIN = rowVehicleInfo.VIN

                    End If

                    'モデルコード
                    If Not rowVehicleInfo.IsMODELCODENull Then

                        rowVisitRegistInfo.MODELCODE = rowVehicleInfo.MODELCODE

                    End If
                End If
            End If

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:RETURNCODE = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , ResultSuccess))

            Return rowVisitRegistInfo

        End Using

    End Function

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    '' 2012/04/11 KN 佐藤 【SERVICE_1】START
    ' ''' <summary>
    ' ''' 有効予約チェック
    ' ''' </summary>
    ' ''' <param name="rowIN">サービス来店者引数</param>
    ' ''' <param name="rowStallKey">ストール予約情報</param>
    ' ''' <returns>チェック結果（True:有効、False：無効）</returns>
    ' ''' <remarks></remarks>
    ' ''' 
    ' ''' <history>
    ' ''' </history>
    'Private Function ReserveCheck(ByVal rowIN As IC3810101inServiceVisitRow, _
    '                              ByVal rowStallKey As IC3810101StallKeyRow) As Boolean
    '    ''戻り値を初期化
    '    Dim retValue As Boolean = False
    '    ''引数をログに出力
    '    Dim args As New List(Of String)
    '    ' DataRow内の項目を列挙
    '    Me.AddLogData(args, rowStallKey)
    '    ''開始ログの出力
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} IN:{2}" _
    '        , Me.GetType.ToString _
    '        , MethodBase.GetCurrentMethod.Name _
    '        , String.Join(", ", args.ToArray())))

    '    Using da As New IC3810101DataTableAdapter

    '        ''ストール実績情報を取得
    '        Dim processInfo As IC3810101ProcessInfoDataTable = da.GetProcessInfo(rowIN, rowStallKey)

    '        ''実績情報が存在しない場合
    '        If processInfo.Rows.Count = 0 Then
    '            ''有効予約
    '            retValue = True
    '        End If

    '        ''実績情報が存在する場合、実績をチェック
    '        For Each rowProcessInfo As IC3810101ProcessInfoRow In processInfo.Rows

    '            ''日跨ぎシーケンス番号が開始されている場合
    '            If rowProcessInfo.DSEQNO > 0 Then
    '                ''無効予約
    '                retValue = False
    '                Exit For
    '            End If

    '            ''シーケンス番号が開始されている場合
    '            If rowProcessInfo.SEQNO > 1 Then
    '                ''無効予約
    '                retValue = False
    '                Exit For
    '            End If

    '            ''実績ステータスのチェック
    '            Select Case rowProcessInfo.RESULT_STATUS

    '                Case StatusNoReceiving, StatusReceiving, StatusTemporary, StatusNoVisit
    '                    ''未入庫、入庫、仮置き、未来店客の場合、有効予約
    '                    retValue = True
    '                    Exit For
    '                Case Else
    '                    ''上記以外の場合、無効予約
    '                    retValue = False
    '                    Exit For
    '            End Select

    '            Exit For
    '        Next

    '    End Using

    '    ''終了ログの出力
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} OUT:RETURNCODE = {2}" _
    '        , Me.GetType.ToString _
    '        , MethodBase.GetCurrentMethod.Name _
    '        , retValue))
    '    Return retValue
    'End Function
    '' 2012/04/11 KN 佐藤 【SERVICE_1】END

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    ' ''2012/08/01 KN 瀧 【A.STEP2】SA ストール予約受付機能開発 START
    ' ''' <summary>
    ' ''' サービス来店者登録(SA用)
    ' ''' </summary>
    ' ''' <param name="dealerCD">販売店コード</param>
    ' ''' <param name="storeCD">店舗コード</param>
    ' ''' <param name="vehicleRegNo">車両登録No</param>
    ' ''' <param name="saCode">SAコード</param>
    ' ''' <param name="account">アカウント</param>
    ' ''' <param name="system">機能ID</param>
    ' ''' <param name="visitSeq">来店実績連番</param>
    ' ''' <returns>登録結果</returns>
    ' ''' <remarks></remarks>
    ' ''' 
    ' ''' <history>
    ' ''' 2012/08/01 KN 瀧 【A.STEP2】SA ストール予約受付機能開発
    ' ''' </history>
    '<EnableCommit()>
    'Public Function InsertServiceVisitSA(ByVal dealerCD As String _
    '                                   , ByVal storeCD As String _
    '                                   , ByVal vehicleRegNo As String _
    '                                   , ByVal saCode As String _
    '                                   , ByVal account As String _
    '                                   , ByVal system As String _
    '                                   , ByRef visitSeq As Long _
    '                                   ) As Long
    '    Dim result As Long = -1
    '    ''引数をログに出力
    '    Dim args As New List(Of String)
    '    ''販売店コード
    '    args.Add(String.Format(CultureInfo.CurrentCulture, "dealerCD = {0}", dealerCD))
    '    ''店舗コード
    '    args.Add(String.Format(CultureInfo.CurrentCulture, "storeCD = {0}", storeCD))
    '    ''車両登録No
    '    args.Add(String.Format(CultureInfo.CurrentCulture, "vehicleRegNo = {0}", vehicleRegNo))
    '    ''SAコード
    '    args.Add(String.Format(CultureInfo.CurrentCulture, "saCode = {0}", saCode))
    '    ''アカウント
    '    args.Add(String.Format(CultureInfo.CurrentCulture, "account = {0}", account))
    '    ''機能ID
    '    args.Add(String.Format(CultureInfo.CurrentCulture, "system = {0}", system))
    '    ''開始ログの出力
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} IN:{2}" _
    '        , Me.GetType.ToString _
    '        , MethodBase.GetCurrentMethod.Name _
    '        , String.Join(", ", args.ToArray())))
    '    Try
    '        ''システム日時の取得
    '        Dim sysDate As DateTime = DateTimeFunc.Now(dealerCD, storeCD)
    '        Using da As New IC3810101DataTableAdapter
    '            ''------------------------------
    '            ''顧客情報の取得
    '            ''------------------------------
    '            Dim rowCV As IC3810101CustomerVehicleInfoRow = da.GetCustomerVehicleInfo(dealerCD, storeCD, vehicleRegNo)
    '            ''------------------------------
    '            ''サービス来店者の新規作成
    '            ''------------------------------
    '            Dim row As IC3810101inServiceVisitRow = (New IC3810101inServiceVisitDataTable).NewIC3810101inServiceVisitRow
    '            ''販売店コード
    '            row.DLRCD = dealerCD
    '            ''店舗コード
    '            row.STRCD = storeCD
    '            ''来店日時
    '            row.VISITTIMESTAMP = sysDate
    '            ''車両登録No
    '            If rowCV.IsVCLREGNONull = False _
    '                AndAlso rowCV.VCLREGNO.Trim.Length > 0 Then
    '                row.VCLREGNO = rowCV.VCLREGNO
    '            Else
    '                row.VCLREGNO = vehicleRegNo
    '            End If
    '            ''顧客区分
    '            row.CUSTSEGMENT = rowCV.CUSTSEGMENT
    '            ''顧客コード
    '            If rowCV.IsCUSTOMERCODENull = False _
    '                AndAlso rowCV.CUSTOMERCODE.Trim.Length > 0 Then
    '                row.CUSTOMERCODE = rowCV.CUSTOMERCODE
    '            End If
    '            ''スタッフコード
    '            row.STAFFCD = rowCV.STAFFCD
    '            ''来店人数
    '            row.VISITPERSONNUM = 1
    '            ''来店手段
    '            row.VISITMEANS = "1"
    '            ''VIN
    '            row.VIN = rowCV.VIN
    '            ''シーケンス番号
    '            row.SEQNO = rowCV.SEQNO
    '            ''性別
    '            If rowCV.IsSEXNull = False _
    '                AndAlso rowCV.SEX.Trim.Length > 0 Then
    '                row.SEX = rowCV.SEX
    '            End If
    '            ''名前
    '            row.CUSTOMERNAME = rowCV.NAME
    '            ''SAコード
    '            row.DEFAULTSACODE = rowCV.SACODE
    '            ''アカウント
    '            row.ACCOUNT = account
    '            ''機能ID
    '            row.SYSTEM = system
    '            result = Me.InsertServiceVisit(row)
    '            If result = 0 Then
    '                ''------------------------------
    '                ''サービス来店者のSA振当て更新
    '                ''------------------------------
    '                da.UpdateServiceVisitSA(row.VISITSEQ, sysDate, saCode, account, system)
    '            End If
    '            ''来店実績連番
    '            visitSeq = row.VISITSEQ
    '        End Using
    '        Return result
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

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

    ' ''' <summary>
    ' ''' 担当SA変更
    ' ''' </summary>
    ' ''' <param name="dealerCD">販売店コード</param>
    ' ''' <param name="storeCD">店舗コード</param>
    ' ''' <param name="visitSeq">来店実績連番</param>
    ' ''' <param name="account">アカウント</param>
    ' ''' <param name="system">機能ID</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    ' ''' 
    ' ''' <history>
    ' ''' 2012/08/10 TMEJ 瀧 【SERVICE_2】入庫日付替え処理の追加
    ' ''' </history>
    '<EnableCommit()>
    'Public Function ChangeSACode(ByVal dealerCD As String _
    '                           , ByVal storeCD As String _
    '                           , ByVal visitSeq As Long _
    '                           , ByVal account As String _
    '                           , ByVal system As String) As Long

    '    Dim result As Long = -1
    '    ''引数をログに出力
    '    Dim args As New List(Of String)
    '    ''販売店コード
    '    args.Add(String.Format(CultureInfo.CurrentCulture, "dealerCD = {0}", dealerCD))
    '    ''店舗コード
    '    args.Add(String.Format(CultureInfo.CurrentCulture, "storeCD = {0}", storeCD))
    '    ''来店実績連番
    '    args.Add(String.Format(CultureInfo.CurrentCulture, "visitSeq = {0}", visitSeq))
    '    ''アカウント
    '    args.Add(String.Format(CultureInfo.CurrentCulture, "account = {0}", account))
    '    ''機能ID
    '    args.Add(String.Format(CultureInfo.CurrentCulture, "system = {0}", system))
    '    ''開始ログの出力
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '        , "{0}.{1} IN:{2}" _
    '        , Me.GetType.ToString _
    '        , MethodBase.GetCurrentMethod.Name _
    '        , String.Join(", ", args.ToArray())))
    '    Try
    '        ''システム日時の取得
    '        Dim sysDate As DateTime = DateTimeFunc.Now(dealerCD, storeCD)
    '        Using da As New IC3810101DataTableAdapter
    '            ''サービス来店者管理テーブルの取得
    '            Dim dtVisit As IC3810101VisitKeyDataTable = da.GetVisitData(dealerCD, storeCD, visitSeq)
    '            If dtVisit.Rows.Count = 0 Then
    '                result = ResultSuccess
    '                Return result
    '            End If
    '            Dim rowVisit As IC3810101VisitKeyRow = DirectCast(dtVisit.Rows(0), IC3810101VisitKeyRow)
    '            ''ログの出力
    '            ''引数をログに出力
    '            Dim argsData As New List(Of String)
    '            ' DataRow内の項目を列挙
    '            Me.AddLogData(argsData, rowVisit)
    '            ''開始ログの出力
    '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} DATA:{2}" _
    '                , Me.GetType.ToString _
    '                , "commonClass.ChangeSACode" _
    '                , String.Join(", ", argsData.ToArray())))
    '            Dim reserveID As Long = SMBCommonClassBusinessLogic.NoReserveId
    '            If rowVisit.DEFAULTSACODE.Equals(rowVisit.SACODE) = False Then
    '                reserveID = rowVisit.REZID
    '            End If
    '            Using commonClass As New SMBCommonClassBusinessLogic
    '                '2012/08/10 TMEJ 瀧 【SERVICE_2】入庫日付替え処理の追加 START
    '                '入庫日付替え処理
    '                If commonClass.ChangeCarInDate(dealerCD, _
    '                                               storeCD, _
    '                                               SMBCommonClassBusinessLogic.NoReserveId, _
    '                                               rowVisit.REZID, _
    '                                               sysDate, _
    '                                               account, _
    '                                               sysDate) <> 0 Then
    '                    ''失敗
    '                    Me.Rollback = True
    '                    result = RegisterError
    '                    Return result
    '                End If
    '                '2012/08/10 TMEJ 瀧 【SERVICE_2】入庫日付替え処理の追加 END
    '                'ストール予約とR/Oの担当SA変更
    '                If commonClass.ChangeSACode(dealerCD, _
    '                                            storeCD, _
    '                                            reserveID, _
    '                                            rowVisit.ORDERNO, _
    '                                            rowVisit.SACODE, _
    '                                            "0", _
    '                                            sysDate, _
    '                                            account, _
    '                                            sysDate) <> 0 Then
    '                    ''失敗
    '                    Me.Rollback = True
    '                    result = RegisterError
    '                    Return result
    '                End If
    '                '上記までの処理で、予約IDのストール予約を更新した場合
    '                'ストール予約履歴登録
    '                If reserveID >= 0 _
    '                    AndAlso commonClass.RegisterStallReserveHis(dealerCD, _
    '                                                                storeCD, _
    '                                                                reserveID, _
    '                                                                sysDate, _
    '                                                                RegisterType.ReserveHisAll) <> 0 Then
    '                    ''失敗
    '                    Me.Rollback = True
    '                    result = RegisterError
    '                    Return result
    '                End If
    '            End Using
    '        End Using
    '        ''成功
    '        result = ResultSuccess
    '        Return result
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
    '        ''ログの出力
    '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '            , "{0}.{1} OUT:{2}" _
    '            , Me.GetType.ToString _
    '            , "commonClass.ChangeSACode" _
    '            , result))
    '    End Try
    'End Function
    ' ''2012/08/01 KN 瀧 【A.STEP2】SA ストール予約受付機能開発 END

    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

    ''' <summary>
    ''' DataRow内の項目を列挙(ログ出力用)
    ''' </summary>
    ''' <param name="args">ログ項目のコレクション</param>
    ''' <param name="row">対象となるDataRow</param>
    ''' <remarks></remarks>
    Private Sub AddLogData(ByVal args As List(Of String), ByVal row As DataRow)
        For Each column As DataColumn In row.Table.Columns
            If row.IsNull(column.ColumnName) = True Then
                args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = NULL", column.ColumnName))
            Else
                args.Add(String.Format(CultureInfo.CurrentCulture, "{0} = {1}", column.ColumnName, row(column.ColumnName)))
            End If
        Next
    End Sub


#Region "通知処理"

#Region "通知用定数"

    ''' <summary>
    ''' 通知API用(カテゴリータイプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyPushCategory As String = "1"

    ''' <summary>
    ''' 通知API用(表示位置)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyPotisionType As String = "1"

    ''' <summary>
    ''' 通知API用(表示時間)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyTime As Integer = 3

    ''' <summary>
    ''' 通知API用(表示タイプ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyDispType As String = "1"

    ''' <summary>
    ''' 通知API用(色)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyColor As String = "1"

    ''' <summary>
    ''' 通知API用(呼び出し関数)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NotifyDispFunction As String = "icropScript.ui.setNotice()"

    ''' <summary>
    ''' 通知履歴のSessionValue(カンマ)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SessionValueKanma As String = ","

    ''' <summary>
    ''' 顧客詳細画面用セッション名("SessionKey.DMS_CST_ID")
    ''' </summary>
    Private Const SessionDMSID As String = "SessionKey.DMS_CST_ID,String,"

    ''' <summary>
    ''' 顧客詳細画面用セッション名("SessionKey.VIN")
    ''' </summary>
    Private Const SessionVIN As String = "SessionKey.VIN,String,"

    ' ''' <summary>
    ' ''' 未取引客のリンク文字列
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const NewCustomerLink As String = "<a id='SC30802250' Class='SC3080225' href='/Website/Pages/SC3080225.aspx' onclick='return ServiceLinkClick(event)'>"

    ''' <summary>
    ''' 自社客のリンク文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MyCustomerLink As String = "<a id='SC30802250' Class='SC3080225' href='/Website/Pages/SC3080225.aspx' onclick='return ServiceLinkClick(event)'>"

    ''' <summary>
    ''' Aタグ終了文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const EndLikTag As String = "</a>"

    ''' <summary>
    ''' リフレッシュ通知のPush情報(SVR：未振当て一覧へのPush  来店管理へのPush)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshSVRPushInfo As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=Send_Visit()"

    ''' <summary>
    ''' リフレッシュ通知のPush情報(SA：振当待ちエリアへのPush)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshSAAssignmentPushInfo As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=AssignmentRefresh()"

    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

    ' ''' <summary>
    ' ''' リフレッシュ通知のPush情報(SA：全体へのPush)
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const RefreshSAPushInfo As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=MainRefresh()"

    ' ''' <summary>
    ' ''' リフレッシュ通知のPush情報(CT/CHT権限(SMBへのPush))
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Private Const RefreshCTAndCHTPushInfo As String = "cat=action&type=main&sub=js&uid=#USER_ACCOUNT#&js1=RefreshSMB()"

    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

    ''' <summary>
    ''' リフレッシュ通知のAccount置換用文字列
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RefreshAccountReplaceWord As String = "#USER_ACCOUNT#"

    ''' <summary>
    ''' 作成するメッセージフラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Enum MessageType

        ''' <summary>
        ''' 自社客かつ車両登録No情報有
        ''' </summary>
        ''' <remarks></remarks>
        MyCustomer = 1

        ''' <summary>
        ''' 未取引客
        ''' </summary>
        ''' <remarks></remarks>
        NewCustomer = 2

    End Enum

#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' 通知処理
    ''' </summary>
    ''' <param name="inVisitSeq">来店実績連番</param>
    ''' <param name="inPresentTime">現在日時</param>
    ''' <param name="inDlrCD">ログイン販売店コード</param>
    ''' <param name="inStrCD">ログイン店舗コード</param>
    ''' <param name="inAccount">ログインアカウント</param>
    ''' <param name="inUserName">ログインユーザ名</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発
    ''' 2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される
    ''' </history>
    Public Sub NoticeProcessing(ByVal inVisitSeq As Long, _
                                ByVal inPresentTime As DateTime, _
                                ByVal inDlrCD As String, _
                                ByVal inStrCD As String, _
                                ByVal inAccount As String, _
                                ByVal inUserName As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START VISITSEQ:{2} PRESENTTIME:{3} DLRCD:{4} STRCD:{5} ACCOUNT:{6} USERNAME:{7} " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inVisitSeq, inPresentTime, inDlrCD, inStrCD, inAccount, inUserName))

        '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

        '来店IDのチェック
        If inVisitSeq < 0 Then
            '0より小さい場合
            '処理終了
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END VisitId is [-1] " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Exit Sub

        End If

        '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

        'IC3810101DataTableAdapterのインスタンス
        Using da As New IC3810101DataTableAdapter

            '通知送信用情報取得
            Dim dtNoticeProcessingInfo As IC3810101NoticeProcessingInfoDataTable = _
                da.GetNoticeProcessingInfo(inVisitSeq, _
                                           inDlrCD, _
                                           inStrCD)

            '通知送信用情報取得チェック
            If 0 < dtNoticeProcessingInfo.Count Then
                '取得できた場合

                'Rowに変換
                Dim rowNoticeProcessingInfo As IC3810101NoticeProcessingInfoRow = _
                    DirectCast(dtNoticeProcessingInfo.Rows(0), IC3810101NoticeProcessingInfoRow)

                '現在日時を設定
                rowNoticeProcessingInfo.PRESENTTIME = inPresentTime

                '来店通知処理の実行
                Me.NoticeMainProcessing(rowNoticeProcessingInfo, inDlrCD, inStrCD, inAccount, inUserName)

                '予約がある場合
                '来店管理にPush処理

                'OperationCodeリスト
                Dim operationCodeList As New List(Of Decimal)

                '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される START
                'OperationCodeリスト(WB用)
                Dim operationCodeListWB As New List(Of Decimal)
                '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される END

                'PresenceCategoryリスト
                Dim presenceCategoryList As New List(Of String)

                'OperationCodeリストに権限"52"：SVRを設定
                'OperationCodeリストに権限"9" ：SAを設定
                operationCodeList.Add(Operation.SVR)
                operationCodeList.Add(Operation.SA)

                Dim isReserveFlg As Boolean = False
                '予約の確認
                If 0 < rowNoticeProcessingInfo.REZID Then
                    'OperationCodeリストに権限"63"：WBを設定
                    '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される START
                    'operationCodeList.Add(63)
                    operationCodeListWB.Add(63)
                    '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される END

                    isReserveFlg = True
                End If

                'PresenceCategoryリストにカテゴリを追加
                presenceCategoryList.Add(PresenceCategory.Standby)
                presenceCategoryList.Add(PresenceCategory.Suspend)
                presenceCategoryList.Add(PresenceCategory.Offline)

                'ユーザーステータス取得
                Dim user As New Visit.Api.BizLogic.VisitUtilityBusinessLogic

                'ユーザーステータス取得処理
                '各権限の商談中以外のユーザー情報取得
                Dim userdt As VisitUtilityDataSet.VisitUtilityUsersDataTable = _
                    user.GetUsers(inDlrCD, inStrCD, operationCodeList, presenceCategoryList, DelFlg_0)

                '各権限の商談中以外のユーザー分ループ
                For Each userRow As VisitUtilityDataSet.VisitUtilityUsersRow In userdt

                    'Push処理
                    Me.SendPushServer(userRow.OPERATIONCODE, userRow.ACCOUNT, isReserveFlg, inAccount, inDlrCD)

                Next

                '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される START
                '予約がある場合
                If isReserveFlg Then

                    'WB権限の全ユーザー情報取得
                    Dim userdtWB As VisitUtilityDataSet.VisitUtilityUsersDataTable = _
                        user.GetUsers(inDlrCD, inStrCD, operationCodeListWB, Nothing, DelFlg_0)

                    'WB権限の全ユーザー分ループ
                    For Each userRowWB As VisitUtilityDataSet.VisitUtilityUsersRow In userdtWB

                        'Push処理
                        Me.SendPushServer(userRowWB.OPERATIONCODE, userRowWB.ACCOUNT, isReserveFlg, inAccount, inDlrCD)

                    Next

                End If
                '2017/03/22 NSK 秋田 TR-SVT-TMT-20151224-001 顧客一覧が遅れてwelcome boardに表示される END

            Else
                '取得失敗

                'エラーログ
                Logger.Error(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} GetNoticeProcessingInfo IS NOTHING" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name))

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START

    ''' <summary>
    ''' 来店取消通知処理
    ''' </summary>
    ''' <param name="inDlrCD">ログイン販売店コード</param>
    ''' <param name="inStrCD">ログイン店舗コード</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Sub VisitCalcelNoticeProcessing(ByVal inDlrCD As String, _
                                ByVal inStrCD As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START DLRCD:{2} STRCD:{3} " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDlrCD, inStrCD))

        'OperationCodeリスト
        Dim operationCodeList As New List(Of Decimal)

        'PresenceCategoryリスト
        Dim presenceCategoryList As New List(Of String)

        'OperationCodeリストに権限"52"：SVRを設定
        'OperationCodeリストに権限"9" ：SAを設定
        operationCodeList.Add(Operation.SVR)
        operationCodeList.Add(Operation.SA)

        'PresenceCategoryリストにカテゴリを追加
        presenceCategoryList.Add(PresenceCategory.Standby)
        presenceCategoryList.Add(PresenceCategory.Suspend)
        presenceCategoryList.Add(PresenceCategory.Offline)

        'ユーザーステータス取得
        Dim user As New Visit.Api.BizLogic.VisitUtilityBusinessLogic

        'ユーザーステータス取得処理
        '各権限の商談中以外のユーザー情報取得
        Dim userdt As VisitUtilityDataSet.VisitUtilityUsersDataTable = _
            user.GetUsers(inDlrCD, inStrCD, operationCodeList, presenceCategoryList, DelFlg_0)

        '各権限の商談中以外のユーザー分ループ
        For Each userRow As VisitUtilityDataSet.VisitUtilityUsersRow In userdt

            'リフレッシュ文字列
            Dim pushWord As String = String.Empty

            '権限毎処理の分岐
            Select Case userRow.OPERATIONCODE

                Case Operation.SVR
                    'SVR権限
                    'リフレッシュの文字列作成
                    pushWord = RefreshSVRPushInfo.Replace(RefreshAccountReplaceWord, userRow.ACCOUNT)

                Case Operation.SA
                    'SA権限
                    'リフレッシュの文字列作成
                    pushWord = RefreshSAAssignmentPushInfo.Replace(RefreshAccountReplaceWord, userRow.ACCOUNT)

            End Select

            'Push
            Dim visitUtility As New Visit.Api.BizLogic.VisitUtility

            'リフレッシュ文字列チェック
            If Not String.IsNullOrEmpty(pushWord) Then
                '文字列が存在してる場合

                'Push処理実行
                visitUtility.SendPush(pushWord)

            End If

            '開放処理
            visitUtility = Nothing
        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

    ''' <summary>
    ''' 各権限に対するPush処理
    ''' </summary>
    ''' <param name="inOperationCode">権限コード</param>
    ''' <param name="inAccount">リフレッシュ先アカウント</param>
    ''' <param name="inReserveFlg">予約客フラグ</param>
    ''' <remarks></remarks>
    Public Sub SendPushServer(ByVal inOperationCode As Long, _
                              ByVal inAccount As String, _
                              ByVal inReserveFlg As Boolean, _
                              ByVal inAccountFrom As String, _
                              ByVal inDlrcd As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'リフレッシュ文字列
        Dim pushWord As String = String.Empty

        '権限毎処理の分岐
        Select Case inOperationCode

            Case Operation.SVR
                'SVR権限
                If Not inReserveFlg Then
                    '未振当て一覧
                    'リフレッシュの文字列作成
                    pushWord = RefreshSVRPushInfo.Replace(RefreshAccountReplaceWord, inAccount)

                Else
                    '来店管理
                    'リフレッシュの文字列作成
                    pushWord = RefreshSVRPushInfo.Replace(RefreshAccountReplaceWord, inAccount)

                End If

            Case Operation.SA
                'SA権限

                'SAメインメニュー
                'リフレッシュの文字列作成
                pushWord = RefreshSAAssignmentPushInfo.Replace(RefreshAccountReplaceWord, inAccount)

        End Select

        'Push
        Dim visitUtility As New Visit.Api.BizLogic.VisitUtility

        'リフレッシュ文字列チェック
        If Not String.IsNullOrEmpty(pushWord) Then
            '文字列が存在してる場合

            'Push処理実行
            visitUtility.SendPush(pushWord)

        End If

        If inOperationCode = 63 Then
            'ウェルカムボード権限もつアカントにPush
            visitUtility.SendPushReconstructionPC(inAccountFrom, inAccount, String.Empty, inDlrcd)
        End If

        '開放処理
        visitUtility = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

#End Region

#Region "Privateメソッド"

    ''' <summary>
    ''' 通知メイン処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inDlrCD">ログイン販売店コード</param>
    ''' <param name="inStrCD">ログイン店舗コード</param>
    ''' <param name="inAccount">ログインアカウント</param>
    ''' <param name="inUserName">ログインユーザ名</param>
    ''' <remarks></remarks>
    Private Sub NoticeMainProcessing(ByVal inRowNoticeProcessingInfo As IC3810101NoticeProcessingInfoRow, _
                                     ByVal inDlrCD As String, _
                                     ByVal inStrCD As String, _
                                     ByVal inAccount As String, _
                                     ByVal inUserName As String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START" _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))


        '送信先アカウント情報設定
        Dim account As List(Of XmlAccount) = Me.CreateAccount(inDlrCD, inStrCD)

        '通知履歴登録情報の設定
        Dim requestNotice As XmlRequestNotice = Me.CreateRequestNotice(inRowNoticeProcessingInfo, inDlrCD, inStrCD, inAccount, inUserName)

        'Push情報作成処理の設定
        Dim pushInfo As XmlPushInfo = Me.CreatePushInfo(inRowNoticeProcessingInfo)

        '設定したものを格納し、通知APIをコール
        Using noticeData As New XmlNoticeData

            '現在時間データの格納
            noticeData.TransmissionDate = inRowNoticeProcessingInfo.PRESENTTIME
            '送信ユーザーデータ格納
            noticeData.AccountList.AddRange(account.ToArray)
            '通知履歴用のデータ格納
            noticeData.RequestNotice = requestNotice
            'Pushデータ格納
            noticeData.PushInfo = pushInfo

            '通知処理実行
            Using ic3040801Biz As New IC3040801BusinessLogic

                '通知処理実行
                ic3040801Biz.NoticeDisplay(noticeData, NoticeDisposal.GeneralPurpose)

            End Using

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 送信先アカウント情報作成処理
    ''' </summary>
    ''' <param name="inDlrCD">ログイン販売店コード</param>
    ''' <param name="inStrCD">ログイン店舗コード</param>
    ''' <returns>送信先アカウント情報リスト</returns>
    ''' <remarks></remarks>
    Private Function CreateAccount(ByVal inDlrCD As String, _
                                   ByVal inStrCD As String) As List(Of XmlAccount)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                  , "{0}.{1} START " _
                  , Me.GetType.ToString _
                  , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '送信先アカウント情報リスト
        Dim accountList As New List(Of XmlAccount)

        'OperationCodeリスト
        Dim operationCodeList As New List(Of Decimal)

        'OperationCodeリスト
        Dim presenceCategoryList As New List(Of String)

        'OperationCodeリストに権限"52"：SVRを設定
        'OperationCodeリストに権限"9" ：SAを設定
        operationCodeList.Add(Operation.SVR)
        operationCodeList.Add(Operation.SA)

        'PresenceCategoryリストにカテゴリを追加
        presenceCategoryList.Add(PresenceCategory.Standby)
        presenceCategoryList.Add(PresenceCategory.Suspend)
        presenceCategoryList.Add(PresenceCategory.Offline)

        'ユーザーステータス取得
        Dim user As New Visit.Api.BizLogic.VisitUtilityBusinessLogic

        'ユーザーステータス取得処理
        '各権限の全ユーザー情報取得
        Dim userdt As VisitUtilityDataSet.VisitUtilityUsersDataTable = _
            user.GetUsers(inDlrCD, inStrCD, operationCodeList, presenceCategoryList, DelFlg_0)

        'オンラインユーザー分ループ
        For Each userRow As VisitUtilityDataSet.VisitUtilityUsersRow In userdt

            '送信先アカウント情報 
            Using account As New XmlAccount

                '受信先のアカウント設定
                account.ToAccount = userRow.ACCOUNT

                '受信者名設定
                account.ToAccountName = userRow.USERNAME

                '送信先アカウント情報リストに送信先アカウント情報を追加
                accountList.Add(account)

            End Using

        Next

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return accountList


    End Function

    ''' <summary>
    ''' 通知履歴登録情報作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inDlrCD">ログイン販売店コード</param>
    ''' <param name="inStrCD">ログイン店舗コード</param>
    ''' <param name="inAccount">ログインアカウント</param>
    ''' <param name="inUserName">ログインユーザ名</param>
    ''' <returns>通知履歴登録情報</returns>
    ''' <remarks></remarks>
    Private Function CreateRequestNotice(ByVal inRowNoticeProcessingInfo As IC3810101NoticeProcessingInfoRow, _
                                         ByVal inDlrCD As String, _
                                         ByVal inStrCD As String, _
                                         ByVal inAccount As String, _
                                         ByVal inUserName As String) As XmlRequestNotice

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'XmlRequestNoticeのインスタンス
        Using requestNotice As New XmlRequestNotice

            '販売店コード設定
            requestNotice.DealerCode = inDlrCD

            '店舗コード設定
            requestNotice.StoreCode = inStrCD

            'スタッフコード(送信元)設定
            requestNotice.FromAccount = inAccount

            'スタッフ名(送信元)設定
            requestNotice.FromAccountName = inUserName

            '顧客種別(リンク制御で使用)
            Dim customerType As Integer = MessageType.NewCustomer

            '通知履歴にリンクをつけるか判定
            '顧客種別"1"：自社客　かつ　DMSISが存在する場合
            '通知履歴にリンクをつける

            '自社客チェック
            If CustSegmentMyCustomer.Equals(inRowNoticeProcessingInfo.CUSTSEGMENT) _
                AndAlso Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.DMSID.Trim) Then
                '自社客の場合

                '自社客設定
                customerType = MessageType.MyCustomer

            End If

            '通知履歴用メッセージ作成設定
            requestNotice.Message = Me.CreateNoticeRequestMessage(inRowNoticeProcessingInfo, customerType)

            'セッション設定値設定
            requestNotice.SessionValue = Me.CreateNoticeRequestSession(inRowNoticeProcessingInfo, customerType)

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return requestNotice

        End Using

    End Function

    ''' <summary>
    ''' 通知履歴用メッセージ作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inCustomerType">顧客種別(リンク制御用"1"：自社客)</param>
    ''' <returns>通知履歴用メッセージ情報</returns>
    ''' <history>
    ''' </history>
    ''' <remarks></remarks>
    Private Function CreateNoticeRequestMessage(ByVal inRowNoticeProcessingInfo As IC3810101NoticeProcessingInfoRow, _
                                                ByVal inCustomerType As Integer) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メッセージ
        Dim workMessage As New StringBuilder

        'メッセージ組立処理

        '文言：来店 設定
        workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordID.Visit))

        'メッセージ間にスペースの設定
        workMessage.Append(Space(3))

        '自社客チェック
        If inCustomerType = MessageType.MyCustomer Then
            '顧客種別"1"：自社客　かつ　DMSISが存在する場合
            '通知履歴にリンクをつける

            '自社客のAタグを設定
            workMessage.Append(MyCustomerLink)

        End If


        'メッセージ組立：車両登録番号
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.VCLREGNO) Then
            '車両登録番号がある場合

            '車両登録番号を設定
            workMessage.Append(inRowNoticeProcessingInfo.VCLREGNO)

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If

        'メッセージ組立：お客様名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.NAME) Then
            'お客様名がある場合

            '敬称利用区分チェック
            If PositionTypeBack.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を後方につけつ

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAME)

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

            ElseIf PositionTypeFront.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を前方につける

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAME)

            Else
                '上記以外の場合

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAME)

            End If

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        Else
            'お客様名がない場合

            '文言：お客様 設定
            workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordID.Customer))

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If


        '自社客チェック
        If inCustomerType = MessageType.MyCustomer Then
            '顧客種別"1"：自社客　かつ　DMSISが存在する場合
            '通知履歴にリンクをつける

            'Aタグ終了を設定
            workMessage.Append(EndLikTag)

        End If

        '予約客の場合
        If 0 < inRowNoticeProcessingInfo.REZID Then

            'メッセージ組立：予約情報：作業開始日時・作業終了日時
            If Not inRowNoticeProcessingInfo.IsSCHE_START_DATETIMENull _
                AndAlso Not inRowNoticeProcessingInfo.IsSCHE_END_DATETIMENull Then
                '作業開始日時・作業終了日時がある場合

                '作業開始日時を設定
                workMessage.Append(inRowNoticeProcessingInfo.SCHE_START_DATETIME.ToString("HH:mm", CultureInfo.CurrentCulture))

                '文言：～ 設定
                workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordID.Mark))

                '作業終了日時を設定
                workMessage.Append(inRowNoticeProcessingInfo.SCHE_END_DATETIME.ToString("HH:mm", CultureInfo.CurrentCulture))

                'メッセージ間にスペースの設定
                workMessage.Append(Space(3))

            End If

            'メッセージ組立：商品名
            If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.MERCHANDISENAME) Then
                '商品名がある場合

                '商品名を設定
                workMessage.Append(inRowNoticeProcessingInfo.MERCHANDISENAME)

            End If

        End If

        '戻り値設定
        Dim notifyMessage As String = workMessage.ToString().TrimEnd

        '開放処理
        workMessage = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END MESSAGE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , notifyMessage))

        Return notifyMessage

    End Function

    ''' <summary>
    ''' 通知履歴用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <param name="inCustomerType">顧客種別(リンク制御用"1"：自社客)</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateNoticeRequestSession(ByVal inRowNoticeProcessingInfo As IC3810101NoticeProcessingInfoRow, _
                                                ByVal inCustomerType As Integer) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim notifySession As String = String.Empty

        'メッセージ種別判定
        Select Case inCustomerType
            Case MessageType.NewCustomer
                '「0:未取引客」の場合

                '未取引客のセッション情報を作成
                notifySession = CreateNewCustomerSession()

            Case MessageType.MyCustomer
                '「1:自社客かつ車両登録No有」の場合

                '自社客かつDMSISがあるときの通知用セッション情報作成処理
                notifySession = CreateCustomerSession(inRowNoticeProcessingInfo)

        End Select

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return notifySession

    End Function

    ''' <summary>
    ''' 未取引客の通知用セッション情報作成メソッド
    ''' </summary>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateNewCustomerSession() As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return Nothing

    End Function

    ''' <summary>
    ''' 自社客かつDMSISがあるときの通知用セッション情報作成メソッド
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <returns>戻り値</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Private Function CreateCustomerSession(inRowNoticeProcessingInfo As IC3810101NoticeProcessingInfoRow) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Dim workSession As New StringBuilder

        'DMSIDのセッション値作成
        Me.SetSessionValueWord(workSession, SessionDMSID, inRowNoticeProcessingInfo.DMSID)

        'VINの設定
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.VIN.Trim) Then
            'VINがある場合は設定

            'VINのセッション値作成
            Me.SetSessionValueWord(workSession, SessionVIN, inRowNoticeProcessingInfo.VIN)

        End If


        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession.ToString

    End Function

    ''' <summary>
    ''' SessionValue文字列作成
    ''' </summary>
    ''' <param name="workSession">追加元文字列</param>
    ''' <param name="SessionValueWord">追加するSESSIONKEY</param>
    ''' <param name="SessionValueData">追加するデータ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetSessionValueWord(ByVal workSession As StringBuilder, _
                                         ByVal SessionValueWord As String, _
                                         ByVal SessionValueData As String) As StringBuilder

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'カンマの設定
        If workSession.Length <> 0 Then
            'データがある場合

            '「,」を結合する
            workSession.Append(SessionValueKanma)

        End If

        'セッションキーを設定
        workSession.Append(SessionValueWord)

        'セッション値を設定
        workSession.Append(SessionValueData)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Return workSession

    End Function


    ''' <summary>
    ''' Push情報作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <returns>Push情報</returns>
    ''' <remarks></remarks>
    Private Function CreatePushInfo(ByVal inRowNoticeProcessingInfo As IC3810101NoticeProcessingInfoRow) As XmlPushInfo

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'PUSH内容設定
        Using pushInfo As New XmlPushInfo

            'カテゴリータイプ設定
            pushInfo.PushCategory = NotifyPushCategory

            '表示位置設定
            pushInfo.PositionType = NotifyPotisionType

            '表示時間設定
            pushInfo.Time = NotifyTime

            '表示タイプ設定
            pushInfo.DisplayType = NotifyDispType

            'Push用メッセージ作成
            pushInfo.DisplayContents = Me.CreatePusuMessage(inRowNoticeProcessingInfo)

            '色設定
            pushInfo.Color = NotifyColor

            '表示時関数設定
            pushInfo.DisplayFunction = NotifyDispFunction

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return pushInfo

        End Using
    End Function

    ''' <summary>
    ''' Push用メッセージ作成処理
    ''' </summary>
    ''' <param name="inRowNoticeProcessingInfo">通知送信用情報取得</param>
    ''' <returns>Puss用メッセージ文言</returns>
    ''' <history>
    ''' </history>
    ''' <remarks></remarks>
    Private Function CreatePusuMessage(ByVal inRowNoticeProcessingInfo As IC3810101NoticeProcessingInfoRow) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'メッセージ
        Dim workMessage As New StringBuilder

        'メッセージ組立処理

        '文言：ご来店 設定
        workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordID.Visit))

        'メッセージ間にスペースの設定
        workMessage.Append(Space(3))

        'メッセージ組立：車両登録番号
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.VCLREGNO) Then
            '車両登録番号がある場合

            '車両登録番号を設定
            workMessage.Append(inRowNoticeProcessingInfo.VCLREGNO)

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If

        'メッセージ組立：お客様名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.NAME) Then
            'お客様名がある場合

            '敬称利用区分チェック
            If PositionTypeBack.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を後方につけつ

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAME)

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

            ElseIf PositionTypeFront.Equals(inRowNoticeProcessingInfo.POSITION_TYPE) Then
                '敬称を前方につける

                '敬称を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAMETITLE_NAME)

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAME)

            Else
                '上記以外の場合

                '顧客名を設定
                workMessage.Append(inRowNoticeProcessingInfo.NAME)

            End If

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        Else
            'お客様名がない場合

            '文言：お客様 設定
            workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordID.Customer))

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If

        'メッセージ組立：予約情報：作業開始日時・作業終了日時
        If Not inRowNoticeProcessingInfo.IsSCHE_START_DATETIMENull _
            AndAlso Not inRowNoticeProcessingInfo.IsSCHE_END_DATETIMENull Then
            '作業開始日時・作業終了日時がある場合

            '作業開始日時を設定
            workMessage.Append(inRowNoticeProcessingInfo.SCHE_START_DATETIME.ToString("HH:mm", CultureInfo.CurrentCulture))

            '文言：～ 設定
            workMessage.Append(WebWordUtility.GetWord(ApplicationID, WordID.Mark))

            '作業終了日時を設定
            workMessage.Append(inRowNoticeProcessingInfo.SCHE_END_DATETIME.ToString("HH:mm", CultureInfo.CurrentCulture))

            'メッセージ間にスペースの設定
            workMessage.Append(Space(3))

        End If

        'メッセージ組立：商品名
        If Not String.IsNullOrEmpty(inRowNoticeProcessingInfo.MERCHANDISENAME) Then
            '商品名がある場合

            '商品名を設定
            workMessage.Append(inRowNoticeProcessingInfo.MERCHANDISENAME)

        End If


        '戻り値設定
        Dim notifyMessage As String = workMessage.ToString().TrimEnd


        '開放処理
        workMessage = Nothing

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END MESSAGE = {2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , notifyMessage))

        Return notifyMessage

    End Function

#End Region

#End Region

#Region "IDisposable Support"
    Private disposedValue As Boolean ' 重複する呼び出しを検出するには

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
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
