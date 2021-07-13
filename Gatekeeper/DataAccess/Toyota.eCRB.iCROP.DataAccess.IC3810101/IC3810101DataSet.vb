'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3810101DataSet.vb
'─────────────────────────────────────
'機能： 来店連携データアクセス
'補足： 
'作成： 2012/01/26 KN 瀧
'更新： 2012/02/28 KN 瀧 【SERVICE_1】管理予約IDがNULLの時データが取得できない不具合を修正
'更新： 2012/04/06 KN 瀧 【SERVICE_1】サービスコードの集約方法を変更
'更新： 2012/04/10 KN 瀧 【SERVICE_1】ストール予約情報の取得条件の変更
'更新： 2012/04/11 KN 佐藤 【SERVICE_1】対応済みチップと紐付く不具合を修正
'更新： 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する
'更新： 2012/04/12 KN 佐藤 【SERVICE_1】予約の紐付け時、顧客名が変更されている可能性があるため、条件から外す
'更新： 2012/04/13 KN 瀧 【SERVICE_1】サービスコードの集約方法を戻す
'更新： 2012/04/17 KN 瀧 【SERVICE_1】サービスコードの集約方法を変更(三回目)
'更新： 2012/05/15 KN 河原 【SERVICE_2事前準備対応】整備受注NO・受付担当予定者を新たに取得
'更新： 2012/05/15 KN 河原 【SERVICE_2事前準備対応】整備受注NOをORDERNOに登録とDEFAULTSACODEの登録方法変更
'更新： 2012/07/05 KN 河原 【SERVICE_2事前準備対応】顧客コード・識別Flagを新たに取得
'更新： 2012/07/05 KN 河原 【SERVICE_2事前準備対応】サービス来店管理テーブルの顧客区分・顧客コード・期間顧客IDを登録
'更新： 2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発
'更新： 2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応
'更新： 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
'更新： 2014/01/23 TMEJ 陳　 TMEJ次世代サービス 工程管理機能開発
'更新： 2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発
'更新： 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発
'更新； 2015/12/17 TM 浅野 12月号口配信に向けた緊急対応 通知処理時の顧客名称の取得元をサービス来店実績から顧客へ変更する
'更新： 2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加
'更新：
'─────────────────────────────────────

Imports System.Text
Imports System.Reflection
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Partial Class IC3810101DataSet
End Class

Namespace IC3810101DataSetTableAdapters
    Public Class IC3810101DataTableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        '2012/07/05 KN 河原 【SERVICE_2事前準備対応】サービス来店管理テーブルの顧客区分・顧客コード・期間顧客IDを登録　START
        ''' <summary>
        ''' ストール予約の自社客情報
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusVisit As String = "0"
        ''' <summary>
        ''' サービス来店管理の自社客
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ManageStatusVisit As String = "1"
        '2012/07/05 KN 河原 【SERVICE_2事前準備対応】サービス来店管理テーブルの顧客区分・顧客コード・期間顧客IDを登録　END

        '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 START
        ''' <summary>
        ''' ストール予約の未取引情報
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusNotVisit As String = "1"
        ''' <summary>
        ''' サービス来店管理の未取引客
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ManageStatusNotVisit As String = "2"
        '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 END

        ''2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発 START
        ''' <summary>
        ''' 販売店環境マスタ.パラメータ名:変換フォーマット
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VclRegNoChangeFormat As String = "VCLREGNO_CHANGE_FORMAT"

        ''' <summary>
        ''' 販売店環境マスタ.パラメータ名:変換当て込み文字
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VclRegNoChangeString As String = "VCLREGNO_CHANGE_STRING"

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ''' <summary>
        ''' 顧客車両区分(所有者)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VehicleType As String = "1"

        ''' <summary>
        ''' サービスステータス(未入庫)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusNoIn As String = "00"

        ''' <summary>
        ''' サービスステータス(未来店)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusNoVisit As String = "01"

        '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

        ''' <summary>
        ''' サービスステータス(納車済み)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusDelivery As String = "13"

        ''' <summary>
        ''' ROステータス（納車済み）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ROStatusDelivery As String = "90"

        ''' <summary>
        ''' ROステータス（R/Oキャンセル）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ROStatusCancel As String = "99"

        '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

        ''' <summary>
        ''' 予約ステータス(仮予約)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RezStatusTentative As String = "0"

        ''' <summary>
        ''' 予約ステータス(本予約)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RezStatus As String = "1"

        ''' <summary>
        ''' 受付区分(予約客)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AcceptanceTypeRez As String = "0"

        ''' <summary>
        ''' キャンセルフラグ(有効)
        ''' </summary>
        Private Const CancelFlagEffective As String = "0"

        ''' <summary>
        ''' 性別「0：男性」
        ''' </summary>
        ''' <remarks></remarks>
        Private Const Male As String = "0"

        ''' <summary>
        ''' サービス来店自社客
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CustSegmentMyCustomer As String = "1"

        ''' <summary>
        ''' サービス来店未取引客
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CustSegmentNewCustomer As String = "2"

        ''' <summary>
        ''' 振当てステータス（未振当て）
        ''' </summary>
        Private Const NonAssign As String = "0"

        '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

        ''' <summary>
        ''' 振当てステータス（退店）
        ''' </summary>
        Private Const DealerOut As String = "4"

        '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

        ''' <summary>
        ''' 登録区分（GK）
        ''' </summary>
        Private Const RegistGK As String = "0"

        '2014/01/23 TMEJ 陳　 TMEJ次世代サービス 工程管理機能開発 START

        ''' <summary>
        ''' DB日付省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MinDate As String = "1900/01/01 00:00:00"

        ''' <summary>
        ''' サービスステータス(キャンセル)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusCancel_02 As String = "02"

        ''' <summary>
        ''' 使用中フラグ(使用中)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const InUse_1 As String = "1"

        '2014/01/23 TMEJ 陳　 TMEJ次世代サービス 工程管理機能開発 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        
        ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
        
        ''' <summary>
        ''' 顧客車両区分(保険)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VehicleTypeInsurance As String = "4"
        
        ''' <summary>
        ''' オーナーチェンジフラグ(未設定)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OwnerChangeFlagNot As String = "0"

        ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
