using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows;
using WpfApp1;

namespace Signature_RSA
{
    public partial class MainWindow : Window
    {
        Utils utils = new Utils();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnSinhKhoa_Click(object sender, RoutedEventArgs e)
        {
            if (rdTuyChon.IsChecked == true)
            {
                if (ischecked() == true)
                {
                    try
                    {
                        BigInteger p = BigInteger.Parse(pTextBox.Text);
                        BigInteger q = BigInteger.Parse(qTextBox.Text);

                        if (!utils.isChecked(p, q))
                        {
                            MessageBox.Show("Số nguyên tố không hợp lệ hoặc p và q bằng nhau.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            utils.sinhKhoaNhap(p, q);
                            nTextBox.Text = utils.N.ToString();
                            phinTextBox.Text = utils.Phi.ToString();
                            eTextBox.Text = utils.E.ToString();
                            dTextBox.Text = utils.D.ToString();
                            MessageBox.Show("Khóa đã được tạo thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    catch (FormatException)
                    {
                        MessageBox.Show("Dữ liệu nhập vào không hợp lệ", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else if (rdTuDong.IsChecked == true)
            {
                utils.sinhKhoa();
                pTextBox.Text = utils.P.ToString();
                qTextBox.Text = utils.Q.ToString();
                nTextBox.Text = utils.N.ToString();
                phinTextBox.Text = utils.Phi.ToString();
                eTextBox.Text = utils.E.ToString();
                dTextBox.Text = utils.D.ToString();
                MessageBox.Show("Khóa đã được tạo thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn nhập khóa hoặc tạo khóa tự động!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private bool ischecked()
        {
            if (pTextBox.Text == "" || qTextBox.Text == "")
            {
                MessageBox.Show("Vui lòng nhập đủ thông tin", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private void btnLayKhoaMoi_Click(object sender, RoutedEventArgs e)
        {
            utils.sinhKhoa();
            pTextBox.Text = utils.P.ToString();
            qTextBox.Text = utils.Q.ToString();
            nTextBox.Text = utils.N.ToString();
            phinTextBox.Text = utils.Phi.ToString();
            eTextBox.Text = utils.E.ToString();
            dTextBox.Text = utils.D.ToString();
            MessageBox.Show("Khóa mới đã được tạo thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnmahoa_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string originalMessage = txtbanro.Text;
                BigInteger messageBigInt = new BigInteger(Encoding.UTF8.GetBytes(originalMessage));
                BigInteger encryptedMessage = BigInteger.ModPow(messageBigInt, utils.E, utils.N);
                txtbanma.Text = encryptedMessage.ToString();
                MessageBox.Show("Mã hóa thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Mã hóa thất bại: {ex.Message}");
            }
        }

        private void btngiaima_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BigInteger encryptedMessage = BigInteger.Parse(tbbanma.Text);
                BigInteger decryptedMessage = BigInteger.ModPow(encryptedMessage, utils.D, utils.N);
                string decryptedText = Encoding.UTF8.GetString(decryptedMessage.ToByteArray());

                // Kiểm tra nếu giải mã không thành công
                if (string.IsNullOrWhiteSpace(decryptedText) || decryptedText.Any(c => char.IsControl(c) && c != '\r' && c != '\n'))
                {
                    throw new Exception("Văn bản không hợp lệ.");
                }
                tbgiaima.Text = Encoding.UTF8.GetString(decryptedMessage.ToByteArray());
                MessageBox.Show("Giải mã thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Giải mã thất bại: {ex.Message}");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    string fileContent = File.ReadAllText(filePath);
                    txtbanro.Text = fileContent;
                    MessageBox.Show("Nhập file thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể đọc file: {ex.Message}");
                }
            }
        }

        private void btnluufileabc_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    File.WriteAllText(filePath, txtbanma.Text);
                    MessageBox.Show("Lưu file thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể lưu file: {ex.Message}");
                }
            }
        }

        private void chonFileMaHoa_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    string fileContent = File.ReadAllText(filePath);
                    tbbanma.Text = fileContent;
                    MessageBox.Show("Nhập file thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể đọc file: {ex.Message}");
                }
            }
        }

        private void luuFileBanRo_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    File.WriteAllText(filePath, tbgiaima.Text);
                    MessageBox.Show("Lưu file thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Không thể lưu file: {ex.Message}");
                }
            }
        }

        private void btnRefresh1_Click(object sender, RoutedEventArgs e)
        {
            pTextBox.Text = string.Empty;
            qTextBox.Text = string.Empty;
            nTextBox.Text = string.Empty;
            phinTextBox.Text = string.Empty;
            eTextBox.Text = string.Empty;
            dTextBox.Text = string.Empty;
        }

        private void btnRefresh2_Click(object sender, RoutedEventArgs e)
        {
            txtbanro.Text = string.Empty;
            txtbanma.Text = string.Empty;
        }

        private void btnRefresh3_Click(object sender, RoutedEventArgs e)
        {
            tbbanma.Text = string.Empty;
            tbgiaima.Text = string.Empty;
        }

        private void btnChuyen_Click(object sender, RoutedEventArgs e)
        {
            tbbanma.Text = txtbanma.Text;
            try
            {
                BigInteger encryptedMessage = BigInteger.Parse(tbbanma.Text);
                BigInteger decryptedMessage = BigInteger.ModPow(encryptedMessage, utils.D, utils.N);
                string decryptedText = Encoding.UTF8.GetString(decryptedMessage.ToByteArray());

                // Kiểm tra nếu giải mã không thành công
                if (string.IsNullOrWhiteSpace(decryptedText) || decryptedText.Any(c => char.IsControl(c) && c != '\r' && c != '\n'))
                {
                    throw new Exception("Văn bản mã hóa đã bị thay đổi.");
                }
                tbgiaima.Text = Encoding.UTF8.GetString(decryptedMessage.ToByteArray());
                MessageBox.Show("Giải mã thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Giải mã thất bại: {ex.Message}");
            }
        }
    }
}
