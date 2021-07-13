''' <summary>
''' TCV機能設定 定数クラス
''' </summary>
''' <remarks></remarks>
Public NotInheritable Class TcvSettingConstants

#Region " コンストラクタ "
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        REM
    End Sub
#End Region

    ''' <summary>
    ''' tcv_web.jsonパス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const TcvWebJsonPath As String = "[Series]\common\data\tcv_web.json"

    ''' <summary>
    ''' car_lineup.jsonパス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CarLineupJsonPath As String = "common\data\car_lineup.json"

    ''' <summary>
    ''' sales_point.jsonパス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalesPointJsonPath As String = "[Series]\introduction\data\sales_point.json"

    ''' <summary>
    ''' interior.jsonパス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const InteriorJsonPath As String = "[Series]\introduction\data\interior.json"

    ''' <summary>
    ''' セールスポイント画像アップロードパス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalespointUploadPath As String = "[Series]\introduction\images\salespoint\"

    ''' <summary>
    ''' JSON登録用文字列(オーバーレイ)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalespointIntroductionPath As String = "introduction/"

    ''' <summary>
    ''' JSON登録用文字列(オーバーレイ)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalespointOverviewPath As String = "images/salespoint/"

    ''' <summary>
    ''' JSON登録用文字列(ポップアップ)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalespointPopupPath As String = "/introduction/images/salespoint/"

    ''' <summary>
    ''' JSON登録用文字列(フルスクリーンポップアップ)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalespointFullscreenPath As String = "/introduction/images/salespoint/"

    ''' <summary>
    ''' セールスポイントエクステリアベース画像パス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalespointExteriorImagePath As String = "[Series]/introduction/images/display/704x440/assy/"

    ''' <summary>
    ''' セールスポイントインテリアベース画像パス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalespointInteriorImagePath As String = "[Series]/introduction/"

    ''' <summary>
    ''' セールスポイント画像パス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SalespointImagePath As String = "[Series]/introduction/images/salespoint/"

    ''' <summary>
    ''' footer.jsonパス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ContentsMenuJsonPath As String = "[Series]\common\data\footer.json"

    ''' <summary>
    ''' アイコン画像パス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ContentsMenuImagePath As String = "[Series]/common/images/footer/"

    ''' <summary>
    ''' アイコン画像アップロードパス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ContentsMenuUploadPath As String = "[Series]\common\images\footer\"

    ''' <summary>
    ''' JSON登録用文字列(アイコン画像)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ContentsMenuPath As String = "../../[Series]/common/images/footer/"

    ''' <summary>
    ''' recommend.jsonパス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RecommendJsonPath As String = "[Series]\introduction\data\recommend.json"

    ''' <summary>
    ''' tcv_dealer.jsonパス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const TcvDealerJsonPath As String = "dealer\[Series]\[Dealer]\tcv_dealer.json"

    ''' <summary>
    ''' tcv_dealer.jsonパス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const TcvDealerJsonDirecty As String = "dealer\[Series]\[Dealer]\"

    ''' <summary>
    ''' ディーラーオプション画像パス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DealerOptionImagePath As String = "dealer/[Series]/[Dealer]/introduction/images/button/introduction/parts/dop/ex/"

    ''' <summary>
    ''' ディーラーオプション画像アップロードパス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DealerOptionImageUploadPath As String = "dealer\[Series]\[Dealer]\introduction\images\button\introduction\parts\dop\ex\"

    ''' <summary>
    ''' JSON登録用文字列(ディーラーオプション)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DealerOptionPath As String = "dop/ex/"

    ''' <summary>
    ''' メーカーオプション画像パス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const MakerOptionImagePath As String = "[Series]/introduction/images/button/introduction/parts/ex/"

End Class
