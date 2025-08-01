﻿using PetProject.Shared.Entities;

namespace PetProject.Shared.ViewModels
{
    public class ProfileViewModel
    {
        public UserEntity User { get; set; } = null;
        public bool IsOwnProfile { get; set; }
        public bool IsFriend { get; set; }

        public string? CurrentUserNick {  get; set; }
    }
}