#End Region

        ''' <summary>
        ''' StallRangeDays
        ''' </summary>
        ''' <param name="dealerCD"></param>
        ''' <param name="storeCD"></param>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private ReadOnly Property StallRangeDays(ByVal dealerCD As String, _
                                                        ByVal storeCD As String) As Long
            Get
                Dim value As Long

                Dim row As DlrEnvSettingDataSet.DLRENVSETTINGRow _
                        = (New BranchEnvSetting).GetEnvSetting(dealerCD, storeCD, "STALL_RANGE_DAYS")

                If row IsNot Nothing _
                    AndAlso Long.TryParse(row.PARAMVALUE, value) = True Then

                    Return value
                Else

                    Return 0
                End If
            End Get
        End Property

        ''2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発 END


        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ' ''2012/01/06 追加 サービス来店者情報に基幹顧客ID,モデルコード,電話番号,携帯番号項目の追加
        ' ''' <summary>
        ' ''' 来客者キー情報の取得
        ' ''' </summary>
        ' ''' <param name="rowIN">サービス来店者引数</param>
        ' ''' <returns>来客者キー情報</returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' ''' </history>
        'Public Function GetCustomerKey(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow) As IC3810101DataSet.IC3810101CustomerKeyDataTable
        '    Dim dt As IC3810101DataSet.IC3810101CustomerKeyDataTable
        '    If rowIN.IsCUSTSEGMENTNull = True Then
        '        dt = New IC3810101DataSet.IC3810101CustomerKeyDataTable
        '        dt.Rows.Add(dt.NewIC3810101CustomerKeyRow)
        '    Else
        '        Select Case rowIN.CUSTSEGMENT
        '            Case "1"    '自社客
        '                ''自社客情報の取得
        '                dt = GetMyCustomerKey(rowIN)
        '            Case "2"    '未取引客
        '                ''未取引客情報の取
        '                dt = GetNewCustomerKey(rowIN)
        '            Case Else
        '                dt = New IC3810101DataSet.IC3810101CustomerKeyDataTable
        '                dt.Rows.Add(dt.NewIC3810101CustomerKeyRow)
        '        End Select
        '    End If
        '    Return dt
        'End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ' ''2012/01/06 追加 サービス来店者情報に基幹顧客ID,モデルコード,電話番号,携帯番号項目の追加
        ' ''' <summary>
        ' ''' 自社客キー情報の取得
        ' ''' </summary>
        ' ''' <param name="rowIN">サービス来店者引数</param>
        ' ''' <returns>自社客キー情報</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' 2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応
        ' ''' </history>
        'Private Function GetMyCustomerKey(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow) As IC3810101DataSet.IC3810101CustomerKeyDataTable
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

        '        Using dtCustomer As New IC3810101DataSet.IC3810101CustomerKeyDataTable
        '            Dim rowCustomer As IC3810101DataSet.IC3810101CustomerKeyRow = dtCustomer.NewIC3810101CustomerKeyRow
        '            ''来客者情報の取得
        '            If rowIN.IsCUSTOMERCODENull = False Then
        '                Using query As New DBSelectQuery(Of DataTable)("IC3810101_001")
        '                    ''SQLの設定
        '                    Dim sqlCustomer As New StringBuilder
        '                    sqlCustomer.AppendLine("SELECT /* IC3810101_001 */")
        '                    sqlCustomer.AppendLine("       CUSTCD AS DMSID")
        '                    ' 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する START
        '                    sqlCustomer.AppendLine("     , RTRIM(CUSTCD) AS CUSTOMERCODE")
        '                    ' 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する END
        '                    sqlCustomer.AppendLine("     , TELNO")
        '                    sqlCustomer.AppendLine("     , MOBILE")
        '                    '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 START
        '                    sqlCustomer.AppendLine("     , NAME")
        '                    '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 END
        '                    sqlCustomer.AppendLine("  FROM TBLORG_CUSTOMER")
        '                    sqlCustomer.AppendLine(" WHERE ORIGINALID = :ORIGINALID")
        '                    ''パラメータの設定
        '                    query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, rowIN.CUSTOMERCODE)
        '                    ''SQLの実行
        '                    query.CommandText = sqlCustomer.ToString()
        '                    Using dt As DataTable = query.GetData()
        '                        If dt.Rows.Count > 0 Then
        '                            For Each column As DataColumn In dt.Columns
        '                                rowCustomer(column.ColumnName) = dt.Rows(0)(column.ColumnName)
        '                            Next
        '                        End If
        '                    End Using
        '                End Using
        '            End If
        '            ''車両情報の取得
        '            If rowIN.IsCUSTOMERCODENull = False _
        '                AndAlso rowIN.IsVINNull = False Then
        '                Using query As New DBSelectQuery(Of DataTable)("IC3810101_002")
        '                    ''SQLの設定
        '                    Dim sqlVehicle As New StringBuilder
        '                    sqlVehicle.AppendLine("SELECT /* IC3810101_002 */")
        '                    sqlVehicle.AppendLine("       MODELCD AS MODELCODE")
        '                    '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 START
        '                    sqlVehicle.AppendLine("     , VIN")
        '                    '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 END
        '                    sqlVehicle.AppendLine("  FROM TBLORG_VCLINFO")
        '                    sqlVehicle.AppendLine(" WHERE ORIGINALID = :ORIGINALID")
        '                    sqlVehicle.AppendLine("   AND VIN = :VIN")
        '                    ''パラメータの設定
        '                    query.AddParameterWithTypeValue("ORIGINALID", OracleDbType.Char, rowIN.CUSTOMERCODE)
        '                    query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowIN.VIN)
        '                    ''SQLの実行
        '                    query.CommandText = sqlVehicle.ToString()
        '                    Using dt As DataTable = query.GetData()
        '                        If dt.Rows.Count > 0 Then
        '                            For Each column As DataColumn In dt.Columns
        '                                rowCustomer(column.ColumnName) = dt.Rows(0)(column.ColumnName)
        '                            Next
        '                        End If
        '                    End Using
        '                End Using
        '            End If
        '            dtCustomer.Rows.Add(rowCustomer)
        '            ''終了ログの出力
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                   , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                   , Me.GetType.ToString _
        '                   , MethodBase.GetCurrentMethod.Name _
        '                   , dtCustomer.Rows.Count))
        '            Return dtCustomer
        '        End Using
        '    Finally

        '    End Try
        'End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ' ''2012/01/06 追加 サービス来店者情報に基幹顧客ID,モデルコード,電話番号,携帯番号項目の追加
        ' ''' <summary>
        ' ''' 未取引客キー情報の取得
        ' ''' </summary>
        ' ''' <param name="rowIN">サービス来店者引数</param>
        ' ''' <returns>未取引客キー情報</returns>
        ' ''' <remarks></remarks>    
        ' ''' <history>
        ' ''' 2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応
        ' ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ' ''' </history>
        'Private Function GetNewCustomerKey(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow) _
        '                                   As IC3810101DataSet.IC3810101CustomerKeyDataTable


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

        '    Using dtCustomer As New IC3810101DataSet.IC3810101CustomerKeyDataTable

        '        Dim rowCustomer As IC3810101DataSet.IC3810101CustomerKeyRow = dtCustomer.NewIC3810101CustomerKeyRow

        '        '来客者情報の取得
        '        If rowIN.IsCUSTOMERCODENull = False Then

        '            Using query As New DBSelectQuery(Of DataTable)("IC3810101_003")

        '                ''SQLの設定
        '                Dim sql As New StringBuilder

        '                sql.AppendLine("SELECT /* IC3810101_003 */")
        '                sql.AppendLine("       TELNO")
        '                sql.AppendLine("     , MOBILE")
        '                '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 START
        '                sql.AppendLine("     , NAME")
        '                '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 END
        '                sql.AppendLine("  FROM TBL_NEWCUSTOMER")
        '                sql.AppendLine(" WHERE CSTID = :CSTID")
        '                ''パラメータの設定
        '                query.AddParameterWithTypeValue("CSTID", OracleDbType.Char, rowIN.CUSTOMERCODE)

        '                ''SQLの実行
        '                query.CommandText = sql.ToString()

        '                Using dt As DataTable = query.GetData()

        '                    If dt.Rows.Count > 0 Then

        '                        Dim row As DataRow = dt.Rows(0)

        '                        For Each column As DataColumn In dt.Columns
        '                            rowCustomer(column.ColumnName) = row(column.ColumnName)
        '                        Next

        '                    End If
        '                End Using
        '            End Using
        '        End If

        '        dtCustomer.Rows.Add(rowCustomer)

        '        ''終了ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '               , Me.GetType.ToString _
        '               , MethodBase.GetCurrentMethod.Name _
        '               , dtCustomer.Rows.Count))

        '        Return dtCustomer

        '    End Using

        'End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        '' 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する START
        '' 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する START
        ' ''' <summary>
        ' ''' ストール予約キー情報の取得
        ' ''' </summary>
        ' ''' <param name="rowIN">サービス来店者引数</param>
        ' ''' <returns>ストール予約キー情報</returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' ''' 2012/02/28 KN 瀧 【SERVICE_1】管理予約IDがNULLの時データが取得できない不具合を修正
        ' ''' 2012/04/06 KN 瀧 【SERVICE_1】サービスコードの集約方法を変更
        ' ''' 2012/04/10 KN 瀧 【SERVICE_1】ストール予約情報の取得条件の変更
        ' ''' 2012/04/11 KN 佐藤 【SERVICE_1】対応済みチップと紐付く不具合を修正
        ' ''' 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する
        ' ''' 2012/04/12 KN 佐藤 【SERVICE_1】予約の紐付け時、顧客名が変更されている可能性があるため、条件から外す
        ' ''' 2012/04/17 KN 瀧 【SERVICE_1】サービスコードの集約方法を変更(三回目)
        ' ''' 2012/05/15 KN 河原 【SERVICE_2事前準備対応】 整備受注NO・受付担当予定者を新たに取得
        ' ''' 2012/07/05 KN 河原 【SERVICE_2事前準備対応】 顧客コード・識別Flagを新たに取得
        ' ''' 2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発
        ' ''' 2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応
        ' ''' </history>
        'Public Function GetStallKey(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow,
        '                            ByVal rowCK As IC3810101DataSet.IC3810101CustomerKeyRow) As IC3810101DataSet.IC3810101StallKeyDataTable
        '    'Public Function GetStallKey(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow) As IC3810101DataSet.IC3810101StallKeyDataTable
        '    ' 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する END
        '    Try
        '        ''引数をログに出力
        '        Dim args As New List(Of String)
        '        ' DataRow内の項目を列挙
        '        Me.AddLogData(args, rowIN)
        '        Me.AddLogData(args, rowCK)
        '        ''開始ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} IN:{2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , String.Join(", ", args.ToArray())))

        '        ''SQLの設定
        '        Dim sql As New StringBuilder
        '        '2012/04/17 KN 瀧 【SERVICE_1】サービスコードの集約方法を変更(三回目) START
        '        '2012/04/13 KN 瀧 【SERVICE_1】サービスコードの集約方法を戻す START
        '        '2012/04/06 KN 瀧 【SERVICE_1】サービスコードの集約方法を変更 START

        '        sql.AppendLine("SELECT /* IC3810101_004 */")
        '        sql.AppendLine("       T1.REZID")
        '        sql.AppendLine("     , T1.SERVICECODE_S")

        '        '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 START
        '        sql.AppendLine("     , T1.VIN")
        '        sql.AppendLine("     , T1.MODELCODE")
        '        sql.AppendLine("     , T1.CUSTOMERNAME")
        '        sql.AppendLine("     , T1.TELNO")
        '        sql.AppendLine("     , T1.MOBILE")
        '        '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 END

        '        ' 2012/05/15 KN 河原 【SERVICE_2事前準備対応】 整備受注NO・受付担当予定者を新たに取得 START
        '        sql.AppendLine("     , T1.ORDERNO")
        '        sql.AppendLine("     , TRIM(T1.ACCOUNT_PLAN) AS ACCOUNT_PLAN")
        '        ' 2012/05/15 KN 河原 【SERVICE_2事前準備対応】 整備受注NO・受付担当予定者を新たに取得 END

        '        ' 2012/07/05 KN 河原 【SERVICE_2事前準備対応】 顧客コード・識別Flagを新たに取得 START
        '        sql.AppendLine("     , T1.CUSTCD")
        '        sql.AppendLine("     , T1.CUSTOMERFLAG")
        '        ' 2012/07/05 KN 河原 【SERVICE_2事前準備対応】 顧客コード・識別Flagを新たに取得 END

        '        sql.AppendLine("     , NVL(T2.SERVICE_COUNT, 0) AS SERVICE_COUNT")
        '        sql.AppendLine("     , CASE ")
        '        sql.AppendLine("           WHEN T1.SERVICECODE_S = '40' THEN '40'")
        '        sql.AppendLine("           WHEN NVL(T2.SERVICE_COUNT, 0) > 0 THEN '20'")
        '        sql.AppendLine("           ELSE '30'")
        '        sql.AppendLine("       END AS SERVICECODE_CONV")
        '        sql.AppendLine("  FROM TBL_STALLREZINFO T1")
        '        sql.AppendLine("     , (")
        '        sql.AppendLine("       SELECT")
        '        sql.AppendLine("              SERVICECODE")
        '        sql.AppendLine("            , COUNT(SERVICECD) AS SERVICE_COUNT")
        '        sql.AppendLine("         FROM TBL_MERCHANDISEMST")
        '        sql.AppendLine("        WHERE DLRCD = :DLRCD")
        '        sql.AppendLine("          AND SERVICECD IS NOT NULL")
        '        sql.AppendLine("          AND DELFLG = '0'")
        '        sql.AppendLine("        GROUP BY SERVICECODE")
        '        sql.AppendLine("  ) T2")
        '        sql.AppendLine(" WHERE T1.SERVICECODE_S = T2.SERVICECODE(+)")
        '        sql.AppendLine("   AND T1.DLRCD = :DLRCD")
        '        sql.AppendLine("   AND T1.STRCD = :STRCD")
        '        ''2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発 START
        '        'sql.AppendLine("   AND TO_CHAR(T1.STARTTIME, 'YYYYMMDD') = :STARTTIME")
        '        sql.AppendLine("   AND TRUNC(T1.STARTTIME) BETWEEN TRUNC(:STARTTIME) AND TRUNC(:STARTTIME + :DAYS)")
        '        ''2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発 END
        '        sql.AppendLine("   AND T1.STATUS IN (1, 2)")
        '        sql.AppendLine("   AND T1.SERVICECODE_S < '90'")

        '        '2012/04/06 KN 瀧 【SERVICE_1】サービスコードの集約方法を変更 END
        '        '2012/04/13 KN 瀧 【SERVICE_1】サービスコードの集約方法を戻す END
        '        '2012/04/17 KN 瀧 【SERVICE_1】サービスコードの集約方法を変更(三回目) END

        '        '2012/02/28 KN 瀧 【SERVICE_1】管理予約IDがNULLの時データが取得できない不具合を修正 START
        '        ''sql.AppendLine("   AND PREZID IN (NULL, REZID)")
        '        'sql.AppendLine("   AND (CUSTCD = :CUSTCD OR CUSTOMERNAME = :CUSTOMERNAME OR VCLREGNO = :VCLREGNO OR VIN = :VIN)")
        '        sql.AppendLine("   AND (T1.PREZID = T1.REZID OR T1.PREZID IS NULL)")
        '        ' 2012/02/28 KN 瀧 【SERVICE_1】管理予約IDがNULLの時データが取得できない不具合を修正 END

        '        ' 2012/04/11 KN 佐藤 【SERVICE_1】対応済みチップと紐付く不具合を修正 START
        '        ''2012/04/10 KN 瀧 【SERVICE_1】ストール予約情報の取得条件の変更 START
        '        ''中断フラグ=0:有効 or 5:WALKIN、キャンセルフラグ=1:取消
        '        'sql.AppendLine("   AND STOPFLG IN ('0', '5') AND CANCELFLG = '1'")
        '        ''2012/04/10 KN 瀧 【SERVICE_1】ストール予約情報の取得条件の変更 END
        '        '中断フラグ=0:有効 or 5:WALKIN、キャンセルフラグ=1:取消を対象外とする
        '        sql.AppendLine("   AND NOT EXISTS (")
        '        sql.AppendLine("       SELECT 1")
        '        sql.AppendLine("         FROM TBL_STALLREZINFO")
        '        sql.AppendLine("        WHERE DLRCD = T1.DLRCD")
        '        sql.AppendLine("          AND STRCD = T1.STRCD")
        '        sql.AppendLine("          AND REZID = T1.REZID")
        '        sql.AppendLine("          AND STOPFLG IN ('0', '5')")
        '        sql.AppendLine("          AND CANCELFLG = '1'")
        '        sql.AppendLine("       )")
        '        ' 2012/04/11 KN 佐藤 【SERVICE_1】対応済みチップと紐付く不具合を修正 END

        '        Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101StallKeyDataTable)("IC3810101_004")
        '            ''パラメータの設定
        '            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)

        '            ''2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発 START
        '            'query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Char, rowIN.VISITTIMESTAMP.ToString("yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture))
        '            'If (rowIN.IsVISITTIMESTAMPNull = True) Then
        '            '    query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Char, DBNull.Value)
        '            'Else
        '            '    query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Char, rowIN.VISITTIMESTAMP.ToString("yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture))
        '            'End If
        '            If (rowIN.IsVISITTIMESTAMPNull = True) Then
        '                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, DBNull.Value)
        '                query.AddParameterWithTypeValue("DAYS", OracleDbType.Long, 0)
        '            Else
        '                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, rowIN.VISITTIMESTAMP)
        '                query.AddParameterWithTypeValue("DAYS", OracleDbType.Long, Me.StallRangeDays(rowIN.DLRCD, rowIN.STRCD))
        '            End If
        '            ''2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発 END

        '            '2012/02/28 KN 瀧 【SERVICE_1】管理予約IDがNULLの時データが取得できない不具合を修正 START
        '            'If (rowIN.IsCUSTOMERCODENull = True) Then
        '            '    query.AddParameterWithTypeValue("CUSTCD", OracleDbType.Varchar2, DBNull.Value)
        '            'Else
        '            '    query.AddParameterWithTypeValue("CUSTCD", OracleDbType.Varchar2, rowIN.CUSTOMERCODE)
        '            'End If
        '            'If (rowIN.IsCUSTOMERNAMENull = True) Then
        '            '    query.AddParameterWithTypeValue("CUSTOMERNAME", OracleDbType.Varchar2, DBNull.Value)
        '            'Else
        '            '    query.AddParameterWithTypeValue("CUSTOMERNAME", OracleDbType.Varchar2, rowIN.CUSTOMERNAME)
        '            'End If
        '            'If (rowIN.IsVCLREGNONull = True) Then
        '            '    query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.Varchar2, DBNull.Value)
        '            'Else
        '            '    query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.Varchar2, rowIN.VCLREGNO)
        '            'End If
        '            'If (rowIN.IsVINNull = True) Then
        '            '    query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, DBNull.Value)
        '            'Else
        '            '    query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowIN.VIN)
        '            'End If
        '            'Dim whereOr As New List(Of String)
        '            Dim whereAnd As New List(Of String)
        '            ' 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する START
        '            'If (rowIN.IsCUSTOMERCODENull = False _
        '            '    AndAlso rowIN.CUSTOMERCODE.Trim.Length > 0) Then
        '            '    '2012/04/10 KN 瀧 【SERVICE_1】ストール予約情報の取得条件の変更 START
        '            '    'whereOr.Add("CUSTCD = :CUSTCD")
        '            '    whereAnd.Add("(CUSTCD = :CUSTCD OR CUSTCD IS NULL)")
        '            '    '2012/04/10 KN 瀧 【SERVICE_1】ストール予約情報の取得条件の変更 END
        '            '    query.AddParameterWithTypeValue("CUSTCD", OracleDbType.Varchar2, rowIN.CUSTOMERCODE)
        '            'End If
        '            If (rowCK.IsCUSTOMERCODENull = False _
        '                AndAlso rowCK.CUSTOMERCODE.Trim.Length > 0) Then
        '                '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 START
        '                'whereAnd.Add("(T1.CUSTCD = :CUSTCD OR T1.CUSTCD IS NULL)")
        '                whereAnd.Add("(T1.CUSTCD = :CUSTCD OR TRIM(T1.CUSTCD) IS NULL)")
        '                '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 END
        '                query.AddParameterWithTypeValue("CUSTCD", OracleDbType.NVarchar2, rowCK.CUSTOMERCODE)
        '            End If
        '            ' 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する END
        '            ' 2012/04/12 KN 佐藤 【SERVICE_1】予約の紐付け時、顧客名が変更されている可能性があるため、条件から外す START
        '            'If (rowIN.IsCUSTOMERNAMENull = False _
        '            '    AndAlso rowIN.CUSTOMERNAME.Trim.Length > 0) Then
        '            '    'whereOr.Add("CUSTOMERNAME = :CUSTOMERNAME")
        '            '    whereAnd.Add("CUSTOMERNAME = :CUSTOMERNAME")
        '            '    query.AddParameterWithTypeValue("CUSTOMERNAME", OracleDbType.Varchar2, rowIN.CUSTOMERNAME)
        '            'End If
        '            ' 2012/04/12 KN 佐藤 【SERVICE_1】予約の紐付け時、顧客名が変更されている可能性があるため、条件から外す END
        '            If (rowIN.IsVCLREGNONull = False _
        '                AndAlso rowIN.VCLREGNO.Trim.Length > 0) Then
        '                'whereOr.Add("VCLREGNO = :VCLREGNO")
        '                whereAnd.Add("T1.VCLREGNO = :VCLREGNO")
        '                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.Varchar2, rowIN.VCLREGNO)
        '            End If
        '            If (rowIN.IsVINNull = False _
        '                AndAlso rowIN.VIN.Trim.Length > 0) Then
        '                'whereOr.Add("VIN = :VIN")
        '                whereAnd.Add("T1.VIN = :VIN")
        '                query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowIN.VIN)
        '            End If
        '            If whereAnd.Count > 0 Then
        '                '2012/04/10 KN 瀧 【SERVICE_1】ストール予約情報の取得条件の変更 START
        '                'sql.AppendLine(String.Format("   AND ({0})", String.Join(" OR ", whereOr.ToArray())))
        '                sql.AppendLine(String.Format(CultureInfo.CurrentCulture, "   AND {0}", String.Join(" AND ", whereAnd.ToArray())))
        '                '2012/04/10 KN 瀧 【SERVICE_1】ストール予約情報の取得条件の変更 END
        '            Else
        '                ''顧客コード、顧客名、車両登録No,VINの入力が１つも設定されていなかった場合は該当データ無しとする
        '                Using newDT As New IC3810101DataSet.IC3810101StallKeyDataTable
        '                    Return newDT
        '                End Using
        '            End If
        '            ' 2012/02/28 KN 瀧 【SERVICE_1】管理予約IDがNULLの時データが取得できない不具合を修正 END

        '            ' 2012/04/11 KN 佐藤 【SERVICE_1】START
        '            sql.AppendLine(" ORDER BY T1.STARTTIME")
        '            ' 2012/04/11 KN 佐藤 【SERVICE_1】END

        '            ''SQLの実行
        '            query.CommandText = sql.ToString()
        '            Using dt As IC3810101DataSet.IC3810101StallKeyDataTable = query.GetData()
        '                ''終了ログの出力
        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                   , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                   , Me.GetType.ToString _
        '                   , MethodBase.GetCurrentMethod.Name _
        '                   , dt.Rows.Count))
        '                Return dt
        '            End Using
        '        End Using
        '    Finally

        '    End Try
        'End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ' ''' <summary>
        ' ''' サービス来店者登録(新規登録)
        ' ''' </summary>
        ' ''' <param name="rowIN">サービス来店者引数</param>
        ' ''' <param name="rowCK">来客者キー情報</param>
        ' ''' <param name="rowSK">ストール予約キー情報</param>
        ' ''' <returns>来店実績連番</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' 2012/05/15 KN 河原 【SERVICE_2事前準備対応】整備受注NOをORDERNOに登録とDEFAULTSACODEの登録方法変更
        ' ''' 2012/07/05 KN 河原 【SERVICE_2事前準備対応】サービス来店管理テーブルの顧客区分・顧客コード・期間顧客IDを登録
        ' ''' 2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応
        ' ''' </history>
        'Public Function InsertServiceVisit(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow _
        '                                   , ByVal rowCK As IC3810101DataSet.IC3810101CustomerKeyRow _
        '                                   , ByVal rowSK As IC3810101DataSet.IC3810101StallKeyRow) As Long
        '    Try
        '        ''引数をログに出力
        '        Dim args As New List(Of String)
        '        ' DataRow内の項目を列挙
        '        Me.AddLogData(args, rowIN)
        '        Me.AddLogData(args, rowCK)
        '        Me.AddLogData(args, rowSK)
        '        ''開始ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} IN:{2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , String.Join(", ", args.ToArray())))

        '        Dim visitseq As Long = 0
        '        Using query As New DBSelectQuery(Of DataTable)("IC3810101_101")
        '            ''SQLの設定
        '            Dim sqlNextVal As New StringBuilder
        '            sqlNextVal.AppendLine("SELECT /* IC3810101_101 */")
        '            sqlNextVal.AppendLine("       SEQ_SERVICE_VISIT_MANAGEMENT.NEXTVAL AS VISITSEQ")
        '            sqlNextVal.AppendLine("  FROM DUAL")
        '            query.CommandText = sqlNextVal.ToString()
        '            Using dt As DataTable = query.GetData()
        '                visitseq = CType(dt.Rows(0)("VISITSEQ"), Long)
        '            End Using
        '        End Using
        '        Try
        '            ''SQLの設定
        '            Dim sqlInsert As New StringBuilder
        '            sqlInsert.AppendLine("INSERT /* IC3810101_102 */")
        '            sqlInsert.AppendLine("  INTO TBL_SERVICE_VISIT_MANAGEMENT (")
        '            sqlInsert.AppendLine("       VISITSEQ")
        '            sqlInsert.AppendLine("     , DLRCD")
        '            sqlInsert.AppendLine("     , STRCD")
        '            sqlInsert.AppendLine("     , VISITTIMESTAMP")
        '            sqlInsert.AppendLine("     , VCLREGNO")
        '            sqlInsert.AppendLine("     , CUSTSEGMENT")
        '            sqlInsert.AppendLine("     , CUSTID")
        '            sqlInsert.AppendLine("     , STAFFCD")
        '            sqlInsert.AppendLine("     , VISITPERSONNUM")
        '            sqlInsert.AppendLine("     , VISITMEANS")
        '            sqlInsert.AppendLine("     , VIN")
        '            ''2012/01/06 追加 サービス来店者情報に基幹顧客ID,モデルコード,電話番号,携帯番号項目の追加
        '            sqlInsert.AppendLine("     , DMSID")
        '            sqlInsert.AppendLine("     , MODELCODE")
        '            sqlInsert.AppendLine("     , TELNO")
        '            sqlInsert.AppendLine("     , MOBILE")
        '            sqlInsert.AppendLine("     , SEQNO")
        '            sqlInsert.AppendLine("     , SEX")
        '            sqlInsert.AppendLine("     , NAME")
        '            sqlInsert.AppendLine("     , DEFAULTSACODE")
        '            sqlInsert.AppendLine("     , SACODE")
        '            sqlInsert.AppendLine("     , ASSIGNTIMESTAMP")
        '            sqlInsert.AppendLine("     , SERVICECODE")
        '            sqlInsert.AppendLine("     , REZID")
        '            sqlInsert.AppendLine("     , PARKINGCODE")
        '            sqlInsert.AppendLine("     , VIPMARK")
        '            sqlInsert.AppendLine("     , ASSIGNSTATUS")
        '            sqlInsert.AppendLine("     , QUEUESTATUS")
        '            sqlInsert.AppendLine("     , HOLDSTAFF")
        '            sqlInsert.AppendLine("     , ORDERNO")
        '            sqlInsert.AppendLine("     , FREZID")
        '            sqlInsert.AppendLine("     , REGISTKIND")
        '            sqlInsert.AppendLine("     , CREATEDATE")
        '            sqlInsert.AppendLine("     , UPDATEDATE")
        '            sqlInsert.AppendLine("     , CREATEACCOUNT")
        '            sqlInsert.AppendLine("     , UPDATEACCOUNT")
        '            sqlInsert.AppendLine("     , CREATEID")
        '            sqlInsert.AppendLine("     , UPDATEID")
        '            sqlInsert.AppendLine(")")
        '            sqlInsert.AppendLine("VALUES (")
        '            sqlInsert.AppendLine("       :VISITSEQ")
        '            sqlInsert.AppendLine("     , :DLRCD")
        '            sqlInsert.AppendLine("     , :STRCD")
        '            sqlInsert.AppendLine("     , :VISITTIMESTAMP")
        '            sqlInsert.AppendLine("     , :VCLREGNO")
        '            sqlInsert.AppendLine("     , :CUSTSEGMENT")
        '            sqlInsert.AppendLine("     , :CUSTID")
        '            sqlInsert.AppendLine("     , :STAFFCD")
        '            sqlInsert.AppendLine("     , :VISITPERSONNUM")
        '            sqlInsert.AppendLine("     , :VISITMEANS")
        '            sqlInsert.AppendLine("     , :VIN")
        '            ''2012/01/06 追加 サービス来店者情報に基幹顧客ID,モデルコード,電話番号,携帯番号項目の追加
        '            sqlInsert.AppendLine("     , :DMSID")
        '            sqlInsert.AppendLine("     , :MODELCODE")
        '            sqlInsert.AppendLine("     , :TELNO")
        '            sqlInsert.AppendLine("     , :MOBILE")
        '            sqlInsert.AppendLine("     , :SEQNO")
        '            sqlInsert.AppendLine("     , :SEX")
        '            sqlInsert.AppendLine("     , :NAME")
        '            sqlInsert.AppendLine("     , :DEFAULTSACODE")
        '            sqlInsert.AppendLine("     , :SACODE")
        '            sqlInsert.AppendLine("     , :ASSIGNTIMESTAMP")
        '            sqlInsert.AppendLine("     , :SERVICECODE")
        '            sqlInsert.AppendLine("     , :REZID")
        '            sqlInsert.AppendLine("     , :PARKINGCODE")
        '            sqlInsert.AppendLine("     , :VIPMARK")
        '            sqlInsert.AppendLine("     , :ASSIGNSTATUS")
        '            sqlInsert.AppendLine("     , :QUEUESTATUS")
        '            sqlInsert.AppendLine("     , :HOLDSTAFF")
        '            sqlInsert.AppendLine("     , :ORDERNO")
        '            sqlInsert.AppendLine("     , :REZID")
        '            sqlInsert.AppendLine("     , :REGISTKIND")
        '            sqlInsert.AppendLine("     , SYSDATE")
        '            sqlInsert.AppendLine("     , SYSDATE")
        '            sqlInsert.AppendLine("     , :ACCOUNT")
        '            sqlInsert.AppendLine("     , :ACCOUNT")
        '            sqlInsert.AppendLine("     , :SYSTEM")
        '            sqlInsert.AppendLine("     , :SYSTEM")
        '            sqlInsert.AppendLine(") ")
        '            Using query As New DBUpdateQuery("IC3810101_102")
        '                query.CommandText = sqlInsert.ToString()
        '                ''パラメータの設定
        '                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, visitseq)
        '                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
        '                If (rowIN.IsVISITTIMESTAMPNull = True) Then
        '                    query.AddParameterWithTypeValue("VISITTIMESTAMP", OracleDbType.Date, DBNull.Value)
        '                Else
        '                    query.AddParameterWithTypeValue("VISITTIMESTAMP", OracleDbType.Date, rowIN.VISITTIMESTAMP)
        '                End If
        '                If (rowIN.IsVCLREGNONull = True) Then
        '                    query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, DBNull.Value)
        '                Else
        '                    query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, rowIN.VCLREGNO)
        '                End If

        '                '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 START
        '                '' 2012/07/05 KN 河原 【SERVICE_2事前準備対応】サービス来店管理テーブルの顧客区分・顧客コード・期間顧客IDを登録　START
        '                ''If (rowIN.IsCUSTSEGMENTNull = True) Then
        '                ''    query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, DBNull.Value)
        '                ''Else
        '                ''    query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, rowIN.CUSTSEGMENT)
        '                ''End If
        '                ' '' 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する START
        '                ' ''If (rowIN.IsCUSTOMERCODENull = True) Then
        '                ' ''    query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, DBNull.Value)
        '                ' ''Else
        '                ' ''    query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, rowIN.CUSTOMERCODE)
        '                ' ''End If
        '                ''If (rowCK.IsCUSTOMERCODENull = True) Then
        '                ''    query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, DBNull.Value)
        '                ''Else
        '                ''    query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, rowCK.CUSTOMERCODE)
        '                ''End If
        '                ' '' 2012/04/12 KN 佐藤 【SERVICE_1】「TBLORG_CUSTOMER」から顧客IDを取得する END
        '                ''自社客のチェック
        '                'If 0 < rowSK.REZID AndAlso StatusVisit.Equals(rowSK.CUSTOMERFLAG) Then
        '                '    '自社客で設定
        '                '    query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, ManageStatusVisit)
        '                '    '顧客コード
        '                '    If (rowSK.IsCUSTCDNull = True) Then
        '                '        query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, DBNull.Value)
        '                '        query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, DBNull.Value)
        '                '    Else
        '                '        query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, rowSK.CUSTCD)
        '                '        query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, rowSK.CUSTCD)
        '                '    End If
        '                'Else '自社客以外
        '                '    If (rowIN.IsCUSTSEGMENTNull = True) Then
        '                '        query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, DBNull.Value)
        '                '    Else
        '                '        query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, rowIN.CUSTSEGMENT)
        '                '    End If
        '                '    If (rowCK.IsCUSTOMERCODENull = True) Then
        '                '        query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, DBNull.Value)
        '                '    Else
        '                '        query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, rowCK.CUSTOMERCODE)
        '                '    End If
        '                '    If (rowCK.IsDMSIDNull = True) Then
        '                '        query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, DBNull.Value)
        '                '    Else
        '                '        query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, rowCK.DMSID)
        '                '    End If
        '                'End If
        '                '' 2012/07/05 KN 河原 【SERVICE_2事前準備対応】サービス来店管理テーブルの顧客区分・顧客コード・期間顧客IDを登録　END

        '                'If (rowIN.IsSTAFFCDNull = True) Then
        '                '    query.AddParameterWithTypeValue("STAFFCD", OracleDbType.Varchar2, DBNull.Value)
        '                'Else
        '                '    query.AddParameterWithTypeValue("STAFFCD", OracleDbType.Varchar2, rowIN.STAFFCD)
        '                'End If
        '                'If (rowIN.IsVISITPERSONNUMNull = True) Then
        '                '    query.AddParameterWithTypeValue("VISITPERSONNUM", OracleDbType.Decimal, DBNull.Value)
        '                'Else
        '                '    query.AddParameterWithTypeValue("VISITPERSONNUM", OracleDbType.Decimal, rowIN.VISITPERSONNUM)
        '                'End If
        '                'If (rowIN.IsVISITMEANSNull = True) Then
        '                '    query.AddParameterWithTypeValue("VISITMEANS", OracleDbType.Char, DBNull.Value)
        '                'Else
        '                '    query.AddParameterWithTypeValue("VISITMEANS", OracleDbType.Char, rowIN.VISITMEANS)
        '                'End If
        '                'If (rowIN.IsVINNull = True) Then
        '                '    query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, DBNull.Value)
        '                'Else
        '                '    query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowIN.VIN)
        '                'End If
        '                ' ''2012/01/06 追加 サービス来店者情報に基幹顧客ID,モデルコード,電話番号,携帯番号項目の追加

        '                '' 2012/07/05 KN 河原 【SERVICE_2事前準備対応】サービス来店管理テーブルの顧客区分・顧客コード・期間顧客IDを登録　START
        '                ''If (rowCK.IsDMSIDNull = True) Then
        '                ''    query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, DBNull.Value)
        '                ''Else
        '                ''    query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, rowCK.DMSID)
        '                ''End If
        '                '' 2012/07/05 KN 河原 【SERVICE_2事前準備対応】サービス来店管理テーブルの顧客区分・顧客コード・期間顧客IDを登録　START

        '                'If (rowCK.IsMODELCODENull = True) Then
        '                '    query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, DBNull.Value)
        '                'Else
        '                '    query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, rowCK.MODELCODE)
        '                'End If
        '                'If (rowCK.IsTELNONull = True) Then
        '                '    query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, DBNull.Value)
        '                'Else
        '                '    query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, rowCK.TELNO)
        '                'End If
        '                'If (rowCK.IsMOBILENull = True) Then
        '                '    query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, DBNull.Value)
        '                'Else
        '                '    query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, rowCK.MOBILE)
        '                'End If
        '                'If (rowIN.IsSEQNONull = True) Then
        '                '    query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, DBNull.Value)
        '                'Else
        '                '    query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, rowIN.SEQNO)
        '                'End If
        '                'If (rowIN.IsSEXNull = True) OrElse (String.IsNullOrEmpty(rowIN.SEX) = True) Then
        '                '    query.AddParameterWithTypeValue("SEX", OracleDbType.Char, "0")
        '                'Else
        '                '    query.AddParameterWithTypeValue("SEX", OracleDbType.Char, rowIN.SEX)
        '                'End If
        '                'If (rowIN.IsCUSTOMERNAMENull = True) Then
        '                '    query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, DBNull.Value)
        '                'Else
        '                '    query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, rowIN.CUSTOMERNAME)
        '                'End If
        '                If Not (rowIN.IsCUSTSEGMENTNull) AndAlso ManageStatusNotVisit.Equals(rowIN.CUSTSEGMENT) AndAlso _
        '                   0 < rowSK.REZID AndAlso StatusVisit.Equals(rowSK.CUSTOMERFLAG) Then
        '                    '「サービス来店者引数：顧客区分=2：未取引客 AND 予約情報有り AND 予約情報：顧客区分=0：自社客」の場合
        '                    '顧客区分
        '                    query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, ManageStatusVisit)
        '                    '顧客コード、基幹顧客ID
        '                    If rowSK.IsCUSTCDNull Then
        '                        query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, DBNull.Value)
        '                        query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, rowSK.CUSTCD)
        '                        query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, rowSK.CUSTCD)
        '                    End If
        '                    'VIN
        '                    If rowSK.IsVINNull Then
        '                        query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowSK.VIN)
        '                    End If
        '                    'モデルコード
        '                    If rowSK.IsMODELCODENull Then
        '                        query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, rowSK.MODELCODE)
        '                    End If
        '                    '氏名
        '                    If rowSK.IsCUSTOMERNAMENull Then
        '                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, rowSK.CUSTOMERNAME)
        '                    End If
        '                    '電話番号
        '                    If rowSK.IsTELNONull Then
        '                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, rowSK.TELNO)
        '                    End If
        '                    '携帯番号
        '                    If rowSK.IsMOBILENull Then
        '                        query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, rowSK.MOBILE)
        '                    End If
        '                ElseIf Not (rowIN.IsCUSTSEGMENTNull) AndAlso ManageStatusVisit.Equals(rowIN.CUSTSEGMENT) AndAlso _
        '                       0 < rowSK.REZID AndAlso StatusNotVisit.Equals(rowSK.CUSTOMERFLAG) Then
        '                    '「サービス来店者引数：顧客区分=1：自社客 AND 予約情報有り AND 予約情報：顧客区分=1：未取引客」の場合
        '                    '顧客区分
        '                    query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, ManageStatusVisit)
        '                    '顧客コード
        '                    If rowCK.IsCUSTOMERCODENull Then
        '                        query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, rowCK.CUSTOMERCODE)
        '                    End If
        '                    '基幹顧客ID
        '                    If rowCK.IsDMSIDNull Then
        '                        query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, rowCK.DMSID)
        '                    End If
        '                    'VIN
        '                    If rowIN.IsVINNull Then
        '                        query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowCK.VIN)
        '                    End If
        '                    'モデルコード
        '                    If rowCK.IsMODELCODENull Then
        '                        query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, rowCK.MODELCODE)
        '                    End If
        '                    '氏名
        '                    If rowCK.IsNAMENull Then
        '                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, rowCK.NAME)
        '                    End If
        '                    '電話番号
        '                    If rowCK.IsTELNONull Then
        '                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, rowCK.TELNO)
        '                    End If
        '                    '携帯番号
        '                    If rowCK.IsMOBILENull Then
        '                        query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, rowCK.MOBILE)
        '                    End If
        '                Else
        '                    If 0 < rowSK.REZID AndAlso StatusVisit.Equals(rowSK.CUSTOMERFLAG) Then
        '                        '「予約情報有り AND 予約情報：顧客区分=0：自社客」の場合
        '                        '顧客区分
        '                        query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, ManageStatusVisit)
        '                        '顧客コード、基幹顧客ID
        '                        If (rowSK.IsCUSTCDNull = True) Then
        '                            query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, DBNull.Value)
        '                            query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, DBNull.Value)
        '                        Else
        '                            query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, rowSK.CUSTCD)
        '                            query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, rowSK.CUSTCD)
        '                        End If
        '                    Else
        '                        '顧客区分
        '                        If (rowIN.IsCUSTSEGMENTNull = True) Then
        '                            query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, DBNull.Value)
        '                        Else
        '                            query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.Char, rowIN.CUSTSEGMENT)
        '                        End If
        '                        '顧客コード
        '                        If (rowCK.IsCUSTOMERCODENull = True) Then
        '                            query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, DBNull.Value)
        '                        Else
        '                            query.AddParameterWithTypeValue("CUSTID", OracleDbType.Char, rowCK.CUSTOMERCODE)
        '                        End If
        '                        '基幹顧客ID
        '                        If (rowCK.IsDMSIDNull = True) Then
        '                            query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, DBNull.Value)
        '                        Else
        '                            query.AddParameterWithTypeValue("DMSID", OracleDbType.Char, rowCK.DMSID)
        '                        End If
        '                    End If
        '                    'VIN
        '                    If (rowCK.IsVINNull = True) Then
        '                        query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, rowCK.VIN)
        '                    End If
        '                    'モデルコード
        '                    If (rowCK.IsMODELCODENull = True) Then
        '                        query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, rowCK.MODELCODE)
        '                    End If
        '                    '氏名
        '                    If (rowCK.IsNAMENull = True) Then
        '                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, rowCK.NAME)
        '                    End If
        '                    '電話番号
        '                    If (rowCK.IsTELNONull = True) Then
        '                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, rowCK.TELNO)
        '                    End If
        '                    '携帯番号
        '                    If (rowCK.IsMOBILENull = True) Then
        '                        query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, rowCK.MOBILE)
        '                    End If
        '                End If
        '                '顧客担当スタッフコード
        '                If (rowIN.IsSTAFFCDNull = True) Then
        '                    query.AddParameterWithTypeValue("STAFFCD", OracleDbType.Varchar2, DBNull.Value)
        '                Else
        '                    query.AddParameterWithTypeValue("STAFFCD", OracleDbType.Varchar2, rowIN.STAFFCD)
        '                End If
        '                '来店人数
        '                If (rowIN.IsVISITPERSONNUMNull = True) Then
        '                    query.AddParameterWithTypeValue("VISITPERSONNUM", OracleDbType.Decimal, DBNull.Value)
        '                Else
        '                    query.AddParameterWithTypeValue("VISITPERSONNUM", OracleDbType.Decimal, rowIN.VISITPERSONNUM)
        '                End If
        '                '来店手段
        '                If (rowIN.IsVISITMEANSNull = True) Then
        '                    query.AddParameterWithTypeValue("VISITMEANS", OracleDbType.Char, DBNull.Value)
        '                Else
        '                    query.AddParameterWithTypeValue("VISITMEANS", OracleDbType.Char, rowIN.VISITMEANS)
        '                End If
        '                'シーケンス番号
        '                If (rowIN.IsSEQNONull = True) Then
        '                    query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, DBNull.Value)
        '                Else
        '                    query.AddParameterWithTypeValue("SEQNO", OracleDbType.Decimal, rowIN.SEQNO)
        '                End If
        '                '性別
        '                If (rowIN.IsSEXNull = True) OrElse (String.IsNullOrEmpty(rowIN.SEX) = True) Then
        '                    query.AddParameterWithTypeValue("SEX", OracleDbType.Char, "0")
        '                Else
        '                    query.AddParameterWithTypeValue("SEX", OracleDbType.Char, rowIN.SEX)
        '                End If
        '                '2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 END

        '                ' 2012/05/15 KN 河原 【SERVICE_2事前準備対応】 DEFAULTSACODEの登録方法変更 START
        '                If (rowSK.IsACCOUNT_PLANNull = True) Then
        '                    If (rowIN.IsDEFAULTSACODENull = True) Then
        '                        query.AddParameterWithTypeValue("DEFAULTSACODE", OracleDbType.Varchar2, DBNull.Value)
        '                    Else
        '                        query.AddParameterWithTypeValue("DEFAULTSACODE", OracleDbType.Varchar2, rowIN.DEFAULTSACODE)
        '                    End If
        '                Else
        '                    query.AddParameterWithTypeValue("DEFAULTSACODE", OracleDbType.Varchar2, rowSK.ACCOUNT_PLAN)
        '                End If
        '                ' 2012/05/15 KN 河原 【SERVICE_2事前準備対応】 DEFAULTSACODEの登録方法変更 END

        '                query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, DBNull.Value)
        '                query.AddParameterWithTypeValue("ASSIGNTIMESTAMP", OracleDbType.Date, DBNull.Value)
        '                If (rowSK.IsSERVICECODE_CONVNull = True) Then
        '                    query.AddParameterWithTypeValue("SERVICECODE", OracleDbType.Char, " ")
        '                Else
        '                    query.AddParameterWithTypeValue("SERVICECODE", OracleDbType.Char, rowSK.SERVICECODE_CONV)
        '                End If
        '                query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, rowSK.REZID)
        '                query.AddParameterWithTypeValue("PARKINGCODE", OracleDbType.Varchar2, DBNull.Value)
        '                query.AddParameterWithTypeValue("VIPMARK", OracleDbType.Char, "0")
        '                query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.Char, "0")
        '                query.AddParameterWithTypeValue("QUEUESTATUS", OracleDbType.Char, "0")
        '                query.AddParameterWithTypeValue("HOLDSTAFF", OracleDbType.Varchar2, DBNull.Value)

        '                ' 2012/05/15 KN 河原 【SERVICE_2事前準備対応】 整備受注NOをORDERNOに登録 START
        '                If (rowSK.IsORDERNONull = True) Then
        '                    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, DBNull.Value)
        '                Else
        '                    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, rowSK.ORDERNO)
        '                End If
        '                ' 2012/05/15 KN 河原 【SERVICE_2事前準備対応】 整備受注NOをORDERNOに登録 END

        '                'query.AddParameterWithTypeValue("FREZID", OracleDbType.Decimal, rowSK.REZID)
        '                query.AddParameterWithTypeValue("REGISTKIND", OracleDbType.Char, "0")
        '                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
        '                query.AddParameterWithTypeValue("SYSTEM", OracleDbType.Varchar2, rowIN.SYSTEM)
        '                ''SQLの実行
        '                query.Execute()
        '            End Using
        '        Finally
        '        End Try
        '        ''終了ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:VISITSEQ = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , visitseq))
        '        Return visitseq
        '    Finally

        '    End Try
        'End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ' 2012/04/11 KN 佐藤 【SERVICE_1】対応済みチップと紐付く不具合を修正 START
        ' ''' <summary>
        ' ''' ストール実績情報を取得
        ' ''' </summary>
        ' ''' <param name="rowIN">サービス来店者引数</param>
        ' ''' <param name="rowStallKey">ストール予約情報</param>
        ' ''' <returns>ストール実績情報</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' </history>
        'Public Function GetProcessInfo(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow, _
        '                                ByVal rowStallKey As IC3810101DataSet.IC3810101StallKeyRow) As IC3810101DataSet.IC3810101ProcessInfoDataTable
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

        '        Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101ProcessInfoDataTable)("IC3810101_005")
        '            ''SQLの設定
        '            Dim sql As New StringBuilder
        '            sql.AppendLine("SELECT /* IC3810101_005 */")
        '            sql.AppendLine("       DSEQNO")
        '            sql.AppendLine("     , SEQNO")
        '            sql.AppendLine("     , RESULT_STATUS")
        '            sql.AppendLine("  FROM TBL_STALLPROCESS")
        '            sql.AppendLine(" WHERE DLRCD = :DLRCD")
        '            sql.AppendLine("   AND STRCD = :STRCD")
        '            sql.AppendLine("   AND REZID = :REZID")
        '            sql.AppendLine(" ORDER BY DSEQNO DESC")
        '            sql.AppendLine("     , SEQNO DESC")
        '            ''パラメータの設定
        '            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
        '            query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, rowStallKey.REZID)
        '            ''SQLの実行
        '            query.CommandText = sql.ToString()
        '            Using dt As IC3810101DataSet.IC3810101ProcessInfoDataTable = query.GetData()
        '                ''終了ログの出力
        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                   , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                   , Me.GetType.ToString _
        '                   , MethodBase.GetCurrentMethod.Name _
        '                   , dt.Rows.Count))
        '                Return dt
        '            End Using
        '        End Using
        '    Finally

        '    End Try
        'End Function
        ' 2012/04/11 KN 佐藤 【SERVICE_1】対応済みチップと紐付く不具合を修正 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '' 2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発 START

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ' '' 2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発 START
        ' ''' <summary>
        ' ''' 顧客情報及び顧客車両情報の取得
        ' ''' </summary>
        ' ''' <param name="dealerCD">販売店コード</param>
        ' ''' <param name="storeCD">店舗コード</param>
        ' ''' <param name="vehicleRegNo">車両登録No</param>
        ' ''' <returns>顧客情報及び顧客車両情報</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' 2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発
        ' ''' </history>
        'Public Function GetCustomerVehicleInfo(ByVal dealerCD As String, _
        '                                       ByVal storeCD As String, _
        '                                       ByVal vehicleRegNo As String) As IC3810101DataSet.IC3810101CustomerVehicleInfoRow

        '    ''引数をログに出力
        '    Dim args As New List(Of String)
        '    ''販売店コード
        '    args.Add(String.Format(CultureInfo.CurrentCulture, "dealerCD = {0}", dealerCD))
        '    ''店舗コード
        '    args.Add(String.Format(CultureInfo.CurrentCulture, "storeCD = {0}", storeCD))
        '    ''車両登録No
        '    args.Add(String.Format(CultureInfo.CurrentCulture, "vehicleRegNo = {0}", vehicleRegNo))
        '    ''開始ログの出力
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '        , "{0}.{1} IN:{2}" _
        '        , Me.GetType.ToString _
        '        , MethodBase.GetCurrentMethod.Name _
        '        , String.Join(", ", args.ToArray())))
        '    ''SQLの設定
        '    Dim sql As New StringBuilder
        '    sql.AppendLine("SELECT /* IC3810101_006 */")
        '    sql.AppendLine("       '1' AS CUSTSEGMENT")
        '    sql.AppendLine("     , T1.ORIGINALID AS CUSTOMERCODE")
        '    sql.AppendLine("     , T2.SEX")
        '    sql.AppendLine("     , T2.NAME")
        '    sql.AppendLine("     , T1.VCLREGNO")
        '    sql.AppendLine("     , T1.VIN")
        '    sql.AppendLine("     , 0 AS SEQNO")
        '    sql.AppendLine("     , T2.STAFFCD")
        '    sql.AppendLine("     , T1.SACODE")
        '    sql.AppendLine("     , T2.UPDATEDATE")
        '    sql.AppendLine("  FROM TBLORG_VCLINFO T1")
        '    sql.AppendLine("     , TBLORG_CUSTOMER T2")
        '    sql.AppendLine("     , TBLORG_BRANCHINFO T3")
        '    sql.AppendLine(" WHERE T1.ORIGINALID = T2.ORIGINALID")
        '    sql.AppendLine("   AND T1.DLRCD = T2.DLRCD")
        '    sql.AppendLine("   AND T1.DLRCD = T3.DLRCD")
        '    sql.AppendLine("   AND T1.STRCD = T3.STRCD")
        '    sql.AppendLine("   AND T1.ORIGINALID = T3.ORIGINALID")
        '    sql.AppendLine("   AND T1.VIN = T3.VIN")
        '    sql.AppendLine("   AND T1.DELFLG = '0'")
        '    sql.AppendLine("   AND T1.DLRCD = :DLRCD")
        '    sql.AppendLine("   AND T1.VCLREGNO IN (:VCLREGNO, :CONVVCLREGNO)")
        '    sql.AppendLine("   AND T2.DELFLG = '0'")
        '    sql.AppendLine("   AND T3.RMFLG = '1'")
        '    sql.AppendLine(" UNION ALL ")
        '    sql.AppendLine("SELECT '2' AS CUSTSEGMENT")
        '    sql.AppendLine("     , T1.CSTID AS CUSTOMERCODE")
        '    sql.AppendLine("     , T2.SEX")
        '    sql.AppendLine("     , T2.NAME")
        '    sql.AppendLine("     , T1.VCLREGNO")
        '    sql.AppendLine("     , ' ' AS VIN")
        '    sql.AppendLine("     , T1.SEQNO")
        '    sql.AppendLine("     , T2.STAFFCD")
        '    sql.AppendLine("     , T2.SACODE")
        '    sql.AppendLine("     , T2.UPDATEDATE")
        '    sql.AppendLine("  FROM TBL_NEWCUSTOMERVCLRE T1")
        '    sql.AppendLine("     , TBL_NEWCUSTOMER T2")
        '    sql.AppendLine(" WHERE T1.CSTID = T2.CSTID")
        '    sql.AppendLine("   AND T1.DLRCD = T2.DLRCD")
        '    sql.AppendLine("   AND T1.DELFLG = '0'")
        '    sql.AppendLine("   AND T1.DLRCD = :DLRCD")
        '    sql.AppendLine("   AND T1.VCLREGNO IN (:VCLREGNO, :CONVVCLREGNO)")
        '    sql.AppendLine("   AND T2.DELFLG = '0'")
        '    sql.AppendLine("   AND TRIM(T2.ORIGINALID) IS NULL")
        '    sql.AppendLine("   AND TRIM(T2.SUBCUSTOMERID) IS NULL")
        '    sql.AppendLine(" ORDER BY CUSTSEGMENT, UPDATEDATE DESC")
        '    Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101CustomerVehicleInfoDataTable)("IC3810101_006")
        '        ''パラメータの設定
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCD)
        '        query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, vehicleRegNo)
        '        query.AddParameterWithTypeValue("CONVVCLREGNO", OracleDbType.NVarchar2, Me.ConvVehicleNo(dealerCD, storeCD, vehicleRegNo))
        '        ''SQLの実行
        '        query.CommandText = sql.ToString()
        '        Using dt As IC3810101DataSet.IC3810101CustomerVehicleInfoDataTable = query.GetData()
        '            ''終了ログの出力
        '            Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '               , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '               , Me.GetType.ToString _
        '               , MethodBase.GetCurrentMethod.Name _
        '               , dt.Rows.Count))
        '            If dt.Rows.Count = 0 Then
        '                Return dt.NewIC3810101CustomerVehicleInfoRow
        '            End If
        '            Return DirectCast(dt.Rows(0), IC3810101DataSet.IC3810101CustomerVehicleInfoRow)
        '        End Using
        '    End Using
        'End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ' ''' <summary>
        ' ''' サービス来店情報更新
        ' ''' </summary>
        ' ''' <param name="visitSeq">来店実績連番</param>
        ' ''' <param name="sysDate">システム日時</param>
        ' ''' <param name="saCode">SAコード</param>
        ' ''' <param name="account">アカウント</param>
        ' ''' <param name="system">機能ID</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' 2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発
        ' ''' </history>
        'Public Function UpdateServiceVisitSA(ByVal visitSeq As Long, _
        '                                     ByVal sysDate As DateTime, _
        '                                     ByVal saCode As String, _
        '                                     ByVal account As String, _
        '                                     ByVal system As String) As Integer
        '    ''引数をログに出力
        '    Dim args As New List(Of String)
        '    ''来店実績連番
        '    args.Add(String.Format(CultureInfo.CurrentCulture, "visitSeq = {0}", visitSeq))
        '    ''システム日時
        '    args.Add(String.Format(CultureInfo.CurrentCulture, "sysDate = {0}", sysDate))
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
        '    Dim sql As New StringBuilder
        '    sql.Append("UPDATE /* IC3810101_103 */")
        '    sql.Append("       TBL_SERVICE_VISIT_MANAGEMENT")
        '    sql.Append("   SET SACODE = :SACODE")
        '    sql.Append("     , ASSIGNTIMESTAMP = :ASSIGNTIMESTAMP")
        '    sql.Append("     , SERVICECODE = NVL(RTRIM(SERVICECODE), '20')")
        '    sql.Append("     , ASSIGNSTATUS = '2'")
        '    sql.Append("     , REGISTKIND = '1'")
        '    sql.Append("     , FREZID = REZID")
        '    sql.Append("     , UPDATEDATE = :UPDATEDATE")
        '    sql.Append("     , UPDATEACCOUNT = :UPDATEACCOUNT")
        '    sql.Append("     , UPDATEID = :UPDATEID")
        '    sql.Append(" WHERE VISITSEQ = :VISITSEQ")

        '    Using query As New DBUpdateQuery("IC3810101_103")
        '        query.CommandText = sql.ToString()
        '        query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, saCode)
        '        query.AddParameterWithTypeValue("ASSIGNTIMESTAMP", OracleDbType.Date, sysDate)
        '        query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, sysDate)
        '        query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, account)
        '        query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, system)
        '        query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, visitSeq)
        '        ''SQLの実行
        '        Dim ret As Integer = query.Execute()
        '        ''終了ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ret))
        '        Return ret
        '    End Using
        'End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ' ''' <summary>
        ' ''' 変換後車輌登録Noの取得
        ' ''' </summary>
        ' ''' <param name="dealerCD">販売店コード</param>
        ' ''' <param name="storeCD">店舗コード</param>
        ' ''' <param name="vehicleNo">車両登録No</param>
        ' ''' <returns>変換後車輌登録No</returns>
        ' ''' <remarks></remarks>
        ' ''' 
        ' ''' <history>
        ' ''' 2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発
        ' ''' </history>
        'Private Function ConvVehicleNo(ByVal dealerCD As String, _
        '                               ByVal storeCD As String, _
        '                               ByVal vehicleNo As String) As String
        '    '変換フォーマット、変換当て込み文字パスを取得
        '    Dim dlrEnvSet As New BranchEnvSetting
        '    Dim sysEnvChangeFormatRow As DlrEnvSettingDataSet.DLRENVSETTINGRow _
        '        = dlrEnvSet.GetEnvSetting(dealerCD, storeCD, VclRegNoChangeFormat)

        '    Dim sysEnvChangeStringRow As DlrEnvSettingDataSet.DLRENVSETTINGRow _
        '        = dlrEnvSet.GetEnvSetting(dealerCD, storeCD, VclRegNoChangeString)
        '    Dim changeFormat As String = String.Empty
        '    Dim changeString As String = String.Empty
        '    ' どちらか一方でも設定されていなければ、フォーマットによる変換は行わない。
        '    If sysEnvChangeFormatRow IsNot Nothing _
        '        AndAlso sysEnvChangeStringRow IsNot Nothing Then
        '        changeFormat = sysEnvChangeFormatRow.PARAMVALUE
        '        changeString = sysEnvChangeStringRow.PARAMVALUE
        '    End If
        '    Dim sb As New StringBuilder
        '    Dim formatIndex As Integer = 0
        '    Dim targetIndex As Integer = 0

        '    '変換フォーマットか対象の車輌Noの文字数を越えるまでループ
        '    While formatIndex < changeFormat.Length _
        '        AndAlso targetIndex < vehicleNo.Length

        '        '変換フォーマットと当て込み文字が一致していたら
        '        If String.Equals(changeFormat(formatIndex), changeString) Then
        '            '車輌登録Noの文字を当て込み
        '            sb.Append(vehicleNo(targetIndex))
        '            targetIndex += 1
        '            formatIndex += 1
        '        Else
        '            '変換フォーマットの文字を当て込み
        '            sb.Append(changeFormat(formatIndex))
        '            formatIndex += 1
        '        End If
        '    End While
        '    Dim returnValue As String = sb.ToString
        '    If String.IsNullOrEmpty(returnValue) = True Then
        '        Return vehicleNo
        '    Else
        '        Return returnValue
        '    End If
        'End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ' ''' <summary>
        ' ''' サービス来店者管理テーブルの取得
        ' ''' </summary>
        ' ''' <param name="dealerCD">販売店コード</param>
        ' ''' <param name="storeCD">店舗コード</param>
        ' ''' <param name="visitSeq">来店実績連番</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetVisitData(ByVal dealerCD As String _
        '                           , ByVal storeCD As String _
        '                           , ByVal visitSeq As Long) As IC3810101DataSet.IC3810101VisitKeyDataTable
        '    Try
        '        Dim result As Long = -1
        '        ''引数をログに出力
        '        Dim args As New List(Of String)
        '        ''販売店コード
        '        args.Add(String.Format(CultureInfo.CurrentCulture, "dealerCD = {0}", dealerCD))
        '        ''店舗コード
        '        args.Add(String.Format(CultureInfo.CurrentCulture, "storeCD = {0}", storeCD))
        '        ''来店実績連番
        '        args.Add(String.Format(CultureInfo.CurrentCulture, "visitSeq = {0}", visitSeq))
        '        ''開始ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} IN:{2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , String.Join(", ", args.ToArray())))

        '        Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101VisitKeyDataTable)("IC3810101_104")
        '            ''SQLの設定
        '            Dim sql As New StringBuilder
        '            sql.AppendLine("SELECT /* IC3810101_104 */")
        '            sql.AppendLine("       T1.REZID")
        '            sql.AppendLine("     , RTRIM(T1.ORDERNO) AS ORDERNO")
        '            sql.AppendLine("     , T1.SACODE")
        '            sql.AppendLine("     , NVL2(T2.REZID, T2.ACCOUNT_PLAN, T1.DEFAULTSACODE) AS DEFAULTSACODE")
        '            sql.AppendLine("  FROM TBL_SERVICE_VISIT_MANAGEMENT T1")
        '            sql.AppendLine("     , TBL_STALLREZINFO T2")
        '            sql.AppendLine(" WHERE T1.DLRCD = T2.DLRCD(+)")
        '            sql.AppendLine("   AND T1.STRCD = T2.STRCD(+)")
        '            sql.AppendLine("   AND T1.REZID = T2.REZID(+)")
        '            sql.AppendLine("   AND T1.VISITSEQ = :VISITSEQ")
        '            sql.AppendLine("   AND T1.DLRCD = :DLRCD")
        '            sql.AppendLine("   AND T1.STRCD = :STRCD")
        '            sql.AppendLine("   AND T1.REZID >= 0")

        '            query.CommandText = sql.ToString()
        '            ''パラメータの設定
        '            query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Decimal, visitSeq)
        '            query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCD)
        '            query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCD)
        '            ''SQLの実行
        '            Using dt As IC3810101DataSet.IC3810101VisitKeyDataTable = query.GetData()
        '                ''終了ログの出力
        '                Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '                    , Me.GetType.ToString _
        '                    , MethodBase.GetCurrentMethod.Name _
        '                    , dt.Rows.Count))
        '                Return dt
        '            End Using
        '        End Using
        '    Finally

        '    End Try
        'End Function
        ' ''2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        ''2012/08/01 TMEJ 瀧 【A.STEP2】SA ストール予約受付機能開発 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ''2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 START
        ' ''' <summary>
        ' ''' ストール予約情報更新処理
        ' ''' </summary>
        ' ''' <param name="rowIN">サービス来店者引数</param>
        ' ''' <param name="rowCK">来客者キー情報</param>
        ' ''' <param name="rowSK">ストール予約キー情報</param>
        ' ''' <returns>更新件数</returns>
        ' ''' <remarks></remarks>
        'Public Function UpdateStallReserveInfo(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow _
        '                                     , ByVal rowCK As IC3810101DataSet.IC3810101CustomerKeyRow _
        '                                     , ByVal rowSK As IC3810101DataSet.IC3810101StallKeyRow) As Integer
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                   , "{0}.{1} " _
        '                   , Me.GetType.ToString _
        '                   , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '    Dim sql As New StringBuilder
        '    sql.Append("UPDATE /* IC3810101_105 */ ")
        '    sql.Append("       TBL_STALLREZINFO ")
        '    sql.Append("   SET CUSTCD = :CUSTCD ")
        '    sql.Append("     , CUSTOMERNAME = :CUSTOMERNAME ")
        '    sql.Append("     , TELNO = :TELNO ")
        '    sql.Append("     , MOBILE = :MOBILE ")
        '    sql.Append("     , VIN = :VIN ")
        '    sql.Append("     , CUSTOMERFLAG = :CUSTOMERFLAG ")
        '    sql.Append("     , MODELCODE = :MODELCODE ")
        '    sql.Append("     , UPDATE_COUNT = UPDATE_COUNT + 1 ")
        '    sql.Append("     , UPDATEDATE = SYSDATE ")
        '    sql.Append("     , UPDATEACCOUNT = :UPDATEACCOUNT ")
        '    sql.Append(" WHERE DLRCD = :DLRCD ")
        '    sql.Append("   AND STRCD = :STRCD ")
        '    sql.Append("   AND REZID = :REZID ")

        '    Using query As New DBUpdateQuery("IC3810101_103")
        '        query.CommandText = sql.ToString()
        '        '顧客コード
        '        If rowIN.IsCUSTOMERCODENull Then
        '            query.AddParameterWithTypeValue("CUSTCD", OracleDbType.Char, DBNull.Value)
        '        Else
        '            query.AddParameterWithTypeValue("CUSTCD", OracleDbType.NVarchar2, rowCK.CUSTOMERCODE)
        '        End If
        '        '顧客名
        '        If rowCK.IsNAMENull Then
        '            query.AddParameterWithTypeValue("CUSTOMERNAME", OracleDbType.NVarchar2, DBNull.Value)
        '        Else
        '            query.AddParameterWithTypeValue("CUSTOMERNAME", OracleDbType.NVarchar2, rowCK.NAME)
        '        End If
        '        '電話番号
        '        If rowCK.IsTELNONull Then
        '            query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, DBNull.Value)
        '        Else
        '            query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, rowCK.TELNO)
        '        End If
        '        '携帯番号
        '        If rowCK.IsMOBILENull Then
        '            query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, DBNull.Value)
        '        Else
        '            query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, rowCK.MOBILE)
        '        End If
        '        'VIN
        '        If rowCK.IsVINNull Then
        '            query.AddParameterWithTypeValue("VIN", OracleDbType.Varchar2, DBNull.Value)
        '        Else
        '            query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, rowCK.VIN)
        '        End If
        '        '顧客区分
        '        If rowIN.IsCUSTSEGMENTNull Then
        '            query.AddParameterWithTypeValue("CUSTOMERFLAG", OracleDbType.Char, DBNull.Value)
        '        Else
        '            query.AddParameterWithTypeValue("CUSTOMERFLAG", OracleDbType.Char, StatusVisit)
        '        End If
        '        'モデルコード
        '        If rowCK.IsMODELCODENull Then
        '            query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, DBNull.Value)
        '        Else
        '            query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, rowCK.MODELCODE)
        '        End If
        '        '更新ユーザーアカウント
        '        query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, rowIN.ACCOUNT)
        '        '販売店コード
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, rowIN.DLRCD)
        '        '店舗コード
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, rowIN.STRCD)
        '        '予約ID
        '        query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, rowSK.REZID)
        '        ''SQLの実行
        '        Dim ret As Integer = query.Execute()

        '        ''終了ログの出力
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
        '            , Me.GetType.ToString _
        '            , MethodBase.GetCurrentMethod.Name _
        '            , ret))
        '        Return ret
        '    End Using
        'End Function
        ''2012/10/18 TMEJ 小澤 【SERVICE_2】問連「GTMC121018037」対応 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ''' <summary>
        ''' IC3810101_007:予約情報の取得
        ''' </summary>
        ''' <param name="rowIN">サービス来店者引数</param>
        ''' <returns>予約情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発
        ''' </history>
        Public Function GetReserveInfo(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow) _
                                              As IC3810101DataSet.IC3810101VisitRegistInfoDataTable

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

            Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101VisitRegistInfoDataTable)("IC3810101_007")

                Dim sql As New StringBuilder

                With sql

                    .AppendLine("   SELECT  /* IC3810101_007 */ ")
                    .AppendLine("           T1.SVCIN_ID AS REZID ")
                    .AppendLine("          ,TRIM(T1.RO_NUM) AS ORDERNO ")
                    .AppendLine("          ,TRIM(T1.PIC_SA_STF_CD) AS ACCOUNT_PLAN ")
                    .AppendLine("          ,T1.CST_ID AS CUSTCD ")
                    .AppendLine("          ,T1.VCL_ID AS VCL_ID ")
                    .AppendLine("          ,TRIM(T4.CST_NAME) AS CUSTOMERNAME ")
                    .AppendLine("          ,TRIM(T4.CST_PHONE) AS TELNO ")
                    .AppendLine("          ,TRIM(T4.CST_MOBILE) AS MOBILE ")
                    .AppendLine("          ,TRIM(T4.CST_GENDER) AS SEX ")
                    .AppendLine("          ,NVL(TRIM(T4.DMS_CST_CD), TRIM(T6.DMS_CST_CD)) AS DMSID ")
                    .AppendLine("          ,NVL2(TRIM(T6.DMS_CST_CD), :MYCUSTOMER, NVL(TRIM(T5.CST_TYPE), :NEWCUSTMOER)) AS CUSTOMERFLAG ")
                    .AppendLine("          ,NVL(TRIM(T7.VCL_VIN), TRIM(T6.VCL_VIN)) AS VIN ")
                    .AppendLine("          ,TRIM(T7.VCL_KATASHIKI) AS MODELCODE ")
                    ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                    .AppendLine("          ,TRIM(T4.CST_NAME) AS VISIT_CST_NAME ")
                    .AppendLine("          ,NVL(TRIM(T4.CST_PHONE), TRIM(T4.CST_MOBILE)) AS VISIT_CST_PHONE ")
                    ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                    .AppendLine("     FROM  TB_T_SERVICEIN T1 ")
                    .AppendLine("          ,TB_T_JOB_DTL T2 ")
                    .AppendLine("          ,TB_T_STALL_USE T3 ")
                    .AppendLine("          ,TB_M_CUSTOMER T4 ")
                    .AppendLine("          ,TB_M_CUSTOMER_DLR T5 ")
                    .AppendLine("          ,TBL_SERVICEIN_APPEND T6 ")
                    .AppendLine("          ,TB_M_VEHICLE T7 ")
                    .AppendLine("    WHERE  T1.SVCIN_ID = T2.SVCIN_ID ")
                    .AppendLine("      AND  T2.JOB_DTL_ID = T3.JOB_DTL_ID ")
                    .AppendLine("      AND  T1.CST_ID = T4.CST_ID(+) ")
                    .AppendLine("      AND  T1.DLR_CD = T5.DLR_CD(+) ")
                    .AppendLine("      AND  T1.CST_ID = T5.CST_ID(+) ")
                    .AppendLine("      AND  T1.CST_ID = T6.CST_ID(+) ")
                    .AppendLine("      AND  T1.VCL_ID = T6.VCL_ID(+) ")
                    .AppendLine("      AND  T1.VCL_ID = T7.VCL_ID(+) ")
                    .AppendLine("      AND  T1.DLR_CD = :DLR_CD ")
                    .AppendLine("      AND  T1.BRN_CD = :BRN_CD ")
                    .AppendLine("      AND  T1.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_0 ")
                    .AppendLine("      AND  T1.RESV_STATUS IN (:RESV_STATUS_0, :RESV_STATUS_1) ")
                    .AppendLine("      AND  T1.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01) ")
                    .AppendLine("      AND  T2.CANCEL_FLG = :CAMCEL_FLG_0 ")
                    .AppendLine("      AND  TRUNC(T3.SCHE_START_DATETIME) BETWEEN TRUNC(:STARTTIME) AND TRUNC(:STARTTIME + :DAYS) ")

                    '顧客コード確認
                    If 0 < rowIN.CUSTOMERCODE Then
                        '顧客コードが存在する場合

                        '条件に顧客コードを追加
                        .AppendLine("      AND  T1.CST_ID = :CST_ID ")

                        '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

                        'query.AddParameterWithTypeValue("CST_ID", OracleDbType.Int64, rowIN.CUSTOMERCODE)

                        query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, rowIN.CUSTOMERCODE)

                        '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

                    End If

                    '車両IDの確認
                    If 0 < rowIN.VCL_ID Then
                        '車両IDが存在する場合

                        '条件に車両IDを追加
                        .AppendLine("      AND  T1.VCL_ID = :VCL_ID ")

                        '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

                        'query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Int64, rowIN.VCL_ID)

                        query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, rowIN.VCL_ID)

                        '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

                    End If

                    .AppendLine(" ORDER BY  T3.SCHE_START_DATETIME ")

                End With

                'パラメータ
                query.AddParameterWithTypeValue("NEWCUSTMOER", OracleDbType.NVarchar2, CustSegmentNewCustomer)
                query.AddParameterWithTypeValue("MYCUSTOMER", OracleDbType.NVarchar2, CustSegmentMyCustomer)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, rowIN.DLRCD)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, rowIN.STRCD)
                query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_0", OracleDbType.NVarchar2, AcceptanceTypeRez)
                query.AddParameterWithTypeValue("RESV_STATUS_0", OracleDbType.NVarchar2, RezStatusTentative)
                query.AddParameterWithTypeValue("RESV_STATUS_1", OracleDbType.NVarchar2, RezStatus)
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, StatusNoIn)
                query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, StatusNoVisit)
                query.AddParameterWithTypeValue("CAMCEL_FLG_0", OracleDbType.NVarchar2, CancelFlagEffective)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, rowIN.VISITTIMESTAMP)
                query.AddParameterWithTypeValue("DAYS", OracleDbType.Long, Me.StallRangeDays(rowIN.DLRCD, rowIN.STRCD))

                ''SQLの実行
                query.CommandText = sql.ToString()

                Dim dt As IC3810101DataSet.IC3810101VisitRegistInfoDataTable = query.GetData()

                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))

                Return dt
            End Using
        End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START


        ''' <summary>
        ''' IC3810101_008:自社客予約情報の取得
        ''' (自社客で送信されて未取引客での予約がある場合の予約検索)
        ''' </summary>
        ''' <param name="rowIN">サービス来店者引数</param>
        ''' <returns>予約情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発
        ''' </history>
        Public Function GetMyCustReserveInfo(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow) _
                                             As IC3810101DataSet.IC3810101VisitRegistInfoDataTable

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

            Dim sql As New StringBuilder

            With sql

                .AppendLine("   SELECT  /* IC3810101_008 */ ")
                .AppendLine("           TRIM(T2.CST_NAME) AS CUSTOMERNAME ")
                .AppendLine("          ,TRIM(T2.CST_PHONE) AS TELNO ")
                .AppendLine("          ,TRIM(T2.CST_MOBILE) AS MOBILE ")
                .AppendLine("          ,TRIM(T2.CST_GENDER) AS SEX ")
                .AppendLine("          ,NVL(TRIM(T3.CST_TYPE), :NEWCUSTMOER) AS CUSTOMERFLAG ")
                .AppendLine("          ,T2.CST_ID AS CUSTCD ")
                .AppendLine("          ,T4.VCL_ID AS VCL_ID ")
                .AppendLine("          ,TRIM(T4.VCL_KATASHIKI) AS MODELCODE ")
                .AppendLine("          ,TRIM(T5.DMS_CST_CD) AS DMSID ")
                .AppendLine("          ,TRIM(T5.VCL_VIN) AS VIN ")
                .AppendLine("          ,T6.SVCIN_ID AS REZID ")
                .AppendLine("          ,TRIM(T6.RO_NUM) AS ORDERNO ")
                .AppendLine("          ,TRIM(T6.PIC_SA_STF_CD) AS ACCOUNT_PLAN ")
                ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                .AppendLine("          ,TRIM(T2.CST_NAME) AS VISIT_CST_NAME ")
                .AppendLine("          ,NVL(TRIM(T2.CST_PHONE), TRIM(T2.CST_MOBILE)) AS VISIT_CST_PHONE ")
                ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                .AppendLine("     FROM  TB_M_CUSTOMER_VCL T1 ")
                .AppendLine("          ,TB_M_CUSTOMER T2 ")
                .AppendLine("          ,TB_M_CUSTOMER_DLR T3")
                .AppendLine("          ,TB_M_VEHICLE T4")
                .AppendLine("          ,TBL_SERVICEIN_APPEND T5")
                .AppendLine("          ,TB_T_SERVICEIN T6 ")
                .AppendLine("          ,TB_T_JOB_DTL T7 ")
                .AppendLine("          ,TB_T_STALL_USE T8 ")
                .AppendLine("    WHERE  T1.CST_ID = T2.CST_ID ")
                .AppendLine("      AND  T1.DLR_CD = T3.DLR_CD")
                .AppendLine("      AND  T1.CST_ID = T3.CST_ID")
                .AppendLine("      AND  T1.VCL_ID = T4.VCL_ID ")
                .AppendLine("      AND  T2.DMS_CST_CD = T5.DMS_CST_CD")
                .AppendLine("      AND  T5.CST_ID = T6.CST_ID")
                .AppendLine("      AND  T5.VCL_ID = T6.VCL_ID")
                .AppendLine("      AND  T6.SVCIN_ID = T7.SVCIN_ID ")
                .AppendLine("      AND  T7.JOB_DTL_ID = T8.JOB_DTL_ID")
                .AppendLine("      AND  T1.DLR_CD = :DLR_CD ")

                '顧客コード確認
                If 0 < rowIN.CUSTOMERCODE Then
                    '顧客コードが存在する場合

                    '条件に顧客コードを追加
                    .AppendLine("      AND  T1.CST_ID = :CST_ID ")

                End If

                '車両IDの確認
                If 0 < rowIN.VCL_ID Then
                    '車両IDが存在する場合

                    '条件に車両IDを追加
                    .AppendLine("      AND  T1.VCL_ID = :VCL_ID ")

                End If

                .AppendLine("      AND  T1.CST_VCL_TYPE = :CST_VCL_TYPE")
                .AppendLine("      AND  T3.DLR_CD = :DLR_CD ")
                .AppendLine("      AND  T6.DLR_CD = :DLR_CD")
                .AppendLine("      AND  T6.BRN_CD = :BRN_CD")
                .AppendLine("      AND  T6.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_0 ")
                .AppendLine("      AND  T6.RESV_STATUS IN (:RESV_STATUS_0, :RESV_STATUS_1) ")
                .AppendLine("      AND  T6.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01) ")
                .AppendLine("      AND  T7.DLR_CD = :DLR_CD")
                .AppendLine("      AND  T7.BRN_CD = :BRN_CD")
                .AppendLine("      AND  T7.CANCEL_FLG = :CAMCEL_FLG_0 ")
                .AppendLine("      AND  T8.DLR_CD = :DLR_CD")
                .AppendLine("      AND  T8.BRN_CD = :BRN_CD")
                .AppendLine("      AND  TRUNC(T8.SCHE_START_DATETIME) BETWEEN TRUNC(:STARTTIME) AND TRUNC(:STARTTIME + :DAYS) ")
                .AppendLine(" ORDER BY  T8.SCHE_START_DATETIME ")

            End With

            Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101VisitRegistInfoDataTable)("IC3810101_008")

                'パラメータ

                '顧客コード確認
                If 0 < rowIN.CUSTOMERCODE Then
                    '顧客コードが存在する場合

                    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

                    'query.AddParameterWithTypeValue("CST_ID", OracleDbType.Int64, rowIN.CUSTOMERCODE)

                    query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, rowIN.CUSTOMERCODE)

                    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

                End If

                '車両IDの確認
                If 0 < rowIN.VCL_ID Then
                    '車両IDが存在する場合

                    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

                    'query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Int64, rowIN.VCL_ID)

                    query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, rowIN.VCL_ID)

                    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

                End If


                query.AddParameterWithTypeValue("NEWCUSTMOER", OracleDbType.NVarchar2, CustSegmentNewCustomer)
                query.AddParameterWithTypeValue("CST_VCL_TYPE", OracleDbType.NVarchar2, VehicleType)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, rowIN.DLRCD)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, rowIN.STRCD)
                query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_0", OracleDbType.NVarchar2, AcceptanceTypeRez)
                query.AddParameterWithTypeValue("RESV_STATUS_0", OracleDbType.NVarchar2, RezStatusTentative)
                query.AddParameterWithTypeValue("RESV_STATUS_1", OracleDbType.NVarchar2, RezStatus)
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, StatusNoIn)
                query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, StatusNoVisit)
                query.AddParameterWithTypeValue("CAMCEL_FLG_0", OracleDbType.NVarchar2, CancelFlagEffective)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, rowIN.VISITTIMESTAMP)
                query.AddParameterWithTypeValue("DAYS", OracleDbType.Long, Me.StallRangeDays(rowIN.DLRCD, rowIN.STRCD))

                ''SQLの実行
                query.CommandText = sql.ToString()

                Dim dt As IC3810101DataSet.IC3810101VisitRegistInfoDataTable = query.GetData()

                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))

                Return dt
            End Using
        End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START


        ''' <summary>
        ''' IC3810101_009:未取引客予約情報の取得
        ''' (未取引客で送信されて自社客での予約がある場合の予約検索)
        ''' </summary>
        ''' <param name="rowIN">サービス来店者引数</param>
        ''' <returns>予約情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetNewCustReserveInfo(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow) _
                                             As IC3810101DataSet.IC3810101VisitRegistInfoDataTable

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



            Dim sql As New StringBuilder

            With sql

                .AppendLine("   SELECT  /* IC3810101_009 */ ")
                .AppendLine("           T3.SVCIN_ID AS REZID ")
                .AppendLine("          ,TRIM(T3.RO_NUM) AS ORDERNO ")
                .AppendLine("          ,TRIM(T3.PIC_SA_STF_CD) AS ACCOUNT_PLAN ")
                .AppendLine("          ,T3.CST_ID AS CUSTCD ")
                .AppendLine("          ,T3.VCL_ID AS VCL_ID ")
                .AppendLine("          ,TRIM(T6.CST_NAME) AS CUSTOMERNAME ")
                .AppendLine("          ,TRIM(T6.CST_PHONE) AS TELNO ")
                .AppendLine("          ,TRIM(T6.CST_MOBILE) AS MOBILE ")
                .AppendLine("          ,TRIM(T6.CST_GENDER) AS SEX ")
                .AppendLine("          ,TRIM(T6.DMS_CST_CD) AS DMSID ")
                .AppendLine("          ,NVL(TRIM(T7.CST_TYPE), :NEWCUSTMOER) AS CUSTOMERFLAG ")
                .AppendLine("          ,TRIM(T8.VCL_VIN) AS VIN ")
                .AppendLine("          ,TRIM(T8.VCL_KATASHIKI) AS MODELCODE ")
                ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                .AppendLine("          ,TRIM(T6.CST_NAME) AS VISIT_CST_NAME ")
                .AppendLine("          ,NVL(TRIM(T6.CST_PHONE), TRIM(T6.CST_MOBILE)) AS VISIT_CST_PHONE ")
                ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                .AppendLine("     FROM  TB_M_VEHICLE_DLR T1 ")
                .AppendLine("          ,TB_M_CUSTOMER_VCL T2")
                .AppendLine("          ,TB_T_SERVICEIN T3")
                .AppendLine("          ,TB_T_JOB_DTL T4")
                .AppendLine("          ,TB_T_STALL_USE T5 ")
                .AppendLine("          ,TB_M_CUSTOMER T6")
                .AppendLine("          ,TB_M_CUSTOMER_DLR T7")
                .AppendLine("          ,TB_M_VEHICLE T8")
                .AppendLine("    WHERE  T1.DLR_CD = T2.DLR_CD ")
                .AppendLine("      AND  T1.VCL_ID = T2.VCL_ID")
                .AppendLine("      AND  T2.CST_ID = T3.CST_ID")
                .AppendLine("      AND  T2.VCL_ID = T3.VCL_ID")
                .AppendLine("      AND  T3.SVCIN_ID = T4.SVCIN_ID ")
                .AppendLine("      AND  T4.JOB_DTL_ID = T5.JOB_DTL_ID")
                .AppendLine("      AND  T3.CST_ID = T6.CST_ID(+)")
                .AppendLine("      AND  T6.CST_ID = T7.CST_ID(+)")
                .AppendLine("      AND  T3.VCL_ID = T8.VCL_ID(+)")
                .AppendLine("      AND  T1.REG_NUM = :REG_NUM")
                .AppendLine("      AND  T1.DLR_CD = :DLR_CD ")
                .AppendLine("      AND  T2.CST_VCL_TYPE = :CST_VCL_TYPE")
                .AppendLine("      AND  T3.DLR_CD = :DLR_CD ")
                .AppendLine("      AND  T3.BRN_CD = :BRN_CD")
                .AppendLine("      AND  T3.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_0 ")
                .AppendLine("      AND  T3.RESV_STATUS IN (:RESV_STATUS_0, :RESV_STATUS_1) ")
                .AppendLine("      AND  T3.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01) ")
                .AppendLine("      AND  T4.DLR_CD = :DLR_CD ")
                .AppendLine("      AND  T4.BRN_CD = :BRN_CD")
                .AppendLine("      AND  T4.CANCEL_FLG = :CAMCEL_FLG_0 ")
                .AppendLine("      AND  T5.DLR_CD = :DLR_CD ")
                .AppendLine("      AND  T5.BRN_CD = :BRN_CD")
                .AppendLine("      AND  TRUNC(T5.SCHE_START_DATETIME) BETWEEN TRUNC(:STARTTIME) AND TRUNC(:STARTTIME + :DAYS) ")
                .AppendLine("      AND  T7.DLR_CD(+) = :DLR_CD ")
                .AppendLine(" ORDER BY  T5.SCHE_START_DATETIME ")

            End With

            Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101VisitRegistInfoDataTable)("IC3810101_009")

                'パラメータ
                query.AddParameterWithTypeValue("NEWCUSTMOER", OracleDbType.NVarchar2, CustSegmentNewCustomer)
                query.AddParameterWithTypeValue("REG_NUM", OracleDbType.NVarchar2, rowIN.VCLREGNO)
                query.AddParameterWithTypeValue("CST_VCL_TYPE", OracleDbType.NVarchar2, VehicleType)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, rowIN.DLRCD)
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, rowIN.STRCD)
                query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_0", OracleDbType.NVarchar2, AcceptanceTypeRez)
                query.AddParameterWithTypeValue("RESV_STATUS_0", OracleDbType.NVarchar2, RezStatusTentative)
                query.AddParameterWithTypeValue("RESV_STATUS_1", OracleDbType.NVarchar2, RezStatus)
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, StatusNoIn)
                query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, StatusNoVisit)
                query.AddParameterWithTypeValue("CAMCEL_FLG_0", OracleDbType.NVarchar2, CancelFlagEffective)
                query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Date, rowIN.VISITTIMESTAMP)
                query.AddParameterWithTypeValue("DAYS", OracleDbType.Long, Me.StallRangeDays(rowIN.DLRCD, rowIN.STRCD))

                ''SQLの実行
                query.CommandText = sql.ToString()

                Dim dt As IC3810101DataSet.IC3810101VisitRegistInfoDataTable = query.GetData()

                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))

                Return dt
            End Using
        End Function


        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ''' <summary>
        ''' IC3810101_010:顧客情報の取得
        ''' </summary>
        ''' <param name="rowIN">サービス来店者引数</param>
        ''' <returns>顧客情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発
        ''' </history>
        Public Function GetCustomerInfo(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow) _
                                        As IC3810101DataSet.IC3810101CustomerInfoDataTable

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

            Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101CustomerInfoDataTable)("IC3810101_010")

                Dim sql As New StringBuilder

                With sql

                    .AppendLine("  SELECT  /* IC3810101_010 */ ")
                    .AppendLine("          T1.CST_ID AS CUSTCD ")
                    .AppendLine("         ,TRIM(T1.DMS_CST_CD) AS DMSID ")
                    .AppendLine("         ,TRIM(T1.CST_NAME) AS CUSTOMERNAME ")
                    .AppendLine("         ,TRIM(T1.CST_PHONE) AS TELNO ")
                    .AppendLine("         ,TRIM(T1.CST_MOBILE) AS MOBILE ")
                    .AppendLine("         ,TRIM(T1.CST_GENDER) AS SEX ")
                    .AppendLine("         ,NVL(TRIM(T2.CST_TYPE), :NEWCUSTMOER) AS CUSTOMERFLAG ")
                    .AppendLine("   FROM   TB_M_CUSTOMER T1 ")
                    .AppendLine("         ,TB_M_CUSTOMER_DLR T2 ")
                    .AppendLine("  WHERE   T1.CST_ID = T2.CST_ID ")
                    .AppendLine("    AND   T1.CST_ID = :CST_ID ")
                    .AppendLine("    AND   T2.DLR_CD = :DLR_CD ")

                End With

                'パラメータ
                query.AddParameterWithTypeValue("NEWCUSTMOER", OracleDbType.NVarchar2, CustSegmentNewCustomer)

                '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

                'query.AddParameterWithTypeValue("CST_ID", OracleDbType.Int64, rowIN.CUSTOMERCODE)

                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, rowIN.CUSTOMERCODE)

                '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, rowIN.DLRCD)

                ''SQLの実行
                query.CommandText = sql.ToString()

                Dim dt As IC3810101DataSet.IC3810101CustomerInfoDataTable = query.GetData()

                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))

                Return dt
            End Using
        End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ''' <summary>
        ''' IC3810101_011:車両情報の取得
        ''' </summary>
        ''' <param name="rowIN">サービス来店者引数</param>
        ''' <returns>車両情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発
        ''' </history>
        Public Function GetVehicleInfo(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow) _
                                       As IC3810101DataSet.IC3810101VehicleInfoDataTable

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

            Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101VehicleInfoDataTable)("IC3810101_011")

                Dim sql As New StringBuilder

                With sql

                    .AppendLine("  SELECT  /* IC3810101_011 */ ")
                    .AppendLine("          T1.VCL_ID ")
                    .AppendLine("         ,TRIM(T1.VCL_VIN) AS VIN ")
                    .AppendLine("         ,TRIM(T1.VCL_KATASHIKI) AS MODELCODE ")
                    .AppendLine("    FROM  TB_M_VEHICLE T1 ")
                    .AppendLine("   WHERE  T1.VCL_ID = :VCL_ID ")

                End With

                'パラメータ
                '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

                'query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Int64, rowIN.VCL_ID)

                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, rowIN.VCL_ID)

                '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

                ''SQLの実行
                query.CommandText = sql.ToString()

                Dim dt As IC3810101DataSet.IC3810101VehicleInfoDataTable = query.GetData()

                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))

                Return dt
            End Using
        End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ''' <summary>
        ''' IC3810101_012:サービス来店者登録(新規登録)
        ''' </summary>
        ''' <param name="rowIN">サービス来店者引数</param>
        ''' <param name="rowVI">来客者情報</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns>来店実績連番</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発
        ''' </history>
        Public Function InsertServiceVisit(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow _
                                         , ByVal rowVI As IC3810101DataSet.IC3810101VisitRegistInfoRow _
                                         , ByVal inNowDate As Date) As Long

            '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

            'Public Function InsertServiceVisit(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow _
            '                                 , ByVal rowVI As IC3810101DataSet.IC3810101VisitRegistInfoRow) As Long

            '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

            '引数をログに出力
            Dim args As New List(Of String)

            'DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            Me.AddLogData(args, rowVI)

            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} IN:{2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , String.Join(", ", args.ToArray())))

            '来店実績連番
            Dim visitseq As Long = 0

            Using query As New DBSelectQuery(Of DataTable)("IC3810101_012")

                ''SQLの設定
                Dim sqlNextVal As New StringBuilder

                With sqlNextVal

                    .AppendLine("SELECT /* IC3810101_012 */")
                    .AppendLine("       SEQ_SERVICE_VISIT_MANAGEMENT.NEXTVAL AS VISITSEQ")
                    .AppendLine("  FROM DUAL")

                End With

                'SQL格納
                query.CommandText = sqlNextVal.ToString()

                Using dt As DataTable = query.GetData()

                    '来店実績連番取得
                    visitseq = CType(dt.Rows(0)("VISITSEQ"), Long)

                End Using
            End Using

            'SQLの設定
            Dim sql As New StringBuilder

            With sql

                sql.AppendLine(" INSERT  /* IC3810101_106 */ ")
                sql.AppendLine("   INTO  TBL_SERVICE_VISIT_MANAGEMENT(  ")
                sql.AppendLine("         VISITSEQ ")
                sql.AppendLine("        ,DLRCD ")
                sql.AppendLine("        ,STRCD ")
                sql.AppendLine("        ,VISITTIMESTAMP ")
                sql.AppendLine("        ,VCLREGNO ")
                sql.AppendLine("        ,CUSTSEGMENT ")
                sql.AppendLine("        ,CUSTID ")
                sql.AppendLine("        ,STAFFCD ")
                sql.AppendLine("        ,VISITPERSONNUM ")
                sql.AppendLine("        ,VISITMEANS ")
                sql.AppendLine("        ,VIN ")
                sql.AppendLine("        ,DMSID ")
                sql.AppendLine("        ,MODELCODE ")
                sql.AppendLine("        ,TELNO ")
                sql.AppendLine("        ,MOBILE ")
                sql.AppendLine("        ,SEQNO ")
                sql.AppendLine("        ,SEX ")
                sql.AppendLine("        ,NAME ")
                sql.AppendLine("        ,DEFAULTSACODE ")
                sql.AppendLine("        ,SACODE ")
                sql.AppendLine("        ,ASSIGNTIMESTAMP ")
                sql.AppendLine("        ,REZID ")
                sql.AppendLine("        ,PARKINGCODE ")
                sql.AppendLine("        ,VIPMARK ")
                sql.AppendLine("        ,ASSIGNSTATUS ")
                sql.AppendLine("        ,QUEUESTATUS ")
                sql.AppendLine("        ,HOLDSTAFF ")
                sql.AppendLine("        ,ORDERNO ")
                sql.AppendLine("        ,FREZID ")
                sql.AppendLine("        ,REGISTKIND ")
                sql.AppendLine("        ,CREATEDATE ")
                sql.AppendLine("        ,UPDATEDATE ")
                sql.AppendLine("        ,CREATEACCOUNT ")
                sql.AppendLine("        ,UPDATEACCOUNT ")
                sql.AppendLine("        ,CREATEID ")
                sql.AppendLine("        ,UPDATEID ")
                sql.AppendLine("        ,VCL_ID ")
                ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                sql.AppendLine("        ,VISITNAME ")
                sql.AppendLine("        ,VISITTELNO ")
                ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                sql.AppendLine("         )  ")
                sql.AppendLine(" VALUES (  ")
                sql.AppendLine("        :VISITSEQ ")
                sql.AppendLine("        ,:DLRCD ")
                sql.AppendLine("        ,:STRCD ")
                sql.AppendLine("        ,:VISITTIMESTAMP ")
                sql.AppendLine("        ,:VCLREGNO ")
                sql.AppendLine("        ,:CUSTSEGMENT ")
                sql.AppendLine("        ,:CUSTID ")
                sql.AppendLine("        ,:STAFFCD ")
                sql.AppendLine("        ,:VISITPERSONNUM ")
                sql.AppendLine("        ,:VISITMEANS ")
                sql.AppendLine("        ,:VIN ")
                sql.AppendLine("        ,:DMSID ")
                sql.AppendLine("        ,:MODELCODE ")
                sql.AppendLine("        ,:TELNO ")
                sql.AppendLine("        ,:MOBILE ")
                sql.AppendLine("        ,NULL ")
                sql.AppendLine("        ,:SEX ")
                sql.AppendLine("        ,:NAME ")
                sql.AppendLine("        ,:DEFAULTSACODE ")
                sql.AppendLine("        ,NULL ")
                sql.AppendLine("        ,NULL ")
                sql.AppendLine("        ,:REZID ")
                sql.AppendLine("        ,NULL ")
                sql.AppendLine("        ,N'0' ")
                sql.AppendLine("        ,:ASSIGNSTATUS ")
                sql.AppendLine("        ,N'0' ")
                sql.AppendLine("        ,NULL ")
                sql.AppendLine("        ,:ORDERNO ")
                sql.AppendLine("        ,:FREZID ")
                sql.AppendLine("        ,:REGISTKIND ")
                sql.AppendLine("        ,:NOWDATE ")
                sql.AppendLine("        ,:NOWDATE ")
                sql.AppendLine("        ,:ACCOUNT ")
                sql.AppendLine("        ,:ACCOUNT ")
                sql.AppendLine("        ,:SYSTEM ")
                sql.AppendLine("        ,:SYSTEM ")
                sql.AppendLine("        ,:VCL_ID ")
                ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                sql.AppendLine("        ,:VISITNAME ")
                sql.AppendLine("        ,:VISITTELNO ")
                ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
                sql.AppendLine("         ) ")

            End With

            Using query As New DBUpdateQuery("IC3810101_106")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, visitseq)     '■来店実績連番
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, rowIN.DLRCD) '■販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, rowIN.STRCD) '■店舗コード

                If rowIN.IsVISITTIMESTAMPNull Then  '■来店日時
                    query.AddParameterWithTypeValue("VISITTIMESTAMP", OracleDbType.Date, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("VISITTIMESTAMP", OracleDbType.Date, rowIN.VISITTIMESTAMP)
                End If

                If rowIN.IsVCLREGNONull Then        '■車両登録番号
                    query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, rowIN.VCLREGNO)
                End If

                If rowVI.IsCUSTOMERFLAGNull Then    '■顧客種別
                    query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.NVarchar2, rowVI.CUSTOMERFLAG)
                End If

                If rowVI.CUSTCD <= 0 Then          '■顧客コード
                    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

                    'query.AddParameterWithTypeValue("CUSTID", OracleDbType.Int64, DBNull.Value)

                    query.AddParameterWithTypeValue("CUSTID", OracleDbType.Decimal, DBNull.Value)

                    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END
                Else
                    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

                    'query.AddParameterWithTypeValue("CUSTID", OracleDbType.Int64, rowVI.CUSTCD)

                    query.AddParameterWithTypeValue("CUSTID", OracleDbType.Decimal, rowVI.CUSTCD)

                    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END
                End If

                If rowIN.IsSTAFFCDNull Then         '■スタッフコード
                    query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, rowIN.STAFFCD)
                End If

                If rowIN.VISITPERSONNUM <= 0 Then   '■来店人数
                    query.AddParameterWithTypeValue("VISITPERSONNUM", OracleDbType.Int64, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("VISITPERSONNUM", OracleDbType.Int64, rowIN.VISITPERSONNUM)
                End If

                If rowIN.IsVISITMEANSNull Then      '■来店手段
                    query.AddParameterWithTypeValue("VISITMEANS", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("VISITMEANS", OracleDbType.NVarchar2, rowIN.VISITMEANS)
                End If

                If rowVI.IsVINNull Then             '■VIN
                    query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, rowVI.VIN)
                End If

                If rowVI.IsDMSIDNull Then           '■基幹顧客コード
                    query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, rowVI.DMSID)
                End If

                If rowVI.IsMODELCODENull Then       '■モデルコード
                    query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, rowVI.MODELCODE)
                End If

                If rowVI.IsTELNONull Then           '■電話番号
                    query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, rowVI.TELNO)
                End If

                If rowVI.IsMOBILENull Then          '■携帯番号
                    query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, rowVI.MOBILE)
                End If

                If rowVI.IsSEXNull Then             '■性別
                    query.AddParameterWithTypeValue("SEX", OracleDbType.NVarchar2, Male)
                Else
                    query.AddParameterWithTypeValue("SEX", OracleDbType.NVarchar2, rowVI.SEX)
                End If

                If rowVI.IsCUSTOMERNAMENull Then    '■氏名
                    query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, rowVI.CUSTOMERNAME)
                End If

                If rowVI.IsACCOUNT_PLANNull Then    '■担当SAコード
                    If rowIN.IsDEFAULTSACODENull Then
                        query.AddParameterWithTypeValue("DEFAULTSACODE", OracleDbType.NVarchar2, DBNull.Value)
                    Else
                        query.AddParameterWithTypeValue("DEFAULTSACODE", OracleDbType.NVarchar2, rowIN.DEFAULTSACODE)
                    End If
                Else
                    query.AddParameterWithTypeValue("DEFAULTSACODE", OracleDbType.NVarchar2, rowVI.ACCOUNT_PLAN)
                End If

                query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, rowVI.REZID)         '■予約ID(サービス入庫ID)

                query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, NonAssign)  '■振当てステータス

                If rowVI.IsORDERNONull Then         '■整備受注No
                    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("ORDERNO", OracleDbType.NVarchar2, rowVI.ORDERNO)
                End If

                query.AddParameterWithTypeValue("FREZID", OracleDbType.Decimal, rowVI.REZID)        '■初回予約ID

                query.AddParameterWithTypeValue("REGISTKIND", OracleDbType.NVarchar2, RegistGK)     '■登録区分

                '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

                'query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, DateTimeFunc.Now(rowIN.DLRCD)) '■作成・更新日時
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)            '■作成・更新日時

                '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

                query.AddParameterWithTypeValue("ACCOUNT", OracleDbType.NVarchar2, rowIN.ACCOUNT)   '■アカウント

                query.AddParameterWithTypeValue("SYSTEM", OracleDbType.NVarchar2, rowIN.SYSTEM)     '■更新ID

                If rowVI.VCL_ID <= 0 Then           '■車両ID
                    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

                    'query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Int64, DBNull.Value)

                    query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, DBNull.Value)

                    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END
                Else
                    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

                    'query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Int64, rowVI.VCL_ID)

                    query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, rowVI.VCL_ID)

                    '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END
                End If

                
                ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                If rowVI.IsVISIT_CST_NAMENull Then
                    query.AddParameterWithTypeValue("VISITNAME", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("VISITNAME", OracleDbType.NVarchar2, rowVI.VISIT_CST_NAME)
                End If
                
                If rowVI.IsVISIT_CST_PHONENull Then
                    query.AddParameterWithTypeValue("VISITTELNO", OracleDbType.NVarchar2, DBNull.Value)
                Else
                    query.AddParameterWithTypeValue("VISITTELNO", OracleDbType.NVarchar2, rowVI.VISIT_CST_PHONE)
                End If
                ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                'SQLの実行
                query.Execute()

            End Using

            '終了ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} OUT:VISITSEQ = {2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , visitseq))

            Return visitseq

        End Function

        '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 START

        ''' <summary>
        ''' IC3810101_013:来店件数情報取得
        ''' </summary>
        ''' <param name="rowIN">サービス来店者引数</param>
        ''' <param name="rowVI">来客者情報</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns>来店件数情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetVisitCountInfo(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow, _
                                          ByVal rowVI As IC3810101DataSet.IC3810101VisitRegistInfoRow, _
                                          ByVal inNowDate As Date) As IC3810101DataSet.IC3810101VisitInfoCountDataTable

            '引数をログに出力
            Dim args As New List(Of String)

            'DataRow内の項目を列挙
            Me.AddLogData(args, rowIN)
            Me.AddLogData(args, rowVI)

            ''開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} IN:{2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , String.Join(", ", args.ToArray())))

            Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101VisitInfoCountDataTable)("IC3810101_013")

                Dim sql As New StringBuilder

                With sql
                    .AppendLine("SELECT /* IC3810101_013 */ ")
                    .AppendLine("       COUNT(1) AS VISITINFO_COUNT ")
                    .AppendLine("  FROM ")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                    .AppendLine("      ,TB_T_SERVICEIN T2 ")
                    .AppendLine("      ,TB_T_RO_INFO T3 ")
                    .AppendLine(" WHERE ")
                    .AppendLine("       T1.DLRCD = T2.DLR_CD(+) ")
                    .AppendLine("   AND T1.STRCD = T2.BRN_CD(+) ")
                    .AppendLine("   AND T1.FREZID = T2.SVCIN_ID(+) ")
                    .AppendLine("   AND T1.DLRCD = T3.DLR_CD(+) ")
                    .AppendLine("   AND T1.STRCD = T3.BRN_CD(+) ")
                    .AppendLine("   AND T1.ORDERNO = T3.RO_NUM(+) ")
                    .AppendLine("   AND T1.DLRCD = :DLRCD ")
                    .AppendLine("   AND T1.STRCD = :STRCD ")
                    .AppendLine("   AND T1.ASSIGNSTATUS <> :ASSIGNSTATUS_4 ")

                    'VINのチェック
                    If (Not (rowVI.IsVINNull) AndAlso Not (String.IsNullOrEmpty(rowVI.VIN))) OrElse _
                       (Not (rowIN.IsVINNull) AndAlso Not (String.IsNullOrEmpty(rowIN.VIN))) Then
                        'サービス来店者引数のVIN又は、来客者情報のVINが存在する場合
                        '車両番号とVINを条件に設定
                        .AppendLine("   AND (RTRIM(T1.VCLREGNO) = :VCLREGNO ")
                        .AppendLine("        OR RTRIM(T1.VIN) = :VIN) ")

                    Else
                        '上記以外の場合
                        '車両登録番号のみ条件に設定
                        .AppendLine("   AND RTRIM(T1.VCLREGNO) = :VCLREGNO ")

                    End If

                    .AppendLine("   AND (T2.SVCIN_ID IS NULL ")
                    .AppendLine("        OR T2.SVC_STATUS <> :SVC_STATUS_13)")
                    .AppendLine("   AND (T3.RO_NUM IS NULL ")
                    .AppendLine("        OR (T3.RO_STATUS <> :RO_STATUS_90 ")
                    .AppendLine("        AND T3.RO_STATUS <> :RO_STATUS_99)) ")
                    .AppendLine("   AND T1.VISITTIMESTAMP >= TRUNC(:NOWDATE) ")

                End With

                'パラメータ
                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, rowIN.DLRCD)

                '店舗コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, rowIN.STRCD)

                '振当ステータス「4：退店」
                query.AddParameterWithTypeValue("ASSIGNSTATUS_4", OracleDbType.NVarchar2, DealerOut)

                '車両番号
                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, rowIN.VCLREGNO)

                'VINのチェック
                If Not (rowVI.IsVINNull) AndAlso Not (String.IsNullOrEmpty(rowVI.VIN)) Then
                    '来店者情報にVINが存在する場合
                    'VIN
                    query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, rowVI.VIN)

                ElseIf Not (rowIN.IsVINNull) AndAlso Not (String.IsNullOrEmpty(rowIN.VIN)) Then
                    'サービス来店者引数にVIN存在する場合
                    'VIN
                    query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, rowIN.VIN)

                End If

                'サービスステータス「13：納車済み」
                query.AddParameterWithTypeValue("SVC_STATUS_13", OracleDbType.NVarchar2, StatusDelivery)

                'ROステータス「90：納車済み」
                query.AddParameterWithTypeValue("RO_STATUS_90", OracleDbType.NVarchar2, ROStatusDelivery)

                'ROステータス「99：R/Oキャンセル」
                query.AddParameterWithTypeValue("RO_STATUS_99", OracleDbType.NVarchar2, ROStatusCancel)

                '現在日時
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)

                'SQLの実行
                query.CommandText = sql.ToString()
                Dim dt As IC3810101DataSet.IC3810101VisitInfoCountDataTable = query.GetData()

                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))

                Return dt

            End Using

        End Function

        '2015/03/03 TMEJ 小澤 DMS連携版タブレット 来店情報異常時 チップ表示制御追加開発 END

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

        '2014/01/23 TMEJ 陳　 TMEJ次世代サービス 工程管理機能開発 START

