﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

namespace ForestHome.Models
{
    public static class PasswordEncrypt
    {
        public static string TextToEncrypt(string passWord)
        {
            return Convert.ToBase64String(System.Security.Cryptography.SHA256.Create().ComputeHash(Encoding.UTF8.GetBytes(passWord)));
        }
    }
}