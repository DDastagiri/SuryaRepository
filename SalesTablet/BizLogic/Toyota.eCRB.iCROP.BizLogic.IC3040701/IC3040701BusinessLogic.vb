'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'IC3040701BusinessLogic.vb
'─────────────────────────────────────
'機能： テンプレート取得インターフェイス ビジネスロジック
'補足： 
'作成： 2014/05/13 TMEJ 曽山
'更新： 2016/01/07 NSK nakamura PRJ1504572_(トライ店システム評価)メールテンプレート機能強化(敬称置換文字追加) $01
'─────────────────────────────────────

Option Explicit On
Option Strict On

Imports System.Text
Imports System.Globalization
Imports System.Xml.Serialization
Imports Toyota.eCRB.SystemFrameworks.Core
Imports Toyota.eCRB.SystemFrameworks.Web
Imports Toyota.eCRB.iCROP.DataAccess.IC3040701
Imports System.Linq
Imports System.IO
Imports System.Xml.Linq
Imports System.Collections
Imports Toyota.eCRB.iCROP.DataAccess.IC3040701.IC3040701DataSetTableAdapters

''' <summary>
''' テンプレート取得インターフェイス ビジネスロジック
''' </summary>
''' <remarks></remarks>
Public Class IC3040701BusinessLogic
    Inherits BaseBusinessComponent
    Implements IDisposable

