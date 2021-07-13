'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3190402DataSet.vb
'─────────────────────────────────────
'機能： 部品庫モニターデータ取得
'補足： 
'作成： 2014/XX/XX NEC 村瀬
'更新： 2014/09/09 TMEJ Y.Gotoh 部品庫B／O管理に向けた評価用アプリ作成 $01 
'       2015/01/05 NEC H.Ogata  2販売店対応に伴う修正 $02
'       2015/03/16 TMEJ M.Asano DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 $03
'       2015/06/01 TMEJ Y.Gotoh サービスタブレット問連（20140913-06） $04
'       2016/07/27 NSK T.Nakanose サービスタブレット問連（20160705-001） $05
'       2017/03/16 NSK A.Minagawa TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 $06
'       2018/11/26 NSk M.Sakamoto TR-SVT-TMT-20180421-001 サービスタブレットのレスポンスが全画面で遅延している $07
'       2019/05/21 NSK M.Sakamoto 18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策
'       2019/11/05 NSK M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 $08
'       2020/08/06 NSK S.Natsume TR-SVT-TMT-20200710-001 ログへ出力される文字が多すぎるために発生するエラー $09
'─────────────────────────────────────
Imports System.Text
Imports System.Globalization
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

Partial Class SC3190402DataSet

    Partial Class BranchWorkTimeDataTable

    End Class

#Region "定数"

    ''' <summary>
    ''' 機能ID
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ApplicationId As String = "SC3190402"

#Region "テーブル名"
    ''' <summary>
    ''' RO情報
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsTblNameTRoInfo As String = "TB_T_RO_INFO"
    ''' <summary>
    ''' 作業指示
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsTblNameTJobInstruct As String = "TB_T_JOB_INSTRUCT"
    ''' <summary>
    ''' 作業内容
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsTblNameTJobDtl As String = "TB_T_JOB_DTL"
    ''' <summary>
    ''' サービス入庫
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsTblNameTServicein As String = "TB_T_SERVICEIN"
    ''' <summary>
    ''' RO情報
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsTblNameTStallUse As String = "TB_T_STALL_USE"
    ''' <summary>
    ''' 販売店車両
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsTblNameMVehicleDlr As String = "TB_M_VEHICLE_DLR"
    ''' <summary>
    ''' 車両
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsTblNameMVehicle As String = "TB_M_VEHICLE"
    ''' <summary>
    ''' モデルマスタ
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsTblNameMModel As String = "TB_M_MODEL"
    ''' <summary>
    ''' ストールマスタ
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsTblNameMStall As String = "TB_M_STALL"
    ''' <summary>
    ''' ユーザマスタ
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsTblNameMUser As String = "TBL_USERS"
    '18PRJ00XXX_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 START
    ' ''' <summary>
    ' ''' 販売店システム設定
    ' ''' </summary>
    ' ''' <remarks></remarks>
    'Const ConsTblNameMSystemSettingDlr As String = "TB_M_SYSTEM_SETTING_DLR"
    '18PRJ00XXX_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 END
    'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $08
    ''' <summary>
    ''' システム設定
    ''' </summary>
    ''' <remarks></remarks>
    Const ConsTblNameMSystemSetting As String = "TB_M_SYSTEM_SETTING"
    'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $08
#End Region

    ''' <summary>
    ''' カンマ
    ''' </summary>
    Const ConsComma As String = ","

    ''' <summary>
    ''' Where条件のセパレータ
    ''' </summary>
    Const ConsSeparator As String = "','"

    ''' <summary>
    ''' R/O番号のDB初期値
    ''' </summary>
    ''' <remarks></remarks>
    Const DBDefaultValueRoNum As String = " "

    ''' <summary>
    ''' R/O番号のDB初期値
    ''' </summary>
    ''' <remarks></remarks>
    Const DBDefaultValueRoSeq As Integer = -1

    ''' <summary>
    ''' 出庫表番号のDB初期値
    ''' </summary>
    ''' <remarks></remarks>
    Const DBDefaultValueShipmentNo As String = " "

    ''' <summary>
    ''' 部品引取フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Const PartsPickFlgNotPick As String = "0"

#End Region

    ''' <summary>
    ''' デフォルトコンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        '処理なし
    End Sub

