'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3802801BusinessLogic.vb
'─────────────────────────────────────
'Ver.  : SA01_LC_026
'Create: 2014/01/15[SA01_GL_001] FJ Takeda
'Update: 2014/03/17[SA01_GL_002] FJ Takeda (Add try catch)
'      : 2014/03/24[SA01_GL_003] FJ Takeda (Mod CustomerType)
'      : 2014/04/08[SA01_GL_004] FJ Hori   (Mod LogType [Info -> Error] )
'      : 2014/04/19[SA01_GL_005] FJ Takeda (GL Version Issue 対応 )
'      : 2014/04/21[SA01_GL_006] FJ Hori   (Test Mode Logic Merge )
'      : 2014/04/22[SA01_GL_006_ZANTEI] FJ Takeda (Action EndTime)(暫定)
'      : 2014/04/25[SA01_GL_007_ZANTEI] FJ Takeda (Address Log Info)(暫定)※性能測定スタンプあり
'      : 2014/05/20[SA01_GL_008] FJ Hori   (PGID Change IC3802601->IC3802801)
'      : 2014/05/23[SA01_GL_009] FJ Takeda (販売店コード、店舗コード未設定時のマスタ取得変更対応)
'      : 2014/05/29[SA01_GL_010] FJ Takeda (3/28に追加したFollowUpResultのログ削除)
'      : 2014/05/30[SA01_GL_011] FJ Takeda [1](日付編集部品の変更。DB初期値の場合、空文字("")を返却)
'                                          [2](直販フラグがDB初期値の場合0設定)
'      : 2014/05/31[SA01_GL_012] FJ Takeda 住所情報の編集。DB初期値の場合、空文字("")を設定
'      : 2014/06/02[SA01_GL_013] FJ Takeda SalesConditionタグ=0件の場合、タグ無しとする
'      : 2014/06/06[SA01_GL_014] FJ Takeda SalesConditionタグの出力順序修正(SalesConditionNoが同じ場合、出力しない)
'      : 2014/06/07[SA01_GL_015] FJ Takeda 用件ソース(1st)コード(SOURCE_1_CD)のコード変換対応
'      : 2014/06/09[SA01_GL_016] FJ Takeda 用件ソース(2nd)名称の取得不具合対応
'      : 2014/06/12[SA01_GL_017] FJ Takeda Infomationログ出力の呼出し削除。※一括修正のため修正履歴なし
'                                          "Logger.Info(...)" -> "Delete-Logger.Info(...)"に一括変更する
'      : 2014/06/12[SA01_GL_018] FJ Takeda 自社客コードの編集方法変更(販売店@自社客コード ->自社客コード)
'      : 2014/06/17[SA01_GL_019] FJ Takeda エラーログ出力見直し
'      : 2014/06/19[SA01_GL_020] FJ Takeda 性能スタンプ(活動SEQ管理の登録処理)をコメントアウト
'      : 2014/07/08[SA01_GL_021] FJ Takeda SourceID2の変換エラー対応。SourceID1=0の時、連携ファイルはタグのみにする
'      : 2014/07/22[SA01_GL_022] FJ Takeda 活動SEQ管理(TB_T_ACTIVITY_SEQ_MANAGER)の登録不具合対応。
'                                          ROW_CREATE_ACCOUNT,ROW_UPDATE_ACCOUNTにUSERNAMEではなくACCOUNTを設定(TBL_USERS)
'      : 2015/01/09[SA01_GL_023] FJ Takeda CIntをClngへ変更(一括置き換えのため修正履歴なし)
'      : 2015/02/05[SA01_GL_024] SKFC MATSUDA エスケープ処理対応
'      : 2016/05/10[SA01_GL_025] NSK Nakamura （トライ店システム評価）他システム連携における複数店舗コード変換対応 $25
'      : 2018/07/05[SA01_LC_026] NSK Niiya TKM Next Gen e-CRB Project Application development Block B-2 SI No.1,2,4,5,6 $26
'      : 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証
'      : 2020/01/06[SA01_LC_027] NSK Natsume TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) $27
'─────────────────────────────────────
Imports Toyota.eCRB.iCROP.DataAccess.IC3802801
Imports Toyota.eCRB.SystemFrameworks.Core
Imports System.Xml
Imports System.Net
Imports Toyota.eCRB.iCROP.DataAccess.IC3802801.IC3802801DataSet
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports Toyota.eCRB.SystemFrameworks.Web

Imports System.Data
Imports System.Text
Imports System.IO
Imports System.Net.Sockets
Imports System.Web
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.SystemSettingDlrDataSet
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess.SystemEnvSettingDataSet
' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

Public Class IC3802801BusinessLogic

    Dim IC3802801TableAdapter As New IC3802801TableAdapter
    ' Common information																
    Public Const CstStrPGID As String = "IC3802801"

    'Dealer System Setting																				
    Public Const CstStrActivityJudgeFlg As String = "ACT_JUDGE_FLG"
    Public Const CstStrProcessJudgeFlg As String = "PRCS_JUDGE_FLG"
    Public Const CstStrErrorInfo As String = "ERROR_INFO"
    Public Const CstStrSendProspectCstUrl As String = "SEND_PROSPECT_CST_URL"
    Public Const CstStrSendDmsSvr As String = "SEND_DMS_SVR"
    Public Const CstStrInitProspectStatus As String = "INIT_PROSPECT_STATUS"
    Public Const CstStrOutReturnIFUrl As String = "OUT_RETURN_INTERFACE_URL"

    'Dealer System Setting																									
    Public GlActivityJudgeFlg As String                   ' 	Activity Linkage Flag									
    Public GlProcessJudgeFlg As String                   ' 	Process Linkage Flag									
    Public GlErrorInfo As String                   ' 	ErrorInfo(FileDirectory & FileName)									
    Public GlSendProspectCstUrl As String                   ' 	Linkage File URL									
    Public GlSendDmsSvr As String                   ' 	Sending DMS server									
    Public GlInitProspectStatus As String                   ' 	First time activity potential(Potential Division)									
    Public GlOutReturnIFUrl As String                   ' 	Response Interface URL(Output)	
    '-> 20140421 FJ.Hori Test Mode Create Start
    Public GlNoDMSFlg As String = "0"
    Public GlTestResponseXML As String = ""
    '-> 20140421 FJ.Hori Test Mode Create End
    Public GlOutProspectCstUrl As String
    ''ISSUE99_Start
    Public GlOutResponseData As String                   ' 	Response Data(Output)	
    ''ISSUE99_End
    'ISSUE_IT2-1_by_takda_update_start
    Public GlACardNo As String
    'ISSUE_IT2-1_by_takda_update_end

    '==========takeda_update_start_20140619_コメントアウト==========
    ' ''==========takeda_update_start_20140422_性能改善調査==========
    ''Public GlDmSalesId As Decimal
    ' ''==========takeda_update_end_20140422_性能改善調査============
    '==========takeda_update_end_20140619_コメントアウト==========


    ' System environment setting																									
    Public GlDmsStatusCold As String                   ' 	Dealer activity potential(Cold)									
    Public GlDmsStatusWarm As String                   ' 	Dealer activity potential(Warm)									
    Public GlDmsStatusHot As String                   ' 	Dealer activity potential(Hot)									
    Public GlDmsResultSuccess As String                   ' 	Dealer activity result(Success)									
    Public GlDmsResultContinue As String                   ' 	Dealer activity result(Continue)									
    Public GlDmsResultGiveup As String                   ' 	Dealer activity result(Give-Up)									

    Public GlIcropStatusCold As String()                 ' 	ICROP activity potential(Cold)									
    Public GlIcropStatusWarm As String()                 ' 	ICROP activity potential(Warm)									
    Public GlIcropStatusHot As String()                 ' 	ICROP activity potential(Hot)									
    Public GlIcropResultSuccess As String()                 ' 	ICROP activity result(Success)									
    Public GlIcropResultContinue As String()                 ' 	ICROP activity result(Continue)									
    Public GlIcropResultGiveup As String()                 ' 	ICROP activity result(Give-Up)	

    Public GlRelationTypeNewCstCd As String                    ' 	New customer code linkage type		
    '20140317 Fujita Add Start
    Public GlBooLog As Boolean                    ' 	Log hantei
    '20140317 Fujita Add End
    'takeda_update_start_20140617
    Public GlErrStepInfo As String
    'takeda_update_end_20140617

    '$25 他システム連携における複数店舗コード変換対応 start 
    Private GlDmsCodeMapUseColumn As String
    '$25 他システム連携における複数店舗コード変換対応 end 

    ' System environment setting																			
    Public Const CstStrActStatusCold As String = "Cnv_PotentialDiviCD_Cold"
    Public Const CstStrActStatusWarm As String = "Cnv_PotentialDiviCD_Warm"
    Public Const CstStrActStatusHot As String = "Cnv_PotentialDiviCD_Hot"
    Public Const CstStrActResultSuccess As String = "Cnv_ActivityRsltCD_Success"
    Public Const CstStrActResultContinue As String = "Cnv_ActivityRsltCD_Continue"
    Public Const CstStrActResultGiveup As String = "Cnv_ActivityRsltCD_Giveup"
    'Public Const CstStrRelationTypeNewCstCd As String = "Cnv_RelationTypeNewCstCd"                          ' 	New customer code linkage type	
    Public Const CstStrRelationTypeNewCstCd As String = "RelationTypeNewCstCd_Flg"                          ' 	New customer code linkage type					


    'First time success flag																									
    Public Const CstStrSalesNormal As String = "0"             ' 	In case of normal contract							
    Public Const CstStrFirstSuccess As String = "1"             ' 	In case of first time success							

    'Activity Results Linkage Type																									
    Public Const CstStrLinkageOn As String = "1"             ' 	Linkage-On							
    Public Const CstStrLinkageOff As String = "0"             ' 	Linkage-Off							

    'Action Status																									
    Public Const CstStrCold As String = "1"             ' 	Cold							
    Public Const CstStrWarm As String = "2"             ' 	Warm							
    Public Const CstStrHot As String = "3"             ' 	Hot	

    ' Sales completion flag																						
    Public Const CstStrContinue As String = "0"             ' 	Sale in progress				
    Public Const CstStrComplete As String = "1"             ' 	Sale completed				


    'Relation Action Type																									
    Public Const CstStrActionType As String = "2"         ' 	Relation Action Type = Action								

    'Timeslot Type																									
    Public Const CstStrIcropTimeslot As String = "1"             ' 	ICROP Fixed Value = "1"							

    'Customer vehicle																									
    Public Const CstStrOwner As String = "1"             ' 	Owner							

    ' Interface Mode																									
    Public Const CstStrModeDMS As String = "DMS"
    Public Const CstStrModeICROP As String = "ICROP"
    Public Const CstStrCdType1 As String = "1"             '	Dealer Code							
    Public Const CstStrCdType2 As String = "2"             '	Branch Code							
    Public Const CstStrCdType11 As String = "11"                '	Action Code		
    Public Const CstStrCdType13 As String = "13"                '	Giveup Reason Code
    Public Const CstStrCdType12 As String = "12"                '	Occupation Code
    'takeda_update_start_20140607
    Public Const CstStrCdType14 As String = "14"                '	Source 1 ID
    'takeda_update_end_20140607

    ' Activity type (action code)																					
    Public Const CstStrActTypeAction As String = "1"             ' 	Activity			
    Public Const CstStrActTypeSalesAction As String = "2"             ' 	Sales activity			

    'Activity Result Flag																			
    Public Const CstStrActRsltFlgOff As String = "0"             ' 	none	
    Public Const CstStrActRsltFlgOn As String = "1"             ' 	

    ' Owner change flag																					
    Public Const CstStrOwnerChanged As String = "1"             ' 	Owner changed			


    'ISSUE-0008_20130217_by_takeda_Start
    'Delete Flag																			
    Public Const CstStrDeleteFlgOff As String = "0"             '  none
    Public Const CstStrDeleteFlgOn As String = "1"             '  delete
    'ISSUE-0008_20130217_by_takeda_End

    'takeda_update_start_20140324
    'CustomerType
    Public Const CstStrCstType0 As String = "0"
    Public Const CstStrCstType1 As String = "1"
    'takeda_update_start_20140324

    'CustomerType
    Public Const maxSalesConditionInfo As Long = 6
    'takeda_update_start_20140412
    'StaffCode(Convert)
    Public Const CstStfCdCnvtOff As String = "0"     'StaffCode@DealerCode(No Convert)
    Public Const CstStfCdCnvtOn As String = "1"     'StaffCode@DealerCode -> StaffCode(Convert)
    'takeda_update_end_20140412
    'takeda_update_end_20140324

    'takeda_update_start_20140523
    Public Const CstStrCmnDlrCd As String = "XXXXX" 'Dealer Code (Common)
    Public Const CstStrCmnBrnCd As String = "XXX"   'Branch Code (Common)
    'takeda_update_end_20140523

    'takeda_update_start_20140523
    'Master Info (No Master Data -> Default Setting)
    Public Const CstDfltActivityJudgeFlg As String = "1"
    Public Const CstDfltProcessJudgeFlg As String = "0"
    Public Const CstDfltErrorInfo As String = ""
    Public Const CstDfltSendProspectCstUrl As String = ""
    Public Const CstDfltSendDmsSvr As String = ""
    Public Const CstDfltInitProspectStatus As String = "3"
    Public Const CstDfltOutReturnIFUrl As String = ""
    'takeda_update_end_20140523

    ' For item error check																									
    Public Const CstStrMandatory As String = "MANDATORY"
    Public Const CstStrOptional As String = "OPTIONAL"
    Public Const CstStrNone As String = "NONE"
    Public Const CstStrTypeString As String = "STRING"
    Public Const CstStrTypeNumber As String = "NUMBER"                ' 	Number Type							
    Public Const CstStrTypeDate As String = "DATE"              ' 	Date Type							
    Public Const CstStrTypeHalfChar As String = "HALFCHAR"              ' 	HalfChar Type							
    Public Const CstStrTypeByte As String = "BYTE"              ' 	2Byte Characters mixed (MIX)

    '$25 他システム連携における複数店舗コード変換対応 start 
    Private Const CstStrDmsCodeMapDmsColumnDefault As String = "DMS_CD_2"
    Private Const CstStrDmsCodeMapBrnCdKey As String = "DMS_CODE_MAP_BRN_CD"
    '$25 他システム連携における複数店舗コード変換対応 end 

    'ISSUE-0029_20130221_by_chatchai_Start
    '初期日付(Default Date)																							
    Public ReadOnly CstStrDefaultDate As DateTime = New DateTime(1900, 1, 1)    '初期日付(Default Date)	
    'ISSUE-0029_20130221_by_chatchai_End

    '$26 start
    '下取り車両利用可能フラグ
    Private Const TradeincarEnabledAvailable As String = "1"
    '$26 end

    ' For checking length (MinLength,MaxLength)																									
    Public Enum ItemCheck
        'Item Size(Max Length)																								
        Size1 = 1
        Size2 = 2
        Size6 = 6
        Size7 = 7
        Size19 = 19
        Size32 = 32
        Size1024 = 1024
    End Enum

    ' For checking permitted value (Value)																									
    Public Const CstValueMessageId As String = "IC35182"

    ' Common error message																									
    Public Const CstStrErrMsgMandatory As String = "Mandatory Item Error"
    Public Const CstStrErrMsgItemType As String = "Item Type Error"
    Public Const CstStrErrMsgItemSize As String = "Item Size Error"
    Public Const CstStrErrMsgValue As String = "Value Error"
    Public Const CstStrErrMsgXmlIncorrect As String = "XML Incorrect"
    Public Const CstStrErrMsgHttpInitialize As String = "HTTP Initialize Error"
    Public Const CstStrErrMsgHttpConnect As String = "HTTP Connection Error"
    Public Const CstStrErrMsgIISConnect As String = "IIS Open Error"
    'TB no data error message																									
    Public Const CstStrErrMsgNoData01 As String = "SalesTemp No Data"
    Public Const CstStrErrMsgNoData02 As String = "Request No Data"
    Public Const CstStrErrMsgNoData03 As String = "Attract No Data"
    Public Const CstStrErrMsgNoData04 As String = "Action No Data"
    Public Const CstStrErrMsgNoData05 As String = "SalesCondition No Data"
    Public Const CstStrErrMsgNoData06 As String = "Vehicle No Data"
    Public Const CstStrErrMsgNoData07 As String = "Customer No Data"

    'XML Infomation(Root Directory)																									
    Public Const CstXmlRootDirectory As String = "//"

    'XML Tag Name (Response)																								
    Public Const CstXmlResponse As String = "Response"
    Public Const CstXmlHead As String = "Head"
    Public Const CstXmlDetail As String = "Detail"
    Public Const CstXmlDetailCommon As String = "Common"
    Public Const CstXmlDetailFollowUpInfo As String = "FollowUpInfo"

    'XML Node Name (Response)																								
    Public Const CstNodeMessageID As String = "MessageID"
    Public Const CstItemCountryCode As String = "CountryCode"
    Public Const CstItemReceptionDate As String = "ReceptionDate"
    Public Const CstItemTransmissionDate As String = "TransmissionDate"


    Public Const CstNodeResultId As String = "ResultId"
    Public Const CstItemMessage As String = "Message"
    Public Const CstNodeFollowUpID As String = "FollowUpID"

    ' Common error code (by items)							<Head>																		
    Public Enum Head
        MessageID = 1
        CountryCode = 2
        ReceptionDate = 3
        TransmissionDate = 4
    End Enum

    ' Common error code (by items)							<Detail><Common>																		
    Public Enum DetailCommon
        ResultId = 5
        Message = 6
    End Enum

    ' Common error code (by items)							<Detail><FollowUpInfo>																		
    Public Enum DetailFollowUpInfo
        FollowUpID = 7
    End Enum

    'Node name				(ReturnCode)																					
    Public Enum NodeName
        Response = 100
        Head = 200
        Detail = 300
        DetailCommon = 400
        DetailFollowUp = 500
    End Enum

    Public Function SetXmlHead(ByVal CountryCode As String, ByVal TransmissionDate As DateTime) As XmlHead
        GlErrStepInfo = "SetXmlHead_Start"
        '20140317 Fujita Upd Start
        Try
            Dim xmlHead As New XmlHead
            xmlHead.MessageID = "IC35182"
            xmlHead.CountryCode = CountryCode
            xmlHead.LinkSystemCode = "0"
            xmlHead.TransmissionDate = ChangeDefaultDate(TransmissionDate, CstStrDefaultDate)
            GlErrStepInfo = "SetXmlHead_End"
            Return xmlHead
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlCommon(ByVal IcropDealerCode As String, ByVal IcropBranchCode As String) As XmlCommon
        GlErrStepInfo = "SetXmlCommon_Start"
        Dim xmlCommon As New XmlCommon
        '20140317 Fujita Upd Start
        Try
            xmlCommon.DealerCode = EditLength(ChangeDlrCd(IcropDealerCode, "DMS"), 1, 20, CstStrTypeString)
            xmlCommon.BranchCode = EditLength(ChangeBranchCd(IcropDealerCode, IcropBranchCode, "DMS"), 1, 20, CstStrTypeString)
            xmlCommon.IcropDealerCode = IcropDealerCode
            xmlCommon.IcropBranchCode = IcropBranchCode
            GlErrStepInfo = "SetXmlCommon_End"
            Return (xmlCommon)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlFollowUpResult(ByVal IcropDealerCode As String, ByVal IcropBranchCode As String, ByVal FirstSuccessFlg As String,
                                                ByVal SalesData As IC3802801DataSet.IC3802801SalesRow,
                                                ByVal CompetitorSeriesData As IC3802801DataSet.IC3802801CompetitorSeriesDataTable,
                                                ByVal ActionResultData As IC3802801DataSet.IC3802801ActionResultRow,
                                                ByVal EstimateData As IC3802801DataSet.IC3802801EstimateInfoRow,
                                                ByVal SelectedSeriesData As IC3802801DataSet.IC3802801SelectedSeriesDataTable,
                                                ByVal FollowUpResultData As IC3802801DataSet.IC3802801ActionRow,
                                                ByVal FollowUpActionData As IC3802801DataSet.IC3802801ActionRow,
                                                ByVal LastActionKeyData As IC3802801DataSet.IC3802801ActionRow,
                                                ByVal MakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
                                                ByVal SalesTempData As IC3802801DataSet.IC3802801SalesTempRow,
                                                ByVal EstimateVclData As IC3802801DataSet.IC3802801EstimateVclInfoRow,
                                                ByVal EstimateVclDataT As IC3802801DataSet.IC3802801EstimateVclInfoDataTable
                                                ) As XmlFollowUpResult
        GlErrStepInfo = "SetXmlFollowUpResult_2_Start"

        '20140310 FUJITA UPD START
        Dim SeriesCode As String = ""
        Dim GradeCode As String = ""
        Dim ExteriorColorCode As String = ""
        Dim InteriorColorCode As String = ""
        Dim ModelSuffix As String = ""
        GlErrStepInfo = "SetXmlFollowUpResult_2_0"
        If (EstimateVclDataT.Rows.Count > 0) Then
            SeriesCode = EstimateVclData.SERIESCD
            GradeCode = EstimateVclData.MODELCD
            ExteriorColorCode = EstimateVclData.EXTCOLORCD
            InteriorColorCode = EstimateVclData.INTCOLORCD
            ModelSuffix = EstimateVclData.SUFFIXCD
        End If
        'Dim SeriesCode As String = EstimateVclData.SERIESCD
        'Dim GradeCode As String = EstimateVclData.MODELCD
        'Dim ExteriorColorCode As String = EstimateVclData.EXTCOLORCD
        'Dim InteriorColorCode As String = EstimateVclData.INTCOLORCD
        'Dim ModelSuffix As String = EstimateVclData.SUFFIXCD
        '20140310 FUJITA UPD END

        Dim GiveUpReasonCode As String = ""
        Dim GiveUpReason As String = ""
        Dim GiveUpMemo As String = ""
        Dim GiveUpMakerCode As String = ""
        Dim GiveUpMakerName As String = ""
        Dim GiveUpSeriesCode As String = ""
        Dim GiveUpSeriesName As String = ""
        Dim xmlFollowUpResult As New XmlFollowUpResult

        '20140317 Fujita Upd Start
        Try
            GlErrStepInfo = "SetXmlFollowUpResult_2_0_1"
            'SelectedSeriesNo
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                'ISSUE-0003,ISSUE-0019_20130219_by_chatchai_Start
                Dim resultSelectedSeriesNo = From TB_T_PREFER_VCL In SelectedSeriesData
                 Where TB_T_PREFER_VCL.SALES_ID = SalesData.SALES_ID And
                       TB_T_PREFER_VCL.SALES_PROSPECT_CD <> " "
                  Select New With {
                    TB_T_PREFER_VCL.PREF_VCL_SEQ
                  }
                'ISSUE-0003,ISSUE-0019_20130219_by_chatchai_End

                GlErrStepInfo = "SetXmlFollowUpResult_2_1"
                If (resultSelectedSeriesNo.Count() > 0) Then
                    xmlFollowUpResult.SelectedSeriesNo = resultSelectedSeriesNo.ToList()(0).PREF_VCL_SEQ.ToString()
                Else
                    xmlFollowUpResult.SelectedSeriesNo = ""
                End If
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                Dim resultSelectedSeriesNo = From TB_T_PREFER_VCL In SelectedSeriesData
                 Where TB_T_PREFER_VCL.SALES_ID = SalesTempData.SALES_ID And
                       TB_T_PREFER_VCL.SALES_PROSPECT_CD = "1"
                  Select New With {
                    TB_T_PREFER_VCL.PREF_VCL_SEQ
                  }

                GlErrStepInfo = "SetXmlFollowUpResult_2_2"
                If (resultSelectedSeriesNo.Count() > 0) Then
                    xmlFollowUpResult.SelectedSeriesNo = resultSelectedSeriesNo.ToList()(0).PREF_VCL_SEQ.ToString()
                Else
                    xmlFollowUpResult.SelectedSeriesNo = ""
                End If
            End If

            GlErrStepInfo = "SetXmlFollowUpResult_2_3"
            'FollowUpResultDate
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                GlErrStepInfo = "SetXmlFollowUpResult_2_3_1"
                xmlFollowUpResult.FollowUpResultDate = ChangeDefaultDate(FollowUpResultData.RSLT_DATETIME, CstStrDefaultDate)
                GlErrStepInfo = "SetXmlFollowUpResult_2_3_2"
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                GlErrStepInfo = "SetXmlFollowUpResult_2_3_3"
                xmlFollowUpResult.FollowUpResultDate = ChangeDefaultDate(EstimateData.CONTRACT_APPROVAL_REQUESTDATE, CstStrDefaultDate)
            End If
            GlErrStepInfo = "SetXmlFollowUpResult_2_4"

            'Issue-0031_20130221_by_chatchai_Start
            'FollowedBranchCode
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                xmlFollowUpResult.FollowedBranchCode = ChangeBranchCd(IcropDealerCode, IcropBranchCode, "DMS")
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                xmlFollowUpResult.FollowedBranchCode = ChangeBranchCd(IcropDealerCode, IcropBranchCode, "DMS")
            End If
            'Issue-0031_20130221_by_chatchai_End
            GlErrStepInfo = "SetXmlFollowUpResult_2_5"

            'FollowedAccount
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                'takeda_update_start_20140412
                'xmlFollowUpResult.FollowedAccount = FollowUpResultData.RSLT_STF_CD
                xmlFollowUpResult.FollowedAccount = ConvertStfCd(FollowUpResultData.RSLT_STF_CD, CstStfCdCnvtOff)
                'takeda_update_end_20140412
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                xmlFollowUpResult.FollowedAccount = ""
            End If
            GlErrStepInfo = "SetXmlFollowUpResult_2_6"

            'ActivityResult
            If (FollowUpResultData IsNot Nothing) Then
                GlErrStepInfo = "SetXmlFollowUpResult_2_6(FollowupResultData_Exist)"
                'GlErrStepInfo = "ACT_STATUS(Database)"
                'GlErrStepInfo = "FollowUpResultData.ACT_STATUS" + FollowUpResultData.ACT_STATUS

                'GlErrStepInfo = "ActivityResult(ICROP)"
                'GlErrStepInfo = "Success[0]:" + GlIcropResultSuccess.GetValue(0)
                'GlErrStepInfo = "Continue[0]:" + GlIcropResultContinue.GetValue(0)
                'GlErrStepInfo = "Giveup[0]:" + GlIcropResultGiveup.GetValue(0)

                If (GlIcropResultSuccess.Contains(FollowUpResultData.ACT_STATUS)) Then
                    GlErrStepInfo = "SetXmlFollowUpResult_2_6_1(Success)"
                    xmlFollowUpResult.ActivityResult = "1"
                ElseIf (GlIcropResultContinue.Contains(FollowUpResultData.ACT_STATUS)) Then
                    GlErrStepInfo = "SetXmlFollowUpResult_2_6_2(Continue)"
                    xmlFollowUpResult.ActivityResult = "2"
                ElseIf (GlIcropResultGiveup.Contains(FollowUpResultData.ACT_STATUS)) Then
                    GlErrStepInfo = "SetXmlFollowUpResult_2_6_3(Giveup)"
                    xmlFollowUpResult.ActivityResult = "3"
                End If
            End If
            'GlErrStepInfo = "ActivityResult(Edit)" + xmlFollowUpResult.ActivityResult
            GlErrStepInfo = "SetXmlFollowUpResult_2_7"

            If (FirstSuccessFlg = CstStrFirstSuccess) Then
                xmlFollowUpResult.ActivityResult = "1"
            End If
            GlErrStepInfo = "SetXmlFollowUpResult_2_8"


            'SameDayBookingFlg
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                xmlFollowUpResult.SameDayBookingFlg = ""
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                xmlFollowUpResult.SameDayBookingFlg = "1"
            End If
            GlErrStepInfo = "SetXmlFollowUpResult_2_9"

            If (FirstSuccessFlg = CstStrSalesNormal) Then
                GlErrStepInfo = "SetXmlFollowUpResult_2_10"
                If (xmlFollowUpResult.ActivityResult = "1") Then
                    '$ISSUE-0032_20130303_by_chatchai_Start

                    'Dim resultSelectedSeriesData = From TB_T_PREFER_VCL In SelectedSeriesData
                    ' Where TB_T_PREFER_VCL.SALES_ID = SalesData.SALES_ID And
                    '       TB_T_PREFER_VCL.SALESBKG_ACT_ID = LastActionKeyData.ACT_ID
                    '  Select New With {
                    '    TB_T_PREFER_VCL.MODEL_CD,
                    '    TB_T_PREFER_VCL.GRADE_CD,
                    '    TB_T_PREFER_VCL.BODYCLR_CD,
                    '    TB_T_PREFER_VCL.INTERIORCLR_CD,
                    '    TB_T_PREFER_VCL.SUFFIX_CD
                    '  }
                    GlErrStepInfo = "SetXmlFollowUpResult_2_11"

                    'If (resultSelectedSeriesData.Count() > 0) Then
                    '    SeriesCode = resultSelectedSeriesData.ToList()(0).MODEL_CD.ToString()
                    '    GradeCode = resultSelectedSeriesData.ToList()(0).GRADE_CD.ToString()
                    '    ExteriorColorCode = resultSelectedSeriesData.ToList()(0).BODYCLR_CD.ToString()
                    '    InteriorColorCode = resultSelectedSeriesData.ToList()(0).INTERIORCLR_CD.ToString()
                    '    ModelSuffix = resultSelectedSeriesData.ToList()(0).SUFFIX_CD.ToString()yyy
                    'End If

                    '$ISSUE-0032_20130303_by_chatchai_End
                    GlErrStepInfo = "SetXmlFollowUpResult_2_12"
                End If
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                GlErrStepInfo = "SetXmlFollowUpResult_2_13"
                If (xmlFollowUpResult.ActivityResult = "1") Then
                    '$ISSUE-0032_20130303_by_chatchai_Start

                    'Dim resultSelectedSeriesData = From TB_T_PREFER_VCL In SelectedSeriesData
                    ' Where TB_T_PREFER_VCL.SALES_ID = SalesTempData.SALES_ID And
                    '       TB_T_PREFER_VCL.SALESBKG_ACT_ID = FollowUpActionData.ACT_ID
                    '  Select New With {
                    '    TB_T_PREFER_VCL.MODEL_CD,
                    '    TB_T_PREFER_VCL.GRADE_CD,
                    '    TB_T_PREFER_VCL.BODYCLR_CD,
                    '    TB_T_PREFER_VCL.INTERIORCLR_CD,
                    '    TB_T_PREFER_VCL.SUFFIX_CD
                    '  }

                    GlErrStepInfo = "SetXmlFollowUpResult_2_14"
                    'If (resultSelectedSeriesData.Count() > 0) Then
                    '    SeriesCode = resultSelectedSeriesData.ToList()(0).MODEL_CD.ToString()
                    '    GradeCode = resultSelectedSeriesData.ToList()(0).GRADE_CD.ToString()
                    '    ExteriorColorCode = resultSelectedSeriesData.ToList()(0).BODYCLR_CD.ToString()
                    '    InteriorColorCode = resultSelectedSeriesData.ToList()(0).INTERIORCLR_CD.ToString()
                    '    ModelSuffix = resultSelectedSeriesData.ToList()(0).SUFFIX_CD.ToString()
                    'End If

                    '$ISSUE-0032_20130303_by_chatchai_End
                End If
            End If

            GlErrStepInfo = "SetXmlFollowUpResult_2_15"
            If (SalesData IsNot Nothing) Then
                GlErrStepInfo = "SetXmlFollowUpResult_2_16"
                If (xmlFollowUpResult.ActivityResult = "3") Then
                    'ISSUE-IT-2-1_by_takeda_start_20140301
                    'If (FollowUpResultData.ACT_STATUS = "32" And ActionResultData.ACT_RSLT_ID = FollowUpResultData.RSLT_ID) Then
                    If (FollowUpResultData.ACT_STATUS = "32") Then
                        'ISSUE-IT-2-1_by_takeda_end_20140301
                        '20140312 Fujita Upd Start 
                        'GiveUpReasonCode = ActionResultData.ACT_RSLT_ID
                        'GiveUpReason = ActionResultData.RSLT_CAT_NAME
                        GiveUpReasonCode = FollowUpResultData.RSLT_ID
                        GiveUpReason = SalesData.GIVEUP_REASON
                        '20140312 Fujita Upd End 
                        GiveUpMemo = SalesData.GIVEUP_REASON

                        'GlErrStepInfo = "GiveUpReasonCode:" + GiveUpReasonCode
                        'GlErrStepInfo = "GiveUpReason:" + GiveUpReason
                        'GlErrStepInfo = "GiveUpMemo:" + GiveUpMemo

                        GlErrStepInfo = "SetXmlFollowUpResult_2_17"
                        Dim resultMaker = From TB_T_COMPETITOR_VCL In CompetitorSeriesData
                                          Join TB_M_MODEL In MakerModelData
                                          On TB_T_COMPETITOR_VCL.MODEL_CD Equals TB_M_MODEL.MODEL_CD
                                             Where TB_T_COMPETITOR_VCL.COMP_VCL_SEQ = SalesData.GIVEUP_COMP_VCL_SEQ
                                              Select New With {
                                                  TB_M_MODEL.MAKER_CD,
                                                  TB_M_MODEL.MAKER_NAME,
                                                  TB_T_COMPETITOR_VCL.MODEL_CD,
                                                  TB_M_MODEL.MODEL_NAME
                                              }

                        GlErrStepInfo = "SetXmlFollowUpResult_2_18"
                        If (resultMaker.Count() > 0) Then
                            GiveUpMakerCode = EditLength(resultMaker.ToList()(0).MAKER_CD.ToString(), 1, 10, CstStrTypeString)
                            GiveUpMakerName = resultMaker.ToList()(0).MAKER_NAME.ToString()
                            GiveUpSeriesCode = resultMaker.ToList()(0).MODEL_CD.ToString()
                            If (GiveUpSeriesCode <> "") Then
                                GiveUpSeriesName = resultMaker.ToList()(0).MODEL_NAME.ToString()
                            End If
                        End If


                    End If
                End If
            End If

            GlErrStepInfo = "SetXmlFollowUpResult_2_19"

            If SeriesCode <> "" Then
                xmlFollowUpResult.SeriesCode = SeriesCode
                xmlFollowUpResult.GradeCode = EditLength(GradeCode, 1, 20, CstStrTypeString)
                xmlFollowUpResult.ExteriorColorCode = EditLength(ExteriorColorCode, 1, 6, CstStrTypeString)
                xmlFollowUpResult.InteriorColorCode = EditLength(InteriorColorCode, 1, 7, CstStrTypeString)
                xmlFollowUpResult.ModelSuffix = EditLength(ModelSuffix, 1, 4, CstStrTypeString)
            End If
            '20140312 Fujita Upd Start コード変換
            If (FollowUpResultData IsNot Nothing) Then
                If (FollowUpResultData.ACT_STATUS = "32") Then
                    GlErrStepInfo = "SetXmlFollowUpResult_2_19_1(Get Dms Code Map)(GiveupReasonCode)"
                    Dim changedDlrCdRow As IC3802801DataSet.IC3802801DmsCodeMapRow
                    Dim changedDlrCdData As New IC3802801DataSet.IC3802801DmsCodeMapDataTable
                    changedDlrCdData = IC3802801TableAdapter.GetDmsCd1(CstStrCdType13, GiveUpReasonCode)
                    If changedDlrCdData.Rows.Count <> 0 Then
                        changedDlrCdRow = changedDlrCdData.Rows(0)
                        xmlFollowUpResult.GiveUpReasonCode = changedDlrCdRow.DMS_CD_1
                        xmlFollowUpResult.GiveUpReason = changedDlrCdRow.DMS_CD_2
                        '20140317 Fujita Upd Start
                    Else
                        GlErrStepInfo = "Change GiveUpReasonCode：" + GiveUpReasonCode + " Not Found"
                        Throw New Exception("Change GiveUpReasonCode：" + GiveUpReasonCode + " Not Found")

                        'xmlFollowUpResult.GiveUpReasonCode = " "
                        'xmlFollowUpResult.GiveUpReason = " "
                        '20140317 Fujita Upd End
                        'xmlFollowUpResult.GiveUpReasonCode = GiveUpReasonCode
                        'xmlFollowUpResult.GiveUpReason = GiveUpReason
                    End If
                Else
                    xmlFollowUpResult.GiveUpReasonCode = " "
                    xmlFollowUpResult.GiveUpReason = " "
                End If
            Else
                xmlFollowUpResult.GiveUpReasonCode = " "
                xmlFollowUpResult.GiveUpReason = " "
            End If
            '20140312 Fujita Upd End
            xmlFollowUpResult.GiveUpMemo = GiveUpMemo
            xmlFollowUpResult.GiveUpMakerCode = GiveUpMakerCode
            xmlFollowUpResult.GiveUpMakerName = GiveUpMakerName
            xmlFollowUpResult.GiveUpSeriesCode = GiveUpSeriesCode
            xmlFollowUpResult.GiveUpSeriesName = GiveUpSeriesName
            GlErrStepInfo = "SetXmlFollowUpResult_2_20"

            'CreateDate
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                GlErrStepInfo = "SetXmlFollowUpResult_2_20_1(Sales Normal)"
                'ISSUE-0021_20130218_by_takeda_Start
                'xmlFollowUpResult.CreateDate = ActionResultData.ROW_CREATE_DATETIME
                xmlFollowUpResult.CreateDate = ChangeDefaultDate(FollowUpResultData.ROW_UPDATE_DATETIME, CstStrDefaultDate)
                'ISSUE-0021_20130218_by_takeda_End
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                GlErrStepInfo = "SetXmlFollowUpResult_2_20_2(First Success)"
                'ISSUE-0021_20130218_by_takeda_Start
                'xmlFollowUpResult.CreateDate = EstimateData.CONTRACT_APPROVAL_REQUESTDATE
                xmlFollowUpResult.CreateDate = ChangeDefaultDate(EstimateData.CONTRACT_APPROVAL_REQUESTDATE, CstStrDefaultDate)
                'ISSUE-0021_20130218_by_takeda_End
            End If
            GlErrStepInfo = "SetXmlFollowUpResult_2_21"

            xmlFollowUpResult.DeleteDate = ""

            'takeda_update_start_20140529_ログ情報コメントアウト(一発Success時、FollowUpResultは未設定なので参照不可)
            ''takeda_update_start_20140328
            'GlErrStepInfo="SetXmlFollowUpResult_XmlInfo")
            'GlErrStepInfo="SelectedSeriesNo")
            'GlErrStepInfo=xmlFollowUpResult.SelectedSeriesNo)
            'GlErrStepInfo="FollowUpResultDate")
            'GlErrStepInfo=xmlFollowUpResult.FollowUpResultDate)
            'GlErrStepInfo="FollowedBranchCode")
            'GlErrStepInfo=xmlFollowUpResult.FollowedBranchCode)
            'GlErrStepInfo="FollowedAccount")
            'GlErrStepInfo=xmlFollowUpResult.FollowedAccount)
            'GlErrStepInfo="ActivityResult")
            'GlErrStepInfo=xmlFollowUpResult.ActivityResult)
            'GlErrStepInfo="SameDayBookingFlg")
            'GlErrStepInfo=xmlFollowUpResult.SameDayBookingFlg)
            'GlErrStepInfo="SeriesCode")
            'GlErrStepInfo=xmlFollowUpResult.SeriesCode)
            'GlErrStepInfo="GradeCode")
            'GlErrStepInfo=xmlFollowUpResult.GradeCode)
            'GlErrStepInfo="ExteriorColorCode")
            'GlErrStepInfo=xmlFollowUpResult.ExteriorColorCode)
            'GlErrStepInfo="InteriorColorCode")
            'GlErrStepInfo=xmlFollowUpResult.InteriorColorCode)
            'GlErrStepInfo="ModelSuffix")
            'GlErrStepInfo=xmlFollowUpResult.ModelSuffix)
            'GlErrStepInfo="GiveUpReasonCode")
            'GlErrStepInfo=xmlFollowUpResult.GiveUpReasonCode)
            'GlErrStepInfo="GiveUpReason")
            'GlErrStepInfo=xmlFollowUpResult.GiveUpReason)
            'GlErrStepInfo="GiveUpMemo")
            'GlErrStepInfo=xmlFollowUpResult.GiveUpMemo)
            'GlErrStepInfo="GiveUpMakerCode")
            'GlErrStepInfo=xmlFollowUpResult.GiveUpMakerCode)
            'GlErrStepInfo="GiveUpMakerName")
            'GlErrStepInfo=xmlFollowUpResult.GiveUpMakerName)
            'GlErrStepInfo="GiveUpSeriesCode")
            'GlErrStepInfo=xmlFollowUpResult.GiveUpSeriesCode)
            'GlErrStepInfo="GiveUpSeriesName")
            'GlErrStepInfo=xmlFollowUpResult.GiveUpSeriesName)
            ''takeda_update_end_20140328
            'takeda_update_end_20140529_ログ情報コメントアウト(一発Success時、FollowUpResultは未設定なので参照不可)

            GlErrStepInfo = "SetXmlFollowUpResult_2_End"
            Return (xmlFollowUpResult)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlVehicle(ByVal VehicleData As IC3802801VehicleDataTable, ByVal VehicleMakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable) As Collection(Of XmlVehicle)
        GlErrStepInfo = "SetXmlVehicle_2_Start"
        Dim CounterSeqNo = 0
        Dim vehicleDataTableDisplay As New VehicleDataTable
        Dim listVehicle As New Collection(Of XmlVehicle)
        '20140317 Fujita Upd Start
        Try
            For Each vehicleRowItem As IC3802801VehicleRow In VehicleData
                CounterSeqNo = CounterSeqNo + 1
                Dim vehicleRowDisplay As VehicleRow
                vehicleRowDisplay = CType(vehicleDataTableDisplay.NewRow(), IC3802801DataSet.VehicleRow)
                vehicleRowDisplay.VehicleSeqNo = CounterSeqNo.ToString()
                vehicleRowDisplay.SeriesCode = vehicleRowItem.MODEL_CD

                'SeriesName
                Dim SeriesName As String = ""
                Dim resultSeriesName = From VehicleMakerModel In VehicleMakerModelData
                Where VehicleMakerModel.MODEL_CD = vehicleRowItem.MODEL_CD
                  Select New With {
                    VehicleMakerModel.MODEL_NAME
                  }

                If (resultSeriesName.Count() > 0) Then
                    SeriesName = resultSeriesName.ToList()(0).MODEL_NAME.ToString()
                Else
                    SeriesName = ""
                End If

                GlErrStepInfo = "SetXmlVehicle_2_1"
                vehicleRowDisplay.SeriesName = SeriesName
                vehicleRowDisplay.Vin = vehicleRowItem.VCL_VIN
                vehicleRowDisplay.VehicleRegistrationNumber = vehicleRowItem.REG_NUM
                'GlErrStepInfo = "DELI_DATE:" + vehicleRowItem.DELI_DATE.ToString()
                '20140320 Fujita Upd Start
                'vehicleRowDisplay.VehicleDeliveryDate = changeDefaultDate(vehicleRowItem.DELI_DATE, CstStrDefaultDate)
                vehicleRowDisplay.VehicleDeliveryDate = ChangeDefaultDate(vehicleRowItem.DELI_DATE, Date.Parse(CstStrDefaultDate.ToString("yyyy") & "/" & CstStrDefaultDate.ToString("MM") & "/" & CstStrDefaultDate.ToString("dd")))
                '20140320 Fujita Upd End

                '$26 start
                Dim xmlVehicle = SetXmlVehicle(vehicleRowDisplay)

                Dim vehicleDlrData As IC3802801VehicleDlrLocalDataTable =
                CType(IC3802801TableAdapter.GetVehicleDlrLocal(StaffContext.Current.DlrCD.ToString,
                                                          CLng(vehicleRowItem.VCL_ID)), IC3802801VehicleDlrLocalDataTable)
                If (vehicleDlrData.Rows.Count > 0) Then
                    Dim vehicleDlrRow As IC3802801VehicleDlrLocalRow =
                    CType(vehicleDlrData.Rows(0), IC3802801VehicleDlrLocalRow)

                    xmlVehicle.VehicleMile = vehicleDlrRow.VCL_MILE
                    xmlVehicle.VehicleModelYear = vehicleDlrRow.MODEL_YEAR

                End If
                listVehicle.Add(xmlVehicle)
                '$26 end

            Next
            GlErrStepInfo = "SetXmlVehicle_2_End"
            Return listVehicle
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlCustomer(ByVal IcropDealerCode As String,
                                          ByVal SalesData As IC3802801DataSet.IC3802801SalesRow,
                                          ByVal CustomerData As IC3802801DataSet.IC3802801CustomerRow,
                                          ByVal DlrCustomerMemoData As IC3802801DataSet.IC3802801CustomerMemoRow,
                                          ByVal TransmissionDate As Date,
                                          ByVal DlrCstVclData As IC3802801DataSet.IC3802801DlrCstVclDataTable,
                                          ByVal ContactTimeslotData As IC3802801DataSet.IC3802801ContactTimeslotRow,
                                          ByVal StateInfoData As IC3802801DataSet.IC3802801StateInfoRow,
                                          ByVal DistrictInfoData As IC3802801DataSet.IC3802801DistrictInfoRow,
                                          ByVal CityInfoData As IC3802801DataSet.IC3802801CityInfoRow,
                                          ByVal LocationInfoData As IC3802801DataSet.IC3802801LocationInfoRow
                                          ) As XmlCustomer
        GlErrStepInfo = "SetXmlCustomer_2_Start"
        Dim xmlCustomer As New XmlCustomer
        Dim stateName As String = StateInfoData.STATE_NAME
        Dim districtName As String = DistrictInfoData.DISTRICT_NAME
        Dim cityName As String = CityInfoData.CITY_NAME
        Dim locationName As String = LocationInfoData.LOCATION_NAME

        Dim display = New CustomerDataTable
        Dim customerRow = display.NewRow()

        '20140317 Fujita Upd Start
        Try
            customerRow(display.CustomerIDColumn.ToString()) = CustomerData.CST_ID.ToString()
            'ISSUE-0022_20130218_by_takeda_Start
            customerRow(display.SeqNoColumn.ToString()) = TransmissionDate.ToString("yyyy") & TransmissionDate.ToString("MM") & TransmissionDate.ToString("dd") & TransmissionDate.ToString("HH") & TransmissionDate.ToString("mm") & TransmissionDate.ToString("ss")
            'ISSUE-0022_20130218_by_takeda_End
            customerRow(display.CustomerSegmentColumn.ToString()) = CustomerData.CST_TYPE.ToString()
            customerRow(display.NewcustomerIDColumn.ToString()) = CustomerData.NEWCST_CD.ToString()
            'takeda_update_start_20140612_自社客コードの編集方法変更
            'customerRow(display.CustomerCodeColumn.ToString()) = editLength(CustomerData.DMS_CST_CD_DISP.ToString(), 1, 18, CstStrTypeString)
            customerRow(display.CustomerCodeColumn.ToString()) = EditLength(CustomerData.DMS_CST_CD.ToString(), 1, 18, CstStrTypeString)
            'takeda_update_end_20140612_自社客コードの編集方法変更
            customerRow(display.EnquiryCustomerCodeColumn.ToString()) = CustomerData.DMS_NEWCST_CD_DISP.ToString()
            If (DlrCstVclData.ToList().Count > 0) Then
                customerRow(display.SalesStaffCodeColumn.ToString()) = DlrCstVclData.ToList()(0).SLS_PIC_STF_CD
            Else
                customerRow(display.SalesStaffCodeColumn.ToString()) = ""
            End If

            GlErrStepInfo = "SetXmlCustomer_2_1"

            '(ここではCustomerTypeの反転をしない)
            customerRow(display.CustomerTypeColumn.ToString()) = CustomerData.FLEET_FLG.ToString()
            customerRow(display.SubCustomerTypeColumn.ToString()) = CustomerData.PRIVATE_FLEET_ITEM_CD.ToString()
            customerRow(display.SocialIDColumn.ToString()) = CustomerData.CST_SOCIALNUM.ToString()
            customerRow(display.SexColumn.ToString()) = CustomerData.CST_GENDER.ToString()
            customerRow(display.BirthDayColumn.ToString()) = ChangeDefaultDate(CDate(CustomerData.CST_BIRTH_DATE), CstStrDefaultDate)
            customerRow(display.NameTitleCodeColumn.ToString()) = CustomerData.NAMETITLE_CD.ToString()
            customerRow(display.NameTitleColumn.ToString()) = CustomerData.NAMETITLE_NAME.ToString()
            customerRow(display.Name1Column.ToString()) = CustomerData.FIRST_NAME.ToString()
            customerRow(display.Name2Column.ToString()) = CustomerData.MIDDLE_NAME.ToString()
            customerRow(display.Name3Column.ToString()) = CustomerData.LAST_NAME.ToString()
            customerRow(display.SubName1Column.ToString()) = CustomerData.NICK_NAME.ToString()
            customerRow(display.CompanyNameColumn.ToString()) = EditLength(CustomerData.CST_COMPANY_NAME.ToString(), 1, 128, CstStrTypeString)
            customerRow(display.EmployeeNameColumn.ToString()) = CustomerData.FLEET_PIC_NAME.ToString()
            customerRow(display.EmployeeDepartmentColumn.ToString()) = CustomerData.FLEET_PIC_DEPT.ToString()
            customerRow(display.EmployeePositionColumn.ToString()) = CustomerData.FLEET_PIC_POSITION.ToString()
            customerRow(display.AddressColumn.ToString()) = CustomerData.CST_ADDRESS.ToString()
            customerRow(display.Address1Column.ToString()) = CustomerData.CST_ADDRESS_1.ToString()
            'takeda_update_start_20140425
            'GlErrStepInfo = "@@@DataCheck(SetXmlCustomer_2)"
            'GlErrStepInfo = "(XML)customerRow(display.AddressColumn.ToString()):" + customerRow(display.AddressColumn.ToString()).ToString()
            'GlErrStepInfo = "(XML)customerRow(display.Address1Column.ToString()):" + customerRow(display.Address1Column.ToString()).ToString()
            'takeda_update_end_20140425

            GlErrStepInfo = "SetXmlCustomer_2_2"

            customerRow(display.Address2Column.ToString()) = CustomerData.CST_ADDRESS_2.ToString()
            customerRow(display.Address3Column.ToString()) = CustomerData.CST_ADDRESS_3.ToString()
            customerRow(display.DomicileColumn.ToString()) = CustomerData.CST_DOMICILE.ToString()
            customerRow(display.CountryColumn.ToString()) = CustomerData.CST_COUNTRY.ToString()
            customerRow(display.ZipCodeColumn.ToString()) = CustomerData.CST_ZIPCD.ToString()
            customerRow(display.StateCodeColumn.ToString()) = EditLength(CustomerData.CST_ADDRESS_STATE.ToString(), 1, 5, CstStrTypeString)

            'If (CustomerAddressData IsNot Nothing) Then
            '    stateName = CustomerAddressData.STATE_NAME
            '    districtName = CustomerAddressData.DISTRICT_NAME
            '    cityName = CustomerAddressData.CITY_NAME
            '    locationName = CustomerAddressData.LOCATION_NAME
            'Else
            '    stateName = ""
            '    districtName = ""
            '    cityName = ""
            '    locationName = ""
            'End If

            GlErrStepInfo = "SetXmlCustomer_2_3"

            customerRow(display.StateNameColumn.ToString()) = stateName
            customerRow(display.DistrictCodeColumn.ToString()) = CustomerData.CST_ADDRESS_DISTRICT.ToString()
            customerRow(display.DistrictNameColumn.ToString()) = districtName
            customerRow(display.CityCodeColumn.ToString()) = CustomerData.CST_ADDRESS_CITY.ToString()
            customerRow(display.CityNameColumn.ToString()) = cityName
            customerRow(display.LocationCodeColumn.ToString()) = CustomerData.CST_ADDRESS_LOCATION.ToString()
            customerRow(display.LocationNameColumn.ToString()) = locationName

            customerRow(display.TelNumberColumn.ToString()) = CustomerData.CST_PHONE.ToString()
            customerRow(display.FaxNumberColumn.ToString()) = CustomerData.CST_FAX.ToString()
            customerRow(display.MobileColumn.ToString()) = CustomerData.CST_MOBILE.ToString()
            customerRow(display.EMail1Column.ToString()) = CustomerData.CST_EMAIL_1.ToString()
            customerRow(display.EMail2Column.ToString()) = CustomerData.CST_EMAIL_2.ToString()
            customerRow(display.BusinessTelNumberColumn.ToString()) = CustomerData.CST_BIZ_PHONE.ToString()
            customerRow(display.IncomeColumn.ToString()) = CustomerData.CST_INCOME.ToString()

            GlErrStepInfo = "SetXmlCustomer_2_4"

            If (ContactTimeslotData IsNot Nothing) Then
                customerRow(display.ContactTimeColumn.ToString()) = ContactTimeslotData.CONTACT_TIMESLOT
            End If

            If (DlrCustomerMemoData IsNot Nothing) Then
                customerRow(display.CustomerMemoColumn.ToString()) = DlrCustomerMemoData.CST_MEMO
            Else
                customerRow(display.CustomerMemoColumn.ToString()) = ""
            End If

            '20140312 Fujita Upd Start
            Dim changedDlrCdRow As IC3802801DataSet.IC3802801DmsCodeMapRow
            Dim changedDlrCdData As New IC3802801DataSet.IC3802801DmsCodeMapDataTable
            GlErrStepInfo = "SetXmlCustomer_2_5(Get Dms Code Map)(Occupation ID)"
            changedDlrCdData = IC3802801TableAdapter.GetDmsCd1(CstStrCdType12, CustomerData.CST_OCCUPATION_ID.ToString())
            If changedDlrCdData.Rows.Count <> 0 Then
                changedDlrCdRow = changedDlrCdData.Rows(0)
                'customerRow(display.OccupationIDColumn.ToString()) = CustomerData.CST_OCCUPATION_ID.ToString()
                'customerRow(display.OccupationColumn.ToString()) = CustomerData.CST_OCCUPATION.ToString()
                customerRow(display.OccupationIDColumn.ToString()) = changedDlrCdRow.DMS_CD_1
                customerRow(display.OccupationColumn.ToString()) = changedDlrCdRow.DMS_CD_2
                '20140317 Fujita Upd Start
            Else
                If CustomerData.CST_OCCUPATION_ID <> "0" Then
                    GlErrStepInfo = "Change CST_OCCUPATION_ID：" + CustomerData.CST_OCCUPATION_ID.ToString() + " Not Found"
                    Throw New Exception("Change CST_OCCUPATION_ID：" + CustomerData.CST_OCCUPATION_ID.ToString() + " Not Found")
                Else
                    customerRow(display.OccupationIDColumn.ToString()) = " "
                    customerRow(display.OccupationColumn.ToString()) = " "
                End If
                '20140317 Fujita Upd End
            End If
            '20140312 Fujita Upd End
            GlErrStepInfo = "SetXmlCustomer_2_6"
            customerRow(display.DefaultLangColumn.ToString()) = CustomerData.DEFAULT_LANG.ToString()

            customerRow(display.CreateDateColumn.ToString()) = ChangeDefaultDate(CDate(CustomerData.ROW_CREATE_DATETIME), CstStrDefaultDate)
            customerRow(display.UpdateDateColumn.ToString()) = ChangeDefaultDate(CDate(CustomerData.ROW_UPDATE_DATETIME), CstStrDefaultDate)
            customerRow(display.DeleteDateColumn.ToString()) = ""
            xmlCustomer = SetXmlCustomer(CType(customerRow, CustomerRow))
            GlErrStepInfo = "SetXmlCustomer_2_End"

            '$26 start
            Dim customerLocalData As IC3802801CustomerLocalDataTable =
                    CType(IC3802801TableAdapter.GetCustomerLocal(CLng(CustomerData.CST_ID)), IC3802801CustomerLocalDataTable)
            If (customerLocalData.Rows.Count > 0) Then
                Dim customerLocalRow As IC3802801CustomerLocalRow =
                CType(customerLocalData.Rows(0), IC3802801CustomerLocalRow)

                xmlCustomer.SubCustomerType2 = customerLocalRow.CST_SUBCAT2_CD
                If (IsDBNull(customerLocalRow.Item("CST_ORGNZ_NAME")) = False) Then
                    xmlCustomer.OrganizationName = customerLocalRow.CST_ORGNZ_NAME
                End If
            End If
            '$26 end

            Return xmlCustomer
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Dim SALES_ACT_STARTTIME As String = ""
    Dim SALES_ACT_ENDTIME As String = ""

    '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
    'Public Function SetXmlFollowUp(ByVal IcropDealerCode As String, ByVal IcropBranchCode As String, ByVal SalesId As String,
    '                                      ByVal SelectedSeriesData As IC3802801DataSet.IC3802801SelectedSeriesDataTable,
    '                                      ByVal TransmissionDate As Date,
    '                                      ByVal SalesData As IC3802801DataSet.IC3802801SalesRow,
    '                                      ByVal SalesTempData As IC3802801DataSet.IC3802801SalesTempRow,
    '                                      ByVal RequestData As IC3802801DataSet.IC3802801FollowUpRequestRow,
    '                                      ByVal AttractData As IC3802801DataSet.IC3802801FollowUpAttractRow,
    '                                      ByVal CompetitorSeriesData As IC3802801DataSet.IC3802801CompetitorSeriesDataTable,
    '                                      ByVal SalesConditionData As IC3802801DataSet.IC3802801SalesConditionDataTable,
    '                                      ByVal ReqSrcData1 As IC3802801DataSet.IC3802801ReqSource1Row,
    '                                      ByVal ReqSrcData2 As IC3802801DataSet.IC3802801ReqSource2Row,
    '                                      ByVal FollowUpResultData As IC3802801DataSet.IC3802801ActionRow,
    '                                      ByVal FollowUpActionData As IC3802801DataSet.IC3802801ActionRow,
    '                                      ByVal ActionData As IC3802801DataSet.IC3802801ActionDataTable,
    '                                      ByVal NegotiationMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
    '                                      ByVal FllwUpBoxSalesData As IC3802801DataSet.IC3802801GetFllwUpBoxSalesDataTable,
    '                                      ByVal SalesActData As IC3802801DataSet.IC3802801SalesActionDataTable,
    '                                      ByVal MakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
    '                                      ByVal EstimateData As IC3802801DataSet.IC3802801EstimateInfoRow,
    '                                      ByVal VehicleData As IC3802801DataSet.IC3802801VehicleDataTable,
    '                                      ByVal ActionMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
    '                                      ByVal ActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable,
    '                                      ByVal FirstSuccessFlg As String) As XmlFollowUp
    Public Function SetXmlFollowUp(ByVal IcropDealerCode As String, ByVal IcropBranchCode As String, ByVal SalesId As String,
                                      ByVal SelectedSeriesData As IC3802801DataSet.IC3802801SelectedSeriesDataTable,
                                      ByVal TransmissionDate As Date,
                                      ByVal SalesData As IC3802801DataSet.IC3802801SalesRow,
                                      ByVal SalesTempData As IC3802801DataSet.IC3802801SalesTempRow,
                                      ByVal RequestData As IC3802801DataSet.IC3802801FollowUpRequestRow,
                                      ByVal AttractData As IC3802801DataSet.IC3802801FollowUpAttractRow,
                                      ByVal CompetitorSeriesData As IC3802801DataSet.IC3802801CompetitorSeriesDataTable,
                                      ByVal SalesConditionData As IC3802801DataSet.IC3802801SalesConditionDataTable,
                                      ByVal ReqSrcData1 As IC3802801DataSet.IC3802801ReqSource1Row,
                                      ByVal ReqSrcData2 As IC3802801DataSet.IC3802801ReqSource2Row,
                                      ByVal FollowUpResultData As IC3802801DataSet.IC3802801ActionRow,
                                      ByVal FollowUpActionData As IC3802801DataSet.IC3802801ActionRow,
                                      ByVal ActionData As IC3802801DataSet.IC3802801ActionDataTable,
                                      ByVal NegotiationMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
                                      ByVal FllwUpBoxSalesData As IC3802801DataSet.IC3802801GetFllwUpBoxSalesDataTable,
                                      ByVal SalesActData As IC3802801DataSet.IC3802801SalesActionDataTable,
                                      ByVal MakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
                                      ByVal EstimateData As IC3802801DataSet.IC3802801EstimateInfoRow,
                                      ByVal VehicleData As IC3802801DataSet.IC3802801VehicleDataTable,
                                      ByVal ActionMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
                                      ByVal ActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable,
                                      ByVal FirstSuccessFlg As String,
                                      ByVal SalesLocalData As IC3802801DataSet.IC3802801SalesLocalRow) As XmlFollowUp
        '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end
        '20140317 Fujita Upd Start
        Try
            GlErrStepInfo = "SetXmlFollowUp_Start"

            Dim xmlFollowUp As New XmlFollowUp
            'ISSUE-0009_20130218_by_takeda_Start
            xmlFollowUp.SeqNo = TransmissionDate.ToString("yyyy") & TransmissionDate.ToString("MM") & TransmissionDate.ToString("dd") & TransmissionDate.ToString("HH") & TransmissionDate.ToString("mm") & TransmissionDate.ToString("ss")
            'ISSUE-0009_20130218_by_takeda_End
            If (SalesData IsNot Nothing) Then
                xmlFollowUp.FollowUpID = SalesData.ACARD_NUM
            End If

            GlErrStepInfo = "SetXmlFollowUp_1"
            xmlFollowUp.FollowUpNo = EditLength(SalesId, 1, 10, CstStrTypeString)

            'ParentFollowUpNo
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                xmlFollowUp.ParentFollowUpNo = EditLength(SalesData.ORIGIN_SALES_ID, 1, 10, CstStrTypeString)
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                xmlFollowUp.ParentFollowUpNo = ""
            End If

            GlErrStepInfo = "SetXmlFollowUp_2"
            xmlFollowUp.PreFollowUpNo = ""

            'FollowUpDate
            Dim FollowUpDate As String = ""
            Dim ResultRSLT_DATETIME As DateTime = Nothing
            'ISSUE-0010_20130218_by_takeda_Start
            Dim DefaultSalesStartDate As New DateTime(1900, 1, 1)
            'ISSUE-0010_20130218_by_takeda_End
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                If (SalesData.REQ_ID <> 0) Then
                    GlErrStepInfo = "SetXmlFollowUp_3"
                    Dim QueryReq = From Action In ActionData
                                 Where Action.REQ_ID = FollowUpResultData.REQ_ID And Action.RSLT_SALES_PROSPECT_CD <> ""
                                 Order By Action.ACT_COUNT Ascending
                      Select New With {Action.RSLT_DATETIME, Action.ACT_COUNT}
                    'Set Data
                    If (QueryReq.ToList().Count > 0) Then
                        ResultRSLT_DATETIME = CDate(QueryReq.ToList()(0).RSLT_DATETIME.ToString())
                    End If
                ElseIf (SalesData.ATT_ID <> 0) Then
                    GlErrStepInfo = "SetXmlFollowUp_4"
                    Dim QueryAtt = From Action In ActionData
                                Where Action.ATT_ID = SalesData.ATT_ID And Action.RSLT_SALES_PROSPECT_CD <> ""
                                Order By Action.ACT_COUNT Ascending
                      Select New With {Action.RSLT_DATETIME}
                    'Set Data
                    If (QueryAtt.ToList().Count > 0) Then
                        ResultRSLT_DATETIME = CDate(QueryAtt.ToList()(0).RSLT_DATETIME.ToString())
                    End If
                End If

                GlErrStepInfo = "SetXmlFollowUp_5"
                'ISSUE-0010_20130218_by_takeda_Start
                If (SalesData.SALES_START_DATE.Equals(DefaultSalesStartDate)) Then   'check (Default Date) 
                    SalesData.SALES_START_DATE = Nothing
                End If
                'ISSUE-0010_20130218_by_takeda_End
                If (ResultRSLT_DATETIME <> Nothing And SalesData.SALES_START_DATE <> Nothing) Then
                    GlErrStepInfo = "SetXmlFollowUp_6"
                    If (ResultRSLT_DATETIME < SalesData.SALES_START_DATE) Then
                        FollowUpDate = ChangeDefaultDate(ResultRSLT_DATETIME, CstStrDefaultDate)
                    Else
                        FollowUpDate = ChangeDefaultDate(SalesData.SALES_START_DATE, CstStrDefaultDate)
                    End If
                Else
                    GlErrStepInfo = "SetXmlFollowUp_7"
                    If (ResultRSLT_DATETIME <> Nothing) Then
                        FollowUpDate = ChangeDefaultDate(ResultRSLT_DATETIME, CstStrDefaultDate)
                    ElseIf (SalesData.SALES_START_DATE <> Nothing) Then
                        FollowUpDate = ChangeDefaultDate(SalesData.SALES_START_DATE, CstStrDefaultDate)
                    Else
                        FollowUpDate = ""
                    End If
                End If
                GlErrStepInfo = "SetXmlFollowUp_8"
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                FollowUpDate = ChangeDefaultDate(EstimateData.CONTRACT_APPROVAL_REQUESTDATE, CstStrDefaultDate)
            End If

            GlErrStepInfo = "SetXmlFollowUp_9"

            xmlFollowUp.FollowUpDate = FollowUpDate
            xmlFollowUp.PreFollowUpCreateDate = ""

            'DemandStructure
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                xmlFollowUp.DemandStructure = SalesData.DEMAND_STRUCTURE
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                xmlFollowUp.DemandStructure = ""
            End If
            GlErrStepInfo = "SetXmlFollowUp_10"

            'DirectBillingFlg
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                'GlErrStepInfo = "DirectBillingFlg(SalesData):" + SalesData.DIRECT_SALES_FLG
                'takeda_update_start_20140530
                'xmlFollowUp.DirectBillingFlg = SalesData.DIRECT_SALES_FLG

                'DB初期値の場合、0に変換して設定
                If (SalesData.DIRECT_SALES_FLG.Trim() <> "") Then
                    GlErrStepInfo = "SetXmlFollowUp_10_1"
                    xmlFollowUp.DirectBillingFlg = SalesData.DIRECT_SALES_FLG
                Else
                    GlErrStepInfo = "SetXmlFollowUp_10_2"
                    xmlFollowUp.DirectBillingFlg = "0"
                End If
                'takeda_update_end_20140530
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                xmlFollowUp.DirectBillingFlg = "0"
            End If
            GlErrStepInfo = "SetXmlFollowUp_11"

            'FirstContactType
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                xmlFollowUp.FirstContactType = SalesData.BRAND_RECOGNITION_ID
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                xmlFollowUp.FirstContactType = SalesTempData.BRAND_RECOGNITION_ID
            End If
            GlErrStepInfo = "SetXmlFollowUp_12"


            'takeda_update_start_20140607
            Dim changedDlrCdRow As IC3802801DataSet.IC3802801DmsCodeMapRow
            Dim changedDlrCdData As New IC3802801DataSet.IC3802801DmsCodeMapDataTable
            'takeda_update_end_20140607

            'SourceID1
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                If (RequestData.REQ_ID <> 0) Then
                    'takeda_update_start_20140607
                    'xmlFollowUp.SourceID1 = RequestData.SOURCE_1_CD
                    'takeda_update_start_20140708
                    If (RequestData.SOURCE_1_CD <> "") Then
                        If (CLng(RequestData.SOURCE_1_CD) = 0) Then
                            '用件ソース(1st)コードがDB初期値(0)の場合、変換は行わずタグのみとする
                            xmlFollowUp.SourceID1 = ""
                        Else
                            'takeda_update_end_20140708
                            changedDlrCdData = IC3802801TableAdapter.GetDmsCd1(CstStrCdType14, RequestData.SOURCE_1_CD)
                            If changedDlrCdData.Rows.Count <> 0 Then
                                GlErrStepInfo = "SetXmlFollowUp_12_1"
                                '基幹コードマップに存在する場合、変換した結果を設定
                                changedDlrCdRow = changedDlrCdData.Rows(0)
                                xmlFollowUp.SourceID1 = changedDlrCdRow.DMS_CD_1
                            Else
                                GlErrStepInfo = "SetXmlFollowUp_12_2"
                                '基幹コードマップに存在しない場合、変換前の結果をそのまま設定
                                xmlFollowUp.SourceID1 = RequestData.SOURCE_1_CD
                            End If
                            'takeda_update_start_20140708
                        End If
                    End If
                    'takeda_update_end_20140708
                    'takeda_update_end_20140607
                ElseIf (AttractData.ATT_ID <> 0) Then
                    'takeda_update_start_20140607
                    'xmlFollowUp.SourceID1 = AttractData.SOURCE_1_CD
                    'takeda_update_start_20140708
                    If (AttractData.SOURCE_1_CD <> "") Then
                        If (CLng(AttractData.SOURCE_1_CD) = 0) Then
                            '用件ソース(1st)コードがDB初期値(0)の場合、変換は行わずタグのみとする
                            xmlFollowUp.SourceID1 = ""
                        Else
                            'takeda_update_end_20140708
                            changedDlrCdData = IC3802801TableAdapter.GetDmsCd1(CstStrCdType14, AttractData.SOURCE_1_CD)
                            If changedDlrCdData.Rows.Count <> 0 Then
                                GlErrStepInfo = "SetXmlFollowUp_12_3"
                                '基幹コードマップに存在する場合、変換した結果を設定
                                changedDlrCdRow = changedDlrCdData.Rows(0)
                                xmlFollowUp.SourceID1 = changedDlrCdRow.DMS_CD_1
                            Else
                                GlErrStepInfo = "SetXmlFollowUp_12_4"
                                '基幹コードマップに存在しない場合、変換前の結果をそのまま設定
                                xmlFollowUp.SourceID1 = AttractData.SOURCE_1_CD
                            End If
                            'takeda_update_start_20140708
                        End If
                    End If
                    'takeda_update_end_20140708
                    'takeda_update_end_20140607
                End If

            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                'takeda_update_start_20140607
                'xmlFollowUp.SourceID1 = SalesTempData.SOURCE_1_CD
                'takeda_update_start_20140708
                If (SalesTempData.SOURCE_1_CD <> "") Then
                    If (CLng(SalesTempData.SOURCE_1_CD) = 0) Then
                        '用件ソース(1st)コードがDB初期値(0)の場合、変換は行わずタグのみとする
                        xmlFollowUp.SourceID1 = ""
                    Else
                        'takeda_update_end_20140708
                        changedDlrCdData = IC3802801TableAdapter.GetDmsCd1(CstStrCdType14, SalesTempData.SOURCE_1_CD)
                        If changedDlrCdData.Rows.Count <> 0 Then
                            GlErrStepInfo = "SetXmlFollowUp_12_5"
                            '基幹コードマップに存在する場合、変換した結果を設定
                            changedDlrCdRow = changedDlrCdData.Rows(0)
                            xmlFollowUp.SourceID1 = changedDlrCdRow.DMS_CD_1
                        Else
                            GlErrStepInfo = "SetXmlFollowUp_12_6"
                            '基幹コードマップに存在しない場合、変換前の結果をそのまま設定
                            xmlFollowUp.SourceID1 = SalesTempData.SOURCE_1_CD
                        End If
                        'takeda_update_start_20140708
                    End If
                End If
                'takeda_update_end_20140708
                'takeda_update_end_20140607
            End If
            GlErrStepInfo = "SetXmlFollowUp_13"

            'SourceID2
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                If (RequestData.REQ_ID <> 0) Then
                    xmlFollowUp.SourceID2 = RequestData.SOURCE_2_CD
                ElseIf (AttractData.ATT_ID <> 0) Then
                    xmlFollowUp.SourceID2 = AttractData.SOURCE_2_CD
                End If
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
                'xmlFollowUp.SourceID2 = ""
                xmlFollowUp.SourceID2 = SalesLocalData.SOURCE_2_CD
                '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end
            End If
            GlErrStepInfo = "SetXmlFollowUp_13"

            'SourceName1
            'takeda_update_start_20140609
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                If (RequestData.SOURCE_1_CD.Trim <> "") Then
                    xmlFollowUp.SourceName1 = ReqSrcData1.SOURCE_1_NAME
                Else
                    xmlFollowUp.SourceName1 = ""
                End If
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                '一発Success時
                If (ReqSrcData1.SOURCE_1_CD.Trim <> "") Then
                    xmlFollowUp.SourceName1 = ReqSrcData1.SOURCE_1_NAME
                Else
                    xmlFollowUp.SourceName1 = ""
                End If
            End If
            'takeda_update_end_20140609

            'SourceName2
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                If (RequestData.SOURCE_1_CD.Trim <> "") And (RequestData.SOURCE_2_CD.Trim() <> "") Then
                    'takeda_update_start_20140609_ソース名称１の設定はここで行わない
                    'xmlFollowUp.SourceName1 = ReqSrcData1.SOURCE_1_NAME
                    'takeda_update_end_20140609
                    xmlFollowUp.SourceName2 = ReqSrcData2.REQ_SECOND_CAT_NAME
                Else
                    'takeda_update_start_20140609_ソース名称１の設定はここで行わない
                    'xmlFollowUp.SourceName1 = ""
                    'takeda_update_end_20140609
                    xmlFollowUp.SourceName2 = ""
                End If
                'takeda_update_start_20140609_一発Success時のソース名称１の設定はここで行わない
                'ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                '    '20140319 Fujita Upd Start
                '    If (ReqSrcData1.SOURCE_1_CD.Trim <> "") Then
                '        xmlFollowUp.SourceName1 = ReqSrcData1.SOURCE_1_NAME
                '    Else
                '        xmlFollowUp.SourceName1 = ""
                '    End If
                '20140319 Fujita Upd End
                'takeda_update_end_20140609_一発Success時のソース名称１の設定はここで行わない
                '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
            Else
                If (SalesTempData.SOURCE_1_CD.Trim <> "") And (SalesLocalData.SOURCE_2_CD.Trim() <> "") Then
                    xmlFollowUp.SourceName2 = ReqSrcData2.REQ_SECOND_CAT_NAME
                Else
                    xmlFollowUp.SourceName2 = ""
                End If
                '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end
            End If
            GlErrStepInfo = "SetXmlFollowUp_14"

            'PotentialDivision
            Dim PotentialDivision As String = ""
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                'ISSUE-0011_20130218_by_takeda_Start
                'If (SalesData.SALES_PROSPECT_CD = GlDmsStatusCold) Then
                '    PotentialDivision = "1"
                'ElseIf (SalesData.SALES_PROSPECT_CD = GlDmsStatusWarm) Then
                '    PotentialDivision = "2"
                'ElseIf (SalesData.SALES_PROSPECT_CD = GlDmsStatusHot) Then
                '    PotentialDivision = "3"
                If (GlIcropStatusCold.Contains(SalesData.SALES_PROSPECT_CD)) Then
                    PotentialDivision = "1"
                ElseIf (GlIcropStatusWarm.Contains(SalesData.SALES_PROSPECT_CD)) Then
                    PotentialDivision = "2"
                ElseIf (GlIcropStatusHot.Contains(SalesData.SALES_PROSPECT_CD)) Then
                    PotentialDivision = "3"
                    'ISSUE-0011_20130218_by_takeda_End
                ElseIf (SalesData.SALES_PROSPECT_CD.Trim() = "") Then
                    PotentialDivision = "0"
                End If

            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                PotentialDivision = GlInitProspectStatus
            End If
            xmlFollowUp.PotentialDivision = PotentialDivision
            GlErrStepInfo = "SetXmlFollowUp_15"

            'Vin
            Dim Vin As String = ""
            If (xmlFollowUp.PotentialDivision = "0") Then
                Dim result = From Vehicle In VehicleData
                                Where Vehicle.VCL_ID = AttractData.VCL_ID
                    Select New With {
                        Vehicle.VCL_VIN
                    }

                GlErrStepInfo = "SetXmlFollowUp_16"
                If (result.ToList().Count > 0) Then
                    Vin = result.ToList()(0).VCL_VIN.ToString()
                Else
                    Vin = ""
                End If

            Else
                Vin = ""
            End If
            xmlFollowUp.Vin = Vin

            GlErrStepInfo = "SetXmlFollowUp_17"

            'InterestDate
            If (SalesData IsNot Nothing) Then
                Dim InterestDate As DateTime = Nothing
                If (SalesData.REQ_ID <> 0) Then
                    Dim QueryReq = From Action In ActionData
                                    Where Action.REQ_ID = FollowUpResultData.REQ_ID And GlIcropStatusCold.Contains(Action.RSLT_SALES_PROSPECT_CD)
                                    Order By Action.RSLT_DATETIME Descending
                        Select New With {Action.RSLT_DATETIME, Action.ACT_COUNT}
                    GlErrStepInfo = "SetXmlFollowUp_18"
                    'Set Data
                    If (QueryReq.ToList().Count > 0) Then
                        InterestDate = CDate(QueryReq.ToList()(0).RSLT_DATETIME.ToString())
                    End If
                ElseIf (SalesData.ATT_ID <> 0) Then
                    Dim QueryAtt = From Action In ActionData
                                Where Action.ATT_ID = SalesData.ATT_ID And GlIcropStatusCold.Contains(Action.RSLT_SALES_PROSPECT_CD)
                                Order By Action.RSLT_DATETIME Descending
                        Select New With {Action.RSLT_DATETIME}
                    GlErrStepInfo = "SetXmlFollowUp_19"
                    'Set Data
                    If (QueryAtt.ToList().Count > 0) Then
                        InterestDate = CDate(QueryAtt.ToList()(0).RSLT_DATETIME.ToString())
                    End If
                End If

                GlErrStepInfo = "SetXmlFollowUp_20"
                If (InterestDate = Nothing) Then
                    xmlFollowUp.InterestDate = ""
                Else
                    xmlFollowUp.InterestDate = ChangeDefaultDate(InterestDate, CstStrDefaultDate)
                End If
            End If

            GlErrStepInfo = "SetXmlFollowUp_21"
            'ProspectDate
            If (SalesData IsNot Nothing) Then
                Dim ProspectDate As DateTime = Nothing
                If (SalesData.REQ_ID <> 0) Then
                    Dim QueryReq = From Action In ActionData
                                    Where Action.REQ_ID = FollowUpResultData.REQ_ID And GlIcropStatusWarm.Contains(Action.RSLT_SALES_PROSPECT_CD)
                                    Order By Action.RSLT_DATETIME Descending
                        Select New With {Action.RSLT_DATETIME, Action.ACT_COUNT}
                    GlErrStepInfo = "SetXmlFollowUp_22"
                    'Set Data
                    If (QueryReq.ToList().Count > 0) Then
                        ProspectDate = CDate(QueryReq.ToList()(0).RSLT_DATETIME.ToString())
                    End If
                ElseIf (SalesData.ATT_ID <> 0) Then
                    Dim QueryAtt = From Action In ActionData
                                Where Action.ATT_ID = SalesData.ATT_ID And GlIcropStatusWarm.Contains(Action.RSLT_SALES_PROSPECT_CD)
                                Order By Action.RSLT_DATETIME Descending
                        Select New With {Action.RSLT_DATETIME}
                    GlErrStepInfo = "SetXmlFollowUp_23"
                    'Set Data
                    If (QueryAtt.ToList().Count > 0) Then
                        ProspectDate = CDate(QueryAtt.ToList()(0).RSLT_DATETIME.ToString())
                    End If
                End If

                GlErrStepInfo = "SetXmlFollowUp_24"
                If (ProspectDate = Nothing) Then
                    xmlFollowUp.ProspectDate = ""
                Else
                    xmlFollowUp.ProspectDate = ChangeDefaultDate(ProspectDate, CstStrDefaultDate)
                End If
            End If

            GlErrStepInfo = "SetXmlFollowUp_25"
            'HotDate
            If (SalesData IsNot Nothing) Then
                Dim HotDate As DateTime = Nothing
                If (SalesData.REQ_ID <> 0) Then
                    Dim QueryReq = From Action In ActionData
                                    Where Action.REQ_ID = FollowUpResultData.REQ_ID And GlIcropStatusHot.Contains(Action.RSLT_SALES_PROSPECT_CD)
                                    Order By Action.RSLT_DATETIME Descending
                        Select New With {Action.RSLT_DATETIME, Action.ACT_COUNT}
                    GlErrStepInfo = "SetXmlFollowUp_26"
                    'Set Data
                    If (QueryReq.ToList().Count > 0) Then
                        HotDate = CDate(QueryReq.ToList()(0).RSLT_DATETIME.ToString())
                    End If
                ElseIf (SalesData.ATT_ID <> 0) Then
                    Dim QueryAtt = From Action In ActionData
                                Where Action.ATT_ID = SalesData.ATT_ID And GlIcropStatusHot.Contains(Action.RSLT_SALES_PROSPECT_CD)
                                Order By Action.RSLT_DATETIME Descending
                        Select New With {Action.RSLT_DATETIME}
                    GlErrStepInfo = "SetXmlFollowUp_27"
                    'Set Data
                    If (QueryAtt.ToList().Count > 0) Then
                        HotDate = CDate(QueryAtt.ToList()(0).RSLT_DATETIME.ToString())
                    End If
                End If

                GlErrStepInfo = "SetXmlFollowUp_28"
                If (HotDate = Nothing) Then
                    xmlFollowUp.HotDate = ""
                Else
                    xmlFollowUp.HotDate = ChangeDefaultDate(HotDate, CstStrDefaultDate)
                End If
            End If

            GlErrStepInfo = "SetXmlFollowUp_29"
            xmlFollowUp.ReconsiderDate = ""
            xmlFollowUp.OtherDLRPurchaseFlg = ""
            If (SalesData IsNot Nothing) Then
                xmlFollowUp.SalesTargetDate = ChangeDefaultDate(CDate(SalesData.SALES_TARGET_DATE), CstStrDefaultDate)
            Else
                xmlFollowUp.SalesTargetDate = ""
            End If
            GlErrStepInfo = "SetXmlFollowUp_30"
            xmlFollowUp.PlannedBranchCode = ChangeBranchCd(IcropDealerCode, IcropBranchCode, "DMS")
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                'takeda_update_start_20140412
                'xmlFollowUp.PlannedAccount = FollowUpResultData.SCHE_STF_CD
                xmlFollowUp.PlannedAccount = ConvertStfCd(FollowUpResultData.SCHE_STF_CD, CstStfCdCnvtOff)
                'takeda_update_end_20140412
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                'takeda_update_start_20140412
                'xmlFollowUp.PlannedAccount = FollowUpActionData.SCHE_STF_CD
                xmlFollowUp.PlannedAccount = ConvertStfCd(FollowUpActionData.SCHE_STF_CD, CstStfCdCnvtOff)
                'takeda_update_end_20140412
            End If
            GlErrStepInfo = "SetXmlFollowUp_31"

            Dim listSelectedSeries As New Collection(Of XmlSelectedSeries)
            For Each selectedSeriesRow As IC3802801SelectedSeriesRow In SelectedSeriesData
                listSelectedSeries.Add(SetXmlSelectedSeriesNode(selectedSeriesRow))
            Next
            xmlFollowUp.SelectedSeries = listSelectedSeries
            GlErrStepInfo = "SetXmlFollowUp_32"

            Dim listcompetitorSeries As New Collection(Of XmlCompetitorSeries)
            For Each competitorSeriesRow As IC3802801CompetitorSeriesRow In CompetitorSeriesData
                listcompetitorSeries.Add(SetXmlCompetitorSeriesNode(competitorSeriesRow, MakerModelData))
            Next
            xmlFollowUp.CompetitorSeries = listcompetitorSeries
            GlErrStepInfo = "SetXmlFollowUp_33"

            Dim listAction As New Collection(Of XmlAction)
            SALES_ACT_STARTTIME = ""
            SALES_ACT_ENDTIME = ""
            'takeda_update_start_20140412
            'ActionSeqData.DefaultView.Sort = "RELATION_ACT_SEQ ASC"
            'takeda_update_end_20140412
            '20140318 Upd Start
            Dim intCnt As Long = 0
            For Each ActionSeq As DataRowView In ActionSeqData.DefaultView
                GlErrStepInfo = "SetXmlFollowUp_33_2"
                'GlErrStepInfo = "ActionSeq_sort_check:" + ActionSeq("RELATION_ACT_SEQ").ToString()
                listAction.Add(SetXmlActionNode(ActionSeq, FllwUpBoxSalesData, SalesActData, ActionMemoData, ActionData, intCnt))
                intCnt = intCnt + 1
            Next
            '20140318 Upd End
            xmlFollowUp.Action = listAction
            GlErrStepInfo = "SetXmlFollowUp_34"

            Dim listSalesCondition As New Collection(Of XmlSalesCondition)
            For Each salesConditionRow As IC3802801SalesConditionRow In SalesConditionData
                listSalesCondition.Add(SetXmlSalesConditionNode(salesConditionRow, FollowUpResultData))
            Next

            'takeda_update_start_20140602
            '<SalesCondition>タグ0件の時、タグは作成しない
            ''takeda_update_start_20140412
            'If SalesConditionData.Count = 0 Then
            '    GlErrStepInfo="SetXmlFollowUp_34_2"
            '    'SalesConditionが０件の場合、6明細分の空行を作成
            '    For indexCnt = 1 To maxSalesConditionInfo Step 1
            '        listSalesCondition.Add(SetXmlSalesConditionNode_Empty(indexCnt))
            '    Next
            'End If
            ''takeda_update_start_20140412
            'takeda_update_end_20140602

            xmlFollowUp.SalesCondition = listSalesCondition

            GlErrStepInfo = "SetXmlFollowUp_35"

            Dim listNegotiationMemo As New Collection(Of XmlNegotiationMemo)
            'NegotiationMemoData
            Dim Negotiation = From NegotiationMemo In NegotiationMemoData
                                Group NegotiationMemo By NegotiationMemo.RELATION_ACT_ID Into Group
                                Select New With {
                                RELATION_ACT_ID
                                }
            GlErrStepInfo = "SetXmlFollowUp_36"

            For Each NegotiationMemoRow In Negotiation.ToList()
                listNegotiationMemo.Add(SetXmlNegotiationMemoNode(NegotiationMemoRow.RELATION_ACT_ID, ActionMemoData))
            Next
            xmlFollowUp.NegotiationMemo = listNegotiationMemo
            GlErrStepInfo = "SetXmlFollowUp_37"

            'takeda_update_start_20140412
            ''Createdby
            'If (FirstSuccessFlg = CstStrSalesNormal) Then
            '    xmlFollowUp.Createdby = SalesData.ROW_CREATE_ACCOUNT
            'ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
            '    xmlFollowUp.Createdby = SalesTempData.ROW_CREATE_ACCOUNT
            'End If
            'GlErrStepInfo="SetXmlFollowUp_38"

            'Createdby
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                xmlFollowUp.Createdby = ConvertStfCd(SalesData.ROW_CREATE_ACCOUNT, CstStfCdCnvtOff)
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                xmlFollowUp.Createdby = ConvertStfCd(SalesTempData.ROW_CREATE_ACCOUNT, CstStfCdCnvtOff)
            End If
            GlErrStepInfo = "SetXmlFollowUp_38"

            'takeda_update_end_20140412

            'Createdate
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                xmlFollowUp.Createdate = ChangeDefaultDate(CDate(SalesData.ROW_CREATE_DATETIME), CstStrDefaultDate)
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                xmlFollowUp.Createdate = ChangeDefaultDate(CDate(SalesTempData.ROW_CREATE_DATETIME), CstStrDefaultDate)
            End If
            GlErrStepInfo = "SetXmlFollowUp_39"

            'Updatedby
            Dim Updatedby As String = ""
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                Dim resultUpdatedby = From Action In ActionData
                    Where Action.REQ_ID = SalesData.REQ_ID
                    Order By Action.ACT_COUNT Descending
                    Select New With {
                    Action.ACT_COUNT,
                    Action.ROW_UPDATE_ACCOUNT
                    }
                GlErrStepInfo = "SetXmlFollowUp_40"
                If (resultUpdatedby.Count() > 0) Then
                    Updatedby = resultUpdatedby.ToList()(0).ROW_UPDATE_ACCOUNT.ToString()
                Else
                    Updatedby = ""
                End If
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                Updatedby = SalesTempData.ROW_UPDATE_ACCOUNT
            End If
            GlErrStepInfo = "SetXmlFollowUp_41"

            'takeda_update_start_20140412
            'xmlFollowUp.Updatedby = Updatedby
            xmlFollowUp.Updatedby = ConvertStfCd(Updatedby, CstStfCdCnvtOff)
            'takeda_update_start_20140412

            'Updatedate
            Dim Updatedate As String = ""
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                Dim resultUpdatedate = From Action In ActionData
                    Where Action.REQ_ID = SalesData.REQ_ID
                    Order By Action.ACT_COUNT Descending
                    Select New With {
                    Action.ACT_COUNT,
                    Action.ROW_UPDATE_DATETIME
                    }
                GlErrStepInfo = "SetXmlFollowUp_42"
                If (resultUpdatedate.Count() > 0) Then
                    Updatedate = ChangeDefaultDate(resultUpdatedate.ToList()(0).ROW_UPDATE_DATETIME, CstStrDefaultDate)
                Else
                    Updatedate = ""
                End If
            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then
                If IsDBNull(SalesTempData.ROW_UPDATE_DATETIME) = False Then
                    Updatedate = ChangeDefaultDate(SalesTempData.ROW_UPDATE_DATETIME, CstStrDefaultDate)
                Else
                    Updatedate = ""
                End If
            End If
            GlErrStepInfo = "SetXmlFollowUp_43"

            xmlFollowUp.Updatedate = Updatedate
            GlErrStepInfo = "SetXmlFollowUp_End"

            Return xmlFollowUp
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
    'Public Function SetXmlProspectCustomer(ByVal IcropDealerCode As String,
    '                                              ByVal IcropBranchCode As String,
    '                                              ByVal SalesId As Long,
    '                                              ByVal CountryCode As String,
    '                                              ByVal FirstSuccessFlg As String,
    '                                              ByVal SalesData As IC3802801DataSet.IC3802801SalesRow,
    '                                              ByVal SalesTempData As IC3802801DataSet.IC3802801SalesTempRow,
    '                                              ByVal RequestData As IC3802801DataSet.IC3802801FollowUpRequestRow,
    '                                              ByVal AttractData As IC3802801DataSet.IC3802801FollowUpAttractRow,
    '                                              ByVal VehicleData As IC3802801VehicleDataTable,
    '                                              ByVal CustomerData As IC3802801DataSet.IC3802801CustomerRow,
    '                                              ByVal SelectedSeriesData As IC3802801DataSet.IC3802801SelectedSeriesDataTable,
    '                                              ByVal CompetitorSeriesData As IC3802801DataSet.IC3802801CompetitorSeriesDataTable,
    '                                              ByVal SalesConditionData As IC3802801DataSet.IC3802801SalesConditionDataTable,
    '                                              ByVal ReqSrcData1 As IC3802801DataSet.IC3802801ReqSource1Row,
    '                                              ByVal ReqSrcData2 As IC3802801DataSet.IC3802801ReqSource2Row,
    '                                              ByVal ActionData As IC3802801DataSet.IC3802801ActionDataTable,
    '                                              ByVal ActionResultData As IC3802801DataSet.IC3802801ActionResultRow,
    '                                              ByVal EstimateData As IC3802801DataSet.IC3802801EstimateInfoRow,
    '                                              ByVal MakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
    '                                              ByVal FollowUpActionData As IC3802801DataSet.IC3802801ActionRow,
    '                                              ByVal FollowUpResultData As IC3802801DataSet.IC3802801ActionRow,
    '                                              ByVal NegotiationMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
    '                                              ByVal FllwUpBoxSalesData As IC3802801DataSet.IC3802801GetFllwUpBoxSalesDataTable,
    '                                              ByVal SalesActData As IC3802801DataSet.IC3802801SalesActionDataTable,
    '                                              ByVal DlrCustomerMemoData As IC3802801DataSet.IC3802801CustomerMemoRow,
    '                                              ByVal ActionMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
    '                                              ByVal VehicleMakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
    '                                              ByVal DlrCstVclData As IC3802801DataSet.IC3802801DlrCstVclDataTable,
    '                                              ByVal LastActionKeyData As IC3802801DataSet.IC3802801ActionRow,
    '                                              ByVal ActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable,
    '                                              ByVal ContactTimeslotData As IC3802801DataSet.IC3802801ContactTimeslotRow,
    '                                              ByVal StateInfoData As IC3802801DataSet.IC3802801StateInfoRow,
    '                                              ByVal DistrictInfoData As IC3802801DataSet.IC3802801DistrictInfoRow,
    '                                              ByVal CityInfoData As IC3802801DataSet.IC3802801CityInfoRow,
    '                                              ByVal LocationInfoData As IC3802801DataSet.IC3802801LocationInfoRow,
    '                                              ByVal SalesActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable,
    '                                              ByVal EstimateVclData As IC3802801DataSet.IC3802801EstimateVclInfoRow,
    '                                              ByVal EstimateVclDataT As IC3802801DataSet.IC3802801EstimateVclInfoDataTable
    '                                              ) As XmlProspectCustomer
    Public Function SetXmlProspectCustomer(ByVal IcropDealerCode As String,
                                              ByVal IcropBranchCode As String,
                                              ByVal SalesId As Long,
                                              ByVal CountryCode As String,
                                              ByVal FirstSuccessFlg As String,
                                              ByVal SalesData As IC3802801DataSet.IC3802801SalesRow,
                                              ByVal SalesTempData As IC3802801DataSet.IC3802801SalesTempRow,
                                              ByVal RequestData As IC3802801DataSet.IC3802801FollowUpRequestRow,
                                              ByVal AttractData As IC3802801DataSet.IC3802801FollowUpAttractRow,
                                              ByVal VehicleData As IC3802801VehicleDataTable,
                                              ByVal CustomerData As IC3802801DataSet.IC3802801CustomerRow,
                                              ByVal SelectedSeriesData As IC3802801DataSet.IC3802801SelectedSeriesDataTable,
                                              ByVal CompetitorSeriesData As IC3802801DataSet.IC3802801CompetitorSeriesDataTable,
                                              ByVal SalesConditionData As IC3802801DataSet.IC3802801SalesConditionDataTable,
                                              ByVal ReqSrcData1 As IC3802801DataSet.IC3802801ReqSource1Row,
                                              ByVal ReqSrcData2 As IC3802801DataSet.IC3802801ReqSource2Row,
                                              ByVal ActionData As IC3802801DataSet.IC3802801ActionDataTable,
                                              ByVal ActionResultData As IC3802801DataSet.IC3802801ActionResultRow,
                                              ByVal EstimateData As IC3802801DataSet.IC3802801EstimateInfoRow,
                                              ByVal MakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
                                              ByVal FollowUpActionData As IC3802801DataSet.IC3802801ActionRow,
                                              ByVal FollowUpResultData As IC3802801DataSet.IC3802801ActionRow,
                                              ByVal NegotiationMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
                                              ByVal FllwUpBoxSalesData As IC3802801DataSet.IC3802801GetFllwUpBoxSalesDataTable,
                                              ByVal SalesActData As IC3802801DataSet.IC3802801SalesActionDataTable,
                                              ByVal DlrCustomerMemoData As IC3802801DataSet.IC3802801CustomerMemoRow,
                                              ByVal ActionMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
                                              ByVal VehicleMakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
                                              ByVal DlrCstVclData As IC3802801DataSet.IC3802801DlrCstVclDataTable,
                                              ByVal LastActionKeyData As IC3802801DataSet.IC3802801ActionRow,
                                              ByVal ActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable,
                                              ByVal ContactTimeslotData As IC3802801DataSet.IC3802801ContactTimeslotRow,
                                              ByVal StateInfoData As IC3802801DataSet.IC3802801StateInfoRow,
                                              ByVal DistrictInfoData As IC3802801DataSet.IC3802801DistrictInfoRow,
                                              ByVal CityInfoData As IC3802801DataSet.IC3802801CityInfoRow,
                                              ByVal LocationInfoData As IC3802801DataSet.IC3802801LocationInfoRow,
                                              ByVal SalesActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable,
                                              ByVal EstimateVclData As IC3802801DataSet.IC3802801EstimateVclInfoRow,
                                              ByVal EstimateVclDataT As IC3802801DataSet.IC3802801EstimateVclInfoDataTable,
                                              ByVal SalesLocalData As IC3802801DataSet.IC3802801SalesLocalRow) As XmlProspectCustomer
        '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end
        '20140317 Fujita Upd Start
        Try
            GlErrStepInfo = "SetXmlProspectCustomer_Start"
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                GlErrStepInfo = "FirstSuccessFlg=Normal"
                If (SalesId <> 0) Then
                    GlErrStepInfo = "SALES_ID(Param):" + SalesId.ToString()
                Else
                    GlErrStepInfo = "SALES_ID(Param)=No Data"
                End If
                If (SalesData.SALES_ID <> "") Then
                    GlErrStepInfo = "SALES_ID:" + SalesData.SALES_ID
                Else
                    GlErrStepInfo = "SALES_ID=No Data"
                End If

            ElseIf (FirstSuccessFlg = CstStrFirstSuccess) Then

                GlErrStepInfo = "FirstSuccessFlg=FirstTimeSuccess"
                If (SalesId <> 0) Then
                    GlErrStepInfo = "SALES_ID(Param):" + SalesId.ToString()
                Else
                    GlErrStepInfo = "SALES_ID(Param)=No Data"
                End If
                If (SalesTempData.SALES_ID <> "") Then
                    GlErrStepInfo = "SALES_ID:" + SalesTempData.SALES_ID.ToString()
                Else
                    GlErrStepInfo = "SALES_ID=No Data"
                End If
            Else
                GlErrStepInfo = "FirstSuccessFlg=Other"
                GlErrStepInfo = "SALES_ID=No Data"
            End If

            If (RequestData.REQ_ID <> 0) Then
                GlErrStepInfo = "REQ_ID:" + RequestData.REQ_ID.ToString()
            Else
                GlErrStepInfo = "REQ_ID=No Data"
            End If
            If (AttractData.ATT_ID <> 0) Then
                GlErrStepInfo = "ATT_ID:" + AttractData.ATT_ID.ToString()
            Else
                GlErrStepInfo = "ATT_ID=No Data"
            End If
            If (CustomerData.CST_ID <> "") Then
                GlErrStepInfo = "CST_ID:" + CustomerData.CST_ID.ToString()
            Else
                GlErrStepInfo = "CST_ID=No Data"
            End If

            'Dim staff As StaffContext = StaffContext.Current
            Dim TransmissionDate As DateTime = Date.Now
            Dim xmlProspectCustomerData As New XmlProspectCustomer
            xmlProspectCustomerData.Head = SetXmlHead(CountryCode, TransmissionDate)
            'xmlProspectCustomer.Common = SetXmlCommon(staff)
            'takeda_update_start_20140529_ログ情報コメントアウト(一発Success時、FollowUpResultは未設定なので参照不可)
            ''takeda_update_start_20140328
            'GlErrStepInfo="(Check1)FollowUpResultData")
            'GlErrStepInfo="ACT_ID")
            'GlErrStepInfo=FollowUpResultData.ACT_ID)
            'GlErrStepInfo="ACT_STATUS")
            'GlErrStepInfo=FollowUpResultData.ACT_STATUS)
            'GlErrStepInfo="ACT_COUNT")
            'GlErrStepInfo=FollowUpResultData.ACT_COUNT)
            ''takeda_update_end_20140328
            'takeda_update_end_20140529_ログ情報コメントアウト(一発Success時、FollowUpResultは未設定なので参照不可)

            xmlProspectCustomerData.Common = SetXmlCommon(IcropDealerCode, IcropBranchCode)
            xmlProspectCustomerData.FollowUpResult = SetXmlFollowUpResult(IcropDealerCode, IcropBranchCode, FirstSuccessFlg, SalesData, CompetitorSeriesData, ActionResultData, EstimateData, SelectedSeriesData, FollowUpResultData, FollowUpActionData, LastActionKeyData, MakerModelData, SalesTempData, EstimateVclData, EstimateVclDataT)
            xmlProspectCustomerData.Vehicle = SetXmlVehicle(VehicleData, VehicleMakerModelData)
            xmlProspectCustomerData.Customer = SetXmlCustomer(IcropDealerCode, SalesData, CustomerData, DlrCustomerMemoData, TransmissionDate, DlrCstVclData, ContactTimeslotData, StateInfoData, DistrictInfoData, CityInfoData, LocationInfoData)
            xmlProspectCustomerData.FollowUp = SetXmlFollowUp(IcropDealerCode, IcropBranchCode, SalesId.ToString(), SelectedSeriesData, TransmissionDate, SalesData, SalesTempData, RequestData, AttractData, CompetitorSeriesData, SalesConditionData, ReqSrcData1, ReqSrcData2, FollowUpResultData, FollowUpActionData, ActionData, NegotiationMemoData, FllwUpBoxSalesData, SalesActData, MakerModelData, EstimateData, VehicleData, ActionMemoData, ActionSeqData, FirstSuccessFlg, SalesLocalData)
            '$26 start
            xmlProspectCustomerData.SalesLocal = SetXmlSalesLocal(SalesId)
            '$26 end
            GlErrStepInfo = "SetXmlProspectCustomer_End"
            Return xmlProspectCustomerData
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlDocument(ByVal ProspectCustomerData As XmlProspectCustomer) As XmlDocument
        GlErrStepInfo = "SetXmlDocument_Start"
        Dim xmlDoc As New XmlDocument()
        '20140317 Fujita Upd Start
        Try
            Dim docNode As XmlNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", vbNullString)
            xmlDoc.AppendChild(docNode)

            Dim RootNode As XmlNode = xmlDoc.CreateElement("FollowUpInfomation")
            xmlDoc.AppendChild(RootNode)
            RootNode.AppendChild(SetHead(xmlDoc, ProspectCustomerData.Head))

            Dim DetailNode = xmlDoc.CreateElement("Detail")
            DetailNode.AppendChild(SetCommon(xmlDoc, ProspectCustomerData.Common))
            DetailNode.AppendChild(SetFollowUp(xmlDoc, ProspectCustomerData))
            DetailNode.AppendChild(SetFollowUpResult(xmlDoc, ProspectCustomerData.FollowUpResult))

            '$26 start
            If (TradeincarEnabledAvailable.Equals(ProspectCustomerData.SalesLocal.TradeincarEnabledFlg)) Then
                DetailNode.AppendChild(SetTradeinLocal(xmlDoc, ProspectCustomerData.SalesLocal))
            End If
            '$26 end

            GlErrStepInfo = "SetXmlDocument_1"

            If (ProspectCustomerData.Vehicle IsNot Nothing) Then
                For Each vehicleRow As XmlVehicle In ProspectCustomerData.Vehicle
                    DetailNode.AppendChild(SetVehicle(xmlDoc, vehicleRow))
                Next
            End If

            DetailNode.AppendChild(SetCustomer(xmlDoc, ProspectCustomerData.Customer))
            RootNode.AppendChild(DetailNode)
            GlErrStepInfo = "SetXmlDocument_End"
            Return xmlDoc
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetHead(ByVal xmlDoc As XmlDocument, ByVal xmlHead As XmlHead) As XmlNode
        GlErrStepInfo = "SetHead_Start"
        '20140317 Fujita Upd Start
        Try
            Dim HeadNode As XmlNode = xmlDoc.CreateElement("Head")

            Dim MessageIDNode As XmlNode = xmlDoc.CreateElement("MessageID")
            MessageIDNode.InnerText = xmlHead.MessageID
            HeadNode.AppendChild(MessageIDNode)

            Dim CountryCodeNode As XmlNode = xmlDoc.CreateElement("CountryCode")
            CountryCodeNode.InnerText = xmlHead.CountryCode
            HeadNode.AppendChild(CountryCodeNode)

            GlErrStepInfo = "SetHead_1"

            Dim LinkSystemCodeNode As XmlNode = xmlDoc.CreateElement("LinkSystemCode")
            LinkSystemCodeNode.InnerText = xmlHead.LinkSystemCode
            HeadNode.AppendChild(LinkSystemCodeNode)

            Dim TransmissionDateNode As XmlNode = xmlDoc.CreateElement("TransmissionDate")
            TransmissionDateNode.InnerText = xmlHead.TransmissionDate
            HeadNode.AppendChild(TransmissionDateNode)
            GlErrStepInfo = "SetHead_End"
            Return HeadNode
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetCommon(ByVal xmlDoc As XmlDocument, ByVal xmlCommon As XmlCommon) As XmlNode
        GlErrStepInfo = "SetCommon_Start"
        '20140317 Fujita Upd Start
        Try
            Dim CommonNode As XmlNode = xmlDoc.CreateElement("Common")

            Dim DealerCodeNode As XmlNode = xmlDoc.CreateElement("DealerCode")
            DealerCodeNode.InnerText = xmlCommon.DealerCode
            CommonNode.AppendChild(DealerCodeNode)

            Dim BranchCodeNode As XmlNode = xmlDoc.CreateElement("BranchCode")
            BranchCodeNode.InnerText = xmlCommon.BranchCode
            CommonNode.AppendChild(BranchCodeNode)

            GlErrStepInfo = "SetCommon_1"

            Dim IcropDealerCodeNode As XmlNode = xmlDoc.CreateElement("IcropDealerCode")
            IcropDealerCodeNode.InnerText = xmlCommon.IcropDealerCode
            CommonNode.AppendChild(IcropDealerCodeNode)

            Dim IcropBranchCodeNode As XmlNode = xmlDoc.CreateElement("IcropBranchCode")
            IcropBranchCodeNode.InnerText = xmlCommon.IcropBranchCode
            CommonNode.AppendChild(IcropBranchCodeNode)
            GlErrStepInfo = "SetCommon_End"
            Return CommonNode
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetFollowUp(ByVal xmlDoc As XmlDocument, ByVal xmlProspectCustomer As XmlProspectCustomer) As XmlNode
        GlErrStepInfo = "SetFollowUp_Start"
        '20140317 Fujita Upd Start
        Try

            Dim xmlFollowUp As XmlFollowUp
            xmlFollowUp = xmlProspectCustomer.FollowUp

            Dim FollowUpNode As XmlNode = xmlDoc.CreateElement("FollowUp")

            Dim SeqNoNode As XmlNode = xmlDoc.CreateElement("SeqNo")
            SeqNoNode.InnerText = xmlFollowUp.SeqNo
            FollowUpNode.AppendChild(SeqNoNode)

            Dim FollowUpIDNode As XmlNode = xmlDoc.CreateElement("FollowUpID")
            FollowUpIDNode.InnerText = xmlFollowUp.FollowUpID
            FollowUpNode.AppendChild(FollowUpIDNode)

            Dim FollowUpNoNode As XmlNode = xmlDoc.CreateElement("FollowUpNo")
            FollowUpNoNode.InnerText = xmlFollowUp.FollowUpNo
            FollowUpNode.AppendChild(FollowUpNoNode)

            GlErrStepInfo = "SetFollowUp_1"

            Dim ParentFollowUpNoNode As XmlNode = xmlDoc.CreateElement("ParentFollowUpNo")
            ParentFollowUpNoNode.InnerText = xmlFollowUp.ParentFollowUpNo
            FollowUpNode.AppendChild(ParentFollowUpNoNode)

            Dim PreFollowUpNoNode As XmlNode = xmlDoc.CreateElement("PreFollowUpNo")
            PreFollowUpNoNode.InnerText = xmlFollowUp.PreFollowUpNo
            FollowUpNode.AppendChild(PreFollowUpNoNode)

            Dim FollowUpDateNode As XmlNode = xmlDoc.CreateElement("FollowUpDate")
            FollowUpDateNode.InnerText = xmlFollowUp.FollowUpDate
            FollowUpNode.AppendChild(FollowUpDateNode)

            Dim PreFollowUpCreateDateNode As XmlNode = xmlDoc.CreateElement("PreFollowUpCreateDate")
            PreFollowUpCreateDateNode.InnerText = xmlFollowUp.PreFollowUpCreateDate
            FollowUpNode.AppendChild(PreFollowUpCreateDateNode)

            Dim DemandStructureNode As XmlNode = xmlDoc.CreateElement("DemandStructure")
            '$26 start
            'DemandStructureNode.InnerText = xmlFollowUp.DemandStructure
            DemandStructureNode.InnerText = xmlProspectCustomer.SalesLocal.DemandStructureCd
            '$26 end
            FollowUpNode.AppendChild(DemandStructureNode)

            GlErrStepInfo = "SetFollowUp_2"

            Dim DirectBillingFlgNode As XmlNode = xmlDoc.CreateElement("DirectBillingFlg")
            DirectBillingFlgNode.InnerText = xmlFollowUp.DirectBillingFlg
            FollowUpNode.AppendChild(DirectBillingFlgNode)

            Dim FirstContactTypeNode As XmlNode = xmlDoc.CreateElement("FirstContactType")
            FirstContactTypeNode.InnerText = xmlFollowUp.FirstContactType
            FollowUpNode.AppendChild(FirstContactTypeNode)

            Dim SourceID1Node As XmlNode = xmlDoc.CreateElement("SourceID1")
            SourceID1Node.InnerText = xmlFollowUp.SourceID1
            FollowUpNode.AppendChild(SourceID1Node)

            Dim SourceName1Node As XmlNode = xmlDoc.CreateElement("SourceName1")
            SourceName1Node.InnerText = xmlFollowUp.SourceName1
            FollowUpNode.AppendChild(SourceName1Node)

            Dim SourceID2Node As XmlNode = xmlDoc.CreateElement("SourceID2")
            SourceID2Node.InnerText = xmlFollowUp.SourceID2
            FollowUpNode.AppendChild(SourceID2Node)

            Dim SourceName2Node As XmlNode = xmlDoc.CreateElement("SourceName2")
            SourceName2Node.InnerText = xmlFollowUp.SourceName2
            FollowUpNode.AppendChild(SourceName2Node)

            GlErrStepInfo = "SetFollowUp_3"

            Dim PotentialDivisionNode As XmlNode = xmlDoc.CreateElement("PotentialDivision")
            PotentialDivisionNode.InnerText = xmlFollowUp.PotentialDivision
            FollowUpNode.AppendChild(PotentialDivisionNode)

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim VinNode As XmlNode = CreateCDataSection(xmlDoc, "Vin", xmlFollowUp.Vin)
            'Dim VinNode As XmlNode = xmlDoc.CreateElement("Vin")
            'VinNode.InnerText = xmlFollowUp.Vin
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            FollowUpNode.AppendChild(VinNode)

            Dim InterestDateNode As XmlNode = xmlDoc.CreateElement("InterestDate")
            InterestDateNode.InnerText = xmlFollowUp.InterestDate
            FollowUpNode.AppendChild(InterestDateNode)

            Dim ProspectDateNode As XmlNode = xmlDoc.CreateElement("ProspectDate")
            ProspectDateNode.InnerText = xmlFollowUp.ProspectDate
            FollowUpNode.AppendChild(ProspectDateNode)

            Dim HotDateNode As XmlNode = xmlDoc.CreateElement("HotDate")
            HotDateNode.InnerText = xmlFollowUp.HotDate
            FollowUpNode.AppendChild(HotDateNode)

            Dim ReconsiderDateNode As XmlNode = xmlDoc.CreateElement("ReconsiderDate")
            ReconsiderDateNode.InnerText = xmlFollowUp.ReconsiderDate
            FollowUpNode.AppendChild(ReconsiderDateNode)

            GlErrStepInfo = "SetFollowUp_4"

            Dim OtherDLRPurchaseFlgNode As XmlNode = xmlDoc.CreateElement("OtherDLRPurchaseFlg")
            OtherDLRPurchaseFlgNode.InnerText = xmlFollowUp.OtherDLRPurchaseFlg
            FollowUpNode.AppendChild(OtherDLRPurchaseFlgNode)

            Dim SalesTargetDateNode As XmlNode = xmlDoc.CreateElement("SalesTargetDate")
            SalesTargetDateNode.InnerText = xmlFollowUp.SalesTargetDate
            FollowUpNode.AppendChild(SalesTargetDateNode)

            Dim PlannedBranchCodeNode As XmlNode = xmlDoc.CreateElement("PlannedBranchCode")
            PlannedBranchCodeNode.InnerText = xmlFollowUp.PlannedBranchCode
            FollowUpNode.AppendChild(PlannedBranchCodeNode)

            Dim PlannedAccountNode As XmlNode = xmlDoc.CreateElement("PlannedAccount")
            PlannedAccountNode.InnerText = xmlFollowUp.PlannedAccount
            FollowUpNode.AppendChild(PlannedAccountNode)

            GlErrStepInfo = "SetFollowUp_5(Set SelctedSeries)"

            If (xmlProspectCustomer.Vehicle IsNot Nothing) Then
                Dim xmlSelectedSeriesCollection As Collection(Of XmlSelectedSeries)
                xmlSelectedSeriesCollection = xmlProspectCustomer.FollowUp.SelectedSeries
                For Each xmlSelectedSeries As XmlSelectedSeries In xmlSelectedSeriesCollection
                    FollowUpNode.AppendChild(SetSelectedSeries(xmlDoc, xmlSelectedSeries))
                Next
            End If

            GlErrStepInfo = "SetFollowUp_6(Set CompetitorSeries)"

            If (xmlProspectCustomer.Vehicle IsNot Nothing) Or (xmlProspectCustomer.FollowUp.Action IsNot Nothing) Then
                Dim xmlCompetitorSeriesCollection As Collection(Of XmlCompetitorSeries)
                xmlCompetitorSeriesCollection = xmlProspectCustomer.FollowUp.CompetitorSeries
                If (xmlCompetitorSeriesCollection IsNot Nothing) Then
                    For Each xmlCompetitorSeries As XmlCompetitorSeries In xmlCompetitorSeriesCollection
                        FollowUpNode.AppendChild(SetCompetitorSeries(xmlDoc, xmlCompetitorSeries))
                    Next
                End If
            End If

            GlErrStepInfo = "SetFollowUp_7(Set Action)"

            If (xmlProspectCustomer.FollowUp.Action IsNot Nothing) Then
                Dim xmlActionCollection As Collection(Of XmlAction)
                xmlActionCollection = xmlProspectCustomer.FollowUp.Action
                If (xmlProspectCustomer.FollowUp.Action IsNot Nothing) Then
                    For Each xmlAction As XmlAction In xmlActionCollection
                        FollowUpNode.AppendChild(SetAction(xmlDoc, xmlAction))
                    Next
                End If
            End If

            GlErrStepInfo = "SetFollowUp_8(Set SalesCondition)"

            If (xmlProspectCustomer.FollowUp.SalesCondition IsNot Nothing) Then
                Dim xmlSalesConditionCollection As Collection(Of XmlSalesCondition)
                xmlSalesConditionCollection = xmlProspectCustomer.FollowUp.SalesCondition
                If (xmlProspectCustomer.FollowUp.SalesCondition IsNot Nothing) Then
                    'takeda_update_start_20140606(SalesConditionNoが同じ場合、出力しないようにする)
                    'For Each xmlSalesCondition As XmlSalesCondition In xmlSalesConditionCollection
                    '    FollowUpNode.AppendChild(SetSalesCondition(xmlDoc, xmlSalesCondition))
                    'Next

                    Dim intRecCnt As Long = 0
                    Dim strSalesConditionNo As String = ""
                    Dim xmlSelectItem As New XmlSelectItem
                    Dim SalesConditionNode As XmlNode = xmlDoc.CreateElement("SalesCondition")
                    Dim SalesConditionNoNode As XmlNode = xmlDoc.CreateElement("SalesConditionNo")
                    GlErrStepInfo = "SetFollowUp_9(Set SalesCondition Item)"
                    For Each xmlSalesCondition As XmlSalesCondition In xmlSalesConditionCollection

                        xmlSelectItem.ItemNo = xmlSalesCondition.ItemNo
                        xmlSelectItem.Other = xmlSalesCondition.Other
                        'GlErrStepInfo = "RecCnt:" & intRecCnt.ToString

                        '1件目の場合
                        If (strSalesConditionNo = "") Then

                            GlErrStepInfo = "Rec=1 Case"
                            'GlErrStepInfo = "SalesConditionNo:" & xmlSalesCondition.SalesConditionNo.ToString

                            'SalesConditionタグに、SalesConditionNoを書き込む(保持)
                            SalesConditionNoNode = xmlDoc.CreateElement("SalesConditionNo")
                            SalesConditionNoNode.InnerText = xmlSalesCondition.SalesConditionNo
                            SalesConditionNode.AppendChild(SalesConditionNoNode)

                            'SalesConditionタグに、SelectItemタグを書き込む(保持)
                            SalesConditionNode.AppendChild(SetSelectItem(xmlDoc, xmlSelectItem))

                            '使用したSalesConditionNoを保持
                            strSalesConditionNo = xmlSalesCondition.SalesConditionNo
                        Else
                            '2件目以降の場合
                            If (xmlSalesCondition.SalesConditionNo.Equals(strSalesConditionNo)) Then

                                GlErrStepInfo = "Same Key Case"
                                'SalesConditionタグに、SelectItemタグを書き込む(保持)
                                SalesConditionNode.AppendChild(SetSelectItem(xmlDoc, xmlSelectItem))

                            Else
                                'SalesConditionNoが変わった場合
                                GlErrStepInfo = "Change Key Case"
                                'GlErrStepInfo = "SalesConditionNo(old):" & strSalesConditionNo

                                'FollowUpタグに、保持していたSalesConditionタグを書き込む
                                FollowUpNode.AppendChild(SalesConditionNode)

                                'SalesCondition領域をクリア
                                SalesConditionNode = xmlDoc.CreateElement("SalesCondition")
                                '
                                'GlErrStepInfo = "SalesConditionNo:" & xmlSalesCondition.SalesConditionNo.ToString

                                'SalesConditionタグに、SalesConditionNoを書き込む(保持)
                                SalesConditionNoNode = xmlDoc.CreateElement("SalesConditionNo")
                                SalesConditionNoNode.InnerText = xmlSalesCondition.SalesConditionNo
                                SalesConditionNode.AppendChild(SalesConditionNoNode)
                                'SalesConditionタグに、SelectItemタグを書き込む(保持)
                                SalesConditionNode.AppendChild(SetSelectItem(xmlDoc, xmlSelectItem))

                                '使用したSalesConditionNoを保持
                                strSalesConditionNo = xmlSalesCondition.SalesConditionNo
                            End If
                        End If
                        intRecCnt = intRecCnt + 1
                    Next
                    If (intRecCnt > 0) Then
                        'ループ後処理
                        GlErrStepInfo = "Last Dataset Case"
                        'GlErrStepInfo = "SalesConditionNo(old):" & strSalesConditionNo

                        'FollowUpタグに、保持していたSalesConditionタグを書き込む
                        FollowUpNode.AppendChild(SalesConditionNode)
                    End If
                    'takeda_update_end_20140606
                End If
            End If

            GlErrStepInfo = "SetFollowUp_10(Set NegotiationMemo)"

            If (xmlProspectCustomer.FollowUp.NegotiationMemo IsNot Nothing) Then
                Dim xmlNegotiationMemoCollection As Collection(Of XmlNegotiationMemo)
                xmlNegotiationMemoCollection = xmlProspectCustomer.FollowUp.NegotiationMemo
                If (xmlProspectCustomer.FollowUp.NegotiationMemo IsNot Nothing) Then
                    For Each xmlNegotiationMemo As XmlNegotiationMemo In xmlNegotiationMemoCollection
                        FollowUpNode.AppendChild(SetNegotiationMemo(xmlDoc, xmlNegotiationMemo))
                    Next
                End If
            End If

            GlErrStepInfo = "SetFollowUp_11"

            Dim CreatedbyNode As XmlNode = xmlDoc.CreateElement("Createdby")
            CreatedbyNode.InnerText = xmlFollowUp.Createdby
            FollowUpNode.AppendChild(CreatedbyNode)

            Dim CreatedateNode As XmlNode = xmlDoc.CreateElement("Createdate")
            CreatedateNode.InnerText = xmlFollowUp.Createdate
            FollowUpNode.AppendChild(CreatedateNode)

            Dim UpdatedbyNode As XmlNode = xmlDoc.CreateElement("Updatedby")
            UpdatedbyNode.InnerText = xmlFollowUp.Updatedby
            FollowUpNode.AppendChild(UpdatedbyNode)

            Dim UpdatedateNode As XmlNode = xmlDoc.CreateElement("Updatedate")
            UpdatedateNode.InnerText = xmlFollowUp.Updatedate
            FollowUpNode.AppendChild(UpdatedateNode)

            GlErrStepInfo = "SetFollowUp_End"
            Return FollowUpNode
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetSelectedSeries(ByVal xmlDoc As XmlDocument, ByVal xmlSelectedSeries As XmlSelectedSeries) As XmlNode
        GlErrStepInfo = "SetSelectedSeries_Start"
        '20140317 Fujita Upd Start
        Try
            Dim SelectedSeriesNode As XmlNode = xmlDoc.CreateElement("SelectedSeries")

            Dim SelectedSeriesNoNode As XmlNode = xmlDoc.CreateElement("SelectedSeriesNo")
            SelectedSeriesNoNode.InnerText = xmlSelectedSeries.SelectedSeriesNo
            SelectedSeriesNode.AppendChild(SelectedSeriesNoNode)

            Dim PreferredVehicleFlgNode As XmlNode = xmlDoc.CreateElement("PreferredVehicleFlg")
            PreferredVehicleFlgNode.InnerText = xmlSelectedSeries.PreferredVehicleFlg
            SelectedSeriesNode.AppendChild(PreferredVehicleFlgNode)

            GlErrStepInfo = "SetSelectedSeries_1"

            Dim SeriesCodeNode As XmlNode = xmlDoc.CreateElement("SeriesCode")
            SeriesCodeNode.InnerText = xmlSelectedSeries.SeriesCode
            SelectedSeriesNode.AppendChild(SeriesCodeNode)

            Dim GradeCodeNode As XmlNode = xmlDoc.CreateElement("GradeCode")
            GradeCodeNode.InnerText = xmlSelectedSeries.GradeCode
            SelectedSeriesNode.AppendChild(GradeCodeNode)

            Dim ExteriorColorCodeNode As XmlNode = xmlDoc.CreateElement("ExteriorColorCode")
            ExteriorColorCodeNode.InnerText = xmlSelectedSeries.ExteriorColorCode
            SelectedSeriesNode.AppendChild(ExteriorColorCodeNode)

            Dim InteriorColorCodeNode As XmlNode = xmlDoc.CreateElement("InteriorColorCode")
            InteriorColorCodeNode.InnerText = xmlSelectedSeries.InteriorColorCode
            SelectedSeriesNode.AppendChild(InteriorColorCodeNode)

            Dim ModelSuffixNode As XmlNode = xmlDoc.CreateElement("ModelSuffix")
            ModelSuffixNode.InnerText = xmlSelectedSeries.ModelSuffix
            SelectedSeriesNode.AppendChild(ModelSuffixNode)

            GlErrStepInfo = "SetSelectedSeries_2"

            Dim QuantityNode As XmlNode = xmlDoc.CreateElement("Quantity")
            QuantityNode.InnerText = xmlSelectedSeries.Quantity
            SelectedSeriesNode.AppendChild(QuantityNode)

            Dim QuotationPriceNode As XmlNode = xmlDoc.CreateElement("QuotationPrice")
            QuotationPriceNode.InnerText = xmlSelectedSeries.QuotationPrice
            SelectedSeriesNode.AppendChild(QuotationPriceNode)

            GlErrStepInfo = "SetSelectedSeries_End"
            Return SelectedSeriesNode
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetCompetitorSeries(ByVal xmlDoc As XmlDocument, ByVal xmlCompetitorSeries As XmlCompetitorSeries) As XmlNode
        GlErrStepInfo = "SetCompetitorSeries_Start"
        '20140317 Fujita Upd Start
        Try
            Dim CompetitorSeriesNode As XmlNode = xmlDoc.CreateElement("CompetitorSeries")

            Dim MakerCodeNode As XmlNode = xmlDoc.CreateElement("MakerCode")
            MakerCodeNode.InnerText = xmlCompetitorSeries.MakerCode
            CompetitorSeriesNode.AppendChild(MakerCodeNode)

            Dim MakerNameNode As XmlNode = xmlDoc.CreateElement("MakerName")
            MakerNameNode.InnerText = xmlCompetitorSeries.MakerName
            CompetitorSeriesNode.AppendChild(MakerNameNode)

            GlErrStepInfo = "SetCompetitorSeries_1"

            Dim SeriesCodeNode As XmlNode = xmlDoc.CreateElement("SeriesCode")
            SeriesCodeNode.InnerText = xmlCompetitorSeries.SeriesCode
            CompetitorSeriesNode.AppendChild(SeriesCodeNode)

            Dim SeriesNameNode As XmlNode = xmlDoc.CreateElement("SeriesName")
            SeriesNameNode.InnerText = xmlCompetitorSeries.SeriesName
            CompetitorSeriesNode.AppendChild(SeriesNameNode)

            Dim DeleteDateNode As XmlNode = xmlDoc.CreateElement("DeleteDate")
            DeleteDateNode.InnerText = xmlCompetitorSeries.DeleteDate
            CompetitorSeriesNode.AppendChild(DeleteDateNode)

            GlErrStepInfo = "SetCompetitorSeries_End"
            Return CompetitorSeriesNode
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetAction(ByVal xmlDoc As XmlDocument, ByVal xmlAction As XmlAction) As XmlNode
        GlErrStepInfo = "SetAction_Start"
        '20140317 Fujita Upd Start
        Try
            Dim ActionNode As XmlNode = xmlDoc.CreateElement("Action")

            Dim ActionSeqNoNode As XmlNode = xmlDoc.CreateElement("ActionSeqNo")
            ActionSeqNoNode.InnerText = xmlAction.ActionSeqNo
            ActionNode.AppendChild(ActionSeqNoNode)

            Dim ActionCodeNode As XmlNode = xmlDoc.CreateElement("ActionCode")
            ActionCodeNode.InnerText = xmlAction.ActionCode
            ActionNode.AppendChild(ActionCodeNode)

            Dim ActionNameNode As XmlNode = xmlDoc.CreateElement("ActionName")
            ActionNameNode.InnerText = xmlAction.ActionName
            ActionNode.AppendChild(ActionNameNode)

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim ActionMemoNode As XmlNode = CreateCDataSection(xmlDoc, "ActionMemo", xmlAction.ActionMemo)
            'Dim ActionMemoNode As XmlNode = xmlDoc.CreateElement("ActionMemo")
            'ActionMemoNode.InnerText = xmlAction.ActionMemo
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END
            ActionNode.AppendChild(ActionMemoNode)

            GlErrStepInfo = "SetAction_1"

            Dim PlannedActionDateNode As XmlNode = xmlDoc.CreateElement("PlannedActionDate")
            PlannedActionDateNode.InnerText = xmlAction.PlannedActionDate
            ActionNode.AppendChild(PlannedActionDateNode)

            Dim StartActionDateNode As XmlNode = xmlDoc.CreateElement("StartActionDate")
            StartActionDateNode.InnerText = xmlAction.StartActionDate
            ActionNode.AppendChild(StartActionDateNode)

            Dim ActionDateNode As XmlNode = xmlDoc.CreateElement("ActionDate")
            ActionDateNode.InnerText = xmlAction.ActionDate
            ActionNode.AppendChild(ActionDateNode)

            GlErrStepInfo = "SetAction_2"

            Dim ActionBranchCodeNode As XmlNode = xmlDoc.CreateElement("ActionBranchCode")
            ActionBranchCodeNode.InnerText = xmlAction.ActionBranchCode
            ActionNode.AppendChild(ActionBranchCodeNode)

            Dim ActionAccountNode As XmlNode = xmlDoc.CreateElement("ActionAccount")
            ActionAccountNode.InnerText = xmlAction.ActionAccount
            ActionNode.AppendChild(ActionAccountNode)

            Return ActionNode
            GlErrStepInfo = "SetAction_End"
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetSalesCondition(ByVal xmlDoc As XmlDocument, ByVal xmlSalesCondition As XmlSalesCondition) As XmlNode
        GlErrStepInfo = "SetSalesCondition_Start"
        '20140317 Fujita Upd Start
        Try
            Dim xmlSelectItem As New XmlSelectItem
            Dim ItemNo As String = ""
            Dim Other As String = ""
            ItemNo = xmlSalesCondition.ItemNo
            Other = xmlSalesCondition.Other
            ItemNo = xmlSalesCondition.ItemNo
            Other = xmlSalesCondition.Other
            xmlSelectItem.ItemNo = ItemNo
            xmlSelectItem.Other = Other

            Dim SalesConditionNode As XmlNode = xmlDoc.CreateElement("SalesCondition")

            Dim SalesConditionNoNode As XmlNode = xmlDoc.CreateElement("SalesConditionNo")
            SalesConditionNoNode.InnerText = xmlSalesCondition.SalesConditionNo
            SalesConditionNode.AppendChild(SalesConditionNoNode)

            'For Each xmlSelectItem As XmlSelectItem In xmlSelectItemCollection
            SalesConditionNode.AppendChild(SetSelectItem(xmlDoc, xmlSelectItem))
            'Next


            GlErrStepInfo = "SetSalesCondition_End"
            Return SalesConditionNode
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetSelectItem(ByVal xmlDoc As XmlDocument, ByVal xmlSelectItem As XmlSelectItem) As XmlNode
        GlErrStepInfo = "SetSelectItem_Start"
        '20140317 Fujita Upd Start
        Try

            Dim SelectItemNode As XmlNode = xmlDoc.CreateElement("SelectItem")

            Dim ItemNoNode As XmlNode = xmlDoc.CreateElement("ItemNo")
            ItemNoNode.InnerText = xmlSelectItem.ItemNo
            SelectItemNode.AppendChild(ItemNoNode)

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim OtherNode As XmlNode = CreateCDataSection(xmlDoc, "Other", xmlSelectItem.Other)
            'Dim OtherNode As XmlNode = xmlDoc.CreateElement("Other")
            'OtherNode.InnerText = xmlSelectItem.Other
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END
            SelectItemNode.AppendChild(OtherNode)

            GlErrStepInfo = "SetSelectItem_End"
            Return SelectItemNode
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetNegotiationMemo(ByVal xmlDoc As XmlDocument, ByVal xmlNegotiationMemo As XmlNegotiationMemo) As XmlNode
        GlErrStepInfo = "SetNegotiationMemo_Start"
        '20140317 Fujita Upd Start
        Try
            Dim NegotiationMemoNode As XmlNode = xmlDoc.CreateElement("NegotiationMemo")

            Dim CreateDateNode As XmlNode = xmlDoc.CreateElement("CreateDate")
            CreateDateNode.InnerText = xmlNegotiationMemo.CreateDate
            NegotiationMemoNode.AppendChild(CreateDateNode)

            Dim CreateAccountNode As XmlNode = xmlDoc.CreateElement("CreateAccount")
            CreateAccountNode.InnerText = xmlNegotiationMemo.CreateAccount
            NegotiationMemoNode.AppendChild(CreateAccountNode)

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim MemoNode As XmlNode = CreateCDataSection(xmlDoc, "Memo", xmlNegotiationMemo.Memo)
            'Dim MemoNode As XmlNode = xmlDoc.CreateElement("Memo")
            'MemoNode.InnerText = xmlNegotiationMemo.Memo
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END
            NegotiationMemoNode.AppendChild(MemoNode)

            GlErrStepInfo = "SetNegotiationMemo_End"
            Return NegotiationMemoNode
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetFollowUpResult(ByVal xmlDoc As XmlDocument, ByVal xmlFollowUpResult As XmlFollowUpResult) As XmlNode
        GlErrStepInfo = "SetFollowUpResult_Start"
        '20140317 Fujita Upd Start
        Try
            Dim FollowUpResultNode As XmlNode = xmlDoc.CreateElement("FollowUpResult")

            Dim SelectedSeriesNoNode As XmlNode = xmlDoc.CreateElement("SelectedSeriesNo")
            SelectedSeriesNoNode.InnerText = xmlFollowUpResult.SelectedSeriesNo
            FollowUpResultNode.AppendChild(SelectedSeriesNoNode)

            Dim FollowUpResultDateNode As XmlNode = xmlDoc.CreateElement("FollowUpResultDate")
            FollowUpResultDateNode.InnerText = xmlFollowUpResult.FollowUpResultDate
            FollowUpResultNode.AppendChild(FollowUpResultDateNode)

            Dim FollowedBranchCodeNode As XmlNode = xmlDoc.CreateElement("FollowedBranchCode")
            FollowedBranchCodeNode.InnerText = xmlFollowUpResult.FollowedBranchCode
            FollowUpResultNode.AppendChild(FollowedBranchCodeNode)

            Dim FollowedAccountNode As XmlNode = xmlDoc.CreateElement("FollowedAccount")
            FollowedAccountNode.InnerText = xmlFollowUpResult.FollowedAccount
            FollowUpResultNode.AppendChild(FollowedAccountNode)

            GlErrStepInfo = "SetFollowUpResult_1"

            Dim ActivityResultNode As XmlNode = xmlDoc.CreateElement("ActivityResult")
            ActivityResultNode.InnerText = xmlFollowUpResult.ActivityResult
            FollowUpResultNode.AppendChild(ActivityResultNode)

            Dim SameDayBookingFlgNode As XmlNode = xmlDoc.CreateElement("SameDayBookingFlg")
            SameDayBookingFlgNode.InnerText = xmlFollowUpResult.SameDayBookingFlg
            FollowUpResultNode.AppendChild(SameDayBookingFlgNode)

            Dim SeriesCodeNode As XmlNode = xmlDoc.CreateElement("SeriesCode")
            SeriesCodeNode.InnerText = xmlFollowUpResult.SeriesCode
            FollowUpResultNode.AppendChild(SeriesCodeNode)

            Dim GradeCodeNode As XmlNode = xmlDoc.CreateElement("GradeCode")
            GradeCodeNode.InnerText = xmlFollowUpResult.GradeCode
            FollowUpResultNode.AppendChild(GradeCodeNode)

            Dim ExteriorColorCodeNode As XmlNode = xmlDoc.CreateElement("ExteriorColorCode")
            ExteriorColorCodeNode.InnerText = xmlFollowUpResult.ExteriorColorCode
            FollowUpResultNode.AppendChild(ExteriorColorCodeNode)

            Dim InteriorColorCodeNode As XmlNode = xmlDoc.CreateElement("InteriorColorCode")
            InteriorColorCodeNode.InnerText = xmlFollowUpResult.InteriorColorCode
            FollowUpResultNode.AppendChild(InteriorColorCodeNode)

            Dim ModelSuffixNode As XmlNode = xmlDoc.CreateElement("ModelSuffix")
            ModelSuffixNode.InnerText = xmlFollowUpResult.ModelSuffix
            FollowUpResultNode.AppendChild(ModelSuffixNode)

            GlErrStepInfo = "SetFollowUpResult_2"

            Dim GiveUpReasonCodeNode As XmlNode = xmlDoc.CreateElement("GiveUpReasonCode")
            GiveUpReasonCodeNode.InnerText = xmlFollowUpResult.GiveUpReasonCode
            FollowUpResultNode.AppendChild(GiveUpReasonCodeNode)

            Dim GiveUpReasonNode As XmlNode = xmlDoc.CreateElement("GiveUpReason")
            GiveUpReasonNode.InnerText = xmlFollowUpResult.GiveUpReason
            FollowUpResultNode.AppendChild(GiveUpReasonNode)

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim GiveUpMemoNode As XmlNode = CreateCDataSection(xmlDoc, "GiveUpMemo", xmlFollowUpResult.GiveUpMemo)
            'Dim GiveUpMemoNode As XmlNode = xmlDoc.CreateElement("GiveUpMemo")
            'GiveUpMemoNode.InnerText = xmlFollowUpResult.GiveUpMemo
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END
            FollowUpResultNode.AppendChild(GiveUpMemoNode)

            Dim GiveUpMakerCodeNode As XmlNode = xmlDoc.CreateElement("GiveUpMakerCode")
            GiveUpMakerCodeNode.InnerText = xmlFollowUpResult.GiveUpMakerCode
            FollowUpResultNode.AppendChild(GiveUpMakerCodeNode)

            Dim GiveUpMakerNameNode As XmlNode = xmlDoc.CreateElement("GiveUpMakerName")
            GiveUpMakerNameNode.InnerText = xmlFollowUpResult.GiveUpMakerName
            FollowUpResultNode.AppendChild(GiveUpMakerNameNode)

            Dim GiveUpSeriesCodeNode As XmlNode = xmlDoc.CreateElement("GiveUpSeriesCode")
            GiveUpSeriesCodeNode.InnerText = xmlFollowUpResult.GiveUpSeriesCode
            FollowUpResultNode.AppendChild(GiveUpSeriesCodeNode)

            Dim GiveUpSeriesNameNode As XmlNode = xmlDoc.CreateElement("GiveUpSeriesName")
            GiveUpSeriesNameNode.InnerText = xmlFollowUpResult.GiveUpSeriesName
            FollowUpResultNode.AppendChild(GiveUpSeriesNameNode)

            GlErrStepInfo = "SetFollowUpResult_3"

            Dim CreateDateNode As XmlNode = xmlDoc.CreateElement("CreateDate")
            CreateDateNode.InnerText = xmlFollowUpResult.CreateDate
            FollowUpResultNode.AppendChild(CreateDateNode)

            Dim DeleteDateNode As XmlNode = xmlDoc.CreateElement("DeleteDate")
            DeleteDateNode.InnerText = xmlFollowUpResult.DeleteDate
            FollowUpResultNode.AppendChild(DeleteDateNode)

            GlErrStepInfo = "SetFollowUpResult_End"
            Return FollowUpResultNode
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetVehicle(ByVal xmlDoc As XmlDocument, ByVal xmlVehicle As XmlVehicle) As XmlNode
        GlErrStepInfo = "SetVehicle_Start"
        '20140317 Fujita Upd Start
        Try
            Dim VehicleNode As XmlNode = xmlDoc.CreateElement("Vehicle")

            Dim VehicleSeqNoNode As XmlNode = xmlDoc.CreateElement("VehicleSeqNo")
            VehicleSeqNoNode.InnerText = xmlVehicle.VehicleSeqNo
            VehicleNode.AppendChild(VehicleSeqNoNode)

            Dim SeriesCodeNode As XmlNode = xmlDoc.CreateElement("SeriesCode")
            SeriesCodeNode.InnerText = xmlVehicle.SeriesCode
            VehicleNode.AppendChild(SeriesCodeNode)

            Dim SeriesNameNode As XmlNode = xmlDoc.CreateElement("SeriesName")
            SeriesNameNode.InnerText = xmlVehicle.SeriesName
            VehicleNode.AppendChild(SeriesNameNode)

            GlErrStepInfo = "SetVehicle_1"

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim VinNode As XmlNode = CreateCDataSection(xmlDoc, "Vin", xmlVehicle.Vin)
            'Dim VinNode As XmlNode = xmlDoc.CreateElement("Vin")
            'VinNode.InnerText = xmlVehicle.Vin
            VehicleNode.AppendChild(VinNode)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim VehicleRegistrationNumberNode As XmlNode = CreateCDataSection(xmlDoc, "VehicleRegistrationNumber", xmlVehicle.VehicleRegistrationNumber)
            'Dim VehicleRegistrationNumberNode As XmlNode = xmlDoc.CreateElement("VehicleRegistrationNumber")
            'VehicleRegistrationNumberNode.InnerText = xmlVehicle.VehicleRegistrationNumber
            VehicleNode.AppendChild(VehicleRegistrationNumberNode)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            Dim VehicleDeliveryDateNode As XmlNode = xmlDoc.CreateElement("VehicleDeliveryDate")
            VehicleDeliveryDateNode.InnerText = xmlVehicle.VehicleDeliveryDate
            VehicleNode.AppendChild(VehicleDeliveryDateNode)

            '$26 start
            Dim VehicleMileNode As XmlNode = xmlDoc.CreateElement("DistanceCovered")
            VehicleMileNode.InnerText = xmlVehicle.VehicleMile
            VehicleNode.AppendChild(VehicleMileNode)

            Dim VehicleModelYearNode As XmlNode = xmlDoc.CreateElement("ModelYear")
            VehicleModelYearNode.InnerText = xmlVehicle.VehicleModelYear
            VehicleNode.AppendChild(VehicleModelYearNode)
            '$26 end

            GlErrStepInfo = "SetVehicle_End"
            Return VehicleNode
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetCustomer(ByVal xmlDoc As XmlDocument, ByVal xmlCustomer As XmlCustomer) As XmlNode
        GlErrStepInfo = "SetCustomer_Start"
        '20140317 Fujita Upd Start
        Try

            'Dim xmlCustomer As XmlCustomer

            Dim xmlFamilyInformation As New XmlFamilyInformation
            Dim xmlHobby As New XmlHobby

            Dim CustomerNode As XmlNode = xmlDoc.CreateElement("Customer")

            Dim SeqNoNode As XmlNode = xmlDoc.CreateElement("SeqNo")
            SeqNoNode.InnerText = xmlCustomer.SeqNo
            CustomerNode.AppendChild(SeqNoNode)

            Dim CustomerSegmentNode As XmlNode = xmlDoc.CreateElement("CustomerSegment")
            CustomerSegmentNode.InnerText = xmlCustomer.CustomerSegment
            CustomerNode.AppendChild(CustomerSegmentNode)

            Dim NewcustomerIDNode As XmlNode = xmlDoc.CreateElement("NewcustomerID")
            NewcustomerIDNode.InnerText = xmlCustomer.NewcustomerID
            CustomerNode.AppendChild(NewcustomerIDNode)

            Dim CustomerCodeNode As XmlNode = xmlDoc.CreateElement("CustomerCode")
            CustomerCodeNode.InnerText = xmlCustomer.CustomerCode
            CustomerNode.AppendChild(CustomerCodeNode)

            Dim EnquiryCustomerCodeNode As XmlNode = xmlDoc.CreateElement("EnquiryCustomerCode")
            EnquiryCustomerCodeNode.InnerText = xmlCustomer.EnquiryCustomerCode
            CustomerNode.AppendChild(EnquiryCustomerCodeNode)

            GlErrStepInfo = "SetCustomer_1"

            Dim SalesStaffCodeNode As XmlNode = xmlDoc.CreateElement("SalesStaffCode")
            SalesStaffCodeNode.InnerText = xmlCustomer.SalesStaffCode
            CustomerNode.AppendChild(SalesStaffCodeNode)

            Dim CustomerTypeNode As XmlNode = xmlDoc.CreateElement("CustomerType")
            CustomerTypeNode.InnerText = xmlCustomer.CustomerType
            CustomerNode.AppendChild(CustomerTypeNode)

            Dim SubCustomerTypeNode As XmlNode = xmlDoc.CreateElement("SubCustomerType")
            SubCustomerTypeNode.InnerText = xmlCustomer.SubCustomerType
            CustomerNode.AppendChild(SubCustomerTypeNode)

            '$26 start
            Dim OrganizationNameNode As XmlNode = xmlDoc.CreateElement("OrganizationName")
            OrganizationNameNode.InnerText = xmlCustomer.OrganizationName
            CustomerNode.AppendChild(OrganizationNameNode)

            Dim SubCustomerType2Node As XmlNode = xmlDoc.CreateElement("SubCustomerType2")
            SubCustomerType2Node.InnerText = xmlCustomer.SubCustomerType2
            CustomerNode.AppendChild(SubCustomerType2Node)
            '$26 end

            Dim SocialIDNode As XmlNode = xmlDoc.CreateElement("SocialID")
            SocialIDNode.InnerText = xmlCustomer.SocialID
            CustomerNode.AppendChild(SocialIDNode)

            GlErrStepInfo = "SetCustomer_2"

            Dim SexNode As XmlNode = xmlDoc.CreateElement("Sex")
            SexNode.InnerText = xmlCustomer.Sex
            CustomerNode.AppendChild(SexNode)

            Dim BirthDayNode As XmlNode = xmlDoc.CreateElement("BirthDay")
            BirthDayNode.InnerText = xmlCustomer.BirthDay
            CustomerNode.AppendChild(BirthDayNode)

            Dim NameTitleCodeNode As XmlNode = xmlDoc.CreateElement("NameTitleCode")
            NameTitleCodeNode.InnerText = xmlCustomer.NameTitleCode
            CustomerNode.AppendChild(NameTitleCodeNode)

            Dim NameTitleNode As XmlNode = xmlDoc.CreateElement("NameTitle")
            NameTitleNode.InnerText = xmlCustomer.NameTitle
            CustomerNode.AppendChild(NameTitleNode)

            GlErrStepInfo = "SetCustomer_3"

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim Name1Node As XmlNode = CreateCDataSection(xmlDoc, "Name1", xmlCustomer.Name1)
            'Dim Name1Node As XmlNode = xmlDoc.CreateElement("Name1")
            'Name1Node.InnerText = xmlCustomer.Name1
            CustomerNode.AppendChild(Name1Node)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim Name2Node As XmlNode = CreateCDataSection(xmlDoc, "Name2", xmlCustomer.Name2)
            'Dim Name2Node As XmlNode = xmlDoc.CreateElement("Name2")
            'Name2Node.InnerText = xmlCustomer.Name2
            CustomerNode.AppendChild(Name2Node)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim Name3Node As XmlNode = CreateCDataSection(xmlDoc, "Name3", xmlCustomer.Name3)
            'Dim Name3Node As XmlNode = xmlDoc.CreateElement("Name3")
            'Name3Node.InnerText = xmlCustomer.Name3
            CustomerNode.AppendChild(Name3Node)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim SubName1Node As XmlNode = CreateCDataSection(xmlDoc, "SubName1", xmlCustomer.SubName1)
            'Dim SubName1Node As XmlNode = xmlDoc.CreateElement("SubName1")
            'SubName1Node.InnerText = xmlCustomer.SubName1
            CustomerNode.AppendChild(SubName1Node)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim CompanyNameNode As XmlNode = CreateCDataSection(xmlDoc, "CompanyName", xmlCustomer.CompanyName)
            'Dim CompanyNameNode As XmlNode = xmlDoc.CreateElement("CompanyName")
            'CompanyNameNode.InnerText = xmlCustomer.CompanyName
            CustomerNode.AppendChild(CompanyNameNode)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim EmployeeNameNode As XmlNode = CreateCDataSection(xmlDoc, "EmployeeName", xmlCustomer.EmployeeName)
            'Dim EmployeeNameNode As XmlNode = xmlDoc.CreateElement("EmployeeName")
            'EmployeeNameNode.InnerText = xmlCustomer.EmployeeName
            CustomerNode.AppendChild(EmployeeNameNode)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim EmployeeDepartmentNode As XmlNode = CreateCDataSection(xmlDoc, "EmployeeDepartment", xmlCustomer.EmployeeDepartment)
            'Dim EmployeeDepartmentNode As XmlNode = xmlDoc.CreateElement("EmployeeDepartment")
            'EmployeeDepartmentNode.InnerText = xmlCustomer.EmployeeDepartment
            CustomerNode.AppendChild(EmployeeDepartmentNode)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim EmployeePositionNode As XmlNode = CreateCDataSection(xmlDoc, "EmployeePosition", xmlCustomer.EmployeePosition)
            'Dim EmployeePositionNode As XmlNode = xmlDoc.CreateElement("EmployeePosition")
            'EmployeePositionNode.InnerText = xmlCustomer.EmployeePosition
            CustomerNode.AppendChild(EmployeePositionNode)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            GlErrStepInfo = "SetCustomer_4"

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim AddressNode As XmlNode = CreateCDataSection(xmlDoc, "Address", xmlCustomer.Address)
            'Dim AddressNode As XmlNode = xmlDoc.CreateElement("Address")
            'AddressNode.InnerText = xmlCustomer.Address
            CustomerNode.AppendChild(AddressNode)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim Address1Node As XmlNode = CreateCDataSection(xmlDoc, "Address1", xmlCustomer.Address1)
            'Dim Address1Node As XmlNode = xmlDoc.CreateElement("Address1")
            'Address1Node.InnerText = xmlCustomer.Address1
            CustomerNode.AppendChild(Address1Node)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            'takeda_update_start_20140425
            'GlErrStepInfo = "@@@DataCheck(SetCustomer)"
            'GlErrStepInfo = "(XML)AddressNode.InnerText"
            'GlErrStepInfo = AddressNode.InnerText
            'GlErrStepInfo = "(XML)Address1Node.InnerText"
            'GlErrStepInfo = Address1Node.InnerText
            'takeda_update_end_20140425

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim Address2Node As XmlNode = CreateCDataSection(xmlDoc, "Address2", xmlCustomer.Address2)
            'Dim Address2Node As XmlNode = xmlDoc.CreateElement("Address2")
            'Address2Node.InnerText = xmlCustomer.Address2
            CustomerNode.AppendChild(Address2Node)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim Address3Node As XmlNode = CreateCDataSection(xmlDoc, "Address3", xmlCustomer.Address3)
            'Dim Address3Node As XmlNode = xmlDoc.CreateElement("Address3")
            'Address3Node.InnerText = xmlCustomer.Address3
            CustomerNode.AppendChild(Address3Node)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim DomicileNode As XmlNode = CreateCDataSection(xmlDoc, "Domicile", xmlCustomer.Domicile)
            'Dim DomicileNode As XmlNode = xmlDoc.CreateElement("Domicile")
            'DomicileNode.InnerText = xmlCustomer.Domicile
            CustomerNode.AppendChild(DomicileNode)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim CountryNode As XmlNode = CreateCDataSection(xmlDoc, "Country", xmlCustomer.Country)
            'Dim CountryNode As XmlNode = xmlDoc.CreateElement("Country")
            'CountryNode.InnerText = xmlCustomer.Country
            CustomerNode.AppendChild(CountryNode)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim ZipCodeNode As XmlNode = CreateCDataSection(xmlDoc, "ZipCode", xmlCustomer.ZipCode)
            'Dim ZipCodeNode As XmlNode = xmlDoc.CreateElement("ZipCode")
            'ZipCodeNode.InnerText = xmlCustomer.ZipCode
            CustomerNode.AppendChild(ZipCodeNode)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            GlErrStepInfo = "SetCustomer_5"

            Dim StateCodeNode As XmlNode = xmlDoc.CreateElement("StateCode")
            StateCodeNode.InnerText = xmlCustomer.StateCode
            CustomerNode.AppendChild(StateCodeNode)

            Dim StateNameNode As XmlNode = xmlDoc.CreateElement("StateName")
            StateNameNode.InnerText = xmlCustomer.StateName
            CustomerNode.AppendChild(StateNameNode)

            Dim DistrictCodeNode As XmlNode = xmlDoc.CreateElement("DistrictCode")
            DistrictCodeNode.InnerText = xmlCustomer.DistrictCode
            CustomerNode.AppendChild(DistrictCodeNode)

            Dim DistrictNameNode As XmlNode = xmlDoc.CreateElement("DistrictName")
            DistrictNameNode.InnerText = xmlCustomer.DistrictName
            CustomerNode.AppendChild(DistrictNameNode)

            Dim CityCodeNode As XmlNode = xmlDoc.CreateElement("CityCode")
            CityCodeNode.InnerText = xmlCustomer.CityCode
            CustomerNode.AppendChild(CityCodeNode)

            Dim CityNameNode As XmlNode = xmlDoc.CreateElement("CityName")
            CityNameNode.InnerText = xmlCustomer.CityName
            CustomerNode.AppendChild(CityNameNode)

            Dim LocationCodeNode As XmlNode = xmlDoc.CreateElement("LocationCode")
            LocationCodeNode.InnerText = xmlCustomer.LocationCode
            CustomerNode.AppendChild(LocationCodeNode)

            Dim LocationNameNode As XmlNode = xmlDoc.CreateElement("LocationName")
            LocationNameNode.InnerText = xmlCustomer.LocationName
            CustomerNode.AppendChild(LocationNameNode)

            GlErrStepInfo = "SetCustomer_6"

            Dim TelNumberNode As XmlNode = xmlDoc.CreateElement("TelNumber")
            TelNumberNode.InnerText = xmlCustomer.TelNumber
            CustomerNode.AppendChild(TelNumberNode)

            Dim FaxNumberNode As XmlNode = xmlDoc.CreateElement("FaxNumber")
            FaxNumberNode.InnerText = xmlCustomer.FaxNumber
            CustomerNode.AppendChild(FaxNumberNode)

            Dim MobileNode As XmlNode = xmlDoc.CreateElement("Mobile")
            MobileNode.InnerText = xmlCustomer.Mobile
            CustomerNode.AppendChild(MobileNode)

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim EMail1Node As XmlNode = CreateCDataSection(xmlDoc, "EMail1", xmlCustomer.EMail1)
            'Dim EMail1Node As XmlNode = xmlDoc.CreateElement("EMail1")
            'EMail1Node.InnerText = xmlCustomer.EMail1
            CustomerNode.AppendChild(EMail1Node)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim EMail2Node As XmlNode = CreateCDataSection(xmlDoc, "EMail2", xmlCustomer.EMail2)
            'Dim EMail2Node As XmlNode = xmlDoc.CreateElement("EMail2")
            'EMail2Node.InnerText = xmlCustomer.EMail2
            CustomerNode.AppendChild(EMail2Node)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            Dim BusinessTelNumberNode As XmlNode = xmlDoc.CreateElement("BusinessTelNumber")
            BusinessTelNumberNode.InnerText = xmlCustomer.BusinessTelNumber
            CustomerNode.AppendChild(BusinessTelNumberNode)

            GlErrStepInfo = "SetCustomer_7"

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim IncomeNode As XmlNode = CreateCDataSection(xmlDoc, "Income", xmlCustomer.Income)
            'Dim IncomeNode As XmlNode = xmlDoc.CreateElement("Income")
            'IncomeNode.InnerText = xmlCustomer.Income
            CustomerNode.AppendChild(IncomeNode)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            Dim ContactTimeNode As XmlNode = xmlDoc.CreateElement("ContactTime")
            ContactTimeNode.InnerText = xmlCustomer.ContactTime
            CustomerNode.AppendChild(ContactTimeNode)

            Dim OccupationIDNode As XmlNode = xmlDoc.CreateElement("OccupationID")
            OccupationIDNode.InnerText = xmlCustomer.OccupationID
            CustomerNode.AppendChild(OccupationIDNode)

            Dim OccupationNode As XmlNode = xmlDoc.CreateElement("Occupation")
            OccupationNode.InnerText = xmlCustomer.Occupation
            CustomerNode.AppendChild(OccupationNode)

            Dim DefaultLangNode As XmlNode = xmlDoc.CreateElement("DefaultLang")
            DefaultLangNode.InnerText = xmlCustomer.DefaultLang
            CustomerNode.AppendChild(DefaultLangNode)

            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
            Dim CustomerMemoNode As XmlNode = CreateCDataSection(xmlDoc, "CustomerMemo", xmlCustomer.CustomerMemo)
            'Dim CustomerMemoNode As XmlNode = xmlDoc.CreateElement("CustomerMemo")
            'CustomerMemoNode.InnerText = xmlCustomer.CustomerMemo
            CustomerNode.AppendChild(CustomerMemoNode)
            '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

            GlErrStepInfo = "SetCustomer_8(FamilyInfomationData)"

            Dim FamilyInfomationData As New IC3802801FamilyInfomationDataTable
            FamilyInfomationData = IC3802801TableAdapter.GetFamilyInfomation(xmlCustomer.CustomerID)
            If (FamilyInfomationData IsNot Nothing) Then
                Dim familyInformationDataTable As New FamilyInformationDataTable
                For Each familyInformationRow As IC3802801FamilyInfomationRow In FamilyInfomationData
                    Dim familyInformationRowDisplay As FamilyInformationRow
                    familyInformationRowDisplay = CType(familyInformationDataTable.NewRow(), IC3802801DataSet.FamilyInformationRow)
                    familyInformationRowDisplay.FamilyNo = familyInformationRow.FAMILYNO
                    familyInformationRowDisplay.FamilyCode = familyInformationRow.FAMILYRELATIONSHIPNO
                    familyInformationRowDisplay.FamilyCodeName = familyInformationRow.FAMILYRELATIONSHIP
                    If (familyInformationRow.BIRTHDAY.Trim() <> "") Then
                        familyInformationRowDisplay.BirthDay = ChangeDefaultDate(Convert.ToDateTime(familyInformationRow.BIRTHDAY), CstStrDefaultDate)
                    Else
                        familyInformationRowDisplay.BirthDay = ""
                    End If

                    CustomerNode.AppendChild(SetFamilyInformation(xmlDoc, SetXmlFamilyInformation(familyInformationRowDisplay)))
                Next
            End If

            GlErrStepInfo = "SetCustomer_9(HobbyData)"

            Dim HobbyData As New IC3802801HobbyDataTable
            HobbyData = IC3802801TableAdapter.GetHobby(xmlCustomer.CustomerID)
            Dim hobbyDataTableDisplay As New HobbyDataTable
            If (HobbyData IsNot Nothing) Then
                For Each hobbyRowData As IC3802801HobbyRow In HobbyData
                    Dim hobbyRowDisplay As HobbyRow
                    hobbyRowDisplay = CType(hobbyDataTableDisplay.NewRow(), IC3802801DataSet.HobbyRow)
                    hobbyRowDisplay.HobbyCode = hobbyRowData.HOBBYNO
                    hobbyRowDisplay.HobbyName = hobbyRowData.HOBBY
                    CustomerNode.AppendChild(SetHobby(xmlDoc, SetXmlHobby(hobbyRowDisplay)))
                Next
            End If

            'CustomerNode.AppendChild(FamilyInformation(xmlDoc, xmlFamilyInformation))
            'CustomerNode.AppendChild(Hobby(xmlDoc, xmlHobby))

            GlErrStepInfo = "SetCustomer_10"

            Dim CreateDateNode As XmlNode = xmlDoc.CreateElement("CreateDate")
            CreateDateNode.InnerText = xmlCustomer.CreateDate
            CustomerNode.AppendChild(CreateDateNode)

            Dim UpdateDateNode As XmlNode = xmlDoc.CreateElement("UpdateDate")
            UpdateDateNode.InnerText = xmlCustomer.UpdateDate
            CustomerNode.AppendChild(UpdateDateNode)

            Dim DeleteDateNode As XmlNode = xmlDoc.CreateElement("DeleteDate")
            DeleteDateNode.InnerText = xmlCustomer.DeleteDate
            CustomerNode.AppendChild(DeleteDateNode)

            GlErrStepInfo = "SetCustomer_End"
            Return CustomerNode
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetFamilyInformation(ByVal xmlDoc As XmlDocument, ByVal xmlFamilyInformation As XmlFamilyInformation) As XmlNode
        GlErrStepInfo = "SetFamilyInformation_Start"
        '20140317 Fujita Upd Start
        Try
            Dim FamilyInformationNode As XmlNode = xmlDoc.CreateElement("FamilyInformation")

            Dim FamilyNoNode As XmlNode = xmlDoc.CreateElement("FamilyNo")
            FamilyNoNode.InnerText = xmlFamilyInformation.FamilyNo
            FamilyInformationNode.AppendChild(FamilyNoNode)

            Dim FamilyCodeNode As XmlNode = xmlDoc.CreateElement("FamilyCode")
            FamilyCodeNode.InnerText = xmlFamilyInformation.FamilyCode
            FamilyInformationNode.AppendChild(FamilyCodeNode)

            Dim FamilyCodeNameNode As XmlNode = xmlDoc.CreateElement("FamilyCodeName")
            FamilyCodeNameNode.InnerText = xmlFamilyInformation.FamilyCodeName
            FamilyInformationNode.AppendChild(FamilyCodeNameNode)

            Dim BirthDayNode As XmlNode = xmlDoc.CreateElement("BirthDay")
            BirthDayNode.InnerText = xmlFamilyInformation.BirthDay
            FamilyInformationNode.AppendChild(BirthDayNode)

            GlErrStepInfo = "SetFamilyInformation_End"
            Return FamilyInformationNode
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetHobby(ByVal xmlDoc As XmlDocument, ByVal xmlHobby As XmlHobby) As XmlNode
        GlErrStepInfo = "SetHobby_Start"
        '20140317 Fujita Upd Start
        Try
            Dim HobbyNode As XmlNode = xmlDoc.CreateElement("Hobby")

            Dim CreateDateNode As XmlNode = xmlDoc.CreateElement("HobbyCode")
            CreateDateNode.InnerText = xmlHobby.HobbyCode
            HobbyNode.AppendChild(CreateDateNode)

            Dim UpdateDateNode As XmlNode = xmlDoc.CreateElement("HobbyName")
            UpdateDateNode.InnerText = xmlHobby.HobbyName
            HobbyNode.AppendChild(UpdateDateNode)

            GlErrStepInfo = "SetHobby_End"
            Return HobbyNode
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlSelectedSeriesNode(ByVal selectedSeriesRow As IC3802801SelectedSeriesRow) As XmlSelectedSeries
        GlErrStepInfo = "SetXmlSelectedSeriesNode_Start"
        '20140317 Fujita Upd Start
        Try
            Dim xmlSelectedSeries As New XmlSelectedSeries

            xmlSelectedSeries.SelectedSeriesNo = selectedSeriesRow.PREF_VCL_SEQ
            'ISSUE-0003,ISSUE-0012_20130219_by_chatchai_Start
            If (selectedSeriesRow.SALES_PROSPECT_CD <> " ") Then
                xmlSelectedSeries.PreferredVehicleFlg = "1"
            Else
                xmlSelectedSeries.PreferredVehicleFlg = "0"
            End If
            'ISSUE-0003,ISSUE-0012_20130219_by_chatchai_End
            xmlSelectedSeries.SeriesCode = selectedSeriesRow.MODEL_CD
            xmlSelectedSeries.GradeCode = EditLength(selectedSeriesRow.GRADE_CD, 1, 20, CstStrTypeString)
            xmlSelectedSeries.ExteriorColorCode = EditLength(selectedSeriesRow.BODYCLR_CD, 1, 6, CstStrTypeString)
            xmlSelectedSeries.InteriorColorCode = EditLength(selectedSeriesRow.INTERIORCLR_CD, 1, 7, CstStrTypeString)
            xmlSelectedSeries.ModelSuffix = EditLength(selectedSeriesRow.SUFFIX_CD, 1, 7, CstStrTypeString)
            xmlSelectedSeries.Quantity = selectedSeriesRow.PREF_AMOUNT
            '20140312 Fujita Upd Start
            GlErrStepInfo = "SetXmlSelectedSeriesNode_1"
            If (selectedSeriesRow.EST_AMOUNT.ToString().Equals("0")) Then
                xmlSelectedSeries.QuotationPrice = " "
            Else
                'xmlSelectedSeries.QuotationPrice = editLength(selectedSeriesRow.EST_AMOUNT, 1, 10, CstStrTypeString)
                xmlSelectedSeries.QuotationPrice = EditLength(selectedSeriesRow.EST_AMOUNT, 1, 10, CstStrTypeString)
            End If
            '20140312 Fujita Upd End

            GlErrStepInfo = "SetXmlSelectedSeriesNode_End"
            Return xmlSelectedSeries
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlActionNode(ByVal ActionSeq As DataRowView,
                                            ByVal FllwUpBoxSalesData As IC3802801DataSet.IC3802801GetFllwUpBoxSalesDataTable,
                                            ByVal SalesActData As IC3802801DataSet.IC3802801SalesActionDataTable,
                                            ByVal ActionMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
                                            ByVal ActionData As IC3802801DataSet.IC3802801ActionDataTable,
                                            ByVal i As Long
                                            ) As XmlAction
        GlErrStepInfo = "SetXmlActionNode_Start"
        Dim ActionCode As String = ""
        Dim ActionName As String = ""
        Dim ActionAccount As String = ""
        Dim StartActionDate As String = ""
        Dim ActionDate As String = ""
        Dim ActionBranchCode As String = ""
        Dim PlannedActionDate As String = ""
        Dim ActionMemo As String = ""
        Dim DateEndTime As Date
        '20140319 Fujita Zantei Add Start
        Dim blnHantei As Boolean = False
        Dim blnHantei2 As Boolean
        '20140319 Fujita Zantei Add End

        Dim DmsCodeMapRow As IC3802801DataSet.IC3802801DmsCodeMapRow

        '20140317 Fujita Upd Start
        Try
            If (ActionSeq("RELATION_ACT_TYPE") = 1) Then
                'ISSUE-IT-2-1_by_takeda(ADD(RSLT_DATETIME, SCHE_CONTACT_MTD)
                Dim result = From TB_T_ACTIVITY In ActionData
                 Where ActionSeq("RELATION_ACT_ID") = TB_T_ACTIVITY.ACT_ID
                  Select New With {
                        TB_T_ACTIVITY.ACT_ID,
                        TB_T_ACTIVITY.RSLT_CONTACT_MTD,
                        TB_T_ACTIVITY.RSLT_FLG,
                        TB_T_ACTIVITY.RSLT_DLR_CD,
                        TB_T_ACTIVITY.RSLT_BRN_CD,
                        TB_T_ACTIVITY.RSLT_STF_CD,
                        TB_T_ACTIVITY.RSLT_DATETIME,
                        TB_T_ACTIVITY.ROW_CREATE_ACCOUNT,
                        TB_T_ACTIVITY.SCHE_STF_CD,
                        TB_T_ACTIVITY.SCHE_DATEORTIME,
                        TB_T_ACTIVITY.SCHE_CONTACT_MTD,
                        TB_T_ACTIVITY.SCHE_DLR_CD,
                        TB_T_ACTIVITY.SCHE_BRN_CD()
                  }
                GlErrStepInfo = "SetXmlActionNode_1"
                'GlErrStepInfo = "ACT_ID:" + result.ToList()(0).ACT_ID.ToString()
                '20140311 Fujita zantei Upd Start 
                'Dim resultActionDate = From FllwUpBox In FllwUpBoxSalesData
                ' Where ActionSeq("RELATION_ACT_ID") = FllwUpBox.SALES_SEQNO
                '  Select New With {
                '        FllwUpBox.STARTTIME,
                '        FllwUpBox.ENDTIME
                '  }
                'GlErrStepInfo = "FllwUpBoxSalesData.Rows.Count:" + FllwUpBoxSalesData.Rows.Count
                GlErrStepInfo = "SetXmlActionNode_1"
                Dim FllwUpBoxSalesDatar As IC3802801DataSet.IC3802801GetFllwUpBoxSalesRow
                Dim FllwUpBoxSalesDatar2 As IC3802801DataSet.IC3802801GetFllwUpBoxSalesRow
                Dim indexCnt As Long = 0
                blnHantei2 = False
                If FllwUpBoxSalesData.Rows.Count <> 0 Then
                    If result.ToList()(0).RSLT_FLG = "1" Then
                        '20140319 Fujita Zantei Add Start
                        'If i <= FllwUpBoxSalesData.Rows.Count - 1 Then
                        '20140319 Fujita Zantei Add End
                        For indexCnt = 0 To FllwUpBoxSalesData.Rows.Count - 1 Step 1
                            FllwUpBoxSalesDatar2 = FllwUpBoxSalesData.Rows(indexCnt)
                            GlErrStepInfo = "SetXmlActionNode_1_1"
                            If Date.Parse(FllwUpBoxSalesDatar2.STARTTIME) = result.ToList()(0).RSLT_DATETIME Then
                                FllwUpBoxSalesDatar = FllwUpBoxSalesData.Rows(indexCnt)
                                blnHantei2 = True
                                Exit For
                            End If
                        Next
                        'Not Found?
                        If blnHantei2 = False Then
                            blnHantei = True
                        End If
                        '20140319 Fujita Zantei Add Start
                        'Else
                        'blnHantei = True
                        '20140319 Fujita Zantei Add End
                        'End If
                    End If
                End If
                '20140311 Fujita zantei Upd End 
                'ActionCode,ActionName
                If (result.Count > 0) Then
                    GlErrStepInfo = "SetXmlActionNode_2"
                    'GlErrStepInfo = "RSLT_CONTACT_MTD:" + result.ToList()(0).RSLT_CONTACT_MTD.ToString()
                    'GlErrStepInfo = "RSLT_FLG:" + result.ToList()(0).RSLT_FLG.ToString()

                    If (GlActivityJudgeFlg = "1") Then

                        If (result.ToList()(0).RSLT_FLG = 1) Then
                            If (ChangeActCd(result.ToList()(0).RSLT_CONTACT_MTD, "1", "DMS").Rows.Count > 0) Then
                                DmsCodeMapRow = ChangeActCd(result.ToList()(0).RSLT_CONTACT_MTD, "1", "DMS").Rows(0)
                                'ISSUE-IT2-1_by_takeda_start
                                'ActionCode = DmsCodeMapRow.ICROP_CD_1
                                'ActionName = DmsCodeMapRow.ICROP_CD_2
                                ActionCode = DmsCodeMapRow.DMS_CD_1
                                ActionName = DmsCodeMapRow.DMS_CD_2
                                GlErrStepInfo = "SetXmlActionNode_2_1"
                                'GlErrStepInfo = "ActionCode:" + ActionCode
                                'ISSUE-IT2-1_by_takeda_end
                                '20140317 Fujita Add Start
                            Else
                                GlErrStepInfo = "Change RSLT_CONTACT_MTD：" + result.ToList()(0).RSLT_CONTACT_MTD + " Not Found"
                                Throw New Exception("Change RSLT_CONTACT_MTD：" + result.ToList()(0).RSLT_CONTACT_MTD + " Not Found")
                                '20140317 Fujita Add End
                            End If
                        End If

                        GlErrStepInfo = "SetXmlActionNode_3"
                        'GlErrStepInfo = "SCHE_CONTACT_MTD:" + result.ToList()(0).SCHE_CONTACT_MTD.ToString()
                        If (result.ToList()(0).RSLT_FLG = 0) Then
                            '20140228 ISSUE-IT2-1_by_fujita_start
                            'If (changeActCd(result.ToList()(0).RSLT_CONTACT_MTD, "1", "DMS").Rows.Count > 0) Then
                            '    DmsCodeMapRow = changeActCd(result.ToList()(0).RSLT_CONTACT_MTD, "1", "DMS").Rows(0)
                            If (ChangeActCd(result.ToList()(0).SCHE_CONTACT_MTD, "1", "DMS").Rows.Count > 0) Then
                                DmsCodeMapRow = ChangeActCd(result.ToList()(0).SCHE_CONTACT_MTD, "1", "DMS").Rows(0)
                                '20140228 ISSUE-IT2-1_by_fujita_end
                                'ISSUE-IT2-1_by_takeda_start
                                'ActionCode = DmsCodeMapRow.ICROP_CD_1
                                'ActionName = DmsCodeMapRow.ICROP_CD_2
                                ActionCode = DmsCodeMapRow.DMS_CD_1
                                ActionName = DmsCodeMapRow.DMS_CD_2
                                GlErrStepInfo = "SetXmlActionNode_3_1"
                                'GlErrStepInfo = "ActionCode:" + ActionCode
                                'ISSUE-IT2-1_by_takeda_end
                                '20140317 Fujita Add Start
                            Else
                                GlErrStepInfo = "Change SCHE_CONTACT_MTD：" + result.ToList()(0).SCHE_CONTACT_MTD + " Not Found"
                                Throw New Exception("Change SCHE_CONTACT_MTD：" + result.ToList()(0).SCHE_CONTACT_MTD + " Not Found")
                                '20140317 Fujita Add End
                            End If
                        End If
                    End If

                    GlErrStepInfo = "SetXmlActionNode_4"
                    'ActionMemo
                    Dim resultMemoData = From memoData In ActionMemoData
                         Where memoData.RELATION_ACT_ID = result.ToList()(0).ACT_ID And memoData.RELATION_ACT_TYPE = "2"
                         Order By memoData.ACT_MEMO_ID Descending
                          Select New With {
                            memoData.ACT_MEMO_ID,
                            memoData.CST_MEMO
                          }
                    GlErrStepInfo = "SetXmlActionNode_5"
                    If (resultMemoData.ToList().Count > 0) Then
                        ActionMemo = resultMemoData.ToList()(0).CST_MEMO.ToString()
                    Else
                        ActionMemo = ""
                    End If

                    GlErrStepInfo = "SetXmlActionNode_6"
                    'PlannedActionDate
                    If (GlActivityJudgeFlg = "1") Then
                        PlannedActionDate = ChangeDefaultDate(result.ToList()(0).SCHE_DATEORTIME, CstStrDefaultDate)
                    ElseIf (result.ToList()(0).RSLT_FLG = "0") Then
                        PlannedActionDate = ChangeDefaultDate(result.ToList()(0).SCHE_DATEORTIME, CstStrDefaultDate)
                    End If

                    GlErrStepInfo = "SetXmlActionNode_7"
                    'StartActionDate,ActionDate
                    'ISUUE-0016&ISUUE-0017_20140219_by_takeda_start
                    If (GlActivityJudgeFlg = "1") Then                                                   'ACTIVITY_JUDGE_FLG
                        If (result.ToList()(0).RSLT_FLG = "0") Then
                            GlErrStepInfo = "SetXmlActionNode_7A"
                            StartActionDate = ""
                            ActionDate = ""
                            SALES_ACT_STARTTIME = StartActionDate
                            SALES_ACT_ENDTIME = ActionDate
                        ElseIf (result.ToList()(0).RSLT_FLG = "1") Then
                            'GlErrStepInfo = "FllwUpBoxSalesData.Rows.Count:" + FllwUpBoxSalesData.Rows.Count.ToString()
                            '20140311 Fujita zantei Upd Start 
                            GlErrStepInfo = "SetXmlActionNode_7B"
                            If FllwUpBoxSalesData.Rows.Count <> 0 Then
                                GlErrStepInfo = "SetXmlActionNode_7B_1"
                                'If j = 1 Then
                                '20140319 Fujita Zantei Add Start
                                If blnHantei Then
                                    StartActionDate = ChangeDefaultDate(result.ToList()(0).RSLT_DATETIME, CstStrDefaultDate)
                                    SALES_ACT_ENDTIME = ""
                                Else
                                    '20140319 Fujita Zantei Add End
                                    'GlErrStepInfo = "RSLT_DATETIME:" + result.ToList()(0).RSLT_DATETIME.ToString()
                                    StartActionDate = ChangeDefaultDate(result.ToList()(0).RSLT_DATETIME, CstStrDefaultDate)
                                    '20140311 Fujita zantei Upd Start 
                                    'GlErrStepInfo = "ENDTIME"
                                    GlErrStepInfo = "SetXmlActionNode_7B_1_1"
                                    If FllwUpBoxSalesData.Rows.Count > 0 Then
                                        If FllwUpBoxSalesDatar.ENDTIME.Trim <> "" Then
                                            DateEndTime = Date.Parse(FllwUpBoxSalesDatar.ENDTIME)
                                            ActionDate = ChangeDefaultDate(DateEndTime, CstStrDefaultDate)
                                            SALES_ACT_ENDTIME = ActionDate
                                        End If
                                        '20140311 Fujita zantei Upd End 
                                        SALES_ACT_STARTTIME = StartActionDate
                                        '20140319 Fujita Zantei Add Start
                                        'GlErrStepInfo=FllwUpBoxSalesDatar.ENDTIME
                                    End If
                                    '20140311 Fujita zantei Upd End 
                                    SALES_ACT_STARTTIME = StartActionDate
                                    '20140319 Fujita Zantei Add Start
                                End If
                                '20140319 Fujita Zantei Add End
                                'Else
                                '    SALES_ACT_STARTTIME = " "
                                '    SALES_ACT_ENDTIME = " "
                                'End If
                                'StartActionDate = changeDefaultDate(Date.Parse(FllwUpBoxSalesData.Rows(0)), CstStrDefaultDate)
                                'ActionDate = changeDefaultDate(Date.Parse(resultActionDate.ToList()(0).ENDTIME), CstStrDefaultDate)
                            Else
                                GlErrStepInfo = "SetXmlActionNode_7B_2"
                                StartActionDate = ChangeDefaultDate(result.ToList()(0).RSLT_DATETIME, CstStrDefaultDate)
                                SALES_ACT_ENDTIME = ""
                            End If
                            '20140311 Fujita zantei Upd End

                            'takeda_update_start_zantei_20140401
                            '上記でActionDate(SALES_ACT_ENDTIME)が求められない場合、
                            '開始時間の2時間後を終了時間に設定
                            If (ActionDate = "") Then
                                GlErrStepInfo = "SetXmlActionNode_7B_3(zantei)"
                                'GlErrStepInfo = "StartActionDate(before):" + StartActionDate.ToString()
                                Dim StartActionDateEdit = ""
                                If (StartActionDate.Length = 19) Then
                                    GlErrStepInfo = "SetXmlActionNode_7B_3_1"
                                    'YYYY/MM/DD HH24:MM:SSに変換
                                    StartActionDateEdit = StartActionDate.Substring(6, 4) + "/" + StartActionDate.Substring(3, 2) + "/" + StartActionDate.Substring(0, 2) + " " + StartActionDate.Substring(11)
                                ElseIf (StartActionDate.Length = 10) Then
                                    GlErrStepInfo = "SetXmlActionNode_7B_3_2"
                                    'YYYY/MM/DDに変換
                                    StartActionDateEdit = StartActionDate.Substring(6, 4) + "/" + StartActionDate.Substring(3, 2) + "/" + StartActionDate.Substring(0, 2)
                                Else
                                    GlErrStepInfo = "SetXmlActionNode_7B_3_3"
                                    StartActionDateEdit = StartActionDate
                                End If
                                'GlErrStepInfo = "StartActionDate(edit):" + StartActionDateEdit.ToString()
                                Dim dtStartActionDate As DateTime
                                If (Date.TryParse(StartActionDateEdit, dtStartActionDate)) Then
                                    GlErrStepInfo = "SetXmlActionNode_7B_3_4"
                                    'GlErrStepInfo = "StartActionDate(after):" + dtStartActionDate.ToString()
                                    ActionDate = EditDateFormat(dtStartActionDate.AddHours(2))
                                End If
                            End If

                            'GlErrStepInfo = "StartActionDate:" + StartActionDate.ToString()
                            'GlErrStepInfo = "ActionDate(endtime):" + ActionDate.ToString()
                            'takeda_update_end_zantei_20140401
                        End If
                    End If

                    GlErrStepInfo = "SetXmlActionNode_8"
                    'ActionBranchCode
                    If (GlActivityJudgeFlg = "1") Then                                                   'ACTIVITY_JUDGE_FLG
                        '20140318 Fujita Upd 
                        If (result.ToList()(0).RSLT_FLG = "1") Then
                            'GlErrStepInfo = "result.ToList()(0).RSLT_DLR_CD:" + result.ToList()(0).RSLT_DLR_CD.ToString()
                            'GlErrStepInfo = "result.ToList()(0).RSLT_BRN_CD:" + result.ToList()(0).RSLT_BRN_CD.ToString()
                            '20140318 Fujita Upd 
                            ActionBranchCode = EditLength(ChangeBranchCd(result.ToList()(0).RSLT_DLR_CD, result.ToList()(0).RSLT_BRN_CD, "DMS"), 1, 20, CstStrTypeString)
                        End If
                    End If


                    GlErrStepInfo = "SetXmlActionNode_9"
                    If (result.ToList()(0).RSLT_FLG = "0") Then                                      'Next activity data
                        '20140318 Fujita Upd Test
                        'GlErrStepInfo = "result.ToList()(0).SCHE_DLR_CD:" + result.ToList()(0).SCHE_DLR_CD.ToString()
                        'GlErrStepInfo = "result.ToList()(0).SCHE_BRN_CD:" + result.ToList()(0).SCHE_BRN_CD.ToString()
                        '20140318 Fujita Upd Test
                        ActionBranchCode = EditLength(ChangeBranchCd(result.ToList()(0).SCHE_DLR_CD, result.ToList()(0).SCHE_BRN_CD, "DMS"), 1, 20, CstStrTypeString)
                    End If

                    GlErrStepInfo = "SetXmlActionNode_10"
                    'ActionAccount
                    If (GlActivityJudgeFlg = "1") Then                                                  'ACTIVITY_JUDGE_FLG
                        ActionAccount = result.ToList()(0).RSLT_STF_CD
                    End If

                    GlErrStepInfo = "SetXmlActionNode_11"
                    If (result.ToList()(0).RSLT_FLG = "0") Then                                      'Next activity data
                        ActionAccount = result.ToList()(0).SCHE_STF_CD
                    End If

                End If
                GlErrStepInfo = "SetXmlActionNode_11_2"

            ElseIf (ActionSeq("RELATION_ACT_TYPE") = 2) Then                                                     'TB_T_SALES_ACT

                GlErrStepInfo = "SetXmlActionNode_12_1(SalesAction)"
                For Each SalesActDataRow As DataRowView In SalesActData.DefaultView
                    GlErrStepInfo = "SetXmlActionNode_12_2(SalesAct)"
                    'GlErrStepInfo = "SALES_ACT_ID:" + SalesActDataRow("SALES_ACT_ID").ToString()
                    'GlErrStepInfo = "SALES_ID:" + SalesActDataRow("SALES_ID").ToString()
                    'GlErrStepInfo = "ACT_ID:" + SalesActDataRow("ACT_ID").ToString()
                    'GlErrStepInfo = "RSLT_SALES_CAT:" + SalesActDataRow("RSLT_SALES_CAT").ToString()
                Next

                GlErrStepInfo = "SetXmlActionNode_13"
                Dim result = From TB_T_SALES_ACT In SalesActData
                     Where ActionSeq("RELATION_ACT_ID") = TB_T_SALES_ACT.SALES_ACT_ID
                      Select New With {
                        TB_T_SALES_ACT.RSLT_SALES_CAT,
                        TB_T_SALES_ACT.ROW_CREATE_ACCOUNT
                      }

                GlErrStepInfo = "SetXmlActionNode_14"
                If (result.Count > 0) Then
                    If (GlProcessJudgeFlg = "1") Then
                        If (ChangeActCd(result.ToList()(0).RSLT_SALES_CAT, "2", "DMS").Rows.Count > 0) Then
                            DmsCodeMapRow = ChangeActCd(result.ToList()(0).RSLT_SALES_CAT, "2", "DMS").Rows(0)
                            'ISSUE-IT2-1_by_takeda_start
                            'ActionCode = DmsCodeMapRow.ICROP_CD_1
                            'ActionName = DmsCodeMapRow.ICROP_CD_2
                            ActionCode = DmsCodeMapRow.DMS_CD_1
                            ActionName = DmsCodeMapRow.DMS_CD_2
                            GlErrStepInfo = "SetXmlActionNode_14_1"
                            'GlErrStepInfo = "ActionCode:" + ActionCode.ToString()
                            'ISSUE-IT2-1_by_takeda_end
                            GlErrStepInfo = "SetXmlActionNode_15"
                            'ISUUE-0016&ISUUE-0017_20140219_by_takeda_start
                            If (GlProcessJudgeFlg = "1") Then                                                'PROCESS_JUDGE_FLG
                                StartActionDate = SALES_ACT_STARTTIME
                                ActionDate = SALES_ACT_ENDTIME

                                'takeda_update_start_zantei_20140401
                                '上記でActionDate(SALES_ACT_ENDTIME)が求められない場合、
                                '開始時間の2時間後を終了時間に設定
                                If (ActionDate = "") Then
                                    GlErrStepInfo = "SetXmlActionNode_16B_3(zantei)"
                                    'GlErrStepInfo = "StartActionDate(before):" + StartActionDate.ToString()
                                    Dim StartActionDateEdit = ""
                                    If (StartActionDate.Length = 19) Then
                                        GlErrStepInfo = "SetXmlActionNode_16B_3_1"
                                        'YYYY/MM/DD HH24:MM:SSに変換
                                        StartActionDateEdit = StartActionDate.Substring(6, 4) + "/" + StartActionDate.Substring(3, 2) + "/" + StartActionDate.Substring(0, 2) + " " + StartActionDate.Substring(11)
                                    ElseIf (StartActionDate.Length = 10) Then
                                        GlErrStepInfo = "SetXmlActionNode_16B_3_2"
                                        'YYYY/MM/DDに変換
                                        StartActionDateEdit = StartActionDate.Substring(6, 4) + "/" + StartActionDate.Substring(3, 2) + "/" + StartActionDate.Substring(0, 2)
                                    Else
                                        GlErrStepInfo = "SetXmlActionNode_16B_3_3"
                                        StartActionDateEdit = StartActionDate
                                    End If
                                    'GlErrStepInfo = "StartActionDate(edit):" + StartActionDateEdit.ToString()
                                    Dim dtStartActionDate As DateTime
                                    If (Date.TryParse(StartActionDateEdit, dtStartActionDate)) Then
                                        GlErrStepInfo = "SetXmlActionNode_16B_3_4"
                                        'GlErrStepInfo = "StartActionDate(after):" + dtStartActionDate.ToString()
                                        ActionDate = EditDateFormat(dtStartActionDate.AddHours(2))
                                    End If
                                End If

                                'GlErrStepInfo = "StartActionDate:" + StartActionDate.ToString()
                                'GlErrStepInfo = "ActionDate(endtime):" + ActionDate.ToString()
                                'takeda_update_end_zantei_20140401

                            End If
                            'ISUUE-0016&ISUUE-0017_20140219_by_takeda_end

                            GlErrStepInfo = "SetXmlActionNode_16"
                            If (GlProcessJudgeFlg = "1") Then                                                'PROCESS_JUDGE_FLG
                                ActionAccount = result.ToList()(0).ROW_CREATE_ACCOUNT
                            End If
                            '20140317 Fujita Add Start
                        Else
                            GlErrStepInfo = "Change RSLT_SALES_CAT：" + result.ToList()(0).RSLT_SALES_CAT + " Not Found"
                            Throw New Exception("Change RSLT_SALES_CAT：" + result.ToList()(0).RSLT_SALES_CAT + " Not Found")
                            '20140317 Fujita Add End
                        End If
                    End If
                End If

                'GlErrStepInfo="SetXmlActionNode_15"
                ''ISUUE-0016&ISUUE-0017_20140219_by_takeda_start
                'If (GlProcessJudgeFlg = "1") Then                                                'PROCESS_JUDGE_FLG
                '    StartActionDate = SALES_ACT_STARTTIME
                '    ActionDate = SALES_ACT_ENDTIME
                'End If
                ''ISUUE-0016&ISUUE-0017_20140219_by_takeda_end

                'GlErrStepInfo="SetXmlActionNode_16"
                'If (GlProcessJudgeFlg = "1") Then                                                'PROCESS_JUDGE_FLG
                '    ActionAccount = result.ToList()(0).ROW_CREATE_ACCOUNT
                'End If

            End If

            GlErrStepInfo = "SetXmlActionNode_17"
            Dim xmlAction As New XmlAction
            xmlAction.ActionSeqNo = CStr(ActionSeq("RELATION_ACT_SEQ"))
            xmlAction.ActionCode = ActionCode
            xmlAction.ActionName = ActionName

            xmlAction.ActionMemo = ActionMemo
            xmlAction.PlannedActionDate = PlannedActionDate
            xmlAction.StartActionDate = StartActionDate
            xmlAction.ActionDate = ActionDate
            xmlAction.ActionBranchCode = ActionBranchCode
            'takeda_update_start_20140412
            'xmlAction.ActionAccount = ActionAccount
            xmlAction.ActionAccount = ConvertStfCd(ActionAccount, CstStfCdCnvtOff)
            'takeda_update_end_20140412
            'GlErrStepInfo = "ActionSeqNo"
            'GlErrStepInfo = "ActionCode"
            'GlErrStepInfo = "ActionName"
            GlErrStepInfo = "SetXmlActionNode_End"
            Return (xmlAction)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlSalesConditionNode(ByVal SalesConditionRow As IC3802801SalesConditionRow, ByVal FollowUpResultData As IC3802801DataSet.IC3802801ActionRow) As XmlSalesCondition
        GlErrStepInfo = "SetXmlSalesConditionNode_Start"
        '20140317 Fujita Upd Start
        Try
            Dim xmlSalesCondition As New XmlSalesCondition
            xmlSalesCondition.SalesConditionNo = SalesConditionRow.SALESCONDITIONNO
            xmlSalesCondition.ItemNo = SalesConditionRow.ITEMNO
            xmlSalesCondition.Other = SalesConditionRow.OTHERSALESCONDITION
            GlErrStepInfo = "SetXmlSalesConditionNode_End"
            Return (xmlSalesCondition)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    'takeda_update_start_20140412
    Public Function SetXmlSalesConditionNode_Empty(ByVal salesConditionNo As Long) As XmlSalesCondition
        GlErrStepInfo = "SetXmlSalesConditionNode_Empty_Start"
        '20140317 Fujita Upd Start
        Try
            Dim xmlSalesCondition As New XmlSalesCondition
            xmlSalesCondition.SalesConditionNo = salesConditionNo.ToString()
            xmlSalesCondition.ItemNo = ""
            xmlSalesCondition.Other = ""

            GlErrStepInfo = "SetXmlSalesConditionNode_Empty_End"
            Return (xmlSalesCondition)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function
    'takeda_update_start_20140412

    Public Function SetXmlSelectItemNode(ByVal SelectItemRow As SelectItemRow) As XmlSelectItem
        GlErrStepInfo = "SetXmlSelectItemNode_Start"
        '20140317 Fujita Upd Start
        Try
            Dim xmlSelectItem As New XmlSelectItem
            xmlSelectItem.ItemNo = SelectItemRow.ItemNo
            xmlSelectItem.Other = SelectItemRow.Other
            GlErrStepInfo = "SetXmlSelectItemNode_End"
            Return (xmlSelectItem)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlNegotiationMemoNode(ByVal RELATION_ACT_ID As String, ByVal ActionMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable) As XmlNegotiationMemo
        GlErrStepInfo = "SetXmlNegotiationMemoNode_Start"
        '20140317 Fujita Upd Start
        Try
            Dim xmlNegotiationMemo As New XmlNegotiationMemo

            Dim resultMemoData = From memoData In ActionMemoData
                                 Where memoData.RELATION_ACT_ID = RELATION_ACT_ID And memoData.RELATION_ACT_TYPE = "2"
                                 Order By memoData.ACT_MEMO_ID Descending
                                  Select New With {
                                    memoData.CREATE_DATETIME,
                                    memoData.CREATE_STF_CD,
                                    memoData.CST_MEMO
                                  }
            GlErrStepInfo = "SetXmlNegotiationMemoNode_1"

            If (resultMemoData.Count() > 0) Then
                GlErrStepInfo = "SetXmlNegotiationMemoNode_2"
                If (IsDBNull(resultMemoData.ToList()(0).CREATE_DATETIME) = False) Then
                    xmlNegotiationMemo.CreateDate = ChangeDefaultDate(resultMemoData.ToList()(0).CREATE_DATETIME, CstStrDefaultDate)
                Else
                    xmlNegotiationMemo.CreateDate = ""
                End If

                GlErrStepInfo = "SetXmlNegotiationMemoNode_3"
                'takeda_update_start_2014012
                'xmlNegotiationMemo.CreateAccount = resultMemoData.ToList()(0).CREATE_STF_CD.ToString()
                xmlNegotiationMemo.CreateAccount = ConvertStfCd(resultMemoData.ToList()(0).CREATE_STF_CD.ToString(), CstStfCdCnvtOff)
                'takeda_update_end_2014012
                xmlNegotiationMemo.Memo = EditLength(resultMemoData.ToList()(0).CST_MEMO.ToString(), 1, 256, CstStrTypeString)
            Else
                GlErrStepInfo = "SetXmlNegotiationMemoNode_4"
                xmlNegotiationMemo.CreateDate = ""
                xmlNegotiationMemo.CreateAccount = ""
                xmlNegotiationMemo.Memo = ""
            End If
            GlErrStepInfo = "SetXmlNegotiationMemoNode_End"
            Return (xmlNegotiationMemo)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlCompetitorSeriesNode(ByVal competitorSeriesRow As IC3802801CompetitorSeriesRow, ByVal MakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable) As XmlCompetitorSeries
        GlErrStepInfo = "SetXmlCompetitorSeriesNode_Start"
        '20140317 Fujita Upd Start
        Try
            Dim result = From MakerModel In MakerModelData
                 Where MakerModel.MODEL_CD = competitorSeriesRow.MODEL_CD
                  Select New With {
                            MakerModel.MAKER_CD,
                            MakerModel.MAKER_NAME,
                            MakerModel.MODEL_NAME
                  }

            Dim xmlCompetitorSeries As New XmlCompetitorSeries
            xmlCompetitorSeries.MakerCode = result.ToList()(0).MAKER_CD.ToString()
            xmlCompetitorSeries.MakerName = result.ToList()(0).MAKER_NAME.ToString()
            xmlCompetitorSeries.SeriesCode = competitorSeriesRow.MODEL_CD
            xmlCompetitorSeries.SeriesName = result.ToList()(0).MODEL_NAME.ToString()
            xmlCompetitorSeries.DeleteDate = ""
            GlErrStepInfo = "SetXmlCompetitorSeriesNode_End"
            Return xmlCompetitorSeries
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlFollowUpResult(ByVal followUpResultRow As FollowUpResultRow) As XmlFollowUpResult
        GlErrStepInfo = "SetXmlFollowUpResult_1_Start"
        '20140317 Fujita Upd Start
        Try
            Dim xmlFollowUpResult As New XmlFollowUpResult
            xmlFollowUpResult.SelectedSeriesNo = followUpResultRow.SelectedSeriesNo
            xmlFollowUpResult.FollowUpResultDate = followUpResultRow.FollowUpResultDate
            xmlFollowUpResult.FollowedBranchCode = followUpResultRow.FollowedBranchCode
            xmlFollowUpResult.FollowedAccount = followUpResultRow.FollowedAccount
            xmlFollowUpResult.ActivityResult = followUpResultRow.ActivityResult
            xmlFollowUpResult.SameDayBookingFlg = followUpResultRow.SameDayBookingFlg
            GlErrStepInfo = "SetXmlFollowUpResult_1_1"
            xmlFollowUpResult.SeriesCode = followUpResultRow.SeriesCode
            xmlFollowUpResult.GradeCode = followUpResultRow.GradeCode
            xmlFollowUpResult.ExteriorColorCode = followUpResultRow.ExteriorColorCode
            xmlFollowUpResult.InteriorColorCode = followUpResultRow.InteriorColorCode
            xmlFollowUpResult.ModelSuffix = followUpResultRow.ModelSuffix
            GlErrStepInfo = "SetXmlFollowUpResult_1_2"
            xmlFollowUpResult.GiveUpReasonCode = followUpResultRow.GiveUpReasonCode
            xmlFollowUpResult.GiveUpReason = followUpResultRow.GiveUpReason
            xmlFollowUpResult.GiveUpMemo = followUpResultRow.GiveUpMemo
            xmlFollowUpResult.GiveUpMakerCode = followUpResultRow.GiveUpMakerCode
            xmlFollowUpResult.GiveUpMakerName = followUpResultRow.GiveUpMakerName
            xmlFollowUpResult.GiveUpSeriesCode = followUpResultRow.GiveUpSeriesCode
            xmlFollowUpResult.GiveUpSeriesName = followUpResultRow.GiveUpSeriesName
            xmlFollowUpResult.CreateDate = followUpResultRow.CreateDate
            xmlFollowUpResult.DeleteDate = followUpResultRow.DeleteDate
            GlErrStepInfo = "SetXmlFollowUpResult_1_End"
            Return (xmlFollowUpResult)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlVehicle(ByVal vehicleRow As VehicleRow) As XmlVehicle
        GlErrStepInfo = "SetXmlVehicle_Start"
        '20140317 Fujita Upd Start
        Try
            Dim xmlVehicle As New XmlVehicle
            xmlVehicle.VehicleSeqNo = vehicleRow.VehicleSeqNo
            xmlVehicle.SeriesCode = vehicleRow.SeriesCode
            xmlVehicle.SeriesName = vehicleRow.SeriesName
            xmlVehicle.Vin = vehicleRow.Vin
            xmlVehicle.VehicleRegistrationNumber = vehicleRow.VehicleRegistrationNumber
            xmlVehicle.VehicleDeliveryDate = vehicleRow.VehicleDeliveryDate
            GlErrStepInfo = "SetXmlVehicle_End"
            Return (xmlVehicle)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlCustomer(ByVal customerRow As CustomerRow) As XmlCustomer
        GlErrStepInfo = "SetXmlCustomer_Start"
        '20140317 Fujita Upd Start
        Try
            Dim xmlCustomer As New XmlCustomer
            xmlCustomer.CustomerID = customerRow.CustomerID
            xmlCustomer.SeqNo = customerRow.SeqNo
            xmlCustomer.CustomerSegment = customerRow.CustomerSegment
            '20140313 Fujita Upd Start
            'xmlCustomer.NewcustomerID = customerRow.NewcustomerID
            GlErrStepInfo = "SetXmlCustomer_1"
            If GlRelationTypeNewCstCd <> "" Then
                If GlRelationTypeNewCstCd = "1" AndAlso customerRow.NewcustomerID.Trim <> "" Then
                    xmlCustomer.NewcustomerID = customerRow.NewcustomerID
                Else
                    xmlCustomer.NewcustomerID = customerRow.CustomerID
                End If
            Else
                xmlCustomer.NewcustomerID = customerRow.CustomerID
            End If
            '20140313 Fujita Upd End

            'takeda_update_start_20140612_自社客コードの編集方法変更
            'xmlCustomer.CustomerCode = customerRow.CustomerCode
            Dim strWkCustomerCode As String = ""    'チェック用
            Dim intStringCheck As Long = 0
            Dim strCustomerCode As String = ""

            GlErrStepInfo = "SetXmlCustomer_2"
            strWkCustomerCode = customerRow.CustomerCode.Trim
            '@存在チェック(何桁目にあるか。存在すれば1以上の値、なければ-1返却)
            intStringCheck = strWkCustomerCode.IndexOf("@")
            If intStringCheck >= 0 Then
                '@が存在する場合、@以降の値を自社客コードとして取得
                strCustomerCode = strWkCustomerCode.Substring(intStringCheck + 1)
                xmlCustomer.CustomerCode = strCustomerCode
            Else
                '@が存在しない場合、DB取得値をそのまま設定
                xmlCustomer.CustomerCode = customerRow.CustomerCode
            End If
            'takeda_update_end_20140612_自社客コードの編集方法変更

            GlErrStepInfo = "SetXmlCustomer_3"
            xmlCustomer.EnquiryCustomerCode = customerRow.EnquiryCustomerCode
            'takeda_update_start_20140412
            'If (customerRow.SalesStaffCode.IndexOf("@") >= 0) Then
            '    xmlCustomer.SalesStaffCode = customerRow.SalesStaffCode.Substring(0, customerRow.SalesStaffCode.IndexOf("@"))
            '    '20140314 Upd Start
            'Else
            '    xmlCustomer.SalesStaffCode = customerRow.SalesStaffCode
            '    '20140314 Upd End
            'End If
            'xmlCustomer.SalesStaffCode = customerRow.SalesStaffCode

            xmlCustomer.SalesStaffCode = ConvertStfCd(customerRow.SalesStaffCode, CstStfCdCnvtOn)
            'takeda_update_end_20140412

            GlErrStepInfo = "SetXmlCustomer_4"
            'takeda_update_start_20140324_DMSでICROPの区分値0,1を反転して設定
            'xmlCustomer.CustomerType = customerRow.CustomerType
            If (customerRow.CustomerType.ToString() = CstStrCstType0) Then
                GlErrStepInfo = "SetXmlCustomer_X(0->1)"
                xmlCustomer.CustomerType = CstStrCstType1
            ElseIf (customerRow.CustomerType.ToString() = CstStrCstType1) Then
                GlErrStepInfo = "SetXmlCustomer_Y(1->0)"
                xmlCustomer.CustomerType = CstStrCstType0
            Else
                GlErrStepInfo = "SetXmlCustomer_Z(None)"
                xmlCustomer.CustomerType = customerRow.CustomerType
            End If
            GlErrStepInfo = "SetXmlCustomer_5"
            'takeda_update_end_20140324_DMSでICROPの区分値0,1を反転して設定
            xmlCustomer.SubCustomerType = customerRow.SubCustomerType
            xmlCustomer.SocialID = customerRow.SocialID
            xmlCustomer.Sex = customerRow.Sex
            xmlCustomer.BirthDay = customerRow.BirthDay
            xmlCustomer.NameTitleCode = customerRow.NameTitleCode
            xmlCustomer.NameTitle = customerRow.NameTitle
            xmlCustomer.Name1 = customerRow.Name1
            xmlCustomer.Name2 = customerRow.Name2
            xmlCustomer.Name3 = customerRow.Name3
            GlErrStepInfo = "SetXmlCustomer_6"
            xmlCustomer.SubName1 = customerRow.SubName1
            xmlCustomer.CompanyName = customerRow.CompanyName
            xmlCustomer.EmployeeName = customerRow.EmployeeName
            xmlCustomer.EmployeeDepartment = customerRow.EmployeeDepartment
            xmlCustomer.EmployeePosition = customerRow.EmployeePosition
            xmlCustomer.Address = customerRow.Address
            xmlCustomer.Address1 = customerRow.Address1
            'takeda_update_start_20140425
            'GlErrStepInfo = "@@@DataCheck(SetXmlCustomer)"
            'GlErrStepInfo = "(XML)xmlCustomer.Address:" + xmlCustomer.Address.ToString()
            'GlErrStepInfo = "(XML)xmlCustomer.Address1:" + xmlCustomer.Address1.ToString()
            'takeda_update_end_20140425
            xmlCustomer.Address2 = customerRow.Address2
            xmlCustomer.Address3 = customerRow.Address3
            GlErrStepInfo = "SetXmlCustomer_7"
            xmlCustomer.Domicile = customerRow.Domicile
            xmlCustomer.Country = customerRow.Country
            xmlCustomer.ZipCode = customerRow.ZipCode
            'takeda_update_start_20140531(DB初期値の場合、Trim処理を行い、タグのみ設定する)
            'xmlCustomer.StateCode = customerRow.StateCode
            xmlCustomer.StateCode = customerRow.StateCode.Trim
            'takeda_update_end_20140531(DB初期値の場合、Trim処理を行い、タグのみ設定する)
            xmlCustomer.StateName = customerRow.StateName
            'takeda_update_start_20140531(DB初期値の場合、Trim処理を行い、タグのみ設定する)
            'xmlCustomer.DistrictCode = customerRow.DistrictCode
            xmlCustomer.DistrictCode = customerRow.DistrictCode.Trim
            'takeda_update_end_20140531(DB初期値の場合、Trim処理を行い、タグのみ設定する)
            xmlCustomer.DistrictName = customerRow.DistrictName
            'takeda_update_start_20140531(DB初期値の場合、Trim処理を行い、タグのみ設定する)
            'xmlCustomer.CityCode = customerRow.CityCode
            xmlCustomer.CityCode = customerRow.CityCode.Trim
            'takeda_update_end_20140531(DB初期値の場合、Trim処理を行い、タグのみ設定する)
            xmlCustomer.CityName = customerRow.CityName
            'takeda_update_start_20140531(DB初期値の場合、Trim処理を行い、タグのみ設定する)
            'xmlCustomer.LocationCode = customerRow.LocationCode
            xmlCustomer.LocationCode = customerRow.LocationCode.Trim
            'takeda_update_end_20140531(DB初期値の場合、Trim処理を行い、タグのみ設定する)
            xmlCustomer.LocationName = customerRow.LocationName
            GlErrStepInfo = "SetXmlCustomer_8"
            xmlCustomer.TelNumber = customerRow.TelNumber
            xmlCustomer.FaxNumber = customerRow.FaxNumber
            xmlCustomer.Mobile = customerRow.Mobile
            xmlCustomer.EMail1 = customerRow.EMail1
            xmlCustomer.EMail2 = customerRow.EMail2
            xmlCustomer.BusinessTelNumber = customerRow.BusinessTelNumber
            xmlCustomer.Income = customerRow.Income
            xmlCustomer.ContactTime = customerRow.ContactTime
            xmlCustomer.OccupationID = customerRow.OccupationID
            xmlCustomer.Occupation = customerRow.Occupation
            xmlCustomer.DefaultLang = customerRow.DefaultLang
            xmlCustomer.CustomerMemo = customerRow.CustomerMemo
            xmlCustomer.CreateDate = customerRow.CreateDate
            xmlCustomer.UpdateDate = customerRow.UpdateDate
            xmlCustomer.DeleteDate = customerRow.DeleteDate
            GlErrStepInfo = "SetXmlCustomer_End"
            Return (xmlCustomer)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlFamilyInformation(ByVal familyInformationRow As FamilyInformationRow) As XmlFamilyInformation
        GlErrStepInfo = "SetXmlFamilyInformation_Start"
        '20140317 Fujita Upd Start
        Try
            Dim xmlFamilyInformation As New XmlFamilyInformation
            xmlFamilyInformation.FamilyNo = familyInformationRow.FamilyNo
            xmlFamilyInformation.FamilyCode = familyInformationRow.FamilyCode
            xmlFamilyInformation.FamilyCodeName = familyInformationRow.FamilyCodeName
            xmlFamilyInformation.BirthDay = familyInformationRow.BirthDay
            GlErrStepInfo = "SetXmlFamilyInformation_End"
            Return (xmlFamilyInformation)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function SetXmlHobby(ByVal hobbyRow As HobbyRow) As XmlHobby
        GlErrStepInfo = "SetXmlHobby_Start"
        '20140317 Fujita Upd Start
        Try
            Dim xmlHobby As New XmlHobby
            xmlHobby.HobbyCode = hobbyRow.HobbyCode
            xmlHobby.HobbyName = hobbyRow.HobbyName
            GlErrStepInfo = "SetXmlHobby_End"
            Return (xmlHobby)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Sub InitDataSetting(ByVal DealerCode As String, ByVal BranchCode As String, ByVal CntCd As String)
        GlErrStepInfo = "InitDataSetting_Start"
        '20140317 Fujita Upd Start
        Try

            'Variables to store retrieve results																				
            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'Dim SystemSettingDlrData As IC3802801DataSet.IC3802801SystemSettingRow
            'Dim SystemSettingEnvData As IC3802801DataSet.IC3802801SystemSettingEnvRow
            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

            '$25 他システム連携における複数店舗コード変換対応 start 
            Dim prgSettingV4 As ProgramSettingV4 = New ProgramSettingV4()
            '$25 他システム連携における複数店舗コード変換対応 end

            Dim strWorkSystemEnv As String
            Dim strWorkData As String

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            Dim systemSettingDlr As SystemSettingDlr = New SystemSettingDlr
            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

            GlErrStepInfo = "InitDataSetting_1"

            'Call dealer system setting data retrieve process(ActivityJudgeFlg)	
            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrActivityJudgeFlg).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_1_1"
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrActivityJudgeFlg).Rows(0), IC3802801SystemSettingRow)
            '    GlActivityJudgeFlg = SystemSettingDlrData.SETTING_VAL
            '    'takeda_update_start_20140523

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrActivityJudgeFlg).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_1_2"
            '    '上記で取得できない場合、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrActivityJudgeFlg).Rows(0), IC3802801SystemSettingRow)
            '    GlActivityJudgeFlg = SystemSettingDlrData.SETTING_VAL

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrActivityJudgeFlg).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_1_3"
            '    '上記で取得できない場合、販売店コード(XXXXX)、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrActivityJudgeFlg).Rows(0), IC3802801SystemSettingRow)
            '    GlActivityJudgeFlg = SystemSettingDlrData.SETTING_VAL

            Dim rowSetting_1_1 As TB_M_SYSTEM_SETTING_DLRRow = systemSettingDlr.GetEnvSetting(DealerCode, BranchCode, CstStrActivityJudgeFlg)
            If (rowSetting_1_1 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_1_1"
                GlActivityJudgeFlg = rowSetting_1_1.SETTING_VAL
                'takeda_update_start_20140523
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

            Else
                GlErrStepInfo = "InitDataSetting_1_4"
                GlActivityJudgeFlg = CstDfltActivityJudgeFlg
                'takeda_update_end_20140523
            End If

            GlErrStepInfo = "InitDataSetting_2"
            'Call dealer system setting data retrieve process(ProcessJudgeFlg)	

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrProcessJudgeFlg).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_2_1"
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrProcessJudgeFlg).Rows(0), IC3802801SystemSettingRow)
            '    GlProcessJudgeFlg = SystemSettingDlrData.SETTING_VAL
            '    'takeda_update_start_20140523

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrProcessJudgeFlg).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_2_2"
            '    '上記で取得できない場合、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrProcessJudgeFlg).Rows(0), IC3802801SystemSettingRow)
            '    GlProcessJudgeFlg = SystemSettingDlrData.SETTING_VAL

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrProcessJudgeFlg).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_2_3"
            '    '上記で取得できない場合、販売店コード(XXXXX)、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrProcessJudgeFlg).Rows(0), IC3802801SystemSettingRow)
            '    GlProcessJudgeFlg = SystemSettingDlrData.SETTING_VAL

            Dim rowSetting_2_1 As TB_M_SYSTEM_SETTING_DLRRow = systemSettingDlr.GetEnvSetting(DealerCode, BranchCode, CstStrProcessJudgeFlg)
            If (rowSetting_2_1 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_2_1"
                GlProcessJudgeFlg = rowSetting_2_1.SETTING_VAL
                'takeda_update_start_20140523
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

            Else
                GlErrStepInfo = "InitDataSetting_2_4"
                GlProcessJudgeFlg = CstDfltProcessJudgeFlg
                'takeda_update_end_20140523
            End If

            GlErrStepInfo = "InitDataSetting_3"
            'Call dealer system setting data retrieve process(ErrorInfo)	

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrErrorInfo).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_3_1"
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrErrorInfo).Rows(0), IC3802801SystemSettingRow)
            '    GlErrorInfo = SystemSettingDlrData.SETTING_VAL
            '    'takeda_update_start_20140523

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrErrorInfo).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_3_2"
            '    '上記で取得できない場合、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrErrorInfo).Rows(0), IC3802801SystemSettingRow)
            '    GlErrorInfo = SystemSettingDlrData.SETTING_VAL

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrErrorInfo).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_3_3"
            '    '上記で取得できない場合、販売店コード(XXXXX)、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrErrorInfo).Rows(0), IC3802801SystemSettingRow)
            '    GlErrorInfo = SystemSettingDlrData.SETTING_VAL

            Dim rowSetting_3_1 As TB_M_SYSTEM_SETTING_DLRRow = systemSettingDlr.GetEnvSetting(DealerCode, BranchCode, CstStrErrorInfo)
            If (rowSetting_3_1 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_3_1"
                GlErrorInfo = rowSetting_3_1.SETTING_VAL
                'takeda_update_start_20140523
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START

            Else
                GlErrStepInfo = "InitDataSetting_3_4"
                GlErrorInfo = CstDfltErrorInfo
                'takeda_update_end_20140523
            End If

            GlErrStepInfo = "InitDataSetting_4"
            'Call dealer system setting data retrieve process(SendProspectCstUrl)

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrSendProspectCstUrl).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_4_1"
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrSendProspectCstUrl).Rows(0), IC3802801SystemSettingRow)
            '    GlSendProspectCstUrl = SystemSettingDlrData.SETTING_VAL
            '    'takeda_update_start_20140523

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrSendProspectCstUrl).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_4_2"
            '    '上記で取得できない場合、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrSendProspectCstUrl).Rows(0), IC3802801SystemSettingRow)
            '    GlSendProspectCstUrl = SystemSettingDlrData.SETTING_VAL

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrSendProspectCstUrl).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_4_3"
            '    '上記で取得できない場合、販売店コード(XXXXX)、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrSendProspectCstUrl).Rows(0), IC3802801SystemSettingRow)
            '    GlSendProspectCstUrl = SystemSettingDlrData.SETTING_VAL

            Dim rowSetting_4_1 As TB_M_SYSTEM_SETTING_DLRRow = systemSettingDlr.GetEnvSetting(DealerCode, BranchCode, CstStrSendProspectCstUrl)
            If (rowSetting_4_1 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_4_1"
                GlSendProspectCstUrl = rowSetting_4_1.SETTING_VAL
                'takeda_update_start_20140523
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

            Else
                GlErrStepInfo = "InitDataSetting_4_4"
                GlSendProspectCstUrl = CstDfltSendProspectCstUrl
                'takeda_update_end_20140523
            End If

            GlErrStepInfo = "InitDataSetting_5"
            ' Call Dealer system setting process (SendDmsSvr)

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrSendDmsSvr).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_5_1"
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrSendDmsSvr).Rows(0), IC3802801SystemSettingRow)
            '    GlSendDmsSvr = SystemSettingDlrData.SETTING_VAL
            '    'takeda_update_start_20140523

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrSendDmsSvr).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_5_2"
            '    '上記で取得できない場合、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrSendDmsSvr).Rows(0), IC3802801SystemSettingRow)
            '    GlSendDmsSvr = SystemSettingDlrData.SETTING_VAL

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrSendDmsSvr).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_5_3"
            '    '上記で取得できない場合、販売店コード(XXXXX)、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrSendDmsSvr).Rows(0), IC3802801SystemSettingRow)
            '    GlSendDmsSvr = SystemSettingDlrData.SETTING_VAL

            Dim rowSetting_5_1 As TB_M_SYSTEM_SETTING_DLRRow = systemSettingDlr.GetEnvSetting(DealerCode, BranchCode, CstStrSendDmsSvr)
            If (rowSetting_5_1 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_5_1"
                GlSendDmsSvr = rowSetting_5_1.SETTING_VAL
                'takeda_update_start_20140523
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

            Else
                GlErrStepInfo = "InitDataSetting_5_4"
                GlSendDmsSvr = CstDfltSendDmsSvr
                'takeda_update_end_20140523
            End If

            GlErrStepInfo = "InitDataSetting_6"
            'Call dealer system setting process (InitProspectStatus)	

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrInitProspectStatus).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_6_1"
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrInitProspectStatus).Rows(0), IC3802801SystemSettingRow)
            '    GlInitProspectStatus = SystemSettingDlrData.SETTING_VAL
            '    'takeda_update_start_20140523

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrInitProspectStatus).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_6_2"
            '    '上記で取得できない場合、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrInitProspectStatus).Rows(0), IC3802801SystemSettingRow)
            '    GlInitProspectStatus = SystemSettingDlrData.SETTING_VAL

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrInitProspectStatus).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_6_3"
            '    '上記で取得できない場合、販売店コード(XXXXX)、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrInitProspectStatus).Rows(0), IC3802801SystemSettingRow)
            '    GlInitProspectStatus = SystemSettingDlrData.SETTING_VAL

            Dim rowSetting_6_1 As TB_M_SYSTEM_SETTING_DLRRow = systemSettingDlr.GetEnvSetting(DealerCode, BranchCode, CstStrInitProspectStatus)
            If (rowSetting_6_1 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_6_1"
                GlInitProspectStatus = rowSetting_6_1.SETTING_VAL
                'takeda_update_start_20140523
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

            Else
                GlErrStepInfo = "InitDataSetting_6_4"
                GlInitProspectStatus = CstDfltInitProspectStatus
                'takeda_update_end_20140523
            End If

            GlErrStepInfo = "InitDataSetting_7"
            'Call dealer system setting data retrieve process(OutReturnIFUrl)

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrOutReturnIFUrl).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_7_1"
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrOutReturnIFUrl).Rows(0), IC3802801SystemSettingRow)
            '    GlOutReturnIFUrl = SystemSettingDlrData.SETTING_VAL
            '    If 0 <= GlOutReturnIFUrl.IndexOf("DMSXML=") Then
            '        GlNoDMSFlg = "1"
            '        GlTestResponseXML = GlOutReturnIFUrl.Substring(7)
            '        'GlErrStepInfo = "GlNoDMSFlg:" + GlNoDMSFlg
            '        'GlErrStepInfo = "GlTestResponseXML:" + GlTestResponseXML
            '    End If
            '    'takeda_update_start_20140523

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrOutReturnIFUrl).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_7_2"
            '    '上記で取得できない場合、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(DealerCode, CstStrCmnBrnCd, CstStrOutReturnIFUrl).Rows(0), IC3802801SystemSettingRow)
            '    GlOutReturnIFUrl = SystemSettingDlrData.SETTING_VAL
            '    If 0 <= GlOutReturnIFUrl.IndexOf("DMSXML=") Then
            '        GlNoDMSFlg = "1"
            '        GlTestResponseXML = GlOutReturnIFUrl.Substring(7)
            '        'GlErrStepInfo = "GlNoDMSFlg:" + GlNoDMSFlg
            '        'GlErrStepInfo = "GlTestResponseXML:" + GlTestResponseXML
            '    End If

            'ElseIf (IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrOutReturnIFUrl).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_7_3"
            '    '上記で取得できない場合、販売店コード(XXXXX)、店舗コード(XXX)を指定して再取得
            '    SystemSettingDlrData = CType(IC3802801TableAdapter.GetSystemSetting(CstStrCmnDlrCd, CstStrCmnBrnCd, CstStrOutReturnIFUrl).Rows(0), IC3802801SystemSettingRow)
            '    GlOutReturnIFUrl = SystemSettingDlrData.SETTING_VAL
            '    If 0 <= GlOutReturnIFUrl.IndexOf("DMSXML=") Then
            '        GlNoDMSFlg = "1"
            '        GlTestResponseXML = GlOutReturnIFUrl.Substring(7)
            '        'GlErrStepInfo = "GlNoDMSFlg:" + GlNoDMSFlg
            '        'GlErrStepInfo = "GlTestResponseXML:" + GlTestResponseXML
            '    End If

            Dim rowSetting_7_1 As TB_M_SYSTEM_SETTING_DLRRow = systemSettingDlr.GetEnvSetting(DealerCode, BranchCode, CstStrOutReturnIFUrl)
            If (rowSetting_7_1 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_7_1"
                GlOutReturnIFUrl = rowSetting_7_1.SETTING_VAL
                If 0 <= GlOutReturnIFUrl.IndexOf("DMSXML=") Then
                    GlNoDMSFlg = "1"
                    GlTestResponseXML = GlOutReturnIFUrl.Substring(7)
                    'GlErrStepInfo = "GlNoDMSFlg:" + GlNoDMSFlg
                    'GlErrStepInfo = "GlTestResponseXML:" + GlTestResponseXML
                End If
                'takeda_update_start_20140523
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

            Else
                GlErrStepInfo = "InitDataSetting_7_4"
                GlOutReturnIFUrl = CstDfltOutReturnIFUrl
                'takeda_update_end_20140523

            End If
            'GlErrStepInfo="OutReturnIFUrl_Count:" + IC3802801TableAdapter.GetSystemSetting(DealerCode, BranchCode, CstStrOutReturnIFUrl).Rows.Count.ToString)

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            Dim systemEnvSetting As SystemEnvSetting = New SystemEnvSetting
            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

            GlErrStepInfo = "InitDataSetting_8"
            'Call System environment setting retrieve process(ActStatusCold)

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrActStatusCold).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_8_1"
            '    SystemSettingEnvData = CType(IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrActStatusCold).Rows(0), IC3802801SystemSettingEnvRow)
            '    strWorkData = SystemSettingEnvData.PARAMVALUE

            Dim rowSetting_8_1 As SYSTEMENVSETTINGRow = systemEnvSetting.GetSystemEnvSetting(CstStrActStatusCold)
            If (rowSetting_8_1 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_8_1"
                strWorkData = rowSetting_8_1.PARAMVALUE
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

                ' Specify "/", and save value before "/"																				
                GlDmsStatusCold = strWorkData.Substring(0, InStr(strWorkData, "/") - 1)
                ' Specify "/", and save value after "/"																				
                strWorkSystemEnv = strWorkData.Substring(InStr(strWorkData, "/"))
                ' Separate by a comma, and store in an array																				
                GlIcropStatusCold = strWorkSystemEnv.Split(CChar(","))
            End If

            'Call System environment setting retrieve process(ActStatusWarm)	

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrActStatusWarm).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_8_2"
            '    SystemSettingEnvData = CType(IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrActStatusWarm).Rows(0), IC3802801SystemSettingEnvRow)
            '    strWorkData = SystemSettingEnvData.PARAMVALUE

            Dim rowSetting_8_2 As SYSTEMENVSETTINGRow = systemEnvSetting.GetSystemEnvSetting(CstStrActStatusWarm)
            If (rowSetting_8_2 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_8_2"
                strWorkData = rowSetting_8_2.PARAMVALUE
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

                ' Specify "/", and save value before "/"																				
                GlDmsStatusWarm = strWorkData.Substring(0, InStr(strWorkData, "/") - 1)
                ' Specify "/", and save value after "/"																				
                strWorkSystemEnv = strWorkData.Substring(InStr(strWorkData, "/"))
                ' Separate by a comma, and store in an array																				
                GlIcropStatusWarm = strWorkSystemEnv.Split(CChar(","))
            End If

            'Call System environment setting retrieve process(ActStatusHot)	

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrActStatusHot).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_8_3"
            '    SystemSettingEnvData = CType(IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrActStatusHot).Rows(0), IC3802801SystemSettingEnvRow)
            '    strWorkData = SystemSettingEnvData.PARAMVALUE

            Dim rowSetting_8_3 As SYSTEMENVSETTINGRow = systemEnvSetting.GetSystemEnvSetting(CstStrActStatusHot)
            If (rowSetting_8_3 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_8_3"
                strWorkData = rowSetting_8_3.PARAMVALUE
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

                ' Specify "/", and save value before "/"																				
                GlDmsStatusHot = strWorkData.Substring(0, InStr(strWorkData, "/") - 1)
                ' Specify "/", and save value after "/"																				
                strWorkSystemEnv = strWorkData.Substring(InStr(strWorkData, "/"))
                ' Separate by a comma, and store in an array																				
                GlIcropStatusHot = strWorkSystemEnv.Split(CChar(","))
            End If

            'Call System environment setting retrieve process(ActResultSuccess)	

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrActResultSuccess).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_8_4"
            '    SystemSettingEnvData = CType(IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrActResultSuccess).Rows(0), IC3802801SystemSettingEnvRow)
            '    strWorkData = SystemSettingEnvData.PARAMVALUE

            Dim rowSetting_8_4 As SYSTEMENVSETTINGRow = systemEnvSetting.GetSystemEnvSetting(CstStrActResultSuccess)
            If (rowSetting_8_4 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_8_4"
                strWorkData = rowSetting_8_4.PARAMVALUE
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

                ' Specify "/", and save value before "/"																				
                GlDmsResultSuccess = strWorkData.Substring(0, InStr(strWorkData, "/") - 1)
                ' Specify "/", and save value after "/"																				
                strWorkSystemEnv = strWorkData.Substring(InStr(strWorkData, "/"))
                ' Separate by a comma, and store in an array																				
                GlIcropResultSuccess = strWorkSystemEnv.Split(CChar(","))
            End If

            'Call System environment setting retrieve process(ActResultContinue)

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrActResultContinue).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_8_5"
            '    SystemSettingEnvData = CType(IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrActResultContinue).Rows(0), IC3802801SystemSettingEnvRow)
            '    strWorkData = SystemSettingEnvData.PARAMVALUE

            Dim rowSetting_8_5 As SYSTEMENVSETTINGRow = systemEnvSetting.GetSystemEnvSetting(CstStrActResultContinue)
            If (rowSetting_8_5 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_8_5"
                strWorkData = rowSetting_8_5.PARAMVALUE
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

                ' Specify "/", and save value before "/"																				
                GlDmsResultContinue = strWorkData.Substring(0, InStr(strWorkData, "/") - 1)
                ' Specify "/", and save value after "/"																				
                strWorkSystemEnv = strWorkData.Substring(InStr(strWorkData, "/"))
                ' Separate by a comma, and store in an array																				
                GlIcropResultContinue = strWorkSystemEnv.Split(CChar(","))
            End If

            'Call System environment setting retrieve process(ActResultGiveup)

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrActResultGiveup).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_8_6"
            '    SystemSettingEnvData = CType(IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrActResultGiveup).Rows(0), IC3802801SystemSettingEnvRow)
            '    strWorkData = SystemSettingEnvData.PARAMVALUE

            Dim rowSetting_8_6 As SYSTEMENVSETTINGRow = systemEnvSetting.GetSystemEnvSetting(CstStrActResultGiveup)
            If (rowSetting_8_6 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_8_6"
                strWorkData = rowSetting_8_6.PARAMVALUE
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

                ' Specify "/", and save value before "/"																				
                GlDmsResultGiveup = strWorkData.Substring(0, InStr(strWorkData, "/") - 1)
                ' Specify "/", and save value after "/"																				
                strWorkSystemEnv = strWorkData.Substring(InStr(strWorkData, "/"))
                ' Separate by a comma, and store in an array																				
                GlIcropResultGiveup = strWorkSystemEnv.Split(CChar(","))
            End If

            ' Call system environment setting retrieve process (RelationTypeNewCstCd)

            ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 START
            'If (IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrRelationTypeNewCstCd).Rows.Count > 0) Then
            '    GlErrStepInfo = "InitDataSetting_9_1"
            '    SystemSettingEnvData = CType(IC3802801TableAdapter.GetSystemSettingEnv(CntCd, CstStrRelationTypeNewCstCd).Rows(0), IC3802801SystemSettingEnvRow)
            '    GlRelationTypeNewCstCd = SystemSettingEnvData.PARAMVALUE

            Dim rowSetting_9_1 As SYSTEMENVSETTINGRow = systemEnvSetting.GetSystemEnvSetting(CstStrRelationTypeNewCstCd)
            If (rowSetting_9_1 IsNot Nothing) Then
                GlErrStepInfo = "InitDataSetting_9_1"
                GlRelationTypeNewCstCd = rowSetting_9_1.PARAMVALUE
                ' 2019/11/12 NSK 鈴木 (トライ店システム評価)次世代セールス基盤：ログ出力機能における保守性向上検証 END

                '20140313 Fujita Add Start
            Else
                GlErrStepInfo = "InitDataSetting_9_2"
                GlRelationTypeNewCstCd = ""
                '20140313 Fujita Add End
            End If

            '$25 他システム連携における複数店舗コード変換対応 start 
            '基幹コードマップ（販売店コード取得）にて使用するカラムを取得
            Dim prgSettingRow = prgSettingV4.GetProgramSettingV4(CstStrPGID, CstStrPGID, CstStrDmsCodeMapBrnCdKey)
            If (prgSettingRow Is Nothing) Then
                'プログラム設定が取得できない場合はデフォルト値を設定
                GlDmsCodeMapUseColumn = CstStrDmsCodeMapDmsColumnDefault
            Else
                GlDmsCodeMapUseColumn = prgSettingRow.SETTING_VAL
            End If
            '$25 他システム連携における複数店舗コード変換対応 end 

            'takeda_update_start_20140523
            'GlErrStepInfo = "-----Dealer System Setting Info(A017)-----"
            'GlErrStepInfo = "GlActivityJudgeFlg:" + GlActivityJudgeFlg
            'GlErrStepInfo = "GlProcessJudgeFlg:" + GlProcessJudgeFlg
            'GlErrStepInfo = "GlErrorInfo:" + GlErrorInfo
            'GlErrStepInfo = "GlSendProspectCstUrl:" + GlSendProspectCstUrl
            'GlErrStepInfo = "GlSendDmsSvr:" + GlSendDmsSvr
            'GlErrStepInfo = "GlInitProspectStatus:" + GlInitProspectStatus
            'GlErrStepInfo = "GlOutReturnIFUrl:" + GlOutReturnIFUrl
            'GlErrStepInfo = "-----System Environment Setting Info(C008)-----"
            'GlErrStepInfo = "GlDmsStatusCold:" + GlDmsStatusCold
            'GlErrStepInfo = "GlDmsStatusWarm:" + GlDmsStatusWarm
            'GlErrStepInfo = "GlDmsStatusHot:" + GlDmsStatusHot
            'GlErrStepInfo = "GlDmsResultSuccess:" + GlDmsResultSuccess
            'GlErrStepInfo = "GlDmsResultContinue:" + GlDmsResultContinue
            'GlErrStepInfo = "GlDmsResultGiveup:" + GlDmsResultGiveup
            'GlErrStepInfo = "GlRelationTypeNewCstCd:" + GlRelationTypeNewCstCd
            'takeda_update_end_20140523

            GlErrStepInfo = "InitDataSetting_End"
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Sub

    Public Function ProspectCustomer1(ByVal staff As StaffContext, ByVal SalesId As Long, ByVal CountryCode As String) As XmlProspectCustomer
        Dim IcropDealerCode As String = staff.DlrCD
        Dim IcropBranchCode As String = staff.BrnCD
        'takeda_update_start_20140722_変数UserNameには、ユーザ情報(TBL_USERS)のACCOUNTを設定
        'Dim UserName As String = staff.UserName
        Dim UserName As String = staff.Account
        'takeda_update_end_20140722_変数UserNameには、ユーザ情報(TBL_USERS)のACCOUNTを設定

        '20140317 Fujita Upd Start
        Try

            GlErrStepInfo = "ProspectCustomer1_Start"
            'Variables to store retrieve results																									
            Dim xmlData As New XmlProspectCustomer

            Dim FollowUpResultData As IC3802801DataSet.IC3802801ActionRow
            Dim StatusActionData As IC3802801DataSet.IC3802801ActionRow
            Dim FollowUpActionData As IC3802801DataSet.IC3802801ActionRow

            Dim SalesData As IC3802801DataSet.IC3802801SalesRow
            Dim SalesTempData As IC3802801DataSet.IC3802801SalesTempRow
            Dim RequestData As IC3802801DataSet.IC3802801FollowUpRequestRow
            Dim AttractData As IC3802801DataSet.IC3802801FollowUpAttractRow
            Dim DlrCustomerMemoData As IC3802801DataSet.IC3802801CustomerMemoRow
            Dim GiveUpMakerModelData As IC3802801DataSet.IC3802801MakerModelRow
            '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
            Dim SalesLocalData As IC3802801DataSet.IC3802801SalesLocalRow
            '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end
            Dim FllwUpBoxSalesData As New IC3802801DataSet.IC3802801GetFllwUpBoxSalesDataTable
            'Dim ReqSrcData As IC3802801DataSet.IC3802801ReqSourceRow
            Dim FllwUpBoxSalesData2 As New IC3802801DataSet.IC3802801GetFllwUpBoxSalesDataTable
            Dim ActionResultData As IC3802801DataSet.IC3802801ActionResultRow
            Dim CustomerData As IC3802801DataSet.IC3802801CustomerRow
            'Dim CustomerAddressData As IC3802801DataSet.IC3802801CustomerAddressRow
            Dim EstimateData As IC3802801DataSet.IC3802801EstimateInfoRow
            Dim ContactTimeslotData As IC3802801DataSet.IC3802801ContactTimeslotRow
            Dim ActionSeqData As New IC3802801DataSet.IC3802801ActionSeqDataTable
            Dim SalesActionSeqData As New IC3802801DataSet.IC3802801ActionSeqDataTable
            Dim ActionMemoData As New IC3802801DataSet.IC3802801ActionMemoDataTable
            Dim SalesConditionData As IC3802801DataSet.IC3802801SalesConditionDataTable
            Dim HobbyData As IC3802801DataSet.IC3802801HobbyDataTable
            Dim VehicleData As New IC3802801DataSet.IC3802801VehicleDataTable
            Dim NegotiationMemoData As New IC3802801DataSet.IC3802801ActionMemoDataTable
            Dim ActionData As IC3802801DataSet.IC3802801ActionDataTable
            Dim SalesActData As New IC3802801DataSet.IC3802801SalesActionDataTable
            'takeda_update_start_20140412
            Dim SalesActDataCheck As IC3802801DataSet.IC3802801SalesActionDataTable
            'takeda_update_end_20140412
            Dim FamilyInfomationData As IC3802801DataSet.IC3802801FamilyInfomationDataTable
            Dim DlrCstVclData As IC3802801DataSet.IC3802801DlrCstVclDataTable
            Dim SelectedSeriesData As IC3802801DataSet.IC3802801SelectedSeriesDataTable
            Dim CompetitorSeriesData As IC3802801DataSet.IC3802801CompetitorSeriesDataTable
            Dim MakerModelData As New IC3802801DataSet.IC3802801MakerModelDataTable
            Dim FirstActionData As IC3802801DataSet.IC3802801ActionDataTable
            Dim VehicleMakerModelData As New IC3802801DataSet.IC3802801MakerModelDataTable

            Dim EstimateVclData As IC3802801DataSet.IC3802801EstimateVclInfoRow
            '20140310 Add Start FUJITA
            Dim EstimateVclDataT As New IC3802801DataSet.IC3802801EstimateVclInfoDataTable
            Dim ReqSrcData As New IC3802801DataSet.IC3802801ReqSource1DataTable
            '20140310 Add End FUJITA

            Dim EmptyAttractData As New IC3802801DataSet.IC3802801FollowUpAttractDataTable
            AttractData = CType(EmptyAttractData.NewRow(), IC3802801FollowUpAttractRow)
            Dim EmptyFollowUpActionData As New IC3802801DataSet.IC3802801ActionDataTable()
            FollowUpActionData = CType(EmptyFollowUpActionData.NewRow(), IC3802801ActionRow)
            Dim EmptyActionResultData As New IC3802801DataSet.IC3802801ActionResultDataTable()
            ActionResultData = CType(EmptyActionResultData.NewRow(), IC3802801ActionResultRow)
            Dim EmptySales As New IC3802801DataSet.IC3802801SalesDataTable
            SalesData = CType(EmptySales.NewRow(), IC3802801SalesRow)
            Dim EmptySalesTemp As New IC3802801DataSet.IC3802801SalesTempDataTable
            SalesTempData = CType(EmptySalesTemp.NewRow(), IC3802801SalesTempRow)
            Dim EmptyRequestData As New IC3802801DataSet.IC3802801FollowUpRequestDataTable
            RequestData = CType(EmptyRequestData.NewRow(), IC3802801FollowUpRequestRow)
            Dim EmptyReqSourceData As New IC3802801DataSet.IC3802801ReqSourceDataTable
            'ReqSrcData = EmptyReqSourceData.NewRow()
            Dim EmptyCustomerData As New IC3802801DataSet.IC3802801CustomerDataTable()
            CustomerData = CType(EmptyCustomerData.NewRow(), IC3802801CustomerRow)
            Dim EmptyCustomerAddressData As New IC3802801DataSet.IC3802801CustomerAddressDataTable
            'CustomerAddressData = EmptyCustomerAddressData.NewRow()
            Dim EmptyEstimateData As New IC3802801DataSet.IC3802801EstimateInfoDataTable
            EstimateData = CType(EmptyEstimateData.NewRow(), IC3802801EstimateInfoRow)

            '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
            Dim EmptySalesLocal As New IC3802801DataSet.IC3802801SalesLocalDataTable
            SalesLocalData = CType(EmptySalesLocal.NewRow(), IC3802801SalesLocalRow)
            '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end
            Dim StateInfoData As IC3802801DataSet.IC3802801StateInfoRow
            Dim EmptyStateData = New IC3802801DataSet.IC3802801StateInfoDataTable
            StateInfoData = CType(EmptyStateData.NewRow(), IC3802801StateInfoRow)

            Dim DistrictInfoData As IC3802801DataSet.IC3802801DistrictInfoRow
            Dim EmptyDistrictData As New IC3802801DataSet.IC3802801DistrictInfoDataTable
            DistrictInfoData = CType(EmptyDistrictData.NewRow(), IC3802801DistrictInfoRow)

            Dim CityInfoData As IC3802801DataSet.IC3802801CityInfoRow
            Dim EmptyCityData As New IC3802801DataSet.IC3802801CityInfoDataTable
            CityInfoData = CType(EmptyCityData.NewRow(), IC3802801CityInfoRow)

            Dim LocationInfoData As IC3802801DataSet.IC3802801LocationInfoRow
            Dim EmptyLocationInfo As New IC3802801DataSet.IC3802801LocationInfoDataTable
            LocationInfoData = CType(EmptyLocationInfo.NewRow(), IC3802801LocationInfoRow)

            Dim ReqSrcData1 As IC3802801DataSet.IC3802801ReqSource1Row
            Dim EmptyReqSrcData1Data As New IC3802801DataSet.IC3802801ReqSource1DataTable
            ReqSrcData1 = CType(EmptyReqSrcData1Data.NewRow(), IC3802801ReqSource1Row)

            Dim ReqSrcData2 As IC3802801DataSet.IC3802801ReqSource2Row
            Dim EmptyReqSrcData2Data As New IC3802801DataSet.IC3802801ReqSource2DataTable
            ReqSrcData2 = CType(EmptyReqSrcData2Data.NewRow(), IC3802801ReqSource2Row)

            'Variable declaration																									
            Dim ReqSrc1stCd As String
            Dim ReqSrc2ndCd As String
            Dim LastActId As String
            Dim FirstSuccessFlg As String           '※"0"=Normal times, "1"= First time success															
            Dim StrCstVclKbn As String
            Dim StrErrKey As String
            Dim StrActStatus As String

            Dim blnRtnCode As Boolean
            Dim LastActCd As String
            Dim GiveUpMakerCd As String       'For FollowUpResult				
            Dim LastActionKeyData As IC3802801DataSet.IC3802801ActionRow   'For FollowUpResult
            'ISSUE-0008_20130217_by_takeda_Start
            Dim intCustId As Long
            'ISSUE-0008_20130217_by_takeda_End

            GlErrStepInfo = "ProspectCustomer1_0(Get Init Data Setting)"

            'Call Dealer System Setting data retrieve process																									
            InitDataSetting(IcropDealerCode, IcropBranchCode, CountryCode)

            'Call Sales data retrieve process	
            If (IC3802801TableAdapter.GetSales(CStr(SalesId)).Rows.Count > 0) Then
                GlErrStepInfo = "ProspectCustomer1_1(Get Sales Data)"
                SalesData = CType(IC3802801TableAdapter.GetSales(CStr(SalesId)).Rows(0), IC3802801SalesRow)
            End If

            'If sales data cannot be retrieved	
            If (SalesData.SALES_ID = "") Then
                'Call sales temporary information retrieve process	
                If (IC3802801TableAdapter.GetSalesTemp(CStr(SalesId)).Rows.Count > 0) Then
                    GlErrStepInfo = "ProspectCustomer1_2(Get Sales Temp Data)"
                    SalesTempData = CType(IC3802801TableAdapter.GetSalesTemp(CStr(SalesId)).Rows(0), IC3802801SalesTempRow)
                    '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
                    SalesLocalData = CType(IC3802801TableAdapter.GetSalesLocalSource2CD(CStr(SalesId)).Rows(0), IC3802801SalesLocalRow)
                    '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end
                End If

                'If sales temporary information cannot be retrieved																								
                If (SalesTempData.SALES_ID = "") Then
                    GlErrStepInfo = "ProspectCustomer1_3"
                    'Ouput error log information																							
                    blnRtnCode = WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgNoData01, SalesData.SALES_ID)
                    'Set return value (end process)																							
                    Return xmlData
                End If

                'Set first time success flag (In case of first time success)																								
                FirstSuccessFlg = CstStrFirstSuccess
            Else
                GlErrStepInfo = "ProspectCustomer1_4"
                'Set first time success flag (In case of normal contract)																								
                FirstSuccessFlg = CstStrSalesNormal
            End If

            'In case of normal contract																									
            If (FirstSuccessFlg = CstStrSalesNormal) Then
                GlErrStepInfo = "ProspectCustomer1_A3(FirstSuccessFlg=Normal)"

                '活動データ連携対象の場合,下記処理を行う																								
                If (GlActivityJudgeFlg = CstStrLinkageOn) Then
                    GlErrStepInfo = "ProspectCustomer1_A4(Activity Linkage On)"

                    'If request iD of sales data is set																							
                    If (SalesData.REQ_ID <> 0) Then
                        GlErrStepInfo = "ProspectCustomer1_A5(Get Request Data)"
                        'Call FollowUp retrieve process (request)	
                        If (IC3802801TableAdapter.GetFollowUpRequest(CLng(SalesData.REQ_ID)).Rows.Count > 0) Then
                            RequestData = CType(IC3802801TableAdapter.GetFollowUpRequest(CLng(SalesData.REQ_ID)).Rows(0), IC3802801FollowUpRequestRow)
                        End If

                        GlErrStepInfo = "ProspectCustomer1_A6"
                        'If FollowUp retrieve process (request) information cannot be retrieved																						
                        If (RequestData.REQ_ID = 0) Then
                            'Ouput error log information																					
                            blnRtnCode = WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgNoData02, RequestData.REQ_ID.ToString())
                            'Set return value (end process)																					
                            Return xmlData
                        End If

                        GlErrStepInfo = "ProspectCustomer1_A7(Get Request Action Data)"
                        'Call Request Action information retrieve process	
                        ActionData = IC3802801TableAdapter.GetRequestAction(CLng(RequestData.REQ_ID))

                        'If Action information cannot be retrieved																						
                        If (ActionData.Rows.Count = 0) Then
                            'Ouput error log information																					
                            blnRtnCode = WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgNoData04, "")
                            'Set return value (end process)																					
                            Return xmlData
                        End If

                        GlErrStepInfo = "ProspectCustomer1_A8"
                        'Repeat per number of Request Action information retrieved																						
                        For Each Row In ActionData
                            ' Acitivity SEQ manager check																			
                            ActionSeqData.ImportRow(CheckActionSeq(CLng(SalesData.SALES_ID), CstStrActTypeAction, CLng(Row.ACT_ID), UserName))

                            'Call FollowUp Box Sales data retieve process
                            '20140311 Fujita Zantei Upd Start
                            'GlErrStepInfo = "IcropDealerCode"
                            'GlErrStepInfo = IcropDealerCode
                            'GlErrStepInfo = "IcropBranchCode"
                            'GlErrStepInfo = IcropBranchCode
                            'GlErrStepInfo = "Clng(SalesData.SALES_ID)"
                            'GlErrStepInfo = Clng(SalesData.SALES_ID)
                            'GlErrStepInfo = "Clng(Row.ACT_ID)"
                            'GlErrStepInfo = Clng(Row.ACT_ID)
                            If Row.RSLT_FLG = "1" Then
                                GlErrStepInfo = "ProspectCustomer1_A8_1(Get FB Sales Data)"
                                'FllwUpBoxSalesData = IC3802801TableAdapter.GetFllwUpBoxSales(IcropDealerCode, IcropBranchCode, Clng(SalesData.SALES_ID), Clng(Row.ACT_ID))
                                FllwUpBoxSalesData2 = IC3802801TableAdapter.GetFllwUpBoxSales(IcropDealerCode, IcropBranchCode, CLng(SalesData.SALES_ID), CLng(Row.ACT_ID))
                                If FllwUpBoxSalesData2.Rows.Count <> 0 Then
                                    For indexI As Long = 0 To FllwUpBoxSalesData2.Rows.Count - 1 Step 1
                                        FllwUpBoxSalesData.ImportRow(FllwUpBoxSalesData2.Rows(indexI))
                                    Next
                                End If
                            End If
                            'GlErrStepInfo = "FllwUpBoxSalesData"
                            'GlErrStepInfo = FllwUpBoxSalesData.Rows.Count
                            'If (IC3802801TableAdapter.GetFllwUpBoxSales(IcropDealerCode, IcropBranchCode, Clng(SalesData.SALES_ID), Clng(Row.ACT_ID)).Rows.Count > 0) Then
                            '    For Each FllwUpBoxSales As IC3802801DataSet.IC3802801GetFllwUpBoxSalesRow In IC3802801TableAdapter.GetFllwUpBoxSales(IcropDealerCode, IcropBranchCode, Clng(SalesData.SALES_ID), Clng(Row.ACT_ID))
                            '        FllwUpBoxSalesData.ImportRow(FllwUpBoxSales)
                            '    Next

                            'End If
                            '20140311 Fujita Zantei Upd End
                            GlErrStepInfo = "ProspectCustomer1_A9"
                            ' In case of actual activity information												
                            If (Row.RSLT_FLG = "1") Then
                                GlErrStepInfo = "ProspectCustomer1_A10"
                                '$FSBT90,91,92_20130228_by_chatchai_Start
                                'In case of Sales activity data linkage subject, perform the following process																		
                                If (GlProcessJudgeFlg = CstStrLinkageOn) Then

                                    GlErrStepInfo = "ProspectCustomer1_A10_2(Get Sales Action Data)"
                                    'GlErrStepInfo = "GetSalesAction(KEY_INFO)"
                                    'GlErrStepInfo = "SalesData.SALES_ID:" + SalesData.SALES_ID.ToString()
                                    'GlErrStepInfo = "Row.ACT_ID:" + Row.ACT_ID.ToString()
                                    ' Call Sales activity retrieve process 																	
                                    'takeda_update_start_20140412
                                    'SalesActData = IC3802801TableAdapter.GetSalesAction(Clng(SalesData.SALES_ID), Clng(Row.ACT_ID))              ' Repeat process by the number of retrieved sales activity information																	
                                    'For Each RowSalesAct In SalesActData
                                    SalesActDataCheck = IC3802801TableAdapter.GetSalesAction(CLng(SalesData.SALES_ID), CLng(Row.ACT_ID))              ' Repeat process by the number of retrieved sales activity information																	
                                    For Each RowSalesAct In SalesActDataCheck
                                        'takeda_update_end_20140412
                                        GlErrStepInfo = "ProspectCustomer1_A10_3"
                                        'takeda_update_start_20140412
                                        SalesActData.ImportRow(RowSalesAct)
                                        'takeda_update_end_20140412
                                        'Activity SEQ Manager Check 																
                                        SalesActionSeqData.ImportRow(CheckActionSeq(CLng(SalesData.SALES_ID), CstStrActTypeSalesAction, CLng(RowSalesAct.SALES_ACT_ID), UserName))
                                        'takeda_update_start_20140412
                                        ActionSeqData.ImportRow(CheckActionSeq(CLng(SalesData.SALES_ID), CstStrActTypeSalesAction, CLng(RowSalesAct.SALES_ACT_ID), UserName))
                                        'takeda_update_end_20140412
                                    Next
                                End If

                                GlErrStepInfo = "ProspectCustomer1_A11_1(Get Action Memo Data)(ActionMemo)"
                                '$FSBT90,91,92_20130228_by_chatchai_End
                                ' Call Activity memo retrieve process	
                                If (IC3802801TableAdapter.GetActionMemo(CstStrActionType, CLng(Row.ACT_ID)).Rows.Count > 0) Then
                                    For Each ActionMemoRow As IC3802801DataSet.IC3802801ActionMemoRow In IC3802801TableAdapter.GetActionMemo(CstStrActionType, CLng(Row.ACT_ID))
                                        ActionMemoData.ImportRow(ActionMemoRow)
                                    Next
                                End If

                                GlErrStepInfo = "ProspectCustomer1_A11_2(Get Action Memo Data)(NegotiationMemo)"
                                ' Call Sales activity memo retrieve process	
                                If (IC3802801TableAdapter.GetActionMemo(CstStrActionType, CLng(Row.ACT_ID)).Rows.Count > 0) Then
                                    For Each ActionMemoRow As IC3802801DataSet.IC3802801ActionMemoRow In IC3802801TableAdapter.GetActionMemo(CstStrActionType, CLng(Row.ACT_ID))
                                        NegotiationMemoData.ImportRow(ActionMemoRow)
                                    Next
                                End If

                            End If
                        Next

                        GlErrStepInfo = "ProspectCustomer1_A12"
                        'Retrieve request source code (request)																						
                        ReqSrc1stCd = RequestData.SOURCE_1_CD ' request source (1st) code																		
                        ReqSrc2ndCd = RequestData.SOURCE_2_CD ' request source (2nd) code																		
                        LastActCd = RequestData.LAST_ACT_ID 'last activity ID	

                        GlErrStepInfo = "ProspectCustomer1_A13(Get Request Action Data (Lastest))"
                        'Retrieve Activity ID (Newest) From Request Action information	
                        If (IC3802801TableAdapter.GetLastRequestActionId(CLng(RequestData.REQ_ID)).Rows.Count > 0) Then
                            GlErrStepInfo = "ProspectCustomer1_GetLastRequestActionId_1"
                            LastActionKeyData = CType(IC3802801TableAdapter.GetLastRequestActionId(CLng(RequestData.REQ_ID)).Rows(0), IC3802801ActionRow)
                            'GlErrStepInfo = "LastActionKeyData(ACT_ID):" + LastActionKeyData.ACT_ID.ToString()
                            'Retrieve Activity Data (Newest)
                            If (IC3802801TableAdapter.GetAction(CStr(LastActionKeyData.ACT_ID)).Rows.Count > 0) Then
                                GlErrStepInfo = "ProspectCustomer1_GetAction"
                                FollowUpResultData = CType(IC3802801TableAdapter.GetAction(CStr(LastActionKeyData.ACT_ID)).Rows(0), IC3802801ActionRow)
                                GlErrStepInfo = "FollowUpResultData(Request)"
                                'GlErrStepInfo = "FollowUpResultData(ACT_ID):" + FollowUpResultData.ACT_ID.ToString()
                                'GlErrStepInfo = "FollowUpResultData(REQ_ID):" + FollowUpResultData.REQ_ID.ToString()
                                'GlErrStepInfo = "FollowUpResultData(ACT_COUNT):" + FollowUpResultData.ACT_COUNT.ToString()
                                'GlErrStepInfo = "FollowUpResultData(SCHE_DATEORTIME):" + FollowUpResultData.SCHE_DATEORTIME.ToString()
                                'GlErrStepInfo = "FollowUpResultData(SCHE_DLR_CD):" + FollowUpResultData.SCHE_DLR_CD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(SCHE_BRN_CD):" + FollowUpResultData.SCHE_BRN_CD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(SCHE_STF_CD):" + FollowUpResultData.SCHE_STF_CD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(SCHE_CONTACT_MTD):" + FollowUpResultData.SCHE_CONTACT_MTD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_DATETIME):" + FollowUpResultData.RSLT_DATETIME.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_DLR_CD):" + FollowUpResultData.RSLT_DLR_CD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_BRN_CD):" + FollowUpResultData.RSLT_BRN_CD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_STF_CD):" + FollowUpResultData.RSLT_STF_CD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_CONTACT_MTD):" + FollowUpResultData.RSLT_CONTACT_MTD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(ACT_STATUS):" + FollowUpResultData.ACT_STATUS.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_ID):" + FollowUpResultData.RSLT_ID.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_ID):" + FollowUpResultData.RSLT_ID.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_SALES_PROSPECT_CD):" + FollowUpResultData.RSLT_SALES_PROSPECT_CD.ToString()
                            End If

                        End If

                        GlErrStepInfo = "ProspectCustomer1_A14(Get Request Action Data (First))"
                        'Call First request action information retrieve process																						
                        FirstActionData = IC3802801TableAdapter.GetFirstRequestAction(CLng(RequestData.REQ_ID), " ")
                        StrActStatus = CstStrCold & "," & CstStrWarm & "," & CstStrHot

                        GlErrStepInfo = "ProspectCustomer1_A15(Get Request Action Data (Sales Prospect Code))"
                        'Call Request action status retieve process	
                        If (IC3802801TableAdapter.GetStatusRequestAction(CLng(RequestData.REQ_ID), StrActStatus, CstStrActRsltFlgOn).Rows.Count > 0) Then
                            StatusActionData = CType(IC3802801TableAdapter.GetStatusRequestAction(CLng(RequestData.REQ_ID), StrActStatus, CstStrActRsltFlgOn).Rows(0), IC3802801ActionRow)
                        End If

                        GlErrStepInfo = "ProspectCustomer1_A16"
                        'In other than above case (sales data attract ID is set),																							
                    Else
                        GlErrStepInfo = "ProspectCustomer1_A17(Get Attract Data)"
                        'Call FollowUp retrieve process (attract)		
                        If (IC3802801TableAdapter.GetFollowUpAttract(CLng(SalesData.ATT_ID)).Rows.Count > 0) Then
                            AttractData = CType(IC3802801TableAdapter.GetFollowUpAttract(CLng(SalesData.ATT_ID)).Rows(0), IC3802801FollowUpAttractRow)
                        End If

                        GlErrStepInfo = "ProspectCustomer1_A18"
                        'If FollowUp retrieve process (attract) information cannot be retrieved																						
                        If (AttractData.ATT_ID = 0) Then
                            'Ouput error log information																					
                            blnRtnCode = WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgNoData03, CStr(AttractData.ATT_ID))
                            'Set return value (end process)																					
                            Return xmlData
                        End If

                        GlErrStepInfo = "ProspectCustomer1_A18_2(Get Attract Action Data)"
                        'Call Attract Action information retrieve process																						
                        ActionData = IC3802801TableAdapter.GetAttractAction(CLng(AttractData.ATT_ID))

                        'If Action information cannot be retrieved																						
                        If (ActionData.Rows.Count = 0) Then
                            'Ouput error log information																					
                            blnRtnCode = WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgNoData04, "")
                            'Set return value (end process)																					
                            Return xmlData
                        End If

                        GlErrStepInfo = "ProspectCustomer1_A19"
                        'Repeat per number of Attract Action information retrieved																						
                        For Each Row In ActionData
                            GlErrStepInfo = "ProspectCustomer1_A19_2(Check Action Seq Manager)"
                            ' Activity SEQ manager check																			
                            ActionSeqData.ImportRow(CheckActionSeq(CLng(SalesData.SALES_ID), CstStrActTypeAction, CLng(Row.ACT_ID), UserName))

                            GlErrStepInfo = "ProspectCustomer1_A20(Get FB Sales Data)"
                            'Call FollowUp Box Sales data retieve process	
                            '20140311 Fujita Upd Start
                            'FllwUpBoxSalesData = IC3802801TableAdapter.GetFllwUpBoxSales(IcropDealerCode, IcropBranchCode, Clng(SalesData.SALES_ID), Clng(Row.ACT_ID))
                            FllwUpBoxSalesData2 = IC3802801TableAdapter.GetFllwUpBoxSales(IcropDealerCode, IcropBranchCode, CLng(SalesData.SALES_ID), CLng(Row.ACT_ID))
                            If FllwUpBoxSalesData2.Rows.Count <> 0 Then
                                For indexJ As Long = 0 To FllwUpBoxSalesData2.Rows.Count - 1 Step 1
                                    FllwUpBoxSalesData.ImportRow(FllwUpBoxSalesData2.Rows(indexJ))
                                Next
                            End If
                            'If (IC3802801TableAdapter.GetFllwUpBoxSales(IcropDealerCode, IcropBranchCode, Clng(SalesData.SALES_ID), Clng(Row.ACT_ID)).Rows.Count > 0) Then
                            '    For Each FllwUpBoxSales As IC3802801DataSet.IC3802801GetFllwUpBoxSalesRow In IC3802801TableAdapter.GetFllwUpBoxSales(IcropDealerCode, IcropBranchCode, Clng(SalesData.SALES_ID), Clng(Row.ACT_ID))
                            '        FllwUpBoxSalesData.ImportRow(FllwUpBoxSales)
                            '    Next
                            'End If
                            '20140311 Fujita Upd End

                            ' In case of actual activity information												
                            If (Row.RSLT_FLG = "1") Then
                                GlErrStepInfo = "ProspectCustomer1_A21"
                                '$FSBT90,91,92_20130228_by_chatchai_Start
                                'In case Sales activity data linkage is subject, perform the following process																		
                                If (GlProcessJudgeFlg = CstStrLinkageOn) Then
                                    ' Call Sales activity retrieve process																	
                                    GlErrStepInfo = "ProspectCustomer1_A21_2(Get Sales Action Data)"
                                    'GlErrStepInfo = "GetSalesAction(KEY_INFO)"
                                    'GlErrStepInfo = "SalesData.SALES_ID:" + SalesData.SALES_ID.ToString()
                                    'GlErrStepInfo = "Row.ACT_ID:" + Row.ACT_ID.ToString()
                                    'takeda_update_start_20140412
                                    '    SalesActData = IC3802801TableAdapter.GetSalesAction(Clng(SalesData.SALES_ID), Clng(Row.ACT_ID))             ' Repeat process by the number of retrieved sales activity information																	
                                    'For Each RowSalesAct In SalesActData
                                    SalesActDataCheck = IC3802801TableAdapter.GetSalesAction(CLng(SalesData.SALES_ID), CLng(Row.ACT_ID))             ' Repeat process by the number of retrieved sales activity information																	
                                    For Each RowSalesAct In SalesActDataCheck
                                        'takeda_update_end_20140412
                                        GlErrStepInfo = "ProspectCustomer1_A21_3(Check Action Seq Manager)"
                                        'takeda_update_start_20140412
                                        SalesActData.ImportRow(RowSalesAct)
                                        'takeda_update_end_20140412
                                        'Activity SEQ Manager Check 																
                                        SalesActionSeqData.ImportRow(CheckActionSeq(CLng(SalesData.SALES_ID), CstStrActTypeSalesAction, CLng(RowSalesAct.SALES_ACT_ID), UserName))
                                        'takeda_update_start_20140412
                                        ActionSeqData.ImportRow(CheckActionSeq(CLng(SalesData.SALES_ID), CstStrActTypeSalesAction, CLng(RowSalesAct.SALES_ACT_ID), UserName))
                                        'takeda_update_start_20140412
                                    Next
                                End If

                                '$FSBT90,91,92_20130228_by_chatchai_End

                                ' Call activity memo retrieve process	
                                For Each ActionMemoRow As IC3802801DataSet.IC3802801ActionMemoRow In IC3802801TableAdapter.GetActionMemo(CstStrActionType, CLng(Row.ACT_ID))
                                    ActionMemoData.ImportRow(ActionMemoRow)
                                Next

                                GlErrStepInfo = "ProspectCustomer1_A22(Get Action Memo Data)"
                                ' Call Sales activity memo retrieve process		
                                If (IC3802801TableAdapter.GetActionMemo(CstStrActionType, CLng(Row.ACT_ID)).Rows.Count > 0) Then
                                    For Each ActionMemoRow As IC3802801DataSet.IC3802801ActionMemoRow In IC3802801TableAdapter.GetActionMemo(CstStrActionType, CLng(Row.ACT_ID))
                                        NegotiationMemoData.ImportRow(ActionMemoRow)
                                    Next
                                End If

                            End If

                        Next
                        GlErrStepInfo = "ProspectCustomer1_A23"

                        'Retrieve request source code (attract)																						
                        ReqSrc1stCd = AttractData.SOURCE_1_CD.ToString()
                        'takeda_update_start_20140609
                        'ReqSrc2ndCd = AttractData.SOURCE_1_CD.ToString()
                        ReqSrc2ndCd = AttractData.SOURCE_2_CD.ToString()
                        'takeda_update_end_20140609
                        LastActId = AttractData.LAST_ACT_ID

                        'Retrieve Activity ID (Newest) From Attract Action information
                        If (IC3802801TableAdapter.GetLastAttractActionId(CLng(AttractData.ATT_ID)).Rows.Count > 0) Then
                            GlErrStepInfo = "ProspectCustomer1_A24(Get Attract Action Data (Lastest)"
                            LastActionKeyData = CType(IC3802801TableAdapter.GetLastAttractActionId(CLng(AttractData.ATT_ID)).Rows(0), IC3802801ActionRow)
                            'Retrieve Activity Data (Newest)	
                            If (IC3802801TableAdapter.GetAction(CStr(LastActionKeyData.ACT_ID)).Rows.Count > 0) Then
                                GlErrStepInfo = "ProspectCustomer1_A25(Get Action Data)"
                                FollowUpResultData = CType(IC3802801TableAdapter.GetAction(CStr(LastActionKeyData.ACT_ID)).Rows(0), IC3802801ActionRow)
                                GlErrStepInfo = "FollowUpResultData(Attract)"
                                'GlErrStepInfo = "FollowUpResultData(ACT_ID):" + FollowUpResultData.ACT_ID.ToString()
                                'GlErrStepInfo = "FollowUpResultData(REQ_ID):" + FollowUpResultData.REQ_ID.ToString()
                                'GlErrStepInfo = "FollowUpResultData(ACT_COUNT):" + FollowUpResultData.ACT_COUNT.ToString()
                                'GlErrStepInfo = "FollowUpResultData(SCHE_DATEORTIME):" + FollowUpResultData.SCHE_DATEORTIME.ToString()
                                'GlErrStepInfo = "FollowUpResultData(SCHE_DLR_CD):" + FollowUpResultData.SCHE_DLR_CD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(SCHE_BRN_CD):" + FollowUpResultData.SCHE_BRN_CD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(SCHE_STF_CD):" + FollowUpResultData.SCHE_STF_CD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(SCHE_CONTACT_MTD):" + FollowUpResultData.SCHE_CONTACT_MTD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_DATETIME):" + FollowUpResultData.RSLT_DATETIME.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_DLR_CD):" + FollowUpResultData.RSLT_DLR_CD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_BRN_CD):" + FollowUpResultData.RSLT_BRN_CD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_STF_CD):" + FollowUpResultData.RSLT_STF_CD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_CONTACT_MTD):" + FollowUpResultData.RSLT_CONTACT_MTD.ToString()
                                'GlErrStepInfo = "FollowUpResultData(ACT_STATUS):" + FollowUpResultData.ACT_STATUS.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_ID):" + FollowUpResultData.RSLT_ID.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_ID):" + FollowUpResultData.RSLT_ID.ToString()
                                'GlErrStepInfo = "FollowUpResultData(RSLT_SALES_PROSPECT_CD):" + FollowUpResultData.RSLT_SALES_PROSPECT_CD.ToString()
                            End If
                        End If
                        GlErrStepInfo = "ProspectCustomer1_A26"

                        'Call First Attract action information retrieve process																						
                        FirstActionData = IC3802801TableAdapter.GetFirstAttractAction(CLng(AttractData.ATT_ID), " ")

                        StrActStatus = CstStrCold & "," & CstStrWarm & "," & CstStrHot

                        'Call Attract action status retieve process	
                        If (IC3802801TableAdapter.GetStatusAttractAction(CLng(AttractData.ATT_ID), StrActStatus, CstStrActRsltFlgOn).Rows.Count > 0) Then
                            GlErrStepInfo = "ProspectCustomer1_A27(Get Attract Action (Sales Prospect Code)"
                            StatusActionData = CType(IC3802801TableAdapter.GetStatusAttractAction(CLng(AttractData.ATT_ID), StrActStatus, CstStrActRsltFlgOn).Rows(0), IC3802801ActionRow)
                        End If

                    End If

                    GlErrStepInfo = "ProspectCustomer1_A28"

                    'ISSUE-0023_20130219_by_chatchai_Start
                    ''Call request source retrieve process	※Use request source code gotten from request or attract
                    'If (IC3802801TableAdapter.GetReqSource(ReqSrc1stCd, ReqSrc2ndCd).Rows.Count > 0) Then
                    '    ReqSrcData = IC3802801TableAdapter.GetReqSource(ReqSrc1stCd, ReqSrc2ndCd).Rows(0)
                    'End If

                    'GlErrStepInfo = "GetReqSource1:" + ReqSrc1stCd.ToString()
                    'Call request source(1st) retrieve process	        ※Use request source code gotten from request or attract
                    If (IC3802801TableAdapter.GetReqSource1(CLng(ReqSrc1stCd)).Rows.Count > 0) Then
                        ReqSrcData1 = CType(IC3802801TableAdapter.GetReqSource1(CLng(ReqSrc1stCd)).Rows(0), IC3802801ReqSource1Row)
                        GlErrStepInfo = "ProspectCustomer1_A29(Get Source Name1)"
                    End If

                    'takeda_update_start_20140708_ソース２名称の取得判定のあやまり対応
                    'If (CDbl(ReqSrcData1.SOURCE_1_CD) <> 0) Then
                    If (CDbl(ReqSrc1stCd) <> 0) Then
                        'takeda_update_end_20140708_ソース２名称の取得判定のあやまり対応
                        'Call request source(2nd) retrieve process	※Use request source code gotten from request or attract	
                        If (IC3802801TableAdapter.GetReqSource2(CLng(ReqSrc1stCd), CLng(ReqSrc2ndCd)).Rows.Count > 0) Then
                            ReqSrcData2 = CType(IC3802801TableAdapter.GetReqSource2(CLng(ReqSrc1stCd), CLng(ReqSrc2ndCd)).Rows(0), IC3802801ReqSource2Row)
                            GlErrStepInfo = "ProspectCustomer1_A30(Get Sourc Name2)"
                        End If
                    End If
                    'ISSUE-0023_20130219_by_chatchai_End

                End If

                'GlErrStepInfo="ProspectCustomer1_B1")
                ''商談活動データ連携対象の場合,下記処理を行う																								
                'If (GlProcessJudgeFlg = CstStrLinkageOn) Then
                '    GlErrStepInfo="ProspectCustomer1_B2")
                '    'Call Sales action data retrieve process																							
                '    SalesActData = IC3802801TableAdapter.GetSalesAction(SalesData.SALES_ID)

                '    'Repeat per number of Sales action data retrieved																							
                '    For Each Row In SalesActData
                '        ' Activity SEQ manager check																				
                '        ActionSeqData.ImportRow(checkActionSeq(SalesData.SALES_ID, CstStrActTypeSalesAction, Row.SALES_ACT_ID, UserName))

                '        'ISSUE-Sales_action_Date_20130220_by_chatchai_Start
                '        ''Call FollowUp Box Sales data retieve process	
                '        'If (IC3802801TableAdapter.GetFllwUpBoxSales(DlrCd, BranchCd, Row.SALES_ID, Row.SALES_ACT_ID).Rows.Count > 0) Then
                '        '    'FllwUpBoxSalesData = IC3802801TableAdapter.GetFllwUpBoxSales(DlrCd, BranchCd, Row.SALES_ID, Row.ACT_ID).Rows(0)
                '        '    For Each FllwUpBoxSales As IC3802801DataSet.IC3802801GetFllwUpBoxSalesRow In IC3802801TableAdapter.GetFllwUpBoxSales(DlrCd, BranchCd, Row.SALES_ID, Row.SALES_ACT_ID)
                '        '        FllwUpBoxSalesData.ImportRow(FllwUpBoxSales)
                '        '    Next
                '        'End If
                '        'ISSUE-Sales_action_Date_20130220_by_chatchai_End
                '    Next
                'End If

                GlErrStepInfo = "ProspectCustomer1_C1(Get Prefer Vehicle Data)"
                'Call preferred vehicle retrieve process																								
                SelectedSeriesData = IC3802801TableAdapter.GetSelectedSeries(SalesData.SALES_ID)

                '$ISSUE-0032_20130228_by_chatchai_Start
                ' Call Estimation information retrieve process	
                'GlErrStepInfo="IcropDealerCode"
                'GlErrStepInfo=IcropDealerCode
                'GlErrStepInfo="IcropBranchCode"
                'GlErrStepInfo=IcropBranchCode
                'GlErrStepInfo="CStr(SalesId)"
                'GlErrStepInfo=CStr(SalesId)
                'GlErrStepInfo="CstStrDeleteFlgOff"
                'GlErrStepInfo=CstStrDeleteFlgOff
                '20140310 Fujita Upd Start
                'SalesData.SALES_COMPLETE_FLG = Success
                If (SalesData.SALES_COMPLETE_FLG = "1") Then
                    For Each Row In ActionData
                        If (Row.ACT_STATUS = "31") Then
                            If (IC3802801TableAdapter.GetEstimateInfo(IcropDealerCode, IcropBranchCode, CStr(SalesId), CstStrDeleteFlgOff).Rows.Count > 0) Then
                                GlErrStepInfo = "ProspectCustomer1_C1_3(Get Estimate Data)"
                                EstimateData = CType(IC3802801TableAdapter.GetEstimateInfo(IcropDealerCode, IcropBranchCode, CStr(SalesId), CstStrDeleteFlgOff).Rows(0), IC3802801EstimateInfoRow)
                                ' Call Estimation vehicle retrieve process
                                GlErrStepInfo = "ProspectCustomer1_C1_4(Get Estimate Vehicle Data)"
                                EstimateVclDataT = IC3802801TableAdapter.GetEstimateVclInfo(CLng(EstimateData.ESTIMATEID))
                                EstimateVclData = CType(EstimateVclDataT.Rows(0), IC3802801EstimateVclInfoRow)
                                'If (IC3802801TableAdapter.GetEstimateVclInfo(Clng(EstimateData.ESTIMATEID)).Rows.Count > 0) Then
                                '    EstimateVclData = CType(IC3802801TableAdapter.GetEstimateVclInfo(Clng(EstimateData.ESTIMATEID)).Rows(0), IC3802801EstimateVclInfoRow)
                            End If
                        End If
                    Next
                    '20140310 Fujita Upd End
                End If
                '$ISSUE-0032_20130228_by_chatchai_End

                ''Repeat per number of preferred vehicle information retrieved																								
                'For Each Row In SelectedSeriesData
                '    GlErrStepInfo="ProspectCustomer1_C1_1")
                '    'If preferred vehicle information agreement activity ID is set																							
                '    '(If agreed activity information exists)
                '    If (Row.SALESBKG_ACT_ID <> 0) Then
                '        'Call activity information retrieve process									※Use activity ID gotten from request or attract
                '        If (IC3802801TableAdapter.GetAction(Row.SALESBKG_ACT_ID).Rows.Count > 0) Then
                '            GlErrStepInfo="ProspectCustomer1_C1_2")
                '            FollowUpActionData = IC3802801TableAdapter.GetAction(Row.SALESBKG_ACT_ID).Rows(0)    ' Row. agreement activity ID
                '        End If

                '    End If
                '    'End If

                'Next

                'GlErrStepInfo="ProspectCustomer1_C2")
                ''Call Action Result information retrieve process											※ Use action result ID for action data of contract vehicle													
                'If (FollowUpActionData.ACT_ID.Trim() <> "") Then
                '    If (IC3802801TableAdapter.GetActionResult(FollowUpActionData.RSLT_ID).Rows.Count > 0) Then
                '        ActionResultData = IC3802801TableAdapter.GetActionResult(FollowUpActionData.RSLT_ID).Rows(0)    ' FollowUpActionData. action result ID
                '    End If
                'End If
                'Call competitor vehicle data retrieve process
                GlErrStepInfo = "ProspectCustomer1_C2(Get Competitor Vehicle Data)"
                CompetitorSeriesData = IC3802801TableAdapter.GetCompetitorSeries(SalesData.SALES_ID)

                GlErrStepInfo = "ProspectCustomer1_C3"
                'Repeat per number of competitor vehicle data information retrieved																								
                For Each Row In CompetitorSeriesData
                    'If sales ID of competitor vehicle data is set																							
                    '(If competitor vehicle data exists)																							
                    If (Row.SALES_ID <> "0") Then
                        'Call maker model retrieve process (competitor vehicle information)
                        If (IC3802801TableAdapter.GetMakerModel(Row.MODEL_CD).Rows.Count > 0) Then
                            GlErrStepInfo = "ProspectCustomer1_C3_1(Get Maker & Model Data)"
                            MakerModelData.ImportRow(IC3802801TableAdapter.GetMakerModel(Row.MODEL_CD).Rows(0))
                        End If
                    End If

                    'If give-up competitor vehicle seq of sales data  is competitor vehicle seq of competitor vehicle data is set																					
                    If (Row.COMP_VCL_SEQ = SalesData.GIVEUP_COMP_VCL_SEQ) Then
                        'Call maker model retrieve process（competitor vehicle information of FollowUpResult)
                        If (IC3802801TableAdapter.GetMakerModel(Row.MODEL_CD).Rows.Count > 0) Then
                            GlErrStepInfo = "ProspectCustomer1_C3_2(Get Maker & Model Data(Giveup))"
                            GiveUpMakerModelData = CType(IC3802801TableAdapter.GetMakerModel(Row.MODEL_CD).Rows(0), IC3802801MakerModelRow)
                            GiveUpMakerCd = GiveUpMakerModelData.MAKER_CD
                        End If
                    End If

                Next

                GlErrStepInfo = "ProspectCustomer1_C4(Get FB Sales Condition Data)"
                'GlErrStepInfo="IcropDealerCode"
                'GlErrStepInfo=IcropDealerCode
                'GlErrStepInfo="IcropBranchCode"
                'GlErrStepInfo=IcropBranchCode
                'Call sales condition retrieve process																								
                SalesConditionData = IC3802801TableAdapter.GetSalesCondition(IcropDealerCode, IcropBranchCode, CLng(SalesData.SALES_ID), SalesData.CST_ID)

                'takeda_update_start_20140412
                '    'If sales condition cannot be retrieved																								
                'If (SalesConditionData.Rows.Count = 0) Then
                '    'Edit key information																							
                '    StrErrKey = IcropDealerCode & "," & IcropBranchCode & "," & SalesData.SALES_ID & "," & SalesData.CST_ID
                '    'Ouput error log information																							
                '    blnRtnCode = WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgNoData05, StrErrKey.ToString())
                '    'Set return value (end process)																							
                '    Return xmlData
                'End If
                'takeda_update_end_20140412

                GlErrStepInfo = "ProspectCustomer1_C5(Get Dealer Customer Vehicle Data)"
                'Set customer vehicle class (Owner)																								
                StrCstVclKbn = CstStrOwner
                'Call dealer customer vehicle retrieve process	
                '$追加作業（１）_20130228_by_chatchai_Start
                DlrCstVclData = IC3802801TableAdapter.GetDlrCstVcl(IcropDealerCode, CLng(SalesData.CST_ID), StrCstVclKbn, CstStrOwnerChanged)
                '$追加作業（１）_20130228_by_chatchai_End

                'If dealer vehicle information cannot be retrieved																								
                If (DlrCstVclData.Rows.Count = 0) Then
                    'Edit key information																							
                    StrErrKey = IcropDealerCode & "," & CLng(SalesData.CST_ID) & "," & StrCstVclKbn & "," & CstStrOwnerChanged & ""
                    'Ouput error log information																							
                    blnRtnCode = WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgNoData06, StrErrKey.ToString())
                    'Set return value (end process)	
                    '20140310 FUJITA Del START
                    'Return xmlData
                    '20140310 FUJITA Del END
                End If


                GlErrStepInfo = "ProspectCustomer1_C6(Get Vehicle Data)"
                ' Repeat process by the number of retrieved dealer vehicle data information
                Dim VehicleRow As IC3802801DataSet.IC3802801VehicleRow
                Dim MakerRow As IC3802801DataSet.IC3802801MakerModelRow
                For Each Row In DlrCstVclData
                    'Call vehicle data retrieve process	
                    If (IC3802801TableAdapter.GetVehicle(IcropDealerCode, CLng(Row.VCL_ID)).Rows.Count > 0) Then
                        GlErrStepInfo = "ProspectCustomer1_C6_1(Get Vehicle Data)"
                        VehicleRow = CType(IC3802801TableAdapter.GetVehicle(IcropDealerCode, CLng(Row.VCL_ID)).Rows(0), IC3802801VehicleRow)
                        VehicleData.ImportRow(VehicleRow)

                        If (IC3802801TableAdapter.GetMakerModel(VehicleRow.MODEL_CD).Rows.Count > 0) Then
                            GlErrStepInfo = "ProspectCustomer1_C6_2(Get Maker & Model Data(Vehicle))"
                            MakerRow = CType(IC3802801TableAdapter.GetMakerModel(VehicleRow.MODEL_CD).Rows(0), IC3802801MakerModelRow)
                            VehicleMakerModelData.ImportRow(MakerRow)
                        End If
                    End If

                Next
                GlErrStepInfo = "ProspectCustomer1_C7(Get Customer Data)"
                'Call customer data retrieve process	
                If (IC3802801TableAdapter.GetCustomer(IcropDealerCode, CLng(SalesData.CST_ID)).Rows.Count > 0) Then
                    CustomerData = CType(IC3802801TableAdapter.GetCustomer(IcropDealerCode, CLng(SalesData.CST_ID)).Rows(0), IC3802801CustomerRow)
                    'takeda_update_start_20140425
                    'GlErrStepInfo = "@@@DataCheck(GetCustomer):"
                    'GlErrStepInfo = "(DB)CustomerData.CST_ADDRESS"
                    'GlErrStepInfo = CustomerData.CST_ADDRESS
                    'GlErrStepInfo = "(DB)CustomerData.CST_ADDRESS_1"
                    'GlErrStepInfo = CustomerData.CST_ADDRESS_1
                    'takeda_update_end_20140425
                End If

                'If customer information cannot retrieved																								
                If (CustomerData.CST_ID = "") Then
                    'Ouput error log information																							
                    blnRtnCode = WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgNoData07, "")
                    'Set return value (end process)																							
                    Return xmlData
                End If

                GlErrStepInfo = "ProspectCustomer1_C8"
                'If customer information exists																								
                If (CustomerData.CST_ID <> "0") Then
                    ' Call dealer customer memo retrieve process	
                    If (IC3802801TableAdapter.GetDlrCustomerMemo(IcropDealerCode, CLng(CustomerData.CST_ID)).Rows.Count > 0) Then
                        GlErrStepInfo = "ProspectCustomer1_C8_1_1(Get Dealer Customer Memo Data)"
                        DlrCustomerMemoData = CType(IC3802801TableAdapter.GetDlrCustomerMemo(IcropDealerCode, CLng(CustomerData.CST_ID)).Rows(0), IC3802801CustomerMemoRow)
                    End If

                    'ISSUE-0025_20130219_by_chatchai_Start
                    ''Call customer address data retrieve process
                    'If (IC3802801TableAdapter.GetCustomerAddress(CustomerData.CST_ADDRESS_STATE, CustomerData.CST_ADDRESS_DISTRICT, CustomerData.CST_ADDRESS_CITY, CustomerData.CST_ADDRESS_LOCATION).Rows.Count > 0) Then
                    '    CustomerAddressData = IC3802801TableAdapter.GetCustomerAddress(CustomerData.CST_ADDRESS_STATE, CustomerData.CST_ADDRESS_DISTRICT, CustomerData.CST_ADDRESS_CITY, CustomerData.CST_ADDRESS_LOCATION).Rows(0)
                    'End If

                    'Call State Information data retrieve process
                    ''@@@@@ commentStart

                    'ISSUE-IT2-1_by_takeda_start
                    StateInfoData.STATE_CD = ""
                    StateInfoData.STATE_NAME = ""
                    DistrictInfoData.DISTRICT_CD = ""
                    DistrictInfoData.DISTRICT_NAME = ""
                    CityInfoData.CITY_CD = ""
                    CityInfoData.CITY_NAME = ""
                    LocationInfoData.LOCATION_CD = ""
                    LocationInfoData.LOCATION_NAME = ""

                    'If (IC3802801TableAdapter.GetStateInfo(CustomerData.CST_ADDRESS_STATE).Rows.Count > 0) Then
                    If (String.IsNullOrEmpty(Trim(CustomerData.CST_ADDRESS_STATE))) Then
                        GlErrStepInfo = "ProspectCustomer1_C8_1(No State Code)"
                    Else
                        'ISSUE-IT2-1_by_takeda_end
                        GlErrStepInfo = "ProspectCustomer1_C8_2(Get State Data)"
                        StateInfoData = IC3802801TableAdapter.GetStateInfo(CustomerData.CST_ADDRESS_STATE).Rows(0)

                        'ISSUE-IT2-1_by_takeda_start
                        'End If
                        ''Call District Information data retrieve process
                        'If (IC3802801TableAdapter.GetDistrictInfo(StateInfoData.STATE_CD, CustomerData.CST_ADDRESS_DISTRICT).Rows.Count > 0) Then
                        If (String.IsNullOrEmpty(Trim(CustomerData.CST_ADDRESS_DISTRICT))) Then
                            GlErrStepInfo = "ProspectCustomer1_C8_3(No District Code)"
                        Else
                            'ISSUE-IT2-1_by_takeda_end
                            GlErrStepInfo = "ProspectCustomer1_C8_4(Get District Data)"

                            'ISSUE-IT2-1_by_takeda_start
                            '   DistrictInfoData = IC3802801TableAdapter.GetDistrictInfo(StateInfoData.STATE_CD, CustomerData.CST_ADDRESS_DISTRICT).Rows(0)
                            'End If
                            'If (DistrictInfoData.DISTRICT_CD <> "") Then
                            'Call City Information data retrieve process	
                            'If (IC3802801TableAdapter.GetCityInfo(StateInfoData.STATE_CD, DistrictInfoData.DISTRICT_CD, CustomerData.CST_ADDRESS_CITY).Rows.Count > 0) Then

                            DistrictInfoData = IC3802801TableAdapter.GetDistrictInfo(CustomerData.CST_ADDRESS_STATE, CustomerData.CST_ADDRESS_DISTRICT).Rows(0)

                            If (String.IsNullOrEmpty(Trim(CustomerData.CST_ADDRESS_CITY))) Then
                                GlErrStepInfo = "ProspectCustomer1_C8_5(No City Code)"
                            Else
                                'ISSUE-IT2-1_by_takeda_end
                                GlErrStepInfo = "ProspectCustomer1_C8_6(Get City Data)"

                                'ISSUE-IT2-1_by_takeda_start
                                '    CityInfoData = IC3802801TableAdapter.GetCityInfo(StateInfoData.STATE_CD, DistrictInfoData.DISTRICT_CD, CustomerData.CST_ADDRESS_CITY).Rows(0)
                                'End If
                                'If (CityInfoData.CITY_CD <> "") Then
                                CityInfoData = IC3802801TableAdapter.GetCityInfo(CustomerData.CST_ADDRESS_STATE, CustomerData.CST_ADDRESS_DISTRICT, CustomerData.CST_ADDRESS_CITY).Rows(0)

                                If (String.IsNullOrEmpty(Trim(CustomerData.CST_ADDRESS_LOCATION))) Then
                                    GlErrStepInfo = "ProspectCustomer1_C8_7(No Location Code)"
                                Else
                                    'ISSUE-IT2-1_by_takeda_end
                                    GlErrStepInfo = "ProspectCustomer1_C8_8(Get Location Data)"
                                    'ISSUE-IT2-1_by_takeda_start
                                    'Call Location Information data retrieve process																				
                                    'LocationInfoData = IC3802801TableAdapter.GetLocationInfo(StateInfoData.STATE_CD, DistrictInfoData.DISTRICT_CD, CityInfoData.CITY_CD, CustomerData.CST_ADDRESS_LOCATION).Rows(0)
                                    LocationInfoData = IC3802801TableAdapter.GetLocationInfo(CustomerData.CST_ADDRESS_STATE, CustomerData.CST_ADDRESS_DISTRICT, CustomerData.CST_ADDRESS_CITY, CustomerData.CST_ADDRESS_LOCATION).Rows(0)
                                    'ISSUE-IT2-1_by_takeda_end
                                End If
                            End If
                        End If
                    End If
                    'ISSUE-0025_20130219_by_chatchai_End

                End If

                GlErrStepInfo = "ProspectCustomer1_C9(Get Contact Timeslot)"
                'Call Customer contact timeslot retrieve process			
                If (IC3802801TableAdapter.GetContactTimeslot(CLng(SalesData.CST_ID), CstStrIcropTimeslot).Rows.Count > 0) Then
                    ContactTimeslotData = CType(IC3802801TableAdapter.GetContactTimeslot(CLng(SalesData.CST_ID), CstStrIcropTimeslot).Rows(0), IC3802801ContactTimeslotRow)
                End If

                'Call customer family data retrieve process	
                GlErrStepInfo = "ProspectCustomer1_C9_1(Get Family Infomation Data)"
                FamilyInfomationData = IC3802801TableAdapter.GetFamilyInfomation(SalesData.CST_ID)

                'Call customer hobby data retrieve process																								
                GlErrStepInfo = "ProspectCustomer1_C9_2(Get Hobby Data)"
                HobbyData = IC3802801TableAdapter.GetHobby(SalesData.CST_ID)

                '$追加作業（３）_20130228_by_chatchai_Start
                ' In case sales completion flag ="1" (sale complete)																					
                If (SalesData.SALES_COMPLETE_FLG = CstStrComplete) Then

                    ' In case of request activity																				
                    If (CDbl(RequestData.REQ_ID) <> 0) Then
                        GlErrStepInfo = "ProspectCustomer1_C9_3(Move History(Request Data))"
                        'Call History TBL data transfer process 																			
                        MoveHistory(CLng(SalesData.SALES_ID))

                        ' In case of attract activity																				
                    ElseIf (CDbl(AttractData.ATT_ID) <> 0) Then
                        ' In case of attract activity, perform pending attract activity data existance check																			
                        Dim AttCount As Long
                        If (IC3802801TableAdapter.CheckAttractData(CLng(AttractData.ATT_ID)).Rows.Count > 0) Then
                            GlErrStepInfo = "ProspectCustomer1_C9_4(Check Attract Data)(DM & RMM)"
                            Dim SumAttract As IC3802801DataSet.SumAttractRow
                            SumAttract = IC3802801TableAdapter.CheckAttractData(CLng(AttractData.ATT_ID)).Rows(0)
                            AttCount = CLng(SumAttract.CNT)
                        End If


                        ' If pending attract activity data does not exist																			
                        If (AttCount = 0) Then
                            GlErrStepInfo = "ProspectCustomer1_C9_5(Move History(Attract Data))"
                            'Call History TBL data transfer process 																		
                            MoveHistory(CLng(SalesData.SALES_ID))
                        End If

                    Else
                        ' Do nothing																			
                    End If
                End If

                '$追加作業（３）_20130228_by_chatchai_End

                GlErrStepInfo = "ProspectCustomer1_C10(OutPutProspectCustomer1)"
                'Output linkage file																								
                '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
                'xmlData = OutPutProspectCustomer1(IcropDealerCode, IcropBranchCode, CLng(SalesData.SALES_ID), FirstSuccessFlg, SalesData, SalesTempData, RequestData, AttractData, SalesActData, FllwUpBoxSalesData, ActionMemoData, FirstActionData, StatusActionData, ReqSrcData1, ReqSrcData2, SelectedSeriesData, CompetitorSeriesData, ActionData, ActionResultData, SalesConditionData, NegotiationMemoData, VehicleData, CustomerData, StateInfoData, DistrictInfoData, CityInfoData, LocationInfoData, ContactTimeslotData, FamilyInfomationData, HobbyData, EstimateData, MakerModelData, FollowUpActionData, FollowUpResultData, DlrCustomerMemoData, VehicleMakerModelData, DlrCstVclData, LastActionKeyData, ActionSeqData, CountryCode, GlOutProspectCstUrl, SalesActionSeqData, EstimateVclData, EstimateVclDataT)
                xmlData = OutPutProspectCustomer1(IcropDealerCode, IcropBranchCode, CLng(SalesData.SALES_ID), FirstSuccessFlg, SalesData, SalesTempData, RequestData, AttractData, SalesActData, FllwUpBoxSalesData, ActionMemoData, FirstActionData, StatusActionData, ReqSrcData1, ReqSrcData2, SelectedSeriesData, CompetitorSeriesData, ActionData, ActionResultData, SalesConditionData, NegotiationMemoData, VehicleData, CustomerData, StateInfoData, DistrictInfoData, CityInfoData, LocationInfoData, ContactTimeslotData, FamilyInfomationData, HobbyData, EstimateData, MakerModelData, FollowUpActionData, FollowUpResultData, DlrCustomerMemoData, VehicleMakerModelData, DlrCstVclData, LastActionKeyData, ActionSeqData, CountryCode, GlOutProspectCstUrl, SalesActionSeqData, EstimateVclData, EstimateVclDataT, SalesLocalData)
                '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end

                'In other than above case (First time success)																									
            Else
                GlErrStepInfo = "ProspectCustomer1_D1(FirstSuccessFlg=FirstTimeSuccess)"
                'Call estimate information retrieve process	
                'ISSUE-0008_20130217_by_takeda_Start
                'If (IC3802801TableAdapter.GetEstimateInfo(SalesId).Rows.Count > 0) Then
                '    EstimateData = IC3802801TableAdapter.GetEstimateInfo(SalesId).Rows(0)
                'End If
                '20140319 Fujita Upd Start
                'GlErrStepInfo = "GetReqSource1:"
                'GlErrStepInfo=ReqSrc1stCd)
                'Call request source(1st) retrieve process	        ※Use request source code gotten from request or attract
                If (SalesTempData.SOURCE_1_CD <> "") Then
                    GlErrStepInfo = "ProspectCustomer1_D1_1(Get Source Name1)"
                    ReqSrcData = IC3802801TableAdapter.GetReqSource1(CLng(SalesTempData.SOURCE_1_CD))
                    If ReqSrcData.Rows.Count <> 0 Then
                        ReqSrcData1 = CType(ReqSrcData.Rows(0), IC3802801ReqSource1Row)
                        GlErrStepInfo = "ProspectCustomer1_D1_1_1"
                    End If
                End If
                '20140319 Fujita Upd End
                '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
                If (SalesTempData.SOURCE_1_CD <> "") Then
                    If (IC3802801TableAdapter.GetReqSource2(CLng(SalesTempData.SOURCE_1_CD), CLng(SalesLocalData.SOURCE_2_CD)).Rows.Count > 0) Then
                        GlErrStepInfo = "ProspectCustomer1_D1_1_2(Get Source Name2)"
                        ReqSrcData2 = CType(IC3802801TableAdapter.GetReqSource2(CLng(SalesTempData.SOURCE_1_CD), CLng(SalesLocalData.SOURCE_2_CD)).Rows(0), IC3802801ReqSource2Row)
                        GlErrStepInfo = "ProspectCustomer1_D1_1_2"
                    End If
                End If
                '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end
                GlErrStepInfo = "ProspectCustomer1_D1_2(Get Estimate Info Data)"
                If (IC3802801TableAdapter.GetEstimateInfo(IcropDealerCode, IcropBranchCode, CStr(SalesId), CstStrDeleteFlgOff).Rows.Count > 0) Then
                    EstimateData = CType(IC3802801TableAdapter.GetEstimateInfo(IcropDealerCode, IcropBranchCode, CStr(SalesId), CstStrDeleteFlgOff).Rows(0), IC3802801EstimateInfoRow)
                    'GlErrStepInfo="EstimateData.DLRCD")
                    'GlErrStepInfo=EstimateData.DLRCD)
                    'GlErrStepInfo="EstimateData.STRCD")
                    'GlErrStepInfo=EstimateData.STRCD)
                    'GlErrStepInfo="EstimateData.FLLWUPBOX_SEQNO")
                    'GlErrStepInfo=EstimateData.FLLWUPBOX_SEQNO.ToString)
                    'GlErrStepInfo="EstimateData.CRCUSTID(check1)")
                    'GlErrStepInfo=EstimateData.CRCUSTID)
                    GlErrStepInfo = "ProspectCustomer1_D1_3"
                End If
                'ISSUE-0008_20130217_by_takeda_End

                ' Call Estimation vehicle retrieve process	
                '20140318 Fujita Add Start
                'If (IC3802801TableAdapter.GetEstimateVclInfo(Clng(EstimateData.ESTIMATEID)).Rows.Count > 0) Then
                GlErrStepInfo = "ProspectCustomer1_D1_4(Get Estimate Vehicle Info Data)"
                EstimateVclDataT = IC3802801TableAdapter.GetEstimateVclInfo(CLng(EstimateData.ESTIMATEID))
                If EstimateVclDataT.Rows.Count <> 0 Then
                    EstimateVclData = CType(EstimateVclDataT.Rows(0), IC3802801EstimateVclInfoRow)
                    'EstimateVclData = CType(IC3802801TableAdapter.GetEstimateVclInfo(Clng(EstimateData.ESTIMATEID)).Rows(0), IC3802801EstimateVclInfoRow)
                    '20140318 Fujita Add End
                End If

                GlErrStepInfo = "ProspectCustomer1_D1_5(Get Perfer Vehicle Data)"
                'Call preferred vehicle data retrieve process																								
                SelectedSeriesData = IC3802801TableAdapter.GetSelectedSeries(CStr(SalesId))

                GlErrStepInfo = "ProspectCustomer1_D2"
                'Repeat per number of preferred vehicle data information retrieved																								
                'For Each Row In SelectedSeriesData
                '    'If agreement activity ID of preferred vehicle data is set																							
                '    '(If agreement activity information exists)																							
                '    If (Row.SALESBKG_ACT_ID <> "0") Then
                '        GlErrStepInfo="ProspectCustomer1_D3")
                '        'Call activity information retrieve process									※Use activity ID gotten from request or attract
                '        If (IC3802801TableAdapter.GetAction(Row.SALESBKG_ACT_ID).Rows.Count > 0) Then
                '            FollowUpActionData = IC3802801TableAdapter.GetAction(Row.SALESBKG_ACT_ID).Rows(0)
                '        End If
                '    End If
                'Next
                GlErrStepInfo = "ProspectCustomer1_D4(Get Competitor Vehicle Data)"

                'Call competitor vehicle data retrieve process																								
                CompetitorSeriesData = IC3802801TableAdapter.GetCompetitorSeries(CStr(SalesId))

                'Repeat per number of competitor vehicle data information retrieved																								
                For Each Row In CompetitorSeriesData
                    'If sales ID of competitor vehicle data is set																							
                    '(If competitor vehicle exists)																							
                    If (Row.SALES_ID <> "0") Then
                        GlErrStepInfo = "ProspectCustomer1_D5(Get Maker & Model Data(Competitor))"
                        'Call maker model retrieve process (competitor vehicle information)	
                        If (IC3802801TableAdapter.GetMakerModel(Row.MODEL_CD).Rows.Count > 0) Then
                            MakerModelData.ImportRow(IC3802801TableAdapter.GetMakerModel(Row.MODEL_CD).Rows(0))
                        End If
                    End If
                Next
                GlErrStepInfo = "ProspectCustomer1_D6(Get FB Sales Condition Data)"
                'GlErrStepInfo="EstimateData.CRCUSTID(check2)"
                'GlErrStepInfo=EstimateData.CRCUSTID

                'Call sales condition retrieve process																								
                'ISSUE-0008_20130217_by_takeda_Start
                'SalesConditionData = IC3802801TableAdapter.GetSalesCondition(DlrCd, BranchCd, SalesId, EstimateData.CUSTID)
                SalesConditionData = IC3802801TableAdapter.GetSalesCondition(IcropDealerCode, IcropBranchCode, SalesId, EstimateData.CRCUSTID)
                'ISSUE-0008_20130217_by_takeda_End
                'If sales condition data cannot be retrieved																								
                If (SalesConditionData.Rows.Count = 0) Then
                    GlErrStepInfo = "ProspectCustomer1_D6_1"
                    'Edit key information																							
                    'ISSUE-0008_20130217_by_takeda_Start
                    'StrErrKey = DlrCd & "," & BranchCd & "," & SalesId & "," & ""
                    StrErrKey = IcropDealerCode & "," & IcropBranchCode & "," & SalesId & "," & EstimateData.CRCUSTID
                    'ISSUE-0008_20130217_by_takeda_End
                    'Ouput error log information																							
                    blnRtnCode = WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgNoData05, StrErrKey.ToString())
                    'Set return value (end process)																							
                    'Return xmlData
                End If
                GlErrStepInfo = "ProspectCustomer1_D7"

                'Set customer vehicle class (Owner)																								
                StrCstVclKbn = CstStrOwner

                'Call dealer customer vehicle																								
                'ISSUE-0008_20130217_by_takeda_Start
                'DlrCstVclData = IC3802801TableAdapter.GetDlrCstVcl(DlrCd, EstimateData.CUSTID, StrCstVclKbn)
                'takeda_update_start_20150109
                'intCustId = Integer.Parse(EstimateData.CRCUSTID.Trim())
                intCustId = Long.Parse(EstimateData.CRCUSTID.Trim())
                'takeda_update_end_20150109
                'GlErrStepInfo="(Str)EstimateData.CRCUSTID"
                'GlErrStepInfo=EstimateData.CRCUSTID
                'GlErrStepInfo="(Int)EstimateData.CRCUSTID"
                'GlErrStepInfo=CStr(intCustId)
                GlErrStepInfo = "ProspectCustomer1_D7_1(Get Dealer Customer Vehicle Data)"
                DlrCstVclData = IC3802801TableAdapter.GetDlrCstVcl(IcropDealerCode, intCustId, StrCstVclKbn, CstStrOwnerChanged)
                'ISSUE-0008_20130217_by_takeda_End
                'If dealer vehicle information cannot be retrieved																								
                If (DlrCstVclData.Rows.Count = 0) Then
                    'Edit key information																							
                    'ISSUE-0008_20130217_by_takeda_Start
                    'StrErrKey = DlrCd & "," & ""
                    StrErrKey = IcropDealerCode & "," & intCustId & "," & StrCstVclKbn
                    'ISSUE-0008_20130217_by_takeda_End
                    'Ouput error log information																							
                    blnRtnCode = WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgNoData06, StrErrKey.ToString())
                    'Set return value (end process)																							
                    'Return xmlData
                End If
                GlErrStepInfo = "ProspectCustomer1_D8"

                ' Repeat process by the number of retrieved dealer vehicle data information
                Dim VehicleRow As IC3802801DataSet.IC3802801VehicleRow
                Dim MakerRow As IC3802801DataSet.IC3802801MakerModelRow
                For Each Row In DlrCstVclData
                    'Call vehicle data retrieve process	
                    If (IC3802801TableAdapter.GetVehicle(IcropDealerCode, CLng(Row.VCL_ID)).Rows.Count > 0) Then
                        GlErrStepInfo = "ProspectCustomer1_D9(Get Vehicle Data)"
                        VehicleRow = CType(IC3802801TableAdapter.GetVehicle(IcropDealerCode, CLng(Row.VCL_ID)).Rows(0), IC3802801VehicleRow)
                        VehicleData.ImportRow(VehicleRow)
                        '20140317 Fujita Del Start
                        'End If
                        '20140317 Fujita Del End

                        GlErrStepInfo = "ProspectCustomer1_D10"
                        If (IC3802801TableAdapter.GetMakerModel(VehicleRow.MODEL_CD).Rows.Count > 0) Then
                            GlErrStepInfo = "ProspectCustomer1_D11(Get Maker & Model Data(Vehicle))"
                            MakerRow = CType(IC3802801TableAdapter.GetMakerModel(VehicleRow.MODEL_CD).Rows(0), IC3802801MakerModelRow)
                            VehicleMakerModelData.ImportRow(MakerRow)
                        End If
                        '20140317 Fujita ADD Start
                    End If
                    '20140317 Fujita ADD End
                Next
                GlErrStepInfo = "ProspectCustomer1_D12"

                'Call customer data retrieve process	
                'ISSUE-0008_20130217_by_takeda_Start
                'If (IC3802801TableAdapter.GetCustomer(DlrCd, EstimateData.CUSTID).Rows.Count > 0) Then
                '    CustomerData = IC3802801TableAdapter.GetCustomer(DlrCd, EstimateData.CUSTID).Rows(0)
                'End If

                'takeda_update_start_20150109
                'intCustId = Integer.Parse(EstimateData.CRCUSTID.Trim())
                intCustId = Long.Parse(EstimateData.CRCUSTID.Trim())
                'takeda_update_end_20150109
                If (IC3802801TableAdapter.GetCustomer(IcropDealerCode, intCustId).Rows.Count > 0) Then
                    GlErrStepInfo = "ProspectCustomer1_D12_1(Get Customer Data)"
                    CustomerData = CType(IC3802801TableAdapter.GetCustomer(IcropDealerCode, intCustId).Rows(0), IC3802801CustomerRow)
                    'takeda_update_start_20140425
                    'GlErrStepInfo = "@@@DataCheck(GetCustomer)"
                    'GlErrStepInfo = "(DB)CustomerData.CST_ADDRESS"
                    'GlErrStepInfo = CustomerData.CST_ADDRESS
                    'GlErrStepInfo = "(DB)CustomerData.CST_ADDRESS_1"
                    'GlErrStepInfo = CustomerData.CST_ADDRESS_1
                    'takeda_update_end_20140425
                End If
                'ISSUE-0008_20130217_by_takeda_End

                'If customer information cannot be retrieved																								
                If (CustomerData.CST_ID = "") Then
                    'Ouput error log information																							
                    'ISSUE-0008_20130217_by_takeda_Start
                    'blnRtnCode = WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgNoData07, EstimateData.CUSTID)
                    blnRtnCode = WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgNoData07, CStr(intCustId))
                    'Set return value (end process)																							
                    Return xmlData
                End If
                GlErrStepInfo = "ProspectCustomer1_D13"

                'If customer information exists																								
                If (CustomerData.CST_ID <> "0") Then
                    GlErrStepInfo = "ProspectCustomer1_D14(Get Customer Memo Data)"
                    ' Call dealer customer memo retrieve process
                    If (IC3802801TableAdapter.GetDlrCustomerMemo(IcropDealerCode, CLng(CustomerData.CST_ID)).Rows.Count > 0) Then
                        DlrCustomerMemoData = CType(IC3802801TableAdapter.GetDlrCustomerMemo(IcropDealerCode, CLng(CustomerData.CST_ID)).Rows(0), IC3802801CustomerMemoRow)
                    End If

                    GlErrStepInfo = "ProspectCustomer1_D15"
                    'ISSUE-0025_20130219_by_chatchai_Start
                    ''Call customer address data retrieve process
                    'If (IC3802801TableAdapter.GetCustomerAddress(CustomerData.CST_ADDRESS_STATE, CustomerData.CST_ADDRESS_DISTRICT, CustomerData.CST_ADDRESS_CITY, CustomerData.CST_ADDRESS_LOCATION).Rows.Count > 0) Then
                    '    CustomerAddressData = IC3802801TableAdapter.GetCustomerAddress(CustomerData.CST_ADDRESS_STATE, CustomerData.CST_ADDRESS_DISTRICT, CustomerData.CST_ADDRESS_CITY, CustomerData.CST_ADDRESS_LOCATION).Rows(0)
                    'End If

                    '20140317 ISSUE-IT2-1_by_takeda_start
                    StateInfoData.STATE_CD = ""
                    StateInfoData.STATE_NAME = ""
                    DistrictInfoData.DISTRICT_CD = ""
                    DistrictInfoData.DISTRICT_NAME = ""
                    CityInfoData.CITY_CD = ""
                    CityInfoData.CITY_NAME = ""
                    LocationInfoData.LOCATION_CD = ""
                    LocationInfoData.LOCATION_NAME = ""

                    If (String.IsNullOrEmpty(Trim(CustomerData.CST_ADDRESS_STATE))) Then
                        GlErrStepInfo = "ProspectCustomer1_D15_1(No State Code)"
                    Else
                        GlErrStepInfo = "ProspectCustomer1_D15_2(Get State Data)"
                        StateInfoData = IC3802801TableAdapter.GetStateInfo(CustomerData.CST_ADDRESS_STATE).Rows(0)

                        If (String.IsNullOrEmpty(Trim(CustomerData.CST_ADDRESS_DISTRICT))) Then
                            GlErrStepInfo = "ProspectCustomer1_D15_3(No District Code)"
                        Else
                            GlErrStepInfo = "ProspectCustomer1_D15_4(Get District Data)"

                            DistrictInfoData = IC3802801TableAdapter.GetDistrictInfo(CustomerData.CST_ADDRESS_STATE, CustomerData.CST_ADDRESS_DISTRICT).Rows(0)

                            If (String.IsNullOrEmpty(Trim(CustomerData.CST_ADDRESS_CITY))) Then
                                GlErrStepInfo = "ProspectCustomer1_D15_5(No City Code)"
                            Else
                                GlErrStepInfo = "ProspectCustomer1_D15_6(Get City Data)"

                                CityInfoData = IC3802801TableAdapter.GetCityInfo(CustomerData.CST_ADDRESS_STATE, CustomerData.CST_ADDRESS_DISTRICT, CustomerData.CST_ADDRESS_CITY).Rows(0)

                                If (String.IsNullOrEmpty(Trim(CustomerData.CST_ADDRESS_LOCATION))) Then
                                    GlErrStepInfo = "ProspectCustomer1_D15_7(No Location Code)"
                                Else
                                    GlErrStepInfo = "ProspectCustomer1_D15_8(Get Location Data)"
                                    LocationInfoData = IC3802801TableAdapter.GetLocationInfo(CustomerData.CST_ADDRESS_STATE, CustomerData.CST_ADDRESS_DISTRICT, CustomerData.CST_ADDRESS_CITY, CustomerData.CST_ADDRESS_LOCATION).Rows(0)
                                End If
                            End If
                        End If
                    End If
                    '    'Call State Information data retrieve process																							
                    '    If (IC3802801TableAdapter.GetStateInfo(CustomerData.CST_ADDRESS_STATE).Rows.Count > 0) Then
                    '        GlErrStepInfo="ProspectCustomer1_D15_1")
                    '        StateInfoData = CType(IC3802801TableAdapter.GetStateInfo(CustomerData.CST_ADDRESS_STATE).Rows(0), IC3802801StateInfoRow)
                    '    End If

                    '    If (StateInfoData.STATE_CD <> "") Then
                    '        'Call District Information data retrieve process
                    '        If (IC3802801TableAdapter.GetDistrictInfo(StateInfoData.STATE_CD, CustomerData.CST_ADDRESS_DISTRICT).Rows.Count > 0) Then
                    '            GlErrStepInfo="ProspectCustomer1_D15_2")
                    '            DistrictInfoData = CType(IC3802801TableAdapter.GetDistrictInfo(StateInfoData.STATE_CD, CustomerData.CST_ADDRESS_DISTRICT).Rows(0), IC3802801DistrictInfoRow)
                    '        End If

                    '        If (DistrictInfoData.DISTRICT_CD <> "") Then
                    '            'Call City Information data retrieve process	
                    '            If (IC3802801TableAdapter.GetCityInfo(StateInfoData.STATE_CD, DistrictInfoData.DISTRICT_CD, CustomerData.CST_ADDRESS_CITY).Rows.Count > 0) Then
                    '                GlErrStepInfo="ProspectCustomer1_D15_3")
                    '                CityInfoData = CType(IC3802801TableAdapter.GetCityInfo(StateInfoData.STATE_CD, DistrictInfoData.DISTRICT_CD, CustomerData.CST_ADDRESS_CITY).Rows(0), IC3802801CityInfoRow)
                    '            End If

                    '            If (CityInfoData.CITY_CD <> "") Then
                    '                GlErrStepInfo="ProspectCustomer1_D15_4")
                    '                'Call Location Information data retrieve process																				
                    '                LocationInfoData = CType(IC3802801TableAdapter.GetLocationInfo(StateInfoData.STATE_CD, DistrictInfoData.DISTRICT_CD, CityInfoData.CITY_CD, CustomerData.CST_ADDRESS_LOCATION).Rows(0), IC3802801LocationInfoRow)
                    '            End If
                    '        End If
                    '    End If
                    'End If
                    'ISSUE-0025_20130219_by_chatchai_End
                    '20140317 ISSUE-0025_by_chatchai_End

                End If
                GlErrStepInfo = "ProspectCustomer1_D16(Get Contact Timeslot)"

                'Call Customer contact timeslot retrieve process	
                If (IC3802801TableAdapter.GetContactTimeslot(CLng(CustomerData.CST_ID), CstStrIcropTimeslot).Rows.Count > 0) Then
                    ContactTimeslotData = CType(IC3802801TableAdapter.GetContactTimeslot(CLng(CustomerData.CST_ID), CstStrIcropTimeslot).Rows(0), IC3802801ContactTimeslotRow)
                End If

                GlErrStepInfo = "ProspectCustomer1_D17(Get Family Infomation Data)"
                'Call customer family data retrieve process		
                'ISSUE-0008_20130217_by_takeda_Start
                'FamilyInfomationData = IC3802801TableAdapter.GetFamilyInfomation(EstimateData.CUSTID)
                FamilyInfomationData = IC3802801TableAdapter.GetFamilyInfomation(EstimateData.CRCUSTID)
                'ISSUE-0008_20130217_by_takeda_End

                GlErrStepInfo = "ProspectCustomer1_D18(Get Hobby Data)"
                'Call customer hobby data retrieve process																								
                'ISSUE-0008_20130217_by_takeda_Start
                'HobbyData = IC3802801TableAdapter.GetHobby(EstimateData.CUSTID)
                HobbyData = IC3802801TableAdapter.GetHobby(EstimateData.CRCUSTID)
                'ISSUE-0008_20130217_by_takeda_End

                'takeda_update_start_20140529_ログ情報コメントアウト(一発Success時、FollowUpResultは未設定なので参照不可)
                ''takeda_update_start_20140328
                'GlErrStepInfo="(Check3)FollowUpResultData")
                'GlErrStepInfo="ACT_ID")
                'GlErrStepInfo=FollowUpResultData.ACT_ID)
                'GlErrStepInfo="ACT_STATUS")
                'GlErrStepInfo=FollowUpResultData.ACT_STATUS)
                'GlErrStepInfo="ACT_COUNT")
                'GlErrStepInfo=FollowUpResultData.ACT_COUNT)
                ''takeda_update_end_20140328
                'takeda_update_end_20140529_ログ情報コメントアウト(一発Success時、FollowUpResultは未設定なので参照不可)

                GlErrStepInfo = "ProspectCustomer1_D19(OutputProspectCustomer1FirstSuccess)"
                'Output linkage file																								
                '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
                'xmlData = OutputProspectCustomer1FirstSuccess(IcropDealerCode, IcropBranchCode, CStr(SalesId), FirstSuccessFlg, SalesTempData, RequestData, AttractData, EstimateData, SelectedSeriesData, CompetitorSeriesData, ActionData, SalesConditionData, VehicleData, NegotiationMemoData, CustomerData, StateInfoData, DistrictInfoData, CityInfoData, LocationInfoData, ContactTimeslotData, FamilyInfomationData, HobbyData, ReqSrcData1, ReqSrcData2, ActionResultData, MakerModelData, FollowUpActionData, FollowUpResultData, FllwUpBoxSalesData, SalesActData, DlrCustomerMemoData, ActionMemoData, VehicleMakerModelData, DlrCstVclData, LastActionKeyData, ActionSeqData, CountryCode, GlOutProspectCstUrl, SalesActionSeqData, EstimateVclData, EstimateVclDataT)
                xmlData = OutputProspectCustomer1FirstSuccess(IcropDealerCode, IcropBranchCode, CStr(SalesId), FirstSuccessFlg, SalesTempData, RequestData, AttractData, EstimateData, SelectedSeriesData, CompetitorSeriesData, ActionData, SalesConditionData, VehicleData, NegotiationMemoData, CustomerData, StateInfoData, DistrictInfoData, CityInfoData, LocationInfoData, ContactTimeslotData, FamilyInfomationData, HobbyData, ReqSrcData1, ReqSrcData2, ActionResultData, MakerModelData, FollowUpActionData, FollowUpResultData, FllwUpBoxSalesData, SalesActData, DlrCustomerMemoData, ActionMemoData, VehicleMakerModelData, DlrCstVclData, LastActionKeyData, ActionSeqData, CountryCode, GlOutProspectCstUrl, SalesActionSeqData, EstimateVclData, EstimateVclDataT, SalesLocalData)
                '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end

            End If
            GlErrStepInfo = "ProspectCustomer1_End"
            'Return Linkage data
            Return xmlData
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
    'Public Function OutPutProspectCustomer1(ByVal IcropDealerCode As String,
    '       ByVal IcropBranchCode As String,
    '       ByVal FollowUpNo As Long,
    '       ByVal FirstSuccessFlg As String,
    '       ByVal SalesData As IC3802801DataSet.IC3802801SalesRow,
    '       ByVal SalesTempData As IC3802801DataSet.IC3802801SalesTempRow,
    '       ByVal RequestData As IC3802801DataSet.IC3802801FollowUpRequestRow,
    '       ByVal AttractData As IC3802801DataSet.IC3802801FollowUpAttractRow,
    '       ByVal SalesActData As IC3802801DataSet.IC3802801SalesActionDataTable,
    '       ByVal FllwUpBoxSalesData As IC3802801DataSet.IC3802801GetFllwUpBoxSalesDataTable,
    '       ByVal ActionMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
    '       ByVal FirstActionData As IC3802801DataSet.IC3802801ActionDataTable,
    '       ByVal StatusActionData As IC3802801DataSet.IC3802801ActionRow,
    '       ByVal ReqSrcData1 As IC3802801DataSet.IC3802801ReqSource1Row,
    '       ByVal ReqSrcData2 As IC3802801DataSet.IC3802801ReqSource2Row,
    '       ByVal SelectedSeriesData As IC3802801DataSet.IC3802801SelectedSeriesDataTable,
    '       ByVal CompetitorSeriesData As IC3802801DataSet.IC3802801CompetitorSeriesDataTable,
    '       ByVal ActionData As IC3802801DataSet.IC3802801ActionDataTable,
    '       ByVal ActionResultData As IC3802801DataSet.IC3802801ActionResultRow,
    '       ByVal SalesConditionData As IC3802801DataSet.IC3802801SalesConditionDataTable,
    '       ByVal NegotiationMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
    '       ByVal VehicleData As IC3802801DataSet.IC3802801VehicleDataTable,
    '       ByVal CustomerData As IC3802801DataSet.IC3802801CustomerRow,
    '       ByVal StateInfoData As IC3802801DataSet.IC3802801StateInfoRow,
    '       ByVal DistrictInfoData As IC3802801DataSet.IC3802801DistrictInfoRow,
    '       ByVal CityInfoData As IC3802801DataSet.IC3802801CityInfoRow,
    '       ByVal LocationInfoData As IC3802801DataSet.IC3802801LocationInfoRow,
    '       ByVal ContactTimeslotData As IC3802801DataSet.IC3802801ContactTimeslotRow,
    '       ByVal FamilyInfomationData As IC3802801DataSet.IC3802801FamilyInfomationDataTable,
    '       ByVal HobbyData As IC3802801DataSet.IC3802801HobbyDataTable,
    '       ByVal EstimateData As IC3802801DataSet.IC3802801EstimateInfoRow,
    '       ByVal MakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
    '       ByVal FollowUpActionData As IC3802801DataSet.IC3802801ActionRow,
    '       ByVal FollowUpResultData As IC3802801DataSet.IC3802801ActionRow,
    '       ByVal DlrCustomerMemoData As IC3802801DataSet.IC3802801CustomerMemoRow,
    '       ByVal VehicleMakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
    '       ByVal DlrCstVclData As IC3802801DataSet.IC3802801DlrCstVclDataTable,
    '       ByVal LastActionKeyData As IC3802801DataSet.IC3802801ActionRow,
    '       ByVal ActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable,
    '       ByVal CountryCode As String, ByVal GlOutProspectCstUrl As String, ByVal SalesActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable, ByVal EstimateVclData As IC3802801DataSet.IC3802801EstimateVclInfoRow, ByVal EstimateVclDataT As IC3802801DataSet.IC3802801EstimateVclInfoDataTable) As XmlProspectCustomer
    Public Function OutPutProspectCustomer1(ByVal IcropDealerCode As String,
           ByVal IcropBranchCode As String,
           ByVal FollowUpNo As Long,
           ByVal FirstSuccessFlg As String,
           ByVal SalesData As IC3802801DataSet.IC3802801SalesRow,
           ByVal SalesTempData As IC3802801DataSet.IC3802801SalesTempRow,
           ByVal RequestData As IC3802801DataSet.IC3802801FollowUpRequestRow,
           ByVal AttractData As IC3802801DataSet.IC3802801FollowUpAttractRow,
           ByVal SalesActData As IC3802801DataSet.IC3802801SalesActionDataTable,
           ByVal FllwUpBoxSalesData As IC3802801DataSet.IC3802801GetFllwUpBoxSalesDataTable,
           ByVal ActionMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
           ByVal FirstActionData As IC3802801DataSet.IC3802801ActionDataTable,
           ByVal StatusActionData As IC3802801DataSet.IC3802801ActionRow,
           ByVal ReqSrcData1 As IC3802801DataSet.IC3802801ReqSource1Row,
           ByVal ReqSrcData2 As IC3802801DataSet.IC3802801ReqSource2Row,
           ByVal SelectedSeriesData As IC3802801DataSet.IC3802801SelectedSeriesDataTable,
           ByVal CompetitorSeriesData As IC3802801DataSet.IC3802801CompetitorSeriesDataTable,
           ByVal ActionData As IC3802801DataSet.IC3802801ActionDataTable,
           ByVal ActionResultData As IC3802801DataSet.IC3802801ActionResultRow,
           ByVal SalesConditionData As IC3802801DataSet.IC3802801SalesConditionDataTable,
           ByVal NegotiationMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
           ByVal VehicleData As IC3802801DataSet.IC3802801VehicleDataTable,
           ByVal CustomerData As IC3802801DataSet.IC3802801CustomerRow,
           ByVal StateInfoData As IC3802801DataSet.IC3802801StateInfoRow,
           ByVal DistrictInfoData As IC3802801DataSet.IC3802801DistrictInfoRow,
           ByVal CityInfoData As IC3802801DataSet.IC3802801CityInfoRow,
           ByVal LocationInfoData As IC3802801DataSet.IC3802801LocationInfoRow,
           ByVal ContactTimeslotData As IC3802801DataSet.IC3802801ContactTimeslotRow,
           ByVal FamilyInfomationData As IC3802801DataSet.IC3802801FamilyInfomationDataTable,
           ByVal HobbyData As IC3802801DataSet.IC3802801HobbyDataTable,
           ByVal EstimateData As IC3802801DataSet.IC3802801EstimateInfoRow,
           ByVal MakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
           ByVal FollowUpActionData As IC3802801DataSet.IC3802801ActionRow,
           ByVal FollowUpResultData As IC3802801DataSet.IC3802801ActionRow,
           ByVal DlrCustomerMemoData As IC3802801DataSet.IC3802801CustomerMemoRow,
           ByVal VehicleMakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
           ByVal DlrCstVclData As IC3802801DataSet.IC3802801DlrCstVclDataTable,
           ByVal LastActionKeyData As IC3802801DataSet.IC3802801ActionRow,
           ByVal ActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable,
           ByVal CountryCode As String, ByVal GlOutProspectCstUrl As String,
           ByVal SalesActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable,
           ByVal EstimateVclData As IC3802801DataSet.IC3802801EstimateVclInfoRow,
           ByVal EstimateVclDataT As IC3802801DataSet.IC3802801EstimateVclInfoDataTable,
           ByVal SalesLocalData As IC3802801DataSet.IC3802801SalesLocalRow) As XmlProspectCustomer
        '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end

        '20140317 Fujita Upd Start
        Try
            GlErrStepInfo = "OutPutProspectCustomer1_Start"
            'Linkage file output data sace definition																								
            Dim prospectCustomerXML As New XmlProspectCustomer()

            '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
            'prospectCustomerXML = SetXmlProspectCustomer(IcropDealerCode, IcropBranchCode, CLng(SalesData.SALES_ID), CountryCode, FirstSuccessFlg, SalesData, SalesTempData, RequestData, AttractData, VehicleData, CustomerData, SelectedSeriesData, CompetitorSeriesData, SalesConditionData, ReqSrcData1, ReqSrcData2, ActionData, ActionResultData, EstimateData, MakerModelData, FollowUpActionData, FollowUpResultData, NegotiationMemoData, FllwUpBoxSalesData, SalesActData, DlrCustomerMemoData, ActionMemoData, VehicleMakerModelData, DlrCstVclData, LastActionKeyData, ActionSeqData, ContactTimeslotData, StateInfoData, DistrictInfoData, CityInfoData, LocationInfoData, SalesActionSeqData, EstimateVclData, EstimateVclDataT)
            prospectCustomerXML = SetXmlProspectCustomer(IcropDealerCode, IcropBranchCode, CLng(SalesData.SALES_ID), CountryCode, FirstSuccessFlg, SalesData, SalesTempData, RequestData, AttractData, VehicleData, CustomerData, SelectedSeriesData, CompetitorSeriesData, SalesConditionData, ReqSrcData1, ReqSrcData2, ActionData, ActionResultData, EstimateData, MakerModelData, FollowUpActionData, FollowUpResultData, NegotiationMemoData, FllwUpBoxSalesData, SalesActData, DlrCustomerMemoData, ActionMemoData, VehicleMakerModelData, DlrCstVclData, LastActionKeyData, ActionSeqData, ContactTimeslotData, StateInfoData, DistrictInfoData, CityInfoData, LocationInfoData, SalesActionSeqData, EstimateVclData, EstimateVclDataT, SalesLocalData)
            '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end

            GlErrStepInfo = "OutPutProspectCustomer1_End"
            Return prospectCustomerXML
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Function

    '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
    'Public Function OutputProspectCustomer1FirstSuccess(ByVal IcropDealerCode As String,
    '         ByVal IcropBranchCode As String,
    '         ByVal FollowUpNo As String,
    '         ByVal FirstSuccessFlg As String,
    '         ByVal SalesTempData As IC3802801DataSet.IC3802801SalesTempRow,
    '         ByVal RequestData As IC3802801DataSet.IC3802801FollowUpRequestRow,
    '         ByVal AttractData As IC3802801DataSet.IC3802801FollowUpAttractRow,
    '         ByVal EstimateData As IC3802801DataSet.IC3802801EstimateInfoRow,
    '         ByVal SelectedSeriesData As IC3802801DataSet.IC3802801SelectedSeriesDataTable,
    '         ByVal CompetitorSeriesData As IC3802801DataSet.IC3802801CompetitorSeriesDataTable,
    '         ByVal ActionData As IC3802801DataSet.IC3802801ActionDataTable,
    '         ByVal SalesConditionData As IC3802801DataSet.IC3802801SalesConditionDataTable,
    '         ByVal VehicleData As IC3802801DataSet.IC3802801VehicleDataTable,
    '         ByVal NegotiationMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
    '         ByVal CustomerData As IC3802801DataSet.IC3802801CustomerRow,
    '         ByVal StateInfoData As IC3802801DataSet.IC3802801StateInfoRow,
    '         ByVal DistrictInfoData As IC3802801DataSet.IC3802801DistrictInfoRow,
    '         ByVal CityInfoData As IC3802801DataSet.IC3802801CityInfoRow,
    '         ByVal LocationInfoData As IC3802801DataSet.IC3802801LocationInfoRow,
    '         ByVal ContactTimeslotData As IC3802801DataSet.IC3802801ContactTimeslotRow,
    '         ByVal FamilyInfomationData As IC3802801DataSet.IC3802801FamilyInfomationDataTable,
    '         ByVal HobbyData As IC3802801DataSet.IC3802801HobbyDataTable,
    '         ByVal ReqSrcData1 As IC3802801DataSet.IC3802801ReqSource1Row,
    '         ByVal ReqSrcData2 As IC3802801DataSet.IC3802801ReqSource2Row,
    '         ByVal ActionResultData As IC3802801DataSet.IC3802801ActionResultRow,
    '         ByVal MakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
    '         ByVal FollowUpActionData As IC3802801DataSet.IC3802801ActionRow,
    '         ByVal FollowUpResultData As IC3802801DataSet.IC3802801ActionRow,
    '         ByVal FllwUpBoxSalesData As IC3802801DataSet.IC3802801GetFllwUpBoxSalesDataTable,
    '         ByVal SalesActData As IC3802801DataSet.IC3802801SalesActionDataTable,
    '         ByVal DlrCustomerMemoData As IC3802801DataSet.IC3802801CustomerMemoRow,
    '         ByVal ActionMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
    '         ByVal VehicleMakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
    '         ByVal DlrCstVclData As IC3802801DataSet.IC3802801DlrCstVclDataTable,
    '         ByVal LastActionKeyData As IC3802801DataSet.IC3802801ActionRow,
    '         ByVal ActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable,
    '         ByVal CountryCode As String, ByVal GlOutProspectCstUrl As String,
    '         ByVal SalesActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable, ByVal EstimateVclData As IC3802801DataSet.IC3802801EstimateVclInfoRow, ByVal EstimateVclDataT As IC3802801DataSet.IC3802801EstimateVclInfoDataTable) As XmlProspectCustomer
    Public Function OutputProspectCustomer1FirstSuccess(ByVal IcropDealerCode As String,
             ByVal IcropBranchCode As String,
             ByVal FollowUpNo As String,
             ByVal FirstSuccessFlg As String,
             ByVal SalesTempData As IC3802801DataSet.IC3802801SalesTempRow,
             ByVal RequestData As IC3802801DataSet.IC3802801FollowUpRequestRow,
             ByVal AttractData As IC3802801DataSet.IC3802801FollowUpAttractRow,
             ByVal EstimateData As IC3802801DataSet.IC3802801EstimateInfoRow,
             ByVal SelectedSeriesData As IC3802801DataSet.IC3802801SelectedSeriesDataTable,
             ByVal CompetitorSeriesData As IC3802801DataSet.IC3802801CompetitorSeriesDataTable,
             ByVal ActionData As IC3802801DataSet.IC3802801ActionDataTable,
             ByVal SalesConditionData As IC3802801DataSet.IC3802801SalesConditionDataTable,
             ByVal VehicleData As IC3802801DataSet.IC3802801VehicleDataTable,
             ByVal NegotiationMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
             ByVal CustomerData As IC3802801DataSet.IC3802801CustomerRow,
             ByVal StateInfoData As IC3802801DataSet.IC3802801StateInfoRow,
             ByVal DistrictInfoData As IC3802801DataSet.IC3802801DistrictInfoRow,
             ByVal CityInfoData As IC3802801DataSet.IC3802801CityInfoRow,
             ByVal LocationInfoData As IC3802801DataSet.IC3802801LocationInfoRow,
             ByVal ContactTimeslotData As IC3802801DataSet.IC3802801ContactTimeslotRow,
             ByVal FamilyInfomationData As IC3802801DataSet.IC3802801FamilyInfomationDataTable,
             ByVal HobbyData As IC3802801DataSet.IC3802801HobbyDataTable,
             ByVal ReqSrcData1 As IC3802801DataSet.IC3802801ReqSource1Row,
             ByVal ReqSrcData2 As IC3802801DataSet.IC3802801ReqSource2Row,
             ByVal ActionResultData As IC3802801DataSet.IC3802801ActionResultRow,
             ByVal MakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
             ByVal FollowUpActionData As IC3802801DataSet.IC3802801ActionRow,
             ByVal FollowUpResultData As IC3802801DataSet.IC3802801ActionRow,
             ByVal FllwUpBoxSalesData As IC3802801DataSet.IC3802801GetFllwUpBoxSalesDataTable,
             ByVal SalesActData As IC3802801DataSet.IC3802801SalesActionDataTable,
             ByVal DlrCustomerMemoData As IC3802801DataSet.IC3802801CustomerMemoRow,
             ByVal ActionMemoData As IC3802801DataSet.IC3802801ActionMemoDataTable,
             ByVal VehicleMakerModelData As IC3802801DataSet.IC3802801MakerModelDataTable,
             ByVal DlrCstVclData As IC3802801DataSet.IC3802801DlrCstVclDataTable,
             ByVal LastActionKeyData As IC3802801DataSet.IC3802801ActionRow,
             ByVal ActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable,
             ByVal CountryCode As String, ByVal GlOutProspectCstUrl As String,
             ByVal SalesActionSeqData As IC3802801DataSet.IC3802801ActionSeqDataTable,
             ByVal EstimateVclData As IC3802801DataSet.IC3802801EstimateVclInfoRow,
             ByVal EstimateVclDataT As IC3802801DataSet.IC3802801EstimateVclInfoDataTable,
             ByVal SalesLocalData As IC3802801DataSet.IC3802801SalesLocalRow) As XmlProspectCustomer
        '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end
        '20140317 Fujita Upd Start
        Try
            GlErrStepInfo = "OutputProspectCustomer1FirstSuccess_Start"
            Dim prospectCustomerXML As New XmlProspectCustomer()
            'takeda_update_start_20140529_ログ情報コメントアウト(一発Success時、FollowUpResultは未設定なので参照不可)
            ''takeda_update_start_20140328
            'GlErrStepInfo="(Check2)FollowUpResultData")
            'GlErrStepInfo="ACT_ID")
            'GlErrStepInfo=FollowUpResultData.ACT_ID)
            'GlErrStepInfo="ACT_STATUS")
            'GlErrStepInfo=FollowUpResultData.ACT_STATUS)
            'GlErrStepInfo="ACT_COUNT")
            'GlErrStepInfo=FollowUpResultData.ACT_COUNT)
            ''takeda_update_end_20140328
            'takeda_update_end_20140529_ログ情報コメントアウト(一発Success時、FollowUpResultは未設定なので参照不可)

            '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) start
            'prospectCustomerXML = SetXmlProspectCustomer(IcropDealerCode, IcropBranchCode, CLng(FollowUpNo), CountryCode, FirstSuccessFlg, Nothing, SalesTempData, RequestData, AttractData, VehicleData, CustomerData, SelectedSeriesData, CompetitorSeriesData, SalesConditionData, ReqSrcData1, ReqSrcData2, ActionData, ActionResultData, EstimateData, MakerModelData, FollowUpActionData, FollowUpResultData, NegotiationMemoData, FllwUpBoxSalesData, SalesActData, DlrCustomerMemoData, ActionMemoData, VehicleMakerModelData, DlrCstVclData, LastActionKeyData, ActionSeqData, ContactTimeslotData, StateInfoData, DistrictInfoData, CityInfoData, LocationInfoData, SalesActionSeqData, EstimateVclData, EstimateVclDataT)
            prospectCustomerXML = SetXmlProspectCustomer(IcropDealerCode, IcropBranchCode, CLng(FollowUpNo), CountryCode, FirstSuccessFlg, Nothing, SalesTempData, RequestData, AttractData, VehicleData, CustomerData, SelectedSeriesData, CompetitorSeriesData, SalesConditionData, ReqSrcData1, ReqSrcData2, ActionData, ActionResultData, EstimateData, MakerModelData, FollowUpActionData, FollowUpResultData, NegotiationMemoData, FllwUpBoxSalesData, SalesActData, DlrCustomerMemoData, ActionMemoData, VehicleMakerModelData, DlrCstVclData, LastActionKeyData, ActionSeqData, ContactTimeslotData, StateInfoData, DistrictInfoData, CityInfoData, LocationInfoData, SalesActionSeqData, EstimateVclData, EstimateVclDataT, SalesLocalData)
            '$27 TKM Change request development for Next Gen e-CRB (CR057,CR058,CR061) end

            GlErrStepInfo = "OutputProspectCustomer1FirstSuccess_End"
            Return prospectCustomerXML
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Function

    Public Function Main(ByVal prmSalesId As String) As Boolean

        '↓20140408 FJ.Hori LogType Moddify( Info -> Error ) Start
        Logger.Info("----------------------------------------------------")
        Logger.Info(" SA01_START(Main)    [Ver.SA01_GL_023]")
        Logger.Info("----------------------------------------------------")
        Logger.Info("Input Paramater(SalesID):" + prmSalesId)
        '↑20140408 FJ.Hori LogType Moddify( Info -> Error ) End
        '==========takeda_update_start_20140619_コメントアウト==========
        ' ''==========takeda_update_start_20140422_性能改善調査==========
        ''Const cstBaseSalesId As Decimal = 7000000000000000000
        ''Dim dtDateTime As DateTime
        ''Dim strDateTime As String = ""
        ''Dim dmDateTime As Decimal = 0
        ' ''現在時刻を求める
        ''dtDateTime = DateTime.Now
        ' ''区切り文字なし、24H形式に編集し、文字列項目に格納(ミリ秒は3桁表記)
        ''strDateTime = dtDateTime.ToString("yyyyMMddHHmmss") & dtDateTime.Millisecond.ToString("000")
        ''dmDateTime = Decimal.Parse(strDateTime)
        ''GlDmSalesId = dmDateTime + cstBaseSalesId

        ' ''開始時間登録
        ''IC3802801TableAdapter.InsertActionSeqDataAdapter2(GlDmSalesId, 1)
        ' ''==========takeda_update_end_20140422_性能改善調査============
        '==========takeda_update_end_20140619_コメントアウト==========

        'Return Code(Main)																						
        ''Dim blnRtnCd As Boolean = False
        Dim blnRtnCd As String = ""
        Dim xmlProspectCustomerData As New XmlProspectCustomer

        ''Retrieve country code from webconfig																						
        Dim CountryCode As String = EnvironmentSetting.CountryCode

        ''Retrieve staff context																						
        Dim staff As StaffContext = StaffContext.Current

        '20140317 Fujita Upd Start
        GlBooLog = False    'IT/ST
        'GlBooLog = True     '号口
        Try
            GlErrStepInfo = "Main_1(Call ProspectCustomer1 Start)"
            xmlProspectCustomerData = ProspectCustomer1(staff, prmSalesId, CountryCode)
            GlErrStepInfo = "Main_1(Call ProspectCustomer1 End)"
            Dim xmlData As New XmlDocument
            If (xmlProspectCustomerData.Head IsNot Nothing) Then
                GlErrStepInfo = "Main_2(Call SetXmlDocument Start)"
                xmlData = SetXmlDocument(xmlProspectCustomerData)
                GlErrStepInfo = "Main_2(Call SetXmlDocument End)"
            End If

            GlErrStepInfo = "Main_3"
            ' Return Value judgment																					
            If (xmlData.InnerText = "") Then
                GlErrStepInfo = "Main_4"
                ' If Linkage data cannot be retrieved, break process																				
                '(Linkage file send is not performed)	
                Logger.Info("blnRtnCd = " + blnRtnCd <> "")
                Logger.Info("----------------------------------------------------")
                Logger.Info(" SA01_END(Main)(Error)")
                Logger.Info("----------------------------------------------------")
                Return blnRtnCd <> ""
            End If

            ''ISSUE99_Start
            GlErrStepInfo = "Main_5(Call SetXmlDocument Start)"
            blnRtnCd = SendProspectCustomer1(xmlData.InnerXml)
            GlErrStepInfo = "Main_5(Call SetXmlDocument End)"
            GlOutResponseData = blnRtnCd
            GlErrStepInfo = "SendAfter_Check(ReturnCode)"
            'GlErrStepInfo=blnRtnCd)
            ''ISSUE99_End

            'Send linkage file
            'ISSUE-0006_20140215_by_takeda_start
            ''blnRtnCd = SendProspectCustomer1(xmlData.InnerXml)
            ''''blnRtnCd = SendProspectCustomer1(xmlData.InnerText)
            'ISSUE-0006_20140215_by_takeda_end

            ' Return Value judgment																					
            ''If (blnRtnCd = False) Then
            ' If linkage file is failed to sent, output the used XML in error log																				
            ''Logger.Error(xmlData.InnerText)
            'break process																				
            ''Return blnRtnCd
            ''Else
            ' If linkage file is successfully sent, output the used XML in log																				
            'Logger.Error("LINKAGE_XML_FROM_eCRB(SendData)"
            'ISSUE-0006_20140215_by_takeda_start
            ''GlErrStepInfo=xmlData.InnerText)
            'ISSUE-0006_20140215_by_takeda_end

            Logger.Info("LINKAGE_XML_FROM_eCRB(SendData)")
            Logger.Info(xmlData.InnerXml)
            Logger.Info("LINKAGE_XML_FROM_DMS(ResponseData)")
            Logger.Info(GlOutResponseData)

            Logger.Info("-- SA01_START(Main_ACardNo) -------------")
            blnRtnCd = Main_ACardNo()
            'GlErrStepInfo="ACardNo(Main_Return)"
            Logger.Info(blnRtnCd)
            Logger.Info("-- SA01_END(Main_ACardNo) -------------")

            'blnRtnCd = True
            ''End If
            Logger.Info("----------------------------------------------------")
            Logger.Info(" SA01_END(Main)")
            Logger.Info("----------------------------------------------------")
            '==========takeda_update_start_20140619_コメントアウト==========
            ' ''==========takeda_update_start_20140422_性能改善調査==========
            ' ''終了時間登録
            ''IC3802801TableAdapter.InsertActionSeqDataAdapter2(GlDmSalesId, 4)
            ' ''==========takeda_update_end_20140422_性能改善調査============
            '==========takeda_update_end_20140619_コメントアウト==========
            'return send Result																					
            Return blnRtnCd <> ""
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            Logger.Error("ERROR STEP INFO:" + GlErrStepInfo)
            Logger.Error("ERROR MESSAGE:" + ex.Message.ToString())
            Logger.Error("----------------------------------------------------")
            Logger.Error(" SA01_END(Main)(Error)")
            Logger.Error("----------------------------------------------------")
            'takeda_update_end_20140617
            Return False
        End Try
        '20140317 Fujita Upd End
    End Function

    Public Function Main_ACardNo() As String

        Try
            If GlACardNo = "" Then
                'Response interface read process & check process																					
                GlErrStepInfo = "Main_ACardNo_1(Call ReadResponse Start)"
                Dim interfaceRead As XmlResponse = ReadResponse()
                GlErrStepInfo = "Main_ACardNo_1(Call ReadResponse End)"
                'ISSUE_IT2-1_by_takda_update_start
                GlACardNo = interfaceRead.FollowUpInfo.FollowUpID
                'ISSUE_IT2-1_by_takda_update_end
            End If
            Return GlACardNo

        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            Logger.Error("ERROR STEP INFO:" + GlErrStepInfo)
            Logger.Error("ERROR MESSAGE:" + ex.Message.ToString())
            Logger.Error("----------------------------------------------------")
            Logger.Error(" SA01_END(Main_ACardNo)(Error)")
            Logger.Error("----------------------------------------------------")
            'takeda_update_end_20140617
            Return GlACardNo
        End Try

    End Function

    'Retrieve child node subordinate information																								
    Public Function GetChildNodeInfo(ByVal parentNode As XmlNode, ByVal childNodeName As String, ByVal assignMode As String, ByVal canMultiple As Boolean, ByVal errorCode As Long)
        GlErrStepInfo = "GetChildNodeInfo_Start"

        '20140317 Fujita Upd Start
        Try
            Dim XmlNodeListData As XmlNodeList
            Dim errorResultId As String

            'Child node existance check																							
            Dim childNodeCount = parentNode.SelectNodes(CstXmlRootDirectory + childNodeName).Count

            GlErrStepInfo = "GetChildNodeInfo_1"
            'If child node does not exist																							
            If (childNodeCount) = 0 Then
                GlErrStepInfo = "GetChildNodeInfo_1_1"
                'Check element status																						
                Select Case assignMode
                    'If is mandatory item																				
                    Case CstStrMandatory
                        'Error code create (mandatory item error)																			
                        errorResultId = CreateReturnId(ReturnCode.MandatoryItemError, errorCode)
                        'Call exception process																			
                        WriteErrorInfo("GetChildNodeInfo", CLng(errorResultId), CstStrErrMsgMandatory, "")
                        'If is optional item																				
                    Case CstStrOptional
                        'Set the result																				
                        XmlNodeListData = parentNode.SelectNodes(CstXmlRootDirectory + childNodeName)
                        'If is not set																				
                    Case CstStrNone
                        'Set the result																				
                        XmlNodeListData = parentNode.SelectNodes(CstXmlRootDirectory + childNodeName)
                End Select
                'If one child node exists																							
            ElseIf (childNodeCount) = 1 Then
                GlErrStepInfo = "GetChildNodeInfo_1_2"
                'Return the result																						
                XmlNodeListData = parentNode.SelectNodes(CstXmlRootDirectory + childNodeName)

                'If multiple child nodes exist																							
            Else
                GlErrStepInfo = "GetChildNodeInfo_1_3"
                'Check if it is OK to have multiple nodes																						
                If (canMultiple = True) Then
                    'Return the result																					
                    XmlNodeListData = parentNode.SelectNodes(CstXmlRootDirectory + childNodeName)

                Else
                    'If multiple nodes existance is not permitted																					
                    'Error code create (XML error)																					
                    errorResultId = CStr(ReturnCode.XmlIncorrect)
                    'Call exception process																					
                    WriteErrorInfo("GetChildNodeInfo", CLng(errorResultId), CstStrErrMsgXmlIncorrect, "")
                End If
            End If
            GlErrStepInfo = "GetChildNodeInfo_End"
            'Return child node information																							
            Return XmlNodeListData
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Function

    'Retrieve child node information																						
    Public Function GetChildNode(ByVal parentNode As XmlNode,
            ByVal childNodeName As String,
            ByVal assignMode As String,
            ByVal errorCode As String)
        GlErrStepInfo = "GetChildNode_Start"

        'Retrieve child node subordinate information																						
        Dim childNodeList As XmlNodeList = CType(GetChildNodeInfo(parentNode, childNodeName, assignMode, False, CLng(errorCode)), XmlNodeList)

        '20140317 Fujita Upd Start
        Try
            'Return first information of child node subordinate	
            If (childNodeList Is Nothing) Then
                GlErrStepInfo = "GetChildNode_IS Nothing"
                'takeda_update_start_20140328
                'Return vbNull
                Return Nothing
                'takeda_update_end_20140328
            End If

            GlErrStepInfo = "GetChildNode_Count"

            If (childNodeList.Count > 0) Then
                Return childNodeList.Item(0)
            Else
                GlErrStepInfo = "GetChildNode_IS No Count"
                'takeda_update_start_20140328
                'Return vbNull
                Return Nothing
                'takeda_update_end_20140328
            End If

            GlErrStepInfo = "GetChildNode_End"
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Function

    'Retrieve child node element (single item check)																			
    Public Function GetNodeInnerText(ByVal parentNode As XmlNode,
            ByVal childNodeName As String,
            ByVal assignMode As String,
            ByVal maxLength As Long,
            ByVal inputType As String,
            ByVal okData As String,
            ByVal errorCode As Long) As String
        GlErrStepInfo = "GetNodeInnerText_Start"
        Dim errorResultId As String
        'Retrieve child node element																			
        Dim childNode As XmlNode = CType(GetChildNode(parentNode, childNodeName, assignMode, CStr(errorCode)), XmlNode)

        '20140317 Fujita Upd Start
        Try
            If (childNode IsNot Nothing) Then
                'If child node exists, but mandatory content is not input																		
                If (childNode.InnerText = "") And (assignMode = CstStrMandatory) Then
                    'Error code create (mandatory item error)																	
                    errorResultId = CreateReturnId(ReturnCode.MandatoryItemError, errorCode)
                    'Call exception process																	
                    WriteErrorInfo("GetNodeInnerText", CLng(errorResultId), CstStrErrMsgMandatory, "")
                End If

                GlErrStepInfo = "GetNodeInnerText_1"

                'If child node content does not exist, return initial value depending on the input types																		
                If (childNode.InnerText = "") Then
                    'Input type check																	
                    Select Case inputType
                        Case CstStrTypeDate
                            Return Nothing             'In case of Date item, Nothing										
                        Case CstStrTypeNumber
                            Return Nothing             'In case of numeric value, 0										
                        Case CstStrTypeString
                            Return String.Empty                'In case of string, blank										
                        Case Else
                            Return Nothing             'In case not set, Nothing										
                    End Select
                End If

                GlErrStepInfo = "GetNodeInnerText_2"

                'Maximum length and type check																		
                Dim blnCheck As Boolean = IsCheckElement(childNode.InnerText, maxLength, inputType, errorCode)

                If blnCheck = False Then
                    'If check result is NG, return Nothing																	
                    Return Nothing
                End If

                'Value check																		
                blnCheck = IsCheckValue(childNode.InnerText, okData, CStr(errorCode))

                If blnCheck = True Then
                    'If check result is OK, return child node information																	
                    Return childNode.InnerText
                Else
                    'If check result is NG, return Nothing																	
                    Return Nothing
                End If
            End If
            GlErrStepInfo = "GetNodeInnerText_End"
            'takeda_update_start_20140328
            Return Nothing
            'takeda_update_end_20140328
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function


    'Maximum length check & type check																							
    Public Function IsCheckElement(ByVal target As String, ByVal maxLength As Long, ByVal inputType As String, ByVal errorCode As Long) As Boolean
        GlErrStepInfo = "IsCheckElement_Start"

        Dim errorResultId As String
        '20140317 Fujita Upd Start
        Try
            'Maximum length check																							
            If Validation.IsCorrectDigit(target, maxLength) Then
                'If length check is met, continue the following the check																						

                'Type check initialization																						
                Dim blnCheck As Boolean = False

                'Input type check																						
                Select Case inputType
                    Case CstStrNone
                        blnCheck = True                     'In case is not set, True															
                    Case CstStrTypeNumber
                        blnCheck = True                     'In case is string, True															
                    Case CstStrTypeString
                        blnCheck = Decimal.TryParse(target, 0)                                  'In case is numeric, perform numeric conversion process												
                        'If numeric conversion is done, True	
                    Case CstStrTypeHalfChar
                        blnCheck = True
                    Case CstStrTypeDate                  'In case is Date															
                        Try
                            If Len(target) = 10 Then                        'If the length is 19 digits, perform conversion (yyyy/mm/dd)														
                                DateTimeFunc.FormatString("dd/MM/yyyy", target)
                            Else                        'In case other than above, perform conversion (yyyy/mm/dd hh:mm:ss)														
                                'DateTimeFunc.FormatString("yyyy/mm/dd hh:mm:ss", target)
                                DateTimeFunc.FormatString("dd/MM/yyyy HH:mm:ss", target)
                            End If
                            blnCheck = True
                        Catch ex As FormatException
                        End Try
                End Select

                GlErrStepInfo = "IsCheckElement_1"

                If (blnCheck = True) Then
                    Return blnCheck
                Else
                    'Error code create (type conversion error)																					
                    errorResultId = CreateReturnId(ReturnCode.ItemTypeError, errorCode)
                    'Call exception process																					
                    WriteErrorInfo("IsCheckElement", CLng(errorResultId), CstStrErrMsgItemType, target)
                End If
            Else
                'Error code create (length error)																						
                errorResultId = CreateReturnId(ReturnCode.ItemSizeError, errorCode)
                'Call exception process																						
                WriteErrorInfo("IsCheckElement", CLng(errorResultId), CstStrErrMsgItemSize, target)

            End If
            GlErrStepInfo = "IsCheckElement_End"

        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Function

    'Value check																								
    Public Function IsCheckValue(ByVal checkColumn As String, ByVal checkOkData As String, ByVal errorCode As String) As Boolean
        GlErrStepInfo = "IsCheckValue_Start"
        '20140317 Fujita Upd Start
        Try

            Dim blnRtnCd As Boolean
            Dim errorResultId As String
            'If check subject item is not input, following check is not performed (set regular return value)																							
            If (String.IsNullOrEmpty(checkColumn) = True) Then
                'If is not input, set regular return value																						
                Return True
            End If

            GlErrStepInfo = "IsCheckValue_1"

            'Input permitted value is not input, following check is not performed (set regular return value)																					
            If (checkOkData = "") Then
                'If is not input, set regular return value																				
                Return True
            End If


            'Return value variable initialization (set false)																							
            blnRtnCd = False


            Dim checkParameter() As String

            'Store input permitted values to array (store comma edited data by dividing)																							
            checkParameter = Split(checkOkData)

            'Repeat check process by the number of arrays of input permitted value																							
            'for (i=0; checkParameter[].length; i++)	
            For i As Long = 0 To checkParameter.Length - 1
                'Check subject item mathes with input permitted value or not																						
                If (checkColumn = checkParameter(i)) Then
                    'If input permitted value is set, set regular return value																					
                    blnRtnCd = True
                End If
            Next

            GlErrStepInfo = "IsCheckValue_2"

            'If input permitted data is not included (leave as false)																							
            If blnRtnCd = False Then
                'Error code create (value error)																						
                errorResultId = CreateReturnId(ReturnCode.ValueError, CLng(errorCode))
                'Call exception process																						
                WriteErrorInfo("IsCheckValue", CLng(errorResultId), CstStrErrMsgValue, checkColumn)
            End If

            GlErrStepInfo = "IsCheckValue_End"
            'Set return value after input permitted value check																							
            Return blnRtnCd
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Function

    'Error code create process																					
    Public Function CreateReturnId(ByVal errorCode As Long, ByVal elementCode As Long) As String
        '20140317 Fujita Upd Start
        Try
            GlErrStepInfo = "CreateReturnId_Start"

            GlErrStepInfo = "CreateReturnId_End"
            'Create error code (error code + item number). Then, perform string conversion and return																					
            Return CStr(errorCode + elementCode)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Function

    ''ISSUE99_Start
    ''Public Function SendProspectCustomer1(ByVal SendData As String) As Boolean
    ''    Try
    ''        Dim request As WebRequest = WebRequest.Create(GlSendProspectCstUrl)
    ''        ' Set the Method property of the request to POST.
    ''        request.Method = "POST"
    ''        ' Create POST data and convert it to a byte array.
    ''        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(SendData)
    ''        ' Set the ContentType property of the WebRequest.
    ''        request.ContentType = "application/x-www-form-urlencoded"
    ''        ' Set the ContentLength property of the WebRequest.
    ''        request.ContentLength = byteArray.Length
    ''        ' Get the request stream.
    ''        Dim dataStream As Stream = request.GetRequestStream()
    ''        ' Write the data to the request stream.
    ''        dataStream.Write(byteArray, 0, byteArray.Length)
    ''        ' Close the Stream object.
    ''        dataStream.Close()
    ''        Return True
    ''    Catch ex As WebException
    ''        WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgHttpConnect, "")
    ''        Return False
    ''    Catch ex As Exception
    ''        Logger.Error(ex.Message, ex)
    ''        Return False
    ''    End Try
    ''End Function

    Public Function SendProspectCustomer1(ByVal postData As String) As String
        GlErrStepInfo = "SendProspectCustomer1_Start"
        GlErrStepInfo = "postData_check(NO_ENCODE):" + postData.ToString()
        GlErrStepInfo = "Request_Url:" + GlSendProspectCstUrl.ToString()

        Try
            '文字コードを指定する
            Dim enc As System.Text.Encoding = _
                System.Text.Encoding.GetEncoding("UTF-8")

            'バイト型配列に変換
            Dim postDataBytes As Byte() = _
                System.Text.Encoding.ASCII.GetBytes("xsData=" + HttpUtility.UrlEncode(postData))
            'Dim postDataBytes As Byte() = _
            '    System.Text.Encoding.ASCII.GetBytes("xsData=" + WebUtility.HtmlEncode(postData))
            'Dim postDataBytes As Byte() = _
            '   System.Text.Encoding.ASCII.GetBytes("xsData=" + "testparam")
            GlErrStepInfo = "Encode_end"

            Dim returnString As String = ""

            If GlNoDMSFlg = "1" Then
                GlErrStepInfo = "SendProspectCustomer1_1"
                '==========takeda_update_start_20140619_コメントアウト==========
                ' ''==========takeda_update_start_20140422_性能改善調査==========
                ' ''終了時間登録
                ''IC3802801TableAdapter.InsertActionSeqDataAdapter2(GlDmSalesId, 2)
                ' ''==========takeda_update_end_20140422_性能改善調査============
                '==========takeda_update_end_20140619_コメントアウト==========

                returnString = GlTestResponseXML

                '==========takeda_update_start_20140619_コメントアウト==========
                ' ''==========takeda_update_start_20140422_性能改善調査==========
                ' ''終了時間登録
                ''IC3802801TableAdapter.InsertActionSeqDataAdapter2(GlDmSalesId, 3)
                ' ''==========takeda_update_end_20140422_性能改善調査============
                '==========takeda_update_end_20140619_コメントアウト==========

            Else
                GlErrStepInfo = "SendProspectCustomer1_2"
                'WebRequestの作成
                Dim req As System.Net.WebRequest = _
                    System.Net.WebRequest.Create(GlSendProspectCstUrl)
                GlErrStepInfo = "Request_Create"
                'メソッドにPOSTを指定
                req.Method = "POST"
                'ContentTypeを"application/x-www-form-urlencoded"にする
                req.ContentType = "application/x-www-form-urlencoded"
                'POST送信するデータの長さを指定
                req.ContentLength = postDataBytes.Length

                GlErrStepInfo = "PostData_Send_Before"
                'データをPOST送信するためのStreamを取得
                Dim reqStream As System.IO.Stream = req.GetRequestStream()
                GlErrStepInfo = "PostData_Sent(GetRequest)"
                '送信するデータを書き込む
                reqStream.Write(postDataBytes, 0, postDataBytes.Length)
                GlErrStepInfo = "PostData_Write:" + postDataBytes.ToString()
                reqStream.Close()

                '==========takeda_update_start_20140619_コメントアウト==========
                ' ''==========takeda_update_start_20140422_性能改善調査==========
                ' ''終了時間登録
                ''IC3802801TableAdapter.InsertActionSeqDataAdapter2(GlDmSalesId, 2)
                ' ''==========takeda_update_end_20140422_性能改善調査============
                '==========takeda_update_end_20140619_コメントアウト==========

                'サーバーからの応答を受信するためのWebResponseを取得
                Dim res As System.Net.WebResponse = req.GetResponse()

                '==========takeda_update_start_20140619_コメントアウト==========
                ' ''==========takeda_update_start_20140422_性能改善調査==========
                ' ''終了時間登録
                ''IC3802801TableAdapter.InsertActionSeqDataAdapter2(GlDmSalesId, 3)
                ' ''==========takeda_update_end_20140422_性能改善調査============
                '==========takeda_update_end_20140619_コメントアウト==========
                GlErrStepInfo = "ResponseData_Received(GetResponse)"

                '応答データを受信するためのStreamを取得
                Dim resStream As System.IO.Stream = res.GetResponseStream()
                '受信して表示
                Dim sr As New System.IO.StreamReader(resStream, enc)

                '返却文字列を取得
                returnString = sr.ReadToEnd()
                GlErrStepInfo = "ResponseData_Check:" + returnString

                '閉じる
                sr.Close()
            End If

            GlErrStepInfo = "SendProspectCustomer1_End"
            Return returnString

        Catch ex As WebException
            WriteErrorInfo("ProspectCustomer1", ReturnCode.SystemError, CstStrErrMsgHttpConnect, "")
            Return False
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Logger.Error(ex.Message, ex)
            Return False
        End Try
        GlErrStepInfo = "SendProspectCustomer1_End"
    End Function
    ''ISSUE99_End

    Public Function ChangeDlrCd(ByVal DlrCd As String, ByVal ChangeMode As String) As String
        GlErrStepInfo = "changeDlrCd_Start"
        '20140317 Fujita Upd Start
        Try
            Dim changedDlrCd As String = ""
            Dim changedDlrCdRow As IC3802801DataSet.IC3802801DmsCodeMapRow
            'Iif Change mode＝（i-CROP　→　DMS)																								
            If (ChangeMode = CstStrModeDMS) Then
                '(Retrieve DMS dealer code)	
                If (IC3802801TableAdapter.GetDmsCd1(CstStrCdType1, DlrCd).Rows.Count > 0) Then
                    changedDlrCdRow = CType(IC3802801TableAdapter.GetDmsCd1(CstStrCdType1, DlrCd).Rows(0), IC3802801DmsCodeMapRow)
                    changedDlrCd = changedDlrCdRow.DMS_CD_1
                    '20140317 Fujita Add Start
                Else
                    GlErrStepInfo = "Change DlrCd：" + DlrCd + " Not Found"
                    Throw New Exception("Change DlrCd：" + DlrCd + " Not Found")
                    '20140317 Fujita Add End
                End If
                'In other than above case																								
                '(If change mode＝（DMS　→　i-CROP))																								
            Else
                '(Retrieve i-CROP dealer code)	
                If (IC3802801TableAdapter.GetIcropCd1(CstStrCdType1, DlrCd).Rows.Count > 0) Then
                    changedDlrCdRow = CType(IC3802801TableAdapter.GetIcropCd1(CstStrCdType1, DlrCd).Rows(0), IC3802801DmsCodeMapRow)
                    changedDlrCd = changedDlrCdRow.ICROP_CD_1
                    '20140317 Fujita Add Start
                Else
                    GlErrStepInfo = "Change DlrCd：" + DlrCd + " Not Found"
                    Throw New Exception("Change DlrCd：" + DlrCd + " Not Found")
                    '20140317 Fujita Add End
                End If
            End If

            GlErrStepInfo = "changeDlrCd_End"
            Return changedDlrCd
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End


    End Function

    ' Action code change process																				
    Public Function ChangeActCd(ByVal ActCd As String, ByVal ActType As String, ByVal ChangeMode As String) As IC3802801DataSet.IC3802801DmsCodeMapDataTable
        GlErrStepInfo = "changeActCd_Start"
        '20140317 Fujita Upd Start
        Try
            Dim changedActCd As New IC3802801DataSet.IC3802801DmsCodeMapDataTable
            ' In case Change mode ＝（i-CROP　→　DMS)																					
            If (ChangeMode = CstStrModeDMS) Then
                '(Retrieve DMS action code)																				
                changedActCd = IC3802801TableAdapter.GetDmsCd3(CstStrCdType11, ActCd, ActType)
                ' In other than above case																					
                '( In case change mode＝（DMS　→　i-CROP))																					
            Else
                '(Retrieve i-CROP action code)																				
                changedActCd = IC3802801TableAdapter.GetIcropCd3(CstStrCdType11, ActCd, ActType)
            End If

            GlErrStepInfo = "changeActCd_End"
            Return changedActCd
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Function

    <EnableCommit()>
    Public Function InsertActionSeq(ByVal table As IC3802801DataSet.IC3802801ActionSeqRow, ByVal UserName As String) As Long
        GlErrStepInfo = "InsertActionSeq_Start"
        '20140317 Fujita Upd Start
        Try

            Dim dataTable As New IC3802801DataSet.IC3802801ActionSeqDataTable
            Dim dataRow As IC3802801DataSet.IC3802801ActionSeqRow = CType(dataTable.NewRow(), IC3802801ActionSeqRow)

            ' Store the retrieved result in DataRow of activity SEQ manager																								
            With dataRow
                .SALES_ID = table.SALES_ID
                .RELATION_ACT_SEQ = table.RELATION_ACT_SEQ
                .RELATION_ACT_TYPE = table.RELATION_ACT_TYPE
                .RELATION_ACT_ID = table.RELATION_ACT_ID
                .ROW_CREATE_DATETIME = CStr(Date.Now)
                .ROW_CREATE_ACCOUNT = UserName
                .ROW_CREATE_FUNCTION = CstStrPGID
                .ROW_UPDATE_DATETIME = CStr(Date.Now)
                .ROW_UPDATE_ACCOUNT = UserName
                .ROW_UPDATE_FUNCTION = CstStrPGID
                .ROW_LOCK_VERSION = 1
            End With

            Dim insertCount As Long
            'Call data adapter method																							
            GlErrStepInfo = "InsertActionSeq_1(Insert Action Seq Manager)"
            insertCount = IC3802801TableAdapter.InsertActionSeqDataAdapter(dataRow)
            GlErrStepInfo = "InsertActionSeq_End"
            Return insertCount
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Function

    Public Function CheckActionSeq(ByVal SalesId As Long, ByVal RelationActType As String, ByVal RelationActId As Long, ByVal UserName As String) As IC3802801ActionSeqRow
        GlErrStepInfo = "checkActionSeq_Start"
        '20140317 Fujita Upd Start
        Try
            Dim intRtnCd As Long

            'Retrieve linkage activity seq of activity SEQ manager, using input parameter
            If (IC3802801TableAdapter.GetActionSeq(SalesId, RelationActType, RelationActId).Rows.Count > 0) Then
                GlErrStepInfo = "checkActionSeq_1(Get Action Seq Manager Data)"
                CheckActionSeq = CType(IC3802801TableAdapter.GetActionSeq(SalesId, RelationActType, RelationActId).Rows(0), IC3802801ActionSeqRow)
            Else
                GlErrStepInfo = "checkActionSeq_2(No Action Seq Manager Data)(New)"
                Dim EmptyActionSeq As New IC3802801DataSet.IC3802801ActionSeqDataTable
                CheckActionSeq = CType(EmptyActionSeq.NewRow(), IC3802801ActionSeqRow)
                CheckActionSeq.RELATION_ACT_SEQ = "0"
            End If


            ' In case linkage activity seq of activity SEQ manager cannot be retrieved																								
            If (CheckActionSeq.RELATION_ACT_SEQ = "0") Then
                ' Retrieve [linkage activity seq biggest value (newest) + 1] from activity SEQ manager
                If (IC3802801TableAdapter.GetLastActionSeq(SalesId).Rows.Count > 0) Then
                    GlErrStepInfo = "checkActionSeq_3(Get Action Seq Manager Data)(Lastest)"
                    CheckActionSeq = CType(IC3802801TableAdapter.GetLastActionSeq(SalesId).Rows(0), IC3802801ActionSeqRow)
                End If
                CheckActionSeq.SALES_ID = CStr(SalesId)
                CheckActionSeq.RELATION_ACT_TYPE = RelationActType
                CheckActionSeq.RELATION_ACT_ID = CStr(RelationActId)

                ' Call activity SEQ manager registration process																							
                GlErrStepInfo = "checkActionSeq_4(Insert Action Seq Manager)"
                intRtnCd = InsertActionSeq(CheckActionSeq, UserName)

            End If

            GlErrStepInfo = "checkActionSeq_End"
            Return CheckActionSeq
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Function

    'Edit length data process																						
    Public Function ChangeBranchCd(ByVal DlrCd As String, ByVal BranchCd As String, ByVal ChangeMode As String) As String
        GlErrStepInfo = "ChangeBranchCd_Start"
        '20140317 Fujita Upd Start
        Try
            Dim changedBranchCd As String = ""
            Dim changedBranchCdRow As IC3802801DataSet.IC3802801DmsCodeMapRow
            'Iif Change mode＝（i-CROP　→　DMS)																						
            If (ChangeMode = CstStrModeDMS) Then
                '(Retrieve DMS branch code)		
                If (IC3802801TableAdapter.GetDmsCd2(CstStrCdType2, DlrCd, BranchCd).Rows.Count > 0) Then
                    changedBranchCdRow = CType(IC3802801TableAdapter.GetDmsCd2(CstStrCdType2, DlrCd, BranchCd).Rows(0), IC3802801DmsCodeMapRow)
                    '$25 他システム連携における複数店舗コード変換対応 start 
                    changedBranchCd = changedBranchCdRow(GlDmsCodeMapUseColumn).ToString
                    '$25 他システム連携における複数店舗コード変換対応 end 
                    '20140317 Fujita Add Start
                Else
                    GlErrStepInfo = "i-CROP　→　DMS Change BranchCd：" + BranchCd + " Not Found"
                    Throw New Exception("i-CROP　→　DMS Change BranchCd：" + BranchCd + " Not Found")
                    '20140317 Fujita Add End
                End If
                'In other than above case																						
                '(If change mode＝（DMS　→　i-CROP))																						
            Else
                '(Retrieve i-CROP branch code)
                If (IC3802801TableAdapter.GetIcropCd2(CstStrCdType2, DlrCd, BranchCd).Rows.Count > 0) Then
                    changedBranchCdRow = CType(IC3802801TableAdapter.GetIcropCd2(CstStrCdType2, DlrCd, BranchCd).Rows(0), IC3802801DmsCodeMapRow)
                    changedBranchCd = changedBranchCdRow.ICROP_CD_2
                    '20140317 Fujita Add Start
                Else
                    GlErrStepInfo = "DMS　→　i-CROP Change BranchCd：" + BranchCd + " Not Found"
                    Throw New Exception("DMS　→　i-CROP Change BranchCd：" + BranchCd + " Not Found")
                    '20140317 Fujita Add End
                End If
                GlErrStepInfo = "ChangeBranchCd_End"
            End If
            Return changedBranchCd
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End


    End Function

    'Length edit process																						
    Public Function EditLength(ByVal TargetData As String,
           ByVal StartPos As Long,
           ByVal TargetLength As Long,
           ByVal EditType As String) As String
        GlErrStepInfo = "editLength_Start"
        '20140317 Fujita Upd Start
        Try

            'Record length check																					
            If (Len(TargetData) = 0) Then
                'If record length is 0, return the data before convertion and end process																				
                EditLength = TargetData
                Return EditLength

            End If

            'Edit method = If is byte specified																					
            If (EditType = CstStrTypeString) Then
                'Retrieve the specified character from the convertion subject data																				
                EditLength = Mid(TargetData, StartPos, TargetLength)
                Return EditLength
            Else
                'Retrieve specified character from the convertion subject data																				
                EditLength = Mid(TargetData, StartPos, TargetLength)
                Return EditLength
            End If
            GlErrStepInfo = "editLength_End"
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Function

    'Date data edit process																						
    Public Function EditDateFormat(ByVal TargetData As Date) As String
        GlErrStepInfo = "editDateFormat_Start"
        '20140317 Fujita Upd Start
        Try

            'Dim strFormatDate As String
            'Dim StrYYYY As String
            'Dim StrMM As String
            'Dim StrDD As String
            'Dim StrHHMMSS As String

            Try
                'Convert to string																				
                'strFormatDate = TargetData.ToString("YYYY/MM/DD HH:MM:SS")

                'Divide year, month, day and retrieve																				
                'StrYYYY = Mid(strFormatDate, 1, 4)
                'StrMM = Mid(strFormatDate, 6, 2)
                'StrDD = Mid(strFormatDate, 9, 2)
                'StrHHMMSS = Mid(strFormatDate, 12)

                'Edit to format [DD/MM/YYYY HH:MM:SS]																				
                EditDateFormat = TargetData.ToString("dd") & "/" & TargetData.ToString("MM") & "/" & TargetData.ToString("yyyy") & " " & TargetData.ToString("HH") & ":" & TargetData.ToString("mm") & ":" & TargetData.ToString("ss")
                Return EditDateFormat

            Catch ex As System.Exception
                'Return blank, in case of exception																				
                EditDateFormat = ""
                Return EditDateFormat

            End Try
            GlErrStepInfo = "editDateFormat_End"
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Function

    'Error control process																						
    Public Function WriteErrorInfo(ByVal methodNm As String,
           ByVal errCd As Long,
           ByVal errMsg As String,
           ByVal errRec As String) As Boolean
        GlErrStepInfo = "WriteErrorInfo_Start"
        'Variable declaration																					
        Dim strErrInfo As String
        Dim strNowDate As String

        '20140317 Fujita Upd Start
        Try
            'Retrieve current date																					
            strNowDate = DateTime.Now().ToString("yyyy") & "/" & DateTime.Now().ToString("MM") & "/" & DateTime.Now().ToString("dd") & " " & DateTime.Now().ToString("HH") & ":" & DateTime.Now().ToString("mm") & ":" & DateTime.Now().ToString("ss")

            'Edit log information																					
            '(current date + occurred method name + error record + error message + error record)																					
            strErrInfo = strNowDate & " " & methodNm & " " & ":" & errCd & " " & errMsg & " " & errRec
            '(※current date got from platform component) 

            'Error log output																					
            Logger.Info(strErrInfo)

            GlErrStepInfo = "WriteErrorInfo_End"
            'Set return value																					
            Return True
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Function
    'Reponse interface read process																				
    Public Function ReadResponse() As XmlResponse

        '20140317 Fujita Upd Start
        Try
            GlErrStepInfo = "ReadResponse_Start"
            ' Linkage file reading data space definition																			
            Dim responseXML As New XmlResponse()

            Dim xmlHeadResponse As New XmlHeadResponse()
            Dim xmlCommonResponse As New XmlCommonResponse()
            Dim xmlFollowUpInfo As New XmlFollowUpInfo()
            responseXML.HeadResponse = xmlHeadResponse
            responseXML.CommonResponse = xmlCommonResponse
            responseXML.FollowUpInfo = xmlFollowUpInfo

            GlErrStepInfo = "ReadResponse_1(Get XML Data)"
            'XML file analysis																			
            'GlErrStepInfo="GlOutReturnIFUrl")
            'GlErrStepInfo=GlOutReturnIFUrl)
            'ISSUE99_Start
            'GetXMLData(GlOutReturnIFUrl, responseXML)
            'GlErrStepInfo="GlOutResponseData(DMS_Response)")
            'GlErrStepInfo=GlOutResponseData)
            GetXMLData(GlOutResponseData, responseXML)
            'ISSUE99_End

            GlErrStepInfo = "ReadResponse_End"
            'Return reading result																			
            Return responseXML
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Function

    'XML file analyisis process																				
    Public Sub GetXMLData(ByVal XmlId As String, ByVal responseXML As XmlResponse)

        '20140317 Fujita Upd Start
        Try
            GlErrStepInfo = "GetXMLData_Start"
            GlErrStepInfo = "XmlId:" + XmlId.ToString()
            Dim responseXmlDocument As New XmlDocument()
            'Load XML file corresponding with the parameter XMLID	
            'ISSUE-IT2-1_by_takeda_start
            responseXmlDocument.LoadXml(XmlId)
            'responseXmlDocument.Load(XmlId)
            'ISSUE-IT2-1_by_takeda_end

            GlErrStepInfo = "GetXMLData_1"
            'Retrieve element (Node) under response																			
            Dim responseNode As XmlNode = CType(GetChildNode(responseXmlDocument, CstXmlResponse, CstStrMandatory, CStr(NodeName.Response)), XmlNode)

            GlErrStepInfo = "GetXMLData_2"
            'Head element retrieve & check	
            GetHeadElementValue(responseNode, responseXML)
            GlErrStepInfo = "GetXMLData_3"
            'Detail element retrieve & check																			
            GetDetailElementValue(responseNode, responseXML)
            GlErrStepInfo = "GetXMLData_4"
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End
    End Sub

    Public Sub GetHeadElementValue(ByVal responseNode As XmlNode, ByVal responseXML As XmlResponse)
        GlErrStepInfo = "GetHeadElementValue_Start"
        '20140317 Fujita Upd Start
        Try

            'Retrieve <Head> tag content																								
            Dim headXml As XmlNode = GetChildNode(responseNode, CstXmlHead, CstStrMandatory, CStr(NodeName.Head)).CloneNode(True)

            'Call GetNodeInnerText method, perform response interface tag information input check																								
            responseXML.HeadResponse.MessageID = GetNodeInnerText(headXml, CstNodeMessageID, CstStrMandatory, ItemCheck.Size7, CstStrTypeHalfChar, CstValueMessageId, Head.MessageID)
            responseXML.HeadResponse.CountryCode = GetNodeInnerText(headXml, CstItemCountryCode, CstStrMandatory, ItemCheck.Size2, CstStrTypeHalfChar, "", Head.CountryCode)
            responseXML.HeadResponse.ReceptionDate = GetNodeInnerText(headXml, CstItemReceptionDate, CstStrMandatory, ItemCheck.Size19, CstStrTypeDate, "", Head.ReceptionDate)
            responseXML.HeadResponse.TransmissionDate = GetNodeInnerText(headXml, CstItemTransmissionDate, CstStrMandatory, ItemCheck.Size19, CstStrTypeDate, "", Head.TransmissionDate)
            GlErrStepInfo = "GetHeadElementValue_End"
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Sub

    'Detail element retrieve & check																									
    Public Sub GetDetailElementValue(ByVal responseNode As XmlNode, ByVal responseXML As XmlResponse)
        GlErrStepInfo = "GetDetailElementValue_Start"
        '20140317 Fujita Upd Start
        Try

            'Retrieve <Detail> tag																									
            GlErrStepInfo = "GetDetailElementValue_1"
            Dim DetailXml As XmlNode = GetChildNode(responseNode, CstXmlDetail, CstStrMandatory, CStr(NodeName.Detail)).CloneNode(True)

            'Retrieve contents of <Detail><Common> tag																									
            GlErrStepInfo = "GetDetailElementValue_2"
            Dim DetailCommonXml As XmlNode = GetChildNode(DetailXml, CstXmlDetailCommon, CstStrMandatory, CStr(ReturnCode.MandatoryItemError)).CloneNode(True)

            'Call GetNodeInnerText method, perform response interface tag information input check																									
            responseXML.CommonResponse.ResultId = GetNodeInnerText(DetailCommonXml, CstNodeResultId, CstStrMandatory, ItemCheck.Size6, CstStrTypeHalfChar, "", DetailCommon.ResultId)
            responseXML.CommonResponse.Message = GetNodeInnerText(DetailCommonXml, CstItemMessage, CstStrMandatory, ItemCheck.Size1024, CstStrTypeHalfChar, "", DetailCommon.Message)

            '////// Change Start  //////
            'Retrieve contents of <Detail><FollowUpInfo> tag																								
            GlErrStepInfo = "GetDetailElementValue_3"
            Dim wkXmlNode As XmlNode
            Dim DetailFollowUpInfoXml As XmlNode

            GlErrStepInfo = "GetDetailElementValue_4_1"
            GlErrStepInfo = "GetChildNode_Pre"
            wkXmlNode = GetChildNode(DetailXml, CstXmlDetailFollowUpInfo, CstStrMandatory, ReturnCode.MandatoryItemError)
            GlErrStepInfo = "GetChildNode_After"

            If (wkXmlNode Is Nothing) Then
                GlErrStepInfo = "GetDetailElementValue_4_2"

                responseXML.FollowUpInfo.FollowUpID = ""
                WriteErrorInfo("GetNodeInnerText", DetailFollowUpInfo.FollowUpID, CstStrErrMsgMandatory, "")
            Else
                GlErrStepInfo = "GetDetailElementValue_4_3"

                DetailFollowUpInfoXml = wkXmlNode.CloneNode(True)
                responseXML.FollowUpInfo.FollowUpID = GetNodeInnerText(DetailFollowUpInfoXml, CstNodeFollowUpID, CstStrMandatory, ItemCheck.Size32, CstStrTypeHalfChar, "", DetailFollowUpInfo.FollowUpID)
            End If

            'ISSUE-IT2-1_by_takeda_start
            ''Call GetNodeInnerText method, perform response interface tag information input check																								
            'responseXML.FollowUpInfo.FollowUpID = GetNodeInnerText(DetailFollowUpInfoXml, CstNodeFollowUpID, CstStrMandatory, ItemCheck.Size32, CstStrTypeHalfChar, "", DetailFollowUpInfo.FollowUpID)


            '////// Change End  //////


            '////// Original Start  //////

            ''Retrieve contents of <Detail><FollowUpInfo> tag																								
            'GlErrStepInfo="GetDetailElementValue_3")
            'Dim DetailFollowUpInfoXml As XmlNode = GetChildNode(DetailXml, CstXmlDetailFollowUpInfo, CstStrMandatory, ReturnCode.MandatoryItemError).CloneNode(True)

            ''ISSUE-IT2-1_by_takeda_start
            ' ''Call GetNodeInnerText method, perform response interface tag information input check																								
            ''responseXML.FollowUpInfo.FollowUpID = GetNodeInnerText(DetailFollowUpInfoXml, CstNodeFollowUpID, CstStrMandatory, ItemCheck.Size32, CstStrTypeHalfChar, "", DetailFollowUpInfo.FollowUpID)

            'If (DetailFollowUpInfoXml IsNot Nothing) Then
            '    GlErrStepInfo="GetDetailElementValue_4_1")
            '    'Call GetNodeInnerText method, perform response interface tag information input check	
            '    responseXML.FollowUpInfo.FollowUpID = GetNodeInnerText(DetailFollowUpInfoXml, CstNodeFollowUpID, CstStrMandatory, ItemCheck.Size32, CstStrTypeHalfChar, "", DetailFollowUpInfo.FollowUpID)
            'Else
            '    GlErrStepInfo="GetDetailElementValue_4_2")
            '    'Call GetNodeInnerText method, perform response interface tag information input check
            '    WriteErrorInfo("GetNodeInnerText", DetailFollowUpInfo.FollowUpID, CstStrErrMsgMandatory, "")
            '    responseXML.FollowUpInfo.FollowUpID = ""
            'End If
            ''ISSUE-IT2-1_by_takeda_end

            '////// Original End  //////


            GlErrStepInfo = "GetDetailElementValue_End"
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Sub

    'ISSUE-0029_20130221_by_chatchai_Start
    ' Default date change process
    Public Function ChangeDefaultDate(ByVal TargetDate As Date, ByVal DefaultDate As Date) As String

        Dim strRtnDate As String
        Try
            ' Judge target date and default date																						
            If (TargetDate.Equals(DefaultDate)) Then
                'takeda_update_start_20140530
                '変換対象の日付が、DB初期値の場合は、空文字(0バイト)を返却
                ' In case it is same as target date, return blank space value																					
                'strRtnDate = " "
                strRtnDate = ""
                'takeda_update_end_20140530
            Else
                ' In case it is not default date, set target date as before																					
                strRtnDate = EditDateFormat(TargetDate)
            End If

            Return strRtnDate

        Catch ex As System.Exception
            ' In case of exception, return blank space																						
            strRtnDate = ""
            Return strRtnDate

        End Try

    End Function
    'ISSUE-0029_20130221_by_chatchai_End

    'takeda_update_start_20140412
    ' Default date change process
    Public Function ConvertStfCd(ByVal icropStfCd As String, ByVal changeMode As String) As String

        Dim strRtnStfCd As String
        Dim intChkStfCd As Long
        Dim strIcropDlrCd As String
        Dim strDmsDlrCd As String
        Try
            ' Judge target date and default date																						
            If (String.IsNullOrEmpty(icropStfCd)) Then
                'スタッフコードが取得できない場合(空白,Null)、空文字(0バイト)返却
                strRtnStfCd = " "
                Return strRtnStfCd
            End If

            If (changeMode = "1") Then
                'スタッフコード変換を行う

                'ICROPのスタッフコード取得(スタッフコード@販売店)
                intChkStfCd = icropStfCd.IndexOf("@")
                If (intChkStfCd >= 0) Then
                    '先頭からスタッフコードだけ抜き出す(@以降を削除)
                    strRtnStfCd = icropStfCd.Substring(0, intChkStfCd)
                    Return strRtnStfCd
                Else
                    '@がない場合、入力パラメタをそのまま返却
                    Return icropStfCd
                End If
            Else
                'スタッフコード変換を行わない

                'ICROPのスタッフコード取得(スタッフコード@販売店)
                intChkStfCd = icropStfCd.IndexOf("@")
                If (intChkStfCd >= 0) Then
                    'ICROP販売店コードを取得します(@以降を取得)
                    strIcropDlrCd = icropStfCd.Substring(intChkStfCd + 1)
                    'DMS販売店コードに変換します
                    strDmsDlrCd = ChangeDlrCd(strIcropDlrCd, "DMS")
                    'スタッフコード@DMS販売店コードに変換します
                    strRtnStfCd = icropStfCd.Substring(0, intChkStfCd) + "@" + strDmsDlrCd

                    Return strRtnStfCd
                Else
                    '@がない場合、入力パラメタをそのまま返却
                    Return icropStfCd
                End If

            End If

        Catch ex As System.Exception
            ' In case of exception, return blank space																						
            strRtnStfCd = ""
            Return strRtnStfCd

        End Try

    End Function
    'takeda_update_end_20140412


    Public Sub MoveHistory(ByVal SalesId As Long)
        Dim IC3802801TableAdapter As New IC3802801TableAdapter
        '20140317 Fujita Upd Start
        Try
            ' Transfer Operation TBL data to History TBL																			
            'Activity SEQ Manager 																			
            IC3802801TableAdapter.MoveActionSeq(SalesId)

            ' Delete Operation TBL data after transferring to HistoryTBL																			
            'Activity SEQ Manager 																			
            IC3802801TableAdapter.DeleteActionSeq(SalesId)
        Catch ex As Exception
            'takeda_update_start_20140617
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            'takeda_update_end_20140617
            Throw ex
        End Try
        '20140317 Fujita Upd End

    End Sub

    '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) START
    ''' <summary>
    ''' CDATAセクション作成
    ''' </summary>
    ''' <param name="xmlDoc">XmlDocument</param>
    ''' <param name="name">ノードの名前</param>
    ''' <param name="text">ノードのテキストの内容</param>
    ''' <returns>CDATAセクション</returns>
    ''' <remarks></remarks>
    Public Function CreateCDataSection(ByVal xmlDoc As XmlDocument, ByVal name As String, ByVal text As String) As XmlNode

        Dim xmlNode As XmlNode = xmlDoc.CreateElement(name)

        If String.IsNullOrEmpty(text) Then
            xmlNode.InnerText = text
        ElseIf text.Trim.Length.Equals(0) Then
            xmlNode.InnerText = text
        Else
            xmlNode.AppendChild(xmlDoc.CreateCDataSection(text))
        End If

        Return xmlNode

    End Function
    '2015/02/05 SKFC 松田 ADD SA01改修(TMT切替BTS-166対応) END

    '$26 start
    Public Function SetXmlSalesLocal(ByVal SalesId As String)

        Try
            Dim xmlSalesLocal As New XmlSalesLocal
            Dim salesLocalData As IC3802801SalesLocalDataTable =
            CType(IC3802801TableAdapter.GetSalesLocal(SalesId), IC3802801DataSet.IC3802801SalesLocalDataTable)

            If (salesLocalData.Rows.Count > 0) Then
                Dim salesLocalRow As IC3802801DataSet.IC3802801SalesLocalRow =
                        CType(salesLocalData.Rows(0), IC3802801SalesLocalRow)

                xmlSalesLocal.DemandStructureCd = salesLocalRow.DEMAND_STRUCTURE_CD

                If (IsDBNull(salesLocalRow.Item("TRADEINCAR_ENABLED_FLG")) = False) Then
                    If (TradeincarEnabledAvailable.Equals(salesLocalRow.TRADEINCAR_ENABLED_FLG)) Then
                        xmlSalesLocal.TradeincarEnabledFlg = salesLocalRow.TRADEINCAR_ENABLED_FLG
                        xmlSalesLocal.TradeinDate = ChangeDefaultDate(salesLocalRow.ROW_CREATE_DATETIME, CstStrDefaultDate)
                        xmlSalesLocal.CreateDate = ChangeDefaultDate(salesLocalRow.ROW_CREATE_DATETIME, CstStrDefaultDate)
                        xmlSalesLocal.ModelYear = salesLocalRow.TRADEINCAR_MODEL_YEAR
                        xmlSalesLocal.DistanceCovered = salesLocalRow.TRADEINCAR_MILE
                        If (IsDBNull(salesLocalRow.Item("MAKER_NAME")) = False) Then
                            xmlSalesLocal.MakerName = salesLocalRow.MAKER_NAME
                        End If
                        If (IsDBNull(salesLocalRow.Item("MODEL_NAME")) = False) Then
                            xmlSalesLocal.SeriesName = salesLocalRow.MODEL_NAME
                        End If
                    End If
                End If
            End If
            Return xmlSalesLocal
        Catch ex As Exception
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            Throw ex
        End Try
    End Function
    '$26 end

    '$26 start
    Public Function SetTradeinLocal(ByVal xmlDoc As XmlDocument, ByVal xmlSalesLocal As XmlSalesLocal) As XmlNode
        GlErrStepInfo = "SetTradein_Start"
        Try
            Dim TradeinNode As XmlNode = xmlDoc.CreateElement("Tradein")

            Dim TradeinDateNode As XmlNode = xmlDoc.CreateElement("TradeinDate")
            TradeinDateNode.InnerText = xmlSalesLocal.TradeinDate
            TradeinNode.AppendChild(TradeinDateNode)

            Dim MakerNameNode As XmlNode = xmlDoc.CreateElement("MakerName")
            MakerNameNode.InnerText = xmlSalesLocal.MakerName
            TradeinNode.AppendChild(MakerNameNode)

            Dim SeriesNameNode As XmlNode = xmlDoc.CreateElement("SeriesName")
            SeriesNameNode.InnerText = xmlSalesLocal.SeriesName
            TradeinNode.AppendChild(SeriesNameNode)

            Dim ModelYearNode As XmlNode = xmlDoc.CreateElement("ModelYear")
            ModelYearNode.InnerText = xmlSalesLocal.ModelYear
            TradeinNode.AppendChild(ModelYearNode)

            Dim CreateDateNode As XmlNode = xmlDoc.CreateElement("CreateDate")
            CreateDateNode.InnerText = xmlSalesLocal.CreateDate
            TradeinNode.AppendChild(CreateDateNode)

            Dim DistanceCoveredNode As XmlNode = xmlDoc.CreateElement("DistanceCovered")
            DistanceCoveredNode.InnerText = xmlSalesLocal.DistanceCovered
            TradeinNode.AppendChild(DistanceCoveredNode)

            Return TradeinNode
        Catch ex As Exception
            '実行メソッド名取得
            Dim strMtdNm = System.Reflection.MethodBase.GetCurrentMethod().Name
            Logger.Error("ERROR METHOD NAME:" + strMtdNm)
            Throw ex
        End Try
    End Function
    '$26 end

End Class
