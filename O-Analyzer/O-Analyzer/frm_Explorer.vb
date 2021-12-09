Imports M_CONST = CLS_CONST.CLS_CONST
Imports M_COMMON = CLS_COMMON.CLS_COMMON
Imports M_FILE = CLS_FILE.CLS_FILE
Imports System.Text

Public Class frm_Explorer
    '#######################################################################
    'エクスプローラ画面
    '#######################################################################

    'モジュール変数
    Private M_SELECTFLG As Boolean = False
    Private M_CANCELFLG As Boolean = False
    Private M_RECONNECT As Boolean = False

    ''''''''
    'ListViewItemSorterに指定するフィールド
    Dim listViewItemSorter As ListViewItemComparer
    ''''''''

    Private Sub LoadDataForm()
        '#######################################################################
        'データ画面表示
        '#######################################################################

        Dim schemaName As String = vbNullString
        Dim objectType As String = vbNullString
        Dim objectName As String = vbNullString
        Dim partitionName As String = vbNullString
        Dim subpartitionName As String = vbNullString
        Dim msg As String = M_COMMON.GetMessage("I0017")

        If Me.TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L0 Then
            'TOPレベル
            If TV_1.SelectedNode.Name = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.DICTIONARY).GetEName Or _
               TV_1.SelectedNode.Name = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.V_VIEW).GetEName Then
                'ディクショナリ
                schemaName = vbNullString
                objectName = LV_1.SelectedItems(0).Text
                objectType = TV_1.SelectedNode.Name
                frm_showdata.SetObjectInfo(schemaName, objectType, objectName, partitionName, subpartitionName)
                If frm_showdata.Visible Then
                    frm_showdata.Focus()
                Else
                    frm_showdata.Visible = True
                End If
                ToolStripStatusLabel1.Text = vbNullString
            End If
        ElseIf Me.TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L1 Then
            '第二レベル
            If TV_1.SelectedNode.Parent.Name = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.DICTIONARY).GetEName Or _
               TV_1.SelectedNode.Parent.Name = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.V_VIEW).GetEName Then
                'ディクショナリ
                schemaName = vbNullString
                objectName = LV_1.SelectedItems(0).Text
                objectType = TV_1.SelectedNode.Parent.Name
                frm_showdata.SetObjectInfo(schemaName, objectType, objectName, partitionName, subpartitionName)
                If frm_showdata.Visible Then
                    frm_showdata.Focus()
                Else
                    frm_showdata.Visible = True
                End If
                ToolStripStatusLabel1.Text = vbNullString
            End If
        ElseIf TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L2 Then
            '第三レベル
            If TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE).GetEName Or _
               TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.VIEW).GetEName Or _
               TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.MATERIALIZED_VIEW).GetEName Or _
               TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_PARTITION).GetEName Or _
               TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_SUBPARTITION).GetEName Or _
               TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.PACKAGE).GetEName Or _
               TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.PACKAGE_BODY).GetEName Or _
               TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.FUNCTION).GetEName Or _
               TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.PROCEDURE).GetEName Or _
               TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TYPE).GetEName Or _
               TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.JAVA_SOURCE).GetEName Then

                'テーブル、ビュー、MV
                For Each obj As ListViewItem In LV_1.SelectedItems
                    ToolStripStatusLabel1.Text = msg
                    Me.Refresh()
                    schemaName = Me.TV_1.SelectedNode.Parent.Text
                    objectName = LV_1.SelectedItems(0).Text
                    objectType = TV_1.SelectedNode.Text
                    If TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_PARTITION).GetEName Then
                        partitionName = LV_1.SelectedItems(0).SubItems(1).Text
                    ElseIf TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_SUBPARTITION).GetEName Then
                        partitionName = LV_1.SelectedItems(0).SubItems(1).Text
                        subpartitionName = LV_1.SelectedItems(0).SubItems(2).Text
                    End If

                    frm_showdata.SetObjectInfo(schemaName, objectType, objectName, partitionName, subpartitionName)
                    If frm_showdata.Visible Then
                        frm_showdata.Focus()
                    Else
                        frm_showdata.Visible = True
                    End If
                    ToolStripStatusLabel1.Text = vbNullString
                    Exit For
                Next
            End If
        End If
    End Sub

    Private Sub DisplayTreeView()
        '#######################################################################
        'ツリービューの表示
        '#######################################################################
        Dim treeNode As TreeNode
        Dim tmpTreeNode As TreeNode

        'ROOT階層追加
        treeNode = Me.TV_1.Nodes.Add("SCHEMA", "スキーマ")
        'スキーマ追加
        GetSchema(treeNode)

        'オブジェクト
        For Each tmpTreeNode In treeNode.Nodes
            For Each obj As CLS_CONST.CLS_CONST.S_OBJECT In M_CONST.C_OBJECT
                '全スキーマ、スキーマ、ディクショナリ、V$ビュー、以外のオブジェクトを追加
                If Not obj.GetID = M_CONST.C_OBJECTID.SCHEMA And _
                    Not obj.GetID = M_CONST.C_OBJECTID.DICTIONARY And _
                    Not obj.GetID = M_CONST.C_OBJECTID.V_VIEW And _
                    Not obj.GetID = M_CONST.C_OBJECTID.ALL_SCHEMA Then
                    tmpTreeNode.Nodes.Add(obj.GetID.ToString, obj.GetEName)
                End If
            Next
        Next

        'ディクショナリ
        treeNode = Me.TV_1.Nodes.Add(M_CONST.C_OBJECT(M_CONST.C_OBJECTID.DICTIONARY).GetEName, M_CONST.C_OBJECT(M_CONST.C_OBJECTID.DICTIONARY).GetJName)
        treeNode.Nodes.Add("DBA", "DBAディクショナリ")
        treeNode.Nodes.Add("ALL", "ALLディクショナリ")
        treeNode.Nodes.Add("USER", "USERディクショナリ")
        treeNode.Nodes.Add("OTHER", "その他")

        '動的パフォーマンスビュー
        treeNode = Me.TV_1.Nodes.Add(M_CONST.C_OBJECT(M_CONST.C_OBJECTID.V_VIEW).GetEName, M_CONST.C_OBJECT(M_CONST.C_OBJECTID.V_VIEW).GetJName)
        treeNode.Nodes.Add("V$", "V$ビュー")
        treeNode.Nodes.Add("GV$", "GV$ビュー")
    End Sub

    Private Sub GetSchema(ByVal treeNode As TreeNode)
        '#######################################################################
        'スキーマ情報取得
        '#######################################################################
        Dim sql As String
        Dim datarow As DataRow

        sql = "SELECT USERNAME "
        sql = sql & "FROM ALL_USERS "
        sql = sql & "UNION ALL "
        sql = sql & "SELECT 'PUBLIC' "
        sql = sql & "FROM DUAL "
        sql = sql & "ORDER BY USERNAME"

        Using dataset As New DataSet
            'ツリービューにスキーマ追加()
            If frm_Login.G_DB.GET_DATASET(sql, dataset) Then
                For Each datarow In dataset.Tables(0).Rows
                    treeNode.Nodes.Add(datarow(0))
                    frm_Login.G_DB.G_USERS.Add(datarow(0))
                Next
            End If
        End Using
    End Sub

    Private Sub DisplayListView()
        '#######################################################################
        'リストビュー表示
        '#######################################################################
        Dim dataset As New DataSet()
        Dim datarowStrings() As String
        Dim displayRowCnt As Integer = 100 'データの表示単位
        Dim sumRowCnt As Integer '表示データ
        Dim tmpColumnCnt As Integer
        Dim datarow As DataRow
        Dim sql As String = vbNullString
        'Dim ColumnHeader As ColumnHeader
        Dim ColumnCnt As Integer = 0 'ListViewの列数
        Dim RowCnt As Integer = 0  '行数
        Dim dictionaryScope As Short = 0
        Dim SelectedType As String = vbNullString
        Dim SelectedOwner As String = vbNullString
        Dim SelectedLevel As Integer
        Dim backColor As Color = Color.White
        Dim foreColor As Color = Color.Black
        Dim font1 As Font = New Font(Font, FontStyle.Regular)
        Dim ListViewItems(displayRowCnt - 1) As ListViewItem
        Dim msg(1) As String

        If M_SELECTFLG Then
            '処理実行中の場合は処理を行なわない
            M_CANCELFLG = False
            M_SELECTFLG = False
            ToolStripStatusLabel1.Text = ""
            Exit Sub
        End If

        LV_1.Columns.Clear()
        LV_1.Items.Clear()

        If TV_1.SelectedNode Is Nothing Then
            Exit Sub
        End If
        SelectedLevel = TV_1.SelectedNode.Level
        'LV_1.Items.Clear()

        'ディクショナリへの参照権限の有無により参照ディクショナリの接頭辞を変更
        If frm_Login.G_DB.G_SELECT_DICTIONARY_FLG Then
            'ディクショナリ参照権限有
            dictionaryScope = 1
        Else
            'ディクショナリ参照権限無
            dictionaryScope = 0
        End If


        '選択されたオブジェクト毎にリストビュー列を設定
        If Me.TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L2 Then
            'オブジェクト階層を選択した場合オブジェクト一覧を追加する
            SelectedOwner = TV_1.SelectedNode.Parent.Text
            SelectedType = TV_1.SelectedNode.Text

            For Each obj As CLS_CONST.CLS_CONST.S_OBJECT In M_CONST.C_OBJECT
                If TV_1.SelectedNode.Name = obj.GetID.ToString Then
                    SetUpListViewColumns(obj.GetID)
                    SetContextMenu(obj.GetID)
                    Exit For
                End If
            Next
        ElseIf Me.TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L1 Then
            'スキーマ階層を選択した場合
            If Me.TV_1.SelectedNode.Parent.Name = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.DICTIONARY).GetEName Then
                'ディクショナリの場合
                SelectedOwner = TV_1.SelectedNode.Text
                SelectedType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.DICTIONARY).GetEName
                SetUpListViewColumns(M_CONST.C_OBJECTID.DICTIONARY)
                SetContextMenu(M_CONST.C_OBJECTID.DICTIONARY)
            ElseIf Me.TV_1.SelectedNode.Parent.Name = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.V_VIEW).GetEName Then
                '動的パフォーマンスビューの場合
                SelectedOwner = TV_1.SelectedNode.Text
                SelectedType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.V_VIEW).GetEName
                SetUpListViewColumns(M_CONST.C_OBJECTID.V_VIEW)
                SetContextMenu(M_CONST.C_OBJECTID.DICTIONARY)
            ElseIf Me.TV_1.SelectedNode.Parent.Name = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.SCHEMA).GetEName Then
                'スキーマオブジェクトの場合
                SelectedOwner = TV_1.SelectedNode.Text
                SelectedType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.SCHEMA).GetEName
                SetUpListViewColumns(M_CONST.C_OBJECTID.SCHEMA)
                SetContextMenu(M_CONST.C_OBJECTID.SCHEMA)
            End If

        ElseIf Me.TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L0 Then
            'TOP階層の場合
            If Me.TV_1.SelectedNode.Name = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.DICTIONARY).GetEName Then
                'ディクショナリの追加
                SelectedType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.DICTIONARY).GetEName
                SetUpListViewColumns(M_CONST.C_OBJECTID.DICTIONARY)
                SetContextMenu(M_CONST.C_OBJECTID.DICTIONARY)
            ElseIf Me.TV_1.SelectedNode.Name = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.V_VIEW).GetEName Then
                '動的パフォーマンスビューの追加
                SelectedType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.V_VIEW).GetEName
                SetUpListViewColumns(M_CONST.C_OBJECTID.V_VIEW)
                SetContextMenu(M_CONST.C_OBJECTID.V_VIEW)
            ElseIf Me.TV_1.SelectedNode.Name = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.SCHEMA).GetEName Then
                '全スキーマの表示
                SelectedType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.ALL_SCHEMA).GetEName
                SetUpListViewColumns(M_CONST.C_OBJECTID.ALL_SCHEMA)
                SetContextMenu(M_CONST.C_OBJECTID.ALL_SCHEMA)
            End If
        End If
        sql = CreateSql(SelectedOwner, SelectedType, dictionaryScope)

        '列数設定
        ColumnCnt = LV_1.Columns.Count
        ReDim datarowStrings(ColumnCnt)

        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0005")
        Me.Refresh()
        'SQL実行
        If frm_Login.G_DB.GET_DATASET(sql, dataset) = False Then
            ToolStripStatusLabel1.Text = vbNullString
            Exit Sub
        End If

        sumRowCnt = dataset.Tables(0).Rows.Count
        displayRowCnt = sumRowCnt / 10

        If sumRowCnt >= 1 And displayRowCnt = 0 Then
            displayRowCnt = 1
        End If
        ReDim ListViewItems(displayRowCnt - 1)

        For Each datarow In dataset.Tables(0).Rows
            RowCnt = RowCnt + 1
            M_SELECTFLG = True
            M_CANCELFLG = False
            For tmpColumnCnt = 0 To ColumnCnt - 1
                If Not datarow(tmpColumnCnt).ToString = vbNullString Then
                    datarowStrings(tmpColumnCnt) = datarow(tmpColumnCnt)
                Else
                    datarowStrings(tmpColumnCnt) = vbNullString
                End If
            Next tmpColumnCnt

            'ListViewItems((RowCnt - 1) Mod displayRowCnt) = New ListViewItem(datarowStrings, RowCnt - 1, foreColor, backColor, font1)
            ListViewItems((RowCnt - 1) Mod displayRowCnt) = New ListViewItem(datarowStrings, "", foreColor, backColor, font1)

            'displayRowCnt回毎に途中処理を実施
            If (RowCnt) Mod displayRowCnt = 0 Then
                'リストビューに追加
                LV_1.Items.AddRange(ListViewItems)
                If (sumRowCnt - RowCnt <= displayRowCnt) Then
                    'あまりの表示分
                    ReDim ListViewItems(sumRowCnt - (RowCnt + 1))
                End If
                msg(0) = sumRowCnt.ToString
                msg(1) = (RowCnt - 1).ToString
                ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0026", msg)

                Me.Refresh()
                System.Windows.Forms.Application.DoEvents()
                'キャンセル指示があった場合
                If M_CANCELFLG Then
                    SetLVColor()
                    MsgBox(M_COMMON.GetMessage("I0008"))
                    ToolStripStatusLabel1.Text = vbNullString
                    Exit For
                End If

                If SelectedLevel <> TV_1.SelectedNode.Level Then
                    '処理中に他の階層が選択された場合
                    M_CANCELFLG = False
                    M_SELECTFLG = False
                    ToolStripStatusLabel1.Text = vbNullString
                    Me.Refresh()
                    System.Windows.Forms.Application.DoEvents()
                    DisplayListView()
                    Exit Sub
                ElseIf TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L2 Then
                    If Not (SelectedOwner = TV_1.SelectedNode.Parent.Text And _
                       SelectedType = TV_1.SelectedNode.Text) Then
                        '別スキーマのオブジェクトを選択した場合
                        M_CANCELFLG = False
                        M_SELECTFLG = False
                        ToolStripStatusLabel1.Text = vbNullString
                        DisplayListView()
                        Exit Sub
                    End If
                ElseIf TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L1 Then
                    If Not (SelectedOwner = TV_1.SelectedNode.Text) Then
                        '別スキーマを選択した場合
                        M_CANCELFLG = False
                        M_SELECTFLG = False
                        ToolStripStatusLabel1.Text = vbNullString
                        DisplayListView()
                        Exit Sub
                    End If
                End If
            End If
        Next

        If M_CANCELFLG = False Then
            '残行をすべて表示
            LV_1.Items.AddRange(ListViewItems)
        End If

        SetLVColor()
        msg(0) = RowCnt.ToString
        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0009", msg)
        M_CANCELFLG = False
        M_SELECTFLG = False
        Me.Refresh()

        '列幅自動調整
        '以下は遅いので却下
        'For Each ColumnHeader In LV_1.Columns
        '    ColumnHeader.Width = -2
        'Next ColumnHeader
        LV_1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)

    End Sub

    Private Sub SetLVColor()
        '#######################################################################
        'リストビューの背景色設定
        '#######################################################################

        Dim backColor = Color.AliceBlue
        '背景色を白と青を交互に設定
        For Each lvItem As ListViewItem In LV_1.Items
            lvItem.BackColor = backColor
            If backColor = Color.AliceBlue Then
                backColor = Color.White
            Else
                backColor = Color.AliceBlue
            End If
        Next
    End Sub

    Private Function CreateSql(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成
        '#######################################################################
        Dim sql As String
        Dim bind(1) As M_CONST.S_BIND

        'SELECT
        sql = "SELECT B.OBJECT_NAME, B.STATUS, C.COMMENTS, NULL "
        'FROM句
        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_OBJECTS B, ALL_TAB_COMMENTS C "
        Else
            sql = sql & "FROM DBA_OBJECTS B, DBA_TAB_COMMENTS C "
        End If
        'WHERE句
        sql = sql & "WHERE B.OWNER = :OWNER AND C.OWNER(+) = :OWNER AND B.OBJECT_NAME = C.TABLE_NAME(+) AND B.OBJECT_TYPE = :OBJECTTYPE "
        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(B.OBJECT_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        '10g以降の場合リサイクルビンを考慮
        If Short.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 And _
           objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE).GetEName Then
            sql = sql + "AND B.OBJECT_NAME IN (SELECT TABLE_NAME FROM ALL_TABLES D WHERE D.OWNER = :OWNER)"
        End If

        'ORDER BY句
        sql = sql + "ORDER BY B.OBJECT_NAME"

        'この関数内でバインドしているのは未開発
        If objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.DATABASE_LINK).GetEName Then
            sql = CreateSqlDblink(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.CLUSTER).GetEName Then
            sql = CreateSqlCluster(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.FUNCTION).GetEName Then
            bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.INDEX).GetEName Then
            sql = CreateSqlIndex(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.INDEX_PARTITION).GetEName Then
            sql = CreateSqlIndexPartition(TV_1.SelectedNode.Parent.Text, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.INDEX_SUBPARTITION).GetEName Then
            sql = CreateSqlIndexSubpartition(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.JAVA_CLASS).GetEName Then
            bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.JAVA_SOURCE).GetEName Then
            bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.LIBRARY).GetEName Then
            sql = CreateSqlLibrary(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.LOB).GetEName Then
            sql = CreateSqlLob(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.MATERIALIZED_VIEW).GetEName Then
            sql = CreateSqlMView(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.PACKAGE).GetEName Then
            bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.PACKAGE_BODY).GetEName Then
            bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.PROCEDURE).GetEName Then
            bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.SCHEMA).GetEName Then
            sql = CreateSqlSchemaObject(owner, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.SEQUENCE).GetEName Then
            sql = CreateSqlSequence(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.SYNONYM).GetEName Then
            sql = CreateSqlSynonym(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE).GetEName Then
            sql = CreateSqlTable(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_PARTITION).GetEName Then
            sql = CreateSqlTablePartition(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_SUBPARTITION).GetEName Then
            sql = CreateSqlTableSubpartition(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TRIGGER).GetEName Then
            sql = CreateSqlTrigger(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TYPE).GetEName Then
            sql = CreateSqlType(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.VIEW).GetEName Then
            bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
            frm_Login.G_DB.SET_BIND_VARIABLE(bind)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.LOB_PARTITION).GetEName Then
            sql = CreateSqlLobPartition(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.LOB_SUBPARTITION).GetEName Then
            sql = CreateSqlLobSubPartition(owner, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.DICTIONARY).GetEName Then
            sql = CreateSqlDictionary(TV_1.SelectedNode.Name, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.V_VIEW).GetEName Then
            sql = CreateSqlV_View(TV_1.SelectedNode.Name, objectType, dictionaryScope)
        ElseIf objectType = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.ALL_SCHEMA).GetEName Then
            sql = CreateSqlAllSchema(dictionaryScope.ToString)
        End If

        Return sql
    End Function

    Private Function CreateSqlSchemaObject(ByVal owner As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(スキーマ)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(0) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT E.OBJECT_TYPE, E.OBJECT_NAME, E.SUBOBJECT_NAME, CREATED, LAST_DDL_TIME, E.STATUS "
        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_OBJECTS E "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_OBJECTS E "
        End If
        sql = sql & "WHERE E.OWNER = :OWNER "

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.OBJECT_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + "ORDER BY E.OBJECT_TYPE, E.OBJECT_NAME"

        Return sql
    End Function

    Private Function CreateSqlTable(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(テーブル)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(1) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT /*+ ORDERED USE_HASH(E D C) */ E.TABLE_NAME, E.TABLESPACE_NAME, D.EXTENTS, " & _
                "D.BYTES/1024, E.LAST_ANALYZED, E.NUM_ROWS, E.AVG_ROW_LEN, C.COMMENTS, NULL "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_TABLES E, (SELECT SEGMENT_NAME,EXTENTS,BYTES FROM USER_SEGMENTS WHERE SEGMENT_TYPE = :OBJECTTYPE) D, ALL_TAB_COMMENTS C "
        Else
            sql = sql & "FROM DBA_TABLES E, (SELECT SEGMENT_NAME,EXTENTS,BYTES FROM DBA_SEGMENTS WHERE OWNER = :OWNER AND SEGMENT_TYPE = :OBJECTTYPE) D, DBA_TAB_COMMENTS C "
        End If

        sql = sql & "WHERE E.OWNER = :OWNER AND E.OWNER = C.OWNER(+) AND E.TABLE_NAME = C.TABLE_NAME(+) AND E.TABLE_NAME = D.SEGMENT_NAME(+) "

        If dictionaryScope = 0 Then
            sql = sql & "AND E.TABLE_NAME NOT IN (SELECT MVIEW_NAME FROM ALL_MVIEWS M WHERE M.OWNER=E.OWNER) "
        Else
            sql = sql & "AND E.TABLE_NAME NOT IN (SELECT MVIEW_NAME FROM DBA_MVIEWS M WHERE M.OWNER=E.OWNER) AND C.OWNER = :OWNER "
        End If

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.TABLE_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + "ORDER BY E.TABLE_NAME"

        Return sql
    End Function

    Private Function CreateSqlTablePartition(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(テーブルパーティション)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(1) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT /*+ ORDERED USE_HASH(E D) */ E.TABLE_NAME, E.PARTITION_NAME, E.SUBPARTITION_COUNT, "
        sql = sql & "D.TABLESPACE_NAME, D.BYTES/1024, D.EXTENTS, E.LAST_ANALYZED, E.NUM_ROWS, E.AVG_ROW_LEN, NULL "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_TAB_PARTITIONS E, USER_SEGMENTS D "
        Else
            sql = sql & "FROM DBA_TAB_PARTITIONS E, DBA_SEGMENTS D "
        End If

        sql = sql & "WHERE E.TABLE_OWNER = :OWNER  AND "
        sql = sql & ":OBJECTTYPE = D.SEGMENT_TYPE(+) AND "
        sql = sql & "E.TABLE_NAME = D.SEGMENT_NAME(+) AND "
        sql = sql & "E.PARTITION_NAME = D.PARTITION_NAME(+) "
        'リサイクルビンは表示しない
        If Short.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
            If dictionaryScope = 0 Then
                sql = sql & "AND NOT EXISTS (SELECT 'A' FROM USER_RECYCLEBIN F WHERE F.OBJECT_NAME = E.TABLE_NAME) "
            Else
                sql = sql & "AND NOT EXISTS (SELECT 'A' FROM DBA_RECYCLEBIN F WHERE F.OBJECT_NAME = E.TABLE_NAME) "
            End If
        End If

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.PARTITION_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        If dictionaryScope = 1 Then
            sql = sql & "AND E.TABLE_OWNER = D.OWNER(+) "
        End If

        sql = sql & " ORDER BY E.TABLE_NAME,E.PARTITION_NAME"

        Return sql
    End Function

    Private Function CreateSqlTableSubpartition(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(テーブルサブパーティション)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(1) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT /*+ ORDERED USE_HASH(E D) */ E.TABLE_NAME, E.PARTITION_NAME, E.SUBPARTITION_NAME, "
        sql = sql & "D.TABLESPACE_NAME, D.BYTES/1024, D.EXTENTS, E.LAST_ANALYZED, E.NUM_ROWS, E.AVG_ROW_LEN, NULL "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_TAB_SUBPARTITIONS E, USER_SEGMENTS D "
        Else
            sql = sql & "FROM DBA_TAB_SUBPARTITIONS E, DBA_SEGMENTS D "
        End If

        sql = sql & "WHERE " & _
            "E.TABLE_OWNER = :OWNER AND " & _
            "D.SEGMENT_TYPE(+) = :OBJECTTYPE AND E.TABLE_NAME = D.SEGMENT_NAME(+) AND " & _
            "E.SUBPARTITION_NAME = D.PARTITION_NAME(+) "
        'リサイクルビンは表示しない
        If Short.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
            If dictionaryScope = 0 Then
                sql = sql & "AND NOT EXISTS (SELECT 'A' FROM USER_RECYCLEBIN F WHERE F.OBJECT_NAME = E.TABLE_NAME) "
            Else
                sql = sql & "AND NOT EXISTS (SELECT 'A' FROM DBA_RECYCLEBIN F WHERE F.OBJECT_NAME = E.TABLE_NAME) "
            End If
        End If

        If dictionaryScope = 1 Then
            sql = sql & "AND E.TABLE_OWNER = D.OWNER(+) "
        End If

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.SUBPARTITION_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + " ORDER BY E.TABLE_NAME,E.PARTITION_NAME,E.SUBPARTITION_NAME"

        Return sql
    End Function

    Private Function CreateSqlIndex(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(インデックス)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(1) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT /*+ ORDERED USE_HASH(E D) */ E.TABLE_NAME, E.INDEX_NAME, D.TABLESPACE_NAME, " & _
              "D.EXTENTS, D.BYTES/1024, E.BLEVEL, E.LEAF_BLOCKS, E.DISTINCT_KEYS, E.CLUSTERING_FACTOR, E.LAST_ANALYZED, E.NUM_ROWS "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_INDEXES E, USER_SEGMENTS D "
        Else
            sql = sql & "FROM DBA_INDEXES E, DBA_SEGMENTS D "
        End If

        sql = sql & "WHERE " & _
            "E.OWNER = :OWNER AND E.INDEX_NAME = D.SEGMENT_NAME(+) AND " & _
            " :OBJECTTYPE = D.SEGMENT_TYPE(+) "

        If dictionaryScope = 1 Then
            sql = sql & "AND E.OWNER = D.OWNER(+) "
        End If

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.INDEX_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + " ORDER BY E.TABLE_NAME,E.INDEX_NAME"

        Return sql
    End Function

    Private Function CreateSqlIndexPartition(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(インデックスパーティション)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(1) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT /*+ ORDERED USE_HASH(E D) */ E.INDEX_NAME, E.PARTITION_NAME, E.SUBPARTITION_COUNT, " & _
                 "D.TABLESPACE_NAME, D.BYTES/1024, D.EXTENTS, E.LAST_ANALYZED, E.NUM_ROWS, E.BLEVEL, E.LEAF_BLOCKS "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_IND_PARTITIONS E, USER_SEGMENTS D "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_IND_PARTITIONS E, DBA_SEGMENTS D "
        End If

        sql = sql & "WHERE " & _
            "E.INDEX_OWNER = :OWNER AND " & _
            "D.SEGMENT_TYPE(+) = :OBJECTTYPE AND E.INDEX_NAME = D.SEGMENT_NAME(+) AND " & _
            "E.PARTITION_NAME = D.PARTITION_NAME(+) "
        'リサイクルビンは表示しない
        If Short.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
            If dictionaryScope = 0 Then
                sql = sql & "AND NOT EXISTS (SELECT 'A' FROM USER_RECYCLEBIN F WHERE F.OBJECT_NAME = E.INDEX_NAME) "
            Else
                sql = sql & "AND NOT EXISTS (SELECT 'A' FROM DBA_RECYCLEBIN F WHERE F.OBJECT_NAME = E.INDEX_NAME) "
            End If
        End If

        If dictionaryScope = 1 Then
            sql = sql & "AND E.INDEX_OWNER = D.OWNER(+) "
        End If

        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.PARTITION_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + " ORDER BY E.INDEX_NAME,E.PARTITION_NAME"

        Return sql
    End Function

    Private Function CreateSqlIndexSubpartition(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(インデックスサブパーティション)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(1) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT /*+ ORDERED USE_HASH(E D) */ E.INDEX_NAME, E.PARTITION_NAME, E.SUBPARTITION_NAME, D.TABLESPACE_NAME, " & _
            "D.BYTES/1024, D.EXTENTS, E.LAST_ANALYZED, E.NUM_ROWS, E.BLEVEL, E.LEAF_BLOCKS "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_IND_SUBPARTITIONS E, USER_SEGMENTS D "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_IND_SUBPARTITIONS E, DBA_SEGMENTS D "
        End If

        sql = sql & "WHERE " & _
            "E.INDEX_OWNER = :OWNER AND " & _
            "D.SEGMENT_TYPE(+) = :OBJECTTYPE AND E.INDEX_NAME = D.SEGMENT_NAME(+) AND " & _
            "E.SUBPARTITION_NAME = D.PARTITION_NAME(+) "
        'リサイクルビンは表示しない
        If Short.Parse(frm_Login.G_DB.G_ORA_VERSION(1)) >= 10 Then
            If dictionaryScope = 0 Then
                sql = sql & "AND NOT EXISTS (SELECT 'A' FROM USER_RECYCLEBIN F WHERE F.OBJECT_NAME = E.INDEX_NAME) "
            Else
                sql = sql & "AND NOT EXISTS (SELECT 'A' FROM DBA_RECYCLEBIN F WHERE F.OBJECT_NAME = E.INDEX_NAME) "
            End If
        End If

        If dictionaryScope = 1 Then
            sql = sql & "AND E.INDEX_OWNER = D.OWNER(+) "
        End If

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.SUBPARTITION_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        'ORDER BY句
        sql = sql + " ORDER BY E.INDEX_NAME, E.PARTITION_NAME,E.SUBPARTITION_NAME"

        Return sql
    End Function

    Private Function CreateSqlLob(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(LOB)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(1) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT /*+ ORDERED USE_HASH(E D) */ E.TABLE_NAME, E.COLUMN_NAME, E.SEGMENT_NAME, D.TABLESPACE_NAME, D.BYTES/1024, D.EXTENTS "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_LOBS E, USER_SEGMENTS D "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_LOBS E, DBA_SEGMENTS D "
        End If

        sql = sql & "WHERE " & _
            "E.OWNER = :OWNER AND D.SEGMENT_TYPE(+) = :OBJECTTYPE AND E.SEGMENT_NAME = D.SEGMENT_NAME(+) "
        If dictionaryScope = 1 Then
            sql = sql & "AND E.OWNER = D.OWNER(+) "
        End If

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.SEGMENT_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + "ORDER BY E.TABLE_NAME,E.COLUMN_NAME, E.SEGMENT_NAME"

        Return sql
    End Function

    Private Function CreateSqlLobPartition(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(LOB PARTITION)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(1) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT /*+ ORDERED USE_HASH(E D) */ E.TABLE_NAME, E.COLUMN_NAME, E.LOB_NAME, E.PARTITION_NAME,E.LOB_PARTITION_NAME, " & _
                        "D.TABLESPACE_NAME,  D.BYTES/1024, D.EXTENTS "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_LOB_PARTITIONS E, USER_SEGMENTS D "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_LOB_PARTITIONS E, DBA_SEGMENTS D "
        End If

        sql = sql & "WHERE " & _
            "E.TABLE_OWNER = :OWNER " + "AND D.SEGMENT_TYPE(+) = :OBJECTTYPE AND E.LOB_NAME = D.SEGMENT_NAME(+) AND E.LOB_PARTITION_NAME = D.PARTITION_NAME(+) "
        If dictionaryScope = 1 Then
            sql = sql & "AND E.TABLE_OWNER = D.OWNER(+) "
        End If

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.LOB_PARTITION_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + "ORDER BY E.TABLE_NAME,E.COLUMN_NAME, E.LOB_NAME, E.PARTITION_NAME,E.LOB_PARTITION_NAME"

        Return sql
    End Function

    Private Function CreateSqlLobSubPartition(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(LOB PARTITION)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(1) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT /*+ ORDERED USE_HASH(E D) */ E.TABLE_NAME, E.COLUMN_NAME, E.LOB_NAME, E.LOB_PARTITION_NAME,E.SUBPARTITION_NAME, " & _
                        "E.LOB_SUBPARTITION_NAME, D.TABLESPACE_NAME,  D.BYTES/1024, D.EXTENTS "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_LOB_SUBPARTITIONS E, USER_SEGMENTS D "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_LOB_SUBPARTITIONS E, DBA_SEGMENTS D "
        End If

        sql = sql & "WHERE " & _
            "E.TABLE_OWNER = :OWNER " + "AND D.SEGMENT_TYPE(+) = :OBJECTTYPE AND E.LOB_NAME = D.SEGMENT_NAME(+) AND E.LOB_SUBPARTITION_NAME = D.PARTITION_NAME(+) "
        If dictionaryScope = 1 Then
            sql = sql & "AND E.TABLE_OWNER = D.OWNER(+) "
        End If

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.LOB_SUBPARTITION_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + "ORDER BY E.TABLE_NAME,E.COLUMN_NAME, E.LOB_NAME, E.LOB_PARTITION_NAME,E.SUBPARTITION_NAME,E.LOB_SUBPARTITION_NAME"

        Return sql
    End Function

    Private Function CreateSqlMView(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(MV)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(0) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT B.MVIEW_NAME, LAST_REFRESH_TYPE, LAST_REFRESH_DATE, D.TABLESPACE_NAME, D.EXTENTS, D.BYTES/1024, B.QUERY, B.UPDATABLE, B.REFRESH_MODE, B.REFRESH_METHOD "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_MVIEWS B, USER_SEGMENTS D "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_MVIEWS B, DBA_SEGMENTS D "
        End If

        sql = sql & "WHERE B.OWNER = :OWNER "
        If dictionaryScope = 1 Then
            sql = sql & "AND D.OWNER(+) = :OWNER "
        End If
        sql = sql & "AND B.MVIEW_NAME = D.SEGMENT_NAME(+) "

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(B.MVIEW_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + "ORDER BY B.MVIEW_NAME"

        Return sql
    End Function

    Private Function CreateSqlDblink(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(データベースリンク)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(0) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT E.DB_LINK, E.HOST, E.USERNAME "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_DB_LINKS E "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_DB_LINKS E "
        End If

        sql = sql & "WHERE " & _
            "E.OWNER = :OWNER "

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.DB_LINK) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + "ORDER BY E.DB_LINK"

        Return sql
    End Function

    Private Function CreateSqlType(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(タイプ)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(0) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT E.TYPE_NAME "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_TYPES E "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_TYPES E "
        End If

        sql = sql & "WHERE "
        sql = sql & "E.OWNER = :OWNER "

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.TYPE_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If
        sql = sql + "ORDER BY E.TYPE_NAME"

        Return sql
    End Function

    Private Function CreateSqlSequence(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(シーケンス)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(0) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT E.SEQUENCE_NAME, E.LAST_NUMBER, E.INCREMENT_BY, E.MIN_VALUE, E.MAX_VALUE, E.CYCLE_FLAG, E.ORDER_FLAG, E.CACHE_SIZE "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_SEQUENCES E "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_SEQUENCES E "
        End If

        sql = sql & "WHERE E.SEQUENCE_OWNER = :OWNER "

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.SEQUENCE_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + "ORDER BY E.SEQUENCE_NAME"

        Return sql
    End Function

    Private Function CreateSqlTrigger(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(トリガー)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(0) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT E.TRIGGER_NAME, E.STATUS, E.TRIGGER_TYPE, E.TRIGGERING_EVENT, " & _
                     "E.TABLE_OWNER, E.BASE_OBJECT_TYPE, E.TABLE_NAME, E.COLUMN_NAME "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_TRIGGERS E "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_TRIGGERS E "
        End If

        sql = sql & "WHERE E.OWNER = :OWNER "

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.TRIGGER_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + "ORDER BY E.TRIGGER_NAME"

        Return sql
    End Function

    Private Function CreateSqlSynonym(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(シノニム)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(0) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT E.SYNONYM_NAME, E.TABLE_OWNER, E.TABLE_NAME, E.DB_LINK "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_SYNONYMS E "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_SYNONYMS E "
        End If

        sql = sql & "WHERE E.OWNER = :OWNER "

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.SYNONYM_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + "ORDER BY E.SYNONYM_NAME"

        Return sql
    End Function

    Private Function CreateSqlLibrary(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(ライブラリ)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(0) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT E.LIBRARY_NAME, E.STATUS, E.DYNAMIC, E.FILE_SPEC "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_LIBRARIES E "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_LIBRARIES E "
        End If

        sql = sql & "WHERE E.OWNER = :OWNER "

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.LIBRARY_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + "ORDER BY E.LIBRARY_NAME"

        Return sql
    End Function

    Private Function CreateSqlCluster(ByVal owner As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(テーブル)
        '#######################################################################
        Dim sql As String = vbNullString
        Dim bind(1) As M_CONST.S_BIND

        bind(0).SET_BIND(":OWNER", owner, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        bind(1).SET_BIND(":OBJECTTYPE", objectType, M_CONST.C_COLUMN_TYPE.STRING, ParameterDirection.Input, 0)
        frm_Login.G_DB.SET_BIND_VARIABLE(bind)

        sql = "SELECT /*+ ORDERED USE_HASH(E D) */ E.CLUSTER_NAME, E.TABLESPACE_NAME, D.BYTES/1024, D.EXTENTS "

        If dictionaryScope = 0 Then
            sql = sql & "FROM ALL_CLUSTERS E, USER_SEGMENTS D "
        ElseIf dictionaryScope = 1 Then
            sql = sql & "FROM DBA_CLUSTERS E, DBA_SEGMENTS D "
        End If

        sql = sql & "WHERE E.OWNER = :OWNER AND E.CLUSTER_NAME = D.SEGMENT_NAME(+) AND D.SEGMENT_TYPE(+) = :OBJECTTYPE "
        If dictionaryScope = 1 Then
            sql = sql & "AND E.OWNER = D.OWNER(+) "
        End If

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(E.CLUSTER_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql + "ORDER BY E.CLUSTER_NAME"

        Return sql
    End Function

    Private Function CreateSqlDictionary(ByVal dictionaryType As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(ディクショナリ)
        '#######################################################################
        Dim sql As String = vbNullString

        sql = "SELECT TABLE_NAME,COMMENTS "

        sql = sql & "FROM DICT "

        sql = sql & "WHERE "

        If dictionaryType = "DBA" Then
            sql = sql & "TABLE_NAME LIKE 'DBA%' "
        ElseIf dictionaryType = "ALL" Then
            sql = sql & "TABLE_NAME LIKE 'ALL%' "
        ElseIf dictionaryType = "USER" Then
            sql = sql & "TABLE_NAME LIKE 'USER%' "
        ElseIf dictionaryType = "OTHER" Then
            sql = sql & "(TABLE_NAME NOT LIKE 'DBA%' AND TABLE_NAME NOT LIKE 'ALL%' AND TABLE_NAME NOT LIKE 'USER%' AND TABLE_NAME NOT LIKE 'V$%' AND TABLE_NAME NOT LIKE 'GV$%') "
        ElseIf dictionaryType = "DICTIONARY" Then
            sql = sql & "(TABLE_NAME NOT LIKE 'V$%' AND TABLE_NAME NOT LIKE 'GV$%') "
        End If

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(TABLE_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql & "ORDER BY TABLE_NAME "

        Return sql
    End Function

    Private Function CreateSqlV_View(ByVal dictionaryType As String, ByVal objectType As String, ByVal dictionaryScope As Short) As String
        '#######################################################################
        'SQL文作成(動的パフォーマンスビュー)
        '#######################################################################
        Dim sql As String = vbNullString

        sql = "SELECT TABLE_NAME,COMMENTS "

        sql = sql & "FROM DICT "

        sql = sql & "WHERE "

        If dictionaryType = "V$" Then
            sql = sql & "TABLE_NAME LIKE 'V$%' "
        ElseIf dictionaryType = "GV$" Then
            sql = sql & "TABLE_NAME LIKE 'GV$%' "
        ElseIf dictionaryType = "V_VIEW" Then
            sql = sql & "(TABLE_NAME LIKE 'V$%' OR TABLE_NAME LIKE 'GV$%') "
        End If

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " AND UPPER(TABLE_NAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql & "ORDER BY TABLE_NAME "

        Return sql
    End Function

    Private Function CreateSqlAllSchema(ByVal dictionaryScope As String) As String
        '#######################################################################
        'スキーマ情報取得
        '#######################################################################
        Dim sql As String = vbNullString

        If Integer.Parse(dictionaryScope) = 1 Then
            sql = "SELECT USERNAME,ACCOUNT_STATUS,LOCK_DATE,EXPIRY_DATE,DEFAULT_TABLESPACE,TEMPORARY_TABLESPACE,CREATED,PROFILE,INITIAL_RSRC_CONSUMER_GROUP,EXTERNAL_NAME "
            sql = sql & "FROM DBA_USERS "
        Else
            sql = "SELECT USERNAME,NULL,NULL,NULL,NULL,NULL,CREATED,NULL,NULL,NULL "
            sql = sql & "FROM ALL_USERS "
        End If

        '画面検索条件
        If TXT_JOKEN.Text <> vbNullString Then
            sql = sql & " WHERE UPPER(USERNAME) LIKE UPPER('%" & TXT_JOKEN.Text & "%') "
        End If

        sql = sql & "ORDER BY USERNAME "

        Return sql
    End Function

    Private Sub SetClipboard()
        '#######################################################################
        'クリップボードセット
        '#######################################################################
        Dim ColCnt As Integer
        Dim copydata As StringBuilder = New StringBuilder
        Dim isStart As Integer = True

        ColCnt = LV_1.Columns.Count
        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0015")
        Me.Refresh()

        For Each selectedItem As ListViewItem In LV_1.SelectedItems
            isStart = True
            For Each subItem As ListViewItem.ListViewSubItem In selectedItem.SubItems
                If isStart Then
                    copydata.Append(subItem.Text)
                    isStart = False
                Else
                    copydata.Append(ControlChars.Tab & subItem.Text)
                End If
            Next

            copydata.Append(ControlChars.CrLf)
        Next

        If copydata.ToString = vbNullString Then
            ToolStripStatusLabel1.Text = M_COMMON.GetMessage("E0005")
        Else
            Clipboard.SetText(copydata.ToString)
            ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0016")
        End If
    End Sub

    Private Sub GetTableCnt()
        '#######################################################################
        'テーブル件数取得
        '#######################################################################
        Dim SchemaName As String
        Dim ObjectName As String
        Dim PartitionName As String = vbNullString
        Dim ColumnCnt As Integer
        Dim sql As String = vbNullString

        If Me.TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L2 And _
           (TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE).GetEName Or _
            TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.VIEW).GetEName Or _
            TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.MATERIALIZED_VIEW).GetEName Or _
            TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_PARTITION).GetEName Or _
            TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_SUBPARTITION).GetEName) Then

            M_SELECTFLG = True
            SchemaName = ControlChars.Quote & TV_1.SelectedNode.Parent.Text & ControlChars.Quote
            ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0027")
            For Each listViewItem As ListViewItem In LV_1.Items
                If listViewItem.Selected Then
                    '件数データ取得
                    ObjectName = ControlChars.Quote & listViewItem.Text & ControlChars.Quote

                    Using dataset As New DataSet
                        Me.Refresh()
                        sql = "SELECT /*+ FIRST_ROWS */ COUNT(*) FROM " & SchemaName & "." & ObjectName

                        'パーティション
                        If TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_PARTITION).GetEName Then
                            sql = sql & " PARTITION(""" & LV_1.SelectedItems(0).SubItems(1).Text & """) "
                        ElseIf TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_SUBPARTITION).GetEName Then
                            sql = sql & " SUBPARTITION(""" & LV_1.SelectedItems(0).SubItems(2).Text & """) "
                        End If

                        'SQL実行
                        If frm_Login.G_DB.GET_DATASET(sql, dataset) = False Then
                            ToolStripStatusLabel1.Text = M_COMMON.GetMessage("E0001")
                            Exit Sub
                        End If
                        ColumnCnt = LV_1.Columns.Count

                        listViewItem.SubItems(ColumnCnt - 1).Text = dataset.Tables(0).Rows(0).Item(0)
                        listViewItem.Selected = False
                        Me.Refresh()
                    End Using

                    System.Windows.Forms.Application.DoEvents()
                    'キャンセル指示があった場合
                    If M_CANCELFLG Then
                        M_SELECTFLG = False
                        M_CANCELFLG = False
                        MsgBox(M_COMMON.GetMessage("I0008"))
                        Exit For
                    End If
                End If
            Next
            M_SELECTFLG = False
            M_CANCELFLG = False
            ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0011")
        End If
    End Sub

    Private Sub ExecTruncate()
        '#######################################################################
        'TRUNCATE実行
        '#######################################################################

        Dim schemaName As String
        Dim tableName As String
        Dim partName As String
        Dim cnt As Long
        Dim resultCode As Integer

        resultCode = MsgBox(M_COMMON.GetMessage("W0002"), MsgBoxStyle.YesNo, "確認")
        If resultCode = vbYes Then
            If Me.TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L2 Then
                M_SELECTFLG = True
                schemaName = ControlChars.Quote & TV_1.SelectedNode.Parent.Text & ControlChars.Quote

                ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0027")

                For Each listViewItem As ListViewItem In LV_1.SelectedItems
                    Me.Refresh()
                    If TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE).GetEName Or _
                       TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.MATERIALIZED_VIEW).GetEName Then
                        'テーブル、MV
                        tableName = ControlChars.Quote & listViewItem.Text & ControlChars.Quote
                        If frm_Login.G_DB.EXEC_NONQUERY("TRUNCATE TABLE " & schemaName & "." & tableName, cnt) = False Then
                            ToolStripStatusLabel1.Text = M_COMMON.GetMessage("E0001")
                            Exit For
                        End If
                    ElseIf TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_PARTITION).GetEName Then
                        'パーティション、サブパーティション
                        tableName = ControlChars.Quote & listViewItem.Text & ControlChars.Quote
                        partName = ControlChars.Quote & listViewItem.SubItems(1).Text & ControlChars.Quote
                        If frm_Login.G_DB.EXEC_NONQUERY("ALTER TABLE " & schemaName & "." & tableName & " TRUNCATE PARTITION " & partName, cnt) = False Then
                            ToolStripStatusLabel1.Text = M_COMMON.GetMessage("E0001")
                            Exit For
                        End If
                    ElseIf TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_SUBPARTITION).GetEName Then
                        tableName = ControlChars.Quote & listViewItem.Text & ControlChars.Quote
                        partName = ControlChars.Quote & listViewItem.SubItems(2).Text & ControlChars.Quote
                        If frm_Login.G_DB.EXEC_NONQUERY("ALTER TABLE " & schemaName & "." & tableName & " TRUNCATE SUBPARTITION " & partName, cnt) = False Then
                            ToolStripStatusLabel1.Text = M_COMMON.GetMessage("E0001")
                            Exit For
                        End If
                    End If


                    listViewItem.Selected = False
                    Me.Refresh()

                    System.Windows.Forms.Application.DoEvents()
                    'キャンセル指示があった場合
                    If M_CANCELFLG Then
                        M_SELECTFLG = False
                        M_CANCELFLG = False
                        MsgBox(M_COMMON.GetMessage("I0008"))
                        Exit For
                    End If
                Next
                M_SELECTFLG = False
                M_CANCELFLG = False
                ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0011")
            End If
        End If
    End Sub

    Private Sub ExecGetStats()
        '#######################################################################
        '統計情報取得
        '#######################################################################
        Dim schemaName As String
        Dim objectName As String
        Dim partname As String
        Dim objectType As String
        Dim cnt As Long
        Dim resultCode As Integer
        Dim sql As String = vbNullString

        resultCode = MsgBox(M_COMMON.GetMessage("W0005"), MsgBoxStyle.YesNoCancel, "確認")
        If resultCode = vbYes Then
            If Me.TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L2 Then
                M_SELECTFLG = True
                schemaName = ControlChars.Quote & TV_1.SelectedNode.Parent.Text & ControlChars.Quote
                objectType = TV_1.SelectedNode.Text
                ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0027")

                For Each listViewItem As ListViewItem In LV_1.SelectedItems
                    '統計情報取得

                    Me.Refresh()

                    If TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE).GetEName Or _
                       TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.MATERIALIZED_VIEW).GetEName Then
                        objectName = ControlChars.Quote & listViewItem.Text & ControlChars.Quote
                        sql = "DECLARE BEGIN DBMS_STATS.GATHER_TABLE_STATS(OWNNAME=>'" & schemaName & "',TABNAME=>'" & objectName & "',CASCADE=>FALSE); END;"
                    ElseIf TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_PARTITION).GetEName Then
                        objectName = ControlChars.Quote & listViewItem.Text & ControlChars.Quote
                        partname = ControlChars.Quote & listViewItem.SubItems(1).Text & ControlChars.Quote
                        sql = "DECLARE BEGIN DBMS_STATS.GATHER_TABLE_STATS(OWNNAME=>'" & schemaName & "',TABNAME=>'" & objectName & "',PARTNAME=>'" & partname & "',CASCADE=>FALSE,GRANULARITY=>'PARTITION'); END;"
                    ElseIf TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.TABLE_SUBPARTITION).GetEName Then
                        objectName = ControlChars.Quote & listViewItem.Text & ControlChars.Quote
                        partname = ControlChars.Quote & listViewItem.SubItems(2).Text & ControlChars.Quote
                        sql = "DECLARE BEGIN DBMS_STATS.GATHER_TABLE_STATS(OWNNAME=>'" & schemaName & "',TABNAME=>'" & objectName & "',PARTNAME=>'" & partname & "',CASCADE=>FALSE,GRANULARITY=>'SUBPARTITION'); END;"
                    ElseIf TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.INDEX).GetEName Then
                        objectName = ControlChars.Quote & listViewItem.SubItems(1).Text & ControlChars.Quote
                        sql = "DECLARE BEGIN DBMS_STATS.GATHER_INDEX_STATS(OWNNAME=>'" & schemaName & "',INDNAME=>'" & objectName & "'); END;"
                    ElseIf TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.INDEX_PARTITION).GetEName Then
                        objectName = ControlChars.Quote & listViewItem.Text & ControlChars.Quote
                        partname = ControlChars.Quote & listViewItem.SubItems(1).Text & ControlChars.Quote
                        sql = "DECLARE BEGIN DBMS_STATS.GATHER_INDEX_STATS(OWNNAME=>'" & schemaName & "',INDNAME=>'" & objectName & "'PARTNAME=>'" & partname & "',GRANULARITY=>'PARTITION'); END;"
                    ElseIf TV_1.SelectedNode.Text = M_CONST.C_OBJECT(M_CONST.C_OBJECTID.INDEX_SUBPARTITION).GetEName Then
                        objectName = ControlChars.Quote & listViewItem.Text & ControlChars.Quote
                        partname = ControlChars.Quote & listViewItem.SubItems(1).Text & ControlChars.Quote
                        sql = "DECLARE BEGIN DBMS_STATS.GATHER_INDEX_STATS(OWNNAME=>'" & schemaName & "',INDNAME=>'" & objectName & "'PARTNAME=>'" & partname & "',GRANULARITY=>'SUBPARTITION'); END;"
                    End If

                    If frm_Login.G_DB.EXEC_NONQUERY(sql, cnt) = False Then
                        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("E0001")
                        Exit Sub
                    End If
                    listViewItem.Selected = False
                    Me.Refresh()
                    System.Windows.Forms.Application.DoEvents()
                    'キャンセル指示があった場合
                    If M_CANCELFLG Then
                        M_SELECTFLG = False
                        M_CANCELFLG = False
                        MsgBox(M_COMMON.GetMessage("I0008"))
                        Exit For
                    End If
                Next
            ElseIf Me.TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L0 Then
                M_SELECTFLG = True
                ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0027")
                Me.Refresh()
                For Each listViewItem As ListViewItem In LV_1.SelectedItems
                    schemaName = ControlChars.Quote & listViewItem.Text & ControlChars.Quote
                    If frm_Login.G_DB.EXEC_NONQUERY("DECLARE BEGIN DBMS_STATS.GATHER_SCHEMA_STATS(OWNNAME=>'" & schemaName & "'); END;", cnt) = False Then
                        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("E0001")
                        Exit Sub
                    End If
                Next
                System.Windows.Forms.Application.DoEvents()
                'キャンセル指示があった場合
                If M_CANCELFLG Then
                    M_SELECTFLG = False
                    M_CANCELFLG = False
                    MsgBox(M_COMMON.GetMessage("I0008"))
                End If
            End If
            M_SELECTFLG = False
            M_CANCELFLG = False
            ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0011")
        End If
    End Sub

    Private Sub ExecDrop()
        '#######################################################################
        'オブジェクトDROP
        '#######################################################################

        Dim schemaName As String
        Dim objectName As String
        Dim objectType As String
        Dim cnt As Long
        Dim resultCode As Integer

        resultCode = MsgBox(M_COMMON.GetMessage("W0003"), MsgBoxStyle.YesNo, "確認")
        If resultCode = vbYes Then
            If Me.TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L2 Then
                M_SELECTFLG = True
                schemaName = ControlChars.Quote & TV_1.SelectedNode.Parent.Text & ControlChars.Quote
                objectType = TV_1.SelectedNode.Text
                ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0027")
                For Each listViewItem As ListViewItem In LV_1.Items
                    If listViewItem.Selected Then
                        '件数データ取得
                        objectName = ControlChars.Quote & listViewItem.Text & ControlChars.Quote
                        Me.Refresh()
                        If frm_Login.G_DB.EXEC_NONQUERY("DROP " & objectType & " " & schemaName & "." & objectName, cnt) = False Then
                            ToolStripStatusLabel1.Text = M_COMMON.GetMessage("E0001")
                            Exit Sub
                        End If
                        listViewItem.Selected = False
                        Me.Refresh()

                        System.Windows.Forms.Application.DoEvents()
                        'キャンセル指示があった場合
                        If M_CANCELFLG Then
                            M_SELECTFLG = False
                            M_CANCELFLG = False
                            MsgBox(M_COMMON.GetMessage("I0008"))
                            Exit For
                        End If
                    End If
                Next
                M_SELECTFLG = False
                M_CANCELFLG = False
                ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0011")
            End If
        End If
    End Sub

    Private Sub LV_1_DoubleClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LV_1.DoubleClick
        LoadDataForm()
    End Sub

    Private Sub Explorer1_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        M_FILE.CreateControlInitFile(Me.Name, Me.Controls)

        '開いている画面を閉じる
        frm_Execsql.Close()

        'ロジックに入らなくともアセンブリがチェックされるようなのでコメントアウト。
        'Try
        '    If frm_Login.G_DB.GET_LOGIN_USER = "SYS" And My.Computer.FileSystem.FileExists(System.IO.Directory.GetCurrentDirectory() & "\FRM_INTERNAL.dll") Then
        '        If Not (frm_Login.G_INTERNAL Is Nothing) Then
        '            frm_Login.G_INTERNAL.Close()
        '        End If
        '    End If
        'Catch ex As Exception
        'End Try

        frm_Bind.Close()
        frm_Remote.Close()
        frm_Session.Close()
        frm_SGA.Close()
        frm_showdata.Close()
        frm_Storage.Close()
        frm_Stress.Close()
        frm_Watch.Close()
        frm_Report.Close()

        'DBコネクション切断
        frm_Login.G_DB.DB_CLOSE()

        If M_RECONNECT Then
            frm_Login.Visible = True
            frm_Login.ToolStripStatusLabel1.Text = ""
            Me.Dispose()
        Else
            frm_Login.Close()
            frm_Login.Dispose()
        End If
    End Sub

    Private Sub ListView_MouseUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles LV_1.MouseUp
        Dim point As Point = Me.PointToClient(Windows.Forms.Cursor.Position)
        '右クリック
        If e.Button = M_CONST.C_MOUSE_CLICK.[RIGHT] Then
            'コンテキストメニューの表示
            ContextMenuStrip1.Show(Me.Left + point.X, Me.Top + point.Y + 20)
        End If
    End Sub

    Private Sub EXCUTING_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TV_1.KeyDown, LV_1.KeyDown, TXT_JOKEN.KeyDown
        If M_SELECTFLG = True Then
            'SELECT LOOP中
            If e.KeyCode = Keys.Escape Then
                M_CANCELFLG = True
            Else
                '処理実行中はESCキー以外の入力を無視
                e.Handled = True
            End If
        Else
            '通常時
            If e.KeyCode = Keys.Enter Then
                'Enterの場合
                If TXT_JOKEN.Focused Then
                    Exit Sub
                End If
                If TV_1.SelectedNode Is Nothing Then
                    Exit Sub
                End If
                If TV_1.SelectedNode.Level = M_CONST.C_TV_LEVEL.L0 = False Then
                    LoadDataForm()
                End If
            ElseIf e.Control And e.KeyCode = Keys.A Then
                'CTRL+Aの場合
                For cnt As Integer = 0 To LV_1.Items.Count - 1
                    LV_1.Items(cnt).Selected = True
                Next
                Exit Sub
            ElseIf e.KeyCode = Keys.F5 Then
                'F5の場合
                DisplayListView()
            ElseIf e.Control And e.KeyCode = Keys.C Then
                'CTRL+Cの場合
                SetClipboard()
            End If
        End If
    End Sub

    Private Sub Explorer1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If frm_Login.G_DB.GET_LOGIN_USER = "SYS" And My.Computer.FileSystem.FileExists(System.IO.Directory.GetCurrentDirectory() & "\FRM_INTERNAL.dll") Then
            '内部用DLLがインストールされている場合
            BTN_INTERNAL.Visible = True
        End If

        LV_1.FullRowSelect = True
        LV_1.GridLines = True
        LV_1.Sorting = SortOrder.Ascending
        LV_1.View = View.Details
        DisplayTreeView()
        Me.Text = "O-Analyzer - エクスプローラ(" & frm_Login.G_DB.GET_LOGIN_USER & ":" & frm_Login.G_DB.GET_CONNECT_STRING & ":" & Process.GetCurrentProcess().Id() & ")"

        '画面初期設定読み込み
        M_FILE.SetControls(Me.Name, Me.Controls)
    End Sub

    Private Sub ContextMenuStrip1_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) _
    Handles ContextMenuStrip1.ItemClicked
        ContextMenuStrip1.Visible = False
        If e.ClickedItem.Name = "CNT" Then
            GetTableCnt()
        ElseIf e.ClickedItem.Name = "CP" Then
            SetClipboard()
        ElseIf e.ClickedItem.Name = "TRUNC" Then
            ExecTruncate()
        ElseIf e.ClickedItem.Name = "STATS" Then
            ExecGetStats()
        ElseIf e.ClickedItem.Name = "DROP" Then
            ExecDrop()
        End If
    End Sub

    Private Sub SetUpListViewColumns(ByVal typeNum As Short)
        Dim columns As String()
        Dim column As String
        Dim colType As Integer()
        Dim ComparerMode As ListViewItemComparer.ComparerMode()

        ' TODO: listview 列を設定するコードを追加します
        LV_1.Columns.Clear()

        columns = M_CONST.C_OBJECT(typeNum).GetCols()
        For Each column In columns
            LV_1.Columns.Add(column)
        Next

        colType = M_CONST.C_OBJECT(typeNum).GetSortType()
        ReDim ComparerMode(colType.Length - 1)
        For i As Integer = 0 To colType.Length - 1
            ComparerMode(i) = colType(i)
        Next
        '''''''''''''''''''''''ソーターの設定
        'M_CONST.C_OBJECT(typeNum).GetColType()
        listViewItemSorter = New ListViewItemComparer
        'listViewItemSorter.ColumnModes = New ListViewItemComparer.ComparerMode() {ComparerMode(0),}
        listViewItemSorter.ColumnModes = ComparerMode
        LV_1.ListViewItemSorter = listViewItemSorter
        '''''''''''''''''''''''ソーターの設定

    End Sub

    Private Sub SetContextMenu(ByVal typeNum As Short)
        Dim contextID As String()
        Dim ContextName As String()
        Dim tempContextID As String
        Dim cnt As Integer = 0

        ContextMenuStrip1.Items.Clear()
        contextID = M_CONST.C_OBJECT(typeNum).GetContextID()
        ContextName = M_CONST.C_OBJECT(typeNum).GetContextName()
        For Each tempContextID In contextID
            ContextMenuStrip1.Items.Add(ContextName(cnt))
            ContextMenuStrip1.Items(cnt).Name = tempContextID
            cnt = cnt + 1
        Next

    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        frm_Login.Visible = True
        frm_Login.G_DB = Nothing
        Global.System.Windows.Forms.Application.Exit()
    End Sub

    Private Sub TreeView_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TV_1.AfterSelect
        DisplayListView()
    End Sub

    Private Sub TXT_JOKEN_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TXT_JOKEN.KeyPress
        If M_SELECTFLG = True Then
        Else
            '通常時
            'SELECT LOOP中
            If e.KeyChar = ControlChars.Cr Then
                DisplayListView()
            End If

        End If
    End Sub

    Private Sub LV_1_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles LV_1.ColumnClick
        'LV_1.ListViewItemSorter = New ListViewItemComparer(e.Column)
        'LV_1.ListViewItemSorter = Nothing

        'Dim listViewItemSorter As ListViewItemComparer

        listViewItemSorter.Column = e.Column
        '並び替える
        LV_1.Sort()
        SetLVColor()
    End Sub

    Private Sub BTN_SQL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_SQL.Click
        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0017")
        Me.Refresh()
        frm_Execsql.Visible = True
        frm_Execsql.Focus()
        ToolStripStatusLabel1.Text = vbNullString
    End Sub

    Private Sub BTN_SESSION_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_SESSION.Click
        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0017")
        Me.Refresh()
        frm_Session.Visible = True
        frm_Session.Focus()
        ToolStripStatusLabel1.Text = vbNullString
    End Sub

    Private Sub BTN_SGA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_SGA.Click
        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0017")
        Me.Refresh()
        frm_SGA.Visible = True
        frm_SGA.Focus()
        ToolStripStatusLabel1.Text = vbNullString
    End Sub

    Private Sub BTN_SEGMENT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_SEGMENT.Click
        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0017")
        Me.Refresh()
        frm_Storage.Visible = True
        frm_Storage.Focus()
        ToolStripStatusLabel1.Text = vbNullString
    End Sub

    Private Sub BTN_WATCH_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_WATCH.Click
        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0017")
        Me.Refresh()
        frm_Watch.Visible = True
        frm_Watch.Focus()
        ToolStripStatusLabel1.Text = vbNullString
    End Sub

    Private Sub BTN_STRESS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_STRESS.Click
        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0017")
        Me.Refresh()
        frm_Stress.Visible = True
        frm_Stress.Focus()
        ToolStripStatusLabel1.Text = vbNullString
    End Sub

    Private Sub BTN_REMOTE_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_REMOTE.Click
        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0017")
        Me.Refresh()
        frm_Remote.Visible = True
        frm_Remote.Focus()
        ToolStripStatusLabel1.Text = vbNullString
    End Sub

    Private Sub BTN_SECRET_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_INTERNAL.Click
        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0017")
        Me.Refresh()

        If Not (frm_Login.G_INTERNAL Is Nothing) Then
            frm_Login.G_INTERNAL.Close()
            frm_Login.G_INTERNAL.Dispose()
        End If
        frm_Login.G_INTERNAL = New FRM_INTERNAL.FRM_INTERNAL
        frm_Login.G_INTERNAL.SetFrmLogin(frm_Login)
        frm_Login.G_INTERNAL.Show()
        frm_Login.G_INTERNAL.Focus()
        ToolStripStatusLabel1.Text = vbNullString
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("http://www.doppo1.net")
    End Sub

    Private Sub BTN_LOG_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_LOG.Click
        M_COMMON.ShowLog()
    End Sub

    Private Sub BTN_CONNECT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_CONNECT.Click
        M_RECONNECT = True
        Me.Close()
    End Sub

    Private Sub frm_Explorer_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        TableLayoutPanel1.Width = SplitContainer2.Width
        TableLayoutPanel1.Height = SplitContainer2.Panel2.Height
        FlowLayoutPanel2.Width = TableLayoutPanel1.Width
        FlowLayoutPanel2.Height = TableLayoutPanel1.Height
    End Sub

    Private Sub SplitContainer2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer2.Resize
        TableLayoutPanel1.Width = SplitContainer2.Width
        'TableLayoutPanel1.Height = SplitContainer2.Panel2.Height
        FlowLayoutPanel2.Width = TableLayoutPanel1.Width
        'FlowLayoutPanel2.Height = TableLayoutPanel1.Height
    End Sub

    Private Sub SplitContainer2_Panel2_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SplitContainer2.Panel2.Resize
        TableLayoutPanel1.Height = SplitContainer2.Panel2.Height
        FlowLayoutPanel2.Height = TableLayoutPanel1.Height
    End Sub

    Private Sub BTN_HELP_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_HELP.Click
        M_COMMON.ShowHelp(Me.Name)
    End Sub

    Private Sub BTN_REPORT_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BTN_REPORT.Click
        ToolStripStatusLabel1.Text = M_COMMON.GetMessage("I0017")
        Me.Refresh()
        frm_Report.Visible = True
        frm_Report.Focus()
        ToolStripStatusLabel1.Text = vbNullString
    End Sub
End Class

'Class ListViewItemComparer
'    Implements IComparer
'    '#######################################################################
'    'リストビューソート用クラス
'    '#######################################################################

'    Private col As Integer

'    Public Sub New()
'        col = 0
'    End Sub

'    Public Sub New(ByVal column As Integer)
'        col = column
'    End Sub

'    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
'        Return [String].Compare(CType(x, ListViewItem).SubItems(col).Text, CType(y, ListViewItem).SubItems(col).Text)
'    End Function
'End Class

Public Class ListViewItemComparer
    Implements IComparer
    '#######################################################################
    'リストビューソート用クラス
    '#######################################################################

    Public Enum ComparerMode
        [String] = M_CONST.C_SORT.STRING
        [Integer] = M_CONST.C_SORT.NUMBER
        DateTime = M_CONST.C_SORT.DATE
    End Enum

    Private _column As Integer
    Private _order As SortOrder
    Private _mode As ComparerMode
    Private _columnModes() As ComparerMode

    Public Property Column() As Integer
        Get
            Return _column
        End Get
        Set(ByVal Value As Integer)
            If _column = Value Then
                If _order = SortOrder.Ascending Then
                    _order = SortOrder.Descending
                Else
                    If _order = SortOrder.Descending Then
                        _order = SortOrder.Ascending
                    End If
                End If
            End If
            _column = Value
        End Set
    End Property

    Public Property Order() As SortOrder
        Get
            Return _order
        End Get
        Set(ByVal Value As SortOrder)
            _order = Value
        End Set
    End Property

    Public Property Mode() As ComparerMode
        Get
            Return _mode
        End Get
        Set(ByVal Value As ComparerMode)
            _mode = Value
        End Set
    End Property

    Public WriteOnly Property ColumnModes() As ComparerMode()
        Set(ByVal Value As ComparerMode())
            _columnModes = Value
        End Set
    End Property

    Public Sub New(ByVal col As Integer, ByVal ord As SortOrder, ByVal cmod As ComparerMode)
        _column = col
        _order = ord
        _mode = cmod
    End Sub

    Public Sub New()
        _column = 0
        _order = SortOrder.Ascending
        _mode = ComparerMode.String
    End Sub

    'xがyより小さいときはマイナスの数、大きいときはプラスの数、
    '同じときは0を返す
    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements IComparer.Compare
        Dim result As Integer = 0
        Dim XisNull As Boolean = False
        Dim YisNull As Boolean = False

        'ListViewItemの取得
        Dim itemx As ListViewItem = CType(x, ListViewItem)
        Dim itemy As ListViewItem = CType(y, ListViewItem)

        '並べ替えの方法を決定
        If Not (_columnModes Is Nothing) And _
                _columnModes.Length > _column Then
            _mode = _columnModes(_column)
        End If
        '並び替えの方法別に、xとyを比較する
        Select Case _mode
            Case ComparerMode.String
                result = String.Compare(itemx.SubItems(_column).Text, _
                    itemy.SubItems(_column).Text)
            Case ComparerMode.Integer
                'NULLの場合-1として扱う
                If itemx.SubItems(_column).Text = vbNullString Then
                    XisNull = True
                    itemx.SubItems(_column).Text = -1
                End If
                If itemy.SubItems(_column).Text = vbNullString Then
                    YisNull = True
                    itemy.SubItems(_column).Text = -1
                End If
                result = Integer.Parse(itemx.SubItems(_column).Text - _
                    Integer.Parse(itemy.SubItems(_column).Text))
                If XisNull Then
                    itemx.SubItems(_column).Text = vbNullString
                End If
                If YisNull Then
                    itemy.SubItems(_column).Text = vbNullString
                End If
            Case ComparerMode.DateTime
                result = DateTime.Compare( _
                    DateTime.Parse(itemx.SubItems(_column).Text), _
                    DateTime.Parse(itemy.SubItems(_column).Text))
        End Select

        '降順の時は結果を+-逆にする
        If _order = SortOrder.Descending Then
            result = -result
        Else
            If _order = SortOrder.None Then
                result = 0
            End If
        End If
        '結果を返す
        Return result
    End Function
End Class