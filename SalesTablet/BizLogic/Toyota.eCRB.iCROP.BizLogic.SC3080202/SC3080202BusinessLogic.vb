'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080202BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客詳細(商談情報)
'補足： 
'作成： 2011/11/24 TCS 小野
'更新： 2012/01/26 TCS 山口 【SALES_1B】
'更新： 2013/03/06 TCS 河原 GL0874 
'更新： 2013/06/30 TCS 徐 【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2013/12/05 TCS 市川 Aカード情報相互連携開発
'更新： 2014/02/12 TCS 山口 受注後フォロー機能開発
'更新： 2014/03/11 TCS 松月 【A STEP2】活動予定スタッフ取得不具合対応（問連TR-V4-GTMC140305008）
'更新： 2014/09/04 TCS 武田 UAT不具合対応(最終活動表示)
'更新： 2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57
'更新： 2015/12/08 TCS 中村 (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発
'更新： 2016/09/12 TCS 鈴木 性能改善（TR-SLT-TMT-20160726-002）
'更新： 2017/11/20 TCS 河原 TKM独自機能開発
'更新： 2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証
'更新： 2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1
'更新： 2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証
'更新： 2019/02/14 TCS 河原 TKM UAT0651対応(タブレットで契約した見積のIDは論削済みでも取得するように修正)
'更新： 2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証
'更新： 2020/01/06 TS  重松 [TMTレスポンススロー] SLT基盤への横展
'更新： 2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061)  
'─────────────────────────────────────

Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080202DataSet
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess.SC3080202TableAdapter
Imports System.Globalization
Imports Toyota.eCRB.SystemFrameworks
' 2012/02/29 TCS 小野 【SALES_2】 START
Imports Toyota.eCRB.CommonUtility.BizLogic
Imports Toyota.eCRB.CommonUtility.DataAccess
' 2012/02/29 TCS 小野 【SALES_2】 END

Public Class SC3080202BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3080202BusinessLogic

#Region "定数"
    ' CR活動カテゴリ
    ' Periodical
    Public Const CractcategoryPeriodical As String = "1"
    ' Repurchase
    Public Const CractcategoryRepurchase As String = "2"
    ' Birthday
    Public Const CractcategoryBirthday As String = "4"
    ' リクエストカテゴリ
    ' Walk-in
    Public Const RequestcategoryWalkin As String = "1"
    ' Call-in
    Public Const RequestcategoryCallin As String = "2"
    ' RMM
    Public Const RequestcategoryRmm As String = "3"
    ' Request
    Public Const RequestcategoryRequest As String = "4"
    ' CR活動結果
    ' Hot
    Public Const CractresultHot As String = "1"
    ' Prospect
    Public Const CractresultProspect As String = "2"
    ' Success
    Public Const CractresultSuccess As String = "3"
    ' Giveup
    Public Const CractresultGiveup As String = "5"
    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
    ' Cold
    Public Const CractresultCold As String = "4"
    '2013/06/30 TCS 黄 2013/10対応版　既存流用 End

    ' アクションコード(SEQ)
    ' カタログ
    Public Const ActionCatalog As Integer = 9
    ' 試乗
    Public Const ActionTestdrive As Integer = 16
    ' 査定
    Public Const ActionEvaluation As Integer = 18
    ' 見積
    Public Const ActionQuotation As Integer = 10

    ' アクションコード(SEQ)
    ' カタログ
    Public Const ActionCdCatalog As String = "A22"
    ' 試乗
    Public Const ActionCdTestdrive As String = "A26"
    ' 査定
    Public Const ActionCdEvaluation As String = "A30"
    ' 見積
    Public Const ActionCdQuotation As String = "A23"

    ' エラーメッセージID
    Public Const ErrMsgid20902 As Integer = 20902
    ' 画面ID
    Public Const DisplayId As String = "SC3080202"
    ' 希望車種フラグ
    ' 追加モード
    Public Const EditFlgAdd As String = "0"
    ' 編集モード
    Public Const EditFlgEdit As String = "1"
    ' 編集
    Public Const CheckFlgEdit As String = "0"
    ' 削除
    Public Const CheckFlgDelete As String = "1"

    ' statusのアイコンパス
    Public Const StatuspicHot As String = "../Styles/images/SC3080202/Hot.png"
    Public Const StatuspicWarm As String = "../Styles/images/SC3080202/Prospect.png"
    Public Const StatuspicCold As String = "../Styles/images/SC3080202/Walk-in.png"
    Public Const StatuspicGiveup As String = "../Styles/images/SC3080202/Giveup.png"
    Public Const StatuspicSuccess As String = "../Styles/images/SC3080202/Success.png"

    '2013/06/30 TCS 内藤 2013/10対応版 既存流用 START
    Private Const BOOKEDAFTERCONTACT As String = "社内作業"
    Private Const BOOKEDAFTERCONTACTCD As Integer = 99
    '2013/06/30 TCS 内藤 2013/10対応版 既存流用 END

    '2013/12/03 TCS 市川 Aカード情報相互連携開発 START
    'システム環境設定・販売店環境設置のKEY
    Private Const ENVSETTINGKEY_GIVEUP_CONTACT_MTD As String = "TABLET_CONTACT_MTD" '用件ソース1st(タブレットセールス)抽出時のコンタクト方法
    Private Const ENVSETTINGKEY_MOST_PREFERRED_PROSPECT_CD As String = "MOST_PREFERRED_PROSPECT_CD" '希望者の商談見込み度コード
    '入力必須チェック用定数
    Public Const INPUT_CHECK_TIMING As String = "02"
    '2013/12/03 TCS 市川 Aカード情報相互連携開発 END

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    'システム環境設定.受注後活動コード
    Private Const ENVSETTINGKEY_AFTER_ODR_VCM_MATCHING As String = "AFTER_ODR_VCM_MATCHING" '車両ステイタス
    Private Const ENVSETTINGKEY_AFTER_ODR_ARRIVAL As String = "AFTER_ODR_ARRIVAL" '到着日
    Private Const ENVSETTINGKEY_AFTER_ODR_FINANCE As String = "AFTER_ODR_FINANCE" 'ファイナンスステイタス
    Private Const ENVSETTINGKEY_AFTER_ODR_FINANCE_APPLICATION As String = "AFTER_ODR_FINANCE_APPLICATION" 'ファイナンス申請日
    Private Const ENVSETTINGKEY_AFTER_ODR_FINANCE_REAPPLICATION As String = "AFTER_ODR_FINANCE_REAPPLICATION" 'ファイナンス再申請日
    Private Const ENVSETTINGKEY_AFTER_ODR_FINANCE_APPROVAL As String = "AFTER_ODR_FINANCE_APPROVAL" 'ファイナンス承認日
    Private Const ENVSETTINGKEY_AFTER_ODR_MATCHING As String = "AFTER_ODR_MATCHING" 'マッチングステイタス
    Private Const ENVSETTINGKEY_AFTER_ODR_ASSIGN As String = "AFTER_ODR_ASSIGN" '振当て日
    Private Const ENVSETTINGKEY_AFTER_ODR_VDQI1 As String = "AFTER_ODR_VDQI1" 'VDQIステイタス1
    Private Const ENVSETTINGKEY_AFTER_ODR_VDQI2 As String = "AFTER_ODR_VDQI2" 'VDQIステイタス2
    Private Const ENVSETTINGKEY_AFTER_ODR_VDQI_REQUEST As String = "AFTER_ODR_VDQI_REQUEST" 'VDQIリクエスト日
    Private Const ENVSETTINGKEY_AFTER_ODR_VDQI_START As String = "AFTER_ODR_VDQI_START" 'VDQI開始日
    Private Const ENVSETTINGKEY_AFTER_ODR_VDQI_COMPLETE As String = "AFTER_ODR_VDQI_COMPLETE" 'VDQI完了日
    Private Const ENVSETTINGKEY_AFTER_ODR_PDS_IMPLEMENT As String = "AFTER_ODR_PDS_IMPLEMENT" 'PDS実施日
    Private Const ENVSETTINGKEY_AFTER_ODR_INSURANCE_REGISTRATION As String = "AFTER_ODR_INSURANCE_REGISTRATION" '保険登録日
    Private Const ENVSETTINGKEY_AFTER_ODR_DELIVERY As String = "AFTER_ODR_DELIVERY" '納車日時
    Private Const ENVSETTINGKEY_AFTER_ODR_REGISTRATION1 As String = "AFTER_ODR_REGISTRATION1" '登録ステイタス1
    Private Const ENVSETTINGKEY_AFTER_ODR_REGISTRATION2 As String = "AFTER_ODR_REGISTRATION2" '登録ステイタス2
    Private Const ENVSETTINGKEY_AFTER_ODR_REGISTRATION3 As String = "AFTER_ODR_REGISTRATION3" '登録ステイタス3
    Private Const ENVSETTINGKEY_AFTER_ODR_REGISTRATION_APPLICATION As String = "AFTER_ODR_REGISTRATION_APPLICATION" '登録申請日
    Private Const ENVSETTINGKEY_AFTER_ODR_NUMBER_ACQUIRE As String = "AFTER_ODR_NUMBER_ACQUIRE" 'ナンバー取得日
    Private Const ENVSETTINGKEY_AFTER_ODR_NUMBER_HANDING As String = "AFTER_ODR_NUMBER_HANDING" 'ナンバー引き渡し日
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
    ''' <summary>
    ''' システム設定の指定パラメータ 受注後工程利用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const C_USE_AFTER_ODR_PROC_FLG As String = "USE_AFTER_ODR_PROC_FLG"
    '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

    '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
#Region "（トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証"
    Private Const SETTING_NAME_USE_FLG_SUFFIX As String = "USE_FLG_SUFFIX"
    Private Const SETTING_NAME_USE_FLG_INTERIOR As String = "USE_FLG_INTERIORCLR"
#End Region
    '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

    '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
    Private Const SETTING_NAME_ACT_STATUS_DISP_FLG As String = "ACT_STATUS_DISP_FLG" '活動ステータス表示フラグ
    Private Const ACT_STATUS_DISP_FLG_ON As String = "1"
    '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END

#Region "TKMローカル"
    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
    Private Const LC_WORDNO_REPLACE_TXT_ITEMTITLE As Decimal = 2020001 '商談条件項目の置換文字列（画面表示用）
    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END
#End Region


#End Region

