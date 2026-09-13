using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Interfaz_Lexico
{
    public class NodoJerarquia
    {
        public string Lexema { get; set; } = "";
        public string TipoNodo { get; set; } = ""; // "Asignacion", "Operador", "Agrupacion", "Identificador", "Constante"
        public int NivelJerarquia { get; set; } // 1 (Paréntesis) a 7 (Asignación)
        public string DescripcionJerarquia { get; set; } = "";
        public string TipoDato { get; set; } = "desconocido"; // "int", "float", "string", "bool"
        public string? ValorCalculado { get; set; }

        public NodoJerarquia? Izquierdo { get; set; }
        public NodoJerarquia? Derecho { get; set; }
        public List<NodoJerarquia> Hijos { get; set; } = new List<NodoJerarquia>();

        public bool TieneError { get; set; } = false;
        public string? MensajeError { get; set; }

        // Inicializa una nueva instancia vacía de un nodo jerárquico NodoJerarquia()
        public NodoJerarquia() { }

        // Inicializa un nodo jerárquico con su información léxica, sintáctica y semántica NodoJerarquia()
        public NodoJerarquia(string lexema, string tipoNodo, int nivel, string descripcion, string tipoDato = "desconocido")
        {
            Lexema = lexema;
            TipoNodo = tipoNodo;
            NivelJerarquia = nivel;
            DescripcionJerarquia = descripcion;
            TipoDato = tipoDato;
        }

        // Convierte el nodo y sus subárboles en un TreeNode formateado con colores e iconos ToTreeNode()
        public TreeNode ToTreeNode()
        {
            string textoNodo;

            if (TieneError)
            {
                textoNodo = $"❌ [ERROR SEMÁNTICO] {Lexema}: {MensajeError}";
            }
            else
            {
                switch (TipoNodo)
                {
                    case "Agrupacion":
                        textoNodo = $"🟣 [Nivel 1: Paréntesis - Prioridad Alta] ( )";
                        break;
                    case "Asignacion":
                        textoNodo = $"🎯 [Nivel 7: Asignación] {Lexema} (Destino: Tipo {TipoDato})";
                        break;
                    case "Operador":
                        string icono = NivelJerarquia switch
                        {
                            2 => "🔺", // Potencia
                            3 => (Lexema == "*" ? "✖️" : Lexema == "/" ? "➗" : "🔣"), // Mult/Div
                            4 => (Lexema == "+" ? "➕" : "➖"), // Suma/Resta
                            5 => "⚖️", // Relacional
                            6 => "🔀", // Lógico
                            _ => "⚙️"
                        };
                        string extra = !string.IsNullOrEmpty(ValorCalculado) ? $" -> Resultado: {ValorCalculado}" : "";
                        textoNodo = $"{icono} [{DescripcionJerarquia}] Operador '{Lexema}' (Tipo: {TipoDato}){extra}";
                        break;
                    case "Identificador":
                        textoNodo = $"📌 [Operando] Variable: '{Lexema}' (Tipo: {TipoDato})";
                        break;
                    case "Constante":
                        textoNodo = $"🔢 [Operando] Constante: {Lexema} (Tipo: {TipoDato})";
                        break;
                    default:
                        textoNodo = $"🔹 {Lexema} ({TipoNodo})";
                        break;
                }
            }

            TreeNode treeNode = new TreeNode(textoNodo);

            if (TieneError)
            {
                treeNode.ForeColor = Color.Red;
            }
            else if (TipoNodo == "Agrupacion")
            {
                treeNode.ForeColor = Color.DarkMagenta;
            }
            else if (TipoNodo == "Asignacion")
            {
                treeNode.ForeColor = Color.Navy;
            }
            else if (TipoNodo == "Operador")
            {
                treeNode.ForeColor = NivelJerarquia switch
                {
                    2 => Color.DarkOrange,
                    3 => Color.DarkGreen,
                    4 => Color.DarkBlue,
                    _ => Color.Black
                };
            }
            else
            {
                treeNode.ForeColor = Color.FromArgb(40, 40, 40);
            }

            // Agregar hijos
            if (Izquierdo != null)
            {
                treeNode.Nodes.Add(Izquierdo.ToTreeNode());
            }

            if (Derecho != null)
            {
                treeNode.Nodes.Add(Derecho.ToTreeNode());
            }

            foreach (var hijo in Hijos)
            {
                treeNode.Nodes.Add(hijo.ToTreeNode());
            }

            return treeNode;
        }
    }
}
