using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Extratistico.Models.Entidades;
using Extratistico.Classes.DataContext;
using System.Data;
using System.IO;
using System.Reflection;
using System.Web.Mvc;

namespace Extratistico.Models.Repositorios
{
    public class FuncionalidadesRepository
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


        public void Update()
        {
            foreach (var f in SelectNew())
            {
                Add(f);
                Commit();
            }
        }

        public bool Exists(Funcionalidade f)
        {
            return Convert.ToInt32(dataContext.ExecuteScalar(string.Format("select count(*) from funcionalidade where area='{0}' and controller='{1}' and action='{2}' and descricao='{3}' and controllerdescription='{4}'",
                f.Area,f.Controller,f.Action,f.Descricao,f.ControllerDescription))) == 1;
        }

        public static List<Funcionalidade> SelectAll()
        {
            List<Funcionalidade> funcionalidades = new List<Funcionalidade>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()
                .Where(myType => myType.IsSubclassOf(typeof(Controller)) && myType.Name != "HomeController"))
            {
                foreach (var item in type.GetMethods().Where(x => x.GetCustomAttributes(typeof(DynamicPermission), false).Count() != 0 && x.GetCustomAttributes(typeof(HttpPostAttribute), false).Count() == 0))
                {
                    string name = ((DynamicPermission)item.GetCustomAttributes(typeof(DynamicPermission), false)[0]).Name;
                    string controller = ((DynamicPermission)item.GetCustomAttributes(typeof(DynamicPermission), false)[0]).ControllerDescription;
                    Funcionalidade f = new Funcionalidade()
                    {
                        Area = type.FullName.Split('.')[type.FullName.Split('.').Length - 3],
                        Controller = type.Name.Replace("Controller", ""),
                        ControllerDescription = controller != null ? controller : type.Name.Replace("Controller", ""),
                        Descricao = name != null ? name : item.Name,
                        Action = item.Name
                    };
                    funcionalidades.Add(f);
                }
            }
            return funcionalidades;
        }

        public List<Funcionalidade> SelectNew()
        {
            List<Funcionalidade> funcionalidades = new List<Funcionalidade>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()
                .Where(myType => myType.IsSubclassOf(typeof(Controller)) && myType.Name != "HomeController"))
            {
                foreach (var item in type.GetMethods().Where(x => x.GetCustomAttributes(typeof(DynamicPermission), false).Count() != 0 && x.GetCustomAttributes(typeof(HttpPostAttribute), false).Count() == 0))
                {
                    string name = ((DynamicPermission)item.GetCustomAttributes(typeof(DynamicPermission), false)[0]).Name;
                    string controller = ((DynamicPermission)item.GetCustomAttributes(typeof(DynamicPermission), false)[0]).ControllerDescription;
                    Funcionalidade f = new Funcionalidade()
                    {
                        Area = type.FullName.Split('.')[type.FullName.Split('.').Length - 3],
                        Controller = type.Name.Replace("Controller", ""),
                        ControllerDescription = controller != null ? controller : type.Name.Replace("Controller", ""),
                        Descricao = name != null ? name : item.Name,
                        Action = item.Name
                    };
                    if (!Exists(f))
                    {
                        funcionalidades.Add(f);
                    }                    
                }
            }
            return funcionalidades;
        }

        public static List<Funcionalidade> SelectSGBD()
        {
            List<Funcionalidade> funcionalidades = new List<Funcionalidade>();
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes()
                .Where(myType => myType.IsSubclassOf(typeof(Controller)) && myType.Name == "SGBDController"))
            {
                foreach (var item in type.GetMethods().Where(x => x.GetCustomAttributes(typeof(DynamicPermission), false).Count() != 0 && x.GetCustomAttributes(typeof(HttpPostAttribute), false).Count() == 0))
                {
                    string name = ((DynamicPermission)item.GetCustomAttributes(typeof(DynamicPermission), false)[0]).Name;
                    string controller = ((DynamicPermission)item.GetCustomAttributes(typeof(DynamicPermission), false)[0]).ControllerDescription;
                    Funcionalidade f = new Funcionalidade()
                    {
                        Area = type.FullName.Split('.')[type.FullName.Split('.').Length - 3],
                        Controller = type.Name.Replace("Controller", ""),
                        ControllerDescription = controller != null ? controller : type.Name.Replace("Controller", ""),
                        Descricao = name != null ? name : item.Name,
                        Action = item.Name
                    };
                    funcionalidades.Add(f);
                }
            }
            return funcionalidades;
        }

        public Dictionary<Funcionalidade, bool> Select()
        {
            DataTable table = dataContext.Select("select *from funcionalidade");
            Dictionary<Funcionalidade, bool> funcionalidades = new Dictionary<Funcionalidade, bool>();
            foreach (DataRow row in table.Rows)
            {
                funcionalidades.Add(new Funcionalidade()
                {
                    Action = row["Action"].ToString(),
                    Area = row["Area"].ToString(),
                    Codigo = Convert.ToInt32(row["Codigo"]),
                    Controller = row["Controller"].ToString(),
                    ControllerDescription = row["ControllerDescription"].ToString(),
                    Descricao = row["Descricao"].ToString()
                },false);
            }
            return funcionalidades;
        }

        //public Funcionalidade SelectSingle(string descricao)
        //{
        //    DataRow row = dataContext.SelectSingle(String.Format("select *from funcionalidade where descricao='{0}'", descricao));
        //    return new Funcionalidade()
        //    {
        //        Action = row["Action"].ToString(),
        //        Area = row["Area"].ToString(),
        //        Codigo = Convert.ToInt32(row["Codigo"]),
        //        Controller = row["Controller"].ToString(),
        //        ControllerDescription = row["ControllerDescription"].ToString(),
        //        Descricao = row["Descricao"].ToString()
        //    };            
        //}

        public List<Funcionalidade> Select(int[] codPerfil)
        {
            if (codPerfil.Length == 0)
            {
                return new List<Funcionalidade>();
            }

            string codigos = string.Empty;
            foreach (var item in codPerfil)
            {
                codigos += item + ",";
            }

            DataTable table = dataContext.Select(String.Format("select f.* from funcionalidade f inner join permissao p on " +
            "p.funcionalidade=f.codigo where p.perfil in({0})", codigos.Substring(0, codigos.Length - 1)));
            List<Funcionalidade> funcionalidades = new List<Funcionalidade>();
            foreach (DataRow row in table.Rows)
            {
                funcionalidades.Add(new Funcionalidade()
                {
                    Action = row["Action"].ToString(),
                    Area = row["Area"].ToString(),
                    Codigo = Convert.ToInt32(row["Codigo"]),
                    Controller = row["Controller"].ToString(),
                    ControllerDescription = row["ControllerDescription"].ToString(),
                    Descricao = row["Descricao"].ToString(),
                });
            }
            return funcionalidades;
        }

        public int Add(Funcionalidade f)
        {
            int codigo = Convert.ToInt32(dataContext.ExecuteScalar("select case when max(codigo) is null then 1 else max(codigo) + 1 end as codigo from funcionalidade"));
            return dataContext.ExecuteNonQuery(String.Format("insert into funcionalidade(Codigo,Action, Area, Controller,ControllerDescription, Descricao) values({0},'{1}','{2}','{3}','{4}','{5}')",
                codigo, f.Action, f.Area, f.Controller,f.ControllerDescription, f.Descricao));
        }
    }
}
