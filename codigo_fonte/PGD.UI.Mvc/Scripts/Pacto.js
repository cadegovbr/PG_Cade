$("input[name='EntregueNoPrazo']").on("change", function () {
    if ($(this).val() === "1") {
        document.getElementById('divJustificativa').style.display = "none";
        $('#tblInterromper').show();
        $('#Justificativa').removeAttr('required');
        $('#DataTerminoReal').removeAttr('required');
        $('#EntregueNoPrazo').val('1');

    }
    else {
        document.getElementById('divJustificativa').style.display = "block";
        $('#tblInterromper').show();
        $('#Justificativa').attr('required', 'required');
        $('#DataTerminoReal').attr('required', 'required');
        $('#EntregueNoPrazo').val('0');
    }
});

function MostrarOpcoesEntregaNoPrazo(linha, valor) {

    if (valor == 0) {
        document.getElementById('divJustificativa[' + linha + ']').style.display = '';
    }
    else {
        document.getElementById('divJustificativa[' + linha + ']').style.display = 'none';
    }

}

function CarregaInformacoesAdicionais(idPacto, idProduto) {
    $("#modalGenerica").modal('show');
    Loading($("#ContentmodalGenerica"));
    RenderPartial("Pacto", "Observacoes", { idPacto: idPacto, idProduto: idProduto }, "get").done(function (success) {
        $("#ContentmodalGenerica").html(success);
    });

}

function SuspenderPacto() {

    var _motivo = $("#motivosuspender").val();
    var _data = $("#datasuspensao").val();

    if (_motivo === "") {
        $("#divAlertaSupender").removeClass('hidden');
        $('#lblMsg').html('O campo Motivo é de preenchimento obrigatório!');
        $('#lblMsg').show();
        $('#lblMsg').delay(8000).fadeOut(1600);
    }
    else if (_data === "") {
        $("#divAlertaSupender").removeClass('hidden');
        $('#lblMsg').html('O campo Suspenso a partir de é de preenchimento obrigatório!');
        $('#lblMsg').show();
        $('#lblMsg').delay(8000).fadeOut(1600);
    }
    else {
        $("#divAlertaSupender").addClass('hidden');
        $('#lblMsg').hide();
        ExecutaJson("Pacto", "Suspender", { idpacto: $('#idPacto').val(), motivo: $("#motivosuspender").val(), data: $("#datasuspensao").val() })
            .done(function (result) {
                if (result.IsValid) {
                    console.log(1);
                    //ShowSuccessMessage(result.Message);
                    console.log(2);
                    $('#modalSuspender').modal('hide');
                    location.reload();
                }
                else {
                    console.log(3);
                    for (var i = 0; i < result.Erros.length; i++) {
                        console.log(result.Erros[i].Message);
                        //ShowErrorMessage(result.Erros[i].Message);
                    };
                    $('#modalSuspender').modal('hide');
                    location.reload();
                }
            });
    }
}

function ShowSuspenderPacto(idpacto) {
    $('#idPacto').val(idpacto);
    $('#modalSuspender').modal('show');
}

function ShowRetornarSuspensao(idpacto, datasuspensao) {
    RenderPartial("Pacto", "RetornaSuspensao", { idPacto: idpacto }, "get").done(function (success) {
        var div = $('#modalVoltarSuspensao');
        div.html(success);
    });

    $('#modalVoltarSuspensao').modal('show');
}

function AssinarPacto(_idpacto) {
    ExecutaJson("Pacto", "Assinar", { idpacto: _idpacto }).done(function (success) {
        ShowSuccessMessage("Plano de Trabalho assinado com sucesso.");
        $("#Alertas").show();
        $("#Alertas").delay(8000).fadeOut(1600);
        location.href = "/Pacto/Index";
    }
    );
}

function NegarPacto(_idpacto) {

    ExecutaJson("Pacto", "Negar", { idpacto: _idpacto }).done(function (success) {
        location.href = "/Pacto/Index";
        ShowSuccessMessage("Pacto negado com sucesso.");
        $("#Alertas").show();
        $("#Alertas").delay(8000).fadeOut(1600);
    }
    );
}

