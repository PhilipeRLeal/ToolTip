
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ToolTip.Attributes;

namespace ToolTip.HtmlTagHelpers
{


    /// <summary>
    /// Html Tag Helper responsável por gerar um elemento do tipo <label></label> anexado a um elemento de suporte do tipo ToolTip.
    /// 
    /// Este elemento de suporte envolve um ícone de dúvida (?), que, ao ser clicado, abre um modal responsável por apresentar a informação relativa 
    /// ao elemento selecionado.
    /// </summary>
    [HtmlTargetElement("tooltip", Attributes = ModelBinderName)]
    public class LabelComToolTipTagHelper : TagHelper
    {
        private const string ModelBinderName = "asp-for";

        [HtmlAttributeName(ModelBinderName)]
        public ModelExpression ModelBinderDataEntry { get; set; }

        private const string LabelClassesName = "label-class";

        [HtmlAttributeName(LabelClassesName)]
        public string LabelClasses { get; set; }


        private IHtmlGenerator _htmlGenerator;

        // Injecting IHtmlGenerator to access the HTML generation methods for inputs/selects
        public LabelComToolTipTagHelper(IHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
        }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Get the ViewModel property information based on the AspFor attribute
            ModelMetadata modelMetaData = ModelBinderDataEntry.ModelExplorer.Metadata;
            var type = modelMetaData.GetType();

            var listaDeAtributos = (type.GetProperty("Attributes")?.GetValue(modelMetaData) as ModelAttributes)?.Attributes ?? null;

            if ( listaDeAtributos is not null )
            {
                var attribute = listaDeAtributos.Where(a => a.GetType() == typeof(TooltipAttribute)).FirstOrDefault() as TooltipAttribute;

                if ( attribute is not null )
                {
                    output.TagMode = TagMode.StartTagAndEndTag;

                    var toolTipHtmlElement = GerarTagDaToolTip(attribute.ToolTip);

                    var modalHtmlElement = GerarModalDaTooltip(toolTipHtmlElement);

                    // Vamos criar uma tag do tipo icon que seja capaz de abrir o modal que acabamos de criar

                    TagBuilder iconeInformacaoTag = GerarIconeDeInformacaoParaControleDeAberturaDoModalDaToolTip();

                    TagBuilder labelTag = GerarLabelTag();

                    TagBuilder divLabelTag = GerarDivTagParaAgruparIconeDeInformacaoEOLabelDaToolTip(iconeInformacaoTag, labelTag);

                    output.PreContent.AppendHtml(divLabelTag);

                    // Por fim, injetar o modal no content de saída.
                    output.PostContent.AppendHtml(modalHtmlElement);
                }
            }
        }

        /// <summary>
        /// Método responsável por agrupar o ícone de informação com o <paramref name="labelTag"/>
        /// em uma única linha de informação.
        /// </summary>
        /// <param name="iconeInformacaoTag"></param>
        /// <param name="labelTag"></param>
        /// <returns></returns>
        private static TagBuilder GerarDivTagParaAgruparIconeDeInformacaoEOLabelDaToolTip(TagBuilder iconeInformacaoTag, TagBuilder labelTag)
        {
            var divLabelTag = new TagBuilder("div");
            divLabelTag.AddCssClass("d-flex  gap-4");

            divLabelTag.InnerHtml.AppendHtml(labelTag);
            divLabelTag.InnerHtml.AppendHtml(iconeInformacaoTag);
            return divLabelTag;
        }

        /// <summary>
        /// Método responsável por gerar o elemento <paramref name="label"/>.
        /// Este elemento armazenará o nome da propriedade do ViewModel.
        /// </summary>
        /// <returns></returns>
        private TagBuilder GerarLabelTag()
        {
            var label = new TagBuilder("label");

            label.Attributes.Add("for", ModelBinderDataEntry.Metadata.Name);
            label.InnerHtml.Append(ModelBinderDataEntry.Metadata.DisplayName);
            label.AddCssClass("tooltip-label");
            label.AddCssClass(LabelClasses);

            return label;
        }

        /// <summary>
        /// Gerar ícone de informação do label. Este ícone deverá servir de gatilho de abertura de um modal 
        /// contendo a informação do elemento HTML selecionado pelo usuário.
        /// 
        /// Para este funcionamento, é necessário o uso de uma rotina JavaScript no FrontEnd. Vide esta rotina em
        /// 
        /// </summary>
        /// <returns></returns>
        private TagBuilder GerarIconeDeInformacaoParaControleDeAberturaDoModalDaToolTip()
        {
            var iconTag = new TagBuilder("i");
            iconTag.AddCssClass("bi bi-info-circle icone-de-tooltip");
            iconTag.Attributes.Add("for", $"Modal_{ModelBinderDataEntry.Name}");
            return iconTag;
        }

