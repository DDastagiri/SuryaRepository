'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080225BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客詳細（参照）(ビジネスロジック)
'補足： 
'作成： 2014/02/14 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
'更新： 2014/09/22 SKFC 佐藤 e-Mail,Line送信機能対応
'更新： 2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更
'更新： 2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証
'更新： 2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
'更新： 2018/07/19 NSK  坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類
'更新： 2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証
'─────────────────────────────────────

Option Strict On
Option Explicit On

Imports System
Imports System.Data
Imports System.Globalization
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.BizLogic.IC3800708BusinessLogic
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080225DataSet
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080225DataSetTableAdapters
Imports Toyota.eCRB.DMSLinkage.CustomerInfo.Api.DataAccess.IC3800708DataSet
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic


Public Class SC3080225BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ''' <summary>
    ''' プログラムID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ProgramId As String = "SC3080225"

    ''' <summary>
    ''' 配置区分（1：名前の後）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeAfter As String = "1"
    ''' <summary>
    ''' 配置区分（2：名前の前）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PositionTypeBefore As String = "2"

    ''' <summary>
    ''' ソートキー（0：VIN一致）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SortKeyIsVin As String = "0"
    ''' <summary>
    ''' ソートキー（1：VIN不一致）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const SortKeyNoVin As String = "1"

    ''' <summary>
    ''' 燃料（0：ガソリン）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuelDevisionPetrol As String = "0"
    ''' <summary>
    ''' 燃料（1：ディーゼル）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuelDevisionDiesel As String = "1"
    ''' <summary>
    ''' 燃料（2：ハイブリッド）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuelDevisionHybird As String = "2"
    ''' <summary>
    ''' 燃料（3：プラグインハイブリッド）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuelDevisionPluginHybird As String = "3"
    ''' <summary>
    ''' 燃料（4：燃料電池）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuelDevisionFuelCell As String = "4"
    ''' <summary>
    ''' 燃料（5：電気）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuelDevisionEV As String = "5"
    ''' <summary>
    ''' 燃料（6：バイオ燃）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuelDevisionBiofuel As String = "6"
    ''' <summary>
    ''' 燃料（7：天然ガス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuelDevisionNGV As String = "7"
    ''' <summary>
    ''' 燃料（8：圧縮天然ガス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuelDevisionCNG As String = "8"
    ''' <summary>
    ''' 燃料（9：液化石油ガス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuelDevisionLPG As String = "9"
    ''' <summary>
    ''' 燃料（10：オートガス）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuelDevisionAutogas As String = "10"
    ''' <summary>
    ''' 燃料（99：その他）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const FuelDevisionOther As String = "99"

    ''' <summary>
    ''' 車両区分（0：新車）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NewVehicleDevisionNewCar As String = "0"
    ''' <summary>
    ''' 車両区分（1：中古車）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NewVehicleDevisionUsedCar As String = "1"
    ''' <summary>
    ''' 車両区分（8：その他）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NewVehicleDevisionOther As String = "8"
    ''' <summary>
    ''' 車両区分（9：サービスのみ）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NewVehicleDevisionServiceOnly As String = "9"

    ''' <summary>
    ''' WebService日付フォーマット（dd/MM/yyyy HH:mi:ss）
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WebServiceDateFormat As String = "dd/MM/yyyy HH:mm:ss"

    ''' <summary>
    ''' 個人法人マスタのDISPLAYID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const PRIVATE_FLEET As String = "PRIVATE_FLEET"

    ''' <summary>
    ''' リターンコード
    ''' </summary>
    Private Enum ReturnCode

        ''' <summary>
        ''' 成功
        ''' </summary>
        Success = 0

        ''' <summary>
        ''' DBタイムアウト
        ''' </summary>
        ErrDBTimeout = 901

        ''' <summary>
        ''' データ無し
        ''' </summary>
        ErrNotFound = 902

        ''' <summary>
        ''' 排他エラー
        ''' </summary>
        ErrExclusion = 903

    End Enum

#End Region

#Region "Publicメソッド"

    ''' <summary>
    ''' 入庫履歴情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inVin">VIN</param>
    ''' <param name="inRegsiterNumber">車両登録番号</param>
    ''' <param name="inAllServiceInHistoryType">全件取得フラグ</param>
    ''' <returns>入庫履歴情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Function GetServiceInHistoryInfo(ByVal inDealerCode As String, _
                                            ByVal inVin As String, _
                                            ByVal inRegsiterNumber As String, _
                                            ByVal inAllServiceInHistoryType As Boolean) As SC3080225ContactHistoryInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inDealerCode, _
                                  inVin, _
                                  inRegsiterNumber, _
                                  inAllServiceInHistoryType.ToString(CultureInfo.CurrentCulture)))

        '戻り値
        Dim returnContactHistoryInfo As SC3080225ContactHistoryInfoDataTable = Nothing

        '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
        Using smbCommonBiz As New ServiceCommonClassBusinessLogic
            '車両登録番号の「*」と区切り文字を削除する
            inRegsiterNumber = smbCommonBiz.ConvertVclRegNumWord(inRegsiterNumber)
        End Using
        '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

        Using da As New SC3080225StallInfoDataTableAdapter
            '入庫履歴情報の取得
            returnContactHistoryInfo = da.GetServiceInHistoryInfo(inDealerCode, _
                                                                  inVin, _
                                                                  inRegsiterNumber, _
                                                                  inAllServiceInHistoryType)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END RETURN:COUNT={2} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  returnContactHistoryInfo.Count))
        Return returnContactHistoryInfo

    End Function

    ''' <summary>
    ''' 顧客車両情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inDmsCustomerCode">基幹顧客コード</param>
    ''' <param name="inVin">VIN</param>
    ''' <returns>顧客車両情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Function GetCustomerVehicleInfo(ByVal inDealerCode As String, _
                                           ByVal inBranchCode As String, _
                                           ByVal inDmsCustomerCode As String, _
                                           ByVal inVin As String) As CustomerDetailClass
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inDealerCode, _
                                  inBranchCode, _
                                  inDmsCustomerCode, _
                                  inVin))

        '戻り値
        Dim returnClassCustomerVehicle As CustomerDetailClass = Nothing

        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
        'ローカル変数にVinを設定
        Dim localVin As String = inVin
        Dim start As Long = -1
        Dim count As Long = -1

        If String.IsNullOrWhiteSpace(localVin) Then
            start = 0
            count = 1
        End If

        Using biz As New IC3800708BusinessLogic
            '顧客車両情報を取得
            returnClassCustomerVehicle = biz.GetCustomerDtlinfo(inDealerCode, _
                                                                inBranchCode, _
                                                                inDmsCustomerCode, _
                                                                localVin, _
                                                                start, _
                                                                count)

            ''結果と車両情報のチェック
            'If returnClassCustomerVehicle.ResultCode = Result.Success AndAlso _
            '   1 < returnClassCustomerVehicle.VhcInfo.Count Then
            '    '「0：成功」且つ車両情報が2件以上ある場合
            '    'ソート処理をする
            '    Me.MergeVehicleInfo(inDealerCode, inVin, returnClassCustomerVehicle.VhcInfo)

            'End If

        End Using
        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END RETURN:RETURNCODE:{2} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  returnClassCustomerVehicle.ResultCode))
        Return returnClassCustomerVehicle
    End Function

    ''' <summary>
    ''' 敬称あり顧客名取得
    ''' </summary>
    ''' <param name="inDmsCustomerCode">基幹顧客コード</param>
    ''' <param name="inCustomerName">顧客名</param>
    ''' <returns>敬称あり顧客名</returns>
    ''' <remarks></remarks>
    Public Function GetCustomerNameInTitleName(ByVal inDmsCustomerCode As String, _
                                               ByVal inCustomerName As String, _
                                               ByVal inNameTitle As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START P1:{2} P2:{3} P3:{4} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inDmsCustomerCode, _
                                  inCustomerName, _
                                  inNameTitle))

        '戻り値
        Dim returnCustomerName As String = String.Empty

        '敬称のチェック
        If Not (String.IsNullOrEmpty(inNameTitle)) Then
            'データが存在する場合
            Using da As New SC3080225StallInfoDataTableAdapter
                '敬称情報取得
                Dim dtNameTitleInfo As SC3080225NameTitleInfoDataTable = _
                    da.GetTitleNameInfo(inDmsCustomerCode)

                '取得情報チェック
                If Not (IsNothing(dtNameTitleInfo)) AndAlso 0 < dtNameTitleInfo.Count Then
                    '取得できた場合
                    '敬称位置チェック
                    If dtNameTitleInfo(0).IsPOSITION_TYPENull Then
                        'データが存在しない場合
                        '顧客名を設定
                        returnCustomerName = inCustomerName

                    ElseIf PositionTypeAfter.Equals(dtNameTitleInfo(0).POSITION_TYPE) Then
                        '「1：名称の後」の場合
                        '顧客名敬称を設定
                        returnCustomerName = String.Concat(inCustomerName, inNameTitle)

                    ElseIf PositionTypeBefore.Equals(dtNameTitleInfo(0).POSITION_TYPE) Then
                        '「2：名称の前」の場合
                        '敬称＋顧客名称を設定
                        returnCustomerName = String.Concat(inNameTitle, inCustomerName)

                    Else
                        '上記以外
                        '引数を設定
                        returnCustomerName = inCustomerName

                    End If

                Else
                    '取得できなかった場合
                    '引数を設定
                    returnCustomerName = inCustomerName

                End If

            End Using

        Else
            'データが存在しない場合
            '引数を設定
            returnCustomerName = inCustomerName

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END RETURN:CUSTOMERNAME= ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  returnCustomerName))
        Return returnCustomerName

    End Function

    ''' <summary>
    ''' 基幹顧客ID変換処理
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inDmsCustomerId">基幹顧客ID</param>
    ''' <returns>変換した基幹顧客ID</returns>
    ''' <remarks></remarks>
    Public Function ReplaceDmsCustomerId(ByVal inDealerCode As String, _
                                         ByVal inDmsCustomerId As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START: P1:{2} P2:{3}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode _
                    , inDmsCustomerId))

        '戻り値
        Dim returnDmsCustomerId As String = inDmsCustomerId

        Using bizSMBCommonClass As New SMBCommonClassBusinessLogic
            '変換処理実行
            returnDmsCustomerId = bizSMBCommonClass.ReplaceBaseCustomerCode(inDealerCode, _
                                                                            inDmsCustomerId)
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:DMS_CST_CD={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnDmsCustomerId))
        Return returnDmsCustomerId
    End Function

    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
    ' ''' <summary>
    ' ''' 車両情報マージ処理
    ' ''' </summary>
    ' ''' <param name="inVin">VIN</param>
    ' ''' <param name="dtCustomerVehicleInfo">車両情報リスト</param>
    ' ''' <remarks></remarks>
    'Public Function MergeVehicleInfo(ByVal inDealerCode As String, _
    '                                 ByVal inVin As String, _
    '                                 ByVal dtCustomerVehicleInfo As IC3800708CustomerVhcInfoDataTable) As SC3080225VehicleInfoDataTable
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
    '                              "{0}.{1} START P1:{2} P2:{3} ", _
    '                              Me.GetType.ToString, _
    '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
    '                              inDealerCode, _
    '                              inVin))

    ''' <summary>
    ''' 車両情報マージ処理
    ''' </summary>
    ''' <param name="dtCustomerVehicleInfo">車両情報リスト</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <remarks></remarks>
    Public Function MergeVehicleInfo(ByVal dtCustomerVehicleInfo As IC3800708CustomerVhcInfoDataTable, _
                                     ByVal inDealerCode As String) As SC3080225VehicleInfoDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START P1:{2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inDealerCode))
        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

        Dim returnVehicleInfo As New SC3080225VehicleInfoDataTable

        '共通文言取得
        'km
        Dim wordId169 As String = WebWordUtility.GetWord(169)

        'Petrol「0：ガソリン」
        Dim wordId174 As String = WebWordUtility.GetWord(174)
        'Diesel「1：ディーゼル」
        Dim wordId175 As String = WebWordUtility.GetWord(175)
        'Hybird「2：ハイブリッド」
        Dim wordId176 As String = WebWordUtility.GetWord(176)
        'Plug-in Hbird「3：プラグインハイブリット」
        Dim wordId177 As String = WebWordUtility.GetWord(177)
        'Fuel Cell「4：燃料電池」
        Dim wordId178 As String = WebWordUtility.GetWord(178)
        'EV「5：電気」
        Dim wordId201 As String = WebWordUtility.GetWord(201)
        'Biofuel「6：バイオ燃料」
        Dim wordId179 As String = WebWordUtility.GetWord(179)
        'NGV「7：天然ガス」
        Dim wordId180 As String = WebWordUtility.GetWord(180)
        'CNG「8：圧縮天然ガス」
        Dim wordId181 As String = WebWordUtility.GetWord(181)
        'LPG「9：液化石油ガス」
        Dim wordId182 As String = WebWordUtility.GetWord(182)
        'Autogas「10：オートガス」
        Dim wordId183 As String = WebWordUtility.GetWord(183)
        'Other「99：その他」
        Dim wordId184 As String = WebWordUtility.GetWord(184)

        'New Car「0：新車」
        Dim wordId185 As String = WebWordUtility.GetWord(185)
        'Used Car「1：中古車」
        Dim wordId186 As String = WebWordUtility.GetWord(186)
        'Other「8：その他」
        Dim wordId187 As String = WebWordUtility.GetWord(187)
        'Service Only「9：サービスのみ」
        Dim wordId188 As String = WebWordUtility.GetWord(188)

        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START

        'モデルコードリスト
        Dim modelCodeList As New List(Of String)
        '車両登録エリアリスト
        Dim regAreaCodeList As New List(Of String)

        For Each drCustomerVehicleInfo As IC3800708CustomerVhcInfoRow In dtCustomerVehicleInfo

            '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 STAR

            ''シリーズコードのチェック
            'If Not (drVehicleInfo.IsSERIESCDNull) AndAlso _
            '   Not (String.IsNullOrEmpty(drVehicleInfo.SERIESCD)) Then
            '    'データが存在する場合
            '    'モデルロゴPathを取得
            '    Dim dtModelLogoInfo As SC3080225ModelLogoInfoDataTable = _
            '        Me.GetModelLogoPath(drVehicleInfo.SERIESCD)

            '    '取得情報チェック
            '    If Not (IsNothing(dtModelLogoInfo)) AndAlso 0 < dtModelLogoInfo.Count Then
            '        '取得できた場合
            '        'モデルロゴPathチェック
            '        If Not (dtModelLogoInfo(0).IsLOGO_PICTURENull) AndAlso
            '           Not (dtModelLogoInfo(0).IsLOGO_PICTURE_SELNull) Then
            '            'データが存在する場合
            '            'モデルロゴPathを設定
            '            drVehicleInfo.ModelLogoOffURL = dtModelLogoInfo(0).LOGO_PICTURE
            '            drVehicleInfo.ModelLogoOnURL = dtModelLogoInfo(0).LOGO_PICTURE

            '        Else
            '            '存在しない場合
            '            '空文字を設定
            '            drVehicleInfo.ModelLogoOffURL = String.Empty
            '            drVehicleInfo.ModelLogoOnURL = String.Empty

            '        End If

            '    Else
            '        '存在しない場合
            '        '空文字を設定
            '        drVehicleInfo.ModelLogoOffURL = String.Empty
            '        drVehicleInfo.ModelLogoOnURL = String.Empty

            '    End If

            'Else
            '    '存在しない場合
            '    '空文字を設定
            '    drVehicleInfo.ModelLogoOffURL = String.Empty
            '    drVehicleInfo.ModelLogoOnURL = String.Empty

            'End If

            ''車両登録エリアコードのチェック
            'If Not (drVehicleInfo.IsVehicleAreaCodeNull) AndAlso _
            '   Not (String.IsNullOrEmpty(drVehicleInfo.VehicleAreaCode)) Then
            '    'データが存在する場合
            '    '車両登録エリア名称を取得
            '    Dim registerAreaName As String = _
            '        Me.GetRegisterAreaName(drVehicleInfo.VehicleAreaCode)

            '    '取得情報チェック
            '    If Not (String.IsNullOrEmpty(registerAreaName)) Then
            '        '取得できた場合
            '        '値を設定
            '        drVehicleInfo.VehicleAreaName = registerAreaName

            '    Else
            '        '取得できなかった場合
            '        '空文字を設定
            '        drVehicleInfo.VehicleAreaName = String.Empty

            '    End If

            'Else
            '    '存在しない場合
            '    '空文字を設定
            '    drVehicleInfo.VehicleAreaName = String.Empty

            'End If

            'モデルコードリストにシリーズコードを追加
            If Not (drCustomerVehicleInfo.IsSERIESCDNull) AndAlso Not (String.IsNullOrEmpty(drCustomerVehicleInfo.SERIESCD)) Then
                modelCodeList.Add(drCustomerVehicleInfo.SERIESCD)
            End If

            '車両登録コードエリアリストに車両登録エリアコードを追加
            If Not (drCustomerVehicleInfo.IsVehicleAreaCodeNull) AndAlso Not (String.IsNullOrEmpty(drCustomerVehicleInfo.VehicleAreaCode)) Then
                regAreaCodeList.Add(drCustomerVehicleInfo.VehicleAreaCode)
            End If

        Next

        Dim dtModelLogoInfo As SC3080225ModelLogoInfoDataTable = Nothing
        If 0 < modelCodeList.Count Then
            'モデルロゴの取得
            dtModelLogoInfo = Me.GetModelLogoPath(modelCodeList)
        End If


        Dim registerAreaInfo As SC3080225RegisterAreaInfoDataTable = Nothing
        If 0 < regAreaCodeList.Count Then
            '車両登録エリア情報を取得
            registerAreaInfo = Me.GetRegisterAreaInfo(regAreaCodeList)
        End If

        For Each drCustomerVehicleInfo As IC3800708CustomerVhcInfoRow In dtCustomerVehicleInfo
            Dim drVehicleInfo As SC3080225VehicleInfoRow = returnVehicleInfo.NewSC3080225VehicleInfoRow

            '新規ROWに取得した情報を入れる
            For Each drCustomerVehicleInfoColumn As DataColumn In dtCustomerVehicleInfo.Columns
                If (returnVehicleInfo.Columns.Contains(drCustomerVehicleInfoColumn.ColumnName)) Then
                    drVehicleInfo(drCustomerVehicleInfoColumn.ColumnName) = _
                        drCustomerVehicleInfo(drCustomerVehicleInfoColumn.ColumnName)
                End If
            Next

            drVehicleInfo.ModelLogoOffURL = String.Empty
            drVehicleInfo.ModelLogoOnURL = String.Empty

            If Not (IsNothing(dtModelLogoInfo)) AndAlso Not (drVehicleInfo.IsSERIESCDNull) AndAlso Not (String.IsNullOrEmpty(drVehicleInfo.SERIESCD)) Then

                'モデルロゴ情報設定
                For Each dtModelLogoInfoRow In dtModelLogoInfo
                    If drVehicleInfo.SERIESCD.Equals(dtModelLogoInfoRow.MODEL_CD) Then
                        If Not (dtModelLogoInfoRow.IsLOGO_PICTURENull) AndAlso Not (dtModelLogoInfoRow.IsLOGO_PICTURE_SELNull) Then
                            drVehicleInfo.ModelLogoOffURL = dtModelLogoInfoRow.LOGO_PICTURE
                            drVehicleInfo.ModelLogoOnURL = dtModelLogoInfoRow.LOGO_PICTURE_SEL
                        End If
                        Exit For
                    End If
                Next
            End If

            drVehicleInfo.VehicleAreaName = String.Empty

            If Not (IsNothing(registerAreaInfo)) AndAlso Not (drVehicleInfo.IsVehicleAreaCodeNull) AndAlso Not (String.IsNullOrEmpty(drVehicleInfo.VehicleAreaCode)) Then

                '車両登録エリア設定
                For Each registerAreaInfoRow In registerAreaInfo
                    If drVehicleInfo.VehicleAreaCode.Equals(registerAreaInfoRow.REG_AREA_CD) Then
                        If Not (registerAreaInfoRow.IsREG_AREA_NAMENull) Then
                            drVehicleInfo.VehicleAreaName = registerAreaInfoRow.REG_AREA_NAME
                        End If
                        Exit For
                    End If
                Next
            End If

            '登録日チェック
            If Not (drVehicleInfo.IsVehicleRegistrationDateNull) AndAlso _
               Not (String.IsNullOrEmpty(drVehicleInfo.VehicleRegistrationDate)) Then
                '存在する場合
                '登録日（日付型）のチェック
                Dim registrationDate As Date = Date.MinValue

                '日付変換チェック
                If Date.TryParse(drVehicleInfo.VehicleRegistrationDate, registrationDate) Then
                    '成功した場合
                    '文字列に変換して設定
                    drVehicleInfo.VehicleRegistrationDate = _
                        DateTimeFunc.FormatDate(3, registrationDate)

                Else
                    '失敗した場合
                    '空文字を設定
                    drVehicleInfo.VehicleRegistrationDate = String.Empty

                End If

            Else
                '存在しない場合
                drVehicleInfo.VehicleRegistrationDate = String.Empty

            End If

            '納車日のチェック
            If Not (drVehicleInfo.IsVehicleDeliveryDateNull) AndAlso _
               Not (String.IsNullOrEmpty(drVehicleInfo.VehicleDeliveryDate)) Then
                '存在する場合
                '納車日の（日付型）のチェック
                Dim vehicleDeliveryDate As Date

                '日付変換チェック
                If Date.TryParseExact(drVehicleInfo.VehicleDeliveryDate, WebServiceDateFormat, Nothing, Nothing, vehicleDeliveryDate) Then
                    '成功した場合
                    '文字列に変換して設定
                    drVehicleInfo.VehicleDeliveryDate = _
                        DateTimeFunc.FormatDate(3, vehicleDeliveryDate)

                Else
                    '失敗した場合
                    '空文字を設定
                    drVehicleInfo.VehicleDeliveryDate = String.Empty

                End If

            Else
                '存在しない場合
                '空文字を設定
                drVehicleInfo.VehicleDeliveryDate = String.Empty

            End If

            '最新走行距離更新日のチェック
            If Not (drVehicleInfo.IsRegistDateNull) AndAlso _
               Not (String.IsNullOrEmpty(drVehicleInfo.RegistDate)) Then
                '存在する場合
                '最新走行距離更新日（日付型）のチェック
                Dim registDate As Date = Date.MinValue

                '日付変換チェック
                If Date.TryParseExact(drVehicleInfo.RegistDate, WebServiceDateFormat, Nothing, Nothing, registDate) Then
                    '成功した場合
                    '文字列に変換して設定
                    drVehicleInfo.RegistDate = _
                        DateTimeFunc.FormatDate(3, registDate)
                    drVehicleInfo.LastUpdateDate = _
                        DateTimeFunc.FormatDate(11, registDate)

                Else
                    '失敗した場合
                    '空文字を設定
                    drVehicleInfo.RegistDate = String.Empty
                    drVehicleInfo.LastUpdateDate = String.Empty

                End If

            Else
                '存在しない場合
                '空文字を設定
                drVehicleInfo.RegistDate = String.Empty
                drVehicleInfo.LastUpdateDate = String.Empty

            End If

            'セールス担当者アカウントチェック
            If Not (drVehicleInfo.IsSalesStaffCodeNull) AndAlso _
               Not (String.IsNullOrEmpty(drVehicleInfo.SalesStaffCode)) Then
                '存在する場合
                'アカウント名を取得
                Dim salesUserInfo As UsersDataSet.USERSRow = _
                    (New Users).GetUser(String.Concat(drVehicleInfo.SalesStaffCode, _
                                                      "@", _
                                                      inDealerCode))
                '取得情報チェック
                If salesUserInfo IsNot Nothing Then
                    'データが存在する場合
                    '値を設定
                    drVehicleInfo.SalesStaffName = salesUserInfo.USERNAME

                Else
                    '存在しない場合
                    '空文字を設定
                    drVehicleInfo.SalesStaffName = String.Empty

                End If

            Else
                '存在しない場合
                '空文字を設定
                drVehicleInfo.SalesStaffName = String.Empty

            End If

            'サービス担当者アカウントチェック
            If Not (drVehicleInfo.IsServiceAdviserCodeNull) AndAlso _
               Not (String.IsNullOrEmpty(drVehicleInfo.ServiceAdviserCode)) Then
                '存在する場合
                'アカウント名を取得
                Dim serviceUserInfo As UsersDataSet.USERSRow = _
                    (New Users).GetUser(String.Concat(drVehicleInfo.ServiceAdviserCode, _
                                                      "@", _
                                                      inDealerCode))
                '取得情報チェック
                If serviceUserInfo IsNot Nothing Then
                    'データが存在する場合
                    '値を設定
                    drVehicleInfo.ServiceAdviserName = serviceUserInfo.USERNAME

                Else
                    '存在しない場合
                    '空文字を設定
                    drVehicleInfo.ServiceAdviserName = String.Empty

                End If

            Else
                '存在しない場合
                '空文字を設定
                drVehicleInfo.ServiceAdviserName = String.Empty

            End If

            '最新走行距離データチェック
            If Not (drVehicleInfo.IsMileageNull) AndAlso _
               Not (String.IsNullOrEmpty(drVehicleInfo.Mileage)) Then
                '存在する場合
                '「最新走行距離＋km」を設定する
                drVehicleInfo.Mileage = String.Concat(drVehicleInfo.Mileage, wordId169)

            Else
                '存在しない場合
                '空文字を設定する
                drVehicleInfo.Mileage = String.Empty

            End If

            '燃料
            Select Case Trim(drVehicleInfo.FuelDivision)
                Case FuelDevisionPetrol
                    'Petrol「0：ガソリン」
                    drVehicleInfo.FuelDivisionName = wordId174

                Case FuelDevisionDiesel
                    'Diesel「1：ディーゼル」
                    drVehicleInfo.FuelDivisionName = wordId175

                Case FuelDevisionHybird
                    'Hybird「2：ハイブリッド」
                    drVehicleInfo.FuelDivisionName = wordId176

                Case FuelDevisionPluginHybird
                    'Plug-in Hbird「3：プラグインハイブリット」
                    drVehicleInfo.FuelDivisionName = wordId177

                Case FuelDevisionFuelCell
                    'Fuel Cell「4：燃料電池」
                    drVehicleInfo.FuelDivisionName = wordId178

                Case FuelDevisionEV
                    'EV「5：電気」
                    drVehicleInfo.FuelDivisionName = wordId201

                Case FuelDevisionBiofuel
                    'Biofuel「6：バイオ燃料」
                    drVehicleInfo.FuelDivisionName = wordId179

                Case FuelDevisionNGV
                    'NGV「7：天然ガス」
                    drVehicleInfo.FuelDivisionName = wordId180

                Case FuelDevisionCNG
                    'CNG「8：圧縮天然ガス」
                    drVehicleInfo.FuelDivisionName = wordId181

                Case FuelDevisionLPG
                    'LPG「9：液化石油ガス」
                    drVehicleInfo.FuelDivisionName = wordId182

                Case FuelDevisionAutogas
                    'Autogas「10：オートガス」
                    drVehicleInfo.FuelDivisionName = wordId183

                Case FuelDevisionOther
                    'Other「99：その他」
                    drVehicleInfo.FuelDivisionName = wordId184

                Case Else
                    '上記以外
                    drVehicleInfo.FuelDivisionName = String.Empty

            End Select

            '車両区分
            Select Case drVehicleInfo.NewVehicleDivision
                Case NewVehicleDevisionNewCar
                    'New Car「0：新車」
                    drVehicleInfo.NewVehicleDivisionName = wordId185

                Case NewVehicleDevisionUsedCar
                    'Used Car「1：中古車」
                    drVehicleInfo.NewVehicleDivisionName = wordId186

                Case NewVehicleDevisionOther
                    'Other「8：その他」
                    drVehicleInfo.NewVehicleDivisionName = wordId187

                Case NewVehicleDevisionServiceOnly
                    'Service Only「9：サービスのみ」
                    drVehicleInfo.NewVehicleDivisionName = wordId188

                Case Else
                    '上記以外
                    drVehicleInfo.NewVehicleDivisionName = String.Empty

            End Select

            '保険満期日のチェック
            If Not (drVehicleInfo.IsEndDateNull) AndAlso _
               Not (String.IsNullOrEmpty(drVehicleInfo.EndDate)) Then
                '存在する場合
                '保険満期日の（日付型）のチェック
                Dim endDate As Date = Date.MinValue

                '日付変換チェック
                If Date.TryParseExact(drVehicleInfo.VehicleRegistrationDate, WebServiceDateFormat, Nothing, Nothing, endDate) Then
                    '成功した場合
                    '文字列に変換して設定
                    drVehicleInfo.EndDate = _
                        DateTimeFunc.FormatDate(3, endDate)

                Else
                    '失敗した場合
                    '空文字を設定
                    drVehicleInfo.EndDate = String.Empty

                End If

            Else
                '存在しない場合
                '空文字を設定
                drVehicleInfo.EndDate = String.Empty

            End If

            'DataTableに格納
            returnVehicleInfo.Rows.Add(drVehicleInfo)

        Next

        ''VINのチェック
        'If Not (String.IsNullOrEmpty(inVin)) AndAlso _
        '   Not (drVehicleInfo.IsVinNull) AndAlso _
        '   inVin.Equals(drVehicleInfo.Vin) Then
        '    'VINが一致する場合
        '    'ソートキー「0：VIN一致」を設定
        '    drVehicleInfo.SortKey = SortKeyIsVin

        'Else
        '    'VINが一致しない場合
        '    'ソートキー「1：VIN不一致」を設定
        '    drVehicleInfo.SortKey = SortKeyNoVin

        'End If

        ''DataTableに格納
        'returnVehicleInfo.Rows.Add(drVehicleInfo)

        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return returnVehicleInfo
    End Function

    ''' <summary>
    ''' 顧客写真登録処理
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inCustomerCode">店舗コード</param>
    ''' <param name="inRowRockVersion">行ロックバージョン</param>
    ''' <param name="inCustomerPhotoPathLarge">写真パス(L)</param>
    ''' <param name="inCustomerPhotoPathMiddle">写真パス(M)</param>
    ''' <param name="inCustomerPhotoPathSmall">写真パス(S)</param>
    ''' <param name="inNowDate">現在日時</param>
    ''' <param name="inAccount">アカウント</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function RegisterCustomerPhotoInfo(ByVal inDealerCode As String, _
                                              ByVal inCustomerCode As Decimal, _
                                              ByVal inRowRockVersion As Long, _
                                              ByVal inCustomerPhotoPathLarge As String, _
                                              ByVal inCustomerPhotoPathMiddle As String, _
                                              ByVal inCustomerPhotoPathSmall As String, _
                                              ByVal inNowDate As Date, _
                                              ByVal inAccount As String) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7} P7:{8} P8:{9} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inDealerCode, _
                                  inCustomerCode.ToString(CultureInfo.CurrentCulture), _
                                  inRowRockVersion.ToString(CultureInfo.CurrentCulture), _
                                  inCustomerPhotoPathLarge, _
                                  inCustomerPhotoPathMiddle, _
                                  inCustomerPhotoPathSmall, _
                                  inNowDate.ToString(CultureInfo.CurrentCulture), _
                                  inAccount))
        Dim errorCode As Long

        Using da As New SC3080225StallInfoDataTableAdapter
            '顧客テーブルロック処理
            Using bizSmbCommonClass As New SMBCommonClassBusinessLogic
                errorCode = bizSmbCommonClass.LockCustomerTable(inCustomerCode, _
                                                                inRowRockVersion, _
                                                                inAccount, _
                                                                inNowDate, _
                                                                ProgramId)

            End Using

            '処理結果チェック
            If errorCode <> ReturnCode.Success Then
                '存在しない場合
                errorCode = ReturnCode.ErrNotFound

            Else
                '写真登録処理
                Dim count = da.RegisterCustomerPhoto(inDealerCode, _
                                                     inCustomerCode, _
                                                     inCustomerPhotoPathLarge, _
                                                     inCustomerPhotoPathMiddle, _
                                                     inCustomerPhotoPathSmall, _
                                                     inNowDate, _
                                                     inAccount)

                '処理結果チェック
                If count <> 1 Then
                    '更新失敗の場合
                    errorCode = ReturnCode.ErrNotFound

                End If

            End If

        End Using

        'エラーコードチェック
        If errorCode <> ReturnCode.Success Then
            'エラーの場合
            'ロールバック処理を行う
            Me.Rollback = True

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return errorCode
    End Function

    ''' <summary>
    ''' RO情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inOrderNumber">RO番号</param>
    ''' <returns>RO情報</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Function GetOrderPreviewInfo(ByVal inDealerCode As String, _
                                        ByVal inOrderNumber As String) As SC3080225OrderPreviewInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START P1:{2} P2:{3} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inDealerCode, _
                                  inOrderNumber))

        '戻り値
        Dim returnOrderInfo As SC3080225OrderPreviewInfoDataTable = Nothing

        Using da As New SC3080225StallInfoDataTableAdapter
            '入庫履歴情報の取得
            returnOrderInfo = da.GetOrderInfo(inDealerCode, _
                                              inOrderNumber)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END RETURN:COUNT={2} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  returnOrderInfo.Count))
        Return returnOrderInfo

    End Function

    ''' <summary>
    ''' DMS情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inAccount">アカウント</param>
    ''' <returns>DMS情報</returns>
    ''' <remarks></remarks>
    Public Function GetDmsDealerData(ByVal inDealerCode As String, _
                                     ByVal inBranchCode As String, _
                                     ByVal inAccount As String) As DmsCodeMapDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        Using biz As New ServiceCommonClassBusinessLogic
            '取得条件
            Dim searchInfo As ServiceCommonClassBusinessLogic.DmsCodeType
            If Not (String.IsNullOrEmpty(inDealerCode)) AndAlso
               Not (String.IsNullOrEmpty(inBranchCode)) Then
                searchInfo = ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode
            Else
                searchInfo = ServiceCommonClassBusinessLogic.DmsCodeType.DealerCode

            End If

            'DMS販売店データの取得
            Dim dtDmsCodeMapDataTable As DmsCodeMapDataTable = _
                biz.GetIcropToDmsCode(inDealerCode,
                                      searchInfo, _
                                      inDealerCode, _
                                      inBranchCode, _
                                      String.Empty, _
                                      inAccount)

            If dtDmsCodeMapDataTable.Count <= 0 Then
                'データが取得できない場合はエラー
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is nothing" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return Nothing

            ElseIf 1 < dtDmsCodeMapDataTable.Count Then
                'データが2件以上取得できた場合は一意に決定できないためエラー
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ERROR:TB_M_DMS_CODE_MAP is sum data" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return Nothing
            Else
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END " _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return dtDmsCodeMapDataTable

            End If

        End Using
    End Function

    ''' <summary>
    ''' 顧客情報取得
    ''' </summary>
    ''' <param name="inDealerCode"></param>
    ''' <param name="inIcropDmsCustomerCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCustomerInfo(ByVal inDealerCode As String, _
                                    ByVal inIcropDmsCustomerCode As String) As SC3080225CustomerInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START P1:{2} P2:{3} " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode _
                    , inIcropDmsCustomerCode))

        '戻り値
        Dim returnCustomerInfo As SC3080225CustomerInfoDataTable = Nothing

        Using da As New SC3080225StallInfoDataTableAdapter
            '顧客情報の取得
            returnCustomerInfo = da.GetCustomerInfo(inDealerCode, _
                                                    inIcropDmsCustomerCode)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnCustomerInfo.Count))
        Return returnCustomerInfo
    End Function

    ''' <summary>
    ''' 個人法人項目マスタの文言取得
    ''' </summary>
    ''' <param name="inSubCustomerType">個人法人項目コード</param>
    ''' <returns>個人法人項目文言</returns>
    ''' <remarks></remarks>
    Public Function GetPrivateFleetWord(ByVal inSubCustomerType As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START P1:{2} " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inSubCustomerType))

        '戻り値
        Dim returnPrivateFleetWord As String = String.Empty

        Using da As New SC3080225StallInfoDataTableAdapter
            '個人法人項目マスタの文言番号取得
            Dim dtPrivateFleetInfo As SC3080225PrivateFleetInfoDataTable = _
                da.GetPrivateFleetWord(inSubCustomerType)

            'DataTableと値のチェック
            If Not (IsNothing(dtPrivateFleetInfo)) AndAlso _
               0 < dtPrivateFleetInfo.Count AndAlso _
               Not (dtPrivateFleetInfo(0).IsPRIVATE_FLEET_ITEMNull) Then
                'データが存在する場合

                Using biz As New ServiceCommonClassBusinessLogic
                    '新文言テーブルから文言情報を取得する
                    Dim dtWordMaster As WordMasterDataTable = _
                        biz.GetNewWordMasterInfo(dtPrivateFleetInfo(0).PRIVATE_FLEET_ITEM)

                    '取得データチェック
                    If Not (IsNothing(dtWordMaster)) AndAlso 0 < dtWordMaster.Count AndAlso Not (dtWordMaster(0).IsWORDNull) Then
                        '取得できた場合
                        '文言を戻り値に設定
                        returnPrivateFleetWord = dtWordMaster(0).WORD

                    End If

                End Using

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnPrivateFleetWord))
        Return returnPrivateFleetWord
    End Function

    '2018/07/23 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 START
    ''' <summary>
    ''' 顧客所属区分取得
    ''' </summary>
    ''' <param name="inSubCustomerType">サブ顧客区分</param>
    ''' <returns>顧客所属区分</returns>
    ''' <remarks></remarks>
    Public Function GetCustomerJoinType(ByVal inSubCustomerType As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START P1:{2} " _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inSubCustomerType))

        '戻り値
        Dim returnCustomerJoinType As String = String.Empty

        Using da As New SC3080225StallInfoDataTableAdapter
            '顧客所属区分の取得
            Dim dtCustomerJoinType As SC3080225CustomerJoinTypeDataTable =
                da.GetCustomerMarkType(inSubCustomerType)

            'DataTableと値のチェック
            If Not (IsNothing(dtCustomerJoinType)) AndAlso _
               0 < dtCustomerJoinType.Count AndAlso _
               Not (dtCustomerJoinType(0).IsCST_JOIN_TYPENull) Then
                'データが存在する場合
                returnCustomerJoinType = dtCustomerJoinType(0).CST_JOIN_TYPE
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnCustomerJoinType))
        Return returnCustomerJoinType
    End Function
    '2018/07/23 NSK 坂本 TKM Next Gen e-CRB Project Application development Block B-1  顧客タイプ分類 END

    ''' <summary>
    ''' ROnum取得
    ''' </summary>
    ''' <param name="inDealerCode">Dealerコード</param>
    ''' <param name="inCustomerCode">顧客コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/09/22 SKFC 佐藤 e-Mail,Line送信機能対応により追加
    ''' </history>
    Public Function GetRONumber(ByVal inDealerCode As String, _
                                    ByVal inCustomerCode As Decimal, _
                                    ByVal inVin As String, _
                                    ByVal inRegisterNumber As String) As String
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , inDealerCode _
                    , inCustomerCode _
                    , inVin _
                    , inRegisterNumber))

        '戻り値
        Dim returnRONumber As String = String.Empty

        '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
        Using smbCommonBiz As New ServiceCommonClassBusinessLogic
            '車両登録番号の「*」と区切り文字を削除する
            inRegisterNumber = smbCommonBiz.ConvertVclRegNumWord(inRegisterNumber)
        End Using
        '2015/11/10 TM 杉田  (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

        Using da As New SC3080225StallInfoDataTableAdapter

            Dim dtRONumberInfo As SC3080225RONumberInfoDataTable = _
                da.GetRONumber(inDealerCode, inCustomerCode, inVin, inRegisterNumber)

            If dtRONumberInfo.Rows.Count > 0 AndAlso _
                Not (dtRONumberInfo(0).IsRO_NUMNull) AndAlso _
                Not String.IsNullOrWhiteSpace(dtRONumberInfo(0).RO_NUM) Then

                returnRONumber = dtRONumberInfo(0).RO_NUM
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnRONumber.Count))
        Return returnRONumber
    End Function

    '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 START
    ''' <summary>
    ''' SSCアイコン取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inVin">VIN</param>
    ''' <param name="inRegisterNumber">登録番号</param>
    ''' <returns>SSC対象フラグ</returns>
    ''' <remarks></remarks>
    Public Function GetSscFlg(ByVal inDealerCode As String, _
                              ByVal inVin As String, _
                              ByVal inRegisterNumber As String) As String

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START P1:{2} P2:{3} P3:{4} " _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inDealerCode _
            , inVin _
            , inRegisterNumber))

        '戻り値
        Dim returnSscFlag As String = String.Empty

        Using da As New SC3080225StallInfoDataTableAdapter
            'SSCフラグ取得
            returnSscFlag = da.GetSscFlg(inDealerCode, _
                                         inVin, _
                                         inRegisterNumber)
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:SSC_FLAG={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnSscFlag))

        Return returnSscFlag
    End Function
    '2018/02/27 NSK 山田 17PRJ01947-00_(トライ店システム評価)サービス重要顧客における、識別及び対処オペレーションの検証 END

    '2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
    ''' <summary>
    ''' 車両フラグ情報取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inVin">VIN</param>
    ''' <param name="inRegisterNumber">登録番号</param>
    ''' <returns>車両フラグ情報</returns>
    ''' <remarks></remarks>
    Public Function GetVehicleFlg(ByVal inDealerCode As String, _
                                  ByVal inVin As String, _
                                  ByVal inRegisterNumber As String) As SC3080225VehicleFlgDataTable

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START P1:{2} P2:{3} P3:{4} " _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inDealerCode _
            , inVin _
            , inRegisterNumber))

        '戻り値
        Dim returnVehicleFlag As SC3080225VehicleFlgDataTable = Nothing

        Using da As New SC3080225StallInfoDataTableAdapter
            '車両フラグ情報取得
            returnVehicleFlag = da.GetVehicleFlg(inDealerCode, _
                                                 inVin, _
                                                 inRegisterNumber)
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURN:COUNT={2}" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnVehicleFlag.Count))
        Return returnVehicleFlag
    End Function
    '2018/07/04 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
    ''' <summary>
    ''' 顧客保有車両取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inDmsCustomerCode">顧客コード</param>
    ''' <param name="inVin">Vin</param>
    ''' <param name="inStart">開始位置</param>
    ''' <param name="inCount">取得件数</param>
    ''' <returns>作業詳細</returns>
    ''' <remarks></remarks>
    Public Function GetHoldingCustomerVehicleInfo(ByVal inDealerCode As String, _
                                           ByVal inBranchCode As String, _
                                           ByVal inDmsCustomerCode As String, _
                                           ByVal inVin As String, _
                                           ByVal inStart As Long, _
                                           ByVal inCount As Long) As CustomerDetailClass
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                          "{0}.{1} START P1:{2} P2:{3} P3:{4} P4:{5} P5:{6} P6:{7}", _
                          Me.GetType.ToString, _
                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                          inDealerCode, _
                          inBranchCode, _
                          inDmsCustomerCode, _
                          inVin, _
                          inStart, _
                          inCount))

        '戻り値
        Dim returnClassCustomerVehicle As CustomerDetailClass = Nothing

        Using biz As New IC3800708BusinessLogic
            '顧客車両情報を取得
            returnClassCustomerVehicle = biz.GetCustomerDtlinfo(inDealerCode, _
                                                                inBranchCode, _
                                                                inDmsCustomerCode, _
                                                                inVin, _
                                                                inStart, _
                                                                inCount)
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                          "{0}.{1} END RETURN:RETURNCODE:{2} ", _
                          Me.GetType.ToString, _
                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                          returnClassCustomerVehicle.ResultCode))

        Return returnClassCustomerVehicle

    End Function

    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END
#End Region

#Region "Privateメソッド"
    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
    ' ''' <summary>
    ' ''' 車両登録エリア名称取得
    ' ''' </summary>
    ' ''' <param name="inRegisterAreaCode">車両登録エリアコード</param>
    ' ''' <returns>車両登録エリア名称</returns>
    ' ''' <remarks></remarks>
    'Private Function GetRegisterAreaName(ByVal inRegisterAreaCode As String) As SC3080225RegisterAreaInfoDataTable
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
    '                              "{0}.{1} START P1:{2} ", _
    '                              Me.GetType.ToString, _
    '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
    '                              inRegisterAreaCode))

    ''戻り値
    'Dim returnRegisterAreaName As String = String.Empty

    '    Using da As New SC3080225StallInfoDataTableAdapter
    ''車両登録エリア情報取得
    'Dim dtRegisterAreaInfo As SC3080225RegisterAreaInfoDataTable = _
    '    da.GetRegisterAreaInfo(inRegisterAreaCodeList)

    ''取得情報チェック
    '        If Not (IsNothing(dtRegisterAreaInfo)) AndAlso 0 < dtRegisterAreaInfo.Count Then
    ''取得できた場合
    ''車両登録エリア名称チェック
    '            If Not (dtRegisterAreaInfo(0).IsREG_AREA_NAMENull) Then
    ''データが存在する場合
    ''車両登録エリア名称を設定
    '                returnRegisterAreaName = dtRegisterAreaInfo(0).REG_AREA_NAME

    '            End If

    '        End If

    '    End Using

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
    '                              "{0}.{1} END RETURN:CUSTOMERNAME= ", _
    '                              Me.GetType.ToString, _
    '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
    '                              returnRegisterAreaName))
    '    Return returnRegisterAreaName

    ''' <summary>
    ''' 車両登録エリア名称取得
    ''' </summary>
    ''' <param name="inRegisterAreaCodeList">車両登録エリアコード</param>
    ''' <returns>車両登録エリア名称</returns>
    ''' <remarks></remarks>
    Private Function GetRegisterAreaInfo(ByVal inRegisterAreaCodeList As List(Of String)) As SC3080225RegisterAreaInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START P1:{2} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inRegisterAreaCodeList))

        '戻り値
        Dim returnRegisterAreaInfo As SC3080225RegisterAreaInfoDataTable

        Using da As New SC3080225StallInfoDataTableAdapter

            '車両登録エリア情報取得
            returnRegisterAreaInfo = da.GetRegisterAreaInfo(inRegisterAreaCodeList)

        End Using
        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END RETURN:CUSTOMERNAME= ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  returnRegisterAreaInfo))
        Return returnRegisterAreaInfo

    End Function
    '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 START
    ' ''' <summary>
    ' ''' モデルロゴのパス取得
    ' ''' </summary>
    ' ''' <param name="inModelCode">モデルコード</param>
    ' ''' <returns>モデルロゴのパス</returns>
    ' ''' <remarks></remarks>
    'Private Function GetModelLogoPath(ByVal inModelCode As String) As SC3080225ModelLogoInfoDataTable
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
    '                              "{0}.{1} START P1:{2} ", _
    '                              Me.GetType.ToString, _
    '                              System.Reflection.MethodBase.GetCurrentMethod.Name, _
    '                              inModelCode))

    ''' <summary>
    ''' モデルロゴのパス取得
    ''' </summary>
    ''' <param name="inModelCodeList">モデルコード</param>
    ''' <returns>モデルロゴのパス</returns>
    ''' <remarks></remarks>
    Private Function GetModelLogoPath(ByVal inModelCodeList As List(Of String)) As SC3080225ModelLogoInfoDataTable
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START P1:{2} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inModelCodeList))

        '戻り値
        Dim returnModelLogoInfo As SC3080225ModelLogoInfoDataTable

        Using da As New SC3080225StallInfoDataTableAdapter
            'モデルロゴ情報取得
            returnModelLogoInfo = da.GetModelLogoInfo(inModelCodeList)

        End Using
        '2020/02/28 NSK 坂本 (トライ店システム評価)大量車両保有顧客における、レスポンス向上検証 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END RETURN:COUNT= ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  returnModelLogoInfo.Count))
        Return returnModelLogoInfo

    End Function

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
