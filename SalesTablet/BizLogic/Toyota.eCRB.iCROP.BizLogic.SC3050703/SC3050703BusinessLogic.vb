Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports System.IO
Imports System.Web
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.TCVSettingUtility
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Reflection.MethodBase
Imports System.Globalization

''' <summary>
''' セールスポイント詳細設定のビジネスロジック層
''' </summary>
''' <remarks></remarks>
Public Class SC3050703BusinessLogic

#Region " コンストラクタ "
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        REM
    End Sub
#End Region

#Region " 置換する項目名 "
    ''' <summary>置換する項目名</summary>
    Private REPLACE_VALUES()() As String = {
        New String() {"view-type", "viewtype"},
        New String() {"interior-id", "interiorid"},
        New String() {"overview-title", "overviewtitle"},
        New String() {"overview-contents", "overviewcontents"},
        New String() {"overview-top", "overviewtop"},
        New String() {"overview-left", "overviewleft"},
        New String() {"overview-img", "overviewimg"},
        New String() {"popup-type", "popuptype"},
        New String() {"popup-title", "popuptitle"},
        New String() {"popup-contents", "popupcontents"},
        New String() {"fullscreen-popup-src", "fullscreenpopupsrc"},
        New String() {"popup-src", "popupsrc"}
    }
#End Region

#Region " 定数 "
    ''' <summary>タイプ エクステリア</summary>
    Public Const TypeExterior As String = "exterior"
    ''' <summary>タイプ インテリア</summary>
    Public Const TypeInterior As String = "interior"

    ''' <summary>削除区分</summary>
    Private Const DELETE_DIV As String = "0"

    ''' <summary>画像パス作成時の置き換え前の文字</summary>
    Private Const PATH_REPLACE_BEFORE As String = "/"
    ''' <summary>画像パス作成時の置き換え後の文字</summary>
    Private Const PATH_REPLACE_AFTER As String = "_"
    ''' <summary>画像パス作成時の接続文字</summary>
    Private Const PATH_CONNECT_CHAR As String = "_0_"
    ''' <summary>画像パス作成時の拡張子</summary>
    Private Const PATH_CONNECT_EXTENSION As String = ".png"

    ''' <summary>マスグレード (1:デフォルト値)</summary>
    Private Const DEFAULT_MASS_GRADE As String = "1"
    ''' <summary>グレード適合 (2:デフォルト値)</summary>
    Private Const DEFAULT_GRADE_AGREEMENT As String = "2"

    ''' <summary>削除区分 ON</summary>
    Private Const DEL_DVS_ON As String = "1"
    ''' <summary>削除区分 OFF</summary>
    Private Const DEL_DVS_OFF As String = "0"

    ''' <summary>履歴ファイル操作区分[UPDATE]</summary>
    Private Const SOUSA_KUBUN_UPDATE As String = "UPDATE"

    ''' <summary>履歴ファイル操作区分[ADD]</summary>
    Private Const SOUSA_KUBUN_ADD As String = "ADD"

    ''' <summary>履歴ファイル操作区分[DELETE]</summary>
    Private Const SOUSA_KUBUN_DELETE As String = "DELETE"

    ''' <summary>拡張子[mp4]</summary>
    Private Const EXT_MP4 As String = ".mp4"

    ''' <summary>拡張子[mov]</summary>
    Private Const EXT_MOV As String = ".mov"

    ''' <summary>拡張子[MP4]</summary>
    Private Const EXT_MP4_BIG As String = ".MP4"

    ''' <summary>拡張子[MOV]</summary>
    Private Const EXT_MOV_BIG As String = ".MOV"

#End Region

#Region " メソッド "
    ''' <summary>
    ''' 外装画像サムネイル情報の取得
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="tcvKasoPath">TCV仮想パス</param>
    ''' <param name="carID">車種ID</param>
    ''' <returns>外装画像サムネイル情報</returns>
    ''' <remarks></remarks>
    Public Function GetExteriorImageInfo(
        ByVal tcvPath As String,
        ByVal tcvKasoPath As String,
        ByVal carId As String
    ) As ThumbnailInfoList

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvKasoPath", tcvKasoPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))


        'tcv_web JSONファイル取得
        Dim tcvWebList As TcvWebListJson =
            TcvSettingUtilityBusinessLogic.GetTcvWeb(
                tcvPath,
                carId)

        '取得情報のチェック
        Dim thumbnailList As New ThumbnailInfoList
        If tcvWebList.player_info Is Nothing OrElse
           tcvWebList.player_info.introduction Is Nothing OrElse
           tcvWebList.player_info.introduction.angles Is Nothing Then
            'アングル数が取得できない場合は処理しない
            Return thumbnailList
        End If


        'グレード情報.型式の取得
        Dim model As String = ""
        Dim indexGrade As Integer = 0
        If Not tcvWebList.grade Is Nothing AndAlso
           tcvWebList.grade.Count > 0 Then
            'グレード情報が存在する場合は、型式の取得を行う
            Dim isExistsModel As Boolean = False

            For indexGrade = 0 To tcvWebList.grade.Count - 1
                If DEFAULT_MASS_GRADE.Equals(tcvWebList.grade(indexGrade).def) Then
                    'マスグレードのデータがあれば型式を保持してループを抜ける
                    model = StrConv(tcvWebList.grade(indexGrade).model, VbStrConv.Lowercase)
                    isExistsModel = True
                    Exit For
                End If
            Next

            If Not isExistsModel Then
                model = StrConv(tcvWebList.grade(0).model, VbStrConv.Lowercase)
            End If

        End If

        'ボディカラー情報.カラーコードの取得
        Dim colorCd As String = ""
        If Not tcvWebList.exterior_color Is Nothing AndAlso
           tcvWebList.exterior_color.Count > 0 Then
            'ボディカラー情報が存在する場合は、カラーコードの取得を行う
            Dim isExistsColor As Boolean = False

            For i As Integer = 0 To tcvWebList.exterior_color.Count - 1
                If DEFAULT_GRADE_AGREEMENT.Equals(tcvWebList.exterior_color(i).grd(indexGrade)) Then
                    'グレード適合のデフォルト値のデータがあればカラーコードを保持してループを抜ける
                    colorCd = StrConv(tcvWebList.exterior_color(i).cd, VbStrConv.Lowercase)
                    isExistsColor = True
                    Exit For
                End If
            Next

            If Not isExistsColor Then
                colorCd = StrConv(tcvWebList.exterior_color(0).cd, VbStrConv.Lowercase)
            End If

        End If


        '再生環境情報のアングル数だけ処理
        For Each angle As Integer In tcvWebList.player_info.introduction.angles

            '画像ファイル名の作成
            Dim imgFileName As New StringBuilder
            imgFileName.Append(colorCd)
            imgFileName.Append(PATH_REPLACE_BEFORE)
            imgFileName.Append(model.Replace(PATH_REPLACE_BEFORE, PATH_REPLACE_AFTER))
            imgFileName.Append(PATH_CONNECT_CHAR)
            imgFileName.Append(CType(angle, String))
            imgFileName.Append(PATH_CONNECT_EXTENSION)


            '情報追加
            Dim thumbnailData As New ThumbnailInfo
            Dim imgFilePath As New StringBuilder
            imgFilePath.Append(tcvKasoPath)
            imgFilePath.Append(TcvSettingConstants.SalespointExteriorImagePath)
            imgFilePath.Replace(JsonUtilCommon.ReplaceFileString, carId)

            thumbnailData.Id = CType(angle, String)
            thumbnailData.ThumbnailPath = imgFilePath.ToString & imgFileName.ToString
            thumbnailData.GridPath = imgFilePath.ToString & imgFileName.ToString

            thumbnailList.ThumbnailInfo.Add(thumbnailData)

        Next


        '終了ログ出力
        Logger.Info(GetReturnThumbnailInfo(thumbnailList))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return thumbnailList

    End Function

    ''' <summary>
    ''' 内装画像サムネイル情報の取得
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="tcvKasoPath">TCV仮想パス</param>
    ''' <param name="carID">車種ID</param>
    ''' <returns>内装画像サムネイル情報</returns>
    ''' <remarks></remarks>
    Public Function GetInteriorImageInfo(
        ByVal tcvPath As String,
        ByVal tcvKasoPath As String,
        ByVal carId As String
    ) As ThumbnailInfoList

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvKasoPath", tcvKasoPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))


        'ファイル名取得
        Dim file As String = JsonUtilCommon.GetJsonFilePath(
                                tcvPath,
                                TcvSettingConstants.InteriorJsonPath,
                                carId
                             )

        'JSONデータを文字列として取得
        '(書込みがないので排他エラーがない)
        Dim readValue As String = JsonUtilCommon.GetValue(file)

        'シリアライザ生成
        Dim serializer As DataContractJsonSerializer = New DataContractJsonSerializer(GetType(InteriorListJson))


        '置換したデータをクラスオブジェクト化
        Dim interiorList As InteriorListJson = Nothing
        Using readStream As New MemoryStream(Encoding.UTF8.GetBytes(readValue))
            Dim readObject As Object = serializer.ReadObject(readStream)
            interiorList = TryCast(readObject, InteriorListJson)
        End Using

        '取得情報のチェック
        Dim thumbnailList As New ThumbnailInfoList
        If interiorList.interior Is Nothing Then
            Return thumbnailList
        End If


        'インテリア情報の数だけ処理
        For Each interiorData As InteriorJson In interiorList.interior

            'サムネイル画像パスの作成
            Dim thumbnailPath As New StringBuilder
            thumbnailPath.Append(tcvKasoPath)
            thumbnailPath.Append(TcvSettingConstants.SalespointInteriorImagePath)
            thumbnailPath.Append(interiorData.img_back)


            'グリッド画像パスの作成
            Dim gridPath As New StringBuilder
            gridPath.Append(tcvKasoPath)
            gridPath.Append(TcvSettingConstants.SalespointInteriorImagePath)
            gridPath.Append(interiorData.img_back)


            '情報追加
            Dim thumbnailData As New ThumbnailInfo

            thumbnailData.Id = interiorData.id
            thumbnailData.ThumbnailPath = thumbnailPath.ToString.Replace(JsonUtilCommon.ReplaceFileString, carId)
            thumbnailData.GridPath = gridPath.ToString.Replace(JsonUtilCommon.ReplaceFileString, carId)

            thumbnailList.ThumbnailInfo.Add(thumbnailData)

        Next


        '終了ログ出力
        Logger.Info(GetReturnThumbnailInfo(thumbnailList))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return thumbnailList

    End Function

    ''' <summary>
    ''' JSONファイルを読み込み、クラスオブジェクトに変換します。
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carID">車種ID</param>
    ''' <param name="type">エクステリア/インテリア</param>
    ''' <param name="targetID">対象セールスポイントID(新規はブランク)</param>
    ''' <param name="targetNo">対象セールスポイント番号(新規はブランク)</param>
    ''' <returns>クラスオブジェクト</returns>    
    ''' <remarks></remarks>
    Public Function GetSalesPointInfo(
        ByVal tcvPath As String,
        ByVal carId As String,
        ByVal type As String,
        ByVal targetId As String,
        ByVal targetNo As String
    ) As SalesPointListJson

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("type", type, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("targetId", targetId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("targetNo", targetNo, True))

        '戻り値
        Dim salesPointInfoList As New SalesPointListJson

        Try
            'ファイル名取得
            Dim file As String = JsonUtilCommon.GetJsonFilePath(
                                    tcvPath,
                                    TcvSettingConstants.SalesPointJsonPath,
                                    carId
                                 )

            'JSONデータを文字列として取得
            Dim readValue As String = JsonUtilCommon.GetValue(file)

            'プロパティ名を置換
            readValue = JsonUtilCommon.ReplaceProperty(readValue, REPLACE_VALUES)

            'シリアライザ生成
            Dim serializer As DataContractJsonSerializer = New DataContractJsonSerializer(GetType(SalesPointListJson))


            '置換したデータをクラスオブジェクト化
            Dim salesPointList As SalesPointListJson = Nothing
            Using readStream As New MemoryStream(Encoding.UTF8.GetBytes(readValue))
                Dim readObject As Object = serializer.ReadObject(readStream)
                salesPointList = TryCast(readObject, SalesPointListJson)
            End Using


            '取得した情報から エクステリア/インテリア の情報を取得する
            Dim no As Integer = 0
            Dim maxID As Integer = 0
            For Each salesPointData As SalesPointJson In salesPointList.sales_point

                'IDの最大値を保持
                Dim currentID As Integer
                If (Not salesPointData.id Is Nothing AndAlso
                    Integer.TryParse(salesPointData.id, currentID)) Then
                    'IDが取得できており、数値に変換できる場合は保持している値と比較
                    If maxID < currentID Then
                        maxID = currentID
                    End If

                End If

                If type.Equals(salesPointData.type) Then

                    '相対パスからファイルパスを取得
                    Dim overviewFile As String = String.Empty
                    Dim popupFile As String = String.Empty
                    Dim fullscreenPopupFile As String = String.Empty

                    'オーバーレイ（画像）のファイル名を取得する
                    overviewFile = GetFileName(salesPointData.overviewimg)

                    'ポップアップ（画像）のファイル名を取得する
                    popupFile = GetFileName(salesPointData.popupsrc)

                    'フルスクリーンポップアップ（画像）のファイル名を取得する
                    fullscreenPopupFile = GetFileName(salesPointData.fullscreenpopupsrc)

                    '連番インクリメント
                    no += 1

                    'JSONファイルから取得できない値を編集して、戻り値のリストに追加する
                    salesPointData.SortNo = no                                                  'ソートno
                    salesPointData.No = CType(no, String)                                       'no
                    salesPointData.OverviewFile = overviewFile                                  'オーバーレイファイル名
                    salesPointData.PopupFile = popupFile                                        'ポップアップファイル名
                    salesPointData.FullscreenPopupFile = fullscreenPopupFile                    'フルスクリーンポップアップファイル名
                    salesPointInfoList.sales_point.Add(salesPointData)                          'JSONファイルの１データを追加

                End If

            Next

            'ファイル更新日時取得
            Dim jsonFileInfo As New FileInfo(file)
            salesPointInfoList.TimeStamp = jsonFileInfo.LastWriteTime.ToString(JsonUtilCommon.TimeStampFormat, CultureInfo.InvariantCulture)

            '対象セールスポイントIDの設定
            If targetId Is Nothing OrElse
               targetId.Trim.Length = 0 Then
                'ブランクの場合はIDの最大値＋１を設定
                salesPointInfoList.TargetId = CType(maxID + 1, String)
            Else
                salesPointInfoList.TargetId = targetId
            End If

            '対象セールスポイント番号の設定
            If targetNo Is Nothing OrElse
               targetNo.Trim.Length = 0 Then
                'ブランクの場合はNOの最大値(＝salesPointInfoListの件数)＋１を設定
                salesPointInfoList.TargetNo = CType(salesPointInfoList.sales_point.Count + 1, String)
            Else
                salesPointInfoList.TargetNo = targetNo
            End If

        Finally
            '終了ログ出力
            Logger.Info(GetReturnSalesPointJson(salesPointInfoList))
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        End Try

        Return salesPointInfoList

    End Function

    ''' <summary>
    ''' クラスオブジェクトをJSONファイルに出力します。
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carID">車種ID</param>
    ''' <param name="type">エクステリア/インテリア</param>
    ''' <param name="targetID">対象セールスポイントID</param>
    ''' <param name="salesPointList">セールスポイント情報データセット</param>
    ''' <param name="outlineImgFile">概要アップロードファイル</param>
    ''' <param name="detailImgFile">詳細アップロードファイル</param>
    ''' <param name="detailPopupImgFile">詳細(拡大画像)アップロードファイル</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Public Function UpdateSalesPointInfoSend(
        ByVal tcvPath As String,
        ByVal carId As String,
        ByVal type As String,
        ByVal targetId As String,
        ByVal salesPointList As SalesPointListJson,
        ByVal outlineImgFile As System.Web.HttpPostedFile,
        ByVal detailImgFile As System.Web.HttpPostedFile,
        ByVal detailPopupImgFile As System.Web.HttpPostedFile
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("type", type, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("targetId", targetId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam(
                    "salesPointList",
                    TcvSettingUtilityBusinessLogic.GetCountLog("sales_point", salesPointList),
                    True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("outlineImgFile", outlineImgFile.FileName, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("detailImgFile", detailImgFile.FileName, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("detailPopupImgFile", detailPopupImgFile.FileName, True))

        '戻り値
        Dim msgID As String = ""

        Try
            '受け取ったデータに含まれていないデータの取得
            If TypeExterior.Equals(type) Then
                '受け取ったデータが外装なら、JSONファイルから内装を取得してくる
                Dim salesPointInterior As SalesPointListJson =
                    GetSalesPointInfo(
                        tcvPath,
                        carId,
                        TypeInterior,
                        String.Empty,
                        String.Empty)

                '外装情報 → 内装情報の順で格納する
                salesPointList.sales_point.AddRange(salesPointInterior.sales_point)

            Else
                '受け取ったデータが内装なら、JSONファイルから外装を取得してくる
                Dim salesPointExterior As SalesPointListJson =
                    GetSalesPointInfo(
                        tcvPath,
                        carId,
                        TypeExterior,
                        String.Empty,
                        String.Empty)

                '外装情報 → 内装情報の順で格納する
                salesPointList.sales_point.InsertRange(0, salesPointExterior.sales_point)

            End If

            'シリアライザ生成
            Dim serializer As DataContractJsonSerializer = New DataContractJsonSerializer(GetType(SalesPointListJson))

            '出力情報取得
            Dim writeValue As String = JsonUtilCommon.GetWriteValue(salesPointList, serializer, REPLACE_VALUES)


            'ファイル名取得
            Dim file As String = JsonUtilCommon.GetJsonFilePath(
                                    tcvPath,
                                    TcvSettingConstants.SalesPointJsonPath,
                                    carId
                                 )

            msgID = JsonUtilCommon.SetValue(file, writeValue, salesPointList.TimeStamp)
            If msgID.Trim.Length > 0 Then
                'メッセージIDが返された場合はエラーなので、画像のアップロード処理を行わない
                Exit Try
            End If


            'アップロード先のファイル名取得
            Dim imgFile As String = ""
            imgFile = Path.Combine(tcvPath, TcvSettingConstants.SalespointUploadPath).Replace(JsonUtilCommon.ReplaceFileString, carId)

            '画像のアップロード処理
            For Each salesPointData As SalesPointJson In salesPointList.sales_point

                If Not targetId.Equals(salesPointData.id) Then
                    '対象セールスポイントIDに該当するIDでない場合は処理しない。次のデータへ
                    Continue For
                End If

                'アップロード
                UploadImageFile(
                    imgFile,
                    salesPointData,
                    outlineImgFile,
                    detailImgFile,
                    detailPopupImgFile,
                    DEL_DVS_OFF)


                'IDは一意なので、アップロード処理が完了したら処理を終わる
                Exit For

            Next

        Finally
            '終了ログ出力
            Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(msgID))
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        End Try

        Return msgID

    End Function

    ''' <summary>
    ''' セールスポイント情報の削除
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carID">車種ID</param>
    ''' <param name="type">エクステリア/インテリア</param>
    ''' <param name="targetID">対象セールスポイントID</param>
    ''' <param name="salesPointList">セールスポイント情報データセット</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Public Function DeleteSalesPointInfoSend(
        ByVal tcvPath As String,
        ByVal carId As String,
        ByVal type As String,
        ByVal targetId As String,
        ByVal salesPointList As SalesPointListJson
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("type", type, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("targetId", targetId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam(
                    "salesPointList",
                    TcvSettingUtilityBusinessLogic.GetCountLog("sales_point", salesPointList),
                    True))

        '戻り値
        Dim msgID As String = ""

        Try
            '削除対象のデータを省く
            Dim salesPointDelete(0) As SalesPointJson
            For i As Integer = 0 To salesPointList.sales_point.Count - 1

                Dim salesPointData As SalesPointJson = salesPointList.sales_point(i)

                If Not targetId.Equals(salesPointData.id) Then
                    '対象セールスポイントIDに該当するIDでない場合は次のデータへ
                    Continue For
                End If

                'セールスポイント情報データセットからデータを省く
                salesPointList.sales_point.CopyTo(0, salesPointDelete, 0, 1)
                salesPointList.sales_point.Remove(salesPointData)

                'IDは一意なので、アップロード処理が完了したら処理を終わる
                Exit For

            Next

            '受け取ったデータに含まれていないデータの取得
            If TypeExterior.Equals(type) Then
                '受け取ったデータが外装なら、JSONファイルから内装を取得してくる
                Dim salesPointInterior As SalesPointListJson =
                    GetSalesPointInfo(
                        tcvPath,
                        carId,
                        TypeInterior,
                        String.Empty,
                        String.Empty)

                '外装情報 → 内装情報の順で格納する
                salesPointList.sales_point.AddRange(salesPointInterior.sales_point)

            Else
                '受け取ったデータが内装なら、JSONファイルから外装を取得してくる
                Dim salesPointExterior As SalesPointListJson =
                    GetSalesPointInfo(
                        tcvPath,
                        carId,
                        TypeExterior,
                        String.Empty,
                        String.Empty)

                '外装情報 → 内装情報の順で格納する
                salesPointList.sales_point.InsertRange(0, salesPointExterior.sales_point)

            End If

            'シリアライザ生成
            Dim serializer As DataContractJsonSerializer = New DataContractJsonSerializer(GetType(SalesPointListJson))

            '出力情報取得
            Dim writeValue As String = JsonUtilCommon.GetWriteValue(salesPointList, serializer, REPLACE_VALUES)


            'ファイル名取得
            Dim file As String = JsonUtilCommon.GetJsonFilePath(
                                    tcvPath,
                                    TcvSettingConstants.SalesPointJsonPath,
                                    carId
                                 )

            Dim result As String =
                JsonUtilCommon.SetValue(file, writeValue, salesPointList.TimeStamp)
            If result.Trim.Length > 0 Then
                'メッセージIDが返された場合はエラーなので、画像の削除処理を行わない
                Return result
            End If

            If salesPointDelete(0) Is Nothing Then
                '削除対象のデータが存在しない場合は、画像の削除処理を行わない
                Exit Try
            End If

            'アップロード先のファイル名取得
            Dim imgFile As String
            imgFile = Path.Combine(tcvPath, TcvSettingConstants.SalespointUploadPath).Replace(JsonUtilCommon.ReplaceFileString, carId)

            'アップロード
            UploadImageFile(
                imgFile,
                salesPointDelete(0),
                Nothing,
                Nothing,
                Nothing,
                DEL_DVS_ON)

        Finally
            '終了ログ出力
            Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(msgID))
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        End Try

        Return msgID

    End Function

    ''' <summary>
    ''' 画像アップロード処理
    ''' </summary>
    ''' <param name="imgFile">アップロード先のパス</param>
    ''' <param name="salesPointData">セールスポイント情報(1データ分)</param>
    ''' <param name="outlineImgFile">概要アップロードファイル(Nothingは削除のみ)</param>
    ''' <param name="detailImgFile">詳細アップロードファイル(Nothingは削除のみ)</param>
    ''' <param name="detailPopupImgFile">詳細(拡大画像)アップロードファイル(Nothingは削除のみ)</param>
    ''' <remarks></remarks>
    Private Sub UploadImageFile(
        ByVal imgFile As String,
        ByVal salesPointData As SalesPointJson,
        ByVal outlineImgFile As System.Web.HttpPostedFile,
        ByVal detailImgFile As System.Web.HttpPostedFile,
        ByVal detailPopupImgFile As System.Web.HttpPostedFile,
        ByVal deleteDvs As String
    )
        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("imgFile", imgFile, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam(
                    "salesPointData",
                    TcvSettingUtilityBusinessLogic.GetCountLog("sales_point", salesPointData),
                    True))
        If outlineImgFile Is Nothing Then
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("outlineImgFile", "", True))
        Else
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("outlineImgFile", outlineImgFile.FileName, True))
        End If
        If detailImgFile Is Nothing Then
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("detailImgFile", "", True))
        Else
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("detailImgFile", detailImgFile.FileName, True))
        End If
        If detailPopupImgFile Is Nothing Then
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("detailPopupImgFile", "", True))
        Else
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("detailPopupImgFile", detailPopupImgFile.FileName, True))
        End If
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("deleteDvs", CStr(deleteDvs), True))

        '編集/削除判定
        If DEL_DVS_ON.Equals(deleteDvs) Then
            '削除の場合
            If Not String.IsNullOrEmpty(salesPointData.OverviewFile) Then
                'オーバーレイ ファイル削除
                Dim overviewFile As New FileInfo(Path.Combine(imgFile, salesPointData.OverviewFile))
                overviewFile.Delete()
            End If

            If Not String.IsNullOrEmpty(salesPointData.PopupFile) Then
                'ポップアップ ファイル削除
                Dim popupFile As New FileInfo(Path.Combine(imgFile, salesPointData.PopupFile))
                popupFile.Delete()
            End If

            If Not String.IsNullOrEmpty(salesPointData.FullscreenPopupFile) Then
                'フルスクリーンポップアップ ファイル削除
                Dim fullscreenPopupFile As New FileInfo(Path.Combine(imgFile, salesPointData.FullscreenPopupFile))
                fullscreenPopupFile.Delete()
            End If

        Else
            '編集の場合
            'オーバーレイイメージ
            If String.IsNullOrEmpty(salesPointData.OverviewFile) And Not String.IsNullOrEmpty(outlineImgFile.FileName) Then
                'オーバーレイ ファイルのアップロード
                Dim overviewUploadFile As String = Path.Combine(imgFile, GetFileName(salesPointData.overviewimg))
                outlineImgFile.SaveAs(overviewUploadFile)
            ElseIf Not String.IsNullOrEmpty(salesPointData.OverviewFile) And Not String.IsNullOrEmpty(outlineImgFile.FileName) Then
                'オーバーレイ ファイル削除
                Dim overviewFile As New FileInfo(Path.Combine(imgFile, salesPointData.OverviewFile))
                overviewFile.Delete()
                'オーバーレイ ファイルのアップロード
                Dim overviewUploadFile As String = Path.Combine(imgFile, GetFileName(salesPointData.overviewimg))
                outlineImgFile.SaveAs(overviewUploadFile)

            ElseIf Not String.IsNullOrEmpty(salesPointData.OverviewFile) And String.IsNullOrEmpty(outlineImgFile.FileName) And String.IsNullOrEmpty(salesPointData.overviewimg) Then
                'オーバーレイ ファイル削除
                Dim overviewFile As New FileInfo(Path.Combine(imgFile, salesPointData.OverviewFile))
                overviewFile.Delete()
            End If

            'ポップアップイメージ
            If String.IsNullOrEmpty(salesPointData.PopupFile) And Not String.IsNullOrEmpty(detailImgFile.FileName) Then
                'ポップアップファイルのアップロード
                Dim popupUploadFile As String = Path.Combine(imgFile, GetFileName(salesPointData.popupsrc))
                detailImgFile.SaveAs(popupUploadFile)
            ElseIf Not String.IsNullOrEmpty(salesPointData.PopupFile) And Not String.IsNullOrEmpty(detailImgFile.FileName) Then
                'ポップアップ ファイル削除
                Dim popupFile As New FileInfo(Path.Combine(imgFile, salesPointData.PopupFile))
                popupFile.Delete()
                'ポップアップファイルのアップロード
                Dim popupUploadFile As String = Path.Combine(imgFile, GetFileName(salesPointData.popupsrc))
                detailImgFile.SaveAs(popupUploadFile)

            ElseIf Not String.IsNullOrEmpty(salesPointData.PopupFile) And String.IsNullOrEmpty(detailImgFile.FileName) And String.IsNullOrEmpty(salesPointData.popupsrc) Then
                'ポップアップ ファイル削除
                Dim popupFile As New FileInfo(Path.Combine(imgFile, salesPointData.PopupFile))
                popupFile.Delete()
            End If

            'フルスクリーンポップアップイメージ
            If String.IsNullOrEmpty(salesPointData.FullscreenPopupFile) And Not String.IsNullOrEmpty(detailPopupImgFile.FileName) Then
                'フルスクリーンポップアップ ファイルのアップロード
                Dim fullscreenPopupUploadFile As String = Path.Combine(imgFile, GetFileName(salesPointData.fullscreenpopupsrc))
                detailPopupImgFile.SaveAs(fullscreenPopupUploadFile)
            ElseIf Not String.IsNullOrEmpty(salesPointData.FullscreenPopupFile) And Not String.IsNullOrEmpty(detailPopupImgFile.FileName) Then
                'フルスクリーンポップアップ ファイル削除
                Dim fullscreenPopupFile As New FileInfo(Path.Combine(imgFile, salesPointData.FullscreenPopupFile))
                fullscreenPopupFile.Delete()
                'フルスクリーンポップアップ ファイルのアップロード
                Dim fullscreenPopupUploadFile As String = Path.Combine(imgFile, GetFileName(salesPointData.fullscreenpopupsrc))
                detailPopupImgFile.SaveAs(fullscreenPopupUploadFile)

            ElseIf Not String.IsNullOrEmpty(salesPointData.FullscreenPopupFile) And String.IsNullOrEmpty(detailPopupImgFile.FileName) And String.IsNullOrEmpty(salesPointData.fullscreenpopupsrc) Then
                'フルスクリーンポップアップ ファイル削除
                If Not EXT_MP4.Equals(System.IO.Path.GetExtension(salesPointData.FullscreenPopupFile)) _
                    And Not EXT_MP4_BIG.Equals(System.IO.Path.GetExtension(salesPointData.FullscreenPopupFile)) _
                    And Not EXT_MOV.Equals(System.IO.Path.GetExtension(salesPointData.FullscreenPopupFile)) _
                    And Not EXT_MOV_BIG.Equals(System.IO.Path.GetExtension(salesPointData.FullscreenPopupFile)) Then
                    '動画ファイル以外の場合削除実行する
                    Dim fullscreenPopupFile As New FileInfo(Path.Combine(imgFile, salesPointData.FullscreenPopupFile))
                    fullscreenPopupFile.Delete()
                End If
            End If
        End If

        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

    End Sub

    ''' <summary>
    ''' 履歴ファイル作成処理
    ''' </summary>
    ''' <param name="carSeries">車種シリーズ</param>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="tcvSettingHistoryFilePath">履歴ファイル格納パス</param>
    ''' <param name="targetSalesPointID">対象セールスポイントID</param>
    ''' <param name="salesPointList">セールスポイント情報</param>
    ''' <param name="timeStamp">タイムスタンプ</param>
    ''' <param name="account">アカウント</param>
    ''' <param name="delDvs">削除区分</param>
    ''' <remarks></remarks>
    Public Sub CallCreateTcvArchiveFile(ByVal carSeries As String, _
                                         ByVal tcvPath As String, _
                                         ByVal tcvSettingHistoryFilePath As String, _
                                         ByVal targetSalesPointId As String, _
                                         ByVal salesPointList As SalesPointListJson, _
                                         ByVal timeStamp As String, _
                                         ByVal account As String, _
                                         ByVal delDvs As String, _
                                         ByVal summaryFile As String, _
                                         ByVal detailFile As String, _
                                         ByVal detailPopupFile As String, _
                                         ByVal summaryDispFile As String, _
                                         ByVal detailDispFile As String, _
                                         ByVal detailPopupDispFile As String)

        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("CallCreateTcvArchiveFile", True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carSeries", carSeries, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvSettingHistoryFilePath", tcvSettingHistoryFilePath, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("targetSalesPointID", targetSalesPointId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam(
                    "salesPointList",
                    TcvSettingUtilityBusinessLogic.GetCountLog("salesPointList", salesPointList),
                    True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("timeStamp", timeStamp, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("account", account, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("delDvs", delDvs, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("summaryFile", summaryFile, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("detailFile", detailFile, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("detailPopupFile", detailPopupFile, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("summaryDispFile", summaryDispFile, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("detailDispFile", detailDispFile, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("detailPopupDispFile", detailPopupDispFile, True))

        Dim tcvSalesPointUploadPath As String = TcvSettingConstants.SalespointUploadPath
        Dim filePath As String = tcvSalesPointUploadPath.Replace(JsonUtilCommon.ReplaceFileString, carSeries)

        Dim repFileRoot As New ReplicationFileRoot

        Dim repFileInfo As New ReplicationFileInfo

        '編集の場合
        For Each salesPointData As SalesPointJson In salesPointList.sales_point

            If Not targetSalesPointId.Equals(salesPointData.id) Then
                '対象セールスポイントIDに該当するIDでない場合は処理しない。次のデータへ
                Continue For
            End If

            'セールスポイントJSON
            Dim salesPointJsonPath As String = TcvSettingConstants.SalesPointJsonPath
            repFileInfo.FileAccess = SOUSA_KUBUN_UPDATE
            repFileInfo.FilePath = salesPointJsonPath.Replace(JsonUtilCommon.ReplaceFileString, carSeries)

            repFileRoot.Root.Add(repFileInfo)

            '編集/削除判定
            If DEL_DVS_OFF.Equals(delDvs) Then
                '保存の場合
                '新規/編集判定
                If String.IsNullOrEmpty(targetSalesPointId) Then
                    '新規の場合
                    'オーバーレイ(画像)
                    If Not String.IsNullOrEmpty(salesPointData.overviewimg) Then
                        repFileInfo = New ReplicationFileInfo
                        repFileInfo.FileAccess = SOUSA_KUBUN_ADD
                        repFileInfo.FilePath = Path.Combine(filePath, GetFileName(salesPointData.overviewimg))

                        repFileRoot.Root.Add(repFileInfo)

                    End If

                    'ポップアップ(画像)
                    If Not String.IsNullOrEmpty(salesPointData.popupsrc) Then
                        repFileInfo = New ReplicationFileInfo
                        repFileInfo.FileAccess = SOUSA_KUBUN_ADD
                        repFileInfo.FilePath = Path.Combine(filePath, GetFileName(salesPointData.popupsrc))

                        repFileRoot.Root.Add(repFileInfo)

                    End If

                    'フルスクリーンポップアップ(画像)
                    If Not String.IsNullOrEmpty(salesPointData.fullscreenpopupsrc) Then
                        repFileInfo = New ReplicationFileInfo
                        repFileInfo.FileAccess = SOUSA_KUBUN_ADD
                        repFileInfo.FilePath = Path.Combine(filePath, GetFileName(salesPointData.fullscreenpopupsrc))

                        repFileRoot.Root.Add(repFileInfo)

                    End If

                Else
                    '編集の場合
                    'オーバーレイ(画像)
                    If Not String.IsNullOrEmpty(summaryFile) And Not String.IsNullOrEmpty(salesPointData.OverviewFile) Then
                        If salesPointData.OverviewFile.Equals(GetFileName(salesPointData.overviewimg)) Then
                            repFileInfo = New ReplicationFileInfo
                            repFileInfo.FileAccess = SOUSA_KUBUN_UPDATE
                            repFileInfo.FilePath = Path.Combine(filePath, GetFileName(salesPointData.overviewimg))

                            repFileRoot.Root.Add(repFileInfo)
                        Else
                            repFileInfo = New ReplicationFileInfo
                            repFileInfo.FileAccess = SOUSA_KUBUN_DELETE
                            repFileInfo.FilePath = Path.Combine(filePath, salesPointData.OverviewFile)

                            repFileRoot.Root.Add(repFileInfo)

                            repFileInfo = New ReplicationFileInfo
                            repFileInfo.FileAccess = SOUSA_KUBUN_ADD
                            repFileInfo.FilePath = Path.Combine(filePath, GetFileName(salesPointData.overviewimg))

                            repFileRoot.Root.Add(repFileInfo)
                        End If
                    End If

                    If Not String.IsNullOrEmpty(summaryFile) And String.IsNullOrEmpty(salesPointData.OverviewFile) Then
                        repFileInfo = New ReplicationFileInfo
                        repFileInfo.FileAccess = SOUSA_KUBUN_ADD
                        repFileInfo.FilePath = Path.Combine(filePath, GetFileName(salesPointData.overviewimg))

                        repFileRoot.Root.Add(repFileInfo)
                    End If

                    If String.IsNullOrEmpty(summaryFile) And Not String.IsNullOrEmpty(salesPointData.OverviewFile) And String.IsNullOrEmpty(summaryDispFile) Then
                        repFileInfo = New ReplicationFileInfo
                        repFileInfo.FileAccess = SOUSA_KUBUN_DELETE
                        repFileInfo.FilePath = Path.Combine(filePath, salesPointData.OverviewFile)

                        repFileRoot.Root.Add(repFileInfo)
                    End If

                    'ポップアップ(画像)
                    If Not String.IsNullOrEmpty(detailFile) And Not String.IsNullOrEmpty(salesPointData.PopupFile) Then
                        If salesPointData.PopupFile.Equals(GetFileName(salesPointData.popupsrc)) Then
                            repFileInfo = New ReplicationFileInfo
                            repFileInfo.FileAccess = SOUSA_KUBUN_UPDATE
                            repFileInfo.FilePath = Path.Combine(filePath, GetFileName(salesPointData.popupsrc))

                            repFileRoot.Root.Add(repFileInfo)
                        Else
                            repFileInfo = New ReplicationFileInfo
                            repFileInfo.FileAccess = SOUSA_KUBUN_DELETE
                            repFileInfo.FilePath = Path.Combine(filePath, salesPointData.PopupFile)

                            repFileRoot.Root.Add(repFileInfo)

                            repFileInfo = New ReplicationFileInfo
                            repFileInfo.FileAccess = SOUSA_KUBUN_ADD
                            repFileInfo.FilePath = Path.Combine(filePath, GetFileName(salesPointData.popupsrc))

                            repFileRoot.Root.Add(repFileInfo)
                        End If
                    End If

                    If Not String.IsNullOrEmpty(detailFile) And String.IsNullOrEmpty(salesPointData.PopupFile) Then
                        repFileInfo = New ReplicationFileInfo
                        repFileInfo.FileAccess = SOUSA_KUBUN_ADD
                        repFileInfo.FilePath = Path.Combine(filePath, GetFileName(salesPointData.popupsrc))

                        repFileRoot.Root.Add(repFileInfo)
                    End If

                    If String.IsNullOrEmpty(detailFile) And Not String.IsNullOrEmpty(salesPointData.PopupFile) And String.IsNullOrEmpty(detailDispFile) Then
                        repFileInfo = New ReplicationFileInfo
                        repFileInfo.FileAccess = SOUSA_KUBUN_DELETE
                        repFileInfo.FilePath = Path.Combine(filePath, salesPointData.PopupFile)

                        repFileRoot.Root.Add(repFileInfo)
                    End If

                    'フルスクリーンポップアップ(画像)
                    If Not String.IsNullOrEmpty(detailPopupFile) And Not String.IsNullOrEmpty(salesPointData.FullscreenPopupFile) Then
                        If salesPointData.FullscreenPopupFile.Equals(GetFileName(salesPointData.fullscreenpopupsrc)) Then
                            repFileInfo = New ReplicationFileInfo
                            repFileInfo.FileAccess = SOUSA_KUBUN_UPDATE
                            repFileInfo.FilePath = Path.Combine(filePath, GetFileName(salesPointData.fullscreenpopupsrc))

                            repFileRoot.Root.Add(repFileInfo)
                        Else
                            repFileInfo = New ReplicationFileInfo
                            repFileInfo.FileAccess = SOUSA_KUBUN_DELETE
                            repFileInfo.FilePath = Path.Combine(filePath, salesPointData.FullscreenPopupFile)

                            repFileRoot.Root.Add(repFileInfo)

                            repFileInfo = New ReplicationFileInfo
                            repFileInfo.FileAccess = SOUSA_KUBUN_ADD
                            repFileInfo.FilePath = Path.Combine(filePath, GetFileName(salesPointData.fullscreenpopupsrc))

                            repFileRoot.Root.Add(repFileInfo)
                        End If
                    End If

                    If Not String.IsNullOrEmpty(detailPopupFile) And String.IsNullOrEmpty(salesPointData.FullscreenPopupFile) Then
                        repFileInfo = New ReplicationFileInfo
                        repFileInfo.FileAccess = SOUSA_KUBUN_ADD
                        repFileInfo.FilePath = Path.Combine(filePath, GetFileName(salesPointData.fullscreenpopupsrc))

                        repFileRoot.Root.Add(repFileInfo)
                    End If

                    If String.IsNullOrEmpty(detailPopupFile) And Not String.IsNullOrEmpty(salesPointData.FullscreenPopupFile) And String.IsNullOrEmpty(detailPopupDispFile) Then
                        If Not EXT_MP4.Equals(System.IO.Path.GetExtension(salesPointData.FullscreenPopupFile)) _
                            And Not EXT_MP4_BIG.Equals(System.IO.Path.GetExtension(salesPointData.FullscreenPopupFile)) _
                            And Not EXT_MOV.Equals(System.IO.Path.GetExtension(salesPointData.FullscreenPopupFile)) _
                            And Not EXT_MOV_BIG.Equals(System.IO.Path.GetExtension(salesPointData.FullscreenPopupFile)) Then
                            '動画以外の場合、削除する
                            repFileInfo = New ReplicationFileInfo
                            repFileInfo.FileAccess = SOUSA_KUBUN_DELETE
                            repFileInfo.FilePath = Path.Combine(filePath, salesPointData.FullscreenPopupFile)

                            repFileRoot.Root.Add(repFileInfo)
                        End If

                    End If

                End If

            Else
                '削除の場合
                'オーバーレイ(画像)
                If Not String.IsNullOrEmpty(salesPointData.OverviewFile) Then
                    repFileInfo = New ReplicationFileInfo
                    repFileInfo.FileAccess = SOUSA_KUBUN_DELETE
                    repFileInfo.FilePath = Path.Combine(filePath, salesPointData.OverviewFile)

                    repFileRoot.Root.Add(repFileInfo)

                End If

                'ポップアップ(画像)
                If Not String.IsNullOrEmpty(salesPointData.PopupFile) Then
                    repFileInfo = New ReplicationFileInfo
                    repFileInfo.FileAccess = SOUSA_KUBUN_DELETE
                    repFileInfo.FilePath = Path.Combine(filePath, salesPointData.PopupFile)

                    repFileRoot.Root.Add(repFileInfo)

                End If

                'フルスクリーンポップアップ(画像)
                If Not String.IsNullOrEmpty(salesPointData.FullscreenPopupFile) Then
                    repFileInfo = New ReplicationFileInfo
                    repFileInfo.FileAccess = SOUSA_KUBUN_DELETE
                    repFileInfo.FilePath = Path.Combine(filePath, salesPointData.FullscreenPopupFile)

                    repFileRoot.Root.Add(repFileInfo)

                End If

            End If


            'IDは一意なので、処理が完了したら処理を終わる
            Exit For

        Next

        TcvSettingUtilityBusinessLogic.CreateRepFile(tcvSettingHistoryFilePath, timeStamp, account, repFileRoot)

        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod("CallCreateTcvArchiveFile", False))

    End Sub


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


        Dim result As String = ""
        If Not filePath Is Nothing Then
            'Nothingでない場合はパスからファイル名を取得する
            result = Path.GetFileName(filePath)

            If filePath.Equals(result) Then
                'パスと取得したファイル名が一致する場合、変換できないパスなので空文字にする
                result = ""
            End If
        End If


        '終了ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(result))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return result

    End Function

    ''' <summary>
    ''' sales_point 格納クラスログ出力
    ''' </summary>
    ''' <param name="paramJsonData">car_lineup JSONファイル</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Function GetReturnSalesPointJson(
        ByVal paramJsonData As SalesPointListJson
    ) As String
        Dim sb As New StringBuilder
        With sb
            .Append("Return SalesPointListJson =")
            '全データ
            If paramJsonData Is Nothing Then
                .Append(TcvSettingUtilityBusinessLogic.GetCountLog("sales_point", paramJsonData))
            Else
                .Append(TcvSettingUtilityBusinessLogic.GetCountLog("sales_point", paramJsonData.sales_point.ToArray))
            End If
        End With

        Return sb.ToString

    End Function

    ''' <summary>
    ''' サムネイル情報 格納クラスログ出力
    ''' </summary>
    ''' <param name="paramData">サムネイル情報</param>
    ''' <returns>加工した文字列</returns>
    ''' <remarks></remarks>
    Private Function GetReturnThumbnailInfo(
        ByVal paramData As ThumbnailInfoList
    ) As String
        Dim sb As New StringBuilder
        With sb
            .Append("Return ThumbnailInfoList =")
            .Append("ThumbnailInfo")
            .Append(" Count:")
            '全データ
            If paramData Is Nothing Then
                .Append("0")
            ElseIf paramData.ThumbnailInfo Is Nothing Then
                .Append("0")
            Else
                .Append(paramData.ThumbnailInfo.Count)
            End If
        End With

        Return sb.ToString

    End Function

#End Region

End Class