#Region "処理"

    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
    ' ''' <summary>
    ' ''' FollowupboxSeqno取得処理
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Shared Function GetFllwupboxSeqno() As SC3080202DataSet.SC3080202GetSeqnoToDataTable
    '    Logger.Info("GetFllwupboxSeqno Start")

    '    Dim fllwupboxseqno As Long
    '    Dim datatableFllwupboxSeqno As SC3080202DataSet.SC3080202GetFllwupboxNoDataTable
    '    Dim datarowFllwupboxSeqno As SC3080202DataSet.SC3080202GetFllwupboxNoRow
    '    ' SQL発行
    '    datatableFllwupboxSeqno = SC3080202TableAdapter.GetFllwupboxSeqno()
    '    datarowFllwupboxSeqno = CType(datatableFllwupboxSeqno.Rows(0), SC3080202DataSet.SC3080202GetFllwupboxNoRow)
    '    fllwupboxseqno = datarowFllwupboxSeqno.SEQ

    '    ' 采番されていれば、返却
    '    Using datatableTo As New SC3080202DataSet.SC3080202GetSeqnoToDataTable
    '        If datatableFllwupboxSeqno.Rows.Count > 0 Then
    '            Dim datarowTo As SC3080202DataSet.SC3080202GetSeqnoToRow
    '            datarowTo = datatableTo.NewSC3080202GetSeqnoToRow
    '            datarowTo.FLLWUPBOX_SEQNO = CLng(fllwupboxseqno)
    '            datatableTo.Rows.Add(datarowTo)
    '        End If
    '        Return datatableTo
    '    End Using

    '    Logger.Info("GetFllwupboxSeqno End")
    'End Function
    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

    ''' <summary>
    ''' 見積ID取得処理
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetEstimatedId(ByVal datatableFrom As SC3080202DataSet.SC3080202GetEstimateidFromDataTable) As SC3080202DataSet.SC3080202GetEstimateidToDataTable
        Logger.Info("GetEstimatedId Start")

        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim fllwupbox_seqno As Decimal               'Followupbox seqno
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim datatableEstimatedId As SC3080202DataSet.SC3080202GetEstimateidToDataTable
        dlrcd = datatableFrom(0).DLRCD
        strcd = datatableFrom(0).STRCD
        fllwupbox_seqno = datatableFrom(0).FLLWUPBOX_SEQNO
        ' SQL発行
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        '2019/02/14 TCS 河原 TKM UAT0651対応(タブレットで契約した見積のIDは論削済みでも取得するように修正) START
        datatableEstimatedId = SC3080202TableAdapter.GetEstimateId(CType(fllwupbox_seqno, String))
        '2019/02/14 TCS 河原 TKM UAT0651対応(タブレットで契約した見積のIDは論削済みでも取得するように修正) END
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        ' 返却
        Return datatableEstimatedId

        Logger.Info("GetEstimatedId End")
    End Function

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    ''' <summary>
    ''' プロセスアイコン取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetProcessIcons() As SC3080202DataSet.SC3080202GetFllwupboxContentDataTable
        Return SC3080202TableAdapter.GetProcessIcons()
    End Function
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    ''' <summary>
    ''' 活動状態取得
    ''' </summary>
    ''' <param name="datatableFrom"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetFollowupboxStatus(ByVal datatableFrom As SC3080202DataSet.SC3080202GetStatusFromDataTable) As SC3080202DataSet.SC3080202GetStatusToDataTable
        Logger.Info("GetFollowupboxStatus Start")

        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim fllwupbox_seqno As Decimal               'Followupbox seqno
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim datarowFrom As SC3080202DataSet.SC3080202GetStatusFromRow
        Dim result As SC3080202DataSet.SC3080202GetStatusToDataTable

        datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetStatusFromRow)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        ' SQL発行
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        result = SC3080202TableAdapter.GetFollowupboxStatus(CType(fllwupbox_seqno, String))
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        '編集
        For Each dr As SC3080202DataSet.SC3080202GetStatusToRow In result
            ' 2012/02/29 TCS 小野 【SALES_2】 START
            ' 商談途中のものは、活性
            If dr.IsCRACTRESULTNull Then
                dr.ENABLEFLG = True
            Else
                ' 2012/02/29 TCS 小野 【SALES_2】 END
                Select Case dr.CRACTRESULT
                    ' 2012/02/29 TCS 小野 【SALES_2】 START
                    'Case CractresultSuccess, CractresultGiveup
                    '    dr.ENABLEFLG = False
                    Case CractresultSuccess
                        ' 成約は、受注後かどうか判定
                        If dr.IsCONTRACTNONull Then
                            ' 契約書実行以外で成約した場合（受注後工程フォローなし）
                            dr.ENABLEFLG = False

                            ' 2013/12/05 TCS 市川 Aカード情報相互連携開発 START
                            'TMT過渡期対応
                        ElseIf Not ActivityInfoBusinessLogic.CheckUsedB2D() Then
                            '成約の活動結果が登録済みの場合、受注後工程を無効化する。
                            Using dt As New ActivityInfoDataSet.ActivityInfoCountFromDataTable()
                                dt.AddActivityInfoCountFromRow(dlrcd, strcd, fllwupbox_seqno)
                                If ActivityInfoBusinessLogic.CountFllwupboxRslt(dt).Equals(ActivityInfoBusinessLogic.SALESAFTER_YES) Then dr.ENABLEFLG = False
                            End Using
                            ' 2013/12/05 TCS 市川 Aカード情報相互連携開発 END

                            ' 2012/03/20 TCS 安田 【SALES_2】 START
                        ElseIf Not dr.IsCANCELFLGNull AndAlso dr.CANCELFLG.Equals("1") Then
                            '注文キャンセル時
                            dr.ENABLEFLG = False
                            ' 2012/03/20 TCS 安田 【SALES_2】 END
                        Else
                            ' 受注後工程フォロー中

                            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
                            If "0".Equals(SC3080202BusinessLogic.GetAfterOdrProcFlg(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD)) Then
                                '受注後工程を利用しない店舗の場合
                                dr.ENABLEFLG = False
                            ElseIf dr.BOOCKEDAFTER_COMPLETEFLG.Equals("1") Then
                                '必須活動が全て完了している場合
                                dr.ENABLEFLG = False
                            Else
                                '必須活動が全て完了していない場合
                                dr.ENABLEFLG = True
                            End If
                            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

                        End If
                    Case CractresultGiveup
                        dr.ENABLEFLG = False
                        ' 2012/02/29 TCS 小野 【SALES_2】 END
                        '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
                    Case CractresultHot, CractresultProspect, CractresultCold
                        '2013/06/30 TCS 黄 2013/10対応版　既存流用 End
                        dr.ENABLEFLG = True
                    Case Else
                        If dr.IsCRACTRESULTNull Then
                            If dr.REQCATEGORY = RequestcategoryWalkin Then
                                dr.ENABLEFLG = True
                            Else
                                dr.ENABLEFLG = False
                            End If
                        Else
                            '一度も活動結果登録していないかつ注文承認済み(受注時に商談終了)
                            dr.ENABLEFLG = True
                        End If
                End Select
                ' 2012/02/29 TCS 小野 【SALES_2】 START
            End If
            ' 2012/02/29 TCS 小野 【SALES_2】 END
        Next

        Return result

        Logger.Info("GetFollowupboxStatus End")
    End Function
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
    ''' <summary>
    ''' 活動リスト取得
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetActivityList(ByVal datatableFrom As SC3080202DataSet.SC3080202GetActivityListFromDataTable) As SC3080202DataSet.SC3080202GetActivityListToDataTable
        Logger.Info("GetActivityList Start")

        ' 変数
        Dim insdid As String                        '自社客ID／未取引客ID
        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        Dim custid As String                        '自社客／未取引客種別
        ' 2012/02/29 TCS 小野 【SALES_2】 START
        Dim newcustid As String                     '未取引客ID
        ' 2012/02/29 TCS 小野 【SALES_2】 END
        Dim datatableActivityList As SC3080202DataSet.SC3080202GetFollowupboxListDataTable
        Dim datarowFrom As SC3080202DataSet.SC3080202GetActivityListFromRow

        ' 引数取得
        datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetActivityListFromRow)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        insdid = datarowFrom.INSDID
        custid = datarowFrom.CUSTFLG
        ' 2012/02/29 TCS 小野 【SALES_2】 START
        If Not datarowFrom.IsNEWCUSTIDNull Then
            newcustid = datarowFrom.NEWCUSTID
        Else
            newcustid = String.Empty
        End If
        ' 2012/02/29 TCS 小野 【SALES_2】 END

        ' SQL発行
        ' 2012/02/29 TCS 小野 【SALES_2】 START
        'datatableActivityList = SC3080202TableAdapter.GetFollowupboxList(dlrcd, strcd, insdid, custid)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 START
        'datatableActivityList = SC3080202TableAdapter.GetFollowupboxList(dlrcd, strcd, insdid)
        datatableActivityList = SC3080202TableAdapter.GetFollowupboxList(dlrcd, insdid)
        '2015/01/08 TCS 外崎 TMT2販社 ST-BTS 57 END
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        ' 2012/02/29 TCS 小野 【SALES_2】 END

        ' データ編集
        Using datatableTo As New SC3080202DataSet.SC3080202GetActivityListToDataTable
            Dim datarowTo As SC3080202DataSet.SC3080202GetActivityListToRow
            '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発
            Dim scDlrCd As String = StaffContext.Current.DlrCD
            Dim account As String = StaffContext.Current.Account
            For Each ActivityRow As SC3080202DataSet.SC3080202GetFollowupboxListRow In datatableActivityList
                datarowTo = datatableTo.NewSC3080202GetActivityListToRow
                ' DLRCD
                datarowTo.DLRCD = ActivityRow.DLRCD
                ' STRCD
                datarowTo.STRCD = ActivityRow.STRCD
                ' Follow-upBox内連番
                datarowTo.FLLWUPBOX_SEQNO = ActivityRow.FLLWUPBOX_SEQNO
                ' 活動名
                ' 2012/02/29 TCS 小野 【SALES_2】 START
                If ActivityRow.IsCRACTCATEGORYNull Then
                    datarowTo.CRACTNAME = WebWordUtility.GetWord(20038)
                Else
                    ' 2012/02/29 TCS 小野 【SALES_2】 END
                    datarowTo.CRACTNAME = ""
                    If ActivityRow.CRACTCATEGORY = CractcategoryPeriodical Or _
                        ActivityRow.CRACTCATEGORY = CractcategoryBirthday Then
                        ' Periodicalの場合
                        ' サービス名か中項目名
                        If ActivityRow.IsSERVICENAMENull Then
                            If ActivityRow.IsSUBCTGORGNAMENull Then
                                datarowTo.CRACTNAME = WebWordUtility.GetWord(20037)
                            Else
                                datarowTo.CRACTNAME = ActivityRow.SUBCTGORGNAME
                            End If
                        Else
                            datarowTo.CRACTNAME = ActivityRow.SERVICENAME
                        End If
                    ElseIf ActivityRow.CRACTCATEGORY = CractcategoryRepurchase Then
                        ' Repurchaseの場合
                        ' サービス名
                        If ActivityRow.IsSERVICENAMENull Then
                            datarowTo.CRACTNAME = WebWordUtility.GetWord(20037)
                        Else
                            datarowTo.CRACTNAME = ActivityRow.SERVICENAME
                        End If
                    End If

                    If ActivityRow.IsPROMOTION_IDNull = False Then
                        ' Promotionの場合
                        ' Promotion名
                        datarowTo.CRACTNAME = ActivityRow.PROMOTIONNAME
                    End If
                    '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
                    If ActivityRow.REQCATEGORY = RequestcategoryWalkin Then
                        ' Walk-in Follow-upの場合
                        ' 画面文言(Walk-in Follow-up)
                        datarowTo.CRACTNAME = WebWordUtility.GetWord(20028)
                    ElseIf datarowTo.CRACTNAME = "" Then
                        ' Request Follow-upの場合
                        ' 画面文言(Request Follow-up)

                        datarowTo.CRACTNAME = WebWordUtility.GetWord(20027)
                    End If
                    '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
                    ' 2012/02/29 TCS 小野 【SALES_2】 START
                End If
                ' 2012/02/29 TCS 小野 【SALES_2】 END

                ' 活動ステータス
                ' 2012/02/29 TCS 小野 【SALES_2】 START
                If ActivityRow.IsCRACTRESULTNull Then
                    ' 新規商談一時保存
                    If ActivityRow.IsCONTRACTNONull Then
                        datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20037)
                    Else
                        ' 受注後
                        If ActivityInfoBusinessLogic.IsExistsUnexecutedAfterOdrAct(datarowTo.FLLWUPBOX_SEQNO) Then
                            '未完了の必須受注後工程活動有り
                            datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20078) '受注後工程
                        Else
                            '未完了の必須受注後工程活動無し
                            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
                            If SC3080202BusinessLogic.GetAfterOdrProcFlg(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD).Equals("1") Then
                                datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20079) 'コンプリート
                            Else
                                datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20025) 'Success
                            End If
                            '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END
                        End If
                    End If
                Else
                    ' 2012/02/29 TCS 小野 【SALES_2】 END
                    Select Case ActivityRow.CRACTRESULT
                        Case CractresultHot
                            ' 画面文言(Hot)
                            datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20024)
                        Case CractresultProspect
                            ' 画面文言(Warm)
                            datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20023)
                        Case CractresultSuccess
                            ' 画面文言(Success)
                            ' 2012/02/29 TCS 小野 【SALES_2】 START
                            'datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20025)
                            ' 新規商談一時保存
                            If ActivityRow.IsCONTRACTNONull Then
                                datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20025)
                            Else
                                ' 受注後
                                If ActivityInfoBusinessLogic.IsExistsUnexecutedAfterOdrAct(datarowTo.FLLWUPBOX_SEQNO) Then
                                    '未完了の必須受注後工程活動有り
                                    datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20078) '受注後工程
                                Else
                                    '未完了の必須受注後工程活動無し
                                    '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
                                    If SC3080202BusinessLogic.GetAfterOdrProcFlg(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD).Equals("1") Then
                                        datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20079) 'コンプリート
                                    Else
                                        datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20025) 'Success
                                    End If
                                    '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END
                                End If
                            End If
                            ' 2012/02/29 TCS 小野 【SALES_2】 END
                        Case CractresultGiveup
                            ' 画面文言(Giveup)
                            '契約状況フラグ取得
                            Dim dtEstimate As SC3080202DataSet.SC3080202GetEstimateidToDataTable
                            dtEstimate = SC3080202TableAdapter.GetEstimateInfo(CStr(datarowTo.FLLWUPBOX_SEQNO))
                            If dtEstimate.Rows.Count = 0 OrElse dtEstimate.Rows(0).IsNull(dtEstimate.CONTRACTFLGColumn.ColumnName) OrElse _
                                Not CStr(dtEstimate.Rows(0).Item(dtEstimate.CONTRACTFLGColumn.ColumnName)).Equals("2") Then
                                '0件 or 契約状況フラグ=NULL or 契約状況フラグ<>"2"
                                datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20026) 'Give-up
                            Else
                                '契約状況フラグ="2"
                                datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20080) '注文キャンセル
                            End If
                        Case Else
                            ' 画面文言(-)
                            datarowTo.CRACTSTATUS = WebWordUtility.GetWord(20022)
                    End Select
                    ' 2012/02/29 TCS 小野 【SALES_2】 START
                End If
                ' 2012/02/29 TCS 小野 【SALES_2】 END

                ' 最終活動日
                If ActivityRow.IsCRACTRESULT_UPDATEDATENull Then
                    datarowTo.CRACTDATE = Nothing
                    datarowTo.CRACTDATESTRING = String.Empty
                Else
                    datarowTo.CRACTDATE = ActivityRow.CRACTRESULT_UPDATEDATE
                    '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発
                    'datarowTo.CRACTDATESTRING = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, ActivityRow.CRACTRESULT_UPDATEDATE, StaffContext.Current.DlrCD)
                    datarowTo.CRACTDATESTRING = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, ActivityRow.CRACTRESULT_UPDATEDATE, scDlrCd)
                End If
                ' 活動予定アカウント
                datarowTo.ACCOUNT_PLAN = ActivityRow.ACCOUNT_PLAN

                '2014/03/11 TCS 松月 【A STEP2】活動予定スタッフ取得不具合対応（問連TR-V4-GTMC140305008）Start
                Dim retCnt As Integer
                retCnt = SC3080202TableAdapter.GetCountStaffPlan(datarowTo.FLLWUPBOX_SEQNO)
                If retCnt > 0 Then
                    datarowTo.ACCOUNT_PLAN = SC3080202TableAdapter.GetStaffPlan(datarowTo.FLLWUPBOX_SEQNO)
                End If
                '2014/03/11 TCS 松月 【A STEP2】活動予定スタッフ取得不具合対応（問連TR-V4-GTMC140305008）End

                ' 活性フラグ

                ' 2012/02/29 TCS 小野 【SALES_2】 START
                ' 商談途中のものは、活性
                If ActivityRow.IsCRACTRESULTNull Then
                    datarowTo.ENABLEFLG = True
                Else
                    ' 2012/02/29 TCS 小野 【SALES_2】 END
                    Select Case ActivityRow.CRACTRESULT
                        ' 2012/02/29 TCS 小野 【SALES_2】 START
                        'Case CractresultSuccess, CractresultGiveup
                        '    datarowTo.ENABLEFLG = False
                        Case CractresultSuccess
                            ' 成約は、受注後かどうか判定
                            If ActivityRow.IsCONTRACTNONull Then
                                ' 契約書実行以外で成約した場合（受注後工程フォローなし）
                                datarowTo.ENABLEFLG = False

                                '2013/03/06 TCS 河原 GL0874 START
                                Dim PresenceCategory As String = StaffContext.Current.PresenceCategory
                                Dim PresenceDetail As String = StaffContext.Current.PresenceDetail

                                If PresenceCategory = "2" Or (PresenceCategory = "1" And PresenceDetail = "1") Then
                                    If ActivityRow.STRCD = datarowFrom.SALESFLLWSTRCD And ActivityRow.FLLWUPBOX_SEQNO = datarowFrom.SALESFLLWSEQNO Then
                                        '現在既に商談中(営業活動中)で、商談中の活動と参照している活動が同じ場合
                                        datarowTo.ENABLEFLG = True
                                    End If
                                End If
                                '2013/03/06 TCS 河原 GL0874 END
                            Else
                                '注文キャンセルされているか判定
                                Dim cancelFlg As Boolean = ActivityInfoBusinessLogic.GetSalesCancel(dlrcd, ActivityRow.CONTRACTNO)
                                If (cancelFlg = True) Then
                                    '注文キャンセル中
                                    datarowTo.ENABLEFLG = False
                                Else
                                    '受注後工程フォロー中
                                    If ActivityInfoBusinessLogic.IsExistsUnexecutedAfterOdrAct(datarowTo.FLLWUPBOX_SEQNO) Then
                                        '未完了の必須受注後工程活動有り
                                        datarowTo.ENABLEFLG = True
                                    Else
                                        '未完了の必須受注後工程活動無し
                                        datarowTo.ENABLEFLG = False
                                    End If
                                End If
                            End If
                        Case CractresultGiveup
                            datarowTo.ENABLEFLG = False
                            ' 2012/02/29 TCS 小野 【SALES_2】 END
                            '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
                        Case CractresultHot, CractresultProspect, CractresultCold
                            '2013/06/30 TCS 黄 2013/10対応版　既存流用 End
                            datarowTo.ENABLEFLG = True
                        Case Else
                            If ActivityRow.REQCATEGORY = RequestcategoryWalkin Then
                                datarowTo.ENABLEFLG = True
                            Else
                                datarowTo.ENABLEFLG = False
                            End If
                    End Select
                    ' 2012/02/29 TCS 小野 【SALES_2】 START
                End If
                ' 2012/02/29 TCS 小野 【SALES_2】 END

                '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
                ' 活動担当ソート用フラグ 
                'If String.Equals(ActivityRow.ACCOUNT_PLAN, StaffContext.Current.Account) Then
                If String.Equals(ActivityRow.ACCOUNT_PLAN, account) Then
                    datarowTo.ACCOUNTSORTFLG = True
                Else
                    datarowTo.ACCOUNTSORTFLG = False
                End If
                '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

                datatableTo.Rows.Add(datarowTo)
            Next

            ' 並び替え
            Dim datatableToSort As New SC3080202DataSet.SC3080202GetActivityListToDataTable
            datatableToSort = CType(datatableTo.Clone(), SC3080202DataSet.SC3080202GetActivityListToDataTable)
            Dim dv As DataView = New DataView(datatableTo)

            '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
            'dv.Sort = "ENABLEFLG DESC, CRACTDATE DESC"
            dv.Sort = "ENABLEFLG DESC, ACCOUNTSORTFLG DESC, CRACTDATE DESC"
            '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

            For Each drv As DataRowView In dv
                datatableToSort.ImportRow(drv.Row)
            Next

            Return datatableToSort
        End Using

        Logger.Info("GetActivityList End")
    End Function
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    ''' <summary>
    ''' 活動詳細取得
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetActivityDetail(ByVal datatableFrom As SC3080202DataSet.SC3080202GetActivityDetailFromDataTable) As SC3080202DataSet.SC3080202GetActivityDetailToDataTable
        Logger.Info("GetActivityDetail Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim fllwupbox_seqno As Decimal               'Followupbox seqno
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim contactno As Long = -1L                 '接触方法
        Dim datatableFollowupboxDetail As SC3080202DataSet.SC3080202GetFollowupboxDetailDataTable = Nothing
        Dim datatableCategoryCount As SC3080202DataSet.SC3080202GetCategoryCountDataTable
        Dim datarowFrom As SC3080202DataSet.SC3080202GetActivityDetailFromRow
        Dim dt As SC3080202DataSet.SC3080202GetFollowupboxDetailRow

        ' 引数取得
        datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetActivityDetailFromRow)
        dlrcd = datarowFrom.DLRCD
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        '2016/09/12 TCS 鈴木 性能改善（TR-SLT-TMT-20160726-002） MOD START
        Dim dtSales As SC3080202DataSet.SC3080202GetSalesDataTable = Nothing
        Dim activeOrHistoryFlg As String = String.Empty
        Dim reqOrAttFlg As String = String.Empty
        Dim afterOdrProcFlg As String = String.Empty

        '商談取得
        dtSales = GetSales(fllwupbox_seqno)

        If dtSales.Rows.Count = 0 Then
            '商談がアクティブにもヒストリーにも存在しなければ何もしない

        Else

            'アクティブ/ヒストリーフラグ（0:アクティブ　1:ヒストリー）
            activeOrHistoryFlg = dtSales.Rows(0).Item("ACTIVE_OR_HIS_FLG").ToString()

            If String.Equals(activeOrHistoryFlg, "0") Then
                'アクティブ（TB_T_SALES）に商談有り

                If Not String.Equals(dtSales.Rows(0).Item("REQ_ID").ToString(), "0") Then
                    '用件
                    reqOrAttFlg = "0"
                Else
                    '誘致
                    reqOrAttFlg = "1"
                End If
            Else
                'ヒストリー（TB_H_SALES）に商談有り

                If Not String.Equals(dtSales.Rows(0).Item("REQ_ID").ToString(), "0") Then
                    '用件
                    reqOrAttFlg = "0"
                Else
                    '誘致
                    reqOrAttFlg = "1"
                End If
            End If

            If String.Equals(SC3080202BusinessLogic.GetAfterOdrProcFlg(StaffContext.Current.DlrCD, StaffContext.Current.BrnCD), "0") Then
                '受注後工程を利用しない店舗の場合
                afterOdrProcFlg = "0"
            Else
                '受注後工程を利用する店舗の場合
                afterOdrProcFlg = "1"
            End If

            ' 活動詳細取得
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
            datatableFollowupboxDetail = SC3080202TableAdapter.GetFollowupboxDetail(dlrcd, CType(fllwupbox_seqno, String), activeOrHistoryFlg, reqOrAttFlg, afterOdrProcFlg)
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        End If

        Using datatableTo As New SC3080202DataSet.SC3080202GetActivityDetailToDataTable
            Dim datarowTo As SC3080202DataSet.SC3080202GetActivityDetailToRow
            'Contactno設定
            If (Not (datatableFollowupboxDetail) Is Nothing) Then
                If (datatableFollowupboxDetail.Rows.Count > 0) Then
                    dt = CType(datatableFollowupboxDetail.Rows(0), SC3080202DataSet.SC3080202GetFollowupboxDetailRow)

                    If Not dt.IsCONTACTNONull Then
                        contactno = dt.CONTACTNO
                    End If

                    ' 活動回数取得
                    If contactno <> -1 Then
                        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
                        datatableCategoryCount = SC3080202TableAdapter.GetCategoryCount(CType(fllwupbox_seqno, String), _
                                                                        CType(contactno, String))
                        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
                    Else
                        datatableCategoryCount = New SC3080202DataSet.SC3080202GetCategoryCountDataTable()
                    End If


                    datarowTo = datatableTo.NewSC3080202GetActivityDetailToRow
                    ' 編集
                    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
                    If datatableCategoryCount.Rows.Count < 1 Then
                        datarowTo.CONTACTNAME = String.Empty
                    Else
                        Dim dtCount As SC3080202DataSet.SC3080202GetCategoryCountRow = CType(datatableCategoryCount.Rows(0), SC3080202DataSet.SC3080202GetCategoryCountRow)
                        ' 活動回数
                        datarowTo.COUNT = CLng(dt.COUNT)

                        If dtCount.CNT = 0 Then
                            datarowTo.CONTACTNAME = String.Empty
                        Else
                            ' 2012/03/27 TCS 河原 【SALES_2】 START
                            If dt.COUNTVIEW.Equals("1") Then
                                datarowTo.CONTACTNAME = Replace(WebWordUtility.GetWord(20004), "{0}", CType(dtCount.CNT, String)) & dt.CONTACT
                            Else
                                If dt.CONTACTNO = BOOKEDAFTERCONTACTCD Then
                                    datarowTo.CONTACTNAME = BOOKEDAFTERCONTACT
                                Else
                                    datarowTo.CONTACTNAME = dt.CONTACT
                                End If
                                ' 2012/03/27 TCS 河原 【SALES_2】 END
                            End If
                        End If
                    End If
                    '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
                    If dt.IsSALESSTARTTIMENull Or dt.IsSALESENDTIMENull Then
                        datarowTo.SALESTIME = String.Empty
                    Else
                        datarowTo.SALESTIME =
                            DateTimeFunc.FormatDate(11, dt.SALESSTARTTIME) &
                            " " &
                            DateTimeFunc.FormatDate(14, dt.SALESSTARTTIME) &
                            WebWordUtility.GetWord(20037) &
                            DateTimeFunc.FormatDate(14, dt.SALESENDTIME)
                    End If

                    If dt.IsWALKINNUMNull Then
                        datarowTo.WALKINNUM = String.Empty
                    Else
                        datarowTo.WALKINNUM = Replace(WebWordUtility.GetWord(20005), "{0}", CStr(dt.WALKINNUM))
                    End If

                    If dt.IsOPERATIONCODENull Then
                    Else
                        datarowTo.OPERATIONCODE = dt.OPERATIONCODE
                    End If

                    ' 2012/02/29 TCS 小野 【SALES_2】 START
                    If dt.IsICON_IMGFILENull Then
                    Else
                        datarowTo.ICON_IMGFILE = dt.ICON_IMGFILE
                    End If
                    ' 2012/02/29 TCS 小野 【SALES_2】 END

                    If dt.IsUSERNAMENull Then
                        datarowTo.USERNAME = String.Empty
                    Else
                        datarowTo.USERNAME = dt.USERNAME
                    End If

                    '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
                    ' 活動ID
                    If Not dt.IsACTIDNull Then
                        datarowTo.ACTID = dt.ACTID
                    Else
                        datarowTo.ACTID = 0
                    End If

                    ' 用件ID
                    If Not dt.IsREQIDNull Then
                        datarowTo.REQID = dt.REQID
                    Else
                        datarowTo.REQID = 0
                    End If

                    ' 誘致ID
                    If Not dt.IsATTIDNull Then
                        datarowTo.ATTID = dt.ATTID
                    Else
                        datarowTo.ATTID = 0
                    End If

                    '予定日時
                    If Not dt.IsSCHEDATEORTIMENull Then
                        datarowTo.SCHEDATEORTIME = dt.SCHEDATEORTIME
                    Else
                        datarowTo.SCHEDATEORTIME = CDate(String.Empty)
                    End If

                    ' 用件ロックバージョン
                    If Not dt.IsREQUESTLOCKVERSIONNull Then
                        datarowTo.REQUESTLOCKVERSION = dt.REQUESTLOCKVERSION
                    Else
                        datarowTo.REQUESTLOCKVERSION = 0
                    End If

                    ' 誘致ロックバージョン
                    If Not dt.IsATTRACTLOCKVERSIONNull Then
                        datarowTo.ATTRACTLOCKVERSION = dt.ATTRACTLOCKVERSION
                    Else
                        datarowTo.ATTRACTLOCKVERSION = 0
                    End If

                    ' 商談ロックバージョン
                    If Not dt.IsSALESLOCKVERSIONNull Then
                        datarowTo.SALESLOCKVERSION = dt.SALESLOCKVERSION
                    Else
                        datarowTo.SALESLOCKVERSION = 0
                    End If

                    ' 活動ロックバージョン
                    If Not dt.IsACTIVITYLOCKVERSIONNull Then
                        datarowTo.ACTIVITYLOCKVERSION = dt.ACTIVITYLOCKVERSION
                    Else
                        datarowTo.ACTIVITYLOCKVERSION = 0
                    End If

                    ' 商談完了フラグ
                    If Not dt.IsCOMPFLGNull Then
                        datarowTo.COMPFLG = dt.COMPFLG
                    Else
                        datarowTo.COMPFLG = String.Empty
                    End If

                    ' 断念競合車種連番
                    If Not dt.IsGIVEUPVCLSEQNull Then
                        datarowTo.GIVEUPVCLSEQ = dt.GIVEUPVCLSEQ
                    Else
                        datarowTo.GIVEUPVCLSEQ = 0
                    End If

                    ' 実施コンタクト方法
                    If Not dt.IsRSLTCONTACTMTDNull Then
                        datarowTo.RSLTCONTACTMTD = dt.RSLTCONTACTMTD
                    Else
                        datarowTo.RSLTCONTACTMTD = String.Empty
                    End If

                    ' 実施商談分類
                    If Not dt.IsRSLTSALESCATNull Then
                        datarowTo.RSLTSALESCAT = dt.RSLTSALESCAT
                    Else
                        datarowTo.RSLTSALESCAT = String.Empty
                    End If

                    ' モデルコード
                    If Not dt.IsMODELCODENull Then
                        datarowTo.MODELCODE = dt.MODELCODE
                    Else
                        datarowTo.MODELCODE = String.Empty
                    End If

                    ' 査定モデル名
                    If Not dt.IsMODELNAMENull Then
                        datarowTo.MODELNAME = dt.MODELNAME
                    Else
                        datarowTo.MODELNAME = String.Empty
                    End If

                    ' 最終活動結果ID
                    If Not dt.IsLASTCALLRSLTIDNull Then
                        datarowTo.LASTCALLRSLTID = dt.LASTCALLRSLTID
                    Else
                        datarowTo.LASTCALLRSLTID = 0
                    End If
                    '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

                    '2014/09/04 TCS 武田 UAT不具合対応(最終活動表示) START
                    '受注後活動がある場合、最終活動情報をセットしなおす
                    For Each detailRow As SC3080202DataSet.SC3080202GetFollowupboxDetailRow In datatableFollowupboxDetail
                        If detailRow.ACTID = 0 Then
                            Dim contactno2 As Long = -1L                 '接触方法
                            If Not detailRow.IsCONTACTNONull Then
                                contactno2 = detailRow.CONTACTNO
                            End If

                            ' 活動回数取得
                            If contactno2 <> -1 Then
                                datatableCategoryCount = SC3080202TableAdapter.GetCategoryCount(CType(fllwupbox_seqno, String), _
                                                                                CType(contactno2, String))
                            Else
                                datatableCategoryCount = New SC3080202DataSet.SC3080202GetCategoryCountDataTable()
                            End If

                            ' 編集
                            If datatableCategoryCount.Rows.Count < 1 Then
                                datarowTo.CONTACTNAME = String.Empty
                            Else
                                Dim dtCount2 As SC3080202DataSet.SC3080202GetCategoryCountRow = CType(datatableCategoryCount.Rows(0), SC3080202DataSet.SC3080202GetCategoryCountRow)
                                ' 活動回数
                                datarowTo.COUNT = CLng(detailRow.COUNT)

                                If dtCount2.CNT = 0 Then
                                    datarowTo.CONTACTNAME = String.Empty
                                Else
                                    If detailRow.COUNTVIEW.Equals("1") Then
                                        datarowTo.CONTACTNAME = Replace(WebWordUtility.GetWord(20004), "{0}", CType(dtCount2.CNT, String)) & detailRow.CONTACT
                                    Else
                                        If detailRow.CONTACTNO = BOOKEDAFTERCONTACTCD Then
                                            datarowTo.CONTACTNAME = BOOKEDAFTERCONTACT
                                        Else
                                            datarowTo.CONTACTNAME = detailRow.CONTACT
                                        End If
                                    End If
                                End If
                            End If
                            If detailRow.IsSALESSTARTTIMENull Or detailRow.IsSALESENDTIMENull Then
                                datarowTo.SALESTIME = String.Empty
                            Else
                                datarowTo.SALESTIME =
                                    DateTimeFunc.FormatDate(11, detailRow.SALESSTARTTIME) &
                                    " " &
                                    DateTimeFunc.FormatDate(14, detailRow.SALESSTARTTIME) &
                                    WebWordUtility.GetWord(20037) &
                                    DateTimeFunc.FormatDate(14, detailRow.SALESENDTIME)
                            End If

                            If detailRow.IsWALKINNUMNull Then
                                datarowTo.WALKINNUM = String.Empty
                            Else
                                datarowTo.WALKINNUM = Replace(WebWordUtility.GetWord(20005), "{0}", CStr(detailRow.WALKINNUM))
                            End If

                            If detailRow.IsOPERATIONCODENull Then
                            Else
                                datarowTo.OPERATIONCODE = detailRow.OPERATIONCODE
                            End If

                            '一度セットできたら、繰り返し処理終了
                            Exit For
                        End If

                    Next
                    '2014/09/04 TCS 武田 UAT不具合対応(最終活動表示) END

                    ' DataTableに格納
                    datatableTo.Rows.Add(datarowTo)
                End If
            End If

            Return datatableTo
        End Using
        '2016/09/12 TCS 鈴木 性能改善（TR-SLT-TMT-20160726-002）MOD END

        Logger.Info("GetActivityDetail End")
    End Function

    ''' <summary>
    ''' メモ履歴取得
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSalesMemoHis(ByVal datatableFrom As SC3080202DataSet.SC3080202GetSalesMemoHisFromDataTable) As SC3080202DataSet.SC3080202GetSalesMemoHisToDataTable
        Logger.Info("GetSalesMemoHis Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim fllwupbox_seqno As Decimal               'Followupbox seqno
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim datatableSalesMemoHis As SC3080202DataSet.SC3080202GetSalesMemoListDataTable
        Dim datarowFrom As SC3080202DataSet.SC3080202GetSalesMemoHisFromRow

        ' 引数取得
        datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetSalesMemoHisFromRow)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        ' 活動詳細取得
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        datatableSalesMemoHis = SC3080202TableAdapter.GetSalesMemoHis(dlrcd, CType(fllwupbox_seqno, String))
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

        '2014/02/12 TCS 山口 受注後フォロー機能開発 START
        ' DataTableに格納
        Using datatableTo As New SC3080202DataSet.SC3080202GetSalesMemoHisToDataTable
            Dim datarowTo As SC3080202DataSet.SC3080202GetSalesMemoHisToRow
            '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発
            Dim scDlrCd As String = StaffContext.Current.DlrCD
            For Each dt As SC3080202DataSet.SC3080202GetSalesMemoListRow In datatableSalesMemoHis
                datarowTo = datatableTo.NewSC3080202GetSalesMemoHisToRow
                'datarowTo.INPUTDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.INPUTDATE, StaffContext.Current.DlrCD)
                datarowTo.INPUTDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.INPUTDATE, scDlrCd)
                datarowTo.MEMO = dt.MEMO
                datarowTo.USERNAME = dt.USERNAME
                datarowTo.ICON_IMGFILE = dt.ICON_IMGFILE
                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using
        '2014/02/12 TCS 山口 受注後フォロー機能開発 END

        Logger.Info("GetSalesMemoHis End")
    End Function

    ''' <summary>
    ''' メモ取得
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSalesMemoToday(ByVal datatableFrom As SC3080202DataSet.SC3080202GetSalesMemoTodayFromDataTable) As SC3080202DataSet.SC3080202GetSalesMemoTodayToDataTable
        Logger.Info("GetSalesMemoToday Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim fllwupbox_seqno As Decimal               'Followupbox seqno
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim datatableSalesMemoToday As SC3080202DataSet.SC3080202GetSalesMemoTodayDataTable
        Dim datarowFrom As SC3080202DataSet.SC3080202GetSalesMemoTodayFromRow

        ' 引数取得
        datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetSalesMemoTodayFromRow)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        ' 活動詳細取得
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        datatableSalesMemoToday = SC3080202TableAdapter.GetSalesMemoToday(CType(fllwupbox_seqno, String))
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        ' DataTableに格納
        Using datatableTo As New SC3080202DataSet.SC3080202GetSalesMemoTodayToDataTable
            Dim datarowTo As SC3080202DataSet.SC3080202GetSalesMemoTodayToRow
            For Each datarowSalesMemoToday As SC3080202DataSet.SC3080202GetSalesMemoTodayRow In datatableSalesMemoToday
                datarowTo = datatableTo.NewSC3080202GetSalesMemoTodayToRow
                datarowTo.MEMO = datarowSalesMemoToday.Item(datatableSalesMemoToday.MEMOColumn.ColumnName).ToString()
                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Info("GetSalesMemoToday End")
    End Function

    ' 2012/02/29 TCS 小野 【SALES_2】 START
    Shared Function GetProcess(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetProcessFromDataTable) As ActivityInfoDataSet.ActivityInfoGetProcessToDataTable
        Logger.Info("GetProcess Start")

        ' プロセス取得
        Return ActivityInfoBusinessLogic.GetProcess(datatableFrom)

        Logger.Info("GetProcess End")
    End Function

    Shared Function GetStatus(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetStatusFromDataTable) As ActivityInfoDataSet.ActivityInfoGetStatusToDataTable
        Logger.Info("GetProcess Start")

        ' ステータス取得
        Return ActivityInfoBusinessLogic.GetStatus(datatableFrom)

        Logger.Info("GetProcess End")
    End Function

    Shared Function GetSelectedSeriesList(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromDataTable) As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable
        Logger.Info("GetProcess Start")

        ' 希望車種取得
        Return ActivityInfoBusinessLogic.GetSelectedSeriesList(datatableFrom)

        Logger.Info("GetProcess End")
    End Function
    ' 2012/02/29 TCS 小野 【SALES_2】 END

    ' 2012/02/29 TCS 小野 【SALES_2】 START
    ' ''' <summary>
    ' ''' プロセス取得
    ' ''' </summary>
    ' ''' <param name="datatableFrom">引数DataTable</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Shared Function GetProcess(ByVal datatableFrom As SC3080202DataSet.SC3080202GetProcessFromDataTable) As SC3080202DataSet.SC3080202GetProcessToDataTable
    '    Logger.Info("GetProcess Start")

    '    ' 変数
    '    Dim dlrcd As String                         '販売店コード
    '    Dim strcd As String                         '店舗コード
    '    Dim fllwupbox_seqno As Long                 'FollowupBox連番
    '    Dim datatableProcess As SC3080202DataSet.SC3080202GetProcessDataTable
    '    Dim datarowProcess As SC3080202DataSet.SC3080202GetProcessRow
    '    Dim datarowFrom As SC3080202DataSet.SC3080202GetProcessFromRow
    '    Dim tempSeqno As Long

    '    ' 引数取得
    '    datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetProcessFromRow)
    '    dlrcd = datarowFrom.DLRCD
    '    strcd = datarowFrom.STRCD
    '    fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

    '    ' プロセス取得
    '    datatableProcess = SC3080202TableAdapter.GetProcess(dlrcd, strcd, fllwupbox_seqno)

    '    ' DataTableに格納
    '    If datatableProcess.Rows.Count > 0 Then
    '        datarowProcess = CType(datatableProcess.Rows(0), SC3080202DataSet.SC3080202GetProcessRow)
    '        tempSeqno = datarowProcess.SEQNO
    '    End If

    '    Using datatableTo As New SC3080202DataSet.SC3080202GetProcessToDataTable
    '        Dim datarowTo As SC3080202DataSet.SC3080202GetProcessToRow
    '        datarowTo = datatableTo.NewSC3080202GetProcessToRow
    '        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発
    '        Dim scDlrCd As String = StaffContext.Current.DlrCD
    '        For Each dt As SC3080202DataSet.SC3080202GetProcessRow In datatableProcess
    '            If tempSeqno <> dt.SEQNO Then
    '                datatableTo.Rows.Add(datarowTo)
    '                datarowTo = datatableTo.NewSC3080202GetProcessToRow
    '                tempSeqno = dt.SEQNO
    '            End If

    '            Select Case dt.ACTIONCD
    '                Case ActionCdCatalog
    '                    datarowTo.CATALOGDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.LASTACTDATE, scDlrCd)
    '                Case ActionCdTestdrive
    '                    datarowTo.TESTDRIVEDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.LASTACTDATE, scDlrCd)
    '                Case ActionCdEvaluation
    '                    datarowTo.EVALUATIONDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.LASTACTDATE, scDlrCd)
    '                Case ActionCdQuotation
    '                    datarowTo.QUOTATIONDATE = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, dt.LASTACTDATE, scDlrCd)
    '            End Select

    '            datarowTo.SEQNO = dt.SEQNO
    '        Next

    '        If datatableProcess.Rows.Count > 0 Then
    '            datatableTo.Rows.Add(datarowTo)
    '        End If

    '        Return datatableTo
    '    End Using

    '    Logger.Info("GetProcess End")
    'End Function

    ' ''' <summary>
    ' ''' ステータス取得
    ' ''' </summary>
    ' ''' <param name="datatableFrom">引数DataTable</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Shared Function GetStatus(ByVal datatableFrom As SC3080202DataSet.SC3080202GetStatusFromDataTable) As SC3080202DataSet.SC3080202GetStatusToDataTable
    '    Logger.Info("GetStatus Start")

    '    ' 変数
    '    Dim dlrcd As String                         '販売店コード
    '    Dim strcd As String                         '店舗コード
    '    Dim fllwupbox_seqno As Long                 'FollowupBox連番
    '    Dim datatableStatus As SC3080202DataSet.SC3080202GetStatusDataTable
    '    Dim datarowFrom As SC3080202DataSet.SC3080202GetStatusFromRow

    '    ' 引数取得
    '    datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetStatusFromRow)
    '    dlrcd = datarowFrom.DLRCD
    '    strcd = datarowFrom.STRCD
    '    fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

    '    ' ステータス取得
    '    datatableStatus = SC3080202TableAdapter.GetStatus(dlrcd, strcd, fllwupbox_seqno)

    '    ' DataTableに格納
    '    Using datatableTo As New SC3080202DataSet.SC3080202GetStatusToDataTable
    '        If datatableStatus.Rows.Count > 0 Then
    '            Dim datarowTo As SC3080202DataSet.SC3080202GetStatusToRow
    '            datarowTo = datatableTo.NewSC3080202GetStatusToRow
    '            Dim dt As SC3080202DataSet.SC3080202GetStatusRow =
    '                CType(datatableStatus.Rows(0), SC3080202DataSet.SC3080202GetStatusRow)
    '            datarowTo.CRACTRESULT = dt.CRACTRESULT
    '            datatableTo.Rows.Add(datarowTo)
    '        End If

    '        Return datatableTo
    '    End Using

    '    Logger.Info("GetStatus End")
    'End Function

    ' ''' <summary>
    ' ''' 希望車種リスト取得
    ' ''' </summary>
    ' ''' <param name="datatableFrom">引数DataTable</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Shared Function GetSelectedSeriesList(ByVal datatableFrom As SC3080202DataSet.SC3080202GetSelectedSeriesListFromDataTable) As SC3080202DataSet.SC3080202GetSelectedSeriesListToDataTable
    '    Logger.Info("GetSelectedSeriesList Start")

    '    ' 変数
    '    Dim dlrcd As String                         '販売店コード
    '    Dim strcd As String                         '店舗コード
    '    Dim cntcd As String                         '国コード
    '    Dim fllwupbox_seqno As Long                 'FollowupBox連番
    '    Dim datatableSelectedSeries As SC3080202DataSet.SC3080202GetSelectedSeriesDataTable
    '    Dim datarowFrom As SC3080202DataSet.SC3080202GetSelectedSeriesListFromRow

    '    ' 引数取得
    '    datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetSelectedSeriesListFromRow)
    '    dlrcd = datarowFrom.DLRCD
    '    strcd = datarowFrom.STRCD
    '    cntcd = datarowFrom.CNTCD
    '    fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

    '    ' 活動詳細取得
    '    datatableSelectedSeries =
    '        SC3080202TableAdapter.GetSelectedSeries(dlrcd, strcd, cntcd, fllwupbox_seqno)

    '    ' DataTableに格納
    '    Using datatableTo As New SC3080202DataSet.SC3080202GetSelectedSeriesListToDataTable
    '        For Each dt As SC3080202DataSet.SC3080202GetSelectedSeriesRow In datatableSelectedSeries
    '            Dim datarowTo As SC3080202DataSet.SC3080202GetSelectedSeriesListToRow
    '            datarowTo = datatableTo.NewSC3080202GetSelectedSeriesListToRow
    '            datarowTo.SERIESCD = dt.SERIESCD
    '            datarowTo.SERIESNM = dt.SERIESNM
    '            If dt.IsMODELCDNull Then
    '                datarowTo.MODELCD = String.Empty
    '            Else
    '                datarowTo.MODELCD = dt.MODELCD
    '            End If
    '            If dt.IsVCLMODEL_NAMENull Then
    '                datarowTo.VCLMODEL_NAME = String.Empty
    '            Else
    '                datarowTo.VCLMODEL_NAME = dt.VCLMODEL_NAME
    '            End If
    '            If dt.IsCOLORCDNull Then
    '                datarowTo.COLORCD = String.Empty
    '            Else
    '                datarowTo.COLORCD = dt.COLORCD
    '            End If
    '            If dt.IsDISP_BDY_COLORNull Then
    '                datarowTo.DISP_BDY_COLOR = String.Empty
    '            Else
    '                datarowTo.DISP_BDY_COLOR = dt.DISP_BDY_COLOR
    '            End If
    '            If dt.IsPICIMAGENull Then
    '                datarowTo.PICIMAGE = String.Empty
    '            Else
    '                datarowTo.PICIMAGE = dt.PICIMAGE
    '            End If
    '            If dt.IsLOGOIMAGENull Then
    '                datarowTo.LOGOIMAGE = String.Empty
    '            Else
    '                datarowTo.LOGOIMAGE = dt.LOGOIMAGE
    '            End If
    '            datarowTo.QUANTITY = dt.QUANTITY
    '            datarowTo.SEQNO = dt.SEQNO
    '            datatableTo.Rows.Add(datarowTo)
    '        Next

    '        Return datatableTo
    '    End Using

    '    Logger.Info("GetSelectedSeriesList End")
    'End Function
    ' 2012/02/29 TCS 小野 【SALES_2】 END

    ''' <summary>
    ''' 競合車種リスト取得
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedCompeList(ByVal datatableFrom As SC3080202DataSet.SC3080202GetSelectedCompeListFromDataTable) As SC3080202DataSet.SC3080202GetSelectedCompeListToDataTable
        Logger.Info("GetSelectedCompeList Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim fllwupbox_seqno As Decimal               'FollowupBox連番
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim datatableSelectedCompe As SC3080202DataSet.SC3080202GetSelectedCompeDataTable
        Dim datarowFrom As SC3080202DataSet.SC3080202GetSelectedCompeListFromRow

        ' 引数取得
        datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetSelectedCompeListFromRow)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO

        ' 活動詳細取得
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        datatableSelectedCompe = SC3080202TableAdapter.GetSelectedCompe(CType(fllwupbox_seqno, String))
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        ' DataTableに格納
        Using datatableTo As New SC3080202DataSet.SC3080202GetSelectedCompeListToDataTable
            Dim datarowTo As SC3080202DataSet.SC3080202GetSelectedCompeListToRow
            Dim tempMakerName As String = String.Empty
            For Each dt As SC3080202DataSet.SC3080202GetSelectedCompeRow In datatableSelectedCompe
                datarowTo = datatableTo.NewSC3080202GetSelectedCompeListToRow
                datarowTo.SERIESCD = dt.SERIESCD
                datarowTo.COMPETITORNM = dt.COMPETITORNM
                datarowTo.SEQNO = dt.SEQNO
                datarowTo.COMPETITIONMAKERNO = dt.COMPETITIONMAKERNO
                If tempMakerName.Equals(dt.COMPETITIONMAKER) Then
                    datarowTo.COMPETITIONMAKER = String.Empty
                Else
                    datarowTo.COMPETITIONMAKER = dt.COMPETITIONMAKER
                End If
                tempMakerName = dt.COMPETITIONMAKER
                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Info("GetSelectedCompeList End")
    End Function

    ''' <summary>
    ''' 商談条件取得
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSalesCondition(ByVal datatableFrom As SC3080202DataSet.SC3080202GetSalesConditionFromDataTable) As SC3080202DataSet.SC3080202GetSalesConditionToDataTable
        Logger.Info("GetSalesCondition Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim strcd As String                         '店舗コード
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim fllwupbox_seqno As Decimal               'FollowupBox連番
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim datatableSalesCondition As SC3080202DataSet.SC3080202GetSalesConditionDataTable
        Dim datatableSalesConditionMaster As SC3080202DataSet.SC3080202GetSalesConditionMasterDataTable
        Dim datarowFrom As SC3080202DataSet.SC3080202GetSalesConditionFromRow

        ' 引数取得
        datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetSalesConditionFromRow)
        dlrcd = datarowFrom.DLRCD
        strcd = datarowFrom.STRCD
        If datarowFrom.IsFLLWUPBOX_SEQNONull Then
        Else
            fllwupbox_seqno = datarowFrom.FLLWUPBOX_SEQNO
        End If

        ' 商談条件取得
        If datarowFrom.IsFLLWUPBOX_SEQNONull Then
            datatableSalesCondition = New SC3080202DataSet.SC3080202GetSalesConditionDataTable
        Else
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
            datatableSalesCondition = SC3080202TableAdapter.GetSalesCondition(CType(fllwupbox_seqno, String))
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        End If
        datatableSalesConditionMaster = SC3080202TableAdapter.GetSalesConditionMaster()

        Dim rowCollection As DataRowCollection = datatableSalesCondition.Rows
        Dim rowCollect As SC3080202DataSet.SC3080202GetSalesConditionRow

        ' DataTableに格納
        Using datatableTo As New SC3080202DataSet.SC3080202GetSalesConditionToDataTable
            Dim datarowTo As SC3080202DataSet.SC3080202GetSalesConditionToRow
            For Each dt As SC3080202DataSet.SC3080202GetSalesConditionMasterRow In datatableSalesConditionMaster
                datarowTo = datatableTo.NewSC3080202GetSalesConditionToRow
                datarowTo.SALESCONDITIONNO = dt.SALESCONDITIONNO
                datarowTo.TITLE = dt.TITLE
                datarowTo.AND_OR = dt.AND_OR
                datarowTo.ITEMNO = dt.ITEMNO
                datarowTo.ITEMTITLE = dt.ITEMTITLE
                datarowTo.OTHER = dt.OTHER
                '2013/12/05 TCS 市川 Aカード情報相互連携開発 START
                datarowTo.IS_MANDATORY = dt.IS_MANDATORY
                '2013/12/05 TCS 市川 Aカード情報相互連携開発 END
                datarowTo.CHECKFLG = False
                datarowTo.OTHERSALESCONDITION = String.Empty

                datarowTo.DEFAULT_ITEMTITLE = dt.ITEMTITLE

                '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
                '可変項目名アイテムの場合
                If "2".Equals(dt.OTHER) Then
                    'ITEMTITLEの置換文字列(%1)を、画面表示用の置換文字列に置換する
                    datarowTo.ITEMTITLE = dt.ITEMTITLE.Replace("%1", WebWordUtility.GetWord(LC_WORDNO_REPLACE_TXT_ITEMTITLE))
                End If
                '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

                Dim arrKeyVals(1) As Object
                arrKeyVals(0) = datarowTo.SALESCONDITIONNO
                arrKeyVals(1) = datarowTo.ITEMNO

                If rowCollection.Contains(arrKeyVals) Then
                    datarowTo.CHECKFLG = True
                    rowCollect = CType(rowCollection.Find(arrKeyVals), SC3080202DataSet.SC3080202GetSalesConditionRow)
                    If rowCollect.IsOTHERSALESCONDITIONNull Then
                        datarowTo.OTHERSALESCONDITION = String.Empty
                    Else
                        datarowTo.OTHERSALESCONDITION = rowCollect.OTHERSALESCONDITION

                        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
                        '可変項目名アイテムの場合
                        If "2".Equals(dt.OTHER) Then
                            'ITEMTITLEの置換文字列(%1)を、OTHERSALESCONDITIONで置換する
                            datarowTo.ITEMTITLE = dt.ITEMTITLE.Replace("%1", rowCollect.OTHERSALESCONDITION)
                        End If
                        '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

                    End If
                End If

                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Info("GetSalesCondition End")
    End Function

    ''' <summary>
    ''' 選択車種シリーズマスタ取得
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedSeriesMaster(ByVal datatableFrom As SC3080202DataSet.SC3080202GetSeriesMasterFromDataTable) As SC3080202DataSet.SC3080202GetSeriesMasterToDataTable
        Logger.Info("GetSelectedSeriesMaster Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim cntcd As String                         '国コード
        Dim datatableSeriesMaster As SC3080202DataSet.SC3080202GetSeriesMasterDataTable
        Dim datarowFrom As SC3080202DataSet.SC3080202GetSeriesMasterFromRow

        ' 引数取得
        datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetSeriesMasterFromRow)
        dlrcd = datarowFrom.DLRCD
        cntcd = datarowFrom.CNTCD

        ' 活動詳細取得
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        datatableSeriesMaster = SC3080202TableAdapter.GetSelectedSeriesMaster(dlrcd)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        ' DataTableに格納
        Using datatableTo As New SC3080202DataSet.SC3080202GetSeriesMasterToDataTable
            Dim datarowTo As SC3080202DataSet.SC3080202GetSeriesMasterToRow
            For Each dt As SC3080202DataSet.SC3080202GetSeriesMasterRow In datatableSeriesMaster
                datarowTo = datatableTo.NewSC3080202GetSeriesMasterToRow
                datarowTo.SERIESCD = dt.SERIESCD
                datarowTo.SERIESNM = dt.SERIESNM

                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Info("GetSelectedSeriesMaster End")
    End Function

    ''' <summary>
    ''' 選択車種グレードマスタ取得
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedGradeMaster(ByVal datatableFrom As SC3080202DataSet.SC3080202GetModelMasterFromDataTable) As SC3080202DataSet.SC3080202GetModelMasterToDataTable
        Logger.Info("GetSelectedGradeMaster Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim cntcd As String                         '国コード
        Dim datatableGradeMaster As SC3080202DataSet.SC3080202GetModelMasterDataTable
        Dim datarowFrom As SC3080202DataSet.SC3080202GetModelMasterFromRow

        ' 引数取得
        datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetModelMasterFromRow)
        dlrcd = datarowFrom.DLRCD
        cntcd = datarowFrom.CNTCD

        ' 活動詳細取得
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        datatableGradeMaster = SC3080202TableAdapter.GetSelectedGradeMaster(dlrcd)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        ' DataTableに格納
        Using datatableTo As New SC3080202DataSet.SC3080202GetModelMasterToDataTable
            Dim datarowTo As SC3080202DataSet.SC3080202GetModelMasterToRow
            For Each dt As SC3080202DataSet.SC3080202GetModelMasterRow In datatableGradeMaster
                datarowTo = datatableTo.NewSC3080202GetModelMasterToRow
                datarowTo.SERIESCD = dt.SERIESCD
                datarowTo.VCLMODEL_CODE = dt.VCLMODEL_CODE
                datarowTo.VCLMODEL_NAME = dt.VCLMODEL_NAME

                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Info("GetSelectedGradeMaster End")
    End Function

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 選択車種サフィックスマスタ取得
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedSuffixMaster(ByVal datatableFrom As SC3080202DataSet.SC3080202GetSuffixMasterFromDataTable) As SC3080202DataSet.SC3080202GetSuffixMasterToDataTable
        Logger.Info("GetSelectedSuffixMaster Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim cntcd As String                         '国コード
        Dim datatableSuffixMaster As SC3080202DataSet.SC3080202GetSuffixMasterDataTable
        Dim datarowFrom As SC3080202DataSet.SC3080202GetSuffixMasterFromRow

        ' 引数取得
        datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetSuffixMasterFromRow)
        dlrcd = datarowFrom.DLRCD
        cntcd = datarowFrom.CNTCD

        ' サフィックスマスタ取得
        datatableSuffixMaster = SC3080202TableAdapter.GetSelectedSuffixMaster(dlrcd)

        ' DataTableに格納
        Using datatableTo As New SC3080202DataSet.SC3080202GetSuffixMasterToDataTable
            Dim datarowTo As SC3080202DataSet.SC3080202GetSuffixMasterToRow
            For Each dt As SC3080202DataSet.SC3080202GetSuffixMasterRow In datatableSuffixMaster
                datarowTo = datatableTo.NewSC3080202GetSuffixMasterToRow

                datarowTo.MODEL_CD = dt.MODEL_CD
                datarowTo.GRADE_CD = dt.GRADE_CD
                datarowTo.SUFFIX_CD = dt.SUFFIX_CD
                datarowTo.SUFFIX_NAME = dt.SUFFIX_NAME

                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Info("GetSelectedSuffixMaster End")
    End Function

    ''' <summary>
    ''' 選択車種外装色マスタ取得
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedExteriorColorMaster(ByVal datatableFrom As SC3080202DataSet.SC3080202GetExteriorColorMasterFromDataTable) As SC3080202DataSet.SC3080202GetExteriorColorMasterToDataTable
        Logger.Info("GetSelectedColorMaster Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim cntcd As String                         '国コード
        Dim datatableColorMaster As SC3080202DataSet.SC3080202GetExteriorColorMasterDataTable
        Dim datarowFrom As SC3080202DataSet.SC3080202GetExteriorColorMasterFromRow

        ' 引数取得
        datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetExteriorColorMasterFromRow)
        dlrcd = datarowFrom.DLRCD
        cntcd = datarowFrom.CNTCD

        ' 活動詳細取得

        datatableColorMaster = SC3080202TableAdapter.GetSelectedColorMaster(dlrcd)

        ' DataTableに格納
        Using datatableTo As New SC3080202DataSet.SC3080202GetExteriorColorMasterToDataTable
            Dim datarowTo As SC3080202DataSet.SC3080202GetExteriorColorMasterToRow
            For Each dt As SC3080202DataSet.SC3080202GetExteriorColorMasterRow In datatableColorMaster
                datarowTo = datatableTo.NewSC3080202GetExteriorColorMasterToRow
                datarowTo.SERIESCD = dt.SERIESCD
                datarowTo.VCLMODEL_CODE = dt.VCLMODEL_CODE
                datarowTo.SUFFIX_CD = dt.SUFFIX_CD
                datarowTo.BODYCLR_CD = dt.BODYCLR_CD
                datarowTo.DISP_BDY_COLOR = dt.DISP_BDY_COLOR

                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Info("GetSelectedColorMaster End")
    End Function

    ''' <summary>
    ''' 選択車種内装色マスタ取得
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedInteriorColorMaster(ByVal datatableFrom As SC3080202DataSet.SC3080202GetInteriorColorMasterFromDataTable) As SC3080202DataSet.SC3080202GetInteriorColorMasterToDataTable
        Logger.Info("GetSelectedInteriorColorMaster Start")

        ' 変数
        Dim dlrcd As String                         '販売店コード
        Dim cntcd As String                         '国コード
        Dim datatableInteriorColorMaster As SC3080202DataSet.SC3080202GetInteriorColorMasterDataTable
        Dim datarowFrom As SC3080202DataSet.SC3080202GetInteriorColorMasterFromRow

        ' 引数取得
        datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetInteriorColorMasterFromRow)
        dlrcd = datarowFrom.DLRCD
        cntcd = datarowFrom.CNTCD

        ' 内装色マスタ取得
        datatableInteriorColorMaster = SC3080202TableAdapter.GetSelectedInteriorColorMaster(dlrcd)

        ' DataTableに格納
        Using datatableTo As New SC3080202DataSet.SC3080202GetInteriorColorMasterToDataTable
            Dim datarowTo As SC3080202DataSet.SC3080202GetInteriorColorMasterToRow
            For Each dt As SC3080202DataSet.SC3080202GetInteriorColorMasterRow In datatableInteriorColorMaster
                datarowTo = datatableTo.NewSC3080202GetInteriorColorMasterToRow
                datarowTo.MODEL_CD = dt.MODEL_CD
                datarowTo.GRADE_CD = dt.GRADE_CD
                datarowTo.SUFFIX_CD = dt.SUFFIX_CD
                datarowTo.BODYCLR_CD = dt.BODYCLR_CD
                datarowTo.INTERIORCLR_CD = dt.INTERIORCLR_CD
                datarowTo.INTERIORCLR_NAME = dt.INTERIORCLR_NAME

                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Info("GetSelectedInteriorColorMaster End")
    End Function
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START    
    ''' <summary>
    ''' 競合車種メーカーマスタ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedCompeMakerMaster(ByVal dlrcd As String) As SC3080202DataSet.SC3080202GetCompeMakerMasterToDataTable
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Logger.Info("GetSelectedCompeMakerMaster Start")

        ' 変数
        Dim datatableCompeMakerMaster As SC3080202DataSet.SC3080202GetCompeMakerMasterDataTable

        ' 活動詳細取得
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        datatableCompeMakerMaster = SC3080202TableAdapter.GetSelectedCompeMakerMaster(dlrcd)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        ' DataTableに格納
        Using datatableTo As New SC3080202DataSet.SC3080202GetCompeMakerMasterToDataTable
            Dim datarowTo As SC3080202DataSet.SC3080202GetCompeMakerMasterToRow
            For Each dt As SC3080202DataSet.SC3080202GetCompeMakerMasterRow In datatableCompeMakerMaster
                datarowTo = datatableTo.NewSC3080202GetCompeMakerMasterToRow
                datarowTo.COMPETITIONMAKERNO = dt.COMPETITIONMAKERNO
                datarowTo.COMPETITIONMAKER = dt.COMPETITIONMAKER

                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Info("GetSelectedCompeMakerMaster End")
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START    
    ''' <summary>
    ''' 競合車種モデル取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSelectedCompeModelMaster(ByVal dlrcd As String) As SC3080202DataSet.SC3080202GetCompeModelMasterToDataTable
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Logger.Info("GetSelectedCompeModelMaster Start")

        ' 変数
        Dim datatableCompeModelMaster As SC3080202DataSet.SC3080202GetCompeModelMasterDataTable

        ' 活動詳細取得
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        datatableCompeModelMaster = SC3080202TableAdapter.GetSelectedCompeModelMaster(dlrcd)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        ' DataTableに格納
        Using datatableTo As New SC3080202DataSet.SC3080202GetCompeModelMasterToDataTable
            Dim datarowTo As SC3080202DataSet.SC3080202GetCompeModelMasterToRow
            For Each dt As SC3080202DataSet.SC3080202GetCompeModelMasterRow In datatableCompeModelMaster
                datarowTo = datatableTo.NewSC3080202GetCompeModelMasterToRow
                datarowTo.COMPETITIONMAKERNO = dt.COMPETITIONMAKERNO
                datarowTo.COMPETITORCD = dt.COMPETITORCD
                datarowTo.COMPETITORNM = dt.COMPETITORNM

                datatableTo.Rows.Add(datarowTo)
            Next

            Return datatableTo
        End Using

        Logger.Info("GetSelectedCompeModelMaster End")
    End Function

    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
    ''' <summary>
    ''' 商談条件登録
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Function UpdateSalesCondition(ByVal datatableFrom As SC3080202DataSet.SC3080202UpdateSalesConditionFromDataTable, _
                                  ByVal datatableFromLocal As SC3080202DataSet.SC3080202GetSalesLocalDataTable, _
                                  ByRef msgId As Integer) As SC3080202DataSet.SC3080202GetSeqnoToDataTable Implements ISC3080202BusinessLogic.UpdateSalesCondition

        Logger.Info("UpdateSalesCondition Start")

        ' FollowupboxSeqno
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim updateFllwupboxseqno As Nullable(Of Decimal)
        Dim rtnFllwupboxseqno As Nullable(Of Decimal)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim updateFllwupboxstrcd As String = String.Empty
        ' Seqno取得用
        Dim datatableFllwupboxSeqno As New SC3080202DataSet.SC3080202GetFllwupboxNoDataTable
        'Dim datarowFllwupboxSeqno As SC3080202DataSet.SC3080202GetFllwupboxNoRow'$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 
        ' 1行目のFollowupboxSeqno取得
        Dim datarowFrom As SC3080202DataSet.SC3080202UpdateSalesConditionFromRow
        ' DataRow取得
        datarowFrom = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202UpdateSalesConditionFromRow)
        ' FollowupboxSeqnoを設定
        If datarowFrom.IsFLLWUPBOX_SEQNONull Then
        Else
            updateFllwupboxseqno = datarowFrom.FLLWUPBOX_SEQNO
            updateFllwupboxstrcd = datarowFrom.STRCD
        End If
        ' 1行目削除
        datatableFrom.RemoveSC3080202UpdateSalesConditionFromRow(datarowFrom)

        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
        '' FollowupboxSeqnoがNULLならば、取得する
        'If IsNothing(updateFllwupboxseqno) Then
        '    datatableFllwupboxSeqno = SC3080202TableAdapter.GetFllwupboxSeqno()
        '    datarowFllwupboxSeqno =
        '        CType(datatableFllwupboxSeqno.Rows(0), SC3080202DataSet.SC3080202GetFllwupboxNoRow)
        '    updateFllwupboxseqno = datarowFllwupboxSeqno.SEQ
        '    rtnFllwupboxseqno = updateFllwupboxseqno
        '    updateFllwupboxstrcd = StaffContext.Current.BrnCD
        'End If
        Dim scDlrCD As String = StaffContext.Current.DlrCD
        Dim account As String = StaffContext.Current.Account
        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

        ' 商談条件一斉削除
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        SC3080202TableAdapter.DeleteFollowupboxSalesCondition(CType(updateFllwupboxseqno, String))
        ' データ分追加
        For Each dt As SC3080202DataSet.SC3080202UpdateSalesConditionFromRow In datatableFrom
            ' 商談条件登録
            SC3080202TableAdapter.AddFollowupboxSalesCondition(scDlrCD,
                                                            updateFllwupboxstrcd,
                                                            CDec(updateFllwupboxseqno),
                                                            dt.SALESCONDITIONNO,
                                                            dt.ITEMNO,
                                                            dt.OTHERSALESCONDITION,
                                                            dt.CSTKIND,
                                                            dt.CSTCLASS,
                                                            dt.CUSTCD,
                                                            account,
                                                            DisplayId)
        Next
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END

        If Not UpdateSalesLocal(datatableFromLocal) Then
            msgId = 901
            Me.Rollback = True
            Return Nothing
        End If

        ' 返却用DataTable、DataRow
        Using datatableTo As New SC3080202DataSet.SC3080202GetSeqnoToDataTable
            If datatableFllwupboxSeqno.Rows.Count > 0 Then
                Dim datarowTo As SC3080202DataSet.SC3080202GetSeqnoToRow
                datarowTo = datatableTo.NewSC3080202GetSeqnoToRow
                If IsNothing(rtnFllwupboxseqno) Then
                Else
                    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
                    datarowTo.FLLWUPBOX_SEQNO = CDec(rtnFllwupboxseqno)
                    '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
                End If
                datatableTo.Rows.Add(datarowTo)
            End If
            ' 正常終了
            Return datatableTo
        End Using

        Logger.Info("UpdateSalesCondition End")
    End Function
    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END

    '2017/11/20 TCS 河原 TKM独自機能開発 START
    ''' <summary>
    ''' 希望車種登録
    ''' </summary>
    ''' <param name="datatablefrom">引数DataTable</param>
    ''' <param name="msgId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Function UpdateSelectedSeries(ByVal datatablefrom As SC3080202DataSet.SC3080202UpdateSelectedSeriesFromDataTable,
                                  ByRef msgId As Integer) As SC3080202DataSet.SC3080202GetSeqnoToDataTable Implements ISC3080202BusinessLogic.UpdateSelectedSeries

        Logger.Info("UpdateSelectedSeries Start")

        Dim updateFllwupboxseqno As Nullable(Of Decimal)
        Dim updateSeqno As Nullable(Of Decimal)
        Dim updateFllwupboxstrcd As String = String.Empty
        ' Seqno取得用
        Dim datatableFllwupboxSeqno As New SC3080202DataSet.SC3080202GetFllwupboxNoDataTable
        ' Seqno取得用
        Dim datatableSeqno As SC3080202DataSet.SC3080202GetSelectedSeriesNoDataTable
        Dim datarowSeqno As SC3080202DataSet.SC3080202GetSelectedSeriesNoRow
        ' 1行目のFollowupboxSeqno取得
        Dim datarowFrom As SC3080202DataSet.SC3080202UpdateSelectedSeriesFromRow
        datarowFrom = CType(datatablefrom.Rows(0), SC3080202DataSet.SC3080202UpdateSelectedSeriesFromRow)
        ' NULLでなければ、FollowupboxSeqnoを設定
        If datarowFrom.IsFLLWUPBOX_SEQNONull Then
        Else
            updateFllwupboxseqno = datarowFrom.FLLWUPBOX_SEQNO
            updateFllwupboxstrcd = datarowFrom.STRCD
        End If
        ' NULLでなければ、Seqnoを設定
        If datarowFrom.IsSEQNONull Then
        Else
            updateSeqno = datarowFrom.SEQNO
        End If

        Dim scDlrCD As String = StaffContext.Current.DlrCD

        ' 入力チェック
        ' 活動詳細取得
        Dim datatableSelectedSeries As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesDataTable
        datatableSelectedSeries =
            ActivityInfoTableAdapter.GetSelectedSeries(scDlrCD,
                                                             updateFllwupboxstrcd,
                                                             EnvironmentSetting.CountryCode,
                                                             CDec(updateFllwupboxseqno))

        ' 重複確認
        ' 車種・グレード・モデルが重複しない場合のみ、希望車種を登録可能
        If (EditFlgAdd.Equals(datarowFrom.EDITFLG) Or CheckFlgEdit.Equals(datarowFrom.CHECKFLG)) Then
            For Each dt As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesRow In datatableSelectedSeries
                If dt.SEQNO = updateSeqno Then
                Else
                    If (Trim(datarowFrom.SERIESCD).Equals(Trim(dt.SERIESCD))) Then
                        If (String.IsNullOrEmpty(Trim(dt.MODELCD))) Then
                            msgId = ErrMsgid20902
                            Return Nothing
                        Else
                            If (String.IsNullOrEmpty(datarowFrom.MODELCD)) Then
                                msgId = ErrMsgid20902
                                Return Nothing
                            Else
                                If (Trim(datarowFrom.MODELCD).Equals(Trim(dt.MODELCD))) Then
                                    If (String.IsNullOrEmpty(Trim(dt.SUFFIX_CD))) Then
                                        msgId = ErrMsgid20902
                                        Return Nothing
                                    Else
                                        If (String.IsNullOrEmpty(datarowFrom.SUFFIX_CD)) Then
                                            msgId = ErrMsgid20902
                                            Return Nothing
                                        Else
                                            If (Trim(datarowFrom.SUFFIX_CD).Equals(Trim(dt.SUFFIX_CD))) Then
                                                If (String.IsNullOrEmpty(Trim(dt.COLORCD))) Then
                                                    msgId = ErrMsgid20902
                                                    Return Nothing
                                                Else
                                                    If (String.IsNullOrEmpty(datarowFrom.COLORCD)) Then
                                                        msgId = ErrMsgid20902
                                                        Return Nothing
                                                    Else
                                                        If (Trim(datarowFrom.COLORCD).Equals(Trim(dt.COLORCD))) Then
                                                            If (String.IsNullOrEmpty(Trim(dt.INTERIORCLR_CD))) Then
                                                                msgId = ErrMsgid20902
                                                                Return Nothing
                                                            Else
                                                                If (String.IsNullOrEmpty(datarowFrom.INTERIORCLR_CD)) Then
                                                                    msgId = ErrMsgid20902
                                                                    Return Nothing
                                                                Else
                                                                    If (Trim(datarowFrom.INTERIORCLR_CD).Equals(Trim(dt.INTERIORCLR_CD))) Then
                                                                        msgId = ErrMsgid20902
                                                                        Return Nothing
                                                                    End If
                                                                End If
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Next
        End If

        If Not IsNothing(updateSeqno) Then
            '商談テーブル行ロック
            Try
                SC3080202TableAdapter.SelectSalesLock(CType(updateFllwupboxseqno, String))
            Catch ex As OracleExceptionEx
                msgId = 901
                Return Nothing
            End Try
        End If

        '追加希望車を強制的に一押しにする。
        If EditFlgAdd.Equals(datarowFrom.EDITFLG) Then datarowFrom.MOST_PREF_VCL_FLG = "1"

        '一押し希望車フラグ(商談見込み度コードを利用)
        Dim mostPerfCd As String = " " '一押しで無い場合は半角スペースとする。

        '一押し希望車を編集する時の処置
        If String.Equals("1", datarowFrom.MOST_PREF_VCL_FLG) Then
            '一押し希望車のクリア
            SC3080202TableAdapter.ClearSalesProspectCd(Decimal.Parse(updateFllwupboxseqno.ToString()), StaffContext.Current.Account)
            datarowFrom.LOCKVERSION += 1
            '一押しを示す商談見込み度コードを取得
            mostPerfCd = GetSysEnvSettingValue(ENVSETTINGKEY_MOST_PREFERRED_PROSPECT_CD)
        End If

        If EditFlgAdd.Equals(datarowFrom.EDITFLG) Then
            ' SeqnoがNULLならば、取得する
            If IsNothing(updateSeqno) Then

                datatableSeqno = SC3080202TableAdapter.GetSelectedSeriesSeqno(CType(updateFllwupboxseqno, String))

                datarowSeqno = CType(datatableSeqno.Rows(0), SC3080202DataSet.SC3080202GetSelectedSeriesNoRow)
                updateSeqno = datarowSeqno.SEQ
            End If

            '選択車種追加
            SC3080202TableAdapter.AddSelectedSeries(CType(updateFllwupboxseqno, String),
                                                    CType(updateSeqno, String),
                                                    datarowFrom.SERIESCD,
                                                    datarowFrom.MODELCD,
                                                    datarowFrom.SUFFIX_CD,
                                                    datarowFrom.COLORCD,
                                                    datarowFrom.INTERIORCLR_CD,
                                                    StaffContext.Current.Account,
                                                    mostPerfCd)

        Else

            If datarowFrom.IsSEQNONull Then
            Else
                updateSeqno = datarowFrom.SEQNO
            End If

            Dim lockvr As Long = datarowFrom.LOCKVERSION
            Dim ret As Integer

            If CheckFlgDelete.Equals(datarowFrom.CHECKFLG) Then
                ' 選択車種削除
                ret = SC3080202TableAdapter.DeleteSelectedSeries(CType(updateFllwupboxseqno, String),
                                                                    CType(updateSeqno, String),
                                                                    lockvr)

                If String.Equals("1", datarowFrom.MOST_PREF_VCL_FLG) Then SC3080202TableAdapter.SetDefaultSalesProspectCd(Decimal.Parse(updateFllwupboxseqno.ToString()), StaffContext.Current.Account, mostPerfCd)


                If ret = 0 Then
                    msgId = 901
                    Me.Rollback = True
                    Return Nothing
                End If
            Else

                ret = SC3080202TableAdapter.UpdateSelectedSeries(CType(updateFllwupboxseqno, String),
                                                                 CDec(updateSeqno),
                                                                 datarowFrom.SERIESCD,
                                                                 datarowFrom.MODELCD,
                                                                 datarowFrom.SUFFIX_CD,
                                                                 datarowFrom.COLORCD,
                                                                 datarowFrom.INTERIORCLR_CD,
                                                                 lockvr,
                                                                 StaffContext.Current.Account,
                                                                 mostPerfCd)

                If ret = 0 Then
                    msgId = 901
                    Me.Rollback = True
                    Return Nothing
                End If
            End If

        End If

        Using datatableTo As New SC3080202DataSet.SC3080202GetSeqnoToDataTable
            ' 返却用DataTable、DataRow
            If datatableFllwupboxSeqno.Rows.Count > 0 Then
                Dim datarowTo As SC3080202DataSet.SC3080202GetSeqnoToRow
                datarowTo = datatableTo.NewSC3080202GetSeqnoToRow

                datarowTo.FLLWUPBOX_SEQNO = CDec(updateFllwupboxseqno)

                datarowTo.SEQ = CLng(updateSeqno)
                datatableTo.Rows.Add(datarowTo)
            End If

            ' 正常終了
            Return datatableTo
        End Using

        Logger.Info("UpdateSelectedSeries End")
    End Function    '2017/11/20 TCS 河原 TKM独自機能開発 END

    ''' <summary>
    ''' 商談メモ登録
    ''' </summary>
    ''' <param name="datatablefrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Function UpdateSalesMemo(ByVal datatablefrom As SC3080202DataSet.SC3080202UpdateSalesMemoFromDataTable) As SC3080202DataSet.SC3080202GetSeqnoToDataTable Implements ISC3080202BusinessLogic.UpdateSalesMemo
        Logger.Info("UpdateSalesMemo Start")

        Dim datarowFrom As SC3080202DataSet.SC3080202UpdateSalesMemoFromRow
        datarowFrom = CType(datatablefrom.Rows(0), SC3080202DataSet.SC3080202UpdateSalesMemoFromRow)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim updateFllwupboxseqno As Decimal
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim updateFllwupboxstrcd As String = String.Empty
        ' Seqno取得用
        Dim datatableFllwupboxSeqno As New SC3080202DataSet.SC3080202GetFllwupboxNoDataTable
        'Dim datarowFllwupboxSeqno As SC3080202DataSet.SC3080202GetFllwupboxNoRow'$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 

        ' NULLでなければ、FollowupboxSeqnoを設定
        If datarowFrom.IsFLLWUPBOX_SEQNONull Then
        Else
            updateFllwupboxseqno = datarowFrom.FLLWUPBOX_SEQNO
            updateFllwupboxstrcd = datarowFrom.STRCD
        End If

        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
        '' FollowupboxSeqnoがNULLならば、取得する
        'If datarowFrom.IsFLLWUPBOX_SEQNONull Then
        '    datatableFllwupboxSeqno = SC3080202TableAdapter.GetFllwupboxSeqno()
        '    datarowFllwupboxSeqno = CType(datatableFllwupboxSeqno.Rows(0), SC3080202DataSet.SC3080202GetFllwupboxNoRow)
        '    updateFllwupboxseqno = datarowFllwupboxSeqno.SEQ
        '    updateFllwupboxstrcd = StaffContext.Current.BrnCD
        'End If
        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

        ' メモ編集　データがあれば追加、なければ編集
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        If SC3080202TableAdapter.GetSalesMemoToday(CType(updateFllwupboxseqno, String)).Count = 0 Then
            ' 追加
            SC3080202TableAdapter.AddFollowupboxSalesMemo(StaffContext.Current.DlrCD,
                                                                    updateFllwupboxstrcd,
                                                                    updateFllwupboxseqno,
                                                                    datarowFrom.MEMO,
                                                                    StaffContext.Current.Account,
                                                                    DisplayId)
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Else
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
            'Follow-up Box商談メモロック処理
            Try
                SC3080202TableAdapter.SelectFollowupBoxSalesConditionLock(CType(updateFllwupboxseqno, String))
            Catch ex As OracleExceptionEx
                Return Nothing
            End Try
            Dim ret As Integer
            ' 編集
            ret = SC3080202TableAdapter.UpdateFollowupboxSalesMemo(CType(updateFllwupboxseqno, String),
                                                                        datarowFrom.MEMO,
                                                                        StaffContext.Current.Account,
                                                                        DisplayId)
            If ret = 0 Then
                Me.Rollback = True
                Return Nothing
            End If
            '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        End If
        Using datatableTo As New SC3080202DataSet.SC3080202GetSeqnoToDataTable
            If datatableFllwupboxSeqno.Rows.Count > 0 Then
                Dim datarowTo As SC3080202DataSet.SC3080202GetSeqnoToRow
                datarowTo = datatableTo.NewSC3080202GetSeqnoToRow
                datarowTo.FLLWUPBOX_SEQNO = updateFllwupboxseqno
                datatableTo.Rows.Add(datarowTo)
            End If
            ' 正常終了
            Return datatableTo
        End Using

        Logger.Info("UpdateSalesMemo End")
    End Function

    ''' <summary>
    ''' 台数登録
    ''' </summary>
    ''' <param name="datatablefrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Function UpdateSelectedVclCount(ByVal datatablefrom As SC3080202DataSet.SC3080202UpdateSelectedVclCountFromDataTable) As Boolean Implements ISC3080202BusinessLogic.UpdateSelectedVclCount
        Logger.Info("UpdateSelectedVclCount Start")

        Dim datarowFrom As SC3080202DataSet.SC3080202UpdateSelectedVclCountFromRow
        datarowFrom = CType(datatablefrom.Rows(0), SC3080202DataSet.SC3080202UpdateSelectedVclCountFromRow)

        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        '商談テーブルロック処理
        Try
            SC3080202TableAdapter.SelectSalesLock(CType(datarowFrom.FLLWUPBOX_SEQNO, String))
        Catch ex As OracleExceptionEx
            Return False
        End Try

        Dim lockvr As Long = datarowFrom.LOCKVERSION
        Dim ret As Integer

        ' 台数更新
        ret = SC3080202TableAdapter.UpdateSelectedSeriesQuantity(CType(datarowFrom.FLLWUPBOX_SEQNO, String),
                                                                        CType(datarowFrom.SEQNO, String),
                                                                        CType(datarowFrom.QUANTITY, String),
                                                                        lockvr,
                                                                        StaffContext.Current.Account)
        If ret = 0 Then
            Me.Rollback = True
            Return False
        End If
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        ' 正常終了
        Return True

        Logger.Info("UpdateSelectedVclCount End")
    End Function

    ''' <summary>
    ''' 競合車種登録
    ''' </summary>
    ''' <param name="datatablefrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Function UpdateSelectedCompe(ByVal datatablefrom As SC3080202DataSet.SC3080202UpdateSelectedCompeFromDataTable) As SC3080202DataSet.SC3080202GetSeqnoToDataTable Implements ISC3080202BusinessLogic.UpdateSelectedCompe
        Logger.Info("UpdateSelectedCompe Start")

        Dim datarowFrom As SC3080202DataSet.SC3080202UpdateSelectedCompeFromRow
        datarowFrom = CType(datatablefrom.Rows(0), SC3080202DataSet.SC3080202UpdateSelectedCompeFromRow)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
        Dim updateFllwupboxseqno As Nullable(Of Decimal)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Dim updateFllwupboxstrcd As String = String.Empty
        ' Seqno取得用
        Dim datatableFllwupboxSeqno As New SC3080202DataSet.SC3080202GetFllwupboxNoDataTable
        'Dim datarowFllwupboxSeqno As SC3080202DataSet.SC3080202GetFllwupboxNoRow '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 

        ' NULLでなければ、FollowupboxSeqnoを設定
        If datarowFrom.IsFLLWUPBOX_SEQNONull Then
        Else
            updateFllwupboxseqno = datarowFrom.FLLWUPBOX_SEQNO
            updateFllwupboxstrcd = datarowFrom.STRCD
        End If

        ' 1行目削除
        datatablefrom.RemoveSC3080202UpdateSelectedCompeFromRow(datarowFrom)

        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 Start
        '' 更新対象がある場合
        'If datatablefrom.Rows.Count > 0 Then
        '    ' FollowupboxSeqnoがNULLならば、取得する
        '    If IsNothing(updateFllwupboxseqno) Then
        '        datatableFllwupboxSeqno = SC3080202TableAdapter.GetFllwupboxSeqno()
        '        datarowFllwupboxSeqno = CType(datatableFllwupboxSeqno.Rows(0), SC3080202DataSet.SC3080202GetFllwupboxNoRow)
        '        updateFllwupboxseqno = datarowFllwupboxSeqno.SEQ
        '        updateFllwupboxstrcd = StaffContext.Current.BrnCD
        '    End If
        'End If
        Dim scDlrCD As String = StaffContext.Current.DlrCD
        '$01 2012/01/24 Version1.01 yamaguchi 【A.STEP2】代理商談入力機能開発 End

        ' 競合車種削除
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 START

        Try
            SC3080202TableAdapter.SelectSalesLock(CType(updateFllwupboxseqno, String))
        Catch ex As OracleExceptionEx
            Return Nothing
        End Try
        Dim ret As Integer
        ret = SC3080202TableAdapter.DeleteSelectedCompe(CType(updateFllwupboxseqno, String))

        For Each dt As SC3080202DataSet.SC3080202UpdateSelectedCompeFromRow In datatablefrom
            ' 競合車種追加
            SC3080202TableAdapter.AddSelectedCompe(CType(updateFllwupboxseqno, String),
                                                            CType(dt.SEQNO, String),
                                                            dt.SERIESCD,
                                                            StaffContext.Current.Account)
        Next

        Using datatableTo As New SC3080202DataSet.SC3080202GetSeqnoToDataTable
            If datatableFllwupboxSeqno.Rows.Count > 0 Then
                Dim datarowTo As SC3080202DataSet.SC3080202GetSeqnoToRow
                datarowTo = datatableTo.NewSC3080202GetSeqnoToRow
                datarowTo.FLLWUPBOX_SEQNO = CDec(updateFllwupboxseqno)
                datatableTo.Rows.Add(datarowTo)
            End If
            Return datatableTo
        End Using
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Logger.Info("UpdateSelectedCompe End")
    End Function

    ' 2012/02/29 TCS 小野 【SALES_2】 START
    ''' <summary>
    ''' 成約車種リスト取得
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSuccessSeriesList(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListFromDataTable) As ActivityInfoDataSet.ActivityInfoGetSelectedSeriesListToDataTable
        Return ActivityInfoBusinessLogic.GetSuccessSeriesList(datatableFrom)
    End Function

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START    
    ''' <summary>
    ''' 受注No取得処理
    ''' </summary>
    ''' <param name="datatableFrom">引数DataTable</param>
    ''' <returns>受注No</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSalesbkgno(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoContractNoFromDataTable) As String
        Return ActivityInfoBusinessLogic.GetContractNo(datatableFrom)
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
    End Function

    ''' <summary>
    ''' 受注後、受注前判定フラグ
    ''' </summary>
    ''' <param name="datatableFrom"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CountFllwupboxRslt(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoCountFromDataTable) As String
        Return ActivityInfoBusinessLogic.CountFllwupboxRslt(datatableFrom)
    End Function
    ' 2012/02/29 TCS 小野 【SALES_2】 END

    '2013/06/30 TCS 徐 2013/10対応版　既存流用 START
    '2013/03/06 TCS 河原 GL0874 START
    ''' <summary>
    ''' 契約状況フラグの取得
    ''' </summary>
    ''' <param name="datatableFrom"></param>
    ''' <returns>契約状況フラグ</returns>
    ''' <remarks>契約状況フラグの取得</remarks>
    Public Shared Function GetContractFlg(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoContractNoFromDataTable) As String
        '2013/06/30 TCS 徐 2013/10対応版　既存流用 END
        Logger.Info("GetContractFlg Start")

        Logger.Info("GetContractFlg End")

        Return ActivityInfoBusinessLogic.GetContractFlg(datatableFrom)
    End Function
    '2013/03/06 TCS 河原 GL0874 END

