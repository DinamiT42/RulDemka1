using Rul.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Rul.Pages
{
    /// <summary>
    /// Логика взаимодействия для Autho.xaml
    /// </summary>
    public partial class Autho : Page
    {
        private int countUnsuccessful = 0; // Количество неверных попыток
        public Autho()
        {
            InitializeComponent();
            txtCaptcha.Visibility = Visibility.Hidden;
            textBlockCaptcha.Visibility = Visibility.Hidden;
        }

        private void btnEnterGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null)); 
        }

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Text.Trim();

            User user = new User();

            user = RullEntities.GetContext().User.Where(p => p.UserLogin == login && p.UserPassword == password).FirstOrDefault();
            int userCount = RullEntities.GetContext().User.Where(p => p.UserLogin == login && p.UserPassword == password).Count();

            if (countUnsuccessful < 1)
            {
                if (userCount > 0)
                {
                    MessageBox.Show("Вы вошли под: " + user.Role.RoleName.ToString());
                    LoadForm(user.Role.RoleName.ToString(), user);
                }
                else
                {
                    MessageBox.Show("Вы ввели неврно логин или пароль!");
                    countUnsuccessful++;
                    if (countUnsuccessful == 1) // Если неверных попыток равно 1
                        GenerateCaptcha(); // Генерируем капчу
                }
            }
            else
            {
                if (userCount > 0 && textBlockCaptcha.Text == txtCaptcha.Text)
                {
                    MessageBox.Show("Вы вошли под: " + user.Role.RoleName.ToString());
                    LoadForm(user.Role.RoleName.ToString(), user);
                }
                else
                {
                    MessageBox.Show("Введите данные заново!");
                }
            }
        }

        private void GenerateCaptcha()
        {
            txtCaptcha.Visibility = Visibility.Visible;
            textBlockCaptcha.Visibility = Visibility.Visible;
            Random random = new Random();
            int randmNum = random.Next(0, 3);
            
            switch(randmNum)
            {
                case 1:
                    textBlockCaptcha.Text = "ju2sT8Cbs";
                    textBlockCaptcha.TextDecorations = TextDecorations.Strikethrough;
                        break;
                case 2:
                    textBlockCaptcha.Text = "iNwK2cl";
                    textBlockCaptcha.TextDecorations = TextDecorations.Strikethrough;
                    break;
                case 3:
                    textBlockCaptcha.Text = "u0ozGk95";
                    textBlockCaptcha.TextDecorations = TextDecorations.Strikethrough;
                    break;
            }
        }

        private void LoadForm(string _role, User user)
        {
            switch (_role)
            {
                case "Клиент":
                    NavigationService.Navigate(new Client(user)); // Если роль пользователя
                    break;
                case "Менеджер":
                    NavigationService.Navigate(new Client(user)); 
                    break;
                case "Администратор":
                    NavigationService.Navigate(new Admin(user));
                    break;
            }
        }

       
    }
}
