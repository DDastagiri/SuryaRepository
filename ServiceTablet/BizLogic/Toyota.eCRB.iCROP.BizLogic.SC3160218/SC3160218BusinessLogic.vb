'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'SC3160218BusinessLogic.vb
'─────────────────────────────────────
'機能： RO作成機能グローバル連携処理
'補足： 
'作成： 2013/11/25 SKFC 久代 
'更新： 
'─────────────────────────────────────
Option Strict On
Option Explicit On

Imports System
Imports System.Text
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.SystemFrameworks.Web.Controls
Imports Toyota.eCRB.iCROP.DataAccess.SC3160218

''' <summary>
''' SC3160218ビジネスロジッククラス
''' </summary>
''' <remarks></remarks>
Public Class SC3160218BusinessLogic
    Inherits BaseBusinessComponent

#Region "DTOクラス宣言"
    ''' <summary>
    ''' 接続パラメータクラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ParamInfo
        Public VISIT_SEQ As Decimal  ' SAChipID
        Public DealerCode As String
        Public BranchCode As String
        Public LoginUserID As String
        Public BASREZID As String
        Public R_O As String
        Public SEQ_NO As Decimal
        Public VIN_NO As String
        Public ViewMode As Long
        Public CheckboxDisp As Long
        Public LegendDisp As Long
        Public ScaleMode As Long
    End Class

    ''' <summary>
    ''' ダメージ情報クラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DamageInfo
        Public RO_EXTERIOR_ID As Decimal
        Public NO_DAMAGE_FLG As String
        Public CANNOT_CHECK_FLG As String
        Public data As New ArrayList
    End Class

    ''' <summary>
    ''' ダメージデータ
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DamageData
        Public PARTS_TYPE As String
        Public DAMAGE_TYPE_1 As String
        Public DAMAGE_TYPE_2 As String
        Public RO_THUMBNAIL_ID As Decimal
    End Class

    ''' <summary>
    ''' 凡例データ
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ExplanationInfo
        Public type As String
        Public word_num As Decimal
        Public fromColor As String
        Public toColor As String
    End Class
#End Region