#Region "通知送信用情報取得"

        ''' <summary>
        ''' IC3810101_201:通知送信用情報取得
        ''' </summary>
        ''' <param name="inVisitSeq">来店実績連番</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inBranchCode">店舗コード</param>
        ''' <returns>通知送信用情報データセット</returns>
        ''' <remarks></remarks>
        Public Function GetNoticeProcessingInfo(ByVal inVisitSeq As Long _
                                              , ByVal inDealerCode As String _
                                              , ByVal inBranchCode As String) _
                                                As IC3810101DataSet.IC3810101NoticeProcessingInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                      , "{0}.{1} VISITSEQ:{2}" _
                      , Me.GetType.ToString _
                      , System.Reflection.MethodBase.GetCurrentMethod.Name _
                      , inVisitSeq))

            Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101NoticeProcessingInfoDataTable)("IC3810101_037")

                'SQL文格納
                Dim sql As New StringBuilder

                With sql

                    .AppendLine("		SELECT /* IC3810101_201 */ ")
                    .AppendLine("		       T1.VISITSEQ ")
                    .AppendLine("		      ,TRIM(T1.VCLREGNO) AS VCLREGNO ")
                    .AppendLine("		      ,TRIM(T1.CUSTSEGMENT) AS CUSTSEGMENT ")
                    .AppendLine("		      ,TRIM(T1.DMSID) AS DMSID ")
                    .AppendLine("		      ,TRIM(T1.VIN) AS VIN ")

                    '2015/12/17 TM 浅野 12月号口配信に向けた緊急対応 START
                    .AppendLine("		      ,TRIM(T8.CST_NAME) AS NAME ")
                    '.AppendLine("		      ,TRIM(T1.NAME) AS NAME ")
                    '2015/12/17 TM 浅野 12月号口配信に向けた緊急対応 END

                    .AppendLine("		      ,NVL(T1.FREZID, -1) AS REZID ")
                    .AppendLine("		      ,CASE ")
                    .AppendLine("		            WHEN T5.SCHE_START_DATETIME = :MINDATE THEN :MINVALUE ")
                    .AppendLine("		            ELSE T5.SCHE_START_DATETIME ")
                    .AppendLine("		             END AS SCHE_START_DATETIME ")
                    .AppendLine("		      ,CASE ")
                    .AppendLine("		            WHEN T5.SCHE_END_DATETIME = :MINDATE THEN :MINVALUE ")
                    .AppendLine("		            ELSE T5.SCHE_END_DATETIME ")
                    .AppendLine("		             END AS SCHE_END_DATETIME ")
                    .AppendLine("		      ,NVL(CONCAT(TRIM(T6.UPPER_DISP), TRIM(T6.LOWER_DISP)), NVL(T7.SVC_CLASS_NAME, T7.SVC_CLASS_NAME_ENG)) AS MERCHANDISENAME ")
                    .AppendLine("		      ,TRIM(T9.NAMETITLE_NAME) AS NAMETITLE_NAME ")
                    .AppendLine("		      ,TRIM(T9.POSITION_TYPE) AS POSITION_TYPE ")
                    .AppendLine("		 FROM  TBL_SERVICE_VISIT_MANAGEMENT T1 ")
                    .AppendLine("		      ,TB_T_SERVICEIN T2 ")
                    .AppendLine("		      ,(SELECT MAX(S3.SVCIN_ID) AS SVCIN_ID ")
                    .AppendLine("		              ,MIN(S3.JOB_DTL_ID) AS JOB_DTL_ID ")
                    .AppendLine("		              ,MAX(S4.STALL_USE_ID) AS STALL_USE_ID ")
                    .AppendLine("		          FROM TBL_SERVICE_VISIT_MANAGEMENT S1 ")
                    .AppendLine("		              ,TB_T_SERVICEIN S2 ")
                    .AppendLine("		              ,TB_T_JOB_DTL S3 ")
                    .AppendLine("		              ,TB_T_STALL_USE S4 ")
                    .AppendLine("		         WHERE S1.FREZID = S2.SVCIN_ID ")
                    .AppendLine("		           AND S2.SVCIN_ID = S3.SVCIN_ID ")
                    .AppendLine("		           AND S3.JOB_DTL_ID = S4.JOB_DTL_ID ")
                    .AppendLine("		           AND S1.VISITSEQ = :VISITSEQ ")
                    .AppendLine("		           AND S1.DLRCD = :DLRCD ")
                    .AppendLine("		           AND S1.STRCD = :STRCD ")
                    .AppendLine("		           AND S2.DLR_CD = :DLRCD ")
                    .AppendLine("		           AND S2.BRN_CD = :STRCD ")
                    .AppendLine("		           AND S2.SVC_STATUS <> :STATUS_CANCEL ")
                    .AppendLine("		           AND S3.DLR_CD = :DLRCD ")
                    .AppendLine("		           AND S3.BRN_CD = :STRCD ")
                    .AppendLine("		           AND S3.CANCEL_FLG = :CANCELFLG ")
                    .AppendLine("		           AND S4.DLR_CD = :DLRCD ")
                    .AppendLine("		           AND S4.BRN_CD = :STRCD ")
                    .AppendLine("		      GROUP BY S1.VISITSEQ ")
                    .AppendLine("		       ) T3 ")
                    .AppendLine("		      ,TB_T_JOB_DTL T4 ")
                    .AppendLine("		      ,TB_T_STALL_USE T5 ")
                    .AppendLine("		      ,TB_M_MERCHANDISE T6 ")
                    .AppendLine("		      ,TB_M_SERVICE_CLASS T7 ")
                    .AppendLine("		      ,TB_M_CUSTOMER T8 ")
                    .AppendLine("		      ,TB_M_NAMETITLE T9 ")
                    .AppendLine("		WHERE  T1.FREZID = T2.SVCIN_ID(+) ")
                    .AppendLine("		  AND  T2.SVCIN_ID = T3.SVCIN_ID(+) ")
                    .AppendLine("		  AND  T3.JOB_DTL_ID = T4.JOB_DTL_ID(+) ")
                    .AppendLine("		  AND  T4.JOB_DTL_ID = T5.JOB_DTL_ID(+) ")
                    .AppendLine("		  AND  T4.MERC_ID = T6.MERC_ID(+) ")
                    .AppendLine("		  AND  T4.SVC_CLASS_ID = T7.SVC_CLASS_ID(+) ")
                    .AppendLine("		  AND  T1.CUSTID = T8.CST_ID(+) ")
                    .AppendLine("		  AND  T8.NAMETITLE_CD = T9.NAMETITLE_CD(+) ")
                    .AppendLine("		  AND  T1.VISITSEQ = :VISITSEQ ")
                    .AppendLine("		  AND  T1.DLRCD = :DLRCD ")
                    .AppendLine("		  AND  T1.STRCD = :STRCD ")
                    .AppendLine("		  AND  T2.DLR_CD(+) = :DLRCD ")
                    .AppendLine("		  AND  T2.BRN_CD(+) = :STRCD ")
                    .AppendLine("		  AND  T2.SVC_STATUS(+) <> :STATUS_CANCEL ")
                    .AppendLine("		  AND  T4.DLR_CD(+) = :DLRCD ")
                    .AppendLine("		  AND  T4.BRN_CD(+) = :STRCD ")
                    .AppendLine("		  AND  T4.CANCEL_FLG(+) = :CANCELFLG ")
                    .AppendLine("		  AND  T5.DLR_CD(+) = :DLRCD ")
                    .AppendLine("		  AND  T5.BRN_CD(+) = :STRCD ")
                    .AppendLine("		  AND  T9.INUSE_FLG(+) = :INUSE_FLG ")

                End With

                'SQL設定
                query.CommandText = sql.ToString()

                'バインド変数

                '日付省略値
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
                '日付最小値
                query.AddParameterWithTypeValue("MINVALUE", OracleDbType.Date, Date.MinValue)
                '来店実績連番
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSeq)
                '販売店コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                '店舗コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inBranchCode)
                'サービスステータス
                query.AddParameterWithTypeValue("STATUS_CANCEL", OracleDbType.NVarchar2, StatusCancel_02)
                'キャンセルフラグ
                query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.NVarchar2, CancelFlagEffective)
                '使用中フラグ("1"：使用中)
                query.AddParameterWithTypeValue("INUSE_FLG", OracleDbType.NVarchar2, InUse_1)

                '実行
                Dim dt As IC3810101DataSet.IC3810101NoticeProcessingInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                          , "{0}.{1} END COUNT = {2}" _
                          , Me.GetType.ToString _
                          , System.Reflection.MethodBase.GetCurrentMethod.Name _
                          , dt.Count))

                Return dt

            End Using

        End Function

