function AddRow() {    
    var DescItemAvaliacao = $("#DescItemAvaliacao").val();
    var ImpactoNota = $("#ImpactoNota").val();
    var IdNotaMaxima = $("#IdNotaMaxima").val();
}

function RemoveRow(id, index) {    
    $('#' + id).hide();
    $("#ItensAvaliacao_" + index + "__Excluir").val('true');
}

function AddItemAvaliacao() {
    
    var index = ItensAvaliacaoCount - 1;

    $.ajax({
        type: "post",
        dataType: "html",
        url: ROOT + '/CriterioAvaliacao/AddItemAvaliacao',
        data: { pCount: ItensAvaliacaoCount },
        success: function (response) {
            var table = $('#tableItensAvaliacao > tbody:last-child');            
            table.append(response);
            ItensAvaliacaoCount++;
            $(".impacto-nota").mask("-9,9");
        }
    });
}