function checkedAll() {
    var radioButtons = $('table#tblInterromper input[type="radio"]');
    console.log(radioButtons.length);
    for (var i = 0; i < radioButtons.length; i++) {
        console.log(radioButtons[i].checked);
        if (radioButtons[i].checked === true) {
            console.log(radioButtons[i].checked);
        }
    }
}


 

function VoltaSuspensao() {
    ExecutaJson("Pacto", "VoltaSuspensao", { idpacto: $('#idPacto').val() })
        .done(function (result) {
            $('#modalVoltarSuspensao').modal('hide');
            if (result.IsValid) {
                console.log(1);
                ShowSuccessMessage(result.Message);
                $("#Alertas").show();
                $("#Alertas").delay(8000).fadeOut(1600);
                location.reload();
            }
            else {
                console.log(3);
                for (var i = 0; i < result.Erros.length; i++) {
                    console.log(result.Erros[i].Message);
                    ShowErrorMessage(result.Erros[i].Message);
                    $("#Alertas").show();
                    $("#Alertas").delay(8000).fadeOut(1600);
                };
            }
        });
}

$(document).ready(function () {
    $('#DataPrevistaInicio').change(function () {
        if ($("#lblCargaTotal").val() !== "0" && $("#lblCargaTotal").val() !== "") {
            ExecutaJson("Pacto",
                "AtualizaData",
                {
                    datainicio: $("#DataPrevistaInicio").val(),
                    cargahoraria: $("#lblCargaTotal").val(),
                    cargahorariapacto: $(CargaHorariaDigitada).val(),
                    editar: false,
                    excluir: false,
                    indice: 1
                },
                "post").done(function (data) {
                    $("#lblDataPrevistaTermino").text(data);
                    $("#DataPrevistaTermino").val(data);
                });
        }
    });
});

function ShowAjustarCronograma(action) {
    RenderPartial("Pacto", action, { datatermino: $("#DataPrevistaTermino").val(), datainicio: $("#DataPrevistaInicio").val(), cargahorariatotal: $("#CargaHorariaTotal").val().replace(',00', ''), cargahorariapacto: $(CargaHorariaDigitada).val() }, "post").done(function (success) {
        $('#modalCronograma').html(success);
        CarregaLabelsCronograma();
    });

    $('#modalCronograma').modal('show');
}