#End Region

        '2014/01/23 TMEJ 陳　 TMEJ次世代サービス 工程管理機能開発 END

        ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START

        #Region "全ユーザー予約情報取得"
        
        ''' <summary>
        ''' 全ユーザー予約情報取得
        ''' </summary>
        ''' <param name="rowIN">来店情報</param>
        ''' <returns>全ユーザー予約情報</returns>
        ''' <remarks></remarks>
        Public Function GetAllUserReserveInfo(ByVal rowIN As IC3810101DataSet.IC3810101inServiceVisitRow) _
            As IC3810101DataSet.IC3810101VisitRegistInfoDataTable
                        
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
            
            Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101VisitRegistInfoDataTable)("IC3810101_013")

                Dim sql As New StringBuilder
                
                With sql
                    .AppendLine("SELECT /* IC3810101_013 */  ")
                    .AppendLine("       T3.SVCIN_ID AS REZID ")
                    .AppendLine("     , TRIM(T3.RO_NUM) AS ORDERNO ")
                    .AppendLine("     , TRIM(T3.PIC_SA_STF_CD) AS ACCOUNT_PLAN ")
                    .AppendLine("     , T3.CST_ID AS CUSTCD ")
                    .AppendLine("     , T3.VCL_ID AS VCL_ID ")
                    .AppendLine("     , TRIM(T2.CST_VCL_TYPE) AS CST_VCL_TYPE ")
                    .AppendLine("     , TRIM(T6.CST_NAME) AS CUSTOMERNAME ")
                    .AppendLine("     , TRIM(T6.CST_PHONE) AS TELNO ")
                    .AppendLine("     , TRIM(T6.CST_MOBILE) AS MOBILE ")
                    .AppendLine("     , TRIM(T6.CST_GENDER) AS SEX ")
                    .AppendLine("     , TRIM(T6.DMS_CST_CD) AS DMSID ")
                    .AppendLine("     , TRIM(T6.CST_NAME) AS VISIT_CST_NAME ")
                    .AppendLine("     , NVL(TRIM(T6.CST_PHONE), TRIM(T6.CST_MOBILE)) AS VISIT_CST_PHONE ")
                    .AppendLine("     , NVL(TRIM(T7.CST_TYPE), :NEWCUSTMOER) AS CUSTOMERFLAG ")
                    .AppendLine("     , TRIM(T8.VCL_VIN) AS VIN ")
                    .AppendLine("     , TRIM(T8.VCL_KATASHIKI) AS MODELCODE ")
                    .AppendLine("  FROM TB_M_VEHICLE_DLR T1 ")
                    .AppendLine("     , TB_M_CUSTOMER_VCL T2 ")
                    .AppendLine("     , TB_T_SERVICEIN T3 ")
                    .AppendLine("     , TB_T_JOB_DTL T4 ")
                    .AppendLine("     , TB_T_STALL_USE T5 ")
                    .AppendLine("     , TB_M_CUSTOMER T6 ")
                    .AppendLine("     , TB_M_CUSTOMER_DLR T7 ")
                    .AppendLine("     , TB_M_VEHICLE T8 ")
                    .AppendLine(" WHERE T1.DLR_CD = T2.DLR_CD ")
                    .AppendLine("   AND T1.VCL_ID = T2.VCL_ID ")
                    .AppendLine("   AND T2.CST_ID = T3.CST_ID ")
                    .AppendLine("   AND T2.VCL_ID = T3.VCL_ID ")
                    .AppendLine("   AND T3.SVCIN_ID = T4.SVCIN_ID ")
                    .AppendLine("   AND T4.JOB_DTL_ID = T5.JOB_DTL_ID ")
                    .AppendLine("   AND T3.CST_ID = T6.CST_ID ")
                    .AppendLine("   AND T6.CST_ID = T7.CST_ID ")
                    .AppendLine("   AND T3.VCL_ID = T8.VCL_ID ")
                    .AppendLine("   AND T1.REG_NUM_SEARCH = UPPER(:REG_NUM) ")
                    .AppendLine("   AND T1.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND T2.CST_VCL_TYPE <> :CST_VCL_TYPE_INS ")
                    .AppendLine("   AND T2.OWNER_CHG_FLG = :OWNER_CHG_FLG_NOT ")
                    .AppendLine("   AND T3.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND T3.BRN_CD = :BRN_CD ")
                    .AppendLine("   AND T3.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE_REZ ")
                    .AppendLine("   AND T3.RESV_STATUS IN (:RESV_STATUS_TEN, :RESV_STATUS) ")
                    .AppendLine("   AND T3.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01) ")
                    .AppendLine("   AND T4.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND T4.BRN_CD = :BRN_CD ")
                    .AppendLine("   AND T4.CANCEL_FLG = :CANCEL_FLG ")
                    .AppendLine("   AND T5.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND T5.BRN_CD = :BRN_CD ")
                    .AppendLine("   AND TRUNC(T5.SCHE_START_DATETIME) BETWEEN TRUNC(:VISIT_DATE) AND TRUNC(:VISIT_DATE + :BOOK_BY_DATES) ")
                    .AppendLine("   AND T7.DLR_CD = :DLR_CD ")
                    .AppendLine("  ORDER BY ")
                    .AppendLine("       T5.SCHE_START_DATETIME ASC ")
                    .AppendLine("     , CUSTOMERFLAG ASC ")
                    .AppendLine("     , CST_VCL_TYPE ASC ")

                End With
                
                ' パラメータ
                ' 販売店コード
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, rowIN.DLRCD)

                ' 店舗コード
                query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, rowIN.STRCD)

                ' 車両番号
                query.AddParameterWithTypeValue("REG_NUM", OracleDbType.NVarchar2, rowIN.VCLREGNO)
                
                ' 来店日時
                query.AddParameterWithTypeValue("VISIT_DATE", OracleDbType.Date, rowIN.VISITTIMESTAMP)

                ' 予約有効日数
                query.AddParameterWithTypeValue("BOOK_BY_DATES", OracleDbType.Long, Me.StallRangeDays(rowIN.DLRCD, rowIN.STRCD))
                                
                ' 予約ステータス(仮予約)
                query.AddParameterWithTypeValue("RESV_STATUS_TEN", OracleDbType.NVarchar2, RezStatusTentative)

                ' 予約ステータス(本予約)
                query.AddParameterWithTypeValue("RESV_STATUS", OracleDbType.NVarchar2, RezStatus)

                ' サービスステータス(未入庫)
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, StatusNoIn)

                ' サービスステータス(未来店)
                query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, StatusNoVisit)

                ' キャンセルフラグ(有効)
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)

                ' 受付区分(予約客)
                query.AddParameterWithTypeValue("ACCEPTANCE_TYPE_REZ", OracleDbType.NVarchar2, AcceptanceTypeRez)

                ' 顧客車両区分(保険)
                query.AddParameterWithTypeValue("CST_VCL_TYPE_INS", OracleDbType.NVarchar2, VehicleTypeInsurance)

                ' オーナーチェンジフラグ(未設定)
                query.AddParameterWithTypeValue("OWNER_CHG_FLG_NOT", OracleDbType.NVarchar2, OwnerChangeFlagNot)
                
                ' 未取引客
                query.AddParameterWithTypeValue("NEWCUSTMOER", OracleDbType.NVarchar2, CustSegmentNewCustomer)

                'SQLの実行
                query.CommandText = sql.ToString()
                Dim dt As IC3810101DataSet.IC3810101VisitRegistInfoDataTable = query.GetData()

                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))
                
                Return dt
            End Using
        End Function

        #End Region

        #Region "オーナー顧客情報取得"

        ''' <summary>
        ''' オーナー顧客情報取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="vehicleId">車両ID</param>
        ''' <returns>オーナー顧客情報</returns>
        ''' <remarks></remarks>
        Public Function GetOwnerCustomerInfo(ByVal dealerCode As String, ByVal vehicleId As Decimal) _
            As IC3810101DataSet.IC3810101OwnerCustomerInfoDataTable
            
            ' 開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} dealerCode:{2} vehicleId:{3}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , dealerCode, vehicleId))

            Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101OwnerCustomerInfoDataTable)("IC3810101_014")
                
                Dim sql As New StringBuilder
                
                With sql
                    .AppendLine("SELECT /* IC3810101_014 */  ")
                    .AppendLine("       T1.CST_ID ")
                    .AppendLine("     , TRIM(T1.CST_VCL_TYPE) AS CST_VCL_TYPE ")
                    .AppendLine("     , TRIM(T2.CST_NAME) AS CST_NAME ")
                    .AppendLine("     , TRIM(T2.CST_PHONE) AS CST_PHONE ")
                    .AppendLine("     , TRIM(T2.CST_MOBILE) AS CST_MOBILE ")
                    .AppendLine("     , TRIM(T2.CST_GENDER) AS CST_GENDER ")
                    .AppendLine("     , TRIM(T2.DMS_CST_CD) AS DMS_CST_CD ")
                    .AppendLine("     , NVL(TRIM(T3.CST_TYPE), :NEWCUSTMOER) AS CST_TYPE ")
                    .AppendLine("     , TRIM(T4.VCL_VIN) AS VCL_VIN ")
                    .AppendLine("     , TRIM(T4.VCL_KATASHIKI) AS VCL_KATASHIKI ")
                    .AppendLine("  FROM TB_M_CUSTOMER_VCL T1 ")
                    .AppendLine("     , TB_M_CUSTOMER T2 ")
                    .AppendLine("     , TB_M_CUSTOMER_DLR T3 ")
                    .AppendLine("     , TB_M_VEHICLE T4 ")
                    .AppendLine(" WHERE T1.CST_ID = T2.CST_ID ")
                    .AppendLine("   AND T2.CST_ID = T3.CST_ID ")
                    .AppendLine("   AND T1.VCL_ID = T4.VCL_ID ")
                    .AppendLine("   AND T1.VCL_ID = :VCL_ID ")
                    .AppendLine("   AND T1.DLR_CD = :DLR_CD ")
                    .AppendLine("   AND T1.CST_VCL_TYPE = :CST_VCL_TYPE ")
                    .AppendLine("   AND T1.OWNER_CHG_FLG = :OWNER_CHG_FLG ")
                    .AppendLine("   AND T3.DLR_CD = :DLR_CD ")
                    .AppendLine("  ORDER BY ")
                    .AppendLine("       T3.DMS_TAKEIN_DATETIME DESC ")
                    .AppendLine("     , CST_TYPE ASC ")
                    .AppendLine("     , CST_VCL_TYPE ASC ")
                End With
                
                ' パラメータ
                ' 販売店コード
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                
                ' 車両ID
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, vehicleId)
                
                ' 顧客車両区分
                query.AddParameterWithTypeValue("CST_VCL_TYPE", OracleDbType.NVarchar2, VehicleType)

                ' オーナーチェンジフラグ(未設定)
                query.AddParameterWithTypeValue("OWNER_CHG_FLG", OracleDbType.NVarchar2, OwnerChangeFlagNot)
                
                ' 未取引客
                query.AddParameterWithTypeValue("NEWCUSTMOER", OracleDbType.NVarchar2, CustSegmentNewCustomer)

                'SQLの実行
                query.CommandText = sql.ToString()
                Dim dt As IC3810101DataSet.IC3810101OwnerCustomerInfoDataTable = query.GetData()

                '終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))

                Return dt
            End Using

        End Function

        #End Region

        ' 2015/09/07 TMEJ 浅野 (トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
        '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 START

