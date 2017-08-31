//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using Extratistico.Models.Entidades;
//using Extratistico.Classes.DataContext;
//using System.Data;

//namespace Extratistico.Models.Repositorios
//{
//    public class LoginRepository
//    {
//        private DataContext dataContext = new DataContext();

//        public bool Exists(string username)
//        {
//            return Convert.ToInt32(dataContext.ExecuteScalar(string.Format("select count(*) from usuario where username='{0}'", username))) != 0;
//        }

//        public bool Exists(string username, string senha)
//        {
//            var usuario = SelectSingle(username, senha);

//            if (usuario != null)
//            {
//                return true;
//            }
//            else
//            {
//                return false;
//            }
//        }

//        public List<Usuario> Select()
//        {
//            DataTable table = dataContext.Select("select *from usuario");
//            List<Usuario> usuarios = new List<Usuario>();
//            foreach (DataRow row in table.Rows)
//            {
//                usuarios.Add(new Usuario()
//                {
//                    Nome = row["Nome"].ToString(),
//                    Email = row["Email"].ToString(),
//                    Password = row["Password"].ToString(),
//                    Status = ((UInt64)row["Status"] == 1 ? true : false),
//                    Username = row["Username"].ToString(),
//                });
//            }
//            return usuarios;
//        }

//        public Usuario SelectSingle(string username)
//        {
//            DataRow row = dataContext.SelectSingle(String.Format("select *from usuario where username='{0}')", username));
//            return new Usuario()
//            {
//                Nome = row["Nome"].ToString(),
//                Email = row["Email"].ToString(),
//                Password = row["Password"].ToString(),
//                Status = ((UInt64)row["Status"] == 1 ? true : false),
//                Username = row["Username"].ToString(),
//            };
//        }

//        public Usuario SelectSingle(string username, string password)
//        {
//            DataRow row = dataContext.SelectSingle(String.Format("select *from usuario where username='{0}' and password='{1}')", username, password));
//            return new Usuario()
//            {
//                Nome = row["Nome"].ToString(),
//                Email = row["Email"].ToString(),
//                Password = row["Password"].ToString(),
//                Status = ((UInt64)row["Status"] == 1 ? true : false),
//                Username = row["Username"].ToString(),
//            };
//        }

//        public int Add(Usuario usuario)
//        {
//            db.Insert<Usuario>(usuario);
//        }

//        public int Edit(Usuario usuarioToEdit)
//        {
//            db.Update<Usuario>(usuarioToEdit);
//        }

//        public int Delete(string username)
//        {
//            return dataContext.ExecuteNonQuery(String.Format("delete from usuario where username='{0}'", username));
//        }

//    }
//}
