﻿using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TRMDesktopUI.Helpers;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
		// varje gång vi byter username eller password så kommer set säga att vi har gjort de
		private string _userName;
		private string _password;
		private IAPIHelper _apiHelper;

        public LoginViewModel(IAPIHelper apiHelper)
		{
			_apiHelper = apiHelper;
		}
		public string UserName
		{
			get { return _userName; }
			set 
			{ 
				_userName = value;
				NotifyOfPropertyChange(() => UserName);
                NotifyOfPropertyChange(() => CanLogIn);
            }
		}

		public string Password
		{
			get { return _password; }
			set 
			{ 
				_password = value; 
				NotifyOfPropertyChange(() => Password);
				NotifyOfPropertyChange(() => CanLogIn);
			}
		}


		public bool IsErrorVisible
		{
            get
            {
				bool output = false;

				if (ErrorMessage?.Length > 0)
				{
					output = true;	
				}
				return output;
            }


		}

		private string _errorMessage;

		public string ErrorMessage
        {
			get { return _errorMessage; }
			set 
			{
                _errorMessage = value;
                NotifyOfPropertyChange(() => IsErrorVisible);
                NotifyOfPropertyChange(() => ErrorMessage);
			}
		}



		public bool CanLogIn
		{
            // här gjorde vi en get nu innan va det CanLogIn(UserName, Password)
			// ? är för null sak (null check)

            get
            {
                bool output = false;
                if (UserName?.Length > 0 && Password?.Length > 0)
                {
                    output = true;
                }

                return output;
            }

		}

		public async Task LogIn()
		{
			try
			{
				ErrorMessage = "";
				var result = await _apiHelper.Authenticate(UserName, Password);
			}
			catch (Exception ex)
			{
				ErrorMessage = ex.Message;
			}
		}



	}
}