#Region "Aカード情報相互連携開発"
    '2013/12/05 TCS 市川 Aカード情報相互連携開発 START

    ''' <summary>
    ''' 活動きっかけマスタ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>活動きっかけ（用件ソース1st）のマスタを取得します。</remarks>
    Public Shared Function GetSourcesOfACardMaster() As SC3080202DataSet.SC3080202SourcesOfACardMasterDataTable

        Dim contactMtd As String = String.Empty

        '環境設定からコンタクト方法・その他活動結果(手入力活動結果)を取得する。
        contactMtd = GetSysEnvSettingValue(ENVSETTINGKEY_GIVEUP_CONTACT_MTD)

        With StaffContext.Current
            Return SC3080202TableAdapter.GetSourcesOfACardMaster(.DlrCD, .BrnCD, contactMtd)
        End With

    End Function

    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061) 削除  

    ''' <summary>
    ''' 商談情報取得
    ''' </summary>
    ''' <param name="salesId"></param>
    ''' <returns></returns>
    ''' <remarks>商談情報(画面表示用)を取得します。</remarks>
    Public Shared Function GetSalesInfoDetail(ByVal salesId As Decimal) As SC3080202DataSet.SC3080202SalesInfoDetailDataTable
        Return SC3080202TableAdapter.GetSalesInfoDetail(salesId)
    End Function

    ''' <summary>
    ''' 入力チェック設定取得（商談）
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>商談情報に関する入力チェック設定を取得します。</remarks>
    Public Shared Function GetSettingsInputCheckForSalesInfo() As ActivityInfoDataSet.ActivityInfoSettingsInputCheckDataTable
        Return ActivityInfoTableAdapter.GetSettingsInputCheck(INPUT_CHECK_TIMING)
    End Function

    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061) 削除  

    ''' <summary>
    ''' 活動きっかけ更新
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="source1stCd">用件ソース1st</param>
    ''' <returns></returns>
    ''' <remarks>活動きっかけ（用件ソース1st）を更新する。</remarks>
    <EnableCommit()>
    Public Function UpdateSourceOfACard(ByVal salesId As Decimal, ByVal source1stCd As Long) As Boolean

        Logger.Info("UpdateSourceOfACard_Start")

        Dim dt As SC3080202DataSet.SC3080202RequestIDAttractIDBySalesIDDataTable = Nothing
        Dim isTemp As Boolean = True
        Dim isReq As Boolean = True
        Dim ret As Boolean = False

        '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061) 追加 start
        Dim retlocal As Boolean = False
        '用件ソース1を更新する際は今後、必ず商談ローカルを使用するので、なければレコード作成する
        retlocal = CheckLocalAndInsert(salesId)
        '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061) 追加 end

        Try
            dt = SC3080202TableAdapter.GetRequestIDAttractIDBySalesID(salesId)

            '商談・用件・誘致テーブル存在確認
            isTemp = (Not dt Is Nothing AndAlso dt.Rows.Count = 0)
            isReq = (Not isTemp AndAlso Not dt(0).IsREQ_IDNull AndAlso dt(0).REQ_ID > 0)
            If Not isTemp AndAlso Not isReq AndAlso (dt(0).IsATT_IDNull OrElse dt(0).ATT_ID = 0) Then
                Throw New Exception(String.Format("Row of TB_T_REQUEST and TB_T_ATTRACT is not found with parameter [SALES_ID]={0}", salesId.ToString()))
            End If

            '行ロック
            If isTemp Then
                If 1 <> SC3080202TableAdapter.LockSalesTemp(salesId) Then
                    Throw New Exception(String.Format("Row of TB_T_SALES_TEMP is not found with parameter [SALES_ID]={0}", salesId.ToString()))
                End If
            ElseIf isReq Then
                If 1 <> SC3080202TableAdapter.LockRequest(dt(0).REQ_ID) Then
                    Throw New Exception(String.Format("Row of TB_T_REQUEST is not found with parameter [REQ_ID]={0}", dt(0).REQ_ID.ToString()))
                End If
            Else
                If 1 <> SC3080202TableAdapter.LockAttract(dt(0).ATT_ID) Then
                    Throw New Exception(String.Format("Row of TB_T_ATTRACT is not found with parameter [ATT_ID]={0}", dt(0).ATT_ID.ToString()))
                End If
            End If

            'ブランド認知理由を更新(更新行は1行のみとする)
            With StaffContext.Current
                If isTemp Then
                    ret = (1 = SC3080202TableAdapter.UpdateSourceOfACardTemp(salesId, source1stCd, .Account))
                ElseIf isReq Then
                    ret = (1 = SC3080202TableAdapter.UpdateSourceOfACardRequesst(dt(0).REQ_ID, source1stCd, .Account))
                Else
                    ret = (1 = SC3080202TableAdapter.UpdateSourceOfACardAttract(dt(0).ATT_ID, source1stCd, .Account))
                End If
            End With

            Me.Rollback = (Not ret)
        Finally
            If Not dt Is Nothing Then dt.Dispose()
        End Try

        Logger.Info("UpdateSourceOfACard_End")

        Return ret

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sysEnvName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Shared Function GetSysEnvSettingValue(ByVal sysEnvName As String) As String
        Dim dr As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow = Nothing
        Dim env As SystemEnvSetting = Nothing
        Try
            env = New SystemEnvSetting()
            dr = env.GetSystemEnvSetting(sysEnvName)
            If Not dr Is Nothing Then
                Return dr.PARAMVALUE.Trim()
            End If
        Catch ex As Exception
            Logger.Error("", ex)
        Finally
            env = Nothing
        End Try

        Return String.Empty
    End Function
    '2013/12/05 TCS 市川 Aカード情報相互連携開発 END
