﻿using System;
using System.Windows;
using System.Windows.Input;
using Ap.Email.Models;
using Ap.Email.Mvvm;
using Ap.EmailSender;

namespace Ap.Email.ViewModels
{
    public class MainWindowModel : BindableBase
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly EmailModel _email;
        private readonly ICommand _sendCommand;

        public MainWindowModel()
        {
            _sendCommand = new DelegateCommand<object>(Send);
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

        public ICommand SendCommand
        {
            get { return _sendCommand; }
        }

        private void Send(object args)
        {
            try
            {
                new SmtpService(_smtpSettings).Send(new[] { _email.To }, _email.Subject, _email.Body, _email.Attachmets);
                MessageBox.Show("ok");
            }
            catch (Exception e)
            {
                MessageBox.Show(e.GetBaseException().Message);
            }
            
        }
    }
}