#Region "Area01.見積もり待ちデータ取得"
    ''' <summary>
    ''' Area01.見積もり待ちデータ取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roStatus">ROステータス</param>
    ''' <param name="addRepair">追加作業判断</param>
    ''' <returns>データセット</returns>
    ''' <remarks>ROステータス及びRO作業連番を条件にデータを取得する</remarks>
    Public Shared Function GetWaitingforPartsQuotationList( _
                        ByVal dealerCode As String,
                        ByVal branchCode As String, _
                        ByVal roStatus() As String, _
                        ByVal addRepair As Integer
                        ) As SC3190402DataSet.AREA01DataTable

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Error(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} P3:{3} P4:{4}", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode, _
        '                          String.Join(ConsComma, roStatus), _
        '                          addRepair))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3190402_001 */ ")
            .Append(" 0 AS SORT_KEY")
            .Append(",RO.RO_STATUS")
            .Append(",RO.RO_NUM")
            .Append(",RO.RO_SEQ")
            .Append(",NVL(MVD.REG_NUM, ' ') REG_NUM")
            .Append(",NVL(MM.MODEL_NAME, ' ') MODEL_NAME")
            .Append(",NVL(MV.GRADE_NAME, ' ') GRADE_NAME")
            .Append(",RO.RO_CREATE_DATETIME") ' 作成日時＝RO発行日時＝見積依頼日時
            .Append(",US.USERNAME")
            .Append(" ")
            .Append("FROM")
            .Append(" ").Append(ConsTblNameTRoInfo).Append(" RO")
            .Append(",").Append(ConsTblNameTServicein).Append(" SV")
            .Append(",").Append(ConsTblNameMVehicleDlr).Append(" MVD")
            .Append(",").Append(ConsTblNameMVehicle).Append(" MV")
            .Append(",").Append(ConsTblNameMModel).Append(" MM")
            .Append(",").Append(ConsTblNameMUser).Append(" US")
            .Append(" ")
            .Append("WHERE")
            .Append(" RO.SVCIN_ID = SV.SVCIN_ID")
            .Append(" AND SV.DLR_CD = MVD.DLR_CD")
            .Append(" AND SV.VCL_ID = MVD.VCL_ID")
            .Append(" AND MVD.VCL_ID = MV.VCL_ID(+)")
            .Append(" AND MV.MODEL_CD = MM.MODEL_CD(+)")
            .Append(" AND RO.RO_CREATE_STF_CD = RTRIM(US.ACCOUNT)")
            .Append(" AND RO.DLR_CD = :DLR_CD")
            .Append(" AND RO.BRN_CD = :BRN_CD")

            'ROステータス条件
            Dim listRoStatus As New List(Of String)
            For i As Integer = 0 To roStatus.Length - 1
                listRoStatus.Add(roStatus(i).ToString)
            Next
            .Append(" AND RO.RO_STATUS IN ('")
            .Append(String.Join(ConsSeparator, listRoStatus.ToArray))
            .Append("')")

            .Append(" AND RO.RO_SEQ >= :ADD_REPAIR")
            '$05 サービスタブレット問連（20160705-001） START
            .Append(" AND SV.SVC_STATUS <> '02'")
            '$05 サービスタブレット問連（20160705-001） END
            .Append(" ")
            .Append("GROUP BY")
            .Append(" RO.RO_STATUS")
            .Append(",RO.RO_NUM")
            .Append(",RO.RO_SEQ")
            .Append(",MVD.REG_NUM")
            .Append(",MM.MODEL_NAME")
            .Append(",MV.GRADE_NAME")
            .Append(",RO.RO_CREATE_DATETIME")
            .Append(",US.USERNAME")
            .Append(" ")
            .Append("ORDER BY")
            .Append(" RO.RO_CREATE_DATETIME")
            .Append(",RO.RO_NUM")
            .Append(",RO.RO_SEQ")
        End With
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Error("DEBUG:sql.ToString()=" & sql.ToString())
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Using query As New DBSelectQuery(Of SC3190402DataSet.AREA01DataTable)("SC3190402_001")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("ADD_REPAIR", OracleDbType.Int32, addRepair)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)

            sql = Nothing

            Using dt As SC3190402DataSet.AREA01DataTable = query.GetData

                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                '                          "{0} QUERY:COUNT = {1}", _
                '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                          dt.Count))
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                Return dt
            End Using
        End Using
    End Function
#End Region

#Region "Area02.作業計画待ちデータ取得"
    ''' <summary>
    ''' Area02.作業計画待ちデータ取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roStatus">ROステータス</param>
    ''' <param name="startWorkInstructFlg">着工指示フラグ条件範囲</param>
    ''' <param name="choiceStartWorkInstructFlg">着工指示フラグ</param>
    ''' <returns>データセット</returns>
    ''' <remarks>ROステータス及びストール利用ステータスを条件にデータを取得する</remarks>
    Public Shared Function GetWatingforJobPlanningList( _
                        ByVal dealerCode As String,
                        ByVal branchCode As String, _
                        ByVal roStatus() As String, _
                        ByVal startWorkInstructFlg() As String, _
                        ByVal choiceStartWorkInstructFlg As String
                        ) As SC3190402DataSet.AREA02DataTable
        'Public Shared Function GetWatingforJobPlanningList( _
        '                    ByVal dealerCode As String,
        '                    ByVal branchCode As String, _
        '                    ByVal roStatus() As String, _
        '                    ByVal stallUseStatus() As String, _
        '                    ByVal startWorkInstructFlg() As String, _
        '                    ByVal choiceStartWorkInstructFlg As String
        '                    ) As SC3190402DataSet.AREA02DataTable

        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} P3:{3} P4:{4} P5:{5} P6:{6}", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode, _
        '                          String.Join(ConsComma, roStatus), _
        '                          String.Join(ConsComma, stallUseStatus), _
        '                          String.Join(ConsComma, startWorkInstructFlg), _
        '                          choiceStartWorkInstructFlg))

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Error(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} P3:{3} P4:{4} P5:{5}", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode, _
        '                          String.Join(ConsComma, roStatus), _
        '                          String.Join(ConsComma, startWorkInstructFlg), _
        '                          choiceStartWorkInstructFlg))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim sql As New StringBuilder

        With sql
            '.Append("SELECT /* SC3190402_002 */")
            '.Append(" 0 AS SORT_KEY")
            '.Append(",SUB.RO_NUM")
            '.Append(",SUB.RO_SEQ")
            '.Append(",NVL(MVD.REG_NUM, ' ') REG_NUM")
            '.Append(",NVL(MM.MODEL_NAME, ' ') MODEL_NAME")
            '.Append(",SV.SCHE_DELI_DATETIME")
            '.Append(",SUB.DLR_CD")
            '.Append(",SUB.BRN_CD")
            '.Append(" ")
            '.Append("FROM")
            '.Append(" ").Append(ConsTblNameTStallUse).Append(" MAIN")
            '.Append(",").Append(ConsTblNameTServicein).Append(" SV")
            '.Append(",").Append(ConsTblNameMVehicleDlr).Append(" MVD")
            '.Append(",").Append(ConsTblNameMVehicle).Append(" MV")
            '.Append(",").Append(ConsTblNameMModel).Append(" MM")
            '.Append(",(SELECT")
            '.Append(" RO.RO_NUM")
            '.Append(",RO.RO_SEQ")
            '.Append(",RO.SVCIN_ID")
            '.Append(",RO.DLR_CD")
            '.Append(",RO.BRN_CD")
            '.Append(",MAX(SU.STALL_USE_ID) MAX_STALL_USE_ID")
            '.Append(" ")
            '.Append("FROM")
            '.Append(" ").Append(ConsTblNameTRoInfo).Append(" RO")
            '.Append(",").Append(ConsTblNameTJobInstruct).Append(" JI")
            '.Append(",").Append(ConsTblNameTJobDtl).Append(" JD")
            '.Append(",").Append(ConsTblNameTStallUse).Append(" SU")
            '.Append(" ")
            '.Append("WHERE")
            '.Append(" RO.RO_NUM = JI.RO_NUM")
            '.Append(" AND RO.RO_SEQ = JI.RO_SEQ")
            '.Append(" AND JI.JOB_DTL_ID = JD.JOB_DTL_ID")
            '.Append(" AND JD.JOB_DTL_ID = SU.JOB_DTL_ID")
            '.Append(" AND RO.DLR_CD = :DLR_CD")
            '.Append(" AND RO.BRN_CD = :BRN_CD")

            ''ROステータス条件
            'Dim listRoStatus As New List(Of String)
            'For i As Integer = 0 To roStatus.Length - 1
            '    listRoStatus.Add(roStatus(i).ToString.TrimEnd)
            'Next
            '.Append(" AND RO.RO_STATUS IN ('")
            '.Append(String.Join(ConsSeparator, listRoStatus.ToArray))
            '.Append("')")

            '.Append(" ")
            '.Append("GROUP BY")
            '.Append(" RO.RO_NUM")
            '.Append(",RO.RO_SEQ")
            '.Append(",RO.SVCIN_ID")
            '.Append(",RO.DLR_CD")
            '.Append(",RO.BRN_CD")
            '.Append(") SUB")
            '.Append(" ")
            '.Append("WHERE")
            '.Append(" MAIN.STALL_USE_ID = SUB.MAX_STALL_USE_ID")
            '.Append(" AND SUB.SVCIN_ID = SV.SVCIN_ID")
            '.Append(" AND SV.DLR_CD = MVD.DLR_CD")
            '.Append(" AND SV.VCL_ID = MVD.VCL_ID")
            '.Append(" AND MVD.VCL_ID = MV.VCL_ID(+)")
            '.Append(" AND MV.MODEL_CD = MM.MODEL_CD(+)")

            ''ストール利用ステータス条件
            'Dim listStallUseStatus As New List(Of String)
            'For i As Integer = 0 To stallUseStatus.Length - 1
            '    listStallUseStatus.Add(stallUseStatus(i).ToString.TrimEnd)
            'Next
            '.Append(" AND MAIN.STALL_USE_STATUS IN ('")
            '.Append(String.Join(ConsSeparator, listStallUseStatus.ToArray))
            '.Append("')")

            '.Append(" ")
            '.Append("ORDER BY")
            '.Append(" SV.SCHE_DELI_DATETIME")
            '.Append(",SUB.RO_NUM")
            '.Append(",SUB.RO_SEQ")

            .Append("SELECT /* SC3190402_002 */")
            .Append(" 0 AS SORT_KEY")
            .Append(",SUB.RO_NUM")
            .Append(",SUB.RO_SEQ")
            .Append(",NVL(MVD.REG_NUM, ' ') REG_NUM")
            .Append(",NVL(MM.MODEL_NAME, ' ') MODEL_NAME")
            .Append(",SV.SCHE_DELI_DATETIME")
            .Append(",SUB.DLR_CD")
            .Append(",SUB.BRN_CD")
            '2014/06/13 ストール利用ステータス条件を削除
            'すべてのストール利用ステータスを取得後にRO連番で振り分けるよう変更
            .Append(",MAIN.STALL_USE_STATUS")
            .Append(" ")
            .Append("FROM")
            .Append(" ").Append(ConsTblNameTStallUse).Append(" MAIN")
            .Append(",").Append(ConsTblNameTServicein).Append(" SV")
            .Append(",").Append(ConsTblNameMVehicleDlr).Append(" MVD")
            .Append(",").Append(ConsTblNameMVehicle).Append(" MV")
            .Append(",").Append(ConsTblNameMModel).Append(" MM")
            .Append(",(SELECT")
            .Append(" RO.RO_NUM")
            .Append(",RO.RO_SEQ")
            .Append(",RO.SVCIN_ID")
            .Append(",RO.DLR_CD")
            .Append(",RO.BRN_CD")
            .Append(",MAX(SU.STALL_USE_ID) MAX_STALL_USE_ID")
            .Append(" ")
            .Append("FROM")
            .Append(" ").Append(ConsTblNameTRoInfo).Append(" RO")
            .Append(",").Append(ConsTblNameTJobDtl).Append(" JD")
            .Append(",").Append(ConsTblNameTStallUse).Append(" SU")

            .Append(",(SELECT")
            .Append(" RO_NUM")
            .Append(",RO_SEQ")
            .Append(",JISub.JOB_DTL_ID") '2015/01/05 販売店対応に伴う修正 $02
            .Append(",MIN(STARTWORK_INSTRUCT_FLG) AS STARTWORK_INSTRUCT_FLG")
            .Append(" ")
            .Append("FROM")
            .Append(" ").Append(ConsTblNameTJobInstruct).Append(" JISub") '2015/01/05 販売店対応に伴う修正 $02
            .Append(",").Append(ConsTblNameTJobDtl).Append(" JDsub")
            .Append(" ")
            .Append("WHERE")

            '着工指示フラグ条件(余計な値を取ってこないよう絞り込む)
            Dim listStartWorkInstructFlg As New List(Of String)
            For i As Integer = 0 To startWorkInstructFlg.Length - 1
                listStartWorkInstructFlg.Add(startWorkInstructFlg(i).ToString.TrimEnd)
            Next
            .Append(" STARTWORK_INSTRUCT_FLG IN ('")
            .Append(String.Join(ConsSeparator, listStartWorkInstructFlg.ToArray))
            .Append("')")
            .Append(" AND JDsub.JOB_DTL_ID = JISub.JOB_DTL_ID") '2015/01/05 販売店対応に伴う修正 $02
            .Append(" AND JDsub.DLR_CD = :DLR_CD")
            .Append(" AND JDsub.BRN_CD = :BRN_CD")
            .Append(" ")
            .Append("GROUP BY")
            .Append(" RO_NUM")
            .Append(",RO_SEQ")
            .Append(",JISub.JOB_DTL_ID")
            .Append(") JI")
            .Append(" ")
            .Append("WHERE")
            .Append(" RO.RO_NUM = JI.RO_NUM")
            .Append(" AND RO.RO_SEQ = JI.RO_SEQ")
            .Append(" AND JI.JOB_DTL_ID = JD.JOB_DTL_ID")
            .Append(" AND JD.JOB_DTL_ID = SU.JOB_DTL_ID")
            .Append(" AND RO.DLR_CD = :DLR_CD")
            .Append(" AND RO.BRN_CD = :BRN_CD")

            'ROステータス条件
            Dim listRoStatus As New List(Of String)
            For i As Integer = 0 To roStatus.Length - 1
                listRoStatus.Add(roStatus(i).ToString.TrimEnd)
            Next
            .Append(" AND RO.RO_STATUS IN ('")
            .Append(String.Join(ConsSeparator, listRoStatus.ToArray))
            .Append("')")
            .Append(" AND JI.STARTWORK_INSTRUCT_FLG = :STARTWORK_INSTRUCT_FLG")
            .Append(" ")
            .Append("GROUP BY")
            .Append(" RO.RO_NUM")
            .Append(",RO.RO_SEQ")
            .Append(",RO.SVCIN_ID")
            .Append(",RO.DLR_CD")
            .Append(",RO.BRN_CD")
            .Append(") SUB")
            .Append(" ")
            .Append("WHERE")
            .Append(" MAIN.STALL_USE_ID = SUB.MAX_STALL_USE_ID")
            .Append(" AND SUB.SVCIN_ID = SV.SVCIN_ID")
            .Append(" AND SV.DLR_CD = MVD.DLR_CD")
            .Append(" AND SV.VCL_ID = MVD.VCL_ID")
            .Append(" AND MVD.VCL_ID = MV.VCL_ID(+)")
            .Append(" AND MV.MODEL_CD = MM.MODEL_CD(+)")
            '$05 サービスタブレット問連（20160705-001） START
            .Append(" AND SV.SVC_STATUS <> '02'")
            '$05 サービスタブレット問連（20160705-001） END

            '2014/06/13 ストール利用ステータス条件を削除
            ''ストール利用ステータス条件
            'Dim listStallUseStatus As New List(Of String)
            'For i As Integer = 0 To stallUseStatus.Length - 1
            '    listStallUseStatus.Add(stallUseStatus(i).ToString.TrimEnd)
            'Next
            '.Append(" AND MAIN.STALL_USE_STATUS IN ('")
            '.Append(String.Join(ConsSeparator, listStallUseStatus.ToArray))
            '.Append("')")

            .Append(" ")
            .Append("ORDER BY")
            .Append(" SV.SCHE_DELI_DATETIME")
            .Append(",SUB.RO_NUM")
            .Append(",SUB.RO_SEQ")

        End With
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Error("DEBUG:sql.ToString()=" & sql.ToString())
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Using query As New DBSelectQuery(Of SC3190402DataSet.AREA02DataTable)("SC3190402_002")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)
            query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG", OracleDbType.Char, choiceStartWorkInstructFlg)

            sql = Nothing

            Using dt As SC3190402DataSet.AREA02DataTable = query.GetData

                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                '                          "{0} QUERY:COUNT = {1}", _
                '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                          dt.Count))
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                Return dt
            End Using
        End Using
    End Function
#End Region

#Region "Area03.出庫待ちデータ取得"
    ''' <summary>
    ''' Area03.出庫待ちデータ取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roStatus">ROステータス</param>
    ''' <param name="startWorkInstructFlg">着工指示フラグ条件範囲</param>
    ''' <param name="choiceStartWorkInstructFlg">着工指示フラグ</param>
    ''' <returns>データセット</returns>
    ''' <remarks>ROステータス及びストール利用ステータスを条件にデータを取得する</remarks>
    Public Shared Function GetWatingforPartsIssuingList( _
                        ByVal dealerCode As String,
                        ByVal branchCode As String, _
                        ByVal roStatus() As String, _
                        ByVal startWorkInstructFlg() As String, _
                        ByVal choiceStartWorkInstructFlg As String
                        ) As SC3190402DataSet.AREA03DataTable

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Error(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} P3:{3} P4:{4} P5:{5}", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode, _
        '                          String.Join(ConsComma, roStatus), _
        '                          String.Join(ConsComma, startWorkInstructFlg), _
        '                          choiceStartWorkInstructFlg))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim sql As New StringBuilder
        With sql
            '$04 サービスタブレット問連（20140913-06） START
            .Append("SELECT /* SC3190402_003 */ ")
            .Append("       0 AS SORT_KEY ")
            .Append("     , SUB.RO_NUM ")
            .Append("     , SUB.RO_SEQ ")
            .Append("     , NVL(MVD.REG_NUM,' ') REG_NUM ")
            .Append("     , NVL(MM.MODEL_NAME, ' ') MODEL_NAME ")
            .Append("     , NVL(MV.GRADE_NAME, ' ') GRADE_NAME ")
            .Append("     , NVL(MS.STALL_NAME_SHORT, ' ') STALL_NAME_SHORT ")
            .Append("     , SUB.SCHE_START_DATETIME ")
            .Append("     , SV.DLR_CD ")
            .Append("     , SV.BRN_CD ")
            .Append("     , NVL(SUB.STALL_USE_STATUS, ' ') STALL_USE_STATUS ")
            .Append("  FROM ").Append(ConsTblNameTServicein).Append(" SV ")
            .Append("     , ").Append(ConsTblNameMVehicleDlr).Append(" MVD ")
            .Append("     , ").Append(ConsTblNameMVehicle).Append(" MV ")
            .Append("     , ").Append(ConsTblNameMModel).Append(" MM ")
            .Append("     , ").Append(ConsTblNameMStall).Append(" MS ")
            .Append("     , ( ")
            .Append("    SELECT ")
            .Append("           RO.RO_NUM ")
            .Append("         , RO.RO_SEQ ")
            .Append("         , RO.SVCIN_ID ")
            .Append("         , NVL(SU.STALL_ID, - 1) AS STALL_ID ")
            .Append("         , SU.SCHE_START_DATETIME ")
            .Append("         , NVL(SU.STALL_USE_STATUS, ' ') AS STALL_USE_STATUS ")
            .Append("         , RANK() OVER ( ")
            .Append("           PARTITION BY RO.RO_NUM ")
            .Append("                      , RO.RO_SEQ ")
            .Append("                      , RO.SVCIN_ID ")
            .Append("               ORDER BY (CASE WHEN SU.STALL_USE_STATUS IN ('01', '02', '04') THEN 0 ELSE 1 END), SU.SCHE_START_DATETIME, SU.STALL_USE_ID) AS RANK ")
            .Append("      FROM ").Append(ConsTblNameTRoInfo).Append(" RO ")
            .Append("         , ( ")
            .Append("        SELECT JISub.RO_NUM ")
            .Append("             , JISub.RO_SEQ ")
            .Append("             , JISub.JOB_DTL_ID ")
            .Append("             , MAX(JISub.STARTWORK_INSTRUCT_FLG) AS STARTWORK_INSTRUCT_FLG ")
            .Append("          FROM ").Append(ConsTblNameTJobInstruct).Append(" JISub ")
            .Append("             , ").Append(ConsTblNameTJobDtl).Append(" JDsub ")
            .Append("         WHERE STARTWORK_INSTRUCT_FLG IN ('")

            '着工指示フラグ条件(余計な値を取ってこないよう絞り込む)
            Dim listStartWorkInstructFlg As New List(Of String)
            For i As Integer = 0 To startWorkInstructFlg.Length - 1
                listStartWorkInstructFlg.Add(startWorkInstructFlg(i).ToString.TrimEnd)
            Next

            .Append(String.Join(ConsSeparator, listStartWorkInstructFlg.ToArray)).Append("')")
            .Append("           AND JDsub.JOB_DTL_ID = JISub.JOB_DTL_ID ")
            .Append("           AND JDsub.DLR_CD = :DLR_CD ")
            .Append("           AND JDsub.BRN_CD = :BRN_CD ")
            .Append("         GROUP BY JISub.RO_NUM, JISub.RO_SEQ, JISub.JOB_DTL_ID ")
            .Append("           ) JI ")
            .Append("         , ( ")
            .Append("        SELECT JOB_DTL_ID ")
            .Append("             , MAX(STALL_USE_ID) AS MAX_STALL_USE_ID ")
            .Append("          FROM ").Append(ConsTblNameTStallUse).Append(" SUSub ")
            .Append("         GROUP BY JOB_DTL_ID ")
            .Append("           ) SUSub ")
            .Append("         , ").Append(ConsTblNameTStallUse).Append(" SU ")
            .Append("     WHERE RO.RO_NUM = JI.RO_NUM(+) ")
            .Append("       AND RO.RO_SEQ = JI.RO_SEQ(+) ")
            .Append("       AND NVL(JI.JOB_DTL_ID, - 1) = SUSub.JOB_DTL_ID(+) ")
            .Append("       AND NVL(SUSub.MAX_STALL_USE_ID, - 1) = SU.STALL_USE_ID(+) ")
            .Append("       AND RO.DLR_CD = :DLR_CD ")
            .Append("       AND RO.BRN_CD = :BRN_CD ")
            .Append("       AND RO.RO_STATUS IN ('")

            'ROステータス条件
            Dim listRoStatus As New List(Of String)
            For i As Integer = 0 To roStatus.Length - 1
                listRoStatus.Add(roStatus(i).ToString.TrimEnd)
            Next

            .Append(String.Join(ConsSeparator, listRoStatus.ToArray)).Append("')")
            .Append("       AND NVL(JI.STARTWORK_INSTRUCT_FLG, :STARTWORK_INSTRUCT_FLG) = :STARTWORK_INSTRUCT_FLG  ")
            .Append("       ) SUB ")
            .Append(" WHERE SUB.SVCIN_ID = SV.SVCIN_ID ")
            .Append("   AND SUB.STALL_ID = MS.STALL_ID(+) ")
            .Append("   AND SV.DLR_CD = MVD.DLR_CD ")
            .Append("   AND SV.VCL_ID = MVD.VCL_ID ")
            .Append("   AND MVD.VCL_ID = MV.VCL_ID ")
            .Append("   AND MV.MODEL_CD = MM.MODEL_CD(+) ")
            .Append("   AND SUB.RANK = 1 ")
            '$05 サービスタブレット問連（20160705-001） START
            .Append("   AND SV.SVC_STATUS <> '02'")
            '$05 サービスタブレット問連（20160705-001） END
            .Append(" ORDER BY SUB.SCHE_START_DATETIME, SUB.RO_NUM, SUB.RO_SEQ ")
            '$04 サービスタブレット問連（20140913-06） END
        End With
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Error("DEBUG:sql.ToString()=" & sql.ToString())
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Using query As New DBSelectQuery(Of SC3190402DataSet.AREA03DataTable)("SC3190402_003")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)
            query.AddParameterWithTypeValue("STARTWORK_INSTRUCT_FLG", OracleDbType.Char, choiceStartWorkInstructFlg)

            sql = Nothing

            Using dt As SC3190402DataSet.AREA03DataTable = query.GetData
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                '                          "{0} QUERY:COUNT = {1}", _
                '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                          dt.Count))
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                Return dt
            End Using
        End Using
    End Function
#End Region

#Region "Area04.引き取り待ちデータ取得"
    ''' <summary>
    ''' Area04.引き取り待ちデータ取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roStatus">ROステータス</param>
    ''' <param name="stallUseStatus">ストール利用ステータス</param>
    ''' <returns>データセット</returns>
    ''' <remarks>ROステータス及びストール利用ステータスを条件にデータを取得する</remarks>
    Public Shared Function GetWaitingforTechnicianPickupList( _
                        ByVal dealerCode As String,
                        ByVal branchCode As String, _
                        ByVal roStatus() As String, _
                        ByVal stallUseStatus() As String
                        ) As SC3190402DataSet.AREA04DataTable

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Error(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} P3:{3} P4:{4}", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode, _
        '                          String.Join(ConsComma, roStatus), _
        '                          String.Join(ConsComma, stallUseStatus)))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim sql As New StringBuilder
        With sql
            '$04 サービスタブレット問連（20140913-06） START
            .Append("SELECT /* SC3190402_004 */ ")
            .Append("       0 AS SORT_KEY ")
            .Append("     , SUB.RO_NUM ")
            .Append("     , SUB.RO_SEQ ")
            .Append("     , NVL(MVD.REG_NUM, ' ') REG_NUM ")
            .Append("     , NVL(MM.MODEL_NAME, ' ') MODEL_NAME ")
            .Append("     , NVL(MV.GRADE_NAME, ' ') GRADE_NAME ")
            .Append("     , NVL(MS.STALL_NAME_SHORT, ' ') STALL_NAME_SHORT ")
            .Append("     , SUB.SCHE_START_DATETIME ")
            .Append("     , SV.SCHE_DELI_DATETIME ")
            .Append("     , SV.DLR_CD ")
            .Append("     , SV.BRN_CD ")
            .Append("  FROM ").Append(ConsTblNameTServicein).Append(" SV ")
            .Append("     , ").Append(ConsTblNameMVehicleDlr).Append(" MVD ")
            .Append("     , ").Append(ConsTblNameMVehicle).Append(" MV ")
            .Append("     , ").Append(ConsTblNameMModel).Append(" MM ")
            .Append("     , ").Append(ConsTblNameMStall).Append(" MS ")
            .Append("     , ( ")
            .Append("    SELECT RO.RO_NUM ")
            .Append("         , RO.RO_SEQ ")
            .Append("         , RO.SVCIN_ID ")
            .Append("         , SU.STALL_ID ")
            .Append("         , SU.SCHE_START_DATETIME ")
            .Append("         , SU.STALL_USE_STATUS ")
            .Append("         , RANK() OVER ( ")
            .Append("           PARTITION BY RO.RO_NUM ")
            .Append("                      , RO.RO_SEQ ")
            .Append("                      , RO.SVCIN_ID ")
            .Append("               ORDER BY SU.SCHE_START_DATETIME, SU.STALL_USE_ID) RANK ")
            .Append("      FROM ").Append(ConsTblNameTRoInfo).Append(" RO ")
            .Append("         , ( ")
            .Append("        SELECT JISub.RO_NUM ")
            .Append("             , JISub.RO_SEQ ")
            .Append("             , JISub.JOB_DTL_ID ")
            .Append("          FROM ").Append(ConsTblNameTJobInstruct).Append(" JISub ")
            .Append("             , ").Append(ConsTblNameTJobDtl).Append(" JDsub ")
            .Append("         WHERE JDsub.JOB_DTL_ID = JISub.JOB_DTL_ID ")
            .Append("           AND JDsub.DLR_CD = :DLR_CD ")
            .Append("           AND JDsub.BRN_CD = :BRN_CD ")
            .Append("         GROUP BY JISub.RO_NUM, JISub.RO_SEQ, JISub.JOB_DTL_ID ")
            .Append("           ) JI ")
            .Append("         , ").Append(ConsTblNameTStallUse).Append(" SU ")
            .Append("     WHERE RO.RO_NUM = JI.RO_NUM ")
            .Append("       AND RO.RO_SEQ = JI.RO_SEQ ")
            .Append("       AND JI.JOB_DTL_ID = SU.JOB_DTL_ID ")
            .Append("       AND RO.DLR_CD = :DLR_CD ")
            .Append("       AND RO.BRN_CD = :BRN_CD ")
            .Append("       AND RO.RO_STATUS IN ('")

            'ROステータス条件
            Dim listRoStatus As New List(Of String)
            For i As Integer = 0 To roStatus.Length - 1
                listRoStatus.Add(roStatus(i).ToString.TrimEnd)
            Next

            .Append(String.Join(ConsSeparator, listRoStatus.ToArray)).Append("')")
            .Append("       AND SU.STALL_USE_STATUS IN ('")

            'ストール利用ステータス条件
            Dim listStallUseStatus As New List(Of String)
            For i As Integer = 0 To stallUseStatus.Length - 1
                listStallUseStatus.Add(stallUseStatus(i).ToString.TrimEnd)
            Next
            .Append(String.Join(ConsSeparator, listStallUseStatus.ToArray)).Append("')")
            .Append("       ) SUB ")
            .Append(" WHERE SUB.SVCIN_ID = SV.SVCIN_ID ")
            .Append("   AND SUB.STALL_ID = MS.STALL_ID(+) ")
            .Append("   AND SV.DLR_CD = MVD.DLR_CD ")
            .Append("   AND SV.VCL_ID = MVD.VCL_ID ")
            .Append("   AND MVD.VCL_ID = MV.VCL_ID ")
            .Append("   AND MV.MODEL_CD = MM.MODEL_CD(+) ")
            .Append("   AND SUB.RANK = 1 ")
            '$05 サービスタブレット問連（20160705-001） START
            .Append("   AND SV.SVC_STATUS <> '02'")
            '$05 サービスタブレット問連（20160705-001） END
            .Append(" ORDER BY SUB.RO_NUM, SUB.RO_SEQ, SUB.SCHE_START_DATETIME ")
            '$04 サービスタブレット問連（20140913-06） END
        End With
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Error("DEBUG:sql.ToString()=" & sql.ToString())
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Using query As New DBSelectQuery(Of SC3190402DataSet.AREA04DataTable)("SC3190402_004")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)

            sql = Nothing

            Using dt As SC3190402DataSet.AREA04DataTable = query.GetData
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Error(String.Format(CultureInfo.CurrentCulture, _
                '                          "{0} QUERY:COUNT = {1}", _
                '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                          dt.Count))
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                Return dt
            End Using
        End Using
    End Function
#End Region

    '18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 START
    '#Region "販売店システム設定データ取得"
    '    ''' <summary>
    '    ''' SC3190402_005:販売店システム設定から設定値を取得する
    '    ''' </summary>
    '    ''' <param name="dealerCode">販売店コード</param>
    '    ''' <param name="branchCode">店舗コード</param>
    '    ''' <param name="allDealerCode">全店舗を示す販売店コード</param>
    '    ''' <param name="allBranchCode">全店舗を示す店舗コード</param>
    '    ''' <param name="settingName">販売店システム設定名</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Shared Function GetDlrSystemSettingValue(ByVal dealerCode As String, _
    '                                             ByVal branchCode As String, _
    '                                             ByVal allDealerCode As String, _
    '                                             ByVal allBranchCode As String, _
    '                                             ByVal settingName As String) As SC3190402DataSet.SystemSettingDataTable

    '        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
    '        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
    '        '                          "{0} P1:{1} P2:{2} P3:{3} P4:{4} P5:{5} ", _
    '        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
    '        '                          dealerCode, _
    '        '                          branchCode, _
    '        '                          allDealerCode, _
    '        '                          allBranchCode, _
    '        '                          settingName))
    '        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

    '        Dim sql As New StringBuilder
    '        With sql
    '            .Append("SELECT /* SC3190402_005 */")
    '            .Append(" SETTING_VAL")
    '            .Append(" ")
    '            .Append("FROM")
    '            .Append(" ").Append(ConsTblNameMSystemSettingDlr)
    '            .Append(" ")
    '            .Append("WHERE")
    '            .Append(" DLR_CD IN (:DLR_CD, :ALL_DLR_CD)")
    '            .Append(" AND BRN_CD IN (:BRN_CD, :ALL_BRN_CD)")
    '            .Append(" AND SETTING_NAME = :SETTING_NAME")
    '            .Append(" ")
    '            .Append("ORDER BY ")
    '            .Append(" DLR_CD ASC, BRN_CD ASC")
    '        End With

    '        Using query As New DBSelectQuery(Of SC3190402DataSet.SystemSettingDataTable)("SC3190402_005")
    '            query.CommandText = sql.ToString()
    '            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
    '            query.AddParameterWithTypeValue("ALL_DLR_CD", OracleDbType.Char, allDealerCode)
    '            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)
    '            query.AddParameterWithTypeValue("ALL_BRN_CD", OracleDbType.Char, allBranchCode)
    '            query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.Char, settingName)

    '            sql = Nothing

    '            Using dt As SC3190402DataSet.SystemSettingDataTable = query.GetData
    '                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
    '                'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
    '                '                          "{0} QUERY:COUNT = {1}", _
    '                '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
    '                '                          dt.Count))
    '                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
    '                Return dt
    '            End Using
    '        End Using
    '    End Function
    '#End Region
    '18PRJ03359-00_(トライ店システム評価)サービス業務における応答性向上の為の性能対策 END