#Region "サービス来店者管理退避"

        ''' <summary>
        ''' サービス来店者管理退避
        ''' </summary>
        ''' <param name="visitSeqList">退避させる来店実績連番のリスト</param>
        ''' <returns>退避件数</returns>
        ''' <remarks></remarks>
        Public Function InsertServiceVisitMngPast(ByVal visitSeqList As List(Of Decimal)) _
            As Integer

            '開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            'SQL格納用
            Dim sqlVisitSeqId As New StringBuilder

            'カウンター
            Dim i As Integer = 1
            Dim j As Integer = 1

            For Each visitSeq As Decimal In visitSeqList

                'IN句の1000行制限の制御
                If j = 1000 Then

                    sqlVisitSeqId.Append(")")
                    sqlVisitSeqId.Append(" OR VISITSEQ IN ( ")

                    j = 1
                End If

                '整備受注NOと枝番
                sqlVisitSeqId.Append(visitSeq)

                If Not j = 999 AndAlso Not visitSeqList.Count = i Then
                    sqlVisitSeqId.Append(",")
                End If
                i = i + 1
                j = j + 1
            Next

            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3810101_017")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("INSERT /* IC3810101_017 */ ")
                    .AppendLine("  INTO TBL_SERVICE_VISIT_MNG_PAST ( ")
                    .AppendLine("       VISITSEQ ")
                    .AppendLine("     , DLRCD ")
                    .AppendLine("     , STRCD ")
                    .AppendLine("     , VISITTIMESTAMP ")
                    .AppendLine("     , VCLREGNO ")
                    .AppendLine("     , CUSTSEGMENT ")
                    .AppendLine("     , CUSTID ")
                    .AppendLine("     , DMSID ")
                    .AppendLine("     , STAFFCD ")
                    .AppendLine("     , VISITPERSONNUM ")
                    .AppendLine("     , VISITMEANS ")
                    .AppendLine("     , VIN ")
                    .AppendLine("     , MODELCODE ")
                    .AppendLine("     , SEQNO ")
                    .AppendLine("     , SEX ")
                    .AppendLine("     , NAME ")
                    .AppendLine("     , TELNO ")
                    .AppendLine("     , MOBILE ")
                    .AppendLine("     , DEFAULTSACODE ")
                    .AppendLine("     , SACODE ")
                    .AppendLine("     , ASSIGNTIMESTAMP ")
                    .AppendLine("     , SERVICECODE ")
                    .AppendLine("     , REZID ")
                    .AppendLine("     , PARKINGCODE ")
                    .AppendLine("     , VIPMARK ")
                    .AppendLine("     , ASSIGNSTATUS ")
                    .AppendLine("     , QUEUESTATUS ")
                    .AppendLine("     , HOLDSTAFF ")
                    .AppendLine("     , ORDERNO ")
                    .AppendLine("     , FREZID ")
                    .AppendLine("     , REGISTKIND ")
                    .AppendLine("     , CREATEDATE ")
                    .AppendLine("     , UPDATEDATE ")
                    .AppendLine("     , CREATEACCOUNT ")
                    .AppendLine("     , UPDATEACCOUNT ")
                    .AppendLine("     , CREATEID ")
                    .AppendLine("     , UPDATEID ")
                    .AppendLine("     , CALLNO ")
                    .AppendLine("     , CALLPLACE ")
                    .AppendLine("     , CALLSTARTDATE ")
                    .AppendLine("     , CALLENDDATE ")
                    .AppendLine("     , CALLSTATUS ")
                    .AppendLine("     , VISITNAME ")
                    .AppendLine("     , VISITTELNO ")
                    .AppendLine("     , VCL_ID ")
                    .AppendLine("     , SVC_CLASS_TYPE ")
                    .AppendLine(") ")
                    .AppendLine("SELECT ")
                    .AppendLine("       VISITSEQ ")
                    .AppendLine("     , DLRCD ")
                    .AppendLine("     , STRCD ")
                    .AppendLine("     , VISITTIMESTAMP ")
                    .AppendLine("     , VCLREGNO ")
                    .AppendLine("     , CUSTSEGMENT ")
                    .AppendLine("     , CUSTID ")
                    .AppendLine("     , DMSID ")
                    .AppendLine("     , STAFFCD ")
                    .AppendLine("     , VISITPERSONNUM ")
                    .AppendLine("     , VISITMEANS ")
                    .AppendLine("     , VIN ")
                    .AppendLine("     , MODELCODE ")
                    .AppendLine("     , SEQNO ")
                    .AppendLine("     , SEX ")
                    .AppendLine("     , NAME ")
                    .AppendLine("     , TELNO ")
                    .AppendLine("     , MOBILE ")
                    .AppendLine("     , DEFAULTSACODE ")
                    .AppendLine("     , SACODE ")
                    .AppendLine("     , ASSIGNTIMESTAMP ")
                    .AppendLine("     , SERVICECODE ")
                    .AppendLine("     , REZID ")
                    .AppendLine("     , PARKINGCODE ")
                    .AppendLine("     , VIPMARK ")
                    .AppendLine("     , ASSIGNSTATUS ")
                    .AppendLine("     , QUEUESTATUS ")
                    .AppendLine("     , HOLDSTAFF ")
                    .AppendLine("     , ORDERNO ")
                    .AppendLine("     , FREZID ")
                    .AppendLine("     , REGISTKIND ")
                    .AppendLine("     , CREATEDATE ")
                    .AppendLine("     , UPDATEDATE ")
                    .AppendLine("     , CREATEACCOUNT ")
                    .AppendLine("     , UPDATEACCOUNT ")
                    .AppendLine("     , CREATEID ")
                    .AppendLine("     , UPDATEID ")
                    .AppendLine("     , CALLNO ")
                    .AppendLine("     , CALLPLACE ")
                    .AppendLine("     , CALLSTARTDATE ")
                    .AppendLine("     , CALLENDDATE ")
                    .AppendLine("     , CALLSTATUS ")
                    .AppendLine("     , VISITNAME ")
                    .AppendLine("     , VISITTELNO ")
                    .AppendLine("     , VCL_ID ")
                    .AppendLine("     , SVC_CLASS_TYPE ")
                    .AppendLine("  FROM TBL_SERVICE_VISIT_MANAGEMENT ")
                    .AppendLine(" WHERE VISITSEQ IN ( ")
                    .AppendLine(sqlVisitSeqId.ToString)
                    .AppendLine(") ")
                End With

                'SQL設定
                query.CommandText = sql.ToString()

                ' SQL実行
                Dim insertCount As Integer = query.Execute()

                '終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:insertCount = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , insertCount))

                Return insertCount
            End Using
        End Function
