﻿//------------------------------------------------------------------------------
//SC3140102.js
//------------------------------------------------------------------------------
//機能：ダッシュボード_javascript
//補足：
//作成：2012/01/16 KN 森下
//更新：
//------------------------------------------------------------------------------

// -------------------------------------------------------------
// メイン処理
// -------------------------------------------------------------
//DOMロード時の処理
$(function () {
    //当月ダッシュボード選択
    $('#dashboardBoxflick').scrollLeft(305);
    //ダッシュボードセクション設定
    $('#dashboardBoxflick').flickable({
        section: 'li'
    });
});
