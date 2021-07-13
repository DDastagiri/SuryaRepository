Imports Toyota.eCRB.SystemFrameworks.Core.iCROP.DataAccess

Namespace Toyota.eCRB.SystemFrameworks.Core.iCROP.BizLogic

    ''' <summary>
    ''' 文言の取得および管理を行うクラスです。
    ''' </summary>
    ''' <remarks></remarks>
    Friend Class WordResourceManager

        ''' <summary>
        ''' 文言のTITLEフラグ
        ''' </summary>
        ''' <remarks></remarks>
        Private Const KINDFLG_ON As String = "1"

        ''' <summary>
        ''' コードテーブル取得時の排他用に使用するオブジェクト
        ''' </summary>
        Private Shared _lockGetCodeTables As Object = New Object()

        ''' <summary>
        ''' 文言管理
        ''' KEY:画面ID VALUE(KEY:販売店CD VALUE:(KEY:文言NO VALUE:文言)))の連想配列
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _wordTable As Dictionary(Of String, Dictionary(Of String, Dictionary(Of Decimal, String)))
        ''' <summary>
        ''' 画面タイトル管理
        ''' KEY:画面ID VALUE(KEY:販売店CD VALUE:画面タイトル)の連想配列
        ''' </summary>
        ''' <remarks></remarks>
        Private Shared _titleTable As Dictionary(Of String, Dictionary(Of String, String))

        ''' <summary>
        ''' インスタンスの生成をできないようにするためのデフォルトのコンストラクタです。
        ''' </summary>
        ''' <remarks>
        ''' このクラスはインスタンスを生成できません。静的メソッドを呼び出してください。
        ''' </remarks>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' 引数で指定された画面ID、販売店CD、文言Noに該当する文言を取得します。
        ''' </summary>
        ''' <param name="displayID">画面ID</param>
        ''' <param name="dlrCd">販売店CD</param>
        ''' <param name="wordNo">文言No</param>
        ''' <returns>
        ''' 該当する文言が存在した場合、その文言。
        ''' 存在しなかった場合はString.Emptyを返却します。
        ''' </returns>
        ''' <remarks></remarks>
        Friend Shared Function GetWordData(ByVal displayID As String, ByVal dlrCd As String, ByVal wordNo As Decimal) As String


            '読み込みチェックチェック
            CheckLoad(displayID)

            Dim strWord As String = String.Empty
            Dim blnWordFound As Boolean = False

            '画面ID存在チェック
            If _wordTable.ContainsKey(displayID) Then

                '指定販売店の文言が登録されているかチェック
                If Not String.IsNullOrEmpty(dlrCd) AndAlso _wordTable(displayID).ContainsKey(dlrCd) Then

                    '指定販売店の、指定文言Noが登録されているかチェック
                    If _wordTable(displayID)(dlrCd).ContainsKey(wordNo) Then
                        '指定販売店の文言が登録されているので、格納
                        strWord = _wordTable(displayID)(dlrCd)(wordNo)
                        blnWordFound = True
                    End If

                End If

                '共通販売店の文言が登録されているかチェック
                If Not blnWordFound And _wordTable(displayID).ContainsKey(ConstantDealerCD.AllDealerCD) Then

                    '共通販売店の、指定文言Noが登録されているかチェック
                    If _wordTable(displayID)(ConstantDealerCD.AllDealerCD).ContainsKey(wordNo) Then
                        '共通販売店の文言が登録されているので、格納
                        strWord = _wordTable(displayID)(ConstantDealerCD.AllDealerCD)(wordNo)
                    End If

                End If

            End If

            Return strWord
        End Function

        ''' <summary>
        ''' 引数で指定された画面ID、販売店コードに該当する画面タイトル文言を取得します。
        ''' </summary>
        ''' <param name="displayID">画面ID</param>
        ''' <param name="dlrCd">販売店CD</param>
        ''' 該当する画面タイトルが存在した場合、その文言。
        ''' 存在しなかった場合はString.Emptyを返却します。
        ''' <remarks></remarks>
        Friend Shared Function GetTitleData(ByVal displayID As String, ByVal dlrCd As String) As String

            '読み込みチェックチェック
            CheckLoad(displayID)

            Dim strTitle As String = String.Empty
            Dim blnTitleFound As Boolean = False

            '画面ID存在チェック
            If _wordTable.ContainsKey(displayID) Then

                '指定販売店のタイトルが登録されているかチェック
                If Not String.IsNullOrEmpty(dlrCd) And _titleTable(displayID).ContainsKey(dlrCd) Then
                    strTitle = _titleTable(displayID)(dlrCd)
                    blnTitleFound = True
                End If

                '共通販売店の文言が登録されているかチェック
                If Not blnTitleFound And _titleTable(displayID).ContainsKey(ConstantDealerCD.AllDealerCD) Then
                    strTitle = _titleTable(displayID)(ConstantDealerCD.AllDealerCD)
                    blnTitleFound = True
                End If

            End If

            Return strTitle

        End Function


        ''' <summary>
        ''' 文言管理テーブルより共通文言、販売店別文言を全て読み込み、メモリ上に格納します。
        ''' </summary>
        ''' <remarks>
        ''' このメソッドはWebアプリケーション開始時に呼び出されることを想定しています。<br/>
        ''' よってそれ以外の用途では呼び出さないで下さい。
        ''' </remarks>
        Friend Shared Sub LoadWord()

            '文言格納テーブル
            Dim dtAll As IcropWordDataSet.IcropWordTableDataTable = Nothing
            Dim dtDlr As IcropWordDataSet.IcropWordTableDataTable = Nothing

            '排他処理を行う
            SyncLock WordResourceManager._lockGetCodeTables

                '初期化
                _wordTable = New Dictionary(Of String, Dictionary(Of String, Dictionary(Of Decimal, String)))
                _titleTable = New Dictionary(Of String, Dictionary(Of String, String))

                '共通文言取得
                dtAll = IcropWordTableAdapter.GetDefaultWordTable

                For Each dr As IcropWordDataSet.IcropWordTableRow In dtAll.Rows

                    '画面IDキーチェック
                    If Not WordResourceManager._wordTable.ContainsKey(dr.DISPLAYID) Then
                        '追加
                        _wordTable.Add(dr.DISPLAYID, New Dictionary(Of String, Dictionary(Of Decimal, String)))
                        '共通販売店追加
                        _wordTable(dr.DISPLAYID).Add(ConstantDealerCD.AllDealerCD, New Dictionary(Of Decimal, String))
                    End If

                    '文言Ｎｏをキー、文言を値として配列に登録
                    _wordTable(dr.DISPLAYID)(ConstantDealerCD.AllDealerCD).Add(CType(dr.DISPLAYNO, Decimal), dr.WORD)

                    'タイトル判定
                    If dr.KINDFLG.Equals(KINDFLG_ON) Then

                        '画面IDキーチェック
                        If Not WordResourceManager._titleTable.ContainsKey(dr.DISPLAYID) Then
                            '追加
                            WordResourceManager._titleTable.Add(dr.DISPLAYID, New Dictionary(Of String, String))
                            '共通販売店追加
                            _titleTable(dr.DISPLAYID).Add(ConstantDealerCD.AllDealerCD, String.Empty)
                        End If

                        'タイトル設定
                        _titleTable(dr.DISPLAYID)(ConstantDealerCD.AllDealerCD) = dr.WORD

                    End If

                Next

                '販売店別文言の取得
                dtDlr = IcropWordTableAdapter.GetDealerWordTable

                For Each dr As IcropWordDataSet.IcropWordTableRow In dtDlr.Rows

                    '画面IDキーチェック
                    If Not WordResourceManager._wordTable.ContainsKey(dr.DISPLAYID) Then
                        '追加
                        WordResourceManager._wordTable.Add(dr.DISPLAYID, New Dictionary(Of String, Dictionary(Of Decimal, String)))
                    End If

                    '販売店コードのキーチェック
                    If Not _wordTable(dr.DISPLAYID).ContainsKey(dr.DLRCD) Then
                        '販売店コード追加
                        _wordTable(dr.DISPLAYID).Add(dr.DLRCD, New Dictionary(Of Decimal, String))
                    End If

                    '文言Ｎｏをキー、文言を値として配列に登録
                    _wordTable(dr.DISPLAYID)(dr.DLRCD).Add(CType(dr.DISPLAYNO, Decimal), dr.WORD)

                    'タイトル判定
                    If dr.KINDFLG.Equals(KINDFLG_ON) Then

                        '画面IDキーチェック
                        If Not WordResourceManager._titleTable.ContainsKey(dr.DISPLAYID) Then
                            '追加
                            WordResourceManager._titleTable.Add(dr.DISPLAYID, New Dictionary(Of String, String))
                        End If

                        '販売店コードのキーチェック
                        If Not _titleTable(dr.DISPLAYID).ContainsKey(dr.DLRCD) Then
                            '追加
                            _titleTable(dr.DISPLAYID)(dr.DLRCD) = dr.WORD
                        End If

                        'タイトル設定
                        _titleTable(dr.DISPLAYID)(dr.DLRCD) = dr.WORD

                    End If

                Next

                '開放
                dtAll.Dispose()
                dtDlr.Dispose()
                dtAll = Nothing
                dtDlr = Nothing

            End SyncLock

        End Sub

        ''' <summary>
        ''' 引数で指定された画面ＩＤの文言がメモリ上に読み込まれているかチェックし、
        ''' 読み込まれていない場合、メモリー上にロードします。
        ''' </summary>
        ''' <param name="displayID"></param>
        ''' <remarks></remarks>
        Private Shared Sub CheckLoad(ByVal displayID As String)

            '排他処理を行う
            SyncLock WordResourceManager._lockGetCodeTables

                'インスタンス未生成の場合は、作成する
                If _wordTable Is Nothing Then
                    '文言管理
                    _wordTable = New Dictionary(Of String, Dictionary(Of String, Dictionary(Of Decimal, String)))
                    '画面タイトル管理
                    _titleTable = New Dictionary(Of String, Dictionary(Of String, String))
                End If

                '既に読み込み済みかチェックする
                If Not WordResourceManager._wordTable.ContainsKey(displayID) Then
                    '指定画面IDのみ読み込み
                    Dim wordDict As New Dictionary(Of String, Dictionary(Of Decimal, String))
                    Dim titleDict As New Dictionary(Of String, String)
                    '文言読み込み
                    LoadWordOneDisplay(displayID, wordDict, titleDict)
                    '文言配列に設定
                    WordResourceManager._wordTable.Add(displayID, wordDict)
                    'タイトル配列に設定
                    WordResourceManager._titleTable.Add(displayID, titleDict)
                End If

            End SyncLock

        End Sub

        ''' <summary>
        ''' １画面文の文言情報をＤＢから取得します。
        ''' </summary>
        ''' <param name="displayID">画面ID</param>
        ''' <remarks>
        ''' 想定外の理由でメモリ上から文言情報が消えた場合に、緊急回避として指定画面ＩＤの文言情報を再読み込みします。
        ''' </remarks>
        Private Shared Sub LoadWordOneDisplay(ByVal displayID As String _
                                            , ByVal wordDict As Dictionary(Of String, Dictionary(Of Decimal, String)) _
                                            , ByVal titleDict As Dictionary(Of String, String))

            '文言格納テーブル
            Dim dtAll As IcropWordDataSet.IcropWordTableDataTable = Nothing
            Dim dtDlr As IcropWordDataSet.IcropWordTableDataTable = Nothing

            '引数画面IDに該当する文言情報をＤＢから取得
            dtAll = IcropWordTableAdapter.GetDefaultWordTableByDisplayId(displayID)

            '共通販売店の配列生成
            wordDict.Add(ConstantDealerCD.AllDealerCD, New Dictionary(Of Decimal, String))
            titleDict.Add(ConstantDealerCD.AllDealerCD, String.Empty)

            '文言Ｎｏをキー、文言を値として配列に登録
            For Each dr As IcropWordDataSet.IcropWordTableRow In dtAll.Rows
                wordDict(ConstantDealerCD.AllDealerCD).Add(CType(dr.DISPLAYNO, Decimal), dr.WORD)
                'タイトル判定
                If dr.KINDFLG.Equals(KINDFLG_ON) Then
                    titleDict(ConstantDealerCD.AllDealerCD) = dr.WORD
                End If
            Next

            '販売店別文言の取得
            dtDlr = IcropWordTableAdapter.GetDealerWordTableByDisplayId(displayID)

            '販売店別に文言Ｎｏをキー、文言を値として配列に登録
            For Each dr As IcropWordDataSet.IcropWordTableRow In dtDlr.Rows

                '販売店ＣＤキーチェック
                If Not wordDict.ContainsKey(dr.DLRCD) Then
                    wordDict.Add(dr.DLRCD, New Dictionary(Of Decimal, String))
                End If

                '文言Ｎｏをキー、文言を値として配列に登録
                wordDict(dr.DLRCD).Add(CType(dr.DISPLAYNO, Decimal), dr.WORD)

                'タイトル判定
                If dr.KINDFLG.Equals(KINDFLG_ON) Then

                    If Not titleDict.ContainsKey(dr.DLRCD) Then
                        titleDict.Add(dr.DLRCD, String.Empty)
                    End If
                    'タイトル設定
                    titleDict(dr.DLRCD) = dr.WORD
                End If

            Next

            '開放
            dtAll.Dispose()
            dtDlr.Dispose()
            dtAll = Nothing
            dtDlr = Nothing

        End Sub

    End Class
End Namespace