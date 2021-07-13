'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3080201BusinessLogic.vb
'─────────────────────────────────────
'機能： 顧客詳細共通処理
'補足： 
'作成：  
'更新： 2012/01/27 TCS 河原 【SALES_1B】
'更新： 2012/06/01 TCS 河原 FS開発
'更新： 2012/08/13 TCS 安田 商談中断メニューの追加
'更新： 2012/09/06 TCS 山口 【A.STEP2】次世代e-CRB 新車受付機能改善
'更新： 2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発
'更新： 2013/03/12 TCS 渡邊 【A.STEP2】新車タブレット受付画面の管理指標変更対応
'更新： 2013/03/06 TCS 河原 GL0874 
'更新： 2013/06/30 TCS 庄 【A STEP2】i-CROP新DB適応に向けた機能開発（既存流用）
'─────────────────────────────────────

Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess
Imports Toyota.eCRB.Tool.Notify.Api.BizLogic
Imports Toyota.eCRB.Tool.Notify.Api.DataAccess
Imports Toyota.eCRB.iCROP.DataAccess.SC3080201

''' <summary>
''' SC3080201()
''' Webページで使用するビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class SC3080201BusinessLogic
    Inherits BaseBusinessComponent
    Implements ISC3080201BusinessLogic
#Region " 定数 "

    ''' <summary>
    ''' 自社客/未取引客フラグ (1：自社客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ORGCUSTFLG As String = "1"

    ''' <summary>
    ''' 自社客/未取引客フラグ (2：未取引客)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEWCUSTFLG As String = "2"

    ''' <summary>
    ''' 1: 名前の前に敬称(主に英語圏)、2: 名前の後ろに敬称(中国など)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONSTKEISYOZENGO As String = "KEISYO_ZENGO"

    '2012/02/15 TCS 山口 【SALES_2】Add
    ''' <summary>
    ''' 完了日の表示範囲
    ''' </summary>
    ''' <remarks></remarks>
    Private Const COMPLAINT_DISPLAYDATE As String = "COMPLAINT_DISPLAYDATE"

    ''' <summary>
    ''' 顔写真の保存先フォルダ(Native向け)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONSTFACEPICUPLOADPATH As String = "FACEPIC_UPLOADPATH"

    ''' <summary>
    ''' 顔写真の保存先フォルダ(Web向け)
    ''' </summary>
    ''' <remarks></remarks>
    Private Const CONSTFACEPICUPLOADURL As String = "FACEPIC_UPLOADURL"

    Private Const OCCUPATIONOTHER As String = "1" '1: その他

    Private Const MODULEID As String = "SC3080201"

    ' 2012/02/15 TCS 相田 【SALES_2】 START
    'Private Const C_SALES_START As String = "1"
    'Private Const C_SALES_CANCEL As String = "2"
    'Private Const C_BUSINESS_START As String = "3"
    'Private Const C_BUSINESS_CANCEL As String = "4"
    'Private Const C_CORRESPOND_START As String = "5"
    'Private Const C_CORRESPOND_END As String = "6"

    Private Const C_SALES_START As String = "1"
    Private Const C_SALES_END As String = "2"
    Private Const C_BUSINESS_START As String = "3"
    Private Const C_BUSINESS_CANCEL As String = "4"
    Private Const C_CORRESPOND_START As String = "5"
    Private Const C_CORRESPOND_END As String = "6"

    ' 2012/08/13 TCS 安田 商談中断メニューの追加 START
    Private Const C_SALES_STOP As String = "7"
    ' 2012/08/13 TCS 安田 商談中断メニューの追加 END

    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add Start
    '納車作業開始
    Private Const C_DELIVERY_START As String = "8"
    '納車作業終了
    Private Const C_DELIVERY_END As String = "9"
    '納車作業開始(一時対応)
    Private Const C_DELIVERYCORRESPOND_START As String = "10"
    '納車作業終了(一時対応)
    Private Const C_DELIVERYCORRESPOND_END As String = "11"
    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Add End

    ''' <summary>
    ''' 登録フラグ　未登録
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REGISTFLG_NOTREGIST As String = "0"
    ''' <summary>
    ''' 登録フラグ　登録済み
    ''' </summary>
    ''' <remarks></remarks>
    Private Const REGISTFLG_REGIST As String = "1"
    ''' <summary>
    ''' 新規活動フラグ　新規
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEWFLLWUPBOXFLG_NEW As String = "1"
    ''' <summary>
    ''' 新規活動フラグ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const NEWFLLWUPBOXFLG_NOTNEW As String = "0"

    ' 2012/02/15 TCS 相田 【SALES_2】 END

    '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 機能パラメータ (All)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UsedFlgContactAll As String = "USED_FLG_CONTACT"
    ''' <summary>
    ''' 機能パラメータ (SMS)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UsedFlgContactSms As String = "USED_FLG_SMS"

    ''' <summary>
    ''' 機能パラメータ (E-Mail)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UsedFlgContactEmail As String = "USED_FLG_E-MAIL"
    ''' <summary>
    ''' 機能パラメータ (E-Mail)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const UsedFlgContactDmail As String = "USED_FLG_D-MAIL"
    '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

#End Region

    ''' <summary>
    ''' 活動先の顧客情報を取得する。
    ''' </summary>
    ''' <param name="dtParam">引数</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Public Shared Function GetCustInfo(ByVal dtParam As SC3080201DataSet.SC3080201CustInfoDataTable) As SC3080201DataSet.SC3080201CustInfoDataTable

        If Not (dtParam IsNot Nothing AndAlso dtParam.Count >= 0) Then
            '検証エラー
            Throw New ArgumentException("SC3080201BusinessLogic.GetCustInfo", "dtParam")
        End If

        Dim dt As SC3080201DataSet.SC3080201CustInfoDataTable


        '検索処理
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Dim vclid As String = Nothing
        If (Not dtParam(0).IsVCLIDNull) Then
            vclid = dtParam(0).VCLID
        End If
        dt = SC3080201TableAdapter.GetCustInfo(custKind:=dtParam(0).CUSTKIND, custId:=dtParam(0).CUSTID, dlr_cd:=dtParam(0).DLRCD, vcl_id:=vclid)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        '処理結果返却
        Return dt

    End Function

#Region " 初期用データ取得処理 "
    ''' <summary>
    ''' 自社客取得
    ''' </summary>
    ''' <param name="inCustomerDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>自社客を取得する処理</remarks>
    Public Shared Function GetOrgCustomerData(ByVal inCustomerDataTbl As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201OrgCustomerDataTable
        Dim customerDataRow As SC3080201DataSet.SC3080201ParameterRow
        customerDataRow = CType(inCustomerDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        Dim outCustomerDataTbl As New SC3080201DataSet.SC3080201OrgCustomerDataTable

        '自社客取得
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        outCustomerDataTbl = SC3080201TableAdapter.GetOrgCustomer(customerDataRow.DLRCD, customerDataRow.CRCUSTID, customerDataRow.VCLID)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

        For Each drOutCustomerDataTbl In outCustomerDataTbl
            '敬称位置取得
            sysEnvRow = sysEnv.GetSystemEnvSetting(CONSTKEISYOZENGO)
            drOutCustomerDataTbl.KEISYO_ZENGO = sysEnvRow.PARAMVALUE

            '顔写真の保存先フォルダ(Native向け)取得
            sysEnvRow = sysEnv.GetSystemEnvSetting(CONSTFACEPICUPLOADPATH)
            drOutCustomerDataTbl.FACEPIC_UPLOADPATH = sysEnvRow.PARAMVALUE

            '顔写真の保存先フォルダ(Web向け)取得
            sysEnvRow = sysEnv.GetSystemEnvSetting(CONSTFACEPICUPLOADURL)
            drOutCustomerDataTbl.FACEPIC_UPLOADURL = sysEnvRow.PARAMVALUE
        Next

        Return outCustomerDataTbl
    End Function

    ''' <summary>
    ''' 未取引客取得
    ''' </summary>
    ''' <param name="inCustomerDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>未取引客を取得する処理</remarks>
    Public Shared Function GetNewCustomerData(ByVal inCustomerDataTbl As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201NewCustomerDataTable
        Dim customerDataRow As SC3080201DataSet.SC3080201ParameterRow
        customerDataRow = CType(inCustomerDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        Dim outCustomerDataTbl As New SC3080201DataSet.SC3080201NewCustomerDataTable

        '未取引客取得
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        outCustomerDataTbl = SC3080201TableAdapter.GetNewCustomer(customerDataRow.DLRCD, customerDataRow.CRCUSTID, customerDataRow.VCLID)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

        For Each drOutCustomerDataTbl In outCustomerDataTbl
            '敬称位置取得
            sysEnvRow = sysEnv.GetSystemEnvSetting(CONSTKEISYOZENGO)
            drOutCustomerDataTbl.KEISYO_ZENGO = sysEnvRow.PARAMVALUE

            '顔写真の保存先フォルダ(Native向け)取得
            sysEnvRow = sysEnv.GetSystemEnvSetting(CONSTFACEPICUPLOADPATH)
            drOutCustomerDataTbl.FACEPIC_UPLOADPATH = sysEnvRow.PARAMVALUE

            '顔写真の保存先フォルダ(Web向け)取得
            sysEnvRow = sysEnv.GetSystemEnvSetting(CONSTFACEPICUPLOADURL)
            drOutCustomerDataTbl.FACEPIC_UPLOADURL = sysEnvRow.PARAMVALUE
        Next

        Return outCustomerDataTbl
    End Function

    ''' <summary>
    ''' 自社客車両取得
    ''' </summary>
    ''' <param name="inVehicleDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>自社客車両データを取得する処理</remarks>
    Public Shared Function GetOrgVehicleData(ByVal inVehicleDataTbl As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201OrgVehicleDataTable
        Dim vehicleDataRow As SC3080201DataSet.SC3080201ParameterRow
        vehicleDataRow = CType(inVehicleDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        '自社客車両取得
        Return (SC3080201TableAdapter.GetOrgCustomerVehicle(vehicleDataRow.CRCUSTID, vehicleDataRow.DLRCD))
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    End Function

    ''' <summary>
    ''' 未取引客車両取得
    ''' </summary>
    ''' <param name="inVehicleDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>自社客車両データを取得する処理</remarks>
    Public Shared Function GetNewVehicleData(ByVal inVehicleDataTbl As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201NewVehicleDataTable
        Dim vehicleDataRow As SC3080201DataSet.SC3080201ParameterRow
        vehicleDataRow = CType(inVehicleDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        '未取引客車両取得
        Return (SC3080201TableAdapter.GetNewCustomerVehicle(vehicleDataRow.CRCUSTID, vehicleDataRow.DLRCD))
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    End Function

    ''' <summary>
    ''' 顧客職業取得
    ''' </summary>
    ''' <param name="inOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>自社客車両データを取得する処理</remarks>
    Public Shared Function GetOccupationData(ByVal inOccupationDataTbl As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201CustomerOccupationDataTable
        Dim occupationDataRow As SC3080201DataSet.SC3080201ParameterRow
        occupationDataRow = CType(inOccupationDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        '顧客職業取得
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Return (SC3080201TableAdapter.GetCustomerOccupation(occupationDataRow.DLRCD, _
                                                                        occupationDataRow.STRCD, _
                                                                        occupationDataRow.CRCUSTID))
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    End Function

    ''' <summary>
    ''' 取得した顧客職業を編集
    ''' </summary>
    ''' <param name="occupationDataTbl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function EditOccupatonData(ByVal occupationDataTbl As SC3080201DataSet.SC3080201CustomerOccupationDataTable) As SC3080201DataSet.SC3080201CustomerOccupationDataTable

        Dim count As Integer
        Dim mOtherRow As Integer
        Dim otherRow As Integer
        Dim other As String = String.Empty

        For Each drControloccupation In occupationDataTbl
            If String.Equals(drControloccupation.OTHER, OCCUPATIONOTHER) Then
                mOtherRow = count
            End If
            If String.Equals(drControloccupation.SORTNO_1ST, "2") And _
               drControloccupation.SORTNO_2ND = 0 Then
                otherRow = count
                other = drControloccupation.OCCUPATION
            End If
            count = count + 1
        Next

        If Not String.IsNullOrEmpty(other) Then
            occupationDataTbl.Rows(mOtherRow).Item(occupationDataTbl.OCCUPATIONColumn.ColumnName) = other
            occupationDataTbl.Rows(otherRow).Delete()
        End If

        Return occupationDataTbl

    End Function


    ''' <summary>
    ''' 家族続柄マスタ取得
    ''' </summary>
    ''' <param name="inCustFamilyDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>家族続柄マスタを取得する処理</remarks>
    Public Shared Function GetCustFamilyMstData(ByVal inCustFamilyDataTbl As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201CustomerFamilyMstDataTable
        Dim custFamilyMstDataRow As SC3080201DataSet.SC3080201ParameterRow
        custFamilyMstDataRow = CType(inCustFamilyDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        '家族続柄マスタ取得
        Return (SC3080201TableAdapter.GetCustomerFamilyMst(custFamilyMstDataRow.DLRCD, _
                                                                    custFamilyMstDataRow.STRCD))
    End Function


    ''' <summary>
    ''' 顧客家族構成取得
    ''' </summary>
    ''' <param name="inCustFamilyDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>顧客家族構成を取得する処理</remarks>
    Public Shared Function GetCustFamilyData(ByVal inCustFamilyDataTbl As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201CustomerFamilyDataTable
        Dim custFamilyDataRow As SC3080201DataSet.SC3080201ParameterRow
        custFamilyDataRow = CType(inCustFamilyDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        '顧客家族構成取得
        Return (SC3080201TableAdapter.GetCustomerFamily(custFamilyDataRow.DLRCD, _
                                                                    custFamilyDataRow.STRCD, _
                                                                    custFamilyDataRow.CSTKIND, _
                                                                    custFamilyDataRow.CUSTOMERCLASS, _
                                                                    custFamilyDataRow.CRCUSTID))
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START    
    ''' <summary>
    ''' 顧客家族構成編集
    ''' </summary>
    ''' <param name="inCustFamilyDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>顧客家族構成をバインド用に編集する処理</remarks>
    Public Shared Function EditCustFamilyData(ByVal inCustFamilyDataTbl As SC3080201DataSet.SC3080201CustomerFamilyDataTable, _
                                       ByVal inCustFamilyMstDataTbl As SC3080201DataSet.SC3080201CustomerFamilyMstDataTable) As SC3080201DataSet.SC3080201CustomerFamilyDataTable
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        Dim outCustFamilyDataTbl As SC3080201DataSet.SC3080201CustomerFamilyDataTable
        outCustFamilyDataTbl = inCustFamilyDataTbl
        '件数取得
        Dim count As Integer = outCustFamilyDataTbl.Rows.Count

        '不明のデータ取得
        Dim unknownRow() As DataRow
        unknownRow = inCustFamilyMstDataTbl.Select("OTHERUNKNOWN = '2'")

        '10行になるまで「不明」行を追加
        For i = 0 To 9
            If i >= count Then
                '10件になるまで不明追加
                Dim custFamilyDataRow As SC3080201DataSet.SC3080201CustomerFamilyRow = outCustFamilyDataTbl.NewSC3080201CustomerFamilyRow
                custFamilyDataRow.FAMILYNO = 0
                custFamilyDataRow.FAMILYRELATIONSHIPNO = CInt(unknownRow(0).Item("FAMILYRELATIONSHIPNO"))
                custFamilyDataRow.OTHERFAMILYRELATIONSHIP = ""
                custFamilyDataRow.FAMILYRELATIONSHIP = CStr(unknownRow(0).Item("FAMILYRELATIONSHIP"))
                custFamilyDataRow.SORTNO = 1

                outCustFamilyDataTbl.Rows.Add(custFamilyDataRow)

            ElseIf Not outCustFamilyDataTbl.Rows(i).Item("OTHERFAMILYRELATIONSHIP") Is DBNull.Value Then
                'その他が入力されている場合
                outCustFamilyDataTbl.Rows(i).Item("FAMILYRELATIONSHIP") = outCustFamilyDataTbl.Rows(i).Item("OTHERFAMILYRELATIONSHIP")

            End If
        Next

        '1行目が本人の場合
        If CInt(outCustFamilyDataTbl.Rows(0).Item("SORTNO")) = 0 Then
            outCustFamilyDataTbl.Rows(0).Item("FAMILYRELATIONSHIP") = WebWordUtility.GetWord(10151)
        End If

        Return outCustFamilyDataTbl
    End Function

    ''' <summary>
    ''' 顧客趣味取得
    ''' </summary>
    ''' <param name="inHobbyDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>顧客趣味を取得する処理</remarks>
    Public Shared Function GetHobbyData(ByVal inHobbyDataTbl As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201CustomerHobbyDataTable
        Dim hobbyDataRow As SC3080201DataSet.SC3080201ParameterRow
        hobbyDataRow = CType(inHobbyDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        '顧客趣味取得
        Return (SC3080201TableAdapter.GetCustomerHobby(hobbyDataRow.DLRCD, _
                                                                hobbyDataRow.STRCD, _
                                                                hobbyDataRow.CSTKIND, _
                                                                hobbyDataRow.CUSTOMERCLASS, _
                                                                hobbyDataRow.CRCUSTID))
    End Function


    ''' <summary>
    ''' 取得した顧客趣味の編集
    ''' </summary>
    ''' <param name="hobbyDataTbl"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function EditHobbyData(ByVal hobbyDataTbl As SC3080201DataSet.SC3080201CustomerHobbyDataTable) As SC3080201DataSet.SC3080201CustomerHobbyDataTable
        Dim count As Integer
        Dim mOtherRow As Integer
        Dim otherRow As Integer
        Dim other As String = String.Empty

        For Each drControloccupation In hobbyDataTbl
            If String.Equals(drControloccupation.OTHER, OCCUPATIONOTHER) Then
                mOtherRow = count
            End If
            If String.Equals(drControloccupation.SORTNO_1ST, "2") And _
               drControloccupation.SORTNO_2ND = 0 Then
                otherRow = count
                other = drControloccupation.HOBBY
            End If
            count = count + 1
        Next

        If Not String.IsNullOrEmpty(other) Then
            hobbyDataTbl.Rows(mOtherRow).Item(hobbyDataTbl.HOBBYColumn.ColumnName) = other
            hobbyDataTbl.Rows(otherRow).Delete()
        End If


        Return hobbyDataTbl
    End Function

    ''' <summary>
    ''' 希望コンタクト方法取得
    ''' </summary>
    ''' <param name="inContactFlgDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>希望コンタクト方法を取得する処理</remarks>
    Public Shared Function GetContactFlg(ByVal inContactFlgDataTbl As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201ContactFlgDataTable
        Dim ContactFlgDataRow As SC3080201DataSet.SC3080201ParameterRow
        ContactFlgDataRow = CType(inContactFlgDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        '希望コンタクト方法取得
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Return (SC3080201TableAdapter.GetContactFlg(ContactFlgDataRow.CRCUSTID, ContactFlgDataRow.DLRCD))
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
    End Function

    '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
    ''' <summary>
    ''' コンタクトシステム設定情報取得
    ''' </summary>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>初期表示時の表示用フラグを取得する。</remarks>
    Public Shared Function GetContactSetFlg(ByVal inContactFlgDataTbl As SC3080201DataSet.SC3080201ParameterDataTable) As String

        Dim settionFlg As Integer = 0
        Dim funcSetting As New FunctionSetting
        Dim ContactFlgDataRow As SC3080201DataSet.SC3080201ParameterRow
        Dim ret As String

        ContactFlgDataRow = CType(inContactFlgDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        '全設定
        settionFlg = funcSetting.GetiCROPFunctionSetting(ContactFlgDataRow.DLRCD, UsedFlgContactAll)
        ret = CStr(CType(settionFlg, Short))

        'SMS使用可否
        settionFlg = funcSetting.GetiCROPFunctionSetting(ContactFlgDataRow.DLRCD, UsedFlgContactSms)
        ret = ret & CStr(CType(settionFlg, Short))

        'e-mail使用可否
        settionFlg = funcSetting.GetiCROPFunctionSetting(ContactFlgDataRow.DLRCD, UsedFlgContactEmail)
        ret = ret & CStr(CType(settionFlg, Short))

        'D-mail使用可否
        settionFlg = funcSetting.GetiCROPFunctionSetting(ContactFlgDataRow.DLRCD, UsedFlgContactDmail)
        ret = ret & CStr(CType(settionFlg, Short))

        Return ret

    End Function
    '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END

    ''' <summary>
    ''' 希望連絡時間帯取得
    ''' </summary>
    ''' <param name="inTimeZoneDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>希望連絡時間帯を取得する処理</remarks>
    Public Shared Function GetTimeZoneData(ByVal inTimeZoneDataTbl As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201ContactTimeZoneDataTable
        Dim timeZoneDataRow As SC3080201DataSet.SC3080201ParameterRow
        timeZoneDataRow = CType(inTimeZoneDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        '希望連絡時間帯取得
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Return (SC3080201TableAdapter.GetContactTimeZone(timeZoneDataRow.DLRCD, _
                                                                    timeZoneDataRow.STRCD, _
                                                                    timeZoneDataRow.CRCUSTID, _
                                                                    timeZoneDataRow.TIMEZONECLASS))
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    End Function

    ''' <summary>
    ''' 希望連絡曜日取得
    ''' </summary>
    ''' <param name="inWeekOfDayDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>希望連絡曜日を取得する処理</remarks>
    Public Shared Function GetWeekOfDayData(ByVal inWeekOfDayDataTbl As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201ContactWeekOfDayDataTable
        Dim weekOfDayDataRow As SC3080201DataSet.SC3080201ParameterRow
        weekOfDayDataRow = CType(inWeekOfDayDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        '希望連絡曜日取得
        Return (SC3080201TableAdapter.GetContactWeekOfDay(weekOfDayDataRow.CSTKIND, _
                                                                    weekOfDayDataRow.CUSTOMERCLASS, _
                                                                    weekOfDayDataRow.CRCUSTID, _
                                                                    weekOfDayDataRow.TIMEZONECLASS))

    End Function

    ''' <summary>
    ''' 最新顧客メモ取得
    ''' </summary>
    ''' <param name="inLastCustMemoDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>最新顧客メモを取得する処理</remarks>
    Public Shared Function GetLastCustMemoData(ByVal inLastCustMemoDataTbl As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201LastCustomerMemoDataTable
        Dim lastCustMemoDataRow As SC3080201DataSet.SC3080201ParameterRow
        lastCustMemoDataRow = CType(inLastCustMemoDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        '最新顧客メモ取得
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        '2012/02/15 TCS 山口 【SALES_2】 START
        Return (SC3080201TableAdapter.GetLastCustomerMemo(lastCustMemoDataRow.CRCUSTID))
        '2012/02/15 TCS 山口 【SALES_2】 END
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
    End Function

    '2012/02/15 TCS 山口 【SALES_2】 START
    ''' <summary>
    ''' 重要連絡取得
    ''' </summary>
    ''' <param name="inImportantContact"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetImportantContact(ByVal inImportantContact As SC3080201DataSet.SC3080201ParameterDataTable) As SC3080201DataSet.SC3080201ImportantContactDataTable
        Logger.Debug("GetImportantContact Start")

        Dim importantContactDataRow As SC3080201DataSet.SC3080201ParameterRow
        importantContactDataRow = CType(inImportantContact.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        Dim sysEnv As New SystemEnvSetting
        Dim sysEnvRow As SystemEnvSettingDataSet.SYSTEMENVSETTINGRow

        sysEnvRow = sysEnv.GetSystemEnvSetting(COMPLAINT_DISPLAYDATE)

        '重要連絡取得
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Return (SC3080201TableAdapter.GetImportantContact(importantContactDataRow.CRCUSTID, _
                                                          importantContactDataRow.CSTKIND, _
                                                          importantContactDataRow.NEWCUSTID, _
                                                          sysEnvRow.PARAMVALUE, _
                                                          importantContactDataRow.DLRCD))
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        Logger.Debug("GetImportantContact End")
    End Function

    ''' <summary>
    ''' コンタクト履歴取得
    ''' </summary>
    ''' <param name="inLastCustMemoDataTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks>コンタクト履歴を取得する処理</remarks>
    Public Shared Function GetContactHistoryData(ByVal inLastCustMemoDataTbl As SC3080201DataSet.SC3080201ParameterDataTable, _
                                                 ByVal tabIndex As String, _
                                                 ByVal vin As String) As SC3080201DataSet.SC3080201ContactHistoryDataTable
        Logger.Debug("GetContactHistoryData Start")

        Dim contactHistoryDataRow As SC3080201DataSet.SC3080201ParameterRow
        contactHistoryDataRow = CType(inLastCustMemoDataTbl.Rows(0), SC3080201DataSet.SC3080201ParameterRow)

        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
        'コンタクト履歴取得
        Return (SC3080201TableAdapter.GetContactHistory(contactHistoryDataRow.CUSTOMERCLASS, _
                                                        contactHistoryDataRow.CRCUSTID, _
                                                        contactHistoryDataRow.DLRCD, _
                                                        contactHistoryDataRow.CSTKIND, _
                                                        contactHistoryDataRow.NEWCUSTID, _
                                                        tabIndex, _
                                                        vin))
        '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END
        Logger.Debug("GetContactHistoryData End")
    End Function
    '2012/02/15 TCS 山口 【SALES_2】 END

    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 START
    ''' <summary>
    ''' 対象の入庫履歴の詳細情報を取得
    ''' </summary>
    ''' <param name="originalid"></param>
    ''' <param name="vin"></param>
    ''' <param name="dlrcd"></param>
    ''' <returns>取得結果</returns>
    ''' <remarks>対象の入庫履歴の詳細情報を取得</remarks>
    Public Shared Function GetServiceInInfo(ByVal originalid As String, _
                                            ByVal vin As String, _
                                            ByVal dlrcd As String) As SC3080201DataSet.SC3080201ServiceInInfoDataTable
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
        Logger.Debug("GetServiceInInfo Start")
        'コンタクト履歴取得
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Return (SC3080201TableAdapter.GetServiceInInfo(originalid, vin, dlrcd))
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        Logger.Debug("GetServiceInInfo End")
    End Function

    ''' <summary>
    ''' 基幹システム名を取得する
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetBasesystemNM() As SC3080201DataSet.SC3080201BasesystemNMDataTable
        Logger.Debug("GetBasesystemNM Start")
        'コンタクト履歴取得
        Return (SC3080201TableAdapter.GetBasesystemNM())
        Logger.Debug("GetBasesystemNM End")
    End Function
    '2013/01/22 TCS 河原 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 END

    ''' <summary>
    ''' 契約状況取得
    ''' </summary>
    ''' <param name="estimateInfoTbl">データセット (インプット)</param>
    ''' <returns>データセット (アウトプット)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetContractFlg(ByVal estimateInfoTbl As SC3080201DataSet.SC3080201ESTIMATEINFODataTable) As SC3080201DataSet.SC3080201ContractDataTable
        Dim dr As SC3080201DataSet.SC3080201ESTIMATEINFORow = CType(estimateInfoTbl.Rows(0), SC3080201DataSet.SC3080201ESTIMATEINFORow)

        '契約状況取得
        Return SC3080201TableAdapter.GetContractFlg(dr.ESTIMATEID)
    End Function
#End Region

#Region " 各種登録処理 "

    ''' <summary>
    ''' 顧客職業登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客職業を登録する処理</remarks>
    <EnableCommit()>
    Public Function InsertCstOccupation(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertCstOccupationDataTable) As Boolean Implements ISC3080201BusinessLogic.InsertCstOccupation

        Dim customerDataRow As SC3080201DataSet.SC3080201InsertCstOccupationRow
        customerDataRow = CType(inCstOccupationDataTbl.Rows(0), SC3080201DataSet.SC3080201InsertCstOccupationRow)

        '更新アカウント
        Dim account As String
        account = StaffContext.Current.Account

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        '親顧客データロック
        Try
            SC3080201TableAdapter.GetCustomerLock(customerDataRow.CRCUSTID)
        Catch ex As OracleExceptionEx
            Return False
        End Try

        Dim ret As Integer
        '値がない場合、登録を行なわない
        If Not customerDataRow.IsOCCUPATIONNONull Then
            '顧客職業登録
            ret = SC3080201TableAdapter.UpdateOrgCustomerOccupation(customerDataRow.OCCUPATIONNO,
                                                                    customerDataRow.OTHEROCCUPATION,
                                                                    account,
                                                                    customerDataRow.CRCUSTID,
                                                                    customerDataRow.ROWLOCKVERSION)
            If ret = 0 Then
                Me.Rollback = True
                Return False
            End If
        Else
            '顧客職業削除
            ret = SC3080201TableAdapter.UpdateOrgCustomerOccupation(CStr(0),
                                                                    " ",
                                                                    account,
                                                                    customerDataRow.CRCUSTID,
                                                                    customerDataRow.ROWLOCKVERSION)
            If ret = 0 Then
                Me.Rollback = True
                Return False
            End If
        End If
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        '正常終了
        Return True
    End Function


    ''' <summary>
    ''' 顧客家族構成登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客家族構成を登録する処理</remarks>
    <EnableCommit()>
    Public Function InsertCstFamily(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertCstFamilyDataTable) As Boolean Implements ISC3080201BusinessLogic.InsertCstFamily

        Dim customerDataRow As SC3080201DataSet.SC3080201InsertCstFamilyRow
        customerDataRow = CType(inCstOccupationDataTbl.Rows(0), SC3080201DataSet.SC3080201InsertCstFamilyRow)

        '更新アカウント
        Dim account As String
        account = StaffContext.Current.Account
        '機能ID
        Dim id As String
        id = MODULEID
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
        '店舗コード
        Dim dlrcd As String
        dlrcd = StaffContext.Current.DlrCD
        '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
        '顧客家族構成削除
        SC3080201TableAdapter.DeleteCustomerFamily(customerDataRow.CSTKIND, _
                                                            customerDataRow.CUSTOMERCLASS, _
                                                            customerDataRow.CRCUSTID)

        '顧客家族構成登録
        Dim count As Integer = 0
        Dim vcldelidate As Nullable(Of DateTime)
        For Each drOccupation In inCstOccupationDataTbl
            '1件目(本人)は対象外
            If count > 0 Then
                If (Not drOccupation.IsBIRTHDAYNull) Then
                    vcldelidate = drOccupation.BIRTHDAY
                Else
                    vcldelidate = Nothing
                End If
                SC3080201TableAdapter.InsertCustomerFamily(drOccupation.CSTKIND, _
                                                                    drOccupation.CUSTOMERCLASS, _
                                                                    drOccupation.CRCUSTID, _
                                                                    drOccupation.FAMILYNO, _
                                                                    drOccupation.FAMILYRELATIONSHIPNO, _
                                                                    drOccupation.OTHERFAMILYRELATIONSHIP, _
                                                                    vcldelidate, _
                                                                    account, _
                                                                    id)
            End If
            count = count + 1
        Next

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        '自社客、未取引客の判定
        If String.Equals(customerDataRow.CSTKIND, ORGCUSTFLG) Then
            '自社客の場合
            '自社客付加情報存在確認

            '親顧客データロック
            Try
                SC3080201TableAdapter.GetCustomerLock(customerDataRow.CRCUSTID)
            Catch ex As OracleExceptionEx
                Return False
            End Try

            Dim ret As Integer
            '自社客付加情報更新(家族構成)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            ret = SC3080201TableAdapter.UpdateOrgCustomerFamily(customerDataRow.NUMBEROFFAMILY, _
                                                                                customerDataRow.CRCUSTID, _
                                                                                customerDataRow.ROWLOCKVERSION, _
                                                                                account, _
                                                                                dlrcd)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            If ret = 0 Then
                Me.Rollback = True
                Return False
            End If

        ElseIf String.Equals(customerDataRow.CSTKIND, NEWCUSTFLG) Then
            '未取引客の場合

            '親顧客データロック
            Try
                SC3080201TableAdapter.GetCustomerLock(customerDataRow.CRCUSTID)
            Catch ex As OracleExceptionEx
                Return False
            End Try

            Dim ret As Integer
            '未取引客更新(家族構成)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 START
            ret = SC3080201TableAdapter.UpdateOrgCustomerFamily(customerDataRow.NUMBEROFFAMILY, _
                                                                                customerDataRow.CRCUSTID, _
                                                                                customerDataRow.ROWLOCKVERSION, _
                                                                                account, _
                                                                                dlrcd)
            '2013/06/30 TCS 小幡 2013/10対応版　既存流用 END
            If ret = 0 Then
                Me.Rollback = True
                Return False
            End If

        End If
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        '正常終了
        Return True
    End Function


    ''' <summary>
    ''' 顧客趣味登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>顧客趣味を登録する処理</remarks>
    <EnableCommit()>
    Public Function InsertCstHobby(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertCstHobbyDataTable) As Boolean Implements ISC3080201BusinessLogic.InsertCstHobby

        Dim customerDataRow As SC3080201DataSet.SC3080201InsertCstHobbyRow
        customerDataRow = CType(inCstOccupationDataTbl.Rows(0), SC3080201DataSet.SC3080201InsertCstHobbyRow)

        '更新アカウント
        Dim account As String
        account = StaffContext.Current.Account
        '機能ID
        Dim id As String
        id = MODULEID

        '顧客趣味削除
        SC3080201TableAdapter.DeleteCustomerHobby(customerDataRow.CSTKIND, _
                                                            customerDataRow.CUSTOMERCLASS, _
                                                            customerDataRow.CRCUSTID)

        '顧客趣味登録
        For Each drHobby In inCstOccupationDataTbl
            If drHobby.IsHOBBYNONull = False And _
                drHobby.IsOTHERHOBBYNull = False Then
                SC3080201TableAdapter.InsertCustomerHobby(drHobby.CSTKIND,
                                                                    drHobby.CUSTOMERCLASS,
                                                                    drHobby.CRCUSTID,
                                                                    drHobby.HOBBYNO,
                                                                    drHobby.OTHERHOBBY,
                                                                    account,
                                                                    id)
            End If
        Next
        '正常終了
        Return True
    End Function


    ''' <summary>
    ''' 希望コンタクト方法登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>希望コンタクト方法を登録する処理</remarks>
    <EnableCommit()>
    Public Function InsertCstContactInfo(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertCstContactInfoDataTable) As Boolean Implements ISC3080201BusinessLogic.InsertCstContactInfo

        Dim customerDataRow As SC3080201DataSet.SC3080201InsertCstContactInfoRow
        customerDataRow = CType(inCstOccupationDataTbl.Rows(0), SC3080201DataSet.SC3080201InsertCstContactInfoRow)

        '更新アカウント
        Dim account As String
        account = StaffContext.Current.Account

        '自社客、未取引客の判定
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
        Try
            SC3080201TableAdapter.GetCustomerLock(customerDataRow.CRCUSTID)
        Catch ex As OracleExceptionEx
            Return False
        End Try

        Dim ret As Integer
        ret = SC3080201TableAdapter.UpdateOrgCustomerAppnedContact(customerDataRow.CRCUSTID, _
                                                                            customerDataRow.CONTACTDMFLG, _
                                                                            customerDataRow.CONTACTHOMEFLG, _
                                                                            customerDataRow.CONTACTMOBILEFLG, _
                                                                            customerDataRow.CONTACTEMAILFLG, _
                                                                            customerDataRow.CONTACTSMSFLG, _
                                                                            customerDataRow.ROWLOCKVERSION, _
                                                                            account)
        If ret = 0 Then
            Me.Rollback = True
            Return False
        End If

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        '2013/06/30 TCS 趙 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        '正常終了
        Return True
    End Function

    ''' <summary>
    ''' 希望連絡時間登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>希望連絡時間を登録する処理</remarks>
    <EnableCommit()>
    Public Function InsertCstContactTime(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertCstContactInfoDataTable) As Boolean Implements ISC3080201BusinessLogic.InsertCstContactTime

        Dim customerDataRow As SC3080201DataSet.SC3080201InsertCstContactInfoRow
        customerDataRow = CType(inCstOccupationDataTbl.Rows(0), SC3080201DataSet.SC3080201InsertCstContactInfoRow)
        '更新アカウント
        Dim account As String
        account = StaffContext.Current.Account
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START 
        SC3080201TableAdapter.DeleteContactTimeZone(customerDataRow.CRCUSTID)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        '希望連絡時間帯登録
        For Each drCstOccupation In inCstOccupationDataTbl
            If drCstOccupation.IsTIMEZONECLASSNull = False And _
                drCstOccupation.IsCONTACTTIMEZONENONull = False Then
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
                SC3080201TableAdapter.InsertContactTimeZone(drCstOccupation.CRCUSTID, _
                                                                            CStr(drCstOccupation.TIMEZONECLASS), _
                                                                            drCstOccupation.CONTACTTIMEZONENO, _
                                                                            account, _
                                                                            account)
                '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
            End If
        Next

        '正常終了
        Return True
    End Function

    ''' <summary>
    ''' 希望連絡曜日登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>希望連絡曜日を登録する処理</remarks>
    <EnableCommit()>
    Public Function InsertCstContactWeekOfDay(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertCstContactInfoDataTable) As Boolean Implements ISC3080201BusinessLogic.InsertCstContactWeekOfDay

        Dim customerDataRow As SC3080201DataSet.SC3080201InsertCstContactInfoRow
        customerDataRow = CType(inCstOccupationDataTbl.Rows(0), SC3080201DataSet.SC3080201InsertCstContactInfoRow)

        '更新アカウント
        Dim account As String
        account = StaffContext.Current.Account
        '機能ID
        Dim id As String
        id = MODULEID

        '希望連絡曜日削除
        SC3080201TableAdapter.DeleteContactWeekOfDay(customerDataRow.CSTKIND, _
                                                                customerDataRow.CUSTOMERCLASS, _
                                                                customerDataRow.CRCUSTID)


        '希望連絡曜日登録
        For Each drCstOccupation In inCstOccupationDataTbl
            SC3080201TableAdapter.InsertContactWeekOfDay(drCstOccupation.CSTKIND, _
                                                                    drCstOccupation.CUSTOMERCLASS, _
                                                                    drCstOccupation.CRCUSTID, _
                                                                    drCstOccupation.TIMEZONECLASS, _
                                                                    drCstOccupation.MONDAY, _
                                                                    drCstOccupation.TUESWDAY, _
                                                                    drCstOccupation.WEDNESDAY, _
                                                                    drCstOccupation.THURSDAY, _
                                                                    drCstOccupation.FRIDAY, _
                                                                    drCstOccupation.SATURDAY, _
                                                                    drCstOccupation.SUNDAY, _
                                                                    account, _
                                                                    id)
        Next

        '正常終了
        Return True
    End Function


    ''' <summary>
    ''' 顔写真登録処理
    ''' </summary>
    ''' <param name="inCstOccupationDataTbl">データセット (インプット)</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>希望連絡情報を登録する処理</remarks>
    <EnableCommit()>
    Public Function InsertImageFile(ByVal inCstOccupationDataTbl As SC3080201DataSet.SC3080201InsertImageFileDataTable) As Boolean Implements ISC3080201BusinessLogic.InsertImageFile

        Dim customerDataRow As SC3080201DataSet.SC3080201InsertImageFileRow
        customerDataRow = CType(inCstOccupationDataTbl.Rows(0), SC3080201DataSet.SC3080201InsertImageFileRow)

        '処理件数確認用
        Dim cnt As Integer = 0
        '更新アカウント
        Dim account As String
        account = StaffContext.Current.Account

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        '親顧客データロック
        Try
            SC3080201TableAdapter.GetCustomerLock(customerDataRow.CRCUSTID)
        Catch ex As OracleExceptionEx
            Return False
        End Try
        '自社客付加情報更新(顔写真)
        cnt = SC3080201TableAdapter.UpdateOrgCustomerAppnedFace(customerDataRow.dlrcd, _
                                                                            customerDataRow.CRCUSTID, _
                                                                            customerDataRow.IMAGEFILE_L, _
                                                                            customerDataRow.IMAGEFILE_M, _
                                                                            customerDataRow.IMAGEFILE_S, _
                                                                            account, _
                                                                            customerDataRow.ROWLOCKVERSION)

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        '更新件数が0件の場合、ロールバックし処理を終了する
        If cnt = 0 Then
            Me.Rollback = True
            Return False
        End If

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        '正常終了
        Return True
    End Function

#End Region

    '2012/01/24 TCS 河原 【SALES_1B】 START
#Region " 共通処理 "


    ''' <summary>
    ''' 来店実績取得
    ''' </summary>
    ''' <param name="dtParam">データテーブル</param>
    ''' <returns>データテーブル</returns>
    ''' <remarks>来店実績取得</remarks>
    Public Function GetVclregNo(ByVal dtParam As SC3080201DataSet.SC3080201VisitSeqDataTable) As SC3080201DataSet.SC3080201VisitResultDataTable

        Logger.Debug("GetVclregNo Start")

        '来店実績を取得
        Return (SC3080201TableAdapter.GetVisitResult(dtParam(0).VISITSEQ))

        Logger.Debug("GetVclregNo End")

    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-up Box商談取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeq</param>
    ''' <returns>データテーブル</returns>
    ''' <remarks>Follow-up Box商談取得</remarks>
    Public Function GetSelectDeleteFllwUpBoxSalas(ByVal fllwupboxseqno As Decimal) As Integer

        Logger.Debug("GetSelectDeleteFllwUpBoxSalas Start")

        '件数を取得
        Return (SC3080201TableAdapter.GetFllwupboxSales(fllwupboxseqno))

        Logger.Debug("GetSelectDeleteFllwUpBoxSalas End")

    End Function
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
    ''' <summary>
    ''' Follow-up Box商談削除処理
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeq</param>
    ''' <returns>データテーブル</returns>
    ''' <remarks>Follow-up Box商談削除処理</remarks>
    Public Function DeleteFllwUpBoxSalas(ByVal fllwupboxseqno As Decimal) As Boolean

        Logger.Debug("DeleteFllwUpBoxSalas Start")

        ' 2012/02/15 TCS 相田 【SALES_2】 START 引数追加
        'SC3080201TableAdapter.DeleteFllwupboxSales(fllwupboxdlrcd, fllwupboxstrcd, fllwupboxseqno, fllwupboxseqno)
        SC3080201TableAdapter.DeleteFllwupboxSales(fllwupboxseqno)
        ' 2012/02/15 TCS 相田 【SALES_2】 END

        Logger.Debug("DeleteFllwUpBoxSalas End")

        '正常終了
        Return True
    End Function
    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END

    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
    ''' <summary>
    ''' Follow-up Box商談登録処理
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeq</param>
    ''' <param name="custsegment">顧客区分</param>
    ''' <param name="customerclass">顧客種別</param>
    ''' <param name="crcustid">顧客ID</param>
    ''' <param name="walkinnum">来店人数</param>
    ''' <param name="branchplan">予定店舗コード</param>
    ''' <param name="accountplan">予定アカウント</param>
    ''' <param name="salesFlg">商談フラグ</param>
    ''' <param name="newFllwupFlg">新規活動フラグ</param>
    ''' <returns>データテーブル</returns>
    ''' <remarks>Follow-up Box商談登録処理</remarks>
    Public Function InsertFllwUpBoxSalas(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Decimal,
                                                ByVal custsegment As String, ByVal customerclass As String, ByVal crcustid As String,
                                                ByVal walkinnum As Integer,
                                                ByVal branchplan As String,
                                                ByVal accountplan As String,
                                                ByVal salesFlg As Boolean,
                                                ByVal newFllwupFlg As String) As Boolean
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Logger.Debug("InsertFllwUpBoxSalas Start")

        Dim num As Nullable(Of Integer)

        If walkinnum = 0 Then
            num = Nothing
        Else
            num = walkinnum
        End If

        Dim account = StaffContext.Current.Account

        ' 2012/02/15 TCS 相田 【SALES_2】 START 引数追加
        ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
        'SC3080201TableAdapter.InsertFllwupboxSales(dlrcd, strcd, fllwupboxseqno, custsegment, customerclass,
        '                                           crcustid, account, num, MODULEID,
        '                                           newFllwupFlg, REGISTFLG_NOTREGIST, branchplan, accountplan, salesFlg, salesseqno)
        SC3080201TableAdapter.InsertFllwupboxSales(dlrcd, strcd, fllwupboxseqno, custsegment, customerclass,
                                                   crcustid, account, num, MODULEID,
                                                   newFllwupFlg, REGISTFLG_NOTREGIST, branchplan, accountplan, salesFlg)
        ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END

        'SC3080201TableAdapter.InsertFllwupboxSales(dlrcd, strcd, fllwupboxseqno, custsegment, customerclass,
        '                                           crcustid, Account, num, MODULEID)

        ' 2012/02/15 TCS 相田 【SALES_2】 END
        Logger.Debug("InsertFllwUpBoxSalas End")

        '正常終了
        Return True
    End Function
    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END

    ''' <summary>
    ''' 商談・一時対応・営業活動開始処理
    ''' </summary>
    ''' <param name="dtParam">データテーブル</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>商談・一時対応・営業活動開始処理</remarks>
    <EnableCommit()>
    Public Function StartVisitSales(ByVal dtParam As SC3080201DataSet.SC3080201SalesStartDataTable,
                                   ByRef msgId As Integer) As Boolean Implements ISC3080201BusinessLogic.StartVisitSales

        'Logger.Debug("StartVisitSales Start")

        ''商談・一時対応開始時間を設定
        'Dim salesstart As Date
        'salesstart = DateTimeFunc.Now(dtParam(0).DLRCD)

        ''商談開始・一時対応開始・納車開始・納車作業開始(一時対応)の場合来店実績のテーブルを更新
        'msgId = 0

        ''2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
        'If dtParam(0).STATUS = C_SALES_START Or dtParam(0).STATUS = C_CORRESPOND_START Or
        '    dtParam(0).STATUS = C_DELIVERY_START Or dtParam(0).STATUS = C_DELIVERYCORRESPOND_START Then
        '    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End


        '    ' 2013/03/13 TCS 渡邊 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        '    '来店実績更新
        '    'msgId = UpdateVisitSalesStart(dtParam(0).CUSTSEGMENT, dtParam(0).CRCUSTID, dtParam(0).SALESSTAFFCD, dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO, salesstart)
        '    'ステータス区分：商談開始の追加
        '    If dtParam(0).STATUS = C_SALES_START Or dtParam(0).STATUS = C_CORRESPOND_START Then
        '        msgId = UpdateVisitSalesStart(dtParam(0).CUSTSEGMENT, dtParam(0).CRCUSTID, dtParam(0).SALESSTAFFCD, dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO, salesstart, UpdateSalesVisitBusinessLogic.LogicStateNegotiationStart)
        '        Logger.Debug("dtParam(0).STATUS = 商談開始")
        '        'ステータス区分：納車作業開始の追加
        '    ElseIf dtParam(0).STATUS = C_DELIVERY_START Or dtParam(0).STATUS = C_DELIVERYCORRESPOND_START Then
        '        msgId = UpdateVisitSalesStart(dtParam(0).CUSTSEGMENT, dtParam(0).CRCUSTID, dtParam(0).SALESSTAFFCD, dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO, salesstart, UpdateSalesVisitBusinessLogic.LogicStateDeliverly)
        '        Logger.Debug("dtParam(0).STATUS = 納車作業開始")
        '    End If
        '    ' 2013/03/13 TCS 渡邊 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END


        '    'メッセージIDが0以外ならエラー
        '    If msgId <> 0 Then
        '        Rollback = True
        '        If msgId <> 5002 Then
        '            Throw New ArgumentException("来店実績更新処理失敗")
        '        End If
        '        Return False
        '    End If

        'End If


        ''商談開始・営業活動開始・納車開始・納車作業開始(一時対応)の場合来店実績のテーブルを更新
        '' 2012/02/15 TCS 相田 【SALES_2】 START
        ''2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
        'If dtParam(0).STATUS = C_SALES_START Or dtParam(0).STATUS = C_BUSINESS_START Or dtParam(0).STATUS = C_CORRESPOND_START Or
        '    dtParam(0).STATUS = C_DELIVERY_START Or dtParam(0).STATUS = C_DELIVERYCORRESPOND_START Then
        '    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
        '    'Follow-upBox商談の存在確認
        '    'Dim Cnt As Integer
        '    'Cnt = GetSelectDeleteFllwUpBoxSalas(dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO)

        '    'データが存在する場合削除
        '    'If Cnt > 0 Then
        '    '    DeleteFllwUpBoxSalas(dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO)
        '    'End If

        '    ''Follow-upBox商談の登録
        '    'InsertFllwUpBoxSalas(dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO, dtParam(0).CUSTSEGMENT,
        '    '                     dtParam(0).CUSTOMERCLASS, dtParam(0).CRCUSTID, dtParam(0).WALKINNUM, "100005@44B20", dtParam(0).SALESSTAFFCD, True)

        '    Dim salesFlg As Boolean = False
        '    '商談開始・納車開始・納車作業開始(一時対応)の場合は商談フラグをTrueにする
        '    'If dtParam(0).STATUS = C_SALES_START Then
        '    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
        '    If dtParam(0).STATUS = C_SALES_START Or dtParam(0).STATUS = C_CORRESPOND_START Or
        '        dtParam(0).STATUS = C_DELIVERY_START Or dtParam(0).STATUS = C_DELIVERYCORRESPOND_START Then
        '        '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
        '        salesFlg = True
        '    End If

        '    '存在確認
        '    Dim countResult As Boolean
        '    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
        '    countResult = CountFllwupbox(dtParam(0).FLLWUPBOX_SEQNO)
        '    '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        '    If REGISTFLG_NOTREGIST.Equals(dtParam(0).REGISTFLG) Then
        '        '未登録の場合
        '        'Follow-upBox商談の更新
        '        ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
        '        'UpdateFllwupboxSales(dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO, salesFlg, True, dtParam(0).SALES_SEQNO)
        '        UpdateFllwupboxSales(dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO, salesFlg, True)
        '        ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END
        '    Else
        '        '登録済みの場合
        '        '顧客担当店舗コードの取得
        '        Dim branchPlan As String = String.Empty
        '        If countResult Then
        '            '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
        '            branchPlan = GetFllwUpBoxCustchrgInfo(dtParam(0).FLLWUPBOX_SEQNO)
        '            '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
        '        Else
        '            branchPlan = GetCustchrgInfo(dtParam(0).CUSTSEGMENT, dtParam(0).CRCUSTID)
        '        End If
        '        'Follow-upBox商談の登録
        '        Dim newFllwupFlg As String = String.Empty
        '        If dtParam(0).NEWFLG Then
        '            newFllwupFlg = NEWFLLWUPBOXFLG_NEW
        '        Else
        '            newFllwupFlg = NEWFLLWUPBOXFLG_NOTNEW
        '        End If
        '        ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
        '        'InsertFllwUpBoxSalas(dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO, dtParam(0).CUSTSEGMENT,
        '        '                     dtParam(0).CUSTOMERCLASS, dtParam(0).CRCUSTID, dtParam(0).WALKINNUM, branchPlan, dtParam(0).SALESSTAFFCD,
        '        '                     salesFlg, newFllwupFlg, dtParam(0).SALES_SEQNO)
        '        InsertFllwUpBoxSalas(dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO, dtParam(0).CUSTSEGMENT,
        '                             dtParam(0).CUSTOMERCLASS, dtParam(0).CRCUSTID, dtParam(0).WALKINNUM, branchPlan, dtParam(0).SALESSTAFFCD,
        '                             salesFlg, newFllwupFlg)
        '        ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END
        '    End If

        'End If
        '' 2012/02/15 TCS 相田 【SALES_2】 END　
        'Logger.Debug("StartVisitSales End")

        ''正常終了
        Return True
    End Function


    ''' <summary>
    ''' 商談・一時対応・営業活動終了・商談中断処理
    ''' </summary>
    ''' <param name="dtParam">データテーブル</param>
    ''' <param name="msgId">メッセージID</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>商談・一時対応・営業活動終了・商談中断処理</remarks>
    <EnableCommit()>
    Public Function EndVisitSales(ByVal dtParam As SC3080201DataSet.SC3080201SalesStartDataTable,
                                  ByRef msgId As Integer) As Boolean Implements ISC3080201BusinessLogic.EndVisitSales

        'Logger.Debug("EndVisitSales Start")

        'msgId = 0

        ''一時対応終了の場合
        '' 2012/02/15 TCS 相田 【SALES_2】 START
        ''If dtParam(0).STATUS = C_SALES_CANCEL Or dtParam(0).STATUS = C_CORRESPOND_END Then
        '' 2012/02/15 TCS 相田 【SALES_2】 END

        '' 2012/08/13 TCS 安田 商談中断メニューの追加 START
        ''If dtParam(0).STATUS = C_SALES_END Or dtParam(0).STATUS = C_CORRESPOND_END Then
        ''2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
        'If dtParam(0).STATUS = C_SALES_END Or dtParam(0).STATUS = C_CORRESPOND_END Or dtParam(0).STATUS = C_SALES_STOP Or
        '    dtParam(0).STATUS = C_DELIVERY_END Or dtParam(0).STATUS = C_DELIVERYCORRESPOND_END Then
        '    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
        '    ' 2012/08/13 TCS 安田 商談中断メニューの追加 END

        '    '商談・一時対応終了時間を設定
        '    Dim salessEnd As Date
        '    salessEnd = DateTimeFunc.Now(dtParam(0).DLRCD)

        '    '来店実績更新
        '    ' 2012/08/13 TCS 安田 商談中断メニューの追加 START
        '    'msgId = UpdateVisitSalesEnd(dtParam(0).CUSTSEGMENT, dtParam(0).CRCUSTID, salessEnd)
        '    If dtParam(0).STATUS = C_SALES_STOP Then
        '        msgId = UpdateVisitSalesEnd(dtParam(0).CUSTSEGMENT, dtParam(0).CRCUSTID, salessEnd, UpdateSalesVisitBusinessLogic.LogicStateNegotiationStop)
        '        Logger.Debug("dtParam(0).STATUS = 商談中断")
        '        ' 2013/03/13 TCS 渡邊 【A.STEP2】新車タブレット受付画面の管理指標変更対応 START
        '        ' 処理区分：納車作業終了の追加
        '    ElseIf dtParam(0).STATUS = C_DELIVERY_END Or dtParam(0).STATUS = C_DELIVERYCORRESPOND_END Then
        '        msgId = UpdateVisitSalesEnd(dtParam(0).CUSTSEGMENT, dtParam(0).CRCUSTID, salessEnd, UpdateSalesVisitBusinessLogic.LogicStateDeliverlyFinish)
        '        Logger.Debug("dtParam(0).STATUS = 納車作業終了")
        '        ' 2013/03/13s TCS 渡邊 【A.STEP2】新車タブレット受付画面の管理指標変更対応 END
        '    Else
        '        msgId = UpdateVisitSalesEnd(dtParam(0).CUSTSEGMENT, dtParam(0).CRCUSTID, salessEnd, UpdateSalesVisitBusinessLogic.LogicStateNegotiationFinish)
        '        Logger.Debug("dtParam(0).STATUS = 商談終了")
        '    End If
        '    ' 2012/08/13 TCS 安田 商談中断メニューの追加 END


        '    'メッセージIDが0以外ならエラー
        '    If msgId <> 0 Then
        '        Rollback = True
        '        If msgId <> 5002 Then
        '            Throw New ArgumentException("来店実績更新処理失敗")
        '        End If
        '        Return False
        '    End If
        'End If

        ''営業活動キャンセルの場合
        '' 2012/02/15 TCS 相田 【SALES_2】 START
        ''If dtParam(0).STATUS = C_SALES_CANCEL Or dtParam(0).STATUS = C_BUSINESS_CANCEL Then
        'If dtParam(0).STATUS = C_BUSINESS_CANCEL Then

        '    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        '    'Follow-upBox商談の商談開始時間を取得
        '    Dim salesStartTime As String = GetSalesTime(dtParam(0).FLLWUPBOX_SEQNO)
        '    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

        '    If String.IsNullOrEmpty(salesStartTime) Then
        '        '商談開始時間が入っていない場合
        '        'Follow-upBox商談の削除
        '        ' 2012/02/15 TCS 相田 【SALES_2】 START 引数追加
        '        ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
        '        'DeleteFllwUpBoxSalas(dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO, dtParam(0).SALES_SEQNO)
        '        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        '        DeleteFllwUpBoxSalas(dtParam(0).FLLWUPBOX_SEQNO)
        '        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        '        ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END
        '        ' 2012/02/15 TCS 相田 【SALES_2】 END
        '    Else
        '        '商談開始時間が入っている場合
        '        '営業活動開始時間をNULLに更新
        '        ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
        '        'UpdateFllwupboxEigyoStartTime(dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO, dtParam(0).SALES_SEQNO)
        '        UpdateFllwupboxEigyoStartTime(dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO)
        '        ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END
        '    End If

        '    Dim rlstFlg As Boolean
        '    '通知キャンセル
        '    rlstFlg = UpdateNoticeRequest(dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO, dtParam(0).CUSTNAME)

        '    If Not rlstFlg Then
        '        Rollback = True
        '        Return False
        '    End If
        'End If

        ''終了の場合
        ''If dtParam(0).STATUS = C_SALES_END Then
        '' 2012/08/13 TCS 安田 商談中断メニューの追加 START
        ''If dtParam(0).STATUS = C_SALES_END Or dtParam(0).STATUS = C_CORRESPOND_END Then
        ''2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify Start
        'If dtParam(0).STATUS = C_SALES_END Or dtParam(0).STATUS = C_CORRESPOND_END Or dtParam(0).STATUS = C_SALES_STOP Or
        '    dtParam(0).STATUS = C_DELIVERY_END Or dtParam(0).STATUS = C_DELIVERYCORRESPOND_END Then
        '    '2013/01/24 TCS 藤井 【A.STEP2】次世代e-CRB 新車タブレットSC活動支援機能開発 Modify End
        '    ' 2012/08/13 TCS 安田 商談中断メニューの追加 END
        '    'Follow-upBox商談の更新
        '    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
        '    'UpdateFllwupboxSales(dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO, True, False, dtParam(0).SALES_SEQNO)
        '    UpdateFllwupboxSales(dtParam(0).DLRCD, dtParam(0).STRCD, dtParam(0).FLLWUPBOX_SEQNO, True, False)
        '    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END
        'End If


        '' 2012/02/15 TCS 相田 【SALES_2】 END

        'Logger.Debug("EndVisitSales End")

        '正常終了
        Return True
    End Function


    ' 2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ' ''' <summary>
    ' ''' 商談・一時対応開始
    ' ''' </summary>
    ' ''' <param name="custsegment">顧客区分</param>
    ' ''' <param name="custid">顧客ID</param>
    ' ''' <param name="salesstaffcd">スタッフコード</param>
    ' ''' <param name="fllwupboxdlrcd">販売店コード</param>
    ' ''' <param name="fllwupboxstrcd">店舗コード</param>
    ' ''' <param name="fllwupboxseqno">Follow-upBoxSeq</param>
    ' ''' <param name="salesstart">開始時間</param>
    ' ''' <returns>メッセージID</returns>
    ' ''' 2013/03/12 TCS 渡邊 【A.STEP2】新車タブレット受付画面の管理指標変更対応 Start
    ' ''' <param name="statusclass">ステータス区分</param>
    ' ''' 2013/03/12 TCS 渡邊 【A.STEP2】新車タブレット受付画面の管理指標変更対応 End
    ' ''' <remarks>商談・一時対応開始</remarks>
    'Public Function UpdateVisitSalesStart(ByVal custsegment As String, ByVal custid As String, ByVal salesstaffcd As String,
    '                                      ByVal fllwupboxdlrcd As String, ByVal fllwupboxstrcd As String,
    '                                      ByVal fllwupboxseqno As Decimal, ByVal salesstart As Date, ByVal statusclass As String) As Integer
    '    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '    Logger.Debug("UpdateVisitSalesStart Start")

    '    Dim msgid As Integer = Nothing
    '    Dim SalesVisit As New UpdateSalesVisitBusinessLogic

    '    ' 2013/03/12 TCS 渡邊 【A.STEP2】新車タブレット受付画面の管理指標変更対応 Start

    '    'If fllwupboxseqno <> 0 Then
    '    '    SalesVisit.UpdateVisitSalesStart(custsegment, custid, salesstaffcd, fllwupboxdlrcd, fllwupboxstrcd, fllwupboxseqno, salesstart, MODULEID, msgid)
    '    'Else
    '    '    SalesVisit.UpdateVisitSalesStart(custsegment, custid, salesstaffcd, Nothing, Nothing, Nothing, salesstart, MODULEID, msgid)
    '    'End If

    '    ' ステータス区分を追加
    '    If fllwupboxseqno <> 0 Then
    '        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    '        SalesVisit.UpdateVisitSalesStart(custsegment, custid, salesstaffcd, fllwupboxdlrcd, fllwupboxstrcd, CLng(fllwupboxseqno), salesstart, MODULEID, msgid, statusclass)
    '        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
    '    Else
    '        SalesVisit.UpdateVisitSalesStart(custsegment, custid, salesstaffcd, Nothing, Nothing, Nothing, salesstart, MODULEID, msgid, statusclass)
    '    End If
    '    ' 2013/03/12 TCS 渡邊 【A.STEP2】新車タブレット受付画面の管理指標変更対応 End

    '    Logger.Debug("UpdateVisitSalesStart End")

    '    '正常終了
    '    Return msgid
    'End Function


    ' ''' <summary>
    ' ''' 来店実績更新(商談終了・商談中断)
    ' ''' </summary>
    ' ''' <param name="custsegment">販売店コード</param>
    ' ''' <param name="custid">店舗コード</param>
    ' ''' <param name="salesend">終了時間</param>
    ' ''' <param name="prockb">処理区分  2012/08/13 TCS 安田 商談中断メニューの追加</param>
    ' ''' <returns>メッセージID</returns>
    ' ''' <remarks>来店実績更新(商談終了・商談中断)</remarks>
    'Public Function UpdateVisitSalesEnd(ByVal custsegment As String, ByVal custid As String, ByVal salesend As Date, ByVal prockb As String) As Integer

    '    Logger.Debug("UpdateVisitSalesEnd Start")

    '    Dim msgid As Integer = Nothing
    '    Dim SalesVisit As New UpdateSalesVisitBusinessLogic

    '    ' 2012/08/13 TCS 安田 商談中断メニューの追加 START
    '    Logger.Debug("UpdateVisitSalesEnd procKb = " + prockb)
    '    SalesVisit.UpdateVisitSalesEnd(custsegment, custid, salesend, MODULEID, msgid, prockb)
    '    ' 2012/08/13 TCS 安田 商談中断メニューの追加 END

    '    Logger.Debug("UpdateVisitSalesEnd End")

    '    '正常終了
    '    Return msgid
    'End Function


    ' ''' <summary>
    ' ''' 商談開始時Push送信
    ' ''' </summary>
    ' ''' <remarks>商談開始時Push送信</remarks>
    'Public Sub PushUpdateVisitSalesStart()

    '    Logger.Debug("PushUpdateVisitSalesStart Start")

    '    Dim SalesVisit As New UpdateSalesVisitBusinessLogic
    '    SalesVisit.PushUpdateVisitSalesStart()

    '    Logger.Debug("PushUpdateVisitSalesStart End")

    'End Sub

    '2012/09/06 TCS 山口 【A STEP2】次世代e-CRB 新車受付機能改善 START
    ' ''' <summary>
    ' ''' 商談終了時Push送信
    ' ''' </summary>
    ' ''' <remarks>商談終了時Push送信</remarks>
    'Public Sub PushUpdateVisitSalesEnd(ByVal status As String)

    '    Logger.Debug("PushUpdateVisitSalesEnd Start")

    '    Dim SalesVisit As New UpdateSalesVisitBusinessLogic

    '    If status = C_SALES_STOP Then
    '        '処理区分に2:商談中断を設定して呼び出し
    '        SalesVisit.PushUpdateVisitSalesEnd(UpdateSalesVisitBusinessLogic.LogicStateNegotiationStop)
    '    Else
    '        '処理区分に1:商談終了を設定して呼び出し
    '        SalesVisit.PushUpdateVisitSalesEnd(UpdateSalesVisitBusinessLogic.LogicStateNegotiationFinish)
    '    End If


    '    Logger.Debug("PushUpdateVisitSalesEnd End")

    'End Sub
    '2012/09/06 TCS 山口 【A STEP2】次世代e-CRB 新車受付機能改善 END

    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 自分が担当している継続中の活動があるか判定
    ''' </summary>
    ''' <param name="cstid">顧客ID</param>
    ''' <param name="account">アカウント</param>
    ''' <returns>判定結果</returns>
    ''' <remarks>自分が担当している継続中の活動があるか判定</remarks>
    Public Shared Function IsExistsNotCompleteAction(ByVal cstid As String,
                                                     ByVal account As String) As Boolean
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END

        Logger.Debug("IsExistsNotCompleteAction Start")

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Dim cnt As Integer = SC3080201TableAdapter.CountFllwupboxNotComplete(cstid, account)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        If cnt > 0 Then

            Logger.Debug("IsExistsNotCompleteAction End")

            '継続している活動が1件以上存在する
            Return True
        Else

            Logger.Debug("IsExistsNotCompleteAction End")

            '存在しない
            Return False
        End If
    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-upBox存在判定
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeq</param>
    ''' <returns>判定結果</returns>
    ''' <remarks>Follow-upBoxが存在するか判定</remarks>
    Public Function CountFllwupbox(ByVal fllwupboxseqno As Decimal) As Boolean
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Logger.Debug("CountFllwupbox Start")

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Dim cnt As Integer = SC3080201TableAdapter.CountFllwupbox(fllwupboxseqno)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        If cnt > 0 Then

            Logger.Debug("CountFllwupbox End")

            '継続している活動が1件以上存在する
            Return True
        Else

            Logger.Debug("CountFllwupbox End")

            '存在しない
            Return False
        End If
    End Function


    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ' ''' <summary>
    ' ''' Follow-upBox内連番取得
    ' ''' </summary>
    ' ''' <returns>Follow-upBox内連番</returns>
    ' ''' <remarks>Follow-upBox内連番を取得</remarks>
    'Public Function GetFllowSeq() As Decimal
    '    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '    Logger.Debug("GetFllowSeq Start")

    '    'Follow-upBoxSeqNoがない場合採番する
    '    Dim SeqDt As SC3080201DataSet.SC3080201SeqDataTable
    '    SeqDt = GetFllwupboxSeqno()
    '    Dim SeqRw As SC3080201DataSet.SC3080201SeqRow
    '    SeqRw = CType(SeqDt.Rows(0), SC3080201DataSet.SC3080201SeqRow)

    '    Logger.Debug("GetFllowSeq End")

    '    Return SeqRw.SEQ
    'End Function


    ' ''' <summary>
    ' ''' 来店実績連番取得
    ' ''' </summary>
    ' ''' <param name="dtParam">データセット</param>
    ' ''' <returns>来店実績連番</returns>
    ' ''' <remarks>顧客情報より来店実績連番を取得</remarks>
    'Public Function GetVisitSeq(ByVal dtParam As SC3080201DataSet.SC3080201SalesStartDataTable) As Long

    '    Logger.Debug("GetVisitSeq Start")

    '    Dim VisitSeq As Long
    '    Dim SalesVisit As New UpdateSalesVisitBusinessLogic
    '    VisitSeq = SalesVisit.GetVisitSeqBeforeSalesStart(dtParam(0).CUSTSEGMENT, dtParam(0).CRCUSTID)

    '    Logger.Debug("GetVisitSeq End")

    '    Return VisitSeq
    'End Function


    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 通知キャンセル処理
    ''' </summary>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeq</param>
    ''' <param name="custName">顧客名</param>
    ''' <returns>処理結果</returns>
    ''' <remarks>通知キャンセル処理</remarks>
    Public Function UpdateNoticeRequest(ByVal strcd As String, ByVal fllwupboxseqno As Decimal, ByVal custName As String) As Boolean
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Logger.Debug("UpdateNoticeRequest Start")

        'キャンセル対象件取得
        Dim NoticeRequestDt As SC3080201DataSet.SC3080201NoticeRequestDataTable
        NoticeRequestDt = SC3080201TableAdapter.GetNoticeRequest(strcd, fllwupboxseqno)

        If NoticeRequestDt.Count > 0 Then

            '商談テーブルNo.取得
            Dim GetVisitSalesDt As SC3080201DataSet.SC3080201VisitSalesDataTable
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
            GetVisitSalesDt = SC3080201TableAdapter.GetVisitSales(fllwupboxseqno)
            '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
            Dim salesTableNo As Integer = 0

            If GetVisitSalesDt.Count > 0 Then
                Dim GetVisitSalesRw As SC3080201DataSet.SC3080201VisitSalesRow
                GetVisitSalesRw = CType(GetVisitSalesDt.Rows(0), SC3080201DataSet.SC3080201VisitSalesRow)
                If Not GetVisitSalesRw.IsSALESTABLENONull Then
                    salesTableNo = GetVisitSalesRw.SALESTABLENO
                End If
            End If

            Dim rsltId As Integer
            Dim returnXmlNotice As XmlCommon
            Dim NoticeRequestRw As SC3080201DataSet.SC3080201NoticeRequestRow
            For i = 0 To NoticeRequestDt.Count - 1
                NoticeRequestRw = CType(NoticeRequestDt.Rows(i), SC3080201DataSet.SC3080201NoticeRequestRow)

                Dim ReqclassId As Nullable(Of Long) = Nothing
                If Not NoticeRequestRw.IsREQCLASSIDNull Then
                    ReqclassId = NoticeRequestRw.REQCLASSID
                End If

                Dim toAccount As String = ""
                If Not NoticeRequestRw.IsTOACCOUNTNull Then
                    toAccount = NoticeRequestRw.TOACCOUNT
                End If

                '通知登録API呼び出し
                returnXmlNotice = SetNoticeInfo(NoticeRequestRw.NOTICEREQCTG, NoticeRequestRw.NOTICEREQID, ReqclassId, custName, salesTableNo, toAccount)

                '処理結果が0以外の場合、処理を終了する
                rsltId = CInt(returnXmlNotice.ResultId)
                If rsltId <> 0 Then
                    Return False
                End If
            Next
        End If

        Logger.Debug("UpdateNoticeRequest Start")

        Return True
    End Function

    '2013/06/30 TCS 趙 2013/10対応版　既存流用 START
    ' 2012/02/15 TCS 相田 【SALES_2】 START
    ''' <summary>
    ''' 未取引客ユーザID取得
    ''' </summary>
    ''' <returns>ID</returns>
    ''' <remarks>ID取得</remarks>
    Public Function GetNewCstId(ByVal custId As String) As SC3080201DataSet.SC3080201CustchrgDataTable
        '2013/06/30 TCS 趙 2013/10対応版　既存流用 END
        Logger.Debug("GetFllwUpBoxCustchrgInfo Start")

        '未取引客ユーザIDを取得
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        custId = custId + " "
        Return New SC3080201DataSet.SC3080201CustchrgDataTable
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Logger.Debug("GetFllwUpBoxCustchrgInfo End")

    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ' ''' <summary>
    ' ''' 契約書No取得
    ' ''' </summary>
    ' ''' <param name="dlrcd">販売店コード</param>
    ' ''' <param name="strcd">店舗コード</param>
    ' ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ' ''' <returns>契約書No</returns>
    ' ''' <remarks> 契約書No取得</remarks>
    'Public Function GetContractNo(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Decimal) As String
    '    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '    Logger.Debug("GetContractInfo Start")

    '    '契約書Noを取得
    '    Dim contractNo As String = String.Empty

    '    Using inputSet As New ActivityInfoDataSet.ActivityInfoContractNoFromDataTable
    '        Dim inputRw As ActivityInfoDataSet.ActivityInfoContractNoFromRow = inputSet.NewActivityInfoContractNoFromRow()
    '        inputRw.DLRCD = dlrcd
    '        inputRw.STRCD = strcd
    '        inputRw.FLLWUPBOX_SEQNO = fllwupboxseqno
    '        inputSet.AddActivityInfoContractNoFromRow(inputRw)
    '        contractNo = ActivityInfoBusinessLogic.GetContractNo(inputSet)
    '    End Using

    '    Return contractNo

    '    Logger.Debug("GetContractInfo End")

    'End Function

    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
    ' 2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    '''  Follow-up Box商談更新
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <param name="salesFlg">商談フラグ</param>
    ''' <param name="startFlg">開始フラグ</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateFllwupboxSales(ByVal dlrcd As String, ByVal strcd As String, ByVal fllwupboxseqno As Decimal,
                                                ByVal salesFlg As Boolean,
                                                ByVal startFlg As Boolean) As Boolean
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Logger.Debug("UpdateFllwupboxSales Start")

        Dim Account = StaffContext.Current.Account

        SC3080201TableAdapter.UpdateFllwupboxSales(dlrcd, strcd, fllwupboxseqno, Account, MODULEID,
                                                   salesFlg, startFlg)

        Logger.Debug("UpdateFllwupboxSales End")

        '正常終了
        Return True
    End Function
    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END

    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 START
    ' 2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    '''  Follow-up Box商談の営業開始時間更新
    ''' </summary>
    ''' <param name="dlrcd">販売店コード</param>
    ''' <param name="strcd">店舗コード</param>
    ''' <param name="fllwupboxseqno">Follow-up Box内連番</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateFllwupboxEigyoStartTime(ByVal dlrcd As String, ByVal strcd As String,
                                                  ByVal fllwupboxseqno As Decimal) As Boolean
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Logger.Debug("UpdateFllwupboxEigyoStartTime Start")

        Dim Account = StaffContext.Current.Account

        SC3080201TableAdapter.UpdateFllwupboxSales(dlrcd, strcd, fllwupboxseqno, Account, MODULEID,
                                                   False, False)

        Logger.Debug("UpdateFllwupboxEigyoStartTime End")

        '正常終了
        Return True
    End Function
    ' 2012/03/06 TCS 山口 【SALES_2】課題番号0003対応 END

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 商談開始時間の取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeqNo</param>
    ''' <returns>商談開始時間</returns>
    ''' <remarks>商談開始時間取得</remarks>
    Public Function GetSalesTime(ByVal fllwupboxseqno As Decimal) As String
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        Logger.Debug("GetSalesTime Start")

        '商談開始時間を取得
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        Dim dataSet As SC3080201DataSet.SC3020801FllwUpBoxSaleDataTable =
            SC3080201TableAdapter.GetSalesTime(fllwupboxseqno)
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END
        Dim starttime As String = String.Empty
        If dataSet.Rows.Count > 0 Then
            Dim timeRw As SC3080201DataSet.SC3020801FllwUpBoxSaleRow
            timeRw = CType(dataSet.Rows(0), SC3080201DataSet.SC3020801FllwUpBoxSaleRow)
            If Not timeRw.IsSTARTTIMENull Then
                starttime = timeRw.STARTTIME
            End If
        End If

        Return starttime

        Logger.Debug("GetSalesTime End")

    End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 商談シーケンスNOの取得
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeqNo</param>
    ''' <returns>商談シーケンスNO</returns>
    ''' <remarks>商談シーケンスNO取得</remarks>
    Public Function GetSalesSeqNoByRegitFlg(ByVal fllwupboxseqno As Decimal) As SC3080201DataSet.SC3020801FllwUpBoxSaleDataTable
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

        Logger.Debug("GetSalesSeqNoByRegitFlg Start")

        '商談シーケンスNOを取得
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
        Dim dataSet As SC3080201DataSet.SC3020801FllwUpBoxSaleDataTable =
            SC3080201TableAdapter.GetSalesSeqNoByRegitFlg(fllwupboxseqno)
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

        Return dataSet

        Logger.Debug("GetSalesSeqNoByRegitFlg End")

    End Function

    ''' <summary>
    ''' 顧客担当情報取得
    ''' </summary>
    ''' <param name="custKind">顧客種別</param>
    ''' <param name="custId">顧客ID</param>
    ''' <returns>顧客担当情報</returns>
    ''' <remarks>顧客担当情報取得</remarks>
    Public Function GetCustchrgInfo(ByVal custKind As String,
                                           ByVal custId As String) As String

        Logger.Debug("GetCustchrgInfo Start")

        '顧客担当情報を取得
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Dim dataSet As SC3080201DataSet.SC3080201CustStrDataTable =
            SC3080201TableAdapter.GetCustchrgInfo(StaffContext.Current.DlrCD, custId, custKind)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        If dataSet.Rows.Count > 0 Then
            Dim strRw As SC3080201DataSet.SC3080201CustStrRow
            strRw = CType(dataSet.Rows(0), SC3080201DataSet.SC3080201CustStrRow)

            Return strRw.STRCDSTAFF
        End If
        Return String.Empty

        Logger.Debug("GetCustchrgInfo End")

    End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' Follow-up Box顧客担当情報
    ''' </summary>
    ''' <param name="fllwupboxseqno">Follow-upBoxSeqNo</param>
    ''' <returns>登録フラグ</returns>
    ''' <remarks>登録フラグ取得</remarks>
    Public Function GetFllwUpBoxCustchrgInfo(ByVal fllwupboxseqno As Decimal) As String
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Logger.Debug("GetFllwUpBoxCustchrgInfo Start")

        '登録フラグを取得
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        Dim dataSet As SC3080201DataSet.SC3080201CustStrDataTable =
            SC3080201TableAdapter.GetFllwUpBoxCustchrgInfo(fllwupboxseqno)
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END
        If dataSet.Rows.Count > 0 Then
            Dim strRw As SC3080201DataSet.SC3080201CustStrRow
            strRw = CType(dataSet.Rows(0), SC3080201DataSet.SC3080201CustStrRow)

            Return strRw.CUSTCHRGSTRCD
        End If
        Return String.Empty

        Logger.Debug("GetFllwUpBoxCustchrgInfo End")

    End Function

    ' ''' <summary>
    ' ''' CR活動成功のデータ存在判定
    ' ''' </summary>
    ' ''' <param name="dlrcd">販売店コード</param>
    ' ''' <param name="strcd">店舗コード</param>
    ' ''' <param name="fllwupboxseqno">Follow-upBoxSeq</param>
    ' ''' <returns>判定結果</returns>
    ' ''' <remarks>CR活動成功のデータが存在するか判定</remarks>
    'Public Function CountFllwupboxRslt(ByVal dlrcd As String,
    '                                   ByVal strcd As String,
    '                                   ByVal fllwupboxseqno As Long) As String

    '    Logger.Debug("CountFllwupboxRslt Start")

    '    '契約書Noを取得
    '    Dim flg As String = String.Empty

    '    Using inputSet As New ActivityInfoDataSet.ActivityInfoCountFromDataTable
    '        Dim inputRw As ActivityInfoDataSet.ActivityInfoCountFromRow = inputSet.NewActivityInfoCountFromRow()
    '        inputRw.DLRCD = dlrcd
    '        inputRw.STRCD = strcd
    '        inputRw.FLLWUPBOX_SEQNO = fllwupboxseqno
    '        inputSet.AddActivityInfoCountFromRow(inputRw)
    '        flg = ActivityInfoBusinessLogic.CountFllwupboxRslt(inputSet)
    '    End Using

    '    Return flg

    '    Logger.Debug("CountFllwupboxRslt End")
    'End Function

    ' ''' <summary>
    ' ''' 受注後活動状態取得
    ' ''' </summary>
    ' ''' <param name="dlrcd">販売店コード</param>
    ' ''' <param name="orderNo">契約書No</param>
    ' ''' <returns>受注後活動状態</returns>
    ' ''' <remarks>受注後活動状態取得</remarks>
    'Public Function GetWaitingObject(ByVal dlrcd As String,
    '                                   ByVal orderNo As String) As Boolean

    '    Logger.Debug("GetWaitingObject Start")

    '    '受注後活動状態を取得
    '    Dim rslt As String = String.Empty

    '    Using inputSet As New ActivityInfoDataSet.ActivityInfoWaitingObjectFromDataTable
    '        Dim inputRw As ActivityInfoDataSet.ActivityInfoWaitingObjectFromRow = inputSet.NewActivityInfoWaitingObjectFromRow()
    '        inputRw.DLRCD = dlrcd
    '        inputRw.SALESBKGNO = orderNo
    '        inputSet.AddActivityInfoWaitingObjectFromRow(inputRw)
    '        rslt = ActivityInfoBusinessLogic.GetWaitingObject(inputSet)
    '    End Using

    '    Select Case rslt
    '        Case ActivityInfoBusinessLogic.WAITINGOBJECT_ALLOCATION
    '            ' 受注後工程（振当待ち）
    '            Return True
    '        Case ActivityInfoBusinessLogic.WAITINGOBJECT_PAYMENT
    '            ' 受注後工程（入金待ち）
    '            Return True
    '        Case ActivityInfoBusinessLogic.WAITINGOBJECT_DELIVERY
    '            ' 受注後工程（納車待ち）
    '            Return True
    '        Case Else
    '            ' 受注後工程（納車済み）
    '            Return False
    '    End Select

    '    Logger.Debug("GetWaitingObject End")
    'End Function

    '2013/06/30 TCS 内藤 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 商談シーケンスNoの取得
    ''' </summary>
    ''' <returns>商談シーケンスNo</returns>
    ''' <remarks>商談シーケンスNoを取得</remarks>
    Public Function GetSalesSeqNo() As Decimal
        '2013/06/30 TCS 内藤 2013/10対応版　既存流用 END

        Logger.Debug("GetSalesSeqNo Start")

        'Follow-upBoxSeqNoがない場合採番する
        Dim SeqDt As SC3080201DataSet.SC3080201SeqDataTable
        SeqDt = SC3080201TableAdapter.GetSalesSeqNo()
        Dim SeqRw As SC3080201DataSet.SC3080201SeqRow
        SeqRw = CType(SeqDt.Rows(0), SC3080201DataSet.SC3080201SeqRow)

        Logger.Debug("GetSalesSeqNo End")

        Return SeqRw.SEQ
    End Function
    ' 2012/02/15 TCS 相田 【SALES_2】 END

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    '2013/03/06 TCS 河原 GL0874 START
    ' ''' <summary>
    ' ''' 契約状況フラグの取得
    ' ''' </summary>
    ' ''' <param name="datatableFrom"></param>
    ' ''' <returns>契約状況フラグ</returns>
    ' ''' <remarks>契約状況フラグの取得</remarks>
    'Public Shared Function GetContractFlg(ByVal datatableFrom As ActivityInfoDataSet.ActivityInfoContractNoFromDataTable) As String
    '    '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

    '    Logger.Debug("GetContractFlg Start")

    '    Logger.Debug("GetContractFlg End")

    '    Return ActivityInfoBusinessLogic.GetContractFlg(datatableFrom)
    'End Function

    '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
    ''' <summary>
    ''' 来店実績連番に紐付く活動情報の取得
    ''' </summary>
    ''' <param name="visiteqno"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetVisitFllwSeq(ByVal visiteqno As Long) As SC3080201DataSet.SC3080201VisitFllwSeqDataTable
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Logger.Debug("GetVisitFllwSeq Start")

        Logger.Debug("GetVisitFllwSeq End")

        Return SC3080201TableAdapter.GetVisitFllwSeq(visiteqno)
    End Function
    '2013/03/06 TCS 河原 GL0874 END


#End Region

#Region " 通知登録API呼び出し "

    ''' <summary>
    ''' 通知登録API呼び出し
    ''' </summary>
    ''' <param name="requestClass"></param>
    ''' <param name="requestId"></param>
    ''' <param name="requestClassId"></param>
    ''' <param name="customrtName"></param>
    ''' <param name="salesTableNo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetNoticeInfo(ByVal requestClass As String, ByVal requestId As Long,
                                  ByVal requestClassId As Nullable(Of Long), ByVal customrtName As String,
                                  ByVal salesTableNo As Integer, ByVal toAccount As String) As XmlCommon

        Logger.Debug("SetNoticeInfo Start")

        Dim returnXmlNotice As XmlCommon

        Dim dlrcd As String = StaffContext.Current.DlrCD
        Dim strcd As String = StaffContext.Current.BrnCD

        Using noticeData As New XmlNoticeData
            '送信日付
            noticeData.TransmissionDate = DateTimeFunc.Now()

            If String.Equals(requestClass, "01") Then
                '査定の場合
                '送信先の端末ID取得
                Dim retTerminalDataTbl As SC3080201DataSet.SC3080201TerminalIdDataTable
                retTerminalDataTbl = SC3080201TableAdapter.GetUcarTerminal(dlrcd, strcd)
                Dim retTerminalIdRw As SC3080201DataSet.SC3080201TerminalIdRow
                '取得した端末ID分ループ
                For i = 0 To retTerminalDataTbl.Count - 1
                    Using xmlAccount As New XmlAccount
                        retTerminalIdRw = CType(retTerminalDataTbl.Rows(i), SC3080201DataSet.SC3080201TerminalIdRow)
                        xmlAccount.ToClientId = retTerminalIdRw.TERMINALID
                        noticeData.AccountList.Add(xmlAccount)
                    End Using
                Next
            Else
                Using xmlAccount As New XmlAccount
                    '査定以外の場合
                    xmlAccount.ToAccount = toAccount
                    noticeData.AccountList.Add(xmlAccount)
                End Using
            End If

            Dim UserName As String = StaffContext.Current.UserName
            Dim Account As String = StaffContext.Current.Account

            Using requestNotice As New XmlRequestNotice
                requestNotice.DealerCode = dlrcd                                        '販売店コード
                requestNotice.StoreCode = strcd                                         '店舗コード
                requestNotice.RequestClass = requestClass                               '依頼種別
                requestNotice.Status = "2"                                              'ステータス
                requestNotice.RequestId = requestId                                     '依頼種別ID
                If Not IsNothing(requestClassId) Then
                    requestNotice.RequestClassId = CLng(requestClassId)                 '依頼ID
                End If
                requestNotice.FromAccount = Account                                     'スタッフコード（送信元）
                requestNotice.FromAccountName = UserName                                'スタッフ名（送信元）
                noticeData.RequestNotice = requestNotice
            End Using

            Using pushInfo As New XmlPushInfo
                '依頼種別が価格相談、ヘルプの場合カテゴリは2(アクション)
                If String.Equals(requestClass, "02") Or String.Equals(requestClass, "03") Then
                    '価格相談、ヘルプの場合
                    pushInfo.PushCategory = "1"                                             'カテゴリータイプ
                    pushInfo.PositionType = "1"                                             '表示位置
                    pushInfo.Time = 3                                                       '表示時間
                    pushInfo.DisplayType = "1"                                              '表示タイプ
                    pushInfo.Color = "1"                                                    '色
                    pushInfo.DisplayContents = WebWordUtility.GetWord(10918)                '表示内容
                    pushInfo.DisplayFunction = "icropScript.ui.openNoticeList()"            '表示時間数
                    pushInfo.ActionFunction = "icropScript.ui.openNoticeList()"             'アクション時間数
                Else
                    '査定の場合
                    pushInfo.PushCategory = "1"                                             'カテゴリータイプ
                    pushInfo.PositionType = "0"                                             '表示位置
                    pushInfo.DisplayType = "1"                                              '表示タイプ

                    '文言の生成
                    Dim wordDispContents As String
                    wordDispContents = WebWordUtility.GetWord(10916).Replace("{0}", UserName)
                    wordDispContents = wordDispContents.Replace("{1}", customrtName)
                    If salesTableNo > 0 Then
                        Dim wordTableNo As String
                        wordTableNo = WebWordUtility.GetWord(10917).Replace("{0}", CStr(salesTableNo))
                        wordDispContents = wordDispContents.Replace("{2}", wordTableNo)
                    Else
                        wordDispContents = wordDispContents.Replace("{2}", "")
                    End If
                    Dim cateWord As String
                    If String.Equals(requestClass, "01") Then
                        cateWord = WebWordUtility.GetWord(10170)
                    ElseIf String.Equals(requestClass, "02") Then
                        cateWord = WebWordUtility.GetWord(10171)
                    ElseIf String.Equals(requestClass, "03") Then
                        cateWord = WebWordUtility.GetWord(10172)
                    Else
                        cateWord = ""
                    End If
                    wordDispContents = wordDispContents.Replace("{3}", cateWord)
                    pushInfo.DisplayContents = wordDispContents                             '表示内容
                End If
                noticeData.PushInfo = pushInfo
            End Using

            Using noticeInfo As New IC3040801BusinessLogic
                returnXmlNotice = noticeInfo.NoticeDisplay(noticeData, ConstCode.NoticeDisposal.Peculiar)
            End Using

            Logger.Debug("SetNoticeInfo End")

            Return returnXmlNotice

        End Using

        Logger.Debug("SetNoticeInfo End")

    End Function

#End Region
    '2012/01/24 TCS 河原 【SALES_1B】 END


    ' 2012/02/29 TCS 安田 【SALES_2】 START
    ' ''' <summary>
    ' ''' 活動中FLLWUPBOX_SEQNO取得
    ' ''' </summary>
    ' ''' <param name="dlrcd">販売店コード</param>
    ' ''' <param name="strcd">店舗コード</param>
    ' ''' <param name="insdid">未取引客ID／自社客連番</param>
    ' ''' <param name="cstkind">未取引客:2／自社客種別:1</param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function GetSalesActiveList(ByVal dlrcd As String,
    '                                   ByVal strcd As String,
    '                                   ByVal insdid As String,
    '                                   ByVal cstkind As String,
    '                                   ByVal newcustid As String) As String

    '    Logger.Debug("GetSalesActiveList Start")

    '    Dim dt As ActivityInfoDataSet.ActivityInfoSalesActiveListDataTable = _
    '      ActivityInfoBusinessLogic.GetSalesActiveList(dlrcd, strcd, insdid, cstkind, newcustid)

    '    If (dt.Rows.Count > 0) Then
    '        Return CType(dt.Item(0).FLLWUPBOX_SEQNO, String)
    '    Else
    '        Return String.Empty
    '    End If

    '    Logger.Debug("GetSalesActiveList End")

    'End Function
    ' 2012/02/29 TCS 安田 【SALES_2】 END

#Region "FS開発"

    ''' <summary>
    ''' SNSIDの更新処理
    ''' </summary>
    ''' <param name="snsIdDT">更新用データテーブル</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdateSnsId(ByVal snsIdDT As SC3080201DataSet.SC3080201CustSnsIdDataTable) As Boolean

        Dim rw As SC3080201DataSet.SC3080201CustSnsIdRow
        rw = CType(snsIdDT.Rows(0), SC3080201DataSet.SC3080201CustSnsIdRow)

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        '親顧客データロック
        Try
            SC3080201TableAdapter.GetCustomerLock(rw.CSTID)
        Catch ex As OracleExceptionEx
            Return False
        End Try
        Dim ret As Integer
        ret = SC3080201TableAdapter.UpdateNewCustomerSnsId(rw.CSTID, rw.MODE, rw.SNSID, StaffContext.Current.Account)
        If ret = 0 Then
            Me.Rollback = True
            Return False
        End If
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Return True
    End Function

    ''' <summary>
    ''' Keywordの更新処理
    ''' </summary>
    ''' <param name="snsIdDT">更新用データテーブル</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <EnableCommit()>
    Public Function UpdateKeyword(ByVal snsIdDT As SC3080201DataSet.SC3080201CustKeywordDataTable) As Boolean

        Dim rw As SC3080201DataSet.SC3080201CustKeywordRow
        rw = CType(snsIdDT.Rows(0), SC3080201DataSet.SC3080201CustKeywordRow)

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START
        '親顧客データロック
        Try
            SC3080201TableAdapter.GetCustomerLock(rw.CSTID)
        Catch ex As OracleExceptionEx
            Return False
        End Try

        Dim cnt As Integer  ' 更新件数退避
        cnt = SC3080201TableAdapter.UpdateNewCustomerKeyword(rw.CSTID, rw.KEYWORD, StaffContext.Current.Account, StaffContext.Current.DlrCD, rw.ROWLOCKVERSION)
        If cnt = 0 Then
            Me.Rollback = True
            Return False
        End If
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        '2013/06/30 TCS 庄 2013/10対応版　既存流用 START DEL
        '2013/06/30 TCS 庄 2013/10対応版　既存流用 END

        Return True
    End Function

#End Region

End Class
