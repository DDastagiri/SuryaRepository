'━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
'ISC3100101BusinessLogic.vb
'──────────────────────────────────
'機能： 受付メイン
'補足： 
'作成： 2011/12/12 KN t.mizumoto
'更新： 2012/08/23 TMEJ m.okamura 新車受付機能改善 $01
'更新： 2013/01/09 TMEJ t.shimamura 新車タブレットショールーム管理機能開発 $02
'更新： 2020/03/12 NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060) $03
'──────────────────────────────────

''' <summary>
''' SC3100101
''' メインメニューの重要事項に表示する内容を登録する画面のビジネスロジック用インターフェース
''' コミット行うメソッドを定義します。
''' </summary>
''' <remarks></remarks>
Public Interface ISC3100101BusinessLogic

    '$03 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ' ''' <summary>
    ' ''' お客様氏名・商談テーブルの登録
    ' ''' </summary>
    ' ''' <param name="visitSequence">来店実績連番</param>
    ' ''' <param name="customerSegment">顧客区分</param>
    ' ''' <param name="tentativeName">仮登録氏名</param>
    ' ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ' ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ' ''' <param name="updateAccount">更新アカウント</param>
    ' ''' <param name="isCustomerNameEdit">仮登録氏名登録フラグ</param>
    ' ''' <returns>処理結果</returns>
    ' ''' <remarks></remarks>
    'Function RegistrationNameAndSalesTable(ByVal visitSequence As Long, ByVal customerSegment As String, _
    '                                       ByVal tentativeName As String, ByVal oldSalesTableNo As Integer, _
    '                                       ByVal newSalesTableNo As Integer, ByVal updateAccount As String, _
    '                              Optional ByVal isCustomerNameEdit As Boolean = True) As Integer
    ''' <summary>
    ''' 来店情報の削除
    ''' </summary>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Function DeleteVisitorRecord(ByVal visitSequence As Long, ByVal updateAccount As String) As Integer

    ''' <summary>
    ''' お客様氏名・商談テーブルの登録
    ''' </summary>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="tentativeName">仮登録氏名</param>
    ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="telNumber">電話番号</param>
    ''' <param name="isCustomerNameEdit">仮登録氏名登録フラグ</param>
    ''' <returns>処理結果</returns>
    ''' <remarks></remarks>
    Function RegistrationNameAndSalesTable(ByVal visitSequence As Long, ByVal customerSegment As String, _
                                           ByVal tentativeName As String, ByVal oldSalesTableNo As Integer, _
                                           ByVal newSalesTableNo As Integer, ByVal updateAccount As String, _
                                           ByVal telNumber As String, _
                                  Optional ByVal isCustomerNameEdit As Boolean = True) As Integer

    '$03 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

    '$03 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ' ''' <summary>
    ' ''' 依頼通知のブロードキャスト
    ' ''' </summary>
    ' ''' <param name="visitSequence">来店実績連番</param>
    ' ''' <param name="customerSegment">顧客区分</param>
    ' ''' <param name="tentativeName">仮登録氏名</param>
    ' ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ' ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ' ''' <param name="vehicleNo">車両登録No.</param>
    ' ''' <param name="standbyStaffList">スタンバイスタッフリスト</param>
    ' ''' <param name="updateAccount">更新アカウント</param>
    ' ''' <returns>メッセージID</returns>
    ' ''' <remarks></remarks>
    'Function RequestNoticeBroadcast(ByVal visitSequence As Long, ByVal customerSegment As String, _
    '                                ByVal tentativeName As String, ByVal oldSalesTableNo As Integer, _
    '                                ByVal newSalesTableNo As Integer, ByVal vehicleNo As String, _
    '                                ByVal standbyStaffList As List(Of String), ByVal updateAccount As String) As Integer
    ''' <summary>
    ''' 依頼通知のブロードキャスト
    ''' </summary>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="tentativeName">仮登録氏名</param>
    ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ''' <param name="vehicleNo">車両登録No.</param>
    ''' <param name="standbyStaffList">スタンバイスタッフリスト</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <param name="telNumber">電話番号</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Function RequestNoticeBroadcast(ByVal visitSequence As Long, ByVal customerSegment As String, _
                                    ByVal tentativeName As String, ByVal oldSalesTableNo As Integer, _
                                    ByVal newSalesTableNo As Integer, ByVal vehicleNo As String, _
                                    ByVal standbyStaffList As List(Of String), ByVal updateAccount As String, _
                                    ByVal telNumber As String) As Integer
    '$03 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)

    ''' <summary>
    ''' SC割り当て処理
    ''' </summary>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <param name="dealAccount">対応アカウント</param>
    ''' <param name="updateAccount">更新アカウント</param>
    ''' <returns>メッセージID</returns>
    ''' <remarks></remarks>
    Function SalesConsultantAssignment(ByVal visitSequence As Long, ByVal dealAccount As String, _
                                       ByVal updateAccount As String) As Integer

    ' $01 start 複数顧客に対する商談平行対応
    ' ''' <summary>
    ' ''' 紐付け解除更新
    ' ''' </summary>
    ' ''' <param name="visitSeqList">来店実績連番リスト</param>
    ' ''' <param name="dealAccount">対応アカウント</param>
    ' ''' <param name="updateAccount">対応アカウント</param>
    ' ''' <returns>メッセージID</returns>
    ' ''' <remarks></remarks>
    'Function LinkingCancel(ByVal visitSeqList As List(Of String), _
    '                              ByVal dealAccount As String, _
    '                              ByVal updateAccount As String) As Integer
    ' $01 end   複数顧客に対する商談平行対応

    ' $02 start 新車タブレットショールーム管理機能開発
    '$03 start NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ' ''' <summary>
    ' ''' 接客不要情報を登録
    ' ''' </summary>
    ' ''' <param name="visitSequence">来店実績連番</param>
    ' ''' <param name="customerSegment">顧客区分</param>
    ' ''' <param name="tentativeName">仮登録氏名</param>
    ' ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ' ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ' ''' <param name="updateAccount">ユーザーアカウント</param>
    ' ''' <returns>登録結果</returns>
    ' ''' <remarks></remarks>
    'Function RegistrationUnNecessary(ByVal visitSequence As Long, ByVal customerSegment As String, _
    '                                 ByVal tentativeName As String, ByVal oldSalesTableNo As Integer, _
    '                                 ByVal newSalesTableNo As Integer, ByVal updateAccount As String) As Integer
    ''' <summary>
    ''' 接客不要情報を登録
    ''' </summary>
    ''' <param name="visitSequence">来店実績連番</param>
    ''' <param name="customerSegment">顧客区分</param>
    ''' <param name="tentativeName">仮登録氏名</param>
    ''' <param name="oldSalesTableNo">更新前商談テーブルNo.</param>
    ''' <param name="newSalesTableNo">更新後商談テーブルNo.</param>
    ''' <param name="updateAccount">ユーザーアカウント</param>
    ''' <param name="telNumber">電話番号</param>
    ''' <returns>登録結果</returns>
    ''' <remarks></remarks>
    Function RegistrationUnNecessary(ByVal visitSequence As Long, ByVal customerSegment As String, _
                                     ByVal tentativeName As String, ByVal oldSalesTableNo As Integer, _
                                     ByVal newSalesTableNo As Integer, ByVal updateAccount As String, _
                                     ByVal telNumber As String) As Integer
    '$03 end NSK s.natsume TKM Change request development for Next Gen e-CRB (CR060)
    ' $02 end 新車タブレットショールーム管理機能開発
End Interface