#End Region

    '2014/02/12 TCS 山口 受注後フォロー機能開発 START
#Region "受注後フォロー機能開発"
    ''' <summary>
    ''' 受注時説明登録確認
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>件数</returns>
    ''' <remarks></remarks>
    Public Shared Function IsOrderExplanation(ByVal salesId As Decimal) As Integer
        Return SC3080202TableAdapter.IsOrderExplanation(salesId)
    End Function

    ''' <summary>
    ''' 契約車両情報取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>契約車両情報</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractCarData(ByVal salesId As Decimal) As SC3080202DataSet.SC3080202GetContractCarDataTable
        Logger.Info("GetContractCarData_Start")

        '契約車両情報取得
        Dim dtContractCarData As SC3080202DataSet.SC3080202GetContractCarDataTable = _
                                SC3080202TableAdapter.GetContractCarData(salesId)

        '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
        Dim DispFlgActStatus = GetDispFlgActStatus()
        '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END

        '契約車両情報の件数が0件の場合処理しない
        For Each drContractCarData In dtContractCarData
            'VIN No.取得
            drContractCarData.ASSIGN_TEMP_VCL_VIN = String.Empty
            If Not drContractCarData.IsDLRCDNull And Not drContractCarData.IsCONTRACTNONull Then
                '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
                Dim vin As SC3080202DataSet.SC3080202GetContractCarVinDataTable = _
                    SC3080202TableAdapter.GetContractCarDataVin(drContractCarData.DLRCD, drContractCarData.CONTRACTNO, DispFlgActStatus)
                '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END
                For Each vinRow In vin
                    If Not vinRow.IsASSIGN_TEMP_VCL_VINNull Then
                        drContractCarData.ASSIGN_TEMP_VCL_VIN = vinRow.ASSIGN_TEMP_VCL_VIN
                    End If
                Next
            End If
            '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
            If DispFlgActStatus.Equals(ACT_STATUS_DISP_FLG_ON) Then

                '車両ステイタス取得
                drContractCarData.AFTER_ODR_ACT_STATUS_NAME = Nothing

                '到着予定日取得
                drContractCarData.SCHE_START_DATEORTIME = GetBookedAfterDetailInfoDate(salesId, ENVSETTINGKEY_AFTER_ODR_ARRIVAL, AfterOdrDate.ScheStartDateOrTime)
            End If

            '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL
            '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END
            '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END
            If Not drContractCarData.IsCONTRACTDATENull AndAlso Not String.IsNullOrWhiteSpace(drContractCarData.CONTRACTDATE) Then
                '日付フォーマット変換
                drContractCarData.CONTRACTDATE = GetDateToString(drContractCarData.CONTRACTDATE, "1")
            End If
        Next

        Logger.Info("GetContractCarData_Start")

        Return dtContractCarData
    End Function

    ''' <summary>
    ''' 受注後工程詳細情報取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>受注後工程詳細情報</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBookedAfterDetailInfo(ByVal salesId As Decimal) As SC3080202DataSet.SC3080202GetBookedAfterDetailInfoDataTable
        Logger.Info("GetBookedAfterDetailInfo_Start")

        Using dtBookedAfterDetailInfo As New SC3080202DataSet.SC3080202GetBookedAfterDetailInfoDataTable
            Dim drBookedAfterDetailInfo As SC3080202DataSet.SC3080202GetBookedAfterDetailInfoRow = _
                                        CType(dtBookedAfterDetailInfo.NewRow, SC3080202GetBookedAfterDetailInfoRow)

            '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
            If GetDispFlgActStatus().Equals(ACT_STATUS_DISP_FLG_ON) Then
                'ファイナンスステイタス
                drBookedAfterDetailInfo.AFTER_ODR_FINANCE = GetBookedAfterDetailInfoStatus(salesId, ENVSETTINGKEY_AFTER_ODR_FINANCE)
                'ファイナンス申請日
                drBookedAfterDetailInfo.AFTER_ODR_FINANCE_APPLICATION = GetBookedAfterDetailInfoDate(salesId, ENVSETTINGKEY_AFTER_ODR_FINANCE_APPLICATION, AfterOdrDate.RsltEndDateOrTime)
                'ファイナンス承認日
                drBookedAfterDetailInfo.AFTER_ODR_FINANCE_APPROVAL = GetBookedAfterDetailInfoDate(salesId, ENVSETTINGKEY_AFTER_ODR_FINANCE_APPROVAL, AfterOdrDate.RsltEndDateOrTime)
                'マッチングステイタス
                drBookedAfterDetailInfo.AFTER_ODR_MATCHING = GetBookedAfterDetailInfoStatus(salesId, ENVSETTINGKEY_AFTER_ODR_MATCHING)
            End If
            '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL
            '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END
            '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END

            '振当て日
            drBookedAfterDetailInfo.AFTER_ODR_ASSIGN = GetBookedAfterDetailInfoDate(salesId, ENVSETTINGKEY_AFTER_ODR_ASSIGN, AfterOdrDate.RsltEndDateOrTime)
            '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
            If GetDispFlgActStatus().Equals(ACT_STATUS_DISP_FLG_ON) Then
                'VDQIステイタス
                drBookedAfterDetailInfo.AFTER_ODR_VDQI = GetBookedAfterDetailInfoVDQIStatus(salesId, ENVSETTINGKEY_AFTER_ODR_VDQI1, ENVSETTINGKEY_AFTER_ODR_VDQI2)
                'VDQIリクエスト日
                drBookedAfterDetailInfo.AFTER_ODR_VDQI_REQUEST = GetBookedAfterDetailInfoDate(salesId, ENVSETTINGKEY_AFTER_ODR_VDQI_REQUEST, AfterOdrDate.RsltEndDateOrTime)
                'VDQI開始日
                drBookedAfterDetailInfo.AFTER_ODR_VDQI_START = GetBookedAfterDetailInfoDate(salesId, ENVSETTINGKEY_AFTER_ODR_VDQI_START, AfterOdrDate.RsltEndDateOrTime)
                'VDQI完了日
                drBookedAfterDetailInfo.AFTER_ODR_VDQI_COMPLETE = GetBookedAfterDetailInfoDate(salesId, ENVSETTINGKEY_AFTER_ODR_VDQI_COMPLETE, AfterOdrDate.RsltEndDateOrTime)
                'PDS実施日
                drBookedAfterDetailInfo.AFTER_ODR_PDS_IMPLEMENT = GetBookedAfterDetailInfoDate(salesId, ENVSETTINGKEY_AFTER_ODR_PDS_IMPLEMENT, AfterOdrDate.RsltEndDateOrTime)
                '保険登録日
                drBookedAfterDetailInfo.AFTER_ODR_INSURANCE_REGISTRATION = GetBookedAfterDetailInfoDate(salesId, ENVSETTINGKEY_AFTER_ODR_INSURANCE_REGISTRATION, AfterOdrDate.RsltEndDateOrTime)
            End If
            '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL
            '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END
            '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END
            '納車日時
            drBookedAfterDetailInfo.AFTER_ODR_DELIVERY = GetBookedAfterDetailInfoDate(salesId, ENVSETTINGKEY_AFTER_ODR_DELIVERY, AfterOdrDate.RsltEndDateOrTime)
            '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
            If GetDispFlgActStatus().Equals(ACT_STATUS_DISP_FLG_ON) Then
                '登録ステイタス
                drBookedAfterDetailInfo.AFTER_ODR_REGISTRATION = GetBookedAfterDetailInfoRegistStatus(salesId, ENVSETTINGKEY_AFTER_ODR_REGISTRATION1, ENVSETTINGKEY_AFTER_ODR_REGISTRATION2, ENVSETTINGKEY_AFTER_ODR_REGISTRATION3)
                '登録申請日
                drBookedAfterDetailInfo.AFTER_ODR_REGISTRATION_APPLICATION = GetBookedAfterDetailInfoDate(salesId, ENVSETTINGKEY_AFTER_ODR_REGISTRATION_APPLICATION, AfterOdrDate.RsltEndDateOrTime)
                'ナンバー取得日
                drBookedAfterDetailInfo.AFTER_ODR_NUMBER_ACQUIRE = GetBookedAfterDetailInfoDate(salesId, ENVSETTINGKEY_AFTER_ODR_NUMBER_ACQUIRE, AfterOdrDate.RsltEndDateOrTime)
                'ナンバー引き渡し日
                drBookedAfterDetailInfo.AFTER_ODR_NUMBER_HANDING = GetBookedAfterDetailInfoDate(salesId, ENVSETTINGKEY_AFTER_ODR_NUMBER_HANDING, AfterOdrDate.RsltEndDateOrTime)
            End If
            '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL
            '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END
            '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END

            dtBookedAfterDetailInfo.AddSC3080202GetBookedAfterDetailInfoRow(drBookedAfterDetailInfo)

            Logger.Info("GetBookedAfterDetailInfo_End")

            Return dtBookedAfterDetailInfo
        End Using
    End Function

    ''' <summary>
    ''' 受注後工程詳細情報日付取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="envSettingKey">受注後活動コードに紐付くシステム設定のキー</param>
    ''' <param name="afterOdrDate">取得日付区分</param>
    ''' <returns>受注後工程詳細情報日付</returns>
    ''' <remarks></remarks>
    Private Shared Function GetBookedAfterDetailInfoDate(ByVal salesId As Decimal, _
                                                        ByVal envSettingKey As String, _
                                                        ByVal afterOdrDate As AfterOdrDate) As String
        Logger.Info("GetBookedAfterDetailInfoDate_Start")

        Dim dtBookedAfterDetailInfoDate As SC3080202DataSet.SC3080202GetBookedAfterDetailInfoDateDataTable = _
                                    SC3080202TableAdapter.GetBookedAfterDetailInfoDate(salesId, GetSysEnvSettingValue(envSettingKey), afterOdrDate)
        Dim ret As String = String.Empty

        For Each drBookedAfterDetailInfoDate In dtBookedAfterDetailInfoDate
            If Not drBookedAfterDetailInfoDate.IsSTART_END_DATETIMENull Then
                '日付フォーマット変換
                ret = GetDateToString(drBookedAfterDetailInfoDate.START_END_DATETIME, drBookedAfterDetailInfoDate.DATEORTIME_FLG)
            End If
        Next

        Logger.Info("GetBookedAfterDetailInfoDate_End")

        Return ret
    End Function

    '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
    ''' <summary>
    ''' 受注後工程詳細情報ステイタス取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="envSettingKey">受注後活動コードに紐付くシステム設定のキー</param>
    ''' <returns>受注後工程詳細情報ステイタス</returns>
    ''' <remarks></remarks>
    Private Shared Function GetBookedAfterDetailInfoStatus(ByVal salesId As Decimal, _
                                                          ByVal envSettingKey As String) As String
        Logger.Info("GetBookedAfterDetailInfoStatus_Start")

        Dim dtBookedAfterDetailInfoStatus As SC3080202DataSet.SC3080202GetBookedAfterDetailInfoStatusDataTable = _
                                    SC3080202TableAdapter.GetBookedAfterDetailInfoStatus(salesId, GetSysEnvSettingValue(envSettingKey))
        Dim ret As String = String.Empty

        For Each drBookedAfterDetailInfoStatus In dtBookedAfterDetailInfoStatus
            If Not drBookedAfterDetailInfoStatus.IsAFTER_ODR_ACT_STATUS_NAMENull Then
                ret = drBookedAfterDetailInfoStatus.AFTER_ODR_ACT_STATUS_NAME
            End If
        Next

        Logger.Info("GetBookedAfterDetailInfoStatus_End")

        Return ret
    End Function

    ''' <summary>
    ''' 受注後工程詳細情報VDQIステイタス取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="envSettingKey1">受注後活動コードに紐付くシステム設定のキー</param>
    ''' <param name="envSettingKey2">受注後活動コードに紐付くシステム設定のキー</param>
    ''' <returns>受注後工程詳細情報ステイタス</returns>
    ''' <remarks></remarks>
    Private Shared Function GetBookedAfterDetailInfoVDQIStatus(ByVal salesId As Decimal, _
                                                               ByVal envSettingKey1 As String, _
                                                               ByVal envSettingKey2 As String) As String
        Logger.Info("GetBookedAfterDetailInfoStatus_Start")

        Dim dtBookedAfterDetailInfoStatus As SC3080202DataSet.SC3080202GetBookedAfterDetailInfoStatusDataTable = _
                                    SC3080202TableAdapter.GetBookedAfterDetailInfoVDQIStatus(salesId, GetSysEnvSettingValue(envSettingKey1), GetSysEnvSettingValue(envSettingKey2))
        Dim ret As String = String.Empty

        For Each drBookedAfterDetailInfoStatus In dtBookedAfterDetailInfoStatus
            If Not drBookedAfterDetailInfoStatus.IsAFTER_ODR_ACT_STATUS_NAMENull Then
                ret = drBookedAfterDetailInfoStatus.AFTER_ODR_ACT_STATUS_NAME
            End If
        Next

        Logger.Info("GetBookedAfterDetailInfoStatus_End")

        Return ret
    End Function

    ''' <summary>
    ''' 受注後工程詳細情報登録ステイタス取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="envSettingKey1">受注後活動コードに紐付くシステム設定のキー</param>
    ''' <param name="envSettingKey2">受注後活動コードに紐付くシステム設定のキー</param>
    ''' <param name="envSettingKey3">受注後活動コードに紐付くシステム設定のキー</param>
    ''' <returns>受注後工程詳細情報ステイタス</returns>
    ''' <remarks></remarks>
    Private Shared Function GetBookedAfterDetailInfoRegistStatus(ByVal salesId As Decimal, _
                                                                 ByVal envSettingKey1 As String, _
                                                                 ByVal envSettingKey2 As String, _
                                                                 ByVal envSettingKey3 As String) As String
        Logger.Info("GetBookedAfterDetailInfoStatus_Start")

        Dim dtBookedAfterDetailInfoStatus As SC3080202DataSet.SC3080202GetBookedAfterDetailInfoStatusDataTable = _
                                    SC3080202TableAdapter.GetBookedAfterDetailInfoRegistStatus(salesId, GetSysEnvSettingValue(envSettingKey1), GetSysEnvSettingValue(envSettingKey2), GetSysEnvSettingValue(envSettingKey3))
        Dim ret As String = String.Empty

        For Each drBookedAfterDetailInfoStatus In dtBookedAfterDetailInfoStatus
            If Not drBookedAfterDetailInfoStatus.IsAFTER_ODR_ACT_STATUS_NAMENull Then
                ret = drBookedAfterDetailInfoStatus.AFTER_ODR_ACT_STATUS_NAME
            End If
        Next

        Logger.Info("GetBookedAfterDetailInfoStatus_End")

        Return ret
    End Function
    '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 START DEL
    '2018/08/07 TCS 中村 （FS）基幹連携を用いたセールス業務実績入力の検証 END
    '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END

    ''' <summary>
    ''' Date形式文字列に対し日付フォーマット変換（DB初期値(1900/01/01 00:00:00)の場合、ブランクを設定）
    ''' </summary>
    ''' <param name="strDate">日付</param>
    ''' <returns>文字列</returns>
    ''' <remarks></remarks>
    Private Shared Function GetDateToString(ByVal strDate As String, ByVal timeSpecify As String) As String
        Logger.Info("GetDateToString_Start")

        Dim ret As String

        If CDate(strDate).Equals(DateTime.Parse("1900/01/01 00:00:00", CultureInfo.InvariantCulture)) Then
            '日付が初期値の場合
            ret = String.Empty
        Else

            Dim timeSpecifyFlg As Boolean
            If String.Equals(timeSpecify, "1") Then
                timeSpecifyFlg = True
            Else
                timeSpecifyFlg = False
            End If

            '日付が初期値以外の場合
            ret = DateTimeFunc.FormatElapsedDate(ElapsedDateFormat.Normal, CDate(strDate), DateTimeFunc.Now(), StaffContext.Current.DlrCD, timeSpecifyFlg)
        End If

        Logger.Info("GetDateToString_End")

        Return ret
    End Function
#End Region
    '2014/02/12 TCS 山口 受注後フォロー機能開発 END

    '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 START
#Region "受注後工程蓋閉め"
    ''' <summary>
    ''' 受注後工程利用フラグ取得
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="brncd">店舗コード</param>
    ''' <returns>受注後工程利用フラグ(0:利用しない、1:利用する)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetAfterOdrProcFlg(ByVal dlrcd As String, ByVal brncd As String) As String
        Logger.Info("GetAfterOdrProcFlg Start")

        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        '①販売店≠'XXXXX'、店舗≠'XXX'（販売店コード・店舗コード該当）
        '②①実行でデータがなければ販売店≠'XXXXX'、店舗＝'XXX'販売店（販売店コードのみ該当）
        '③①②実行でデータがなければ販売店＝'XXXXX'、店舗＝'XXX'（販売店コード・店舗コードいずれも該当なし(デフォルト値)  
        Dim afterOrderProcessFlg As String
        Dim systemBiz As New SystemSettingDlr
        Dim drSettingDlr As SystemSettingDlrDataSet.TB_M_SYSTEM_SETTING_DLRRow = systemBiz.GetEnvSetting(dlrcd, brncd, C_USE_AFTER_ODR_PROC_FLG)

        'データそのものが取れなかった場合、取得した列に値が設定されていない場合はエラー
        If drSettingDlr Is Nothing Then
            Return Nothing  
        End If

        afterOrderProcessFlg = drSettingDlr.SETTING_VAL
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

        Logger.Info("GetAfterOdrProcFlg End")

        Return afterOrderProcessFlg
    End Function
