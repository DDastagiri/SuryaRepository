'===================================================================
' SC3100401DataSet
'-------------------------------------------------------------------
' 機能：未振当て一覧画面 データアクセス
' 補足：               
' 作成：2013/03/01 TMEJ 河原 
' 更新：2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
' 更新：2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
' 更新：2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発
' 更新：2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
' 更新：2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更
' 更新；2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する
' 更新； 2015/12/17 TM 浅野 12月号口配信に向けた緊急対応 通知処理時の顧客名称の取得元をサービス来店実績から顧客へ変更する
' 更新：2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない
' 更新：2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする
' 更新：2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示
' 更新：
'===================================================================

Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Namespace SC3100401DataSetTableAdapters
    Public Class SC3100401TableAdapter
        Inherits Global.System.ComponentModel.Component

#Region "定数"

        ''' <summary>
        ''' アプリケーションID
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ApplicationID As String = "SC3100401"

        ''' <summary>
        ''' LisrRowName
        ''' </summary>
        ''' <remarks></remarks>
        Private Const LisrRowName As String = "ROWNO_"

        ''' <summary>
        ''' ログインカテゴリ－(小カテゴリー)
        ''' </summary>
        Private Const PresenceDetail As String = "0"

        ''' <summary>
        ''' 削除フラグ
        ''' </summary>
        Private Const DeleteFlag As String = "0"

        ''' <summary>
        ''' 振当てステータス（未振当て）
        ''' </summary>
        Private Const NonAssign As String = "0"

        ''' <summary>
        ''' 振当てステータス（受付待ち）
        ''' </summary>
        Private Const AssignWait As String = "1"

        ''' <summary>
        ''' 振当てステータス（振当済み）
        ''' </summary>
        Private Const AssignFinish As String = "2"

        ''' <summary>
        ''' 振当てステータス（退店）
        ''' </summary>
        Private Const DealerOut As String = "4"

        ''' <summary>
        ''' 呼出ステータス（未呼出）
        ''' </summary>
        Private Const NonCall As String = "0"

        ''' <summary>
        ''' 呼出ステータス（呼出中）
        ''' </summary>
        Private Const Calling As String = "1"

        ''' <summary>
        ''' 中断フラグ「0：有効」
        ''' </summary>
        Private Const StopFlag As String = "0"

        ''' <summary>
        ''' 中断フラグ「0：WALKIN」
        ''' </summary>
        Private Const StopFlagWalkIn As String = "5"

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START
        ''' <summary>
        ''' キャンセルフラグ(キャンセル)
        ''' </summary>
        Private Const CancelFlag As String = "1"

        ''' <summary>
        ''' キャンセルフラグ(有効)
        ''' </summary>
        Private Const CancelFlagEffective As String = "0"

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        ''' <summary>
        ''' 性別「0：男性」
        ''' </summary>
        ''' <remarks></remarks>
        Private Const Male As String = "0"

        ''' <summary>
        ''' サービスコード「20：定期点検」
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiCodeRegular As String = "20"

        ''' <summary>
        ''' サービスコード「30：一般点検」
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiCodeGeneral As String = "30"

        ''' <summary>
        ''' サービスコード「40：BP保険」
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiCodeBP As String = "40"

        ''' <summary>
        ''' サービスコード「97：引取り」
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiCodeTaking As String = "97"

        ''' <summary>
        ''' サービスコード「98：納車」
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ServiCodeDelivery As String = "98"

        ''' <summary>
        ''' 案内待ちキュー状態(非案内待ち)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const QueueStatusNotWait As String = "1"

        ''' <summary>
        ''' ステータス「1：本予約」
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CommittedResource As Integer = 1

        ''' <summary>
        ''' ステータス「2：仮予約」
        ''' </summary>
        ''' <remarks></remarks>
        Private Const ProposedResource As Integer = 2

        ''' <summary>
        ''' サービス来店自社客
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CustSegmentMyCustomer As String = "1"

        ''' <summary>
        ''' サービス来店未取引客
        ''' </summary>
        ''' <remarks></remarks>
        Private Const CustSegmentNewCustomer As String = "2"

        ''' <summary>
        ''' ストール予約自社客
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallRezMyCustomer As String = "0"

        ''' <summary>
        ''' ストール予約未取引客
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StallRezNewCustomer As String = "1"

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ''' <summary>
        ''' サービスステータス(未入庫)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusNoIn As String = "00"

        ''' <summary>
        ''' サービスステータス(未来店客)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusNoVisit As String = "01"

        ''' <summary>
        ''' サービスステータス(キャンセル)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusCancel As String = "02"

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

        ''' <summary>
        ''' サービスステータス(納車済み)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const StatusFinishDelivery As String = "13"

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        ''' <summary>
        ''' 受付区分(予約客)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const AcceptanceTypeRez As String = "0"

        ''' <summary>
        ''' オーナーチェンジフラグ(未設定)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const OwnerChangeFlag As String = "0"

        '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
        ' ''' <summary>
        ' ''' 顧客車両区分(所有者)
        ' ''' </summary>
        ' ''' <remarks></remarks>
        'Private Const VehicleType As String = "1"

        ''' <summary>
        ''' 顧客車両区分(所有者)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VehicleTypeOwner As String = "1"

        ''' <summary>
        ''' 顧客車両区分(保険)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const VehicleTypeInsurance As String = "4"

        '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END



        ''' <summary>
        ''' DB日付省略値
        ''' </summary>
        ''' <remarks></remarks>
        Private Const MinDate As String = "1900/01/01 00:00:00"

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

        ''' <summary>
        ''' RO情報有無(0：情報なし)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RepairOrderInfoNone As String = "0"
        ''' <summary>
        ''' RO情報有無(1：情報あり)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RepairOrderInfoExist As String = "1"

        ''' <summary>
        ''' 使用中フラグ(1：使用中)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const UsingTypeUse As String = "1"

        ''' <summary>
        ''' ROステータス(35：SA承認待ち)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RepairOrderStatusWaitSA As String = "35"
        ''' <summary>
        ''' ROステータス(80：納車準備待ち)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RepairOrderStatusWaitDelivery As String = "80"
        ''' <summary>
        ''' ROステータス(85：納車作業中)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RepairOrderStatusWorkDelivery As String = "85"
        ''' <summary>
        ''' ROステータス(99：ROキャンセル)
        ''' </summary>
        ''' <remarks></remarks>
        Private Const RepairOrderStatusCancel As String = "99"

        '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
        ''' <summary>
        ''' アイコン表示フラグ（0：非表示）
        ''' </summary>
        ''' <remarks></remarks>
        Private Const IconFlagOff As String = "0"
        '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

#End Region

