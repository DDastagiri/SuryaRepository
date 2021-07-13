'-------------------------------------------------------------------------
'Actions.vb
'-------------------------------------------------------------------------
'機能：タブレットSMB共通関数
'補足：各アクション操作
'作成：2013/08/14 TMEJ 張 タブレット版SMB機能開発(工程管理)
'更新：2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
'更新：2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
'更新：2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発
'更新：2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発
'更新：2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
'更新：2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発
'更新：2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応
'更新：2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応
'更新：2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応
'更新：2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更）
'更新：2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
'更新：2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化)
'更新：2015/04/07 TMEJ 小澤 BTS-XXX JOB_IDのシーケンス設定を修正
'更新：2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
'更新：2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
'更新：2015/06/15 TMEJ 小澤 タブレットSMB部品ステータスIFのログ出力処理
'更新：2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理
'更新：2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力
'更新：2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応
'更新：2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化
'更新：2015/09/08 TMEJ 皆川 タブレットSMB ストールグループ表示対応
'更新：2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化
'更新：2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応
'更新：2016/01/12 NSK 皆川 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応
'更新：2016/04/20 NSK 小牟禮 工程管理の初期表示処理性能改善対応
'更新：2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない
'更新：2016/06/29 NSK 皆川 TR-SVT-TMT-20160512-001 SA1はチップを作成していないのに、通知を受け取った
'更新：2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 
'更新：2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする
'更新：2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない
'更新：2018/12/21 NSK 坂本 TR-SVT-TMT-20180421-001 サービスタブレットのレスポンスが全画面で遅延している_工程管理でPS01を呼び出さないようにしたい
'更新：2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証
'更新：2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新：2019/07/19 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証
'更新：2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証
'更新：
'─────────────────────────────────────

Imports System.Xml
Imports System.Net
Imports System.Web
Imports System.IO
Imports System.Globalization
Imports System.Reflection
Imports System.Xml.Serialization
Imports System.Text.RegularExpressions
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSet
Imports Toyota.eCRB.CommonUtility.TabletSMBCommonClass.Api.DataAccess.TabletSMBCommonClassDataSetTableAdapters
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.SMBCommonClass.Api.DataAccess
Imports System.Text
'2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
'Imports Toyota.eCRB.DMSLinkage.AddRepair.BizLogic.IC3800805
'2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports System.Collections.Generic
'2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
'Imports Toyota.eCRB.DMSLinkage.AddRepair.BizLogic.IC3800809
'Imports Toyota.eCRB.DMSLinkage.AddRepair.DataAccess.IC3800809
'Imports Toyota.eCRB.DMSLinkage.Reserve.BizLogic.IC3800902
'Imports Toyota.eCRB.DMSLinkage.Reserve.DataAccess.IC3800902
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.BizLogic.IC3801015
'Imports Toyota.eCRB.DMSLinkage.RepairOrderSearch.DataAccess.IC3801015
'2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

'2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
Imports Toyota.eCRB.DMSLinkage.Reserve.Api.BizLogic
'2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

'2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.BizLogic
Imports Toyota.eCRB.CommonUtility.ServiceCommonClass.Api.DataAccess
Imports Toyota.eCRB.DMSLinkage.StatusInfo.Api.BizLogic
'2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

'2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.PartsInfo.Api.DataAccess.IC3802503DataSet
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.BizLogic
Imports Toyota.eCRB.DMSLinkage.JobDispatchResult.Api.DataAccess.IC3802701DataSet

'2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
''' <summary>
''' タブレットSMB共通関数群の部分クラス
''' </summary>
''' <remarks>
''' 各アクション
''' </remarks>
Partial Class TabletSMBCommonClassBusinessLogic

#Region "各取得処理"

#Region "チップエンティティを取得"
    ''' <summary>
    ''' チップエンティティを取得
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="nType">取得タイプ:1洗車時間、検査時間を含めて取得</param>
    ''' <returns>チップエンティティ</returns>
    ''' <remarks></remarks>
    Public Function GetChipEntity(ByVal stallUseId As Decimal, Optional ByVal nType As Short = 0) As TabletSmbCommonClassChipEntityDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, nType={2}" _
            , MethodBase.GetCurrentMethod.Name, stallUseId, nType))
        Dim dtResult As TabletSmbCommonClassChipEntityDataTable
        Using ta As New TabletSMBCommonClassDataAdapter
            dtResult = ta.GetChipEntity(stallUseId, nType)
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return dtResult
    End Function


#End Region

#Region "ストールチップ情報の取得"

    ''' <summary>
    ''' ストール上のチップ一覧の取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="stallStartTime">稼動開始日時</param>
    ''' <param name="stallEndTime">稼動終了日時</param>
    ''' <param name="stallIdList">ストールIDリスト</param>
    ''' <param name="theTime">この日時後変更があったチップを取得</param>
    ''' <returns>ストール上のチップ情報</returns>
    ''' <hitory>
    ''' 2015/06/15 TMEJ 小澤 タブレットSMB部品ステータスIFのログ出力処理
    ''' </hitory>
    Public Function GetAllStallChip(ByVal dlrCode As String, _
                                    ByVal brnCode As String, _
                                    ByVal stallStartTime As Date, _
                                    ByVal stallEndTime As Date, _
                                    Optional ByVal stallIdList As List(Of Decimal) = Nothing, _
                                    Optional ByVal theTime As Date = Nothing) As TabletSmbCommonClassStallChipInfoDataTable
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        'Public Function GetAllStallChip(ByVal dlrCode As String, _
        '                        ByVal brnCode As String, _
        '                        ByVal stallStartTime As Date, _
        '                        ByVal stallEndTime As Date, _
        '                        Optional ByVal stallIdList As List(Of Long) = Nothing) As TabletSmbCommonClassStallChipInfoDataTable
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dlrCode={1}, brnCode={2}, stallStartTime={3}, stallEndTime={4}" _
                                , MethodBase.GetCurrentMethod.Name, dlrCode, brnCode, stallStartTime, stallEndTime))

        Dim chipList As TabletSmbCommonClassStallChipInfoDataTable

        '■■■■■ストール上のチップ一覧の取得 2-20 4-0-0-0-0-0 START■■■■■
        LogServiceCommonBiz.OutputLog(54, "●■● 2.4 ストール上のチップ一覧の取得 START")

        'ストール上のチップ一覧の取得
        Using ta As New TabletSMBCommonClassDataAdapter
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            '差分リフレッシュの場合、RO情報テーブルの前回リフレッシュ時間から変わったサービス入庫IDを取得する
            Dim svcinIdListWithRoInfo As String = ""
            If theTime <> CDate(Nothing) Then

                '■■■■■SQL_TABLETSMBCOMMONCLASS_063 差分リフレッシュの間に変更があるRO情報のサービス入庫IDを取得 2-21 4-1-0-0-0-0 START■■■■■
                LogServiceCommonBiz.OutputLog(55, "●■● 2.4.1 TABLETSMBCOMMONCLASS_063 START")

                Dim svcinIdTbl As TabletSmbCommonClassNumberValueDataTable = _
                    ta.GetSvcinIdByDiffRefresh(dlrCode, brnCode, theTime)

                LogServiceCommonBiz.OutputLog(55, "●■● 2.4.1 TABLETSMBCOMMONCLASS_063[取得件数:" & svcinIdTbl.Count & "] END")
                '■■■■■SQL_TABLETSMBCOMMONCLASS_063 差分リフレッシュの間に変更があるRO情報のサービス入庫IDを取得(★件数表示) 2-21 4-1-0-0-0-0  END■■■■■

                svcinIdListWithRoInfo = ConvertNumberTableToString(svcinIdTbl)
            End If


            'chipList = ta.GetAllStallChip(dlrCode, brnCode, stallStartTime, stallEndTime, stallIdList)
            chipList = ta.GetAllStallChip(dlrCode, brnCode, stallStartTime, stallEndTime, stallIdList, theTime, svcinIdListWithRoInfo)
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        End Using

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''追加作業ステータスを取得
        'Dim biz As New IC3800809BusinessLogic
        'Dim addWorkStatuslist() As String = {AddWorkAddingWork, AddWorkConfirmWait, AddWorkConfirmWait2, AddWorkConfirmWait3}
        ''追加作業承認待ち、追加作業起票中のレコードを取得する
        ''追加作業情報取得apiを呼ぶ
        'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. PARAM:dlrCode={1},addWorkStatuslist={2}", _
        '                                     MethodBase.GetCurrentMethod.Name, dlrCode, Me.ConvertStringArrayToString(addWorkStatuslist)))
        'Dim dtAddInfo As IC3800809DataSet.IC3800809AddRepairInfoDataTable = biz.GetAddRepairInfoList(dlrCode, addWorkStatuslist)
        'OutPutIFLog(dtAddInfo, "IC3800809BusinessLogic.GetAddRepairInfoList")

        ''チップ情報テーブルに追加する
        'For Each drChipInfo As TabletSmbCommonClassStallChipInfoRow In chipList
        '    Dim roNo As String = drChipInfo.RO_NUM
        '    If String.IsNullOrWhiteSpace(roNo) Then
        '        Continue For
        '    End If

        '    '追加作業ステータステーブルにRONUMで探す
        '    For Each drAddinfo As IC3800809DataSet.IC3800809AddRepairInfoRow In dtAddInfo
        '        If drAddinfo.orderNO.Equals(roNo) Then
        '            '2～4が全部追加作業承認待ち、それで、3、4の場合、2に設定する
        '            If drAddinfo.ADDStatus.Equals(AddWorkConfirmWait2) Or drAddinfo.ADDStatus.Equals(AddWorkConfirmWait3) Then
        '                drChipInfo.ADDWORK_STATUS = AddWorkConfirmWait
        '            Else
        '                drChipInfo.ADDWORK_STATUS = drAddinfo.ADDStatus
        '            End If
        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0} Set chip addworkstatus: STALLUSEID={1},ADDWORK_STATUS={2}", _
        '                                      MethodBase.GetCurrentMethod.Name, drChipInfo.STALL_USE_ID, drChipInfo.ADDWORK_STATUS))
        '            Exit For
        '        End If
        '    Next
        'Next

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ''ROステータス取得
        'chipList = Me.GetRoStatus(chipList)
        ''部品出庫フラグを登録する
        'chipList = Me.GetPartFlgs(chipList, dlrCode, brnCode)

        'RO番号のリスト(GetRoStatus引数)
        Dim roNumList As New List(Of String)

        '部品のステータス情報テーブル
        Dim partsStatusTable As IC3802503PartsStatusDataTable = Nothing

        '部品ステータス取得するため、渡す用引数(RO番号とRO枝番で構成されたテーブル)
        Using roNumTable As New IC3802503RONumInfoDataTable

            '全てチップをループする
            For Each chipRow As TabletSmbCommonClassStallChipInfoRow In chipList

                '全てチップのRO番号リストを作成
                'RO番号に値があれば
                If Not chipRow.IsRO_NUMNull AndAlso Not String.IsNullOrWhiteSpace(chipRow.RO_NUM) Then

                    '重複のRO番号を絞り込む
                    If Not roNumList.Contains(chipRow.RO_NUM) Then

                        'ROリストに既に存在してない場合、追加する
                        roNumList.Add(chipRow.RO_NUM)

                        'テーブルにRO番号を設定する
                        Dim roNumRow As IC3802503RONumInfoRow = roNumTable.NewIC3802503RONumInfoRow
                        roNumRow.R_O = chipRow.RO_NUM
                        'テーブルに追加する
                        roNumTable.AddIC3802503RONumInfoRow(roNumRow)

                    End If

                End If

            Next

            '2015/06/15 TMEJ 小澤 タブレットSMB部品ステータスIFのログ出力処理 START

            ''部品のステータス情報テーブルを取得する
            'partsStatusTable = Me.GetPartFlgs(dlrCode, _
            '                                  brnCode, _
            '                                  roNumTable)

            '2018/12/21 NSK 坂本 TR-SVT-TMT-20180421-001 サービスタブレットのレスポンスが全画面で遅延している_工程管理でPS01を呼び出さないようにしたい START
            ''初期表示か差分リフレッシュ化のチェック
            'If IsNothing(theTime) OrElse theTime = Date.MinValue Then
            '    '初期表示の場合

            '    '部品のステータス情報テーブルを取得する
            '    partsStatusTable = Me.GetPartFlgs(dlrCode, _
            '                                      brnCode, _
            '                                      roNumTable)

            'Else
            '    '上記以外の場合
            '    'ストップウォッチで部品ステータス情報取得の計測をしてログ出力する
            '    Dim sw As New Stopwatch

            '    '測定開始
            '    sw.Start()

            '    '部品のステータス情報テーブルを取得する
            '    partsStatusTable = Me.GetPartFlgs(dlrCode, _
            '                                      brnCode, _
            '                                      roNumTable)

            '    '測定終了
            '    sw.Stop()

            '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
            '        , "{0}.{1} ①SC3240101_初期表示 IC3802503BusinessLogic.GetPartsStatusList Processing Time is [{2}]" _
            '        , Me.GetType.ToString _
            '        , System.Reflection.MethodBase.GetCurrentMethod.Name _
            '        , sw.Elapsed.ToString()))

            'End If
            '2018/12/21 NSK 坂本 TR-SVT-TMT-20180421-001 サービスタブレットのレスポンスが全画面で遅延している_工程管理でPS01を呼び出さないようにしたい END

            '2015/06/15 TMEJ 小澤 タブレットSMB部品ステータスIFのログ出力処理 START

        End Using

        '■■■■■ROステータスにより、追加作業マークを設定 2-22 4-2-0-0-0-0 START■■■■■
        LogServiceCommonBiz.OutputLog(56, "●■● 2.4.2 ROステータスにより、追加作業マークを設定 START")

        '追加作業マーク取得
        Dim roStatusTable As TabletSmbCommonClassROInfoDataTable = _
            Me.GetRoStatus(chipList, _
                           roNumList, _
                           dlrCode, _
                           brnCode)

        LogServiceCommonBiz.OutputLog(56, "●■● 2.4.2 ROステータスにより、追加作業マークを設定 END")
        '■■■■■ROステータスにより、追加作業マークを設定 2-22 4-2-0-0-0-0 END■■■■■

        LogServiceCommonBiz.OutputLog(54, "●■● 2.4 ストール上のチップ一覧の取得 END")
        '■■■■■ストール上のチップ一覧の取得 2-20 4-0-0-0-0-0 END■■■■■


        '■■■■■⑫～④の間の詳細ログEND■■■■■

        'チップ情報テーブルに取得された部品のステータス、追加作業マークを登録する
        Me.SetDataToChipTable(chipList, _
                              partsStatusTable, _
                              roStatusTable)

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        Return chipList
    End Function

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' チップ情報テーブルに「部品のステータス」、「追加作業マーク」、「中断Job含むフラグ」を登録する
    ''' </summary>
    ''' <param name="inChipInfoTable">チップ情報テーブルテーブル</param>
    ''' <param name="inPartsStatusTable">部品のステータステーブル</param>
    ''' <param name="inRoStatusTable">追加作業マークテーブル</param>
    ''' <returns>登録済のチップ情報テーブル</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理
    ''' </history>
    Private Function SetDataToChipTable(ByVal inChipInfoTable As TabletSmbCommonClassStallChipInfoDataTable, _
                                        ByVal inPartsStatusTable As IC3802503PartsStatusDataTable, _
                                        ByVal inRoStatusTable As TabletSmbCommonClassROInfoDataTable) As TabletSmbCommonClassStallChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start." _
                                  , MethodBase.GetCurrentMethod.Name))

        '2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理 START

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} ④SC3240101_チップ情報精査処理 START" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理 END

        'チップごとでループする
        For Each drChipInfo As TabletSmbCommonClassStallChipInfoRow In inChipInfoTable

            '各設定値の初期化
            '部品出庫アイコン全部非表示にする
            drChipInfo.PARTS_FLG = PartsFlgOff
            'プラスマークが表示されないのを初期化
            drChipInfo.ADDWORK_STATUS = AddWorkNoMark

            '部品出庫ステータスの設定
            'Apiで部品ステータス取得成功且つ部品ステータステーブルに値がある場合
            If ActionResult.Success = Me.IC3802503ResultValue _
                And Not IsNothing(inPartsStatusTable) Then

                'RO番号があれば
                If Not drChipInfo.IsRO_NUMNull AndAlso Not String.IsNullOrEmpty(drChipInfo.RO_NUM) Then

                    '該当チップの部品出庫フラグを設定する
                    drChipInfo.PARTS_FLG = Me.GetPartFlg(drChipInfo.RO_NUM, _
                                                         drChipInfo.JOB_DTL_ID, _
                                                         inPartsStatusTable)

                End If

            End If


            '追加作業マークの設定
            'RONUMがない場合、次へ
            'RO番号があれば
            If Not IsNothing(inRoStatusTable) _
                And Not drChipInfo.IsRO_NUMNull _
                AndAlso Not String.IsNullOrWhiteSpace(drChipInfo.RO_NUM) Then

                'RO番号がある行を洗い出す
                Dim roStatusRows As TabletSmbCommonClassROInfoRow() = _
                    CType(inRoStatusTable.Select(String.Format(CultureInfo.CurrentCulture, "RO_NUM = '{0}'", drChipInfo.RO_NUM)), TabletSmbCommonClassROInfoRow())

                '該当ROのデータをループする
                For Each roStatusRow As TabletSmbCommonClassROInfoRow In roStatusRows

                    If RoStatusTcIssuing.Equals(roStatusRow.RO_STATUS) Then
                        'TC 承認待ちの場合

                        '白いプラスマークを表示する
                        drChipInfo.ADDWORK_STATUS = AddWorkAddingWork
                        Exit For

                    ElseIf RoStatusWaitingForFmApproval.Equals(roStatusRow.RO_STATUS) _
                        Or RoStatusCreatingPartsRoughQuotation.Equals(roStatusRow.RO_STATUS) _
                        Or RoStatusCreatingPartsQuotation.Equals(roStatusRow.RO_STATUS) _
                        Or RoStatusCreatingWaitingForRoConfirmation.Equals(roStatusRow.RO_STATUS) _
                        Or RoStatusCreatingWaitingForCustomerApproval.Equals(roStatusRow.RO_STATUS) Then
                        'FM承認待ちなど場合

                        '黄色プラスマークを表示する
                        drChipInfo.ADDWORK_STATUS = AddWorkConfirmWait
                        Exit For

                    End If

                Next

            End If

        Next

        '2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理 START

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} ④SC3240101_チップ情報精査処理 END" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name))

        '2015/06/18 TMEJ 小澤 チップ情報取得のログ出力処理 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End", _
                                  MethodBase.GetCurrentMethod.Name))
        Return inChipInfoTable

    End Function

    ''' <summary>
    ''' ROステータスにより、追加作業マークを設定する
    ''' </summary>
    ''' <param name="chipList">ストールチップの情報テーブル</param>
    ''' <param name="roNumList">RO番号リスト</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetRoStatus(ByVal chipList As TabletSmbCommonClassStallChipInfoDataTable, _
                                 ByVal roNumList As List(Of String), _
                                 ByVal inDealerCode As String, _
                                 ByVal inBranchCode As String) As TabletSmbCommonClassROInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. chipList.Count={1}, roNumList.Count={2}, inDealerCode={3}, inBranchCode={4}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  chipList.Count, _
                                  roNumList.Count, _
                                  inDealerCode, _
                                  inBranchCode))
        ' ''' <summary>
        ' ''' ROステータスにより、追加作業マークを設定する
        ' ''' </summary>
        ' ''' <param name="chipList">ストールチップの情報テーブル</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Private Function GetRoStatus(ByVal chipList As TabletSmbCommonClassStallChipInfoDataTable) As TabletSmbCommonClassStallChipInfoDataTable

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                          "{0}.S.", _
        '                          MethodBase.GetCurrentMethod.Name))

        ''引数チェック
        'If chipList.Rows.Count = 0 Then
        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E chipList has no data.", MethodBase.GetCurrentMethod.Name))
        '    Return chipList
        'End If

        ''RO_NUMのリストを作成する
        'Dim roNumList As New List(Of String)
        'For Each chipRow As TabletSmbCommonClassStallChipInfoRow In chipList
        '    If Not chipRow.IsRO_NUMNull AndAlso Not String.IsNullOrWhiteSpace(chipRow.RO_NUM) Then
        '        If Not roNumList.Contains(chipRow.RO_NUM) Then
        '            roNumList.Add(chipRow.RO_NUM)
        '        End If
        '    End If
        'Next

        ''画面のチップに一つでもRO番号がない場合、
        'If roNumList.Count = 0 Then
        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E roNumList has no data.", MethodBase.GetCurrentMethod.Name))
        '    Return chipList
        'End If

        '返却用ROステータステーブル
        Dim roStatusTable As TabletSmbCommonClassROInfoDataTable = Nothing

        '画面のチップに一つでもRO番号がない場合、
        If roNumList.Count = 0 Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E roNumList has no data.", _
                                      MethodBase.GetCurrentMethod.Name))
            Return roStatusTable

        End If
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        'stringに変換する
        Dim roNums As String = Me.ConvertStringArrayToString(roNumList)

        'RONUMにより、ROステータスを取得する
        Using ta As New TabletSMBCommonClassDataAdapter

            '■■■■■SQL_TABLETSMBCOMMONCLASS_048 指定RONUMのROステータスを取得する 2-23 4-2-1-0-0-0 START■■■■■
            LogServiceCommonBiz.OutputLog(57, "●■● 2.4.2.1 TABLETSMBCOMMONCLASS_048 START")

            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
            'roStatusTable = ta.GetROStatusByRONum(roNums)
            roStatusTable = ta.GetROStatusByRONum(roNums, _
                                                  inDealerCode, _
                                                  inBranchCode)
            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

            LogServiceCommonBiz.OutputLog(57, "●■● 2.4.2.1 TABLETSMBCOMMONCLASS_048 END")
            '■■■■■SQL_TABLETSMBCOMMONCLASS_048 指定RONUMのROステータスを取得する 2-23 4-2-1-0-0-0 END■■■■■

        End Using

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E", _
                                  MethodBase.GetCurrentMethod.Name))

        Return roStatusTable

        'For Each drChipInfo As TabletSmbCommonClassStallChipInfoRow In chipList

        '    'プラスマークが表示されないのを初期化
        '    drChipInfo.ADDWORK_STATUS = AddWorkNoMark

        '    'RONUMがない場合、次へ
        '    Dim roNo As String = drChipInfo.RO_NUM
        '    If String.IsNullOrWhiteSpace(roNo) Then
        '        Continue For
        '    End If

        '    'RONUMがある行を洗い出す
        '    Dim roStatusRows As TabletSmbCommonClassROInfoRow() = _
        '        CType(roStatusTable.Select(String.Format(CultureInfo.CurrentCulture, "RO_NUM = '{0}'", roNo)), TabletSmbCommonClassROInfoRow())

        '    For Each roStatusRow As TabletSmbCommonClassROInfoRow In roStatusRows

        '        If RoStatusTcIssuing.Equals(roStatusRow.RO_STATUS) Then
        '            'TC 承認待ちの場合、白いプラスマークが表示される
        '            drChipInfo.ADDWORK_STATUS = AddWorkAddingWork
        '            Exit For
        '        ElseIf RoStatusWaitingForFmApproval.Equals(roStatusRow.RO_STATUS) _
        '            Or RoStatusCreatingPartsRoughQuotation.Equals(roStatusRow.RO_STATUS) _
        '            Or RoStatusCreatingPartsQuotation.Equals(roStatusRow.RO_STATUS) _
        '            Or RoStatusCreatingWaitingForRoConfirmation.Equals(roStatusRow.RO_STATUS) _
        '            Or RoStatusCreatingWaitingForCustomerApproval.Equals(roStatusRow.RO_STATUS) Then
        '            'FM承認待ちなど場合、黄色プラスマークが表示される
        '            drChipInfo.ADDWORK_STATUS = AddWorkConfirmWait
        '            Exit For
        '        End If

        '    Next

        'Next

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        'Return chipList
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    End Function

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' ストールチップの情報テーブルに部品出庫フラグを登録する
    ''' </summary>
    ''' <param name="dealCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <returns>IC3802503PartsStatusDataTable(部品のステータス情報テーブル)</returns>
    ''' <remarks></remarks>
    Private Function GetPartFlgs(ByVal dealCode As String, _
                                 ByVal branchCode As String, _
                                 ByVal roNumTable As IC3802503RONumInfoDataTable) As IC3802503PartsStatusDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. dealCode={1}, branchCode={2} ", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  dealCode, _
                                  branchCode))

        ' ''' <summary>
        ' ''' ストールチップの情報テーブルに部品出庫フラグを登録する
        ' ''' </summary>
        ' ''' <param name="chipInfoTable">ストールチップの情報テーブル</param>
        ' ''' <returns>部品出庫フラグ登録したストールチップの情報テーブル</returns>
        ' ''' <remarks></remarks>
        'Private Function GetPartFlgs(ByVal chipInfoTable As TabletSmbCommonClassStallChipInfoDataTable, _
        '                             ByVal dealCode As String, _
        '                             ByVal branchCode As String) As TabletSmbCommonClassStallChipInfoDataTable

        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
        '                              "{0}.S.", _
        '                              MethodBase.GetCurrentMethod.Name))

        ''引数チェック
        'If chipInfoTable.Rows.Count = 0 Then
        '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E chipInfoTable has no data.", MethodBase.GetCurrentMethod.Name))
        '    Me.IC3802503ResultValue = ActionResult.Success
        '    Return chipInfoTable
        'End If

        'Dim partsStatusTable As IC3802503PartsStatusDataTable = Nothing

        ''ストールのチップ情報に値があるRO番号のテーブル作成
        'Using roNumTable As New IC3802503RONumInfoDataTable
        '    Dim roNumList As New List(Of String)
        '    For Each drChipInfo As TabletSmbCommonClassStallChipInfoRow In chipInfoTable
        '        '部品出庫アイコン全部非表示にする
        '        drChipInfo.PARTS_FLG = PartsFlgOff

        '        'RO番号に値があるチップ
        '        If Not drChipInfo.IsRO_NUMNull AndAlso Not String.IsNullOrWhiteSpace(drChipInfo.RO_NUM) Then
        '            'RO番号に値の重複チェック
        '            If Not roNumList.Contains(drChipInfo.RO_NUM) Then
        '                roNumList.Add(drChipInfo.RO_NUM)
        '                Dim roNumRow As IC3802503RONumInfoRow = roNumTable.NewIC3802503RONumInfoRow
        '                roNumRow.R_O = drChipInfo.RO_NUM
        '                roNumTable.AddIC3802503RONumInfoRow(roNumRow)
        '            End If
        '        End If
        '    Next

        '    'RO番号により、部品ステータス情報取得
        '    If roNumTable.Rows.Count > 0 Then
        '        Using biz As New IC3802503BusinessLogic
        '            partsStatusTable = biz.GetPartsStatusList(dealCode, branchCode, roNumTable)
        '        End Using
        '    End If
        'End Using

        ''partsStatusTableに値があれば
        'If Not IsNothing(partsStatusTable) AndAlso partsStatusTable.Count > 0 Then

        '    'GetPartsStatusList操作がエラー発生したかチェック
        '    If partsStatusTable(0).ResultCode <> IC3802503BusinessLogic.Result.Success Then
        '        Select Case partsStatusTable(0).ResultCode
        '            Case IC3802503BusinessLogic.Result.TimeOutError
        '                Me.IC3802503ResultValue = ActionResult.IC3802503ResultTimeOutError
        '            Case IC3802503BusinessLogic.Result.DmsError
        '                Me.IC3802503ResultValue = ActionResult.IC3802503ResultDmsError
        '            Case IC3802503BusinessLogic.Result.OtherError
        '                Me.IC3802503ResultValue = ActionResult.IC3802503ResultOtherError
        '        End Select
        '    End If

        '    For Each drChipInfo As TabletSmbCommonClassStallChipInfoRow In chipInfoTable
        '        'RO番号に値があるチップの部品出庫フラグをリセットする
        '        If Not drChipInfo.IsRO_NUMNull AndAlso Not String.IsNullOrEmpty(drChipInfo.RO_NUM) Then
        '            drChipInfo.PARTS_FLG = Me.GetPartFlg(drChipInfo.RO_NUM, _
        '                                                 drChipInfo.JOB_DTL_ID, _
        '                                                 partsStatusTable)
        '        End If
        '    Next
        'Else
        '    Me.IC3802503ResultValue = ActionResult.Success
        'End If


        'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        'Return chipInfoTable

        '部品のステータス情報テーブル
        Dim partsStatusTable As IC3802503PartsStatusDataTable = Nothing

        'RO番号テーブルにデータがある場合
        If roNumTable.Rows.Count > 0 Then

            Using biz As New IC3802503BusinessLogic

                '部品のステータス情報テーブルを取得する
                partsStatusTable = biz.GetPartsStatusList(dealCode, branchCode, roNumTable)

            End Using

        End If

        '成功で初期化
        Me.IC3802503ResultValue = ActionResult.Success

        'partsStatusTableに値があれば
        If Not IsNothing(partsStatusTable) AndAlso partsStatusTable.Count > 0 Then

            'GetPartsStatusList操作がエラー発生したかチェック
            If IC3802503BusinessLogic.Result.Success <> partsStatusTable(0).ResultCode Then

                '結果コードにより、エラーを設定する
                Select Case partsStatusTable(0).ResultCode

                    Case IC3802503BusinessLogic.Result.TimeOutError
                        '部品ステータス情報取得タイムアウトエラーを設定する
                        Me.IC3802503ResultValue = ActionResult.IC3802503ResultTimeOutError

                    Case IC3802503BusinessLogic.Result.DmsError
                        '部品ステータス情報取得基幹側のエラーを設定する
                        Me.IC3802503ResultValue = ActionResult.IC3802503ResultDmsError

                    Case Else
                        '部品ステータス情報取得その他のエラーを設定する
                        Me.IC3802503ResultValue = ActionResult.IC3802503ResultOtherError

                End Select

            End If

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End Result={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  Me.IC3802503ResultValue))
        '取得した部品のステータス情報テーブルを返却する
        Return partsStatusTable

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    End Function

    ''' <summary>
    ''' 1つチップの部品出庫フラグを取得する
    ''' </summary>
    ''' <param name="roNum">RO番号</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="partsStatusTable">部品ステータステーブル</param>
    ''' <returns>部品出庫フラグ</returns>
    ''' <remarks></remarks>
    Private Function GetPartFlg(ByVal roNum As String, _
                                ByVal jobDtlId As Decimal, _
                                ByVal partsStatusTable As IC3802503PartsStatusDataTable) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. jobDtlId={1}, roNum={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  jobDtlId, _
                                  roNum))

        Dim jobInstructTable As TabletSmbCommonClassJobInstructDataTable = Nothing

        '該チップと紐付くRO枝番を取得する
        Using ta As New TabletSMBCommonClassDataAdapter
            jobInstructTable = ta.GetROJobSeqByJobDtlId(jobDtlId)
        End Using

        'データがない場合、出庫済みフラグを非表示する
        If jobInstructTable.Rows.Count = 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E retPartFlg={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      PartsFlgOff))

            Return PartsFlgOff
        End If

        '見つけフラグにFalseを初期化
        Dim roFindedFlg As Boolean = False

        For Each jobInstructRow As TabletSmbCommonClassJobInstructRow In jobInstructTable

            '部品ステータステーブルに指定RO番号、RO枝番のデータを取得する
            Dim partStatusRows As IC3802503PartsStatusRow() = _
                CType(partsStatusTable.Select(String.Format(CultureInfo.CurrentCulture, _
                                                           "R_O = '{0}' AND R_O_SEQNO = {1}", _
                                                           roNum, _
                                                           jobInstructRow.RO_JOB_SEQ)),  _
                                                   IC3802503PartsStatusRow())

            '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
            ''対応のデータが取得出来ない場合、出庫済みフラグを非表示する
            'If partStatusRows.Count = 0 Then
            '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
            '              "{0}.E retPartFlg={1}", _
            '              MethodBase.GetCurrentMethod.Name, _
            '              PartsFlgOff))

            '    Return PartsFlgOff
            'End If

            'RO連番、RO番号で戻りテーブルから見つける場合
            If partStatusRows.Count > 0 Then

                '見つけフラグにTrueを設定する
                roFindedFlg = True

            End If

            '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

            '任意部品出庫ステータスが出庫済みではない場合、出庫済みフラグを非表示する
            For Each partStatusRow As IC3802503PartsStatusRow In partStatusRows
                If Not PartsStatusFinish.Equals(partStatusRow.PARTS_ISSUE_STATUS) Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                              "{0}.E retPartFlg={1}", _
                                              MethodBase.GetCurrentMethod.Name, _
                                              PartsFlgOff))

                    Return PartsFlgOff
                End If
            Next
        Next

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        '該当チップに紐づくRO、RO連番が1つ部品データがない場合
        If Not roFindedFlg Then

            '部品アイコンを非表示にする
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E retPartFlg={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      PartsFlgOff))

            Return PartsFlgOff

        End If
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E retPartFlg={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  PartsFlgOn))

        Return PartsFlgOn

    End Function

    ' ''' <summary>
    ' ''' サービス入庫IDにより、チップの全情報を取得
    ' ''' </summary>
    ' ''' <param name="dlrCode">販売店コード</param>
    ' ''' <param name="brnCode">店舗コード</param>
    ' ''' <param name="dtNow">今の時間</param>
    ' ''' <param name="svcidList">サービス入庫IDリスト</param>
    ' ''' <returns>指定サービス入庫IDの関連チップ情報</returns>
    ' ''' <remarks></remarks>
    'Public Function GetStallChipBySvcinId(ByVal dlrCode As String, _
    '                                      ByVal brnCode As String, _
    '                                      ByVal dtNow As Date, _
    '                                      ByVal svcidList As List(Of Decimal)) As TabletSmbCommonClassStallChipInfoDataTable

    ''' <summary>
    ''' サービス入庫IDにより、チップの全情報を取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="dtNow">今の時間</param>
    ''' <param name="svcidList">サービス入庫IDリスト</param>
    ''' <param name="cancelChipFlg">キャンセルチップフラグ True:キャンセルしたチップがいる</param>
    ''' <returns>指定サービス入庫IDの関連チップ情報</returns>
    ''' <remarks></remarks>
    Public Function GetStallChipBySvcinId(ByVal dlrCode As String, _
                                          ByVal brnCode As String, _
                                          ByVal dtNow As Date, _
                                          ByVal svcidList As List(Of Decimal), _
                                          Optional ByVal cancelChipFlg As Boolean = True) As TabletSmbCommonClassStallChipInfoDataTable
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dlrCode={1}, brnCode={2}" _
                    , MethodBase.GetCurrentMethod.Name, dlrCode, brnCode))

        Dim chipList As TabletSmbCommonClassStallChipInfoDataTable

        '指定サービス入庫IDの関連チップ情報を取得する
        Using ta As New TabletSMBCommonClassDataAdapter
            chipList = ta.GetStallChipBySvcinId(dlrCode, brnCode, svcidList, cancelChipFlg)
        End Using

        'チップが一つでもない場合
        If chipList.Count = 0 Then
            Return chipList
        End If

        Dim svcinIdList As New List(Of Decimal)
        'サービス入庫IDのリストを作成
        For Each drChipInfo As TabletSmbCommonClassStallChipInfoRow In chipList
            '重複のサービス入庫IDをいれない
            If Not svcinIdList.Contains(drChipInfo.SVCIN_ID) Then
                svcinIdList.Add(drChipInfo.SVCIN_ID)
            End If
        Next

        '遅れ見込み列のデータを取得する
        Dim dtDelay As TabletSmbCommonClassDeliDelayDateDataTable = Me.GetDeliveryDelayDateList(svcinIdList, dlrCode, brnCode, dtNow)
        For Each drDelay As TabletSmbCommonClassDeliDelayDateRow In dtDelay
            Dim svcinId As Decimal = drDelay.SVCIN_ID

            Dim arrChipInfo = (From p In chipList Where p.SVCIN_ID = svcinId Select p).ToArray()
            For nLoop = 0 To arrChipInfo.Count - 1
                arrChipInfo(nLoop).PLAN_DELAYDATE = drDelay.DELI_DELAY_DATETIME
                '2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                '遅れ見込み情報から残完成検査区分を取得
                arrChipInfo(nLoop).REMAINING_INSPECTION_TYPE = drDelay.REMAINING_INSPECTION_TYPE
                '2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END
            Next
        Next


        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return chipList
    End Function

    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 START
    ''' <summary>
    ''' 各操作後、ストール上更新されたチップの情報を取得
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="inShowDate">画面に表示されてる日時</param>
    ''' <param name="inLastRefreshTime">最新の更新日時</param>
    ''' <returns>最新のチップ情報</returns>
    ''' <remarks></remarks>
    Public Function GetStallChipAfterOperation(ByVal dlrCode As String, _
                                               ByVal brnCode As String, _
                                               ByVal inShowDate As Date, _
                                               ByVal inLastRefreshTime As Date) As TabletSmbCommonClassStallChipInfoDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture _
                                , "{0}.Start. dlrCode={1}, brnCode={2}, inShowDate={3}, inLastRefreshTime={4}" _
                                , MethodBase.GetCurrentMethod.Name _
                                , dlrCode _
                                , brnCode _
                                , inShowDate _
                                , inLastRefreshTime))

        Dim chipList As TabletSmbCommonClassStallChipInfoDataTable = Nothing

        '■■■■■ストール上更新されたチップの情報を取得 2-18 3-0-0-0-0-0 START■■■■■
        LogServiceCommonBiz.OutputLog(52, "●■● 2.3 ストール上更新されたチップの情報を取得 START")

        '営業開始終了日時を取得する
        Dim operTime As TabletSmbCommonClassBranchOperatingHoursDataTable = _
            Me.GetOneDayBrnOperatingHours(inShowDate, dlrCode, brnCode)

        LogServiceCommonBiz.OutputLog(52, "●■● 2.3 ストール上更新されたチップの情報を取得 END")
        '■■■■■ストール上更新されたチップの情報を取得 2-18 3-0-0-0-0-0 END■■■■■

        'エラーの場合
        If IsNothing(operTime) Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.End. Chip count is = 0(GetOneDayBrnOperatingHours is nothing)", _
                                      MethodBase.GetCurrentMethod.Name))

            '空白テーブルを返す
            Return New TabletSmbCommonClassStallChipInfoDataTable

        End If

        'rowUpdateDate以後更新されたチップ情報を取得
        chipList = Me.GetAllStallChip(dlrCode, _
                                      brnCode, _
                                      operTime(0).SVC_JOB_START_TIME, _
                                      operTime(0).SVC_JOB_END_TIME, _
                                      Nothing, _
                                      inLastRefreshTime)

        '現在日時を取得
        Dim dtNow As Date = DateTimeFunc.Now(dlrCode)

        'チップが一つでもない場合
        If chipList.Count = 0 Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.End Chip count is = 0", _
                                      MethodBase.GetCurrentMethod.Name))

            Return chipList

        End If

        Dim svcinIdList As New List(Of Decimal)
        'サービス入庫IDのリストを作成
        For Each drChipInfo As TabletSmbCommonClassStallChipInfoRow In chipList
            '重複のサービス入庫IDをいれない
            If Not svcinIdList.Contains(drChipInfo.SVCIN_ID) Then
                svcinIdList.Add(drChipInfo.SVCIN_ID)
            End If
        Next

        '遅れ見込み列のデータを取得する
        Dim dtDelay As TabletSmbCommonClassDeliDelayDateDataTable = _
            Me.GetDeliveryDelayDateList(svcinIdList, dlrCode, brnCode, dtNow)

        'チップテーブルに遅れ見込み日時を登録する
        For Each drDelay As TabletSmbCommonClassDeliDelayDateRow In dtDelay

            Dim svcinId As Decimal = drDelay.SVCIN_ID

            'ループしてるサービス入庫IDのチップを洗い出す
            Dim arrChipInfo = (From p In chipList Where p.SVCIN_ID = svcinId Select p).ToArray()

            '洗い出したチップに遅れ見込み日時を登録
            For nLoop = 0 To arrChipInfo.Count - 1

                arrChipInfo(nLoop).PLAN_DELAYDATE = drDelay.DELI_DELAY_DATETIME
                '2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 START
                '遅れ見込み情報から残完成検査区分を取得
                arrChipInfo(nLoop).REMAINING_INSPECTION_TYPE = drDelay.REMAINING_INSPECTION_TYPE
                '2017/09/25 NSK 竹中(悠) REQ-SVT-TMT-20170227-002 見込納車遅れ日時計算ロジック統一 END

            Next

        Next

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E Chip count is = {1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  chipList.Count))
        Return chipList

    End Function
    '2014/09/12 TMEJ 張 BTS-390 「1ｽﾄｰﾙで2ﾁｯﾌﾟが作業中（表示のみ）」対応 END

    ''' <summary>
    ''' ストール上の仮仮チップ情報を取得する
    ''' </summary>
    ''' <param name="dtNow">今の時間</param>
    ''' <param name="stallList">ストールリスト</param>
    ''' <param name="dtStallStartTime">営業開始時間</param>
    ''' <param name="dtStallEndTime">営業終了時間</param>
    ''' <returns>仮仮チップ情報</returns>
    ''' <remarks></remarks>
    Public Function GetAllStallKariKariChip(ByVal stallList As List(Of Decimal), _
                                            ByVal dtNow As Date, _
                                            ByVal dtStallStartTime As Date, _
                                            ByVal dtStallEndTime As Date) As TabletSmbCommonClassKariKariChipInfoDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dtNow={1}", MethodBase.GetCurrentMethod.Name, dtNow))
        Dim chipList As TabletSmbCommonClassKariKariChipInfoDataTable
        '仮仮チップ情報を取得する
        Using ta As New TabletSMBCommonClassDataAdapter
            chipList = ta.GetKariKariChipByStallId(stallList, dtNow, dtStallStartTime, dtStallEndTime)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return chipList
    End Function
#End Region

#Region "関連チップ情報の取得"
    ''' <summary>
    ''' サービス入庫IDにより、関連チップ情報を取得	
    ''' </summary>
    ''' <param name="svcInIdList">サービス入庫IDリスト</param>
    ''' <returns></returns>
    Public Function GetAllRelationChipInfo(ByVal svcInIdList As List(Of Decimal)) As TabletSmbCommonClassRelationChipInfoDataDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Dim dtResult As TabletSmbCommonClassRelationChipInfoDataDataTable
        'サービス入庫IDにより、関連チップ情報を取得
        Using ta As New TabletSMBCommonClassDataAdapter
            dtResult = ta.GetAllRelationChipInfo(svcInIdList)
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return dtResult
    End Function


#End Region

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
#Region "指定チーフテクニシャンがあるストールIDの取得"
    ''' <summary>
    ''' 指定チーフテクニシャンがあるストールIDの取得
    ''' </summary>
    ''' <param name="account">チーフテクニシャンアカウント</param>
    ''' <returns>ストールID</returns>
    ''' <remarks></remarks>
    Public Function GetStallidByChtAccount(ByVal account As String) As TabletSmbCommonClassNumberValueDataTable
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. account={1}" _
                , MethodBase.GetCurrentMethod.Name, account))

        Dim stallidTable As TabletSmbCommonClassNumberValueDataTable

        Using ta As New TabletSMBCommonClassDataAdapter
            stallidTable = ta.GetStallidByChtAccount(account)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return stallidTable
    End Function
#End Region
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '2015/09/08 TMEJ 皆川 タブレットSMB ストールグループ表示対応 START
#Region "タブレットSMB ストールグループ表示対応"

    ''' <summary>
    ''' スタッフストール割当取得
    ''' </summary>
    ''' <param name="account">ログインスタッフアカウント</param>
    ''' <returns>ストールIDテーブル</returns>
    ''' <remarks></remarks>
    Public Function GetStaffStall(ByVal account As String) As TabletSmbCommonClassNumberValueDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. account={1}" _
                , MethodBase.GetCurrentMethod.Name, account))

        '組織IDリストを初期化する
        Dim orgnz As List(Of Decimal)

        Using ta As New TabletSMBCommonClassDataAdapter
            '組織IDリストを取得する
            orgnz = ta.GetOrgnzIdByAccount(account)
        End Using

        '2016/01/12 NSK 皆川 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START

        'Dim orgnzIdList As List(Of Decimal)

        ''組織IDリストが空の場合
        'If IsNothing(orgnz) OrElse orgnz.Count = 0 Then
        '    orgnzIdList = New List(Of Decimal)
        'Else
        '    Dim parentOrgnzIdList As List(Of Decimal) = New List(Of Decimal)
        '    parentOrgnzIdList.Add(orgnz.First())
        '    orgnzIdList = Me.GetOrganizationList(orgnz.First(), parentOrgnzIdList)
        'End If

        ''組織IDリストからストールIDテーブル作成
        'Dim stallIdDataTable As TabletSmbCommonClassNumberValueDataTable = Me.GetStallIdList(orgnzIdList)
        Dim stallIdDataTable As TabletSmbCommonClassNumberValueDataTable = Me.GetStallIdList(orgnz)

        '2016/01/12 NSK 皆川 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Count={1}" _
                , MethodBase.GetCurrentMethod.Name, stallIdDataTable.Count))

        Return stallIdDataTable

    End Function

    '2016/01/12 NSK 皆川 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
    ' ''' <summary>
    ' ''' 組織IDリスト取得
    ' ''' </summary>
    ' ''' <param name="parentOrgnzId">親組織ID</param>
    ' ''' <param name="parentOrgnzIdList">親組織IDリスト</param>
    ' ''' <returns>組織IDリスト</returns>
    'Private Function GetOrganizationList(ByVal parentOrgnzId As Decimal, parentOrgnzIdList As List(Of Decimal)) As List(Of Decimal)

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. parentOrgnzId={1}. parentOrgnzIdList={2}" _
    '            , MethodBase.GetCurrentMethod.Name, parentOrgnzId, String.Join(",", parentOrgnzIdList)))

    '    '返却用の組織IDリストを作成する
    '    Dim orgnzIdList As List(Of Decimal) = New List(Of Decimal)
    '    orgnzIdList.Add(parentOrgnzId)

    '    Dim orgnzSet As List(Of Decimal)
    '    Using ta As New TabletSMBCommonClassDataAdapter
    '        '該当の組織を親として持つ組織マスタを取得する
    '        orgnzSet = ta.GetChildOrgnzIdByParentOrgnzId(parentOrgnzId)
    '    End Using

    '    '子組織が存在しない場合
    '    If IsNothing(orgnzSet) OrElse orgnzSet.Count = 0 Then

    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Count={1}" _
    '            , MethodBase.GetCurrentMethod.Name, orgnzIdList.Count))

    '        Return orgnzIdList
    '    End If

    '    '入力データ．組織IDリストと一致する組織IDの組織マスタの件数を取得する
    '    Dim orgnzIdCollisionCount As Integer = (From p In orgnzSet Where parentOrgnzIdList.Contains(p)).Count()

    '    '入力データ．組織IDリストと一致する組織マスタが存在した場合
    '    '※親子関係が逆転するような組織が存在した場合
    '    If 0 < orgnzIdCollisionCount Then

    '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
    '                                , "{0}.{1} ErrPram:parentOrgnzId = {2} and parentOrgnzIdList = {3}" _
    '                                , Me.GetType.ToString _
    '                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
    '                                , parentOrgnzId, String.Join(",", parentOrgnzIdList)))

    '        Throw New InvalidOperationException("Parent And Child ORGNZ_ID The Same.")
    '    End If

    '    '取得した組織マスタ分繰り返す
    '    For Each orgnz As Decimal In orgnzSet

    '        'リスト+自分のIDを保持する
    '        Dim parentIdList As List(Of Decimal) = New List(Of Decimal)(parentOrgnzIdList)
    '        parentIdList.Add(orgnz)

    '        '親組織IDリストから子の組織IDリストを取得する
    '        Dim childOrgnzIdList As List(Of Decimal) = Me.GetOrganizationList(orgnz, parentIdList)

    '        '取得した組織IDリストを返却用の組織IDリストに追加する
    '        orgnzIdList.AddRange(childOrgnzIdList)
    '    Next

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Count={1}" _
    '            , MethodBase.GetCurrentMethod.Name, orgnzIdList.Count))

    '    Return orgnzIdList

    'End Function
    '2016/01/12 NSK 皆川 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

    ''' <summary>
    ''' ストールIDテーブル取得
    ''' </summary>
    ''' <param name="orgnzIdList">組織IDリスト</param>
    ''' <returns>ストールIDテーブル</returns>
    Private Function GetStallIdList(ByVal orgnzIdList As List(Of Decimal)) As TabletSmbCommonClassNumberValueDataTable

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. orgnzIdList={1}" _
                , MethodBase.GetCurrentMethod.Name, String.Join(",", orgnzIdList)))

        '引数．組織IDリストが空の場合
        If IsNothing(orgnzIdList) OrElse orgnzIdList.Count = 0 Then
            '空のデータテーブルを設定

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Count={1}" _
                , MethodBase.GetCurrentMethod.Name, 0))

            Return New TabletSmbCommonClassNumberValueDataTable

        End If

        Dim stallIdTable As TabletSmbCommonClassNumberValueDataTable
        Using ta As New TabletSMBCommonClassDataAdapter
            '該当の組織に紐付くストールを取得する
            stallIdTable = ta.GetStallIdByOrgnzId(orgnzIdList)
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Count={1}" _
                , MethodBase.GetCurrentMethod.Name, stallIdTable.Count))

        Return stallIdTable

    End Function

#End Region
    '2015/09/08 TMEJ 皆川 タブレットSMB ストールグループ表示対応 END

#End Region

#Region "本予約処理"
    ''' <summary>
    ''' 本予約
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="stallUseId">サービス入庫ID</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="systemId">呼ぶ画面ID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function Reserve(ByVal svcinId As Decimal, _
                            ByVal stallUseId As Decimal, _
                            ByVal updateDate As Date, _
                            ByVal objStaffContext As StaffContext, _
                            ByVal systemId As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. srvInId={1}, stallUseId={2}, updateDate={3}, systemId={4}" _
                                , MethodBase.GetCurrentMethod.Name, svcinId, stallUseId, updateDate, systemId))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Using ta As New TabletSMBCommonClassDataAdapter

            ' チップエンティティを取得する
            Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId)
            If dtChipEntity.Count <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                    , MethodBase.GetCurrentMethod.Name))
                Return ActionResult.GetChipEntityError
            End If

            '日跨ぎ終了を含むか否か
            Dim containsMidfinishChip As Boolean = GetContainsMidfinishChip(objStaffContext.DlrCD, objStaffContext.BrnCD, svcinId)
            ' ステータス遷移可否をチェックする
            If Not CanReserve(dtChipEntity(0).SVC_STATUS, _
                dtChipEntity(0).STALL_USE_STATUS, _
                containsMidfinishChip, _
                dtChipEntity(0).RESV_STATUS, _
                dtChipEntity(0).ACCEPTANCE_TYPE) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
                                        , MethodBase.GetCurrentMethod.Name))
                Return ActionResult.CheckError
            End If

            '変更前の情報を取得する
            Dim dtServiceinBefore As TabletSmbCommonClassServiceinChangeInfoDataTable
            dtServiceinBefore = ta.GetChipChangeInfo(svcinId, dtChipEntity(0).DLR_CD, dtChipEntity(0).BRN_CD)
            '予約送信ため、変更前のチップステータス、予約ステータスを取得する
            Dim preChipStatus As String = Me.JudgeChipStatus(stallUseId)
            Dim preResvStatus As String = dtChipEntity(0).RESV_STATUS

            ' 更新処理を実行する
            Dim cnt As Long = 0
            '予約区分を本予約に変更する
            cnt = ta.UpdateServiceinResvStatus(svcinId, ResvStatusConfirmed, objStaffContext.Account)
            If cnt <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Failed to update TB_T_SERVICEIN. SVCIN_ID={1},  UPDATE_DATETIME={2}, UPDATE_STF_CD={3}" _
                                        , MethodBase.GetCurrentMethod.Name, svcinId, updateDate, objStaffContext.Account))
                Return ActionResult.ExceptionError
            End If

            '変更後の情報を取得する
            Dim dtServiceinAfter As TabletSmbCommonClassServiceinChangeInfoDataTable
            dtServiceinAfter = ta.GetChipChangeInfo(svcinId, dtChipEntity(0).DLR_CD, dtChipEntity(0).BRN_CD)
            '予約送信ため、変更後のチップステータスを取得する
            Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)

            '履歴登録
            cnt = CreateChipOperationHistory(dtServiceinBefore, dtServiceinAfter, updateDate, objStaffContext.Account, 0, systemId)
            If cnt <> 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} HISINSERT FAILURE " _
                            , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                Return ActionResult.ExceptionError
            End If

            '予約送信
            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
            'cnt = Me.SendReserveInfo(svcinId, dtChipEntity(0).JOB_DTL_ID, stallUseId, preChipStatus, _
            '        crntStatus, preResvStatus, systemId)
            Using biz3800903 As New IC3800903BusinessLogic
                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'cnt = biz3800903.SendReserveInfo(svcinId, _
                '                 dtChipEntity(0).JOB_DTL_ID, _
                '                 stallUseId, _
                '                 preChipStatus, _
                '                 crntStatus, _
                '                 preResvStatus, _
                '                 systemId)

                '予約連携実施
                Dim returnCodeSendReserve As Integer = biz3800903.SendReserveInfo(svcinId, _
                                                                                  dtChipEntity(0).JOB_DTL_ID, _
                                                                                  stallUseId, _
                                                                                  preChipStatus, _
                                                                                  crntStatus, _
                                                                                  preResvStatus, _
                                                                                  systemId)


                '処理結果チェック
                If returnCodeSendReserve = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf returnCodeSendReserve = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合

                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

                    ''「15：他システムとの連携エラー」を返却
                    'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    '    , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                    '    , Me.GetType.ToString _
                    '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '    , ActionResult.DmsLinkageError))
                    'Return ActionResult.DmsLinkageError

                    '予約連携送信のエラーコードが文言コードの場合、文言コード（エラーコード）を返す。
                    '文言コードでない場合、「15：他システムとの連携エラー」を返す。
                    Dim returnValue As Integer = CheckReturnCodeSendReserveError(returnCodeSendReserve)

                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , returnValue))

                    Return returnValue

                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End Using
            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If cnt <> 0 Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendReserveInfo  FAILURE " _
            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'ステータス送信
            Using ic3802601blc As New IC3802601BusinessLogic
                Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(svcinId, _
                                                                        dtChipEntity(0).JOB_DTL_ID, _
                                                                        stallUseId, _
                                                                        preChipStatus, _
                                                                        crntStatus, _
                                                                        0)

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'If dmsSendResult <> 0 Then
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
                '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                '    Return ActionResult.DmsLinkageError
                'End If

                '処理結果チェック
                If dmsSendResult = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End Using
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        End Using

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

    ''' <summary>
    ''' 予約連携送信のエラーコードが文言コードか判定する
    ''' </summary>
    ''' <param name="returnCodeSendReserve">予約連携送信エラーコード</param>
    ''' <returns>
    ''' 予約連携送信のエラーコードが文言コードの場合、文言コード（エラーコード）を返す。
    ''' 文言コードでない場合、「15：他システムとの連携エラー」を返す。
    ''' </returns>
    ''' <remarks></remarks>
    Private Function CheckReturnCodeSendReserveError(ByVal returnCodeSendReserve As Integer) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. returnCodeSendReserve={1}" _
                     , MethodBase.GetCurrentMethod.Name, returnCodeSendReserve))

        Dim returnCode As Integer = 0

        If ActionResult.IC3800903ResultRangeLower <= returnCodeSendReserve _
                    AndAlso returnCodeSendReserve <= ActionResult.IC3800903ResultRangeUpper Then
            '予約連携エラーコードが8000以上かつ8999以下の場合
            '予約連携エラーコードを返却
            returnCode = returnCodeSendReserve

        Else
            '上記以外の場合
            '「15：他システムとの連携エラー」を返却
            returnCode = ActionResult.DmsLinkageError

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E result={1}", MethodBase.GetCurrentMethod.Name, returnCode))

        Return returnCode

    End Function

    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

#Region "本予約、仮予約用--日跨ぎ終了を含むか否か取得"
    ''' <summary>
    ''' 日跨ぎ終了を含むか否かを取得する.
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="srvInId">サービス入庫ID</param>
    ''' <returns>日跨ぎ終了を含むか否か</returns>
    ''' <remarks></remarks>
    Private Function GetContainsMidfinishChip(ByVal dlrCode As String, ByVal brnCode As String, ByVal srvInId As Decimal) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. srvInId={1}" _
                     , MethodBase.GetCurrentMethod.Name, srvInId))

        Dim rtResult = False
        Using ta As New TabletSMBCommonClassDataAdapter
            Dim dtResult As TabletSmbCommonClassNumberValueDataTable = ta.GetContainsMidfinishChip(dlrCode, brnCode, srvInId)
            If dtResult.Count > 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E result=True", MethodBase.GetCurrentMethod.Name))
                rtResult = True
            End If
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E result={1}", MethodBase.GetCurrentMethod.Name, rtResult))
        Return rtResult
    End Function
#End Region

#End Region

#Region "仮予約処理"

    ''' <summary>
    '''   仮予約処理
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="stallUseId">サービス入庫ID</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="systemId">呼ぶ画面ID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function TentativeReserve(ByVal svcinId As Decimal, _
                                     ByVal stallUseId As Decimal, _
                                     ByVal updateDate As Date, _
                                     ByVal objStaffContext As StaffContext, _
                                     ByVal systemId As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. srvInId={1}, stallUseId={2}, updateDate={3}, systemId={4}" _
                                , MethodBase.GetCurrentMethod.Name, svcinId, stallUseId, updateDate, systemId))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Using ta As New TabletSMBCommonClassDataAdapter

            Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId)
            If dtChipEntity.Count <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                                , MethodBase.GetCurrentMethod.Name))
                Return ActionResult.GetChipEntityError
            End If
            ' ステータス遷移可否をチェックする
            Dim containsMidfinishChip As Boolean = GetContainsMidfinishChip(objStaffContext.DlrCD, objStaffContext.BrnCD, svcinId)
            If Not CanTentativeReserve(dtChipEntity(0).SVC_STATUS, _
                                    dtChipEntity(0).STALL_USE_STATUS, _
                                    containsMidfinishChip, _
                                    dtChipEntity(0).RESV_STATUS, _
                                     dtChipEntity(0).ACCEPTANCE_TYPE) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError." _
                                       , MethodBase.GetCurrentMethod.Name))
                Return ActionResult.CheckError
            End If

            '変更前の情報を取得する
            Dim dtServiceinBefore As TabletSmbCommonClassServiceinChangeInfoDataTable
            dtServiceinBefore = ta.GetChipChangeInfo(svcinId, dtChipEntity(0).DLR_CD, dtChipEntity(0).BRN_CD)
            '予約送信ため、変更前のチップステータス、予約ステータスを取得する
            Dim preChipStatus As String = Me.JudgeChipStatus(stallUseId)
            Dim preResvStatus As String = dtChipEntity(0).RESV_STATUS

            ' 更新処理を実行する
            Dim cnt As Long = 0
            '予約区分を仮予約に変更する
            cnt = ta.UpdateServiceinResvStatus(svcinId, ResvStatusTentative, objStaffContext.Account)
            If cnt <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Failed to update TB_T_SERVICEIN. SVCIN_ID={1},  UPDATE_DATETIME={2}, UPDATE_STF_CD={3}" _
                                                          , MethodBase.GetCurrentMethod.Name, svcinId, updateDate, objStaffContext.Account))
                Return ActionResult.ExceptionError
            End If

            '変更後の情報を取得する
            Dim dtServiceinAfter As TabletSmbCommonClassServiceinChangeInfoDataTable
            dtServiceinAfter = ta.GetChipChangeInfo(svcinId, dtChipEntity(0).DLR_CD, dtChipEntity(0).BRN_CD)
            '予約送信ため、変更後のチップステータスを取得する
            Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)

            '履歴登録
            cnt = CreateChipOperationHistory(dtServiceinBefore, dtServiceinAfter, updateDate, objStaffContext.Account, 0, systemId)
            If cnt <> 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} HISINSERT FAILURE " _
                            , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                Return ActionResult.ExceptionError
            End If

            '予約送信
            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
            'cnt = Me.SendReserveInfo(svcinId, dtChipEntity(0).JOB_DTL_ID, stallUseId, preChipStatus, _
            '                    crntStatus, preResvStatus, systemId)
            Using biz3800903 As New IC3800903BusinessLogic
                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'cnt = biz3800903.SendReserveInfo(svcinId, _
                '                                 dtChipEntity(0).JOB_DTL_ID, _
                '                                 stallUseId, _
                '                                 preChipStatus, _
                '                                 crntStatus, _
                '                                 preResvStatus, _
                '                                 systemId)

                '予約連携実施
                Dim returnCodeSendReserve As Integer = biz3800903.SendReserveInfo(svcinId, _
                                                                                  dtChipEntity(0).JOB_DTL_ID, _
                                                                                  stallUseId, _
                                                                                  preChipStatus, _
                                                                                  crntStatus, _
                                                                                  preResvStatus, _
                                                                                  systemId)

                '処理結果チェック
                If returnCodeSendReserve = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf returnCodeSendReserve = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合

                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

                    ''「15：他システムとの連携エラー」を返却
                    'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    '    , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                    '    , Me.GetType.ToString _
                    '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '    , ActionResult.DmsLinkageError))
                    'Return ActionResult.DmsLinkageError

                    '予約連携送信のエラーコードが文言コードの場合、文言コード（エラーコード）を返す。
                    '文言コードでない場合、「15：他システムとの連携エラー」を返す。
                    Dim returnValue As Integer = CheckReturnCodeSendReserveError(returnCodeSendReserve)

                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , returnValue))

                    Return returnValue

                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End Using

            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If cnt <> 0 Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendReserveInfo FAILURE " _
            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'ステータス送信
            Using ic3802601blc As New IC3802601BusinessLogic
                Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(svcinId, _
                                                                        dtChipEntity(0).JOB_DTL_ID, _
                                                                        stallUseId, _
                                                                        preChipStatus, _
                                                                        crntStatus, _
                                                                        0)

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'If dmsSendResult <> 0 Then
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
                '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                '    Return ActionResult.DmsLinkageError
                'End If

                '処理結果チェック
                If dmsSendResult = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End Using

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        End Using

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

#End Region

#Region "入庫処理"

    ''' <summary>
    '''   入庫
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rsltServiceinDateTime">実績入庫日時</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="systemId">呼ぶ画面ID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function CarIn(ByVal svcinId As Decimal, _
                          ByVal stallUseId As Decimal, _
                          ByVal rsltServiceinDateTime As Date,
                          ByVal updateDate As Date, _
                          ByVal staffCode As String, _
                          ByVal systemId As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcinId={1}, stallUseId={2}, rsltServiceinDateTime={3}, updateDate={4}, staffCode={5}" _
                                , MethodBase.GetCurrentMethod.Name, svcinId, stallUseId, rsltServiceinDateTime, updateDate, staffCode))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        ' 秒を切り捨てた日時を取得する
        Dim rsltServiceinDateTimeNoSec As Date = Me.GetDateTimeFloorSecond(rsltServiceinDateTime)
        ' チップエンティティを取得する
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId)
        If dtChipEntity.Count <> 1 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                            , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.GetChipEntityError
        End If

        ' ステータス遷移可否をチェックする
        If Not CanCarIn(dtChipEntity(0).SVC_STATUS, _
                    dtChipEntity(0).TEMP_FLG, _
                    dtChipEntity(0).STALL_ID, _
                    dtChipEntity(0).SCHE_START_DATETIME) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError." _
                                       , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.CheckError
        End If

        Using ta As New TabletSMBCommonClassDataAdapter

            '変更前の情報を取得する
            Dim dtServiceinBefore As TabletSmbCommonClassServiceinChangeInfoDataTable = _
                            ta.GetChipChangeInfo(svcinId, dtChipEntity(0).DLR_CD, dtChipEntity(0).BRN_CD)
            '予約送信ため、変更前のチップステータス、予約ステータスを取得する
            Dim preChipStatus As String = Me.JudgeChipStatus(stallUseId)
            Dim preResvStatus As String = dtChipEntity(0).RESV_STATUS

            ' 更新処理を実行する
            Dim cnt As Long = 0
            ' 実績入庫日時を更新する
            cnt = ta.UpdateRsltServiceinDate(svcinId, SvcStatusStartwait, rsltServiceinDateTimeNoSec, updateDate, staffCode)
            If cnt <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Failed to update TB_T_SERVICEIN. srvseq={1}, Account={2}" _
                                        , MethodBase.GetCurrentMethod.Name, svcinId, StaffContext.Current.Account))
                Return ActionResult.ExceptionError        ' ストール予約情報の更新に失敗
            End If

            '変更後の情報を取得する
            Dim dtServiceinAfter As TabletSmbCommonClassServiceinChangeInfoDataTable
            dtServiceinAfter = ta.GetChipChangeInfo(svcinId, dtChipEntity(0).DLR_CD, dtChipEntity(0).BRN_CD)
            '予約送信ため、変更後のチップステータスを取得する
            Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)

            '履歴登録
            cnt = CreateChipOperationHistory(dtServiceinBefore, dtServiceinAfter, updateDate, staffCode, 0, systemId)
            If cnt <> 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} HISINSERT FAILURE " _
                            , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                Return ActionResult.ExceptionError
            End If

            '予約送信
            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
            'cnt = Me.SendReserveInfo(svcinId, dtChipEntity(0).JOB_DTL_ID, stallUseId, preChipStatus, _
            '                    crntStatus, preResvStatus, systemId)
            Using biz3800903 As New IC3800903BusinessLogic

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
                'cnt = biz3800903.SendReserveInfo(svcinId, _
                '                                 dtChipEntity(0).JOB_DTL_ID, _
                '                                 stallUseId, _
                '                                 preChipStatus, _
                '                                 crntStatus, _
                '                                 preResvStatus, _
                '                                 systemId)

                '予約連携実施
                Dim returnCodeSendReserve As Integer = biz3800903.SendReserveInfo(svcinId, _
                                                                                  dtChipEntity(0).JOB_DTL_ID, _
                                                                                  stallUseId, _
                                                                                  preChipStatus, _
                                                                                  crntStatus, _
                                                                                  preResvStatus, _
                                                                                  systemId)

                '処理結果チェック
                If returnCodeSendReserve = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf returnCodeSendReserve = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合

                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

                    ''「15：他システムとの連携エラー」を返却
                    'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    '    , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                    '    , Me.GetType.ToString _
                    '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '    , ActionResult.DmsLinkageError))
                    'Return ActionResult.DmsLinkageError

                    '予約連携送信のエラーコードが文言コードの場合、文言コード（エラーコード）を返す。
                    '文言コードでない場合、「15：他システムとの連携エラー」を返す。
                    Dim returnValue As Integer = CheckReturnCodeSendReserveError(returnCodeSendReserve)

                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , returnValue))

                    Return returnValue

                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

                End If
                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End Using

            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If cnt <> 0 Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendReserveInfo FAILURE " _
            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'ステータス送信
            Using ic3802601blc As New IC3802601BusinessLogic
                Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(svcinId, _
                                                                        dtChipEntity(0).JOB_DTL_ID, _
                                                                        stallUseId, _
                                                                        preChipStatus, _
                                                                        crntStatus, _
                                                                        0)

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'If dmsSendResult <> 0 Then
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
                '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                '    Return ActionResult.DmsLinkageError
                'End If

                '処理結果チェック
                If dmsSendResult = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End Using

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        End Using


        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

#End Region

#Region "入庫取消処理"

    ''' <summary>
    '''   入庫取消処理
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="systemId">呼ぶ画面ID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function CancelCarIn(ByVal svcinId As Decimal, _
                                ByVal stallUseId As Decimal, _
                                ByVal updateDate As Date, _
                                ByVal staffCode As String, _
                                ByVal systemId As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. svcinId={1}, stallUseId={2}, updateDate={3}, updateDate={4}" _
                                , MethodBase.GetCurrentMethod.Name, svcinId, stallUseId, updateDate, staffCode))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        ' チップエンティティを取得する
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId)
        If dtChipEntity.Count <> 1 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                            , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.GetChipEntityError
        End If

        ' ステータス遷移可否をチェックする
        Dim containsResultChip As Boolean = False
        If Not CanCancelCarIn(dtChipEntity(0).SVC_STATUS, _
                            containsResultChip,
                            dtChipEntity(0).TEMP_FLG, _
                            dtChipEntity(0).STALL_ID, _
                            dtChipEntity(0).SCHE_START_DATETIME) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError." _
                       , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.CheckError
        End If

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '予約送信ため、変更前のチップステータス、予約ステータスを取得する
        Dim preChipStatus As String = Me.JudgeChipStatus(stallUseId)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Using ta As New TabletSMBCommonClassDataAdapter
            '変更前の情報を取得する
            Dim dtServiceinBefore As TabletSmbCommonClassServiceinChangeInfoDataTable
            dtServiceinBefore = ta.GetChipChangeInfo(svcinId, dtChipEntity(0).DLR_CD, dtChipEntity(0).BRN_CD)

            ' 更新処理を実行する
            Dim cnt As Long = 0
            cnt = ta.UpdateRsltServiceinDate(svcinId, SvcStatusNotCarin, DefaultDateTimeValueGet(), updateDate, staffCode)      '実績入庫日時を更新する
            If cnt <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Failed to update TB_T_SERVICEIN. " _
                                        , MethodBase.GetCurrentMethod.Name))
                Return ActionResult.ExceptionError        ' ストール予約情報の更新に失敗
            End If

            '変更後の情報を取得する
            Dim dtServiceinAfter As TabletSmbCommonClassServiceinChangeInfoDataTable
            dtServiceinAfter = ta.GetChipChangeInfo(svcinId, dtChipEntity(0).DLR_CD, dtChipEntity(0).BRN_CD)
            '履歴登録
            cnt = CreateChipOperationHistory(dtServiceinBefore, dtServiceinAfter, updateDate, staffCode, 0, systemId)
            If cnt <> 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} HISINSERT FAILURE " _
                            , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                Return ActionResult.ExceptionError
            End If
        End Using

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)
        'ステータス送信
        Using ic3802601blc As New IC3802601BusinessLogic
            Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(svcinId, _
                                                                    dtChipEntity(0).JOB_DTL_ID, _
                                                                    stallUseId, _
                                                                    preChipStatus, _
                                                                    crntStatus, _
                                                                    0)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If dmsSendResult <> 0 Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If


            '処理結果チェック
            If dmsSendResult = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

#End Region

#Region "チップの移動、リサイズ処理"

    ''' <summary>
    ''' ストール上の一つのチップを移動、リサイズ
    ''' </summary>
    ''' <param name="stallUseId">チップの予約ID</param>
    ''' <param name="stallId">移動先のストールのSTALLID</param>
    ''' <param name="dispStartDateTime">移動先の表示開始日時</param>
    ''' <param name="scheWorkTime">仕事時間</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="systemId">更新クラス</param>
    ''' <param name="dtNow">今の時刻</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function MoveAndResize(ByVal stallUseId As Decimal, _
                                    ByVal stallId As Decimal, _
                                    ByVal dispStartDateTime As Date, _
                                    ByVal scheWorkTime As Long, _
                                    ByVal restFlg As String, _
                                    ByVal stallStartTime As Date, _
                                    ByVal stallEndTime As Date, _
                                    ByVal updateDate As Date, _
                                    ByVal objStaffContext As StaffContext, _
                                    ByVal systemId As String, _
                                    ByVal dtNow As Date, _
                                    ByVal rowLockVersion As Long) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, stallId={2}, dispStartDateTime={3}, scheWorkTime={4}, restFlg={5}, stallStartTime={6}, stallEndTime={7}, staffCode={8}" _
                                , MethodBase.GetCurrentMethod.Name, stallUseId, stallId, dispStartDateTime, scheWorkTime, restFlg, stallStartTime, stallEndTime, objStaffContext.Account))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Dim resultCode As Long = ActionResult.Success
        ' チップエンティティを取得する
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId)
        If dtChipEntity.Count <> 1 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                            , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.GetChipEntityError
        End If
        Dim drChipInfo As TabletSmbCommonClassChipEntityRow = dtChipEntity(0)

        ' 休憩フラグがない場合、元のdbの値を使ってる
        If IsNothing(restFlg) Then
            restFlg = RestTimeGetFlgGetRest
        End If
        ' ステータス遷移可否をチェックする
        If Not CanMoveAndResize(drChipInfo.RSLT_END_DATETIME, drChipInfo.STALL_USE_STATUS) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
                    , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.CheckError
        End If

        '予約送信ため、変更前のチップステータス、予約ステータスを取得する
        Dim preChipStatus As String = Me.JudgeChipStatus(stallUseId)
        Dim preResvStatus As String = drChipInfo.RESV_STATUS

        'ローカル変数．作業終了日時として処理対象のストール利用．予定終了日時を保持する
        Dim serviceWorkTime As Long = scheWorkTime

        Dim rsltStartTime As Date = drChipInfo.RSLT_START_DATETIME
        Dim prmsEndDateTime As Date = drChipInfo.PRMS_END_DATETIME
        Dim scheStartDateTime As Date = drChipInfo.SCHE_START_DATETIME
        Dim scheEndDateTime As Date = drChipInfo.SCHE_END_DATETIME
        Dim stallUseStatus As String = drChipInfo.STALL_USE_STATUS

        '開始時間を取得する
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        'Dim truncSecondDispStartDateTime As Date
        Dim truncSecondDispStartDateTime As Date = dispStartDateTime
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
        Dim serviceWorkEndDateTime As Date
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        Dim serviceEndDateTimeData As New ServiceEndDateTimeData
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
        '普通予約チップの場合
        If IsDefaultValue(rsltStartTime) OrElse stallUseStatus.Equals(StalluseStatusStop) Then
            '普通のチップの場合、開始時間を計算する(休憩チップと)
            truncSecondDispStartDateTime = Me.GetServiceStartDateTime(stallId, dispStartDateTime, stallStartTime, stallEndTime, restFlg)

            '普通の予約チップの場合、予約終了時間が変わる
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'scheEndDateTime = GetServiceEndDateTime(stallId, truncSecondDispStartDateTime, _
            '                                        serviceWorkTime, stallStartTime, stallEndTime, restFlg)
            serviceEndDateTimeData = GetServiceEndDateTime(stallId, truncSecondDispStartDateTime, _
                                                    serviceWorkTime, stallStartTime, stallEndTime, restFlg)
            scheEndDateTime = serviceEndDateTimeData.ServiceEndDateTime
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
            'チップ表示終了日時
            serviceWorkEndDateTime = scheEndDateTime
        Else
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            '実績チップの場合、開始時間が変わらない
            'truncSecondDispStartDateTime = dispStartDateTime
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

            '見込終了日時を取得する
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'prmsEndDateTime = GetServiceEndDateTime(stallId, rsltStartTime, _
            '                                        serviceWorkTime, stallStartTime, stallEndTime, restFlg)
            serviceEndDateTimeData = GetServiceEndDateTime(stallId, rsltStartTime, serviceWorkTime, _
                                                                stallStartTime, stallEndTime, restFlg)
            prmsEndDateTime = serviceEndDateTimeData.ServiceEndDateTime
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

            '予定終了日時が変わらない
            scheEndDateTime = drChipInfo.SCHE_END_DATETIME
            'チップ表示終了日時
            serviceWorkEndDateTime = prmsEndDateTime
        End If

        'チップ操作制約チェックを行う
        Dim validate As Integer = ValidateMove(stallUseId, objStaffContext, stallId, truncSecondDispStartDateTime, scheWorkTime, serviceWorkEndDateTime, stallStartTime, stallEndTime, dtNow)
        If validate <> ActionResult.Success Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E NotValidateMove" _
                    , MethodBase.GetCurrentMethod.Name))
            Return validate
        End If

        Dim svcinId As Decimal = drChipInfo.SVCIN_ID
        Dim cnt As Long
        Using ta As New TabletSMBCommonClassDataAdapter
            'update用データセット
            Using targetDtChipEntity As New TabletSmbCommonClassChipEntityDataTable
                Dim targetDrChipEntity As TabletSmbCommonClassChipEntityRow = CType(targetDtChipEntity.NewRow(), TabletSmbCommonClassChipEntityRow)
                'ストール利用のストール利用ステータスが「07：未来店客」の場合
                If stallUseStatus.Equals(StalluseStatusNoshow) Then
                    'Noshowから移動
                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                    'resultCode = Me.MoveFromNoShow(stallId, truncSecondDispStartDateTime, serviceWorkEndDateTime, serviceWorkTime, objStaffContext _
                    '                 , updateDate, restFlg, rowLockVersion, drChipInfo)
                    resultCode = Me.MoveFromNoShow(stallId, truncSecondDispStartDateTime, serviceWorkEndDateTime, serviceWorkTime, objStaffContext _
                                      , updateDate, serviceEndDateTimeData.RestFlg, rowLockVersion, drChipInfo)
                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                    If resultCode <> ActionResult.Success Then
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
                        Return resultCode
                    End If
                ElseIf stallUseStatus.Equals(StalluseStatusStop) Then
                    '中断から移動
                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                    'resultCode = Me.MoveFromStop(stallId, truncSecondDispStartDateTime, serviceWorkEndDateTime, serviceWorkTime, objStaffContext _
                    '                  , updateDate, restFlg, rowLockVersion, drChipInfo)
                    resultCode = Me.MoveFromStop(stallId, truncSecondDispStartDateTime, serviceWorkEndDateTime, serviceWorkTime, objStaffContext _
                                      , updateDate, serviceEndDateTimeData.RestFlg, rowLockVersion, drChipInfo)
                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                    If resultCode <> ActionResult.Success Then
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
                        Return resultCode
                    End If
                ElseIf IsDefaultValue(rsltStartTime) Then
                    'ストール上に移動する(作業前のチップ)
                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                    'resultCode = Me.MoveInStall(stallId, truncSecondDispStartDateTime, serviceWorkEndDateTime, serviceWorkTime, objStaffContext _
                    '                    , updateDate, restFlg, rowLockVersion, drChipInfo)
                    resultCode = Me.MoveInStall(stallId, truncSecondDispStartDateTime, serviceWorkEndDateTime, serviceWorkTime, objStaffContext _
                                        , updateDate, serviceEndDateTimeData.RestFlg, rowLockVersion, drChipInfo)
                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                    If resultCode <> ActionResult.Success Then
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
                        Return resultCode
                    End If
                Else
                    'サービス入庫をロックして、チェックする
                    Dim result As Long = LockServiceInTable(svcinId, rowLockVersion, objStaffContext.Account, dtNow, systemId)
                    If result <> ActionResult.Success Then
                        Me.Rollback = True
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                        Return result
                    End If
                    'ストール上に移動、リサイズ。作業中の場合、実績チップのりサイズ
                    targetDrChipEntity.STALL_USE_ID = stallUseId
                    targetDrChipEntity.STALL_ID = stallId
                    If IsDefaultValue(rsltStartTime) Then
                        targetDrChipEntity.SCHE_START_DATETIME = truncSecondDispStartDateTime
                    Else
                        targetDrChipEntity.SCHE_START_DATETIME = scheStartDateTime
                    End If

                    targetDrChipEntity.SCHE_END_DATETIME = scheEndDateTime
                    targetDrChipEntity.PRMS_END_DATETIME = prmsEndDateTime
                    targetDrChipEntity.SCHE_WORKTIME = serviceWorkTime
                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                    'targetDrChipEntity.REST_FLG = restFlg
                    targetDrChipEntity.REST_FLG = serviceEndDateTimeData.RestFlg
                    '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                    targetDrChipEntity.UPDATE_DATETIME = updateDate
                    targetDrChipEntity.UPDATE_STF_CD = objStaffContext.Account

                    '更新処理を実行する
                    cnt = ta.StallChipMoveResize(targetDrChipEntity, systemId)
                    If cnt <> 1 Then
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E StallChipMoveResize failed. cnt={1}" _
                                                , MethodBase.GetCurrentMethod.Name, cnt))
                        Return ActionResult.ExceptionError
                    End If
                End If
            End Using

            '予約送信ため、変更後のチップステータスを取得する
            Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)
            '予約送信
            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
            'cnt = Me.SendReserveInfo(svcinId, dtChipEntity(0).JOB_DTL_ID, stallUseId, preChipStatus, _
            '                    crntStatus, preResvStatus, systemId)
            Using biz3800903 As New IC3800903BusinessLogic

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'cnt = biz3800903.SendReserveInfo(svcinId, _
                '                                 dtChipEntity(0).JOB_DTL_ID, _
                '                                 stallUseId, _
                '                                 preChipStatus, _
                '                                 crntStatus, _
                '                                 preResvStatus, _
                '                                 systemId)

                '予約連携実施
                Dim returnCodeSendReserve As Integer = biz3800903.SendReserveInfo(svcinId, _
                                                                                  dtChipEntity(0).JOB_DTL_ID, _
                                                                                  stallUseId, _
                                                                                  preChipStatus, _
                                                                                  crntStatus, _
                                                                                  preResvStatus, _
                                                                                  systemId)

                '処理結果チェック
                If returnCodeSendReserve = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf returnCodeSendReserve = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合

                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

                    ''「15：他システムとの連携エラー」を返却
                    'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    '    , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                    '    , Me.GetType.ToString _
                    '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '    , ActionResult.DmsLinkageError))
                    'Return ActionResult.DmsLinkageError

                    '予約連携送信のエラーコードが文言コードの場合、文言コード（エラーコード）を返す。
                    '文言コードでない場合、「15：他システムとの連携エラー」を返す。
                    Dim returnValue As Integer = CheckReturnCodeSendReserveError(returnCodeSendReserve)

                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , returnValue))

                    Return returnValue

                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End Using

            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If cnt <> 0 Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendReserveInfo FAILURE " _
            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            ''End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'ステータス送信
            Using ic3802601blc As New IC3802601BusinessLogic
                'タブレットSMBでは、NoShow処理のみ予約ステータスを使うかつ予約ステータスを更新しないため、前後予約ステータスを更新前の予約ステータスを渡す
                Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(svcinId, _
                                                                        dtChipEntity(0).JOB_DTL_ID, _
                                                                        stallUseId, _
                                                                        preChipStatus, _
                                                                        crntStatus, _
                                                                        0, _
                                                                        preResvStatus, _
                                                                        preResvStatus)

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'If dmsSendResult <> 0 Then
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
                '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                '    Return ActionResult.DmsLinkageError
                'End If

                '処理結果チェック
                If dmsSendResult = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End Using

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    ''' <summary>
    ''' チップがNoShowエリアからストールに置く操作
    ''' </summary>
    ''' <param name="stallId">移動先のストールID</param>
    ''' <param name="startTime">移動先のチップ開始時間</param>
    ''' <param name="endTime">移動先のチップ終了時間</param>
    ''' <param name="serviceWorkTime">移動先のチップの幅対応する時間</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <param name="drChipInfo">チップの情報</param>
    ''' <returns>操作結果</returns>
    ''' <remarks></remarks>
    Private Function MoveFromNoShow(ByVal stallId As Decimal, _
                                    ByVal startTime As Date, _
                                    ByVal endTime As Date, _
                                    ByVal serviceWorkTime As Long, _
                                    ByVal objStaffContext As StaffContext, _
                                    ByVal updateDate As Date, _
                                    ByVal restFlg As String, _
                                    ByVal rowLockVersion As Long, _
                                    ByVal drChipInfo As TabletSmbCommonClassChipEntityRow) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        'NOSHOWからの移動
        'Webserviceを呼ぶためXML作成
        Dim xmlclass As New SMBCommonClassBusinessLogic.XmlDocumentClass
        xmlclass = StructWebServiceXml(drChipInfo.JOB_DTL_ID.ToString(CultureInfo.InvariantCulture), _
               "", _
               stallId.ToString(CultureInfo.InvariantCulture), _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", startTime), _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", endTime), _
               serviceWorkTime.ToString(CultureInfo.InvariantCulture), _
               objStaffContext, _
               updateDate, _
               GetWebServiceRestFlg(restFlg), _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", drChipInfo.SCHE_DELI_DATETIME), _
               "", _
               "", _
               "", _
               NoShowFlgNotNoShow, _
               "", _
               drChipInfo.PICK_DELI_TYPE, _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", drChipInfo.SCHE_SVCIN_DATETIME), _
               CType(rowLockVersion, String))
        Using commbiz As New SMBCommonClassBusinessLogic
            Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow = commbiz.CallReserveWebService(xmlclass)
            If drWebServiceResult.RESULTCODE <> 0 Then
                'RowLockVersionError(最新のデータではない)の場合、ActionResult.RowLockVersionErrorを戻す
                If drWebServiceResult.RESULTCODE = WebServiceRowLockVersionError Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E RowLockVersionError. " _
                        , MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.RowLockVersionError
                Else
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E StallChipMoveResize call webservice failed. RESULTCODE={1}" _
                        , MethodBase.GetCurrentMethod.Name, drWebServiceResult.RESULTCODE))
                    Return ActionResult.ExceptionError
                End If

            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return ActionResult.Success
    End Function

    ''' <summary>
    ''' チップが中断エリアからストールに置く操作
    ''' </summary>
    ''' <param name="stallId">移動先のストールID</param>
    ''' <param name="startTime">移動先のチップ開始時間</param>
    ''' <param name="endTime">移動先のチップ終了時間</param>
    ''' <param name="serviceWorkTime">移動先のチップの幅対応する時間</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <param name="drChipInfo">チップの情報</param>
    ''' <returns>操作結果</returns>
    ''' <remarks></remarks>
    Private Function MoveFromStop(ByVal stallId As Decimal, _
                                    ByVal startTime As Date, _
                                    ByVal endTime As Date, _
                                    ByVal serviceWorkTime As Long, _
                                    ByVal objStaffContext As StaffContext, _
                                    ByVal updateDate As Date, _
                                    ByVal restFlg As String, _
                                    ByVal rowLockVersion As Long, _
                                    ByVal drChipInfo As TabletSmbCommonClassChipEntityRow) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        '中断からの移動
        'Webserviceを呼ぶためXML作成
        Dim xmlclass As New SMBCommonClassBusinessLogic.XmlDocumentClass
        xmlclass = StructWebServiceXml(drChipInfo.JOB_DTL_ID.ToString(CultureInfo.InvariantCulture), _
                       "", _
                       stallId.ToString(CultureInfo.InvariantCulture), _
                       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", startTime), _
                       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", endTime), _
                       serviceWorkTime.ToString(CultureInfo.InvariantCulture), _
                       objStaffContext, _
                       updateDate, _
                       GetWebServiceRestFlg(restFlg), _
                       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", drChipInfo.SCHE_DELI_DATETIME), _
                       "", _
                       "", _
                       "", _
                       "", _
                       "", _
                       drChipInfo.PICK_DELI_TYPE, _
                       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", drChipInfo.SCHE_SVCIN_DATETIME), _
                       CType(rowLockVersion, String))

        Using commbiz As New SMBCommonClassBusinessLogic
            Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow = commbiz.CallReserveWebService(xmlclass)
            If drWebServiceResult.RESULTCODE <> 0 Then
                'RowLockVersionError(最新のデータではない)の場合、ActionResult.RowLockVersionErrorを戻す
                If drWebServiceResult.RESULTCODE = WebServiceRowLockVersionError Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E RowLockVersionError. " _
                    , MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.RowLockVersionError
                Else
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E StallChipMoveResize call webservice failed. RESULTCODE={1}" _
                    , MethodBase.GetCurrentMethod.Name, drWebServiceResult.RESULTCODE))
                    Return ActionResult.ExceptionError
                End If
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return ActionResult.Success
    End Function

    ''' <summary>
    ''' ストールに移動、リサイズ(作業前のチップ)
    ''' </summary>
    ''' <param name="stallId">移動先のストールID</param>
    ''' <param name="startTime">移動先のチップ開始時間</param>
    ''' <param name="endTime">移動先のチップ終了時間</param>
    ''' <param name="serviceWorkTime">移動先のチップの幅対応する時間</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <param name="drChipInfo">チップの情報</param>
    ''' <returns>操作結果</returns>
    ''' <remarks></remarks>
    Private Function MoveInStall(ByVal stallId As Decimal, _
                                    ByVal startTime As Date, _
                                    ByVal endTime As Date, _
                                    ByVal serviceWorkTime As Long, _
                                    ByVal objStaffContext As StaffContext, _
                                    ByVal updateDate As Date, _
                                    ByVal restFlg As String, _
                                    ByVal rowLockVersion As Long, _
                                    ByVal drChipInfo As TabletSmbCommonClassChipEntityRow) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        'ストール上に移動、リサイズ。作業前のチップの場合、Webserviceを呼ぶためXML作成
        Dim xmlclass As New SMBCommonClassBusinessLogic.XmlDocumentClass
        xmlclass = StructWebServiceXml(drChipInfo.JOB_DTL_ID.ToString(CultureInfo.InvariantCulture), _
               "", _
               stallId.ToString(CultureInfo.InvariantCulture), _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", startTime), _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", endTime), _
               serviceWorkTime.ToString(CultureInfo.InvariantCulture), _
               objStaffContext, _
               updateDate, _
               GetWebServiceRestFlg(restFlg), _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", drChipInfo.SCHE_DELI_DATETIME), _
               "", _
               "", _
               "", _
               "", _
               "", _
               drChipInfo.PICK_DELI_TYPE, _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", drChipInfo.SCHE_SVCIN_DATETIME), _
               CType(rowLockVersion, String))
        Using commbiz As New SMBCommonClassBusinessLogic
            Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow = commbiz.CallReserveWebService(xmlclass)
            If drWebServiceResult.RESULTCODE <> 0 Then
                'RowLockVersionError(最新のデータではない)の場合、ActionResult.RowLockVersionErrorを戻す
                If drWebServiceResult.RESULTCODE = WebServiceRowLockVersionError Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E RowLockVersionError. " _
                        , MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.RowLockVersionError
                Else
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E StallChipMoveResize call webservice failed. RESULTCODE={1}" _
                        , MethodBase.GetCurrentMethod.Name, drWebServiceResult.RESULTCODE))
                    Return ActionResult.ExceptionError
                End If
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return ActionResult.Success
    End Function

#Region "チップの移動、リサイズ処理--操作制約チェック"
    ''' <summary>
    ''' チップ移動に対するチップ操作制約チェックをします
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="moveStallId">移動先ストールId</param>
    ''' <param name="moveStartTime">移動先開始時間</param>
    ''' <param name="moveWorkTime">移動先チップの作業時間</param>
    ''' <param name="serviceEndDateTime">作業終了日時</param>
    ''' <param name="stallStartTime">営業開始時間</param>
    ''' <param name="stallEndTime">営業終了時間</param>
    ''' <param name="dtNow">今の時間</param>
    ''' <remarks></remarks>
    Private Function ValidateMove(ByVal stallUseId As Decimal, _
                             ByVal objStaffContext As StaffContext, _
                             ByVal moveStallId As Decimal, _
                             ByVal moveStartTime As Date, _
                             ByVal moveWorkTime As Long, _
                             ByVal serviceEndDateTime As Date, _
                             ByVal stallStartTime As Date, _
                             ByVal stallEndTime As Date, _
                             ByVal dtNow As Date) As Integer

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, serviceEndDateTime={2}, stallStartTime={3}, stallEndTime={4}, moveStartTime={5}, moveWorkTime={6}, moveStallId={7}" _
                    , MethodBase.GetCurrentMethod.Name, stallUseId, serviceEndDateTime, stallStartTime, stallEndTime, moveStartTime, moveWorkTime, moveStallId))

        '営業時間外
        If IsOutOfWorkingTime(moveStartTime, stallStartTime, stallEndTime) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.E OutOfWorkingTimeError " _
                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            Return ActionResult.OutOfWorkingTimeError
        End If

        'ストール利用．チップ重複配置チェック
        If CheckChipOverlapPosition(objStaffContext.DlrCD, objStaffContext.BrnCD, _
                                           stallUseId, moveStallId, moveStartTime, serviceEndDateTime, dtNow) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.E OverlapError " _
                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            Return ActionResult.OverlapError
        End If

        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        'ストール使用不可重複配置チェック
        If CheckStallUnavailableOverlapPosition(moveStartTime, serviceEndDateTime, moveStallId) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.E OverlapUnavailableError " _
                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            Return ActionResult.ChipOverlapUnavailableError
        End If
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return ActionResult.Success
    End Function

    ''' <summary>
    ''' 作業日時が休憩と重複しているか判定します。
    ''' </summary>
    ''' <param name="stallStartTime">営業開始時間</param>
    ''' <param name="stallEndTime">営業終了時間</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="workStartDateTime">作業開始日時</param>
    ''' <param name="workTime">作業時間</param>
    ''' <param name="bShowDateCheck">当画面だけチェックフラグ</param>
    ''' <param name="workEndDateTime">作業終了日時</param>
    ''' <returns>重複している場合<c>true</c>、それ以外は<c>false</c></returns>
    ''' <remarks></remarks>
    Public Function HasRestTimeInServiceTime(ByVal stallStartTime As Date, _
                                             ByVal stallEndTime As Date, _
                                             ByVal stallId As Decimal, _
                                             ByVal workStartDateTime As Date, _
                                             ByVal workTime As Long, _
                                             ByVal bShowDateCheck As Boolean, _
                                             Optional ByVal workEndDateTime As Date = Nothing) As Boolean
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallStartTime={1}, stallEndTime={2}, stallId={3}, workStartDateTime={4}, workTime={5}, workEndDateTime={6}" _
                    , MethodBase.GetCurrentMethod.Name, stallStartTime, stallEndTime, stallId, workStartDateTime, workTime, workEndDateTime))
        '秒を切り捨てる
        Dim workStartDateTimeNoSec As Date = GetDateTimeFloorSecond(workStartDateTime)
        Dim workEndDateTimeNoSec As Date
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        Dim serviceEndDateTimeData As New ServiceEndDateTimeData
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
        'workEndDateTimeがない場合(チップの移動が日跨ぎ)
        If (workEndDateTime = CType(Nothing, Date)) Then
            '営業時間
            'ストール非稼働マスタ．作業時間取得サービスを呼び出し、作業時間を取得する
            '日跨ぎの終了時間を取得する
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'workEndDateTimeNoSec = GetServiceEndDateTime(stallId, workStartDateTimeNoSec, workTime, stallStartTime, stallEndTime, RestTimeGetFlgNoGetRest)
            serviceEndDateTimeData = GetServiceEndDateTime(stallId, workStartDateTimeNoSec, workTime, stallStartTime, stallEndTime, RestTimeGetFlgNoGetRest)
            workEndDateTimeNoSec = serviceEndDateTimeData.ServiceEndDateTime
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
        Else
            workEndDateTimeNoSec = GetDateTimeFloorSecond(workEndDateTime)
        End If

        '当画面だけをチェックすれば、終了時間が当画面営業終了時間を超える場合、営業終了時間を設定する
        If bShowDateCheck Then
            '営業開始日時の終了日時を取得する
            Dim stallStartEndTime As Date = New Date(stallStartTime.Year, stallStartTime.Month, stallStartTime.Day, stallEndTime.Hour, stallEndTime.Minute, 0)
            If workEndDateTimeNoSec.CompareTo(stallStartEndTime) > 0 Then
                workEndDateTimeNoSec = stallStartEndTime
            End If
        End If

        '休憩時間情報を取得する
        Dim restInfo As TabletSmbCommonClassIdleTimeInfoDataTable = GetRestTimeInfo(stallId, workStartDateTimeNoSec, workEndDateTimeNoSec)
        If restInfo.Count >= 1 Then
            '「true：重複している」を返却する
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E return=True ", MethodBase.GetCurrentMethod.Name))
            Return True
        End If

        ' 2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        ''ストール使用不可情報を取得する。
        'Dim unavailableInfo As TabletSmbCommonClassIdleTimeInfoDataTable = GetStallUnavailableInfo(stallId, workStartDateTimeNoSec, workEndDateTimeNoSec)
        'If unavailableInfo.Count >= 1 Then
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E return=True ", MethodBase.GetCurrentMethod.Name))
        '    '「true：重複している」を返却する
        '    Return True
        'End If
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E return=False ", MethodBase.GetCurrentMethod.Name))
        '「false：重複していない」を返却する
        Return False
    End Function
#End Region

#End Region

#Region "NoShow処理"

    ''' <summary>
    '''   サービス入庫を「未来店客」へ更新します
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function NoShow(ByVal stallUseId As Decimal, _
                           ByVal updateDate As Date, _
                           ByVal objStaffContext As StaffContext, _
                           ByVal rowLockVersion As Long) As Integer
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, updateDate={2}" _
                        , MethodBase.GetCurrentMethod.Name, stallUseId, updateDate))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        ' エンティティを取得する
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId)
        If dtChipEntity.Count <> 1 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                            , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.GetChipEntityError
        End If

        'ステータス遷移可否をチェックする
        If Not CanNoShow(dtChipEntity(0).SVC_STATUS, dtChipEntity(0).STALL_USE_STATUS, dtChipEntity(0).TEMP_FLG) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
                    , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.CheckError
        End If

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        Dim preChipStatus As String = Me.JudgeChipStatus(stallUseId)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '作業前のチップの場合、Webserviceを呼ぶためXML作成
        Dim xmlclass As New SMBCommonClassBusinessLogic.XmlDocumentClass
        xmlclass = StructWebServiceXml(dtChipEntity(0).JOB_DTL_ID.ToString(CultureInfo.InvariantCulture), _
               "", _
               dtChipEntity(0).STALL_ID.ToString(CultureInfo.InvariantCulture), _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", dtChipEntity(0).SCHE_START_DATETIME), _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", dtChipEntity(0).SCHE_END_DATETIME), _
               dtChipEntity(0).SCHE_WORKTIME.ToString(CultureInfo.InvariantCulture), _
               objStaffContext, _
               updateDate, _
               "", _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", dtChipEntity(0).SCHE_DELI_DATETIME), _
               "", _
               "", _
               "", _
               NoShowFlgNoShow, _
               "", _
               dtChipEntity(0).PICK_DELI_TYPE, _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", dtChipEntity(0).SCHE_SVCIN_DATETIME), _
               CType(rowLockVersion, String))
        Using commbiz As New SMBCommonClassBusinessLogic
            'Webserviceを呼ぶ
            Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow = commbiz.CallReserveWebService(xmlclass)
            If drWebServiceResult.RESULTCODE <> 0 Then
                'RowLockVersionError(最新のデータではない)の場合、ActionResult.RowLockVersionErrorを戻す
                If drWebServiceResult.RESULTCODE = WebServiceRowLockVersionError Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E RowLockVersionError. " _
                    , MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.RowLockVersionError
                Else
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E NoShow callReserveWebService failed. RESULTCODE={1}" _
                    , MethodBase.GetCurrentMethod.Name, drWebServiceResult.RESULTCODE))
                    Return ActionResult.ExceptionError
                End If
            End If
        End Using

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '更新後のステータス取得
        Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)
        'ステータス送信
        Using ic3802601blc As New IC3802601BusinessLogic
            'タブレットSMBでは、NoShow処理のみ予約ステータスを使うかつ予約ステータスを更新しないため、前後予約ステータスを更新前の予約ステータスを渡す
            Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(dtChipEntity(0).SVCIN_ID, _
                                                                    dtChipEntity(0).JOB_DTL_ID, _
                                                                    stallUseId, _
                                                                    preChipStatus, _
                                                                    crntStatus, _
                                                                    0, _
                                                                    dtChipEntity(0).RESV_STATUS, _
                                                                    dtChipEntity(0).RESV_STATUS)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If dmsSendResult <> 0 Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '処理結果チェック
            If dmsSendResult = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END
    End Function


#End Region

#Region "中断処理"

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ' ''' <summary>
    ' ''' All Stopボタンを押すイベント
    ' ''' </summary>
    ' ''' <param name="stallUseId">ストール利用ID</param>
    ' ''' <param name="rsltEndDateTime">実績終了日時</param>
    ' ''' <param name="stallWaitTime">中断時間</param>
    ' ''' <param name="stopMemo">中断メモ</param>
    ' ''' <param name="stopReasonType">中断原因</param>
    ' ''' <param name="restFlg">休憩を取るフラグ</param>
    ' ''' <param name="updateDate">更新日時</param>
    ' ''' <param name="rowLockVersion">行ロックバージョン</param>
    ' ''' <param name="systemId">呼ぶ画面ID</param>
    ' ''' <returns>実行結果</returns>
    ' ''' <remarks></remarks>
    ' ''' '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    'Public Function JobStop(ByVal stallUseId As Decimal, _
    '                        ByVal rsltEndDateTime As Date, _
    '                        ByVal stallWaitTime As Long, _
    '                        ByVal stopMemo As String, _
    '                        ByVal stopReasonType As String, _
    '                        ByVal restFlg As String, _
    '                        ByVal updateDate As Date, _
    '                        ByVal rowLockVersion As Long, _
    '                        ByVal systemId As String) As Long
    '    'Public Function JobStop(ByVal stallUseId As Decimal, _
    '    '                       ByVal rsltEndDateTime As Date, _
    '    '                       ByVal stallWaitTime As Long, _
    '    '                       ByVal stopMemo As String, _
    '    '                       ByVal stopReasonType As String, _
    '    '                       ByVal restFlg As String, _
    '    '                       ByVal stallStartTime As Date, _
    '    '                       ByVal stallEndTime As Date, _
    '    '                       ByVal updateDate As Date, _
    '    '                       ByVal objStaffContext As StaffContext, _
    '    '                       ByVal rowLockVersion As Long, _
    '    '                       ByVal systemId As String) As Integer
    '    'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, rsltEndDateTime={2}, restFlg={3}, stallStartTime={4}, stallEndTime={5}, updateDate={6}, rowLockVersion={7}, systemId={8}, stallWaitTime={9}, stopReasonType={10}" _
    '    '   , MethodBase.GetCurrentMethod.Name, stallUseId, rsltEndDateTime, restFlg, stallStartTime, stallEndTime, updateDate, rowLockVersion, systemId, stallWaitTime, stopReasonType))

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, rsltEndDateTime={2}, restFlg={3},  updateDate={4}, systemId={5}, stallWaitTime={6}, stopReasonType={7}, inRowLockVersion={8}" _
    '               , MethodBase.GetCurrentMethod.Name, stallUseId, rsltEndDateTime, restFlg, updateDate, systemId, stallWaitTime, stopReasonType, rowLockVersion))


    '    '***********************************************************************
    '    ' 1. いろいろな値を準備する
    '    '***********************************************************************

    '    Dim objStaffContext As StaffContext = StaffContext.Current

    '    '営業開始終了日時を取得する
    '    Dim dtBranchOperatingHours As TabletSmbCommonClassBranchOperatingHoursDataTable = _
    '        Me.GetBranchOperatingHours(objStaffContext.DlrCD, objStaffContext.BrnCD)

    '    If dtBranchOperatingHours.Count = 0 Then
    '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E ExceptionError:GetBranchOperatingHours" _
    '                                  , MethodBase.GetCurrentMethod.Name))
    '        Return ActionResult.ExceptionError
    '    End If
    '    '営業開始終了日時を設定する
    '    Dim stallStartTime As Date = New Date(rsltEndDateTime.Year, rsltEndDateTime.Month, rsltEndDateTime.Day, _
    '                                          dtBranchOperatingHours(0).SVC_JOB_START_TIME.Hour, dtBranchOperatingHours(0).SVC_JOB_START_TIME.Minute, 0)
    '    Dim stallEndTime As Date = New Date(rsltEndDateTime.Year, rsltEndDateTime.Month, rsltEndDateTime.Day, _
    '                                          dtBranchOperatingHours(0).SVC_JOB_END_TIME.Hour, dtBranchOperatingHours(0).SVC_JOB_END_TIME.Minute, 0)

    '    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '    '秒を切り捨てる
    '    rsltEndDateTime = GetDateTimeFloorSecond(rsltEndDateTime)

    '    ' エンティティを取得する
    '    Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId)
    '    If dtChipEntity.Count <> 1 Then
    '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
    '                            , MethodBase.GetCurrentMethod.Name))
    '        Return ActionResult.GetChipEntityError
    '    End If

    '    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    '    ''restFlg設定してない場合、1に設定する
    '    'If IsNothing(restFlg) Then
    '    '    restFlg = RestTimeGetFlgGetRest
    '    'End If

    '    ''中断で移動不可エリア生成すれば
    '    'If stallWaitTime > 0 Then
    '    '    '移動不可チップ生成範囲の重複チェック
    '    '    Dim hasRestTimeInServiceTime As Boolean = Me.HasRestTimeInServiceTime(stallStartTime, stallEndTime, dtChipEntity(0).STALL_ID, rsltEndDateTime, stallWaitTime, False)
    '    '    '休憩または使用不可エリアと重複場合、
    '    '    If hasRestTimeInServiceTime Then
    '    '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E OverlapError", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    '        Return ActionResult.OverlapUnavailableError
    '    '    End If

    '    '    'ストール利用チップとの重複配置チェック
    '    '    Dim idleEndDateTime As Date = rsltEndDateTime.AddMinutes(stallWaitTime)

    '    '    Using ta As New TabletSMBCommonClassDataAdapter
    '    '        '重複してるチップの数を取得する
    '    '        Dim overlapChipNums As Long = ta.GetChipOverlapChipNums(objStaffContext.DlrCD, objStaffContext.BrnCD, stallUseId, dtChipEntity(0).STALL_ID, _
    '    '                                                                rsltEndDateTime, idleEndDateTime, DefaultDateTimeValueGet())
    '    '        If overlapChipNums > 0 Then
    '    '            '重複のは自分以外の場合
    '    '            If Not (overlapChipNums = 1 And dtChipEntity(0).PRMS_END_DATETIME.CompareTo(updateDate) > 0) Then
    '    '                Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.E CheckChipOverlapPosition error. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
    '    '                Return ActionResult.OverlapUnavailableError
    '    '            End If
    '    '        End If
    '    '    End Using
    '    'End If

    '    ''ステータス遷移可否をチェックする
    '    'If Not CanStop(dtChipEntity(0).STALL_USE_STATUS) Then
    '    '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
    '    '            , MethodBase.GetCurrentMethod.Name))
    '    '    Return ActionResult.CheckError
    '    'End If
    '    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
    '    ''検査ステータスが1(検査依頼中)の場合、中断できない
    '    'Dim inspectionStatus As String = dtChipEntity(0).INSPECTION_STATUS
    '    'If inspectionStatus.Equals(InspectionApproval) Then
    '    '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E InspectionStatusStopError" _
    '    '                    , MethodBase.GetCurrentMethod.Name))
    '    '    Return ActionResult.InspectionStatusStopError
    '    'End If 
    '    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

    '    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    '    '作業実績送信使用するフラグを取得する
    '    Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()

    '    '作業実績送信の場合、作業ステータスを取得する
    '    Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing
    '    If isUseJobDispatch Then
    '        prevJobStatus = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)
    '    End If

    '    '***********************************************************************
    '    ' 2. いろいろなチェックをする
    '    '***********************************************************************
    '    Dim rsltCheck As Long = Me.CheckJobStopAction(dtChipEntity(0), _
    '                                                  rsltEndDateTime, _
    '                                                  stallStartTime, _
    '                                                  stallEndTime, _
    '                                                  stallWaitTime, _
    '                                                  rowLockVersion, _
    '                                                  restFlg, _
    '                                                  objStaffContext, _
    '                                                  updateDate, _
    '                                                  systemId)
    '    If rsltCheck <> ActionResult.Success Then
    '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckJobStopAction error: Error num is {1}" _
    '                                    , MethodBase.GetCurrentMethod.Name _
    '                                    , rsltCheck))
    '        Return rsltCheck
    '    End If

    '    'restFlg設定してない場合、1に設定する
    '    If IsNothing(restFlg) Then
    '        restFlg = RestTimeGetFlgGetRest
    '    End If

    '    '更新前のステータス取得
    '    Dim prevStatus As String = Me.JudgeChipStatus(stallUseId)

    '    '***********************************************************************
    '    ' 3. DB更新
    '    '***********************************************************************
    '    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    '    'update用データセット
    '    Using targetDtChipEntity As New TabletSmbCommonClassChipEntityDataTable
    '        Dim targetDrChipEntity As TabletSmbCommonClassChipEntityRow = CType(targetDtChipEntity.NewRow, TabletSmbCommonClassChipEntityRow)
    '        'チップの作業を完了する
    '        targetDrChipEntity = ChipFinish(dtChipEntity, rsltEndDateTime, StalluseStatusStop, restFlg, updateDate, _
    '                                      stallStartTime, stallEndTime, objStaffContext.Account, updateDate, systemId)

    '        'ストール利用を更新する
    '        targetDrChipEntity = SetStopStallUse(targetDrChipEntity, stallWaitTime, stopMemo, stopReasonType, dtChipEntity(0).STALL_ID, rsltEndDateTime, updateDate, _
    '                                            objStaffContext, stallUseId, stallStartTime, stallEndTime, systemId)
    '        '新規した非稼働チップID
    '        NewStallIdleId = targetDrChipEntity.STALL_IDLE_ID

    '        Using ta As New TabletSMBCommonClassDataAdapter

    '            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発  START
    '            '作業実績テーブルを更新する
    '            Dim updateCount As Long = ta.UpdateJobRsltOnFinish(dtChipEntity(0).JOB_DTL_ID, _
    '                                                               rsltEndDateTime, _
    '                                                               JobStatusStop, _
    '                                                               objStaffContext.Account, _
    '                                                               systemId, _
    '                                                               updateDate, _
    '                                                               stopReasonType, _
    '                                                               stopMemo)
    '            If updateCount = 0 Then
    '                Logger.Error(String.Format(CultureInfo.InvariantCulture, _
    '                                           "{0}.E ExceptionError:UpdateJobRsltOnFinish update count=0.", _
    '                                           MethodBase.GetCurrentMethod.Name))
    '                Return ActionResult.ExceptionError
    '            End If

    '            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発  END


    '            '関連チップが存在する場合
    '            If ta.IsExistRelationChip(dtChipEntity(0).SVCIN_ID) Then
    '                'サービス入庫を「次の作業開始待ち」に更新する
    '                targetDrChipEntity.SVC_STATUS = SvcStatusNextStartWait
    '            Else
    '                'サービス入庫を「作業開始待ち」に更新する
    '                targetDrChipEntity.SVC_STATUS = SvcStatusStartwait
    '            End If

    '            '中断のdb更新
    '            Dim rtCnt As Long = UpdateChipStop(stallUseId, dtChipEntity(0).SVCIN_ID, targetDrChipEntity, systemId)
    '            If rtCnt <> 1 Then
    '                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Failed to update " _
    '                    , MethodBase.GetCurrentMethod.Name))
    '                Return ActionResult.ExceptionError
    '            End If
    '        End Using
    '    End Using

    '    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    '    '***********************************************************************
    '    ' 4. 基幹連携
    '    '***********************************************************************
    '    '更新後のステータス取得
    '    Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)

    '    '基幹側にステータス情報を送信
    '    Using ic3802601blc As New IC3802601BusinessLogic
    '        Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(dtChipEntity(0).SVCIN_ID, _
    '                                                                dtChipEntity(0).JOB_DTL_ID, _
    '                                                                stallUseId, _
    '                                                                prevStatus, _
    '                                                                crntStatus, _
    '                                                                0)
    '        If dmsSendResult <> 0 Then
    '            Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
    '                                       , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
    '            Return ActionResult.DmsLinkageError
    '        End If
    '    End Using

    '    '実績送信使用の場合
    '    If isUseJobDispatch Then

    '        '作業ステータスを取得する
    '        Dim crntJobStatus As IC3802701JobStatusDataTable = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)

    '        '基幹側にJobDispatch実績情報を送信
    '        Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(dtChipEntity(0).SVCIN_ID, _
    '                                                               dtChipEntity(0).JOB_DTL_ID, _
    '                                                               prevJobStatus, _
    '                                                               crntJobStatus)
    '        If resultSendJobClock <> ActionResult.Success Then
    '            Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.End. DmsLinkageError:SendJobClockOnInfo FAILURE " _
    '                                        , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
    '            Return ActionResult.DmsLinkageError
    '        End If

    '    End If
    '    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E ", MethodBase.GetCurrentMethod.Name))
    '    Return ActionResult.Success
    'End Function

    ''' <summary>
    ''' All Stopボタンを押すイベント
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inRsltEndDateTime">実績終了日時</param>
    ''' <param name="inStallWaitTime">中断時間</param>
    ''' <param name="inStopMemo">中断メモ</param>
    ''' <param name="inStopReasonType">中断原因</param>
    ''' <param name="inRestFlg">休憩を取るフラグ</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inSystemId">呼ぶ画面ID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function JobStop(ByVal inStallUseId As Decimal, _
                            ByVal inRsltEndDateTime As Date, _
                            ByVal inStallWaitTime As Long, _
                            ByVal inStopMemo As String, _
                            ByVal inStopReasonType As String, _
                            ByVal inRestFlg As String, _
                            ByVal inUpdateDate As Date, _
                            ByVal inRowLockVersion As Long, _
                            ByVal inSystemId As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inStallUseId={1}, inRsltEndDateTime={2}, inRestFlg={3},  inUpdateDate={4}, inSystemId={5}, inStallWaitTime={6}, inStopReasonType={7}, inRowLockVersion={8}, inStopMemo={9}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inStallUseId, _
                                  inRsltEndDateTime, _
                                  inRestFlg, _
                                  inUpdateDate, _
                                  inSystemId, _
                                  inStallWaitTime, _
                                  inStopReasonType, _
                                  inRowLockVersion, _
                                  inStopMemo))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Try
            'Push送信フラグをFalseで初期化
            NeedPushAfterStopSingleJob = False

            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
            ' サブエリアリフレッシュグラグをFalseで初期化
            NeedPushSubAreaRefresh = False
            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

            'チップエンティティを取得する
            Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(inStallUseId, 1)

            'ストール利用IDで取得した件数が1件以外の場合、チップエンティティエラーを戻す
            If 1 <> dtChipEntity.Count Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E GetChipEntityError" _
                                , MethodBase.GetCurrentMethod.Name))
                Return ActionResult.GetChipEntityError

            End If

            'ログインスタッフ情報取得
            Dim staffInfo As StaffContext = StaffContext.Current

            '実績終了日時を取得
            Dim rsltEndDateTimeNoSec As Date = Me.CheckRsltEndDateTime(dtChipEntity(0).RSLT_START_DATETIME, _
                                                                       inRsltEndDateTime, _
                                                                       staffInfo.DlrCD, _
                                                                       staffInfo.BrnCD)

            '指定Job中断後、次のチップのステータス(作業中、中断)を取得する
            Dim drAfterStopChipStatus As TabletSmbCommonClassChipStatusRow = _
                Me.GetChipStatusAfterStopJob(dtChipEntity(0).JOB_DTL_ID)

            If AfterFinishChipStatusStop.Equals(drAfterStopChipStatus.CHIP_STATUS) Then
                'チップが中断中になる場合

                '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
                ''チップを中断にする
                'Dim retChangeToStopChip As Long = Me.ChangeToStopChipByStop(inStallUseId, _
                '                                                            rsltEndDateTimeNoSec, _
                '                                                            inStallWaitTime, _
                '                                                            inStopMemo, _
                '                                                            inStopReasonType, _
                '                                                            inRestFlg, _
                '                                                            inUpdateDate, _
                '                                                            inRowLockVersion, _
                '                                                            inSystemId, _
                '                                                            dtChipEntity)

                '作業実績送信使用するフラグを取得する
                Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()

                '作業実績送信の場合、作業ステータスを取得する
                Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing
                If isUseJobDispatch Then
                    prevJobStatus = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)
                End If

                'チップを中断にする
                Dim retChangeToStopChip As Long = Me.ChangeToStopChipByStop(inStallUseId, _
                                                                            rsltEndDateTimeNoSec, _
                                                                            inStallWaitTime, _
                                                                            inStopMemo, _
                                                                            inStopReasonType, _
                                                                            inRestFlg, _
                                                                            inUpdateDate, _
                                                                            inRowLockVersion, _
                                                                            inSystemId, _
                                                                            dtChipEntity, _
                                                                            prevJobStatus)
                '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない END

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'If ActionResult.Success = retChangeToStopChip Then
                '    '成功の場合、

                '    'Push送信フラグを立っる
                '    NeedPushAfterStopSingleJob = True

                'Else
                '    '失敗の場合

                '    'エラーコードを戻す
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                '                              "{0}.End. Return Errorcode={1}.", _
                '                              MethodBase.GetCurrentMethod.Name, _
                '                              retChangeToStopChip))
                '    Return retChangeToStopChip

                'End If

                '処理結果チェック
                If retChangeToStopChip = ActionResult.Success Then
                    '「0：成功」の場合
                    'Push送信フラグをTrueにする
                    NeedPushAfterStopSingleJob = True
                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                    ' サブエリアリフレッシュグラグををTrueにする
                    NeedPushSubAreaRefresh = True
                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
                ElseIf retChangeToStopChip = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                    'Push送信フラグをTrueにする
                    NeedPushAfterStopSingleJob = True
                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                    ' サブエリアリフレッシュグラグををTrueにする
                    NeedPushSubAreaRefresh = True
                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[ChangeToStopChipByStop FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            ElseIf AfterFinishChipStatusWorking.Equals(drAfterStopChipStatus.CHIP_STATUS) Then
                'チップがまだ作業中の場合

                '次のチップステータスが変わらないから、ただ選択したJob中断

                '選択したJobを中断する
                Dim retSingJobStop As Long = _
                    Me.ChangeToWorkingChipByStop(rsltEndDateTimeNoSec, _
                                                 inStopMemo, _
                                                 inStopReasonType, _
                                                 inUpdateDate, _
                                                 inRowLockVersion, _
                                                 inSystemId, _
                                                 dtChipEntity(0))

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''エラーがあれば
                'If ActionResult.Success <> retSingJobStop Then

                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                '                               "{0}.End. StopSingleJobAction failed. ErrorCode={1}", _
                '                               MethodBase.GetCurrentMethod.Name, _
                '                               retSingJobStop))
                '    'エラーコードを戻す
                '    Return retSingJobStop

                'End If

                '処理結果チェック
                If retSingJobStop = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf retSingJobStop = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[ChangeToWorkingChipByStop FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                      "{0}.End. Return Success.", _
                                      MethodBase.GetCurrentMethod.Name))

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'Return ActionResult.Success

            Return returnCode

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Catch ex As OracleExceptionEx When ex.Number = 1013

            'DBタイムアウトの場合、DBタイムアウトエラーコードを戻す
            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.End Error:DBTimeOutError.", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.DBTimeOutError

        End Try

    End Function

    '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
    ' ''' <summary>
    ' ''' 中断操作でチップが「作業中断」になる
    ' ''' </summary>
    ' ''' <param name="stallUseId">ストール利用ID</param>
    ' ''' <param name="rsltEndDateTime">実績終了日時</param>
    ' ''' <param name="stallWaitTime">中断時間</param>
    ' ''' <param name="stopMemo">中断メモ</param>
    ' ''' <param name="stopReasonType">中断原因</param>
    ' ''' <param name="restFlg">休憩を取るフラグ</param>
    ' ''' <param name="updateDate">更新日時</param>
    ' ''' <param name="rowLockVersion">行ロックバージョン</param>
    ' ''' <param name="systemId">呼ぶ画面ID</param>
    ' ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ' ''' <remarks>
    ' ''' チップが中断になる
    ' ''' </remarks>
    ' ''' <history>
    ' ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ' ''' </history>
    'Private Function ChangeToStopChipByStop(ByVal stallUseId As Decimal, _
    '                                        ByVal rsltEndDateTime As Date, _
    '                                        ByVal stallWaitTime As Long, _
    '                                        ByVal stopMemo As String, _
    '                                        ByVal stopReasonType As String, _
    '                                        ByVal restFlg As String, _
    '                                        ByVal updateDate As Date, _
    '                                        ByVal rowLockVersion As Long, _
    '                                        ByVal systemId As String, _
    '                                        ByVal dtChipEntity As TabletSmbCommonClassChipEntityDataTable) As Long

    ''' <summary>
    ''' 中断操作でチップが「作業中断」になる
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rsltEndDateTime">実績終了日時</param>
    ''' <param name="stallWaitTime">中断時間</param>
    ''' <param name="stopMemo">中断メモ</param>
    ''' <param name="stopReasonType">中断原因</param>
    ''' <param name="restFlg">休憩を取るフラグ</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <param name="systemId">呼ぶ画面ID</param>
    ''' <param name="prevJobStatus">更新前作業連携ステータス</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks>
    ''' チップが中断になる
    ''' </remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Private Function ChangeToStopChipByStop(ByVal stallUseId As Decimal, _
                                            ByVal rsltEndDateTime As Date, _
                                            ByVal stallWaitTime As Long, _
                                            ByVal stopMemo As String, _
                                            ByVal stopReasonType As String, _
                                            ByVal restFlg As String, _
                                            ByVal updateDate As Date, _
                                            ByVal rowLockVersion As Long, _
                                            ByVal systemId As String, _
                                            ByVal dtChipEntity As TabletSmbCommonClassChipEntityDataTable, _
                                            ByVal prevJobStatus As IC3802701JobStatusDataTable) As Long

        '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. stallUseId={1}, rsltEndDateTime={2}, restFlg={3},  updateDate={4}, systemId={5}, stallWaitTime={6}, stopReasonType={7}, inRowLockVersion={8}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  stallUseId, _
                                  rsltEndDateTime, _
                                  restFlg, _
                                  updateDate, _
                                  systemId, _
                                  stallWaitTime, _
                                  stopReasonType, _
                                  rowLockVersion))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '***********************************************************************
        ' 1. いろいろな値を準備する
        '***********************************************************************

        Dim objStaffContext As StaffContext = StaffContext.Current

        '営業開始と終了時間を取得する
        Dim dtBranchOperatingHours As TabletSmbCommonClassBranchOperatingHoursDataTable = _
            Me.GetOneDayBrnOperatingHours(rsltEndDateTime, _
                                          objStaffContext.DlrCD, _
                                          objStaffContext.BrnCD)

        'Nothingの場合、予期せぬエラーを出す
        If IsNothing(dtBranchOperatingHours) Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.End. ExceptionError:GetOneDayBrnOperatingHours", _
                                       MethodBase.GetCurrentMethod.Name))
            Return ActionResult.ExceptionError

        End If

        '当日の営業開始日時
        Dim stallStartTime As Date = dtBranchOperatingHours(0).SVC_JOB_START_TIME

        '当日の営業終了日時
        Dim stallEndTime As Date = dtBranchOperatingHours(0).SVC_JOB_END_TIME


        '秒を切り捨てる
        rsltEndDateTime = GetDateTimeFloorSecond(rsltEndDateTime)

        '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
        ''作業実績送信使用するフラグを取得する
        'Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()

        ''作業実績送信の場合、作業ステータスを取得する
        'Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing
        'If isUseJobDispatch Then
        '    prevJobStatus = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)
        'End If
        '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない END

        '***********************************************************************
        ' 2. いろいろなチェックをする
        '***********************************************************************
        Dim rsltCheck As Long = Me.CheckChangeToStopChipByStop(dtChipEntity(0), _
                                                               rsltEndDateTime, _
                                                               stallStartTime, _
                                                               stallEndTime, _
                                                               stallWaitTime, _
                                                               rowLockVersion, _
                                                               restFlg, _
                                                               objStaffContext, _
                                                               updateDate, _
                                                               systemId)

        'チェックエラーの場合、エラーコードを戻す
        If rsltCheck <> ActionResult.Success Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckJobStopAction error: Error num is {1}" _
                                        , MethodBase.GetCurrentMethod.Name _
                                        , rsltCheck))
            Return rsltCheck

        End If

        'restFlg設定してない場合、1(休憩取る)に設定する
        If IsNothing(restFlg) Then

            restFlg = RestTimeGetFlgGetRest

        End If

        '更新前のステータス取得
        Dim prevStatus As String = Me.JudgeChipStatus(stallUseId)

        '***********************************************************************
        ' 3. DB更新
        '***********************************************************************
        'update用データセット
        Using targetDtChipEntity As New TabletSmbCommonClassChipEntityDataTable

            Dim targetDrChipEntity As TabletSmbCommonClassChipEntityRow = _
                CType(targetDtChipEntity.NewRow, TabletSmbCommonClassChipEntityRow)

            'チップの作業を完了する
            targetDrChipEntity = ChipFinish(dtChipEntity, _
                                            rsltEndDateTime, _
                                            StalluseStatusStop, _
                                            restFlg, _
                                            updateDate, _
                                            stallStartTime, _
                                            stallEndTime, _
                                            objStaffContext.Account, _
                                            updateDate, _
                                            systemId)

            'ストール利用を更新する
            targetDrChipEntity = SetStopStallUse(targetDrChipEntity, _
                                                 stallWaitTime, _
                                                 stopMemo, _
                                                 stopReasonType, _
                                                 dtChipEntity(0).STALL_ID, _
                                                 rsltEndDateTime, _
                                                 updateDate, _
                                                 objStaffContext, _
                                                 stallUseId, _
                                                 stallStartTime, _
                                                 stallEndTime, _
                                                 systemId)
            '新規した非稼働チップID
            NewStallIdleId = targetDrChipEntity.STALL_IDLE_ID

            Using ta As New TabletSMBCommonClassDataAdapter

                '作業実績テーブルを更新する
                ta.UpdateJobRsltOnFinish(dtChipEntity(0).JOB_DTL_ID, _
                                         rsltEndDateTime, _
                                         JobStatusStop, _
                                         objStaffContext.Account, _
                                         systemId, _
                                         updateDate, _
                                         stopReasonType, _
                                         stopMemo)


                '次のサービスステータスを設定する
                targetDrChipEntity.SVC_STATUS = Me.GetNextSvcStatusByStop(dtChipEntity(0).SVCIN_ID)

                '中断のdb更新
                Dim rtCnt As Long = UpdateChipStop(stallUseId, dtChipEntity(0).SVCIN_ID, targetDrChipEntity, systemId)
                If rtCnt <> 1 Then

                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.E Failed to update ", _
                                               MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.ExceptionError

                End If

            End Using

        End Using

        '***********************************************************************
        ' 4. 基幹連携
        '***********************************************************************
        '更新後のステータス取得
        Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)

        '基幹側にステータス情報を送信
        Using ic3802601blc As New IC3802601BusinessLogic

            Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(dtChipEntity(0).SVCIN_ID, _
                                                                    dtChipEntity(0).JOB_DTL_ID, _
                                                                    stallUseId, _
                                                                    prevStatus, _
                                                                    crntStatus, _
                                                                    0)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If dmsSendResult <> 0 Then

            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, _
            '                               "{0}.{1} SendStatusInfo FAILURE ", _
            '                               Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError

            'End If

            '処理結果チェック
            If dmsSendResult = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using

        '実績送信使用の場合
        '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
        'If isUseJobDispatch Then
        If IsUseJobDispatch() Then
            '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない END

            '作業ステータスを取得する
            Dim crntJobStatus As IC3802701JobStatusDataTable = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)

            '基幹側にJobDispatch実績情報を送信
            Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(dtChipEntity(0).SVCIN_ID, _
                                                                   dtChipEntity(0).JOB_DTL_ID, _
                                                                   prevJobStatus, _
                                                                   crntJobStatus)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If resultSendJobClock <> ActionResult.Success Then

            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.End. DmsLinkageError:SendJobClockOnInfo FAILURE " _
            '                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError

            'End If

            '処理結果チェック
            If resultSendJobClock = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf resultSendJobClock = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendJobClockOnInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E ", MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    ''' <summary>
    ''' 中断操作で「作業計画の一部の作業が中断」になるまたは「作業中」のまま
    ''' </summary>
    ''' <param name="inRsltEndDateTime">実績終了日時</param>
    ''' <param name="inStopMemo">中断メモ</param>
    ''' <param name="inStopResonType">作業指示シーケンス</param>
    ''' <param name="inJobInstructId">作業指示ID</param>
    ''' <param name="inJobInstructSeq">作業指示シーケンス</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inSystemId">画面ID</param>
    ''' <param name="inDataRowChipEntity">チップエンティティ</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Private Function ChangeToWorkingChipByStop(ByVal inRsltEndDateTime As Date, _
                                               ByVal inStopMemo As String, _
                                               ByVal inStopResonType As String, _
                                               ByVal inUpdateDate As Date, _
                                               ByVal inRowLockVersion As Long, _
                                               ByVal inSystemId As String, _
                                               ByVal inDataRowChipEntity As TabletSmbCommonClassChipEntityRow, _
                                               Optional ByVal inJobInstructId As String = "", _
                                               Optional ByVal inJobInstructSeq As Long = 0) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inRsltEndDateTime={1}, inJobInstructId={2}, inJobInstructSeq={3}, inUpdateDate={4}, inRowLockVersion={5}, inSystemId={6}, inStopMemo={7}, inStopResonType={8}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inRsltEndDateTime, _
                                  inJobInstructId, _
                                  inJobInstructSeq, _
                                  inUpdateDate, _
                                  inRowLockVersion, _
                                  inSystemId, _
                                  inStopMemo, _
                                  inStopResonType))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '***********************************************************************
        ' 1. ローカル変数初期化
        '***********************************************************************

        Dim svcInId As Decimal = inDataRowChipEntity.SVCIN_ID
        Dim stallUseId As Decimal = inDataRowChipEntity.STALL_USE_ID
        Dim jobDtlId As Decimal = inDataRowChipEntity.JOB_DTL_ID
        Dim stallUseStatus As String = inDataRowChipEntity.STALL_USE_STATUS

        '秒を切り捨てる
        inRsltEndDateTime = GetDateTimeFloorSecond(inRsltEndDateTime)

        'スタッフ情報
        Dim staffInfo As StaffContext = StaffContext.Current

        '該当作業の作業ステータスを取得する
        Dim dtJobStatus As TabletSmbCommonClassJobStatusDataTable = Nothing
        '更新前のJOBステータスを取得する
        Dim preJobStatus As String = String.Empty

        If Not String.IsNullOrEmpty(inJobInstructId) Then
            '引数.作業指示IDが空白ではない(Single Job Stop)

            'Jobステータス取得
            dtJobStatus = Me.GetJobStatusDataTable(jobDtlId, _
                                                   inJobInstructId, _
                                                   inJobInstructSeq)

            'データ正しく取得できなかったら、エラーコードを戻す
            If 1 <> dtJobStatus.Count Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.End 1 <> dtJobStatus.Count", _
                                           MethodBase.GetCurrentMethod.Name))
                Return ActionResult.ExceptionError

            End If

            '更新前のJOBステータス
            preJobStatus = dtJobStatus(0).JOB_STATUS

        End If

        '***********************************************************************
        ' 2. チェック処理
        '***********************************************************************
        Dim rsltCheck As Long = Me.CheckChangeToWorkingChipByStop(inDataRowChipEntity, _
                                                                  inRowLockVersion, _
                                                                  staffInfo, _
                                                                  inUpdateDate, _
                                                                  inSystemId, _
                                                                  preJobStatus)

        'チェックでエラーがあれば、エラーコードを戻す
        If ActionResult.Success <> rsltCheck Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.End CheckSingleStopAction error: Error code={1}", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       rsltCheck))
            Return rsltCheck

        End If

        'JobDispatch送信使用フラグを取得する
        Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()

        'DB更新前の作業ステータスを取得する
        Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing

        'JobDispatch実績送信の場合、作業ステータスを取得する
        If isUseJobDispatch Then

            If Not String.IsNullOrEmpty(inJobInstructId) Then
                '引数.作業指示IDが空白ではない(Single Job Stop)
                prevJobStatus = Me.JudgeSingleJobStatus(jobDtlId, _
                                                        inJobInstructId, _
                                                        inJobInstructSeq, _
                                                        preJobStatus)

            Else
                '引数.作業指示IDが空白の場合(All Job Stop)

                '該当チップ全てのJobステータスを取得する
                prevJobStatus = JudgeJobStatus(jobDtlId)

            End If

        End If

        '更新前のチップのステータス取得
        Dim prevStatus As String = Me.JudgeChipStatus(stallUseId)

        '***********************************************************************
        ' 3. DB更新
        '***********************************************************************

        'ストール利用ステータスに04一部作業中断を更新する
        Dim retUpdateStallUseStatus As Long = _
            UpdateStallUseStatusToStartIncludeStopJobByStop(jobDtlId, _
                                                            stallUseStatus, _
                                                            inUpdateDate, _
                                                            staffInfo.Account, _
                                                            inSystemId, _
                                                            inDataRowChipEntity, _
                                                            inJobInstructId)

        '更新エラーの場合、エラーコードそのままで戻す
        If ActionResult.Success <> retUpdateStallUseStatus Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}.{1} UpdateStallUseStatusToStartIncludeStopJobByStop Failed. ", _
                                       Me.GetType.ToString, _
                                       MethodBase.GetCurrentMethod.Name))
            Return retUpdateStallUseStatus

        End If

        '作業実績テーブル更新
        Using ta As New TabletSMBCommonClassDataAdapter

            If Not String.IsNullOrEmpty(inJobInstructId) Then
                '単独なJob中断する時

                '作業ステータスを[2:中断]に設定する
                dtJobStatus(0).JOB_STATUS = JobStatusStop
                '中断メモ
                If String.IsNullOrWhiteSpace(inStopMemo) Then
                    '空白の場合、1つspaceを設定する(DBに空白できない)
                    dtJobStatus(0).STOP_MEMO = Space(1)

                Else
                    '値があれば、値に設定する
                    dtJobStatus(0).STOP_MEMO = inStopMemo

                End If

                '中断理由区分
                dtJobStatus(0).STOP_REASON_TYPE = inStopResonType
                '作業内容ID
                dtJobStatus(0).JOB_DTL_ID = jobDtlId
                '作業指示ID
                dtJobStatus(0).JOB_INSTRUCT_ID = inJobInstructId
                '作業指示連番
                dtJobStatus(0).JOB_INSTRUCT_SEQ = inJobInstructSeq

                '作業実績テーブルを更新する
                Dim updateCount As Long = ta.UpdateSingleJobResultByJobStopFinish(dtJobStatus(0), _
                                                                                  inRsltEndDateTime, _
                                                                                  staffInfo.Account, _
                                                                                  inUpdateDate, _
                                                                                  inSystemId)
                '更新行数が0の場合、予期せぬエラーを戻す
                If 0 = updateCount Then

                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                "{0}.E ExceptionError:UpdateJobResultByJobStop update count=0.", _
                                                MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.ExceptionError

                End If
            Else
                'All Job Stop時

                '作業実績テーブルを更新する
                ta.UpdateJobRsltOnFinish(jobDtlId, _
                                         inRsltEndDateTime, _
                                         JobStatusStop, _
                                         staffInfo.Account, _
                                         inSystemId, _
                                         inUpdateDate, _
                                         inStopResonType, _
                                         inStopMemo)
            End If


        End Using

        '***********************************************************************
        ' 4. 基幹連携
        '***********************************************************************

        '更新後のチップのステータス取得
        Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)

        '基幹側にステータス情報を送信
        Using ic3802601blc As New IC3802601BusinessLogic

            Dim resultSendStatusInfo As Long = ic3802601blc.SendStatusInfo(svcInId, _
                                                                           jobDtlId, _
                                                                           stallUseId, _
                                                                           prevStatus, _
                                                                           crntStatus, _
                                                                           0)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            ''送信失敗の場合、DMS連携エラーコードを戻す
            'If ActionResult.Success <> resultSendStatusInfo Then

            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, _
            '                               "{0}.{1} SendStatusInfo Failed. ", _
            '                               Me.GetType.ToString, _
            '                               MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError

            'End If

            '処理結果チェック
            If resultSendStatusInfo = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf resultSendStatusInfo = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If
            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using


        '実績送信使用の場合
        If isUseJobDispatch Then

            '作業ステータスを取得する
            Dim crntJobStatus As IC3802701JobStatusDataTable = Nothing

            If Not String.IsNullOrEmpty(inJobInstructId) Then
                '引数.作業指示IDが空白ではない(Single Job Stop)

                'JobDispatch用のJobのステータスに変更
                crntJobStatus = Me.JudgeSingleJobStatus(jobDtlId, _
                                                        inJobInstructId, _
                                                        inJobInstructSeq, _
                                                        JobStatusStop)

            Else
                '引数.作業指示IDが空白の場合(All Job Stop)

                '該当チップ全てのJobステータスを取得する
                crntJobStatus = JudgeJobStatus(jobDtlId)

            End If

            '基幹側にJobDispatch実績情報を送信
            Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(svcInId, _
                                                                   jobDtlId, _
                                                                   prevJobStatus, _
                                                                   crntJobStatus)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            ''送信失敗の場合、DMS連携エラーコードを戻す
            'If ActionResult.Success <> resultSendJobClock Then

            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, _
            '                               "{0}.{1}.End DmsLinkageError:SendJobClockOnInfo Failure. ", _
            '                               Me.GetType.ToString, _
            '                               MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError

            'End If

            '処理結果チェック
            If resultSendJobClock = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf resultSendJobClock = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendJobClockOnInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End If

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End", MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    ''' <summary>
    ''' 中断操作でストール利用ステータス更新
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseStatus">ストール利用ステータス</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inUpdateUser">更新アカウント</param>
    ''' <param name="inSystemId">呼び出し元</param>
    ''' <param name="inDataRowChipEntity">チップエンティティ</param>
    ''' <param name="inJobInstructId">作業指示ID(単独なJob中断用)</param>
    ''' <returns>実行結果</returns>
    ''' <remarks></remarks>
    Private Function UpdateStallUseStatusToStartIncludeStopJobByStop(ByVal inJobDtlId As Decimal, _
                                                                     ByVal inStallUseStatus As String, _
                                                                     ByVal inUpdateDate As Date, _
                                                                     ByVal inUpdateUser As String, _
                                                                     ByVal inSystemId As String, _
                                                                     ByVal inDataRowChipEntity As TabletSmbCommonClassChipEntityRow, _
                                                                     Optional ByVal inJobInstructId As String = "") As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inJobDtlId={1}, inStallUseStatus={2}, inUpdateDate={3}, inUpdateUser={4}, inSystemId={5}, inJobInstructId={6}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inJobDtlId, _
                                  inStallUseStatus, _
                                  inUpdateDate, _
                                  inUpdateUser, _
                                  inSystemId, _
                                  inJobInstructId))

        Using ta As New TabletSMBCommonClassDataAdapter

            'ストール利用テーブル更新
            'ストール利用ステータスが02作業中の場合、04作業指示の一部の作業が中断に更新する
            If StalluseStatusStart.Equals(inStallUseStatus) Then

                '04に作業指示の一部の作業に変更フラグ(False：変更しない)
                Dim StartIncludeStopJob As Boolean = False

                If Not String.IsNullOrEmpty(inJobInstructId) Then
                    '単独なJob中断する時

                    '他のJobに中断Jobがあれば、04に作業指示の一部の作業が中断なる
                    If Not Me.HasStopJob(inJobDtlId) Then

                        '04に作業指示の一部の作業に変更フラグ(True：変更する)
                        StartIncludeStopJob = True

                    End If

                Else
                    'All Stop時

                    If Me.HasBeforeStartJob(inJobDtlId) Then
                        '未開始Jobがある場合

                        '04に作業指示の一部の作業に変更フラグ(True：変更する)
                        StartIncludeStopJob = True

                    End If

                End If

                '04に作業指示の一部の作業に変更フラグがTrue：変更する場合、ストール利用テーブル更新を実行する
                If StartIncludeStopJob Then

                    '更新用DataRow作成
                    Using targetdtChipEntity As New TabletSmbCommonClassChipEntityDataTable

                        targetdtChipEntity.ImportRow(inDataRowChipEntity)
                        Dim targetdrChipEntity As TabletSmbCommonClassChipEntityRow = targetdtChipEntity(0)

                        'ストール利用テーブル更新用データを準備する
                        '作業ステータスを[4:作業指示の一部の作業が中断]に更新
                        targetdrChipEntity.STALL_USE_STATUS = StalluseStatusStartIncludeStopJob
                        targetdrChipEntity.UPDATE_DATETIME = inUpdateDate
                        targetdrChipEntity.UPDATE_STF_CD = inUpdateUser

                        'ストール利用テーブルを更新する
                        Dim updateCnt As Long = ta.UpdateStallUseRsltStartDate(targetdrChipEntity, inSystemId)

                        '更新行数が1行以外の場合、予期せぬエラーを戻す
                        If 1 <> updateCnt Then

                            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                       "{0}.End UpdateStallUseRsltStartDate Error.", _
                                                       MethodBase.GetCurrentMethod.Name))
                            Return ActionResult.ExceptionError

                        End If

                    End Using

                End If

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End.", _
                                  MethodBase.GetCurrentMethod.Name))
        Return ActionResult.Success

    End Function
    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    ''' <summary>
    ''' 中断時のストール利用を更新します
    ''' </summary>
    ''' <param name="targetChipEntity">チップエンティティ</param>
    ''' <param name="stallWaitTime">中断時間</param>
    ''' <param name="stopReasonType">中断原因</param>
    ''' <param name="stopMemo">中断メモ</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="rsltEndDateTimeNoSec">実績終了日時</param>
    ''' <param name="updateDate">現在日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="stallStartTime">営業開始時間</param>
    ''' <param name="stallEndTime">営業終了時間</param>
    ''' <param name="systemId">呼ぶ画面ID</param>
    ''' <returns>チップエンティティ(非稼働エリアのIDを追加)</returns>
    ''' <remarks></remarks>
    Private Function SetStopStallUse(ByVal targetChipEntity As TabletSmbCommonClassChipEntityRow, _
                                ByVal stallWaitTime As Long, _
                                ByVal stopMemo As String, _
                                ByVal stopReasonType As String, _
                                ByVal stallId As Decimal, _
                                ByVal rsltEndDateTimeNoSec As Date, _
                                ByVal updateDate As Date, _
                                ByVal objStaffContext As StaffContext, _
                                ByVal stallUseId As Decimal, _
                                ByVal stallStartTime As Date, _
                                ByVal stallEndTime As Date, _
                                ByVal systemId As String) As TabletSmbCommonClassChipEntityRow
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, rsltEndDateTimeNoSec={2}, updateDate={3}, stallUseId={4}" _
                    , MethodBase.GetCurrentMethod.Name, stallId, rsltEndDateTimeNoSec, updateDate, stallUseId))
        Dim stallIdleId As Decimal = 0
        'ストール待機時間があれば、
        If stallWaitTime > 0 Then

            '使用不可チップ開始時間が５分倍数ではない場合、５分倍数に切り上げる
            Dim idleStartDatetime As Date = rsltEndDateTimeNoSec
            Dim interval As Long
            interval = idleStartDatetime.Minute Mod 5
            If interval > 0 Then
                Dim addMinutes = 5 - interval
                idleStartDatetime = idleStartDatetime.AddMinutes(addMinutes)
            End If
            '使用不可チップの終了日時を算出
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'Dim idleEndDatetime As Date = GetServiceEndDateTime(stallId, idleStartDatetime, _
            '                                                    stallWaitTime, _
            '                                                    stallStartTime, _
            '                                                    stallEndTime, _
            '                                                    RestTimeGetFlgNoGetRest)
            Dim serviceEndDateTimeData As ServiceEndDateTimeData = GetServiceEndDateTime(stallId, idleStartDatetime, _
                                                                stallWaitTime, _
                                                                stallStartTime, _
                                                                stallEndTime, _
                                                                RestTimeGetFlgNoGetRest)
            Dim idleEndDatetime As Date = serviceEndDateTimeData.ServiceEndDateTime
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

            'ストール非稼働マスタ．ストール使用不可作成を呼び出す
            '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
            'stallIdleId = CreateStallUnavailable(stallId, idleStartDatetime, idleEndDatetime, stopMemo, _
            '                                    updateDate, objStaffContext, stallUseId, systemId)

            '中断時は使用不可チップのメモの値が入らないように修正
            CreateStallUnavailable(stallId, idleStartDatetime, idleEndDatetime, String.Empty, _
                                                updateDate, objStaffContext, stallUseId, systemId, stallIdleId)
            '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END
            targetChipEntity.STALL_IDLE_ID = stallIdleId
        Else
            targetChipEntity.STALL_IDLE_ID = 0
        End If

        targetChipEntity.STOP_REASON_TYPE = stopReasonType
        If Not String.IsNullOrEmpty(stopMemo) Then
            targetChipEntity.STOP_MEMO = stopMemo
        Else
            targetChipEntity.STOP_MEMO = " "
        End If

        targetChipEntity.UPDATE_DATETIME = updateDate
        targetChipEntity.UPDATE_STF_CD = objStaffContext.Account

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        Return targetChipEntity
    End Function

    ''' <summary>
    ''' ストール使用不可を作成します
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="idleStartDatetime">非稼働開始日時</param>
    ''' <param name="idleEndDatetime">非稼働終了日時</param>
    ''' <param name="idleMemo">非稼働メモ</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="systemId">プログラムID</param>
    ''' <param name="stallIdleId">ストール非稼働ID</param>
    ''' <returns>結果コード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
    ''' </history>
    Public Function CreateStallUnavailable(ByVal stallId As Decimal, _
                                            ByVal idleStartDatetime As Date, _
                                            ByVal idleEndDatetime As Date, _
                                            ByVal idleMemo As String, _
                                            ByVal updateDate As Date, _
                                            ByVal objStaffContext As StaffContext, _
                                            ByVal stallUseId As Decimal, _
                                            ByVal systemId As String, _
                                            ByRef stallIdleId As Decimal) As Integer
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1} START IN:stallId={2}, idleStartDatetime={3}, idleEndDatetime={4}, idleMemo={5}, updateDate={6}, stallUseId={7}, systemId={8} ", _
                                  Me.GetType.ToString, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  stallId.ToString(CultureInfo.CurrentCulture), _
                                  idleStartDatetime.ToString(CultureInfo.CurrentCulture), _
                                  idleEndDatetime.ToString(CultureInfo.CurrentCulture), _
                                  idleMemo, _
                                  updateDate.ToString(CultureInfo.CurrentCulture), _
                                  stallUseId, _
                                  systemId))

        'ストール利用チップとの重複配置チェックを行い、
        '戻り値が「true：チップ重複配置あり」の場合
        If Me.CheckChipOverlapPosition(objStaffContext.DlrCD, _
                                        objStaffContext.BrnCD, _
                                        stallUseId, _
                                        stallId, _
                                        idleStartDatetime, _
                                        idleEndDatetime, _
                                        updateDate) Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0} OverlapError ", MethodBase.GetCurrentMethod.Name))

            stallIdleId = -1
            Return ActionResult.OverlapError
        End If

        '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
        'Dim stallIdleId As Decimal = InsertStallUnavailable(stallId, idleStartDatetime, idleEndDatetime, String.Empty, updateDate, objStaffContext.Account, systemId)

        'idlememoの値
        '中断時は空文字列、使用不可チップ作成時はストール使用不可画面(SC3240701)のメモの値を設定
        stallIdleId = InsertStallUnavailable(stallId, idleStartDatetime, idleEndDatetime, idleMemo, updateDate, objStaffContext.Account, systemId)
        '2017/09/05 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E stallIdleId={1}", MethodBase.GetCurrentMethod.Name, stallIdleId))
        'ストール非稼働IDを戻す
        Return ActionResult.Success
    End Function

    ''' <summary>
    ''' ストール使用不可を追加
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="idleStartDatetime">非稼働開始日時</param>
    ''' <param name="idleEndDatetime">非稼働終了日時</param>
    ''' <param name="idleMemo">非稼働メモ</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="stfCode">スタッフコード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InsertStallUnavailable(ByVal stallId As Decimal, ByVal idleStartDatetime As Date, ByVal idleEndDatetime As Date, _
                                           ByVal idleMemo As String, ByVal updateDate As Date, ByVal stfCode As String, ByVal systemId As String) As Decimal
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, idleStartDatetime={2}, idleEndDatetime={3}, idleMemo={4}, updateDate={5}, stfCode={6}, systemId={7}" _
                    , MethodBase.GetCurrentMethod.Name, stallId, idleStartDatetime, idleEndDatetime, idleMemo, updateDate, stfCode, systemId))
        Dim stallIdleId As Decimal = 0
        Using ta As New TabletSMBCommonClassDataAdapter
            'シーケンスで非稼働テーブルの次のID取得
            stallIdleId = ta.GetSequenceNextVal(StallIdleIdSeq)
            'ストール非稼働の追加
            ta.InsertStallUnavailable(stallIdleId, stallId, idleStartDatetime, idleEndDatetime, idleMemo, updateDate, stfCode, systemId)
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E stallIdleId={1}", MethodBase.GetCurrentMethod.Name, stallIdleId))

        Return stallIdleId
    End Function

    ''' <summary>
    ''' 中断のDB更新
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="targetDrChipEntity"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateChipStop(ByVal stallUseId As Decimal, ByVal svcinId As Decimal, ByVal targetdrChipEntity As TabletSmbCommonClassChipEntityRow, ByVal systemId As String) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, svcinId={2}" _
                    , MethodBase.GetCurrentMethod.Name, stallUseId, svcinId))
        Dim updateCnt As Long = 0
        Using ta As New TabletSMBCommonClassDataAdapter
            'サービス入庫テーブルを更新する
            updateCnt = ta.UpdateSvcinStatus(svcinId, targetdrChipEntity.SVC_STATUS, targetdrChipEntity.UPDATE_DATETIME, targetdrChipEntity.UPDATE_STF_CD)
            If updateCnt = 1 Then
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                ''ストール利用テーブルを更新する
                'updateCnt = ta.UpdateStallUseChipStop(stallUseId, targetdrChipEntity, systemId)

                Using dealerEnvBiz As New ServiceCommonClassBusinessLogic
                    '休憩取得自動判定フラグ
                    Dim autoJudgeFlg = String.Empty
                    autoJudgeFlg = dealerEnvBiz.GetDlrSystemSettingValueBySettingName(RestAutoJudgeFlg)

                'ストール利用テーブルを更新する
                    updateCnt = ta.UpdateStallUseChipStop(stallUseId, targetdrChipEntity, systemId, autoJudgeFlg)
                End Using
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
                
            End If
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E updateCnt={1}", MethodBase.GetCurrentMethod.Name, updateCnt))
        Return updateCnt
    End Function

    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 START

    ''' <summary>
    ''' 単独Job中断の主な処理
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inRsltEndDateTime">実績終了日時</param>
    ''' <param name="inStallWaitTime">休憩取得フラグ</param>
    ''' <param name="inStopMemo">中断メモ</param>
    ''' <param name="inStopResonType">中断理由区分</param>
    ''' <param name="inRestFlg">休憩取得フラグ</param>
    ''' <param name="inJobInstructId">作業指示ID</param>
    ''' <param name="inJobInstructSeq">作業指示枝番</param>
    ''' <param name="inUpdateDate">更新時間</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inSystemId">呼ぶ画面ID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function StopSingleJob(ByVal inStallUseId As Decimal, _
                                  ByVal inRsltEndDateTime As Date, _
                                  ByVal inStallWaitTime As Long, _
                                  ByVal inStopMemo As String, _
                                  ByVal inStopResonType As String, _
                                  ByVal inRestFlg As String, _
                                  ByVal inJobInstructId As String, _
                                  ByVal inJobInstructSeq As Long, _
                                  ByVal inUpdateDate As Date, _
                                  ByVal inRowLockVersion As Long, _
                                  ByVal inSystemId As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inStallUseId={1}, inRsltEndDateTime={2}, inRestFlg={3},　inStallWaitTime={4}, inStopResonType={5}, inStopMemo={6} inJobInstructId={7}, inJobInstructSeq={8}, inUpdateDate={9}, inRowLockVersion={10}, inSystemId={11}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inStallUseId, _
                                  inRsltEndDateTime, _
                                  inRestFlg, _
                                  inStallWaitTime, _
                                  inStopResonType, _
                                  inStopMemo, _
                                  inJobInstructId, _
                                  inJobInstructSeq, _
                                  inUpdateDate, _
                                  inRowLockVersion, _
                                  inSystemId))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        'Push送信フラグを初期化(False：送信しない)
        NeedPushAfterStopSingleJob = False

        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
        ' サブエリアリフレッシュグラグをFalseで初期化
        NeedPushSubAreaRefresh = False
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

        Try

            'チップエンティティを取得する
            Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(inStallUseId, 1)

            'ストール利用IDで取得した件数が1件以外の場合、チップエンティティエラーを戻す
            If 1 <> dtChipEntity.Count Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E GetChipEntityError" _
                                , MethodBase.GetCurrentMethod.Name))
                Return ActionResult.GetChipEntityError

            End If

            'ログインスタッフ情報取得
            Dim staffInfo As StaffContext = StaffContext.Current

            '実績終了日時を取得
            Dim rsltEndDateTimeNoSec As Date = Me.CheckRsltEndDateTime(dtChipEntity(0).RSLT_START_DATETIME, _
                                                                       inRsltEndDateTime, _
                                                                       staffInfo.DlrCD, _
                                                                       staffInfo.BrnCD)

            '指定Job中断後、次のチップのステータス(作業中、中断)を取得する
            Dim drAfterStopChipStatus As TabletSmbCommonClassChipStatusRow = _
                Me.GetChipStatusAfterStopJob(dtChipEntity(0).JOB_DTL_ID, _
                                                     inJobInstructId, _
                                                     inJobInstructSeq)

            If AfterFinishChipStatusStop.Equals(drAfterStopChipStatus.CHIP_STATUS) Then
                '次のチップステータスが中断の場合

                '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
                ''作業中断関数を呼んで、チップを中断する
                'Dim retChangeToStopChip As Long = Me.ChangeToStopChipByStop(inStallUseId, _
                '                                                            rsltEndDateTimeNoSec, _
                '                                                            inStallWaitTime, _
                '                                                            inStopMemo, _
                '                                                            inStopResonType, _
                '                                                            inRestFlg, _
                '                                                            inUpdateDate, _
                '                                                            inRowLockVersion, _
                '                                                            inSystemId, _
                '                                                            dtChipEntity)

                '作業実績送信使用するフラグを取得する
                Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()

                '作業実績送信の場合、作業ステータスを取得する
                Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing
                If isUseJobDispatch Then
                    prevJobStatus = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)
                End If

                '作業中断関数を呼んで、チップを中断する
                Dim retChangeToStopChip As Long = Me.ChangeToStopChipByStop(inStallUseId, _
                                                                            rsltEndDateTimeNoSec, _
                                                                            inStallWaitTime, _
                                                                            inStopMemo, _
                                                                            inStopResonType, _
                                                                            inRestFlg, _
                                                                            inUpdateDate, _
                                                                            inRowLockVersion, _
                                                                            inSystemId, _
                                                                            dtChipEntity, _
                                                                            prevJobStatus)
                '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない END

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'If ActionResult.Success = retChangeToStopChip Then
                '    '成功の場合、

                '    'Push送信フラグを立っる
                '    NeedPushAfterStopSingleJob = True

                'Else
                '    '失敗の場合

                '    'エラーコードを戻す
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                '                              "{0}.End. Return Errorcode={1}.", _
                '                              MethodBase.GetCurrentMethod.Name, _
                '                              retChangeToStopChip))
                '    Return retChangeToStopChip

                'End If

                '処理結果チェック
                If retChangeToStopChip = ActionResult.Success Then
                    '「0：成功」の場合
                    'Push送信フラグをTrueにする
                    NeedPushAfterStopSingleJob = True
                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                    ' サブエリアリフレッシュグラグををTrueにする
                    NeedPushSubAreaRefresh = True
                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
                ElseIf retChangeToStopChip = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                    'Push送信フラグをTrueにする
                    NeedPushAfterStopSingleJob = True
                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
                    ' サブエリアリフレッシュグラグををTrueにする
                    NeedPushSubAreaRefresh = True
                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[ChangeToStopChipByStop FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            Else

                '次のチップステータスが変わらないから、ただ選択したJob中断

                '選択したJobを中断する
                Dim retSingJobStop As Long = _
                    Me.ChangeToWorkingChipByStop(rsltEndDateTimeNoSec, _
                                                 inStopMemo, _
                                                 inStopResonType, _
                                                 inUpdateDate, _
                                                 inRowLockVersion, _
                                                 inSystemId, _
                                                 dtChipEntity(0), _
                                                 inJobInstructId, _
                                                 inJobInstructSeq)

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''エラーがあれば
                'If ActionResult.Success <> retSingJobStop Then

                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                '                               "{0}.End. StopSingleJobAction failed. ErrorCode={1}", _
                '                               MethodBase.GetCurrentMethod.Name, _
                '                               retSingJobStop))
                '    'エラーコードを戻す
                '    Return retSingJobStop

                'End If

                '処理結果チェック
                If retSingJobStop = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf retSingJobStop = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[ChangeToWorkingChipByStop FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If

            ' 正常終了
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.End. Success", _
                                      MethodBase.GetCurrentMethod.Name))

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'Return ActionResult.Success

            Return returnCode

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Catch ex As OracleExceptionEx When ex.Number = 1013

            'DBタイムアウトの場合、DBタイムアウトエラーコードを戻す
            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.End Error:DBTimeOutError.", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.DBTimeOutError

        End Try

    End Function

    ''' <summary>
    ''' 指定作業中断後、チップのステータスを取得する
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inJobInstructId">作業指示ID</param>
    ''' <param name="inJobInstructSeq">作業指示連番</param>
    ''' <returns>チップのステータスデータ行</returns>
    ''' <remarks></remarks>
    Private Function GetChipStatusAfterStopJob(ByVal inJobDtlId As Decimal, _
                                               Optional ByVal inJobInstructId As String = "", _
                                               Optional ByVal inJobInstructSeq As Long = 0) As TabletSmbCommonClassChipStatusRow

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inJobDtlId={1}, inJobInstructId={2}, inInstructSeq={3}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inJobDtlId, _
                                  inJobInstructId, _
                                  inJobInstructSeq))

        Dim dtJobInstruct As TabletSmbCommonClassJobInstructDataTable = Nothing
        Dim dtJobInstructResult As TabletSmbCommonClassJobStatusDataTable = Nothing

        Using ta As New TabletSMBCommonClassDataAdapter

            '該当作業に紐づく全て作業を取得する(作業指示テーブルから)
            dtJobInstruct = ta.GetJobInstructIdAndSeqByJobDtlId(inJobDtlId)

            '該当作業に紐づく全て作業実績を取得する(作業実績テーブルから)
            dtJobInstructResult = ta.GetAllJobRsltInfoByJobDtlId(inJobDtlId)

        End Using

        '返却用テーブル
        Using dtChipStatus As New TabletSmbCommonClassChipStatusDataTable

            Dim drChipStatus As TabletSmbCommonClassChipStatusRow = _
                dtChipStatus.NewTabletSmbCommonClassChipStatusRow

            '実績件数が全作業件数と違う場合、未開始作業がいる
            If dtJobInstruct.Count <> dtJobInstructResult.Count Then

                '未開始Jobがいるから、作業終了後、チップがまだ作業中
                drChipStatus.CHIP_STATUS = AfterFinishChipStatusWorking

            Else
                '未開始作業がいない

                '作業中Jobがあるフラグ(False:ない)
                Dim bWorkingJobFlg As Boolean = False

                If Not String.IsNullOrEmpty(inJobInstructId) Then
                    '単独なJob中断の場合

                    '作業実績データテーブルでループして、自分以外のJob中、中断、作業中Jobがあるかどうか
                    For Each drInstructResult As TabletSmbCommonClassJobStatusRow In dtJobInstructResult

                        'Jobが自分の場合、Continue
                        If (inJobInstructId.Equals(drInstructResult.JOB_INSTRUCT_ID) _
                                        And inJobInstructSeq = drInstructResult.JOB_INSTRUCT_SEQ) Then

                            Continue For

                        End If

                        '自分以外Jobの中に作業中Jobがある場合
                        If JobStatusWorking.Equals(drInstructResult.JOB_STATUS) Then

                            '作業中JobがあるフラグにTrueを設定する(作業中Jobあり)
                            bWorkingJobFlg = True

                            Exit For
                        End If

                    Next

                End If

                '作業中Jobがある場合
                If bWorkingJobFlg Then

                    '作業中チップがいるから、作業中断後、チップがまだ作業中
                    drChipStatus.CHIP_STATUS = AfterFinishChipStatusWorking

                Else

                    '作業中チップがないのばあい、チップが中断中にする
                    drChipStatus.CHIP_STATUS = AfterFinishChipStatusStop

                End If

            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E Return={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      drChipStatus.CHIP_STATUS))
            '返却
            Return drChipStatus

        End Using

    End Function
    '2014/07/15 TMEJ 丁 タブレットSMB Job Dispatch機能開発 END

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 中断操作で次のサービスステータスを取得する
    ''' </summary>
    ''' <param name="inSvcinId">サービス入庫ID</param>
    ''' <returns>サービスステータス</returns>
    ''' <remarks></remarks>
    Private Function GetNextSvcStatusByStop(ByVal inSvcinId As Decimal) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inSvcinId={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inSvcinId))

        '返却用サービス入庫ステータス
        Dim retSvcinStatus As String = String.Empty

        'TabletSMBCommonClassのデータアクセスクラスインスタンス生成
        Using ta As New TabletSMBCommonClassDataAdapter

            If ta.IsExistRelationChip(inSvcinId) Then
                '関連チップが存在する場合

                'サービス入庫を「次の作業開始待ち」に更新する
                retSvcinStatus = SvcStatusNextStartWait

            Else
                '関連チップがない場合

                'サービス入庫を「作業開始待ち」に更新する
                retSvcinStatus = SvcStatusStartwait

            End If

        End Using

        '返却
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1}_End. Return={2}", _
                                  Me.GetType.ToString, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  retSvcinStatus))
        Return retSvcinStatus

    End Function
    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END	
#End Region

    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START

#Region "検査合格処理"

    ''' <summary>
    ''' 検査合格時の処理を行う
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inUpdateDateTime">更新日時</param>
    ''' <param name="inUpdateProgramId">更新機能ID</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks>
    ''' 完成検査承認時に、以下を更新する処理を行なう
    ''' 　●サービスステータス
    ''' 　●ROステータス
    ''' </remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function PassedInspection(ByVal inStallUseId As Decimal, _
                                     ByVal inUpdateDateTime As Date, _
                                     ByVal inUpdateProgramId As String, _
                                     ByVal inRowLockVersion As Long) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.{1} START IN:inStallUseId={2}, inUpdateDateTime={3}, inUpdateProgramId={4}, inRowLockVersion={5}", _
                                  Me.GetType.ToString, _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inStallUseId.ToString(CultureInfo.CurrentCulture), _
                                  inUpdateDateTime.ToString(CultureInfo.CurrentCulture), _
                                  inUpdateProgramId, _
                                  inRowLockVersion.ToString(CultureInfo.CurrentCulture)))

        'ログイン情報の取得
        Dim objStaffContext As StaffContext = StaffContext.Current

        'チップエンティティ(チップ情報)を取得する
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(inStallUseId)

        '取得結果が1件以外(通常あり得ない)の場合はエラー
        If dtChipEntity.Count <> 1 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.{1}_Error GetChipEntityError", _
                                       Me.GetType.ToString, _
                                       MethodBase.GetCurrentMethod.Name))
            'チップエンティティ取得エラー
            Return ActionResult.GetChipEntityError
        End If

        Dim serviceInId As Decimal = dtChipEntity(0).SVCIN_ID                   'チップのサービス入庫ID
        Dim prevStallUseStatus As String = dtChipEntity(0).STALL_USE_STATUS     '更新前のストール利用ステータス
        Dim updateAccount As String = objStaffContext.Account                   '更新アカウント

        '更新処理開始前チェック
        '更新前のストール利用ステータスが03(作業完了)でない場合、エラー
        If Not StalluseStatusFinish.Equals(prevStallUseStatus) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.{1}_Error BeforeUpdateCheckError, prevStallUseStatus:{2}", _
                                       Me.GetType.ToString, _
                                       MethodBase.GetCurrentMethod.Name, _
                                       prevStallUseStatus))
            'システムエラー
            Return ActionResult.ExceptionError

        End If

        'サービス入庫テーブルロック処理
        Dim resultLockServiceIn As Long = Me.LockServiceInTable(serviceInId, _
                                                                inRowLockVersion, _
                                                                updateAccount, _
                                                                inUpdateDateTime, _
                                                                inUpdateProgramId)

        'サービス入庫テーブルロック処理結果コードが0以外の場合、エラー
        If resultLockServiceIn <> 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.{1}_Error LockServiceInTableError, ErrorCode:{2}", _
                                       Me.GetType.ToString, _
                                       MethodBase.GetCurrentMethod.Name, _
                                       resultLockServiceIn))
            Return resultLockServiceIn

        End If

        'TabletSMBCommonClassのデータアクセスクラスインスタンス生成
        Using ta As New TabletSMBCommonClassDataAdapter

            '更新用サービスステータス
            Dim updateServiceStatus As String

            '洗車有りの場合
            If InspectionNeedFlgNeed.Equals(dtChipEntity(0).CARWASH_NEED_FLG) Then

                '洗車実績テーブルのレコードを削除する
                ta.DeleteCarWashResult(dtChipEntity(0).SVCIN_ID)

                '洗車待ち：07
                updateServiceStatus = SvcStatusCarWashWait

            Else

                If DeliTypeWaiting.Equals(dtChipEntity(0).PICK_DELI_TYPE) Then
                    '引取納車区分が0の場合

                    '納車待ち(Waiting)：12
                    updateServiceStatus = SvcStatusWaitingCustomer

                Else
                    '引取納車区分が0以外の場合

                    '預かり中(DropOff)：11
                    updateServiceStatus = SvcStatusDropOffCustomer

                End If

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            ''サービス入庫テーブルの更新
            'Dim resultUpdateServiceIn As Long = ta.UpdateSvcinStatus(serviceInId, _
            '                                                         updateServiceStatus, _
            '                                                         inUpdateDateTime, _
            '                                                         updateAccount)
            ''更新行数が1行でない場合は異常
            'If resultUpdateServiceIn <> 1 Then
            '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
            '                               "{0}.{1}_Error UpdateSvcinStatusError, UpdateRowCount:{2}", _
            '                               Me.GetType.ToString, _
            '                               MethodBase.GetCurrentMethod.Name, _
            '                               resultUpdateServiceIn))
            '    'システムエラー
            '    Return ActionResult.ExceptionError

            'End If

            ''RO情報テーブルの更新
            ''※更新対象は該当するRO番号、かつROステータスが60(作業中)のレコードのみとする
            ''　そのため、更新行数が0行の可能性も見越し、更新行数チェックはなしとする

            ''2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
            ''ta.UpdateROInfoByPassedInspection(dtChipEntity(0).RO_NUM, _
            ''                                  RostatusWaitForDelivery, _
            ''                                  updateAccount, _
            ''                                  inUpdateDateTime, _
            ''                                  inUpdateProgramId)

            'ta.UpdateROInfoByPassedInspection(dtChipEntity(0).RO_NUM, _
            '                                  serviceInId, _
            '                                  RostatusWaitForDelivery, _
            '                                  updateAccount, _
            '                                  inUpdateDateTime, _
            '                                  inUpdateProgramId)
            ''2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

            Using bizServiceCommonClass As New ServiceCommonClassBusinessLogic
                'サービスDMS納車実績ワークを取得
                Dim dtWorkServiceDmsResultDelivery As ServiceCommonClassDataSet.WorkServiceDmsResultDeliveryDataTable = _
                    bizServiceCommonClass.GetWorkServiceDmsResultDelivery(dtChipEntity(0).SVCIN_ID)

                '取得結果と更新後サービスステータスのチェック
                If Not (IsNothing(dtWorkServiceDmsResultDelivery)) AndAlso 0 < dtWorkServiceDmsResultDelivery.Count AndAlso _
                       (SvcStatusDropOffCustomer.Equals(updateServiceStatus) OrElse _
                        SvcStatusWaitingCustomer.Equals(updateServiceStatus)) Then
                    '取得できた場合且つ、更新後サービスステータスが「11：預かり中 or 12：納車待ち」の場合
                    '強制納車処理を実施
                    Dim returnCodeForceDeliverd As Integer = Me.ForceDeliverd(objStaffContext.DlrCD,
                                                                              objStaffContext.BrnCD, _
                                                                              dtChipEntity(0).SVCIN_ID, _
                                                                              dtChipEntity(0).RO_NUM, _
                                                                              dtWorkServiceDmsResultDelivery(0).DMS_RSLT_DELI_DATETIME, _
                                                                              objStaffContext.Account, _
                                                                              inUpdateDateTime, _
                                                                              inUpdateProgramId)

                    '処理結果チェック
                    If returnCodeForceDeliverd <> ActionResult.Success Then
                        '「0：成功」以外の場合
                        '「22：予期せぬエラー」を返却
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[ForceDeliverd FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ActionResult.ExceptionError))
                        Return ActionResult.ExceptionError

                    End If

                Else

                    'サービス入庫テーブルの更新
                    Dim resultUpdateServiceIn As Long = ta.UpdateSvcinStatus(serviceInId, _
                                                                             updateServiceStatus, _
                                                                             inUpdateDateTime, _
                                                                             updateAccount)
                    '更新行数が1行でない場合は異常
                    If resultUpdateServiceIn <> 1 Then
                        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                   "{0}.{1}_Error UpdateSvcinStatusError, UpdateRowCount:{2}", _
                                                   Me.GetType.ToString, _
                                                   MethodBase.GetCurrentMethod.Name, _
                                                   resultUpdateServiceIn))
                        'システムエラー
                        Return ActionResult.ExceptionError

                    End If

                    'RO情報テーブルの更新
                    '※更新対象は該当するRO番号、かつROステータスが60(作業中)のレコードのみとする
                    '　そのため、更新行数が0行の可能性も見越し、更新行数チェックはなしとする
                    ta.UpdateROInfoByPassedInspection(dtChipEntity(0).RO_NUM, _
                                                      serviceInId, _
                                                      RostatusWaitForDelivery, _
                                                      updateAccount, _
                                                      inUpdateDateTime, _
                                                      inUpdateProgramId)

                End If

            End Using

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1}_E", _
                                  Me.GetType.ToString, _
                                  MethodBase.GetCurrentMethod.Name))

        '成功
        Return ActionResult.Success

    End Function

#End Region

#Region "検査不合格処理"

    ''' <summary>
    ''' 検査不合格時の処理を行う
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inUpdateDateTime">更新日時</param>
    ''' <param name="inUpdateProgramId">更新機能ID</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks>完成検査がある場合、完成検査否認時に呼ばれる想定</remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function FailedInspection(ByVal inStallUseId As Decimal, _
                                     ByVal inUpdateDateTime As Date, _
                                     ByVal inUpdateProgramId As String, _
                                     ByVal inRowLockVersion As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inStallUseId={1}, inUpdateDateTime={2}, inUpdateProgramId={3}, inRowLockVersion={4}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inStallUseId, _
                                  inUpdateDateTime, _
                                  inUpdateProgramId, _
                                  inRowLockVersion))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        'ログイン情報の取得
        Dim objStaffContext As StaffContext = StaffContext.Current

        'チップエンティティ(チップ情報)を取得する
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(inStallUseId)

        '取得結果が1件以外(通常あり得ない)の場合はエラー
        If dtChipEntity.Count <> 1 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error GetChipEntityError", _
                                       MethodBase.GetCurrentMethod.Name))
            'チップエンティティ取得エラー
            Return ActionResult.GetChipEntityError
        End If

        Dim serviceInId As Decimal = dtChipEntity(0).SVCIN_ID                   'チップのサービス入庫ID
        Dim jobDetailId As Decimal = dtChipEntity(0).JOB_DTL_ID                 'チップの作業内容ID
        Dim prevStallUseStatus As String = dtChipEntity(0).STALL_USE_STATUS     '更新前のストール利用ステータス
        Dim updateAccount As String = objStaffContext.Account                   '更新アカウント

        '更新処理開始前チェック
        '更新前のストール利用ステータスが03(作業完了)でない場合、エラー
        If Not StalluseStatusFinish.Equals(prevStallUseStatus) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error BeforeUpdateCheckError, prevStallUseStatus:{1}", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       prevStallUseStatus))
            'システムエラー
            Return ActionResult.ExceptionError
        End If

        '作業実績送信使用するフラグを取得する
        Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()

        '作業実績送信の場合、作業ステータスを取得する
        Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing
        If isUseJobDispatch Then
            prevJobStatus = JudgeJobStatus(jobDetailId)
        End If

        '更新前のチップステータス(基幹連携用)取得
        Dim prevChipStatus As String = Me.JudgeChipStatus(inStallUseId)

        'サービス入庫テーブルロック処理
        Dim resultLockServiceIn As Long = Me.LockServiceInTable(serviceInId, _
                                                                inRowLockVersion, _
                                                                updateAccount, _
                                                                inUpdateDateTime, _
                                                                inUpdateProgramId)

        'サービス入庫テーブルロック処理結果コードが0以外の場合、エラー
        If resultLockServiceIn <> 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.Error LockServiceInTableError, ErrorCode:{1}", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       resultLockServiceIn))
            Return resultLockServiceIn
        End If

        'TabletSMBCommonClassのデータアクセスクラスインスタンス生成
        Using ta As New TabletSMBCommonClassDataAdapter

            '更新用サービスステータス
            Dim updateServiceStatus As String

            If ta.IsExistRelationChip(serviceInId) Then
                '関連チップが存在する場合
                'サービス入庫ステータスを「06:次の作業開始待ち」に更新する
                updateServiceStatus = SvcStatusNextStartWait
            Else
                '関連チップが存在しない場合
                'サービス入庫ステータスを「04:作業開始待ち」に更新する
                updateServiceStatus = SvcStatusStartwait
            End If

            'サービス入庫テーブルの更新
            Dim resultUpdateServiceIn As Long = ta.UpdateSvcinStatus(serviceInId, _
                                                                     updateServiceStatus, _
                                                                     inUpdateDateTime, _
                                                                     updateAccount)
            '更新行数が1行でない場合は異常
            If resultUpdateServiceIn <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error UpdateSvcinStatusError, UpdateRowCount:{1}", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           resultUpdateServiceIn))
                'システムエラー
                Return ActionResult.ExceptionError
            End If

            'ストール利用テーブルの更新
            Dim resultUpdateStallUse As Integer = ta.UpdateStallUseByFailedInspection(inStallUseId, _
                                                                                      StalluseStatusStop, _
                                                                                      StopReasonInspectionFailure, _
                                                                                      updateAccount, _
                                                                                      inUpdateDateTime, _
                                                                                      inUpdateProgramId)
            '更新行数が1行でない場合は異常
            If resultUpdateStallUse <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error UpdateStallUseByFailedInspectionError, UpdateRowCount:{1}", _
                                           MethodBase.GetCurrentMethod.Name, _
                                           resultUpdateStallUse))
                'システムエラー
                Return ActionResult.ExceptionError
            End If

            '作業実績テーブルの更新
            Dim resultUpdateJobRslt As Long _
                = ta.UpdateJobResultByFailedInspection(dtChipEntity(0).JOB_DTL_ID, _
                                                       JobStatusStop, _
                                                       StopReasonInspectionFailure, _
                                                       updateAccount, _
                                                       inUpdateDateTime, _
                                                       inUpdateProgramId)

            '更新行数が0行の場合は異常
            If resultUpdateJobRslt = 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.Error UpdateJobResultByFailedInspection, UpdateRowCount:0", _
                                           MethodBase.GetCurrentMethod.Name))
                'システムエラー
                Return ActionResult.ExceptionError
            End If

            'RO情報テーブルの更新
            '(検査不合格となったチップに紐づくJobが所属するROのROステータスを60(作業中)に戻す)
            '※空振りの可能性有りのため、更新行数チェックはなし
            ta.UpdateROStatusByJobDtlId(serviceInId, _
                                        jobDetailId, _
                                        RostatusWorkinProgress, _
                                        inUpdateDateTime, _
                                        updateAccount, _
                                        inUpdateProgramId)

        End Using

        '更新後のチップステータス(基幹連携用)取得
        Dim crntChipStatus As String = Me.JudgeChipStatus(inStallUseId)

        '基幹側にステータス情報を送信
        Using sendStatusBiz As New IC3802601BusinessLogic

            Dim resultSendStatusInfo As Long = sendStatusBiz.SendStatusInfo(serviceInId, _
                                                                            jobDetailId, _
                                                                            inStallUseId, _
                                                                            prevChipStatus, _
                                                                            crntChipStatus, _
                                                                            0)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If resultSendStatusInfo <> 0 Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, _
            '                               "{0}.Error SendStatusInfo FAILURE, resultSendStatusInfo:{1}", _
            '                               MethodBase.GetCurrentMethod.Name, _
            '                               resultSendStatusInfo))
            '    '基幹連携エラー
            '    Return ActionResult.DmsLinkageError
            'End If

            '処理結果チェック
            If resultSendStatusInfo = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf resultSendStatusInfo = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using

        '実績送信使用の場合
        If isUseJobDispatch Then

            Dim crntJobStatus As IC3802701JobStatusDataTable = JudgeJobStatus(jobDetailId)

            '基幹側にJobDispatch実績情報を送信
            Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(serviceInId, _
                                                                   jobDetailId, _
                                                                   prevJobStatus, _
                                                                   crntJobStatus)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If resultSendJobClock <> ActionResult.Success Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.End DmsLinkageError:SendJobClockOnInfo FAILURE " _
            '                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '処理結果チェック
            If resultSendJobClock = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf resultSendJobClock = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendJobClockOnInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.E", _
                                  MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        ''成功
        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

#End Region
    '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

#Region "作業開始処理"

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

    '    ''' <summary>
    '    '''   チップ作業の開始処理を行う
    '    ''' </summary>
    '    ''' <param name="stallUseId">ストール利用ID</param>
    '    ''' <param name="svcInId">サービス入庫ID</param>
    '    ''' <param name="stallId">ストールID</param>
    '    ''' <param name="rsltStartDateTime">実績開始時間</param>
    '    ''' <param name="restFlg">休憩取得フラグ</param>
    '    ''' <param name="objStaffContext">スタッフ情報</param>
    '    ''' <param name="stallStartTime">営業開始時間</param>
    '    ''' <param name="stallEndTime">営業終了時間</param>
    '    ''' <param name="updateDate">更新時間</param>
    '    ''' <param name="systemId">更新クラス</param>
    '    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    'Public Function Start(ByVal stallUseId As Decimal, _
    '                      ByVal svcInId As Decimal, _
    '                      ByVal stallId As Decimal, _
    '                      ByVal rsltStartDateTime As Date, _
    '                      ByVal restFlg As String, _
    '                      ByVal objStaffContext As StaffContext, _
    '                      ByVal stallStartTime As Date, _
    '                      ByVal stallEndTime As Date, _
    '                      ByVal updateDate As Date, _
    '                      ByVal systemId As String) As Long
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, svcInId={2}, stallId={3}, rsltStartDateTime={4}, restFlg={5},  stallStartTime={6}, stallEndTime={7}" _
    '                    , MethodBase.GetCurrentMethod.Name, stallUseId, svcInId, stallId, rsltStartDateTime, restFlg, stallStartTime, stallEndTime))

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ' ''' <summary>
    ' '''   チップ作業の開始処理を行う
    ' ''' </summary>
    ' ''' <param name="stallUseId">ストール利用ID</param>
    ' ''' <param name="rsltStartDateTime">実績開始時間</param>
    ' ''' <param name="restFlg">休憩取得フラグ</param>
    ' ''' <param name="updateDate">更新時間</param>
    ' ''' <param name="rowLockVersion">行ロックバージョン</param>
    ' ''' <param name="systemId">更新クラス</param>
    ' ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    'Public Function Start(ByVal stallUseId As Decimal, _
    '                      ByVal rsltStartDateTime As Date, _
    '                      ByVal restFlg As String, _
    '                      ByVal updateDate As Date, _
    '                      ByVal rowLockVersion As Long, _
    '                      ByVal systemId As String) As Long

    ''' <summary>
    '''   チップ作業の開始処理を行う
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rsltStartDateTime">実績開始時間</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="updateDate">更新時間</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <param name="systemId">更新クラス</param>
    ''' <param name="restartStopJobFlg">中断中Job再開フラグ</param>
    ''' <param name="callerType">呼び出し元タイプ</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    Public Function Start(ByVal stallUseId As Decimal, _
                          ByVal rsltStartDateTime As Date, _
                          ByVal restFlg As String, _
                          ByVal updateDate As Date, _
                          ByVal rowLockVersion As Long, _
                          ByVal systemId As String, _
                          Optional ByVal restartStopJobFlg As Boolean = True, _
                          Optional ByVal callerType As Long = CallerTypeSmbAllJobAction) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. stallUseId={1}, rsltStartDateTime={2}, restFlg={3}, rowLockVersion={4}, systemId={5}, restartStopJobFlg={6}, callerType={7}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  stallUseId, _
                                  rsltStartDateTime, _
                                  restFlg, _
                                  rowLockVersion, _
                                  systemId, _
                                  restartStopJobFlg, _
                                  callerType))

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        Dim objStaffContext As StaffContext = StaffContext.Current
        ' エンティティを取得する
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId)
        If dtChipEntity.Count <> 1 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End GetChipEntityError" _
                            , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.GetChipEntityError
        End If

        Try
            Dim result As Long = Me.StartAllJobAction(stallUseId, _
                                                      rsltStartDateTime, _
                                                      restFlg, _
                                                      updateDate, _
                                                      rowLockVersion, _
                                                      systemId, _
                                                      dtChipEntity, _
                                                      restartStopJobFlg, _
                                                      callerType)
            Return result
        Catch ex As OracleExceptionEx When ex.Number = 1013
            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.End Error:DBTimeOutError.", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.DBTimeOutError
        Finally
            If isStallLocked Then
                'ストールロック解除
                Me.LockStallReset(dtChipEntity(0).STALL_ID, rsltStartDateTime, objStaffContext.Account, updateDate, systemId)
            End If
        End Try

    End Function

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ' ''' <summary>
    ' ''' チップにすべて作業の開始処理を行う
    ' ''' </summary>
    ' ''' <param name="stallUseId">ストール利用ID</param>
    ' ''' <param name="rsltStartDateTime">実績開始時間</param>
    ' ''' <param name="restFlg">休憩取得フラグ</param>
    ' ''' <param name="updateDate">更新時間</param>
    ' ''' <param name="rowLockVersion">行ロックバージョン</param>
    ' ''' <param name="systemId">更新クラス</param>
    ' ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    'Private Function StartAllJobAction(ByVal stallUseId As Decimal, _
    '                                   ByVal rsltStartDateTime As Date, _
    '                                   ByVal restFlg As String, _
    '                                   ByVal updateDate As Date, _
    '                                   ByVal rowLockVersion As Long, _
    '                                   ByVal systemId As String, _
    '                                   ByVal dtChipEntity As TabletSmbCommonClassChipEntityDataTable) As Long

    ''' <summary>
    ''' チップにすべて作業の開始処理を行う
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rsltStartDateTime">実績開始時間</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="updateDate">更新時間</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <param name="systemId">更新クラス</param>
    ''' <param name="restartStopJobFlg">中断中Job再開フラグ</param>
    ''' <param name="callerType">呼び出し元タイプ</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Private Function StartAllJobAction(ByVal stallUseId As Decimal, _
                                       ByVal rsltStartDateTime As Date, _
                                       ByVal restFlg As String, _
                                       ByVal updateDate As Date, _
                                       ByVal rowLockVersion As Long, _
                                       ByVal systemId As String, _
                                       ByVal dtChipEntity As TabletSmbCommonClassChipEntityDataTable, _
                                       ByVal restartStopJobFlg As Boolean, _
                                       ByVal callerType As Long) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. ", _
                                  MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        '***********************************************************************
        ' 1. いろいろな値を準備する
        '***********************************************************************
        isStallLocked = False
        Dim stallId As Decimal
        Dim objStaffContext As StaffContext = StaffContext.Current

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, rsltStartDateTime={2}, restFlg={3}, rowLockVersion={4}, systemId={5}" _
                        , MethodBase.GetCurrentMethod.Name, stallUseId, rsltStartDateTime, restFlg, rowLockVersion, systemId))

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ''営業開始終了日時を取得する
        'Dim dtBranchOperatingHours As TabletSmbCommonClassBranchOperatingHoursDataTable = _
        '        Me.GetBranchOperatingHours(objStaffContext.DlrCD, objStaffContext.BrnCD)


        'If dtBranchOperatingHours.Count = 0 Then
        '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E ExceptionError:GetBranchOperatingHours" _
        '                              , MethodBase.GetCurrentMethod.Name))
        '    Return ActionResult.ExceptionError
        'End If
        ''営業開始終了日時を設定する
        'Dim stallStartTime As Date = New Date(rsltStartDateTime.Year, rsltStartDateTime.Month, rsltStartDateTime.Day, _
        '                                      dtBranchOperatingHours(0).SVC_JOB_START_TIME.Hour, dtBranchOperatingHours(0).SVC_JOB_START_TIME.Minute, 0)
        'Dim stallEndTime As Date = New Date(rsltStartDateTime.Year, rsltStartDateTime.Month, rsltStartDateTime.Day, _
        '                                      dtBranchOperatingHours(0).SVC_JOB_END_TIME.Hour, dtBranchOperatingHours(0).SVC_JOB_END_TIME.Minute, 0)

        'Push送信フラグにFalseを初期化(送信しない)
        NeedPushAfterStartSingleJob = False

        'チップ開始フラグ(True:チップが作業中、False:チップが未作業)
        Dim chipStartedFlg As Boolean = False

        '既にチップが作業中(ストール利用ステータスが開始中、作業計画の一部の作業が中断)の場合、チップ開始フラグにTrueを設定する
        If StalluseStatusStart.Equals(dtChipEntity(0).STALL_USE_STATUS) _
            Or StalluseStatusStartIncludeStopJob.Equals(dtChipEntity(0).STALL_USE_STATUS) Then

            chipStartedFlg = True

        End If

        '営業開始と終了時間を取得する
        Dim dtBranchOperatingHours As TabletSmbCommonClassBranchOperatingHoursDataTable = _
            Me.GetOneDayBrnOperatingHours(rsltStartDateTime, _
                                          objStaffContext.DlrCD, _
                                          objStaffContext.BrnCD)

        'Nothingの場合、予期せぬエラーを出す
        If IsNothing(dtBranchOperatingHours) Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. ExceptionError:GetOneDayBrnOperatingHours" _
                          , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.ExceptionError

        End If

        '営業開始日時を設定する
        Dim stallStartTime As Date = dtBranchOperatingHours(0).SVC_JOB_START_TIME

        '営業終了日時を設定する
        Dim stallEndTime As Date = dtBranchOperatingHours(0).SVC_JOB_END_TIME

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Dim rsltStartDateTimeNoSec As Date = GetDateTimeFloorSecond(rsltStartDateTime)

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '' エンティティを取得する
        'Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId)
        'If dtChipEntity.Count <> 1 Then
        '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
        '                    , MethodBase.GetCurrentMethod.Name))
        '    Return ActionResult.GetChipEntityError
        'End If

        stallId = dtChipEntity(0).STALL_ID
        Dim svcInId As Decimal = dtChipEntity(0).SVCIN_ID

        ''RO NOを紐付けてるかチェックする
        'Dim roNum As String = dtChipEntity(0).RO_NUM
        'If String.IsNullOrEmpty(roNum.Trim()) Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0} NotSetroNoError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
        '    Return ActionResult.NotSetroNoError
        'End If
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        'restFlg設定してない場合、1に設定する
        If IsNothing(restFlg) Then
            restFlg = RestTimeGetFlgGetRest
        End If

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''該ストールのテクニシャン人数を取得する
        'Dim staffStallDataList As TabletSmbCommonClassStringValueDataTable = GetStaffCodeByStallId(objStaffContext.DlrCD, objStaffContext.BrnCD, stallId)
        'Dim nCount As Long = staffStallDataList.Count
        ''テクニシャンがない場合
        'If nCount = 0 Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0} NoTechnicianError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
        '    Return ActionResult.NoTechnicianError
        'End If

        ''ステータス遷移可否をチェックする
        'If Not CanWorkStart(dtChipEntity(0).SVC_STATUS, dtChipEntity(0).STALL_USE_STATUS, _
        '                                    dtChipEntity(0).TEMP_FLG, dtChipEntity(0).STALL_ID, dtChipEntity(0).SCHE_START_DATETIME) Then
        '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
        '            , MethodBase.GetCurrentMethod.Name))
        '    Return ActionResult.CheckError
        'End If

        'Dim roJobSeq As Long = dtChipEntity(0).RO_JOB_SEQ
        'チップ操作制約チェックを行う
        'Dim rtStart As Long = ValidateStart(svcInId, objStaffContext, stallId, dtChipEntity(0).SVC_CLASS_ID, dtChipEntity(0).MERC_ID, rsltStartDateTime, updateDate, stallStartTime, stallEndTime, roJobSeq)

        'If rtStart <> ActionResult.Success Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.E return={1}. ", MethodBase.GetCurrentMethod.Name, rtStart))
        '    Return rtStart
        'End If

        '追加作業の場合
        'If roJobSeq > 0 Then
        '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0} IC3800805.UpdateAddRepairStatus start. PARAM:dlrcd={1},ronum={2},rojobseq={3} ", _
        '                               MethodBase.GetCurrentMethod.Name, objStaffContext.DlrCD, dtChipEntity(0).RO_NUM, dtChipEntity(0).RO_JOB_SEQ))
        '    Dim IC3800805 As New IC3800805BusinessLogic
        '    Dim rtnVal As Integer = IC3800805.UpdateAddRepairStatus(objStaffContext.DlrCD, dtChipEntity(0).RO_NUM, CType(dtChipEntity(0).RO_JOB_SEQ, Integer))
        '    '追加作業の更新失敗
        '    If rtnVal <> 0 Then
        '        Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0} IC3800805.UpdateAddRepairStatus failed.Returncode={1} ", MethodBase.GetCurrentMethod.Name, rtnVal))
        '        Return ActionResult.ExceptionError
        '    Else
        '        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0} IC3800805.UpdateAddRepairStatus successed.Returncode={1} ", MethodBase.GetCurrentMethod.Name, rtnVal))
        '    End If
        'End If

        '***********************************************************************
        ' 2. ストールロック
        '***********************************************************************

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

        'チップ開始してない場合、ストールロックをする
        If Not chipStartedFlg Then

            '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
            'ストールロック
            Dim result As Long = Me.LockStall(stallId, rsltStartDateTime, objStaffContext.Account, updateDate, systemId)
            If result <> ActionResult.Success Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:LockStallError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return result
            End If
            isStallLocked = True

            '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        End If
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END


        '***********************************************************************
        ' 3. いろいろなチェックをする
        '***********************************************************************

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        'Dim rsltCheck As Long = Me.CheckStartAction(dtChipEntity(0), _
        '                                    rsltStartDateTimeNoSec, _
        '                                    stallStartTime, _
        '                                    stallEndTime, _
        '                                    rowLockVersion, _
        '                                    objStaffContext, _
        '                                    updateDate, _
        '                                    systemId, _
        '                                    callerType)
        'If rsltCheck <> ActionResult.Success Then
        '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckStartAction error: Error num is {1}" _
        '                                , MethodBase.GetCurrentMethod.Name _
        '                                , rsltCheck))
        '    Return rsltCheck
        'End If

        'チェック結果
        Dim rsltCheck As Long

        'チップがまだ開始してない場合
        If Not chipStartedFlg Then

            '未開始チップに対する開始操作のチェックを行う
            rsltCheck = Me.CheckStartAction(dtChipEntity(0), _
                                            rsltStartDateTimeNoSec, _
                                            stallStartTime, _
                                            stallEndTime, _
                                            rowLockVersion, _
                                            objStaffContext, _
                                            updateDate, _
                                            systemId, _
                                            callerType)

        Else

            '作業中チップに紐づくJobの開始チェック
            rsltCheck = Me.CheckStartedStartAction(dtChipEntity(0), _
                                                   rsltStartDateTimeNoSec, _
                                                   stallStartTime, _
                                                   stallEndTime, _
                                                   rowLockVersion, _
                                                   objStaffContext, _
                                                   updateDate, _
                                                   systemId)

        End If

        'チェック結果がエラーの場合、エラーコードを戻す
        If rsltCheck <> ActionResult.Success Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.E CheckStartAction error: Error num is {1}", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       rsltCheck))
            Return rsltCheck

        End If
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        '作業実績送信使用するフラグを取得する
        Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()

        '作業実績送信の場合、作業ステータスを取得する
        Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing
        If isUseJobDispatch Then
            prevJobStatus = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)
        End If

        '更新前のステータス取得
        Dim prevStatus As String = Me.JudgeChipStatus(stallUseId)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '2013/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ''作業終了日時取得を取得する
        'Dim serviceEndDateTime As Date = GetServiceEndDateTime(stallId, _
        '                                                    rsltStartDateTimeNoSec, _
        '                                                    dtChipEntity(0).SCHE_WORKTIME, _
        '                                                    stallStartTime, _
        '                                                    stallEndTime, _
        '                                                    restFlg)
        ''update用データセット
        'Using targetDtChipEntity As New TabletSmbCommonClassChipEntityDataTable
        '    Dim targetdrChipEntity As TabletSmbCommonClassChipEntityRow = CType(targetDtChipEntity.NewRow(), TabletSmbCommonClassChipEntityRow)
        '    '更新処理を実行する
        '    Using ta As New TabletSMBCommonClassDataAdapter

        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '        '該チップ紐付くROステータスを60に設定する、0行の可能性があるので、戻り値判断する必要がない
        '        ta.UpdateROStatusByJobDtlId(dtChipEntity(0).SVCIN_ID, _
        '                                    dtChipEntity(0).JOB_DTL_ID, _
        '                                    RostatusWorkinProgress, _
        '                                    updateDate, _
        '                                    objStaffContext.Account, _
        '                                    systemId)
        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '        'サービス入庫のステータス(作業中)を更新する
        '        Dim updateCnt As Long = ta.UpdateSvcinStatus(svcInId, SvcStatusStart, updateDate, objStaffContext.Account)
        '        If updateCnt <> 1 Then
        '            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E UpdateSvcinStatus Error.", MethodBase.GetCurrentMethod.Name))
        '            Return ActionResult.ExceptionError
        '        End If

        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '        Dim insertResult As Long = Me.InsertJobResult(dtChipEntity(0).JOB_DTL_ID, _
        '                                                      stallId, _
        '                                                      rsltStartDateTime, _
        '                                                      updateDate, _
        '                                                      objStaffContext, _
        '                                                      systemId)
        '        If insertResult <> ActionResult.Success Then
        '            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E InsertJobResult ExceptionError.", MethodBase.GetCurrentMethod.Name))
        '            Return ActionResult.ExceptionError
        '        End If
        '        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '        'スタッフ作業に1行を挿入する
        '        Dim jobId As Decimal = Me.InsertStaffJob(stallId, rsltStartDateTime, updateDate, objStaffContext, systemId)

        '        targetdrChipEntity.STALL_USE_ID = stallUseId
        '        targetdrChipEntity.JOB_ID = jobId
        '        targetdrChipEntity.RSLT_START_DATETIME = rsltStartDateTimeNoSec
        '        targetdrChipEntity.PRMS_END_DATETIME = serviceEndDateTime
        '        targetdrChipEntity.REST_FLG = restFlg
        '        targetdrChipEntity.STALL_USE_STATUS = StalluseStatusStart
        '        targetdrChipEntity.UPDATE_DATETIME = updateDate
        '        targetdrChipEntity.UPDATE_STF_CD = objStaffContext.Account
        '        '更新
        '        updateCnt = ta.UpdateStallUseRsltStartDate(targetdrChipEntity, systemId)
        '        If updateCnt <> 1 Then
        '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E UpdateStallUseRsltStartDate Error.", MethodBase.GetCurrentMethod.Name))
        '            Return ActionResult.ExceptionError
        '        End If
        '    End Using
        'End Using


        'チップが未開始の場合
        If Not chipStartedFlg Then
            'DB更新
            Dim updateRslt As Long = Me.UpdateDataByStart(dtChipEntity(0), _
                                                          stallId, _
                                                          rsltStartDateTimeNoSec, _
                                                          stallStartTime, _
                                                          stallEndTime, _
                                                          restFlg, _
                                                          updateDate, _
                                                          systemId, _
                                                          objStaffContext, _
                                                          restartStopJobFlg, _
                                                          callerType)
            If updateRslt <> ActionResult.Success Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} UpdateDataByStart FAILURE " _
                                        , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                Return updateRslt
            End If

        Else
            'チップが既に開始中の場合

            '開始中チップに作業の開始DB更新処理
            Dim updateRslt As Long = Me.UpdateDataByStartedChipStart(dtChipEntity(0), _
                                                                     stallId, _
                                                                     rsltStartDateTimeNoSec, _
                                                                     stallStartTime, _
                                                                     stallEndTime, _
                                                                     restFlg, _
                                                                     updateDate, _
                                                                     systemId, _
                                                                     objStaffContext, _
                                                                     callerType, _
                                                                     restartStopJobFlg)

            '更新結果が異常の場合、エラーコードを戻す
            If updateRslt <> ActionResult.Success Then

                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                           "{0}.{1} UpdateDataByStartedChipStart Failure. ", _
                                           Me.GetType.ToString, _
                                           MethodBase.GetCurrentMethod.Name))

                Return updateRslt

            End If

        End If
        '2013/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '更新後のステータス取得
        Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)

        '基幹側にステータス情報を送信
        Using ic3802601blc As New IC3802601BusinessLogic
            Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(svcInId, _
                                                                    dtChipEntity(0).JOB_DTL_ID, _
                                                                    stallUseId, _
                                                                    prevStatus, _
                                                                    crntStatus, _
                                                                    0)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If dmsSendResult <> 0 Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
            '                               , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '処理結果チェック
            If dmsSendResult = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using


        '実績送信使用の場合
        If isUseJobDispatch Then

            '作業実績送信の場合、作業ステータスを取得する
            Dim crntJobStatus As IC3802701JobStatusDataTable = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)

            '基幹側にJobDispatch実績情報を送信
            Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(svcInId, _
                                                                   dtChipEntity(0).JOB_DTL_ID, _
                                                                   prevJobStatus, _
                                                                   crntJobStatus)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If resultSendJobClock <> ActionResult.Success Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.End DmsLinkageError:SendJobClockOnInfo FAILURE " _
            '                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '処理結果チェック
            If resultSendJobClock = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf resultSendJobClock = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendJobClockOnInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End If

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        'まだ開始してない場合、
        If Not chipStartedFlg Then
            'ここまでreturnしてないなら、チップが作業前→作業中になた
            'Pushをする
            NeedPushAfterStartSingleJob = True

        End If
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End", MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 単独Job開始の主な処理
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inRsltStartDateTime">実績開始日時</param>
    ''' <param name="inRestFlg">休憩取得フラグ</param>
    ''' <param name="inJobInstructId">作業指示ID</param>
    ''' <param name="inJobInstructSeq">作業指示枝番</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inSystemId">画面ID</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    Public Function StartSingleJob(ByVal inStallUseId As Decimal, _
                                   ByVal inRsltStartDateTime As Date, _
                                   ByVal inRestFlg As String, _
                                   ByVal inJobInstructId As String, _
                                   ByVal inJobInstructSeq As Long, _
                                   ByVal inUpdateDate As Date, _
                                   ByVal inRowLockVersion As Long, _
                                   ByVal inSystemId As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inStallUseId={1}, inRsltStartDateTime={2}, inRestFlg={3}, inJobInstructId={4}, inJobInstructSeq={5}, inUpdateDate={6}, inRowLockVersion={7}, inSystemId={8}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inStallUseId, _
                                  inRsltStartDateTime, _
                                  inRestFlg, _
                                  inJobInstructId, _
                                  inJobInstructSeq, _
                                  inUpdateDate, _
                                  inRowLockVersion, _
                                  inSystemId))

        'Push送信フラグにFalseを初期化(送信しない)
        NeedPushAfterStartSingleJob = False

        'ログイン情報の取得
        Dim objStaffContext As StaffContext = StaffContext.Current

        'チップエンティティ
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = Nothing

        Try
            'エンティティを取得する
            dtChipEntity = GetChipEntity(inStallUseId)

            If 1 <> dtChipEntity.Count Then
                '取得件数が1件でない場合(異常)

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E GetChipEntityError", _
                                           MethodBase.GetCurrentMethod.Name))
                Return ActionResult.GetChipEntityError

            End If

            'ストールロック変数初期化(ロックしてない)
            isStallLocked = False

            '単独Jobを開始する
            Dim result As Long = Me.StartSingleJobAction(inStallUseId, _
                                                         inRsltStartDateTime, _
                                                         inRestFlg, _
                                                         inJobInstructId, _
                                                         inJobInstructSeq, _
                                                         inUpdateDate, _
                                                         inRowLockVersion, _
                                                         inSystemId, _
                                                         dtChipEntity(0))

            Return result

        Catch ex As OracleExceptionEx When ex.Number = 1013

            'DBタイムアウトの場合、DBタイムアウトエラーコードを戻す
            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.End Error:DBTimeOutError.", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))

            Return ActionResult.DBTimeOutError

        Finally

            'ストールロックされている場合
            If isStallLocked Then

                'ストールロック解除
                Me.LockStallReset(dtChipEntity(0).STALL_ID, _
                                  inRsltStartDateTime, _
                                  objStaffContext.Account, _
                                  inUpdateDate, _
                                  inSystemId)
            End If

        End Try

    End Function

    '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    ''' <summary>
    ''' 単独Jobの開始処理
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inRsltStartDateTime">実績開始時間</param>
    ''' <param name="inRestFlg">休憩取得フラグ</param>
    ''' <param name="inJobInstructId">作業指示ID</param>
    ''' <param name="inJobInstructSeq">作業指示連番</param>
    ''' <param name="inUpdateDate">更新時間</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inSystemId">更新クラス</param>
    ''' <param name="inDatarowChipEntity">チップエンティティ</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Private Function StartSingleJobAction(ByVal inStallUseId As Decimal, _
                                          ByVal inRsltStartDateTime As Date, _
                                          ByVal inRestFlg As String, _
                                          ByVal inJobInstructId As String, _
                                          ByVal inJobInstructSeq As Long, _
                                          ByVal inUpdateDate As Date, _
                                          ByVal inRowLockVersion As Long, _
                                          ByVal inSystemId As String, _
                                          ByVal inDatarowChipEntity As TabletSmbCommonClassChipEntityRow) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inStallUseId={1}, inRsltStartDateTime={2}, inRestFlg={3}, inJobInstructId={4}, inJobInstructSeq={5}, updateDate={6}, rowLockVersion={7}, systemId={8}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inStallUseId, _
                                  inRsltStartDateTime, _
                                  inRestFlg, _
                                  inJobInstructId, _
                                  inJobInstructSeq, _
                                  inUpdateDate, _
                                  inRowLockVersion, _
                                  inSystemId))
        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '***********************************************************************
        ' 1. 変数初期化
        '***********************************************************************

        'チップ開始フラグ(True:チップが作業中、False:チップが未作業)
        Dim chipStartedFlg As Boolean = False

        '既にチップが作業中(ストール利用ステータスが開始中、作業計画の一部の作業が中断)の場合、チップ開始フラグにTrueを設定する
        If StalluseStatusStart.Equals(inDatarowChipEntity.STALL_USE_STATUS) _
            Or StalluseStatusStartIncludeStopJob.Equals(inDatarowChipEntity.STALL_USE_STATUS) Then
            chipStartedFlg = True
        End If

        'ログイン情報の取得
        Dim objStaffContext As StaffContext = StaffContext.Current


        '該当作業の作業ステータスを取得する
        Dim jobStatus As String = Me.GetJobStatus(inDatarowChipEntity.JOB_DTL_ID, _
                                                  inJobInstructId, _
                                                  inJobInstructSeq)

        '営業開始と終了時間を取得する
        Dim dtBranchOperatingHours As TabletSmbCommonClassBranchOperatingHoursDataTable = _
            Me.GetOneDayBrnOperatingHours(inRsltStartDateTime, _
                                          objStaffContext.DlrCD, _
                                          objStaffContext.BrnCD)

        'Nothingの場合、予期せぬエラーを出す
        If IsNothing(dtBranchOperatingHours) Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.End. ExceptionError:GetOneDayBrnOperatingHours" _
                                       , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.ExceptionError

        End If

        '営業開始日時を設定する
        Dim stallStartTime As Date = dtBranchOperatingHours(0).SVC_JOB_START_TIME

        '営業終了日時を設定する
        Dim stallEndTime As Date = dtBranchOperatingHours(0).SVC_JOB_END_TIME

        '実績開始日時に秒を切り捨てる
        Dim rsltStartDateTimeNoSec As Date = GetDateTimeFloorSecond(inRsltStartDateTime)

        'ストールID
        Dim stallId As Decimal = inDatarowChipEntity.STALL_ID

        'サービス入庫ID
        Dim svcInId As Decimal = inDatarowChipEntity.SVCIN_ID

        '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        ''引数の休憩取得フラグが設定されていない場合、1(取得する)に設定する
        'If String.IsNullOrWhiteSpace(inRestFlg) Then

        '    inRestFlg = RestTimeGetFlgGetRest

        'End If
        '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        '***********************************************************************
        ' 2. ストールロック
        '***********************************************************************
        'チップがまだ開始してない場合、ストールロックする
        If Not chipStartedFlg Then

            'ストールロック
            Dim result As Long = Me.LockStall(stallId, _
                                              inRsltStartDateTime, _
                                              objStaffContext.Account, _
                                              inUpdateDate, _
                                              inSystemId)

            'ロック失敗の場合、エラーコードを戻す
            If result <> ActionResult.Success Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:LockStallError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return result

            End If

            'ストールロックフラグにTrue(ロックした)を設定する
            isStallLocked = True

        End If

        '***********************************************************************
        ' 3. 開始前のチェック
        '***********************************************************************

        'チェック結果
        Dim rsltCheck As Long

        Dim roSeq As Long

        Using ta As New TabletSMBCommonClassDataAdapter

            '指定JobのRo連番を取得
            Dim roseqTable As TabletSmbCommonClassNumberValueDataTable = _
                ta.GetROSeqByJob(inDatarowChipEntity.JOB_DTL_ID, _
                                 inJobInstructId, _
                                 inJobInstructSeq)

            If roseqTable.Count <> 1 Then
                '取得RO連番レコードカウントが1つ以外の場合

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E GetROSeqByJob error: Query count is not 1", _
                                           MethodBase.GetCurrentMethod.Name))
                '予期せぬエラーを戻す
                Return ActionResult.ExceptionError

            Else

                'RO連番を取得
                roSeq = CType(roseqTable(0).COL1, Long)

            End If

        End Using

        'チップがまだ開始してない場合
        If Not chipStartedFlg Then

            '未開始チップに対する開始操作のチェックを行う
            rsltCheck = Me.CheckStartAction(inDatarowChipEntity, _
                                            rsltStartDateTimeNoSec, _
                                            stallStartTime, _
                                            stallEndTime, _
                                            inRowLockVersion, _
                                            objStaffContext, _
                                            inUpdateDate, _
                                            inSystemId, _
                                            CallerTypeDetailSingleJobAction, _
                                            jobStatus, _
                                            roSeq)

        Else

            '作業中チップに紐づくJobの開始チェック
            rsltCheck = Me.CheckStartedStartAction(inDatarowChipEntity, _
                                                   rsltStartDateTimeNoSec, _
                                                   stallStartTime, _
                                                   stallEndTime, _
                                                   inRowLockVersion, _
                                                   objStaffContext, _
                                                   inUpdateDate, _
                                                   inSystemId, _
                                                   jobStatus, _
                                                   roSeq)
        End If

        'チェック結果がエラーの場合、エラーコードを戻す
        If rsltCheck <> ActionResult.Success Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.E CheckStartAction error: Error num is {1}", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       rsltCheck))
            Return rsltCheck

        End If

        '***********************************************************************
        ' 4. DB更新
        '***********************************************************************

        '作業実績送信使用するフラグを取得する
        Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()

        '作業実績送信の場合、作業ステータスを取得する
        Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing

        'JobDispatch送信フラグを使用の場合、該当作業のステータスを取得する(JobDispatch送信制御用)
        If isUseJobDispatch Then

            prevJobStatus = Me.JudgeSingleJobStatus(inDatarowChipEntity.JOB_DTL_ID, _
                                                    inJobInstructId, _
                                                    inJobInstructSeq, _
                                                    jobStatus)

        End If

        '更新前のチップステータス取得(ステータス送信制御用)
        Dim prevChipStatus As String = Me.JudgeChipStatus(inStallUseId)

        'チップが未開始の場合
        If Not chipStartedFlg Then

            '未開始チップの開始DB更新処理をする
            Dim updateRslt As Long = Me.UpdateDataByStart(inDatarowChipEntity, _
                                                          stallId, _
                                                          rsltStartDateTimeNoSec, _
                                                          stallStartTime, _
                                                          stallEndTime, _
                                                          inRestFlg, _
                                                          inUpdateDate, _
                                                          inSystemId, _
                                                          objStaffContext, _
                                                          True, _
                                                          CallerTypeDetailSingleJobAction, _
                                                          inJobInstructId, _
                                                          inJobInstructSeq)

            'DB更新失敗の場合、エラーコードを戻す
            If updateRslt <> ActionResult.Success Then

                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                           "{0}.{1} UpdateDataByStart Failure. ", _
                                           Me.GetType.ToString, _
                                           MethodBase.GetCurrentMethod.Name))
                Return updateRslt

            End If

        Else
            'チップが既に開始中の場合

            '開始中チップに作業の開始DB更新処理
            Dim updateRslt As Long = Me.UpdateDataByStartedChipStart(inDatarowChipEntity, _
                                                                     stallId, _
                                                                     rsltStartDateTimeNoSec, _
                                                                     stallStartTime, _
                                                                     stallEndTime, _
                                                                     inRestFlg, _
                                                                     inUpdateDate, _
                                                                     inSystemId, _
                                                                     objStaffContext, _
                                                                     CallerTypeDetailSingleJobAction, _
                                                                     True, _
                                                                     inJobInstructId, _
                                                                     inJobInstructSeq)

            '更新結果が異常の場合、エラーコードを戻す
            If updateRslt <> ActionResult.Success Then

                Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                           "{0}.{1} UpdateDataByStartedChipStart Failure. ", _
                                           Me.GetType.ToString, _
                                           MethodBase.GetCurrentMethod.Name))
                Return updateRslt

            End If

        End If


        '***********************************************************************
        ' 5. 基幹連携
        '***********************************************************************
        '更新後のチップステータス取得(ステータス送信制御用)
        Dim crntChipStatus As String = Me.JudgeChipStatus(inStallUseId)

        'チップが未開始の場合
        If Not chipStartedFlg Then

            '基幹側にステータス情報を送信
            Using ic3802601blc As New IC3802601BusinessLogic

                'ステータス送信をする
                Dim resultSendStatusInfo As Long = ic3802601blc.SendStatusInfo(svcInId, _
                                                                               inDatarowChipEntity.JOB_DTL_ID, _
                                                                               inStallUseId, _
                                                                               prevChipStatus, _
                                                                               crntChipStatus, _
                                                                               0)

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''送信失敗の場合、DMS連携エラーを戻す
                'If resultSendStatusInfo <> 0 Then

                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                '                               "{0}.{1} SendStatusInfo Failure ", _
                '                               Me.GetType.ToString, _
                '                               MethodBase.GetCurrentMethod.Name))
                '    Return ActionResult.DmsLinkageError

                'End If

                '処理結果チェック
                If resultSendStatusInfo = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf resultSendStatusInfo = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End Using

        End If

        '実績送信使用の場合
        If isUseJobDispatch Then

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            ''作業実績送信の場合、作業ステータスを取得する
            'Dim crntJobStatus As IC3802701JobStatusDataTable = Me.JudgeSingleJobStatus(inDatarowChipEntity.JOB_DTL_ID, _
            '                                                                           inJobInstructId, _
            '                                                                           inJobInstructSeq, _
            '                                                                           jobStatus)

            '該当作業のデータ更新後作業ステータスを取得する
            Dim crntSingleJobStatus As String = Me.GetJobStatus(inDatarowChipEntity.JOB_DTL_ID, _
                                                                inJobInstructId, _
                                                                inJobInstructSeq)

            '作業実績送信の場合、作業ステータスを取得する
            Dim crntJobStatus As IC3802701JobStatusDataTable = Me.JudgeSingleJobStatus(inDatarowChipEntity.JOB_DTL_ID, _
                                                                                       inJobInstructId, _
                                                                                       inJobInstructSeq, _
                                                                                       crntSingleJobStatus)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            '基幹側にJobDispatch実績情報を送信
            Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(svcInId, _
                                                                   inDatarowChipEntity.JOB_DTL_ID, _
                                                                   prevJobStatus, _
                                                                   crntJobStatus)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            ''送信失敗の場合、DMS連携エラーを戻す
            'If resultSendJobClock <> ActionResult.Success Then

            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, _
            '                               "{0}.{1}.End DmsLinkageError:SendJobClockOnInfo Failure ", _
            '                               Me.GetType.ToString, _
            '                               MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError

            'End If

            '処理結果チェック
            If resultSendJobClock = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf resultSendJobClock = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendJobClockOnInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End If

        'まだ開始してない場合、
        If Not chipStartedFlg Then
            'ここまでreturnしてないなら、チップが作業前→作業中になた
            'Pushをする
            NeedPushAfterStartSingleJob = True

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End", _
                                  MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

#Region "開始処理のDB更新"

    ''' <summary>
    ''' 未開始チップを開始すると、DB更新処理
    ''' </summary>
    ''' <param name="chipEntity">チップエンティティ</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="inRsltStartDateTime">実績開始日時</param>
    ''' <param name="inStallStartTime">ストール開始日時</param>
    ''' <param name="inStallEndTime">ストール終了日時</param>
    ''' <param name="inRestFlg">休憩取得フラグ</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inSystemId">呼び出し元画面ID</param>
    ''' <param name="inObjStaffContext">スタッフ情報</param>
    ''' <param name="inRestartStopJobFlg">中断作業再開始フラグ</param>
    ''' <param name="inCallType">呼び出し元タイプ</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    Private Function UpdateDataByStart(ByVal chipEntity As TabletSmbCommonClassChipEntityRow, _
                                       ByVal inStallId As Decimal, _
                                       ByVal inRsltStartDateTime As Date, _
                                       ByVal inStallStartTime As Date, _
                                       ByVal inStallEndTime As Date, _
                                       ByVal inRestFlg As String, _
                                       ByVal inUpdateDate As Date, _
                                       ByVal inSystemId As String, _
                                       ByVal inObjStaffContext As StaffContext, _
                                       ByVal inRestartStopJobFlg As Boolean, _
                                       ByVal inCallType As Long, _
                                       Optional ByVal inInstructId As String = "", _
                                       Optional ByVal inInstructSeq As Long = 0) As Long

        '作業終了日時取得を取得する
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        'Dim serviceEndDateTime As Date = GetServiceEndDateTime(inStallId, _
        '                                                       inRsltStartDateTime, _
        '                                                       chipEntity.SCHE_WORKTIME, _
        '                                                       inStallStartTime, _
        '                                                       inStallEndTime, _
        '                                                       inRestFlg)
        '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        Dim serviceEndDateTimeData As ServiceEndDateTimeData = GetServiceEndDateTime(inStallId, _
                                                               inRsltStartDateTime, _
                                                               chipEntity.SCHE_WORKTIME, _
                                                               inStallStartTime, _
                                                               inStallEndTime, _
                                                               inRestFlg)
        '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
        Dim serviceEndDateTime As Date = serviceEndDateTimeData.ServiceEndDateTime
        Dim restFlg = serviceEndDateTimeData.RestFlg
        '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        'update用データセット
        Using targetDtChipEntity As New TabletSmbCommonClassChipEntityDataTable

            '新しいチップエンティティ行を生成する(ストール利用テーブル更新用)
            Dim targetdrChipEntity As TabletSmbCommonClassChipEntityRow = targetDtChipEntity.NewTabletSmbCommonClassChipEntityRow

            '更新処理を実行する
            Using ta As New TabletSMBCommonClassDataAdapter

                If CallerTypeDetailSingleJobAction = inCallType Then
                    '単独なJob開始の場合

                    '該当JobのROステータスに60を設定する、0行の可能性があるので、戻り値判断する必要がない
                    ta.UpdateROStatusByJob(chipEntity.JOB_DTL_ID, _
                                           inInstructId, _
                                           inInstructSeq, _
                                           RostatusWorkinProgress, _
                                           inUpdateDate, _
                                           inObjStaffContext.Account, _
                                           inSystemId)

                Else

                    '他の場合

                    '該当チップに紐付くROのステータスに60を設定する、0行の可能性があるので、戻り値判断する必要がない
                    ta.UpdateROStatusByJobDtlId(chipEntity.SVCIN_ID, _
                                                chipEntity.JOB_DTL_ID, _
                                                RostatusWorkinProgress, _
                                                inUpdateDate, _
                                                inObjStaffContext.Account, _
                                                inSystemId)

                End If

                'サービス入庫のステータス(作業中)を更新する
                Dim updateCnt As Long = ta.UpdateSvcinStatus(chipEntity.SVCIN_ID, _
                                                             SvcStatusStart, _
                                                             inUpdateDate, _
                                                             inObjStaffContext.Account)

                '更新行数が1行以外の場合
                If updateCnt <> 1 Then

                    '予期せぬエラーを戻す
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E UpdateSvcinStatus Error.", MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.ExceptionError

                End If

                If CallerTypeDetailSingleJobAction = inCallType Then
                    '単独なJob開始の場合

                    If inRestartStopJobFlg Then
                        '中断Job再開の場合、実績テーブルにJob情報を登録

                        '作業実績テーブルに1行しか登録しない
                        Dim insertResult As Long = Me.InsertOneJobResult(chipEntity.JOB_DTL_ID, _
                                                                            chipEntity.STALL_ID, _
                                                                            inRsltStartDateTime, _
                                                                            inInstructId, _
                                                                            inInstructSeq, _
                                                                            inUpdateDate, _
                                                                            inObjStaffContext.Account, _
                                                                            inSystemId)

                        '登録失敗の場合、予期せぬエラーを戻す
                        If ActionResult.Success <> insertResult Then

                            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                        "{0}.E InsertJobResult ExceptionError.", _
                                                        MethodBase.GetCurrentMethod.Name))
                            Return ActionResult.ExceptionError

                        End If

                    End If

                Else
                    '他の場合

                    '作業実績テーブルに該当チップに紐づくJobを登録する
                    Dim insertResult As Long = Me.InsertJobResult(chipEntity.JOB_DTL_ID, _
                                                                    inStallId, _
                                                                    inRsltStartDateTime, _
                                                                    inUpdateDate, _
                                                                    inObjStaffContext, _
                                                                    inSystemId, _
                                                                    inRestartStopJobFlg)

                    'エラーがあれば、エラーコードを戻す
                    If insertResult <> ActionResult.Success Then

                        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E InsertJobResult ExceptionError.", MethodBase.GetCurrentMethod.Name))
                        Return ActionResult.ExceptionError

                    End If

                End If

                'スタッフ作業に1行を挿入する
                Dim jobId As Decimal = Me.InsertStaffJob(inStallId, _
                                                         inRsltStartDateTime, _
                                                         inUpdateDate, _
                                                         inObjStaffContext, _
                                                         inSystemId)

                'ストール利用テーブル更新用値を設定する
                targetdrChipEntity.STALL_USE_ID = chipEntity.STALL_USE_ID
                targetdrChipEntity.JOB_ID = jobId                               'スタッフ作業に登録した作業ID
                targetdrChipEntity.RSLT_START_DATETIME = inRsltStartDateTime
                targetdrChipEntity.PRMS_END_DATETIME = serviceEndDateTime
                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                'targetdrChipEntity.REST_FLG = inRestFlg
                targetdrChipEntity.REST_FLG = restFlg
                '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                targetdrChipEntity.UPDATE_DATETIME = inUpdateDate
                targetdrChipEntity.UPDATE_STF_CD = inObjStaffContext.Account

                If Me.HasStopJob(chipEntity.JOB_DTL_ID) Then
                    '中断作業含む場合

                    'ストール利用ステータスに「04」作業計画の一部の作業が中断を設定する
                    targetdrChipEntity.STALL_USE_STATUS = StalluseStatusStartIncludeStopJob

                Else
                    '中断作業含まない場合

                    'ストール利用ステータスに「02」作業中を設定する
                    targetdrChipEntity.STALL_USE_STATUS = StalluseStatusStart

                End If

                'ストール利用テーブルにデータを更新する
                updateCnt = ta.UpdateStallUseRsltStartDate(targetdrChipEntity, inSystemId)

                '更新行数が1行以外の場合、予期せぬエラーを戻す
                If updateCnt <> 1 Then

                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E UpdateStallUseRsltStartDate Error.", MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.ExceptionError

                End If

            End Using

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E ", MethodBase.GetCurrentMethod.Name))

        Return ActionResult.Success

    End Function

    '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    ''' <summary>
    ''' 開始中チップに作業を開始すると、DB更新処理
    ''' </summary>
    ''' <param name="chipEntity">チップエンティティ</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="inStallEndTime">ストール終了日時</param>
    ''' <param name="inRestFlg">休憩取得フラグ</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inSystemId">呼び出し元画面ID</param>
    ''' <param name="inObjStaffContext">スタッフ情報</param>
    ''' <param name="inRsltStartDateTime">実績開始日時</param>
    ''' <param name="inStallStartTime">ストール開始日時</param>
    ''' <param name="inRestartStopJobFlg">中断Job再開フラグ</param>
    ''' <returns>正常終了：0、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    Private Function UpdateDataByStartedChipStart(ByVal chipEntity As TabletSmbCommonClassChipEntityRow, _
                                                  ByVal inStallId As Decimal, _
                                                  ByVal inRsltStartDateTime As Date, _
                                                  ByVal inStallStartTime As Date, _
                                                  ByVal inStallEndTime As Date, _
                                                  ByVal inRestFlg As String, _
                                                  ByVal inUpdateDate As Date, _
                                                  ByVal inSystemId As String, _
                                                  ByVal inObjStaffContext As StaffContext, _
                                                  ByVal inCallType As Long, _
                                                  ByVal inRestartStopJobFlg As Boolean, _
                                                  Optional ByVal inInstructId As String = "", _
                                                  Optional ByVal inInstructSeq As Long = 0) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inStallId={1}, inRsltStartDateTime={2}, inStallStartTime={3}, inStallEndTime={4}, inRestFlg={5}, inUpdateDate={6}, inSystemId={7}, inRestartStopJobFlg={8}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inStallId, _
                                  inRsltStartDateTime, _
                                  inStallStartTime, _
                                  inStallEndTime, _
                                  inRestFlg, _
                                  inUpdateDate, _
                                  inSystemId, _
                                  inRestartStopJobFlg))

        'update用データセット
        Using targetDtChipEntity As New TabletSmbCommonClassChipEntityDataTable

            '新しいチップエンティティ行を生成する(ストール利用テーブル更新用)
            Dim targetdrChipEntity As TabletSmbCommonClassChipEntityRow = targetDtChipEntity.NewTabletSmbCommonClassChipEntityRow

            '更新処理を実行する
            Using ta As New TabletSMBCommonClassDataAdapter

                If CallerTypeDetailSingleJobAction = inCallType Then
                    '単独なJob開始の場合

                    '該当JobのROステータスを60に設定する、0行の可能性があるので、戻り値判断する必要がない
                    ta.UpdateROStatusByJob(chipEntity.JOB_DTL_ID, _
                                           inInstructId, _
                                           inInstructSeq, _
                                           RostatusWorkinProgress, _
                                           inUpdateDate, _
                                           inObjStaffContext.Account, _
                                           inSystemId)

                    '作業実績テーブルに1行しか登録しない
                    Dim insertResult As Long = Me.InsertOneJobResult(chipEntity.JOB_DTL_ID, _
                                                                     chipEntity.STALL_ID, _
                                                                     inRsltStartDateTime, _
                                                                     inInstructId, _
                                                                     inInstructSeq, _
                                                                     inUpdateDate, _
                                                                     inObjStaffContext.Account, _
                                                                     inSystemId)

                    '登録失敗の場合、予期せぬエラーを戻す
                    If ActionResult.Success <> insertResult Then

                        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                   "{0}.E InsertJobResult ExceptionError.", _
                                                   MethodBase.GetCurrentMethod.Name))
                        Return ActionResult.ExceptionError

                    End If

                Else
                    '他の場合

                    '該チップ紐付くJobのROステータスを60に設定する、0行の可能性があるので、戻り値判断する必要がない
                    ta.UpdateROStatusByJobDtlId(chipEntity.SVCIN_ID, _
                                                chipEntity.JOB_DTL_ID, _
                                                RostatusWorkinProgress, _
                                                inUpdateDate, _
                                                inObjStaffContext.Account, _
                                                inSystemId)

                    '作業実績テーブルに該当チップに紐づくJobを登録する
                    Dim insertResult As Long = Me.InsertJobResult(chipEntity.JOB_DTL_ID, _
                                                                  inStallId, _
                                                                  inRsltStartDateTime, _
                                                                  inUpdateDate, _
                                                                  inObjStaffContext, _
                                                                  inSystemId, _
                                                                  inRestartStopJobFlg)

                    'エラーがあれば、エラーコードを戻す
                    If insertResult <> ActionResult.Success Then

                        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E InsertJobResult ExceptionError.", MethodBase.GetCurrentMethod.Name))
                        Return ActionResult.ExceptionError

                    End If

                End If

                '作業計画の一部の作業が中断の場合
                '(2つJobが中断、チップが作業中、この2つJobを1つづつRestartしたら、ストール利用ステータスが作業中になる)
                If StalluseStatusStartIncludeStopJob.Equals(chipEntity.STALL_USE_STATUS) Then

                    '中断作業が無くなった場合、ストール利用ステータスに「開始中」を更新する
                    If Not Me.HasStopJob(chipEntity.JOB_DTL_ID) Then

                        'ストール利用テーブル更新用データを準備する
                        targetdrChipEntity.STALL_USE_ID = chipEntity.STALL_USE_ID
                        targetdrChipEntity.JOB_ID = chipEntity.JOB_ID
                        targetdrChipEntity.RSLT_START_DATETIME = chipEntity.RSLT_START_DATETIME
                        targetdrChipEntity.PRMS_END_DATETIME = chipEntity.PRMS_END_DATETIME
                        targetdrChipEntity.REST_FLG = chipEntity.REST_FLG
                        targetdrChipEntity.STALL_USE_STATUS = StalluseStatusStart
                        targetdrChipEntity.UPDATE_DATETIME = inUpdateDate
                        targetdrChipEntity.UPDATE_STF_CD = inObjStaffContext.Account

                        'ストール利用テーブルを更新する
                        Dim updateCnt As Long = ta.UpdateStallUseRsltStartDate(targetdrChipEntity, inSystemId)

                        '更新行数が1行以外の場合、予期せぬエラーを戻す
                        If 1 <> updateCnt Then

                            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E UpdateStallUseRsltStartDate Error.", MethodBase.GetCurrentMethod.Name))
                            Return ActionResult.ExceptionError

                        End If

                    End If

                End If

            End Using

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.E ", _
                                  MethodBase.GetCurrentMethod.Name))

        Return ActionResult.Success

    End Function

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

#Region "開始処理--スタッフ作業挿入"
    ''' <summary>
    ''' スタッフ作業テーブルに1行を挿入
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="rsltStartDateTime">移動情報</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフコード</param>
    ''' <param name="systemId">更新クラス</param>
    ''' <returns>JOB ID</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/07 TMEJ 小澤 BTS-XXX JOB_IDのシーケンス設定を修正
    ''' </history>
    Private Function InsertStaffJob(ByVal stallId As Decimal, _
                                    ByVal rsltStartDateTime As Date, _
                                    ByVal updateDate As Date, _
                                    ByVal objStaffContext As StaffContext, _
                                    ByVal systemId As String) As Decimal
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, rsltStartDateTime={2}, updateDate={3}" _
                    , MethodBase.GetCurrentMethod.Name, stallId, rsltStartDateTime, updateDate))
        Dim jobId As Decimal
        'スタッフストール割当を取得する
        Dim staffStallDataList As TabletSmbCommonClassStringValueDataTable = GetStaffCodeByStallId(objStaffContext.DlrCD, objStaffContext.BrnCD, stallId)
        '取得した件数が1件以上存在した場合
        If staffStallDataList.Count >= 1 Then
            Using ta As New TabletSMBCommonClassDataAdapter

                '2015/04/07 TMEJ 小澤 BTS-XXX JOB_IDのシーケンス設定を修正 START

                'jobId = ta.GetSequenceNextVal(StfJobIdSeq)

                jobId = ta.GetSequenceNextVal(JobIdSeq)

                '2015/04/07 TMEJ 小澤 BTS-XXX JOB_IDのシーケンス設定を修正 END


                For Each staffStall As TabletSmbCommonClassStringValueRow In staffStallDataList
                    Using dtStaffJob As New TabletSmbCommonClassStaffJobDataTable
                        Dim drStaffJob As TabletSmbCommonClassStaffJobRow = CType(dtStaffJob.NewRow(), TabletSmbCommonClassStaffJobRow)
                        Dim defaultDate As Date = DefaultDateTimeValueGet()

                        'スタッフ作業を登録する
                        drStaffJob.STF_JOB_ID = ta.GetSequenceNextVal(StfJobIdSeq)
                        drStaffJob.STF_CD = staffStall.COL1
                        drStaffJob.JOB_ID = jobId
                        drStaffJob.JOB_TYPE = JobTypeStallWork
                        drStaffJob.SCHE_START_DATETIME = defaultDate
                        drStaffJob.SCHE_END_DATETIME = defaultDate
                        drStaffJob.RSLT_START_DATETIME = rsltStartDateTime
                        drStaffJob.RSLT_END_DATETIME = defaultDate
                        '１行を挿入する
                        ta.InsertTblStaffJob(drStaffJob, updateDate, objStaffContext.Account, systemId)
                    End Using
                Next
            End Using

        End If
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E jobId={1}", MethodBase.GetCurrentMethod.Name, jobId))
        Return jobId
    End Function
#End Region

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
#Region "開始処理--作業実績挿入"

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ' ''' <summary>
    ' ''' 作業実績テーブルにN行を挿入
    ' ''' </summary>
    ' ''' <param name="stallId">ストールID</param>
    ' ''' <param name="rsltStartDateTime">移動情報</param>
    ' ''' <param name="updateDate">更新日時</param>
    ' ''' <param name="objStaffContext">スタッフコード</param>
    ' ''' <param name="systemId">更新クラス</param>
    ' ''' <returns>ActionResult</returns>
    ' ''' <remarks></remarks>
    'Private Function InsertJobResult(ByVal jobDtlId As Decimal, _
    '                                 ByVal stallId As Decimal, _
    '                                 ByVal rsltStartDateTime As Date, _
    '                                 ByVal updateDate As Date, _
    '                                 ByVal objStaffContext As StaffContext, _
    '                                 ByVal systemId As String) As Long

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                              "{0}.S. stallId={1}, rsltStartDateTime={2}, updateDate={3}, jobDtlId={4}", _
    '                              MethodBase.GetCurrentMethod.Name, _
    '                              stallId, _
    '                              rsltStartDateTime, _
    '                              updateDate, _
    '                              jobDtlId))

    ''' <summary>
    ''' 作業実績テーブルにN行を挿入
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="rsltStartDateTime">移動情報</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフコード</param>
    ''' <param name="systemId">更新クラス</param>
    ''' <param name="restartStopJobFlg">中断中Job再開フラグ</param>
    ''' <returns>ActionResult</returns>
    ''' <remarks></remarks>
    Private Function InsertJobResult(ByVal jobDtlId As Decimal, _
                                     ByVal stallId As Decimal, _
                                     ByVal rsltStartDateTime As Date, _
                                     ByVal updateDate As Date, _
                                     ByVal objStaffContext As StaffContext, _
                                     ByVal systemId As String, _
                                     ByVal restartStopJobFlg As Boolean) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. stallId={1}, rsltStartDateTime={2}, updateDate={3}, jobDtlId={4}, systemId={5}, restartStopJobFlg={6}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  stallId, _
                                  rsltStartDateTime, _
                                  updateDate, _
                                  jobDtlId, _
                                  systemId, _
                                  restartStopJobFlg))
        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        Using ta As New TabletSMBCommonClassDataAdapter
            '指定作業内容IDの全作業指示情報(着工した)を取得する
            Dim jobInstructDataTable As TabletSmbCommonClassJobInstructDataTable = ta.GetJobInstructIdAndSeqByJobDtlId(jobDtlId)

            If jobInstructDataTable.Rows.Count = 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                           "{0}.E ExceptionError: GetJobInstruct query count=0.", _
                                           MethodBase.GetCurrentMethod.Name))
                Return ActionResult.ExceptionError
            End If

            '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
            '該当チップに紐づく全Jobのステータスを取得
            Dim jobStatusTable As TabletSmbCommonClassJobStatusDataTable = ta.GetAllJobRsltInfoByJobDtlId(jobDtlId)
            '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

            Using jobResultDataTable As New TabletSmbCommonClassJobResultDataTable

                '作業指示テーブルの行数により、作業実績テーブルにデータを登録する
                For Each jobInstructRow As TabletSmbCommonClassJobInstructRow In jobInstructDataTable

                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

                    '        '作業実績テーブルにInsert用の行のデータを登録
                    '        Dim jobResultRow As TabletSmbCommonClassJobResultRow = jobResultDataTable.NewTabletSmbCommonClassJobResultRow
                    '        jobResultRow.JOB_RSLT_ID = ta.GetSequenceNextVal(JobRsltSeq)
                    '        jobResultRow.JOB_DTL_ID = jobDtlId
                    '        jobResultRow.JOB_INSTRUCT_ID = jobInstructRow.JOB_INSTRUCT_ID
                    '        jobResultRow.JOB_INSTRUCT_SEQ = jobInstructRow.JOB_INSTRUCT_SEQ
                    '        jobResultRow.STALL_ID = stallId
                    '        jobResultRow.RSLT_START_DATETIME = rsltStartDateTime
                    '        jobResultRow.RSLT_END_DATETIME = Me.DefaultDateTimeValueGet()
                    '        jobResultRow.JOB_STATUS = JobStatusWorking
                    '        '1行をinsertする
                    '        Dim insertCount As Long = ta.InsertJobResult(jobResultRow, _
                    '                                                     updateDate, _
                    '                                                     objStaffContext.Account, _
                    '                                                     systemId)
                    '        If insertCount <> 1 Then
                    '            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                    '                                       "{0}.E ExceptionError: InsertJobResult count={1}. ", _
                    '                                       MethodBase.GetCurrentMethod.Name, _
                    '                                       insertCount))
                    '            Return ActionResult.ExceptionError
                    '        End If

                    '該当ループJobのステータスを取得
                    Dim jobInstructId As String = jobInstructRow.JOB_INSTRUCT_ID
                    Dim jobInstrutcSeq As Long = jobInstructRow.JOB_INSTRUCT_SEQ

                    '中断実績のデータを洗い出す
                    Dim jobStatusList As List(Of TabletSmbCommonClassJobStatusRow) = _
                        (From p In jobStatusTable _
                         Where p.JOB_INSTRUCT_ID = jobInstructId _
                         And p.JOB_INSTRUCT_SEQ = jobInstrutcSeq _
                         Order By p.JOB_RSLT_ID Descending _
                         Select p).ToList()

                    '作業ステータスを未開始で初期化
                    Dim jobStatus As String = JobStatusBeforeStart

                    If jobStatusList.Count > 0 Then

                        '実績テーブルにデータがあれば、実績テーブルの作業ステータスに設定する
                        jobStatus = jobStatusList(0).JOB_STATUS

                    End If

                    If restartStopJobFlg Then
                        '中断作業再開の場合

                        If JobStatusFinish.Equals(jobStatus) _
                            Or JobStatusWorking.Equals(jobStatus) Then
                            '該当Jobが作業中、作業完了の場合

                            'Continue実績テーブルに挿入不可
                            Continue For

                        End If

                    Else
                        '中断作業再開しない場合
                        If JobStatusFinish.Equals(jobStatus) _
                            Or JobStatusWorking.Equals(jobStatus) _
                            Or JobStatusStop.Equals(jobStatus) Then
                            '該当Jobが作業中、作業完了、作業中断の場合

                            'Continue実績テーブルに挿入不可
                            Continue For

                        End If

                    End If

                    '1行をinsertする
                    Dim insertResult As Long = Me.InsertOneJobResult(jobDtlId, _
                                                                     stallId, _
                                                                     rsltStartDateTime, _
                                                                     jobInstructRow.JOB_INSTRUCT_ID, _
                                                                     jobInstructRow.JOB_INSTRUCT_SEQ, _
                                                                     updateDate, _
                                                                     objStaffContext.Account, _
                                                                     systemId)

                    '登録失敗の場合、予期せぬエラーコードを戻す
                    If ActionResult.Success <> insertResult Then

                        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                   "{0}.E ExceptionError: InsertJobResult result={1}. ", _
                                                   MethodBase.GetCurrentMethod.Name, _
                                                   insertResult))
                        Return ActionResult.ExceptionError
                    End If

                    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
                Next

            End Using

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        Return ActionResult.Success

    End Function

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
    ''' <summary>
    ''' 作業実績テーブルに1行を登録する
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="inRsltStartDateTime">実績開始日時</param>
    ''' <param name="inInstructId">作業指示ID</param>
    ''' <param name="inInstructSeq">作業指示連番</param>
    ''' <param name="inSystemId">呼び出し元画面ID</param>
    ''' <returns>操作結果</returns>
    ''' <remarks></remarks>
    Private Function InsertOneJobResult(ByVal inJobDtlId As Decimal, _
                                        ByVal inStallId As Decimal, _
                                        ByVal inRsltStartDateTime As Date, _
                                        ByVal inInstructId As String, _
                                        ByVal inInstructSeq As Long, _
                                        ByVal inUpdateDate As Date, _
                                        ByVal inStaffAccount As String, _
                                        ByVal inSystemId As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inJobDtlId={1}, inStallId={2}, inRsltStartDateTime={3}, inInstructId={4}, inInstructSeq={5}, inUpdateDate={6}, inStaffAccount={7}, inSystemId={8}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inJobDtlId, _
                                  inStallId, _
                                  inRsltStartDateTime, _
                                  inInstructId, _
                                  inInstructSeq, _
                                  inUpdateDate, _
                                  inStaffAccount, _
                                  inSystemId))

        Using ta As New TabletSMBCommonClassDataAdapter
            '作業実績テーブルに1行しか登録しない
            Using jobResultDataTable As New TabletSmbCommonClassJobResultDataTable

                Dim jobResultRow As TabletSmbCommonClassJobResultRow = jobResultDataTable.NewTabletSmbCommonClassJobResultRow

                jobResultRow.JOB_RSLT_ID = ta.GetSequenceNextVal(JobRsltSeq)
                jobResultRow.JOB_DTL_ID = inJobDtlId
                jobResultRow.JOB_INSTRUCT_ID = inInstructId
                jobResultRow.JOB_INSTRUCT_SEQ = inInstructSeq
                jobResultRow.STALL_ID = inStallId
                jobResultRow.RSLT_START_DATETIME = inRsltStartDateTime
                jobResultRow.RSLT_END_DATETIME = Me.DefaultDateTimeValueGet()
                jobResultRow.JOB_STATUS = JobStatusWorking
                '1行をinsertする
                Dim insertCount As Long = ta.InsertJobResult(jobResultRow, _
                                                             inUpdateDate, _
                                                             inStaffAccount, _
                                                             inSystemId)

                '登録失敗の場合、予期せぬエラーを戻す
                If insertCount <> 1 Then

                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.End. ExceptedError:InsertJobResult(insertCount = {1}) .", _
                                               MethodBase.GetCurrentMethod.Name, _
                                               insertCount))
                    Return ActionResult.ExceptionError

                End If

            End Using

        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End", MethodBase.GetCurrentMethod.Name))

        Return ActionResult.Success

    End Function

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END
#End Region

#Region "開始処理--最初開始のチップであるか"
    ' ''' <summary>
    ' ''' チップ移動に対するチップ操作制約チェックをします
    ' ''' </summary>
    ' ''' <param name="targetServicein">ストール利用ID</param>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <param name="stallId">ストールID</param>
    ' ''' <param name="jobSvcClassId">作業内容．表示サービス分類コード</param>
    ' ''' <param name="mercId">商品ID</param>
    ' ''' <param name="rsltStartDateTime">移動情報</param>
    ' ''' <param name="dtNow">現在日時</param>
    ' ''' <param name="stallStartTime">営業開始時間</param>
    ' ''' <param name="stallEndTime">営業終了時間</param>
    ' ''' <remarks></remarks>
    'Private Function ValidateStart(ByVal targetServicein As Decimal, _
    '                         ByVal objStaffContext As StaffContext, _
    '                         ByVal stallId As Decimal, _
    '                         ByVal jobSvcClassId As Decimal, _
    '                         ByVal mercId As Decimal, _
    '                         ByVal rsltStartDateTime As Date, _
    '                         ByVal dtNow As Date, _
    '                         ByVal stallStartTime As Date, _
    '                         ByVal stallEndTime As Date, _
    '                         ByVal roJobSeq As Long) As Long

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. targetServicein={1}, stallId={2}, jobSvcClassId={3}, rsltStartDateTime={4}, dtNow={5}, stallStartTime={6}, stallEndTime={7}, roJobSeq={8}, mercId={9}" _
    '                    , MethodBase.GetCurrentMethod.Name, targetServicein, stallId, jobSvcClassId, rsltStartDateTime, dtNow, stallStartTime, stallEndTime, roJobSeq, mercId))


    '    '営業時間外
    '    If IsOutOfWorkingTime(rsltStartDateTime, stallStartTime, stallEndTime) Then
    '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E OutOfWorkingTimeError", MethodBase.GetCurrentMethod.Name))
    '        Return ActionResult.OutOfWorkingTimeError
    '    End If

    '    '予定開始日時に対する営業日を取得する
    '    Dim scheStartWorkingDate As Date = GetWorkingDate(dtNow, stallStartTime)
    '    '現在日時に対する営業日を取得する
    '    Dim nowWorkingDate As Date = GetWorkingDate(dtNow, stallStartTime)

    '    '予定開始日時に対する営業日が現在日時に対する営業日と異なる場合
    '    If scheStartWorkingDate <> nowWorkingDate Then
    '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E NotStartDayError", MethodBase.GetCurrentMethod.Name))
    '        Return ActionResult.NotStartDayError
    '    End If

    '    '処理対象の作業内容．表示サービス分類コードが未設定値の場合
    '    If IsDefaultValue(jobSvcClassId) And IsDefaultValue(mercId) Then
    '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E NotSetJobSvcClassIdError", MethodBase.GetCurrentMethod.Name))
    '        Return ActionResult.NotSetJobSvcClassIdError
    '    End If


    '    'ストール利用．ストール利用ステータスが「02：作業中」「04：作業計画の一部の作業が中断」の場合
    '    If Not CanStart(objStaffContext, targetServicein) Then
    '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
    '                , MethodBase.GetCurrentMethod.Name))
    '        Return ActionResult.CheckError
    '    End If

    '    Dim workingDateTimeData As List(Of Date) = GetStallDispDate(rsltStartDateTime, stallStartTime, stallEndTime)
    '    Dim startTime As Date = workingDateTimeData(0)

    '    '同一のストールに既に作業中のステータスが存在する場合
    '    If HasWorkingStatus(objStaffContext, stallId, startTime) Then
    '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E HasWorkingChipInOneStallError", MethodBase.GetCurrentMethod.Name))
    '        Return ActionResult.HasWorkingChipInOneStallError
    '    End If

    '    '親R/Oが作業開始されていないため、追加作業の作業は開始できない
    '    If roJobSeq > 0 Then
    '        If Not HasParentroStarted(targetServicein, jobDtlId) Then
    '            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
    '            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E ParentroNotStartedError", MethodBase.GetCurrentMethod.Name))
    '            Return ActionResult.ParentroNotStartedError
    '        End If

    '    End If

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    '    Return ActionResult.Success
    'End Function

    ''' <summary>
    ''' 最初開始のチップであるか
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <returns>最初開始のチップ場合<c>true</c>、最初開始のチップではない場合<c>false</c></returns>
    ''' <remarks></remarks>
    Public Function IsFirstStartChip(ByVal dlrCode As String, _
                                     ByVal brnCode As String, _
                                     ByVal svcinId As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dlrCode={1}, brnCode={2}, svcinId={3}" _
                                , MethodBase.GetCurrentMethod.Name, dlrCode, brnCode, svcinId))

        Dim rstlChipCountTable As TabletSmbCommonClassNumberValueDataTable

        Using ta As New TabletSMBCommonClassDataAdapter
            rstlChipCountTable = ta.IsExistRsltChip(dlrCode, brnCode, svcinId)
        End Using

        '実績チップがあれば、falseを戻す
        If rstlChipCountTable(0).COL1 > 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E return false.", MethodBase.GetCurrentMethod.Name))
            Return False
        Else
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E return true.", MethodBase.GetCurrentMethod.Name))
            Return True
        End If
    End Function

#End Region

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

#End Region

#End Region

#Region "作業完了処理"

    ''' <summary>
    ''' ストール利用を「作業完了」へ更新します
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="rsltEndDateTime">実績終了日時</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="updateDate">更新時間</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <param name="systemId">呼ぶ画面ID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' </history>
    Public Function Finish(ByVal stallUseId As Decimal, _
                           ByVal rsltEndDateTime As Date, _
                           ByVal restFlg As String, _
                           ByVal updateDate As Date, _
                           ByVal rowLockVersion As Long, _
                           ByVal systemId As String) As Long
        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''Public Function Finish(ByVal stallUseId As Decimal, _
        ''                   ByVal rsltEndDateTime As Date, _
        ''                   ByVal restFlg As String, _
        ''                   ByVal staffInfo As StaffContext, _
        ''                   ByVal stallStartTime As Date, _
        ''                   ByVal stallEndTime As Date, _
        ''                   ByVal updateDate As Date, _
        ''                   ByVal systemId As String) As Long
        ''Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, rsltEndDateTime={2}, stallStartTime={3}, stallEndTime={4}" _
        ''         , MethodBase.GetCurrentMethod.Name, stallUseId, rsltEndDateTime, stallStartTime, stallEndTime))
        'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, rsltEndDateTime={2}, restFlg={3}, rowLockVersion={4}, systemId={5}" _
        '                , MethodBase.GetCurrentMethod.Name, stallUseId, rsltEndDateTime, restFlg, rowLockVersion, systemId))
        ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        ''2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ' '' 実績終了日時の秒を切り捨てる
        ''Dim rsltEndDateTimeNoSec As Date = GetDateTimeFloorSecond(rsltEndDateTime)
        ''2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        ''***********************************************************************
        '' 1. いろいろな値を準備する
        ''***********************************************************************
        ''対象の情報を取得する
        'Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId, 1)
        'If dtChipEntity.Count <> 1 Then
        '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
        '                    , MethodBase.GetCurrentMethod.Name))
        '    Return ActionResult.GetChipEntityError
        'End If

        'Dim svcInId As Decimal = dtChipEntity(0).SVCIN_ID

        ''2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ''更新後のストール利用ステータス
        'Dim targetStalluseStatus As String = String.Empty

        ''Push送信フラグ初期化
        'Me.NeedPushAfterStopSingleJob = False
        'Me.NeedPushAfterFinishSingleJob = False
        ''2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        ' ''restFlg設定してない場合、1に設定する
        ''If IsNothing(restFlg) Then
        ''    restFlg = RestTimeGetFlgGetRest
        ''End If

        ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ' ''ステータス遷移可否をチェックする
        ''If Not CanFinish(dtChipEntity(0).STALL_USE_STATUS) Then
        ''    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
        ''                    , MethodBase.GetCurrentMethod.Name))
        ''    Return ActionResult.CheckError
        ''End If

        ' ''検査ステータスが1(検査依頼中)の場合、終了できない
        ''Dim inspectionStatus As String = dtChipEntity(0).INSPECTION_STATUS
        ''If inspectionStatus.Equals(InspectionApproval) Then
        ''    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E InspectionStatusFinishError" _
        ''                    , MethodBase.GetCurrentMethod.Name))
        ''    Return ActionResult.InspectionStatusFinishError
        ''End If

        ' ''RO NOを紐付けてるかチェックする
        ''Dim roNum As String = dtChipEntity(0).RO_NUM
        ''If String.IsNullOrEmpty(roNum.Trim()) Then
        ''    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0} NotSetroNoError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
        ''    Return ActionResult.NotSetroNoError
        ''End If

        'Dim staffInfo As StaffContext = StaffContext.Current

        ''2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        ' ''営業開始終了日時を取得する
        ''Dim dtBranchOperatingHours As TabletSmbCommonClassBranchOperatingHoursDataTable = _
        ''    Me.GetBranchOperatingHours(staffInfo.DlrCD, staffInfo.BrnCD)


        ''If dtBranchOperatingHours.Count = 0 Then
        ''    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E ExceptionError:GetBranchOperatingHours" _
        ''                                , MethodBase.GetCurrentMethod.Name))
        ''    Return ActionResult.ExceptionError
        ''End If
        ' ''営業開始終了日時を設定する
        ''Dim stallStartTime As Date = New Date(rsltEndDateTime.Year, rsltEndDateTime.Month, rsltEndDateTime.Day, _
        ''                                        dtBranchOperatingHours(0).SVC_JOB_START_TIME.Hour, dtBranchOperatingHours(0).SVC_JOB_START_TIME.Minute, 0)
        ''Dim stallEndTime As Date = New Date(rsltEndDateTime.Year, rsltEndDateTime.Month, rsltEndDateTime.Day, _
        ''                                        dtBranchOperatingHours(0).SVC_JOB_END_TIME.Hour, dtBranchOperatingHours(0).SVC_JOB_END_TIME.Minute, 0)


        ''実績終了日時を取得
        'Dim rsltEndDateTimeNoSec As Date = Me.CheckRsltEndDateTime(dtChipEntity(0).RSLT_START_DATETIME, _
        '                                                           rsltEndDateTime, _
        '                                                           staffInfo.DlrCD, _
        '                                                           staffInfo.BrnCD)

        ''営業開始と終了時間を取得する
        'Dim dtBranchOperatingHours As TabletSmbCommonClassBranchOperatingHoursDataTable = _
        '    Me.GetOneDayBrnOperatingHours(rsltEndDateTimeNoSec, _
        '                                  staffInfo.DlrCD, _
        '                                  staffInfo.BrnCD)

        ''Nothingの場合、予期せぬエラーを出す
        'If IsNothing(dtBranchOperatingHours) Then

        '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.End. ExceptionError:GetOneDayBrnOperatingHours" _
        '                  , MethodBase.GetCurrentMethod.Name))
        '    Return ActionResult.ExceptionError

        'End If

        ''営業開始日時を設定する
        'Dim stallStartTime As Date = dtBranchOperatingHours(0).SVC_JOB_START_TIME

        ''営業終了日時を設定する
        'Dim stallEndTime As Date = dtBranchOperatingHours(0).SVC_JOB_END_TIME
        ''2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        ''***********************************************************************
        '' 2. いろいろなチェックをする
        ''***********************************************************************
        'Dim rsltCheck As Long = Me.CheckFinishAction(dtChipEntity(0), _
        '                                            rsltEndDateTimeNoSec, _
        '                                            stallStartTime, _
        '                                            stallEndTime, _
        '                                            rowLockVersion, _
        '                                            restFlg, _
        '                                            staffInfo, _
        '                                            updateDate, _
        '                                            systemId)
        'If rsltCheck <> ActionResult.Success Then
        '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckFinishAction error: Error num is {1}" _
        '                                , MethodBase.GetCurrentMethod.Name _
        '                                , rsltCheck))
        '    Return rsltCheck
        'End If

        ''restFlg設定してない場合、1に設定する
        'If IsNothing(restFlg) Then
        '    restFlg = RestTimeGetFlgGetRest
        'End If

        ''作業実績送信使用するフラグを取得する
        'Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()
        ''作業実績送信の場合、作業ステータスを取得する
        'Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing
        'If isUseJobDispatch Then
        '    prevJobStatus = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)
        'End If

        ''更新前のステータス取得
        'Dim prevStatus As String = Me.JudgeChipStatus(stallUseId)

        ''***********************************************************************
        '' 3. DB更新
        ''***********************************************************************
        ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        ''update用データセット
        'Using targetDtChipEntity As New TabletSmbCommonClassChipEntityDataTable
        '    Dim targetDrChipEntity As TabletSmbCommonClassChipEntityRow = CType(targetDtChipEntity.NewRow(), TabletSmbCommonClassChipEntityRow)

        '    '処理対象のストール利用．ストール利用ステータスが「05：中断」の場合 'kari 中断状態で終了ボタンが表示かどうか知らない
        '    If dtChipEntity(0).STALL_USE_STATUS.Equals(StalluseStatusStop) Then
        '        targetDrChipEntity.STALL_USE_STATUS = StalluseStatusFinish
        '        targetDrChipEntity.UPDATE_DATETIME = updateDate
        '        targetDrChipEntity.UPDATE_STF_CD = staffInfo.Account
        '    Else


        '        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
        '        ''チップの作業を完了する
        '        'targetDrChipEntity = ChipFinish(dtChipEntity, rsltEndDateTimeNoSec, StalluseStatusFinish, restFlg, updateDate, _
        '        '                                    stallStartTime, stallEndTime, staffInfo.Account, updateDate, systemId)

        '        '更新用ストール利用ステータス、中断メモ、中断理由区分
        '        Dim targetStopMemo As String = String.Empty
        '        Dim targetStopReason As String = String.Empty

        '        If StalluseStatusStartIncludeStopJob.Equals(dtChipEntity(0).STALL_USE_STATUS) Then
        '            'ストール利用ステータスが「04:作業計画の一部の作業が中断」の場合

        '            '「05中断」に遷移する
        '            targetStalluseStatus = StalluseStatusStop

        '            '最大の作業実績IDの実績中断理由タイプ、中断メモはストール利用テーブルの中に設定する
        '            Using taTabletCommon As New TabletSMBCommonClassDataAdapter

        '                '該当Jobに紐づく全て作業実績を取得する(作業実績テーブルから)
        '                Dim dtJobInstructResult As TabletSmbCommonClassJobStatusDataTable = _
        '                    taTabletCommon.GetAllJobRsltInfoByJobDtlId(dtChipEntity(0).JOB_DTL_ID)

        '                '中断実績のデータを洗い出す
        '                Dim resultStop = (From p In dtJobInstructResult _
        '                                  Where p.JOB_STATUS = JobStatusStop _
        '                                  Order By p.JOB_RSLT_ID Descending _
        '                                  Select p).ToList()

        '                '中断実績の最終の中断メモ、中断理由区分をストール利用テーブルに設定する
        '                targetStopReason = resultStop(0).STOP_REASON_TYPE
        '                targetStopMemo = resultStop(0).STOP_MEMO

        '            End Using

        '        Else
        '            '他の場合

        '            '「03終了」に遷移する
        '            targetStalluseStatus = StalluseStatusFinish

        '        End If

        '        'スタッフ作業テーブルの実績終了日時を更新して、targetDrChipEntityに更新用のデータを登録する
        '        targetDrChipEntity = ChipFinish(dtChipEntity, _
        '                                        rsltEndDateTimeNoSec, _
        '                                        targetStalluseStatus, _
        '                                        restFlg, _
        '                                        updateDate, _
        '                                        stallStartTime, _
        '                                        stallEndTime, _
        '                                        staffInfo.Account, _
        '                                        updateDate, _
        '                                        systemId)

        '        '中断理由区分に値があれば
        '        If Not String.IsNullOrEmpty(targetStopReason) Then

        '            '更新用のテーブルに設定する
        '            targetDrChipEntity.STOP_REASON_TYPE = targetStopReason
        '            targetDrChipEntity.STOP_MEMO = targetStopMemo

        '        End If
        '        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        '    End If

        '    'ストール利用IDが最大かつ、ストール利用ステータスが
        '    '「03：完了」でないストール利用の取得件数が1件以上の場合
        '    Using ta As New TabletSMBCommonClassDataAdapter
        '        '全部完了してない場合、次の作業開始待ちにする
        '        If ta.GetBeforeFinishRelationChipCount(staffInfo.DlrCD, staffInfo.BrnCD, svcInId) > 1 Then
        '            targetDrChipEntity.SVC_STATUS = SvcStatusNextStartWait
        '        Else
        '            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '            ''検査待ち
        '            'targetDrChipEntity.SVC_STATUS = SvcStatusInspectionWait
        '            '次のサービスステータスを更新する(検査待ち、洗車待ち、納車待ち)
        '            targetDrChipEntity.SVC_STATUS = Me.GetNextSvcStatusByFinish(staffInfo, dtChipEntity(0))
        '            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        '        End If

        '        'サービス入庫ステータスが変更するかを判断して、サービス入庫テーブルを更新するかどうかを決める
        '        Dim bSetFlg As Boolean = True
        '        If targetDrChipEntity.SVC_STATUS.ToString().Equals(dtChipEntity(0).SVC_STATUS) Then
        '            bSetFlg = False
        '        End If

        '        targetDrChipEntity.SVCIN_ID = svcInId
        '        targetDrChipEntity.STALL_USE_ID = stallUseId
        '        targetDrChipEntity.REST_FLG = restFlg

        '        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

        '        ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発  START
        '        ''作業実績テーブルを更新する
        '        'Dim updateCount As Long = ta.UpdateJobRsltOnFinish(dtChipEntity(0).JOB_DTL_ID, _
        '        '                                                    rsltEndDateTimeNoSec, _
        '        '                                                    JobStatusFinish, _
        '        '                                                    staffInfo.Account, _
        '        '                                                    systemId, _
        '        '                                                    updateDate)
        '        'If updateCount = 0 Then

        '        '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
        '        '                                "{0}.E ExceptionError:UpdateJobRsltOnFinish update count=0.", _
        '        '                                MethodBase.GetCurrentMethod.Name))
        '        '    Return ActionResult.ExceptionError

        '        'End If
        '        ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発  END

        '        '作業実績テーブルを更新する
        '        '0行の可能性がある：
        '        '詳細画面で1つ未開始のJobを外して、ほかのJobが全部終了の場合、終了になる
        '        ta.UpdateJobRsltOnFinish(dtChipEntity(0).JOB_DTL_ID, _
        '                                 rsltEndDateTimeNoSec, _
        '                                 JobStatusFinish, _
        '                                 staffInfo.Account, _
        '                                 systemId, _
        '                                 updateDate)
        '        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        '        'サービス入庫テーブルの更新が必要の場合
        '        Dim rtCnt As Long = 0
        '        If bSetFlg Then
        '            rtCnt = ta.UpdateSvcinStatus(targetDrChipEntity.SVCIN_ID, targetDrChipEntity.SVC_STATUS, targetDrChipEntity.UPDATE_DATETIME, targetDrChipEntity.UPDATE_STF_CD)
        '            If rtCnt <> 1 Then
        '                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Failed to update TB_T_SERVICEIN. SVCIN_ID={1}, SVC_STATUS={2}" _
        '                    , MethodBase.GetCurrentMethod.Name, targetDrChipEntity.SVCIN_ID, targetDrChipEntity.SVC_STATUS))
        '                Return ActionResult.ExceptionError
        '            End If
        '        End If

        '        'ストール利用テーブルを更新する
        '        rtCnt = ta.UpdateStallUseRsltEndDate(targetDrChipEntity, systemId)
        '        If rtCnt <= 0 Then
        '            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Failed to update TB_T_STALL_USE. STALL_USE_ID={1}, STALL_USE_STATUS={2}" _
        '                , MethodBase.GetCurrentMethod.Name, targetDrChipEntity.STALL_USE_ID, targetDrChipEntity.STALL_USE_STATUS))
        '            Return ActionResult.ExceptionError
        '        End If
        '    End Using
        'End Using

        ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

        ''2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
        ''ROステータスを変える
        ''Me.FinishRoStatus(svcInId, _
        ''                  dtChipEntity(0).JOB_DTL_ID, _
        ''                  dtChipEntity(0).INSPECTION_NEED_FLG, _
        ''                  dtChipEntity(0).INSPECTION_STATUS, _
        ''                  dtChipEntity(0).RO_NUM, _
        ''                  updateDate, _
        ''                  staffInfo.Account, _
        ''                  systemId)

        'Me.FinishRoStatus(svcInId, _
        '                  dtChipEntity(0).JOB_DTL_ID, _
        '                  dtChipEntity(0).RO_NUM, _
        '                  updateDate, _
        '                  staffInfo.Account, _
        '                  systemId)
        ''2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

        ''***********************************************************************
        '' 4. 基幹連携
        ''***********************************************************************

        ''更新後のステータス取得
        'Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)

        ''基幹側にステータス情報を送信
        'Using ic3802601blc As New IC3802601BusinessLogic
        '    Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(svcInId, _
        '                                                            dtChipEntity(0).JOB_DTL_ID, _
        '                                                            stallUseId, _
        '                                                            prevStatus, _
        '                                                            crntStatus, _
        '                                                            0)
        '    If dmsSendResult <> 0 Then
        '        Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
        '                                    , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
        '        Return ActionResult.DmsLinkageError
        '    End If
        'End Using


        ''実績送信使用の場合
        'If isUseJobDispatch Then

        '    '作業ステータスを取得する
        '    Dim crntJobStatus As IC3802701JobStatusDataTable = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)

        '    '基幹側にJobDispatch実績情報を送信
        '    Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(svcInId, _
        '                                                           dtChipEntity(0).JOB_DTL_ID, _
        '                                                           prevJobStatus, _
        '                                                           crntJobStatus)
        '    If resultSendJobClock <> ActionResult.Success Then
        '        Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.End DmsLinkageError:SendJobClockOnInfo FAILURE " _
        '                                    , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
        '        Return ActionResult.DmsLinkageError
        '    End If

        'End If

        ''2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

        ''All Finish後、Pushフラグを設定
        'Me.SetAllFinishPushFlg(targetStalluseStatus)

        ''2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        '' 正常終了
        'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End", MethodBase.GetCurrentMethod.Name))
        'Return ActionResult.Success

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} START stallUseId={2}, rsltEndDateTime={3}, restFlg={4}, updateDate={5}, rowLockVersion={6}, systemId={7} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  stallUseId.ToString(CultureInfo.CurrentCulture), _
                                  rsltEndDateTime.ToString(CultureInfo.CurrentCulture), _
                                  restFlg, _
                                  updateDate.ToString(CultureInfo.CurrentCulture), _
                                  rowLockVersion.ToString(CultureInfo.CurrentCulture), _
                                  systemId))

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '***********************************************************************
        ' 1. いろいろな値を準備する
        '***********************************************************************
        'エンティティ情報を取得する
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId, 1)

        '取得チェック
        If dtChipEntity.Count <> 1 Then
            '取得できなかった場合
            '「11：チップエンティティの取得エラー」を返却
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END RETURNCODE={2}[GetChipEntity FAILURE]" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , ActionResult.GetChipEntityError))
            Return ActionResult.GetChipEntityError

        End If

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        ''エンティティ情報からサービス入庫IDを取得する
        'Dim svcInId As Decimal = dtChipEntity(0).SVCIN_ID

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        '更新後のストール利用ステータス
        Dim targetStalluseStatus As String = String.Empty

        'Push送信フラグ初期化
        Me.NeedPushAfterStopSingleJob = False
        Me.NeedPushAfterFinishSingleJob = False
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 START
        Me.NeedPushSubAreaRefresh = False
        '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

        'ログイン情報を取得
        Dim staffInfo As StaffContext = StaffContext.Current

        '実績終了日時を取得
        Dim rsltEndDateTimeNoSec As Date = Me.CheckRsltEndDateTime(dtChipEntity(0).RSLT_START_DATETIME, _
                                                                   rsltEndDateTime, _
                                                                   staffInfo.DlrCD, _
                                                                   staffInfo.BrnCD)

        '営業開始時間と営業終了時間を取得する
        Dim dtBranchOperatingHours As TabletSmbCommonClassBranchOperatingHoursDataTable = _
            Me.GetOneDayBrnOperatingHours(rsltEndDateTimeNoSec, _
                                          staffInfo.DlrCD, _
                                          staffInfo.BrnCD)

        '営業開始時間と営業終了時間のチェック
        If IsNothing(dtBranchOperatingHours) Then
            '取得できなかった場合
            '「22：予期せぬエラー」を返却
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END RETURNCODE={2}[GetOneDayBrnOperatingHours FAILURE]" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , ActionResult.ExceptionError))
            Return ActionResult.ExceptionError

        End If

        '営業開始日時を設定する
        Dim stallStartTime As Date = dtBranchOperatingHours(0).SVC_JOB_START_TIME

        '営業終了日時を設定する
        Dim stallEndTime As Date = dtBranchOperatingHours(0).SVC_JOB_END_TIME

        '***********************************************************************
        ' 2. いろいろなチェックをする
        '***********************************************************************
        '終了操作のチェックをする
        Dim rsltCheck As Long = Me.CheckFinishAction(dtChipEntity(0), _
                                                    rsltEndDateTimeNoSec, _
                                                    stallStartTime, _
                                                    stallEndTime, _
                                                    rowLockVersion, _
                                                    restFlg, _
                                                    staffInfo, _
                                                    updateDate, _
                                                    systemId)

        '処理結果をチェック
        If rsltCheck <> ActionResult.Success Then
            '終了操作ができない場合
            '終了操作の結果を返却
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END RETURNCODE={2}[CheckFinishAction FAILURE]" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , rsltCheck))
            Return rsltCheck

        End If

        '休憩フラグのチェック
        If IsNothing(restFlg) Then
            '存在しない場合
            '「1：休憩する」を設定
            restFlg = RestTimeGetFlgGetRest
        End If

        '作業実績送信使用するフラグを取得する
        Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()

        '作業実績送信の場合、作業ステータスを取得する
        Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing

        '作業実績送信使用フラグのチェック
        If isUseJobDispatch Then
            '「1：使用する」の場合
            '作業ステータスの設定
            prevJobStatus = Me.JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)

        End If

        '更新前のステータス取得
        Dim prevStatus As String = Me.JudgeChipStatus(stallUseId)

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        ''***********************************************************************
        '' 3. DB更新
        ''***********************************************************************
        'Using targetDtChipEntity As New TabletSmbCommonClassChipEntityDataTable
        '    '更新後エンティティ情報
        '    Dim targetDrChipEntity As TabletSmbCommonClassChipEntityRow = _
        '        CType(targetDtChipEntity.NewRow(), TabletSmbCommonClassChipEntityRow)

        '    'ストール利用ステータスのチェック
        '    If dtChipEntity(0).STALL_USE_STATUS.Equals(StalluseStatusStop) Then
        '        '「05：中断」の場合
        '        ''kari 中断状態で終了ボタンが表示かどうか知らない
        '        targetDrChipEntity.STALL_USE_STATUS = StalluseStatusFinish
        '        targetDrChipEntity.UPDATE_DATETIME = updateDate
        '        targetDrChipEntity.UPDATE_STF_CD = staffInfo.Account

        '    Else
        '        '上記以外の場合
        '        '更新用ストール利用ステータス、中断メモ、中断理由区分
        '        Dim targetStopMemo As String = String.Empty
        '        Dim targetStopReason As String = String.Empty

        '        'ストール利用ステータスのチェック
        '        If StalluseStatusStartIncludeStopJob.Equals(dtChipEntity(0).STALL_USE_STATUS) Then
        '            '「04：作業計画の一部の作業が中断」の場合
        '            '更新後ストール利用ステータスに「05：中断」を設定
        '            targetStalluseStatus = StalluseStatusStop

        '            Using taTabletCommon As New TabletSMBCommonClassDataAdapter

        '                '該当Jobに紐づく全て作業実績を取得する(作業実績テーブルから)
        '                Dim dtJobInstructResult As TabletSmbCommonClassJobStatusDataTable = _
        '                    taTabletCommon.GetAllJobRsltInfoByJobDtlId(dtChipEntity(0).JOB_DTL_ID)

        '                '中断実績のデータを抽出する
        '                Dim resultStop = (From p In dtJobInstructResult _
        '                                  Where p.JOB_STATUS = JobStatusStop _
        '                                  Order By p.JOB_RSLT_ID Descending _
        '                                  Select p).ToList()

        '                '中断実績の最終の中断メモ、中断理由区分をストール利用テーブルに設定する
        '                targetStopReason = resultStop(0).STOP_REASON_TYPE
        '                targetStopMemo = resultStop(0).STOP_MEMO

        '            End Using

        '        Else
        '            '上記以外の場合
        '            '更新後ストール利用ステータスに「03：完了」を設定
        '            targetStalluseStatus = StalluseStatusFinish

        '        End If

        '        'スタッフ作業テーブルの実績終了日時を更新して、targetDrChipEntityに更新用のデータを登録する
        '        targetDrChipEntity = Me.ChipFinish(dtChipEntity, _
        '                                           rsltEndDateTimeNoSec, _
        '                                           targetStalluseStatus, _
        '                                           restFlg, _
        '                                           updateDate, _
        '                                           stallStartTime, _
        '                                           stallEndTime, _
        '                                           staffInfo.Account, _
        '                                           updateDate, _
        '                                           systemId)

        '        '中断理由区分に値があれば
        '        If Not String.IsNullOrEmpty(targetStopReason) Then

        '            '更新用のテーブルに設定する
        '            targetDrChipEntity.STOP_REASON_TYPE = targetStopReason
        '            targetDrChipEntity.STOP_MEMO = targetStopMemo

        '        End If

        '    End If

        '    'ストール利用IDが最大かつ、ストール利用ステータスが
        '    '「03：完了」でないストール利用の取得件数が1件以上の場合
        '    Using ta As New TabletSMBCommonClassDataAdapter
        '        '全部完了してない場合、次の作業開始待ちにする
        '        '全作業終了のチェック
        '        If ta.GetBeforeFinishRelationChipCount(staffInfo.DlrCD, staffInfo.BrnCD, svcInId) > 1 Then
        '            '終了していない場合
        '            '更新後サービスステータスに「06：次の作業開始待ち」を設定
        '            targetDrChipEntity.SVC_STATUS = SvcStatusNextStartWait

        '        Else
        '            '終了している場合
        '            '更新後サービスステータスを取得して設定する「09：検査待ち、07：洗車待ち、11:預かり中、12:納車待ち」
        '            targetDrChipEntity.SVC_STATUS = Me.GetNextSvcStatusByFinish(staffInfo, dtChipEntity(0))

        '        End If

        '        'サービスステータスの変更有無フラグ「True：変更有」
        '        Dim bSetFlg As Boolean = True

        '        '更新前と更新後のサービスステータスをチェック
        '        If targetDrChipEntity.SVC_STATUS.ToString().Equals(dtChipEntity(0).SVC_STATUS) Then
        '            '変更がない場合
        '            'サービスステータスの変更有無フラグに「False：変更無」を設定
        '            bSetFlg = False

        '        End If

        '        '更新後エンティティにサービス入庫IDとストール利用IDと休憩フラグを設定する
        '        targetDrChipEntity.SVCIN_ID = svcInId
        '        targetDrChipEntity.STALL_USE_ID = stallUseId
        '        targetDrChipEntity.REST_FLG = restFlg

        '        '作業実績テーブルを更新する
        '        '0行の可能性がある：
        '        '詳細画面で1つ未開始のJobを外して、ほかのJobが全部終了の場合、終了になる
        '        ta.UpdateJobRsltOnFinish(dtChipEntity(0).JOB_DTL_ID, _
        '                                 rsltEndDateTimeNoSec, _
        '                                 JobStatusFinish, _
        '                                 staffInfo.Account, _
        '                                 systemId, _
        '                                 updateDate)

        '        'ストール利用テーブルを更新する
        '        Dim countUpdateStallUse = ta.UpdateStallUseRsltEndDate(targetDrChipEntity, _
        '                                                               systemId)

        '        '処理結果チェック
        '        If countUpdateStallUse <= 0 Then
        '            '更新失敗の場合
        '            '「22：予期せぬエラー」を返却
        '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} END RETURNCODE={2}[UpdateStallUseRsltEndDate FAILURE]" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , ActionResult.ExceptionError))
        '            Return ActionResult.ExceptionError

        '        End If

        '        Using bizServiceCommonClass As New ServiceCommonClassBusinessLogic
        '            'サービスDMS納車実績ワークを取得
        '            Dim dtWorkServiceDmsResultDelivery As ServiceCommonClassDataSet.WorkServiceDmsResultDeliveryDataTable = _
        '                bizServiceCommonClass.GetWorkServiceDmsResultDelivery(dtChipEntity(0).SVCIN_ID)

        '            '取得結果と更新後サービスステータスのチェック
        '            If Not (IsNothing(dtWorkServiceDmsResultDelivery)) AndAlso 0 < dtWorkServiceDmsResultDelivery.Count AndAlso _
        '               (SvcStatusDropOffCustomer.Equals(targetDrChipEntity.SVC_STATUS) OrElse _
        '                SvcStatusWaitingCustomer.Equals(targetDrChipEntity.SVC_STATUS)) Then
        '                '取得できた場合且つ、サービスステータスが「11：預かり中 or 12：納車待ち」の場合
        '                '強制納車処理を実施
        '                Dim returnCodeForceDeliverd As Integer = Me.ForceDeliverd(dtChipEntity(0).DLR_CD,
        '                                                                          dtChipEntity(0).BRN_CD, _
        '                                                                          dtChipEntity(0).SVCIN_ID, _
        '                                                                          dtChipEntity(0).RO_NUM, _
        '                                                                          dtWorkServiceDmsResultDelivery(0).DMS_RSLT_DELI_DATETIME, _
        '                                                                          staffInfo.Account, _
        '                                                                          updateDate, _
        '                                                                          systemId)

        '                '処理結果チェック
        '                If returnCodeForceDeliverd <> ActionResult.Success Then
        '                    '「0：成功」以外の場合
        '                    '戻り値に「-9000：DMS除外エラーの警告」を設定
        '                    returnCode = ActionResult.WarningOmitDmsError
        '                    '「22：予期せぬエラー」を返却
        '                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                        , "{0}.{1} END RETURNCODE={2}[ForceDeliverd FAILURE]" _
        '                        , Me.GetType.ToString _
        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                        , ActionResult.ExceptionError))
        '                    Return ActionResult.ExceptionError

        '                End If

        '            Else
        '                '上記以外の場合
        '                '作業完了処理を実施

        '                'サービスステータスの変更有無フラグのチェック
        '                If bSetFlg Then
        '                    '「True：変更有」の場合
        '                    'サービス入庫ステータスを更新する
        '                    Dim countUpdateSvcin As Long = ta.UpdateSvcinStatus(targetDrChipEntity.SVCIN_ID, _
        '                                                                        targetDrChipEntity.SVC_STATUS, _
        '                                                                        targetDrChipEntity.UPDATE_DATETIME, _
        '                                                                        targetDrChipEntity.UPDATE_STF_CD)

        '                    '処理結果チェック
        '                    If countUpdateSvcin <> 1 Then
        '                        '更新失敗の場合
        '                        '「22：予期せぬエラー」を返却
        '                        Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                            , "{0}.{1} END RETURNCODE={2}[UpdateSvcinStatus FAILURE]" _
        '                            , Me.GetType.ToString _
        '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                            , ActionResult.ExceptionError))
        '                        Return ActionResult.ExceptionError

        '                    End If

        '                End If

        '                'ROステータスを変える
        '                Me.FinishRoStatus(svcInId, _
        '                                  dtChipEntity(0).JOB_DTL_ID, _
        '                                  dtChipEntity(0).RO_NUM, _
        '                                  updateDate, _
        '                                  staffInfo.Account, _
        '                                  systemId)

        '                '***********************************************************************
        '                ' 4. 基幹連携
        '                '***********************************************************************

        '                'ステータス連携で送信する更新後のROステータスを取得する
        '                Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)

        '                '基幹側にステータス情報を送信
        '                Using ic3802601blc As New IC3802601BusinessLogic
        '                    'ステータス連携実施
        '                    Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(svcInId, _
        '                                                                            dtChipEntity(0).JOB_DTL_ID, _
        '                                                                            stallUseId, _
        '                                                                            prevStatus, _
        '                                                                            crntStatus, _
        '                                                                            0)

        '                    '処理結果チェック
        '                    If dmsSendResult = ActionResult.Success Then
        '                        '「0：成功」の場合
        '                        '処理なし

        '                    ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
        '                        '「-9000：DMS除外エラーの警告」の場合
        '                        '戻り値に「-9000：DMS除外エラーの警告」を設定
        '                        returnCode = ActionResult.WarningOmitDmsError

        '                    Else
        '                        '上記以外の場合
        '                        '「15：他システムとの連携エラー」を返却
        '                        Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                            , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
        '                            , Me.GetType.ToString _
        '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                            , ActionResult.DmsLinkageError))
        '                        Return ActionResult.DmsLinkageError

        '                    End If

        '                End Using

        '                '実績送信使用の場合
        '                If isUseJobDispatch Then

        '                    'JobDispatch連携で送信する更新後の作業ステータスを取得する
        '                    Dim crntJobStatus As IC3802701JobStatusDataTable = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)

        '                    'JobDispatch連携を実施
        '                    Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(svcInId, _
        '                                                                           dtChipEntity(0).JOB_DTL_ID, _
        '                                                                           prevJobStatus, _
        '                                                                           crntJobStatus)

        '                    '処理結果チェック
        '                    If resultSendJobClock = ActionResult.Success Then
        '                        '「0：成功」の場合
        '                        '処理なし

        '                    ElseIf resultSendJobClock = ActionResult.WarningOmitDmsError Then
        '                        '「-9000：DMS除外エラーの警告」の場合
        '                        '戻り値に「-9000：DMS除外エラーの警告」を設定
        '                        returnCode = ActionResult.WarningOmitDmsError

        '                    Else
        '                        '上記以外の場合
        '                        '「15：他システムとの連携エラー」を返却
        '                        Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                            , "{0}.{1} END RETURNCODE={2}[SendJobClockOnInfo FAILURE]" _
        '                            , Me.GetType.ToString _
        '                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                            , ActionResult.DmsLinkageError))
        '                        Return ActionResult.DmsLinkageError

        '                    End If

        '                End If

        '            End If

        '        End Using

        '    End Using

        'End Using

        '作業完了のDB処理実施
        Dim resultFinishDB As Long = Me.FinishDB(stallUseId, _
                                                 restFlg, _
                                                 targetStalluseStatus, _
                                                 rsltEndDateTimeNoSec, _
                                                 stallStartTime, _
                                                 stallEndTime, _
                                                 prevStatus, _
                                                 updateDate, _
                                                 systemId, _
                                                 staffInfo, _
                                                 dtChipEntity, _
                                                 prevJobStatus)

        '処理結果チェック
        If resultFinishDB = ActionResult.Success Then
            '「0：成功」の場合
            '処理なし

        ElseIf resultFinishDB = ActionResult.WarningOmitDmsError Then
            '「-9000：DMS除外エラーの警告」の場合
            '戻り値に「-9000：DMS除外エラーの警告」を設定
            returnCode = ActionResult.WarningOmitDmsError

        Else
            '上記以外の場合
            '処理結果を返却
            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END RETURNCODE={2}[FinishDB FAILURE]" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , resultFinishDB))
            Return resultFinishDB

        End If

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        'All Finish後、Pushフラグを設定
        Me.SetAllFinishPushFlg(targetStalluseStatus)

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} END RETURNCODE={2} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  returnCode))
        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

    ''' <summary>
    ''' 作業完了のDB処理
    ''' </summary>
    ''' <param name="inStallUseId">作業内容ID</param>
    ''' <param name="inRestFlg">休憩フラグ</param>
    ''' <param name="inTargetStalluseStatus"></param>
    ''' <param name="inRsltEndDateTimeNoSec"></param>
    ''' <param name="inStallStartTime"></param>
    ''' <param name="inStallEndTime"></param>
    ''' <param name="inPrevStatus"></param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inSystemId">プログラムID</param>
    ''' <param name="inStaffInfo">ログイン情報</param>
    ''' <param name="dtChipEntity">更新前チップ情報</param>
    ''' <param name="dtPrevJobStatus"></param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function FinishDB(ByVal inStallUseId As Decimal, _
                              ByVal inRestFlg As String, _
                              ByRef inTargetStalluseStatus As String, _
                              ByVal inRsltEndDateTimeNoSec As Date, _
                              ByVal inStallStartTime As Date, _
                              ByVal inStallEndTime As Date, _
                              ByVal inPrevStatus As String, _
                              ByVal inUpdateDate As Date, _
                              ByVal inSystemId As String, _
                              ByVal inStaffInfo As StaffContext, _
                              ByVal dtChipEntity As TabletSmbCommonClassChipEntityDataTable, _
                              ByVal dtPrevJobStatus As IC3802701JobStatusDataTable) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START,inStallUseId={2},inRestFlg={3},inTargetStalluseStatus={4},inRsltEndDateTimeNoSec={5},inStallStartTime={6},inStallEndTime={7},inPrevStatus={8},inUpdateDate={9},inSystemId={10}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inStallUseId.ToString(CultureInfo.CurrentCulture) _
            , inRestFlg _
            , inTargetStalluseStatus _
            , inRsltEndDateTimeNoSec.ToString(CultureInfo.CurrentCulture) _
            , inStallStartTime.ToString(CultureInfo.CurrentCulture) _
            , inStallEndTime.ToString(CultureInfo.CurrentCulture) _
            , inPrevStatus _
            , inUpdateDate.ToString(CultureInfo.CurrentCulture) _
            , inSystemId))

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '***********************************************************************
        ' DB更新
        '***********************************************************************
        Using targetDtChipEntity As New TabletSmbCommonClassChipEntityDataTable
            '更新後チップ情報の取得
            Dim targetDrChipEntity As TabletSmbCommonClassChipEntityRow = _
                CType(targetDtChipEntity.NewRow(), TabletSmbCommonClassChipEntityRow)

            '更新前のストール利用ステータスのチェック
            If dtChipEntity(0).STALL_USE_STATUS.Equals(StalluseStatusStop) Then
                '「05：中断」の場合
                ''kari 中断状態で終了ボタンが表示かどうか知らない
                targetDrChipEntity.STALL_USE_STATUS = StalluseStatusFinish
                targetDrChipEntity.UPDATE_DATETIME = inUpdateDate
                targetDrChipEntity.UPDATE_STF_CD = inStaffInfo.Account

            Else
                '上記以外の場合
                '更新用ストール利用ステータス、中断メモ、中断理由区分
                Dim targetStopMemo As String = String.Empty
                Dim targetStopReason As String = String.Empty

                '更新前のストール利用ステータスのチェック
                If StalluseStatusStartIncludeStopJob.Equals(dtChipEntity(0).STALL_USE_STATUS) Then
                    '「04：作業計画の一部の作業が中断」の場合
                    '更新後ストール利用ステータスに「05：中断」を設定
                    inTargetStalluseStatus = StalluseStatusStop

                    Using taTabletCommon As New TabletSMBCommonClassDataAdapter

                        '該当Jobに紐づく全て作業実績を取得する(作業実績テーブルから)
                        Dim dtJobInstructResult As TabletSmbCommonClassJobStatusDataTable = _
                            taTabletCommon.GetAllJobRsltInfoByJobDtlId(dtChipEntity(0).JOB_DTL_ID)

                        '中断実績のデータを抽出する
                        Dim resultStop = (From p In dtJobInstructResult _
                                          Where p.JOB_STATUS = JobStatusStop _
                                          Order By p.JOB_RSLT_ID Descending _
                                          Select p).ToList()

                        '中断実績の最終の中断メモ、中断理由区分をストール利用テーブルに設定する
                        targetStopReason = resultStop(0).STOP_REASON_TYPE
                        targetStopMemo = resultStop(0).STOP_MEMO

                    End Using

                Else
                    '上記以外の場合
                    '更新後ストール利用ステータスに「03：完了」を設定
                    inTargetStalluseStatus = StalluseStatusFinish

                End If

                'スタッフ作業テーブルの実績終了日時を更新して、targetDrChipEntityに更新用のデータを登録する
                targetDrChipEntity = Me.ChipFinish(dtChipEntity, _
                                                   inRsltEndDateTimeNoSec, _
                                                   inTargetStalluseStatus, _
                                                   inRestFlg, _
                                                   inUpdateDate, _
                                                   inStallStartTime, _
                                                   inStallEndTime, _
                                                   inStaffInfo.Account, _
                                                   inUpdateDate, _
                                                   inSystemId)

                '中断理由区分に値があれば
                If Not String.IsNullOrEmpty(targetStopReason) Then

                    '更新用のテーブルに設定する
                    targetDrChipEntity.STOP_REASON_TYPE = targetStopReason
                    targetDrChipEntity.STOP_MEMO = targetStopMemo

                End If

            End If

            'ストール利用IDが最大かつ、ストール利用ステータスが
            '「03：完了」でないストール利用の取得件数が1件以上の場合
            Using ta As New TabletSMBCommonClassDataAdapter
                '全部完了してない場合、次の作業開始待ちにする
                '全作業終了のチェック
                If ta.GetBeforeFinishRelationChipCount(inStaffInfo.DlrCD, inStaffInfo.BrnCD, dtChipEntity(0).SVCIN_ID) > 1 Then
                    '終了していない場合
                    '更新後サービスステータスに「06：次の作業開始待ち」を設定
                    targetDrChipEntity.SVC_STATUS = SvcStatusNextStartWait

                Else
                    '終了している場合
                    '更新後サービスステータスを取得して設定する「09：検査待ち、07：洗車待ち、11:預かり中、12:納車待ち」
                    targetDrChipEntity.SVC_STATUS = Me.GetNextSvcStatusByFinish(inStaffInfo, dtChipEntity(0))

                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
                    ' 最後のチップが作業完了の場合は、全CT/CHTにPush送信するため、サブエリアリフレッシュフラグにTrueを設定する
                    Me.NeedPushSubAreaRefresh = True
                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
                End If

                'サービスステータスの変更有無フラグ「True：変更有」
                Dim bSetFlg As Boolean = True

                '更新前と更新後のサービスステータスをチェック
                If targetDrChipEntity.SVC_STATUS.ToString().Equals(dtChipEntity(0).SVC_STATUS) Then
                    '変更がない場合
                    'サービスステータスの変更有無フラグに「False：変更無」を設定
                    bSetFlg = False

                End If

                '更新後エンティティにサービス入庫IDとストール利用IDと休憩フラグを設定する
                targetDrChipEntity.SVCIN_ID = dtChipEntity(0).SVCIN_ID
                targetDrChipEntity.STALL_USE_ID = inStallUseId
                targetDrChipEntity.REST_FLG = inRestFlg

                '作業実績テーブルを更新する
                '0行の可能性がある：
                '詳細画面で1つ未開始のJobを外して、ほかのJobが全部終了の場合、終了になる
                ta.UpdateJobRsltOnFinish(dtChipEntity(0).JOB_DTL_ID, _
                                         inRsltEndDateTimeNoSec, _
                                         JobStatusFinish, _
                                         inStaffInfo.Account, _
                                         inSystemId, _
                                         inUpdateDate)

                ''2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                ''ストール利用テーブルを更新する
                'Dim countUpdateStallUse As Long = ta.UpdateStallUseRsltEndDate(targetDrChipEntity, _
                '                                                               inSystemId)

                Dim countUpdateStallUse As Long

                Using dealerEnvBiz As New ServiceCommonClassBusinessLogic
                    '休憩取得自動判定フラグ
                    Dim autoJudgeFlg = String.Empty
                    autoJudgeFlg = dealerEnvBiz.GetDlrSystemSettingValueBySettingName(RestAutoJudgeFlg)

                'ストール利用テーブルを更新する
                    countUpdateStallUse = ta.UpdateStallUseRsltEndDate(targetDrChipEntity, inSystemId, autoJudgeFlg)
                End Using
                ''2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                '処理結果チェック
                If countUpdateStallUse <= 0 Then
                    '更新失敗の場合
                    '「22：予期せぬエラー」を返却
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[UpdateStallUseRsltEndDate FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.ExceptionError))
                    Return ActionResult.ExceptionError

                End If

                Using bizServiceCommonClass As New ServiceCommonClassBusinessLogic
                    'サービスDMS納車実績ワークを取得
                    Dim dtWorkServiceDmsResultDelivery As ServiceCommonClassDataSet.WorkServiceDmsResultDeliveryDataTable = _
                        bizServiceCommonClass.GetWorkServiceDmsResultDelivery(dtChipEntity(0).SVCIN_ID)

                    '取得結果と更新後サービスステータスのチェック
                    If Not (IsNothing(dtWorkServiceDmsResultDelivery)) AndAlso 0 < dtWorkServiceDmsResultDelivery.Count AndAlso _
                       (SvcStatusDropOffCustomer.Equals(targetDrChipEntity.SVC_STATUS) OrElse _
                        SvcStatusWaitingCustomer.Equals(targetDrChipEntity.SVC_STATUS)) Then
                        '取得できた場合且つ、サービスステータスが「11：預かり中 or 12：納車待ち」の場合
                        '強制納車処理を実施
                        Dim returnCodeForceDeliverd As Integer = Me.ForceDeliverd(dtChipEntity(0).DLR_CD,
                                                                                  dtChipEntity(0).BRN_CD, _
                                                                                  dtChipEntity(0).SVCIN_ID, _
                                                                                  dtChipEntity(0).RO_NUM, _
                                                                                  dtWorkServiceDmsResultDelivery(0).DMS_RSLT_DELI_DATETIME, _
                                                                                  inStaffInfo.Account, _
                                                                                  inUpdateDate, _
                                                                                  inSystemId)

                        '処理結果チェック
                        If returnCodeForceDeliverd <> ActionResult.Success Then
                            '「0：成功」以外の場合
                            '戻り値に「-9000：DMS除外エラーの警告」を設定
                            returnCode = ActionResult.WarningOmitDmsError
                            '「22：予期せぬエラー」を返却
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END RETURNCODE={2}[ForceDeliverd FAILURE]" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ActionResult.ExceptionError))
                            Return ActionResult.ExceptionError

                        End If

                    Else
                        '上記以外の場合
                        '作業完了処理を実施

                        'サービスステータスの変更有無フラグのチェック
                        If bSetFlg Then
                            '「True：変更有」の場合
                            'サービス入庫ステータスを更新する
                            Dim countUpdateSvcin As Long = ta.UpdateSvcinStatus(targetDrChipEntity.SVCIN_ID, _
                                                                                targetDrChipEntity.SVC_STATUS, _
                                                                                targetDrChipEntity.UPDATE_DATETIME, _
                                                                                targetDrChipEntity.UPDATE_STF_CD)

                            '処理結果チェック
                            If countUpdateSvcin <> 1 Then
                                '更新失敗の場合
                                '「22：予期せぬエラー」を返却
                                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} END RETURNCODE={2}[UpdateSvcinStatus FAILURE]" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , ActionResult.ExceptionError))
                                Return ActionResult.ExceptionError

                            End If

                        End If

                        'ROステータスを変える
                        Me.FinishRoStatus(dtChipEntity(0).SVCIN_ID, _
                                          dtChipEntity(0).JOB_DTL_ID, _
                                          dtChipEntity(0).RO_NUM, _
                                          inUpdateDate, _
                                          inStaffInfo.Account, _
                                          inSystemId)

                        '作業完了のWebService処理
                        Dim resultFinishWebService As Long = Me.FinishWebService(dtChipEntity, _
                                                                                 inStallUseId, _
                                                                                 inPrevStatus, _
                                                                                 dtPrevJobStatus, _
                                                                                 IsUseJobDispatch)

                        '処理結果チェック
                        If resultFinishWebService = ActionResult.Success Then
                            '「0：成功」の場合
                            '処理なし

                        ElseIf resultFinishWebService = ActionResult.WarningOmitDmsError Then
                            '「-9000：DMS除外エラーの警告」の場合
                            '戻り値に「-9000：DMS除外エラーの警告」を設定
                            returnCode = ActionResult.WarningOmitDmsError

                        Else
                            '上記以外の場合
                            '「15：他システムとの連携エラー」を返却
                            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END RETURNCODE={2}[FinishWebService FAILURE]" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ActionResult.DmsLinkageError))
                            Return ActionResult.DmsLinkageError

                        End If
                    End If

                End Using

            End Using

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END RETURNCODE={2}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , returnCode))
        Return returnCode

    End Function

    ''' <summary>
    ''' 作業完了のWebService処理
    ''' </summary>
    ''' <param name="dtChipEntity">チップ情報</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inPrevStatus">更新前予約ステータス</param>
    ''' <param name="inPrevJobStatus">更新前作業ステータス</param>
    ''' <param name="inIsUseJobDispatch">作業連携送信有無フラグ</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function FinishWebService(ByVal dtChipEntity As TabletSmbCommonClassChipEntityDataTable, _
                                      ByVal inStallUseId As Decimal, _
                                      ByVal inPrevStatus As String, _
                                      ByVal inPrevJobStatus As IC3802701JobStatusDataTable, _
                                      ByVal inIsUseJobDispatch As Boolean) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START,inStallUseId={2},inPrevStatus={3},inIsUseJobDispatch={4}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inStallUseId.ToString(CultureInfo.CurrentCulture) _
            , inPrevStatus _
            , inIsUseJobDispatch.ToString(CultureInfo.CurrentCulture)))

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        'ステータス連携で送信する更新後のROステータスを取得する
        Dim crntStatus As String = Me.JudgeChipStatus(inStallUseId)

        '基幹側にステータス情報を送信
        Using ic3802601blc As New IC3802601BusinessLogic
            'ステータス連携実施
            Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(dtChipEntity(0).SVCIN_ID, _
                                                                    dtChipEntity(0).JOB_DTL_ID, _
                                                                    inStallUseId, _
                                                                    inPrevStatus, _
                                                                    crntStatus, _
                                                                    0)

            '処理結果チェック
            If dmsSendResult = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

        End Using

        '実績送信使用の場合
        If inIsUseJobDispatch Then

            'JobDispatch連携で送信する更新後の作業ステータスを取得する
            Dim crntJobStatus As IC3802701JobStatusDataTable = Me.JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)

            'JobDispatch連携を実施
            Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(dtChipEntity(0).SVCIN_ID, _
                                                                   dtChipEntity(0).JOB_DTL_ID, _
                                                                   inPrevJobStatus, _
                                                                   crntJobStatus)

            '処理結果チェック
            If resultSendJobClock = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf resultSendJobClock = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendJobClockOnInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

        End If

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END RETURNCODE={2}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , returnCode))
        Return returnCode

    End Function

    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

    '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) START

    ''' <summary>
    ''' 中断したチップの完了処理
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inUpdateDate">現在日時</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inSystemId">呼ぶ先プログラムID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function FinishStopChip(ByVal inStallUseId As Decimal, _
                                   ByVal inUpdateDate As Date, _
                                   ByVal inRowLockVersion As Long, _
                                   ByVal inSystemId As String) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} Start inStallUseId={2}, inUpdateDate={3}, inRowLockVersion={4}, inSystemId={5} ", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  inStallUseId, _
                                  inUpdateDate, _
                                  inRowLockVersion, _
                                  inSystemId))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '***********************************************************************
        ' 1. いろいろな値を準備する
        '***********************************************************************
        'チップエンティティを取得処理
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(inStallUseId, 1)

        If dtChipEntity.Count <> 1 Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}.{1} End [GetChipEntityError]", _
                                       Me.GetType.ToString, _
                                       MethodBase.GetCurrentMethod.Name))

            Return ActionResult.GetChipEntityError

        End If

        'サービス入庫ID
        Dim svcInId As Decimal = dtChipEntity(0).SVCIN_ID
        'スタッフ情報
        Dim staffInfo As StaffContext = StaffContext.Current

        '***********************************************************************
        ' 2. いろいろなチェックをする
        '***********************************************************************
        '中断終了操作のチェック処理
        Dim rsltCheck As Long = Me.CheckStopFinishAction(dtChipEntity(0), _
                                                         inRowLockVersion, _
                                                         staffInfo.Account, _
                                                         inUpdateDate, _
                                                         inSystemId)

        'チェックエラーがある場合（ローカル変数中断終了チェック結果<>0時
        If rsltCheck <> ActionResult.Success Then

            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                       "{0}.{1} End [CheckStopFinishAction : Error num is {2}]", _
                                       Me.GetType.ToString, _
                                       MethodBase.GetCurrentMethod.Name, _
                                       rsltCheck))
            Return rsltCheck

        End If

        '作業実績送信使用するフラグを取得する
        Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()
        '作業実績送信の場合、作業ステータスを取得する
        Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing

        '作業実績送信の場合（ローカル変数作業実績送信使用するフラグ ＝TRUE時）
        If isUseJobDispatch Then

            '作業ステータスを取得
            prevJobStatus = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)

        End If

        '更新前のステータス取得
        Dim prevStatus As String = Me.JudgeChipStatus(inStallUseId)

        '***********************************************************************
        ' 3. DB更新
        '***********************************************************************
        'update用データセット
        Using targetDtChipEntity As New TabletSmbCommonClassChipEntityDataTable

            Dim targetDrChipEntity As TabletSmbCommonClassChipEntityRow = CType(targetDtChipEntity.NewRow(), TabletSmbCommonClassChipEntityRow)

            'ストール利用ステータス、更新日時、更新者、サービスID、ストール利用IDに設定
            targetDrChipEntity.STALL_USE_STATUS = StalluseStatusFinish
            targetDrChipEntity.UPDATE_DATETIME = inUpdateDate
            targetDrChipEntity.UPDATE_STF_CD = staffInfo.Account
            targetDrChipEntity.SVCIN_ID = svcInId
            targetDrChipEntity.STALL_USE_ID = inStallUseId

            Using ta As New TabletSMBCommonClassDataAdapter

                '次のサービスステータスを更新する(検査待ち、洗車待ち、預かり中、納車待ち)
                targetDrChipEntity.SVC_STATUS = Me.GetNextSvcStatusByFinish(staffInfo, dtChipEntity(0))

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''サービス入庫テーブルの更新
                'Dim rtCntSvcinStatus As Long = ta.UpdateSvcinStatus(targetDrChipEntity.SVCIN_ID, _
                '                                                    targetDrChipEntity.SVC_STATUS, _
                '                                                    targetDrChipEntity.UPDATE_DATETIME, _
                '                                                    targetDrChipEntity.UPDATE_STF_CD)

                ''更新件数が１件でないの場合（ローカル変数更新件数<>1時）
                'If rtCntSvcinStatus <> 1 Then

                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                '                               "{0}.{1} End [Failed to update TB_T_SERVICEIN. SVCIN_ID={2}, SVC_STATUS={3}]", _
                '                               Me.GetType.ToString, _
                '                               MethodBase.GetCurrentMethod.Name, _
                '                               targetDrChipEntity.SVCIN_ID, _
                '                               targetDrChipEntity.SVC_STATUS))

                '    Return ActionResult.ExceptionError

                'End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                ''ストール利用テーブルを更新
                'Dim rtCntStallUseRsltEndDate As Long = ta.UpdateStallUseRsltEndDate(targetDrChipEntity, inSystemId)

                Dim rtCntStallUseRsltEndDate As Long

                Using dealerEnvBiz As New ServiceCommonClassBusinessLogic
                    '休憩取得自動判定フラグ
                    Dim autoJudgeFlg = String.Empty
                    autoJudgeFlg = dealerEnvBiz.GetDlrSystemSettingValueBySettingName(RestAutoJudgeFlg)

                'ストール利用テーブルを更新
                    rtCntStallUseRsltEndDate = ta.UpdateStallUseRsltEndDate(targetDrChipEntity, inSystemId, autoJudgeFlg)
                End Using
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                '更新件数が0件以下の場合
                If rtCntStallUseRsltEndDate <= 0 Then

                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                               "{0}.{1} End [Failed to update TB_T_STALL_USE. STALL_USE_ID={2}, STALL_USE_STATUS={3}]", _
                                               Me.GetType.ToString, _
                                               MethodBase.GetCurrentMethod.Name, _
                                               targetDrChipEntity.STALL_USE_ID, _
                                               targetDrChipEntity.STALL_USE_STATUS))

                    Return ActionResult.ExceptionError

                End If

                '作業実績テーブルを更新
                Dim resultUpdateJobResult As Integer = _
                    ta.UpdateJobResultByMoveToNextProcess(dtChipEntity(0).JOB_DTL_ID, _
                                                          staffInfo.Account, _
                                                          inUpdateDate, _
                                                          inSystemId)

                '更新件数が0件以下の場合
                If resultUpdateJobResult <= 0 Then

                    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                               "{0}.{1} End [Failed to update TB_T_JOB_RESULT.]", _
                                               Me.GetType.ToString, _
                                               MethodBase.GetCurrentMethod.Name))

                    Return ActionResult.ExceptionError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                Using bizServiceCommonClass As New ServiceCommonClassBusinessLogic
                    
                    'サービスDMS納車実績ワークを取得
                    Dim dtWorkServiceDmsResultDelivery As ServiceCommonClassDataSet.WorkServiceDmsResultDeliveryDataTable = _
                        bizServiceCommonClass.GetWorkServiceDmsResultDelivery(dtChipEntity(0).SVCIN_ID)

                    '取得結果と更新後サービスステータスのチェック
                    If Not (IsNothing(dtWorkServiceDmsResultDelivery)) AndAlso 0 < dtWorkServiceDmsResultDelivery.Count AndAlso _
                       (SvcStatusDropOffCustomer.Equals(targetDrChipEntity.SVC_STATUS) OrElse _
                        SvcStatusWaitingCustomer.Equals(targetDrChipEntity.SVC_STATUS)) Then
                        '取得できた場合且つ、更新後サービスステータスが「11：預かり中 or 12：納車待ち」の場合
                        '強制納車処理を実施
                        Dim returnCodeForceDeliverd As Integer = Me.ForceDeliverd(dtChipEntity(0).DLR_CD,
                                                                                  dtChipEntity(0).BRN_CD, _
                                                                                  dtChipEntity(0).SVCIN_ID, _
                                                                                  dtChipEntity(0).RO_NUM, _
                                                                                  dtWorkServiceDmsResultDelivery(0).DMS_RSLT_DELI_DATETIME, _
                                                                                  staffInfo.Account, _
                                                                                  inUpdateDate, _
                                                                                  inSystemId)

                        '処理結果チェック
                        If returnCodeForceDeliverd <> ActionResult.Success Then
                            '「0：成功」以外の場合
                            '戻り値に「-9000：DMS除外エラーの警告」を設定
                            returnCode = ActionResult.WarningOmitDmsError
                            '「22：予期せぬエラー」を返却
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END RETURNCODE={2}[ForceDeliverd FAILURE]" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ActionResult.ExceptionError))
                            Return ActionResult.ExceptionError

                        End If

                    Else
                        '上記以外の場合
                        '中断終了処理を実施
                        'サービス入庫テーブルの更新
                        Dim rtCntSvcinStatus As Long = ta.UpdateSvcinStatus(targetDrChipEntity.SVCIN_ID, _
                                                                            targetDrChipEntity.SVC_STATUS, _
                                                                            targetDrChipEntity.UPDATE_DATETIME, _
                                                                            targetDrChipEntity.UPDATE_STF_CD)

                        '更新件数が１件でないの場合（ローカル変数更新件数<>1時）
                        If rtCntSvcinStatus <> 1 Then

                            Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                                       "{0}.{1} End [Failed to update TB_T_SERVICEIN. SVCIN_ID={2}, SVC_STATUS={3}]", _
                                                       Me.GetType.ToString, _
                                                       MethodBase.GetCurrentMethod.Name, _
                                                       targetDrChipEntity.SVCIN_ID, _
                                                       targetDrChipEntity.SVC_STATUS))

                            Return ActionResult.ExceptionError

                        End If

                        'RO情報テーブルのROステータスを更新
                        Me.FinishRoStatus(svcInId, _
                                          dtChipEntity(0).JOB_DTL_ID, _
                                          dtChipEntity(0).RO_NUM, _
                                          inUpdateDate, _
                                          staffInfo.Account, _
                                          inSystemId)

                        '***********************************************************************
                        ' 4. 基幹連携
                        '***********************************************************************
                        '更新後のステータス取得
                        Dim crntStatus As String = Me.JudgeChipStatus(inStallUseId)

                        '基幹側にステータス情報を送信
                        Using ic3802601blc As New IC3802601BusinessLogic
                            'ステータス連携実施
                            Dim resultSendStatus As Long = ic3802601blc.SendStatusInfo(svcInId, _
                                                                                       dtChipEntity(0).JOB_DTL_ID, _
                                                                                       inStallUseId, _
                                                                                       prevStatus, _
                                                                                       crntStatus, _
                                                                                       0)

                            '処理結果チェック
                            If resultSendStatus = ActionResult.Success Then
                                '「0：成功」の場合
                                '処理なし

                            ElseIf resultSendStatus = ActionResult.WarningOmitDmsError Then
                                '「-9000：DMS除外エラーの警告」の場合
                                '戻り値に「-9000：DMS除外エラーの警告」を設定
                                returnCode = ActionResult.WarningOmitDmsError

                            Else
                                '上記以外の場合
                                '「15：他システムとの連携エラー」を返却
                                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , ActionResult.DmsLinkageError))
                                Return ActionResult.DmsLinkageError

                            End If

                        End Using

                        '実績送信使用の場合
                        If isUseJobDispatch Then

                            '作業ステータスを取得する
                            Dim crntJobStatus As IC3802701JobStatusDataTable = Me.JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)

                            '基幹側にJobDispatch実績情報を送信
                            Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(svcInId, _
                                                                                   dtChipEntity(0).JOB_DTL_ID, _
                                                                                   prevJobStatus, _
                                                                                   crntJobStatus)

                            '処理結果チェック
                            If resultSendJobClock = ActionResult.Success Then
                                '「0：成功」の場合
                                '処理なし

                            ElseIf resultSendJobClock = ActionResult.WarningOmitDmsError Then
                                '「-9000：DMS除外エラーの警告」の場合
                                '戻り値に「-9000：DMS除外エラーの警告」を設定
                                returnCode = ActionResult.WarningOmitDmsError

                            Else
                                '上記以外の場合
                                '「15：他システムとの連携エラー」を返却
                                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} END RETURNCODE={2}[SendJobClockOnInfo FAILURE]" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , ActionResult.DmsLinkageError))
                                Return ActionResult.DmsLinkageError

                            End If

                        End If

                    End If

                End Using

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End Using

        End Using

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        ''RO情報テーブルのROステータスを更新
        'Me.FinishRoStatus(svcInId, _
        '                  dtChipEntity(0).JOB_DTL_ID, _
        '                  dtChipEntity(0).RO_NUM, _
        '                  inUpdateDate, _
        '                  staffInfo.Account, _
        '                  inSystemId)

        ''***********************************************************************
        '' 4. 基幹連携
        ''***********************************************************************
        ''更新後のステータス取得
        'Dim crntStatus As String = Me.JudgeChipStatus(inStallUseId)

        ''基幹側にステータス情報を送信
        'Using ic3802601blc As New IC3802601BusinessLogic
        '    'ステータス連携実施
        '    Dim resultSendStatus As Long = ic3802601blc.SendStatusInfo(svcInId, _
        '                                                               dtChipEntity(0).JOB_DTL_ID, _
        '                                                               inStallUseId, _
        '                                                               prevStatus, _
        '                                                               crntStatus, _
        '                                                               0)

        '    '送信失敗の場合
        '    If resultSendStatus <> 0 Then

        '        Logger.Error(String.Format(CultureInfo.CurrentCulture, _
        '                                   "{0}.{1} End [DmsLinkageError:SendStatusInfo FAILURE]", _
        '                                   Me.GetType.ToString, _
        '                                   MethodBase.GetCurrentMethod.Name))

        '        Return ActionResult.DmsLinkageError

        '    End If

        'End Using

        ''実績送信使用の場合
        'If isUseJobDispatch Then

        '    '作業ステータスを取得する
        '    Dim crntJobStatus As IC3802701JobStatusDataTable = Me.JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)

        '    '基幹側にJobDispatch実績情報を送信
        '    Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(svcInId, _
        '                                                           dtChipEntity(0).JOB_DTL_ID, _
        '                                                           prevJobStatus, _
        '                                                           crntJobStatus)

        '    '送信失敗の場合
        '    If resultSendJobClock <> ActionResult.Success Then

        '        Logger.Error(String.Format(CultureInfo.CurrentCulture, _
        '                                   "{0}.{1} End [DmsLinkageError:SendJobClockOnInfo FAILURE]", _
        '                                   Me.GetType.ToString, _
        '                                   MethodBase.GetCurrentMethod.Name))

        '        Return ActionResult.DmsLinkageError

        '    End If

        'End If

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.{1} End ReturnValue={2}", _
                                  Me.GetType.ToString, _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  returnCode))
        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    '2015/02/13 TMEJ 範 新プラットフォーム版i-CROP仕様変更対応 (SMBチップ検索及び作業中断機能強化) END

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

    ''' <summary>
    ''' All Finish後、Pushフラグを設定
    ''' </summary>
    ''' <param name="inStallUseStatus">All Finish後、該当チップのストール利用ステータス</param>
    ''' <remarks>コード分析でFinish関数の複雑度が25以上になって、関数化しました</remarks>
    Private Sub SetAllFinishPushFlg(ByVal inStallUseStatus As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inStallUseStatus={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inStallUseStatus))


        If StalluseStatusFinish.Equals(inStallUseStatus) Then
            'ストール利用ステータスが終了になれば

            '終了Push送信フラグにTrueを設定する
            Me.NeedPushAfterFinishSingleJob = True

        ElseIf StalluseStatusStop.Equals(inStallUseStatus) Then
            'ストール利用ステータスが中断になれば

            '中断Push送信フラグにTrueを設定する
            Me.NeedPushAfterStopSingleJob = True

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End", _
                                  MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 単独Job終了の主な処理
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inRsltEndDateTime">実績終了日時</param>
    ''' <param name="inRestFlg">休憩取得フラグ</param>
    ''' <param name="inJobInstructId">作業指示ID</param>
    ''' <param name="inJobInstructSeq">作業指示枝番</param>
    ''' <param name="inUpdateDate">更新時間</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inSystemId">呼ぶ画面ID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function FinishSingleJob(ByVal inStallUseId As Decimal, _
                                    ByVal inRsltEndDateTime As Date, _
                                    ByVal inRestFlg As String, _
                                    ByVal inJobInstructId As String, _
                                    ByVal inJobInstructSeq As Long, _
                                    ByVal inUpdateDate As Date, _
                                    ByVal inRowLockVersion As Long, _
                                    ByVal inSystemId As String) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inStallUseId={1}, inRsltEndDateTime={2}, inRestFlg={3}, inJobInstructId={4}, inJobInstructSeq={5}, inUpdateDate={6}, inRowLockVersion={7}, inSystemId={8}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inStallUseId, _
                                  inRsltEndDateTime, _
                                  inRestFlg, _
                                  inJobInstructId, _
                                  inJobInstructSeq, _
                                  inUpdateDate, _
                                  inRowLockVersion, _
                                  inSystemId))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Try

            '中断、終了のPushフラグを初期化(送信しない)
            NeedPushAfterStopSingleJob = False
            NeedPushAfterFinishSingleJob = False

            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
            NeedPushSubAreaRefresh = False
            '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

            'チップエンティティを取得する
            Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(inStallUseId, 1)

            'ストール利用IDで取得した件数が1件以外の場合、チップエンティティエラーを戻す
            If 1 <> dtChipEntity.Count Then

                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                                , MethodBase.GetCurrentMethod.Name))
                Return ActionResult.GetChipEntityError

            End If

            'スタッフ情報
            Dim staffInfo As StaffContext = StaffContext.Current

            '実績終了日時を取得
            Dim rsltEndDateTimeNoSec As Date = Me.CheckRsltEndDateTime(dtChipEntity(0).RSLT_START_DATETIME, _
                                                                       inRsltEndDateTime, _
                                                                       staffInfo.DlrCD, _
                                                                       staffInfo.BrnCD)

            '指定Job終了後、次のチップのステータス(作業中、中断、終了)を取得する
            Dim drAfterFinishChipStatus As TabletSmbCommonClassChipStatusRow = Me.GetChipStatusAfterFinishSingleJob(dtChipEntity(0).JOB_DTL_ID, _
                                                                                                                    inJobInstructId, _
                                                                                                                    inJobInstructSeq)

            If AfterFinishChipStatusStop.Equals(drAfterFinishChipStatus.CHIP_STATUS) Then
                '次のチップステータスが中断の場合

                '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
                '作業実績送信使用するフラグを取得する
                Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()

                '作業実績送信の場合、作業ステータスを取得する
                Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing
                If isUseJobDispatch Then
                    prevJobStatus = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)
                End If
                '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない END

                '選択したJobを終了する
                Dim retSingJobFinish As Long = Me.FinishSingleJobAction(inStallUseId, _
                                                                        rsltEndDateTimeNoSec, _
                                                                        inRestFlg, _
                                                                        inJobInstructId, _
                                                                        inJobInstructSeq, _
                                                                        inUpdateDate, _
                                                                        inRowLockVersion, _
                                                                        inSystemId, _
                                                                        False, _
                                                                        dtChipEntity)

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''エラーがあれば
                'If ActionResult.Success <> retSingJobFinish Then

                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                '                               "{0}.End. FinishSingleJobAction failed. ErrorCode={1}", _
                '                               MethodBase.GetCurrentMethod.Name, _
                '                               retSingJobFinish))
                '    'エラーコードを戻す
                '    Return retSingJobFinish

                'End If

                '処理結果チェック
                If retSingJobFinish = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf retSingJobFinish = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[FinishSingleJobAction FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない START
                ''作業中断関数を呼んで、チップを中断する
                'Dim retJobStop As Long = Me.ChangeToStopChipByStop(inStallUseId, _
                '                                                   rsltEndDateTimeNoSec, _
                '                                                   0, _
                '                                                   drAfterFinishChipStatus.STOP_MEMO, _
                '                                                   drAfterFinishChipStatus.STOP_REASON_TYPE, _
                '                                                   inRestFlg, _
                '                                                   inUpdateDate, _
                '                                                   inRowLockVersion, _
                '                                                   inSystemId, _
                '                                                   dtChipEntity)

                '作業中断関数を呼んで、チップを中断する
                Dim retJobStop As Long = Me.ChangeToStopChipByStop(inStallUseId, _
                                                                   rsltEndDateTimeNoSec, _
                                                                   0, _
                                                                   drAfterFinishChipStatus.STOP_MEMO, _
                                                                   drAfterFinishChipStatus.STOP_REASON_TYPE, _
                                                                   inRestFlg, _
                                                                   inUpdateDate, _
                                                                   inRowLockVersion, _
                                                                   inSystemId, _
                                                                   dtChipEntity, _
                                                                   prevJobStatus)
                '2016/06/23 NSK 皆川 TR-SVT-TMT-20151110-001 技術者がタブレットでジョブを停止できない END

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'If ActionResult.Success = retJobStop Then
                '    '中断操作成功の場合

                '    '中断した後のPushフラグを立てる(True:送信する)
                '    NeedPushAfterStopSingleJob = True

                'End If

                '処理結果チェック
                If retJobStop = ActionResult.Success Then
                    '「0：成功」の場合
                    '中断した後のPushフラグを立てる(True:送信する)
                    NeedPushAfterStopSingleJob = True

                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
                    ' Job終了した結果、中断になる場合は、全CT/CHTにPush送信するため、サブエリアリフレッシュフラグにTrueを設定する
                    Me.NeedPushSubAreaRefresh = True
                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END

                ElseIf retJobStop = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                    '中断した後のPushフラグを立てる(True:送信する)
                    NeedPushAfterStopSingleJob = True

                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
                    ' Job終了した結果、中断になる場合は、全CT/CHTにPush送信するため、サブエリアリフレッシュフラグにTrueを設定する
                    Me.NeedPushSubAreaRefresh = True
                    '2016/01/11 NSK 浅野 (トライ店システム評価)アカウント・組織・文言マスタの仕様変更対応 END
                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[ChangeToStopChipByStop FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}.End return={1}", _
                                          MethodBase.GetCurrentMethod.Name, _
                                          returnCode))

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''中断結果を戻す
                'Return retJobStop

                '処理結果を戻す
                Return returnCode

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            ElseIf AfterFinishChipStatusFinish.Equals(drAfterFinishChipStatus.CHIP_STATUS) Then
                '次のチップステータスが終了の場合

                'チップ終了関数を呼んで、チップを終了する
                Dim retFinish As Long = Me.Finish(inStallUseId, _
                                                  rsltEndDateTimeNoSec, _
                                                  inRestFlg, _
                                                  inUpdateDate, _
                                                  inRowLockVersion, _
                                                  inSystemId)

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'If ActionResult.Success = retFinish Then
                '    '終了操作成功の場合（DB更新もう成功したからPush送信をする）

                '    '終了した後のPushフラグを立てる(True:送信する)
                '    NeedPushAfterFinishSingleJob = True

                'End If

                '処理結果チェック
                If retFinish = ActionResult.Success Then
                    '「0：成功」の場合
                    '終了した後のPushフラグを立てる(True:送信する)
                    NeedPushAfterFinishSingleJob = True

                ElseIf retFinish = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                    '終了した後のPushフラグを立てる(True:送信する)
                    NeedPushAfterFinishSingleJob = True

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                          "{0}.End return={1}", _
                                          MethodBase.GetCurrentMethod.Name, _
                                          returnCode))

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''終了結果を戻す
                'Return retFinish

                '処理結果を戻す
                Return returnCode

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            Else

                '次のチップステータスが変わらないから、ただ選択したJob終了

                '選択したJobを終了する
                Dim retSingJobFinish As Long = Me.FinishSingleJobAction(inStallUseId, _
                                                                        rsltEndDateTimeNoSec, _
                                                                        inRestFlg, _
                                                                        inJobInstructId, _
                                                                        inJobInstructSeq, _
                                                                        inUpdateDate, _
                                                                        inRowLockVersion, _
                                                                        inSystemId, _
                                                                        True, _
                                                                        dtChipEntity)

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''エラーがあれば
                'If ActionResult.Success <> retSingJobFinish Then

                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                '                               "{0}.End. FinishSingleJobAction failed. ErrorCode={1}", _
                '                               MethodBase.GetCurrentMethod.Name, _
                '                               retSingJobFinish))
                '    'エラーコードを戻す
                '    Return retSingJobFinish

                'End If

                '処理結果チェック
                If retSingJobFinish = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf retSingJobFinish = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If

            ' 正常終了
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End. Success", MethodBase.GetCurrentMethod.Name))

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'Return ActionResult.Success

            Return returnCode

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Catch ex As OracleExceptionEx When ex.Number = 1013
            'DBと接続タイムアウトの場合

            'タイムアウトコードを戻す
            Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.E Error:DBTimeOutError.", _
                                       System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.DBTimeOutError

        End Try

    End Function

    ''' <summary>
    ''' 単独Job終了処理
    ''' </summary>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inRsltEndDateTime">実績終了日時</param>
    ''' <param name="inRestFlg">休憩取得フラグ</param>
    ''' <param name="inJobInstructId">作業指示ID</param>
    ''' <param name="inJobInstructSeq">作業指示枝番</param>
    ''' <param name="inUpdateDate">更新時間</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inSystemId">呼ぶ画面ID</param>
    ''' <param name="dtChipEntity">チップエンティティ</param>
    ''' <param name="inLockTableFlg">サービス入庫テーブルをロックするかどうかフラグ</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Private Function FinishSingleJobAction(ByVal inStallUseId As Decimal, _
                                           ByVal inRsltEndDateTime As Date, _
                                           ByVal inRestFlg As String, _
                                           ByVal inJobInstructId As String, _
                                           ByVal inJobInstructSeq As Long, _
                                           ByVal inUpdateDate As Date, _
                                           ByVal inRowLockVersion As Long, _
                                           ByVal inSystemId As String, _
                                           ByVal inLockTableFlg As Boolean, _
                                           ByVal dtChipEntity As TabletSmbCommonClassChipEntityDataTable) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.S. inRsltEndDateTime={1}, inLockTableFlg={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inRsltEndDateTime, _
                                  inLockTableFlg))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '***********************************************************************
        ' 1. ローカル変数初期化
        '***********************************************************************

        'チップエンティティのDataRow
        Dim dataRowChipEntity As TabletSmbCommonClassChipEntityRow = dtChipEntity(0)

        'サービス入庫ID
        Dim svcInId As Decimal = dataRowChipEntity.SVCIN_ID

        'スタッフ情報
        Dim staffInfo As StaffContext = StaffContext.Current

        '引数の休憩取得フラグが設定されていない場合、1(取得する)に設定する
        If String.IsNullOrWhiteSpace(inRestFlg) Then

            inRestFlg = RestTimeGetFlgGetRest

        End If

        '該当作業の作業ステータスを取得する
        Dim jobStatus As String = Me.GetJobStatus(dataRowChipEntity.JOB_DTL_ID, _
                                                  inJobInstructId, _
                                                  inJobInstructSeq)

        '***********************************************************************
        ' 2. チェック処理
        '***********************************************************************
        Dim rsltCheck As Long = Me.CheckSingleFinishAction(dataRowChipEntity, _
                                                           inRsltEndDateTime, _
                                                           inRowLockVersion, _
                                                           staffInfo, _
                                                           inUpdateDate, _
                                                           inSystemId, _
                                                           jobStatus, _
                                                           inLockTableFlg)

        'チェックでエラーがあれば、エラーコードを戻す
        If ActionResult.Success <> rsltCheck Then

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                       "{0}.E CheckFinishAction error: Error num is {1}", _
                                       MethodBase.GetCurrentMethod.Name, _
                                       rsltCheck))
            Return rsltCheck

        End If

        'JobDispatch送信使用フラグを取得する
        Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()

        'DB更新前の作業ステータスを取得する
        Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing

        'JobDispatch実績送信の場合、作業ステータスを取得する
        If isUseJobDispatch Then

            prevJobStatus = Me.JudgeSingleJobStatus(dataRowChipEntity.JOB_DTL_ID, _
                                                    inJobInstructId, _
                                                    inJobInstructSeq, _
                                                    jobStatus)

        End If

        '更新前のステータス取得
        Dim prevStatus As String = Me.JudgeChipStatus(inStallUseId)

        '***********************************************************************
        ' 3. DB更新
        '***********************************************************************

        '該当作業の実績を更新する(作業実績テーブルに)
        Using ta As New TabletSMBCommonClassDataAdapter

            '作業実績更新用の情報テーブル
            Using dtJobStatus As New TabletSmbCommonClassJobStatusDataTable

                '新しいデータ行を生成する
                Dim drJobStatus As TabletSmbCommonClassJobStatusRow = dtJobStatus.NewTabletSmbCommonClassJobStatusRow

                '更新用のデータを登録する
                drJobStatus.JOB_DTL_ID = dataRowChipEntity.JOB_DTL_ID
                drJobStatus.JOB_INSTRUCT_ID = inJobInstructId
                drJobStatus.JOB_INSTRUCT_SEQ = inJobInstructSeq
                drJobStatus.JOB_STATUS = JobStatusFinish
                drJobStatus.STOP_MEMO = Space(1)
                drJobStatus.STOP_REASON_TYPE = Space(1)
                dtJobStatus.AddTabletSmbCommonClassJobStatusRow(drJobStatus)

                '作業実績テーブルを更新する(該当作業実績を終了する)
                Dim updateCount As Long = _
                    ta.UpdateSingleJobResultByJobStopFinish(dtJobStatus(0), _
                                                            inRsltEndDateTime, _
                                                            staffInfo.Account, _
                                                            inUpdateDate, _
                                                            inSystemId)

                '更新行数が1以外の場合、予期せぬエラーを戻す
                If 1 <> updateCount Then

                    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                                "{0}.E ExceptionError:UpdateJobRsltOnFinish update count=0.", _
                                                MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.ExceptionError

                End If

            End Using

            'ROステータスを終了する
            Me.FinishRoStatus(dataRowChipEntity.SVCIN_ID, _
                              dataRowChipEntity.JOB_DTL_ID, _
                              dataRowChipEntity.RO_NUM, _
                              inUpdateDate, _
                              staffInfo.Account, _
                              inSystemId)


        End Using

        '***********************************************************************
        ' 4. 基幹連携
        '***********************************************************************

        'ロックテーブルフラグがTrueの場合、この関数を終わった後、期間連携は送信しないので、基幹連携送信をする
        If inLockTableFlg Then

            '更新後のステータス取得
            Dim crntStatus As String = Me.JudgeChipStatus(inStallUseId)

            '基幹側にステータス情報を送信
            Using ic3802601blc As New IC3802601BusinessLogic

                Dim resultSendStatusInfo As Long = ic3802601blc.SendStatusInfo(svcInId, _
                                                                               dataRowChipEntity.JOB_DTL_ID, _
                                                                               inStallUseId, _
                                                                               prevStatus, _
                                                                               crntStatus, _
                                                                               0)

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''送信失敗の場合、DMS連携エラーコードを戻す
                'If ActionResult.Success <> resultSendStatusInfo Then

                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                '                               "{0}.{1} SendStatusInfo FAILURE ", _
                '                               Me.GetType.ToString, _
                '                               MethodBase.GetCurrentMethod.Name))
                '    Return ActionResult.DmsLinkageError

                'End If

                '処理結果チェック
                If resultSendStatusInfo = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf resultSendStatusInfo = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End Using

            '実績送信使用の場合
            If isUseJobDispatch Then

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''作業ステータスを取得する
                'Dim crntJobStatus As IC3802701JobStatusDataTable = Me.JudgeSingleJobStatus(dataRowChipEntity.JOB_DTL_ID, _
                '                                                                           inJobInstructId, _
                '                                                                           inJobInstructSeq, _
                '                                                                           jobStatus)

                '該当作業のデータ更新後作業ステータスを取得する
                Dim crntSingleJobStatus As String = Me.GetJobStatus(dataRowChipEntity.JOB_DTL_ID, _
                                                                    inJobInstructId, _
                                                                    inJobInstructSeq)

                '作業ステータスを取得する
                Dim crntJobStatus As IC3802701JobStatusDataTable = Me.JudgeSingleJobStatus(dataRowChipEntity.JOB_DTL_ID, _
                                                                                           inJobInstructId, _
                                                                                           inJobInstructSeq, _
                                                                                           crntSingleJobStatus)

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                '基幹側にJobDispatch実績情報を送信
                Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(svcInId, _
                                                                       dataRowChipEntity.JOB_DTL_ID, _
                                                                       prevJobStatus, _
                                                                       crntJobStatus)

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                ''送信失敗の場合、DMS連携エラーコードを戻す
                'If ActionResult.Success <> resultSendJobClock Then

                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                '                               "{0}.{1}.End DmsLinkageError:SendJobClockOnInfo Failure. ", _
                '                               Me.GetType.ToString, _
                '                               MethodBase.GetCurrentMethod.Name))
                '    Return ActionResult.DmsLinkageError

                'End If

                '処理結果チェック
                If resultSendJobClock = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf resultSendJobClock = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[SendJobClockOnInfo FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If

        End If

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End", MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function


    ''' <summary>
    ''' 実績終了日時をチェック
    ''' </summary>
    ''' <param name="inRsltStartDate">実績開始日時</param>
    ''' <param name="inRsltEndDate">実績終了日時</param>
    ''' <returns>実績終了日時</returns>
    ''' <remarks>
    ''' 実績開始日時、実績終了日時の日付が別々の場合、
    ''' 実績開始日の営業終了日時を戻す
    ''' </remarks>
    Private Function CheckRsltEndDateTime(ByVal inRsltStartDate As Date, _
                                          ByVal inRsltEndDate As Date, _
                                          ByVal inDlrCode As String, _
                                          ByVal inBrnCode As String) As Date

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inRsltStartDate={1}, inRsltEndDate={2}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inRsltStartDate, _
                                  inRsltEndDate))

        '戻る用実績終了日時を初期化(秒を切り捨てる)
        Dim retRsltEndDate As Date = Me.GetDateTimeFloorSecond(inRsltEndDate)

        If Date.Compare(inRsltStartDate.Date, inRsltEndDate.Date) <> 0 Then
            '日付が違う場合

            '実績開始日の営業終了日時を取得
            Dim operationDate As TabletSmbCommonClassBranchOperatingHoursDataTable = _
                GetOneDayBrnOperatingHours(inRsltStartDate, inDlrCode, inBrnCode)

            If Not IsNothing(operationDate) Then
                '取得出来る場合

                '戻る
                retRsltEndDate = operationDate(0).SVC_JOB_END_TIME

            End If

        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.End. Return={1}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  retRsltEndDate))

        Return retRsltEndDate

    End Function

    ''' <summary>
    ''' 指定Job終了後、チップのステータスを取得する
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inJobInstructId">作業指示ID</param>
    ''' <param name="inJobInstructSeq">作業指示連番</param>
    ''' <returns>チップのステータスデータ行</returns>
    ''' <remarks></remarks>
    Private Function GetChipStatusAfterFinishSingleJob(ByVal inJobDtlId As Decimal, _
                                                       ByVal inJobInstructId As String, _
                                                       ByVal inJobInstructSeq As Long) As TabletSmbCommonClassChipStatusRow

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. inJobDtlId={1}, inJobInstructId={2}, inInstructSeq={3}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  inJobDtlId, _
                                  inJobInstructId, _
                                  inJobInstructSeq))

        '作業指示テーブル
        Dim dtJobInstruct As TabletSmbCommonClassJobInstructDataTable = Nothing

        '作業実績テーブル
        Dim dtJobInstructResult As TabletSmbCommonClassJobStatusDataTable = Nothing

        Using ta As New TabletSMBCommonClassDataAdapter

            '該当Jobに紐づく全て作業を取得する(作業指示テーブルから)
            dtJobInstruct = ta.GetJobInstructIdAndSeqByJobDtlId(inJobDtlId)

            '該当Jobに紐づく全て作業実績を取得する(作業実績テーブルから)
            dtJobInstructResult = ta.GetAllJobRsltInfoByJobDtlId(inJobDtlId)

        End Using

        '返却用テーブル
        Using dtChipStatus As New TabletSmbCommonClassChipStatusDataTable

            Dim drChipStatus As TabletSmbCommonClassChipStatusRow = dtChipStatus.NewTabletSmbCommonClassChipStatusRow

            '初期化(中断以外の場合全部空白から)
            drChipStatus.STOP_REASON_TYPE = ""
            drChipStatus.STOP_MEMO = ""


            '実績件数が全作業件数と違う場合、未開始作業がいる
            If dtJobInstruct.Count <> dtJobInstructResult.Count Then

                '未開始Jobがいるから、作業終了後、チップがまだ作業中
                drChipStatus.CHIP_STATUS = AfterFinishChipStatusWorking

            Else
                '未開始作業がいない

                '作業中Jobがあるフラグ(False:ない)
                Dim bWorkingJobFlg As Boolean = False

                '中断Jobがあるフラグ(False:ない)
                Dim bStopJobFlg As Boolean = False


                '作業実績データテーブルでループして、自分以外のJob中、中断、作業中Jobがあるかどうか
                For Each drInstructResult As TabletSmbCommonClassJobStatusRow In dtJobInstructResult

                    'Jobが自分の場合、Continue
                    If (inJobInstructId.Equals(drInstructResult.JOB_INSTRUCT_ID) _
                                    And inJobInstructSeq = drInstructResult.JOB_INSTRUCT_SEQ) Then

                        Continue For

                    End If

                    '自分以外Jobの中に作業中Jobがある場合
                    If JobStatusWorking.Equals(drInstructResult.JOB_STATUS) Then

                        '作業中JobがあるフラグにTrueを設定する(作業中Jobあり)
                        bWorkingJobFlg = True

                    ElseIf JobStatusStop.Equals(drInstructResult.JOB_STATUS) Then

                        '自分以外Jobの中に中断Jobがある場合
                        bStopJobFlg = True

                    End If

                Next

                '作業中Jobがある場合
                If bWorkingJobFlg Then

                    '作業中チップがいるから、作業終了後、チップがまだ作業中
                    drChipStatus.CHIP_STATUS = AfterFinishChipStatusWorking

                ElseIf bStopJobFlg Then
                    '中断中Jobがある場合

                    '中断実績のデータを洗い出す
                    Dim resultStop = (From p In dtJobInstructResult _
                                      Where p.JOB_STATUS = JobStatusStop _
                                      Order By p.JOB_RSLT_ID Descending _
                                      Select p).ToList()

                    '中断実績の最終の中断メモをメンバー変数に保存する
                    drChipStatus.STOP_MEMO = resultStop(0).STOP_MEMO

                    '中断実績の最終の中断理由区分をメンバー変数に保存する
                    drChipStatus.STOP_REASON_TYPE = resultStop(0).STOP_REASON_TYPE

                    '中断作業があれば、作業終了後、チップが中断中になる
                    drChipStatus.CHIP_STATUS = AfterFinishChipStatusStop

                Else

                    'ほかの作業は全部完了したまたは自分以外作業がない場合、作業終了後、チップが終了になる
                    drChipStatus.CHIP_STATUS = AfterFinishChipStatusFinish

                End If

            End If

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E Return={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      drChipStatus.CHIP_STATUS))
            '返却
            Return drChipStatus

        End Using

    End Function

    '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 終了する時、次のサービス入庫ステータスを取得する
    ''' </summary>
    ''' <param name="staffInfo">スタッフ情報</param>
    ''' <param name="chipInfoDatarow">終了チップの情報</param>
    ''' <returns>サービス入庫ステータス(検査待ち、洗車待ち、納車待ち)</returns>
    ''' <remarks></remarks>
    Private Function GetNextSvcStatusByFinish(ByVal staffInfo As StaffContext, _
                                              ByVal chipInfoDatarow As TabletSmbCommonClassChipEntityRow) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. ", MethodBase.GetCurrentMethod.Name))

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START

        'ストール利用ステータスが04作業計画に一部作業中断の場合
        If StalluseStatusStartIncludeStopJob.Equals(chipInfoDatarow.STALL_USE_STATUS) Then

            'チップが中断になるから、中断のサービスステータス取得関数を呼ぶ
            Dim svcStatus As String = Me.GetNextSvcStatusByStop(chipInfoDatarow.SVCIN_ID)

            Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                      "{0}.E Return={1}", _
                                      MethodBase.GetCurrentMethod.Name, _
                                      svcStatus))
            '結果(開始待ちまたは次の作業開始待ち)を返却
            Return svcStatus

        End If

        '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END


        '自分を含めたリレーションチップに検査有り、かつ完成検査が承認前のチップが存在する場合
        If Me.IsChangeStatusToInspection(staffInfo.DlrCD, staffInfo.BrnCD, chipInfoDatarow.SVCIN_ID) Then
            '検査待ち
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return={1}", MethodBase.GetCurrentMethod.Name, SvcStatusInspectionWait))
            Return SvcStatusInspectionWait
        Else
            '洗車有りの場合
            If InspectionNeedFlgNeed.Equals(chipInfoDatarow.CARWASH_NEED_FLG) Then
                Using ta As New TabletSMBCommonClassDataAdapter
                    '洗車実績テーブルに同じデータがあれば、削除する
                    ta.DeleteCarWashResult(chipInfoDatarow.SVCIN_ID)
                End Using
                '洗車待ち
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return={1}", MethodBase.GetCurrentMethod.Name, SvcStatusCarWashWait))
                Return SvcStatusCarWashWait
            Else
                '引取納車区分pickdelitypeはWaitingの場合、納車待ち。他の場合、預かり中。
                If DeliTypeWaiting.Equals(chipInfoDatarow.PICK_DELI_TYPE) Then
                    '納車待ち
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return={1}", MethodBase.GetCurrentMethod.Name, SvcStatusWaitingCustomer))
                    Return SvcStatusWaitingCustomer
                Else
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return={1}", MethodBase.GetCurrentMethod.Name, SvcStatusDropOffCustomer))
                    Return SvcStatusDropOffCustomer
                End If
            End If
        End If

    End Function

    ''' <summary>
    ''' 検査待ちに遷移するか
    ''' </summary>
    ''' <param name="dlrCode">販売店コード</param>
    ''' <param name="brnCode">店舗コード</param>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <returns>True:検査エリアに遷移する</returns>
    ''' <remarks></remarks>
    Private Function IsChangeStatusToInspection(ByVal dlrCode As String, _
                                                ByVal brnCode As String, _
                                                ByVal svcinId As Decimal) As Boolean

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. ", MethodBase.GetCurrentMethod.Name))

        Dim chipList As TabletSmbCommonClassStallChipInfoDataTable
        Dim svcIdList As New List(Of Decimal)
        svcIdList.Add(svcinId)
        '指定サービス入庫IDの関連チップ情報を取得する
        Using ta As New TabletSMBCommonClassDataAdapter
            chipList = ta.GetStallChipBySvcinId(dlrCode, brnCode, svcIdList)
        End Using

        '関連チップを全部ループして、
        For Each chipRow As TabletSmbCommonClassStallChipInfoRow In chipList
            '検査有り、かつ完成検査が承認前のチップが存在する場合
            If InspectionNeedFlgNeed.Equals(chipRow.INSPECTION_NEED_FLG) _
                And (InspectionNotFinish.Equals(chipRow.INSPECTION_STATUS) _
                 Or InspectionApproval.Equals(chipRow.INSPECTION_STATUS)) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return=True", MethodBase.GetCurrentMethod.Name))
                Return True
            End If
        Next
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Return=False", MethodBase.GetCurrentMethod.Name))
        Return False
    End Function

    '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
    ' ''' <summary>
    ' ''' 終了時、ROステータスを更新する
    ' ''' </summary>
    ' ''' <param name="svcinId">サービス入庫ID</param>
    ' ''' <param name="jobDtlId">作業内容ID</param>
    ' ''' <param name="inspectionFlg">検査フラグ</param>
    ' ''' <param name="inUpdateDateTime">更新日時</param>
    ' ''' <param name="inStaffCode">スタッフコード</param>
    ' ''' <param name="inUpdateFunction">更新プログラム</param>
    ' ''' <remarks></remarks>
    'Private Sub FinishRoStatus(ByVal svcinId As Decimal, _
    '                                ByVal jobDtlId As Decimal, _
    '                                ByVal inspectionFlg As String, _
    '                                ByVal inspectionStatus As String, _
    '                                ByVal roNum As String, _
    '                                ByVal inUpdateDateTime As Date, _
    '                                ByVal inStaffCode As String, _
    '                                ByVal inUpdateFunction As String)

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, _
    '                              "{0}.S. svcinId={1}, jobDtlId={2}, inspectionFlg={3}, inspectionStatus={4}, roNum={5}, inUpdateDateTime={6}, inStaffCode={7}, inUpdateFunction={8}", _
    '                              MethodBase.GetCurrentMethod.Name, _
    '                              svcinId, _
    '                              jobDtlId, _
    '                              inspectionFlg, _
    '                              inspectionStatus, _
    '                              roNum, _
    '                              inUpdateDateTime, _
    '                              inStaffCode, _
    '                              inUpdateFunction))


    ''自分の検査フラグがある且つ検査が終わってない場合、戻る
    'If InspectionNeedFlgNeed.Equals(inspectionFlg) _
    '    And Not InspectionFinished.Equals(inspectionStatus) Then
    '    ' 正常終了
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
    '    Return
    'End If

    ''' <summary>
    ''' 終了時、ROステータスを更新する
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="inUpdateDateTime">更新日時</param>
    ''' <param name="inStaffCode">スタッフコード</param>
    ''' <param name="inUpdateFunction">更新プログラム</param>
    ''' <remarks></remarks>
    Private Sub FinishRoStatus(ByVal svcinId As Decimal, _
                                    ByVal jobDtlId As Decimal, _
                                    ByVal roNum As String, _
                                    ByVal inUpdateDateTime As Date, _
                                    ByVal inStaffCode As String, _
                                    ByVal inUpdateFunction As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                                  "{0}.Start. svcinId={1}, jobDtlId={2}, roNum={3}, inUpdateDateTime={4}, inStaffCode={5}, inUpdateFunction={6}", _
                                  MethodBase.GetCurrentMethod.Name, _
                                  svcinId, _
                                  jobDtlId, _
                                  roNum, _
                                  inUpdateDateTime, _
                                  inStaffCode, _
                                  inUpdateFunction))
        '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END


        Using ta As New TabletSMBCommonClassDataAdapter
            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
            ''該チップに紐付くRO作業連番を取得する
            'Dim roJobSeqTable As TabletSmbCommonClassJobInstructDataTable = _
            '    ta.GetROJobSeqByJobDtlId(jobDtlId)

            'Dim roJobSeq As New List(Of Long)
            'For Each roJobSeqRow As TabletSmbCommonClassJobInstructRow In roJobSeqTable
            '    roJobSeq.Add(roJobSeqRow.RO_JOB_SEQ)
            'Next

            ''該チップに紐付く最終のRO作業連番を取得する
            'Dim roFinishJobSeqTable As TabletSmbCommonClassNumberValueDataTable = _
            '    ta.GetLastFinishJobSeq(svcinId, roNum, Me.ConvertLongArrayToString(roJobSeq))

            '該チップに紐付く最終のRO作業連番を取得する
            Dim roFinishJobSeqTable As TabletSmbCommonClassNumberValueDataTable = _
                ta.GetROSeqForFinish(svcinId)
            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END


            Dim roFinishJobSeq As New List(Of Decimal)
            For Each roJobSeqRow As TabletSmbCommonClassNumberValueRow In roFinishJobSeqTable
                roFinishJobSeq.Add(roJobSeqRow.COL1)
            Next
            '絞り込んだRo作業連番がない場合、戻る
            If roFinishJobSeq.Count = 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E. roFinishJobSeq.Count = 0 ", MethodBase.GetCurrentMethod.Name))
                Return
            End If

            'ROステータスを80に更新する
            ta.UpdateROStatusByJobDtlId(svcinId, _
                                        jobDtlId, _
                                        RostatusWaitForDelivery, _
                                        inUpdateDateTime, _
                                        inStaffCode, _
                                        inUpdateFunction, _
                                        Me.ConvertDecimalArrayToString(roFinishJobSeq))
        End Using

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End", MethodBase.GetCurrentMethod.Name))

    End Sub
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

#Region "作業中のチップの作業完了"
    ''' <summary>
    ''' 作業中のチップの作業を完了します。
    ''' </summary>
    ''' <param name="dtInChipEntity">チップエンティティ(in)</param>
    ''' <param name="rsltEndDateTimeNoSec">実績終了日時</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="dtNow">現在日時</param>
    ''' <param name="stallStartTime">営業開始時間</param>
    ''' <param name="stallEndTime">営業終了時間</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <remarks></remarks>
    Public Function ChipFinish(ByVal dtInChipEntity As TabletSmbCommonClassChipEntityDataTable, _
                               ByVal rsltEndDateTimeNoSec As Date, _
                               ByVal stallUseStatus As String, _
                               ByVal restFlg As String, _
                               ByVal dtNow As Date, _
                               ByVal stallStartTime As Date, _
                               ByVal stallEndTime As Date, _
                               ByVal staffCode As String, _
                               ByVal updateDate As Date, _
                               ByVal systemId As String) As TabletSmbCommonClassChipEntityRow

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. rsltEndDateTimeNoSec={1}, stallUseStatus={2}, restFlg={3}, dtNow={4}, staffCode={5}, updateDate={6}, systemId={7}" _
                    , MethodBase.GetCurrentMethod.Name, rsltEndDateTimeNoSec, stallUseStatus, restFlg, dtNow, staffCode, updateDate, systemId))

        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        Dim restFlgWork As String
        Using biz As New TabletSMBCommonClassBusinessLogic
            '休憩を自動判定する場合
            If biz.IsRestAutoJudge() Then
                restFlgWork = RestTimeGetFlgNoGetRest
            Else
                restFlgWork = restFlg
            End If
        End Using
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

        'サービス入庫共通．実績終了日時取得サービスを呼び出す
        Dim rsltEndDateTimeLocal As Date = GetRsltEndDateTime(dtInChipEntity(0).RSLT_START_DATETIME, rsltEndDateTimeNoSec, stallStartTime, stallEndTime)
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
        'Dim rsltWorkTime As Long = GetServiceWorkTime(dtInChipEntity(0).STALL_ID, dtInChipEntity(0).RSLT_START_DATETIME, rsltEndDateTimeNoSec, restFlg, stallStartTime, stallEndTime)
        Dim rsltWorkTime As Long = GetServiceWorkTime(dtInChipEntity(0).STALL_ID, dtInChipEntity(0).RSLT_START_DATETIME, rsltEndDateTimeNoSec, restFlgWork, stallStartTime, stallEndTime)
        '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
        Using dtOutChipEntity As New TabletSmbCommonClassChipEntityDataTable
            Dim drOutChipEntity As TabletSmbCommonClassChipEntityRow = CType(dtOutChipEntity.NewRow, TabletSmbCommonClassChipEntityRow)
            'ストール利用を更新する
            drOutChipEntity.RSLT_END_DATETIME = rsltEndDateTimeLocal
            drOutChipEntity.RSLT_WORKTIME = rsltWorkTime
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
            'drOutChipEntity.REST_FLG = restFlg
            drOutChipEntity.REST_FLG = restFlgWork
            '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END
            drOutChipEntity.STALL_USE_STATUS = stallUseStatus
            drOutChipEntity.UPDATE_DATETIME = dtNow
            drOutChipEntity.UPDATE_STF_CD = staffCode

            '処理対象のストール利用に紐付くスタッフ作業が存在する場合、スタッフ作業に実績終了日時を設定する
            Using ta As New TabletSMBCommonClassDataAdapter
                '実績終了日時を修正
                ta.UpdateStaffJobRsltDatetime(dtInChipEntity(0).JOB_ID, Date.MinValue, rsltEndDateTimeLocal, staffCode, systemId, updateDate)
            End Using
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E ", MethodBase.GetCurrentMethod.Name))
            Return drOutChipEntity
        End Using
    End Function

#End Region
#End Region

#Region "日跨ぎ終了処理"
    ' ''' <summary>
    ' ''' ストール利用を「日跨ぎ終了」へ更新します
    ' ''' </summary>
    ' ''' <param name="stallUseId">ストール利用ID</param>
    ' ''' <param name="rsltEndDateTime">実績終了日時</param>
    ' ''' <param name="stallStartTime">営業開始時間</param>
    ' ''' <param name="stallEndTime">営業終了時間</param>
    ' ''' <param name="updateDate">更新時間</param>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <param name="systemId">更新クラス</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function MidFinish(ByVal stallUseId As Decimal, _
    '                           ByVal rsltEndDateTime As Date, _
    '                           ByVal restFlg As String, _
    '                           ByVal objStaffContext As StaffContext, _
    '                           ByVal stallStartTime As Date, _
    '                           ByVal stallEndTime As Date, _
    '                           ByVal updateDate As Date, _
    '                           ByVal systemId As String, _
    '                           Optional ByVal scheStartDateTime As Date = Nothing, _
    '                           Optional ByVal scheWorkTime As Long = Nothing) As Long
    ''' <summary>
    ''' ストール利用を「日跨ぎ終了」へ更新します
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="rsltStartDateTime">実績開始日時</param>
    ''' <param name="rsltEndDateTime">実績終了日時</param>
    ''' <param name="stallStartTime">営業開始時間</param>
    ''' <param name="stallEndTime">営業終了時間</param>
    ''' <param name="updateDate">更新時間</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="systemId">更新クラス</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function MidFinish(ByVal svcinId As Decimal, _
                              ByVal stallUseId As Decimal, _
                              ByVal stallId As Decimal, _
                              ByVal rsltStartDateTime As Date, _
                              ByVal rsltEndDateTime As Date, _
                              ByVal restFlg As String, _
                              ByVal objStaffContext As StaffContext, _
                              ByVal stallStartTime As Date, _
                              ByVal stallEndTime As Date, _
                              ByVal updateDate As Date, _
                              ByVal systemId As String, _
                              ByVal rowLockVersion As Long, _
                              Optional ByVal scheStartDateTime As Date = Nothing, _
                              Optional ByVal scheWorkTime As Long = Nothing) As Long

        'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, rsltEndDateTime={2}, stallStartTime={3}, stallEndTime={4}" _
        '                , MethodBase.GetCurrentMethod.Name, stallUseId, rsltEndDateTime, stallStartTime, stallEndTime))
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, rsltEndDateTime={2}, stallStartTime={3}, stallEndTime={4}, scheStartDateTime={5}, scheWorkTime={6}" _
                        , MethodBase.GetCurrentMethod.Name, stallUseId, rsltEndDateTime, stallStartTime, stallEndTime, scheStartDateTime, scheWorkTime))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '開始時間から終了時間までの範囲に重複休憩エリアがあるか
        Dim workTime As Long = DateDiff("n", rsltStartDateTime, rsltEndDateTime)
        Dim hasRestTimeInServiceTime As Boolean = Me.HasRestTimeInServiceTime(stallStartTime, stallEndTime, stallId, rsltStartDateTime, workTime, True)
        '休憩と重複場合、
        If hasRestTimeInServiceTime Then
            '画面に重複で表示してない
            If IsNothing(restFlg) Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:OverlapError", System.Reflection.MethodBase.GetCurrentMethod.Name))
                Return TabletSMBCommonClassBusinessLogic.ActionResult.OverlapError
            End If
        End If

        'サービス入庫をロックして、チェックする
        Dim result As Long = Me.LockServiceInTable(svcinId, rowLockVersion, objStaffContext.Account, updateDate, systemId)
        If result <> TabletSMBCommonClassBusinessLogic.ActionResult.Success Then
            Me.Rollback = True
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:LockServiceInTableError", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return result
        End If
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        ' ステータス遷移可否をチェックする
        Dim rsltEndDateTimeNoSec As Date = DefaultDateTimeValueGet()
        If Not IsNothing(rsltEndDateTime) Then
            rsltEndDateTimeNoSec = GetDateTimeFloorSecond(rsltEndDateTime)
        End If

        '対象の情報を取得する
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId, 1)
        If dtChipEntity.Count <> 1 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                            , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.GetChipEntityError
        End If

        'restFlg設定してない場合、1に設定する
        If IsNothing(restFlg) Then
            restFlg = RestTimeGetFlgGetRest
        End If

        'ステータス遷移可否をチェックする
        If Not CanMidFinish(dtChipEntity(0).STALL_USE_STATUS) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
                    , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.CheckError
        End If

        '検査ステータスが1(検査依頼中)の場合、終了できない
        Dim inspectionStatus As String = dtChipEntity(0).INSPECTION_STATUS
        If inspectionStatus.Equals(InspectionApproval) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E InspectionStatusFinishError" _
                            , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.InspectionStatusFinishError
        End If

        'RO NOを紐付けてるかチェックする
        Dim roNum As String = dtChipEntity(0).RO_NUM
        If String.IsNullOrEmpty(roNum.Trim()) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0} NotSetroNoError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NotSetroNoError
        End If

        Dim rsltStartWorkingDate As Date = GetWorkingDate(dtChipEntity(0).RSLT_START_DATETIME, stallStartTime)
        Dim prmsEndWorkingDate As Date = GetWorkingDate(dtChipEntity(0).PRMS_END_DATETIME, stallStartTime)

        '実績開始日時と見込終了日時の営業日が異なること
        If rsltStartWorkingDate.CompareTo(prmsEndWorkingDate) = 0 Then
            Return ActionResult.ExceptionError
        End If

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発  START
        '指定ストールにテクニシャンいない場合
        Dim hasTechnicianInStall As Boolean = Me.HasTechnicianInStall(stallId, objStaffContext)
        If Not hasTechnicianInStall Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0} NoTechnicianError. ", System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ActionResult.NoTechnicianError
        End If

        '作業実績送信使用するフラグを取得する
        Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()

        '作業実績送信の場合、作業ステータスを取得する
        Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing
        If isUseJobDispatch Then
            prevJobStatus = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)
        End If

        '更新前のステータス取得
        Dim prevStatus As String = Me.JudgeChipStatus(stallUseId)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発  END

        'update用データセット
        Using targetDtChipEntity As New TabletSmbCommonClassChipEntityDataTable
            Dim targetDrTodayChipEntity As TabletSmbCommonClassChipEntityRow = CType(targetDtChipEntity.NewRow(), TabletSmbCommonClassChipEntityRow)
            Dim targetDrTomorrowChipEntity As TabletSmbCommonClassChipEntityRow = CType(targetDtChipEntity.NewRow(), TabletSmbCommonClassChipEntityRow)
            targetDrTodayChipEntity.SVCIN_ID = dtChipEntity(0).SVCIN_ID
            targetDrTodayChipEntity.STALL_USE_ID = dtChipEntity(0).STALL_USE_ID
            targetDrTomorrowChipEntity.SVCIN_ID = dtChipEntity(0).SVCIN_ID

            'チップの作業を完了する
            targetDrTodayChipEntity = ChipFinish(dtChipEntity, rsltEndDateTimeNoSec, StalluseStatusMidfinish, restFlg, updateDate, _
                                                    stallStartTime, stallEndTime, objStaffContext.Account, updateDate, systemId)

            'ストール利用を登録する
            targetDrTomorrowChipEntity = InsertMidFinishStallUse(dtChipEntity(0), targetDrTomorrowChipEntity, _
                                                                stallStartTime, stallEndTime, updateDate, objStaffContext.Account, scheStartDateTime, scheWorkTime)

            '2019/07/19 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'ストール使用不可チップと重複する場合
            If CheckStallUnavailableOverlapPosition(targetDrTomorrowChipEntity.SCHE_START_DATETIME, _
                                                    targetDrTomorrowChipEntity.SCHE_END_DATETIME, targetDrTomorrowChipEntity.STALL_ID) Then
                Return ActionResult.ChipOverlapUnavailableError
            End If
            '2019/07/19 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

            '更新処理
            Using ta As New TabletSMBCommonClassDataAdapter
                '自分以外のチップで、ストール利用ステータスが「03：完了」又は「05：中断」の
                'ストール利用の件数が1件以上の場合
                If ta.IsExistOtherFinishOrStop(objStaffContext.DlrCD, objStaffContext.BrnCD, dtChipEntity(0).SVCIN_ID, stallUseId) Then
                    'サービス入庫を「次の作業開始待ち」に更新する
                    targetDrTodayChipEntity.SVC_STATUS = SvcStatusNextStartWait
                Else
                    'サービス入庫を「作業開始待ち」に更新する
                    targetDrTodayChipEntity.SVC_STATUS = SvcStatusStartwait
                End If

                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発  START

                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
                ''作業実績テーブルを更新する(日跨ぎ終了の作業ステータスを「中断」、中断理由区分を「その他」に設定する)
                'Dim updateCount As Long = ta.UpdateJobRsltOnFinish(dtChipEntity(0).JOB_DTL_ID, _
                '                                                   rsltEndDateTimeNoSec, _
                '                                                   JobStatusStop, _
                '                                                   objStaffContext.Account, _
                '                                                   systemId, _
                '                                                   updateDate, _
                '                                                   StopReasonOthers)
                'If updateCount = 0 Then
                '    Logger.Error(String.Format(CultureInfo.InvariantCulture, _
                '                               "{0}.E ExceptionError:UpdateJobRsltOnFinish update count=0.", _
                '                               MethodBase.GetCurrentMethod.Name))
                '    Return ActionResult.ExceptionError
                'End If

                '作業実績テーブルを更新する(日跨ぎ終了の作業ステータスを「中断」、中断理由区分を「その他」に設定する)
                ta.UpdateJobRsltOnFinish(dtChipEntity(0).JOB_DTL_ID, _
                                         rsltEndDateTimeNoSec, _
                                         JobStatusStop, _
                                         objStaffContext.Account, _
                                         systemId, _
                                         updateDate, _
                                         StopReasonOthers)
                '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

                '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発  END

                Dim rtCnt As Long = 0
                'サービス入庫ステータスを更新
                rtCnt = ta.UpdateSvcinStatus(dtChipEntity(0).SVCIN_ID, targetDrTodayChipEntity.SVC_STATUS, updateDate, objStaffContext.Account)
                If rtCnt <> 1 Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Failed to update TB_T_SERVICEIN. STALL_USE_ID={1}, STALL_USE_STATUS={2}" _
                        , MethodBase.GetCurrentMethod.Name, targetDrTodayChipEntity.STALL_USE_ID, targetDrTodayChipEntity.STALL_USE_STATUS))
                    Return ActionResult.ExceptionError
                End If

                '今日のチップが終了
                targetDrTodayChipEntity.STALL_USE_ID = dtChipEntity(0).STALL_USE_ID

                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 START
                'rtCnt = ta.UpdateStallUseRsltEndDate(targetDrTodayChipEntity, systemId)

                Using dealerEnvBiz As New ServiceCommonClassBusinessLogic
                    '休憩取得自動判定フラグ
                    Dim autoJudgeFlg = String.Empty
                    autoJudgeFlg = dealerEnvBiz.GetDlrSystemSettingValueBySettingName(RestAutoJudgeFlg)

                    rtCnt = ta.UpdateStallUseRsltEndDate(targetDrTodayChipEntity, systemId, autoJudgeFlg)
                End Using
                '2019/12/06 NSK 皆川 (トライ店システム評価)整備作業における休憩時間取得判定向上検証 END

                If rtCnt <> 1 Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Failed to update TB_T_STALL_USE. STALL_USE_ID={1}, STALL_USE_STATUS={2}" _
                        , MethodBase.GetCurrentMethod.Name, targetDrTodayChipEntity.STALL_USE_ID, targetDrTodayChipEntity.STALL_USE_STATUS))
                    Return ActionResult.ExceptionError
                End If

                '明日の日跨ぎ記録を追加する
                rtCnt = ta.InsertStallUse(targetDrTomorrowChipEntity, systemId)
                If rtCnt <> 1 Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Failed to insert into TB_T_STALL_USE. " _
                        , MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.ExceptionError
                End If

            End Using
        End Using

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '更新後のステータス取得
        Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)

        '基幹側にステータス情報を送信
        Using ic3802601blc As New IC3802601BusinessLogic
            'ステータス連携実施
            Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(dtChipEntity(0).SVCIN_ID, _
                                                                    dtChipEntity(0).JOB_DTL_ID, _
                                                                    stallUseId, _
                                                                    prevStatus, _
                                                                    crntStatus, _
                                                                    0)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If dmsSendResult <> 0 Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.End SendStatusInfo FAILURE " _
            '                               , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '処理結果チェック
            If dmsSendResult = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using

        '実績送信使用の場合
        If isUseJobDispatch Then
            '作業ステータスを取得する
            Dim crntJobStatus As IC3802701JobStatusDataTable = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)

            '基幹側にJobDispatch実績情報を送信
            Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(dtChipEntity(0).SVCIN_ID, _
                                                                   dtChipEntity(0).JOB_DTL_ID, _
                                                                   prevJobStatus, _
                                                                   crntJobStatus)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If resultSendJobClock <> ActionResult.Success Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.End DmsLinkageError:SendJobClockOnInfo FAILURE " _
            '                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '処理結果チェック
            If resultSendJobClock = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf resultSendJobClock = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendJobClockOnInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End If

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.End", MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    ''' <summary>
    ''' 日跨ぎ終了の場合、翌日の部分が新しいストール利用を登録する()
    ''' </summary>
    ''' <param name="dtChipEntity">チップ情報</param>
    ''' <param name="targetDrChipEntity">戻る用テーブル</param>
    ''' <param name="stallStartTime">営業開始時間</param>
    ''' <param name="stallEndTime">営業終了時間</param>
    ''' <param name="dtNow">現在日時</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="scheStartDateTime">開始予定日時</param>
    ''' <param name="scheWorkTime">予定作業時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InsertMidFinishStallUse(ByVal dtChipEntity As TabletSmbCommonClassChipEntityRow, ByVal targetDrChipEntity As TabletSmbCommonClassChipEntityRow, _
                                             ByVal stallStartTime As Date, ByVal stallEndTime As Date, ByVal dtNow As Date, ByVal staffCode As String, _
                                             Optional ByVal scheStartDateTime As Date = Nothing, Optional ByVal scheWorkTime As Long = Nothing) As TabletSmbCommonClassChipEntityRow

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. dtNow={1}, staffCode={2}, stallStartTime={3}, stallEndTime={4}" _
                    , MethodBase.GetCurrentMethod.Name, dtNow, staffCode, stallStartTime, stallEndTime))
        '翌営業日の営業開始日時を取得する
        Dim nextWorkingStartDateTime As Date = GetScheStartDateTime(dtChipEntity.STALL_ID, dtChipEntity.RSLT_START_DATETIME, stallStartTime)

        Dim scheStartDateTimeLocal As Date = Date.MinValue

        '入力データ．予定開始日時がnull以外の場合
        If scheStartDateTime <> CType(Nothing, Date) Then
            ' 入力データ．予定開始日時を予定開始日時とする。
            scheStartDateTimeLocal = CType(scheStartDateTime, Date)
        Else
            scheStartDateTimeLocal = nextWorkingStartDateTime
        End If

        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        '作業開始日時を取得する
        scheStartDateTimeLocal = GetServiceStartDateTime(dtChipEntity.STALL_ID, scheStartDateTimeLocal, _
                                                         stallStartTime, stallEndTime, dtChipEntity.REST_FLG, True)
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        Dim scheWorkTimeLocal As Long = 0
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        'Dim scheEndDateTimeLocal As Date = dtChipEntity.PRMS_END_DATETIME
        Dim scheEndDateTimeDataLocal As New ServiceEndDateTimeData
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        '入力データ．予定開始日時が0の場合
        If scheWorkTime = 0 Then
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'scheWorkTimeLocal = GetServiceWorkTime(dtChipEntity.STALL_ID, nextWorkingStartDateTime, dtChipEntity.PRMS_END_DATETIME, dtChipEntity.REST_FLG, stallStartTime, stallEndTime)
            scheWorkTimeLocal = GetServiceWorkTime(dtChipEntity.STALL_ID, scheStartDateTimeLocal, dtChipEntity.PRMS_END_DATETIME, dtChipEntity.REST_FLG, stallStartTime, stallEndTime)
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
        Else
            ' 入力データ．予定開始日時を予定開始日時とする。
            scheWorkTimeLocal = scheWorkTime
        End If

        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        '' 入力データ．予定開始日時がnull以外 または 入力データ．予定作業時間がnull以外の場合
        'If scheStartDateTime <> CType(Nothing, Date) OrElse scheWorkTime <> 0 Then
        '    scheEndDateTimeLocal = GetServiceEndDateTime(dtChipEntity.STALL_ID, scheStartDateTimeLocal, scheWorkTimeLocal, stallStartTime, stallEndTime, dtChipEntity.REST_FLG)
        'End If
        scheEndDateTimeDataLocal = GetServiceEndDateTime(dtChipEntity.STALL_ID, scheStartDateTimeLocal, scheWorkTimeLocal, _
                                                              stallStartTime, stallEndTime, dtChipEntity.REST_FLG)
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        Dim stallUseId As Decimal = 0
        Using ta As New TabletSMBCommonClassDataAdapter
            'ストール利用IDの採番値を取得する
            stallUseId = ta.GetSequenceNextVal(StallUseIdSeq)
        End Using
        targetDrChipEntity.STALL_USE_ID = stallUseId
        targetDrChipEntity.JOB_DTL_ID = dtChipEntity.JOB_DTL_ID
        targetDrChipEntity.DLR_CD = dtChipEntity.DLR_CD
        targetDrChipEntity.BRN_CD = dtChipEntity.BRN_CD
        targetDrChipEntity.STALL_ID = dtChipEntity.STALL_ID
        targetDrChipEntity.TEMP_FLG = dtChipEntity.TEMP_FLG
        targetDrChipEntity.PARTS_FLG = dtChipEntity.PARTS_FLG
        targetDrChipEntity.STALL_USE_STATUS = StalluseStatusStartWait
        targetDrChipEntity.SCHE_START_DATETIME = scheStartDateTimeLocal
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        'targetDrChipEntity.SCHE_END_DATETIME = scheEndDateTimeLocal
        targetDrChipEntity.SCHE_END_DATETIME = scheEndDateTimeDataLocal.ServiceEndDateTime
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
        targetDrChipEntity.SCHE_WORKTIME = scheWorkTimeLocal
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        'targetDrChipEntity.REST_FLG = dtChipEntity.REST_FLG
        targetDrChipEntity.REST_FLG = scheEndDateTimeDataLocal.RestFlg
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
        targetDrChipEntity.RSLT_START_DATETIME = DefaultDateTimeValueGet()
        targetDrChipEntity.PRMS_END_DATETIME = DefaultDateTimeValueGet()
        targetDrChipEntity.RSLT_END_DATETIME = DefaultDateTimeValueGet()
        targetDrChipEntity.RSLT_WORKTIME = 0
        targetDrChipEntity.JOB_ID = 0
        targetDrChipEntity.STOP_REASON_TYPE = dtChipEntity.STOP_REASON_TYPE
        targetDrChipEntity.STOP_MEMO = dtChipEntity.STOP_MEMO
        targetDrChipEntity.STALL_IDLE_ID = 0
        targetDrChipEntity.UPDATE_DATETIME = dtNow
        targetDrChipEntity.UPDATE_STF_CD = staffCode

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E stallUseId={1}", MethodBase.GetCurrentMethod.Name, stallUseId))
        Return targetDrChipEntity
    End Function

    ''' <summary>
    ''' 予定開始日時を取得します
    ''' </summary>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="rsltStartTime">実績終了日</param>
    ''' <param name="stallStartTime">営業開始時間</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetScheStartDateTime(ByVal stallId As Decimal, ByVal rsltStartTime As Date, ByVal stallStartTime As Date) As Date
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallId={1}, rsltStartTime={2}" _
                    , MethodBase.GetCurrentMethod.Name, stallId, rsltStartTime))

        'サービス入庫共通．共通処理_営業日取得を呼び出し、営業日を取得する
        Dim startWorkingDate As Date = GetWorkingDate(rsltStartTime, stallStartTime)

        '非稼働日フラグを「true：非稼働日」で初期化する
        Dim isStallIdleDay As Boolean = True

        While isStallIdleDay
            '営業日を加算する
            startWorkingDate = startWorkingDate.AddDays(1)

            Using ta As New TabletSMBCommonClassDataAdapter
                'ストール非稼働マスタ．非稼働日判定サービスを呼び出す
                isStallIdleDay = ta.IsStallIdleDay(startWorkingDate, startWorkingDate, stallId)
            End Using
        End While

        Dim scheStartDateTimeLocal As Date = New Date(startWorkingDate.Year, startWorkingDate.Month, startWorkingDate.Day, _
                                                      stallStartTime.Hour, stallStartTime.Minute, 0)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E scheStartDateTime={1}", MethodBase.GetCurrentMethod.Name, scheStartDateTimeLocal))
        Return scheStartDateTimeLocal
    End Function
#End Region

#Region "洗車開始処理"

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 洗車開始処理
    ' ''' </summary>
    ' ''' <param name="inServiceInId">サービス入庫ID</param>
    ' ''' <param name="inRONum">RO番号</param>
    ' ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ' ''' <param name="inSystem">プログラムID</param>
    ' ''' <returns>戻り値「0：正常終了、0以外：エラー」</returns>
    ' ''' <remarks></remarks>
    ''<EnableCommit()>
    'Public Function UpdateChipWashStart(ByVal inServiceInId As Long, _
    '                            ByVal inRONum As String, _
    '                            ByVal inRowLockVersion As Long, _
    '                            ByVal inSystem As String) As Long

    ''' <summary>
    ''' 洗車開始処理
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール使用ID</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function UpdateChipWashStart(ByVal inServiceInId As Decimal, _
                                        ByVal inJobDtlId As Decimal, _
                                        ByVal inStallUseId As Decimal, _
                                        ByVal inRowLockVersion As Long, _
                                        ByVal inSystem As String) As Long
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Dim returnCode As Long = 0

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Dim updateCount As Long = 0
        Dim userInfo As StaffContext = StaffContext.Current
        Dim inAccount As String = userInfo.Account
        Dim inDlrCode As String = userInfo.DlrCD
        Dim inNowDate As Date = DateTimeFunc.Now(inDlrCode)
        Dim inDropNowDate As Date = GetDateTimeFloorSecond(inNowDate)

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim operationFlg As Boolean = CheckAddWorkStatus(inRONum)
        'If Not operationFlg Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} ADDWORKSTATUS CHECK FAILURE " _
        '                    , Me.GetType.ToString _
        '                    , MethodBase.GetCurrentMethod.Name))
        '    Me.Rollback = True
        '    Return ActionResult.CheckAddWorkError
        'End If
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'returnCode = LockServiceInTable(inServiceInId, inRowLockVersion, inAccount, inNowDate, inSystem)
        'If returnCode <> 0 Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} TABLELOCK FAILURE " _
        '                , Me.GetType.ToString _
        '                , MethodBase.GetCurrentMethod.Name))
        '    Me.Rollback = True
        '    Return returnCode
        'End If

        'サービス入庫テーブルロック処理
        Dim returnCodeServiceinLock As Long = Me.LockServiceInTable(inServiceInId, _
                                                                    inRowLockVersion, _
                                                                    inAccount, _
                                                                    inNowDate, _
                                                                    inSystem)

        '処理結果チェック
        If returnCodeServiceinLock <> 0 Then
            'ロックに失敗した場合
            'ロールバックフラグを「True」にしてロックのエラーコードを返却
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[TABLELOCK FAILURE]" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , returnCodeServiceinLock))

            Me.Rollback = True

            Return returnCodeServiceinLock

        End If

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        '更新前のステータス取得
        Dim prevStatus As String = Me.JudgeChipStatus(inStallUseId)
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        Using da As New TabletSMBCommonClassDataAdapter
            '洗車開始DB処理
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'updateCount = da.UpdateServiceinWashStart(inServiceInId, _
            '                                inAccount, _
            '                                inNowDate)
            updateCount = da.UpdateServiceinWashCar(inServiceInId, _
                                                        SvcStatusCarWashWait, _
                                                        SvcStatusCarWashStart, _
                                                        inAccount, _
                                                        inNowDate)
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            If updateCount = 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} UPDATE FAILURE " _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return ActionResult.ExceptionError
            Else
                '洗車実績ID取得
                Dim inCarWashRsltId As Decimal = da.GetSequenceNextVal(CarwashRsltIdSeq)
                '洗車実績登録
                updateCount = da.InsertCarWashResult(inServiceInId, _
                                                        inCarWashRsltId, _
                                                        inDropNowDate, _
                                                        inNowDate, _
                                                        inSystem, _
                                                        inAccount, _
                                                        DefaultDateTimeValueGet())
                If updateCount = 0 Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} ERROR UPDATECOUNT = 0 SC3500400StallInfoDataTableAdapter.InsertCarWashResult " _
                                , Me.GetType.ToString _
                                , MethodBase.GetCurrentMethod.Name))
                    Me.Rollback = True
                    Return ActionResult.ExceptionError
                End If

                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
                '更新後のステータス取得
                Dim crntStatus As String = Me.JudgeChipStatus(inStallUseId)
                'ステータス送信
                Using ic3802601blc As New IC3802601BusinessLogic
                    'ステータス連携実施
                    Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(inServiceInId, _
                                                                            inJobDtlId, _
                                                                            inStallUseId, _
                                                                            prevStatus, _
                                                                            crntStatus, _
                                                                            0)

                    '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                    'If dmsSendResult <> 0 Then
                    '    Me.Rollback = True
                    '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
                    '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                    '    Return ActionResult.DmsLinkageError
                    'End If

                    '処理結果チェック
                    If dmsSendResult = ActionResult.Success Then
                        '「0：成功」の場合
                        '処理なし

                    ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                        '「-9000：DMS除外エラーの警告」の場合
                        '戻り値に「-9000：DMS除外エラーの警告」を設定
                        returnCode = ActionResult.WarningOmitDmsError

                    Else
                        '上記以外の場合
                        '「15：他システムとの連携エラー」を返却
                        Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ActionResult.DmsLinkageError))
                        Me.Rollback = True
                        Return ActionResult.DmsLinkageError

                    End If

                    '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                End Using
                '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

            End If

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE:{2} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , returnCode))
        Return returnCode
    End Function

#End Region

#Region "洗車終了処理"

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 洗車終了処理
    ' ''' </summary>
    ' ''' <param name="inServiceInId">サービス入庫ID</param>
    ' ''' <param name="inPickDeliType">引取納車区分</param>
    ' ''' <param name="inRONum">RO番号</param>
    ' ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ' ''' <param name="inSystem">プログラムID</param>
    ' ''' <returns>戻り値「0：正常終了、0以外：エラー」</returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function UpdateChipWashEnd(ByVal inServiceInId As Decimal, _
    '                          ByVal inPickDeliType As String, _
    '                          ByVal inRONum As String, _
    '                          ByVal inRowLockVersion As Long, _
    '                          ByVal inSystem As String) As Long

    ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
    ' ''' <summary>
    ' ''' 洗車終了処理
    ' ''' </summary>
    ' ''' <param name="inServiceInId">サービス入庫ID</param>
    ' ''' <param name="inJobDtlId">作業内容ID</param>
    ' ''' <param name="inStallUseId">ストール利用ID</param>
    ' ''' <param name="inPickDeliType">引取納車区分</param>
    ' ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ' ''' <param name="inSystem">プログラムID</param>
    ' ''' <returns>戻り値「0：正常終了、0以外：エラー」</returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function UpdateChipWashEnd(ByVal inServiceInId As Decimal, _
    '                                  ByVal inJobDtlId As Decimal, _
    '                                  ByVal inStallUseId As Decimal, _
    '                                  ByVal inPickDeliType As String, _
    '                                  ByVal inRowLockVersion As Long, _
    '                                  ByVal inSystem As String) As Long
    ''' <summary>
    ''' 洗車終了処理
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inPickDeliType">引取納車区分</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <param name="inRONum">RO番号</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function UpdateChipWashEnd(ByVal inServiceInId As Decimal, _
                                      ByVal inJobDtlId As Decimal, _
                                      ByVal inStallUseId As Decimal, _
                                      ByVal inPickDeliType As String, _
                                      ByVal inRowLockVersion As Long, _
                                      ByVal inSystem As String, _
                                      ByVal inRONum As String) As Long
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} START " _
        '    , Me.GetType.ToString _
        '    , MethodBase.GetCurrentMethod.Name))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START inServiceInId={2}, inJobDtlId={3}, inStallUseId={4}, inPickDeliType={5}, inRowLockVersion={6}, inRONum={7}, inSystem={8}" _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name _
            , inServiceInId _
            , inJobDtlId _
            , inStallUseId _
            , inPickDeliType _
            , inRowLockVersion _
            , inRONum _
            , inSystem))
        '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Dim returnCode As Long = 0
        'Dim updateCount As Long = 0
        'Dim userInfo As StaffContext = StaffContext.Current
        'Dim inAccount As String = userInfo.Account
        'Dim inDlrCode As String = userInfo.DlrCD
        'Dim inNowDate As Date = DateTimeFunc.Now(inDlrCode)
        'Dim inDropNowDate As Date = GetDateTimeFloorSecond(inNowDate)

        ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
        ''納車実績日時を取得
        'Dim rsltDeliDateTime As Date = GetRsltDeliDate(inServiceInId)

        'If rsltDeliDateTime = Nothing Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} UPDATE FAILURE " _
        '    , Me.GetType.ToString _
        '    , MethodBase.GetCurrentMethod.Name))
        '    Me.Rollback = True
        '    Return ActionResult.ExceptionError
        'End If

        ''納車実績日時がデフォルトでなければ、既に納車済みと判断する
        'Dim deliveredFlg As Boolean = Not IsDefaultValue(rsltDeliDateTime)
        ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

        ''2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ''Dim operationFlg As Boolean = CheckAddWorkStatus(inRONum)
        ''If Not operationFlg Then
        ''    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        ''                    , "{0}.{1} ADDWORKSTATUS CHECK FAILURE " _
        ''                    , Me.GetType.ToString _
        ''                    , MethodBase.GetCurrentMethod.Name))
        ''    Me.Rollback = True
        ''    Return ActionResult.CheckAddWorkError
        ''End If
        ''2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        'Dim inSvcStatus As String = SvcStatusWaitingCustomer

        ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
        ''If inPickDeliType = DeliTypeWaiting Then
        ''    inSvcStatus = SvcStatusWaitingCustomer
        ''ElseIf inPickDeliType = DeliTypeDropOff Then
        ''    inSvcStatus = SvcStatusDropOffCustomer
        ''End If

        'If deliveredFlg Then
        '    inSvcStatus = SvcStatusDelivery
        'Else
        '    If inPickDeliType = DeliTypeWaiting Then
        '        inSvcStatus = SvcStatusWaitingCustomer
        '    ElseIf inPickDeliType = DeliTypeDropOff Then
        '        inSvcStatus = SvcStatusDropOffCustomer
        '    End If
        'End If
        ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

        ''サービス入庫テーブルロック処理
        'returnCode = LockServiceInTable(inServiceInId, inRowLockVersion, inAccount, inNowDate, inSystem)
        'If returnCode <> 0 Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} TABLELOCK FAILURE " _
        '                , Me.GetType.ToString _
        '                , MethodBase.GetCurrentMethod.Name))
        '    Me.Rollback = True
        '    Return returnCode
        'End If

        ''2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ''更新前のステータス取得
        'Dim prevStatus As String = Me.JudgeChipStatus(inStallUseId)
        ''2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        'Using da As New TabletSMBCommonClassDataAdapter
        '    updateCount = da.UpdateServiceWorkWashEnd(inServiceInId, _
        '                                                inSvcStatus)
        '    If updateCount = 0 Then
        '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} UPDATE FAILURE " _
        '                    , Me.GetType.ToString _
        '                    , MethodBase.GetCurrentMethod.Name))
        '        Me.Rollback = True
        '        Return ActionResult.ExceptionError
        '    Else
        '        updateCount = da.UpdateCarWashResult(inServiceInId, inDropNowDate, inNowDate, inAccount, inSystem)
        '        If updateCount = 0 Then
        '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                        , "{0}.{1} ERROR UPDATECOUNT = 0 " _
        '                        , Me.GetType.ToString _
        '                        , MethodBase.GetCurrentMethod.Name))
        '            Me.Rollback = True
        '            Return ActionResult.ExceptionError
        '        End If
        '    End If

        '    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
        '    If deliveredFlg Then
        '        '指定サービス入庫IDのROステータスを「納車済み：90」に更新
        '        updateCount = da.UpdateROStatusBySvcinId(RostatusDelivered, _
        '                                                 inServiceInId, _
        '                                                 inNowDate, _
        '                                                 inAccount, _
        '                                                 inSystem,
        '                                                 inRONum)


        '        If updateCount = 0 Then
        '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                        , "{0}.{1} RO_STATUS UPDATE FAILURE " _
        '                        , Me.GetType.ToString _
        '                        , MethodBase.GetCurrentMethod.Name))
        '            Me.Rollback = True
        '            Return ActionResult.ExceptionError
        '        End If
        '    End If
        '    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

        '    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        '    '更新後のステータス取得
        '    Dim crntStatus As String = Me.JudgeChipStatus(inStallUseId)

        '    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
        '    If Not deliveredFlg Then
        '        '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

        '        'ステータス送信
        '        Using ic3802601blc As New IC3802601BusinessLogic
        '            Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(inServiceInId, _
        '                                                                    inJobDtlId, _
        '                                                                    inStallUseId, _
        '                                                                    prevStatus, _
        '                                                                    crntStatus, _
        '                                                                    0)
        '            If dmsSendResult <> 0 Then
        '                Me.Rollback = True
        '                Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
        '                            , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
        '                Return ActionResult.DmsLinkageError
        '            End If
        '        End Using

        '        '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
        '    End If
        '    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

        'End Using
        ''2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        'ログイン情報を取得
        Dim userInfo As StaffContext = StaffContext.Current

        '現在日時と現在日時(秒切捨て)を取得
        Dim inNowDate As Date = DateTimeFunc.Now(userInfo.DlrCD)
        Dim inDropNowDate As Date = GetDateTimeFloorSecond(inNowDate)

        '更新後サービス入庫ステータスを設定
        Dim inSvcStatus As String = SvcStatusDropOffCustomer

        '引き取り納車区分のチェック
        If inPickDeliType = DeliTypeWaiting Then
            '「0：Waiting」の場合
            '「12：納車待ち」を設定
            inSvcStatus = SvcStatusWaitingCustomer

        ElseIf inPickDeliType = DeliTypeDropOff Then
            '「4：DropOff」の場合
            '「11：預かり中」を設定
            inSvcStatus = SvcStatusDropOffCustomer

        End If

        'サービス入庫テーブルロック処理
        Dim returnCodeLockServiceInTable As Long = LockServiceInTable(inServiceInId, _
                                                                      inRowLockVersion, _
                                                                      userInfo.Account, _
                                                                      inNowDate, _
                                                                      inSystem)

        '処理結果チェック
        If returnCodeLockServiceInTable <> 0 Then
            '失敗した場合
            'ロック時の値を返却
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END RETURNCODE={2}[LockServiceInTable FAILURE]" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , returnCodeLockServiceInTable))
            Me.Rollback = True
            Return returnCodeLockServiceInTable

        End If

        '更新前のステータス取得
        Dim prevStatus As String = Me.JudgeChipStatus(inStallUseId)

        Using bizServiceCommonClass As New ServiceCommonClassBusinessLogic
            
            'サービスDMS納車実績ワークを取得
            Dim dtWorkServiceDmsResultDelivery As ServiceCommonClassDataSet.WorkServiceDmsResultDeliveryDataTable = _
                bizServiceCommonClass.GetWorkServiceDmsResultDelivery(inServiceInId)

            '取得結果のチェック
            If Not (IsNothing(dtWorkServiceDmsResultDelivery)) AndAlso 0 < dtWorkServiceDmsResultDelivery.Count Then
                '取得できた場合
                '強制納車処理を実施
                Dim returnCodeForceDeliverd As Integer = Me.ForceDeliverd(userInfo.DlrCD,
                                                                          userInfo.BrnCD, _
                                                                          inServiceInId, _
                                                                          inRONum, _
                                                                          dtWorkServiceDmsResultDelivery(0).DMS_RSLT_DELI_DATETIME, _
                                                                          userInfo.Account, _
                                                                          inNowDate, _
                                                                          inSystem)

                '処理結果チェック
                If returnCodeForceDeliverd <> ActionResult.Success Then
                    '「0：成功」以外の場合
                    '「22：予期せぬエラー」を返却
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[ForceDeliverd FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.ExceptionError))
                    Me.Rollback = True
                    Return ActionResult.ExceptionError

                End If

            Else
                '上記以外の場合
                '洗車終了処理を実施
                Using da As New TabletSMBCommonClassDataAdapter

                    'サービス入庫テーブルを更新する
                    Dim updateCountUpdateServiceWorkWashEnd As Long = _
                        da.UpdateServiceWorkWashEnd(inServiceInId, _
                                                    inSvcStatus)

                    '処理結果チェック
                    If updateCountUpdateServiceWorkWashEnd = 0 Then
                        '更新できなかった場合
                        '「22:予期せぬエラー」を返却
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[UpdateServiceWorkWashEnd FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ActionResult.ExceptionError))
                        Me.Rollback = True
                        Return ActionResult.ExceptionError

                    End If

                    '洗車実績テーブルの更新
                    Dim updateCountUpdateCarWashResult As Long = da.UpdateCarWashResult(inServiceInId, _
                                                                                        inDropNowDate, _
                                                                                        inNowDate, _
                                                                                        userInfo.Account, _
                                                                                        inSystem)

                    '処理結果チェック
                    If updateCountUpdateCarWashResult = 0 Then
                        '更新できなかった場合
                        '「22:予期せぬエラー」を返却
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[UpdateCarWashResult FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ActionResult.ExceptionError))
                        Me.Rollback = True
                        Return ActionResult.ExceptionError

                    End If

                    '更新後のステータス取得
                    Dim crntStatus As String = Me.JudgeChipStatus(inStallUseId)

                    'ステータス送信
                    Using ic3802601blc As New IC3802601BusinessLogic
                        'ステータス連携実施
                        Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(inServiceInId, _
                                                                                inJobDtlId, _
                                                                                inStallUseId, _
                                                                                prevStatus, _
                                                                                crntStatus, _
                                                                                0)

                        '処理結果チェック
                        If dmsSendResult = ActionResult.Success Then
                            '「0：成功」の場合
                            '処理なし

                        ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                            '「-9000：DMS除外エラーの警告」の場合
                            '戻り値に「-9000：DMS除外エラーの警告」を設定
                            returnCode = ActionResult.WarningOmitDmsError

                        Else
                            '上記以外の場合
                            '「15：他システムとの連携エラー」を返却
                            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ActionResult.DmsLinkageError))
                            Me.Rollback = True
                            Return ActionResult.DmsLinkageError

                        End If

                    End Using

                End Using

            End If

        End Using

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE:{2} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , returnCode))
        Return returnCode
    End Function

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START

    ''' <summary>
    ''' 洗車終了の通知・PUSH処理
    ''' </summary>
    ''' <param name="dtNoticeInfo">通知情報テーブル</param>
    ''' <param name="userInfo">スタフ情報</param>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成
    ''' </history>
    Public Sub WashEndNoticePush(ByVal dtNoticeInfo As TabletSmbCommonClassNoticeInfoDataTable, _
                              ByVal userInfo As StaffContext)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START " _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))

        '通知対象ユーザー
        Dim noticeUsersList As New List(Of String)
        If dtNoticeInfo.Count > 0 Then
            Dim drNoticeInfo As TabletSmbCommonClassNoticeInfoRow = dtNoticeInfo(0)
            '通知対象のユーザー
            '2016/06/29 NSK 皆川 TR-SVT-TMT-20160512-001 SA1はチップを作成していないのに、通知を受け取った START
            'If Not String.IsNullOrEmpty(drNoticeInfo.PIC_SA_STF_CD) AndAlso _
            '    Not userInfo.Account.Equals(drNoticeInfo.PIC_SA_STF_CD) Then
            If Not String.IsNullOrEmpty(drNoticeInfo.PIC_SA_STF_CD) AndAlso Not userInfo.Account.Equals(drNoticeInfo.PIC_SA_STF_CD) AndAlso _
                Not String.IsNullOrEmpty(drNoticeInfo.SAChipID) AndAlso Not drNoticeInfo.SAChipID.Equals(DefaultNumberValue.ToString()) Then
                '2016/06/29 NSK 皆川 TR-SVT-TMT-20160512-001 SA1はチップを作成していないのに、通知を受け取った END

                Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapRow = Me.GetDmsDlrBrnCode(userInfo.DlrCD, userInfo.BrnCD, userInfo.Account)
                If Not IsNothing(dmsDlrBrnTable) Then
                    '通知対象はない場合は通知しない
                    noticeUsersList.Add(drNoticeInfo.PIC_SA_STF_CD)
                    '基幹販売店コード
                    drNoticeInfo.DearlerCode = dmsDlrBrnTable.CODE1
                    '基幹店舗コード
                    drNoticeInfo.BranchCode = dmsDlrBrnTable.CODE2
                    'ログインユーザー
                    drNoticeInfo.LoginUserID = dmsDlrBrnTable.ACCOUNT
                    '枝番「0」で固定
                    drNoticeInfo.SEQ_NO = "0"

                    '顧客名敬称付く
                    If Not drNoticeInfo.IsNAMETITLE_NAMENull AndAlso _
                        Not String.IsNullOrEmpty(drNoticeInfo.NAMETITLE_NAME) Then
                        Dim cstName As New StringBuilder
                        If Position_Type_Before.Equals(drNoticeInfo.POSITION_TYPE) Then
                            cstName.Append(drNoticeInfo.NAMETITLE_NAME)
                            cstName.Append(drNoticeInfo.CST_NAME)
                        Else
                            cstName.Append(drNoticeInfo.CST_NAME)
                            cstName.Append(drNoticeInfo.NAMETITLE_NAME)
                        End If
                        drNoticeInfo.CST_NAME = cstName.ToString
                    End If

                    '通知のメッセージを作成する
                    Dim noticeMessage As New StringBuilder
                    noticeMessage.Append(WebWordUtility.GetWord(ProgramId_SubChipBox, 9))
                    noticeMessage.Append(Space(3))
                    noticeMessage.Append(drNoticeInfo.R_O)
                    noticeMessage.Append(Space(3))
                    noticeMessage.Append(drNoticeInfo.VCLREGNO)
                    noticeMessage.Append(Space(3))
                    noticeMessage.Append(drNoticeInfo.CST_NAME)

                    drNoticeInfo.Message = noticeMessage.ToString


                    '送り先リストを作成
                    Dim toAccountList As New List(Of String)
                    If Not drNoticeInfo.IsPIC_SA_STF_CDNull AndAlso _
                       Not userInfo.Account.Equals(drNoticeInfo.PIC_SA_STF_CD) Then
                        toAccountList.Add(drNoticeInfo.PIC_SA_STF_CD)
                    End If
                    '通知共通関数を呼ぶ
                    Notice(toAccountList, userInfo, drNoticeInfo)
                End If
            End If
        End If

        'Push
        Dim pushUsersList As New List(Of String)
        Dim operationCodeList As New List(Of Decimal)
        Dim exceptStaffCodeList As New List(Of String)
        'CTとCHT権限を追加
        operationCodeList.Add(Operation.CT)
        operationCodeList.Add(Operation.CHT)
        '自分を除外する
        exceptStaffCodeList.Add(userInfo.Account)
        '自分以外のCTとCHT権限のユーザーを取得
        pushUsersList = Me.GetSendStaffCode(userInfo.DlrCD, userInfo.BrnCD, operationCodeList, exceptStaffCodeList)

        'ユーザーリストに対してPUSHする
        SendPushByStaffCodeList(pushUsersList, PUSH_FuntionTabletSMB)

        '通知対象ユーザーリストに対してPUSHする
        If noticeUsersList.Count > 0 Then
            SendPushByStaffCodeList(noticeUsersList, PUSH_FuntionSA)
        End If

        '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

        'CW権限にPUSHする
        Me.SendAllCWPush(userInfo.DlrCD, userInfo.BrnCD, userInfo.Account)

        '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} " _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))
    End Sub

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

#End Region

#Region "納車処理"

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START

    ' ''' <summary>
    ' ''' 納車処理
    ' ''' </summary>
    ' ''' <param name="inServiceInId">サービス入庫ID</param>
    ' ''' <param name="inRONum">RO番号</param>
    ' ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ' ''' <param name="inSystem">プログラムID</param>
    ' ''' <returns>戻り値「0：正常終了、0以外：エラー」</returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function UpdateChipDelivery(ByVal inServiceInId As Decimal, _
    '                           ByVal inRONum As String, _
    '                           ByVal inRowLockVersion As Long, _
    '                           ByVal inSystem As String) As Long

    ''' <summary>
    ''' 納車処理
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール使用ID</param>
    ''' <param name="inRONum">RO番号</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function UpdateChipDelivery(ByVal inServiceInId As Decimal, _
                                       ByVal inJobDtlId As Decimal, _
                                       ByVal inStallUseId As Decimal, _
                                       ByVal inRONum As String, _
                                       ByVal inRowLockVersion As Long, _
                                       ByVal inSystem As String) As Long

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} START " _
                                , Me.GetType.ToString _
                                , MethodBase.GetCurrentMethod.Name))

        'If Not CheckInvoicePrintDateTime(inServiceInId) Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '        , "{0}.{1} InvoicePrintDateTime CHECK FAILURE " _
        '        , Me.GetType.ToString _
        '        , MethodBase.GetCurrentMethod.Name))
        '    Me.Rollback = True
        '    Return ActionResult.CheckInvoicePrintDateTimeError
        'End If

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        '納車処理前チェック
        Dim checkBeforeDeliveryResult As Integer = Me.CheckBeforeDelivery(inRONum)

        If checkBeforeDeliveryResult <> 0 Then
            '納車処理前チェックエラー
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} BEFORE DELIVERY CHECK FAILURE " _
                                    , Me.GetType.ToString _
                                    , MethodBase.GetCurrentMethod.Name))
            Me.Rollback = True
            Return checkBeforeDeliveryResult
        End If
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Dim returnCode As Long = 0

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Dim updateCount As Long = 0
        Dim userInfo As StaffContext = StaffContext.Current
        Dim inAccount As String = userInfo.Account
        Dim inDlrCode As String = userInfo.DlrCD
        Dim inNowDate As Date = DateTimeFunc.Now(inDlrCode)
        Dim inDropNowDate As Date = GetDateTimeFloorSecond(inNowDate)

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim operationFlg As Boolean = CheckAddWorkStatus(inRONum)
        'If Not operationFlg Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} ADDWORKSTATUS CHECK FAILURE " _
        '                    , Me.GetType.ToString _
        '                    , MethodBase.GetCurrentMethod.Name))
        '    Me.Rollback = True
        '    Return ActionResult.CheckAddWorkError
        'End If
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        ''サービス入庫テーブルロック処理
        'returnCode = LockServiceInTable(inServiceInId, inRowLockVersion, inAccount, inNowDate, inSystem)
        'If returnCode <> 0 Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} TABLELOCK FAILURE " _
        '                , Me.GetType.ToString _
        '                , MethodBase.GetCurrentMethod.Name))
        '    Me.Rollback = True
        '    Return returnCode
        'End If

        'サービス入庫テーブルロック処理
        Dim returnCodeServiceinLock As Long = LockServiceInTable(inServiceInId, _
                                                                 inRowLockVersion, _
                                                                 inAccount, _
                                                                 inNowDate, _
                                                                 inSystem)

        '処理結果チェック
        If returnCodeServiceinLock <> 0 Then
            'ロックに失敗した場合
            'ロールバックフラグを「True」にしてロックのエラーコードを返却
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[TABLELOCK FAILURE]" _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name _
                        , returnCodeServiceinLock))

            Me.Rollback = True

            Return returnCodeServiceinLock

        End If

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        '更新前のステータス取得
        Dim prevStatus As String = Me.JudgeChipStatus(inStallUseId)
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        Using da As New TabletSMBCommonClassDataAdapter
            updateCount = da.UpdateServiceinDelivery(inServiceInId, _
                                                     inAccount, _
                                                     inNowDate, _
                                                     inDropNowDate)
            If updateCount = 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} UPDATE FAILURE " _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return ActionResult.ExceptionError
            End If

            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
            ''ROステータスを「納車済み：90」に更新
            'updateCount = da.UpdateROStatusByRONum(RostatusDelivered, _
            '                                       inRONum, _
            '                                       inNowDate, _
            '                                       inAccount, _
            '                                       inSystem)
            '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
            '指定サービス入庫IDのROステータスを「納車済み：90」に更新
            'updateCount = da.UpdateROStatusBySvcinId(RostatusDelivered, _
            '                             inServiceInId, _
            '                             inNowDate, _
            '                             inAccount, _
            '                             inSystem)
            updateCount = da.UpdateROStatusBySvcinId(RostatusDelivered, _
                                                     inServiceInId, _
                                                     inNowDate, _
                                                     inAccount, _
                                                     inSystem, _
                                                     inRONum)
            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END
            '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

            If updateCount = 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} RO_STATUS UPDATE FAILURE " _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return ActionResult.ExceptionError
            End If
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        End Using

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        '更新後のステータス取得
        Dim crntStatus As String = Me.JudgeChipStatus(inStallUseId)
        'ステータス送信
        Using ic3802601blc As New IC3802601BusinessLogic
            Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(inServiceInId, _
                                                                    inJobDtlId, _
                                                                    inStallUseId, _
                                                                    prevStatus, _
                                                                    crntStatus, _
                                                                    0)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If dmsSendResult <> 0 Then
            '    Me.Rollback = True
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '処理結果チェック
            If dmsSendResult = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Me.Rollback = True
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE:{2} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , returnCode))
        Return returnCode
    End Function

#End Region
    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START

#Region "洗車へ移動処理"

    ''' <summary>
    ''' 洗車へ移動処理
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function ChipMoveToWash(ByVal inServiceInId As Decimal, _
                                   ByVal inJobDtlId As Decimal, _
                                   ByVal inStallUseId As Decimal, _
                                   ByVal inRowLockVersion As Long, _
                                   ByVal inSystem As String) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Dim returnCode As Long = 0

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Dim updateCount As Long = 0
        Dim userInfo As StaffContext = StaffContext.Current
        Dim inAccount As String = userInfo.Account
        Dim inDlrCode As String = userInfo.DlrCD
        Dim inNowDate As Date = DateTimeFunc.Now(inDlrCode)


        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'サービス入庫テーブルロック処理
        'returnCode = LockServiceInTable(inServiceInId, inRowLockVersion, inAccount, inNowDate, inSystem)
        'If returnCode <> 0 Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} TABLELOCK FAILURE " _
        '                , Me.GetType.ToString _
        '                , MethodBase.GetCurrentMethod.Name))
        '    Me.Rollback = True
        '    Return returnCode
        'End If

        'サービス入庫テーブルロック処理
        Dim returnCodeServiceinLock As Long = LockServiceInTable(inServiceInId, _
                                                                 inRowLockVersion, _
                                                                 inAccount, _
                                                                 inNowDate, _
                                                                 inSystem)

        '処理結果チェック
        If returnCodeServiceinLock <> 0 Then
            'テーブルロックに失敗した場合
            'ロック時のエラーを返却
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} TABLELOCK FAILURE " _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))
            Me.Rollback = True
            Return returnCodeServiceinLock

        End If

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        '更新前のステータス取得
        Dim prevStatus As String = Me.JudgeChipStatus(inStallUseId)
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        Using da As New TabletSMBCommonClassDataAdapter
            'サービスステータスを「洗車待ち」に変更する
            updateCount = da.UpdateServiceinMoveToWash(inServiceInId)
            If updateCount = 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} UPDATE FAILURE " _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return ActionResult.ExceptionError
            End If

            '洗車実績削除する前に削除テーブルに登録する
            da.InsertCarWashResultDel(inServiceInId, inNowDate, inAccount, inSystem)

            '洗車実績削除する
            da.DeleteCarWashResult(inServiceInId)

        End Using

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        '更新後のステータス取得
        Dim crntStatus As String = Me.JudgeChipStatus(inStallUseId)
        'ステータス送信
        Using ic3802601blc As New IC3802601BusinessLogic
            'ステータス連携実施
            Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(inServiceInId, _
                                                                    inJobDtlId, _
                                                                    inStallUseId, _
                                                                    prevStatus, _
                                                                    crntStatus, _
                                                                    0)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If dmsSendResult <> 0 Then
            '    Me.Rollback = True
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '処理結果チェック
            If dmsSendResult = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Me.Rollback = True
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE:{2} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , returnCode))
        Return returnCode
    End Function

#End Region

#Region "納車待ちへ移動処理"

    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
    ' ''' <summary>
    ' ''' 納車待ちへ移動処理
    ' ''' </summary>
    ' ''' <param name="inServiceInId">サービス入庫ID</param>
    ' ''' <param name="inJobDtlId">作業内容ID</param>
    ' ''' <param name="inStallUseId">ストール利用ID</param>
    ' ''' <param name="inPickDeliType">引取納車区分</param>
    ' ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ' ''' <param name="inSystem">プログラムID</param>
    ' ''' <returns>戻り値「0：正常終了、0以外：エラー」</returns>
    ' ''' <remarks></remarks>
    '<EnableCommit()>
    'Public Function ChipMoveToDeliWait(ByVal inServiceInId As Decimal, _
    '                                   ByVal inJobDtlId As Decimal, _
    '                                   ByVal inStallUseId As Decimal, _
    '                                   ByVal inPickDeliType As String, _
    '                                   ByVal inRowLockVersion As Long, _
    '                                   ByVal inSystem As String) As Long
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
    '                , "{0}.{1} START " _
    '                , Me.GetType.ToString _
    '                , MethodBase.GetCurrentMethod.Name))

    ''' <summary>
    ''' 納車待ちへ移動処理
    ''' </summary>
    ''' <param name="inServiceInId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inPickDeliType">引取納車区分</param>
    ''' <param name="inRowLockVersion">行ロックバージョン</param>
    ''' <param name="inSystem">プログラムID</param>
    ''' <param name="inRoNum">RO番号</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function ChipMoveToDeliWait(ByVal inServiceInId As Decimal, _
                                       ByVal inJobDtlId As Decimal, _
                                       ByVal inStallUseId As Decimal, _
                                       ByVal inPickDeliType As String, _
                                       ByVal inRowLockVersion As Long, _
                                       ByVal inSystem As String, _
                                       ByVal inRONum As String) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START inServiceInId={2}, inJobDtlId={3}, inStallUseId={4}, inPickDeliType={5}, inRowLockVersion={6}, inRoNum={7}" _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , inServiceInId _
                    , inJobDtlId _
                    , inStallUseId _
                    , inPickDeliType _
                    , inRowLockVersion _
                    , inRONum))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

        'Dim returnCode As Long = 0
        'Dim updateCount As Long = 0
        'Dim userInfo As StaffContext = StaffContext.Current
        'Dim inAccount As String = userInfo.Account
        'Dim inDlrCode As String = userInfo.DlrCD
        'Dim inNowDate As Date = DateTimeFunc.Now(inDlrCode)

        ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
        ''納車実績日時を取得
        'Dim rsltDeliDateTime As Date = GetRsltDeliDate(inServiceInId)

        'If rsltDeliDateTime = Nothing Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} UPDATE FAILURE " _
        '    , Me.GetType.ToString _
        '    , MethodBase.GetCurrentMethod.Name))
        '    Me.Rollback = True
        '    Return ActionResult.ExceptionError
        'End If

        ''納車実績日時がデフォルトでなければ、既に納車済みと判断する
        'Dim deliveredFlg As Boolean = Not IsDefaultValue(rsltDeliDateTime)
        ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

        ''引取納車区分より更新するステータスを判別する
        'Dim inSvcStatus As String = SvcStatusDropOffCustomer

        ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
        ''If inPickDeliType = DeliTypeWaiting Then
        ''    inSvcStatus = SvcStatusWaitingCustomer
        ''ElseIf inPickDeliType = DeliTypeDropOff Then
        ''    inSvcStatus = SvcStatusDropOffCustomer
        ''End If
        'If deliveredFlg Then
        '    inSvcStatus = SvcStatusDelivery
        'Else
        '    If inPickDeliType = DeliTypeWaiting Then
        '        inSvcStatus = SvcStatusWaitingCustomer
        '    ElseIf inPickDeliType = DeliTypeDropOff Then
        '        inSvcStatus = SvcStatusDropOffCustomer
        '    End If
        'End If
        ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

        ''サービス入庫テーブルロック処理
        'returnCode = LockServiceInTable(inServiceInId, inRowLockVersion, inAccount, inNowDate, inSystem)
        'If returnCode <> 0 Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} TABLELOCK FAILURE " _
        '                , Me.GetType.ToString _
        '                , MethodBase.GetCurrentMethod.Name))
        '    Me.Rollback = True
        '    Return returnCode
        'End If

        ''2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ''更新前のステータス取得
        'Dim prevStatus As String = Me.JudgeChipStatus(inStallUseId)
        ''2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        'Using da As New TabletSMBCommonClassDataAdapter
        '    'サービスステータスをUPDATEする
        '    updateCount = da.UpdateServiceWorkWashEnd(inServiceInId, inSvcStatus)
        '    If updateCount = 0 Then
        '        Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} UPDATE FAILURE " _
        '                    , Me.GetType.ToString _
        '                    , MethodBase.GetCurrentMethod.Name))
        '        Me.Rollback = True
        '        Return ActionResult.ExceptionError
        '    End If

        '    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
        '    If deliveredFlg Then
        '        '指定サービス入庫IDのROステータスを「納車済み：90」に更新
        '        updateCount = da.UpdateROStatusBySvcinId(RostatusDelivered, _
        '                                                 inServiceInId, _
        '                                                 inNowDate, _
        '                                                 inAccount, _
        '                                                 inSystem,
        '                                                 inRoNum)


        '        If updateCount = 0 Then
        '            Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                        , "{0}.{1} RO_STATUS UPDATE FAILURE " _
        '                        , Me.GetType.ToString _
        '                        , MethodBase.GetCurrentMethod.Name))
        '            Me.Rollback = True
        '            Return ActionResult.ExceptionError
        '        End If
        '    End If
        '    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

        'End Using

        ''2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        ''更新後のステータス取得
        'Dim crntStatus As String = Me.JudgeChipStatus(inStallUseId)
        ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
        'If Not deliveredFlg Then
        '    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END
        '    'ステータス送信
        '    Using ic3802601blc As New IC3802601BusinessLogic
        '        Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(inServiceInId, _
        '                                                                inJobDtlId, _
        '                                                                inStallUseId, _
        '                                                                prevStatus, _
        '                                                                crntStatus, _
        '                                                                0)
        '        If dmsSendResult <> 0 Then
        '            Me.Rollback = True
        '            Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
        '                        , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
        '            Return ActionResult.DmsLinkageError
        '        End If
        '    End Using
        '    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        '    '2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） START
        'End If
        ''2014/12/04 TMEJ 丁 新プラットフォーム版i-CROP仕様変更対応（サービス入庫実績更新処理変更） END

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        'ログイン情報の取得
        Dim userInfo As StaffContext = StaffContext.Current

        '現在日時の取得
        Dim inNowDate As Date = DateTimeFunc.Now(userInfo.DlrCD)

        '更新後サービス入庫ステータスを設定
        Dim inSvcStatus As String = SvcStatusDropOffCustomer

        '引き取り納車区分のチェック
        If inPickDeliType = DeliTypeWaiting Then
            '「0：Waiting」の場合
            '「12：納車待ち」を設定
            inSvcStatus = SvcStatusWaitingCustomer

        ElseIf inPickDeliType = DeliTypeDropOff Then
            '「4：DropOff」の場合
            '「11：預かり中」を設定
            inSvcStatus = SvcStatusDropOffCustomer

        End If

        'サービス入庫テーブルロック処理
        Dim returnCodeLockServiceInTable As Long = LockServiceInTable(inServiceInId, _
                                                                      inRowLockVersion, _
                                                                      userInfo.Account, _
                                                                      inNowDate, _
                                                                      inSystem)

        '処理結果チェック
        If returnCodeLockServiceInTable <> 0 Then
            '失敗した場合
            'ロック時の値を返却
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END RETURNCODE={2}[LockServiceInTable FAILURE]" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , returnCodeLockServiceInTable))
            Me.Rollback = True
            Return returnCodeLockServiceInTable

        End If

        '更新前のステータス取得
        Dim prevStatus As String = Me.JudgeChipStatus(inStallUseId)

        Using bizServiceCommonClass As New ServiceCommonClassBusinessLogic
            'サービスDMS納車実績ワークを取得
            Dim dtWorkServiceDmsResultDelivery As ServiceCommonClassDataSet.WorkServiceDmsResultDeliveryDataTable = _
                bizServiceCommonClass.GetWorkServiceDmsResultDelivery(inServiceInId)

            '取得結果のチェック
            If Not (IsNothing(dtWorkServiceDmsResultDelivery)) AndAlso 0 < dtWorkServiceDmsResultDelivery.Count Then
                '取得できた場合
                '強制納車処理を実施
                Dim returnCodeForceDeliverd As Integer = Me.ForceDeliverd(userInfo.DlrCD,
                                                                          userInfo.BrnCD, _
                                                                          inServiceInId, _
                                                                          inRONum, _
                                                                          dtWorkServiceDmsResultDelivery(0).DMS_RSLT_DELI_DATETIME, _
                                                                          userInfo.Account, _
                                                                          inNowDate, _
                                                                          inSystem)

                '処理結果チェック
                If returnCodeForceDeliverd <> ActionResult.Success Then
                    '「0：成功」以外の場合
                    '「22：予期せぬエラー」を返却
                    Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[ForceDeliverd FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.ExceptionError))
                    Me.Rollback = True
                    Return ActionResult.ExceptionError

                End If

            Else
                '上記以外の場合
                '納車待ちへ移動処理を実施
                Using da As New TabletSMBCommonClassDataAdapter
                    'サービス入庫テーブルを更新する
                    Dim updateCountUpdateServiceWorkWashEnd As Long = _
                        da.UpdateServiceWorkWashEnd(inServiceInId, _
                                                    inSvcStatus)

                    '処理結果チェック
                    If updateCountUpdateServiceWorkWashEnd = 0 Then
                        '更新できなかった場合
                        '「22:予期せぬエラー」を返却
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[UpdateServiceWorkWashEnd FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ActionResult.ExceptionError))
                        Me.Rollback = True
                        Return ActionResult.ExceptionError

                    End If

                End Using

                '更新後のステータス取得
                Dim crntStatus As String = Me.JudgeChipStatus(inStallUseId)

                'ステータス送信
                Using ic3802601blc As New IC3802601BusinessLogic
                    'ステータス連携実施
                    Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(inServiceInId, _
                                                                            inJobDtlId, _
                                                                            inStallUseId, _
                                                                            prevStatus, _
                                                                            crntStatus, _
                                                                            0)

                    '処理結果チェック
                    If dmsSendResult = ActionResult.Success Then
                        '「0：成功」の場合
                        '処理なし

                    ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                        '「-9000：DMS除外エラーの警告」の場合
                        '戻り値に「-9000：DMS除外エラーの警告」を設定
                        returnCode = ActionResult.WarningOmitDmsError

                    Else
                        '上記以外の場合
                        '「15：他システムとの連携エラー」を返却
                        Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ActionResult.DmsLinkageError))
                        Me.Rollback = True
                        Return ActionResult.DmsLinkageError

                    End If

                End Using

            End If

        End Using

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE:{2} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , returnCode))
        Return returnCode
    End Function


    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 START

    ''' <summary>
    ''' 納車待ちへ移動のPush処理
    ''' </summary>
    ''' <remarks></remarks>
    ''' <history>
    ''' </history>
    Public Sub ToDeliWaitNoticePush()
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

        'ログイン情報取得
        Dim staffInfo As StaffContext = StaffContext.Current

        'CT権限にPUSHする
        Me.SendAllCTPush(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account)

        'ChT権限にPUSHする
        Me.SendAllChtPush(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account)

        'CW権限にPUSHする
        Me.SendAllCWPush(staffInfo.DlrCD, staffInfo.BrnCD, staffInfo.Account)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2014/12/17 TMEJ 小澤 IT9824_NextSTEPサービス 洗車用端末開発向けた評価用アプリ作成 END

#End Region

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

#Region "キャンセル処理"
#Region "普通予約情報キャンセル"
    ''' <summary>
    ''' チップキャンセル処理
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseStatus">ストール利用ステータス</param>
    ''' <param name="inTempFlg">仮置きフラグ</param>
    ''' <param name="inStallId">ストールID</param>
    ''' <param name="scheDeliDate">納車予定時刻</param>
    ''' <returns>戻り値「0：正常終了、0以外：エラー」</returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Private Function UpdateChipCancel(ByVal inJobDtlId As Decimal, _
                                     ByVal inStallUseStatus As String, _
                                     ByVal inTempFlg As String, _
                                     ByVal inStallId As Decimal, _
                                     ByVal pickDeliType As String, _
                                     ByVal scheSvcinDateTime As Date, _
                                     ByVal scheDeliDate As Date, _
                                     ByVal rowLockVersion As Long) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        Dim userInfo As StaffContext = StaffContext.Current
        Dim inDlrCode As String = userInfo.DlrCD
        Dim inNowDate As Date = DateTimeFunc.Now(inDlrCode)

        'キャンセルできるチェック
        If Not CanCancel(inStallUseStatus, inTempFlg, inStallId) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} CHIPSTATUSCHECK FAILURE " _
                            , Me.GetType.ToString _
                            , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.CheckError
        End If

        'キャンセルDB処理
        Dim xmlclass As New SMBCommonClassBusinessLogic.XmlDocumentClass
        xmlclass = StructWebServiceXml(inJobDtlId.ToString(CultureInfo.InvariantCulture), _
                                        "", _
                                        "", _
                                        "", _
                                        "", _
                                        "", _
                                        userInfo, _
                                        inNowDate, _
                                        "", _
                                        String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", scheDeliDate), _
                                        "", _
                                        "", _
                                        "", _
                                        "", _
                                        CancelFlgCancel, _
                                        pickDeliType, _
                                        String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", scheSvcinDateTime), _
                                        CType(rowLockVersion, String))

        'WebServiceを呼ぶ
        Using commbiz As New SMBCommonClassBusinessLogic
            Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow = commbiz.CallReserveWebService(xmlclass)
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE:{2} " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name _
                    , drWebServiceResult.RESULTCODE))
            If drWebServiceResult.RESULTCODE <> ActionResult.Success Then
                'RowLockVersionError(最新のデータではない)の場合、ActionResult.RowLockVersionErrorを戻す
                If drWebServiceResult.RESULTCODE = WebServiceRowLockVersionError Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E RowLockVersionError. " _
                    , MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.RowLockVersionError
                Else
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E ExceptionError. " _
                                    , MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.ExceptionError
                End If
            End If
        End Using

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return ActionResult.Success

    End Function

    ''' <summary>
    ''' ストールチップ削除
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="systemId">更新クラス</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function DeleteStallChip(ByVal stallUseId As Decimal, _
                                    ByVal systemId As String, _
                                    ByVal rowLockVersion As Long) As Long
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        'Public Function DeleteStallChip(ByVal stallUseId As Decimal, _
        '                                 ByVal objStaffContext As StaffContext, _
        '                                 ByVal systemId As String, _
        '                                 ByVal rowLockVersion As Long) As Long
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} START " _
                    , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))

        '対象の情報を取得する
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId, 1)
        If dtChipEntity.Count <> 1 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                            , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.GetChipEntityError
        End If
        Dim roNum As String = dtChipEntity(0).RO_NUM
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        'Dim roJobSeq As Long = dtChipEntity(0).RO_JOB_SEQ
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Dim returnCode As Long = 0

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '予約送信ため、変更前のチップステータス、予約ステータスを取得する
        Dim preChipStatus As String = Me.JudgeChipStatus(stallUseId)
        Dim preResvStatus As String = dtChipEntity(0).RESV_STATUS

        Dim preJobDtlId As TabletSmbCommonClassCanceledJobInfoDataTable
        Dim afterJobDtlId As TabletSmbCommonClassCanceledJobInfoDataTable
        Dim prevJobDtlIdTable As TabletSmbCommonClassNumberValueDataTable

        Using ta As New TabletSMBCommonClassDataAdapter

            '削除前に、現在の時点でキャンセル済みの作業内容IDを取得する
            prevJobDtlIdTable = ta.GetCanceledJobDtlIdList(dtChipEntity(0).SVCIN_ID)

            '削除する前、RO作業連番があり、かつ作業開始待ちの状態で表示されてるチップの作業内容IDを取得する
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'preJobDtlId = ta.GetCanceledJobDtlId(dtChipEntity(0).SVCIN_ID)
            preJobDtlId = ta.GetNotCanceledJobDtlId(dtChipEntity(0).SVCIN_ID)
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            ''キャンセル
            'returnCode = Me.UpdateChipCancel(dtChipEntity(0).JOB_DTL_ID, dtChipEntity(0).STALL_USE_STATUS, dtChipEntity(0).TEMP_FLG, dtChipEntity(0).STALL_ID, _
            '                                    dtChipEntity(0).PICK_DELI_TYPE, dtChipEntity(0).SCHE_SVCIN_DATETIME, dtChipEntity(0).SCHE_DELI_DATETIME, rowLockVersion)
            ''削除エラーの場合
            'If returnCode <> ActionResult.Success Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} UpdateChipCancel FAILURE " _
            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return returnCode
            'End If

            'キャンセル
            Dim returnCodeChipCancel As Long = Me.UpdateChipCancel(dtChipEntity(0).JOB_DTL_ID, _
                                                                   dtChipEntity(0).STALL_USE_STATUS, _
                                                                   dtChipEntity(0).TEMP_FLG, _
                                                                   dtChipEntity(0).STALL_ID, _
                                                                   dtChipEntity(0).PICK_DELI_TYPE, _
                                                                   dtChipEntity(0).SCHE_SVCIN_DATETIME, _
                                                                   dtChipEntity(0).SCHE_DELI_DATETIME, _
                                                                   rowLockVersion)

            '処理結果チェック
            If returnCodeChipCancel <> ActionResult.Success Then
                '失敗した場合
                '失敗時のエラーコードを返却
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[UpdateChipCancel FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , returnCodeChipCancel))
                Return returnCodeChipCancel

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            '削除したあとで、もう一回表示されてるチップの作業内容IDを取得する
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            'afterJobDtlId = ta.GetCanceledJobDtlId(dtChipEntity(0).SVCIN_ID)
            afterJobDtlId = ta.GetNotCanceledJobDtlId(dtChipEntity(0).SVCIN_ID)
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
        End Using

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        '' 本予約チップの場合、予約送信する
        'If ResvStatusConfirmed.Equals(dtChipEntity(0).RESV_STATUS) Then
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '削除処理前から削除されていた作業内容IDのリストを作成
        Dim prevCanceledJobDtlIdList As List(Of Decimal) = Nothing

        If 0 < prevJobDtlIdTable.Rows.Count Then
            prevCanceledJobDtlIdList = New List(Of Decimal)

            For Each preJobDtlIdRow In prevJobDtlIdTable
                '作業内容IDをリストに追加する
                prevCanceledJobDtlIdList.Add(preJobDtlIdRow.COL1)
            Next
        End If

        '予約送信
        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
        'returnCode = Me.SendReserveInfo(dtChipEntity(0).SVCIN_ID, _
        '                                dtChipEntity(0).JOB_DTL_ID, _
        '                                stallUseId, _
        '                                preChipStatus, _
        '                                preChipStatus, _
        '                                preResvStatus, _
        '                                systemId, _
        '                                prevCanceledJobDtlIdList)
        Using biz3800903 As New IC3800903BusinessLogic
            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'returnCode = biz3800903.SendReserveInfo(dtChipEntity(0).SVCIN_ID, _
            '                                        dtChipEntity(0).JOB_DTL_ID, _
            '                                        stallUseId, _
            '                                        preChipStatus, _
            '                                        preChipStatus, _
            '                                        preResvStatus, _
            '                                        systemId, _
            '                                        prevCanceledJobDtlIdList)

            '予約連携実施
            Dim returnCodeSendReserve As Integer = biz3800903.SendReserveInfo(dtChipEntity(0).SVCIN_ID, _
                                                                              dtChipEntity(0).JOB_DTL_ID, _
                                                                              stallUseId, _
                                                                              preChipStatus, _
                                                                              preChipStatus, _
                                                                              preResvStatus, _
                                                                              systemId, _
                                                                              prevCanceledJobDtlIdList)

            '処理結果チェック
            If returnCodeSendReserve = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf returnCodeSendReserve = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合

                '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

                ''「15：他システムとの連携エラー」を返却
                'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                '    , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                '    , Me.GetType.ToString _
                '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                '    , ActionResult.DmsLinkageError))
                'Return ActionResult.DmsLinkageError

                '予約連携送信のエラーコードが文言コードの場合、文言コード（エラーコード）を返す。
                '文言コードでない場合、「15：他システムとの連携エラー」を返す。
                Dim returnValue As Integer = CheckReturnCodeSendReserveError(returnCodeSendReserve)

                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , returnValue))

                Return returnValue

                '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using

        '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'If returnCode <> 0 Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendReserveInfo FAILURE " _
        '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
        '    Return ActionResult.DmsLinkageError
        'End If

        ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''ステータス送信
        'For Each preJobDtlIdRow In prevJobDtlIdTable
        '    Using ic3802601blc As New IC3802601BusinessLogic
        '        'ステータス連携実施
        '        Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(dtChipEntity(0).SVCIN_ID, _
        '                                                                preJobDtlIdRow.COL1, _
        '                                                                stallUseId, _
        '                                                                preChipStatus, _
        '                                                                preChipStatus, _
        '                                                                0)

        '        If dmsSendResult <> 0 Then
        '            Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
        '                        , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
        '            Return ActionResult.DmsLinkageError
        '        End If

        '    End Using

        'Next

        ''2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '基幹連携
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        ''ROもう紐付いてる場合、
        'If roJobSeq >= 0 And Not String.IsNullOrEmpty(roNum.Trim()) Then

        '削除したデータ(紐付いた)がある
        If Not String.IsNullOrEmpty(roNum.Trim()) _
            And preJobDtlId.Count <> afterJobDtlId.Count _
            And preJobDtlId.Count > 0 Then

            '今回削除されたチップの作業内容IDを取得する
            '２つテーブルの差分のレコードを取得する
            'Dim diffJobDtl As DataTable = _
            '    CType(afterJobDtlId, DataTable).AsEnumerable().Except(CType(preJobDtlId, DataTable).AsEnumerable(), DataRowComparer.Default).CopyToDataTable()
            Dim diffJobDtl As DataTable = _
                 CType(preJobDtlId, DataTable).AsEnumerable().Except(CType(afterJobDtlId, DataTable).AsEnumerable(), DataRowComparer.Default).CopyToDataTable()
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

            '1行もない場合、戻す
            If diffJobDtl.Rows.Count = 0 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'Return ActionResult.Success

                Return returnCode

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If

            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
            ''削除したチップのjobDtlIdをlistで作成する
            'Dim jobDtlIdList As List(Of String) = New List(Of String)
            'For Each drJobDtlId As DataRow In diffJobDtl.Rows
            '    jobDtlIdList.Add(CType(drJobDtlId("JOB_DTL_ID"), String))
            'Next

            '削除したチップのstallUseIdをlistで作成する
            Dim stallUseIdList As List(Of String) = New List(Of String)
            For Each drDiff As DataRow In diffJobDtl.Rows
                stallUseIdList.Add(CType(drDiff("STALL_USE_ID"), String))
            Next

            '着工指示キャンセルされたチップ情報を取得する
            TabletSmbCommonCancelInstructedChipInfo = Me.GetInstructedChipInfo(stallUseIdList)

            'jobDtlIdList.Clear()
            'For Each drJobDtlId As TabletSmbCommonClassCanceledJobInfoRow In TabletSmbCommonCancelInstructedChipInfo
            '    jobDtlIdList.Add(CType(drJobDtlId.JOB_DTL_ID, String))
            'Next

            '着工指示(紐付け解除)
            'returnCode = Me.CancelInstructJob(roNum, objStaffContext, jobDtlIdList)

            'If returnCode <> ActionResult.Success Then
            '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E CancelInstructJob error. ResultCode={1}" _
            '            , System.Reflection.MethodBase.GetCurrentMethod.Name, returnCode))
            '    Return returnCode
            'End If
            '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        End If

        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        'TCにPUSH送信する(着工指示整備解除)
        'SendPushByCancel(diffJobDtl, objStaffContext)
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return returnCode
    End Function

    ''' <summary>
    ''' 着工指示キャンセルしたチップがあるチップ情報を取得する
    ''' </summary>
    ''' <param name="stallUseIdList">ストール利用IDリスト</param>
    ''' <returns>ストールIDリスト</returns>
    ''' <remarks></remarks>
    Private Function GetInstructedChipInfo(ByVal stallUseIdList As List(Of String)) As TabletSmbCommonClassCanceledJobInfoDataTable
        'RO作業連番リストをstringに変更する
        Dim stallUseIds As String = Me.ConvertStringArrayToString(stallUseIdList)
        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.S. stallUseIdList={2} " _
                    , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name, stallUseIds))

        Dim stallIdList As New List(Of Decimal)
        'ストール利用IDがない場合、戻る
        If stallUseIdList.Count = 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CancelInstructJob:rojobSeqList.Count=0", MethodBase.GetCurrentMethod.Name))
            Return Nothing
        End If

        Dim stallIdTable As TabletSmbCommonClassCanceledJobInfoDataTable = Nothing
        Using ta As New TabletSMBCommonClassDataAdapter
            '指定ストール利用IDのチップ情報を取得する
            stallIdTable = ta.GetInstructedChipInfo(stallUseIds)
        End Using
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. stallId count={1}", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name, _
                                  stallIdList.Count))

        Return stallIdTable
    End Function

    ''' <summary>
    ''' TCにPUSH送信する(着工指示整備解除)
    ''' </summary>
    ''' <param name="diffJobDtl"></param>
    ''' <param name="objStaffContext"></param>
    ''' <remarks></remarks>
    Private Sub SendPushByCancel(ByVal diffJobDtl As DataTable, ByVal objStaffContext As StaffContext)

        Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.Start. " _
                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))

        If IsNothing(diffJobDtl) Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End. diffJobDtl is nothing", _
                          System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return
        End If

        'TCにPUSH送信用
        Dim stallIdList As New List(Of Decimal)
        '絞り込んだデータをループする
        For Each drJobDtlId As DataRow In diffJobDtl.Rows
            'この判断がコメントする原因は以後戻る可能性がある
            'Dim dtStart As Date = CType(drJobDtlId("SCHE_START_DATETIME"), Date)
            'Dim dtEnd As Date = CType(drJobDtlId("SCHE_END_DATETIME"), Date)
            'Dim dtNow As Date = DateTimeFunc.Now(objStaffContext.DlrCD)
            ''開始または終了時間が今日の場合、TCにPUSH送信が必要
            'If dtStart.Date = dtNow.Date Or dtEnd.Date = dtNow.Date Then
            Dim stallId As Decimal = CType(drJobDtlId("STALL_ID"), Decimal)
            '重複のは除く
            If Not stallIdList.Contains(stallId) Then
                stallIdList.Add(stallId)
            End If
            'End If
        Next

        '着工指示キャンセルの時、push送信
        'TCにPUSH送信する(着工指示済)
        If stallIdList.Count > 0 Then
            Dim operationCodeList As New List(Of Decimal)
            operationCodeList.Add(Operation.TEC)
            SendPushGetReady(objStaffContext.DlrCD, objStaffContext.BrnCD, operationCodeList, PUSH_FuntionNM, stallIdList)
        End If

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_End.", _
                                  System.Reflection.MethodBase.GetCurrentMethod.Name))
    End Sub

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
    ' ''' <summary>
    ' ''' 着工指示の紐付くapiを呼ぶ
    ' ''' </summary>
    ' ''' <param name="roNum">RO番号</param>
    ' ''' <param name="roJobSeq">RO作業シーケンス</param>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <param name="jobDtlId">作業内容ID</param>
    ' ''' <returns>結果</returns>
    ' ''' <remarks></remarks>
    'Private Function InstructJob(ByVal roNum As String, _
    '                           ByVal roJobSeq As Long, _
    '                           ByVal objStaffContext As StaffContext, _
    '                           ByVal jobDtlId As Decimal) As Long
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.S. roNum={2}, roJobSeq={3}, jobDtlId={4} " _
    '        , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name, roNum, roJobSeq, jobDtlId))

    '    Using dtFixItem As New IC3800902DataSet.IC3800902ServiceInfoDataTable
    '        Dim iC3801015bl As New IC3801015BusinessLogic
    '        '整備情報をAPIから取得する
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "IC3801015BusinessLogic.GetServiceInfo.S PARAM: dlrcd={0},ronum={1}", objStaffContext.DlrCD, roNum))
    '        Dim dtRoPartsInfo As IC3801015DataSet.IC3801015ServiceInfoDataTable = iC3801015bl.GetServiceInfo(objStaffContext.DlrCD, roNum)
    '        Me.OutPutIFLog(dtRoPartsInfo, "IC3801015BusinessLogic.GetServiceInfo")
    '        '自分の整備情報のみを表示させるため、作業連番で絞り込む、
    '        Dim selectDrList As IC3801015DataSet.IC3801015ServiceInfoRow() = _
    '            CType(dtRoPartsInfo.Select(String.Format(CultureInfo.CurrentCulture, "workSeq = {0} AND customerConfirmFlag = '1'", roJobSeq)), IC3801015DataSet.IC3801015ServiceInfoRow())

    '        If selectDrList.Count > 0 Then
    '            '見つけた場合、着工指示をする(紐付け解除)
    '            For Each drselectInfo As IC3801015DataSet.IC3801015ServiceInfoRow In selectDrList
    '                '紐付けを基幹システム側に反映のために整備情報を作成する
    '                Dim drFixItem As IC3800902DataSet.IC3800902ServiceInfoRow = CType(dtFixItem.NewRow(), IC3800902DataSet.IC3800902ServiceInfoRow)
    '                '着工指示の場合、紐付いてない項目を登録する
    '                If drselectInfo.IsREZIDNull Then
    '                    drFixItem.REZID = CType(jobDtlId, Integer)
    '                    drFixItem.srvCode = drselectInfo.srvCode     '整備コード
    '                    drFixItem.srvSeq = drselectInfo.srvSeq       '整備連番
    '                    dtFixItem.Rows.Add(drFixItem)
    '                End If
    '            Next

    '            '残るが0以上の場合、着工指示をする、0件の場合、1件でもないのソースを流れる
    '            If dtFixItem.Rows.Count > 0 Then
    '                '着工指示apiを呼ぶ
    '                Dim biz As New IC3800902BusinessLogic
    '                Logger.Info(String.Format(CultureInfo.InvariantCulture, "IC3800902BusinessLogic.UpdateInstruct.S PARAM: dlrcd={0},ronum={1},addSeq={2}", objStaffContext.DlrCD, roNum, selectDrList(0).addSeq))
    '                Me.OutPutIFLog(dtFixItem, "IC3800902BusinessLogic.UpdateInstruct PARAM: dtFixItem")
    '                Dim result As Long = biz.UpdateInstruct(objStaffContext.DlrCD, roNum, selectDrList(0).addSeq, dtFixItem)
    '                If result <> ActionResult.Success Then
    '                    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E DmsLinkageError. UpdateInstruct returncode={1}" _
    '                             , System.Reflection.MethodBase.GetCurrentMethod.Name, result))
    '                    Return ActionResult.DmsLinkageError
    '                Else
    '                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E UpdateInstruct returncode=0", MethodBase.GetCurrentMethod.Name))
    '                    Return ActionResult.Success
    '                End If
    '            End If
    '        End If

    '        '1件でもないの場合
    '        '着工指示の場合、0件が間違う、少なくとも1件がある
    '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E NoDataFoundError. " _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return ActionResult.NoDataFound

    '    End Using
    'End Function

    ' ''' <summary>
    ' ''' 着工指示の紐付きの解除 apiを呼ぶ
    ' ''' </summary>
    ' ''' <param name="roNum">RO番号</param>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <param name="jobDtlIdList">作業内容ID</param>
    ' ''' <returns>結果</returns>
    ' ''' <remarks></remarks>
    'Private Function CancelInstructJob(ByVal roNum As String, _
    '                                   ByVal objStaffContext As StaffContext, _
    '                                   ByVal jobDtlIdList As List(Of String)) As Long

    '    'RO作業連番リストをstringに変更する
    '    Dim jobDtlId As String = Me.ConvertStringArrayToString(jobDtlIdList)
    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.S. roNum={2}, jobDtlIdList={3} " _
    '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name, roNum, jobDtlId))
    '    'RO作業連番がない場合、戻る
    '    If jobDtlIdList.Count = 0 Then
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CancelInstructJob:rojobSeqList.Count=0", MethodBase.GetCurrentMethod.Name))
    '        Return ActionResult.Success
    '    End If

    '    Dim iC3801015bl As New IC3801015BusinessLogic
    '    '整備情報をAPIから取得する
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "IC3801015BusinessLogic.GetServiceInfo.S PARAM: dlrcd={0},ronum={1}", objStaffContext.DlrCD, roNum))
    '    Dim dtServiceInfo As IC3801015DataSet.IC3801015ServiceInfoDataTable = iC3801015bl.GetServiceInfo(objStaffContext.DlrCD, roNum)
    '    Me.OutPutIFLog(dtServiceInfo, "IC3801015BusinessLogic.GetServiceInfo") 'ログ

    '    '絞り込む(紐ついたかつ顧客承認かつ作業連番が今回削除したチップの作業連番の場合)
    '    Dim serviceList As IC3801015DataSet.IC3801015ServiceInfoRow() = _
    '        CType(dtServiceInfo.Select(String.Format(CultureInfo.CurrentCulture, _
    '                                                 "customerConfirmFlag = '1' AND REZID IS NOT NULL AND REZID in ({0}) ", _
    '                                                 jobDtlId)),  _
    '                                         IC3801015DataSet.IC3801015ServiceInfoRow())

    '    '整備情報があれば
    '    If serviceList.Count > 0 Then

    '        'サービス連番により、紐付きを解除する
    '        '全て追加サービス連番を取得する
    '        Dim srvAddSeqList As New List(Of Integer)
    '        For Each drselectInfo As IC3801015DataSet.IC3801015ServiceInfoRow In serviceList
    '            If Not srvAddSeqList.Contains(drselectInfo.addSeq) Then
    '                srvAddSeqList.Add(drselectInfo.addSeq)
    '            End If
    '        Next
    '        Dim bErrorFlg As Boolean = False
    '        '追加サービス連番ごとに、紐付きを解除する
    '        For Each srvAddSeq As Integer In srvAddSeqList
    '            If Me.CancelInstructJobBySrvAddSeq(objStaffContext.DlrCD, _
    '                                            roNum, _
    '                                            srvAddSeq, _
    '                                            serviceList) <> ActionResult.Success Then
    '                bErrorFlg = True
    '            End If
    '        Next
    '        'エラーの場合
    '        If bErrorFlg Then
    '            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E DmsLinkageError. CancelInstructJobBySrvAddSeq error" _
    '                        , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '            Return ActionResult.DmsLinkageError
    '        End If
    '    End If

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CancelInstructJob", MethodBase.GetCurrentMethod.Name))
    '    Return ActionResult.Success
    'End Function

    ' ''' <summary>
    ' ''' サービス連番により、紐付きを解除する
    ' ''' </summary>
    ' ''' <param name="dlrCode">販売店コード</param>
    ' ''' <param name="roNum">RO番号</param>
    ' ''' <param name="srvAddSeq">追加サービス連番</param>
    ' ''' <param name="serviceList">整備情報リスト</param>
    ' ''' <returns>結果</returns>
    ' ''' <remarks></remarks>
    'Private Function CancelInstructJobBySrvAddSeq(ByVal dlrCode As String, _
    '                                              ByVal roNum As String, _
    '                                              ByVal srvAddSeq As Integer, _
    '                                              ByVal serviceList As IC3801015DataSet.IC3801015ServiceInfoRow()) As Long

    '    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.S. roNum={2}, srvAddSeq={3} " _
    '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name, roNum, srvAddSeq))

    '    Using dtFixItem As New IC3800902DataSet.IC3800902ServiceInfoDataTable
    '        '見つけた場合、着工指示をする(紐付け解除)
    '        For Each drselectInfo As IC3801015DataSet.IC3801015ServiceInfoRow In serviceList
    '            If srvAddSeq = drselectInfo.addSeq Then
    '                '紐付けを基幹システム側に反映のために整備情報を作成する
    '                Dim drFixItem As IC3800902DataSet.IC3800902ServiceInfoRow = CType(dtFixItem.NewRow(), IC3800902DataSet.IC3800902ServiceInfoRow)
    '                '予約ID(紐付け解除の場合、DBNullを設定)
    '                drFixItem.SetREZIDNull()
    '                drFixItem.srvCode = drselectInfo.srvCode     '整備コード
    '                drFixItem.srvSeq = drselectInfo.srvSeq       '整備連番
    '                dtFixItem.Rows.Add(drFixItem)
    '            End If
    '        Next

    '        '残るが0以上の場合、着工指示をする、0件の場合、1件でもないのソースを流れる
    '        If dtFixItem.Rows.Count > 0 Then
    '            '着工指示apiを呼ぶ
    '            Dim biz As New IC3800902BusinessLogic
    '            'ログ
    '            Logger.Info(String.Format(CultureInfo.InvariantCulture, "IC3800902BusinessLogic.UpdateInstruct.S PARAM: dlrcd={0},ronum={1},addSeq={2}", _
    '                                      dlrCode, roNum, srvAddSeq))
    '            Me.OutPutIFLog(dtFixItem, "IC3800902BusinessLogic.UpdateInstruct PARAM: dtFixItem")
    '            '指定追加サービス連番のサービス解除
    '            Dim result As Long = biz.UpdateInstruct(dlrCode, roNum, srvAddSeq, dtFixItem)
    '            If result <> ActionResult.Success Then
    '                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E DmsLinkageError. UpdateInstruct returncode={1}" _
    '                         , System.Reflection.MethodBase.GetCurrentMethod.Name, result))
    '                Return ActionResult.DmsLinkageError
    '            Else
    '                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CancelInstructJob service count={1}", _
    '                                          MethodBase.GetCurrentMethod.Name, dtFixItem.Rows.Count))
    '                Return ActionResult.Success
    '            End If
    '        End If
    '    End Using

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CancelInstructJob service count=0", _
    '                      MethodBase.GetCurrentMethod.Name))
    '    Return ActionResult.Success
    'End Function
    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END
#End Region

#Region "ストール使用不可削除"
    ''' <summary>
    ''' ストール使用不可を削除します。
    ''' </summary>
    ''' <param name="stallIdleId">ストール非稼働ID</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <returns>戻り値「0：正常終了、0以外：エラー」</returns>
    ''' <remarks></remarks>
    Public Function DeleteStallUnavailable(ByVal stallIdleId As Decimal, ByVal updateDate As Date, ByVal staffCode As String, ByVal rowLockVersion As Long, ByVal systemId As String) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} START " _
                    , Me.GetType.ToString _
                    , MethodBase.GetCurrentMethod.Name))

        Dim returnCode As Long = 0
        Dim updateCount As Long = 0
        Using da As New TabletSMBCommonClassDataAdapter
            'todo 最新のrowlockversionを取得
            '作業内容をキャンセルにする
            updateCount = da.UpdateDeleteStallUnavailable(stallIdleId, staffCode, updateDate, rowLockVersion, systemId)
            If updateCount <> 1 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} UpdateDeleteStallUnavailable FAILURE " _
                            , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                Return ActionResult.RowLockVersionError
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", System.Reflection.MethodBase.GetCurrentMethod.Name))
        Return returnCode
    End Function
#End Region

#End Region

#Region "非稼働ストール更新処理"
    ''' <summary>
    ''' 非稼働ストール更新処理
    ''' </summary>
    ''' <param name="stallIdleId">非稼働ストールID</param>
    ''' <param name="stallId">変更後のストールのSTALLID</param>
    ''' <param name="idleStartDateTime">変更後の表示開始日時</param>
    ''' <param name="idleEndDateTime">変更後の表示開始日時</param>
    ''' <param name="idleMemo">仕事時間</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="dtNow">現在日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="systemId">プログラムID</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns> </returns>
    ''' <history>
    ''' 2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加
    ''' </history>
    Public Function UpdateStallUnavailable(ByVal stallIdleId As Decimal, _
                                   ByVal stallId As Decimal, _
                                   ByVal idleStartDateTime As Date, _
                                   ByVal idleEndDateTime As Date, _
                                   ByVal idleMemo As String, _
                                   ByVal updateDate As Date, _
                                   ByVal dtNow As Date, _
                                   ByVal objStaffContext As StaffContext, _
                                   ByVal systemId As String, _
                                   ByVal rowLockVersion As Long) As Long
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallIdleId={1}, stallId={2}, idleStartDateTime={3}, idleEndDateTime={4}, idleMemo={5}, rowLockVersion={6}" _
                                , MethodBase.GetCurrentMethod.Name, stallIdleId, stallId, idleStartDateTime, idleEndDateTime, idleMemo, rowLockVersion))


        'ストール利用チップとの重複配置チェック
        If CheckChipOverlapPosition(objStaffContext.DlrCD, objStaffContext.BrnCD, Nothing, stallId, _
                                    idleStartDateTime, idleEndDateTime, dtNow) Then
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E CheckChipOverlapPosition error. ", MethodBase.GetCurrentMethod.Name))
            Return ActionResult.OverlapUnavailableError
        End If

        '2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 START
        ''他の休憩時間と重複配置チェック
        'If CheckStallIdleOverlapPosition(stallIdleId, stallId, idleStartDateTime, idleEndDateTime) Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.E CheckStallIdleOverlapPosition error. ", MethodBase.GetCurrentMethod.Name))
        '    Return ActionResult.OverlapUnavailableError
        'End If
        '2017/08/30 NSK 竹中(悠) REQ-SVT-TMT-20161109-001 SMB iPadにストールクローズ機能の追加 END

        '更新処理
        Using targetDtStallIdle As New TabletSmbCommonClassStallIdleInfoDataTable

            Dim targetDrStallIdle As TabletSmbCommonClassStallIdleInfoRow = CType(targetDtStallIdle.NewRow, TabletSmbCommonClassStallIdleInfoRow)

            targetDrStallIdle.STALL_IDLE_ID = stallIdleId
            targetDrStallIdle.STALL_ID = stallId
            targetDrStallIdle.IDLE_START_DATETIME = idleStartDateTime
            targetDrStallIdle.IDLE_END_DATETIME = idleEndDateTime

            If Not IsNothing(idleMemo) Then
                targetDrStallIdle.IDLE_MEMO = idleMemo
            Else
                targetDrStallIdle.IDLE_MEMO = ""
            End If

            targetDrStallIdle.UPDATE_DATETIME = updateDate
            targetDrStallIdle.UPDATE_STF_CD = objStaffContext.Account
            targetDrStallIdle.ROW_LOCK_VERSION = rowLockVersion

            Dim result As Long = 0
            Using ta As New TabletSMBCommonClassDataAdapter
                result = ta.UpdateStallUnavailable(targetDrStallIdle, systemId)
            End Using

            '更新成功してない
            If result <> 1 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E result={1} ", MethodBase.GetCurrentMethod.Name, result))
                Return ActionResult.RowLockVersionError
            End If
        End Using

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return ActionResult.Success
    End Function
#End Region

#Region "リレーションコピー"

    ''' <summary>
    ''' リレーションコピー
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="stallId">変更後のストールのSTALLID</param>
    ''' <param name="dispStartDateTime">変更後の表示開始日時</param>
    ''' <param name="scheWorkTime">仕事時間</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="scheDeliDate">納車予定時刻</param>
    ''' <param name="inspectionNeedFlg">検査必要フラグ</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証
    ''' </history>
    Public Function RelationCopy(ByVal stallUseId As Decimal,
                                 ByVal jobDtlId As Decimal, _
                                 ByVal svcinId As Decimal, _
                                 ByVal stallId As Decimal, _
                                 ByVal dispStartDateTime As Date, _
                                 ByVal scheWorkTime As Long, _
                                 ByVal restFlg As String, _
                                 ByVal stallStartTime As Date, _
                                 ByVal stallEndTime As Date, _
                                 ByVal updateDate As Date, _
                                 ByVal objStaffContext As StaffContext, _
                                 ByVal inspectionNeedFlg As String, _
                                 ByVal picekDeliType As String, _
                                 ByVal scheSvcinDateTime As Date, _
                                 ByVal scheDeliDate As Date, _
                                 ByVal rowLockVersion As Long, _
                                 ByVal systemId As String) As Long

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '秒を切り捨てる
        Dim truncSecondDispStartDateTime As Date = Me.GetDateTimeFloorSecond(dispStartDateTime)
        ' 休憩フラグがない場合、1で設定する
        If IsNothing(restFlg) Then
            restFlg = RestTimeGetFlgGetRest
        End If

        'ローカル変数．作業終了日時として処理対象のストール利用．予定終了日時を保持する
        Dim serviceWorkTime As Long = scheWorkTime
        Dim serviceWorkEndDateTime As Date
        '普通の予約チップの場合、予約終了時間が変わる

        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        'Dim scheEndDateTime As Date = GetServiceEndDateTime(stallId, truncSecondDispStartDateTime, _
        '                                            serviceWorkTime, stallStartTime, stallEndTime, restFlg)
        Dim serviceEndDateTimeData As ServiceEndDateTimeData = GetServiceEndDateTime(stallId, truncSecondDispStartDateTime, _
                                                    serviceWorkTime, stallStartTime, stallEndTime, restFlg)
        Dim scheEndDateTime As Date = serviceEndDateTimeData.ServiceEndDateTime
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
        serviceWorkEndDateTime = scheEndDateTime

        '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        restFlg = serviceEndDateTimeData.RestFlg
        '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        'チップ操作制約チェックを行う
        Dim validate As Integer = ValidateMove(stallUseId, objStaffContext, stallId, truncSecondDispStartDateTime, scheWorkTime, serviceWorkEndDateTime, stallStartTime, stallEndTime, updateDate)
        If validate <> ActionResult.Success Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E NotValidateMove" _
                    , MethodBase.GetCurrentMethod.Name))
            Return validate
        End If

        '2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応 START
        'サービス入庫リストを作成
        Dim svcidList As New List(Of Decimal)
        svcidList.Add(svcinId)

        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
        ' サービス分類コード
        Dim svcClassCd As String = Nothing
        ' ローカル変数の洗車必要フラグを'0'（洗車不要）で初期化
        Dim carwashNeedFlg As String = CarWashNeedFlgNeedless
        ' ローカル変数の検査必要フラグを'1'（検査必要）で初期化
        Dim localInspectionNeedFlg As String = InspectionNeedFlgNeed
        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

        Using ta As New TabletSMBCommonClassDataAdapter

            'サービス入庫IDより、チップ情報を取得
            Dim chipTable As TabletSmbCommonClassStallChipInfoDataTable = _
                ta.GetStallChipBySvcinId(objStaffContext.DlrCD, _
                                         objStaffContext.BrnCD, _
                                         svcidList)

            If chipTable.Count > 0 Then
                '0行以上の場合

                '追加作業が画面に置く時、サービスステータスをチェック
                Dim checkResult As Integer = _
                    Me.CheckSvcStatusByPlaningChip(chipTable(0).SVC_STATUS)

                '該当車両が洗車中、検査中、納車済の場合、置けない
                If checkResult <> ActionResult.Success Then

                    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                               "{0}.End ValidateMove failed ", _
                                               MethodBase.GetCurrentMethod.Name))

                    Return checkResult

                End If

                ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
                ' チップ情報の中に洗車必要フラグが'1'（洗車必要）のデータがあった場合、ローカル変数の洗車必要フラグを'1'（洗車必要）にする。
                For i As Integer = 0 To chipTable.Count - 1
                    If CarWashNeedFlgNeed.Equals(chipTable(i).CARWASH_NEED_FLG) Then
                        carwashNeedFlg = CarWashNeedFlgNeed
                        Exit For
            End If
                Next

                ' ストールに紐づくサービス分類情報を取得する。
                Dim rowServiceClass As TabletSmbCommonClassServiceClassRow = GetSvcClassInfo(stallId)

                ' サービス分類情報を取得できた場合
                If (rowServiceClass IsNot Nothing) Then
                    ' サービス分類コード取得
                    svcClassCd = rowServiceClass.SVC_CLASS_CD

                    ' 洗車必要フラグ設定
                    ' ローカル変数の洗車必要フラグが'0'（洗車不要）の場合、サービス分類情報の洗車必要フラグを設定する。
                    If CarWashNeedFlgNeedless.Equals(carwashNeedFlg) Then
                        carwashNeedFlg = rowServiceClass.CARWASH_NEED_FLG
                    End If

                    ' 検査必要フラグ設定
                    If (SvcClassTypeEM.Equals(rowServiceClass.SVC_CLASS_TYPE) _
                        OrElse SvcClassTypeFM.Equals(rowServiceClass.SVC_CLASS_TYPE)) Then
                        ' サービス分類情報．サービス分類区分が「"1"：ＥＭ、"2"：ＰＭ」の場合
                        ' ローカル変数の検査必要フラグを'0'（検査不要）にする。
                        localInspectionNeedFlg = InspectionNeedFlgNeedless

                    Else
                        ' 上記以外の場合
                        ' ローカル変数の検査必要フラグを'1'（検査必要）にする。
                        localInspectionNeedFlg = InspectionNeedFlgNeed
                    End If
                End If
                ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

            End If

        End Using
        '2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応 END

        'WebServiceを呼ぶためXML作成
        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
        'Dim xmlclass As New SMBCommonClassBusinessLogic.XmlDocumentClass
        'xmlclass = StructWebServiceXml("", _
        '       jobDtlId.ToString(CultureInfo.InvariantCulture), _
        '       stallId.ToString(CultureInfo.InvariantCulture), _
        '       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", truncSecondDispStartDateTime), _
        '       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", serviceWorkEndDateTime), _
        '       serviceWorkTime.ToString(CultureInfo.InvariantCulture), _
        '       objStaffContext, _
        '       updateDate, _
        '       GetWebServiceRestFlg(restFlg), _
        '       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", scheDeliDate), _
        '       "", _
        '       inspectionNeedFlg, _
        '       "", _
        '       "", _
        '       "", _
        '       picekDeliType, _
        '       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", scheSvcinDateTime), _
        '       CType(rowLockVersion, String))

        Dim xmlclass As New SMBCommonClassBusinessLogic.XmlDocumentClass
        xmlclass = StructWebServiceXml("", _
               jobDtlId.ToString(CultureInfo.InvariantCulture), _
               stallId.ToString(CultureInfo.InvariantCulture), _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", truncSecondDispStartDateTime), _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", serviceWorkEndDateTime), _
               serviceWorkTime.ToString(CultureInfo.InvariantCulture), _
               objStaffContext, _
               updateDate, _
               GetWebServiceRestFlg(restFlg), _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", scheDeliDate), _
               "", _
               localInspectionNeedFlg, _
               "", _
               "", _
               "", _
               picekDeliType, _
               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", scheSvcinDateTime), _
               CType(rowLockVersion, String), _
               svcClassCd, _
               carwashNeedFlg)
        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

        'WebServiceを呼ぶ
        Using commbiz As New SMBCommonClassBusinessLogic
            Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow = commbiz.CallReserveWebService(xmlclass)
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

            If drWebServiceResult.RESULTCODE <> ActionResult.Success Then
                'RowLockVersionError(最新のデータではない)の場合、ActionResult.RowLockVersionErrorを戻す
                If drWebServiceResult.RESULTCODE = WebServiceRowLockVersionError Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E RowLockVersionError. " _
                        , MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.RowLockVersionError

                Else
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E ExceptionError. " _
                        , MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.ExceptionError

                End If

            Else
                '予約送信ため、変更後のチップステータスを取得する
                Dim crntStatus As String = Me.JudgeChipStatus(drWebServiceResult.STALL_USE_ID)
                Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(drWebServiceResult.STALL_USE_ID)

                If dtChipEntity.Count <> 1 Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                                            , MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.ExceptionError

                End If

                '予約送信
                '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
                'Dim result As Integer = Me.SendReserveInfo(dtChipEntity(0).SVCIN_ID, drWebServiceResult.JOB_DTL_ID, drWebServiceResult.STALL_USE_ID, crntStatus, _
                '                    crntStatus, dtChipEntity(0).RESV_STATUS, systemId)
                Dim result As Integer

                Using biz3800903 As New IC3800903BusinessLogic
                    result = biz3800903.SendReserveInfo(dtChipEntity(0).SVCIN_ID, _
                                                        drWebServiceResult.JOB_DTL_ID, _
                                                        drWebServiceResult.STALL_USE_ID, _
                                                        crntStatus, _
                                                        crntStatus, _
                                                        dtChipEntity(0).RESV_STATUS, _
                                                        systemId)

                    '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                    '処理結果チェック
                    If result = ActionResult.Success Then
                        '「0：成功」の場合
                        '処理なし

                    ElseIf result = ActionResult.WarningOmitDmsError Then
                        '「-9000：DMS除外エラーの警告」の場合
                        '戻り値に「-9000：DMS除外エラーの警告」を設定
                        returnCode = ActionResult.WarningOmitDmsError

                    Else
                        '上記以外の場合

                        '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

                        ''「15：他システムとの連携エラー」を返却
                        'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                        '    , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                        '    , Me.GetType.ToString _
                        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        '    , ActionResult.DmsLinkageError))
                        'Return ActionResult.DmsLinkageError

                        '予約連携送信のエラーコードが文言コードの場合、文言コード（エラーコード）を返す。
                        '文言コードでない場合、「15：他システムとの連携エラー」を返す。
                        Dim returnValue As Integer = CheckReturnCodeSendReserveError(result)

                        Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , returnValue))

                        Return returnValue

                        '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

                    End If

                    '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

                End Using

                '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'If result <> ActionResult.Success Then
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendReserveInfo FAILURE " _
                '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                '    Return ActionResult.DmsLinkageError
                'End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End If
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'Return ActionResult.Success

            Return returnCode

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using
    End Function

#End Region

    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする START
#Region "計画取消処理"

    ''' <summary>
    '''   計画取消
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="staffCode">スタッフコード</param>
    ''' <param name="systemId">呼ぶ画面ID</param>
    ''' <param name="chipInstructFlg">着工指示フラグ</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    Public Function ToReception(ByVal svcinId As Decimal, _
                                            ByVal jobDtlId As Decimal, _
                                            ByVal stallUseId As Decimal, _
                                            ByVal updateDate As Date, _
                                            ByVal staffCode As String, _
                                            ByVal systemId As String,
                                            ByVal rowLockVersion As Long, _
                                            ByRef chipInstructFlg As Boolean) As Long
        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        Using ta As New TabletSMBCommonClassDataAdapter

            'サービス入庫をロックして、チェックする
            Dim returnCodeLockService As Long = LockServiceInTable(svcinId, rowLockVersion, staffCode, updateDate, systemId)
            If returnCodeLockService <> ActionResult.Success Then
                Return returnCodeLockService
            End If

            ' チップエンティティを取得する
            Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId)
            If dtChipEntity.Count <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                                , MethodBase.GetCurrentMethod.Name))
                Return ActionResult.GetChipEntityError
            End If

            '変更前の履歴登録するか否かを判断する情報取得
            Dim dtServiceinBefore As TabletSmbCommonClassServiceinChangeInfoDataTable
            dtServiceinBefore = ta.GetChipChangeInfo(svcinId, dtChipEntity(0).DLR_CD, dtChipEntity(0).BRN_CD)

            '変更前のチップのステータス,予約ステータスの取得
            Dim preChipStatus As String = Me.JudgeChipStatus(stallUseId)
            Dim preResvStatus As String = dtChipEntity(0).RESV_STATUS

            Dim cnt As Long = 0

            'ストール利用を仮置きに更新
            cnt = ta.UpdateStallUseForTemp(stallUseId, systemId, staffCode, updateDate)
            If cnt <> 1 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E Failed to update TB_T_STALL_USE. TB_T_STALL_USE={1},  UPDATE_DATETIME={2}, UPDATE_STF_CD={3} " _
                            , MethodBase.GetCurrentMethod.Name, stallUseId, updateDate, staffCode))
                Return ActionResult.ExceptionError
            End If

            'チップに紐づくjobの着工指示フラグを未指示に更新
            cnt = ta.UpdateJobInstructForNotInstructByJobDtlId(jobDtlId, systemId, staffCode, updateDate)

            '着工指示されたかどうかを保持する
            If cnt >= 1 Then
                chipInstructFlg = True
            End If

            '変更後の履歴登録するか否かを判断する情報情報を取得
            Dim dtServiceinAfter As TabletSmbCommonClassServiceinChangeInfoDataTable
            dtServiceinAfter = ta.GetChipChangeInfo(svcinId, dtChipEntity(0).DLR_CD, dtChipEntity(0).BRN_CD)

            '変更後のチップステータス判定の取得
            Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)

            '履歴登録
            cnt = CreateChipOperationHistory(dtServiceinBefore, dtServiceinAfter, updateDate, staffCode, 0, systemId)
            If cnt <> 0 Then
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} HISINSERT FAILURE " _
                            , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                Return ActionResult.ExceptionError
            End If

            '予約送信
            Using biz3800903 As New IC3800903BusinessLogic

                '予約連携実施
                Dim returnCodeSendReserve As Integer = biz3800903.SendReserveInfo(svcinId, _
                                                                                  dtChipEntity(0).JOB_DTL_ID, _
                                                                                  stallUseId, _
                                                                                  preChipStatus, _
                                                                                  crntStatus, _
                                                                                  preResvStatus, _
                                                                                  systemId)

                '処理結果チェック
                If returnCodeSendReserve = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf returnCodeSendReserve = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合

                    '予約連携送信のエラーコードが文言コードの場合、文言コード（エラーコード）を返す。
                    '文言コードでない場合、「15：他システムとの連携エラー」を返す。
                    Dim returnValue As Integer = CheckReturnCodeSendReserveError(returnCodeSendReserve)

                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , returnValue))

                    Return returnValue

                End If

            End Using

            'ステータス送信
            Using ic3802601blc As New IC3802601BusinessLogic
                Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(svcinId, _
                                                                        dtChipEntity(0).JOB_DTL_ID, _
                                                                        stallUseId, _
                                                                        preChipStatus, _
                                                                        crntStatus, _
                                                                        0)

                '処理結果チェック
                If dmsSendResult = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    returnCode = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Return ActionResult.DmsLinkageError

                End If

            End Using

        End Using

        Return returnCode

    End Function
#End Region


#Region "仮置きチップをストールに配置処理"
    ''' <summary>
    ''' 仮置きチップをストールに配置処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="chipDispStartDate">チップ表示開始日時</param>
    ''' <param name="scheWorkTime">予定作業時間</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="stallStartTime">サービス営業開始時間</param>
    ''' <param name="stallEndTime">サービス営業終了時間</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="systemId">システムID</param>
    ''' <param name="scheDeliDatetime">予定納車日時</param>
    ''' <param name="rowLockVersion">ROWロックバージョン</param>
    ''' <param name="staffcd">スタッフコード</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    <EnableCommit()>
    Public Function ToReceptionChipMove(ByVal stallUseId As Decimal, _
                                            ByVal stallId As Decimal, _
                                            ByVal jobDtlId As Decimal, _
                                            ByVal chipDispStartDate As Date, _
                                            ByVal scheWorkTime As Long, _
                                            ByVal restFlg As String, _
                                            ByVal stallStartTime As Date, _
                                            ByVal stallEndTime As Date, _
                                            ByVal updateDate As Date, _
                                            ByVal objStaffContext As StaffContext, _
                                            ByVal systemId As String, _
                                            ByVal scheDeliDatetime As Date, _
                                            ByVal rowLockVersion As Long, _
                                            ByVal staffcd As String) As SMBCommonClassDataSet.WebServiceResultRow

        Using ta As New TabletSMBCommonClassDataAdapter

            '返却用データ行宣言する
            Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow
            Using dtWebServiceResult As New SMBCommonClassDataSet.WebServiceResultDataTable
                drWebServiceResult = dtWebServiceResult.NewWebServiceResultRow
            End Using

            '秒を切り捨てる
            Dim truncSecondDispStartDateTime As Date = Me.GetDateTimeFloorSecond(chipDispStartDate)
            'チップエンティティの取得'
            Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId)
            If dtChipEntity.Count <> 1 Then
                drWebServiceResult.RESULTCODE = ActionResult.GetChipEntityError
                Me.Rollback = True
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                                        , MethodBase.GetCurrentMethod.Name))
                Return drWebServiceResult
            End If
            'チップ移動できるかチェック
            If Not CanMoveAndResize(dtChipEntity(0).RSLT_END_DATETIME, dtChipEntity(0).STALL_USE_STATUS) Then
                drWebServiceResult.RESULTCODE = ActionResult.CheckError
                Me.Rollback = True
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
                        , MethodBase.GetCurrentMethod.Name))
                Return drWebServiceResult
            End If

            'ローカル変数．作業終了日時として処理対象のストール利用．予定終了日時を保持する
            Dim serviceWorkTime As Long = scheWorkTime
            Dim rsltStartTime As Date = dtChipEntity(0).RSLT_START_DATETIME
            Dim prmsEndDateTime As Date = dtChipEntity(0).PRMS_END_DATETIME
            Dim scheEndDateTime As Date = dtChipEntity(0).SCHE_END_DATETIME

            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            Dim serviceEndDateTimeData As New ServiceEndDateTimeData
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
            Dim serviceWorkEndDateTime As Date
            '普通予約チップの場合
            If IsDefaultValue(rsltStartTime) Then
                '普通のチップの場合、開始時間を計算する(休憩チップと)
                truncSecondDispStartDateTime = Me.GetServiceStartDateTime(stallId, chipDispStartDate, stallStartTime, stallEndTime, restFlg)

                '普通の予約チップの場合、予約終了時間が変わる
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                'scheEndDateTime = GetServiceEndDateTime(stallId, truncSecondDispStartDateTime, _
                '                                        serviceWorkTime, stallStartTime, stallEndTime, restFlg)
                serviceEndDateTimeData = GetServiceEndDateTime(stallId, truncSecondDispStartDateTime, _
                                                        serviceWorkTime, stallStartTime, stallEndTime, restFlg)
                scheEndDateTime = serviceEndDateTimeData.ServiceEndDateTime
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                serviceWorkEndDateTime = scheEndDateTime
            Else
                '実績チップの場合、開始時間が変わらない
                truncSecondDispStartDateTime = chipDispStartDate

                '見込終了日時を取得する
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
                'prmsEndDateTime = GetServiceEndDateTime(stallId, rsltStartTime, _
                '                                        serviceWorkTime, stallStartTime, stallEndTime, restFlg)
                serviceEndDateTimeData = GetServiceEndDateTime(stallId, rsltStartTime, serviceWorkTime, _
                                                                    stallStartTime, stallEndTime, restFlg)
                prmsEndDateTime = serviceEndDateTimeData.ServiceEndDateTime
                '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
                'クライアント側での計算誤差で、五分以内の変更は無効にする
                If DateDiff("n", dtChipEntity(0).PRMS_END_DATETIME, prmsEndDateTime) > -5 AndAlso DateDiff("n", dtChipEntity(0).PRMS_END_DATETIME, prmsEndDateTime) < 5 Then
                    prmsEndDateTime = dtChipEntity(0).PRMS_END_DATETIME
                    serviceWorkTime = dtChipEntity(0).SCHE_WORKTIME
                End If
                serviceWorkEndDateTime = prmsEndDateTime
            End If

            'チップ操作制約チェックを行う
            Dim validate As Integer = ValidateMove(stallUseId, objStaffContext, stallId, truncSecondDispStartDateTime, scheWorkTime, serviceWorkEndDateTime, stallStartTime, stallEndTime, updateDate)
            If validate <> ActionResult.Success Then
                drWebServiceResult.RESULTCODE = validate
                Me.Rollback = True
                Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E ValidateMove failed ", MethodBase.GetCurrentMethod.Name))
                Return drWebServiceResult
            End If
            '予約送信のため、変更前のチップステータス、予約ステータスを取得す
            Dim preChipStatus As String = Me.JudgeChipStatus(stallUseId)
            Dim preResvStatus As String = dtChipEntity(0).RESV_STATUS

            'WebServiceで更新する
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'drWebServiceResult = Me.CallWebServiceUpdate(dtChipEntity, _
            '                                 stallId, _
            '                                 truncSecondDispStartDateTime, _
            '                                 serviceWorkEndDateTime, _
            '                                 serviceWorkTime, _
            '                                 objStaffContext, _
            '                                 updateDate, _
            '                                 restFlg, _
            '                                 scheDeliDatetime, _
            '                                 rowLockVersion)
            drWebServiceResult = Me.CallWebServiceUpdate(dtChipEntity, _
                                             stallId, _
                                             truncSecondDispStartDateTime, _
                                             serviceWorkEndDateTime, _
                                             serviceWorkTime, _
                                             objStaffContext, _
                                             updateDate, _
                                             serviceEndDateTimeData.RestFlg, _
                                             scheDeliDatetime, _
                                             rowLockVersion)
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} ⑨SC3240301_受付エリアからの仮置きチップ配置処理 [WebServiceで更新] END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))

            If drWebServiceResult.RESULTCODE <> ActionResult.Success Then
                Me.Rollback = True
                Return drWebServiceResult
            End If

            'チップに紐づくJobの着工指示フラグを指示済みに更新
            ta.UpdateJobInstructForInstructByJobDtlId(jobDtlId, _
                                    systemId, _
                                    staffcd, _
                                    updateDate)

            '予約送信ため、変更後のチップステータスを取得する
            Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)
            '予約送信
            Using biz3800903 As New IC3800903BusinessLogic
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} ⑩SC3240301_受付エリアからの仮置きチップ配置処理 [予約連携] START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

                '予約連携実施
                Dim returnCodeSendReserve As Integer = biz3800903.SendReserveInfo(dtChipEntity(0).SVCIN_ID, _
                                                                                  dtChipEntity(0).JOB_DTL_ID, _
                                                                                  stallUseId, _
                                                                                  preChipStatus, _
                                                                                  crntStatus, _
                                                                                  preResvStatus, _
                                                                                  systemId, _
                                                                                  Nothing, _
                                                                                  False, _
                                                                                  True)
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} ⑩SC3240301_受付エリアからの仮置きチップ配置処理 [予約連携] END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
                '処理結果チェック
                If returnCodeSendReserve = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf returnCodeSendReserve = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    drWebServiceResult.RESULTCODE = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '予約連携送信のエラーコードが文言コードの場合、文言コード（エラーコード）を返す。
                    '文言コードでない場合、「15：他システムとの連携エラー」を返す。
                    drWebServiceResult.RESULTCODE = CheckReturnCodeSendReserveError(returnCodeSendReserve)

                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , drWebServiceResult.RESULTCODE))

                    Me.Rollback = True
                    Return drWebServiceResult

                End If
            End Using

            'ステータス送信
            Using ic3802601blc As New IC3802601BusinessLogic
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} ⑪SC3240301_受付エリアからの仮置きチップ配置処理 [ステータス連携] START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

                'ステータス連携実施
                Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(dtChipEntity(0).SVCIN_ID, _
                                                                        dtChipEntity(0).JOB_DTL_ID, _
                                                                        stallUseId, _
                                                                        preChipStatus, _
                                                                        crntStatus, _
                                                                        0, _
                                                                        preResvStatus, _
                                                                        preResvStatus)
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} ⑪SC3240301_受付エリアからの仮置きチップ配置処理 [ステータス連携] END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

                '処理結果チェック
                If dmsSendResult = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    drWebServiceResult.RESULTCODE = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    drWebServiceResult.RESULTCODE = ActionResult.DmsLinkageError
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Me.Rollback = True
                    Return drWebServiceResult

                End If

            End Using

            Return drWebServiceResult

        End Using

    End Function

#End Region
    '2017/09/05 NSK 小川 REQ-SVT-TMT-20160906-003 計画済みチップをストールから簡単に外せるようにする END

#Region "サブチップ移動処理"
    ''' <summary>
    ''' 受付エリアのチップストールに配置処理(Update)
    ''' </summary>
    ''' <param name="stallUseId">チップの予約ID</param>
    ''' <param name="stallId">変更後のストールのSTALLID</param>
    ''' <param name="dispStartDateTime">変更後の表示開始日時</param>
    ''' <param name="scheWorkTime">仕事時間</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="systemId">更新クラス</param>
    ''' <param name="scheDeliDatetime">予定開始時間</param>
    ''' <param name="workSeq">作業連番</param>
    ''' <param name="rowLockVersion">ROWロックバージョン</param>
    ''' <param name="staffcd">スタッフコード</param>
    ''' <returns>WebService処理結果</returns>
    ''' <history>
    ''' 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function ReceptionChipMoveUpdate(ByVal stallUseId As Decimal, _
                                            ByVal stallId As Decimal, _
                                            ByVal dispStartDateTime As Date, _
                                            ByVal scheWorkTime As Long, _
                                            ByVal restFlg As String, _
                                            ByVal stallStartTime As Date, _
                                            ByVal stallEndTime As Date, _
                                            ByVal updateDate As Date, _
                                            ByVal objStaffContext As StaffContext, _
                                            ByVal systemId As String, _
                                            ByVal scheDeliDatetime As Date, _
                                            ByVal workSeq As Long, _
                                            ByVal rowLockVersion As Long, _
                                            ByVal staffcd As String) As SMBCommonClassDataSet.WebServiceResultRow

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        'Public Function ReceptionChipMoveUpdate(ByVal svcinId As Long, _
        '                        ByVal stallUseId As Long, _
        '                        ByVal stallId As Long, _
        '                        ByVal dispStartDateTime As Date, _
        '                        ByVal scheWorkTime As Long, _
        '                        ByVal restFlg As String, _
        '                        ByVal stallStartTime As Date, _
        '                        ByVal stallEndTime As Date, _
        '                        ByVal updateDate As Date, _
        '                        ByVal objStaffContext As StaffContext, _
        '                        ByVal systemId As String, _
        '                        ByVal scheDeliDatetime As Date, _
        '                        ByVal partsFlg As String, _
        '                        ByVal workSeq As Long, _
        '                        ByVal rowLockVersion As Long, _
        '                        ByVal staffcd As String, _
        '                        ByVal roNum As String) As Long


        'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S. stallUseId={1}, stallId={2}, ScheStartDateTime={3}, scheWorkTime={4}, restFlg={5}, stallStartTime={6}, stallEndTime={7}" _
        '                               , MethodBase.GetCurrentMethod.Name, stallUseId, stallId, dispStartDateTime, scheWorkTime, restFlg, stallStartTime, stallEndTime))
        Logger.Info(String.Format(CultureInfo.InvariantCulture, _
                          "{0}.S. stallUseId={1}, stallId={2}, dispStartDateTime={3}, scheWorkTime={4}, restFlg={5}, stallStartTime={6}, stallEndTime={7}, updateDate={8},systemId={9},scheDeliDatetime={10},workSeq={11}" _
                        , MethodBase.GetCurrentMethod.Name, _
                        stallUseId, _
                        stallId, _
                        dispStartDateTime, _
                        scheWorkTime, _
                        restFlg, _
                        stallStartTime, _
                        stallEndTime, _
                        updateDate, _
                        systemId, _
                        scheDeliDatetime, _
                        workSeq))
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        Dim result As Long = 0

        '返却用データ行宣言する
        Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow
        Using dtWebServiceResult As New SMBCommonClassDataSet.WebServiceResultDataTable
            drWebServiceResult = dtWebServiceResult.NewWebServiceResultRow
        End Using

        '秒を切り捨てる
        Dim truncSecondDispStartDateTime As Date = Me.GetDateTimeFloorSecond(dispStartDateTime)
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId)
        If dtChipEntity.Count <> 1 Then
            drWebServiceResult.RESULTCODE = ActionResult.GetChipEntityError
            Me.Rollback = True
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                                    , MethodBase.GetCurrentMethod.Name))
            Return drWebServiceResult
        End If

        If Not CanMoveAndResize(dtChipEntity(0).RSLT_END_DATETIME, dtChipEntity(0).STALL_USE_STATUS) Then
            drWebServiceResult.RESULTCODE = ActionResult.CheckError
            Me.Rollback = True
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E CheckError" _
                    , MethodBase.GetCurrentMethod.Name))
            Return drWebServiceResult
        End If

        'ローカル変数．作業終了日時として処理対象のストール利用．予定終了日時を保持する
        Dim serviceWorkTime As Long = scheWorkTime
        Dim rsltStartTime As Date = dtChipEntity(0).RSLT_START_DATETIME
        Dim prmsEndDateTime As Date = dtChipEntity(0).PRMS_END_DATETIME
        Dim scheEndDateTime As Date = dtChipEntity(0).SCHE_END_DATETIME

        Dim serviceWorkEndDateTime As Date
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        '操作制約チェックを行う開始日時
        Dim serviceEndDateTimeData As New ServiceEndDateTimeData
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
        '普通予約チップの場合
        If IsDefaultValue(rsltStartTime) Then
            '普通のチップの場合、開始時間を計算する(休憩チップと)
            truncSecondDispStartDateTime = Me.GetServiceStartDateTime(stallId, dispStartDateTime, stallStartTime, stallEndTime, restFlg)

            '普通の予約チップの場合、予約終了時間が変わる
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'scheEndDateTime = GetServiceEndDateTime(stallId, truncSecondDispStartDateTime, _
            '                                        serviceWorkTime, stallStartTime, stallEndTime, restFlg)
            serviceEndDateTimeData = GetServiceEndDateTime(stallId, truncSecondDispStartDateTime, _
                                                    serviceWorkTime, stallStartTime, stallEndTime, restFlg)
            scheEndDateTime = serviceEndDateTimeData.ServiceEndDateTime
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
            serviceWorkEndDateTime = scheEndDateTime
        Else
            '実績チップの場合、開始時間が変わらない
            truncSecondDispStartDateTime = dispStartDateTime

            '見込終了日時を取得する
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'prmsEndDateTime = GetServiceEndDateTime(stallId, rsltStartTime, _
            '                                        serviceWorkTime, stallStartTime, stallEndTime, restFlg)
            serviceEndDateTimeData = GetServiceEndDateTime(stallId, rsltStartTime, serviceWorkTime, _
                                                                stallStartTime, stallEndTime, restFlg)
            prmsEndDateTime = serviceEndDateTimeData.ServiceEndDateTime
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
            'クライアント側での計算誤差で、五分以内の変更は無効にする
            If DateDiff("n", dtChipEntity(0).PRMS_END_DATETIME, prmsEndDateTime) > -5 AndAlso DateDiff("n", dtChipEntity(0).PRMS_END_DATETIME, prmsEndDateTime) < 5 Then
                prmsEndDateTime = dtChipEntity(0).PRMS_END_DATETIME
                serviceWorkTime = dtChipEntity(0).SCHE_WORKTIME
            End If
            serviceWorkEndDateTime = prmsEndDateTime
        End If

        'チップ操作制約チェックを行う
        Dim validate As Integer = ValidateMove(stallUseId, objStaffContext, stallId, truncSecondDispStartDateTime, scheWorkTime, serviceWorkEndDateTime, stallStartTime, stallEndTime, updateDate)
        If validate <> ActionResult.Success Then
            drWebServiceResult.RESULTCODE = validate
            Me.Rollback = True
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E ValidateMove failed ", MethodBase.GetCurrentMethod.Name))
            Return drWebServiceResult
        End If
        Dim cnt As Long
        '予約送信ため、変更前のチップステータス、予約ステータスを取得する
        Dim preChipStatus As String = Me.JudgeChipStatus(stallUseId)
        Dim preResvStatus As String = dtChipEntity(0).RESV_STATUS
        If IsDefaultValue(dtChipEntity(0).RSLT_START_DATETIME) Then
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START

            '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} ⑨SC3240301_受付エリアからのチップ配置処理 [WebServiceで更新] START" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
            '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

            'WebServiceで更新する
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'drWebServiceResult = Me.CallWebServiceUpdate(dtChipEntity, _
            '                                 stallId, _
            '                                 truncSecondDispStartDateTime, _
            '                                 serviceWorkEndDateTime, _
            '                                 serviceWorkTime, _
            '                                 objStaffContext, _
            '                                 updateDate, _
            '                                 restFlg, _
            '                                 scheDeliDatetime, _
            '                                 rowLockVersion)
            drWebServiceResult = Me.CallWebServiceUpdate(dtChipEntity, _
                                             stallId, _
                                             truncSecondDispStartDateTime, _
                                             serviceWorkEndDateTime, _
                                             serviceWorkTime, _
                                             objStaffContext, _
                                             updateDate, _
                                             serviceEndDateTimeData.RestFlg, _
                                             scheDeliDatetime, _
                                             rowLockVersion)
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

            '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} ⑨SC3240301_受付エリアからのチップ配置処理 [WebServiceで更新] END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
            '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

            If drWebServiceResult.RESULTCODE <> ActionResult.Success Then
                Me.Rollback = True
                Return drWebServiceResult
            End If
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            'WebServiceを呼ぶためXML作成
            'Dim drChipInfo As TabletSmbCommonClassChipEntityRow = dtChipEntity(0)
            'Dim xmlclass As New SMBCommonClassBusinessLogic.XmlDocumentClass
            'xmlclass = StructWebServiceXml(drChipInfo.JOB_DTL_ID.ToString(CultureInfo.InvariantCulture), _
            '                   "", _
            '                   stallId.ToString(CultureInfo.InvariantCulture), _
            '                   String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", truncSecondDispStartDateTime), _
            '                   String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", serviceWorkEndDateTime), _
            '                   serviceWorkTime.ToString(CultureInfo.InvariantCulture), _
            '                   objStaffContext, _
            '                   updateDate, _
            '                   GetWebServiceRestFlg(restFlg), _
            '                   String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", scheDeliDatetime), _
            '                   "", _
            '                   "", _
            '                   WorkOrderFlgOn, _
            '                   "", _
            '                   "", _
            '                   dtChipEntity(0).PICK_DELI_TYPE, _
            '                   String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", dtChipEntity(0).SCHE_SVCIN_DATETIME), _
            '                   CType(rowLockVersion, String))

            ''WebServiceを呼ぶ
            'Using commbiz As New SMBCommonClassBusinessLogic
            '    Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow = commbiz.CallReserveWebService(xmlclass)
            '    If drWebServiceResult.RESULTCODE <> ActionResult.Success Then
            '        'RowLockVersionError(最新のデータではない)の場合、ActionResult.RowLockVersionErrorを戻す
            '        If drWebServiceResult.RESULTCODE = WebServiceRowLockVersionError Then
            '            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E RowLockVersionError. " _
            '                , MethodBase.GetCurrentMethod.Name))
            '            Return ActionResult.RowLockVersionError
            '        Else
            '            Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E StallChipMoveResize failed. RESULTCODE={1}" _
            '                , MethodBase.GetCurrentMethod.Name, drWebServiceResult.RESULTCODE))
            '            Return ActionResult.ExceptionError
            '        End If

            '        'Else
            '        '    If PartsFlgOn.Equals(partsFlg) Then
            '        '        '部品準備完了フラグ更新
            '        '        cnt = ta.UpdatePartsFlg(drWebServiceResult.STALL_USE_ID, objStaffContext.Account, updateDate, systemId)
            '        '        If cnt <> 1 Then
            '        '            Return ActionResult.ExceptionError
            '        '        End If
            '        '    End If

            '    End If
            'End Using
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        Else
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START

            '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} ⑨SC3240301_受付エリアからのチップ配置処理 [ローカルで更新] START" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
            '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

            'ローカルで更新する
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'result = Me.LocalUpdate(dtChipEntity, _
            '                        objStaffContext, _
            '                        stallId, _
            '                        truncSecondDispStartDateTime, _
            '                        scheEndDateTime, _
            '                        serviceWorkTime, _
            '                        rowLockVersion, _
            '                        restFlg, _
            '                        prmsEndDateTime, _
            '                        scheDeliDatetime, _
            '                        staffcd, _
            '                        updateDate, _
            '                        systemId)
            result = Me.LocalUpdate(dtChipEntity, _
                                    objStaffContext, _
                                    stallId, _
                                    truncSecondDispStartDateTime, _
                                    scheEndDateTime, _
                                    serviceWorkTime, _
                                    rowLockVersion, _
                                    serviceEndDateTimeData.RestFlg, _
                                    prmsEndDateTime, _
                                    scheDeliDatetime, _
                                    staffcd, _
                                    updateDate, _
                                    systemId)
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

            '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} ⑨SC3240301_受付エリアからのチップ配置処理 [ローカルで更新] END" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
            '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

            drWebServiceResult.RESULTCODE = result

            If result <> ActionResult.Success Then
                Me.Rollback = True
                Return drWebServiceResult
            End If

            'サービス入庫をロックして、チェックする
            'result = LockServiceInTable(svcinId, rowLockVersion, staffcd, updateDate, systemId)
            'If result <> ActionResult.Success Then
            '    Me.Rollback = True
            '    Return result
            'End If
            ''変更前の情報を取得する
            'Dim dtServiceinBefore As TabletSmbCommonClassServiceinChangeInfoDataTable
            'dtServiceinBefore = ta.GetChipChangeInfo(dtChipEntity(0).SVCIN_ID, objStaffContext.DlrCD, objStaffContext.BrnCD)
            ''StallUseをupdate用データセット作成
            'Using targetDtChipEntity As New TabletSmbCommonClassChipEntityDataTable
            '    Dim targetDrChipEntity As TabletSmbCommonClassChipEntityRow = CType(targetDtChipEntity.NewRow(), TabletSmbCommonClassChipEntityRow)

            '    targetDrChipEntity.STALL_USE_ID = stallUseId
            '    targetDrChipEntity.STALL_ID = stallId
            '    targetDrChipEntity.SCHE_START_DATETIME = truncSecondDispStartDateTime
            '    targetDrChipEntity.SCHE_END_DATETIME = scheEndDateTime
            '    targetDrChipEntity.SCHE_WORKTIME = serviceWorkTime
            '    targetDrChipEntity.REST_FLG = restFlg
            '    targetDrChipEntity.UPDATE_DATETIME = updateDate
            '    targetDrChipEntity.UPDATE_STF_CD = objStaffContext.Account
            '    targetDrChipEntity.PRMS_END_DATETIME = prmsEndDateTime
            '    'targetDrChipEntity.PARTS_FLG = partsFlg

            '    'StallUse更新処理を実行する
            '    cnt = ta.StallChipMoveResize(targetDrChipEntity, systemId)
            '    If cnt <> 1 Then
            '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E ReceptionChipMoveResize failed. cnt={1}" _
            '                                , MethodBase.GetCurrentMethod.Name, cnt))
            '        Return ActionResult.ExceptionError
            '    End If

            '    ''作業内容テーブルを更新（RO紐付く)実行する
            '    'cnt = ta.UpdateJobDtlAttchment(dtChipEntity(0).JOB_DTL_ID, workSeq, updateDate, objStaffContext.Account, systemId)
            '    'If cnt <> 1 Then
            '    '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E ReceptionChipMoveResize failed. cnt={1}" _
            '    '                            , MethodBase.GetCurrentMethod.Name, cnt))
            '    '    Return ActionResult.ExceptionError
            '    'End If

            '    'サービスステータス更新判定
            '    Dim svcStatus As String
            '    If CheckSvcStatusUpdate(dtChipEntity(0).SVC_STATUS) Then
            '        'サービスステータスを"「06:次の作業開始待ち」"更新する
            '        svcStatus = SvcStatusNextStartWait
            '    Else
            '        svcStatus = dtChipEntity(0).SVC_STATUS
            '    End If
            '    'サービス入庫テーブルを更新
            '    cnt = ta.UpdateSvcinUpdateJobDtlAttchment(dtChipEntity(0).SVCIN_ID, scheDeliDatetime, svcStatus, updateDate, objStaffContext.Account)
            '    If cnt <> 1 Then
            '        Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E ReceptionChipMoveResize failed. cnt={1}" _
            '                                , MethodBase.GetCurrentMethod.Name, cnt))
            '        Return ActionResult.ExceptionError
            '    End If
            '    '変更後の情報を取得する
            '    Dim dtServiceinAfter As TabletSmbCommonClassServiceinChangeInfoDataTable
            '    dtServiceinAfter = ta.GetChipChangeInfo(dtChipEntity(0).SVCIN_ID, objStaffContext.DlrCD, objStaffContext.BrnCD)
            '    '履歴登録
            '    cnt = CreateChipOperationHistory(dtServiceinBefore, dtServiceinAfter, updateDate, objStaffContext.Account, 0, systemId)
            '    If cnt <> 0 Then
            '        Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} HISINSERT FAILURE " _
            '                    , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '        Return ActionResult.ExceptionError
            '    End If
            'End Using
            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        End If

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        Using ta As New TabletSMBCommonClassDataAdapter
            '旧作業指示を削除する前保持する
            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
            'Dim dtJobInstruct As TabletSmbCommonClassJobInstructDataTable = ta.GetJobInstruct(workSeq, dtChipEntity(0).RO_NUM)
            Dim dtJobInstruct As TabletSmbCommonClassJobInstructDataTable = _
                ta.GetJobInstruct(workSeq, _
                                  dtChipEntity(0).RO_NUM, _
                                  objStaffContext.DlrCD, _
                                  objStaffContext.BrnCD)
            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

            If Not dtJobInstruct.Count > 0 Then
                drWebServiceResult.RESULTCODE = ActionResult.ExceptionError
                Me.Rollback = True
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetJobInstruct failed. cnt={1}" _
                        , MethodBase.GetCurrentMethod.Name, dtJobInstruct.Count))
                Return drWebServiceResult
            End If

            '旧作業指示を削除する
            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
            'cnt = ta.DeleteJobInstruct(workSeq, dtChipEntity(0).RO_NUM)
            cnt = ta.DeleteJobInstruct(workSeq, _
                                       dtChipEntity(0).RO_NUM, _
                                       objStaffContext.DlrCD, _
                                       objStaffContext.BrnCD)
            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END
            If Not cnt > 0 Then
                drWebServiceResult.RESULTCODE = ActionResult.ExceptionError
                Me.Rollback = True
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E JobInstruct failed. cnt={1}" _
                                        , MethodBase.GetCurrentMethod.Name, cnt))
                Return drWebServiceResult
            End If

            '着工指示待ちのJobをループで着工指示をINSERTする
            For Each drJobInstruct As TabletSmbCommonClassJobInstructRow In dtJobInstruct
                '着工指示
                drJobInstruct.JOB_DTL_ID = dtChipEntity(0).JOB_DTL_ID
                cnt = ta.InsertJobInstructBinding(drJobInstruct, _
                                  updateDate, _
                                  staffcd, _
                                  systemId)
                If cnt <> 1 Then
                    drWebServiceResult.RESULTCODE = ActionResult.ExceptionError
                    Me.Rollback = True
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E JobInstruct failed. cnt={1}" _
                                            , MethodBase.GetCurrentMethod.Name, cnt))
                    Return drWebServiceResult
                End If
            Next

            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
            ''紐付けを基幹システム側に反映連携
            'result = Me.InstructJob(roNum, workSeq, objStaffContext, dtChipEntity(0).JOB_DTL_ID)
            'If result <> ActionResult.Success Then
            '    Return result
            'End If

            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

            '予約送信ため、変更後のチップステータスを取得する
            Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)
            '予約送信
            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
            'cnt = Me.SendReserveInfo(dtChipEntity(0).SVCIN_ID, dtChipEntity(0).JOB_DTL_ID, stallUseId, preChipStatus, _
            '                    crntStatus, preResvStatus, systemId)
            Using biz3800903 As New IC3800903BusinessLogic

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'cnt = biz3800903.SendReserveInfo(dtChipEntity(0).SVCIN_ID, _
                '                                 dtChipEntity(0).JOB_DTL_ID, _
                '                                 stallUseId, _
                '                                 preChipStatus, _
                '                                 crntStatus, _
                '                                 preResvStatus, _
                '                                 systemId)

                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} ⑩SC3240301_受付エリアからのチップ配置処理 [予約連携] START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

                '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 START

                '予約連携実施
                'Dim returnCodeSendReserve As Integer = biz3800903.SendReserveInfo(dtChipEntity(0).SVCIN_ID, _
                '                                                                  dtChipEntity(0).JOB_DTL_ID, _
                '                                                                  stallUseId, _
                '                                                                  preChipStatus, _
                '                                                                  crntStatus, _
                '                                                                  preResvStatus, _
                '                                                                  systemId)

                Dim returnCodeSendReserve As Integer = biz3800903.SendReserveInfo(dtChipEntity(0).SVCIN_ID, _
                                                                                  dtChipEntity(0).JOB_DTL_ID, _
                                                                                  stallUseId, _
                                                                                  preChipStatus, _
                                                                                  crntStatus, _
                                                                                  preResvStatus, _
                                                                                  systemId, _
                                                                                  Nothing, _
                                                           False, _
                                                           True)
                '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 END

                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} ⑩SC3240301_受付エリアからのチップ配置処理 [予約連携] END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

                '処理結果チェック
                If returnCodeSendReserve = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf returnCodeSendReserve = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    drWebServiceResult.RESULTCODE = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合

                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

                    ''「15：他システムとの連携エラー」を返却
                    'drWebServiceResult.RESULTCODE = ActionResult.DmsLinkageError
                    'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                    '    , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                    '    , Me.GetType.ToString _
                    '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    '    , ActionResult.DmsLinkageError))

                    '予約連携送信のエラーコードが文言コードの場合、文言コード（エラーコード）を返す。
                    '文言コードでない場合、「15：他システムとの連携エラー」を返す。
                    drWebServiceResult.RESULTCODE = CheckReturnCodeSendReserveError(returnCodeSendReserve)

                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                           , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                           , Me.GetType.ToString _
                           , System.Reflection.MethodBase.GetCurrentMethod.Name _
                           , drWebServiceResult.RESULTCODE))

                    '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

                    Me.Rollback = True
                    Return drWebServiceResult

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End Using

            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If cnt <> 0 Then
            '    drWebServiceResult.RESULTCODE = ActionResult.DmsLinkageError
            '    Me.Rollback = True
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendReserveInfo FAILURE " _
            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return drWebServiceResult
            'End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START

            'ステータス送信
            Using ic3802601blc As New IC3802601BusinessLogic

                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} ⑪SC3240301_受付エリアからのチップ配置処理 [ステータス連携] START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

                'ステータス連携実施
                Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(dtChipEntity(0).SVCIN_ID, _
                                                                        dtChipEntity(0).JOB_DTL_ID, _
                                                                        stallUseId, _
                                                                        preChipStatus, _
                                                                        crntStatus, _
                                                                        0)

                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} ⑪SC3240301_受付エリアからのチップ配置処理 [ステータス連携] END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

                'If dmsSendResult <> 0 Then
                '    drWebServiceResult.RESULTCODE = ActionResult.DmsLinkageError
                '    Me.Rollback = True
                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
                '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                '    Return drWebServiceResult
                'End If

                '処理結果チェック
                If dmsSendResult = ActionResult.Success Then
                    '「0：成功」の場合
                    '処理なし

                ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                    '「-9000：DMS除外エラーの警告」の場合
                    '戻り値に「-9000：DMS除外エラーの警告」を設定
                    drWebServiceResult.RESULTCODE = ActionResult.WarningOmitDmsError

                Else
                    '上記以外の場合
                    '「15：他システムとの連携エラー」を返却
                    drWebServiceResult.RESULTCODE = ActionResult.DmsLinkageError
                    Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , ActionResult.DmsLinkageError))
                    Me.Rollback = True
                    Return drWebServiceResult

                End If

                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

            End Using

        End Using
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return drWebServiceResult
    End Function

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' ウェブサービス経由で更新する
    ''' </summary>
    ''' <param name="dtChipEntity">チップEntity</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="truncSecondDispStartDateTime">秒省略開始日時</param>
    ''' <param name="serviceWorkEndDateTime">終了日時</param>
    ''' <param name="serviceWorkTime">作業時間</param>
    ''' <param name="objStaffContext">スタフ情報</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="scheDeliDatetime">予定納車日時</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function CallWebServiceUpdate(ByVal dtChipEntity As TabletSmbCommonClassChipEntityDataTable, _
                                          ByVal stallId As Decimal, _
                                          ByVal truncSecondDispStartDateTime As Date, _
                                          ByVal serviceWorkEndDateTime As Date, _
                                          ByVal serviceWorkTime As Long, _
                                          ByVal objStaffContext As StaffContext, _
                                          ByVal updateDate As Date, _
                                          ByVal restFlg As String, _
                                          ByVal scheDeliDatetime As Date, _
                                          ByVal rowLockVersion As Long) As SMBCommonClassDataSet.WebServiceResultRow
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        , "{0}.{1} START " _
        , Me.GetType.ToString _
        , MethodBase.GetCurrentMethod.Name))
        'WebServiceを呼ぶためXML作成
        Dim drChipInfo As TabletSmbCommonClassChipEntityRow = dtChipEntity(0)
        Dim xmlclass As New SMBCommonClassBusinessLogic.XmlDocumentClass
        xmlclass = StructWebServiceXml(drChipInfo.JOB_DTL_ID.ToString(CultureInfo.InvariantCulture), _
                           "", _
                           stallId.ToString(CultureInfo.InvariantCulture), _
                           String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", truncSecondDispStartDateTime), _
                           String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", serviceWorkEndDateTime), _
                           serviceWorkTime.ToString(CultureInfo.InvariantCulture), _
                           objStaffContext, _
                           updateDate, _
                           GetWebServiceRestFlg(restFlg), _
                           String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", scheDeliDatetime), _
                           "", _
                           "", _
                           WorkOrderFlgOn, _
                           "", _
                           "", _
                           dtChipEntity(0).PICK_DELI_TYPE, _
                           String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", dtChipEntity(0).SCHE_SVCIN_DATETIME), _
                           CType(rowLockVersion, String))

        'WebServiceを呼ぶ
        Using commbiz As New SMBCommonClassBusinessLogic
            Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow = commbiz.CallReserveWebService(xmlclass)
            If drWebServiceResult.RESULTCODE <> ActionResult.Success Then
                'RowLockVersionError(最新のデータではない)の場合、ActionResult.RowLockVersionErrorを戻す
                If drWebServiceResult.RESULTCODE = WebServiceRowLockVersionError Then
                    drWebServiceResult.RESULTCODE = ActionResult.RowLockVersionError
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E RowLockVersionError. " _
                        , MethodBase.GetCurrentMethod.Name))
                    Return drWebServiceResult
                Else
                    drWebServiceResult.RESULTCODE = ActionResult.ExceptionError
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E ReceptionChipMoveUpdate failed. RESULTCODE={1}" _
                        , MethodBase.GetCurrentMethod.Name, drWebServiceResult.RESULTCODE))
                    Return drWebServiceResult
                End If
            Else
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} " _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

                Return drWebServiceResult
            End If
        End Using

    End Function

    ''' <summary>
    ''' ローカルで更新する
    ''' </summary>
    ''' <param name="dtChipEntity">チップEntity</param>
    ''' <param name="objStaffContext">スタフ情報</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="truncSecondDispStartDateTime">秒省略開始日時</param>
    ''' <param name="scheEndDateTime">予定終了日時</param>
    ''' <param name="serviceWorkTime">作業時間</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <param name="restFlg">休憩フラグ</param>
    ''' <param name="prmsEndDateTime">見込み終了日時</param>
    ''' <param name="scheDeliDatetime">予定納車日時</param>
    ''' <param name="staffcd">ログインユーザー</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="systemId">プログラムID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Private Function LocalUpdate(ByVal dtChipEntity As TabletSmbCommonClassChipEntityDataTable, _
                                 ByVal objStaffContext As StaffContext, _
                                 ByVal stallId As Decimal, _
                                 ByVal truncSecondDispStartDateTime As Date, _
                                 ByVal scheEndDateTime As Date, _
                                 ByVal serviceWorkTime As Long, _
                                 ByVal rowLockVersion As Long, _
                                 ByVal restFlg As String, _
                                 ByVal prmsEndDateTime As Date, _
                                 ByVal scheDeliDatetime As Date, _
                                 ByVal staffcd As String, _
                                 ByVal updateDate As Date, _
                                 ByVal systemId As String) As Long
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        , "{0}.{1} START " _
        , Me.GetType.ToString _
        , MethodBase.GetCurrentMethod.Name))

        '更新件数
        Dim cnt As Long
        '処理結果
        Dim result As Long
        Dim drChipInfo As TabletSmbCommonClassChipEntityRow = dtChipEntity(0)
        'サービス入庫をロックして、チェックする
        result = LockServiceInTable(drChipInfo.SVCIN_ID, rowLockVersion, staffcd, updateDate, systemId)
        If result <> ActionResult.Success Then
            Return result
        End If
        '変更前の情報を取得する
        Using tabletSMBCommondataAdapter As New TabletSMBCommonClassDataAdapter
            Dim dtServiceinBefore As TabletSmbCommonClassServiceinChangeInfoDataTable
            dtServiceinBefore = tabletSMBCommondataAdapter.GetChipChangeInfo(dtChipEntity(0).SVCIN_ID, objStaffContext.DlrCD, objStaffContext.BrnCD)
            'StallUseをupdate用データセット作成
            Using targetDtChipEntity As New TabletSmbCommonClassChipEntityDataTable
                Dim targetDrChipEntity As TabletSmbCommonClassChipEntityRow = CType(targetDtChipEntity.NewRow(), TabletSmbCommonClassChipEntityRow)

                targetDrChipEntity.STALL_USE_ID = drChipInfo.STALL_USE_ID
                targetDrChipEntity.STALL_ID = stallId
                targetDrChipEntity.SCHE_START_DATETIME = truncSecondDispStartDateTime
                targetDrChipEntity.SCHE_END_DATETIME = scheEndDateTime
                targetDrChipEntity.SCHE_WORKTIME = serviceWorkTime
                targetDrChipEntity.REST_FLG = restFlg
                targetDrChipEntity.UPDATE_DATETIME = updateDate
                targetDrChipEntity.UPDATE_STF_CD = objStaffContext.Account
                targetDrChipEntity.PRMS_END_DATETIME = prmsEndDateTime

                'StallUse更新処理を実行する
                cnt = tabletSMBCommondataAdapter.StallChipMoveResize(targetDrChipEntity, systemId)
                If cnt <> 1 Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E ReceptionChipMoveResize failed. cnt={1}" _
                                            , MethodBase.GetCurrentMethod.Name, cnt))
                    Return ActionResult.ExceptionError
                End If

                'サービスステータス更新判定
                Dim svcStatus As String
                If CheckSvcStatusUpdate(dtChipEntity(0).SVC_STATUS) Then
                    'サービスステータスを"「06:次の作業開始待ち」"更新する
                    svcStatus = SvcStatusNextStartWait
                Else
                    svcStatus = dtChipEntity(0).SVC_STATUS
                End If
                'サービス入庫テーブルを更新
                cnt = tabletSMBCommondataAdapter.UpdateSvcinUpdateJobDtlAttchment(dtChipEntity(0).SVCIN_ID, scheDeliDatetime, svcStatus, updateDate, objStaffContext.Account)
                If cnt <> 1 Then
                    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E ReceptionChipMoveResize failed. cnt={1}" _
                                            , MethodBase.GetCurrentMethod.Name, cnt))
                    Return ActionResult.ExceptionError
                End If
                '変更後の情報を取得する
                Dim dtServiceinAfter As TabletSmbCommonClassServiceinChangeInfoDataTable
                dtServiceinAfter = tabletSMBCommondataAdapter.GetChipChangeInfo(dtChipEntity(0).SVCIN_ID, objStaffContext.DlrCD, objStaffContext.BrnCD)
                '履歴登録
                cnt = CreateChipOperationHistory(dtServiceinBefore, dtServiceinAfter, updateDate, objStaffContext.Account, 0, systemId)
                If cnt <> 0 Then
                    Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.{1} HISINSERT FAILURE " _
                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
                    Return ActionResult.ExceptionError
                End If

                Return ActionResult.Success
            End Using
        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        , "{0}.{1} " _
        , Me.GetType.ToString _
        , MethodBase.GetCurrentMethod.Name))
    End Function

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    ''' <summary>
    ''' 受付エリアのチップストールに配置処理(Insert)
    ''' </summary>
    ''' <param name="svcInId">サービス入庫ID(親チップの)</param>
    ''' <param name="jobDtlId">作業内容ID(親チップの)</param>
    ''' <param name="stallId">変更後のストールのSTALLID</param>
    ''' <param name="dispStartDateTime">変更後の表示開始日時</param>
    ''' <param name="scheWorkTime">仕事時間</param>
    ''' <param name="restFlg">休憩取得フラグ</param>
    ''' <param name="stallStartTime">稼働開始日時</param>
    ''' <param name="stallEndTime">稼働終了日時</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="systemId">プログラムID</param>
    ''' <param name="scheDeliDatetime">予定納車日時</param>
    ''' <param name="mainteCode">整備コード</param>
    ''' <param name="workSeq">作業連番</param>
    ''' <param name="picrkDeliType">納車区分</param>
    ''' <param name="scheSvcinDateTime">予定入庫日時</param>
    ''' <param name="rowLockVersion">ROWロックバージョン</param>
    ''' <param name="roNum">RO連番</param>
    ''' <param name="inspectionNeedFlg">検査必要フラグ</param>
    ''' <returns>WebService処理結果</returns>
    ''' <history>
    ''' 2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' 2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応)
    ''' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証
    ''' </history>
    <EnableCommit()>
    Public Function ReceptionChipMoveInsert(ByVal svcInId As Decimal, _
                                            ByVal jobDtlId As Decimal, _
                                            ByVal stallId As Decimal, _
                                            ByVal dispStartDateTime As Date, _
                                            ByVal scheWorkTime As Long, _
                                            ByVal restFlg As String, _
                                            ByVal stallStartTime As Date, _
                                            ByVal stallEndTime As Date, _
                                            ByVal updateDate As Date, _
                                            ByVal objStaffContext As StaffContext, _
                                            ByVal systemId As String, _
                                            ByVal scheDeliDatetime As Date, _
                                            ByVal mainteCode As String, _
                                            ByVal workSeq As Long, _
                                            ByVal picrkDeliType As String, _
                                            ByVal scheSvcinDateTime As Date, _
                                            ByVal rowLockVersion As Long, _
                                            ByVal roNum As String, _
                                            ByVal inspectionNeedFlg As String) As SMBCommonClassDataSet.WebServiceResultRow

        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        'Public Function ReceptionChipMoveInsert(ByVal jobDtlId As Long, _
        '                                ByVal stallId As Long, _
        '                                ByVal dispStartDateTime As Date, _
        '                                ByVal scheWorkTime As Long, _
        '                                ByVal restFlg As String, _
        '                                ByVal stallStartTime As Date, _
        '                                ByVal stallEndTime As Date, _
        '                                ByVal updateDate As Date, _
        '                                ByVal objStaffContext As StaffContext, _
        '                                ByVal systemId As String, _
        '                                ByVal scheDeliDatetime As Date, _
        '                                ByVal mainteCode As String, _
        '                                ByVal partsFlg As String, _
        '                                ByVal workSeq As Long, _
        '                                ByVal picrkDeliType As String, _
        '                                ByVal scheSvcinDateTime As Date, _
        '                                ByVal rowLockVersion As Long, _
        '                                ByVal roNum As String) As Long
        'Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S." _
        '                        , MethodBase.GetCurrentMethod.Name))
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S, jobDtlId={1},stallId={2},dispStartDateTime={3},scheWorkTime={4},restFlg={5},stallStartTime={6},stallEndTime={7},updateDate={8},systemId={9},scheDeliDatetime={10},mainteCode={11},workSeq={12},picrkDeliType={13},scheSvcinDateTime={14},roNum={15},inspectionNeedFlg={16}" _
                        , MethodBase.GetCurrentMethod.Name, _
                        jobDtlId, _
                        stallId, _
                        dispStartDateTime, _
                        scheWorkTime, _
                        restFlg, _
                        stallStartTime, _
                        stallEndTime, _
                        updateDate, _
                        systemId, _
                        scheDeliDatetime, _
                        mainteCode, _
                        workSeq, _
                        picrkDeliType, _
                        scheSvcinDateTime, _
                        roNum, _
                        inspectionNeedFlg))
        '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '秒を切り捨てる
        Dim truncSecondDispStartDateTime As Date = Me.GetDateTimeFloorSecond(dispStartDateTime)

        'ローカル変数．作業終了日時として処理対象のストール利用．予定終了日時を保持する
        Dim serviceWorkEndDateTime As New Date
        Dim serviceWorkTime As Long = scheWorkTime
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        'serviceWorkEndDateTime = GetServiceEndDateTime(stallId, truncSecondDispStartDateTime, serviceWorkTime, stallStartTime, stallEndTime, restFlg)
        Dim serviceEndDateTimeData As ServiceEndDateTimeData = GetServiceEndDateTime(stallId, truncSecondDispStartDateTime, serviceWorkTime, stallStartTime, stallEndTime, restFlg)
        serviceWorkEndDateTime = serviceEndDateTimeData.ServiceEndDateTime
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        '返却用データ行宣言する
        Dim drWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow
        Using dtWebServiceResult As New SMBCommonClassDataSet.WebServiceResultDataTable
            drWebServiceResult = dtWebServiceResult.NewWebServiceResultRow
        End Using

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        '返却値の初期値設定
        drWebServiceResult.RESULTCODE = ActionResult.Success

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        'チップ操作制約チェックを行う
        Dim validate As Integer = ValidateMove(DefaultNumberValue, objStaffContext, stallId, truncSecondDispStartDateTime, scheWorkTime, serviceWorkEndDateTime, stallStartTime, stallEndTime, updateDate)
        If validate <> ActionResult.Success Then
            Me.Rollback = True
            drWebServiceResult.RESULTCODE = validate
            Logger.Info(String.Format(CultureInfo.CurrentCulture, "{0}.E ValidateMove failed ", MethodBase.GetCurrentMethod.Name))
            Return drWebServiceResult
        End If

        '2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応 START
        'サービス入庫リストを作成
        Dim svcidList As New List(Of Decimal)
        svcidList.Add(svcInId)

        'スタッフ情報を取得
        Dim staffInfo As StaffContext = StaffContext.Current

        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
        ' サービス分類コード
        Dim svcClassCd As String = Nothing
        ' ローカル変数の洗車必要フラグを'0'（洗車不要）で初期化
        Dim carwashNeedFlg As String = CarWashNeedFlgNeedless
        ' ローカル変数の検査必要フラグを'1'（検査必要）で初期化
        Dim localInspectionNeedFlg As String = InspectionNeedFlgNeed
        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

        Using ta As New TabletSMBCommonClassDataAdapter

            'サービス入庫IDより、チップ情報を取得
            Dim chipTable As TabletSmbCommonClassStallChipInfoDataTable = _
                ta.GetStallChipBySvcinId(staffInfo.DlrCD, _
                                         staffInfo.BrnCD, _
                                         svcidList)

            If chipTable.Count > 0 Then
                '0行以上の場合

                '追加作業が画面に置く時、サービスステータスをチェック
                Dim checkResult As Integer = _
                    Me.CheckSvcStatusByPlaningChip(chipTable(0).SVC_STATUS)

                '該当車両が洗車中、検査中、納車済の場合、置けない
                If checkResult <> ActionResult.Success Then

                    Me.Rollback = True
                    drWebServiceResult.RESULTCODE = checkResult

                    Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                               "{0}.End ValidateMove failed ", _
                                               MethodBase.GetCurrentMethod.Name))

                    Return drWebServiceResult

                End If

                ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
                ' チップ情報の中に洗車必要フラグが'1'（洗車必要）のデータがあった場合、ローカル変数の洗車必要フラグを'1'（洗車必要）にする。
                For i As Integer = 0 To chipTable.Count - 1
                    If CarWashNeedFlgNeed.Equals(chipTable(i).CARWASH_NEED_FLG) Then
                        carwashNeedFlg = CarWashNeedFlgNeed
                        Exit For
            End If
                Next

                ' ストールに紐づくサービス分類情報を取得する。
                Dim rowServiceClass As TabletSmbCommonClassServiceClassRow = GetSvcClassInfo(stallId)

                ' サービス分類情報を取得できた場合
                If (rowServiceClass IsNot Nothing) Then
                    ' サービス分類コード取得
                    svcClassCd = rowServiceClass.SVC_CLASS_CD

                    ' 洗車必要フラグ設定
                    ' ローカル変数の洗車必要フラグが'0'（洗車不要）の場合、サービス分類情報の洗車必要フラグを設定する。
                    If CarWashNeedFlgNeedless.Equals(carwashNeedFlg) Then
                        carwashNeedFlg = rowServiceClass.CARWASH_NEED_FLG
                    End If

                    ' 検査必要フラグ設定
                    If (SvcClassTypeEM.Equals(rowServiceClass.SVC_CLASS_TYPE) _
                        OrElse SvcClassTypeFM.Equals(rowServiceClass.SVC_CLASS_TYPE)) Then
                        ' サービス分類情報．サービス分類区分が「"1"：ＥＭ、"2"：ＰＭ」の場合
                        ' ローカル変数の検査必要フラグを'0'（検査不要）にする。
                        localInspectionNeedFlg = InspectionNeedFlgNeedless

                    Else
                        ' 上記以外の場合
                        ' ローカル変数の検査必要フラグを'1'（検査必要）にする。
                        localInspectionNeedFlg = InspectionNeedFlgNeed
                    End If
                End If
                ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

            End If

        End Using
        '2014/09/25 TMEJ 張 BTS-180 「洗車中に関連チップ作成すると予期せぬエラーメッセージ」対応 END

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

        'Dim cnt As Long
        'Using da As New TabletSMBCommonClassDataAdapter
        '    'WebServiceを呼ぶためXML作成
        '    Dim xmlclass As New SMBCommonClassBusinessLogic.XmlDocumentClass
        '    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        '    'xmlclass = StructWebServiceXml("", _
        '    '                               jobDtlId.ToString(CultureInfo.InvariantCulture), _
        '    '                               stallId.ToString(CultureInfo.InvariantCulture), _
        '    '                               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", truncSecondDispStartDateTime), _
        '    '                               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", serviceWorkEndDateTime), _
        '    '                               serviceWorkTime.ToString(CultureInfo.InvariantCulture), _
        '    '                               objStaffContext, _
        '    '                               updateDate, _
        '    '                               GetWebServiceRestFlg(restFlg), _
        '    '                               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", scheDeliDatetime), _
        '    '                               mainteCode, _
        '    '                               "", _
        '    '                               WorkOrderFlgOn, _
        '    '                               "", _
        '    '                               "", _
        '    '                               picrkDeliType, _
        '    '                               String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", scheSvcinDateTime), _
        '    '                               CType(rowLockVersion, String))
        '    xmlclass = StructWebServiceXml("", _
        '                       jobDtlId.ToString(CultureInfo.InvariantCulture), _
        '                       stallId.ToString(CultureInfo.InvariantCulture), _
        '                       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", truncSecondDispStartDateTime), _
        '                       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", serviceWorkEndDateTime), _
        '                       serviceWorkTime.ToString(CultureInfo.InvariantCulture), _
        '                       objStaffContext, _
        '                       updateDate, _
        '                       GetWebServiceRestFlg(restFlg), _
        '                       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", scheDeliDatetime), _
        '                       mainteCode, _
        '                       inspectionNeedFlg, _
        '                       WorkOrderFlgOn, _
        '                       "", _
        '                       "", _
        '                       picrkDeliType, _
        '                       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", scheSvcinDateTime), _
        '                       CType(rowLockVersion, String))
        '    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '    'WebServiceを呼ぶ
        '    Using commbiz As New SMBCommonClassBusinessLogic
        '        drWebServiceResult = commbiz.CallReserveWebService(xmlclass)

        '        If drWebServiceResult.RESULTCODE <> ActionResult.Success Then
        '            Me.Rollback = True

        '            'RowLockVersionError(最新のデータではない)の場合、ActionResult.RowLockVersionErrorを戻す
        '            If drWebServiceResult.RESULTCODE = WebServiceRowLockVersionError Then
        '                drWebServiceResult.RESULTCODE = ActionResult.RowLockVersionError
        '                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E RowLockVersionError. " _
        '                    , MethodBase.GetCurrentMethod.Name))
        '                Return drWebServiceResult

        '            Else
        '                drWebServiceResult.RESULTCODE = ActionResult.ExceptionError
        '                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E ReceptionChipMoveInsert failed. RESULTCODE={1}" _
        '                    , MethodBase.GetCurrentMethod.Name, drWebServiceResult.RESULTCODE))
        '                Return drWebServiceResult

        '            End If

        '        Else
        '            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        '            'If PartsFlgOn.Equals(partsFlg) Then
        '            '    '部品準備完了フラグ更新
        '            '    cnt = da.UpdatePartsFlg(drWebServiceResult.STALL_USE_ID, objStaffContext.Account, updateDate, systemId)
        '            '    If cnt <> 1 Then
        '            '        Return ActionResult.ExceptionError
        '            '    End If
        '            'End If
        '            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        '            '旧作業指示を削除する前保持する
        '            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
        '            'Dim dtJobInstruct As TabletSmbCommonClassJobInstructDataTable = da.GetJobInstruct(workSeq, roNum)
        '            Dim dtJobInstruct As TabletSmbCommonClassJobInstructDataTable = _
        '                da.GetJobInstruct(workSeq, _
        '                                  roNum, _
        '                                  objStaffContext.DlrCD, _
        '                                  objStaffContext.BrnCD)

        '            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

        '            '旧作業指示を削除する
        '            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 START
        '            'cnt = da.DeleteJobInstruct(workSeq, roNum)
        '            cnt = da.DeleteJobInstruct(workSeq, _
        '                                       roNum, _
        '                                       objStaffContext.DlrCD, _
        '                                       objStaffContext.BrnCD)
        '            '2014/09/12 TMEJ 張 BTS-392「TopServのROとSMBのJOBが不一致」対応 END

        '            If Not cnt > 0 Then
        '                Me.Rollback = True
        '                drWebServiceResult.RESULTCODE = ActionResult.ExceptionError
        '                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E JobInstruct failed. cnt={1}" _
        '                                        , MethodBase.GetCurrentMethod.Name, cnt))
        '                Return drWebServiceResult
        '            End If

        '            '着工指示待ちのJobをループで着工指示をINSERTする
        '            For Each drJobInstruct As TabletSmbCommonClassJobInstructRow In dtJobInstruct
        '                '着工指示
        '                drJobInstruct.JOB_DTL_ID = drWebServiceResult.JOB_DTL_ID
        '                cnt = da.InsertJobInstructBinding(drJobInstruct, _
        '                                  updateDate, _
        '                                  objStaffContext.Account, _
        '                                  systemId)
        '                If cnt <> 1 Then
        '                    drWebServiceResult.RESULTCODE = ActionResult.ExceptionError
        '                    Me.Rollback = True
        '                    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E JobInstruct failed. cnt={1}" _
        '                                            , MethodBase.GetCurrentMethod.Name, cnt))
        '                    Return drWebServiceResult
        '                End If
        '            Next

        '            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '            '予約送信ため、変更後のチップステータスを取得する
        '            Dim crntStatus As String = Me.JudgeChipStatus(drWebServiceResult.STALL_USE_ID)
        '            Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(drWebServiceResult.STALL_USE_ID)
        '            If dtChipEntity.Count <> 1 Then
        '                drWebServiceResult.RESULTCODE = ActionResult.ExceptionError
        '                Me.Rollback = True
        '                Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
        '                                        , MethodBase.GetCurrentMethod.Name))
        '                Return drWebServiceResult
        '            End If

        '            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        '            ''紐付けを基幹システム側に反映連携
        '            'result = Me.InstructJob(roNum, workSeq, objStaffContext, dtChipEntity(0).JOB_DTL_ID)
        '            'If result <> ActionResult.Success Then
        '            '    Return result
        '            'End If
        '            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

        '            '予約送信
        '            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 START
        '            'cnt = Me.SendReserveInfo(dtChipEntity(0).SVCIN_ID, drWebServiceResult.JOB_DTL_ID, drWebServiceResult.STALL_USE_ID, crntStatus, _
        '            '                    crntStatus, dtChipEntity(0).RESV_STATUS, systemId)
        '            Using biz3800903 As New IC3800903BusinessLogic

        '                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START
        '                'cnt = biz3800903.SendReserveInfo(dtChipEntity(0).SVCIN_ID, _
        '                '                                 drWebServiceResult.JOB_DTL_ID, _
        '                '                                 drWebServiceResult.STALL_USE_ID, _
        '                '                                 crntStatus, _
        '                '                                 crntStatus, _
        '                '                                 dtChipEntity(0).RESV_STATUS, _
        '                '                                 systemId)

        '                '予約連携実施
        '                Dim returnCodeSendReserve As Integer = biz3800903.SendReserveInfo(dtChipEntity(0).SVCIN_ID, _
        '                                                                                  drWebServiceResult.JOB_DTL_ID, _
        '                                                                                  drWebServiceResult.STALL_USE_ID, _
        '                                                                                  crntStatus, _
        '                                                                                  crntStatus, _
        '                                                                                  dtChipEntity(0).RESV_STATUS, _
        '                                                                                  systemId)

        '                '処理結果チェック
        '                If returnCodeSendReserve = ActionResult.Success Then
        '                    '「0：成功」の場合
        '                    '処理なし

        '                ElseIf returnCodeSendReserve = ActionResult.WarningOmitDmsError Then
        '                    '「-9000：DMS除外エラーの警告」の場合
        '                    '戻り値に「-9000：DMS除外エラーの警告」を設定
        '                    drWebServiceResult.RESULTCODE = ActionResult.WarningOmitDmsError

        '                Else
        '                    '上記以外の場合
        '                    '「15：他システムとの連携エラー」を返却
        '                    drWebServiceResult.RESULTCODE = ActionResult.DmsLinkageError
        '                    Me.Rollback = True
        '                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                        , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
        '                        , Me.GetType.ToString _
        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                        , ActionResult.DmsLinkageError))
        '                    Return drWebServiceResult

        '                End If

        '                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '            End Using

        '            '2013/12/05 TMEJ 明瀬 タブレット版SMB チーフテクニシャン機能開発 END

        '            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '            'If cnt <> 0 Then
        '            '    drWebServiceResult.RESULTCODE = ActionResult.DmsLinkageError
        '            '    Me.Rollback = True
        '            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendReserveInfo FAILURE " _
        '            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
        '            '    Return drWebServiceResult
        '            'End If

        '            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
        '            'ステータス送信
        '            Using ic3802601blc As New IC3802601BusinessLogic
        '                'ステータス連携実施
        '                Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(dtChipEntity(0).SVCIN_ID, _
        '                                                                        drWebServiceResult.JOB_DTL_ID, _
        '                                                                        drWebServiceResult.STALL_USE_ID, _
        '                                                                        crntStatus, _
        '                                                                        crntStatus, _
        '                                                                        0)

        '                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '                'If dmsSendResult <> 0 Then
        '                '    drWebServiceResult.RESULTCODE = ActionResult.DmsLinkageError
        '                '    Me.Rollback = True
        '                '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
        '                '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
        '                '    Return drWebServiceResult
        '                'End If

        '                '処理結果チェック
        '                If dmsSendResult = ActionResult.Success Then
        '                    '「0：成功」の場合
        '                    '処理なし

        '                ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
        '                    '「-9000：DMS除外エラーの警告」の場合
        '                    '戻り値に「-9000：DMS除外エラーの警告」を設定
        '                    drWebServiceResult.RESULTCODE = ActionResult.WarningOmitDmsError

        '                Else
        '                    '上記以外の場合
        '                    '「15：他システムとの連携エラー」を返却
        '                    drWebServiceResult.RESULTCODE = ActionResult.DmsLinkageError
        '                    Me.Rollback = True
        '                    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                        , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
        '                        , Me.GetType.ToString _
        '                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                        , ActionResult.DmsLinkageError))
        '                    Return drWebServiceResult

        '                End If

        '                '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '            End Using

        '            '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END
        '        End If
        '    End Using

        'End Using

        'DB処理の実施
        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
        'drWebServiceResult = _
        '    Me.ReceptionChipMoveInsertDB(jobDtlId, _
        '                                 stallId, _
        '                                 restFlg, _
        '                                 updateDate, _
        '                                 objStaffContext, _
        '                                 systemId, _
        '                                 scheDeliDatetime, _
        '                                 mainteCode, _
        '                                 workSeq, _
        '                                 picrkDeliType, _
        '                                 scheSvcinDateTime, _
        '                                 rowLockVersion, _
        '                                 roNum, _
        '                                 inspectionNeedFlg, _
        '                                 truncSecondDispStartDateTime, _
        '                                 serviceWorkEndDateTime, _
        '                                 serviceWorkTime)

        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        'drWebServiceResult = _
        '    Me.ReceptionChipMoveInsertDB(jobDtlId, _
        '                                 stallId, _
        '                                 restFlg, _
        '                                 updateDate, _
        '                                 objStaffContext, _
        '                                 systemId, _
        '                                 scheDeliDatetime, _
        '                                 mainteCode, _
        '                                 workSeq, _
        '                                 picrkDeliType, _
        '                                 scheSvcinDateTime, _
        '                                 rowLockVersion, _
        '                                 roNum, _
        '                                 inspectionNeedFlg, _
        '                                 truncSecondDispStartDateTime, _
        '                                 serviceWorkEndDateTime, _
        '                                 serviceWorkTime)
        drWebServiceResult = _
            Me.ReceptionChipMoveInsertDB(jobDtlId, _
                                         stallId, _
                                         serviceEndDateTimeData.RestFlg, _
                                         updateDate, _
                                         objStaffContext, _
                                         systemId, _
                                         scheDeliDatetime, _
                                         mainteCode, _
                                         workSeq, _
                                         picrkDeliType, _
                                         scheSvcinDateTime, _
                                         rowLockVersion, _
                                         roNum, _
                                         localInspectionNeedFlg, _
                                         truncSecondDispStartDateTime, _
                                         serviceWorkEndDateTime, _
                                         serviceWorkTime, _
                                         svcClassCd, _
                                         carwashNeedFlg)
        '2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        '処理結果チェック
        If drWebServiceResult.RESULTCODE <> ActionResult.Success AndAlso _
           drWebServiceResult.RESULTCODE <> ActionResult.WarningOmitDmsError Then
            '「0：成功」「-9000：DMS除外エラーの警告」でないの場合
            'ロールバックをして処理結果を返却
            Me.Rollback = True
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END RETURNCODE={2}[ReceptionChipMoveInsertDB FAILURE]" _
                , Me.GetType.ToString _
                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                , drWebServiceResult.RESULTCODE))
            Return drWebServiceResult

        End If

        '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))
        Return drWebServiceResult
    End Function

    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) START

    ''' <summary>
    ''' 受付エリアのチップストールに配置処理のDB処理
    ''' </summary>
    ''' <param name="inJobDtlId">作業内容ID(親チップの)</param>
    ''' <param name="inStallId">変更後のストールのSTALLID</param>
    ''' <param name="inRestFlg">休憩取得フラグ</param>
    ''' <param name="inUpdateDate">更新日時</param>
    ''' <param name="inStaffInfo">スタッフ情報</param>
    ''' <param name="inSystemId">プログラムID</param>
    ''' <param name="inScheDeliDatetime">予定納車日時</param>
    ''' <param name="inMainteCode">整備コード</param>
    ''' <param name="inWorkSeq">作業連番</param>
    ''' <param name="inPickDeliType">納車区分</param>
    ''' <param name="inScheSvcinDateTime">予定入庫日時</param>
    ''' <param name="inRowLockVersion">ROWロックバージョン</param>
    ''' <param name="inRoNum">RO連番</param>
    ''' <param name="inInspectionNeedFlg">検査必要フラグ</param>
    ''' <param name="inTruncSecondDispStartDateTime">変更後の表示開始日時(秒切捨て)</param>
    ''' <param name="inServiceWorkEndDateTime">作業終了日時</param>
    ''' <param name="inServiceWorkTime">予定作業時間</param>
    ''' <param name="svcCd">サービスコード</param>
    ''' <param name="carwashNeedFlg">洗車必要フラグ</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証
    ''' </history>
    Private Function ReceptionChipMoveInsertDB(ByVal inJobDtlId As Decimal, _
                                               ByVal inStallId As Decimal, _
                                               ByVal inRestFlg As String, _
                                               ByVal inUpdateDate As Date, _
                                               ByVal inStaffInfo As StaffContext, _
                                               ByVal inSystemId As String, _
                                               ByVal inScheDeliDatetime As Date, _
                                               ByVal inMainteCode As String, _
                                               ByVal inWorkSeq As Long, _
                                               ByVal inPickDeliType As String, _
                                               ByVal inScheSvcinDateTime As Date, _
                                               ByVal inRowLockVersion As Long, _
                                               ByVal inRoNum As String, _
                                               ByVal inInspectionNeedFlg As String, _
                                               ByVal inTruncSecondDispStartDateTime As Date, _
                                               ByVal inServiceWorkEndDateTime As Date, _
                                               ByVal inServiceWorkTime As Long, _
                                               ByVal svcCd As String, _
                                               ByVal carwashNeedFlg As String) As SMBCommonClassDataSet.WebServiceResultRow

        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '    , "{0}.{1} START:inJobDtlId={2},inStallId={3},inRestFlg={4},inUpdateDate={5},inStaffInfo=StaffInfo,inSystemId={6},inScheDeliDatetime={7},inMainteCode={8},inWorkSeq={9},inPickDeliType={10},inScheSvcinDateTime={11},inRowLockVersion={12},inRoNum={13},inInspectionNeedFlg={14},inTruncSecondDispStartDateTime={15},inServiceWorkEndDateTime={16},inServiceWorkTime={17}" _
        '    , Me.GetType.ToString _
        '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '    , inJobDtlId.ToString(CultureInfo.CurrentCulture) _
        '    , inStallId.ToString(CultureInfo.CurrentCulture) _
        '    , inRestFlg _
        '    , inUpdateDate.ToString(CultureInfo.CurrentCulture) _
        '    , inSystemId _
        '    , inScheDeliDatetime.ToString(CultureInfo.CurrentCulture) _
        '    , inMainteCode _
        '    , inWorkSeq.ToString(CultureInfo.CurrentCulture) _
        '    , inPickDeliType _
        '    , inScheSvcinDateTime.ToString(CultureInfo.CurrentCulture) _
        '    , inRowLockVersion.ToString(CultureInfo.CurrentCulture) _
        '    , inRoNum _
        '    , inInspectionNeedFlg _
        '    , inTruncSecondDispStartDateTime.ToString(CultureInfo.CurrentCulture) _
        '    , inServiceWorkEndDateTime.ToString(CultureInfo.CurrentCulture) _
        '    , inServiceWorkTime.ToString(CultureInfo.CurrentCulture)))

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START:inJobDtlId={2},inStallId={3},inRestFlg={4},inUpdateDate={5},inStaffInfo=StaffInfo,inSystemId={6},inScheDeliDatetime={7},inMainteCode={8},inWorkSeq={9},inPickDeliType={10},inScheSvcinDateTime={11},inRowLockVersion={12},inRoNum={13},inInspectionNeedFlg={14},inTruncSecondDispStartDateTime={15},inServiceWorkEndDateTime={16},inServiceWorkTime={17},svcCd={18},carwashNeedFlg={19}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inJobDtlId.ToString(CultureInfo.CurrentCulture) _
            , inStallId.ToString(CultureInfo.CurrentCulture) _
            , inRestFlg _
            , inUpdateDate.ToString(CultureInfo.CurrentCulture) _
            , inSystemId _
            , inScheDeliDatetime.ToString(CultureInfo.CurrentCulture) _
            , inMainteCode _
            , inWorkSeq.ToString(CultureInfo.CurrentCulture) _
            , inPickDeliType _
            , inScheSvcinDateTime.ToString(CultureInfo.CurrentCulture) _
            , inRowLockVersion.ToString(CultureInfo.CurrentCulture) _
            , inRoNum _
            , inInspectionNeedFlg _
            , inTruncSecondDispStartDateTime.ToString(CultureInfo.CurrentCulture) _
            , inServiceWorkEndDateTime.ToString(CultureInfo.CurrentCulture) _
            , inServiceWorkTime.ToString(CultureInfo.CurrentCulture) _
            , svcCd _
            , carwashNeedFlg))
        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

        '戻り値
        Dim returnWebServiceResult As SMBCommonClassDataSet.WebServiceResultRow

        Using da As New TabletSMBCommonClassDataAdapter
            'WebServiceを呼ぶためXML作成
            ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
            'Dim xmlclass As SMBCommonClassBusinessLogic.XmlDocumentClass = _
            '    Me.StructWebServiceXml("", _
            '                           inJobDtlId.ToString(CultureInfo.InvariantCulture), _
            '                           inStallId.ToString(CultureInfo.InvariantCulture), _
            '                           String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", inTruncSecondDispStartDateTime), _
            '                           String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", inServiceWorkEndDateTime), _
            '                           inServiceWorkTime.ToString(CultureInfo.InvariantCulture), _
            '                           inStaffInfo, _
            '                           inUpdateDate, _
            '                           GetWebServiceRestFlg(inRestFlg), _
            '                           String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", inScheDeliDatetime), _
            '                           inMainteCode, _
            '                           inInspectionNeedFlg, _
            '                           WorkOrderFlgOn, _
            '                           "", _
            '                           "", _
            '                           inPickDeliType, _
            '                           String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", inScheSvcinDateTime), _
            '                           CType(inRowLockVersion, String))

            Dim xmlclass As SMBCommonClassBusinessLogic.XmlDocumentClass = _
                Me.StructWebServiceXml("", _
                                       inJobDtlId.ToString(CultureInfo.InvariantCulture), _
                                       inStallId.ToString(CultureInfo.InvariantCulture), _
                                       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", inTruncSecondDispStartDateTime), _
                                       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", inServiceWorkEndDateTime), _
                                       inServiceWorkTime.ToString(CultureInfo.InvariantCulture), _
                                       inStaffInfo, _
                                       inUpdateDate, _
                                       GetWebServiceRestFlg(inRestFlg), _
                                       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", inScheDeliDatetime), _
                                       inMainteCode, _
                                       inInspectionNeedFlg, _
                                       WorkOrderFlgOn, _
                                       "", _
                                       "", _
                                       inPickDeliType, _
                                       String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", inScheSvcinDateTime), _
                                       CType(inRowLockVersion, String), _
                                       svcCd, _
                                       carwashNeedFlg)
            ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

            Using commbiz As New SMBCommonClassBusinessLogic

                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} ⑨SC3240301_受付エリアからのチップ配置処理 [WebServiceで更新] START" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

                '予約更新登録WebService呼出処理
                returnWebServiceResult = commbiz.CallReserveWebService(xmlclass)

                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} ⑨SC3240301_受付エリアからのチップ配置処理 [WebServiceで更新] END" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name))
                '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

                '処理結果チェック
                If returnWebServiceResult.RESULTCODE <> ActionResult.Success Then
                    '「0：成功」以外の場合
                    'エラー内容チェック
                    If returnWebServiceResult.RESULTCODE = WebServiceRowLockVersionError Then
                        'RowLockVersionError(最新のデータではない)の場合
                        '「12：行ロックバージョンエラー」を返却
                        returnWebServiceResult.RESULTCODE = ActionResult.RowLockVersionError
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[CallReserveWebService FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ActionResult.RowLockVersionError))
                        Return returnWebServiceResult

                    Else
                        '上記以外のエラーの場合
                        '「22：予期せぬエラー」を返却
                        returnWebServiceResult.RESULTCODE = ActionResult.ExceptionError
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[CallReserveWebService FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ActionResult.ExceptionError))
                        Return returnWebServiceResult

                    End If

                Else
                    '旧作業指示を削除する前保持する
                    Dim dtJobInstruct As TabletSmbCommonClassJobInstructDataTable = _
                        da.GetJobInstruct(inWorkSeq, _
                                          inRoNum, _
                                          inStaffInfo.DlrCD, _
                                          inStaffInfo.BrnCD)

                    '旧作業指示を削除する
                    Dim deleteJobInstructCount As Long = da.DeleteJobInstruct(inWorkSeq, _
                                                                              inRoNum, _
                                                                              inStaffInfo.DlrCD, _
                                                                              inStaffInfo.BrnCD)

                    '処理件数チェック
                    If deleteJobInstructCount <= 0 Then
                        '0件以下の場合
                        '「22：予期せぬエラー」を返却
                        returnWebServiceResult.RESULTCODE = ActionResult.ExceptionError
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[DeleteJobInstruct FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ActionResult.ExceptionError))
                        Return returnWebServiceResult

                    End If

                    '着工指示待ちのJobをループで着工指示をINSERTする
                    For Each drJobInstruct As TabletSmbCommonClassJobInstructRow In dtJobInstruct
                        '着工指示処理用にデータ格納
                        drJobInstruct.JOB_DTL_ID = returnWebServiceResult.JOB_DTL_ID

                        '着工指示処理
                        Dim insertJobInstructBindingCount As Long = _
                            da.InsertJobInstructBinding(drJobInstruct, _
                                                        inUpdateDate, _
                                                        inStaffInfo.Account, _
                                                        inSystemId)

                        '処理結果チェック
                        If insertJobInstructBindingCount <> 1 Then
                            '1件でない場合
                            '「22：予期せぬエラー」を返却
                            returnWebServiceResult.RESULTCODE = ActionResult.ExceptionError
                            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END RETURNCODE={2}[InsertJobInstructBinding FAILURE]" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ActionResult.ExceptionError))
                            Return returnWebServiceResult

                        End If

                    Next

                    '予約送信ため、変更後のチップステータスを取得する
                    Dim crntStatus As String = Me.JudgeChipStatus(returnWebServiceResult.STALL_USE_ID)

                    '更新後チップエンティティ取得
                    Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = _
                        Me.GetChipEntity(returnWebServiceResult.STALL_USE_ID)

                    '取得情報チェック
                    If dtChipEntity.Count <> 1 Then
                        '取得できなかった場合
                        '「22：予期せぬエラー」を返却
                        returnWebServiceResult.RESULTCODE = ActionResult.ExceptionError
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END RETURNCODE={2}[GetChipEntity FAILURE]" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , ActionResult.ExceptionError))
                        Return returnWebServiceResult

                    End If

                    '予約送信
                    Using biz3800903 As New IC3800903BusinessLogic

                        '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ⑩受付エリアからのチップ配置処理 [予約連携] START" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                        '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

                        '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 START

                        '予約連携実施
                        'Dim returnCodeSendReserve As Integer = _
                        '    biz3800903.SendReserveInfo(dtChipEntity(0).SVCIN_ID, _
                        '                               returnWebServiceResult.JOB_DTL_ID, _
                        '                               returnWebServiceResult.STALL_USE_ID, _
                        '                               crntStatus, _
                        '                               crntStatus, _
                        '                               dtChipEntity(0).RESV_STATUS, _
                        '                               inSystemId)
                        Dim returnCodeSendReserve As Integer = _
                            biz3800903.SendReserveInfo(dtChipEntity(0).SVCIN_ID, _
                                                       returnWebServiceResult.JOB_DTL_ID, _
                                                       returnWebServiceResult.STALL_USE_ID, _
                                                       crntStatus, _
                                                       crntStatus, _
                                                       dtChipEntity(0).RESV_STATUS, _
                                                       inSystemId, _
                                                       Nothing, _
                                                       False, _
                                                       True)

                        '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 ログ出力強化対応 END

                        '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ⑩SC3240301_受付エリアからのチップ配置処理 [予約連携] END" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                        '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

                        '処理結果チェック
                        If returnCodeSendReserve = ActionResult.Success Then
                            '「0：成功」の場合
                            '処理なし

                        ElseIf returnCodeSendReserve = ActionResult.WarningOmitDmsError Then
                            '「-9000：DMS除外エラーの警告」の場合
                            '戻り値に「-9000：DMS除外エラーの警告」を設定
                            returnWebServiceResult.RESULTCODE = ActionResult.WarningOmitDmsError

                        Else
                            '上記以外の場合

                            '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 START

                            ''「15：他システムとの連携エラー」を返却
                            'returnWebServiceResult.RESULTCODE = ActionResult.DmsLinkageError
                            'Logger.Error(String.Format(CultureInfo.CurrentCulture _
                            '    , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                            '    , Me.GetType.ToString _
                            '    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            '    , ActionResult.DmsLinkageError))

                            '予約連携送信のエラーコードが文言コードの場合、文言コード（エラーコード）を返す。
                            '文言コードでない場合、「15：他システムとの連携エラー」を返す。
                            returnWebServiceResult.RESULTCODE = CheckReturnCodeSendReserveError(returnCodeSendReserve)

                            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                    , "{0}.{1} END RETURNCODE={2}[SendReserveInfo FAILURE]" _
                                    , Me.GetType.ToString _
                                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                    , returnWebServiceResult.RESULTCODE))

                            '2015/10/08 TM 皆川 タブレットSMBのチップ移動時の連携送信メッセージ詳細化 END

                            Return returnWebServiceResult

                        End If

                    End Using

                    'ステータス送信
                    Using ic3802601blc As New IC3802601BusinessLogic

                        '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ⑪SC3240301_受付エリアからのチップ配置処理 [ステータス連携] START" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                        '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

                        'ステータス連携実施
                        Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(dtChipEntity(0).SVCIN_ID, _
                                                                                returnWebServiceResult.JOB_DTL_ID, _
                                                                                returnWebServiceResult.STALL_USE_ID, _
                                                                                crntStatus, _
                                                                                crntStatus, _
                                                                                0)

                        '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 START
                        Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} ⑪SC3240301_受付エリアからのチップ配置処理 [ステータス連携] END" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name))
                        '2015/06/26 TMEJ 小澤 受付エリアからの着工指示時のログ出力 END

                        '処理結果チェック
                        If dmsSendResult = ActionResult.Success Then
                            '「0：成功」の場合
                            '処理なし

                        ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                            '「-9000：DMS除外エラーの警告」の場合
                            '戻り値に「-9000：DMS除外エラーの警告」を設定
                            returnWebServiceResult.RESULTCODE = ActionResult.WarningOmitDmsError

                        Else
                            '上記以外の場合
                            '「15：他システムとの連携エラー」を返却
                            returnWebServiceResult.RESULTCODE = ActionResult.DmsLinkageError
                            Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                                , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                                , Me.GetType.ToString _
                                , System.Reflection.MethodBase.GetCurrentMethod.Name _
                                , ActionResult.DmsLinkageError))
                            Return returnWebServiceResult

                        End If

                    End Using

                End If

            End Using

        End Using

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} END RETURNCODE={2}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , returnWebServiceResult.RESULTCODE))
        Return returnWebServiceResult

    End Function

    '2015/05/28 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発(コード分析対応) END

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 START
    ''' <summary>
    ''' 着工指示の通知処理
    ''' </summary>
    ''' <param name="drNoticeInfo">通知情報行</param>
    ''' <param name="userInfo">スタフ情報</param>
    ''' <param name="startDateTime">予約の開始日時</param>
    ''' <param name="endDateTime">予約の終了日時</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    Public Sub JobInstructNotice(ByVal drNoticeInfo As TabletSmbCommonClassNoticeInfoRow, _
                              ByVal userInfo As StaffContext, _
                              ByVal startDateTime As Date, _
                              ByVal endDateTime As Date, _
                              ByVal stallId As Decimal)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} START " _
            , Me.GetType.ToString _
            , MethodBase.GetCurrentMethod.Name))

        LogServiceCommonBiz.OutputLog(9, "●■● 1.4.1 ServiceCommonClass_001 START")

        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapRow = Me.GetDmsDlrBrnCode(userInfo.DlrCD, userInfo.BrnCD, userInfo.Account)

        LogServiceCommonBiz.OutputLog(9, "●■● 1.4.1 ServiceCommonClass_001 END")

        If Not IsNothing(dmsDlrBrnTable) Then

            LogServiceCommonBiz.OutputLog(10, "●■● 1.4.2 通知APIに渡す情報を作成 START")

            '基幹販売店コード
            drNoticeInfo.DearlerCode = dmsDlrBrnTable.CODE1
            '基幹店舗コード
            drNoticeInfo.BranchCode = dmsDlrBrnTable.CODE2
            'ログインユーザー
            drNoticeInfo.LoginUserID = dmsDlrBrnTable.ACCOUNT
            '枝番「0」で固定
            drNoticeInfo.SEQ_NO = "0"

            '現在日付を取得
            Dim nowDate As Date = DateTimeFunc.Now(userInfo.DlrCD)
            '開始時間
            Dim startTime As String
            Dim startTimeDateFormat As String
            '当日ではない場合、日付で表示
            If String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd}", nowDate).Equals(String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd}", startDateTime)) Then
                startTimeDateFormat = DateTimeFunc.GetDateFormat(14)
            Else
                startTimeDateFormat = DateTimeFunc.GetDateFormat(11)
            End If
            startTime = String.Format(CultureInfo.InvariantCulture, "{0:" & startTimeDateFormat & "}", startDateTime)

            '終了時間
            Dim endTime As String
            Dim endTimeDateFormat As String
            '当日ではない場合、日付で表示
            If String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd}", nowDate).Equals(String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd}", endDateTime)) Then
                endTimeDateFormat = DateTimeFunc.GetDateFormat(14)
            Else
                endTimeDateFormat = DateTimeFunc.GetDateFormat(11)
            End If
            endTime = String.Format(CultureInfo.InvariantCulture, "{0:" & endTimeDateFormat & "}", endDateTime)

            '顧客名敬称付く
            If Not drNoticeInfo.IsNAMETITLE_NAMENull AndAlso _
                Not String.IsNullOrEmpty(drNoticeInfo.NAMETITLE_NAME) Then
                Dim cstName As New StringBuilder
                If Position_Type_Before.Equals(drNoticeInfo.POSITION_TYPE) Then
                    cstName.Append(drNoticeInfo.NAMETITLE_NAME)
                    cstName.Append(drNoticeInfo.CST_NAME)
                Else
                    cstName.Append(drNoticeInfo.CST_NAME)
                    cstName.Append(drNoticeInfo.NAMETITLE_NAME)
                End If
                drNoticeInfo.CST_NAME = cstName.ToString
            End If

            '通知のメッセージを作成する
            Dim noticeMessage As New StringBuilder
            noticeMessage.Append(WebWordUtility.GetWord(ProgramId_SubChipBox, 8))
            noticeMessage.Append(Space(3))
            noticeMessage.Append(drNoticeInfo.R_O)
            noticeMessage.Append(Space(3))
            noticeMessage.Append(drNoticeInfo.VCLREGNO)
            noticeMessage.Append(Space(3))
            noticeMessage.Append(drNoticeInfo.CST_NAME)
            noticeMessage.Append(Space(3))
            noticeMessage.Append(startTime)
            noticeMessage.Append(WebWordUtility.GetWord(ProgramId_SubChipBox, 10))
            noticeMessage.Append(endTime)
            noticeMessage.Append(Space(3))
            If Not drNoticeInfo.IsMAINTE_NAMENull Then
                noticeMessage.Append(drNoticeInfo.MAINTE_NAME)
            End If

            drNoticeInfo.Message = noticeMessage.ToString

            '送り先リストを作成
            Dim stallList As New List(Of Decimal)
            stallList.Add(stallId)

            LogServiceCommonBiz.OutputLog(10, "●■● 1.4.2 通知APIに渡す情報を作成 END")

            LogServiceCommonBiz.OutputLog(11, "●■● 1.4.3 通知先アカウント取得 START")

            Dim toAccountList As List(Of String) = Me.GetNoticeAccountList(userInfo, stallList)

            LogServiceCommonBiz.OutputLog(11, "●■● 1.4.3 通知先アカウント取得 END")

            LogServiceCommonBiz.OutputLog(14, "●■● 1.4.4 通知処理 START")

            '通知共通関数を呼ぶ
            Me.Notice(toAccountList, userInfo, drNoticeInfo)

            LogServiceCommonBiz.OutputLog(14, "●■● 1.4.4 通知処理 END")

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END " _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End If
    End Sub

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    ''' <summary>
    ''' 着工指示の通知処理
    ''' </summary>
    ''' <param name="drNoticeInfo">通知情報行</param>
    ''' <param name="startDateTime">予約の開始日時</param>
    ''' <param name="endDateTime">予約の終了日時</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inStaffAccount">ログインスタッフアカウント</param>
    ''' <param name="inStaffName">ログインスタッフ名</param>
    ''' <remarks></remarks>
    Public Sub JobInstructNotice(ByVal drNoticeInfo As TabletSmbCommonClassNoticeInfoRow, _
                                 ByVal startDateTime As Date, _
                                 ByVal endDateTime As Date, _
                                 ByVal stallId As Decimal, _
                                 ByVal inDealerCode As String, _
                                 ByVal inBranchCode As String, _
                                 ByVal inStaffAccount As String, _
                                 ByVal inStaffName As String)

        Logger.Info(String.Format( _
                    CultureInfo.CurrentCulture, _
                    "{0}.{1} START", _
                    Me.GetType.ToString, _
                    MethodBase.GetCurrentMethod.Name))

        LogServiceCommonBiz.OutputLog(9, "●■● 1.4.1 ServiceCommonClass_001 START")

        Dim dmsDlrBrnTable As ServiceCommonClassDataSet.DmsCodeMapRow = _
            Me.GetDmsDlrBrnCode(inDealerCode, _
                                inBranchCode, _
                                inStaffAccount)

        LogServiceCommonBiz.OutputLog(9, "●■● 1.4.1 ServiceCommonClass_001 END")

        If Not IsNothing(dmsDlrBrnTable) Then

            LogServiceCommonBiz.OutputLog(10, "●■● 1.4.2 通知APIに渡す情報を作成 START")

            '基幹販売店コード
            drNoticeInfo.DearlerCode = dmsDlrBrnTable.CODE1
            '基幹店舗コード
            drNoticeInfo.BranchCode = dmsDlrBrnTable.CODE2
            'ログインユーザー
            drNoticeInfo.LoginUserID = dmsDlrBrnTable.ACCOUNT
            '枝番「0」で固定
            drNoticeInfo.SEQ_NO = "0"

            '現在日付を取得
            Dim nowDate As Date = DateTimeFunc.Now(inDealerCode)
            '開始時間
            Dim startTime As String
            Dim startTimeDateFormat As String
            '当日ではない場合、日付で表示
            If String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd}", nowDate).Equals(String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd}", startDateTime)) Then
                startTimeDateFormat = DateTimeFunc.GetDateFormat(14)
            Else
                startTimeDateFormat = DateTimeFunc.GetDateFormat(11)
            End If
            startTime = String.Format(CultureInfo.InvariantCulture, "{0:" & startTimeDateFormat & "}", startDateTime)

            '終了時間
            Dim endTime As String
            Dim endTimeDateFormat As String
            '当日ではない場合、日付で表示
            If String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd}", nowDate).Equals(String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd}", endDateTime)) Then
                endTimeDateFormat = DateTimeFunc.GetDateFormat(14)
            Else
                endTimeDateFormat = DateTimeFunc.GetDateFormat(11)
            End If
            endTime = String.Format(CultureInfo.InvariantCulture, "{0:" & endTimeDateFormat & "}", endDateTime)

            '顧客名敬称付く
            If Not drNoticeInfo.IsNAMETITLE_NAMENull AndAlso _
                Not String.IsNullOrEmpty(drNoticeInfo.NAMETITLE_NAME) Then
                Dim cstName As New StringBuilder
                If Position_Type_Before.Equals(drNoticeInfo.POSITION_TYPE) Then
                    cstName.Append(drNoticeInfo.NAMETITLE_NAME)
                    cstName.Append(drNoticeInfo.CST_NAME)
                Else
                    cstName.Append(drNoticeInfo.CST_NAME)
                    cstName.Append(drNoticeInfo.NAMETITLE_NAME)
                End If
                drNoticeInfo.CST_NAME = cstName.ToString
            End If

            '通知のメッセージを作成する
            Dim noticeMessage As New StringBuilder
            noticeMessage.Append(WebWordUtility.GetWord(ProgramId_SubChipBox, 8))
            noticeMessage.Append(Space(3))
            noticeMessage.Append(drNoticeInfo.R_O)
            noticeMessage.Append(Space(3))
            noticeMessage.Append(drNoticeInfo.VCLREGNO)
            noticeMessage.Append(Space(3))
            noticeMessage.Append(drNoticeInfo.CST_NAME)
            noticeMessage.Append(Space(3))
            noticeMessage.Append(startTime)
            noticeMessage.Append(WebWordUtility.GetWord(ProgramId_SubChipBox, 10))
            noticeMessage.Append(endTime)
            noticeMessage.Append(Space(3))
            If Not drNoticeInfo.IsMAINTE_NAMENull Then
                noticeMessage.Append(drNoticeInfo.MAINTE_NAME)
            End If

            drNoticeInfo.Message = noticeMessage.ToString

            '送り先リストを作成
            Dim stallList As New List(Of Decimal)
            stallList.Add(stallId)

            LogServiceCommonBiz.OutputLog(10, "●■● 1.4.2 通知APIに渡す情報を作成 END")

            LogServiceCommonBiz.OutputLog(11, "●■● 1.4.3 通知先アカウント取得 START")

            Dim toAccountList As List(Of String) = Me.GetNoticeAccountList(inDealerCode, _
                                                                           inBranchCode, _
                                                                           inStaffAccount, _
                                                                           stallList)

            LogServiceCommonBiz.OutputLog(11, "●■● 1.4.3 通知先アカウント取得 END")

            LogServiceCommonBiz.OutputLog(14, "●■● 1.4.4 通知処理 START")

            '通知共通関数を呼ぶ
            Me.Notice(toAccountList, _
                      inDealerCode, _
                      inBranchCode, _
                      inStaffAccount, _
                      inStaffName, _
                      drNoticeInfo)

            LogServiceCommonBiz.OutputLog(14, "●■● 1.4.4 通知処理 END")

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                , "{0}.{1} END " _
                , Me.GetType.ToString _
                , MethodBase.GetCurrentMethod.Name))

        End If
    End Sub

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END
    ''' <summary>
    ''' 通知対象にPush
    ''' </summary>
    ''' <param name="objStaffContext">スタフ情報</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    Public Sub NoticeAccountPush(ByVal objStaffContext As StaffContext, _
                                  ByVal stallId As Decimal)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        , "{0}.{1} START " _
        , Me.GetType.ToString _
        , MethodBase.GetCurrentMethod.Name))
        '自分を除外するCHTリスト
        Dim toAccountExceptedCHTList As New List(Of String)
        '指定TCリスト
        Dim toAccountListTC As New List(Of String)
        '指定のCHTリスト
        Dim toAccountListCHT As New List(Of String)
        Dim stallIdList As New List(Of Decimal)
        stallIdList.Add(stallId)

        'TCとCHT権限のユーザーを取得

        LogServiceCommonBiz.OutputLog(19, "●■● 1.5.1 TABLETSMBCOMMONCLASS_062 START")

        toAccountListTC = Me.GetSendStaffCodeTC(objStaffContext.DlrCD, objStaffContext.BrnCD, stallIdList)

        LogServiceCommonBiz.OutputLog(19, "●■● 1.5.1 TABLETSMBCOMMONCLASS_062 END")

        LogServiceCommonBiz.OutputLog(20, "●■● 1.5.2 TABLETSMBCOMMONCLASS_061 START")

        toAccountListCHT = Me.GetSendStaffCodeCht(objStaffContext.DlrCD, objStaffContext.BrnCD, stallIdList)

        LogServiceCommonBiz.OutputLog(20, "●■● 1.5.2 TABLETSMBCOMMONCLASS_061 END")

        '自分以外のCHTを送信先リストに追加
        For Each toAccountCHT As String In toAccountListCHT
            If objStaffContext.Account.Equals(toAccountCHT) Then
                Continue For
            End If
            toAccountExceptedCHTList.Add(toAccountCHT)
        Next

        LogServiceCommonBiz.OutputLog(21, "●■● 1.5.3 TCへPush処理 START")

        'TCに対してPUSHする
        SendPushByStaffCodeList(toAccountListTC, PUSH_FuntionNM)

        LogServiceCommonBiz.OutputLog(21, "●■● 1.5.3 TCへPush処理[送信件数：" & toAccountListTC.Count & "] END")
        '件数

        LogServiceCommonBiz.OutputLog(22, "●■● 1.5.4 CHTへPush処理 START")

        'CHTに対してPUSHする
        SendPushByStaffCodeList(toAccountExceptedCHTList, PUSH_FuntionTabletSMB)

        LogServiceCommonBiz.OutputLog(22, "●■● 1.5.4 CHTへPush処理[送信件数：" & toAccountExceptedCHTList.Count & "] END")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        , "{0}.{1} END " _
        , Me.GetType.ToString _
        , MethodBase.GetCurrentMethod.Name))

    End Sub

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    ''' <summary>
    ''' 通知対象にPush
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inStaffAccount">ログインスタッフアカウント</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    Public Sub NoticeAccountPush(ByVal inDealerCode As String, _
                                 ByVal inBranchCode As String, _
                                 ByVal inStaffAccount As String, _
                                 ByVal stallId As Decimal)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        , "{0}.{1} START " _
        , Me.GetType.ToString _
        , MethodBase.GetCurrentMethod.Name))
        '自分を除外するCHTリスト
        Dim toAccountExceptedCHTList As New List(Of String)
        '指定TCリスト
        Dim toAccountListTC As New List(Of String)
        '指定のCHTリスト
        Dim toAccountListCHT As New List(Of String)
        Dim stallIdList As New List(Of Decimal)
        stallIdList.Add(stallId)

        'TCとCHT権限のユーザーを取得

        LogServiceCommonBiz.OutputLog(19, "●■● 1.5.1 TABLETSMBCOMMONCLASS_062 START")

        toAccountListTC = Me.GetSendStaffCodeTC(inDealerCode, inBranchCode, stallIdList)

        LogServiceCommonBiz.OutputLog(19, "●■● 1.5.1 TABLETSMBCOMMONCLASS_062 END")

        LogServiceCommonBiz.OutputLog(20, "●■● 1.5.2 TABLETSMBCOMMONCLASS_061 START")

        toAccountListCHT = Me.GetSendStaffCodeCht(inDealerCode, inBranchCode, stallIdList)

        LogServiceCommonBiz.OutputLog(20, "●■● 1.5.2 TABLETSMBCOMMONCLASS_061 END")

        '自分以外のCHTを送信先リストに追加
        For Each toAccountCHT As String In toAccountListCHT
            If inStaffAccount.Equals(toAccountCHT) Then
                Continue For
            End If
            toAccountExceptedCHTList.Add(toAccountCHT)
        Next

        LogServiceCommonBiz.OutputLog(21, "●■● 1.5.3 TCへPush処理 START")

        'TCに対してPUSHする
        SendPushByStaffCodeList(toAccountListTC, _
                                PUSH_FuntionNM, _
                                inDealerCode)

        LogServiceCommonBiz.OutputLog(21, "●■● 1.5.3 TCへPush処理[送信件数：" & toAccountListTC.Count & "] END")
        '件数

        LogServiceCommonBiz.OutputLog(22, "●■● 1.5.4 CHTへPush処理 START")

        'CHTに対してPUSHする
        SendPushByStaffCodeList(toAccountExceptedCHTList, _
                                PUSH_FuntionTabletSMB, _
                                inDealerCode)

        LogServiceCommonBiz.OutputLog(22, "●■● 1.5.4 CHTへPush処理[送信件数：" & toAccountExceptedCHTList.Count & "] END")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        , "{0}.{1} END " _
        , Me.GetType.ToString _
        , MethodBase.GetCurrentMethod.Name))

    End Sub

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

    ''' <summary>
    ''' 着工指示Push
    ''' </summary>
    ''' <param name="objStaffContext">スタフ情報</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    Public Sub JobInstructPush(ByVal objStaffContext As StaffContext, _
                                ByVal stallId As Decimal)
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        , "{0}.{1} START " _
        , Me.GetType.ToString _
        , MethodBase.GetCurrentMethod.Name))
        'CTとCHT
        Dim pushUsersList As New List(Of String)
        Dim operationCodeList As New List(Of Decimal)
        Dim exceptStaffCodeList As New List(Of String)

        'CTとCHT権限を追加
        operationCodeList.Add(Operation.CT)
        operationCodeList.Add(Operation.CHT)
        Dim stallIdList As New List(Of Decimal)
        stallIdList.Add(stallId)

        LogServiceCommonBiz.OutputLog(24, "●■● 1.6.1 TABLETSMBCOMMONCLASS_061 START")

        'ストールIDのCHT権限のユーザーを取得
        Dim toAccountListCHT As List(Of String) = Me.GetSendStaffCodeCht(objStaffContext.DlrCD, objStaffContext.BrnCD, stallIdList)

        LogServiceCommonBiz.OutputLog(24, "●■● 1.6.1 TABLETSMBCOMMONCLASS_061 END")

        '自分を除外する
        exceptStaffCodeList.Add(objStaffContext.Account)
        '指定のCHTを除外する
        exceptStaffCodeList.AddRange(toAccountListCHT)

        LogServiceCommonBiz.OutputLog(25, "●■● 1.6.2 VisitUtility_002 START")

        '自分以外のCTとCHT権限のユーザーを取得
        pushUsersList = Me.GetSendStaffCode(objStaffContext.DlrCD, objStaffContext.BrnCD, operationCodeList, exceptStaffCodeList)

        LogServiceCommonBiz.OutputLog(25, "●■● 1.6.2 VisitUtility_002 END")

        LogServiceCommonBiz.OutputLog(26, "●■● 1.6.3 CTへPush処理 START")

        'ユーザーリストに対してPUSHする
        SendPushByStaffCodeList(pushUsersList, PUSH_FuntionTabletSMB)

        LogServiceCommonBiz.OutputLog(26, "●■● 1.6.3 CTへPush処理[送信件数：" & pushUsersList.Count & "] END")

        LogServiceCommonBiz.OutputLog(27, "●■● 1.6.4 PSへPush処理 START")

        '全てPSにPushする
        Me.SendAllPSPush(objStaffContext.DlrCD, objStaffContext.BrnCD)

        LogServiceCommonBiz.OutputLog(27, "●■● 1.6.4 PSへPush処理 END")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        , "{0}.{1} " _
        , Me.GetType.ToString _
        , MethodBase.GetCurrentMethod.Name))
    End Sub

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    ''' <summary>
    ''' 着工指示Push
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inStaffAccount">ログインスタッフアカウント</param>
    ''' <param name="stallId">ストールID</param>
    ''' <remarks></remarks>
    '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
    'Public Sub JobInstructPush(ByVal inDealerCode As String, _
    '                           ByVal inBranchCode As String, _
    '                           ByVal inStaffAccount As String, _
    '                           ByVal stallId As Decimal)
    Public Sub JobInstructPush(ByVal inDealerCode As String, _
                           ByVal inBranchCode As String, _
                           ByVal inStaffAccount As String, _
                           ByVal stallId As Decimal, _
                           ByVal pushCompleteFlg As Boolean)
        '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END
        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        , "{0}.{1} START " _
        , Me.GetType.ToString _
        , MethodBase.GetCurrentMethod.Name))
        'CTとCHT
        Dim pushUsersList As New List(Of String)
        Dim operationCodeList As New List(Of Decimal)
        Dim exceptStaffCodeList As New List(Of String)

        'CTとCHT権限を追加
        operationCodeList.Add(Operation.CT)
        operationCodeList.Add(Operation.CHT)
        Dim stallIdList As New List(Of Decimal)
        stallIdList.Add(stallId)

        '自分を除外する
        exceptStaffCodeList.Add(inStaffAccount)
        '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない START
        ''指定のCHTを除外する
        'exceptStaffCodeList.AddRange(toAccountListCHT)

        'Push送信が完了している場合、指定のCHTにはPushを送信しない
        If pushCompleteFlg Then

            LogServiceCommonBiz.OutputLog(24, "●■● 1.6.1 TABLETSMBCOMMONCLASS_061 START")

            'ストールIDのCHT権限のユーザーを取得
            Dim toAccountListCHT As List(Of String) = Me.GetSendStaffCodeCht(inDealerCode, inBranchCode, stallIdList)

            LogServiceCommonBiz.OutputLog(24, "●■● 1.6.1 TABLETSMBCOMMONCLASS_061 END")

            '指定のCHTを除外する
            exceptStaffCodeList.AddRange(toAccountListCHT)
        End If
        '2017/11/13 NSK 竹中(悠) TR-SVT-TMT-20170830-001 CHTが追加ジョブをチップにマージすると部品監視画面が更新されず、アラートがならない END
        LogServiceCommonBiz.OutputLog(25, "●■● 1.6.2 VisitUtility_002 START")

        '自分以外のCTとCHT権限のユーザーを取得
        pushUsersList = Me.GetSendStaffCode(inDealerCode, inBranchCode, operationCodeList, exceptStaffCodeList)

        LogServiceCommonBiz.OutputLog(25, "●■● 1.6.2 VisitUtility_002 END")

        LogServiceCommonBiz.OutputLog(26, "●■● 1.6.3 CTへPush処理 START")

        'ユーザーリストに対してPUSHする
        SendPushByStaffCodeList(pushUsersList, _
                                PUSH_FuntionTabletSMB, _
                                inDealerCode)

        LogServiceCommonBiz.OutputLog(26, "●■● 1.6.3 CTへPush処理[送信件数：" & pushUsersList.Count & "] END")

        LogServiceCommonBiz.OutputLog(27, "●■● 1.6.4 PSへPush処理 START")

        '全てPSにPushする
        Me.SendAllPSPush(inDealerCode, _
                         inBranchCode, _
                         True)

        LogServiceCommonBiz.OutputLog(27, "●■● 1.6.4 PSへPush処理 END")

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        , "{0}.{1} " _
        , Me.GetType.ToString _
        , MethodBase.GetCurrentMethod.Name))
    End Sub

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

    ''' <summary>
    ''' 着工指示の通知対象ユーザーリスト取得
    ''' </summary>
    ''' <param name="userInfo">スタフ情報</param>
    ''' <param name="stallIdList">ストールIDリスト</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNoticeAccountList(ByVal userInfo As StaffContext, _
                                          ByVal stallIdList As List(Of Decimal)) As List(Of String)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        , "{0}.{1} START " _
        , Me.GetType.ToString _
        , MethodBase.GetCurrentMethod.Name))

        Dim toAccountList As New List(Of String)
        Dim toAccountListTC As New List(Of String)
        Dim toAccountListCHT As New List(Of String)

        'TCとCHT権限のユーザーを取得

        LogServiceCommonBiz.OutputLog(12, "●■● 1.4.3.1 TABLETSMBCOMMONCLASS_062 START")

        toAccountListTC = Me.GetSendStaffCodeTC(userInfo.DlrCD, userInfo.BrnCD, stallIdList)

        LogServiceCommonBiz.OutputLog(12, "●■● 1.4.3.1 TABLETSMBCOMMONCLASS_062 END")

        LogServiceCommonBiz.OutputLog(13, "●■● 1.4.3.2 TABLETSMBCOMMONCLASS_061 START")

        toAccountListCHT = Me.GetSendStaffCodeCht(userInfo.DlrCD, userInfo.BrnCD, stallIdList)

        LogServiceCommonBiz.OutputLog(13, "●■● 1.4.3.2 TABLETSMBCOMMONCLASS_061 END")

        '自分以外のCHTを送信先リストに追加
        For Each toAccountCHT As String In toAccountListCHT
            If userInfo.Account.Equals(toAccountCHT) Then
                Continue For
            End If
            toAccountList.Add(toAccountCHT)
        Next

        '指定TCを送信リストに追加
        toAccountList.AddRange(toAccountListTC)

        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        , "{0}.{1} " _
        , Me.GetType.ToString _
        , MethodBase.GetCurrentMethod.Name))

        Return toAccountList
    End Function

    '2013/11/29 TMEJ 丁 タブレット版SMB チーフテクニシャン機能開発 END

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 START

    ''' <summary>
    ''' 着工指示の通知対象ユーザーリスト取得
    ''' </summary>
    ''' <param name="inDealerCode">販売店コード</param>
    ''' <param name="inBranchCode">店舗コード</param>
    ''' <param name="inStaffAccount">ログインスタッフアカウント</param>
    ''' <param name="stallIdList">ストールIDリスト</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNoticeAccountList(ByVal inDealerCode As String, _
                                         ByVal inBranchCode As String, _
                                         ByVal inStaffAccount As String, _
                                         ByVal stallIdList As List(Of Decimal)) As List(Of String)

        Logger.Info(String.Format( _
                    CultureInfo.CurrentCulture, _
                    "{0}.{1} START", _
                    Me.GetType.ToString, _
                    MethodBase.GetCurrentMethod.Name))

        Dim toAccountList As New List(Of String)
        Dim toAccountListTC As New List(Of String)
        Dim toAccountListCHT As New List(Of String)

        'TCとCHT権限のユーザーを取得

        LogServiceCommonBiz.OutputLog(12, "●■● 1.4.3.1 TABLETSMBCOMMONCLASS_062 START")

        toAccountListTC = Me.GetSendStaffCodeTC(inDealerCode, _
                                                inBranchCode, _
                                                stallIdList)

        LogServiceCommonBiz.OutputLog(12, "●■● 1.4.3.1 TABLETSMBCOMMONCLASS_062 END")

        LogServiceCommonBiz.OutputLog(13, "●■● 1.4.3.2 TABLETSMBCOMMONCLASS_061 START")

        toAccountListCHT = Me.GetSendStaffCodeCht(inDealerCode, _
                                                  inBranchCode, _
                                                  stallIdList)

        LogServiceCommonBiz.OutputLog(13, "●■● 1.4.3.2 TABLETSMBCOMMONCLASS_061 END")

        '自分以外のCHTを送信先リストに追加
        For Each toAccountCHT As String In toAccountListCHT
            If inStaffAccount.Equals(toAccountCHT) Then
                Continue For
            End If
            toAccountList.Add(toAccountCHT)
        Next

        '指定TCを送信リストに追加
        toAccountList.AddRange(toAccountListTC)

        Logger.Info(String.Format( _
                    CultureInfo.CurrentCulture, _
                    "{0}.{1} END", _
                    Me.GetType.ToString, _
                    MethodBase.GetCurrentMethod.Name))

        Return toAccountList

    End Function

    '2015/07/17 TMEJ 明瀬 タブレットSMB性能改善 通知WebService化 END

#End Region

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START

#Region "Undo処理"

    '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
    ' ''' <summary>
    ' ''' 作業中チップのUndo処理
    ' ''' </summary>
    ' ''' <param name="stallUseId">ストール利用ID</param>
    ' ''' <param name="updateDate">更新日時</param>
    ' ''' <param name="objStaffContext">スタッフ情報</param>
    ' ''' <param name="systemId">プログラムID</param>
    ' ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ' ''' <history>
    ' ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ' ''' </history>
    'Public Function UndoWorkingChip(ByVal stallUseId As Decimal, _
    '                                ByVal updateDate As Date, _
    '                                ByVal objStaffContext As StaffContext, _
    '                                ByVal systemId As String) As Long
    ''' <summary>
    ''' 作業中チップのUndo処理
    ''' </summary>
    ''' <param name="stallUseId">ストール利用ID</param>
    ''' <param name="stallStartTime">サービス営業開始時間</param>
    ''' <param name="stallEndTime">サービス営業終了時間</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="objStaffContext">スタッフ情報</param>
    ''' <param name="systemId">プログラムID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    Public Function UndoWorkingChip(ByVal stallUseId As Decimal, _
                                    ByVal stallStartTime As Date, _
                                    ByVal stallEndTime As Date, _
                                    ByVal updateDate As Date, _
                                    ByVal objStaffContext As StaffContext, _
                                    ByVal systemId As String) As Long
        '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.  stallUseId={1}, updateDate={2}, staffCode={3}" _
                                , MethodBase.GetCurrentMethod.Name, stallUseId, updateDate, objStaffContext.Account))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        '*******************************************************
        '* Step1. データを準備
        '*******************************************************
        ' チップエンティティを取得する
        Dim dtChipEntity As TabletSmbCommonClassChipEntityDataTable = GetChipEntity(stallUseId)
        If dtChipEntity.Count <> 1 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E GetChipEntityError" _
                , MethodBase.GetCurrentMethod.Name))
            Return ActionResult.GetChipEntityError
        End If

        Dim svcinId As Decimal = dtChipEntity(0).SVCIN_ID
        'undo先の開始時間と終了時間
        Dim scheWorkTime As Long = dtChipEntity(0).SCHE_WORKTIME
        Dim undoStartTime As Date = dtChipEntity(0).SCHE_START_DATETIME
        Dim undoEndTime As Date = dtChipEntity(0).SCHE_END_DATETIME
        Dim undoStallId As Decimal = dtChipEntity(0).STALL_ID

        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START

        'チップ操作履歴からUndo前の作業時間を取得する
        Dim stallUseIdList As New List(Of Decimal)
        stallUseIdList.Add(stallUseId)
        Dim dtChipHisInfo As TabletSmbCommonClassChipHisDataTable = _
            GetWorkingChipHis(stallUseIdList)

        If (0 < dtChipHisInfo.Rows.Count) Then
            scheWorkTime = dtChipHisInfo(0).SCHE_WORKTIME
        End If

        '営業開始と終了時間を取得する
        '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        'Dim dtBranchOperatingHours As TabletSmbCommonClassBranchOperatingHoursDataTable = _
        '    Me.GetOneDayBrnOperatingHours(undoStartTime, _
        '                                  objStaffContext.DlrCD, _
        '                                  objStaffContext.BrnCD)
        '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        '休憩を取得する場合の作業終了日時を取得
        '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
        'Dim serviceEndDateTimeData As ServiceEndDateTimeData = _
        '    GetServiceEndDateTime(undoStallId, _
        '                               undoStartTime, _
        '                               scheWorkTime, _
        '                               dtBranchOperatingHours(0).SVC_JOB_START_TIME, _
        '                               dtBranchOperatingHours(0).SVC_JOB_END_TIME, _
        '                               RestTimeGetFlgGetRest)
        Dim serviceEndDateTimeData As ServiceEndDateTimeData = _
            GetServiceEndDateTime(undoStallId, _
                                       undoStartTime, _
                                       scheWorkTime, _
                                        stallStartTime, _
                                        stallEndTime, _
                                       RestTimeGetFlgGetRest)
        '2019/07/22 NSK 近藤 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        Dim autoJudgeRestFlg As String

        'Undo後の予定終了日時より再計算した予定終了日時（休憩あり）が大きい場合
        If undoEndTime < serviceEndDateTimeData.ServiceEndDateTime Then
            'Undo後の休憩取得フラグを取得しないに設定
            autoJudgeRestFlg = RestTimeGetFlgNoGetRest
        Else
            'Undo後の休憩取得フラグを、自動判別結果の休憩フラグに設定
            autoJudgeRestFlg = serviceEndDateTimeData.RestFlg
        End If
        '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END

        '作業実績送信使用するフラグを取得する
        Dim isUseJobDispatch As Boolean = Me.IsUseJobDispatch()

        '作業実績送信の場合、作業ステータスを取得する
        Dim prevJobStatus As IC3802701JobStatusDataTable = Nothing
        If isUseJobDispatch Then
            prevJobStatus = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)
        End If

        '更新前のステータス取得
        Dim prevStatus As String = Me.JudgeChipStatus(stallUseId)
        Dim rsltStallUses As TabletSmbCommonClassNumberValueDataTable

        '*******************************************************
        '* Step2. Undo操作チェック
        '*******************************************************
        Dim checkResult As Long = CheckUndoAction(stallUseId, _
                                                  undoStallId, _
                                                  dtChipEntity(0).SVC_STATUS, _
                                                  dtChipEntity(0).STALL_USE_STATUS, _
                                                  objStaffContext.DlrCD, _
                                                  objStaffContext.BrnCD, _
                                                  undoStartTime, _
                                                  undoEndTime, _
                                                  updateDate)
        If checkResult <> ActionResult.Success Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:Code is {1}." _
                                     , MethodBase.GetCurrentMethod.Name _
                                     , checkResult))
            Return checkResult
        End If

        '*******************************************************
        '* Step3. DB更新
        '*******************************************************
        Using ta As New TabletSMBCommonClassDataAdapter

            'ROステータスをUndoする
            Me.UndoRoStatus(svcinId, _
                            dtChipEntity(0).JOB_DTL_ID, _
                            stallUseId, _
                            updateDate, _
                            objStaffContext.Account, _
                            systemId)


            '作業実績テーブル対応データをundoする
            Dim undoJobResultResult As Long = Me.UndoJobResult(dtChipEntity(0).JOB_DTL_ID, _
                                                               updateDate, _
                                                               objStaffContext.Account, _
                                                               systemId)
            If undoJobResultResult <> ActionResult.Success Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:ExceptionError." _
                                         , MethodBase.GetCurrentMethod.Name))
                Return ActionResult.ExceptionError
            End If

            Dim resvStatus As String = ResvStatusConfirmed
            Dim svcStatus As String = SvcStatusStartwait

            ' 関連チップに実績チップのjobdtlidを取得する
            rsltStallUses = ta.GetRsltStallUses(svcinId)
            ' 対象のチップに実績が存在する場合は作業開始待ちへUndoする
            If rsltStallUses.Any() Then
                ' 実績が存在するチップが関連チップの場合は、次の作業開始待ちとなる
                If rsltStallUses.Select(String.Format(CultureInfo.CurrentCulture, "COL1 <> {0}", dtChipEntity(0).JOB_DTL_ID)).ToArray().Count > 0 Then
                    svcStatus = SvcStatusNextStartWait
                End If
            Else
                Dim chipHistories As TabletSmbCommonClassChipHisDataTable = ta.GetChipHis(svcinId, dtChipEntity(0).JOB_DTL_ID)
                '履歴テーブルにデータがあれば
                If chipHistories.Count > 0 Then
                    resvStatus = chipHistories.First().RESV_STATUS
                    svcStatus = chipHistories.First().SVC_STATUS
                    scheWorkTime = chipHistories.First().SCHE_WORKTIME
                End If
            End If

            Dim stallUseStatus As String = StalluseStatusStartWait
            '開始する前のサービ入庫スステータスより、ストール利用ステータスを設定する
            Dim isWorkOrderWait As Boolean = SvcStatusWorkOrderWait.Equals(svcStatus)
            Dim isStartWait As Boolean = SvcStatusStartwait.Equals(svcStatus)
            If isWorkOrderWait Or isStartWait Then
                If isWorkOrderWait Then
                    '着工指示待ち
                    stallUseStatus = StalluseStatusWorkOrderWait
                Else
                    '作業開始待ち
                    stallUseStatus = StalluseStatusStartWait
                End If
            End If

            'スタッフ作業(スタッフジョブテーブルに)を削除する
            Dim jobidList As New List(Of Decimal)
            jobidList.Add(dtChipEntity(0).JOB_ID)
            ta.DeleteStaffJobByJobid(ConvertDecimalArrayToString(jobidList))

            'サービス入庫テーブルの更新
            Dim updateSvcinCnt As Long = ta.UpdateSvcinTblForUndo(svcinId, svcStatus, resvStatus, updateDate, objStaffContext.Account)
            If updateSvcinCnt <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:UpdateSvcinTblForUndo failed." _
                                            , MethodBase.GetCurrentMethod.Name))
                Return ActionResult.ExceptionError
            End If

            'ストール利用テーブルの更新
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 START
            'Dim updateStallUseCnt As Long = ta.UpdateStallUseTblForUndo(stallUseId, stallUseStatus, scheWorkTime, DefaultDateTimeValueGet(), updateDate, objStaffContext.Account)
            Dim updateStallUseCnt As Long = ta.UpdateStallUseTblForUndo(stallUseId, stallUseStatus, scheWorkTime, DefaultDateTimeValueGet(), autoJudgeRestFlg, updateDate, objStaffContext.Account)
            '2019/06/18 NSK 皆川 (トライ店システム評価)次世代サービスオペレーション効率化に向けた、業務適合性検証 END
            If updateStallUseCnt <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:UpdateStallUseTblForUndo failed." _
                                            , MethodBase.GetCurrentMethod.Name))
                Return ActionResult.ExceptionError
            End If

        End Using

        '更新後のステータス取得
        Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)

        '基幹側にステータス情報を送信
        Using ic3802601blc As New IC3802601BusinessLogic
            Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(svcinId, _
                                                                    dtChipEntity(0).JOB_DTL_ID, _
                                                                    stallUseId, _
                                                                    prevStatus, _
                                                                    crntStatus, _
                                                                    0)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If dmsSendResult <> 0 Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} Error:SendStatusInfo FAILURE " _
            '                               , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '処理結果チェック
            If dmsSendResult = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If
            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using

        '実績送信使用の場合
        If isUseJobDispatch Then

            '作業ステータスを取得する
            Dim crntJobStatus As IC3802701JobStatusDataTable = JudgeJobStatus(dtChipEntity(0).JOB_DTL_ID)

            '基幹側にJobDispatch実績情報を送信
            Dim resultSendJobClock As Long = Me.SendJobClockOnInfo(svcinId, _
                                                                   dtChipEntity(0).JOB_DTL_ID, _
                                                                   prevJobStatus, _
                                                                   crntJobStatus)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If resultSendJobClock <> ActionResult.Success Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1}.End DmsLinkageError:SendJobClockOnInfo FAILURE " _
            '                                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '処理結果チェック
            If resultSendJobClock = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf resultSendJobClock = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendJobClockOnInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If
            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End If

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    ''' <summary>
    ''' Undo操作で最初作業のROステータスを60→50に変更する
    ''' </summary>
    ''' <param name="inSvcinId">サービス入庫ID</param>
    ''' <param name="inJobDtlId">作業内容ID</param>
    ''' <param name="inStallUseId">ストール利用ID</param>
    ''' <param name="inUpdateDateTime">更新日時</param>
    ''' <param name="inStaffCode">更新スタッフ</param>
    ''' <param name="inUpdateFunction">更新画面ID</param>
    ''' <remarks></remarks>
    Private Sub UndoRoStatus(ByVal inSvcinId As Decimal, _
                             ByVal inJobDtlId As Decimal, _
                             ByVal inStallUseId As Decimal, _
                             ByVal inUpdateDateTime As Date, _
                             ByVal inStaffCode As String, _
                             ByVal inUpdateFunction As String)

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.", MethodBase.GetCurrentMethod.Name))

        Using ta As New TabletSMBCommonClassDataAdapter
            ta.UndoROStatus(inSvcinId, _
                            inJobDtlId, _
                            inStallUseId, _
                            inUpdateDateTime, _
                            inStaffCode, _
                            inUpdateFunction)
        End Using

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

    End Sub

    ''' <summary>
    ''' 作業実績テーブルから指定作業内容IDのデータを削除する
    ''' </summary>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="updateDate">更新日時</param>
    ''' <param name="account">更新スタッフ</param>
    ''' <param name="updateFunction">更新ファンクション</param>
    ''' <returns>ActionResult</returns>
    ''' <remarks></remarks>
    Private Function UndoJobResult(ByVal jobDtlId As Decimal, _
                                   ByVal updateDate As Date, _
                                   ByVal account As String, _
                                   ByVal updateFunction As String) As Long

        Using ta As New TabletSMBCommonClassDataAdapter

            '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 START
            ''作業実績テーブルのデータを作業実績DELテーブルに移行する
            'Dim insertCount As Long = ta.InsertJobResultDel(jobDtlId, _
            '                                                updateDate, _
            '                                                account, _
            '                                                updateFunction)

            'If insertCount = 0 Then
            '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:InsertJobResultDel insertCount=0." _
            '                , MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.ExceptionError
            'End If

            ''作業実績テーブルから該データを削除する
            'Dim deleteCount As Long = ta.DeleteInsertJobResult(jobDtlId)
            'If deleteCount = 0 Then
            '    Logger.Error(String.Format(CultureInfo.InvariantCulture, "{0}.E Error:DeleteInsertJobResult deleteCount=0." _
            '                , MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.ExceptionError
            'End If

            'JobDispatch機能開発して、作業中チップに1個Jobも開始してないケースがあるので
            '削除、移行Countが0の可能性がある

            '作業実績テーブルのデータを作業実績DELテーブルに移行する
            ta.InsertJobResultDel(jobDtlId, _
                                  updateDate, _
                                  account, _
                                  updateFunction)

            '作業実績テーブルから該データを削除する
            ta.DeleteInsertJobResult(jobDtlId)
            '2014/07/15 TMEJ 張 タブレットSMB Job Dispatch機能開発 END

        End Using

        Return ActionResult.Success

    End Function

    ''' <summary>
    ''' 洗車中チップのUndo処理
    ''' </summary>
    ''' <param name="svcinId">サービス入庫ID</param>
    ''' <param name="jobDtlId">作業内容ID</param>
    ''' <param name="stallUseId">サービス入庫ID</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <param name="systemId">プログラムID</param>
    ''' <returns>正常終了：0、DMS除外エラーの警告：-9000、異常終了：エラーコード</returns>
    ''' <history>
    ''' 2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発
    ''' </history>
    <EnableCommit()>
    Public Function UndoWashingChip(ByVal svcinId As Decimal, _
                                    ByVal jobDtlId As Decimal, _
                                    ByVal stallUseId As Decimal, _
                                    ByVal rowLockVersion As Long, _
                                    ByVal systemId As String) As Long

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.S.  svcinId={1}, rowLockVersion={2}, systemId={3}, stallUseId={4}, jobDtlId={5}" _
                                , MethodBase.GetCurrentMethod.Name, svcinId, rowLockVersion, systemId, stallUseId, jobDtlId))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        '戻り値
        Dim returnCode As ActionResult = ActionResult.Success

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        Dim objStaffContext As StaffContext = StaffContext.Current
        Dim updateDate As Date = DateTimeFunc.Now(objStaffContext.DlrCD)

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        ''サービス入庫テーブルロック処理
        'Dim returnCode As Long = LockServiceInTable(svcinId, rowLockVersion, objStaffContext.Account, updateDate, systemId)
        'If returnCode <> 0 Then
        '    Logger.Error(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} TABLELOCK FAILURE " _
        '                , Me.GetType.ToString _
        '                , MethodBase.GetCurrentMethod.Name))
        '    Me.Rollback = True
        '    Return returnCode
        'End If

        'サービス入庫テーブルロック処理
        Dim returnCodeServiceinLock As Long = LockServiceInTable(svcinId, _
                                                                 rowLockVersion, _
                                                                 objStaffContext.Account, _
                                                                 updateDate, _
                                                                 systemId)

        '処理結果チェック
        If returnCodeServiceinLock <> 0 Then
            'テーブルロックに失敗した場合
            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} TABLELOCK FAILURE " _
                        , Me.GetType.ToString _
                        , MethodBase.GetCurrentMethod.Name))
            Me.Rollback = True
            Return returnCode

        End If

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        'ステータス送信ため、変更前のチップステータス、予約ステータスを取得する
        Dim preChipStatus As String = Me.JudgeChipStatus(stallUseId)

        Using ta As New TabletSMBCommonClassDataAdapter
            'TB_T_CARWASH_RESULTテーブルから対応のレコードを取得して、TB_T_CARWASH_RESULT_DELに保存するために
            Dim moveData As TabletSmbCommonClassCarWashRsultDataTable = _
                ta.GetCarWashResultBySvcinId(svcinId)

            If moveData.Count <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E ExceptionError:GetCarWashResultBySvcinId failed.Count={1}" _
                                        , MethodBase.GetCurrentMethod.Name, moveData.Count))
                Me.Rollback = True
                Return ActionResult.ExceptionError

            End If

            'サービスステータス変更：洗車中→洗車待ち
            Dim updateCount As Long = ta.UpdateServiceinWashCar(svcinId, _
                                                                SvcStatusCarWashStart, _
                                                                SvcStatusCarWashWait, _
                                                                objStaffContext.Account, _
                                                                updateDate)

            If updateCount <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E ExceptionError:UpdateServiceinWashCar failed." _
                                        , MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return ActionResult.ExceptionError

            End If

            '洗車実績テーブルに関連データを削除する
            Dim deleteCount As Long = ta.DeleteCarWashResultByServiceId(svcinId)

            If deleteCount <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E ExceptionError:DeleteCarWashResultByServiceId failed." _
                                        , MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return ActionResult.ExceptionError

            End If

            'TB_T_CARWASH_RESULTテーブルに削除されたデータをTB_T_CARWASH_RESULT_DELに保存する
            Dim insertCount As Long = ta.InsertCarWashResultDel(CType(moveData.Rows(0), TabletSmbCommonClassCarWashRsultRow))

            If insertCount <> 1 Then
                Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E ExceptionError:DeleteCarWashResultByServiceId failed." _
                                        , MethodBase.GetCurrentMethod.Name))
                Me.Rollback = True
                Return ActionResult.ExceptionError

            End If

        End Using

        Dim crntStatus As String = Me.JudgeChipStatus(stallUseId)
        'ステータス送信
        Using ic3802601blc As New IC3802601BusinessLogic
            Dim dmsSendResult As Long = ic3802601blc.SendStatusInfo(svcinId, _
                                                                    jobDtlId, _
                                                                    stallUseId, _
                                                                    preChipStatus, _
                                                                    crntStatus, _
                                                                    0)

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

            'If dmsSendResult <> 0 Then
            '    Logger.Error(String.Format(CultureInfo.CurrentCulture, "{0}.{1} SendStatusInfo FAILURE " _
            '                , Me.GetType.ToString, MethodBase.GetCurrentMethod.Name))
            '    Return ActionResult.DmsLinkageError
            'End If

            '処理結果チェック
            If dmsSendResult = ActionResult.Success Then
                '「0：成功」の場合
                '処理なし

            ElseIf dmsSendResult = ActionResult.WarningOmitDmsError Then
                '「-9000：DMS除外エラーの警告」の場合
                '戻り値に「-9000：DMS除外エラーの警告」を設定
                returnCode = ActionResult.WarningOmitDmsError

            Else
                '上記以外の場合
                '「15：他システムとの連携エラー」を返却
                Me.Rollback = True
                Logger.Warn(String.Format(CultureInfo.CurrentCulture _
                    , "{0}.{1} END RETURNCODE={2}[SendStatusInfo FAILURE]" _
                    , Me.GetType.ToString _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
                    , ActionResult.DmsLinkageError))
                Return ActionResult.DmsLinkageError

            End If

            '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

        End Using

        ' 正常終了
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}.E", MethodBase.GetCurrentMethod.Name))

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

        'Return ActionResult.Success

        Return returnCode

        '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    End Function

    ''' <summary>
    ''' Decimal()をStringに変更
    ''' </summary>
    ''' <param name="dataArray">変更したいString()</param>
    ''' <returns>変更されたString</returns>
    ''' <remarks></remarks>
    Private Function ConvertDecimalArrayToString(ByVal dataArray As List(Of Decimal)) As String

        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S" _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

        If IsNothing(dataArray) OrElse dataArray.Count = 0 Then
            Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. Array is empty." _
                , System.Reflection.MethodBase.GetCurrentMethod.Name))
            Return ""
        End If

        Dim sbReturn As New StringBuilder
        With sbReturn
            For Each stringData As String In dataArray
                .Append(stringData)
                .Append(",")
            Next
        End With
        '最後の,を削除する
        Dim strReturn As String = sbReturn.ToString()
        strReturn = strReturn.Substring(0, strReturn.Length - 1)
        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. retrun={1}" _
                    , System.Reflection.MethodBase.GetCurrentMethod.Name, strReturn))
        Return strReturn
    End Function

    '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 START

    ' ''' <summary>
    ' ''' Long()をStringに変更
    ' ''' </summary>
    ' ''' <param name="dataArray">変更したいString()</param>
    ' ''' <returns>変更されたString</returns>
    ' ''' <remarks></remarks>
    'Private Function ConvertLongArrayToString(ByVal dataArray As List(Of Long)) As String

    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_S" _
    '                    , System.Reflection.MethodBase.GetCurrentMethod.Name))

    '    If IsNothing(dataArray) OrElse dataArray.Count = 0 Then
    '        Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. Array is empty." _
    '            , System.Reflection.MethodBase.GetCurrentMethod.Name))
    '        Return ""
    '    End If

    '    Dim sbReturn As New StringBuilder
    '    With sbReturn
    '        For Each stringData As String In dataArray
    '            .Append(stringData)
    '            .Append(",")
    '        Next
    '    End With
    '    '最後の,を削除する
    '    Dim strReturn As String = sbReturn.ToString()
    '    strReturn = strReturn.Substring(0, strReturn.Length - 1)
    '    Logger.Info(String.Format(CultureInfo.InvariantCulture, "{0}_E. retrun={1}" _
    '                , System.Reflection.MethodBase.GetCurrentMethod.Name, strReturn))
    '    Return strReturn
    'End Function

    '2015/04/27 TMEJ 小澤 DMS連携版サービスタブレット強制納車機能追加開発 END

    ''' <summary>
    ''' 作業中チップの履歴を取得する(前回のみ)
    ''' </summary>
    ''' <param name="stallUseIdList">サービス入庫ID</param>
    ''' <returns>チップ履歴情報テーブル</returns>
    ''' <remarks></remarks>
    Public Function GetWorkingChipHis(ByVal stallUseIdList As List(Of Decimal)) As TabletSmbCommonClassChipHisDataTable
        Using ta As New TabletSMBCommonClassDataAdapter
            Return ta.GetWorkingChipHis(Me.ConvertDecimalArrayToString(stallUseIdList))
        End Using
    End Function

    '2016/04/20 NSK 小牟禮 工程管理の初期表示処理性能改善対応 START
    ''' <summary>
    ''' 作業中チップの履歴を取得する(前回のみ) ※営業時間より取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="stallStartTime">稼働時間From</param>
    ''' <param name="stallEndTime">稼働時間To</param>
    ''' <returns>チップ履歴情報テーブル</returns>
    ''' <remarks></remarks>
    Public Function GetWorkingChipHisFromStallTime(ByVal dealerCode As String, _
                                          ByVal branchCode As String, _
                                          ByVal stallStartTime As Date, _
                                          ByVal stallEndTime As Date _
                                          ) As TabletSmbCommonClassChipHisDataTable
        Using ta As New TabletSMBCommonClassDataAdapter
            Return ta.GetWorkingChipHisFromStallTime(dealerCode, branchCode, stallStartTime, stallEndTime)
        End Using
    End Function
    '2016/04/20 NSK 小牟禮 工程管理の初期表示処理性能改善対応 END

#End Region

    '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

#Region "WebService用XMLを構築"
    ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START

    ''' <summary>
    ''' WebService用XMLを構築
    ''' </summary>
    ''' <param name="reserveId">予約ID</param>
    ''' <param name="pReserveId">管理予約ID(リレーションコピー用)</param>
    ''' <param name="stallId">ストールID</param>
    ''' <param name="startDateTime">使用開始日時</param>
    ''' <param name="serviceWorkEndDateTime">使用終了日時(休憩エリアと計算した終了日時)</param>
    ''' <param name="serviceWorkTime">予定作業時間</param>
    ''' <param name="objStaffContext">スタフ情報</param>
    ''' <param name="updateDate">現在日時</param>
    ''' <param name="restFlg">ストール休憩フラグ</param>
    ''' <param name="scheDeliDatetime">納車希望日時</param>
    ''' <param name="mainteCode">整備コード</param>
    ''' <param name="inspectionNeedFlg">検査フラグ</param>
    ''' <param name="workOrderFlg">着工指示フラグ</param>
    ''' <param name="noShowFlg">未来店客フラグ</param>
    ''' <param name="cancelFlg">キャンセルフラグ</param>
    ''' <param name="reserveReception">受付納車区分</param>
    ''' <param name="scheSvcinDatetime">予定入庫日時</param>
    ''' <param name="rowLockVersion">行ロックバージョン</param>
    ''' <param name="svcCd">サービスコード</param>
    ''' <param name="carwashNeedFlg">洗車必要フラグ</param>
    ''' <returns>構築したXMLドキュメント</returns>
    ''' <remarks></remarks>
    ''' <history>
    ''' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証
    ''' </history>
    Private Function StructWebServiceXml(ByVal reserveId As String, _
                                         ByVal pReserveId As String, _
                                         ByVal stallId As String, _
                                         ByVal startDateTime As String, _
                                         ByVal serviceWorkEndDateTime As String, _
                                         ByVal serviceWorkTime As String, _
                                         ByVal objStaffContext As StaffContext, _
                                         ByVal updateDate As Date, _
                                         ByVal restFlg As String, _
                                         ByVal scheDeliDatetime As String, _
                                         ByVal mainteCode As String, _
                                         ByVal inspectionNeedFlg As String, _
                                         ByVal workOrderFlg As String, _
                                         ByVal noShowFlg As String, _
                                         ByVal cancelFlg As String, _
                                         ByVal reserveReception As String, _
                                         ByVal scheSvcinDatetime As String, _
                                         ByVal rowLockVersion As String, _
                                         Optional ByVal svcCd As String = Nothing, _
                                         Optional ByVal carwashNeedFlg As String = Nothing) As SMBCommonClassBusinessLogic.XmlDocumentClass
        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

        Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                                  "{0}.S ", _
                                  MethodBase.GetCurrentMethod.Name))

        'WebServiceを呼ぶためXML作成
        Dim xmlclass As New SMBCommonClassBusinessLogic.XmlDocumentClass

        'headタグの構築
        '送信日付
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 START
        'xmlclass.Head.TransmissionDate = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", updateDate)
        Using srvCommonBiz As New ServiceCommonClassBusinessLogic

            'DATE_FORMATを取得
            Dim dateFormat As String = srvCommonBiz.GetSystemSettingValueBySettingName(SysDateFormat)

            If String.IsNullOrWhiteSpace(dateFormat) Then
                '取得値なし
                xmlclass.Head.TransmissionDate = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", updateDate)
            Else
                '取得値あり
                xmlclass.Head.TransmissionDate = updateDate.ToString(dateFormat, CultureInfo.CurrentCulture)
            End If

        End Using
        '2013/12/02 TMEJ 張 タブレット版SMB チーフテクニシャン機能開発 END

        'Commonタグの構築
        '販売店コード
        xmlclass.Detail.Common.DealerCode = objStaffContext.DlrCD
        '店舗コード
        xmlclass.Detail.Common.BranchCode = objStaffContext.BrnCD
        'スタッフコード
        xmlclass.Detail.Common.StaffCode = objStaffContext.Account

        'Reserve_Customerタグの構築
        '氏名
        xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.CustomerName = NoChangeItem
        '電話番号
        xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.TelNo = NoChangeItem
        '携帯番号
        xmlclass.Detail.ReserveInformation.ReserveCustomerInformation.Mobile = NoChangeItem

        'Reserve_VehicleInformationタグの構築
        '登録ナンバー
        xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.VehicleNo = NoChangeItem
        'VIN
        xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.Vin = NoChangeItem
        '車名
        xmlclass.Detail.ReserveInformation.ReserveVehicleInformation.SeriesName = NoChangeItem


        'Detail_ReserveInformation_ReserveServiceInformationタグの構築
        'ストールID
        If Not String.IsNullOrWhiteSpace(stallId) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.StallId = stallId
        End If
        '作業開始予定日時
        If Not String.IsNullOrWhiteSpace(startDateTime) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.StartTime = startDateTime
        End If
        '作業終了予定日時
        If Not String.IsNullOrWhiteSpace(serviceWorkEndDateTime) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.EndTime = serviceWorkEndDateTime
        End If
        '予定作業時間
        If Not String.IsNullOrWhiteSpace(serviceWorkTime) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.WorkTime = serviceWorkTime
        End If
        'ストール休憩フラグ
        If Not String.IsNullOrWhiteSpace(restFlg) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.BreakFlg = restFlg
        End If
        '検査フラグ
        If Not String.IsNullOrWhiteSpace(inspectionNeedFlg) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.InspectionFlg = inspectionNeedFlg
        End If
        '受付納車区分
        If Not String.IsNullOrWhiteSpace(reserveReception) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReserveReception = reserveReception
            '受付納車区分のコードが（1、2、3、4）の場合は「引取希望日時」「納車希望日時」指定が必須。（0）の場合はOptional
            If Not reserveReception.Equals(DeliTypeWaiting) Then
                If Not String.IsNullOrWhiteSpace(scheSvcinDatetime) Then
                    xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReservePickDate = scheSvcinDatetime
                Else
                    xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReservePickDate = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", DefaultDateTimeValueGet())
                End If

                If Not String.IsNullOrWhiteSpace(scheSvcinDatetime) Then
                    xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReserveDeliDate = scheDeliDatetime
                Else
                    xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ReserveDeliDate = String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd HH:mm:ss}", DefaultDateTimeValueGet())
                End If
            End If
        End If
        '整備コード
        If Not String.IsNullOrWhiteSpace(mainteCode) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.MntnCode = mainteCode
        End If

        'Detail_ReserveInformationタグの構築
        'シーケンスナンバー　「0」固定で設定する
        xmlclass.Detail.ReserveInformation.SeqNo = DefaultNumberValue.ToString(CultureInfo.InvariantCulture)
        '予約ID
        If Not String.IsNullOrWhiteSpace(reserveId) Then
            xmlclass.Detail.ReserveInformation.ReserveId = reserveId
        End If
        '管理予約ID
        If Not String.IsNullOrWhiteSpace(pReserveId) AndAlso Not pReserveId.Equals(CType(DefaultNumberValue, String)) Then
            xmlclass.Detail.ReserveInformation.PReserveId = pReserveId
        End If
        'キャンセルフラグ
        If Not String.IsNullOrWhiteSpace(cancelFlg) Then
            xmlclass.Detail.ReserveInformation.CancelFlg = cancelFlg
        End If
        '未来店客フラグ
        If Not String.IsNullOrWhiteSpace(noShowFlg) Then
            xmlclass.Detail.ReserveInformation.NoShowFlg = noShowFlg
        End If
        '着工指示フラグ
        If Not String.IsNullOrWhiteSpace(workOrderFlg) Then
            xmlclass.Detail.ReserveInformation.WorkOrderFlg = workOrderFlg
        End If
        '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 START
        ''RO作業連番
        'If Not String.IsNullOrWhiteSpace(workSeq) Then
        '    xmlclass.Detail.ReserveInformation.OerderJobSeq = workSeq
        'End If
        '2014/01/13 TMEJ 下村 タブレット版SMB チーフテクニシャン機能開発 END
        'ROWロックバージョン
        If Not String.IsNullOrWhiteSpace(rowLockVersion) Then
            xmlclass.Detail.ReserveInformation.RowLockVersion = rowLockVersion
        End If
        '受付担当予定者
        xmlclass.Detail.ReserveInformation.AcountPlan = Nothing
        '更新オペレータ
        xmlclass.Detail.ReserveInformation.UpdateAccount = objStaffContext.Account

        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 START
        ' サービスコード
        If Not String.IsNullOrWhiteSpace(svcCd) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.ServiceCode = svcCd
        End If
        ' 洗車必要フラグ
        If Not String.IsNullOrWhiteSpace(carwashNeedFlg) Then
            xmlclass.Detail.ReserveInformation.ReserveServiceInformation.WashFlg = carwashNeedFlg
        End If
        ' 2020/01/08 NSK 鈴木 TR-V4-BMS-20191001-001 (トライ店システム評価)コールセンター業務における洗車要否判断の効率化検証 END

        Return xmlclass

    End Function

    ''' <summary>
    ''' ウェブサービス用の休憩フラグを取得する
    ''' </summary>
    ''' <param name="restFlg">画面の休憩フラグ値</param>
    ''' <returns>ウェブサービス用の休憩フラグ値</returns>
    ''' <remarks></remarks>
    Private Function GetWebServiceRestFlg(ByVal restFlg As String) As String
        '休憩を取得場合、0を戻す
        If RestTimeGetFlgGetRest.Equals(restFlg) Then
            Return "0"
        Else
            Return "1"
        End If
    End Function
#End Region

End Class