#Region "追加作業チェック用データ取得処理"
    ''' <summary>
    ''' SC3190402_006:追加作業のチェック用データを取得する
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roNum">RO番号</param>
    Public Shared Function GetMaxRoSeq(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal roNum As String
                                        ) As SC3190402DataSet.MaxRoSeqDataTable

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} P3:{3} ", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode, _
        '                          roNum))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim sql As New StringBuilder
        With sql
            .Append("SELECT /* SC3190402_006 */")
            .Append(" MAX(RO.RO_SEQ) MAX_RO_SEQ")
            .Append(" ")
            .Append("FROM")
            .Append(" ").Append(ConsTblNameTRoInfo).Append(" RO")
            .Append(" ")
            .Append("WHERE")
            .Append(" RO.DLR_CD = :DLR_CD")
            .Append(" AND RO.BRN_CD = :BRN_CD")
            .Append(" AND RO.RO_NUM = :RO_NUM")
            .Append(" ")
            .Append("GROUP BY")
            .Append(" RO.RO_NUM")
        End With

        Using query As New DBSelectQuery(Of SC3190402DataSet.MaxRoSeqDataTable)("SC3190402_006")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.Char, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.Char, branchCode)
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.Char, roNum)

            sql = Nothing

            Using dt As SC3190402DataSet.MaxRoSeqDataTable = query.GetData
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                '                          "{0} QUERY:COUNT = {1}", _
                '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                          dt.Count))
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                Return dt
            End Using

        End Using

    End Function
