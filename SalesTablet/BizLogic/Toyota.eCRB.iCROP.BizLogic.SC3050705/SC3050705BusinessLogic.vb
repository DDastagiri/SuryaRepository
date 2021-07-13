Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports System.IO
Imports System.Web
Imports System.Web.Script.Serialization
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.TCVSettingUtility
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Reflection.MethodBase
Imports System.Globalization

''' <summary>
''' MOP/DOP詳細設定のビジネスロジック層
''' </summary>
''' <remarks></remarks>
Public Class SC3050705BusinessLogic

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

    ''' <summary>オプション種別（1：メーカーオプション）</summary>
    Private Const OPTION_KIND_MAKER As String = "1"
    ''' <summary>オプション種別（2：ディーラーオプション）</summary>
    Private Const OPTION_KIND_DEALER As String = "2"

    ''' <summary>販売店情報ファイル:パーツ情報</summary>
    Private Const TCV_DEALER_PARTS As String = "parts"
    ''' <summary>販売店情報ファイル:パーツ情報：パーツID</summary>
    Private Const TCV_DEALER_PARTS_ID As String = "id"
    ''' <summary>販売店情報ファイル:パーツ情報：名称</summary>
    Private Const TCV_DEALER_PARTS_NAME As String = "name"
    ''' <summary>販売店情報ファイル:パーツ情報：塗装済み税込価格</summary>
    Private Const TCV_DEALER_PARTS_PRICE_TT As String = "price_tt"
    ''' <summary>販売店情報ファイル:パーツ情報：塗装済み画像</summary>
    Private Const TCV_DEALER_PARTS_IMG_T As String = "img_t"
    ''' <summary>販売店情報ファイル:パーツ情報：グレード適合</summary>
    Private Const TCV_DEALER_PARTS_GRD As String = "grd"
    ''' <summary>販売店情報ファイル:ディーラーオプション関連付け</summary>
    Private Const TCV_DEALER_OPTION_GROUP_RELATIVE_OPTION As String = "option_group_relative_option"

    ''' <summary>リコメンド情報ファイル:オプショングループマスタ</summary>
    Private Const RECOMMEND_OPTION_GROUP_MST As String = "option_group_mst"
    ''' <summary>リコメンド情報ファイル:メーカーオプション関連付け</summary>
    Private Const RECOMMEND_OPTION_GROUP_RELATIVE_OPTION As String = "option_group_relative_option"

    ''' <summary>
    ''' 更新リスト日付書式
    ''' </summary>
    ''' <remarks></remarks>
    Private Const UPDATE_LIST_DATE_FORMAT As Integer = 15

    ''' <summary>マスグレード (1:デフォルト値)</summary>
    Private Const DEFAULT_MASS_GRADE As String = "1"
    ''' <summary>グレード適合 (2:デフォルト値)</summary>
    Private Const DEFAULT_GRADE_AGREEMENT As String = "2"

    ''' <summary>グレード適合 (1:適合)</summary>
    Private Const GRADE_AGREEMENT As String = "1"

    ''' <summary>処理区分（1：新規）</summary>
    Private Const PROCESS_CD_INSERT As String = "1"
    ''' <summary>処理区分（2：更新）</summary>
    Private Const PROCESS_CD_UPDATE As String = "2"
    ''' <summary>処理区分（3：削除）</summary>
    Private Const PROCESS_CD_DELETE As String = "3"


    ''' <summary>履歴ファイル操作区分[UPDATE]</summary>
    Private Const ACCESS_KBN_UPDATE As String = "UPDATE"
    ''' <summary>履歴ファイル操作区分[ADD]</summary>
    Private Const ACCESS_KBN_ADD As String = "ADD"
    ''' <summary>履歴ファイル操作区分[DELETE]</summary>
    Private Const ACCESS_KBN_DELETE As String = "DELETE"
#End Region

