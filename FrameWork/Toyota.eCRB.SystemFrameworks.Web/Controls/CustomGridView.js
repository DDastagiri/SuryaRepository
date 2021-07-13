/// <reference path="Scripts/jquery.js"/>
/// <reference path="Scripts/jquery.fingerscroll.js"/>
/// <reference path="Scripts/icropScript.js"/>
(function (window) {
    var eCRB = {};
    window.eCRB = eCRB;

    //customGridView空間を追加
    $.extend(eCRB, { customGridView: {} });
    //customGridViewに処理追加
    $.extend(eCRB.customGridView, {

        //初期化処理 ( 引数paramには[id],[keyNames],[serverCallBack],[serverPostBack],[hiddenId]プロパティが必要 )
        init: function (param) {

            var $table = $("#" + param.id);
            //クラス属性付与
            $table.addClass("eCRB_CustomGridView");
            //ヘッダー・データをそれぞれのテーブルタグに分割
            eCRB.customGridView.splitTable($table);

            //サーバーコールバック・ポストバックを追加
            $table.get(0).serverCallBack = param.serverCallBack;
            $table.get(0).serverPostBack = param.serverPostBack;
            $table.get(0).keyNames = param.keyNames;
            $table.get(0).hiddenId = param.hiddenId;

            //テーブルタグの動作をカスタムコントロール用に拡張
            $.extend($table.get(0), eCRB.customGridView.extendTableFunctions);

            //初期化用にクリア処理を呼ぶ
            $table.get(0).clearRow();

            //フリックリリースのイベント監視
            var $dataDivWrap = $($.data($table.get(0), "dataDIV"));
            $dataDivWrap.bind("startFlickReleaseTop startFlickReleaseBottom endFlickReleaseTop endFlickReleaseBottom", function (e) {

                if (e.type === "startFlickReleaseBottom") {

                    if ($table.get(0).hasNextPage()) {
                        $table.get(0).nextPage();
                    }
                }
            });

            //ページングのイベント監視
            $("tr[rowType=nextPagingRow]", $table).bind("click", function (e) { $table.get(0).nextPage(); });
            //行選択の監視
            $("tr[rowType=dataRow]", $table).live("click", { tableID: param.id }, eCRB.customGridView.liveSelect);
            //クリックソート
            var $header = $($.data($table.get(0), "headerDIV"));
            $header.find("table th").each(function () {
                var data = {
                    target: $(this),
                    tableID: param.id
                };
                $(this).bind("click", data, eCRB.customGridView.clickSort);
            });
        },

        /*
        検索処理
        */
        search: function (id) {
            var table = $("#" + id).get(0);
            table.clearRow();
            table.nextPage();
        },

        /*
        ソート監視
        */
        clickSort: function (e) {

            var table = $("#" + e.data.tableID).get(0);
            var sortDirection = parseInt(e.data.target.attr("sortDirection"));  //現在のソート方法
            var sortColumnIndex = parseInt(e.data.target.attr("columnIndex"))   //選択列のインデックス
            sortDirection = sortDirection < 2 ? sortDirection + 1 : 0;          //ソートの変更
            //ソートがデフォルトに戻る場合は列インデックスクリア
            if (sortDirection === 0) sortColumnIndex = -1;

            //一旦ソート方向をクリアする
            var $header = $($.data(table, "headerDIV"));
            $header.find("table th").attr("sortDirection", 0).removeClass("eCRB_CustomGridView_SortAscHead eCRB_CustomGridView_SortDescHead");
            //選択列のソート方向をセット
            e.data.target.attr("sortDirection", sortDirection);

            //サーバー側コールバック引数にセット
            table.sortColumnIndex = sortColumnIndex;
            table.sortDirection = sortDirection;

            //ソート用のクラス設定
            if (sortDirection === 1) e.data.target.addClass("eCRB_CustomGridView_SortAscHead");
            if (sortDirection === 2) e.data.target.addClass("eCRB_CustomGridView_SortDescHead");

            //再検索を行う
            table.clearRow();
            table.nextPage();

        },

        /*
        行選択を監視
        */
        liveSelect: function (e) {

            var $selectTR;
            if ($(e.target).is("tr[rowType=dataRow]") === true) {
                //TRを選択
                $selectTR = $(e.target);
            } else {
                //TR以外のイベントの場合は、親となるTR要素を探す
                $selectTR = $(e.target).parents("tr[rowType=dataRow]");
            }

            //キー設定
            $("#" + e.data.tableID).get(0).selectRowDataKey = $.data($selectTR.get(0), "dataKey");
            //HIDDENに格納
            var hiddenID = $("#" + e.data.tableID).get(0).hiddenId;
            $("#" + hiddenID).val(eCRB.customGridView.getJsonString({ "SELKEY": $("#" + e.data.tableID).get(0).selectRowDataKey }));
            //クライアントサイドイベント
            var func = $("#" + e.data.tableID).attr("onClientSelectedRow");
            if (func !== "") {
                //実行
                var ret = eval("(function() { " + func + " })();");
                //結果判定(falseが返却された場合は処理キャンセル)
                if (ret !== undefined && ret === false) return;
            }

            //ポストバック実行
            $("#" + e.data.tableID).get(0).serverPostBack();

        },

        /*
        テーブルタグの動作を拡張
        */
        extendTableFunctions: {

            //行クリアを拡張
            clearRow: function () {

                //行書き込み中の場合はタイマーを止める
                if ($.data(this, "rowWriteTimer")) {
                    clearInterval($.data(this, "rowWriteTimer"));
                    $.data(this, "rowWriteTimer", null);
                }

                //データ行削除
                $("tr[rowType=dataRow]", this).remove();
                //件数などクリア
                this.pageIndex = -1;
                this.totalCount = 0;
                //１本指スクロールを初期化
                var $dataDiv = $($.data(this, "dataDIV"));
                $dataDiv.fingerScroll();
                //ページング行を隠す
                $("tr[rowType=nextPagingRow]", this).hide(0);
                //キャッシュクリア
                $.data(this, "pagesCash", null);
            },

            //次ページ処理
            nextPage: function () { eCRB.customGridView.callPaging(this.id, "NEXT"); },
            //前ページ処理
            prePage: function () { eCRB.customGridView.callPaging(this.id, "PREV"); },

            //ページインデックス
            pageIndex: -1,
            //総レコード数
            totalCount: 0,
            //ソート列インデックス
            sortColumnIndex: -1,
            //ソート方向
            sortDirection: 0,
            //選択行のキー
            selectRowDataKey: {},

            //表示行数を取得
            rowCount: function () {
                return $(this).find("tr[rowType=dataRow]").length;
            },

            //次ページが存在するか
            hasNextPage: function () {
                //データが１件もない場合
                if (this.totalCount <= 0) return false;
                //判定
                return (this.rowCount() < this.totalCount);
            },
            //前ページが存在するか
            hasPrePage: function () {
                return false;
            },

            //引数pageIndexのデータキャッシュを取得(存在しなければNULLを返す)
            getPageCache: function (pageIndex) {

                var cashPages = $.data(this, "pagesCash");
                var hitDataList = null;
                if (!$.isArray(cashPages)) return null;
                //対象ページインデックスのデータを取得
                $.each(cashPages, function (idx, val) {
                    if (val.pageIndex == pageIndex) {
                        //該当ページの情報がヒット
                        hitDataList = val.dataList;
                        //ループ終了
                        return false;
                    }
                });
                return hitDataList;
            },

            //引数のデータをキャッシュ
            setPageCache: function (result) {

                //取得ページインデックスFROM-TO
                var pageIdxFrom = result.PageIndexFrom, pageIdxTo = result.PageIndexTo;
                //サーバーから渡されたデータ配列
                var srvDataList;

                //１ページに表示する行数
                var $table = $(this);
                var pagingRowCount = parseInt($table.attr("PagingRowCount"));

                if ($.isArray(result.Data)) {
                    //List系のデータ
                    srvDataList = result.Data;
                } else if ($.isArray(result.Data["DataTable"])) {
                    //DataTableのデータ
                    srvDataList = result.Data["DataTable"];
                } else {
                    //認識不可能なデータ
                    return false;
                }

                //キャッシュしたデータを保持する配列
                var cashPages = [];
                //取得したページ数分ループ
                var cnt = 0;
                for (var pageIdx = pageIdxFrom; pageIdx <= pageIdxTo; pageIdx++) {
                    //１ページ分切り出し
                    var onePageList = srvDataList.slice(cnt * pagingRowCount, (cnt + 1) * pagingRowCount);
                    //キャッシュリストに詰め込み
                    cashPages[cnt] = { pageIndex: pageIdx, dataList: onePageList };
                    cnt++;
                }

                //データキャッシュ
                $.data(this, "pagesCash", cashPages);
                this.totalCount = result.TotalCount;

                //データキャッシュ成功を返却
                return true;
            },

            //コールバックアクション
            callBackAction: ""

        },

        /*
        クライアント側コールバック
        */
        clientCallBack: function (jsonString, id) {

            try {
                //JSON形式の文字列を変換
                var result = $.parseJSON(jsonString);
                var $table = $("#" + id);

                //サーバー側でエラーが発生している場合、エラーコールバックに切り替える
                if (result["errorMessege"] !== undefined) eCRB.customGridView.errorCallBack(jsonString, id);

                //取得したデータをキャッシュ
                if (!$table.get(0).setPageCache(result)) {
                    //エラーコールバックを呼ぶ
                    eCRB.customGridView.callErrorCallback(id);
                }
                //表示対象のデータを取得
                var dataList = $table.get(0).getPageCache(result.RequestPageIndex);
                //データ表示処理を実行
                eCRB.customGridView.displayList($table, dataList, result.RequestPageIndex);

            } catch (e) {
                //エラー処理
                eCRB.customGridView.errorProcess(id);
                alert(e);
            } finally {
                icropScript.ui.closeLodingWindow();
            }
        },

        /*
        サーバー側処理でエラーが発生した場合に呼ばれるコールバック関数
        */
        errorCallBack: function (msg, context) {
            icropScript.ui.closeLodingWindow();
            //画面遷移の指示がある場合、指定された画面に遷移する
            var jsonData;
            try {
                //JSON形式に変換する
                jsonData = $.parseJSON(msg);
                if (jsonData["redirectUrl"] !== undefined) {
                    //ここでエラーページに遷移
                    location.replace(jsonData["redirectUrl"]);
                    return;
                }
            } catch (e) { } //変換できなくてもエラーとしない

            //エラーコールバックがあれば実行
            eCRB.customGridView.errorProcess(context);
            alert(msg);
        },

        /*
        エラーが発生した際に呼ぶ関数
        */
        errorProcess: function (id) {
            //処理中フラグをOFFにする
            eCRB.customGridView.processFlag = false;
        },

        /*
        コールバック引数の作成
        */
        createCallBackArgs: function (targetID) {

            var args = {
                "CTRLNAME": "Toyota.eCRB.SystemFrameworks.Web.Controls.CustomGridView",
                "PAGEINDEX": $("#" + targetID).get(0).pageIndex,
                "ACTION": $("#" + targetID).get(0).callBackAction,
                "SELKEY": $("#" + targetID).get(0).selectRowDataKey,
                "SORTCOLUMN": $("#" + targetID).get(0).sortColumnIndex,
                "SORTDVS": $("#" + targetID).get(0).sortDirection
            };

            return eCRB.customGridView.getJsonString(args);
        },

        /*
        サーバー側に渡すJSON形式の文字列取得
        */
        getJsonString: function (args) {
            var strParam = "{";
            var cnt = 0;
            $.each(args, function (key, value) {
                if (cnt > 0) strParam += ",";
                if (key !== "SELKEY") {
                    strParam += '"' + key + '" : "' + value + '"';
                } else {
                    //選択行の情報
                    var keyTemp = "{";
                    var cnt2 = 0;
                    $.each(value, function (i, v) {
                        if (cnt2 > 0) keyTemp += ",";
                        keyTemp += '"' + i + '" : "' + v + '"';
                        cnt2++;
                    });
                    keyTemp += "}";
                    strParam += '"' + key + '" : ' + keyTemp;
                }
                cnt++;
            });
            strParam += "}";
            return strParam;
        },

        /*
        ページング処理の呼び出し
        */
        callPaging: function (id, action) {
            var $table = $("#" + id);

            try {

                //処理中の場合は処理終了
                if (eCRB.customGridView.processFlag === true) return;
                //行書き込み中の場合も終了
                if ($.data($table.get(0), "rowWriteTimer")) return;

                //処理中フラグをONにする
                eCRB.customGridView.processFlag = true;

                var dataList, pageIndex
                //キャッシュに前ページ、次ページの情報が存在しているかチェック
                if (action === "NEXT") pageIndex = $table.get(0).pageIndex + 1;             //次ページ処理
                else if (action === "PREV") pageIndex = $table.get(0).startPageIndex - 1;   //前ページ処理

                //キャッシュ存在している場合、キャッシュからデータ表示
                dataList = $table.get(0).getPageCache(pageIndex);
                if ($.isArray(dataList)) {
                    //キャッシュから一覧表示
                    setTimeout(function () { eCRB.customGridView.displayList($table, dataList, pageIndex); }, 0);
                } else {
                    //前or次アクション名を設定
                    $table.get(0).callBackAction = action;
                    //現在のスレッドが終了したら実行
                    var $wrapDiv = $($.data($table.get(0), "wrapDIV"));
                    icropScript.ui.showLodingWindow($wrapDiv);
                    setTimeout(function () { $table.get(0).serverCallBack(); }, 0);
                }

            } catch (e) {
                //エラー処理
                eCRB.customGridView.errorProcess(id);
            }

        },


        /*
        サーバーコールバックが正常終了し、データ表示処理を開始するためのメソッド
        または、キャッシュデータから表示開始するためのメソッド
        */
        displayList: function ($table, dataList, pageIndex) {

            var curPageIndex = $table.get(0).pageIndex;

            //ページングをいったん隠す
            $("tr[rowType=nextPagingRow]", $table).hide(0);
            //リストデータをバインド
            eCRB.customGridView.bindTable($table, dataList, pageIndex);

            //描画が終了した際にページ数をセット
            var $lastRow = $("tr[rowType=dataRow]:last", $table);
            if ($lastRow.length > 0) {

                //最終行のページインデックス取得
                $table.get(0).pageIndex = parseInt($lastRow.attr("pageIndex"));

            } else {
                //データ行なし
                $table.get(0).pageIndex = 0;
            }

            //処理中フラグをOFFにする
            eCRB.customGridView.processFlag = false;
        },


        /*
        データをバインド
        */
        bindTable: function ($table, listData, pageIndex) {

            var $rowTemp = $("tr[rowType=dataTemplate]", $table);       //データ行のテンプレート
            var $nextRowTmp = $("tr[rowType=nextPagingRow]", $table);   //次ページテンプレート
            var $prePageTmp = $("tr[rowType=prePagingRow]", $table);   //前ページテンプレート
            var curPageIndex = parseInt($table.get(0).pageIndex);       //現在表示されている末尾のページインデックス
            var $scrollDiv = $($.data($table.get(0), "dataDIV")).find(".scroll-inner"); //スクロール用のDIV
            //行番号の開始位置セット
            var baseRowNo = pageIndex * parseInt($table.attr("PagingRowCount"));
            //データ行のテンプレートが指定されていない場合終了
            if ($rowTemp.length <= 0) return;
            //行リスト
            var arrayRows = [];
            /*************************************************
            //行書き込み処理 
            *************************************************/
            function writeProcess(fromIndex, toIndex) {

                var index = fromIndex;
                var arrayRows = [];

                //１回分の書き込みループ
                for (var loopCnt = 0; loopCnt < (fromIndex < toIndex ? (toIndex - fromIndex) + 1 : (fromIndex - toIndex) + 1); loopCnt++) {

                    //終了判定
                    if (fromIndex < toIndex) {
                        //次ページ用
                        if (index >= listData.length) break;
                    } else {
                        //前ページ用
                        if (index < 0) break;
                    }

                    var rowNo = baseRowNo + (index + 1);    //１からの連番である行NOを算出
                    var val = listData[index];              //行作成用のデータセット(データソース)

                    //テンプレートをコピー
                    var $tr = $rowTemp.clone(true);
                    //属性変更
                    $tr.attr("rowType", "dataRow").attr("pageIndex", pageIndex).hide(0);

                    /*******************************************************************/
                    //キーの作成
                    var key = {};
                    $.each($table.get(0).keyNames, function (keyIndex, keyName) {
                        //キー名に対応する値を取得
                        key[keyName] = val[keyName] !== undefined ? val[keyName] : "";
                    });
                    //キーの設定
                    $.data($tr.get(0), "dataKey", key);
                    /*******************************************************************/
                    //IDの連番化
                    $("*", $tr).each(function () {
                        var id = $(this).attr("id");
                        if (id !== undefined && id != "") {
                            id = id + "_" + rowNo;
                            $(this).attr("id", id);
                        }
                    });
                    /*******************************************************************/
                    //bindField属性があるエレメントを探す
                    var $bindFields = $tr.find("[bindField]");

                    //バインド値を設定する
                    $bindFields.each(function () {
                        var $field = $(this);
                        var fieldName = $field.attr("bindField");
                        $field.text((val[fieldName] === undefined ? "" : val[fieldName]));  //設定
                    });
                    /*******************************************************************/
                    //行番号の連番をセット
                    $tr.find("[autoNumber=autoNumber]").text(rowNo);
                    /*******************************************************************/

                    //TR要素格納配列に追加
                    arrayRows.push($tr);

                    //加算又は減算
                    index += (fromIndex < toIndex ? 1 : -1);
                }

                //行追加
                var sumHeight = 0;
                $.each(arrayRows, function (aryIndex, aryValue) {

                    aryValue.css("opacity", 0).show(0);       //TR表示

                    if (fromIndex < toIndex) {
                        //次ページ処理
                        if ($nextRowTmp.length > 0) aryValue.insertBefore($nextRowTmp); //次ページ行ありのTABLEの場合
                        else $table.append(aryValue); //次ページ行なしのTABLEの場合
                    } else {
                        //前ページ処理
                        if ($prePageTmp.length > 0) aryValue.insertAfter($prePageTmp); //前ページ行ありのTABLEの場合
                        else $table.prepend(aryValue); //前ページ行なしのTABLEの場合
                    }
                    //高さの合計値加算
                    sumHeight += aryValue.outerHeight();
                });

                $.each(arrayRows, function (aryIndex, aryValue) { aryValue.fadeTo(150, 1); });
                $scrollDiv.triggerHandler("refreshScrollBar");

            }

            //１回分で書き込む行数
            var oneProcessRowCount = 5;

            //最初の一回目だけは同期で処理
            if (curPageIndex < pageIndex) {
                //次ページ処理
                writeProcess(0, oneProcessRowCount - 1);
            } else {
                //前ページ処理
                writeProcess(listData.length - 1, (listData.length - 1) - (oneProcessRowCount - 1));
            }

            //２回目以降の処理の開始インデックス
            var procCnt = curPageIndex < pageIndex ? oneProcessRowCount : (listData.length - 1) - oneProcessRowCount;
            //２回目以降は非同期(タイマを使用し一定間隔毎に処理する)
            var timer = setInterval(function () {

                var timerEnd = false;

                //ループ終了判定
                if (curPageIndex < pageIndex) {
                    //次ページ処理
                    if (procCnt >= listData.length) timerEnd = true;
                } else {
                    //前ページ処理
                    if (procCnt < 0) timerEnd = true;
                }

                //タイマー終了処理(全てのデータの書き込みが終了した)
                if (timerEnd === true) {
                    //タイマを終了させる
                    clearInterval(timer); timer = null;
                    //タイマオブジェクトを開放
                    $.data($table.get(0), "rowWriteTimer", null);
                    //ページング行の制御
                    eCRB.customGridView.setPagingRow($table);
                    //列幅再調整
                    var colWidth = eCRB.customGridView.getColWidthArray($table);
                    eCRB.customGridView.resizeHeader($table, "headerDIV", colWidth);
                    return;
                }

                if (curPageIndex < pageIndex) {
                    //次ページ処理
                    writeProcess(procCnt, (procCnt + oneProcessRowCount) - 1);
                    procCnt += oneProcessRowCount;
                } else {
                    //前ページ処理
                    writeProcess(procCnt, procCnt - (oneProcessRowCount - 1));
                    procCnt -= oneProcessRowCount;
                }

            }, 220);

            //タイマオブジェクトを保存する
            $.data($table.get(0), "rowWriteTimer", timer);
        },

        /*
        ページング行の制御
        */
        setPagingRow: function ($table) {
            if ($table.get(0).totalCount > $table.get(0).rowCount()) {
                //次ページテンプレート行を表示
                $table.find("tr[rowType=nextPagingRow]").show(0);
            } else {
                //次ページテンプレート行を非表示
                $table.find("tr[rowType=nextPagingRow]").hide(0);
            }
        },

        /*
        テーブルタグをヘッダー用とデータ用で分割する
        さらに各テーブルタグをDIVタグで囲う
        */
        splitTable: function ($table) {

            //現在の各列の幅を取得する
            var colWidth = eCRB.customGridView.getColWidthArray($table);

            //ヘッダー行のみを抜き出したテーブルを作成
            var $headerTable = $table.css("border-spacing", "0px").clone(true);
            $headerTable.attr("id", "").find("tr[rowType!=headerRow]").remove();
            //対象テーブルからヘッダー行を削除
            $table.find("tr[rowType=headerRow]").remove();

            //ヘッダーとデータ
            var $wrapDIV = $table.parent("div");
            var $dataDIV = $table.wrap("<div></div>").parent("div");
            var $headerDIV = $headerTable.wrap("<div></div>").parent("div");

            //ヘッダーを追加
            $dataDIV.before($headerDIV);

            //各DIVをデータとして登録
            $.data($table.get(0), "wrapDIV", $wrapDIV.get(0));
            $.data($table.get(0), "dataDIV", $dataDIV.get(0));
            $.data($table.get(0), "headerDIV", $headerDIV.get(0));

            //ヘッダーの列幅を調整
            eCRB.customGridView.resizeHeader($table, "headerDIV", colWidth);
            eCRB.customGridView.resizeHeader($table, "dataDIV", colWidth);

            //データ部の囲いをスクロール化にする
            if ($table.attr("MaxHeight") !== undefined) {

                //囲いの高さを指定
                $wrapDIV.css({ height: $table.attr("MaxHeight"), "overflow-y": "hidden" });
                //ピクセルに変換
                var pixHeight = $wrapDIV.outerHeight();
                var dateHeight = pixHeight - $headerDIV.outerHeight();
                //スクロール用のスタイル設定
                $dataDIV.css({ "height": dateHeight, "overflow-y": "hidden" });
            }


            //サイズ変更を監視
            $(window).bind("resize", function () {
                //列幅再調整
                var colWidthEvent = eCRB.customGridView.getColWidthArray($table);
                eCRB.customGridView.resizeHeader($table, "headerDIV", colWidthEvent);
            });

        },


        /*
        ヘッダーの列幅を調整する
        */
        resizeHeader: function ($table, key, colWidthArray) {

            if ($.data($table.get(0), key) === undefined) return;
            if (!$.isArray(colWidthArray)) return;
            var $headerTable = $($.data($table.get(0), key)).find("table");

            $headerTable.find("tr[rowType!=nextPagingRow][rowType!=prePagingRow]:first").find("td,th").each(function (index) {
                //結合数
                var spanCnt = $(this).attr("colspan") === undefined ? 1 : parseInt($(this).attr("colspan"));
                //設定する列幅を算出
                var colWidth = 0;
                for (var cidx = index; cidx < index + spanCnt; cidx++) {
                    if (colWidthArray[cidx] !== undefined) colWidth += colWidthArray[cidx];
                }
                //列幅設定
                $(this).css("width", colWidth + "px");
            });
        },

        /*
        テーブルの最初の行の各列の幅を配列で取得します。
        */
        getColWidthArray: function ($table) {

            var colary = [];

            //最初の行
            var $firstRow = $table.find("tr[rowType!=nextPagingRow][rowType!=prePagingRow]:visible:first");
            //行数が0件の場合終了(空配列)
            if ($firstRow.length <= 0) return [];
            //各列の幅を配列に格納していく
            $firstRow.find("td,th").each(function (index) {
                //列の結合数を取得
                var spanCnt = $(this).attr("colspan") === undefined ? 1 : parseInt($(this).attr("colspan"));
                //微調整用
                var bdr = Math.ceil(($(this).outerWidth() - $(this).innerWidth()) / 2);
                if (spanCnt > 1) bdr = bdr / spanCnt;
                var oneW = $(this).width() / spanCnt;
                //配列に格納
                for (var cidx = 0; cidx < spanCnt; cidx++) colary.push(oneW + bdr);
            });

            //各列の幅を格納した配列を返却
            return colary;
        },

        /*
        処理中フラグ
        */
        processFlag: false

    });

})(window);