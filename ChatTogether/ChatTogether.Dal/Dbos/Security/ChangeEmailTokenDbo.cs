﻿using ChatTogether.Commons.GenericRepository;

namespace ChatTogether.Dal.Dbos.Security
{
    public class ChangeEmailTokenDbo : DboModel
    {
        public int AccountId { get; set; }
        public AccountDbo Account { get; set; }
        public string Token { get; set; }
    }
}