#Region " メソッド "

    ''' <summary>
    ''' オプション情報の取得
    ''' </summary>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="optionKind">オプション種別</param>
    ''' <param name="carId">車種ID</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="tcvHttpPath">TCV URL</param>
    ''' <returns>オプション情報</returns>
    ''' <remarks></remarks>
    Public Function GetOptionInfo(
        ByVal optionId As String,
        ByVal optionKind As String,
        ByVal carId As String,
        ByVal dealerCD As String,
        ByVal tcvPath As String,
        ByVal tcvHttpPath As String
    ) As OptionInfo

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionId", optionId, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionKind", optionKind, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCD, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvHttpPath", tcvHttpPath, True))

        Dim OptionInfo As New OptionInfo

        'オプション種別が"1"【メーカーオプション】の場合
        If OPTION_KIND_MAKER.Equals(optionKind) Then
            'メーカーオプション情報取得
            OptionInfo = GetMakerOptionInfo(
                    optionId,
                    tcvPath,
                    tcvHttpPath,
                    carId
                    )
            'オプション種別が"2"【ディーラーオプション】の場合
        ElseIf OPTION_KIND_DEALER.Equals(optionKind) Then
            'ディーラーオプション情報取得
            OptionInfo = GetDealerOptionInfo(
                    optionId,
                    tcvPath,
                    tcvHttpPath,
                    carId,
                    dealerCD
                    )
        End If

        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return OptionInfo

    End Function

    ''' <summary>
    ''' メーカーオプション情報を取得する
    ''' </summary>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="tcvHttpPath">TCV URL</param>
    ''' <param name="carID">車種ID</param>
    ''' <returns>クラスオブジェクト</returns>    
    ''' <remarks></remarks>
    Private Function GetMakerOptionInfo(
        ByVal optionId As String,
        ByVal tcvPath As String,
        ByVal tcvHttpPath As String,
        ByVal carId As String
    ) As OptionInfo

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvHttpPath", tcvHttpPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))

        '戻り値
        Dim OptionInfo As New OptionInfo

        'tcv_web JSONファイル取得
        Dim tcvWebList As TcvWebListJson =
            TcvSettingUtilityBusinessLogic.GetTcvWeb(
                tcvPath,
                carId)

        'パーツ情報が存在しない場合処理しない。
        If tcvWebList.parts Is Nothing Then
            Return OptionInfo
        End If

        'オプションIDがブランクの場合処理しない。
        If String.IsNullOrEmpty(optionId) Then
            Return OptionInfo
        End If

        'グレード情報.課税タイプの取得
        Dim taxType As String = ""
        Dim indexGrade As Integer = 0
        If Not tcvWebList.grade Is Nothing AndAlso
           tcvWebList.grade.Count > 0 Then

            For indexGrade = 0 To tcvWebList.grade.Count - 1
                If DEFAULT_MASS_GRADE.Equals(tcvWebList.grade(indexGrade).def) Then
                    'マスグレードのデータがあれば課税タイプを保持してループを抜ける
                    taxType = StrConv(tcvWebList.grade(indexGrade).tax, VbStrConv.Lowercase)
                    Exit For
                End If
            Next

        End If

        'ボディカラー情報.カラーコードの取得
        Dim colorCd As String = ""
        If Not tcvWebList.exterior_color Is Nothing AndAlso
           tcvWebList.exterior_color.Count > 0 Then

            For i As Integer = 0 To tcvWebList.exterior_color.Count - 1
                If DEFAULT_GRADE_AGREEMENT.Equals(tcvWebList.exterior_color(i).grd(indexGrade)) Then
                    'グレード適合のデフォルト値のデータがあればカラーコードを保持してループを抜ける
                    colorCd = StrConv(tcvWebList.exterior_color(i).cd, VbStrConv.Lowercase)
                    Exit For
                End If
            Next

        End If

        ' パーツ情報からオプション情報を取得する。
        For Each partsData As TcvWebPartsJson In tcvWebList.parts
            ' パーツ情報の【ID】と引数のオプションIDが一致した場合
            If partsData.id.Equals(optionId) Then
                ' オプションID
                OptionInfo.OptionId = partsData.id
                ' オプション名
                OptionInfo.OptionName = partsData.name
                ' 価格
                OptionInfo.Price = GetPriceInfo(partsData, taxType, colorCd)

                ' 画像ファイルパス
                Dim fileName As String = GetFileName(partsData, colorCd)
                Dim filePath As String
                filePath = TcvSettingConstants.MakerOptionImagePath.Replace(JsonUtilCommon.ReplaceFileString, carId)
                OptionInfo.ImageFilePath = tcvHttpPath + filePath + GetFileName(fileName)
                ' 画像ファイル名
                OptionInfo.ImageFileName = GetFileName(partsData.img_t)
                'グレード適合
                OptionInfo.SetGradeConformity(partsData.grd)

                'ループExit
                Exit For
            End If
        Next

        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return OptionInfo

    End Function

    ''' <summary>
    ''' 価格を取得する
    ''' </summary>
    ''' <param name="partsData">パーツ情報</param>
    ''' <param name="taxType">課税タイプ</param>
    ''' <param name="colorCd">カラーコード</param>
    ''' <returns>価格</returns>    
    ''' <remarks></remarks>
    Private Function GetPriceInfo(
        ByVal partsData As TcvWebPartsJson,
        ByVal taxType As String,
        ByVal colorCd As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("taxType", taxType, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("colorCd", colorCd, True))

        '戻り値
        Dim price As String = String.Empty
        Dim i As Integer = 0
        '課税タイプが"1"（非課税）の場合
        If taxType.Equals("1") Then
            If partsData.col_e_t.Count = 0 Then
                '税抜塗装済み価格
                price = partsData.price_tf
            Else
                Dim isColor As Boolean = False
                For i = 0 To partsData.col_e_t.Count
                    If String.IsNullOrEmpty(colorCd) AndAlso _
                       partsData.col_e_t(i).Equals(colorCd) Then
                        isColor = True
                    End If
                Next

                If isColor Then
                    '税抜塗装済み価格
                    price = partsData.price_tf
                Else
                    '税抜素地価格
                    price = partsData.price_sf
                End If
            End If
        Else
            If partsData.col_e_t.Count = 0 Then
                '税込塗装済み価格
                price = partsData.price_tt
            Else
                Dim isColor As Boolean = False
                For i = 0 To partsData.col_e_t.Count
                    If String.IsNullOrEmpty(colorCd) AndAlso _
                       partsData.col_e_t(i).Equals(colorCd) Then
                        isColor = True
                    End If
                Next

                If isColor Then
                    '税込塗装済み価格
                    price = partsData.price_tt
                Else
                    '税込素地価格
                    price = partsData.price_st
                End If
            End If
        End If
        If Not String.IsNullOrEmpty(price) Then
            price = String.Format(CultureInfo.CurrentCulture, "{0:#,#.##}", CDec(price))
        End If

        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return price

    End Function

    ''' <summary>
    ''' 画像ファイル名を取得する
    ''' </summary>
    ''' <param name="partsData">パーツ情報</param>
    ''' <param name="colorCd">カラーコード</param>
    ''' <returns>価格</returns>    
    ''' <remarks></remarks>
    Private Function GetFileName(
        ByVal partsData As TcvWebPartsJson,
        ByVal colorCd As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("colorCd", colorCd, False))

        '戻り値
        Dim fileName As String = String.Empty
        If partsData.col_e_t.Count = 0 Then
            '塗装済ボタン画像
            fileName = partsData.img_t
        Else
            Dim isColor As Boolean = False
            For i = 0 To partsData.col_e_t.Count
                If String.IsNullOrEmpty(colorCd) AndAlso _
                   partsData.col_e_t(i).Equals(colorCd) Then
                    isColor = True
                End If
            Next

            If isColor Then
                '塗装済ボタン画像
                fileName = partsData.img_t
            Else
                '素地ボタン画像
                fileName = partsData.img_s
            End If
        End If

        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return fileName

    End Function
    ''' <summary>
    ''' 販売店情報を取得する
    ''' </summary>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="tcvHttpPath">TCV URL</param>
    ''' <param name="carID">車種ID</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <returns>クラスオブジェクト</returns>    
    ''' <remarks></remarks>
    Private Function GetDealerOptionInfo(
        ByVal optionId As String,
        ByVal tcvPath As String,
        ByVal tcvHttpPath As String,
        ByVal carId As String,
        ByVal dealerCD As String
    ) As OptionInfo

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionId", optionId, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvHttpPath", tcvHttpPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCD, True))

        '戻り値
        Dim OptionInfo As New OptionInfo

        'オプションIDがブランクの場合処理しない。
        If String.IsNullOrEmpty(optionId) Then
            Return OptionInfo
        End If

        'tcv_dealer JSONファイル取得
        Dim tcvDealerInfo As Dictionary(Of String, Object) = GetTcvDealerJson(tcvPath, carId, dealerCD)
        Dim serializer As New JavaScriptSerializer
        Dim partsList As List(Of Dictionary(Of String, Object)) = serializer.ConvertToType(Of List(Of Dictionary(Of String, Object)))(tcvDealerInfo.Item(TCV_DEALER_PARTS))
        For Each parts As Dictionary(Of String, Object) In partsList
            'パーツ情報の【ID】と引数のオプションIDが一致した場合
            If optionId.Equals(parts.Item(TCV_DEALER_PARTS_ID).ToString) Then
                ' オプションID
                OptionInfo.OptionId = parts.Item(TCV_DEALER_PARTS_ID).ToString
                ' オプション名
                OptionInfo.OptionName = parts.Item(TCV_DEALER_PARTS_NAME).ToString
                ' 価格
                OptionInfo.Price = parts.Item(TCV_DEALER_PARTS_PRICE_TT).ToString
                ' 画像ファイルパス
                Dim filePath As String
                filePath = TcvSettingConstants.DealerOptionImagePath.Replace(JsonUtilCommon.ReplaceFileString, carId)
                filePath = filePath.Replace(JsonUtilCommon.ReplaceFileString2, dealerCD)
                OptionInfo.ImageFilePath = tcvHttpPath + filePath + GetFileName(parts.Item(TCV_DEALER_PARTS_IMG_T).ToString)
                ' 画像ファイル名
                OptionInfo.ImageFileName = GetFileName(parts.Item(TCV_DEALER_PARTS_IMG_T).ToString)
                'グレード適合
                OptionInfo.SetGradeConformity(serializer.ConvertToType(Of List(Of String))(parts.Item(TCV_DEALER_PARTS_GRD)))

                'ループExit
                Exit For
            End If
        Next

        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return OptionInfo

    End Function

    ''' <summary>
    ''' リコメンド情報の取得
    ''' </summary>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="optionKind">オプション種別</param>
    ''' <param name="carId">車種ID</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <returns>オプション情報</returns>
    ''' <remarks></remarks>
    Public Function GetRecommendInfo(
        ByVal optionId As String,
        ByVal optionKind As String,
        ByVal carId As String,
        ByVal dealerCD As String,
        ByVal tcvPath As String
    ) As RecommendInfoList

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionId", optionId, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionKind", optionKind, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCd, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))

        Dim RecommendInfo As New RecommendInfoList

        'オプション種別が"1"【メーカーオプション】の場合
        If OPTION_KIND_MAKER.Equals(optionKind) Then
            'メーカーオプションリコメンド情報取得
            RecommendInfo = GetMakerRecommendInfo(
                    optionId,
                    tcvPath,
                    carId
                    )
            'オプション種別が"2"【ディーラーオプション】の場合
        ElseIf OPTION_KIND_DEALER.Equals(optionKind) Then
            'ディーラーオプションリコメンド情報取得
            RecommendInfo = GetDealerRecommendInfo(
                    optionId,
                    tcvPath,
                    carId,
                    dealerCd
                    )
        End If

        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("RecommendInfo", RecommendInfo.Root.ToArray))

        Return RecommendInfo

    End Function

    ''' <summary>
    ''' メーカーオプションのリコメンド情報を取得
    ''' </summary>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carID">車種ID</param>
    ''' <returns>クラスオブジェクト</returns>    
    ''' <remarks></remarks>
    Private Function GetMakerRecommendInfo(
        ByVal optionId As String,
        ByVal tcvPath As String,
        ByVal carId As String
    ) As RecommendInfoList

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionId", optionId, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))

        '戻り値
        Dim recommendRoot As New RecommendInfoList

        Dim recommendJson As Dictionary(Of String, Object) = GetRecommendJson(tcvPath, carId)
        '新規フラグ
        Dim addFlg As Boolean = False

        'オプションIDがブランクの場合フラグをtrue
        If String.IsNullOrEmpty(optionId) Then
            addFlg = True
        End If

        'リコメンド内部情報取得
        Dim optionGroupMst As Dictionary(Of String, Object) = DirectCast(recommendJson.Item(RECOMMEND_OPTION_GROUP_MST), Dictionary(Of String, Object))
        Dim optionGroupRelative As Dictionary(Of String, Object) = DirectCast(recommendJson.Item(RECOMMEND_OPTION_GROUP_RELATIVE_OPTION), Dictionary(Of String, Object))

        Dim relativeInfo As New Dictionary(Of String, Object)
        If Not addFlg Then
            relativeInfo = DirectCast(optionGroupRelative.Item(optionId), Dictionary(Of String, Object))
        End If
        Dim optionGroupMstKeyList As List(Of String) = optionGroupMst.Keys.ToList

        For Each recommendId As String In optionGroupMstKeyList
            Dim recommendInfo As New RecommendInfo
            recommendInfo.RecommendId = recommendId
            recommendInfo.RecommendName = optionGroupMst.Item(recommendId).ToString
            If addFlg Then
                recommendInfo.RecommendCheck = False
            Else
                If relativeInfo.ContainsKey(recommendId) Then
                    recommendInfo.RecommendCheck = True
                Else
                    recommendInfo.RecommendCheck = False
                End If
            End If
            recommendRoot.Root.Add(recommendInfo)
        Next

        '排他日時の取得
        recommendRoot.TimeStamp = GetFileTimeStamp(JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.RecommendJsonPath, carId))

        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("RecommendInfo", recommendRoot.Root.ToArray))

        Return recommendRoot

    End Function

    ''' <summary>
    ''' ディーラーオプションのリコメンド情報を取得
    ''' </summary>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carID">車種ID</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <returns>クラスオブジェクト</returns>    
    ''' <remarks></remarks>
    Private Function GetDealerRecommendInfo(
        ByVal optionId As String,
        ByVal tcvPath As String,
        ByVal carId As String,
        ByVal dealerCD As String
    ) As RecommendInfoList

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionId", optionId, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCD, True))

        '戻り値
        Dim recommendRoot As New RecommendInfoList

        '新規フラグ
        Dim addFlg As Boolean = False

        'オプションIDがブランクの場合フラグをtrue
        If String.IsNullOrEmpty(optionId) Then
            addFlg = True
        End If

        'リコメンド情報取得
        Dim recommendJson As Dictionary(Of String, Object) = GetRecommendJson(tcvPath, carId)
        '販売店情報取得
        Dim dealerJson As Dictionary(Of String, Object) = GetTcvDealerJson(tcvPath, carId, dealerCD)

        'リコメンド内部情報取得
        Dim optionGroupMst As Dictionary(Of String, Object) = DirectCast(recommendJson.Item(RECOMMEND_OPTION_GROUP_MST), Dictionary(Of String, Object))
        Dim optionGroupRelative As Dictionary(Of String, Object)

        Dim relativeInfo As New Dictionary(Of String, Object)
        If Not addFlg Then
            optionGroupRelative = DirectCast(dealerJson.Item(TCV_DEALER_OPTION_GROUP_RELATIVE_OPTION), Dictionary(Of String, Object))
            relativeInfo = DirectCast(optionGroupRelative.Item(optionId), Dictionary(Of String, Object))
        End If
        Dim optionGroupMstKeyList As List(Of String) = optionGroupMst.Keys.ToList

        For Each recommendId As String In optionGroupMstKeyList
            Dim recommendInfo As New RecommendInfo
            recommendInfo.RecommendId = recommendId
            recommendInfo.RecommendName = optionGroupMst.Item(recommendId).ToString
            If addFlg Then
                recommendInfo.RecommendCheck = False
            Else
                If relativeInfo.ContainsKey(recommendId) Then
                    recommendInfo.RecommendCheck = True
                Else
                    recommendInfo.RecommendCheck = False
                End If
            End If
            recommendRoot.Root.Add(recommendInfo)
        Next

        '排他日時の取得
        recommendRoot.TimeStamp = GetFileTimeStamp(JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.TcvDealerJsonPath, carId, dealerCD))

        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("RecommendInfo", recommendRoot.Root.ToArray))

        Return recommendRoot

    End Function

    ''' <summary>
    ''' オプション情報の更新
    ''' </summary>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="optionKind">オプション種別</param>
    ''' <param name="carId">車種ID</param>
    ''' <param name="dealerCD">販売店コード</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="uploadFile">アップロードファイル</param>
    ''' <returns>メッセージ</returns>
    ''' <remarks></remarks>
    Public Function UpdateOptionInfo(
        ByVal optionInfo As OptionInfo,
        ByVal recommendInfo As RecommendInfoList,
        ByVal optionId As String,
        ByVal optionKind As String,
        ByVal carId As String,
        ByVal dealerCD As String,
        ByVal account As String,
        ByVal tcvPath As String,
        ByVal tcvSettingHistoryFilePath As String,
        ByVal uploadFile As HttpPostedFile,
        ByVal processId As String
    ) As Integer

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("recommendInfo", recommendInfo.Root.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionId", optionId, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionKind", optionKind, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCD, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("account", account, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvSettingHistoryFilePath", tcvSettingHistoryFilePath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("processId", processId, True))

        Dim resultId As Integer = 0
        Dim updatePathList As New ReplicationFileRoot

        'オプション種別が"1"【メーカーオプション】の場合
        If OPTION_KIND_MAKER.Equals(optionKind) Then
            'リコメンド情報の更新
            resultId = UpdateMakerOptionInfo(
                    recommendInfo,
                    optionId,
                    carId,
                    tcvPath,
                    recommendInfo.TimeStamp,
                    updatePathList
                    )
            'オプション種別が"2"【ディーラーオプション】の場合
        ElseIf OPTION_KIND_DEALER.Equals(optionKind) Then
            resultId = UpdateDealerOptionInfo(
                    optionInfo,
                    recommendInfo,
                    optionId,
                    carId,
                    dealerCD,
                    tcvPath,
                    uploadFile,
                    recommendInfo.TimeStamp,
                    processId,
                    updatePathList
                    )
        End If

        '更新リスト作成
        If resultId = 0 Then
            CallCreateTcvArchiveFile(updatePathList, tcvSettingHistoryFilePath, account)
        End If

        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(resultId.ToString(CultureInfo.CurrentCulture)))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return resultId

    End Function

    ''' <summary>
    ''' メーカーオプション情報を更新します。
    ''' </summary>
    ''' <param name="recommnedInfo">リコメンド情報</param>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="carId">車両ID</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <returns>正常時は0、異常時はエラーメッセージIDを返します。</returns>
    ''' <remarks></remarks>
    Private Function UpdateMakerOptionInfo(
        ByVal recommnedInfo As RecommendInfoList,
        ByVal optionId As String,
        ByVal carId As String,
        ByVal tcvPath As String,
        ByVal timeStamp As String,
        ByRef updatePathList As ReplicationFileRoot
    ) As Integer

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("recommendInfo", recommnedInfo.Root.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionId", optionId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("timeStamp", timeStamp, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("updatePathList", updatePathList.Root.ToArray))

        '処理結果
        Dim resultId As Integer = 0
        Dim msgId As String = String.Empty

        'リコメンド情報取得
        Dim fullPath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.RecommendJsonPath, carId)
        Dim recommendJson As Dictionary(Of String, Object) = GetRecommendJson(tcvPath, carId)

        'リコメンド情報からメーカーオプション関連付けを抽出
        Dim optionGroupRelative As Dictionary(Of String, Object) = DirectCast(recommendJson.Item(RECOMMEND_OPTION_GROUP_RELATIVE_OPTION), Dictionary(Of String, Object))
        'オプションIDに紐付くリコメンド情報を取得
        Dim optionRecommend As Dictionary(Of String, Object) = DirectCast(optionGroupRelative.Item(optionId), Dictionary(Of String, Object))

        '該当オプションに値を設定
        Dim recommends As New Dictionary(Of String, Integer)
        For Each recommendInfo As RecommendInfo In recommnedInfo.Root
            'リコメンド属性の選択がある場合
            If recommendInfo.RecommendCheck Then
                'リコメンド属性が存在するか
                If optionRecommend.ContainsKey(recommendInfo.RecommendId) Then
                    recommends.Add(recommendInfo.RecommendId, CInt(optionRecommend.Item(recommendInfo.RecommendId)))
                Else
                    recommends.Add(recommendInfo.RecommendId, 0)
                End If
            End If
        Next
        '要素の更新
        optionGroupRelative.Item(optionId) = recommends

        'JSONファイルに出力する文字列に変換
        Dim sirealizer As New JavaScriptSerializer
        Dim writeValue As String = sirealizer.Serialize(recommendJson)

        'リコメンド情報ファイルに書き込み
        msgId = JsonUtilCommon.SetValue(fullPath, writeValue, timeStamp)

        'リコメンド情報の読み込み/書き込みに失敗
        If Not String.IsNullOrEmpty(msgId) Then
            resultId = CInt(msgId)
        End If

        '更新リスト情報の追加（json）
        Dim updateInfo As New ReplicationFileInfo
        updateInfo.FileAccess = ACCESS_KBN_UPDATE
        updateInfo.FilePath = fullPath.Replace(tcvPath, "")
        updatePathList.Root.Add(updateInfo)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(resultId.ToString(CultureInfo.CurrentCulture)))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        '処理結果を返却
        Return resultId

    End Function

    ''' <summary>
    ''' ディーラーオプション情報を更新します。
    ''' </summary>
    ''' <param name="recommnedInfo">リコメンド情報</param>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="carId">車両ID</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <returns>正常時は0、異常時はエラーメッセージIDを返します。</returns>
    ''' <remarks></remarks>
    Private Function UpdateDealerOptionInfo(
        ByVal optionInfo As OptionInfo,
        ByVal recommnedInfo As RecommendInfoList,
        ByVal optionId As String,
        ByVal carId As String,
        ByVal dealerCD As String,
        ByVal tcvPath As String,
        ByVal uploadFile As HttpPostedFile,
        ByVal timeStamp As String,
        ByVal processId As String,
        ByRef updatePathList As ReplicationFileRoot
    ) As Integer

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("recommendInfo", recommnedInfo.Root.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionId", optionId, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCD, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("processId", processId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("timeStamp", timeStamp, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("updatePathList", updatePathList.Root.ToArray))

        '処理結果
        Dim resultId As Integer = 0

        '処理フラグにより条件分岐
        If processId.Equals(PROCESS_CD_INSERT) Then
            '新規の場合
            '販売店情報の追加を行う。
            resultId = insertDealerInfo(optionInfo,
                             recommnedInfo,
                             optionId,
                             carId,
                             dealerCD,
                             tcvPath,
                             uploadFile,
                             timeStamp,
                             updatePathList)
        ElseIf processId.Equals(PROCESS_CD_UPDATE) Then
            '更新の場合
            '販売店情報の更新を行う。
            resultId = updateDealerInfo(optionInfo,
                             recommnedInfo,
                             optionId,
                             carId,
                             dealerCD,
                             tcvPath,
                             uploadFile,
                             timeStamp,
                             updatePathList)
        ElseIf processId.Equals(PROCESS_CD_DELETE) Then
            '削除の場合
            '販売店情報の削除を行う。
            resultId = deleteDealerInfo(optionId,
                             carId,
                             dealerCD,
                             tcvPath,
                             timeStamp,
                             updatePathList)
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(resultId.ToString(CultureInfo.CurrentCulture)))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        '処理結果を返却
        Return resultId

    End Function

    ''' <summary>
    ''' ディーラーオプション情報を追加します。
    ''' </summary>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="carId">車両ID</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <returns>正常時は0、異常時はエラーメッセージIDを返します。</returns>
    ''' <remarks></remarks>
    Private Function insertDealerInfo(
        ByVal optionInfo As OptionInfo,
        ByVal recommnedInfo As RecommendInfoList,
        ByVal optionId As String,
        ByVal carId As String,
        ByVal dealerCD As String,
        ByVal tcvPath As String,
        ByVal uploadFile As HttpPostedFile,
        ByVal timeStamp As String,
        ByRef updatePathList As ReplicationFileRoot
    ) As Integer

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("recommendInfo", recommnedInfo.Root.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionId", optionId, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCD, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("timeStamp", timeStamp, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("updatePathList", updatePathList.Root.ToArray))

        '処理結果
        Dim resultId As Integer = 0
        Dim msgId As String = String.Empty

        'TcvDelaerの存在チェック
        Dim isFileExsit As Boolean = CheckFileExist(tcvPath, carId, dealerCD)

        'アップロードファイルが存在する場合
        'ファイル名をリネームする
        Dim uploadFileName As String = String.Empty
        If Not uploadFile Is Nothing AndAlso _
           Not String.IsNullOrEmpty(uploadFile.FileName) Then
            uploadFileName = "t" + optionId + Path.GetExtension(uploadFile.FileName)
        End If

        Dim tcvDealerWriteInfo As New Dictionary(Of String, Object)
        Dim tcvDealerInfo As New Dictionary(Of String, Object)
        Dim partsList As New List(Of Dictionary(Of String, Object))
        Dim optionGroupRelative As New Dictionary(Of String, Object)

        Dim serializer As New JavaScriptSerializer
        Dim jsonFileName As String = String.Empty
        If isFileExsit Then
            tcvDealerInfo = GetTcvDealerJson(tcvPath, carId, dealerCD)
            partsList = serializer.ConvertToType(Of List(Of Dictionary(Of String, Object)))(tcvDealerInfo.Item(TCV_DEALER_PARTS))
            optionGroupRelative = DirectCast(tcvDealerInfo.Item(TCV_DEALER_OPTION_GROUP_RELATIVE_OPTION), Dictionary(Of String, Object))
        End If

        Dim parts As New Dictionary(Of String, Object)

        '画像ファイル名判定
        If Not String.IsNullOrEmpty(uploadFileName) Then
            'アップロードファイル名をjsonに書き込む
            jsonFileName = TcvSettingConstants.DealerOptionPath + uploadFileName
        Else
            jsonFileName = String.Empty
        End If

        parts.Add("id", optionId)
        parts.Add("type", "1")
        parts.Add("name", optionInfo.OptionName)
        parts.Add("cd_t", "")
        parts.Add("cd_s", "")
        parts.Add("speckbn", "S")
        parts.Add("div", "")
        parts.Add("price_tt", optionInfo.Price)
        parts.Add("price_tf", "")
        parts.Add("price_st", "")
        parts.Add("price_sf", "")
        parts.Add("cd3", "")
        parts.Add("img_t", jsonFileName)
        parts.Add("img_s", "")
        parts.Add("pb_id", "")
        parts.Add("grd", optionInfo.GradeConformity)
        parts.Add("col_e", New List(Of Object))
        parts.Add("grp", "")
        parts.Add("set", New List(Of Object))
        parts.Add("col_e_t", New List(Of Object))
        parts.Add("btn_flg", "1")
        parts.Add("est_flg", "1")
        parts.Add("img_0", "")
        parts.Add("img_1", "")
        parts.Add("asc", "")
        parts.Add("insert_id", "")

        Dim recommends As New Dictionary(Of String, Integer)
        For Each recommendInfo As RecommendInfo In recommnedInfo.Root
            If recommendInfo.RecommendCheck Then
                recommends.Add(recommendInfo.RecommendId, -1000)
            End If
        Next

        partsList.Add(parts)
        optionGroupRelative.Add(optionId, recommends)

        tcvDealerWriteInfo.Add(TCV_DEALER_PARTS, partsList)
        tcvDealerWriteInfo.Add(TCV_DEALER_OPTION_GROUP_RELATIVE_OPTION, optionGroupRelative)

        'JSONファイルに出力する文字列に変換
        Dim sirealizer As New JavaScriptSerializer
        Dim writeValue As String = sirealizer.Serialize(tcvDealerWriteInfo)

        'ファイルが存在しない場合、ディレクトリ作成
        If Not isFileExsit Then
            Dim jsonFileDirectry As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.TcvDealerJsonDirecty, carId, dealerCD)
            Dim imageFilePath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.DealerOptionImageUploadPath, carId, dealerCD)

            System.IO.Directory.CreateDirectory(jsonFileDirectry)
            System.IO.Directory.CreateDirectory(imageFilePath)
        End If

        '販売店情報ファイルに書き込み
        Dim dealerJsonFullPath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.TcvDealerJsonPath, carId, dealerCD)
        If isFileExsit Then
            msgId = JsonUtilCommon.SetValue(dealerJsonFullPath, writeValue, timeStamp)
        Else
            msgId = JsonUtilCommon.SetValue(dealerJsonFullPath, writeValue)
        End If

        'リコメンド情報の読み込み/書き込みに失敗
        If Not String.IsNullOrEmpty(msgId) Then
            resultId = CInt(msgId)
        End If

        '販売店情報の書き込みに成功した場合
        If resultId = 0 Then
            '更新リスト情報の追加（json）
            Dim updateInfo As New ReplicationFileInfo
            updateInfo.FileAccess = ACCESS_KBN_UPDATE
            updateInfo.FilePath = dealerJsonFullPath.Replace(tcvPath, "")
            updatePathList.Root.Add(updateInfo)
            '画像ファイルの更新
            If Not String.IsNullOrEmpty(uploadFileName) Then
                'アップロードファイルが存在する場合
                'ファイルアップロード
                Dim saveFile As String = Path.Combine(JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.DealerOptionImageUploadPath, carId, dealerCD), uploadFileName)
                uploadFile.SaveAs(saveFile)
                '更新リスト情報の追加（画像）
                updateInfo = New ReplicationFileInfo
                updateInfo.FileAccess = ACCESS_KBN_ADD
                updateInfo.FilePath = saveFile.Replace(tcvPath, "")
                updatePathList.Root.Add(updateInfo)
            End If
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(resultId.ToString(CultureInfo.CurrentCulture)))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        '処理結果を返却
        Return resultId

    End Function

    ''' <summary>
    ''' ディーラーオプション情報を更新します。
    ''' </summary>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="carId">車両ID</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <returns>正常時は0、異常時はエラーメッセージIDを返します。</returns>
    ''' <remarks></remarks>
    Private Function updateDealerInfo(
        ByVal optionInfo As OptionInfo,
        ByVal recommnedInfo As RecommendInfoList,
        ByVal optionId As String,
        ByVal carId As String,
        ByVal dealerCD As String,
        ByVal tcvPath As String,
        ByVal uploadFile As HttpPostedFile,
        ByVal timeStamp As String,
        ByRef updatePathList As ReplicationFileRoot
    ) As Integer

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("recommendInfo", recommnedInfo.Root.ToArray))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionId", optionId, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCD, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("timeStamp", timeStamp, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("updatePathList", updatePathList.Root.ToArray))

        '処理結果
        Dim resultId As Integer = 0
        Dim msgId As String = String.Empty

        'アップロードファイルが存在する場合
        'ファイル名をリネームする
        Dim uploadFileName As String = String.Empty
        If Not uploadFile Is Nothing AndAlso _
           Not String.IsNullOrEmpty(uploadFile.FileName) Then
            uploadFileName = "t" + optionId + Path.GetExtension(uploadFile.FileName)
        End If

        Dim tcvDealerInfo As Dictionary(Of String, Object) = GetTcvDealerJson(tcvPath, carId, dealerCD)
        Dim serializer As New JavaScriptSerializer
        Dim partsList As List(Of Dictionary(Of String, Object)) = serializer.ConvertToType(Of List(Of Dictionary(Of String, Object)))(tcvDealerInfo.Item(TCV_DEALER_PARTS))
        Dim img_t_FileName As String = String.Empty
        Dim imageFileName As String = String.Empty
        Dim jsonFileName As String = String.Empty
        For Each parts As Dictionary(Of String, Object) In partsList
            If optionId.Equals(parts.Item(TCV_DEALER_PARTS_ID).ToString) Then

                ' 塗装済みボタン画像の保持
                img_t_FileName = GetFileName(DirectCast(parts.Item(TCV_DEALER_PARTS_IMG_T), String))
                ' 画面の画像ファイル名
                imageFileName = optionInfo.ImageFileName
                ' パーツ情報の名称
                parts.Item(TCV_DEALER_PARTS_NAME) = optionInfo.OptionName
                ' パーツ情報の塗装済み価格
                parts.Item(TCV_DEALER_PARTS_PRICE_TT) = optionInfo.Price
                ' パーツ情報の塗装済みボタン画像
                '画像ファイル名判定
                If Not String.IsNullOrEmpty(uploadFileName) Then
                    'アップロードファイル名をjsonに書き込む
                    jsonFileName = TcvSettingConstants.DealerOptionPath + uploadFileName
                ElseIf String.IsNullOrEmpty(uploadFileName) AndAlso _
                       Not String.IsNullOrEmpty(imageFileName) Then
                    '塗装済みファイル名をjsonに書き込む
                    jsonFileName = TcvSettingConstants.DealerOptionPath + img_t_FileName
                Else
                    jsonFileName = String.Empty
                End If

                parts.Item(TCV_DEALER_PARTS_IMG_T) = jsonFileName
                'グレード適合
                parts.Item(TCV_DEALER_PARTS_GRD) = optionInfo.GradeConformity
                'ループExit
                Exit For
            End If
        Next
        Dim optionGroupRelative As Dictionary(Of String, Object) = DirectCast(tcvDealerInfo.Item(TCV_DEALER_OPTION_GROUP_RELATIVE_OPTION), Dictionary(Of String, Object))

        'オプションIDに紐付くリコメンド情報を取得
        Dim optionRecommend As Dictionary(Of String, Object) = DirectCast(optionGroupRelative.Item(optionId), Dictionary(Of String, Object))

        '該当オプションに値を設定
        Dim recommends As New Dictionary(Of String, Integer)
        For Each recommendInfo As RecommendInfo In recommnedInfo.Root
            'リコメンド属性の選択がある場合
            If recommendInfo.RecommendCheck Then
                'リコメンド属性が存在するか
                If optionRecommend.ContainsKey(recommendInfo.RecommendId) Then
                    recommends.Add(recommendInfo.RecommendId, CInt(optionRecommend.Item(recommendInfo.RecommendId)))
                Else
                    recommends.Add(recommendInfo.RecommendId, -1000)
                End If
            End If
        Next
        '要素の更新
        optionGroupRelative.Item(optionId) = recommends

        'JSONファイルに出力する文字列に変換
        Dim sirealizer As New JavaScriptSerializer
        Dim writeValue As String = sirealizer.Serialize(tcvDealerInfo)

        '販売店情報ファイルに書き込み
        Dim dealerJsonFullPath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.TcvDealerJsonPath, carId, dealerCD)
        msgId = JsonUtilCommon.SetValue(dealerJsonFullPath, writeValue, timeStamp)

        'リコメンド情報の読み込み/書き込みに失敗
        If Not String.IsNullOrEmpty(msgId) Then
            resultId = CInt(msgId)
        End If

        '販売店情報の書き込みに成功した場合
        If resultId = 0 Then
            '更新リスト情報の追加（json）
            Dim updateInfo As New ReplicationFileInfo
            updateInfo.FileAccess = ACCESS_KBN_UPDATE
            updateInfo.FilePath = dealerJsonFullPath.Replace(tcvPath, "")
            updatePathList.Root.Add(updateInfo)
            '画像ファイルの更新
            If Not String.IsNullOrEmpty(uploadFileName) Then
                'アップロードファイルが存在する場合
                Dim AccessStauts As String = String.Empty
                'ファイル名の差異判定
                If String.Equals(uploadFileName, imageFileName) Then
                    '更新
                    AccessStauts = ACCESS_KBN_UPDATE
                Else
                    'ADD
                    AccessStauts = ACCESS_KBN_ADD
                End If

                'ファイルアップロード
                Dim saveFile As String = Path.Combine(JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.DealerOptionImageUploadPath, carId, dealerCD), uploadFileName)
                uploadFile.SaveAs(saveFile)
                '更新リスト情報の追加（画像）
                updateInfo = New ReplicationFileInfo
                updateInfo.FileAccess = AccessStauts
                updateInfo.FilePath = saveFile.Replace(tcvPath, "")
                updatePathList.Root.Add(updateInfo)
                '削除ファイルがあるか判定
                If Not String.IsNullOrEmpty(img_t_FileName) AndAlso _
                   Not AccessStauts.Equals(ACCESS_KBN_UPDATE) Then
                    Dim deleteFile As New FileInfo(Path.Combine(JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.DealerOptionImageUploadPath, carId, dealerCD), img_t_FileName))
                    deleteFile.Delete()
                    '更新リスト情報の追加（画像）
                    updateInfo = New ReplicationFileInfo
                    updateInfo.FileAccess = ACCESS_KBN_DELETE
                    updateInfo.FilePath = deleteFile.FullName.Replace(tcvPath, "")
                    updatePathList.Root.Add(updateInfo)
                End If
            Else
                'アップロードファイルが存在しない場合
                '画面の画像ファイル名がブランク且つ、
                '塗装済み画像ファイル名がブランク以外の場合、削除
                If String.IsNullOrEmpty(imageFileName) AndAlso _
                   Not String.IsNullOrEmpty(img_t_FileName) Then
                    Dim deleteFile As New FileInfo(Path.Combine(JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.DealerOptionImageUploadPath, carId, dealerCD), img_t_FileName))
                    deleteFile.Delete()
                    '更新リスト情報の追加（画像）
                    updateInfo = New ReplicationFileInfo
                    updateInfo.FileAccess = ACCESS_KBN_DELETE
                    updateInfo.FilePath = deleteFile.FullName.Replace(tcvPath, "")
                    updatePathList.Root.Add(updateInfo)
                End If

            End If
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(resultId.ToString(CultureInfo.CurrentCulture)))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        '処理結果を返却
        Return resultId

    End Function

    ''' <summary>
    ''' ディーラーオプション情報を削除します。
    ''' </summary>
    ''' <param name="optionId">オプションID</param>
    ''' <param name="carId">車両ID</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <returns>正常時は0、異常時はエラーメッセージIDを返します。</returns>
    ''' <remarks></remarks>
    Private Function deleteDealerInfo(
        ByVal optionId As String,
        ByVal carId As String,
        ByVal dealerCD As String,
        ByVal tcvPath As String,
        ByVal timeStamp As String,
        ByRef updatePathList As ReplicationFileRoot
    ) As Integer

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("optionId", optionId, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCD, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("timeStamp", timeStamp, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetCountLog("updatePathList", updatePathList.Root.ToArray))

        '処理結果
        Dim resultId As Integer = 0
        Dim msgId As String = String.Empty

        Dim tcvDealerInfo As Dictionary(Of String, Object) = GetTcvDealerJson(tcvPath, carId, dealerCD)
        Dim serializer As New JavaScriptSerializer
        Dim partsList As List(Of Dictionary(Of String, Object)) = serializer.ConvertToType(Of List(Of Dictionary(Of String, Object)))(tcvDealerInfo.Item(TCV_DEALER_PARTS))
        Dim delIndex As Integer = 0
        Dim img_t_FileName As String = String.Empty
        For Each parts As Dictionary(Of String, Object) In partsList
            If optionId.Equals(parts.Item(TCV_DEALER_PARTS_ID).ToString) Then

                '塗装済み画像
                img_t_FileName = parts.Item(TCV_DEALER_PARTS_IMG_T).ToString()

                'ループExit
                Exit For
            End If
            'インデックスカウントアップ
            delIndex += 1
        Next
        Dim optionGroupRelative As Dictionary(Of String, Object) = DirectCast(tcvDealerInfo.Item(TCV_DEALER_OPTION_GROUP_RELATIVE_OPTION), Dictionary(Of String, Object))
        'パーツ情報の削除
        partsList.RemoveAt(delIndex)
        'パーツ情報の再設定
        tcvDealerInfo.Item(TCV_DEALER_PARTS) = partsList
        'リコメンド情報の削除
        optionGroupRelative.Remove(optionId)

        'JSONファイルに出力する文字列に変換
        Dim sirealizer As New JavaScriptSerializer
        Dim writeValue As String = sirealizer.Serialize(tcvDealerInfo)

        '販売店情報ファイルに書き込み
        Dim dealerJsonFullPath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.TcvDealerJsonPath, carId, dealerCD)
        msgId = JsonUtilCommon.SetValue(dealerJsonFullPath, writeValue, timeStamp)

        '販売店情報情報の読み込み/書き込みに失敗
        If Not String.IsNullOrEmpty(msgId) Then
            resultId = CInt(msgId)
        End If

        '販売店情報の書き込みに成功した場合
        If resultId = 0 Then
            '更新リスト情報の追加（json）
            Dim updateInfo As New ReplicationFileInfo
            updateInfo.FileAccess = ACCESS_KBN_UPDATE
            updateInfo.FilePath = dealerJsonFullPath.Replace(tcvPath, "")
            updatePathList.Root.Add(updateInfo)
            '画像ファイルの削除
            '画像ファイル名が存在した場合はファイル削除を行う。
            If Not String.IsNullOrEmpty(img_t_FileName) Then
                Dim optionFile As New FileInfo(Path.Combine(JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.DealerOptionImageUploadPath, carId, dealerCD), GetFileName(img_t_FileName)))
                optionFile.Delete()
                '更新リスト情報の追加（画像）
                updateInfo = New ReplicationFileInfo
                updateInfo.FileAccess = ACCESS_KBN_DELETE
                updateInfo.FilePath = optionFile.FullName.Replace(tcvPath, "")
                updatePathList.Root.Add(updateInfo)
            End If
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(resultId.ToString(CultureInfo.CurrentCulture)))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        '処理結果を返却
        Return resultId

    End Function

    ''' <summary>
    ''' ファイルパスからファイル名の取得を行う
    ''' </summary>
    ''' <param name="filePath">ファイルパス</param>
    ''' <returns>ファイル名</returns>
    ''' <remarks></remarks>
    Private Function GetFileName(
        ByVal filePath As String
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("filePath", filePath, False))

        Dim result As String = String.Empty
        If Not filePath Is Nothing Then
            'Nothingでない場合はパスからファイル名を取得する
            result = Path.GetFileName(filePath)

            If filePath.Equals(result) Then
                'パスと取得したファイル名が一致する場合、変換できないパスなので空文字にする
                result = String.Empty
            End If
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(result))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return result

    End Function

    ''' <summary>
    ''' tcv_dealer.jsonの情報を取得します。
    ''' </summary>
    ''' <param name="tcvPath">tcv_dealer.jsonのパス</param>
    ''' <param name="carId">メッセージID</param>
    ''' <param name="dealerCD">メッセージID</param>
    ''' <returns>販売店情報</returns>
    ''' <remarks></remarks>
    Private Function CheckFileExist(
        ByVal tcvPath As String,
        ByVal carId As String,
        ByVal dealerCD As String
    ) As Boolean

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCD, True))

        Dim dealerJsonFullPath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.TcvDealerJsonPath, carId, dealerCD)

        Dim result As Boolean

        'ファイルの存在確認
        If File.Exists(dealerJsonFullPath) Then
            result = True
        Else
            result = False
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '販売店情報を返却
        Return result

    End Function

    ''' <summary>
    ''' tcv_dealer.jsonの情報を取得します。
    ''' </summary>
    ''' <param name="tcvPath">tcv_dealer.jsonのパス</param>
    ''' <param name="carId">メッセージID</param>
    ''' <param name="dealerCD">メッセージID</param>
    ''' <returns>販売店情報</returns>
    ''' <remarks></remarks>
    Private Function GetTcvDealerJson(
        ByVal tcvPath As String,
        ByVal carId As String,
        ByVal dealerCD As String
    ) As Dictionary(Of String, Object)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("dealerCD", dealerCD, True))

        Dim dealerJsonFullPath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.TcvDealerJsonPath, carId, dealerCD)

        Dim tcvDealerJson As Dictionary(Of String, Object) = Nothing

        'ファイルの存在確認
        If File.Exists(dealerJsonFullPath) Then
            '販売店情報取得
            Dim tcvDealerJsonValue As String = JsonUtilCommon.GetValue(dealerJsonFullPath)

            '販売店情報を変換
            Dim serializer As New JavaScriptSerializer(New SimpleTypeResolver)
            tcvDealerJson = serializer.Deserialize(Of Dictionary(Of String, Object))(tcvDealerJsonValue)
        Else
            '販売店情報が存在しない
            tcvDealerJson = Nothing
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        '販売店情報を返却
        Return tcvDealerJson

    End Function

    ''' <summary>
    ''' recommend.jsonの情報を取得します。
    ''' </summary>
    ''' <param name="tcvPath">recommend.jsonのパス</param>
    ''' <param name="carId">メッセージID</param>
    ''' <returns>リコメンド情報</returns>
    ''' <remarks></remarks>
    Private Function GetRecommendJson(
        ByVal tcvPath As String,
        ByRef carId As String
    ) As Dictionary(Of String, Object)

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))

        Dim recommendJsonFullPath As String = JsonUtilCommon.GetJsonFilePath(tcvPath, TcvSettingConstants.RecommendJsonPath, carId)
        'リコメンド情報取得
        Dim recommendJsonValue As String = JsonUtilCommon.GetValue(recommendJsonFullPath)
        Dim recommendJson As Dictionary(Of String, Object) = Nothing

        'リコメンド情報を変換
        Dim serializer As New JavaScriptSerializer(New SimpleTypeResolver)
        recommendJson = serializer.Deserialize(Of Dictionary(Of String, Object))(recommendJsonValue)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        'リコメンド情報を返却
        Return recommendJson

    End Function

    ''' <summary>
    ''' 更新リストを作成します。
    ''' </summary>
    ''' <param name="updatePathList">更新JSONファイルパス</param>
    ''' <param name="updateListPath">更新リストファイルパス</param>
    ''' <param name="account">アカウント</param>
    ''' <remarks></remarks>
    Private Sub CallCreateTcvArchiveFile(
        ByVal updatePathList As ReplicationFileRoot,
        ByVal updateListPath As String,
        ByVal account As String
    )

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("updateListPath", updateListPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("account", account, True))

        '現在日時取得
        Dim timeStamp As String = DateTimeFunc.FormatDate(UPDATE_LIST_DATE_FORMAT, DateTimeFunc.Now)

        '更新リスト作成
        TcvSettingUtilityBusinessLogic.CreateRepFile(updateListPath, timeStamp, account, updatePathList)

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

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
    ''' IDの最大値を取得します。
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <returns>IDの最大値</returns>
    ''' <remarks></remarks>
    Public Function GetMaxId(ByVal tcvPath As String,
                              ByVal carId As String,
                              ByVal dealerCD As String) As Integer

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))

        Dim tcvDealerJson As Dictionary(Of String, Object) = GetTcvDealerJson(tcvPath, carId, dealerCD)
        Dim maxId As Integer = 0
        If tcvDealerJson Is Nothing Then
            maxId = 1000
        Else
            Dim serializer As New JavaScriptSerializer
            Dim partsList As List(Of Dictionary(Of String, Object)) = serializer.ConvertToType(Of List(Of Dictionary(Of String, Object)))(tcvDealerJson.Item(TCV_DEALER_PARTS))

            For Each parts As Dictionary(Of String, Object) In partsList
                Dim id As Integer
                If Integer.TryParse(parts.Item(TCV_DEALER_PARTS_ID).ToString, id) Then
                    If maxId < id Then
                        maxId = id
                    End If
                End If
            Next
            'IDのカウントアップ
            maxId = maxId + 1
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(maxId.ToString(CultureInfo.InvariantCulture)))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return maxId

    End Function
#End Region

End Class
