//------------------------------------------------------------------------------
//SC3140102.js
//------------------------------------------------------------------------------
//機能：ダッシュボード_javascript
//補足：
//作成：2012/01/16 KN 森下
//更新：2012/04/16 KN 西田 ユーザーテスト課題No.37 ダッシュボードをタップしてもチップ詳細が閉じられない
//更新：2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策
//------------------------------------------------------------------------------

// -------------------------------------------------------------
// メイン処理
// -------------------------------------------------------------
//DOMロード時の処理
$(function () {
    //2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 START
    parent.endLoadIFrame();
    //2019/03/14 NSK 山田 18PRJ02750-00_(トライ店システム評価)サービス業務における次世代オペレーション実現の為の性能対策 END
    //当月ダッシュボード選択
    $('#dashboardBoxflick').scrollLeft(305);
    //ダッシュボードセクション設定
    $('#dashboardBoxflick').flickable({
        section: 'li'
    });
    // 2012/04/16 KN 西田 ユーザーテスト課題No.37 ダッシュボードをタップしてもチップ詳細が閉じられない START
    //ダッシュボード全体のタップイベント
    $('#dashboardBoxSubAreaStyle').bind('touchstart mousedown', function () {
        window.parent.FlickChip(window.parent.nowSelectArea);
    });
    //フリック可能な箇所のみ別に定義
    $('#dashboardBoxflick').bind('touchstart.flickable mousedown.flickable', function () {
        window.parent.FlickChip(window.parent.nowSelectArea);
        parent.ParentPopoverClose();
    });
    //一時的に表示されるフィルター
    $('#construction').bind('touchstart mousedown', function () {
        window.parent.FlickChip(window.parent.nowSelectArea);
        parent.ParentPopoverClose();
    });
    //2012/04/16 KN 西田 ユーザーテスト課題No.37 ダッシュボードをタップしてもチップ詳細が閉じられない END
});
