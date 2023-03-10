using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TRMDesktopUI.ViewModels
{
    public class LoginViewModel : Screen
    {
		// varje gång vi byter username eller password så kommer set säga att vi har gjort de
		private string _userName;
		private string _password;

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

		public void LogIn()
		{

		}



	}
}
