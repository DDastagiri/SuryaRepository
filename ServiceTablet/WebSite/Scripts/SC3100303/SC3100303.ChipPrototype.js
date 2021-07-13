//ChipPrototype.js
//機能：SMBメイン画面_予約チップクラス
//作成：2012/12/22 TMEJ 張
//更新：2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発
//更新：2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加
//------------------------------

//ストール予約チップクラス
//チップの生成まで、実際の配置は呼び出したクラスで行うものとする

//チップ情報を格納し、チップを生成配置
//@class チップ情報の格納・生成クラス
//       チップの情報を所持し、それらを取り扱う機能を保有
//@param {String} aRezId チップID（プライマリー）
function ReserveChip(aRezId) {
    this.rezId = aRezId;        // 予約ID
    this.vclRegNo = null;       // 車両番号
    this.vclName = null;        // 車種名
    this.customerName = null;   // 顧客名
    this.planVstDate = null;    // 来店予定時間
    this.noShowFollowFlg = 0;   // NoShowフォローフラグ(電話マーク)
    this.updateCnt = 0;         // 更新カウント
    this.vstFlg = 0;            // 来店フラグ(点滅用、1の場合、点滅で表示される)
    // 2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 START
    //    this.sex = 0;               // 性別(0:男性　1:女性)
    this.nameTitle = "";        // 敬称
    this.positionType = "";     // 敬称位置
    // 2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 END
    // 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
    this.cstType = "";          // 顧客種別
    // 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
    this.chipStatus = C_CP_ST_BLUE; // チップステータス(青いかつコールマークがない)
}

