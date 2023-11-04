using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

namespace Sat.Recruitment.Api.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<UserRepository> _logger;
        public UserRepository(ILogger<UserRepository> logger) 
        {
            _logger= logger;
        }

        public List<User> GetUsersFromDatabase()
        {
            var users = new List<User>();

            try
            {
                using (var streamReader = ReadUsersFromFile())
                {
                    while (!streamReader.EndOfStream)
                    {
                        var line = streamReader.ReadLine();
                        var user = new User()
                        {
                            Name = line.Split(',')[0].ToString().Trim(),
                            Email = line.Split(",")[1].ToString().Trim(),
                            Phone = line.Split(",")[2].ToString().Trim(),
                            Address = line.Split(",")[3].ToString().Trim(),
                            UserType = (UserType)Enum.Parse(typeof(UserType), line.Split(',')[4].ToString().Trim()),
                            Money = decimal.Parse(line.Split(',')[5].Trim())
                        };
                        users.Add(user);
                    }
                    streamReader.Close();                    
                }
            }
            catch(IOException ex)
            {
                _logger.LogError("Error reading the Database: " + ex.Message);
            }
            catch(UnauthorizedAccessException ex)
            {
                _logger.LogError("Anauthorized access", ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError("Unexpected exception", ex.Message);
            }

            return users;

        }

        public bool UserExists(User newUser, List<User> existingUsers)
        {
            try
            {
                foreach (var user in existingUsers)
                {
                    if (user.Email == newUser.Email || user.Phone == newUser.Phone)
                    {
                        return true;
                    }
                    if (user.Name == newUser.Name && user.Address == newUser.Address)
                    {
                        return true;
                    }
                }
            }            
            catch(Exception ex)
            {
                _logger.LogError("Unexpected exception", ex.Message);
            }
            
            return false;
        }

        public void Save(List<User> users)
        {
            try
            {
                var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";

                using (var writer = new StreamWriter(path, false))
                {
                    foreach (var user in users)
                    {
                        writer.WriteLine($"{user.Name},{user.Email},{user.Phone},{user.Address},{user.UserType},{user.Money}");
                    }
                }
            }
            catch (IOException ex)
            {
                _logger.LogError("Error saving in database: " + ex.Message);
            }
            catch (Exception ex) 
            {
                _logger.LogError("Unexpected exception", ex.Message);
            }
            
        }

        private StreamReader ReadUsersFromFile()
        {
            try 
            {
                var path = Directory.GetCurrentDirectory() + "/Files/Users.txt";
                FileStream fileStream = new FileStream(path, FileMode.Open);
                StreamReader reader = new StreamReader(fileStream);
                return reader;
            }
            catch(FileNotFoundException ex)
            {
                _logger.LogError("File not found", ex.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError("Unexpected exception", ex.Message);
            }   

            return null;            
        }
    }
}