#End Region

#Region "サービス来店者管理削除"

        ''' <summary>
        ''' サービス来店者管理削除
        ''' </summary>
        ''' <param name="visitSeqList">削除させる来店実績連番のリスト</param>
        ''' <returns>削除件数</returns>
        ''' <remarks></remarks>
        Public Function DeleteServiceVisitMng(ByVal visitSeqList As List(Of Decimal)) _
            As Integer

            '開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))

            'SQL格納用
            Dim sqlVisitSeqId As New StringBuilder

            'カウンター
            Dim i As Integer = 1
            Dim j As Integer = 1

            For Each visitSeq As Decimal In visitSeqList

                'IN句の1000行制限の制御
                If j = 1000 Then

                    sqlVisitSeqId.Append(")")
                    sqlVisitSeqId.Append(" OR VISITSEQ IN ( ")

                    j = 1
                End If

                '整備受注NOと枝番
                sqlVisitSeqId.Append(visitSeq)

                If Not j = 999 AndAlso Not visitSeqList.Count = i Then
                    sqlVisitSeqId.Append(",")
                End If
                i = i + 1
                j = j + 1
            Next

            'DbUpdateQueryインスタンス生成
            Using query As New DBUpdateQuery("IC3810101_018")

                'SQL組み立て
                Dim sql As New StringBuilder
                With sql
                    .AppendLine("DELETE /* IC3810101_018 */ ")
                    .AppendLine("  FROM TBL_SERVICE_VISIT_MANAGEMENT ")
                    .AppendLine(" WHERE VISITSEQ IN ( ")
                    .AppendLine(sqlVisitSeqId.ToString)
                    .AppendLine(") ")
                End With

                'SQL設定
                query.CommandText = sql.ToString()

                ' SQL実行
                Dim deleteCount As Integer = query.Execute()

                '終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:deleteCount = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , deleteCount))

                Return deleteCount
            End Using
        End Function
