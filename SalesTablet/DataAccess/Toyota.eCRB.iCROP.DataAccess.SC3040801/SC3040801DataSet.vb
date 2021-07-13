'-------------------------------------------------------------------------
'SC3040801DateSet.vb
'-------------------------------------------------------------------------
'機能：通知履歴
'補足：
'作成：2012/02/3 KN 河原 【servive_1】
'更新：----/--/-- TMEJ Sales Step 2 $01
'更新：2012/11/7  TMEJ t.shimamura サービス入庫追加 $02
'更新：2013/05/24 TMEJ t.shimamura 【A.STEP2】次世代e-CRB新車タブレット　新DB適応に向けた機能開発 $03
'更新：2014/01/14 TMEJ t.shimamura セールスタブレット契約承認機能開発 $04
'更新：2014/03/03 TMEJ y.nakamura 受注後フォロー機能開発 $05
'更新：2014/04/08 TMEJ y.nakamura 納車予定日変更対応 $06
'更新：2014/06/30 TMEJ a.minagawa フォローアップメモ更新対応 $07
'──────────────────────────────────

Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace SC3040801DataSetTableAdapters

    ''' <summary>
    ''' 通知履歴のデータアクセスクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SC3040801TableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' デフォルトコンストラクタ
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            '処理なし
        End Sub

#Region "定数"

        ''' <summary>
        ''' ROWカウント
        ''' </summary>
        Private Const RowNextRowsCount As Integer = 6

        ''' <summary>
        ''' 既読フラグ
        ''' </summary>
        Private Const ReadList As String = "1"

        ''' <summary>
        ''' ステータスキャンセル
        ''' </summary>
        Private Const Status As String = "2"

        ''' <summary>
        ''' 査定
        ''' </summary>
        Private Const Assessment As String = "01"

        ''' <summary>
        ''' 価格相談
        ''' </summary>
        Private Const Consultation As String = "02"

        ''' <summary>
        ''' ヘルプ
        ''' </summary>
        Private Const Help As String = "03"

        ' $01 start step2開発
        ''' <summary>
        ''' 苦情
        ''' </summary>
        Private Const Claim As String = "05"

        ''' <summary>
        ''' CS Survey
        ''' </summary>
        Private Const CSSurvey As String = "06"
        ' $01 end   step2開発

        ''' <summary>
        ''' 来店者実績（商談中)
        ''' </summary>
        Private Const Negotiations As String = "07"

        ' $02 start サービス入庫
        ''' <summary>
        ''' サービス入庫
        ''' </summary>
        Private Const SurviceStore As String = "07"
        ' $02 end サービス入庫

        '$04 start 契約承認機能
        ''' <summary>
        ''' 契約承認依頼
        ''' </summary>
        Private Const ContractApproval As String = "08"

        ''' <summary>
        ''' 注文情報登録・変更
        ''' </summary>
        Private Const OrderInfomation As String = "09"
        '$04 end 契約承認機能

        '$05 start 受注後フォロー機能開発
        ''' <summary>
        ''' 受注後フォロー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AfterOdrFollow As String = "10"
        '$05 end 受注後フォロー機能開発

        ' $06 start 納車予定日変更対応
        ''' <summary>
        ''' 納車予定日変更
        ''' </summary>
        Private Const DeliScheDateChg As String = "11"
        ' $06 end 納車予定日変更対応

        ' $07 start フォローアップメモ更新対応
        ''' <summary>
        ''' フォローアップメモ更新
        ''' </summary>
        Private Const FllwupMemoUpdate As String = "12"
        ' $07 end フォローアップメモ更新対応

        '$04 start 契約承認機能
        ''' <summary>
        ''' ステータス：依頼
        ''' </summary>
        Private Const StatusRequest As String = "1"

        '$04 end 契約承認機能

        ''' <summary>
        ''' 写真URLkey1
        ''' </summary>
        Private Const FilePath As String = "FILE_PATH_STAFFPHOTO"

        ''' <summary>
        ''' 写真URLkey2
        ''' </summary>
        Private Const FileUrl As String = "URI_STAFFPHOTO"

        ''' <summary>
        ''' 表示日付設定key
        ''' </summary>
        Private Const DisplayDayKey As String = "NOTICE_DISP_DAYS"

        '$05 start 受注後フォロー機能開発
