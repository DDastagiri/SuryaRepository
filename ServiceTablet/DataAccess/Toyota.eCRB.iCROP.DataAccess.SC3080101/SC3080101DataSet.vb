Imports System.Text
Imports Oracle.DataAccess.Client
Imports Toyota.eCRB.SystemFrameworks.Core

Namespace SC3080101DataSetTableAdapters

    Public Class SC3080101DataTableTableAdapter
        Inherits Global.System.ComponentModel.Component

        ''' <summary>
        ''' 検索条件の種類
        ''' </summary>
        ''' <remarks></remarks>
        Public Const IdSerchVclregno As Integer = 1    '車両登録No
        Public Const IdSerchName As Integer = 2        '顧客名称
        Public Const IdSerchVin As Integer = 3         'VIN
        Public Const IdSerchTel As Integer = 4         '電話番号/携帯番号

        ''' <summary>
        ''' ソート条件の種類
        ''' </summary>
        ''' <remarks></remarks>
        Public Const IdSortName As Integer = 1          '顧客名称
        Public Const IdSortModel As Integer = 2         'モデル名称
        Public Const IdSortSsusername As Integer = 3    'セールススタッフ名称
        Public Const IdSortSausername As Integer = 4    'サービスアドバイザー名称

        ''' <summary>
        ''' ソート方向(昇順)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const IdOrderAsc As Integer = 1

        ''' <summary>
        ''' ソート方向(降順)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const IdOrderDesc As Integer = 2

        ''' <summary>
        ''' 検索方向 (1:前方一致)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const IdSerchdirectionAfter As Integer = 1

        ''' <summary>
        ''' 検索方向 (2:あいまい検索)
        ''' </summary>
        ''' <remarks></remarks>
        Public Const IdSerchdirectionAll As Integer = 2

        ''' <summary>
        ''' 顧客件数取得
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="strcd">店舗コード</param>
        ''' <param name="serchDirection">検索方向</param>
        ''' <param name="serchType">検索タイプ</param>
        ''' <param name="serchValue">検索テキスト</param>
        ''' <returns>顧客件数</returns>
        ''' <remarks></remarks>
        Public Shared Function GetCountCustomer(ByVal dlrcd As String, _
                                         ByVal strcd As String, _
                                         ByVal serchDirection As Integer, _
                                         ByVal serchType As Integer, _
                                         ByVal serchValue As String) As Integer


            If (serchType < 0) Then
                Return Nothing
            End If

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080101_001 */ ")
                .Append("    SUM(CNT) AS CNT ")
                .Append("FROM ")
                .Append("    ( ")
                .Append("    /* 自社客 */ ")
                .Append("    SELECT ")
                .Append("        COUNT(1) AS CNT ")
                .Append("    FROM ")
                .Append("        TBLORG_CUSTOMER A, ")
                .Append("        TBLORG_VCLINFO B, ")
                .Append("        TBLORG_BRANCHINFO C ")
                .Append("    WHERE ")

                If (serchDirection = IdSerchdirectionAfter) Then
                    '前方一致
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        UPPER(A.NAME) like :NAME || '%' AND ")
                        Case IdSerchVin       'VIN
                            .Append("        UPPER(B.VIN) like :VIN || '%' AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        UPPER(B.VCLREGNO) like :VCLREGNO || '%' AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            'REM .Append("        (A.TELNO like :TELNO || '%' OR A.MOBILE like :TELNO || '%') AND ")
                            .Append("    (REPLACE(A.TELNO, '-', '' ) like :TELNO || '%' ")
                            .Append(" OR  REPLACE(A.MOBILE, '-', '' ) like :TELNO || '%') AND ")
                        Case Else
                    End Select
                Else
                    'あいまい検索
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        UPPER(A.NAME) like '%' || :NAME || '%' AND ")
                        Case IdSerchVin       'VIN
                            .Append("        UPPER(B.VIN) like '%' || :VIN || '%' AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        UPPER(B.VCLREGNO) like '%' || :VCLREGNO || '%' AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            'REM .Append("        (A.TELNO like '%' || :TELNO || '%' OR A.MOBILE like '%' || :TELNO || '%') AND ")
                            .Append("    (REPLACE(A.TELNO, '-', '' ) like '%' || :TELNO || '%' ")
                            .Append(" OR  REPLACE(A.MOBILE, '-', '' ) like '%' || :TELNO || '%') AND ")
                        Case Else
                    End Select
                End If

                .Append("        A.DELFLG = '0' AND ")
                .Append("        A.DLRCD = :DLRCD AND ")
                'Del .Append("        A.STRCD = :STRCD AND ")
                .Append("        B.DLRCD = A.DLRCD AND ")
                .Append("        B.ORIGINALID = A.ORIGINALID AND ")
                .Append("        B.DELFLG = '0' AND ")
                .Append("        C.DLRCD = B.DLRCD AND ")
                .Append("        C.STRCD = B.STRCD AND ")
                .Append("        C.ORIGINALID = B.ORIGINALID AND ")
                .Append("        C.VIN = B.VIN AND ")
                .Append("        C.RMFLG = '1' ")
                .Append("    UNION ALL ")
                .Append("    /* 未取引客 */ ")
                .Append("    SELECT ")
                .Append("        COUNT(1) AS CNT ")
                .Append("    FROM ")
                .Append("        TBL_NEWCUSTOMER A, ")
                .Append("        TBL_NEWCUSTOMERVCLRE B ")
                .Append("    WHERE ")

                If (serchDirection = IdSerchdirectionAfter) Then
                    '前方一致
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        UPPER(A.NAME) like :NAME || '%' AND ")
                        Case IdSerchVin       'VIN
                            .Append("        UPPER(B.VIN) like :VIN || '%' AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        UPPER(B.VCLREGNO) like :VCLREGNO || '%' AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            'REM .Append("        (A.TELNO like :TELNO || '%' OR A.MOBILE like :TELNO || '%') AND ")
                            .Append("    (REPLACE(A.TELNO, '-', '' ) like :TELNO || '%' ")
                            .Append(" OR  REPLACE(A.MOBILE, '-', '' ) like :TELNO || '%') AND ")
                        Case Else
                    End Select
                Else
                    'あいまい検索
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        UPPER(A.NAME) like '%' || :NAME || '%' AND ")
                        Case IdSerchVin       'VIN
                            .Append("        UPPER(B.VIN) like '%' || :VIN || '%' AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        UPPER(B.VCLREGNO) like '%' || :VCLREGNO || '%' AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            'REM .Append("        (A.TELNO like '%' || :TELNO || '%' OR A.MOBILE like '%' || :TELNO || '%') AND ")
                            .Append("    (REPLACE(A.TELNO, '-', '' ) like '%' || :TELNO || '%' ")
                            .Append(" OR  REPLACE(A.MOBILE, '-', '' ) like '%' || :TELNO || '%') AND ")
                        Case Else
                    End Select
                End If
                .Append("   Trim(A.ORIGINALID) IS NULL AND ")

                .Append("        A.DELFLG = '0' AND ")
                .Append("        TRIM(A.SUBCUSTOMERID) IS NULL AND ")
                .Append("        A.DLRCD = :DLRCD AND ")
                'Del .Append("        A.STRCD = :STRCD AND ")
                .Append("        B.CSTID(+) = A.CSTID AND ")
                .Append("        B.DLRCD(+) = A.DLRCD AND ")
                .Append("        B.DELFLG(+) = '0' ")
                .Append("    ) ")
            End With

            Using query As New DBSelectQuery(Of SC3080101DataSet.SC3080101CNTDataTable)("SC3080101_001")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)          '販売店コード
                'Del query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)          '店舗コード

                '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                Select Case serchType
                    Case IdSerchName      '顧客名称
                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, serchValue)
                    Case IdSerchVin       'VIN
                        query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, serchValue)
                    Case IdSerchVclregno  '車両登録No
                        query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, serchValue)
                    Case IdSerchTel       '電話番号/携帯番号
                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, serchValue)
                End Select

                Dim cntTbl As SC3080101DataSet.SC3080101CNTDataTable

                cntTbl = query.GetData()

                Return CType(cntTbl.Item(0).CNT, Integer)

            End Using

        End Function

        ''' <summary>
        ''' 顧客一覧取得
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="strcd">店舗コード</param>
        ''' <param name="serchDirection">検索方向</param>
        ''' <param name="serchType">検索タイプ</param>
        ''' <param name="serchValue">検索テキスト</param>
        ''' <param name="sortType">ソート条件</param>
        ''' <param name="sortOrder">ソート方向</param>
        ''' <returns>顧客一覧</returns>
        ''' <remarks></remarks>
        Public Shared Function GetCustomerList(ByVal dlrcd As String, _
                                         ByVal strcd As String, _
                                         ByVal serchDirection As Integer, _
                                         ByVal serchType As Integer, _
                                         ByVal serchValue As String, _
                                         ByVal sortType As Integer, _
                                         ByVal sortOrder As Integer) As SC3080101DataSet.SC3080101CustDataTable


            If (serchType < 0) Then
                Return Nothing
            End If
            If (sortType < 0) Then
                Return Nothing
            End If

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080101_002 */ ")
                .Append("    CSTKIND, ")
                .Append("    CUSTYPE, ")
                .Append("    CRCUSTID, ")
                .Append("    IMAGEFILE_L, ")
                .Append("    IMAGEFILE_M, ")
                .Append("    IMAGEFILE_S, ")
                .Append("    NAMETITLE, ")
                .Append("    NAME, ")
                .Append("    TELNO, ")
                .Append("    MOBILE, ")
                .Append("    SERIESNM, ")
                .Append("    VCLREGNO, ")
                .Append("    VIN, ")
                .Append("    SEQNO, ")
                .Append("    SSUSERNAME, ")
                .Append("    SAUSERNAME, ")
                .Append("    STAFFCD ")
                .Append("FROM ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        CSTKIND, ")
                .Append("        CUSTYPE, ")
                .Append("        CRCUSTID, ")
                .Append("        IMAGEFILE_L, ")
                .Append("        IMAGEFILE_M, ")
                .Append("        IMAGEFILE_S, ")
                .Append("        NAMETITLE, ")
                .Append("        NAME, ")
                .Append("        TELNO, ")
                .Append("        MOBILE, ")
                .Append("        SERIESNM, ")
                .Append("        VCLREGNO, ")
                .Append("        VIN, ")
                .Append("        SEQNO, ")
                .Append("        SSUSERNAME, ")
                .Append("        SAUSERNAME, ")
                .Append("        STAFFCD, ")
                '1：顧客名称、2：モデル名称、3：セールススタッフ名称、4：サービスアドバイザー名称
                Select Case sortType
                    Case IdSortName         '顧客名称
                        If (sortOrder = IdOrderAsc) Then
                            .Append("        ROW_NUMBER() OVER (ORDER BY NAME) AS ROWNO ")
                        Else
                            .Append("        ROW_NUMBER() OVER (ORDER BY NAME DESC) AS ROWNO ")
                        End If
                    Case IdSortModel         'モデル名称
                        If (sortOrder = IdOrderAsc) Then
                            .Append("        ROW_NUMBER() OVER (ORDER BY VCLREGNO) AS ROWNO ")
                        Else
                            .Append("        ROW_NUMBER() OVER (ORDER BY VCLREGNO DESC) AS ROWNO ")
                        End If
                    Case IdSortSsusername         'セールススタッフ名称
                        If (sortOrder = IdOrderAsc) Then
                            .Append("        ROW_NUMBER() OVER (ORDER BY SSUSERNAME) AS ROWNO ")
                        Else
                            .Append("        ROW_NUMBER() OVER (ORDER BY SSUSERNAME DESC) AS ROWNO ")
                        End If
                    Case IdSortSausername         'サービスアドバイザー名称
                        If (sortOrder = IdOrderAsc) Then
                            .Append("        ROW_NUMBER() OVER (ORDER BY SAUSERNAME) AS ROWNO ")
                        Else
                            .Append("        ROW_NUMBER() OVER (ORDER BY SAUSERNAME DESC) AS ROWNO ")
                        End If
                    Case Else
                End Select
                .Append("    FROM ")
                .Append("        ( ")
                .Append("        /* 自社客 */ ")
                .Append("        SELECT ")
                .Append("            '1' AS CSTKIND, ")
                .Append("            A.CUSTYPE, ")
                .Append("            A.ORIGINALID AS CRCUSTID, ")
                .Append("            F.IMAGEFILE_L AS IMAGEFILE_L, ")
                .Append("            F.IMAGEFILE_M AS IMAGEFILE_M, ")
                .Append("            F.IMAGEFILE_S AS IMAGEFILE_S, ")
                .Append("            A.NAMETITLE AS NAMETITLE, ")
                .Append("            A.NAME AS NAME, ")
                .Append("            A.TELNO AS TELNO, ")
                .Append("            A.MOBILE AS MOBILE, ")
                .Append("            B.SERIESNM AS SERIESNM, ")
                .Append("            B.VCLREGNO AS VCLREGNO, ")
                .Append("            B.VIN AS VIN, ")
                .Append("            0 AS SEQNO, ")
                .Append("            D.USERNAME AS SSUSERNAME, ")
                .Append("            E.USERNAME AS SAUSERNAME, ")
                .Append("            A.STAFFCD AS STAFFCD ")
                .Append("        FROM ")
                .Append("            TBLORG_CUSTOMER A, ")
                .Append("            TBLORG_CUSTOMER_APPEND F, ")
                .Append("            TBLORG_VCLINFO B, ")
                .Append("            TBLORG_BRANCHINFO C, ")
                .Append("            TBL_USERS D, ")
                .Append("            TBL_USERS E ")
                .Append("        WHERE ")
                If (serchDirection = IdSerchdirectionAfter) Then
                    '前方一致
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        UPPER(A.NAME) like :NAME || '%' AND ")
                        Case IdSerchVin       'VIN
                            .Append("        UPPER(B.VIN) like :VIN || '%' AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        UPPER(B.VCLREGNO) like :VCLREGNO || '%' AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            '.Append("        (A.TELNO like :TELNO || '%' OR A.MOBILE like :TELNO || '%') AND ")

                            .Append("    (REPLACE(A.TELNO, '-', '' ) like :TELNO || '%' ")
                            .Append(" OR  REPLACE(A.MOBILE, '-', '' ) like :TELNO || '%') AND ")
                        Case Else
                    End Select
                Else
                    'あいまい検索
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        UPPER(A.NAME) like '%' || :NAME || '%' AND ")
                        Case IdSerchVin       'VIN
                            .Append("        UPPER(B.VIN) like '%' || :VIN || '%' AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        UPPER(B.VCLREGNO) like '%' || :VCLREGNO || '%' AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            'REM .Append("        (A.TELNO like '%' || :TELNO || '%' OR A.MOBILE like '%' || :TELNO || '%') AND ")

                            .Append("    (REPLACE(A.TELNO, '-', '' ) like '%' || :TELNO || '%' ")
                            .Append(" OR  REPLACE(A.MOBILE, '-', '' ) like '%' || :TELNO || '%') AND ")
                        Case Else
                    End Select
                End If
                .Append("        A.DELFLG = '0' AND ")
                .Append("            A.DLRCD = :DLRCD AND ")
                'Del .Append("            A.STRCD = :STRCD AND ")
                .Append("            F.ORIGINALID(+) = A.ORIGINALID AND ")
                .Append("            B.DLRCD = A.DLRCD AND ")
                .Append("            B.ORIGINALID = A.ORIGINALID AND ")
                .Append("            B.DELFLG = '0' AND ")
                .Append("            C.DLRCD = B.DLRCD AND ")
                .Append("            C.STRCD = B.STRCD AND ")
                .Append("            C.ORIGINALID = B.ORIGINALID AND ")
                .Append("            C.VIN = B.VIN AND ")
                .Append("            C.RMFLG = '1' AND ")
                .Append("            D.ACCOUNT(+) = A.STAFFCD AND ")
                .Append("            D.DELFLG(+) = '0' AND ")
                .Append("            E.ACCOUNT(+) = B.SACODE AND ")
                .Append("            E.DELFLG(+) = '0' ")
                .Append("        UNION ALL ")
                .Append("        /* 未取引客 */ ")
                .Append("        SELECT ")
                .Append("            '2' AS CSTKIND, ")
                .Append("            A.CUSTYPE, ")
                .Append("            A.CSTID AS CRCUSTID, ")
                .Append("            A.IMAGEFILE_L AS IMAGEFILE_L, ")
                .Append("            A.IMAGEFILE_M AS IMAGEFILE_M, ")
                .Append("            A.IMAGEFILE_S AS IMAGEFILE_S, ")
                .Append("            A.NAMETITLE AS NAMETITLE, ")
                .Append("            A.NAME AS NAME, ")
                .Append("            A.TELNO AS TELNO, ")
                .Append("            A.MOBILE AS MOBILE, ")
                .Append("            B.SERIESNAME AS SERIESNM, ")
                .Append("            B.VCLREGNO AS VCLREGNO, ")
                .Append("            B.VIN AS VIN, ")
                .Append("            B.SEQNO, ")
                .Append("            D.USERNAME AS SSUSERNAME, ")
                .Append("            E.USERNAME AS SAUSERNAME, ")
                .Append("            A.STAFFCD AS STAFFCD ")
                .Append("        FROM ")
                .Append("            TBL_NEWCUSTOMER A, ")
                .Append("            TBL_NEWCUSTOMERVCLRE B, ")
                .Append("            TBL_USERS D, ")
                .Append("            TBL_USERS E ")
                .Append("        WHERE ")
                If (serchDirection = IdSerchdirectionAfter) Then
                    '前方一致
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        UPPER(A.NAME) like :NAME || '%' AND ")
                        Case IdSerchVin       'VIN
                            .Append("        UPPER(B.VIN) like :VIN || '%' AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        UPPER(B.VCLREGNO) like :VCLREGNO || '%' AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            'REM .Append("        (A.TELNO like :TELNO || '%' OR A.MOBILE like :TELNO || '%') AND ")

                            .Append("    (REPLACE(A.TELNO, '-', '' ) like :TELNO || '%' ")
                            .Append(" OR  REPLACE(A.MOBILE, '-', '' ) like :TELNO || '%') AND ")
                        Case Else
                    End Select
                Else
                    'あいまい検索
                    '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                    Select Case serchType
                        Case IdSerchName      '顧客名称
                            .Append("        UPPER(A.NAME) like '%' || :NAME || '%' AND ")
                        Case IdSerchVin       'VIN
                            .Append("        UPPER(B.VIN) like '%' || :VIN || '%' AND ")
                        Case IdSerchVclregno  '車両登録No
                            .Append("        UPPER(B.VCLREGNO) like '%' || :VCLREGNO || '%' AND ")
                        Case IdSerchTel       '電話番号/携帯番号
                            'REM .Append("        (A.TELNO like '%' || :TELNO || '%' OR A.MOBILE like '%' || :TELNO || '%') AND ")

                            .Append("    (REPLACE(A.TELNO, '-', '' ) like '%' || :TELNO || '%' ")
                            .Append(" OR  REPLACE(A.MOBILE, '-', '' ) like '%' || :TELNO || '%') AND ")
                        Case Else
                    End Select
                End If
                .Append("   Trim(A.ORIGINALID) IS NULL AND ")

                .Append("        A.DELFLG = '0' AND ")
                .Append("        TRIM(A.SUBCUSTOMERID) IS NULL AND ")
                .Append("            A.DLRCD = :DLRCD AND ")
                'Del .Append("            A.STRCD = :STRCD AND ")
                .Append("            B.CSTID(+) = A.CSTID AND ")
                .Append("            B.DLRCD(+) = A.DLRCD AND ")
                .Append("            B.DELFLG(+) = '0' AND ")
                .Append("            D.ACCOUNT(+) = A.STAFFCD AND ")
                .Append("            D.DELFLG(+) = '0' AND ")
                .Append("            E.ACCOUNT(+) = A.SACODE AND ")
                .Append("            E.DELFLG(+) = '0' ")
                .Append("        ) ")
                .Append("    ) ")
                .Append(" ORDER BY ROWNO ")

                '.Append("WHERE ")
                '.Append("    ROWNO < :TONO AND ")
                '.Append("    ROWNO > :FROMNO ")
            End With

            Using query As New DBSelectQuery(Of SC3080101DataSet.SC3080101CustDataTable)("SC3080101_002")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)          '販売店コード
                'Del query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)          '店舗コード

                '1: 顧客名称、2: VIN、3: 車両登録No、4: 電話番号/携帯番号
                Select Case serchType
                    Case IdSerchName      '顧客名称
                        query.AddParameterWithTypeValue("NAME", OracleDbType.NVarchar2, serchValue)
                    Case IdSerchVin       'VIN
                        query.AddParameterWithTypeValue("VIN", OracleDbType.NVarchar2, serchValue)
                    Case IdSerchVclregno  '車両登録No
                        query.AddParameterWithTypeValue("VCLREGNO", OracleDbType.NVarchar2, serchValue)
                    Case IdSerchTel       '電話番号/携帯番号
                        query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, serchValue)
                    Case Else
                End Select

                Return query.GetData()

            End Using

        End Function


        ''' <summary>
        ''' 顧客件数取得　(電話番号検索用ＳＱＬ)
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="strcd">店舗コード</param>
        ''' <param name="serchDirection">検索方向</param>
        ''' <param name="serchType">検索タイプ</param>
        ''' <param name="serchValue">検索テキスト</param>
        ''' <returns>顧客件数</returns>
        ''' <remarks></remarks>
        Public Shared Function GetTelSearchCountCustomer(ByVal dlrcd As String, _
                                         ByVal strcd As String, _
                                         ByVal serchDirection As Integer, _
                                         ByVal serchType As Integer, _
                                         ByVal serchValue As String) As Integer


            If (serchType < 0) Then
                Return Nothing
            End If

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080101_001 */ ")
                .Append("    SUM(CNT) AS CNT ")
                .Append("FROM ")
                .Append("    ( ")
                .Append("    /* 自社客 */ ")
                .Append("    SELECT ")
                .Append("        COUNT(1) AS CNT ")
                .Append("    FROM ")
                .Append("            (SELECT ")
                .Append("                 * ")
                .Append("             FROM ")
                .Append("                 TBLORG_CUSTOMER ")
                .Append("             WHERE ")
                .Append("                 DLRCD = :DLRCD         AND ")
                If (serchDirection = IdSerchdirectionAfter) Then
                    .Append("                 REPLACE(TRIM(TELNO), N'-', NULL ) like :TELNO || '%'  AND ")
                Else
                    .Append("                 REPLACE(TRIM(TELNO), N'-', NULL ) like '%' || :TELNO || '%'  AND ")
                End If
                .Append("                 DELFLG = '0' ")
                .Append("             UNION      ")
                .Append("             SELECT ")
                .Append("                 * ")
                .Append("             FROM ")
                .Append("                 TBLORG_CUSTOMER ")
                .Append("             WHERE ")
                .Append("                 DLRCD = :DLRCD         AND ")
                If (serchDirection = IdSerchdirectionAfter) Then
                    .Append("                 REPLACE(TRIM(MOBILE), N'-', NULL ) like :TELNO || '%'  AND ")
                Else
                    .Append("                 REPLACE(TRIM(MOBILE), N'-', NULL ) like '%' || :TELNO || '%'  AND ")
                End If
                .Append("                 DELFLG = '0' ")
                .Append("            ) A, ")

                .Append("        TBLORG_VCLINFO B, ")
                .Append("        TBLORG_BRANCHINFO C ")
                .Append("    WHERE ")
                .Append("        B.DLRCD = A.DLRCD AND ")
                .Append("        B.ORIGINALID = A.ORIGINALID AND ")
                .Append("        B.DELFLG = '0' AND ")
                .Append("        C.DLRCD = B.DLRCD AND ")
                .Append("        C.STRCD = B.STRCD AND ")
                .Append("        C.ORIGINALID = B.ORIGINALID AND ")
                .Append("        C.VIN = B.VIN AND ")
                .Append("        C.RMFLG = '1' ")
                .Append("    UNION ALL ")
                .Append("    /* 未取引客 */ ")
                .Append("    SELECT ")
                .Append("        COUNT(1) AS CNT ")
                .Append("    FROM ")
                .Append("            (SELECT ")
                .Append("                 * ")
                .Append("             FROM ")
                .Append("                 tbl_NEWCUSTOMER ")
                .Append("             WHERE ")
                .Append("                 DLRCD = :DLRCD           AND ")
                If (serchDirection = IdSerchdirectionAfter) Then
                    .Append("                 REPLACE(TRIM(TELNO), N'-', NULL ) like :TELNO || '%'  AND ")
                Else
                    .Append("                 REPLACE(TRIM(TELNO), N'-', NULL ) like '%' || :TELNO || '%'  AND ")
                End If
                .Append("                 TRIM(ORIGINALID) IS NULL            AND ")
                .Append("                 TRIM(SUBCUSTOMERID) IS NULL            AND ")
                .Append("                 DELFLG = '0' ")
                .Append("             UNION         ")
                .Append("             SELECT ")
                .Append("                 * ")
                .Append("             FROM ")
                .Append("                 tbl_NEWCUSTOMER ")
                .Append("             WHERE ")
                .Append("                 DLRCD = :DLRCD           AND ")
                If (serchDirection = IdSerchdirectionAfter) Then
                    .Append("                 REPLACE(TRIM(MOBILE), N'-', NULL ) like :TELNO || '%'  AND ")
                Else
                    .Append("                 REPLACE(TRIM(MOBILE), N'-', NULL ) like '%' || :TELNO || '%'  AND ")
                End If
                .Append("                 TRIM(ORIGINALID) IS NULL            AND ")
                .Append("                 TRIM(SUBCUSTOMERID) IS NULL            AND ")
                .Append("                 DELFLG = '0' ")
                .Append("            ) A, ")
                .Append("        TBL_NEWCUSTOMERVCLRE B ")
                .Append("    WHERE ")
                .Append("        B.CSTID(+) = A.CSTID AND ")
                .Append("        B.DLRCD(+) = A.DLRCD AND ")
                .Append("        B.DELFLG(+) = '0' ")
                .Append("    ) ")
            End With

            Using query As New DBSelectQuery(Of SC3080101DataSet.SC3080101CNTDataTable)("SC3080101_001")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)          '販売店コード
                'Del query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)          '店舗コード

                '4: 電話番号/携帯番号
                query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, serchValue)

                Dim cntTbl As SC3080101DataSet.SC3080101CNTDataTable

                cntTbl = query.GetData()

                Return CType(cntTbl.Item(0).CNT, Integer)

            End Using

        End Function

        ''' <summary>
        ''' 顧客一覧取得　(電話番号検索用ＳＱＬ)
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="strcd">店舗コード</param>
        ''' <param name="serchDirection">検索方向</param>
        ''' <param name="serchType">検索タイプ</param>
        ''' <param name="serchValue">検索テキスト</param>
        ''' <param name="sortType">ソート条件</param>
        ''' <param name="sortOrder">ソート方向</param>
        ''' <returns>顧客一覧</returns>
        ''' <remarks></remarks>
        Public Shared Function GetTelSearchCustomerList(ByVal dlrcd As String, _
                                         ByVal strcd As String, _
                                         ByVal serchDirection As Integer, _
                                         ByVal serchType As Integer, _
                                         ByVal serchValue As String, _
                                         ByVal sortType As Integer, _
                                         ByVal sortOrder As Integer) As SC3080101DataSet.SC3080101CustDataTable


            If (serchType < 0) Then
                Return Nothing
            End If
            If (sortType < 0) Then
                Return Nothing
            End If

            Dim sql As New StringBuilder
            With sql
                .Append("SELECT /* SC3080101_002 */ ")
                .Append("    CSTKIND, ")
                .Append("    CUSTYPE, ")
                .Append("    CRCUSTID, ")
                .Append("    IMAGEFILE_L, ")
                .Append("    IMAGEFILE_M, ")
                .Append("    IMAGEFILE_S, ")
                .Append("    NAMETITLE, ")
                .Append("    NAME, ")
                .Append("    TELNO, ")
                .Append("    MOBILE, ")
                .Append("    SERIESNM, ")
                .Append("    VCLREGNO, ")
                .Append("    VIN, ")
                .Append("    SEQNO, ")
                .Append("    SSUSERNAME, ")
                .Append("    SAUSERNAME, ")
                .Append("    STAFFCD ")
                .Append("FROM ")
                .Append("    ( ")
                .Append("    SELECT ")
                .Append("        CSTKIND, ")
                .Append("        CUSTYPE, ")
                .Append("        CRCUSTID, ")
                .Append("        IMAGEFILE_L, ")
                .Append("        IMAGEFILE_M, ")
                .Append("        IMAGEFILE_S, ")
                .Append("        NAMETITLE, ")
                .Append("        NAME, ")
                .Append("        TELNO, ")
                .Append("        MOBILE, ")
                .Append("        SERIESNM, ")
                .Append("        VCLREGNO, ")
                .Append("        VIN, ")
                .Append("        SEQNO, ")
                .Append("        SSUSERNAME, ")
                .Append("        SAUSERNAME, ")
                .Append("        STAFFCD, ")
                '1：顧客名称、2：モデル名称、3：セールススタッフ名称、4：サービスアドバイザー名称
                Select Case sortType
                    Case IdSortName         '顧客名称
                        If (sortOrder = IdOrderAsc) Then
                            .Append("        ROW_NUMBER() OVER (ORDER BY NAME) AS ROWNO ")
                        Else
                            .Append("        ROW_NUMBER() OVER (ORDER BY NAME DESC) AS ROWNO ")
                        End If
                    Case IdSortModel         'モデル名称
                        If (sortOrder = IdOrderAsc) Then
                            .Append("        ROW_NUMBER() OVER (ORDER BY VCLREGNO) AS ROWNO ")
                        Else
                            .Append("        ROW_NUMBER() OVER (ORDER BY VCLREGNO DESC) AS ROWNO ")
                        End If
                    Case IdSortSsusername         'セールススタッフ名称
                        If (sortOrder = IdOrderAsc) Then
                            .Append("        ROW_NUMBER() OVER (ORDER BY SSUSERNAME) AS ROWNO ")
                        Else
                            .Append("        ROW_NUMBER() OVER (ORDER BY SSUSERNAME DESC) AS ROWNO ")
                        End If
                    Case IdSortSausername         'サービスアドバイザー名称
                        If (sortOrder = IdOrderAsc) Then
                            .Append("        ROW_NUMBER() OVER (ORDER BY SAUSERNAME) AS ROWNO ")
                        Else
                            .Append("        ROW_NUMBER() OVER (ORDER BY SAUSERNAME DESC) AS ROWNO ")
                        End If
                    Case Else
                End Select
                .Append("    FROM ")
                .Append("        ( ")
                .Append("        /* 自社客 */ ")
                .Append("        SELECT ")
                .Append("            '1' AS CSTKIND, ")
                .Append("            A.CUSTYPE, ")
                .Append("            A.ORIGINALID AS CRCUSTID, ")
                .Append("            F.IMAGEFILE_L AS IMAGEFILE_L, ")
                .Append("            F.IMAGEFILE_M AS IMAGEFILE_M, ")
                .Append("            F.IMAGEFILE_S AS IMAGEFILE_S, ")
                .Append("            A.NAMETITLE AS NAMETITLE, ")
                .Append("            A.NAME AS NAME, ")
                .Append("            A.TELNO AS TELNO, ")
                .Append("            A.MOBILE AS MOBILE, ")
                .Append("            B.SERIESNM AS SERIESNM, ")
                .Append("            B.VCLREGNO AS VCLREGNO, ")
                .Append("            B.VIN AS VIN, ")
                .Append("            0 AS SEQNO, ")
                .Append("            D.USERNAME AS SSUSERNAME, ")
                .Append("            E.USERNAME AS SAUSERNAME, ")
                .Append("            A.STAFFCD AS STAFFCD ")
                .Append("        FROM ")
                .Append("            (SELECT ")
                .Append("                 * ")
                .Append("             FROM ")
                .Append("                 TBLORG_CUSTOMER ")
                .Append("             WHERE ")
                .Append("                 DLRCD = :DLRCD         AND ")
                If (serchDirection = IdSerchdirectionAfter) Then
                    .Append("                 REPLACE(TRIM(TELNO), N'-', NULL ) like :TELNO || '%'  AND ")
                Else
                    .Append("                 REPLACE(TRIM(TELNO), N'-', NULL ) like '%' || :TELNO || '%'  AND ")
                End If
                .Append("                 DELFLG = '0' ")
                .Append("             UNION      ")
                .Append("             SELECT ")
                .Append("                 * ")
                .Append("             FROM ")
                .Append("                 TBLORG_CUSTOMER ")
                .Append("             WHERE ")
                .Append("                 DLRCD = :DLRCD         AND ")
                If (serchDirection = IdSerchdirectionAfter) Then
                    .Append("                 REPLACE(TRIM(MOBILE), N'-', NULL ) like :TELNO || '%'  AND ")
                Else
                    .Append("                 REPLACE(TRIM(MOBILE), N'-', NULL ) like '%' || :TELNO || '%'  AND ")
                End If
                .Append("                 DELFLG = '0' ")
                .Append("            ) A, ")
                .Append("            TBLORG_CUSTOMER_APPEND F, ")
                .Append("            TBLORG_VCLINFO B, ")
                .Append("            TBLORG_BRANCHINFO C, ")
                .Append("            TBL_USERS D, ")
                .Append("            TBL_USERS E ")
                .Append("        WHERE ")
                .Append("            F.ORIGINALID(+) = A.ORIGINALID AND ")
                .Append("            B.DLRCD = A.DLRCD AND ")
                .Append("            B.ORIGINALID = A.ORIGINALID AND ")
                .Append("            B.DELFLG = '0' AND ")
                .Append("            C.DLRCD = B.DLRCD AND ")
                .Append("            C.STRCD = B.STRCD AND ")
                .Append("            C.ORIGINALID = B.ORIGINALID AND ")
                .Append("            C.VIN = B.VIN AND ")
                .Append("            C.RMFLG = '1' AND ")
                .Append("            D.ACCOUNT(+) = A.STAFFCD AND ")
                .Append("            D.DELFLG(+) = '0' AND ")
                .Append("            E.ACCOUNT(+) = B.SACODE AND ")
                .Append("            E.DELFLG(+) = '0' ")
                .Append("        UNION ALL ")
                .Append("        /* 未取引客 */ ")
                .Append("        SELECT ")
                .Append("            '2' AS CSTKIND, ")
                .Append("            A.CUSTYPE, ")
                .Append("            A.CSTID AS CRCUSTID, ")
                .Append("            A.IMAGEFILE_L AS IMAGEFILE_L, ")
                .Append("            A.IMAGEFILE_M AS IMAGEFILE_M, ")
                .Append("            A.IMAGEFILE_S AS IMAGEFILE_S, ")
                .Append("            A.NAMETITLE AS NAMETITLE, ")
                .Append("            A.NAME AS NAME, ")
                .Append("            A.TELNO AS TELNO, ")
                .Append("            A.MOBILE AS MOBILE, ")
                .Append("            B.SERIESNAME AS SERIESNM, ")
                .Append("            B.VCLREGNO AS VCLREGNO, ")
                .Append("            B.VIN AS VIN, ")
                .Append("            B.SEQNO, ")
                .Append("            D.USERNAME AS SSUSERNAME, ")
                .Append("            E.USERNAME AS SAUSERNAME, ")
                .Append("            A.STAFFCD AS STAFFCD ")
                .Append("        FROM ")
                .Append("            (SELECT ")
                .Append("                 * ")
                .Append("             FROM ")
                .Append("                 tbl_NEWCUSTOMER ")
                .Append("             WHERE ")
                .Append("                 DLRCD = :DLRCD           AND ")
                If (serchDirection = IdSerchdirectionAfter) Then
                    .Append("                 REPLACE(TRIM(TELNO), N'-', NULL ) like :TELNO || '%'  AND ")
                Else
                    .Append("                 REPLACE(TRIM(TELNO), N'-', NULL ) like '%' || :TELNO || '%'  AND ")
                End If
                .Append("                 TRIM(ORIGINALID) IS NULL            AND ")
                .Append("                 TRIM(SUBCUSTOMERID) IS NULL            AND ")
                .Append("                 DELFLG = '0' ")
                .Append("             UNION         ")
                .Append("             SELECT ")
                .Append("                 * ")
                .Append("             FROM ")
                .Append("                 tbl_NEWCUSTOMER ")
                .Append("             WHERE ")
                .Append("                 DLRCD = :DLRCD           AND ")
                If (serchDirection = IdSerchdirectionAfter) Then
                    .Append("                 REPLACE(TRIM(MOBILE), N'-', NULL ) like :TELNO || '%'  AND ")
                Else
                    .Append("                 REPLACE(TRIM(MOBILE), N'-', NULL ) like '%' || :TELNO || '%'  AND ")
                End If
                .Append("                 TRIM(ORIGINALID) IS NULL            AND ")
                .Append("                 TRIM(SUBCUSTOMERID) IS NULL            AND ")
                .Append("                 DELFLG = '0' ")
                .Append("            ) A, ")
                .Append("            TBL_NEWCUSTOMERVCLRE B, ")
                .Append("            TBL_USERS D, ")
                .Append("            TBL_USERS E ")
                .Append("        WHERE ")
                .Append("            B.CSTID(+) = A.CSTID AND ")
                .Append("            B.DLRCD(+) = A.DLRCD AND ")
                .Append("            B.DELFLG(+) = '0' AND ")
                .Append("            D.ACCOUNT(+) = A.STAFFCD AND ")
                .Append("            D.DELFLG(+) = '0' AND ")
                .Append("            E.ACCOUNT(+) = A.SACODE AND ")
                .Append("            E.DELFLG(+) = '0' ")
                .Append("        ) ")
                .Append("    ) ")
                .Append(" ORDER BY ROWNO ")

                '.Append("WHERE ")
                '.Append("    ROWNO < :TONO AND ")
                '.Append("    ROWNO > :FROMNO ")
            End With

            Using query As New DBSelectQuery(Of SC3080101DataSet.SC3080101CustDataTable)("SC3080101_002")

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)          '販売店コード
                'Del query.AddParameterWithTypeValue("STRCD", OracleDbType.Char, strcd)          '店舗コード

                '4: 電話番号/携帯番号
                query.AddParameterWithTypeValue("TELNO", OracleDbType.NVarchar2, serchValue)

                Return query.GetData()

            End Using

        End Function

        ''' <summary>
        ''' 権限情報取得
        ''' </summary>
        ''' <param name="dlrcd">販売店コード</param>
        ''' <param name="operationcode">権限コード</param>
        ''' <returns>SC3080205OrgCustomerDataTableDataTable</returns>
        ''' <remarks></remarks>
        Public Shared Function GetOperaType(ByVal dlrcd As String, _
                                            ByVal operationcode As Long) As SC3080101DataSet.SC3080101OperaTypeDataTable

            Using query As New DBSelectQuery(Of SC3080101DataSet.SC3080101OperaTypeDataTable)("SC3080101_003")

                Dim sql As New StringBuilder
                With sql
                    .Append("SELECT /* SC3080101_003 */ ")
                    .Append("    OPERATIONNAME ")
                    .Append("FROM ")
                    .Append("    TBL_OPERATIONTYPE ")
                    .Append("WHERE ")
                    .Append("    DLRCD = :DLRCD AND ")
                    .Append("    OPERATIONCODE = :OPERATIONCODE ")
                End With

                query.CommandText = sql.ToString()
                query.AddParameterWithTypeValue("DLRCD", OracleDbType.Char, dlrcd)
                query.AddParameterWithTypeValue("OPERATIONCODE", OracleDbType.Int64, operationcode)

                Return query.GetData()

            End Using

        End Function

    End Class

End Namespace