#End Region

    '$01 部品庫B／O管理に向けた評価用アプリ作成 START
#Region "かご件数取得"

    ''' <summary>
    ''' かご件数取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <returns>かご件数</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCageCount(ByVal dealerCode As String, _
                                        ByVal branchCode As String) As Integer

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} ", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3190402_008 */ ")
            .Append("        CAGE_NO ")
            .Append("   FROM TB_T_CAGE_INFO ")
            .Append("  WHERE DLR_CD = :DLR_CD ")
            .Append("    AND BRN_CD = :BRN_CD ")
            .Append("  ORDER BY CAGE_NO ")
            '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            '.Append("  FOR UPDATE ")
            .Append("  FOR UPDATE WAIT 1 ")
            '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        End With

        Using query As New DBSelectQuery(Of DataTable)("SC3190402_008")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)

            sql = Nothing

            Dim returnCount As Integer = 0

            Using dt As DataTable = query.GetData
                If dt IsNot Nothing Then
                    returnCount = dt.Rows.Count
                End If
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                '                          "{0} QUERY:COUNT = {1}", _
                '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                          returnCount))
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                Return returnCount
            End Using

        End Using

    End Function
#End Region

#Region "かごの解放"
    ''' <summary>
    ''' かごの解放
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roStatus">ROステータス</param>
    ''' <param name="nowDate">現在日付</param>
    ''' <param name="account">アカウント</param>
    ''' <returns>更新件数</returns>
    Public Shared Function ReleaseCage(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal roStatus() As String, _
                                        ByVal nowDate As Date, _
                                        ByVal account As String) As Integer

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} P3:{3} ", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode, _
        '                          String.Join(ConsComma, roStatus)))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim sql As New StringBuilder
        With sql
            .Append(" UPDATE /* SC3190402_009 */ ")
            .Append("        TB_T_CAGE_INFO ")
            .Append("    SET RO_NUM = :RO_NUM_DEFAULT ")
            .Append("      , RO_SEQ = :RO_SEQ_DEFAULT ")
            .Append("      , SHIPMENT_NO = :SHIPMENT_NO_DEFAULT ")
            .Append("      , PARTS_PICK_FLG = :PARTS_PICK_FLG ")
            .Append("      , ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
            .Append("      , ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ")
            .Append("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
            .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1")
            .Append("  WHERE DLR_CD = :DLR_CD ")
            .Append("    AND BRN_CD = :BRN_CD ")
            .Append("    AND CAGE_NO IN ( ")
            .Append("     SELECT CAGE.CAGE_NO ")
            .Append("       FROM TB_T_CAGE_INFO CAGE ")
            .Append("          , TB_T_RO_INFO RO ")
            .Append("      WHERE CAGE.DLR_CD = RO.DLR_CD(+) ")
            .Append("        AND CAGE.BRN_CD = RO.BRN_CD(+) ")
            .Append("        AND CAGE.RO_NUM = RO.RO_NUM(+) ")
            .Append("        AND CAGE.RO_SEQ = RO.RO_SEQ(+) ")
            .Append("        AND CAGE.DLR_CD = :DLR_CD ")
            .Append("        AND CAGE.BRN_CD = :BRN_CD ")
            .Append("        AND CAGE.RO_NUM <> :RO_NUM_DEFAULT ")
            'ROステータス条件
            Dim listRoStatus As New List(Of String)
            For i As Integer = 0 To roStatus.Length - 1
                listRoStatus.Add(roStatus(i).ToString.TrimEnd)
            Next
            .Append("        AND ( ")
            .Append("                RO.RO_NUM IS NULL ")
            .Append("             OR RO.RO_STATUS IN ('")
            .Append(String.Join(ConsSeparator, listRoStatus.ToArray))
            .Append("') ")
            .Append("            ) ")
            .Append("        ) ")
        End With

        Using query As New DBUpdateQuery("SC3190402_009")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("RO_NUM_DEFAULT", OracleDbType.NVarchar2, DBDefaultValueRoNum)
            query.AddParameterWithTypeValue("RO_SEQ_DEFAULT", OracleDbType.Int32, DBDefaultValueRoSeq)
            query.AddParameterWithTypeValue("SHIPMENT_NO_DEFAULT", OracleDbType.NVarchar2, DBDefaultValueShipmentNo)
            query.AddParameterWithTypeValue("PARTS_PICK_FLG", OracleDbType.NVarchar2, PartsPickFlgNotPick)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
            query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, nowDate)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, ApplicationId)

            sql = Nothing

            Dim updateCount As Integer = query.Execute()
            '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
            '              "{0} QUERY:COUNT = {1}", _
            '              System.Reflection.MethodBase.GetCurrentMethod.Name, _
            '              updateCount))
            '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
            Return updateCount
        End Using

    End Function
#End Region

#Region "空きかご取得"
    ''' <summary>
    ''' 空きかご取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <returns>空きかごデータテーブル</returns>
    Public Shared Function GetNotUseCage(ByVal dealerCode As String, _
                                        ByVal branchCode As String) As SC3190402DataSet.NotUseCageDataTable

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} ", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim sql As New StringBuilder
        With sql
            '2018/11/26 NSk M.Sakamoto TR-SVT-TMT-20180421-001 サービスタブレットのレスポンスが全画面で遅延している START
            '.Append(" SELECT /* SC3190402_010 */ ")
            .Append(" SELECT /* SC3190402_010 */ /*+ INDEX(TB_T_CAGE_INFO TB_T_CAGE_INFO_IX3) */ ")
            '2018/11/26 NSk M.Sakamoto TR-SVT-TMT-20180421-001 サービスタブレットのレスポンスが全画面で遅延している END
            .Append("        DLR_CD ")
            .Append("      , BRN_CD ")
            .Append("      , CAGE_NO ")
            .Append("      , USE_COUNT ")
            .Append("   FROM TB_T_CAGE_INFO ")
            .Append("  WHERE DLR_CD = :DLR_CD ")
            .Append("    AND BRN_CD = :BRN_CD ")
            .Append("    AND RO_NUM = :RO_NUM_DEFAULT ")
            .Append("  ORDER BY USE_COUNT, CAGE_NO ")

        End With

        Using query As New DBSelectQuery(Of SC3190402DataSet.NotUseCageDataTable)("SC3190402_010")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
            query.AddParameterWithTypeValue("RO_NUM_DEFAULT", OracleDbType.NVarchar2, DBDefaultValueRoNum)

            sql = Nothing

            Using dt As SC3190402DataSet.NotUseCageDataTable = query.GetData
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                '                          "{0} QUERY:COUNT = {1}", _
                '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                          dt.Count))
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                Return dt
            End Using
        End Using

    End Function
#End Region

#Region "出庫表番号未設定のかご取得"
    ''' <summary>
    ''' 出庫表番号未設定のかご取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <returns>出庫表番号未設定かごデータテーブル</returns>
    Public Shared Function GetNotSetShipmentNo(ByVal dealerCode As String, _
                                        ByVal branchCode As String) As SC3190402DataSet.NotSetShipmentNoInfoDataTable

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} ", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3190402_011 */ ")
            .Append("        DLR_CD ")
            .Append("      , BRN_CD ")
            .Append("      , CAGE_NO ")
            .Append("      , RO_NUM ")
            .Append("      , RO_SEQ ")
            .Append("   FROM TB_T_CAGE_INFO ")
            .Append("  WHERE DLR_CD = :DLR_CD ")
            .Append("    AND BRN_CD = :BRN_CD ")
            .Append("    AND RO_NUM <> :RO_NUM_DEFAULT ")
            .Append("    AND SHIPMENT_NO = :SHIPMENT_NO_DEFAULT ")
        End With

        Using query As New DBSelectQuery(Of SC3190402DataSet.NotSetShipmentNoInfoDataTable)("SC3190402_011")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
            query.AddParameterWithTypeValue("RO_NUM_DEFAULT", OracleDbType.NVarchar2, DBDefaultValueRoNum)
            query.AddParameterWithTypeValue("SHIPMENT_NO_DEFAULT", OracleDbType.NVarchar2, DBDefaultValueShipmentNo)

            sql = Nothing

            Using dt As SC3190402DataSet.NotSetShipmentNoInfoDataTable = query.GetData
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                '                          "{0} QUERY:COUNT = {1}", _
                '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                          dt.Count))
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                Return dt
            End Using
        End Using

    End Function
#End Region

#Region "出庫表番号更新"
    ''' <summary>
    ''' 出庫表番号更新
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roNum">R/O番号</param>
    ''' <param name="roSeq">R/O番号連番</param>
    ''' <param name="shipmentNo">出庫表番号</param>
    ''' <param name="nowDate">現在日付</param>
    ''' <param name="account">アカウント</param>
    ''' <returns>更新件数</returns>
    Public Shared Function UpdateShipmentNo(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal roNum As String, _
                                        ByVal roSeq As Integer, _
                                        ByVal shipmentNo As String, _
                                        ByVal nowDate As Date, _
                                        ByVal account As String) As Integer

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} P3:{3} P4:{4} P5:{5} ", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode, _
        '                          roNum, _
        '                          roSeq, _
        '                          shipmentNo
        '                          ))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim sql As New StringBuilder
        With sql
            .Append(" UPDATE /* SC3190402_012 */ ")
            .Append("        TB_T_CAGE_INFO ")
            .Append("    SET SHIPMENT_NO = :SHIPMENT_NO ")
            .Append("      , ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
            .Append("      , ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ")
            .Append("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
            .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1")
            .Append("  WHERE DLR_CD = :DLR_CD ")
            .Append("    AND BRN_CD = :BRN_CD ")
            .Append("    AND RO_NUM = :RO_NUM ")
            .Append("    AND RO_SEQ = :RO_SEQ ")
            .Append("    AND SHIPMENT_NO = :SHIPMENT_NO_DEFAULT ")
        End With

        Using query As New DBUpdateQuery("SC3190402_012")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("SHIPMENT_NO", OracleDbType.NVarchar2, shipmentNo)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)
            query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Int32, roSeq)
            query.AddParameterWithTypeValue("SHIPMENT_NO_DEFAULT", OracleDbType.NVarchar2, DBDefaultValueShipmentNo)
            query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, nowDate)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, ApplicationId)

            sql = Nothing

            Dim updateCount As Integer = query.Execute()
            '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
            '              "{0} QUERY:COUNT = {1}", _
            '              System.Reflection.MethodBase.GetCurrentMethod.Name, _
            '              updateCount))
            '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
            Return updateCount
        End Using

    End Function
