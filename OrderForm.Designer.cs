namespace POS_qu
{
    partial class OrderForm
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox textBoxOrderNumber;
        private TextBox textBoxCustomer;
        private TextBox textBoxNoHp;

        private ComboBox comboBoxOrderType;          // jenis pesanan (status)
        private ComboBox comboBoxPaymentMethod;      // metode pembayaran
        private ComboBox comboBoxDeliveryMethod;     // metode pengantaran

        private DateTimePicker dateTimePickerOrderDate;
        private DateTimePicker dateTimePickerDeliveryTime; // waktu pengantaran (hanya jika pengantaran)

        private Button buttonSave;
        private Button buttonCancel;

        private Label labelOrderNumber, labelCustomer, labelNoHp, labelOrderType;
        private Label labelPaymentMethod, labelDeliveryMethod, labelOrderDate, labelDeliveryTime;

        private void InitializeComponent()
        {
            textBoxOrderNumber = new TextBox();
            textBoxCustomer = new TextBox();
            textBoxNoHp = new TextBox();
            comboBoxOrderType = new ComboBox();
            comboBoxPaymentMethod = new ComboBox();
            comboBoxDeliveryMethod = new ComboBox();
            dateTimePickerOrderDate = new DateTimePicker();
            dateTimePickerDeliveryTime = new DateTimePicker();
            buttonSave = new Button();
            buttonCancel = new Button();
            labelOrderNumber = new Label();
            labelCustomer = new Label();
            labelNoHp = new Label();
            labelOrderType = new Label();
            labelPaymentMethod = new Label();
            labelDeliveryMethod = new Label();
            labelOrderDate = new Label();
            labelDeliveryTime = new Label();
            labelKeterangan = new Label();
            richTextBoxKeterangan = new RichTextBox();
            SuspendLayout();
            // 
            // textBoxOrderNumber
            // 
            textBoxOrderNumber.Location = new Point(328, 32);
            textBoxOrderNumber.Name = "textBoxOrderNumber";
            textBoxOrderNumber.Size = new Size(200, 31);
            textBoxOrderNumber.TabIndex = 1;
            // 
            // textBoxCustomer
            // 
            textBoxCustomer.Location = new Point(328, 72);
            textBoxCustomer.Name = "textBoxCustomer";
            textBoxCustomer.Size = new Size(200, 31);
            textBoxCustomer.TabIndex = 2;
            // 
            // textBoxNoHp
            // 
            textBoxNoHp.Location = new Point(328, 112);
            textBoxNoHp.Name = "textBoxNoHp";
            textBoxNoHp.Size = new Size(200, 31);
            textBoxNoHp.TabIndex = 3;
            // 
            // comboBoxOrderType
            // 
            comboBoxOrderType.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxOrderType.Location = new Point(328, 152);
            comboBoxOrderType.Name = "comboBoxOrderType";
            comboBoxOrderType.Size = new Size(200, 33);
            comboBoxOrderType.TabIndex = 4;
            // 
            // comboBoxPaymentMethod
            // 
            comboBoxPaymentMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxPaymentMethod.Location = new Point(328, 192);
            comboBoxPaymentMethod.Name = "comboBoxPaymentMethod";
            comboBoxPaymentMethod.Size = new Size(200, 33);
            comboBoxPaymentMethod.TabIndex = 5;
            comboBoxPaymentMethod.Visible = false;
            // 
            // comboBoxDeliveryMethod
            // 
            comboBoxDeliveryMethod.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxDeliveryMethod.Location = new Point(328, 232);
            comboBoxDeliveryMethod.Name = "comboBoxDeliveryMethod";
            comboBoxDeliveryMethod.Size = new Size(200, 33);
            comboBoxDeliveryMethod.TabIndex = 6;
            comboBoxDeliveryMethod.SelectedIndexChanged += ComboBoxDeliveryMethod_SelectedIndexChanged;
            // 
            // dateTimePickerOrderDate
            // 
            dateTimePickerOrderDate.Location = new Point(328, 272);
            dateTimePickerOrderDate.Name = "dateTimePickerOrderDate";
            dateTimePickerOrderDate.Size = new Size(200, 31);
            dateTimePickerOrderDate.TabIndex = 7;
            // 
            // dateTimePickerDeliveryTime
            // 
            dateTimePickerDeliveryTime.CustomFormat = "dd MMM yyyy HH:mm";
            dateTimePickerDeliveryTime.Format = DateTimePickerFormat.Custom;
            dateTimePickerDeliveryTime.Location = new Point(328, 312);
            dateTimePickerDeliveryTime.Name = "dateTimePickerDeliveryTime";
            dateTimePickerDeliveryTime.Size = new Size(200, 31);
            dateTimePickerDeliveryTime.TabIndex = 8;
            dateTimePickerDeliveryTime.Visible = false;
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(320, 440);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(88, 48);
            buttonSave.TabIndex = 10;
            buttonSave.Text = "Save";
            buttonSave.Click += buttonSave_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(448, 440);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(88, 48);
            buttonCancel.TabIndex = 11;
            buttonCancel.Text = "Cancel";
            buttonCancel.Click += buttonCancel_Click;
            // 
            // labelOrderNumber
            // 
            labelOrderNumber.Location = new Point(64, 32);
            labelOrderNumber.Name = "labelOrderNumber";
            labelOrderNumber.Size = new Size(150, 32);
            labelOrderNumber.TabIndex = 0;
            labelOrderNumber.Text = "No Pesanan";
            // 
            // labelCustomer
            // 
            labelCustomer.Location = new Point(64, 72);
            labelCustomer.Name = "labelCustomer";
            labelCustomer.Size = new Size(150, 32);
            labelCustomer.TabIndex = 2;
            labelCustomer.Text = "Customer";
            // 
            // labelNoHp
            // 
            labelNoHp.Location = new Point(64, 112);
            labelNoHp.Name = "labelNoHp";
            labelNoHp.Size = new Size(150, 32);
            labelNoHp.TabIndex = 3;
            labelNoHp.Text = "No. Hp";
            // 
            // labelOrderType
            // 
            labelOrderType.Location = new Point(64, 152);
            labelOrderType.Name = "labelOrderType";
            labelOrderType.Size = new Size(150, 32);
            labelOrderType.TabIndex = 4;
            labelOrderType.Text = "Status Pesanan";
            // 
            // labelPaymentMethod
            // 
            labelPaymentMethod.Location = new Point(64, 192);
            labelPaymentMethod.Name = "labelPaymentMethod";
            labelPaymentMethod.Size = new Size(200, 32);
            labelPaymentMethod.TabIndex = 5;
            labelPaymentMethod.Text = "Metode Pembayaran";
            // 
            // labelDeliveryMethod
            // 
            labelDeliveryMethod.Location = new Point(64, 232);
            labelDeliveryMethod.Name = "labelDeliveryMethod";
            labelDeliveryMethod.Size = new Size(216, 32);
            labelDeliveryMethod.TabIndex = 6;
            labelDeliveryMethod.Text = "Metode Pengantaran";
            // 
            // labelOrderDate
            // 
            labelOrderDate.Location = new Point(64, 272);
            labelOrderDate.Name = "labelOrderDate";
            labelOrderDate.Size = new Size(150, 32);
            labelOrderDate.TabIndex = 7;
            labelOrderDate.Text = "Tanggal Pesanan";
            // 
            // labelDeliveryTime
            // 
            labelDeliveryTime.Location = new Point(64, 312);
            labelDeliveryTime.Name = "labelDeliveryTime";
            labelDeliveryTime.Size = new Size(176, 32);
            labelDeliveryTime.TabIndex = 8;
            labelDeliveryTime.Text = "Waktu Pengantaran";
            labelDeliveryTime.Visible = false;
            // 
            // labelKeterangan
            // 
            labelKeterangan.Location = new Point(64, 392);
            labelKeterangan.Name = "labelKeterangan";
            labelKeterangan.Size = new Size(150, 32);
            labelKeterangan.TabIndex = 9;
            labelKeterangan.Text = "Keterangan";
            labelKeterangan.Click += labelKeterangan_Click;
            // 
            // richTextBoxKeterangan
            // 
            richTextBoxKeterangan.Location = new Point(328, 360);
            richTextBoxKeterangan.Name = "richTextBoxKeterangan";
            richTextBoxKeterangan.Size = new Size(544, 64);
            richTextBoxKeterangan.TabIndex = 9;
            richTextBoxKeterangan.Text = "";
            // 
            // OrderForm
            // 
            ClientSize = new Size(1235, 581);
            Controls.Add(labelKeterangan);
            Controls.Add(richTextBoxKeterangan);
            Controls.Add(labelOrderNumber);
            Controls.Add(textBoxOrderNumber);
            Controls.Add(labelCustomer);
            Controls.Add(textBoxCustomer);
            Controls.Add(labelNoHp);
            Controls.Add(textBoxNoHp);
            Controls.Add(labelOrderType);
            Controls.Add(comboBoxOrderType);
            Controls.Add(labelDeliveryMethod);
            Controls.Add(comboBoxDeliveryMethod);
            Controls.Add(labelOrderDate);
            Controls.Add(dateTimePickerOrderDate);
            Controls.Add(labelDeliveryTime);
            Controls.Add(dateTimePickerDeliveryTime);
            Controls.Add(buttonSave);
            Controls.Add(buttonCancel);
            Name = "OrderForm";
            Text = "Order Form";
            Load += OrderForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }
        private Label labelKeterangan;
        private RichTextBox richTextBoxKeterangan;
    }
}
