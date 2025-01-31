namespace ToolTip.Attributes
{
    /// <summary>
    /// Atributo responsável por compor a descrição no formato de tooltip 
    /// de algum parâmetro da View Model.
    /// 
    /// Este atributo é utilizado pela ToolTipTagHelper a fim de construir um Modal 
    /// que sirva de informação para o cliente.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class TooltipAttribute : Attribute
    {
        /// <summary>
        /// Retenedor da Mensagem descritiva (tooltip) da Propriedade/Field da ViewModel que comporá o elemento HTML
        /// </summary>
        public string ToolTip { get; set; }

        public TooltipAttribute(string toolTip)
        {
            ToolTip = toolTip;
        }
    }
}
