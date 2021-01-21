$(document).ready(function () {
    $('.tableFilter').dataTable({
        "bFilter": false,
        "language": {
            "url": ROOT.concat("/", "Scripts/Portuguese-Brasil.json")
        }
    });
    $('.verDetalhes').click(function (e) {
        e.preventDefault();

        $(this).siblings('.lista-hide').slideToggle('fast')
    });
});