#End Region
    '2015/12/08 TCS 中村 MOD (ﾄﾗｲ店ｼｽﾃﾑ評価)新車ﾀﾌﾞﾚｯﾄ(受注後)の管理機能開発 END

    '2016/09/12 TCS 鈴木 ADD 性能改善（TR-SLT-TMT-20160726-002） START
#Region "性能改善"
    ''' <summary>
    ''' 商談取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSales(ByVal salesId As Decimal) As SC3080202DataSet.SC3080202GetSalesDataTable
        Logger.Info("GetSales Start")

        Dim dtSales As SC3080202DataSet.SC3080202GetSalesDataTable = Nothing

        '商談存在確認
        dtSales = SC3080202TableAdapter.GetSales(salesId)

        Logger.Info("GetSales End")
        Return dtSales
    End Function
#End Region
    '2016/09/12 TCS 鈴木 ADD 性能改善（TR-SLT-TMT-20160726-002） END

    '2017/11/20 TCS 河原 TKM独自機能開発 START
#Region "TKM独自機能開発"
    ''' <summary>
    ''' 直販フラグ更新
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="directBillingFlg">直販フラグ</param>
    ''' <returns>処理結果(True：正常終了/False：異常終了)</returns>
    ''' <remarks>直販フラグを更新する</remarks>
    <EnableCommit()>
    Public Function UpdateDirectBilling(ByVal salesId As Decimal, ByVal directBillingFlg As String) As Boolean

        Logger.Info("UpdateDirectBilling_Start")

        Dim dt As SC3080202DataSet.SC3080202GetSalesDataTable = Nothing
        Dim dr As SC3080202DataSet.SC3080202GetSalesRow = Nothing
        Dim isTemp As Boolean = False
        Dim ret As Boolean = False

        Try
            dt = SC3080202TableAdapter.GetSales(salesId)

            '商談テーブル存在確認
            If Not dt Is Nothing AndAlso dt.Rows.Count = 0 Then
                '商談一時にある場合
                isTemp = True
            End If

            '行ロック
            If isTemp Then
                If 1 <> SC3080202TableAdapter.LockSalesTemp(salesId) Then
                    Throw New Exception(String.Format("Row of TB_T_SALES_TEMP is not found with parameter [SALES_ID]={0}", salesId.ToString()))
                End If
            Else
                SC3080202TableAdapter.SelectSalesLock(salesId.ToString())
            End If

            '直販フラグをを更新
            With StaffContext.Current
                If isTemp Then
                    ret = (1 = SC3080202TableAdapter.UpdateDirectBilling_Temp(salesId, directBillingFlg, .Account))
                Else
                    ret = (1 = SC3080202TableAdapter.UpdateDirectBilling_Sales(salesId, directBillingFlg, .Account))
                End If
            End With

            Me.Rollback = (Not ret)
        Finally
            If Not dt Is Nothing Then dt.Dispose()
        End Try

        Logger.Info("UpdateDirectBilling_End")

        Return ret

    End Function
