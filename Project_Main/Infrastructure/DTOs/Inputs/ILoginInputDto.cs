﻿namespace Project_Main.Infrastructure.DTOs.Inputs
{
    public interface ILoginInputDto
    {
        string Password { get; set; }
        string Username { get; set; }
    }
}