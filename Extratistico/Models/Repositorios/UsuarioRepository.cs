using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extratistico.Classes.DataContext;
using Extratistico.Models.Entidades;
using System.Data;

namespace Extratistico.Models.Repositorios
{
    public class UsuarioRepository
    {
        private DataContext dataContext = new DataContext();

        public void Commit()
        {
            dataContext.Commit();
        }

        public void RollBack()
        {
            dataContext.Rollback();
        }

        public bool Exists(string login)
        {
            var usuario = SelectSingle(login);

            if (usuario != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Usuario> Select(string username)
        {
            DataTable table = dataContext.Select(String.Format("select *from usuario where username != '{0}' and username!='Admin'",username));
            List<Usuario> usuarios = new List<Usuario>();
            foreach (DataRow row in table.Rows)
	        {
                usuarios.Add(new Usuario()
                {
                    Nome = row["Nome"].ToString(),
                    Email = row["Email"].ToString(),
                    Password = row["Password"].ToString(),
                    Status = (Convert.ToInt32(row["Status"]) == 1 ? true : false),
                    Username = row["Username"].ToString(),
                });
	        }
            return usuarios;
        }

        public Usuario SelectSingle(string username)
        {
            DataRow row = dataContext.SelectSingle(String.Format("select *from usuario where username='{0}'", username));
            if (row != null)
            {
                return new Usuario()
                {
                    Nome = row["Nome"].ToString(),
                    Email = row["Email"].ToString(),
                    Password = row["Password"].ToString(),
                    Status = (Convert.ToInt32(row["Status"]) == 1 ? true : false),
                    Username = row["Username"].ToString(),
                };
            }
            else
            {
                return null;
            }
        }

        public Usuario SelectSingle(string username, string password)
        {
            DataRow row = dataContext.SelectSingle(String.Format("select *from usuario where username='{0}' and password='{1}'", username, password));
            if (row != null)
            {
                return new Usuario()
                {
                    Nome = row["Nome"].ToString(),
                    Email = row["Email"].ToString(),
                    Password = row["Password"].ToString(),
                    Status = (Convert.ToInt32(row["Status"]) == 1 ? true : false),
                    Username = row["Username"].ToString(),
                };
            }
            else
            {
                return null;
            }
        }

        public int Add(Usuario usuarioToAdd)
        {
                return dataContext.ExecuteNonQuery(String.Format(
                "insert into usuario(username, nome, email, password, status) values('{0}','{1}','{2}','{3}',{4})",
                usuarioToAdd.Username,usuarioToAdd.Nome,usuarioToAdd.Email,usuarioToAdd.Password,usuarioToAdd.Status ? 1 : 0));
        }

        public int Edit(Usuario usuarioToEdit)
        {
            return dataContext.ExecuteNonQuery(String.Format("update usuario set nome='{0}',email='{1}',password='{2}',status={3} where username='{4}'",
            usuarioToEdit.Nome, usuarioToEdit.Email, usuarioToEdit.Password, usuarioToEdit.Status ? 1 : 0, usuarioToEdit.Username));
        }

        public int Delete(string username)
        {
            return dataContext.ExecuteNonQuery(String.Format("delete from usuario where username='{0}'", username));
        }
    }
}