#End Region

#Region "未紐付けRO情報取得"
    ''' <summary>
    ''' 未紐付けRO情報取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roNum">R/O番号(表示対象のR/O番号全て)</param>
    ''' <param name="roSeq">R/O番号連番(表示対象のR/O番号連番全て)</param>
    ''' <returns>未紐付けRO情報データテーブル</returns>
    Public Shared Function GetNotAssociatedRoInfo(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal roNum() As String, _
                                        ByVal roSeq() As Integer) As SC3190402DataSet.NotAssociatedRoInfoDataTable

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} P3:{3} P4:{4} ", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode, _
        '                          String.Join(ConsComma, roNum), _
        '                          String.Join(ConsComma, roSeq) _
        '                          ))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'IN句の作成
        Dim inCondition As List(Of String) = New List(Of String)
        For i As Integer = 0 To roNum.Length - 1
            Dim roNumRoSeq As String = String.Format(CultureInfo.CurrentCulture _
                                              , "('{0}','{1}')" _
                                              , roNum(i) _
                                              , roSeq(i))
            inCondition.Add(roNumRoSeq)
        Next

        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3190402_013 */ ")
            .Append("        RO.DLR_CD ")
            .Append("      , RO.BRN_CD ")
            .Append("      , RO.RO_NUM ")
            .Append("      , RO.RO_SEQ ")
            .Append("   FROM TB_T_CAGE_INFO CAGE, ")
            .Append("        TB_T_RO_INFO RO ")
            .Append("  WHERE RO.DLR_CD = CAGE.DLR_CD(+) ")
            .Append("    AND RO.BRN_CD = CAGE.BRN_CD(+) ")
            .Append("    AND RO.RO_NUM = CAGE.RO_NUM(+) ")
            .Append("    AND RO.RO_SEQ = CAGE.RO_SEQ(+) ")
            .Append("    AND RO.DLR_CD = :DLR_CD ")
            .Append("    AND RO.BRN_CD = :BRN_CD ")
            .Append("    AND (RO.RO_NUM,RO.RO_SEQ) IN (")
            .Append(String.Join(ConsComma, inCondition))
            .Append(") ")
            .Append("    AND CAGE.CAGE_NO IS NULL ")
        End With

        Using query As New DBSelectQuery(Of SC3190402DataSet.NotAssociatedRoInfoDataTable)("SC3190402_013")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)

            sql = Nothing

            Using dt As SC3190402DataSet.NotAssociatedRoInfoDataTable = query.GetData
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                '                          "{0} QUERY:COUNT = {1}", _
                '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                          dt.Count))
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                Return dt
            End Using
        End Using
    End Function
