﻿using System.Threading.Tasks;
using Cookbook.Models.Login;
using Cookbook.Models.Register;
using Models.Models.Login;
using ClientModel = Cookbook.Models.Database.Client.Client;

namespace Cookbook.Database.Services.Interfaces;

public interface IClientService
{
    public Task<LoginResult> Login(ClientModel client);
    public Task<LoginResult> Login(string login, string password);
    public Task<RegisterResult> Register(ClientModel client);
    // public void DeleteClient(int id);
}