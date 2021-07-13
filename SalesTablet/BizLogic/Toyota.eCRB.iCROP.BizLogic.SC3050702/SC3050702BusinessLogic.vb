Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Json
Imports System.Text
Imports System.IO
Imports Toyota.eCRB.TCV.TCVSetting.BizLogic.TCVSettingUtility
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Reflection.MethodBase
Imports System.Globalization

''' <summary>
''' セールスポイント設定画面のビジネスロジック層
''' </summary>
''' <remarks></remarks>
Public Class SC3050702BusinessLogic

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
#End Region

#Region " メソッド "
    ''' <summary>
    ''' JSONファイルを読み込み、クラスオブジェクトに変換します。
    ''' </summary>
    ''' <param name="tcvPath">TCV物理パス</param>
    ''' <param name="carID">車種ID</param>
    ''' <param name="type">エクステリア/インテリア</param>
    ''' <returns>クラスオブジェクト</returns>    
    ''' <remarks></remarks>
    Public Function GetSalesPointInfo(
        ByVal tcvPath As String,
        ByVal carId As String,
        ByVal type As String
    ) As SalesPointListJson

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("type", type, True))

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
            For Each salesPointData As SalesPointJson In salesPointList.sales_point

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

        Finally
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
    ''' <param name="salesPointList">セールスポイント情報データセット</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Public Function UpdateSalesPointInfoSend(
        ByVal tcvPath As String,
        ByVal carId As String,
        ByVal type As String,
        ByVal salesPointList As SalesPointListJson
    ) As String

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("tcvPath", tcvPath, False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("carId", carId, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("type", type, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam(
                    "salesPointList",
                    TcvSettingUtilityBusinessLogic.GetCountLog("sales_point", salesPointList.sales_point.ToArray),
                    True))

        '戻り値
        Dim msgID As String = ""

        Try
            If salesPointList.sales_point.Count > 0 Then
                '受け取ったデータに１件以上のデータがあればソート処理
                salesPointList = salesPointSort(
                                    salesPointList,
                                    0,
                                    salesPointList.sales_point.Count - 1)
            End If


            '受け取ったデータに含まれていないデータの取得
            If TypeExterior.Equals(type) Then
                '受け取ったデータが外装なら、JSONファイルから内装を取得してくる
                Dim salesPointInterior As SalesPointListJson =
                    GetSalesPointInfo(
                        tcvPath,
                        carId,
                        TypeInterior)

                '外装情報 → 内装情報の順で格納する
                salesPointList.sales_point.AddRange(salesPointInterior.sales_point)

            Else
                '受け取ったデータが内装なら、JSONファイルから外装を取得してくる
                Dim salesPointExterior As SalesPointListJson =
                    GetSalesPointInfo(
                        tcvPath,
                        carId,
                        TypeExterior)

                '外装情報 → 内装情報の順で格納する
                salesPointList.sales_point.InsertRange(0, salesPointExterior.sales_point)

            End If

            If msgID.Trim.Length > 0 Then
                'メッセージIDが空文字でない場合は処理終了
                Exit Try
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

        Finally
            Logger.Info(TcvSettingUtilityBusinessLogic.GetReturnParam(msgID))
            Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        End Try

        Return msgID

    End Function

    ''' <summary>
    ''' セールスポイント情報のソート処理
    ''' </summary>
    ''' <param name="salesPointList">セールスポイント情報データセット</param>
    ''' <returns>ソートの完了したセールスポイント情報データセット</returns>
    ''' <remarks></remarks>
    Private Function salesPointSort(
        ByVal salesPointList As SalesPointListJson,
        ByVal indexMin As Integer,
        ByVal indexMax As Integer
    ) As SalesPointListJson

        '開始ログ出力
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam(
                    "salesPointList",
                    TcvSettingUtilityBusinessLogic.GetCountLog("sales_point", salesPointList),
                    False))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("indexMin", CType(indexMin, String), True))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogParam("indexMax", CType(indexMax, String), True))


        '対象データの中央付近の値を基準値に設定
        Dim baseVaule As Integer =
            salesPointList.sales_point((indexMin + indexMax) \ 2).SortNo

        '左辺(基準値のインデックスを境に左側)インデックスに最小インデックスをセット
        Dim indexL As Integer = indexMin

        '右辺(基準値のインデックスを境に右側)インデックスに最大インデックスをセット
        Dim indexR As Integer = indexMax

        'ソート処理
        Do
            'インデックスの小さい側から基準値に向けて比較処理
            For indexL = indexL To indexMax
                If salesPointList.sales_point(indexL).SortNo >= baseVaule Then
                    '基準値以上の値が見つかればループ終了(左辺入替対象要素)
                    Exit For
                End If
            Next

            'インデックスの大きい側から基準値に向けて比較処理
            For indexR = indexR To indexMin Step -1
                If salesPointList.sales_point(indexR).SortNo <= baseVaule Then
                    '基準値以下の値が見つかればループ終了(右辺入替対象要素)
                    Exit For
                End If
            Next

            '左辺インデックスと右辺インデックスが同じか大小逆転したらソート処理終了
            If indexL >= indexR Then
                Exit Do
            End If

            '左辺・右辺の入替対象の要素を交換
            Dim salesPointData As SalesPointJson = salesPointList.sales_point(indexL)
            salesPointList.sales_point(indexL) = salesPointList.sales_point(indexR)
            salesPointList.sales_point(indexR) = salesPointData

            '左辺と右辺のインデックスをインクリメント
            indexL += 1
            indexR -= 1

        Loop

        If indexMin < (indexL - 1) Then
            '左辺に対して同じソートを行う
            salesPointSort(salesPointList, indexMin, indexL - 1)
        End If

        If indexMax > (indexR + 1) Then
            '右辺に対して同じソートを行う
            salesPointSort(salesPointList, indexR + 1, indexMax)
        End If


        '終了ログ出力
        Logger.Info(GetReturnSalesPointJson(salesPointList))
        Logger.Info(TcvSettingUtilityBusinessLogic.GetLogMethod(GetCurrentMethod.Name, False))

        Return salesPointList

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

#End Region

End Class
