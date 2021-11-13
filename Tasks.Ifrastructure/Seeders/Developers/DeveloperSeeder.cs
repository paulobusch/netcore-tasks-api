using System;
using System.Collections.Generic;
using Tasks.Domain.Developers.Entities;
using Tasks.Ifrastructure._Common.Interfaces;

namespace Tasks.Ifrastructure.Seeders.Developers
{
    public class DeveloperSeeder : ISeeder<Developer>
    {
        public IEnumerable<Developer> GetList()
        {
            return new List<Developer> { 
                new Developer(
                    id: new Guid("86b6b3a7-965e-46dd-843d-661f6e76ded1"),
                    name: "Pleno",
                    login: "pleno",
                    cpf: "13467669085",
                    password: "321654"
                )    
            };
        }
    }
}
