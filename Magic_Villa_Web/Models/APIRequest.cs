﻿using static Magic_Villa_Utilidad.DS;

namespace Magic_Villa_Web.Models
{
    public class APIRequest
    {
        public APITipo APITipo { get; set; } = APITipo.GET;

        public string Url { get; set; }

        public object Datos { get; set; }

        public string Token { get; set; }

        public Parametros Parametros { get; set; }
    }

    public class Parametros
    {
        public int PageNumber { get; set; }

        public int PageSize { get; set; }
    }
}