#End Region

#Region "サービス入庫IDから予約情報の取得"

        ''' <summary>
        ''' サービス入庫IDから予約情報予約情報の取得
        ''' </summary>
        ''' <param name="serviceinId">来店させる予約のサービス入庫ID</param>
        ''' <returns>サービス入庫IDに紐付く予約情報</returns>
        ''' <remarks></remarks>
        Public Function GetReserveInfoBySvcinId(ByVal serviceinId As Decimal) _
            As IC3810101DataSet.IC3810101VisitRegistInfoDataTable

            '開始ログの出力
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} IN:{2}" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , serviceinId))

            Using query As New DBSelectQuery(Of IC3810101DataSet.IC3810101VisitRegistInfoDataTable)("IC3810101_019")

                Dim sql As New StringBuilder

                With sql
                    .AppendLine("SELECT /* IC3810101_019 */ ")
                    .AppendLine("       T1.SVCIN_ID AS REZID ")
                    .AppendLine("     , TRIM(T1.RO_NUM) AS ORDERNO ")
                    .AppendLine("     , TRIM(T1.PIC_SA_STF_CD) AS ACCOUNT_PLAN ")
                    .AppendLine("     , T1.CST_ID AS CUSTCD ")
                    .AppendLine("     , T1.VCL_ID AS VCL_ID ")
                    .AppendLine("     , TRIM(T2.CST_NAME) AS CUSTOMERNAME ")
                    .AppendLine("     , TRIM(T2.CST_PHONE) AS TELNO ")
                    .AppendLine("     , TRIM(T2.CST_MOBILE) AS MOBILE ")
                    .AppendLine("     , TRIM(T2.CST_GENDER) AS SEX ")
                    .AppendLine("     , NVL(TRIM(T2.DMS_CST_CD),TRIM(T4.DMS_CST_CD)) AS DMSID ")
                    .AppendLine("     , TRIM(T2.CST_NAME) AS VISIT_CST_NAME ")
                    .AppendLine("     , NVL(TRIM(T2.CST_PHONE),TRIM(T2.CST_MOBILE)) AS VISIT_CST_PHONE ")
                    .AppendLine("     , NVL(TRIM(T3.VCL_VIN), TRIM(T4.VCL_VIN)) AS VIN ")
                    .AppendLine("     , TRIM(T3.VCL_KATASHIKI) AS MODELCODE ")
                    .AppendLine("     , NVL2(TRIM(T4.DMS_CST_CD), :MYCUSTOMER, NVL(TRIM(T5.CST_TYPE), :NEWCUSTOMER)) AS CUSTOMERFLAG ")
                    .AppendLine("  FROM ")
                    .AppendLine("       TB_T_SERVICEIN T1 ")
                    .AppendLine("     , TB_M_CUSTOMER T2 ")
                    .AppendLine("     , TB_M_VEHICLE T3 ")
                    .AppendLine("     , TBL_SERVICEIN_APPEND T4  ")
                    .AppendLine("     , TB_M_CUSTOMER_DLR T5 ")
                    .AppendLine(" WHERE ")
                    .AppendLine("       T1.CST_ID = T2.CST_ID (+) ")
                    .AppendLine("   AND T1.DLR_CD = T5.DLR_CD (+) ")
                    .AppendLine("   AND T1.CST_ID = T5.CST_ID (+) ")
                    .AppendLine("   AND T1.CST_ID = T4.CST_ID (+) ")
                    .AppendLine("   AND T1.VCL_ID = T4.VCL_ID (+) ")
                    .AppendLine("   AND T1.VCL_ID = T3.VCL_ID (+) ")
                    .AppendLine("   AND T1.SVCIN_ID = :SVCIN_ID ")
                End With

                ' パラメータ
                ' サービス入庫ID
                query.AddParameterWithTypeValue("SVCIN_ID", OracleDbType.Decimal, serviceinId)

                ' 自社客
                query.AddParameterWithTypeValue("MYCUSTOMER", OracleDbType.NVarchar2, CustSegmentMyCustomer)

                ' 未取引客
                query.AddParameterWithTypeValue("NEWCUSTOMER", OracleDbType.NVarchar2, CustSegmentNewCustomer)

                'SQLの実行
                query.CommandText = sql.ToString()
                Dim dt As IC3810101DataSet.IC3810101VisitRegistInfoDataTable = query.GetData()

                ''終了ログの出力
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} OUT:ROWSCOUNT = {2}" _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name _
                            , dt.Rows.Count))

                Return dt

            End Using

        End Function

#End Region

        '2018/02/19 NSK 河谷 REQ-SVT-TMT-20170615-001_iPod Gatekeeperに顧客の車両登録番号を入力する機能を追加 END

    End Class

End Namespace

