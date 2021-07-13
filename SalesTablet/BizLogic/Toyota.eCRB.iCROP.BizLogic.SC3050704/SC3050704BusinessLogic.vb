Imports System.Globalization
Imports System.Reflection.MethodBase
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports System.IO
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.TCVSettingUtility

''' <summary>
''' MOP/DOP設定画面のビジネスロジック層
''' </summary>
''' <remarks></remarks>
Public Class SC3050704BusinessLogic

#Region " コンストラクタ "
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        REM
    End Sub
#End Region

#Region " 定数 "

#Region " JSON "

    ''' <summary>
    ''' リコメンド情報ファイル:オプショングループマスタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RECOMMEND_OPTION_GROUP_MST As String = "option_group_mst"

    ''' <summary>
    ''' リコメンド情報ファイル:メーカーオプション関連付け
    ''' </summary>
    ''' <remarks></remarks>
    Private Const RECOMMEND_OPTION_GROUP_RELATIVE As String = "option_group_relative_option"

    ''' <summary>
    ''' 販売店情報ファイル:パーツ情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEALER_PARTS As String = "parts"

    ''' <summary>
    ''' 販売店情報ファイル:ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEALER_PARTS_ID As String = "id"

    ''' <summary>
    ''' 販売店情報ファイル:名称
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEALER_PARTS_NAME As String = "name"

    ''' <summary>
    ''' 販売店情報ファイル:ボタン表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEALER_PARTS_BTN_FLG As String = "btn_flg"

    ''' <summary>
    ''' 販売店情報ファイル:販売店オプションリコメンド情報
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DEALER_OPTION_GROUP_RELATIVE As String = "option_group_relative_option"

#End Region

#Region " コード値 etc. "

    ''' <summary>
    ''' オプション種別:メーカーオプション
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPTION_KIND_MAKER As String = "1"

    ''' <summary>
    ''' オプション種別:販売店オプション
    ''' </summary>
    ''' <remarks></remarks>
    Private Const OPTION_KIND_DEALER As String = "2"

    ''' <summary>
    ''' 表示フラグ:可視
    ''' </summary>
    ''' <remarks></remarks>
    Private Const DISPLAY_FLG_VISIBILITY As String = "1"

    ''' <summary>
    ''' 属性名:リコメンドが存在しない
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ATTRIBUTE_NAME_NOT_EXIST As String = "-"

    ''' <summary>
    ''' 重み:指定なし
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WEIGHT_UNSPECIFIED As Integer = 0

    ''' <summary>
    ''' 重み:変換オペランド
    ''' </summary>
    ''' <remarks></remarks>
    Private Const WEIGHT_OPERAND As Integer = 1000

    ''' <summary>
    ''' 更新リスト日付書式
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UPDATE_LIST_DATE_FORMAT As Integer = 15

    ''' <summary>
    ''' 更新リスト操作区分:UPDATE
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UPDATE_LIST_UPDATE As String = "UPDATE"

    ''' <summary>
    ''' 文言:エラーメッセージ
    ''' </summary>
    ''' <remarks>画面に表示された情報が最新ではない可能性があります。画面を再表示して下さい。</remarks>
    Private Const WORD_ERR_NOT_LATEST As Integer = 900

    ''' <summary>
    ''' 重みの最低値
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORDER_MIN_VALUE As Integer = -1000
#End Region

    ''' <summary>
    ''' 処理結果:正常終了
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultSucceed As Integer = 0

    ''' <summary>
    ''' 処理結果:異常終了
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ResultFailed As Integer = WORD_ERR_NOT_LATEST


#End Region