#End Region
    '2017/11/20 TCS 河原 TKM独自機能開発 END

    '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 START
#Region "（トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証"

    ''' <summary>
    ''' サフィックス使用可否フラグ取得
    ''' </summary>
    ''' <returns>サフィックス使用可否フラグ("0"：使用しない / "1"：使用する)</returns>
    ''' <remarks>サフィックス使用可否フラグを取得する</remarks>
    Public Shared Function GetUseFlgSuffix() As String
        Logger.Info("GetUseFlgSuffix Start")

        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        'サフィックス使用可否フラグ(設定値が無ければ0)
        Dim useFlgSuffix As String
        Dim systemBiz As New SystemSetting
        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRow = systemBiz.GetSystemSetting(SETTING_NAME_USE_FLG_SUFFIX)

        If IsNothing(dataRow) Then
            useFlgSuffix = String.Empty
        Else
            useFlgSuffix = dataRow.SETTING_VAL
        End If

        Logger.Info("GetUseFlgSuffix End")
        Return useFlgSuffix
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END
    End Function

    ''' <summary>
    ''' 内装色使用可否フラグ取得
    ''' </summary>
    ''' <returns>内装色使用可否フラグ("0"：使用しない / "1"：使用する)</returns>
    ''' <remarks>内装色使用可否フラグを取得する</remarks>
    Public Shared Function GetUseFlgInteriorColor() As String
        Logger.Info("GetUseFlgInteriorColor Start")

        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        '内装色使用可否フラグ(設定値が無ければ0)
        Dim systemBiz As New SystemSetting
        Dim useFlgInteriorClr As String
        Dim dataRowclr As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRowclr = systemBiz.GetSystemSetting(SETTING_NAME_USE_FLG_INTERIOR)

        If IsNothing(dataRowclr) Then
            useFlgInteriorClr = String.Empty
        Else
            useFlgInteriorClr = dataRowclr.SETTING_VAL
        End If

        Logger.Info("GetUseFlgInteriorColor End")
        Return useFlgInteriorClr
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END
    End Function


