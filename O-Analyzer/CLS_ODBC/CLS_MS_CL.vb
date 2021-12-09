Imports System.Data.OracleClient

Public Class CLS_ODBC
    '#######################################################################
    'ODBC操作クラス
    '#######################################################################

    'グローバル変数
    Public G_CONN As New OracleConnection 'オラクルコネクション
    Public G_CONN2 As Object = G_CONN 'G_CONNを参照する変数

    'モジュール変数
    Private M_READER As OracleDataReader
    Private M_ADAPTER As OracleDataAdapter
    Private M_CONST As New CLS_CONST.CLS_CONST

    Function DB_CONNECT(ByVal W_USERID As String, ByVal W_PASS As String, ByVal W_CONN_STR As String, Optional ByVal W_META_POOL_FLG As Boolean = True, Optional ByVal W_POOL_FLG As Boolean = False, Optional ByVal W_MIN_POOL As Integer = 1, Optional ByVal W_MAX_POOL As Integer = 5, Optional ByVal W_SYSDBA_FLG As Boolean = False) As Boolean
        '#######################################################################
        'DBコネクション取得
        '  取得したコネクションをG_CONNに保持()
        '引数
        '  W_USERID        :接続ユーザID
        '  W_PASS          :接続パスワード
        '  W_CONN_STR      :接続文字列
        '  W_META_POOL_FLG :メタデータプーリング機能の使用有無
        '  W_POOL_FLG      :コネクションプーリング機能の使用有無
        '  W_MIN_POOL      :コネクションプーリングの最小接続数
        '  W_MAX_POOL      :コネクションプーリングの最大接続数
        '  W_SYSDBA_FLG    :SYSDBAでの接続有無
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################

        If W_USERID = vbNullString And W_PASS = vbNullString Then
            'OS認証
            G_CONN.ConnectionString = "User Id= / " & ";Data Source=" + W_CONN_STR & ";Pooling=" & W_POOL_FLG & ";Min Pool Size=" & W_MIN_POOL & ";Max Pool Size=" & W_MAX_POOL & ";"
        ElseIf W_SYSDBA_FLG And W_PASS = vbNullString Then
            'SYSDBA接続(パスワード入力無(OS認証))
            G_CONN.ConnectionString = "User Id= / ;Data Source=" + W_CONN_STR & ";Pooling=" & W_POOL_FLG & ";Min Pool Size=" & W_MIN_POOL & ";Max Pool Size=" & W_MAX_POOL & ";" & "DBA Privilege=SYSDBA;"
        ElseIf W_SYSDBA_FLG And W_PASS <> vbNullString Then
            'SYSDBA接続(パスワード入力有(OS認証＋パスワード認証))
            G_CONN.ConnectionString = "User Id=" & W_USERID & ";Password=" & W_PASS & ";Data Source=" + W_CONN_STR & ";Pooling=" & W_POOL_FLG & ";Min Pool Size=" & W_MIN_POOL & ";Max Pool Size=" & W_MAX_POOL & ";" & "DBA Privilege=SYSDBA;"
        Else
            '通常接続
            G_CONN.ConnectionString = "User Id=" & W_USERID & ";Password=" & W_PASS & ";Data Source=" + W_CONN_STR & ";Pooling=" & W_POOL_FLG & ";Min Pool Size=" & W_MIN_POOL & ";Max Pool Size=" & W_MAX_POOL & ";"
        End If

        Try
            G_CONN.Open()
            G_CONN2 = G_CONN
            CLS_FILE.CLS_FILE.WRITE_AP_LOG("接続に成功しました。 ユーザID:" & W_USERID & " 接続文字列:" & W_CONN_STR & "ミドルウエア:" & M_CONST.GET_MIDDLE_NAME(CLS_CONST.CLS_CONST.C_MIDDLE_MS_CL))
            Return True
        Catch ex As OracleException
            MsgBox(ex.Message)
            CLS_FILE.CLS_FILE.WRITE_AP_LOG("接続に失敗しました。  ユーザID:" & W_USERID & " 接続文字列:" & W_CONN_STR & "ミドルウエア:" & M_CONST.GET_MIDDLE_NAME(CLS_CONST.CLS_CONST.C_MIDDLE_MS_CL) & " エラーメッセージ：" & ex.Message)
            Return False
        Catch ex2 As Exception
            MsgBox(ex2.Message)
            CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex2)
            Return False
        End Try
    End Function

    Public Function GET_VERSION() As String()
        Dim W_ORA_VERSION(5) As String
        Dim W_MOTO_POS As Short = 0 '元ポジション
        Dim W_SAKI_POS As Short = 0 '先ポジション

        'バージョン情報取得
        W_ORA_VERSION(0) = G_CONN.ServerVersion.ToString()
        For W_CNT As Short = 1 To 5
            W_SAKI_POS = InStr(W_SAKI_POS + 1, W_ORA_VERSION(0), ".")
            If W_SAKI_POS = 0 Then
                W_ORA_VERSION(W_CNT) = W_ORA_VERSION(0).Substring(W_MOTO_POS, W_ORA_VERSION(0).Length - W_MOTO_POS)
            Else
                W_ORA_VERSION(W_CNT) = W_ORA_VERSION(0).Substring(W_MOTO_POS, W_SAKI_POS - W_MOTO_POS - 1)
            End If

            W_MOTO_POS = W_SAKI_POS
        Next

        Return W_ORA_VERSION

    End Function
    Function GET_COMPETENCE() As Boolean
        '#######################################################################
        '権限情報取得
        '  V$ビューやデータディクショナリにアクセスできるか確認する
        '戻り値
        '  TRUE  :参照成功
        '  FALSE :参照失敗
        '#######################################################################
        Dim W_CMD As New OracleCommand
        Dim W_READER As OracleDataReader

        W_CMD.Connection = G_CONN
        W_READER = Nothing

        Try
            'V$VIEWの検索チェック
            W_CMD.CommandText = "SELECT 0 FROM V$INSTANCE"
            W_READER = W_CMD.ExecuteReader()

            W_READER.Read()
            W_READER.Close()

            'ディクショナリの検索チェック
            W_CMD.CommandText = "SELECT COUNT(*) FROM DBA_TABLES"

            W_READER = W_CMD.ExecuteReader()
            W_READER.Read()
            W_READER.Close()
            Return True
        Catch ex As OracleException
            If InStr(ex.Message, "ORA-00942") <> 0 Then
                'テーブルが参照できない場合
                Return False
            Else
                CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex)
                MsgBox(ex.Message)
                Return False
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
            CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex)
            Return False
            Exit Try
        End Try
    End Function

    Function GET_LOGIN_USER() As String
        '#######################################################################
        'ログインユーザ取得
        '  ログインユーザを戻す
        '戻り値
        '  ログインユーザ
        '#######################################################################
        Dim W_CMD As New OracleCommand
        Dim W_READER As OracleDataReader
        Dim W_USER As String = vbNullString

        W_CMD.Connection = G_CONN
        W_READER = Nothing

        'ログインユーザ確認
        Try
            W_CMD.CommandText = "SELECT USERNAME FROM USER_USERS"
            W_READER = W_CMD.ExecuteReader()
            W_READER.Read()
            W_USER = W_READER.Item(0).ToString
            W_READER.Close()
            Return W_USER
        Catch ex As Exception
            MsgBox(ex.Message)
            CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex)
            Return W_USER
        End Try

    End Function

    Function GET_CONNECT_STRING() As String
        '#######################################################################
        '接続文字列取得
        '  接続文字列を戻す
        '戻り値
        '  接続文字列
        '#######################################################################
        Dim W_USER As String = vbNullString

        'ログインユーザ確認
        Return G_CONN.DataSource.ToString

    End Function

    Sub DB_CLOSE()
        '#######################################################################
        'DBコネクション切断
        ' コネクションを切断する
        '※コネクションプーリングを使用している場合は
        '　実行しても物理コネクションが破棄されないケースがある
        '#######################################################################
        System.Data.OracleClient.OracleConnection.ClearPool(G_CONN)
        G_CONN.Close()
        G_CONN.Dispose()

        CLS_FILE.CLS_FILE.WRITE_AP_LOG("コネクションを切断しました")
    End Sub

    Public Function GET_DATA_ADAPTER(ByVal W_SQL As String, Optional ByVal W_TIMEOUT_VALUE As Integer = 0) As Boolean
        '#######################################################################
        'SELECTデータの非接続型取得
        '　OracleDataAdapter⇒DataSetに変換したものを参照渡しで戻す
        '引数
        '  W_SQL           :SQL文
        '  W_DATASET       :DataSetオブジェクト(参照戻し)
        '  W_TIMEOUT_VALUE :タイムアウト値(秒)
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        Dim W_CMD As New OracleCommand(W_SQL, G_CONN)

        M_ADAPTER = Nothing
        'SQLタイムアウトの設定
        If W_TIMEOUT_VALUE > 0 Then
            W_CMD.CommandTimeout = W_TIMEOUT_VALUE
        End If

        Try
            CLS_FILE.CLS_FILE.WRITE_AP_LOG("SQL実行開始：" & W_SQL)
            M_ADAPTER = New OracleDataAdapter(W_CMD)
            CLS_FILE.CLS_FILE.WRITE_AP_LOG("SQL実行終了：" & W_SQL)
            GET_DATA_ADAPTER = True
        Catch ex As Exception
            MsgBox(ex.Message)
            CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex)
            M_ADAPTER = Nothing
            GET_DATA_ADAPTER = False
        End Try

    End Function

    Public Function GET_DATASET(ByRef W_DATASET As DataSet, ByRef M_ROW_CNT As Integer, ByVal W_READ_CNT As Integer, _
                                ByVal W_TABLE_NAME As String, ByRef M_SELECT_FLG As Boolean, ByRef M_JITSU_READ_CNT As Integer) As Boolean
        '#######################################################################
        'SELECTデータの非接続型取得
        '　OracleDataAdapter⇒DataSetに変換したものを参照渡しで戻す
        '引数
        '  W_DATASET    :DataSetオブジェクト(参照戻し)
        '  W_START_CNT  :読み込みスタート行数
        '  W_END_CNT    :読み込み行数
        '  W_TABLE_NAME :テーブル名
        '  M_ROW_CNT    :実際に読み込まれた行数
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################

        Try
            M_JITSU_READ_CNT = M_ADAPTER.Fill(W_DATASET, M_ROW_CNT, W_READ_CNT, W_TABLE_NAME)
            If M_JITSU_READ_CNT < W_READ_CNT Then
                M_SELECT_FLG = False
            End If
            M_ROW_CNT = M_ROW_CNT + M_JITSU_READ_CNT
            GET_DATASET = True
        Catch ex As Exception
            MsgBox(ex.Message)
            CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex)
            W_DATASET = Nothing
            GET_DATASET = False
        End Try
    End Function

    Public Function GET_DATASET(ByVal W_SQL As String, ByRef W_DATASET As DataSet, Optional ByVal W_TIMEOUT_VALUE As Integer = 0) As Boolean
        '#######################################################################
        'SELECTデータの非接続型取得
        '　SELECT文の結果セットをOracleDataAdapterで取得し、
        '  datasetで戻す
        '引数
        '  W_SQL           :SQL文
        '  W_DATASET       :DataSetオブジェクト(参照戻し)
        '  W_TIMEOUT_VALUE :タイムアウト値(秒)
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        Dim W_CMD As New OracleCommand(W_SQL, G_CONN)
        Dim W_DA As OracleDataAdapter

        'SQLタイムアウトの設定
        If W_TIMEOUT_VALUE > 0 Then
            W_CMD.CommandTimeout = W_TIMEOUT_VALUE
        End If

        Try
            W_DA = New OracleDataAdapter(W_CMD)
            CLS_FILE.CLS_FILE.WRITE_AP_LOG("SQL実行開始：" & W_SQL)
            W_DA.Fill(W_DATASET)
            CLS_FILE.CLS_FILE.WRITE_AP_LOG("SQL実行終了：" & W_SQL)
            GET_DATASET = True
        Catch ex As Exception
            MsgBox(ex.Message)
            CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex)
            W_DATASET = Nothing
            GET_DATASET = False
        End Try
    End Function

    Public Function GET_ORACLEDATAREADER(ByVal W_SQL As String, Optional ByVal W_TIMEOUT_VALUE As Integer = 0) As Boolean
        '#######################################################################
        'SELECTデータの接続型取得
        '　SELECT文の結果セットをOracleDataReaderオブジェクトで戻す
        '引数
        '  W_SQL              :SQL文
        '  W_ORACLEDATAREADER :OracleDataReaderオブジェクト(参照戻し)
        '  W_TIMEOUT_VALUE    :タイムアウト値(秒)
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        Dim W_CMD As OracleCommand

        M_READER = Nothing

        Try
            W_CMD = New OracleCommand(W_SQL, G_CONN)
            'SQLタイムアウトの設定
            If W_TIMEOUT_VALUE > 0 Then
                W_CMD.CommandTimeout = W_TIMEOUT_VALUE
            End If

            CLS_FILE.CLS_FILE.WRITE_AP_LOG("SQL実行開始：" & W_SQL)
            M_READER = W_CMD.ExecuteReader()
            CLS_FILE.CLS_FILE.WRITE_AP_LOG("SQL実行終了：" & W_SQL)
            GET_ORACLEDATAREADER = True

        Catch ex As Exception
            MsgBox(ex.Message)
            CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex)
            M_READER = Nothing
            GET_ORACLEDATAREADER = False
        End Try

    End Function

    Public Sub INIT_DATA_TABLE(ByRef M_DATA_TABLE As DataTable, ByRef M_COLUMN_COUNT As Long, ByRef M_ROW_COUNT As Long)
        Dim W_VALUES As String()
        Dim W_INT As Integer = 0

        Try
            '列数
            M_COLUMN_COUNT = M_READER.FieldCount
            ReDim W_VALUES(M_COLUMN_COUNT - 1)

            '行数初期化
            M_ROW_COUNT = 0

            'グリッドビューの列設定
            For W_CNT = 0 To M_COLUMN_COUNT - 1
                Try
                    M_DATA_TABLE.Columns.Add(M_READER.GetName(W_CNT))
                Catch ex As Exception
                    '同じ列名が複数あった場合最後に数字を付与した列名とする
                    W_INT = W_INT + 1
                    M_DATA_TABLE.Columns.Add(M_READER.GetName(W_CNT) & W_INT)
                End Try
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
            CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex)
        End Try
    End Sub

    Public Sub FETCH_DATA_DR(ByRef M_ROW_COUNT As Long, ByRef M_DATA_TABLE As DataTable, ByRef M_SELECT_FLG As Boolean, _
                          ByRef M_COLUMN_COUNT As Long, Optional ByVal W_FETCH_COUNT As Long = 10000000000)
        '#######################################################################
        'フェッチデータ
        '  W_FETCH_COUNTで指定された行数分M_DATA_TABLEにデータを追加する
        '引数：
        ' M_ROW_COUNT          :フェッチ済み行数
        ' M_DATA_TABLE         :DataGridViewに設定するDataTableオブジェクト
        ' M_SELECT_FLG         :SELECT実行中フラグ
        ' M_COLUMN_COUNT       :SELECTした列数
        ' W_FETCH_COUNT        :フェッチ行数
        '#######################################################################

        Dim W_VALUES(M_COLUMN_COUNT - 1) As String

        '開発用
        'Dim w_reader As OracleDataReader

        M_COLUMN_COUNT = M_READER.FieldCount
        Try
            If M_ROW_COUNT = 0 Then
                '初回のみreadする(次回以降はread済み状態のため不要)
                M_SELECT_FLG = M_READER.Read()
            End If

            While M_SELECT_FLG
                '行データ取得
                W_VALUES = GET_ROW_VALUES(M_COLUMN_COUNT)
                M_DATA_TABLE.Rows.Add(W_VALUES)
                M_ROW_COUNT = M_ROW_COUNT + 1
                'W_CNT件まで読み込み
                If M_ROW_COUNT Mod W_FETCH_COUNT = 0 Then
                    M_SELECT_FLG = True
                    Exit While
                End If
                M_SELECT_FLG = M_READER.Read()
            End While

            '読み込み件数+1件目のデータがあるか判断
            If M_READER.Read() Then
                M_SELECT_FLG = True
            Else
                M_SELECT_FLG = False
                M_READER.Close()
            End If

        Catch ex As Exception
            M_SELECT_FLG = False
            MsgBox(ex.Message)
            CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex)
        End Try
    End Sub

    Public Sub FETCH_DATA(ByRef M_DATASET As DataSet, ByRef M_ROW_COUNT As Long, ByRef M_SELECT_FLG As Boolean, _
                          ByRef M_COLUMN_COUNT As Long, Optional ByVal W_FETCH_COUNT As Long = 10000000000)
        '#######################################################################
        'フェッチデータ
        '  W_FETCH_COUNTで指定された行数分M_DATA_TABLEにデータを追加する
        '引数：
        ' M_ROW_COUNT          :フェッチ済み行数
        ' M_DATA_TABLE         :DataGridViewに設定するDataTableオブジェクト
        ' M_SELECT_FLG         :SELECT実行中フラグ
        ' M_COLUMN_COUNT       :SELECTした列数
        ' W_FETCH_COUNT        :フェッチ行数
        '#######################################################################
        M_ADAPTER.Fill(M_DATASET, M_ROW_COUNT, M_SELECT_FLG, "")

    End Sub

    Public Function FETCH_ROW(ByRef W_ROW As String(), ByRef M_ROW_COUNT As Long, ByRef M_SELECT_FLG As Boolean, _
                          ByRef M_COLUMN_COUNT As Long, Optional ByVal W_FETCH_COUNT As Long = 10000000000) As Boolean
        '#######################################################################
        'フェッチ行取得
        '  
        '引数：
        ' W_FETCH_COUNT        :フェッチ行数
        '#######################################################################

        Dim W_VALUES() As String

        M_COLUMN_COUNT = M_READER.FieldCount
        Try
            M_SELECT_FLG = M_READER.Read()

            If M_SELECT_FLG Then
                W_VALUES = GET_ROW_VALUES(M_COLUMN_COUNT)
                W_ROW = W_VALUES
                M_ROW_COUNT = M_ROW_COUNT + 1
                Return True
            Else
                W_ROW = Nothing
                Return False
            End If
        Catch ex As Exception
            M_SELECT_FLG = False
            MsgBox(ex.Message)
            CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex)
            Return False
        End Try
    End Function

    Private Function GET_ROW_VALUES(ByVal W_COLUMN_COUNT As Integer) As String()
        '#######################################################################
        '行データ取得
        '  
        '引数：
        ' W_COLUMN_COUNT        :列数
        '#######################################################################
        Dim W_VALUES(W_COLUMN_COUNT - 1) As String
        Dim W_TYPE As String
        Dim W_CLOB As OracleLob
        Dim W_ACTUAL As Integer = 0

        For W_COL_CNT = 0 To W_COLUMN_COUNT - 1
            W_TYPE = M_READER.GetDataTypeName(W_COL_CNT)

            W_VALUES(W_COL_CNT) = vbNullString
            If Not M_READER.IsDBNull(W_COL_CNT) Then
                If W_TYPE = "RAW" Then
                    W_VALUES(W_COL_CNT) = BitConverter.ToString(M_READER.GetOracleBinary(W_COL_CNT).Value, 0).Replace("-", "")
                ElseIf W_TYPE = "CLOB" Then
                    W_CLOB = M_READER.GetOracleLob(W_COL_CNT)
                    W_VALUES(W_COL_CNT) = M_READER.GetOracleLob(W_COL_CNT).Value

                    'ストリームを利用して読み込む方法
                    'Dim W_STREADREADER As System.IO.StreamReader = New System.IO.StreamReader(W_CLOB, System.Text.Encoding.Unicode)
                    'Dim cbuffer As Char() = New Char(M_READER.GetOracleLob(W_COL_CNT).Length) {}
                    'W_ACTUAL = W_STREADREADER.Read(cbuffer, 0, M_READER.GetOracleLob(W_COL_CNT).Length - 1)
                    'W_VALUES(W_COL_CNT) = cbuffer

                ElseIf W_TYPE = "BLOB" Then
                    'BLOB型は「BLOB:レングスbyte」を表示
                    W_VALUES(W_COL_CNT) = "BLOB:" & M_READER.GetOracleLob(W_COL_CNT).Length.ToString & "byte"
                ElseIf W_TYPE = "BFILE" Then
                    'BFILE型は「BFILE:ディレクトリ:ファイル名」を表示
                    W_VALUES(W_COL_CNT) = "BFILE:" & M_READER.GetOracleBFile(W_COL_CNT).DirectoryName & ":" & M_READER.GetOracleBFile(W_COL_CNT).FileName
                ElseIf W_TYPE = "DATE" Then
                    'DATE型の場合は YYYY/MM/DD HH24:MI:SSの書式で表示
                    W_VALUES(W_COL_CNT) = M_READER.GetDateTime(W_COL_CNT).Year & "/" & M_READER.GetDateTime(W_COL_CNT).Month & "/" & M_READER.GetDateTime(W_COL_CNT).Day & " " & M_READER.GetDateTime(W_COL_CNT).Hour & ":" & M_READER.GetDateTime(W_COL_CNT).Minute & ":" & M_READER.GetDateTime(W_COL_CNT).Second
                ElseIf W_TYPE = "DECIMAL" Then
                    W_VALUES(W_COL_CNT) = M_READER.GetDecimal(W_COL_CNT).ToString
                ElseIf W_TYPE = "DOUBLE" Then
                    W_VALUES(W_COL_CNT) = M_READER.GetOracleNumber(W_COL_CNT).ToString
                ElseIf W_TYPE = "TIMESTAMP" Then
                    W_VALUES(W_COL_CNT) = M_READER.GetDateTime(W_COL_CNT).ToString
                ElseIf W_TYPE = "INT16" Then
                    W_VALUES(W_COL_CNT) = M_READER.GetDecimal(W_COL_CNT).ToString
                ElseIf W_TYPE = "INT32" Then
                    W_VALUES(W_COL_CNT) = M_READER.GetDecimal(W_COL_CNT).ToString
                ElseIf W_TYPE = "INT64" Then
                    W_VALUES(W_COL_CNT) = M_READER.GetDecimal(W_COL_CNT).ToString
                ElseIf W_TYPE = "SINGLE" Then
                    W_VALUES(W_COL_CNT) = M_READER.GetDecimal(W_COL_CNT).ToString
                ElseIf W_TYPE = "VARCHAR2" Then
                    W_VALUES(W_COL_CNT) = M_READER.GetOracleString(W_COL_CNT).ToString
                ElseIf W_TYPE = "IntervalDS" Then
                    W_VALUES(W_COL_CNT) = M_READER.GetOracleTimeSpan(W_COL_CNT).ToString
                ElseIf W_TYPE = "IntervalYM" Then
                    W_VALUES(W_COL_CNT) = M_READER.GetOracleTimeSpan(W_COL_CNT).ToString
                ElseIf W_TYPE = "NUMBER" Then
                    W_VALUES(W_COL_CNT) = M_READER.GetOracleNumber(W_COL_CNT).ToString
                Else
                    MsgBox(W_TYPE)
                    W_VALUES(W_COL_CNT) = M_READER.GetOracleString(W_COL_CNT).ToString
                End If
            End If
        Next
        Return W_VALUES

    End Function

    Public Function EXEC_NONQUERY(ByVal W_SQL As String, ByRef W_COUNT As Long) As Boolean
        '#######################################################################
        '更新SQL実行
        '  SELECT以外の更新系SQLや無名プロシージャ等を実行する。
        '引数
        '  W_SQL   :SQL文
        '  W_COUNT :処理件数(戻し用)
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        Dim W_CMD As New OracleCommand

        Try
            W_CMD.Connection = G_CONN
            W_CMD.CommandText = W_SQL
            W_CMD.CommandType = CommandType.Text
            CLS_FILE.CLS_FILE.WRITE_AP_LOG("SQL実行開始：" & W_SQL)
            W_COUNT = W_CMD.ExecuteNonQuery()
            CLS_FILE.CLS_FILE.WRITE_AP_LOG("SQL実行終了：" & W_SQL)
            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex)
            W_COUNT = -1
            Return False
        End Try
    End Function

    Public Function EXEC_PROCEDURE(ByVal W_SQL As String) As Boolean
        '#######################################################################
        'プロシージャ実行
        '  ストアドプロシージャを実行する
        '引数
        '  W_SQL : SQL文
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################
        Dim W_CMD As New OracleCommand

        Try
            W_CMD.Connection = G_CONN
            W_CMD.CommandText = W_SQL
            W_CMD.CommandType = CommandType.StoredProcedure
            CLS_FILE.CLS_FILE.WRITE_AP_LOG("SQL実行開始：" & W_SQL)
            W_CMD.ExecuteNonQuery()
            CLS_FILE.CLS_FILE.WRITE_AP_LOG("SQL実行終了：" & W_SQL)
            Return True
        Catch ex As Exception
            MsgBox(ex.Message)
            CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex)
            Return False
        End Try
    End Function

    Function EXEC_DBMS_SPACE_UNUSED_SPACE(ByVal W_OWNER As String, ByVal W_NAME As String, ByVal W_TYPE As String, _
                                          ByVal W_PARTITION_NAME As String) As String()
        '#######################################################################
        'DBMS_SPACE_UNUSED_SPACE実行
        '  DBMS_SPACE_UNUSED_SPACEの実行結果を文字列配列として戻す
        '引数
        '  W_OWNER          :オブジェクト所有スキーマ
        '　W_NAME           :オブジェクト名称
        '  W_TYPE           :オブジェクトタイプ
        '  W_PARTITION_NAME :パーティションオブジェクトの場合のパーティション名
        '戻り値
        '  TRUE  :成功
        '  FALSE :失敗
        '#######################################################################

        Dim W_SQL2 As String = "SYS.DBMS_SPACE.UNUSED_SPACE"
        Dim W_STR() As String = {0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
        Dim W_CMD As New OracleCommand(W_SQL2, G_CONN)
        Dim W_TOTAL_BLOCKS As OracleParameter
        Dim W_TOTAL_BYTES As OracleParameter
        Dim W_UNUSED_BLOCKS As OracleParameter
        Dim W_UNUSED_BYTES As OracleParameter
        Dim W_LAST_USED_EXTENT_FILE_ID As OracleParameter
        Dim W_LAST_USED_EXTENT_BLOCK_ID As OracleParameter
        Dim W_LAST_USED_BLOCK As OracleParameter

        Try
            W_TOTAL_BLOCKS = New OracleParameter("TOTAL_BLOCKS", DbType.Decimal)
            W_TOTAL_BYTES = New OracleParameter("TOTAL_BYTES", DbType.Decimal)
            W_UNUSED_BLOCKS = New OracleParameter("UNUSED_BLOCKS", DbType.Decimal)
            W_UNUSED_BYTES = New OracleParameter("UNUSED_BYTES", DbType.Decimal)
            W_LAST_USED_EXTENT_FILE_ID = New OracleParameter("LAST_USED_EXTENT_FILE_ID", DbType.Decimal)
            W_LAST_USED_EXTENT_BLOCK_ID = New OracleParameter("LAST_USED_EXTENT_BLOCK_ID", DbType.Decimal)
            W_LAST_USED_BLOCK = New OracleParameter("LAST_USED_BLOCK", DbType.Decimal)

            'パラメータプロパティ設定
            W_TOTAL_BLOCKS.Direction = ParameterDirection.Output
            W_TOTAL_BYTES.Direction = ParameterDirection.Output
            W_UNUSED_BLOCKS.Direction = ParameterDirection.Output
            W_UNUSED_BYTES.Direction = ParameterDirection.Output
            W_LAST_USED_EXTENT_FILE_ID.Direction = ParameterDirection.Output
            W_LAST_USED_EXTENT_BLOCK_ID.Direction = ParameterDirection.Output
            W_LAST_USED_BLOCK.Direction = ParameterDirection.Output

            '引数set
            W_CMD.Parameters.AddWithValue("SEGMENT_OWNER", DbType.String).Value = W_OWNER
            W_CMD.Parameters.AddWithValue("SEGMENT_NAME", DbType.String).Value = W_NAME
            W_CMD.Parameters.AddWithValue("SEGMENT_TYPE", DbType.String).Value = W_TYPE
            W_CMD.Parameters.Add(W_TOTAL_BLOCKS)
            W_CMD.Parameters.Add(W_TOTAL_BYTES)
            W_CMD.Parameters.Add(W_UNUSED_BLOCKS)
            W_CMD.Parameters.Add(W_UNUSED_BYTES)
            W_CMD.Parameters.Add(W_LAST_USED_EXTENT_FILE_ID)
            W_CMD.Parameters.Add(W_LAST_USED_EXTENT_BLOCK_ID)
            W_CMD.Parameters.Add(W_LAST_USED_BLOCK)
            W_CMD.Parameters.AddWithValue("PARTITION_NAME", DbType.String).Value = Trim(W_PARTITION_NAME)

            '実行
            W_CMD.CommandType = CommandType.StoredProcedure
            W_CMD.ExecuteNonQuery()

            '戻り値セット
            W_STR(0) = W_OWNER
            W_STR(1) = W_NAME
            W_STR(2) = W_TYPE
            W_STR(3) = W_TOTAL_BLOCKS.Value.ToString
            W_STR(4) = W_TOTAL_BYTES.Value.ToString
            W_STR(5) = W_UNUSED_BLOCKS.Value.ToString
            W_STR(6) = W_UNUSED_BYTES.Value.ToString
            W_STR(7) = W_LAST_USED_EXTENT_FILE_ID.Value.ToString
            W_STR(8) = W_LAST_USED_EXTENT_BLOCK_ID.Value.ToString

            'パラメータ削除
            W_CMD.Parameters.RemoveAt("SEGMENT_OWNER")
            W_CMD.Parameters.RemoveAt("SEGMENT_NAME")
            W_CMD.Parameters.RemoveAt("SEGMENT_TYPE")
            W_CMD.Parameters.RemoveAt("TOTAL_BLOCKS")
            W_CMD.Parameters.RemoveAt("TOTAL_BYTES")
            W_CMD.Parameters.RemoveAt("UNUSED_BLOCKS")
            W_CMD.Parameters.RemoveAt("UNUSED_BYTES")
            W_CMD.Parameters.RemoveAt("LAST_USED_EXTENT_FILE_ID")
            W_CMD.Parameters.RemoveAt("LAST_USED_EXTENT_BLOCK_ID")
            W_CMD.Parameters.RemoveAt("LAST_USED_BLOCK")
            W_CMD.Parameters.RemoveAt("PARTITION_NAME")
            Return W_STR
        Catch ex As Exception
            MsgBox(ex.Message)
            CLS_FILE.CLS_FILE.WRITE_ERR_LOG(ex)
            Return W_STR
        End Try
    End Function

End Class