#Region "定数"

    ''' <summary>
    ''' 文字チェックを行う際の型
    ''' </summary>
    Private Enum TypeConversion As Integer

        ''' <summary>チェックをしない</summary>
        None

        ''' <summary>文字列チェックを行う</summary>
        StringType

        ''' <summary>数値チェックを行う</summary>
        IntegerType

    End Enum

    ''' <summary>
    ''' エラー番号
    ''' </summary>
    Private Enum ReturnCode As Integer

        ''' <summary>項目必須エラー</summary>
        NotXmlElementError = 2000

        ''' <summary>項目型エラー</summary>
        XmlParseError = 3000

        ''' <summary>項目サイズエラー</summary>
        XmlMaximumOfDigitError = 4000

        ''' <summary>値チェックエラー</summary>
        XmlValueCheckError = 5000

        ''' <summary>テンプレートが見つからないエラー</summary>
        TemplateNotFound = 1101

        ''' <summary>正常終了</summary>
        Successful = 0

        ''' <summary>XMLタグ不正エラー</summary>
        XmlIncorrect = -1

        ''' <summary>スタッフコードチェックエラー</summary>
        StaffCodeError = 1

        ''' <summary>エラーコード：システムエラー</summary>
        ErrCodeSys = 9999

    End Enum

    ''' <summary>
    ''' XML要素に対応した処理結果コード
    ''' </summary>
    Private Enum ElementCode As Integer

        ''' <summary>TransmissionDate要素</summary>
        TransmissionDate = 1

        ''' <summary>DealerCode要素</summary>
        DealerCode = 101

        ''' <summary>StoreCode要素</summary>
        StoreCode = 102

        ''' <summary>TemplateClass要素</summary>
        TemplateClass = 103

        ''' <summary>DisplayID要素</summary>
        DisplayID = 104

        ''' <summary>CustomId要素</summary>
        CustomId = 105

        ''' <summary>StaffCode要素</summary>
        StaffCode = 106

        ''' <summary>SalesID要素</summary>
        SalesID = 107
    End Enum

    ''' <summary>
    ''' 処理結果メッセージ
    ''' </summary>
    Private Class Message

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>インスタンス生成抑制のため</remarks>
        Private Sub New()
        End Sub

        ''' <summary>Success</summary>
        Public Const Success As String = "Success"

        ''' <summary>Failure</summary>
        Public Const Failure As String = "Failure"
    End Class

    ''' <summary>
    ''' XML要素名
    ''' </summary>
    Private Class NodeName

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>インスタンス生成抑制のため</remarks>
        Private Sub New()
        End Sub

        ''' <summary>Head要素</summary>
        Public Const Head As String = "Head"
        ''' <summary>TransmissionDate要素</summary>
        Public Const TransmissionDate As String = "TransmissionDate"

        ''' <summary>Detail要素</summary>
        Public Const Detail As String = "Detail"

        ''' <summary>RequestTemplate要素</summary>
        Public Const RequestTemplate As String = "RequestTemplate"

        ''' <summary>DealerCode要素</summary>
        Public Const DealerCode As String = "DealerCode"

        ''' <summary>StoreCode要素</summary>
        Public Const StoreCode As String = "StoreCode"

        ''' <summary>DisplayID要素</summary>
        Public Const DisplayID As String = "DisplayID"

        ''' <summary>TemplateClass要素</summary>
        Public Const TemplateClass As String = "TemplateClass"

        ''' <summary>CustomId要素</summary>
        Public Const CustomId As String = "CustomId"

        ''' <summary>StaffCode要素</summary>
        Public Const StaffCode As String = "StaffCode"

        ''' <summary>SalesID要素</summary>
        Public Const SalesID As String = "SalesID"
    End Class

    ''' <summary>TransmissionDate要素のチェック内容</summary>
    ''' <remarks>実際に値を使用しないため、型変換は行わない。従ってTypeはNone。</remarks>
    Private ReadOnly TransmissionDateNode As New NodeCheckInfo With _
        {.Name = NodeName.TransmissionDate, .MaxLength = 19, .Type = TypeConversion.None, _
         .IsMandatory = True, .ElementCode = ElementCode.TransmissionDate}

    ''' <summary>DealerCode要素のチェック内容</summary>
    Private ReadOnly DealerCodeNode As New NodeCheckInfo With _
        {.Name = NodeName.DealerCode, .MaxLength = 5, .Type = TypeConversion.StringType, _
         .IsMandatory = True, .ElementCode = ElementCode.DealerCode}

    ''' <summary>StoreCode要素のチェック内容</summary>
    Private ReadOnly StoreCodeNode As New NodeCheckInfo With _
        {.Name = NodeName.StoreCode, .MaxLength = 3, .Type = TypeConversion.StringType, _
         .IsMandatory = True, .ElementCode = ElementCode.StoreCode}

    ''' <summary>DisplayID要素のチェック内容</summary>
    Private ReadOnly DisplayIDNode As New NodeCheckInfo With _
        {.Name = NodeName.DisplayID, .MaxLength = 16, .Type = TypeConversion.StringType, _
         .IsMandatory = True, .ElementCode = ElementCode.DisplayID}

    ''' <summary>TemplateClass要素のチェック内容</summary>
    Private ReadOnly TemplateClassNode As New NodeCheckInfo With _
        {.Name = NodeName.TemplateClass, .MaxLength = 1, .Type = TypeConversion.StringType, _
         .IsMandatory = True, .Range = {"1", "2"}, .ElementCode = ElementCode.TemplateClass}

    ''' <summary>CustomId要素のチェック内容</summary>
    Private ReadOnly CustomIdNode As New NodeCheckInfo With _
        {.Name = NodeName.CustomId, .MaxLength = 20, .Type = TypeConversion.IntegerType, _
         .IsMandatory = False, .ElementCode = ElementCode.CustomId}

    ''' <summary>StaffCode要素のチェック内容</summary>
    Private ReadOnly StaffCodeNode As New NodeCheckInfo With _
        {.Name = NodeName.StaffCode, .MaxLength = 20, .Type = TypeConversion.StringType, _
         .IsMandatory = False, .ElementCode = ElementCode.StaffCode}

    ''' <summary>SalesID要素のチェック内容</summary>
    Private ReadOnly SalesIDNode As New NodeCheckInfo With _
        {.Name = NodeName.SalesID, .MaxLength = 20, .Type = TypeConversion.IntegerType, _
         .IsMandatory = False, .ElementCode = ElementCode.SalesID}

    ''' <summary>
    ''' ArgumentExceptionの処理結果コードを表すキー名
    ''' </summary>
    Private Const ReturnCodeKey As String = "ReturnCode"

    ''' <summary>
    ''' 置換文字列
    ''' </summary>
    Private Class ReplaceLiteral

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <remarks>インスタンス生成抑制のため</remarks>
        Private Sub New()
        End Sub

        ''' <summary>販売店名称</summary>
        Public Const DlrName As String = "**DLRNAME"

        ''' <summary>販売店名称(省略記法)</summary>
        Public Const DlrNameNum As String = "*2"

        ''' <summary>販売店URL</summary>
        Public Const DlrUrl As String = "**DLRURL"

        ''' <summary>販売店URL(省略記法)</summary>
        Public Const DlrUrlNum As String = "*7"

        ''' <summary>店舗名称</summary>
        Public Const BranchName As String = "**BRANCHNAME"

        ''' <summary>店舗名称(省略記法)</summary>
        Public Const BranchNameNum As String = "*3"

        ''' <summary>スタッフ名称</summary>
        Public Const StaffName As String = "**STAFF_NAME"

        ''' <summary>スタッフ名称(省略記法)</summary>
        Public Const StaffNameNum As String = "*6"

        ''' <summary>顧客氏名</summary>
        Public Const CustName As String = "**CUSTNAME"

        ''' <summary>顧客氏名(省略記法)</summary>
        Public Const CustNameNum As String = "*1"

        ''' <summary>法人担当者名</summary>
        Public Const Department As String = "**DEPARTMENT"

        ''' <summary>法人担当者所属部署</summary>
        Public Const JobTitle As String = "**JOBTITLE"

        ''' <summary>法人担当者役職</summary>
        Public Const PersonInCharge As String = "**PERSONINCHARGE"

        ''' <summary>シリーズ名</summary>
        Public Const SeriesName As String = "**SERIES_NAME"

        ''' <summary>シリーズ名(省略記法)</summary>
        Public Const SeriesNameNum As String = "*4"

        ''' <summary>点検推奨日</summary>
        Public Const CRDate As String = "**CRDATE"

        ''' <summary>点検推奨日(省略記法)</summary>
        Public Const CRDateNum As String = "*5"

        ''' <summary>サービス名</summary>
        Public Const ServiceName As String = "**SERVICE_NAME"

        ''' <summary>サービス名(省略記法)</summary>
        Public Const ServiceNameNum As String = "*8"

        ''' <summary>メーカー名</summary>
        Public Const MakerName As String = "**MAKERNAME"

        ''' <summary>メーカー名(省略記法)</summary>
        Public Const MakerNameNum As String = "*9"

        ''' <summary>敬称</summary>
        Public Const NameTitle As String = "**NAMETITLE"
    End Class

    ''' <summary>
    ''' 置換文字カテゴリ：販売店・店舗
    ''' </summary>
    Private ReadOnly DlrBrnLiteral As New List(Of String) From _
        {ReplaceLiteral.DlrName, ReplaceLiteral.DlrNameNum,
         ReplaceLiteral.DlrUrl, ReplaceLiteral.DlrUrlNum, _
         ReplaceLiteral.BranchName, ReplaceLiteral.BranchNameNum}

    ''' <summary>
    ''' 置換文字カテゴリ：スタッフ
    ''' </summary>
    Private ReadOnly StaffLiteral As New List(Of String) From {ReplaceLiteral.StaffName, ReplaceLiteral.StaffNameNum}

    ''' <summary>
    ''' 置換文字カテゴリ：車両情報
    ''' </summary>
    Private ReadOnly VehicleLiteral As New List(Of String) From _
        {ReplaceLiteral.SeriesName, ReplaceLiteral.SeriesNameNum,
         ReplaceLiteral.CRDate, ReplaceLiteral.CRDateNum, _
         ReplaceLiteral.ServiceName, ReplaceLiteral.ServiceNameNum, _
         ReplaceLiteral.MakerName, ReplaceLiteral.MakerNameNum}