        /// <summary>
        /// Método responsável por gerar o modal que armazenará o texto da ToolTip.
        /// </summary>
        /// <param name="labelHtmlElement"></param>
        /// <returns></returns>
        private IHtmlContent GerarModalDaTooltip(TagBuilder labelHtmlElement)
        {
            // Generate the HTML <label> element
            var modalHtmlElement = new TagBuilder("div");

            // Atribuindo o nome da propriedade do viewmodel ao atributo for do <label>
            modalHtmlElement.AddCssClass("modal");
            modalHtmlElement.AddCssClass("fade");
            modalHtmlElement.Attributes.Add("for", ModelBinderDataEntry.Name);
            modalHtmlElement.Attributes.Add("tabindex", "-1");
            modalHtmlElement.Attributes.Add("id", $"Modal_{ModelBinderDataEntry.Name}");

            var modaldialog = new TagBuilder("div");
            modaldialog.AddCssClass("modal-dialog");

            var modalContent = new TagBuilder("div");
            modalContent.AddCssClass("modal-content");

            var modalHeader = new TagBuilder("div");
            modalHeader.AddCssClass("modal-header");

            var modalTitleWrapper = new TagBuilder("div");

            var modalTitle = new TagBuilder("h5");
            modalTitle.AddCssClass("modal-title h3");
            modalTitle.AddCssClass("text-center");
            modalTitle.InnerHtml.Append($"Descrição");

            var modalTitleVariavel = new TagBuilder("h6");
            modalTitleVariavel.AddCssClass("modal-title h6");
            modalTitleVariavel.AddCssClass("text-center");
            modalTitleVariavel.InnerHtml.Append($"{ModelBinderDataEntry.Metadata.DisplayName}");

            modalTitleWrapper.InnerHtml.AppendHtml(modalTitle);
            modalTitleWrapper.InnerHtml.AppendHtml(modalTitleVariavel);

            var botaoDeFechamentoDoModal = new TagBuilder("button");
            var atributos = new Dictionary<string, string> {
                                                          { "type", "button" },
                                                          { "data-bs-dismiss", "modal" },

            };
            foreach ( var (key, value) in atributos )
            {
                botaoDeFechamentoDoModal.Attributes.Add(key, value);
            }
            botaoDeFechamentoDoModal.AddCssClass("btn-close");


            var modalBody = new TagBuilder("div");
            modalBody.AddCssClass("modal-body");

            modalBody.AddCssClass("overflow-scroll");
            modalBody.Attributes.Add("style", "max-height:80vh;");

            modalBody.InnerHtml.AppendHtml(labelHtmlElement);
            modalHeader.InnerHtml.AppendHtml(modalTitleWrapper);
            modalHeader.InnerHtml.AppendHtml(botaoDeFechamentoDoModal);
            modalContent.InnerHtml.AppendHtml(modalHeader);
            modalContent.InnerHtml.AppendHtml(modalBody);
            modaldialog.InnerHtml.AppendHtml(modalContent);
            modalHtmlElement.InnerHtml.AppendHtml(modaldialog);

            return modalHtmlElement;
        }

        /// <summary>
        /// Gera elemento HTML responsável por armazenar o texto da Tooltip
        /// </summary>
        /// <returns></returns>
        private TagBuilder GerarTagDaToolTip(string tooltip)
        {

            // Generate the HTML <div> element
            var elementoTextual = new TagBuilder("div");

            // Atribuindo o nome da propriedade do viewmodel ao atributo for do <label>
            elementoTextual.Attributes.Add("for", ModelBinderDataEntry.Name);

            elementoTextual.Attributes.Add("data-bs-toggle", "tooltip");

            var bloboDeCitacaoInicializado = false;
            TagBuilder? blockQuote = null;

            foreach ( var linha in tooltip.Split("\n") )
            {
                var paragrafo = new TagBuilder("p");
                paragrafo.InnerHtml.Append(linha);

                if ( linha.Replace(" ", "") == "\r" )
                {
                    continue;
                }

                if ( linha.Contains("<quotation>") &&
                    blockQuote is null )
                {
                    bloboDeCitacaoInicializado = true;
                    blockQuote = new TagBuilder("blockquote");
                    blockQuote.AddCssClass("blockquote text-center");

                }
                else if ( !linha.Contains("</quotation>") &&
                         bloboDeCitacaoInicializado )
                {
                    blockQuote.InnerHtml.Append(linha);
                }
                else if ( linha.Contains("</quotation>") &&
                          blockQuote is not null )
                {
                    bloboDeCitacaoInicializado = false;
                    elementoTextual.InnerHtml.AppendHtml(blockQuote);
                }
                else
                {
                    bloboDeCitacaoInicializado = false;
                    elementoTextual.InnerHtml.AppendHtml(paragrafo);
                }
            }

            return elementoTextual;
        }
    }
}
