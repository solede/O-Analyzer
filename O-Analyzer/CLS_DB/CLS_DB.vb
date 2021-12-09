Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_FILE = CLS_FILE.CLS_FILE
Imports System.Windows.Forms

'234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890
Public Class CLS_DB
    '#######################################################################
    'DB操作クラス
    '#######################################################################

    'グローバル変数
    Public G_CONN As Object                   'DBコネクション
    Public G_SELECT_DICTIONARY_FLG As Boolean 'ディクショナリ参照権限
    Public G_ORA_VERSION(5) As String         'DBのバージョン(配列0はフル桁(10.2.0.1.0等)、1～5は各桁)
    Public G_USERS As New Collection          'データベースのユーザ一覧

    '各ミドルウエアのクラスインスタンス
    Private M_ODP As CLS_ODP.CLS_ODP
    Private M_OO4O As CLS_OO4O.CLS_OO4O
    Private M_MS_CL As CLS_MS_CL.CLS_MS_CL
    Private M_ODBC As CLS_ODBC.CLS_ODBC

    Private M_DATA_TABLE As New DataTable  'データテーブル
    Private M_DATASET As New DataSet       'データセット

    'モジュール変数
    Private M_MIDDLEWARE As Short          '接続に使用しているミドルウエア
    Private M_READED_ROW_COUNT As Long     '読み込み済行数
    Private M_COL_COUNT As Integer         '列数
    Private M_READ_ROW_COUNT As Long       '読み込みを行う行数
    Private M_SELECT_FLG As Boolean        'SELECT中フラグ
    Private M_SET_READER_CNT As Integer = 0 'リーダーオブジェクトの初期化回数
    Private M_ADAPTER_FLG As Boolean       '非接続型フラグ
    Private M_LOGIN_USER As String         'ログインユーザ
    Private M_SID As Integer = -1          'SID

    'ゲッタとセッタ
    Public Function GET_COL_COUNT() As Long
        Return M_COL_COUNT
    End Function
    Sub SET_ADAPTER_FLG(ByVal adapterFlg As Boolean)
        M_ADAPTER_FLG = adapterFlg
    End Sub

    Function GET_ADAPTER_FLG() As Short
        GET_ADAPTER_FLG = M_ADAPTER_FLG
    End Function

    Sub SET_MIDDLEWARE(ByVal middleware As Integer)
        M_MIDDLEWARE = middleware
    End Sub

    Function GET_MIDDLEWARE() As Short
        GET_MIDDLEWARE = M_MIDDLEWARE
    End Function

    Function GET_ROW_COUNT() As Integer
        GET_ROW_COUNT = M_READED_ROW_COUNT
    End Function

    Public Function SET_SELECT_FLG(ByVal selectFlg As Boolean) As Boolean
        M_SELECT_FLG = selectFlg
        Return True
    End Function

    Public Function GET_SELECT_FLG() As Boolean
        Return M_SELECT_FLG
    End Function

    Public Function GET_SET_READER_CNT() As Integer
        Return M_SET_READER_CNT
    End Function

    Function DB_CONNECT(ByVal userID As String, ByVal pass As String, ByVal connStr As String, _
                        Optional ByVal connOption As String = "", _
                        Optional ByVal isSysdba As Boolean = False, _
                        Optional ByVal outputErr As Boolean = True,
                        Optional ByVal logName As String = vbNullString) As Boolean
        '#######################################################################
        '各ミドルウエアを使用するためのライブラリをロードし、データベースへ接続する
        '引数
        '    userID：ユーザID
        '    pass：パスワード
        '    connStr：接続文字列
        '    connOption:オプション文字列
        '    isSysdba:SYSDBA接続フラグ(TRUE:SYSDBAで接続する FALSE:通常接続)
        '    outputErr:エラーメッセージの画面出力フラグ
        '    logName:ログの名前
        '戻り値
        '    TRUE :接続成功
        '    FALSE:接続失敗
        '#######################################################################
        M_FILE.WriteApLog("接続を開始します。", , logName)
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            'DLLのロード
            Try
                M_ODP = New CLS_ODP.CLS_ODP
            Catch ex As Exception
                MsgBox(ex.Message)
                M_FILE.WriteErrLog(ex, , logName)
                Return (False)
            End Try

            If M_ODP.DB_CONNECT(userID, pass, connStr, connOption, isSysdba, outputErr, logName) Then
                '接続成功
                G_CONN = M_ODP.G_CONN2
            Else
                '接続失敗
                M_ODP = Nothing
                Return False
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            'DLLのロード
            Try
                M_OO4O = New CLS_OO4O.CLS_OO4O
            Catch ex As Exception
                MsgBox(ex.Message)
                M_FILE.WriteErrLog(ex, , logName)
                Return (False)
            End Try
            If M_OO4O.DB_CONNECT(userID, pass, connStr, connOption, isSysdba) Then
                '接続成功
                G_CONN = M_OO4O.G_ORADB
            Else
                '接続失敗
                M_OO4O = Nothing
                Return False
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            'DLLのロード
            Try
                M_MS_CL = New CLS_MS_CL.CLS_MS_CL
            Catch ex As Exception
                MsgBox(ex.Message)
                M_FILE.WriteErrLog(ex, , logName)
                Return False
            End Try
            If M_MS_CL.DB_CONNECT(userID, pass, connStr, connOption, isSysdba, outputErr, logName) Then
                '接続成功
                G_CONN = M_MS_CL.G_CONN2
            Else
                '接続失敗
                M_MS_CL = Nothing
                Return False
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            'DLLのロード
            Try
                M_ODBC = New CLS_ODBC.CLS_ODBC
            Catch ex As Exception
                MsgBox(ex.Message)
                M_FILE.WriteErrLog(ex, , logName)
                Return False
            End Try
            If M_ODBC.DB_CONNECT(userID, pass, connStr, connOption, isSysdba, outputErr, logName) Then
                '接続成功
                G_CONN = M_ODBC.G_CONN2
            Else
                '接続失敗
                M_ODBC = Nothing
                Return False
            End If
        End If

        'トランザクションセット
        SET_TRANSACTION()

        'logNameに値が入るのは負荷テスト時のみ。負荷テスト時は余計なSQLは発行しない
        If logName = vbNullString Then
            M_LOGIN_USER = vbNullString
            M_SID = -1
            '日付書式設定
            SET_NLS_DATE_FORMAT()
            'バージョン取得
            GET_VERSION()
            'ログインユーザ取得
            GET_LOGIN_USER()
            '権限情報
            GET_COMPETENCE()
        End If
        'SID取得
        GET_SID(logName)

        Return True
    End Function

    Sub DB_CLOSE(Optional ByVal logName As String = vbNullString)
        '#######################################################################
        'データベースへの接続をCLOSEする
        '#######################################################################
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            M_ODP.DB_CLOSE(logName)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            M_OO4O.DB_CLOSE()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            M_MS_CL.DB_CLOSE(logName)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            M_ODBC.DB_CLOSE(logName)
        End If
    End Sub

    Private Sub GET_VERSION()
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            G_ORA_VERSION = M_ODP.GET_VERSION()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            G_ORA_VERSION = M_OO4O.GET_VERSION()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            G_ORA_VERSION = M_MS_CL.GET_VERSION()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            G_ORA_VERSION = M_ODBC.GET_VERSION()
        End If
        M_FILE.WriteApLog("データベースのバージョン：" & G_ORA_VERSION(0))
    End Sub

    Private Sub GET_COMPETENCE()
        '#######################################################################
        '権限チェック
        '#######################################################################
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            G_SELECT_DICTIONARY_FLG = M_ODP.GET_COMPETENCE()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            G_SELECT_DICTIONARY_FLG = M_OO4O.GET_COMPETENCE()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            G_SELECT_DICTIONARY_FLG = M_MS_CL.GET_COMPETENCE()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            G_SELECT_DICTIONARY_FLG = M_ODBC.GET_COMPETENCE()
        End If
    End Sub

    Function GET_LOGIN_USER() As String
        '#######################################################################
        'ログインユーザ取得
        '  ログインユーザを戻す
        '戻り値
        '  ログインユーザ
        '#######################################################################
        Dim sql As String = "SELECT USER FROM DUAL"
        If M_LOGIN_USER <> vbNullString Then
            Return M_LOGIN_USER
        End If

        Using DataSet As New DataSet
            If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
                M_ODP.GET_DATASET(sql, DataSet)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
                M_OO4O.SET_ORACLEDYNASET(sql)
                DataSet.Tables.Add()
                M_OO4O.INIT_DATASET(DataSet, M_COL_COUNT, M_READED_ROW_COUNT)
                M_OO4O.GET_DATASET(M_READED_ROW_COUNT, DataSet, M_SELECT_FLG, M_COL_COUNT)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
                M_MS_CL.GET_DATASET(sql, DataSet)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
                M_ODBC.GET_DATASET(sql, DataSet)
            End If
            M_LOGIN_USER = DataSet.Tables(0).Rows(0).Item(0).ToString()
        End Using

        Return M_LOGIN_USER
    End Function

    Function GET_SID(Optional ByVal logName As String = vbNullString) As Integer
        '#######################################################################
        'SID取得
        '  SIDを戻す
        '戻り値
        '  SID
        '#######################################################################
        Dim sql As String = "SELECT SID FROM V$MYSTAT WHERE ROWNUM=1"
        Dim ret As Boolean

        If M_SID <> -1 Then
            Return M_SID
        End If

        Using dataSet As New DataSet
            If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
                ret = M_ODP.GET_DATASET(sql, dataSet, , False, logName)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
                M_OO4O.SET_ORACLEDYNASET(sql)
                dataSet.Tables.Add()
                M_OO4O.INIT_DATASET(dataSet, M_COL_COUNT, M_READED_ROW_COUNT)
                ret = M_OO4O.GET_DATASET(M_READED_ROW_COUNT, dataSet, M_SELECT_FLG, M_COL_COUNT)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
                ret = M_MS_CL.GET_DATASET(sql, dataSet, , False, logName)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
                ret = M_ODBC.GET_DATASET(sql, dataSet, , False, logName)
            End If
            If ret Then
                M_SID = dataSet.Tables(0).Rows(0).Item(0).ToString()
            Else
                'SIDが取得できなかった場合
                M_SID = vbNullString
            End If
        End Using

        Return M_SID
    End Function

    Private Sub SET_NLS_DATE_FORMAT()
        '#######################################################################
        'NLS_DATEフォーマットのセット(YYYY/MM/DD HH24:MI:SS)
        '#######################################################################
        Dim sql As String = "ALTER SESSION SET NLS_DATE_FORMAT = 'YYYY/MM/DD HH24:MI:SS'"

        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            M_ODP.EXEC_NONQUERY(sql, 0)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            M_OO4O.EXEC_NONQUERY(sql, 0)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            M_MS_CL.EXEC_NONQUERY(sql, 0)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            M_ODBC.EXEC_NONQUERY(sql, 0)
        End If
    End Sub

    Public Function SET_BINDING_SOURCE(ByRef dgv As DataGridView) As Boolean
        '#######################################################################
        'テーブルをDGVオブジェクトにセットする
        '引数
        '    dgv:設定するデータグリッドビューオブジェクト
        '戻り値
        '    TRUE:成功
        '    FALSE:失敗
        '#######################################################################
        Dim bindingSource1 As New BindingSource

        Try
            bindingSource1.DataSource = M_DATASET.Tables(0)
            dgv.DataSource = bindingSource1
            SET_BINDING_SOURCE = True
        Catch ex As Exception
            MsgBox(ex.Message)
            M_FILE.WriteErrLog(ex)
            M_DATASET = Nothing
            SET_BINDING_SOURCE = False
        End Try

    End Function

    Function GET_CONNECT_STRING() As String
        '#######################################################################
        '接続文字列取得
        '#######################################################################
        Dim connStr As String = vbNullString
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            connStr = M_ODP.GET_CONNECT_STRING()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            '未実装なので注意。。
            'connStr = M_OO4O.GET_CONNECT_STRING()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            connStr = M_MS_CL.GET_CONNECT_STRING()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            connStr = M_ODBC.GET_CONNECT_STRING()
        End If

        Return connStr
    End Function

    Public Sub INIT_DATA_TABLE(Optional ByVal W_OUTPUT_ERR As Boolean = True)
        '#######################################################################
        'データテーブルオブジェクト初期化
        '#######################################################################
        M_DATA_TABLE = New DataTable

        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            M_ODP.INIT_DATA_TABLE(M_DATA_TABLE, M_COL_COUNT, M_READED_ROW_COUNT, W_OUTPUT_ERR)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            M_OO4O.INIT_DATA_TABLE(M_DATA_TABLE, M_COL_COUNT, M_READED_ROW_COUNT)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            M_MS_CL.INIT_DATA_TABLE(M_DATA_TABLE, M_COL_COUNT, M_READED_ROW_COUNT, W_OUTPUT_ERR)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            M_ODBC.INIT_DATA_TABLE(M_DATA_TABLE, M_COL_COUNT, M_READED_ROW_COUNT, W_OUTPUT_ERR)
        End If
    End Sub

    Public Sub INIT_LOCAL_DATA_TABLE(ByRef dataTable As DataTable, Optional ByVal W_OUTPUT_ERR As Boolean = True)
        '#######################################################################
        'データテーブルオブジェクト初期化
        '#######################################################################

        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            M_ODP.INIT_DATA_TABLE(dataTable, M_COL_COUNT, M_READED_ROW_COUNT, W_OUTPUT_ERR)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            M_OO4O.INIT_DATA_TABLE(dataTable, M_COL_COUNT, M_READED_ROW_COUNT)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            M_MS_CL.INIT_DATA_TABLE(dataTable, M_COL_COUNT, M_READED_ROW_COUNT, W_OUTPUT_ERR)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            M_ODBC.INIT_DATA_TABLE(dataTable, M_COL_COUNT, M_READED_ROW_COUNT, W_OUTPUT_ERR)
        End If
    End Sub

    Public Sub INIT_DATASET(ByRef dataset1 As DataSet)
        '#######################################################################
        'データセットオブジェクト初期化
        '#######################################################################
        M_DATASET = dataset1
        M_DATASET.Tables.Add()
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            'M_ODP.INIT_DATA_TABLE(M_DATA_TABLE, M_COL_COUNT, M_ROW_COUNT)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            M_OO4O.INIT_DATASET(M_DATASET, M_COL_COUNT, M_READED_ROW_COUNT)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            'M_MS_CL.INIT_DATA_TABLE(M_DATA_TABLE, M_COL_COUNT, M_ROW_COUNT)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            'M_ODBC.INIT_DATA_TABLE(M_DATA_TABLE, M_COL_COUNT, M_ROW_COUNT)
        End If
    End Sub

    Public Function GET_DATA_ADAPTER(ByVal sql As String, Optional ByVal timeoutValue As Integer = 0) As Boolean
        '#######################################################################
        'SQL文を実行し、データアダプタオブジェクトを取得する
        '引数
        '    sql:SQL文
        '    timeoutValue:タイムアウト値(秒)
        '戻り値
        '    TRUE :成功
        '    FALSE:失敗
        '#######################################################################
        M_ADAPTER_FLG = True
        M_SELECT_FLG = True
        M_DATASET = New DataSet
        M_READED_ROW_COUNT = 0

        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            Return M_ODP.GET_DATA_ADAPTER(sql, timeoutValue)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            'Return M_OO4O.GET_DATA_ADAPTER(W_SQL, W_TIMEOUT_VALUE)
            Return False
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            Return M_MS_CL.GET_DATA_ADAPTER(sql, timeoutValue)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            Return M_ODBC.GET_DATA_ADAPTER(sql, timeoutValue)
        Else
            Return False
        End If

    End Function

    Public Function GET_DATASET(ByVal sql As String, ByRef dataset1 As DataSet, _
                                Optional ByVal timeoutValue As Integer = 0, Optional ByVal isDisplayError As Boolean = True) As Boolean
        '#######################################################################
        'データセットオブジェクトを取得する
        '引数
        '    sql:SQL文
        '    dataset1:データセットオブジェクト
        '    timeoutValue:タイムアウト値(秒)
        '    isDisplayError:エラーの表示有無
        '戻り値
        '    TRUE :成功
        '    FALSE:失敗
        '#######################################################################

        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            Return M_ODP.GET_DATASET(sql, dataset1, timeoutValue, isDisplayError)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            M_OO4O.SET_ORACLEDYNASET(sql, isDisplayError)
            M_DATASET = dataset1
            M_DATASET.Tables.Add()
            M_OO4O.INIT_DATASET(M_DATASET, M_COL_COUNT, M_READED_ROW_COUNT)
            Return M_OO4O.GET_DATASET(M_READED_ROW_COUNT, dataset1, M_SELECT_FLG, M_COL_COUNT, 10000000000)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            Return M_MS_CL.GET_DATASET(sql, dataset1, timeoutValue, isDisplayError)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            Return M_ODBC.GET_DATASET(sql, dataset1, timeoutValue, isDisplayError)
        End If
        Return False
    End Function

    Public Function SET_RESULTSET(ByVal sql As String, Optional ByVal timeoutValue As Integer = 0, _
                                  Optional ByVal isOutputError As Boolean = True, Optional ByVal logName As String = vbNullString) As Boolean
        '#######################################################################
        '結果セットオブジェクトの取得
        '引数
        '    sql:SQL文
        '    timeoutValue:タイムアウト値(秒)(対応していないミドルウエアでは無視)
        '    isDisplayError:エラーの表示有無
        '戻り値
        '    TRUE :成功
        '    FALSE:失敗
        '#######################################################################
        M_DATASET = Nothing
        M_ADAPTER_FLG = False
        M_SELECT_FLG = True
        M_READED_ROW_COUNT = 0
        M_SET_READER_CNT = M_SET_READER_CNT + 1

        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            If M_ODP.SET_ORACLEDATAREADER(sql, timeoutValue, isOutputError, logName) Then
                Return True
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            If M_OO4O.SET_ORACLEDYNASET(sql) Then
                Return True
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            If M_MS_CL.SET_ORACLEDATAREADER(sql, timeoutValue, isOutputError, logName) Then
                Return True
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            If M_ODBC.SET_ODBCDATAREADER(sql, timeoutValue, isOutputError, logName) Then
                Return True
            End If
        End If
        Return False

    End Function

    Public Function FETCH_TO_DATATABLE(ByRef dataTable1 As DataTable, Optional ByVal fetchCnt As Long = 10000000000, Optional ByVal minDispCnt As Long = 0, _
                                       Optional ByVal maxDispCnt As Long = 0, Optional ByVal isDisplayError As Boolean = True, Optional ByVal logName As String = vbNullString) As Boolean
        '#######################################################################
        'リザルトセットからフェッチしたデータをデータテーブルオブジェクトで戻す
        '引数
        '    dataTable1:データテーブル(参照渡し)
        '    fetchCnt:フェッチ行数
        '    minDispCnt:最小表示件数
        '    maxDispCnt:最大表示件数
        '戻り値
        '    TRUE :成功
        '    FALSE:失敗
        '#######################################################################
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            If M_ODP.FETCH_TO_DATATABLE(M_READED_ROW_COUNT, M_DATA_TABLE, M_SELECT_FLG, M_COL_COUNT, fetchCnt, minDispCnt, maxDispCnt, isDisplayError, logName) Then
                dataTable1 = M_DATA_TABLE
            Else
                Return False
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            If M_OO4O.FETCH_TO_DATATABLE(M_READED_ROW_COUNT, M_DATA_TABLE, M_SELECT_FLG, M_COL_COUNT, fetchCnt, minDispCnt, maxDispCnt, isDisplayError) Then
                dataTable1 = M_DATA_TABLE
            Else
                Return False
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            If M_MS_CL.FETCH_TO_DATATABLE(M_READED_ROW_COUNT, M_DATA_TABLE, M_SELECT_FLG, M_COL_COUNT, fetchCnt, minDispCnt, maxDispCnt, isDisplayError, logName) Then
                dataTable1 = M_DATA_TABLE
            Else
                Return False
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            If M_ODBC.FETCH_TO_DATATABLE(M_READED_ROW_COUNT, M_DATA_TABLE, M_SELECT_FLG, M_COL_COUNT, fetchCnt, minDispCnt, maxDispCnt, isDisplayError, logName) Then
                dataTable1 = M_DATA_TABLE
            Else
                Return False
            End If
        End If

        Return True
    End Function

    Public Function FETCH_TO_LOCAL_DATATABLE(ByRef dataTable1 As DataTable, Optional ByVal fetchCnt As Long = 10000000000, Optional ByVal minDispCnt As Long = 0, _
                                   Optional ByVal maxDispCnt As Long = 0, Optional ByVal isDisplayError As Boolean = True) As Boolean
        '#######################################################################
        'リザルトセットからフェッチしたデータをデータテーブルオブジェクトで戻す
        '引数
        '    dataTable1:データテーブル(参照渡し)
        '    fetchCnt:フェッチ行数
        '    minDispCnt:最小表示件数
        '    maxDispCnt:最大表示件数
        '戻り値
        '    TRUE :成功
        '    FALSE:失敗
        '#######################################################################
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            If M_ODP.FETCH_TO_DATATABLE(M_READED_ROW_COUNT, dataTable1, M_SELECT_FLG, M_COL_COUNT, fetchCnt, minDispCnt, maxDispCnt, isDisplayError) = False Then
                Return False
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            If M_OO4O.FETCH_TO_DATATABLE(M_READED_ROW_COUNT, dataTable1, M_SELECT_FLG, M_COL_COUNT, fetchCnt, minDispCnt, maxDispCnt, isDisplayError) = False Then
                Return False
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            If M_MS_CL.FETCH_TO_DATATABLE(M_READED_ROW_COUNT, dataTable1, M_SELECT_FLG, M_COL_COUNT, fetchCnt, minDispCnt, maxDispCnt, isDisplayError) = False Then
                Return False
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            If M_ODBC.FETCH_TO_DATATABLE(M_READED_ROW_COUNT, dataTable1, M_SELECT_FLG, M_COL_COUNT, fetchCnt, minDispCnt, maxDispCnt, isDisplayError) = False Then
                Return False
            End If
        End If

        Return True
    End Function

    Public Function FETCH_ROW_TO_STRING(ByRef row As String(), Optional ByVal fetchCnt As Long = 10000000000) As Boolean
        '#######################################################################
        'データをフェッチし、フェッチした行データを文字列配列で戻す
        '引数
        '    row:行データ
        '    fetchCnt:フェッチ行数
        '戻り値
        '    TRUE :成功
        '    FALSE:失敗
        '#######################################################################
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            Return M_ODP.FETCH_TO_STRING(row, M_READED_ROW_COUNT, M_SELECT_FLG, M_COL_COUNT, fetchCnt)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            Return M_OO4O.FETCH_TO_STRING(row, M_READED_ROW_COUNT, M_SELECT_FLG, M_COL_COUNT, fetchCnt)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            Return M_MS_CL.FETCH_TO_STRING(row, M_READED_ROW_COUNT, M_SELECT_FLG, M_COL_COUNT, fetchCnt)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            Return M_ODBC.FETCH_TO_STRING(row, M_READED_ROW_COUNT, M_SELECT_FLG, M_COL_COUNT, fetchCnt)
        Else
            Return False
        End If

    End Function

    Public Function FETCH_DATA_DS(Optional ByVal fetchCnt As Long = 10000000000) As Boolean
        '#######################################################################
        'データセットオブジェクトのデータをセットする
        '引数
        '    fetchCnt:フェッチ行数
        '戻り値
        '    TRUE :成功
        '    FALSE:失敗
        '#######################################################################
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            If M_ODP.FETCH_TO_DATASET(M_DATASET, M_READED_ROW_COUNT, fetchCnt, "TABLE0", M_SELECT_FLG, M_READ_ROW_COUNT) Then
                FETCH_DATA_DS = True
            Else
                FETCH_DATA_DS = False
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            If M_OO4O.FETCH_TO_DATASET(M_DATASET, M_READED_ROW_COUNT, fetchCnt, "TABLE0", M_SELECT_FLG, M_READ_ROW_COUNT) Then
                FETCH_DATA_DS = True
            Else
                FETCH_DATA_DS = False
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            If M_MS_CL.FETCH_TO_DATASET(M_DATASET, M_READED_ROW_COUNT, fetchCnt, "TABLE0", M_SELECT_FLG, M_READ_ROW_COUNT) Then
                FETCH_DATA_DS = True
            Else
                FETCH_DATA_DS = False
            End If
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            If M_ODBC.FETCH_TO_DATASET(M_DATASET, M_READED_ROW_COUNT, fetchCnt, "TABLE0", M_SELECT_FLG, M_READ_ROW_COUNT) Then
                FETCH_DATA_DS = True
            Else
                FETCH_DATA_DS = False
            End If
        Else
            FETCH_DATA_DS = False
        End If

    End Function

    Public Function EXEC_NONQUERY(ByVal sqlOrg As String, ByRef returnRowCnt As Long, Optional ByVal paramStr As String() = Nothing, _
                                  Optional ByVal isDisplayError As Boolean = True, Optional ByVal timeoutValue As Integer = 0, Optional ByVal logName As String = vbNullString) As Boolean
        '#######################################################################
        '更新SQL実行
        '  SELECT以外の更新系SQLや無名プロシージャ等を実行する。
        '引数
        '  sqlOrg  :SQL文
        '  returnRowCnt:処理件数
        '  paramStr:パラメータ文字列
        '  isDisplayError:エラーの表示有無
        '  timeoutValue:タイムアウト値(秒)(対応していないミドルウエアでは無視)
        '#######################################################################
        'CRが無効な文字と判断される為置換
        Dim sql As String = sqlOrg.Replace(ControlChars.Cr, vbNullString)

        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            Return M_ODP.EXEC_NONQUERY(sql, returnRowCnt, paramStr, isDisplayError, timeoutValue, logName)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            '未実装
            MsgBox("未実装")
            Return M_OO4O.EXEC_NONQUERY(sql, returnRowCnt)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            Return M_MS_CL.EXEC_NONQUERY(sql, returnRowCnt, paramStr, isDisplayError, timeoutValue, logName)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            '未実装
            MsgBox("未実装")
            Return M_ODBC.EXEC_NONQUERY(sql, returnRowCnt, paramStr, isDisplayError, timeoutValue, logName)
        Else
            Return False
        End If
    End Function

    Public Function EXEC_PROCEDURE(ByVal sql As String) As Boolean
        '#######################################################################
        'プロシージャ実行
        '  プロシージャを実行する。
        '引数
        '  W_SQL  :SQL文
        '#######################################################################
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            Return M_ODP.EXEC_PROCEDURE(sql)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            Return M_OO4O.EXEC_PROCEDURE(sql)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            Return M_MS_CL.EXEC_PROCEDURE(sql)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            Return M_ODBC.EXEC_PROCEDURE(sql)
        Else
            Return False
        End If
    End Function

    Function EXEC_DBMS_SPACE_UNUSED_SPACE(ByVal owner As String, ByVal objectName As String, ByVal objectType As String, _
                                           ByVal partitionName As String, ByRef returnStr As String()) As Boolean
        '#######################################################################
        'DBMS_SPACE_UNUSED_SPACE実行
        '  DBMS_SPACE_UNUSED_SPACEの実行結果を文字列配列として戻す
        '引数
        '  owner        :オブジェクト所有スキーマ
        '　objectName   :オブジェクト名称
        '  objectType   :オブジェクトタイプ
        '  partitionName:パーティション名(パーティションオブジェクトの場合のみ)
        '  returnStr    :結果
        '   returnStr(0):合計ブロック数
        '   returnStr(1):合計バイト
        '   returnStr(2):合計未使用ブロック数
        '   returnStr(3):合計未使用ブロックバイト
        '   returnStr(4):最後に使用されたエクステントID
        '   returnStr(5):最後に使用されたブロックID
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################

        Dim sql As String = vbNullString
        Dim oraType(10) As Short
        Dim inOut(10) As ParameterDirection
        Dim paramName(10) As String
        Dim paramValue(10) As String
        Dim tmpStr(10) As String  'プロシージャに参照渡しする引数
        Dim bind(10) As M_CONST.S_BIND
        Dim isSuccess As Boolean = False

        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            sql = "BEGIN SYS.DBMS_SPACE.UNUSED_SPACE(SEGMENT_OWNER=>:SEGMENT_OWNER, SEGMENT_NAME=>:SEGMENT_NAME, " _
            & "SEGMENT_TYPE=>:SEGMENT_TYPE, TOTAL_BLOCKS=>:TOTAL_BLOCKS, TOTAL_BYTES=>:TOTAL_BYTES, UNUSED_BLOCKS=>:UNUSED_BLOCKS, " _
            & "UNUSED_BYTES=>:UNUSED_BYTES, LAST_USED_EXTENT_FILE_ID=>:LAST_USED_EXTENT_FILE_ID, LAST_USED_EXTENT_BLOCK_ID=>:LAST_USED_EXTENT_BLOCK_ID, " _
            & "LAST_USED_BLOCK=>:LAST_USED_BLOCK, PARTITION_NAME=>:PARTITION_NAME); end;"
        Else
            sql = "SYS.DBMS_SPACE.UNUSED_SPACE"
        End If

        ''パラメータ設定
        'パラメータ名
        paramName(0) = "SEGMENT_OWNER"
        paramName(1) = "SEGMENT_NAME"
        paramName(2) = "SEGMENT_TYPE"
        paramName(3) = "TOTAL_BLOCKS"
        paramName(4) = "TOTAL_BYTES"
        paramName(5) = "UNUSED_BLOCKS"
        paramName(6) = "UNUSED_BYTES"
        paramName(7) = "LAST_USED_EXTENT_FILE_ID"
        paramName(8) = "LAST_USED_EXTENT_BLOCK_ID"
        paramName(9) = "LAST_USED_BLOCK"
        paramName(10) = "PARTITION_NAME"
        'パラメータ値
        paramValue(0) = owner
        paramValue(1) = objectName
        paramValue(2) = objectType
        paramValue(3) = ""
        paramValue(4) = ""
        paramValue(5) = ""
        paramValue(6) = ""
        paramValue(7) = ""
        paramValue(8) = ""
        paramValue(9) = ""
        paramValue(10) = partitionName
        '型
        oraType(0) = M_CONST.C_COLUMN_TYPE.STRING
        oraType(1) = M_CONST.C_COLUMN_TYPE.STRING
        oraType(2) = M_CONST.C_COLUMN_TYPE.STRING
        oraType(3) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(4) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(5) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(6) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(7) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(8) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(9) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(10) = M_CONST.C_COLUMN_TYPE.STRING

        '引数の属性
        inOut(0) = ParameterDirection.Input
        inOut(1) = ParameterDirection.Input
        inOut(2) = ParameterDirection.Input
        inOut(3) = ParameterDirection.Output
        inOut(4) = ParameterDirection.Output
        inOut(5) = ParameterDirection.Output
        inOut(6) = ParameterDirection.Output
        inOut(7) = ParameterDirection.Output
        inOut(8) = ParameterDirection.Output
        inOut(9) = ParameterDirection.Output
        inOut(10) = ParameterDirection.Input

        'パラメータセット
        For i = 0 To bind.Length - 1
            bind(i).SET_BIND(paramName(i), paramValue(i), oraType(i), inOut(i), 0)
        Next
        If SET_BIND_VARIABLE(bind) Then
            '実行
            'EXEC_PROCEDURE(W_SQL, W_STR2)
            If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
                isSuccess = M_ODP.EXEC_PROCEDURE(sql, tmpStr)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
                isSuccess = M_OO4O.EXEC_PROCEDURE(sql, tmpStr)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
                isSuccess = M_MS_CL.EXEC_PROCEDURE(sql, tmpStr)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
                '要修正
                'M_ODBC.EXEC_PROCEDURE(W_SQL, W_STR2)
                isSuccess = M_ODBC.EXEC_PROCEDURE(sql)
            End If
        End If

        '戻り値セット
        returnStr(0) = tmpStr(3)
        returnStr(1) = tmpStr(4)
        returnStr(2) = tmpStr(5)
        returnStr(3) = tmpStr(6)
        returnStr(4) = tmpStr(7)
        returnStr(5) = tmpStr(8)

        Return isSuccess
    End Function

    Function EXEC_DBMS_SPACE_SPACE_USAGE(ByVal owner As String, ByVal objectName As String, ByVal objectType As String, _
                                       ByVal partitionName As String, ByRef returnStr As String()) As Boolean
        '#######################################################################
        'DBMS_SPACE.SPACE_USAGE実行
        '  DBMS_SPACE_UNUSED_SPACEの実行結果を文字列配列として戻す
        '引数
        '  owner          :オブジェクト所有スキーマ
        '　objectName     :オブジェクト名称
        '  objectType     :オブジェクトタイプ
        '  partitionName  :パーティション名(パーティションオブジェクトの場合のみ)
        '  returnStr(0):未フォーマットブロック数
        '  returnStr(1):未フォーマットバイト
        '  returnStr(2):0-25%空きブロック数
        '  returnStr(3):0-25%空きバイト
        '  returnStr(4):25-50%空きブロック数
        '  returnStr(5):25-50%空きバイト
        '  returnStr(6):50-75%空きブロック数
        '  returnStr(7):50-75%空きバイト
        '  returnStr(8):75-100%空きブロック数
        '  returnStr(9):75-100%空きバイト
        '  returnStr(10):フルブロック数
        '  returnStr(11):フルバイト
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################

        Dim sql As String = vbNullString
        Dim oraType(15) As Short
        Dim inOut(15) As ParameterDirection
        Dim paramName(15) As String
        Dim paramValue(15) As String
        Dim tmpStr(15) As String  'プロシージャに参照渡しする引数
        Dim isSuccess As Boolean = False
        Dim bind(15) As M_CONST.S_BIND

        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            sql = "BEGIN SYS.DBMS_SPACE.SPACE_USAGE(SEGMENT_OWNER=>:SEGMENT_OWNER, SEGMENT_NAME=>:SEGMENT_NAME, " _
            & "SEGMENT_TYPE=>:SEGMENT_TYPE, UNFORMATTED_BLOCKS=>:UNFORMATTED_BLOCKS, UNFORMATTED_BYTES=>:UNFORMATTED_BYTES, " _
            & "FS1_BLOCKS=>:FS1_BLOCKS, FS1_BYTES=>:FS1_BYTES, FS2_BLOCKS=>:FS2_BLOCKS, FS2_BYTES=>:FS2_BYTES, " _
            & "FS3_BLOCKS=>:FS3_BLOCKS, FS3_BYTES=>:FS3_BYTES, FS4_BLOCKS=>:FS4_BLOCKS, FS4_BYTES=>:FS4_BYTES, " _
            & "FULL_BLOCKS=>:FULL_BLOCKS, FULL_BYTES=>:FULL_BYTES, PARTITION_NAME=>:PARTITION_NAME); end;"
        Else
            sql = "SYS.DBMS_SPACE.SPACE_USAGE"
        End If

        ''パラメータ設定
        'パラメータ名
        paramName(0) = "SEGMENT_OWNER"
        paramName(1) = "SEGMENT_NAME"
        paramName(2) = "SEGMENT_TYPE"
        paramName(3) = "UNFORMATTED_BLOCKS"
        paramName(4) = "UNFORMATTED_BYTES"
        paramName(5) = "FS1_BLOCKS"
        paramName(6) = "FS1_BYTES"
        paramName(7) = "FS2_BLOCKS"
        paramName(8) = "FS2_BYTES"
        paramName(9) = "FS3_BLOCKS"
        paramName(10) = "FS3_BYTES"
        paramName(11) = "FS4_BLOCKS"
        paramName(12) = "FS4_BYTES"
        paramName(13) = "FULL_BLOCKS"
        paramName(14) = "FULL_BYTES"
        paramName(15) = "PARTITION_NAME"
        'パラメータ値
        paramValue(0) = owner
        paramValue(1) = objectName
        paramValue(2) = objectType
        paramValue(3) = ""
        paramValue(4) = ""
        paramValue(5) = ""
        paramValue(6) = ""
        paramValue(7) = ""
        paramValue(8) = ""
        paramValue(9) = ""
        paramValue(10) = ""
        paramValue(11) = ""
        paramValue(12) = ""
        paramValue(13) = ""
        paramValue(14) = ""
        paramValue(15) = partitionName
        '型
        oraType(0) = M_CONST.C_COLUMN_TYPE.STRING
        oraType(1) = M_CONST.C_COLUMN_TYPE.STRING
        oraType(2) = M_CONST.C_COLUMN_TYPE.STRING
        oraType(3) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(4) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(5) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(6) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(7) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(8) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(9) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(10) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(11) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(12) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(13) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(14) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(15) = M_CONST.C_COLUMN_TYPE.STRING

        '引数の属性
        inOut(0) = ParameterDirection.Input
        inOut(1) = ParameterDirection.Input
        inOut(2) = ParameterDirection.Input
        inOut(3) = ParameterDirection.Output
        inOut(4) = ParameterDirection.Output
        inOut(5) = ParameterDirection.Output
        inOut(6) = ParameterDirection.Output
        inOut(7) = ParameterDirection.Output
        inOut(8) = ParameterDirection.Output
        inOut(9) = ParameterDirection.Output
        inOut(10) = ParameterDirection.Output
        inOut(11) = ParameterDirection.Output
        inOut(12) = ParameterDirection.Output
        inOut(13) = ParameterDirection.Output
        inOut(14) = ParameterDirection.Output
        inOut(15) = ParameterDirection.Input

        'パラメータセット
        For i = 0 To bind.Length - 1
            bind(i).SET_BIND(paramName(i), paramValue(i), oraType(i), inOut(i), 0)
        Next
        If SET_BIND_VARIABLE(bind) Then
            '実行
            If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
                isSuccess = M_ODP.EXEC_PROCEDURE(sql, tmpStr)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
                isSuccess = M_OO4O.EXEC_PROCEDURE(sql, tmpStr)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
                isSuccess = M_MS_CL.EXEC_PROCEDURE(sql, tmpStr)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
                M_ODBC.EXEC_PROCEDURE(sql, tmpStr)
            End If
        End If

        '戻り値セット
        returnStr(0) = tmpStr(3)
        returnStr(1) = tmpStr(4)
        returnStr(2) = tmpStr(5)
        returnStr(3) = tmpStr(6)
        returnStr(4) = tmpStr(7)
        returnStr(5) = tmpStr(8)
        returnStr(6) = tmpStr(9)
        returnStr(7) = tmpStr(10)
        returnStr(8) = tmpStr(11)
        returnStr(9) = tmpStr(12)
        returnStr(10) = tmpStr(13)
        returnStr(11) = tmpStr(14)

        Return isSuccess
    End Function

    Function EXEC_DBMS_SPACE_SPACE_USAGE_SECUREFILE(ByVal owner As String, ByVal objectName As String, ByVal objectType As String, _
                                   ByVal partitionName As String, ByRef returnStr As String()) As Boolean
        '#######################################################################
        'DBMS_SPACE.SPACE_USAGE(セキュアファイル)実行
        '  DBMS_SPACE_UNUSED_SPACEの実行結果を文字列配列として戻す
        '引数
        '  owner        :オブジェクト所有スキーマ
        '　objectName   :オブジェクト名称
        '  objectType   :オブジェクトタイプ
        '  partitionName:パーティション名(パーティションオブジェクトの場合のみ)
        '  returnStr(0) :セグメントに割り当てられているブロックの数
        '  returnStr(1) :セグメントに割り当てられているバイトの数
        '  returnStr(2) :アクティブなデータが含まれているLOBに割り当てられているブロックの数
        '  returnStr(3) :アクティブなデータが含まれているLOBに割り当てられているバイトの数
        '  returnStr(4) :LOBでバージョン・データの保持に使用されている期限切れブロックの数
        '  returnStr(5) :LOBでバージョン・データの保持に使用されている期限切れバイトの数
        '  returnStr(6) :LOBでバージョン・データの保持に使用されている期限切れになっていないブロックの数
        '  returnStr(7) :LOBでバージョン・データの保持に使用されている期限切れになっていないバイトの数
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################

        Dim sql As String = vbNullString
        Dim oraType(11) As Short
        Dim inOut(11) As ParameterDirection
        Dim paramName(11) As String
        Dim paramValue(11) As String
        Dim tmpStr(11) As String
        Dim isSuccess As Boolean = False
        Dim bind(11) As M_CONST.S_BIND

        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            sql = "BEGIN SYS.DBMS_SPACE.SPACE_USAGE(SEGMENT_OWNER=>:SEGMENT_OWNER, SEGMENT_NAME=>:SEGMENT_NAME, " _
            & "SEGMENT_TYPE=>:SEGMENT_TYPE, SEGMENT_SIZE_BLOCKS=>:SEGMENT_SIZE_BLOCKS, SEGMENT_SIZE_BYTES=>:SEGMENT_SIZE_BYTES, " _
            & "USED_BLOCKS=>:USED_BLOCKS, USED_BYTES=>:USED_BYTES, EXPIRED_BLOCKS=>:EXPIRED_BLOCKS, EXPIRED_BYTES=>:EXPIRED_BYTES, " _
            & "UNEXPIRED_BLOCKS=>:UNEXPIRED_BLOCKS, UNEXPIRED_BYTES=>:UNEXPIRED_BYTES, PARTITION_NAME=>:PARTITION_NAME); end;"
        Else
            sql = "SYS.DBMS_SPACE.SPACE_USAGE"
        End If

        ''パラメータ設定
        'パラメータ名
        paramName(0) = "SEGMENT_OWNER"
        paramName(1) = "SEGMENT_NAME"
        paramName(2) = "SEGMENT_TYPE"
        paramName(3) = "SEGMENT_SIZE_BLOCKS"
        paramName(4) = "SEGMENT_SIZE_BYTES"
        paramName(5) = "USED_BLOCKS"
        paramName(6) = "USED_BYTES"
        paramName(7) = "EXPIRED_BLOCKS"
        paramName(8) = "EXPIRED_BYTES"
        paramName(9) = "UNEXPIRED_BLOCKS"
        paramName(10) = "UNEXPIRED_BYTES"
        paramName(11) = "PARTITION_NAME"
        'パラメータ値
        paramValue(0) = owner
        paramValue(1) = objectName
        paramValue(2) = objectType
        paramValue(3) = ""
        paramValue(4) = ""
        paramValue(5) = ""
        paramValue(6) = ""
        paramValue(7) = ""
        paramValue(8) = ""
        paramValue(9) = ""
        paramValue(10) = ""
        paramValue(11) = partitionName
        '型
        oraType(0) = M_CONST.C_COLUMN_TYPE.STRING
        oraType(1) = M_CONST.C_COLUMN_TYPE.STRING
        oraType(2) = M_CONST.C_COLUMN_TYPE.STRING
        oraType(3) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(4) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(5) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(6) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(7) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(8) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(9) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(10) = M_CONST.C_COLUMN_TYPE.NUMBER
        oraType(11) = M_CONST.C_COLUMN_TYPE.STRING

        '引数の属性
        inOut(0) = ParameterDirection.Input
        inOut(1) = ParameterDirection.Input
        inOut(2) = ParameterDirection.Input
        inOut(3) = ParameterDirection.Output
        inOut(4) = ParameterDirection.Output
        inOut(5) = ParameterDirection.Output
        inOut(6) = ParameterDirection.Output
        inOut(7) = ParameterDirection.Output
        inOut(8) = ParameterDirection.Output
        inOut(9) = ParameterDirection.Output
        inOut(10) = ParameterDirection.Output
        inOut(11) = ParameterDirection.Input

        'パラメータセット
        For i = 0 To bind.Length - 1
            bind(i).SET_BIND(paramName(i), paramValue(i), oraType(i), inOut(i), 0)
        Next
        If SET_BIND_VARIABLE(bind) Then
            '実行
            If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
                isSuccess = M_ODP.EXEC_PROCEDURE(sql, tmpStr)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
                isSuccess = M_OO4O.EXEC_PROCEDURE(sql, tmpStr)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
                isSuccess = M_MS_CL.EXEC_PROCEDURE(sql, tmpStr)
            ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
                M_ODBC.EXEC_PROCEDURE(sql, tmpStr)
            End If
        End If

        '戻り値セット
        returnStr(0) = tmpStr(3)
        returnStr(1) = tmpStr(4)
        returnStr(2) = tmpStr(5)
        returnStr(3) = tmpStr(6)
        returnStr(4) = tmpStr(7)
        returnStr(5) = tmpStr(8)
        returnStr(6) = tmpStr(9)
        returnStr(7) = tmpStr(10)

        Return isSuccess
    End Function

    Function SET_TRANSACTION() As Boolean
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            Return M_ODP.SET_BEGINTRAN()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            Return M_OO4O.SET_BEGINTRAN()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            Return M_MS_CL.SET_BEGINTRAN()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            Return M_ODBC.SET_BEGINTRAN()
        Else
            Return False
        End If
    End Function

    Function EXEC_COMMIT() As Boolean
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            Return M_ODP.EXEC_COMMIT()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            Return M_OO4O.EXEC_COMMIT()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            Return M_MS_CL.EXEC_COMMIT()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            Return M_ODBC.EXEC_COMMIT()
        Else
            Return False
        End If
    End Function

    Function EXEC_ROLLBACK() As Boolean
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            Return M_ODP.EXEC_ROLLBACK()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            Return M_OO4O.EXEC_ROLLBACK()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            Return M_MS_CL.EXEC_ROLLBACK()
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            Return M_ODBC.EXEC_ROLLBACK()
        Else
            Return False
        End If
    End Function

    Function SET_BIND_VARIABLE(ByVal bind() As M_CONST.S_BIND) As Boolean
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            Return M_ODP.SET_BIND_VARIABLE(bind)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            Return M_OO4O.SET_BIND_VARIABLE(bind)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            Return M_MS_CL.SET_BIND_VARIABLE(bind)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            Return M_ODBC.SET_BIND_VARIABLE(bind)
        Else
            Return False
        End If
    End Function

    Function SET_BIND_ARRAYVARIABLE(Optional ByVal bindArray() As M_CONST.S_BIND_ARR = Nothing, Optional ByVal bind() As M_CONST.S_BIND = Nothing) As Boolean
        'バインド配列に対応しているのはODP.NETのみ。OO4Oは実装はできそうだが、今のところ未対応
        If M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODP Then
            Return M_ODP.SET_BIND_ARRAYVARIABLE(bindArray, bind)
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.OO4O Then
            'Return M_OO4O.SET_BIND_ARRAYVARIABLE(bindArray)
            MsgBox("未対応")
            Return False
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.MS_CL Then
            'Return M_MS_CL.SET_BIND_ARRAYVARIABLE(bindArray)
            MsgBox("未対応")
            Return False
        ElseIf M_MIDDLEWARE = M_CONST.C_MIDDLE_TYPE.ODBC Then
            'Return M_ODBC.SET_BIND_ARRAYVARIABLE(bindArray)
            MsgBox("未対応")
            Return False
        Else
            Return False
        End If
    End Function

    Public Sub DISPOSE_DATA_TABLE()
        '#######################################################################
        '本クラスで定義しているM_DATA_TABLEを破棄する
        '#######################################################################
        If Not (M_DATA_TABLE Is Nothing) Then
            M_DATA_TABLE.Dispose()
            M_DATA_TABLE = Nothing
        End If
    End Sub
End Class