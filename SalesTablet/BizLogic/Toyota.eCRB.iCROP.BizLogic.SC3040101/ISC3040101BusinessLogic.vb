Imports Toyota.eCRB.Tool.Message.DataAccess

''' <summary>
''' SC3040101<br/>
''' メインメニューの重要事項に表示する内容を登録する画面のビジネスロジック用インターフェース<br/>
''' コミット行うメソッドを定義します。
''' </summary>
''' <remarks></remarks>
Public Interface ISC3040101BusinessLogic

    ''' <summary>
    ''' 連絡事項登録処理
    ''' </summary>
    ''' <param name="insMessageDataTable">連絡事項登録データテーブル</param>
    ''' <returns>成功:True/失敗:False</returns>
    ''' <remarks>本メソッドでは、以下のメソッドも利用しているため、例外情報に関しては、下記、メソッドを参照してください。</remarks>
    ''' <seealso>InsertMessages</seealso>
    Function InsertPost(ByVal insMessageDataTable As SC3040101DataSet.SC3040101MessageInfoDataTable) As Boolean

End Interface