#Region "受注後活動アイコンパス取得"

        ''' <summary>
        ''' 販売店コード(XXXXX)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DealerCodeX As String = "XXXXX"

        ''' <summary>
        ''' 区分種別コード(受注後活動コード)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const TypeCdAfterOdrActCd As String = "AFTER_ODR_ACT_CD"

        ''' <summary>
        ''' デバイス/機能区分(セールスタブレット)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const DeviceTypeSalesTablet As String = "01"

        ''' <summary>
        ''' 2NDキー
        ''' </summary>
        ''' <remarks></remarks>
        Private Const SecondKey As String = "00"

#End Region
        '$05 end 受注後フォロー機能開発

#End Region

#Region "メソッド"
        ' $04 start リーダフラグ対応
        ''' <summary>
        ''' セールス通知履歴を取得
        ''' </summary>
        ''' <param name="userAccount">ログインユーザーアカウント</param>
        ''' <param name="beginRowIndex">リピータのスタート行</param>
        ''' <param name="displayDays">表示日数</param>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="storeCode">店舗コード</param>
        ''' <param name="leaderFlag">リーダフラグ</param>
        ''' <returns>GetSalesNoticeDataTable</returns>
        ''' <remarks></remarks>
        Public Function GetSalesNotice(ByVal userAccount As String, _
                                              ByVal beginRowIndex As Integer, _
                                              ByVal displayDays As Date, _
                                              ByVal dealerCode As String, _
                                              ByVal storeCode As String, _
                                              ByVal leaderFlag As Boolean) _
                                          As SC3040801DataSet.GetSalesNoticeDataTable
            ' $04 end リーダフラグ対応

            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_userAccount=" & _
                        userAccount & _
                        "_beginRowIndex=" & _
                        CStr(beginRowIndex) & _
                        "_DispDays=" & _
                        CStr(displayDays))

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetSalesNoticeDataTable)("SC3040801_001")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    ' $03 start
                    .Append(" SELECT  /* SC3040801_001 */ ")
                    .Append("       A.NOTICEREQID  ")
                    .Append("     , A.NOTICEREQCTG  ")
                    .Append("     , A.CRCUSTID  ")
                    .Append("     , NVL(A.CUSTOMNAME,'NONAME') AS CUSTOMNAME ")
                    .Append("     , A.FLLWUPBOX  ")
                    .Append("     , F.NOTICEID  ")
                    .Append("     , F.FROMACCOUNT  ")
                    .Append("     , F.TOACCOUNT ")
                    .Append("     , F.FROMACCOUNTNAME  ")
                    .Append("     , F.TOACCOUNTNAME  ")
                    .Append("     , F.READFLG  ")
                    .Append("     , TO_CHAR(F.SENDDATE,'yyyy/mm/dd hh24:mi') AS SENDDATE  ")
                    .Append("     , F.STATUS  ")
                    .Append("     , C.SALES_ID AS FLLWUPBOX_SEQNO ")
                    .Append("     , D.ORG_IMGFILE  ")
                    .Append("     , NVL(E.NOTICEMSG_DLR,NVL(E.NOTICEMSG_ENG,'NOMESSAGE')) AS NOTICEMSG_DLR  ")
                    .Append("     , NVL(A.CSPAPERNAME,'NOPAPER') AS CSPAPERNAME  ")
                    '$05 start 受注後フォロー機能開発
                    .Append("     , A.AFTER_ODR_ACT_CD  ")
                    '$05 end 受注後フォロー機能開発
                    .Append("    FROM  ")
                    .Append("        TBL_NOTICEREQUEST A  ")
                    .Append("        ,( SELECT  ")
                    .Append("                   MAX(Z.NOTICEID) AS NOTICEID  ")
                    .Append("             FROM  TBL_NOTICEINFO Z ")
                    .Append("            WHERE  Z.TOACCOUNT = :TOACCOUNT  ")
                    .Append("              AND  Z.SENDDATE >= TRUNC(:SENDDATE) ")
                    .Append("         GROUP BY  Z.NOTICEREQID  ")
                    .Append("                 , Z.STATUS  ")
                    .Append("                 , Z.FROMACCOUNT  ")
                    .Append("                 , Z.TOACCOUNT ) B  ")
                    .Append("       , TB_T_SALES C ")
                    .Append("       , TBL_NOTICEINFO F ")
                    .Append("       , TBL_USERS D ")
                    .Append("       , TBL_NOTICECTGMST E ")
                    .Append("   WHERE B.NOTICEID = F.NOTICEID  ")
                    .Append("     AND A.NOTICEREQID = F.NOTICEREQID  ")
                    .Append("     AND A.STATUS = F.STATUS  ")
                    .Append("     AND A.FLLWUPBOX = C.SALES_ID(+)  ")
                    .Append("     AND A.NOTICEREQCTG = E.NOTICEREQCTG  ")
                    .Append("     AND F.STATUS = E.NOTICESTATUSID ")
                    .Append("     AND F.FROMACCOUNT = D.ACCOUNT(+) ")
                    .Append("     AND A.NOTICEREQCTG IN(:ASSESSMENT,:CONSULTATION,:HELP,:CLAIM,:CSSURVEY,:SURVICESTORE,:CONTRACTAPPROVAL,:ORDERINFOMATION,:AFTERODRFOLLOW,:DELISCHEDATECHG,:FLLWUPMEMOUPDATE) ")
                    .Append("     AND F.TOACCOUNT = :TOACCOUNT ")
                    .Append("     AND C.DLR_CD(+) = :DLRCD ")
                    .Append("     AND C.BRN_CD(+) = :STRCD ")
                    .Append("     AND F.SENDDATE >= TRUNC(:SENDDATE) ")
                    'スタート行が１行目以外は既読のみ読み取る
                    If beginRowIndex <> RowNextRowsCount Then
                        Logger.Info(" WHERE.APPEND=(F.READFLG = :READFLG)")
                        .Append("     AND F.READFLG = :READFLG ")
                    End If

                    If leaderFlag Then
                        .Append(" MINUS ")
                        .Append("   SELECT  ")
                        .Append("       AA.NOTICEREQID  ")
                        .Append("     , AA.NOTICEREQCTG  ")
                        .Append("     , AA.CRCUSTID  ")
                        .Append("     , NVL(AA.CUSTOMNAME,'NONAME') AS CUSTOMNAME ")
                        .Append("     , AA.FLLWUPBOX  ")
                        .Append("     , FF.NOTICEID  ")
                        .Append("     , FF.FROMACCOUNT  ")
                        .Append("     , FF.TOACCOUNT ")
                        .Append("     , FF.FROMACCOUNTNAME  ")
                        .Append("     , FF.TOACCOUNTNAME  ")
                        .Append("     , FF.READFLG  ")
                        .Append("     , TO_CHAR(FF.SENDDATE,'yyyy/mm/dd hh24:mi') AS SENDDATE  ")
                        .Append("     , FF.STATUS  ")
                        .Append("     , CC.SALES_ID AS FLLWUPBOX_SEQNO ")
                        .Append("     , DD.ORG_IMGFILE  ")
                        .Append("     , NVL(EE.NOTICEMSG_DLR,NVL(EE.NOTICEMSG_ENG,'NOMESSAGE')) AS NOTICEMSG_DLR  ")
                        .Append("     , NVL(AA.CSPAPERNAME,'NOPAPER') AS CSPAPERNAME  ")
                        '$05 start 受注後フォロー機能開発
                        .Append("     , AA.AFTER_ODR_ACT_CD  ")
                        '$05 end 受注後フォロー機能開発
                        .Append("      ")
                        .Append("    FROM  ")
                        .Append("            TBL_NOTICEREQUEST AA ")
                        .Append("            ,( ")
                        .Append("                 SELECT  ")
                        .Append("                   MAX(ZZ.NOTICEID) AS NOTICEID  ")
                        .Append("                   ,ZZ.STATUS ")
                        .Append("                   ,ZZ.NOTICEREQID ")
                        .Append("                   ,ZZ.FROMACCOUNT ")
                        .Append("             FROM  TBL_NOTICEINFO ZZ ")
                        .Append("            WHERE  ZZ.TOACCOUNT = :TOACCOUNT  ")
                        .Append("              AND  ZZ.SENDDATE >= TRUNC(:SENDDATE) ")
                        .Append("              AND  ZZ.STATUS = :STATUSREQUEST ")
                        .Append("              AND  ZZ.FROMACCOUNT <> :TOACCOUNT ")
                        .Append("         GROUP BY  ZZ.NOTICEREQID  ")
                        .Append("                 , ZZ.STATUS  ")
                        .Append("                 , ZZ.FROMACCOUNT  ")
                        .Append("                 , ZZ.TOACCOUNT  ")
                        .Append("           )BB ")
                        .Append("        , TB_T_SALES CC ")
                        .Append("       , TBL_NOTICEINFO FF ")
                        .Append("       , TBL_USERS DD ")
                        .Append("       , TBL_NOTICECTGMST EE ")
                        .Append("   WHERE BB.NOTICEREQID = FF.NOTICEREQID ")
                        .Append("     AND AA.NOTICEREQID = FF.NOTICEREQID  ")
                        .Append("     AND AA.STATUS = FF.STATUS  ")
                        .Append("     AND AA.FLLWUPBOX = CC.SALES_ID(+)  ")
                        .Append("     AND AA.NOTICEREQCTG = EE.NOTICEREQCTG  ")
                        .Append("     AND FF.STATUS = EE.NOTICESTATUSID ")
                        .Append("     AND FF.FROMACCOUNT = DD.ACCOUNT(+) ")
                        .Append("     AND AA.NOTICEREQCTG IN(:CONSULTATION,:HELP,:CONTRACTAPPROVAL) ")
                        .Append("     AND FF.TOACCOUNT = :TOACCOUNT ")
                        .Append("     AND CC.DLR_CD(+) = :DLRCD ")
                        .Append("     AND CC.BRN_CD(+) = :STRCD ")
                        .Append("     AND FF.SENDDATE >= TRUNC(:SENDDATE) ")
                    End If
                    .Append(" ORDER BY           SENDDATE DESC  ")
                    .Append("                  ,NOTICEREQID ASC ")
                End With
                ' $03 end
                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("ASSESSMENT", OracleDbType.Char, Assessment)
                query.AddParameterWithTypeValue("CONSULTATION", OracleDbType.Char, Consultation)
                query.AddParameterWithTypeValue("HELP", OracleDbType.Char, Help)
                ' $01 start step2開発
                query.AddParameterWithTypeValue("CLAIM", OracleDbType.Char, Claim)
                query.AddParameterWithTypeValue("CSSURVEY", OracleDbType.Char, CSSurvey)
                ' $01 end   step2開発
                ' $02 start サービス入庫
                query.AddParameterWithTypeValue("SURVICESTORE", OracleDbType.Char, SurviceStore)
                ' $02 end サービス入庫
                ' $05 start 受注後フォロー機能開発
                query.AddParameterWithTypeValue("AFTERODRFOLLOW", OracleDbType.Char, AfterOdrFollow)
                ' $05 end 受注後フォロー機能開発
                ' $06 start 納車予定日変更対応
                query.AddParameterWithTypeValue("DELISCHEDATECHG", OracleDbType.Char, DeliScheDateChg)
                ' $06 end 納車予定日変更対応
                ' $07 start フォローアップメモ更新対応
                query.AddParameterWithTypeValue("FLLWUPMEMOUPDATE", OracleDbType.Char, FllwupMemoUpdate)
                ' $07 end フォローアップメモ更新対応
                query.AddParameterWithTypeValue("TOACCOUNT", OracleDbType.Varchar2, userAccount)
                query.AddParameterWithTypeValue("SENDDATE", OracleDbType.Date, displayDays)
                ' $03 start
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, storeCode)
                ' $03 end
                'スタート行が１行目以外は既読のみ読み取る
                If beginRowIndex <> RowNextRowsCount Then
                    query.AddParameterWithTypeValue("READFLG", OracleDbType.Char, ReadList)
                End If
                ' $04 start 契約承認対応
                query.AddParameterWithTypeValue("CONTRACTAPPROVAL", OracleDbType.Char, ContractApproval)
                query.AddParameterWithTypeValue("ORDERINFOMATION", OracleDbType.Char, OrderInfomation)
                If leaderFlag Then
                    query.AddParameterWithTypeValue("STATUSREQUEST", OracleDbType.Char, StatusRequest)
                End If
                ' $04 end 契約承認対応

                Dim dt As SC3040801DataSet.GetSalesNoticeDataTable = query.GetData()

                Logger.Info("return=GetSalesNoticeDataTable.COUNT=" & _
                            CStr(dt.Rows.Count) & _
                            "_" & _
                            System.Reflection.MethodBase.GetCurrentMethod.Name & _
                            "__END")
                '検索結果返却
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' サービス通知履歴を取得
        ''' </summary>
        ''' <param name="userAccount">ログインユーザーアカウント</param>
        ''' <param name="beginRowIndex">リピータのスタート行</param>
        ''' <param name="displayDays">表示日数</param>
        ''' <returns>GetServiceNoticeDataTable</returns>
        ''' <remarks></remarks>
        Public Function GetServiceNotice(ByVal userAccount As String, _
                                                ByVal beginRowIndex As Integer, _
                                                ByVal displayDays As Date) _
                                            As SC3040801DataSet.GetServiceNoticeDataTable
            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_userAccount=" & _
                        userAccount & _
                        "_beginRowIndex=" & _
                        CStr(beginRowIndex) & _
                        "_displayDays=" & _
                        CStr(displayDays))

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetServiceNoticeDataTable)("SC3040801_002")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("  SELECT /* SC3040801_002 */ ")
                    .Append("	 	    A.NOTICEREQID ")
                    .Append("	 	   ,B.NOTICEID ")
                    .Append("		   ,B.READFLG ")
                    .Append("		   ,B.SESSIONVALUE ")
                    '.Append("		   ,NVL(B.SESSIONVALUE,',') AS SESSIONVALUE")
                    .Append("		   ,TO_CHAR(B.SENDDATE,'yyyy/mm/dd hh24:mi') AS SENDDATE ")
                    .Append("		   ,NVL(B.MESSAGE,'NOMESSAGE') AS MESSAGE ")
                    '.Append("		   ,NVL(C.SYS_IMGFILE,NVL(C.ORG_IMGFILE,'NOPHOTO')) AS SYS_IMGFILE")
                    .Append("		   ,C.ORG_IMGFILE ")
                    .Append("    FROM 		 TBL_NOTICEREQUEST A ")
                    .Append("			    ,TBL_NOTICEINFO B ")
                    .Append("			    ,TBL_USERS C ")
                    .Append("   WHERE 			A.NOTICEREQID = B.NOTICEREQID ")
                    .Append("     AND			B.FROMACCOUNT = C.ACCOUNT(+) ")
                    .Append("     AND           B.TOACCOUNT = :TOACCOUNT ")
                    .Append("     AND           B.SENDDATE >= TRUNC(:SENDDATE) ")

                    'スタート行が１行目以外は既読のみ読み取る
                    If beginRowIndex <> RowNextRowsCount Then
                        Logger.Info("WHERE.APPEND=(B.READFLG = :READFLG)")
                        .Append(" AND           B.READFLG = :READFLG ")
                    End If
                    .Append("ORDER BY            B.SENDDATE DESC ")
                    .Append("                   ,A.NOTICEREQID ASC ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("TOACCOUNT", OracleDbType.Varchar2, userAccount)
                query.AddParameterWithTypeValue("SENDDATE", OracleDbType.Date, displayDays)

                'スタート行が１行目以外は既読のみ読み取る
                If beginRowIndex <> RowNextRowsCount Then
                    query.AddParameterWithTypeValue("READFLG", OracleDbType.Char, ReadList)
                End If

                Dim dt As SC3040801DataSet.GetServiceNoticeDataTable = query.GetData()

                Logger.Info("return=GetServiceNoticeDataTable.COUNT=" & _
                            CStr(dt.Rows.Count) & _
                            "_" & _
                            System.Reflection.MethodBase.GetCurrentMethod.Name & _
                            "__END")
                '検索結果返却
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' 写真のURLの取得
        ''' </summary>
        ''' <param name="storeCode">販売店コード</param>
        ''' <param name="dealerCode">店舗コード</param>
        ''' <returns>写真のURL</returns>
        ''' <remarks></remarks>
        Public Function GetPhotoPath(ByVal dealerCode As String, _
                                            ByVal storeCode As String) _
                                        As SC3040801DataSet.GetPhotoPathDataTable
            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_dealerCode=" & _
                        dealerCode & _
                        "_storeCode=" & _
                        storeCode)

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetPhotoPathDataTable)("SC3040801_003")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("SELECT /* SC3040801_003 */ ")
                    .Append("       NVL(A.PARAMVALUE,'NOPHOTO') AS IPPATH ")
                    .Append("      ,NVL(B.PARAMVALUE,'NOPHOTO') AS IMGPATH ")
                    .Append("  FROM 	 TBL_DLRENVSETTING A ")
                    .Append("           ,TBL_DLRENVSETTING B ")
                    .Append(" WHERE 		A.PARAMNAME = :URL1 ")
                    .Append("   AND 		A.STRCD = :STRCD ")
                    .Append("   AND 		A.DLRCD = :DLRCD ")
                    .Append("   AND 		B.PARAMNAME = :URL2 ")
                    .Append("   AND 		B.STRCD = :STRCD ")
                    .Append("   AND		    B.DLRCD = :DLRCD ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("URL1", OracleDbType.Varchar2, FilePath)
                query.AddParameterWithTypeValue("URL2", OracleDbType.Varchar2, FileUrl)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, storeCode)
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dealerCode)

                Dim dt As SC3040801DataSet.GetPhotoPathDataTable = query.GetData()

                Logger.Info("return=GetPhotoPathDataTable.COUNT=" & _
                            CStr(dt.Rows.Count) & _
                            "_" & _
                            System.Reflection.MethodBase.GetCurrentMethod.Name & _
                            "__END")

                '検索結果返却
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' 最終ステータスの確認
        ''' </summary>
        ''' <param name="noticeRequestId">通知依頼ID</param>
        ''' <returns>最終ステータス</returns>
        ''' <remarks></remarks>
        Public Function GetLastStatus(ByVal noticeRequestId As Long) _
                   As SC3040801DataSet.GetLastStatusDataTable
            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_noticeRequestId=" & _
                        CStr(noticeRequestId))

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetLastStatusDataTable)("SC3040801_004")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("SELECT /* SC3040801_004 */ ")
                    .Append("       STATUS ")
                    .Append("  FROM     TBL_NOTICEREQUEST ")
                    .Append(" WHERE          NOTICEREQID = :NOTICEREQID ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, noticeRequestId)

                Dim dt As SC3040801DataSet.GetLastStatusDataTable = query.GetData()

                Logger.Info("return=GetLastStatusDataTable.COUNT=" & _
                            CStr(dt.Rows.Count) & _
                            "_" & _
                            System.Reflection.MethodBase.GetCurrentMethod.Name & _
                            "__END")
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' session情報
        ''' </summary>
        ''' <param name="noticeRequestId">通知依頼ID</param>
        ''' <returns>GetTransitionParameterDataTable</returns>
        ''' <remarks></remarks>
        Public Function GetTransitionParameter(ByVal noticeRequestId As Long) _
                   As SC3040801DataSet.GetTransitionParameterDataTable
            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_noticeRequestId=" & _
                        CStr(noticeRequestId))

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetTransitionParameterDataTable)("SC3040801_005")

                Dim sql As New StringBuilder

                'SQL文作成
                With sql
                    .Append("	SELECT /* SC3040801_005 */ ")
                    .Append("				  REQCLASSID ")
                    .Append("				 ,CRCUSTID ")
                    .Append("				 ,CSTKIND ")
                    .Append("				 ,CUSTOMERCLASS ")
                    .Append("				 ,SALESSTAFFCD ")
                    .Append("				 ,VCLID ")
                    .Append("				 ,FLLWUPBOX ")
                    .Append("				 ,FLLWUPBOXSTRCD ")
                    .Append("	 FROM           TBL_NOTICEREQUEST ")
                    .Append("   WHERE 				NOTICEREQID = :NOTICEREQID ")

                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, noticeRequestId)

                Dim dt As SC3040801DataSet.GetTransitionParameterDataTable = query.GetData()

                Logger.Info("return=GetTransitionParameterDataTable.COUNT=" & _
                            CStr(dt.Rows.Count) & _
                            "_" & _
                            System.Reflection.MethodBase.GetCurrentMethod.Name & _
                            "__END")
                Return dt
            End Using
        End Function

        ''' <summary>
        ''' CANCEL処理を行うパラメーターの取得
        ''' </summary>
        ''' <param name="noticeRequestId">通知依頼ID</param>
        ''' <param name="userAccount">ユーザーアカウント</param>
        ''' <returns>GetCancelParameterDataTable</returns>
        ''' <remarks></remarks>
        Public Function GetCancelParameter(ByVal noticeRequestId As Long, _
                                           ByVal userAccount As String) _
                                           As SC3040801DataSet.GetCancelParameterDataTable
            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_noticeRequestId=" & _
                        CStr(noticeRequestId))

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetCancelParameterDataTable)("SC3040801_006")

                Dim sql As New StringBuilder
                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3040801_006 */ ")
                    .Append("           A.NOTICEREQID ")
                    .Append("          ,A.NOTICEREQCTG ")
                    .Append("          ,A.REQCLASSID ")
                    .Append("          ,A.CUSTOMNAME ")
                    .Append("          ,B.FROMCLIENTID ")
                    .Append("          ,B.TOACCOUNT ")
                    .Append("          ,B.TOCLIENTID ")
                    .Append("          ,B.TOACCOUNTNAME ")
                    .Append("   FROM     TBL_NOTICEREQUEST A ")
                    .Append("           ,TBL_NOTICEINFO B ")
                    .Append("  WHERE        A.NOTICEREQID = B.NOTICEREQID ")
                    .Append("    AND        A.STATUS = B.STATUS ")
                    .Append("    AND        A.NOTICEREQID = :NOTICEREQID ")
                    .Append("    AND        A.LASTNOTICEID <= B.NOTICEID ")
                    .Append("    AND       (B.TOACCOUNT <> :TOACCOUNT ")
                    .Append("     OR        B.TOACCOUNT IS NULL) ")

                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("NOTICEREQID", OracleDbType.Int64, noticeRequestId)
                query.AddParameterWithTypeValue("TOACCOUNT", OracleDbType.Varchar2, userAccount)

                Dim dt As SC3040801DataSet.GetCancelParameterDataTable = query.GetData()

                Logger.Info("return=GetCancelParameterDataTable.COUNT=" & _
                            CStr(dt.Rows.Count) & _
                            "_" & _
                            System.Reflection.MethodBase.GetCurrentMethod.Name & _
                            "__END")
                Return dt
            End Using
        End Function

        '$05 start 受注後フォロー機能開発

        ''' <summary>
        ''' 受注後活動名称の取得
        ''' </summary>
        ''' <param name="afterOdrActCd"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetAfterOdrActName(ByVal afterOdrActCd As String) _
                                           As SC3040801DataSet.GetAfterOdrActNameDataTable
            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_afterOdrActCd=" & _
                        afterOdrActCd)

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetAfterOdrActNameDataTable)("SC3040801_007")

                Dim sql As New StringBuilder
                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3040801_007 */ ")
                    .Append("   CASE WHEN T2.WORD_VAL IS NULL THEN NULL ")
                    .Append("        WHEN T2.WORD_VAL = ' ' THEN TRIM(T2.WORD_VAL_ENG) ")
                    .Append("        ELSE TRIM(T2.WORD_VAL) ")
                    .Append("    END AS WORD_VAL ")
                    .Append("   FROM TB_M_AFTER_ODR_ACT T1 ")
                    .Append("      , TB_M_WORD T2 ")
                    .Append("  WHERE T1.AFTER_ODR_ACT_NAME = T2.WORD_CD(+) ")
                    .Append("    AND T1.AFTER_ODR_ACT_CD = :AFTER_ODR_ACT_CD ")

                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("AFTER_ODR_ACT_CD", OracleDbType.NVarchar2, afterOdrActCd)

                Dim dt As SC3040801DataSet.GetAfterOdrActNameDataTable = query.GetData()

                Logger.Info("return=GetAfterOdrActNameDataTable.COUNT=" & _
                CStr(dt.Rows.Count) & _
                "_" & _
                System.Reflection.MethodBase.GetCurrentMethod.Name & _
                "__END")
                Return dt

            End Using
        End Function

        ''' <summary>
        ''' 受注後活動アイコンパスの取得
        ''' </summary>
        ''' <param name="dealerCode">販売店コード</param>
        ''' <param name="afterOdrActCd">受注後活動コード</param>
        ''' <returns>GetAfterOdrActIconPathDataTable</returns>
        ''' <remarks></remarks>
        Public Function GetAfterOdrActIconPath(ByVal dealerCode As String, _
                                               ByVal afterOdrActCd As String) _
                                           As SC3040801DataSet.GetAfterOdrActIconPathDataTable
            Logger.Info("START__" & _
                        System.Reflection.MethodBase.GetCurrentMethod.Name & _
                        "_dealerCode=" & _
                        dealerCode & _
                        "_afterOdrActCd=" & _
                        afterOdrActCd)

            Using query As New DBSelectQuery(Of SC3040801DataSet.GetAfterOdrActIconPathDataTable)("SC3040801_008")

                Dim sql As New StringBuilder
                'SQL文作成
                With sql
                    .Append(" SELECT /* SC3040801_008 */ ")
                    .Append("        NVL(T4.ICON_PATH, T5.ICON_PATH) AS ICON_PATH ")
                    .Append("   FROM TB_M_IMG_PATH_CONTROL T1 ")
                    .Append("      , (SELECT T2.ICON_PATH ")
                    .Append("              , T2.TYPE_CD ")
                    .Append("              , T2.DEVICE_TYPE ")
                    .Append("              , T2.FIRST_KEY ")
                    .Append("              , T2.SECOND_KEY ")
                    .Append("           FROM TB_M_IMG_PATH_CONTROL T2 ")
                    .Append("          WHERE T2.DLR_CD = :DLR_CD) T4 ")
                    .Append("      , (SELECT T3.ICON_PATH ")
                    .Append("              , T3.TYPE_CD ")
                    .Append("              , T3.DEVICE_TYPE ")
                    .Append("              , T3.FIRST_KEY ")
                    .Append("              , T3.SECOND_KEY ")
                    .Append("           FROM TB_M_IMG_PATH_CONTROL T3 ")
                    .Append("          WHERE T3.DLR_CD = :DLR_CD_X) T5 ")
                    .Append("  WHERE T1.TYPE_CD = T4.TYPE_CD(+) ")
                    .Append("    AND T1.DEVICE_TYPE = T4.DEVICE_TYPE(+) ")
                    .Append("    AND T1.FIRST_KEY = T4.FIRST_KEY(+) ")
                    .Append("    AND T1.SECOND_KEY = T4.SECOND_KEY(+) ")
                    .Append("    AND T1.TYPE_CD = T5.TYPE_CD(+) ")
                    .Append("    AND T1.DEVICE_TYPE = T5.DEVICE_TYPE(+) ")
                    .Append("    AND T1.FIRST_KEY = T5.FIRST_KEY(+) ")
                    .Append("    AND T1.SECOND_KEY = T5.SECOND_KEY(+) ")
                    .Append("    AND T1.TYPE_CD = :TYPE_CD ")
                    .Append("    AND T1.DEVICE_TYPE = :DEVICE_TYPE ")
                    .Append("    AND T1.FIRST_KEY = :FIRST_KEY ")
                    .Append("    AND T1.SECOND_KEY = :SECOND_KEY ")
                    .Append("    AND ROWNUM <= 1 ")

                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
                query.AddParameterWithTypeValue("DLR_CD_X", OracleDbType.NVarchar2, DealerCodeX)
                query.AddParameterWithTypeValue("TYPE_CD", OracleDbType.NVarchar2, TypeCdAfterOdrActCd)
                query.AddParameterWithTypeValue("DEVICE_TYPE", OracleDbType.NVarchar2, DeviceTypeSalesTablet)
                query.AddParameterWithTypeValue("FIRST_KEY", OracleDbType.NVarchar2, afterOdrActCd)
                query.AddParameterWithTypeValue("SECOND_KEY", OracleDbType.NVarchar2, SecondKey)

                Dim dt As SC3040801DataSet.GetAfterOdrActIconPathDataTable = query.GetData()

                Logger.Info("return=GetAfterOdrActIconPathDataTable.COUNT=" & _
                CStr(dt.Rows.Count) & _
                "_" & _
                System.Reflection.MethodBase.GetCurrentMethod.Name & _
                "__END")
                Return dt

            End Using
        End Function

        '$05 end 受注後フォロー機能開発

#End Region

    End Class

End Namespace

Partial Class SC3040801DataSet
End Class