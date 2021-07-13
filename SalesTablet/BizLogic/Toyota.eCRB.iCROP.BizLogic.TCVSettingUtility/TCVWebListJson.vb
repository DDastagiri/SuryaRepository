Imports System.Runtime.Serialization

''' <summary>
''' tcv_web JSONファイル 全データ格納クラス
''' </summary>
''' <remarks></remarks>
<DataContract()>
Public Class TcvWebListJson
    Inherits AbstractJson

    Private _fileinfo As TCVWebFileInfoJson
    Private _car As TcvWebCarJson
    Private _player_info As TcvWebPlayerInfoJson
    Private _grade As List(Of TcvWebGradeJson)
    Private _exterior_color As List(Of TcvWebExteriorColorJson)
    Private _interior_color As List(Of TcvWebInteriorColorJson)
    Private _parts As List(Of TcvWebPartsJson)
    Private _haita As List(Of TcvWebHaitaJson)
    Private _parts_bunrui As List(Of TcvWebPartsBunruiJson)
    Private _init_depth As List(Of TcvWebInitDepthJson)
    Private _add_img As List(Of TcvWebAddImgJson)

    ''' <summary>
    ''' ファイル情報の設定と取得を行う
    ''' </summary>
    ''' <value>ファイル情報</value>
    ''' <returns>ファイル情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property fileinfo As TCVWebFileInfoJson
        Get
            Return _fileinfo
        End Get
        Set(value As TCVWebFileInfoJson)
            _fileinfo = value
        End Set
    End Property

    ''' <summary>
    ''' 車種情報の設定と取得を行う
    ''' </summary>
    ''' <value>車種情報</value>
    ''' <returns>車種情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property car As TcvWebCarJson
        Get
            Return _car
        End Get
        Set(value As TcvWebCarJson)
            _car = value
        End Set
    End Property

    ''' <summary>
    ''' 再生環境情報の設定と取得を行う
    ''' </summary>
    ''' <value>再生環境情報</value>
    ''' <returns>再生環境情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property player_info As TcvWebPlayerInfoJson
        Get
            Return _player_info
        End Get
        Set(value As TcvWebPlayerInfoJson)
            _player_info = value
        End Set
    End Property

    ''' <summary>
    ''' グレード情報の設定と取得を行う
    ''' </summary>
    ''' <value>グレード情報</value>
    ''' <returns>グレード情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property grade As List(Of TcvWebGradeJson)
        Get
            Return _grade
        End Get
        Set(value As List(Of TcvWebGradeJson))
            _grade = value
        End Set
    End Property

    ''' <summary>
    ''' ボディカラー情報の設定と取得を行う
    ''' </summary>
    ''' <value>ボディカラー情報</value>
    ''' <returns>ボディカラー情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property exterior_color As List(Of TcvWebExteriorColorJson)
        Get
            Return _exterior_color
        End Get
        Set(value As List(Of TcvWebExteriorColorJson))
            _exterior_color = value
        End Set
    End Property

    ''' <summary>
    ''' インテリアカラー情報の設定と取得を行う
    ''' </summary>
    ''' <value>インテリアカラー情報</value>
    ''' <returns>インテリアカラー情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property interior_color As List(Of TcvWebInteriorColorJson)
        Get
            Return _interior_color
        End Get
        Set(value As List(Of TcvWebInteriorColorJson))
            _interior_color = value
        End Set
    End Property

    ''' <summary>
    ''' パーツ情報の設定と取得を行う
    ''' </summary>
    ''' <value>パーツ情報</value>
    ''' <returns>パーツ情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property parts As List(Of TcvWebPartsJson)
        Get
            Return _parts
        End Get
        Set(value As List(Of TcvWebPartsJson))
            _parts = value
        End Set
    End Property

    ''' <summary>
    ''' 排他情報の設定と取得を行う
    ''' </summary>
    ''' <value>排他情報</value>
    ''' <returns>排他情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property haita As List(Of TcvWebHaitaJson)
        Get
            Return _haita
        End Get
        Set(value As List(Of TcvWebHaitaJson))
            _haita = value
        End Set
    End Property

    ''' <summary>
    ''' パーツ分類情報の設定と取得を行う
    ''' </summary>
    ''' <value>パーツ分類情報</value>
    ''' <returns>パーツ分類情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property parts_bunrui As List(Of TcvWebPartsBunruiJson)
        Get
            Return _parts_bunrui
        End Get
        Set(value As List(Of TcvWebPartsBunruiJson))
            _parts_bunrui = value
        End Set
    End Property

    ''' <summary>
    ''' 深度情報の設定と取得を行う
    ''' </summary>
    ''' <value>深度情報</value>
    ''' <returns>深度情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property init_depth As List(Of TcvWebInitDepthJson)
        Get
            Return _init_depth
        End Get
        Set(value As List(Of TcvWebInitDepthJson))
            _init_depth = value
        End Set
    End Property

    ''' <summary>
    ''' 追加画像情報の設定と取得を行う
    ''' </summary>
    ''' <value>追加画像情報</value>
    ''' <returns>追加画像情報</returns>
    ''' <remarks></remarks>
    <DataMember()>
    Public Property add_img As List(Of TcvWebAddImgJson)
        Get
            Return _add_img
        End Get
        Set(value As List(Of TcvWebAddImgJson))
            _add_img = value
        End Set
    End Property

End Class
