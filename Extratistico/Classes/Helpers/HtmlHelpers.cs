using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace System.Web.Mvc
{
    public static class HtmlHelpers
    {
        public enum TypeButton { Button, Submit, Reset, Back };

        public enum TypeActionLink { Normal, Table };

        public static MvcHtmlString YearMonthPickerDialog(this HtmlHelper helper, int startYear)
        {
            string html = string.Empty;

            
            html += string.Format("<input id={0}btnYearPicker{0} type={0}button{0} value={0}Selecionar Período{0} onclick={0}$('#yearMonthPickerDialog').dialog('open'){0} />", (char)34);
            html += "<div id='yearMonthPickerDialog' title='Selecione o ano e mês da pesquisa'>";
            html += "<select id='selectAno' name='selectAno'>";
            while (startYear <= DateTime.Now.Year)
            {
                html += "<option value='" + startYear + "'>" + startYear + "</option>";
                startYear++;
            }
            html += "</select>";

            html += "<select id='selectMes' name='selectMes'>";
            html += "<option value='0'>Todos</option>";
            html += "<option value='1'>Janeiro</option>";
            html += "<option value='2'>Fevereiro</option>";
            html += "<option value='3'>Março</option>";
            html += "<option value='4'>Abril</option>";
            html += "<option value='5'>Maio</option>";
            html += "<option value='6'>Junho</option>";
            html += "<option value='7'>Julho</option>";
            html += "<option value='8'>Agosto</option>";
            html += "<option value='9'>Setembro</option>";
            html += "<option value='10'>Outubro</option>";
            html += "<option value='11'>Novembro</option>";
            html += "<option value='12'>Dezembro</option>";
            html += "</select>";

            html += "</div>";

            return MvcHtmlString.Create(html);
        }

        public static MvcHtmlString YearPickerDialog(this HtmlHelper helper)
        {
            string html = string.Empty;

            int ano = 2008;

            html += string.Format("<input id={0}btnYearPicker{0} type={0}button{0} value={0}Selecionar Ano{0} onclick={0}$('#yearPickerDialog').dialog('open'){0} />", (char)34);
            html += "<div id='yearPickerDialog' title='Selecione o ano da pesquisa'>";
            html += "<select id='selectAno' name='selectAno'>";
            while (ano <= DateTime.Now.Year)
            {
                html += "<option value='" + ano + "'>" + ano + "</option>";
                ano++;
            }            
            html += "</select>";
            html += "</div>";

            return MvcHtmlString.Create(html);
        }

        public static MvcHtmlString DatePickerDialog(this HtmlHelper helper)
        {
            string html = string.Empty;

            html += string.Format("<input id={0}btnDatePicker{0} type={0}button{0} value={0}Selecionar Datas{0} onclick={0}$('#datePickerDialog').dialog('open'){0} />", (char)34);
            html += "<div id='datePickerDialog' title='Selecione o intervalo de datas'>";
            html += "<div id='message'></div>";
            html += "<label for='startdate'>Start Date</label>";
            html += "<input class='picker' id='startdate' type='text' readonly /><input type='hidden' name='startdate' id='start_alternate' />";
            html += "<label for='enddate'>End Date</label>";
            html += "<input class='picker' id='enddate' type='text' readonly /><input type='hidden' name='enddate' id='end_alternate' /></div>";

            return MvcHtmlString.Create(html);
        }

        public static string ContaUsuario(this HtmlHelper helper, DynamicMenu menu)
        {
            string html = string.Empty;
            if (menu == null)
            {
                return html;
            }

            html += "<div class='right'>";
            //html += "<img class='imagemconta' src='/Content/Images/ico-logoff-24.png'>";
            html += string.Format("<label class='logout'>Bem-vindo, {0}!  </label><a class='logout' href='/Administrativo/Conta/Detalhes?username={0}'>Minha Conta</a>", menu.username);
            html += "|";
            html += "<a class='logout' href='/Login/Logout' onclick = \"return confirm('Deseja realmente sair da aplicação?')\">Sair</a>";
            html += "</div>";
            return html;
        }

        public static string DynamicMenu(this HtmlHelper helper, DynamicMenu menu)
        {
            string html = string.Empty;
            string submenu = string.Empty;
            html += "<div id='menu'><ul id='primeiro'><li><h2><a href='/'>Home</a></h2></li></ul>";
            //html += "<ul><li><h2><a href='/Sobre'>Sobre</a></h2></li></ul>";

            if (menu == null)
            {
                html += "</div>";
                return html;
            }

            foreach (var area in menu.funcionalidades.Select(i => i.Area).Distinct())
            {
                foreach (var item in menu.funcionalidades.Where(x => x.Area == area && x.Action == "Index"))
                {
                    submenu += String.Format("<li><a href='/{1}/{2}'>{0}</a></li>", item.Descricao, item.Area, item.Controller);
                }
                html += String.Format("<ul><li><h2>{0}</h2><ul>{1}</ul></li></ul>", area.Replace("_"," "), submenu);
                submenu = string.Empty;
            }

            html += "</div>";

            return html;
        }

        public static string ImageCheckBox(this HtmlHelper helper, bool status)
        {
            return string.Format("<img class='checkimage' src='{0}'/>", status ? "/Content/Images/habilitar-20.png" : "/Content/Images/desabilitar-20.png");
        }

        public static string Button(this HtmlHelper helper, TypeButton type, string imagePath, string buttonName, string buttonText)
        {
            switch (type)
            {
                case TypeButton.Submit:
                    return String.Format(
                        "<span class='button-n'>" +
                        "<button type='submit' name='{0}'>" +
                        "<img src='{1}' alt=''/>" +
                        "{2}" +
                        "</button>" +
                        "</span>",
                        buttonName, imagePath, buttonText);

                case TypeButton.Button:
                    return String.Format(
                        "<span class='button-n'>" +
                        "<button type='button' name='{0}'>" +
                        "<img src='{1}' alt=''/>" +
                        "{2}" +
                        "</button>" +
                        "</span>",
                        buttonName, imagePath, buttonText);

                case TypeButton.Reset:
                    return String.Format(
                        "<span class='button-n'>" +
                        "<button type='reset' name='{0}'>" +
                        "<img src='{1}' alt=''/>" +
                        "{2}" +
                        "</button>" +
                        "</span>",
                        buttonName, imagePath, buttonText);
                case TypeButton.Back:
                    return String.Format(
                        "<span class='button-n'>" +
                        "<button type='button' name='voltar' onclick='javascript:history.go(-1)'>" +
                        "<img src='{0}' alt=''/>" +
                        "Voltar" +
                        "</button>" +
                        "</span>",
                        imagePath);
                default:
                    return "An error occurred while the button was created.";
            }
        }

        public static string Button(this HtmlHelper helper, TypeActionLink type, string imagePath, MvcHtmlString actionLink)
        {
            switch (type)
            {
                case TypeActionLink.Normal:
                    return String.Format(
                        "<span class='button-al'>" +
                        "<img src='{0}' alt=''/>" +
                        "{1}" +
                        "</span>",
                        imagePath, actionLink);

                case TypeActionLink.Table:
                    return String.Format(
                        "<span class='button-t'>" +
                        "<img src='{0}' alt=''/>" +
                        "{1}" +
                        "</span>",
                        imagePath, actionLink);

                default:
                    return "An error occurred while the button was created.";
            }
        }

        public static string ButtonJS(this HtmlHelper helper, string imagePath, string buttonName, string buttonText, string JSFunction)
        {
            return String.Format(
                        "<span class='button-n'>" +
                        "<button type='button' name='{0}' onclick='{3}'>" +
                        "<img src='{1}' alt=''/>" +
                        "{2}" +
                        "</button>" +
                        "</span>",
                        buttonName, imagePath, buttonText, JSFunction);
        }

        public static MvcHtmlString ImageActionLink(this HtmlHelper helper, string imagePath, string actionName, string controllerName,
            object routeValues, object imageAttributes, object linkAttributes)
        {
            var urlHelper = new UrlHelper(helper.ViewContext.RequestContext, helper.RouteCollection);

            var aElementBuilder = new TagBuilder("a");
            aElementBuilder.MergeAttributes(new RouteValueDictionary(linkAttributes));
            aElementBuilder.MergeAttribute("href", urlHelper.Action(actionName, controllerName, routeValues));

            var imgElementBuilder = new TagBuilder("img");
            aElementBuilder.MergeAttributes(new RouteValueDictionary(imageAttributes));
            imgElementBuilder.MergeAttribute("src", imagePath);

            aElementBuilder.InnerHtml = imgElementBuilder.ToString(TagRenderMode.SelfClosing);
            return new MvcHtmlString(aElementBuilder.ToString(TagRenderMode.Normal));
        }

        public static string Paginar(this HtmlHelper helper, int atual, int total, int paginas)
        {
            string html = string.Empty;
            if ((int)total > 10)
            {
                if ((atual >= 0) && (atual < 5))
                {

                    if (paginas <= 10)
                    {
                        for (int i = 1; i <= paginas + 1; i++)
                        {
                            if (atual + 1 != i)
                            {
                                html += "<span class='pager' onclick='paginacao(" + (i - 1) + ")'> " + (i) + " </span>";
                            }
                            else
                            {
                                html += "<span> " + i + " </span>";
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= 10; i++)
                        {
                            if ((int)atual + 1 != i)
                            {
                                html += "<span class='pager' onclick='paginacao(" + (i - 1) + ")'> " + (i) + " </span>";
                            }
                            else
                            {
                                html += "<span> " + i + " </span>";
                            }
                        }
                    }

                }
                else if ((atual >= 5) && (atual < (paginas - 5)))
                {
                    for (int i = ((int)atual - 3); i <= ((int)atual + 6); i++)
                    {
                        if ((int)atual + 1 != i)
                        {
                            html += "<span class='pager' onclick='paginacao(" + (i - 1) + ")'> " + (i) + " </span>";
                        }
                        else
                        {
                            html += "<span> " + (i) + " </span>";
                        }
                    }
                }
                else
                {
                    for (int i = (paginas - 9); i <= paginas; i++)
                    {
                        if (atual + 1 != i)
                        {
                            html += "<span class='pager' onclick='paginacao(" + (i - 1) + ")'> " + (i) + " </span>";
                        }
                        else
                        {
                            html += "<span> " + (i) + " </span>";
                        }
                    }
                }
            }
            return html;
        }

        public static string File(this HtmlHelper helper, string name)
        {
            return String.Format(
                "<input type='file' name='{0}' id='{0}' />",
                name);
        }
    }
}