#End Region

#Region "各XML要素のチェック内容クラス"

    ''' <summary>
    ''' 各XML要素のチェック内容
    ''' </summary>
    ''' <remarks></remarks>
    Private Class NodeCheckInfo
        ''' <summary>要素名</summary>
        Public Property Name As String = String.Empty

        ''' <summary>要素の処理結果コード</summary>
        Public Property ElementCode As Integer

        ''' <summary>要素の型チェック</summary>
        Public Property Type As TypeConversion = TypeConversion.None

        ''' <summary>必須項目か否か</summary>
        Public Property IsMandatory As Boolean

        ''' <summary>要素の最大長</summary>
        Public Property MaxLength As Integer

        ''' <summary>要素の取りうる値の列挙</summary>
        Public Property Range As String() = New String() {}

        ''' <summary>
        ''' このインスタンスの文字列表現を返却する。
        ''' </summary>
        ''' <returns>インスタンスの文字列表現を返却する。</returns>
        Public Overloads Function ToString() As String
            Dim sb As New StringBuilder
            With sb
                .Append(LogUtil.GetLogParam("Name", Me.Name, False))
                .Append(LogUtil.GetLogParam("ElementCode", CStr(Me.ElementCode), True))
                .Append(LogUtil.GetLogParam("Type", CStr(Me.Type), True))
                .Append(LogUtil.GetLogParam("IsMandatory", CStr(Me.IsMandatory), True))
                .Append(LogUtil.GetLogParam("MaxLength", CStr(Me.MaxLength), True))
                .Append(LogUtil.GetLogParam("Range", Me.Range, True))
            End With
            Return sb.ToString
        End Function
    End Class

#End Region

#Region "Responseクラス"
    ''' <summary>
    ''' Responseクラス(応答用XMLのIFクラス)
    ''' </summary>
    ''' <remarks>応答用のXML情報を格納するクラス</remarks>
    <System.Xml.Serialization.XmlRoot("Response", Namespace:="http://tempuri.org/Response.xsd")> _
    Public Class Response

        ''' <summary>
        ''' Headタグの定義
        ''' </summary>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElementAttribute(ElementName:="Head", IsNullable:=False)> _
        Private outHead As RootHead

        ''' <summary>
        ''' Detailタグの定義
        ''' </summary>
        ''' <remarks></remarks>
        <System.Xml.Serialization.XmlElementAttribute(ElementName:="Detail", IsNullable:=False)> _
        Private outDetail As RootDetail

        ''' <summary>
        ''' Headerタグ用プロパティ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Head() As RootHead
            Set(ByVal value As RootHead)
                outHead = value
            End Set
            Get
                Return outHead
            End Get
        End Property

        ''' <summary>
        ''' Detailタグ用プロパティ
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Detail() As RootDetail
            Set(ByVal value As RootDetail)
                outDetail = value
            End Set
            Get
                Return outDetail
            End Get
        End Property

        ''' <summary>
        ''' Headタグ用クラス
        ''' </summary>
        ''' <remarks></remarks>
        Public Class RootHead

            ''' <summary>
            ''' TransmissionDateタグの定義
            ''' </summary>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="TransmissionDate",
                                                          IsNullable:=False)> _
            Private outTransmissionDate As String

            ''' <summary>
            ''' TransmissionDateタグ用のプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="TransmissionDate",
                                                          IsNullable:=False)> _
            Public Property TransmissionDate As String
                Get
                    Return outTransmissionDate
                End Get
                Set(ByVal value As String)
                    outTransmissionDate = value
                End Set
            End Property
        End Class

        ''' <summary>
        ''' Detailタグ用クラス
        ''' </summary>
        ''' <remarks></remarks>
        Public Class RootDetail

            ''' <summary>
            ''' Commonタグの定義
            ''' </summary>
            ''' <remarks></remarks>
            <System.Xml.Serialization.XmlElementAttribute(ElementName:="Common",
                                                          IsNullable:=False)> _
            Private outCommon As DetailCommon

            ''' <summary>
            ''' Commonタグ用のプロパティ
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Property Common() As DetailCommon
                Set(ByVal value As DetailCommon)
                    outCommon = value
                End Set
                Get
                    Return outCommon
                End Get
            End Property

            ''' <summary>
            ''' Commonタグ用クラス
            ''' </summary>
            ''' <remarks></remarks>
            Public Class DetailCommon

                ''' <summary>
                ''' ReturnCodeタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="ReturnCode",
                                                              IsNullable:=False)> _
                Private outReturnCode As String

                ''' <summary>
                ''' Messageタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="Message",
                                                              IsNullable:=False)> _
                Private outMessage As String

                ''' <summary>
                ''' EmailAddressタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="EmailAddress",
                                                              IsNullable:=False)> _
                Private outEmailAddress As String

                ''' <summary>
                ''' TemplateSubjectタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="TemplateSubject",
                                                              IsNullable:=False)> _
                Private outTemplateSubject As String

                ''' <summary>
                ''' TemplateTextタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="TemplateText",
                                                              IsNullable:=False)> _
                Private outTemplateText As String

                ''' <summary>
                ''' TemplateClassタグの定義
                ''' </summary>
                ''' <remarks></remarks>
                <System.Xml.Serialization.XmlElementAttribute(ElementName:="TemplateClass",
                                                              IsNullable:=False)> _
                Private outTemplateClass As String
                ''' <summary>
                ''' ReturnCodeタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property ReturnCode() As String
                    Set(ByVal value As String)
                        outReturnCode = value
                    End Set
                    Get
                        Return outReturnCode
                    End Get
                End Property

                ''' <summary>
                ''' Messageタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property Message() As String
                    Set(ByVal value As String)
                        outMessage = value
                    End Set
                    Get
                        Return outMessage
                    End Get
                End Property

                ''' <summary>
                ''' EmailAddressタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property EmailAddress() As String
                    Set(ByVal value As String)
                        outEmailAddress = value
                    End Set
                    Get
                        Return outEmailAddress
                    End Get
                End Property

                ''' <summary>
                ''' TemplateSubjectタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property TemplateSubject() As String
                    Set(ByVal value As String)
                        outTemplateSubject = value
                    End Set
                    Get
                        Return outTemplateSubject
                    End Get
                End Property

                ''' <summary>
                ''' TemplateTextタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property TemplateText() As String
                    Set(ByVal value As String)
                        outTemplateText = value
                    End Set
                    Get
                        Return outTemplateText
                    End Get
                End Property


                ''' <summary>
                ''' TemplateClassタグ用のプロパティ
                ''' </summary>
                ''' <value></value>
                ''' <returns></returns>
                ''' <remarks></remarks>
                Public Property TemplateClass() As String
                    Set(ByVal value As String)
                        outTemplateClass = value
                    End Set
                    Get
                        Return outTemplateClass
                    End Get
                End Property
            End Class
        End Class
    End Class