#End Region

#Region "未紐付けRO情報更新"
    ''' <summary>
    ''' 未紐付けRO情報更新
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="cageNo">かご番号</param>
    ''' <param name="roNum">R/O番号</param>
    ''' <param name="roSeq">R/O番号連番</param>
    ''' <param name="shipmentNo">出庫表番号</param>
    ''' <param name="useCount">利用回数</param>
    ''' <param name="nowDate">現在日付</param>
    ''' <param name="account">アカウント</param>
    ''' <returns>更新件数</returns>
    Public Shared Function UpdateNotAssociatedRoInfo(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal cageNo As String, _
                                        ByVal roNum As String, _
                                        ByVal roSeq As Integer, _
                                        ByVal shipmentNo As String, _
                                        ByVal useCount As Integer, _
                                        ByVal nowDate As Date, _
                                        ByVal account As String) As Integer

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} P3:{3} P4:{4} P5:{5} P6:{6} ", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode, _
        '                          roNum, _
        '                          roSeq, _
        '                          shipmentNo, _
        '                          useCount))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim sql As New StringBuilder
        With sql
            .Append(" UPDATE /* SC3190402_014 */ ")
            .Append("        TB_T_CAGE_INFO ")
            .Append("    SET RO_NUM = :RO_NUM ")
            .Append("      , RO_SEQ = :RO_SEQ ")
            .Append("      , SHIPMENT_NO = :SHIPMENT_NO ")
            .Append("      , USE_COUNT = :USE_COUNT ")
            .Append("      , ROW_UPDATE_DATETIME = :ROW_UPDATE_DATETIME ")
            .Append("      , ROW_UPDATE_ACCOUNT = :ROW_UPDATE_ACCOUNT ")
            .Append("      , ROW_UPDATE_FUNCTION = :ROW_UPDATE_FUNCTION ")
            .Append("      , ROW_LOCK_VERSION = ROW_LOCK_VERSION + 1")
            .Append("  WHERE DLR_CD = :DLR_CD ")
            .Append("    AND BRN_CD = :BRN_CD ")
            .Append("    AND CAGE_NO = :CAGE_NO ")
            .Append("    AND RO_NUM = :RO_NUM_DEFAULT ")
        End With

        Using query As New DBUpdateQuery("SC3190402_014")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("RO_NUM", OracleDbType.NVarchar2, roNum)
            query.AddParameterWithTypeValue("RO_SEQ", OracleDbType.Int32, roSeq)
            query.AddParameterWithTypeValue("SHIPMENT_NO", OracleDbType.NVarchar2, shipmentNo)
            query.AddParameterWithTypeValue("USE_COUNT", OracleDbType.NVarchar2, useCount)
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)
            query.AddParameterWithTypeValue("CAGE_NO", OracleDbType.NVarchar2, cageNo)
            query.AddParameterWithTypeValue("RO_NUM_DEFAULT", OracleDbType.NVarchar2, DBDefaultValueRoNum)
            query.AddParameterWithTypeValue("ROW_UPDATE_DATETIME", OracleDbType.Date, nowDate)
            query.AddParameterWithTypeValue("ROW_UPDATE_ACCOUNT", OracleDbType.NVarchar2, account)
            query.AddParameterWithTypeValue("ROW_UPDATE_FUNCTION", OracleDbType.NVarchar2, ApplicationId)

            sql = Nothing

            Dim updateCount As Integer = query.Execute()
            '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
            'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
            '              "{0} QUERY:COUNT = {1}", _
            '              System.Reflection.MethodBase.GetCurrentMethod.Name, _
            '              updateCount))
            '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
            Return updateCount
        End Using

    End Function
#End Region

#Region "かご番号取得"
    ''' <summary>
    ''' かご番号取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roNum">R/O番号</param>
    ''' <param name="roSeq">R/O番号連番</param>
    ''' <param name="shipmentNo">出庫表番号</param>
    ''' <returns>かご番号データセット</returns>
    Public Shared Function GetCageNo(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal roNum() As String, _
                                        ByVal roSeq() As Integer, _
                                        ByVal shipmentNo() As String) As SC3190402DataSet.CageNoInfoDataTable

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} P3:{3} P4:{4} P5:{5} ", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode, _
        '                          String.Join(ConsComma, roNum), _
        '                          String.Join(ConsComma, roSeq), _
        '                          String.Join(ConsComma, shipmentNo) _
        '                          ))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'IN句の作成
        Dim inCondition As List(Of String) = New List(Of String)
        For i As Integer = 0 To roNum.Length - 1
            Dim roNumRoSeqShipmentNo As String = String.Format(CultureInfo.CurrentCulture _
                                              , "('{0}','{1}','{2}')" _
                                              , roNum(i) _
                                              , roSeq(i) _
                                              , shipmentNo(i))
            inCondition.Add(roNumRoSeqShipmentNo)
        Next

        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3190402_015 */ ")
            .Append("        DLR_CD ")
            .Append("      , BRN_CD ")
            .Append("      , CAGE_NO ")
            .Append("      , RO_NUM ")
            .Append("      , RO_SEQ ")
            .Append("      , SHIPMENT_NO ")
            .Append("   FROM TB_T_CAGE_INFO ")
            .Append("  WHERE DLR_CD = :DLR_CD ")
            .Append("    AND BRN_CD = :BRN_CD ")
            .Append("    AND (RO_NUM,RO_SEQ,SHIPMENT_NO) IN (")
            .Append(String.Join(ConsComma, inCondition))
            .Append(") ")
        End With

        Using query As New DBSelectQuery(Of SC3190402DataSet.CageNoInfoDataTable)("SC3190402_015")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)

            sql = Nothing

            Using dt As SC3190402DataSet.CageNoInfoDataTable = query.GetData
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                '                          "{0} QUERY:COUNT = {1}", _
                '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                          dt.Count))
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                Return dt
            End Using
        End Using

    End Function