$(document).on('keyup', ".qtdHoras", function () {
    CarregaLabelsCronograma();
});
$(document).on('change', ".qtdHoras", function () {
    CarregaLabelsCronograma();
});
function CarregaLabelsCronograma() {
    var horaInicial = 0;
    if ($("#lblCargaTotal").val() !== "") {
        $("#lblCargaTotalCronograma").val($("#lblCargaTotal").val().replace(',', '.'));
        $("#lblCargaTotalCronograma").text($("#lblCargaTotal").val().replace(',', '.'));
        horaInicial = Number($("#lblCargaTotal").val());
    }
    else {
        $("#lblCargaTotalCronograma").val($("#lblCargaTotal").text().replace(',', '.'));
        $("#lblCargaTotalCronograma").text($("#lblCargaTotal").text().replace(',', '.'));
        horaInicial = Number($("#lblCargaTotal").text().replace(',', '.'));
    }
    var resultadoCargaDistribuida = 0;
    var resultadoCargaDistribuir = 0;
    var cargaHorariaTotal = 0;
    var dist = 0;
    var index = 0;
    $("#tblCronogramas > tbody > tr").each(function (data) {
        if ($("#Cronogramas_" + index + "__HorasCronograma").val() !== undefined) {
            var hora = Number($("#Cronogramas_" + index + "__HorasCronograma").val());
            resultadoCargaDistribuida += hora;
            index++;
        }
    });
    dist = horaInicial - resultadoCargaDistribuida;

    $("#lblCargaDistribuida").val(resultadoCargaDistribuida.toFixed(2));
    $("#lblCargaDistribuida").text(resultadoCargaDistribuida.toFixed(2));

    $("#lblCargaDistribuir").val(dist.toFixed(2));
    $("#lblCargaDistribuir").text(dist.toFixed(2));


}
function RetiraDataZerada(cronogramaVM) {
    var ultimo = cronogramaVM[cronogramaVM.length - 1];
    var resultado = cronogramaVM;
    if (ultimo.HorasCronograma === 0) {
        cronogramaVM.pop();
        resultado = RetiraDataZerada(cronogramaVM);
    }
    return resultado;
}
function ExisteDiaMaior() {
    var index = 0;
    var resultado = false;
    $("#tblCronogramas > tbody > tr").each(function (data) {
        var hora = $("#Cronogramas_" + index + "__HorasCronograma").val();
        if (hora !== undefined) {
            if (hora > Number($(CargaHorariaDigitada).val())) {
                resultado = true;
                return false;
            }
        }
        index++;
    });
    return resultado;
}
$(document).ready(function () {
    //jQuery("input.celular")
    //    .mask("(99) 99999-9999")
    //    .focusout(function (event) {
    //        var target, phone, element;
    //        target = (event.currentTarget) ? event.currentTarget : event.srcElement;
    //        phone = target.value.replace(/\D/g, '');
    //        element = $(target);
    //        element.unmask();
    //        if (phone.length > 10) {
    //            element.mask("(99) 99999-999?9");
    //        } else {
    //            element.mask("(99) 99999-9999");
    //        }
    //    });
    //jQuery("input.telefone")
    //    .mask("(99) 9999-9999")
    //    .focusout(function (event) {
    //        var target, phone, element;
    //        target = (event.currentTarget) ? event.currentTarget : event.srcElement;
    //        phone = target.value.replace(/\D/g, '');
    //        element = $(target);
    //        element.unmask();
    //        if (phone.length > 10) {
    //            element.mask("(99) 9999-999?9");
    //        } else {
    //            element.mask("(99) 9999-9999");
    //        }
    //    });
    $("#Nome").autocomplete({
        source: function (request, response) {
            ExecutaJson("Pacto", "ListarNomesPorUnidade", {}, "get")
                .done(function (data) {
                    response($.map($.parseJSON(data), function (val, item) {
                        if (retira_acentos(val.Nome).toLowerCase().indexOf(retira_acentos(request.term).toLowerCase()) !== -1)
                            return {
                                label: val.Nome,
                                value: val.Nome,
                                CPF: val.CPF,
                                Matricula: val.Matricula,
                                nomeUnidade: val.nomeUnidade,
                                Unidade: val.Unidade
                            }
                    }))
                });
        },
        minLength: 3,
        select: function (event, ui) {
            $("#CpfUsuario").val(ui.item.CPF);
            $("#lblMatriculaSIAPE").text(ui.item.Matricula);
            $("#lblUnidadeExercicio").text(ui.item.nomeUnidade);
            $("#MatriculaSIAPE").val(ui.item.Matricula);
            $("#UnidadeExercicio").val(ui.item.Unidade);
            VerificaPendencias(ui.item.CPF);
        }
    });

    $("#NomeServidor").autocomplete({
        source: function (request, response) {
            ExecutaJson("Pacto", "ListarTodosNomesDaBase", {}, "get")
                .done(function (data) {
                    response($.map($.parseJSON(data), function (val, item) {
                        if (retira_acentos(val.Nome).toLowerCase().indexOf(retira_acentos(request.term).toLowerCase()) !== -1)
                            return {
                                label: val.Nome,
                                value: val.Nome,
                                CPF: val.CPF,
                                Matricula: val.Matricula,
                                nomeUnidade: val.nomeUnidade,
                                Unidade: val.Unidade
                            }
                    }))
                });
        },
        minLength: 3,
        select: function (event, ui) {
            $("#CpfUsuario").val(ui.item.CPF);
            $("#lblMatriculaSIAPE").text(ui.item.Matricula);
            $("#lblUnidadeExercicio").text(ui.item.nomeUnidade);
            $("#MatriculaSIAPE").val(ui.item.Matricula);
            $("#UnidadeExercicio").val(ui.item.Unidade);
        }
    });

    $("#lnkAddRow").click(function () {
        var index = (ProdutosCount - 1);
        var dataAtual = new Date($.now());
        var datadt = new Date(Date.parse(toValidDate(dataAtual.toLocaleDateString())));
        if ($("#DataPrevistaInicio").val() != "") {
            //var dataDigitada = new Date($("#DataPrevistaInicio").val());
            var dataDigitada = new Date(Date.parse(toValidDate($("#DataPrevistaInicio").val())));
            /* CADE
             * Comentado para permitir inicio de plano de trabalho com data retroativa
            if (dataDigitada < datadt) {
                ShowErrorMessage("Data prevista de início do plano de trabalho não pode ser inferior a atual.");
                $("#Alertas").show();
                $("#Alertas").delay(8000).fadeOut(1600);
                $("#DataPrevistaInicio").focus();
                return false;
            }
            */
            if (!$("input[id='rdbtnyes']:checked").is(':checked') && !$("input[id='rdbtnno']:checked").is(':checked')) {
                var msg = '"Servidor tem redução de carga horária? "' + 'é obrigatório.';
                ShowErrorMessage(msg);
                $("#Alertas").show();
                $("#Alertas").delay(8000).fadeOut(1600);
                return false;
            }
            if ($("input[id='rdbtnno']:checked").is(':checked')) {
                $(CargaHorariaDigitada).val(8);
            }
            if ($("input[id='rdbtnyes']:checked").is(':checked') && ($(CargaHorariaDigitada).val() == 0 || $(CargaHorariaDigitada).val() >= 8)) {
                $("#CargaHoraria").focus();
                var msg = "A Carga Horária deve ser maior que 0 e menor que 8.";
                ShowErrorMessage(msg);
                $("#Alertas").show();
                $("#Alertas").delay(8000).fadeOut(1600);

            }
            else {
                if (CamposPreenchidos() && ProdutosCount === 0) {
                    //if (!EmAndamento) {
                    RenderPartial("Pacto", "AddProduto", {
                        idPacto: $("#IdPacto").val(),
                        idProduto: 0, idGrupo: $("#ddlGrupo").val(),
                        idAtividade: $("#ddlAtividade").val(),
                        idTipoAtividade: $("#ddlFaixa").val(),
                        quantidade: $("#QuantidadeProduto").val(),
                        cargahoraria: $("#CargaHorariaProduto").val(),
                        observacao: $("#lblObservacoes").val(),
                        cargahorariapacto: $(CargaHorariaDigitada).val(),
                        pCount: ProdutosCount
                    }, "get").done(function (success) {
                        var table = $('#tblProdutos > tbody:last-child');
                        table.html(success);
                        ZeraCombosProdutos();
                        RetornaCargaTotal();
                        ProdutosCount++;
                        console.log(ProdutosCount);
                        EdicaoDeProdutos();
                    });
                    //} else {
                    //    ShowErrorMessage("Não é possível adicionar produtos com plano de trabalho em andamento");
                    //}
                } else {
                    if (ProdutosCount >= 1 && CamposPreenchidos()) {
                        RenderPartial("Pacto", "AddProduto", {
                            idPacto: $("#IdPacto").val(),
                            idProduto: 0, idGrupo: $("#ddlGrupo").val(),
                            idAtividade: $("#ddlAtividade").val(),
                            idTipoAtividade: $("#ddlFaixa").val(),
                            quantidade: $("#QuantidadeProduto").val(),
                            cargahoraria: $("#CargaHorariaProduto").val(),
                            observacao: $("#lblObservacoes").val(),
                            cargahorariapacto: $(CargaHorariaDigitada).val(),
                            pCount: ProdutosCount
                        }, "get").done(function (success) {
                            var table = $('#tblProdutos > tbody:last-child');
                            table.append(success);
                            ZeraCombosProdutos();
                            RetornaCargaTotal();
                            ProdutosCount++;
                            console.log(ProdutosCount);
                            EdicaoDeProdutos();
                        });
                        //RenderPartial("Pacto", "AddProduto", {idProduto:$("#Produtos_"+ indexLinha +"__IdProduto").val(),
                        //    idGrupo: $("#ddlGrupo").val(),
                        //    idAtividade: $("#ddlAtividade").val(),
                        //    idTipoAtividade: $("#ddlFaixa").val(),
                        //    quantidade:$("#QuantidadeProduto").val(),
                        //    cargahoraria: $("#CargaHorariaProduto").val(),
                        //    observacao:$("#lblObservacoes").val(),
                        //    cargahorariapacto:$("#CargaHoraria").val(),
                        //    pCount: indexLinha }, "get").done(function (success) {
                        //        var table = $('#tblProdutos > tbody:last-child');
                        //        $("#rowProduto" + indexLinha).remove();
                        //        table.append(success);
                        //        ZeraCombosProdutos();
                        //        RetornaCargaTotal();
                        //        indexLinha = null;
                        //    });
                    } else {
                        var campos = "";
                        var qtd = 0;

                        if ($("#ddlGrupo").val() == '') {
                            campos += "Grupo de atividade, ";
                            qtd++;
                        }
                        if ($("#ddlAtividade").val() == '') {
                            campos += "Atividade, ";
                            qtd++;
                        }
                        if ($("#ddlFaixa").val() == '') {
                            campos += "Faixa, ";
                            qtd++;
                        }
                        if ($("#QuantidadeProduto").val() == '0' || $("#QuantidadeProduto").val() == '') {
                            campos += "Quantidade de produtos a serem entregues, ";
                            qtd++;
                        }
                        if (qtd == 1) {
                            ShowErrorMessage("O campo " + campos.replace(', ', '') + " é de preenchimeto obrigatório.");
                            $("#Alertas").show();
                            $("#Alertas").delay(8000).fadeOut(1600);
                        }
                        else {
                            ShowErrorMessage("Os campos " + campos + " são de preenchimeto obrigatório.");
                            $("#Alertas").show();
                            $("#Alertas").delay(8000).fadeOut(1600);

                        }
                    }
                };

            };
        }
        else {
            ShowErrorMessage('O campo Data Prevista de Início é de preenchimento obrigatório.');
            $("#Alertas").show();
            $("#Alertas").delay(8000).fadeOut(1600);
        }
    });
    function toValidDate(datestring) {
        return datestring.replace(/(\d{2})(\/)(\d{2})/, "$3$2$1");
    }
    var CamposPreenchidos = function () {
        return $("#ddlGrupo").val() !== '' && $("#ddlAtividade").val() !== '' && $("#ddlFaixa").val() !== '' && $("#QuantidadeProduto").val() !== '0' && $("#QuantidadeProduto").val() !== '';
    }
    var RetornaCargaTotal = function () {
        var indexTodo = ProdutosCount;
        var resultado = 0.0;
        var indexTotal = 0;
        var resultadoTotal = 0;
        $("#tblProdutos > tbody > tr").each(function (data) {
            if ($("#Produtos_" + indexTotal + "__CargaHorariaProduto").val() !== undefined && $("#Produtos_" + indexTotal + "__Excluir").val() !== "true") {
                resultadoTotal += Number($("#Produtos_" + indexTotal + "__CargaHorariaProduto").val().replace(',', '.'));
            }
            indexTotal++;
        });
        $("#lblCargaTotal").text(resultadoTotal.toFixed(2));
        $("#lblCargaTotal").val(resultadoTotal.toFixed(2));
        $("#lblCargaTotalCronograma").val(resultadoTotal.toFixed(2));
        $("#lblCargaTotalCronograma").text(resultadoTotal.toFixed(2));
        $("#CargaHorariaTotal").val(resultadoTotal.toFixed(2).replace('.', ','));
        $("#verAjustar").show();
        AtualizaDataPrevista();
    }
    var AtualizaDataPrevista = function () {
        ExecutaJson("Pacto", "AtualizaData", { datainicio: $("#DataPrevistaInicio").val(), cargahoraria: $("#lblCargaTotal").val(), cargahorariapacto: $(CargaHorariaDigitada).val(), editar: false, excluir: false, indice: 1 }, "post").
            done(function (data) {
                $("#lblDataPrevistaTermino").text(data);
                $("#DataPrevistaTermino").val(data);
            });

    }
    var ZeraCombosProdutos = function () {
        $("#ddlGrupo").val(''),
            $("#ddlAtividade").val(''),
            $("#ddlFaixa").val(''),
            $("#QuantidadeProduto").val('1'),
            $("#CargaHorariaProduto").val(''),
            $("#lblObservacoes").val('');
        $("#ddlGrupo").trigger('change');
        $("#lblObservacoes").focus();
        if (podeEditar && EmAndamento) {
            $("#Nome").attr('readonly', 'readonly');
            //$("#TelefoneFixoServidor").attr('readonly', 'readonly');
            //$("#TelefoneMovelServidor").attr('readonly', 'readonly');
            $("#PossuiCargaHoraria").attr('disabled', 'disabled');
            $("#rdbtnyes").attr('disabled', 'disabled');
            $("#rdbtnno").attr('disabled', 'disabled');
            $("#CargaHoraria").attr('readonly', 'readonly');
            $("#DataPrevistaInicio").prop('disabled', true);
            $("#ddlGrupo").attr("disabled", true);
            $("#ddlAtividade").attr("disabled", true);
            $("#ddlFaixa").attr("disabled", true);
            $("#QuantidadeProduto").attr('readonly', 'readonly');
            $("#lblObservacoes").attr('readonly', 'readonly');
        }
    }



    var AdicionaIdentificadores = function () {
        $("#IdGrupoAtividade").val($("#ddlGrupo").val());
        $("#IdAtividade").val($("#ddlAtividade").val());
        $("#IdFaixa").val($("#ddlFaixa").val());
        $("#CargaHorariaProduto").val($("#lblCargaHoraria").val());
    }
});