#Region "公開メソッド"
    ''' <summary>
    ''' 損傷凡例データ取得
    ''' </summary>
    ''' <returns>凡例データ(DTO配列)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetExplanationInfo() As ArrayList
        Logger.Info("SC3160218BusinessLogic.GetExplanationInfo function Begin.")

        Dim resultList As ArrayList = New ArrayList

        Dim dataSet As SC3160218DataSet.TB_M_RO_DAMAGE_TYPEDataTable

        dataSet = SC3160218TableAdapter.GetDamageTypeInfo()
        If 0 < dataSet.Count Then
            For i As Integer = 0 To dataSet.Count - 1
                Dim ex As ExplanationInfo = New ExplanationInfo
                ex.type = dataSet.Item(i).DAMAGE_TYPE
                ex.word_num = Decimal.Parse(dataSet.Item(i).DAMAGE_WORD_NUM)
                ex.fromColor = dataSet.Item(i).GRADATION_FROM
                ex.toColor = dataSet.Item(i).GRADATION_TO
                resultList.Add(ex)
            Next
        End If

        Logger.Info("SC3160218BusinessLogic.GetExplanationInfo function End.")
        Return resultList
    End Function

    ''' <summary>
    ''' 外観損傷情報データの取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetExteriorId(ByVal inParam As ParamInfo) As Decimal
        Logger.Info("SC3160218BusinessLogic.GetExteriorId function Begin.")
        Dim result As Decimal = -1

        'RO外装情報の取得
        Dim exteriorDataSet As SC3160218DataSet.RO_EXTERIORDataTable
        exteriorDataSet = SC3160218TableAdapter.GetExteriorInfo(inParam.VISIT_SEQ,
                                                                inParam.R_O,
                                                                inParam.DealerCode,
                                                                inParam.BranchCode)
        If 1 = exteriorDataSet.Count Then
            result = exteriorDataSet.Item(0).RO_EXTERIOR_ID
        ElseIf 1 < exteriorDataSet.Count Then
            Throw New ApplicationException("データベース不正")
        End If

        Logger.Info("SC3160218BusinessLogic.GetExteriorId function end.")

        Return result
    End Function

    ''' <summary>
    ''' 外観損傷情報データの取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetExteriorDamageInfo(ByVal RO_EXTERIOR_ID As Decimal,
                                                 ByRef outDamageInfo As DamageInfo) As Boolean
        Logger.Info("SC3160218BusinessLogic.GetExteriorDamageInfo function Begin.")

        'RO外装情報の取得
        Dim exteriorDataSet As SC3160218DataSet.RO_EXTERIORDataTable
        exteriorDataSet = SC3160218TableAdapter.GetExteriorInfoFromId(RO_EXTERIOR_ID)
        If 1 <> exteriorDataSet.Count Then
            Logger.Info("SC3160218BusinessLogic.GetExteriorDamageInfo Exterior data num=" + exteriorDataSet.Count.ToString + ".")
            Logger.Info("SC3160218BusinessLogic.GetExteriorDamageInfo function End.")
            Return False
        End If

        'RO外装ID取得
        outDamageInfo.RO_EXTERIOR_ID = RO_EXTERIOR_ID
        'フラグデータ取得
        outDamageInfo.NO_DAMAGE_FLG = exteriorDataSet.Item(0).NO_DAMAGE_FLG
        outDamageInfo.CANNOT_CHECK_FLG = exteriorDataSet.Item(0).CANNOT_CHECK_FLG

        'RO外装情報の取得
        Dim damageDataSet As SC3160218DataSet.TB_T_RO_EXTERIOR_DAMAGEDataTable
        damageDataSet = SC3160218TableAdapter.GetDamageInfo(RO_EXTERIOR_ID)

        '損傷データ取得
        outDamageInfo.data.Clear()
        If 0 < damageDataSet.Count Then
            For i As Integer = 0 To damageDataSet.Count - 1
                Dim damageData As New DamageData
                ' 部位種別
                damageData.PARTS_TYPE = damageDataSet.Item(i).PARTS_TYPE
                ' 損傷情報の上位2つをピックアップ
                Dim damageTypes As String = damageDataSet.Item(i).DAMAGE_TYPE_EXISTS
                If 0 < damageTypes.Length Then
                    If "-" <> damageTypes Then
                        damageData.DAMAGE_TYPE_1 = damageTypes.Substring(0, 1)
                        If 1 < damageTypes.Length Then
                            damageData.DAMAGE_TYPE_2 = damageTypes.Substring(1, 1)
                        End If
                    End If
                End If
                ' サムネイルID
                damageData.RO_THUMBNAIL_ID = damageDataSet.Item(i).RO_THUMBNAIL_ID
                outDamageInfo.data.Add(damageData)
            Next
        End If

        Logger.Info("SC3160218BusinessLogic.GetExteriorDamageInfo function end.")
        Return True
    End Function

    ''' <summary>
    ''' 外観損傷情報データ追加
    ''' </summary>
    ''' <param name="inParam"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function AddExteriorDamageInfo(ByVal inParam As ParamInfo) As Decimal
        Logger.Info("SC3160218BusinessLogic.AddExteriorDamageInfo function Begin.")

        Dim result As Decimal = -1

        '外観損傷情報データ追加
        result = SC3160218TableAdapter.AddExteriorInfo(inParam.DealerCode,
                                                       inParam.BranchCode,
                                                       inParam.VISIT_SEQ,
                                                       inParam.BASREZID,
                                                       inParam.R_O,
                                                       inParam.SEQ_NO,
                                                       inParam.VIN_NO,
                                                       inParam.LoginUserID)

        Logger.Info("SC3160218BusinessLogic.AddExteriorDamageInfo function end.")

        Return result
    End Function

    ''' <summary>
    ''' NoDamageフラグの更新
    ''' </summary>
    ''' <param name="RO_EXTERIOR_ID">RO外装ID</param>
    ''' <param name="NO_DAMAGE_FLG">NoDamageフラグ</param>
    ''' <param name="UserId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateNoDamage(ByVal RO_EXTERIOR_ID As Decimal,
                                          ByVal NO_DAMAGE_FLG As Boolean,
                                          ByVal UserId As String) As Long
        Logger.Info("SC3160218BusinessLogic.UpdateNoDamage function Begin.")
        Dim result As Long = -1L

        Dim flg As String = If(True = NO_DAMAGE_FLG, "1", "0")
        result = SC3160218TableAdapter.UpdateNoDamage(RO_EXTERIOR_ID, flg, UserId)

        ' NO_DAMAGE_FLGがONの場合
        If NO_DAMAGE_FLG Then
            SC3160218TableAdapter.DeleteDamageInfo(RO_EXTERIOR_ID, UserId)
        End If


        Logger.Info("SC3160218BusinessLogic.UpdateNoDamage function end.")

        Return result
    End Function

    ''' <summary>
    ''' Can'tCheckフラグの更新
    ''' </summary>
    ''' <param name="RO_EXTERIOR_ID">RO外装ID</param>
    ''' <param name="CANNOT_CHECK_FLG">Can'tCheckフラグ</param>
    ''' <param name="UserId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function UpdateCanNotCheck(ByVal RO_EXTERIOR_ID As Decimal,
                                             ByVal CANNOT_CHECK_FLG As Boolean,
                                             ByVal UserId As String) As Long
        Logger.Info("SC3160218BusinessLogic.UpdateCanNotCheck function Begin.")
        Dim result As Long = -1L

        Dim flg As String = If(True = CANNOT_CHECK_FLG, "1", "0")
        result = SC3160218TableAdapter.UpdateCanNotCheck(RO_EXTERIOR_ID, flg, UserId)

        Logger.Info("SC3160218BusinessLogic.UpdateCanNotCheck function end.")

        Return result
    End Function

    ''' <summary>
    ''' 販売店/店舗コード変換
    ''' </summary>
    ''' <param name="orgDlrCd">基幹販売店コード</param>
    ''' <param name="orgStrCd">基幹店舗コード</param>
    ''' <param name="dlrCd">販売店コード</param>
    ''' <param name="strCd">店舗コード</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ChangeDealerOrg2Icrop(ByVal orgDlrCd As String,
                                                 ByVal orgStrCd As String,
                                                 ByRef dlrCd As String,
                                                 ByRef strCd As String) As Boolean
        Logger.Info("SC3160218BusinessLogic.ChangeDealerOrg2Icrop function Begin.")

        Dim dataset As SC3160218DataSet.TB_M_DMS_CODE_MAPDataTable
        dataset = SC3160218TableAdapter.ChangeDlrStrCodeToICROP(orgDlrCd, orgStrCd)
        If 1 <> dataset.Count Then
            Logger.Info("SC3160218BusinessLogic.ChangeDealerOrg2Icrop not found dearler store code. end.")
            Return False
        End If

        dlrCd = dataset.Item(0).ICROP_CD_1
        strCd = dataset.Item(0).ICROP_CD_2

        Logger.Info("SC3160218BusinessLogic.ChangeDealerOrg2Icrop function end.")
        Return True
    End Function

    ''' <summary>
    ''' プログラム設定取得
    ''' </summary>
    ''' <param name="PROGRAM_CD"></param>
    ''' <param name="SETTING_SECTION"></param>
    ''' <param name="SETTING_KEY"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetProgramSetting(ByVal PROGRAM_CD As String,
                                             ByVal SETTING_SECTION As String,
                                             ByVal SETTING_KEY As String) As String
        Logger.Info("SC3160218BusinessLogic.GetProgramSetting function begin.")
        Dim result As String

        result = SC3160218TableAdapter.GetProgramSetting(PROGRAM_CD,
                                                          SETTING_SECTION,
                                                          SETTING_KEY)

        Logger.Info("SC3160218BusinessLogic.GetProgramSetting function end.")
        Return result
    End Function

#End Region

End Class