#End Region

#Region "紐付け済みRO情報取得"
    ''' <summary>
    ''' 紐付け済みRO情報取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <param name="roNum">R/O番号(表示対象のR/O番号全て)</param>
    ''' <param name="roSeq">R/O番号連番(表示対象のR/O番号連番全て)</param>
    ''' <param name="billNo">出庫表番号(表示対象の出庫表番号全て)</param>
    ''' <returns>未紐付けRO情報データテーブル</returns>
    Public Shared Function GetAssociatedRoInfo(ByVal dealerCode As String, _
                                        ByVal branchCode As String, _
                                        ByVal roNum() As String, _
                                        ByVal roSeq() As Integer, _
                                        ByVal billNo() As String) As SC3190402DataSet.AssociatedRoInfoDataTable

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} P1:{1} P2:{2} P3:{3} P4:{4} P5:{5} ", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode, _
        '                          String.Join(ConsComma, roNum), _
        '                          String.Join(ConsComma, roSeq), _
        '                          String.Join(ConsComma, billNo)))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        'IN句の作成
        Dim inCondition As List(Of String) = New List(Of String)
        For i As Integer = 0 To roNum.Length - 1
            Dim roNumRoSeq As String = String.Format(CultureInfo.CurrentCulture _
                                              , "('{0}','{1}','{2}')" _
                                              , roNum(i) _
                                              , roSeq(i) _
                                              , billNo(i))
            inCondition.Add(roNumRoSeq)
        Next

        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3190402_016 */ ")
            .Append("        DLR_CD ")
            .Append("      , BRN_CD ")
            .Append("      , RO_NUM ")
            .Append("      , RO_SEQ ")
            .Append("      , SHIPMENT_NO ")
            .Append("   FROM TB_T_CAGE_INFO ")
            .Append("  WHERE DLR_CD = :DLR_CD ")
            .Append("    AND BRN_CD = :BRN_CD ")
            .Append("    AND (RO_NUM,RO_SEQ,SHIPMENT_NO) IN (")
            .Append(String.Join(ConsComma, inCondition))
            .Append(") ")
        End With

        Using query As New DBSelectQuery(Of SC3190402DataSet.AssociatedRoInfoDataTable)("SC3190402_016")
            query.CommandText = sql.ToString()
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)

            sql = Nothing

            Using dt As SC3190402DataSet.AssociatedRoInfoDataTable = query.GetData
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
                'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
                '                          "{0} QUERY:COUNT = {1}", _
                '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
                '                          dt.Count))
                '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END
                Return dt
            End Using
        End Using
    End Function
#End Region

    '$01 部品庫B／O管理に向けた評価用アプリ作成 END

    'Shared Function newRow() As Object
    '    Throw New NotImplementedException
    'End Function

    '$03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 START
#Region "店舗営業時間取得"
    ''' <summary>
    ''' 店舗営業時間取得
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="branchCode">店舗コード</param>
    ''' <returns>BranchWorkTimeDataTable(店舗営業時間情報)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetBranchWorkTime(ByVal dealerCode As String, ByVal branchCode As String) _
                                          As SC3190402DataSet.BranchWorkTimeDataTable

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} dealerCode:{1} branchCode:{2} ", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          dealerCode, _
        '                          branchCode))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        Dim branchWorkTimeDataTable As SC3190402DataSet.BranchWorkTimeDataTable = Nothing

        Dim sql As New StringBuilder
        With sql
            .Append(" SELECT /* SC3190402_017 */ ")
            .Append("        SVC_JOB_START_TIME ")
            .Append("      , SVC_JOB_END_TIME ")
            .Append("   FROM TB_M_BRANCH_DETAIL ")
            .Append("  WHERE DLR_CD = :DLR_CD ")
            .Append("    AND BRN_CD = :BRN_CD ")
        End With

        Using query As New DBSelectQuery(Of SC3190402DataSet.BranchWorkTimeDataTable)("SC3190402_017")
            query.CommandText = sql.ToString()
            sql = Nothing

            ' バインド変数
            query.AddParameterWithTypeValue("DLR_CD", OracleDbType.NVarchar2, dealerCode)
            query.AddParameterWithTypeValue("BRN_CD", OracleDbType.NVarchar2, branchCode)

            ' クエリ実行
            branchWorkTimeDataTable = query.GetData()

        End Using

        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 START
        'Logger.Info(String.Format(CultureInfo.CurrentCulture, _
        '                          "{0} QUERY:COUNT = {1}", _
        '                          System.Reflection.MethodBase.GetCurrentMethod.Name, _
        '                          branchWorkTimeDataTable.Count))
        '$06 TR-SVT-TMT-20161229-001 部品庫モニターでエラーを検知 END

        ' 検索結果返却
        Return branchWorkTimeDataTable

    End Function

