
'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3060102DataSet.vb
'─────────────────────────────────────
'機能： 試乗入力データアクセス
'補足： 
'作成： 
'更新： 2013/05/27 TMEJ m.asano 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 $01
'更新： 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.TrialRide.TrialRidePreparation.DataAccess
Imports Toyota.eCRB.Visit.Api.BizLogic

Imports System.Web
Imports System.Text
Imports Oracle.DataAccess.Client
Imports System.Globalization

''' <summary>
''' SC3110101 試乗入力画面 ビジネスロジック層
''' </summary>
''' <remarks></remarks>
Public Class SC3110101BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3110101BusinessLogic

#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const AppId As String = "SC3110101"

    ''' <summary>
    ''' シリーズ名、またはカラー名が空欄文字だった場合
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NoName As String = "-"

    ''' <summary>
    ''' オラクルエラー：タイムアウト
    ''' </summary>
    ''' <remarks>試乗車ステータスの更新時のエラー</remarks>
    Private Const OracleException2049 As Integer = 2049


#Region "メッセージID"

    ''' <summary>
    ''' 正常値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const MessageIdSuccess As Integer = 0

    ''' <summary>
    ''' 他の人が更新している
    ''' </summary>
    ''' <remarks>試乗車ステータスの更新時のエラー</remarks>
    Private Const MessageIdInsertFailed As Integer = 901

    ''' <summary>
    ''' 登録失敗データが存在する
    ''' </summary>
    ''' <remarks>試乗車ステータスの更新時のエラー</remarks>
    Private Const MessageIdDataBaseFailed As Integer = 902

#End Region

#End Region

