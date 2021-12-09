<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frm_Explorer
    Inherits System.Windows.Forms.Form

    'フォームがコンポーネントの一覧をクリーンアップするために dispose をオーバーライドします。
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Friend WithEvents ToolStripContainer As System.Windows.Forms.ToolStripContainer
    Friend WithEvents TreeNodeImageList As System.Windows.Forms.ImageList
    Friend WithEvents SplitContainer As System.Windows.Forms.SplitContainer
    Friend WithEvents TV_1 As System.Windows.Forms.TreeView
    Friend WithEvents LV_1 As System.Windows.Forms.ListView
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents ToolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel

    'Windows フォーム デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    'メモ: 以下のプロシージャは Windows フォーム デザイナで必要です。
    'Windows フォーム デザイナを使用して変更できます。  
    'コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frm_Explorer))
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.ToolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.TreeNodeImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.ToolStripContainer = New System.Windows.Forms.ToolStripContainer()
        Me.SplitContainer = New System.Windows.Forms.SplitContainer()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.TV_1 = New System.Windows.Forms.TreeView()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.BTN_CONNECT = New System.Windows.Forms.Button()
        Me.BTN_SQL = New System.Windows.Forms.Button()
        Me.BTN_SESSION = New System.Windows.Forms.Button()
        Me.BTN_SGA = New System.Windows.Forms.Button()
        Me.BTN_SEGMENT = New System.Windows.Forms.Button()
        Me.BTN_WATCH = New System.Windows.Forms.Button()
        Me.BTN_STRESS = New System.Windows.Forms.Button()
        Me.BTN_REMOTE = New System.Windows.Forms.Button()
        Me.BTN_REPORT = New System.Windows.Forms.Button()
        Me.BTN_HELP = New System.Windows.Forms.Button()
        Me.BTN_LOG = New System.Windows.Forms.Button()
        Me.BTN_INTERNAL = New System.Windows.Forms.Button()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.TXT_JOKEN = New System.Windows.Forms.TextBox()
        Me.LV_1 = New System.Windows.Forms.ListView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.StatusStrip.SuspendLayout()
        Me.ToolStripContainer.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer.ContentPanel.SuspendLayout()
        Me.ToolStripContainer.SuspendLayout()
        Me.SplitContainer.Panel1.SuspendLayout()
        Me.SplitContainer.Panel2.SuspendLayout()
        Me.SplitContainer.SuspendLayout()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel2.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'StatusStrip
        '
        Me.StatusStrip.Dock = System.Windows.Forms.DockStyle.None
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripStatusLabel, Me.ToolStripStatusLabel1})
        Me.StatusStrip.Location = New System.Drawing.Point(0, 0)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(784, 23)
        Me.StatusStrip.TabIndex = 6
        Me.StatusStrip.Text = "StatusStrip"
        '
        'ToolStripStatusLabel
        '
        Me.ToolStripStatusLabel.Name = "ToolStripStatusLabel"
        Me.ToolStripStatusLabel.Size = New System.Drawing.Size(44, 18)
        Me.ToolStripStatusLabel.Text = "状態："
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(0, 18)
        '
        'TreeNodeImageList
        '
        Me.TreeNodeImageList.ImageStream = CType(resources.GetObject("TreeNodeImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.TreeNodeImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.TreeNodeImageList.Images.SetKeyName(0, "ClosedFolder")
        Me.TreeNodeImageList.Images.SetKeyName(1, "OpenFolder")
        Me.TreeNodeImageList.Images.SetKeyName(2, "table.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(3, "view.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(4, "index.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(5, "TABLE_PARTITION.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(6, "TABLE_SUBPARTITION.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(7, "INDEX_PARTITION.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(8, "INDEX_SUBPARTITIONー.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(9, "MATERIALIZED_VIEW.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(10, "LOB.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(11, "SEQUENCE.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(12, "PROCEDURE.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(13, "FUNCTION.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(14, "PACKAGE_S.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(15, "PACKAGE_B.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(16, "JAVA_C.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(17, "JAVA_S.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(18, "TRIGGER.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(19, "SYNONYM.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(20, "DATABASE_LINK.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(21, "TYPE.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(22, "LIBRARY.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(23, "CLUSTER.jpg")
        Me.TreeNodeImageList.Images.SetKeyName(24, "EXE.BMP")
        Me.TreeNodeImageList.Images.SetKeyName(25, "EXE.BMP")
        '
        'ToolStripContainer
        '
        '
        'ToolStripContainer.BottomToolStripPanel
        '
        Me.ToolStripContainer.BottomToolStripPanel.Controls.Add(Me.StatusStrip)
        '
        'ToolStripContainer.ContentPanel
        '
        Me.ToolStripContainer.ContentPanel.Controls.Add(Me.SplitContainer)
        Me.ToolStripContainer.ContentPanel.Size = New System.Drawing.Size(784, 514)
        Me.ToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer.Name = "ToolStripContainer"
        Me.ToolStripContainer.Size = New System.Drawing.Size(784, 562)
        Me.ToolStripContainer.TabIndex = 0
        Me.ToolStripContainer.Text = "ToolStripContainer1"
        '
        'SplitContainer
        '
        Me.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer.Name = "SplitContainer"
        '
        'SplitContainer.Panel1
        '
        Me.SplitContainer.Panel1.Controls.Add(Me.SplitContainer2)
        '
        'SplitContainer.Panel2
        '
        Me.SplitContainer.Panel2.Controls.Add(Me.SplitContainer1)
        Me.SplitContainer.Size = New System.Drawing.Size(784, 514)
        Me.SplitContainer.SplitterDistance = 204
        Me.SplitContainer.TabIndex = 0
        Me.SplitContainer.TabStop = False
        Me.SplitContainer.Text = "SplitContainer1"
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.Controls.Add(Me.TV_1)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.Controls.Add(Me.TableLayoutPanel1)
        Me.SplitContainer2.Size = New System.Drawing.Size(204, 514)
        Me.SplitContainer2.SplitterDistance = 270
        Me.SplitContainer2.TabIndex = 0
        Me.SplitContainer2.TabStop = False
        '
        'TV_1
        '
        Me.TV_1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TV_1.ImageIndex = 0
        Me.TV_1.ImageList = Me.TreeNodeImageList
        Me.TV_1.Location = New System.Drawing.Point(0, 0)
        Me.TV_1.Name = "TV_1"
        Me.TV_1.SelectedImageKey = "OpenFolder"
        Me.TV_1.ShowLines = False
        Me.TV_1.Size = New System.Drawing.Size(204, 270)
        Me.TV_1.TabIndex = 0
        Me.TV_1.TabStop = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.LinkLabel1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel2, 0, 1)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 3)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(209, 234)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.Font = New System.Drawing.Font("MS UI Gothic", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(128, Byte))
        Me.LinkLabel1.Location = New System.Drawing.Point(15, 3)
        Me.LinkLabel1.Margin = New System.Windows.Forms.Padding(15, 3, 3, 3)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(156, 13)
        Me.LinkLabel1.TabIndex = 0
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "http://www.doppo1.net"
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Controls.Add(Me.BTN_CONNECT)
        Me.FlowLayoutPanel2.Controls.Add(Me.BTN_SQL)
        Me.FlowLayoutPanel2.Controls.Add(Me.BTN_SESSION)
        Me.FlowLayoutPanel2.Controls.Add(Me.BTN_SGA)
        Me.FlowLayoutPanel2.Controls.Add(Me.BTN_SEGMENT)
        Me.FlowLayoutPanel2.Controls.Add(Me.BTN_WATCH)
        Me.FlowLayoutPanel2.Controls.Add(Me.BTN_STRESS)
        Me.FlowLayoutPanel2.Controls.Add(Me.BTN_REMOTE)
        Me.FlowLayoutPanel2.Controls.Add(Me.BTN_REPORT)
        Me.FlowLayoutPanel2.Controls.Add(Me.BTN_HELP)
        Me.FlowLayoutPanel2.Controls.Add(Me.BTN_LOG)
        Me.FlowLayoutPanel2.Controls.Add(Me.BTN_INTERNAL)
        Me.FlowLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(3, 23)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(203, 208)
        Me.FlowLayoutPanel2.TabIndex = 20
        '
        'BTN_CONNECT
        '
        Me.BTN_CONNECT.Location = New System.Drawing.Point(3, 3)
        Me.BTN_CONNECT.Name = "BTN_CONNECT"
        Me.BTN_CONNECT.Size = New System.Drawing.Size(90, 23)
        Me.BTN_CONNECT.TabIndex = 0
        Me.BTN_CONNECT.Text = "再接続(&R)"
        Me.BTN_CONNECT.UseVisualStyleBackColor = True
        '
        'BTN_SQL
        '
        Me.BTN_SQL.Location = New System.Drawing.Point(99, 3)
        Me.BTN_SQL.Name = "BTN_SQL"
        Me.BTN_SQL.Size = New System.Drawing.Size(90, 23)
        Me.BTN_SQL.TabIndex = 1
        Me.BTN_SQL.Text = "SQL実行(&S)"
        Me.BTN_SQL.UseVisualStyleBackColor = True
        '
        'BTN_SESSION
        '
        Me.BTN_SESSION.Location = New System.Drawing.Point(3, 32)
        Me.BTN_SESSION.Name = "BTN_SESSION"
        Me.BTN_SESSION.Size = New System.Drawing.Size(90, 23)
        Me.BTN_SESSION.TabIndex = 2
        Me.BTN_SESSION.Text = "セッション(&1)"
        Me.BTN_SESSION.UseVisualStyleBackColor = True
        '
        'BTN_SGA
        '
        Me.BTN_SGA.Location = New System.Drawing.Point(99, 32)
        Me.BTN_SGA.Name = "BTN_SGA"
        Me.BTN_SGA.Size = New System.Drawing.Size(90, 23)
        Me.BTN_SGA.TabIndex = 3
        Me.BTN_SGA.Text = "SGA情報(&2)"
        Me.BTN_SGA.UseVisualStyleBackColor = True
        '
        'BTN_SEGMENT
        '
        Me.BTN_SEGMENT.Location = New System.Drawing.Point(3, 61)
        Me.BTN_SEGMENT.Name = "BTN_SEGMENT"
        Me.BTN_SEGMENT.Size = New System.Drawing.Size(90, 23)
        Me.BTN_SEGMENT.TabIndex = 4
        Me.BTN_SEGMENT.Text = "記憶域情報(&3)"
        Me.BTN_SEGMENT.UseVisualStyleBackColor = True
        '
        'BTN_WATCH
        '
        Me.BTN_WATCH.Location = New System.Drawing.Point(99, 61)
        Me.BTN_WATCH.Name = "BTN_WATCH"
        Me.BTN_WATCH.Size = New System.Drawing.Size(90, 23)
        Me.BTN_WATCH.TabIndex = 5
        Me.BTN_WATCH.Text = "DB監視(&4)"
        Me.BTN_WATCH.UseVisualStyleBackColor = True
        '
        'BTN_STRESS
        '
        Me.BTN_STRESS.Location = New System.Drawing.Point(3, 90)
        Me.BTN_STRESS.Name = "BTN_STRESS"
        Me.BTN_STRESS.Size = New System.Drawing.Size(90, 23)
        Me.BTN_STRESS.TabIndex = 6
        Me.BTN_STRESS.Text = "負荷テスト(&5)"
        Me.BTN_STRESS.UseVisualStyleBackColor = True
        '
        'BTN_REMOTE
        '
        Me.BTN_REMOTE.Location = New System.Drawing.Point(99, 90)
        Me.BTN_REMOTE.Name = "BTN_REMOTE"
        Me.BTN_REMOTE.Size = New System.Drawing.Size(90, 23)
        Me.BTN_REMOTE.TabIndex = 7
        Me.BTN_REMOTE.Text = "リモート操作(&6)"
        Me.BTN_REMOTE.UseVisualStyleBackColor = True
        '
        'BTN_REPORT
        '
        Me.BTN_REPORT.Location = New System.Drawing.Point(3, 119)
        Me.BTN_REPORT.Name = "BTN_REPORT"
        Me.BTN_REPORT.Size = New System.Drawing.Size(90, 25)
        Me.BTN_REPORT.TabIndex = 8
        Me.BTN_REPORT.Text = "レポート解析"
        Me.BTN_REPORT.UseVisualStyleBackColor = True
        '
        'BTN_HELP
        '
        Me.BTN_HELP.Location = New System.Drawing.Point(99, 119)
        Me.BTN_HELP.Name = "BTN_HELP"
        Me.BTN_HELP.Size = New System.Drawing.Size(90, 23)
        Me.BTN_HELP.TabIndex = 9
        Me.BTN_HELP.Text = "ヘルプ(&H)"
        Me.BTN_HELP.UseVisualStyleBackColor = True
        '
        'BTN_LOG
        '
        Me.BTN_LOG.Location = New System.Drawing.Point(3, 150)
        Me.BTN_LOG.Name = "BTN_LOG"
        Me.BTN_LOG.Size = New System.Drawing.Size(90, 23)
        Me.BTN_LOG.TabIndex = 10
        Me.BTN_LOG.Text = "ログ(&L)"
        Me.BTN_LOG.UseVisualStyleBackColor = True
        '
        'BTN_INTERNAL
        '
        Me.BTN_INTERNAL.Location = New System.Drawing.Point(99, 150)
        Me.BTN_INTERNAL.Name = "BTN_INTERNAL"
        Me.BTN_INTERNAL.Size = New System.Drawing.Size(90, 23)
        Me.BTN_INTERNAL.TabIndex = 11
        Me.BTN_INTERNAL.Text = "内部機能"
        Me.BTN_INTERNAL.UseVisualStyleBackColor = True
        Me.BTN_INTERNAL.Visible = False
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Label1)
        Me.SplitContainer1.Panel1.Controls.Add(Me.TXT_JOKEN)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Controls.Add(Me.LV_1)
        Me.SplitContainer1.Size = New System.Drawing.Size(576, 514)
        Me.SplitContainer1.SplitterDistance = 25
        Me.SplitContainer1.TabIndex = 1
        Me.SplitContainer1.TabStop = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 6)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(53, 12)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "検索条件"
        '
        'TXT_JOKEN
        '
        Me.TXT_JOKEN.Location = New System.Drawing.Point(64, 3)
        Me.TXT_JOKEN.Name = "TXT_JOKEN"
        Me.TXT_JOKEN.Size = New System.Drawing.Size(500, 19)
        Me.TXT_JOKEN.TabIndex = 1
        '
        'LV_1
        '
        Me.LV_1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.LV_1.Location = New System.Drawing.Point(0, 0)
        Me.LV_1.Name = "LV_1"
        Me.LV_1.Size = New System.Drawing.Size(576, 485)
        Me.LV_1.SmallImageList = Me.TreeNodeImageList
        Me.LV_1.TabIndex = 0
        Me.LV_1.UseCompatibleStateImageBehavior = False
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'frm_Explorer
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 562)
        Me.Controls.Add(Me.ToolStripContainer)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(800, 600)
        Me.Name = "frm_Explorer"
        Me.Text = "Explorer1"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ToolStripContainer.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer.ResumeLayout(False)
        Me.ToolStripContainer.PerformLayout()
        Me.SplitContainer.Panel1.ResumeLayout(False)
        Me.SplitContainer.Panel2.ResumeLayout(False)
        Me.SplitContainer.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        Me.SplitContainer2.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel2.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel1.PerformLayout()
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ToolStripStatusLabel1 As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents TXT_JOKEN As System.Windows.Forms.TextBox
    Friend WithEvents ContextMenuStrip1 As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel
    Friend WithEvents FlowLayoutPanel2 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents BTN_CONNECT As System.Windows.Forms.Button
    Friend WithEvents BTN_SQL As System.Windows.Forms.Button
    Friend WithEvents BTN_SESSION As System.Windows.Forms.Button
    Friend WithEvents BTN_SGA As System.Windows.Forms.Button
    Friend WithEvents BTN_SEGMENT As System.Windows.Forms.Button
    Friend WithEvents BTN_WATCH As System.Windows.Forms.Button
    Friend WithEvents BTN_STRESS As System.Windows.Forms.Button
    Friend WithEvents BTN_REMOTE As System.Windows.Forms.Button
    Friend WithEvents BTN_INTERNAL As System.Windows.Forms.Button
    Friend WithEvents BTN_LOG As System.Windows.Forms.Button
    Friend WithEvents BTN_HELP As System.Windows.Forms.Button
    Friend WithEvents BTN_REPORT As System.Windows.Forms.Button

End Class
