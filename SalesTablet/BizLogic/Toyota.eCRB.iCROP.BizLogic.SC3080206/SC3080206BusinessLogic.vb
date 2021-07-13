'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080206BusinessLogic.vb
'─────────────────────────────────────
'機能： 車両編集 (ビジネスロジック)
'補足： 
'作成： 2011/11/15 TCS 安田
'更新： 2013/06/30 TCS 趙   【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'更新： 2014/04/01 TCS 松月 【A STEP2】TMT不具合対応
'更新： 2014/05/01 TCS 松月 新PF残課題No.21
'更新： 2014/05/16 TCS 松月 TR-V4-GTMC140428004対応(仕様変更：活動区分を全車両データに反映)
'更新： 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1
'─────────────────────────────────────
Imports Toyota.eCRB.SystemFrameworks.Core
'2013/06/30 TCS 趙 2013/10対応版　既存流用 START
Imports Toyota.eCRB.SystemFrameworks.Web
'2013/06/30 TCS 趙 2013/10対応版　既存流用 END
Imports Toyota.eCRB.CustomerInfo.Details.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

''' <summary>
''' SC3080206(Edit Customer's Vehicle)
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
''' 
Public Class SC3080206BusinessLogic
    Inherits BaseBusinessComponent

    ''' <summary>
    ''' 自社客/未取引客フラグ (1：自社客)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const OrgCustflg As Integer = 1

    ''' <summary>
    ''' 自社客/未取引客フラグ (2：未取引客)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const NewCustflg As Integer = 2

    ''' <summary>
    ''' モード (０：新規登録モード)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ModeCreate As Integer = 0

    ''' <summary>
    ''' モード (１：編集モード)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ModeEdit As Integer = 1

    ''' <summary>
    ''' 中古車認証区分使用フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UsedFlgCpotype As String = "USED_FLG_CPOTYPE"

    ''' <summary>
    ''' テレマ機能使用可否フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UsedFlgTelema As String = "USED_FLG_TELEMA"

    ''' <summary>
    ''' 機能設定マスタ設定値　(1:使用可)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UseFuncstatus As Integer = 1

    ''' <summary>
    ''' 非表示フラグ (0：非表示)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const NoneDisplay As Short = 0

    ''' <summary>
    ''' 1:希望する
    ''' </summary>
    ''' <remarks></remarks>
    Public Const KibouSuru As String = "1"

    ''' <summary>
    ''' 文言マスタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const FueldvsStartDisplayno As Integer = 50100   '燃料
    Public Const FueldvsStartNewvcldvs As Integer = 50200   '新・中区分
    Public Const FueldvsStartMemregstatus As Integer = 50300   '会員ステータス
    Public Const FueldvsStartContractStatuss As Integer = 50400   '契約ステータス
    Public Const FueldvsStartContractStatussNo As Integer = 50499   '契約ステータス(契約なし)
    Public Const Unlimited As Integer = 50500   '契約ステータス(契約なし)
    Public Const FueldvsStartConnectDvs As Integer = 50600   '接続方法


    ''' <summary>
    ''' 新・中区分が1:Used(Car以外の場合は)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const NewvcldvsUsed As String = "1"

    ''' <summary>
    ''' 契約ｽﾃｰﾀｽ (4：契約中(Yes))
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ContractStatusKeiyaku As String = "4"

    ''' <summary>
    ''' 契約ｽﾃｰﾀｽ (2：申し込み中(Applying))
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ContractStatusMoushikomi As String = "2"

    ''' <summary>
    ''' 活動区分変更機能 (1:Call（Call画面の Customer Detailより変更）)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ACModffuncdvsValue As String = "1"

    ''' <summary>
    ''' 1:車両登録No表示フラグ
    ''' </summary>
    Public Const RegNoDispBtn As String = "1"

    ''' <summary>
    ''' 1:車両編集を表示する
    ''' </summary>
    Public Const VehicleOpenFlg As String = "1"

    ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
    Private Const SETTING_NAME_MODELYEAR_MIN As String = "L_MIN_MODEL_YEAR"
    ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

    ''' <summary>
    ''' 初期表示用フラグ情報取得
    ''' </summary>
    ''' <param name="vehicleDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>初期表示時の表示用フラグを取得する。</remarks>
    Public Shared Function GetInitializeFlg(ByVal vehicleDataTbl As SC3080206DataSet.SC3080206VehicleDataTable, ByRef msgId As Integer) As SC3080206DataSet.SC3080206VehicleDataTable

        msgId = 0
        Dim vehicleDataRow As SC3080206DataSet.SC3080206VehicleRow

        Dim settionFlg As Integer = 0
        Dim funcSetting As New FunctionSetting

        vehicleDataRow = vehicleDataTbl.Item(0)

        If (vehicleDataRow.CUSTFLG = OrgCustflg) Then
            '０：自社客

            '中古車認証区分使用フラグ
            Dim sysEnv As New SystemEnvSetting
            Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow
            sysEnvRow = sysEnv.GetSystemEnvSetting(UsedFlgCpotype)
            vehicleDataRow.ASSURANFLG = CType(sysEnvRow.PARAMVALUE, Short)

            'テレマ機能使用可否フラグ
            settionFlg = funcSetting.GetiCROPFunctionSetting(vehicleDataRow.DLRCD, UsedFlgTelema)
            vehicleDataRow.TELEMAFLG = CType(settionFlg, Short)

        Else
            vehicleDataRow.ASSURANFLG = NoneDisplay
            vehicleDataRow.TELEMAFLG = NoneDisplay
        End If

        Return vehicleDataTbl

    End Function

    ''' <summary>
    ''' 初期表示
    ''' </summary>
    ''' <param name="vehicleDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>車両情報を取得する。</remarks>
    Public Shared Function GetInitialize(ByVal vehicleDataTbl As SC3080206DataSet.SC3080206VehicleDataTable, ByRef msgId As Integer) As SC3080206DataSet.SC3080206VehicleDataTable

        msgId = 0
        Dim retVehicleDataTbl As SC3080206DataSet.SC3080206VehicleDataTable
        Dim retVehicleDataRow As SC3080206DataSet.SC3080206VehicleRow
        Dim vehicleDataRow As SC3080206DataSet.SC3080206VehicleRow

        vehicleDataRow = vehicleDataTbl.Item(0)

        If (vehicleDataRow.CUSTFLG = OrgCustflg) Then
            '０：自社客

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialize OrgCustflg")
            'ログ出力 End *****************************************************************************

            '自社客車両情報取得
            retVehicleDataTbl = SC3080206TableAdapter.GetOrgVehicle(vehicleDataRow.DLRCD, vehicleDataRow.VIN, vehicleDataRow.ORIGINALID)


            '取得できなかった場合の処理 (例外処理とする)
            'If (retVehicleDataTbl.Rows.Count = 0) Then
            '    Return custDataTbl
            'End If

            '検索結果のセット
            retVehicleDataRow = retVehicleDataTbl.Item(0)
            vehicleDataRow.SERIESNM = DBValueToTrim(retVehicleDataRow.SERIESNM)          'モデル
            vehicleDataRow.VIN = DBValueToTrim(retVehicleDataRow.VIN)                    'VIN
            vehicleDataRow.VCLREGNO = DBValueToTrim(retVehicleDataRow.VCLREGNO)          '車両登録No.
            vehicleDataRow.MAKERNAME = DBValueToTrim(retVehicleDataRow.MAKERNAME)        'メーカー
            If (Not retVehicleDataRow.IsGRADENull) Then
                vehicleDataRow.GRADE = retVehicleDataRow.GRADE                           'グレード
            End If
            vehicleDataRow.FUELDVS = DBValueToTrim(retVehicleDataRow.FUELDVS)            '燃料区分
            vehicleDataRow.BASETYPE = DBValueToTrim(retVehicleDataRow.BASETYPE)          '型式
            vehicleDataRow.BDYCLRCD = DBValueToTrim(retVehicleDataRow.BDYCLRCD)          '外鈑色コード
            vehicleDataRow.BDYCLRNM = DBValueToTrim(retVehicleDataRow.BDYCLRNM)          '外鈑色名称
            vehicleDataRow.ENGINENO = DBValueToTrim(retVehicleDataRow.ENGINENO)          'エンジンNo.
            vehicleDataRow.NEWVCLDVS = DBValueToTrim(retVehicleDataRow.NEWVCLDVS)        '新・中区分
            vehicleDataRow.STRCD = DBValueToTrim(retVehicleDataRow.STRCD)                '販売店舗
            If (Not retVehicleDataRow.IsACTVCTGRYIDNull) Then
                vehicleDataRow.ACTVCTGRYID = retVehicleDataRow.ACTVCTGRYID               'AC
            End If
            If (Not retVehicleDataRow.IsREASONIDNull) Then
                vehicleDataRow.REASONID = retVehicleDataRow.REASONID               '断念理由
            End If
            If (Not retVehicleDataRow.IsVCLREGDATENull) Then
                vehicleDataRow.VCLREGDATE = retVehicleDataRow.VCLREGDATE                     '車両登録日
            End If
            If (Not retVehicleDataRow.IsVCLDELIDATENull) Then
                vehicleDataRow.VCLDELIDATE = retVehicleDataRow.VCLDELIDATE                   '納車日
            End If
            vehicleDataRow.CPONM = DBValueToTrim(retVehicleDataRow.CPONM)                'CPO区分
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
            If (Not retVehicleDataRow.IsVCLLCVERNull) Then
                vehicleDataRow.VCLLCVER = retVehicleDataRow.VCLLCVER               '車両行ロックバージョン
            End If
            If (Not retVehicleDataRow.IsVCLDLRLCVERNull) Then
                vehicleDataRow.VCLDLRLCVER = retVehicleDataRow.VCLDLRLCVER               '販売店車両ロックバージョン
            End If
            If (Not retVehicleDataRow.IsREASONIDNull) Then
                vehicleDataRow.CSTVCLLCVER = retVehicleDataRow.CSTVCLLCVER               '販売店顧客車両ロックバージョン
            End If
            If (Not retVehicleDataRow.IsVCLIDNull) Then
                vehicleDataRow.VCLID = retVehicleDataRow.VCLID                       'VCLID
            End If
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

            '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
            vehicleDataRow.VCL_MILE = DBValueToTrim(retVehicleDataRow.VCL_MILE)          '走行距離
            vehicleDataRow.MODEL_YEAR = DBValueToTrim(retVehicleDataRow.MODEL_YEAR)      '年式
            If (Not retVehicleDataRow.IsLC_VCLDLRLCVERNull) Then
                vehicleDataRow.LC_VCLDLRLCVER = retVehicleDataRow.LC_VCLDLRLCVER         'ローカル販売店車両ロックバージョン
            End If
            '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

            Dim mileageHisDataTable As SC3080206DataSet.SC3080206MileageHisDataTable
            Dim mileageHisRow As SC3080206DataSet.SC3080206MileageHisRow


            '自社客車両最終入庫情報取得
            mileageHisDataTable = SC3080206TableAdapter.GetMileageHis(vehicleDataRow.DLRCD, vehicleDataRow.VIN, vehicleDataRow.ORIGINALID)

            If (mileageHisDataTable.Rows.Count > 0) Then

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialize mileageHisDataTable.Rows.Count > 0")
                'ログ出力 End *****************************************************************************

                mileageHisRow = mileageHisDataTable.Item(0)
                If (Not mileageHisRow.IsREGISTDATENull) Then
                    vehicleDataRow.REGISTDATE = mileageHisRow.REGISTDATE                    '取得日時
                End If
                If (Not mileageHisRow.IsMILEAGENull) Then
                    vehicleDataRow.MILEAGE = mileageHisRow.MILEAGE                          '走行距離
                End If
            End If

            Dim ownersiteDataTable As SC3080206DataSet.SC3080206OwnersiteDataTable
            Dim ownersiteRow As SC3080206DataSet.SC3080206OwnersiteRow

            'オーナーサイト情報取得
            ownersiteDataTable = SC3080206TableAdapter.GetOwnersite(vehicleDataRow.DLRCD, vehicleDataRow.VIN, vehicleDataRow.ORIGINALID)

            If (ownersiteDataTable.Rows.Count > 0) Then

                ownersiteRow = ownersiteDataTable.Item(0)
                vehicleDataRow.MEMSYSTEMID = ownersiteRow.MEMSYSTEMID                   'Owner's ID
                vehicleDataRow.MEMREGSTATUS = ownersiteRow.MEMREGSTATUS                 '会員ステータス

            End If

            'テレマ機能使用可否フラグ
            If (vehicleDataRow.TELEMAFLG = 1) Then

                'ログ出力 Start ***************************************************************************
                Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialize vehicleDataRow.TELEMAFLG = 1")
                'ログ出力 End *****************************************************************************

                Dim gbookDataTable As SC3080206DataSet.SC3080206GbookDataTable
                Dim gbookRow As SC3080206DataSet.SC3080206GbookRow
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
                'G-BOOK情報取得
                gbookDataTable = SC3080206TableAdapter.GetGbook(vehicleDataRow.MEMSYSTEMID, vehicleDataRow.VIN, vehicleDataRow.DLRCD)
                '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

                If (gbookDataTable.Rows.Count > 0) Then

                    gbookRow = gbookDataTable.Item(0)
                    vehicleDataRow.CONTRACT_STATUS = gbookRow.CONTRACT_STATUS               'テレマ契約の有無
                    If (Not gbookRow.IsCONTRACT_START_DATENull) Then
                        vehicleDataRow.CONTRACT_START_DATE = gbookRow.CONTRACT_START_DATE   '利用開始日
                    End If
                    vehicleDataRow.CONNECT_DVS = gbookRow.CONNECT_DVS                       '接続方法
                    vehicleDataRow.TELEMA_TELNUMBER1 = gbookRow.TELEMA_TELNUMBER1           '緊急連絡先１
                    vehicleDataRow.TELEMA_TELNUMBER2 = gbookRow.TELEMA_TELNUMBER2           '緊急連絡先２
                    vehicleDataRow.TELEMA_TELNUMBER3 = gbookRow.TELEMA_TELNUMBER3           '緊急連絡先３
                    If (Not gbookRow.IsCONTRACT_END_DATENull) Then
                        vehicleDataRow.CONTRACT_END_DATE = gbookRow.CONTRACT_END_DATE       '契約満了日
                    End If
                    vehicleDataRow.GBOOKFLG = gbookRow.GBOOKFLG                             'G-BOOK配信可否

                End If
            End If

        Else
            '１：未顧客

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("GetInitialize New")
            'ログ出力 End *****************************************************************************
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            '未取引客車両情報取得
            retVehicleDataTbl = SC3080206TableAdapter.GetNewVehicle(vehicleDataRow.DLRCD, vehicleDataRow.SEQNO)
            '2013/06/30 TCS 趙 2013/10対応版　既存流用  END
            '取得できなかった場合の処理 (例外処理とする)
            'If (retVehicleDataTbl.Rows.Count = 0) Then
            '    Return custDataTbl
            'End If

            '検索結果のセット
            retVehicleDataRow = retVehicleDataTbl.Item(0)
            vehicleDataRow.SERIESNM = DBValueToTrim(retVehicleDataRow.SERIESNM)         'モデル
            vehicleDataRow.VIN = DBValueToTrim(retVehicleDataRow.VIN)                   'VIN
            vehicleDataRow.VCLREGNO = DBValueToTrim(retVehicleDataRow.VCLREGNO)         '車両登録No.
            vehicleDataRow.MAKERNAME = DBValueToTrim(retVehicleDataRow.MAKERNAME)       'メーカー
            '納車日は、DELIDATE→VCLDELIDATEで変換する
            If (Not retVehicleDataRow.IsDELIDATENull) Then
                vehicleDataRow.VCLDELIDATE = retVehicleDataRow.DELIDATE                 '納車日
            End If
            vehicleDataRow.SEQNO = retVehicleDataRow.SEQNO                              'SEQ№

            '以下いらないそうです
            'Dim regNoDataTable As SC3080206DataSet.SC3080206RegNoDataTable
            'Dim regNoRow As SC3080206DataSet.SC3080206RegNoRow
            'Using da As New SC3080206DataTableTableAdapter
            '    'ゲート通過車両情報取得
            '    regNoDataTable = da.GetRegNo(vehicleDataRow.DLRCD, vehicleDataRow.STRCD)
            'End Using
            'If (regNoDataTable.Rows.Count > 0) Then
            '    regNoRow = regNoDataTable.Item(0)
            '    vehicleDataRow.VCLREGNO = regNoRow.VCLREGNO                 '車両登録No.
            'End If
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 START
            If (Not retVehicleDataRow.IsVCLLCVERNull) Then
                vehicleDataRow.VCLLCVER = retVehicleDataRow.VCLLCVER               '車両行ロックバージョン
            End If
            If (Not retVehicleDataRow.IsVCLDLRLCVERNull) Then
                vehicleDataRow.VCLDLRLCVER = retVehicleDataRow.VCLDLRLCVER               '販売店車両ロックバージョン
            End If
            If (Not retVehicleDataRow.IsVCLIDNull) Then
                vehicleDataRow.VCLID = retVehicleDataRow.VCLID                       'VCLID
            End If
            '2013/06/30 TCS 黄 2013/10対応版　既存流用 END

            '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
            vehicleDataRow.VCL_MILE = DBValueToTrim(retVehicleDataRow.VCL_MILE)          '走行距離
            vehicleDataRow.MODEL_YEAR = DBValueToTrim(retVehicleDataRow.MODEL_YEAR)      '年式
            If (Not retVehicleDataRow.IsLC_VCLDLRLCVERNull) Then
                vehicleDataRow.LC_VCLDLRLCVER = retVehicleDataRow.LC_VCLDLRLCVER         'ローカル販売店車両ロックバージョン
            End If
            '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

        End If

        Return vehicleDataTbl

    End Function

    ''' <summary>
    ''' バリデーション判定
    ''' </summary>
    ''' <param name="vehicleDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>バリデーションを判定する。</remarks>
    Public Shared Function CheckValidation(ByVal vehicleDataTbl As SC3080206DataSet.SC3080206VehicleDataTable, ByRef msgId As Integer) As Boolean

        msgId = 0
        Dim vehicleDataRow As SC3080206DataSet.SC3080206VehicleRow

        vehicleDataRow = vehicleDataTbl.Item(0)

        'TODO:入力チェックする対象を自客と、未顧客で変える

        'モデルが未入力の場合
        If (String.IsNullOrEmpty(vehicleDataRow.SERIESNM)) Then
            msgId = 50912
            Return False
        End If

        ''VINが入力されていることもしくは登録番号が入力されていること
        'If (String.IsNullOrEmpty(vehicleDataRow.VIN) AndAlso String.IsNullOrEmpty(vehicleDataRow.VCLREGNO)) Then
        '    msgId = 50913
        '    Return False
        'End If

        'モデルが32文字を超えている
        If (Not String.IsNullOrEmpty(vehicleDataRow.SERIESNM)) Then
            If (Validation.IsCorrectDigit(vehicleDataRow.SERIESNM, 32) = False) Then
                msgId = 50901
                Return False
            End If

            'モデルに絵文字が入っている
            If (Validation.IsValidString(vehicleDataRow.SERIESNM) = False) Then
                msgId = 50913
                Return False
            End If
        End If

        'メーカー名が128文字を超えている
        If (Not String.IsNullOrEmpty(vehicleDataRow.MAKERNAME)) Then
            If (Validation.IsCorrectDigit(vehicleDataRow.MAKERNAME, 128) = False) Then
                msgId = 50902
                Return False
            End If

            'メーカー名に絵文字が入っている
            If (Validation.IsValidString(vehicleDataRow.MAKERNAME) = False) Then
                msgId = 50914
                Return False
            End If
        End If

        'VINが128文字を超えている
        If (Not String.IsNullOrEmpty(vehicleDataRow.VIN)) Then
            If (Validation.IsCorrectDigit(vehicleDataRow.VIN, 128) = False) Then
                msgId = 50903
                Return False
            End If
            If (Validation.IsVin(vehicleDataRow.VIN) = False) Then
                msgId = 50906
                Return False
            End If
            'VINに絵文字が入っている
            If (Validation.IsValidString(vehicleDataRow.VIN) = False) Then
                msgId = 50915
                Return False
            End If
        End If

        '登録番号が32文字を超えている
        If (Not String.IsNullOrEmpty(vehicleDataRow.VCLREGNO)) Then
            If (Validation.IsCorrectDigit(vehicleDataRow.VCLREGNO, 32) = False) Then
                msgId = 50904
                Return False
            End If
            If (Validation.IsRegNo(vehicleDataRow.VCLREGNO) = False) Then
                msgId = 50907
                Return False
            End If
            '登録番号に絵文字が入っている
            If (Validation.IsValidString(vehicleDataRow.VCLREGNO) = False) Then
                msgId = 50916
                Return False
            End If
        End If

        '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        '走行距離
        If (Not String.IsNullOrEmpty(vehicleDataRow.VCL_MILE)) Then
            '走行距離が20文字を超えている
            If (Validation.IsCorrectDigit(vehicleDataRow.VCL_MILE, 20) = False) Then
                msgId = 2020904
                Return False
            End If
            '走行距離が数値以外
            Dim parseRslt As Double
            If (Not Double.TryParse(vehicleDataRow.VCL_MILE, parseRslt)) Then
                msgId = 2020905
                Return False
            End If
            '走行距離を小数点で分割
            Dim splittedMile As String() = vehicleDataRow.VCL_MILE.Split("."c)
            '分割後の要素数が１より大きい＝小数点が入力されている場合
            If splittedMile.Length > 1 Then
                '小数点以下の桁数が４桁より大きい場合はエラー
                If splittedMile(1).Length > 4 Then
                    msgId = 2020911
                    Return False
                End If
            End If
        End If
        '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

        'このパターンは、日付コントロールを使用するのでいらない
        '納車日の書式が間違っている
        '初回納車日の書式が間違っている

        Return True

    End Function

    ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
    ''' <summary>
    ''' 車両更新
    ''' </summary>
    ''' <param name="vehicleDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>車両情報を更新する。</remarks>
    <EnableCommit()>
    Public Function UpdateVehicle(ByVal vehicleDataTbl As SC3080206DataSet.SC3080206VehicleDataTable, ByRef msgId As Integer, ByVal actEditFlg As Integer) As Integer
        ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
        msgId = 0
        Dim ret As Integer = 1
        Dim vehicleDataRow As SC3080206DataSet.SC3080206VehicleRow

        vehicleDataRow = vehicleDataTbl.Item(0)

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
        If (vehicleDataRow.CUSTFLG = NewCustflg) Then
            SC3080206TableAdapter.SelectVehicleForLock(vehicleDataRow.SEQNO)
        End If
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        'ブランクを半角一文字スペースにする
        Call EditDataRow(vehicleDataRow)

        If (vehicleDataRow.CUSTFLG = OrgCustflg) Then

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateVehicle OrgCustflg")
            'ログ出力 End *****************************************************************************

            '０：自社客
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            Dim actvctgryid As String
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START DEL
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            If (Not vehicleDataRow.IsACTVCTGRYIDNull) Then  '活動区分ID
                actvctgryid = CStr(vehicleDataRow.ACTVCTGRYID)
            Else
                actvctgryid = " "
            End If
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START DEL
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            Dim account As String
            account = StaffContext.Current.Account
            Dim reasonid As String
            If (vehicleDataRow.IsREASONIDNull()) Then
                reasonid = " "
            Else
                reasonid = vehicleDataRow.REASONID
            End If
            '販売店管理車両情報更新
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify Start
            If actEditFlg = 1 Then
                '2014/05/16 TCS 松月 TR-V4-GTMC140428004対応 Modify Start
                ret = SC3080206TableAdapter.UpdateDlrCstVcl(actvctgryid, _
                                                            vehicleDataRow.AC_MODFFUNCDVS, _
                                                            reasonid, _
                                                            account, _
                                                            vehicleDataRow.DLRCD, _
                                                            CStr(vehicleDataRow.VCLID))
                '2014/05/16 TCS 松月 TR-V4-GTMC140428004対応 Modify End
                '2013/05/01 TCS 松月 新PF残課題No.21 Start
                If ret = 0 Then
                    Me.Rollback = True
                    Return -1
                End If
                ret = SC3080206TableAdapter.InsertCstVclActCat(vehicleDataRow.DLRCD, _
                                            CStr(vehicleDataRow.ORIGINALID), _
                                            actvctgryid, _
                                            reasonid, _
                                            account, _
                                            CStr(vehicleDataRow.VCLID))
                '2013/05/01 TCS 松月 新PF残課題No.21 End
            End If
            ' 2014/04/01 TCS 松月 TMT不具合対応 Modify End
            ''更新に失敗していたらロールバック
            If ret = 0 Then
                Me.Rollback = True
                Return -1
            End If
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        Else
            '１：未顧客

            'ログ出力 Start ***************************************************************************
            Toyota.eCRB.SystemFrameworks.Core.Logger.Info("UpdateVehicle NewCustflg")
            'ログ出力 End *****************************************************************************

            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START DEL
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
            '更新アカウント
            Dim account As String
            account = StaffContext.Current.Account
            '未取引客車両(車両)情報更新
            ret = SC3080206TableAdapter.UpdateNewcustomerVclre(vehicleDataRow.SERIESNM, _
                                vehicleDataRow.VIN, _
                                vehicleDataRow.MAKERNAME, _
                                account, _
                                vehicleDataRow.SEQNO, _
                                vehicleDataRow.VCLLCVER)
            ''更新に失敗していたらロールバック
            If ret = 0 Then
                Me.Rollback = True
                Return -1
            End If

            '納車日
            If vehicleDataRow.IsVCLDELIDATENull Then
                vehicleDataRow.VCLDELIDATE = Date.ParseExact("1900/01/01 00:00", "yyyy/MM/dd HH:mm", Nothing)
            End If

            '未取引客車両(販売店車両)情報更新
            ret = SC3080206TableAdapter.UpdateDlrVcl(vehicleDataRow.VCLREGNO, _
                                                     vehicleDataRow.VCLDELIDATE, _
                                                     account, _
                                                     vehicleDataRow.DLRCD, _
                                                     vehicleDataRow.SEQNO, _
                                                     vehicleDataRow.VCLDLRLCVER)
            ''更新に失敗していたらロールバック
            If ret = 0 Then
                Me.Rollback = True
                Return -1
            End If
        End If

        '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        Dim paramVclID As Decimal
        If vehicleDataRow.CUSTFLG = OrgCustflg Then
            paramVclID = vehicleDataRow.VCLID
        Else
            paramVclID = vehicleDataRow.SEQNO
        End If
        If SC3080206TableAdapter.GetCountDlrVclLocal(vehicleDataRow.DLRCD, paramVclID) > 0 Then
            '対象の販売店車両ローカルレコードが存在する場合
            '販売店車両ローカル更新
            ret = SC3080206TableAdapter.UpdateDlrVclLocal(vehicleDataRow.VCL_MILE, _
                                                         vehicleDataRow.MODEL_YEAR, _
                                                         StaffContext.Current.Account, _
                                                         vehicleDataRow.DLRCD, _
                                                         paramVclID, _
                                                         vehicleDataRow.LC_VCLDLRLCVER)
        Else
            '対象の販売店車両ローカルレコードが存在しない場合
            If Not String.IsNullOrWhiteSpace(vehicleDataRow.VCL_MILE) Or Not String.IsNullOrWhiteSpace(vehicleDataRow.MODEL_YEAR) Then
                '走行距離または年式が入力されている場合
                '販売店車両ローカル新規作成
                ret = SC3080206TableAdapter.InsertDlrVclLocal(vehicleDataRow.DLRCD, _
                                                   paramVclID, _
                                                   vehicleDataRow.VCL_MILE, _
                                                   vehicleDataRow.MODEL_YEAR, _
                                                   StaffContext.Current.Account)
            End If
        End If
        '更新に失敗していたらロールバック
        If ret = 0 Then
            Me.Rollback = True
            Return -1
        End If
        '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

        '誘致最新化メソッド実行
        If (vehicleDataRow.CUSTFLG = OrgCustflg) Then
            ret = InsertAttPlanNew(vehicleDataRow.ORIGINALID, vehicleDataRow.VCLID)
        Else
            ret = InsertAttPlanNew(vehicleDataRow.CSTID, vehicleDataRow.SEQNO)
        End If
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        '更新に失敗していたらロールバック
        If ret = 0 Then
            Me.Rollback = True
            Return 0
        End If
        Return ret

    End Function

    ''' <summary>
    ''' 車両新規登録
    ''' </summary>
    ''' <param name="vehicleDataTbl">データセット (インプット)</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>車両情報を登録する。</remarks>
    <EnableCommit()>
    Public Function InsertVehicle(ByVal vehicleDataTbl As SC3080206DataSet.SC3080206VehicleDataTable, ByRef msgId As Integer) As Integer

        msgId = 0
        Dim ret As Integer = 1
        Dim vehicleDataRow As SC3080206DataSet.SC3080206VehicleRow

        vehicleDataRow = vehicleDataTbl.Item(0)

        'ブランクを半角一文字スペースにする
        Call EditDataRow(vehicleDataRow)

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
        '顧客シーケンス采番                          
        vehicleDataRow.SEQNO = SC3080206TableAdapter.GetNewcustVclseq()
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
        'ログ出力 Start ***************************************************************************
        Toyota.eCRB.SystemFrameworks.Core.Logger.Info("InsertVehicle vehicleDataRow.SEQNO = " + CType(vehicleDataRow.SEQNO, String))
        'ログ出力 End *****************************************************************************

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
        '未取引客車両(販売店顧客車両)情報新規作成
        '更新アカウント
        Dim account As String
        account = StaffContext.Current.Account
        ret = SC3080206TableAdapter.InsertNewcustomerVclre(vehicleDataRow.DLRCD, _
                                        vehicleDataRow.CSTID, _
                                        vehicleDataRow.SEQNO, _
                                        vehicleDataRow.STRCD, _
                                        account)
        '未取引客車両(販売店顧客車両)初期情報削除
        ret = SC3080206TableAdapter.DeleteNewcustomerVclre(vehicleDataRow.DLRCD, _
                                        vehicleDataRow.CSTID)

        '未取引客車両情報（車両）新規作成
        ret = SC3080206TableAdapter.InsertVcl(vehicleDataRow.SEQNO, _
                                        vehicleDataRow.VIN, _
                                        vehicleDataRow.SERIESNM, _
                                        vehicleDataRow.MAKERNAME, _
                                        vehicleDataRow.MODELCODE, _
                                        account)

        '納車日
        If vehicleDataRow.IsVCLDELIDATENull Then
            vehicleDataRow.VCLDELIDATE = Date.ParseExact("1900/01/01 00:00", "yyyy/MM/dd HH:mm", Nothing)
        End If

        '未取引客車両（販売店車両）情報新規作成
        SC3080206TableAdapter.InsertDlrVcl(vehicleDataRow.DLRCD, _
                                           vehicleDataRow.SEQNO, _
                                           vehicleDataRow.VCLDELIDATE, _
                                           vehicleDataRow.VCLREGNO, _
                                           account)

        '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        '走行距離または年式が入力されている場合
        If Not String.IsNullOrWhiteSpace(vehicleDataRow.VCL_MILE) Or Not String.IsNullOrWhiteSpace(vehicleDataRow.MODEL_YEAR) Then
            '販売店車両ローカル新規作成
            ret = SC3080206TableAdapter.InsertDlrVclLocal(vehicleDataRow.DLRCD, _
                                               vehicleDataRow.SEQNO, _
                                               vehicleDataRow.VCL_MILE, _
                                               vehicleDataRow.MODEL_YEAR, _
                                               account)
        End If
        '2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

        '誘致最新化メソッド実行 
        ret = InsertAttPlanNew(vehicleDataRow.CSTID, vehicleDataRow.SEQNO)

        '更新に失敗していたらロールバック
        If ret = 0 Then
            Me.Rollback = True
            Return 0
        End If

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        Return ret

    End Function

    ' ''' <summary>
    ' ''' メーカー名取得
    ' ''' </summary>
    ' ''' <param name="vehicleDataTbl">データセット (インプット)</param>
    ' ''' <param name="msgId">メッセージID</param>
    ' ''' <returns>データセット (アウトプット)</returns>
    ' ''' <remarks>メーカー名を取得する。</remarks>
    'Public Function GetMaker(ByVal vehicleDataTbl As SC3080206DataSet.SC3080206VehicleDataTable, ByRef msgId As Integer) As SC3080206DataSet.SC3080206MakerDataTable

    '    Dim vehicleDataRow As SC3080206DataSet.SC3080206VehicleRow

    '    vehicleDataRow = vehicleDataTbl.Item(0)

    '    Using da As New SC3080206DataTableTableAdapter
    '        Return da.GetMaker(vehicleDataRow.DLRCD, vehicleDataRow.MAKERCD)
    '    End Using

    'End Function

    ''' <summary>
    ''' 断念リスト取得
    ''' </summary>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>断念リストを取得する。</remarks>
    Public Shared Function GetGiveupReason(ByRef msgId As Integer) As SC3080206DataSet.SC3080206GiveupReasonDataTable

        msgId = 0

        Return SC3080206TableAdapter.GetGiveupReason()

    End Function

    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 誘致最新化
    ''' </summary>
    ''' <param name="originalid">顧客ID</param>
    ''' <param name="vclid">車両ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    <EnableCommit()>
    Public Function InsertAttPlanNew(ByRef originalid As Decimal, ByRef vclid As Decimal) As Integer
        Dim subcustTbl As SC3080206DataSet.SC3080206PeriodDataTable
        Dim subcustTbl2 As SC3080206DataSet.SC3080206AttCstDataTable
        Dim subcustTbl3 As SC3080206DataSet.SC3080206PlanNewDataTable
        Dim SEQ_NO As Decimal
        Dim context As StaffContext = StaffContext.Current
        Dim account As String = context.Account
        Dim ret As Integer = 1

        '定期点検誘致最新化用情報取得
        subcustTbl = SC3080206TableAdapter.SelectCstVclforPeriodTgt(vclid)
        If (subcustTbl.Rows.Count > 0) Then
            For index = 0 To subcustTbl.Rows.Count - 1
                '定期点検誘致最新化シーケンス取得
                SEQ_NO = SC3080206TableAdapter.GetSqPeriodTgt().Item(0).SEQ
                '定期点検誘致最新化
                ret = SC3080206TableAdapter.InsertPeriodTgt(SEQ_NO, _
                                                            subcustTbl.Item(index).DLRCD, _
                                                            subcustTbl.Item(index).VCLID, _
                                                            account)
                '誘致グループ所属車両最新化シーケンス取得
                SEQ_NO = SC3080206TableAdapter.GetSqAttGroupVclTgt().Item(0).SEQ
                '誘致グループ所属車両最新化
                ret = SC3080206TableAdapter.InsertAttGroupVclTgt(SEQ_NO, _
                                                                 subcustTbl.Item(index).DLRCD, _
                                                                 subcustTbl.Item(index).VCLID, _
                                                                 account)
            Next
        End If

        '誘致グループ所属顧客最新化用情報取得
        subcustTbl2 = SC3080206TableAdapter.SelectAttGroupCstTgt(originalid) '顧客ID
        If (subcustTbl2.Rows.Count > 0) Then
            For index = 0 To subcustTbl2.Rows.Count - 1
                '誘致グループ所属顧客最新化シーケンス取得
                SEQ_NO = SC3080206TableAdapter.GetSqAttGroupCstTgt().Item(0).SEQ

                '誘致グループ所属顧客最新化
                ret = SC3080206TableAdapter.InsertAttGroupCstTgt(SEQ_NO, subcustTbl2.Item(index).DLRCD, _
                                                                 subcustTbl2.Item(index).CSTID, _
                                                                 account)
            Next
        End If
        '起点日誘致最新化シーケンス取得
        SEQ_NO = SC3080206TableAdapter.GetSqSpecifyTgt().Item(0).SEQ
        '起点日誘致最新化
        ret = SC3080206TableAdapter.InsertSpecifyTgt(SEQ_NO, _
                                                     context.DlrCD, _
                                                     originalid, _
                                                     vclid, _
                                                     account)

        '誘致最新化用情報取得
        subcustTbl3 = SC3080206TableAdapter.SelectPlanNewTgt(originalid)

        If (subcustTbl3.Rows.Count > 0) Then
            For index = 0 To subcustTbl3.Rows.Count - 1
                '誘致最新化シーケンス取得
                SEQ_NO = SC3080206TableAdapter.GetSqPlanNewTgt().Item(0).SEQ
                '誘致最新化
                ret = SC3080206TableAdapter.InsertPlanNewTgt(SEQ_NO, _
                                                             subcustTbl3.Item(index).DLRCD, _
                                                             subcustTbl3.Item(index).CSTID, _
                                                             subcustTbl3.Item(index).VCLID, _
                                                             account)
            Next
        End If

        If ret = 0 Then
            Me.Rollback = True
            Return 0
        End If

        Return ret
    End Function
    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

    ' ''' <summary>
    ' ''' 車両登録No取得
    ' ''' </summary>
    ' ''' <param name="vehicleDataTbl">データセット (インプット)</param>
    ' ''' <param name="msgId">メッセージID</param>
    ' ''' <returns>データセット (アウトプット)</returns>
    ' ''' <remarks>車両取得Noをゲート通過情報から取得する。</remarks>
    'Public Function GetRegno(ByVal vehicleDataTbl As SC3080206DataSet.SC3080206VehicleDataTable, ByRef msgId As Integer) As Long

    '    msgId = 0
    '    Dim regNoDataTable As SC3080206DataSet.SC3080206RegNoDataTable
    '    Dim regNoRow As SC3080206DataSet.SC3080206RegNoRow
    '    Dim vehicleDataRow As SC3080206DataSet.SC3080206VehicleRow

    '    vehicleDataRow = vehicleDataTbl.Item(0)

    '    Using da As New SC3080206DataTableTableAdapter
    '        regNoDataTable = da.GetRegNo(vehicleDataRow.DLRCD, vehicleDataRow.STRCD)
    '    End Using

    '    If (regNoDataTable.Rows.Count > 0) Then

    '        regNoRow = regNoDataTable.Item(0)

    '        Return CLng(regNoRow.VCLREGNO)            '車両登録No.

    '    Else

    '        'TODO:ない場合の処理
    '        Return 0

    '    End If

    'End Function

    ' 空白を、半角スペース１文字に変換する (DBに空白の場合に、半角スペース１文字しとして出力されているため)
    Protected Shared Function BlanckToSpace1(ByVal val As String) As String

        If (String.IsNullOrEmpty(val)) Then
            Return " "
        End If

        Return val

    End Function


    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    ' 空白を、半角スペース１文字に変換する (DBに空白の場合に、半角スペース１文字しとして出力されているため)
    Protected Shared Function BlanckToSpaceTrim1(ByVal val As String) As String

        If (String.IsNullOrEmpty(Trim(val))) Then
            Return " "
        End If

        Return Trim(val)

    End Function
    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END


    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    ' 空白を、半角スペース１文字に変換する
    Protected Shared Sub EditDataRow(ByVal vehicleDataRow As SC3080206DataSet.SC3080206VehicleRow)
    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        vehicleDataRow.MAKERNAME = BlanckToSpace1(vehicleDataRow.MAKERNAME)     'メーカー
        vehicleDataRow.MODELCODE = BlanckToSpace1(vehicleDataRow.MODELCODE)     'モデル (この画面では未使用)

        vehicleDataRow.SERIESNM = BlanckToSpace1(vehicleDataRow.SERIESNM)       'モデル

        vehicleDataRow.VCLREGNO = BlanckToSpaceTrim1(vehicleDataRow.VCLREGNO)   '車両登録No.
        vehicleDataRow.VIN = BlanckToSpaceTrim1(vehicleDataRow.VIN)             'VIN
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
        ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START
        vehicleDataRow.MODEL_YEAR = BlanckToSpaceTrim1(vehicleDataRow.MODEL_YEAR)   '年式
        vehicleDataRow.VCL_MILE = BlanckToSpaceTrim1(vehicleDataRow.VCL_MILE)       '走行距離
        ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

    End Sub

    ' Trimする (DBに空白の場合に、半角スペース１文字しとして出力されているため)
    Protected Shared Function DBValueToTrim(ByVal val As String) As String

        If (String.IsNullOrEmpty(val) = True) Then
            Return String.Empty
        End If

        Return val.Trim

    End Function

    ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 START

    ''' <summary>
    ''' 年式リスト取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetModelYear() As SC3080206DataSet.SC3080206ModelYearDataTable
        Logger.Info("GetModelYear Start")

        ' 変数
        Dim modelYearMin As Integer   '年式の下限値
        Dim modelYearMax As Integer   '年式の上限値
        Dim settingData As SC3080206DataSet.SC3080206SystemSettingDataTable = Nothing

        '年式の下限値を取得
        settingData = SC3080206TableAdapter.GetSystemSetting(SETTING_NAME_MODELYEAR_MIN)
        If Not settingData Is Nothing AndAlso settingData.Count > 0 Then
            Integer.TryParse(settingData(0).SETTING_VAL, modelYearMin)
        End If

        '年式の上限値を取得
        modelYearMax = Now.Year

        Using dtModelYear As New SC3080206DataSet.SC3080206ModelYearDataTable
            '下限値・上限値ともに有効値を取得できた場合
            If modelYearMin > 0 AndAlso modelYearMax >= modelYearMin Then
                '下限値から上限値までの値を文字列に変換して返却値にセット
                For i As Integer = modelYearMax To modelYearMin Step -1
                    Dim dtModelYearRow As SC3080206DataSet.SC3080206ModelYearRow = dtModelYear.NewSC3080206ModelYearRow()
                    dtModelYearRow.MODEL_YEAR = i.ToString()
                    dtModelYear.Rows.Add(dtModelYearRow)
                Next
            End If
            Return dtModelYear
        End Using

        Logger.Info("GetModelYear End")
    End Function
    ' 2018/06/18 TCS 前田 TKM Next Gen e-CRB Project Application development Block B-1 END

End Class