#Region "初期表示データの取得"

    ''' <summary>
    ''' 初期表示データの取得を行う
    ''' </summary>
    ''' <param name="dealerCode"></param>
    ''' <param name="storeCode"></param>
    ''' <returns>結果返却</returns>
    ''' <remarks></remarks>
    Public Function GetInitialDisplay(ByVal dealerCode As String, ByVal storeCode As String) As SC3110101DataSet.SC3110101TestDriveCarInfoDataTable

        Dim getInitialDisplayStartLog As New StringBuilder
        With getInitialDisplayStartLog
            .Append("GetInitialDisplay_Start ")
            .Append("param1[" & dealerCode & "]")
            .Append(",param2[" & storeCode & "]")
        End With
        Logger.Info(getInitialDisplayStartLog.ToString)

        '結果格納用テーブル
        Dim retTable As New SC3110101DataSet.SC3110101TestDriveCarInfoDataTable

        '現在日付の取得
        Logger.Info("GetInitialDisplay_001 Call_Start DateTimeFunc.Now param[" & dealerCode & "]")
        Dim visitTimeStamp As Date = DateTimeFunc.Now(dealerCode)
        Logger.Info("GetInitialDisplay_001 Call_End DateTimeFunc.Now Ret[" & visitTimeStamp & "]")

        Using adapter As New SC3110101DataSetTableAdapters.SC3110101TableAdapter

            '試乗車の取得時に使用するテーブル
            Using carStatusTable As SC3110101DataSet.SC3110101CarStatusInfoDataTable = adapter.GetTestDriveCar(dealerCode, storeCode, visitTimeStamp)

                If carStatusTable Is Nothing Then

                    '試乗車データが存在しない
                    Logger.Info("GetInitialDisplay_002 NotTestDriveCarStatus")
                    Return Nothing
                End If

                Logger.Info("GetInitialDisplay_003 TestDriveCarStatus")

                'モデル、ロゴの値を取得
                For Each targetRow As SC3110101DataSet.SC3110101CarStatusInfoRow In carStatusTable

                    'モデルロゴ
                    Dim modelLogo As String = String.Empty

                    'モデルロゴ用データテーブルから情報を取得
                    Using modelLogoTable As SC3110101DataSet.SC3110101ModelLogoInfoDataTable = adapter.GetModelLogo(targetRow.DLRCD, targetRow.VCLSERIESCD)

                        '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                        If modelLogoTable Is Nothing OrElse modelLogoTable.Count = 0 _
                           OrElse String.IsNullOrEmpty(Trim(modelLogoTable.Item(0).LOGO_NOTSELECTED)) Then

                            Logger.Info("GetInitialDisplay_004 modelLogoTable Is Nothing")
                        Else

                            'モデルロゴ
                            Logger.Info("GetInitialDisplay_005 modelLogoTable IsNot Nothing")
                            modelLogo = modelLogoTable.Item(0).LOGO_NOTSELECTED
                        End If
                        '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                    End Using

                    'モデル画像
                    Dim modelPicture As String = String.Empty

                    'モデル画像用データテーブルから情報を取得
                    Using modelPictureTable As SC3110101DataSet.SC3110101ModelPictureInfoDataTable = _
                        adapter.GetModelImageFile(targetRow.DLRCD, targetRow.VCLSERIESCD, targetRow.VCLMODELCD, targetRow.BODYCLRCD)

                        '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                        If modelPictureTable Is Nothing OrElse modelPictureTable.Count = 0 _
                           OrElse String.IsNullOrEmpty(Trim(modelPictureTable.Item(0).IMAGEFILE)) Then

                            Logger.Info("GetInitialDisplay_006 modelPictureTable Is Nothing")
                        Else

                            Logger.Info("GetInitialDisplay_007 modelPictureTable IsNot Nothing")
                            modelPicture = modelPictureTable.Item(0).IMAGEFILE
                        End If
                        '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END
                    End Using

                    '必要情報をセット
                    Dim retRow As SC3110101DataSet.SC3110101TestDriveCarInfoRow = _
                        SetTestDriveCarInfo(retTable.NewSC3110101TestDriveCarInfoRow, targetRow, modelPicture, modelLogo)

                    'データの追加
                    retTable.Rows.Add(retRow)

                    '解放処理
                    retRow = Nothing
                Next

                '解放処理
            End Using
        End Using

        '結果返却
        Dim getInitialDisplayEndLog As New StringBuilder
        With getInitialDisplayEndLog
            .Append("GetInitialDisplay_End Ret:[")
            .Append(retTable)
            .Append("] ")
        End With
        Logger.Info(getInitialDisplayEndLog.ToString)

        Return retTable
    End Function

#End Region

#Region "取得データのセット"

    ''' <summary>
    ''' 取得データのセット
    ''' </summary>
    ''' <param name="retRow">情報を格納するためのデータロウ</param>
    ''' <param name="dataRow">取得対象のデータロウ</param>
    ''' <param name="picture">画像パス</param>
    ''' <param name="logo">ロゴパス</param>
    ''' <returns>格納した情報</returns>
    ''' <remarks></remarks>
    Private Function SetTestDriveCarInfo(ByVal retRow As SC3110101DataSet.SC3110101TestDriveCarInfoRow, _
                                         ByVal dataRow As SC3110101DataSet.SC3110101CarStatusInfoRow, _
                                         ByVal picture As String, _
                                         ByVal logo As String) As SC3110101DataSet.SC3110101TestDriveCarInfoRow

        Dim nameUtility As New VisitUtility

        'データを結果テーブルに格納
        retRow.TESTDRIVECARID = dataRow.TESTDRIVECARID             '試乗車ID
        retRow.TESTDRIVECARSTATUS = dataRow.TESTDRIVECARSTATUS     '試乗車ステータス

        'シリーズ名
        If String.IsNullOrEmpty(Trim(dataRow.TESTDRIVECARNAME)) Then

            'trimした結果が空欄なら"-"
            retRow.TESTDRIVECARNAME = NoName
        Else

            retRow.TESTDRIVECARNAME = dataRow.TESTDRIVECARNAME
        End If

        retRow.LOGO_NOTSELECTED = logo                                                         'モデルロゴ
        retRow.IMAGEFILE = picture                                                             'モデル画像

        '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
        'グレード名
        If String.IsNullOrEmpty(Trim(dataRow.GRADENAME)) Then
            'trimした結果が空欄なら"-"
            retRow.GRADENAME = NoName
        Else
            retRow.GRADENAME = dataRow.GRADENAME
        End If
        '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

        'カラー名
        If String.IsNullOrEmpty(Trim(dataRow.CORTCOLOR)) Then

            'trimした結果が空欄なら"-"
            retRow.CORTCOLOR = NoName
        Else

            retRow.CORTCOLOR = dataRow.CORTCOLOR
        End If

        retRow.UPDATEDATE = dataRow.UPDATEDATE                                                    '更新日

        Return retRow
    End Function
#End Region

