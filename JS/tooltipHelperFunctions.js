
//* Método responsável por gerenciar a abertura do modal das Tooltips das Views do sistema.*/
const apresentarToolTipAPartirDoIconeDeInformacoes = function ()
{
  $(".icone-de-tooltip").on("click", function (handler)
  {
    var icone = handler.currentTarget;
    var nomeDoModalDaToolTip = $(icone).attr("for");

    $(`#${nomeDoModalDaToolTip}`).modal("show");
  });

}