function VerificaPendencias(cpfSelecionado) {
    var url = "/Pacto/VerificaPendencias";
    $.ajax({
        type: 'POST',
        url: url,
        dataType: 'json',
        data: { cpfSelecionadoUsr: cpfSelecionado },
        success: function (data) {
            if (data) {
                ShowErrorMessage("Servidor não pode iniciar novo plano de trabalho se houver outro aberto nas situações: " + "Pendente de assinatura, " + "A iniciar, " + "Em andamento, " + "Pendente de avaliação" + " ou " + "Suspenso.");
                $("#Nome").val('');
                $("#CpfUsuario").val('');
                $("#lblMatriculaSIAPE").text('');
                $("#lblUnidadeExercicio").text('');
                $("#MatriculaSIAPE").val('');
                $("#UnidadeExercicio").val('');
            }
        },
        error: function (ex) {
        }
    });
}
function retira_acentos(palavra) {
    var com_acento = 'áàãâäéèêëíìîïóòõôöúùûüçÁÀÃÂÄÉÈÊËÍÌÎÏÓÒÕÖÔÚÙÛÜÇ';
    var sem_acento = 'aaaaaeeeeiiiiooooouuuucAAAAAEEEEIIIIOOOOOUUUUC';
    var nova = '';
    for (var i = 0; i < palavra.length; i++) {
        if (com_acento.search(palavra.substr(i, 1)) >= 0) {
            nova += sem_acento.substr(com_acento.search(palavra.substr(i, 1)), 1);
        }
        else {
            nova += palavra.substr(i, 1);
        }
    }
    return nova;
}
function shortString() {
    var shorts = document.querySelectorAll('.short');
    if (shorts) {
        Array.prototype.forEach.call(shorts, function (ele) {
            for (var i = 0; i < ele.length; i++) {
                var str = ele[i].innerText,
                    indt = '...';
                if (str !== "") {
                    if (ele[i].hasAttribute('data-limit')) {
                        if (str.length > ele[i].dataset.limit) {
                            var result = `${str.substring(0, ele[i].dataset.limit - indt.length).trim()}${indt}`;
                            ele[i].innerText = result;
                            str = null;
                            result = null;
                        }
                    }
                }
            }
        });
    }
}
function shortStringLabel() {
    var shorts = document.querySelectorAll('.shortLabel');
    if (shorts) {
        Array.prototype.forEach.call(shorts, function (ele) {
            var str = ele.innerText,
                indt = '...';
            ele.title = str;
            if (str !== "") {
                if (str.length > 30) {
                    var result = `${str.substring(0, 30 - indt.length).trim()}${indt}`;
                    ele.innerText = result;
                    str = null;
                    result = null;
                }
            }
        });
    }
}

// funcao para tirar o '<' e evitar o erro de request validation
// uso  <input type='text' id=txt onblur='retiraCaracterDangerousRequest(this)'/>
function retiraCaracterDangerousRequest(obj, replace = " ") {    
    while (obj.val().indexOf("<") !== -1) {
        obj.val(obj.val().replace("<", replace));
    }    
}




