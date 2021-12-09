Public Class CLS_CONST
    '#######################################################################
    '定数定義クラス
    '#######################################################################

    'オブジェクトの種類数
    Public Const C_OBJECT_CNT As Short = 28
    'パラメータの数
    Public Const C_PARAM_CNT As Short = 11
    'ミドルウエアの数  
    Public Const C_MIDDLE_CNT As Short = 2       'OO4OとODBCは当面封印
    ''メッセージ数
    'Public Const C_MESSAGE_CNT As Integer = 250

    Public Shared C_MESSAGE_COLLECTION As New System.Collections.Generic.SortedDictionary(Of String, String)

    'サーバ名
    'Public Const C_SERVER_NAME As String = "localhost"
    Public Const C_SERVER_NAME As String = "www.doppo1.net"

    'マウスクリックコード
    Public Enum C_MOUSE_CLICK
        [LEFT] = 1048576
        [RIGHT] = 2097152
    End Enum

    'DBオブジェクトID
    Public Enum C_OBJECTID
        SCHEMA = 0
        TABLE = 1
        VIEW = 2
        INDEX = 3
        MATERIALIZED_VIEW = 4
        LOB = 5
        TABLE_PARTITION = 6
        TABLE_SUBPARTITION = 7
        INDEX_PARTITION = 8
        INDEX_SUBPARTITION = 9
        LOB_PARTITION = 10
        LOB_SUBPARTITION = 11
        SEQUENCE = 12
        PROCEDURE = 13
        [FUNCTION] = 14
        PACKAGE = 15
        PACKAGE_BODY = 16
        JAVA_CLASS = 17
        JAVA_SOURCE = 18
        TRIGGER = 19
        SYNONYM = 20
        DATABASE_LINK = 21
        TYPE = 22
        LIBRARY = 23
        CLUSTER = 24
        DICTIONARY = 25
        V_VIEW = 26
        ALL_SCHEMA = 27
    End Enum

    'ツリービュー階層レベル
    Public Enum C_TV_LEVEL
        L0 = 0
        L1 = 1
        L2 = 2
    End Enum

    'フォームステータス
    Public Enum C_FORM_STATUS
        NORMAL = 0
        EXECUTING = 1
        INTERRUPT = 2
    End Enum

    'ミドルウエア
    Public Enum C_MIDDLE_TYPE
        ODP = 0    'ODP.NET
        MS_CL = 1  'MicroSoft Oracle Client(system.data.oracleclient)
        ODBC = 2   'ODBC
        OO4O = 3   'OO4O
    End Enum

    'SQLデータ型
    Public Enum C_COLUMN_TYPE
        [STRING] = 1        '文字列
        [NUMBER] = 2        '数字
        [DATE] = 3          '日付
        [TIMESTAMP] = 4     'タイムスタンプ
        [TIMESTAMPTZ] = 5   'タイムスタンプ(タイムゾーン)
        [TIMESTAMPLTZ] = 6  'タイムスタンプ(ローカルタイムゾーン)
        [RAW] = 7           'RAWデータ
        [BLOB] = 8          'バイナリLOB
        [BFILE] = 9         'バイナリファイル
        [INTERVALDS] = 10   'インターバル(日付)
        [INTERVALYM] = 11   'インターバル(年月)
        [UNKNOWN] = 12      'その他
        [CLOB] = 13         'キャラクタLOB
        [LONG] = 14         'LONG
        [INT16] = 15        '2^16までの数字
        [INT32] = 16        '2^32までの数字
        [INT64] = 17        '2^64までの数字
    End Enum

    'パラメータ名
    Public Const C_INITPARAM_INIT_APLOG As String = "INIT_APLOG"
    Public Const C_INITPARAM_INIT_ERRLOG As String = "INIT_ERRLOG"
    Public Const C_INITPARAM_INIT_FETCH As String = "INIT_FETCH"
    Public Const C_INITPARAM_INIT_ENCODE As String = "INIT_ENCODE"
    Public Const C_INITPARAM_INIT_DELIMITER As String = "INIT_DELIMITER"
    Public Const C_INITPARAM_INIT_LOGDIR As String = "INIT_LOGDIR"
    Public Const C_INITPARAM_INIT_FETCH_REFLESH As String = "INIT_FETCH_REFLESH"
    Public Const C_INITPARAM_INIT_ODP_FETCHSIZE As String = "INIT_ODP_FETCHSIZE"
    Public Const C_INITPARAM_INIT_ODP_LONGFETCHSIZE As String = "INIT_ODP_LONGFETCHSIZE"
    Public Const C_INITPARAM_INIT_NLS_LANG As String = "INIT_NLS_LANG"
    Public Const C_INITPARAM_INIT_ENCLOSURE As String = "INIT_ENCLOSURE"
    Public Const C_INITPARAM_INIT_ONLINE As String = "INIT_ONLINE"

    '値の検証タイプ
    Public Enum C_VALIDATE_TYPE
        [NUMBER] = 1
        [STRING] = 2
    End Enum

    'SQLタイプ
    Public Enum C_SQLTYPE
        QUERY = 0
        DML = 1
        DDL_DCL = 2
        PROCEDURE = 3
        OTHER = 4
    End Enum

    'エンコード
    Public Enum C_ENCODE
        UTF8 = 0
        SJIS = 1
        EUC = 2
    End Enum

    '区切り文字
    Public Enum C_DELIMITER
        COMMA = 0
        TAB = 1
        SPACE = 2
    End Enum

    '囲い文字
    Public Enum C_ENCLOSURE
        NONE = 0
        DOUBLE_QUOTATION = 1
        SINGLE_QUOTATION = 2
    End Enum

    'ソートタイプ
    Public Enum C_SORT
        [STRING] = 0
        [NUMBER] = 1
        [DATE] = 2
    End Enum

    Public Structure S_MIDDLE
        Private ID As Short
        Private name As String
        Public Sub SetMiddleware(ByVal ID1 As C_MIDDLE_TYPE, ByVal name1 As String)
            Me.ID = ID1
            Me.name = name1
        End Sub
        Public Function GetID() As C_MIDDLE_TYPE
            Return Me.ID
        End Function
        Public Function GetName() As String
            Return Me.name
        End Function
    End Structure

    Public Shared C_MIDDLE(C_MIDDLE_CNT - 1) As S_MIDDLE

    Public Function GetMiddlewareID(ByVal name As String) As C_MIDDLE_TYPE
        Dim ID As Integer = -1

        For i As Integer = 0 To C_MIDDLE_CNT - 1
            If C_MIDDLE(i).GetName = name Or _
               C_MIDDLE(i).GetName = name Then
                ID = C_MIDDLE(i).GetID
                Exit For
            End If
        Next
        Return ID
    End Function

    Public Shared Function GetMiddlewareName(ByVal ID As String)
        Dim name As String = vbNullString

        For i As Integer = 0 To C_MIDDLE_CNT - 1
            If C_MIDDLE(i).GetID = ID Then
                name = C_MIDDLE(i).GetName
                Exit For
            End If
        Next
        Return name
    End Function

    'DBオブジェクト構造体
    Public Structure S_OBJECT
        Private ID As C_OBJECTID          'オブジェクトID
        Private eName As String           '英語名称
        Private jName As String           '日本語名称
        Private cols As String()          'エクスプローラのリストビューで表示する列
        Private contextID As String()     'エクスプローラで表示するコンテキストメニューのID
        Private contextName As String()   'エクスプローラで表示するコンテキストメニュー
        Private sortType() As Integer     '各列のソートタイプ
        Public Sub SetObject(ByVal ID1 As C_OBJECTID, ByVal eName1 As String, ByVal jName1 As String, ByVal cols1 As String(), ByVal contextID1 As String(), ByVal contextName1 As String(), ByVal sortType As Integer())

            '定義の誤りチェック
            If Not (cols1.Length = sortType.Length) Then
                MsgBox("列定義が誤っています")
            End If
            If Not (contextID1.Length = contextName1.Length) Then
                MsgBox("コンテキスト定義が誤っています")
            End If

            Me.ID = ID1
            Me.eName = eName1
            Me.jName = jName1
            Me.cols = cols1
            Me.contextID = contextID1
            Me.contextName = contextName1
            Me.sortType = sortType
        End Sub
        Public Function GetID() As C_OBJECTID
            Return Me.ID
        End Function
        Public Function GetEName() As String
            Return Me.eName
        End Function
        Public Function GetJName() As String
            Return Me.jName
        End Function
        Public Function GetCols() As String()
            Return Me.cols
        End Function
        Public Function GetContextID() As String()
            Return Me.contextID
        End Function
        Public Function GetContextName() As String()
            Return Me.contextName
        End Function
        Public Function GetSortType() As Integer()
            Return Me.sortType
        End Function
    End Structure
    Public Shared C_OBJECT(C_OBJECT_CNT - 1) As S_OBJECT

    ''メッセージ構造体
    'Public Structure S_MESSAGE
    '    Private ID As String          'メッセージID
    '    Private eMessage As String   '英語メッセージ
    '    Private jMessage As String   '日本語メッセージ
    '    Public Sub SetMessage(ByVal ID1 As String, ByVal eMessage1 As String, ByVal jMessage1 As String)
    '        ID = ID1
    '        eMessage = eMessage1
    '        jMessage = jMessage1
    '    End Sub
    '    Public Function GET_ID() As String
    '        Return Me.ID
    '    End Function
    '    Public Function GET_E_MESSAGE() As String
    '        Return Me.eMessage
    '    End Function
    '    Public Function GET_J_MESSAGE() As String
    '        Return Me.jMessage
    '    End Function
    'End Structure
    'Public Shared C_MESSAGE(C_MESSAGE_CNT) As S_MESSAGE

    'バインド変数構造体
    Public Structure S_BIND
        Private NAME As String
        Private VALUE As String
        Private TYPE As Short
        Private INOUT As ParameterDirection
        Private SIZE As Integer

        Public Sub SET_BIND(ByVal name As String, ByVal value As String, ByVal type As Short, ByVal inOut As ParameterDirection, ByVal size As Integer)
            Me.NAME = name
            Me.VALUE = value
            Me.TYPE = type
            Me.INOUT = inOut
            Me.SIZE = size
        End Sub
        Public Function GET_NAME() As String
            Return Me.NAME
        End Function
        Public Function GET_VALUE() As String
            Return Me.VALUE
        End Function
        Public Function GET_TYPE() As Short
            Return Me.TYPE
        End Function
        Public Function GET_INOUT() As ParameterDirection
            Return Me.INOUT
        End Function
        Public Function GET_SIZE() As Integer
            Return Me.SIZE
        End Function
        Public Sub SET_TYPE(ByVal type As Integer)
            Me.TYPE = type
        End Sub

    End Structure

    'PL/SQL連想配列バインド変数構造体
    Public Structure S_BIND_ARR
        Private NAME As String
        Private ARRAY_COUNT As Integer
        Private VALUE As String()
        Private TYPE As Short
        Private INOUT As ParameterDirection
        Private ARRAY_BIND_SIZE As Integer()

        Public Sub SET_BIND(ByVal name As String, ByVal arrayCount As Integer, ByVal value As String(), ByVal type As Short, ByVal inOut As ParameterDirection, ByVal arrayBindSize As Integer())
            Me.NAME = name
            Me.ARRAY_COUNT = arrayCount
            Me.VALUE = value
            Me.TYPE = type
            Me.INOUT = inOut
            Me.ARRAY_BIND_SIZE = arrayBindSize
        End Sub
        Public Function GET_NAME() As String
            Return Me.NAME
        End Function
        Public Function GET_ARRAY_COUNT() As String
            Return Me.ARRAY_COUNT
        End Function
        Public Function GET_VALUE() As String()
            Return Me.VALUE
        End Function
        Public Function GET_TYPE() As Short
            Return Me.TYPE
        End Function
        Public Function GET_INOUT() As ParameterDirection
            Return Me.INOUT
        End Function
        Public Function GET_ARRAY_BIND_SIZE() As Integer()
            Return Me.ARRAY_BIND_SIZE
        End Function
        Public Sub SET_TYPE(ByVal type As Integer)
            Me.TYPE = type
        End Sub

    End Structure

    Public Shared Sub Init()
        SetObject()
        SetMessage()
        SetDefaultInitParam()
        SetMiddleware()
    End Sub

    '初期化パラメータ構造体
    Public Structure S_INITPARAM
        Private NAME As String
        Private INFO As String
        Private VALUE As String
        Private VALIDATE_TYPE As Short
        Private MIN_VALUE As Integer
        Private MAX_VALUE As Integer
        Public Sub SET_INITPARAM(ByVal NAME As String, ByVal INFO As String, ByVal VALUE As String)
            Me.NAME = NAME
            Me.INFO = INFO
            Me.VALUE = VALUE
        End Sub
        Public Sub CHANGE_INITPARAM_VALUE(ByVal PARAM As String())
            'Me.NAME = PARAM(0)　'パラメータの名前や説明文は変更せず値のみ変更
            'Me.INFO = PARAM(1)  
            Me.VALUE = PARAM(2)
        End Sub
        Public Function GET_NAME() As String
            Return NAME
        End Function
        Public Function GET_INFO() As String
            Return INFO
        End Function
        Public Function GET_VALUE() As String
            Return VALUE
        End Function
        Public Function GET_ALL(Optional ByVal delimiter As String = ",") As String
            Return NAME & delimiter & INFO & delimiter & VALUE
        End Function
    End Structure
    Public Shared C_INITPARAM(C_PARAM_CNT) As S_INITPARAM

    Public Shared Function SetInitParam(ByVal initParamRowStr As String) As Boolean
        '初期化パラメータ設定
        Dim rowStr(2) As String  'パラメータの形式："paramname,description,paramvalue"

        '行数文の配列で定義
        rowStr = initParamRowStr.Split(",")
        If rowStr.Length = 3 Then
            For i As Integer = 0 To C_INITPARAM.Length - 1
                If C_INITPARAM(i).GET_NAME = rowStr(0) Then
                    C_INITPARAM(i).CHANGE_INITPARAM_VALUE(rowStr)
                    Exit For
                End If
            Next
        End If
        Return True

    End Function

    Public Shared Function GetInitParamValue(ByVal paramName As String) As String
        For i As Integer = 0 To C_INITPARAM.Length - 1
            If C_INITPARAM(i).GET_NAME = paramName Then
                Return C_INITPARAM(i).GET_VALUE
            End If
        Next
        MsgBox("指定したパラメータ(" & paramName & ")は見つかりませんでした")
        Return vbNullString
    End Function

    Private Shared Sub SetObject()
        Dim colInfo() As String
        Dim contextID() As String
        Dim contextName() As String
        Dim sprtType() As Integer

        'スキーマオブジェクト
        colInfo = New String() {"タイプ", "オブジェクト名", "サブオブジェクト名", "作成時間", "最終DDL実行時間", "状態"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}

        C_OBJECT(C_OBJECTID.SCHEMA).SetObject(C_OBJECTID.SCHEMA, "SCHEMA", "スキーマ", colInfo, contextID, contextName, sprtType)

        'テーブル
        colInfo = New String() {"名称", "表領域名", "エクステント数", "サイズ(KB)", "統計情報採取時間", "統計情報件数", "統計情報平均行サイズ(B)", "コメント", "実件数"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.STRING, C_SORT.NUMBER}
        contextID = New String() {"CNT", "CP", "TRUNC", "DROP", "STATS"}
        contextName = New String() {"件数取得", "コピー", "TRUNCATE", "DROP", "統計取得"}
        C_OBJECT(C_OBJECTID.TABLE).SetObject(C_OBJECTID.TABLE, "TABLE", "テーブル", colInfo, contextID, contextName, sprtType)

        'ビュー
        colInfo = New String() {"名称", "状態", "コメント", "件数"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.NUMBER}
        contextID = New String() {"CNT", "CP", "DROP"}
        contextName = New String() {"件数取得", "コピー", "DROP"}
        C_OBJECT(C_OBJECTID.VIEW).SetObject(C_OBJECTID.VIEW, "VIEW", "ビュー", colInfo, contextID, contextName, sprtType)

        'インデックス
        colInfo = New String() {"テーブル名", "インデックス名", "表領域名", "エクステント数", "サイズ(KB)", "階層レベル", "リーフブロック数", "キー値の数", "クラスタ化係数", "統計情報採取時間", "統計情報件数"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.STRING, C_SORT.NUMBER}
        contextID = New String() {"CP", "DROP", "REBUILD", "STATS"}
        contextName = New String() {"コピー", "DROP", "再構築", "統計取得"}
        C_OBJECT(C_OBJECTID.INDEX).SetObject(C_OBJECTID.INDEX, "INDEX", "インデックス", colInfo, contextID, contextName, sprtType)

        'テーブルパーティション
        colInfo = New String() {"テーブル名", "パーティション名", "サブパーティション数", "表領域名", "サイズ(KB)", "エクステント数", "統計情報採取時間", "統計情報件数", "統計情報平均行サイズ(B)", "実件数"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.NUMBER, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.NUMBER}
        contextID = New String() {"CNT", "CP", "TRUNC", "DROP", "STATS"}
        contextName = New String() {"件数取得", "コピー", "TRUNCATE", "DROP", "統計取得"}
        C_OBJECT(C_OBJECTID.TABLE_PARTITION).SetObject(C_OBJECTID.TABLE_PARTITION, "TABLE PARTITION", "テーブルパーティション", colInfo, contextID, contextName, sprtType)

        'テーブルサブパーティション
        colInfo = New String() {"テーブル名", "パーティション名", "サブパーティション名", "表領域名", "サイズ(KB)", "エクステント数", "統計情報採取時間", "統計情報件数", "統計情報平均行サイズ(B)", "実件数"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.NUMBER}
        contextID = New String() {"CNT", "CP", "TRUNC", "DROP", "STATS"}
        contextName = New String() {"件数取得", "コピー", "TRUNCATE", "DROP", "統計取得"}
        C_OBJECT(C_OBJECTID.TABLE_SUBPARTITION).SetObject(C_OBJECTID.TABLE_SUBPARTITION, "TABLE SUBPARTITION", "テーブルサブパーティション", colInfo, contextID, contextName, sprtType)

        'インデックスパーティション
        colInfo = New String() {"インデックス名", "パーティション名", "サブパーティション数", "表領域名", "サイズ(KB)", "エクステント数", "統計情報採取時間", "統計情報件数", "索引の深さ", "リーフ・ブロック数"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.NUMBER, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.NUMBER}
        contextID = New String() {"CP", "DROP", "REBUILD", "STATS"}
        contextName = New String() {"コピー", "DROP", "REBUILD", "統計取得"}
        C_OBJECT(C_OBJECTID.INDEX_PARTITION).SetObject(C_OBJECTID.INDEX_PARTITION, "INDEX PARTITION", "インデックスパーティション", colInfo, contextID, contextName, sprtType)

        'インデックスサブパーティション
        colInfo = New String() {"インデックス名", "パーティション名", "サブパーティション名", "表領域名", "サイズ(KB)", "エクステント数", "統計情報採取時間", "統計情報件数", "索引の深さ", "リーフ・ブロック数"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.NUMBER}
        contextID = New String() {"CP", "DROP", "REBUILD", "STATS"}
        contextName = New String() {"コピー", "DROP", "REBUILD", "統計取得"}
        C_OBJECT(C_OBJECTID.INDEX_SUBPARTITION).SetObject(C_OBJECTID.INDEX_SUBPARTITION, "INDEX SUBPARTITION", "インデックスサブパーティション", colInfo, contextID, contextName, sprtType)

        'マテリアライズドビュー
        colInfo = New String() {"MV名", "最終リフレッシュメソッド", "最終リフレッシュ時刻", "表領域名", "エクステント数", "サイズ(KB)", "クエリー", "更新可能", "リフレッシュモード", "リフレッシュメソッド"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CNT", "CP", "TRUNC", "DROP", "STATS"}
        contextName = New String() {"件数取得", "コピー", "TRUNCATE", "DROP", "統計取得"}
        C_OBJECT(C_OBJECTID.MATERIALIZED_VIEW).SetObject(C_OBJECTID.MATERIALIZED_VIEW, "MATERIALIZED VIEW", "マテリアライズドビュー", colInfo, contextID, contextName, sprtType)

        'LOB
        colInfo = New String() {"テーブル名", "列名", "LOB名", "表領域名", "サイズ(KB)", "エクステント数"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.LOB).SetObject(C_OBJECTID.LOB, "LOBSEGMENT", "ラージオブジェクト", colInfo, contextID, contextName, sprtType)

        'LOB PARTITION
        colInfo = New String() {"テーブル名", "列名", "LOB名", "パーティション名", "LOBパーティション名", "表領域名", "サイズ(KB)", "エクステント数"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.LOB_PARTITION).SetObject(C_OBJECTID.LOB_PARTITION, "LOB PARTITION", "LOBパーティション", colInfo, contextID, contextName, sprtType)

        'LOB SUBPARTITION
        colInfo = New String() {"テーブル名", "列名", "LOB名", "LOBパーティション名", "サブパーティション名", "LOBサブパーティション名", "表領域名", "サイズ(KB)", "エクステント数"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.LOB_SUBPARTITION).SetObject(C_OBJECTID.LOB_SUBPARTITION, "LOB SUBPARTITION", "LOBサブパーティション", colInfo, contextID, contextName, sprtType)

        'シーケンス
        colInfo = New String() {"名称", "最後のキャッシュ値", "増加値", "最小値", "最大値", "サイクル", "整列", "キャッシュ値"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.NUMBER}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.SEQUENCE).SetObject(C_OBJECTID.SEQUENCE, "SEQUENCE", "シーケンス", colInfo, contextID, contextName, sprtType)

        'プロシージャ
        colInfo = New String() {"名称", "状態"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.PROCEDURE).SetObject(C_OBJECTID.PROCEDURE, "PROCEDURE", "プロシージャ", colInfo, contextID, contextName, sprtType)

        'ファンクション
        colInfo = New String() {"名称", "状態"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.FUNCTION).SetObject(C_OBJECTID.FUNCTION, "FUNCTION", "ファンクション", colInfo, contextID, contextName, sprtType)

        'パッケージ(仕様部)
        colInfo = New String() {"名称", "状態"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.PACKAGE).SetObject(C_OBJECTID.PACKAGE, "PACKAGE", "パッケージ(仕様部)", colInfo, contextID, contextName, sprtType)

        'パッケージ(本体部)
        colInfo = New String() {"名称", "状態"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.PACKAGE_BODY).SetObject(C_OBJECTID.PACKAGE_BODY, "PACKAGE BODY", "パッケージ(本体部)", colInfo, contextID, contextName, sprtType)

        'JAVAクラス
        colInfo = New String() {"名称", "状態"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.JAVA_CLASS).SetObject(C_OBJECTID.JAVA_CLASS, "JAVA CLASS", "JAVAクラス", colInfo, contextID, contextName, sprtType)

        'JAVAソース
        colInfo = New String() {"名称", "状態"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.JAVA_SOURCE).SetObject(C_OBJECTID.JAVA_SOURCE, "JAVA SOURCE", "JAVAソース", colInfo, contextID, contextName, sprtType)

        'トリガー
        colInfo = New String() {"名称", "状態", "トリガータイプ", "トリガーイベント", "ベースオーナー", "ベースタイプ", "ベースオブジェクト", "列名"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.TRIGGER).SetObject(C_OBJECTID.TRIGGER, "TRIGGER", "トリガー", colInfo, contextID, contextName, sprtType)

        'シノニム
        colInfo = New String() {"名称", "テーブルオーナー", "テーブル名", "DBリンク"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.SYNONYM).SetObject(C_OBJECTID.SYNONYM, "SYNONYM", "シノニム", colInfo, contextID, contextName, sprtType)

        'DBリンク
        colInfo = New String() {"名称", "接続先", "接続先ユーザ名"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.DATABASE_LINK).SetObject(C_OBJECTID.DATABASE_LINK, "DATABASE LINK", "データベースリンク", colInfo, contextID, contextName, sprtType)

        'タイプ
        colInfo = New String() {"名称"}
        sprtType = New Integer() {C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.TYPE).SetObject(C_OBJECTID.TYPE, "TYPE", "タイプ", colInfo, contextID, contextName, sprtType)

        'ライブラリ
        colInfo = New String() {"名称", "状態", "動的", "ファイル"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.LIBRARY).SetObject(C_OBJECTID.LIBRARY, "LIBRARY", "ライブラリ", colInfo, contextID, contextName, sprtType)

        'クラスタ
        colInfo = New String() {"名称", "表領域名", "サイズ(KB)", "エクステント数"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.NUMBER, C_SORT.NUMBER}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.CLUSTER).SetObject(C_OBJECTID.CLUSTER, "CLUSTER", "クラスタ", colInfo, contextID, contextName, sprtType)

        'ディクショナリ
        colInfo = New String() {"ディクショナリ名", "コメント"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.DICTIONARY).SetObject(C_OBJECTID.DICTIONARY, "DICTIONARY", "ディクショナリ", colInfo, contextID, contextName, sprtType)

        '動的パフォーマンスビュー
        colInfo = New String() {"動的パフォーマンスビュー名", "コメント"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP"}
        contextName = New String() {"コピー"}
        C_OBJECT(C_OBJECTID.V_VIEW).SetObject(C_OBJECTID.V_VIEW, "V_VIEW", "動的パフォーマンスビュー", colInfo, contextID, contextName, sprtType)

        'スキーマ
        colInfo = New String() {"ユーザ名", "アカウントステータス", "ロック時刻", "期限切れ時刻", "デフォルト表領域", "デフォルト一時表領域", "作成日", "プロファイル", "コンシューマグループ", "外部名"}
        sprtType = New Integer() {C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING, C_SORT.STRING}
        contextID = New String() {"CP", "STATS"}
        contextName = New String() {"コピー", "統計取得"}
        C_OBJECT(C_OBJECTID.ALL_SCHEMA).SetObject(C_OBJECTID.ALL_SCHEMA, "ALL_SCHEMA", "スキーマ", colInfo, contextID, contextName, sprtType)

    End Sub

    Public Shared Sub SetMessage()
        'メッセージ定義
        'C_MESSAGE(1).SetMessage("I0001", "表示しました", "表示しました")
        'C_MESSAGE(2).SetMessage("I0002", "接続中・・・", "接続中・・・")
        'C_MESSAGE(3).SetMessage("I0003", "接続成功", "接続成功")
        'C_MESSAGE(4).SetMessage("I0004", "接続失敗", "接続失敗")
        'C_MESSAGE(5).SetMessage("I0005", "実行中(サーバからの応答待ち)・・・", "実行中(サーバからの応答待ち)・・・")
        'C_MESSAGE(6).SetMessage("I0006", "実行中(フェッチ)・・・", "実行中(フェッチ)・・・")
        'C_MESSAGE(7).SetMessage("I0007", "%1件読み込み中・・・(Escでキャンセル)", "%1件読み込み中・・・(Escでキャンセル)")
        'C_MESSAGE(8).SetMessage("I0008", "キャンセルしました", "キャンセルしました")
        'C_MESSAGE(9).SetMessage("I0009", "%1件読み込みました", "%1件読み込みました")
        'C_MESSAGE(10).SetMessage("I0010", "%1件処理しました", "%1件処理しました")
        'C_MESSAGE(11).SetMessage("I0011", "処理しました", "処理しました")
        'C_MESSAGE(12).SetMessage("I0012", "ファイルを出力しました", "ファイルを出力しました")
        'C_MESSAGE(13).SetMessage("I0013", "中止しました", "中止しました")
        'C_MESSAGE(14).SetMessage("I0014", "データ取得中", "データ取得中")
        'C_MESSAGE(15).SetMessage("I0015", "クリップボードにコピーしています", "クリップボードにコピーしています")
        'C_MESSAGE(16).SetMessage("I0016", "クリップボードにコピーしました", "クリップボードにコピーしました")
        'C_MESSAGE(17).SetMessage("I0017", "呼出中・・・", "呼出中・・・")
        'C_MESSAGE(18).SetMessage("I0018", "件数取得終了", "件数取得終了")
        'C_MESSAGE(19).SetMessage("I0019", "ログインユーザ:%1 接続文字列:%2", "ログインユーザ:%1 接続文字列:%2,")
        'C_MESSAGE(20).SetMessage("I0020", "セッションを切断しました", "セッションを切断しました")
        'C_MESSAGE(21).SetMessage("I0021", "有効にしました", "有効にしました")
        'C_MESSAGE(22).SetMessage("I0022", "無効にしました", "無効にしました")
        'C_MESSAGE(23).SetMessage("I0023", "SQLトレースを有効にする場合は「はい」、無効にする場合は「いいえ」を押してください", "SQLトレースを有効にする場合は「はい」、無効にする場合は「いいえ」を押してください")
        'C_MESSAGE(24).SetMessage("I0024", "%1件更新しました", "%1件更新しました")
        'C_MESSAGE(25).SetMessage("I0025", "コンフィグファイルを更新しました", "コンフィグファイルを更新しました")
        'C_MESSAGE(26).SetMessage("I0026", "%1件中%2件ロード済み・・・(ESCでキャンセル)", "%1件中%2件ロード済み・・・(ESCでキャンセル)")
        'C_MESSAGE(27).SetMessage("I0027", "実行中・・・(ESCでキャンセル)", "実行中・・・(ESCでキャンセル)")
        'C_MESSAGE(28).SetMessage("I0028", "一時表(%1)を作成しました", "一時表(%1)を作成しました")
        'C_MESSAGE(29).SetMessage("I0029", "実行中・・・", "実行中・・・")
        'C_MESSAGE(30).SetMessage("I0030", "実行しました", "実行しました")
        'C_MESSAGE(31).SetMessage("I0031", "バインドしました", "バインドしました")
        'C_MESSAGE(32).SetMessage("I0032", "%1byte出力されました", "%1byte出力されました")
        'C_MESSAGE(33).SetMessage("I0033", "表示中・・・", "表示中・・・")
        'C_MESSAGE(34).SetMessage("I0034", "出力の取り出し中・・・", "出力の取り出し中・・・")
        'C_MESSAGE(35).SetMessage("I0035", "結果セットが破棄されたため再開できませんでした", "結果セットが破棄されたため再開できませんでした")
        'C_MESSAGE(36).SetMessage("I0036", "フェッチを中断しました", "フェッチを中断しました")
        'C_MESSAGE(37).SetMessage("I0037", "設定を更新しました", "設定を更新しました")

        'C_MESSAGE(101).SetMessage("W0001", "未表示の行が存在します。全件取得後にコピーしますか？", "未表示の行が存在します。全件取得後にコピーしますか？")
        'C_MESSAGE(102).SetMessage("W0002", "選択されたオブジェクトをTRUNCATEします。本当によろしいですか？", "選択されたオブジェクトをTRUNCATEします。本当によろしいですか？")
        'C_MESSAGE(103).SetMessage("W0003", "選択されたオブジェクトをDROPします。本当によろしいですか？", "選択されたオブジェクトをDROPします。本当によろしいですか？")
        'C_MESSAGE(104).SetMessage("W0004", "選択行のセッションをKILLします。本当によろしいですか？", "選択行のセッションをKILLします。本当によろしいですか？")
        'C_MESSAGE(105).SetMessage("W0005", "選択されたオブジェクトの統計情報を取得します。本当によろしいですか？", "選択されたオブジェクトの統計情報を取得します。本当によろしいですか？")

        'C_MESSAGE(201).SetMessage("E0001", "処理が失敗しました", "処理が失敗しました")
        'C_MESSAGE(202).SetMessage("E0002", "ファイル(%1)が存在しません", "ファイル(%1)が存在しません")
        'C_MESSAGE(203).SetMessage("E0003", "一時表の作成に失敗しました", "一時表の作成に失敗しました")
        'C_MESSAGE(204).SetMessage("E0004", "データ作成に失敗しました", "データ作成に失敗しました")
        'C_MESSAGE(205).SetMessage("E0005", "データがありません", "データがありません")
        'C_MESSAGE(206).SetMessage("E0006", "入力した値は最大値を超えています", "入力した値は最大値を超えています")
        'C_MESSAGE(207).SetMessage("E0007", "セッションが選択されていません", "セッションが選択されていません")
        'C_MESSAGE(208).SetMessage("E0008", "SQL文が入力されていません", "SQL文が入力されていません")
        'C_MESSAGE(209).SetMessage("E0009", "処理が失敗しました。dbms_systemの実行権限がない可能性があります" & vbCrLf & " 権限を付与するには以下を実行します" & vbCrLf & "   grant all on sys.dbms_system to <ユーザ名>", "処理が失敗しました。dbms_systemの実行権限がない可能性があります" & vbCrLf & " 権限を付与するには以下を実行します" & vbCrLf & "   grant all on sys.dbms_system to <ユーザ名>")
        'C_MESSAGE(210).SetMessage("E0010", "抽出条件が選択されていません", "抽出条件が選択されていません")
        'C_MESSAGE(211).SetMessage("E0011", "バインドが失敗しました", "バインドが失敗しました")
        'C_MESSAGE(212).SetMessage("E0012", "値は%1以上である必要があります", "値は%1以上である必要があります")
        'C_MESSAGE(213).SetMessage("E0013", "値は%1以下である必要があります", "値は%1以下である必要があります")
        'C_MESSAGE(214).SetMessage("E0014", "値が短すぎます", "値が短すぎます")
        'C_MESSAGE(215).SetMessage("E0015", "値が長すぎます", "値が長すぎます")
        'C_MESSAGE(216).SetMessage("E0016", "値をInt32型に変換できません", "値をInt32型に変換できません")
        'C_MESSAGE(217).SetMessage("E0017", "使用できない文字ば含まれています", "使用できない文字が含まれています")
        'C_MESSAGE(218).SetMessage("E0018", "ディレクトリ(%1)が作成できませんでした", "ディレクトリ(%1)が作成できませんでした")
        'C_MESSAGE(219).SetMessage("E0019", "指定したディレクトリ(%1)は存在しません", "指定したディレクトリ(%1)は存在しません")
        'C_MESSAGE(220).SetMessage("E0020", "一度に選択可能なセッション数は2つまでです", "一度に選択可能なセッション数は2つまでです")
        'C_MESSAGE(221).SetMessage("E0021", "指定したセッションは現在存在しません", "指定したセッションは現在存在しません")


        C_MESSAGE_COLLECTION.Add("I0001", "表示しました")
        C_MESSAGE_COLLECTION.Add("I0002", "接続中・・・")
        C_MESSAGE_COLLECTION.Add("I0003", "接続成功")
        C_MESSAGE_COLLECTION.Add("I0004", "接続失敗")
        C_MESSAGE_COLLECTION.Add("I0005", "実行中(サーバからの応答待ち)・・・")
        C_MESSAGE_COLLECTION.Add("I0006", "実行中(フェッチ)・・・")
        C_MESSAGE_COLLECTION.Add("I0007", "%1件読み込み中・・・(Escでキャンセル)")
        C_MESSAGE_COLLECTION.Add("I0008", "キャンセルしました")
        C_MESSAGE_COLLECTION.Add("I0009", "%1件読み込みました")
        C_MESSAGE_COLLECTION.Add("I0010", "%1件処理しました")
        C_MESSAGE_COLLECTION.Add("I0011", "処理しました")
        C_MESSAGE_COLLECTION.Add("I0012", "ファイルを出力しました")
        C_MESSAGE_COLLECTION.Add("I0013", "中止しました")
        C_MESSAGE_COLLECTION.Add("I0014", "データ取得中")
        C_MESSAGE_COLLECTION.Add("I0015", "クリップボードにコピーしています")
        C_MESSAGE_COLLECTION.Add("I0016", "クリップボードにコピーしました")
        C_MESSAGE_COLLECTION.Add("I0017", "呼出中・・・")
        C_MESSAGE_COLLECTION.Add("I0018", "件数取得終了")
        C_MESSAGE_COLLECTION.Add("I0019", "ログインユーザ:%1 接続文字列:%2")
        C_MESSAGE_COLLECTION.Add("I0020", "セッションを切断しました")
        C_MESSAGE_COLLECTION.Add("I0021", "有効にしました")
        C_MESSAGE_COLLECTION.Add("I0022", "無効にしました")
        C_MESSAGE_COLLECTION.Add("I0023", "SQLトレースを有効にする場合は「はい」、無効にする場合は「いいえ」を押してください")
        C_MESSAGE_COLLECTION.Add("I0024", "%1件更新しました")
        C_MESSAGE_COLLECTION.Add("I0025", "コンフィグファイルを更新しました")
        C_MESSAGE_COLLECTION.Add("I0026", "%1件中%2件ロード済み・・・(ESCでキャンセル)")
        C_MESSAGE_COLLECTION.Add("I0027", "実行中・・・(ESCでキャンセル)")
        C_MESSAGE_COLLECTION.Add("I0028", "一時表(%1)を作成しました")
        C_MESSAGE_COLLECTION.Add("I0029", "実行中・・・")
        C_MESSAGE_COLLECTION.Add("I0030", "実行しました")
        C_MESSAGE_COLLECTION.Add("I0031", "バインドしました")
        C_MESSAGE_COLLECTION.Add("I0032", "%1byte出力されました")
        C_MESSAGE_COLLECTION.Add("I0033", "表示中・・・")
        C_MESSAGE_COLLECTION.Add("I0034", "出力の取り出し中・・・")
        C_MESSAGE_COLLECTION.Add("I0035", "結果セットが破棄されたため再開できませんでした")
        C_MESSAGE_COLLECTION.Add("I0036", "フェッチを中断しました")
        C_MESSAGE_COLLECTION.Add("I0037", "設定を更新しました")

        C_MESSAGE_COLLECTION.Add("W0001", "未表示の行が存在します。全件取得後にコピーしますか？")
        C_MESSAGE_COLLECTION.Add("W0002", "選択されたオブジェクトをTRUNCATEします。本当によろしいですか？")
        C_MESSAGE_COLLECTION.Add("W0003", "選択されたオブジェクトをDROPします。本当によろしいですか？")
        C_MESSAGE_COLLECTION.Add("W0004", "選択行のセッションをKILLします。本当によろしいですか？")
        C_MESSAGE_COLLECTION.Add("W0005", "選択されたオブジェクトの統計情報を取得します。本当によろしいですか？")

        C_MESSAGE_COLLECTION.Add("E0001", "処理が失敗しました")
        C_MESSAGE_COLLECTION.Add("E0002", "ファイル(%1)が存在しません")
        C_MESSAGE_COLLECTION.Add("E0003", "一時表の作成に失敗しました")
        C_MESSAGE_COLLECTION.Add("E0004", "データ作成に失敗しました")
        C_MESSAGE_COLLECTION.Add("E0005", "データがありません")
        C_MESSAGE_COLLECTION.Add("E0006", "入力した値は最大値を超えています")
        C_MESSAGE_COLLECTION.Add("E0007", "セッションが選択されていません")
        C_MESSAGE_COLLECTION.Add("E0008", "SQL文が入力されていません")
        C_MESSAGE_COLLECTION.Add("E0009", "処理が失敗しました。dbms_systemの実行権限がない可能性があります" & vbCrLf & " 権限を付与するには以下を実行します" & vbCrLf & "   grant all on sys.dbms_system to <ユーザ名>")
        C_MESSAGE_COLLECTION.Add("E0010", "抽出条件が選択されていません")
        C_MESSAGE_COLLECTION.Add("E0011", "バインドが失敗しました")
        C_MESSAGE_COLLECTION.Add("E0012", "値は%1以上である必要があります")
        C_MESSAGE_COLLECTION.Add("E0013", "値は%1以下である必要があります")
        C_MESSAGE_COLLECTION.Add("E0014", "値が短すぎます")
        C_MESSAGE_COLLECTION.Add("E0015", "値が長すぎます")
        C_MESSAGE_COLLECTION.Add("E0016", "値をInt32型に変換できません")
        C_MESSAGE_COLLECTION.Add("E0017", "使用できない文字ば含まれています")
        C_MESSAGE_COLLECTION.Add("E0018", "ディレクトリ(%1)が作成できませんでした")
        C_MESSAGE_COLLECTION.Add("E0019", "指定したディレクトリ(%1)は存在しません")
        C_MESSAGE_COLLECTION.Add("E0020", "一度に選択可能なセッション数は2つまでです")
        C_MESSAGE_COLLECTION.Add("E0021", "指定したセッションは現在存在しません")

    End Sub

    Public Shared Sub SetDefaultInitParam()
        'コンフィグファイルのデフォルト値
        C_INITPARAM(0).SET_INITPARAM(C_INITPARAM_INIT_APLOG, "アプリケーションログのロギング設定(0:無効 1:有効)", "1")
        C_INITPARAM(1).SET_INITPARAM(C_INITPARAM_INIT_ERRLOG, "エラーログのロギング設定(0:無効 1:有効)", "1")
        C_INITPARAM(2).SET_INITPARAM(C_INITPARAM_INIT_FETCH, "フェッチを中断するまでの行数", "1000000")
        C_INITPARAM(3).SET_INITPARAM(C_INITPARAM_INIT_FETCH_REFLESH, "フェッチ中の画面リフレッシュ間隔", "1000")
        C_INITPARAM(4).SET_INITPARAM(C_INITPARAM_INIT_ENCODE, "出力データのエンコーディング(" & C_ENCODE.UTF8 & ":UTF8 " & C_ENCODE.SJIS & ":SHIFT-JIS " & C_ENCODE.EUC & ":EUC)", "1")
        C_INITPARAM(5).SET_INITPARAM(C_INITPARAM_INIT_DELIMITER, "出力データの区切り文字(0:カンマ 1:タブ 2:半角空白)", "1")
        C_INITPARAM(6).SET_INITPARAM(C_INITPARAM_INIT_ENCLOSURE, "出力データの囲い文字(0:なし 1:ダブルクォーテーション 2:シングルクォーテーション)", "1")
        C_INITPARAM(7).SET_INITPARAM(C_INITPARAM_INIT_LOGDIR, "ログ出力ディレクトリ(なしの場合は<アプリケーションパス>\log)", vbNullString)
        C_INITPARAM(8).SET_INITPARAM(C_INITPARAM_INIT_ODP_FETCHSIZE, "ODP.NETのフェッチサイズ(B)", 1048576)
        C_INITPARAM(9).SET_INITPARAM(C_INITPARAM_INIT_ODP_LONGFETCHSIZE, "ODP.NETのLONG型のフェッチサイズ(B)", 4096)
        C_INITPARAM(10).SET_INITPARAM(C_INITPARAM_INIT_NLS_LANG, "環境変数NLS_LANGの値(空白の場合は変更しない)", "JAPANESE_JAPAN.JA16SJIS")
        C_INITPARAM(11).SET_INITPARAM(C_INITPARAM_INIT_ONLINE, "オンラインモード設定(1:オンライン 0:オフライン)", "1")
    End Sub

    Private Shared Sub SetMiddleware()
        'ミドルウエアの識別値
        C_MIDDLE(0).SetMiddleware(C_MIDDLE_TYPE.ODP, "ODP.NET")
        C_MIDDLE(1).SetMiddleware(C_MIDDLE_TYPE.MS_CL, "MS_CLIENT")
        'C_MIDDLE(2).SetMiddleware(C_MIDDLE_ODBC, "ODBC")　　　'OO4OとODBCは当面封印
        'C_MIDDLE(3).SetMiddleware(C_MIDDLE_OO4O, "OO4O")
    End Sub

End Class
