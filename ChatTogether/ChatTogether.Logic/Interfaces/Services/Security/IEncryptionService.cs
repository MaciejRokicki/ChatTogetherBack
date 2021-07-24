﻿namespace ChatTogether.Logic.Interfaces.Services.Security
{
    public interface IEncryptionService
    {
        string EncryptionSHA256(string value);
        bool VerifySHA256(string valueString, string hash);
    }
}