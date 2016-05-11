using System;
using System.Windows;
using System.Windows.Input;
using EmailApp.Models;
using EmailSender;
using Mvvm;
using Mvvm.Commands;

namespace EmailApp.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private readonly EmailModel _email;
        private readonly SmtpSettings _smtpSettings;

        public MainViewModel()
        {
            SendCommand = new DelegateCommand(Send);
            _smtpSettings = new SmtpSettings
            {
                From = "",
                Host = "",
                EnableSsl = false,
                Port = 25,
                User = "",
                Password = ""
            };
            _email = new EmailModel
            {
                To = "",
                Subject = "Test subject",
                Body = "Test body"
            };
        }

        public string Host
        {
            get { return _smtpSettings.Host; }
            set
            {
                if (_smtpSettings.Host != value)
                {
                    _smtpSettings.Host = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Port
        {
            get { return _smtpSettings.Port; }
            set
            {
                if (_smtpSettings.Port != value)
                {
                    _smtpSettings.Port = value;
                    OnPropertyChanged();
                }
            }
        }

        public string User
        {
            get { return _smtpSettings.User; }
            set
            {
                if (_smtpSettings.User != value)
                {
                    _smtpSettings.User = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get { return _smtpSettings.Password; }
            set
            {
                if (_smtpSettings.Password != value)
                {
                    _smtpSettings.Password = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool EnableSsl
        {
            get { return _smtpSettings.EnableSsl; }
            set
            {
                if (_smtpSettings.EnableSsl != value)
                {
                    _smtpSettings.EnableSsl = value;
                    OnPropertyChanged();
                }
            }
        }

        public string From
        {
            get { return _smtpSettings.From; }
            set
            {
                if (_smtpSettings.From != value)
                {
                    _smtpSettings.From = value;
                    OnPropertyChanged();
                }
            }
        }

        public string To
        {
            get { return _email.To; }
            set
            {
                if (_email.To != value)
                {
                    _email.To = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Subject
        {
            get { return _email.Subject; }
            set
            {
                if (_email.Subject != value)
                {
                    _email.Subject = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Body
        {
            get { return _email.Body; }
            set
            {
                if (_email.Body != value)
                {
                    _email.Body = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SendCommand { get; }

        private void Send()
        {
            try
            {
                new SmtpService(_smtpSettings).Send(new[] {_email.To}, _email.Subject, _email.Body);
                MessageBox.Show("ok");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.GetBaseException().Message);
            }
        }
    }
}