ReserveChip.prototype = {
    //予約チップクラスのメンバ変数にデータベースから取得した値を格納
    //@param {DataSet} aDataSet データベースより取得した値
    //@return {void}
    setChipParameter: function setChipParameter(aDataSet) {
        this.setRezId(aDataSet.REZID);                          // 予約ID
        this.setVclRegNo(aDataSet.VCLREGNO);                    // 車両登録番号
        this.setVclName(aDataSet.VEHICLENAME);                  // 車種名
        this.setCustomerName(aDataSet.CUSTOMERNAME);            // 顧客名
        this.setPlanVstDate(aDataSet.REZ_PICK_DATE);            // 開始予定日時
        this.setNoShowFollowFlg(aDataSet.NOSHOWFOLLOWFLG);      // NoShowフォローフラグ(電話マーク)
        this.setUpdateCnt(aDataSet.UPDATE_COUNT);               // 更新カウント
        this.setVstFlg(aDataSet.VISITFLG);                      // 来店フラグ(点滅用、1の場合、点滅で表示される)
        // 2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 START
        //        this.setSex(aDataSet.SEX);                              // 性別(0:男性　1:女性)
        this.setNameTitle(aDataSet.NAMETITLE_NAME);             // 敬称
        this.setPositionType(aDataSet.POSITION_TYPE);           // 敬称位置
        // 2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 END
        // 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
        this.setCstType(aDataSet.CST_TYPE);                     //顧客種別
        // 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
        this.setChipStatus();                                   // チップステータス
    },

    //チップのコピー
    //@return {void}
    copy: function copy(objDestChip) {
        objDestChip.rezId = this.rezId;
        objDestChip.vclRegNo = this.vclRegNo;
        objDestChip.vclName = this.vclName;
        objDestChip.customerName = this.customerName;
        objDestChip.planVstDate = this.planVstDate;
        objDestChip.noShowFollowFlg = this.noShowFollowFlg;
        objDestChip.updateCnt = this.updateCnt;
        objDestChip.vstFlg = this.vstFlg;
        // 2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 START
        //        objDestChip.sex = this.sex;
        objDestChip.nameTitle = this.nameTitle;
        objDestChip.positionType = this.positionType;
        // 2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 END
        // 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
        objDestChip.cstType = this.cstType;
        // 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END
        objDestChip.chipStatus = this.chipStatus;
    },

    //全チップを生成
    //@param {String} nChipType チップタイプ
    //@return {void}
    createChip: function createChip(nType) {
        // チップの枠
        var objChip = $("<li />").addClass("wbChips MainChipArea");
        objChip.attr("id", this.rezId);
        // チップ色を設定
        this.ShowChipColor(objChip);

        // 車両番号
        var objDistinguishedName = $("<div />").addClass("DistinguishedName");

        if (this.vclRegNo) {

            objDistinguishedName.append(this.vclRegNo);

        } else {

            objDistinguishedName[0].innerHTML = "&nbsp";
        };

        // 車種名
        var objCarName = $("<div />").addClass("CarName");

        if (this.vclName) {

            objCarName.append(this.vclName);

        } else {

            objCarName[0].innerHTML = "&nbsp";

        };
        // ユーザ名前
        var objUserName = $("<div style='text-overflow: ellipsis;'/>").addClass("UserName");

        // 2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 START
        //        var strSex = "";

        //        if (this.customerName) {

        //            if (gSC3100303WordIni) {
        //                // 女性の場合
        //                if (this.sex == 1) {
        //                    strSex = " " + gSC3100303WordIni[C_WORD_SEX_WOMEN];
        //                } else {
        //                    // 男性
        //                    strSex = " " + gSC3100303WordIni[C_WORD_SEX_MAN];
        //                }
        //            }

        //            // XXXX 様と表示される
        //            objUserName.append(this.customerName + strSex);

        //        } else {

        //            objUserName[0].innerHTML = "&nbsp";

        //        };

        // 敬称がない場合、顧客名しか表示しない
        if (this.nameTitle == "") {
            objUserName.append(this.customerName);
        } else {
            // 敬称が名称の後
            if (this.positionType == "1") {
                objUserName.append(this.customerName + " " + this.nameTitle);
            } else if (this.positionType == "2") {
                // 敬称が名称の前
                objUserName.append(this.nameTitle + " " + this.customerName);
            } else {
                objUserName.append(this.customerName);
            }
        }
        // 2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 END

        objChip.append(objDistinguishedName);
        objChip.append(objCarName);
        objChip.append(objUserName);

        // 電話マークを追加する
        if ((this.chipStatus == C_CP_ST_BLUE + C_CP_ST_CALL) || (this.chipStatus == C_CP_ST_RED + C_CP_ST_CALL)) {
            var objInactiveTel = $("<div />").addClass("InactiveTel");
            objChip.append(objInactiveTel);
        }

        if (nType == "") {
            // チップが取得した列に置く
            var nColNo = this.getColNo();
            $("li.wbChipsArea:eq(" + (nColNo - 1) + ") .Inner").append(objChip);
        } else {
            // 選択したチップ
            $(".blackBackGround").append(objChip);
        }

        // 来店フラグが1の場合、チップを点滅させる
        if (this.vstFlg == 1) {
            $("#" + this.rezId).addClass("FotterBlink");
        }
    },

    //列番目を取得
    //@param {無し}
    //@return {Integer} 列番目
    getColNo: function getColNo() {
        // 当画面の営業開始時間を取得
        var dtStartWorkTime = new Date();
        dtStartWorkTime.setTime(gStartWorkTime.getTime());
        dtStartWorkTime.setMinutes(0);

        var nMinutes = (this.planVstDate - dtStartWorkTime) / 60 / 1000;
        return Math.floor(nMinutes / 30) + 1;
    },

    // チップ色の設定
    ShowChipColor: function ShowChipColor(objChip) {

        // 青いまたは青い+コールマーク
        if ((this.chipStatus == C_CP_ST_BLUE) || (this.chipStatus == C_CP_ST_BLUE + C_CP_ST_CALL)) {
            //青いチップ
            objChip.addClass("Blue");
        } else {
            //赤いチップ
            objChip.addClass("Red");
        }
    },

    // 予約IDに値を格納する.
    setRezId: function setRezId(aRezId) {
        try {
            if (aRezId) {
                this.rezId = parseInt(aRezId);
            }
        }
        catch (e) {
            this.rezId = null;
        }
    },

    // 車両登録番号
    setVclRegNo: function setVclRegNo(aVclRegNo) {
        if (aVclRegNo) {
            this.vclRegNo = aVclRegNo;
        }
    },

    // 車種名
    setVclName: function setVclName(aVclName) {
        if (aVclName) {
            this.vclName = aVclName;
        }
    },

    // 顧客名
    setCustomerName: function setCustomerName(aCustomerName) {
        if (aCustomerName) {
            this.customerName = aCustomerName;
        }
    },

    // 来店予定日時
    setPlanVstDate: function setPlanVstDate(aPlanVstDate) {
        try {
            if (aPlanVstDate) {
                // DBから取得した形式はyyyyMMddhhmmの12桁stringデータ
                if (aPlanVstDate.toString().length == 12) {
                    this.planVstDate = new Date(left(aPlanVstDate, 4), aPlanVstDate.substr(4, 2) - 1, aPlanVstDate.substr(6, 2), aPlanVstDate.substr(8, 2), aPlanVstDate.substr(10, 2), 0);
                } else {
                    this.planVstDate = new Date(aPlanVstDate);
                }
            }
        }
        catch (e) {
            this.planVstDate = null;
        }
    },

    // NoShowフォローフラグ(電話マーク)
    setNoShowFollowFlg: function setNoShowFollowFlg(aNoShowFollowFlg) {
        try {
            if (aNoShowFollowFlg) {
                this.noShowFollowFlg = parseInt(aNoShowFollowFlg);
                if (this.noShowFollowFlg < 0 || isNaN(this.noShowFollowFlg)) {
                    this.noShowFollowFlg = 0;
                }
            } else {
                this.noShowFollowFlg = 0;
            }
        }
        catch (e) {
            this.noShowFollowFlg = 0;
        }
    },

    // 更新カウント
    setUpdateCnt: function setUpdateCnt(aUpdateCnt) {
        try {
            if (aUpdateCnt) {
                this.updateCnt = parseInt(aUpdateCnt);
                if (this.updateCnt < 0 || isNaN(this.updateCnt)) {
                    this.updateCnt = 0;
                }
            } else {
                this.updateCnt = 0;
            }
        }
        catch (e) {
            this.updateCnt = 0;
        }
    },

    // 来店フラグ
    setVstFlg: function setVstFlg(aVstFlg) {
        try {
            if (aVstFlg) {
                this.vstFlg = parseInt(aVstFlg);
                if (this.vstFlg < 0 || isNaN(this.vstFlg)) {
                    this.vstFlg = 0;
                }
            } else {
                this.vstFlg = 0;
            }
        }
        catch (e) {
            this.vstFlg = 0;
        }
    },

    // 2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 START
    //    // 性別
    //    setSex: function setSex(aSex) {
    //        try {
    //            if (aSex) {
    //                this.sex = parseInt(aSex);
    //                if (this.sex < 0 || isNaN(this.sex)) {
    //                    this.sex = 0;
    //                }
    //            } else {
    //                this.sex = 0;
    //            }
    //        }
    //        catch (e) {
    //            this.sex = 0;
    //        }
    //    },

    // 敬称
    setNameTitle: function setNameTitle(aNameTitle) {
        if (aNameTitle) {
            this.nameTitle = aNameTitle;
        }
    },

    // 敬称位置
    setPositionType: function setPositionType(aPositionType) {
        if (aPositionType) {
            this.positionType = aPositionType;
        }
    },
    // 2014/02/26 TMEJ 張 IT9600_タブレット版SMB チーフテクニシャン機能開発 END
    
    // 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 START
    setCstType: function setCstType(aCstType) {
        if (aCstType) {
            this.cstType = aCstType.trim();
        }
    },
    // 2018/02/20 NSK  山田 REQ-SVT-TMT-20170615-001 来店管理に入庫予約を指定してROを発行する機能を追加 END

    // チップステータス
    setChipStatus: function setChipStatus() {
        var dtDelayDate = new Date();
        dtDelayDate.setTime(this.planVstDate.getTime() + gDelayTime * 1000);
        // 今の時刻を取得
        var dtNow = GetServerTimeNow();

        if (dtDelayDate - dtNow < 0) {
            //青いチップ
            this.chipStatus = C_CP_ST_RED;
        } else {
            //赤いチップ
            this.chipStatus = C_CP_ST_BLUE;
        }

        // コールマーク追加
        if (this.noShowFollowFlg == 1) {
            this.chipStatus += C_CP_ST_CALL;
        }
    }
}