#End Region

#Region "公開メソッド"

    ''' <summary>
    ''' 置換済みテンプレート取得処理
    ''' </summary>
    ''' <param name="xmlString">XML文字列</param>
    ''' <returns>該当のテンプレートを応答XMLとして返却する。</returns>
    ''' <remarks>
    ''' タブレット画面から受けたリクエストにより、該当のテンプレートを取得する。
    ''' 置換文字に該当する情報をDBから取得し、上記テンプレートに対して置き換え処理を行う。
    ''' </remarks>
    Public Function GetReplacedTemplate(ByVal xmlStr As String) As Response
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogUtil.GetLogParam("xmlString", xmlStr, False))

        Dim response As New Response

        Try
            Dim templateInfo As TemplateInfo = GetXmlData(xmlStr)
            Dim replaced As ReplacedTemplate = ConvertInfo(templateInfo)
            response = CreateResponseXML(replaced, CStr(ReturnCode.Successful), Message.Success)
        Catch argEx As ArgumentException
            ' エラーコードが割り当たっている場合、Dataプロパティに格納されている
            Dim code As String = If(argEx.Data.Contains(ReturnCodeKey), _
                                    CStr(argEx.Data(ReturnCodeKey)), CStr(ReturnCode.ErrCodeSys))
            Logger.Error(argEx.Message, argEx)
            response = CreateResponseXML(Nothing, code, Message.Failure)
        Catch ex As Exception
            Logger.Error(ex.Message, ex)
            response = CreateResponseXML(Nothing, CStr(ReturnCode.ErrCodeSys), Message.Failure)
        End Try

        Logger.Info(LogUtil.GetReturnParam(response.ToString))
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return response
    End Function

#End Region

#Region "非公開メソッド"

#Region "XMLからのデータ取得処理"

    ''' <summary>
    ''' 受信したXML文字列からテンプレート取得情報を取得する。
    ''' </summary>
    ''' <param name="xmlString">受信したXML文字列</param>
    ''' <returns>テンプレート取得情報を返却する。</returns>
    ''' <exception cref="ArgumentException">XMLに対するチェック結果が不正であった場合に送出する。</exception>
    Private Function GetXmlData(ByVal xmlString As String) As TemplateInfo
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogUtil.GetLogParam("xmlString", xmlString, False))

        Dim xml = XDocument.Parse(xmlString)

        Dim head = xml.Descendants(NodeName.Head).FirstOrDefault

        If head Is Nothing Then
            Throw GetArgumentException(CStr(ReturnCode.ErrCodeSys))
        End If

        GetChildNodeInfo(head, TransmissionDateNode)

        Dim requestTemplateNode = xml.Descendants(NodeName.RequestTemplate).FirstOrDefault

        If requestTemplateNode Is Nothing Then
            Throw GetArgumentException(CStr(ReturnCode.ErrCodeSys))
        End If

        Dim templateInfo As New TemplateInfo

        templateInfo.DealerCode = GetChildNodeInfo(requestTemplateNode, DealerCodeNode)
        templateInfo.StoreCode = GetChildNodeInfo(requestTemplateNode, StoreCodeNode)
        templateInfo.TemplateClass = GetChildNodeInfo(requestTemplateNode, TemplateClassNode)
        templateInfo.DisplayID = GetChildNodeInfo(requestTemplateNode, DisplayIDNode)
        templateInfo.CustomId = GetChildNodeInfo(requestTemplateNode, CustomIdNode)
        templateInfo.StaffCode = GetChildNodeInfo(requestTemplateNode, StaffCodeNode)
        templateInfo.SalesID = GetChildNodeInfo(requestTemplateNode, SalesIDNode)

        Logger.Info(LogUtil.GetReturnParam(templateInfo.ToString))
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return templateInfo
    End Function

    ''' <summary>
    ''' 指定した処理結果コードを保持したArgumentExceptionを作成する。
    ''' </summary>
    ''' <param name="returnCode">処理結果コード</param>
    ''' <returns>指定した処理結果コードを保持したArgumentExceptionを返却する。</returns>
    Private Function GetArgumentException(ByVal returnCode As String) As ArgumentException
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogUtil.GetLogParam("returnCode", returnCode, False))

        Dim argumentException As New ArgumentException(returnCode)
        argumentException.Data.Add(ReturnCodeKey, returnCode)

        Logger.Info(LogUtil.GetReturnParam(argumentException.ToString))
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return argumentException
    End Function

    ''' <summary>
    ''' 子ノードの情報を取得します
    ''' </summary>
    ''' <param name="parentsNode">親ノード</param>
    ''' <param name="nodeInfo">取得するノードの定義情報</param>
    ''' <returns>子ノードの情報</returns>
    Private Function GetChildNodeInfo(ByVal parentsNode As XElement, ByVal nodeInfo As NodeCheckInfo) As String
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogUtil.GetLogParam("parentsNode", parentsNode.Value, False) & _
                    LogUtil.GetLogParam("nodeInfo", nodeInfo.ToString, True))

        Dim childs As IEnumerable(Of XElement) = parentsNode.Descendants(nodeInfo.Name)
        Dim child As XElement = childs.FirstOrDefault()

        '子ノードが存在するか確認
        Dim childNodesCount As Integer = childs.Count

        If 1 < childNodesCount Then
            '複数個、同名の子ノードが存在するためエラー
            Throw GetArgumentException(CStr(ReturnCode.XmlIncorrect))
        ElseIf childNodesCount = 0 OrElse String.IsNullOrEmpty(child.Value) Then

            '必須項目の場合、子ノードが存在しない/子ノードが空のためエラー
            If nodeInfo.IsMandatory Then
                Dim resultCode As String = CStr(ReturnCode.NotXmlElementError + nodeInfo.ElementCode)
                Throw GetArgumentException(resultCode)
            End If

            ' 必須項目ではない場合は、いずれの場合もエラーとならない
            Logger.Info(LogUtil.GetReturnParam(String.Empty))
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
            Return String.Empty
        End If

        ' 型チェック
        If Not IsValidType(child.Value, nodeInfo.Type) Then
            Dim resultCode As String = CStr(ReturnCode.XmlParseError + nodeInfo.ElementCode)
            Throw GetArgumentException(resultCode)
        End If

        '桁チェック
        If 0 < nodeInfo.MaxLength AndAlso Not Validation.IsCorrectDigit(child.Value, nodeInfo.MaxLength) Then
            Dim resultCode As String = CStr(ReturnCode.XmlMaximumOfDigitError + nodeInfo.ElementCode)
            Throw GetArgumentException(resultCode)
        End If

        ' 値チェック
        If nodeInfo.Range.Any AndAlso Not nodeInfo.Range.Contains(child.Value) Then
            Dim resultCode As String = CStr(ReturnCode.XmlValueCheckError + nodeInfo.ElementCode)
            Throw GetArgumentException(resultCode)
        End If

        Logger.Info(LogUtil.GetReturnParam(child.Value))
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return child.Value
    End Function

    ''' <summary>
    ''' 値が指定した型として正しいかを判定する。
    ''' </summary>
    ''' <param name="target">判定対象</param>
    ''' <param name="type">指定した型</param>
    ''' <returns>正しい形式の場合は、Trueを返却する。</returns>
    Private Function IsValidType(ByVal target As String, ByVal type As TypeConversion) As Boolean
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogUtil.GetLogParam("target", target, False) & _
                    LogUtil.GetLogParam("type", CStr(type), True))

        ' 値が空の場合はこちらではチェックしない
        If String.IsNullOrEmpty(target) Then
            Return True
        End If

        Dim isCheck As Boolean = False
        Select Case type
            Case TypeConversion.None
                ' noneはチェックしない
                isCheck = True
            Case TypeConversion.StringType
                ' 元々文字列型なのでチェックの必要性はない
                isCheck = True
            Case TypeConversion.IntegerType
                isCheck = Decimal.TryParse(target, 0)
        End Select

        Logger.Info(LogUtil.GetReturnParam(CStr(isCheck)))
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return isCheck
    End Function

