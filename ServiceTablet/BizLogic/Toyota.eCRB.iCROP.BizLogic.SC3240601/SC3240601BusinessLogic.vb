'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3240601BusinessLogic.vb
'─────────────────────────────────────
'機能： WarningMileage BusinessLogic
'補足： 
'作成： 2014/06/24 TMEJ 陳 IT9678_タブレット版SMB（テレマ走行距離機能開発）
'更新： 
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SMB.Telema.DataAccess
Imports Toyota.eCRB.SMB.Telema.DataAccess.SC3240601DataSet
Imports Toyota.eCRB.SMB.Telema.DataAccess.SC3240601DataSetTableAdapters
Imports System.Reflection
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess.ServiceCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess

Public Class SC3240601BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "メイン処理"

    ''' <summary>
    ''' 走行履歴一覧取得
    ''' </summary>
    ''' <param name="inVclId">車両ID</param>
    ''' <param name="inVin">VIN</param>
    ''' <param name="inOwnersId">オーナーズID</param>
    ''' <param name="inOccurdate">発生日時</param>
    ''' <param name="inStartIndex">検索開始Index</param>
    ''' <param name="inEndIndex">検索終了Index</param>
    ''' <param name="inTelemaDisplayCount">GBOOK表示件数</param>
    ''' <returns>走行履歴一覧DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetMileageList(ByVal inVclId As Decimal, _
                                   ByVal inVin As String, _
                                   ByVal inOwnersId As String, _
                                   ByVal inOccurdate As Date, _
                                   ByVal inStartIndex As Long, _
                                   ByVal inEndIndex As Long, _
                                   ByVal inTelemaDisplayCount As Long) As SC3240601DataSet.SC3240601TelemaInfoDataTable
        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START inVclId:{2};inVin:{3};inOwnersId:{4};inOccurdate:{5};inStartIndex:{6};inEndIndex:{7};inTelemaDispCount:{8};" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , inVclId _
                    , inVin _
                    , inOwnersId _
                    , inOccurdate _
                    , inStartIndex _
                    , inEndIndex _
                    , inTelemaDisplayCount))

        Dim dt As SC3240601DataSet.SC3240601TelemaInfoDataTable


        Using da As New SC3240601DataTableAdapter

            '走行履歴一覧取得
            dt = da.GetMileageList(inVclId, _
                                   inVin, _
                                   inOwnersId, _
                                   inOccurdate, _
                                   inStartIndex, _
                                   inEndIndex, _
                                   inTelemaDisplayCount)

        End Using

        If IsNothing(dt) Then
            'Nothingを取得する場合、エラー発生としてNothing返却

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} Return Nothing END" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

            Return Nothing

        End If

        If 0 = dt.Count Then
            '走行履歴一覧が取得できなかった場合

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} MileageList is not found. END" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

            '結果返却
            Return dt

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        '結果返却
        Return dt

    End Function

    ''' <summary>
    ''' 走行距離履歴一覧Warning情報を取得
    ''' </summary>
    ''' <param name="inOwnerId">オーナーズID</param>
    ''' <param name="inVin">VIN</param>
    ''' <param name="inOccurdate">発生日時</param>
    ''' <param name="inReceiveSeq">受信連番</param>
    ''' <returns>走行履歴一覧DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetMileageWarningList(ByVal inOwnerId As String, _
                                            ByVal inVin As String, _
                                            ByVal inOccurdate As Date, _
                                            ByVal inReceiveSeq As Long) As SC3240601DataSet.SC3240601TelemaInfoDataTable
        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START inOwnerId:{2};inVin:{3};inOccurdate:{4};inReceiveSeq:{5};" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , inOwnerID _
                    , inVin _
                    , inOccurdate _
                    , inReceiveSeq))

        Dim dt As SC3240601DataSet.SC3240601TelemaInfoDataTable

        Using da As New SC3240601DataTableAdapter

            '走行履歴一覧Warning情報を取得
            dt = da.GetMileageWarningList(inOwnerID, inVin, inOccurdate, inReceiveSeq)

        End Using

        If IsNothing(dt) Then
            'Nothingを取得する場合、エラー発生としてNothing返却

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} Return Nothing END" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

            Return Nothing
        End If

        If 0 = dt.Count Then
            '走行距離履歴一覧Warning情報が取得できなかった場合

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} WarningInfo is not found. END" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

            '結果返却
            Return dt

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        '結果返却
        Return dt

    End Function

    ''' <summary>
    ''' 走行履歴一覧件数取得
    ''' </summary>
    ''' <param name="inVclId">車両ID</param>
    ''' <param name="inVin">VIN</param>
    ''' <param name="inOwnersId">オーナーズID</param>
    ''' <param name="inOccurdate">発生日時</param>
    ''' <param name="inTelemaDisplayCount">GBOOK表示件数</param>
    ''' <returns>走行履歴一覧件数；-1の場合取得エラー発生</returns>
    ''' <remarks></remarks>
    Public Function GetMileageListCount(ByVal inVclId As Decimal, _
                                       ByVal inVin As String, _
                                       ByVal inOwnersId As String, _
                                       ByVal inOccurdate As Date, _
                                       ByVal inTelemaDisplayCount As Long) As Long
        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START inVclId:{2};inVin:{3};inOwnersId:{4};inOccurdate:{5};inTelemaDisplayCount:{6};" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , inVclId _
                    , inVin _
                    , inOwnersId _
                    , inOccurdate _
                    , inTelemaDisplayCount))

        Dim dt As SC3240601DataSet.SC3240601TelemaInfoDataTable


        Using da As New SC3240601DataTableAdapter

            '走行履歴一覧件数取得
            dt = da.GetMileageListCount(inVclId, _
                                        inVin, _
                                        inOwnersId, _
                                        inOccurdate, _
                                        inTelemaDisplayCount)


        End Using

        If IsNothing(dt) Then
            'Nothingを取得する場合、エラー発生として-1返却

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} Return -1 END" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

            Return -1

        End If

        If 0 = dt.Count Then
            '走行履歴一覧件数0の場合、0を返却

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} GetMileageListCount = 0. END" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

            Return 0

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        '結果返却
        Return dt.Rows.Count

    End Function

    ''' <summary>
    ''' グラフ情報を取得
    ''' </summary>
    ''' <param name="inVclId">車両ID</param>
    ''' <param name="inVin">VIN</param>
    ''' <param name="inOwnersId">オーナーズID</param>
    ''' <param name="inOccurdate">発生日時</param>
    ''' <param name="inStartDate">検索開始日時</param>
    ''' <param name="inEndDate">検索終了日時</param>
    ''' <returns>走行履歴一覧DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetMileageGraph(ByVal inVclId As Decimal, _
                                    ByVal inVin As String, _
                                    ByVal inOwnersId As String, _
                                    ByVal inOccurdate As Date, _
                                    ByVal inStartDate As Date, _
                                    ByVal inEndDate As Date) As SC3240601DataSet.SC3240601TelemaInfoDataTable
        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START inVclId:{2};inVin:{3};inOwnersId:{4};inOccurdate:{5};inStartDate:{6};inEndDate:{7};" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , inVclID _
                    , inVin _
                    , inOwnersId _
                    , inOccurdate _
                    , inStartDate _
                    , inEndDate))

        Dim dt As SC3240601DataSet.SC3240601TelemaInfoDataTable


        Using da As New SC3240601DataTableAdapter

            'Graph情報取得
            dt = da.GetMileageGraph(inVclID, inVin, inOwnersId, inOccurdate, inStartDate, inEndDate)

        End Using

        If IsNothing(dt) Then
            'Nothingを取得する場合、エラー発生としてNothing返却

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} Return Nothing END" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

            Return Nothing
        End If

        If 0 = dt.Count Then
            'グラフ情報が取得できなかった場合

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} MileageGraph is not found. END" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

            '結果返却
            Return dt

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        '結果返却
        Return dt

    End Function

    ''' <summary>
    ''' 所有者情報取得
    ''' </summary>
    ''' <param name="inDlrCD">販売店コード</param>
    ''' <param name="inVclId">車両ID</param>
    ''' <returns>所有者情報Row</returns>
    ''' <remarks></remarks>
    Public Function GetOwnerInfo(ByVal inDlrCD As String, _
                                 ByVal inVclId As Decimal) As SC3240601DataSet.SC3240601OwnerInfoRow
        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START inDlrCD:{2};inVclId:{3};" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , inDlrCD _
                    , inVclID))

        Dim dr As SC3240601DataSet.SC3240601OwnerInfoRow

        Using da As New SC3240601DataTableAdapter

            '所有者情報取得
            dr = da.GetOwnerInfo(inDlrCD, inVclID)

        End Using

        If IsNothing(dr) Then
            'Nothingを取得する場合、エラー発生としてNothing返却

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} OwnerInfo not found.(Return Nothing) END" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

            Return Nothing

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        '結果返却
        Return dr

    End Function

    ''' <summary>
    ''' 店舗名を取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <returns>店舗名称</returns>
    ''' <remarks></remarks>
    Public Function GetBranchName(ByVal inDealerCode As String, _
                                  ByVal inBranchCode As String) As String
        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START inDealerCode:{2};inBranchCode:{3};" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , inDealerCode _
                    , inBranchCode))

        Dim ret As String = String.Empty

        Using da As New SC3240601DataTableAdapter

            '販売店名を取得
            ret = da.GetBranchName(inDealerCode, inBranchCode)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        '結果返却
        Return ret

    End Function

    ''' <summary>
    ''' オーナーズID取得
    ''' </summary>
    ''' <param name="inVin">VIN</param>
    ''' <param name="inVclId">車両ID</param>
    ''' <returns>オーナーズID</returns>
    ''' <remarks></remarks>
    Public Function GetOwnerId(ByVal inVin As String, _
                                 ByVal inVclId As Decimal) As String
        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START inVin:{2};inVclId:{3};" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , inVin _
                    , inVclId))

        Dim ret As String = String.Empty

        Using da As New SC3240601DataTableAdapter

            'オーナーズID取得
            ret = da.GetOwnerId(inVin, inVclId)

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        '結果返却
        Return ret

    End Function

    ''' <summary>
    ''' Warning詳細を取得
    ''' </summary>
    ''' <param name="inCntCD">国番号</param>
    ''' <param name="inOwnersId">オーナーズID</param>
    ''' <param name="inVin">VIN</param>
    ''' <param name="inReceiveSeq">受信連番</param>
    ''' <param name="inSeqNo">連番</param>
    ''' <returns>Warning詳細DataTable</returns>
    ''' <remarks></remarks>
    Public Function GetWarningDetail(ByVal inCntCD As String, _
                                     ByVal inOwnersId As String, _
                                     ByVal inVin As String, _
                                     ByVal inReceiveSeq As Long, _
                                     ByVal inSeqNo As Long) As SC3240601WarningDetailDataTable
        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START inCntCD:{2};inOwnersId:{3};inVin:{4};inReceiveSeq:{5};inSeqNo:{6};" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , inCntCD _
                    , inOwnersId _
                    , inVin _
                    , inReceiveSeq _
                    , inSeqNo))

        Dim dt As SC3240601DataSet.SC3240601WarningDetailDataTable

        Using da As New SC3240601DataTableAdapter

            'Warning詳細を取得
            dt = da.GetWarningDetail(inCntCD, inOwnersId, inVin, inReceiveSeq, inSeqNo)

        End Using

        If IsNothing(dt) Then
            'Nothingを取得する場合、エラー発生としてNothing返却

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} Return Nothing END" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

            Return Nothing

        End If

        If 0 = dt.Count Then
            'Warning詳細情報が取得できなかった場合

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} WarningDetail is not found. END" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

            '結果返却
            Return dt

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END" _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        '結果返却
        Return dt

    End Function

    ''' <summary>
    ''' 基幹コードへ変換処理
    ''' </summary>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <returns>基幹コードMapRow</returns>
    ''' <remarks></remarks>
    Public Function ChangeDmsCode(ByVal inStaffInfo As StaffContext) _
                                  As ServiceCommonClassDataSet.DmsCodeMapRow
        '開始ログの出力
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START inDlrCD:{2}; inBrnCD:{3}; inAccount:{4};" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , inStaffInfo.DlrCD _
                    , inStaffInfo.BrnCD _
                    , inStaffInfo.Account))

        'ServiceCommonClassBusinessLogicのインスタンス
        Using smbCommon As New ServiceCommonClassBusinessLogic


            '基幹コードへ変換処理
            Dim dtDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapDataTable = _
                smbCommon.GetIcropToDmsCode(inStaffInfo.DlrCD, _
                                            ServiceCommonClassBusinessLogic.DmsCodeType.BranchCode, _
                                            inStaffInfo.DlrCD, _
                                            inStaffInfo.BrnCD, _
                                            String.Empty, _
                                            inStaffInfo.Account)

            '基幹コード情報Row
            Dim rowDmsCodeMap As ServiceCommonClassDataSet.DmsCodeMapRow

            '基幹コードへ変換処理結果チェック
            If Not IsNothing(dtDmsCodeMap) AndAlso 0 < dtDmsCodeMap.Rows.Count Then
                '基幹コードへ変換処理成功

                'Rowに変換
                rowDmsCodeMap = CType(dtDmsCodeMap.Rows(0), ServiceCommonClassDataSet.DmsCodeMapRow)

                '基幹アカウントチェック
                If rowDmsCodeMap.IsACCOUNTNull Then
                    '値無し

                    '空文字を設定する
                    '基幹アカウント
                    rowDmsCodeMap.ACCOUNT = String.Empty

                End If

                '基幹販売店コードチェック
                If rowDmsCodeMap.IsCODE1Null Then
                    '値無し

                    '空文字を設定する
                    '基幹販売店コード
                    rowDmsCodeMap.CODE1 = String.Empty

                End If

                '基幹店舗コードチェック
                If rowDmsCodeMap.IsCODE2Null Then
                    '値無し

                    '空文字を設定する
                    '基幹店舗コード
                    rowDmsCodeMap.CODE2 = String.Empty

                End If

            Else
                '基幹コードへ変換処理失敗

                '新しいRowを作成
                rowDmsCodeMap = dtDmsCodeMap.NewDmsCodeMapRow

                '空文字を設定する
                '基幹アカウント
                rowDmsCodeMap.ACCOUNT = String.Empty
                '基幹販売店コード
                rowDmsCodeMap.CODE1 = String.Empty
                '基幹店舗コード
                rowDmsCodeMap.CODE2 = String.Empty

            End If

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} dtDmsCodeMap:COUNT = {2} END" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , dtDmsCodeMap.Count))

            '結果返却
            Return rowDmsCodeMap

        End Using

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

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

#End Region

End Class

