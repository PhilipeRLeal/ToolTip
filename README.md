# ToolTip

Esta é uma biblioteca em C# ASP NET CORE, a qual é voltada única e exclusivamente para gestão de notificações informativas (a.k.a., tooltips) de recursos constituintes de Views.


## Restrições
Esta biblioteca é aplicável somente a páginas (Views) que se utilizem da tecnologia RAZOR, que é uma tecnologia de marcação para incorporar um código baseado em .NET em páginas da Web (maiores detalhes [aqui](https://learn.microsoft.com/pt-br/aspnet/core/mvc/views/razor?view=aspnetcore-9.0])).

## Exemplo de Uso

### ViewModel
```line_numbers,c#
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace meuNameSpace
{
    public class ViewModel
    {
        
        [Tooltip(@"""Minha tooltip desta propriedade. Nota: esta tooltip será apresentada como corpo do Modal do elemento HTML respectivo a esta propriedade na View.
        Qualquer quebra de linha e/espaçamento aqui aplicado serão preservados no corpo do modal.""")]
        [Description("Minha descrição desta propriedade")]
        [Display(Name="Nome da Propriedade")]
        public bool MinhaPropriedadeBooleana {get; set;}
    }
}
```


### Razor View:
```line_numbers,html
@model ViewModel

<div>
    <form asp-controller="MeuController" asp-action="Cadastrar" method="post" enctype="multipart/form-data">
        <div class="form-group">
            <div class="col-12">
                <tooltip asp-for="@Model.MinhaPropriedadeBooleana"></tooltip>
                <input asp-for="@Model.MinhaPropriedadeBooleana" class="form-control" />
                <span asp-validation-for="@Model.MinhaPropriedadeBooleana"></span>
            </div>
        </div>
        <div class="modal-footer">
            <button type="submit">Submit</button>
        </div>
    </form>
</div>
```

### HTML resultante

```line_numbers, html

<div>
    <form asp-controller="MeuController" asp-action="Cadastrar" method="post" enctype="multipart/form-data">
            <div class="form-group">
                <div class="col-12">
                    <tooltip class="form-label"><div class="d-flex  gap-4">
                        <label class="tooltip-label " for="MontanteASerAdiantado">Montante a ser adiantado (na moeda do contrato
                        </label>
                        <i class="bi bi-info-circle icone-de-tooltip" for="Modal_Adiantamento.MontanteASerAdiantado">
                        </i>
                        <div class="modal fade" for="Adiantamento.MontanteASerAdiantado" id="Modal_Adiantamento.MontanteASerAdiantado" tabindex="-1">
                            <div class="modal-dialog">
                                <div class="modal-content">
                                    <div class="modal-header">
                                    <div>
                                                <h5 class="modal-title h3 text-center">Descrição
                                            </h5>
                                            <h6 class="modal-title h6 text-center">Montante a ser adiantado (na moeda do contrato)
                                            </h6>
                                        </div>
                                        <button class="btn-close" data-bs-dismiss="modal" type="button">
                                        </button>
                                    </div>
                                    <div class="modal-body overflow-scroll" style="max-height:80vh;">
                                        <div data-bs-toggle="tooltip" for="Adiantamento.MontanteASerAdiantado">
                                            <p>Minha tooltip desta propriedade. Nota: esta tooltip será apresentada como corpo do Modal do elemento HTML respectivo a esta propriedade na View.
                                            </p>
                                            <p>Qualquer quebra de linha e/espaçamento aqui aplicado serão preservados no corpo do modal. 
                                            </p>
                                            <blockquote class="blockquote text-center">
                                            </blockquote>
                                        </div>
                                    </div>
                                </div>
                                </div>
                        </div>
                    </tooltip>
                    <input class="form-control" type="text" data-val="true" data-val-number="Campo numérico." data-val-required="The Montante a ser adiantado (na moeda do contrato) field is required." id="Adiantamento_MontanteASerAdiantado" name="Adiantamento.MontanteASerAdiantado" value="">
                   <span class="field-validation-valid" data-valmsg-for="Adiantamento.MontanteASerAdiantado" data-valmsg-replace="true">
                   </span>
                </div>
            </div>
        </div>
        <div class="modal-footer">
            <button type="submit">Submit</button>
        </div>
    </form>
</div>


```

### Representação gráfica do Recurso com Tag helper e respectivo Modal aberto
[]()<img width="444" alt="ToolTip_Imagem" src="https://github.com/user-attachments/assets/ee2e54e4-91fd-49cd-99bc-db08a7748712" />