#Region "試乗車ステータスの更新"

    ''' <summary>
    ''' 試乗車ステータスの更新/挿入を行う
    ''' </summary>
    ''' <param name="updateCarStatus">更新対象データ</param>
    ''' <param name="account">更新者(アカウント)</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
    ''' </history>
    <EnableCommit()> _
    Public Function UpdateTestDriveCarStatus(ByVal updateCarStatus As SC3110101DataSet.SC3110101InsertTestDriveCarStatusDataTable, ByVal account As String) As Integer Implements ISC3110101BusinessLogic.UpdateTestDriveCarStatus

        Dim updateTestDriveCarStatusStartLog As New StringBuilder
        With updateTestDriveCarStatusStartLog
            .Append("UpdateTestDriveCarStatus_Start ")
            .Append("param1[")
            .Append(updateCarStatus)
            .Append("],param2[" & account & "]")
        End With
        Logger.Info(updateTestDriveCarStatusStartLog.ToString)

        'DataTableに情報がない場合
        If updateCarStatus Is Nothing Then

            Logger.Info("UpdateTestDriveCarStatus_001 updateCarStatus Is Nothing")
            Logger.Info("UpdateTestDriveCarStatus_End Ret[" + CStr(MessageIdInsertFailed) + "]")
            Return MessageIdInsertFailed
        End If

        Logger.Info("UpdateTestDriveCarStatus_002 updateCarStatus IsNot Nothing")

        Try

            Using adapter As New SC3110101DataSetTableAdapters.SC3110101TableAdapter
                For Each dat As SC3110101DataSet.SC3110101InsertTestDriveCarStatusRow In updateCarStatus

                    'チェック用変数
                    Dim dataExist As Boolean = False

                    '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 START
                    '試乗車ステータス情報の存在チェック
                    dataExist = adapter.ExistsTestDriveCar(dat.DLRCD, CDec(dat.TESTDRIVECARID))
                    '$01 次世代e-CRB新車タブレット 新DB適応に向けた機能開発 END

                    If dataExist Then

                        '新規挿入で他の更新が入っていた場合
                        If String.IsNullOrEmpty(dat.UPDATEDATE) Then

                            Logger.Error("UpdateTestDriveCarStatus_003 Empty UpdateDate")
                            Me.Rollback = True

                            Logger.Error("UpdateTestDriveCarStatus_End")
                            Return MessageIdInsertFailed
                        End If

                        Logger.Info("UpdateTestDriveCarStatus_004 ExistsTestDriveCar")

                        '試乗車ステータスの更新
                        dataExist = adapter.UpdateTestDriveCar(dat, account, AppId)

                        '更新できたかチェック
                        If dataExist = False Then

                            Logger.Error("UpdateTestDriveCarStatus_005 UpdateFalse")
                            Me.Rollback = True

                            Logger.Error("UpdateTestDriveCarStatus_End")
                            Return MessageIdInsertFailed
                        End If
                    Else

                        Logger.Info("UpdateTestDriveCarStatus_006 NotExistsTestDriveCar")

                        '試乗車ステータスの挿入
                        dataExist = adapter.InsertTestDriveCar(dat, account, AppId)

                        '挿入できたかチェック
                        If dataExist = False Then

                            Logger.Error("UpdateTestDriveCarStatus_007 InsertFalse")
                            Me.Rollback = True

                            Logger.Error("UpdateTestDriveCarStatus_End")
                            Return MessageIdInsertFailed
                        End If
                    End If
                Next
            End Using
        Catch ex As OracleExceptionEx

            ' データベースの操作中に例外が発生した場合
            Logger.Error("UpdateTestDriveCarStatus_008 " & "Catch OracleExceptionEx")
            Logger.Error("ErrorID:" & CStr(ex.Number) & "Exception:" & ex.Message)

            If ex.Number = OracleException2049 Then

                'DBタイムアウトエラー時
                Logger.Error("UpdateTestDriveCarStatus_009 " & "ex.Number = MessageId_OracleException2049")
                Logger.Error("UpdateTestDriveCarStatus ResultId:" & CStr(OracleException2049))
                Logger.Error("UpdateTestDriveCarStatus_End Ret[ " + MessageIdDataBaseFailed.ToString(CultureInfo.InvariantCulture()) + " ]")
                Return MessageIdDataBaseFailed
            Else

                '上記以外のエラーは基盤側で制御
                Logger.Error("UpdateTestDriveCarStatus_010 " & "ex.Number <> MessageId_OracleException2049")
                Logger.Error("UpdateTestDriveCarStatus_End Ret[Throw OracleExceptionEx]")
                Throw
            End If
        End Try

        Logger.Info("UpdateTestDriveCarStatus_End")
        Return MessageIdSuccess
    End Function

#End Region

End Class