#End Region
    '$03 DMS連携版サービスタブレット サービスアドバイザ情報連携追加開発 END

    'S.Natsume TR-SVT-TMT-20200710-001 ログへ出力される文字が多すぎるために発生するエラー START $09
    '    'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 START $08
    '#Region "システム設定データ取得"
    '    ''' <summary>
    '    ''' SC3190402_005:システム設定から設定値を取得する
    '    ''' </summary>
    '    ''' <param name="settingName">システム設定名</param>
    '    ''' <returns></returns>
    '    ''' <remarks></remarks>
    '    Public Shared Function GetSystemSettingValue(ByVal settingName As String) As SC3190402DataSet.SystemSettingDataTable


    '        Dim sql As New StringBuilder
    '        With sql
    '            .Append("SELECT /* SC3190402_018 */")
    '            .Append(" SETTING_VAL")
    '            .Append(" ")
    '            .Append("FROM")
    '            .Append(" ").Append(ConsTblNameMSystemSetting)
    '            .Append(" ")
    '            .Append("WHERE")
    '            .Append(" SETTING_NAME = :SETTING_NAME")
    '        End With

    '        Using query As New DBSelectQuery(Of SC3190402DataSet.SystemSettingDataTable)("SC3190402_018")
    '            query.CommandText = sql.ToString()

    '            query.AddParameterWithTypeValue("SETTING_NAME", OracleDbType.Char, settingName)

    '            sql = Nothing

    '            Using dt As SC3190402DataSet.SystemSettingDataTable = query.GetData
    '                Return dt
    '            End Using
    '        End Using
    '    End Function
    '#End Region
    '    'M.Sakamoto (トライ店システム評価)部品出庫オペレーションにおける遅れ管理機能強化検証 END $08
    'S.Natsume TR-SVT-TMT-20200710-001 ログへ出力される文字が多すぎるために発生するエラー END $09
End Class