#End Region

#Region "置換文字列処理"

    ''' <summary>
    ''' 置換文字変換処理
    ''' </summary>
    ''' <param name="templateInfo">置換文字未置き換えのテンプレート</param>
    ''' <returns>置換文字置き換え後のテンプレートを返却する。</returns>
    ''' <remarks>
    ''' 取得したテンプレートに置換文字が存在する場合、変換処理を行う。
    ''' </remarks>
    Private Function ConvertInfo(ByVal templateInfo As TemplateInfo) As ReplacedTemplate
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogUtil.GetLogParam("templateInfo", templateInfo.ToString, False))

        Using tableAdapter As New IC3040701TableAdapters
            ' テンプレート取得
            Dim replaceResult As ReplacedTemplate = tableAdapter.GetTemplateInfo(templateInfo.DealerCode, _
                templateInfo.StoreCode, templateInfo.DisplayID, templateInfo.TemplateClass)

            ' テンプレートが見つからない場合はエラーとする
            If ReplacedTemplate.TemplateResult.NotFound.Equals(replaceResult.Result) Then
                Throw GetArgumentException(CStr(ReturnCode.TemplateNotFound))
            End If

            ' 顧客情報取得
            replaceResult = ReplaceCustomerInfo(templateInfo.CustomId, replaceResult, tableAdapter)

            ' 販売店・店舗情報取得
            If replaceResult.ContainsGroupLiteral(DlrBrnLiteral) Then
                replaceResult = ReplaceDlrBrnInfo(templateInfo.DealerCode, templateInfo.StoreCode, replaceResult, tableAdapter)
            End If

            ' スタッフ情報取得
            If replaceResult.ContainsGroupLiteral(StaffLiteral) Then
                replaceResult = ReplaceStaffInfo(templateInfo.StaffCode, replaceResult, tableAdapter)
            End If

            ' 車両関連情報取得
            If replaceResult.ContainsGroupLiteral(VehicleLiteral) Then
                replaceResult = ReplaceVehicleInfo(templateInfo.SalesID, replaceResult, tableAdapter)
            End If

            Logger.Info(LogUtil.GetReturnParam(replaceResult.ToString))
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
            Return replaceResult
        End Using
    End Function

    ''' <summary>
    ''' テンプレートのタイトル、本文を顧客情報で置換する。
    ''' </summary>
    ''' <param name="customId">顧客ID</param>
    ''' <param name="orgTemplate">テンプレート</param>
    ''' <param name="adapter">データアクセスクラス</param>
    ''' <returns>置換済みテンプレートを返却する。</returns>
    ''' <remarks>顧客IDが未指定の場合、顧客項目の置換文字列グループを空文字置換する。</remarks>
    Private Function ReplaceCustomerInfo(ByVal customId As String, ByVal orgTemplate As ReplacedTemplate, ByVal adapter As IC3040701TableAdapters) As ReplacedTemplate
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogUtil.GetLogParam("customId", customId, False) & _
                    LogUtil.GetLogParam("orgTemplate", orgTemplate.ToString, True) & _
                    LogUtil.GetLogParam("adapter", adapter.ToString, True))

        Dim replaceResult As New ReplacedTemplate With _
            {.Email = orgTemplate.Email, .Subject = orgTemplate.Subject, .Text = orgTemplate.Text, _
             .Result = orgTemplate.Result, .TemplateClass = orgTemplate.TemplateClass}

        ' 顧客IDが未指定の場合は置換文字列を空文字置換する
        If String.IsNullOrEmpty(customId) Then
            replaceResult.Email = String.Empty
            replaceResult = Replace(ReplaceLiteral.CustName, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.CustNameNum, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.Department, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.JobTitle, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.PersonInCharge, String.Empty, replaceResult)
            ' $01 start PRJ1504572_(トライ店システム評価)メールテンプレート機能強化(敬称置換文字追加)
            replaceResult = Replace(ReplaceLiteral.NameTitle, String.Empty, replaceResult)
            ' $01 end PRJ1504572_(トライ店システム評価)メールテンプレート機能強化(敬称置換文字追加)

            Logger.Info(LogUtil.GetReturnParam(replaceResult.ToString))
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
            Return replaceResult
        End If

        ' 顧客情報取得
        Using customerInfo As IC3040701DataSet.IC3040701CustomerInfoDataTable =
            adapter.GetCustomerInfo(Decimal.Parse(customId, NumberFormatInfo.InvariantInfo))

            Dim hasAnyCst As Boolean = (0 < customerInfo.Count)
            Dim email As String = If(hasAnyCst, customerInfo(0).CST_EMAIL, String.Empty)
            Dim cstName As String = If(hasAnyCst, customerInfo(0).CST_NAME, String.Empty)
            Dim department As String = If(hasAnyCst, customerInfo(0).FLEET_PIC_NAME, String.Empty)
            Dim jobtitle As String = If(hasAnyCst, customerInfo(0).FLEET_PIC_DEPT, String.Empty)
            Dim personInCharge As String = If(hasAnyCst, customerInfo(0).FLEET_PIC_POSITION, String.Empty)
            ' $01 start PRJ1504572_(トライ店システム評価)メールテンプレート機能強化(敬称置換文字追加)
            Dim nameTitle As String = If(hasAnyCst, customerInfo(0).NAMETITLE_NAME, String.Empty)
            ' $01 end PRJ1504572_(トライ店システム評価)メールテンプレート機能強化(敬称置換文字追加)

            replaceResult.Email = email
            replaceResult = Replace(ReplaceLiteral.CustName, cstName, replaceResult)
            replaceResult = Replace(ReplaceLiteral.CustNameNum, cstName, replaceResult)
            replaceResult = Replace(ReplaceLiteral.Department, department, replaceResult)
            replaceResult = Replace(ReplaceLiteral.JobTitle, jobtitle, replaceResult)
            replaceResult = Replace(ReplaceLiteral.PersonInCharge, personInCharge, replaceResult)
            ' $01 start PRJ1504572_(トライ店システム評価)メールテンプレート機能強化(敬称置換文字追加)
            replaceResult = Replace(ReplaceLiteral.NameTitle, nameTitle, replaceResult)
            ' $01 end PRJ1504572_(トライ店システム評価)メールテンプレート機能強化(敬称置換文字追加)

            Logger.Info(LogUtil.GetReturnParam(replaceResult.ToString))
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
            Return replaceResult
        End Using
    End Function

    ''' <summary>
    ''' テンプレートのタイトル、本文を販売店・店舗情報で置換する。
    ''' </summary>
    ''' <param name="dealerCode">販売店コード</param>
    ''' <param name="storeCode">店舗コード</param>
    ''' <param name="orgTemplate">テンプレート</param>
    ''' <param name="adapter">データアクセスクラス</param>
    ''' <returns>置換済みテンプレートを返却する。</returns>
    ''' <remarks>販売店/店舗コードコードが未指定の場合、スタッフ項目の置換文字列グループを空文字置換する。</remarks>
    Private Function ReplaceDlrBrnInfo(ByVal dealerCode As String, ByVal storeCode As String, _
                                       ByVal orgTemplate As ReplacedTemplate, ByVal adapter As IC3040701TableAdapters) As ReplacedTemplate
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogUtil.GetLogParam("dealerCode", dealerCode, False) & _
                    LogUtil.GetLogParam("storeCode", storeCode, True) & _
                    LogUtil.GetLogParam("orgTemplate", orgTemplate.ToString, True) & _
                    LogUtil.GetLogParam("adapter", adapter.ToString, True))

        Dim replaceResult As New ReplacedTemplate With _
            {.Email = orgTemplate.Email, .Subject = orgTemplate.Subject, .Text = orgTemplate.Text, _
             .Result = orgTemplate.Result, .TemplateClass = orgTemplate.TemplateClass}

        ' 販売店/店舗コードが未指定の場合は置換文字列を空文字置換する
        If String.IsNullOrEmpty(dealerCode) OrElse String.IsNullOrEmpty(storeCode) Then
            replaceResult = Replace(ReplaceLiteral.DlrName, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.DlrNameNum, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.DlrUrl, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.DlrUrlNum, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.BranchName, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.BranchNameNum, String.Empty, replaceResult)
            Logger.Info(LogUtil.GetReturnParam(replaceResult.ToString))
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
            Return replaceResult
        End If

        Using dlrBrnInfo As IC3040701DataSet.IC3040701DealerBranchInfoDataTable = _
            adapter.GetDealerBranchInfo(dealerCode, storeCode)

            Dim hasAny As Boolean = (0 < dlrBrnInfo.Count)
            Dim dlrName As String = If(hasAny, dlrBrnInfo(0).DLR_NAME, String.Empty)
            Dim dlrUrl As String = If(hasAny, dlrBrnInfo(0).DLR_URL, String.Empty)
            Dim brnName As String = If(hasAny, dlrBrnInfo(0).BRN_NAME, String.Empty)

            replaceResult = Replace(ReplaceLiteral.DlrName, dlrName, replaceResult)
            replaceResult = Replace(ReplaceLiteral.DlrNameNum, dlrName, replaceResult)
            replaceResult = Replace(ReplaceLiteral.DlrUrl, dlrUrl, replaceResult)
            replaceResult = Replace(ReplaceLiteral.DlrUrlNum, dlrUrl, replaceResult)
            replaceResult = Replace(ReplaceLiteral.BranchName, brnName, replaceResult)
            replaceResult = Replace(ReplaceLiteral.BranchNameNum, brnName, replaceResult)

            Logger.Info(LogUtil.GetReturnParam(replaceResult.ToString))
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
            Return replaceResult
        End Using
    End Function

    ''' <summary>
    ''' テンプレートのタイトル、本文をスタッフ情報で置換する。
    ''' </summary>
    ''' <param name="stfCd">スタッフコード</param>
    ''' <param name="orgTemplate">テンプレート</param>
    ''' <param name="adapter">データアクセスクラス</param>
    ''' <returns>置換済みテンプレートを返却する。</returns>
    ''' <remarks>スタッフコードが未指定の場合、スタッフ項目の置換文字列グループを空文字置換する。</remarks>
    Private Function ReplaceStaffInfo(ByVal stfCd As String, ByVal orgTemplate As ReplacedTemplate, ByVal adapter As IC3040701TableAdapters) As ReplacedTemplate
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogUtil.GetLogParam("stfCd", stfCd, False) & _
                    LogUtil.GetLogParam("orgTemplate", orgTemplate.ToString, True) & _
                    LogUtil.GetLogParam("adapter", adapter.ToString, True))

        Dim replaceResult As New ReplacedTemplate With _
            {.Email = orgTemplate.Email, .Subject = orgTemplate.Subject, .Text = orgTemplate.Text, _
             .Result = orgTemplate.Result, .TemplateClass = orgTemplate.TemplateClass}

        ' スタッフコードが未指定の場合は置換文字列を空文字置換する
        If String.IsNullOrEmpty(stfCd) Then
            replaceResult = Replace(ReplaceLiteral.StaffName, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.StaffNameNum, String.Empty, replaceResult)
            Logger.Info(LogUtil.GetReturnParam(replaceResult.ToString))
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
            Return replaceResult
        End If

        ' スタッフ情報取得
        Using staffInfo As IC3040701DataSet.IC3040701StaffInfoDataTable = _
            adapter.GetStaffInfo(stfCd)

            Dim hasAny As Boolean = (0 < staffInfo.Count)
            Dim stfName As String = If(hasAny, staffInfo(0).STF_NAME, String.Empty)

            replaceResult = Replace(ReplaceLiteral.StaffName, stfName, replaceResult)
            replaceResult = Replace(ReplaceLiteral.StaffNameNum, stfName, replaceResult)

            Logger.Info(LogUtil.GetReturnParam(replaceResult.ToString))
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
            Return replaceResult
        End Using
    End Function

    ''' <summary>
    ''' テンプレートのタイトル、本文を車両関連情報で置換する。
    ''' </summary>
    ''' <param name="salesId">商談ID</param>
    ''' <param name="orgTemplate">テンプレート</param>
    ''' <param name="adapter">データアクセスクラス</param>
    ''' <returns>置換済みテンプレートを返却する。</returns>
    ''' <remarks>商談IDが未指定の場合、車両関連情報の置換文字列グループを空文字置換する。</remarks>
    Private Function ReplaceVehicleInfo(ByVal salesId As String, ByVal orgTemplate As ReplacedTemplate, ByVal adapter As IC3040701TableAdapters) As ReplacedTemplate
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogUtil.GetLogParam("salesId", salesId, False) & _
                    LogUtil.GetLogParam("orgTemplate", orgTemplate.ToString, True) & _
                    LogUtil.GetLogParam("adapter", adapter.ToString, True))

        Dim replaceResult As New ReplacedTemplate With _
            {.Email = orgTemplate.Email, .Subject = orgTemplate.Subject, .Text = orgTemplate.Text, _
             .Result = orgTemplate.Result, .TemplateClass = orgTemplate.TemplateClass}

        ' 商談IDが未指定の場合は置換文字列を空文字置換する
        If String.IsNullOrEmpty(salesId) Then
            replaceResult = Replace(ReplaceLiteral.SeriesName, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.SeriesNameNum, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.CRDate, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.CRDateNum, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.ServiceName, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.ServiceNameNum, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.MakerName, String.Empty, replaceResult)
            replaceResult = Replace(ReplaceLiteral.MakerNameNum, String.Empty, replaceResult)

            Logger.Info(LogUtil.GetReturnParam(replaceResult.ToString))
            Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
            Return replaceResult
        End If

        Dim vehicleInfo As VehicleInfo = adapter.GetVehicleInfo(Decimal.Parse(salesId, NumberFormatInfo.InvariantInfo))
        replaceResult = Replace(ReplaceLiteral.SeriesName, vehicleInfo.SeriesName, replaceResult)
        replaceResult = Replace(ReplaceLiteral.SeriesNameNum, vehicleInfo.SeriesName, replaceResult)
        replaceResult = Replace(ReplaceLiteral.CRDate, vehicleInfo.CRDate, replaceResult)
        replaceResult = Replace(ReplaceLiteral.CRDateNum, vehicleInfo.CRDate, replaceResult)
        replaceResult = Replace(ReplaceLiteral.ServiceName, vehicleInfo.ServiceName, replaceResult)
        replaceResult = Replace(ReplaceLiteral.ServiceNameNum, vehicleInfo.ServiceName, replaceResult)
        replaceResult = Replace(ReplaceLiteral.MakerName, vehicleInfo.MakerName, replaceResult)
        replaceResult = Replace(ReplaceLiteral.MakerNameNum, vehicleInfo.MakerName, replaceResult)

        Logger.Info(LogUtil.GetReturnParam(replaceResult.ToString))
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return replaceResult
    End Function

    ''' <summary>
    ''' 対象のテンプレート本文・件名中のリテラルを指定した文字列で置き換える。
    ''' </summary>
    ''' <param name="literal">置換対象リテラル</param>
    ''' <param name="value">置換文字列</param>
    ''' <param name="template">対象のテンプレート</param>
    ''' <returns>本文・件名置換済みのテンプレートを返却する。</returns>
    ''' <remarks></remarks>
    Private Function Replace(ByVal literal As String, ByVal value As String, ByVal template As ReplacedTemplate) As ReplacedTemplate
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogUtil.GetLogParam("literal", literal, False) & _
                LogUtil.GetLogParam("value", value, True) & _
                LogUtil.GetLogParam("template", template.ToString, True))

        Dim replaced As New ReplacedTemplate With _
            {.Email = template.Email, .Subject = template.Subject, .Text = template.Text, _
             .Result = template.Result, .TemplateClass = template.TemplateClass}

        Dim replaceValue As String = If(String.IsNullOrEmpty(value), String.Empty, value)

        If Not String.IsNullOrEmpty(template.Subject) Then
            replaced.Subject = template.Subject.Replace(literal, replaceValue)
        End If

        If Not String.IsNullOrEmpty(template.Text) Then
            replaced.Text = template.Text.Replace(literal, replaceValue)
        End If

        Logger.Info(LogUtil.GetReturnParam(replaced.ToString))
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return replaced
    End Function
#End Region

#Region "応答用XML生成処理"
    ''' <summary>
    ''' 応答用XML生成処理
    ''' </summary>
    ''' <returns>応答XMLを返却する。</returns>
    ''' <remarks>
    ''' タブレット画面への応答用XMLを生成する。　(正常系・異常系)
    ''' </remarks>
    Private Function CreateResponseXML(ByVal replacedTemplate As ReplacedTemplate, _
                                       ByVal resultReturnCode As String, ByVal message As String) As Response
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, True))
        Logger.Info(LogUtil.GetLogParam("replacedTemplate", If(replacedTemplate Is Nothing, "Null", replacedTemplate.ToString), False) & _
                LogUtil.GetLogParam("resultReturnCode", resultReturnCode, True) & _
                LogUtil.GetLogParam("message", message, True))

        ' Responseクラス生成
        Dim returnXml As New Response

        ' Headerクラスに値をセット
        Dim createRespHead As New Response.RootHead
        createRespHead.TransmissionDate = DateTimeFunc.FormatDate(1, Date.Now())

        ' Detailクラス生成
        Dim createRespDetail As New Response.RootDetail

        ' Commonクラスに値をセット
        Dim createRespCommon As New Response.RootDetail.DetailCommon

        If replacedTemplate IsNot Nothing Then
            createRespCommon.EmailAddress = If(String.IsNullOrWhiteSpace(replacedTemplate.Email), String.Empty, replacedTemplate.Email)
            createRespCommon.TemplateSubject = If(String.IsNullOrWhiteSpace(replacedTemplate.Subject), String.Empty, replacedTemplate.Subject)
            createRespCommon.TemplateText = If(String.IsNullOrWhiteSpace(replacedTemplate.Text), String.Empty, replacedTemplate.Text)
            createRespCommon.TemplateClass = If(String.IsNullOrWhiteSpace(replacedTemplate.TemplateClass), String.Empty, replacedTemplate.TemplateClass)
        Else
            createRespCommon.EmailAddress = String.Empty
            createRespCommon.TemplateSubject = String.Empty
            createRespCommon.TemplateText = String.Empty
            createRespCommon.TemplateClass = String.Empty
        End If

        createRespCommon.Message = message
        createRespCommon.ReturnCode = resultReturnCode

        'Commonにセットした値をDetailに反映
        createRespDetail.Common = createRespCommon

        'Header、Detailにセットした値をResponseに反映
        returnXml.Head = createRespHead
        returnXml.Detail = createRespDetail

        'ログ出力
        Using writer As New StringWriter(CultureInfo.InvariantCulture())
            Dim outXml As New XmlSerializer(GetType(Response))
            outXml.Serialize(writer, returnXml)

            If CStr(ReturnCode.Successful).Equals(resultReturnCode) Then
                '成功
                Logger.Info(writer.ToString)
            Else
                '失敗
                Logger.Error(writer.ToString)
            End If
        End Using

        Logger.Info(LogUtil.GetReturnParam(returnXml.ToString))
        Logger.Info(LogUtil.GetLogMethod(System.Reflection.MethodBase.GetCurrentMethod.Name, False))
        Return returnXml
    End Function

#End Region

#Region "IDisposable Support"
    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        ' 開放が必要なリソースはないため、空実装とする
    End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#End Region

End Class