#Region " パブリック メソッド "

    ''' <summary>
    ''' MOP/DOP情報を取得します。
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carId">車両ID</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="mopKindName">MOP名</param>
    ''' <param name="dopKindName">DOP名</param>
    ''' <param name="isDist">DIST権限かどうか</param>
    ''' <returns>MOP/DOP情報</returns>
    ''' <remarks></remarks>
    Public Function GetOptionInfo(
        ByVal tcvPath As String,
        ByVal carId As String,
        ByVal dealerCD As String,
        ByVal mopKindName As String,
        ByVal dopKindName As String,
        ByVal isDist As Boolean
    ) As MopDopInfoList

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCD, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("mopKindName", mopKindName, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dopKindName", dopKindName, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("isDist", isDist.ToString, True))

        '返却するMOP/DOP情報
        Dim mopDopInfo As New MopDopInfoList

        'JSOファイルパス取得
        Dim recommendJsonPath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.RecommendJsonPath, carId)
        Dim tcvDealerJsonPath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.TcvDealerJsonPath, carId, dealerCD)

        'JSONファイルの更新日時取得
        If isDist Then
            'Dist権限の場合はリコメンド情報の更新日付
            mopDopInfo.TimeStamp = GetFileTimeStamp(recommendJsonPath)
        Else
            'Dist権限以外の場合は販売店情報の更新日付
            mopDopInfo.TimeStamp = GetFileTimeStamp(tcvDealerJsonPath)
        End If

        'リコメンド情報取得
        Dim recommendJson As Dictionary(Of String, Object) = GetRecommendJson(recommendJsonPath)

        'MOP/DOP情報一覧を生成
        Dim mopDopInfoList As New List(Of MopDopInfo)

        'MOP情報取得
        Dim mopInfoList As List(Of MopDopInfo) = GetMakerOptionInfo(recommendJson, tcvPath, carId, mopKindName)
        mopDopInfoList.AddRange(mopInfoList)

        'DIST権限以外の場合
        If Not isDist Then
            'DOP情報取得
            Dim dopInfoList As List(Of MopDopInfo) = GetDealerOptionInfo(recommendJson, tcvDealerJsonPath, dopKindName)

            '取得した情報を結合
            mopDopInfoList.AddRange(dopInfoList)
        End If

        'MOP/DOP情報一覧を調整
        AdjustForGet(mopDopInfoList)

        'MOP/DOP情報に一覧を設定
        mopDopInfo.SetMopDopInfoList(mopDopInfoList)


        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("mopDopInfoList", mopDopInfo.MopDopInfoList.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        'MOP/DOP情報を返却
        Return mopDopInfo

    End Function

    ''' <summary>
    ''' MOP/DOP情報を更新します。
    ''' </summary>
    ''' <param name="optionInfo">MOP/DOP情報</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carId">車両ID</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="account"></param>
    ''' <param name="updateListPath">更新リスト格納パス</param>
    ''' <param name="isDist">DIST権限かどうか</param>
    ''' <returns>正常時は0、異常時はエラーメッセージIDを返します。</returns>
    ''' <remarks></remarks>
    Public Function UpdateOptionInfo(
        ByVal optionInfo As MopDopInfoList,
        ByVal tcvPath As String,
        ByVal carId As String,
        ByVal dealerCD As String,
        ByVal account As String,
        ByVal updateListPath As String,
        ByVal isDist As Boolean
    ) As Integer

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("MopDopInfoList", optionInfo.MopDopInfoList.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("TimeStamp", optionInfo.TimeStamp, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCD, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("account", account, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("updateListPath", updateListPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("isDist", isDist.ToString, True))

        '処理結果
        Dim resultId As Integer = ResultSucceed
        Dim isUpdated As Boolean = False

        'JSOファイルパス取得
        Dim recommendJsonPath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.RecommendJsonPath, carId)
        Dim tcvDealerJsonPath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.TcvDealerJsonPath, carId, dealerCD)

        '更新JSONパス
        Dim updateJsonPathList As New List(Of String)

        'MOP/DOP情報一覧を調整
        AdjustForUpdate(optionInfo.MopDopInfoList)

        'DIST権限かどうかを判定
        If isDist Then

            If optionInfo.MopDopInfoList.Count > 0 Then
                'MOP情報の書き込み内容を取得
                Dim writeValue As String = CreateRecommendWriteValue(optionInfo.MopDopInfoList, recommendJsonPath)

                '更新JSONパスを追加
                updateJsonPathList.Add(recommendJsonPath.Replace(tcvPath, ""))

                '書き込み処理
                Dim msgId As String = JsonUtilCommon.SetValue(recommendJsonPath, writeValue, optionInfo.TimeStamp)

                If String.IsNullOrEmpty(msgId) Then
                    isUpdated = True
                Else
                    resultId = CInt(msgId)
                End If
            End If
        Else

            'DOP情報の書き込み内容を取得
            Dim dopWriteValue As String = Nothing
            If optionInfo.MopDopInfoList.Count > 0 Then
                dopWriteValue = CreateTcvDealerWriteValue(optionInfo.MopDopInfoList, tcvDealerJsonPath)
            End If

            '更新対象のJSONファイルが下記条件を満たしていれば書き込みを行う
            '┗画面表示時点から更新されていないこと
            '┗書き込み可能な状態であること
            If Not String.IsNullOrEmpty(dopWriteValue) Then
                '更新JSONパスを追加
                updateJsonPathList.Add(tcvDealerJsonPath.Replace(tcvPath, ""))

                '書き込み処理
                Dim msgId As String = JsonUtilCommon.SetValue(tcvDealerJsonPath, dopWriteValue, optionInfo.TimeStamp)

                If String.IsNullOrEmpty(msgId) Then
                    isUpdated = True
                Else
                    resultId = CInt(msgId)
                End If
            End If
        End If

        '更新リスト作成
        If isUpdated Then
            CallCreateTcvArchiveFile(updateJsonPathList, updateListPath, account)
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(resultId.ToString(CultureInfo.InvariantCulture)))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        '処理結果を返却
        Return resultId

    End Function

#End Region

#Region " プライベート メソッド "

#Region " JSON取得 "

    ''' <summary>
    ''' recommend.jsonの情報を取得します。
    ''' </summary>
    ''' <param name="recommendJsonPath">recommend.jsonのパス</param>
    ''' <returns>MOP情報</returns>
    ''' <remarks></remarks>
    Private Function GetRecommendJson(ByVal recommendJsonPath As String) As Dictionary(Of String, Object)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("recommendJsonPath", recommendJsonPath, True))

        'リコメンド情報取得
        Dim recommendJsonValue As String = JsonUtilCommon.GetValue(recommendJsonPath)
        Dim recommendJson As Dictionary(Of String, Object) = Nothing

        'リコメンド情報を変換
        Dim serializer As New JavaScriptSerializer(New SimpleTypeResolver)
        recommendJson = serializer.Deserialize(Of Dictionary(Of String, Object))(recommendJsonValue)
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("recommendJson", recommendJson.ToArray))


        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        'リコメンド情報を返却
        Return recommendJson

    End Function

    ''' <summary>
    ''' MOP情報を取得します。
    ''' </summary>
    ''' <param name="recommendJson">リコメンド情報</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carId">車両ID</param>
    ''' <param name="mopKindName">MOP名</param>
    ''' <returns>MOP情報</returns>
    ''' <remarks></remarks>
    Private Function GetMakerOptionInfo(
        ByVal recommendJson As Dictionary(Of String, Object),
        ByVal tcvPath As String,
        ByVal carId As String,
        ByVal mopKindName As String
    ) As List(Of MopDopInfo)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("mopKindName", mopKindName, True))

        '返却するMOP情報
        Dim mopInfoList As New List(Of MopDopInfo)

        '表示対象のパーツ情報のみを取得
        Dim partsNames As Dictionary(Of String, String) = GetMakerPartsNames(tcvPath, carId)

        'リコメンド内部情報取得
        Dim optionGroupMst As Dictionary(Of String, Object) = GetInnerItem(recommendJson, RECOMMEND_OPTION_GROUP_MST)
        Dim optionGroupRelative As Dictionary(Of String, Object) = GetInnerItem(recommendJson, RECOMMEND_OPTION_GROUP_RELATIVE)
        Dim relarivePartsIdList As List(Of String) = optionGroupRelative.Keys.ToList

        'MOP情報を構築
        For Each partsId As String In relarivePartsIdList
            '表示対象のパーツのみを処理する
            If partsNames.ContainsKey(partsId) Then
                Dim relativeInfo As Dictionary(Of String, Object) = GetInnerItem(optionGroupRelative, partsId)
                Dim relativeAttributeList As List(Of String) = relativeInfo.Keys.ToList

                'オプションに紐付くリコメンド情報が存在するかどうかを判定
                If relativeAttributeList.Count > 0 Then
                    'リコメンド情報が存在する場合
                    For Each attribute As String In relativeAttributeList
                        Dim mopInfo As New MopDopInfo
                        mopInfo.OptionId = partsId
                        mopInfo.OptionKind = OPTION_KIND_MAKER
                        mopInfo.OptionKindName = mopKindName
                        mopInfo.OptionName = partsNames.Item(partsId)
                        mopInfo.Attribute = attribute
                        mopInfo.AttributeName = optionGroupMst.Item(attribute).ToString
                        mopInfo.Order = ToValidOrder(relativeInfo.Item(attribute))
                        mopInfoList.Add(mopInfo)
                    Next
                Else
                    'リコメンド情報が存在しない場合
                    Dim mopInfo As New MopDopInfo
                    mopInfo.OptionId = partsId
                    mopInfo.OptionKind = OPTION_KIND_MAKER
                    mopInfo.OptionKindName = mopKindName
                    mopInfo.OptionName = partsNames.Item(partsId)
                    mopInfo.Attribute = String.Empty
                    mopInfo.AttributeName = ATTRIBUTE_NAME_NOT_EXIST
                    mopInfo.Order = WEIGHT_UNSPECIFIED
                    mopInfoList.Add(mopInfo)
                End If

            End If
        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("mopInfoList", mopInfoList.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        'MOP情報を返却
        Return mopInfoList

    End Function

    ''' <summary>
    ''' DOP情報を取得します。
    ''' </summary>
    ''' <param name="recommendJson">リコメンド情報</param>
    ''' <param name="tcvDealerJsonPath">tcv_dealer.jsonのパス</param>
    ''' <param name="dopKindName">DOP名</param>
    ''' <returns>MOP情報</returns>
    ''' <remarks></remarks>
    Private Function GetDealerOptionInfo(
        ByVal recommendJson As Dictionary(Of String, Object),
        ByVal tcvDealerJsonPath As String,
        ByVal dopKindName As String
    ) As List(Of MopDopInfo)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvDealerJsonPath", tcvDealerJsonPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dopKindName", dopKindName, True))

        '返却するDOP情報
        Dim dopInfoList As New List(Of MopDopInfo)

        '販売店情報取得
        Dim dealerJson As Dictionary(Of String, Object) = GetTcvDealerJson(tcvDealerJsonPath)

        'リコメンド内部情報取得
        Dim optionGroupMst As Dictionary(Of String, Object) = GetInnerItem(recommendJson, RECOMMEND_OPTION_GROUP_MST)

        '販売店内部情報取得
        Dim optionGroupRelative As Dictionary(Of String, Object) = GetInnerItem(dealerJson, DEALER_OPTION_GROUP_RELATIVE)
        Dim relarivePartsIdList As List(Of String) = optionGroupRelative.Keys.ToList

        '販売店情報から表示対象のパーツ情報のみを取得
        Dim partsNames As Dictionary(Of String, String) = GetDealerPartsNames(dealerJson)

        'DOP情報を構築
        For Each partsId As String In relarivePartsIdList
            '表示対象のパーツのみを処理する
            If partsNames.ContainsKey(partsId) Then
                Dim relativeInfo As Dictionary(Of String, Object) = GetInnerItem(optionGroupRelative, partsId)
                Dim relativeAttributeList As List(Of String) = relativeInfo.Keys.ToList

                'オプションに紐付くリコメンド情報が存在するかどうかを判定
                If relativeAttributeList.Count > 0 Then
                    'リコメンド情報が存在する場合
                    For Each attribute As String In relativeAttributeList
                        Dim dopInfo As New MopDopInfo
                        dopInfo.OptionId = partsId
                        dopInfo.OptionKind = OPTION_KIND_DEALER
                        dopInfo.OptionKindName = dopKindName
                        dopInfo.OptionName = partsNames.Item(partsId)
                        dopInfo.Attribute = attribute
                        dopInfo.AttributeName = optionGroupMst.Item(attribute).ToString
                        dopInfo.Order = ToValidOrder(relativeInfo.Item(attribute))
                        dopInfoList.Add(dopInfo)
                    Next
                Else
                    'リコメンド情報が存在しない場合
                    Dim dopInfo As New MopDopInfo
                    dopInfo.OptionId = partsId
                    dopInfo.OptionKind = OPTION_KIND_DEALER
                    dopInfo.OptionKindName = dopKindName
                    dopInfo.OptionName = partsNames.Item(partsId)
                    dopInfo.Attribute = String.Empty
                    dopInfo.AttributeName = ATTRIBUTE_NAME_NOT_EXIST
                    dopInfo.Order = ORDER_MIN_VALUE
                    dopInfoList.Add(dopInfo)
                End If

            End If
        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("dopInfoList", dopInfoList.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        'DOP情報を返却
        Return dopInfoList

    End Function

    ''' <summary>
    ''' tcv_web.jsonから表示対象のパーツ情報を取得します。
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carId">車両ID</param>
    ''' <returns>キーにパーツID、値にパーツ名を持つパーツ情報</returns>
    ''' <remarks></remarks>
    Private Function GetMakerPartsNames(
        ByVal tcvPath As String,
        ByVal carId As String
    ) As Dictionary(Of String, String)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))

        'tcv_web.json情報取得
        Dim tcvWebList As TcvWebListJson = TcvSettingUtilityBusinessLogic.GetTcvWeb(tcvPath, carId)

        '返却するパーツ情報
        Dim partsNames As New Dictionary(Of String, String)

        'パーツ情報を抽出
        For Each parts In tcvWebList.parts
            '表示フラグが表示の場合のみ追加
            If DISPLAY_FLG_VISIBILITY.Equals(parts.btn_flg) Then
                partsNames.Add(parts.id, parts.name)
            End If
        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("partsNames", partsNames.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        'パーツ情報を返却
        Return partsNames

    End Function

    ''' <summary>
    ''' tcv_dealer.jsonの情報を取得します。
    ''' </summary>
    ''' <param name="tcvDealerJsonPath">JSONファイルパス</param>
    ''' <returns>販売店情報</returns>
    ''' <remarks></remarks>
    Private Function GetTcvDealerJson(ByVal tcvDealerJsonPath As String) As Dictionary(Of String, Object)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvDealerJsonPath", tcvDealerJsonPath, False))

        '返却する販売店情報
        Dim dealerJson As Dictionary(Of String, Object) = Nothing

        'ファイルの存在確認
        If File.Exists(tcvDealerJsonPath) Then

            '販売店情報取得
            Dim dealerJsonValue As String = JsonUtilCommon.GetValue(tcvDealerJsonPath)

            '販売店情報を変換
            Dim serializer As New JavaScriptSerializer(New SimpleTypeResolver)
            dealerJson = serializer.Deserialize(Of Dictionary(Of String, Object))(dealerJsonValue)
            Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("recommendJson", dealerJson.ToArray))

        Else
            '販売店情報が存在しない
            dealerJson = New Dictionary(Of String, Object)
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        '販売店情報を返却
        Return dealerJson

    End Function

    ''' <summary>
    ''' 販売店情報から表示対象のパーツ情報を取得します。
    ''' </summary>
    ''' <param name="dealerJson"></param>
    ''' <returns>キーにパーツID、値にパーツ名を持つパーツ情報</returns>
    ''' <remarks></remarks>
    Private Function GetDealerPartsNames(dealerJson As Dictionary(Of String, Object)) As Dictionary(Of String, String)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '返却するパーツ情報
        Dim partsNames As New Dictionary(Of String, String)

        'パーツ情報の有無を確認
        If dealerJson.ContainsKey(DEALER_PARTS) Then
            'パーツ情報を抽出
            Dim serializer As New JavaScriptSerializer
            Dim partsList As List(Of Dictionary(Of String, Object)) = serializer.ConvertToType(Of List(Of Dictionary(Of String, Object)))(dealerJson.Item(DEALER_PARTS))

            For Each parts As Dictionary(Of String, Object) In partsList
                '表示フラグが表示の場合のみ追加
                If DISPLAY_FLG_VISIBILITY.Equals(parts.Item(DEALER_PARTS_BTN_FLG).ToString) Then
                    partsNames.Add(parts.Item(DEALER_PARTS_ID).ToString, parts.Item(DEALER_PARTS_NAME).ToString)
                End If
            Next
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("partsNames", partsNames.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        'パーツ情報を返却
        Return partsNames

    End Function

#End Region

#Region " JSON更新 "

    ''' <summary>
    ''' recommend.jsonに書き込む内容を構築します。
    ''' </summary>
    ''' <param name="mopInfoList">MOP情報一覧</param>
    ''' <param name="recommendJsonPath">recommend.jsonのパス</param>
    ''' <returns>書き込み内容</returns>
    ''' <remarks></remarks>
    Private Function CreateRecommendWriteValue(
        ByVal mopInfoList As List(Of MopDopInfo),
        ByVal recommendJsonPath As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("mopInfoList", mopInfoList.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("recommendJsonPath", recommendJsonPath, False))

        'リコメンド情報取得
        Dim recommendJson As Dictionary(Of String, Object) = GetRecommendJson(recommendJsonPath)

        'リコメンド情報からメーカーオプション関連付けを抽出
        Dim optionGroupRelative As Dictionary(Of String, Object) = GetInnerItem(recommendJson, RECOMMEND_OPTION_GROUP_RELATIVE)

        For Each mopInfo As MopDopInfo In mopInfoList
            '該当オプションに更新値を設定
            Dim options As Dictionary(Of String, Object) = GetInnerItem(optionGroupRelative, mopInfo.OptionId)
            options.Item(mopInfo.Attribute) = mopInfo.Order
        Next

        'JSONファイルに出力する文字列に変換
        Dim sirealizer As New JavaScriptSerializer
        Dim writeValue As String = sirealizer.Serialize(recommendJson)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        '書き込み内容を返却
        Return writeValue

    End Function

    ''' <summary>
    ''' tcv_dealer.jsonに書き込む内容を構築します。
    ''' </summary>
    ''' <param name="dopInfoList">DOP情報一覧</param>
    ''' <param name="tcvDealerJsonPath">tcv_dealer.jsonのパス</param>
    ''' <returns>書き込み内容</returns>
    ''' <remarks></remarks>
    Private Function CreateTcvDealerWriteValue(
        ByVal dopInfoList As List(Of MopDopInfo),
        ByVal tcvDealerJsonPath As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("dopInfoList", dopInfoList.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvDealerJsonPath", tcvDealerJsonPath, False))

        '販売店情報取得
        Dim dealerJson As Dictionary(Of String, Object) = GetTcvDealerJson(tcvDealerJsonPath)

        '販売店情報からメーカーオプション関連付けを抽出
        Dim optionGroupRelative As Dictionary(Of String, Object) = GetInnerItem(dealerJson, DEALER_OPTION_GROUP_RELATIVE)

        For Each dopInfo As MopDopInfo In dopInfoList
            '該当オプションに更新値を設定
            Dim options As Dictionary(Of String, Object) = GetInnerItem(optionGroupRelative, dopInfo.OptionId)
            options.Item(dopInfo.Attribute) = dopInfo.Order
        Next

        'JSONファイルに出力する文字列に変換
        Dim sirealizer As New JavaScriptSerializer
        Dim writeValue As String = sirealizer.Serialize(dealerJson)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        '書き込み内容を返却
        Return writeValue

    End Function

#End Region

#Region " オプション一覧加工 "

    ''' <summary>
    ''' データ取得時のMOP/DOP情報を調整します。
    ''' </summary>
    ''' <param name="mopDopInfoList"></param>
    ''' <remarks></remarks>
    Private Sub AdjustForGet(ByVal mopDopInfoList As List(Of MopDopInfo))

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        'ソート
        mopDopInfoList.Sort(AddressOf CompareGetMopDopInfo)

        'オプションの重みの変換
        SetConversionOrders(mopDopInfoList)

        '属性毎の要素数を設定
        SetCountOfEachAttribute(mopDopInfoList)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' ソート条件を定義します。
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CompareGetMopDopInfo(ByVal x As MopDopInfo, ByVal y As MopDopInfo) As Integer

        Dim result As Integer = 0

        '第一キーの条件
        '┗属性の昇順
        '　未指定の場合は後ろへ
        Dim isEmptyAttrX As Boolean = String.IsNullOrEmpty(x.Attribute)  '比較値Xの属性が未指定かどうか
        Dim isEmptyAttrY As Boolean = String.IsNullOrEmpty(y.Attribute)  '比較値Yの属性が未指定かどうか
        If (isEmptyAttrX AndAlso Not isEmptyAttrY) OrElse
            (Not isEmptyAttrX AndAlso Not isEmptyAttrY) AndAlso x.Attribute > y.Attribute Then
            result = 1
        ElseIf (Not isEmptyAttrX AndAlso isEmptyAttrY) OrElse
            (Not isEmptyAttrX AndAlso Not isEmptyAttrY) AndAlso x.Attribute < y.Attribute Then
            result = -1
        Else
            '第二キーの条件
            '┗表示順の降順
            '未指定の場合は末尾になるよう上限を超える値を設定
            Dim orderX As Integer = x.Order
            Dim orderY As Integer = y.Order
            If orderX > orderY Then
                result = -1
            ElseIf orderX < orderY Then
                result = 1
            Else
                '第三キーの条件
                '┗オプションIDの昇順
                '　[補足]
                '　数値としてソートされるよう型変換を実施する
                '　数値以外の値は存在しない前提であるが
                '　万一の場合を考慮し TryParse でエラー回避する
                '　ただしこの場合の並び順は保証しない
                Dim optionIdX As Integer
                Dim optionIdY As Integer
                If Integer.TryParse(x.OptionId, optionIdX) AndAlso Integer.TryParse(y.OptionId, optionIdY) Then
                    If optionIdX > optionIdY Then
                        result = 1
                    ElseIf optionIdX < optionIdY Then
                        result = -1
                    End If
                End If
            End If
        End If

        Return result

    End Function

    ''' <summary>
    ''' データ更新時のMOP/DOP情報を調整します。
    ''' </summary>
    ''' <param name="mopDopInfoList"></param>
    ''' <remarks></remarks>
    Private Sub AdjustForUpdate(ByVal mopDopInfoList As List(Of MopDopInfo))

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        'ソート
        mopDopInfoList.Sort(AddressOf CompareMopDopInfo)

        '表示順が連番となるよう再設定
        Dim order As Integer = 0
        Dim Attribute As String = ""
        For Each mopDopInfo As MopDopInfo In mopDopInfoList
            '属性が一致しない場合
            If Not mopDopInfo.Attribute.Equals(Attribute) Then
                '属性を設定
                Attribute = mopDopInfo.Attribute
                '表示順をクリア
                order = 0
            End If

            '表示順が設定されている場合
            If mopDopInfo.Order <> WEIGHT_UNSPECIFIED Then
                order += 1
                mopDopInfo.Order = order
            End If
        Next

        '表示順を重みに変換
        SetConversionOrders(mopDopInfoList)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' ソート条件を定義します。
    ''' </summary>
    ''' <param name="x"></param>
    ''' <param name="y"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CompareMopDopInfo(ByVal x As MopDopInfo, ByVal y As MopDopInfo) As Integer

        Dim result As Integer = 0

        '第一キーの条件
        '┗属性の昇順
        '　未指定の場合は後ろへ
        Dim isEmptyAttrX As Boolean = String.IsNullOrEmpty(x.Attribute)  '比較値Xの属性が未指定かどうか
        Dim isEmptyAttrY As Boolean = String.IsNullOrEmpty(y.Attribute)  '比較値Yの属性が未指定かどうか
        If (isEmptyAttrX AndAlso Not isEmptyAttrY) OrElse
            (Not isEmptyAttrX AndAlso Not isEmptyAttrY) AndAlso x.Attribute > y.Attribute Then
            result = 1
        ElseIf (Not isEmptyAttrX AndAlso isEmptyAttrY) OrElse
            (Not isEmptyAttrX AndAlso Not isEmptyAttrY) AndAlso x.Attribute < y.Attribute Then
            result = -1
        Else
            '第二キーの条件
            '┗表示順の昇順
            '未指定の場合は末尾になるよう上限を超える値を設定
            Dim orderX As Integer = x.Order
            Dim orderY As Integer = y.Order
            If orderX = WEIGHT_UNSPECIFIED Then
                orderX = ORDER_MIN_VALUE
            End If
            If orderY = WEIGHT_UNSPECIFIED Then
                orderY = ORDER_MIN_VALUE
            End If
            If orderX > orderY Then
                result = 1
            ElseIf orderX < orderY Then
                result = -1
            Else
                '第三キーの条件
                '┗オプションIDの昇順
                '　[補足]
                '　数値としてソートされるよう型変換を実施する
                '　数値以外の値は存在しない前提であるが
                '　万一の場合を考慮し TryParse でエラー回避する
                '　ただしこの場合の並び順は保証しない
                Dim optionIdX As Integer
                Dim optionIdY As Integer
                If Integer.TryParse(x.OptionId, optionIdX) AndAlso Integer.TryParse(y.OptionId, optionIdY) Then
                    If optionIdX > optionIdY Then
                        result = 1
                    ElseIf optionIdX < optionIdY Then
                        result = -1
                    End If
                End If
            End If
        End If

        Return result

    End Function

    ''' <summary>
    ''' 表示順と重みを相互変換した値を設定します。
    ''' </summary>
    ''' <param name="mopDopInfoList">MOP/DOP情報</param>
    ''' <remarks></remarks>
    Private Sub SetConversionOrders(ByVal mopDopInfoList As List(Of MopDopInfo))
        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("mopDopInfoList", mopDopInfoList.ToArray))

        '設定する付加情報
        Dim maxOrders As New Dictionary(Of String, Integer) '表示順の最大値

        '一覧を走査して付加情報を作成
        For Each mopDopInfo As MopDopInfo In mopDopInfoList
            '既出の属性かどうかを判定
            If maxOrders.ContainsKey(mopDopInfo.Attribute) Then

                '属性毎の表示順の最大値を設定
                If maxOrders.Item(mopDopInfo.Attribute) < mopDopInfo.Order Then
                    maxOrders.Item(mopDopInfo.Attribute) = mopDopInfo.Order
                End If
            Else
                '初期値設定
                maxOrders.Add(mopDopInfo.Attribute, mopDopInfo.Order)
            End If
        Next

        '付加情報を一覧に設定
        For Each mopDopInfo As MopDopInfo In mopDopInfoList

            '属性毎の表示順の最大値を設定
            mopDopInfo.MaxOrderInAttr = maxOrders.Item(mopDopInfo.Attribute)

            '重みから変換した表示順を設定
            mopDopInfo.Order = ConvertOrder(mopDopInfo)

        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' MOP/DOP情報に属性毎の要素数を設定します。
    ''' 事前にソートされている必要があります。
    ''' </summary>
    ''' <param name="mopDopInfoList">MOP/DOP情報</param>
    ''' <remarks></remarks>
    Private Sub SetCountOfEachAttribute(ByVal mopDopInfoList As List(Of MopDopInfo))
        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("mopDopInfoList", mopDopInfoList.ToArray))

        '設定する付加情報
        Dim counts As New Dictionary(Of String, Integer)    '要素数

        '一覧を走査して付加情報を作成
        For Each mopDopInfo As MopDopInfo In mopDopInfoList
            '既出の属性かどうかを判定
            If counts.ContainsKey(mopDopInfo.Attribute) Then
                '属性毎の件数を加算
                counts.Item(mopDopInfo.Attribute) += 1
            Else
                '初期値設定
                counts.Add(mopDopInfo.Attribute, 1)
            End If
        Next

        '付加情報を一覧に設定
        Dim tempAttribute As String = Nothing
        For Each mopDopInfo As MopDopInfo In mopDopInfoList

            '属性毎の要素数を設定
            '┗属性の先頭行のみ該当属性の要素数を設定する
            '　先頭行以外には0を設定する
            If mopDopInfo.Attribute.Equals(tempAttribute) Then
                mopDopInfo.CountInAttr = 0
            Else
                mopDopInfo.CountInAttr = counts.Item(mopDopInfo.Attribute)
            End If

            '属性を保持
            tempAttribute = mopDopInfo.Attribute
        Next

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

#End Region

#Region " ユーティリティ "

    ''' <summary>
    ''' 重みと表示順を相互に変換した値を取得します。
    ''' </summary>
    ''' <param name="mopDopInfo">変換前の値</param>
    ''' <returns>変換後の値</returns>
    ''' <remarks></remarks>
    Private Function ConvertOrder(ByVal mopDopInfo As MopDopInfo) As Integer
        Dim result As Integer
        If mopDopInfo.OptionKind.Equals(OPTION_KIND_MAKER) Then
            If mopDopInfo.Order = WEIGHT_UNSPECIFIED Then
                '未指定の場合は0を設定
                result = WEIGHT_UNSPECIFIED
            Else
                'メーカーオプションの場合は最大値+1から引いた値を設定する
                result = (mopDopInfo.MaxOrderInAttr + 1) - mopDopInfo.Order
            End If
        Else
            If mopDopInfo.Order = WEIGHT_UNSPECIFIED OrElse mopDopInfo.Order = ORDER_MIN_VALUE Then
                '未指定の場合は-1000を設定
                result = ORDER_MIN_VALUE
            Else
                'ディーラーオプションの場合は、表示順 * -1の値を設定する
                result = mopDopInfo.Order * -1
            End If
        End If

        Return result
    End Function

    ''' <summary>
    ''' 表示順を有効な数値に変換します。
    ''' </summary>
    ''' <param name="original">変換前の値</param>
    ''' <returns>変換後の値</returns>
    ''' <remarks></remarks>
    Private Function ToValidOrder(ByVal original As Object) As Integer
        Dim result As Integer
        If Not Integer.TryParse(original.ToString, result) Then
            result = WEIGHT_UNSPECIFIED
        End If
        Return result
    End Function

    ''' <summary>
    ''' ファイルの最終更新日時を取得します。
    ''' </summary>
    ''' <param name="filePath">ファイルパス</param>
    ''' <returns>最終更新日時</returns>
    ''' <remarks></remarks>
    Private Function GetFileTimeStamp(ByVal filePath As String) As String
        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("filePath", filePath, False))
        Dim timestamp As String
        If File.Exists(filePath) Then
            Dim fileInfo As New FileInfo(filePath)
            timestamp = fileInfo.LastWriteTime.ToString(JsonUtilCommon.TimeStampFormat, CultureInfo.InvariantCulture)
        Else
            timestamp = String.Empty
        End If
        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(timestamp))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))
        Return timestamp
    End Function

    ''' <summary>
    ''' 親要素から指定した子要素を取得します。
    ''' </summary>
    ''' <param name="outer">親要素</param>
    ''' <param name="key">取得する子要素のキー</param>
    ''' <returns>子要素</returns>
    ''' <remarks>子要素を正しく取得出来ない場合は空のインスタンスを返します。</remarks>
    Private Function GetInnerItem(ByVal outer As Dictionary(Of String, Object), ByVal key As String) As Dictionary(Of String, Object)

        Dim inner As Dictionary(Of String, Object) = Nothing

        '親要素および子要素の状態確認
        If IsNothing(outer) OrElse Not outer.ContainsKey(key) Then
            '子要素を取得出来ない場合は空のインスタンスを設定
            inner = New Dictionary(Of String, Object)
        Else
            '子要素を取得出来る場合はキャストして設定
            inner = DirectCast(outer.Item(key), Dictionary(Of String, Object))
        End If

        '子要素を返却
        Return inner

    End Function

    ''' <summary>
    ''' 更新リストを作成します。
    ''' </summary>
    ''' <param name="updateJsonPathList">更新JSONファイルパス</param>
    ''' <param name="updateListPath">更新リストファイルパス</param>
    ''' <param name="account">アカウント</param>
    ''' <remarks></remarks>
    Private Sub CallCreateTcvArchiveFile(
        ByVal updateJsonPathList As List(Of String),
        ByVal updateListPath As String,
        ByVal account As String
    )

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("updateListPath", updateListPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("account", account, False))

        '更新リスト情報作成
        Dim parent As New ReplicationFileRoot
        For Each jsonPath As String In updateJsonPathList
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("updateJsonPath", jsonPath, False))
            Dim child As New ReplicationFileInfo
            child.FileAccess = UPDATE_LIST_UPDATE
            child.FilePath = jsonPath
            parent.Root.Add(child)
        Next

        '現在日時取得
        Dim timeStamp As String = DateTimeFunc.FormatDate(UPDATE_LIST_DATE_FORMAT, DateTimeFunc.Now)

        '更新リスト作成
        TcvSettingUtilityBusinessLogic.CreateRepFile(updateListPath, timeStamp, account, parent)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

#End Region

#End Region

End Class
