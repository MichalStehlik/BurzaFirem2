﻿namespace BurzaFirem2.InputModels
{
    public class EditUserIM
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
    }
}