#Region "Enum"

        ''' <summary>
        ''' イベントキーID
        ''' </summary>
        Private Enum EventKeyID

            ''' <summary>
            ''' なし
            ''' </summary>
            ''' <remarks></remarks>
            None = 0
            ''' <summary>
            ''' 呼出ボタン
            ''' </summary>
            FooterCallButton = 100
            ''' <summary>
            ''' 呼出キャンセルボタン
            ''' </summary>
            FooterCancelButton = 200
            ''' <summary>
            ''' チップ削除ボタン
            ''' </summary>
            FooterDeleteButton = 300

            ''' <summary>
            ''' 発券番号テキスト
            ''' </summary>
            ReceiptNoText = 1100
            ''' <summary>
            ''' 車両登録Noテキスト
            ''' </summary>
            RegNoText = 1200
            ''' <summary>
            ''' 来店者テキスト
            ''' </summary>
            VisitorText = 1300
            ''' <summary>
            ''' 電話番号テキスト
            ''' </summary>
            TellNoText = 1400
            ''' <summary>
            ''' テーブルNoテキスト
            ''' </summary>
            TableNoText = 1500

        End Enum

#End Region

#Region "SELECT"

        ''' <summary>
        ''' 来店一覧情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <returns>SA一覧情報返却</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
        ''' </history>
        Public Function GetDBReceptionList(ByVal inDealerCode As String, _
                                           ByVal inStoreCode As String, _
                                           ByVal inPresentTime As Date) As SC3100401DataSet.ReceptionListDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} DEALERCODE:{2} STORECODE:{3} PRESENTTIME:{4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inStoreCode, inPresentTime))

            Dim sql As New StringBuilder                                                    ' SQL文格納

            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

            'With sql
            '    .AppendLine("  SELECT /* SC3100401_001 */")
            '    .AppendLine("          CONCAT(:ROWNAME, TO_CHAR(ROWNUM)) AS ROWNO")
            '    .AppendLine("         ,T1.VISITSEQ")
            '    .AppendLine("         ,T1.VISITTIMESTAMP")
            '    .AppendLine("         ,T1.ELAPSEDTIME")
            '    .AppendLine("         ,T1.VCLREGNO")
            '    .AppendLine("         ,T1.SEX")
            '    .AppendLine("         ,T1.NAME")
            '    .AppendLine("         ,T1.TELNO")
            '    .AppendLine("         ,T1.SACODE")
            '    .AppendLine("         ,T1.SANAME")
            '    .AppendLine("         ,T1.DEFAULTSANAME")
            '    .AppendLine("         ,T1.ASSIGNSTATUS")
            '    .AppendLine("         ,T1.ORDERNO")
            '    .AppendLine("         ,T1.UPDATEDATE")
            '    .AppendLine("         ,T1.CALLNO")
            '    .AppendLine("         ,T1.CALLPLACE")
            '    .AppendLine("         ,T1.CALLSTATUS")
            '    .AppendLine("         ,T1.VISITNAME")
            '    .AppendLine("         ,T1.VISITTELNO")
            '    .AppendLine("         ,T1.VEHICLENAME")
            '    .AppendLine("         ,CASE TO_CHAR(T1.PLANSTARTDATE, 'YYYY/MM/DD') ")
            '    .AppendLine("               WHEN :PLANSTARTDATE THEN TO_CHAR(T1.PLANSTARTDATE, 'HH24:MI') ")
            '    .AppendLine("               ELSE TO_CHAR(T1.PLANSTARTDATE, 'MM/DD') ")
            '    .AppendLine("                END AS PLANSTARTDATE")
            '    .AppendLine("         ,T1.MERCHANDISENAME ")
            '    .AppendLine("    FROM ( ")
            '    .AppendLine("             SELECT   D1.DLRCD")
            '    .AppendLine("                     ,D1.STRCD")
            '    .AppendLine("                     ,D1.VISITSEQ")
            '    .AppendLine("                     ,D1.VISITTIMESTAMP")
            '    .AppendLine("                     ,D1.ELAPSEDTIME")
            '    .AppendLine("                     ,D1.VCLREGNO")
            '    .AppendLine("                     ,D1.SEX")
            '    .AppendLine("                     ,D1.NAME")
            '    .AppendLine("                     ,D1.TELNO")
            '    .AppendLine("                     ,D1.SACODE")
            '    .AppendLine("                     ,D1.SANAME")
            '    .AppendLine("                     ,D1.DEFAULTSANAME ")
            '    .AppendLine("                     ,D1.FREZID")
            '    .AppendLine("                     ,D1.ASSIGNSTATUS")
            '    .AppendLine("                     ,D1.ORDERNO")
            '    .AppendLine("                     ,D1.UPDATEDATE")
            '    .AppendLine("                     ,D1.CALLNO")
            '    .AppendLine("                     ,D1.CALLPLACE")
            '    .AppendLine("                     ,D1.CALLSTATUS")
            '    .AppendLine("                     ,D1.VISITNAME")
            '    .AppendLine("                     ,D1.VISITTELNO")
            '    .AppendLine("                     ,S1.MODELCODE")
            '    .AppendLine("                     ,S1.VEHICLENAME")
            '    .AppendLine("                     ,NVL(TO_DATE(S1.REZ_PICK_DATE, 'yyyy/mm/dd hh24:mi'), (SELECT MIN(S3.STARTTIME) ")
            '    .AppendLine("                                                                              FROM TBL_STALLREZINFO S3 ")
            '    .AppendLine("                                                                             WHERE S3.DLRCD = S1.DLRCD ")
            '    .AppendLine("                                                                               AND S3.STRCD = S1.STRCD ")
            '    .AppendLine("                                                                               AND (S3.REZID = S1.REZID ")
            '    .AppendLine("                                                                                OR S3.PREZID = S1.PREZID) ")
            '    .AppendLine("                                                                               AND NOT EXISTS (SELECT 1 ")
            '    .AppendLine("                                                                                                 FROM TBL_STALLREZINFO S4 ")
            '    .AppendLine("                                                                                                WHERE S4.DLRCD = S3.DLRCD ")
            '    .AppendLine("                                                                                                  AND S4.STRCD = S3.STRCD ")
            '    .AppendLine("                                                                                                  AND S4.REZID = S3.REZID ")
            '    .AppendLine("                                                                                                  AND (S4.STOPFLG = :STOPFLG ")
            '    .AppendLine("                                                                                                  AND S4.CANCELFLG = :CANCELFLG)) ")
            '    .AppendLine("                          )) AS PLANSTARTDATE ")
            '    .AppendLine("                     ,S1.MNTNCD")
            '    .AppendLine("                     ,NVL(CONCAT(M1.SVCORGNMCT, M1.SVCORGNMCB), (SELECT MAX(NVL(M2.SVCORGNAME,M2.SVCENGNAME)) AS MERCHANDISENAME ")
            '    .AppendLine("                                                                   FROM TBL_SSERVICE M2 ")
            '    .AppendLine("                                                                  WHERE  M2.DLRCD = S1.DLRCD ")
            '    .AppendLine("                                                                    AND  M2.STRCD = S1.STRCD ")
            '    .AppendLine("                                                                    AND  M2.SERVICECODE = TRIM(S1.SERVICECODE_S)) ")
            '    .AppendLine("                          ) AS MERCHANDISENAME ")
            '    .AppendLine("               FROM  (")
            '    .AppendLine("                         SELECT   V.DLRCD")
            '    .AppendLine("                                 ,V.STRCD")
            '    .AppendLine("                                 ,V.VISITSEQ")
            '    .AppendLine("                                 ,TO_CHAR(V.VISITTIMESTAMP,'HH24:MI') AS VISITTIMESTAMP")
            '    .AppendLine("                                 ,TRUNC((:VISITTIMESTAMP - V.VISITTIMESTAMP) * (24 * 60)) AS ELAPSEDTIME")
            '    .AppendLine("                                 ,V.VCLREGNO")
            '    .AppendLine("                                 ,NVL(V.SEX, :SEX) AS SEX")
            '    .AppendLine("                                 ,V.NAME")
            '    .AppendLine("                                 ,NVL(V.MOBILE, V.TELNO) AS TELNO")
            '    .AppendLine("                                 ,V.SACODE")
            '    .AppendLine("                                 ,V.REZID")
            '    .AppendLine("                                 ,V.FREZID")
            '    .AppendLine("                                 ,NVL(V.ASSIGNSTATUS, :ASSIGNSTATUS_RECEPTION) AS ASSIGNSTATUS")
            '    .AppendLine("                                 ,TRIM(V.ORDERNO) AS ORDERNO")
            '    .AppendLine("                                 ,V.UPDATEDATE")
            '    .AppendLine("                                 ,V.CALLNO")
            '    .AppendLine("                                 ,V.CALLPLACE")
            '    .AppendLine("                                 ,V.CALLSTATUS")
            '    .AppendLine("                                 ,V.VISITNAME")
            '    .AppendLine("                                 ,V.VISITTELNO")
            '    .AppendLine("                                 ,U1.USERNAME AS SANAME")
            '    .AppendLine("                                 ,U2.USERNAME AS DEFAULTSANAME")
            '    .AppendLine("                           FROM  TBL_SERVICE_VISIT_MANAGEMENT V")
            '    .AppendLine("                                ,TBL_USERS U1")
            '    .AppendLine("                                ,TBL_USERS U2 ")
            '    .AppendLine("                          WHERE   V.DLRCD = U1.DLRCD(+)")
            '    .AppendLine("                            AND   V.STRCD = U1.STRCD(+)")
            '    .AppendLine("                            AND   V.SACODE = U1.ACCOUNT(+)")
            '    .AppendLine("                            AND   V.DLRCD = U2.DLRCD(+)")
            '    .AppendLine("                            AND   V.STRCD = U2.STRCD(+)")
            '    .AppendLine("                            AND   V.DEFAULTSACODE = U2.ACCOUNT(+)")
            '    .AppendLine("                            AND   V.DLRCD = :DLRCD")
            '    .AppendLine("                            AND   V.STRCD = :STRCD")
            '    .AppendLine("                            AND   V.VISITTIMESTAMP")
            '    .AppendLine("                        BETWEEN   TRUNC(:VISITTIMESTAMP)")
            '    .AppendLine("                            AND   TRUNC(:VISITTIMESTAMP) + 86399/86400")
            '    .AppendLine("                            AND   V.ASSIGNSTATUS IN (:ASSIGNSTATUS_RECEPTION, :ASSIGNSTATUS_WAIT, :ASSIGNSTATUS_FIN) ")
            '    .AppendLine("                            AND   V.CALLSTATUS IN (:CALLSTATUS_NOCALL, :CALLSTATUS_CALLING) ")
            '    .AppendLine("                      ) D1 ")
            '    .AppendLine("                       ,TBL_STALLREZINFO S1 ")
            '    .AppendLine("                       ,TBL_MERCHANDISEMST M1 ")
            '    .AppendLine("              WHERE   D1.DLRCD = S1.DLRCD(+) ")
            '    .AppendLine("                AND   D1.STRCD = S1.STRCD(+) ")
            '    .AppendLine("                AND   D1.REZID = S1.REZID(+) ")
            '    .AppendLine("                AND   S1.DLRCD = M1.DLRCD(+)")
            '    .AppendLine("                AND   S1.MERCHANDISECD = M1.MERCHANDISECD(+)")
            '    .AppendLine("                AND NOT EXISTS ( SELECT 1 ")
            '    .AppendLine("                                   FROM TBL_STALLREZINFO S2 ")
            '    .AppendLine("                                  WHERE S2.DLRCD = S1.DLRCD ")
            '    .AppendLine("                                    AND S2.STRCD = S1.STRCD ")
            '    .AppendLine("                                    AND S2.REZID = S1.REZID ")
            '    .AppendLine("                                    AND (S2.STOPFLG = :STOPFLG ")
            '    .AppendLine("                                    AND S2.CANCELFLG = :CANCELFLG)) ")
            '    .AppendLine("          ) T1 ")
            '    .AppendLine(" ORDER BY T1.VISITTIMESTAMP ASC ")
            'End With

            With sql

                .AppendLine(" SELECT /* SC3100401_001 */ ")
                .AppendLine("         CONCAT(:ROWNAME, TO_CHAR(ROWNUM)) AS ROWNO ")
                .AppendLine("        ,R1.VISITSEQ ")
                .AppendLine("        ,R1.VISITTIMESTAMP ")
                .AppendLine("        ,R1.ELAPSEDTIME ")
                .AppendLine("        ,R1.VCLREGNO ")
                .AppendLine("        ,R1.SEX ")
                .AppendLine("        ,R1.NAME ")
                .AppendLine("        ,R1.TELNO ")
                .AppendLine("        ,R1.SACODE ")
                .AppendLine("        ,R1.ASSIGNSTATUS ")
                .AppendLine("        ,R1.ORDERNO ")
                .AppendLine("        ,R1.UPDATEDATE ")
                .AppendLine("        ,R1.CALLNO ")
                .AppendLine("        ,R1.CALLPLACE ")
                .AppendLine("        ,R1.CALLSTATUS ")
                .AppendLine("        ,R1.VISITNAME ")
                .AppendLine("        ,R1.VISITTELNO ")
                .AppendLine("        ,R1.SANAME ")
                .AppendLine("        ,R1.DEFAULTSANAME ")
                .AppendLine("        ,CASE TO_CHAR(R1.PLANSTARTDATE, 'YYYY/MM/DD') ")
                .AppendLine("              WHEN :PLANSTARTDATE ")
                .AppendLine("              THEN TO_CHAR(R1.PLANSTARTDATE, 'HH24:MI') ")
                .AppendLine("              ELSE TO_CHAR(R1.PLANSTARTDATE, 'MM/DD')  ")
                .AppendLine("         END AS PLANSTARTDATE ")
                .AppendLine("        ,R1.MODEL_NAME AS VEHICLENAME ")
                .AppendLine("        ,R1.MERCHANDISENAME ")

                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                .AppendLine("        ,NVL2(R2.VISIT_ID, :REPAIRORDERINFO_1, :REPAIRORDERINFO_0) AS RO_INFO_TYPE ")
                .AppendLine("        ,NVL(R1.REZID, -1) AS REZID ")
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                .AppendLine("        ,NVL(TRIM(R1.IMP_VCL_FLG), :ICON_FLAG_OFF) AS IMP_VCL_FLG ")
                '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                .AppendLine("    FROM ")
                .AppendLine("         (SELECT T1.DLRCD ")
                .AppendLine("                ,T1.STRCD ")
                .AppendLine("                ,T1.VISITSEQ ")
                .AppendLine("                ,TO_CHAR(T1.VISITTIMESTAMP, 'HH24:MI') AS VISITTIMESTAMP ")
                .AppendLine("                ,TRUNC((:VISITTIMESTAMP - T1.VISITTIMESTAMP) * (24 * 60)) AS ELAPSEDTIME ")
                .AppendLine("                ,T1.VCLREGNO ")
                .AppendLine("                ,NVL(T1.SEX, :SEX) AS SEX ")
                .AppendLine("                ,T1.NAME ")
                .AppendLine("                ,NVL(T1.MOBILE, T1.TELNO) AS TELNO ")
                .AppendLine("                ,T1.SACODE ")
                .AppendLine("                ,T1.DEFAULTSACODE ")
                .AppendLine("                ,T1.REZID ")
                .AppendLine("                ,T1.FREZID ")
                .AppendLine("                ,T1.ASSIGNSTATUS ")
                .AppendLine("                ,TRIM(T1.ORDERNO) AS ORDERNO ")
                .AppendLine("                ,T1.UPDATEDATE ")
                .AppendLine("                ,T1.CALLNO ")
                .AppendLine("                ,T1.CALLPLACE ")
                .AppendLine("                ,T1.CALLSTATUS ")
                .AppendLine("                ,T1.VISITNAME ")
                .AppendLine("                ,T1.VISITTELNO ")
                .AppendLine("                ,T2.USERNAME AS SANAME ")
                .AppendLine("                ,T3.USERNAME AS DEFAULTSANAME ")
                .AppendLine("                ,T4.VCL_ID ")
                .AppendLine("                ,CASE T4.SCHE_SVCIN_DATETIME ")
                .AppendLine("                    WHEN :MINDATE ")
                .AppendLine("                    THEN T4.SCHE_START_DATETIME_MIN ")
                .AppendLine("                    ELSE T4.SCHE_SVCIN_DATETIME  ")
                .AppendLine("                 END AS PLANSTARTDATE ")
                .AppendLine("                ,T4.JOB_DTL_ID ")
                .AppendLine("                ,T4.MERC_ID ")
                .AppendLine("                ,T4.SVC_CLASS_ID ")

                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                '.AppendLine("                ,TRIM(T4.MODEL_CD) AS MODELCODE ")
                '.AppendLine("                ,TRIM(T4.VCL_KATASHIKI) AS VCL_KATASHIKI ")
                '.AppendLine("                ,NVL(NVL(TRIM(T4.MODEL_NAME), TRIM(T1.MODEL_NAME)), TRIM(T4.NEWCST_MODEL_NAME)) AS MODEL_NAME ")
                .AppendLine("                ,T1.MODEL_NAME ")
                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

                .AppendLine("                ,T4.MERCHANDISENAME ")
                '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                .AppendLine("                ,T1.IMP_VCL_FLG ")
                '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                .AppendLine("            FROM (SELECT V1.DLRCD ")
                .AppendLine("                        ,V1.STRCD ")
                .AppendLine("                        ,V1.VISITSEQ ")
                .AppendLine("                        ,V1.VISITTIMESTAMP ")

                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                '.AppendLine("                        ,V1.VCLREGNO ")
                .AppendLine("                        ,NVL(TRIM(V4.REG_NUM), V1.VCLREGNO ) AS VCLREGNO ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END

                .AppendLine("                        ,V1.SEX ")
                .AppendLine("                        ,V1.NAME ")
                .AppendLine("                        ,V1.MOBILE ")
                .AppendLine("                        ,V1.TELNO ")
                .AppendLine("                        ,V1.SACODE ")
                .AppendLine("                        ,V1.DEFAULTSACODE ")
                .AppendLine("                        ,V1.REZID ")
                .AppendLine("                        ,V1.FREZID ")
                .AppendLine("                        ,V1.ASSIGNSTATUS ")
                .AppendLine("                        ,V1.ORDERNO ")
                .AppendLine("                        ,V1.UPDATEDATE ")
                .AppendLine("                        ,V1.CALLNO ")
                .AppendLine("                        ,V1.CALLPLACE ")
                .AppendLine("                        ,V1.CALLSTATUS ")
                .AppendLine("                        ,V1.VISITNAME ")
                .AppendLine("                        ,V1.VISITTELNO  ")

                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                '.AppendLine("                        ,TRIM(V3.MODEL_NAME) AS MODEL_NAME ")
                .AppendLine("                        ,NVL(TRIM(V3.MODEL_NAME),TRIM(V5.NEWCST_MODEL_NAME)) AS MODEL_NAME ")
                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

                '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                .AppendLine("                        ,V4.IMP_VCL_FLG ")
                '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END
                .AppendLine("                    FROM TBL_SERVICE_VISIT_MANAGEMENT V1 ")

                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                '.AppendLine("                        ,TB_M_KATASHIKI V2 ")
                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

                .AppendLine("                        ,TB_M_MODEL V3 ")

                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                .AppendLine("                        ,TB_M_VEHICLE_DLR V4 ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END

                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                .AppendLine("                        ,TB_M_VEHICLE V5 ")

                '.AppendLine("                   WHERE V1.MODELCODE = V2.VCL_KATASHIKI(+) ")
                '.AppendLine("                     AND V2.MODEL_CD = V3.MODEL_CD(+) ")
                .AppendLine("                   WHERE V1.VCL_ID = V5.VCL_ID(+) ")
                .AppendLine("                     AND V5.MODEL_CD = V3.MODEL_CD(+) ")
                '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

                .AppendLine("                     AND V1.DLRCD = :DLRCD ")
                .AppendLine("                     AND V1.STRCD = :STRCD ")
                .AppendLine("                     AND V1.VISITTIMESTAMP ")
                .AppendLine("                 BETWEEN TRUNC(:VISITTIMESTAMP) ")
                .AppendLine("                     AND TRUNC(:VISITTIMESTAMP) + 86399/86400 ")
                .AppendLine("                     AND V1.ASSIGNSTATUS IN (:ASSIGNSTATUS_RECEPTION, :ASSIGNSTATUS_WAIT, :ASSIGNSTATUS_FIN) ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する START
                '.AppendLine("                     AND V1.CALLSTATUS IN (:CALLSTATUS_NOCALL, :CALLSTATUS_CALLING)) T1 ")
                .AppendLine("                     AND V1.CALLSTATUS IN (:CALLSTATUS_NOCALL, :CALLSTATUS_CALLING) ")
                .AppendLine("                     AND V1.VCL_ID = V4.VCL_ID(+) ")
                .AppendLine("                     AND V4.DLR_CD(+) = :DLRCD ")
                .AppendLine("                 ) T1 ")
                '2015/11/11 TM 皆川 TR-SVT-TMT-20150929-001対応 車両登録番号を販売店車両から優先して取得する END

                .AppendLine("                ,TBL_USERS T2 ")
                .AppendLine("                ,TBL_USERS T3 ")
                .AppendLine("                ,(SELECT S1.SVCIN_ID ")
                .AppendLine("                        ,S1.VCL_ID ")
                .AppendLine("                        ,S1.SCHE_SVCIN_DATETIME ")
                .AppendLine("                        ,S2.JOB_DTL_ID ")
                .AppendLine("                        ,S2.MERC_ID ")
                .AppendLine("                        ,S2.SVC_CLASS_ID ")
                .AppendLine("                        ,S7.SCHE_START_DATETIME_MIN ")

                '2017/03/23 NSK 竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                '.AppendLine("                        ,S3.MODEL_CD ")
                '.AppendLine("                        ,S3.VCL_KATASHIKI ")
                '.AppendLine("                        ,S3.NEWCST_MODEL_NAME ")
                '.AppendLine("                        ,S4.MODEL_NAME ")
                '.AppendLine("                        ,NVL(CONCAT(TRIM(S5.UPPER_DISP), TRIM(S5.LOWER_DISP)),NVL(S6.SVC_CLASS_NAME,S6.SVC_CLASS_NAME_ENG)) AS MERCHANDISENAME ")
                .AppendLine("                        ,NVL(CONCAT(TRIM(S5.UPPER_DISP), TRIM(S5.LOWER_DISP)),NVL(TRIM(S6.SVC_CLASS_NAME),TRIM(S6.SVC_CLASS_NAME_ENG))) AS MERCHANDISENAME ")
                '2017/03/23 NSK 竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

                .AppendLine("                    FROM TB_T_SERVICEIN S1 ")
                .AppendLine("                        ,TB_T_JOB_DTL S2 ")

                '2017/03/23 NSK 竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                '.AppendLine("                        ,TB_M_VEHICLE S3 ")
                '.AppendLine("                        ,TB_M_MODEL S4 ")
                '2017/03/23 NSK 竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

                .AppendLine("                        ,TB_M_MERCHANDISE S5 ")
                .AppendLine("                        ,TB_M_SERVICE_CLASS S6 ")
                .AppendLine("                        ,(SELECT M2.SVCIN_ID ")
                .AppendLine("                                ,MIN(M3.JOB_DTL_ID) AS JOB_DTL_ID ")
                .AppendLine("                                ,MIN(M4.SCHE_START_DATETIME) AS SCHE_START_DATETIME_MIN ")
                .AppendLine("                            FROM TBL_SERVICE_VISIT_MANAGEMENT M1 ")
                .AppendLine("                                ,TB_T_SERVICEIN M2 ")
                .AppendLine("                                ,TB_T_JOB_DTL M3 ")
                .AppendLine("                                ,TB_T_STALL_USE M4 ")
                .AppendLine("                           WHERE M1.FREZID = M2.SVCIN_ID ")
                .AppendLine("                             AND M2.SVCIN_ID = M3.SVCIN_ID ")
                .AppendLine("                             AND M3.JOB_DTL_ID = M4.JOB_DTL_ID ")
                .AppendLine("                             AND M1.DLRCD = :DLRCD ")
                .AppendLine("                             AND M1.STRCD = :STRCD ")
                .AppendLine("                             AND M1.VISITTIMESTAMP ")
                .AppendLine("                         BETWEEN TRUNC(:VISITTIMESTAMP) ")
                .AppendLine("                             AND TRUNC(:VISITTIMESTAMP) + 86399/86400 ")
                .AppendLine("                             AND M1.ASSIGNSTATUS IN (:ASSIGNSTATUS_RECEPTION, :ASSIGNSTATUS_WAIT, :ASSIGNSTATUS_FIN) ")
                .AppendLine("                             AND M1.CALLSTATUS IN (:CALLSTATUS_NOCALL, :CALLSTATUS_CALLING) ")
                .AppendLine("                             AND M2.DLR_CD = :DLRCD ")
                .AppendLine("                             AND M2.BRN_CD = :STRCD ")
                .AppendLine("                             AND M3.DLR_CD = :DLRCD ")
                .AppendLine("                             AND M3.BRN_CD = :STRCD ")
                .AppendLine("                             AND M3.CANCEL_FLG = :CANCELFLG ")
                .AppendLine("                             AND M4.DLR_CD = :DLRCD ")
                .AppendLine("                             AND M4.BRN_CD = :STRCD ")
                .AppendLine("                        GROUP BY M2.SVCIN_ID ")
                .AppendLine("                            ) S7 ")
                .AppendLine("                         WHERE S1.SVCIN_ID = S2.SVCIN_ID ")
                .AppendLine("                           AND S2.JOB_DTL_ID = S7.JOB_DTL_ID ")

                '2017/03/23 NSK 竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                '.AppendLine("                           AND S1.VCL_ID = S3.VCL_ID(+) ")
                '.AppendLine("                           AND S3.MODEL_CD = S4.MODEL_CD(+) ")
                '2017/03/23 NSK 竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

                .AppendLine("                           AND S2.MERC_ID = S5.MERC_ID(+) ")
                .AppendLine("                           AND S2.SVC_CLASS_ID = S6.SVC_CLASS_ID(+) ")
                .AppendLine("                           AND NOT EXISTS (SELECT 1  ")
                .AppendLine("                                             FROM TB_T_SERVICEIN M5 ")
                .AppendLine("                                            WHERE M5.SVCIN_ID = S1.SVCIN_ID ")
                .AppendLine("                                              AND M5.SVC_STATUS = :SVC_STATUS) ")
                .AppendLine("                 ) T4 ")
                .AppendLine("           WHERE T1.SACODE = T2.ACCOUNT(+) ")
                .AppendLine("             AND T1.DEFAULTSACODE = T3.ACCOUNT(+) ")
                .AppendLine("             AND T1.FREZID = T4.SVCIN_ID(+) ")
                .AppendLine("             AND T2.DLRCD(+) = :CHARDLRCD ")
                .AppendLine("             AND T2.STRCD(+) = :CHARSTRCD ")
                .AppendLine("             AND T3.DLRCD(+) = :CHARDLRCD ")
                .AppendLine("             AND T3.STRCD(+) = :CHARSTRCD ")
                .AppendLine("          ) R1 ")

                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                .AppendLine("         ,(SELECT S1.VISIT_ID ")
                .AppendLine("                 ,COUNT(S1.VISIT_ID) ")
                .AppendLine("             FROM TB_T_RO_INFO S1 ")
                .AppendLine("            WHERE S1.RO_STATUS <> :RO_STATUS_99 ")
                .AppendLine("            GROUP BY S1.VISIT_ID) R2 ")
                .AppendLine("    WHERE R1.VISITSEQ = R2.VISIT_ID(+) ")
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                .AppendLine(" ORDER BY R1.VISITTIMESTAMP ASC ")

            End With

            '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

            Using query As New DBSelectQuery(Of SC3100401DataSet.ReceptionListDataTable)("SC3100401_001")

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("ROWNAME", OracleDbType.Char, LisrRowName)
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                query.AddParameterWithTypeValue("REPAIRORDERINFO_1", OracleDbType.NVarchar2, RepairOrderInfoExist)
                query.AddParameterWithTypeValue("REPAIRORDERINFO_0", OracleDbType.NVarchar2, RepairOrderInfoNone)
                query.AddParameterWithTypeValue("RO_STATUS_99", OracleDbType.NVarchar2, "99")
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inStoreCode)
                query.AddParameterWithTypeValue("SEX", OracleDbType.NVarchar2, Male)
                query.AddParameterWithTypeValue("VISITTIMESTAMP", OracleDbType.Date, inPresentTime)
                query.AddParameterWithTypeValue("PLANSTARTDATE", OracleDbType.Char, String.Format(CultureInfo.InvariantCulture, "{0:yyyy/MM/dd}", inPresentTime))
                query.AddParameterWithTypeValue("ASSIGNSTATUS_RECEPTION", OracleDbType.NVarchar2, NonAssign)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_WAIT", OracleDbType.NVarchar2, AssignWait)
                query.AddParameterWithTypeValue("ASSIGNSTATUS_FIN", OracleDbType.NVarchar2, AssignFinish)
                query.AddParameterWithTypeValue("CALLSTATUS_NOCALL", OracleDbType.NVarchar2, NonCall)
                query.AddParameterWithTypeValue("CALLSTATUS_CALLING", OracleDbType.NVarchar2, Calling)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("STOPFLG", OracleDbType.Char, StopFlag)
                'query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.Char, CancelFlag)

                query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.NVarchar2, CancelFlagEffective)
                query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, StatusCancel)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))

                query.AddParameterWithTypeValue("CHARDLRCD", OracleDbType.Char, inDealerCode)
                query.AddParameterWithTypeValue("CHARSTRCD", OracleDbType.Char, inStoreCode)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 START
                query.AddParameterWithTypeValue("ICON_FLAG_OFF", OracleDbType.NVarchar2, IconFlagOff)
                '2018/06/19 NSK 可児 TKM Next Gen e-CRB Project Application development Block B-1  Pマーク、L マークなどを表示 END

                '実行
                Dim dt As SC3100401DataSet.ReceptionListDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using
        End Function

        ''' <summary>
        ''' SA一覧情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <returns>SA一覧情報返却</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
        ''' 2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない
        ''' </history>
        Public Function GetDBServiceAdvisorList(ByVal inDealerCode As String, _
                                                ByVal inStoreCode As String, _
                                                ByVal inPresentTime As Date) As SC3100401DataSet.ServiceAdviserInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} DEALERCODE:{2} STORECODE:{3} PRESENTTIME:{4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inStoreCode, inPresentTime))

            Using query As New DBSelectQuery(Of SC3100401DataSet.ServiceAdviserInfoDataTable)("SC3100401_002")
                Dim sql As New StringBuilder                                                    ' SQL文格納

                With sql
                    .AppendLine("SELECT /* SC3100401_002 */")
                    .AppendLine("       TO_CHAR(ROWNUM) AS ROWNO")
                    .AppendLine("     , T3.ACCOUNT")
                    .AppendLine("     , T3.USERNAME")
                    .AppendLine("     , DECODE(T3.PRESENCECATEGORY, :PRESENCECATEGORYSTANDBY, '0'")
                    .AppendLine("                                 , :PRESENCECATEGORYLEAVING, '1') AS PRESENCECATEGORY")

                    '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない START
                    .AppendLine("     , NVL(T4.COUNT_WORKBEF, 0) AS COUNT_WORKBEF")
                    .AppendLine("     , NVL(T4.COUNT_WORK, 0) AS COUNT_WORK")
                    '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない END

                    .AppendLine("  FROM (")
                    .AppendLine("    SELECT T1.ACCOUNT")
                    .AppendLine("         , T1.USERNAME")
                    .AppendLine("         , T1.PRESENCECATEGORY")
                    .AppendLine("      FROM TBL_USERS T1")
                    .AppendLine("     WHERE T1.DLRCD = :DLRCD")
                    .AppendLine("       AND T1.STRCD = :STRCD")
                    .AppendLine("       AND T1.OPERATIONCODE = :OPERATIONCODE")
                    .AppendLine("       AND T1.DELFLG = :DELFLG")
                    .AppendLine("       AND T1.PRESENCECATEGORY IN (:PRESENCECATEGORYSTANDBY, ")
                    .AppendLine("                                   :PRESENCECATEGORYLEAVING) ")
                    .AppendLine("       AND T1.PRESENCEDETAIL = :PRESENCEDETAIL ")
                    .AppendLine("         ) T3")
                    .AppendLine("     , (")

                    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                    '.AppendLine("    SELECT T2.SACODE")
                    '.AppendLine("         , COUNT(T2.SACODE) AS SACOUNT")
                    '.AppendLine("      FROM TBL_SERVICE_VISIT_MANAGEMENT T2")

                    ' ''2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                    ''.AppendLine("     WHERE T2.DLRCD = :DLRCD")
                    ''.AppendLine("       AND T2.STRCD = :STRCD")

                    '.AppendLine("     WHERE T2.DLRCD = :DLR_CD")
                    '.AppendLine("       AND T2.STRCD = :STR_CD")

                    ' ''2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                    '.AppendLine("       AND T2.ASSIGNSTATUS = :ASSIGNSTATUS")
                    '.AppendLine("       AND T2.VISITTIMESTAMP >= TRUNC(:VISITTIMESTAMP)")
                    '.AppendLine("       AND T2.ORDERNO IS NULL ")
                    '.AppendLine("  GROUP BY T2.SACODE ")

                    .AppendLine("    SELECT T2.SACODE")

                    '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない START
                    .AppendLine("          ,SUM(NVL2(T3.VISIT_ID, 0, 1)) AS COUNT_WORKBEF")
                    .AppendLine("          ,SUM(NVL2(T3.VISIT_ID, 1, 0)) AS COUNT_WORK")
                    '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない END

                    .AppendLine("      FROM TBL_SERVICE_VISIT_MANAGEMENT T2")
                    .AppendLine("          ,(SELECT Y1.VISIT_ID")
                    .AppendLine("                  ,COUNT(Y1.VISIT_ID)")
                    .AppendLine("              FROM TB_T_RO_INFO Y1")
                    .AppendLine("             GROUP BY Y1.VISIT_ID) T3")
                    .AppendLine("     WHERE T2.VISITSEQ = T3.VISIT_ID(+)")
                    .AppendLine("       AND T2.DLRCD = :DLR_CD")
                    .AppendLine("       AND T2.STRCD = :STR_CD")
                    .AppendLine("       AND T2.ASSIGNSTATUS = :ASSIGNSTATUS")
                    .AppendLine("       AND T2.VISITTIMESTAMP >= TRUNC(:VISITTIMESTAMP)")
                    .AppendLine("       AND T2.ORDERNO IS NULL")
                    .AppendLine("  GROUP BY T2.SACODE ")

                    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                    .AppendLine("         ) T4")
                    .AppendLine(" WHERE T3.ACCOUNT = T4.SACODE (+)")
                    .AppendLine(" ORDER BY T3.ACCOUNT")

                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Int64, CType(Operation.SA, Long))
                query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlag)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("STR_CD", OracleDbType.NVarchar2, inStoreCode)

                'query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.Char, AssignFinish)
                query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, AssignFinish)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                query.AddParameterWithTypeValue("VISITTIMESTAMP", OracleDbType.Date, inPresentTime)

                query.AddParameterWithTypeValue("PRESENCECATEGORYSTANDBY", OracleDbType.Char, PresenceCategory.Standby)
                query.AddParameterWithTypeValue("PRESENCECATEGORYLEAVING", OracleDbType.Char, PresenceCategory.Suspend)
                query.AddParameterWithTypeValue("PRESENCEDETAIL", OracleDbType.Char, PresenceDetail)

                '実行
                Dim dt As SC3100401DataSet.ServiceAdviserInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using
        End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 STAT

        ' ''' <summary>
        ' ''' 自社客車両情報
        ' ''' </summary>
        ' ''' <param name="inVehicleNo">車両登録No</param>
        ' ''' <param name="inChangeVehicleNo">変換後車輌登録No</param>
        ' ''' <param name="inDealerCode">ログイン情報</param>
        ' ''' <returns>自社客車両情報返却</returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' ''' </history>
        'Public Function GetDBCustomerInfo(ByVal inVehicleNo As String, _
        '                                  ByVal inChangeVehicleNo As String, _
        '                                  ByVal inDealerCode As String) _
        '                                  As SC3100401DataSet.ChageRegNoInfoDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} DEALERCODE:{2} STORECODE:{3} PRESENTTIME:{4}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inVehicleNo, inChangeVehicleNo, inDealerCode))

        '    Using query As New DBSelectQuery(Of SC3100401DataSet.ChageRegNoInfoDataTable)("SC3100401_003")

        '        Dim sql As New StringBuilder      ' SQL文格納
        '        With sql
        '            .AppendLine("SELECT /* SC3100401_003 */")
        '            .AppendLine("       T1.VIN ")
        '            .AppendLine("     , T1.VCLREGNO ")
        '            .AppendLine("     , RTRIM(T1.MODELCD) AS MODELCODE ")
        '            .AppendLine("     , T1.SACODE AS DEFAULTSACODE")
        '            .AppendLine("     , RTRIM(T2.CUSTCD) AS CUSTCD ")
        '            .AppendLine("     , T2.STAFFCD ")
        '            .AppendLine("     , T2.NAME ")
        '            .AppendLine("     , NVL(TRIM(T2.SEX), :SEX) AS SEX ")
        '            .AppendLine("     , T2.TELNO ")
        '            .AppendLine("     , T2.MOBILE ")
        '            .AppendLine("  FROM TBLORG_VCLINFO T1 ")
        '            .AppendLine("     , TBLORG_CUSTOMER T2 ")
        '            .AppendLine(" WHERE T1.ORIGINALID = T2.ORIGINALID ")
        '            .AppendLine("   AND T1.DELFLG = :DELFLG ")
        '            .AppendLine("   AND T1.DLRCD = :DLRCD ")


        '            '変換後車両登録Noのチェック
        '            If String.IsNullOrEmpty(inChangeVehicleNo) Then
        '                '変換後車両登録No無し

        '                '変換前の車両登録No両方で検索
        '                .AppendLine("   AND T1.VCLREGNO = :VCLREGNO ")
        '            Else
        '                '変換後車両登録No有り

        '                '変換前と変換後の車両登録No両方で検索
        '                .AppendLine("   AND T1.VCLREGNO IN (:VCLREGNO, :CHANGEVCLREGNO) ")

        '                query.AddParameterWithTypeValue("CHANGEVCLREGNO", OracleDbType.NVarchar2, inChangeVehicleNo)
        '            End If


        '            .AppendLine("   AND T2.DELFLG = :DELFLG ")
        '        End With

        '        query.CommandText = sql.ToString()

        '        query.AddParameterWithTypeValue("SEX", OracleDbType.Char, Male)
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
        '        query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, inVehicleNo)
        '        query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlag)

        '        '実行
        '        Dim dt As SC3100401DataSet.ChageRegNoInfoDataTable = query.GetData()

        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} END COUNT = {2}" _
        '                    , Me.GetType.ToString _
        '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                    , dt.Count))

        '        Return dt

        '    End Using
        'End Function

        ' ''' <summary>
        ' ''' ストール予約情報の取得
        ' ''' </summary>
        ' ''' <param name="inRegNo">車両登録No</param>
        ' ''' <param name="inchangeRegNo">変換後車輌登録No</param>
        ' ''' <param name="inDealerCode">販売店コード</param>
        ' ''' <param name="inStoreCode">店舗コード</param>
        ' ''' <param name="inPresentTime">現在時間</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history></history>
        'Public Function GetDBStallRezInfo(ByVal inRegNo As String, _
        '                                  ByVal inChangeRegNo As String, _
        '                                  ByVal inDealerCode As String, _
        '                                  ByVal inStoreCode As String, _
        '                                  ByVal inPresentTime As Date) _
        '                                  As SC3100401DataSet.StallRezInfoDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} REGNO:{2} CHANGEREGNO:{3} DEALERCODE:{4} STORECODE:{5} PRESENTTIME:{6}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inRegNo, inChangeRegNo, inDealerCode, inStoreCode, inPresentTime))

        '    Using query As New DBSelectQuery(Of SC3100401DataSet.StallRezInfoDataTable)("SC3100401_004")

        '        Dim sql As New StringBuilder      ' SQL文格納
        '        With sql
        '            .AppendLine("  SELECT /* SC3100401_004 */")
        '            .AppendLine("         T3.REZID")
        '            .AppendLine("       , T3.CUSTCD ")
        '            .AppendLine("       , T3.VCLREGNO ")
        '            .AppendLine("       , T3.CUSTOMERNAME ")
        '            .AppendLine("       , T3.TELNO ")
        '            .AppendLine("       , T3.MOBILE ")
        '            .AppendLine("       , T3.CUSTSEGMENT ")
        '            .AppendLine("       , T3.MODELCODE ")
        '            .AppendLine("       , T3.SERVICECODE ")
        '            .AppendLine("       , T3.ACCOUNT_PLAN ")
        '            .AppendLine("       , T3.ORDERNO ")
        '            .AppendLine("    FROM ")
        '            .AppendLine("         ( ")
        '            .AppendLine("           SELECT ")
        '            .AppendLine("                  T1.DLRCD")
        '            .AppendLine("                , T1.REZID")
        '            .AppendLine("                , TRIM(T1.CUSTCD) AS CUSTCD ")
        '            .AppendLine("                , T1.VCLREGNO ")
        '            .AppendLine("                , TRIM(T1.CUSTOMERNAME) AS CUSTOMERNAME ")
        '            .AppendLine("                , TRIM(T1.TELNO) AS TELNO ")
        '            .AppendLine("                , TRIM(T1.MOBILE) AS MOBILE ")
        '            .AppendLine("                , CASE ")
        '            .AppendLine("                      WHEN T1.CUSTOMERFLAG = :STALLREZMYCUST THEN :MYCUSTOMER ")
        '            .AppendLine("                      WHEN T1.CUSTOMERFLAG = :STALLREZNEWCUST THEN :NEWCUSTOMER ")
        '            .AppendLine("                      ELSE :NEWCUSTOMER")
        '            .AppendLine("                  END AS CUSTSEGMENT")
        '            .AppendLine("                , TRIM(T1.MODELCODE) AS MODELCODE ")
        '            .AppendLine("                , CASE ")
        '            .AppendLine("                      WHEN T1.SERVICECODE_S = :SERVICECODE_BP THEN :SERVICECODE_BP ")
        '            .AppendLine("                      WHEN NVL( ")
        '            .AppendLine("                                (SELECT ")
        '            .AppendLine("                                        COUNT(1) AS SERVICE_COUNT ")
        '            .AppendLine("                                   FROM TBL_MERCHANDISEMST ")
        '            .AppendLine("                                  WHERE DLRCD = T1.DLRCD ")
        '            .AppendLine("                                    AND SERVICECODE = T1.SERVICECODE_S ")
        '            .AppendLine("                                    AND DELFLG = :DELFLG ")
        '            .AppendLine("                               GROUP BY SERVICECODE)")
        '            .AppendLine("                              , 0) > 0 THEN :SERVICECODE_GENERAL ")
        '            .AppendLine("                      ELSE :SERVICECODE_REGULAR ")
        '            .AppendLine("                  END AS SERVICECODE")
        '            .AppendLine("                , TRIM(T1.ACCOUNT_PLAN) AS ACCOUNT_PLAN")
        '            .AppendLine("                , TRIM(T1.ORDERNO) AS ORDERNO")
        '            .AppendLine("             FROM TBL_STALLREZINFO T1")
        '            .AppendLine("            WHERE T1.DLRCD = :DLRCD")
        '            .AppendLine("              AND T1.STRCD = :STRCD")


        '            '変換後車両登録Noのチェック
        '            If String.IsNullOrEmpty(inChangeRegNo) Then
        '                '変換後車両登録No無し

        '                '変換前の車両登録No両方で検索
        '                .AppendLine("           AND T1.VCLREGNO = :VCLREGNO ")
        '            Else
        '                '変換後車両登録No有り

        '                '変換前と変換後の車両登録No両方で検索
        '                .AppendLine("           AND T1.VCLREGNO IN (:VCLREGNO, :CHANGEVCLREGNO) ")

        '                query.AddParameterWithTypeValue("CHANGEVCLREGNO", OracleDbType.NVarchar2, inChangeRegNo)
        '            End If


        '            .AppendLine("           AND TO_CHAR(T1.STARTTIME, 'YYYYMMDD') = :STARTTIME")
        '            .AppendLine("           AND T1.STATUS IN (:STATUSCOMMITTED, :STATUSPROPOSEDD)")
        '            .AppendLine("           AND NOT (T1.SERVICECODE_S IN (:SERVICECODE_TAKE, :SERVICECODE_DELIVERY))")
        '            .AppendLine("           AND (T1.PREZID = T1.REZID OR T1.PREZID IS NULL)")
        '            .AppendLine("           AND NOT EXISTS (")
        '            .AppendLine("               SELECT 1")
        '            .AppendLine("                 FROM TBL_STALLREZINFO ")
        '            .AppendLine("                WHERE DLRCD = T1.DLRCD")
        '            .AppendLine("                  AND STRCD = T1.STRCD")
        '            .AppendLine("                  AND REZID = T1.REZID")
        '            .AppendLine("                  AND STOPFLG IN (:STOPFLG, :WALKIN)")
        '            .AppendLine("                  AND CANCELFLG = :CANCELFLG")
        '            .AppendLine("               )")
        '            .AppendLine("       ORDER BY T1.STARTTIME")
        '            .AppendLine("     ) T3")
        '        End With
        '        query.CommandText = sql.ToString()

        '        'バインド変数
        '        query.AddParameterWithTypeValue("STALLREZMYCUST", OracleDbType.Char, StallRezMyCustomer)
        '        query.AddParameterWithTypeValue("MYCUSTOMER", OracleDbType.Char, CustSegmentMyCustomer)
        '        query.AddParameterWithTypeValue("STALLREZNEWCUST", OracleDbType.Char, StallRezNewCustomer)
        '        query.AddParameterWithTypeValue("NEWCUSTOMER", OracleDbType.Char, CustSegmentNewCustomer)
        '        query.AddParameterWithTypeValue("SERVICECODE_BP", OracleDbType.Char, ServiCodeBP)
        '        query.AddParameterWithTypeValue("SERVICECODE_GENERAL", OracleDbType.Char, ServiCodeGeneral)
        '        query.AddParameterWithTypeValue("SERVICECODE_REGULAR", OracleDbType.Char, ServiCodeRegular)
        '        query.AddParameterWithTypeValue("DELFLG", OracleDbType.Char, DeleteFlag)
        '        query.AddParameterWithTypeValue("STATUSCOMMITTED", OracleDbType.Int64, CommittedResource)
        '        query.AddParameterWithTypeValue("STATUSPROPOSEDD", OracleDbType.Int64, ProposedResource)
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
        '        query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, inRegNo)
        '        query.AddParameterWithTypeValue("STARTTIME", OracleDbType.Char, String.Format(CultureInfo.InvariantCulture, "{0:yyyyMMdd}", inPresentTime))
        '        query.AddParameterWithTypeValue("SERVICECODE_TAKE", OracleDbType.Char, ServiCodeTaking)
        '        query.AddParameterWithTypeValue("SERVICECODE_DELIVERY", OracleDbType.Char, ServiCodeDelivery)
        '        query.AddParameterWithTypeValue("STOPFLG", OracleDbType.Char, StopFlag)
        '        query.AddParameterWithTypeValue("WALKIN", OracleDbType.Char, StopFlagWalkIn)
        '        query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.Char, CancelFlag)

        '        '実行
        '        Dim dt As SC3100401DataSet.StallRezInfoDataTable = query.GetData()

        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} END COUNT = {2}" _
        '                    , Me.GetType.ToString _
        '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                    , dt.Count))

        '        Return dt

        '    End Using
        'End Function

        ' ''' <summary>
        ' ''' ストール実績の取得
        ' ''' </summary>
        ' ''' <param name="inRezId">予約ID</param>
        ' ''' <param name="inDealerCode">販売店コード</param>
        ' ''' <param name="inStoreCode">店舗コード</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history></history>
        'Public Function GetDBStallProcess(ByVal inRezId As Long, _
        '                                  ByVal inDealerCode As String, _
        '                                  ByVal inStoreCode As String) _
        '                                  As SC3100401DataSet.StallProcessDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} REZID:{2} DEALERCODE:{3} STORECODE:{4}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inRezId, inDealerCode, inStoreCode))

        '    Using query As New DBSelectQuery(Of SC3100401DataSet.StallProcessDataTable)("SC3100401_005")

        '        Dim sql As New StringBuilder      ' SQL文格納
        '        With sql
        '            .AppendLine("SELECT /* SC3100401_005 */")
        '            .AppendLine("       T1.REZID")
        '            .AppendLine("     , T1.DSEQNO")
        '            .AppendLine("     , T1.SEQNO")
        '            .AppendLine("     , T1.RESULT_STATUS")
        '            .AppendLine("  FROM (")
        '            .AppendLine("       SELECT")
        '            .AppendLine("              REZID")
        '            .AppendLine("            , DSEQNO")
        '            .AppendLine("            , SEQNO")
        '            .AppendLine("            , RESULT_STATUS")
        '            .AppendLine("            , ROW_NUMBER() OVER (ORDER BY DSEQNO DESC, SEQNO DESC) RNUM")
        '            .AppendLine("         FROM TBL_STALLPROCESS")
        '            .AppendLine("        WHERE DLRCD = :DLRCD")
        '            .AppendLine("          AND STRCD = :STRCD")
        '            .AppendLine("          AND REZID = :REZID")
        '            .AppendLine("  ) T1")
        '            .AppendLine("  WHERE T1.RNUM = 1")
        '        End With
        '        query.CommandText = sql.ToString()

        '        'バインド変数
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
        '        query.AddParameterWithTypeValue("REZID", OracleDbType.Int64, inRezId)

        '        '実行
        '        Dim dt As SC3100401DataSet.StallProcessDataTable = query.GetData()

        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} END COUNT = {2}" _
        '                    , Me.GetType.ToString _
        '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                    , dt.Count))

        '        Return dt

        '    End Using
        'End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        ''' <summary>
        ''' SA振当用来店管理情報の取得
        ''' </summary>
        ''' <param name="inVisitSeq">来店実績連番</param>
        ''' <param name="inUpDateTime">更新日時</param>
        ''' <returns>来店管理情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
        ''' </history>
        Public Function GetDBVisitInfo(ByVal inVisitSeq As Long, _
                                       ByVal inUpDateTime As Date) _
                                       As SC3100401DataSet.VisitInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} VISITSEQ:{2} UPDATEDATE:{3}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inVisitSeq, inUpDateTime))

            Using query As New DBSelectQuery(Of SC3100401DataSet.VisitInfoDataTable)("SC3100401_006")

                Dim sql As New StringBuilder      ' SQL文格納
                With sql
                    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                    '.AppendLine("SELECT /* SC3100401_006 */")
                    '.AppendLine("       VISITSEQ")
                    '.AppendLine("     , VISITTIMESTAMP")
                    '.AppendLine("     , VCLREGNO")
                    '.AppendLine("     , CUSTSEGMENT")
                    '.AppendLine("     , VIN")
                    '.AppendLine("     , MODELCODE")
                    '.AppendLine("     , NAME")
                    '.AppendLine("     , TELNO")
                    '.AppendLine("     , MOBILE")
                    '.AppendLine("     , DEFAULTSACODE")
                    '.AppendLine("     , SACODE AS BEFORESACODE")
                    '.AppendLine("     , SERVICECODE")
                    '.AppendLine("     , NVL(REZID, -1) AS REZID")
                    '.AppendLine("     , PARKINGCODE")
                    '.AppendLine("     , ASSIGNSTATUS")
                    '.AppendLine("     , ORDERNO")
                    '.AppendLine("  FROM TBL_SERVICE_VISIT_MANAGEMENT")
                    '.AppendLine(" WHERE VISITSEQ = :VISITSEQ")
                    '.AppendLine("   AND UPDATEDATE = :UPDATEDATE")

                    .AppendLine("SELECT /* SC3100401_006 */ ")
                    .AppendLine("       Q1.VISITSEQ ")
                    .AppendLine("      ,Q1.VISITTIMESTAMP ")
                    .AppendLine("      ,Q1.VCLREGNO ")
                    .AppendLine("      ,Q1.CUSTSEGMENT ")
                    .AppendLine("      ,Q1.VIN ")
                    .AppendLine("      ,Q1.MODELCODE ")

                    '2015/12/17 TM 浅野 12月号口配信に向けた緊急対応 START
                    .AppendLine("      ,Q3.CST_NAME AS NAME ")
                    '.AppendLine("      ,Q1.NAME ")
                    '2015/12/17 TM 浅野 12月号口配信に向けた緊急対応 END

                    .AppendLine("      ,Q1.TELNO ")
                    .AppendLine("      ,Q1.MOBILE ")
                    .AppendLine("      ,Q1.DEFAULTSACODE ")
                    .AppendLine("      ,Q1.SACODE AS BEFORESACODE ")
                    .AppendLine("      ,Q1.SERVICECODE ")
                    .AppendLine("      ,NVL(Q1.REZID, -1) AS REZID ")
                    .AppendLine("      ,Q1.PARKINGCODE ")
                    .AppendLine("      ,Q1.ASSIGNSTATUS ")
                    .AppendLine("      ,Q1.ORDERNO ")
                    .AppendLine("      ,Q1.DMSID ")
                    .AppendLine("      ,Q2.SCHE_START_DATETIME_MIN AS START_DATETIME ")
                    .AppendLine("      ,Q2.SCHE_END_DATETIME_MAX AS END_DATETIME ")
                    .AppendLine("      ,Q2.MERCHANDISENAME ")
                    .AppendLine("      ,Q3.POSITION_TYPE ")
                    .AppendLine("      ,Q3.NAMETITLE_NAME ")
                    .AppendLine("  FROM TBL_SERVICE_VISIT_MANAGEMENT Q1 ")
                    .AppendLine("      ,(SELECT S1.SVCIN_ID ")
                    .AppendLine("              ,S5.SCHE_START_DATETIME_MIN ")
                    .AppendLine("              ,S5.SCHE_END_DATETIME_MAX ")
                    '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                    '.AppendLine("              ,NVL(CONCAT(TRIM(S3.UPPER_DISP), TRIM(S3.LOWER_DISP)),NVL(S4.SVC_CLASS_NAME,S4.SVC_CLASS_NAME_ENG)) AS MERCHANDISENAME ")
                    .AppendLine("              ,NVL(CONCAT(TRIM(S3.UPPER_DISP), TRIM(S3.LOWER_DISP)),NVL(TRIM(S4.SVC_CLASS_NAME),TRIM(S4.SVC_CLASS_NAME_ENG))) AS MERCHANDISENAME ")
                    '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END
                    .AppendLine("          FROM TB_T_SERVICEIN S1 ")
                    .AppendLine("              ,TB_T_JOB_DTL S2 ")
                    .AppendLine("              ,TB_M_MERCHANDISE S3 ")
                    .AppendLine("              ,TB_M_SERVICE_CLASS S4 ")
                    .AppendLine("              ,(SELECT M2.SVCIN_ID ")
                    .AppendLine("                      ,MIN(M3.JOB_DTL_ID) AS JOB_DTL_ID ")
                    .AppendLine("                      ,MIN(M4.SCHE_START_DATETIME) AS SCHE_START_DATETIME_MIN ")
                    .AppendLine("                      ,MAX(M4.SCHE_END_DATETIME) AS SCHE_END_DATETIME_MAX ")
                    .AppendLine("                  FROM TBL_SERVICE_VISIT_MANAGEMENT M1 ")
                    .AppendLine("                      ,TB_T_SERVICEIN M2 ")
                    .AppendLine("                      ,TB_T_JOB_DTL M3 ")
                    .AppendLine("                      ,TB_T_STALL_USE M4 ")
                    .AppendLine("                 WHERE M1.FREZID = M2.SVCIN_ID ")
                    .AppendLine("                   AND M2.SVCIN_ID = M3.SVCIN_ID ")
                    .AppendLine("                   AND M3.JOB_DTL_ID = M4.JOB_DTL_ID ")
                    .AppendLine("                   AND M1.VISITSEQ = :VISITSEQ ")
                    .AppendLine("                   AND M3.CANCEL_FLG = :CANCELFLG_0 ")
                    .AppendLine("                 GROUP BY M2.SVCIN_ID) S5 ")
                    .AppendLine("         WHERE S1.SVCIN_ID = S2.SVCIN_ID ")
                    .AppendLine("           AND S2.JOB_DTL_ID = S5.JOB_DTL_ID ")
                    .AppendLine("           AND S2.MERC_ID = S3.MERC_ID(+) ")
                    .AppendLine("           AND S2.SVC_CLASS_ID = S4.SVC_CLASS_ID(+) ")
                    .AppendLine("           AND NOT EXISTS (SELECT 1 ")
                    .AppendLine("                             FROM TB_T_SERVICEIN M5 ")
                    .AppendLine("                            WHERE M5.SVCIN_ID = S1.SVCIN_ID ")
                    .AppendLine("                              AND M5.SVC_STATUS = :SVC_STATUS_02) ")
                    .AppendLine("       ) Q2 ")
                    .AppendLine("      ,(SELECT J1.CST_ID ")
                    .AppendLine("               ,J2.POSITION_TYPE ")
                    .AppendLine("               ,J2.NAMETITLE_NAME ")

                    '2015/12/17 TM 浅野 12月号口配信に向けた緊急対応 START
                    .AppendLine("               ,J1.CST_NAME ")
                    '2015/12/17 TM 浅野 12月号口配信に向けた緊急対応 END

                    .AppendLine("           FROM TB_M_CUSTOMER J1 ")
                    .AppendLine("               ,TB_M_NAMETITLE J2 ")
                    '2015/12/17 TM 浅野 12月号口配信に向けた緊急対応 START
                    .AppendLine("          WHERE J1.NAMETITLE_CD = J2.NAMETITLE_CD(+) ")
                    .AppendLine("            AND J2.INUSE_FLG(+) = :INUSE_FLG_1 ")
                    '.AppendLine("          WHERE J1.NAMETITLE_CD = J2.NAMETITLE_CD ")
                    '.AppendLine("            AND J2.INUSE_FLG = :INUSE_FLG_1 ")
                    '2015/12/17 TM 浅野 12月号口配信に向けた緊急対応 END
                    .AppendLine("       ) Q3 ")
                    .AppendLine(" WHERE Q1.REZID = Q2.SVCIN_ID(+) ")
                    .AppendLine("   AND Q1.CUSTID = Q3.CST_ID(+) ")
                    .AppendLine("   AND Q1.VISITSEQ = :VISITSEQ ")
                    .AppendLine("   AND Q1.UPDATEDATE = :UPDATEDATE ")
                    '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

                End With
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSeq)
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpDateTime)
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START
                query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, StatusCancel)
                query.AddParameterWithTypeValue("CANCELFLG_0", OracleDbType.NVarchar2, CancelFlagEffective)
                query.AddParameterWithTypeValue("INUSE_FLG_1", OracleDbType.NVarchar2, UsingTypeUse)
                '2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

                '実行
                Dim dt As SC3100401DataSet.VisitInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using
        End Function

        ''' <summary>
        ''' ストール予約の最新情報取得
        ''' </summary>
        ''' <param name="inRezID">予約ID</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Function GetDBNewestStallRezInfo(ByVal inRezId As Decimal, _
                                                ByVal inDealerCode As String, _
                                                ByVal inStoreCode As String) _
                                                As SC3100401DataSet.NewestStallRezInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} REZID:{2} DEALERCODE:{3} STORECODE:{4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inRezId, inDealerCode, inStoreCode))

            Using query As New DBSelectQuery(Of SC3100401DataSet.NewestStallRezInfoDataTable)("SC3100401_007")

                Dim sql As New StringBuilder      ' SQL文格納

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'With sql
                '    .AppendLine("SELECT /* SC3100401_007 */")
                '    .AppendLine("       TRIM(T1.ACCOUNT_PLAN) AS ACCOUNT_PLAN")
                '    .AppendLine("     , T1.ORDERNO")
                '    .AppendLine("  FROM TBL_STALLREZINFO T1")
                '    .AppendLine(" WHERE T1.DLRCD = :DLRCD")
                '    .AppendLine("   AND T1.STRCD = :STRCD")
                '    .AppendLine("   AND T1.REZID = :REZID")
                '    .AppendLine("   AND NOT EXISTS (")
                '    .AppendLine("       SELECT 1")
                '    .AppendLine("         FROM TBL_STALLREZINFO ")
                '    .AppendLine("        WHERE DLRCD = T1.DLRCD")
                '    .AppendLine("          AND STRCD = T1.STRCD")
                '    .AppendLine("          AND REZID = T1.REZID")
                '    .AppendLine("          AND (STOPFLG  = :STOPFLG")
                '    .AppendLine("          AND CANCELFLG = :CANCELFLG)")
                '    .AppendLine("       )")
                'End With

                With sql

                    .AppendLine(" SELECT /* SC3100401_007 */ ")
                    .AppendLine("        TRIM(T1.PIC_SA_STF_CD) AS ACCOUNT_PLAN ")
                    .AppendLine("       ,TRIM(T1.RO_NUM) AS ORDERNO ")
                    .AppendLine("       ,ROW_LOCK_VERSION AS ROW_LOCK_VERSION ")
                    .AppendLine("   FROM ")
                    .AppendLine("        TB_T_SERVICEIN T1 ")
                    .AppendLine("  WHERE ")
                    .AppendLine("        T1.SVCIN_ID = :REZID ")
                    .AppendLine("    AND T1.SVC_STATUS <> :SVC_STATUS ")

                End With

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                query.CommandText = sql.ToString()


                'バインド変数

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
                'query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)
                'query.AddParameterWithTypeValue("STOPFLG", OracleDbType.Char, StopFlag)
                'query.AddParameterWithTypeValue("CANCELFLG", OracleDbType.Char, CancelFlag)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, inRezId)
                query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, StatusCancel)


                '実行
                Dim dt As SC3100401DataSet.NewestStallRezInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using
        End Function

        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START

        ''2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

        ''2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 STAT
        ' '''' <summary>
        ' '''' 車両登録No情報取得
        ' '''' </summary>
        ' '''' <param name="inRegNo">車両登録番号</param>
        ' '''' <param name="inChangeRegNo">変更後車両登録番号</param>
        ' '''' <param name="inDealerCode">販売店コード</param>
        ' '''' <param name="inStoreCode">店舗コード</param>
        ' '''' <param name="inPresentTime">現在時間</param>
        ' '''' <returns></returns>
        ' '''' <remarks></remarks>
        ' '''' <history>
        ' '''' 2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発
        ' '''' </history>
        ''Public Function GetRegNoInfo(ByVal inRegNo As String, _
        ''                             ByVal inChangeRegNo As List(Of String), _
        ''                             ByVal inDealerCode As String, _
        ''                             ByVal inStoreCode As String, _
        ''                             ByVal inPresentTime As Date) _
        ''                             As SC3100401DataSet.ChageRegNoInfoDataTable
        ''    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 START
        ''    'Public Function GetRegNoInfo(ByVal inRegNo As String, _
        ''    '                             ByVal inChangeRegNo As String, _
        ''    '                             ByVal inDealerCode As String, _
        ''    '                             ByVal inStoreCode As String, _
        ''    '                             ByVal inPresentTime As Date) _
        ''    '                             As SC3100401DataSet.ChageRegNoInfoDataTable
        ''    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 END
        ''
        ''    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        ''                , "{0}.{1} REGNO:{2} CHANGEREGNO:{3} DEALERCODE:{4} STORECODE:{5} PRESENTTIME:{6}" _
        ''                , Me.GetType.ToString _
        ''                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        ''                , inRegNo, inChangeRegNo, inDealerCode, inStoreCode, inPresentTime))
        ''
        ' ''' <summary>
        ' ''' 車両登録No情報取得
        ' ''' </summary>
        ' ''' <param name="inRegNo">車両登録番号</param>
        ' ''' <param name="inDealerCode">販売店コード</param>
        ' ''' <param name="inStoreCode">店舗コード</param>
        ' ''' <param name="inPresentTime">現在時間</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' ''' 2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発
        ' ''' 2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ' ''' </history>
        'Public Function GetRegNoInfo(ByVal inRegNo As String, _
        '                             ByVal inDealerCode As String, _
        '                             ByVal inStoreCode As String, _
        '                             ByVal inPresentTime As Date) _
        '                             As SC3100401DataSet.ChageRegNoInfoDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} REGNO:{2} DEALERCODE:{3} STORECODE:{4} PRESENTTIME:{5}" _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inRegNo, inDealerCode, inStoreCode, inPresentTime))

        '    'Using query As New DBSelectQuery(Of SC3100401DataSet.ChageRegNoInfoDataTable)("SC3100401_008")

        '    '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

        '    Dim sql As New StringBuilder      ' SQL文格納

        '    With sql
        '        .AppendLine("  SELECT  /* SC3100401_008 */ ")
        '        .AppendLine("           T1.VCL_ID ")
        '        .AppendLine("          ,TRIM(T1.REG_NUM) AS VCLREGNO ")
        '        .AppendLine("          ,NVL(TRIM(T7.VCL_VIN), TRIM(T2.VCL_VIN)) AS VIN ")
        '        .AppendLine("          ,TRIM(T2.VCL_KATASHIKI) AS MODELCODE ")
        '        .AppendLine("          ,T3.SVC_PIC_STF_CD AS STAFFCD ")
        '        .AppendLine("          ,T4.CST_ID AS CUSTCD ")
        '        .AppendLine("          ,NVL(TRIM(T7.DMS_CST_CD), TRIM(T4.DMS_CST_CD)) AS DMS_CST_CD ")
        '        .AppendLine("          ,TRIM(T4.CST_NAME) AS NAME ")
        '        .AppendLine("          ,NVL(TRIM(T4.CST_GENDER), :SEX) AS SEX ")
        '        .AppendLine("          ,TRIM(T4.CST_PHONE) AS TELNO ")
        '        .AppendLine("          ,TRIM(T4.CST_MOBILE) AS MOBILE ")
        '        .AppendLine("          ,CASE WHEN TRIM(T7.DMS_CST_CD) IS NULL ")
        '        .AppendLine("                THEN NVL(TRIM(T5.CST_TYPE), :CUSTSEGMENT) ")
        '        .AppendLine("                ELSE :MYCUSTOMER ")
        '        .AppendLine("                END AS CUSTSEGMENT ")
        '        .AppendLine("          ,T6.SVCIN_ID AS REZID ")
        '        .AppendLine("          ,TRIM(T6.RO_NUM) AS ORDERNO ")
        '        .AppendLine("          ,NVL(TRIM(T6.PIC_SA_STF_CD), TRIM(T3.SVC_PIC_STF_CD)) AS DEFAULTSACODE ")
        '        .AppendLine("          ,T6.CST_ID ")
        '        '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
        '        '.AppendLine("          ,TRIM(T6.CST_VCL_TYPE) AS CST_VCL_TYPE ")
        '        .AppendLine("          ,NVL(TRIM(T6.CST_VCL_TYPE), NVL(TRIM(T3.CST_VCL_TYPE), :CST_VCL_TYPE_1)) AS CST_VCL_TYPE")
        '        '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発END
        '        .AppendLine("          ,TRIM(T7.DMS_CST_CD) AS DMS_DMSID ")
        '        .AppendLine("          ,TRIM(T7.VCL_VIN) AS DMS_VIN ")
        '        .AppendLine("     FROM  TB_M_VEHICLE_DLR T1 ")
        '        .AppendLine("          ,TB_M_VEHICLE T2 ")
        '        .AppendLine("          ,TB_M_CUSTOMER_VCL T3 ")
        '        .AppendLine("          ,TB_M_CUSTOMER T4 ")
        '        .AppendLine("          ,TB_M_CUSTOMER_DLR T5 ")
        '        .AppendLine("          ,(SELECT  S1.SVCIN_ID ")
        '        .AppendLine("                   ,S1.DLR_CD ")
        '        .AppendLine("                   ,S1.RO_NUM ")
        '        .AppendLine("                   ,S1.VCL_ID ")
        '        .AppendLine("                   ,S1.PIC_SA_STF_CD ")
        '        .AppendLine("                   ,S1.CST_ID ")
        '        .AppendLine("                   ,S1.CST_VCL_TYPE ")
        '        .AppendLine("              FROM  TB_T_SERVICEIN S1 ")
        '        .AppendLine("                   ,TB_T_JOB_DTL S2 ")
        '        .AppendLine("                   ,TB_T_STALL_USE S3 ")
        '        .AppendLine("             WHERE  S1.SVCIN_ID = S2.SVCIN_ID ")
        '        .AppendLine("               AND  S2.JOB_DTL_ID = S3.JOB_DTL_ID ")
        '        .AppendLine("               AND  S1.DLR_CD = :DLR_CD ")
        '        .AppendLine("               AND  S1.BRN_CD(+) = :STRCD ")
        '        .AppendLine("               AND  S1.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE  ")
        '        .AppendLine("               AND  S1.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01) ")
        '        .AppendLine("               AND  S2.CANCEL_FLG = :CANCEL_FLG ")
        '        .AppendLine("               AND  TRUNC(S3.SCHE_START_DATETIME) = TRUNC(:SCHE_START_DATETIME) ")
        '        .AppendLine("           ) T6 ")
        '        .AppendLine("          ,TBL_SERVICEIN_APPEND T7 ")
        '        .AppendLine("    WHERE  T1.VCL_ID = T2.VCL_ID ")
        '        .AppendLine("      AND  T1.DLR_CD = T3.DLR_CD ")
        '        .AppendLine("      AND  T2.VCL_ID = T3.VCL_ID ")
        '        .AppendLine("      AND  T3.CST_ID = T4.CST_ID(+) ")
        '        .AppendLine("      AND  T3.CST_ID = T5.CST_ID(+) ")
        '        .AppendLine("      AND  T1.DLR_CD = T6.DLR_CD(+) ")
        '        .AppendLine("      AND  T1.VCL_ID = T6.VCL_ID(+) ")
        '        .AppendLine("      AND  T6.CST_ID = T7.CST_ID(+) ")
        '        .AppendLine("      AND  T6.VCL_ID = T7.VCL_ID(+) ")
        '        .AppendLine("      AND  T1.DLR_CD = :DLR_CD ")

        '        '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
        '        ''変換後車両登録Noのチェック
        '        'If 0 = inChangeRegNo.Count Then
        '        '    '変換後車両登録No無し
        '        '
        '        '    '変換前の車両登録No両方で検索
        '        '    .AppendLine("    AND T1.REG_NUM_SEARCH = UPPER(:VCLREGNO) ")
        '        'Else
        '        '    '変換後車両登録No有り
        '        '
        '        '    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 START
        '        '    ''変換前と変換後の車両登録No両方で検索
        '        '    '.AppendLine("    AND T1.REG_NUM_SEARCH IN (UPPER(:VCLREGNO), UPPER(:CHANGEVCLREGNO)) ")
        '        '
        '        '    'query.AddParameterWithTypeValue("CHANGEVCLREGNO", OracleDbType.NVarchar2, inChangeRegNo)
        '        '
        '        '    '変換前と変換後の車両登録No両方で検索
        '        '    .AppendLine("    AND T1.REG_NUM_SEARCH IN (UPPER(:VCLREGNO) ")
        '        '
        '        '    'ループ用カウント変数
        '        '    Dim count As Integer = 0
        '        '
        '        '    For Each replaceRegisterNo As String In inChangeRegNo
        '        '        'バインド変数
        '        '        Dim bindSqlWord As String = "VCLREGNO_" & count
        '        '        'SQL用の変数
        '        '        Dim createSqlWord As New StringBuilder
        '        '
        '        '        '文字列作成
        '        '        createSqlWord.Append(",UPPER(:").Append(bindSqlWord).Append(")")
        '        '
        '        '        '文字列格納
        '        '        .AppendLine(createSqlWord.ToString)
        '        '
        '        '        'バインド変数設定
        '        '        query.AddParameterWithTypeValue(bindSqlWord, OracleDbType.NVarchar2, replaceRegisterNo)
        '        '
        '        '        'カウントアップ
        '        '        count += 1
        '        '
        '        '    Next
        '        '
        '        '    .AppendLine("        ) ")
        '        '
        '        '    '2014/05/20 TMEJ 小澤 IT9684_サービスタブレットゲートカメラ連携機能追加開発 END
        '        '
        '        'End If

        '        .AppendLine("    AND T1.REG_NUM_SEARCH = UPPER(:VCLREGNO) ")
        '        '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

        '        '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
        '        '.AppendLine("      AND  T3.CST_VCL_TYPE = :CST_VCL_TYPE ")
        '        .AppendLine("      AND  T3.CST_VCL_TYPE <> :CST_VCL_TYPE_4 ")
        '        '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
        '        .AppendLine("      AND  T3.OWNER_CHG_FLG = :OWNER_CHG_FLG ")
        '        .AppendLine("      AND  T5.DLR_CD(+) = :DLR_CD ")
        '        .AppendLine(" ORDER BY  REZID ASC ")
        '        .AppendLine("          ,CUSTSEGMENT ASC ")
        '        '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
        '        .AppendLine("          ,CST_VCL_TYPE ASC ")
        '        '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
        '        .AppendLine("          ,CUSTCD DESC ")

        '    End With

        '    '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 START
        '    Using query As New DBSelectQuery(Of SC3100401DataSet.ChageRegNoInfoDataTable)("SC3100401_008")
        '    '2015/11/10 TM 小牟禮 (トライ店システム評価)SMBチップ検索の絞り込み方法変更 END

        '        'SQL格納
        '        query.CommandText = sql.ToString()

        '        'バインド変数
        '        query.AddParameterWithTypeValue("SEX", OracleDbType.NVarchar2, Male)
        '        query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.NVarchar2, CustSegmentNewCustomer)
        '        query.AddParameterWithTypeValue("MYCUSTOMER", OracleDbType.NVarchar2, CustSegmentMyCustomer)
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
        '        query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, inRegNo)
        '        '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
        '        'query.AddParameterWithTypeValue("CST_VCL_TYPE", OracleDbType.NVarchar2, VehicleType)
        '        query.AddParameterWithTypeValue("CST_VCL_TYPE_1", OracleDbType.NVarchar2, VehicleTypeOwner)
        '        query.AddParameterWithTypeValue("CST_VCL_TYPE_4", OracleDbType.NVarchar2, VehicleTypeInsurance)
        '        '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END
        '        query.AddParameterWithTypeValue("OWNER_CHG_FLG", OracleDbType.NVarchar2, OwnerChangeFlag)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inStoreCode)
        '        query.AddParameterWithTypeValue("SCHE_START_DATETIME", OracleDbType.Date, inPresentTime)
        '        query.AddParameterWithTypeValue("ACCEPTANCE_TYPE", OracleDbType.NVarchar2, AcceptanceTypeRez)
        '        query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, StatusNoIn)
        '        query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, StatusNoVisit)
        '        query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)

        '        '実行
        '        Dim dt As SC3100401DataSet.ChageRegNoInfoDataTable = query.GetData()

        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} END COUNT = {2}" _
        '                    , Me.GetType.ToString _
        '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                    , dt.Count))

        '        Return dt

        '    End Using

        'End Function

        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

        ''' <summary>
        ''' 顧客情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inCustomerID">顧客ID</param>
        ''' <param name="inVehicleID">車両ID</param>
        ''' <param name="inVehicleType">車両区分</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history></history>
        Public Function GetCustomerInfo(ByVal inDealerCode As String, _
                                        ByVal inCustomerId As Decimal, _
                                        ByVal inVehicleId As Decimal, _
                                        ByVal inVehicleType As String) _
                                        As SC3100401DataSet.CustomerInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} DEALERCODE:{2} CST_ID:{3} VCR_ID:{4} VCL_TYPE:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inCustomerId, inVehicleId, inVehicleType))

            Using query As New DBSelectQuery(Of SC3100401DataSet.CustomerInfoDataTable)("SC3100401_009")

                Dim sql As New StringBuilder      ' SQL文格納

                With sql

                    .AppendLine(" SELECT /* SC3100401_009 */ ")
                    .AppendLine("        NVL(TRIM(T1.CST_GENDER), :CST_GENDER) AS CST_GENDER ")
                    .AppendLine("       ,TRIM(T1.DMS_CST_CD) AS DMS_CST_CD ")
                    .AppendLine("       ,TRIM(T1.CST_NAME) AS CST_NAME ")
                    .AppendLine("       ,TRIM(T1.CST_PHONE) AS CST_PHONE ")
                    .AppendLine("       ,TRIM(T1.CST_MOBILE) AS CST_MOBILE ")
                    .AppendLine("       ,NVL(T2.CST_TYPE, :CST_TYPE) AS CST_TYPE ")
                    .AppendLine("       ,TRIM(T3.SVC_PIC_STF_CD) AS SVC_PIC_STF_CD ")
                    .AppendLine("   FROM ")
                    .AppendLine("        TB_M_CUSTOMER T1 ")
                    .AppendLine("       ,TB_M_CUSTOMER_DLR T2 ")
                    .AppendLine("       ,TB_M_CUSTOMER_VCL T3 ")
                    .AppendLine("  WHERE ")
                    .AppendLine("        T1.CST_ID = T2.CST_ID ")
                    .AppendLine("    AND T1.CST_ID = T3.CST_ID ")
                    .AppendLine("    AND T1.CST_ID = :CST_ID ")
                    .AppendLine("    AND T2.DLR_CD = :DLR_CD ")
                    .AppendLine("    AND T3.DLR_CD = :DLR_CD ")
                    .AppendLine("    AND T3.VCL_ID = :VCL_ID ")
                    .AppendLine("    AND T3.CST_VCL_TYPE = :CST_VCL_TYPE ")

                End With

                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("CST_GENDER", OracleDbType.NVarchar2, Male)
                query.AddParameterWithTypeValue("CST_TYPE", OracleDbType.NVarchar2, CustSegmentNewCustomer)
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.Decimal, inCustomerId)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVehicleId)
                query.AddParameterWithTypeValue("CST_VCL_TYPE", OracleDbType.NVarchar2, inVehicleType)

                '実行
                Dim dt As SC3100401DataSet.CustomerInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using
        End Function

        '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

        ''2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 START

        ' ''' <summary>
        ' ''' SA負荷情報取得
        ' ''' </summary>
        ' ''' <param name="inDealerCode">販売店コード</param>
        ' ''' <param name="inStoreCode">店舗コード</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        ' ''' <history>
        ' ''' 2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発
        ' ''' </history>
        'Public Function GetServiceAdviserAssumingInfo(ByVal inDealerCode As String, _
        '                                              ByVal inStoreCode As String, _
        '                                              ByVal inNowDate As Date) As SC3100401DataSet.AssumingInfoDataTable

        '    Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                , "{0}.{1} P1:{2} P2:{3} P3:{4} " _
        '                , Me.GetType.ToString _
        '                , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                , inDealerCode _
        '                , inStoreCode _
        '                , inNowDate))

        '    Using query As New DBSelectQuery(Of SC3100401DataSet.AssumingInfoDataTable)("SC3100401_010")

        '        Dim sql As New StringBuilder

        '        With sql
        '            .AppendLine("SELECT /* SC3100401_010 */ ")
        '            .AppendLine("       T0.ACCOUNT AS SACODE ")
        '            .AppendLine("      ,NVL(T1.RECEPT_PROCESS_COUNT, 0) AS RECEPT_PROCESS_COUNT ")
        '            .AppendLine("      ,NVL(T2.ADDWORK_PROCESS_COUNT, 0) AS ADDWORK_PROCESS_COUNT ")
        '            .AppendLine("      ,NVL(T3.DELIVERYPRE_PROCESS_COUNT, 0) AS DELIVERYPRE_PROCESS_COUNT ")
        '            .AppendLine("      ,NVL(T4.DELIVERYWR_PROCESS_COUNT, 0) AS DELIVERYWR_PROCESS_COUNT ")
        '            .AppendLine("      ,NVL(T5.TODAY_DELIVERY_PLAN_COUNT, 0) AS TODAY_DELIVERY_PLAN_COUNT ")
        '            .AppendLine("  FROM ")
        '            .AppendLine("       TBL_USERS T0 ")
        '            .AppendLine("      ,(/* 受付仕掛中の件数 */ ")
        '            .AppendLine("        SELECT F2.SACODE ")
        '            .AppendLine("              ,COUNT(1) AS RECEPT_PROCESS_COUNT ")
        '            .AppendLine("          FROM (SELECT Q1.VISIT_ID ")
        '            .AppendLine("                      ,COUNT(Q1.VISIT_ID) ")
        '            .AppendLine("                  FROM TB_T_RO_INFO Q1 ")
        '            .AppendLine("                 WHERE Q1.RO_NUM = :SPACE1 ")
        '            .AppendLine("                   AND Q1.RO_STATUS <> :RO_STATUS_99 ")
        '            .AppendLine("                 GROUP BY Q1.VISIT_ID) F1 ")
        '            .AppendLine("              ,TBL_SERVICE_VISIT_MANAGEMENT F2 ")
        '            .AppendLine("         WHERE F1.VISIT_ID = F2.VISITSEQ ")
        '            .AppendLine("           AND F2.DLRCD = :DLR_CD ")
        '            .AppendLine("           AND F2.STRCD = :STR_CD ")
        '            .AppendLine("           AND F2.ASSIGNSTATUS = :ASSIGNSTATUS_2 ")
        '            .AppendLine("         GROUP BY F2.SACODE) T1 ")
        '            .AppendLine("      ,(/* 追加作業承認待ちの件数 */ ")
        '            .AppendLine("        SELECT V2.SACODE ")
        '            .AppendLine("              ,COUNT(1) AS ADDWORK_PROCESS_COUNT ")
        '            .AppendLine("          FROM (SELECT W1.VISIT_ID ")
        '            .AppendLine("                      ,COUNT(W1.VISIT_ID) ")
        '            .AppendLine("                  FROM TB_T_RO_INFO W1 ")
        '            .AppendLine("                 WHERE W1.RO_NUM <> :SPACE1 ")
        '            .AppendLine("                   AND W1.RO_STATUS = :RO_STATUS_35 ")
        '            .AppendLine("                 GROUP BY W1.VISIT_ID) V1 ")
        '            .AppendLine("              ,TBL_SERVICE_VISIT_MANAGEMENT V2 ")
        '            .AppendLine("         WHERE V1.VISIT_ID = V2.VISITSEQ ")
        '            .AppendLine("           AND V2.DLRCD = :DLR_CD ")
        '            .AppendLine("           AND V2.STRCD = :STR_CD ")
        '            .AppendLine("           AND V2.ASSIGNSTATUS = :ASSIGNSTATUS_2 ")
        '            .AppendLine("         GROUP BY V2.SACODE) T2 ")
        '            .AppendLine("      ,(/* 納車準備待ちの件数 */ ")
        '            .AppendLine("        SELECT E3.SACODE ")
        '            .AppendLine("              ,COUNT(1) AS DELIVERYPRE_PROCESS_COUNT ")
        '            .AppendLine("          FROM (SELECT EE.VISIT_ID ")
        '            .AppendLine("                      ,MIN(EE.RO_STATUS) AS RO_STATUS_MIN ")
        '            .AppendLine("                  FROM TB_T_RO_INFO EE ")
        '            .AppendLine("                 WHERE EE.RO_NUM <> :SPACE1 ")
        '            .AppendLine("                 GROUP BY EE.VISIT_ID ) E2 ")
        '            .AppendLine("              ,TBL_SERVICE_VISIT_MANAGEMENT E3 ")
        '            .AppendLine("         WHERE E2.VISIT_ID = E3.VISITSEQ ")
        '            .AppendLine("           AND E2.RO_STATUS_MIN = :RO_STATUS_80 ")
        '            .AppendLine("           AND E3.DLRCD = :DLR_CD ")
        '            .AppendLine("           AND E3.STRCD = :STR_CD ")
        '            .AppendLine("           AND E3.ASSIGNSTATUS = :ASSIGNSTATUS_2 ")
        '            .AppendLine("         GROUP BY E3.SACODE) T3 ")
        '            .AppendLine("      ,(/* 納車待ちの件数 */ ")
        '            .AppendLine("        SELECT D3.SACODE ")
        '            .AppendLine("              ,COUNT(1) AS DELIVERYWR_PROCESS_COUNT ")
        '            .AppendLine("          FROM (SELECT DD.VISIT_ID ")
        '            .AppendLine("                      ,MIN(DD.RO_STATUS) AS RO_STATUS_MIN ")
        '            .AppendLine("                  FROM TB_T_RO_INFO DD ")
        '            .AppendLine("                 WHERE DD.RO_NUM <> :SPACE1 ")
        '            .AppendLine("                 GROUP BY DD.VISIT_ID) D2 ")
        '            .AppendLine("              ,TBL_SERVICE_VISIT_MANAGEMENT D3 ")
        '            .AppendLine("         WHERE D2.VISIT_ID = D3.VISITSEQ ")
        '            .AppendLine("           AND D2.RO_STATUS_MIN = :RO_STATUS_85 ")
        '            .AppendLine("           AND D3.DLRCD = :DLR_CD ")
        '            .AppendLine("           AND D3.STRCD = :STR_CD ")
        '            .AppendLine("           AND D3.ASSIGNSTATUS = :ASSIGNSTATUS_2 ")
        '            .AppendLine("         GROUP BY D3.SACODE) T4 ")
        '            .AppendLine("      ,(/* 本日納車予定台数 */ ")
        '            .AppendLine("        SELECT G1.SACODE ")
        '            .AppendLine("              ,COUNT(1) AS TODAY_DELIVERY_PLAN_COUNT ")
        '            .AppendLine("          FROM TBL_SERVICE_VISIT_MANAGEMENT G1 ")
        '            .AppendLine("              ,TB_T_SERVICEIN G2 ")
        '            .AppendLine("         WHERE G1.FREZID = G2.SVCIN_ID ")
        '            .AppendLine("           AND G1.DLRCD = :DLR_CD ")
        '            .AppendLine("           AND G1.STRCD = :STR_CD ")
        '            .AppendLine("           AND G1.ASSIGNSTATUS <> (:ASSIGNSTATUS_4) ")
        '            .AppendLine("           AND G2.SVC_STATUS NOT IN(:SVC_STATUS_02, :SVC_STATUS_13) ")
        '            .AppendLine("           AND G2.SCHE_DELI_DATETIME BETWEEN TRUNC(:NOWDATE) AND TRUNC(:NOWDATE) + 86399/86400 ")
        '            .AppendLine("         GROUP BY G1.SACODE) T5 ")
        '            .AppendLine(" WHERE T0.ACCOUNT = T1.SACODE(+) ")
        '            .AppendLine("   AND T0.ACCOUNT = T2.SACODE(+) ")
        '            .AppendLine("   AND T0.ACCOUNT = T3.SACODE(+) ")
        '            .AppendLine("   AND T0.ACCOUNT = T4.SACODE(+) ")
        '            .AppendLine("   AND T0.ACCOUNT = T5.SACODE(+) ")
        '            .AppendLine("   AND T0.DLRCD = :DLRCD ")
        '            .AppendLine("   AND T0.STRCD = :STRCD ")
        '            .AppendLine("   AND (T1.RECEPT_PROCESS_COUNT IS NOT NULL ")
        '            .AppendLine("    OR T2.ADDWORK_PROCESS_COUNT IS NOT NULL ")
        '            .AppendLine("    OR T3.DELIVERYPRE_PROCESS_COUNT IS NOT NULL ")
        '            .AppendLine("    OR T4.DELIVERYWR_PROCESS_COUNT IS NOT NULL ")
        '            .AppendLine("    OR T5.TODAY_DELIVERY_PLAN_COUNT IS NOT NULL) ")
        '        End With

        '        query.CommandText = sql.ToString()

        '        'バインド変数
        '        query.AddParameterWithTypeValue("SPACE1", OracleDbType.NVarchar2, Space(1))
        '        query.AddParameterWithTypeValue("RO_STATUS_99", OracleDbType.NVarchar2, RepairOrderStatusCancel)
        '        query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
        '        query.AddParameterWithTypeValue("STR_CD", OracleDbType.NVarchar2, inStoreCode)
        '        query.AddParameterWithTypeValue("ASSIGNSTATUS_2", OracleDbType.NVarchar2, AssignFinish)
        '        query.AddParameterWithTypeValue("RO_STATUS_35", OracleDbType.NVarchar2, RepairOrderStatusWaitSA)
        '        query.AddParameterWithTypeValue("RO_STATUS_80", OracleDbType.NVarchar2, RepairOrderStatusWaitDelivery)
        '        query.AddParameterWithTypeValue("RO_STATUS_85", OracleDbType.NVarchar2, RepairOrderStatusWorkDelivery)
        '        query.AddParameterWithTypeValue("ASSIGNSTATUS_4", OracleDbType.NVarchar2, DealerOut)
        '        query.AddParameterWithTypeValue("SVC_STATUS_02", OracleDbType.NVarchar2, StatusCancel)
        '        query.AddParameterWithTypeValue("SVC_STATUS_13", OracleDbType.NVarchar2, StatusFinishDelivery)
        '        query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)
        '        query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)
        '        query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)

        '        '実行
        '        Dim dt As SC3100401DataSet.AssumingInfoDataTable = query.GetData()

        '        Logger.Info(String.Format(CultureInfo.CurrentCulture _
        '                    , "{0}.{1} END COUNT = {2}" _
        '                    , Me.GetType.ToString _
        '                    , System.Reflection.MethodBase.GetCurrentMethod.Name _
        '                    , dt.Count))

        '        Return dt

        '    End Using

        'End Function

        ''2014/01/23 TMEJ 小澤 IT9611_次世代サービス 工程管理機能開発 END

        '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START


        ''' <summary>
        ''' オーナー顧客情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inVehicleID">車両ID</param>
        ''' <returns>オーナー顧客情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発
        ''' </history>
        Public Function GetOwnerCustomerInfo(ByVal inDealerCode As String, _
                                            ByVal inVehicleId As Decimal) As SC3100401DataSet.OwnerCustomerInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
            , "{0}.{1} DEALERCODE:{2} VCR_ID:{3}" _
            , Me.GetType.ToString _
            , System.Reflection.MethodBase.GetCurrentMethod.Name _
            , inDealerCode, inVehicleId))

            Using query As New DBSelectQuery(Of SC3100401DataSet.OwnerCustomerInfoDataTable)("SC3100401_011")
                Dim sql As New StringBuilder

                With sql
                    .AppendLine("   SELECT  /* SC3100401_011 */")
                    .AppendLine("           T1.CST_ID")
                    .AppendLine("          ,TRIM(T1.CST_VCL_TYPE)  AS CST_VCL_TYPE")
                    .AppendLine("          ,TRIM(T2.CST_NAME) AS CUSTOMERNAME")
                    .AppendLine("          ,TRIM(T2.CST_PHONE) AS TELNO")
                    .AppendLine("          ,TRIM(T2.CST_MOBILE) AS MOBILE")
                    .AppendLine("          ,NVL(TRIM(T2.CST_GENDER), :CST_GENDER) AS SEX ")
                    .AppendLine("          ,TRIM(T2.DMS_CST_CD) AS DMSID")
                    .AppendLine("          ,NVL(TRIM(T3.CST_TYPE), :NEWCUSTMOER) AS CUSTOMERFLAG")
                    .AppendLine("     FROM  TB_M_CUSTOMER_VCL T1")
                    .AppendLine("          ,TB_M_CUSTOMER T2")
                    .AppendLine("          ,TB_M_CUSTOMER_DLR T3")
                    .AppendLine("    WHERE  T1.CST_ID = T2.CST_ID")
                    .AppendLine("      AND  T2.CST_ID = T3.CST_ID")
                    .AppendLine("      AND  T1.VCL_ID = :VCL_ID")
                    .AppendLine("      AND  T1.DLR_CD = :DLR_CD")
                    .AppendLine("      AND  T1.CST_VCL_TYPE = :CST_VCL_TYPE_1")
                    .AppendLine("      AND  T1.OWNER_CHG_FLG = :OWNER_CHG_FLG_0")
                    .AppendLine("      AND  T3.DLR_CD = :DLR_CD")
                    .AppendLine(" ORDER BY  T3.DMS_TAKEIN_DATETIME DESC")
                    .AppendLine("          ,CUSTOMERFLAG ASC")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("CST_GENDER", OracleDbType.NVarchar2, Male)                          ' 性別(性別:0)
                query.AddParameterWithTypeValue("NEWCUSTMOER", OracleDbType.NVarchar2, CustSegmentNewCustomer)       ' 顧客区分(新規顧客:4)
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inVehicleId)                         ' 車両ID
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)                      ' 販売店コード
                query.AddParameterWithTypeValue("CST_VCL_TYPE_1", OracleDbType.NVarchar2, VehicleTypeOwner)          ' 顧客車両区分(所有者:1)
                query.AddParameterWithTypeValue("OWNER_CHG_FLG_0", OracleDbType.NVarchar2, OwnerChangeFlag)          ' オーナーチェンジフラグ(未設定:0)

                '実行
                Dim dt As SC3100401DataSet.OwnerCustomerInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using

        End Function
        '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

        '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない START

        ''' <summary>
        ''' 追加承認工程台数取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <returns>追加承認工程台数情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない
        ''' </history>
        Public Function GetAddWorkProcess(ByVal inDealerCode As String, _
                                          ByVal inStoreCode As String) As SC3100401DataSet.AddWorkProcessInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode _
                        , inStoreCode))

            Using query As New DBSelectQuery(Of SC3100401DataSet.AddWorkProcessInfoDataTable)("SC3100401_012")
                Dim sql As New StringBuilder      ' SQL文格納

                With sql
                    .AppendLine("   SELECT  /* SC3100401_012 */")
                    .AppendLine("       VM.SACODE ")
                    .AppendLine("       ,COUNT(1) AS ADDWORK_PROCESS_COUNT ")
                    .AppendLine("   FROM ")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT VM ")
                    .AppendLine("   WHERE VM.DLRCD = :DLR_CD  ")
                    .AppendLine("     AND VM.STRCD = :STR_CD ")
                    .AppendLine("     AND VM.SACODE IN ( ")
                    .AppendLine("       SELECT T1.ACCOUNT ")
                    .AppendLine("       FROM TBL_USERS T1 ")
                    .AppendLine("       WHERE T1.DLRCD = :DLRCD ")
                    .AppendLine("         AND T1.STRCD = :STRCD ")
                    .AppendLine("         AND T1.OPERATIONCODE = '9' ")
                    .AppendLine("         AND T1.DELFLG = '0' ")
                    .AppendLine("         AND T1.PRESENCECATEGORY IN ('1', '3') ")
                    .AppendLine("         AND T1.PRESENCEDETAIL = '0' ")
                    .AppendLine("   ) ")
                    .AppendLine("   AND VM.ASSIGNSTATUS = :ASSIGNSTATUS_2 ")
                    .AppendLine("   AND EXISTS ( ")
                    .AppendLine("       SELECT 1 ")
                    .AppendLine("       FROM TB_T_RO_INFO RO ")
                    .AppendLine("       WHERE RO.VISIT_ID = VM.VISITSEQ ")
                    .AppendLine("         AND RO.RO_STATUS = :RO_STATUS_35 ")
                    .AppendLine("   ) ")
                    .AppendLine("   AND EXISTS ( ")
                    .AppendLine("       SELECT 1 ")
                    .AppendLine("       FROM TB_T_SERVICEIN SI ")
                    .AppendLine("       WHERE SI.SVCIN_ID = VM.FREZID ")
                    .AppendLine("         AND SI.DLR_CD = :DLR_CD ")
                    .AppendLine("         AND SI.BRN_CD = :STR_CD ")
                    .AppendLine("         AND SI.SVC_STATUS IN ('00', '01', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12') ")
                    .AppendLine("   ) ")
                    .AppendLine("GROUP BY ")
                    .AppendLine("   VM.SACODE ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STR_CD", OracleDbType.NVarchar2, inStoreCode)                      ' 店舗コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)                        ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)                       ' 店舗コード
                query.AddParameterWithTypeValue("ASSIGNSTATUS_2", OracleDbType.NVarchar2, AssignFinish)             ' 振当てステータス（2：振当済み）
                query.AddParameterWithTypeValue("RO_STATUS_35", OracleDbType.NVarchar2, RepairOrderStatusWaitSA)    ' ROステータス(35：SA承認待ち)

                '実行
                Dim dt As SC3100401DataSet.AddWorkProcessInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using

        End Function
        '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない END

        '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない START

        ''' <summary>
        ''' 納車工程台数取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <returns>納車工程台数情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない
        ''' </history>
        Public Function GetDeliveryProcess(ByVal inDealerCode As String, _
                                          ByVal inStoreCode As String) As SC3100401DataSet.DeliveryProcessInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode _
                        , inStoreCode))

            Using query As New DBSelectQuery(Of SC3100401DataSet.DeliveryProcessInfoDataTable)("SC3100401_013")
                Dim sql As New StringBuilder      ' SQL文格納

                With sql
                    .AppendLine("   SELECT  /* SC3100401_013 */")
                    .AppendLine("       VM.SACODE ")
                    .AppendLine("       ,SUM( ")
                    .AppendLine("           CASE ")
                    .AppendLine("               WHEN MIN_RO_STATUS = :RO_STATUS_80 THEN ")
                    .AppendLine("                   1 ")
                    .AppendLine("               ELSE ")
                    .AppendLine("                   0 ")
                    .AppendLine("           END ")
                    .AppendLine("       ) AS DELIVERYPRE_PROCESS_COUNT ")
                    .AppendLine("       ,SUM( ")
                    .AppendLine("           CASE ")
                    .AppendLine("               WHEN MIN_RO_STATUS = :RO_STATUS_85 THEN ")
                    .AppendLine("                   1 ")
                    .AppendLine("               ELSE ")
                    .AppendLine("                   0 ")
                    .AppendLine("           END ")
                    .AppendLine("       ) AS DELIVERYWR_PROCESS_COUNT ")
                    .AppendLine("   FROM ")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT VM ")
                    .AppendLine("   INNER JOIN ( ")
                    .AppendLine("       SELECT ")
                    .AppendLine("           RO.VISIT_ID ")
                    .AppendLine("         , MIN(RO.RO_STATUS) AS MIN_RO_STATUS ")
                    .AppendLine("       FROM TB_T_RO_INFO RO ")
                    .AppendLine("       WHERE RO.RO_STATUS IN ('50', '60', '80', '85') ")
                    .AppendLine("         AND RO.DLR_CD = :DLR_CD ")
                    .AppendLine("         AND RO.BRN_CD = :STR_CD ")
                    .AppendLine("       GROUP BY ")
                    .AppendLine("           RO.VISIT_ID ")
                    .AppendLine("   ) Q1 ")
                    .AppendLine("       ON Q1.VISIT_ID = VM.VISITSEQ ")
                    .AppendLine("   WHERE VM.DLRCD = :DLR_CD  ")
                    .AppendLine("     AND VM.STRCD = :STR_CD ")
                    .AppendLine("     AND VM.SACODE IN ( ")
                    .AppendLine("       SELECT T1.ACCOUNT ")
                    .AppendLine("       FROM TBL_USERS T1 ")
                    .AppendLine("       WHERE T1.DLRCD = :DLRCD ")
                    .AppendLine("         AND T1.STRCD = :STRCD ")
                    .AppendLine("         AND T1.OPERATIONCODE = '9' ")
                    .AppendLine("         AND T1.DELFLG = '0' ")
                    .AppendLine("         AND T1.PRESENCECATEGORY IN ('1', '3') ")
                    .AppendLine("         AND T1.PRESENCEDETAIL = '0' ")
                    .AppendLine("   ) ")
                    .AppendLine("   AND VM.ASSIGNSTATUS = :ASSIGNSTATUS_2 ")
                    .AppendLine("   AND EXISTS ( ")
                    .AppendLine("       SELECT 1 ")
                    .AppendLine("       FROM TB_T_SERVICEIN SI ")
                    .AppendLine("       WHERE SI.SVCIN_ID = VM.FREZID ")
                    .AppendLine("         AND SI.DLR_CD = :DLR_CD ")
                    .AppendLine("         AND SI.BRN_CD = :STR_CD ")
                    .AppendLine("         AND SI.SVC_STATUS IN ('00', '01', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12') ")
                    .AppendLine("   ) ")
                    .AppendLine("GROUP BY ")
                    .AppendLine("   VM.SACODE ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("RO_STATUS_80", OracleDbType.NVarchar2, RepairOrderStatusWaitDelivery)    ' ROステータス(80：納車準備待ち)
                query.AddParameterWithTypeValue("RO_STATUS_85", OracleDbType.NVarchar2, RepairOrderStatusWorkDelivery)    ' ROステータス(85：納車作業中)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)                           ' 販売店コード
                query.AddParameterWithTypeValue("STR_CD", OracleDbType.NVarchar2, inStoreCode)                            ' 店舗コード
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)                            ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)                             ' 店舗コード
                query.AddParameterWithTypeValue("ASSIGNSTATUS_2", OracleDbType.NVarchar2, AssignFinish)             ' 振当てステータス（2：振当済み）

                '実行
                Dim dt As SC3100401DataSet.DeliveryProcessInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using

        End Function
        '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない END

        '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない START

        ''' <summary>
        ''' 本日納車予定台数取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inNowDate">現在日時</param>
        ''' <returns>本日納車予定台数情報</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない
        ''' </history>
        Public Function GetTodayDeliveryPlan(ByVal inDealerCode As String, _
                                          ByVal inStoreCode As String, _
                                          ByVal inNowDate As Date) As SC3100401DataSet.TodayDeliveryPlanInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} P1:{2} P2:{3} P3:{4} " _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode _
                        , inStoreCode _
                        , inNowDate))

            Using query As New DBSelectQuery(Of SC3100401DataSet.TodayDeliveryPlanInfoDataTable)("SC3100401_014")
                Dim sql As New StringBuilder      ' SQL文格納

                With sql
                    .AppendLine("   SELECT  /* SC3100401_014 */")
                    .AppendLine("       SI.PIC_SA_STF_CD ")
                    .AppendLine("       ,COUNT(1) AS TODAY_DELIVERY_PLAN_COUNT ")
                    .AppendLine("   FROM TB_T_SERVICEIN SI ")
                    .AppendLine("   WHERE SI.DLR_CD = :DLR_CD ")
                    .AppendLine("     AND SI.BRN_CD = :STR_CD ")
                    .AppendLine("     AND SI.SVC_STATUS IN ('00', '01', '03', '04', '05', '06', '07', '08', '09', '10', '11', '12') ")
                    .AppendLine("     AND SI.SCHE_DELI_DATETIME BETWEEN TRUNC(:NOWDATE) AND TRUNC(:NOWDATE) + 86399/86400 ")
                    .AppendLine("     AND SI.PIC_SA_STF_CD IN ( ")
                    .AppendLine("       SELECT T1.ACCOUNT ")
                    .AppendLine("       FROM TBL_USERS T1 ")
                    .AppendLine("       WHERE T1.DLRCD = :DLRCD ")
                    .AppendLine("         AND T1.STRCD = :STRCD ")
                    .AppendLine("         AND T1.OPERATIONCODE = '9' ")
                    .AppendLine("         AND T1.DELFLG = '0' ")
                    .AppendLine("         AND T1.PRESENCECATEGORY IN ('1', '3') ")
                    .AppendLine("         AND T1.PRESENCEDETAIL = '0' ")
                    .AppendLine("   ) ")
                    .AppendLine("   GROUP BY ")
                    .AppendLine("       SI.PIC_SA_STF_CD ")
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)                     ' 販売店コード
                query.AddParameterWithTypeValue("STR_CD", OracleDbType.NVarchar2, inStoreCode)                      ' 店舗コード
                query.AddParameterWithTypeValue("NOWDATE", OracleDbType.Date, inNowDate)                            ' 現在日付
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, inDealerCode)                      ' 販売店コード
                query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, inStoreCode)                       ' 店舗コード

                '実行
                Dim dt As SC3100401DataSet.TodayDeliveryPlanInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using

        End Function
        '2016/10/19 NSK 中ノ瀬 TR-SVT-TMT-20160916-001 SVRのアカウント権限が使えない END

        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
        ''' <summary>
        ''' 車両情報取得
        ''' </summary>
        ''' <param name="inRegNo">車両登録番号</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <param name="inPresentTime">現在時間</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする
        ''' </history>
        Public Function GetVehicleInfo(ByVal inRegNo As String, _
                                       ByVal inDealerCode As String, _
                                       ByVal inStoreCode As String, _
                                       ByVal inPresentTime As Date) _
                                       As SC3100401DataSet.VehicleInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} REGNO:{2} DEALERCODE:{3} STORECODE:{4} PRESENTTIME:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inRegNo, inDealerCode, inStoreCode, inPresentTime))

            Dim sql As New StringBuilder      ' SQL文格納

            With sql
                .AppendLine("  SELECT  /* SC3100401_015 */ ")
                .AppendLine("           T1.VCL_ID ")
                .AppendLine("          ,TRIM(T1.REG_NUM) AS VCLREGNO ")
                .AppendLine("          ,TRIM(T2.VCL_VIN) AS VIN ")
                .AppendLine("          ,NVL(TRIM(T7.MODEL_NAME),TRIM(T2.NEWCST_MODEL_NAME)) AS MODEL ")
                .AppendLine("          ,T4.CST_ID ")
                .AppendLine("          ,TRIM(T4.CST_NAME) AS CST_NAME ")
                .AppendLine("          ,NVL(TRIM(T4.CST_MOBILE), TRIM(T4.CST_PHONE)) AS TELNO ")
                .AppendLine("          ,CASE WHEN TRIM(T4.DMS_CST_CD) IS NULL ")
                .AppendLine("                THEN NVL(TRIM(T5.CST_TYPE), :CUSTSEGMENT) ")
                .AppendLine("                ELSE :MYCUSTOMER ")
                .AppendLine("                END AS CUSTSEGMENT ")
                .AppendLine("          ,NVL(TRIM(T3.CST_VCL_TYPE), :CST_VCL_TYPE_1) AS CST_VCL_TYPE ")
                .AppendLine("          ,T6.SVCIN_ID AS REZID ")
                .AppendLine("          ,T6.PLANSTARTDATE ")
                .AppendLine("          ,T6.PLANENDDATE ")
                .AppendLine("          ,T6.MERCHANDISENAME ")
                .AppendLine("     FROM  TB_M_VEHICLE_DLR T1 ")
                .AppendLine("          ,TB_M_VEHICLE T2 ")
                .AppendLine("          ,TB_M_CUSTOMER_VCL T3 ")
                .AppendLine("          ,TB_M_CUSTOMER T4 ")
                .AppendLine("          ,TB_M_CUSTOMER_DLR T5 ")
                .AppendLine("          ,(SELECT  S2.SVCIN_ID ")
                .AppendLine("                   ,S2.DLR_CD ")
                .AppendLine("                   ,S6.VCL_ID ")
                .AppendLine("                   ,CASE S6.SCHE_SVCIN_DATETIME ")
                .AppendLine("                         WHEN :MINDATE ")
                .AppendLine("                         THEN S6.SCHE_START_DATETIME_MIN ")
                .AppendLine("                         ELSE S6.SCHE_SVCIN_DATETIME ")
                .AppendLine("                         END AS PLANSTARTDATE ")
                .AppendLine("                   ,CASE S6.SCHE_DELI_DATETIME ")
                .AppendLine("                         WHEN :MINDATE ")
                .AppendLine("                         THEN S6.SCHE_END_DATETIME_MAX ")
                .AppendLine("                         ELSE S6.SCHE_DELI_DATETIME ")
                .AppendLine("                         END AS PLANENDDATE ")
                '2017/03/23 NSK 竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする START
                '.AppendLine("                   ,NVL(CONCAT(TRIM(S4.UPPER_DISP), TRIM(S4.LOWER_DISP)),NVL(S5.SVC_CLASS_NAME,S5.SVC_CLASS_NAME_ENG)) AS MERCHANDISENAME ")
                .AppendLine("                   ,NVL(CONCAT(TRIM(S4.UPPER_DISP), TRIM(S4.LOWER_DISP)),NVL(TRIM(S5.SVC_CLASS_NAME),TRIM(S5.SVC_CLASS_NAME_ENG))) AS MERCHANDISENAME ")
                '2017/03/23 NSK 竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END
                .AppendLine("              FROM  TB_T_JOB_DTL S2 ")
                .AppendLine("                   ,TB_T_STALL_USE S3 ")
                .AppendLine("                   ,TB_M_MERCHANDISE S4 ")
                .AppendLine("                   ,TB_M_SERVICE_CLASS S5 ")
                .AppendLine("                   ,(SELECT M3.SVCIN_ID ")
                .AppendLine("                           ,M2.VCL_ID")
                .AppendLine("                           ,MIN(M3.JOB_DTL_ID) AS JOB_DTL_ID ")
                .AppendLine("                           ,MIN(M2.SCHE_SVCIN_DATETIME) AS SCHE_SVCIN_DATETIME ")
                .AppendLine("                           ,MIN(M4.SCHE_START_DATETIME) AS SCHE_START_DATETIME_MIN ")
                .AppendLine("                           ,MAX(M2.SCHE_DELI_DATETIME) AS SCHE_DELI_DATETIME ")
                .AppendLine("                           ,MAX(M4.SCHE_END_DATETIME) AS SCHE_END_DATETIME_MAX ")
                .AppendLine("                       FROM TB_T_SERVICEIN M2 ")
                .AppendLine("                           ,TB_T_JOB_DTL M3 ")
                .AppendLine("                           ,TB_T_STALL_USE M4 ")
                .AppendLine("                      WHERE M2.SVCIN_ID = M3.SVCIN_ID")
                .AppendLine("                        AND M3.JOB_DTL_ID = M4.JOB_DTL_ID ")
                .AppendLine("                        AND M2.DLR_CD = :DLR_CD ")
                .AppendLine("                        AND M2.BRN_CD = :STRCD ")
                .AppendLine("                        AND M3.DLR_CD = :DLR_CD ")
                .AppendLine("                        AND M3.BRN_CD = :STRCD ")
                .AppendLine("                        AND M4.DLR_CD = :DLR_CD ")
                .AppendLine("                        AND M4.BRN_CD = :STRCD ")
                .AppendLine("                        AND M3.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("                        AND M2.SVC_STATUS IN (:SVC_STATUS_00, :SVC_STATUS_01) ")
                .AppendLine("                        AND M2.ACCEPTANCE_TYPE = :ACCEPTANCE_TYPE  ")
                .AppendLine("                        AND EXISTS (")
                .AppendLine("                                SELECT 1")
                .AppendLine("                                  FROM TB_T_JOB_DTL Q1")
                .AppendLine("                                     , TB_T_STALL_USE Q2")
                .AppendLine("                                 WHERE Q1.SVCIN_ID = M2.SVCIN_ID ")
                .AppendLine("                                   AND Q1.JOB_DTL_ID = Q2.JOB_DTL_ID ")
                .AppendLine("                                   AND Q1.CANCEL_FLG = :CANCEL_FLG ")
                .AppendLine("                                   AND Q1.DLR_CD = :DLR_CD ")
                .AppendLine("                                   AND Q1.BRN_CD = :STRCD ")
                .AppendLine("                                   AND Q2.DLR_CD = :DLR_CD ")
                .AppendLine("                                   AND Q2.BRN_CD = :STRCD ")
                .AppendLine("                                   AND Q2.SCHE_START_DATETIME ")
                .AppendLine("                               BETWEEN TRUNC(:SCHE_START_DATETIME) ")
                .AppendLine("                                   AND TRUNC(:SCHE_START_DATETIME) + 86399/86400 ")
                .AppendLine("                            )")
                .AppendLine("                   GROUP BY M3.SVCIN_ID ,M2.VCL_ID")
                .AppendLine("                       ) S6 ")
                .AppendLine("             WHERE  S2.JOB_DTL_ID = S3.JOB_DTL_ID ")
                .AppendLine("               AND  S2.DLR_CD = :DLR_CD ")
                .AppendLine("               AND  S2.BRN_CD = :STRCD ")
                .AppendLine("               AND  S3.DLR_CD = :DLR_CD ")
                .AppendLine("               AND  S3.BRN_CD = :STRCD ")
                .AppendLine("               AND  S2.MERC_ID = S4.MERC_ID(+) ")
                .AppendLine("               AND  S2.SVC_CLASS_ID = S5.SVC_CLASS_ID(+) ")
                .AppendLine("               AND  S2.JOB_DTL_ID = S6.JOB_DTL_ID ")
                .AppendLine("           ) T6 ")
                .AppendLine("          ,TB_M_MODEL T7 ")
                .AppendLine("    WHERE  T1.VCL_ID = T2.VCL_ID ")
                .AppendLine("      AND  T1.DLR_CD = T3.DLR_CD ")
                .AppendLine("      AND  T2.VCL_ID = T3.VCL_ID ")
                .AppendLine("      AND  T3.CST_ID = T4.CST_ID ")
                .AppendLine("      AND  T3.CST_ID = T5.CST_ID ")
                .AppendLine("      AND  T1.DLR_CD = T6.DLR_CD(+) ")
                .AppendLine("      AND  T1.VCL_ID = T6.VCL_ID(+) ")
                .AppendLine("      AND  T2.MODEL_CD = T7.MODEL_CD(+) ")
                .AppendLine("      AND  T1.DLR_CD = :DLR_CD ")
                .AppendLine("      AND  T1.REG_NUM_SEARCH = UPPER(:VCLREGNO) ")
                .AppendLine("      AND  T3.CST_VCL_TYPE <> :CST_VCL_TYPE_4 ")
                .AppendLine("      AND  T3.OWNER_CHG_FLG = :OWNER_CHG_FLG ")
                .AppendLine("      AND  T5.DLR_CD = :DLR_CD ")
                .AppendLine(" ORDER BY  T6.PLANSTARTDATE ASC ")
                .AppendLine("          ,T2.DMS_TAKEIN_DATETIME DESC ")
                .AppendLine("          ,CST_VCL_TYPE ASC ")
                .AppendLine("          ,T4.CST_ID DESC ")
            End With

            Using query As New DBSelectQuery(Of SC3100401DataSet.VehicleInfoDataTable)("SC3100401_015")

                'SQL格納
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.NVarchar2, CustSegmentNewCustomer)
                query.AddParameterWithTypeValue("MYCUSTOMER", OracleDbType.NVarchar2, CustSegmentMyCustomer)
                query.AddParameterWithTypeValue("CST_VCL_TYPE_1", OracleDbType.NVarchar2, VehicleTypeOwner)
                query.AddParameterWithTypeValue("MINDATE", OracleDbType.Date, Date.Parse(MinDate, CultureInfo.InvariantCulture))
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("STRCD", OracleDbType.NVarchar2, inStoreCode)
                query.AddParameterWithTypeValue("ACCEPTANCE_TYPE", OracleDbType.NVarchar2, AcceptanceTypeRez)
                query.AddParameterWithTypeValue("SVC_STATUS_00", OracleDbType.NVarchar2, StatusNoIn)
                query.AddParameterWithTypeValue("SVC_STATUS_01", OracleDbType.NVarchar2, StatusNoVisit)
                query.AddParameterWithTypeValue("CANCEL_FLG", OracleDbType.NVarchar2, CancelFlagEffective)
                query.AddParameterWithTypeValue("SCHE_START_DATETIME", OracleDbType.Date, inPresentTime)
                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, inRegNo)
                query.AddParameterWithTypeValue("CST_VCL_TYPE_4", OracleDbType.NVarchar2, VehicleTypeInsurance)
                query.AddParameterWithTypeValue("OWNER_CHG_FLG", OracleDbType.NVarchar2, OwnerChangeFlag)


                '実行
                Dim dt As SC3100401DataSet.VehicleInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using

        End Function
        ''' <summary>
        ''' 更新用車両情報取得
        ''' </summary>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inPresentTime">現在時間</param>
        ''' <param name="inCstId">顧客ID</param>
        ''' <param name="inVclId">車両ID</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする
        ''' </history>
        Public Function GetUpdateVehicleInfo(ByVal inDealerCode As String, _
                                             ByVal inPresentTime As Date, _
                                             ByVal inCstId As Decimal, _
                                             ByVal inVclId As Decimal) _
                                             As SC3100401DataSet.ChageRegNoInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} DEALERCODE:{2} PRESENTTIME:{3} CST_ID:{4} VCL_ID:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inDealerCode, inPresentTime, inCstId, inVclId))

            Dim sql As New StringBuilder      ' SQL文格納

            With sql
                .AppendLine("  SELECT  /* SC3100401_016 */ ")
                .AppendLine("           T1.VCL_ID ")
                .AppendLine("          ,TRIM(T1.REG_NUM) AS VCLREGNO ")
                .AppendLine("          ,TRIM(T2.VCL_VIN) AS VIN ")
                .AppendLine("          ,TRIM(T2.VCL_KATASHIKI) AS MODELCODE ")
                .AppendLine("          ,T3.SVC_PIC_STF_CD AS STAFFCD ")
                .AppendLine("          ,T4.CST_ID AS CUSTCD ")
                .AppendLine("          ,TRIM(T4.DMS_CST_CD) AS DMS_CST_CD ")
                .AppendLine("          ,TRIM(T4.CST_NAME) AS NAME ")
                .AppendLine("          ,NVL(TRIM(T4.CST_GENDER), :SEX) AS SEX ")
                .AppendLine("          ,TRIM(T4.CST_PHONE) AS TELNO ")
                .AppendLine("          ,TRIM(T4.CST_MOBILE) AS MOBILE ")
                .AppendLine("          ,CASE WHEN TRIM(T4.DMS_CST_CD) IS NULL ")
                .AppendLine("                THEN NVL(TRIM(T5.CST_TYPE), :CUSTSEGMENT) ")
                .AppendLine("                ELSE :MYCUSTOMER ")
                .AppendLine("                END AS CUSTSEGMENT ")
                .AppendLine("          ,TRIM(T3.SVC_PIC_STF_CD) AS DEFAULTSACODE ")
                .AppendLine("          ,NVL(TRIM(T3.CST_VCL_TYPE), :CST_VCL_TYPE_1) AS CST_VCL_TYPE ")
                .AppendLine("     FROM  TB_M_VEHICLE_DLR T1 ")
                .AppendLine("          ,TB_M_VEHICLE T2 ")
                .AppendLine("          ,TB_M_CUSTOMER_VCL T3 ")
                .AppendLine("          ,TB_M_CUSTOMER T4 ")
                .AppendLine("          ,TB_M_CUSTOMER_DLR T5 ")
                .AppendLine("    WHERE  T1.VCL_ID = T2.VCL_ID ")
                .AppendLine("      AND  T1.DLR_CD = T3.DLR_CD ")
                .AppendLine("      AND  T2.VCL_ID = T3.VCL_ID ")
                .AppendLine("      AND  T3.CST_ID = T4.CST_ID(+) ")
                .AppendLine("      AND  T3.CST_ID = T5.CST_ID(+) ")
                .AppendLine("      AND  T1.DLR_CD = :DLR_CD ")
                .AppendLine("      AND  T1.VCL_ID = :VCL_ID ")
                .AppendLine("      AND  T4.CST_ID = :CST_ID ")
                .AppendLine("      AND  T5.DLR_CD(+) = :DLR_CD ")
            End With

            Using query As New DBSelectQuery(Of SC3100401DataSet.ChageRegNoInfoDataTable)("SC3100401_016")

                'SQL格納
                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("SEX", OracleDbType.NVarchar2, Male)
                query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.NVarchar2, CustSegmentNewCustomer)
                query.AddParameterWithTypeValue("MYCUSTOMER", OracleDbType.NVarchar2, CustSegmentMyCustomer)
                query.AddParameterWithTypeValue("CST_VCL_TYPE_1", OracleDbType.NVarchar2, VehicleTypeOwner)
                query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, inDealerCode)
                query.AddParameterWithTypeValue("CST_ID", OracleDbType.NVarchar2, inCstId)
                query.AddParameterWithTypeValue("VCL_ID", OracleDbType.NVarchar2, inVclId)


                '実行
                Dim dt As SC3100401DataSet.ChageRegNoInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using

        End Function

        ''' <summary>
        ''' 予約情報取得
        ''' </summary>
        ''' <param name="inRezID">予約ID</param>
        ''' <param name="inDealerCode">販売店コード</param>
        ''' <param name="inStoreCode">店舗コード</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' </history>
        Public Function GetRezInfo(ByVal inRezId As Decimal, _
                                   ByVal inDealerCode As String, _
                                   ByVal inStoreCode As String) _
                                   As SC3100401DataSet.RezInfoDataTable

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} REZID:{2} DEALERCODE:{3} STORECODE:{4}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inRezId, inDealerCode, inStoreCode))

            Using query As New DBSelectQuery(Of SC3100401DataSet.RezInfoDataTable)("SC3100401_007")

                Dim sql As New StringBuilder      ' SQL文格納

                With sql

                    .AppendLine("  SELECT  /* SC3100401_017 */ ")
                    .AppendLine("           T1.SVCIN_ID AS REZID ")
                    .AppendLine("          ,TRIM(T1.RO_NUM) AS ORDERNO ")
                    .AppendLine("          ,TRIM(T1.PIC_SA_STF_CD) AS DEFAULTSACODE ")
                    .AppendLine("          ,T1.CST_ID AS CUSTCD")
                    .AppendLine("          ,TRIM(T1.CST_VCL_TYPE) AS CST_VCL_TYPE ")
                    .AppendLine("   FROM ")
                    .AppendLine("        TB_T_SERVICEIN T1 ")
                    .AppendLine("  WHERE ")
                    .AppendLine("        T1.SVCIN_ID = :REZID ")
                    .AppendLine("    AND T1.SVC_STATUS <> :SVC_STATUS ")

                End With

                query.CommandText = sql.ToString()

                'バインド変数
                query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, inRezId)
                query.AddParameterWithTypeValue("SVC_STATUS", OracleDbType.NVarchar2, StatusCancel)


                '実行
                Dim dt As SC3100401DataSet.RezInfoDataTable = query.GetData()

                Logger.Info(String.Format(CultureInfo.CurrentCulture _
                            , "{0}.{1} END COUNT = {2}" _
                            , Me.GetType.ToString _
                            , System.Reflection.MethodBase.GetCurrentMethod.Name _
                            , dt.Count))

                Return dt

            End Using
        End Function

        '2017/03/23 NSK  竹中 TR-SVT-TMT-20170117-001 SVR画面でランダムな車両でJOBオープンする END

#End Region

#Region "UPDATE"

        ''' <summary>
        ''' 車両登録No変更に伴うサービス来店管理テーブルの更新
        ''' </summary>
        ''' <param name="inRowChageRegNoInfo">車両登録No更新情報</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' 2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 
        ''' </history>
        Public Function UpDateDBRegNo(ByVal inRowChageRegNoInfo As SC3100401DataSet.ChageRegNoInfoRow) _
                                      As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'UPDATE件数返却
            Dim updateCount As Integer = 0

            Using query As New DBUpdateQuery("SC3100401_101")
                Dim sql As New StringBuilder

                With sql
                    .AppendLine("UPDATE /* SC3100401_101 */")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                    .AppendLine("   SET VCLREGNO = :VCLREGNO")              ' 車両登録No
                    .AppendLine("     , CUSTSEGMENT = :CUSTSEGMENT")        ' 登録区分

                    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                    ''顧客コードの確認
                    'If inRowChageRegNoInfo.IsCUSTCDNull Then
                    '    '顧客コード存在しない

                    '    .AppendLine("     , CUSTID = NULL")                 ' 顧客コード
                    '    .AppendLine("     , DMSID = NULL")                  ' 基幹顧客ID

                    'Else
                    '    '顧客コード存在する
                    '    .AppendLine("     , CUSTID = :CUSTID")              ' 顧客コード
                    '    .AppendLine("     , DMSID = :CUSTID")               ' 基幹顧客ID
                    '    query.AddParameterWithTypeValue("CUSTID", OracleDbType.NVarchar2, inRowChageRegNoInfo.CUSTCD)          ' 顧客コード

                    'End If

                    '顧客コードの確認
                    If inRowChageRegNoInfo.IsCUSTCDNull _
                        OrElse inRowChageRegNoInfo.CUSTCD < 1 Then
                        '顧客コード存在しない

                        .AppendLine("     , CUSTID = NULL")                 ' 顧客コード

                    Else
                        '顧客コード存在する
                        .AppendLine("     , CUSTID = :CUSTID")              ' 顧客コード
                        query.AddParameterWithTypeValue("CUSTID", OracleDbType.Decimal, inRowChageRegNoInfo.CUSTCD)               ' 顧客コード

                    End If

                    '基幹顧客コードの確認
                    If inRowChageRegNoInfo.IsDMS_CST_CDNull Then
                        '基幹顧客コード存在しない

                        .AppendLine("     , DMSID = NULL")                 ' 基幹顧客コード

                    Else
                        '基幹顧客コード存在する
                        .AppendLine("     , DMSID = :DMSID")               ' 基幹顧客コード
                        query.AddParameterWithTypeValue("DMSID", OracleDbType.NVarchar2, inRowChageRegNoInfo.DMS_CST_CD)        ' 基幹顧客コード

                    End If

                    '車両IDの確認
                    If inRowChageRegNoInfo.IsVCL_IDNull _
                        OrElse inRowChageRegNoInfo.VCL_ID < 1 Then
                        '車両IDが存在しない

                        .AppendLine("     , VCL_ID = NULL")                 ' 車両ID

                    Else
                        '車両IDが存在する

                        .AppendLine("     , VCL_ID = :VCL_ID")              ' 車両ID
                        query.AddParameterWithTypeValue("VCL_ID", OracleDbType.Decimal, inRowChageRegNoInfo.VCL_ID)            ' 車両ID

                    End If

                    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                    '顧客担当スタッフコードの確認
                    If inRowChageRegNoInfo.IsSTAFFCDNull Then
                        '顧客担当スタッフコード存在しない

                        .AppendLine("     , STAFFCD = NULL")                ' 顧客担当スタッフコード

                    Else
                        '顧客担当スタッフコード存在する

                        .AppendLine("     , STAFFCD = :STAFFCD")            ' 顧客担当スタッフコード
                        query.AddParameterWithTypeValue("STAFFCD", OracleDbType.NVarchar2, inRowChageRegNoInfo.STAFFCD)         ' 顧客担当スタッフコード

                    End If

                    'VINの確認
                    If inRowChageRegNoInfo.IsVINNull Then
                        'VIN存在しない

                        .AppendLine("     , VIN = NULL")                    ' VIN

                    Else
                        'VIN存在する

                        .AppendLine("     , VIN = :VIN")                    ' VIN
                        query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, inRowChageRegNoInfo.VIN)                 ' VIN

                    End If

                    'モデルコードの確認
                    If inRowChageRegNoInfo.IsMODELCODENull Then
                        'モデルコード存在しない

                        .AppendLine("     , MODELCODE = NULL")              ' モデルコード

                    Else
                        'モデルコード存在する

                        .AppendLine("     , MODELCODE = :MODELCODE")        ' モデルコード
                        query.AddParameterWithTypeValue("MODELCODE", OracleDbType.NVarchar2, inRowChageRegNoInfo.MODELCODE)     ' モデルコード

                    End If

                    '性別の確認
                    If inRowChageRegNoInfo.IsSEXNull Then
                        '性別存在しない

                        .AppendLine("     , SEX = :MAN")                    ' 性別
                        query.AddParameterWithTypeValue("MAN", OracleDbType.NVarchar2, Male)                                         ' 性別

                    Else
                        '性別存在する

                        .AppendLine("     , SEX = :SEX")                    ' 性別
                        query.AddParameterWithTypeValue("SEX", OracleDbType.NVarchar2, inRowChageRegNoInfo.SEX)                      ' 性別

                    End If

                    '氏名の確認
                    If inRowChageRegNoInfo.IsNAMENull Then
                        '氏名存在しない

                        .AppendLine("     , NAME = NULL")                   ' 氏名

                    Else
                        '氏名存在する

                        .AppendLine("     , NAME = :NAME")                  ' 氏名
                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, inRowChageRegNoInfo.NAME)               ' 氏名

                    End If

                    '電話番号の確認
                    If inRowChageRegNoInfo.IsTELNONull Then
                        '電話番号存在しない

                        .AppendLine("     , TELNO = NULL")                  ' 電話番号

                    Else
                        '電話番号存在する

                        .AppendLine("     , TELNO = :TELNO")                ' 電話番号
                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, inRowChageRegNoInfo.TELNO)             ' 電話番号

                    End If

                    '携帯番号の確認
                    If inRowChageRegNoInfo.IsMOBILENull Then
                        '携帯番号存在しない

                        .AppendLine("     , MOBILE = NULL")                 ' 携帯番号

                    Else
                        '携帯番号存在する

                        .AppendLine("     , MOBILE = :MOBILE")              ' 携帯番号
                        query.AddParameterWithTypeValue("MOBILE", OracleDbType.NVarchar2, inRowChageRegNoInfo.MOBILE)           ' 携帯番号

                    End If

                    '担当SAの確認
                    If inRowChageRegNoInfo.IsDEFAULTSACODENull Then
                        '担当SA存在しない

                        .AppendLine("     , DEFAULTSACODE = NULL")          ' 担当SA

                    Else
                        '担当SA存在する

                        .AppendLine("     , DEFAULTSACODE = :DEFAULTSACODE") ' 担当SA
                        query.AddParameterWithTypeValue("DEFAULTSACODE", OracleDbType.NVarchar2, inRowChageRegNoInfo.DEFAULTSACODE) ' 担当SA

                    End If

                    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                    ''サービスコードの確認
                    'If inRowChageRegNoInfo.IsSERVICECODENull Then
                    '    'サービスコード存在しない

                    '    .AppendLine("     , SERVICECODE = :GENERAL")        ' サービスコード
                    '    query.AddParameterWithTypeValue("GENERAL", OracleDbType.Char, ServiCodeGeneral)                         ' サービスコード

                    'Else
                    '    'サービスコード存在する

                    '    .AppendLine("     , SERVICECODE = :SERVICECODE")    ' サービスコード
                    '    query.AddParameterWithTypeValue("SERVICECODE", OracleDbType.Char, inRowChageRegNoInfo.SERVICECODE)      ' サービスコード

                    'End If

                    '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                    '整備受注Noの確認
                    If inRowChageRegNoInfo.IsORDERNONull Then
                        '整備受注No存在しない

                        .AppendLine("     , ORDERNO = NULL")                ' 整備受注No

                    Else
                        '整備受注No存在する

                        .AppendLine("     , ORDERNO = :ORDERNO")            ' 整備受注No
                        query.AddParameterWithTypeValue("ORDERNO", OracleDbType.NVarchar2, inRowChageRegNoInfo.ORDERNO)           ' 整備受注No

                    End If


                    .AppendLine("     , REZID = :REZID")                    ' 予約ID                  
                    .AppendLine("     , FREZID = :REZID")                   ' 初回予約ID
                    .AppendLine("     , UPDATEDATE = :PRESENTTIME")         ' 更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")    ' 更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID")              ' 更新機能ID

                    '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 START
                    '来店者氏名の確認
                    If inRowChageRegNoInfo.IsVISITNAMENull Then
                        '来店者氏名存在しない

                        .AppendLine("     , VISITNAME = NULL")                ' 来店者氏名

                    Else
                        '来店者氏名存在する

                        .AppendLine("     , VISITNAME = :VISITNAME")            ' 来店者氏名
                        query.AddParameterWithTypeValue("VISITNAME", OracleDbType.NVarchar2, inRowChageRegNoInfo.VISITNAME)           ' 来店者氏名

                    End If

                    '来店者電話番号の確認
                    If inRowChageRegNoInfo.IsVISITTELNONull Then
                        '来店者電話番号存在しない

                        .AppendLine("     , VISITTELNO = NULL")                ' 来店者電話番号

                    Else
                        '来店者電話番号存在する

                        .AppendLine("     , VISITTELNO = :VISITTELNO")            ' 来店者電話番号
                        query.AddParameterWithTypeValue("VISITTELNO", OracleDbType.NVarchar2, inRowChageRegNoInfo.VISITTELNO)           ' 来店者電話番号

                    End If
                    '2015/09/08 TMEJ 春日井 IT9960_(トライ店システム評価)サービス入庫予約のユーザ管理機能開発 END

                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ")              ' 来店実績連番
                    .AppendLine("   AND UPDATEDATE = :UPDATEDATE")          ' 更新日時

                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, inRowChageRegNoInfo.VCLREGNO)          ' 車両登録No
                query.AddParameterWithTypeValue("CUSTSEGMENT", OracleDbType.NVarchar2, inRowChageRegNoInfo.CUSTSEGMENT)    ' 登録区分
                query.AddParameterWithTypeValue("REZID", OracleDbType.Decimal, inRowChageRegNoInfo.REZID)                  ' 予約ID
                query.AddParameterWithTypeValue("PRESENTTIME", OracleDbType.Date, inRowChageRegNoInfo.PRESENTTIME)         ' 更新日
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inRowChageRegNoInfo.ACCOUNT)      ' 更新アカウント
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationID)                         ' 更新機能I

                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inRowChageRegNoInfo.VISITSEQ)              ' 来店実績連番
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inRowChageRegNoInfo.UPDATEDATE)           ' 更新日

                '処理結果
                updateCount = query.Execute()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END UPDATECOUNT = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , updateCount))

            Return updateCount

        End Function

        ''' <summary>
        ''' SA振当登録・SA変更登録処理
        ''' </summary>
        ''' <param name="inRowVisitInfo">来店管理情報</param>
        ''' <param name="inAccount">更新アカウント</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Function RegisterDBAssignSA(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                           ByVal inAccount As String) _
                                           As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'UPDATE件数返却
            Dim updateCount As Integer = 0

            Using query As New DBUpdateQuery("SC3100401_102")
                Dim sql As New StringBuilder

                With sql
                    .AppendLine("UPDATE /* SC3100401_102 */")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                    .AppendLine("   SET DEFAULTSACODE = :DEFAULTSACODE")        ' 受付担当予定者
                    .AppendLine("     , SACODE = :SACODE")                      ' 振当SA
                    .AppendLine("     , ASSIGNTIMESTAMP = :ASSIGNTIMESTAMP")    ' 振当時間
                    .AppendLine("     , ASSIGNSTATUS = :ASSIGNSTATUS")          ' 振当ステータス
                    .AppendLine("     , QUEUESTATUS = :QUEUESTATUS")            ' 案内待ちキュー状態
                    .AppendLine("     , HOLDSTAFF = NULL")                      ' ホールドスタッフ
                    .AppendLine("     , ORDERNO = :ORDERNO")                    ' 整備受注No
                    .AppendLine("     , UPDATEDATE = :PRESENTTIME")             ' 更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")        ' 更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID")                  ' 更新機能ID
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ")                  ' 来店実績連番
                End With

                query.CommandText = sql.ToString()

                'バインド変数

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("DEFAULTSACODE", OracleDbType.Varchar2, inRowVisitInfo.DEFAULTSACODE)               
                'query.AddParameterWithTypeValue("SACODE", OracleDbType.Varchar2, inRowVisitInfo.SACODE)                             
                query.AddParameterWithTypeValue("DEFAULTSACODE", OracleDbType.NVarchar2, inRowVisitInfo.DEFAULTSACODE)               ' 受付担当予定者
                query.AddParameterWithTypeValue("SACODE", OracleDbType.NVarchar2, inRowVisitInfo.SACODE)                             ' 振当SA

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


                query.AddParameterWithTypeValue("ASSIGNTIMESTAMP", OracleDbType.Date, inRowVisitInfo.PRESENTTIME)                    ' 振当時間


                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.Char, AssignFinish)
                'query.AddParameterWithTypeValue("QUEUESTATUS", OracleDbType.Char, QueueStatusNotWait)
                'query.AddParameterWithTypeValue("ORDERNO", OracleDbType.Char, inRowVisitInfo.ORDERNO)
                query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, AssignFinish)                                 ' 振当ステータス
                query.AddParameterWithTypeValue("QUEUESTATUS", OracleDbType.NVarchar2, QueueStatusNotWait)                            ' 案内待ちキュー状態
                query.AddParameterWithTypeValue("ORDERNO", OracleDbType.NVarchar2, inRowVisitInfo.ORDERNO)                            ' 整備受注No

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                query.AddParameterWithTypeValue("PRESENTTIME", OracleDbType.Date, inRowVisitInfo.PRESENTTIME)                         ' 更新日

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inAccount)
                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, ApplicationID)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)                                   ' 更新アカウント
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationID)                                    ' 更新機能ID  

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inRowVisitInfo.VISITSEQ)                              ' 来店実績連番

                '処理結果
                updateCount = query.Execute()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END UPDATECOUNT = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , updateCount))

            Return updateCount

        End Function

        ''' <summary>
        ''' SA解除処理
        ''' </summary>
        ''' <param name="inRowVisitInfo">来店管理情報</param>
        ''' <param name="inAccount">更新アカウント</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Function RegisterDBUndoSA(ByVal inRowVisitInfo As SC3100401DataSet.VisitInfoRow, _
                                         ByVal inAccount As String) _
                                         As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} START" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name))

            'UPDATE件数返却
            Dim updateCount As Integer = 0

            Using query As New DBUpdateQuery("SC3100401_103")
                Dim sql As New StringBuilder

                With sql
                    .AppendLine("UPDATE /* SC3100401_103 */")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                    .AppendLine("   SET SACODE = NULL")                         ' 振当SA
                    .AppendLine("     , ASSIGNTIMESTAMP = NULL")                ' 振当時間
                    .AppendLine("     , ASSIGNSTATUS = :ASSIGNSTATUS")          ' 振当ステータス
                    .AppendLine("     , UPDATEDATE = :PRESENTTIME")             ' 更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")        ' 更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID")                  ' 更新機能ID
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ")                  ' 来店実績連番
                End With

                query.CommandText = sql.ToString()

                'バインド変数

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.Char, NonAssign)                                       
                query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, NonAssign)                                   ' 振当ステータス

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                query.AddParameterWithTypeValue("PRESENTTIME", OracleDbType.Date, inRowVisitInfo.PRESENTTIME)                        ' 更新日


                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inAccount)
                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, ApplicationID)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)                                  ' 更新アカウント
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationID)                                   ' 更新機能ID 

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inRowVisitInfo.VISITSEQ)                             ' 来店実績連番

                '処理結果
                updateCount = query.Execute()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END UPDATECOUNT = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , updateCount))

            Return updateCount

        End Function

        ''' <summary>
        ''' テキストエリア「受付No・来店者・電話番号・テーブルNo」登録処理
        ''' </summary>
        ''' <param name="invisitSeq">来店実績連番</param>
        ''' <param name="inupDateTime">更新日時</param>
        ''' <param name="inTextAreaID">テキストエリアID</param>
        ''' <param name="inAfterValue">更新する値</param>
        ''' <param name="inAccount">更新アカウント</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Function RegisterDBTextArea(ByVal inVisitSeq As Long, _
                                           ByVal inUpDateTime As Date, _
                                           ByVal inTextAreaId As String, _
                                           ByVal inAfterValue As String, _
                                           ByVal inAccount As String, _
                                           ByVal inPresentTime As Date) _
                                           As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} VISITSEQ:{2} UPDATETIME:{3} AREAID:{4} AFTERVALUE:{5} ACCOUNT:{6} PRESENTTIME:{7}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inVisitSeq, inUpDateTime, inTextAreaId, inAfterValue, inAccount, inPresentTime))

            'UPDATE件数返却
            Dim updateCount As Integer = 0

            Using query As New DBUpdateQuery("SC3100401_104")
                Dim sql As New StringBuilder

                With sql
                    .AppendLine("UPDATE /* SC3100401_104 */")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                    .AppendLine("   SET UPDATEDATE = :PRESENTTIME")         ' 更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")    ' 更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID")              ' 更新機能ID

                    'テキストエリアごと登録内容の変王
                    Select Case inTextAreaId
                        Case CType(EventKeyID.ReceiptNoText, String)
                            '受付番号テキストエリア

                            .AppendLine("     , CALLNO = :AFTERVALUE")

                        Case CType(EventKeyID.VisitorText, String)
                            '来店者テキストエリア

                            .AppendLine("     , VISITNAME = :AFTERVALUE")

                        Case CType(EventKeyID.TellNoText, String)
                            '電話番号テキストエリア

                            .AppendLine("     , VISITTELNO = :AFTERVALUE")

                        Case CType(EventKeyID.TableNoText, String)
                            'テーブルNoテキストエリア

                            .AppendLine("     , CALLPLACE = :AFTERVALUE")

                    End Select

                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ")              ' 来店実績連番
                    .AppendLine("   AND UPDATEDATE = :UPDATEDATE")          ' 更新日時
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("PRESENTTIME", OracleDbType.Date, inPresentTime)                    ' 更新日


                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inAccount)
                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, ApplicationID)
                'query.AddParameterWithTypeValue("AFTERVALUE", OracleDbType.NVarchar2, inAfterValue)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)                  ' 更新アカウント
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationID)                   ' 更新機能ID
                query.AddParameterWithTypeValue("AFTERVALUE", OracleDbType.NVarchar2, inAfterValue)                  ' 変更後の値

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSeq)                         ' 来店実績連番
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpDateTime)                      ' 更新日

                '処理結果
                updateCount = query.Execute()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END UPDATECOUNT = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , updateCount))

            Return updateCount

        End Function

        ''' <summary>
        ''' 呼出登録処理
        ''' </summary>
        ''' <param name="invisitSeq">来店実績連番</param>
        ''' <param name="inupDateTime">更新日時</param>
        ''' <param name="inAccount">更新アカウント</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Function RegisterDBCallStatus(ByVal inVisitSeq As Long, _
                                             ByVal inUpDateTime As Date, _
                                             ByVal inAccount As String, _
                                             ByVal inPresentTime As Date) _
                                             As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} VISITSEQ:{2} UPDATETIME:{3} ACCOUNT:{4} PRESENTTIME:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inVisitSeq, inUpDateTime, inAccount, inPresentTime))

            'UPDATE件数返却
            Dim updateCount As Integer = 0

            Using query As New DBUpdateQuery("SC3100401_105")
                Dim sql As New StringBuilder

                With sql
                    .AppendLine("UPDATE /* SC3100401_105 */")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                    .AppendLine("   SET UPDATEDATE = :PRESENTTIME")         ' 更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")    ' 更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID")              ' 更新機能ID
                    .AppendLine("     , CALLSTARTDATE = :CALLSTARTDATE")    ' 呼出開始日時
                    .AppendLine("     , CALLSTATUS = :CALLSTATUS")          ' 呼出ステータス                   
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ")              ' 来店実績連番
                    .AppendLine("   AND UPDATEDATE = :UPDATEDATE")          ' 更新日時
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("PRESENTTIME", OracleDbType.Date, inPresentTime)          ' 更新日


                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inAccount)       
                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, ApplicationID)        
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)       ' 更新アカウント
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationID)        ' 更新機能ID

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


                query.AddParameterWithTypeValue("CALLSTARTDATE", OracleDbType.Date, inPresentTime)        ' 呼出開始日時


                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("CALLSTATUS", OracleDbType.Char, Calling)                
                query.AddParameterWithTypeValue("CALLSTATUS", OracleDbType.NVarchar2, Calling)            ' 呼出ステータス

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSeq)               ' 来店実績連番
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpDateTime)            ' 更新日

                '処理結果
                updateCount = query.Execute()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END UPDATECOUNT = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , updateCount))

            Return updateCount

        End Function

        ''' <summary>
        ''' 呼出キャンセル登録処理
        ''' </summary>
        ''' <param name="invisitSeq">来店実績連番</param>
        ''' <param name="inupDateTime">更新日時</param>
        ''' <param name="inAccount">更新アカウント</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Function RegisterDBCallCancel(ByVal inVisitSeq As Long, _
                                             ByVal inUpDateTime As Date, _
                                             ByVal inAccount As String, _
                                             ByVal inPresentTime As Date) _
                                             As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} VISITSEQ:{2} UPDATETIME:{3} ACCOUNT:{4} PRESENTTIME:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inVisitSeq, inUpDateTime, inAccount, inPresentTime))

            'UPDATE件数返却
            Dim updateCount As Integer = 0

            Using query As New DBUpdateQuery("SC3100401_106")
                Dim sql As New StringBuilder

                With sql
                    .AppendLine("UPDATE /* SC3100401_106 */")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                    .AppendLine("   SET UPDATEDATE = :PRESENTTIME")         ' 更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")    ' 更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID")              ' 更新機能ID
                    .AppendLine("     , CALLSTARTDATE = NULL")              ' 呼出開始日時
                    .AppendLine("     , CALLSTATUS = :CALLSTATUS")          ' 呼出ステータス
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ")              ' 来店実績連番
                    .AppendLine("   AND UPDATEDATE = :UPDATEDATE")          ' 更新日時
                End With

                query.CommandText = sql.ToString()

                query.AddParameterWithTypeValue("PRESENTTIME", OracleDbType.Date, inPresentTime)          ' 更新日


                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inAccount)
                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, ApplicationID)
                'query.AddParameterWithTypeValue("CALLSTATUS", OracleDbType.Char, NonCall)
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)       ' 更新アカウント
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationID)        ' 更新機能ID
                query.AddParameterWithTypeValue("CALLSTATUS", OracleDbType.NVarchar2, NonCall)            ' 呼出ステータス

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSeq)               ' 来店実績連番
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpDateTime)            ' 更新日

                '処理結果
                updateCount = query.Execute()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                       , "{0}.{1} END UPDATECOUNT = {2}" _
                       , Me.GetType.ToString _
                       , System.Reflection.MethodBase.GetCurrentMethod.Name _
                       , updateCount))

            Return updateCount

        End Function

        ''' <summary>
        ''' チップ削除登録(退店)処理
        ''' </summary>
        ''' <param name="invisitSeq">来店実績連番</param>
        ''' <param name="inupDateTime">更新日時</param>
        ''' <param name="inAccount">更新アカウント</param>
        ''' <param name="inPresentTime">現在日時</param>
        ''' <returns>更新件数</returns>
        ''' <remarks></remarks>
        ''' <history>
        ''' 2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計
        ''' </history>
        Public Function RegisterDBTipDelete(ByVal inVisitSeq As Long, _
                                            ByVal inUpDateTime As Date, _
                                            ByVal inAccount As String, _
                                            ByVal inPresentTime As Date) _
                                            As Integer

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} VISITSEQ:{2} UPDATETIME:{3} ACCOUNT:{4} PRESENTTIME:{5}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , inVisitSeq, inUpDateTime, inAccount, inPresentTime))

            'UPDATE件数返却
            Dim updateCount As Integer = 0

            Using query As New DBUpdateQuery("SC3100401_107")
                Dim sql As New StringBuilder

                With sql
                    .AppendLine("UPDATE /* SC3100401_107 */")
                    .AppendLine("       TBL_SERVICE_VISIT_MANAGEMENT")
                    .AppendLine("   SET ASSIGNSTATUS = :ASSIGNSTATUS")      ' 振当ステータス
                    .AppendLine("     , UPDATEDATE = :PRESENTTIME")         ' 更新日
                    .AppendLine("     , UPDATEACCOUNT = :UPDATEACCOUNT")    ' 更新アカウント
                    .AppendLine("     , UPDATEID = :UPDATEID")              ' 更新機能ID
                    .AppendLine(" WHERE VISITSEQ = :VISITSEQ")              ' 来店実績連番
                    .AppendLine("   AND UPDATEDATE = :UPDATEDATE")          ' 更新日時
                End With

                query.CommandText = sql.ToString()


                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.Char, DealerOut)            
                query.AddParameterWithTypeValue("ASSIGNSTATUS", OracleDbType.NVarchar2, DealerOut)       ' 振当ステータス(退店:4)

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END

                query.AddParameterWithTypeValue("PRESENTTIME", OracleDbType.Date, inPresentTime)         ' 更新日


                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 START

                'query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.Varchar2, inAccount)       
                'query.AddParameterWithTypeValue("UPDATEID", OracleDbType.Varchar2, ApplicationID)        
                query.AddParameterWithTypeValue("UPDATEACCOUNT", OracleDbType.NVarchar2, inAccount)      ' 更新アカウント
                query.AddParameterWithTypeValue("UPDATEID", OracleDbType.NVarchar2, ApplicationID)       ' 更新機能ID

                '2013/06/03 TMEJ 河原 IT9513_【A.STEP2】次世代e-CRBタブレット　新DB適応に向けた機能設計 END


                query.AddParameterWithTypeValue("VISITSEQ", OracleDbType.Int64, inVisitSeq)              ' 来店実績連番
                query.AddParameterWithTypeValue("UPDATEDATE", OracleDbType.Date, inUpDateTime)           ' 更新日時

                '処理結果
                updateCount = query.Execute()

            End Using

            Logger.Info(String.Format(CultureInfo.CurrentCulture _
                        , "{0}.{1} END UPDATECOUNT = {2}" _
                        , Me.GetType.ToString _
                        , System.Reflection.MethodBase.GetCurrentMethod.Name _
                        , updateCount))

            Return updateCount

        End Function

#End Region

    End Class
End Namespace

Partial Class SC3100401DataSet
End Class