#End Region
    '2018/04/17 TCS 前田 （トライ店システム評価）基幹連携を用いたセールス業務実績入力の検証 END

#Region "TKMローカル"
    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 START
    Private Const SETTING_NAME_MODELYEAR_MIN As String = "L_MIN_MODEL_YEAR"

    ''' <summary>
    ''' 購入分類マスタローカル取得
    ''' </summary>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDemandStructure() As SC3080202DataSet.SC3080202GetDemandStructureLocalDataTable
        Logger.Info("GetDemandStructure Start")

        Dim dtDmdStrLc As SC3080202DataSet.SC3080202GetDemandStructureLocalDataTable

        '購入分類マスタローカル取得
        dtDmdStrLc = SC3080202TableAdapter.GetDemandStructureLocal()

        Logger.Info("GetDemandStructure End")
        Return dtDmdStrLc
    End Function

    ''' <summary>
    ''' 下取り車両メーカーマスタローカル取得
    ''' </summary>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetTradeincarMaker() As SC3080202DataSet.SC3080202GetTradeincarMakerLocalDataTable
        Logger.Info("GetTradeincarMaker Start")

        Dim dtTicMakerLc As SC3080202DataSet.SC3080202GetTradeincarMakerLocalDataTable

        '下取り車両メーカーマスタローカル取得
        dtTicMakerLc = SC3080202TableAdapter.GetTradeincarMakerLocal()

        Logger.Info("GetTradeincarMaker End")
        Return dtTicMakerLc
    End Function

    ''' <summary>
    ''' 下取り車両モデルマスタローカル取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetTradeincarModel(ByVal tradeincar_maker_cd As String) As SC3080202DataSet.SC3080202GetTradeincarModelLocalDataTable
        Logger.Info("GetTradeincarModel Start")

        Dim dtTicModelLc As SC3080202DataSet.SC3080202GetTradeincarModelLocalDataTable

        '下取り車両モデルマスタローカル取得
        dtTicModelLc = SC3080202TableAdapter.GetTradeincarModelLocal(tradeincar_maker_cd)

        Logger.Info("GetTradeincarModel End")
        Return dtTicModelLc
    End Function

    ''' <summary>
    ''' 下取り車両年式取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetTradeincarModelYear() As SC3080202DataSet.SC3080202GetModelYearDataTable
        Logger.Info("GetModelYear Start")

        ' 変数
        Dim modelYearMin As Integer   '下取り車両年式の下限値
        Dim modelYearMax As Integer   '下取り車両年式の上限値
        Dim settingData As SC3080202DataSet.SC3080202GetSystemSettingDataTable = Nothing

        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        '下取り車両年式の下限値を取得(無ければ0）
        Dim systemBiz As New SystemSetting
        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRow = systemBiz.GetSystemSetting(SETTING_NAME_MODELYEAR_MIN)

        If IsNothing(dataRow) Then
            modelYearMin = 0
        Else
            Integer.TryParse(dataRow.SETTING_VAL, modelYearMin)
        End If
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

        '下取り車両年式の上限値を取得
        modelYearMax = Now().Year

        Using dtModelYear As New SC3080202DataSet.SC3080202GetModelYearDataTable
            '下限値・上限値ともに有効値を取得できた場合
            If modelYearMin > 0 AndAlso modelYearMax > modelYearMin Then
                '下限値から上限値までの値を文字列に変換して返却値にセット
                For i As Integer = modelYearMax To modelYearMin Step -1
                    Dim dtModelYearRow As SC3080202DataSet.SC3080202GetModelYearRow = dtModelYear.NewSC3080202GetModelYearRow()
                    dtModelYearRow.MODEL_YEAR = i.ToString()
                    dtModelYear.Rows.Add(dtModelYearRow)
                Next
            End If
            Return dtModelYear
        End Using

        Logger.Info("GetModelYear End")
    End Function

    ''' <summary>
    ''' 商談ローカル取得
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <returns>取得結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSalesLocal(ByVal salesId As Decimal) As SC3080202DataSet.SC3080202GetSalesLocalDataTable
        Logger.Info("GetSalesLocal Start")

        Dim dtSalesLc As SC3080202DataSet.SC3080202GetSalesLocalDataTable

        dtSalesLc = SC3080202TableAdapter.GetSalesLocal(salesId)

        Logger.Info("GetSalesLocal End")
        Return dtSalesLc
    End Function

    ''' <summary>
    ''' 購入分類/下取車両情報更新
    ''' </summary>
    ''' <param name="datatableFrom"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateSalesLocal(ByVal datatableFrom As SC3080202DataSet.SC3080202GetSalesLocalDataTable) As Boolean

        Dim result As Boolean = False
        Dim dt As SC3080202DataSet.SC3080202GetSalesLocalDataTable
        Dim isLocalSales As Boolean

        Dim datatableFromRow As SC3080202DataSet.SC3080202GetSalesLocalRow

        datatableFromRow = CType(datatableFrom.Rows(0), SC3080202DataSet.SC3080202GetSalesLocalRow)

        Dim salesId As Decimal = datatableFromRow.SALES_ID
        Dim DemandStructureCd As String = datatableFromRow.DEMAND_STRUCTURE_CD
        Dim Trade_in_Maker As String = datatableFromRow.TRADEINCAR_MAKER_CD
        Dim Trade_in_Model As String = datatableFromRow.TRADEINCAR_MODEL_CD
        Dim Trade_in_Mileage As Double = datatableFromRow.TRADEINCAR_MILE
        Dim Trade_in_ModelYear As String = datatableFromRow.TRADEINCAR_MODEL_YEAR
        Dim lockvr As Long = datatableFromRow.ROW_LOCK_VERSION

        dt = GetSalesLocal(salesId)

        If Not dt Is Nothing AndAlso dt.Rows.Count = 0 Then
            isLocalSales = False
        Else
            isLocalSales = True
        End If

        Dim rt As Integer

        If isLocalSales Then
            '更新
            SC3080202TableAdapter.SelectSalesLocalLock(salesId)
            rt = SC3080202TableAdapter.UpdateSalesLocal(salesId, DemandStructureCd, Trade_in_Maker, Trade_in_Model, Trade_in_Mileage, Trade_in_ModelYear, StaffContext.Current.Account, DisplayId, lockvr)

            If rt = 1 Then
                result = True
            End If
        Else
            '新規
            rt = SC3080202TableAdapter.AddSalesLocal(salesId, DemandStructureCd, Trade_in_Maker, Trade_in_Model, Trade_in_Mileage, Trade_in_ModelYear, StaffContext.Current.Account, DisplayId)

            If rt = 1 Then
                result = True
            End If
        End If

        Return result

    End Function
    '2018/07/10 TCS 河原 TKM Next Gen e-CRB Project Application development Block B-1 END
    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061) 追加 start
    ''' <summary>
    ''' 活動きっかけ２マスタ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>活動きっかけ（用件ソース2nd）のマスタを取得します。</remarks>
    Public Shared Function GetSources2Master(ByVal source1Cd As Long) As SC3080202DataSet.SC3080202Sources2OfACardMasterDataTable
        Logger.Debug("GetSources2Master_Start")

        With StaffContext.Current
            Return SC3080202TableAdapter.GetSource2Master(.DlrCD, .BrnCD, source1Cd)
        End With
        Logger.Debug("GetSources2Master_End")

    End Function

    ''' <summary>
    ''' 商談ローカル存在確認（未存在時は追加）
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks>商談ローカル存在確認し、未存在時はレコード追加します</remarks>
    <EnableCommit()>
    Public Function CheckLocalAndInsert(ByVal salesId As Decimal) As Boolean
        Logger.Debug("CheckLocalAndInsert_Start")

        '商談ローカルテーブル存在確認
        Dim dtSalesLc As SC3080202DataSet.SC3080202GetSalesLocalDataTable
        Dim ret As Boolean = False
        dtSalesLc = SC3080202TableAdapter.GetSalesLocal(salesId)

        If dtSalesLc Is Nothing OrElse dtSalesLc.Rows.Count = 0 Then
            Try
                '商談ローカルテーブルは必ず使用するため、無ければインサート
                ret = (1 = SC3080202TableAdapter.AddSalesLocalRecord(salesId, StaffContext.Current.Account))

                Me.Rollback = (Not ret)
            Finally
                If Not dtSalesLc Is Nothing Then dtSalesLc.Dispose()
            End Try
        Else
            'レコードが存在する場合はTrueを返す
            ret = True
        End If

        Logger.Debug("CheckLocalAndInsert_End")
        Return ret

    End Function

    ''' <summary>
    ''' 活動きっかけ２更新
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="source2ndCd">用件ソース2nd</param>
    ''' <returns></returns>
    ''' <remarks>活動きっかけ２に関連する項目を更新する。</remarks>
    <EnableCommit()>
    Public Function UpdateSource2(ByVal salesId As Decimal, ByVal source2ndCd As Long, ByRef rowlockversion As Decimal) As Boolean

        Logger.Debug("UpdateSource2_Start")
        Dim isTemp As Boolean = True

        Dim dt As SC3080202DataSet.SC3080202RequestIDAttractIDBySalesIDDataTable = Nothing
        Dim isReq As Boolean = True
        Dim ret As Boolean = False
        Dim dtsales As SC3080202DataSet.SC3080202GetSalesDataTable = Nothing

        '用件ソース1を更新する際は今後、必ず商談ローカルを使用するので、なければレコード作成する()
        Dim retlocal As Boolean = False
        retlocal = CheckLocalAndInsert(salesId)

        If (Not retlocal) Then
            Return retlocal
        End If

        Try
            '用件・誘致テーブル存在確認
            dt = SC3080202TableAdapter.GetRequestIDAttractIDBySalesID(salesId)
            isTemp = (Not dt Is Nothing AndAlso dt.Rows.Count = 0)
            isReq = (Not isTemp AndAlso Not dt(0).IsREQ_IDNull AndAlso dt(0).REQ_ID > 0)
            If Not isTemp AndAlso Not isReq AndAlso (dt(0).IsATT_IDNull OrElse dt(0).ATT_ID = 0) Then
                Throw New Exception(String.Format("Row of TB_T_REQUEST and TB_T_ATTRACT is not found with parameter [SALES_ID]={0}", salesId.ToString()))
            End If

            '行ロック
            If isTemp Then
                '商談ローカル自身をテーブルロック
                SC3080202TableAdapter.SelectSalesLocalLock(salesId)
            ElseIf isReq Then
                If 1 <> SC3080202TableAdapter.LockRequest(dt(0).REQ_ID) Then
                    Throw New Exception(String.Format("Row of TB_T_REQUEST is not found with parameter [REQ_ID]={0}", dt(0).REQ_ID.ToString()))
                End If
            Else
                If 1 <> SC3080202TableAdapter.LockAttract(dt(0).ATT_ID) Then
                    Throw New Exception(String.Format("Row of TB_T_ATTRACT is not found with parameter [ATT_ID]={0}", dt(0).ATT_ID.ToString()))
                End If
            End If

            '活動きっかけ２を更新(更新行は1行のみとする)
            With StaffContext.Current
                If isTemp Then
                    ret = (1 = SC3080202TableAdapter.UpdatedSource2_Local(salesId, source2ndCd, .Account, rowlockversion))
                    If (ret) Then
                        rowlockversion = rowlockversion + 1
                    End If
                ElseIf isReq Then
                    ret = (1 = SC3080202TableAdapter.UpdateSource2_Requesst(dt(0).REQ_ID, source2ndCd, .Account))
                Else
                    ret = (1 = SC3080202TableAdapter.UpdateSource2_Attract(dt(0).ATT_ID, source2ndCd, .Account))
                End If

                If (ret = True And source2ndCd <> 0) Then
                    '0への更新時を除いて、商談ローカルテーブルのソース2編集可能フラグのみを設定する
                    ret = UpdateSourceFlg_Local(salesId, False, True, rowlockversion)
                End If
            End With

            Me.Rollback = (Not ret)
        Finally
            If Not dt Is Nothing Then dt.Dispose()
        End Try

        Logger.Debug("UpdateSource2_End")

        Return ret

    End Function

    ''' <summary>
    ''' 商談ローカルテーブルの更新(フラグのみ）
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="isSource1FlgColumn">ソース２編集フラグの更新かどうか</param>
    ''' <param name="isSource2FlgColumn">ソース２編集フラグの更新かどうか</param>
    ''' <returns></returns>
    ''' <remarks>活動きっかけ編集フラグをONにする処理</remarks>
    Public Function UpdateSourceFlg_Local(ByVal salesId As Decimal, ByVal isSource1FlgColumn As Boolean, ByVal isSource2FlgColumn As Boolean, ByRef rowlockversion As Decimal) As Boolean

        Logger.Debug("UpdateSource2Flg_Local_Start")

        Dim isReq As Boolean = True
        Dim ret As Boolean = False

        '用件ソース1を更新する際は今後、必ず商談ローカルを使用するので、なければレコード作成する()
        Dim retlocal As Boolean = False
        retlocal = CheckLocalAndInsert(salesId)

        If (Not retlocal) Then
            Return retlocal
        End If

        Try
            '親である商談取得テーブルロック
            SC3080202TableAdapter.SelectSalesLocalLock(salesId)

            '商談ローカルテーブルの値を取得する場合に使用する
            Dim SalesLocalDataTable As SC3080202DataSet.SC3080202GetSalesLocalDataTable
            SalesLocalDataTable = SC3080202BusinessLogic.GetSalesLocal(salesId)

            Dim datarowFrom As SC3080202DataSet.SC3080202GetSalesLocalRow
            datarowFrom = CType(SalesLocalDataTable.Rows(0), SC3080202DataSet.SC3080202GetSalesLocalRow)

            '活動きっかけ２編集可能フラグを更新(更新行は1行のみとする)
            With StaffContext.Current
                If (isSource1FlgColumn = True) Then
                    ret = (1 = SC3080202TableAdapter.UpdatedSource1Flg_Local(salesId, .Account, rowlockversion))
                ElseIf (isSource2FlgColumn = True) Then
                    ret = (1 = SC3080202TableAdapter.UpdatedSource2Flg_Local(salesId, .Account, rowlockversion))
                Else
                End If

                If (ret) Then
                    rowlockversion = rowlockversion + 1
                End If
            End With

        Finally
        End Try

        Logger.Debug("UpdateSource2Flg_Local_End")

        Return ret

    End Function

    '2020/01/22 TCS 重松 TKM Change request development for Next Gen e-CRB (CR058,CR061) 追加 end
#End Region
    '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 START
    ''' <summary>
    ''' 活動ステータス表示フラグ取得
    ''' </summary>
    ''' <returns>活動ステータス表示フラグ("0"：表示しない / "1"：表示する)</returns>
    ''' <remarks>活動ステータス表示フラグを取得する</remarks>
    Public Shared Function GetDispFlgActStatus() As String
        Logger.Info("GetDispFlgActStatus Start")

        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 START
        '活動ステータス表示フラグ()
        Dim useFlgActStatus As String

        Dim systemBiz As New SystemSetting
        Dim dataRow As SystemSettingDataSet.TB_M_SYSTEM_SETTINGRow
        dataRow = systemBiz.GetSystemSetting(SETTING_NAME_ACT_STATUS_DISP_FLG)

        If IsNothing(dataRow) Then
            useFlgActStatus = String.Empty
        Else
            useFlgActStatus = dataRow.SETTING_VAL
        End If

        Logger.Info("GetDispFlgActStatus End")
        Return useFlgActStatus
        '2020/01/06 TS 重松 [TMTレスポンススロー] SLT基盤への横展 END

    End Function
    '2019/12/06 TS  三浦 （FS）e-CRBシステム各種作業効率化の品質検証 END

#End Region

End Class
